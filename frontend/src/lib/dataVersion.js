// "The shop's master data changed outside this app" signal. The backend (app/master_watch.py) reloads its masters when someone changes
// them in SQL Server or the Windows vLQMS and sends its counter in the X-Data-Version header of every reply; lib/api.js passes each
// value to noteDataVersion(). When it differs from the one seen before, `snapshot` goes up and the screens that show master data read
// it again: they add useDataVersion() to the dependencies of the effect that loads their rows, never replacing a record being typed.

import { useSyncExternalStore } from 'react';

let seen = null;
let snapshot = 0;
const listeners = new Set();

/** Called with the X-Data-Version header of a reply (null/undefined when the server sent none). */
export function noteDataVersion(value) {
  if (value === null || value === undefined || value === '') return;
  if (seen !== null && seen !== value) { snapshot += 1; listeners.forEach(l => l()); }
  seen = value;
}

export const subscribeDataVersion = (cb) => { listeners.add(cb); return () => listeners.delete(cb); };
export const getDataVersion = () => snapshot;

/** Re-renders when the master data changed outside the app; put the number in an effect's dependencies to read again. */
export const useDataVersion = () => useSyncExternalStore(subscribeDataVersion, getDataVersion);
