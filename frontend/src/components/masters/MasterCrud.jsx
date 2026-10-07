// Generic master screen (the web version of legacy frmMstGeneral): list + search on the left,
// form on the right, New / Edit / Delete / Save.
//
// Talks to the backend CRUD factory at /masters/<path> (backend/app/routers/masters.py).
// `fields` (see masterConfigs.js) describes the form; `optionsFrom` fills a select from another master.
// Keyboard: Enter/⌘S save, Esc cancel, ⌘N/Alt+N new, ↑/↓ move between fields.
// Cashiers cannot delete items (hidden here and refused with 403 by the backend).

import Pager from '../common/Pager';
import { usePage } from '../common/usePage';
import { useEffect, useState, } from 'react';
import { Edit3, FilePlus, Save, Search, Trash2, X } from 'lucide-react';
import { useConfirm } from '../common/ConfirmDialog';
import Toast from '../common/Toast';
import { toastStore } from '../common/toastStore';
import { FormErrorSummary, InlineError } from '../common/FormErrorSummary';
import { canDo, canSeeCost, createMaster, deleteMaster, errorText, fieldRight, listMaster, updateMaster } from '../../lib/api';
import { badge, field, ghostButton, label as labelClass, pageHeader, pageSub, pageTitle, panel, primaryButton, secondaryButton, sectionTitle, tableBody, tableHead } from '../../lib/styles';

 















const blank = (fields) =>
  Object.fromEntries(fields.map(f => [f.key, f.default ?? (f.type === 'bool' ? false : f.type === 'number' ? 0 : '')]));

