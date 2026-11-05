// File Attachment on a master record (the desktop's "File Attachment (n)" button, frmUtlFileControl): the files kept for one item, customer,
// supplier, ... Add picks a file (PDF, Office, text or image, 8 MB at most); each file can be opened, saved or removed. Files are kept by the
// web app on its server (not in the desktop's shared folder), so they are seen from the web.
//
//   <AttachmentsDialog master="items" code="WH750" name="Whisky 750" canEdit onClose={() => …} />

import { useCallback, useEffect, useRef, useState } from 'react';
import { Download, ExternalLink, Paperclip, Trash2 } from 'lucide-react';
import Modal from '../common/Modal';
import { useConfirm } from '../common/ConfirmDialog';
import { toastStore } from '../common/toastStore';
import { deleteAttachment, errorText, fetchAttachment, listAttachments, uploadAttachment } from '../../lib/api';
import { field, primaryButton, secondaryButton } from '../../lib/styles';

const kb = (n) => (n >= 1048576 ? `${(n / 1048576).toFixed(1)} MB` : `${Math.max(1, Math.round(n / 1024))} KB`);

const toBase64 = (file) => new Promise((resolve, reject) => {
  const r = new FileReader();
  r.onload = () => resolve(String(r.result).split(',')[1] ?? '');
  r.onerror = () => reject(new Error('The file could not be read.'));
  r.readAsDataURL(file);
});

export default function AttachmentsDialog({ master, code, name, canEdit, onClose }) {
  const [files, setFiles] = useState(null);
  const [error, setError] = useState('');
  const [remarks, setRemarks] = useState('');
  const picker = useRef(null);
  const { confirm, dialog: confirmDialog } = useConfirm();

  const load = useCallback(() => listAttachments(master, code).then(l => { setFiles(l); setError(''); }).catch(e => { setFiles([]); setError(errorText(e)); }), [master, code]);
  useEffect(() => { void load(); }, [load]);

  const add = async (e) => {
    const file = e.target.files?.[0];
    e.target.value = '';
    if (!file) return;
    if (file.size > 8 * 1024 * 1024) return setError('The file is larger than 8 MB.');
    try {
      await uploadAttachment(master, code, { file_name: file.name, data: await toBase64(file), remarks });
      setRemarks('');
      toastStore.add('ok', `${file.name} attached.`);
      await load();
    } catch (err) { setError(errorText(err)); }
  };

  const open = async (f, download) => {
    try {
      const blob = await fetchAttachment(master, f.id, download);
      const url = URL.createObjectURL(blob);
      if (download) {
        const a = document.createElement('a');
        a.href = url; a.download = f.file_name; a.click();
      } else window.open(url, '_blank', 'noopener');
      setTimeout(() => URL.revokeObjectURL(url), 60_000);
    } catch (err) { setError(errorText(err)); }
  };

  const remove = async (f) => {
    if (!(await confirm(`Remove ${f.file_name}?`))) return;
    try { await deleteAttachment(master, f.id); await load(); } catch (err) { setError(errorText(err)); }
  };

  return (
    <Modal title={`File Attachment: ${name || code}`} onClose={onClose} wide footer={<button onClick={onClose} className={secondaryButton}>Close</button>}>
      {confirmDialog}
      {canEdit && (
        <div className="mb-3 flex flex-wrap items-center gap-2">
          <input value={remarks} onChange={e => setRemarks(e.target.value)} placeholder="Remarks for the next file (optional)" aria-label="Remarks" maxLength={200} className={`${field} flex-1`} />
          <input ref={picker} type="file" hidden onChange={e => void add(e)} accept=".pdf,.xlsx,.xls,.docx,.doc,.csv,.txt,.jpg,.jpeg,.png,.webp,.gif" />
          <button onClick={() => picker.current?.click()} className={primaryButton}><Paperclip size={14} />Add file</button>
        </div>
      )}
      {error && <p role="alert" className="mb-2 text-sm text-red-600">{error}</p>}
      {files === null ? <p className="text-sm text-stone-500">Loading…</p> : !files.length ? <p className="py-6 text-center text-sm text-stone-500">No files attached.</p> : (
        <ul className="divide-y divide-line rounded-lg border border-line">
          {files.map(f => (
            <li key={f.id} className="flex items-center gap-3 px-3 py-2 text-sm">
              <span className="min-w-0 flex-1"><span className="block truncate font-medium">{f.file_name}</span><span className="block truncate text-xs text-stone-500">{kb(f.size)} · {f.added_by} · {f.added_on.replace('T', ' ')}{f.remarks ? ` · ${f.remarks}` : ''}</span></span>
              <button onClick={() => void open(f, false)} className={secondaryButton} aria-label={`Open ${f.file_name}`}><ExternalLink size={13} /></button>
              <button onClick={() => void open(f, true)} className={secondaryButton} aria-label={`Download ${f.file_name}`}><Download size={13} /></button>
              {canEdit && <button onClick={() => void remove(f)} className={`${secondaryButton} hover:!text-red-600`} aria-label={`Remove ${f.file_name}`}><Trash2 size={13} /></button>}
            </li>
          ))}
        </ul>
      )}
    </Modal>
  );
}
