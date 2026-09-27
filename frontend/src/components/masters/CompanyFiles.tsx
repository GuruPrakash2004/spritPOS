// Company Master → Images / Documents tabs. Files are uploaded as base64 to /company/files
// and stored by the backend in backend/uploads/company/ (8 MB limit). Upload/edit/delete are
// Administrator only.

import { useCallback, useEffect, useRef, useState } from 'react';
import { useConfirm } from '../common/ConfirmDialog';
import { Download, Edit3, Eye, FileSpreadsheet, ImagePlus, Save, Trash2, Upload } from 'lucide-react';
import Modal from '../common/Modal';
import Toast from '../common/Toast';
import { api, errorText } from '../../lib/api';
import { downloadText, toCsv } from '../../lib/csv';
import {  field, label, primaryButton, secondaryButton, tableBody, tableHead , exportButton } from '../../lib/styles';

// Company Master → Images / Documents, stored by the backend under backend/uploads/company.
export type CompanyFile = {
  id: string; kind: 'image' | 'document'; file_name: string; ext: string; size: number; type_label: string;
  title: string; group: string; remarks: string; show_on_pos: boolean; uploaded_by: string; uploaded_at: string;
};

const MAX_MB = 8;
const IMAGE_ACCEPT = '.jpg,.jpeg,.png,.webp,.gif';
const DOC_ACCEPT = '.pdf,.xlsx,.xls,.docx,.doc,.csv,.txt,.jpg,.jpeg,.png,.webp';
const DOC_GROUPS = ['Excise', 'Taxation', 'KSBCL', 'Licence', 'Bank', 'Other'];

const contentUrl = (f: CompanyFile, download = false) => `/api/company/files/${f.id}/content${download ? '?download=true' : ''}`;
const sizeText = (n: number) => (n < 1024 * 1024 ? `${Math.max(1, Math.round(n / 1024))} KB` : `${(n / 1024 / 1024).toFixed(1)} MB`);

// The API takes base64 JSON, so read the file as a data URL and keep only the payload.
const toBase64 = (file: File) => new Promise<string>((resolve, reject) => {
  const reader = new FileReader();
  reader.onload = () => resolve(String(reader.result).split(',')[1] ?? '');
  reader.onerror = () => reject(new Error('Could not read the file'));
  reader.readAsDataURL(file);
});

type Draft = { file?: File; preview?: string; editing?: CompanyFile; title: string; group: string; remarks: string; show_on_pos: boolean };

function useCompanyFiles(kind: 'image' | 'document') {
  const [files, setFiles] = useState<CompanyFile[] | null>(null);
  const [toast, setToast] = useState<{ kind: 'ok' | 'error'; text: string } | null>(null);
  const { confirm, dialog } = useConfirm();

  useEffect(() => {
    if (!toast) return;
    const timer = setTimeout(() => setToast(null), 3000);
    return () => clearTimeout(timer);
  }, [toast]);

  const reload = useCallback(() => {
    api<CompanyFile[]>(`/company/files?kind=${kind}`).then(setFiles).catch(e => { setFiles([]); setToast({ kind: 'error', text: errorText(e) }); });
  }, [kind]);

  useEffect(() => { reload(); }, [reload]);

  const save = async (d: Draft) => {
    if (d.editing) {
      await api(`/company/files/${d.editing.id}`, { method: 'PUT', body: { title: d.title, group: d.group, remarks: d.remarks, show_on_pos: d.show_on_pos } });
      setToast({ kind: 'ok', text: 'Details updated.' });
    } else if (d.file) {
      if (d.file.size > MAX_MB * 1024 * 1024) throw new Error(`File is larger than ${MAX_MB} MB.`);
      await api('/company/files', { method: 'POST', body: { kind, file_name: d.file.name, data: await toBase64(d.file), title: d.title, group: d.group, remarks: d.remarks } });
      setToast({ kind: 'ok', text: `${d.file.name} uploaded.` });
    }
    reload();
  };

  const remove = async (f: CompanyFile) => {
    if (!(await confirm(`Delete ${f.file_name}? This cannot be undone.`))) return;
    api(`/company/files/${f.id}`, { method: 'DELETE' })
      .then(() => { setToast({ kind: 'ok', text: `${f.file_name} deleted.` }); reload(); })
      .catch(e => setToast({ kind: 'error', text: errorText(e) }));
  };

  return { files, toast, setToast, save, remove, dialog };
}

