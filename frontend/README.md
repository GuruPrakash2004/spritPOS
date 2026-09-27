# vLQMS web (React + TypeScript + Vite)

The browser front end of vLQMS, Softvent's liquor-store ERP: masters, purchase and sales transactions, stock, the Daily Summary Report (DSR) and other reports. It talks to the FastAPI backend in `../backend`.

## Run

```bash
npm install
npm run dev      # http://localhost:5173, proxies /api/* to the backend on :8001
npm run build    # type-check (tsc -b) and production build into dist/
npm run lint     # eslint
```

Start the backend first (see `../backend/README.md`). Without it the screens show "API: offline" and API calls fail. Log in as `ADMIN` / `admin` to see every module. `GURU`/`guru` is a Manager, and `PRADEEP`/`pradeep` and `RAVI`/`ravi` are Cashiers.

## Where things are

- `src/App.tsx`: login, the app shell, and a `switch` on the active module name. There is no router library. The only URL route is `/adminMaster`.
- `src/components/modules.ts`: module names and groups (sidebar, search, role rights). `Sidebar.tsx` lists them with icons.
- `src/components/api.ts`: the fetch wrapper (`api<T>()`), auth token handling, and response types.
- Generic engines: `MasterCrud.tsx` + `masterConfigs.ts` (simple masters), `reports/reportDefs.ts` + `ApiReport.tsx` (tabular reports), `ItemLinesEntry.tsx` (item-line transactions), and `print/` (legacy `.rpt` print formats).
- `LEGACY_PARITY.md`: each legacy screen and how complete it is here.

Developer notes for Claude Code are in `../.claude/CLAUDE.md` and `CLAUDE.md` (styling).
