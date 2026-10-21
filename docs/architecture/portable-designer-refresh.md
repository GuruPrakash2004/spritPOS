# Metadata-driven screens that refresh when the designer changes (portable recipe)

How vLQMS makes the web screens follow the desktop's Doc Designer, and how to copy the idea into another project.
Source here: `backend/app/designer.py`, `schema_meta.py`, `routers/designer.py`; `frontend/src/lib/designerStore.js`,
`designerFields.js`, `hooks/useScreenFields.js`, `components/layout/DesignerRefreshButton.jsx`.

## The idea in one paragraph

The screen layout is **data in the database** (which fields, caption, order, mandatory, hidden, read-only), not code.
The backend reads that data into memory and turns it, together with the table's real columns, into a **screen
definition** (JSON). The browser draws the screen from that JSON. When the designer changes something, an
Administrator presses **Refresh**: the server re-reads the data, the browser throws away its cached definitions, and every
open screen redraws **without losing what the user typed**. Nothing is re-read on a page load, a timer or a window
focus (a poll after every save/print slowed billing and was removed on 2026-10-12).

## The flow

```
Designer edits rows in Utl* tables (and the DBA adds/drops a table column)
        |
  [Refresh button]  (Administrator only)
        |
  POST /designer/reload                       (server, one at a time: a lock)
     1. designer.refresh()      re-read the designer tables into memory (build new lists first, then swap
                                in with one assignment so a screen being read never sees half a list)
     2. schema_meta.invalidate()  forget the cached column lists (sys.columns)
     3. sqlstore.reload_extras()  re-find each master's extra columns
     4. fingerprint()            cheap "version" of the structure
        -> returns { version, build }
        |
  forceDesignerReload(version, build)         (browser)
     - drop docsPromise / menuPromise / defs cache
     - snapshot.changes += 1, notify subscribers
        |
  every screen using useScreenFields(path)    (useSyncExternalStore on `changes`)
     - effect depends on `changes` -> loadDocs() -> loadDefinition(docId) -> GET /designer/screen/{id}
     - fields = extraFields(definition)      (useMemo: same array while the definition is the same)
     - the form state is NOT reset, so typed values stay; fields added show up, dropped ones disappear
```

## Backend pieces

1. **Load the designer tables into memory** (`designer.load()`): one `SELECT *` per table; trim padded CHAR text, drop
   secrets; optional tables in try/except (a shop without one just has no such setting). Build everything first, then
   assign (`LIST[:] = new`) so readers never see a half-empty list.
