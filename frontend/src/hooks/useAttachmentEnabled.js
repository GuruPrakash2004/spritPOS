// useAttachmentEnabled(path): whether a master shows its File Attachment button. Global Settings → "File Attachment" has one Y/N row per master
// (AttachFile_<path>, e.g. AttachFile_item_groups; backend app/attachment_masters.py); only an N hides it. The values come from
// GET /settings/values (one read shared by every master screen, read again every minute and when the window regains focus).
// True until the values are known, so the button does not flicker away on a shop that keeps everything on.

import { useEffect, useState } from 'react';
import { getSettingValues } from '../lib/api';

const CACHE_MS = 60_000;
let values = null;
let loadedAt = 0;
let inflight = null;

const settingCode = (path) => `AttachFile_${String(path).replace(/-/g, '_')}`;

function load(force = false) {
  if (!force && values && Date.now() - loadedAt < CACHE_MS) return Promise.resolve(values);
  inflight ??= getSettingValues()
    .then(v => { values = v && typeof v === 'object' && !Array.isArray(v) ? v : {}; loadedAt = Date.now(); return values; })
    .catch(() => values ?? {})
    .finally(() => { inflight = null; });
  return inflight;
}

export const attachmentEnabledIn = (vals, path) => String(vals?.[settingCode(path)] ?? 'Y').trim().toUpperCase() !== 'N';

export function useAttachmentEnabled(path) {
  const [on, setOn] = useState(() => attachmentEnabledIn(values, path));
  useEffect(() => {
    let live = true;
    const refresh = (force) => load(force).then(v => { if (live) setOn(attachmentEnabledIn(v, path)); });
    refresh(false);
    const onFocus = () => refresh(true);
    window.addEventListener('focus', onFocus);
    return () => { live = false; window.removeEventListener('focus', onFocus); };
  }, [path]);
  return on;
}

/** For tests: forget the shared read. */
export function resetAttachmentSettings() { values = null; loadedAt = 0; inflight = null; }
