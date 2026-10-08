// Dialogs used by the Purchase Bill: ItemLookupModal (pick an item), CopyFromModal (copy lines from a
// Purchase Indent) and AddSupplierModal.

import { useMemo, useState } from 'react';
import Modal from '../../common/Modal';
import ItemPickGrid from '../ItemPickGrid';

import { field, headCell, money } from './purchaseBillData';

const IDS = ['ai-cases', 'ai-loose', 'ai-rate', 'ai-selling', 'ai-aroed'];
const box = 'rounded-md border border-line bg-surface/60 p-3';
const rowLabel = 'w-28 shrink-0 text-xs font-medium text-stone-700 dark:text-stone-200';
const focusId = (id) => setTimeout(() => { const el = document.getElementById(id); el?.focus(); el?.select?.(); }, 0);

// "Add Item Into Purchase Bill" (legacy frmTrnPBAddItem): search the liquor items, pick one, then enter the cases and loose
// bottles; Total Bottles and Amount follow. F12 or OK adds the row, Esc closes. `items` are the liquor items (with their
// STOCK_ITEMS index as `stock`); `minMarginPct` is Global Settings PurSellVarPer (the selling price must be that % above cost).
export function ItemLookupModal({ items, minMarginPct = 0, onAdd, onClose }) {
  const [text, setText] = useState('');
  const [at, setAt] = useState(0);
  const [picked, setPicked] = useState(null);
  const [cases, setCases] = useState('');
  const [loose, setLoose] = useState('');
  const [rate, setRate] = useState('');
  const [selling, setSelling] = useState('');
  const [aroed, setAroed] = useState('');
  const [error, setError] = useState('');

  const matches = useMemo(() => {
    const words = text.trim().toLowerCase().split(/\s+/).filter(Boolean);
    return items.filter(i => {
      const hay = `${i.code} ${i.shortName} ${i.hotKey} ${i.brand} ${i.name} ${i.pkg}`.toLowerCase();
      return words.every(w => hay.includes(w));
    }).slice(0, 300);
  }, [items, text]);
  const current = Math.min(at, matches.length - 1);

  const perCase = picked?.bottlesPerCase || 1;
  const total = (Number(cases) || 0) * perCase + (Number(loose) || 0);
  const amount = Math.round(total * (Number(rate) || 0) * 100) / 100;
  const variation = Number(rate) > 0 ? ((Number(selling) || 0) - Number(rate)) / Number(rate) * 100 : 0;
  const lowMargin = minMarginPct > 0 && variation < minMarginPct;

  const choose = (item) => {
    if (!item) return;
    setPicked(item);
    setRate(item.purchaseRate ? String(item.purchaseRate) : '');
    setSelling(item.rate ? String(item.rate) : '');
    setCases(''); setLoose(''); setAroed(''); setError('');
    focusId('ai-cases');
  };
  const clear = () => { setText(''); setPicked(null); setAt(0); setError(''); focusId('ai-search'); };
  const add = () => {
    if (!picked) return choose(matches[current]);
    if (total <= 0) { setError('Enter the cases or loose bottles.'); focusId('ai-cases'); return; }
    if (!(Number(rate) > 0)) { setError('Enter the purchase rate.'); focusId('ai-rate'); return; }
    onAdd({ stock: picked.stock, qty: total, rate: Number(rate) });
  };

  // F12 adds from any field; Enter in the fields moves on (it must not reach the screen's own Enter-to-next-field behind).
  const dialogKeys = (e) => {
    if (e.key === 'F12') { e.preventDefault(); e.stopPropagation(); add(); }
    else if (e.key === 'Enter') e.stopPropagation();
  };
  const fieldKeys = (i) => (e) => {
    if (e.key !== 'Enter') return;
    e.preventDefault();
    if (i < IDS.length - 1) focusId(IDS[i + 1]); else add();
  };
  const numberInput = (i, id, value, set, extra = '') => (
    <input id={id} type="number" min={0} step="any" value={value} onChange={e => { set(e.target.value); setError(''); }}
      onFocus={e => e.target.select()} onKeyDown={fieldKeys(i)} className={`${field} w-28 py-1 text-right tabular-nums ${extra}`} />
  );

  return (
    <Modal title="Add Item Into Purchase Bill" onClose={onClose} wide
      footer={<>
        <span className="mr-auto hidden text-[11px] text-red-600 sm:inline dark:text-red-400">
          {picked ? 'Press F12 to Save / Click on OK' : 'Type in Search area [Barcode / ShortName / Name / HotKey]'} · Press Escape / click Cancel to close the screen
        </span>
        <button onClick={add} className="rounded-lg border border-line bg-surface px-6 py-2 text-xs font-bold text-stone-900 hover:bg-subtle dark:text-stone-50">OK</button>
        <button onClick={clear} className="rounded-lg border border-line bg-surface px-6 py-2 text-xs font-bold text-stone-900 hover:bg-subtle dark:text-stone-50">Clear</button>
        <button onClick={onClose} className="rounded-lg border border-line bg-surface px-6 py-2 text-xs font-bold text-stone-900 hover:bg-subtle dark:text-stone-50">Cancel</button>
      </>}>
      <div onKeyDown={dialogKeys} className="space-y-3">
        <div className="flex items-center gap-3">
          <label htmlFor="ai-search" className="w-16 shrink-0 text-sm font-medium">Search</label>
          <input id="ai-search" data-autofocus autoFocus value={text} autoComplete="off" aria-label="Search items"
            onChange={e => { setText(e.target.value); setAt(0); if (picked) setPicked(null); }}
            onKeyDown={e => {
              if (e.key === 'ArrowDown' || e.key === 'ArrowUp') { e.preventDefault(); setAt(Math.max(0, Math.min(matches.length - 1, current + (e.key === 'ArrowDown' ? 1 : -1)))); }
              else if (e.key === 'Enter') { e.preventDefault(); if (!picked) choose(matches[current]); else focusId('ai-cases'); }
            }}
            className={`${field} flex-1`} />
        </div>

        {!picked && <ItemPickGrid id="ai-items" items={matches} at={current} onPick={choose} className="h-72" empty="No liquor item matches." />}

        {picked && (
          <>
            <div className="flex gap-3">
              <span className="w-16 shrink-0 text-sm font-medium">Item Info</span>
              <div className="min-h-20 flex-1 rounded-md border border-line bg-surface p-2.5 text-sm">
                <div>{picked.name}</div>
                <div>{picked.shortName || picked.name} {picked.pkg}</div>
                <div className="mt-2">{perCase} Bottles per case</div>
              </div>
            </div>
            <div className="grid gap-3 md:grid-cols-[minmax(0,1fr)_minmax(0,1fr)_minmax(0,1.1fr)]">
              <div className={`${box} space-y-2`}>
                <div className="flex items-center"><label htmlFor="ai-cases" className={rowLabel}>Cases</label>{numberInput(0, 'ai-cases', cases, setCases)}</div>
                <div className="flex items-center"><label htmlFor="ai-loose" className={rowLabel}>Loose Bottles</label>{numberInput(1, 'ai-loose', loose, setLoose)}</div>
                <div className="flex items-center"><span className={rowLabel}>Total Bottles</span><output aria-label="Total bottles" className={`${field} w-28 py-1 text-right tabular-nums`}>{total}</output></div>
              </div>
              <div className={`${box} space-y-2`}>
                <div className="flex items-center"><label htmlFor="ai-rate" className={rowLabel}>Purchase Rate</label>{numberInput(2, 'ai-rate', rate, setRate)}</div>
                <div className="flex items-center"><label htmlFor="ai-selling" className={rowLabel}>Selling Price</label>{numberInput(3, 'ai-selling', selling, setSelling)}</div>
                <div className="flex items-center"><span className={rowLabel}>Amount</span><output aria-label="Amount" className={`${field} w-28 py-1 text-right tabular-nums`}>{amount}</output></div>
                <div className="flex items-center"><label htmlFor="ai-aroed" className={rowLabel}>AROED Per Bot</label>{numberInput(4, 'ai-aroed', aroed, setAroed)}</div>
              </div>
              <div className={`${box} space-y-1.5 text-sm`}>
                <div className={`text-lg font-semibold ${lowMargin ? 'text-red-600 dark:text-red-400' : 'text-stone-800 dark:text-stone-100'}`}>Price variation : {Math.round(variation * 100) / 100}%</div>
                <div>Present Stock : {picked.clQty}</div>
                <div>Order Level : {money(picked.rol ?? 0)}</div>
                <div>Reorder Qty : {money(picked.roq ?? 0)}</div>
                {lowMargin && <p className="text-xs text-red-600 dark:text-red-400">Selling price should be at least {minMarginPct}% above the purchase rate (Global Settings).</p>}
              </div>
            </div>
            {error && <p role="alert" className="text-xs text-red-600 dark:text-red-400">{error}</p>}
          </>
        )}
      </div>
    </Modal>
  );
}

