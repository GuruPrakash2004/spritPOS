// Print window for a report laid out by the desktop's .rpt files (Crystal Reports bridge): a list of the report's
// formats on the left (click one to see it), the PDF as Crystal laid it out on the right, and two ways to print:
//   Print            prints the PDF that is on show (the browser's print dialog, page size as the report's);
//   Print directly   sends the report to the chosen printer through the bridge, without showing anything.
// The caller passes `render(rpt, printer?)`: without a printer it answers the PDF, with one it prints and answers JSON.

import { useEffect, useRef, useState } from 'react';
import { Printer, Send } from 'lucide-react';
import Modal from '../common/Modal';
import { toastStore } from '../common/toastStore';
import { errorText, getPrintBridge } from '../../lib/api';
import { field, label as labelClass, primaryButton, secondaryButton } from '../../lib/styles';
import { printFrame, sheetStyle, usePdfPreview } from './usePdfPreview';








const PRINTER_KEY = 'vlqms_report_printer';
const savedPrinter = () => { try { return localStorage.getItem(PRINTER_KEY) || ''; } catch { return ''; } };

export default function ReportPrintModal({ title, formats, render, onClose }) {
  const [chosen, setChosen] = useState(formats[0] ?? '');
  const [printers, setPrinters] = useState([]);
  const [printer, setPrinter] = useState(savedPrinter());
  const [sending, setSending] = useState(false);
  const frame = useRef(null);
  const { url, loading, error, size } = usePdfPreview(chosen, () => render(chosen));

  useEffect(() => {
    getPrintBridge().then(b => setPrinters(b.printers ?? [])).catch(() => { /* the preview says why */ });
  }, []);

  const printDirect = async () => {
    if (!printer || sending) return;
    if (!(await toastStore.defer('ok', `${title} will be sent to ${printer} when this closes.`, 'Print cancelled. Nothing was sent.'))) return;
    setSending(true);
    try {
      await render(chosen, printer);
      try { localStorage.setItem(PRINTER_KEY, printer); } catch { /* blocked */ }
      toastStore.add('ok', `${title} sent to ${printer}.`);
    } catch (e) {
      toastStore.add('error', errorText(e));
    } finally {
      setSending(false);
    }
  };

  return (
    <Modal
      title={title}
      onClose={onClose}
      wide
      footer={<>
        <button onClick={onClose} className={secondaryButton}>Close</button>
        <button onClick={printDirect} disabled={!printer || !chosen || sending} className={secondaryButton}
          title={printer ? `Prints on ${printer} without showing it` : 'Choose a printer first'}><Send size={14} />Print directly</button>
        <button onClick={() => printFrame(frame, url)} disabled={!url || loading} className={primaryButton} data-autofocus><Printer size={14} />Print</button>
      </>}
    >
      <div className="flex flex-col gap-3 sm:flex-row">
        <div className="sm:w-48 sm:shrink-0">
          <p className={labelClass}>Report format</p>
          <div role="radiogroup" aria-label="Report format" className="space-y-1">
            {formats.map(f => (
              <label key={f} className={`flex cursor-pointer items-center gap-2 rounded-lg border px-3 py-2 ${f === chosen
                ? 'border-[#64283A] bg-[#64283A]/5 dark:border-[#D4A847] dark:bg-[#D4A847]/10' : 'border-[#E3D9CD] dark:border-[#2A2A2A]'}`}>
                <input type="radio" name="report-format" checked={f === chosen} onChange={() => setChosen(f)} />
                <span className="truncate font-medium">{f.replace(/\.rpt$/i, '')}</span>
              </label>
            ))}
          </div>
          <label className="mt-4 block">
            <span className={labelClass}>Printer (for Print directly)</span>
            <select value={printer} onChange={e => setPrinter(e.target.value)} className={field}>
              <option value="">Choose a printer…</option>
              {printers.map(p => <option key={p} value={p}>{p}</option>)}
            </select>
          </label>
        </div>
        <div className="relative flex min-h-[60vh] flex-1 items-start justify-center overflow-auto rounded-lg border border-[#E3D9CD] bg-stone-200 p-4 dark:border-[#2A2A2A] dark:bg-[#0c0c0c]">
          {loading && <p className="absolute inset-0 flex items-center justify-center text-stone-500">Laying out the report…</p>}
          {error && <p role="alert" className="p-3 text-red-600 dark:text-red-400">{error}</p>}
          {url && !loading && <iframe ref={frame} title={`${title} preview`} src={`${url}#toolbar=0&navpanes=0&view=FitH`} style={sheetStyle(size)} className="bg-white shadow-lg ring-1 ring-black/10" />}
        </div>
      </div>
    </Modal>
  );
}
