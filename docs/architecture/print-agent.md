# Print Agent: printing on the counter PC's own printers (and reusing it in another application)

A web page can't list the printers of the PC it is open on, and can't print to them without a print dialog. The **vLQMS Print
Agent** is a small Windows program, one per counter PC, that closes that gap. This document describes how it works here and
**what another application has to do to use the same set-up**: the *Download Print Agent for this PC* button, the one-file
installer, the printer list in Settings and silent printing.

Source in this repository: `backend/tools/printagent/` (agent), `backend/app/routers/printing.py` (`/print/pages`,
`/print/agent`), `frontend/src/lib/localPrint.js` (browser client), `frontend/src/components/common/LocalPrinterSetting.jsx`
(Settings box), `frontend/src/components/print/PrintDialog.jsx` (print window).

---

## 1. What it does

```
 Counter PC (Windows)                                              Server (any OS)
┌──────────────────────────────────────────┐                     ┌───────────────────────────┐
│ Browser ── opens the web app ───────────────────────────────────▶ web app + API             │
│    │                                      │                     │  draws the document      │
│    │  1. GET  http://127.0.0.1:8766/health│                     │  (PDF → PNG pages)       │
│    │  3. POST http://127.0.0.1:8766/print │ ◀── 2. page pictures ┤  POST /print/pages       │
│    ▼                                      │                     └───────────────────────────┘
│ Print Agent (VlqmsPrintAgent.exe)         │
│    └─ prints the pictures on a printer of THIS PC (USB or network, or its default)         │
└──────────────────────────────────────────┘
```

* The **server draws** the document (here: Crystal Reports → PDF → PNG pages). The agent never knows the application's data.
* The **agent prints pictures** on whatever printer Windows has on that PC, silently, at the page size the server gave.
* The **browser is the bridge** between the two, because only the browser on the counter PC can reach `127.0.0.1` of that PC.
* No database, no login, nothing stored by the agent except `printagent.txt` (allowed web addresses) and a small log.

## 2. The agent (`Program.cs`, ~250 lines, C#, .NET Framework 4)

* Builds with the compiler that ships with Windows: `build.cmd` (`csc.exe`, no SDK) → `bin\VlqmsPrintAgent.exe`.
* Listens on **`127.0.0.1:8766` only** (nothing outside the PC can connect). A tiny HTTP/1.1 server on `TcpListener`, so it needs
  **no URL reservation and no administrator rights**.
* One instance per Windows user (mutex `Local\VlqmsPrintAgent`). One print job at a time (receipts never interleave).
* Runs hidden (`/target:winexe`), started at every sign-in by `HKCU\...\Run`.

### Protocol

