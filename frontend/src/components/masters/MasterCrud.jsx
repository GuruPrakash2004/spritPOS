// Generic master screen (the web version of legacy frmMstGeneral): list + search on the left,
// form on the right, New / Edit / Delete / Save (Cancel / Save are at the top and again at the bottom of the form).
//
// Talks to the backend CRUD factory at /masters/<path> (backend/app/routers/masters.py).
// `fields` (see masterConfigs.js) describes the form; `optionsFrom` fills a select from another master.
// Keyboard: Enter/⌘S save, Esc cancel, ⌘N/Alt+N new, ↑/↓ move between fields, Alt+Home / Alt+PageUp / Alt+PageDown / Alt+End first / previous / next / last record (also the arrow buttons).
// Cashiers cannot delete items (hidden here and refused with 403 by the backend).

import GroupBox from '../common/GroupBox';
import { FIELD_RULES, cleanInput, fieldProblem } from '../../lib/validation';
import { useCallback, useEffect, useMemo, useRef, useState, } from 'react';
import ActionBar, { ScreenFrame } from '../common/ActionBar';
import AliasGrid from './AliasGrid';
import { cleanAlias } from '../../lib/aliases';
import ListModal from '../common/ListModal';
import { goHome, openModule } from '../../lib/nav';
import LookupSelect from '../common/LookupSelect';
import PartyRecords from './PartyRecords';
import AttachmentsDialog from './AttachmentsDialog';
import { getDesignerNextCode, getDesignerPickup, getItemLocks, getSimilarMasters } from '../../lib/api';
import Modal from '../common/Modal';
import { useScreenFields } from '../../hooks/useScreenFields';
import { compute, isSupported } from '../../lib/formulas';
import { applyDefinition, defaultFor, extraProblem, fieldsFromDefinition, pickupsAfter, ruleHolds, ruleValues, valueOf, withValue } from '../../lib/designerFields';
import { useConfirm } from '../common/ConfirmDialog';
import Toast from '../common/Toast';
import { toastStore } from '../common/toastStore';
import { FormErrorSummary, InlineError } from '../common/FormErrorSummary';
import { canDo, canSeeCost, createMaster, deleteMaster, errorText, fieldRight, listMaster, updateMaster } from '../../lib/api';
import { cn } from '../../lib/utils';
import { badge, field, fieldTone, label as labelClass, panel, sectionTitle } from '../../lib/styles';

// vLQMS.exe's own compiled field set for these three forms (frmMstItem / frmMstParty), mirroring
// backend/app/schema_meta.py's compiled_columns() exactly, by lower-case database column name (`f.column`).
// A field whose column is NOT in this set never appeared on the exe's main form at all — MRP, GST %, ROL/ROQ
// etc. are typed in this web app's own schema, but on the real form they only ever lived in the Additional
// Fields grid (tbAdnlFlds) on the right, same as a genuinely "extra" designer column.
const LEGACY_FORM_COLUMNS = {
  'Item Master': new Set(['itemcode', 'itemname', 'itemseries', 'itemno', 'inactive', 'inactivermks', 'shortname', 'hotkey', 'hsncode', 'itemgroupid', 'liqtype', 'nonstkable', 'uomid', 'botpercases']),
  'Supplier Master': new Set(['suppcode', 'suppname', 'suppseries', 'suppno', 'inactive', 'inactivermks', 'suppaliasname', 'badd1', 'badd2', 'badd3', 'badd4', 'city', 'state', 'pincode', 'country', 'remarks', 'suppgroupid', 'ledgroupid']),
  'Customer Master': new Set(['custcode', 'custname', 'custseries', 'custno', 'inactive', 'inactivermks', 'custaliasname', 'badd1', 'badd2', 'badd3', 'badd4', 'city', 'state', 'pincode', 'country', 'remarks', 'custgroupid', 'ledgroupid']),
};

 















const blank = (fields) => {
  const base = Object.fromEntries(fields.filter(f => !f.extra).map(f => [f.key, f.default ?? (f.type === 'bool' ? false : f.type === 'lines' ? [] : f.type === 'number' ? 0 : '')]));
  const extras = Object.fromEntries(fields.filter(f => f.extra).map(f => [f.key, f.default ?? (f.type === 'bool' ? false : '')]));
  // Designer defaults, and defaults that depend on another field (vLQMS.exe: "default against condition").
  const values = { ...base, ...extras };
  for (const f of fields) {
    if (!f.useDefault && !f.defaultConditions) continue;
    const v = defaultFor(f, values);
    if (v === undefined) continue;
    if (f.extra) extras[f.key] = v; else base[f.key] = f.type === 'number' ? Number(v) || 0 : v;
    values[f.key] = v;
  }
  // Columns added in the database (read live, see hooks/useScreenFields.js) live in `extras`.
  return { ...base, extras };
};

