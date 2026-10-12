// Top bar button (Administrator): "Refresh from Doc Designer". Nothing is read from the Doc Designer again on its own (no check at page
// load, none after printing, no poll: each made billing slower); press this after changing the designer and every open screen, the menu and the
// print options take the new settings. Reads the designer's tables, the tables' column lists and the masters' extra columns on the server
// (POST /designer/reload), then redraws the screens (lib/designerStore.js forceDesignerReload).

import { useState } from 'react';
import { RefreshCw } from 'lucide-react';
import { errorText, reloadFromDocDesigner } from '../../lib/api';
import { forceDesignerReload } from '../../lib/designerStore';
import { toastStore } from '../common/toastStore';
import { iconButton } from '../../lib/styles';

export default function DesignerRefreshButton() {
  const [busy, setBusy] = useState(false);

  const refresh = async () => {
    if (busy) return;
    if (!(await toastStore.defer('refresh', 'The Doc Designer settings will be read again when this closes.', 'Cancelled. Nothing was refreshed.'))) return;
    setBusy(true);
    try {
      const state = await reloadFromDocDesigner();
      forceDesignerReload(state?.version, state?.build);
      toastStore.add('refresh', 'Refreshed from the Doc Designer.');
    } catch (e) {
      toastStore.add('error', errorText(e));
    } finally {
      setBusy(false);
    }
  };

  return (
    <button onClick={refresh} disabled={busy} className={`${iconButton} print:hidden`} title="Refresh from Doc Designer" aria-label="Refresh from Doc Designer">
      <RefreshCw size={16} className={busy ? 'animate-spin' : ''} />
    </button>
  );
}
