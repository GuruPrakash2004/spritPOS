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
        while (true)
        {
            var ctx = http.GetContext();
            // A report can hang inside Crystal (a format that waits for a database logon dialog nobody can answer).
            // The request runs on a worker; past the limit the caller gets an answer and the bridge exits, so the
            // start script (run_bridge.cmd) starts a clean one. Nothing else can use a stuck Crystal engine anyway.
            var worker = new System.Threading.Thread(() =>
            {
                try { Handle(ctx); }
                catch (Exception e) { try { Fail(ctx, 500, e.Message); } catch { } }
            });
            worker.IsBackground = true;
            worker.Start();
            if (!worker.Join(TimeSpan.FromSeconds(55)))
            {
                try { Fail(ctx, 504, "Crystal Reports did not finish this report (it may be waiting for a database logon). The bridge is restarting."); } catch { }
                System.Diagnostics.Process.GetCurrentProcess().Kill();   // Environment.Exit can itself hang on a stuck Crystal thread
            }
        }
    }

    static void Handle(HttpListenerContext ctx)
    {
        if (ctx.Request.Headers["X-Bridge-Key"] != Key) { Fail(ctx, 401, "Wrong bridge key"); return; }
        string path = ctx.Request.Url.AbsolutePath;
        if (path == "/health" && ctx.Request.HttpMethod == "GET")
        {
            var printers = new List<string>();
            foreach (string p in PrinterSettings.InstalledPrinters) printers.Add(p);
            Send(ctx, 200, "application/json", Encoding.UTF8.GetBytes(Json.Serialize(new Dictionary<string, object> { { "version", Version }, { "printers", printers } })));
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
                    using (var s = rpt.ExportToStream(ExportFormatType.PortableDocFormat))
                    using (var ms = new MemoryStream())
                    {
                        s.CopyTo(ms);
                        steps.Add("run=" + (clock.ElapsedMilliseconds - mark));
                        Send(ctx, 200, "application/pdf", ms.ToArray(), Timing(steps, reused));
                    }
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

    static string Timing(List<string> steps, bool reused) => (reused ? "reused;" : "loaded;") + string.Join(";", steps.ToArray()) + ";ms";

    // Loaded reports are kept (a few of them) and used again: opening a .rpt (Load) is the slowest part of a print, and the desktop
    // keeps its report open too. A kept report is brought back to how the file was saved before each job: the shop-label texts that
    // FillLabels added are put back, the selection formula is replaced (or cleared) and the parameters are set again, and the data
    // is read again. RPT_BRIDGE_CACHE=0 turns this off (every job loads the file, as before).
    class Kept { public ReportDocument Doc; public DateTime Stamp; public long Used; public List<KeyValuePair<TextObject, string>> Texts = new List<KeyValuePair<TextObject, string>>(); }
    static readonly Dictionary<string, Kept> Reports = new Dictionary<string, Kept>(StringComparer.OrdinalIgnoreCase);
    static long Tick;
    const int KeepReports = 6;
    static readonly bool CacheOn = Environment.GetEnvironmentVariable("RPT_BRIDGE_CACHE") != "0";

    static string KeyOf(Dictionary<string, object> job) => Path.Combine(Str(job, "folder"), Str(job, "rpt")) + "|" + Str(job, "stored_proc") + "|" + Str(job, "table_prefix");
    static bool Cached(Dictionary<string, object> job) => CacheOn && Reports.ContainsKey(KeyOf(job));
    static void Drop(Dictionary<string, object> job)
    {
        Kept k;
        string key = KeyOf(job);
        if (Reports.TryGetValue(key, out k)) { Reports.Remove(key); try { k.Doc.Dispose(); } catch { } }
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
        try
        {
            rpt.Load(file);
            Configure(rpt, job, keep);
            return rpt;
        }
        catch { rpt.Dispose(); throw; }
    }

    // Logon, selection formula, parameters and the shop's label texts for this job (a freshly loaded report or a kept one).
    static void Configure(ReportDocument rpt, Dictionary<string, object> job, Kept keep)
    {
        var logon = (Dictionary<string, object>)job["logon"];
        string prefix = Str(job, "table_prefix");
        string sp = Str(job, "stored_proc");
        Apply(rpt, logon, prefix, sp, Str(logon, "database"));
        foreach (ReportDocument sub in rpt.Subreports) Apply(sub, logon, prefix, sp, Str(logon, "database"));

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
