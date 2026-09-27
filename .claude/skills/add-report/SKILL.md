---
name: add-report
description: Add a new tabular report to vLQMS (backend /reports endpoint plus a REPORTS entry rendered by ApiReport). Use when asked to add or port a legacy report screen.
---

# Add a report

Most reports need no new component. `App.tsx` renders any module whose name is a key in `REPORTS` (`frontend/src/components/reports/reportDefs.ts`) through `ApiReport`. The sidebar does not read `REPORTS`, so the name also has to go into `REPORT_ITEMS` in `Sidebar.tsx`.

1. **Backend endpoint** in `backend/app/routers/reports.py` (router prefix `/reports`). Accept the query params that match the filters you pick below. Build rows from `store.MOVEMENTS` (use the `_moves(from_date, to_date, code)` helper) and from `store.ITEMS` / `store.DOCS`, never from hard-coded numbers. Use `_litres()` for litre quantities. Return a list of flat dicts.
2. **Frontend definition:** add an entry to `REPORTS`: `{ title, subtitle, endpoint: '/reports/<path>', filters, columns }`.
   - `filters`: any of `'on'` (single date → `on`), `'range'` (→ `from_date`, `to_date`), `'item'` (→ `item_code`), `'months'` (→ `months`). `ReportShell.tsx` builds the query string.
   - `columns`: `{ key, label, kind?: 'text' | 'number' | 'money', total?: true }`. Each `key` must match a key in the backend rows.
3. **Sidebar and rights:** add the name to `REPORT_ITEMS` in `frontend/src/components/Sidebar.tsx`, to the Reports group in `MODULE_GROUPS` and to `MODULE_KEYWORDS` (`frontend/src/components/modules.ts`), to `MATRIX_SCREENS` in `matrixDefs.ts` (Admin Master rights grid), and to the relevant roles in `backend/app/store.py` `ROLES`.
4. **Check:** run `npm run build` and `npm run lint`, open the report while logged in as ADMIN, and compare the totals with the Stock Balance or DSR for the same dates.
5. **Parity doc:** update the Reports table in `frontend/LEGACY_PARITY.md`.

Reports with a custom layout (DSR, Stock Balance) have their own components and `case` lines in `App.tsx`. Customised Reports (the list of legacy report names) live separately in `backend/app/routers/custom_reports.py`.
