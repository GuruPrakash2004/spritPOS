// Crystal Reports bridge for the vLQMS web app.
//
// The web app can't lay out a .rpt file itself; only the Crystal Reports runtime can. This program does what the
// desktop vLQMS does (frmUtlReportViewer): load the .rpt, log every table on to SQL Server, set the record selection
// formula and the parameters, then export a PDF (preview) or print to a printer. It listens on 127.0.0.1 only.
//
//   GET  /health   -> {"version": "...", "printers": [...]}
//   POST /render   -> PDF bytes          body: the job (below)
//   POST /print    -> {"printed": true}  body: the job; prints `copies` copies on `printer`
//
// Job (JSON, sent by backend/app/rpt_bridge.py):
//   rpt, folder, selection, parameters{name:value}, copies, printer, stored_proc,
//   table_prefix ("" = the desktop's own tables; a prefix makes the report read <prefix><name>), logon{server, database, user, password, integrated}
// Every request needs the X-Bridge-Key header equal to the RPT_BRIDGE_KEY setting.
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.IO;
using System.Net;
using System.Diagnostics;
using System.Text;
using System.Web.Script.Serialization;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

static class Program
{
    const string Version = "1.0";
    static string Key;
    static readonly JavaScriptSerializer Json = new JavaScriptSerializer { MaxJsonLength = int.MaxValue };
    // Crystal's runtime is not thread-safe per process in practice: one report at a time, like the desktop.
    static readonly object Gate = new object();

    static int Main(string[] args)
    {
        string port = Environment.GetEnvironmentVariable("RPT_BRIDGE_PORT") ?? "8765";
        Key = Environment.GetEnvironmentVariable("RPT_BRIDGE_KEY") ?? "";
        if (args.Length > 0) port = args[0];
        if (args.Length > 1) Key = args[1];
        if (Key.Length == 0)
        {
            Console.Error.WriteLine("Set RPT_BRIDGE_KEY (or pass: RptBridge.exe <port> <key>). It must equal RPT_BRIDGE_KEY in backend/.env.");
            return 2;
        }
        var http = new HttpListener();
        http.Prefixes.Add("http://127.0.0.1:" + port + "/");   // this PC only
        http.Start();
        Console.WriteLine("vLQMS Crystal Reports bridge " + Version + " on http://127.0.0.1:" + port + "/");
        // The first call into Windows' printing system in a process can stall for ~50-60 s here (a printer that is offline or unreachable,
        // e.g. a network printer on another PC: measured with System.Drawing's PrinterSettings, every later call takes 0.2 s). Crystal makes
        // that call while it opens a report, so the first report of every new bridge hit the 55 s limit below, was killed, and the restart
        // paid the same again: a bridge that never became usable. The stall is paid HERE, once, in the background, before any report
        // is handled; /health says ready=false meanwhile and the 55 s limit counts only after it.
        var prewarm = new System.Threading.Thread(Prewarm) { IsBackground = true };
        prewarm.Start();
        while (true)
        {
            var ctx = http.GetContext();
            if (ctx.Request.HttpMethod == "GET" && ctx.Request.Url.AbsolutePath == "/health")
            {
                try { Health(ctx); } catch { }     // never waits on Crystal or the printers: the backend polls it
                continue;
            }
            var c = ctx;
            new System.Threading.Thread(() => Serve(c)) { IsBackground = true }.Start();
        }
    }

    static readonly System.Threading.ManualResetEvent Ready = new System.Threading.ManualResetEvent(false);
    static readonly System.Threading.SemaphoreSlim Slot = new System.Threading.SemaphoreSlim(1, 1);   // one report at a time, like the desktop
    static List<string> PrinterList = new List<string>();

    static void Prewarm()
    {
        var clock = Stopwatch.StartNew();
        try
        {
            var set = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            var list = new List<string>();
            foreach (string p in PrinterSettings.InstalledPrinters) { set.Add(p); list.Add(p); }
            try { var d = new PrinterSettings(); var ignore = d.PrinterName; var page = d.DefaultPageSettings; } catch { }   // the default printer's settings, as Crystal asks for them
            installedPrinters = set;
            PrinterList = list;
            Log("printing system ready in " + clock.ElapsedMilliseconds + " ms (" + list.Count + " printers)");
        }
        catch (Exception e) { Log("printing system check failed after " + clock.ElapsedMilliseconds + " ms: " + e.Message); }
        finally { Ready.Set(); }
    }

    static void Health(HttpListenerContext ctx)
    {
        if (ctx.Request.Headers["X-Bridge-Key"] != Key) { Fail(ctx, 401, "Wrong bridge key"); return; }
        bool ready = Ready.WaitOne(0);
        Send(ctx, 200, "application/json", Encoding.UTF8.GetBytes(Json.Serialize(new Dictionary<string, object> { { "version", Version }, { "ready", ready }, { "printers", ready ? PrinterList : new List<string>() } })));
    }

