// Customised Reports (module key "Reports Dashboard"): the list of legacy report names, with
// Generate & View, Export XLS, Email and Print. Data comes from GET /reports/custom
// (backend/app/routers/custom_reports.py); Email asks the backend to build and send the .xls
// (ReportEmailDialog). Period mistakes show inline under the dates; server results as popups.

import { useEffect, useState } from 'react';
import { Mail, Download, Eye, Plus, X, Loader2, Printer } from 'lucide-react';
import { badge, field, fieldError, ghostButton, iconButton, label, pageHeader, pageSub, pageTitle, panel, primaryButton, secondaryButton, sectionTitle, tableBody, tableHead, tableRow } from '../../lib/styles';
import { toast } from '../common/toastStore';
import { InlineError } from '../common/FormErrorSummary';
import { errorText, getCustomReportRights, listMaster, renderCustomReportPrint, runCustomReport, todayIso } from '../../lib/api';
import { downloadXls, type ReportColumn, type ReportRow } from '../../lib/xlsExport';
import { useCompanyProfile } from '../../hooks/useCompanyProfile';
import PrintFormatSelectorModal from '../print/PrintFormatSelectorModal';
import ReportPrintModal from '../print/ReportPrintModal';
import ReportEmailDialog, { type ReportEmailRequest } from './ReportEmailDialog';

const toDmy = (iso: string) => iso.split('-').reverse().join('/');

type ReportResult = { name: string; from_date: string; to_date: string; columns: ReportColumn[]; rows: ReportRow[]; filterKey?: string; rpt?: string; group?: string };
type Master = { code: string; name: string };
type ActiveFilter = { on: string; val: string; param?: MasterParam; code?: string };

// Filter On choices that pick a master record; the date choices use From / To instead.
const MASTER_FILTERS = {
  'Ledger Name': { param: 'ledger', path: 'ledgers' },
  'Supplier Name': { param: 'supplier', path: 'suppliers' },
  'Item Group': { param: 'group', path: 'item-groups' },
} as const;
type MasterParam = (typeof MASTER_FILTERS)[keyof typeof MASTER_FILTERS]['param'];
const masterFilter = (on: string) => MASTER_FILTERS[on as keyof typeof MASTER_FILTERS];

const fmtCell = (v: ReportRow[string], col: ReportColumn) => {
  if (typeof v === 'number') return v.toLocaleString('en-IN', { maximumFractionDigits: col.key === 'litres' ? 3 : 2 });
  if (typeof v === 'string' && /^\d{4}-\d{2}-\d{2}$/.test(v)) return toDmy(v);
  return v ?? '';
};

