# Legacy desktop app (`MAIN_Root/vLQMSAPP`)

An install of the original vLQMS: a .NET WinForms app (`App/vLQMS.exe`) using ComponentOne grids and Crystal Reports on SQL Server. It is **reference material, not code to build or run**. Use it to learn what a screen must do before building the web version.

`frontend/LEGACY_PARITY.md` is the distilled result: every legacy screen, its React status (Built / Partial / Stub / Missing) and a suggested build order. Read that first, and keep it updated as modules land.

## Where the specs are
- `Docs/VLQMS Help Doc.pdf`: the 41-page user manual; the best description of each screen's behaviour.
- `Docs/WebDash.xlsx`: field-by-field spec of the Daily Summary Report / web dashboard.
- `Docs/vLQMS Closing Stock Entry screen Creation.xlsx`: spec for Closing Stock Entry.
- `Docs/ItemImport_Template.xlsx.xlsx` and `Docs/StockImport.xlsx`: Item Master import columns, plus real item/group/package names that make good sample data.
- `Docs/vLQMS_Loyalty Points/`: loyalty program setup and redemption.
- `Rpts/*.rpt`: Crystal print formats and reports (`fmtSInv*` invoice, `fmtPB*` purchase bill, `RptDSR*`, `RptStkBal*`…).
- `App/AutoSave/<Module>/*.xml`: auto-saved transaction drafts whose `<Data>` cells list each screen's real header, grid, total and footer fields.

The `legacy-docs` skill has the commands for reading these files on this machine.

## Gotchas
- Most screens are generic engines (`frmTrnVch` for transactions, `frmMstGeneral` for masters) configured from SQL tables (`MstMenu`, `UtlTransactions`, `UtlDocDsgrFlds`), and `DB/` is empty. So the menu is not recoverable from files alone; form names and captions come from the exe.
- The exe includes generic-ERP screens (BOM, KOT, quotations, SO/PO, doctor-vs-service) that are **not** part of the liquor edition. Don't port them.

## Sensitive: do not copy or quote
`App/ServerDetail.ini`, `App/RECENT.INI`, `App/FacticDsgr.ini`, `Rpts/SvtConn.ini` and `Rpts/RECENT.INI` hold plaintext SQL Server credentials, including `sa`. The folder also holds licence DLLs and customer exports. Never copy these into `frontend/`, echo their values, or commit `MAIN_Root/`. `.claude/settings.json` denies reading the `.ini` files.
