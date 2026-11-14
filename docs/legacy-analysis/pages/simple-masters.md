# Simple masters: `frmMstGeneral`, `frmMstUOM`, `frmMstCurr`, `frmMstHoliday` vs the web app

Legacy (read in full unless noted): `source_code/Masters/frmMstGeneral.vb` (1,331 lines), `frmMstHoliday.vb` (559), `frmUtlGrpOrderBy.vb`, `frmUtlPkgOrderBy.vb`; `frmMstUOM.vb` and `frmMstCurr.vb` were compared by their save, validation and key-press code (they are copies of `frmMstGeneral` plus the fields named below).
`frmMstGeneral` draws every master the menu marks `TrnDocType = General` (`frmMain.OpenGeneralForm`): Item Group, Brand, Package, Supplier / Customer Group, State, Location, Tax, Bank, Currency (its own `frmMstCurr` is only used from `getMasterFlds`).
Web: `routers/masters.py`, `master_specs.py`, `schemas.py`, `db_store.py`, `sqlstore.py`, `MasterCrud.jsx`, `Masters.jsx`, `ArrangeOrderDialog.jsx`, `simpleMasters.js`, `masterConfigs.js`.
Decision (owner): follow the exe, build what is missing.

## Done in this pass (backend 695 tests, frontend build / lint / 282 tests pass)
| Legacy | Web now |
|---|---|
| Inactive check box + remarks on every general master (`chkInactive`, 333) | `active` on Item Group, Brand, UOM, Package (loaders keep inactive rows now; savers write `InActive`); remarks rule is the shared one; pick-lists offer active rows only (exe combos use `InActive = 'N'`) |
| UOM `Decimals` whole number (frmMstUOM 341-374, 1089) | `decimals` typed field (read with `_optional`, saved to `MstUOM.Decimals`) |
| Currency: Decimal Name required "Please enter the Decimal Name!", Exchange Rate required (frmMstCurr 311-318, `ExRate` saved) | `exchange_rate` added (`ExRate`), Decimal Name required (server), Exchange Rate required (screen) |
| Edit refused for a brand in use: "Editing is not available for this masters" (frmMstGeneral 891-897, `Editable`) | `PUT /masters/brands/{code}` answers 409 with that text when items use the brand |
| `stringReplace`: apostrophe becomes a back-quote in every text box on save (3852) | `_backquote` for these masters (names, codes, extras) |
| Holiday: date within the financial year ("Holiday Date Should be within the financial year", 195-202); holiday name may not be a number ("Please Enter holiday in words", 285-290); date locked when editing; delete also needs a date in the year (95-98) | all four rules in `_clean` / update / delete |
| Options → Arrange Group Slno / Arrange Pkg Slno (`frmUtlGrpOrderBy`, `frmUtlPkgOrderBy`): list with Slno, Move Up / Down, Save writes `GroupOrderBy` / `PkgOrderBy`; packages also Ascending / Descending by litres; Export to Excel | `GET/POST /masters/{item-groups\|packages}/order`, `ArrangeOrderDialog` from each master's Options |

## Already matching
Code and name required; duplicate code refused; delete asks first and uses the desktop's `UtlDeletion` rules; auto-generated codes with series; attachments; First / Previous / Next / Last; rights per screen; Master Approvals replace the exe's Approve option (by design).

## Differences kept on purpose
- The Slno grid leaves out the dummy row `ID 0` ("-") the exe lists.
- Arrange Slno saves in one transaction (the exe updates row by row).
- Calendar view of `frmMstHoliday` (month grids, bold dates, tooltip) is not drawn; the Holiday master is the same list as the other masters (date = code). Month / date filter and the search box of that screen are covered by the List window. Say if you want the calendar.
- The exchange-rate box accepts any number; the exe's key filter allowing one decimal point is the browser's number box.

## Open
1. Browser check (Claude in Chrome not connected): the Arrange dialog and the new fields need a look against the shop database; Arrange Slno only works with the real database.
2. `Item Group` / `Package` order is saved but reports and pickers do not sort by it yet (the exe uses `GroupOrderBy` / `PkgOrderBy` in its own reports; each report is audited on its own page).
3. `frmMstSCC` (State City Country) is a separate screen, still to audit.
