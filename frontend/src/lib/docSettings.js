// Global Settings → Other Settings as the screens use them (backend app/doc_settings.py):
// - useDocSettings(kind): print after save (none / prompt / preview), default print format, Copy From, and
//   whether the document date can be changed, for one document kind;
// - menu layout (Menu Sequence): modules hidden from the sidebar and module search, and shortcut codes.
// Both are fetched once per page load (GET /settings/documents, GET /settings/menu) and default to the
// desktop's defaults until they arrive; the server enforces the same rules.
import { useEffect, useState } from 'react';
import { getDocumentSettings, getMenuLayout } from './api';

 






export const DEFAULT_DOC_SETTINGS = {
  after_save: 'none', default_format: '', allow_duplicate_items: true, allow_edit_date: true, edit_days: 0,
  allow_copy_from: true, update_rate_from_pb: false, screen: '', prints: false, affects_stock: false,
};

let docs = null;
export const loadDocSettings = () => {
  docs ??= getDocumentSettings().catch(() => { docs = null; return {}; });
  return docs;
};
/** Forget the cached settings (after saving them in Global Settings). */
export const resetDocSettings = () => { docs = null; };

export function useDocSettings(kind) {
  const [s, setS] = useState(DEFAULT_DOC_SETTINGS);
  useEffect(() => {
    let live = true;
    loadDocSettings().then(all => { if (live && all?.[kind]) setS({ ...DEFAULT_DOC_SETTINGS, ...all[kind] }); });
    return () => { live = false; };
  }, [kind]);
  return s;
}

export const loadMenuLayout = () => getMenuLayout().catch(() => ({ hidden: [], codes: {} } ));
