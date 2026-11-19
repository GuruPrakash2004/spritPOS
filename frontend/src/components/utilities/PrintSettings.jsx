// Global Settings → Print Settings (the desktop's Other Settings → Print Settings and its Print Option Name window).
//
// For each document that has print formats (the Doc Designer's UtlOption rows, which the desktop reads too):
// what a click on Print, a Ctrl+click and a Ctrl+Alt+click do (Preview / Direct print / E-mail / Multi print), what
// happens after a save (Do not print / Ask first / Print at once), the printer for a direct print, the copies, the
// default format and whether the format is in use; the report file and selection formula; the e-mail details (routing,
// subject, body, footer, condition and the To / Cc / Bcc rows) and the multi-print details (date range, document number
// range, track printed, query). Add option, Delete (asks first) and Alter Order (move up / down, Save) work as on the
// desktop. Each row saves on its own (Save waits for its popup). Administrator only.

import { useCallback, useEffect, useMemo, useState } from 'react';
import { ArrowDown, ArrowUp, ListOrdered, Plus, RefreshCw, Save, Trash2 } from 'lucide-react';
import { toastStore } from '../common/toastStore';
import { useConfirm } from '../common/ConfirmDialog';
import RptFolderSetting from '../common/RptFolderSetting';
import SmtpSetting from '../common/SmtpSetting';
import ServerPrinterSetting from '../common/ServerPrinterSetting';
import LocalPrinterSetting from '../common/LocalPrinterSetting';
import { detectAgent } from '../../lib/localPrint';
import { InlineError } from '../common/FormErrorSummary';
import { addPrintOption, deletePrintOption, errorText, getPrintBridge, getPrintSettings, orderPrintOptions, savePrintOption, } from '../../lib/api';
import { badge, field, label, pageHeader, pageSub, pageTitle, panel, primaryButton, secondaryButton } from '../../lib/styles';

const FIELDS = ['name', 'rpt', 'selection', 'copies', 'default', 'inactive', 'click', 'ctrl_click', 'ctrl_alt_click', 'after_save', 'printer',
  'email_routing', 'attach_copy', 'email_subject', 'email_body', 'email_footer', 'email_cond', 'allow_date_range', 'allow_docno_range',
  'track_printed', 'multi_query', 'emails'];
const draftOf = (o) => Object.fromEntries(FIELDS.map(f => [f, o[f]]));
const key = (o) => `${o.doc_id}/${o.order}`;
const blankEmail = () => ({ fixed: true, type: 'T', email: '', field: '', cond: '' });

/** The row's problem, or '' when it is fine (same rules as the backend). */
function problem(d) {
  if (!d.name.trim()) return 'Enter the option name';
  if (!d.click) return 'Invalid Print Click....';
  if (!Number.isInteger(d.copies) || d.copies < 1 || d.copies > 20) return 'Copies must be from 1 to 20';
  if (d.default && d.inactive) return 'The default option must be in use';
  return '';
}

const select = 'rounded-md border border-[#E3D9CD] bg-white px-2 py-1 text-xs dark:border-[#2A2A2A] dark:bg-[#141414]';

