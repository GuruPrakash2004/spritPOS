# Frontend notes

## Styling

- Tailwind v4 through `@tailwindcss/vite`. There is no tailwind config file. Colours are hard-coded hex values, each with an explicit `dark:` variant. Light mode uses burgundy `#64283A` on cream `#FFFCF7`/`#F3EEE6` with `#E3D9CD` borders. Dark mode uses gold `#D4A847` on `#141414`/`#1A1A1A` with `#2A2A2A` borders. Dark mode is a `dark` class that `App.jsx` toggles. Global styles and keyframes (such as `animate-shake`) are in `src/index.css`. `src/index.luxury.css` is not imported anywhere.
- Use the shared class strings in `src/lib/styles.js` (`panel`, `field`, `primaryButton`, `tableHead`, …) and `money()` instead of writing new class lists. Icons come from `lucide-react`. Charts use `recharts`, and animations use `framer-motion`. `@/` is an alias for `src/`, and `cn()` (clsx + tailwind-merge) is in `src/lib/utils.js`.
- `src/components/ui/` holds pasted-in components: `c-chart-21.jsx` (the Dashboard chart), and `chart.jsx`/`card.jsx`, which they use. `demo.jsx` is not imported anywhere.
- Logo: `components/layout/VlqmsLogo.jsx` renders the Softvent logo from `src/img/brand/` (`softvent_logo.png` for light backgrounds, `softvent_logo_gold.png`, gold and ivory, for dark mode; it switches with dark mode, and `onDark` forces the gold one on photos). The browser-tab icon is `public/favicon.png` (plus `apple-touch-icon.png`). The product is called **e-vLQMS ERP**; `textOnly` shows just that name, which is what the sign-in page's top and the sidebar use (the Softvent logo itself appears in "Developed by" and About). Use this component rather than adding the logo image elsewhere.
- Images: brand photos are in `src/img/products/` and drink photos per category in `src/img/products/stock/` (credits in its `CREDITS.md`; Unsplash License or supplied by Softvent). `src/constants/images.js` maps an item to a photo by brand, then category (whisky, scotch, rum, vodka, gin, brandy, wine, beer, soft drinks, accessories). Use `distinctProductPhotos(items)` when several items are on screen so no photo repeats. Add photos as local files there, not remote URLs, and don't use photos of another brand's labelled bottle for an item.

## Forms

- New forms follow `.claude/rules/popups-and-errors.md`. Use `TextField` (`FormField.jsx`) and `InlineError` for per-field errors, `FormErrorSummary` for the summary card (both in `FormErrorSummary.jsx`), and `toastStore.js` for server results. `MasterCrud.jsx` shows the full pattern, including arrow-key field movement and ⌘/Ctrl+S.
- Every screen must work without a mouse (billing counters rarely use one): screen keys with `useShortcuts()` and Enter-to-next-field with `enterMovesNext` (`lib/keys.js`), key hints with `<Kbd />`, dialogs through `Modal` (or `useDialogKeys` for a custom one). See `.claude/rules/popups-and-errors.md` §4.
- Pop-up menus ("⋯ More options" and similar) use `common/DropdownMenu.jsx`, never an `absolute` div under the button: it renders in a portal, keeps the menu inside the screen on phones, and flips upwards near the bottom.
- Export helpers are `csv.js` (CSV) and `xlsExport.js` (Excel-readable XLS). `formatters.js` has `numberToWords()` for amounts in words.
