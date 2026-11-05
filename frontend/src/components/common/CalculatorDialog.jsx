// The desktop's calculator (frmUtlCalci): type an expression such as 1250*3-40, the result shows as you type, and Esc (or Enter) copies the
// result to the clipboard and closes. Opened with Alt+K from any screen. The expression is read by lib/calculate.js, never run as code.

import { useMemo, useState } from 'react';
import Modal from './Modal';
import { calculate } from '../../lib/calculate';
import { field, secondaryButton } from '../../lib/styles';

export default function CalculatorDialog({ onClose }) {
  const [text, setText] = useState('');
  const result = useMemo(() => { try { return { value: calculate(text), error: '' }; } catch { return { value: null, error: 'There is some error in the entered expression. Please rectify it.' }; } }, [text]);

  const done = () => {
    if (result.value !== null) { try { void navigator.clipboard?.writeText(String(result.value)); } catch { /* not copied */ } }
    onClose();
  };

  return (
    <Modal title="Calculator" onClose={done} footer={<button onClick={done} className={secondaryButton}>Close (Esc copies the result)</button>}>
      <label htmlFor="calc-expr" className="mb-1 block text-xs font-semibold">Expression</label>
      <input id="calc-expr" autoFocus value={text} onChange={e => setText(e.target.value)} onKeyDown={e => { if (e.key === 'Enter') { e.preventDefault(); done(); } }}
        placeholder="e.g. 1250*3-40" autoComplete="off" className={`${field} text-lg tabular-nums`} />
      <p className="mt-3 text-xs font-semibold text-stone-500">Result</p>
      <p aria-live="polite" className="min-h-9 text-3xl font-bold tabular-nums">{result.value ?? ''}</p>
      {result.error && text && <p role="alert" className="mt-1 text-xs text-red-600">{result.error}</p>}
    </Modal>
  );
}
