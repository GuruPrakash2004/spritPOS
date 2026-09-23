# Code style

## General
- Match the surrounding code: naming, comment density, Tailwind class ordering. Comments explain *why*, not what.
- Everything is sample data. Actions with no real behaviour are stubs (`alert()` or a toast). Don't invent backends, fetch calls or storage.
- Keep UI text in the store's domain language (see `domain.md`). Amounts: `₹` with Indian grouping via `toLocaleString('en-IN', …)`. Amount-in-words: `numberToWords()` from `components/formatters.ts`.

## Colors: Version5 Midnight Sunset
Colors are **literal Tailwind arbitrary values**, not theme tokens. Use these hexes; don't invent new ones.

| Role | Light | Dark |
|---|---|---|
| page bg | `#f6efe8` | `#0e101b` |
| panel | `#fffaf5` | `#171b2b` |
| panel-2 / raised | `#f1e3d8` | `#20253a` |
| line | `#e1c9b6` | `#343b59` (strong: `#414a6e`) |
| accent | `#e8702a` | `#e8702a` |
| accent text | `#b84716` | `#ffb07e` |
| brand gradient | `#e8702a` → `#e75b6f` | same |
| hero gradient | `#27233b` → `#4b2840` → `#8b422b` | same |

- **Every new piece of UI needs explicit `dark:` variants.** Dark mode is a manual `.dark` class, not the media query, so nothing falls back automatically.
- **Neutrals:** use `stone-*` / `slate-*` only. Both are remapped to theme-aware variables in `index.css`. `gray`/`zinc`/`neutral` are not remapped and will look wrong.
- **Buttons:** primary actions (Save, Generate, Export) use `bg-[#e8702a] hover:bg-[#b84716]`. Emerald is for success/status only (toasts, "Active" badges, balanced indicators). Red is for destructive actions and errors.
- **Cards:** `div.rounded-2xl.shadow-sm` inside the workspace automatically gets the 22px radius, warm shadow and hover lift. Use those two classes for content cards. Don't put forms that need a steady layout inside a hovering card.

## Components
- **Toasts:** use `components/Toast.tsx` (`<Toast kind="ok|error" text=… />`). It portals to `<body>`. Never render `fixed` overlays inline inside a module: `<main class="workspace-backdrop">` is a stacking context below the z-20 header, and hovered cards apply a transform, so inline `fixed`/`z-50` elements get hidden or displaced.
- **File size:** promote a module out of `components/OperationalModules.tsx` into its own file once it passes ~100 lines.
- **Shared data:** shared constants/types go in a non-component `.ts` file (e.g. `salesCounters.ts`, `stockData.ts`). The `react-refresh/only-export-components` lint rule fails if a component file exports constants.
- **Icons:** `lucide-react`. Remove icon imports you stop using, or `tsc -b` fails the build.
