// Global Settings → Other Settings (the desktop's Global Settings → Other Settings window): Transaction Settings per
// document (legacy FrmUtlTrnSettings), Doc Prefix Settings (frmUtlDocNumber) and Menu Sequence (frmUtlMenuSequence).
// Bulk Item Import Fields (UtlItemBulkUpdate) is the list Update Item Master Fields offers. Print Settings is the tab beside it. Each save waits for its popup; mistakes are shown inline. Administrator only
// (backend: /utilities/doc-settings, /utilities/numbering, /utilities/menu).

import { useCallback, useEffect, useState } from 'react';
import { Save } from 'lucide-react';
import { toastStore } from '../common/toastStore';
import { InlineError } from '../common/FormErrorSummary';
import {
  errorText, getDocSettingsAdmin, getMenuAdmin, getNumbering, getTrnSettings, saveDocSettings, saveMenuAdmin, saveNumbering, saveTrnSettings,

} from '../../lib/api';
import AttachmentSettings from './AttachmentSettings';
import BulkImportFields from './BulkImportFields';
import DeleteLinkage from './DeleteLinkage';
import InvoiceSettings from './InvoiceSettings';
import { resetDocSettings } from '../../lib/docSettings';
import { MODULE_GROUPS } from '../../lib/modules';
import { field, label, pageHeader, pageSub, pageTitle, panel, primaryButton, secondaryButton } from '../../lib/styles';


const TABS = [['transaction', 'Transaction Settings'], ['prefix', 'Doc Prefix'], ['menu', 'Menu Sequence'], ['attachment', 'File Attachment'], ['invoice', 'Invoice Settings'], ['linkage', 'Delete Linkage'], ['bulkfields', 'Bulk Item Import Fields']];
const select = 'rounded-md border border-[#E3D9CD] bg-white px-2 py-1 text-xs dark:border-[#2A2A2A] dark:bg-[#141414]';

export default function OtherSettings({ embedded = false }) {
  const [tab, setTab] = useState('transaction');
  return (
    <div className={tab === 'linkage' || tab === 'invoice' ? '' : 'mx-auto max-w-5xl'}>
      {!embedded && (
        <div className={`${pageHeader} mb-4`}>
          <div>
            <h1 className={pageTitle}>Other Settings</h1>
            <p className={pageSub}>Transaction settings, document prefixes and the menu.</p>
          </div>
        </div>
      )}
      <div role="tablist" className="mb-4 flex gap-1 border-b border-[#E3D9CD] dark:border-[#2A2A2A]">
        {TABS.map(([id, name]) => (
          <button key={id} role="tab" aria-selected={tab === id} onClick={() => setTab(id)}
            className={`-mb-px border-b-2 px-4 py-2 text-xs font-semibold ${tab === id ? 'border-[#64283A] text-[#64283A] dark:border-[#D4A847] dark:text-[#D4A847]' : 'border-transparent text-stone-500'}`}>{name}</button>
        ))}
      </div>
      {tab === 'transaction' && <TransactionSettings />}
      {tab === 'prefix' && <DocPrefix />}
      {tab === 'menu' && <MenuSequence />}
      {tab === 'attachment' && <AttachmentSettings />}
      {tab === 'invoice' && <InvoiceSettings onClose={() => setTab('transaction')} />}
      {tab === 'bulkfields' && <BulkImportFields />}
      {tab === 'linkage' &&<DeleteLinkage onClose={() => setTab('transaction')} />}
    </div>
  );
}

// ─── Transaction Settings ──────────────────────────────────────────────────────────────────────────

