# Legacy vLQMS → web UI parity

What the legacy Windows app in `MAIN_Root/vLQMSAPP` does, and how much of it the React prototype in `frontend` covers.

**Sources.** `Docs/VLQMS Help Doc.pdf` (41-page user manual), the form classes and captions compiled into `App/vLQMS.exe`, `Docs/WebDash.xlsx` (DSR / web dashboard spec), `Docs/vLQMS Closing Stock Entry screen Creation.xlsx`, `Docs/ItemImport_Template.xlsx.xlsx`, `Docs/vLQMS_Loyalty Points/`, the Crystal Reports in `Rpts/`, and an auto-saved Purchase Bill draft in `App/AutoSave/`.

The legacy menu itself lives in the `MstMenu` SQL table, and `DB/` is empty, so the list below is reconstructed from those sources. The exe is a generic Softvent ERP engine; BOM, KOT, quotations, sales/purchase orders and doctor-vs-service screens are compiled in but are not part of the liquor edition, so they are left out here.

**Status key.** **Built** = interactive screen. **Partial** = screen exists, but legacy features are missing. **Stub** = `GenericModule` field list only. **Missing** = no screen.

## Masters

| Legacy screen | Status | Gaps vs legacy |
|---|---|---|
| Company Master | Built | Backup Database / Backup RPTS and "mail to Softvent" are alert stubs. Legacy also creates a new financial year (`frmMstCreateFinYear`). **Images** and **Documents** tabs upload, view, download, edit and delete real files (Administrator only; stored on disk in `backend/uploads/company/`, 8 MB limit). |
| Item Master | Built | List/New/Edit/Delete via API, Bottles/Case ≥ 1. **Import** from the template saved as CSV (duplicates in red). Missing: native .xlsx, Update Stock from Excel, barcode print, attachments. |
| Item Price List Maintenance | Built | Group filter, MRP or purchase rate, Load, change by amount with preview, Apply. |
| Item Group Master | Built | Liquor flag and Consider for Loyalty (generic `MasterCrud`). |
| UOM / Package / Brand Master | Built | Package holds litres per bottle, which drives litre totals. |
| Supplier Master | Built | — |
| Customer Master | Built | Mobile, Loyalty Card No, points, Send SMS flag (SMS itself not sent). |
| Ledger / Ledger Group Master | Built | Both in the sidebar; Expense Entry draws ledgers from here. |
| Sales Counter Master | Built | Legacy requires a counter before any Sales Invoice (the React flow already enforces this through Invoice Setup). |
| Opening Balance Entry, Holiday, Address/Contact masters | Missing | Low priority; generic engine screens. |

## Transactions

| Legacy screen | Status | Gaps vs legacy |
|---|---|---|
| Purchase Bill (adds stock) | Built | Has supplier Inv No/Date/Value, balance purchase value, QOH, cases/bottles, PB Qty/Rate/Amt, CGST/SGST/Cess/IGST, TCS, amount in words, remarks. Missing: **F4 add-single-item** dialog, **Copy From** another doc no, Location field, auto-save draft. |
| Purchase Indent | Built | Cases/bottles grid, no stock effect; copied into a Purchase Bill with **Copy From**. |
| Sales Invoice (reduces stock) | Built | Has barcode entry, +/- qty, collected amount → payback/shortage, previous bill, quick item selection, salesperson and counter, Hold/New Bill, Cancel, Delete, templates. Now also: **Re-Print Bill**, **Cancel Invoice** with mandatory remark (stock and points returned), **total litres**, loyalty **points redemption**, held bills parked on the backend (own HOLD- series). |
| Expense Entry | Built | Ledger lines, paid from Cash/Bank; cash expenses reduce the DSR cash balance. |
| Stock Deduction | Built | Reason (Breakage/Leakage/Sample/Other) and item lines; reduces stock. |
| Sales Return | Built | Optionally against a saved invoice; adds stock. |
| **Closing Stock Entry (CSE)** | Built | As-on Date → Load → closing qty → Sales Qty = OP − Closing → Save → Generate Sales Invoice. |
| **Redeem Points** (loyalty) | Missing | Settings: points per rupee, alert when accrued > N, only bills > ₹X earn points. |

## Reports

