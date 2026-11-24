# Remaining work (as of 2026-10-31)

Tick a box when it is done and checked. Order inside each section is priority order.

## 1. Before going live
- [ ] (Not done: Chrome could not open the local dev server, so this still needs a person or a working browser session.) Click through every new screen in a browser with real data: Payment / Receipt / Credit Note / Debit Note / Contra / Journal Voucher, Quotation, General Ledger, Party wise Outstanding, Bill wise Outstanding, Bill wise Ageing, Physical Stock Entry, Update Item Stock, Item Vs Brand, Shared Documents, Delete Non-Liquor Items, Drops reports (Sales Summary, Purchase, Petty Cash, Consolidated), Loyalty Detail, Day's OP-Balance.
- [x] Save for real (not fake or rolled back) on a client database and check the rows (2026-10-31; BW, DROPSJP, LHOUSE; every one wrote its desktop rows, moved stock, created no `Web_` table and came back after a reload):
  - [x] Quotation (BW, NDrops or NDROPSBLDR)
  - [x] Expense Entry and Day's OP-Balance (a DROPS client)
  - [x] Purchase Indent, Sales Return, Stock Deduction, Physical Stock
  - [x] Master saves (item, customer, supplier, brand ...) in direct mode
  - [x] Sales Invoice with a split payment (BW) and a salesperson at LHOUSE (refused without one, saved with one). The loyalty customer part could not be tested: none of the 14 databases has a loyalty card customer or any `TrnPoints` row.
  - [x] Purchase Return
- [ ] Try the Crystal prints (Sales Invoice, DSR, Stock Balance) in direct mode; they write temporary rows into the desktop's work tables.
- [ ] Review and commit the changes to git (about 50 files).
- [ ] Deployment: back up each client database, put the connection in `backend/.env` only, HTTPS for the password encryption, run the 14-database check again on the final build.

## 2. Things that cannot save yet (refused with a message)
- [x] Global Settings: now saved to the shop's own `UtlGSettings` (value, caption, hide flag; new settings added as rows) and the desktop-only rows (98 in BW) are shown too. Checked on BW with a real edit and revert.
- [ ] Admin Master: rights, roles, field rights, master approvals, report rights, login settings
- [ ] Number series, Menu Sequence, import field maps
- [ ] Re Write Stock repair
- [ ] New users (the desktop keeps the password encrypted)
- [ ] Closing Stock Entry on clients without `TrnHdrCSE` (only NDROPSBLDR has it)
- [x] Held bills now survive a restart: the desktop keeps a held bill's lines in an XML file on its own PC (the database only has a name row), so the web app keeps them in `backend/data/held_bills_<database>.json` (git-ignored, nothing written to the database). Checked on BW: hold, file written, restart restored it, discard removed it, stock untouched.

