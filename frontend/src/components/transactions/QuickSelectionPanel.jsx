// The Sales Invoice's "Quick Selection" panel (the desktop's panel left of the bill): the items of a named set kept in the shop's database
// (MstQuickSelItems). Pick the set at the top, click an item (or ↑↓ + Enter) to put it on the bill. Edit changes the set right here, as in
// the desktop: add items from Item Master, remove them, make a new set or delete the set; Save writes it to the database.
//
//   <QuickSelectionPanel onAdd={stockIndex => …} onClose={() => …} />
// With kind="template" it is the desktop's F6 Load Template instead (MstItemTemp): same panel, plus "Add all" that puts every item of the set on the bill.

import { useCallback, useEffect, useMemo, useState } from 'react';
import { Pencil, Plus, Save, Search, Trash2, X } from 'lucide-react';
import { deleteItemTemplate, deleteQuickSelection, errorText, getItemTemplates, getQuickSelections, saveItemTemplate, saveQuickSelection } from '../../lib/api';
import { sellable, stockVersion, useStockItems } from '../../data/stockData';
import { useConfirm } from '../common/ConfirmDialog';
import { toast, toastStore } from '../common/toastStore';
import { field, iconButton, money, panel, primaryButton, secondaryButton } from '../../lib/styles';

const LAST = 'vlqms_quick_set';   // the set the clerk used last, kept on this computer

const APIS = {
  quick: { title: 'Quick Selection', get: getQuickSelections, save: saveQuickSelection, del: deleteQuickSelection, last: LAST },
  template: { title: 'Template', get: getItemTemplates, save: saveItemTemplate, del: deleteItemTemplate, last: 'vlqms_item_template' },
};

