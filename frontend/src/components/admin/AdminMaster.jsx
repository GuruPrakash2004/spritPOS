// Admin Master (Security) (/adminMaster, Administrator only; the desktop menu's Utilities → Security, 700):
// users, roles and per-screen rights (legacy frmMstUSer, frmMstRole, frmMstRights), plus the tabs Field rights
// (frmMstFldRights), Master approvals (frmUtlMstApprovals), Report rights (frmUtlAssignRight) and Login settings
// (frmUtlLoginSettings, Change Adnl. ADMIN Password). Rights are per role: a user who needs different rights
// gets a role of their own.
//
// Endpoints: /admin/users (GET/POST/PUT/DELETE), /admin/roles, /admin/field-rights,
// /admin/master-approval, /admin/report-rights, /admin/login-settings, /admin/adnl-admin-password.
// A role's modules and its Add/Edit/Delete ticks are saved and enforced by the server.

import { useCallback, useEffect, useState, useRef } from 'react';
import {
  FilePlus, KeyRound, Save, Trash2, Users, X,
  Eye, Edit3, TextCursorInput, BadgeCheck, FileBarChart, LockKeyhole,
} from 'lucide-react';
import { useConfirm } from '../common/ConfirmDialog';
import Toast from '../common/Toast';
import { toastStore } from '../common/toastStore';
import { FormErrorSummary } from '../common/FormErrorSummary';
import { createAdminUser, deleteAdminUser, deleteAdminRole, errorText, getAdminUsers, getRoles, saveAdminRole, updateAdminUser, } from '../../lib/api';
import { ADMIN_ROLE } from '../../lib/modules';
import { badge, label, pageHeader, pageSub, pageTitle, primaryButton, secondaryButton, dangerButton } from '../../lib/styles';
import { MATRIX_SCREENS, draftRights, parseRoleToDraft, toggleActionDraft, toggleGroupDraft, toggleOpenDraft } from './matrixDefs';
import AdminUsersTab, { } from './AdminUsersTab';
import { loadPasswordRules, passwordProblem } from '../../lib/passwords';
import FieldRightsTab from './FieldRightsTab';
import ApprovalsTab from './ApprovalsTab';
import ReportRightsTab from './ReportRightsTab';
import LoginSettingsTab from './LoginSettingsTab';
import { encryptSecret } from '../../lib/secureField';
import PermissionMatrixTable from './PermissionMatrixTable';

const tabButton = (on) => `-mb-px flex items-center gap-2 whitespace-nowrap border-b-2 px-3.5 py-2.5 text-xs font-semibold transition-colors ${on
  ? 'border-[#64283A] text-[#64283A] dark:border-[#D4A847] dark:text-[#E8C36A]'
  : 'border-transparent text-stone-500 hover:text-stone-900 dark:text-stone-400 dark:hover:text-stone-50'}`;
const EMPTY_USER_DETAILS = { full_name: '', email: '', mobile: '', department: '', dob: '', password_reset_on: '', inactive_reason: '' };