    // One request: wait until the printing system has been touched, take the single report slot, then run it on a worker. A report can
    // hang inside Crystal (a format that waits for a database logon dialog nobody can answer): past the limit the caller gets an answer
    // and the bridge exits, so the supervisor starts a clean one. Waiting for the slot or for start-up is not counted against the limit.
    static void Serve(HttpListenerContext ctx)
    {
        try
        {
            if (!Ready.WaitOne(TimeSpan.FromSeconds(240))) { Fail(ctx, 503, "The Crystal Reports bridge is still starting (Windows' printing system is slow to answer)."); return; }
            Slot.Wait();
            try
            {
                var worker = new System.Threading.Thread(() =>
                {
                    try { Handle(ctx); }
                    catch (Exception e) { try { Fail(ctx, 500, e.Message); } catch { } }
                }) { IsBackground = true };
                worker.Start();
                // A report whose own file is slow to open (RptZBItem.rpt takes ~45 s to load) may ask for longer in X-Bridge-Limit (seconds, up to 300).
                int limit = 55;
                int.TryParse(ctx.Request.Headers["X-Bridge-Limit"], out limit);
                if (limit < 55 || limit > 300) limit = 55;
                if (!worker.Join(TimeSpan.FromSeconds(limit)))
                {
                    Log("report did not finish in " + limit + " s: " + ctx.Request.Url.AbsolutePath + " - bridge exits");
                    try { Fail(ctx, 504, "Crystal Reports did not finish this report (it may be waiting for a database logon). The bridge is restarting."); } catch { }
                    System.Diagnostics.Process.GetCurrentProcess().Kill();   // Environment.Exit can itself hang on a stuck Crystal thread
                }
            }
            finally { Slot.Release(); }
        }
        catch (Exception e) { try { Fail(ctx, 500, e.Message); } catch { } }
    }

