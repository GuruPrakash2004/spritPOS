# Sales Invoice: legacy `frmTrnInv` vs the web app

Legacy: `source_code/Transaction/frmTrnInv.vb` (4,492 lines) + `frmTrnInv.designer.vb`, opened by the menu item "Sales Invoice" (`frmMain.vb:1099`, `FRMINV`).
`frmTrnPosInv` is a different menu entry (`FRMTRPOINTINV`) and is not this screen.
Web: `frontend/src/components/transactions/SalesInvoice*.jsx`, `backend/app/routers/transactions.py`, `app/sqlstore.py`, `app/store.py`.

Method: line ranges of the VB were extracted by three readers and the web side by a fourth; line numbers below are `frmTrnInv.vb:line` unless stated.
Rows marked **verify** were reported by a reader and must be re-read at the line before code is changed.
Items driven by the Doc Designer (`UtlDocDsgrFlds`, the detail/total columns and formulas) need a read-only SELECT on the shop's DB to confirm; they are listed under "Needs DB check".

Legend: **Match**, **Differs** (both exist, behave differently), **Missing** (legacy has it, web does not), **By design** (web differs on purpose, documented in CLAUDE.md).

## A. Save, numbering and tables

| # | Legacy | Web | Status |
|---|---|---|---|
| A1 | Invoice no = `getInvNo` (2917): `max(InvCNo)+1` for `FinYear`; `InvNoReset` D (per day) / M (per month) / blank; `CounterWsRst`=Y numbers per counter (`SCID`); format `Invoice/DocNoFmt` tokens `<<1>>`..`<<8>>`, default `<<5>>/<<6>>`, month/day prefixes `PrefixSettings`; `//`→`/`, `--`→`-` | `FY/<counter prefix>-0000001`, one sequence for all counters (`store.py:195`), settings above not read | Differs |
| A2 | Header insert writes `FINYEAR`, `splitInvRef`, `SINVNO`, `InvCNo`, `SCID`, `BILLSYSTEM`=PC name, `SPID`, `SINVDate`, `SINVDATETIME`, `CUSTID`, `PartyName`=customer **code**, `PartyCMob`, `AddedBy/On`, `RCVDAMT`, `PAYBACKAMT` + designer fields (3290-3317) | Writes most; not `FINYEAR`, `InvCNo`, `splitInvRef`, `PartyCMob`; `BILLSYSTEM`='WEB' | Differs |
| A3 | Detail writes `SINVID, SlNo, ITEMID, UOMID, ItemSlNO, STKQTY, SINVQtyInLtrs` + designer columns (rate, amount, discount, GST...) | Writes a fixed list; line discount, `CGSTPER`, `SGSTPER`, `DISCAMT`, `DISCPER` are 0 although the screen shows them | Differs |
| A4 | Stock: rows into `UtlStockList` (`StkMultiplier` -1, `DocType` 'INV', Qty, LtrQty, Rate); negative stock stops the save through a DB CHECK: "Stock Is going negative. Cannot Save." (title "No Stock") | `store.move_stock` + own check "Insufficient stock for X..." | Differs (messages; verify rows written) |
| A5 | One transaction (ReadCommitted) from header to audit; any failure rolls back and leaves the screen as is | Same idea, 503 and undo | Match |
| A6 | Edit (Mode M): deletes `TrnDtlSInv`, `TrnDtlSInvDenom`, `UtlStockList` rows, updates header only for SPID/ChangedBy/ChangedOn + designer fields; not date, customer, RCVDAMT, PAYBACKAMT; checks the **Add** right | Rewrites header, lines, points, denominations, multi-amount; checks **edit** right | Differs |
| A7 | `PAYBACKAMT` stored from the text: "Shortage by X" is stored as 0 (parse quirk) | Shortage not stored either | Match |
| A8 | `RCVDAMT` box accepts digits only (3790) | Decimals allowed | Differs |
| A9 | `UpdtSInvRate` to Item Master when `RateFld`='Y' | `_rate_to_item` | Match |
| A10 | Denominations: `TrnDtlSINVDenom (SINVID, CurName, Nos, Value)`; reload sets Rcvd = sum of Value | Written; Coins rows store the rupee amount in Nos | Differs (verify) |
| A11 | Multi-amount designer field -> `TrnMultiAmt` (Slno, Particulars, Amount) | UPI / CREDIT parts only | Partial |
| A12 | After save: QR images (`strQRUrl`, `UPIurl`/`APPROVEURL`) saved to `QRImage`, `QRImageCheck` columns and files | Not built | Missing |
| A13 | `ReWriteINPROGRESS`='Y' blocks Print: "re-Writing Stocks.. Try after few mins." | Not checked | Missing |
| A14 | Counter-wise guard: if `gCheckCounter` and not `gCounterWiseFmt`: "Counter wise reset enabled in global settings!..." opens Global Settings | n/a until A1 | Missing |