export default function MasterCrud({ title, subtitle, path, fields: allFields, toolbar, reloadKey = 0 }



) {
  // Cost fields are left out entirely for roles that can't see costs; the backend keeps their values.
  // Cost fields for roles without cost access, and fields the role's Field Level Rights hide (Admin Master
  // (Security)), are neither shown nor sent; the server keeps their values.
  const fields = allFields.filter(f => (!f.costField || canSeeCost()) && fieldRight(path, f.key) !== 'hidden');
  const [rows, setRows] = useState([]);
  const [selected, setSelected] = useState(null);
  const [form, setForm] = useState(() => blank(fields));
  const [formErrors, setFormErrors] = useState({});
  const [mode, setMode] = useState('View');
  const [filter, setFilter] = useState('');
  const [options, setOptions] = useState({});
  const [toast, setToast] = useState(null);
  const { confirm, dialog } = useConfirm();

  // The role's Add/Edit/Delete ticks for this screen (Admin Master); the server checks them too.
  const canAdd = canDo(title, 'add');
  const canEdit = canDo(title, 'edit');
  const canDelete = canDo(title, 'delete');

  useEffect(() => {
    listMaster(path)
      .then(list => { setRows(list); if (list[0]) { setSelected(String(list[0].code)); setForm(list[0]); } })
      .catch(e => setToast({ kind: 'error', text: errorText(e) }));
    // From allFields (the prop, stable between renders), not `fields`: that is filtered anew on every render,
    // so depending on it re-ran this effect after each load and fetched the lists in an endless loop.
    allFields.filter(f => f.optionsFrom).forEach(f => {
      listMaster(f.optionsFrom ?? '')
        .then(list => setOptions(o => ({ ...o, [f.key]: list.map(r => String(r.code)) })))
        .catch(() => undefined);
    });
  }, [path, allFields, reloadKey]);

  const readOnly = mode === 'View';
  const listFields = fields.filter(f => f.showInList !== false).slice(0, 4);
  const visible = rows.filter(r => `${r.code} ${r.name ?? ''}`.toLowerCase().includes(filter.toLowerCase()));
  const pager = usePage(visible);

  const select = (row) => {
    setSelected(String(row.code));
    setForm(row);
    setFormErrors({});
    setMode('View');
  };

  const handleFieldChange = (key, value) => {
    let duplicateAlert = null;
    setForm(prev => ({ ...prev, [key]: value }));

    // When creating a new item: if entered code or short name already exists in master, show duplicate alert!
    if (mode === 'New' && (key === 'code' || key === 'short_name')) {
      const query = String(value).trim().toLowerCase();
      if (query.length > 0) {
        const match = rows.find(r => {
          const rCode = String(r.code ?? '').trim().toLowerCase();
          const rShort = String(r.short_name ?? '').trim().toLowerCase();
          return (key === 'code' && rCode === query) || (key === 'short_name' && rShort === query);
        });
        if (match) {
          duplicateAlert = `Item Master '${match.code}' already exists`;
        }
      }
    }

    if (duplicateAlert) {
      setFormErrors(prev => ({ ...prev, [key]: duplicateAlert }));
      setToast({ kind: 'error', text: duplicateAlert });
    } else if (formErrors[key]) {
      setFormErrors(prev => {
        const next = { ...prev };
        delete next[key];
        return next;
      });
    }
  };

  const focusField = (key) => {
    const el = document.getElementById(`mc-${path}-${key}`);
    if (el) {
      el.scrollIntoView({ behavior: 'smooth', block: 'center' });
      el.focus();
    }
  };

  const save = async () => {
    // Amazon-style validation: collect all missing required fields
    const newErrors = {};
    for (const f of fields) {
      if (f.required) {
        const val = String(form[f.key] ?? '').trim();
        if (val === '') {
          newErrors[f.key] = `Enter the ${f.label}`;
        }
      }
    }

    if (Object.keys(newErrors).length > 0) {
      setFormErrors(newErrors);
      // Auto-jump cursor into the first invalid box
      const firstInvalidKey = fields.find(f => newErrors[f.key])?.key;
      if (firstInvalidKey) {
        setTimeout(() => {
          const el = document.getElementById(`mc-${path}-${firstInvalidKey}`);
          if (el) {
            el.focus();
            if ('select' in el && typeof el.select === 'function') {
              el.select();
            }
          }
        }, 50);
      }
      return;
    }

    setFormErrors({});

    const body = Object.fromEntries(fields.map(f => [f.key, f.type === 'number' ? Number(form[f.key]) || 0 : form[f.key]]));
    const name = title.replace(' Master', '');
    // The save waits for its popup (Cancel stops it): see .claude/rules/popups-and-errors.md.
    if (!(await toastStore.defer('ok', `${name} "${String(form.code ?? '').trim() || 'new record'}" will be saved when this closes.`, 'Save cancelled. Nothing was changed.'))) return;
    const request = mode === 'New'
      ? createMaster(path, body)
      : updateMaster(path, selected ?? '', body);
    request.then(saved => {
      setRows(mode === 'New' ? [...rows, saved] : rows.map(r => (String(r.code) === selected ? saved : r)));
      select(saved);
      setToast({ kind: 'ok', text: `${title.replace(' Master', '')} "${saved.code}" saved.` });
    }).catch(e => {
      // Server errors stay until closed in the Walmart-style snackbar
      setToast({ kind: 'error', text: errorText(e) });
    });
  };

  const remove = async () => {
    if (!canDelete) {
      setToast({ kind: 'error', text: `Your role is not allowed to delete in ${title}.` });
      return;
    }
    if (!selected || !(await confirm(`Delete "${selected}"? This can't be undone.`))) return;
    if (!(await toastStore.defer('delete', `"${selected}" will be deleted when this closes.`, 'Delete cancelled. Nothing was removed.'))) return;
    deleteMaster(path, selected)
      .then(() => {
        const next = rows.filter(r => String(r.code) !== selected);
        setRows(next);
        if (next[0]) select(next[0]); else { setSelected(null); setForm(blank(fields)); }
        setToast({ kind: 'ok', text: `"${selected}" deleted.` });
      })
      .catch(e => setToast({ kind: 'error', text: errorText(e) }));
  };

  const cancel = () => {
    setFormErrors({});
    const current = rows.find(r => String(r.code) === selected);
    if (current) select(current); else { setForm(blank(fields)); setMode('View'); }
  };

  // Keyboard navigation for Mac & Windows
  useEffect(() => {
    const handleKeyDown = (e) => {
      if (document.querySelector('[role="dialog"]')) return;
      const isMac = navigator.platform.toUpperCase().indexOf('MAC') >= 0;
      const modKey = isMac ? e.metaKey : e.ctrlKey;

      // Enter to save when editing/adding in form
      if (e.key === 'Enter' && !readOnly) {
        const target = e.target ;
        if (target.tagName !== 'TEXTAREA' && target.tagName !== 'BUTTON') {
          e.preventDefault();
          save();
          return;
        }
      }

      // Escape to cancel
      if (e.key === 'Escape' && !readOnly) {
        e.preventDefault();
        cancel();
        return;
      }

      // Cmd+S or Ctrl+S to save
      if (modKey && e.key.toLowerCase() === 's') {
        e.preventDefault();
        if (!readOnly) save();
        return;
      }

      // Cmd+N, Ctrl+N or Alt+N for New
      if (canAdd && ((modKey && e.key.toLowerCase() === 'n') || (e.altKey && e.key.toLowerCase() === 'n'))) {
        e.preventDefault();
        setForm(blank(fields));
        setFormErrors({});
        setMode('New');
        setTimeout(() => {
          const first = fields[0]?.key;
          if (first) document.getElementById(`mc-${path}-${first}`)?.focus();
        }, 50);
        return;
      }

      // Cmd+E, Ctrl+E or Alt+E for Edit
      if (canEdit && readOnly && selected && ((modKey && e.key.toLowerCase() === 'e') || (e.altKey && e.key.toLowerCase() === 'e'))) {
        e.preventDefault();
        setFormErrors({});
        setMode('Edit');
        return;
      }

      // Arrow navigation between form inputs
      if (!readOnly) {
        const target = e.target ;
        if (target && (target.tagName === 'INPUT' || target.tagName === 'SELECT')) {
          if (e.key === 'ArrowDown' || e.key === 'ArrowUp') {
            const formInputs = Array.from(document.querySelectorAll(`[id^="mc-${path}-"]`));
            const currentIndex = formInputs.indexOf(target);
            if (currentIndex !== -1) {
              if (e.key === 'ArrowDown' && currentIndex < formInputs.length - 1) {
                e.preventDefault();
                formInputs[currentIndex + 1].focus();
              } else if (e.key === 'ArrowUp' && currentIndex > 0) {
                e.preventDefault();
                formInputs[currentIndex - 1].focus();
              }
            }
          }
        }
      }

      // Table row selection with Arrow keys in View mode
      if (readOnly && visible.length > 0) {
        const target = e.target ;
        const isTyping = target && (target.tagName === 'INPUT' || target.tagName === 'SELECT' || target.tagName === 'TEXTAREA');
        if (!isTyping) {
          if (e.key === 'ArrowDown') {
            e.preventDefault();
            const currIdx = visible.findIndex(r => String(r.code) === selected);
            const nextIdx = Math.min(visible.length - 1, currIdx + 1);
            if (nextIdx !== currIdx && visible[nextIdx]) {
              select(visible[nextIdx]);
            }
          } else if (e.key === 'ArrowUp') {
            e.preventDefault();
            const currIdx = visible.findIndex(r => String(r.code) === selected);
            const prevIdx = Math.max(0, currIdx - 1);
            if (prevIdx !== currIdx && visible[prevIdx]) {
              select(visible[prevIdx]);
            }
          }
        }
      }
    };

    window.addEventListener('keydown', handleKeyDown);
    return () => window.removeEventListener('keydown', handleKeyDown);
  });

  return (
    <div className="mx-auto max-w-7xl">
      {toast && <Toast kind={toast.kind} text={toast.text} />}
      {dialog}
      <div className={`${pageHeader} mb-5`}>
        <div>
          <h1 className={pageTitle}>{title}</h1>
          <p className={pageSub}>{subtitle}</p>
        </div>
        <div className="flex flex-wrap gap-2">
          {readOnly ? (
            <>
              {toolbar}
              <button
                onClick={remove}
                disabled={!selected || !canDelete}
                title={!canDelete ? 'Your role is not allowed to delete here' : undefined}
                className={`${ghostButton} ${!canDelete ? 'opacity-40 cursor-not-allowed hover:!text-stone-400' : 'hover:!text-red-600 dark:hover:!text-red-400'}`}
              >
                <Trash2 size={14} />Delete
              </button>
              {canEdit && <button onClick={() => { setFormErrors({}); setMode('Edit'); }} disabled={!selected} className={secondaryButton}><Edit3 size={14} />Edit</button>}
              {canAdd && <button onClick={() => { setForm(blank(fields)); setFormErrors({}); setMode('New'); }} className={primaryButton}><FilePlus size={14} />New</button>}
            </>
          ) : (
            <>
              <button onClick={cancel} className={secondaryButton}><X size={14} />Cancel</button>
              <button onClick={save} className={primaryButton}><Save size={14} />Save</button>
            </>
          )}
        </div>
      </div>

      <div className="grid grid-cols-1 gap-4 lg:grid-cols-[minmax(0,1.3fr)_minmax(0,1fr)]">
        <section aria-label={`${title} list`} className={`${panel} flex max-h-[600px] min-h-[320px] flex-col overflow-hidden`}>
          <div className="flex items-center gap-3 border-b border-line p-3">
            <div className="relative flex-1">
              <Search size={14} className="pointer-events-none absolute left-3 top-1/2 -translate-y-1/2 text-stone-400" />
              <input value={filter} onChange={e => setFilter(e.target.value)} placeholder="Search code or name…" aria-label="Search records" className={`${field} pl-8`} />
            </div>
            <span className="shrink-0 text-xs tabular-nums text-stone-500">{visible.length} of {rows.length}</span>
          </div>
          <div className="flex-1 overflow-auto">
            <table className="w-full text-xs">
              <thead className={tableHead}>
                <tr>{listFields.map((f, i) => <th key={f.key} className={`px-4 py-2.5 ${f.type === 'number' ? 'text-right' : ''} ${i > 1 ? 'hidden sm:table-cell' : ''}`}>{f.label}</th>)}</tr>
              </thead>
              <tbody className={tableBody}>
                {pager.slice.map(r => {
                  const isSel = String(r.code) === selected;
                  return (
                    <tr
                      key={String(r.code)}
                      onClick={() => mode === 'View' && select(r)}
                      onKeyDown={e => { if ((e.key === 'Enter' || e.key === ' ') && mode === 'View') { e.preventDefault(); select(r); } }}
                      tabIndex={mode === 'View' ? 0 : -1}
                      aria-selected={isSel}
                      className={`cursor-pointer outline-none transition-colors focus-visible:bg-subtle ${isSel ? 'bg-accent-soft' : 'hover:bg-subtle/70'}`}
                    >
                      {listFields.map((f, i) => (
                        <td key={f.key} className={`px-4 py-2.5 ${i > 1 ? 'hidden sm:table-cell' : ''} ${i === 0 ? `font-medium ${isSel ? 'text-accent-ink' : 'text-stone-900 dark:text-stone-50'}` : 'text-stone-600 dark:text-stone-300'} ${f.type === 'number' ? 'text-right tabular-nums' : ''}`}>
                          {f.type === 'bool' ? <span className={badge(r[f.key] ? 'success' : 'neutral')}>{r[f.key] ? 'Yes' : 'No'}</span> : String(r[f.key] ?? '')}
                          {i === 0 && r.authorised === false && <span className={`${badge('warning')} ml-2`}>Waiting for approval</span>}
                        </td>
                      ))}
                    </tr>
                  );
                })}
                {!visible.length && <tr><td colSpan={listFields.length} className="px-4 py-12 text-center text-stone-500">{filter ? `No records match “${filter}”.` : 'No records yet. Click New to add one.'}</td></tr>}
              </tbody>
            </table>
          </div>
          <Pager p={pager} />
        </section>

        <section aria-label="Record details" className={`${panel} self-start`}>
          <div className="flex items-center justify-between border-b border-line px-5 py-3.5">
            <h2 className={sectionTitle}>{mode === 'New' ? 'New record' : mode === 'Edit' ? `Editing ${selected}` : selected ?? 'Details'}</h2>
            {readOnly
              ? <span className={badge('neutral')}>View</span>
              : <span className="text-[11px] text-stone-500"><span className="text-red-600">*</span> required</span>}
          </div>

          <div className="p-5">
            {/* Amazon-style "There was a problem" alert summary box */}
            {!readOnly && <FormErrorSummary errors={formErrors} onFocusField={focusField} />}

            <div className={`grid gap-4 ${fields.length > 8 ? 'sm:grid-cols-2' : ''}`}>
              {fields.map(f => {
                // Read-only by Field Level Rights: can be filled in for a new record, not changed afterwards.
                const locked = readOnly || ((f.key === 'code' || f.newOnly || fieldRight(path, f.key) === 'readonly') && mode === 'Edit');
                const value = form[f.key];
                const opts = f.options ?? options[f.key] ?? [];
                const id = `mc-${path}-${f.key}`;
                const hasError = !readOnly && !!formErrors[f.key];

                return (
                  <div key={f.key}>
                    {readOnly ? (
                      <>
                        <div className="mb-1 text-xs font-medium text-stone-500 dark:text-stone-400">{f.label}</div>
                        <div className={`min-h-5 text-sm text-stone-900 dark:text-stone-50 ${f.type === 'number' ? 'tabular-nums' : ''}`}>
                          {f.type === 'bool'
                            ? <span className={badge(value ? 'success' : 'neutral')}>{value ? 'Yes' : 'No'}</span>
                            : String(value ?? '') === '' ? <span className="text-stone-400">—</span> : String(value)}
                        </div>
                      </>
                    ) : f.type === 'bool' ? (
                      <label htmlFor={id} className="flex items-center gap-2 pt-1 text-xs font-medium text-stone-700 dark:text-stone-200">
                        <input id={id} type="checkbox" checked={Boolean(value)} onChange={e => handleFieldChange(f.key, e.target.checked)} className="h-4 w-4 accent-accent" />
                        {f.label}
                      </label>
                    ) : (
                      <>
                        <label htmlFor={id} className={labelClass}>{f.label}{f.required && <span className="ml-0.5 text-red-600" aria-hidden="true">*</span>}</label>
                        {f.type === 'select' || f.optionsFrom ? (
                          <select
                            id={id}
                            required={f.required}
                            value={String(value ?? '')}
                            onChange={e => handleFieldChange(f.key, e.target.value)}
                            aria-invalid={hasError}
                            aria-errormessage={hasError ? `err-${path}-${f.key}` : undefined}
                            className={`${field} ${hasError ? '!border-red-500 !ring-2 !ring-red-500/20 dark:!border-red-400 dark:!ring-red-400/20 animate-shake' : ''}`}
                          >
                            <option value="">Select…</option>
                            {opts.map(o => <option key={o}>{o}</option>)}
                          </select>
                        ) : (
                          <>
                            <input
                              id={id}
                              type={f.type === 'number' ? 'number' : f.type === 'date' ? 'date' : 'text'}
                              readOnly={locked}
                              disabled={locked}
                              required={f.required}
                              value={String(value ?? '')}
                              list={path === 'items' && f.key === 'code' ? 'mc-items-code-list' : path === 'items' && f.key === 'short_name' ? 'mc-items-shortname-list' : undefined}
                              onChange={e => handleFieldChange(f.key, e.target.value)}
                              aria-invalid={hasError}
                              aria-errormessage={hasError ? `err-${path}-${f.key}` : undefined}
                              className={`${field} ${f.type === 'number' ? 'text-right tabular-nums' : ''} ${hasError ? '!border-red-500 !ring-2 !ring-red-500/20 dark:!border-red-400 dark:!ring-red-400/20 animate-shake' : ''}`}
                            />
                            {path === 'items' && f.key === 'code' && (
                              <datalist id="mc-items-code-list">
                                {rows.map(r => (
                                  <option key={String(r.code)} value={String(r.code)}>
                                    {String(r.name || '')}
                                  </option>
                                ))}
                              </datalist>
                            )}
                            {path === 'items' && f.key === 'short_name' && (
                              <datalist id="mc-items-shortname-list">
                                {rows.filter(r => r.short_name).map(r => (
                                  <option key={String(r.short_name)} value={String(r.short_name)}>
                                    {String(r.name || '')}
                                  </option>
                                ))}
                              </datalist>
                            )}
                          </>
                        )}
                        <InlineError message={formErrors[f.key]} id={`err-${path}-${f.key}`} />
                      </>
                    )}
                  </div>
                );
              })}
            </div>
          </div>
        </section>
      </div>
    </div>
  );
}
