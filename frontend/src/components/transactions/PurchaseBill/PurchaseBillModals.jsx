// Dialogs used by the Purchase Bill: ItemLookupModal (pick an item), CopyFromModal (copy lines from a
// Purchase Indent) and AddSupplierModal.

import { useEffect, useMemo, useState } from 'react';
import { Search, X } from 'lucide-react';
import { lineName } from '../../../data/stockData';
import Modal from '../../common/Modal';
import ItemPickGrid from '../ItemPickGrid';

import { field, headCell, money } from './purchaseBillData';
import { SkeletonTableRows } from '../../common/Skeleton';

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

  // Typing the Total Bottles works out the cases (by the item's bottles per case) and the loose bottles.
  const setTotalBottles = (v) => {
    const t = Math.max(0, Math.floor(Number(v)) || 0);
    setCases(t ? String(Math.floor(t / perCase)) : ''); setLoose(t ? String(t % perCase) : ''); setError('');
  };

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
                <div className="flex items-center"><span className={rowLabel}>Total Bottles</span><input id="ai-total" type="number" min={0} step={1} aria-label="Total bottles" value={total || ''} placeholder="0"
                  onChange={e => setTotalBottles(e.target.value)} onFocus={e => e.target.select()} onKeyDown={fieldKeys(1)}
                  className={`${field} w-28 py-1 text-right tabular-nums`} /></div>
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

// "Add Line Item" (the Add item button): the item list stays on the left with its search box, the chosen item's details are filled in on the right, and the
// amount follows. Pick with a click or ↑ ↓ Enter, then Cases / Loose bottles (or Total bottles) and the rate; "Add & next" keeps the window open for the
// next item, "Add & close" (F12) adds and closes, Esc closes. F4 still opens the older one-window "Add Item Into Purchase Bill" (ItemLookupModal).
const LINE_IDS = ['al-cases', 'al-loose', 'al-total', 'al-rate', 'al-selling'];

