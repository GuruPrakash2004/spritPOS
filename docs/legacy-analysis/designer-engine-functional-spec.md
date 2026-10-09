# vLQMS.exe: how the Doc Designer runtime works (functional spec, from the decompiled `DsgrModule`)

Source: `App\vLQMS.exe` decompiled with ILSpy (kept outside the repository). This note records **behaviour only**; no desktop
code is copied. Where the web app now follows it, the file is named.

## 1. Where the fields come from
- A screen asks the database view `vwAddnlFldsHFTSec` = `SELECT * FROM UtlDocDsgrFlds WHERE FldSection <> 'Detail'`, joined to
  `UtlTransactions` by `DocID`, **one grid per `SubSectionName`** (a tab per sub-section; the first is titled "Header Info" when
  the name is blank), rows ordered by `FldOrderBy`, one `SELECT` per section (Header, Footer, Total). Detail columns come from
  `vwAddnlFldsDetailSec`. Everything that is in the table is a row of the grid; nothing about a designer field is in code.
- A combo field's caption gets the suffix ` [Mst]`.
- Hidden rows are rows with `FldBlnHide = Y` (and no condition, or the condition true). `FldClrBefHide = Y` also clears the
  row's value when it is hidden.
- Web: `app/schema_meta.py` (`build_screen`, `_field`), `hooks/useScreenFields.js`, `lib/designerFields.js`.

## 2. Colours and flags
| Flag | Desktop | Web |
|---|---|---|
| Mandatory (`FldBlnMandatory`) | caption **red** | red caption + `*` (`lib/styles.js fieldTone`) |
| Read-only (`FldBlnReadOnly`) | caption **blue**, cannot be edited | blue caption and value |
| Hide (`FLdBlnHide`) | row not shown | not drawn |
| Display only (`FldBlnDispOnly`) | shown, never saved | read-only |

**Conditions** (`FldHide*`, `FldMndtry*`, `FldRO*`: CompareVal / Operator / WithVal): the **operator is the switch**. A flag that
is Y with an *empty* operator holds always. With an operator it holds only while `compare <operator> with` is true. A condition on
a flag that is N is ignored. Web: `schema_meta._rule` / `_field` (`hidden` / `mandatory` / `readonly` mean "always", `*_when` the
conditional case), `lib/designerFields.js ruleHolds`, `app/field_rules.py`.

