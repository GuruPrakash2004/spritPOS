# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## What this is

A UI prototype of **Softvent vLQMS**, a cloud ERP for an Indian (Karnataka, CL-2 excise licence) liquor retail store, "THE LIQUOR HOUSE". It is the web successor to Softvent's legacy Windows desktop product, which is kept alongside for reference. The web UI is front-end only: no backend, no API, no persistence. All data is hardcoded sample data, and Save/Export/Generate actions are stubs.

```
.claude/              this file, settings, rules/ and skills/ (see below)
frontend/
  src/, package.json… React + TypeScript + Vite + Tailwind v4 app (the product)
  models/             Version1-5.html design explorations + Design.md
  LEGACY_PARITY.md    legacy feature list vs. what the React app covers
  smaple.css          scraped reference stylesheet, inspiration only; nothing imports it
  test.png            full-page reference screenshot (3840×16384)
MAIN_Root/vLQMSAPP/   legacy vLQMS desktop install (read-only reference, gitignored)
CLAUDE.local.md       personal, machine-specific notes (gitignored)
```

`frontend/models/Version1-5.html` are single-file design explorations of the same app; **Version5 "Midnight Sunset"** is the theme the React app follows.

## Commands (run from `frontend/`)

```bash
npm install
npm run dev      # Vite dev server with HMR
npm run build    # tsc -b && vite build
npm run lint     # eslint .
npm run preview  # serve the production build
```

Backend (run from `backend/`): `python3 -m venv .venv && .venv/bin/pip install -r requirements.txt`, then `.venv/bin/uvicorn app.main:app --reload --port 8000`. Swagger is at http://localhost:8000/docs. It's a FastAPI rebuild of the legacy screens (no .NET source exists), with in-memory data that resets on restart. Vite proxies `/api/*` to it; the React side calls it through `components/api.ts`. Every screen except Company Master and Supplier Master reads or writes through the API; the app opens on a demo Login (ADMIN/admin).

There is no test runner. Verify changes with `npm run build && npm run lint`. The `frontend/models/*.html` prototypes need no build: open them in a browser.

## Where the rest of the guidance lives

Claude Code loads everything in `.claude/rules/` automatically. Rules with a `paths:` header load only when working on matching files.

| File | Scope | Covers |
|---|---|---|
| `rules/code-style.md` | always | color palette, neutrals, component conventions, toasts, lint gotchas |
| `rules/domain.md` | always | liquor-retail and legacy vocabulary |
| `rules/legacy-app.md` | always | what `MAIN_Root/` is, where its specs are, what's sensitive |
| `rules/frontend/react.md` | `frontend/src/**` | shell/routing, component layout, Sales Invoice flow, theming internals |
| `rules/frontend/prototypes.md` | `frontend/models/**` | structure of the vanilla prototypes, pixel-matching workflow |

Skills in `.claude/skills/`:
- `add-module`: the steps for adding a new ERP screen.
- `legacy-docs`: how to read the legacy PDFs/xlsx/docx and exe on this Mac.

**Before building any module, read `frontend/LEGACY_PARITY.md`** for what the legacy screen did, and update its status row when you're done.

## Repo hygiene

- The root `.gitignore` excludes `MAIN_Root/` (~211MB, contains plaintext DB credentials), `CLAUDE.local.md` and `.claude/settings.local.json`.
- `frontend/.gitignore` excludes `node_modules` and `dist`.
- Stage paths explicitly rather than `git add .`.
