// Quiet, non-blocking notices about the database's structure (not a snackbar: nothing waits on them):
// "Screens updated from the Doc Designer" for a few seconds when the layout changed and open screens redrew, and a
// persistent "new version" bar when the backend now serves a newer front-end build (a click reloads; the page never
// reloads itself, so nobody loses a bill in progress).

import { useEffect, useState, useSyncExternalStore } from 'react';
import { RefreshCw } from 'lucide-react';
import { getDesignerSnapshot, subscribeDesigner } from '../../lib/designerStore';

export default function DesignerNotice() {
  const { changes, buildChanged } = useSyncExternalStore(subscribeDesigner, getDesignerSnapshot);
  const [shownFor, setShownFor] = useState(0);
  useEffect(() => {
    if (!changes) return undefined;
    const open = setTimeout(() => setShownFor(changes), 0);
    const close = setTimeout(() => setShownFor(0), 5000);
    return () => { clearTimeout(open); clearTimeout(close); };
  }, [changes]);

  return (
    <div className="pointer-events-none fixed bottom-12 left-4 z-[900] flex max-w-[calc(100vw-2rem)] flex-col gap-2">
      {shownFor > 0 && (
        <div role="status" className="rounded-lg border border-line bg-surface px-3 py-2 text-xs text-stone-700 shadow-lg dark:text-stone-200">
          Screens updated from the Doc Designer.
        </div>
      )}
      {buildChanged && (
        <div role="status" className="pointer-events-auto flex items-center gap-3 rounded-lg border border-line bg-surface px-3 py-2 text-xs text-stone-700 shadow-lg dark:text-stone-200">
          A new version of the app is ready.
          <button type="button" onClick={() => window.location.reload()} className="inline-flex items-center gap-1 font-semibold text-accent-ink hover:underline">
            <RefreshCw size={12} />Reload
          </button>
        </div>
      )}
    </div>
  );
}
