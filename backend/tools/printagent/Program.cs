// vLQMS Print Agent: a tiny program that runs on each COUNTER PC and lets the web app print silently to that PC's own
// printers (USB or network), including the PC's default printer.
//
// Why: a web page may not list or use the printers of the PC it is open on. This agent runs on that PC, listens on
// 127.0.0.1 only (nothing outside this PC can reach it) and the web app, open in a browser on the same PC, talks to it.
//
//   GET  /health   -> {"agent":"vlqms-print-agent","version":"1.0","host":"PC-NAME","default":"POS-80","printers":["POS-80", ...]}
//   POST /print    {"printer":"POS-80","copies":1,"name":"26/SI-0000012","pages":[{"png":"<base64>","w":226.8,"h":600.0}, ...]}
//                  w / h are the page size in points (1/72 inch). Each page is a PNG the server drew from the report.
//                                                                -> {"printed":true,"printer":"POS-80","copies":1}
//
// Which web pages may use it: only the addresses listed in printagent.txt next to this exe (one per line, e.g.
// http://192.168.1.10 or https://shop.example.com; a line with just * allows any page). Without that file nothing is allowed.
// Build with build.cmd (needs no SDK: the .NET Framework that comes with Windows), install with install.cmd.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Web.Script.Serialization;

static class Agent
{
    const string Version = "1.0";
    const int MaxBody = 64 * 1024 * 1024;
    static int Port = 8766;
    static readonly HashSet<string> Origins = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
    static bool AnyOrigin;
    static readonly string Dir = AppDomain.CurrentDomain.BaseDirectory;
    static readonly object LogLock = new object();
    static readonly object PrintLock = new object();   // one job at a time per PC, so receipts never interleave