| Call | Body | Answer |
|---|---|---|
| `GET /health` | none | `{"agent":"vlqms-print-agent","version":"1.0","host":"PC-NAME","default":"POS-80","printers":["POS-80","HP LaserJet", ...]}` |
| `POST /print` | `{"printer":"POS-80","copies":1,"name":"26/SI-0000012","pages":[{"png":"<base64>","w":226.8,"h":600.0}, ...]}` | `{"printed":true,"printer":"POS-80","copies":1}` |
| `OPTIONS *` | none | `204` with CORS headers (the browser's permission check) |

* `printer` blank → the PC's default printer. `copies` 1 to 20. `w` / `h` are the page size in **points** (1/72 inch); the agent sets
  the paper to exactly that (a 3-inch receipt keeps its width and length) with zero margins.
* Errors are JSON `{"error": "..."}`: `403` page not allowed, `409` no default printer, `413` too large (64 MB), `422` no pages,
  `500` Windows refused (the message says why, e.g. "This PC has no printer called ...").
* `GET /health` is how the web app **detects** the agent (`"agent":"vlqms-print-agent"`) and **lists the printers** (the list is
  `PrinterSettings.InstalledPrinters`: every printer connected to that PC, USB and network, as Windows sees it).

### Security

* **Loopback only.** Only software on the same PC can reach the agent.
* **Origin allow-list.** The browser sends the page's `Origin`; the agent answers only for origins listed in `printagent.txt`
  (next to the exe, one per line, e.g. `http://192.168.1.10` or `https://shop.example.com`; a line `*` allows any page; **no file = nothing
  is allowed**). Any other page gets `403`, so a random website open in the same browser can't print on the counter's printer.
* **CORS / Chrome "local network" rule.** Replies carry `Access-Control-Allow-Origin: <that origin>` and
  `Access-Control-Allow-Private-Network: true` (a LAN or public page reaching a loopback address needs it). Chrome may ask the
  user once to *allow access to devices on the local network*: the user allows it.
* Limits: 64 MB per job, 20 copies, 20 s socket timeouts.

## 3. The server side

### 3.1 Drawing the pages: `POST /print/pages`

The application must be able to produce **page pictures** for a document. Here: render the document to PDF (Crystal), then rasterise
every page with PyMuPDF at 203 dpi (a thermal receipt printer's resolution) and return
`{"pages":[{"png":"<base64>","w":pts,"h":pts}], "copies":1, "name":"<doc no>"}`.
Any renderer works as long as the answer has that shape: HTML to PDF (Chromium, wkhtmltopdf, WeasyPrint), a PDF library, a report
engine. Page width / height must be the real paper size in points.

### 3.2 The download: `GET /print/agent?origin=http://192.168.1.10`

The button *Download Print Agent for this PC* gets **one self-installing file**, `vLQMS-Print-Agent-Setup.cmd`, already set to the
web address the user has open (`window.location.origin`):

1. The server finds the built exe (`bin/VlqmsPrintAgent.exe`; if missing and the server is Windows it builds it once with `csc.exe` at
   start-up, see `_agent_exe()`), base64-encodes it and writes it into a `.cmd` file as lines starting `::B64 `.
2. The `.cmd` starts with a short batch script that: stops a running agent, creates `%LOCALAPPDATA%\VlqmsPrintAgent`, unpacks the exe
   from its own `::B64` lines with a PowerShell one-liner, **adds the origin to `printagent.txt` (kept if already there)**, registers
   `HKCU\...\Run\VlqmsPrintAgent`, starts the agent and tells the user to go back to the app.
3. The origin is validated (`^https?://[A-Za-z0-9.\-\[\]:]+$`) before it is written into the script.

No administrator rights, no installer, no files to copy: the user double-clicks the downloaded file once. From then on the
agent starts at every sign-in.

## 4. The browser side

`frontend/src/lib/localPrint.js` (framework independent, plain `fetch`):

* `detectAgent()`: `GET http://127.0.0.1:8766/health` with a **600 ms timeout**, remembered for 20 s (3 s while missing). Returns
  `{host, default, printers}` or `null`. **A save or a print never waits for it**: `agentNow()` returns what is already known and
  `warmAgent()` looks in the background (the page also looks once when it loads).
* `printOnThisPc(pages, {printer, copies, name})`: `POST /print` to the agent. If the agent is gone it clears its cache and says so.
* `printDocumentOnThisPc(request, printer)`: asks the server for the pages (`POST /print/pages`) then sends them to the agent.
* The chosen printer is kept in the browser (`localStorage`), prefixed `pc:` so it can be told from a server printer.

`LocalPrinterSetting.jsx` (Settings → Print Settings): a drop-down of the PC's printers from `/health` (default marked), a Save button,
and, when the agent is missing, the explanation and **Download Print Agent for this PC**. While the agent is missing it asks again
every 4 s, so the list appears by itself right after the one-time set-up. `PrintDialog.jsx` lists the PC's printers under *This PC*
and the server's under *Server*; a Save & Print with a direct print uses the PC's remembered printer, else its default, and falls
back to the server's printer if the agent has gone away.

## 5. Using the same set-up in another application

### Option A (recommended): share the agent that is already on the PC

The agent knows nothing about vLQMS: it prints pictures for any allowed web address. Another application on the same PC only
needs its **address added to `printagent.txt`**, which the download in step 4 does for you (it appends, never overwrites).

1. **Server: add `POST /print/pages`** that returns page pictures in the shape of section 3.1.
2. **Server: add `GET /print/agent?origin=...`** that returns the Setup `.cmd` of section 3.2 for *your* address. Copy the script
   template (`AGENT_INSTALL` in `printing.py`) and the `::B64` packing. Ship the same `VlqmsPrintAgent.exe`
   (copy `bin/VlqmsPrintAgent.exe` or build it with `build.cmd`).
3. **Browser: copy `localPrint.js`** (the base address `http://127.0.0.1:8766` stays) and call `detectAgent()` /
   `printDocumentOnThisPc()`.
4. **UI:** a printer drop-down fed from `detectAgent().printers` and the *Download Print Agent* button (copy
   `LocalPrinterSetting.jsx`; only the API function that downloads the file differs).
5. **Permission:** the first time, the user runs the downloaded Setup file once on each counter PC. If the other application
   is opened at a different address, its own Setup run adds that address.

If the agent is already installed for the first application, the second application's Setup just adds one line to
`printagent.txt` and restarts the agent (`taskkill`, copy the exe, start): both applications then use the one agent.

### Option B: an agent of its own for the other application

Use this only when the two applications must not share a PC-wide agent. Copy `backend/tools/printagent/`, then change, in
`Program.cs`, `install.cmd` and the server's `AGENT_INSTALL`:

| What | Why |
|---|---|
| port `8766` (and `BASE` in `localPrint.js`) | two agents can't listen on the same port |
| mutex name `Local\VlqmsPrintAgent` | so both can run |
| exe name, folder `%LOCALAPPDATA%\VlqmsPrintAgent`, `HKCU\...\Run` value | so one install doesn't replace the other |
| the `"agent":"vlqms-print-agent"` identifier (and its check in `detectAgent`) | so each app finds its own agent |

### Checklist when wiring it into the other application

* [ ] `GET http://127.0.0.1:8766/health` on the counter PC opens in its browser and shows the PC's printers.
* [ ] The web address in `printagent.txt` equals the browser's address bar **exactly** (`http` vs `https`, the port, a name vs an IP).
* [ ] `/print/pages` returns the real paper size in points; a receipt prints at the roll's width.
* [ ] The probe never blocks the user: short timeout, cached, started when the page loads.
* [ ] The page works without the agent (falls back to the browser's print window or a server printer) and says what to do.
* [ ] The download file is generated for the page's own origin (`window.location.origin`).

## 6. Limits and things to know

* **Windows counter PCs only** (the agent is a .NET Framework program on `System.Drawing.Printing`).
* It prints **pictures**: no text mode and no ESC/POS commands (cash-drawer kicks, paper cuts need the printer driver's own setting).
* The page size comes from the server, so set the printer's paper to the roll width in Windows (printing preferences).
* Browsers: Chrome and Edge work (loopback is a trustworthy origin, so an `https` page may call it; allow the *local network*
  prompt). Firefox and Safari may block calls from a LAN or public page to `127.0.0.1`: test them before promising them.
* An unsigned exe may trigger SmartScreen or antivirus: sign the exe for a customer roll-out.
* The agent trusts the allow-listed pages completely: anything on such a page can print on that PC. Keep the list short and avoid `*`.
* Logs: `%LOCALAPPDATA%\VlqmsPrintAgent\printagent.log` (jobs and errors, trimmed at 512 KB). Remove with `uninstall.cmd`.

## 7. Troubleshooting

| Symptom | Cause / fix |
|---|---|
| `/health` doesn't open | the agent isn't running: run `%LOCALAPPDATA%\VlqmsPrintAgent\VlqmsPrintAgent.exe`, read `printagent.log` |
| `/health` opens, the app still shows no PC printers | the address in `printagent.txt` doesn't match the browser's; fix it and restart the agent |
| browser asks to access devices on the local network | allow it |
| 403 "This page is not allowed to use the print agent" | add the page's address to `printagent.txt`, restart the agent |
| "This PC has no printer called ..." | the printer was removed or renamed on that PC; choose another |
| receipt too small or cut | set the printer's paper to the roll width in Windows |
| first print slow, later ones fast | the server's first render; see `RPT_BRIDGE_CACHE` and the `Crystal bridge ... took` log lines |
