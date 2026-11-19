// Tools → Re Write Stock (legacy frmStkRewrite + mSp_RewriteStkList, shortcut 604).
//
// Shows the desktop screen's five checks on the web app's stock: Purchase, Sales Invoice, Stock
// Deduction and Sales Return (document quantities "Trn.Stock" vs stock movements "Sys.Stock"), and
// Ledger vs QOH (opening + movements vs the item's QOH). Only mismatching items are listed.
// Re-Write rebuilds the movements from the documents and QOH from them (backend/app/stock_rewrite.py);
// it asks first, then waits for its popup (Cancel stops it). With SQL Server connected, the desktop
// database's own checks are shown too, read-only: those are fixed with Re Write Stock in the desktop app.

import { useCallback, useEffect, useRef, useState } from 'react';
import { CheckCircle2, AlertTriangle, FileSpreadsheet, RefreshCw, Wrench } from 'lucide-react';
import { downloadXls } from '../../lib/xlsExport';
import { useConfirm } from '../common/ConfirmDialog';
import { toastStore } from '../common/toastStore';
import { errorText, getRewriteStock, getRewriteStockDesktopTab, runRewriteStock, } from '../../lib/api';
import { badge, dangerButton, pageHeader, pageSub, pageTitle, panel, secondaryButton, sectionTitle, tableBody, tableHead } from '../../lib/styles';

const qty = (n) => (n === undefined ? '' : Number(n).toLocaleString('en-IN', { maximumFractionDigits: 3 }));

// The desktop database's checks, in the order of the desktop screen's tabs (keys as in backend stock_rewrite.LEGACY_KEYS).
const DESKTOP_TABS = [['purchase', 'Purchase'], ['sales_invoice', 'Sales Invoice'], ['stock_deduction', 'Stock Deduction'],
  ['sales_return', 'Sales Return'], ['ledger', 'Ledger vs QOH']];
const waitingDesktop = () => ({ tabs: DESKTOP_TABS.map(([key, caption]) => ({ key, caption, rows: [], error: null, loading: true })), rewrite_required: false });

function DiffTable({ tab }) {
  const ledger = tab.key === 'ledger';
  if (tab.loading) return <p className="flex items-center gap-2 p-4 text-xs text-stone-500"><RefreshCw size={14} className="animate-spin" />Checking {tab.caption} in the desktop database… the Sales Invoice check reads every bill ever saved and can take a while. You can keep working.</p>;
  if (tab.error) return <p className="p-4 text-xs text-[#9A4B45] dark:text-red-400">Could not check {tab.caption}: {tab.error}</p>;
  if (!tab.rows.length) return <p className="flex items-center gap-2 p-4 text-xs text-emerald-700 dark:text-emerald-400"><CheckCircle2 size={14} />No differences in {tab.caption}.</p>;
  return (
    <table className="min-w-full text-xs">
      <thead>
        <tr>
          <th className={`${tableHead} px-3 py-2`}>Item</th>
          <th className={`${tableHead} px-3 py-2 text-right`}>{ledger ? 'Ledger.Stock' : 'Trn.Stock'}</th>
          <th className={`${tableHead} px-3 py-2 text-right`}>{ledger ? 'QOH' : 'Sys.Stock'}</th>
          <th className={`${tableHead} px-3 py-2 text-right`}>Diff</th>
        </tr>
      </thead>
      <tbody className={tableBody}>
        {tab.rows.map(r => (
          <tr key={r.item_code}>
            <td className="px-3 py-1.5"><span className="font-medium">{r.item_name}</span> <span className="font-mono text-[11px] text-stone-400">{r.item_code}</span></td>
            <td className="px-3 py-1.5 text-right tabular-nums">{qty(ledger ? r.ledger_stock : r.trn_stock)}</td>
            <td className="px-3 py-1.5 text-right tabular-nums">{qty(ledger ? r.qoh : r.sys_stock)}</td>
            <td className="px-3 py-1.5 text-right font-semibold tabular-nums text-[#9A4B45] dark:text-red-400">{qty(r.diff)}</td>
          </tr>
        ))}
      </tbody>
    </table>
  );
}