const ReportsDashboard = () => {
  const { company } = useCompanyProfile();
  const [selectedReport, setSelectedReport] = useState<string>('Expense Ledger Report');
  const [filterOn, setFilterOn] = useState<string>('Exp Date');
  const [activeFilters, setActiveFilters] = useState<ActiveFilter[]>([]);
  // Inline mistakes: the period (under From/To) and the Filter On value / Add filter row.
  const [dateError, setDateError] = useState('');
  const [filterError, setFilterError] = useState('');
  const [filterValue, setFilterValue] = useState('');
  const [masters, setMasters] = useState<Record<string, Master[]>>({});

  // Load the value list the first time a master-based Filter On is picked.
  useEffect(() => {
    const mf = masterFilter(filterOn);
    if (!mf || masters[filterOn]) return;
    listMaster<Master>(mf.path)
      .then(list => setMasters(m => ({ ...m, [filterOn]: list })))
      .catch(e => toast.error(errorText(e)));
  }, [filterOn, masters]);

  // Reports this user may run (Admin Master (Security) → Report rights); null until loaded = show all.
  // Only reports the server has and this user may not run are hidden ("not available yet" ones stay listed).
  const [hiddenReports, setHiddenReports] = useState<Set<string>>(new Set());
  const [serverGroups, setServerGroups] = useState<Record<string, string>>({});
  useEffect(() => {
    getCustomReportRights()
      .then(r => { setHiddenReports(new Set(r.known.filter(n => !r.allowed.includes(n)))); setServerGroups(r.groups ?? {}); })
      .catch(() => undefined);
  }, []);

  // Default period: this month so far.
  const [fromDate, setFromDate] = useState(() => `${todayIso().slice(0, 8)}01`);
  const [toDate, setToDate] = useState(todayIso);
  const [result, setResult] = useState<ReportResult | null>(null);
  const [busy, setBusy] = useState(false);
  const [showPrintModal, setShowPrintModal] = useState(false);
  const [emailRequest, setEmailRequest] = useState<ReportEmailRequest | null>(null);

  const builtInCategories = [
    {
      cat: 'Expense Report',
      reports: ['Expense Ledger Report', 'Expense Report']
    },
    {
      cat: 'Purchase Report',
      reports: [
        '1.GRN Wise Purchase Report',
        '2.Day Wise Group Wise Purchase Report',
        '3.Supplier Bill Wise Purchase Report',
        'Item Wise Purchase Report-Day Wise',
        'Item Wise Purchase Report-Bill Wise',
        'Item Wise Purchase Report-Month Wise',
        'ItemWise Purchase Report',
        'MRP RATES',
        'Purchase Report- Item Wise Summary',
        'Purchase Report-MonthWise Summary'
      ]
    },
    {
      cat: 'Sales Report',
      reports: [
        '1.Deleted Invoice Report',
        '2.Sales-Item Wise Report',
        '3.Item wise Sales Report - Bill Wise',
        'Cancelled Invoice report',
        'Item wise Sales Report - Day Wise',
        'Item wise Sales Report - Month Wise',
        'Sales - Bill Wise Report',
        'Sales - Counter Wise Report',
        'Sales - Day Wise Report',
        'Sales - Group Wise Report',
        'Sales - Hourly Report',
        'Sales - Item Wise Report',
        'Sales - Month Wise Report',
        'Sales - PackageItem Wise Report',
        'Sales - Sales Person Wise Report',
        'Sales Other Payment',
        'Sales Return - Day Wise Report',
        'Sales Return Report'
      ]
    }
  ];

  // Reports the shop's database defines (the desktop's Customised Reports) are listed under their own group; a report
  // the app has built in but the database doesn't define keeps its built-in place.
  const reportCategories = (() => {
    const byGroup = new Map<string, string[]>();
    for (const [name, group] of Object.entries(serverGroups)) if (group) byGroup.set(group, [...(byGroup.get(group) ?? []), name]);
    if (!byGroup.size) return builtInCategories;
    const order = ['Expense Report', 'Purchase Report', 'Sales Report'];
    const cats = [...byGroup.entries()].sort(([a], [b]) => (order.indexOf(a) + 1 || 99) - (order.indexOf(b) + 1 || 99) || a.localeCompare(b))
      .map(([cat, reports]) => ({ cat, reports: reports.sort((x, y) => x.localeCompare(y)) }));
    const known = new Set(cats.flatMap(c => c.reports));
    const extra = builtInCategories.map(c => ({ ...c, reports: c.reports.filter(n => !known.has(n) && !(n in serverGroups)) })).filter(c => c.reports.length);
    return [...cats, ...extra];
  })();

  // The period's mistake, shown under From/To (and the cursor goes to the faulty date).
  const checkPeriod = () => {
    const problem = !fromDate || !toDate ? 'Pick both From and To dates' : fromDate > toDate ? 'From Date cannot be after To Date' : '';
    setDateError(problem);
    if (problem) document.getElementById(!fromDate || fromDate > toDate ? 'cr-from' : 'cr-to')?.focus();
    return !problem;
  };

  const addFilter = () => {
    if (!checkPeriod()) return;
    const mf = masterFilter(filterOn);
    if (mf && !filterValue) {
      setFilterError(`Pick a ${filterOn} to filter on`);
      return document.getElementById('cr-val')?.focus();
    }
    const val = mf
      ? masters[filterOn]?.find(m => m.code === filterValue)?.name ?? filterValue
      : `${toDmy(fromDate)} to ${toDmy(toDate)}`;
    if (activeFilters.some(f => f.on === filterOn && f.val === val)) return setFilterError('That filter is already applied');
    setFilterError('');
    setActiveFilters([...activeFilters, mf ? { on: filterOn, val, param: mf.param, code: filterValue } : { on: filterOn, val }]);
  };

  const filterCodes = (param: MasterParam) => activeFilters.filter(f => f.param === param && f.code).map(f => f.code as string);

  const openEmail = () => {
    if (!checkPeriod()) return;
    setEmailRequest({ name: selectedReport, from_date: fromDate, to_date: toDate,
      ledger: filterCodes('ledger'), supplier: filterCodes('supplier'), group: filterCodes('group') });
  };

  const fetchReport = async (): Promise<ReportResult | null> => {
    if (!checkPeriod()) return null;
    // Reuse what is on screen when nothing has changed since Generate.
    const params = new URLSearchParams({ name: selectedReport, from_date: fromDate, to_date: toDate });
    activeFilters.forEach(f => { if (f.param && f.code) params.append(f.param, f.code); });
    const filterKey = params.toString();
    if (result?.filterKey === filterKey) return result;
    setBusy(true);
    try {
      const res = { ...(await runCustomReport<ReportResult>(filterKey)), filterKey };
      setResult(res);
      return res;
    } catch (e) {
      toast.error(errorText(e));
      return null;
    } finally {
      setBusy(false);
    }
  };

  const generate = async () => {
    const res = await fetchReport();
    if (res) toast.ok(`${res.rows.length} row${res.rows.length === 1 ? '' : 's'} found`);
  };

  const exportXls = async () => {
    const res = await fetchReport();
    if (!res) return;
    if (!res.rows.length) return toast.error('Nothing to export for this period.');
    if (!(await toast.later('ok', `${res.name} will be exported to XLS when this closes.`, 'Export cancelled.'))) return;
    downloadXls(`${res.name} ${res.from_date} to ${res.to_date}`, `${company?.name || 'Company'} - ${res.name}`,
      `Period: ${toDmy(res.from_date)} to ${toDmy(res.to_date)}`, res.columns, res.rows);
    toast.ok(`${res.name} exported to XLS`);
  };

  const removeFilter = (idx: number) => {
    setActiveFilters(activeFilters.filter((_, i) => i !== idx));
  };

  return (
    <div className="mx-auto max-w-7xl space-y-5">
      <div className={pageHeader}>
        <div>
          <h1 className={pageTitle}>Customised Reports</h1>
          <p className={pageSub}>Audit and MIS reporting across purchase, sales, stock and expenses.</p>
        </div>
        <div className="flex items-center gap-2">
          <button onClick={openEmail} className={secondaryButton}><Mail size={14} />Email</button>
          <button onClick={exportXls} disabled={busy} className={secondaryButton}><Download size={14} />Export XLS</button>
        </div>
      </div>

      <div className="grid grid-cols-1 items-start gap-5 lg:grid-cols-[300px_minmax(0,1fr)]">
        {/* Report catalogue */}
        <nav aria-label="Reports" className={`${panel} max-h-[640px] overflow-y-auto p-2`}>
          {reportCategories.map(g => ({ ...g, reports: g.reports.filter(r => !hiddenReports.has(r)) })).filter(g => g.reports.length).map(group => (
            <div key={group.cat} className="mb-2 last:mb-0">
              <div className="px-2.5 pb-1 pt-2 text-[11px] font-medium text-stone-500 dark:text-stone-400">{group.cat}</div>
              {group.reports.map(rep => (
                <button
                  key={rep}
                  onClick={() => setSelectedReport(rep)}
                  aria-current={selectedReport === rep ? 'true' : undefined}
                  className={`block w-full truncate rounded-lg px-2.5 py-1.5 text-left text-xs transition-colors ${
                    selectedReport === rep
                      ? 'bg-accent-soft font-semibold text-accent-ink'
                      : 'text-stone-600 hover:bg-subtle hover:text-stone-900 dark:text-stone-300 dark:hover:text-stone-50'
                  }`}
                >
                  {rep}
                </button>
              ))}
            </div>
          ))}
        </nav>

        {/* Filters */}
        <div className={`${panel} divide-y divide-line`}>
          <div className="px-5 py-4">
            <div className="text-[11px] font-medium text-stone-500 dark:text-stone-400">Selected report</div>
            <h2 className="mt-0.5 text-base font-semibold text-stone-900 dark:text-stone-50">{selectedReport}</h2>
          </div>

          <div className="space-y-4 p-5">
            <div className={`grid grid-cols-1 gap-4 ${masterFilter(filterOn) ? 'sm:grid-cols-2 xl:grid-cols-4' : 'sm:grid-cols-3'}`}>
              <div>
                <label htmlFor="cr-on" className={label}>Filter on</label>
                <select id="cr-on" value={filterOn} onChange={(e) => { setFilterOn(e.target.value); setFilterValue(''); }} className={field}>
                  <option>Exp Date</option>
                  <option>Bill Date</option>
                  <option>Ledger Name</option>
                  <option>Supplier Name</option>
                  <option>Item Group</option>
                </select>
              </div>
              {masterFilter(filterOn) && (
                <div>
                  <label htmlFor="cr-val" className={label}>{filterOn}</label>
                  <select id="cr-val" value={filterValue} onChange={(e) => { setFilterValue(e.target.value); setFilterError(''); }}
                    className={`${field} ${filterError && !filterValue ? fieldError : ''}`}>
                    <option value="">{masters[filterOn] ? `Select ${filterOn}…` : 'Loading…'}</option>
                    {masters[filterOn]?.map(m => <option key={m.code} value={m.code}>{m.name}</option>)}
                  </select>
                  {!filterValue && <InlineError message={filterError} />}
                </div>
              )}
              <div>
                <label htmlFor="cr-from" className={label}>From</label>
                <input id="cr-from" type="date" value={fromDate} onChange={(e) => { setFromDate(e.target.value); setDateError(''); }}
                  aria-invalid={!!dateError} className={`${field} tabular-nums ${dateError ? fieldError : ''}`} />
              </div>
              <div>
                <label htmlFor="cr-to" className={label}>To</label>
                <input id="cr-to" type="date" value={toDate} onChange={(e) => { setToDate(e.target.value); setDateError(''); }}
                  aria-invalid={!!dateError} className={`${field} tabular-nums ${dateError ? fieldError : ''}`} />
                <InlineError message={dateError} />
              </div>
            </div>

            <div className="flex flex-wrap items-center gap-2">
              <button onClick={addFilter} className={secondaryButton}><Plus size={14} />Add filter</button>
              {activeFilters.map((f, i) => (
                <span key={i} className={`${badge('neutral')} py-1 pl-2 pr-1 text-xs`}>
                  <span className="text-stone-500">{f.on}:</span>
                  <span className="font-medium">{f.val}</span>
                  <button onClick={() => removeFilter(i)} className="ml-0.5 rounded p-0.5 text-stone-500 hover:bg-surface hover:text-red-600" aria-label={`Remove filter ${f.on}`}><X size={12} /></button>
                </span>
              ))}
              {filterError && (filterValue || !masterFilter(filterOn)) && <InlineError message={filterError} />}
              {activeFilters.length === 0 && <span className="text-xs text-stone-500">No filters. The whole period is shown.</span>}
              {activeFilters.length > 0 && (
                <button onClick={() => setActiveFilters([])} className={`${ghostButton} ml-auto hover:!text-red-600`}>Clear all</button>
              )}
            </div>
          </div>

          <div className="flex justify-end bg-subtle/60 px-5 py-3">
            <button onClick={generate} disabled={busy} className={primaryButton}>
              {busy ? <Loader2 size={14} className="animate-spin" /> : <Eye size={14} />}Generate report
            </button>
          </div>
        </div>
      </div>

      {result && (
        <section aria-label="Report results" className={`${panel} overflow-hidden`}>
          <div className="flex flex-wrap items-center justify-between gap-2 border-b border-line px-5 py-3.5">
            <h2 className={sectionTitle}>{result.name}</h2>
            <div className="flex items-center gap-2 text-xs text-stone-500">
              <span className="tabular-nums">{toDmy(result.from_date)} – {toDmy(result.to_date)}</span>
              <span className={badge('neutral')}>{result.rows.length} row{result.rows.length === 1 ? '' : 's'}</span>
              <button onClick={() => setShowPrintModal(true)} disabled={!result.rows.length} className={iconButton} aria-label="Print report" title={result.rows.length ? 'Print report' : 'Nothing to print for this period'}><Printer size={14} /></button>
              <button onClick={exportXls} className={iconButton} aria-label="Export XLS" title="Export XLS"><Download size={14} /></button>
            </div>
          </div>
          {/* Wide reports scroll sideways inside the card so the page itself never scrolls horizontally on mobile. */}
          <div className="max-h-[560px] overflow-auto">
            <table className="w-full whitespace-nowrap text-left text-xs">
              <thead className={tableHead}>
                <tr>
                  {result.columns.map(c => (
                    <th key={c.key} className={`px-4 py-2.5 ${c.align === 'right' ? 'text-right' : ''}`}>{c.label}</th>
                  ))}
                </tr>
              </thead>
              <tbody className={tableBody}>
                {result.rows.map((row, i) => (
                  <tr key={i} className={`${tableRow} text-stone-700 dark:text-stone-200`}>
                    {result.columns.map(c => (
                      <td key={c.key} className={`px-4 py-2 ${c.align === 'right' ? 'text-right tabular-nums' : ''}`}>{fmtCell(row[c.key], c)}</td>
                    ))}
                  </tr>
                ))}
                {result.rows.length === 0 && (
                  <tr>
                    <td colSpan={result.columns.length} className="px-4 py-12 text-center text-stone-500">
                      No transactions in this period.
                    </td>
                  </tr>
                )}
              </tbody>
              {result.rows.length > 0 && result.columns.some(c => c.total) && (
                <tfoot className="sticky bottom-0 bg-[#F5F1E9] font-semibold dark:bg-[#101010]">
                  <tr>
                    {result.columns.map((c, i) => (
                      <td key={c.key} className={`px-4 py-2 ${c.align === 'right' ? 'text-right tabular-nums' : ''}`}>
                        {c.total ? fmtCell(result.rows.reduce((sum, row) => sum + (typeof row[c.key] === 'number' ? (row[c.key] as number) : 0), 0), c) : i === 0 ? 'Total' : ''}
                      </td>
                    ))}
                  </tr>
                </tfoot>
              )}
            </table>
          </div>
        </section>
      )}

      {emailRequest && (
        <ReportEmailDialog request={emailRequest} defaultTo={company?.email ?? ''} onClose={() => setEmailRequest(null)} />
      )}

      {showPrintModal && result && result.rpt && (
        <ReportPrintModal
          title={`Print: ${result.name}`}
          formats={[result.rpt]}
          render={(_rpt, printer) => renderCustomReportPrint({ name: result.name, from_date: result.from_date, to_date: result.to_date,
            ledger: filterCodes('ledger'), supplier: filterCodes('supplier'), group: filterCodes('group') }, printer)}
          onClose={() => setShowPrintModal(false)}
        />
      )}

      {showPrintModal && result && !result.rpt && (
        <PrintFormatSelectorModal
          category="GENERIC_REPORT"
          data={{
            title: result.name,
            subtitle: `Period: ${toDmy(result.from_date)} to ${toDmy(result.to_date)}`,
            columns: result.columns.map(column => ({
              key: column.key,
              label: column.label,
              kind: column.align === 'right' ? 'number' : 'text',
              total: result.rows.length > 0 && result.rows.every(row => typeof row[column.key] === 'number'),
            })),
            rows: result.rows,
          }}
          title={`Print: ${result.name}`}
          onClose={() => setShowPrintModal(false)}
        />
      )}
    </div>
  );
};

export default ReportsDashboard;