// The extra fields of a master screen (or a document's header), read from the database and kept current. The screen
// list and the definitions come from lib/designerStore.js, which polls the structure version ONCE for the whole app
// (every 30 s and on window focus). When the database changed (a column added or dropped, a caption, order, flag
// edited) the definition is read again and `version` changes so the screen can reload its rows, keeping what was typed.
// Without a definition (offline, no designer tables, the master has no screen) there are simply no extra fields.

import { useEffect, useMemo, useState, useSyncExternalStore } from 'react';
import { extraFields } from '../lib/designerFields';
import { getDesignerSnapshot, loadDefinition, loadDocs, subscribeDesigner } from '../lib/designerStore';

/** `path` is a master's path ('items'), or 'table:TrnHdrSINV' for a document's header table. */
export function useScreenFields(path) {
  const { version, changes } = useSyncExternalStore(subscribeDesigner, getDesignerSnapshot);
  const [found, setFound] = useState({ path: '', def: null });
  const screen = found.path === path ? found.def : null;   // never another screen's definition while this one loads

  useEffect(() => {
    let live = true;
    (async () => {
      const docs = await loadDocs();
      const doc = docs.find(d => (path.startsWith('table:') ? d.header_table.toLowerCase() === path.slice(6).toLowerCase() : d.master_path === path));
      if (!doc) { if (live) setFound({ path, def: null }); return; }
      try {
        const def = await loadDefinition(doc.doc_id);
        if (live) setFound({ path, def });
      } catch { /* keep the last definition */ }
    })();
    return () => { live = false; };
  }, [path, changes]);

  // The same array while the definition is the same: MasterCrud's effects depend on its identity.
  const fields = useMemo(() => extraFields(screen), [screen]);
  return { fields, version: changes ? String(version) : '', screen };
}
