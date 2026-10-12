// One shared view of the Doc Designer's definitions for the whole app: the screen list, each screen's definition and the menu are
// read once and cached. NOTHING asks the server whether the designer changed on its own (no poll, no check when the window gets focus,
// no push stream: each of those cost the shop time while billing). The top bar's "Refresh from Doc Designer" button reads the designer
// again on the server (POST /designer/reload) and then calls forceDesignerReload(): the caches are dropped and every subscribed screen
// (useScreenFields, the menu, LookupSelect) reads its definition again and redraws, keeping what was typed. `changes` counts those
// moments; `buildChanged` is true when the backend reported a newer front-end build than the page was loaded with.

import { getDesignerDocs, getDesignerMenu, getScreenDef } from './api';

let snapshot = { version: '', changes: 0, buildChanged: false };
let baseBuild = '';
let docsPromise = null;
const defs = new Map();
let menuPromise = null;
const listeners = new Set();

const emit = (next) => { snapshot = { ...snapshot, ...next }; listeners.forEach(l => l()); };

export const getDesignerSnapshot = () => snapshot;

/** The list of the designer's screens (read once per structure version). */
export const loadDocs = () => (docsPromise ??= getDesignerDocs().then(d => (Array.isArray(d) ? d : [])).catch(() => { docsPromise = null; return []; }));

/** The desktop's menu (MstMenu), read once per structure version. */
export const loadMenu = () => (menuPromise ??= getDesignerMenu().then(m => (Array.isArray(m) ? m : [])).catch(() => { menuPromise = null; return []; }));

/** One screen's definition (read once per structure version). Rejects when it can't be read. */
export const loadDefinition = (docId) => {
  if (!defs.has(docId)) defs.set(docId, getScreenDef(docId).catch(e => { defs.delete(docId); throw e; }));
  return defs.get(docId);
};

/** Take a {version, build} answer (from the reload button): note a newer build, and when the structure
 *  version moved drop the caches and tell every subscribed screen. */
export function applyDesignerState({ version, build } = {}) {
  if (build) {
    if (!baseBuild) baseBuild = build;
    else if (build !== baseBuild && !snapshot.buildChanged) emit({ buildChanged: true });
  }
  if (!version) return;
  if (snapshot.version && version !== snapshot.version) {
    docsPromise = null;
    menuPromise = null;
    defs.clear();
    emit({ version, changes: snapshot.changes + 1 });
  } else if (!snapshot.version) {
    emit({ version });
  }
}

/** The top bar's "Refresh from Doc Designer": forget every cached list / definition / menu and tell every subscribed screen to read
 *  them again, whether or not the structure version moved (the server has just read the designer's tables again). */
export function forceDesignerReload(version = '', build = '') {
  if (build) applyDesignerState({ build });   // notes a newer front-end build
  docsPromise = null;
  menuPromise = null;
  defs.clear();
  emit({ version: version || snapshot.version, changes: snapshot.changes + 1 });
}

/** Subscribe to changes (for useSyncExternalStore). Subscribing asks the server nothing. The last subscriber leaving (sign-out)
 *  puts the store back to a clean state for the next sign-in. */
export function subscribeDesigner(listener) {
  listeners.add(listener);
  return () => {
    listeners.delete(listener);
    if (listeners.size === 0) {
      snapshot = { version: '', changes: 0, buildChanged: false };
      baseBuild = ''; docsPromise = null; menuPromise = null; defs.clear();
    }
  };
}