## B. Loyalty points

| # | Legacy | Web | Status |
|---|---|---|---|
| B1 | Balance = `sum(InvPoints - RPoints)` from `vwredeem` per customer | Customer `points` field | Differs (verify) |
| B2 | Earn base = amount of lines whose item group has `Loyalty='Y'` only; earns when that amount >= `gdblBillValueGreaterThan`; points = `Round(amt * PointsForOneRs, 0)` | Whole bill net, `MinBillForPoints`, 2 decimals; item-group flag not used | Differs |
| B3 | Redeem is automatic in blocks of `AlertIfPointsGreaterThan` points: `floor(points/block)*block*RsForOnePoint`, capped by the loyalty amount | Cashier types an amount | Differs |
| B4 | Redeem written as `TRNHDRRDM` row (RDMNO from doc 1284, `GREMARKS`=invoice no, `RPOINTS`=`CASHAMT`=value); re-save deletes the old row | No redeem row | Missing |
| B5 | `TrnPoints` row (DocDate, CustID, SInvID, SInvNo, InvAmt=loyalty amount, Points) | Written (net amount) | Differs |
| B6 | Points box turns LightGreen when points > `AlertIfPointsGreaterThan` | Shown if > 0 | Differs |
| B7 | Mail: subject "<company> Reward Points" via `MstCust.EMAILID` when `sendSms`='Y'; SMS texts (hard-coded "SVS Drops") | SMS only, shop name | Partial |
| B8 | Message "Redeem Points exceeds Invoice Value ..." | Different text | Differs |

## C. Rules on the bill

| # | Legacy | Web | Status |
|---|---|---|---|
| C1 | Sales person typed as **code**; "Invalid Sales Person Code..." (3080); last SPID remembered in the INI; `validFlds`: "Please select sales person" | Select list | Differs |
| C2 | Doc date editable only by user `ADMIN` and only if `AllowEditDocDate` | `allow_edit_date` setting | Differs |
| C3 | Date outside the financial year: long message ending "Please Take a Database Backup Before Doing Above Steps", opens Company Master | Date must be today | Differs |
| C4 | Stock check on save: "Stock is Less... Cannot Proceed.." (not when negative stock allowed); "Qty is negative... Cannot Proceed.."; "Qty cannot Be zero or less than One...." | Own texts | Differs (texts) |
| C5 | Grid QOH on a saved bill = current QOH + this line's own qty | Check | Verify |
| C6 | Litre limit: strict `>` against `AllowBeerInLtr/WineInLtr/IMLinLtr`; ignored when `SPLITINV`=Y; labels turn red; `qty` truncated to integer | Matches in spirit; message "{Beer} quantity exceeds the limit of N Ltr" vs legacy "Beer quantity exceeds more than N Ltr" | Differs (texts) |
| C7 | Split (`SPLITINV`): every unit exploded, packed into bills <= limit, `splitInvRef`=ticks, each bill saved and printed with its own number; Rcvd on each bill; "Prev bill" shows "<n> SPLIT Bills. TotalAmt" | Split exists; no `splitInvRef`; split payment refused | Differs |
| C8 | Item list: active, `NoSelInvoice<>'Y'`, authorised; item must have a package row | Check | Verify |
| C9 | Scan: `ItemCode`, then alias view `vwItemAlias`, `HotKey`; "Item not found!!"; QR `SVTQRCODE` + `code,qty;...`; `+`/`-` on the row; duplicate item adds qty | Present | Match (verify texts) |
| C10 | Customer: search by code/name/phone, F4 list; with loyalty: card no, then phone; "Please enter either card number or Phone Number" | Customer picker | Differs |
| C11 | Cancel/Delete dialog `frmUtlDelCancelDoc`: CANCELLED or DELETED (hard delete of header, lines, stock); "Document is already cancelled"; confirmations; SMS on delete/cancel (`gBlnSMSInvDeleted/Cancelled`); F4 Delete disabled for DB `VLQMSDROPSJP` | Cancel only (reason mandatory) | Differs |
| C12 | Edit/delete limits: `blnFreezeDoc`, cancelled, `AllowEditDeleteForDays` (messages "Document Cannot edit Since it is freezed!", "Current record is cancelled. Cannot edit!", "Edit limit days crossed... Cannot edit!"); delete before freeze date: "Document cannot be Deleted. Date is before Freezed Date (Global Settings)!!" | `check_not_frozen`, `edit_days` | Differs (texts, logic inverted in legacy: replicate) |
| C13 | Fraud audit on edit (`gBlnCheckForFraud`): `UtlAdmAudit` + `UtlAdmAuditItem` rows with Added/Edited/Deleted per item | `admin_audit.record` | Differs (verify tables) |
| C14 | Exit while adding/editing: "Are you sure you want to exit the form without saving?" | Draft restore | Differs |
| C15 | Company rule "SVS DROPS"/"SVS WINES": INVVALUE = CASHAMT - POINTSRDMD | n/a | Not applicable |

