// Print layout: 80 mm thermal POS receipt (legacy fmtSPInvPrint*.rpt).

import React from 'react';
import type { ApiCompany, ApiInvoice, ApiFullDsr } from '../../../lib/api';
import { money } from '../../../lib/styles';
import { numberToWords } from '../../../lib/formatters';
import { SAMPLE_INVOICE_DATA } from '../samplePrintData';

interface ThermalPosLayoutProps {
  modelId: string;
  data: unknown;
  company: ApiCompany | null;
  watermark?: string;
  isPrintPreview?: boolean;
}

export const ThermalPosLayout: React.FC<ThermalPosLayoutProps> = ({
  modelId,
  data,
  company,
  watermark,
  isPrintPreview = false,
}) => {
  const storeName = company?.name || 'THE LIQUOR HOUSE';
  const storeAddress = company?.address || 'No. 42, Brigade Road, Bangalore - 560001';
  const storePhone = company?.phone || company?.mobile || '+91 98450 12345';
  const storeLicence = company?.licence_type ? `${company.licence_type} (Retail)` : 'CL-2 (Retail Vend of Liquor)';
  const storeGstin = company?.gstin || '29AAAAA0000A1Z5';

  // The same slip prints an invoice or a DSR, depending on the data it is given.
  const given = (data && typeof data === 'object' ? data : null) as (Partial<ApiInvoice> & Partial<ApiFullDsr>) | null;

  // Render Sales Invoice Thermal Slip
  if (!given || !('sales_incl_gst' in given)) {
    const inv: ApiInvoice = given?.lines?.length
      ? given as ApiInvoice
      : (given && 'doc_no' in given ? { ...SAMPLE_INVOICE_DATA, ...given, lines: SAMPLE_INVOICE_DATA.lines } : SAMPLE_INVOICE_DATA);
    const isGstModel = modelId === 'fmtSPInvPrint_GST';
    const taxableBasic = inv.totals?.basic ?? 0;
    const cgstAmt = Math.round(taxableBasic * 0.09 * 100) / 100;
    const sgstAmt = Math.round(taxableBasic * 0.09 * 100) / 100;

    return (
      <div
        className={`rpt-print-container rpt-thermal-80 font-mono text-[12px] leading-tight text-black bg-white select-none ${
          isPrintPreview ? 'p-4 shadow-md border border-stone-300 rounded mx-auto max-w-[340px]' : 'p-2'
        }`}
      >
        {watermark && watermark !== 'None' && (
          <div className="watermark-badge text-center text-xs font-bold border-y border-dashed border-black py-0.5 mb-1.5 uppercase">
            *** {watermark} ***
          </div>
        )}

        {/* Store Header */}
        <div className="text-center space-y-0.5 pb-2">
          <div className="text-[15px] font-black tracking-tight uppercase">{storeName}</div>
          <div className="text-[11px] leading-tight text-stone-800">{storeAddress}</div>
          <div className="text-[11px] text-stone-700">Tel: {storePhone}</div>
          <div className="text-[11px] font-semibold">{storeLicence}</div>
          {isGstModel && <div className="text-[11px] font-bold">GSTIN: {storeGstin}</div>}
        </div>

        {/* Bill Metadata */}
        <div className="border-t border-dashed border-black pt-1.5 pb-1 text-[11px] space-y-0.5">
          <div className="flex justify-between font-bold">
            <span>BILL: {inv.doc_no}</span>
            <span>{inv.date} {inv.time || '14:32'}</span>
          </div>
          <div className="flex justify-between text-stone-700">
            <span>Counter: {inv.counter || 'C1'}</span>
            <span>By: {inv.salesperson || 'Staff'}</span>
          </div>
          <div className="flex justify-between text-stone-700">
            <span>Customer: {inv.customer || 'Cash Retail'}</span>
            <span>Mode: {inv.payment?.card ? 'CARD' : inv.payment?.upi ? 'UPI' : 'CASH'}</span>
          </div>
        </div>

        {/* Itemised Table */}
        <table className="w-full border-t border-b border-dashed border-black my-1 text-[11px]">
          <thead>
            <tr className="border-b border-dashed border-black">
              <th className="text-left py-1 font-bold">Item Description</th>
              <th className="text-center py-1 font-bold w-10">Qty</th>
              <th className="text-right py-1 font-bold w-14">Rate</th>
              <th className="text-right py-1 font-bold w-14">Amt</th>
            </tr>
          </thead>
          <tbody className="divide-y divide-dotted divide-stone-300">
            {inv.lines?.map((line, idx) => (
              <tr key={idx} className="align-top">
                <td className="py-1 pr-1">
                  <div className="font-semibold">{line.item_name}</div>
                  <div className="text-[10px] text-stone-500 font-sans">{line.item_code}</div>
                </td>
                <td className="py-1 text-center font-bold">{line.qty}</td>
                <td className="py-1 text-right">{money(line.rate)}</td>
                <td className="py-1 text-right font-bold">{money(line.amount)}</td>
              </tr>
            ))}
          </tbody>
        </table>

        {/* Totals Summary */}
        <div className="space-y-0.5 text-[11px] pt-1">
          <div className="flex justify-between">
            <span>Total Qty:</span>
            <span className="font-bold">{inv.totals?.qty ?? 0} bottles ({inv.totals?.litres ?? 0} L)</span>
          </div>
          <div className="flex justify-between">
            <span>Sub-Total:</span>
            <span>₹ {money(inv.totals?.basic ?? inv.totals?.net ?? 0)}</span>
          </div>

          {(inv.totals?.discount ?? 0) > 0 && (
            <div className="flex justify-between text-stone-700">
              <span>Discount Scheme:</span>
              <span>-₹ {money(inv.totals?.discount ?? 0)}</span>
            </div>
          )}

          {(inv.totals?.points_redeemed ?? 0) > 0 && (
            <div className="flex justify-between text-stone-700">
              <span>Points Redeemed:</span>
              <span>-₹ {money(inv.totals?.points_redeemed ?? 0)}</span>
            </div>
          )}

          {isGstModel && (
            <div className="border-t border-dotted border-stone-400 my-1 pt-1 text-[10px] space-y-0.5">
              <div className="flex justify-between">
                <span>Taxable Value:</span>
                <span>₹ {money(taxableBasic)}</span>
              </div>
              <div className="flex justify-between">
                <span>CGST @ 9%:</span>
                <span>₹ {money(cgstAmt)}</span>
              </div>
              <div className="flex justify-between">
                <span>SGST @ 9%:</span>
                <span>₹ {money(sgstAmt)}</span>
              </div>
            </div>
          )}

          <div className="border-t-2 border-black pt-1.5 flex justify-between items-baseline text-[14px] font-black">
            <span>NET PAYABLE:</span>
            <span>₹ {money(inv.totals?.net ?? 0)}</span>
          </div>

          <div className="text-[10px] italic text-stone-700 pt-0.5 border-b border-dashed border-black pb-1.5">
            {numberToWords(inv.totals?.net ?? 0)}
          </div>
        </div>

        {/* Payment Breakdown */}
        <div className="text-[11px] py-1 border-b border-dashed border-black space-y-0.5">
          <div className="flex justify-between">
            <span>Cash Paid:</span>
            <span>₹ {money(inv.payment?.cash ?? (inv.totals?.net ?? 0))}</span>
          </div>
          {(inv.payment?.card ?? 0) > 0 && (
            <div className="flex justify-between">
              <span>Card / POS:</span>
              <span>₹ {money(inv.payment?.card ?? 0)}</span>
            </div>
          )}
          {(inv.payment?.upi ?? 0) > 0 && (
            <div className="flex justify-between">
              <span>UPI / QR:</span>
              <span>₹ {money(inv.payment?.upi ?? 0)}</span>
            </div>
          )}
        </div>

        {/* Barcode & Footer */}
        <div className="pt-2 text-center space-y-1">
          <div className="inline-block tracking-[0.25em] font-bold text-xs bg-stone-100 px-3 py-1 border border-stone-400">
            *{inv.doc_no}*
          </div>
          <div className="text-[10px] text-stone-700">Liquor once sold cannot be returned or exchanged.</div>
          <div className="text-[10px] font-bold uppercase tracking-wider">Please Drink Responsibly • Don't Drink & Drive</div>
          <div className="text-[9px] text-stone-500 pt-1">Softvent vLQMS • Powered by Cloud POS</div>
        </div>
      </div>
    );
  }

  // Render DSR Compact Thermal Slip (RptDSR_S / RptDropsDSR)
  if (given && 'sales_incl_gst' in given) {
    const dsr = given as ApiFullDsr;
    return (
      <div
        className={`rpt-print-container rpt-thermal-80 font-mono text-[12px] leading-tight text-black bg-white select-none ${
          isPrintPreview ? 'p-4 shadow-md border border-stone-300 rounded mx-auto max-w-[340px]' : 'p-2'
        }`}
      >
        <div className="text-center pb-2 border-b border-dashed border-black">
          <div className="text-[14px] font-black uppercase">{storeName}</div>
          <div className="text-[11px]">DAILY AUDIT SUMMARY SLIP</div>
          <div className="text-[11px] font-bold mt-1">DATE: {dsr.date || new Date().toISOString().slice(0, 10)}</div>
        </div>

        <div className="py-2 space-y-1 text-[11px] border-b border-dashed border-black">
          <div className="flex justify-between font-bold text-[13px]">
            <span>TOTAL BILLS ISSUED:</span>
            <span>{dsr.total_bills}</span>
          </div>
          <div className="flex justify-between font-black text-[13px]">
            <span>NET REVENUE:</span>
            <span>₹ {money(dsr.sales_incl_gst)}</span>
          </div>
          <div className="flex justify-between text-stone-700">
            <span>Profit of the Day:</span>
            <span>+₹ {money(dsr.profit_of_the_day)}</span>
          </div>
        </div>

        <div className="py-2 space-y-1 text-[11px] border-b border-dashed border-black">
          <div className="font-bold text-[11px]">COLLECTIONS BY PAYMENT MODE:</div>
          {Object.entries(dsr.payment_modes || {}).map(([mode, amt]) => (
            <div key={mode} className="flex justify-between">
              <span>{mode}:</span>
              <span>₹ {money(amt)}</span>
            </div>
          ))}
        </div>

        <div className="py-2 space-y-1 text-[11px] border-b border-dashed border-black">
          <div className="font-bold text-[11px]">STOCK MOVEMENT RECAP:</div>
          <div className="flex justify-between">
            <span>Opening Stock:</span>
            <span>{dsr.opening_stock?.qty} btl (₹ {money(dsr.opening_stock?.value || 0)})</span>
          </div>
          <div className="flex justify-between">
            <span>Closing Stock:</span>
            <span>{dsr.closing_stock?.qty} btl ({dsr.closing_stock?.litres} L)</span>
          </div>
          <div className="flex justify-between">
            <span>Closing Value:</span>
            <span>₹ {money(dsr.closing_stock?.value || 0)}</span>
          </div>
        </div>

        <div className="pt-2 text-center text-[10px] text-stone-500">
          Generated via Softvent vLQMS Thermal Engine
        </div>
      </div>
    );
  }

  // Fallback generic thermal layout for stock deductions, expenses, returns
  return (
    <div
      className={`rpt-print-container rpt-thermal-80 font-mono text-[12px] leading-tight text-black bg-white select-none ${
        isPrintPreview ? 'p-4 shadow-md border border-stone-300 rounded mx-auto max-w-[340px]' : 'p-2'
      }`}
    >
      <div className="text-center pb-2 border-b border-dashed border-black">
        <div className="text-[14px] font-black uppercase">{storeName}</div>
        <div className="text-[11px] font-bold uppercase">{modelId.replace(/_/g, ' ')} VOUCHER</div>
        <div className="text-[10px]">Date: {new Date().toLocaleDateString('en-IN')}</div>
      </div>
      <div className="py-3 text-center text-xs text-stone-700">
        Voucher details ready for printing.
      </div>
      <div className="border-t border-dashed border-black pt-2 text-center text-[10px] text-stone-500">
        Softvent vLQMS Thermal POS
      </div>
    </div>
  );
};
