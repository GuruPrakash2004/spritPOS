// Print layout: half-page A5 retail cash memo (legacy fmtSInv.rpt).

import React from 'react';
import type { ApiCompany, ApiInvoice } from '../../../lib/api';
import { money } from '../../../lib/styles';
import { numberToWords } from '../../../lib/formatters';
import { SAMPLE_INVOICE_DATA } from '../samplePrintData';

interface A5SlipLayoutProps {
  modelId: string;
  data: unknown;
  company: ApiCompany | null;
  watermark?: string;
  isPrintPreview?: boolean;
}

export const A5SlipLayout: React.FC<A5SlipLayoutProps> = ({
  modelId,
  data,
  company,
  watermark,
  isPrintPreview = false,
}) => {
  const storeName = company?.name || 'THE LIQUOR HOUSE';
  const storeAddress = company?.address || 'No. 42, Brigade Road, Bangalore - 560001';
  const storeLicence = company?.licence_type ? `${company.licence_type}` : 'CL-2 Retail';
  const storeGstin = company?.gstin || '29AAAAA0000A1Z5';

  const given = data as Partial<ApiInvoice> | null;
  const inv: ApiInvoice = given?.lines?.length ? given as ApiInvoice : SAMPLE_INVOICE_DATA;

  return (
    <div
      className={`rpt-print-container rpt-a5 font-sans text-stone-900 bg-white leading-tight select-none relative ${
        isPrintPreview ? 'p-6 shadow-lg border border-stone-300 rounded mx-auto max-w-[650px] min-h-[460px]' : 'p-4'
      }`}
    >
      {watermark && watermark !== 'None' && (
        <div className="absolute inset-0 flex items-center justify-center pointer-events-none z-0">
          <span className="text-stone-200 font-black text-6xl uppercase tracking-widest -rotate-25 select-none opacity-40">
            {watermark}
          </span>
        </div>
      )}

      <div className="relative z-10 space-y-2">
        {/* Compact Header */}
        <div className="flex justify-between items-start border-b border-stone-800 pb-2">
          <div>
            <div className="text-lg font-black tracking-tight uppercase">{storeName}</div>
            <div className="text-[11px] text-stone-600">{storeAddress}</div>
            <div className="text-[10px] text-stone-500 font-semibold">{storeLicence} • GSTIN: {storeGstin}</div>
          </div>
          <div className="text-right">
            <span className="inline-block px-2 py-0.5 bg-stone-800 text-white font-bold text-[11px] uppercase rounded-xs">
              RETAIL CASH MEMO
            </span>
            <div className="text-[11px] font-mono font-bold mt-1">NO: {inv?.doc_no || '26/c10-0083718'}</div>
            <div className="text-[10px] text-stone-500">{inv?.date || '2026-09-26'} {inv?.time || ''}</div>
          </div>
        </div>

        {/* Customer & Counter Info */}
        <div className="flex justify-between text-[11px] bg-stone-50 p-1.5 border border-stone-200 rounded-xs">
          <span>Customer: <strong>{inv?.customer || 'CASH'}</strong></span>
          <span>Counter: <strong>{inv?.counter || 'Counter 1'}</strong></span>
          <span>Staff: <strong>{inv?.salesperson || 'Staff'}</strong></span>
          <span>Payment: <strong>{inv?.payment?.card ? 'CARD' : inv?.payment?.upi ? 'UPI' : 'CASH'}</strong></span>
        </div>

        {/* Items Table */}
        <table className="w-full border-collapse border border-stone-300 text-[11px]">
          <thead>
            <tr className="bg-stone-100 border-b border-stone-300 font-bold text-stone-700">
              <th className="border-r border-stone-300 px-2 py-1 text-center w-6">#</th>
              <th className="border-r border-stone-300 px-2 py-1 text-left">Item Name</th>
              <th className="border-r border-stone-300 px-2 py-1 text-center w-12">Qty</th>
              <th className="border-r border-stone-300 px-2 py-1 text-right w-16">Rate (₹)</th>
              <th className="px-2 py-1 text-right w-20">Amount (₹)</th>
            </tr>
          </thead>
          <tbody className="divide-y divide-stone-200">
            {inv?.lines?.map((line, idx) => (
              <tr key={idx}>
                <td className="border-r border-stone-300 px-2 py-1 text-center text-stone-500">{idx + 1}</td>
                <td className="border-r border-stone-300 px-2 py-1 font-medium">{line.item_name}</td>
                <td className="border-r border-stone-300 px-2 py-1 text-center font-bold">{line.qty}</td>
                <td className="border-r border-stone-300 px-2 py-1 text-right tabular-nums">{money(line.rate)}</td>
                <td className="px-2 py-1 text-right font-bold tabular-nums">{money(line.amount)}</td>
              </tr>
            ))}
          </tbody>
          <tfoot className="border-t-2 border-stone-400 bg-stone-50 font-bold">
            <tr>
              <td colSpan={2} className="border-r border-stone-300 px-2 py-1 text-right">TOTAL</td>
              <td className="border-r border-stone-300 px-2 py-1 text-center">{inv?.totals?.qty ?? 0}</td>
              <td className="border-r border-stone-300 px-2 py-1 text-right">—</td>
              <td className="px-2 py-1 text-right text-xs">₹ {money(inv?.totals?.net ?? 0)}</td>
            </tr>
          </tfoot>
        </table>

        {/* Footer with Amount in words & Sign */}
        <div className="flex justify-between items-end pt-2 text-[10px] border-t border-stone-200">
          <div>
            <div className="italic text-stone-700">Rupees {numberToWords(inv?.totals?.net ?? 0)} only</div>
            <div className="text-stone-400 mt-1">Thank you, visit again! • Liquor once sold cannot be returned. • {modelId}.rpt</div>
          </div>
          <div className="text-right">
            <div className="border-t border-stone-400 w-32 pt-0.5 text-stone-600 font-semibold text-center">
              Authorized Sign
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};
