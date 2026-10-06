// Print window for a saved document, driven by the Doc Designer's print options (lib/printing.ts).
//
// - Lists the document's print options (UtlOption) with the user's own last pick, else the designer's default, selected;
//   with no option designed it lists the FMTSINV*.rpt files, as the desktop's invoice screen does (the pick is remembered).
// - Click a format and it is shown on the right: Crystal Reports lays the report out through the server and the PDF is
//   shown as it prints (the report's own page size). Nothing is redrawn here, so what prints is the .rpt itself.
// - Print prints what is shown (the browser's print dialog); Print directly sends the report to the chosen printer
//   (the option's own printer unless another is picked) without showing it; Email sends it as a PDF.
// Keyboard: the Print button has focus when it opens (Enter prints); ↑/↓ choose the format; Ctrl/⌘+Enter prints
// directly; Esc closes (useDialogKeys).
import { useEffect, useRef, useState } from 'react';
import { Mail, Printer, Send } from 'lucide-react';
import Modal from '../common/Modal';
import { toast, toastStore } from '../common/toastStore';
import { errorText, getPrintBridge, renderPrint } from '../../lib/api';
import { field, label as labelClass, primaryButton, secondaryButton } from '../../lib/styles';
import {
  emailPrint, fetchPrintOptions, setPrintPreference, saveFormat, savedFormat,
  type PrintKind, type PrintOption, type PrintOptions,
} from '../../lib/printing';
import { printFrame, sheetStyle, usePdfPreview } from './usePdfPreview';

type Choice = { key: string; label: string; option?: PrintOption; rpt?: string };

const PRINTER_KEY = 'vlqms_doc_printer';
const savedPrinter = () => { try { return localStorage.getItem(PRINTER_KEY) || ''; } catch { return ''; } };

