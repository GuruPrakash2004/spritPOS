// Print layout: monospaced text for 80-column dot-matrix printers on continuous stationery.

import React from 'react';
import type { ApiCompany, ApiInvoice } from '../../../lib/api';
import { money } from '../../../lib/styles';
import { numberToWords } from '../../../lib/formatters';
import { EMPTY_DOC } from '../printData';

interface DotMatrixLayoutProps {
  modelId: string;
  data: unknown;
  company: ApiCompany | null;
  watermark?: string;
  isPrintPreview?: boolean;
}

export const DotMatrixLayout: React.FC<DotMatrixLayoutProps> = ({
  modelId,
  data,
  company,
  watermark,
  isPrintPreview = false,
}) => {
  const storeName = company?.name || '';
  const storeAddress = company?.address || '';
  const storeLicence = company?.licence_type || '';

  const given = data as Partial<ApiInvoice> | null;
  const inv: ApiInvoice = { ...EMPTY_DOC, ...given } as ApiInvoice;

  return (
    <div
      className={`rpt-print-container rpt-dmp font-mono text-[11px] leading-snug text-black bg-[#FAF9F5] select-none ${
        isPrintPreview
          ? 'p-6 shadow-md border-y-8 border-x-4 border-dashed border-stone-400 rounded mx-auto max-w-[640px]'
          : 'p-4'
      }`}
    >
      {/* DMP Tractor Feed Holes Simulation in preview */}
      {isPrintPreview && (
        <div className="flex justify-between pb-3 text-stone-400 text-[10px] select-none">
          <span>○ ○ ○ ○ ○ ○ ○ ○ ○ ○ ○ ○ ○ ○ ○ ○ ○ ○ ○ ○</span>
          <span>○ ○ ○ ○ ○ ○ ○ ○ ○ ○ ○ ○ ○ ○ ○ ○ ○ ○ ○ ○</span>
        </div>
      )}

      {watermark && watermark !== 'None' && (
        <div className="text-center font-bold text-xs uppercase mb-1">
          *** {watermark} ***
        </div>
      )}

      {/* Header */}
      <div className="text-center font-bold">
        <div>{storeName.toUpperCase()}</div>
        <div className="text-[10px]">{storeAddress}</div>
        <div className="text-[10px]">{storeLicence}</div>
        <div>================================================================</div>
        <div className="flex justify-between text-[10px]">
          <span>BILL: {inv?.doc_no}</span>
          <span>DATE: {inv?.date} {inv?.time}</span>
        </div>
        <div className="flex justify-between text-[10px]">
          <span>COUNTER: {inv?.counter}</span>
          <span>CUST: {inv?.customer}</span>
        </div>
        <div>----------------------------------------------------------------</div>
      </div>

      {/* Items Table in plain monospace */}
      <div className="my-1">
        <div className="flex justify-between font-bold text-[10px] border-b border-black pb-0.5">
          <span className="w-8">SN</span>
          <span className="flex-1 text-left">ITEM DESCRIPTION</span>
          <span className="w-12 text-right">QTY</span>
          <span className="w-16 text-right">RATE</span>
          <span className="w-18 text-right">AMOUNT</span>
        </div>
        <div className="divide-y divide-dotted divide-stone-400 py-0.5">
          {inv?.lines?.map((line, idx) => (
            <div key={idx} className="flex justify-between text-[10px] py-0.5">
              <span className="w-8">{idx + 1}</span>
              <span className="flex-1 text-left truncate pr-1">{line.item_name}</span>
              <span className="w-12 text-right">{line.qty}</span>
              <span className="w-16 text-right">{money(line.rate)}</span>
              <span className="w-18 text-right font-bold">{money(line.amount)}</span>
            </div>
          ))}
        </div>
      </div>

      {/* Summary */}
      <div className="pt-1 text-[10px]">
        <div>----------------------------------------------------------------</div>
        <div className="flex justify-between">
          <span>TOTAL BOTTLES: {inv?.totals?.qty ?? 0} ({inv?.totals?.litres ?? 0} LTR)</span>
          <span className="font-bold">GROSS: {money(inv?.totals?.basic ?? inv?.totals?.net ?? 0)}</span>
        </div>
        {(inv?.totals?.discount ?? 0) > 0 && (
          <div className="flex justify-between">
            <span>DISCOUNT:</span>
            <span>-{money(inv?.totals?.discount ?? 0)}</span>
          </div>
        )}
        <div className="flex justify-between font-bold text-xs pt-1">
          <span>NET AMOUNT:</span>
          <span>Rs. {money(inv?.totals?.net ?? 0)}</span>
        </div>
        <div>================================================================</div>
        <div className="italic text-[9px] pt-0.5">
          {numberToWords(inv?.totals?.net ?? 0)}
        </div>
        <div className="text-center pt-2 text-[9px]">
          THANK YOU • VISIT AGAIN • SOFTVENT VLQMS ({modelId}.rpt)
        </div>
      </div>

      {/* Bottom tractor holes */}
      {isPrintPreview && (
        <div className="flex justify-between pt-4 text-stone-400 text-[10px] select-none">
          <span>○ ○ ○ ○ ○ ○ ○ ○ ○ ○ ○ ○ ○ ○ ○ ○ ○ ○ ○ ○</span>
          <span>○ ○ ○ ○ ○ ○ ○ ○ ○ ○ ○ ○ ○ ○ ○ ○ ○ ○ ○ ○</span>
        </div>
      )}
    </div>
  );
};
