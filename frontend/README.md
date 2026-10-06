# vLQMS web (React + TypeScript + Vite)

The browser front end of vLQMS, Softvent's liquor-store ERP: masters, purchase and sales transactions, stock, the Daily Summary Report (DSR) and other reports. It talks to the FastAPI backend in `../backend`.

## Run

```bash
npm install
npm run dev      # http://localhost:5173; calls the backend at VITE_API_URL (frontend/.env, e.g. http://localhost:8001)
npm run build    # type-check (tsc -b) and production build into dist/
npm run lint     # eslint
npm test         # Vitest + React Testing Library (watch); npm run test:run runs once
```

Start the backend first (see `../backend/README.md`). Without it the screens show "API: offline" and API calls fail. Sign in with one of the shop's own users (there are no demo logins; the first run asks you to connect the database on the sign-in page).

## Where things are

Every source file starts with a comment that explains what it does, so open the file to read its purpose.

```
src/
├── main.tsx              entry point
├── App.tsx               login gate, app shell, and the switch that picks the screen for the active module
├── lib/                  code without UI
│   ├── api.ts            the only backend client: address (VITE_API_URL), login token, one function per endpoint, response types
│   ├── modules.ts        every module (screen) name, sidebar groups, search words, canOpen()
│   ├── styles.ts         shared Tailwind class strings (panel, field, buttons, tables) and money()
│   └── csv.ts, xlsExport.ts, formatters.ts, utils.ts
├── hooks/                useCompanyProfile()
├── data/                 prototype data still in use (sample stock, invoice constants)
├── constants/images.ts   bundled images
├── img/                  image files
└── components/
    ├── common/           Modal, confirm dialog, popups (Toast + toastStore), form fields and errors
    ├── layout/           Sidebar, module search, logo
    ├── auth/             Login
    ├── dashboard/        Dashboard
    ├── admin/            Admin Master: users, roles, rights
    ├── masters/          MasterCrud engine + all master screens
    ├── transactions/     Sales Invoice, Purchase Bill, stock transactions, expenses, closing stock, cash count
    ├── reports/          table-driven reports (reportDefs) + DSR, Stock Balance, Customised Reports
    ├── print/            legacy .rpt print formats and their web layouts
    └── ui/               third-party chart/loader components
```

How a screen is shown: the module name (e.g. `'Item Master'`) is listed in `lib/modules.ts`, drawn by `layout/Sidebar.tsx`, allowed by the user's role (`canOpen`), and rendered by a `case` in `App.tsx`. The same name is used in the backend's `ROLES`.

To add something, follow the recipes in `../.claude/skills/add-master/SKILL.md` and `../.claude/skills/add-report/SKILL.md`. `LEGACY_PARITY.md` lists each legacy screen and how complete it is here.

Developer notes for Claude Code are in `../.claude/CLAUDE.md` and `CLAUDE.md` (styling).
