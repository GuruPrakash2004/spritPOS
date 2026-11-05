// The accounting reports, read from the shop's own tables (legacy FrmRptAccGL, frmRptAccOutstanding, FrmRptAccOSBillWise, FrmRptAccAgeBillWise):
//   GeneralLedger      one ledger's postings between two dates, with the opening balance and a running balance
//   PartyOutstanding   every customer or supplier whose postings do not net to zero
//   BillWiseReport     pending bills per party (mode "outstanding") or put in age buckets by due date (mode "ageing")
// Each has Export CSV.

import { useEffect, useState } from 'react';
import { Download, Eye } from 'lucide-react';
import { InlineError } from '../common/FormErrorSummary';
import { downloadText, toCsv } from '../../lib/csv';
import { errorText, getAccountingLedgers, getBillAgeing, getBillWise, getGeneralLedger, getPartyOutstanding, todayIso } from '../../lib/api';
import { exportButton, field, label, money, pageHeader, pageSub, pageTitle, panel, primaryButton, tableBody, tableHead } from '../../lib/styles';

function Page({ title, sub, controls, problem, children }) {
  return (
    <div className="mx-auto flex max-w-6xl flex-col gap-4">
      <div className={pageHeader}><div><h1 className={pageTitle}>{title}</h1><p className={pageSub}>{sub}</p></div></div>
      <div className={`${panel} flex flex-wrap items-end gap-3 px-5 py-3`}>{controls}</div>
      {problem && <InlineError message={problem} />}
      {children}
    </div>
  );
}

const th = 'px-3 py-2';
const num = 'px-3 py-1.5 text-right tabular-nums';
const monthStart = () => `${todayIso().slice(0, 8)}01`;

export function GeneralLedger() {
  const [ledgers, setLedgers] = useState([]);
  const [ledger, setLedger] = useState('');
  const [from, setFrom] = useState(monthStart());
  const [to, setTo] = useState(todayIso());
  const [data, setData] = useState(null);
  const [problem, setProblem] = useState('');
  useEffect(() => { getAccountingLedgers().then(setLedgers).catch(e => setProblem(errorText(e))); }, []);
  const show = () => {
    if (!ledger) return setProblem('Please select a valid Ledger!');
    setProblem('');
    getGeneralLedger(ledger, from, to).then(setData).catch(e => { setData(null); setProblem(errorText(e)); });
  };
  const csv = () => downloadText('General_Ledger.csv', toCsv([['Date', 'Type', 'Doc No', 'Narration', 'Debit', 'Credit', 'Balance'],
    ...data.rows.map(r => [r.date, r.doc_type, r.doc_no, r.narration, r.debit, r.credit, r.balance])]));
  return (
    <Page title="General Ledger" sub="One ledger's postings, with the opening balance and a running balance." problem={problem}
      controls={<>
        <div><label htmlFor="gl-led" className={label}>Ledger</label>
          <select id="gl-led" value={ledger} onChange={e => setLedger(e.target.value)} className={`${field} w-72`}>
            <option value="">— pick a ledger —</option>
            {ledgers.map(l => <option key={l.id} value={l.id}>{l.name}{l.group ? ` — ${l.group}` : ''}</option>)}
          </select></div>
        <div><label htmlFor="gl-from" className={label}>From</label><input id="gl-from" type="date" value={from} onChange={e => setFrom(e.target.value)} className={`${field} tabular-nums`} /></div>
        <div><label htmlFor="gl-to" className={label}>To</label><input id="gl-to" type="date" value={to} onChange={e => setTo(e.target.value)} className={`${field} tabular-nums`} /></div>
        <button onClick={show} className={primaryButton}><Eye size={14} />View</button>
        {data && <button onClick={csv} className={exportButton}><Download size={14} />Export CSV</button>}
      </>}>
      {data && (
        <div className={`${panel} overflow-auto`}>
          <div className="border-b border-line px-4 py-2 text-xs font-bold">{data.ledger}</div>
          <table className="w-full text-xs">
            <thead className={tableHead}><tr><th className={th}>Date</th><th className={th}>Type</th><th className={th}>Doc No</th><th className={th}>Narration</th><th className={`${th} text-right`}>Debit</th><th className={`${th} text-right`}>Credit</th><th className={`${th} text-right`}>Balance</th></tr></thead>
            <tbody className={tableBody}>
              {data.rows.map((r, i) => (
                <tr key={i}><td className="px-3 py-1.5">{r.date}</td><td className="px-3 py-1.5">{r.doc_type}</td><td className="px-3 py-1.5">{r.doc_no}</td><td className="px-3 py-1.5">{r.narration}</td>
                  <td className={num}>{r.debit ? money(r.debit) : ''}</td><td className={num}>{r.credit ? money(r.credit) : ''}</td><td className={num}>{money(r.balance)}</td></tr>
              ))}
              <tr className="font-bold"><td colSpan={6} className="px-3 py-2 text-right">Closing balance</td><td className={num}>{money(data.closing)}</td></tr>
            </tbody>
          </table>
        </div>
      )}
    </Page>
  );
}