function CheckPanel({ title, sub, check }) {
  const [active, setActive] = useState(check.tabs.find(t => t.rows.length)?.key ?? check.tabs[0]?.key);
  const tab = check.tabs.find(t => t.key === active) ?? check.tabs[0];
  return (
    <section className={`${panel} overflow-hidden`}>
      <div className="flex flex-wrap items-center gap-3 border-b border-[#E3D9CD] px-5 py-3 dark:border-line">
        <div className="mr-auto">
          <h2 className={sectionTitle}>{title}</h2>
          <p className="text-[11px] text-stone-500">{sub}</p>
        </div>
        {check.tabs.some(t => t.loading)
          ? <span className={badge('neutral')}><RefreshCw size={12} className="animate-spin" />Checking…</span>
          : check.rewrite_required
            ? <span className={badge('danger')}><AlertTriangle size={12} />Re-Write required</span>
            : <span className={badge('success')}><CheckCircle2 size={12} />Re-Write not Required</span>}
      </div>
      <div role="tablist" className="flex flex-wrap gap-1 border-b border-[#E3D9CD] px-3 pt-2 dark:border-line">
        {check.tabs.map(t => (
          <button key={t.key} role="tab" aria-selected={t.key === tab?.key} onClick={() => setActive(t.key)}
            className={`-mb-px rounded-t-lg border px-3 py-1.5 text-xs font-medium ${t.key === tab?.key
              ? 'border-[#E3D9CD] border-b-[#FFFFFF] bg-[#FFFCF7] text-[#64283A] dark:border-line dark:border-b-surface dark:bg-surface dark:text-[#D4A847]'
              : 'border-transparent text-stone-500 hover:text-stone-800 dark:hover:text-stone-200'}`}>
            {t.caption}{t.rows.length > 0 && <span className="ml-1.5 rounded-full bg-[#9A4B45] px-1.5 text-[10px] text-white">{t.rows.length}</span>}
          </button>
        ))}
      </div>
      <div className="max-h-[50vh] overflow-auto">{tab && <DiffTable tab={tab} />}</div>
    </section>
  );
}

