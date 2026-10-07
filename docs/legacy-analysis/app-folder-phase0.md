# Legacy vLQMS desktop `App` folder: Phase 0 (foundation)

Source: `C:\Users\LEGION\Desktop\1.vLQMSAPP\App` (read-only study). Method: CLysis `current-legacy` Phase 0, auto-collected
from the folder, `vLQMS.exe.config`, the `.ini` files, `vLQMS.xml` (XML doc of the assembly), the exe's .NET metadata
(`ReflectionOnlyLoadFrom`, type and method names only) and `ErrorLog.txt`. Nothing was run; no code was decompiled.
Statements marked *(estimated)* come from names, not from reading code. **No credentials are recorded here.**

## 1. What the folder is
A **VB.NET WinForms** app (.NET Framework 4.x, `supportedRuntime v4.0 / 4.5`), one main exe plus its libraries, INI files,
a work folder per document type, and companion tools. 120 files, 4 sub-folders.

| Count | Kind |
|---|---|
| 5 | exe: `vLQMS.exe` (18.7 MB, built 2026-08-06; entry point `vLQMS.My.MyApplication.Main`), `vLQMS_old.exe`, `vLQMS_Test.exe`, `vLQMSWEBUpdate.exe` (web updater, 2021), `vFactIC Designer.exe` (Doc Designer, 2019) |
| 52 | dll: ComponentOne (C1) grids/input/chart/ribbon 2013, Crystal Reports 13.0.2000 runtime assemblies, ClosedXML + DocumentFormat.OpenXml (Excel), PdfSharp, iTextSharp, ExcelDataReader, ThoughtWorks.QRCode, QRCoder, AForge (webcam), `SSPLLic`/`svtLicence` (licence), `EssaeScaleDLL` (weighing scale), `TCPCam`, Flash control |
| 24 xml, 2 pdb | assembly docs and symbols (`vLQMS`, `vFactIC70`) |
| 5 ini | `Factic.ini`, `FacticDsgr.ini`, `ServerDetail.ini`, `Recentdb.ini`, `RECENT.INI` (+ `Factic.inibak`) |
| 25 xlsx | import/export working files (e.g. closing stock entry) |
| dirs | `AutoSave` (7), `Purchase Bill`, `Sales Invoice`, `Temp` (25) |

## 2. Startup (init) chain *(estimated from type and method names)*
```mermaid
flowchart TD
  EXE[vLQMS.exe] --> MAIN[My.MyApplication.Main / OnInitialize]
  MAIN --> SPL[frmsplash / FrmMainSplash]
  MAIN --> CFG[INI files: connection, paths, last user]
  CFG --> DBC{DB reachable?}
  DBC -- no --> CONN[frmUtlDBConnection: server, DB name, login, paths, init DBs]
  DBC -- yes --> LIC[Licence check: SSPLLic.dll, clsReg, frmKeyReg]
  LIC --> LOGIN[frmLogin: LoadUser, GetUserImage, userValid, password rules]
  LOGIN --> OTP[frmOTP (optional)]
  LOGIN --> FY[FrmSelFinYear / frmMstSelComp / frmUtlSelBranch]
  FY --> MAINF[frmMain: AddMenusFromDB, SyncDB, ChkFinYearCreation, SetCultureInfo, welcomeMsg]
  MAINF --> SCR[screens from the menu tables]
```
`ErrorLog.txt` shows the first DB call at login is `frmLogin.LoadUser` then `GetUserImage` (an OleDb query), so the connection
must be up before the login form can fill.

## 3. Technology stack
| Area | Finding |
|---|---|
| Language / runtime | VB.NET, .NET Framework 4.0/4.5, WinForms |
| Data access | `svtLibrary` (`getDATATable`, `getDataReaderFromSQL`, `execSQLStatement`, `WriteToErrorLog`), OleDb (log shows `System.Data.OleDb`), SQL Server (`DBTYPE;;MS-SQL`; Access `.mdb` option exists in the designer) |
| UI | ComponentOne FlexGrid (`vsf*` grid code), Input, Command, Chart, Ribbon; DotNetBar-style tabs; custom `CButton`, `ucSearch` |
| Reports | Crystal Reports 13 (`.rpt` in `Rpts`), plus C1Report, a report designer (`frmRptCRDesign`, `frmRptDesigner`), Excel export (ClosedXML/Interop) |
| Printing/labels | `PrintBillCrpt`, `PrintInvCrpt`, `PrintKOT`, barcode/label designers (`frmUtlBCDesigner`, `frmUtlLabelDesign`), QR codes |
| Config store | INI files read through `svtFileCtrl`/`svtModule`; `Factic.ini` holds paths, last user, document prefixes; `FacticDsgr.ini` the designer's DB connection; `ServerDetail.ini` the app's DB connection |
| Security | `svtCrypto` (`MD5Hash`, `Encrypt`, `Decrypt`); the DB password in the INIs is stored encoded, not plain |
| Licence | `SSPLLic.dll` (+ older copies), `svtLicence.dll`, `clsReg` (`getKey`, `validKey`, `getRegNo`), `frmKeyReg` |
| Extras | Outlook/Excel interop, SMS (`frmTrnSMS`, `frmUtlSendSMS`), email (`frmTrnEmail`, `frmUtlSMTPSettings`), web sync (`frmModWebSync`, `WEBDBCHANGES`, `vLQMSWEBUpdate.exe`), webcam, voice commands (`getVoiceCmd`), weighing scale |