export function CopyFromModal({ indents, onCopy, onClose }

) {
  return (
    <Modal title="Copy From Purchase Indent" onClose={onClose} wide>
      <table className="w-full text-xs">
        <thead className="bg-subtle text-stone-600 dark:text-stone-300">
          <tr><th className={headCell}>Indent No</th><th className={headCell}>Date</th><th className={headCell}>Supplier</th><th className={`${headCell} text-right`}>Items</th><th className={`${headCell} text-right`}>Qty</th><th className={headCell} /></tr>
        </thead>
        <tbody className="divide-y divide-line-soft">
          {indents?.map(ind => (
            <tr key={ind.doc_no}>
              <td className="px-2 py-2 font-mono">{ind.doc_no}</td>
              <td className="px-2 py-2 font-mono">{ind.date}</td>
              <td className="px-2 py-2">{ind.supplier}</td>
              <td className="px-2 py-2 text-right tabular-nums">{ind.lines.length}</td>
              <td className="px-2 py-2 text-right tabular-nums">{ind.totals.qty}</td>
              <td className="px-2 py-2 text-right"><button onClick={() => onCopy(ind)} className="rounded-lg bg-accent px-3 py-1 font-bold text-white hover:bg-accent-strong">Copy</button></td>
            </tr>
          ))}
          {indents && !indents.length && <tr><td colSpan={6} className="px-2 py-8 text-center text-stone-400">No purchase indents yet. Raise one in Purchase Indent first.</td></tr>}
          {!indents && <tr><td colSpan={6} className="px-2 py-8 text-center text-stone-400">Loading…</td></tr>}
        </tbody>
      </table>
    </Modal>
  );
}

