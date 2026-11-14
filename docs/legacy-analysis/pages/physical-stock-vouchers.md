# Physical Stock Entry (`frmTrnPSE`) and Vouchers (`frmTrnVch`) vs the web app

## Physical Stock Entry
Legacy: `frmTrnPSE.vb` (914 lines, read in full). The shop's database (read-only): menu item 1722 (shown), `TrnHdrPSE` / `TrnDtlPSE` empty (not used yet). Web: `PhysicalStockEntry.jsx`, `routers/physical_stock.py`.
Already a thorough port: pick groups / packages / items → Load, Physical Stock, Pur Rate, MRP editable, Diff; Save updates MRP / purchase rate where changed, posts the excess as a Purchase Bill (supplier KSBCL, "From Physical Stock Entry") and the shortage as a Stock Deduction or, with "Show Reduced Qty as Sales", a Sales Invoice; only changed rows kept; Clear Physical Stock; Summary Report; View Previous Entered Records; Print (`fmtPSE.rpt`) from the viewer.
| Legacy | Web now |
|---|---|
| "Please Select Alteast One Item / ItemGroup / Package.." (337), "There are no Changes to Update!" (507) | the exe's texts, on the screen and the server |
| Order of the list: `GroupOrderBy, ItemGroupName, ..., PkgOrderBy, PurRate desc` (412), i.e. the shelf order set in Arrange Slno | the same (groups and packages now carry `GroupOrderBy` / `PkgOrderBy`) |
| Summary Report: per package and group, physical / system bottles, difference, litres, and "N Cases,M Bottles" (btnShowSumm 708-809); Export to Excel of the grid (145) and of the summary (815) | Cases / loose-bottle columns added; **Export to Excel** on both |
| The documents it posts carry the exe's numbers (`2600202`, printed `26//00202`) | follows from the Purchase Bill / Stock Deduction numbering done earlier |
Differences kept: the exe takes the system stock from `UtlStockList` (sum of qty x multiplier since 2017) and the web from the stock balance (same figures while they agree); the exe's "Show Reduced Qty as Sales" writes an invoice numbered `<year><7 digits>` with counter 0, the web uses the chosen counter's normal invoice; `PkgItem` (an item column) is not part of the web's ordering.

## Vouchers (`frmTrnVch`, 4,391 lines)
Payment, Receipt, Credit / Debit Note, Contra and Journal vouchers: hidden in this shop's menu (`MenuShow = 'N'`), `TrnHdrPMTVCH` / `TrnHdrRCPTVCH` / `TrnHdrAcc` empty, `MstAcLed` has 69 ledgers. Web: `accounting.py`, `routers/accounting.py`, `VoucherEntry.jsx`.
Already matching: debit = credit "Debit and Credit totals are NOT matching!" (1427), ledgers must exist and be active, edit / cancel with the freeze date, numbering `<year><5 digits>`, postings in `TrnHdrAcc` / `TrnDtlAcc`, cancel = `Cancelled = 'Y'`.
Changed: "Current record is cancelled. Cannot edit!" (3479) and "Current record is cancelled. Deletion not possible!" (370); a missing / inactive ledger says "AcLedName does not exist!!" first.
Open: bill-wise adjustment dialogs, cheque details, file attachments, Excel import, the "Delete" (hard) choice: not covered / not built.

## Closing Stock Entry edit (follow-up to the previous pass)
Built: `PUT /transactions/closing-stock/{no}` and Edit on the screen's action bar: refused when cancelled or once the sales invoice is generated ("Invoice Generated Cannot To Edit"); same number, lines rewritten.