    static void Handle(HttpListenerContext ctx)
    {
        if (ctx.Request.Headers["X-Bridge-Key"] != Key) { Fail(ctx, 401, "Wrong bridge key"); return; }
        string path = ctx.Request.Url.AbsolutePath;
        if (path == "/health" && ctx.Request.HttpMethod == "GET")
        {
            Health(ctx);
            return;
        }
        if (path == "/warm" && ctx.Request.HttpMethod == "POST")
        {
            // Crystal's first Load in a process costs ~10 s (runtime start-up, JIT) and each report's first open ~1 s (Load + database logon).
            // The backend calls this once after the bridge starts with the reports the shop prints: each is opened, logged on and KEPT
            // (the same cache the prints use), so even the first print of a bill is served like the next ones.
            string wb;
            using (var r = new StreamReader(ctx.Request.InputStream, Encoding.UTF8)) wb = r.ReadToEnd();
            var wj = Json.Deserialize<Dictionary<string, object>>(wb);
            var clock = Stopwatch.StartNew();
            int kept = 0;
            lock (Gate)
            {
                if (wj.ContainsKey("files") && wj["files"] is System.Collections.ArrayList fl)
                    foreach (object f in fl)
                    {
                        string name = Convert.ToString(f);
                        string file = Path.Combine(Str(wj, "folder"), name);
                        if (name.IndexOfAny(new[] { (char)92, '/', ':' }) >= 0 || !File.Exists(file)) continue;
                        try
                        {
                            var one = new Dictionary<string, object>(wj);
                            one["rpt"] = name; one["selection"] = ""; one["stored_proc"] = "";
                            if (!wj.ContainsKey("table_prefix")) one["table_prefix"] = "";
                            bool again;
                            if (wj.ContainsKey("logon") && CacheOn) { Open(one, out again); kept++; }
                            else { var d = new ReportDocument(); try { d.Load(file); } finally { try { d.Dispose(); } catch { } } }
                        }
                        catch { try { Drop(new Dictionary<string, object>(wj) { { "rpt", name }, { "stored_proc", "" } }); } catch { } }
                    }
            }
            Send(ctx, 200, "application/json", Encoding.UTF8.GetBytes("{\"warm\":true,\"kept\":" + kept + ",\"ms\":" + clock.ElapsedMilliseconds + "}"));
            return;
        }
        if (path == "/describe" && ctx.Request.HttpMethod == "POST")
        {
            // A report's layout (sections and their objects with position, size and what they show), to see why a format prints as it does.
            string db;
            using (var r = new StreamReader(ctx.Request.InputStream, Encoding.UTF8)) db = r.ReadToEnd();
            var dj = Json.Deserialize<Dictionary<string, object>>(db);
            var sb = new StringBuilder();
            lock (Gate)
            {
                var d = new ReportDocument();
                try
                {
                    d.Load(Path.Combine(Str(dj, "folder"), Str(dj, "rpt")));
                    foreach (Section sec in d.ReportDefinition.Sections)
                    {
                        sb.AppendLine("== " + sec.Name + " (" + sec.Kind + ") height=" + sec.Height + " canGrow=" + sec.SectionFormat.EnableKeepTogether + "/" + sec.SectionFormat.EnableSuppress);
                        foreach (ReportObject o in sec.ReportObjects)
                        {
                            string what = "";
                            var t = o as TextObject; var f = o as FieldObject;
                            if (t != null) what = "text '" + (t.Text ?? "").Replace("\r", "\\r").Replace("\n", "\\n") + "'";
                            else if (f != null) { string ds = f.DataSource == null ? "" : f.DataSource.FormulaName; bool grow = false; try { grow = f.ObjectFormat.EnableCanGrow; } catch { } what = "field " + ds + " canGrow=" + grow; }
                            string font = "";
                            try { if (f != null) font = " font=" + f.Font.Name + " " + f.Font.Size + "pt"; else if (t != null) font = " font=" + t.Font.Name + " " + t.Font.Size + "pt"; } catch { }
                            sb.AppendLine("   " + o.Kind + " '" + o.Name + "' left=" + o.Left + " top=" + o.Top + " w=" + o.Width + " h=" + o.Height + " " + what + font);
                        }
                    }
                    foreach (FormulaFieldDefinition ff in d.DataDefinition.FormulaFields) sb.AppendLine("formula " + ff.Name + " = " + (ff.Text ?? "").Replace("\r", "").Replace("\n", " | "));
                    foreach (ParameterFieldDefinition pf in d.DataDefinition.ParameterFields) sb.AppendLine("parameter " + pf.Name);
                    foreach (Table tb in d.Database.Tables) sb.AppendLine("table " + tb.Name + " -> " + tb.Location);
                }
                finally { try { d.Dispose(); } catch { } }
            }
            Send(ctx, 200, "text/plain", Encoding.UTF8.GetBytes(sb.ToString()));
            return;
        }
        if (ctx.Request.HttpMethod != "POST" || (path != "/render" && path != "/print")) { Fail(ctx, 404, "Not found"); return; }
        string body;
        using (var r = new StreamReader(ctx.Request.InputStream, Encoding.UTF8)) body = r.ReadToEnd();
        var job = Json.Deserialize<Dictionary<string, object>>(body);
        lock (Gate)
        {
            var clock = Stopwatch.StartNew();
            var steps = new List<string>();
            bool reused;
            var rpt = Open(job, out reused);
            try
            {
                steps.Add("open=" + clock.ElapsedMilliseconds);
                long mark = clock.ElapsedMilliseconds;
                if (path == "/render")
                {
                    byte[] pdf;
                    try { pdf = ExportPdf(rpt); }
                    catch (Exception ex)
                    {
                        // "Selected exporter does not support one of the features used in this report": the PDF exporter can't embed an OpenType (CFF)
                        // font such as svtBarcode-Regular.otf. Retry once with TrueType stand-ins for those fonts (see SwapOpenTypeFonts).
                        if (ex.Message.IndexOf("does not support", StringComparison.OrdinalIgnoreCase) < 0 || !SwapOpenTypeFonts(rpt)) throw;
                        Log("PDF export failed (" + ex.Message.Trim().Replace("\r", " ").Replace("\n", " ") + "): retrying with TrueType stand-ins for OpenType fonts");
                        pdf = ExportPdf(rpt);
                    }
                    steps.Add("run=" + (clock.ElapsedMilliseconds - mark));
                    Send(ctx, 200, "application/pdf", pdf, Timing(steps, reused));
                }
                else
                {
                    string printer = Str(job, "printer");
                    if (printer.Length == 0) throw new Exception("No printer is set for this print option (Doc Designer → Print options → Printer).");
                    int copies = Math.Max(1, (int)Num(job, "copies"));
                    var ps = new PrinterSettings { PrinterName = printer, Copies = (short)copies };
                    if (!ps.IsValid) throw new Exception("Printer '" + printer + "' is not installed on this PC.");
                    var pd = new PageSettings();            // the desktop passes a default (custom) paper size for direct prints
                    rpt.PrintToPrinter(ps, pd, false);
                    steps.Add("run=" + (clock.ElapsedMilliseconds - mark));
                    Send(ctx, 200, "application/json", Encoding.UTF8.GetBytes("{\"printed\":true}"), Timing(steps, reused));
                }
            }
            catch
            {
                Drop(job);   // a report that failed part-way is not trusted again: the next job opens it fresh
                try { rpt.Dispose(); } catch { }
                throw;
            }
            finally
            {
                if (!Cached(job)) { try { rpt.Dispose(); } catch { } }
            }
        }
    }

    static byte[] ExportPdf(ReportDocument rpt)
    {
        using (var st = rpt.ExportToStream(ExportFormatType.PortableDocFormat))
        using (var ms = new MemoryStream())
        {
            st.CopyTo(ms);
            return ms.ToArray();
        }
    }

