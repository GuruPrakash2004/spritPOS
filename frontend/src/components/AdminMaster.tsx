import { useCallback, useEffect, useState } from 'react';
import { FilePlus, KeyRound, Save, ShieldCheck, Trash2, Users, X } from 'lucide-react';
import Toast from './Toast';
import { api, errorText, type ApiAdminUser, type ApiRole } from './api';
import { ADMIN_ROLE, MODULE_GROUPS } from './modules';
import { dangerButton, field, label, pageSub, pageTitle, panel, primaryButton, secondaryButton, tableBody, tableHead } from './ui';

type UserForm = { user: string; password: string; role: string; active: boolean; isNew: boolean };
const tabButton = (on: boolean) => `flex items-center gap-1.5 rounded-lg px-3.5 py-2 text-xs font-bold ${on
  ? 'bg-[#e8702a] text-white'
  : 'text-stone-600 hover:bg-[#e8702a]/10 hover:text-[#b84716] dark:text-stone-300 dark:hover:text-[#ffb07e]'}`;

// Admin Master (/adminMaster): users, roles and module rights. The web version of the legacy
// User Master, Role Master and Assign Rights screens. Administrator-only, enforced by the backend too.
export default function AdminMaster({ currentUser }: { currentUser: string }) {
  const [tab, setTab] = useState<'users' | 'roles'>('users');
  const [users, setUsers] = useState<ApiAdminUser[]>([]);
  const [roles, setRoles] = useState<ApiRole[]>([]);
  const [userForm, setUserForm] = useState<UserForm | null>(null);
  const [roleName, setRoleName] = useState('');
  const [roleDraft, setRoleDraft] = useState<{ name: string; modules: Set<string> } | null>(null);
  const [toast, setToast] = useState<{ kind: 'ok' | 'error'; text: string } | null>(null);

  useEffect(() => {
    if (!toast) return;
    const timer = setTimeout(() => setToast(null), 3000);
    return () => clearTimeout(timer);
  }, [toast]);

  const fail = (e: unknown) => setToast({ kind: 'error', text: errorText(e) });

  const reload = useCallback(() => {
    api<ApiAdminUser[]>('/admin/users').then(setUsers).catch(fail);
    api<ApiRole[]>('/admin/roles').then(setRoles).catch(fail);
  }, []);

  useEffect(() => { reload(); }, [reload]);

  // ---- users ----
  const saveUser = () => {
    if (!userForm) return;
    const { user, password, role, active, isNew } = userForm;
    if (!user.trim() || !role) return setToast({ kind: 'error', text: 'User name and role are required.' });
    if (isNew && password.length < 4) return setToast({ kind: 'error', text: 'Password must be at least 4 characters.' });
    const body = { user: user.trim().toUpperCase(), password: password || null, role, active };
    const request = isNew
      ? api('/admin/users', { method: 'POST', body })
      : api(`/admin/users/${encodeURIComponent(user)}`, { method: 'PUT', body });
    request.then(() => {
      setToast({ kind: 'ok', text: `User ${body.user} saved.` });
      setUserForm(null);
      reload();
    }).catch(fail);
  };

  const deleteUser = (name: string) => {
    if (!window.confirm(`Delete user ${name}?`)) return;
    api(`/admin/users/${encodeURIComponent(name)}`, { method: 'DELETE' })
      .then(() => { setToast({ kind: 'ok', text: `User ${name} deleted.` }); reload(); })
      .catch(fail);
  };

  // ---- roles ----
  const editRole = (r: ApiRole) => { setRoleName(r.name); setRoleDraft({ name: r.name, modules: new Set(r.modules) }); };

  const toggle = (modules: string[], on: boolean) => {
    if (!roleDraft) return;
    const next = new Set(roleDraft.modules);
    modules.forEach(m => (on ? next.add(m) : next.delete(m)));
    setRoleDraft({ ...roleDraft, modules: next });
  };

  const saveRole = () => {
    if (!roleDraft) return;
    const name = roleDraft.name.trim();
    if (name.length < 2) return setToast({ kind: 'error', text: 'Enter a role name.' });
    if (!roleDraft.modules.size) return setToast({ kind: 'error', text: 'Give the role at least one module.' });
    api(`/admin/roles/${encodeURIComponent(roleName || name)}`, { method: 'PUT', body: { name, modules: [...roleDraft.modules] } })
      .then(() => {
        setToast({ kind: 'ok', text: `Rights for ${name} saved. Users see the change at their next login.` });
        setRoleDraft(null);
        reload();
      })
      .catch(fail);
  };

  const deleteRole = (name: string) => {
    if (!window.confirm(`Delete role ${name}?`)) return;
    api(`/admin/roles/${encodeURIComponent(name)}`, { method: 'DELETE' })
      .then(() => { setToast({ kind: 'ok', text: `Role ${name} deleted.` }); setRoleDraft(null); reload(); })
      .catch(fail);
  };

  return (
    <div className="mx-auto flex max-w-6xl flex-col gap-4">
      {toast && <Toast kind={toast.kind} text={toast.text} />}
      <div className={`${panel} flex flex-wrap items-center justify-between gap-3 px-5 py-3`}>
        <div className="flex items-center gap-3">
          <div className="rounded-xl bg-[#e8702a]/10 p-2.5 text-[#b84716] dark:text-[#ffb07e]"><ShieldCheck size={20} /></div>
          <div>
            <h2 className={pageTitle}>Admin Master</h2>
            <p className={pageSub}>Users, roles and which modules each role can open. Administrators only.</p>
          </div>
        </div>
        <div className="flex gap-1 rounded-xl bg-[#f1e3d8] p-1 dark:bg-[#20253a]">
          <button onClick={() => setTab('users')} className={tabButton(tab === 'users')}><Users size={14} />Users</button>
          <button onClick={() => setTab('roles')} className={tabButton(tab === 'roles')}><KeyRound size={14} />Roles &amp; Rights</button>
        </div>
      </div>

      {tab === 'users' && (
        <div className="grid grid-cols-1 gap-4 lg:grid-cols-[1.4fr_1fr]">
          <div className={`${panel} overflow-hidden`}>
            <div className="flex items-center justify-between border-b border-[#e1c9b6] px-3 py-2 dark:border-[#343b59]">
              <span className="text-xs font-bold text-stone-600 dark:text-stone-300">{users.length} users</span>
              <button onClick={() => setUserForm({ user: '', password: '', role: roles.find(r => r.name !== ADMIN_ROLE)?.name ?? ADMIN_ROLE, active: true, isNew: true })} className={primaryButton}><FilePlus size={14} />New User</button>
            </div>
            <div className="overflow-x-auto">
            <table className="w-full min-w-[480px] text-xs">
              <thead className={tableHead}>
                <tr><th className="px-3 py-2">User</th><th className="px-3 py-2">Role</th><th className="px-3 py-2">Status</th><th className="px-3 py-2" /></tr>
              </thead>
              <tbody className={tableBody}>
                {users.map(u => (
                  <tr key={u.user} className="hover:bg-stone-50 dark:hover:bg-[#1b2033]">
                    <td className="px-3 py-2 font-mono font-bold text-stone-900 dark:text-stone-100">{u.user}{u.user === currentUser && <span className="ml-2 font-sans text-[10px] font-normal text-stone-400">(you)</span>}</td>
                    <td className="px-3 py-2">{u.role}</td>
                    <td className="px-3 py-2">
                      <span className={`rounded-full px-2 py-0.5 text-[10px] font-bold ${u.active ? 'bg-emerald-100 text-emerald-700 dark:bg-emerald-950/50 dark:text-emerald-300' : 'bg-stone-200 text-stone-600 dark:bg-stone-800 dark:text-stone-400'}`}>{u.active ? 'Active' : 'Inactive'}</span>
                    </td>
                    <td className="px-3 py-2">
                      <div className="flex justify-end gap-1.5">
                        <button onClick={() => setUserForm({ user: u.user, password: '', role: u.role, active: u.active, isNew: false })} className={`${secondaryButton} py-1`}>Edit</button>
                        <button onClick={() => deleteUser(u.user)} disabled={u.user === currentUser} className={`${dangerButton} py-1`} aria-label={`Delete ${u.user}`}><Trash2 size={12} /></button>
                      </div>
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
            </div>
          </div>

          <div className={`${panel} space-y-3 p-5`}>
            {!userForm && <p className="py-10 text-center text-xs text-stone-400">Pick a user to edit, or press New User.</p>}
            {userForm && (<>
              <div className="text-xs font-bold uppercase tracking-wider text-stone-500">{userForm.isNew ? 'New user' : `Editing ${userForm.user}`}</div>
              <div>
                <label htmlFor="am-user" className={`${label} !text-red-600 dark:!text-red-400`}>User Name</label>
                <input id="am-user" value={userForm.user} readOnly={!userForm.isNew} onChange={e => setUserForm({ ...userForm, user: e.target.value.toUpperCase() })} className={`${field} w-full font-mono uppercase ${userForm.isNew ? '' : 'bg-stone-100 dark:bg-[#1b2033]'}`} />
              </div>
              <div>
                <label htmlFor="am-pwd" className={`${label} ${userForm.isNew ? '!text-red-600 dark:!text-red-400' : ''}`}>{userForm.isNew ? 'Password' : 'New Password (leave empty to keep)'}</label>
                <input id="am-pwd" type="password" value={userForm.password} onChange={e => setUserForm({ ...userForm, password: e.target.value })} className={`${field} w-full`} autoComplete="new-password" />
              </div>
              <div>
                <label htmlFor="am-role" className={`${label} !text-red-600 dark:!text-red-400`}>Role</label>
                <select id="am-role" value={userForm.role} onChange={e => setUserForm({ ...userForm, role: e.target.value })} className={`${field} w-full`}>
                  {roles.map(r => <option key={r.name}>{r.name}</option>)}
                </select>
              </div>
              <label className="flex items-center gap-2 text-xs font-semibold text-stone-700 dark:text-stone-200">
                <input type="checkbox" checked={userForm.active} onChange={e => setUserForm({ ...userForm, active: e.target.checked })} className="accent-[#e8702a]" />
                Active (inactive users cannot log in)
              </label>
              <div className="flex gap-2 pt-1">
                <button onClick={saveUser} className={primaryButton}><Save size={14} />Save User</button>
                <button onClick={() => setUserForm(null)} className={secondaryButton}><X size={14} />Cancel</button>
              </div>
            </>)}
          </div>
        </div>
      )}

      {tab === 'roles' && (
        <div className="grid grid-cols-1 gap-4 lg:grid-cols-[280px_1fr]">
          <div className={`${panel} overflow-hidden`}>
            <div className="flex items-center justify-between border-b border-[#e1c9b6] px-3 py-2 dark:border-[#343b59]">
              <span className="text-xs font-bold text-stone-600 dark:text-stone-300">Roles</span>
              <button onClick={() => { setRoleName(''); setRoleDraft({ name: '', modules: new Set(['Dashboard']) }); }} className={primaryButton}><FilePlus size={14} />New Role</button>
            </div>
            <ul className="divide-y divide-[#f1e3d8] text-xs dark:divide-[#343b59]">
              {roles.map(r => (
                <li key={r.name}>
                  <button
                    onClick={() => r.name !== ADMIN_ROLE && editRole(r)}
                    className={`flex w-full items-center justify-between px-3 py-2.5 text-left ${roleDraft && roleName === r.name ? 'bg-[#e8702a]/10' : 'hover:bg-stone-50 dark:hover:bg-[#1b2033]'} ${r.name === ADMIN_ROLE ? 'cursor-default' : ''}`}
                  >
                    <span>
                      <span className="block font-bold text-stone-900 dark:text-stone-100">{r.name}</span>
                      <span className="text-[10px] text-stone-500">{r.modules.includes('*') ? 'All modules (fixed)' : `${r.modules.length} modules`} • {r.users} users</span>
                    </span>
                  </button>
                </li>
              ))}
            </ul>
          </div>

          <div className={`${panel} p-5`}>
            {!roleDraft && <p className="py-10 text-center text-xs text-stone-400">Pick a role to assign rights, or press New Role. The Administrator role always has every module.</p>}
            {roleDraft && (
              <div className="space-y-4">
                <div className="flex flex-wrap items-end gap-3">
                  <div>
                    <label htmlFor="am-rname" className={`${label} !text-red-600 dark:!text-red-400`}>Role Name</label>
                    <input id="am-rname" value={roleDraft.name} onChange={e => setRoleDraft({ ...roleDraft, name: e.target.value })} className={`${field} w-56`} />
                  </div>
                  <span className="text-xs text-stone-500">{roleDraft.modules.size} modules selected</span>
                  <div className="ml-auto flex gap-2">
                    <button onClick={saveRole} className={primaryButton}><Save size={14} />Save Rights</button>
                    {roleName && <button onClick={() => deleteRole(roleName)} className={dangerButton}><Trash2 size={14} />Delete Role</button>}
                    <button onClick={() => setRoleDraft(null)} className={secondaryButton}><X size={14} />Cancel</button>
                  </div>
                </div>
                <div className="grid grid-cols-1 gap-4 md:grid-cols-2">
                  {MODULE_GROUPS.map(g => {
                    const all = g.modules.every(m => roleDraft.modules.has(m));
                    return (
                      <fieldset key={g.group} className="rounded-xl border border-[#e1c9b6] p-3 dark:border-[#343b59]">
                        <legend className="px-1 text-[10px] font-bold uppercase tracking-wider text-stone-500">{g.group}</legend>
                        <label className="mb-2 flex items-center gap-2 border-b border-[#f1e3d8] pb-2 text-xs font-bold text-[#b84716] dark:border-[#343b59] dark:text-[#ffb07e]">
                          <input type="checkbox" checked={all} onChange={e => toggle(g.modules, e.target.checked)} className="accent-[#e8702a]" />
                          All {g.group}
                        </label>
                        <div className="grid grid-cols-1 gap-1.5 sm:grid-cols-2">
                          {g.modules.map(m => (
                            <label key={m} className="flex items-center gap-2 text-xs text-stone-700 dark:text-stone-200">
                              <input type="checkbox" checked={roleDraft.modules.has(m)} onChange={e => toggle([m], e.target.checked)} className="accent-[#e8702a]" />
                              {m}
                            </label>
                          ))}
                        </div>
                      </fieldset>
                    );
                  })}
                </div>
              </div>
            )}
          </div>
        </div>
      )}
    </div>
  );
}
