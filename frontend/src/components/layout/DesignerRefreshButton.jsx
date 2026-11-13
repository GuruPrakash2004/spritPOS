// Top bar button (Administrator): "Refresh from Doc Designer". Nothing is read from the Doc Designer again on its own (no check at page
// load, none after printing, no poll: each made billing slower); press this after changing the designer and every open screen, the menu and the
// print options take the new settings. Reads the designer's tables, the tables' column lists and the masters' extra columns on the server
// (POST /designer/reload), then redraws the screens (lib/designerStore.js forceDesignerReload).

import { useState } from 'react';
import { RefreshCw } from 'lucide-react';
import { runDesignerRefresh } from './designerRefresh';
import { iconButton } from '../../lib/styles';

export default function DesignerRefreshButton() {
  const [busy, setBusy] = useState(false);

  const refresh = async () => {
    if (busy) return;
    setBusy(true);
    try { await runDesignerRefresh(); } finally { setBusy(false); }
  };

  return (
    <button onClick={refresh} disabled={busy} className={`${iconButton} print:hidden`} title="Refresh from Doc Designer" aria-label="Refresh from Doc Designer">
      <RefreshCw size={16} className={busy ? 'animate-spin' : ''} />
    </button>
  );
}