    // Font families installed from an OpenType (CFF) file (.otf): Windows lists them in the registry with the file path. Crystal's PDF exporter
    // can't embed those. Name -> file, both hives (a font installed for one user only is under HKCU, and the bridge runs as that user).
    static HashSet<string> OpenTypeFamilies()
    {
        var set = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        foreach (var hive in new[] { Microsoft.Win32.Registry.LocalMachine, Microsoft.Win32.Registry.CurrentUser })
        {
            try
            {
                using (var key = hive.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Fonts"))
                {
                    if (key == null) continue;
                    foreach (string name in key.GetValueNames())
                    {
                        string file = Convert.ToString(key.GetValue(name));
                        if (file == null || !file.EndsWith(".otf", StringComparison.OrdinalIgnoreCase)) continue;
                        string fam = System.Text.RegularExpressions.Regex.Replace(name, @"\s*\((TrueType|OpenType)\)\s*$", "");
                        fam = System.Text.RegularExpressions.Regex.Replace(fam, @"\s+(Regular|Bold|Italic|Bold Italic|Oblique)$", "", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                        set.Add(fam.Trim());
                    }
                }
            }
            catch { }
        }
        return set;
    }

    // Put TrueType stand-ins on the report's OpenType fonts: a barcode font becomes an installed TrueType Code 39 font (the labels' formula
    // writes the code between * characters, which is Code 39), any other becomes Arial. RPT_FONT_MAP="svtBarcode=Code39AzaleaRegular1;Other=Arial"
    // overrides the choice. Returns true when something was changed (so a retry is worth it).
    static bool SwapOpenTypeFonts(ReportDocument rpt)
    {
        var open = OpenTypeFamilies();
        if (open.Count == 0) return false;
        var installed = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        foreach (var f in System.Drawing.FontFamily.Families) if (!open.Contains(f.Name)) installed.Add(f.Name);
        var map = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        foreach (string pair in (Environment.GetEnvironmentVariable("RPT_FONT_MAP") ?? "").Split(';'))
        {
            int eq = pair.IndexOf('=');
            if (eq > 0) map[pair.Substring(0, eq).Trim()] = pair.Substring(eq + 1).Trim();
        }
        string code39 = null;
        foreach (string pick in new[] { "Code39AzaleaRegular1", "Free 3 of 9", "Free 3 of 9 Extended", "Code 39", "Code39" })
            if (installed.Contains(pick)) { code39 = pick; break; }
        bool changed = false;
        Action<ReportDocument> walk = null;
        walk = (doc) =>
        {
            foreach (Section sec in doc.ReportDefinition.Sections)
                foreach (ReportObject o in sec.ReportObjects)
                {
                    System.Drawing.Font font = null;
                    var f = o as FieldObject; var t = o as TextObject;
                    try { font = f != null ? f.Font : t != null ? t.Font : null; } catch { }
                    if (font == null || !open.Contains(font.Name)) continue;
                    string to;
                    if (!map.TryGetValue(font.Name, out to))
                        to = font.Name.IndexOf("barcode", StringComparison.OrdinalIgnoreCase) >= 0 || font.Name.IndexOf("code", StringComparison.OrdinalIgnoreCase) >= 0 ? code39 : "Arial";
                    if (string.IsNullOrEmpty(to) || !installed.Contains(to)) to = "Arial";
                    // The Code 39 stand-in has taller bars than the original font: a little smaller so it stays above the text under it.
                    var swapped = new System.Drawing.Font(to, string.Equals(to, code39, StringComparison.OrdinalIgnoreCase) ? font.Size * 0.85f : font.Size, font.Style);
                    if (f != null) f.ApplyFont(swapped); else t.ApplyFont(swapped);
                    Log("font " + font.Name + " -> " + to + " on '" + o.Name + "'");
                    changed = true;
                }
        };
        walk(rpt);
        foreach (ReportDocument sub in rpt.Subreports) walk(sub);
        return changed;
    }

    static string Timing(List<string> steps, bool reused) => (reused ? "reused;" : "loaded;") + string.Join(";", steps.ToArray()) + ";ms";

    // Loaded reports are kept (a few of them) and used again: opening a .rpt (Load) is the slowest part of a print, and the desktop
    // keeps its report open too. A kept report is brought back to how the file was saved before each job: the shop-label texts that
    // FillLabels added are put back, the selection formula is replaced (or cleared) and the parameters are set again, and the data
    // is read again. RPT_BRIDGE_CACHE=0 turns this off (every job loads the file, as before).
    class Kept { public string LogonSig = ""; public ReportDocument Doc; public DateTime Stamp; public long Used; public List<KeyValuePair<TextObject, string>> Texts = new List<KeyValuePair<TextObject, string>>(); public Dictionary<ReportObject, int> Tops; public Dictionary<ReportObject, int> ObjHeights; public Dictionary<ReportObject, int> LineBottoms; public Dictionary<Section, int> Heights; }
    static readonly Dictionary<string, Kept> Reports = new Dictionary<string, Kept>(StringComparer.OrdinalIgnoreCase);
    static long Tick;
    const int KeepReports = 12;
    static readonly bool CacheOn = Environment.GetEnvironmentVariable("RPT_BRIDGE_CACHE") != "0";

    static string KeyOf(Dictionary<string, object> job) => Path.Combine(Str(job, "folder"), Str(job, "rpt")) + "|" + Str(job, "stored_proc") + "|" + Str(job, "table_prefix");
    static bool Cached(Dictionary<string, object> job) => CacheOn && Reports.ContainsKey(KeyOf(job));
    static void Drop(Dictionary<string, object> job)
    {
        Kept k;
        string key = KeyOf(job);
        if (Reports.TryGetValue(key, out k)) { Reports.Remove(key); try { k.Doc.Dispose(); } catch { } }
    }

    static HashSet<string> installedPrinters;
    static bool Installed(string name)
    {
        if (installedPrinters == null)
        {
            var set = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (string p in PrinterSettings.InstalledPrinters) set.Add(p);
            installedPrinters = set;
        }
        return installedPrinters.Contains(name);
    }

    // The desktop's ShowRptFileFromUtlOptions: load, apply logon to every table (and subreport), selection, parameters.
    static ReportDocument Open(Dictionary<string, object> job, out bool reused)
    {
        reused = false;
        if (!CacheOn) return Fresh(job, null);
        string key = KeyOf(job);
        string file = Path.Combine(Str(job, "folder"), Str(job, "rpt"));
        Kept kept;
        if (Reports.TryGetValue(key, out kept) && File.Exists(file) && File.GetLastWriteTimeUtc(file) == kept.Stamp)
        {
            try
            {
                foreach (var t in kept.Texts) t.Key.Text = t.Value;   // the report as saved
                kept.Texts.Clear();
                kept.Used = ++Tick;
                Configure(kept.Doc, job, kept);
                kept.Doc.Refresh();                                   // read the data again for this document
                // Refresh() clears the parameter values (the next export then fails with "Missing parameter values"): set them again.
                if (job.ContainsKey("parameters") && job["parameters"] is Dictionary<string, object> again)
                    foreach (var kv in again) SetValue(kept.Doc, kv.Key, kv.Value);
                reused = true;
                return kept.Doc;
            }
            catch { Drop(job); }                                      // anything odd: open it fresh below
        }
        while (Reports.Count >= KeepReports)
        {
            string oldest = null; long low = long.MaxValue;
            foreach (var kv in Reports) if (kv.Value.Used < low) { low = kv.Value.Used; oldest = kv.Key; }
            try { Reports[oldest].Doc.Dispose(); } catch { }
            Reports.Remove(oldest);
        }
        var entry = new Kept { Stamp = File.Exists(file) ? File.GetLastWriteTimeUtc(file) : DateTime.MinValue, Used = ++Tick };
        var doc = Fresh(job, entry);
        entry.Doc = doc;
        Reports[key] = entry;
        return doc;
    }

    // Load the .rpt and set it up for this job (the first time a report is used, or with the cache off).
    static ReportDocument Fresh(Dictionary<string, object> job, Kept keep)
    {
        string folder = Str(job, "folder"), name = Str(job, "rpt");
        if (name.IndexOfAny(new[] { '\\', '/', ':' }) >= 0) throw new Exception("Bad report name");
        string file = Path.Combine(folder, name);
        if (!File.Exists(file)) throw new Exception("Please ensure " + file + " exists!");
        var rpt = new ReportDocument();
        var clock = Stopwatch.StartNew();
        try
        {
            Log("load start " + name);
            rpt.Load(file);
            string saved = "";
            try { saved = rpt.PrintOptions.PrinterName; } catch { }
            Log("load done " + name + " in " + clock.ElapsedMilliseconds + " ms, printer saved in the file: '" + saved + "'");
            Configure(rpt, job, keep);
            Log("configured " + name + " in " + clock.ElapsedMilliseconds + " ms");
            return rpt;
        }
        catch (Exception e) { Log("FAILED " + name + " after " + clock.ElapsedMilliseconds + " ms: " + e.Message); rpt.Dispose(); throw; }
    }

    // A line per stage in backend\data\bridge.log (next to the exe's backend folder), so a report that hangs shows where it stopped:
    // "load start X" without "load done X" = Crystal is stuck opening the file (often the printer saved in it), not in the database.
    static readonly object LogGate = new object();
    static string LogFile;
    static void Log(string line)
    {
        try
        {
            lock (LogGate)
            {
                if (LogFile == null)
                {
                    string dir = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "data"));
                    if (!Directory.Exists(dir)) dir = AppDomain.CurrentDomain.BaseDirectory;
                    LogFile = Path.Combine(dir, "bridge.log");
                }
                File.AppendAllText(LogFile, DateTime.Now.ToString("HH:mm:ss.fff") + " [" + Process.GetCurrentProcess().Id + "] " + line + Environment.NewLine);
            }
        }
        catch { /* logging must never break a print */ }
    }

