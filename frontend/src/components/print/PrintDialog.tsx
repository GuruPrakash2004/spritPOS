// Print dialog for a saved document, driven by the Doc Designer's print options (lib/printing.ts).
//
// - Lists the document's print options (UtlOption) with the designer's default selected; with no option designed it lists
//   the FMTSINV*.rpt files, as the desktop's invoice screen does (the pick is remembered on this PC).
// - The report is run by Crystal Reports through the server and opens as a PDF in a new tab (Preview), goes to the
//   option's printer (Print), or is e-mailed as a PDF (Email). Nothing is redrawn here, so what prints is the .rpt itself.
// Keyboard: the main button has focus when it opens (Enter runs the option's own click action); ↑/↓ choose the format;
// Ctrl/⌘+Enter runs its Ctrl+click action; Esc closes (useDialogKeys).
import { useEffect, useRef, useState } from 'react';
import { Mail, Printer, Eye } from 'lucide-react';
import Modal from '../common/Modal';
import { toast, toastStore } from '../common/toastStore';
import { errorText } from '../../lib/api';
import { field, label as labelClass, primaryButton, secondaryButton } from '../../lib/styles';
import {
  emailPrint, fetchPrintOptions, openPreviewTab, runPrint, saveFormat, savedFormat,
  type PrintAction, type PrintKind, type PrintOption, type PrintOptions,
} from '../../lib/printing';

type Choice = { key: string; label: string; option?: PrintOption; rpt?: string };

export default function PrintDialog({ kind, docNo, title, onClose }: {
  kind: PrintKind; docNo: string; title?: string; onClose: () => void;
}) {
  const [info, setInfo] = useState<PrintOptions | null>(null);
  const [error, setError] = useState('');
  const [chosen, setChosen] = useState('');
  const [busy, setBusy] = useState(false);
  const [emailing, setEmailing] = useState(false);
  const [to, setTo] = useState('');
  const mainRef = useRef<HTMLButtonElement>(null);

  useEffect(() => {
    fetchPrintOptions(kind).then(o => {
      setInfo(o);
      const pick = o.designed ? (o.default ?? o.options[0]?.name ?? '') : (o.files.find(f => f === savedFormat(kind)) ?? o.files[0] ?? '');
      setChosen(pick);
    }).catch(e => setError(errorText(e)));
  }, [kind]);

  const choices: Choice[] = !info ? [] : info.designed
    ? info.options.map(o => ({ key: o.name, label: o.name, option: o }))
    : info.files.map(f => ({ key: f, label: f.replace(/\.rpt$/i, ''), rpt: f }));
  const current = choices.find(c => c.key === chosen);
  const own = (c: Choice | undefined, click: 'N' | 'C'): PrintAction => {
    if (!c?.option) return 'preview';
    return (click === 'C' ? c.option.ctrl_click : c.option.click) || 'preview';
  };

  const run = async (action: PrintAction, click: 'N' | 'C' = 'N') => {
    if (!current || busy) return;
    if (action === 'email') { setEmailing(true); return; }
    if (action === 'multi') { toast.error('Multi print is not available in the web app yet. Use Preview.'); return; }
    if (!info?.designed) saveFormat(kind, current.key);
    const tab = action === 'direct' ? null : openPreviewTab();
    setBusy(true);
    try {
      const r = await runPrint({
        kind, doc_no: docNo, click, action: action === 'direct' ? 'direct' : 'preview',
        ...(current.option ? { option: current.option.name } : { rpt: current.rpt }),
      }, tab);
      toast.ok(r.text);
      if (r.action === 'direct') onClose();
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
      const r = await emailPrint({ kind, doc_no: docNo, ...(current.option ? { option: current.option.name } : { rpt: current.rpt }) }, addresses);
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
      const i = choices.findIndex(c => c.key === chosen);
      setChosen(choices[(i + (e.key === 'ArrowDown' ? 1 : choices.length - 1)) % choices.length].key);
    } else if (e.key === 'Enter' && (e.ctrlKey || e.metaKey)) {
      e.preventDefault();
      void run(own(current, 'C') || own(current, 'N'), 'C');
    }
  };

  const plain = own(current, 'N');
  const canDirect = !current?.option || !!current.option.printer;

  return (
    <Modal
      title={title ?? `Print ${info?.doc_name || ''} ${docNo}`.trim()}
      onClose={onClose}
      footer={emailing ? <>
        <button onClick={() => setEmailing(false)} className={secondaryButton}>Back</button>
        <button onClick={sendEmail} disabled={busy || !to.trim()} className={primaryButton}><Mail size={14} />Send email</button>
      </> : <>
        <button onClick={onClose} className={secondaryButton}>Close</button>
        <button onClick={() => run('email')} disabled={!current || busy} className={secondaryButton}><Mail size={14} />Email</button>
        {canDirect && <button onClick={() => run('direct')} disabled={!current || busy || (!!current?.option && !current.option.printer)} className={secondaryButton}
          title={current?.option?.printer ? `Prints on ${current.option.printer}` : 'No printer is set for this option in the Doc Designer'}><Printer size={14} />Print</button>}
        <button ref={mainRef} onClick={() => run(plain)} disabled={!current || busy} className={primaryButton} data-autofocus>
          {plain === 'direct' ? <Printer size={14} /> : <Eye size={14} />}{plain === 'direct' ? 'Print' : 'Preview'}
        </button>
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
          <fieldset>
            <legend className={labelClass}>{info?.designed ? 'Print format' : 'Print format (.rpt file)'}</legend>
            <div role="radiogroup" className="space-y-1">
              {choices.map(c => (
                <label key={c.key} className={`flex cursor-pointer items-center justify-between gap-3 rounded-lg border px-3 py-2 ${c.key === chosen
                  ? 'border-[#64283A] bg-[#64283A]/5 dark:border-[#D4A847] dark:bg-[#D4A847]/10' : 'border-[#E3D9CD] dark:border-[#2A2A2A]'}`}>
                  <span className="flex min-w-0 items-center gap-2">
                    <input type="radio" name="print-format" checked={c.key === chosen} onChange={() => setChosen(c.key)} />
                    <span className="truncate font-medium">{c.label}</span>
                    {c.option?.default && <span className="rounded bg-emerald-100 px-1.5 py-0.5 text-[10px] font-semibold text-emerald-800 dark:bg-emerald-900/40 dark:text-emerald-300">Default</span>}
                  </span>
                  <span className="shrink-0 text-[11px] text-stone-500">
                    {c.option ? `${c.option.rpt}${c.option.copies > 1 ? ` × ${c.option.copies}` : ''}` : ''}
                  </span>
                </label>
              ))}
            </div>
          </fieldset>
        )}
      </div>
    </Modal>
  );
}
