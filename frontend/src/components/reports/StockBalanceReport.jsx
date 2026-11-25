// Stock Balance report (frmRptStockBal): pick the view and filters, press View, and the result opens in a big window (StockBalanceResult) with the grid and
// its Export Excel / Col Select / Col Freeze / Print buttons. Selected Group and Select Item open the exe's "Select [Multiple] ..." windows
// (MultiSelectDialog; groups are the database's active item groups in Arrange Group Slno order). The rate is one of four radio buttons as in the exe:
// Last Purchase Rate, FIFO, Sales Rate, Wt.Avg Rate; With Tax appears only with Last Purchase Rate; FIFO and Wt.Avg Rate appear only when Show Value is ticked (FIFO also on Group wise Summary Balance).

import ReportPrintModal from '../print/ReportPrintModal';
import { toast } from '../common/toastStore';
import { useCallback, useEffect, useMemo, useState } from 'react';
import { ChevronDown, Play } from 'lucide-react';
import StockBalanceResult from './StockBalanceResult';
import { errorText, getItemGroups, getItems, getMasterOrder, getStockBalanceFormats, getStockBalanceView, getZeroStockFormats, renderStockBalancePrint, renderZeroStockPrint, todayIso } from '../../lib/api';
import { field, label, primaryButton } from '../../lib/styles';
import { groupLabel } from '../../lib/formatters';
import MultiSelectDialog from '../common/MultiSelectDialog';
import { pageHeader, pageSub, pageTitle, panel } from '../../lib/styles';
import DateInput from '../common/DateInput';

// The button that opens a "Select [Multiple] ..." window: shows "All ..." when nothing is ticked, the one name, or "N selected".
function PickButton({ id, text, onClick }) {
  return (
    <button type="button" id={id} onClick={onClick} aria-haspopup="dialog" className={`${field} flex items-center justify-between gap-2 text-left`}>
      <span className="truncate">{text}</span><ChevronDown size={14} className="shrink-0" />
    </button>
  );
}
// The exe's "Selected Item" / "Selected Package" boxes: what was picked, one per line, each removable.
function SelectedList({ title, names, onRemove }) {
  return (
    <section aria-label={title} className={`${panel} max-h-40 overflow-auto`}>
      <h2 className="sticky top-0 border-b border-line bg-subtle px-3 py-1.5 text-center text-xs font-semibold">{title}</h2>
      {!names.length && <p className="px-3 py-3 text-center text-xs text-stone-500">None selected: all are included.</p>}
      <ul>{names.map((n, i) => (
        <li key={n + i} className="flex items-center justify-between gap-2 px-3 py-1 text-xs hover:bg-subtle">
          <span className="truncate">{n}</span>
          <button type="button" onClick={() => onRemove(i)} aria-label={`Remove ${n}`} className="text-stone-500 hover:text-accent">✕</button>
        </li>))}
      </ul>
    </section>
  );
}
const comboKey = (view, rate, opt) => `vlqms_sb_cols_${view}_${rate}_${opt.values ? 'V' : opt.litres ? 'L' : ''}`;
const pickedText = (keys, nameOf) => (keys.length === 1 ? nameOf(keys[0]) : `${keys.length} selected`);
const fmtDate = (iso) => iso.split('-').reverse().join('/');

