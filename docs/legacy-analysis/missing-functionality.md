# Missing functionality: Windows vLQMS source vs web app (working checklist)

Built 2026-11-02 from `source_code/` (Designer captions in `screen-audit.md`, handlers read in the forms). Tick a box when built and checked.
Menu level is complete (see `feature-parity-audit.md`); this lists what is missing **inside** screens.

## A. Sales Invoice (`frmTrnPosInv` / `frmTrnInv`)
- [x] Beer / Wine / IML litre limit per bill (`AllowBeerInLtr`, `AllowWineInLtr`, `AllowIMLinLtr`, off when `SPLITINV` = Y): labels, red, save blocked, server check
- [x] F7 Clear Items (the web's Cancel bill, Alt+X, empties the bill)
- [x] Load Template (`MstItemTemp`; Alt+T, "Add all to bill", Edit to add/remove items and make sets; `/item-templates`)
- [x] Cash denominations saved per bill (`TrnDtlSINVDenom`; "Count" button beside Cash tendered, values from `UtlDenom`)
- [x] `AllowNegative` setting (Y: bills may go below zero stock; server check and screen caps off; QOH column not hidden)
- [x] Copy From a saved bill (Alt+Y: pick a bill, its items go onto the new bill)
- [x] Search in bill (web: Find in bill), Print Bill Copy (web: Re-print), F4 Delete (web: Remove Duplicate Invoice utility); Top 10 Items is a design-time button with no code in the exe
- [x] Split billing (`saveSplitBills`: SPLITINV = Y and over the litre limit → several bills, each within the limits; one payment type only)
- [x] Loyalty SMS after a bill (`LoyaltySENDMAIL`, to customers marked Send SMS, in the background) and the SMS screen (send to numbers / customers, History) through UtlMsgSettings / TrnSMS (`app/sms.py`, `/sms/*`)
- [ ] Loyalty e-mail (`LoyaltySENDMAIL`'s mail part; the web has SMTP for reports only)
- [x] Draft auto-save and restore (kept in this browser; banner offers Restore / Discard)
- [x] QR code multi-item scan (`CODE,QTY;CODE,QTY` in the scan box); multi-currency button (`btnCurr` is the note-count grid: the Count button)
- [x] `updateSInvRateToMstItem` (UtlMisc.RateFld = Y: the bill's rate becomes the item's ItemSell)

## B. Purchase Bill, Indent, Sales Return, Stock Deduction, Physical Stock (`frmTrnPBM`, `frmTrnPartyDoc`, `frmTrnPSE`)
- [x] Load Purchase Indent (already there: Copy from indent) and Load reorder items (`getROLItems`: items below their reorder level)
- [ ] Load Pending (`btnPendingDoc`, `frmTrnPendPopUp`: pending quantities of a source document, driven by the Doc Designer's document links; not used by the liquor documents)
- [ ] Amendment checkbox (`chkAmmendment`, AmmNo versioning; the liquor documents do not use it)
- [ ] Copy From a document, F4 add-single-item, Location field
- [x] Physical Stock: Show Summary (already there: the Summary Report below the grid)
- [x] Email a document (already in the print window: Email), Party SMS (Utilities → SMS: pick customers, History). Not applicable: Import from XL on `frmTrnNonPartyNonItem` (a generic Doc Designer document this shop does not have)
- [ ] Short close (`frmUtlShortclose`): not applicable here: it closes pending quantities of *linked* documents (`UtlTransCum`), and this shop's UtlTransactions has no linked document types and TrnDtlPI is empty

## C. Masters
- [x] File Attachment on every master (Options → File Attachment; files kept by the web server under `backend/uploads/attachments/`, not in the desktop's shared TrnDocFiles folder / UtlAttachFiles) — was: File Attachment button on every master (Item, Customer, Supplier, UOM, Currency, general masters, Role)
- [x] Item Master: Non Stockable checkbox (`MstItem.NonStkable`)
- [x] Customer / Supplier: Multi Address (already built: `party_info.py`, Options → Contacts, addresses, alerts, notes) (`frmMstPartyAddress`), contact persons, alerts, activities
- [x] Customer vs Item / Supplier vs Item (new "Item rates" tab in that window: `MstCUSTITEM` / `MstSUPPITEM`; Open Transaction and Import from XL not built) (`frmPartyItem`: Open Transaction, Import from XL)
- [x] Price List Maintenance: type a new rate per row, Replace for rest of the list, Apply = Update to DB
- [x] Currency: Show amount in words in millions (already a field)
- [ ] Company: Select Company / Switch database (`frmMstSelComp`), form alias
- [ ] BOM master and BOM details (`frmMstBOM`, `frmTrnBOMDetails`) , if a shop uses them

## D. Utilities and others
- [x] SMS history (Utilities → SMS → History)
- [x] PDF Generator (Utilities → PDF Generator: Sales Invoice / Purchase Bill / Purchase Indent, up to 100 at a time, one ZIP of PDFs through the Crystal bridge)
- [x] Audit Trail (Reports → Audit Trail, Administrator only: the `UtlAudit` actions; the before/after values live in a separate log database the desktop uses, which this shop does not have)
- [ ] Auto E-mail Scheduler: not applicable here: its tables (`UtlAutoEmailSchHdr/Dtl`) do not exist in this database, the web app never creates tables in a client database, and sending needs a scheduler service
- [ ] Split Data (`frmUtlSplitData`): on hold, it splits the database; needs the owner's go-ahead and a tested restore
- [x] Calculator (Alt+K, any screen; Esc copies the result)
- [ ] Button hotkeys (`frmUtlBtnHotkeys`) and on-screen keyboard: the web has its own fixed keys and the OS has an on-screen keyboard
- [ ] Import Price Data (`frmUtlPriceImport`): not applicable here: it fills a price category (`MstPriceCat`), a table this database does not have
- [ ] Update from Tally (`frmUtlUpdatefromTally`): not applicable on the web: it reads Tally through an ODBC DSN (`TallyODBC_9000`) on the PC running the desktop
- [ ] Redeem Points document (`frmTrnRDM`): not applicable here: the database has no `vwredeem` view or `TrnHdrRDM` table; redeeming points on a bill is built

## E. Roles and rights (checked 2026-11-02 against frmMstRights, frmMain.EnableDisableMenus, svtModule.chkUserRights)
- [x] Rights come from the desktop's own tables: `MstUserRights` (or `MstRoleRights` when Login Settings' Rights Allocation is by role, `UtlMisc.UserBased`), 12 flags per menu screen. A screen opens when its row is not all zeros; Add / Edit / Delete flags drive the buttons and the server (`app/desktop_rights.py`, `security.modules_for` / `rights_for_user`). Before this every user was one of three fixed web roles by the name of their role.
- [x] A web screen with no menu row (Cash Denomination, SMS, ...) keeps the user's web role; a role the Administrator sets in Admin Master still wins; ADMIN has every right.
- [x] Print right (flag 4): enforced where the exe checks it: Purchase Bill, Purchase Indent, Sales Return, Stock Deduction, Closing Stock prints and e-mail, and Customised Reports (the exe's sales invoice screens do not check it, so neither do ours)
- [ ] Auth / Auth1 (6, 7) and Opt-1..5 (8-12) flags: no web equivalent yet (document authorisation, per-screen options)
- [x] Admin Master → Desktop rights: edit the 12 flags per menu screen for a user (or role) and write them to `MstUserRights` / `MstRoleRights` like frmMstRights; Copy From; filter by type and name; column Select All. Not built: Transpose, the user photo. The older "Roles & rights" tab still edits the web's own role lists (used only for web-only screens and roles you set in the web)
- [ ] Authorise Documents (`frmUtlAuth`): not applicable here: no document type in this shop's `UtlTransactions` requires authorisation (`AuthReq`)
