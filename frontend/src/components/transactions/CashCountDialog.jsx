// The Sales Invoice's Denomination window (the desktop's frmUtlDenom grid / TrnDtlSINVDenom): a row per note (2000, 500, 200, 100, 50, 20, 10)
// and one for loose coins, each with its picture, how many ("Nos") and the amount. OK puts the total in "Cash tendered"; the counts are
// saved with the bill. Double-click a picture to add one to Nos (the desktop's tip); Clear zeroes everything.
// Keys: Enter = next row (OK on the last), ↑/↓ move between rows, Esc cancels.
//
//   <CashCountDialog counts={{ '500': 2 }} onApply={(counts, total) => …} onClose={() => …} />
// Coins: Nos is the rupee amount of loose coins (as on the Cash Denomination screen).

import { useEffect, useState } from 'react';
import { getDenominationConfig } from '../../lib/api';
import { field, money, primaryButton, secondaryButton } from '../../lib/styles';
import Modal from '../common/Modal';
import NoteImage from './NoteImage';

const STANDARD = ['2000', '500', '200', '100', '50', '20', '10', 'coins'];
const face = (v) => (v === 'coins' ? 1 : Number(v));   // what one "Nos" is worth

export default function CashCountDialog({ counts, onApply, onClose }) {
  const [values, setValues] = useState(null);
  const [draft, setDraft] = useState(counts ?? {});
  const [row, setRow] = useState(0);
  useEffect(() => { getDenominationConfig().then(r => setValues(r.values?.length ? r.values : STANDARD)).catch(() => setValues(STANDARD)); }, []);

  const total = (values ?? []).reduce((sum, v) => sum + face(v) * (Number(draft[v]) || 0), 0);
  const apply = () => {
    const clean = Object.fromEntries(Object.entries(draft).filter(([, n]) => Number(n) > 0).map(([v, n]) => [v, Math.floor(Number(n))]));
    onApply(clean, total);
  };
  const focusRow = (i) => { setRow(i); document.getElementById(`dn-${i}`)?.focus(); };
  const bump = (v, i) => { setDraft(d => ({ ...d, [v]: (Math.floor(Number(d[v])) || 0) + 1 })); focusRow(i); };

  return (
    <Modal title="Denomination" onClose={onClose} wide footer={
      <>
        <button onClick={() => { setDraft({}); focusRow(0); }} className={secondaryButton}>Clear</button>
        <button onClick={onClose} className={secondaryButton}>Cancel</button>
        <button onClick={apply} className={primaryButton}>OK</button>
      </>
    }>
      {!values ? <p className="text-sm text-stone-500">Loading…</p> : (
        <div className="space-y-3">
          <table className="w-full border border-line text-sm">
            <thead><tr className="bg-subtle text-left text-xs"><th className="w-28 border border-line px-2 py-1 font-medium text-[#1B5FA8]">Currency</th><th className="w-40 border border-line px-2 py-1 font-medium">Nos</th><th className="border border-line px-2 py-1 text-right font-medium">Amount</th></tr></thead>
            <tbody>
              {values.map((v, i) => {
                const amount = face(v) * (Number(draft[v]) || 0);
                const on = i === row;
                return (
                  <tr key={v} className={on ? 'bg-[#FF8C1A] text-white' : ''}>
                    <td className="border border-line p-0.5 text-center">
                      <button type="button" tabIndex={-1} onDoubleClick={() => bump(v, i)} title="Double click to add one" className="block cursor-pointer">
                        <NoteImage value={v} className="h-9 w-[4.5rem]" />
                      </button>
                    </td>
                    <td className={`border border-line p-0 ${on ? 'bg-[#FFFF80]' : ''}`}>
                      <input id={`dn-${i}`} type="number" min={0} autoFocus={i === 0} value={draft[v] ?? ''} aria-label={v === 'coins' ? 'Coins amount' : `Number of ${v} notes`}
                        onFocus={e => { setRow(i); e.target.select(); }}
                        onChange={e => setDraft(d => ({ ...d, [v]: e.target.value }))}
                        onKeyDown={e => {
                          if (e.key === 'Enter') { e.preventDefault(); e.stopPropagation(); if (i + 1 < values.length) focusRow(i + 1); else apply(); }
                          else if (e.key === 'ArrowDown' && i + 1 < values.length) { e.preventDefault(); focusRow(i + 1); }
                          else if (e.key === 'ArrowUp' && i > 0) { e.preventDefault(); focusRow(i - 1); }
                        }}
                        className={`${field} h-10 w-full !rounded-none !border-0 bg-transparent text-right text-base tabular-nums text-stone-900`} />
                    </td>
                    <td className="border border-line px-2 text-right text-base tabular-nums">{Math.round(amount * 100) / 100}</td>
                  </tr>
                );
              })}
            </tbody>
          </table>
          <output aria-label="Total" className="block rounded border border-line bg-subtle px-3 py-2 text-base font-semibold tabular-nums">{money(total)}</output>
          <p className="text-[11px] text-stone-500">Tip: Double click on the currency image to increase the Nos · Enter next row · Coins: enter the rupee amount</p>
        </div>
      )}
    </Modal>
  );
}
