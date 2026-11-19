// Item Master Options menu dialog of the exe's frmMstItem: "Items-DataLink Problem" (frmRPtInvalidItems: the database view of items whose links
// point at nothing, exportable). Update Item Master Fields is its own page now: components/utilities/ImportItemMasterFields.jsx.

import { useEffect, useState } from 'react';
import Modal from '../common/Modal';
import { downloadXls } from '../../lib/xlsExport';
import { errorText, getInvalidItems } from '../../lib/api';
import { primaryButton, secondaryButton, tableBody, tableHead } from '../../lib/styles';

export function DataLinkProblemDialog({ onClose }) {
  const [data, setData] = useState(null);
  const [error, setError] = useState('');
  useEffect(() => { getInvalidItems().then(setData).catch(e => setError(errorText(e))); }, []);
  const columns = (data?.columns ?? []).map(c => ({ key: c, label: c }));
  return (
    <Modal title="Items-DataLink Problem" onClose={onClose} wide
      footer={<>
        <button className={secondaryButton} disabled={!data?.rows?.length} onClick={() => void downloadXls('Items_DataLink_Problem', 'Items-DataLink Problem', '', columns, data.rows)}>Export to Excel</button>
        <button className={primaryButton} onClick={onClose}>Close</button>
      </>}>
      {error && <p className="text-red-600 dark:text-red-400">{error}</p>}
      {!data && !error && <p>Loading…</p>}
      {data && !data.rows.length && <p>{data.note || 'No items with a data-link problem.'}</p>}
      {!!data?.rows?.length && (
        <table className="w-full">
          <thead className={tableHead}><tr>{columns.map(c => <th key={c.key} className="px-2 py-1.5 text-left">{c.label}</th>)}</tr></thead>
          <tbody className={tableBody}>
            {data.rows.map((r, i) => <tr key={i}>{columns.map(c => <td key={c.key} className="px-2 py-1.5">{String(r[c.key] ?? '')}</td>)}</tr>)}
          </tbody>
        </table>
      )}
    </Modal>
  );
}