export default function QuickSelectionPanel({ onAdd, onAddAll, onClose, kind = 'quick' }) {
  const api = APIS[kind];
  useStockItems();
  const { confirm, dialog } = useConfirm();
  const [sets, setSets] = useState(null);   // [{ name, codes }]; null until loaded
  const [name, setName] = useState(() => { try { return localStorage.getItem(api.last) ?? ''; } catch { return ''; } });
  const [error, setError] = useState('');
  const [filter, setFilter] = useState('');
  const [at, setAt] = useState(0);
  const [editing, setEditing] = useState(null);   // while editing: { name, codes, isNew } (a copy; nothing is saved until Save)
  const [find, setFind] = useState('');   // Edit: search among all items to add

  const load = useCallback(() => api.get().then(r => { setSets(r.sets); setError(''); }).catch(e => { setSets([]); setError(errorText(e)); }), [api]);
  useEffect(() => { void load(); }, [load]);

  const current = sets?.find(s => s.name === name) ?? sets?.[0] ?? null;
  const shownName = editing ? editing.name : current?.name ?? '';
  const codes = editing ? editing.codes : current?.codes ?? [];
  const version = stockVersion();   // stock changes as bills are saved
  const items = useMemo(() => {
    const q = filter.trim().toLowerCase();
    const byCode = new Map(sellable().map(i => [i.code, i]));
    return codes.map(code => byCode.get(code)).filter(i => i && (!q || `${i.brand} ${i.name} ${i.code}`.toLowerCase().includes(q)));
  }, [codes, filter, version]);   // eslint-disable-line react-hooks/exhaustive-deps

  const choose = (n) => { setName(n); setAt(0); try { localStorage.setItem(api.last, n); } catch { /* not remembered */ } };
  const pick = (item) => { if (item) onAdd(item.stock); };

  const startEdit = () => setEditing({ name: current?.name ?? '', codes: [...(current?.codes ?? [])], isNew: !current });
  const startNewSet = () => setEditing({ name: '', codes: [], isNew: true });   // type the name, search items to add, Save
  const save = async () => {
    if (!editing.name.trim()) return setError('Enter a name for the quick selection.');
    if (!(await toastStore.defer('ok', 'The quick selection will be saved when this closes.', 'Cancelled. The quick selection was not changed.'))) return;
    try {
      const saved = await api.save(editing.name.trim(), editing.codes);
      await load();
      choose(saved.name);
      setEditing(null);
      toast.ok('Quick selection saved.');
    } catch (e) { setError(errorText(e)); }
  };
  const removeSet = async () => {
    if (!current || !(await confirm(`Delete the quick selection '${current.name}'?`))) return;
    if (!(await toastStore.defer('delete', `Quick selection '${current.name}' will be deleted when this closes.`, 'Cancelled. Nothing was deleted.'))) return;
    try { await api.del(current.name); await load(); setEditing(null); } catch (e) { setError(errorText(e)); }
  };

  // Edit: the items that can be added (not already in the set), narrowed by the search box.
  const addable = useMemo(() => {
    if (!editing) return [];
    const q = find.trim().toLowerCase();
    return sellable().filter(i => !editing.codes.includes(i.code) && q && `${i.brand} ${i.name} ${i.code} ${i.shortName ?? ''}`.toLowerCase().includes(q)).slice(0, 30);
  }, [editing, find]);

  const cardKeys = (e) => {
    if (e.key === 'Escape') { e.preventDefault(); if (editing) setEditing(null); else onClose(); return; }
    if (editing) return;
    if (e.key === 'ArrowDown' || e.key === 'ArrowUp') { e.preventDefault(); setAt(a => Math.max(0, Math.min(items.length - 1, a + (e.key === 'ArrowDown' ? 1 : -1)))); }
    if (e.key === 'Enter' && items[at]) { e.preventDefault(); pick(items[at]); }
  };

  return (
    <div id="si-quick" className={`${panel} flex max-h-[520px] flex-col overflow-hidden`}>
      {dialog}
      <div className="flex items-center gap-1 border-b border-line px-3 py-2.5">
        <span className="min-w-0 flex-1 truncate text-xs font-semibold text-stone-900 dark:text-stone-50">{api.title}</span>
        {!editing && <button onClick={startEdit} className={`${iconButton} h-7 w-7`} aria-label="Edit quick selection" title="Edit this quick selection"><Pencil size={13} /></button>}
        <button onClick={onClose} className={`${iconButton} h-7 w-7`} aria-label="Close quick selection"><X size={14} /></button>
      </div>

      <div className="flex items-center gap-1 border-b border-line p-2">
        {editing
          ? <input value={editing.name} onChange={e => setEditing(x => ({ ...x, name: e.target.value }))} readOnly={!editing.isNew} aria-label="Quick selection name" className={`${field} h-8 flex-1 py-1 text-xs`} />
          : (
            <select value={shownName} onChange={e => choose(e.target.value)} aria-label="Quick selection" disabled={!sets?.length} className={`${field} h-8 flex-1 py-1 text-xs`}>
              {!sets?.length && <option value="">{sets === null ? 'Loading…' : 'No quick selection yet'}</option>}
              {sets?.map(s => <option key={s.name}>{s.name}</option>)}
            </select>
          )}
        {!editing && <button onClick={startNewSet} className={`${iconButton} h-8 w-8`} aria-label="New quick selection" title="New quick selection"><Plus size={14} /></button>}
      </div>
      {error && <p role="alert" className="border-b border-line px-3 py-1.5 text-[11px] text-red-600 dark:text-red-400">{error}</p>}

      {editing && (
        <div className="relative border-b border-line p-2">
          <Search size={14} className="pointer-events-none absolute left-4 top-1/2 -translate-y-1/2 text-stone-400" />
          <input autoFocus value={find} onChange={e => setFind(e.target.value)} onKeyDown={cardKeys} placeholder="Search an item to add" aria-label="Search an item to add" className={`${field} pl-8`} />
        </div>
      )}
      {editing && !!addable.length && (
        <ul className="max-h-32 overflow-auto border-b border-line p-1 text-xs">
          {addable.map(i => (
            <li key={i.code}>
              <button tabIndex={-1} onClick={() => { setEditing(x => ({ ...x, codes: [...x.codes, i.code] })); setFind(''); }} className="flex w-full items-center gap-2 rounded-lg px-2.5 py-1.5 text-left hover:bg-subtle">
                <Plus size={12} className="shrink-0 text-stone-400" /><span className="min-w-0 flex-1 truncate">{i.name}</span>
              </button>
            </li>
          ))}
        </ul>
      )}

      {!editing && (
        <div className="relative border-b border-line p-2">
          <Search size={14} className="pointer-events-none absolute left-4 top-1/2 -translate-y-1/2 text-stone-400" />
          <input autoFocus value={filter} onChange={e => { setFilter(e.target.value); setAt(0); }} onKeyDown={cardKeys} placeholder="Filter, ↑↓, Enter adds" aria-label="Filter items" className={`${field} pl-8`} />
        </div>
      )}

      <ul className="min-h-24 flex-1 overflow-auto p-1 text-xs">
        {items.map((item, qi) => (
          <li key={item.code} className="flex items-center gap-1">
            <button tabIndex={-1} disabled={!!editing} onClick={() => pick(item)} ref={!editing && qi === at ? el => el?.scrollIntoView({ block: 'nearest' }) : undefined}
              className={`min-w-0 flex-1 rounded-lg px-2.5 py-2 text-left transition-colors hover:bg-subtle disabled:cursor-default ${!editing && qi === at ? 'bg-accent-soft' : ''} ${item.clQty > 0 ? '' : 'opacity-50'}`}>
              <span className="block truncate font-medium text-stone-900 dark:text-stone-50">{item.brand && item.brand !== '-' ? item.brand : item.name}</span>
              <span className="mt-0.5 flex justify-between text-[11px] tabular-nums text-stone-500 dark:text-stone-400">
                <span>{[item.pkg && item.pkg !== '-' ? item.pkg : '', `${item.clQty} in stock`].filter(Boolean).join(' · ')}</span>
                <span className="font-medium text-stone-700 dark:text-stone-300">₹ {money(item.rate)}</span>
              </span>
            </button>
            {editing && <button onClick={() => setEditing(x => ({ ...x, codes: x.codes.filter(c => c !== item.code) }))} className={`${iconButton} h-7 w-7 shrink-0 hover:!text-red-600`} aria-label={`Remove ${item.name}`}><X size={13} /></button>}
          </li>
        ))}
        {!items.length && <li className="px-3 py-6 text-center text-stone-500">{editing ? 'No items yet: search above to add.' : current ? 'No items match.' : 'No quick selection yet. Use + to make one.'}</li>}
      </ul>

      {editing ? (
        <div className="flex items-center justify-between gap-2 border-t border-line p-2">
          {!editing.isNew ? <button onClick={() => void removeSet()} className={`${secondaryButton} !px-2.5 text-red-700`}><Trash2 size={13} />Delete set</button> : <span />}
          <div className="flex gap-1">
            <button onClick={() => void save()} className={`${primaryButton} !px-3`}><Save size={13} />Save</button>
            <button onClick={() => setEditing(null)} className={`${secondaryButton} !px-3`}>Cancel</button>
          </div>
        </div>
      ) : (
        <div className="flex items-center justify-between gap-2 border-t border-line px-3 py-1.5">
          <p className="text-[10px] text-stone-500">{items.length} item{items.length === 1 ? '' : 's'} · Edit changes this list</p>
          {kind === 'template' && <button onClick={() => { if (onAddAll) onAddAll(items.map(i => i.stock)); else items.forEach(i => onAdd(i.stock)); onClose(); }} disabled={!items.length} className={`${primaryButton} !px-3 !py-1 text-xs`}>Add all to bill</button>}
        </div>
      )}
    </div>
  );
}
