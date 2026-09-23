import { useEffect, useState } from 'react';
import { FileDown, Play, Printer } from 'lucide-react';
import { api, errorText, qs, todayIso, type ApiItem } from '../api';
import { downloadText, toCsv } from '../csv';
import { field, label, money, pageSub, pageTitle, panel, primaryButton, secondaryButton, tableBody, tableHead } from '../ui';

export type ReportColumn = { key: string; label: string; kind?: 'text' | 'number' | 'money'; total?: boolean };
export type ReportFilter = 'on' | 'range' | 'item' | 'months';
type Row = Record<string, string | number>;

const monthStart = () => `${todayIso().slice(0, 8)}01`;

// Shared frame for the API reports: filter row → Run → table with totals, Print and Export CSV.
export default function ReportShell({ title, subtitle, endpoint, filters, columns }: {
  title: string; subtitle: string; endpoint: string; filters: ReportFilter[]; columns: ReportColumn[];
}) {
  const [on, setOn] = useState(todayIso());
  const [from, setFrom] = useState(monthStart());
  const [to, setTo] = useState(todayIso());
  const [item, setItem] = useState('');
  const [months, setMonths] = useState('6');
  const [items, setItems] = useState<ApiItem[]>([]);
  const [rows, setRows] = useState<Row[] | null>(null);
  const [error, setError] = useState('');

  useEffect(() => {
    if (filters.includes('item')) {
      api<ApiItem[]>('/masters/items').then(list => { setItems(list); setItem(i => i || list[0]?.code || ''); }).catch(() => undefined);
    }
  }, [filters]);

  const run = () => {
    const params: Record<string, string | undefined> = {};
    if (filters.includes('on')) params[endpoint.includes('ageing') ? 'as_on' : 'on'] = on;
    if (filters.includes('range')) { params.from_date = from; params.to_date = to; }
    if (filters.includes('item')) params.item_code = item;
    if (filters.includes('months')) params.months = months;
    api<Row[]>(`${endpoint}${qs(params)}`)
      .then(r => { setRows(r); setError(''); })
      .catch(e => { setRows(null); setError(errorText(e)); });
  };

  const format = (c: ReportColumn, v: string | number | undefined) =>
    v === undefined || v === '' ? '—' : c.kind === 'money' ? money(Number(v)) : String(v);

  const totals = columns.map(c => (c.total && rows ? rows.reduce((acc, r) => acc + (Number(r[c.key]) || 0), 0) : null));

  const exportCsv = () => {
    if (!rows) return;
    downloadText(`${title.replace(/\s+/g, '_')}.csv`, toCsv([columns.map(c => c.label), ...rows.map(r => columns.map(c => r[c.key] ?? ''))]));
  };

  return (
    <div className="mx-auto flex max-w-7xl flex-col gap-4">
      <div className={`${panel} flex flex-wrap items-end justify-between gap-3 px-5 py-3`}>
        <div>
          <h2 className={pageTitle}>{title}</h2>
          <p className={pageSub}>{subtitle}</p>
        </div>
        <div className="flex flex-wrap items-end gap-2 print:hidden">
          {filters.includes('item') && (
            <div className="w-full min-w-0 sm:w-auto">
              <label htmlFor="rp-item" className={label}>Item</label>
              <select id="rp-item" value={item} onChange={e => setItem(e.target.value)} className={`${field} w-full sm:w-72`}>
                {items.map(i => <option key={i.code} value={i.code}>{i.code} • {i.name}</option>)}
              </select>
            </div>
          )}
          {filters.includes('on') && (
            <div><label htmlFor="rp-on" className={label}>{endpoint.includes('ageing') ? 'As-on Date' : 'Date'}</label><input id="rp-on" type="date" value={on} onChange={e => setOn(e.target.value)} className={`${field} font-mono`} /></div>
          )}
          {filters.includes('range') && (<>
            <div><label htmlFor="rp-from" className={label}>From</label><input id="rp-from" type="date" value={from} onChange={e => setFrom(e.target.value)} className={`${field} font-mono`} /></div>
            <div><label htmlFor="rp-to" className={label}>To</label><input id="rp-to" type="date" value={to} onChange={e => setTo(e.target.value)} className={`${field} font-mono`} /></div>
          </>)}
          {filters.includes('months') && (
            <div><label htmlFor="rp-months" className={label}>Months</label><input id="rp-months" type="number" min={1} max={24} value={months} onChange={e => setMonths(e.target.value)} className={`${field} w-20 text-right`} /></div>
          )}
          <button onClick={run} className={primaryButton}><Play size={14} />Run</button>
          <button onClick={() => window.print()} disabled={!rows} className={secondaryButton}><Printer size={14} />Print</button>
          <button onClick={exportCsv} disabled={!rows} className={secondaryButton}><FileDown size={14} />Export CSV</button>
        </div>
      </div>

      <div className={`${panel} max-h-[600px] overflow-auto`}>
        <table className="w-full text-xs">
          <thead className={tableHead}>
            <tr>{columns.map(c => <th key={c.key} className={`px-3 py-2 ${c.kind && c.kind !== 'text' ? 'text-right' : ''}`}>{c.label}</th>)}</tr>
          </thead>
          <tbody className={tableBody}>
            {error && <tr><td colSpan={columns.length} className="px-3 py-6 text-center text-red-600 dark:text-red-400">{error}</td></tr>}
            {!rows && !error && <tr><td colSpan={columns.length} className="px-3 py-10 text-center text-stone-400">Set the filters and press Run.</td></tr>}
            {rows && !rows.length && <tr><td colSpan={columns.length} className="px-3 py-10 text-center text-stone-400">No data for these filters.</td></tr>}
            {rows?.map((r, i) => (
              <tr key={i} className="hover:bg-stone-50 dark:hover:bg-[#1b2033]">
                {columns.map(c => (
                  <td key={c.key} className={`px-3 py-1.5 ${c.kind && c.kind !== 'text' ? 'text-right font-mono' : 'text-stone-800 dark:text-stone-200'}`}>{format(c, r[c.key])}</td>
                ))}
              </tr>
            ))}
          </tbody>
          {rows && rows.length > 0 && totals.some(t => t !== null) && (
            <tfoot className="bg-[#e8702a]/10 font-bold text-[#b84716] dark:text-[#ffb07e]">
              <tr>
                {columns.map((c, i) => (
                  <td key={c.key} className={`px-3 py-2 ${c.kind && c.kind !== 'text' ? 'text-right font-mono' : ''}`}>
                    {i === 0 ? 'Total' : totals[i] === null ? '' : c.kind === 'money' ? money(totals[i]!) : Number(totals[i]!.toFixed(3))}
                  </td>
                ))}
              </tr>
            </tfoot>
          )}
        </table>
      </div>
    </div>
  );
}
