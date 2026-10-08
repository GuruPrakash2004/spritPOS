# vLQMS Crystal Reports bridge

The web app prints the desktop's `.rpt` formats by asking this small Windows program to run them through the Crystal
Reports runtime, exactly as the desktop vLQMS does (load the `.rpt`, log on to SQL Server, set the selection formula and
parameters, then show / print / export). The web app never draws its own copy of a format.

**Status: written from the desktop's source but not yet built or run on a Windows PC.** Build it, then check one
invoice against the desktop's print of the same bill before relying on it.

## Build (Windows)

1. Install the **SAP Crystal Reports runtime for .NET Framework** that the desktop uses (the desktop vLQMS folder
   already contains `CrystalDecisions.*.dll`; the runtime's bitness must match this program's `PlatformTarget`,
   `x86` by default).
2. `dotnet build -c Release -p:CrystalDir=C:\vLQMS\` (or open `RptBridge.csproj` in Visual Studio).
3. Copy the DLLs the desktop folder has (`CrystalDecisions.*`, `log4net`, ...) next to `RptBridge.exe`.

## Run

The backend does not start the bridge: run `RptBridge.exe <port>` yourself and set `RPT_BRIDGE_URL` and `RPT_BRIDGE_KEY` in `backend/.env`. Without them the backend uses its built-in print (`app/mock_bridge.py`). The old text follows:

```
set RPT_BRIDGE_KEY=<long random text>
RptBridge.exe 8765
```
(`run_bridge.cmd` keeps that running; then put the same URL and key in `backend/.env`; `RPT_BRIDGE_AUTOSTART=0` stops the backend starting its own.) It listens on `127.0.0.1` only.

## backend/.env

```
RPT_FOLDER=C:\vLQMS\Rpts\        # the desktop's Reports → RptFilePath; default is backend/Rpts
RPT_TABLE_PREFIX=               # leave empty: reports read the desktop's own tables, which hold the bills made on the web too
```

`GET /print/bridge` (and the Print dialog) shows whether it answers.

## Notes

- The reports are made on the desktop's tables (`TrnHdrSINV`, ...), and the web app saves its bills in those same tables, so a bill saved on the web prints as it is. `RPT_TABLE_PREFIX` stays empty.
- The `showSPRpt` path (UtlMisc.InvPrintFromSP = Y: `fmtSPInvPrint.rpt` over the `mSP_PrintInv` procedure) is supported.
- The 7 `.rpt` files that embed the desktop's SQL Server connection stay out of git; copy them by hand.
