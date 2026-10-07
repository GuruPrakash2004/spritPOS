// Users tab of Admin Master (Security): the user list plus the add/edit form (user, password, role, active,
// and the desktop user master's full name, email, mobile, department, DOB, password reset date and inactive reason).

import { FilePlus, Pencil, Save, Trash2, X } from 'lucide-react';
import { FormErrorSummary, InlineError } from '../common/FormErrorSummary';

import { badge, field, iconButton, label, panel, primaryButton, secondaryButton, sectionTitle, tableBody, tableHead, tableRow } from '../../lib/styles';

 



const bad = '!border-red-500 !ring-2 !ring-red-500/20 dark:!border-red-400 dark:!ring-red-400/20 animate-shake';
















const required = <span className="ml-0.5 text-red-600" aria-hidden="true">*</span>;

export default function AdminUsersTab({ users, roles, currentUser, userForm, errors, onNew, onEdit, onDelete, onChange, onFocusField, onCancel, onSave }) {
  return (
    <div className="grid grid-cols-1 gap-4 lg:grid-cols-[1.4fr_1fr]">
      <div className={`${panel} overflow-hidden`}>
        <div className="flex items-center justify-between border-b border-line px-4 py-3">
          <h2 className={sectionTitle}>Users <span className="ml-1 font-normal tabular-nums text-stone-500">{users.length}</span></h2>
          <button onClick={onNew} className={secondaryButton}><FilePlus size={14} />New user</button>
        </div>
        <div className="overflow-x-auto">
          <table className="w-full min-w-[480px] text-xs">
            <thead className={tableHead}>
              <tr><th className="px-4 py-2.5">User</th><th className="px-4 py-2.5">Role</th><th className="px-4 py-2.5">Status</th><th className="px-4 py-2.5"><span className="sr-only">Actions</span></th></tr>
            </thead>
            <tbody className={tableBody}>
              {users.map(user => (
                <tr key={user.user} className={`${tableRow} ${userForm && !userForm.isNew && userForm.user === user.user ? 'bg-accent-soft' : ''}`}>
                  <td className="px-4 py-2.5">
                    <div className="flex items-center gap-2.5">
                      <span className="flex h-7 w-7 items-center justify-center rounded-full bg-muted text-[10px] font-semibold text-stone-700 dark:text-stone-200" aria-hidden="true">{user.user.slice(0, 2)}</span>
                      <span className="font-medium text-stone-900 dark:text-stone-50">{user.user}</span>
                      {user.user === currentUser && <span className={badge('accent')}>You</span>}
                    </div>
                  </td>
                  <td className="px-4 py-2.5 text-stone-600 dark:text-stone-300">
                    <span className="font-medium text-stone-800 dark:text-stone-200">{user.role}</span>
                    {user.role === 'Cashier' && <span className="ml-2 text-[10px] font-semibold text-emerald-600 dark:text-emerald-400">(Can Add Items)</span>}
                  </td>
                  <td className="px-4 py-2.5">
                    <span className={badge(user.active ? 'success' : 'neutral')}>{user.active ? 'Active' : 'Inactive'}</span>
                    {user.password_expired && <span className={`${badge('warning')} ml-1`}>Password expired</span>}
                    {user.full_name && <div className="mt-0.5 text-[11px] text-stone-500">{user.full_name}</div>}
                  </td>
                  <td className="px-4 py-2">
                    <div className="flex justify-end gap-1">
                      <button onClick={() => onEdit(user)} className={iconButton} aria-label={`Edit ${user.user}`} title="Edit"><Pencil size={14} /></button>
                      <button onClick={() => onDelete(user.user)} disabled={user.user === currentUser} className={`${iconButton} hover:!text-red-600 dark:hover:!text-red-400`} aria-label={`Delete ${user.user}`} title="Delete"><Trash2 size={14} /></button>
                    </div>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      </div>

      <div className={`${panel} space-y-4 self-start p-5`}>
        {!userForm && <p className="py-10 text-center text-xs text-stone-500">Select a user to edit, or add a new one.</p>}
        {userForm && <>
          <h2 className={sectionTitle}>{userForm.isNew ? 'New user' : `Edit ${userForm.user}`}</h2>
          <FormErrorSummary errors={errors} onFocusField={onFocusField} />
          <div>
            <label htmlFor="am-user" className={label}>User name{required}</label>
            <input id="am-user" value={userForm.user} readOnly={!userForm.isNew} disabled={!userForm.isNew}
              onChange={event => onChange('user', event.target.value.toUpperCase())} aria-invalid={!!errors.user}
              className={`${field} font-mono uppercase ${errors.user ? '!border-red-500 !ring-2 !ring-red-500/20 dark:!border-red-400 dark:!ring-red-400/20 animate-shake' : ''}`} />
            <InlineError message={errors.user} />
          </div>
          <div>
            <label htmlFor="am-pwd" className={label}>{userForm.isNew ? <>Password{required}</> : 'New password'}</label>
            <input id="am-pwd" type="password" value={userForm.password} onChange={event => onChange('password', event.target.value)}
              className={`${field} ${errors.password ? '!border-red-500 !ring-2 !ring-red-500/20 dark:!border-red-400 dark:!ring-red-400/20 animate-shake' : ''}`}
              autoComplete="new-password" aria-invalid={!!errors.password} />
            <InlineError message={errors.password} />
            {!userForm.isNew && <p id="am-pwd-hint" className="mt-1 text-[11px] text-stone-500">Leave empty to keep current password.</p>}
          </div>
          <div>
            <label htmlFor="am-role" className={label}>Role{required}</label>
            <select id="am-role" value={userForm.role} onChange={event => onChange('role', event.target.value)}
              className={`${field} ${errors.role ? '!border-red-500 !ring-2 !ring-red-500/20 dark:!border-red-400 dark:!ring-red-400/20 animate-shake' : ''}`} aria-invalid={!!errors.role}>
              {roles.map(role => <option key={role.name}>{role.name}</option>)}
            </select>
            <InlineError message={errors.role} />
          </div>
          <div className="grid grid-cols-1 gap-3 sm:grid-cols-2">
            {([['full_name', 'Full name', 'text'], ['email', 'Email ID', 'email'], ['mobile', 'Mobile No', 'tel'], ['department', 'Department', 'text'],
              ['dob', 'DOB', 'date'], ['password_reset_on', 'Password reset on', 'date']] ).map(([k, caption, type]) => (
              <div key={k}>
                <label htmlFor={`am-${k}`} className={label}>{caption}</label>
                <input id={`am-${k}`} type={type} value={userForm[k]} onChange={event => onChange(k, event.target.value)}
                  className={`${field} ${errors[k] ? bad : ''}`} aria-invalid={!!errors[k]} />
                <InlineError message={errors[k]} />
                {k === 'password_reset_on' && <p className="mt-1 text-[11px] text-stone-500">From this date the user must change their password at sign-in.</p>}
              </div>
            ))}
          </div>
          <label className="flex items-center gap-2 text-xs text-stone-700 dark:text-stone-200">
            <input type="checkbox" checked={userForm.active} onChange={event => onChange('active', event.target.checked)} className="h-4 w-4 accent-accent" />
            Active (inactive users cannot log in)
          </label>
          {!userForm.active && (
            <div>
              <label htmlFor="am-inactive_reason" className={label}>Reason for inactive{required}</label>
              <input id="am-inactive_reason" value={userForm.inactive_reason} onChange={event => onChange('inactive_reason', event.target.value)}
                className={`${field} ${errors.inactive_reason ? bad : ''}`} aria-invalid={!!errors.inactive_reason} />
              <InlineError message={errors.inactive_reason} />
            </div>
          )}
          <div className="flex justify-end gap-2 border-t border-line pt-4">
            <button onClick={onCancel} className={secondaryButton}><X size={14} />Cancel</button>
            <button onClick={onSave} className={primaryButton}><Save size={14} />Save user</button>
          </div>
        </>}
      </div>
    </div>
  );
}