**Condition operands** (`ReplSVTExpWithConstantOrValues`): a constant, `#CODE#` (another field's value), `#CODEPER#` / `#CODEAMT#`
(percent / amount parts of a per-amount field), an expression over fields, or `SVTCURDATE`, `SVTCURTIME`, `SVTUSERCODE`, `#D#`
(document date), `#FYF#` / `#FYT#` (financial year start / end), `#P#` (party id), `#PFX#` (document prefix). The data type
comes from the field mentioned: Numeric compares as numbers, Date as dates, otherwise as text (case-insensitive, SQL collation).
The desktop evaluates it with a `SELECT` on the server; the web evaluates it in Python and the browser with the same rules.

## 3. Editing a cell
- **Before edit** (`vsfBeforeEditCancel`): read-only (flag Y, no operator or condition true) cancels the edit; in "M" (master)
  mode on a transaction the fields that decide the document prefix (`UtlDocNoPrefix.CndFldCode`) cannot be changed; a **combo**
  field offers `FldCmbTblName` rows (`FldCmbStorageFldName` stored, `FldCmbDisplayCols` shown, `FldCmbAddnlCond` as `WHERE` with
  a backtick standing for a quote, `#C#` / `#S#` = the customer's / supplier's id); a **list** field offers `FldNumStringListContent`
  (`Y|N`); a multi-line field opens a pop-up editor; a date field gets the mask `##/##/####`; `FldBlnRefill` fills the list from
  the table column's own distinct values.
- **Keys** (`vsfKeyPressEdit`): numeric fields accept digits and `.` (and `-` when negatives are allowed); text fields by
  `FldTxtStringType` ALPHANUMBER / ALPHABET / NUMBER and refuse `FldTxtDoNotAllowChars`; case by `FldTxtCase`; width by
  `FldDataWidth`; multi-line when `FldTxtBlnMultiLine`.
- **After edit / validate** (`vsfValidateEdit`):
  1. Mandatory (flag and condition) and the value is blank or `0`: "Mandatory field: 'X' cannot be blank / zero".
  2. **Pick-up**: every row whose `FldPickUPAfterFld` is this field runs its `FldPickUP` SQL with `#THISCODE#` replaced by the
     new value and stores the result (text, number or date by the row's type).
  3. **Unique** (`FldUnique`, optionally `FldUniqueBasedOn`): "X is to be unique [based on Y]".
  4. Dates: `FldDtBlnFirstDOM` / `LastDOM` snap the date; `FldDtBlnWithinFinYear` requires the financial year; from / to ranges
     with operators ("--- Entered Date not >= value").
  5. **Combo**: the typed value must exist in the combo's table ("[v] ... Not found in table !!").
  6. Numbers: **percentage** fields 0-100 (1-100 when mandatory), ranges with operators ("--- Entered number not <= v"),
     mandatory numbers without negatives must be above zero.
- **Save-time check** (`validOthFLds`): every visible mandatory row is checked again ("Field X cannot be blank [or less than zero]"),
  an empty date `/ /` is cleared, combos are verified again.
- **Defaults** (`fillVsfHFTDefValues`): `FldUseDefault = Y` rows get `FldDefStdDefault` (tokens as above) when empty (or 0 when
  not read-only); with `FldDefBlnAgnstCond` the default comes from `FldDefCondList` (conditions separated by `°`, parts by `ß`:
  compare, operator, with, value).
- Web: `app/field_rules.py` (server, authoritative; `schema_meta.clean_extras` calls it), `lib/designerFields.js` (browser).
  Not yet built: pick-up SQL, unique, combo-exists, conditional defaults, `FldClrBefHide`, per-sub-section tabs.

## 4. Formulas (`fnParse`, `fnMstParse`, `CalColSum`)
Functions found in the engine: `SVTCOLSUM`, `SVTCOLAVG`, `SVTCOLMAX`, `SVTCOLCUM`, `SVTCOLSUMGROUP`, `SVTPROD`, `SVTROUND(x, n)`
(half up: `floor(x*10^n + 0.5) / 10^n`), `SVTROUND2`, `SVTROUND50` (look at the first decimal digit: 5 or more adds one to the
whole part; so 3894.49 -> 3894, 8.99 -> 9), `SVTROUND100` (any fraction rounds **up**), `SVTROUNDOFFAMT` (rounded minus original),
`SVTAMTINWORDS`, `SVTCONCATENATE`, `SVTDATEDIFF`, `SVTDATEADD`, `SVTGETQOH`, `SVTCALCTAXAMT`, `SVTPARTYDATA`, `SVTPARTYADDNLDATA`,
`SVTGETGRIDDATA`, `SVTDOCNO`, `SVTPREFIX`, `SVTUSERID`, `SVTSRCDOCQTY`, `SVTPENDQTY`; tokens `SVTFYF`, `SVTFYT`, `SVTCURDATE`,
`SVTWRKDAYS`, `SVTMNTHDAYS`, `SVTCUSTCODE`, `SVTSUPPCODE`, `SVTDEBITCOL`, `SVTCREDITCOL`, `SVTSTARTMNTHDOCDATE`. This shop's
database uses only four (`SVTCOLSUM`, `SVTAMTINWORDS`, `SVTROUND2`, `SVTROUND50`); the web engine (`app/formulas.py`,
`lib/formulas.js`) has those, proven on 400 saved invoices.

## 5. Gaps still open (web vs desktop engine)
Per-sub-section tabs, pick-up SQL, unique checks, combo-exists check, conditional defaults, the other functions above, "Hide" with
`FldClrBefHide`, the per-user layout tables, `UtlDocNoPrefix` prefix rules, and everything in the transaction forms (see the other
notes in this folder as they are written).

## 6. The compiled forms (what the designer does NOT describe)
`frmMain.OpenGeneralForm` finds the screen by menu caption -> `MstMenu.DocID` -> `UtlTransactions.TrnDocType` and opens:
| TrnDocType | Form | Standard controls drawn by the form itself (everything else comes from the designer's rows) |
|---|---|---|
| General | `frmMstGeneral` | Code, Name, Inactive + Inactive remarks (mandatory when inactive), Series + No (when `MstAutoGenCode`: code = series + 6-digit number), file attachments, approval flag |
| Party Variant | `frmMstParty` | Series/No, Code, Name, Tally alias, Inactive + remarks, Billing address 1-4, City, State, Pin, Country, Remarks, Party group, Ledger group (with vouchers) + contact / address / alert sub-forms |
| Item Variant | `frmMstItem` | Code, Name, Short name, Hot key, HSN, Item group, Liquor type, Non-stockable, Inactive + remarks, UOM, Bottles per case, **item aliases** (`MstItemAlias`), rate-change audit (`MstItemAudit`) |
| (Company) | `FrmMstComp` | grid of five rows: Company Name, Address, ".", "..", Email ID = `CompName`, `Address1-3`, `EMAIL`; then the designer's rows as "Additional Fields"; financial years (create / remove: "Financial Year already exists!", "Atleast 1 active financial year must exist!", only the first or last year can be removed), images, documents, backup |
A table column that is neither one of these nor a designer row is **not shown**. Web: `schema_meta.compiled_columns` keeps only these
standard columns; the rest of a table's columns are shown only when the designer lists them.

## 7. Save and delete of a master (frmMstGeneral / frmMstParty / frmMstItem)
- Save: code and name not blank; code unique (`code = X and id <> this id`: "<caption> already exists.."); the designer's rows
  re-checked (`validOthFLds`); an inactive record needs remarks; new id = `max(id) + 1`; `Authorised` = `N` (waits) when the
  document needs authorisation, else `Y` with user and date; audit log when the document is audited; one transaction.
  Messages: "<caption> '<code>' added successfully." / "Updated successfully." / "Deleted Successfully!".
- Delete (`svtModule.blnDeleteOK`): the rows of `UtlDeletion` (DelTblName is referenced by TblName.FldName, shown as TblCaption) are
  looked up; any hit refuses with "Cannot Delete!!! Data : <value> of <table> exists in <caption>". Web: `sqlstore._check_unused`.
- Sales Invoice also writes `TrnPoints` (loyalty), `TrnMultiAmt` (split payments), `TrnDtlSINVDenom` (cash denominations), `TrnHdrRDM`
  (redemption) and `UtlHoldBill` (held bills); Purchase Bill `UtlStockList`, `MstItem` (rates); Stock Deduction / Sales Return
  `TrnDtlPLBox`. The web writes none of the first four yet.

## 8. Built from this study (web)
Database setup lists every SQL Server instance of any version (`app/sql_discovery.py`: SQL Browser broadcast / unicast, this PC's
registry) and scans one server with a login to show each instance's databases (`/setup/instances`, `/setup/scan`); the extra
formula functions (`SVTROUND`, `SVTROUND100`, `SVTROUNDOFFAMT`, `SVTCOLAVG`, `SVTCOLMAX`, `SVTDATEDIFF`, `SVTCALCTAXAMT`,
`SVTCONCATENATE`); conditional defaults; pick-up SQL (`GET /designer/pickup`); unique checks (`schema_meta._taken`); auto-generated
codes (`/designer/series`, `/designer/next-code`); a tab per designer sub-section; item aliases (`MstItemAlias`) and the rate-change
log (`MstItemAudit`); loyalty points (`TrnPoints`) and split payments (`TrnMultiAmt`) written with a sales invoice; financial-year
create / remove with the desktop's rules; Company Master Backup (`BACKUP DATABASE`, `app/db_backup.py`), Companies... and Update
Dashboard Link; contact persons, addresses, alerts and notes of a customer / supplier (`routers/party_info.py`). Not built: cash
denominations per invoice (`TrnDtlSINVDenom`), `TrnHdrRDM` (absent from this database), "Backup and mail".
