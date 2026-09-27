# Frontend notes

## Styling

- Tailwind v4 through `@tailwindcss/vite`. There is no tailwind config file. Colours are hard-coded hex values, each with an explicit `dark:` variant. Light mode uses burgundy `#64283A` on cream `#FFFCF7`/`#F3EEE6` with `#E3D9CD` borders. Dark mode uses gold `#D4A847` on `#141414`/`#1A1A1A` with `#2A2A2A` borders. Dark mode is a `dark` class that `App.tsx` toggles. Global styles and keyframes (such as `animate-shake`) are in `src/index.css`. `src/index.luxury.css` is not imported anywhere.
- Use the shared class strings in `src/lib/styles.ts` (`panel`, `field`, `primaryButton`, `tableHead`, …) and `money()` instead of writing new class lists. Icons come from `lucide-react`. Charts use `recharts`, and animations use `framer-motion`. `@/` is an alias for `src/`, and `cn()` (clsx + tailwind-merge) is in `src/lib/utils.ts`.
- `src/components/ui/` holds pasted-in components: `c-chart-21.tsx` (the Dashboard chart), `runway-loader.tsx` (the toast countdown bar), and `chart.tsx`/`card.tsx`, which they use. `demo.tsx` is not imported anywhere.
- Images: product and brand photos are in `src/img/` (imported through `src/constants/images.ts`) and `public/products/`.

## Forms

- New forms follow `.claude/rules/popups-and-errors.md`. Use `TextField` (`FormField.tsx`) and `InlineError` for per-field errors, `FormErrorSummary` for the summary card (both in `FormErrorSummary.tsx`), and `toastStore.ts` for server results. `MasterCrud.tsx` shows the full pattern, including arrow-key field movement and ⌘/Ctrl+S.
- Export helpers are `csv.ts` (CSV) and `xlsExport.ts` (Excel-readable XLS). `formatters.ts` has `numberToWords()` for amounts in words.