// One option's form: the main settings, plus e-mail and multi-print details when a click uses them (as the desktop's window).
function OptionForm({ d, onChange, data, printerList, listId }) {
  const clicks = Object.entries(data?.clicks ?? {});
  const afterSave = Object.entries(data?.after_save ?? {});
  const uses = [d.click, d.ctrl_click, d.ctrl_alt_click];
  const hasEmail = uses.includes('E');
  const hasMulti = uses.includes('M');
  const setMail = (i, patch) => onChange({ emails: d.emails.map((r, n) => (n === i ? { ...r, ...patch } : r)) });
  return (
    <>
      <div className="grid grid-cols-1 gap-3 sm:grid-cols-2 lg:grid-cols-4">
        <label><span className={label}>Option name</span>
          <input value={d.name} onChange={e => onChange({ name: e.target.value })} className={field} maxLength={50} /></label>
        <label><span className={label}>Report file (.rpt)</span>
          <input value={d.rpt} onChange={e => onChange({ rpt: e.target.value })} className={field} maxLength={100} /></label>
        <label className="sm:col-span-2"><span className={label}>Selection formula</span>
          <input value={d.selection} onChange={e => onChange({ selection: e.target.value })} className={`${field} font-mono`} maxLength={2000} /></label>
        <label><span className={label}>Print button / click</span>
          <select value={d.click} onChange={e => onChange({ click: e.target.value })} className={`${select} w-full`}>
            <option value="">Select…</option>
            {clicks.filter(([c]) => c).map(([c, n]) => <option key={c} value={c}>{n}</option>)}
          </select></label>
        <label><span className={label}>Ctrl + click</span>
          <select value={d.ctrl_click} onChange={e => onChange({ ctrl_click: e.target.value })} className={`${select} w-full`}>
            {clicks.map(([c, n]) => <option key={c} value={c}>{n}</option>)}
          </select></label>
        <label><span className={label}>Ctrl + Alt + click</span>
          <select value={d.ctrl_alt_click} onChange={e => onChange({ ctrl_alt_click: e.target.value })} className={`${select} w-full`}>
            {clicks.map(([c, n]) => <option key={c} value={c}>{n}</option>)}
          </select></label>
        <label><span className={label}>After saving the document</span>
          <select value={d.after_save} onChange={e => onChange({ after_save: e.target.value })} className={`${select} w-full`}>
            {afterSave.map(([c, n]) => <option key={c} value={c}>{n}</option>)}
          </select></label>
        <label><span className={label}>Printer (direct print)</span>
          <input list={listId} value={d.printer} onChange={e => onChange({ printer: e.target.value })} className={field} maxLength={50} placeholder="Windows default" />
          <datalist id={listId}>{printerList}</datalist></label>
        <label><span className={label}>Copies</span>
          <input type="number" min={1} max={20} value={d.copies} onChange={e => onChange({ copies: Math.trunc(Number(e.target.value)) })} className={field} /></label>
        <div className="flex items-end gap-4 pb-1.5">
          <label className="flex items-center gap-2 text-xs"><input type="checkbox" checked={d.default} onChange={e => onChange({ default: e.target.checked })} />Default</label>
          <label className="flex items-center gap-2 text-xs"><input type="checkbox" checked={!d.inactive} onChange={e => onChange({ inactive: !e.target.checked })} />In use</label>
        </div>
      </div>

      {hasEmail && (
        <fieldset className="mt-3 rounded-lg border border-[#E3D9CD] p-3 dark:border-[#2A2A2A]">
          <legend className="px-1 text-xs font-semibold">E-mail details</legend>
          <div className="grid grid-cols-1 gap-3 sm:grid-cols-2">
            <div className="flex items-center gap-4 text-xs sm:col-span-2">
              <label className="flex items-center gap-1.5"><input type="radio" name={`${listId}-route`} checked={d.email_routing === 'O'} onChange={() => onChange({ email_routing: 'O' })} />Outlook</label>
              <label className="flex items-center gap-1.5"><input type="radio" name={`${listId}-route`} checked={d.email_routing !== 'O'} onChange={() => onChange({ email_routing: 'S' })} />SMTP</label>
              <label className="flex items-center gap-1.5"><input type="checkbox" checked={d.attach_copy} onChange={e => onChange({ attach_copy: e.target.checked })} />Do not attach a copy</label>
            </div>
            <label><span className={label}>Subject</span><input value={d.email_subject} onChange={e => onChange({ email_subject: e.target.value })} className={field} maxLength={500} /></label>
            <label><span className={label}>Condition</span><input value={d.email_cond} onChange={e => onChange({ email_cond: e.target.value })} className={`${field} font-mono`} maxLength={1000} /></label>
            <label><span className={label}>Body</span><textarea value={d.email_body} onChange={e => onChange({ email_body: e.target.value })} className={`${field} h-20`} maxLength={4000} /></label>
            <label><span className={label}>Footer</span><textarea value={d.email_footer} onChange={e => onChange({ email_footer: e.target.value })} className={`${field} h-20`} maxLength={2000} /></label>
          </div>
          <table className="mt-3 w-full text-xs">
            <thead><tr className="text-left text-stone-500"><th className="py-1">Sl</th><th>Fixed</th><th>Type</th><th>E-mail ID</th><th>Document field</th><th>Condition</th><th /></tr></thead>
            <tbody>
              {d.emails.map((r, i) => (
                <tr key={i}>
                  <td className="py-1 pr-2">{i + 1}</td>
                  <td><input type="checkbox" aria-label={`Fixed e-mail ${i + 1}`} checked={r.fixed} onChange={e => setMail(i, { fixed: e.target.checked })} /></td>
                  <td><select aria-label={`E-mail type ${i + 1}`} value={r.type} onChange={e => setMail(i, { type: e.target.value })} className={select}>
                    <option value="T">To</option><option value="C">Cc</option><option value="B">Bcc</option></select></td>
                  <td><input aria-label={`E-mail ID ${i + 1}`} value={r.email} disabled={!r.fixed} onChange={e => setMail(i, { email: e.target.value })} className={field} maxLength={200} /></td>
                  <td><input aria-label={`Document field ${i + 1}`} value={r.field} disabled={r.fixed} onChange={e => setMail(i, { field: e.target.value })} className={field} maxLength={100} /></td>
                  <td><input aria-label={`Condition ${i + 1}`} value={r.cond} disabled={r.fixed} onChange={e => setMail(i, { cond: e.target.value })} className={`${field} font-mono`} maxLength={500} /></td>
                  <td><button type="button" aria-label={`Remove e-mail row ${i + 1}`} className="px-2 text-red-600" onClick={() => onChange({ emails: d.emails.filter((_, n) => n !== i) })}>✕</button></td>
                </tr>
              ))}
            </tbody>
          </table>
          <button type="button" className={`${secondaryButton} mt-2`} onClick={() => onChange({ emails: [...d.emails, blankEmail()] })}><Plus size={14} />Add e-mail row</button>
        </fieldset>
      )}

      {hasMulti && (
        <fieldset className="mt-3 rounded-lg border border-[#E3D9CD] p-3 dark:border-[#2A2A2A]">
          <legend className="px-1 text-xs font-semibold">Multi print details</legend>
          <div className="flex flex-wrap items-center gap-4 text-xs">
            <label className="flex items-center gap-1.5"><input type="checkbox" checked={d.allow_date_range} onChange={e => onChange({ allow_date_range: e.target.checked })} />Date range</label>
            <label className="flex items-center gap-1.5"><input type="checkbox" checked={d.allow_docno_range} onChange={e => onChange({ allow_docno_range: e.target.checked })} />Document number range</label>
            <label className="flex items-center gap-1.5"><input type="checkbox" checked={d.track_printed} onChange={e => onChange({ track_printed: e.target.checked })} />Track printed</label>
          </div>
          <label className="mt-2 block"><span className={label}>Query</span>
            <textarea value={d.multi_query} onChange={e => onChange({ multi_query: e.target.value })} className={`${field} h-20 font-mono`} maxLength={4000} /></label>
        </fieldset>
      )}
    </>
  );
}