| Legacy report | Status | Gaps vs legacy |
|---|---|---|
| Customised Reports | Partial | Generate & View and Export XLS work for 28 of the 29 listed reports (`GET /reports/custom`, `backend/app/routers/custom_reports.py`). Filter On narrows by Ledger, Supplier or Item Group. Deleted Invoice Report is not available because deleted bills are not kept. Email is a stub (no mail server). Missing: **Design** (build your own report) and **Assign Rights** per report. |
| Stock Balance | Built | Has opening/receipt/issue/closing and value. Missing: litre qty, print, email as PDF. |
| **Daily Summary Report (DSR)** | Built | KPI tiles and payment/counter/salesperson/group/expense/cancelled tables; Dashboard KPIs use it. Missing: write-off split, purchase at sales rate, cash opening balance. |
| Stock Ledger | Built | Item + date range with running balance. |
| Stock Ageing | Built | As-on date and month buckets by last receipt. |
| Profit Margin | Built | Purchase vs sales value per item over a date range. |
| Excise Stock, Drops DSR, Discount Scheme, Zero-Balance Item, Package-wise reports | Missing | Crystal templates exist in `Rpts/` (`RptDropsDSR`, `rptDISCSCHM`, `RptZBItem`, `RptSalesPkg`, `RptStkPkgBal`…). |
| Account Outstanding / Ageing (bill-wise, payable & receivable) | Missing | — |

## Utilities and cross-cutting

| Legacy feature | Status | Notes |
|---|---|---|
| Global Settings | Missing | Database (backup paths); Invoice (salesperson required, print preview, print format, barcode scan → jump to Qty vs auto-add); Report paths; Loyalty. |
| Other Settings | Missing | Print settings (prompt / preview / direct / email / multi-print), Transaction settings (affects stock, allow Copy From, file attachment, PDF preview), Menu Sequence (show/hide, groups, separators, numeric shortcut codes like `201`), Doc Prefix settings. |
| Security | Partial | **Admin Master** at `/adminMaster` (Administrator only): users (password, role, active), roles and per-module Assign Rights; the sidebar and screens follow the role. Missing: field-level rights, real authentication (demo passwords, user name sent as `X-User`), server-side rights on non-admin endpoints. |
| Barcode label designer / printing | Missing | — |
| Audit log | Missing | — |
| Standard control bar and shortcuts | Missing | New Alt+N, Edit Alt+E, Delete Alt+D, Close Alt+L, first/prev/list/next/last navigation, Ctrl+Shift+X doc Excel export, Ctrl+Alt+F Copy From. Caption colours: **red = mandatory, blue = read-only, black = optional**. |
| SQL query execution, DB connection, exe update, data port to cloud | Out of scope | Backend/desktop concerns; no web UI needed. |

## Suggested order

1. Closing Stock Entry and Daily Summary Report: core daily store operations with complete specs.
2. Item Master import/export and Item Price List Maintenance.
3. ~~Purchase Indent → Purchase Bill Copy From, plus the single-item dialog~~ (done as web buttons: Add Item, Copy From; no F-key shortcuts by choice).
4. Sales Invoice Re-Print, litre totals, then loyalty (Customer fields → Redeem Points).
5. Stock Ledger, Stock Ageing, Profit Margin.
6. Settings, security and the standard control bar.

## Found in the exe but missing from the list above (added 2026-09-23)

| Legacy form | Status | Notes |
|---|---|---|
| Login (`frmLogin`) | Built | Demo users only (ADMIN/admin, GURU/guru, PRADEEP/pradeep, RAVI/ravi), FY picker, logout. No real security. |
| Cash Denomination (`frmUtlDenom`) | Built | Note/coin count vs DSR cash balance, saved per date. |
| Hourly / Daily / Brand-wise Sales (`frmRptHourlySales`, `frmRptDailySales`, `frmRptBrandWise`) | Built | `reports/` with Print and Export CSV. |
| Litre reports (`frmRptDayLtr`, `frmRptStkLtr`) | Built | Litres sold per day by LIQTYPE, stock in litres. |
| Stock Level (`frmRptStkLevel`) | Built | Items at or below ROL with suggested ROQ. |
| Cancel remarks (`frmUtlCancelInvRmks`), Hold Bill (`frmUtlHoldBill`) | Built | Part of Sales Invoice. |
| Merge Items, Compare Stock, Delete Non-Liquor Items, Calculator, on-screen keyboard, Loyalty/SMS history reports | Missing | Not requested yet. |
