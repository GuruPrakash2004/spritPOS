// Sign-in page (legacy frmLogin). Posts to POST /auth/login and hands the returned user
// (role, modules, JWT) and the chosen financial year to App.tsx via onLogin.
// Has its own light/dark toggle and shows the company name from useCompanyProfile().

import { useState, useEffect, type FormEvent } from 'react';
import { AlertCircle, Loader2, LogIn, Moon, Sun, Wine } from 'lucide-react';
import { api, errorText, setRememberSession, type ApiUser } from '../../lib/api';
import { loadPasswordRules } from '../../lib/passwords';
import { encryptSecret } from '../../lib/secureField';
import { IMAGES } from '../../constants/images';
import VlqmsLogo from '../layout/VlqmsLogo';
import DevelopedBy from '../layout/DevelopedBy';
import { useCompanyProfile } from '../../hooks/useCompanyProfile';

const FIN_YEARS = ['01/04/2026 - 31/03/2027', '01/04/2025 - 31/03/2026'];

// ─── REALISTIC WATER DROPLET FLOW & LIQUOR SPLASH DYNAMICS ───
// Simulates individual liquor water droplets cascading downwards with gravity acceleration
// and erupting in multi-bead splashes and concentric ripples upon hitting the roaming liquor surface.
const LiquidDropletWaterfall = () => {
  const drops = [
    { left: '18%', duration: '2.4s', delay: '0.0s', size: 10 },
    { left: '36%', duration: '2.8s', delay: '1.1s', size: 12 },
    { left: '54%', duration: '2.3s', delay: '0.5s', size: 11 },
    { left: '72%', duration: '2.9s', delay: '1.6s', size: 13 },
    { left: '88%', duration: '2.5s', delay: '0.8s', size: 10 },
  ];

  return (
    <div className="pointer-events-none absolute inset-0 overflow-hidden bottle-container-glass" aria-hidden="true">
      {/* ── 1. Cascading Droplets with Splash Impact ── */}
      {drops.map((drop, idx) => (
        <div key={`water-drop-${idx}`} className="absolute top-0 bottom-0" style={{ left: drop.left }}>
          {/* Subtle luminous vertical stream guide */}
          <div
            className="absolute top-0 bottom-0 w-[2px] opacity-40 dark:opacity-60"
            style={{
              background: 'linear-gradient(180deg, transparent 0%, rgba(245, 213, 128, 0.3) 20%, rgba(212, 140, 45, 0.5) 70%, transparent 100%)',
              filter: 'blur(0.5px)',
            }}
          />

          {/* Realistic falling water/liquor droplet (teardrop with highlight) */}
          <div
            className="absolute -top-4 rounded-full"
            style={{
              width: `${drop.size}px`,
              height: `${drop.size * 1.5}px`,
              left: `-${drop.size / 2 - 1}px`,
              background: 'radial-gradient(ellipse at 35% 25%, #FFF5D6 0%, #F5D580 35%, #D48030 75%, #8A3D12 100%)',
              boxShadow: '0 0 10px rgba(245, 185, 60, 0.85), inset 0 1px 2px rgba(255,255,255,0.8)',
              borderRadius: '50% 50% 50% 50% / 40% 40% 60% 60%',
              animation: `dropletFallAccelerate ${drop.duration} cubic-bezier(0.55, 0.085, 0.68, 0.53) ${drop.delay} infinite`,
            }}
          />

          {/* Splash Impact Center at liquid line (around 82% height) */}
          <div
            className="absolute"
            style={{
              top: '82%',
              left: '0px',
              width: '1px',
              height: '1px',
            }}
          >
            {/* Expanding impact concentric ripple */}
            <div
              className="absolute -left-6 -top-2.5 rounded-full border border-amber-300/80 dark:border-amber-400"
              style={{
                width: '48px',
                height: '18px',
                animation: `dropletImpactRipple ${drop.duration} ease-out ${drop.delay} infinite`,
              }}
            />

            {/* Splashing liquid bead — Center */}
            <div
              className="absolute -left-1 -top-1 rounded-full bg-amber-200 dark:bg-amber-300 shadow-[0_0_8px_rgba(255,220,100,0.9)]"
              style={{
                width: '4px',
                height: '4px',
                animation: `splashBeadCenter ${drop.duration} ease-out ${drop.delay} infinite`,
              }}
            />

            {/* Splashing liquid bead — Left */}
            <div
              className="absolute -left-1 -top-1 rounded-full bg-amber-300 dark:bg-amber-400 shadow-[0_0_6px_rgba(240,180,60,0.8)]"
              style={{
                width: '3.5px',
                height: '3.5px',
                animation: `splashBeadLeft ${drop.duration} ease-out ${drop.delay} infinite`,
              }}
            />

            {/* Splashing liquid bead — Right */}
            <div
              className="absolute -left-1 -top-1 rounded-full bg-amber-300 dark:bg-amber-400 shadow-[0_0_6px_rgba(240,180,60,0.8)]"
              style={{
                width: '3.5px',
                height: '3.5px',
                animation: `splashBeadRight ${drop.duration} ease-out ${drop.delay} infinite`,
              }}
            />
          </div>
        </div>
      ))}

      {/* ── 2. Roaming Liquor at the Bottom (Bottle Container Reservoir) ── */}
      <div className="absolute inset-x-0 bottom-0 h-44 sm:h-56 overflow-hidden pointer-events-none">
        {/* Micro-bubbles floating upward */}
        {[
          { left: '18%', delay: '0.2s', duration: '4.5s', size: 5 },
          { left: '35%', delay: '1.6s', duration: '3.8s', size: 6 },
          { left: '52%', delay: '0.9s', duration: '5.0s', size: 4 },
          { left: '68%', delay: '2.4s', duration: '4.2s', size: 7 },
          { left: '82%', delay: '1.2s', duration: '3.5s', size: 5 },
          { left: '44%', delay: '2.9s', duration: '4.7s', size: 6 },
        ].map((b, i) => (
          <div
            key={`b-${i}`}
            className="absolute bottom-4 rounded-full bg-amber-200/80 dark:bg-amber-300/85 shadow-[0_0_6px_rgba(255,210,90,0.85)]"
            style={{
              left: b.left,
              width: `${b.size}px`,
              height: `${b.size}px`,
              animation: `bubbleRise ${b.duration} ease-in ${b.delay} infinite`,
            }}
          />
        ))}

        {/* Deep wave — base liquor */}
        <svg
          className="absolute bottom-0 w-[200%] h-40"
          style={{ animation: 'roamingLiquidSlosh1 11s ease-in-out infinite alternate' }}
          viewBox="0 0 1440 160"
          preserveAspectRatio="none"
        >
          <path
            d="M0,60 C240,110 480,20 720,65 C960,110 1200,25 1440,60 L1440,160 L0,160 Z"
            className="fill-[#B06020]/45 dark:fill-[#B08A45]/35"
          />
        </svg>

        {/* Mid wave — fluid liquor wave */}
        <svg
          className="absolute bottom-0 w-[200%] h-36"
          style={{ animation: 'roamingLiquidSlosh2 7.5s ease-in-out infinite alternate' }}
          viewBox="0 0 1440 140"
          preserveAspectRatio="none"
        >
          <path
            d="M0,50 C220,15 460,95 700,45 C940,5 1180,85 1440,50 L1440,140 L0,140 Z"
            className="fill-[#D48030]/55 dark:fill-[#D4A847]/45"
          />
        </svg>

        {/* Surface meniscus wave */}
        <svg
          className="absolute bottom-0 w-[200%] h-32"
          style={{ animation: 'roamingLiquidSlosh3 5.2s ease-in-out infinite alternate' }}
          viewBox="0 0 1440 120"
          preserveAspectRatio="none"
        >
          <path
            d="M0,35 C180,65 420,10 680,40 C940,65 1180,15 1440,35 L1440,120 L0,120 Z"
            className="fill-[#F0B048]/70 dark:fill-[#E8C36A]/65"
          />
        </svg>
      </div>

      {/* ── 3. Glass reflections along container edges ── */}
      <div className="absolute inset-y-0 left-0 w-8 bg-gradient-to-r from-white/12 to-transparent pointer-events-none" />
      <div className="absolute inset-y-0 right-0 w-8 bg-gradient-to-l from-white/10 to-transparent pointer-events-none" />
    </div>
  );
};

