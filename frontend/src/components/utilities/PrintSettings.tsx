// Global Settings → Print Settings (the desktop's Other Settings → Print Settings).
//
// For each document that has print formats (the Doc Designer's UtlOption rows, which the desktop reads too):
// what a click on Print, a Ctrl+click and a Ctrl+Alt+click do (Preview / Direct print / E-mail / Multi print), what
// happens after a save (Do not print / Ask first / Print at once), the printer for a direct print, the copies, the
// default format and whether the format is in use. The report file and its selection formula belong to the Doc
// Designer and are only shown. Each row saves on its own (Save waits for its popup). Administrator only.

import { useCallback, useEffect, useMemo, useState } from 'react';
import { RefreshCw, Save } from 'lucide-react';
import { toastStore } from '../common/toastStore';
import { InlineError } from '../common/FormErrorSummary';
import { errorText, getPrintBridge, getPrintSettings, savePrintOption, type ApiPrintOption, type ApiPrintSettings } from '../../lib/api';
import { badge, field, label, pageHeader, pageSub, pageTitle, panel, primaryButton, secondaryButton } from '../../lib/styles';

type Draft = Pick<ApiPrintOption, 'name' | 'copies' | 'default' | 'inactive' | 'click' | 'ctrl_click' | 'ctrl_alt_click' | 'after_save' | 'printer'>;

const draftOf = (o: ApiPrintOption): Draft => ({
  name: o.name, copies: o.copies, default: o.default, inactive: o.inactive, click: o.click, ctrl_click: o.ctrl_click,
  ctrl_alt_click: o.ctrl_alt_click, after_save: o.after_save, printer: o.printer,
});
const key = (o: ApiPrintOption) => `${o.doc_id}/${o.order}`;

/** The row's problem, or '' when it is fine (same rules as the backend). */
function problem(d: Draft): string {
  if (!d.name.trim()) return 'Enter the option name';
  if (!Number.isInteger(d.copies) || d.copies < 1 || d.copies > 20) return 'Copies must be from 1 to 20';
  if (d.default && d.inactive) return 'The default option must be in use';
  return '';
}