export default function AdminMaster({ currentUser }) {
  const [tab, setTab] = useState('roles');
  const [users, setUsers] = useState([]);
  const [roles, setRoles] = useState([]);
  const [userForm, setUserForm] = useState(null);
  const [userFormErrors, setUserFormErrors] = useState({});
  const [roleName, setRoleName] = useState('Manager');
  const { confirm, dialog } = useConfirm();

  // Role draft with structured module rights and action button permissions
  // Empty until /admin/roles loads (then the chosen role's saved rights).
  const [roleDraft, setRoleDraft] = useState(() => parseRoleToDraft({ name: 'Manager', modules: [] }));

  const [roleFormErrors, setRoleFormErrors] = useState({});
  const [toast, setToast] = useState(null);
  const gridRefs = useRef(new Map());

  const fail = (e) => setToast({ kind: 'error', text: errorText(e) });

  const loadRoleIntoDraft = useCallback((r) => {
    setRoleName(r.name);
    setRoleDraft(parseRoleToDraft(r));
    setRoleFormErrors({});
  }, []);

  const reload = useCallback(() => {
    getAdminUsers().then(setUsers).catch(fail);
    getRoles().then(rList => {
      setRoles(rList);
      setRoleName(current => {
        const target = rList.find(r => r.name === current) ?? rList.find(r => r.name === 'Manager') ?? rList[0];
        if (target) {
          setRoleDraft(parseRoleToDraft(target));
          return target.name;
        }
        return current;
      });
    }).catch(fail);
  }, []);

  useEffect(() => { reload(); loadPasswordRules(); }, [reload]);

  // ---- Users ----
  const handleUserFieldChange = (k, v) => {
    if (!userForm) return;
    setUserForm({ ...userForm, [k]: v });
    if (userFormErrors[k ]) {
      setUserFormErrors(prev => {
        const next = { ...prev };
        delete next[k ];
        return next;
      });
    }
  };

  const focusUserField = (k) => {
    const map = { user: 'am-user', password: 'am-pwd', role: 'am-role' };
    if (!map[k]) map[k] = `am-${k}`;
    const el = document.getElementById(map[k] || k);
    if (el) { el.scrollIntoView({ behavior: 'smooth', block: 'center' }); el.focus(); }
  };

  const saveUser = async () => {
    if (!userForm) return;
    const { user, password, role, active, isNew, full_name, email, mobile, department, dob, password_reset_on, inactive_reason } = userForm;
    const errors = {};
    if (email.trim() && !/^[^@\s]+@[^@\s]+\.[^@\s]+$/.test(email.trim())) errors.email = 'EmailID is not valid!!';
    if (!active && !inactive_reason.trim()) errors.inactive_reason = 'Enter why this user is inactive';
    if (dob && dob > new Date().toISOString().slice(0, 10)) errors.dob = 'Not a valid DOB';

    if (!user.trim()) errors.user = 'Enter the user name';
    if (!role) errors.role = 'Select a role';
    const pwProblem = passwordProblem(password, user);
    if (isNew && !password) errors.password = 'Enter a password for the new user';
    else if (password && pwProblem) errors.password = pwProblem;

    if (Object.keys(errors).length > 0) {
      setUserFormErrors(errors);
      focusUserField(Object.keys(errors)[0]);
      return;
    }
    setUserFormErrors({});

    const body = {
      user: user.trim().toUpperCase(), password: password || null, role, active, full_name: full_name.trim(), email: email.trim(),
      mobile: mobile.trim(), department: department.trim(), dob: dob || null, password_reset_on: password_reset_on || null,
      inactive_reason: active ? '' : inactive_reason.trim(),
    };
    if (!(await toastStore.defer('ok', `User ${body.user} will be saved when this closes.`, 'Save cancelled. Nothing was changed.'))) return;
    // The password leaves the browser only encrypted (lib/secureField.js).
    const request = encryptSecret(body.password ?? '', 'user').then(sealed => {
      const sent = { ...body, password: sealed || null };
      return isNew
        ? createAdminUser(sent)
        : updateAdminUser(user, sent);
    });
    request.then(() => {
      setToast({ kind: 'ok', text: `User ${body.user} saved.` });
      setUserForm(null);
      reload();
    }).catch(fail);
  };

  const deleteUser = async (name) => {
    if (!(await confirm(`Delete user ${name}? They will no longer be able to sign in.`))) return;
    if (!(await toastStore.defer('delete', `User ${name} will be deleted when this closes.`, 'Delete cancelled. Nothing was removed.'))) return;
    deleteAdminUser(name)
      .then(() => { setToast({ kind: 'ok', text: `User ${name} deleted.` }); reload(); })
      .catch(fail);
  };

  // ---- Matrix ticks ----
  const toggleOpen = (screenId) => setRoleDraft(d => toggleOpenDraft(d, screenId));
  const toggleAction = (screenId, actionType) => setRoleDraft(d => toggleActionDraft(d, screenId, actionType));
  const toggleGroup = (group, col) => setRoleDraft(d => toggleGroupDraft(d, group, col));

  const saveRole = async () => {
    if (!roleDraft) return;
    const name = roleDraft.name.trim();
    const errors = {};

    if (name.length < 2) errors.name = 'Enter a role name';
    if (!roleDraft.modules.size) errors.modules = 'Give the role at least one screen';

    if (Object.keys(errors).length > 0) {
      setRoleFormErrors(errors);
      return;
    }
    setRoleFormErrors({});

    const body = { name, modules: [...roleDraft.modules], rights: draftRights(roleDraft) };
    if (!(await toastStore.defer('ok', `Rights for ${name} will be saved when this closes.`, 'Save cancelled. Nothing was changed.'))) return;
    saveAdminRole(roleName || name, body)
      .then(() => {
        setToast({ kind: 'ok', text: `Rights for ${name} saved, including Add/Edit/Delete. Signed-in users get the change within a minute.` });
        reload();
      })
      .catch(fail);
  };

  const deleteRole = async () => {
    if (!roleName) return;
    if (!(await confirm(`Delete role ${roleName}?`))) return;
    if (!(await toastStore.defer('delete', `Role ${roleName} will be deleted when this closes.`, 'Delete cancelled. Nothing was removed.'))) return;
    deleteAdminRole(roleName)
      .then(() => {
        setToast({ kind: 'ok', text: `Role ${roleName} deleted.` });
        const remaining = roles.filter(r => r.name !== roleName);
        if (remaining[0]) loadRoleIntoDraft(remaining[0]);
        reload();
      })
      .catch(fail);
  };

  // Keyboard navigation on ticks: arrow keys move, Space toggles, Enter saves, Esc cancels
  const handleTickKeyDown = (e, rowIdx, colIdx, screenId, colType) => {
    if (e.key === ' ' || e.key === 'Spacebar') {
      e.preventDefault();
      if (colType === 'open') toggleOpen(screenId);
      else toggleAction(screenId, colType);
      return;
    }
    if (e.key === 'Enter') {
      e.preventDefault();
      saveRole();
      return;
    }
    if (e.key === 'Escape') {
      e.preventDefault();
      const original = roles.find(r => r.name === roleName);
      if (original) loadRoleIntoDraft(original);
      return;
    }

    let nextRow = rowIdx;
    let nextCol = colIdx;

    if (e.key === 'ArrowRight') nextCol = Math.min(3, colIdx + 1);
    else if (e.key === 'ArrowLeft') nextCol = Math.max(0, colIdx - 1);
    else if (e.key === 'ArrowDown') nextRow = Math.min(MATRIX_SCREENS.length - 1, rowIdx + 1);
    else if (e.key === 'ArrowUp') nextRow = Math.max(0, rowIdx - 1);
    else return;

    e.preventDefault();
    const key = `${nextRow}-${nextCol}`;
    const el = gridRefs.current.get(key);
    if (el) el.focus();
  };

  // Derived counts
  const openCount = MATRIX_SCREENS.filter(s => roleDraft.modules.has(s.id)).length;
  // Total buttons allowed = number of checked actions + checked screens
  const buttonActionsCount = roleDraft.actions.size;
  const totalButtonsAllowed = openCount + buttonActionsCount;

  return (
    <div className="mx-auto flex max-w-6xl flex-col gap-5">
      {toast && <Toast kind={toast.kind} text={toast.text} />}
      {dialog}

      <div className={pageHeader}>
        <div>
          <div className="flex items-center gap-2">
            <h1 className={pageTitle}>Admin Master (Security)</h1>
            <span className={badge('neutral')}>Administrators only</span>
          </div>
          <p className={pageSub}>Users, roles, structured item permissions and operational hierarchy.</p>
        </div>
      </div>

      <div role="tablist" aria-label="Admin sections" className="flex gap-1 overflow-x-auto border-b border-line">
        <button role="tab" aria-selected={tab === 'users'} onClick={() => setTab('users')} className={tabButton(tab === 'users')}>
          <Users size={14} />Users
        </button>
        <button role="tab" aria-selected={tab === 'roles'} onClick={() => setTab('roles')} className={tabButton(tab === 'roles')}>
          <KeyRound size={14} />Roles &amp; rights
        </button>
        <button role="tab" aria-selected={tab === 'fields'} onClick={() => setTab('fields')} className={tabButton(tab === 'fields')}>
          <TextCursorInput size={14} />Field rights
        </button>
        <button role="tab" aria-selected={tab === 'approvals'} onClick={() => setTab('approvals')} className={tabButton(tab === 'approvals')}>
          <BadgeCheck size={14} />Master approvals
        </button>
        <button role="tab" aria-selected={tab === 'reports'} onClick={() => setTab('reports')} className={tabButton(tab === 'reports')}>
          <FileBarChart size={14} />Report rights
        </button>
        <button role="tab" aria-selected={tab === 'login'} onClick={() => setTab('login')} className={tabButton(tab === 'login')}>
          <LockKeyhole size={14} />Login settings
        </button>
      </div>

      {tab === 'fields' && <FieldRightsTab roles={roles} />}
      {tab === 'approvals' && <ApprovalsTab />}
      {tab === 'reports' && <ReportRightsTab />}
      {tab === 'login' && <LoginSettingsTab currentUser={currentUser} />}

      {tab === 'users' && <AdminUsersTab
        users={users}
        roles={roles}
        currentUser={currentUser}
        userForm={userForm}
        errors={userFormErrors}
        onNew={() => {
          setUserFormErrors({});
          setUserForm({ user: '', password: '', role: roles.find(r => r.name !== ADMIN_ROLE)?.name ?? ADMIN_ROLE, active: true, isNew: true, ...EMPTY_USER_DETAILS });
        }}
        onEdit={user => {
          setUserFormErrors({});
          setUserForm({
            user: user.user, password: '', role: user.role, active: user.active, isNew: false, full_name: user.full_name ?? '',
            email: user.email ?? '', mobile: user.mobile ?? '', department: user.department ?? '', dob: user.dob ?? '',
            password_reset_on: user.password_reset_on ?? '', inactive_reason: user.inactive_reason ?? '',
          });
        }}
        onDelete={deleteUser}
        onChange={handleUserFieldChange}
        onFocusField={focusUserField}
        onCancel={() => { setUserForm(null); setUserFormErrors({}); }}
        onSave={saveUser}
      />}

      {/* ── TAB 2 & TAB 3: PERMISSION MATRIX HIERARCHY (Matches Image Exactly) ── */}
      {tab === 'roles' && (
        <div className="space-y-4 animate-slideUp">
          {/* Quick Role Switcher Bar */}
          <div className="flex flex-wrap items-center justify-between gap-3 rounded-2xl border border-[#E3D9CD] bg-[#FFFCF7] p-3.5 shadow-xs dark:border-[#2A2A2A] dark:bg-[#141414]">
            <div className="flex items-center gap-2.5">
              <span className="text-xs font-semibold text-[#70675F] dark:text-stone-400">Select Role:</span>
              <div className="flex flex-wrap gap-1.5">
                {roles.map(r => (
                  <button
                    key={r.name}
                    onClick={() => loadRoleIntoDraft(r)}
                    className={`rounded-lg px-3 py-1 text-xs font-semibold transition-all active:scale-[0.98] ${
                      roleDraft.name === r.name
                        ? 'bg-[#64283A] text-white shadow-sm dark:bg-gradient-to-r dark:from-[#D4A847] dark:via-[#F5D88A] dark:to-[#C8952E] dark:text-[#180E09] dark:font-extrabold'
                        : 'bg-transparent border border-[#E3D9CD] text-[#70675F] hover:bg-[#F3EEE6] dark:border-[#2A2A2A] dark:text-stone-300 dark:hover:bg-[#1C1C1C]'
                    }`}
                  >
                    {r.name}
                  </button>
                ))}
              </div>
            </div>
            <button
              onClick={() => {
                setRoleName('New Role');
                setRoleDraft({
                  name: 'New Role',
                  modules: new Set(['Dashboard']),
                  actions: new Set(['Dashboard:open']),
                });
                setRoleFormErrors({});
              }}
              className={secondaryButton}
            >
              <FilePlus size={14} />Create new role
            </button>
          </div>

          {/* Form Error Banner if any */}
          <FormErrorSummary errors={roleFormErrors} onFocusField={() => document.getElementById('role-name-input')?.focus()} />

          {/* ══ TOP ROW: Role Name + Count + Action Buttons ══ */}
          <div className="flex flex-wrap items-end justify-between gap-4 pt-1">
            <div>
              <label htmlFor="role-name-input" className={label}>
                Role Name
              </label>
              <div className="flex flex-wrap items-center gap-3">
                <input
                  id="role-name-input"
                  value={roleDraft.name}
                  onChange={e => {
                    setRoleDraft({ ...roleDraft, name: e.target.value });
                    if (roleFormErrors.name) setRoleFormErrors({});
                  }}
                  className={`w-64 rounded-xl border border-[#DCD1C5] bg-[#FFFCF7] px-3 py-2 text-xs font-semibold text-[#29231F] shadow-xs outline-none transition-all placeholder:text-[#A69D94] hover:border-[#CBBEAF] focus:border-[#64283A] focus:ring-[3px] focus:ring-[#64283A]/10 dark:border-[#2A2A2A] dark:bg-[#1A1A1A] dark:text-stone-50 dark:focus:border-[#D4A847] dark:focus:ring-[#D4A847]/20 ${
                    roleFormErrors.name ? '!border-red-500 !ring-2 !ring-red-500/20' : ''
                  }`}
                />
                <span className="text-xs font-medium text-[#70675F] dark:text-stone-400 tabular-nums">
                  {openCount} screens open • {totalButtonsAllowed} buttons allowed
                </span>
              </div>
            </div>

            {/* Action Buttons */}
            <div className="flex items-center gap-2">
              <button
                type="button"
                onClick={saveRole}
                className={primaryButton}
              >
                <Save size={14} />
                <span>Save Rights</span>
              </button>

              {roleName && roleName !== ADMIN_ROLE && (
                <button
                  type="button"
                  onClick={deleteRole}
                  className={dangerButton}
                >
                  <Trash2 size={14} />
                  <span>Delete Role</span>
                </button>
              )}

              <button
                type="button"
                onClick={() => {
                  const r = roles.find(x => x.name === roleName);
                  if (r) loadRoleIntoDraft(r);
                }}
                className={secondaryButton}
              >
                <X size={14} />
                <span>Cancel</span>
              </button>
            </div>
          </div>

          {/* ══ 4 SUMMARY CARDS: Open · Add / Save · Edit · Delete / Cancel ══ */}
          <div className="grid grid-cols-1 gap-3 sm:grid-cols-2 lg:grid-cols-4 pt-1">
            {/* Card 1: Open */}
            <div className="rounded-2xl border border-[#E3D9CD] bg-[#FFFCF7] p-3.5 text-xs shadow-xs dark:border-[#2A2A2A] dark:bg-[#141414]">
              <div className="flex items-center gap-1.5 font-bold text-[#64283A] dark:text-[#D4A847]">
                <Eye size={15} />
                <span>Open</span>
              </div>
              <p className="mt-1 text-[11px] text-[#70675F] dark:text-stone-400">
                See the screen in the sidebar and look at its data
              </p>
            </div>

            {/* Card 2: Add / Save */}
            <div className="rounded-2xl border border-[#E3D9CD] bg-[#FFFCF7] p-3.5 text-xs shadow-xs dark:border-[#2A2A2A] dark:bg-[#141414]">
              <div className="flex items-center gap-1.5 font-bold text-[#64283A] dark:text-[#D4A847]">
                <FilePlus size={15} />
                <span>Add / Save</span>
              </div>
              <p className="mt-1 text-[11px] text-[#70675F] dark:text-stone-400">
                Create new records or save bills and entries
              </p>
            </div>

            {/* Card 3: Edit */}
            <div className="rounded-2xl border border-[#E3D9CD] bg-[#FFFCF7] p-3.5 text-xs shadow-xs dark:border-[#2A2A2A] dark:bg-[#141414]">
              <div className="flex items-center gap-1.5 font-bold text-[#64283A] dark:text-[#D4A847]">
                <Edit3 size={15} />
                <span>Edit</span>
              </div>
              <p className="mt-1 text-[11px] text-[#70675F] dark:text-stone-400">
                Change existing records
              </p>
            </div>

            {/* Card 4: Delete / Cancel */}
            <div className="rounded-2xl border border-[#E3D9CD] bg-[#FFFCF7] p-3.5 text-xs shadow-xs dark:border-[#2A2A2A] dark:bg-[#141414]">
              <div className="flex items-center gap-1.5 font-bold text-[#9A4B45] dark:text-red-400">
                <Trash2 size={15} />
                <span>Delete / Cancel</span>
              </div>
              <p className="mt-1 text-[11px] text-[#70675F] dark:text-stone-400">
                Remove records or cancel saved bills
              </p>
            </div>
          </div>

          {/* Keyboard tip instruction line */}
          <div className="text-xs text-stone-600 dark:text-stone-400 pt-1">
            Keyboard: click any tick, then use the <span className="font-bold text-stone-800 dark:text-stone-200">arrow keys</span> to move,{' '}
            <span className="font-bold text-stone-800 dark:text-stone-200">Space</span> to tick or untick,{' '}
            <span className="font-bold text-stone-800 dark:text-stone-200">Enter</span> to save and{' '}
            <span className="font-bold text-stone-800 dark:text-stone-200">Esc</span> to cancel.
          </div>

          <PermissionMatrixTable
            roleDraft={roleDraft}
            onToggleOpen={toggleOpen}
            onToggleAction={toggleAction}
            onToggleGroup={toggleGroup}
            onTickKeyDown={handleTickKeyDown}
            registerCell={(key, element) => {
              if (element) gridRefs.current.set(key, element);
              else gridRefs.current.delete(key);
            }}
          />
        </div>
      )}
    </div>
  );
}