// Central site → Stock: an item's stock in every shop (or one shop's whole list), from the stock each shop
// sends every few minutes. Items below their minimum level are marked. Type at least two letters, or pick a shop.

import { useState } from 'react';
import { Search } from 'lucide-react';
import { toastStore } from '../components/common/toastStore';
import { InlineError } from '../components/common/FormErrorSummary';
import { centralStock, errorText } from '../lib/api';
import { badge, field, fieldError, label, money, pageHeader, pageSub, pageTitle, panel, primaryButton, tableBody, tableHead, tableRow } from '../lib/styles';
import { isStale, when, } from './types';

export default function StockLookup({ shops }) {
  const [q, setQ] = useState('');
  const [client, setClient] = useState('');
  const [qError, setQError] = useState('');
  const [data, setData] = useState(null);
  const [busy, setBusy] = useState(false);

  const search = async () => {
    if (!client && q.trim().length < 2) {
      setQError('Type at least two letters of the item, or pick a shop');
      document.getElementById('cs-q')?.focus();
      return;
    }
    setQError('');
    const params = new URLSearchParams({ q: q.trim(), ...(client ? { client } : {}) });
    if (!(await toastStore.defer('refresh', 'Stock will be loaded when this closes.', 'Cancelled.'))) return;
    setBusy(true);
    try {
      const res = await centralStock(params);
      setData(res);
      toastStore.add('refresh', `${res.items.length} item${res.items.length === 1 ? '' : 's'} found.`);
    } catch (e) {
      toastStore.add('error', errorText(e));
    } finally {
      setBusy(false);
    }
  };

  const cols = data?.shops ?? [];
  return (
    <div className="flex flex-col gap-4">
      <div className={pageHeader}>
        <div>
          <h1 className={pageTitle}>Stock</h1>
          <p className={pageSub}>Stock on hand in each shop, as last sent by that shop.</p>
        </div>
      </div>
      <section className={`${panel} flex flex-wrap items-end gap-3 p-4`}>
        <div className="min-w-[240px] flex-1">
          <label htmlFor="cs-q" className={label}>Item</label>
          <input id="cs-q" value={q} placeholder="Name or code" onKeyDown={e => e.key === 'Enter' && search()}
            onChange={e => { setQ(e.target.value); setQError(''); }} className={`${field} ${qError ? fieldError : ''}`} aria-invalid={!!qError} />
          <InlineError message={qError} />
        </div>
        <div className="min-w-[200px]">
          <label htmlFor="cs-shop" className={label}>Shop</label>
          <select id="cs-shop" value={client} onChange={e => { setClient(e.target.value); setQError(''); }} className={field}>
            <option value="">All shops</option>
            {shops.map(s => <option key={s.code} value={s.code}>{s.code} · {s.name}</option>)}
          </select>
        </div>
        <button className={primaryButton} onClick={search} disabled={busy}><Search size={14} />Search</button>
      </section>

      {data && (
        <section className={`${panel} overflow-hidden`}>
          <div className="max-h-[65vh] overflow-auto">
            <table className="min-w-full text-xs">
              <thead>
                <tr>
                  <th className={`${tableHead} px-3 py-1.5`}>Item</th>
                  <th className={`${tableHead} px-3 py-1.5 text-right`}>MRP</th>
                  {cols.map(s => (
                    <th key={s.client} className={`${tableHead} px-3 py-1.5 text-right`} title={`Stock sent ${when(s.last_stock_sync)}`}>
                      {s.client}{isStale(s.last_stock_sync) && ' ⚠'}
                    </th>
                  ))}
                  {cols.length > 1 && <th className={`${tableHead} px-3 py-1.5 text-right`}>Total</th>}
                </tr>
              </thead>
              <tbody className={tableBody}>
                {data.items.length === 0 && <tr><td colSpan={cols.length + 3} className="px-3 py-3 text-stone-500">No items found.</td></tr>}
                {data.items.map(it => (
                  <tr key={it.item_code} className={tableRow}>
                    <td className="px-3 py-1.5"><span className="font-medium">{it.item_name}</span> <span className="text-stone-500">{it.item_code} · {it.package}</span></td>
                    <td className="px-3 py-1.5 text-right tabular-nums">{money(it.mrp)}</td>
                    {cols.map(s => {
                      const at = it.shops.find(x => x.client === s.client);
                      return (
                        <td key={s.client} className="px-3 py-1.5 text-right tabular-nums">
                          {at ? <>{at.qoh.toLocaleString()}{at.short && <span className={`${badge('danger')} ml-1`}>low</span>}</> : '–'}
                        </td>
                      );
                    })}
                    {cols.length > 1 && <td className="px-3 py-1.5 text-right font-semibold tabular-nums">{it.total_qoh.toLocaleString()}</td>}
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        </section>
      )}
    </div>
  );
}