// The desktop window's other settings of one document (UtlTransactions). The web app does not act on these; they are shown and
// saved so the desktop and the web read the same row. Switches that clear their details when off do so on the server, as the desktop.
function DesktopSettings({ doc, fields, skip = [], onSaved }) {
  const [draft, setDraft] = useState(doc.values);
  const [error, setError] = useState('');
  const [saving, setSaving] = useState(false);
  const shown = fields.filter(f => f.column in doc.values && !skip.includes(f.column));
  const changed = Object.fromEntries(shown.filter(f => draft[f.column] !== doc.values[f.column]).map(f => [f.column, draft[f.column]]));
  const save = async () => {
    if (!(await toastStore.defer('ok', `Desktop settings of ${doc.name} will be saved when this closes.`, 'Save cancelled. Nothing was changed.'))) return;
    setSaving(true);
    try {
      await saveTrnSettings(doc.doc_id, changed);
      toastStore.add('ok', `Transaction settings of ${doc.name} saved.`);
      onSaved();
    } catch (e) {
      setError(errorText(e));
    } finally {
      setSaving(false);
    }
  };
  if (!shown.length) return null;
  return (
    <section className={`${panel} space-y-3 p-4`} aria-label={`${doc.name} desktop settings`}>
      <div className="flex items-center justify-between gap-2">
        <div>
          <h3 className="text-sm font-semibold">Desktop settings: {doc.name}</h3>
          <p className="text-[11px] text-stone-500">Saved in the shop's database for the desktop app. The web app does not act on these.</p>
        </div>
        <button onClick={save} disabled={!Object.keys(changed).length || saving} className={primaryButton}><Save size={14} />Save</button>
      </div>
      <div className="grid gap-3 sm:grid-cols-2">
        {shown.filter(f => f.kind === 'flag').map(f => (
          <label key={f.column} className="flex items-center gap-2 text-xs">
            <input type="checkbox" checked={!!draft[f.column]} onChange={e => { setDraft({ ...draft, [f.column]: e.target.checked }); setError(''); }} />{f.caption}
          </label>
        ))}
      </div>
      <div className="grid gap-3 sm:grid-cols-2">
        {shown.filter(f => f.kind !== 'flag').map(f => (
          <label key={f.column}><span className={label}>{f.caption}</span>
            <input value={draft[f.column] ?? ''} type={f.kind === 'number' ? 'number' : 'text'} className={field}
              onChange={e => { setDraft({ ...draft, [f.column]: f.kind === 'number' ? Math.trunc(Number(e.target.value)) : e.target.value }); setError(''); }} /></label>
        ))}
      </div>
      {error && <InlineError message={error} />}
    </section>
  );
}

