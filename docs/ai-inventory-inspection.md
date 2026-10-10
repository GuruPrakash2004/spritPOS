# AI stock prediction: Phase 1 inspection report

Read-only inspection of the codebase (2026-10-10). Nothing was executed against the database. Everything below comes from reading source; items that need the live database are marked **NEEDS DB** and have a query in the appendix for you to run.

Paths are under `backend/app/` unless stated.

## 1. How current stock and inventory value are calculated

- **Current stock (QOH)** lives in `store.ITEMS[code]["qoh"]`. `db_store.load_items()` fills it from `TrnQOH.CLBal` for `LOCID = 1`, using a LEFT JOIN from `MstITEM` (`WHERE i.ITEMID > 0 AND i.InActive = 'N'`). Items with no `TrnQOH` row get 0.
- **Changes** go only through `store.move_stock()` (`store.py:247`): it adds to `qoh` and appends to `store.MOVEMENTS`. `store.DB_BASE_MOVEMENTS` and `store.APP_MOVEMENTS_FROM` mark where this app's own movements start; `store.BASE_QOH` is each item's QOH at that point.
- **Persistence**: web movements are written to `Web_AppMove` and to the desktop's `UtlStockList` (`sqlstore._write_move`, `sqlstore.py:676`), with `StkMultiplier` +1/-1 and `DocType` INV / PB / and so on.
- **Inventory value**: `routers/reports.py:stock_balance()` (line 57) gives `cl_qty = qoh - movements after to_date` and `cl_value = cl_qty * purchase_rate`. The DSR (`/reports/dsr`, line 227) sums these into `closing_stock {qty, value, litres}`. The Dashboard's "Inventory Value" card shows `dsr.closing_stock.value` (`Dashboard.jsx:110`). The "Inventory Mix" chart uses `qoh * mrp` (`Dashboard.jsx:133`), a different basis (MRP, not purchase rate).
- **Live stock**: `db_store.live_sales_and_stock()` (line 611) re-reads `SELECT ITEMID, CLBal FROM TrnQOH WITH (NOLOCK) WHERE LOCID = 1`, cached 20 s. Only `/reports/restock` uses it.

## 2. TrnHdrSINV / TrnDtlSINV

| Aspect | Value (source) |
|---|---|
| Header key | `SINVID` (identity); number `SINVNO` (`sqlstore.py:524`) |
| Detail join | `TrnDtlSINV.SINVID = TrnHdrSINV.SINVID` (`db_store.py:459`) |
| Item key | `TrnDtlSINV.ITEMID` → `MstITEM.ITEMID` |
| Date | `TrnHdrSINV.SINVDate`; also `SINVDateTime`, `SINVTime` |
| Quantity | `TrnDtlSINV.SINVQty` (bottles); `STKQty` is written equal to it by the web |
| Cancelled | `TrnHdrSINV.Cancelled = 'Y'` |
| Deleted | `TrnHdrSINV.IsDelete = 'Y'` (a removed duplicate is also `Cancelled = 'Y'`, `sqlstore.py:516`) |
| Origin | `BILLSYSTEM = 'WEB'` for web-made bills (`sqlstore.py:534`) |
| Other | `ITEMAMT`, `NETTOT`, `SINVRate`, `SCID`, `SPID`, `CUSTID`, `LOCID` |