export function PartyOutstanding() {
  const [party, setParty] = useState('C');
  const [rows, setRows] = useState(null);
  const [problem, setProblem] = useState('');
  const show = () => { setProblem(''); getPartyOutstanding(party).then(setRows).catch(e => { setRows(null); setProblem(errorText(e)); }); };
  const dr = (rows || []).reduce((a, r) => a + r.debit, 0), cr = (rows || []).reduce((a, r) => a + r.credit, 0);
  return (
    <Page title="Party wise Outstanding" sub="Every customer or supplier whose postings do not net to zero." problem={problem}
      controls={<>
        <div><label htmlFor="po-party" className={label}>Party</label>
          <select id="po-party" value={party} onChange={e => { setParty(e.target.value); setRows(null); }} className={field}><option value="C">Customers</option><option value="S">Suppliers</option></select></div>
        <button onClick={show} className={primaryButton}><Eye size={14} />View</button>
        {rows && <button onClick={() => downloadText('Party_Outstanding.csv', toCsv([['Party', 'Debit', 'Credit'], ...rows.map(r => [r.party, r.debit, r.credit])]))} className={exportButton}><Download size={14} />Export CSV</button>}
      </>}>
      {rows && (
        <div className={`${panel} overflow-auto`}>
          <table className="w-full text-xs">
            <thead className={tableHead}><tr><th className={th}>Party</th><th className={`${th} text-right`}>Debit</th><th className={`${th} text-right`}>Credit</th></tr></thead>
            <tbody className={tableBody}>
              {!rows.length && <tr><td colSpan={3} className="px-4 py-10 text-center text-stone-500">Nothing is outstanding.</td></tr>}
              {rows.map(r => <tr key={r.party}><td className="px-3 py-1.5">{r.party}</td><td className={num}>{r.debit ? money(r.debit) : ''}</td><td className={num}>{r.credit ? money(r.credit) : ''}</td></tr>)}
              {rows.length > 0 && <tr className="font-bold"><td className="px-3 py-2">Total</td><td className={num}>{money(dr)}</td><td className={num}>{money(cr)}</td></tr>}
            </tbody>
          </table>
        </div>
      )}
    </Page>
  );
}

export const BillWiseOutstanding = () => <BillWiseReport mode="outstanding" />;
export const BillWiseAgeing = () => <BillWiseReport mode="ageing" />;

const BUCKETS = ['Not due', '1-30 days', '31-60 days', '61-90 days', 'Over 90 days'];

export function BillWiseReport({ mode }) {
  const [type, setType] = useState('');
  const [asOn, setAsOn] = useState(todayIso());
  const [rows, setRows] = useState(null);
  const [problem, setProblem] = useState('');
  const ageing = mode === 'ageing';
  const show = () => {
    setProblem('');
    (ageing ? getBillAgeing(type, asOn) : getBillWise(type)).then(setRows).catch(e => { setRows(null); setProblem(errorText(e)); });
  };
  const head = ageing ? ['Ledger', ...BUCKETS, 'Total'] : ['Ledger', 'Type', 'Doc No', 'Date', 'Due', 'Amount', 'Adjusted', 'Balance'];
  const cells = (r) => (ageing ? [r.ledger, ...BUCKETS.map(b => r[b]), r.total] : [r.ledger, r.doc_type, r.doc_no, r.date, r.due_date, r.amount, r.adjusted, r.balance]);
  return (
    <Page title={ageing ? 'Bill wise Ageing' : 'Bill wise Outstanding'} sub={ageing ? 'Pending bills by how long past their due date.' : 'Pending bills per party, after adjustments.'} problem={problem}
      controls={<>
        <div><label htmlFor="bw-type" className={label}>Parties</label>
          <select id="bw-type" value={type} onChange={e => { setType(e.target.value); setRows(null); }} className={field}><option value="">All</option><option value="R">Receivable (customers)</option><option value="P">Payable (suppliers)</option></select></div>
        {ageing && <div><label htmlFor="bw-on" className={label}>As on</label><input id="bw-on" type="date" value={asOn} onChange={e => setAsOn(e.target.value)} className={`${field} tabular-nums`} /></div>}
        <button onClick={show} className={primaryButton}><Eye size={14} />View</button>
        {rows && <button onClick={() => downloadText(`${ageing ? 'Bill_wise_Ageing' : 'Bill_wise_Outstanding'}.csv`, toCsv([head, ...rows.map(cells)]))} className={exportButton}><Download size={14} />Export CSV</button>}
      </>}>
      {rows && (
        <div className={`${panel} overflow-auto`}>
          <table className="w-full text-xs">
            <thead className={tableHead}><tr>{head.map((h, i) => <th key={h} className={`${th} ${i > (ageing ? 0 : 4) || (!ageing && i > 4) ? 'text-right' : ''}`}>{h}</th>)}</tr></thead>
            <tbody className={tableBody}>
              {!rows.length && <tr><td colSpan={head.length} className="px-4 py-10 text-center text-stone-500">No pending bills.</td></tr>}
              {rows.map((r, n) => (
                <tr key={n}>{cells(r).map((c, i) => <td key={i} className={typeof c === 'number' ? num : 'px-3 py-1.5'}>{typeof c === 'number' ? money(c) : c}</td>)}</tr>
              ))}
            </tbody>
          </table>
        </div>
      )}
    </Page>
  );
}