export default function Login({ onLogin, notice = '' }: { onLogin: (user: ApiUser, finYear: string) => void; notice?: string }) {
  const { company } = useCompanyProfile();
  const [user, setUser] = useState('ADMIN');
  const [password, setPassword] = useState('');
  const [finYear, setFinYear] = useState(FIN_YEARS[0]);
  // Login Settings: offer "Remember me" only when the Administrator allows it.
  const [offerRemember, setOfferRemember] = useState(false);
  const [remember, setRemember] = useState(false);
  useEffect(() => { loadPasswordRules().then(r => setOfferRemember(r.show_remember_me)); }, []);
  // Starts with the reason the last session ended, if any (see App.tsx).
  const [error, setError] = useState(notice);
  const [busy, setBusy] = useState(false);

  // Theme toggle state
  const [dark, setDark] = useState(() => {
    return document.documentElement.classList.contains('dark') ||
           (!('theme' in localStorage) && window.matchMedia('(prefers-color-scheme: dark)').matches);
  });

  useEffect(() => {
    document.documentElement.classList.toggle('dark', dark);
    localStorage.setItem('theme', dark ? 'dark' : 'light');
  }, [dark]);

  const submit = (e: FormEvent) => {
    e.preventDefault();
    if (!user.trim() || !password) return setError('Enter user name and password.');
    setBusy(true);
    setRememberSession(offerRemember && remember);
    // The password leaves the browser only encrypted (lib/secureField.ts).
    encryptSecret(password, 'login')
      .then(sealed => api<ApiUser>('/auth/login', { method: 'POST', body: { user, password: sealed } }))
      .then(u => onLogin(u, finYear))
      .catch(err => setError(errorText(err)))
      .finally(() => setBusy(false));
  };

  const inputClass = `w-full rounded-xl border px-4 py-3 text-sm outline-none transition-all duration-200
    border-[#DCD1C5] bg-[#FFFCF7] text-[#29231F] placeholder:text-[#A69D94]
    hover:border-[#CBBEAF]
    focus:border-[#64283A] focus:ring-[3px] focus:ring-[#64283A]/10
    dark:border-[#2A2A2A] dark:bg-[#1A1A1A] dark:text-stone-50
    dark:placeholder:text-stone-600 dark:hover:border-[#3A3A3A]
    dark:focus:border-[#D4A847] dark:focus:ring-[#D4A847]/20`;

  const labelClass = 'mb-2 block text-[11px] font-semibold uppercase tracking-[0.08em] text-[#70675F] dark:text-stone-400';

  return (
    <div className="flex min-h-screen font-sans bg-[#F5F1E9] dark:bg-[#0A0A0A] text-[#29231F] dark:text-stone-100 transition-colors duration-300">
      
      {/* ═══ TOP-RIGHT THEME SWITCHER ═══ */}
      <div className="fixed top-5 right-5 z-50">
        <button
          type="button"
          onClick={() => setDark(!dark)}
          aria-label="Toggle theme"
          className="inline-flex h-10 w-10 items-center justify-center rounded-xl border
                     bg-[#FFFCF7]/90 border-[#E3D9CD] text-[#70675F] shadow-md
                     hover:text-[#64283A] hover:border-[#64283A]/30
                     dark:bg-[#1C1C1C]/90 dark:border-[#2A2A2A] dark:text-[#D4A847] dark:hover:bg-[#252525]
                     backdrop-blur-md transition-all active:scale-95"
          title={dark ? 'Switch to Heritage Light' : 'Switch to Dark Luxury'}
        >
          {dark ? <Sun size={18} /> : <Moon size={18} />}
        </button>
      </div>

      {/* ═══ LEFT — Cinematic Image Panel (Transparent Bottle Container) ═══ */}
      <div className="hidden lg:flex lg:w-[58%] relative overflow-hidden bg-[#180E09] dark:bg-[#0A0A0A]">
        {/* Background luxury bar shelves image with 65% visibility */}
        <img
          src={IMAGES.login.hero}
          alt="Luxury Bar Shelves"
          className="absolute inset-0 h-full w-full object-cover object-[center_35%] ken-burns"
          style={{ opacity: 0.65, filter: 'brightness(0.96) saturate(0.96)' }}
        />

        {/* Tailored atmospheric gradients preserving 65% image clarity */}
        <div className="absolute inset-0 bg-gradient-to-r from-[#180E09]/85 via-[#180E09]/40 to-[#180E09]/65
                        dark:from-[#0A0A0A]/85 dark:via-[#0A0A0A]/45 dark:to-[#0A0A0A]/75" />
        <div className="absolute inset-0 bg-gradient-to-t from-[#180E09]/80 via-transparent to-[#180E09]/30
                        dark:from-[#0A0A0A]/85 dark:via-transparent dark:to-[#0A0A0A]/35" />

        {/* Real Liquid Droplet Flow & Splash Dynamics */}
        <LiquidDropletWaterfall />

        {/* Content overlay */}
        <div className="relative z-10 flex flex-col justify-between p-10 xl:p-14 w-full">
          {/* Top — Custom VLQMS Emblem Logo */}
          <div className="flex items-center gap-3 p-[4px] animate-fadeInUp">
            <VlqmsLogo size="md" showText={true} onDark={true} />
          </div>

          {/* Bottom — Hero Typography & Metric Badges */}
          <div className="max-w-lg mb-6">
            <p className="animate-fadeInUp animate-fadeInUp-d1 text-[11px] font-bold tracking-[0.35em] text-[#D3BB89] uppercase">
              Enterprise Resource Planning
            </p>
            <h1 className="animate-fadeInUp animate-fadeInUp-d2 mt-4 text-4xl xl:text-5xl font-extrabold text-white leading-[1.1]"
                style={{ fontFamily: 'var(--font-display)' }}>
              Everything under{' '}
              <span className="text-gold-gradient">one roof.</span>
            </h1>
            <p className="animate-fadeInUp animate-fadeInUp-d3 mt-4 text-sm leading-relaxed text-stone-200/90 max-w-md">
              Monitor inventory, sales, procurement and compliance across your entire
              distillery & retail operation — from warehouse to countertop.
            </p>


          </div>
        </div>
      </div>

      {/* ═══ RIGHT — Login Form Panel ═══ */}
      <div className="flex flex-1 flex-col bg-[#F5F1E9] dark:bg-[#0A0A0A] transition-colors duration-300">
        {/* Mobile top hero header */}
        <div className="relative h-48 overflow-hidden lg:hidden">
          <img src={IMAGES.login.hero} alt="" className="absolute inset-0 h-full w-full object-cover opacity-65" />
          <div className="absolute inset-0 bg-gradient-to-b from-[#180E09]/75 via-[#180E09]/40 to-[#F5F1E9] dark:to-[#0A0A0A]" />
          <div className="relative z-10 flex items-center gap-3 p-6">
            <VlqmsLogo size="sm" showText={true} onDark={true} />
          </div>
        </div>

        {/* Form area */}
        <main className="flex flex-1 items-center justify-center px-6 py-8 lg:py-12">
          <div className="w-full max-w-sm">
            {/* Custom VLQMS Crest Logo & Welcome Headline */}
            <div className="mb-8 hidden flex-col items-center text-center lg:flex animate-fadeInUp">
              <div className="mb-4 p-[4px]">
                <VlqmsLogo size="lg" />
              </div>
              <h1 className="text-2xl font-bold tracking-tight text-[#29231F] dark:text-white"
                  style={{ fontFamily: 'var(--font-display)' }}>
                Welcome back
              </h1>
              <p className="mt-1 text-xs text-[#70675F] dark:text-stone-400">
                Sign in to vLQMS · {company?.name || 'Company profile'} · {company?.licence_type || 'Licence'}
              </p>
            </div>

            {/* Mobile heading */}
            <div className="mb-6 lg:hidden">
              <h1 className="text-xl font-bold text-[#29231F] dark:text-white"
                  style={{ fontFamily: 'var(--font-display)' }}>Welcome back</h1>
              <p className="mt-1 text-xs text-[#70675F] dark:text-stone-400">
                Sign in to vLQMS · {company?.name || 'Company profile'} · {company?.licence_type || 'Licence'}
              </p>
            </div>

            {/* Warm Frosted Glass Form Card */}
            <form
              onSubmit={submit}
              className="animate-fadeInUp animate-fadeInUp-d2
                         glass-card-login rounded-2xl p-7
                         border border-[#E3D9CD] dark:border-[#2A2A2A]
                         shadow-[0_20px_50px_rgba(72,53,38,0.08)] dark:shadow-card"
              noValidate
            >
              <div className="space-y-5">
                <div>
                  <label htmlFor="login-user" className={labelClass}>User name</label>
                  <input id="login-user" autoFocus value={user} onChange={e => setUser(e.target.value)}
                    className={`${inputClass} uppercase`} autoComplete="username" placeholder="Enter user name" />
                </div>
                <div>
                  <label htmlFor="login-pwd" className={labelClass}>Password</label>
                  <input id="login-pwd" type="password" value={password} onChange={e => setPassword(e.target.value)}
                    className={inputClass} autoComplete="current-password" placeholder="Enter password" aria-invalid={!!error} />
                </div>
                <div>
                  <label htmlFor="login-fy" className={labelClass}>Financial year</label>
                  <select id="login-fy" value={finYear} onChange={e => setFinYear(e.target.value)}
                    className={`${inputClass} tabular-nums`}>
                    {FIN_YEARS.map(y => <option key={y}>{y}</option>)}
                  </select>
                </div>
                {offerRemember && (
                  <label className="flex items-center gap-2 text-xs text-[#70675F] dark:text-stone-400">
                    <input type="checkbox" checked={remember} onChange={e => setRemember(e.target.checked)} />
                    Remember me on this device (until Sign out)
                  </label>
                )}
              </div>

              {error && (
                <p role="alert" className="mt-4 flex items-center gap-2 rounded-xl bg-[#9A4B45]/10 dark:bg-red-500/10 px-4 py-3 text-xs font-medium text-[#9A4B45] dark:text-red-400">
                  <AlertCircle size={14} className="shrink-0" />{error}
                </p>
              )}

              {/* ── HIGH-CONTRAST LOGIN BUTTON IN BOTH THEMES ── */}
              <button
                type="submit"
                disabled={busy}
                className="mt-6 flex w-full items-center justify-center gap-2
                           rounded-xl py-3.5 text-sm font-bold
                           transition-all duration-200 active:scale-[0.98]
                           disabled:pointer-events-none disabled:opacity-45
                           bg-[#64283A] text-white shadow-[0_4px_16px_rgba(100,40,58,0.25)] hover:bg-[#521E2E]
                           dark:bg-gradient-to-r dark:from-[#D4A847] dark:via-[#F7D98B] dark:to-[#C8952E]
                           dark:text-[#181106] dark:font-extrabold dark:shadow-[0_0_24px_rgba(212,168,71,0.55)]
                           dark:hover:shadow-[0_0_36px_rgba(212,168,71,0.75)] dark:hover:brightness-110"
              >
                {busy ? <Loader2 size={16} className="animate-spin" /> : <LogIn size={16} />}
                {busy ? 'Signing in…' : 'Sign in'}
              </button>

              <DevelopedBy className="mt-6 border-t border-[#E3D9CD] pt-5 dark:border-[#2A2A2A]" />
            </form>

            {/* Demo credential hint */}
            <div className="mt-6 animate-fadeInUp animate-fadeInUp-d4">
              <p className="text-center text-xs text-[#70675F] dark:text-stone-500">
                Demo access: <span className="font-semibold text-[#64283A] dark:text-[#D4A847]">ADMIN / admin</span>
              </p>
            </div>

            {/* Decorative wine icon */}
            <div className="mt-8 flex justify-center animate-float opacity-20 dark:opacity-15" aria-hidden="true">
              <Wine size={28} className="text-[#B08A45] dark:text-[#D4A847]" />
            </div>
          </div>
        </main>

        <footer className="pb-6 text-center text-[11px] text-[#958B81] dark:text-stone-600">
          Softvent vLQMS Cloud ERP · Licence 827106637
        </footer>
      </div>
    </div>
  );
}
