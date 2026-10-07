// Utilities → ADMIN Audit Log (legacy frmRptAdminAuditLog, tables UtlAdmAudit + UtlAdmAuditItem).
//
// Pick a Screen (or all) and a From / To date, then View. The grid has the desktop columns: Screen, Doc No,
// Date Changed, Time Changed, Action, User, Earlier Data, Changed Data, and one line per item for documents
// (Item Code, Item Name, Item Status, Item Details). Tools runs (SQL Query Execution, Re Write Stock) are
// included. Export to Excel. Administrator only (backend: /utilities/audit). Viewing only: nothing changes.

import { useCallback, useEffect, useState } from 'react';
import { FileSpreadsheet, Search } from 'lucide-react';
import { toastStore } from '../common/toastStore';
import { InlineError } from '../common/FormErrorSummary';
import { errorText, getAuditLog, getAuditScreens, } from '../../lib/api';
import { downloadXls } from '../../lib/xlsExport';
import { exportButton, field, fieldError, label, pageHeader, pageSub, pageTitle, panel, primaryButton, tableBody, tableHead } from '../../lib/styles';

const iso = (d) => d.toISOString().slice(0, 10);
const weekAgo = () => { const d = new Date(); d.setDate(d.getDate() - 7); return iso(d); };

export default function AdminAuditLog() {
  const [screens, setScreens] = useState([]);
  const [screen, setScreen] = useState('');
  const [from, setFrom] = useState(weekAgo);
  const [to, setTo] = useState(() => iso(new Date()));
  const [rows, setRows] = useState([]);
  const [loading, setLoading] = useState(true);
  const [dateError, setDateError] = useState('');

  const load = useCallback((scr, f, t) =>
    getAuditLog({ from_date: f, to_date: t, ...(scr ? { screen: scr } : {}) })
      .then(r => { setRows(r); return true; })
      .catch(e => { toastStore.add('error', errorText(e)); return false; })
      .finally(() => setLoading(false)), []);

  useEffect(() => {
    getAuditScreens().then(setScreens).catch(() => undefined);
    load('', weekAgo(), iso(new Date()));
  }, [load]);

  const view = async () => {
    if (!from || !to || to < from) {
      setDateError(!from || !to ? 'Pick both dates' : 'To date is before From date');
      document.getElementById('aal-to')?.focus();
      return;
    }
    setDateError('');
    if (!(await toastStore.defer('refresh', 'The audit log will be loaded when this closes.', 'Cancelled.'))) return;
    setLoading(true);
    if (await load(screen, from, to)) toastStore.add('refresh', 'Audit log loaded.');
  };

  const exportXls = async () => {
    if (!rows.length) return toastStore.add('error', 'Nothing to export: load some entries first.');
    if (!(await toastStore.defer('ok', 'The audit log will be exported when this closes.', 'Export cancelled.'))) return;
    const flat = rows.flatMap(r => (r.items.length ? r.items : [null]).map(i => ({
      screen: r.screen, doc: r.main_fld, date: r.at.slice(0, 10), time: r.at.slice(11), action: r.action, user: r.user_code ?? '',
      earlier: r.earlier, changed: r.changed, code: i?.code ?? '', name: i?.name ?? '', status: i?.status ?? '', details: i?.details ?? '',
    })));
    downloadXls(`AdminAuditLog_${from}_${to}`, 'Admin Audit Log', `${screen || 'All screens'}, ${from} to ${to}`,
      [['screen', 'Screen'], ['doc', 'Doc No'], ['date', 'Date Changed'], ['time', 'Time Changed'], ['action', 'Action'], ['user', 'User'],
        ['earlier', 'Earlier Data'], ['changed', 'Changed Data'], ['code', 'Item Code'], ['name', 'Item Name'], ['status', 'Item Status'],
        ['details', 'Item Details']].map(([key, lbl]) => ({ key, label: lbl, align: 'left'  })), flat);
    toastStore.add('ok', 'Audit log exported.');
  };

  return (
    <div className="mx-auto flex max-w-7xl flex-col gap-4">
      <div className={pageHeader}>
        <div>
          <h1 className={pageTitle}>Admin Audit Log</h1>
          <p className={pageSub}>Who added, changed, cancelled or removed what, and when. Nothing can be changed here.</p>
        </div>
        <button className={exportButton} onClick={exportXls}><FileSpreadsheet size={14} />Export to Excel</button>
      </div>

      <section className={`${panel} flex flex-wrap items-end gap-3 p-4`}>
        <div className="min-w-[200px]">
          <label htmlFor="aal-screen" className={label}>Screen</label>
          <select id="aal-screen" value={screen} onChange={e => setScreen(e.target.value)} className={field}>
            <option value="">All screens</option>
            {screens.map(s => <option key={s} value={s}>{s}</option>)}
          </select>
        </div>
        <div>
          <label htmlFor="aal-from" className={label}>From</label>
          <input id="aal-from" type="date" value={from} onChange={e => { setFrom(e.target.value); setDateError(''); }} className={field} />
        </div>
        <div>
          <label htmlFor="aal-to" className={label}>To</label>
          <input id="aal-to" type="date" value={to} onChange={e => { setTo(e.target.value); setDateError(''); }}
            className={`${field} ${dateError ? fieldError : ''}`} aria-invalid={!!dateError} />
          <InlineError message={dateError} />
        </div>
        <button className={primaryButton} onClick={view} disabled={loading}><Search size={14} />View</button>
      </section>

      <section className={`${panel} overflow-hidden`}>
        <div className="max-h-[65vh] overflow-auto">
          <table className="min-w-full text-xs">
            <thead>
              <tr>{['Screen', 'Doc No', 'Date Changed', 'Time Changed', 'Action', 'User', 'Earlier Data', 'Changed Data', 'Item Code', 'Item Name', 'Item Status', 'Item Details']
                .map(h => <th key={h} className={`${tableHead} px-3 py-2`}>{h}</th>)}</tr>
            </thead>
            <tbody className={tableBody}>
              {rows.flatMap(r => (r.items.length ? r.items : [null]).map((i, n) => (
                <tr key={`${r.id}-${n}`} className="align-top">
                  {n === 0 ? <>
                    <td className="px-3 py-1.5">{r.screen}</td>
                    <td className="px-3 py-1.5 font-mono">{r.main_fld}</td>
                    <td className="px-3 py-1.5 tabular-nums">{r.at.slice(0, 10).split('-').reverse().join('-')}</td>
                    <td className="px-3 py-1.5 tabular-nums">{r.at.slice(11)}</td>
                    <td className="px-3 py-1.5">{r.action}</td>
                    <td className="px-3 py-1.5">{r.user_code}</td>
                    <td className="max-w-xs whitespace-pre-wrap break-words px-3 py-1.5 text-stone-500">{r.earlier}</td>
                    <td className="max-w-xs whitespace-pre-wrap break-words px-3 py-1.5">{r.changed}</td>
                  </> : <td colSpan={8} />}
                  <td className="px-3 py-1.5 font-mono">{i?.code}</td>
                  <td className="px-3 py-1.5">{i?.name}</td>
                  <td className="px-3 py-1.5">{i?.status}</td>
                  <td className="px-3 py-1.5 tabular-nums">{i?.details}</td>
                </tr>
              )))}
              {!rows.length && <tr><td colSpan={12} className="px-3 py-6 text-center text-stone-500">{loading ? 'Loading…' : 'No changes in this period.'}</td></tr>}
            </tbody>
          </table>
        </div>
        {rows.length > 0 && <p className="border-t border-[#E3D9CD] px-4 py-2 text-[11px] text-stone-500 dark:border-line">{rows.length} entr{rows.length === 1 ? 'y' : 'ies'}</p>}
      </section>
    </div>
  );
}
