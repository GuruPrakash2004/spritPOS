// SMS settings, as the desktop's two settings windows (Administrator only; the SMS screen shows them as tabs):
//  - Providers (frmUtlSendSMS): the shop's SMS gateways in UtlMsgSettings: Provider, Daily Limit, URL (with <<MOBNO>> and <<MSG>> in it) and
//    Default. New, edit (double-click), delete (asks first) and the Test SMS panel of a saved provider.
//  - Invoice SMS (frmUtlInvSMS): UtlMisc's invoice SMS switches (sales above a value, cancelled, deleted), the numbers that get them and the three texts.
// Saves wait for their popup; mistakes are shown inline with the desktop's wording.

import { useCallback, useEffect, useState } from 'react';
import { Plus, Save, Send, Trash2 } from 'lucide-react';
import { toastStore } from '../common/toastStore';
import { useConfirm } from '../common/ConfirmDialog';
import { InlineError } from '../common/FormErrorSummary';
import { deleteSmsProvider, errorText, getSmsInvoiceSettings, getSmsProviderRows, saveSmsInvoiceSettings, saveSmsProvider, testSmsProvider } from '../../lib/api';
import { field, label, panel, primaryButton, secondaryButton, tableBody, tableHead } from '../../lib/styles';

const blank = { id: 0, name: '', daily_limit: '', url: '', default: false };

