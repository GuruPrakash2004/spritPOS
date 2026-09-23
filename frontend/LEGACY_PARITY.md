# Legacy vLQMS → web UI parity

What the legacy Windows app in `MAIN_Root/vLQMSAPP` does, and how much of it the React prototype in `frontend/VLQM` covers.

**Sources.** `Docs/VLQMS Help Doc.pdf` (41-page user manual), the form classes and captions compiled into `App/vLQMS.exe`, `Docs/WebDash.xlsx` (DSR / web dashboard spec), `Docs/vLQMS Closing Stock Entry screen Creation.xlsx`, `Docs/ItemImport_Template.xlsx.xlsx`, `Docs/vLQMS_Loyalty Points/`, the Crystal Reports in `Rpts/`, and an auto-saved Purchase Bill draft in `App/AutoSave/`.

The legacy menu itself lives in the `MstMenu` SQL table, and `DB/` is empty, so the list below is reconstructed from those sources. The exe is a generic Softvent ERP engine; BOM, KOT, quotations, sales/purchase orders and doctor-vs-service screens are compiled in but are not part of the liquor edition, so they are left out here.

**Status key.** **Built** = interactive screen. **Partial** = screen exists, but legacy features are missing. **Stub** = `GenericModule` field list only. **Missing** = no screen.

## Masters

| Legacy screen | Status | Gaps vs legacy |
|---|---|---|
| Company Master | Built | Backup Database / Backup RPTS and "mail to Softvent" are alert stubs. Legacy also creates a new financial year (`frmMstCreateFinYear`). |
| Item Master | Partial | Browse-only (prev/next). Missing: **Import Item from Excel** (download template → load → duplicates in red → import), **Update Stock from Excel**, barcode print, file attachment, Bottles/Case ≥ 1 rule. Template columns: ITEMCODE, ITEMNAME, SHORTNAME, HSNCODE, ITEMGROUPCode, LIQTYPE, NONSTKABLE, UOMCode, MAX/MINLEVEL, CGSTPER, SGSTPER, BOTPERCASES, PKGITEM, PKGCode, MRP, PURRATE, ROL, ROQ. |
| Item Price List Maintenance | Missing | Select by item group or item, choose rate columns, **Load**, bulk update rate by amount, **Apply**. |
| Item Group Master | Stub | Needs "Consider for Loyalty" and liquor-item flag behaviour. |
| UOM / Package / Brand Master | Stub | Package master needs litres-per-bottle (drives the litre totals in reports). |
| Supplier Master | Built | — |
| Customer Master | Stub | Loyalty adds Mobile No, **Loyalty Card Number** and **Send SMS to customer**. |
| Ledger / Ledger Group Master | Stub | Ledger Group Master has no sidebar entry. Expense Entry draws its ledgers from here. |
| Sales Counter Master | Built | Legacy requires a counter before any Sales Invoice (the React flow already enforces this through Invoice Setup). |
| Opening Balance Entry, Holiday, Address/Contact masters | Missing | Low priority; generic engine screens. |

## Transactions

| Legacy screen | Status | Gaps vs legacy |
|---|---|---|
| Purchase Bill (adds stock) | Built | Has supplier Inv No/Date/Value, balance purchase value, QOH, cases/bottles, PB Qty/Rate/Amt, CGST/SGST/Cess/IGST, TCS, amount in words, remarks. Missing: **F4 add-single-item** dialog, **Copy From** another doc no, Location field, auto-save draft. |
| Purchase Indent | Stub | Legacy: does not affect stock; its lines are copied into a Purchase Bill via Copy From. Cases/bottles read-only, PB Qty/Rate mandatory. |
| Sales Invoice (reduces stock) | Built | Has barcode entry, +/- qty, collected amount → payback/shortage, previous bill, quick item selection, salesperson and counter, Hold/New Bill, Cancel, Delete, templates. Missing: **Re-Print Bill**, **total litres** in the footer grid, "print with prompt" after save, loyalty points redemption (`POINTSRDMD`). |
| Expense Entry | Partial | Read-only sample rows, no inputs. Legacy posts amounts against ledgers from Ledger Master. |
| Stock Deduction | Partial | One read-only sample line (breakage), no inputs. |
| Sales Return | Stub | — |
| **Closing Stock Entry (CSE)** | Missing | Pick As-on Date → loads liquor items with opening balance → enter physical closing stock → Sales Qty = OP − Closing → **Generate Sales Invoice**. |
| **Redeem Points** (loyalty) | Missing | Settings: points per rupee, alert when accrued > N, only bills > ₹X earn points. |