export default function ReWriteStock() {
  const [data, setData] = useState(null);
  const [loading, setLoading] = useState(true);   // the first check starts on open
  const [writing, setWriting] = useState(false);
  const { confirm, dialog } = useConfirm();

  const round = useRef(0);   // a newer check makes the answers of an older one (still running on the server) be ignored

  // The web app's check is answered at once. The desktop database's five checks are asked for side by side afterwards and
  // fill in as each one arrives (the Sales Invoice one reads every bill ever saved), so the screen is never held up by them.
  // Resolves true when the web check loaded (the popup for a failure is shown here).
  const load = useCallback((fresh = false) => {
    const mine = ++round.current;
    return getRewriteStock()
      .then(d => {
        if (mine !== round.current) return true;
        setData({ ...d, desktop: d.db_connected ? waitingDesktop() : null });
        if (d.db_connected) {
          DESKTOP_TABS.forEach(([key, caption]) => {
            getRewriteStockDesktopTab(key, fresh)
              .then(tab => ({ ...tab, loading: false }))
              .catch(e => ({ key, caption, rows: [], error: errorText(e), loading: false }))
              .then(tab => {
                if (mine !== round.current) return;
                setData(cur => {
                  if (!cur?.desktop) return cur;
                  const tabs = cur.desktop.tabs.map(t => (t.key === key ? tab : t));
                  return { ...cur, desktop: { tabs, rewrite_required: tabs.some(t => t.rows.length > 0) } };
                });
              });
          });
        }
        return true;
      })
      .catch(e => { toastStore.add('error', errorText(e)); return false; })
      .finally(() => setLoading(false));
  }, []);

  useEffect(() => { load(); }, [load]);

  const checkAgain = async () => {
    if (!(await toastStore.defer('refresh', 'Stock will be checked again when this closes.', 'Check cancelled.'))) return;
    setLoading(true);
    if (await load(true)) toastStore.add('refresh', 'Stock checked again.');
  };

  // Export to Excel (legacy btnExport: both difference grids). One sheet: the "Diff. in Transaction Qty and System Stock Qty"
  // checks and the "Diff. in Stock Ledger and QOH" check side by side, one row per mismatching item.
  const exportDiffs = async () => {
    const rows = [];
    [['Web app', data?.web], ['Desktop database', data?.desktop]].forEach(([source, check]) => {
      (check?.tabs ?? []).forEach(t => t.rows.forEach(r => {
        const ledger = t.key === 'ledger';
        rows.push({ source, check: ledger ? 'Diff. in Stock Ledger and QOH' : `Diff. in Transaction Qty and System Stock Qty (${t.caption})`,
          item: r.item_name, code: r.item_code, a: ledger ? r.ledger_stock : r.trn_stock, b: ledger ? r.qoh : r.sys_stock, diff: r.diff });
      }));
    });
    if (!rows.length) return toastStore.add('error', 'Nothing to export: there are no differences.');
    if (!(await toastStore.defer('ok', `${rows.length} rows will be exported to Excel when this closes.`, 'Export cancelled.'))) return;
    const cols = [['source', 'Source'], ['check', 'Check'], ['item', 'Item Name'], ['code', 'Item Code'], ['a', 'Trn / Ledger Stock'], ['b', 'Sys Stock / QOH'], ['diff', 'Difference']]
      .map(([key, label]) => ({ key, label, align: ['a', 'b', 'diff'].includes(key) ? 'right' : 'left' }));
    await downloadXls('Re_Write_Stock_Differences', 'Re-Write Stock Balance', `${rows.length} rows`, cols, rows);
    toastStore.add('ok', `${rows.length} rows exported to Excel.`);
  };

  const reWrite = async () => {
    if (data && !data.web.rewrite_required) return toastStore.add('ok', 'Re-Write not Required');
    if (!(await confirm('Re-Write the stock now? Bills on every counter wait until it finishes (a few seconds). '
      + 'Movements are rebuilt from the saved documents and QOH from the movements.'))) return;
    if (!(await toastStore.defer('refresh', 'Stock will be re-written when this closes.', 'Re-Write cancelled. Nothing was changed.'))) return;
    setWriting(true);
    try {
      const res = await runRewriteStock();
      toastStore.add('ok', res.rewritten ? `${res.message} ${res.items_changed ?? 0} item(s) corrected.` : res.message);
      setData(d => (d ? { ...d, web: res.check } : d));
    } catch (e) {
      toastStore.add('error', errorText(e));
    } finally {
      setWriting(false);
    }
  };

  const notRequired = !!data && !data.web.rewrite_required;   // legacy greys the button as "Re-Write not Required"

  return (
    <div className="mx-auto flex max-w-6xl flex-col gap-4">
      {dialog}
      <div className={pageHeader}>
        <div>
          <h1 className={pageTitle}>Re-Write Stock Balance</h1>
          <p className={pageSub}>Compares every document with the stock movements and the stock on hand, and rebuilds them when they differ.</p>
        </div>
        <div className="flex gap-2">
          <button className={secondaryButton} onClick={checkAgain} disabled={loading || writing}><RefreshCw size={14} className={loading ? 'animate-spin' : ''} />Check again</button>
          <button className={secondaryButton} onClick={exportDiffs} disabled={!data || loading || writing}><FileSpreadsheet size={14} />Export to Excel</button>
          <button className={notRequired ? secondaryButton : dangerButton} onClick={reWrite} disabled={!data || loading || writing || notRequired}><Wrench size={14} />{writing ? 'Re-Writing…' : notRequired ? 'Re-Write not Required' : 'Re-Write'}</button>
        </div>
      </div>

      {!data && <div className={`${panel} p-6 text-sm text-stone-500`}>{loading ? 'Checking stock…' : 'Could not load the stock check.'}</div>}
      {data && <CheckPanel key={JSON.stringify(data.web.tabs.map(t => t.rows.length))} title="Web app stock" check={data.web}
        sub="Bills, purchases, returns and deductions made in the web app, against its stock movements and QOH." />}
      {data?.desktop && <CheckPanel title="Desktop database (vLQMS)" check={data.desktop}
        sub="The desktop app's own check, read-only. Differences here are fixed with Tools → Re Write Stock in the desktop app; the web app never changes the vLQMS database." />}
      {data && !data.db_connected && (
        <p className="text-[11px] text-stone-500">SQL Server is not connected, so the desktop database's own check isn't shown.</p>
      )}
    </div>
  );
}
