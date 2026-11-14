# Purchase Bill: `frmTrnPBM` (7,469 lines; the generic party-document form, Purchase Bill is DocID 1276) vs the web app

Web: `PurchaseBill/index.jsx`, `DocHistoryPanel.jsx`, `routers/transactions.py` (`_build_purchase_bill`, `edit_purchase_bill`, `cancel_purchase_bill`), `sqlstore.py`, `db_store.py`.
Read for this pass: `validFlds` (2104-2309), `CalTotal` (2040-2102), delete / cancel (484-536, 2760-2832), `ChkEditRights` (5942-5968), print rules, number generation (`svtModule.TrnNoAutoGen` / `getFmtDocNo`, `getUtlDocNoFmt` 6718-6774), and the shop's database (read-only): `UtlTransactions` row, `UtlDocDsgrFlds` (DocID 1276), `UtlDocNoFmt`, `UtlGSettings`, real `TrnHdrPB` rows. Parts of the form not covered yet: import from Excel, linked-document (pending quantity) logic, party alert popups, accounting transactions, Retailer Code (RTLCODE) and the rest of the grid events.

## Done in this pass (backend 706 tests, frontend lint, build, 282 tests pass)
| Legacy | Web now |
|---|---|
| Numbering: `<year digits><prefix><5 digits>` ("2600202", blank prefix; `TrnNoAutoGen` continues the highest number of the year and prefix 2762-2765), printed number from `UtlDocNoFmt` (F1..F6 = year / prefix / last five digits: `PrintPBNO` = "26//00201") | New Purchase Bill, Purchase Indent, Sales Return and Stock Deduction numbers follow it (owner chose "continue the exe's series"); the next number is read from `TrnHdrPB` etc. (`LEFT(no, year+prefix)` and `Prefix`), `PrintXNO` is written in the exe's printed shape; the Next PB No box shows 2600202; the older "26/PB-0000001" numbers already saved stay as they are. Document numbering prefix (Global Settings) may be blank |
| Delete button, CANCELLED choice (2767-2832): stock rows go, `Cancelled = 'Y'`; "Document is already cancelled", "Current record is cancelled. Deletion not possible!" (493), "Stock Is going negative. Cannot Save." (2787), `ReWriteINPROGRESS` block (485), freeze date | `POST /transactions/purchase-bill/{no}/cancel`, **Cancel Bill** in the action bar (confirm "Are you sure to CANCEL this Document ?"); saved as `Cancelled = 'Y'` |
| Cancelled bills are listed as cancelled; a cancelled bill cannot be edited: "Current record is cancelled. Cannot edit!" (5950) | loader reads `Cancelled`; edit refused |
| Bill total must equal the invoice value only when `CheckPBValue` = Y (2184-2191; this shop: N) | the save gate follows the setting (the green Balance box still shows) |
| Print blocked while a stock re-write runs (291, 335, 393) | `/print/render` for purchase bills too |

## Already matching
Duplicate item rows (designer "check duplicate"), qty > 0 and qty / rate mandatory (designer `PBQTY`, `PBRATE`), formulas (`PBAMT = PBQTY x PBRATE`, `CGSTAMT = PBAMT x CGSTPER / 100`, `NETAMT`, `TOTNET`, `GRNDTOT = TOT + OTHAMT`), supplier needed, stock added by `move_stock`, edit rewrites header and lines and takes the old stock back, freeze date and edit limit, Master Approvals.

## Differences kept on purpose
- Supplier Inv No, Inv Date and Inv Value are required on the web screen (the owner's rule: items are entered only after them); the designer marks none of them mandatory.
- Cancel only (no Delete): the exe's Delete removes the bill's rows from the shop's tables; the web never does. `TrnHdrPB` has no `IsDelete` column, so a soft delete cannot be kept either.
- A cancel stores no reason (the exe asks none; `TrnHdrPB` has no `CancelRmks`).

## Open
1. Delete (hard) not built, see above.
2. Items 2 of the not-covered list: import from Excel (`frmTrnPBM` 4600-4780, 7380-7420), pending-quantity link to Purchase Indent, party alert message (5814), Retailer Code check (7093), "Balance is going negative" (5290), `Party Selection Alert`.
3. `TrnHdrPB.Prefix` / `PartyCMob` / address columns the exe fills from the supplier (`PartyAdd1..4`, `PartyCity`, `PartyState`, `PartyPinCode`) are not written by the web (verify against a bill the exe saved).