    // Logon, selection formula, parameters and the shop's label texts for this job (a freshly loaded report or a kept one).
    static string LogonSig(Dictionary<string, object> job)
    {
        var logon = (Dictionary<string, object>)job["logon"];
        return string.Join("|", new[] { Str(logon, "server"), Str(logon, "database"), Str(logon, "user"), Str(logon, "password"), Str(logon, "integrated"), Str(job, "table_prefix"), Str(job, "stored_proc") });
    }

    static void Configure(ReportDocument rpt, Dictionary<string, object> job, Kept keep)
    {
        var logon = (Dictionary<string, object>)job["logon"];
        string prefix = Str(job, "table_prefix");
        string sp = Str(job, "stored_proc");
        // A kept report already has this logon on every table (and subreport): applying it again on each job costs ~50 ms for nothing.
        string sig = LogonSig(job);
        if (keep == null || keep.LogonSig != sig)
        {
            Apply(rpt, logon, prefix, sp, Str(logon, "database"));
            foreach (ReportDocument sub in rpt.Subreports) Apply(sub, logon, prefix, sp, Str(logon, "database"));
            if (keep != null) keep.LogonSig = sig;
        }

        // Crystal asks the printer driver for text metrics while it lays pages out, and it contacts that printer again about every 60 s.
        // When the Windows default is a network (WSD) printer, that contact costs ~8.5 s on the next preview/export. The backend sends
        // "layout_printer" (a local driver such as Microsoft Print to PDF) for previews and exports only; it is used only when installed.
        // Direct prints are unaffected: PrintToPrinter gets its own printer settings.
        string layoutPrinter = Str(job, "layout_printer");
        if (layoutPrinter.Length > 0 && Installed(layoutPrinter)) { try { rpt.PrintOptions.PrinterName = layoutPrinter; } catch { } }

        string selection = Str(job, "selection");
        if (selection.Length > 0 || keep != null) rpt.RecordSelectionFormula = selection;   // a kept report must not keep the last job's selection
        if (job.ContainsKey("parameters") && job["parameters"] is Dictionary<string, object> ps)
            foreach (var kv in ps) SetValue(rpt, kv.Key, kv.Value);
        if (job.ContainsKey("label_values") && job["label_values"] is Dictionary<string, object> lv)
        {
            var addr = new List<string>();
            if (job.ContainsKey("address_lines") && job["address_lines"] is System.Collections.ArrayList al)
                foreach (object a in al) if (!string.IsNullOrWhiteSpace(Convert.ToString(a))) addr.Add(Convert.ToString(a).Trim());
            FillLabels(rpt, lv, addr, keep);
            foreach (ReportDocument sub in rpt.Subreports) FillLabels(sub, lv, new List<string>(), keep);
        }
        MakeRoom(rpt, job.ContainsKey("parameters") ? job["parameters"] as Dictionary<string, object> : null, keep);
        if (Num(job, "columns") == 1) StackColumns(rpt);
    }

