# Feature parity audit: desktop `App` (vLQMS.exe) vs the web app

Generated and updated 2026-10-07 from the shop's own `MstMenu` table (the menu `vLQMS.exe` builds at start with `frmMain.AddMenusFromDB`)
compared with `frontend/src/lib/modules.js`, `App.jsx`, the report registry and the backend routers. A menu row is "shown" when
`MenuShow = 'Y'` in this shop. Status is by screen; inside-screen details are tracked in `frontend/LEGACY_PARITY.md`.

## Result
- Menu items: **83**; shown in this shop: **42**.
- Shown items missing from the web app: **1**: Database Archive (deliberately not built: it deletes live data).
- Hidden (not shown in this shop) items still missing: **31** (vouchers, quotation, accounting reports, short-close, authorise, party-vs-item and others; the accounting tables are empty here).

## Checklist: what is still to build
- [x] Supplier Group, Customer Group, State, Currency, Location, Tax, Bank, Holiday masters (built 2026-10-07, desktop tables `MstSUPPGROUP`, `MstCUSTGROUP`, `MstSTATE`, `MstCUR`, `MstLOC`, `MstTAX`, `MstBANK`, `MstHoliday`; no new table needed)
- [ ] Database Archive (`1724`, Utilities, shown): NOT built on purpose. The exe's strings show it archives the database to `<db>_Archive_<suffix>` and moves transactions before a date into archive tables; it deletes live data, so it needs the owner's go-ahead and a tested restore first
- [x] Bulk Master Update (`1725`): items, fields HSN, brand, liquor type, bottles per case, MRP, purchase rate, ROL, ROQ
- [x] Label Printing (Code 128 item labels through the browser); the label designer (`frmUtlBCDesigner`) and `.prn` templates are not built
- [x] Package-wise Sales, Package-wise Purchase, Loyalty Report, Excise Stock
- [ ] Still missing reports: Account Outstanding / Ageing and General Ledger (accounting tables are empty in this shop), Drops DSR / Petty cash / Purchase, SMS history, Stock Ref, Invalid items
- [ ] Opening Balance entry (`MstOpBalance`), Party address / contact / alert / activity masters, Customer vs Item, Supplier vs Item: tables exist and are empty here; their use by the desktop is unread, so not guessed
- [ ] SMS and e-mail sending from documents (`frmTrnSMS`, `frmTrnEmail`, `frmUtlSendSMS`, `frmUtlAutoEmailSch`): e-mail exists only for Customised Reports
- [ ] Hidden in this shop (optional): Quotation, Physical Stock Entry, Payment / Receipt / Contra / Journal vouchers, Credit / Debit note, Short-close, Authorise Documents, Post Doc Entry, Shared Documents
- [ ] Check which of these exist: bulk import from Excel (`frmUtlImportItemFmExcel`, `frmUtlImportXL`), Update from Tally, Restore
- [ ] Desktop conveniences: Calculator, on-screen keyboard, button hotkeys, voice commands, webcam (`frmUtlCalci`, `frmUtlKeyBoard`, `frmUtlBtnHotkeys`, `frmUtlWebCam`)
- [ ] Desktop look-and-feel settings (doc/invoice number format, form colours, field captions, tooltips): the web keeps its own UI, not planned

## Not built on purpose
- Licence key / registration (`frmKeyReg`, `clsReg`, `SSPLLic.dll`) and the desktop's Exit item, as you asked.