export default function PrintSettings({ embedded = false }) {
  const [data, setData] = useState(null);
  const [docId, setDocId] = useState(null);
  const [drafts, setDrafts] = useState({});
  const [errors, setErrors] = useState({});
  const [printers, setPrinters] = useState([]);
  const [pcPrinters, setPcPrinters] = useState([]);   // the printers of the PC this page is open on (Print Agent), offered first
  const [saving, setSaving] = useState('');
  const [loading, setLoading] = useState(true);
  const [adding, setAdding] = useState(null);        // the draft of an option being added
  const [ordering, setOrdering] = useState(null);    // option names while Alter Order is on
  const { confirm, dialog } = useConfirm();

  const load = useCallback(() => getPrintSettings()
    .then(d => { setData(d); setDrafts({}); setErrors({}); setDocId(cur => cur ?? d.documents[0]?.doc_id ?? null); return true; })
    .catch(e => { toastStore.add('error', errorText(e)); return false; })
    .finally(() => setLoading(false)), []);

  useEffect(() => { load(); }, [load]);
  useEffect(() => { getPrintBridge().then(b => setPrinters(b.printers ?? [])).catch(() => { /* typing a printer name still works */ }); }, []);
  useEffect(() => { void detectAgent(true).then(a => setPcPrinters(a?.printers ?? [])); }, []);

  const doc = useMemo(() => data?.documents.find(d => d.doc_id === docId) ?? null, [data, docId]);
  const printerList = [...new Set([...pcPrinters, ...printers])].map(p => <option key={p} value={p} label={pcPrinters.includes(p) ? 'this PC' : 'server'} />);

  const edit = (o, patch) => {
    setDrafts(d => ({ ...d, [key(o)]: { ...(d[key(o)] ?? draftOf(o)), ...patch } }));
    setErrors(e => ({ ...e, [key(o)]: '' }));
  };
  const dirty = (o) => {
    const d = drafts[key(o)];
    return !!d && JSON.stringify(d) !== JSON.stringify(draftOf(o));
  };

  const save = async (o) => {
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

  const startAdd = () => setAdding({
    name: '', rpt: '', selection: '', copies: 1, default: false, inactive: false, click: '', ctrl_click: '', ctrl_alt_click: '', after_save: 'DNP',
    printer: '', email_routing: 'S', attach_copy: false, email_subject: '', email_body: '', email_footer: '', email_cond: '',
    allow_date_range: false, allow_docno_range: false, track_printed: false, multi_query: '', emails: [], error: '',
  });
  const saveAdd = async () => {
    const d = { ...adding };
    delete d.error;
    const bad = problem(d);
    if (bad) return setAdding({ ...adding, error: bad });
    if (!(await toastStore.defer('ok', `Print option ${d.name} will be added when this closes.`, 'Add cancelled. Nothing was changed.'))) return;
    try {
      await addPrintOption(docId, d);
      toastStore.add('ok', `Print option ${d.name} added.`);
      setAdding(null);
      await load();
    } catch (e) {
      setAdding({ ...adding, error: errorText(e) });
      toastStore.add('error', errorText(e));
    }
  };

  const remove = async (o) => {
    if (!(await confirm(`Are you sure you want to delete the print option "${o.name}"?`))) return;
    if (!(await toastStore.defer('delete', `Print option ${o.name} will be deleted when this closes.`, 'Delete cancelled. Nothing was changed.'))) return;
    try {
      await deletePrintOption(o.doc_id, o.order);
      toastStore.add('delete', 'Deleted Successfully!');
      await load();
    } catch (e) {
      toastStore.add('error', errorText(e));
    }
  };

  const move = (i, by) => setOrdering(list => {
    const next = [...list];
    const j = i + by;
    if (j < 0 || j >= next.length) return list;
    [next[i], next[j]] = [next[j], next[i]];
    return next;
  });
  const saveOrder = async () => {
    if (!(await toastStore.defer('ok', 'The new order will be saved when this closes.', 'Cancelled. The order is unchanged.'))) return;
    try {
      await orderPrintOptions(docId, ordering);
      toastStore.add('ok', 'Saved Successfully!');
      setOrdering(null);
      await load();
    } catch (e) {
      toastStore.add('error', errorText(e));
    }
  };

  return (
    <div className="mx-auto max-w-7xl">
      {dialog}
      <div className={`${pageHeader} mb-5`}>
        <div>
          {!embedded && <h1 className={pageTitle}>Print Settings</h1>}
          <p className={pageSub}>How each document prints: what Print does, what happens after a save, the printer and copies. The desktop reads the same settings.</p>
        </div>
        <button onClick={load} className={secondaryButton}><RefreshCw size={14} />Reload</button>
      </div>

      <div className={`${panel} mb-5 p-4`}><RptFolderSetting plain /></div>
      <div className={`${panel} mb-5 p-4`}><LocalPrinterSetting plain /></div>
      <div className={`${panel} mb-5 p-4`}><ServerPrinterSetting plain /></div>
      <div className={`${panel} mb-5 p-4`}><SmtpSetting plain /></div>

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
                <button key={d.doc_id} role="option" aria-selected={d.doc_id === docId} onClick={() => { setDocId(d.doc_id); setAdding(null); setOrdering(null); }}
                  className={`flex w-full items-center justify-between gap-2 rounded-lg border px-3 py-2 text-left text-xs ${d.doc_id === docId
                    ? 'border-[#64283A] bg-[#64283A]/5 dark:border-[#D4A847] dark:bg-[#D4A847]/10' : 'border-[#E3D9CD] dark:border-[#2A2A2A]'}`}>
                  <span className="truncate font-medium">{d.name}</span>
                  <span className={badge('neutral')}>{d.options.length}</span>
                </button>
              ))}
            </div>
          </div>

          <div className="min-w-0 flex-1 space-y-3">
            <div className="flex flex-wrap gap-2">
              <button type="button" className={secondaryButton} disabled={!doc || !!ordering} onClick={startAdd}><Plus size={14} />Add option</button>
              <button type="button" className={secondaryButton} disabled={!doc || (doc?.options.length ?? 0) < 2 || !!ordering}
                onClick={() => setOrdering(doc.options.map(o => o.name))}><ListOrdered size={14} />Alter Order</button>
            </div>

            {ordering && (
              <section className={`${panel} p-4`} aria-label="Alter Order">
                <p className="mb-2 text-xs text-stone-500">Move the options up or down, then Save.</p>
                <ol className="space-y-1">
                  {ordering.map((n, i) => (
                    <li key={n} className="flex items-center gap-2 rounded-md border border-[#E3D9CD] px-3 py-1.5 text-xs dark:border-[#2A2A2A]">
                      <span className="w-5 text-stone-400">{i + 1}</span><span className="flex-1 truncate">{n}</span>
                      <button type="button" aria-label={`Move ${n} up`} disabled={i === 0} onClick={() => move(i, -1)}><ArrowUp size={14} /></button>
                      <button type="button" aria-label={`Move ${n} down`} disabled={i === ordering.length - 1} onClick={() => move(i, 1)}><ArrowDown size={14} /></button>
                    </li>
                  ))}
                </ol>
                <div className="mt-3 flex gap-2">
                  <button type="button" className={primaryButton} onClick={() => void saveOrder()}><Save size={14} />Save</button>
                  <button type="button" className={secondaryButton} onClick={() => setOrdering(null)}>Cancel</button>
                </div>
              </section>
            )}

            {adding && (
              <section className={`${panel} p-4`} aria-label="New print option">
                <p className="mb-3 text-sm font-semibold">Print Option Name for {doc?.name}</p>
                <OptionForm d={adding} onChange={p => setAdding(a => ({ ...a, ...p, error: '' }))} data={data} printerList={printerList} listId="printers-new" />
                {adding.error && <InlineError message={adding.error} />}
                <div className="mt-3 flex gap-2">
                  <button type="button" className={primaryButton} onClick={() => void saveAdd()}><Save size={14} />Save</button>
                  <button type="button" className={secondaryButton} onClick={() => setAdding(null)}>Cancel</button>
                </div>
              </section>
            )}

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
                    <div className="flex gap-2">
                      <button onClick={() => remove(o)} className={secondaryButton} aria-label={`Delete ${o.name}`}><Trash2 size={14} />Delete</button>
                      <button onClick={() => save(o)} disabled={!dirty(o) || saving === key(o)} className={primaryButton}><Save size={14} />Save</button>
                    </div>
                  </div>
                  <OptionForm d={d} onChange={p => edit(o, p)} data={data} printerList={printerList} listId={`printers-${key(o)}`} />
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
