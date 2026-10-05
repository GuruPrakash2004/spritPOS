// Tools → SQL Query Execution (legacy frmUtlSQLQueryExec, shortcut 600). Administrator only.
// Runs on the vLQMS SQL Server database, where changes are allowed only on the web app's own Web_* tables
// (the old desktop tables stay untouched). The web app keeps its data in memory from startup, so a change
// made here shows on the screens after the backend restarts. The backend decides what may run
// (backend/app/routers/tools.py); a change asks first, then waits for its popup, then shows "N row(s) affected".
//
// Left: object explorer (Tables / Views / Procedures) with a name filter and a hide/pin toggle; each
// object's ⋯ menu (or right-click) offers Select Top 1000 Rows, View Top 200 Rows (the desktop's Edit Top
// 200 Rows, without editing), Script As CREATE / DROP, or the view/procedure definition (double-click).
// Right: the query box (F5 or Ctrl/⌘+Enter runs the selected text, or everything), the result grid with
// "Total no of rows", time taken and the server's error text, CSV / XLS export and the last 20 queries.
// Running a query, loading rows and exporting wait for their popup (Cancel stops them).

import { useCallback, useEffect, useMemo, useRef, useState } from 'react';
import { ChevronDown, ChevronRight, Database, Download, Eraser, FileSpreadsheet, History, MoreHorizontal, PanelLeftClose, PanelLeftOpen, Play, RefreshCw, Search } from 'lucide-react';
import Modal from '../common/Modal';
import { useConfirm } from '../common/ConfirmDialog';
import DropdownMenu, { menuItem } from '../common/DropdownMenu';
import { toastStore } from '../common/toastStore';
import { api, errorText, qs, type ApiSqlObject, type ApiSqlResult, type ApiSqlStatus, type ApiSqlTarget, type ApiSqlTop } from '../../lib/api';
import { downloadText, toCsv } from '../../lib/csv';
import { downloadXls } from '../../lib/xlsExport';
import { exportButton, field, ghostButton, iconButton, pageHeader, pageSub, pageTitle, panel, primaryButton, secondaryButton, tableBody, tableHead } from '../../lib/styles';

const HISTORY_KEY = 'vlqms_sql_history';
const KINDS: ApiSqlObject['kind'][] = ['Tables', 'Views', 'Procedures'];

const readHistory = (): string[] => {
  try { return JSON.parse(localStorage.getItem(HISTORY_KEY) || '[]'); } catch { return []; }
};
const writeHistory = (list: string[]) => {
  try { localStorage.setItem(HISTORY_KEY, JSON.stringify(list)); } catch { /* private mode: history just isn't kept */ }
};

const cellText = (v: unknown) => (v === null || v === undefined ? 'NULL' : String(v));

