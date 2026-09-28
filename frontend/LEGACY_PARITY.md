# Legacy vLQMS → web UI parity

What the legacy Windows app (`MAIN_Root/vLQMSAPP`, not in this repo) does, and how much of it the web app (React in `frontend/`, FastAPI in `backend/`) covers.

**Sources.** `Docs/VLQMS Help Doc.pdf` (41-page user manual), the form classes and captions compiled into `App/vLQMS.exe`, `Docs/WebDash.xlsx` (DSR / web dashboard spec), `Docs/vLQMS Closing Stock Entry screen Creation.xlsx`, `Docs/ItemImport_Template.xlsx.xlsx`, `Docs/vLQMS_Loyalty Points/`, the Crystal Reports in `Rpts/` (copied to `rpts_files/Rpts/` and `backend/Rpts/`), and an auto-saved Purchase Bill draft in `App/AutoSave/`. `Docs/` and `App/` are not in this repo.

The legacy menu itself lives in the `MstMenu` SQL table of the office database, so the list below is reconstructed from those sources.

**Data.** The backend can load masters and recent invoices/bills from the legacy SQL Server (`vLQMSLHOUSE`, office LAN only), but every write stays in memory and is lost on restart. Outside the office it runs on seed data. "Built" below describes the screen, not persistence. The exe is a generic Softvent ERP engine; BOM, KOT, quotations, sales/purchase orders and doctor-vs-service screens are compiled in but are not part of the liquor edition, so they are left out here.

**Status key.** **Built** = interactive screen. **Partial** = screen exists, but legacy features are missing. **Missing** = no screen.

## Masters

| Legacy screen | Status | Gaps vs legacy |
|---|---|---|
| Company Master | Partial | Save and file upload/edit/delete are Administrator only. Company name, email, registered address and additional company identifiers load from the API (from `MstComp` when the DB is connected); the dashboard, app shell, sign-in, invoice reprint and report export use this profile. Backup Database / Backup RPTS and "mail to Softvent" are alert stubs. Legacy also creates a new financial year (`frmMstCreateFinYear`). **Images** and **Documents** tabs upload, view, download, edit and delete real files (Administrator only; stored on disk in `backend/uploads/company/`, 8 MB limit). |
| Item Master | Built | List/New/Edit/Delete via API, Bottles/Case ≥ 1. QOH is opening stock, entered only for a new item (logged in the stock ledger); afterwards stock changes only through transactions. Items on bills or with stock movements can't be deleted (mark them inactive). Group, package, UOM and brand must exist; import checks every row first and imports nothing if any row is invalid. Cashiers can add and edit but not delete (blocked in the UI and with a 403 from the API). **Import** from the template saved as CSV (duplicates in red). Missing: native .xlsx, Update Stock from Excel, barcode print, attachments. |
| Item Price List Maintenance | Built | Group filter, MRP or purchase rate, Load, change by amount with preview, Apply. |
| Item Group Master | Built | Liquor flag and Consider for Loyalty (generic `MasterCrud`). |
| UOM / Package / Brand Master | Built | Package holds litres per bottle, which drives litre totals. |
| Supplier Master | Built | Legacy-style form saved through `/masters/suppliers`: pick a supplier to edit (PUT) or New to add one (POST), with code, name, Tally alias, address, city, pincode, state, country, phone, GSTIN, currency and Active. Duplicate codes are caught inline. With a DB connected only code, name, GSTIN and city are loaded from it. Missing: delete, and saving to SQL Server (like every master, changes last until the backend restarts). |
| Customer Master | Built | Mobile, Loyalty Card No, points, Send SMS flag (SMS itself not sent). |
| Ledger / Ledger Group Master | Built | Both in the sidebar; Expense Entry draws ledgers from here. |
| Sales Counter Master | Built | Legacy requires a counter before any Sales Invoice (the React flow already enforces this through Invoice Setup). |
| Opening Balance Entry, Holiday, Address/Contact masters | Missing | Low priority; generic engine screens. |

## Transactions

| Legacy screen | Status | Gaps vs legacy |
|---|---|---|
| Purchase Bill (adds stock) | Built | Items from Item Master (live), opens blank, CGST/SGST from Item Master (read-only), Refresh QOH reloads real stock; a Purchase Return can't take stock below zero. Has supplier Inv No/Date/Value, balance purchase value, QOH, cases/bottles, PB Qty/Rate/Amt, CGST/SGST/Cess/IGST, TCS, amount in words, remarks. Missing: **F4 add-single-item** dialog, **Copy From** another doc no, Location field, auto-save draft. |
| Purchase Indent | Built | Cases/bottles grid, no stock effect; copied into a Purchase Bill with **Copy From**. |
| Sales Invoice (reduces stock) | Built | Items from Item Master (live; scan the item code), bills open empty, stock limit from real QOH, total rounded to the rupee like the saved bill, Save & Print prints the saved bill. The server refuses overselling (same item on several lines), negative quantities/rates/points, discounts above the bill and payments that don't add up. Has barcode entry, +/- qty, collected amount → payback/shortage, previous bill, quick item selection, salesperson and counter, Hold/New Bill, Cancel, Delete, templates. Now also: **Re-Print Bill**, **Cancel Invoice** with mandatory remark (stock and points returned), **total litres**, loyalty **points redemption**, held bills parked on the backend (own HOLD- series). |
| Expense Entry | Built | Ledger lines, paid from Cash/Bank; cash expenses reduce the DSR cash balance. |
| Stock Deduction | Built | Reason (Breakage/Leakage/Sample/Other) and item lines; reduces stock. |
| Sales Return | Built | Optionally against a saved invoice; adds stock. |
| **Closing Stock Entry (CSE)** | Built | As-on Date → Load → closing qty → Sales Qty = OP − Closing → Save → Generate Sales Invoice. |
| **Redeem Points** (loyalty) | Missing | Settings: points per rupee, alert when accrued > N, only bills > ₹X earn points. |

