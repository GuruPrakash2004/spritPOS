// Registry of the legacy Crystal Reports (.rpt) print formats.
//
// Maps each document type (sales invoice, purchase bill, DSR, stock balance, …) to the .rpt models
// the desktop app offered and the printer each targets (thermal 80/58 mm, A4/A5 laser, dot-matrix).
// The chosen model per document type is remembered in localStorage.

export type PrinterStructure =
  | 'THERMAL_80MM'      // 80mm (3-inch) POS continuous thermal slip
  | 'THERMAL_58MM'      // 58mm (2-inch) POS continuous mini-thermal
  | 'LASER_A4_PORTRAIT' // Standard 210x297mm A4 portrait
  | 'LASER_A4_LANDSCAPE'// A4 landscape for wide registers
  | 'LASER_A5_LANDSCAPE'// Half-sheet 148x210mm A5 landscape
  | 'DOT_MATRIX_80'     // 80-Column tractor-feed continuous continuous sheet
  | 'DOT_MATRIX_132';   // 132-Column wide continuous ledger

export type DocumentCategory =
  | 'SALES_INVOICE'
  | 'PURCHASE_BILL'
  | 'DSR'
  | 'STOCK_BALANCE'
  | 'PURCHASE_INDENT'
  | 'STOCK_DEDUCTION'
  | 'SALES_RETURN'
  | 'EXPENSE'
  | 'GENERIC_REPORT';

export interface RptModelDef {
  id: string;                      // e.g. "fmtSPInvPrint"
  rptFileName: string;             // e.g. "fmtSPInvPrint.rpt"
  name: string;                    // e.g. "Thermal POS Receipt (80mm)"
  category: DocumentCategory;
  printer: PrinterStructure;
  paperName: string;               // e.g. "80mm Roll", "A4 Sheet", "Continuous 80-Col"
  description: string;
  badge: string;                   // e.g. "Fast POS", "GST Official", "Audit"
  isDefault?: boolean;
  supportsWatermark?: boolean;
  supportsCopies?: boolean;
}