## D. Screen, keys and extras

| # | Legacy | Web | Status |
|---|---|---|---|
| D1 | Keys: F12 Save, Ctrl+F12 and Alt+F12 (each Save / Save&Print / Save&Preview from `INVF12`, `INVCTRLF12`, `INVALTF12` = S/SP/SPP); F2 Re-Print, F3 Cancel, F4 Delete, F5 Hold/New bill, F6 Load Template, F7 Clear, F9 Close, F10 Tender box, Ctrl+Shift+D/K, Ctrl+T toggle columns, Ctrl+G switch grid, Ctrl+F8/F9 save/restore layout, F7 in grid freezes columns | F2 search, F3 quick pick, F4 customer, F6 payment, F7 hold, F8 held, F9 cash, F10 save&print, Alt+R/C/T | By design (owner's keyboard set in CLAUDE.md); needs decision |
| D2 | Hold = opens a new Sales Invoice tab; nothing stored | Held bills stored in a file | By design |
| D3 | Print: `BLOCKPRINT` (system names never print), "Do you want to print the invoice...?" when `strPrintPrompt`='P', `NOOFCOPIES` from `UtlOption` default row | `after_save` setting | Differs |
| D4 | Header "Total Bills: n & Sales: x" for today after each save (`SHOWSALESVALUE` or user ADMIN) | Today totals endpoint exists; show on screen? | Verify |
| D5 | "Prev Bill. <no> Time: <t> Amt: <v>" for this counter's last bill; double-click reprints it | Last-bill line (last 7 days) | Match (verify double-click) |
| D6 | Templates `MstItemTemp`; Quick selection `MstQuickSelItems` and last set in INI | Alt+T, F3 | Match |
| D7 | `ShowInvVal`='Y' shows a second "Inv Val" box | Not built | Missing (minor) |
| D8 | Auto-save grid XML every 10 s, restore, purge after 1 day; `UtlHoldBill` purge | localStorage draft | Match |
| D9 | Counter chosen from PC name; "Sales Counter Not Defined..., Do you want to create new?" | Same | Match |
| D10 | `chkHdrFlds`: warns when designer fields sit in the Header section | n/a | Not applicable |
| D11 | Auto-restart after 100 bills (memory workaround) | n/a | Not applicable |

## Needs DB check (read-only SELECT against the shop's database)
- `UtlDocDsgrFlds` rows for the Sales Invoice DocID: the real detail columns (rate, discount, GST, amount), total rows (`TOT`, `CASHAMT`, `POINTSRDMD`, `INVVALUE`, `TenderChangeFld`) and their formulas and rounding, to settle A2, A3 and the rupee rounding.
- `UtlTransactions` row (freeze, edit days, edit doc date, rights), `UtlOption` rows (F12 modes, copies, default print), `UtlGSettings` keys listed above, and the `vwredeem` / `vwItemAlias` view definitions.

## Proposed fix groups (for approval)
1. **Numbering and header columns** (A1, A2, A14): token format, D/M reset, per-counter option, `InvCNo`, `FINYEAR`, `PartyCMob`, `splitInvRef`.
2. **Loyalty** (B1-B6, B8): earn on loyalty-group items, whole-point rounding, auto-redeem blocks, `TRNHDRRDM` row, balance from `vwredeem`.
3. **Detail columns** (A3, A10): write line discount and tax percentages as the designer defines.
4. **Messages and checks** (A4, A13, C4-C6, C12): exact texts, `ReWriteINPROGRESS`, edit/delete limits.
5. **Cancel/Delete choice** (C11) and **print rules** (D3).
6. **Extras** (A12 QR images, B7 mail, C1 sales-person code entry, C10 card/phone lookup, D7).
Keys (D1) and hold (D2) stay as the owner designed them unless told otherwise.

## Progress (2026-11-14)
Approved by the owner: all four fix groups. Done so far:
- **Group 4, messages and checks (done):** C4 stock ("Stock is Less... Cannot Proceed.." + item and figures), zero qty ("Qty cannot Be zero or less than One...."), C6 litre limits (Beer "exceeds more than", Wine / IML "exceeds limit more than"), redeem above invoice value ("Redeem Points exceeds Invoice Value"), cancelled bill ("Current record is cancelled. Cannot edit!", "Document is already cancelled"), edit limit ("Edit limit days crossed... Cannot edit!" + detail), A13 print blocked while `UtlMisc.ReWriteINPROGRESS = 'Y'` ("re-Writing Stocks.. Try after few mins."). The exe's edit-limit test is inverted (2097); the web keeps its own correct direction and only the text follows the exe.
- **Group 5, Delete (done):** C11 Cancel Bill dialog now has Delete Bill ("Are you sure to DELETE this Document ?"); `POST /transactions/sales-invoice/{doc_no}/delete`: stock and points go back, the bill is saved as `Cancelled='Y'`, `IsDelete='Y'` (status DELETED). Kept different from the exe on purpose: no rows are removed from the shop's tables. Refused before the Freeze Date with the exe's text.
- **Still open:** group 1 numbering (A1, A14), group 2 loyalty (B1-B6, B8), group 3 detail / header columns (A2, A3, A10), print rules (D3), C3 date text, C1 / C10 entry changes. These need a read-only look at the shop's `UtlDocDsgrFlds`, `UtlGSettings` and `vwredeem` first.

## What the shop's database showed (read-only SELECTs, 2026-11-14)
- `UtlGSettings`: `DocNoFmt` = `<<5>>/,<<2>>-,<<7>>` (year digits, counter prefix, 7-digit running number; commas dropped), `InvNoReset` blank, `CounterWsRst` N, `PrefixSettings` empty, `SPLITINV` blank, `INVF12` SP, `INVCTRLF12` S, `INVALTF12` SPP, `KeepDelData` N, `BlockPrint` blank. Loyalty: `BillValueGreaterThan` 1500, `PointsForOneRs` 0.01, `RsForOnePoint` 1, `AlertIfPointsGreaterThan` 50000. `TrnPoints` has no rows: this shop does not use loyalty.
- **A1 Match for this shop:** the web's `26/c10-0083754` is exactly that format with one sequence for all counters. The D / M / per-counter resets and other tokens are not set here and are not built.
- **A3 Match:** the web already writes every detail column the designer defines (`SINVAmt`, `CGSTPER/AMT`, `SGSTPER/AMT`, `MRP`, `DISCAMT`, `BASICRATE`, `SUBTOT`, `GSTAMT`, `ITEMAMT`, `NETTOT`, `ITEMSELRATE`, `TOTITEMSELRATE`, ...); formulas are the designer's (`CGSTAMT = TOTITEMSELRATE * CGSTPER / 100`, `INVVALUE = SVTROUND50(TOT - WRITEOFFAMT - TOTDISCAMT)`).
- **A2 Differs, fixed:** web bills had `FINYEAR` and `InvCNo` NULL; the exe's `getInvNo` takes `max(InvCNo) + 1` per `FinYear`, so it could not see web bills and could hand out a number the web already used. New web bills now write `FINYEAR` (26) and `InvCNo` (the running number). Bills saved before this change keep NULL: back-filling live rows was not done (owner's call). `PartyCMob` (blank) and `SplitInvRef` stay as they were.
- **B2 Differs, fixed for shops with the setting:** the web used its own `MinBillForPoints` (default 1000, strictly above) and the whole bill with two decimals. When the shop's `BillValueGreaterThan` row exists the web now follows the exe: only lines of "consider for loyalty" item groups count, at or above the value, `Round(amount x PointsForOneRs, 0)` whole points. Other loyalty parts (B1 balance view `vwredeem`, auto-redeem in blocks of `AlertIfPointsGreaterThan`, the `TRNHDRRDM` row) are not built: the shop has no points rows, and the `vwredeem` definition could not be read.
- **D1 settings:** F12 = Save & Print, Ctrl+F12 = Save, Alt+F12 = Save, Print & Preview in the exe's settings; the web keeps the owner's keyboard set.