export default function PrintDialog({ kind, docNo, title, onClose }: {
  kind: PrintKind; docNo: string; title?: string; onClose: () => void;
}) {
  const [info, setInfo] = useState<PrintOptions | null>(null);
  const [error, setError] = useState('');
  const [chosen, setChosen] = useState('');
  const [busy, setBusy] = useState(false);
  const [emailing, setEmailing] = useState(false);
  const [to, setTo] = useState('');
  const [printers, setPrinters] = useState<string[]>([]);
  const [printer, setPrinter] = useState('');
  const frame = useRef<HTMLIFrameElement>(null);
  const mainRef = useRef<HTMLButtonElement>(null);

  useEffect(() => {
    fetchPrintOptions(kind).then(o => {
      setInfo(o);
      const usable = o.options.filter(x => x.available);
      const pick = o.designed
        ? (usable.find(x => x.name === o.preferred)?.name ?? usable.find(x => x.name === o.default)?.name ?? usable[0]?.name ?? o.default ?? o.options[0]?.name ?? '')
        : (o.files.find(f => f === savedFormat(kind)) ?? o.files[0] ?? '');
      setChosen(pick);
    }).catch(e => setError(errorText(e)));
    getPrintBridge().then(b => setPrinters(b.printers ?? [])).catch(() => { /* the preview says why */ });
  }, [kind]);

  const choices: Choice[] = !info ? [] : info.designed
    ? info.options.map(o => ({ key: o.name, label: o.name, option: o }))
    : info.files.map(f => ({ key: f, label: f.replace(/\.rpt$/i, ''), rpt: f }));
  const current = choices.find(c => c.key === chosen);
  const target = (c: Choice | undefined) => (c?.option ? { option: c.option.name } : { rpt: c?.rpt });

  // The printer for Print directly: the option's own (Doc Designer), else the one picked last on this PC.
  useEffect(() => {
    setPrinter(current?.option?.printer || savedPrinter());
  }, [current?.option?.printer, current?.key]);

  const ready = !!current && !!info?.bridge_configured && current.option?.available !== false;
  const { url, loading, error: previewError, size } = usePdfPreview(
    ready ? `${docNo}|${chosen}` : '',
    () => renderPrint({ kind, doc_no: docNo, action: 'preview', ...target(current) }),
  );

  const remember = () => {
    if (!info?.designed) saveFormat(kind, current!.key);
    else if (current?.option && current.option.name !== info.preferred) setPrintPreference(kind, current.option.name).catch(() => { /* a remembered pick is a convenience */ });
  };

  const printShown = () => {
    if (!current || !url) return;
    remember();
    printFrame(frame, url);
  };

  const printDirect = async () => {
    if (!current || !printer || busy) return;
    if (!(await toastStore.defer('ok', `${docNo} will be sent to ${printer} when this closes.`, 'Print cancelled. Nothing was sent.'))) return;
    remember();
    setBusy(true);
    try {
      const res = await renderPrint({ kind, doc_no: docNo, action: 'direct', printer, ...target(current) });
      const r = await res.json() as { printer: string; copies: number };
      try { localStorage.setItem(PRINTER_KEY, printer); } catch { /* blocked */ }
      toast.ok(`${docNo} sent to ${r.printer || printer} (${r.copies} ${r.copies === 1 ? 'copy' : 'copies'}).`);
      onClose();
    } catch (e) {
      toast.error(errorText(e));
    } finally {
      setBusy(false);
    }
  };

  const sendEmail = async () => {
    if (!current) return;
    const addresses = to.split(/[;,\s]+/).filter(Boolean);
    if (!(await toastStore.defer('ok', `${docNo} will be e-mailed as a PDF when this closes.`, 'Email cancelled. Nothing was sent.'))) return;
    setBusy(true);
    try {
      const r = await emailPrint({ kind, doc_no: docNo, ...target(current) }, addresses);
      toast.ok(`Email sent to ${r.to.join(', ')}.`);
      onClose();
    } catch (e) {
      toast.error(errorText(e));
    } finally {
      setBusy(false);
    }
  };

  const onKeyDown = (e: React.KeyboardEvent) => {
    if (emailing || !choices.length) return;
    if (e.key === 'ArrowDown' || e.key === 'ArrowUp') {
      e.preventDefault();
      const usable = choices.filter(c => c.option?.available !== false);
      const i = usable.findIndex(c => c.key === chosen);
      if (usable.length) setChosen(usable[(i + (e.key === 'ArrowDown' ? 1 : usable.length - 1)) % usable.length].key);
    } else if (e.key === 'Enter' && (e.ctrlKey || e.metaKey)) {
      e.preventDefault();
      void printDirect();
    }
  };

  return (
    <Modal
      title={title ?? `Print ${info?.doc_name || ''} ${docNo}`.trim()}
      onClose={onClose}
      wide
      footer={emailing ? <>
        <button onClick={() => setEmailing(false)} className={secondaryButton}>Back</button>
        <button onClick={sendEmail} disabled={busy || !to.trim()} className={primaryButton}><Mail size={14} />Send email</button>
      </> : <>
        <button onClick={onClose} className={secondaryButton}>Close</button>
        <button onClick={() => setEmailing(true)} disabled={!current || busy} className={secondaryButton}><Mail size={14} />Email</button>
        <button onClick={printDirect} disabled={!current || !printer || busy} className={secondaryButton}
          title={printer ? `Prints on ${printer} without showing it (Ctrl+Enter)` : 'Choose a printer first'}><Send size={14} />Print directly</button>
        <button ref={mainRef} onClick={printShown} disabled={!url || loading || busy} className={primaryButton} data-autofocus><Printer size={14} />Print</button>
      </>}
    >
      <div onKeyDown={onKeyDown}>
        {error && <p role="alert" className="mb-2 text-red-600 dark:text-red-400">{error}</p>}
        {!info && !error && <p className="text-stone-500">Loading print formats…</p>}
        {info && !choices.length && (
          <p className="text-stone-600 dark:text-stone-300">
            No print format is set up for this document. Add one in the Doc Designer (Print options), or put its .rpt file in the Rpts folder.
          </p>
        )}
        {info && !info.bridge_configured && (
          <p role="alert" className="mb-3 rounded-lg border border-amber-500/40 bg-amber-50 p-2 text-amber-800 dark:bg-amber-950/30 dark:text-amber-300">
            The Crystal Reports bridge is not set up on this server, so reports cannot be laid out yet (see backend/tools/rptbridge/README.md).
          </p>
        )}
        {emailing ? (
          <div>
            <label htmlFor="pd-to" className={labelClass}>Send {docNo} to</label>
            <input id="pd-to" value={to} onChange={e => setTo(e.target.value)} placeholder="name@example.com" autoFocus className={field}
              onKeyDown={e => { if (e.key === 'Enter') { e.preventDefault(); void sendEmail(); } }} />
            <p className="mt-1.5 text-[11px] text-stone-500">Addresses fixed in the Doc Designer for this option are added. The report goes as a PDF.</p>
          </div>
        ) : choices.length > 0 && (
          <div className="flex flex-col gap-3 sm:flex-row">
            <div className="sm:w-56 sm:shrink-0">
              <fieldset>
                <legend className={labelClass}>{info?.designed ? 'Print format' : 'Print format (.rpt file)'}</legend>
                <div role="radiogroup" className="space-y-1">
                  {choices.map(c => (
                    <label key={c.key} className={`flex cursor-pointer flex-col gap-0.5 rounded-lg border px-3 py-2 ${c.key === chosen
                      ? 'border-[#64283A] bg-[#64283A]/5 dark:border-[#D4A847] dark:bg-[#D4A847]/10' : 'border-[#E3D9CD] dark:border-[#2A2A2A]'}`}>
                      <span className="flex min-w-0 items-center gap-2">
                        <input type="radio" name="print-format" checked={c.key === chosen} disabled={c.option?.available === false} onChange={() => setChosen(c.key)} />
                        <span className="truncate font-medium">{c.label}</span>
                      </span>
                      <span className="flex flex-wrap items-center gap-1 pl-5">
                        {c.option?.preferred && <span className="rounded bg-sky-100 px-1.5 py-0.5 text-[10px] font-semibold text-sky-800 dark:bg-sky-900/40 dark:text-sky-300">Your default</span>}
                        {c.option?.available === false && <span className="rounded bg-red-100 px-1.5 py-0.5 text-[10px] font-semibold text-red-800 dark:bg-red-900/40 dark:text-red-300">Not found</span>}
                        {c.option?.default && <span className="rounded bg-emerald-100 px-1.5 py-0.5 text-[10px] font-semibold text-emerald-800 dark:bg-emerald-900/40 dark:text-emerald-300">Default</span>}
                        {c.option && <span className="text-[11px] text-stone-500">{c.option.rpt}{c.option.copies > 1 ? ` × ${c.option.copies}` : ''}</span>}
                      </span>
                    </label>
                  ))}
                </div>
              </fieldset>
              <label className="mt-4 block">
                <span className={labelClass}>Printer (for Print directly)</span>
                <select value={printer} onChange={e => setPrinter(e.target.value)} className={field}>
                  <option value="">Choose a printer…</option>
                  {printer && !printers.includes(printer) && <option value={printer}>{printer}</option>}
                  {printers.map(p => <option key={p} value={p}>{p}</option>)}
                </select>
              </label>
            </div>
            <div className="relative flex min-h-[60vh] flex-1 items-start justify-center overflow-auto rounded-lg border border-[#E3D9CD] bg-stone-200 p-4 dark:border-[#2A2A2A] dark:bg-[#0c0c0c]">
              {loading && <p className="absolute inset-0 flex items-center justify-center text-stone-500">Laying out the report…</p>}
              {previewError && <p role="alert" className="p-3 text-red-600 dark:text-red-400">{previewError}</p>}
              {url && !loading && <iframe ref={frame} title="Print preview" src={`${url}#toolbar=0&navpanes=0&view=FitH`} style={sheetStyle(size)} className="bg-white shadow-lg ring-1 ring-black/10" />}
            </div>
          </div>
        )}
      </div>
    </Modal>
  );
}
