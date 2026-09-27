---
name: add-master
description: Add a new simple master screen (code/name style table) to vLQMS, wiring the backend store, schema, CRUD route, frontend field list, sidebar, module list and role rights. Use when asked to add or build a "... Master" screen.
---

# Add a simple master

Simple masters use one generic engine on each side: `crud()` in `backend/app/routers/masters.py` and `MasterCrud` in `frontend/src/components/masters/MasterCrud.tsx`. If the master needs custom behaviour (Item, Supplier, Company, Sales Counter), write a dedicated component instead, and still do steps 1–3 and 5–9.

## Backend
1. `backend/app/schemas.py`: add a Pydantic model with a `code: str` field (the CRUD factory keys on `code`).
2. `backend/app/store.py`: add a seeded dict `NAME = {code: {...}}`.
3. `backend/app/routers/masters.py`: add `crud("<kebab-path>", store.NAME, Model, "<X> Master")` at the bottom.

## Frontend
4. `frontend/src/components/masters/masterConfigs.ts`: add `X_FIELDS: MasterField[]` that mirrors the schema (types: `text` | `number` | `bool` | `select`). In `components/masters/Masters.tsx`, export `XMaster = () => <MasterCrud title=... subtitle=... path="<kebab-path>" fields={X_FIELDS} />`.
5. `frontend/src/lib/modules.ts`: add the module name to the Masters group in `MODULE_GROUPS`, and add search words to `MODULE_KEYWORDS`.
6. `frontend/src/components/layout/Sidebar.tsx`: add an `item(<LucideIcon size={16} />, '<X> Master')` line. The sidebar lists items explicitly.
7. `frontend/src/App.tsx`: add `case '<X> Master': return <XMaster />;`.
8. `frontend/src/components/admin/matrixDefs.ts`: add a `MATRIX_SCREENS` entry (group `'MASTERS'`) so it appears in the Admin Master rights grid.
9. `backend/app/store.py` `ROLES`: add the name to every non-admin role that should see it. The module name must be the same string in all of these places.

If the table exists in the legacy DB and should load from it, add a loader in `backend/app/db_store.py` and call it from `load_masters()`. Writes still stay in memory.

## Finish
- Run `npm run build` and `npm run lint` in `frontend/`. Check the endpoint at http://localhost:8001/docs.
- Update the row in `frontend/LEGACY_PARITY.md`.