## 3. Missing features
- [x] Vouchers: edit and cancel (2026-10-31, checked on BW). Edit rewrites the header, lines and ledger postings and keeps the number; cancel flags the header `Cancelled = 'Y'` only, as the exe does (the ledger postings stay, as in the exe). Still open: bill-wise adjustments (`TrnDtlAccAdj`, `…BillDet`) and the exe's Delete.
- [x] Quotation: edit and cancel (2026-10-31, checked on BW)
- [ ] Screens a client's menu shows that the web does not have: Split Data, Audit Trail, PDF Generator, Audit Log (reads a separate log database), Short Close, SMS History, Auto E-mail Scheduler
- [ ] Import Price Data (held back: only the restaurant Quick Service Bill uses price categories; build only if a shop needs them)
- [ ] Cash denominations saved per invoice (`TrnDtlSinvDenom`: SINVID, CurName, Nos, Value; 8 clients have data). The exe writes them from the cash-tendered panel of the POS invoice. Needs a counting panel on the Sales Invoice screen: not done, because a billing-screen change needs checking in a browser.
- [ ] Redeem Points screen; Discount Scheme and Zero-Balance reports have no logic in the exe (Crystal templates only)
- [x] Mandatory fields: Customer GSTIN and Currency are built (2026-10-31, checked on AKSHAYA: saved to `MstCUST.GSTNO` / `CURID`, read back after a reload; the walk-in CASH customer is exempt and the desktop's `-` counts as a GSTIN). Sales Invoice Location: every web bill is saved with Main Stock (LOCID 1), the only location in 10 of the 14 databases. Still open: the 4 databases with a second location (`Mumbai Wear House` in dvg, dvgLiqStore, LMDVG, SLSDVG) need a location choice and per-location stock on the bill, which is not built.
- [ ] Doc Designer features: combo, pick-up, unique checks, auto-generated codes
- [x] Item rate-change history report (built 2026-10-31 as `Item Rate History`, from `MstItemAudit`; checked on BW: 176 changes). Original note: (`MstItemAudit` has 380 to 4,475 rows in every client; the exe has no screen for it, so this would be new)
- [ ] Database Archive: deliberately not built (it deletes live data)

## 4. Performance (client databases, not code faults)
- [ ] `TrnHdrSINV` has no index on `SINVDate`: the first 7-day sales read takes 3 to 17 seconds on the biggest clients. Needs your decision before any database is changed.
- [ ] Re Write Stock checks take 50 to 290 seconds on the largest clients.

## 5. Cleanup on the test server
- [ ] BW holds my test rows: voucher `26//00001`, purchase bill `26/PB-0000001` (supplier invoice WEBTEST1), cancelled invoice `26/SR-0003377`. Delete only with your say-so.
- [ ] Small ID gaps in `TrnHdrQTN` and `TrnHdrEXP` from rolled-back tests (harmless).
- [ ] ADMIN's password was changed in all 14 databases to the agreed one; the old values are in the scratchpad file `admin_pwd_backup.json` if they must be restored.

## 5a. Editing saved bills (2026-11-02)
- [x] Sales Invoice and Purchase Bill: List → Edit Selected loads a saved bill, Save updates it in place (same number). Checked on BW with real writes: new bill edited twice, a bill that exists only as database rows edited (after a reload), a purchase bill the same way; header, lines, stock rows and TrnQOH all followed.
- [x] Found and fixed on the way: after any reload in direct mode the next stock moves were not written to `UtlStockList` (the counter of written moves was never reset). Bills read from the database were thin (customer always CASH, no supplier invoice no., purchase type always Purchase); they now carry the real header, on all 14 databases.
- [x] Purchase Indent, Sales Return, Stock Deduction and Expense Entry can be edited the same way (2026-11-02): `PUT /transactions/purchase-indent|sales-return|stock-deduction|expense/{doc_no}`, List → Edit Selected. A return or deduction takes its old stock rows back first and is refused when the change would leave an item below zero; an expense edit keeps the petty-cash receipt lines. Covered by backend tests (memory) and frontend tests. **Not yet checked against a real database**: the test server was unreachable when this was finished; `scratchpad/real_editdocs.py` is ready to run on BW (and an expense edit on a DROPS client).
- [x] Loyalty edge cases tested in memory: a bill that redeemed and earned points edited (points settled from the start), a failed edit leaves points alone, a bill moved to another customer settles both. Not testable on real data: no client has loyalty customers, and in direct mode customers' points are not read back from the database (they start at 0 after a restart).
- [ ] Not editable: Closing Stock and Physical Stock (they create other documents: an invoice, a purchase bill / deduction). A purchase edit is refused when the stock the old bill added was sold since. Held bills are changed on the counter (recall).
- [ ] Not looked at in a browser (Chrome could not reach the local server).

## 5b. Action bar and List window (2026-11-01)
- [x] Floating bar and List / Select Columns windows on every master (`MasterCrud`), the document screens (`WithHistory`), the six vouchers, Quotation, Closing Stock, Physical Stock, Cash Denomination and Company Master. Tested with component tests; **not yet looked at in a browser** (Chrome could not reach the local server), so check the look once.
- [ ] Not on: Price List Maintenance and the report / utility screens (they have no New / Edit / Delete).

## 6. Housekeeping
- [x] The 9 backend and 5 frontend tests that were failing are fixed (they tested behaviour the owner had since changed: plain passwords accepted, settings open from the sign-in page, no year picker, a recent-days history). Backend 606 passed, frontend all passed.
- [x] `docs/architecture/db-and-save.md` now starts with a direct-mode section (the older `Web_` text is kept below it, marked as the older mode); `.claude/CLAUDE.md` already did.
- [x] Held-bill and refused-change behaviour is explained for shop staff in `docs/DEPLOYMENT_AND_STAFF_NOTES.md`, which also holds the deployment checklist.
