# Party masters (Customer and Supplier): `frmMstParty` vs the web app, and State City Country (`frmMstSCC`)

Legacy: `source_code/Masters/frmMstParty.vb` (2,042 lines, read in full; draws Customer Master and Supplier Master from `UtlTransactions` rows of type "Party Variant") and `frmMstSCC.vb` (state / city / country master, read for its save and validation code).
Web: `routers/masters.py` (`_check_party`, `similar_codes`), `master_specs.py`, `sqlstore.py` (`_write_master`), `db_store.py`, `MasterCrud.jsx`, `SupplierMaster.jsx`, `simpleMasters.js`.
Decision (owner): follow the exe, build what is missing.

## Done in this pass (backend 699 tests; frontend build, lint, 282 tests pass)
| Legacy | Web now |
|---|---|
| **State City Country master** (`frmMstSCC`): City is the code, State chosen from State Master by name ("State Not found in Master!"), Country; Active + remarks | new master `state-city-country` (`MstSCC`), sidebar Masters → Others, role list updated; the State box offers State Master names |
| Address line 1 required: "Please enter atleast first line address!" (393) | `_check_party` |
| Group required and must exist and be active, "-" allowed: "<Customer/Supplier> Group Code NOT found in the master !" (429-436) | `_check_party` (blank refused) |
| City, Country, State each required ("Please Enter The City!" ...) and each present in State City Country ("City Not found in Master!" ...) (442-480) | `_check_party` |
| Duplicate **name** refused on Add: "<Customer> Name already exists.." (417-426) | 409 on create |
| Customer Active flag + remarks; inactive suppliers no longer dropped at load; Sales Invoice lists active customers only | `active` on Customer, loaders keep inactive rows |
| Add: leaving Code copies it to Name (2026); leaving Name copies it to the Tally alias (1634) | `MasterCrud.handleBlur` for party forms |
| City chosen: Country and State filled from the master; an unknown city asks "City Not found! Do you want to add city ??" and opens State City Country (1642-1672) | same; City / State / Country boxes suggest from the master |
| Add defaults: Country INDIA, State from the company (the exe hardcodes KARNATAKA) | `newDefaults` now also reach database columns |
| "Similar <code> already exists. Do you want to view them?" (`<Code>Clean`) (1801-1824, also frmMstGeneral) | generic `GET /masters/{path}/similar`, used by every master form except Holiday and State City Country |
| Copy From in Add (1548); close while editing asks first (156); F5 refills the lists (180); Ctrl+A opens File Attachment (191) | enabled for Customer and Supplier |
| Address to clipboard: name, address lines, city, state, PIN (1826-1859) | Options → Copy Address |
| Apostrophe turned into a back-quote on save (`stringReplace`) | `_BACKQUOTE` includes customers and suppliers |
| Delete removes the party's multi-addresses, contact persons and attachments (1193-1209; alerts and activities stay, as in the exe) | `sqlstore._write_master` |

## Already matching
Code and name required; duplicate code; inactive remarks; delete asks first and uses `UtlDeletion`; auto-generated code with series; GSTIN / currency mandatory for customers (Doc Designer); Master Approvals (exe's Approve option, by design); counts and dialogs for Multi Address, Contact Person, Alert Message, Activities, File Attachment (`PartyRecords.jsx`, `party_info.py`; each dialog still needs its own line-by-line audit: `frmMstPartyAddress`, `frmMstPartyContactPerson`, `frmMstPartyAlert`, `frmMstPartyActivities`).

## Rules apply to the screen, not a bare API call
The party checks run when the request carries the database's extra columns, as the real screen always sends them (the Purchase Bill's quick-add does not, so it can still create a supplier from name and GSTIN). A client that sends only code, name and GSTIN is not asked for an address.

## Differences kept on purpose
- State City Country is keyed by City, so the same city name cannot be entered under two states (the exe allows a city once per state and country).
- The default state follows the company's state, not the hardcoded KARNATAKA.
- Address copy does not use the `UtlPartyCopy` template (Ctrl+click on the label opens its editor in the exe).

## Open
1. Options not built yet: SMS (`frmTrnPartySMS`, when `UtlMisc.PartySMSReq` = Y), Envelope Printing (`frmUtlEnv`, when `BlnEnvPrint` = Y) and Label Print (`frmRptLblPrint`); each is a separate form to read first.
2. Ledger link: when vouchers are in use the exe fixes the ledger group (Sundry Debtors / Creditors) and writes `MstAcLed` on save (`updateLedger`); the web's voucher module has not been checked for this.
3. Party group default / lookup values depend on how the screen sends the group column; check in the browser (Claude in Chrome not connected).
4. The party photo (`LoadPartyImage`) is not shown.
