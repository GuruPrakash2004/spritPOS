// One shared view of the database's structure for the whole app, like the desktop reading the Doc Designer at start-up,
// but live: ONE poll (every 30 s and when the window gets focus, only while something is subscribed) asks
// GET /designer/version. When it moves, the cached screen list and definitions are dropped and every subscribed screen
// (useScreenFields) reads them again and redraws, keeping what was typed. `changes` counts those moments; `buildChanged`
// is true when the backend now serves a newer front-end build than the page was loaded with (the page offers a reload,
// it never reloads itself). The page also listens to the server's push stream (/designer/events) so a change shows
// within a second or two; the 30-second poll stays as the fallback.

import { getDesignerDocs, getDesignerMenu, getSchemaVersion, getScreenDef, streamDesignerEvents } from './api';

const POLL_MS = 30000;
let snapshot = { version: '', changes: 0, buildChanged: false };
let baseBuild = '';
let docsPromise = null;
const defs = new Map();
let menuPromise = null;
const listeners = new Set();
let timer = null;
let onFocus = null;
let generation = 0;   // bumped when the last subscriber leaves: a poll still in flight then is ignored

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

/** Take a {version, build} answer (from the poll or the push stream): note a newer build, and when the structure
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

export async function checkDesignerVersion() {
  const mine = generation;
  try {
    const state = await getSchemaVersion();
    if (mine !== generation) return;   // signed out meanwhile
    applyDesignerState(state);
  } catch { /* offline or signed out: try again later */ }
}

// The push stream: the server says the moment the structure changes, so screens redraw at once. It is reopened after a
// break (a few seconds later), and the 30-second check above keeps working whatever happens to it.
let stream = null;
function openStream() {
  const mine = generation;
  const controller = new AbortController();
  stream = controller;
  streamDesignerEvents(state => { if (mine === generation) applyDesignerState(state); }, controller.signal)
    .catch(() => undefined)
    .finally(() => { if (mine === generation && listeners.size) setTimeout(() => { if (mine === generation) openStream(); }, 5000); });
}

/** Subscribe to changes (for useSyncExternalStore). The first subscriber starts the poll, the last one stops it. */
export function subscribeDesigner(listener) {
  listeners.add(listener);
  if (listeners.size === 1) {
    checkDesignerVersion();
    timer = setInterval(checkDesignerVersion, POLL_MS);
    onFocus = () => { if (document.visibilityState !== 'hidden') checkDesignerVersion(); };
    window.addEventListener('focus', onFocus);
    if (typeof ReadableStream !== 'undefined') openStream();
  }
  return () => {
    listeners.delete(listener);
    if (listeners.size === 0) {
      generation += 1;
      stream?.abort();
      stream = null;
      clearInterval(timer);
      window.removeEventListener('focus', onFocus);
      snapshot = { version: '', changes: 0, buildChanged: false };   // the next sign-in starts from a clean baseline
      baseBuild = ''; docsPromise = null; menuPromise = null; defs.clear();
    }
  };
}
