import React from 'react';
import type { ApiCompany, ApiInvoice } from '../../api';
import { money } from '../../ui';
import { numberToWords } from '../../formatters';

interface A4GstTaxInvoiceLayoutProps {
  modelId: string;
  data: any;
  company: ApiCompany | null;
  watermark?: string;
  isPrintPreview?: boolean;
}

export const A4GstTaxInvoiceLayout: React.FC<A4GstTaxInvoiceLayoutProps> = ({
  modelId,
  data,
  company,
  watermark,
  isPrintPreview = false,
}) => {
  const storeName = company?.name || 'THE LIQUOR HOUSE';
  const storeAddress = company?.address || 'No. 42, Brigade Road, Civil Station, Bangalore - 560001, Karnataka';
  const storePhone = company?.phone || company?.mobile || '+91 98450 12345';
  const storeEmail = company?.email || 'accounts@theliquorhouse.in';
  const storeLicence = company?.licence_type ? `${company.licence_type}` : 'CL-2 (Retail Vend of Foreign Liquor & Beer)';
  const storeGstin = company?.gstin || '29AAAAA0000A1Z5';
  const storePan = company?.pan || 'AAAAA0000A';

  const inv = data as ApiInvoice;

  // Determine document title based on modelId
  const docTitle = modelId.includes('PB')
    ? 'PURCHASE INWARD GOODS NOTE'
    : modelId.includes('PI')
    ? 'KSBCL PURCHASE INDENT ORDER'
    : modelId.includes('STKDED')
    ? 'STOCK DEDUCTION & BREAKAGE VOUCHER'
    : 'TAX INVOICE / CASH MEMO';

  return (
    <div
      className={`rpt-print-container rpt-a4 font-sans text-stone-900 bg-white leading-normal select-none relative ${
        isPrintPreview ? 'p-8 shadow-xl border border-stone-300 rounded-sm mx-auto max-w-[794px] min-h-[1050px]' : 'p-6'
      }`}
    >
      {/* Watermark overlay */}
      {watermark && watermark !== 'None' && (
        <div className="absolute inset-0 flex items-center justify-center pointer-events-none z-0 overflow-hidden">
          <span className="text-stone-200 dark:text-stone-200/20 font-black text-7xl uppercase tracking-widest -rotate-45 select-none opacity-40">
            {watermark}
          </span>
        </div>
      )}

      <div className="relative z-10 space-y-4">
        {/* Top Header */}
        <div className="border-b-2 border-stone-800 pb-3 flex justify-between items-start gap-4">
          <div>
            <h1 className="text-2xl font-black tracking-tight uppercase text-stone-900">{storeName}</h1>
            <p className="text-xs text-stone-600 max-w-md">{storeAddress}</p>
            <div className="mt-1 flex flex-wrap gap-x-4 text-xs font-semibold text-stone-700">
              <span>GSTIN: <strong className="text-stone-900">{storeGstin}</strong></span>
              <span>PAN: <strong className="text-stone-900">{storePan}</strong></span>
              <span>Licence: <strong className="text-stone-900">{storeLicence}</strong></span>
            </div>
            <div className="text-xs text-stone-500">Contact: {storePhone} • {storeEmail}</div>
          </div>
          <div className="text-right flex flex-col items-end">
            <span className="inline-block px-3 py-1 bg-stone-900 text-white font-bold text-xs uppercase tracking-wider rounded-sm">
              {docTitle}
            </span>
            <span className="mt-1 text-[11px] text-stone-500 font-medium">Original for Recipient</span>
          </div>
        </div>

        {/* Invoice Metadata Grid */}
        <div className="grid grid-cols-2 gap-4 border border-stone-300 rounded-sm p-3 text-xs bg-stone-50/50">
          <div className="space-y-1">
            <div className="flex">
              <span className="w-28 text-stone-500 font-medium">Invoice No:</span>
              <strong className="text-stone-900 font-mono text-sm">{inv?.doc_no || '26/c10-0083718'}</strong>
            </div>
            <div className="flex">
              <span className="w-28 text-stone-500 font-medium">Date & Time:</span>
              <span className="text-stone-800">{inv?.date || '2026-09-26'} {inv?.time || '14:30'}</span>
            </div>
            <div className="flex">
              <span className="w-28 text-stone-500 font-medium">Sales Counter:</span>
              <span className="text-stone-800">{inv?.counter || 'Counter 1'}</span>
            </div>
            <div className="flex">
              <span className="w-28 text-stone-500 font-medium">Salesperson:</span>
              <span className="text-stone-800">{inv?.salesperson || 'Staff'}</span>
            </div>
          </div>

          <div className="space-y-1 border-l border-stone-300 pl-4">
            <div className="flex">
              <span className="w-28 text-stone-500 font-medium">Customer:</span>
              <strong className="text-stone-900">{inv?.customer || 'Retail Cash Customer'}</strong>
            </div>
            <div className="flex">
              <span className="w-28 text-stone-500 font-medium">Payment Mode:</span>
              <span className="text-stone-800 font-semibold">{inv?.payment?.card ? 'Card Swipe' : inv?.payment?.upi ? 'UPI QR' : 'Cash'}</span>
            </div>
            <div className="flex">
              <span className="w-28 text-stone-500 font-medium">Place of Supply:</span>
              <span className="text-stone-800">Karnataka (State Code 29)</span>
            </div>
          </div>
        </div>

        {/* Line Items Table */}
        <table className="w-full border-collapse border border-stone-300 text-xs">
          <thead>
            <tr className="bg-stone-100 text-stone-700 border-b border-stone-300 font-bold">
              <th className="border-r border-stone-300 px-2 py-2 text-center w-8">#</th>
              <th className="border-r border-stone-300 px-3 py-2 text-left">Description of Goods</th>
              <th className="border-r border-stone-300 px-2 py-2 text-center w-16">HSN Code</th>
              <th className="border-r border-stone-300 px-2 py-2 text-right w-14">Qty</th>
              <th className="border-r border-stone-300 px-2 py-2 text-right w-20">Rate (₹)</th>
              <th className="border-r border-stone-300 px-2 py-2 text-right w-20">Basic (₹)</th>
              <th className="border-r border-stone-300 px-2 py-2 text-right w-14">GST %</th>
              <th className="px-3 py-2 text-right w-24">Total (₹)</th>
            </tr>
          </thead>
          <tbody className="divide-y divide-stone-200">
            {inv?.lines?.map((line, idx) => {
              const basic = (line.qty * line.rate) / 1.18; // illustrative calculation for display
              return (
                <tr key={idx} className="hover:bg-stone-50">
                  <td className="border-r border-stone-300 px-2 py-2 text-center text-stone-500">{idx + 1}</td>
                  <td className="border-r border-stone-300 px-3 py-2">
                    <div className="font-semibold text-stone-900">{line.item_name}</div>
                    <div className="text-[10px] text-stone-500 font-mono">{line.item_code}</div>
                  </td>
                  <td className="border-r border-stone-300 px-2 py-2 text-center font-mono text-[11px] text-stone-600">220890</td>
                  <td className="border-r border-stone-300 px-2 py-2 text-right font-bold text-stone-900">{line.qty}</td>
                  <td className="border-r border-stone-300 px-2 py-2 text-right tabular-nums">{money(line.rate)}</td>
                  <td className="border-r border-stone-300 px-2 py-2 text-right tabular-nums">{money(basic)}</td>
                  <td className="border-r border-stone-300 px-2 py-2 text-right text-stone-600">0%*</td>
                  <td className="px-3 py-2 text-right font-bold text-stone-900 tabular-nums">{money(line.amount)}</td>
                </tr>
              );
            })}
          </tbody>
          <tfoot className="border-t-2 border-stone-400 bg-stone-50 font-bold text-stone-900">
            <tr>
              <td colSpan={3} className="border-r border-stone-300 px-3 py-2 text-right uppercase">
                Totals ({inv?.lines?.length ?? 0} Items)
              </td>
              <td className="border-r border-stone-300 px-2 py-2 text-right">{inv?.totals?.qty ?? 0}</td>
              <td className="border-r border-stone-300 px-2 py-2 text-right">—</td>
              <td className="border-r border-stone-300 px-2 py-2 text-right tabular-nums">
                ₹ {money(inv?.totals?.basic ?? inv?.totals?.net ?? 0)}
              </td>
              <td className="border-r border-stone-300 px-2 py-2 text-right">—</td>
              <td className="px-3 py-2 text-right tabular-nums text-sm">
                ₹ {money(inv?.totals?.net ?? 0)}
              </td>
            </tr>
          </tfoot>
        </table>

        {/* Amount in words & Notes */}
        <div className="grid grid-cols-3 gap-4 border border-stone-300 rounded-sm p-3 text-xs">
          <div className="col-span-2 space-y-1">
            <span className="text-stone-500 font-medium">Invoice Value in Words:</span>
            <div className="font-semibold text-stone-800 italic">
              INR {numberToWords(inv?.totals?.net ?? 0)} Only
            </div>
            <div className="mt-3 text-[10px] text-stone-500 leading-tight">
              *Note: Under Section 9(2) of the CGST Act, alcoholic liquor for human consumption is subject to State Excise duties and exempted from Central Goods and Services Tax.
            </div>
          </div>

          <div className="space-y-1 text-right tabular-nums border-l border-stone-200 pl-4">
            <div className="flex justify-between text-stone-600">
              <span>Gross Amount:</span>
              <span>₹ {money(inv?.totals?.basic ?? inv?.totals?.net ?? 0)}</span>
            </div>
            {(inv?.totals?.discount ?? 0) > 0 && (
              <div className="flex justify-between text-emerald-700">
                <span>Discount / Schemes:</span>
                <span>-₹ {money(inv?.totals?.discount ?? 0)}</span>
              </div>
            )}
            <div className="flex justify-between font-bold text-stone-900 text-sm border-t border-stone-300 pt-1">
              <span>Final Net Amount:</span>
              <span>₹ {money(inv?.totals?.net ?? 0)}</span>
            </div>
          </div>
        </div>

        {/* Declarations & Signatures */}
        <div className="grid grid-cols-2 gap-8 pt-8 text-xs border-t border-stone-200">
          <div>
            <div className="font-bold text-stone-800">Terms & Conditions:</div>
            <ol className="list-decimal list-inside text-[11px] text-stone-600 space-y-0.5 mt-1">
              <li>Goods once sold will not be taken back or exchanged.</li>
              <li>Sale prohibited to persons below 21 years of age.</li>
              <li>Subject to Bangalore jurisdiction.</li>
            </ol>
          </div>
          <div className="text-right flex flex-col justify-between items-end h-24">
            <div className="text-xs font-bold text-stone-800">For {storeName}</div>
            <div className="border-t border-stone-400 w-48 text-center pt-1 text-[11px] text-stone-600">
              Authorized Signatory
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};
