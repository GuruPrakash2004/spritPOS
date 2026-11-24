// Global Settings → Other Settings → File Attachment: tick the masters that show the File Attachment button (and Ctrl+A); a master left
// unticked has both hidden and takes no new files. Each master is one Global Settings row (group "File Attachment", AttachFile_<path>, Y / N;
// backend app/attachment_masters.py), read and saved through the Global Settings API, so the desktop's UtlGSettings holds the same values.
// Administrator only. The save waits for its popup.

import { useCallback, useEffect, useMemo, useState } from 'react';
import { Save } from 'lucide-react';
import { toastStore } from '../common/toastStore';
import { InlineError } from '../common/FormErrorSummary';
import { errorText, getGlobalSettings, saveGlobalSetting } from '../../lib/api';
import { resetAttachmentSettings } from '../../hooks/useAttachmentEnabled';
import { panel, primaryButton, secondaryButton, tableBody, tableHead } from '../../lib/styles';

export const ATTACHMENT_GROUP = 'File Attachment';
const masterName = (row) => String(row.caption).replace(/^Show File Attachment on /, '');

export default function AttachmentSettings() {
  const [rows, setRows] = useState(null);
  const [ticks, setTicks] = useState({});   // code -> true when the master shows File Attachment
  const [problem, setProblem] = useState('');
  const [busy, setBusy] = useState(false);

  const load = useCallback(() => getGlobalSettings()
    .then(all => {
      const list = all.filter(r => r.group === ATTACHMENT_GROUP).sort((a, b) => masterName(a).localeCompare(masterName(b)));
      setRows(list);
      setTicks(Object.fromEntries(list.map(r => [r.code, String(r.value).toUpperCase() !== 'N'])));
      setProblem('');
    })
    .catch(e => setProblem(errorText(e))), []);
  useEffect(() => { void load(); }, [load]);

  const changed = useMemo(() => (rows ?? []).filter(r => ticks[r.code] !== (String(r.value).toUpperCase() !== 'N')), [rows, ticks]);
  const setAll = (on) => setTicks(Object.fromEntries((rows ?? []).map(r => [r.code, on])));

  const save = async () => {
    if (!changed.length) return setProblem('Nothing has been changed.');
    if (!(await toastStore.defer('ok', 'File Attachment settings will be saved when this closes.', 'Save cancelled. Nothing was changed.'))) return;
    setBusy(true);
    try {
      for (const r of changed) await saveGlobalSetting(r.code, { caption: r.caption, value: ticks[r.code] ? 'Y' : 'N', hidden: r.hidden });
      resetAttachmentSettings();   // master screens already open read the new switches at once
      toastStore.add('ok', 'Saved Successfully.');
      await load();
    } catch (e) {
      setProblem(errorText(e));
    } finally {
      setBusy(false);
    }
  };

  if (!rows) return problem ? <InlineError message={problem} /> : <p className="text-xs text-stone-500">Loading…</p>;

  return (
    <section className={`${panel} space-y-3 p-4`} aria-label="File Attachment settings">
      <div>
        <h2 className="text-sm font-semibold">File Attachment on masters</h2>
        <p className="text-xs text-stone-500">Tick the masters that show the File Attachment button. An unticked master hides the button and takes no new files; files already attached are kept.</p>
      </div>
      <div className="flex gap-2">
        <button type="button" className={`${secondaryButton} !px-3 !py-1 text-xs`} onClick={() => setAll(true)}>Select all</button>
        <button type="button" className={`${secondaryButton} !px-3 !py-1 text-xs`} onClick={() => setAll(false)}>Clear</button>
      </div>
      <div className="max-h-[28rem] overflow-auto rounded-lg border border-line">
        <table className="w-full text-xs">
          <thead className={tableHead}><tr><th className="w-16 px-3 py-2 text-left">Show</th><th className="px-3 py-2 text-left">Master</th></tr></thead>
          <tbody className={tableBody}>
            {!rows.length && <tr><td colSpan={2} className="px-3 py-6 text-center text-stone-500">No masters were listed. The server may need a restart to offer these settings; press Reload on Global Settings after it is back.</td></tr>}
            {rows.map(r => (
              <tr key={r.code}>
                <td className="px-3 py-1.5">
                  <input type="checkbox" className="accent-accent" checked={!!ticks[r.code]} aria-label={`Show File Attachment on ${masterName(r)}`}
                    onChange={e => { setTicks(t => ({ ...t, [r.code]: e.target.checked })); setProblem(''); }} />
                </td>
                <td className="px-3 py-1.5">{masterName(r)}</td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
      {problem && <InlineError message={problem} />}
      <div className="flex justify-end">
        <button type="button" className={primaryButton} disabled={busy} onClick={() => void save()}><Save size={14} aria-hidden="true" /> Save</button>
      </div>
    </section>
  );
}
