// Global Settings → Other Settings as the screens use them (backend app/doc_settings.py):
// - useDocSettings(kind): print after save (none / prompt / preview), default print format, Copy From, and
//   whether the document date can be changed, for one document kind;
// - menu layout (Menu Sequence): modules hidden from the sidebar and module search, and shortcut codes.
// Both are fetched once per page load (GET /settings/documents, GET /settings/menu) and default to the
// desktop's defaults until they arrive; the server enforces the same rules.
import { useEffect, useState } from 'react';
import { api } from './api';

export type DocKind = 'sales_invoice' | 'purchase_bill' | 'purchase_indent' | 'sales_return' | 'stock_deduction' | 'expense' | 'closing_stock';
export type DocSettings = {
  after_save: 'none' | 'prompt' | 'preview'; default_format: string; allow_duplicate_items: boolean; allow_edit_date: boolean;
  edit_days: number; allow_copy_from: boolean; update_rate_from_pb: boolean; screen: string; prints: boolean; affects_stock: boolean;
};
export type MenuLayout = { hidden: string[]; codes: Record<string, string> };

export const DEFAULT_DOC_SETTINGS: DocSettings = {
  after_save: 'none', default_format: '', allow_duplicate_items: true, allow_edit_date: true, edit_days: 0,
  allow_copy_from: true, update_rate_from_pb: false, screen: '', prints: false, affects_stock: false,
};

let docs: Promise<Record<string, DocSettings>> | null = null;
export const loadDocSettings = () => {
  docs ??= api<Record<string, DocSettings>>('/settings/documents').catch(() => { docs = null; return {}; });
  return docs;
};
/** Forget the cached settings (after saving them in Global Settings). */
export const resetDocSettings = () => { docs = null; };

export function useDocSettings(kind: DocKind): DocSettings {
  const [s, setS] = useState<DocSettings>(DEFAULT_DOC_SETTINGS);
  useEffect(() => {
    let live = true;
    loadDocSettings().then(all => { if (live && all?.[kind]) setS({ ...DEFAULT_DOC_SETTINGS, ...all[kind] }); });
    return () => { live = false; };
  }, [kind]);
  return s;
}

export const loadMenuLayout = () => api<MenuLayout>('/settings/menu').catch(() => ({ hidden: [], codes: {} } as MenuLayout));
