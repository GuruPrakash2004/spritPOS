import { useState, type FormEvent } from 'react';
import { LogIn } from 'lucide-react';
import { api, errorText, type ApiUser } from './api';
import AnimatedGradient, { type GradientConfig } from './ui/animated-gradient';
import { field, label } from './ui';

const FIN_YEARS = ['01/04/2026 - 31/03/2027', '01/04/2025 - 31/03/2026'];

// Version5 "Midnight Sunset": night navy → hero plum → brand orange, drifting slowly behind the card.
// Module-level so the object is stable and the WebGL loop isn't rebuilt on every keystroke.
const SUNSET: GradientConfig = {
  preset: 'custom',
  color1: '#0e101b',
  color2: '#4b2840',
  color3: '#e8702a',
  rotation: -30,
  proportion: 40,
  scale: 0.45,
  speed: 18,
  distortion: 6,
  swirl: 55,
  swirlIterations: 6,
  softness: 100,
  offset: -200,
  shape: 'Edge',
  shapeSize: 45,
};
// Honour reduced motion: same colours, frozen.
const SUNSET_STILL: GradientConfig = { ...SUNSET, speed: 0 };
const prefersReducedMotion = () => typeof window !== 'undefined' && window.matchMedia('(prefers-reduced-motion: reduce)').matches;

// Legacy frmLogin: user, password, company and financial year before the main window opens.
// Demo logins only (ADMIN/admin, GURU/guru, PRADEEP/pradeep, RAVI/ravi).
export default function Login({ onLogin }: { onLogin: (user: ApiUser, finYear: string) => void }) {
  const [user, setUser] = useState('ADMIN');
  const [password, setPassword] = useState('');
  const [finYear, setFinYear] = useState(FIN_YEARS[0]);
  const [error, setError] = useState('');
  const [busy, setBusy] = useState(false);

  const submit = (e: FormEvent) => {
    e.preventDefault();
    if (!user.trim() || !password) return setError('Enter user name and password.');
    setBusy(true);
    api<ApiUser>('/auth/login', { method: 'POST', body: { user, password } })
      .then(u => onLogin(u, finYear))
      .catch(err => setError(errorText(err)))
      .finally(() => setBusy(false));
  };

  return (
    // The static hero gradient stays as the fallback when WebGL2 is unavailable; `isolate` keeps the
    // canvas (z-index -1) above it and below the card.
    <div className="relative isolate flex min-h-screen items-center justify-center overflow-hidden bg-linear-115 from-[#27233b] via-[#4b2840] via-48% to-[#8b422b] p-4">
      <AnimatedGradient config={prefersReducedMotion() ? SUNSET_STILL : SUNSET} noise={{ opacity: 0.12 }} />
      <form onSubmit={submit} className="w-full max-w-sm space-y-4 rounded-[22px] border border-[#e1c9b6] bg-[#fffaf5]/95 p-7 shadow-[0_18px_55px_rgba(0,0,0,0.45)] backdrop-blur">
        <div className="flex items-center gap-3">
          <div className="flex h-11 w-11 items-center justify-center rounded-full bg-linear-135 from-[#e8702a] to-[#e75b6f] text-lg font-bold text-white">V</div>
          <div>
            <h1 className="text-lg font-bold text-stone-900">Softvent vLQMS</h1>
            <p className="text-xs text-stone-500">THE LIQUOR HOUSE • CL-2</p>
          </div>
        </div>
        <div>
          <label htmlFor="login-user" className={label}>User Name</label>
          <input id="login-user" autoFocus value={user} onChange={e => setUser(e.target.value)} className={`${field} w-full uppercase`} autoComplete="username" />
        </div>
        <div>
          <label htmlFor="login-pwd" className={label}>Password</label>
          <input id="login-pwd" type="password" value={password} onChange={e => setPassword(e.target.value)} className={`${field} w-full`} autoComplete="current-password" />
        </div>
        <div>
          <label htmlFor="login-fy" className={label}>Financial Year</label>
          <select id="login-fy" value={finYear} onChange={e => setFinYear(e.target.value)} className={`${field} w-full font-mono`}>
            {FIN_YEARS.map(y => <option key={y}>{y}</option>)}
          </select>
        </div>
        {error && <p role="alert" className="rounded-lg bg-red-100 px-3 py-2 text-xs font-medium text-red-700">{error}</p>}
        <button type="submit" disabled={busy} className="flex w-full items-center justify-center gap-2 rounded-xl bg-[#e8702a] py-2.5 text-sm font-bold text-white shadow-sm hover:bg-[#b84716] disabled:opacity-60">
          <LogIn size={16} />{busy ? 'Signing in…' : 'Sign in'}
        </button>
        <p className="text-center text-[10px] text-stone-400">Demo: ADMIN / admin</p>
      </form>
    </div>
  );
}
