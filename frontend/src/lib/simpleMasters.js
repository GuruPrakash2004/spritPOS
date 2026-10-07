





 

const active = { key: 'active', label: 'Active', type: 'bool', default: true };

export const SIMPLE_MASTERS = [
  { path: 'supplier-groups', title: 'Supplier Group Master', subtitle: 'Groups for suppliers.', keywords: 'supplier group vendor category',
    fields: [{ key: 'code', label: 'Group Code', required: true }, { key: 'name', label: 'Group Name', required: true }, active] },
  { path: 'customer-groups', title: 'Customer Group Master', subtitle: 'Groups for customers.', keywords: 'customer group party category',
    fields: [{ key: 'code', label: 'Group Code', required: true }, { key: 'name', label: 'Group Name', required: true }, active] },
  { path: 'states', title: 'State Master', subtitle: 'States used on customer and supplier addresses.', keywords: 'state gst code',
    fields: [{ key: 'code', label: 'State Code', required: true }, { key: 'name', label: 'State Name', required: true }, active] },
  { path: 'currencies', title: 'Currency Master', subtitle: 'Currencies and how amounts are written in words.', keywords: 'currency rupees decimals',
    fields: [{ key: 'code', label: 'Currency Code', required: true }, { key: 'name', label: 'Currency Name', required: true },
      { key: 'decimals', label: 'Decimals' }, { key: 'words_in_millions', label: 'Words in millions' }, active] },
  { path: 'locations', title: 'Location Master', subtitle: 'Stock locations.', keywords: 'location godown store warehouse',
    fields: [{ key: 'code', label: 'Location Code', required: true }, { key: 'name', label: 'Location Name', required: true }, active] },
  { path: 'taxes', title: 'Tax Master', subtitle: 'Tax codes with CGST, SGST and IGST percentages.', keywords: 'tax gst cgst sgst igst',
    fields: [{ key: 'code', label: 'Tax Code', required: true }, { key: 'name', label: 'Tax Name', required: true },
      { key: 'tax_type', label: 'Tax Type', type: 'select', options: ['LOCAL', 'INTERSTATE'], showInList: false },
      { key: 'cgst_per', label: 'CGST %', type: 'number' }, { key: 'sgst_per', label: 'SGST %', type: 'number' },
      { key: 'igst_per', label: 'IGST %', type: 'number' }, active] },
  { path: 'banks', title: 'Bank Master', subtitle: 'Bank accounts.', keywords: 'bank account ifsc branch',
    fields: [{ key: 'code', label: 'Bank Code', required: true }, { key: 'name', label: 'Bank Name', required: true },
      { key: 'display', label: 'Display name', showInList: false }, { key: 'account_name', label: 'Account name', showInList: false },
      { key: 'account_type', label: 'Account type', showInList: false }, { key: 'account_no', label: 'Account no.', showInList: false },
      { key: 'branch', label: 'Branch', showInList: false }, { key: 'pan', label: 'PAN', showInList: false },
      { key: 'micr', label: 'MICR code', showInList: false }, { key: 'ifsc', label: 'IFSC code', showInList: false }, active] },
  { path: 'holidays', title: 'Holiday Master', subtitle: 'Shop holidays (the date is the code).', keywords: 'holiday festival closed date',
    fields: [{ key: 'code', label: 'Date', type: 'date', required: true }, { key: 'name', label: 'Holiday Name', required: true }] },
];

export const SIMPLE_TITLES = SIMPLE_MASTERS.map(m => m.title);
