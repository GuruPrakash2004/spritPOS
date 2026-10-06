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

```
set RPT_BRIDGE_KEY=<long random text>
RptBridge.exe 8765
```
It listens on `127.0.0.1` only. Run it as a Windows service or from Task Scheduler at start-up (e.g. with NSSM).

## backend/.env

```
RPT_BRIDGE_URL=http://127.0.0.1:8765
RPT_BRIDGE_KEY=<the same text>
RPT_FOLDER=C:\vLQMS\Rpts\        # the desktop's Reports → RptFilePath; default is backend/Rpts
RPT_TABLE_PREFIX=Web_            # reports read the Web_ copies, where bills made on the web are saved
```

`GET /print/bridge` (and the Print dialog) shows whether it answers.

## Notes

- The reports are made on the legacy tables (`TrnHdrSINV`, ...). The bridge points each table at its `Web_` copy so a bill
  saved on the web prints. Bills made in the desktop after the Database setup copy are in the legacy tables only: set
  `RPT_TABLE_PREFIX=` (empty) to print those, or run the Database setup copy again.
- The `showSPRpt` path (UtlMisc.InvPrintFromSP = Y: `fmtSPInvPrint.rpt` over the `mSP_PrintInv` procedure) is supported.
- The 7 `.rpt` files that embed the desktop's SQL Server connection stay out of git; copy them by hand.
