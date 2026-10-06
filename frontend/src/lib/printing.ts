// Printing a saved document through its original Crystal .rpt format (backend: app/rpt_print.py, app/rpt_bridge.py).
//
// The desktop looks a document's print options up in the Doc Designer's UtlOption table: each option names a .rpt file,
// and one is the default; a plain click, Ctrl+click and Ctrl+Alt+click can each preview, print directly, e-mail or
// multi-print; and "after save" says whether a save prints (asking first, or at once). The web app reads the same
// settings from GET /print/options and asks the server to run the report, which comes back as a PDF laid out by
// Crystal Reports itself. A document the designer has no option for uses the desktop's file mode: pick one of the
// FMTSINV*.rpt files (the choice is remembered on this PC, as the desktop's INI does).
import { emailPrint, getPrintOptions, renderPrint, setPrintPreference } from './api';

export type PrintKind = 'sales_invoice' | 'purchase_bill' | 'purchase_indent' | 'sales_return' | 'stock_deduction' | 'expense';
export type PrintAction = 'preview' | 'direct' | 'email' | 'multi' | '';
export type PrintOption = {
  name: string; rpt: string; copies: number; default: boolean; click: PrintAction; ctrl_click: PrintAction;
  ctrl_alt_click: PrintAction; after_save: string; printer: string; multi_rpt: boolean;
  available: boolean;   // the option's .rpt file is in the report folder
  preferred: boolean;   // this user's own last pick
};
export type PrintOptions = {
  kind: PrintKind; short_form: string; doc_id: number | null; doc_name: string; designed: boolean;
  options: PrintOption[]; default: string | null; preferred: string | null; files: string[]; stored_proc: boolean; bridge_configured: boolean;
};
export type PrintRequest = { kind: PrintKind; doc_no: string; option?: string; click?: 'N' | 'C' | 'A'; rpt?: string; action?: 'preview' | 'direct'; printer?: string };

export { emailPrint, setPrintPreference };

/** Put a PDF response in `tab` (opened inside the click). Failures throw and close the tab. */
export async function showPdf(res: Response, tab: Window | null): Promise<void> {
  const url = URL.createObjectURL(await res.blob());
  const w = tab ?? window.open('', '_blank');
  if (!w) { URL.revokeObjectURL(url); throw new Error('The browser blocked the print tab. Allow pop-ups for this site, then press Print again.'); }
  w.location.href = url;
  setTimeout(() => URL.revokeObjectURL(url), 60_000);
}
export const fetchPrintOptions = getPrintOptions;

const FILE_KEY = 'vlqms_print_format';   // the desktop's INI "Print Format / PrintName"
export const savedFormat = (kind: PrintKind) => { try { return localStorage.getItem(`${FILE_KEY}_${kind}`) || ''; } catch { return ''; } };
export const saveFormat = (kind: PrintKind, name: string) => { try { localStorage.setItem(`${FILE_KEY}_${kind}`, name); } catch { /* blocked */ } };

/** The option a save prints with, or null when the designer has none (file mode). */
export const defaultOption = (o: PrintOptions): PrintOption | null =>
  o.options.find(x => x.name === o.default) ?? null;

/** Open a blank tab now (inside the click, so the browser allows it); the PDF is put in it when it arrives. */
export const openPreviewTab = (): Window | null => {
  const w = window.open('', '_blank');
  if (w) w.document.title = 'Preparing print…';
  return w;
};

/**
 * Run a print: a preview opens the PDF in `tab` (or a new one), a direct print goes to the option's printer.
 * Returns what happened, for a snackbar. Failures throw ApiError (the tab is closed).
 */
export async function runPrint(req: PrintRequest, tab?: Window | null): Promise<{ action: 'preview' | 'direct'; text: string }> {
  try {
    const res = await renderPrint(req);
    if ((res.headers.get('Content-Type') ?? '').includes('application/pdf')) {
      const url = URL.createObjectURL(await res.blob());
      const w = tab ?? window.open('', '_blank');
      // Never navigate this tab away: it may hold a bill in progress.
      if (!w) { URL.revokeObjectURL(url); throw new Error('The browser blocked the print tab. Allow pop-ups for this site, then press Print again.'); }
      w.location.href = url;
      setTimeout(() => URL.revokeObjectURL(url), 60_000);
      return { action: 'preview', text: `${req.doc_no} opened for printing.` };
    }
    const r = await res.json() as { printer: string; copies: number };
    tab?.close();
    return { action: 'direct', text: `${req.doc_no} sent to ${r.printer || 'the printer'} (${r.copies} ${r.copies === 1 ? 'copy' : 'copies'}).` };
  } catch (e) {
    tab?.close();
    throw e;
  }
}

export type AfterSave = 'none' | 'ask' | 'dialog' | 'direct';

/**
 * What a save does about printing. A document with a designed default option follows that option's AfterSaveAction
 * (P = ask first, PP = print without asking, empty = don't print); one without follows Global Settings
 * (`fallback`: none / prompt / preview). `andPrint` is the Save & Print key. A preview needs a browser tab, which can only be
 * opened from a key press or click, so after a save it is the Print dialog (Enter opens the tab); a direct print
 * to a printer needs no tab and goes straight out ('direct').
 */
export async function afterSavePlan(kind: PrintKind, fallback: 'none' | 'prompt' | 'preview', andPrint: boolean): Promise<{ plan: AfterSave; option?: PrintOption }> {
  let o: PrintOptions | null = null;
  try { o = await fetchPrintOptions(kind); } catch { /* offline: fall back to the settings */ }
  const d = o?.designed ? defaultOption(o) : null;
  const direct = (x: PrintOption) => (x.click === 'direct' && x.printer ? { plan: 'direct' as const, option: x } : null);
  if (d) {
    if (andPrint) return direct(d) ?? { plan: 'dialog' };
    if (d.after_save === 'PP') return direct(d) ?? { plan: 'dialog' };
    if (d.after_save === 'P') return { plan: 'ask' };
    return { plan: 'none' };
  }
  if (andPrint || fallback === 'preview') return { plan: 'dialog' };
  return { plan: fallback === 'prompt' ? 'ask' : 'none' };
}
