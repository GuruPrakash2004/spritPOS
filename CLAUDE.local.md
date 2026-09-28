# Local notes (this machine only, gitignored)

- macOS (zsh). Shell gotchas:
  - `$VAR` holding several paths does **not** word-split, so pass globs directly.
  - `echo ======` fails because zsh treats a leading `=` as command lookup.
  - There is no `timeout` command.
- `rtk` proxies CLI output (see `~/.claude/RTK.md`); `rtk proxy <cmd>` gives raw output.
- The dev server is usually left running from `frontend` (`npm run dev`). Restart it after moving folders or changing `vite.config.ts`.
- `backend/.venv` was copied from `~/Downloads/Saas/backend`: its `pip`/`uvicorn`/`pytest` scripts run the **other** copy's Python. Use `.venv/bin/python -m pip|uvicorn|pytest` here, or recreate the venv.
