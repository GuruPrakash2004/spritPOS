# Item Master: legacy `frmMstItem` vs the web app

Legacy: `source_code/Masters/frmMstItem.vb` (2,511 lines, read in full) plus `frmMstItem.designer.vb` (item type list), `svtModule.vb` (`blnDeleteOK`, `getMasterFlds`), `frmRPtInvalidItems.vb`, `frmUtlImportAnyFld.vb`. Line numbers are `frmMstItem.vb:line`.
Web: `ItemMaster.jsx`, `ItemOptionDialogs.jsx`, `MasterCrud.jsx`, `masterConfigs.js`; `routers/masters.py` (`_check_item*`, item option routes), `schemas.py`, `sqlstore.py`, `db_store.py`, `store.py` (`recode_item`).
Decision (owner): follow the exe everywhere and build everything that was missing.

Status: **Done** (changed to match the exe, tested), **Match**, **By design**, **Open** (needs the shop database or a browser check).

## Done in this pass (backend tests: 689 pass; frontend build, lint, 282 tests pass)
| Legacy | Web now |
|---|---|
| Duplicate checks (578-616): code and aliases against other items' code, alias, short name, hot key; alias = code refused; alias no apostrophe, 30 chars | `_check_item_names`, same semantics (short name / hot key of the item itself are not compared, as in the exe) |
| Bottles per case: any number (blank 0) (674) | `ge=0`; label no longer says ≥ 1; loader keeps 0 |
| Short name saved as typed (661) | blank no longer becomes the name |
| Item may be re-coded after bills (no check) | allowed; `store.recode_item` moves stock, movements, last purchase and bill lines to the new code |
| Group list = active **and authorised** (440); UOM list excludes '-' (441) | refused with the exe's texts; `db_store` reads `Authorised` / `InActive` of groups |
| Inactive item needs remarks always (562) | 422 "Please enter remarks for why the item is inactive!"; cleared when active |
| Edit: UOM box disabled when on any `TrnDtl*` (1507-1515) | `GET /items/{code}/locks`; UOM select disabled in Edit; server still refuses a change (409) |
| Add: leaving Item Code copies it to Item Name; leaving Item Name copies it to HotKey (1856, 2185) | `MasterCrud.handleBlur` (item form) |
| "Similar ItemCode already exists. Do you want to view them?" (1973-1999, `ItemCodeClean`) | `GET /items/similar` (database function, or the same comparison in memory) + list dialog |
| Copy From in Add mode (1626-1646) | Options → Copy From (Add mode only) |
| Buttons beside Item Group / UOM open that master (2127, 2141) | "Open … Master" links (opens the screen; it does not jump to the chosen record) |
| Apostrophe refused in every item box (many `KeyPress`) | stripped on input in the item form |
| Find box looks in code, name, short name, hot key (1268-1274) | `GET /masters/items?q=` also matches short name, hot key, aliases |
| Close while adding / editing asks "Are you sure you want to exit the form without saving?!" (126) | `closeScreen` (item form) |
| F5 refills group / UOM lists (147); Ctrl+A opens File Attachment (163) | both; Ctrl+A only outside a text box (so text can still be selected) |
| Options menu in the exe's order (1916-1970) | Items-DataLink Problem, Import Item from Excel, (Download Template), Update Stock from Excel, Item Vs Brand, Update Item Master Fields, Update Item Rates, Print Barcode |
| Items-DataLink Problem: `select * from vwInvalidItems order by 1,2`, export (`frmRPtInvalidItems`) | `GET /items/invalid` + dialog with Export to Excel; says so when the view is missing |
| Update Item Master Fields (`frmUtlImportAnyFld`): fields from `UtlItemBulkupdate`, filters (groups / packing / items), Template, Load to Grid, pick file ("Please Select a valid Template!" check), edit grid, Import Data, Export | `GET /items/bulk-fields`, `POST /items/bulk-fields/load`, `POST /items/bulk-fields/import`, `UpdateItemFieldsDialog`. Differences: one transaction (the exe's is commented out); a code in an ID column that doesn't exist is refused instead of storing 0; cost needs cost access |
| Update Item Rates: recompute every Item Master formula field (2440-2506). The exe's SQL ends `where  and ItemID>0` (2499) and fails, so it never worked | `POST /items/recalculate-rates` does the intended update; `ALTER TABLE` for UITCCGST / UITCSGST is not done (web never changes table design), NULLs are set to 0 |

## Already matching (checked in source)
Required code, name, group, type, UOM; type list Wine / Beer / IML / Others (designer line 605); group / UOM must exist; duplicate code; duplicate name allowed; margin rule `PurSellVarPer` (not for OTHERS); aliases delete-and-reinsert in the save transaction; MRP / PURRATE change writes `MstItemAudit`; one transaction with rollback; delete uses the desktop's `UtlDeletion` rules (`sqlstore._check_unused`, same "Cannot Delete" meaning) and removes the `TrnQOH` rows; File Attachment; First / Previous / Next / Last.

## By design (kept, documented)
- Opening stock field on New items (logged as an `opening` movement).
- Approve option replaced by Master Approvals in Admin Master (the exe's body is commented out).
- Layout save / restore keys (Ctrl+F8 / F9) not built.
- The exe's per-user "Options" right (opt1Rights) has no web equivalent; the web uses Item Master edit right for the option screens.

## Still open
1. **Doc Designer rules inside the extra-fields grid** (`vsfHdr`: formula, combo, pick-up, conditional hide / read-only, `[MST]` double-click): handled by shared modules (`DsgrModule.vb`) used by every master, so they belong to a shared "designer engine" audit (`designer-engine-functional-spec.md`). Not an Item Master gap on its own.
2. `Update Stock from Excel` (`frmUpdateItemStock`), `Print Barcode` (`frmUtlBarCodePrint`), `Import Item from Excel` (`frmImportItemXLData`), `Item Vs Brand`: the menu entries now open the web equivalents, each still needs its own page audit (Utilities).
3. **Browser check** of the new dialogs and key behaviour against the shop database (Claude in Chrome was not connected in this session). Items-DataLink, Update Item Master Fields and Update Item Rates only work with the real database.
4. Fraud audit text (`gBlnCheckForFraud`, 732-769) is written by the web's own ADMIN audit entries; the exact wording is not reproduced.