## Full menu (MstMenu, 83 rows)
| Menu item | Group | Shown | Web app |
|---|---|---|---|
| Dashboard | - | Yes | Built |
| Company Master | COMPANY DETAILS | Yes | Built |
| Item Master | ITEM RELATED | Yes | Built |
| Item Group Master | ITEM RELATED | Yes | Built |
| Item Price Maintenance Master | ITEM RELATED | No | Built (Price List Maintenance) |
| UOM Master | ITEM RELATED | Yes | Built |
| Customer Master | CUSTOMER RELATED | Yes | Built |
| Customer Group Master | CUSTOMER RELATED | No | Built |
| Customer Vs Item Master | CUSTOMER RELATED | No | MISSING |
| Supplier Master | SUPPLIER RELATED | Yes | Built |
| Supplier Group Master | SUPPLIER RELATED | Yes | Built |
| Supplier Vs Item Master | SUPPLIER RELATED | No | MISSING |
| State Master | OTHERS | No | Built |
| Currency Master | OTHERS | No | Built |
| Sales Person Master | OTHERS | Yes | Built |
| Location Master | OTHERS | No | Built |
| Ledger Master | OTHERS | Yes | Built |
| Ledger Group Master | OTHERS | Yes | Built |
| State City Country | OTHERS | No | MISSING |
| Tax Master | OTHERS | No | Built |
| Bank Master | OTHERS | No | Built |
| Role Master | OTHERS | No | MISSING |
| Company Settings... | OTHERS | No | MISSING |
| Sales Counter Master | OTHERS | Yes | Built |
| Package Master | OTHERS | Yes | Built |
| Brand Master | OTHERS | Yes | Built |
| Sales Invoice | - | Yes | Built |
| Purchase Bill | - | Yes | Built |
| Expense Entry | - | Yes | Built |
| Stock Deduction | - | Yes | Built |
| Purchase Indent | - | Yes | Built |
| Sales Return | - | Yes | Built |
| Quotation | - | No | MISSING |
| Physical Stock Entry | - | No | MISSING |
| Payment Voucher | - | No | MISSING |
| Receipt Voucher | - | No | MISSING |
| Contra Voucher | - | No | MISSING |
| Journal Voucher | - | No | MISSING |
| Credit Note | - | No | MISSING |
| Debit Note | - | No | MISSING |
| Audit Trail | - | No | MISSING |
| Customised Reports... | - | Yes | Built (Reports Dashboard, run from the desktop's own report definitions) |
| Label Printing | - | No | Built |
| PDF Generator | - | No | MISSING |
| Stock Balance Report | - | Yes | Built |
| Stock Ledger Report | - | Yes | Built |
| Stock Ageing Report | - | Yes | Built |
| Stock Level Report | - | Yes | Built |
| Daily Summary Report | - | Yes | Built |
| Package Wise Sales Report | - | No | Built |
| Package Wise Purchase Report | - | No | Built |
| Brand Wise Report | - | Yes | Built |
| Profit Margin Report | - | Yes | Built |
| Bill wise Ageing | - | No | MISSING |
| Bill wise OutStanding | - | No | MISSING |
| Party wise OutStanding | - | No | MISSING |
| General Ledger | - | No | MISSING |
| SQL Query Execution | - | Yes | Built |
| Bulk Update | - | No | MISSING |
| SQL Saved Query | - | No | MISSING |
| AuditLog Sync | - | No | MISSING |
| Re Write Stock | - | Yes | Built |
| Split Data | - | No | MISSING |
| Download Latest Exe | - | Yes | Left out on purpose (desktop self-update) |
| Security | - | Yes | Built (Admin Master (Security)) |
| Audit Log | - | No | MISSING |
| Global Settings... | - | Yes | Built (Global Settings (+ Print/Other Settings tabs)) |
| Database Connectivity | - | Yes | Built (Global Settings → Database tab and the sign-in Database setup) |
| ShortClose Documents | - | No | MISSING |
| Authorise Documents | - | No | MISSING |
| Post Doc Entry | - | No | MISSING |
| Shared Documents | - | No | MISSING |
| Data Port To Cloud (203) | - | Yes | Built (Web Sync) |
| Barcode Layout Format | - | No | MISSING |
| Barcode Ammendment | - | No | MISSING |
| Voice Command Alternatives | - | No | MISSING |
| Remove Duplicate Invoice | - | Yes | Built |
| Replace and Remove Duplicate items | - | Yes | Built |
| Merge Items | - | Yes | Built |
| Compare Stock | - | Yes | Built |
| ADMIN Audit Log | - | Yes | Built |
| Database Archive | - | Yes | MISSING |
| Bulk Master Update | - | Yes | Built |

## Where the web app goes beyond the desktop
Cash Denomination, Hourly / Daily / Brand-wise sales, litre reports, Dashboard restock carousel, shop open/closed chip, keyboard-first billing,
Admin Master field rights per role, Web Sync to the central site, per-user print preference, SQL-backed audit log.

## Notes
- Hidden menu items are not shown in this shop's desktop, so building them is optional.
- The desktop's other ~150 forms are dialogs, pickers and designers inside the screens above, not separate features.
- Inside each built screen, field-level differences are listed in `frontend/LEGACY_PARITY.md` (not re-audited here).