## Reports

| Legacy report | Status | Gaps vs legacy |
|---|---|---|
| Customised Reports | Partial | Catalogue and filters are built (`ReportsDashboard.tsx`). View / Print / Email are stubs. Missing: **Design** (build your own report) and **Assign Rights** per report. |
| Stock Balance | Built | Has opening/receipt/issue/closing and value. Missing: litre qty, print, email as PDF. |
| **Daily Summary Report (DSR)** | Missing | The spec in `WebDash.xlsx` covers: total bills; sales incl. GST; liquor and non-liquor basic; counter-wise; profit of the day; opening and closing stock qty/value; cash, card, other, credit and write-off; purchase (at sales rate, basic, incl. GST); sales returns; cash opening balance, expenses and cash balance; redemption. Plus tables for payment modes, counter-wise sales, cancelled invoices, salesperson-wise, group-wise sales and purchase, and expense ledger-wise. The React Dashboard KPIs should be aligned to this. |
| Stock Ledger | Missing | Item + date range → purchases and sales movement. |
| Stock Ageing | Missing | As-on date and month range. |
| Profit Margin | Missing | Purchase rate/value vs sales qty/rate/value over a date range. |
| Excise Stock, Drops DSR, Discount Scheme, Zero-Balance Item, Package-wise reports | Missing | Crystal templates exist in `Rpts/` (`RptDropsDSR`, `rptDISCSCHM`, `RptZBItem`, `RptSalesPkg`, `RptStkPkgBal`…). |
| Account Outstanding / Ageing (bill-wise, payable & receivable) | Missing | — |

## Utilities and cross-cutting

| Legacy feature | Status | Notes |
|---|---|---|
| Global Settings | Missing | Database (backup paths); Invoice (salesperson required, print preview, print format, barcode scan → jump to Qty vs auto-add); Report paths; Loyalty. |
| Other Settings | Missing | Print settings (prompt / preview / direct / email / multi-print), Transaction settings (affects stock, allow Copy From, file attachment, PDF preview), Menu Sequence (show/hide, groups, separators, numeric shortcut codes like `201`), Doc Prefix settings. |
| Security | Missing | Users, roles, Assign Rights, field-level rights. |
| Barcode label designer / printing | Missing | — |
| Audit log | Missing | — |
| Standard control bar and shortcuts | Missing | New Alt+N, Edit Alt+E, Delete Alt+D, Close Alt+L, first/prev/list/next/last navigation, Ctrl+Shift+X doc Excel export, Ctrl+Alt+F Copy From. Caption colours: **red = mandatory, blue = read-only, black = optional**. |
| SQL query execution, DB connection, exe update, data port to cloud | Out of scope | Backend/desktop concerns; no web UI needed. |

## Suggested order

1. Closing Stock Entry and Daily Summary Report: core daily store operations with complete specs.
2. Item Master import/export and Item Price List Maintenance.
3. Purchase Indent → Purchase Bill Copy From, plus the F4 single-item dialog.
4. Sales Invoice Re-Print, litre totals, then loyalty (Customer fields → Redeem Points).
5. Stock Ledger, Stock Ageing, Profit Margin.
6. Settings, security and the standard control bar.