    static void Log(string text)
    {
        try
        {
            lock (LogLock)
            {
                string file = Path.Combine(Dir, "printagent.log");
                if (File.Exists(file) && new FileInfo(file).Length > 512 * 1024) File.Delete(file);
                File.AppendAllText(file, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ") + text + Environment.NewLine);
            }
        }
        catch { /* logging must never stop printing */ }
    }

    static void LoadOrigins()
    {
        Origins.Clear(); AnyOrigin = false;
        string file = Path.Combine(Dir, "printagent.txt");
        if (!File.Exists(file)) { Log("No printagent.txt: no web page is allowed to print yet."); return; }
        foreach (string raw in File.ReadAllLines(file))
        {
            string line = raw.Trim().TrimEnd('/');
            if (line.Length == 0 || line.StartsWith("#")) continue;
            if (line == "*") AnyOrigin = true; else Origins.Add(line);
        }
    }

    [STAThread]
    static void Main(string[] args)
    {
        bool created;
        using (new Mutex(true, @"Local\VlqmsPrintAgent", out created))
        {
            if (!created) return;   // already running for this user
            if (args.Length > 0) int.TryParse(args[0], out Port);
            LoadOrigins();
            TcpListener listener;
            try { listener = new TcpListener(IPAddress.Loopback, Port); listener.Start(); }
            catch (Exception e) { Log("Cannot listen on 127.0.0.1:" + Port + ": " + e.Message); return; }
            Log("Print agent " + Version + " listening on 127.0.0.1:" + Port);
            while (true)
            {
                TcpClient client;
                try { client = listener.AcceptTcpClient(); } catch { continue; }
                ThreadPool.QueueUserWorkItem(_ => { try { Handle(client); } catch (Exception e) { Log("Request failed: " + e.Message); } finally { client.Close(); } });
            }
        }
    }

    // ───── a very small HTTP/1.1 server (no URL reservation, so no administrator rights are needed) ─────

    static void Handle(TcpClient client)
    {
        client.ReceiveTimeout = 20000; client.SendTimeout = 20000;
        NetworkStream stream = client.GetStream();
        var head = new List<byte>();
        while (true)   // read up to the blank line that ends the headers
        {
            int b = stream.ReadByte();
            if (b < 0 || head.Count > 65536) return;
            head.Add((byte)b);
            int n = head.Count;
            if (n >= 4 && head[n - 4] == '\r' && head[n - 3] == '\n' && head[n - 2] == '\r' && head[n - 1] == '\n') break;
        }
        string[] lines = Encoding.ASCII.GetString(head.ToArray()).Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
        string[] first = lines[0].Split(' ');
        if (first.Length < 2) return;
        string method = first[0].ToUpperInvariant(), path = first[1].Split('?')[0];
        var headers = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        for (int i = 1; i < lines.Length; i++)
        {
            int c = lines[i].IndexOf(':');
            if (c > 0) headers[lines[i].Substring(0, c).Trim()] = lines[i].Substring(c + 1).Trim();
        }
        string origin; headers.TryGetValue("Origin", out origin); origin = (origin ?? "").TrimEnd('/');
        bool allowed = origin.Length > 0 && (AnyOrigin || Origins.Contains(origin));

        if (method == "OPTIONS")   // the browser's permission check before a POST from a web page
        {
            if (!allowed) { Reply(stream, 403, "{\"error\":\"This page is not allowed to use the print agent. Add its address to printagent.txt.\"}", null); return; }
            Reply(stream, 204, "", origin);
            return;
        }
        if (origin.Length > 0 && !allowed)
        {
            Reply(stream, 403, "{\"error\":\"This page (" + Esc(origin) + ") is not allowed to use the print agent. Add its address to printagent.txt next to the agent and restart it.\"}", null);
            return;
        }
        var json = new JavaScriptSerializer { MaxJsonLength = int.MaxValue };
        try
        {
            if (method == "GET" && path == "/health")
            {
                var info = new Dictionary<string, object>
                {
                    { "agent", "vlqms-print-agent" }, { "version", Version }, { "host", Environment.MachineName },
                    { "default", DefaultPrinter() }, { "printers", Printers() },
                };
                Reply(stream, 200, json.Serialize(info), allowed ? origin : null);
            }
            else if (method == "POST" && path == "/print")
            {
                if (!allowed) { Reply(stream, 403, "{\"error\":\"Not allowed.\"}", null); return; }
                int length; string len;
                if (!headers.TryGetValue("Content-Length", out len) || !int.TryParse(len, out length) || length < 0 || length > MaxBody)
                { Reply(stream, 413, "{\"error\":\"Nothing to print, or too large.\"}", origin); return; }
                var body = new byte[length];
                int got = 0;
                while (got < length) { int n = stream.Read(body, got, length - got); if (n <= 0) break; got += n; }
                var req = json.Deserialize<Dictionary<string, object>>(Encoding.UTF8.GetString(body, 0, got));
                string printer = req.ContainsKey("printer") ? Convert.ToString(req["printer"]) : "";
                string name = req.ContainsKey("name") ? Convert.ToString(req["name"]) : "vLQMS document";
                int copies = req.ContainsKey("copies") ? Math.Max(1, Math.Min(20, Convert.ToInt32(req["copies"]))) : 1;
                var pages = new List<Page>();
                foreach (object o in (IEnumerable)req["pages"])
                {
                    var p = (Dictionary<string, object>)o;
                    pages.Add(new Page(Convert.FromBase64String(Convert.ToString(p["png"])), Convert.ToDouble(p["w"]), Convert.ToDouble(p["h"])));
                }
                if (pages.Count == 0) { Reply(stream, 422, "{\"error\":\"There is no page to print.\"}", origin); return; }
                if (printer.Length == 0) printer = DefaultPrinter();
                if (printer.Length == 0) { Reply(stream, 409, "{\"error\":\"This PC has no default printer. Choose a printer.\"}", origin); return; }
                lock (PrintLock) PrintPages(printer, copies, name, pages);
                foreach (Page p in pages) p.Dispose();
                Log("Printed " + name + " on " + printer + " x" + copies + " (" + pages.Count + " page(s)) for " + origin);
                Reply(stream, 200, json.Serialize(new Dictionary<string, object> { { "printed", true }, { "printer", printer }, { "copies", copies } }), origin);
            }
            else Reply(stream, 404, "{\"error\":\"Not found.\"}", allowed ? origin : null);
        }
        catch (Exception e)
        {
            Log("Error: " + e.Message);
            Reply(stream, 500, "{\"error\":\"" + Esc(e.Message) + "\"}", allowed ? origin : null);
        }
    }

    static string Esc(string s) { return (s ?? "").Replace("\\", "\\\\").Replace("\"", "\\\"").Replace("\r", " ").Replace("\n", " "); }

    static void Reply(NetworkStream stream, int status, string body, string origin)
    {
        string text = status == 200 ? "OK" : status == 204 ? "No Content" : status == 403 ? "Forbidden" : status == 404 ? "Not Found" : "Error";
        var sb = new StringBuilder();
        sb.Append("HTTP/1.1 ").Append(status).Append(' ').Append(text).Append("\r\n");
        if (origin != null)
        {
            sb.Append("Access-Control-Allow-Origin: ").Append(origin).Append("\r\n");
            sb.Append("Access-Control-Allow-Methods: GET, POST, OPTIONS\r\n");
            sb.Append("Access-Control-Allow-Headers: Content-Type\r\n");
            sb.Append("Access-Control-Allow-Private-Network: true\r\n");   // Chrome: a public/LAN page reaching this PC's loopback
            sb.Append("Access-Control-Max-Age: 600\r\nVary: Origin\r\n");
        }
        byte[] bytes = Encoding.UTF8.GetBytes(body);
        sb.Append("Content-Type: application/json; charset=utf-8\r\nContent-Length: ").Append(bytes.Length).Append("\r\nConnection: close\r\n\r\n");
        byte[] head = Encoding.ASCII.GetBytes(sb.ToString());
        stream.Write(head, 0, head.Length);
        if (bytes.Length > 0) stream.Write(bytes, 0, bytes.Length);
    }

    // ───── printers ─────

    static List<string> Printers()
    {
        var list = new List<string>();
        foreach (string p in PrinterSettings.InstalledPrinters) list.Add(p);
        return list;
    }

    static string DefaultPrinter()
    {
        try { var ps = new PrinterSettings(); return ps.IsValid && ps.IsDefaultPrinter ? ps.PrinterName : ""; }
        catch { return ""; }
    }

    sealed class Page : IDisposable
    {
        public Image Img; public int W, H;   // size in hundredths of an inch (the printer's page unit)
        public Page(byte[] png, double widthPt, double heightPt)
        {
            Img = Image.FromStream(new MemoryStream(png));
            W = (int)Math.Round(widthPt / 72.0 * 100.0); H = (int)Math.Round(heightPt / 72.0 * 100.0);
        }
        public void Dispose() { if (Img != null) Img.Dispose(); }
    }

    static void PrintPages(string printer, int copies, string name, List<Page> pages)
    {
        for (int c = 0; c < copies; c++)
        {
            var doc = new PrintDocument();
            doc.PrinterSettings.PrinterName = printer;
            if (!doc.PrinterSettings.IsValid) throw new Exception("This PC has no printer called '" + printer + "'.");
            doc.DocumentName = name;
            doc.OriginAtMargins = false;
            doc.DefaultPageSettings.Margins = new Margins(0, 0, 0, 0);
            int i = 0;
            doc.BeginPrint += (s, e) => { i = 0; };
            doc.QueryPageSettings += (s, e) =>
            {
                e.PageSettings.Margins = new Margins(0, 0, 0, 0);
                e.PageSettings.PaperSize = new PaperSize("vLQMS", pages[i].W, pages[i].H);   // a receipt's own width and length
            };
            doc.PrintPage += (s, e) =>
            {
                e.Graphics.DrawImage(pages[i].Img, 0, 0, pages[i].W, pages[i].H);
                i++;
                e.HasMorePages = i < pages.Count;
            };
            doc.Print();
        }
    }
}
