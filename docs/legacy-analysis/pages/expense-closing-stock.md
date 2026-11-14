# Expense Entry (`frmTrnNonPartyNonItem`) and Closing Stock Entry (`Clients/LHouse/frmTrnCloseStk`) vs the web app

## Expense Entry
Legacy: `frmTrnNonPartyNonItem.vb` (2,998 lines; menu "Expense Entry", `NonPartyNonItemDoc`, DocID 1277, `TrnHdrEXP` / `TrnDtlEXP`). Read: edit / delete / save buttons (140-248), `validFlds` (1120-1219), insert / update (1880-1960). The shop's database (read-only): the designer (`LEDID` Particulars/Ledger, `AMT`, `REMARKS`, `RAMT` Receipt Amt; footer `RMKS`, `AMTINWORDS`; total `TOTAMT = SVTCOLSUM(AMT)`), `UtlDocNoFmt` (year / prefix / last five digits), no saved expense entries (`TrnHdrEXP` empty).
Web: `ExpenseEntry.jsx`, `expense_db.py`, `routers/transactions.py`.

| Legacy | Web now |
|---|---|
| Delete button, CANCELLED choice: header `Cancelled = 'Y'` (no stock); "Current record is cancelled. Deletion not possible!" (238), freeze date, `ReWriteINPROGRESS` | `POST /transactions/expense/{no}/cancel` (`expense_db.cancel_expense`, one transaction in `TrnHdrEXP`) and **Cancel Entry** in the action bar |
| "Current record is cancelled. Cannot edit!" (160) | the edit refusal now says this |
| Number `<year><prefix><5 digits>`, printed `26//00115` | already (`accounting.next_number`) |
| Header and lines saved to `TrnHdrEXP` / `TrnDtlEXP` with ledger, amount, remarks | already (`expense_db.create_expense`); receipts (`RAMT`) are kept on edit |

Open: Particulars/Ledger must exist ("Unknown ledger" is the web's text); the exe's "Please enter the qty greater then 0" is the amount rule on the web; Excel import (2337, 2555), file attachment, amendment fields (`strAmmReq`), print prompt: not covered.

## Closing Stock Entry
Legacy: `Clients/LHouse/frmTrnCloseStk.vb` (6,831 lines): the Drops client's form (it also writes `TrnHdrCSE` and generates a sales invoice, 3407-3559). It is **not in this shop's menu and the shop has no `TrnHdrCSE` table**, so it could not be compared against real rows; the web's entry lives only in memory and is lost on a restart (it is not written to any desktop table).
| Legacy | Web now |
|---|---|
| Delete refused after the invoice is generated: "Invoice Generated Cannot To Delete" (466), "Current record is cancelled. Deletion not possible!" (473); CANCELLED = `Cancelled = 'Y'`, no stock effect | `POST /transactions/closing-stock/{no}/cancel` and **Cancel Entry**; a cancelled entry cannot make an invoice |
| "Sales Qty is Negative" (2238) | already: "Closing qty for X exceeds opening" |
| Edit before the invoice exists; "Invoice Generated Cannot To Edit" (326) | **Not built**: the web screen has no Edit for a saved entry (open) |

## Open (both)
1. Hard Delete: not built (the web never removes rows from the shop's tables; `TrnHdrEXP` has no `IsDelete`).
2. Closing Stock Entry has no persistence in direct mode and no edit. Needs the owner's decision: keep it web-only (as today) or add `TrnHdrCSE` tables to a database that uses the Drops form.

## Follow-up pass (2026-11-14): the other databases on the server
The shop's own database lacks some of this data, so the other databases on the same server were read (read-only) to build and check the rest: `vLQMSNDROPSBLDR` (Closing Stock Entry: `TrnHdrCSE` 2 entries, `TrnDtlCSE`, menu, designer, number format), `vTmpl*` temple databases (expense entries with extra header columns, 440 attachment rows), `vLQMSSLSDVG` (Purchase Bill / Indent with File Attachment on), `vLQMSBOOZE` and this shop (Purchase Indent with Import/Export on).
- **Closing Stock Entry saved to the database** where the tables exist (header `CSENO`, `Prefix` CSE, `PrintCSENO`, `CSEDate`, `ASONDT`, `TOTINTQTY`, `INVNO`; lines `OPQTY`, `CLQTY`, `INVQTY`, `ITEMGROUPNAME`); numbered `26CSE00001`; edit, cancel and the generated invoice number (`INVNO`) are written; entries made in the desktop are read back. Checked against the real Drops database (the two real entries read correctly; a test entry written, edited, cancelled and rolled back). In a database without `TrnHdrCSE` (this shop) a save is still refused with the "no table" message.
- **Printed number from the document's own format** (`UtlDocNoFmt`): Drops prints `27/CSE/00001` (token 5 = the year the financial year ends in), this shop's documents print `26//00201`; the real Drops value matched.
- **Excel import and template** on Purchase Indent, Sales Return and Stock Deduction, shown only where the Doc Designer's Allow Import/Export is on (this shop: Purchase Indent): first row Item Code / Qty / Rate ("Please Select the valid template"), repeated items skipped, unknown or inactive items reported.
- **File Attachment** for documents, shown only where the Doc Designer's File Attachment (`FileAttachReq`) is on for the document (not on any document of this shop; on in `vLQMSSLSDVG`): Options → File Attachment on an opened document; files are kept by the web server like the masters' attachments.
- **Not built:** Expense Entry's designer-added header columns (`REFNO`, `PAYMNTMODE`, `CHEQUENO`, `CHEQUEDT`, `CHEQUEAMT`, `BANKNAME`) exist only in the temple databases (a different product); hard Delete (the web never removes rows from the shop's tables); Excel import of Purchase Bill, Sales Return lines against an invoice and Closing Stock (switched on in other databases, not in this shop).
- Side effect to know: the test entry in the Drops database used an identity number, so the next real `CSEID` there skips three numbers (no data left behind).
