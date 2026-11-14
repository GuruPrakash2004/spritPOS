# Company Master: `frmMstComp` (+ `frmSelFinYear`, `frmMstCreateFinYear`) vs the web app

Legacy: `source_code/Masters/frmMstComp.vb` (1,443 lines, read in full), `frmSelFinYear.vb` (179, full), `frmMstCreateFinYear.vb` (258, full). `frmMstNewComp` (443), `frmMstGlobeComp` (368), `frmMstSelComp` (123) also read in full (multi-company / database screens).
Web: `CompanyMaster.jsx`, `CompanyFiles.jsx`, `routers/masters.py` (`/masters/company`, `/company/fin-years`), `routers/company_files.py`, `schemas.Company`.
Decision (owner): follow the exe, build what is missing.

## Done in this pass (backend 701 tests; frontend lint, build, 282 tests pass)
| Legacy | Web now |
|---|---|
| Only the company name is mandatory: "Company Name Should not be blank!!!" (251-257) | name required; address and e-mail no longer forced (the Doc Designer can still mark them mandatory) |
| E-mail checked only when typed: "EmailID is not valid!!" (259-266) | server and screen |
| Box lengths: name 250, address rows and e-mail 100 (`vsfCDET_SetupEditor`, 789-805) | schema + `maxLength` |
| Apostrophe becomes a back-quote in every box on save (223-244) | `update_company` |
| Document needs a group: "Group name cannot be empty!" (1084); same file name refused: "Document already exists!" (1094-1106) | `company_files.upload_file`, dialog |

## Already matching
Company grid rows Company Name / Address / "." / ".." / Email = `CompName, Address1-3, EMAIL` (87-92, 270-281); Doc Designer extras in `MstComp` (`GetUpdateString`); Financial Year list in date order; create year refused when its start lies inside an existing year: "Financial Year already exists!" (397-403), created inactive (410); remove only the first or last year, never the active one, one year must stay (424-444); select the active year (`frmSelFinYear.btnSave`: all N, chosen Y, ADMIN only); Dashboard link update (1426-1435); Backup (`BACKUP DATABASE` to `SQLBACKUPPATH`, 863-893); documents list, add, edit, delete, export (1219-1277).

## Differences kept on purpose
- Images and Documents live in `backend/uploads/company` (any number of images, up to 8 MB, jpg/png/webp/gif) instead of `MstComp.jpgFile` (one jpg, 5 MB) and `MstCompDocs` + `SharedDocs`. The web never writes to a table or folder of the client's.
- Documents: Edit changes title / group / remarks only (as the exe changes group and remarks only).
- `frmMstCreateFinYear` (carry-forward routine) is never called by the exe's menu (the carry-forward is commented out); the web's "Carry forward stock" is the Stock Balance routine.

## Open
1. Backup menu items: Truncate Log, Backup and Mail (own address / Softvent), Backup RPTS (+ mail) (895-1003). Only plain Backup exists.
2. Dashboard link is shown always; the exe shows it only when `UtlMisc.DashBoardReq = 'Y'` (206-209).
3. Alias button (`frmUtlAlias`), Companies... (`frmMstGlobeComp`), Select Company, F5 refresh, Ctrl+F8 / Ctrl+F9 layout save: not built / not applicable.

## Multi-company screens (read in full, not built, owner to confirm)
| Legacy | Status |
|---|---|
| `frmMstSelComp`: pick a company, `USE <db>`, reload settings, remember the DB in the INI (26-43). Its list loader is commented out (9-24) | By design: the web talks to one shop database, chosen once in the sign-in page's Database setup |
| `frmMstGlobeComp` (Companies... button): list / add / edit / delete rows of `<globe>..MstGlobeComp`; Add creates a SQL database and restores a `.bak` into it (323-352); Delete backs the database up then `DROP DATABASE` (301-311); validations: name and database name required, letters and digits only, database / company must be new (163-209) | Not built: creating, restoring and dropping SQL Server databases is outside the web's rule of never changing the client's server structure; the database is switched in Database setup |
| `frmMstNewComp`: wizard that copies an Access `.MDB` template (`SVTFactIC.Mdb`, `SvtDemo.MDB`, `C<n>.MDB`) and inserts a first financial year | Dead code (Access era, never reached from the SQL Server menu); not built |