    /// <summary>Formats differ from client to client, so nothing here depends on one file's object names.
    /// 1. A text object that holds only a label such as "PH NO:", "Email ID:", "GSTIN No:" (compared without case, colon and
    ///    spaces) gets the shop's value after it, in the report's own font and alignment. `values` maps a label to its value.
    /// 2. The address: the desktop leaves a wide, blank (spaces only) text object between the logo and those labels and fills
    ///    it when it prints. A blank wide text object in a section above the first filled label is the address: the shop's
    ///    address lines go in (one line per blank object, or all lines in the only one). Without a filled label nothing is
    ///    treated as an address, so other reports are left alone.
    /// It runs when the report is opened, so a preview, an export and a direct print all have it.</summary>
    static void FillLabels(ReportDocument doc, Dictionary<string, object> values, List<string> address, Kept keep)
    {
        int index = 0, firstLabel = -1;
        var blanks = new List<KeyValuePair<int, TextObject>>();
        foreach (Section sec in doc.ReportDefinition.Sections)
        {
            foreach (ReportObject o in sec.ReportObjects)
            {
                var t = o as TextObject;
                if (t == null) continue;
                string raw = t.Text ?? "";
                string key = raw.Trim().TrimEnd(':').Trim().ToLowerInvariant();
                if (key.Length > 0 && values.ContainsKey(key))
                {
                    string v = Convert.ToString(values[key]);
                    if (!string.IsNullOrWhiteSpace(v)) { if (keep != null) keep.Texts.Add(new KeyValuePair<TextObject, string>(t, raw)); t.Text = raw.TrimEnd() + " " + v.Trim(); if (firstLabel < 0) firstLabel = index; }
                }
                else if (raw.Length > 0 && raw.Trim().Length == 0 && t.Width >= 2500) blanks.Add(new KeyValuePair<int, TextObject>(index, t));
            }
            index++;
        }
        if (firstLabel < 0 || address.Count == 0) return;
        var slots = blanks.FindAll(kv => kv.Key < firstLabel);
        if (slots.Count == 0) return;
        if (keep != null) foreach (var sl in slots) keep.Texts.Add(new KeyValuePair<TextObject, string>(sl.Value, sl.Value.Text));
        if (slots.Count == 1) slots[0].Value.Text = string.Join("\r\n", address.ToArray());
        else for (int n = 0; n < slots.Count && n < address.Count; n++) slots[n].Value.Text = address[n];
    }

