// Central site → Purchase indents: the indents each shop raised on a day, with their lines.

import { useCallback, useEffect, useState } from 'react';
import { Search } from 'lucide-react';
import { toastStore } from '../components/common/toastStore';
import { InlineError } from '../components/common/FormErrorSummary';
import { centralIndents, errorText, todayIso } from '../lib/api';
import { field, fieldError, label, money, pageHeader, pageSub, pageTitle, panel, primaryButton, tableBody, tableHead, tableRow } from '../lib/styles';


export default function Indents({ shops }) {
  const [client, setClient] = useState('');
  const [day, setDay] = useState(todayIso);
  const [dayError, setDayError] = useState('');
  const [rows, setRows] = useState(null);
  const [loading, setLoading] = useState(true);

  const load = useCallback((c, d) =>
    centralIndents(new URLSearchParams({ day: d, ...(c ? { client: c } : {}) }))
      .then(r => { setRows(r); return true; })
      .catch(e => { toastStore.add('error', errorText(e)); return false; })
      .finally(() => setLoading(false)), []);

  useEffect(() => { load('', todayIso()); }, [load]);

  const show = async () => {
    if (!day) {
      setDayError('Pick a day');
      document.getElementById('ci-day')?.focus();
      return;
    }
    setDayError('');
    if (!(await toastStore.defer('refresh', 'Indents will be loaded when this closes.', 'Cancelled.'))) return;
    setLoading(true);
    if (await load(client, day)) toastStore.add('refresh', 'Indents loaded.');
  };

  return (
    <div className="flex flex-col gap-4">
      <div className={pageHeader}>
        <div>
          <h1 className={pageTitle}>Purchase indents</h1>
          <p className={pageSub}>Stock each shop asked for on the day.</p>
        </div>
      </div>
      <section className={`${panel} flex flex-wrap items-end gap-3 p-4`}>
        <div className="min-w-[200px]">
          <label htmlFor="ci-shop" className={label}>Shop</label>
          <select id="ci-shop" value={client} onChange={e => setClient(e.target.value)} className={field}>
            <option value="">All shops</option>
            {shops.map(s => <option key={s.code} value={s.code}>{s.code} · {s.name}</option>)}
          </select>
        </div>
        <div>
          <label htmlFor="ci-day" className={label}>Day</label>
          <input id="ci-day" type="date" value={day} onChange={e => { setDay(e.target.value); setDayError(''); }}
            className={`${field} ${dayError ? fieldError : ''}`} aria-invalid={!!dayError} />
          <InlineError message={dayError} />
        </div>
        <button className={primaryButton} onClick={show} disabled={loading}><Search size={14} />Show</button>
      </section>

      {rows && rows.length === 0 && <section className={`${panel} p-6 text-center text-sm text-stone-500`}>No indents for this day.</section>}
      {rows?.map(ind => (
        <section key={`${ind.client}-${ind.PINO}`} className={`${panel} overflow-hidden`}>
          <div className="flex flex-wrap justify-between gap-2 border-b border-[#E3D9CD] px-3 py-2 text-xs dark:border-line">
            <span className="font-semibold">{ind.client} · {ind.PINO}</span>
            <span>{ind.SUPPName} · ₹ {money(ind.GRNDTOT)}</span>
          </div>
          <table className="min-w-full text-xs">
            <thead><tr>{['#', 'Item', 'Qty', 'Rate', 'Amount'].map(h => <th key={h} className={`${tableHead} px-3 py-1.5`}>{h}</th>)}</tr></thead>
            <tbody className={tableBody}>
              {ind.lines.map(l => (
                <tr key={l.SlNo} className={tableRow}>
                  <td className="px-3 py-1.5">{l.SlNo}</td>
                  <td className="px-3 py-1.5">{l.ITEMName} <span className="text-stone-500">{l.ItemCode}</span></td>
                  <td className="px-3 py-1.5 tabular-nums">{l.PIQty}</td>
                  <td className="px-3 py-1.5 tabular-nums">{money(l.PIRate)}</td>
                  <td className="px-3 py-1.5 tabular-nums">{money(l.PIAmt)}</td>
                </tr>
              ))}
            </tbody>
          </table>
        </section>
      ))}
    </div>
  );
}