export default function StockBalanceReport() {
  const [formats, setFormats] = useState([]);
  const day = todayIso();
  // The exe's view options (frmRptStockBal): which view, the period, filters and the valuation.
  const [view, setView] = useState('closing');
  const [from, setFrom] = useState(day);
  const [to, setTo] = useState(day);
  const [groupList, setGroupList] = useState([]);   // the "Select [Multiple] Item Groups" rows: [{ code, slno }] in Arrange Group Slno order
  const [picking, setPicking] = useState(null);   // which "Select [Multiple] ..." window is open: 'group' or 'item'
  const [itemList, setItemList] = useState([]);
  const [groupSel, setGroupSel] = useState([]);
  const [itemSel, setItemSel] = useState([]);
  const [opt, setOpt] = useState({ litres: false, values: false, zero_hide: false, group_total: false, with_tax: false, act_sales: false, aroed: false });
  const [rate, setRate] = useState('pur');
  const [result, setResult] = useState(null);
  const [viewError, setViewError] = useState('');
  const [running, setRunning] = useState(false);
  const [resultOpen, setResultOpen] = useState(false);
  const [shown, setShown] = useState(null);   // what the open result window was loaded with: { view, rate, opt, from, to }
  const flip = k => e => setOpt(o => ({ ...o, [k]: e.target.checked }));

  // FIFO / Wt.Avg Rate appear when Show Value is ticked (the exe shows them for Closing Balance With Detailed; here Closing Balance Only values at them too),
  // and FIFO also on Group wise Summary Balance; otherwise Last Purchase Rate stands in for a rate that is not offered.
  const warOk = (view === 'closing' || view === 'detailed') && opt.values;
  const fifoOk = warOk || view === 'group';
  const rateNow = (rate === 'war' && !warOk) || (rate === 'fifo' && !fifoOk) ? 'pur' : rate;

  const runView = () => {
    const sent = { view, rate: rateNow, opt, from, to, picks: { groups: groupSel.join(','), items: itemSel.join(',') } };
    setShown(sent);
    setResult(null);
    setViewError('');
    setResultOpen(true);
    setRunning(true);
    getStockBalanceView({ from_date: from, to_date: to, view, groups: groupSel.join(','), items: itemSel.join(','), ...opt,
      values: view === 'package' ? false : opt.values, rate: rateNow })
      .then(setResult)
      .catch(e => setViewError(errorText(e)))
      .finally(() => setRunning(false));
  };
  // Stable between renders, so the print window's preview is asked for once per format.

  // The exe lists MstItemGroup (active only) ORDER BY GroupOrderBy: the Arrange Group Slno endpoint reads exactly that order from the database
  // on every request; the group list only says which groups are inactive. With the order unavailable, the group list's own order is used.
  const loadGroups = useCallback(() => {
    Promise.all([getItemGroups().catch(() => []), getMasterOrder('item-groups').catch(() => null)]).then(([groups, order]) => {
      const inactive = new Set(groups.filter(g => g.active === false).map(g => g.code));
      const list = order ? order.map(o => ({ code: o.code, slno: o.slno })) : groups.map((g, i) => ({ code: g.code, slno: i + 1 }));
      setGroupList(list.filter(g => !inactive.has(g.code)));
    });
  }, []);

  useEffect(() => {
    loadGroups();
    getItems().then(setItemList).catch(() => undefined);
  }, [loadGroups]);

  useEffect(() => {
    getStockBalanceFormats().then(f => setFormats(f.formats)).catch(() => { /* printing says why */ });
  }, []);

  // The exe lists active items by ItemCode (MstItem, InActive = 'N'); its package list is the distinct PKGItem values, A to Z (the server sends them).
  const itemRows = useMemo(() => itemList.filter(i => i.active !== false).sort((a, b) => String(a.code).localeCompare(String(b.code), undefined, { numeric: true })), [itemList]);
  // Print Zero Stock Items (a button of the result window): the desktop's RptZBItem.rpt in the same print window as the other Stock Balance prints.
  const [zeroFormats, setZeroFormats] = useState(null);   // the .rpt names while that window is open
  const [zeroPrinter, setZeroPrinter] = useState('');   // the printer of its Print Settings entry (a UtlOption row for RptZBItem.rpt), if any
  const printZero = () => {
    getZeroStockFormats().then(f => {
      if (f.formats.length) { setZeroPrinter(f.option?.printer || ''); setZeroFormats(f.formats); }
      else toast.error('RptZBItem.rpt is not in the report folder. Copy it from the desktop report folder into backend/Rpts.');
    }).catch(e => toast.error(errorText(e)));
  };
  const itemName = (code) => itemList.find(i => i.code === code)?.name || code;
  const groupText = !groupSel.length ? 'All groups' : groupSel.length === 1 ? groupLabel(groupSel[0]) : `${groupSel.length} selected`;
  const radio = (value, text) => (
    <label key={value} className="flex items-center gap-1.5 text-xs">
      <input type="radio" name="sb-rate" className="accent-accent" value={value} checked={rateNow === value} onChange={() => setRate(value)} />{text}
    </label>
  );

  return (
    <div className="mx-auto max-w-7xl">
      <div className={`${pageHeader} mb-5`}>
        <div>
          <h1 className={pageTitle}>Stock balance</h1>
          <p className={pageSub}>Today's excise opening, receipt, issue and closing balances.</p>
        </div>
      </div>
      <form onSubmit={e => { e.preventDefault(); runView(); }} aria-label="Stock balance options" className={`${panel} mb-4 flex flex-wrap items-end gap-3 p-4`}>
        <div className="w-40"><label htmlFor="sb-from" className={label}>From</label><DateInput id="sb-from" value={from} onChange={e => setFrom(e.target.value)} className={`${field} tabular-nums`} /></div>
        <div className="w-40"><label htmlFor="sb-to" className={label}>To</label><DateInput id="sb-to" value={to} onChange={e => setTo(e.target.value)} className={`${field} tabular-nums`} /></div>
        <div className="w-56"><label htmlFor="sb-view" className={label}>View</label>
          <select id="sb-view" value={view} onChange={e => setView(e.target.value)} className={field}>
            <option value="closing">Closing Balance Only</option><option value="detailed">Closing Balance With Detailed</option>
            <option value="package">Package wise Balance</option><option value="group">Group wise Summary Balance</option>
          </select>
        </div>
        {view !== 'group' && (<>
          <div className="w-44"><label htmlFor="sb-groups" className={label}>Selected Group</label>
            <PickButton id="sb-groups" text={groupText} onClick={() => { loadGroups(); setPicking('group'); }} /></div>
          <div className="w-44"><label htmlFor="sb-items" className={label}>Select Item</label>
            <PickButton id="sb-items" text={itemSel.length ? pickedText(itemSel, itemName) : 'All items'} onClick={() => setPicking('item')} /></div>
        </>)}
        {view !== 'group' && <label className="flex items-center gap-2 pb-2 text-xs"><input type="checkbox" checked={opt.litres} onChange={flip('litres')} />Show qty in Litres</label>}
        {(view === 'closing' || view === 'detailed') && <label className="flex items-center gap-2 pb-2 text-xs"><input type="checkbox" checked={opt.values} onChange={flip('values')} />Show Value</label>}
        {view !== 'group' && <label className="flex items-center gap-2 pb-2 text-xs"><input type="checkbox" checked={opt.zero_hide} onChange={flip('zero_hide')} />Do Not Show Items With Zero Balance</label>}
        {view !== 'group' && <label className="flex items-center gap-2 pb-2 text-xs"><input type="checkbox" checked={opt.group_total} onChange={flip('group_total')} />Group Wise Total Required</label>}
        <fieldset className="flex basis-full flex-wrap items-center gap-x-5 gap-y-1 pb-1">
          <legend className="sr-only">Rate</legend>
          {radio('pur', 'Last Purchase Rate')}
          {fifoOk && radio('fifo', 'FIFO')}
          {radio('mrp', 'Sales Rate')}
          {warOk && radio('war', 'Wt.Avg Rate')}
        </fieldset>
        {rateNow === 'pur' && <label className="flex items-center gap-2 pb-2 text-xs"><input type="checkbox" checked={opt.with_tax} onChange={flip('with_tax')} />With Tax</label>}
        <label className="flex items-center gap-2 pb-2 text-xs" title="Used by Closing Balance With Detailed with Show Value (as in the exe)"><input type="checkbox" checked={opt.act_sales} onChange={flip('act_sales')} />Actual Sales Value</label>
        <label className="flex items-center gap-2 pb-2 text-xs" title="Used by Closing Balance With Detailed with Show Value (as in the exe)"><input type="checkbox" checked={opt.aroed} onChange={flip('aroed')} />AROED Value</label>
        <button type="submit" className={primaryButton} disabled={running}><Play size={14} />View</button>
      </form>
      {view !== 'group' && itemSel.length > 0 && (
        <div className="mb-4 grid gap-3 sm:grid-cols-2">
          <SelectedList title="Selected Item" names={itemSel.map(itemName)} onRemove={i => setItemSel(itemSel.filter((_, n) => n !== i))} />
        </div>
      )}
      {resultOpen && shown && (
        <StockBalanceResult result={result} running={running} error={viewError} view={shown.view} rate={shown.rate} layoutKey={comboKey(shown.view, shown.rate, shown.opt)}
          title="Stock Balance Report" subtitle={`From ${fmtDate(shown.from)} to ${fmtDate(shown.to)}`} formats={formats} onPrintZero={itemList.length ? printZero : undefined} onClose={() => setResultOpen(false)}
          renderPrint={(rpt, printer) => renderStockBalancePrint(shown.from, shown.to, '', rpt, printer, shown.picks)} />
      )}
      {zeroFormats && <ReportPrintModal title="Zero Stock Items" formats={zeroFormats} render={renderZeroStockPrint} defaultPrinter={zeroPrinter} directOnly onClose={() => setZeroFormats(null)} />}
      {picking === 'group' && (
        <MultiSelectDialog title="Select [Multiple] Item Groups" rows={groupList} rowKey={g => g.code} chosen={groupSel} wide={false}
          onOk={keys => { setGroupSel(keys); setPicking(null); }} onClose={() => setPicking(null)}
          columns={[{ key: 'code', label: 'ItemGroupCode', render: g => groupLabel(g.code) }, { key: 'slno', label: 'GroupOrderBy', align: 'right' }]}
          pasteBy={[{ label: 'ItemGroupCode', value: g => g.code }]} />
      )}
      {picking === 'item' && (
        <MultiSelectDialog title="Select [Multiple] Item" rows={itemRows} rowKey={i => i.code} chosen={itemSel} wide={false}
          onOk={keys => { setItemSel(keys); setPicking(null); }} onClose={() => setPicking(null)}
          columns={[{ key: 'code', label: 'ItemCode' }, { key: 'name', label: 'ItemName' }]}
          pasteBy={[{ label: 'ItemCode', value: i => i.code }, { label: 'ItemName', value: i => i.name }]} />
      )}
    </div>
  );
}
