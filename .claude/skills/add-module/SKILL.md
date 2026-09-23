---
name: add-module
description: Add a new ERP screen (master, transaction or report) to the vLQMS React app, or turn an existing GenericModule stub into a real screen. Use when asked to build, add or implement a module such as Closing Stock Entry, Price List Maintenance or Stock Ledger.
---

# Adding a module to the React app

All paths are relative to `frontend/VLQM/src/`.

1. **Check the legacy spec first.** Find the module's row in `frontend/LEGACY_PARITY.md`, then read its section in the legacy help doc or spec sheet (see the `legacy-docs` skill). Build what the legacy screen does; don't invent fields.

2. **Pick a routing key**: the exact module name, e.g. `'Closing Stock Entry'`. The same string must appear in all three places below; a mismatch silently shows the placeholder screen.

3. **Write the component.**
   - Small (< ~100 lines): export it by name from `components/OperationalModules.tsx`.
   - Larger: create `components/<ModuleName>.tsx` with a default export.
   - Shared constants and types go in a separate `.ts` file, because of the only-export-components lint rule.
   - Sample data derives from `components/stockData.ts` where possible.

4. **Add the `case`** to the `switch` in `App.tsx`'s `renderContent()`.

5. **Add the sidebar entry** in `components/Sidebar.tsx`: a `<SidebarItem>` under the right section (Masters / Transactions / Reports) with a `lucide-react` icon, the label, `onOpenModule('<key>')` and `active={activeTab === '<key>'}`.

6. **Follow the style rules** (`.claude/rules/code-style.md`):
   - Version5 hexes, with `dark:` variants on everything.
   - `stone`/`slate` for neutrals.
   - Orange primary buttons.
   - `<Toast>` for feedback.
   - `rounded-2xl shadow-sm` for cards.

7. **Verify**: from `frontend/VLQM/`, run `npm run build && npm run lint`.

8. **Update `frontend/LEGACY_PARITY.md`**: set the module's status and list any legacy features still missing.