function TransactionSettings() {
  const [all, setAll] = useState(null);
  const [kind, setKind] = useState('');
  const [desktop, setDesktop] = useState(null);   // { docs, fields } of UtlTransactions, or null without it
  const [otherDoc, setOtherDoc] = useState(null);  // a desktop-only document picked from "Other documents"
  const loadDesktop = useCallback(() => getTrnSettings().then(setDesktop).catch(() => setDesktop(null)), []);
  useEffect(() => { loadDesktop(); }, [loadDesktop]);
  const [draft, setDraft] = useState(null);
  const [error, setError] = useState('');
  const [saving, setSaving] = useState(false);

  const load = useCallback(() => getDocSettingsAdmin()
    .then(a => { setAll(a); setKind(k => k || Object.keys(a)[0] || ''); })
    .catch(e => toastStore.add('error', errorText(e))), []);
  useEffect(() => { load(); }, [load]);
  // The edit copy restarts whenever another document or freshly loaded settings are chosen.
  // eslint-disable-next-line react-hooks/set-state-in-effect
  useEffect(() => { setDraft(all && kind ? { ...all[kind] } : null); setError(''); }, [all, kind]);

  if (!all || !draft) return <p className="text-xs text-stone-500">Loading…</p>;
  const cur = all[kind];
  const dirty = JSON.stringify(draft) !== JSON.stringify(cur);
  const set = (p) => { setDraft({ ...draft, ...p }); setError(''); };

  const save = async () => {
    if (!Number.isInteger(draft.edit_days) || draft.edit_days < 0 || draft.edit_days > 3650) { setError('Edit days must be a whole number from 0 to 3650'); return; }
    if (!(await toastStore.defer('ok', `Transaction settings of ${cur.screen} will be saved when this closes.`, 'Save cancelled. Nothing was changed.'))) return;
    setSaving(true);
    try {
      await saveDocSettings(kind, draft);
      resetDocSettings();
      toastStore.add('ok', `Transaction settings of ${cur.screen} saved.`);
      await load();
    } catch (e) {
      setError(errorText(e));
    } finally {
      setSaving(false);
    }
  };

  const check = (id, text, note) => (
    <label className="flex items-start gap-2 text-xs">
      <input type="checkbox" className="mt-0.5" checked={!!draft[id]} onChange={e => set({ [id]: e.target.checked } )} />
      <span>{text}{note && <span className="block text-[11px] text-stone-500">{note}</span>}</span>
    </label>
  );

  return (
    <div className="flex flex-col gap-4 lg:flex-row">
      <div className="lg:w-56 lg:shrink-0">
        <p className={label}>Transaction</p>
        <div role="listbox" aria-label="Transaction" className="space-y-1">
          {Object.entries(all).map(([k, s]) => (
            <button key={k} role="option" aria-selected={k === kind && !otherDoc} onClick={() => { setKind(k); setOtherDoc(null); }}
              className={`w-full rounded-lg border px-3 py-2 text-left text-xs font-medium ${k === kind && !otherDoc ? 'border-[#64283A] bg-[#64283A]/5 dark:border-[#D4A847] dark:bg-[#D4A847]/10' : 'border-[#E3D9CD] dark:border-[#2A2A2A]'}`}>{s.screen}</button>
          ))}
          {desktop?.docs.filter(d => !d.kind).length > 0 && <p className="pt-2 text-[11px] font-semibold uppercase text-stone-500">Other documents</p>}
          {desktop?.docs.filter(d => !d.kind).map(d => (
            <button key={d.doc_id} role="option" aria-selected={otherDoc === d.doc_id} onClick={() => setOtherDoc(d.doc_id)}
              className={`w-full rounded-lg border px-3 py-2 text-left text-xs font-medium ${otherDoc === d.doc_id ? 'border-[#64283A] bg-[#64283A]/5 dark:border-[#D4A847] dark:bg-[#D4A847]/10' : 'border-[#E3D9CD] dark:border-[#2A2A2A]'}`}>{d.name}</button>
          ))}
        </div>
      </div>
      <div className="min-w-0 flex-1 space-y-4">
      {otherDoc && desktop && (() => {
        const d = desktop.docs.find(x => x.doc_id === otherDoc);
        return d ? <DesktopSettings key={d.doc_id + JSON.stringify(d.values)} doc={d} fields={desktop.fields} onSaved={loadDesktop} /> : null;
      })()}
      {!otherDoc && <section className={`${panel} space-y-4 p-4`} aria-label={`${cur.screen} settings`}>
        <div className="flex items-center justify-between gap-2">
          <h2 className="text-sm font-semibold">{cur.screen}</h2>
          <button onClick={save} disabled={!dirty || saving} className={primaryButton}><Save size={14} />Save</button>
        </div>
        <p className="text-[11px] text-stone-500">{cur.affects_stock ? 'This transaction changes stock.' : 'This transaction does not change stock.'}</p>
        {cur.prints && (
          <div className="grid gap-3 sm:grid-cols-2">
            <label><span className={label}>After saving</span>
              <select value={draft.after_save} onChange={e => set({ after_save: e.target.value  })} className={`${select} w-full`}>
                <option value="none">Do not print</option><option value="prompt">Ask whether to print</option><option value="preview">Show the print preview</option>
              </select></label>
            <label><span className={label}>Default print format</span>
              <input value={draft.default_format} onChange={e => set({ default_format: e.target.value })} className={field} maxLength={60} /></label>
          </div>
        )}
        <div className="grid gap-3 sm:grid-cols-2">
          {check('allow_duplicate_items', 'Allow the same item on more than one line')}
          {check('allow_edit_date', 'Allow changing the document date', 'Off: the document must be dated today.')}
          {kind === 'purchase_bill' && check('allow_copy_from', 'Allow Copy From (a Purchase Indent)')}
          {/* "Update Rate From PB to Item Master" lives in Invoice Settings (UtlMisc.RateFld), one switch for the desktop and the web */}
        </div>
        {kind === 'sales_invoice' && (
          <label className="block max-w-xs"><span className={label}>Bills older than this many days can't be cancelled (0 = no limit)</span>
            <input type="number" min={0} max={3650} value={draft.edit_days} onChange={e => set({ edit_days: Math.trunc(Number(e.target.value)) })} className={field} /></label>
        )}
        {error && <InlineError message={error} />}
      </section>}
      {!otherDoc && desktop && (() => {
        const d = desktop.docs.find(x => x.kind === kind);
        return d ? <DesktopSettings key={d.doc_id + JSON.stringify(d.values)} doc={d} fields={desktop.fields}
          skip={kind === 'purchase_bill' ? ['RateFldMstItem'] : []} onSaved={loadDesktop} /> : null;
      })()}
      </div>
    </div>
  );
}