export function AddLineItemModal({ items, minMarginPct = 0, onAdd, onClose }) {
  const [text, setText] = useState('');
  const [at, setAt] = useState(0);
  const [picked, setPicked] = useState(null);
  const [cases, setCases] = useState('');
  const [loose, setLoose] = useState('');
  const [rate, setRate] = useState('');
  const [selling, setSelling] = useState('');
  const [error, setError] = useState('');
  const [added, setAdded] = useState(0);

  const matches = useMemo(() => {
    const words = text.trim().toLowerCase().split(/\s+/).filter(Boolean);
    return items.filter(i => {
      const hay = `${i.code} ${i.shortName} ${i.hotKey} ${i.brand} ${i.name} ${i.pkg}`.toLowerCase();
      return words.every(w => hay.includes(w));
    }).slice(0, 300);
  }, [items, text]);
  const current = Math.max(0, Math.min(at, matches.length - 1));
  useEffect(() => { document.getElementById(`al-row-${current}`)?.scrollIntoView?.({ block: 'nearest' }); }, [current]);

  const perCase = picked?.bottlesPerCase || 1;
  const total = (Number(cases) || 0) * perCase + (Number(loose) || 0);
  const amount = Math.round(total * (Number(rate) || 0) * 100) / 100;
  const variation = Number(rate) > 0 ? ((Number(selling) || 0) - Number(rate)) / Number(rate) * 100 : 0;
  const lowMargin = minMarginPct > 0 && variation < minMarginPct;
  const setTotalBottles = (v) => {
    const t = Math.max(0, Math.floor(Number(v)) || 0);
    setCases(t ? String(Math.floor(t / perCase)) : ''); setLoose(t ? String(t % perCase) : ''); setError('');
  };

  const choose = (item) => {
    if (!item) return;
    setPicked(item);
    setRate(item.purchaseRate ? String(item.purchaseRate) : '');
    setSelling(item.rate ? String(item.rate) : '');
    setCases(''); setLoose(''); setError('');
    focusId('al-cases');
  };
  const submit = (keepOpen) => {
    if (!picked) { choose(matches[current]); return; }
    if (total <= 0) { setError('Enter the cases or loose bottles.'); focusId('al-cases'); return; }
    if (!(Number(rate) > 0)) { setError('Enter the purchase rate.'); focusId('al-rate'); return; }
    onAdd({ stock: picked.stock, qty: total, rate: Number(rate) }, keepOpen);
    if (keepOpen) { setAdded(n => n + 1); setPicked(null); setText(''); setAt(0); setCases(''); setLoose(''); setError(''); focusId('al-search'); }
  };
  const dialogKeys = (e) => {
    if (e.key === 'F12') { e.preventDefault(); e.stopPropagation(); submit(false); }
    else if (e.key === 'Enter') e.stopPropagation();   // must not reach the screen's own Enter-to-next-field behind
  };
  const fieldKeys = (i) => (e) => {
    if (e.key !== 'Enter') return;
    e.preventDefault();
    if (i < LINE_IDS.length - 1) focusId(LINE_IDS[i + 1]); else submit(true);
  };
  const inputBox = (i, id, label, value, onChange, { prefix, required } = {}) => (
    <div>
      <label htmlFor={id} className="mb-1 block text-[11px] font-semibold uppercase tracking-wide text-stone-500 dark:text-stone-400">{label}{required && <span className="ml-0.5 text-red-600" aria-hidden="true"> *</span>}</label>
      <div className="flex">
        {prefix && <span className="inline-flex items-center rounded-l-md border border-r-0 border-line bg-subtle px-2.5 text-sm text-stone-500">{prefix}</span>}
        <input id={id} type="number" min={0} step="any" value={value} onChange={e => onChange(e.target.value)} onFocus={e => e.target.select()} onKeyDown={fieldKeys(i)}
          disabled={!picked} className={`${field} w-full text-right tabular-nums ${prefix ? 'rounded-l-none' : ''}`} />
      </div>
    </div>
  );

  return (
    <Modal title="Add Line Item" onClose={onClose} wide
      footer={<>
        <span className="mr-auto hidden text-[11px] text-stone-500 sm:inline">
          {added > 0 ? `${added} item${added === 1 ? '' : 's'} added · ` : ''}F12 adds and closes · Esc closes
        </span>
        <button type="button" onClick={onClose} className="rounded-lg border border-line bg-surface px-5 py-2 text-xs font-bold text-stone-900 hover:bg-subtle dark:text-stone-50">Close</button>
        <button type="button" onClick={() => submit(true)} disabled={!picked} className="rounded-lg border border-line bg-surface px-5 py-2 text-xs font-bold text-stone-900 hover:bg-subtle disabled:opacity-40 dark:text-stone-50">Add &amp; next</button>
        <button type="button" onClick={() => submit(false)} disabled={!picked} className="rounded-lg bg-accent px-5 py-2 text-xs font-bold text-white hover:bg-accent-strong disabled:opacity-40">Add &amp; close</button>
      </>}>
      <div onKeyDown={dialogKeys} className="grid gap-5 lg:grid-cols-[minmax(0,1.05fr)_minmax(0,1fr)]">
        <section aria-label="Select item from master">
          <div className="mb-2 flex items-center justify-between text-[11px]">
            <h3 className="font-semibold uppercase tracking-wide text-stone-600 dark:text-stone-300">Select item from master</h3>
            <span className="text-stone-400">Click or use ↑ ↓ to select</span>
          </div>
          <div className="relative mb-2">
            <Search size={14} className="pointer-events-none absolute left-3 top-1/2 -translate-y-1/2 text-stone-400" aria-hidden="true" />
            <input id="al-search" data-autofocus autoFocus value={text} autoComplete="off" aria-label="Search items" placeholder="Type item code, name, short name or hot key…"
              onChange={e => { setText(e.target.value); setAt(0); if (picked) setPicked(null); }}
              onKeyDown={e => {
                if (e.key === 'ArrowDown' || e.key === 'ArrowUp') { e.preventDefault(); setAt(Math.max(0, Math.min(matches.length - 1, current + (e.key === 'ArrowDown' ? 1 : -1)))); }
                else if (e.key === 'Enter') { e.preventDefault(); choose(matches[current]); }
              }}
              className={`${field} w-full pl-9 pr-9`} />
            {text && <button type="button" onClick={() => { setText(''); setAt(0); focusId('al-search'); }} aria-label="Clear search" className="absolute right-2 top-1/2 -translate-y-1/2 rounded p-1 text-stone-400 hover:text-stone-700"><X size={14} /></button>}
          </div>
          <div className="max-h-[48vh] overflow-auto rounded-lg border border-line">
            <table className="w-full text-xs">
              <thead className="sticky top-0 bg-subtle text-[10px] uppercase tracking-wide text-stone-600 dark:text-stone-300">
                <tr><th className="px-3 py-2 text-left">Item code</th><th className="px-3 py-2 text-left">Item name</th><th className="px-3 py-2 text-right">In stock</th></tr>
              </thead>
              <tbody>
                {matches.map((it, n) => (
                  <tr key={it.stock} id={`al-row-${n}`} aria-selected={n === current} onClick={() => { setAt(n); choose(it); }}
                    className={`cursor-pointer border-t border-line-soft ${picked?.stock === it.stock ? 'bg-[#64283A]/15 dark:bg-[#D4A847]/20' : n === current ? 'bg-[#64283A]/8 dark:bg-[#D4A847]/10' : 'hover:bg-subtle'}`}>
                    <td className="px-3 py-1.5 font-mono">{it.code}</td>
                    <td className="px-3 py-1.5">{lineName(it)}</td>
                    <td className="px-3 py-1.5 text-right tabular-nums">{it.clQty}</td>
                  </tr>
                ))}
                {!matches.length && <tr><td colSpan={3} className="px-3 py-10 text-center text-stone-400">No liquor item matches.</td></tr>}
              </tbody>
            </table>
          </div>
          <p className="mt-2 text-[11px] text-stone-400">Use ↑ / ↓ to move · Enter to select the item</p>
        </section>

        <section aria-label="Line details">
          <div className="mb-4 rounded-xl border border-line bg-subtle/60 p-3">
            {picked ? (
              <>
                <div className="text-sm font-semibold">{lineName(picked)}</div>
                <div className="mt-1 flex flex-wrap gap-2 text-[11px] text-stone-600 dark:text-stone-300">
                  <span className="rounded-full border border-line px-2 py-0.5 font-mono">{picked.code}</span>
                  <span className="rounded-full border border-line px-2 py-0.5">{perCase} bottles per case</span>
                  <span className="rounded-full border border-line px-2 py-0.5">Stock {picked.clQty}</span>
                  <span className="rounded-full border border-line px-2 py-0.5">Reorder level {money(picked.rol ?? 0)}</span>
                </div>
              </>
            ) : <p className="text-sm text-stone-500">Pick an item on the left to enter its quantity and rate.</p>}
          </div>
          <div className="grid grid-cols-2 gap-3">
            {inputBox(0, 'al-cases', 'No. of cases', cases, v => { setCases(v); setError(''); })}
            {inputBox(1, 'al-loose', 'Loose bottles', loose, v => { setLoose(v); setError(''); })}
            <div>
              <label htmlFor="al-total" className="mb-1 block text-[11px] font-semibold uppercase tracking-wide text-stone-500 dark:text-stone-400">Total bottles (PB qty)<span className="ml-0.5 text-red-600" aria-hidden="true"> *</span></label>
              <input id="al-total" type="number" min={0} step={1} aria-label="Total bottles" value={total || ''} placeholder="0" disabled={!picked}
                onChange={e => setTotalBottles(e.target.value)} onFocus={e => e.target.select()} onKeyDown={fieldKeys(2)} className={`${field} w-full text-right tabular-nums`} />
            </div>
            {inputBox(3, 'al-rate', 'Purchase rate', rate, v => { setRate(v); setError(''); }, { prefix: '₹', required: true })}
            {inputBox(4, 'al-selling', 'Selling price', selling, setSelling, { prefix: '₹' })}
            <div className="flex flex-col justify-end text-xs">
              <span className={`text-sm font-semibold ${lowMargin ? 'text-red-600 dark:text-red-400' : 'text-stone-700 dark:text-stone-200'}`}>Price variation: {Math.round(variation * 100) / 100}%</span>
              {lowMargin && <span className="text-[11px] text-red-600 dark:text-red-400">At least {minMarginPct}% above the purchase rate (Global Settings).</span>}
            </div>
          </div>
          {error && <p role="alert" className="mt-3 text-xs text-red-600 dark:text-red-400">{error}</p>}
          <div className="mt-4 flex items-center justify-between rounded-xl bg-[#64283A] px-5 py-4 text-white dark:bg-[#2A2210] dark:text-[#F5D88A]">
            <div>
              <div className="text-[11px] font-semibold uppercase tracking-wide opacity-80">Amount</div>
              <div className="text-xs opacity-90">{total || 0} × {money(Number(rate) || 0)}</div>
            </div>
            <output aria-label="Amount" className="text-3xl font-bold tabular-nums">{money(amount)}</output>
          </div>
        </section>
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
          {!indents && <SkeletonTableRows rows={4} cols={6} />}
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
