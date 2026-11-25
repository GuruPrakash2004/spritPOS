// printHtml(html): prints a small self-contained HTML page through the browser's own print dialog (Save as PDF works from there too).
// It draws the page in a hidden frame, so no pop-up window is opened and nothing on the screen changes. `esc` makes text safe to put in the page.

export const esc = (v) => String(v ?? '').replace(/[&<>"']/g, ch => ({ '&': '&amp;', '<': '&lt;', '>': '&gt;', '"': '&quot;', "'": '&#39;' }[ch]));

export function printHtml(html) {
  const frame = document.createElement('iframe');
  frame.setAttribute('aria-hidden', 'true');
  frame.style.cssText = 'position:fixed;right:0;bottom:0;width:0;height:0;border:0;';
  document.body.appendChild(frame);
  const doc = frame.contentDocument;
  doc.open();
  doc.write(html);
  doc.close();
  const run = () => {
    try { frame.contentWindow.focus(); frame.contentWindow.print(); } finally { setTimeout(() => frame.remove(), 1000); }
  };
  if (doc.readyState === 'complete') run(); else frame.onload = run;
}