// ─── Doc Prefix ────────────────────────────────────────────────────────────────────────────────────

function DocPrefix() {
  const [rows, setRows] = useState(null);
  const [drafts, setDrafts] = useState({});
  const [errors, setErrors] = useState({});
  const [saving, setSaving] = useState('');

  const load = useCallback(() => getNumbering().then(r => { setRows(r); setDrafts({}); setErrors({}); })
    .catch(e => toastStore.add('error', errorText(e))), []);
  useEffect(() => { load(); }, [load]);

  if (!rows) return <p className="text-xs text-stone-500">Loading…</p>;

  const save = async (r) => {
    const d = drafts[r.kind];
    if (!d) return;
    const prefix = d.prefix.trim();
    const next = Number(d.next);
    if (!r.from_counter && !/^[A-Za-z0-9]{1,6}$/.test(prefix)) { setErrors(e => ({ ...e, [r.kind]: 'Prefix: 1 to 6 letters or digits' })); return; }
    if (!Number.isInteger(next) || next < 1 || next > 9_999_999) { setErrors(e => ({ ...e, [r.kind]: 'Next number: a whole number from 1 to 9999999' })); return; }
    if (!(await toastStore.defer('ok', `Numbering of ${r.screen} will be saved when this closes.`, 'Save cancelled. Nothing was changed.'))) return;
    setSaving(r.kind);
    try {
      await saveNumbering(r.kind, { ...(r.from_counter ? {} : { prefix }), next_number: next });
      toastStore.add('ok', `Numbering of ${r.screen} saved.`);
      await load();
    } catch (e) {
      setErrors(er => ({ ...er, [r.kind]: errorText(e) }));
    } finally {
      setSaving('');
    }
  };

  return (
    <section className={`${panel} divide-y divide-[#E3D9CD] dark:divide-[#2A2A2A]`} aria-label="Doc Prefix">
      {rows.map(r => {
        const d = drafts[r.kind] ?? { prefix: r.prefix ?? '', next: String(r.next_number) };
        const dirty = !!drafts[r.kind] && (d.prefix !== (r.prefix ?? '') || d.next !== String(r.next_number));
        return (
          <div key={r.kind} className="grid items-end gap-3 p-3 sm:grid-cols-[1fr_9rem_9rem_auto]">
            <div className="min-w-0"><p className="text-xs font-semibold">{r.screen}</p><p className="truncate font-mono text-[11px] text-stone-500">{r.example}</p></div>
            <label><span className={label}>Prefix</span>
              <input value={r.from_counter ? 'Sales Counter' : d.prefix} disabled={r.from_counter} maxLength={6} className={field}
                onChange={e => { setDrafts(s => ({ ...s, [r.kind]: { ...d, prefix: e.target.value } })); setErrors(x => ({ ...x, [r.kind]: '' })); }} /></label>
            <label><span className={label}>Next number</span>
              <input value={d.next} inputMode="numeric" className={field}
                onChange={e => { setDrafts(s => ({ ...s, [r.kind]: { ...d, next: e.target.value } })); setErrors(x => ({ ...x, [r.kind]: '' })); }} /></label>
            <button onClick={() => save(r)} disabled={!dirty || saving === r.kind} className={primaryButton}><Save size={14} />Save</button>
            {errors[r.kind] && <div className="sm:col-span-4"><InlineError message={errors[r.kind]} /></div>}
          </div>
        );
      })}
    </section>
  );
}