export default function PrintSettings({ embedded = false }: { embedded?: boolean }) {
  const [data, setData] = useState<ApiPrintSettings | null>(null);
  const [docId, setDocId] = useState<number | null>(null);
  const [drafts, setDrafts] = useState<Record<string, Draft>>({});
  const [errors, setErrors] = useState<Record<string, string>>({});
  const [printers, setPrinters] = useState<string[]>([]);
  const [saving, setSaving] = useState('');
  const [loading, setLoading] = useState(true);

  const load = useCallback(() => getPrintSettings()
    .then(d => { setData(d); setDrafts({}); setErrors({}); setDocId(cur => cur ?? d.documents[0]?.doc_id ?? null); return true; })
    .catch(e => { toastStore.add('error', errorText(e)); return false; })
    .finally(() => setLoading(false)), []);

  useEffect(() => { load(); }, [load]);
  useEffect(() => { getPrintBridge().then(b => setPrinters(b.printers ?? [])).catch(() => { /* typing a printer name still works */ }); }, []);

  const doc = useMemo(() => data?.documents.find(d => d.doc_id === docId) ?? null, [data, docId]);

  const edit = (o: ApiPrintOption, patch: Partial<Draft>) => {
    setDrafts(d => ({ ...d, [key(o)]: { ...(d[key(o)] ?? draftOf(o)), ...patch } }));
    setErrors(e => ({ ...e, [key(o)]: '' }));
  };
  const dirty = (o: ApiPrintOption) => {
    const d = drafts[key(o)];
    return !!d && JSON.stringify(d) !== JSON.stringify(draftOf(o));
  };

  const save = async (o: ApiPrintOption) => {
    const d = drafts[key(o)] ?? draftOf(o);
    const bad = problem(d);
    if (bad) { setErrors(e => ({ ...e, [key(o)]: bad })); return; }
    if (!(await toastStore.defer('ok', `Print settings of ${o.name} will be saved when this closes.`, 'Save cancelled. Nothing was changed.'))) return;
    setSaving(key(o));
    try {
      await savePrintOption(o.doc_id, o.order, d);
      toastStore.add('ok', `Print settings of ${d.name} saved.`);
      await load();
    } catch (e) {
      setErrors(er => ({ ...er, [key(o)]: errorText(e) }));
      toastStore.add('error', errorText(e));
    } finally {
      setSaving('');
    }
  };

  const select = 'rounded-md border border-[#E3D9CD] bg-white px-2 py-1 text-xs dark:border-[#2A2A2A] dark:bg-[#141414]';
  const clicks = Object.entries(data?.clicks ?? {});
  const afterSave = Object.entries(data?.after_save ?? {});

  return (
    <div className="mx-auto max-w-7xl">
      <div className={`${pageHeader} mb-5`}>
        <div>
          {!embedded && <h1 className={pageTitle}>Print Settings</h1>}
          <p className={pageSub}>How each document prints: what Print does, what happens after a save, the printer and copies. The desktop reads the same settings.</p>
        </div>
        <button onClick={load} className={secondaryButton}><RefreshCw size={14} />Reload</button>
      </div>

      {loading && <p className="text-xs text-stone-500">Loading…</p>}
      {data && !data.documents.length && (
        <p className={`${panel} p-4 text-xs text-stone-600 dark:text-stone-300`}>No document has print formats in the Doc Designer yet.</p>
      )}

      {data && data.documents.length > 0 && (
        <div className="flex flex-col gap-4 lg:flex-row">
          <div className="lg:w-64 lg:shrink-0">
            <p className={label}>Document</p>
            <div role="listbox" aria-label="Document" className="space-y-1">
              {data.documents.map(d => (
                <button key={d.doc_id} role="option" aria-selected={d.doc_id === docId} onClick={() => setDocId(d.doc_id)}
                  className={`flex w-full items-center justify-between gap-2 rounded-lg border px-3 py-2 text-left text-xs ${d.doc_id === docId
                    ? 'border-[#64283A] bg-[#64283A]/5 dark:border-[#D4A847] dark:bg-[#D4A847]/10' : 'border-[#E3D9CD] dark:border-[#2A2A2A]'}`}>
                  <span className="truncate font-medium">{d.name}</span>
                  <span className={badge('neutral')}>{d.options.length}</span>
                </button>
              ))}
            </div>
          </div>

          <div className="min-w-0 flex-1 space-y-3">
            {doc?.options.map(o => {
              const d = drafts[key(o)] ?? draftOf(o);
              const err = errors[key(o)];
              return (
                <section key={key(o)} className={`${panel} p-4`} aria-label={o.name}>
                  <div className="mb-3 flex flex-wrap items-center justify-between gap-2">
                    <div className="min-w-0">
                      <p className="truncate text-sm font-semibold">{o.name}{o.default && <span className={`${badge('success')} ml-2`}>Default</span>}{o.inactive && <span className={`${badge('neutral')} ml-2`}>Not in use</span>}</p>
                      <p className="truncate font-mono text-[11px] text-stone-500" title={o.selection}>{o.rpt}{o.selection ? `  ·  ${o.selection}` : ''}</p>
                    </div>
                    <button onClick={() => save(o)} disabled={!dirty(o) || saving === key(o)} className={primaryButton}><Save size={14} />Save</button>
                  </div>
                  <div className="grid grid-cols-1 gap-3 sm:grid-cols-2 lg:grid-cols-4">
                    <label><span className={label}>Option name</span>
                      <input value={d.name} onChange={e => edit(o, { name: e.target.value })} className={field} maxLength={50} /></label>
                    <label><span className={label}>Print button / click</span>
                      <select value={d.click} onChange={e => edit(o, { click: e.target.value })} className={`${select} w-full`}>
                        {clicks.map(([c, n]) => <option key={c} value={c}>{n}</option>)}
                      </select></label>
                    <label><span className={label}>Ctrl + click</span>
                      <select value={d.ctrl_click} onChange={e => edit(o, { ctrl_click: e.target.value })} className={`${select} w-full`}>
                        {clicks.map(([c, n]) => <option key={c} value={c}>{n}</option>)}
                      </select></label>
                    <label><span className={label}>Ctrl + Alt + click</span>
                      <select value={d.ctrl_alt_click} onChange={e => edit(o, { ctrl_alt_click: e.target.value })} className={`${select} w-full`}>
                        {clicks.map(([c, n]) => <option key={c} value={c}>{n}</option>)}
                      </select></label>
                    <label><span className={label}>After saving the document</span>
                      <select value={d.after_save} onChange={e => edit(o, { after_save: e.target.value })} className={`${select} w-full`}>
                        {afterSave.map(([c, n]) => <option key={c} value={c}>{n}</option>)}
                      </select></label>
                    <label><span className={label}>Printer (direct print)</span>
                      <input list={`printers-${key(o)}`} value={d.printer} onChange={e => edit(o, { printer: e.target.value })} className={field} maxLength={50} placeholder="Windows default" />
                      <datalist id={`printers-${key(o)}`}>{printers.map(p => <option key={p} value={p} />)}</datalist></label>
                    <label><span className={label}>Copies</span>
                      <input type="number" min={1} max={20} value={d.copies} onChange={e => edit(o, { copies: Math.trunc(Number(e.target.value)) })} className={field} /></label>
                    <div className="flex items-end gap-4 pb-1.5">
                      <label className="flex items-center gap-2 text-xs"><input type="checkbox" checked={d.default} onChange={e => edit(o, { default: e.target.checked })} />Default</label>
                      <label className="flex items-center gap-2 text-xs"><input type="checkbox" checked={!d.inactive} onChange={e => edit(o, { inactive: !e.target.checked })} />In use</label>
                    </div>
                  </div>
                  {err && <InlineError message={err} />}
                </section>
              );
            })}
          </div>
        </div>
      )}
    </div>
  );
}
