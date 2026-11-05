// Desktop rights tab of Admin Master (Security) (the desktop's frmMstRights): the rights the exe itself uses, kept in MstUserRights (or MstRoleRights
// when the shop allocates by role). Pick a user (or role), tick Add / Edit / Delete / Print / View / Auth / Auth1 / Opt-1..5 for each menu screen,
// and Save: it writes the shop's own tables, so the desktop and the web agree. Copy From fills the grid from another user (nothing is saved until
// Save). A screen with no ticks at all is greyed out in the desktop and closed in the web. The user ADMIN always has every right.

import { useCallback, useEffect, useMemo, useState } from 'react';
import { Copy, Save } from 'lucide-react';
import { toastStore } from '../common/toastStore';
import { InlineError } from '../common/FormErrorSummary';
import { errorText, getDesktopRights, getDesktopRightsOverview, saveDesktopRights } from '../../lib/api';
import { field, label, panel, primaryButton, secondaryButton, tableBody, tableHead } from '../../lib/styles';

const BLANK = '0'.repeat(12);
const flagAt = (flags, i) => (flags ?? BLANK)[i] === '1';
const setFlag = (flags, i, on) => { const f = (flags ?? BLANK).split(''); f[i] = on ? '1' : '0'; return f.join(''); };

export default function DesktopRightsTab() {
  const [info, setInfo] = useState(null);
  const [owner, setOwner] = useState('');
  const [rights, setRights] = useState({});           // { menuId: '010100000000' } as shown (edited)
  const [dirty, setDirty] = useState(false);
  const [type, setType] = useState('');
  const [search, setSearch] = useState('');
  const [copyFrom, setCopyFrom] = useState('');
  const [problem, setProblem] = useState('');

  useEffect(() => { getDesktopRightsOverview().then(setInfo).catch(e => setProblem(errorText(e))); }, []);

  const load = useCallback(async (id) => {
    setProblem('');
    try { const r = await getDesktopRights(id); setRights(r.rights); setDirty(false); } catch (e) { setProblem(errorText(e)); }
  }, []);
  const choose = (id) => { setOwner(id); setCopyFrom(''); if (id) void load(id); else setRights({}); };

  const types = useMemo(() => [...new Set((info?.menu ?? []).map(m => m.type))], [info]);
  const shown = useMemo(() => {
    const q = search.trim().toLowerCase();
    return (info?.menu ?? []).filter(m => (!type || m.type === type) && (!q || m.caption.toLowerCase().includes(q)));
  }, [info, type, search]);

  const toggle = (id, i, on) => { setRights(r => ({ ...r, [id]: setFlag(r[id], i, on) })); setDirty(true); };
  const toggleColumn = (i, on) => { setRights(r => ({ ...r, ...Object.fromEntries(shown.map(m => [m.id, setFlag(r[m.id], i, on)])) })); setDirty(true); };
  const copy = async () => {
    if (!copyFrom) return;
    try { const r = await getDesktopRights(copyFrom); setRights(r.rights); setDirty(true); toastStore.add('ok', 'Rights copied. Save to keep them.'); } catch (e) { setProblem(errorText(e)); }
  };

  const save = async () => {
    if (!owner) return setProblem(`Select a ${info.by}.`);
    setProblem('');
    if (!(await toastStore.defer('ok', 'The rights will be saved when this closes.', 'Save cancelled. Nothing was changed.'))) return;
    try {
      const body = info.menu.map(m => ({ menu_id: m.id, flags: rights[m.id] ?? BLANK }));
      const r = await saveDesktopRights(owner, body);
      setRights(r.rights); setDirty(false);
      toastStore.add('ok', 'Updated successfully.');
    } catch (e) { setProblem(errorText(e)); }
  };

  if (!info) return <p className="text-sm text-stone-500">{problem || 'Loading…'}</p>;
  const ownerName = info.owners.find(o => String(o.id) === String(owner));
  const allOn = (i) => shown.length > 0 && shown.every(m => flagAt(rights[m.id], i));

  return (
    <div className="flex flex-col gap-3">
      <p className="text-xs text-stone-500">Rights are given {info.by === 'user' ? 'to each user' : 'to each role'} here, as in the desktop's Security screen (this shop allocates by {info.by}). ADMIN always has every right.</p>
      {problem && <InlineError message={problem} />}
      <div className={`${panel} flex flex-wrap items-end gap-3 px-4 py-3`}>
        <div>
          <label htmlFor="dr-owner" className={label}>{info.by === 'user' ? 'User' : 'Role'}</label>
          <select id="dr-owner" value={owner} onChange={e => choose(e.target.value)} className={field}>
            <option value="">Select…</option>
            {info.owners.map(o => <option key={o.id} value={o.id}>{o.name}{o.detail && o.detail !== o.name ? ` – ${o.detail}` : ''}</option>)}
          </select>
        </div>
        <div>
          <label htmlFor="dr-type" className={label}>Show</label>
          <select id="dr-type" value={type} onChange={e => setType(e.target.value)} className={field}>
            <option value="">All</option>
            {types.map(t => <option key={t}>{t}</option>)}
          </select>
        </div>
        <div>
          <label htmlFor="dr-find" className={label}>Filter on name</label>
          <input id="dr-find" value={search} onChange={e => setSearch(e.target.value)} className={field} />
        </div>
        <div className="ml-auto flex items-end gap-2">
          <div>
            <label htmlFor="dr-copy" className={label}>Copy From</label>
            <select id="dr-copy" value={copyFrom} onChange={e => setCopyFrom(e.target.value)} disabled={!owner} className={field}>
              <option value="">Select…</option>
              {info.owners.filter(o => String(o.id) !== String(owner)).map(o => <option key={o.id} value={o.id}>{o.name}</option>)}
            </select>
          </div>
          <button onClick={() => void copy()} disabled={!copyFrom} className={secondaryButton}><Copy size={14} />Copy</button>
          <button onClick={() => void save()} disabled={!owner} className={primaryButton}><Save size={14} />Save{dirty ? ' *' : ''}</button>
        </div>
      </div>

      {owner ? (
        <div className={`${panel} max-h-[60vh] overflow-auto`}>
          <table className="w-full text-xs">
            <thead className={`${tableHead} sticky top-0`}>
              <tr>
                <th className="px-3 py-2 text-left">Type</th><th className="px-3 py-2 text-left">Menu</th>
                {info.flags.map((f, i) => (
                  <th key={f} className="px-2 py-2 text-center">
                    <label className="flex flex-col items-center gap-0.5"><span>{f}</span>
                      <input type="checkbox" aria-label={`All ${f}`} checked={allOn(i)} onChange={e => toggleColumn(i, e.target.checked)} className="accent-accent" /></label>
                  </th>
                ))}
              </tr>
            </thead>
            <tbody className={tableBody}>
              {shown.map(m => (
                <tr key={m.id}>
                  <td className="px-3 py-1 text-stone-500">{m.type}</td>
                  <td className="px-3 py-1 font-medium">{m.caption}</td>
                  {info.flags.map((f, i) => (
                    <td key={f} className="px-2 py-1 text-center">
                      <input type="checkbox" aria-label={`${f} for ${m.caption}`} checked={flagAt(rights[m.id], i)} onChange={e => toggle(m.id, i, e.target.checked)} className="accent-accent" />
                    </td>
                  ))}
                </tr>
              ))}
              {!shown.length && <tr><td colSpan={2 + info.flags.length} className="px-3 py-8 text-center text-stone-500">No menu screens match.</td></tr>}
            </tbody>
          </table>
        </div>
      ) : <p className="py-8 text-center text-sm text-stone-500">Select a {info.by} to see and change their rights{ownerName ? '' : '.'}</p>}
    </div>
  );
}
