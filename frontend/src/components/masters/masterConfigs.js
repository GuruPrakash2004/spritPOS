




// Field lists mirror backend/app/schemas.py. Kept at module level so the arrays are stable across renders.
export const ITEM_GROUP_FIELDS = [
  { key: 'code', label: 'Item Group Code', required: true },
  { key: 'name', label: 'Item Group Name', required: true },
  { key: 'liquor', label: 'Liquor Item Group', type: 'bool' },
  { key: 'consider_for_loyalty', label: 'Consider for Loyalty', type: 'bool' },
];

export const UOM_FIELDS = [
  { key: 'code', label: 'UOM Code', required: true },
  { key: 'name', label: 'UOM Name', required: true },
];

export const PACKAGE_FIELDS = [
  { key: 'code', label: 'Package Code', required: true },
  { key: 'litres_per_bottle', label: 'Litres per Bottle (used in litre reports)', type: 'number', required: true },
];

export const BRAND_FIELDS = [
  { key: 'code', label: 'Brand Code', required: true },
  { key: 'name', label: 'Brand Name', required: true },
];

export const CUSTOMER_FIELDS = [
  { key: 'code', label: 'Customer Code', required: true },
  { key: 'name', label: 'Customer Name', required: true },
  { key: 'mobile', label: 'Mobile No' },
  { key: 'loyalty_card_no', label: 'Loyalty Card Number' },
  { key: 'points', label: 'Loyalty Points', type: 'number' },
  { key: 'send_sms', label: 'Send SMS to customer', type: 'bool', showInList: false },
];

export const LEDGER_GROUP_FIELDS = [
  { key: 'code', label: 'Group Code', required: true },
  { key: 'name', label: 'Group Name', required: true },
];

// Mirrors backend schemas.SalesPerson (the desktop's MstSP).
export const SALES_PERSON_FIELDS = [
  { key: 'code', label: 'Sales Person Code', required: true },
  { key: 'name', label: 'Sales Person Name', required: true },
  { key: 'mobile', label: 'Mobile No' },
  { key: 'active', label: 'Active', type: 'bool', default: true },
];

export const LEDGER_FIELDS = [
  { key: 'code', label: 'Ledger Code', required: true },
  { key: 'name', label: 'Ledger Name', required: true },
  { key: 'group', label: 'Ledger Group', optionsFrom: 'ledger-groups', required: true },
];

export const ITEM_FIELDS = [
  { key: 'code', label: 'Item Code', required: true },
  { key: 'name', label: 'Item Name', required: true },
  { key: 'short_name', label: 'Short Name', showInList: false },
  { key: 'hsn_code', label: 'HSN Code', showInList: false },
  { key: 'group', label: 'Item Group', optionsFrom: 'item-groups', required: true },
  { key: 'brand', label: 'Brand', optionsFrom: 'brands', showInList: false },
  { key: 'liqtype', label: 'LIQTYPE', type: 'select', options: ['IML', 'Beer', 'Wine', 'Others'], required: true, showInList: false, default: 'IML' },
  { key: 'uom', label: 'UOM', optionsFrom: 'uoms', showInList: false, default: 'BTL' },
  { key: 'package', label: 'Package', optionsFrom: 'packages', required: true, showInList: false },
  { key: 'bottles_per_case', label: 'Bottles per Case (≥ 1)', type: 'number', required: true, showInList: false, default: 12 },
  { key: 'mrp', label: 'MRP', type: 'number', required: true },
  { key: 'purchase_rate', label: 'Purchase Rate', type: 'number', required: true, costField: true },
  { key: 'cgst_per', label: 'CGST % (non-liquor only)', type: 'number', showInList: false },
  { key: 'sgst_per', label: 'SGST % (non-liquor only)', type: 'number', showInList: false },
  { key: 'rol', label: 'ROL (reorder level)', type: 'number', showInList: false },
  { key: 'roq', label: 'ROQ (reorder qty)', type: 'number', showInList: false },
  // Opening stock: used only when the item is created. After that, stock changes through transactions
  // (the backend ignores QOH on edits so the stock ledger stays complete).
  { key: 'qoh', label: 'Opening stock (new items only)', type: 'number', showInList: false, newOnly: true },
  { key: 'active', label: 'Active', type: 'bool', showInList: false, default: true },
];

// Column order of the legacy Docs/ItemImport_Template.xlsx.xlsx, mapped to the API field names.
export const ITEM_IMPORT_COLUMNS = [
  ['ITEMCODE', 'code'], ['ITEMNAME', 'name'], ['SHORTNAME', 'short_name'], ['HSNCODE', 'hsn_code'],
  ['ITEMGROUPCode', 'group'], ['BRAND', 'brand'], ['LIQTYPE', 'liqtype'], ['UOMCode', 'uom'], ['PKGCode', 'package'],
  ['BOTPERCASES', 'bottles_per_case'], ['MRP', 'mrp'], ['PURRATE', 'purchase_rate'], ['CGSTPER', 'cgst_per'],
  ['SGSTPER', 'sgst_per'], ['ROL', 'rol'], ['ROQ', 'roq'],
];

export const ITEM_IMPORT_NUMERIC = new Set(['bottles_per_case', 'mrp', 'purchase_rate', 'cgst_per', 'sgst_per', 'rol', 'roq']);