export default function MasterCrud({ title, path, fields: baseFields = [], options: extraOptions = [], reloadKey = 0, onSaved, newDefaults, startNew = false }



) {
  // The screen follows the database: columns the Doc Designer / the table have beyond `baseFields` are added after
  // them, and the set changes when the database does. The array keeps its identity while nothing changed.
  const live = useScreenFields(path);
  const allFields = useMemo(() => {
    // The database's definition decides the whole screen; the field list given by the caller is only the fallback for
    // when there is no definition (offline, no Doc Designer rows for this master).
    const built = fieldsFromDefinition(live.screen);
    if (built) return built;
    const typed = applyDefinition(baseFields, live.screen);
    return live.fields.length ? [...typed, ...live.fields] : typed;
  }, [baseFields, live.fields, live.screen]);
  // Cost fields are left out entirely for roles that can't see costs; the backend keeps their values.
  // Cost fields for roles without cost access, and fields the role's Field Level Rights hide (Admin Master
  // (Security)), are neither shown nor sent; the server keeps their values.
  const fields = allFields.filter(f => (!f.costField || canSeeCost()) && fieldRight(path, f.key) !== 'hidden');
  const [rows, setRows] = useState([]);
  const [loaded, setLoaded] = useState(false);   // false until the first list arrives: skeleton rows, not "No records"
  const [selected, setSelected] = useState(null);
  const [listOpen, setListOpen] = useState(false);   // the List window (ListModal)
  const [attachOpen, setAttachOpen] = useState(false);   // File Attachment (the desktop's button on every master)
  const [partyOpen, setPartyOpen] = useState(false);   // customer / supplier: the records kept beside the master
  const [form, setForm] = useState(() => blank(fields));
  const [formErrors, setFormErrors] = useState({});
  const [mode, setMode] = useState('View');
  const modeRef = useRef(mode);
  useEffect(() => { modeRef.current = mode; }, [mode]);
  const [options, setOptions] = useState({});
  // The exe's item and party forms share these behaviours: Copy From, the close prompt, F5, Ctrl+A; the party forms also fill
  // Country and State from the chosen City (State City Country master) and copy the name into the alias.
  const partyForm = path === 'customers' || path === 'suppliers';
  const exeForm = path === 'items' || partyForm;
  const [scc, setScc] = useState([]);   // State City Country rows: { code: city, name: state, country }
  useEffect(() => { if (partyForm) listMaster('state-city-country').then(setScc).catch(() => undefined); }, [partyForm]);
  // Item Master only (frmMstItem): the UOM box locks when the item is on a transaction; Copy From and the similar-code list are dialogs.
  const [uomLocked, setUomLocked] = useState(false);
  const [copyOpen, setCopyOpen] = useState(false);
  const [copyFilter, setCopyFilter] = useState('');
  const [similar, setSimilar] = useState(null);
  const lastSimilar = useRef('');
  const [toast, setToast] = useState(null);
  const { confirm, dialog } = useConfirm();

  // The role's Add/Edit/Delete ticks for this screen (Admin Master); the server checks them too.
  const canAdd = canDo(title, 'add');
  const canEdit = canDo(title, 'edit');
  const canDelete = canDo(title, 'delete');

  useEffect(() => {
    listMaster(path)
      .then(list => {
        setRows(list);
        setLoaded(true);
        // Read again because the database changed (live.version): stay on the record being looked at.
        setSelected(cur => (cur && list.some(r => String(r.code) === cur) ? cur : list[0] ? String(list[0].code) : null));
        setForm(cur => {
          const same = cur?.code != null && list.find(r => String(r.code) === String(cur.code));
          if (modeRef.current !== 'View') return cur;   // a record being typed is never replaced
          return same && cur.code ? same : list[0] ?? cur;
        });
      })
      .catch(e => { setLoaded(true); setToast({ kind: 'error', text: errorText(e) }); });
  }, [path, reloadKey, live.version]);

  const [optionsKey, setOptionsKey] = useState(0);
  useEffect(() => {
    // From allFields (stable between renders), not `fields`: that is filtered anew on every render,
    // so depending on it re-ran this effect after each load and fetched the lists in an endless loop.
    allFields.filter(f => f.optionsFrom).forEach(f => {
      listMaster(f.optionsFrom ?? '')
        .then(list => setOptions(o => ({ ...o, [f.key]: list.filter(r => r.active !== false).map(r => String(r[f.optionKey ?? 'code'])) })))
        .catch(() => undefined);
    });
  }, [allFields, reloadKey, optionsKey]);

  const withDefaults = useCallback((list) => {
    const form0 = blank(list);
    const extras = { ...form0.extras };
    const typed = {};
    for (const [k, v] of Object.entries(newDefaults ?? {})) {
      const extraKey = Object.keys(extras).find(x => x.toLowerCase() === k.toLowerCase());
      if (list.some(f => f.key === k && !f.extra)) typed[k] = v; else if (extraKey) extras[extraKey] = v; else typed[k] = v;
    }
    return { ...form0, ...typed, extras };
  }, [newDefaults]);
  const startNewRecord = () => {
    setForm(withDefaults(fields));
    setFormErrors({});
    setMode('New');
    setTimeout(() => { const first = fields[0]?.key; if (first) document.getElementById(`mc-${path}-${first}`)?.focus(); }, 50);
  };
  const startEditRecord = () => {
    setFormErrors({});
    setMode('Edit');
    if (path === 'items' && selected) {   // Edit: the UOM box is disabled when the item is on any transaction (CheckforItemUsedInTrn)
      setUomLocked(false);
      getItemLocks(selected).then(r => setUomLocked(Boolean(r.uom_locked))).catch(() => undefined);
    }
  };

  const startedNew = useRef(false);
  useEffect(() => {
    // Opened to add a record (the Purchase Bill's New supplier button): start with a blank form once the list is here.
    if (!startNew || !loaded || startedNew.current || !fields.length) return;
    startedNew.current = true;
    setForm(withDefaults(fields));
    setMode('New');
  }, [startNew, loaded, fields, withDefaults]);

  const readOnly = mode === 'View';
  const isRemarks = f => f.extra && String(f.key).toLowerCase() === 'inactivermks';   // Not Active Remarks (a database column)
  const inactiveNow = form.active === false || form.inactive === true;
  const listFields = fields.filter(f => !f.designerHidden && f.showInList !== false).slice(0, 4);
  // Record order for the record-navigation buttons (the desktop's Move First / Previous / Next / Last) and the List
  // window's search (the record picker beside the form is gone — the bottom bar's List button covers the same job).
  const visible = rows;

  const select = (row) => {
    setSelected(String(row.code));
    setForm(row);
    setFormErrors({});
    setMode('View');
  };

  // Record navigation (the desktop's Move First / Previous / Next / Last): through the records in the list's order (the search
  // filter applies), showing the page the record is on.
  const selectedAt = visible.findIndex(r => String(r.code) === selected);
  const goTo = (where) => {
    if (!visible.length) return;
    const to = where === 'first' ? 0 : where === 'last' ? visible.length - 1
      : where === 'next' ? Math.min(visible.length - 1, selectedAt + 1) : Math.max(0, selectedAt - 1);
    select(visible[to]);
  };

  // The designer's formula fields (e.g. Item Selling Rate = (MRP - Comp.Cess) / (100 + CGST + SGST + Cess) * 100) are worked out again
  // as soon as a field they use changes, as the desktop does while typing.
  const withFormulas = (next) => {
    const formulaFields = allFields.filter(f => f.formula && isSupported(f.formula));
    if (!formulaFields.length) return next;
    const codeOf = f => String(f.column ?? f.key);
    const inputs = Object.fromEntries(allFields.filter(f => !f.formula).map(f => [codeOf(f), valueOf(next, f)]));
    const worked = compute(formulaFields.map(f => ({ code: codeOf(f), formula: f.formula, decimals: f.decimals ?? 2, section: 'Header', type: 'number' })), inputs, [], {});
    let out = next;
    for (const f of formulaFields) {
      const v = worked.values[codeOf(f).toLowerCase()] ?? worked.values[codeOf(f)];
      if (typeof v === 'number' && Number.isFinite(v)) out = withValue(out, f, f.type === 'number' ? v : String(v));
    }
    return out;
  };

  const handleFieldChange = (key, rawValue) => {
    if (path === 'items' && typeof rawValue === 'string') rawValue = rawValue.replace(/'/g, '');   // the exe's boxes refuse an apostrophe
    const extraField = allFields.find(f => f.extra && f.key === key);
    const value = extraField ? rawValue : cleanInput(key, rawValue);   // phone / PIN / GSTIN ... keep only what is allowed
    let duplicateAlert = null;
    setForm(prev => withFormulas(extraField ? withValue(prev, extraField, value) : { ...prev, [key]: value }));
    // A master that generates its codes (MstAutoGenCode): choosing the series fills the next number and the code (series + 6 digits).
    if (live.screen?.flags?.auto_code && mode === 'New' && extraField && /series$/i.test(String(extraField.column ?? ''))) {
      const noField = allFields.find(f => f.extra && /no$/i.test(String(f.column ?? '')) && !/series$/i.test(String(f.column)));
      getDesignerNextCode(live.screen.doc_id, String(value).trim()).then(r => setForm(prev => {
        const next = { ...prev, code: r.code || prev.code };
        return noField ? withValue(next, noField, r.no || '') : next;
      })).catch(() => undefined);
    }
    // Pick-up fields (the designer's FldPickUP SQL) take their value from the database once the field they follow is edited.
    const edited = allFields.find(f => f.key === key);
    const followers = edited ? pickupsAfter(allFields, edited) : [];
    if (followers.length && live.screen?.doc_id) {
      const values = { ...Object.fromEntries(allFields.map(f => [f.column ?? f.key, valueOf(form, f)])), [edited.column ?? key]: value };
      followers.forEach(f => getDesignerPickup(live.screen.doc_id, f.column ?? f.key, values)
        .then(r => setForm(prev => withValue(prev, f, r.value ?? ''))).catch(() => undefined));
    }

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

  // frmMstItem LostFocus / Validating: in Add mode leaving Item Code copies it into Item Name, leaving Item Name copies it into HotKey;
  // leaving Item Code asks about similar codes (ItemCodeClean), unless codes are generated.
  const handleBlur = async (f) => {
    if (mode === 'View') return;
    const colOf = x => String(x.column ?? x.key).toLowerCase();
    if (exeForm && f.key === 'code' && mode === 'New') setForm(prev => ({ ...prev, name: String(prev.code ?? '') }));
    if (path === 'items' && f.key === 'name' && mode === 'New') {
      const hot = allFields.find(x => x.extra && colOf(x) === 'hotkey');
      if (hot) setForm(prev => withValue(prev, hot, String(prev.name ?? '')));
    }
    if (partyForm && f.key === 'name') {   // frmMstParty.txtPartyName_Validated: the Tally alias follows the name
      const alias = allFields.find(x => /alias(name)?$/.test(colOf(x)) && !x.designerHidden);
      if (alias) setForm(prev => withValue(prev, alias, String(prev.name ?? '')));
    }
    if (partyForm && colOf(f) === 'city') {   // frmMstParty.cboCity_Validating: Country and State follow the city; an unknown city offers to add it
      const city = String(valueOf(form, f) ?? '').trim();
      if (city) {
        const hit = scc.find(r => String(r.code).toLowerCase() === city.toLowerCase());
        if (hit) {
          const fill = (prev, col, v) => { const t = allFields.find(x => colOf(x) === col && !x.designerHidden); return t ? withValue(prev, t, v) : prev; };
          setForm(prev => fill(fill(prev, 'country', String(hit.country ?? '')), 'state', String(hit.name ?? '')));
        } else if (await confirm('City Not found! Do you want to add city ??')) {
          setForm(prev => withValue(prev, f, ''));
          openModule('State City Country');
        }
      }
    }
    if (f.key === 'code' && path !== 'holidays' && path !== 'state-city-country' && !live.screen?.flags?.auto_code) {
      const code = String(form.code ?? '').trim();
      if (!code || lastSimilar.current === code) return;
      lastSimilar.current = code;
      try {
        const list = await getSimilarMasters(path, code, mode === 'Edit' ? selected ?? '' : '');
        if (list.length && await confirm(`Similar ${path === 'items' ? 'ItemCode' : 'code'} already exists. Do you want to view them?`)) setSimilar({ code, list });
      } catch { /* the check is a courtesy: never block typing */ }
    }
  };

  // Copy From (Add mode): take every detail of another item except its code.
  const copyFrom = (row) => {
    setForm(prev => ({ ...row, code: prev.code, qoh: prev.qoh, authorised: undefined }));
    setCopyOpen(false);
    setCopyFilter('');
  };

  // frmMstParty.lblAddress_DoubleClick: the party's name and address lines, city, state and PIN on the clipboard.
  const copyAddress = async () => {
    const col = (c) => { const x = allFields.find(y => String(y.column ?? y.key).toLowerCase() === c && !y.designerHidden); return x ? String(valueOf(form, x) ?? '').trim() : ''; };
    const lines = [form.name, col('badd1') || form.address, col('badd2'), col('badd3'), col('badd4'), col('city') || form.city, col('state') || form.state, col('pincode') || form.pin]
      .map(x => String(x ?? '').trim()).filter(Boolean);
    try { await navigator.clipboard.writeText(lines.join('\n')); setToast({ kind: 'ok', text: 'Address copied to clipboard.' }); }
    catch { setToast({ kind: 'error', text: 'The browser would not copy to the clipboard.' }); }
  };

  // Closing while adding or editing asks first (the exe's FormClosing).
  const closeScreen = async () => {
    if (exeForm && mode !== 'View' && !(await confirm('Are you sure you want to exit the form without saving?!'))) return;
    goHome();
  };

  const focusField = (key) => {
    const target = fields.find(f => f.key === key);
    if (target) setActiveTab(target.tab ?? '');   // the field may be on another tab
    setTimeout(() => {
      const el = document.getElementById(`mc-${path}-${key}`);
      if (el) {
        el.scrollIntoView({ behavior: 'smooth', block: 'center' });
        el.focus();
      }
    }, 0);
  };

  const save = async () => {
    // Amazon-style validation: collect all missing required fields
    const newErrors = {};
    for (const f of fields) {
      if (f.designerHidden) continue;   // not shown, so nothing to ask for
      if (f.extra) {   // a column added in the database: its rules come from the Doc Designer row
        const wrong = extraProblem(f, form);
        if (wrong) newErrors[f.key] = wrong;
        continue;
      }
      const rules = ruleValues(form);
      if (f.hideWhen && ruleHolds(f.hideWhen, rules)) continue;   // the designer hides it under this condition
      if (f.required || (f.mandatoryWhen && ruleHolds(f.mandatoryWhen, rules))) {
        const val = String(form[f.key] ?? '').trim();
        if (val === '') {
          newErrors[f.key] = `Enter the ${f.label}`;
        }
      }
      if (!newErrors[f.key]) {
        const wrong = fieldProblem(f.key, form[f.key]);   // 10-digit phone, 6-digit PIN, GSTIN / PAN / HSN patterns, tax % ...
        if (wrong) newErrors[f.key] = wrong;
      }
    }

    // vLQMS.exe: "Please enter remarks for why the record is inactive!" (the Not Active Remarks column).
    const remarksField = fields.find(isRemarks);
    const isInactive = inactiveNow;
    if (remarksField && isInactive && !String(valueOf(form, remarksField) ?? '').trim()) newErrors[remarksField.key] = 'Enter the reason this record is inactive';

    // The exe's alias grid (frmMstItem.ValidFields): no alias twice, and none may be the item's own code.
    const aliasField = fields.find(f => f.type === 'lines' && f.key === 'aliases');
    if (aliasField) {
      const seen = new Set();
      for (const a of (form[aliasField.key] ?? []).map(x => String(x).trim()).filter(Boolean)) {
        if (seen.has(a.toLowerCase())) { newErrors[aliasField.key] = `Duplicate alias '${a}'`; break; }
        seen.add(a.toLowerCase());
      }
      if (!newErrors[aliasField.key] && seen.has(String(form.code ?? '').trim().toLowerCase())) newErrors[aliasField.key] = 'Item code and alias cannot be the same';
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

    const body = Object.fromEntries(fields.filter(f => !f.extra).map(f => [f.key, f.type === 'number' ? Number(form[f.key]) || 0 : f.type === 'lines' ? (form[f.key] ?? []).map(cleanAlias).map(x => x.trim()).filter(Boolean) : form[f.key]]));
    // Only the extra columns still in the definition are sent: one dropped from the database is left out.
    // On a new record, boxes left empty are not sent, so the table's own defaults stay.
    body.extras = Object.fromEntries(fields
      .filter(f => f.extra && !f.lockedAlways && !(f.hideWhen && ruleHolds(f.hideWhen, ruleValues(form))))
      .map(f => [f.key, isRemarks(f) && !inactiveNow ? '' : valueOf(form, f) ?? ''])   // reactivated: the reason is cleared, as in the exe
      .filter(([, v]) => mode !== 'New' || String(v).trim() !== ''));
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
      onSaved?.(saved);
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

      // The item form's own keys (frmMstItem.KeyUp): F5 refills the group / UOM lists, Ctrl+A opens File Attachment (outside a text box).
      if (exeForm && e.key === 'F5') { e.preventDefault(); setOptionsKey(k => k + 1); if (partyForm) listMaster('state-city-country').then(setScc).catch(() => undefined); return; }
      if (exeForm && modKey && e.key.toLowerCase() === 'a' && mode !== 'New' && selected
        && !['INPUT', 'TEXTAREA', 'SELECT'].includes(e.target?.tagName)) { e.preventDefault(); setAttachOpen(true); return; }

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

      // Cmd+N / Ctrl+N for New, Cmd+E / Ctrl+E for Edit (Alt+N / Alt+E are the action bar's)
      if (canAdd && modKey && e.key.toLowerCase() === 'n') { e.preventDefault(); startNewRecord(); return; }
      if (canEdit && readOnly && selected && modKey && e.key.toLowerCase() === 'e') { e.preventDefault(); startEditRecord(); return; }

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

    };

    window.addEventListener('keydown', handleKeyDown);
    return () => window.removeEventListener('keydown', handleKeyDown);
  });

  // The legacy form's layout: the web app's own (typed) fields usually sit on the left; the designer's extra
  // fields (vLQMS.exe's "Additional Fields" grid, tbAdnlFlds) live in a persistent "Additional Information" panel
  // on the right, one tab per SubSectionName — the first, nameless one is "Header Info" (designerFields.js).
  // `f.standard` extra fields are vLQMS.exe's own fixed fields for this form (compiled_columns in
  // schema_meta.py — e.g. Item Master's HotKey, InActiveRmks) that just aren't modelled as typed columns
  // here; they belong with the typed fields on the left, not the "Additional Fields" grid on the right.
  // When the exact legacy column set is known (LEGACY_FORM_COLUMNS, title match) it overrides both of
  // those: a field outside it never appeared on the exe's own form, however this web app models it.
  const legacyFormColumns = LEGACY_FORM_COLUMNS[title];
  const onLegacyForm = f => legacyFormColumns && f.column && legacyFormColumns.has(String(f.column).toLowerCase());
  const rightFields = fields.filter(f => !f.designerHidden && ((f.tab ?? '') !== '' ||
    (legacyFormColumns && f.column ? !onLegacyForm(f) : (f.extra && !f.standard))));
  const leftFields = fields.filter(f => !f.designerHidden && (f.tab ?? '') === '' &&
    (legacyFormColumns && f.column ? onLegacyForm(f) : (!f.extra || f.standard)));
  const tabNames = [...new Set(rightFields.map(f => f.tab ?? ''))];
  const [activeTab, setActiveTab] = useState('');
  const shownTab = tabNames.includes(activeTab) ? activeTab : (tabNames[0] ?? '');
  const groupBySection = list => {
    const out = [];
    for (const f of list) {
      const title = f.section ?? '';
      const last = out.at(-1);
      if (last && last.title === title) last.fields.push(f); else out.push({ title, fields: [f] });
    }
    return out;
  };
  // The legacy Windows screens these three masters are modelled on (frmMstItem / frmMstParty) use a dark
  // header bar over a light-blue form body; reuse colours already in lib/styles.js (the navy ActionBar/
  // Sidebar accent, the `info` tone's light blue) rather than inventing a new palette.
  const legacySkin = title === 'Item Master' || title === 'Supplier Master' || title === 'Customer Master';
  // The legacy main form has no group-box headers of its own either — just one continuous set of fields.
  const leftSections = legacySkin ? [{ title: '', fields: leftFields }] : groupBySection(leftFields);
  const shownRightFields = rightFields.filter(f => (f.tab ?? '') === shownTab);
  // The legacy grid (vLQMS.exe's tbAdnlFlds) is one flat table under the tab, not sub-boxed by section.
  const rightSections = legacySkin ? [{ title: '', fields: shownRightFields }] : groupBySection(shownRightFields);

  const barOptions = [
    ...(path === 'customers' || path === 'suppliers' ? [{ label: 'Contacts, addresses, alerts, notes', onClick: () => setPartyOpen(true), disabled: !selected }] : []),
    ...(exeForm ? [{ label: 'Copy From', onClick: () => setCopyOpen(true), disabled: mode !== 'New' }] : []),
    ...(partyForm ? [{ label: 'Copy Address', onClick: () => void copyAddress(), disabled: !selected && mode === 'View' }] : []),
    { label: 'File Attachment', onClick: () => setAttachOpen(true), disabled: !selected },
    ...extraOptions,
  ];
  const listColumns = fields.filter(f => !f.designerHidden && f.type !== 'lines').map(f => ({
    key: f.key, label: f.label, kind: f.type === 'number' ? 'number' : f.type === 'bool' ? 'bool' : 'text', value: r => valueOf(r, f),
  }));
  const bar = (
    <ActionBar
      mode={mode} onNew={canAdd ? startNewRecord : undefined} onEdit={canEdit ? startEditRecord : undefined} onDelete={() => void remove()} onList={() => setListOpen(true)} onClose={() => void closeScreen()}
      onSave={() => void save()} onCancel={cancel}
      canNew={canAdd} canEdit={canEdit && !!selected} canDelete={canDelete && !!selected}
      noEditReason={!canEdit ? undefined : 'Select a record first'} noDeleteReason={!canDelete ? undefined : 'Select a record first'}
      options={barOptions}
      nav={visible.length ? {
        onFirst: () => goTo('first'), onPrev: () => goTo('prev'), onNext: () => goTo('next'), onLast: () => goTo('last'), onFind: () => setListOpen(true),
        atStart: selectedAt <= 0, atEnd: selectedAt === visible.length - 1, label: `${Math.max(selectedAt, 0) + 1} / ${visible.length}`,
      } : { onFind: () => setListOpen(true), atStart: true, atEnd: true }}
    />
  );

  // The value/control for one field, with no opinion on where its label goes — shared by the left column's
  // stacked blocks and the "Additional Information" panel's compact rows (vLQMS.exe's right-hand tab grid).
  // The legacy form's inputs sit on light-blue fill, not white, against its light-blue body.
  const fieldCls = legacySkin ? cn(field, '!bg-[#FFFCF7] dark:!bg-[#1A1A1A]') : field;
  const renderFieldControl = (f, ctx) => {
    const { id, value, locked, hasError, required, opts, compact } = ctx;
    if (readOnly) {
      return f.type === 'lines'
        ? <span className="whitespace-pre-line">{(Array.isArray(value) ? value : []).join('\n') || '—'}</span>
        : f.type === 'lookup'
        ? <LookupSelect docId={f.lookup.docId} code={f.lookup.code} value={value} disabled onChange={() => undefined} className="w-full cursor-default appearance-none border-0 bg-transparent p-0 text-sm" />
        : f.type === 'bool'
        ? <span className={badge(value ? 'success' : 'neutral')}>{value ? 'Yes' : 'No'}</span>
        : String(value ?? '') === '' ? <span className="text-stone-400">—</span> : String(value);
    }
    if (f.type === 'bool') {
      // The exe's check box is "Inactive" (chkInactive): shown ticked when the record is not active.
      const flipped = f.key === 'active';
      return <input id={id} type="checkbox" checked={flipped ? !value : Boolean(value)} onChange={e => handleFieldChange(f.key, flipped ? !e.target.checked : e.target.checked)} className="h-4 w-4 accent-accent" />;
    }
    const errorClass = hasError ? '!border-red-500 !ring-2 !ring-red-500/20 dark:!border-red-400 dark:!ring-red-400/20 animate-shake' : '';
    return (
      <>
        {f.type === 'lines' ? (
          <AliasGrid id={id} value={value} locked={locked} hasError={hasError} onChange={v => handleFieldChange(f.key, v)}
            confirmRemove={name => confirm(`Do You want to Remove Alias Name --> ${name} ?`)} />
        ) : f.type === 'lookup' ? (
          <LookupSelect id={id} docId={f.lookup.docId} code={f.lookup.code} value={value} disabled={locked}
            onChange={v => handleFieldChange(f.key, v)} invalid={hasError} className={`${fieldCls} ${errorClass}`} />
        ) : f.type === 'select' || f.optionsFrom ? (
          <>
          <select id={id} required={required} disabled={(f.extra || (path === 'items' && f.key === 'uom')) && locked} value={String(value ?? '')} onChange={e => handleFieldChange(f.key, e.target.value)}
            aria-invalid={hasError} aria-errormessage={hasError ? `err-${path}-${f.key}` : undefined} className={`${fieldCls} ${errorClass}`}>
            <option value="">Select…</option>
            {opts.map(o => <option key={o}>{o}</option>)}
          </select>
          {path === 'items' && (f.key === 'group' || f.key === 'uom') && (
            <button type="button" onClick={() => openModule(f.key === 'group' ? 'Item Group Master' : 'UOM Master')} className="mt-1 text-[11px] font-medium text-accent hover:underline"
              title={`Open ${f.key === 'group' ? 'Item Group' : 'UOM'} Master`}>Open {f.key === 'group' ? 'Item Group' : 'UOM'} Master</button>
          )}
          </>
        ) : f.extra && f.multiline ? (
          <textarea id={id} rows={compact ? 2 : 3} readOnly={locked} disabled={locked} maxLength={f.maxLength} value={String(value ?? '')}
            onChange={e => handleFieldChange(f.key, e.target.value)} aria-invalid={hasError} aria-errormessage={hasError ? `err-${path}-${f.key}` : undefined}
            className={`${fieldCls} resize-y ${errorClass}`} />
        ) : (
          <>
            <input
              id={id}
              type={f.type === 'number' ? 'number' : f.type === 'date' ? 'date' : 'text'}
              step={f.extra && f.type === 'number' ? (f.decimals > 0 ? 10 ** -f.decimals : 1) : undefined}
              readOnly={locked}
              disabled={locked}
              maxLength={f.extra ? f.maxLength : FIELD_RULES[f.key]?.max}
              inputMode={FIELD_RULES[f.key]?.digits ? 'numeric' : undefined}
              required={required}
              value={String(value ?? '')}
              list={path === 'items' && f.key === 'code' ? 'mc-items-code-list' : path === 'items' && f.key === 'short_name' ? 'mc-items-shortname-list' : partyForm && ['city', 'state', 'country'].includes(String(f.column ?? f.key).toLowerCase()) ? `mc-scc-${String(f.column ?? f.key).toLowerCase()}` : undefined}
              onChange={e => handleFieldChange(f.key, e.target.value)}
              onBlur={() => void handleBlur(f)}
              aria-invalid={hasError}
              aria-errormessage={hasError ? `err-${path}-${f.key}` : undefined}
              className={cn(fieldCls, f.type === 'number' ? 'text-right tabular-nums' : '', f.lockedAlways && 'text-blue-700 dark:text-blue-300', errorClass)}
            />
            {partyForm && ['city', 'state', 'country'].includes(String(f.column ?? f.key).toLowerCase()) && (
              <datalist id={`mc-scc-${String(f.column ?? f.key).toLowerCase()}`}>
                {[...new Set(scc.map(r => String(String(f.column ?? f.key).toLowerCase() === 'city' ? r.code : String(f.column ?? f.key).toLowerCase() === 'state' ? r.name : r.country ?? '')))].filter(Boolean).map(v => <option key={v} value={v} />)}
              </datalist>
            )}
            {path === 'items' && f.key === 'code' && (
              <datalist id="mc-items-code-list">
                {rows.map(r => <option key={String(r.code)} value={String(r.code)}>{String(r.name || '')}</option>)}
              </datalist>
            )}
            {path === 'items' && f.key === 'short_name' && (
              <datalist id="mc-items-shortname-list">
                {rows.filter(r => r.short_name).map(r => <option key={String(r.code)} value={String(r.short_name)}>{String(r.name || '')}</option>)}
              </datalist>
            )}
          </>
        )}
        <InlineError message={formErrors[f.key]} id={`err-${path}-${f.key}`} />
      </>
    );
  };

  // One titled box of fields; `nested` drops the box's own border/background and draws a compact label-left/
  // value-right row per field (vLQMS.exe's right-hand "Additional Fields" grid) instead of the left column's
  // stacked label-above-value blocks.
  const renderSection = (sec, i, nested = false) => (
    <GroupBox key={`${nested ? 'r' : 'l'}${i}-${sec.title || 'main'}`} title={sec.title} className={nested ? '!border-0 !bg-transparent' : undefined}>
      <div className={nested && legacySkin ? 'overflow-hidden rounded-sm border border-[#E3D9CD] text-xs dark:border-[#2A2A2A]'
        : nested ? 'divide-y divide-line-soft'
        : legacySkin ? 'grid grid-cols-1 gap-4 sm:grid-cols-2'
        : !listOpen || sec.fields.every(f => f.extra) ? 'grid grid-cols-[repeat(auto-fit,minmax(min(100%,13rem),1fr))] gap-4'
        : `grid gap-4 ${sec.fields.length > 4 || (!sec.title && fields.length > 8) ? '2xl:grid-cols-2' : ''}`}>
        {sec.fields.map((f, fi) => {
          const values = ruleValues(form);
          if (f.hideWhen && ruleHolds(f.hideWhen, values)) return null;   // hidden by the designer's rule
          // the exe shows the Not Active Remarks box only while the record is inactive, under the Inactive check box
          if (isRemarks(f) && (!inactiveNow || (!readOnly && fields.some(x => x.key === 'active')))) return null;
          // Read-only by Field Level Rights: can be filled in for a new record, not changed afterwards.
          const locked = readOnly || f.lockedAlways || (f.readonlyWhen && ruleHolds(f.readonlyWhen, values))
            || ((f.newOnly || fieldRight(path, f.key) === 'readonly') && mode === 'Edit') || (path === 'items' && f.key === 'uom' && mode === 'Edit' && uomLocked) || (f.key === 'code' && !!live.screen?.flags?.auto_code);
          const value = f.extra && f.type === 'date' ? String(valueOf(form, f) ?? '').slice(0, 10) : valueOf(form, f);
          const opts = f.options ?? options[f.key] ?? [];
          const id = `mc-${path}-${f.key}`;
          const hasError = !readOnly && !!formErrors[f.key];
          const required = f.required || (f.mandatoryWhen && ruleHolds(f.mandatoryWhen, values));
          const tone = fieldTone(required, f.lockedAlways || (f.readonlyWhen && ruleHolds(f.readonlyWhen, values)));   // red = mandatory, blue = read-only, as in vLQMS.exe
          const ctx = { id, value, locked, hasError, required, opts, compact: nested };

          if (nested && legacySkin) {
            // The legacy grid: a bordered two-column table, its first row highlighted the way the exe's grid
            // highlights the selected cell.
            return (
              <div key={f.key} className={cn('grid grid-cols-[minmax(0,1fr)_minmax(0,1.1fr)] border-b border-[#E3D9CD] last:border-b-0 dark:border-[#2A2A2A]', fi === 0 && 'bg-[#F8EDDA] dark:bg-amber-500/10')}>
                <span className={cn('border-r border-[#E3D9CD] px-2 py-1 font-medium text-stone-700 dark:border-[#2A2A2A] dark:text-stone-300 break-words', tone)}>
                  {f.label}{required && !tone.includes('blue') && <span className="ml-0.5 text-red-600" aria-hidden="true">*</span>}
                </span>
                <span className="min-w-0 px-2 py-1 text-right text-stone-900 dark:text-stone-50">
                  {f.type === 'bool' && !readOnly ? (
                    <label htmlFor={id} className="inline-flex cursor-pointer items-center justify-end">{renderFieldControl(f, ctx)}</label>
                  ) : renderFieldControl(f, ctx)}
                </span>
              </div>
            );
          }

          if (nested) {
            return (
              <div key={f.key} className="flex items-center justify-between gap-3 py-1.5 text-xs">
                <span className={cn('shrink-0 font-medium text-stone-600 dark:text-stone-400 break-words', tone)}>
                  {f.label}{required && !tone.includes('blue') && <span className="ml-0.5 text-red-600" aria-hidden="true">*</span>}
                </span>
                <span className="min-w-0 flex-1 text-right text-stone-900 dark:text-stone-50">
                  {f.type === 'bool' && !readOnly ? (
                    <label htmlFor={id} className="inline-flex cursor-pointer items-center justify-end">{renderFieldControl(f, ctx)}</label>
                  ) : renderFieldControl(f, ctx)}
                </span>
              </div>
            );
          }

          return (
            <div key={f.key} className={`min-w-0 ${f.extra && f.wide ? 'col-span-full' : ''}`}>
              {readOnly ? (
                <>
                  <div className={cn('mb-1 text-xs font-medium text-stone-500 dark:text-stone-400', tone)}>{f.label}{required && !tone.includes('blue') && <span className="ml-0.5" aria-hidden="true">*</span>}</div>
                  <div className={`min-h-5 text-sm text-stone-900 dark:text-stone-50 ${f.type === 'number' ? 'tabular-nums' : ''}`}>{renderFieldControl(f, ctx)}</div>
                </>
              ) : f.type === 'bool' ? (
                <>
                  <label htmlFor={id} className="flex items-center gap-2 pt-1 text-xs font-medium text-stone-700 dark:text-stone-200">
                    {renderFieldControl(f, ctx)}
                    {f.key === 'active' ? 'Inactive' : f.label}
                  </label>
                  {f.key === 'active' && inactiveNow && fields.filter(isRemarks).map(rf => {
                    const rid = `mc-${path}-${rf.key}`;
                    const rErr = !!formErrors[rf.key];
                    return (
                      <div key={rf.key} className="mt-2">
                        <label htmlFor={rid} className={cn(labelClass, 'break-words text-red-600')}>{rf.label}<span className="ml-0.5" aria-hidden="true">*</span></label>
                        {renderFieldControl(rf, { id: rid, value: valueOf(form, rf), locked: false, hasError: rErr, required: true, opts: [], compact: false })}
                      </div>
                    );
                  })}
                </>
              ) : (
                <>
                  <label htmlFor={id} title={f.tooltip} className={cn(labelClass, 'break-words', tone)}>{f.label}{required && <span className="ml-0.5 text-red-600" aria-hidden="true">*</span>}</label>
                  {renderFieldControl(f, ctx)}
                </>
              )}
            </div>
          );
        })}
      </div>
    </GroupBox>
  );

  return (
    <ScreenFrame bar={bar}>
    <div className="w-full">
      {listOpen && (
        <ListModal title={title} subtitle={`Listing of ${title}`} columns={listColumns} defaultKeys={listFields.map(f => f.key)} rows={rows} rowKey={r => r.code}
          loading={!loaded} storageKey={`master-${path}`} exportName={title} onRefresh={() => listMaster(path).then(setRows).catch(e => setToast({ kind: 'error', text: errorText(e) }))}
          onOpen={select} onClose={() => setListOpen(false)} />
      )}
      {toast && <Toast kind={toast.kind} text={toast.text} />}
      {dialog}
      {copyOpen && (
        <Modal title="Select the item to copy" onClose={() => setCopyOpen(false)} wide>
          <input autoFocus value={copyFilter} onChange={e => setCopyFilter(e.target.value)} placeholder="Type to filter by code or name" className={`${field} mb-2`} />
          <div className="max-h-[50vh] overflow-auto">
            {rows.filter(r => !copyFilter || `${r.code} ${r.name}`.toLowerCase().includes(copyFilter.toLowerCase())).slice(0, 200).map(r => (
              <button key={String(r.code)} type="button" onClick={() => copyFrom(r)} className="flex w-full justify-between gap-3 border-b border-line-soft px-2 py-1.5 text-left hover:bg-accent/10">
                <span className="font-mono">{String(r.code)}</span><span className="truncate">{String(r.name ?? '')}</span>
              </button>
            ))}
          </div>
        </Modal>
      )}
      {similar && (
        <Modal title={`ItemCode '${similar.code}' is similar to below Item:`} onClose={() => setSimilar(null)}>
          {similar.list.map(r => <div key={r.code} className="flex justify-between gap-3 border-b border-line-soft py-1.5"><span className="font-mono">{r.code}</span><span>{r.name}</span></div>)}
        </Modal>
      )}
      {attachOpen && selected && <AttachmentsDialog master={path} code={selected} name={String(form.name ?? '')} canEdit={canEdit} onClose={() => setAttachOpen(false)} />}
      {partyOpen && selected && <PartyRecords party={path} code={selected} name={String(form.name ?? '')} canEdit={canEdit} onClose={() => setPartyOpen(false)} />}
      <section id={`mc-${path}-details`} aria-label="Record details" className={`${legacySkin ? '-mx-4 rounded-none border-0 bg-[#F3EEE6] dark:bg-[#141414] sm:-mx-6' : panel} scroll-mt-20 self-start`}>
          <div className={`flex items-center justify-between border-b border-line px-5 ${legacySkin ? 'py-2' : 'py-3.5'}`}>
            <h2 className={sectionTitle}>{mode === 'New' ? 'New record' : mode === 'Edit' ? `Editing ${selected}` : selected ?? 'Details'}</h2>
            {legacySkin
              ? (!readOnly && <span className="text-[11px] text-stone-500"><span className="text-red-600">*</span> required</span>)
              : readOnly
              ? <span className={badge('neutral')}>View</span>
              : <span className="text-[11px] text-stone-500"><span className="text-red-600">*</span> required</span>}
          </div>

          <div className={legacySkin ? 'p-4' : 'p-5'}>
            {/* Amazon-style "There was a problem" alert summary box */}
            {!readOnly && <FormErrorSummary errors={formErrors} onFocusField={focusField} />}

            <div className={rightFields.length > 0 ? 'grid gap-4 lg:grid-cols-[minmax(0,1fr)_minmax(380px,520px)]' : ''}>
              <div className="space-y-4">
                {leftSections.map((sec, i) => renderSection(sec, i))}
              </div>

              {rightFields.length > 0 && (legacySkin ? (
                // The legacy grid has no "Additional Information" caption of its own — just the tab, then the grid.
                <div className="rounded-sm border border-[#E3D9CD] bg-white/30 p-2 dark:border-[#2A2A2A] dark:bg-[#1A1A1A]/40">
                  <div role="tablist" aria-label="Sections" className="mb-2 flex gap-1 overflow-x-auto overflow-y-hidden border-b border-[#E3D9CD] dark:border-[#2A2A2A]">
                    {tabNames.map(name => (
                      <button key={name || 'header'} type="button" role="tab" aria-selected={shownTab === name} onClick={() => setActiveTab(name)}
                        className={`-mb-px whitespace-nowrap border-b-2 px-3 py-2 text-xs font-medium transition-colors ${shownTab === name ? 'border-accent text-stone-900 dark:text-stone-50' : 'border-transparent text-stone-500 hover:text-stone-900 dark:text-stone-400 dark:hover:text-stone-50'}`}>
                        {name || 'Header Info'}
                      </button>
                    ))}
                  </div>
                  <div className="space-y-4">
                    {rightSections.map((sec, i) => renderSection(sec, i, true))}
                  </div>
                </div>
              ) : (
                <GroupBox title="Additional Information">
                  <div role="tablist" aria-label="Sections" className="mb-3 flex gap-1 overflow-x-auto overflow-y-hidden border-b border-line">
                    {tabNames.map(name => (
                      <button key={name || 'header'} type="button" role="tab" aria-selected={shownTab === name} onClick={() => setActiveTab(name)}
                        className={`-mb-px whitespace-nowrap border-b-2 px-3 py-2 text-xs font-medium transition-colors ${shownTab === name ? 'border-accent text-stone-900 dark:text-stone-50' : 'border-transparent text-stone-500 hover:text-stone-900 dark:text-stone-400 dark:hover:text-stone-50'}`}>
                        {name || 'Header Info'}
                      </button>
                    ))}
                  </div>
                  <div className="space-y-4">
                    {rightSections.map((sec, i) => renderSection(sec, i, true))}
                  </div>
                </GroupBox>
              ))}
            </div>
          </div>

        </section>
    </div>
    </ScreenFrame>
  );
}
