// Stock Balance report: opening, receipts, issues, closing and value per item for a date
// (GET /reports/stock-balance). Printable in the stock register formats.

import { useCallback, useEffect, useState } from 'react';
import { Printer } from 'lucide-react';
import { errorText, getItems, getStockBalance, getStockBalanceFormats, renderStockBalancePrint, todayIso } from '../../lib/api';
import { badge, money, pageHeader, pageSub, pageTitle, panel, tableBody, tableHead, tableRow, printButton } from '../../lib/styles';
import ReportPrintModal from '../print/ReportPrintModal';

type StockRow = { group: string; brand: string; name: string; pkg: string; opQty: number; recQty: number; issQty: number; clQty: number; rate: number; value: number | null };

export default function StockBalanceReport() {
  const [rows, setRows] = useState<StockRow[] | null>(null);
  const [error, setError] = useState('');
  const [showPrintModal, setShowPrintModal] = useState(false);
  const [formats, setFormats] = useState<string[]>([]);
  const day = todayIso();
  // Stable between renders, so the print window's preview is asked for once per format.
  const renderOn = useCallback((rpt: string, printer?: string) => renderStockBalancePrint(day, day, '', rpt, printer), [day]);

  useEffect(() => {
    getStockBalanceFormats().then(f => setFormats(f.formats)).catch(() => { /* printing says why */ });
  }, []);

  useEffect(() => {
    const date = todayIso();
    Promise.all([getItems(), getStockBalance(date)])
      .then(([items, balance]) => {
        const byCode = new Map(items.map(item => [item.code, item]));
        setRows(balance.map(row => ({
          group: row.group, brand: byCode.get(row.item_code)?.brand ?? '', name: row.item_name, pkg: row.package,
          opQty: row.op_qty, recQty: row.rec_qty, issQty: row.iss_qty, clQty: row.cl_qty,
          rate: byCode.get(row.item_code)?.mrp ?? 0, value: row.cl_value,
        })));
      })
      .catch(error => setError(errorText(error)));
  }, []);

  const th = 'px-4 py-2.5';
  return (
    <div className="mx-auto max-w-7xl">
      <div className={`${pageHeader} mb-5`}>
        <div>
          <h1 className={pageTitle}>Stock balance</h1>
          <p className={pageSub}>Today's excise opening, receipt, issue and closing balances.</p>
        </div>
        <div className="flex items-center gap-3">
          {rows && <span className={badge('neutral')}>{rows.length} items</span>}
          <button
            onClick={() => setShowPrintModal(true)}
            disabled={!rows || !rows.length}
            className={printButton}
          >
            <Printer size={14} />Print Stock Sheet
          </button>
        </div>
      </div>
      <div className={`${panel} overflow-x-auto`}>
        <table className="w-full text-left text-xs">
          <thead className={tableHead}>
            <tr>
              <th className={th}>Item</th><th className={th}>Group</th>
              <th className={`${th} text-center`}>Pkg</th>
              <th className={`${th} text-right`}>Opening</th><th className={`${th} text-right`}>Receipt</th>
              <th className={`${th} text-right`}>Issued</th><th className={`${th} text-right`}>Closing</th>
              <th className={`${th} text-right`}>Rate (₹)</th><th className={`${th} text-right`}>Closing value (₹)</th>
            </tr>
          </thead>
          <tbody className={tableBody}>
            {error && <tr><td colSpan={9} className="px-4 py-8 text-center text-[#9A4B45] dark:text-red-400">{error}</td></tr>}
            {!error && !rows && <tr><td colSpan={9} className="px-4 py-8 text-center text-[#70675F]">Loading stock…</td></tr>}
            {rows && !rows.length && <tr><td colSpan={9} className="px-4 py-8 text-center text-[#70675F]">No stock on record.</td></tr>}
            {rows?.map((row, index) => (
              <tr key={index} className={tableRow}>
                <td className="px-4 py-2.5">
                  <div className="font-medium text-[#29231F] dark:text-stone-50">{row.name}</div>
                  {row.brand && <div className="text-[11px] text-[#958B81] dark:text-stone-400">{row.brand}</div>}
                </td>
                <td className="px-4 py-2.5 text-[#70675F] dark:text-stone-300">{row.group}</td>
                <td className="px-4 py-2.5 text-center text-[#958B81]">{row.pkg}</td>
                <td className="px-4 py-2.5 text-right text-[#70675F] dark:text-stone-300">{row.opQty}</td>
                <td className="px-4 py-2.5 text-right text-[#55745A] dark:text-emerald-400">+{row.recQty}</td>
                <td className="px-4 py-2.5 text-right text-[#9A4B45] dark:text-red-400">−{row.issQty}</td>
                <td className="px-4 py-2.5 text-right font-semibold text-[#29231F] dark:text-stone-50">{row.clQty}</td>
                <td className="px-4 py-2.5 text-right text-[#70675F] dark:text-stone-300">{money(row.rate)}</td>
                <td className="px-4 py-2.5 text-right font-semibold text-[#29231F] dark:text-stone-50">{money(row.value)}</td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>

      {showPrintModal && rows && (
        <ReportPrintModal
          title={`Stock Balance (${day})`}
          formats={formats}
          render={renderOn}
          onClose={() => setShowPrintModal(false)}
        />
      )}
    </div>
  );
}