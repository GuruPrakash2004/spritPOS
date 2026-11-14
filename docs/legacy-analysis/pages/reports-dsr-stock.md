# Reports (first pass): Daily Summary, Stock Balance, Stock Ledger, Profit Margin, Stock Ageing

The shop's menu shows eight reports: Brand Wise, Customised Reports, Daily Summary, Profit Margin, Stock Ageing, Stock Balance, Stock Ledger, Stock Level. This pass covers the first five against `frmRptDailySummary.vb` (1,371 lines), `frmRptProMrg.vb`, `frmRptStockAgeing.vb` (838) and the stock list the exe reads (`UtlStockList`). Each was checked against the exe's own SQL on the shop's database (read-only): same questions asked of the same day or period, the answers compared.
Web: `routers/reports.py`, `DailySummaryReport.jsx`, `reportDefs.js`.

## The main finding
The web's Stock Balance, Stock Ledger, Profit Margin and the DSR's stock rows were worked out from the web's own in-memory stock movements, which start when the web app starts. For any day or period touching documents made in the desktop they were wrong (DSR of 12 Nov: web opening stock 22,502, the exe's 22,546; closing 22,502 against 22,516). They now read the stock list like the exe.

## Done (backend 716 tests; frontend lint, build, 285 tests pass)
| Report | Change | Checked against the exe's SQL |
|---|---|---|
| Stock Balance | opening, receipts, issues, closing per item from `UtlStockList` (`DocDate`, `StkMultiplier`; rows marked deleted left out) | period 1-13 Nov: opening 22,609, receipts 0, issues 107, closing 22,502: identical |
| Stock Ledger | the item's movements from `UtlStockList` with a running balance | same source |
| Daily Summary | stock rows from the stock list; **new section "Summary as in the desktop"** with the exe grid's rows: Total Bills, Total Sales Amount, Liquor / NonLiquor basic (by `LiqType`, as the exe does), Opening / Closing Stock Qty and Value at sales rate (MRP), Cash / Card / Other Pay, Total Purchase Amount (Sales rate), Purchase (Basic / Incl.GST), Sales Return, GST Summary (CGST / SGST by rate), Expense Summary, Mode of Payments, Counter Wise Sales (bills and value) with cancelled invoices, Sales Person Wise, Group Wise Sales (qty and amount, in Arrange Slno order), Group Wise Purchase, Expense Ledger Wise, Redemption; Export to Excel | 12 Nov: bills 5, sales 33,260, liquor 33,260, groups (WHISKY 29 / 33,160; WINE 1 / 100), opening qty 22,546 and value 71,42,723, closing 22,516: identical |
| Profit Margin | per item from the saved sales lines of the period (cancelled and deleted bills left out), sales amount `SINVAMT`, item purchase rate, group then item order; items with quantity 0 left out | 1-13 Nov: 19 items, 107 qty, 1,66,480 sales, 1,51,368.47 purchase value: identical |
| Stock Ageing | closing quantity as on the date from the stock list (items above zero only) and the age of the oldest receipt still holding stock (the exe's "Show PB" layers: newest receipts cover the closing quantity) | 13 Nov: 906 items, 22,502 qty: identical |

## Kept as they were
The DSR tiles and their sections (web spec, `Docs/WebDash.xlsx`), Print through `RptDSR.rpt`, Profit of the Day (the exe shows it only for one client's database).

## Open
1. **Stock Balance**: see the section below (views built 2026-11-14); open: AROED columns, Print Zero Stock Items, Email PDF, Ctrl+F8/F9 layout, column select / freeze.
2. **Stock Ageing**: built 2026-11-14, see the section below; the web's 30-day month buckets stay as the default "By months" choice.
3. **Customised Reports** (`frmRptCustomiseRptView`): audited, see the section below.
4. **DSR options**: Include Non-Liquor / Purchase-rate valuation exist in the exe (the panel is shown only for two clients' databases); the web uses the exe's defaults (include non-liquor, sales rate).

## Brand Wise (`frmRptBrandWise.vb`), audited 2026-11-14
| Legacy | Web | Status |
|---|---|---|
| `vwBrandReport` (purchase bills + sales invoices, not cancelled/deleted), grouped per item, brand then item name (:116-132) | `/reports/brand-wise`: the stock list (PB / INV rows, indexed by date) + the bill lines' own PBAmt / SInvAmt for those bills; 1 Apr-14 Nov totals identical to the view (qty 289,242 / 290,879; value 4,60,97,005.43 / 4,99,65,995.04). The view scans 800,000 bill headers (no date index) = 20-30 s; this is ms for a short period | Match (faster) |
| Options Purchase / Sale / Purchase Vs Sale, With Value (:119-122, :201-234) | Show select + With Value box decide the columns | Match |
| Select Brand / Select ItemGroup multi-selection (:58-94) | tick-box drop-downs (`MultiSelectDropdown.jsx`) | Match (drop-down by request) |
| Cases = floor(qty / BotPerCases), bottles = remainder, only for Purchase or Sale (:159-186) | same; blank for Purchase Vs Sale | Match |
| Brand subtotal "Total- x" and Grand Total (:251-258) | rows with the same names | Match |
| Ctrl+F8 / F9 layout save, copy-grid panel, merge cells | not built | Not built (desktop grid features) |

## Stock Level (`frmRptStkLevel.vb`), audited 2026-11-14
Below Min / Below ROL / Above Max SQL (:446-657): Match. Added: Selected Group, Select Item, Select Package filters (:470-487) and Show qty in Litres (:455). `chkZeroBal` ("Do Not Show Items With Zero Balance") is never applied by the exe (`RemoveVsfRows` is not called), so it is not built. Open: Print / Email PDF through `RptStkBal.rpt` with `RptHdrStk` / `RptDtlStk` work tables (:669-837).

## Stock Balance screen (`frmRptStockBal.vb`), built 2026-11-14 (`app/stock_balance_view.py`, `GET /reports/stock-balance/view`)
| Legacy | Web | Status |
|---|---|---|
| Closing Balance Only (`ViewClData` :712): opening (< From), receipts (StkMultiplier 1), issues (-1), closing, value at Last Purchase Rate / MRP, active items only | `view=closing`; 1-13 Nov: opening 22,609, issues 107, closing 22,502 (same as the exe SQL checked earlier) | Match |
| Closing Balance With Detailed (`ViewClDetailed` :1311): OP, PB (PB + ST), SR, Inv, Ded, PR, SA, Cl qty and values, Inv Rate, Rate Per Case, In Case, Actual Sales Value (invoice lines' ItemAmt), With Tax (NetAmt/PBQty) | `view=detailed` | Match |
| Package wise Balance (`ViewPkgData` :1003): a column per package, rows group / brand / main item | `view=package` (closing up to the To date) | Match |
| Group wise Summary (`ViewGrpSmryData` :2319): opening and closing value per liquor group, one Non-Liquor line | `view=group`; the exe counts closing up to the **From** date (:2368): repeated. This shop has every group flagged LiqItem N, so it shows one Non-Liquor line, as the exe | Match |
| Filters Selected Group / Select Item / Select Package, Show qty in Litres, Do Not Show Zero Balance, Group Wise Total, Brand column only when `BRANDREQ` = Y | all built (drop-downs) | Match |
| Wt.Avg Rate (`valuateWAR` :2210) **writes** `UtlStockList.DocValue` | the same: rewrites only the rows whose value changes, inside `db.direct_writes()` (first run on this shop rewrote ~1.58 million rows; old values saved before the run; a second run changes 0) | Match (as the owner asked) |
| FIFO (`showFifoClDetailed`) runs `mSp_UpdateFIFORate` first | the same procedure is run before reading | Match |
| AROED columns, Print Zero Stock Items, Email PDF, grid layout save, column select / freeze, Stock Carry Forward button | not built here (carry forward is under Company Master) | Open |

## Stock Ageing (`frmRptStockAgeing.vb`), built 2026-11-14 (`app/stock_ageing_view.py`, `GET /reports/stock-ageing/desktop`)
| Legacy | Web | Status |
|---|---|---|
| Age ranges grid, defaults 0-30, 31-60, 61-90, 91-180, 180-; From = previous To + 1; "Invalid Range! Check Entry..." (:106-134, :396-486) | "By age ranges (days)": a text field of ranges (`0-30,31-60,...,180-`), same validation message | Match |
| Closing quantity > 0 as on the date per item, group order (:519-538) | same, from the stock list | Match (906 items, 22,502 as on 13 Nov) |
| Receipt quantity (StkMultiplier 1, per date) put in the range of its age in days (:577-648); issues are not taken off, so range totals can exceed the closing quantity | same, quirk kept | Match |
| Valuation Standard / FIFO / Wt.Avg. / Last Purchase Rate from `UtlStockRate` on the newest StkDate (:497-520) | same; this shop's UtlStockRate is empty so values are 0, as in the exe | Match |
| Selected Group, Select Package (:525-535) | drop-downs. The exe's own receipts query refers to the item table without joining it, so its filters would fail there; here they apply | Match (exe bug not repeated) |
| "Value" column after each range (headed, never filled by the exe) | shown, blank | Match |
| Show Ageing - Purchase Bill Wise (`PBAgeing` :711): newest receipts covering the closing quantity, PB No / Date / Age Days (to today) / Particulars / Qty / Qty Left / Rate / Amount | "Purchase Bill wise" | Match |
| Location choice (only the first location name is loaded), Print / Email PDF, saved layout | not built | Open (no action in the exe's code for print) |

## Customised Reports (`frmRptCustomiseRptView.vb`), audited 2026-11-14 (`app/custom_engine.py`, `routers/custom_reports.py`, `ReportsDashboard.jsx`)
| Legacy | Web | Status |
|---|---|---|
| Report list by group, search by name, rights per report | list by group, per-report rights | Match |
| Per-report filters = the definition's `FilterReq` fields (:543-607): DATE fields take From / To, other fields a multi-value pick (`field IN (..)`, :466-479) | `GET /reports/custom/filters` + `/filter-values`; the "Filter on" list gains the report's own fields; picked values go as `ff=FIELD|value` (also in Email and Print) | Built |
| The period applies only to the report's DATE filter fields; a report without one is not date-limited (:439-449) | same (before, the first DATE column was always limited); the From / To boxes show only when the report takes a period | Built |
| Hidden columns are not selected (:387); GROUP BY only when a column is aggregated; ORDER BY every column (:398, :506-508) | same | Built |
| Aggregates `FldAggregate(...)` (:390) | Sum, Count, Avg, Max, Min | Built |
| `AddnlCol` (`expression|ALIAS`), `IsSlnoReq` serial column (:400-407, :374) | built (not used by this shop's reports) | Built |
| Subtotal rows "<value> SubTotal" by `SubTotOn` and Grand Total by `TotReq` (:297-355) | subtotal rows (first SubTotOn column) and the footer total | Built (nested subtotal levels not) |
| Header "Report for the period: from .. to .." and "Report Filtered by .." (:446, :460) | returned as `header`, shown above the table | Built |
| Print Report through the Crystal file, Email PDF | unchanged (Crystal logon gap noted in LEGACY_PARITY) | Open |
| Design, Import (:1078), Assign Rights buttons (report designer) | not built; rights are set in Admin Master → Report rights | Open |
| Note: these reports run 20-30 s on a long period: the exe's header tables (806,000 invoices) have no date index | unchanged | Open (database) |
