// Printing on THIS computer's own printers (USB or network, and its default printer), through the vLQMS Print Agent
// (backend/tools/printagent): a small program on the counter PC that listens on 127.0.0.1:8766. A web page cannot list or
// use the printers of the PC it is open on, so the agent does it; the server only draws the pages (POST /print/pages).
// Nothing here touches the database: the choice of printer is remembered in this browser (localStorage).
//
//   const agent = await detectAgent();            // null when the agent isn't running on this PC
//   agent.printers, agent.default, agent.host
//   await printOnThisPc(pages, { printer, copies, name })   // pages from renderPrintPages()

import { renderPrintPages } from './api';

const BASE = 'http://127.0.0.1:8766';
const PRINTER_KEY = 'vlqms_doc_printer';      // the last printer picked in the Print window; "pc:Name" = a printer of this PC
export const PC_PREFIX = 'pc:';

let seen = { at: 0, agent: null };

/** The agent's answer ({host, default, printers}) or null. Asked at most every 20 s (every 3 s while it is missing, so a just-started agent is found quickly). */
export async function detectAgent(force = false) {
  const age = Date.now() - seen.at;
  if (!force && seen.at && age < (seen.agent ? 20_000 : 3_000)) return seen.agent;
  let agent = null;
  try {
    const ctl = new AbortController();
    const timer = setTimeout(() => ctl.abort(), 600);   // short: a PC with no agent must never make a print or a save wait
    const res = await fetch(`${BASE}/health`, { signal: ctl.signal });
    clearTimeout(timer);
    const data = res.ok ? await res.json() : null;
    if (data?.agent === 'vlqms-print-agent') agent = { host: data.host || '', default: data.default || '', printers: data.printers || [], version: data.version };
  } catch { /* not running on this PC, or this page isn't allowed in its printagent.txt */ }
  seen = { at: Date.now(), agent };
  return agent;
}

/** What is known about the agent right now, with no waiting (null until it has been found); `warmAgent()` looks again in the background.
 *  A save or a print uses these, so the probe of 127.0.0.1 can never hold up billing. */
export const agentNow = () => seen.agent;
export const warmAgent = () => { void detectAgent(); };
// Found as soon as the page loads, so the first print after sign-in already knows whether this PC has the agent.
if (typeof window !== 'undefined' && import.meta.env?.MODE !== 'test') warmAgent();

export const savedPrinter = () => { try { return localStorage.getItem(PRINTER_KEY) || ''; } catch { return ''; } };
export const rememberPrinter = (value) => { try { localStorage.setItem(PRINTER_KEY, value); } catch { /* not remembered */ } };

/** The printer of this PC a direct print should use: the one picked last on this PC, else its default printer; '' when no agent / no printer. */
export const pcPrinterFor = (agent) => {
  if (!agent) return '';
  const saved = savedPrinter();
  if (saved.startsWith(PC_PREFIX) && agent.printers.includes(saved.slice(PC_PREFIX.length))) return saved.slice(PC_PREFIX.length);
  return agent.default || '';
};

/** Send drawn pages to a printer of this PC. Throws an Error with a message for the user. */
export async function printOnThisPc(pages, { printer, copies = 1, name = '' }) {
  let res;
  try {
    res = await fetch(`${BASE}/print`, {
      method: 'POST', headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ printer, copies, name, pages }),
    });
  } catch {
    seen = { at: 0, agent: null };
    throw new Error('The Print Agent is not running on this PC. Start it (see backend/tools/printagent/README.md) or choose a server printer.');
  }
  const data = await res.json().catch(() => null);
  if (!res.ok) throw new Error(data?.error || `The Print Agent refused the job (${res.status}).`);
  return data;
}

/** Draw a document on the server and print it on this PC: used by the Print window and by a save that prints at once. */
export async function printDocumentOnThisPc(req, printer) {
  const drawn = await renderPrintPages(req);
  const r = await printOnThisPc(drawn.pages, { printer, copies: drawn.copies || 1, name: drawn.name || req.doc_no });
  return { printer: r.printer || printer, copies: r.copies || drawn.copies || 1 };
}