// Upload / edit dialog shared by both tabs.
function FileDialog({ kind, draft, setDraft, onSave, onClose }: {
  kind: 'image' | 'document'; draft: Draft; setDraft: (d: Draft) => void; onSave: () => void; onClose: () => void;
}) {
  const [busy, setBusy] = useState(false);
  const isNew = !draft.editing;
  return (
    <Modal
      title={isNew ? (kind === 'image' ? 'Upload Image' : 'Add Document') : `Edit ${draft.editing?.file_name}`}
      onClose={onClose}
      footer={<>
        <button onClick={onClose} className={secondaryButton}>Cancel</button>
        <button disabled={busy} onClick={() => { setBusy(true); onSave(); }} className={primaryButton}>
          {isNew ? <Upload size={14} /> : <Save size={14} />}{busy ? 'Saving…' : isNew ? 'Upload' : 'Save'}
        </button>
      </>}
    >
      <div className="space-y-3">
        {draft.preview && <img src={draft.preview} alt="" className="max-h-48 w-full rounded-lg object-contain bg-stone-100" />}
        {draft.file && <p className="font-mono text-stone-500">{draft.file.name} • {sizeText(draft.file.size)}</p>}
        <div>
          <label htmlFor="cf-title" className={label}>{kind === 'image' ? 'Title' : 'Display name'}</label>
          <input id="cf-title" value={draft.title} onChange={e => setDraft({ ...draft, title: e.target.value })} className={`${field} w-full`} placeholder={kind === 'image' ? 'e.g. Main POS brand wall' : 'e.g. CL-2 licence 2026-27'} />
        </div>
        {kind === 'document' && (
          <div>
            <label htmlFor="cf-group" className={label}>Group</label>
            <input id="cf-group" list="cf-groups" value={draft.group} onChange={e => setDraft({ ...draft, group: e.target.value })} className={`${field} w-full`} />
            <datalist id="cf-groups">{DOC_GROUPS.map(g => <option key={g} value={g} />)}</datalist>
          </div>
        )}
        <div>
          <label htmlFor="cf-rmk" className={label}>{kind === 'image' ? 'Caption' : 'Remarks'}</label>
          <input id="cf-rmk" value={draft.remarks} onChange={e => setDraft({ ...draft, remarks: e.target.value })} className={`${field} w-full`} />
        </div>
        {kind === 'image' && !isNew && (
          <label className="flex items-center gap-2 font-semibold text-stone-700 dark:text-stone-200">
            <input type="checkbox" checked={draft.show_on_pos} onChange={e => setDraft({ ...draft, show_on_pos: e.target.checked })} className="accent-accent" />
            Show on POS screen
          </label>
        )}
      </div>
    </Modal>
  );
}

function useDraft(kind: 'image' | 'document', save: (d: Draft) => Promise<void>, onError: (text: string) => void) {
  const [draft, setDraft] = useState<Draft | null>(null);
  const input = useRef<HTMLInputElement>(null);

  const pick = (file: File | undefined) => {
    if (!file) return;
    const preview = kind === 'image' || file.type.startsWith('image/') ? URL.createObjectURL(file) : undefined;
    setDraft({ file, preview, title: file.name.replace(/\.[^.]+$/, ''), group: '', remarks: '', show_on_pos: true });
  };
  const edit = (f: CompanyFile) => setDraft({ editing: f, title: f.title, group: f.group, remarks: f.remarks, show_on_pos: f.show_on_pos });
  const close = () => { if (draft?.preview) URL.revokeObjectURL(draft.preview); setDraft(null); };
  const submit = () => {
    if (!draft) return;
    save(draft).then(close).catch(e => { onError(errorText(e)); close(); });
  };
  const fileInput = (
    <input ref={input} type="file" hidden accept={kind === 'image' ? IMAGE_ACCEPT : DOC_ACCEPT}
      onChange={e => { pick(e.target.files?.[0]); e.target.value = ''; }} />
  );
  return { draft, setDraft, edit, close, submit, openPicker: () => input.current?.click(), fileInput };
}