export const RPT_MODELS: Record<string, RptModelDef> = {
  // ─── SALES INVOICE MODELS ───
  'fmtSPInvPrint': {
    id: 'fmtSPInvPrint',
    rptFileName: 'fmtSPInvPrint.rpt',
    name: 'POS 80mm Thermal Receipt (Standard)',
    category: 'SALES_INVOICE',
    printer: 'THERMAL_80MM',
    paperName: '80mm Roll (3-Inch)',
    description: 'High-speed compact POS slip with store header, barcode, item breakdown, and cash/card totals.',
    badge: 'Counter Default',
    isDefault: true,
    supportsWatermark: false,
    supportsCopies: true,
  },
  'fmtSPInvPrint_GST': {
    id: 'fmtSPInvPrint_GST',
    rptFileName: 'fmtSPInvPrint_GST.rpt',
    name: 'POS 80mm Thermal (with GST Recap)',
    category: 'SALES_INVOICE',
    printer: 'THERMAL_80MM',
    paperName: '80mm Roll (3-Inch)',
    description: 'POS receipt including compact GST HSN slab summary and store GSTIN.',
    badge: 'POS GST',
    supportsWatermark: false,
    supportsCopies: true,
  },
  'fmtSInv_GST': {
    id: 'fmtSInv_GST',
    rptFileName: 'fmtSInv_GST.rpt',
    name: 'A4 GST Detailed Tax Invoice',
    category: 'SALES_INVOICE',
    printer: 'LASER_A4_PORTRAIT',
    paperName: 'A4 Portrait',
    description: 'Full statutory GST tax invoice with supplier/buyer details, HSN codes, CGST/SGST matrix, and seal.',
    badge: 'Laser / Inkjet A4',
    supportsWatermark: true,
    supportsCopies: true,
  },
  'fmtSInv': {
    id: 'fmtSInv',
    rptFileName: 'fmtSInv.rpt',
    name: 'A5 Half-Page Retail Invoice Slip',
    category: 'SALES_INVOICE',
    printer: 'LASER_A5_LANDSCAPE',
    paperName: 'A5 Half-Sheet',
    description: 'Crisp half-page bill layout saving 50% paper with item lines, discounts and excise details.',
    badge: 'Paper Saver A5',
    supportsWatermark: true,
    supportsCopies: true,
  },
  'fmtSPInvPrint_watermark': {
    id: 'fmtSPInvPrint_watermark',
    rptFileName: 'fmtSPInvPrint_watermark.rpt',
    name: 'A4 / Slip Watermarked Duplicate Copy',
    category: 'SALES_INVOICE',
    printer: 'LASER_A4_PORTRAIT',
    paperName: 'A4 Sheet',
    description: 'Formal copy with prominent DUPLICATE / AUDIT / CANCELLED security watermark.',
    badge: 'Duplicate Copy',
    supportsWatermark: true,
    supportsCopies: true,
  },
  'fmtSInv_DMP': {
    id: 'fmtSInv_DMP',
    rptFileName: 'fmtSInv.rpt (DMP)',
    name: 'Dot Matrix 80-Column Continuous Slip',
    category: 'SALES_INVOICE',
    printer: 'DOT_MATRIX_80',
    paperName: '8.5" Continuous Fanfold',
    description: 'High-speed monospaced impact printing for dot-matrix printers with carbon-copy stationery.',
    badge: 'Dot Matrix DMP',
    supportsWatermark: false,
    supportsCopies: true,
  },

  // ─── DAILY SUMMARY REPORT (DSR) ───
  'RptDSR': {
    id: 'RptDSR',
    rptFileName: 'RptDSR.rpt',
    name: 'A4 Daily Summary Report (Executive Audit)',
    category: 'DSR',
    printer: 'LASER_A4_PORTRAIT',
    paperName: 'A4 Sheet',
    description: 'Complete day-end excise statement: Opening/Closing stock, sales, returns, expenses, profit, and audit KPIs.',
    badge: 'Audit Sheet',
    isDefault: true,
    supportsWatermark: false,
    supportsCopies: true,
  },
  'RptDSR_S': {
    id: 'RptDSR_S',
    rptFileName: 'RptDSR_S.rpt',
    name: 'POS 80mm DSR Summary Slip',
    category: 'DSR',
    printer: 'THERMAL_80MM',
    paperName: '80mm Roll',
    description: 'Pocket thermal roll printout of daily sales, total collection, and drawer balance for cashier closing.',
    badge: 'Cashier Slip',
    supportsWatermark: false,
    supportsCopies: true,
  },
  'RptDSR_8.5': {
    id: 'RptDSR_8.5',
    rptFileName: 'RptDSR_8.5.rpt',
    name: 'Dot-Matrix 8.5" Continuous DSR Ledger',
    category: 'DSR',
    printer: 'DOT_MATRIX_80',
    paperName: '8.5" Continuous Roll',
    description: 'Monospaced day-end register for dot-matrix audit logging.',
    badge: 'DMP 80-Col',
    supportsWatermark: false,
    supportsCopies: true,
  },
  'RptDropsDSR': {
    id: 'RptDropsDSR',
    rptFileName: 'RptDropsDSR.rpt',
    name: 'Cash Drops & Denomination Slip',
    category: 'DSR',
    printer: 'THERMAL_80MM',
    paperName: '80mm Roll',
    description: 'Cash drops summary with note denomination tally and safe deposits.',
    badge: 'Safe Audit',
    supportsWatermark: false,
    supportsCopies: true,
  },

  // ─── STOCK BALANCE REPORT ───
  'RptStkBal': {
    id: 'RptStkBal',
    rptFileName: 'RptStkBal.rpt',
    name: 'Stock Balance Matrix (Standard A4)',
    category: 'STOCK_BALANCE',
    printer: 'LASER_A4_LANDSCAPE',
    paperName: 'A4 Landscape',
    description: 'Opening, Inward Receipts, Issues, Closing Quantity and Value by Item/Brand.',
    badge: 'Standard Matrix',
    isDefault: true,
    supportsWatermark: false,
    supportsCopies: true,
  },
  'RptStkBal_Without_CLvalue': {
    id: 'RptStkBal_Without_CLvalue',
    rptFileName: 'RptStkBal_Without CLvalue.rpt',
    name: 'Physical Stock Count Sheet (Quantities Only)',
    category: 'STOCK_BALANCE',
    printer: 'LASER_A4_PORTRAIT',
    paperName: 'A4 Portrait',
    description: 'Physical audit count sheet with blank verification column, omitting valuations to prevent stocktaking bias.',
    badge: 'Physical Audit',
    supportsWatermark: false,
    supportsCopies: true,
  },
  'RptStkBal_with_Mrp_inv_rate': {
    id: 'RptStkBal_with_Mrp_inv_rate',
    rptFileName: 'RptStkBal_with Mrp and inv rate.rpt',
    name: 'Dual Valuation (Cost vs Realizable MRP)',
    category: 'STOCK_BALANCE',
    printer: 'LASER_A4_LANDSCAPE',
    paperName: 'A4 Landscape',
    description: 'Comparative valuation showing Purchase Cost vs Retail MRP with unrealized margin totals.',
    badge: 'Margin Valuation',
    supportsWatermark: false,
    supportsCopies: true,
  },
  'RptStkBal_Biggsize': {
    id: 'RptStkBal_Biggsize',
    rptFileName: 'RptStkBal_Biggsize.rpt',
    name: 'Wide Stock Ledger (132-Column Continuous)',
    category: 'STOCK_BALANCE',
    printer: 'DOT_MATRIX_132',
    paperName: '15" Wide Continuous Stationery',
    description: 'Extended columns including Cases/Bottles, Batch No, Breakage, In-transit, and Volume in Litres.',
    badge: 'Wide DMP 132',
    supportsWatermark: false,
    supportsCopies: true,
  },

  // ─── PURCHASE BILL ───
  'fmtPB_A4': {
    id: 'fmtPB_A4',
    rptFileName: 'fmtPB_A4.rpt',
    name: 'A4 Purchase Inward Goods Note',
    category: 'PURCHASE_BILL',
    printer: 'LASER_A4_PORTRAIT',
    paperName: 'A4 Portrait',
    description: 'Comprehensive KSBCL Inward Bill with pass/permit verification, excise check, and ledger accounts.',
    badge: 'KSBCL Inward A4',
    isDefault: true,
    supportsWatermark: false,
    supportsCopies: true,
  },
  'fmtPB': {
    id: 'fmtPB',
    rptFileName: 'fmtPB.rpt',
    name: 'Purchase Bill Voucher Slip (A5 / 80mm)',
    category: 'PURCHASE_BILL',
    printer: 'LASER_A5_LANDSCAPE',
    paperName: 'A5 Half-Sheet',
    description: 'Compact warehouse stock receipt slip with supplier name, case/bottle counts, and net inward cost.',
    badge: 'Compact Inward',
    supportsWatermark: false,
    supportsCopies: true,
  },

  // ─── PURCHASE INDENT ───
  'fmtPI': {
    id: 'fmtPI',
    rptFileName: 'fmtPI.rpt',
    name: 'KSBCL Purchase Indent Order Sheet',
    category: 'PURCHASE_INDENT',
    printer: 'LASER_A4_PORTRAIT',
    paperName: 'A4 Portrait',
    description: 'Official KSBCL depot requisition order formatted for depot submission with category-wise bottle cases.',
    badge: 'Depot Indent',
    isDefault: true,
    supportsWatermark: false,
    supportsCopies: true,
  },

  // ─── STOCK DEDUCTION (Breakage / Sampling) ───
  'fmtSTKDED_A4': {
    id: 'fmtSTKDED_A4',
    rptFileName: 'fmtSTKDED_A4.rpt',
    name: 'Stock Deduction Voucher (A4 Formal)',
    category: 'STOCK_DEDUCTION',
    printer: 'LASER_A4_PORTRAIT',
    paperName: 'A4 Portrait',
    description: 'Breakage, leakage, expired stock or sampling voucher with authorized signatory lines.',
    badge: 'Excise Deduction',
    isDefault: true,
    supportsWatermark: true,
    supportsCopies: true,
  },
  'fmtSTKDED': {
    id: 'fmtSTKDED',
    rptFileName: 'fmtSTKDED.rpt',
    name: 'POS 80mm Stock Deduction Slip',
    category: 'STOCK_DEDUCTION',
    printer: 'THERMAL_80MM',
    paperName: '80mm Roll',
    description: 'Instant counter deduction slip for broken bottles or sampling.',
    badge: 'Counter Slip',
    supportsWatermark: false,
    supportsCopies: true,
  },

  // ─── SALES RETURN ───
  'fmtSR': {
    id: 'fmtSR',
    rptFileName: 'fmtSR.rpt',
    name: 'Sales Return & Refund Slip (80mm Thermal)',
    category: 'SALES_RETURN',
    printer: 'THERMAL_80MM',
    paperName: '80mm Roll',
    description: 'Customer bottle return note with original invoice reference and credit/refund amount.',
    badge: 'Credit Note',
    isDefault: true,
    supportsWatermark: false,
    supportsCopies: true,
  },

  // ─── EXPENSE ENTRY ───
  'fmtExp': {
    id: 'fmtExp',
    rptFileName: 'fmtExp.rpt',
    name: 'Petty Cash Payment Voucher (80mm Thermal)',
    category: 'EXPENSE',
    printer: 'THERMAL_80MM',
    paperName: '80mm Roll',
    description: 'Cash payment voucher with expense ledger name, receiver signature, and cashier sign-off.',
    badge: 'Petty Cash',
    isDefault: true,
    supportsWatermark: false,
    supportsCopies: true,
  },

  // ─── GENERIC ANALYTICAL REPORT ───
  'UDR_A4_Landscape': {
    id: 'UDR_A4_Landscape',
    rptFileName: 'UDRSalesItemwise.rpt',
    name: 'Analytical Register (A4 Landscape)',
    category: 'GENERIC_REPORT',
    printer: 'LASER_A4_LANDSCAPE',
    paperName: 'A4 Landscape',
    description: 'Wide tabular register for multi-column analytical reports with running subtotals.',
    badge: 'Landscape A4',
    isDefault: true,
    supportsWatermark: false,
    supportsCopies: true,
  },
  'UDR_A4_Portrait': {
    id: 'UDR_A4_Portrait',
    rptFileName: 'RptItemSalesDay.rpt',
    name: 'Standard Register (A4 Portrait)',
    category: 'GENERIC_REPORT',
    printer: 'LASER_A4_PORTRAIT',
    paperName: 'A4 Portrait',
    description: 'Standard report listing with clean headers, date filters, and column totals.',
    badge: 'Portrait A4',
    supportsWatermark: false,
    supportsCopies: true,
  },
};

/** Get all models applicable to a specific document category */
export function getModelsForCategory(category: DocumentCategory): RptModelDef[] {
  return Object.values(RPT_MODELS).filter(m => m.category === category);
}

/** Get or set default model for category in localStorage */
const PREF_KEY_PREFIX = 'vlqms_print_pref_';

export function getSavedModelPreference(category: DocumentCategory): string {
  const saved = localStorage.getItem(`${PREF_KEY_PREFIX}${category}`);
  if (saved && RPT_MODELS[saved]?.category === category) return saved;
  const def = Object.values(RPT_MODELS).find(m => m.category === category && m.isDefault);
  return def ? def.id : Object.values(RPT_MODELS).find(m => m.category === category)?.id ?? 'fmtSPInvPrint';
}

export function saveModelPreference(category: DocumentCategory, modelId: string): void {
  localStorage.setItem(`${PREF_KEY_PREFIX}${category}`, modelId);
}