## Reports

| Legacy report | Status | Gaps vs legacy |
|---|---|---|
| Customised Reports | Partial | Generate & View and Export XLS work for 28 of the 29 listed reports (`GET /reports/custom`, `backend/app/routers/custom_reports.py`). Filter On narrows by Ledger, Supplier or Item Group. Deleted Invoice Report is not available because deleted bills are not kept. Email sends the report as an .xls attachment (the backend builds it from the same report and filters and sends it through the SMTP server in `backend/.env`; without one it says email isn't set up). Missing: **Design** (build your own report) and **Assign Rights** per report. |
| Stock Balance | Built | Has opening/receipt/issue/closing and value, and prints through the format picker. Missing: litre qty, email as PDF. |
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
| Security | Partial | **Admin Master** at `/adminMaster` (Administrator only): users (password, role, active), roles and per-module Assign Rights; the sidebar and screens follow the role. Login issues a JWT (24 h). Passwords are salted PBKDF2 hashes; 5 wrong passwords lock a user for 15 min; a password change or deactivation signs the user out; every write endpoint checks the role's modules on the server. Add/Edit/Delete ticks are saved and enforced by the server; users change their own password (header key button); roles without purchase/cost modules don't see purchase rates, stock value at cost or profit; users, roles and all web-app data survive a restart (local `backend/data` file). Optional write-back copies web documents and stock changes to separate `Web_*` tables in SQL Server (`DB_WRITE_ENABLED`, `backend/migrations/`); the legacy tables are not written, so the desktop app still doesn't see web bills. Missing: field-level rights beyond costs, writing into the legacy tables. |
| Print formats | Partial | `PrintFormatSelectorModal` lists the legacy `.rpt` formats per document (`print/rptRegistry.ts`) and previews them, but **the web layouts are not copies of the `.rpt` designs**: see `rpts_files/RPT_LAYOUT_AUDIT.md` (every mapped format differs; 4 registry entries have the wrong paper size; `fmtSInv_DMP` points at no real file). 11 of the 36 `.rpt` files can be read with `backend/tools/rpt_inspect.py` (text, fields, sections, sizes; not positions); 25 are encrypted. Exact reproduction needs PDF exports from the desktop app. |
| Barcode label designer / printing | Missing | — |
| Audit log | Missing | — |
| Standard control bar and shortcuts | Partial | Web shortcuts exist: ⌘/Ctrl+K or `/` module search, `?` shortcut list, ⌘/Ctrl+S save, Enter save, Esc cancel, ⌘/Ctrl+N or Alt+N new, and arrow-key movement in `MasterCrud` (Company and Supplier Master have save shortcuts). Missing legacy set elsewhere: New Alt+N, Edit Alt+E, Delete Alt+D, Close Alt+L, first/prev/list/next/last navigation, Ctrl+Shift+X doc Excel export, Ctrl+Alt+F Copy From. Caption colours: **red = mandatory, blue = read-only, black = optional**. |
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
| Login (`frmLogin`) | Built | Demo users (ADMIN/admin, GURU/guru, PRADEEP/pradeep, RAVI/ravi) plus `MstUser` users when the DB is connected (password = lowercase user code), FY picker, JWT, logout. A page reload logs out. See Security above. |
| Cash Denomination (`frmUtlDenom`) | Built | Note/coin count vs DSR cash balance, saved per date. |
| Hourly / Daily / Brand-wise Sales (`frmRptHourlySales`, `frmRptDailySales`, `frmRptBrandWise`) | Built | `reports/` with Print and Export CSV. |
| Litre reports (`frmRptDayLtr`, `frmRptStkLtr`) | Built | Litres sold per day by LIQTYPE, stock in litres. |
| Stock Level (`frmRptStkLevel`) | Built | Items at or below ROL with suggested ROQ. The Dashboard's "Restock immediately" carousel goes further: `/reports/restock` ranks items by sales speed and days of stock left (fast sellers running out first) and suggests two weeks' cover. |
| Cancel remarks (`frmUtlCancelInvRmks`), Hold Bill (`frmUtlHoldBill`) | Built | Part of Sales Invoice. |
| Merge Items, Compare Stock, Delete Non-Liquor Items, Calculator, on-screen keyboard, Loyalty/SMS history reports | Missing | Not requested yet. |