export function CompanyImages({ canEdit }: { canEdit: boolean }) {
  const { files, toast, setToast, save, remove, dialog } = useCompanyFiles('image');
  const d = useDraft('image', save, text => setToast({ kind: 'error', text }));

  return (
    <div className="space-y-4">
      {toast && <Toast kind={toast.kind} text={toast.text} />}
      {dialog}
      <div className="flex flex-wrap items-center justify-between gap-2">
        <h4 className="text-sm font-semibold text-slate-800 dark:text-slate-200">Store Showcase &amp; Beverage Visuals</h4>
        {canEdit && <button onClick={d.openPicker} className={primaryButton}><ImagePlus size={14} />Upload Image</button>}
        {d.fileInput}
      </div>
      {files === null && <p className="py-10 text-center text-xs text-stone-400">Loading…</p>}
      {files?.length === 0 && (
        <div className="rounded-xl border border-dashed border-line py-12 text-center text-xs text-stone-400">
          No images yet.{canEdit ? ' Upload the shop front, brand wall or licence display (JPG, PNG, WebP, GIF, up to 8 MB).' : ''}
        </div>
      )}
      <div className="grid grid-cols-1 gap-4 sm:grid-cols-2 lg:grid-cols-3">
        {files?.map(f => (
          <figure key={f.id} className="group relative h-52 overflow-hidden rounded-xl bg-stone-200 shadow-lg">
            <img src={contentUrl(f)} alt={f.title} loading="lazy" className="absolute inset-0 h-full w-full object-cover" />
            <div className="absolute inset-0 bg-linear-to-t from-black/75 via-black/10 to-transparent" />
            <figcaption className="absolute inset-x-0 bottom-0 p-4 text-white">
              <span className={`rounded-md px-2 py-0.5 text-[10px] font-bold ${f.show_on_pos ? 'bg-emerald-500/80' : 'bg-white/25'}`}>{f.show_on_pos ? 'On POS screen' : 'Hidden from POS'}</span>
              <h5 className="mt-1.5 truncate text-base font-bold">{f.title}</h5>
              {f.remarks && <p className="truncate text-xs text-white/80">{f.remarks}</p>}
            </figcaption>
            <div className="absolute right-2 top-2 flex gap-1 opacity-100 transition-opacity sm:opacity-0 sm:group-hover:opacity-100 sm:group-focus-within:opacity-100">
              <a href={contentUrl(f)} target="_blank" rel="noreferrer" className="rounded-lg bg-black/50 p-1.5 text-white hover:bg-black/70" aria-label={`View ${f.title}`}><Eye size={14} /></a>
              {canEdit && <button onClick={() => d.edit(f)} className="rounded-lg bg-black/50 p-1.5 text-white hover:bg-black/70" aria-label={`Edit ${f.title}`}><Edit3 size={14} /></button>}
              {canEdit && <button onClick={() => remove(f)} className="rounded-lg bg-red-600/80 p-1.5 text-white hover:bg-red-600" aria-label={`Delete ${f.title}`}><Trash2 size={14} /></button>}
            </div>
          </figure>
        ))}
      </div>
      {d.draft && <FileDialog kind="image" draft={d.draft} setDraft={d.setDraft} onSave={d.submit} onClose={d.close} />}
    </div>
  );
}

