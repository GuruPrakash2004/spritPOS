// Utilities → Web Sync (legacy frmModWebSync).
//
// Billing stays on this PC; Web Sync sends the day's summary (the desktop's DSR tables, fast-moving and
// minimum-stock lists, purchase indents) and the current stock to the hosted central site, where the owner
// sees every shop. The backend sends automatically every few minutes (backend/app/websync.py); this screen
// shows how the last push went and has Sync now for a chosen day. The central site and this shop's key are
// set in backend/.env only, so the screen shows the site's host and never the key.

import { useCallback, useEffect, useState } from 'react';
import { CloudUpload, RefreshCw } from 'lucide-react';
import { toastStore } from '../common/toastStore';
import { InlineError } from '../common/FormErrorSummary';
import { errorText, getWebSync, runWebSync, todayIso, type ApiWebSync } from '../../lib/api';
import { badge, field, fieldError, label, pageHeader, pageSub, pageTitle, panel, primaryButton, secondaryButton } from '../../lib/styles';

const today = todayIso;
const when = (iso: string | null) => (iso ? new Date(iso).toLocaleString() : 'Never');

export default function WebSync() {
  const [status, setStatus] = useState<ApiWebSync | null>(null);
  const [day, setDay] = useState(today);
  const [dayError, setDayError] = useState('');
  const [busy, setBusy] = useState(false);

  const load = useCallback(() =>
    getWebSync().then(s => { setStatus(s); return true; })
      .catch(e => { toastStore.add('error', errorText(e)); return false; }), []);

  useEffect(() => { load(); }, [load]);

  const refresh = async () => {
    if (!(await toastStore.defer('refresh', 'The sync status will be refreshed when this closes.', 'Refresh cancelled.'))) return;
    if (await load()) toastStore.add('refresh', 'Sync status refreshed.');
  };

  const syncNow = async () => {
    if (!day || day > today()) {
      setDayError(!day ? 'Pick a day' : 'Choose today or an earlier day');
      document.getElementById('ws-day')?.focus();
      return;
    }
    setDayError('');
    const body = { day };
    if (!(await toastStore.defer('ok', `${day} and the stock will be sent to the central site when this closes.`, 'Sync cancelled. Nothing was sent.'))) return;
    setBusy(true);
    try {
      setStatus(await runWebSync(body));
      toastStore.add('ok', `Sent ${day} and the stock to the central site.`);
    } catch (e) {
      toastStore.add('error', errorText(e));
      await load();
    } finally {
      setBusy(false);
    }
  };

  const off = status !== null && !status.configured;
  return (
    <div className="mx-auto flex max-w-3xl flex-col gap-4">
      <div className={pageHeader}>
        <div>
          <h1 className={pageTitle}>Web Sync</h1>
          <p className={pageSub}>Sends the day's summary and the current stock to the central site, where the owner sees every shop. Billing never waits for it.</p>
        </div>
        <button className={secondaryButton} onClick={refresh}><RefreshCw size={14} />Refresh</button>
      </div>

      <section className={`${panel} grid gap-3 p-4 text-xs sm:grid-cols-2`} aria-label="Sync status">
        <div>
          <p className={label}>Central site</p>
          {status === null ? <p>Loading…</p>
            : off ? <span className={badge('warning')}>Not set up</span>
            : <p className="font-medium text-stone-800 dark:text-stone-100">{status.central} <span className="text-stone-500">· every {status.every_minutes} min</span></p>}
        </div>
        <div>
          <p className={label}>Last sent</p>
          <p>{when(status?.last_ok ?? null)}{status?.last_day ? ` (day ${status.last_day})` : ''}</p>
        </div>
        <div>
          <p className={label}>Stock last sent</p>
          <p>{when(status?.last_stock_ok ?? null)}</p>
        </div>
        <div>
          <p className={label}>Last problem</p>
          {status?.last_error
            ? <p className="text-red-700 dark:text-red-300">{status.last_error} <span className="text-stone-500">({when(status.last_error_at)})</span></p>
            : <span className={badge('success')}>None</span>}
        </div>
        {off && (
          <p className="sm:col-span-2 text-stone-600 dark:text-stone-300">
            Ask the central site's administrator for this shop's sync key, then set WEB_SYNC_URL and WEB_SYNC_KEY in backend/.env and restart the backend.
          </p>
        )}
      </section>

      <section className={`${panel} flex flex-wrap items-end gap-3 p-4`}>
        <div>
          <label htmlFor="ws-day" className={label}>Day</label>
          <input id="ws-day" type="date" value={day} max={today()} onChange={e => { setDay(e.target.value); setDayError(''); }}
            className={`${field} ${dayError ? fieldError : ''}`} aria-invalid={!!dayError} />
          <InlineError message={dayError} />
        </div>
        <button className={primaryButton} onClick={syncNow} disabled={busy || off}><CloudUpload size={14} />{busy ? 'Sending…' : 'Sync now'}</button>
      </section>
    </div>
  );
}