    // Move an object to a new Top. A line can't be changed one coordinate at a time (Crystal: "Only vertical or horizontal lines are supported"):
    // a horizontal line is first made vertical (Right = Left), then moved as Top/Bottom, then made horizontal again. `keep` (when given) holds a kept
    // report's saved line bottoms, used when a line is put back.
    static void MoveTo(ReportObject o, int top, Kept keep)
    {
        var ln = o as LineObject;
        if (ln == null) { o.Top = top; return; }
        int height = ln.Bottom - ln.Top;
        if (keep != null && keep.LineBottoms != null && keep.Tops != null && keep.Tops.ContainsKey(o) && keep.LineBottoms.ContainsKey(o)) height = keep.LineBottoms[o] - keep.Tops[o];
        int bottom = top + height;
        if (ln.Top == top && ln.Bottom == bottom) return;
        int left = ln.Left, right = ln.Right;
        if (ln.Top == ln.Bottom && left != right) { ln.Right = left; ln.Top = top; ln.Bottom = bottom; ln.Right = right; }   // horizontal, via a vertical state
        else if (bottom >= ln.Bottom) { ln.Bottom = bottom; ln.Top = top; }                                                     // vertical, moving down
        else { ln.Top = top; ln.Bottom = bottom; }                                                                             // vertical, moving up
    }

    // "columns": 1 in a job = one column. A label format (RPTBC.rpt) is a multi-column sheet: labels run left to right, several across, and
    // only then down. The printable width is narrowed to the Detail section's width plus a little, so just one column fits and the labels
    // stack top to bottom, one under another. The format file itself is not touched.
    static void StackColumns(ReportDocument rpt)
    {
        try
        {
            int detail = 0;
            foreach (Section sec in rpt.ReportDefinition.Sections)
                if (sec.Kind == AreaSectionKind.Detail)
                    foreach (ReportObject o in sec.ReportObjects) detail = Math.Max(detail, o.Left + o.Width);
            if (detail <= 0) return;
            var po = rpt.PrintOptions;
            var m = po.PageMargins;
            int pageWidth = po.PageContentWidth + m.leftMargin + m.rightMargin;
            int want = detail + 300;                                   // one column wide; a second would need about twice this
            int right = pageWidth - m.leftMargin - want;
            if (right <= m.rightMargin) return;                        // already narrower than that
            m.rightMargin = right;
            po.ApplyPageMargins(m);
            Log("stacked labels: printable width " + want + " twips (Detail width " + detail + ")");
        }
        catch (Exception e) { Log("StackColumns skipped: " + e.Message); }
    }

