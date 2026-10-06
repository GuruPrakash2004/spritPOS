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
            try { Handle(ctx); }
            catch (Exception e) { Fail(ctx, 500, e.Message); }
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
            using (var rpt = Open(job))
            {
                if (path == "/render")
                {
                    using (var s = rpt.ExportToStream(ExportFormatType.PortableDocFormat))
                    using (var ms = new MemoryStream())
                    {
                        s.CopyTo(ms);
                        Send(ctx, 200, "application/pdf", ms.ToArray());
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
                    Send(ctx, 200, "application/json", Encoding.UTF8.GetBytes("{\"printed\":true}"));
                }
            }
        }
    }

    // The desktop's ShowRptFileFromUtlOptions: load, apply logon to every table (and subreport), selection, parameters.
    static ReportDocument Open(Dictionary<string, object> job)
    {
        string folder = Str(job, "folder"), name = Str(job, "rpt");
        if (name.IndexOfAny(new[] { '\\', '/', ':' }) >= 0) throw new Exception("Bad report name");
        string file = Path.Combine(folder, name);
        if (!File.Exists(file)) throw new Exception("Please ensure " + file + " exists!");
        var rpt = new ReportDocument();
        try
        {
            rpt.Load(file);
            var logon = (Dictionary<string, object>)job["logon"];
            string prefix = Str(job, "table_prefix");
            string sp = Str(job, "stored_proc");
            Apply(rpt, logon, prefix, sp, Str(logon, "database"));
            foreach (ReportDocument sub in rpt.Subreports) Apply(sub, logon, prefix, sp, Str(logon, "database"));

            string selection = Str(job, "selection");
            if (selection.Length > 0) rpt.RecordSelectionFormula = selection;
            if (job.ContainsKey("parameters") && job["parameters"] is Dictionary<string, object> ps)
                foreach (var kv in ps) SetValue(rpt, kv.Key, kv.Value);
            return rpt;
        }
        catch { rpt.Dispose(); throw; }
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

    static void Send(HttpListenerContext ctx, int code, string type, byte[] data)
    {
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
