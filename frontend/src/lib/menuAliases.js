// The caption a web screen has in the desktop's menu (MstMenu.MenuItemCaption) when it differs from the web's own name. The sidebar
// follows each client's menu: a screen whose menu row says MenuShow = N is left out, and one the menu doesn't mention stays listed.
// Checked against the 14 vLQMS client databases (the captions are the same in all of them; which are shown differs per client).
export const MENU_CAPTIONS = {
  'Price List Maintenance': 'Item Price Maintenance Master',
  'Stock Ledger': 'Stock Ledger Report',
  'Stock Ageing': 'Stock Ageing Report',
  'Profit Margin': 'Profit Margin Report',
  'Brand-wise Sales': 'Brand Wise Report',
  'Stock Level / Reorder': 'Stock Level Report',
  'Package-wise Sales': 'Package Wise Sales Report',
  'Package-wise Purchase': 'Package Wise Purchase Report',
  'Drops Sales Summary': 'Drops Reports',
  'Drops Purchase': 'Drops Reports',
  'Drops Petty Cash': 'Drops Reports',
  'Drops Consolidated': 'Drops Reports',
  'Reports Dashboard': 'Customised Reports...',
  'Global Settings': 'Global Settings...',
};

/** The module names (lower case) to leave out of the sidebar, from the menu rows [{caption, show}]. A caption several rows share is
 *  shown if any of them is. */
export function hiddenModules(menu) {
  const shown = new Map();   // caption (lower case) -> shown by the menu?
  for (const m of menu) if (m.caption) { const k = m.caption.trim().toLowerCase(); shown.set(k, shown.get(k) || !!m.show); }
  const hidden = new Set();
  for (const [k, ok] of shown) if (!ok) hidden.add(k);   // a screen with the same name as its caption
  for (const [module, caption] of Object.entries(MENU_CAPTIONS)) {
    const ok = shown.get(caption.toLowerCase());
    if (ok === false) hidden.add(module.toLowerCase()); else if (ok === true) hidden.delete(module.toLowerCase());
  }
  return hidden;
}