## 4. Quantitative scan (counts only)
- 418 types in `vLQMS.exe`; **234 forms** (`frm*`), the rest are controls/modules (`svtModule` 213 methods, `DsgrModule` 81,
  `svtLibrary` 18, `svtCrypto` 3, `clsReg` 5, `frmMain` 95, `frmLogin` 26).
- Form families (by prefix): `frmMst*` masters (item, party, user, role, rights, UOM, company, fin year, price list, BOM, holidays...),
  `frmTrn*` transactions (invoice, POS invoice, quick bill, PSE, closing stock, STKDED, SR, memorandum, vouchers, SMS, email...),
  `frmRpt*` reports (daily summary, sales, purchase, stock balance/ledger/ageing/level, accounts outstanding/ageing, drops DSR/petty cash,
  loyalty, audit log, excise stock, brand/pkg-wise...), `frmUtl*` utilities and settings (about, audit log, global settings, SQL query exec,
  merge items, import from Excel/Tally, restore, DB connection, print settings, barcode, dashboard, WAS, short close...).
- Largest binaries: `vLQMS.exe` 18.7 MB, `vFactIC70.pdb` 8.5 MB, `DocumentFormat.OpenXml.dll` 5.2 MB.

## 5. Risks found
- **`Recentdb.ini` stores earlier SQL Server connections with the login and password in plain text** (`server|database|user|password`).
  Treat the file as a secret; do not copy it, commit it or ship it. (Values deliberately not reproduced.)
- `Factic.ini`, `FacticDsgr.ini`, `ServerDetail.ini` hold the live server, database and an encoded password; same handling.
- Three copies of the licence DLL (`SSPLLic`, `_old`, `_16062026`) and three exe copies (`vLQMS`, `_old`, `_Test`): rollbacks kept in place.
- C1 components are from 2013 and Crystal 13 from 2015 (EOL-range runtimes), Flash control referenced.
- `copyini.bat` copies `D:\vlqms\factic.bak` over `factic.ini` (restore of the connection file), hard-coded paths.
- Data/report paths in `Factic.ini` are on a mapped drive `R:\...\1.vLQMSAPP\Rpts\` (same `Rpts` folder this web app prints from).

## 6. Known / still unknown
**Known:** stack, file inventory, entry point, form families, config files and their roles, licensing/crypto modules by name,
that login precedes the main form, that menus come from the database (`AddMenusFromDB`), web sync exists.

**Unknown (Phase 1 targets):**
1. Exact sequence inside `OnInitialize`/`OnCreateMainForm` (splash vs licence vs DB check order): needs IL or the legacy source.
2. How `svtCrypto.Decrypt` encodes the INI password, and the licence key algorithm (`clsReg`); out of scope for the web app.
3. Which tables each `frmTrn*` form writes (the web app already maps invoices, purchase bills, stock; the others are open).
4. Menu tables behind `AddMenusFromDB`, the rights model (`frmMstRights`, `frmUtlAssignRight(s)`), and field-level rights.
5. What `SyncDB`, `updateToCloudDB`, `ExecReWriteStock`, `alterReWriteStkSP` run on start.
6. Web-sync/`WEBDBCHANGES` payloads (the web app has its own `websync.py`).
7. Roles of `AutoSave`, `Temp`, `Purchase Bill`, `Sales Invoice` work folders (draft recovery *(estimated)*).

## 7. Next (Phase 1 candidates)
Pick one theme to trace: (a) **login + licence + menu build** (feeds the web login and role model), (b) **POS/Sales invoice save**
(`frmTrnPosInv`/`frmTrnInv`), (c) **Re Write Stock / startup SQL** (`ExecReWriteStock`), (d) **Loyalty** (`frmRptLoyalty`, `TrnPoints`).
The web app's existing UI styles stay as they are; the findings feed behaviour and data only.
