import React from 'react';
import type { ApiCompany } from '../../api';
import { money } from '../../ui';

export interface AnalyticalReportData {
  title: string;
  subtitle?: string;
  columns: Array<{ key: string; label: string; kind?: 'text' | 'number' | 'money'; total?: boolean }>;
  rows: Array<Record<string, string | number | null>>;
}

interface AnalyticalReportLayoutProps {
  modelId: string;
  data: AnalyticalReportData;
  company: ApiCompany | null;
  watermark?: string;
  isPrintPreview?: boolean;
}

export const AnalyticalReportLayout: React.FC<AnalyticalReportLayoutProps> = ({
  modelId,
  data,
  company,
  watermark,
  isPrintPreview = false,
}) => {
  const storeName = company?.name || 'THE LIQUOR HOUSE';
  const storeLicence = company?.licence_type ? `${company.licence_type}` : 'CL-2 Retail';

  const title = data?.title || 'Analytical Register';
  const subtitle = data?.subtitle || '';
  const columns = data?.columns || [];
  const rows = data?.rows || [];

  const isLandscape = modelId.includes('Landscape') || modelId.includes('UDRSales');

  const formatVal = (col: { kind?: 'text' | 'number' | 'money' }, val: unknown) => {
    if (val === undefined || val === null || val === '') return '—';
    if (col.kind === 'money') return `₹ ${money(Number(val))}`;
    return String(val);
  };

  const totals = columns.map(col =>
    col.total && rows.length
      ? rows.reduce((acc, r) => acc + (Number(r[col.key]) || 0), 0)
      : null
  );

  return (
    <div
      className={`rpt-print-container rpt-analytical font-sans text-stone-900 bg-white leading-normal select-none relative ${
        isPrintPreview
          ? `p-8 shadow-xl border border-stone-300 rounded mx-auto ${
              isLandscape ? 'max-w-[1020px] min-h-[720px]' : 'max-w-[800px] min-h-[1050px]'
            }`
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
            <div className="text-xs text-stone-600">Licence: {storeLicence} • MIS Analytical Reports</div>
          </div>
          <div className="text-right">
            <span className="inline-block px-2.5 py-1 bg-stone-900 text-white font-bold text-xs uppercase tracking-wider rounded-xs">
              {title.toUpperCase()}
            </span>
            <div className="text-xs font-semibold text-stone-800 mt-1">
              Date: {new Date().toLocaleDateString('en-IN', { day: '2-digit', month: 'short', year: 'numeric' })}
            </div>
            <div className="text-[10px] text-stone-500">Template: {modelId}.rpt ({rows.length} Records)</div>
          </div>
        </div>

        {subtitle && (
          <div className="text-xs text-stone-600 italic border-l-2 border-stone-400 pl-2">
            {subtitle}
          </div>
        )}

        {/* Dynamic Table */}
        <table className="w-full border-collapse border border-stone-300 text-xs">
          <thead>
            <tr className="bg-stone-100 text-stone-800 border-b border-stone-300 font-bold">
              <th className="border-r border-stone-300 px-2 py-1.5 text-center w-8">#</th>
              {columns.map(col => (
                <th
                  key={col.key}
                  className={`border-r border-stone-300 px-3 py-1.5 ${
                    col.kind && col.kind !== 'text' ? 'text-right' : 'text-left'
                  }`}
                >
                  {col.label}
                </th>
              ))}
            </tr>
          </thead>
          <tbody className="divide-y divide-stone-200">
            {rows.map((row, idx) => (
              <tr key={idx} className="hover:bg-stone-50">
                <td className="border-r border-stone-300 px-2 py-1.5 text-center text-stone-500">{idx + 1}</td>
                {columns.map(col => {
                  const isNum = col.kind && col.kind !== 'text';
                  return (
                    <td
                      key={col.key}
                      className={`border-r border-stone-300 px-3 py-1.5 ${
                        isNum ? 'text-right tabular-nums' : 'text-left font-medium'
                      }`}
                    >
                      {formatVal(col, row[col.key])}
                    </td>
                  );
                })}
              </tr>
            ))}
          </tbody>
          {totals.some(t => t !== null) && (
            <tfoot className="border-t-2 border-stone-400 bg-stone-100 font-bold text-stone-900">
              <tr>
                <td className="border-r border-stone-300 px-2 py-1.5 text-center uppercase">Σ</td>
                {columns.map((col, i) => (
                  <td
                    key={col.key}
                    className={`border-r border-stone-300 px-3 py-1.5 ${
                      col.kind && col.kind !== 'text' ? 'text-right tabular-nums' : 'text-left'
                    }`}
                  >
                    {i === 0 && !col.total
                      ? 'Total'
                      : totals[i] === null
                      ? ''
                      : col.kind === 'money'
                      ? `₹ ${money(totals[i]!)}`
                      : totals[i]!.toLocaleString('en-IN')}
                  </td>
                ))}
              </tr>
            </tfoot>
          )}
        </table>

        {/* Footer */}
        <div className="flex justify-between items-center pt-6 text-[10px] text-stone-500 border-t border-stone-200">
          <div>Generated by Softvent vLQMS • Confidential Business Document</div>
          <div>Page 1 of 1</div>
        </div>
      </div>
    </div>
  );
};
