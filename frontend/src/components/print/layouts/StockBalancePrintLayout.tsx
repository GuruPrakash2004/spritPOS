// Print layout: stock balance / excise register on A4 landscape (legacy RptStk*.rpt).

import React from 'react';
import type { ApiCompany } from '../../../lib/api';
import { money } from '../../../lib/styles';
import { SAMPLE_STOCK_BALANCE_DATA } from '../samplePrintData';

interface StockRow {
  group: string;
  brand: string;
  name: string;
  pkg: string;
  opQty: number;
  recQty: number;
  issQty: number;
  clQty: number;
  rate: number;
  value: number;
}

interface StockBalancePrintLayoutProps {
  modelId: string;
  data: any;
  company: ApiCompany | null;
  watermark?: string;
  isPrintPreview?: boolean;
}

export const StockBalancePrintLayout: React.FC<StockBalancePrintLayoutProps> = ({
  modelId,
  data,
  company,
  watermark,
  isPrintPreview = false,
}) => {
  const storeName = company?.name || 'THE LIQUOR HOUSE';
  const storeLicence = company?.licence_type ? `${company.licence_type}` : 'CL-2 Retail Vend of Liquor';

  const rows: StockRow[] = (Array.isArray(data) && data.length > 0) ? data : SAMPLE_STOCK_BALANCE_DATA;
  const isNoValuation = modelId === 'RptStkBal_Without_CLvalue';

  // Compute totals
  const totalOp = rows.reduce((acc, r) => acc + (r.opQty || 0), 0);
  const totalRec = rows.reduce((acc, r) => acc + (r.recQty || 0), 0);
  const totalIss = rows.reduce((acc, r) => acc + (r.issQty || 0), 0);
  const totalCl = rows.reduce((acc, r) => acc + (r.clQty || 0), 0);
  const totalVal = rows.reduce((acc, r) => acc + (r.value || 0), 0);

  return (
    <div
      className={`rpt-print-container rpt-stock-bal font-sans text-stone-900 bg-white leading-normal select-none relative ${
        isPrintPreview
          ? 'p-8 shadow-xl border border-stone-300 rounded mx-auto max-w-[1000px] min-h-[700px]'
          : 'p-6'
      }`}
    >
      {watermark && watermark !== 'None' && (
        <div className="absolute inset-0 flex items-center justify-center pointer-events-none z-0">
          <span className="text-stone-200 font-black text-7xl uppercase tracking-widest -rotate-25 select-none opacity-40">
            {watermark}
          </span>
        </div>
      )}

      <div className="relative z-10 space-y-3">
        {/* Header */}
        <div className="border-b-2 border-stone-800 pb-2 flex justify-between items-end">
          <div>
            <div className="text-xl font-black uppercase tracking-tight">{storeName}</div>
            <div className="text-xs text-stone-600">Licence: {storeLicence} • Daily Stock Balance Register</div>
          </div>
          <div className="text-right">
            <span className="inline-block px-2.5 py-1 bg-stone-900 text-white font-bold text-xs uppercase tracking-wider rounded-xs">
              {isNoValuation ? 'PHYSICAL STOCK COUNT SHEET' : 'EXCISE STOCK BALANCE REGISTER'}
            </span>
            <div className="text-xs font-semibold text-stone-800 mt-1">
              Date: {new Date().toLocaleDateString('en-IN', { day: '2-digit', month: 'short', year: 'numeric' })}
            </div>
            <div className="text-[10px] text-stone-500">RPT: {modelId}.rpt ({rows.length} Items)</div>
          </div>
        </div>

        {/* Stock Matrix Table */}
        <table className="w-full border-collapse border border-stone-300 text-xs">
          <thead>
            <tr className="bg-stone-100 text-stone-800 border-b border-stone-300 font-bold">
              <th className="border-r border-stone-300 px-2 py-1.5 text-center w-8">#</th>
              <th className="border-r border-stone-300 px-2 py-1.5 text-left">Item Description</th>
              <th className="border-r border-stone-300 px-2 py-1.5 text-left w-24">Group</th>
              <th className="border-r border-stone-300 px-2 py-1.5 text-center w-14">Pkg</th>
              <th className="border-r border-stone-300 px-2 py-1.5 text-right w-14">Opening</th>
              <th className="border-r border-stone-300 px-2 py-1.5 text-right w-14">Receipt</th>
              <th className="border-r border-stone-300 px-2 py-1.5 text-right w-14">Issued</th>
              <th className="border-r border-stone-300 px-2 py-1.5 text-right w-14">Closing</th>
              {!isNoValuation && (
                <>
                  <th className="border-r border-stone-300 px-2 py-1.5 text-right w-16">Rate (₹)</th>
                  <th className="px-2 py-1.5 text-right w-24">Closing Value (₹)</th>
                </>
              )}
              {isNoValuation && (
                <th className="px-2 py-1.5 text-center w-28">Physical Count</th>
              )}
            </tr>
          </thead>
          <tbody className="divide-y divide-stone-200">
            {rows.map((row, idx) => (
              <tr key={idx} className="hover:bg-stone-50">
                <td className="border-r border-stone-300 px-2 py-1 text-center text-stone-500">{idx + 1}</td>
                <td className="border-r border-stone-300 px-2 py-1 font-medium">
                  {row.name}
                  {row.brand && <span className="ml-1 text-[10px] text-stone-500">({row.brand})</span>}
                </td>
                <td className="border-r border-stone-300 px-2 py-1 text-stone-600">{row.group}</td>
                <td className="border-r border-stone-300 px-2 py-1 text-center text-stone-500 font-mono text-[11px]">{row.pkg}</td>
                <td className="border-r border-stone-300 px-2 py-1 text-right tabular-nums">{row.opQty}</td>
                <td className="border-r border-stone-300 px-2 py-1 text-right tabular-nums text-emerald-700">+{row.recQty}</td>
                <td className="border-r border-stone-300 px-2 py-1 text-right tabular-nums text-red-700">−{row.issQty}</td>
                <td className="border-r border-stone-300 px-2 py-1 text-right tabular-nums font-bold">{row.clQty}</td>
                {!isNoValuation && (
                  <>
                    <td className="border-r border-stone-300 px-2 py-1 text-right tabular-nums">{money(row.rate)}</td>
                    <td className="px-2 py-1 text-right tabular-nums font-semibold">{money(row.value)}</td>
                  </>
                )}
                {isNoValuation && (
                  <td className="px-2 py-1 text-center">
                    <div className="border-b border-stone-400 w-16 mx-auto h-4"></div>
                  </td>
                )}
              </tr>
            ))}
          </tbody>
          <tfoot className="border-t-2 border-stone-400 bg-stone-100 font-bold text-stone-900">
            <tr>
              <td colSpan={4} className="border-r border-stone-300 px-3 py-1.5 text-right uppercase">
                Grand Totals ({rows.length} Items)
              </td>
              <td className="border-r border-stone-300 px-2 py-1.5 text-right tabular-nums">{totalOp}</td>
              <td className="border-r border-stone-300 px-2 py-1.5 text-right tabular-nums text-emerald-800">+{totalRec}</td>
              <td className="border-r border-stone-300 px-2 py-1.5 text-right tabular-nums text-red-800">−{totalIss}</td>
              <td className="border-r border-stone-300 px-2 py-1.5 text-right tabular-nums font-black">{totalCl}</td>
              {!isNoValuation && (
                <>
                  <td className="border-r border-stone-300 px-2 py-1.5 text-right">—</td>
                  <td className="px-2 py-1.5 text-right tabular-nums text-sm font-black">₹ {money(totalVal)}</td>
                </>
              )}
              {isNoValuation && <td className="px-2 py-1.5 text-center text-[10px] text-stone-500">Verified</td>}
            </tr>
          </tfoot>
        </table>

        {/* Footer Verification */}
        <div className="flex justify-between items-end pt-8 text-xs">
          <div className="text-[11px] text-stone-500">
            Certified that the closing balance entries are verified against physical shelf stocks.
          </div>
          <div className="flex gap-12 text-center">
            <div>
              <div className="border-t border-stone-400 w-36 pt-1 text-stone-700 font-semibold">Stock In-Charge</div>
            </div>
            <div>
              <div className="border-t border-stone-400 w-36 pt-1 text-stone-700 font-semibold">Excise Officer / Sign</div>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};
