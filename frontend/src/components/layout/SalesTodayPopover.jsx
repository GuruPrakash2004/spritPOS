// The footer's "Sales today" button and the floating summary it opens: today's sales split by how the money came in
// (cash / card / UPI / credit), taken from the DSR the footer already loads. The box pours out of the button and
// sucks back into it (genie effect, CSS in index.css). Esc, a click outside or the button again closes it.

import { useEffect, useRef, useState } from 'react';
import { money } from '../../lib/styles';

const ROWS = [['cash', 'Cash'], ['card', 'Card'], ['upi', 'UPI'], ['credit', 'Credit']];
const inr = (n) => Number(n || 0).toLocaleString('en-IN', { minimumFractionDigits: 2, maximumFractionDigits: 2 });

export default function SalesTodayPopover({ dsr }) {
  const [open, setOpen] = useState(false);
  const [leaving, setLeaving] = useState(false);
  const wrap = useRef(null);

  const close = () => {
    setLeaving(true);
    window.setTimeout(() => { setOpen(false); setLeaving(false); }, 220);
  };
  const toggle = () => (open && !leaving ? close() : (setLeaving(false), setOpen(true)));

  useEffect(() => {
    if (!open) return undefined;
    const onKey = (e) => { if (e.key === 'Escape') close(); };
    const onDown = (e) => { if (wrap.current && !wrap.current.contains(e.target)) close(); };
    window.addEventListener('keydown', onKey);
    window.addEventListener('mousedown', onDown);
    return () => { window.removeEventListener('keydown', onKey); window.removeEventListener('mousedown', onDown); };
  }, [open]);

  const modes = dsr?.payment_modes || {};
  const total = dsr?.sales_incl_gst ?? 0;
  const extra = Object.keys(modes).filter(k => !ROWS.some(([key]) => key === k) && modes[k]);

  return (
    <span ref={wrap} className="relative">
      <button type="button" onClick={toggle} aria-expanded={open} aria-haspopup="dialog" title="Today's sales by payment mode" className="rounded px-1 hover:bg-subtle focus-visible:outline focus-visible:outline-2">
        Sales today <span className="font-semibold text-[#1B2B4B] dark:text-[#D4A847]">₹ {inr(total)}</span>
      </button>
      {open && (
        <div
          role="dialog"
          aria-label="Sales today by payment mode"
          style={{ transformOrigin: '88% 100%' }}
          className={`${leaving ? 'genie-out' : 'genie-in'} absolute bottom-full right-0 z-[60] mb-2 w-64 rounded-xl border border-line bg-surface p-3 text-xs shadow-2xl dark:bg-[#141414]`}
        >
          <div className="mb-2 flex items-baseline justify-between">
            <span className="font-semibold text-stone-800 dark:text-stone-100">Sales today</span>
            <span className="text-[11px] text-stone-500">{dsr?.total_bills ?? 0} bills</span>
          </div>
          <dl className="space-y-1 tabular-nums">
            {ROWS.map(([key, label]) => (
              <div key={key} className="flex justify-between gap-3">
                <dt className="text-stone-500 dark:text-stone-400">{label}</dt>
                <dd className={modes[key] ? 'font-medium text-stone-900 dark:text-stone-100' : 'text-stone-400'}>₹ {inr(modes[key])}</dd>
              </div>
            ))}
            {extra.map(k => (
              <div key={k} className="flex justify-between gap-3">
                <dt className="capitalize text-stone-500 dark:text-stone-400">{k}</dt>
                <dd className="font-medium text-stone-900 dark:text-stone-100">₹ {inr(modes[k])}</dd>
              </div>
            ))}
            <div className="flex justify-between gap-3 border-t border-line pt-1.5 text-sm font-semibold text-stone-900 dark:text-stone-50">
              <dt>Total</dt><dd>₹ {money(total)}</dd>
            </div>
          </dl>
        </div>
      )}
    </span>
  );
}
