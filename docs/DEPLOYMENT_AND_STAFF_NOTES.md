# Going live: deployment checklist and notes for shop staff

## 1. Deployment checklist (do once per shop)
- [ ] Back up the shop's SQL Server database (full backup, keep a copy off the PC). The web app writes to the same tables the desktop uses.
- [ ] Put the connection in `backend/.env` only (host, database, login). Never in the code, the docs or git. Use a SQL login that can read and write the desktop's tables; it does **not** need rights to create tables (the web app creates none).
- [ ] Leave `DB_READ_ONLY` and `DB_WEB_TABLES` unset (direct mode). Set `DB_READ_ONLY=1` for a first look at a new shop (nothing is saved).
- [ ] Serve the site over HTTPS (or use it on the server PC itself): the browser can encrypt passwords only on HTTPS or localhost. Plain `http://` on the LAN still works; passwords are then sent as typed.
- [ ] Start the backend (`uvicorn app.main:app --port 8001`) and open the sign-in page. Check the footer says the database is connected.
- [ ] Sign in as the shop's Administrator with the **desktop password** of that user. The web app keeps no password of its own in direct mode, so every user signs in with the desktop password. A password changed in the web app (the key button) is written to the desktop's own user table, so the desktop accepts it too; the desktop keeps passwords of at most 31 plain English characters.
- [ ] Check Utilities -> Global Settings: FreezeDate (no bill can be dated on or before it), SPID (salesperson required on every bill), loyalty rules.
- [ ] Make one test bill and one test purchase on a quiet day. Compare the stock with the desktop's Stock Balance. Cancel the test bill.
- [ ] Run the desktop and web side by side for a day: a bill made in one should show in the other's Daily Summary.
- [ ] Keep the daily backup running, and include `backend/data` (held bills) in it.

## 2. What staff should know
**Bills and stock**
- A bill saved in the web app is a normal row of the desktop's tables: the desktop shows it, prints it and counts it in its reports.
- Stock goes down when a bill is saved and comes back when the bill is cancelled. The web app refuses a bill that would make stock negative.
- A bill dated on or before the FreezeDate is refused. Ask the Administrator to change the date in Global Settings.

**Held bills**
- A bill put on hold (F7) is kept in a small file on the server PC (`backend/data/held_bills_<database>.json`), not in the database, so it survives a restart. It is only visible to the web app (the desktop keeps its own held bills in its own files). Back that `data` folder up with the rest of the app; held bills don't move stock until they are finished.

**Messages saying "can't be saved: the desktop database has no table for it"**
- Some settings have no place in the desktop's database yet: user rights and roles, number series, menu layout, import field maps, the day-end cash denomination count, Re Write Stock's repair and creating new users. The web app refuses these and says so; nothing is changed. Do them in the desktop app.

**Vouchers and Quotation**
- A voucher or quotation can be edited (its number stays) or cancelled. A cancelled one cannot be edited again.
- Cancelling a voucher only marks it cancelled, as the desktop does; its ledger lines are not removed.

**Customers**
- GSTIN and Currency are mandatory for a customer (the walk-in CASH customer is exempt; enter `-` if a customer has no GSTIN).

**Locations**
- Bills are saved against Main Stock. Shops with a second location (for example "Mumbai Wear House") can't bill from it in the web app yet.

## 3. If something goes wrong
- "Database offline" on the sign-in page: check SQL Server is running and the PC can reach it; the page recovers by itself.
- A save fails with a 503 message: nothing was saved; the screen reloads from the database. Try again, and tell the Administrator the message.
- Restore from the backup only as a last resort, and only after telling the shop: it undoes everything entered since.