Filters already used in the code, to copy for the pipeline:
- `h.Cancelled = 'N'` plus a date range (`db_store.live_sales_and_stock`, line 635).
- `h.Cancelled = 'N' AND h.IsDelete = 'N'` (`stock_rewrite.py:172`, the desktop's own consistency check). **Use this stricter form.**

**Returns are not on the invoice.** They are separate documents: `TrnHdrSR` / `TrnDtlSR` (`SRQty`, `SRDate`), stock ledger `DocType = 'SR'`. The `TrnDtlSR` table is optional in the desktop (`stock_rewrite.legacy_check` tolerates it missing), so the pipeline must handle its absence. Net daily sales = `SINVQty` (non-cancelled) minus `SRQty` (non-cancelled `TrnHdrSR`), by date.

Open question (**NEEDS DB**): whether a return is ever recorded as a negative `SINVQty` row by the desktop. Query A5 checks.

## 3. Bottles vs cases

- Every stored quantity (`SINVQty`, `PBQty`, `PIQty`, `CLBal`) is in **bottles**.
- Cases are derived: `cases = qty // BotPerCases`, `bottles = qty % BotPerCases` (`db_store.py:525`). The purchase side also stores `NOOFCASES` / `NOOFBOTTLES`.
- `bottles_per_case` comes from `MstITEM.BotPerCases`, `COALESCE(..., 12)`, then `max(int(..), 1)` (`db_store.py:82,120`). So a NULL silently becomes 12; a real 0 becomes 1.
- Litres: `qty * MstPKG.LTRPERBOT`.

## 4. Purchase receipts and pending purchase orders

- **Receipts**: `TrnHdrPB` / `TrnDtlPB` (`PBQty`, `PBDate`). A saved bill calls `move_stock(kind="purchase")`; a purchase return is negative. Ledger `DocType = 'PB'`.
- **There is no purchase-order table and no pending-PO concept** anywhere in the backend (searched for PO, lead time, pending).
- The nearest thing is the **Purchase Indent** (`TrnHdrPI` / `TrnDtlPI`, `PIQty`). It "does not affect stock" (`routers/transactions.py:133`) and carries no received/pending status. A bill can reuse an indent's lines through `copy_from_indent` (`transactions.py:178`), but no link is stored on the indent saying it was fulfilled.
- Consequence for Phase 4: "pending PO qty" has to be either 0 or derived (indents newer than the last purchase of that item). **Decision needed from you.**

## 5. Existing purchase indent creation and approval

- Endpoint: `POST /transactions/purchase-indent` → `routers/transactions.py:create_indent(body: PurchaseIndentIn)` (line 136). Steps: `gsettings.check_not_frozen`, `_doc_extras("TrnHdrPI")`, `doc_settings.check_lines`, supplier lookup via `store.find(store.SUPPLIERS, ...)`, `security.check_authorised("suppliers", ...)`, `_price_lines(..., "purchase_rate", with_gst=False)`, `store.next_doc_no("purchase_indent", prefix)`, store in `store.DOCS["purchase_indent"]`, `admin_audit.record(...)`.
- Persistence: `sqlstore._write_purchase_indent` (line 610) inserts `TrnHdrPI` (`BILLSYSTEM = 'WEB'`, `Authorised = 'Y'`) and `TrnDtlPI` rows, inside the same request transaction.
- Right required: `require_right("add", "Purchase Indent", "Purchase Bill")`.
- **No approval workflow for indents.** Authorisation exists only for masters (Master Approvals, `security.check_authorised`). The indent is saved as `Authorised = 'Y'` straight away.
- Phase 6 note: the creation logic is inside an HTTP handler, not a reusable function. To "call the existing function" without duplicating logic, the handler body would need extracting into a plain function (a refactor of non-billing but transaction code). Raise this at Phase 6.

## 6. qoh, ROL, ROQ, MINLEVEL, MAXLEVEL, UOM, bottles_per_case

| Field | Source | Use |
|---|---|---|
| `qoh` | `TrnQOH.CLBal` | everywhere |
| `rol` | `MstITEM.ROL`, loaded as `_f(r.get("ROL"), 12)` | `/reports/reorder` (`qoh <= rol`), `/reports/restock` (LOW if `qoh <= rol`) |
| `roq` | `MstITEM.ROQ`, default 48 | restock `suggested_qty = max(roq, ceil(avg_daily*14 - max(qoh,0)))` (`reports.py:212`) |
| `MINLEVEL` / `MAXLEVEL` | **not referenced anywhere in the backend**; may or may not exist as `MstITEM` columns | none today |
| `uom` | `MstUOM.UOMCODE` via `i.UOMID`, default "BTL" | display and line UOM |
| `bottles_per_case` | `MstITEM.BotPerCases`, default 12 | case maths |

Because the loader replaces NULL with 12 / 48 / 12, the app cannot tell "populated" from "defaulted". **NEEDS DB:** population counts come from queries B1 to B3.

Existing restock rules to keep as the fallback (`routers/reports.py:119`): CRITICAL ≤ 3 days left, LOW ≤ 7 days or `qoh <= rol`, cover 14 days; `avg_daily = sold / window_days` over the last 15 days (default).

## 7. Supplier lead time

None found in the code. `MstSUPP` is read for code, name, and GSTIN-type fields only. **NEEDS DB:** query C1 lists `MstSUPP` columns. Expect to configure lead time as a setting (the brief already asks for a default).

## 8. Stored procedures, views, triggers

Seen in code (definitions not available here):
- `mSp_RewriteStkList`: the desktop's Re Write Stock procedure (`stock_rewrite.py:1`).
- `mSP_PrintInv`: invoice print procedure (`rpt_print.py:149`).
- A trigger on `UtlStockList`: `sqlstore.py:683` comment says deleting a bill's stock rows lets "the trigger put the stock back", so `TrnQOH.CLBal` is maintained by a database trigger on `UtlStockList`.

No `.sql` files, views or `CREATE PROCEDURE` exist in the repo. **NEEDS DB:** queries D1 to D3 list them.

## 9. Existing tests

`backend/tests/` (pytest, in-process, sample data from `tests/seed.py`): `test_restock.py` (restock ranking), `test_sales_invoice.py`, `test_business_rules.py` (stock rules), `test_sqlstore.py`, `test_tools.py` (Re Write Stock), `test_live_data.py`, `test_extra_reports.py`, `test_websync.py`. There is **no test for `/reports/reorder`** and no test of a purchase indent's database write beyond `test_sqlstore.py`.

## 10. Why the Dashboard total stock can disagree with OUT OF STOCK items

These are hypotheses from the code. None is confirmed against your data; query E1 measures them.

1. **Two different QOH sources.** Dashboard totals (`stock_balance` → DSR) use the in-memory `ITEMS` snapshot taken at startup (plus this app's movements). The restock/OUT_OF_STOCK panel uses `TrnQOH` read live (20 s cache) plus app movements since the DB snapshot (`reports.py:154-168`). Billing on the desktop after the backend started moves one but not the other.
2. **Different item sets.** `stock_balance` sums every item in `store.ITEMS` including negative `cl_qty`; negative stock reduces the total while each such item shows OUT OF STOCK (`qoh <= 0`). The restock list skips `active is False` items.
3. **Different "out" test.** OUT OF STOCK is `qoh <= 0` (`reports.py:188`); a negative item shows as 0 stock in the panel but subtracts from the total.
4. **Valuation basis.** Inventory Value uses purchase rate; Inventory Mix uses MRP, so the two cards never match.
5. **Location.** Only `LOCID = 1` is read; stock in other locations is invisible to both.
6. **TrnQOH vs UtlStockList drift.** If the desktop's Re Write Stock has not been run, `TrnQOH.CLBal` can differ from the ledger (`stock_rewrite.legacy_check`, "Ledger vs QOH" tab).

## Notes found while inspecting

- `stock_rewrite.py`'s docstring says the web app "never writes the legacy tables", but `sqlstore.py` mirrors invoices, bills, indents, returns and deductions into the desktop tables (`MIRRORED_DOCS`, line 51), and CLAUDE.md says so too. The docstring is out of date; the behaviour in `sqlstore.py` is the real one. Consequence for Phase 2: web bills are already in `TrnHdrSINV`, so one query covers desktop and web sales.
- `db_store.load_purchase_bills` uses a fixed `0.75` litres per bottle; not relevant to AI work but wrong for non-750 ml items.

## Appendix: read-only queries for you to run

All are `SELECT` only. Run on the live DB with `WITH (NOLOCK)` or a replica.

```sql
-- A5: do returns appear as negative invoice quantities?
SELECT COUNT(*) AS neg_rows FROM TrnDtlSINV WITH (NOLOCK) WHERE SINVQty < 0;
SELECT MIN(SINVDate) AS first_bill, MAX(SINVDate) AS last_bill, COUNT(*) AS bills FROM TrnHdrSINV WITH (NOLOCK) WHERE Cancelled='N' AND IsDelete='N';
SELECT COUNT(*) AS return_lines FROM TrnDtlSR WITH (NOLOCK);   -- errors if the table does not exist

-- B1: how many active items have ROL / ROQ / BotPerCases set
SELECT COUNT(*) AS items,
       SUM(CASE WHEN ISNULL(ROL,0)=0 THEN 1 ELSE 0 END) AS rol_zero_or_null,
       SUM(CASE WHEN ISNULL(ROQ,0)=0 THEN 1 ELSE 0 END) AS roq_zero_or_null,
       SUM(CASE WHEN ISNULL(BotPerCases,0)=0 THEN 1 ELSE 0 END) AS bpc_zero_or_null,
       SUM(CASE WHEN UOMID IS NULL OR UOMID=0 THEN 1 ELSE 0 END) AS uom_missing
FROM MstITEM WITH (NOLOCK) WHERE ITEMID>0 AND InActive='N';

-- B2: do MINLEVEL / MAXLEVEL columns exist on the item table
SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME='MstITEM' AND (COLUMN_NAME LIKE '%MIN%' OR COLUMN_NAME LIKE '%MAX%' OR COLUMN_NAME LIKE '%LEVEL%' OR COLUMN_NAME LIKE '%LEAD%');

-- B3: if B2 shows MINLEVEL/MAXLEVEL, how many are populated (edit the names to match)
-- SELECT SUM(CASE WHEN ISNULL(MINLEVEL,0)=0 THEN 1 ELSE 0 END) AS min_zero, SUM(CASE WHEN ISNULL(MAXLEVEL,0)=0 THEN 1 ELSE 0 END) AS max_zero FROM MstITEM WHERE InActive='N';

-- C1: supplier lead time or similar columns
SELECT TABLE_NAME, COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS
WHERE COLUMN_NAME LIKE '%LEAD%' OR COLUMN_NAME LIKE '%DELIVER%' OR COLUMN_NAME LIKE '%CREDITDAYS%';

-- D1: procedures and views; D2: triggers; D3: definition of the stock triggers
SELECT name, type_desc FROM sys.objects WHERE type IN ('P','V','TR') ORDER BY type_desc, name;
SELECT t.name AS trigger_name, OBJECT_NAME(t.parent_id) AS on_table, t.is_disabled FROM sys.triggers t;
SELECT OBJECT_NAME(object_id) AS obj, definition FROM sys.sql_modules WHERE OBJECT_NAME(object_id) IN (SELECT name FROM sys.triggers WHERE OBJECT_NAME(parent_id)='UtlStockList');

-- E1: dashboard vs live disagreement: items out of stock, negative stock, and ledger drift
SELECT SUM(CASE WHEN CLBal<=0 THEN 1 ELSE 0 END) AS out_of_stock,
       SUM(CASE WHEN CLBal<0 THEN 1 ELSE 0 END) AS negative_items,
       SUM(CASE WHEN CLBal<0 THEN CLBal ELSE 0 END) AS negative_qty_total,
       SUM(CLBal) AS total_qty
FROM TrnQOH WITH (NOLOCK) WHERE LOCID=1;
SELECT COUNT(DISTINCT LOCID) AS locations FROM TrnQOH WITH (NOLOCK);
```
