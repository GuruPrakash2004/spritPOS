// The folder the .rpt print formats are read from (backend RPT_FOLDER in .env, routers/setup.py /setup/rpt-folder).
// Shown on the sign-in page's settings and at the top of Print Settings. The path is a folder on the PC the backend
// runs on; blank means the default (backend/Rpts). Save checks the folder exists, uses it at once and says how many
// .rpt files it holds. Only an Administrator (or a request from the server PC itself) can change it.

import { useEffect, useState } from 'react';
import { AlertCircle, CheckCircle2, FolderOpen, Loader2, Save } from 'lucide-react';
import { toastStore } from './toastStore';
import { errorText, getRptFolder, saveRptFolder } from '../../lib/api';
import { field as fieldClass, label as labelClass } from '../../lib/styles';

export default function RptFolderSetting({ inputClass = fieldClass, captionClass = labelClass, plain = false }) {
  const [info, setInfo] = useState(null);
  const [value, setValue] = useState('');
  const [busy, setBusy] = useState(false);
  const [error, setError] = useState('');

  useEffect(() => {
    getRptFolder().then(i => { setInfo(i); setValue(i.folder); }).catch(e => setError(errorText(e)));
  }, []);

  const save = async (e) => {
    e.preventDefault();
    const folder = value.trim();
    const text = folder ? 'The report files path will be saved when this closes.' : 'The default report files path will be used when this closes.';
    if (!(await toastStore.defer('ok', text, 'Cancelled. The report files path is unchanged.'))) return;
    setBusy(true); setError('');
    try {
      const i = await saveRptFolder(folder);
      setInfo(i); setValue(i.folder);
      toastStore.add('ok', `Report files path saved: ${i.count} .rpt file${i.count === 1 ? '' : 's'} found.`);
    } catch (err) { setError(errorText(err)); } finally { setBusy(false); }
  };

  const changed = info != null && value.trim() !== info.folder;
  const good = info && info.exists && info.count > 0;
  return (
    <form onSubmit={save} noValidate className={plain ? '' : 'rounded-2xl border border-[#DDE3EC] p-5 dark:border-[#2A2A2A]'}>
      <div className="mb-3 flex items-center gap-2">
        <FolderOpen size={16} className="text-[#1B2B4B] dark:text-[#D4A847]" />
        <h2 className="text-sm font-bold">Report files (.rpt) path</h2>
      </div>
      <label htmlFor="rpt-folder" className={captionClass}>Folder on this server</label>
      <input id="rpt-folder" value={value} onChange={e => { setValue(e.target.value); setError(''); }} spellCheck={false}
        placeholder={info ? `Blank = ${info.default}` : 'e.g. C:\\vLQMS\\Reports'} className={inputClass} aria-invalid={!!error} />
      {info && !error && (
        <p className={`mt-2 flex items-center gap-2 text-xs ${good ? 'text-emerald-700 dark:text-emerald-400' : 'text-amber-700 dark:text-amber-400'}`}>
          {good ? <CheckCircle2 size={14} /> : <AlertCircle size={14} />}
          {!info.exists ? `The folder ${info.effective} was not found.`
            : info.count ? `${info.count} .rpt file${info.count === 1 ? '' : 's'} in ${info.effective}` : `No .rpt files in ${info.effective}.`}
        </p>
      )}
      {error && <p role="alert" className="mt-2 flex items-center gap-2 text-xs text-red-600 dark:text-red-400"><AlertCircle size={14} className="shrink-0" />{error}</p>}
      <button type="submit" disabled={busy || !changed}
        className="mt-3 flex items-center justify-center gap-2 rounded-xl bg-[#1B2B4B] px-4 py-2.5 text-xs font-bold text-white transition hover:bg-[#121D36] disabled:pointer-events-none disabled:opacity-45 dark:bg-gradient-to-r dark:from-[#D4A847] dark:via-[#F7D98B] dark:to-[#C8952E] dark:text-[#181106]">
        {busy ? <Loader2 size={14} className="animate-spin" /> : <Save size={14} />}Save path
      </button>
    </form>
  );
}