export function ProvidersTab() {
  const [data, setData] = useState(null);
  const [form, setForm] = useState(null);       // the provider being added / edited, or null
  const [problem, setProblem] = useState('');
  const [mobile, setMobile] = useState('');
  const [busy, setBusy] = useState(false);
  const { confirm, dialog } = useConfirm();

  const load = useCallback(() => getSmsProviderRows().then(setData).catch(e => { setData({ providers: [] }); setProblem(errorText(e)); }), []);
  useEffect(() => { load(); }, [load]);

  const save = async () => {
    // The same checks, in the same order, as the desktop (the server repeats them).
    if (!form.name.trim()) return setProblem('Please enter Provider!');
    if (String(form.daily_limit).trim() === '') return setProblem('Please enter Daily Limit of messages!');
    if (!form.url.trim()) return setProblem('Please enter SMS URL!');
    setProblem('');
    if (!(await toastStore.defer('ok', `Provider ${form.name.trim()} will be saved when this closes.`, 'Save cancelled. Nothing was changed.'))) return;
    setBusy(true);
    try {
      await saveSmsProvider(form.id, { name: form.name, daily_limit: form.daily_limit, url: form.url, default: form.default });
      toastStore.add('ok', 'Updated Provider details successfully!');
      setForm(null);
      await load();
    } catch (e) {
      setProblem(errorText(e));
    } finally {
      setBusy(false);
    }
  };

  const remove = async (p) => {
    if (!(await confirm('Do you want to delete this provider settings?'))) return;
    if (!(await toastStore.defer('delete', `Provider ${p.name} will be deleted when this closes.`, 'Delete cancelled. Nothing was changed.'))) return;
    try {
      await deleteSmsProvider(p.id);
      toastStore.add('delete', `Provider ${p.name} deleted.`);
      await load();
    } catch (e) {
      setProblem(errorText(e));
    }
  };

  const test = async () => {
    setProblem('');
    if (!(await toastStore.defer('ok', 'The test message will be sent when this closes.', 'Cancelled. Nothing was sent.'))) return;
    try {
      const r = await testSmsProvider(form.id, mobile);
      if (r.sent) toastStore.add('ok', r.message);
      else setProblem(r.message);
    } catch (e) {
      setProblem(errorText(e));
    }
  };

  if (!data) return <p className="text-xs text-stone-500">Loading…</p>;
  const set = (patch) => { setForm({ ...form, ...patch }); setProblem(''); };

  return (
    <div className="space-y-4">
      {dialog}
      {!form && (
        <div className={`${panel} overflow-auto`}>
          <div className="flex items-center justify-between gap-2 border-b border-[#E3D9CD] px-4 py-2 dark:border-[#2A2A2A]">
            <p className="text-xs text-stone-500">Double-click a provider to change it. The default provider sends the messages.</p>
            <button className={primaryButton} onClick={() => { setForm({ ...blank }); setMobile(''); setProblem(''); }}><Plus size={14} />New</button>
          </div>
          <table className="w-full text-sm">
            <thead className={tableHead}><tr>
              <th className="px-3 py-2 text-left">Provider</th><th className="px-3 py-2 text-left">Default</th><th className="px-3 py-2 text-right">Daily Limit</th>
              <th className="px-3 py-2 text-left">URL</th><th className="w-10" />
            </tr></thead>
            <tbody className={tableBody}>
              {!data.providers.length && <tr><td colSpan={5} className="px-4 py-10 text-center text-stone-500">No SMS provider is set up.</td></tr>}
              {data.providers.map(p => (
                <tr key={p.id} className="cursor-pointer" onDoubleClick={() => { setForm({ ...p }); setMobile(''); setProblem(''); }}>
                  <td className="px-3 py-1.5 font-medium">{p.name}</td><td className="px-3 py-1.5">{p.default ? 'Y' : 'N'}</td>
                  <td className="px-3 py-1.5 text-right tabular-nums">{p.daily_limit}</td>
                  <td className="max-w-md truncate px-3 py-1.5 font-mono text-[11px]" title={p.url}>{p.url}</td>
                  <td className="px-2"><button type="button" aria-label={`Delete ${p.name}`} onClick={() => void remove(p)}><Trash2 size={14} /></button>
                    <button type="button" className="ml-2 text-xs underline" onClick={() => { setForm({ ...p }); setMobile(''); setProblem(''); }}>Edit</button></td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      )}

      {form && (
        <section className={`${panel} space-y-3 p-4`} aria-label="Provider">
          <h2 className="text-sm font-semibold">{form.id ? 'Change provider' : 'New provider'}</h2>
          <div className="grid gap-3 sm:grid-cols-2">
            <label><span className={label}>Provider</span><input value={form.name} onChange={e => set({ name: e.target.value })} className={field} maxLength={100} /></label>
            <label><span className={label}>Daily Limit</span><input inputMode="numeric" value={form.daily_limit} onChange={e => set({ daily_limit: e.target.value.replace(/[^0-9]/g, '') })} className={field} /></label>
            <label className="sm:col-span-2"><span className={label}>SMS URL (use &lt;&lt;MOBNO&gt;&gt; and &lt;&lt;MSG&gt;&gt;)</span>
              <input value={form.url} onChange={e => set({ url: e.target.value })} className={`${field} font-mono`} maxLength={1000} /></label>
            <label className="flex items-center gap-2 text-xs"><input type="checkbox" checked={form.default} onChange={e => set({ default: e.target.checked })} />Default</label>
          </div>
          {form.id > 0 && (
            <div className="flex flex-wrap items-end gap-2 rounded-lg border border-[#E3D9CD] p-3 dark:border-[#2A2A2A]">
              <label><span className={label}>Test SMS: mobile number</span>
                <input inputMode="numeric" value={mobile} onChange={e => { setMobile(e.target.value.replace(/[^0-9]/g, '')); setProblem(''); }} className={field} maxLength={10} /></label>
              <button type="button" className={secondaryButton} onClick={() => void test()}><Send size={14} />Test SMS</button>
              <p className="basis-full text-[11px] text-stone-500">Sends a test message through the saved settings of this provider (save changes first).</p>
            </div>
          )}
          {problem && <InlineError message={problem} />}
          <div className="flex gap-2">
            <button className={primaryButton} disabled={busy} onClick={() => void save()}><Save size={14} />Save</button>
            <button className={secondaryButton} onClick={() => { setForm(null); setProblem(''); }}>Close</button>
          </div>
        </section>
      )}
      {!form && problem && <InlineError message={problem} />}
    </div>
  );
}

export function InvoiceSmsTab() {
  const [draft, setDraft] = useState(null);
  const [problem, setProblem] = useState('');
  const [busy, setBusy] = useState(false);

  useEffect(() => { getSmsInvoiceSettings().then(setDraft).catch(e => setProblem(errorText(e))); }, []);
  if (!draft) return problem ? <InlineError message={problem} /> : <p className="text-xs text-stone-500">Loading…</p>;
  const set = (patch) => { setDraft({ ...draft, ...patch }); setProblem(''); };
  const msg = (id, text, key) => (
    <label className="block"><span className={label}>{text}</span>
      <textarea id={id} rows={2} maxLength={250} value={draft[key]} onChange={e => set({ [key]: e.target.value.replace(/'/g, '`') })} className={field} /></label>
  );

  const save = async () => {
    if (!(await toastStore.defer('ok', 'Invoice SMS settings will be saved when this closes.', 'Save cancelled. Nothing was changed.'))) return;
    setBusy(true);
    try {
      setDraft(await saveSmsInvoiceSettings(draft));
      toastStore.add('ok', 'Saved Successfully.');
    } catch (e) {
      setProblem(errorText(e));
    } finally {
      setBusy(false);
    }
  };

  return (
    <section className={`${panel} space-y-4 p-4`} aria-label="Invoice SMS">
      {!draft.available && <p className="text-xs text-amber-700">This database has no invoice SMS settings, so nothing can be saved.</p>}
      <p className="text-xs text-stone-500">Messages to the shop's own numbers about its sales invoices. Put &lt;&lt;FIELD&gt;&gt; names of the invoice (for example &lt;&lt;SINVNO&gt;&gt;) in the texts. A cancelled or deleted invoice sends its message at once; as in the desktop app, the sales message is stored but never sent.</p>
      <div className="flex flex-wrap gap-6 text-xs">
        <label className="flex items-center gap-2"><input type="checkbox" checked={draft.on_sales} onChange={e => set({ on_sales: e.target.checked })} />For sales value above</label>
        <label className="flex items-center gap-2"><input type="checkbox" checked={draft.on_cancel} onChange={e => set({ on_cancel: e.target.checked })} />For cancellation</label>
        <label className="flex items-center gap-2"><input type="checkbox" checked={draft.on_delete} onChange={e => set({ on_delete: e.target.checked })} />For deletion</label>
      </div>
      <div className="grid gap-3 sm:grid-cols-2">
        <label><span className={label}>Sales value</span>
          <input type="number" min={0} value={draft.sales_value} onChange={e => set({ sales_value: Number(e.target.value) })} className={field} /></label>
        <label><span className={label}>Mobile numbers (comma separated)</span>
          <input value={draft.mobiles} onChange={e => set({ mobiles: e.target.value.replace(/[^0-9,;\s]/g, '') })} className={field} maxLength={300} /></label>
      </div>
      {msg('inv-sales-msg', 'Sales message', 'sales_msg')}
      {msg('inv-cancel-msg', 'Cancellation message', 'cancel_msg')}
      {msg('inv-delete-msg', 'Deletion message', 'delete_msg')}
      {problem && <InlineError message={problem} />}
      <button className={primaryButton} disabled={busy || !draft.available} onClick={() => void save()}><Save size={14} />Save</button>
    </section>
  );
}
