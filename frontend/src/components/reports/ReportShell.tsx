import { useEffect, useState } from 'react';
import { FileDown, Play, Printer } from 'lucide-react';
import { api, errorText, qs, todayIso, type ApiItem } from '../api';
import { downloadText, toCsv } from '../csv';
import { badge, field, label, money, pageHeader, pageSub, pageTitle, panel, primaryButton, tableBody, tableHead, tableRow, exportButton, printButton } from '../ui';
import PrintFormatSelectorModal from '../print/PrintFormatSelectorModal';

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
  const [showPrintModal, setShowPrintModal] = useState(false);

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

  const num = (c: ReportColumn) => c.kind && c.kind !== 'text';

  return (
    <div className="mx-auto max-w-7xl">
      <div className={`${pageHeader} mb-5`}>
        <div>
          <h1 className={pageTitle}>{title}</h1>
          <p className={pageSub}>{subtitle}</p>
        </div>
        <div className="flex gap-2 print:hidden">
          <button onClick={() => setShowPrintModal(true)} className={printButton}><Printer size={14} />Print</button>
          <button onClick={exportCsv} disabled={!rows} className={exportButton}><FileDown size={14} />Export CSV</button>
        </div>
      </div>

      <form
        onSubmit={e => { e.preventDefault(); run(); }}
        aria-label="Report filters"
        className={`${panel} mb-4 flex flex-wrap items-end gap-3 p-4 print:hidden`}
      >
        {filters.includes('item') && (
          <div className="w-full min-w-0 sm:w-72">
            <label htmlFor="rp-item" className={label}>Item</label>
            <select id="rp-item" value={item} onChange={e => setItem(e.target.value)} className={field}>
              {items.map(i => <option key={i.code} value={i.code}>{i.code} · {i.name}</option>)}
            </select>
          </div>
        )}
        {filters.includes('on') && (
          <div className="w-40"><label htmlFor="rp-on" className={label}>{endpoint.includes('ageing') ? 'As-on date' : 'Date'}</label><input id="rp-on" type="date" value={on} onChange={e => setOn(e.target.value)} className={`${field} tabular-nums`} /></div>
        )}
        {filters.includes('range') && (<>
          <div className="w-40"><label htmlFor="rp-from" className={label}>From</label><input id="rp-from" type="date" value={from} onChange={e => setFrom(e.target.value)} className={`${field} tabular-nums`} /></div>
          <div className="w-40"><label htmlFor="rp-to" className={label}>To</label><input id="rp-to" type="date" value={to} onChange={e => setTo(e.target.value)} className={`${field} tabular-nums`} /></div>
        </>)}
        {filters.includes('months') && (
          <div className="w-24"><label htmlFor="rp-months" className={label}>Months</label><input id="rp-months" type="number" min={1} max={24} value={months} onChange={e => setMonths(e.target.value)} className={`${field} text-right tabular-nums`} /></div>
        )}
        <button type="submit" className={primaryButton}><Play size={14} />Run report</button>
        {rows && <span className={`${badge('neutral')} ml-auto`}>{rows.length} rows</span>}
      </form>

      <div className={`${panel} max-h-[640px] overflow-auto`}>
        <table className="w-full text-xs">
          <thead className={tableHead}>
            <tr>{columns.map(c => <th key={c.key} className={`px-4 py-2.5 ${num(c) ? 'text-right' : ''}`}>{c.label}</th>)}</tr>
          </thead>
          <tbody className={tableBody}>
            {error && <tr><td colSpan={columns.length} className="px-4 py-10 text-center text-red-600 dark:text-red-400">{error}</td></tr>}
            {!rows && !error && <tr><td colSpan={columns.length} className="px-4 py-14 text-center text-stone-500">Choose the filters above and run the report.</td></tr>}
            {rows && !rows.length && <tr><td colSpan={columns.length} className="px-4 py-14 text-center text-stone-500">No data for these filters.</td></tr>}
            {rows?.map((r, i) => (
              <tr key={i} className={tableRow}>
                {columns.map((c, ci) => (
                  <td key={c.key} className={`px-4 py-2 ${num(c) ? 'text-right tabular-nums text-stone-700 dark:text-stone-200' : ci === 0 ? 'font-medium text-stone-900 dark:text-stone-50' : 'text-stone-600 dark:text-stone-300'}`}>{format(c, r[c.key])}</td>
                ))}
              </tr>
            ))}
          </tbody>
          {rows && rows.length > 0 && totals.some(t => t !== null) && (
            <tfoot className="sticky bottom-0 border-t border-line-strong bg-subtle font-semibold text-stone-900 dark:text-stone-50">
              <tr>
                {columns.map((c, i) => (
                  <td key={c.key} className={`px-4 py-2.5 ${num(c) ? 'text-right tabular-nums' : ''}`}>
                    {i === 0 ? 'Total' : totals[i] === null ? '' : c.kind === 'money' ? money(totals[i]!) : Number(totals[i]!.toFixed(3))}
                  </td>
                ))}
              </tr>
            </tfoot>
          )}
        </table>
      </div>

      {showPrintModal && (
        <PrintFormatSelectorModal
          category="GENERIC_REPORT"
          data={{ title, subtitle, columns, rows: rows || [] }}
          title={`Print Report: ${title}`}
          onClose={() => setShowPrintModal(false)}
        />
      )}
    </div>
  );
}
