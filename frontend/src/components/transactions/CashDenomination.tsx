// Cash Denomination (legacy frmUtlDenom): count notes and coins in the drawer at day end and compare
// the total with the DSR cash balance. Saved per date via /transactions/denomination.

import { useEffect, useState } from 'react';
import { Save } from 'lucide-react';
import Toast from '../common/Toast';
import { toastStore } from '../common/toastStore';
import { api, errorText, qs, todayIso, type ApiDenomination, type ApiFullDsr } from '../../lib/api';
import { field, money, pageSub, pageTitle, panel, primaryButton, tableBody, tableHead, pageHeader } from '../../lib/styles';

const NOTES = [500, 200, 100, 50, 20, 10];

// Legacy frmUtlDenom: count the drawer at day end and compare with the DSR cash balance.
export default function CashDenomination() {
  const [on, setOn] = useState(todayIso());
  const [counts, setCounts] = useState<Record<string, string>>({});
  const [expected, setExpected] = useState<number | null>(null);
  const [toast, setToast] = useState<{ kind: 'ok' | 'error'; text: string } | null>(null);

  useEffect(() => {
    if (!toast) return;
    const timer = setTimeout(() => setToast(null), 2500);
    return () => clearTimeout(timer);
  }, [toast]);

  // Load the saved count (if any) and the expected cash for the chosen day.
  useEffect(() => {
    api<ApiDenomination | null>(`/transactions/denomination${qs({ on })}`)
      .then(d => setCounts(d ? Object.fromEntries(Object.entries(d.counts).map(([k, v]) => [k, String(v)])) : {}))
      .catch(() => setCounts({}));
    api<ApiFullDsr>(`/reports/dsr${qs({ on })}`).then(d => setExpected(typeof d?.cash_balance === "number" ? d.cash_balance : null)).catch(() => setExpected(null));
  }, [on]);

  const n = (key: string) => Math.max(0, Math.floor(Number(counts[key]) || 0));
  const total = NOTES.reduce((acc, v) => acc + v * n(String(v)), 0) + n('coins');
  const diff = expected === null ? 0 : total - expected;

  const save = async () => {
    if (!(await toastStore.defer('ok', `Cash count of ₹ ${money(total)} for ${on} will be saved when this closes.`, 'Save cancelled. Nothing was changed.'))) return;
    const body = { date: on, counts: Object.fromEntries([...NOTES.map(String), 'coins'].map(k => [k, n(k)])) };
    api<ApiDenomination>('/transactions/denomination', { method: 'POST', body })
      .then(d => setToast({ kind: 'ok', text: `Cash count for ${d.date} saved — ₹ ${money(d.total)}.` }))
      .catch(e => setToast({ kind: 'error', text: errorText(e) }));
  };

  return (
    <div className="mx-auto flex max-w-3xl flex-col gap-4">
      {toast && <Toast kind={toast.kind} text={toast.text} />}
      <div className={pageHeader}>
        <div>
          <h1 className={pageTitle}>Cash Denomination</h1>
          <p className={pageSub}>Count the cash drawer and compare it with the day's cash balance.</p>
        </div>
        <div className="flex items-end gap-2">
          <input type="date" value={on} onChange={e => setOn(e.target.value)} className={`${field} tabular-nums`} aria-label="Date" />
          <button onClick={save} className={primaryButton}><Save size={14} />Save</button>
        </div>
      </div>

      <div className={`${panel} overflow-hidden`}>
        <table className="w-full text-xs">
          <thead className={tableHead}>
            <tr><th className="px-4 py-2">Denomination</th><th className="px-4 py-2 text-right">Count</th><th className="px-4 py-2 text-right">Amount</th></tr>
          </thead>
          <tbody className={`${tableBody} tabular-nums`}>
            {NOTES.map(v => (
              <tr key={v}>
                <td className="px-4 py-1.5 font-bold text-stone-800 dark:text-stone-100">₹ {v}</td>
                <td className="px-4 py-1.5 text-right">
                  <input type="number" min={0} value={counts[v] ?? ''} placeholder="0" onChange={e => setCounts({ ...counts, [v]: e.target.value })} className={`${field} w-24 py-0.5 text-right`} aria-label={`Count of ₹${v}`} />
                </td>
                <td className="px-4 py-1.5 text-right">{money(v * n(String(v)))}</td>
              </tr>
            ))}
            <tr>
              <td className="px-4 py-1.5 font-bold text-stone-800 dark:text-stone-100">Coins (₹ total)</td>
              <td className="px-4 py-1.5 text-right">
                <input type="number" min={0} value={counts.coins ?? ''} placeholder="0" onChange={e => setCounts({ ...counts, coins: e.target.value })} className={`${field} w-24 py-0.5 text-right`} aria-label="Coins total" />
              </td>
              <td className="px-4 py-1.5 text-right">{money(n('coins'))}</td>
            </tr>
          </tbody>
        </table>
      </div>

      <div className="grid grid-cols-1 gap-3 sm:grid-cols-3">
        <div className={`${panel} px-4 py-3`}>
          <div className="text-xs font-medium text-stone-500 dark:text-stone-400">Counted</div>
          <div className="mt-1 text-xl font-semibold tracking-tight tabular-nums text-stone-900 dark:text-stone-50">₹ {money(total)}</div>
        </div>
        <div className={`${panel} px-4 py-3`}>
          <div className="text-xs font-medium text-stone-500 dark:text-stone-400">Expected (DSR cash balance)</div>
          <div className="mt-1 text-xl font-semibold tracking-tight tabular-nums text-stone-900 dark:text-stone-50">{expected === null ? '—' : `₹ ${money(expected)}`}</div>
        </div>
        <div role="status" className={`rounded-xl border px-4 py-3 ${diff < 0 ? 'border-red-500/30 bg-red-500/8' : diff > 0 ? 'border-amber-500/30 bg-amber-500/10' : 'border-emerald-500/30 bg-emerald-500/8'}`}>
          <div className="text-xs font-medium text-stone-500 dark:text-stone-400">{diff < 0 ? 'Shortage' : diff > 0 ? 'Excess' : 'Balanced'}</div>
          <div className={`mt-1 text-xl font-semibold tracking-tight tabular-nums ${diff < 0 ? 'text-red-700 dark:text-red-300' : diff > 0 ? 'text-amber-700 dark:text-amber-300' : 'text-emerald-700 dark:text-emerald-300'}`}>₹ {money(Math.abs(diff))}</div>
        </div>
      </div>
    </div>
  );
}