export function AddSupplierModal({ saving, onSave, onClose }

) {
  const [name, setName] = useState('');
  const [address, setAddress] = useState('');
  const ready = name.trim() !== '' && !saving;
  const submit = () => { if (ready) onSave(name.trim().toUpperCase(), address.trim()); };
  return (
    <Modal
      title="Add New Supplier"
      onClose={onClose}
      footer={<>
        <button onClick={onClose} className="rounded-lg border border-line px-4 py-2 text-xs font-semibold text-stone-600 hover:bg-subtle dark:text-stone-300">Cancel</button>
        <button onClick={submit} disabled={!ready} className="rounded-lg bg-accent px-4 py-2 text-xs font-bold text-white hover:bg-accent-strong disabled:cursor-not-allowed disabled:opacity-50">{saving ? 'Saving…' : 'Save Supplier'}</button>
      </>}
    >
      <label className="mb-1 block font-semibold text-accent-ink">Supplier Name *</label>
      <input autoFocus value={name} onChange={e => setName(e.target.value.toUpperCase())} onKeyDown={e => e.key === 'Enter' && submit()} placeholder="e.g. R&D DISTRIBUTORS" className={`${field} mb-3 w-full`} />
      <label className="mb-1 block font-semibold text-stone-600 dark:text-stone-300">Billing Address</label>
      <textarea value={address} onChange={e => setAddress(e.target.value)} rows={3} placeholder="Street, area, city - PIN" className={`${field} w-full resize-none`} />
    </Modal>
  );
}
