// Item Master: MasterCrud for /masters/items plus the exe's Options menu. "Import Item from Excel" opens its own screen
// (components/utilities/ImportItemData.jsx, the exe's frmImportItemXLData).

import { useState } from 'react';
import MasterCrud from './MasterCrud';
import { toastStore } from '../common/toastStore';
import { errorText, recalculateItemRates } from '../../lib/api';
import { openModule } from '../../lib/nav';
import { useConfirm } from '../common/ConfirmDialog';
import { DataLinkProblemDialog } from './ItemOptionDialogs';
import { ITEM_FIELDS } from './masterConfigs';

export default function ItemMaster() {
  const [reloadKey, setReloadKey] = useState(0);
  const [message, setMessage] = useState('');
  const [dialog, setDialog] = useState('');   // 'datalink'
  const { confirm, dialog: confirmDialog } = useConfirm();

  // Options → Update Item Rates (frmMstItem.updateItemRates): recalculate every Item Master formula field for all items.
  const updateRates = async () => {
    if (!(await confirm('Are you sure you want to Update/Recalulate Rates?'))) return;
    if (!(await toastStore.defer('ok', 'Item rates will be recalculated when this closes.', 'Update cancelled. No rates were changed.'))) return;
    recalculateItemRates()
      .then(r => { setReloadKey(k => k + 1); setMessage(r.message); })
      .catch(e => setMessage(errorText(e)));
  };

  return (
    <>
      <MasterCrud
        title="Item Master"
        subtitle="Liquor and non-liquor items, packages, MRP, purchase rate and reorder levels."
        path="items"
        fields={ITEM_FIELDS}
        reloadKey={reloadKey}
        options={[   // the exe's Options menu, in its order
          { label: 'Items-DataLink Problem', onClick: () => setDialog('datalink') },
          { label: 'Import Item from Excel', onClick: () => openModule('Import Item Data From Excel') },
          { label: 'Update Stock from Excel', onClick: () => openModule('Update Item Stock') },
          { label: 'Item Vs Brand', onClick: () => openModule('Item Vs Brand') },
          { label: 'Update Item Master Fields', onClick: () => openModule('Import Item Master Fields') },
          { label: 'Update Item Rates', onClick: () => void updateRates() },
          { label: 'Print Barcode', onClick: () => openModule('Label Printing') },
        ]}
      />
      {confirmDialog}
      {dialog === 'datalink' && <DataLinkProblemDialog onClose={() => setDialog('')} />}
      {message && (
        <p role="status" className="mx-auto mt-3 max-w-6xl rounded-lg bg-accent/10 px-4 py-2 text-xs font-medium text-accent-ink">{message}</p>
      )}
    </>
  );
}