export function CompanyDocuments({ canEdit }: { canEdit: boolean }) {
  const { files, toast, setToast, save, remove, dialog } = useCompanyFiles('document');
  const d = useDraft('document', save, text => setToast({ kind: 'error', text }));

  const exportList = () => {
    if (!files?.length) return;
    downloadText('Company_Documents.csv', toCsv([
      ['Group', 'File Type', 'Name', 'File Name', 'Remarks', 'Size', 'Uploaded By', 'Uploaded At'],
      ...files.map(f => [f.group, f.type_label, f.title, f.file_name, f.remarks, sizeText(f.size), f.uploaded_by, f.uploaded_at]),
    ]));
  };

  return (
    <div className="space-y-4">
      {toast && <Toast kind={toast.kind} text={toast.text} />}
      {dialog}
      {d.fileInput}
      <div className="overflow-x-auto rounded-xl border border-slate-200 dark:border-slate-800">
        <table className="w-full min-w-[640px] text-left text-xs">
          <thead className={tableHead}>
            <tr>
              <th className="px-4 py-3">Group</th><th className="px-4 py-3">File Type</th><th className="px-4 py-3">Name</th>
              <th className="px-4 py-3">Remarks</th><th className="px-4 py-3">Uploaded</th><th className="px-4 py-3 text-right">Actions</th>
            </tr>
          </thead>
          <tbody className={tableBody}>
            {files === null && <tr><td colSpan={6} className="px-4 py-8 text-center text-stone-400">Loading…</td></tr>}
            {files?.length === 0 && <tr><td colSpan={6} className="px-4 py-8 text-center text-stone-400">No documents yet.{canEdit ? ' Add the excise licence, GST certificate or KSBCL permit (PDF, Excel, Word, CSV, images; up to 8 MB).' : ''}</td></tr>}
            {files?.map(f => (
              <tr key={f.id} className="hover:bg-slate-50 dark:hover:bg-slate-800/40">
                <td className="px-4 py-3 font-semibold text-slate-700 dark:text-slate-300">{f.group || '—'}</td>
                <td className="px-4 py-3 text-slate-500">{f.type_label}</td>
                <td className="px-4 py-3">
                  <a href={contentUrl(f)} target="_blank" rel="noreferrer" className="font-medium text-emerald-600 hover:underline dark:text-emerald-400">{f.title}</a>
                  <div className="font-mono text-[10px] text-stone-400">{f.file_name} • {sizeText(f.size)}</div>
                </td>
                <td className="px-4 py-3 text-slate-600 dark:text-slate-400">{f.remarks}</td>
                <td className="px-4 py-3 text-slate-500">{f.uploaded_at.slice(0, 10)}<div className="text-[10px]">{f.uploaded_by}</div></td>
                <td className="px-4 py-3">
                  <div className="flex justify-end gap-1">
                    <a href={contentUrl(f, true)} className="rounded p-1 text-slate-500 hover:text-emerald-600" aria-label={`Download ${f.file_name}`}><Download size={14} /></a>
                    {canEdit && <button onClick={() => d.edit(f)} className="rounded p-1 text-slate-500 hover:text-accent-ink" aria-label={`Edit ${f.file_name}`}><Edit3 size={14} /></button>}
                    {canEdit && <button onClick={() => remove(f)} className="rounded p-1 text-slate-500 hover:text-red-600" aria-label={`Delete ${f.file_name}`}><Trash2 size={14} /></button>}
                  </div>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
      <div className="flex flex-wrap justify-end gap-2 pt-2">
        {canEdit && <button onClick={d.openPicker} className={secondaryButton}><Upload size={14} />Add Document</button>}
        <button onClick={exportList} disabled={!files?.length} className={exportButton}><FileSpreadsheet size={14} />Export List (CSV)</button>
      </div>
      {!canEdit && <p className="text-right text-[11px] text-stone-400">Only an Administrator can add, edit or delete company files.</p>}
      {d.draft && <FileDialog kind="document" draft={d.draft} setDraft={d.setDraft} onSave={d.submit} onClose={d.close} />}
    </div>
  );
}
