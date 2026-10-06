// URL routes for the screens, so each one has its own address (/sales-invoice, /item-master, ...).
//
// A route is the module name made URL-safe: "Stock Level / Reorder" -> /stock-level-reorder. Dashboard is "/".
// Admin Master keeps its older address, /adminMaster. Because every screen is a real link, a biller can
// open any screen in another browser tab (middle-click or Ctrl/Cmd-click in the sidebar) and keep billing
// in the first one. The backend answers any page load with the app (main.py serve_frontend), so a
// refresh or a bookmarked address works.

import { ADMIN_MASTER, ADMIN_PATH, MODULE_GROUPS } from './modules';

const slug = (name: string) => name.toLowerCase().replace(/[^a-z0-9]+/g, '-').replace(/^-+|-+$/g, '');

// Names that open the same screen as another module (the sidebar shows "Customised Reports").
const ALIASES: Record<string, string> = { 'Customised Reports': 'Reports Dashboard' };

const BY_PATH = new Map<string, string>();
for (const g of MODULE_GROUPS) for (const m of g.modules) BY_PATH.set(`/${slug(m)}`, m);
BY_PATH.set(ADMIN_PATH.toLowerCase(), ADMIN_MASTER);
BY_PATH.set('/customised-reports', 'Reports Dashboard');

/** The address of a module's screen. */
export const pathForModule = (name: string): string => {
  if (name === 'Dashboard') return '/';
  if (name === ADMIN_MASTER) return ADMIN_PATH;
  return `/${slug(ALIASES[name] ?? name)}`;
};

/** The module a browser address opens ("Dashboard" for "/" and for an address no screen has). */
export const moduleForPath = (pathname: string): string => {
  const clean = pathname.replace(/\/+$/, '').toLowerCase();
  return BY_PATH.get(clean || '/') ?? 'Dashboard';
};

/** True when a click should open normally in this tab (a plain left click), not in a new one. */
export const isPlainClick = (e: { button: number; metaKey: boolean; ctrlKey: boolean; shiftKey: boolean; altKey: boolean }) =>
  e.button === 0 && !e.metaKey && !e.ctrlKey && !e.shiftKey && !e.altKey;