2. **Column cache** (`schema_meta.columns(table)`): `sys.columns` per table, cached; `invalidate()` clears it (and the
   writer's own cache).
3. **Build the definition** (`schema_meta.build_screen(doc, field_rows, header_cols, detail_cols, captions)`): for each
   designer row, merge with the real column (type, length, nullable). Columns with no designer row can be added as
   standard fields; columns the web has typed fields for are flagged `modelled`, with a `web_key` so the designer's
   caption/order/flags can override the typed field. No X/Y coordinates: layout = `Section -> SubSection -> Order`.
   Return per field: `code, caption, type, width, section, order, hidden, readonly, mandatory, rules[], combo, pickup, ...`.
4. **Endpoints**:
   - `GET /designer/docs` list of screens; `GET /designer/screen/{id}` one definition; `GET /designer/menu`.
   - `POST /designer/reload` (admin) does the refresh above and returns `{version, build}`.
   - `GET /designer/version` returns the fingerprint (kept for scripts; the browser does not poll it).
5. **Fingerprint**: `MAX(modify_date)` + count of the app's tables from `sys.objects`, plus `CHECKSUM_AGG(CHECKSUM(*))`
   and `COUNT(*)` of each designer table (fall back to the row count when a column type can't be checksummed). It
   changes when a column is added/dropped or a designer row is edited.
6. **Free-form extras**: columns the web has no typed field for travel in each row's `extras` object; the server
   validates them (`clean_extras`: type, length, nullable, mandatory) and writes them to the table's own columns.
   Never let a designer-added column be one of: identity, computed, audit, key or total columns.
7. **Build id**: the reload answer carries the served front-end build (time of `index.html`); an old open page offers a
   Reload (never reloads by itself).

## Frontend pieces

1. **Store** (`designerStore.js`): module-level caches (`docsPromise`, `menuPromise`, `defs` Map) and a tiny
   subscribe/emit snapshot `{version, changes, buildChanged}`. `loadDefinition(id)` caches the promise and removes it
   on failure. `forceDesignerReload()` clears all caches and does `changes++`. Last unsubscribe resets the store
   (sign-out).
2. **Hook** (`useScreenFields(path)`): `useSyncExternalStore(subscribe, snapshot)`; an effect keyed on `[path, changes]`
   finds the doc for the path, loads its definition, keeps the previous definition on error, and never returns another
   screen's definition while loading. `useMemo` the field list so child effects don't re-fire.
3. **Field engine** (`designerFields.js`): `toFieldConfig` (type -> input kind, width -> room), `applyDefinition`
   (override typed fields' caption/order/flags; code and name are never hidden), `fieldsFromDefinition` (draw a master
   from the definition alone), conditional rules (`ruleHolds`: hide / mandatory / read-only when another field has a
   value), `valueOf/withValue` (extras vs typed), `defaultFor`, pick-up fields.
4. **Refresh button**: confirm/defer, call `POST /designer/reload`, then `forceDesignerReload(state.version, state.build)`,
   then a "Refreshed" message. Disable it while busy.
5. A screen must keep user input across a redraw: field values live in the screen's own state keyed by field code, and
   only the *list of fields* changes.

## Rules that kept it fast and safe

- Never re-read the designer on page load, screen open, timer, focus or after save/print.
- Server refresh is serialised by a lock and runs in a thread pool (blocking DB calls off the event loop).
- Build new lists, then swap in one assignment.
- A missing designer table, missing definition or offline database means "no extra fields", never an error screen.
- Hidden typed fields are still saved unchanged; key columns (e.g. group id) keep their web drop-down.
- Test: a vitest for the hook (changes counter triggers a re-read and keeps typed values); a backend test that
  `reload` answers a version and clears the caches.

## Prompt to give Claude in another project

> Make my screens follow definitions stored in the database, the way vLQMS does. Read
> `docs/architecture/portable-designer-refresh.md` from the vLQMS repo (I will paste it) and apply it to this project.
> My definition tables are: `<table names and what each column means>`. My screens to drive: `<list>`.
>
> Build:
> 1. Backend: load the definition tables into memory (build then swap); a cached `sys.columns` reader with
>    `invalidate()`; `build_screen()` merging definition rows with real columns into JSON (caption, type, width,
>    section, order, hidden, readonly, mandatory, rules, dropdown source); `GET /designer/docs`,
>    `GET /designer/screen/{id}`; admin-only `POST /designer/reload` (lock, thread pool: reload tables, invalidate
>    column cache, return a fingerprint `version` and front-end `build`); `GET /designer/version`.
> 2. Server-side validation for columns the front end has no typed field for (type, length, nullable, mandatory;
>    never identity/computed/audit/key columns), saved from an `extras` object.
> 3. Frontend: a `designerStore` (cached promises + subscribe/emit with a `changes` counter and
>    `forceDesignerReload()`), a `useScreenFields(path)` hook built on `useSyncExternalStore` whose effect depends on
>    `changes`, a field engine (`toFieldConfig`, `applyDefinition`, conditional rules), and a top-bar Refresh button
>    that calls the reload endpoint then `forceDesignerReload`.
> 4. Do NOT poll or re-read on page load, focus, or after saves. Refresh happens only from the button. Typed form
>    values must survive a redraw.
> 5. Add tests, then run the project's test, lint and build commands and report real results.
