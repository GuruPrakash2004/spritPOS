// The records vLQMS.exe keeps beside a customer or supplier: contact persons, other / shipping addresses, alert messages (shown
// when the party is picked on a bill) and dated activity notes. One window with a tab for each; contacts, addresses and alerts
// are edited as a list and saved together, notes are only added. The lists come from GET /masters/{customers|suppliers}/{code}/{kind}.

import { useCallback, useEffect, useState } from 'react';
import { Plus, Trash2 } from 'lucide-react';
import { useConfirm } from '../common/ConfirmDialog';
import Modal from '../common/Modal';
import { toastStore } from '../common/toastStore';
import { addPartyNote, errorText, getPartyRecords, savePartyRecords } from '../../lib/api';
import { field, label, primaryButton, secondaryButton } from '../../lib/styles';

const TABS = [
  { id: 'contacts', title: 'Contact persons', captions: { CName: 'Name', CDesig: 'Designation', CMobNo: 'Mobile', CPhone: 'Phone', Fax: 'Fax', Email: 'E-mail', CDepartment: 'Department', CRemarks: 'Remarks', SetDefault: 'Default' } },
  { id: 'addresses', title: 'Addresses', captions: { AddCat: 'Category', Add1: 'Address 1', Add2: 'Address 2', Add3: 'Address 3', Add4: 'Address 4', City: 'City', PinCode: 'Pin', State: 'State', Country: 'Country', DefShipAdd: 'Default shipping' } },
  { id: 'alerts', title: 'Alert messages', captions: { Message: 'Message' } },
  { id: 'activities', title: 'Notes', captions: { Notes: 'Note' } },
];

export default function PartyRecords({ party, code, name, canEdit, onClose }) {
  const [tab, setTab] = useState('contacts');
  const [data, setData] = useState({ columns: [], rows: [] });
  const [note, setNote] = useState('');
  const [error, setError] = useState('');
  const { confirm, dialog: confirmDialog } = useConfirm();
  const spec = TABS.find(t => t.id === tab);

  const load = useCallback(() => getPartyRecords(party, code, tab).then(r => { setError(''); setData(r); }).catch(e => setError(errorText(e))), [party, code, tab]);
  useEffect(() => { load(); }, [load]);

  const edit = (i, key, value) => setData(d => ({ ...d, rows: d.rows.map((r, j) => (j === i ? { ...r, [key]: value } : r)) }));
  const add = () => setData(d => ({ ...d, rows: [...d.rows, Object.fromEntries(d.columns.map(c => [c.key, c.type === 'yn' ? false : '']))] }));
  const remove = async (i) => {
    if (!(await confirm('Are you sure you want to delete this row?'))) return;
    setData(d => ({ ...d, rows: d.rows.filter((_, j) => j !== i) }));
  };

  const save = async () => {
    setError('');
    if (!(await toastStore.defer('ok', `The ${spec.title.toLowerCase()} of ${name || code} will be saved when this closes.`, 'Save cancelled. Nothing was changed.'))) return;
    try { setData(await savePartyRecords(party, code, tab, data.rows)); } catch (e) { setError(errorText(e)); }
  };
  const addNote = async () => {
    if (!note.trim()) { setError('Enter the note'); return; }
    if (!(await toastStore.defer('ok', 'The note will be added when this closes.', 'Cancelled. No note was added.'))) return;
    try { setData(await addPartyNote(party, code, note)); setNote(''); setError(''); } catch (e) { setError(errorText(e)); }
  };

  return (
    <>
    {confirmDialog}
    <Modal title={`${name || code}: records`} onClose={onClose} wide footer={<button onClick={onClose} className={secondaryButton}>Close</button>}>
      <div role="tablist" className="mb-3 flex gap-1 border-b border-line">
        {TABS.map(t => (
          <button key={t.id} role="tab" aria-selected={tab === t.id} onClick={() => setTab(t.id)}
            className={`-mb-px border-b-2 px-3 py-2 text-xs font-medium ${tab === t.id ? 'border-accent text-stone-900 dark:text-stone-50' : 'border-transparent text-stone-500'}`}>{t.title}</button>
        ))}
      </div>
      {error && <p role="alert" className="mb-2 text-xs text-[#9A4B45] dark:text-red-400">{error}</p>}

      {tab === 'activities' ? (
        <div className="space-y-3">
          {canEdit && (
            <div className="flex gap-2">
              <input aria-label="New note" value={note} onChange={e => setNote(e.target.value)} maxLength={250} placeholder="Write a note about this party" className={field} />
              <button onClick={addNote} className={primaryButton}>Add</button>
            </div>
          )}
          <ul className="divide-y divide-line-soft text-xs">
            {data.rows.map((r, i) => <li key={i} className="py-2"><span className="tabular-nums text-stone-500">{r.ActDate}</span> · {r.Notes}</li>)}
            {!data.rows.length && <li className="py-3 text-stone-500">No notes yet.</li>}
          </ul>
        </div>
      ) : (
        <div className="space-y-3">
          <div className="overflow-x-auto">
            <table className="w-full text-xs">
              <thead><tr>{data.columns.map(c => <th key={c.key} className={`${label} px-1 text-left`}>{spec.captions[c.key] ?? c.key}</th>)}<th /></tr></thead>
              <tbody>
                {data.rows.map((r, i) => (
                  <tr key={i}>
                    {data.columns.map(c => (
                      <td key={c.key} className="px-1 py-1">
                        {c.type === 'yn'
                          ? <input type="checkbox" aria-label={spec.captions[c.key]} checked={!!r[c.key]} disabled={!canEdit} onChange={e => edit(i, c.key, e.target.checked)} className="h-4 w-4 accent-accent" />
                          : <input aria-label={spec.captions[c.key]} value={r[c.key] ?? ''} maxLength={c.width} readOnly={!canEdit} onChange={e => edit(i, c.key, e.target.value)} className={`${field} min-w-[7rem]`} />}
                      </td>
                    ))}
                    <td>{canEdit && <button onClick={() => remove(i)} aria-label="Remove row" className="text-stone-400 hover:text-red-500"><Trash2 size={14} /></button>}</td>
                  </tr>
                ))}
                {!data.rows.length && <tr><td className="px-1 py-3 text-stone-500" colSpan={data.columns.length + 1}>None yet.</td></tr>}
              </tbody>
            </table>
          </div>
          {canEdit && (
            <div className="flex gap-2">
              <button onClick={add} className={secondaryButton}><Plus size={14} />Add row</button>
              <button onClick={save} className={primaryButton}>Save</button>
            </div>
          )}
        </div>
      )}
    </Modal>
    </>
  );
}