    /// <summary>A text object whose text holds parameter placeholders (the desktop's address block is one object, "{?Add1} {?Add2} {?Add3} {?Add4}",
    /// one line tall, with the "PH NO:" line a fixed distance below it) wraps over more lines when the shop's address is long: the extra lines
    /// were printed on top of the objects below it. The wrapped height is measured with the object's own font and width, the objects under
    /// it in the same section are moved down by the missing height and the section grows by it (every section below follows). A kept
    /// report is put back to the positions it was saved with before each job. Nothing changes when the text fits.</summary>
    static void MakeRoom(ReportDocument rpt, Dictionary<string, object> ps, Kept keep)
    {
        try
        {
            if (keep != null)
            {
                if (keep.Tops == null)
                {
                    keep.Tops = new Dictionary<ReportObject, int>(); keep.ObjHeights = new Dictionary<ReportObject, int>(); keep.LineBottoms = new Dictionary<ReportObject, int>(); keep.Heights = new Dictionary<Section, int>();
                    foreach (Section s in rpt.ReportDefinition.Sections) { keep.Heights[s] = s.Height; foreach (ReportObject o in s.ReportObjects) { keep.Tops[o] = o.Top; keep.ObjHeights[o] = o.Height; var ln = o as LineObject; if (ln != null) keep.LineBottoms[o] = ln.Bottom; } }
                }
                else
                {
                    foreach (var kv in keep.ObjHeights) { try { kv.Key.Height = kv.Value; } catch { } }
                    foreach (var kv in keep.Tops) MoveTo(kv.Key, kv.Value, keep);          // objects first: a section can't be made shorter than what is in it
                    foreach (var kv in keep.Heights) kv.Key.Height = kv.Value;
                }
            }
            if (ps == null) return;
            var values = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            foreach (var kv in ps) values[kv.Key.TrimStart('@')] = Convert.ToString(kv.Value);
            using (var bmp = new System.Drawing.Bitmap(1, 1))
            using (var g = System.Drawing.Graphics.FromImage(bmp))
            {
                g.PageUnit = System.Drawing.GraphicsUnit.Pixel;
                foreach (Section sec in rpt.ReportDefinition.Sections)
                {
                    var objects = new List<ReportObject>();
                    foreach (ReportObject o in sec.ReportObjects) objects.Add(o);
                    foreach (ReportObject o in objects)
                    {
                        var t = o as TextObject;
                        if (t == null || t.Text == null || t.Text.IndexOf("{?", StringComparison.Ordinal) < 0) continue;
                        string shown = System.Text.RegularExpressions.Regex.Replace(t.Text, @"\{\?(\w+)\}", m => { string v; return values.TryGetValue(m.Groups[1].Value, out v) ? v : ""; });
                        float room = t.Width / 15f * 0.97f;                       // twips -> pixels at 96 dpi, a little margin for padding
                        int lines = 0;
                        foreach (string para in shown.Replace("\r", "").Split('\n'))
                        {
                            var line = "";
                            lines++;
                            foreach (string w in para.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries))
                            {
                                string next = line.Length == 0 ? w : line + " " + w;
                                if (line.Length > 0 && g.MeasureString(next, t.Font, 100000, System.Drawing.StringFormat.GenericTypographic).Width > room) { lines++; line = w; }
                                else line = next;
                            }
                        }
                        int needed = (int)Math.Ceiling(lines * t.Font.GetHeight(g) * 15f);
                        int delta = needed - t.Height;
                        if (delta <= 100 || lines < 2) continue;   // a one-line text already has its room; only extra wrapped lines need it
                        int bottom = t.Top + t.Height, h0 = sec.Height;
                        foreach (ReportObject below in objects) if (below != t && below.Top >= bottom - 10) MoveTo(below, below.Top + delta, null);
                        sec.Height = h0 + delta;   // moving objects down already stretches the section; this makes it exactly the old height plus the added room
                        try { t.Height = needed; } catch { /* the text object grows by itself */ }
                        var back = new StringBuilder();
                        foreach (Section s2 in rpt.ReportDefinition.Sections) if (s2.Name == sec.Name) foreach (ReportObject o2 in s2.ReportObjects) back.Append(o2.Name + "@" + o2.Top + "/" + o2.Height + " ");
                        Log("made room for " + (lines) + " lines in '" + t.Name + "': +" + delta + " twips; section now " + sec.Height + "; read back: " + back);
                    }
                }
            }
        }
        catch (Exception e) { Log("MakeRoom skipped: " + e.Message); }
    }

    /// <summary>Give the report one of the print option's values (Doc Designer FormulasFld/FormulasVal). The desktop sets
    /// the ones the report has: a parameter field, or else a formula field. A name the report does not define is skipped:
    /// asking Crystal for it fails with "Invalid index" (DISP_E_BADINDEX).</summary>
    static void SetValue(ReportDocument rpt, string name, object value)
    {
        string bare = name.TrimStart('@');
        foreach (ParameterFieldDefinition p in rpt.DataDefinition.ParameterFields)
            if (string.Equals(p.Name.TrimStart('@'), bare, StringComparison.OrdinalIgnoreCase))
            {
                rpt.SetParameterValue(p.Name, value);
                return;
            }
        foreach (FormulaFieldDefinition f in rpt.DataDefinition.FormulaFields)
            if (string.Equals(f.Name, bare, StringComparison.OrdinalIgnoreCase))
            {
                bool text = value == null || value is string;
                f.Text = text ? "'" + (value ?? "").ToString().Replace("'", "''") + "'" : Convert.ToString(value, System.Globalization.CultureInfo.InvariantCulture);
                return;
            }
    }

    static void Apply(ReportDocument doc, Dictionary<string, object> logon, string prefix, string storedProc, string database)
    {
        var info = new TableLogOnInfo();
        info.ConnectionInfo.ServerName = Str(logon, "server");
        info.ConnectionInfo.DatabaseName = database;
        if (logon.ContainsKey("integrated") && logon["integrated"] is bool b && b) info.ConnectionInfo.IntegratedSecurity = true;
        else { info.ConnectionInfo.UserID = Str(logon, "user"); info.ConnectionInfo.Password = Str(logon, "password"); }
        foreach (Table t in doc.Database.Tables)
        {
            t.ApplyLogOnInfo(info);
            if (storedProc.Length > 0) { t.Location = database + ".." + storedProc + ";1"; continue; }   // showSPRpt
            // The report was made on the desktop's tables, which also hold the web's bills; a prefix is only for a custom setup.
            if (prefix.Length > 0)
            {
                string loc = t.Location;
                int dot = loc.LastIndexOf('.');
                string head = dot >= 0 ? loc.Substring(0, dot + 1) : "", tbl = dot >= 0 ? loc.Substring(dot + 1) : loc;
                if (!tbl.StartsWith(prefix, StringComparison.OrdinalIgnoreCase)) t.Location = head + prefix + tbl;
            }
        }
    }

    static string Str(Dictionary<string, object> d, string k) => d.ContainsKey(k) && d[k] != null ? Convert.ToString(d[k]) : "";
    static double Num(Dictionary<string, object> d, string k) => d.ContainsKey(k) && d[k] != null ? Convert.ToDouble(d[k]) : 0;

    static void Send(HttpListenerContext ctx, int code, string type, byte[] data, string timing = null)
    {
        if (timing != null) ctx.Response.AddHeader("X-Bridge-Timing", timing);
        ctx.Response.StatusCode = code;
        ctx.Response.ContentType = type;
        ctx.Response.ContentLength64 = data.Length;
        ctx.Response.OutputStream.Write(data, 0, data.Length);
        ctx.Response.Close();
    }

    static void Fail(HttpListenerContext ctx, int code, string message)
    {
        try { Send(ctx, code, "application/json", Encoding.UTF8.GetBytes(Json.Serialize(new Dictionary<string, object> { { "error", message } }))); }
        catch { /* the client went away */ }
    }
}
