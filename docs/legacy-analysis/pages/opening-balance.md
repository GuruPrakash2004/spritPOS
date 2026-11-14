# Item Opening Balance: `frmMstOPBalEntry` (1,316 lines, read in full) vs the web app

Menu caption `Item Opening Balance` (`MstMenu`); not in this shop's menu.

## What the form does
One record at a time (item, location, receipt date, receipt ref, qty, rate, remarks) for the active financial year (`MstOPBalance.FinYear`), listed with search per column and Export to Excel; Add / double-click to edit / Remove Row; rights Add / Edit / Delete.
Rules: location chosen (`LocCode`, active only) 708-728; item chosen and active (442-447, 730-782); receipt date required, month ≤ 12 and year 1901-2098 (741-771); Qty > 0 (749); Rate > 0 (757); one balance per item and location: "OP Balance available for the item...." (785-793); rate × qty = value (237, 614).
Save (801-888): in one transaction, an edit first writes a reversing `TrnStkFlow` row and deletes the old `MstOPBalance` row; inserts the new `MstOPBalance` row and a `TrnStkFlow` row; rebuilds `TrnQOH.TrnQty` from `TrnStkFlow` and `CLBal = OPQty + TrnQty`; "Stock Is going negative. Cannot Save." on the check constraint. Remove Row (947-1019) does the same reversal.

## Status: not built (owner to confirm)
The liquor edition no longer keeps stock in `TrnStkFlow`: its bills write `UtlStockList` and `TrnQOH` (the web's stock model and `stock_carry_forward.py` rebuild `TrnQOH` from `UtlStockList`; the `TrnStkFlow` code in `frmTrnInv.vb` is commented out, only `frmTrnPosInv`, `frmTrnMemorandum` and `frmUtlMergeItems` still mention it). Writing `TrnStkFlow` rows from the web would make `TrnQOH` disagree with `UtlStockList`. The web's equivalents are the item's opening stock (set when the item is created, `store.move_stock` "opening" movement) and Company Master → Financial Year(s) → Carry forward stock (writes `MstOpBalance` + `UtlStockList` + `TrnQOH` as the exe's `StkCarryForward` does).
If the owner wants the screen anyway it should be built on `UtlStockList` (`DocType = 'OP'`, as `StkCarryForward` 153-171), not `TrnStkFlow`.
