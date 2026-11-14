# Purchase Indent (`FRMTRNPI`), Sales Return (`frmTrnSR`), Stock Deduction (`frmTrnSTKDED`) vs the web app

Legacy: Purchase Indent opens `frmTrnPBM` (the Purchase Bill form, `frmMain.vb:1109`), Sales Return `frmTrnSR.vb` (6,403 lines), Stock Deduction `frmTrnSTKDED.vb` (6,403): the same generic party-document form as the Purchase Bill, so the rules in `purchase-bill.md` apply. Messages, `deleteDoc` (SR 2600-2665, SD 2596-2661), `ChkEditRights` (SR 5430-5450, SD 5426-5446) and the validations were compared; the shop's database (read-only): `UtlTransactions` rows (DocID 1280 / 1281 / 1279), the three `UtlDocDsgrFlds` definitions, real rows (`TrnHdrSR` 1, `TrnHdrSTKDED` 1, `TrnHdrPI` 0: this shop hardly uses them).
Web: `routers/transactions.py` (`create_indent_doc`, `_build_sales_return`, `_build_stock_deduction`, `_edit_stock_doc`, `_cancel_stock_doc`), `sqlstore.py`, `db_store.py`, `DocHistoryPanel.jsx`.

## Done in this pass (backend 710 tests; frontend lint, build, 282 tests pass)
| Legacy | Web now |
|---|---|
| Delete button, CANCELLED choice: stock rows go, `Cancelled = 'Y'` (a return's added stock comes off, a deduction's goes back, an indent moves none); "Current record is cancelled. Deletion not possible!", "Stock Is going negative. Cannot Save." (unless `AllowNegative` = Y), `ReWriteINPROGRESS` block, freeze date | `POST /transactions/{purchase-indent\|sales-return\|stock-deduction}/{no}/cancel`; **Cancel** in the action bar of the three screens (confirm "Are you sure to CANCEL this Document ?"); saved as `Cancelled = 'Y'` |
| A cancelled document cannot be edited: "Current record is cancelled. Cannot edit!" | already enforced (`_editable`), now reachable after a cancel |
| Numbers as the exe (`2600002`, printed `26//00002`) | done with the Purchase Bill; documents read from the database are now known by that number (they showed as `26//00001`) |
| Sales Return header carries the sales counter and sales person (`SCID`, `SPID`) | taken from the invoice the return is against |

## Already matching
Qty > 0, qty and rate mandatory per the designer, `SRAMT = SRQTY x SRRATE`, `SRQTYINLTRS = SRQTY x LTRPERBOT`, `STKDEDQTYINLTRS`, `PIAMT = PIQTY x PIRATE`, `GRNDTOT = TOTAMT + OTHAMT`; stock added by a return / removed by a deduction through `move_stock`; stock cannot go below zero ("Please enter the QTY Less than QOH" is the exe's text; the web stops with its own); edit rewrites in place and takes the old stock back; freeze date and edit limit; a return cannot exceed what its invoice sold.

## Differences kept on purpose
- Delete (hard) is not built: the web never removes rows from the shop's tables (same as Purchase Bill).
- No reason is stored on a cancel (the exe asks none).
- Stock Deduction's counter (`SCID`) and sales person: the web screen has no counter, so they stay empty (the exe fills the PC's counter).

## Open
1. Hard Delete (see above); `TrnHdrSR` / `TrnHdrSTKDED` / `TrnHdrPI` have no `IsDelete` column.
2. Linked-document logic (pending quantity of an indent copied into a bill, "Qty cannot more than Pending Qty", `CumQty`), Excel import, party alert message, accounting variant: not covered.
3. Messages "Please enter the qty greater then 0" / "Please enter the QTY Less than QOH" are shown by the web screens in their own words.
