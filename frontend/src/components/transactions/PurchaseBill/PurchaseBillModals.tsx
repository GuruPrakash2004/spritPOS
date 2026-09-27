import { useState } from 'react';
import Modal from '../Modal';
import type { ApiIndent } from '../api';
import { SAMPLE_STOCK_DATA } from '../stockData';
import { field, headCell, ITEM_CODES, money } from './purchaseBillData';

export function ItemLookupModal({ filter, onFilter, onPick, onClose }: {
  filter: string; onFilter: (v: string) => void; onPick: (stock: number) => void; onClose: () => void;
}) {
  const lookupItems = SAMPLE_STOCK_DATA
    .map((item, stock) => ({ ...item, stock, code: ITEM_CODES[stock] }))
    .filter(item => `${item.code} ${item.brand} ${item.name}`.toLowerCase().includes(filter.toLowerCase()));
  return (
    <Modal title="Add Item" onClose={onClose} wide>
      <input autoFocus value={filter} onChange={e => onFilter(e.target.value)} placeholder="Search code, brand or name" className={`${field} mb-3 w-full`} />
      <table className="w-full text-xs">
        <thead className="bg-subtle text-stone-600 dark:text-stone-300">
          <tr><th className={headCell}>Code</th><th className={headCell}>Item</th><th className={`${headCell} text-right`}>QOH</th><th className={`${headCell} text-right`}>Purchase Rate</th></tr>
        </thead>
        <tbody className="divide-y divide-line-soft">
          {lookupItems.map(item => (
            <tr key={item.ksbcl} onClick={() => onPick(item.stock)} className="cursor-pointer hover:bg-accent/10">
              <td className="px-2 py-2 font-mono">{item.code}</td>
              <td className="px-2 py-2">{item.name} <span className="text-stone-400">{item.pkg}</span></td>
              <td className="px-2 py-2 text-right tabular-nums">{item.clQty}</td>
              <td className="px-2 py-2 text-right tabular-nums">{money(item.purchaseRate)}</td>
            </tr>
          ))}
        </tbody>
      </table>
    </Modal>
  );
}

export function CopyFromModal({ indents, onCopy, onClose }: {
  indents: ApiIndent[] | null; onCopy: (indent: ApiIndent) => void; onClose: () => void;
}) {
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

export function AddSupplierModal({ saving, onSave, onClose }: {
  saving: boolean; onSave: (name: string, address: string) => void; onClose: () => void;
}) {
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
