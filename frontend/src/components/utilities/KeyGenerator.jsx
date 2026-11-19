// Utilities → Key Generation (the desktop's frmKeyReg "Key Generator", the vendor's tool): type a customer's Reg. No. (numbers only) and press
// Enter; the Key shows below, or "-" when that Reg. No. has none. Administrator only, and only on a server that holds the vendor's key file
// (backend/app/licence_keys.py): the table of keys never leaves the server, only the one key asked for.

import { useEffect, useState } from 'react';
import { KeyRound, X } from 'lucide-react';
import { InlineError } from '../common/FormErrorSummary';
import { errorText, getKeyGeneratorAvailable, getLicenceKey } from '../../lib/api';
import { goHome } from '../../lib/nav';
import { field, label, pageSub, pageTitle, panel, secondaryButton } from '../../lib/styles';

export default function KeyGenerator() {
  const [available, setAvailable] = useState(null);   // null while asking the server
  const [regNo, setRegNo] = useState('');
  const [key, setKey] = useState('');
  const [problem, setProblem] = useState('');

  useEffect(() => { getKeyGeneratorAvailable().then(r => setAvailable(r.available)).catch(e => { setAvailable(false); setProblem(errorText(e)); }); }, []);

  const generate = async () => {
    if (!regNo.trim()) return;
    try {
      setKey((await getLicenceKey(regNo.trim())).key);
      setProblem('');
    } catch (e) {
      setKey('');
      setProblem(errorText(e));
    }
  };

  if (available === null) return null;
  return (
    <div className="space-y-4">
      <div>
        <h1 className={pageTitle}>Key Generator</h1>
        <p className={pageSub}>The vendor's tool: the Key for a customer's Reg. No.</p>
      </div>
      {!available ? (
        <section className={`${panel} p-4 text-xs text-stone-600 dark:text-stone-300`}>
          {problem || "The Key Generator is not set up on this server. It works only where the vendor's key file is installed."}
        </section>
      ) : (
        <section className={`${panel} max-w-md space-y-4 p-5`} aria-label="Key generator">
          <div>
            <label htmlFor="kg-reg" className={label}>Reg. No.</label>
            <input id="kg-reg" autoFocus inputMode="numeric" autoComplete="off" value={regNo} className={`${field} font-mono`}
              onChange={e => { setRegNo(e.target.value.replace(/\D/g, '')); setKey(''); setProblem(''); }}
              onKeyDown={e => { if (e.key === 'Enter') { e.preventDefault(); void generate(); } }} />
          </div>
          <div>
            <label htmlFor="kg-key" className={label}>Key</label>
            <div className="relative">
              <input id="kg-key" readOnly value={key} className={`${field} pr-9 font-mono`} />
              <KeyRound size={14} className="pointer-events-none absolute right-3 top-1/2 -translate-y-1/2 text-stone-400" />
            </div>
          </div>
          <p className="text-[11px] text-stone-500">Type the Reg. No. and press Enter. A dash means that number has no key.</p>
          {problem && <InlineError message={problem} />}
        </section>
      )}
      <button type="button" className={secondaryButton} onClick={goHome}><X size={14} />Close</button>
    </div>
  );
}