// INSERT / UPDATE / DELETE (after any leading comments): these ask before they run.
const writeVerb = (sql: string) => {
  const head = sql.replace(/^(\s*(--[^\n]*\n?|\/\*[\s\S]*?\*\/))*/, '').trimStart().replace(/^\(\s*/, '');
  return /^(insert|update|delete)\b/i.exec(head)?.[1].toUpperCase() ?? null;
};

// Result grid shared by the main result and the View Top 200 window.
function ResultGrid({ result, startAt = 1 }: { result: ApiSqlResult; startAt?: number }) {
  if (!result.columns.length) return <p className="p-4 text-xs text-stone-500">The query returned no result set.</p>;
  if (!result.rows.length) return <p className="p-4 text-xs text-stone-500">No rows found.</p>;
  return (
    <table className="min-w-full text-xs">
      <thead>
        <tr>
          <th className={`${tableHead} px-2 py-2 text-right`}>#</th>
          {result.columns.map((c, i) => <th key={i} className={`${tableHead} px-3 py-2`}>{c}</th>)}
        </tr>
      </thead>
      <tbody className={tableBody}>
        {result.rows.map((row, r) => (
          <tr key={r} className="hover:bg-[#FAF6EF] dark:hover:bg-subtle/70">
            <td className="px-2 py-1 text-right font-mono text-[11px] text-stone-400">{startAt + r}</td>
            {row.map((v, c) => (
              <td key={c} className={`whitespace-nowrap px-3 py-1 font-mono text-[11px] ${v === null ? 'italic text-stone-400' : typeof v === 'number' ? 'text-right' : ''}`}>
                {cellText(v)}
              </td>
            ))}
          </tr>
        ))}
      </tbody>
    </table>
  );
}

export default function SqlQueryExecution() {
  const [status, setStatus] = useState<ApiSqlStatus | null>(null);
  const target: ApiSqlTarget = 'sqlserver';
  const { confirm, dialog } = useConfirm();
  const [objects, setObjects] = useState<ApiSqlObject[]>([]);
  const [search, setSearch] = useState('');
  const [open, setOpen] = useState<Record<string, boolean>>({ Tables: true, Views: false, Procedures: false });
  const [explorerShown, setExplorerShown] = useState(true);
  const [sql, setSql] = useState('');
  const [running, setRunning] = useState(false);
  const [result, setResult] = useState<ApiSqlResult | null>(null);
  const [error, setError] = useState('');
  const [history, setHistory] = useState<string[]>(readHistory);
  const [historyOpen, setHistoryOpen] = useState(false);
  const [menuFor, setMenuFor] = useState<ApiSqlObject | null>(null);
  const [top, setTop] = useState<{ obj: ApiSqlObject; data: ApiSqlTop | null; loading: boolean } | null>(null);
  const editor = useRef<HTMLTextAreaElement>(null);
  const menuAnchor = useRef<HTMLElement | null>(null);
  const historyRef = useRef<HTMLButtonElement>(null);
  const serverUp = status?.db_connected === true;
  const connected = status !== null && serverUp;

  useEffect(() => {
    api<ApiSqlStatus>('/tools/sql/status')
      .then(setStatus)
      .catch(e => { setStatus({ db_connected: false, database: null, driver: null }); setError(errorText(e)); });
  }, []);

  // Explorer list, filtered on the server by name (debounced while typing).
  useEffect(() => {
    if (!connected) return;
    const t = window.setTimeout(() => {
      api<ApiSqlObject[]>(`/tools/sql/objects${qs({ search: search.trim(), target })}`).then(setObjects).catch(e => setError(errorText(e)));
    }, 250);
    return () => window.clearTimeout(t);
  }, [connected, search, target]);

  const grouped = useMemo(() => Object.fromEntries(KINDS.map(k => [k, objects.filter(o => o.kind === k)])) as Record<string, ApiSqlObject[]>, [objects]);

  // Run `text` (the selection or the whole box). Waits for its popup; Cancel stops it.
  const run = useCallback(async (text: string) => {
    const query = text.trim();
    if (!query) { setError('Type a query to execute.'); editor.current?.focus(); return; }
    const verb = writeVerb(query);
    if (verb && !(await confirm(`Run this ${verb} on the vLQMS SQL Server? The change is saved immediately and can't be undone from here.`))) return;
    if (!(await toastStore.defer('ok', verb ? 'Changes will be saved when this closes.' : 'Query will run when this closes.',
      verb ? 'Cancelled. Nothing was changed.' : 'Query cancelled. Nothing was run.'))) return;
    setRunning(true);
    setError('');
    try {
      const res = await api<ApiSqlResult>('/tools/sql/execute', { method: 'POST', body: { sql: query, target } });
      setResult(res);
      const next = [query, ...history.filter(h => h !== query)].slice(0, 20);
      setHistory(next);
      writeHistory(next);
      if (res.kind === 'write') toastStore.add('ok', `${res.rows_affected} row(s) affected in ${res.table}.`);
      else toastStore.add('ok', `${res.row_count} row${res.row_count === 1 ? '' : 's'} returned in ${res.elapsed_ms} ms.`);
    } catch (e) {
      setResult(null);
      setError(errorText(e));
      toastStore.add('error', 'The query could not be run. See the message under the query box.');
    } finally {
      setRunning(false);
    }
  }, [history, target, confirm]);

  const selectedOrAll = () => {
    const el = editor.current;
    if (el && el.selectionEnd > el.selectionStart) return sql.slice(el.selectionStart, el.selectionEnd);
    return sql;
  };

  const onEditorKey = (e: React.KeyboardEvent<HTMLTextAreaElement>) => {
    if (e.key === 'F5' || ((e.ctrlKey || e.metaKey) && e.key === 'Enter')) {
      e.preventDefault();
      if (!running && connected) run(selectedOrAll());
    }
  };

  const scriptInto = async (obj: ApiSqlObject, kind: 'create' | 'drop' | 'select' | 'definition', thenRun = false) => {
    setMenuFor(null);
    try {
      const text = kind === 'definition'
        ? (await api<{ definition: string }>(`/tools/sql/definition${qs({ name: obj.name, schema: obj.schema, target })}`)).definition
        : (await api<{ script: string }>(`/tools/sql/script${qs({ name: obj.name, schema: obj.schema, kind, target })}`)).script;
      setSql(text);
      setError('');
      editor.current?.focus();
      if (thenRun) await run(text);
    } catch (e) {
      toastStore.add('error', errorText(e));
    }
  };

  const loadTop = async (obj: ApiSqlObject, ask = true) => {
    setMenuFor(null);
    if (ask && !(await toastStore.defer('refresh', `Top 200 rows of ${obj.name} will load when this closes.`, 'Loading cancelled.'))) return;
    setTop({ obj, data: top?.obj.name === obj.name ? top.data : null, loading: true });
    try {
      const data = await api<ApiSqlTop>(`/tools/sql/top${qs({ name: obj.name, schema: obj.schema, n: 200, target })}`);
      setTop({ obj, data, loading: false });
    } catch (e) {
      setTop(null);
      toastStore.add('error', errorText(e));
    }
  };

  const exportResult = async (format: 'csv' | 'xls') => {
    if (!result?.rows.length) return toastStore.add('error', 'Nothing to export: run a query that returns rows first.');
    const what = format === 'csv' ? 'CSV' : 'XLS';
    if (!(await toastStore.defer('ok', `${result.row_count} rows will be exported to ${what} when this closes.`, 'Export cancelled.'))) return;
    const name = `SQL_Query_${new Date().toISOString().slice(0, 16).replace(/[-:T]/g, '')}`;
    if (format === 'csv') {
      downloadText(`${name}.csv`, toCsv([result.columns, ...result.rows.map(r => r.map(cellText))]));
    } else {
      const cols = result.columns.map((c, i) => ({ key: `c${i}`, label: c, align: 'left' as const }));
      downloadXls(name, 'SQL Query Execution', `${status?.database ?? ''} · ${result.row_count} rows`, cols,
        result.rows.map(r => Object.fromEntries(r.map((v, i) => [`c${i}`, typeof v === 'number' ? v : v === null ? null : String(v)]))));
    }
    toastStore.add('ok', `${result.row_count} rows exported to ${what}.`);
  };

  const openMenu = (obj: ApiSqlObject, el: HTMLElement) => {
    menuAnchor.current = el;
    setMenuFor(obj);
  };

  return (
    <div className="mx-auto flex h-full max-w-[1600px] flex-col gap-4">
      {dialog}
      <div className={pageHeader}>
        <div>
          <h1 className={pageTitle}>SQL Query Execution</h1>
          <p className={pageSub}>
            Queries on the vLQMS database. SELECT anything; INSERT, UPDATE and DELETE work on the web app's Web_* tables only,
            never on the old desktop tables. The screens show a change after the backend restarts.
          </p>
        </div>
        <div className="flex flex-wrap items-center gap-3 text-xs text-[#70675F] dark:text-stone-400">
          <span className="flex items-center gap-1.5"><Database size={14} />DB Name : <strong className="text-[#29231F] dark:text-stone-100">{status?.database ?? '—'}</strong></span>
          <span className={`inline-flex items-center gap-1.5 ${connected ? 'text-emerald-700 dark:text-emerald-400' : 'text-[#9A4B45] dark:text-red-400'}`}>
            <span className={`h-1.5 w-1.5 rounded-full ${connected ? 'bg-emerald-500' : 'bg-[#9A4B45]'}`} />
            {status === null ? 'Checking…' : connected ? 'Connected' : 'Not connected'}
          </span>
        </div>
      </div>

      {status && !connected && (
        <div className={`${panel} px-5 py-4 text-sm text-[#70675F] dark:text-stone-300`}>
          SQL Server is not connected. It works once the backend can reach the shop's database (set up from the sign-in page's Database setup).
        </div>
      )}

      <div className="flex min-h-0 flex-1 gap-4">
        {explorerShown && (
          <aside className={`${panel} flex w-72 shrink-0 flex-col overflow-hidden`} aria-label="Database objects">
            <div className="flex items-center gap-2 border-b border-[#E3D9CD] p-3 dark:border-line">
              <div className="relative flex-1">
                <Search size={13} className="pointer-events-none absolute left-2.5 top-1/2 -translate-y-1/2 text-stone-400" />
                <input value={search} onChange={e => setSearch(e.target.value)} placeholder="Find table, view, procedure" aria-label="Find database object"
                  className={`${field} pl-7`} disabled={!connected} />
              </div>
              <button className={iconButton} onClick={() => setExplorerShown(false)} title="Hide object list" aria-label="Hide object list"><PanelLeftClose size={16} /></button>
            </div>
            <div className="min-h-0 flex-1 overflow-y-auto p-2 text-xs">
              {KINDS.map(kind => (
                <div key={kind}>
                  <button onClick={() => setOpen({ ...open, [kind]: !open[kind] })} aria-expanded={open[kind]}
                    className="flex w-full items-center gap-1 rounded-md px-1.5 py-1 font-semibold text-[#29231F] hover:bg-[#EFE9DF] dark:text-stone-100 dark:hover:bg-subtle">
                    {open[kind] ? <ChevronDown size={14} /> : <ChevronRight size={14} />}{kind}
                    <span className="ml-auto text-[10px] font-normal text-stone-400">{grouped[kind]?.length ?? 0}</span>
                  </button>
                  {open[kind] && grouped[kind]?.map(o => (
                    <div key={`${o.schema}.${o.name}`}
                      onContextMenu={e => { e.preventDefault(); openMenu(o, e.currentTarget); }}
                      onDoubleClick={() => scriptInto(o, o.kind === 'Tables' ? 'select' : 'definition')}
                      className="group flex items-center rounded-md py-0.5 pl-6 pr-1 hover:bg-[#EFE9DF] dark:hover:bg-subtle">
                      <span className="flex-1 truncate font-mono text-[11px]" title={`${o.schema}.${o.name}`}>{o.schema !== 'dbo' && o.schema !== 'main' && `${o.schema}.`}{o.name}</span>
                      <button onClick={e => openMenu(o, e.currentTarget)} aria-label={`Options for ${o.name}`}
                        className="rounded p-0.5 text-stone-400 opacity-0 hover:text-stone-700 focus:opacity-100 group-hover:opacity-100 dark:hover:text-stone-100">
                        <MoreHorizontal size={14} />
                      </button>
                    </div>
                  ))}
                </div>
              ))}
            </div>
          </aside>
        )}

        <section className="flex min-w-0 flex-1 flex-col gap-3">
          <div className={`${panel} flex flex-col gap-2 p-3`}>
            <div className="flex flex-wrap items-center gap-2">
              {!explorerShown && <button className={iconButton} onClick={() => setExplorerShown(true)} title="Show object list" aria-label="Show object list"><PanelLeftOpen size={16} /></button>}
              <button className={primaryButton} disabled={!connected || running} onClick={() => run(selectedOrAll())} title="Execute : F5 (runs the selected text, or everything)">
                {running ? <RefreshCw size={14} className="animate-spin" /> : <Play size={14} />}{running ? 'Running…' : 'Execute Query'}
              </button>
              <button className={ghostButton} onClick={() => { setSql(''); setResult(null); setError(''); editor.current?.focus(); }}><Eraser size={14} />Clear text</button>
              <button ref={historyRef} className={ghostButton} onClick={() => setHistoryOpen(!historyOpen)} aria-expanded={historyOpen} disabled={!history.length}><History size={14} />Recent</button>
              <DropdownMenu anchorRef={historyRef} open={historyOpen} onClose={() => setHistoryOpen(false)} align="start" width={420}>
                {history.map((h, i) => (
                  <button key={i} className={`${menuItem} truncate font-mono text-[11px]`} title={h} onClick={() => { setSql(h); setHistoryOpen(false); editor.current?.focus(); }}>{h.replace(/\s+/g, ' ')}</button>
                ))}
              </DropdownMenu>
              <span className="ml-auto text-[11px] text-stone-400">F5 or Ctrl/⌘+Enter to execute</span>
            </div>
            <textarea ref={editor} value={sql} onChange={e => setSql(e.target.value)} onKeyDown={onEditorKey} spellCheck={false}
              aria-label="SQL query" placeholder="SELECT TOP 100 * FROM MstItem"
              className={`${field} h-44 resize-y font-mono text-[12px] leading-relaxed`} />
            {error && (
              <pre role="alert" className="max-h-32 overflow-auto whitespace-pre-wrap rounded-lg border border-red-500/30 bg-red-50 p-2.5 font-mono text-[11px] text-red-800 dark:bg-red-950/30 dark:text-red-200">{error}</pre>
            )}
          </div>

          <div className={`${panel} flex min-h-0 flex-1 flex-col overflow-hidden`}>
            <div className="flex flex-wrap items-center gap-3 border-b border-[#E3D9CD] px-4 py-2 text-xs dark:border-line">
              {result?.kind === 'write'
                ? <span className="font-semibold">{result.rows_affected} row(s) affected in {result.table}</span>
                : <span className="font-semibold">Total no of rows : {result?.row_count ?? 0}</span>}
              {result && <span className="text-stone-500">{result.elapsed_ms} ms</span>}
              {result?.truncated && <span className="text-amber-700 dark:text-amber-400">Only the first 5,000 rows are shown. Narrow the query to see the rest.</span>}
              <div className="ml-auto flex gap-2">
                <button className={exportButton} onClick={() => exportResult('csv')} disabled={!result?.rows.length}><Download size={14} />CSV</button>
                <button className={exportButton} onClick={() => exportResult('xls')} disabled={!result?.rows.length}><FileSpreadsheet size={14} />XLS</button>
              </div>
            </div>
            <div className="min-h-0 flex-1 overflow-auto">
              {result?.kind === 'write'
                ? <p className="p-4 text-xs text-emerald-700 dark:text-emerald-400">{result.rows_affected} row(s) affected. The change is saved.</p>
                : result ? <ResultGrid result={result} /> : <p className="p-4 text-xs text-stone-500">Results appear here.</p>}
            </div>
          </div>
        </section>
      </div>

      <DropdownMenu anchorRef={menuAnchor} open={menuFor !== null} onClose={() => setMenuFor(null)} align="start" width={220}>
        {menuFor && menuFor.kind !== 'Procedures' && <>
          <button className={menuItem} onClick={() => scriptInto(menuFor, 'select', true)}>Select Top 1000 Rows</button>
          <button className={menuItem} onClick={() => loadTop(menuFor)}>View Top 200 Rows</button>
        </>}
        {menuFor?.kind === 'Tables' && <>
          <button className={menuItem} onClick={() => scriptInto(menuFor, 'create')}>Script Table As → CREATE</button>
          <button className={menuItem} onClick={() => scriptInto(menuFor, 'drop')}>Script Table As → DROP</button>
        </>}
        {menuFor && menuFor.kind !== 'Tables' && (
          <button className={menuItem} onClick={() => scriptInto(menuFor, 'definition')}>Show definition</button>
        )}
      </DropdownMenu>

      {top && (
        <Modal title={`${top.obj.name}: top 200 rows`} onClose={() => setTop(null)} wide
          footer={<>
            <span className="mr-auto text-[11px] text-stone-500">
              {top.data?.writable
                ? `Change these rows with an UPDATE or DELETE query on ${top.obj.name}.`
                : "Read-only: this table can't be changed from the web app."}
            </span>
            <button className={secondaryButton} onClick={() => loadTop(top.obj)} disabled={top.loading}><RefreshCw size={14} className={top.loading ? 'animate-spin' : ''} />Reload</button>
            <button className={ghostButton} onClick={() => setTop(null)}>Close</button>
          </>}>
          {top.data ? (
            <>
              <p className="mb-2 text-xs text-stone-500">No of Records in {top.obj.name}: {top.data.total_rows} · showing {top.data.row_count}</p>
              <div className="max-h-[60vh] overflow-auto rounded-lg border border-[#E3D9CD] dark:border-line"><ResultGrid result={top.data} /></div>
            </>
          ) : <p className="p-4 text-xs text-stone-500">Loading…</p>}
        </Modal>
      )}
    </div>
  );
}