// ─── Menu Sequence ─────────────────────────────────────────────────────────────────────────────────

function MenuSequence() {
  const [layout, setLayout] = useState(null);
  const [hidden, setHidden] = useState(new Set());
  const [codes, setCodes] = useState({});
  const [error, setError] = useState('');
  const [saving, setSaving] = useState(false);

  const load = useCallback(() => getMenuAdmin().then(l => { setLayout(l); setHidden(new Set(l.hidden)); setCodes({ ...l.codes }); setError(''); })
    .catch(e => toastStore.add('error', errorText(e))), []);
  useEffect(() => { load(); }, [load]);

  if (!layout) return <p className="text-xs text-stone-500">Loading…</p>;
  const dirty = JSON.stringify([...hidden].sort()) !== JSON.stringify([...layout.hidden].sort())
    || JSON.stringify(codes) !== JSON.stringify(layout.codes);

  const save = async () => {
    const cleaned = Object.fromEntries(Object.entries(codes).map(([m, c]) => [m, c.trim()]).filter(([, c]) => c));
    const bad = Object.entries(cleaned).filter(([, c]) => !/^\d{1,6}$/.test(c));
    if (bad.length) { setError(`Shortcut codes are 1 to 6 digits: ${bad.map(([m]) => m).join(', ')}`); return; }
    const seen = new Set();
    const dup = Object.values(cleaned).filter(c => seen.has(c) || !seen.add(c));
    if (dup.length) { setError(`Duplicate data for Shortcut Menu Name: ${[...new Set(dup)].join(', ')}`); return; }
    if (!(await toastStore.defer('ok', 'The menu will be saved when this closes.', 'Save cancelled. Nothing was changed.'))) return;
    setSaving(true);
    try {
      await saveMenuAdmin({ hidden: [...hidden], codes: cleaned });
      toastStore.add('ok', 'Menu saved. Reload the page to see it in the sidebar.');
      await load();
    } catch (e) {
      setError(errorText(e));
    } finally {
      setSaving(false);
    }
  };

  return (
    <section className={`${panel} p-4`} aria-label="Menu Sequence">
      <div className="mb-3 flex items-center justify-between gap-2">
        <p className="text-[11px] text-stone-500">Untick Show to hide a screen from the sidebar and search. A shortcut code (e.g. 201) opens the screen from the module search.</p>
        <div className="flex gap-2">
          <button onClick={load} disabled={!dirty} className={secondaryButton}>Undo</button>
          <button onClick={save} disabled={!dirty || saving} className={primaryButton}><Save size={14} />Save</button>
        </div>
      </div>
      {error && <div className="mb-2"><InlineError message={error} /></div>}
      <div className="space-y-4">
        {MODULE_GROUPS.map(g => (
          <div key={g.group}>
            <p className={label}>{g.group}</p>
            <div className="divide-y divide-[#E3D9CD] rounded-lg border border-[#E3D9CD] dark:divide-[#2A2A2A] dark:border-[#2A2A2A]">
              {g.modules.map(m => (
                <div key={m} className="grid grid-cols-[1fr_5rem_8rem] items-center gap-3 px-3 py-1.5 text-xs">
                  <span className="truncate">{m}</span>
                  <label className="flex items-center gap-1.5"><input type="checkbox" checked={!hidden.has(m)} disabled={m === 'Admin Master'}
                    onChange={e => { const n = new Set(hidden); if (e.target.checked) n.delete(m); else n.add(m); setHidden(n); setError(''); }} />Show</label>
                  <input aria-label={`Shortcut code for ${m}`} value={codes[m] ?? ''} placeholder="Shortcut" maxLength={6} className={field}
                    onChange={e => { setCodes(c => ({ ...c, [m]: e.target.value })); setError(''); }} />
                </div>
              ))}
            </div>
          </div>
        ))}
      </div>
    </section>
  );
}
