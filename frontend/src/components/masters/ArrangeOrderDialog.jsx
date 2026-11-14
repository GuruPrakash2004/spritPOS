// Options → Arrange Group Slno / Arrange Pkg Slno (the exe's frmUtlGrpOrderBy / frmUtlPkgOrderBy): the master's rows in their saved order with a
// Slno column; Move Up / Move Down change the order, Save writes Slno 1, 2, 3 ... back (GroupOrderBy / PkgOrderBy). Packages also have Ascending /
// Descending, which list the packages by litres per bottle. Export to Excel as in the exe.

import { useCallback, useEffect, useState } from 'react';
import { ArrowDown, ArrowUp } from 'lucide-react';
import Modal from '../common/Modal';
import { toastStore } from '../common/toastStore';
import { downloadXls } from '../../lib/xlsExport';
import { errorText, getMasterOrder, saveMasterOrder } from '../../lib/api';
import { primaryButton, secondaryButton, tableBody, tableHead } from '../../lib/styles';

export default function ArrangeOrderDialog({ path, title, onClose }) {
  const [rows, setRows] = useState([]);
  const [at, setAt] = useState(0);
  const [message, setMessage] = useState('');
  const isPackage = path === 'packages';

  const load = useCallback((sort = '') => {
    getMasterOrder(path, sort).then(r => { setRows(r); setAt(0); setMessage(''); }).catch(e => setMessage(errorText(e)));
  }, [path]);
  useEffect(() => { void load(); }, [load]);

  const renumber = (list) => list.map((r, i) => ({ ...r, slno: i + 1 }));
  const move = (by) => {
    const to = at + by;
    if (to < 0 || to >= rows.length) return;
    const next = [...rows];
    [next[at], next[to]] = [next[to], next[at]];
    setRows(renumber(next));
    setAt(to);
  };
  const save = async () => {
    if (!(await toastStore.defer('ok', `The new ${title} order will be saved when this closes.`, 'Save cancelled. The order is unchanged.'))) return;
    saveMasterOrder(path, rows.map(r => r.id)).then(() => setMessage('Updated Successfully...')).catch(e => setMessage(errorText(e)));
  };
  const columns = [{ key: 'slno', label: 'Slno' }, { key: 'code', label: title }, ...(isPackage ? [{ key: 'litres', label: 'Ltr Per Bot' }] : [])];

  return (
    <Modal title={`Arrange ${title} Slno`} onClose={onClose}
      footer={<>
        <button className={secondaryButton} disabled={!rows.length} onClick={() => void downloadXls(`Arrange_${title.replace(/\W+/g, '_')}_Slno`, `Arrange ${title} Slno`, '', columns, rows)}>Export to Excel</button>
        {isPackage && <button className={secondaryButton} onClick={() => load('asc')}>Ascending</button>}
        {isPackage && <button className={secondaryButton} onClick={() => load('dsc')}>Descending</button>}
        <button className={secondaryButton} onClick={onClose}>Close</button>
        <button className={primaryButton} disabled={!rows.length} onClick={() => void save()}>Save</button>
      </>}>
      <div className="mb-2 flex gap-2">
        <button className={secondaryButton} onClick={() => move(-1)} disabled={at <= 0} title="Move up"><ArrowUp size={14} />Move Up</button>
        <button className={secondaryButton} onClick={() => move(1)} disabled={at >= rows.length - 1} title="Move down"><ArrowDown size={14} />Move Down</button>
        {message && <span role="status" className="self-center text-accent-ink">{message}</span>}
      </div>
      <div className="max-h-[50vh] overflow-auto">
        <table className="w-full">
          <thead className={tableHead}><tr>{columns.map(c => <th key={c.key} className="px-2 py-1.5 text-left">{c.label}</th>)}</tr></thead>
          <tbody className={tableBody}>
            {rows.map((r, i) => (
              <tr key={r.id} onClick={() => setAt(i)} className={`cursor-pointer ${i === at ? 'bg-accent/15' : ''}`}>
                {columns.map(c => <td key={c.key} className="px-2 py-1.5">{String(r[c.key] ?? '')}</td>)}
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </Modal>
  );
}
