// Print layout: Daily Summary Report audit sheet (legacy RptDSR*.rpt).

import React from 'react';
import type { ApiCompany, ApiFullDsr } from '../../../lib/api';
import { money } from '../../../lib/styles';
import { SAMPLE_DSR_DATA } from '../samplePrintData';

interface DsrAuditLayoutProps {
  modelId: string;
  data: unknown;
  company: ApiCompany | null;
  watermark?: string;
  isPrintPreview?: boolean;
}

export const DsrAuditLayout: React.FC<DsrAuditLayoutProps> = ({
  modelId,
  data,
  company,
  watermark,
  isPrintPreview = false,
}) => {
  const storeName = company?.name || 'THE LIQUOR HOUSE';
  const storeAddress = company?.address || 'No. 42, Brigade Road, Bangalore - 560001';
  const storeLicence = company?.licence_type ? `${company.licence_type}` : 'CL-2 Retail Vend of Liquor';
  const storeGstin = company?.gstin || '29AAAAA0000A1Z5';

  const given = data as Partial<ApiFullDsr> | null;
  const dsr: ApiFullDsr = (given && (given.sales_incl_gst || given.total_bills)) ? given as ApiFullDsr : SAMPLE_DSR_DATA;
  const isDmp = modelId === 'RptDSR_8.5';

  return (
    <div
      className={`rpt-print-container rpt-dsr text-stone-900 bg-white leading-normal select-none relative ${
        isDmp ? 'font-mono text-[11px]' : 'font-sans text-xs'
      } ${
        isPrintPreview ? 'p-8 shadow-xl border border-stone-300 rounded mx-auto max-w-[820px] min-h-[1050px]' : 'p-6'
      }`}
    >
      {watermark && watermark !== 'None' && (
        <div className="absolute inset-0 flex items-center justify-center pointer-events-none z-0">
          <span className="text-stone-200 font-black text-7xl uppercase tracking-widest -rotate-45 select-none opacity-40">
            {watermark}
          </span>
        </div>
      )}

      <div className="relative z-10 space-y-4">
        {/* Header */}
        <div className="border-b-2 border-stone-900 pb-3 flex justify-between items-start">
          <div>
            <h1 className="text-xl font-black uppercase tracking-tight">{storeName}</h1>
            <p className="text-xs text-stone-600">{storeAddress}</p>
            <div className="text-[11px] font-semibold text-stone-700">Licence: {storeLicence} • GSTIN: {storeGstin}</div>
          </div>
          <div className="text-right">
            <span className="inline-block px-3 py-1 bg-stone-900 text-white font-bold text-xs uppercase tracking-wider rounded-xs">
              DAILY SUMMARY REPORT (DSR)
            </span>
            <div className="mt-1 text-xs font-bold text-stone-800">
              AUDIT DATE: {dsr.date || new Date().toISOString().slice(0, 10)}
            </div>
            <div className="text-[10px] text-stone-500">Report Template: {modelId}.rpt</div>
          </div>
        </div>

        {/* Executive KPI Summary Cards */}
        <div className="grid grid-cols-4 gap-2 text-xs">
          <div className="border border-stone-300 p-2.5 rounded-xs bg-stone-50">
            <div className="text-[10px] uppercase font-bold text-stone-500">Total Bills Issued</div>
            <div className="text-lg font-black text-stone-900 mt-0.5">{dsr.total_bills}</div>
            <div className="text-[10px] text-stone-500">Across active counters</div>
          </div>
          <div className="border border-stone-300 p-2.5 rounded-xs bg-stone-50">
            <div className="text-[10px] uppercase font-bold text-stone-500">Sales (incl. GST)</div>
            <div className="text-lg font-black text-stone-900 mt-0.5">₹ {money(dsr.sales_incl_gst)}</div>
            <div className="text-[10px] text-emerald-700 font-semibold">+₹ {money(dsr.profit_of_the_day)} profit</div>
          </div>
          <div className="border border-stone-300 p-2.5 rounded-xs bg-stone-50">
            <div className="text-[10px] uppercase font-bold text-stone-500">Stock Valuation</div>
            <div className="text-lg font-black text-stone-900 mt-0.5">₹ {money(dsr.closing_stock?.value || 0)}</div>
            <div className="text-[10px] text-stone-500">{dsr.closing_stock?.qty} btl • {dsr.closing_stock?.litres} L</div>
          </div>
          <div className="border border-stone-300 p-2.5 rounded-xs bg-stone-50">
            <div className="text-[10px] uppercase font-bold text-stone-500">Net Cash Balance</div>
            <div className="text-lg font-black text-stone-900 mt-0.5">₹ {money(dsr.cash_balance)}</div>
            <div className="text-[10px] text-stone-500">Cash in drawer</div>
          </div>
        </div>

        {/* Financial Flow Section */}
        <div className="border border-stone-300 rounded-xs overflow-hidden">
          <div className="bg-stone-100 px-3 py-1.5 font-bold border-b border-stone-300 text-xs text-stone-800">
            DAILY FINANCIAL & REVENUE RECONCILIATION
          </div>
          <table className="w-full text-xs">
            <tbody className="divide-y divide-stone-200">
              <tr className="flex justify-between px-3 py-1.5">
                <td className="text-stone-700">Opening Stock Value (as of 06:00 AM)</td>
                <td className="font-bold tabular-nums">₹ {money(dsr.opening_stock?.value || 0)}</td>
              </tr>
              <tr className="flex justify-between px-3 py-1.5">
                <td className="text-stone-700">(+) Today's Inward Purchases (incl. GST)</td>
                <td className="font-bold tabular-nums text-emerald-800">+₹ {money(dsr.purchase?.incl_gst || 0)}</td>
              </tr>
              <tr className="flex justify-between px-3 py-1.5">
                <td className="text-stone-700">(−) Today's Total Gross Sales</td>
                <td className="font-bold tabular-nums text-red-800">−₹ {money(dsr.sales_incl_gst)}</td>
              </tr>
              <tr className="flex justify-between px-3 py-1.5">
                <td className="text-stone-700">(+) Sales Returns Accepted</td>
                <td className="font-bold tabular-nums">₹ {money(dsr.sales_returns || 0)}</td>
              </tr>
              <tr className="flex justify-between px-3 py-1.5">
                <td className="text-stone-700">(−) Operating & Petty Cash Expenses</td>
                <td className="font-bold tabular-nums text-amber-800">−₹ {money(dsr.expenses || 0)}</td>
              </tr>
              <tr className="flex justify-between px-3 py-1.5 bg-stone-50 font-bold border-t border-stone-300">
                <td>Closing Stock Valuation (Physical EOD)</td>
                <td className="tabular-nums">₹ {money(dsr.closing_stock?.value || 0)}</td>
              </tr>
            </tbody>
          </table>
        </div>

        {/* Two-Column Breakdowns */}
        <div className="grid grid-cols-2 gap-4 text-xs">
          {/* Payment Modes */}
          <div className="border border-stone-300 rounded-xs overflow-hidden">
            <div className="bg-stone-100 px-3 py-1.5 font-bold border-b border-stone-300 text-stone-800">
              Collection by Payment Mode
            </div>
            <table className="w-full">
              <tbody className="divide-y divide-stone-200">
                {Object.entries(dsr.payment_modes || {}).map(([mode, amt]) => (
                  <tr key={mode} className="flex justify-between px-3 py-1">
                    <td className="text-stone-700">{mode}</td>
                    <td className="font-semibold tabular-nums">₹ {money(amt)}</td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>

          {/* Counter-wise Sales */}
          <div className="border border-stone-300 rounded-xs overflow-hidden">
            <div className="bg-stone-100 px-3 py-1.5 font-bold border-b border-stone-300 text-stone-800">
              Counter-wise Sales Breakdown
            </div>
            <table className="w-full">
              <tbody className="divide-y divide-stone-200">
                {Object.entries(dsr.counter_wise || {}).map(([counter, amt]) => (
                  <tr key={counter} className="flex justify-between px-3 py-1">
                    <td className="text-stone-700">{counter}</td>
                    <td className="font-semibold tabular-nums">₹ {money(amt)}</td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        </div>

        {/* Liquor Category Sales Breakdown */}
        <div className="border border-stone-300 rounded-xs overflow-hidden">
          <div className="bg-stone-100 px-3 py-1.5 font-bold border-b border-stone-300 text-stone-800">
            Category Group-wise Sales
          </div>
          <div className="grid grid-cols-3 divide-x divide-stone-200 p-2">
            {Object.entries(dsr.group_wise_sales || {}).map(([grp, amt]) => (
              <div key={grp} className="px-2 py-1 flex justify-between">
                <span className="text-stone-600">{grp}:</span>
                <span className="font-bold tabular-nums">₹ {money(amt)}</span>
              </div>
            ))}
          </div>
        </div>

        {/* Audit Signatures */}
        <div className="grid grid-cols-3 gap-6 pt-12 text-center text-xs">
          <div>
            <div className="border-t border-stone-400 pt-1 font-semibold text-stone-800">Chief Cashier</div>
            <div className="text-[10px] text-stone-500">Drawer Count Verified</div>
          </div>
          <div>
            <div className="border-t border-stone-400 pt-1 font-semibold text-stone-800">Store Manager</div>
            <div className="text-[10px] text-stone-500">Excise Book Entry Checked</div>
          </div>
          <div>
            <div className="border-t border-stone-400 pt-1 font-semibold text-stone-800">Licensee / Partner</div>
            <div className="text-[10px] text-stone-500">Final Verification</div>
          </div>
        </div>
      </div>
    </div>
  );
};
