// Sign-in page (legacy frmLogin). Posts to POST /auth/login and hands the returned user
// (role, modules, JWT) and the chosen financial year to App.jsx via onLogin.
// Has its own light/dark toggle and shows the company name from useCompanyProfile().
// On a new install (GET /setup/status: needs_setup) it shows the one-time Database setup instead of the form;
// when the database is set up but can't be reached it says so, with Retry.

import { useState, useEffect, } from 'react';
import { AlertCircle, Database, Eye, EyeOff, Loader2, LogIn, Moon, Sun, Wine } from 'lucide-react';
import { errorText, getSetupStatus, login, setRememberSession, } from '../../lib/api';
import { loadPasswordRules } from '../../lib/passwords';
import { encryptSecret } from '../../lib/secureField';
import { IMAGES } from '../../constants/images';
import VlqmsLogo from '../layout/VlqmsLogo';
import DevelopedBy from '../layout/DevelopedBy';
import DatabaseSetup from './DatabaseSetup';
import { useCompanyProfile } from '../../hooks/useCompanyProfile';
import CreateAdmin from './CreateAdmin';
import { finYearLabel } from '../../lib/formatters';

const RETRY_MS = 20_000;

// ─── REALISTIC WATER DROPLET FLOW & LIQUOR SPLASH DYNAMICS ───
// Simulates individual liquor water droplets cascading downwards with gravity acceleration
// and erupting in multi-bead splashes and concentric ripples upon hitting the roaming liquor surface.
// Height of the liquor surface above the panel's bottom edge (px): where drops land and splash.
const SURFACE = 96;

// Crown of splash beads: sideways reach, height and size in px (the middle one is the tall centre jet).
const SPLASH_BEADS = [
  { dx: -38, h: 20, size: 4 }, { dx: -20, h: 34, size: 5 }, { dx: 0, h: 46, size: 4 },
  { dx: 20, h: 34, size: 5 }, { dx: 38, h: 20, size: 4 },
];

const DROP_SIZE = 11; // width in px of every falling drop

// A whisky bottle tipped over at the top right, pouring a stream that arcs down into the liquor at the bottom
// (the 9 s cycle is in index.css). The mouth sits at (POUR_LEFT, POUR_TOP); the stream is as long as the way down to
// the liquor surface (cqh = panel height) and lands STREAM_DRIFT px to the left of the mouth.
const POUR_LEFT = '68%';
const POUR_TOP = 'max(26cqh, 215px)'; // keeps the whole tipped bottle inside the panel
const POUR_CYCLE = '9s';
const STREAM_DRIFT = 22;
const POUR_FALL = `calc(100cqh - ${POUR_TOP} - ${SURFACE}px)`;
const POUR_BEADS = [
  { dx: -30, h: 14, size: 3, delay: '0s' }, { dx: 26, h: 18, size: 4, delay: '0.22s' },
  { dx: -14, h: 28, size: 3, delay: '0.45s' }, { dx: 12, h: 12, size: 3, delay: '0.7s' },
  { dx: -22, h: 20, size: 2, delay: '0.9s' }, { dx: 32, h: 10, size: 2, delay: '0.35s' },
];

// The stream as a shape in a 100 x 100 box (the box is stretched down the way to the surface): the mouth at (80, 0),
// drifting left as it falls, like a parabola, and thinning from about 7 px to 3.5 px as gravity stretches it.
const STREAM = (() => {
  const left = [], right = [], mid = [];
  for (let i = 0; i <= 24; i++) {
    const t = i / 24;
    const x = 80 - STREAM_DRIFT * Math.pow(t, 0.7), y = 100 * (0.1 * t + 0.9 * t * t), w = 3.8 - 1.8 * t;
    left.push(`${(x - w).toFixed(1)},${y.toFixed(1)}`);
    right.push(`${(x + w).toFixed(1)},${y.toFixed(1)}`);
    mid.push(`${(x - w * 0.35).toFixed(1)},${y.toFixed(1)}`);
  }
  return { body: `M${left.join(' L')} L${right.reverse().join(' L')} Z`, shine: `M${mid.join(' L')}` };
})();

// Outline of the bottle (mouth at the top centre) and the same shape pulled in a little for the liquor.
const BOTTLE_OUTER = 'M33.5 5H46.5V50C46.5 66 70 72 70 104V212Q70 224 58 224H22Q10 224 10 212V104C10 72 33.5 66 33.5 50Z';
const BOTTLE_INNER = 'M35.5 6H44.5V50C44.5 68 66 74 66 104V210Q66 220 57 220H23Q14 220 14 210V104C14 74 35.5 68 35.5 50Z';
const LEVEL_FRAME = { transformBox: 'view-box', transformOrigin: '40px 0px' };

const PouringBottle = () => (
  <>
    <div className="absolute" style={{ left: POUR_LEFT, top: POUR_TOP }}>
      {/* Bottle: drawn upright with the mouth at the top, then turned about the mouth so it hangs tipped over. */}
      <svg width="80" height="230" viewBox="0 0 80 230" className="absolute overflow-visible"
        style={{ left: '-40px', top: '-2px', transformOrigin: '50% 0', animation: `pourBottleTilt ${POUR_CYCLE} infinite`,
          filter: 'drop-shadow(0 8px 10px rgba(0,0,0,0.55))' }}>
        <defs>
          <clipPath id="pour-inner"><path d={BOTTLE_INNER} /></clipPath>
          <linearGradient id="pour-glass" x1="0" x2="1">
            <stop offset="0" stopColor="#1B120B" stopOpacity="0.85" /><stop offset="0.5" stopColor="#3A2412" stopOpacity="0.55" />
            <stop offset="1" stopColor="#120B06" stopOpacity="0.9" />
          </linearGradient>
          <linearGradient id="pour-liquor" x1="0" x2="1">
            <stop offset="0" stopColor="#8A4A12" /><stop offset="0.4" stopColor="#E0922C" /><stop offset="1" stopColor="#7A3E0C" />
          </linearGradient>
          <linearGradient id="pour-volume" x1="0" x2="1">
            <stop offset="0" stopColor="#000" stopOpacity="0.55" /><stop offset="0.22" stopColor="#000" stopOpacity="0" />
            <stop offset="0.78" stopColor="#000" stopOpacity="0" /><stop offset="1" stopColor="#000" stopOpacity="0.6" />
          </linearGradient>
          <linearGradient id="pour-paper" x1="0" x2="1">
            <stop offset="0" stopColor="#CDBF9F" /><stop offset="0.35" stopColor="#F7EDD4" /><stop offset="1" stopColor="#B9A985" />
          </linearGradient>
          <linearGradient id="pour-lip" x1="0" x2="1">
            <stop offset="0" stopColor="#8F8A80" /><stop offset="0.5" stopColor="#F2EFE6" /><stop offset="1" stopColor="#7C776D" />
          </linearGradient>
        </defs>

        {/* glass body */}
        <path d={BOTTLE_OUTER} fill="url(#pour-glass)" />
        {/* liquor, kept level while the bottle turns */}
        <g clipPath="url(#pour-inner)">
          <g style={{ ...LEVEL_FRAME, animation: `pourLiquidTurn ${POUR_CYCLE} infinite` }}>
            <g style={{ animation: `pourLevel ${POUR_CYCLE} infinite` }}>
              <rect x="-260" y="0" width="600" height="600" fill="url(#pour-liquor)" transform="translate(0 0)" style={{ fill: '#C8761A' }} />
              <rect x="-260" y="-1.5" width="600" height="3" fill="#FFE2A0" fillOpacity="0.9" />
            </g>
          </g>
          {/* roundness of the glass over the liquor */}
          <path d={BOTTLE_INNER} fill="url(#pour-volume)" />
        </g>
        {/* paper label with a gold seal */}
        <g>
          <rect x="11" y="128" width="58" height="64" fill="url(#pour-paper)" />
          <rect x="15" y="132" width="50" height="56" fill="none" stroke="#8A5A1A" strokeWidth="0.8" />
          <text x="40" y="153" textAnchor="middle" fontSize="12" fontWeight="700" fill="#4A2208" fontFamily="Georgia, serif" letterSpacing="1">SPIRIT</text>
          <text x="40" y="164" textAnchor="middle" fontSize="5" fill="#6B4A22" fontFamily="Georgia, serif" letterSpacing="1.5">RESERVE · 12 YEARS</text>
          <circle cx="40" cy="176" r="5" fill="#D4A847" stroke="#8A5A1A" strokeWidth="0.6" />
          <rect x="11" y="128" width="58" height="64" fill="url(#pour-volume)" />
        </g>
        {/* glass edge, thickness and reflections */}
        <path d={BOTTLE_OUTER} fill="none" stroke="#F5E3B8" strokeOpacity="0.5" strokeWidth="1.4" />
        <path d={BOTTLE_INNER} fill="none" stroke="#000" strokeOpacity="0.25" strokeWidth="1" />
        <path d="M18 108V204" stroke="#fff" strokeOpacity="0.38" strokeWidth="4" strokeLinecap="round" />
        <path d="M62 112V198" stroke="#fff" strokeOpacity="0.16" strokeWidth="2" strokeLinecap="round" />
        <path d="M37.5 10V46" stroke="#fff" strokeOpacity="0.4" strokeWidth="1.6" strokeLinecap="round" />
        {/* the open mouth */}
        <rect x="31.5" y="0" width="17" height="7" rx="2.5" fill="url(#pour-lip)" />
        <rect x="31.5" y="5.2" width="17" height="1.6" fill="#000" fillOpacity="0.25" />
      </svg>

      {/* The stream: arcs out of the mouth, tapers, and surges a little as the bottle glugs */}
      <div className="absolute" style={{ left: '-80px', top: '3px', width: '100px', height: POUR_FALL,
        transformOrigin: '80% 0', animation: `pourStream ${POUR_CYCLE} infinite` }}>
        <div className="h-full w-full" style={{ transformOrigin: '80% 0', animation: 'pourGlug 0.7s ease-in-out infinite' }}>
          <svg width="100" height="100%" viewBox="0 0 100 100" preserveAspectRatio="none" className="overflow-visible"
            style={{ filter: 'drop-shadow(0 0 5px rgba(245,185,60,0.65))' }}>
            <defs>
              <linearGradient id="pour-stream" x1="0" x2="1">
                <stop offset="0" stopColor="#B26A1C" /><stop offset="0.5" stopColor="#F3B650" /><stop offset="1" stopColor="#C27820" />
              </linearGradient>
            </defs>
            <path d={STREAM.body} fill="url(#pour-stream)" fillOpacity="0.95" />
            <path d={STREAM.shine} fill="none" stroke="#FFF3CC" strokeOpacity="0.7" strokeWidth="1.1" strokeLinecap="round" />
          </svg>
        </div>
      </div>

      {/* A last drip after the stream stops */}
      <div className="absolute rounded-full"
        style={{ left: '-2px', top: '6px', width: '5px', height: '8px', '--fall': POUR_FALL, opacity: 0,
          borderRadius: '50% 50% 50% 50% / 35% 35% 65% 65%',
          background: 'radial-gradient(ellipse at 35% 25%, #FFF5D6, #F5D580 40%, #D48030 80%)',
          animation: `pourDrip ${POUR_CYCLE} infinite` }} />
    </div>

    {/* Where the stream lands: a heaving mound, rings and beads, on only while it is pouring */}
    <div className="absolute" style={{ left: `calc(${POUR_LEFT} - ${STREAM_DRIFT}px)`, bottom: `${SURFACE}px`, animation: `pourSplashOn ${POUR_CYCLE} infinite` }}>
      <div className="absolute rounded-full"
        style={{ left: '-9px', top: '-5px', width: '18px', height: '9px', background: 'radial-gradient(ellipse at 50% 70%, #F3B650, #C27820)',
          boxShadow: '0 0 8px rgba(245,185,60,0.8)', animation: 'pourMound 0.45s ease-in-out infinite' }} />
      {['0s', '0.5s'].map(d => (
        <div key={d} className="absolute rounded-full border border-amber-300/80"
          style={{ left: '-36px', top: '-6px', width: '72px', height: '12px', animation: `pourRipple 1s ease-out ${d} infinite` }} />
      ))}
      {POUR_BEADS.map((b, i) => (
        <div key={i} className="absolute rounded-full bg-amber-200 shadow-[0_0_6px_rgba(255,215,100,0.9)]"
          style={{ left: `-${b.size / 2}px`, top: `-${b.size / 2}px`, width: `${b.size}px`, height: `${b.size}px`,
            '--dx': `${b.dx}px`, '--h': `${b.h}px`, animation: `pourBead 1.1s ease-out ${b.delay} infinite` }} />
      ))}
    </div>
  </>
);

// The liquor's three layers, back to front: crest outline (from the curve each used to fill), height class, view box
// height, colours at the surface and at depth, and how strong the crest line and its glow are.
const WAVE_LAYERS = [
  { h: 'h-40', box: 160, crest: 'M0,60 C240,110 480,20 720,65 C960,110 1200,25 1440,60', top: '#C97B22', topA: 0.8, bottom: '#4E2508',
    glow: 0.15, line: 0.45, animation: 'roamingLiquidSlosh1 11s ease-in-out infinite alternate' },
  { h: 'h-36', box: 140, crest: 'M0,50 C220,15 460,95 700,45 C940,5 1180,85 1440,50', top: '#E8A63C', topA: 0.85, bottom: '#7A3E0C',
    glow: 0.2, line: 0.6, animation: 'roamingLiquidSlosh2 7.5s ease-in-out infinite alternate' },
  { h: 'h-32', box: 120, crest: 'M0,35 C180,65 420,10 680,40 C940,65 1180,15 1440,35', top: '#F8CB68', topA: 0.9, bottom: '#B8701C',
    glow: 0.3, line: 0.9, animation: 'roamingLiquidSlosh3 5.2s ease-in-out infinite alternate' },
];

const LiquidDropletWaterfall = () => {
  // All drops are one size; only the column, the fall time and the start delay differ.
  const drops = [
    { left: '9%',  duration: '3.6s', delay: '0.6s' },
    { left: '18%', duration: '3.4s', delay: '0.0s' },
    { left: '27%', duration: '3.9s', delay: '1.9s' },
    { left: '36%', duration: '3.8s', delay: '1.1s' },
    { left: '45%', duration: '3.5s', delay: '1.4s' },
    { left: '54%', duration: '3.3s', delay: '0.5s' },
    { left: '63%', duration: '3.7s', delay: '0.3s' },
    { left: '72%', duration: '3.9s', delay: '1.6s' },
    { left: '80%', duration: '3.6s', delay: '2.0s' },
    { left: '88%', duration: '3.5s', delay: '0.8s' },
    { left: '94%', duration: '3.8s', delay: '1.2s' },
  ];

  return (
    <div className="pointer-events-none absolute inset-0 overflow-hidden bottle-container-glass" style={{ containerType: 'size' }} aria-hidden="true">
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

        {/* Three layers of liquor, back to front. Each is lit from above (lighter at the surface, darker with depth),
            has a bright crest line and a soft glow along it, and slides at its own pace so the crests never line up. */}
        {WAVE_LAYERS.map((w, i) => (
          <svg
            key={i}
            className={`absolute bottom-0 w-[200%] ${w.h}`}
            style={{ animation: w.animation }}
            viewBox={`0 0 1440 ${w.box}`}
            preserveAspectRatio="none"
          >
            <defs>
              <linearGradient id={`wave-body-${i}`} x1="0" x2="0" y1="0" y2="1">
                <stop offset="0" stopColor={w.top} stopOpacity={w.topA} />
                <stop offset="1" stopColor={w.bottom} stopOpacity="0.96" />
              </linearGradient>
            </defs>
            <path d={`${w.crest} L1440,${w.box} L0,${w.box} Z`} fill={`url(#wave-body-${i})`} />
            <path d={w.crest} fill="none" stroke="#FFD98A" strokeOpacity={w.glow} strokeWidth="7" vectorEffect="non-scaling-stroke" style={{ filter: 'blur(3px)' }} />
            <path d={w.crest} fill="none" stroke="#FFF3CF" strokeOpacity={w.line} strokeWidth="1.6" strokeLinecap="round" vectorEffect="non-scaling-stroke" />
          </svg>
        ))}

        {/* Light bending through the liquor: slow diagonal shafts that fade with depth */}
        <div className="absolute inset-x-0 bottom-0 h-28 overflow-hidden"
          style={{ maskImage: 'linear-gradient(to bottom, #000 10%, transparent 90%)', WebkitMaskImage: 'linear-gradient(to bottom, #000 10%, transparent 90%)' }}>
          <div className="absolute inset-y-0 -left-24 w-[calc(100%+12rem)]"
            style={{
              background: 'repeating-linear-gradient(108deg, rgba(255,238,180,0) 0 46px, rgba(255,238,180,0.17) 70px, rgba(255,238,180,0) 100px)',
              animation: 'causticDrift 9s linear infinite',
            }} />
        </div>
      </div>

      {/* ── 1. Cascading Droplets with Splash Impact ── */}
      {drops.map((drop, idx) => (
        <div key={`water-drop-${idx}`} className="absolute top-0 bottom-0" style={{ left: drop.left }}>
          {/* A falling drop: pointed at the top, round at the bottom, with a dark rim, a lit body and a bright highlight */}
          <svg
            width={DROP_SIZE}
            height={DROP_SIZE * 1.6}
            viewBox="0 0 10 16"
            className="absolute -top-4 overflow-visible"
            style={{
              left: `-${DROP_SIZE / 2}px`,
              filter: 'drop-shadow(0 0 5px rgba(245,185,60,0.7))',
              transformOrigin: '50% 0',
              animation: `dropletFallAccelerate ${drop.duration} cubic-bezier(0.55, 0.085, 0.68, 0.53) ${drop.delay} infinite`,
            }}
          >
            <defs>
              <radialGradient id="drop-body" cx="0.38" cy="0.62" r="0.7">
                <stop offset="0" stopColor="#FFE9A8" /><stop offset="0.5" stopColor="#E9A23A" /><stop offset="1" stopColor="#7A3A0C" />
              </radialGradient>
            </defs>
            <path d="M5 0C5.6 3 9.4 6.4 9.4 10.2A4.4 4.4 0 0 1 0.6 10.2C0.6 6.4 4.4 3 5 0Z" fill="url(#drop-body)" stroke="#5A2A08" strokeOpacity="0.55" strokeWidth="0.5" />
            <ellipse cx="3.4" cy="10" rx="1.1" ry="2" fill="#fff" fillOpacity="0.85" transform="rotate(12 3.4 10)" />
            <circle cx="6.9" cy="12.6" r="0.6" fill="#fff" fillOpacity="0.5" />
          </svg>

          {/* Splash Impact Center at liquid line (around 82% height) */}
          <div
            className="absolute"
            style={{
              bottom: `${SURFACE}px`,
              left: '0px',
              width: '1px',
              height: '1px',
            }}
          >
            {/* Two flat rings spreading out from the impact point */}
            {[['dropletImpactRipple', 'border-amber-300/80 dark:border-amber-400'], ['dropletImpactRipple2', 'border-amber-200/60 dark:border-amber-300/80']].map(([name, cls]) => (
              <div
                key={name}
                className={`absolute rounded-full border ${cls}`}
                style={{
                  left: '-34px', top: '-6px', width: '68px', height: '12px',
                  animation: `${name} ${drop.duration} cubic-bezier(0.2, 0.6, 0.4, 1) ${drop.delay} infinite`,
                }}
              />
            ))}

            {/* The thin jet that rises from the middle of the splash just after impact */}
            <div
              className="absolute"
              style={{
                left: '-2px', top: '-26px', width: '4px', height: '26px', transformOrigin: '50% 100%',
                borderRadius: '50% 50% 20% 20% / 45% 45% 10% 10%',
                background: 'linear-gradient(90deg, #C27820, #FFE29A 50%, #C27820)',
                boxShadow: '0 0 6px rgba(245,185,60,0.7)',
                animation: `splashJet ${drop.duration} ease-out ${drop.delay} infinite`,
              }}
            />

            {/* A crown of beads thrown up in arcs (--dx sideways, --h height) */}
            {SPLASH_BEADS.map((b, i) => (
              <div
                key={i}
                className="absolute rounded-full bg-amber-200 dark:bg-amber-300 shadow-[0_0_6px_rgba(255,215,100,0.9)]"
                style={{
                  left: `-${b.size / 2}px`, top: `-${b.size / 2}px`, width: `${b.size}px`, height: `${b.size}px`,
                  '--dx': `${b.dx}px`, '--h': `${b.h}px`,
                  animation: `splashArc ${drop.duration} linear ${drop.delay} infinite`,
                }}
              />
            ))}
          </div>
        </div>
      ))}

      <PouringBottle />

      {/* ── 3. Glass reflections along container edges ── */}
      <div className="absolute inset-y-0 left-0 w-8 bg-gradient-to-r from-white/12 to-transparent pointer-events-none" />
      <div className="absolute inset-y-0 right-0 w-8 bg-gradient-to-l from-white/10 to-transparent pointer-events-none" />
    </div>
  );
};

export default function Login({ onLogin, notice = '' }) {
  const { company } = useCompanyProfile();
  const [user, setUser] = useState('');
  const [password, setPassword] = useState('');
  const [showPwd, setShowPwd] = useState(false);
  // The shop's financial years (newest first), the active one chosen.
  const finYears = (company?.fin_years?.length ? company.fin_years : company?.fy_start ? [{ start: company.fy_start, end: company.fy_end, active: true }] : [])
    .map(y => ({ label: finYearLabel(y.start, y.end), active: y.active, start: y.start, end: y.end })).filter(y => y.label);
  // No choice is offered at sign-in: the year that contains today, else the active one. Only an Administrator
  // changes the years (Company Master → Financial Year(s)).
  const todayDay = new Date().toLocaleDateString('en-CA');
  const finYear = (finYears.find(y => y.start <= todayDay && todayDay <= y.end) || finYears.find(y => y.active) || finYears[0])?.label || '';
  // Login Settings: offer "Remember me" only when the Administrator allows it.
  const [offerRemember, setOfferRemember] = useState(false);
  const [remember, setRemember] = useState(false);
  useEffect(() => { loadPasswordRules().then(r => setOfferRemember(r.show_remember_me)); }, []);
  // One-time Database setup / database offline (backend routers/setup.py).
  const [dbState, setDbState] = useState(null);
  const loadDbState = () => getSetupStatus().then(setDbState).catch(() => setDbState(null));
  useEffect(() => { loadDbState(); }, []);
  const [editingDb, setEditingDb] = useState(false);
  const setup = !!dbState?.needs_setup;
  const offline = !!dbState && dbState.configured && !dbState.connected && !setup;
  const needsAdmin = !!dbState?.needs_admin && !setup && !offline;
  // While the database is offline there is nobody to sign in: check again every 20 s.
  useEffect(() => {
    if (!offline) return;
    const timer = window.setInterval(loadDbState, RETRY_MS);
    return () => window.clearInterval(timer);
  }, [offline]);
  // Starts with the reason the last session ended, if any (see App.jsx).
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

  const submit = (e) => {
    e.preventDefault();
    if (!user.trim() || !password) return setError('Enter user name and password.');
    setBusy(true);
    setRememberSession(offerRemember && remember);
    // The password leaves the browser only encrypted (lib/secureField.js).
    encryptSecret(password, 'login')
      .then(sealed => login({ user, password: sealed }))
      .then(u => onLogin(u, finYear))
      .catch(err => setError(errorText(err)))
      .finally(() => setBusy(false));
  };

  const inputClass = `w-full rounded-xl border px-4 py-3 text-sm outline-none transition duration-150
    border-[#D5DCE7] bg-[#FFFFFF] text-[#1A2236] placeholder:text-[#9AA5B8]
    hover:border-[#CBBEAF]
    focus:border-[#1B2B4B] focus:ring-[3px] focus:ring-[#1B2B4B]/10
    dark:border-[#2A2A2A] dark:bg-[#1A1A1A] dark:text-stone-50
    dark:placeholder:text-stone-600 dark:hover:border-[#3A3A3A]
    dark:focus:border-[#D4A847] dark:focus:ring-[#D4A847]/20`;

  const labelClass = 'mb-2 block text-[11px] font-semibold uppercase tracking-[0.08em] text-[#5F6B80] dark:text-stone-400';

  // Sign-in form fields shrink with the window height so the page never scrolls.
  const fitInput = inputClass.replace('py-3', 'py-[clamp(0.4rem,1.4dvh,0.75rem)]');
  const fitLabel = labelClass.replace('mb-2', 'mb-[clamp(0.125rem,0.8dvh,0.5rem)]');

  return (
    <div className="flex h-dvh overflow-hidden font-sans bg-[#F4F6FA] dark:bg-[#0A0A0A] text-[#1A2236] dark:text-stone-100 transition-colors duration-250">
      
      {/* â•â•â• TOP-RIGHT THEME SWITCHER â•â•â• */}
      <div className="fixed top-5 right-5 z-50">
        <button
          type="button"
          onClick={() => setDark(!dark)}
          aria-label="Toggle theme"
          className="inline-flex h-10 w-10 items-center justify-center rounded-xl border
                     bg-[#FFFFFF]/90 border-[#DDE3EC] text-[#5F6B80] shadow-md
                     hover:text-[#1B2B4B] hover:border-[#1B2B4B]/30
                     dark:bg-[#1C1C1C]/90 dark:border-[#2A2A2A] dark:text-[#D4A847] dark:hover:bg-[#252525]
                     backdrop-blur-md transition active:scale-95"
          title={dark ? 'Switch to Heritage Light' : 'Switch to Dark Luxury'}
        >
          {dark ? <Sun size={18} /> : <Moon size={18} />}
        </button>
      </div>

      {/* â•â•â• LEFT — Cinematic Image Panel (Transparent Bottle Container) â•â•â• */}
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
        <div className="relative z-10 flex flex-col justify-between p-8 xl:p-12 pb-52 xl:pb-60 w-full">
          {/* Top — Custom VLQMS Emblem Logo */}
          <div className="flex items-center gap-3 p-[4px] animate-fadeInUp">
            <VlqmsLogo size="lg" onDark />
          </div>

          {/* Bottom — Hero Typography & Metric Badges */}
          <div className="max-w-[min(32rem,100%)] mb-4">
            <p className="animate-fadeInUp animate-fadeInUp-d1 text-[11px] font-bold tracking-[0.35em] text-[#F0CF8A] uppercase">
              Enterprise Resource Planning
            </p>
            <h1 className="animate-fadeInUp animate-fadeInUp-d2 mt-3 font-extrabold text-white leading-[1.1]"
                style={{ fontFamily: 'var(--font-display)', fontSize: 'clamp(1.5rem, 2.6vw, 2.75rem)' }}>
              Fast Billing. Accurate Stock.{' '}
              Better Control.
            </h1>
            <p className="animate-fadeInUp animate-fadeInUp-d3 mt-3 text-xs xl:text-sm leading-relaxed text-stone-200/90 max-w-md">
              Monitor inventory, sales, procurement and compliance across your entire
              distillery & retail operation — from warehouse to countertop.
            </p>


          </div>
        </div>
      </div>

      {/* â•â•â• RIGHT — Login Form Panel â•â•â• */}
      <div className="flex min-h-0 flex-1 flex-col bg-[#F4F6FA] dark:bg-[#0A0A0A] transition-colors duration-250">
        {/* Mobile top hero header */}
        <div className="relative h-[clamp(4.5rem,16dvh,12rem)] shrink-0 overflow-hidden lg:hidden">
          <img src={IMAGES.login.hero} alt="" className="absolute inset-0 h-full w-full object-cover opacity-65" />
          <div className="absolute inset-0 bg-gradient-to-b from-[#180E09]/75 via-[#180E09]/40 to-[#F4F6FA] dark:to-[#0A0A0A]" />
          <div className="relative z-10 flex items-center gap-3 p-6">
            <VlqmsLogo size="md" onDark />
          </div>
        </div>

        {/* Form area */}
        <main className="flex min-h-0 flex-1 items-center justify-center overflow-y-auto px-6 py-[clamp(0.5rem,3dvh,3rem)]">
          <div className="w-full max-w-sm">
            {/* Custom VLQMS Crest Logo & Welcome Headline */}
            <div className="mb-[clamp(0.5rem,3dvh,2rem)] hidden flex-col items-center text-center lg:flex animate-fadeInUp">
              <h1 className="text-2xl font-bold tracking-tight text-[#1A2236] dark:text-white"
                  style={{ fontFamily: 'var(--font-display)' }}>
                {setup ? 'Set up SpiritPOS' : 'Welcome back'}
              </h1>
              <p className="mt-1 text-xs text-[#5F6B80] dark:text-stone-400">
                {setup ? 'Connect this shop\'s database once, then sign in.' : `Sign in to SpiritPOS · ${company?.name || 'Company profile'}`}
              </p>
            </div>

            {/* Mobile heading */}
            <div className="mb-[clamp(0.25rem,2dvh,1.5rem)] lg:hidden">
              <h1 className="text-xl font-bold text-[#1A2236] dark:text-white"
                  style={{ fontFamily: 'var(--font-display)' }}>Welcome back</h1>
              <p className="mt-1 text-xs text-[#5F6B80] dark:text-stone-400">
                Sign in to SpiritPOS · {company?.name || 'Company profile'}
              </p>
            </div>

            {setup && <DatabaseSetup inputClass={inputClass} labelClass={labelClass} onDone={loadDbState} />}
            {/* Database and report-files settings: always on offer (open to anyone, no Administrator needed). */}
            {!setup && editingDb && (
              <div className="space-y-4">
                <DatabaseSetup inputClass={inputClass} labelClass={labelClass} change onDone={() => { setEditingDb(false); window.location.reload(); }} onCancel={() => setEditingDb(false)} />   {/* another database: the page starts again, so nothing of the old one stays in the browser */}
                <button type="button" onClick={() => setEditingDb(false)} className="w-full rounded-xl border border-[#D5DCE7] py-2.5 text-xs font-semibold dark:border-[#2A2A2A]">Back to sign in</button>
              </div>
            )}
            {offline && !editingDb && (
              <p role="alert" className="mb-4 flex items-center gap-2 rounded-xl bg-[#9A4B45]/10 px-4 py-3 text-xs font-medium text-[#9A4B45] dark:bg-red-500/10 dark:text-red-400">
                <AlertCircle size={14} className="shrink-0" />Database offline: the SQL Server can't be reached. Sign-in comes back when it does (checked every 20 seconds).
                <button type="button" onClick={loadDbState} className="ml-auto font-semibold underline">Retry</button>
                <button type="button" onClick={() => setEditingDb(true)} className="font-semibold underline">Database settings</button>
              </p>
            )}

            {needsAdmin && <CreateAdmin inputClass={inputClass} labelClass={labelClass} onDone={loadDbState} />}

            {/* Warm Frosted Glass Form Card */}
            {!setup && !offline && !needsAdmin && !editingDb && <form
              onSubmit={submit}
              className="animate-fadeInUp animate-fadeInUp-d2
                         glass-card-login rounded-2xl p-[clamp(1rem,3.2dvh,1.75rem)]
                         border border-[#DDE3EC] dark:border-[#2A2A2A]
                         shadow-[0_20px_50px_rgba(72,53,38,0.08)] dark:shadow-card"
              noValidate
            >
              <div className="space-y-[clamp(0.5rem,2dvh,1.25rem)]">
                <div>
                  <label htmlFor="login-user" className={fitLabel}>User name</label>
                  <input id="login-user" autoFocus value={user} onChange={e => setUser(e.target.value)}
                    className={`${fitInput} uppercase`} autoComplete="username" placeholder="Enter user name" />
                </div>
                <div>
                  <label htmlFor="login-pwd" className={fitLabel}>Password</label>
                  <div className="relative">
                    <input id="login-pwd" type={showPwd ? 'text' : 'password'} value={password} onChange={e => setPassword(e.target.value)}
                      className={`${fitInput} pr-11`} autoComplete="current-password" placeholder="Enter password" aria-invalid={!!error} />
                    <button type="button" onClick={() => setShowPwd(v => !v)} tabIndex={-1}
                      aria-label={showPwd ? 'Hide password' : 'Show password'} title={showPwd ? 'Hide password' : 'Show password'}
                      className="absolute inset-y-0 right-0 flex w-11 items-center justify-center text-[#5F6B80] hover:text-[#1B2B4B] dark:text-stone-400 dark:hover:text-[#D4A847]">
                      {showPwd ? <EyeOff size={16} /> : <Eye size={16} />}
                    </button>
                  </div>
                </div>
                {offerRemember && (
                  <label className="flex items-center gap-2 text-xs text-[#5F6B80] dark:text-stone-400">
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
                className="mt-[clamp(0.75rem,2.5dvh,1.5rem)] flex w-full items-center justify-center gap-2
                           rounded-xl py-[clamp(0.5rem,1.6dvh,0.875rem)] text-sm font-bold
                           transition duration-150 active:scale-[0.98]
                           disabled:pointer-events-none disabled:opacity-45
                           bg-[#1B2B4B] text-white shadow-[0_4px_16px_rgba(100,40,58,0.25)] hover:bg-[#121D36]
                           dark:bg-gradient-to-r dark:from-[#D4A847] dark:via-[#F7D98B] dark:to-[#C8952E]
                           dark:text-[#181106] dark:font-extrabold dark:shadow-[0_0_24px_rgba(212,168,71,0.55)]
                           dark:hover:shadow-[0_0_36px_rgba(212,168,71,0.75)] dark:hover:brightness-110"
              >
                {busy ? <Loader2 size={16} className="animate-spin" /> : <LogIn size={16} />}
                {busy ? 'Signing in…' : 'Sign in'}
              </button>

              <DevelopedBy className="mt-[clamp(0.5rem,2dvh,1.5rem)] border-t border-[#DDE3EC] pt-[clamp(0.5rem,1.6dvh,1.25rem)] dark:border-[#2A2A2A]" />
            </form>}

            {!setup && !editingDb && (
              <button type="button" onClick={() => setEditingDb(true)}
                className="mt-4 flex w-full items-center justify-center gap-2 text-xs font-semibold text-[#1B2B4B] underline-offset-2 hover:underline dark:text-[#D4A847]">
                <Database size={13} />Database settings
              </button>
            )}

            {/* Decorative wine icon */}
            <div className="mt-6 flex justify-center animate-float [@media(max-height:760px)]:hidden opacity-20 dark:opacity-15" aria-hidden="true">
              <Wine size={28} className="text-[#D99A2B] dark:text-[#D4A847]" />
            </div>
          </div>
        </main>

        <footer className="shrink-0 pb-[clamp(0.25rem,1.5dvh,1.5rem)] text-center text-[11px] text-[#8592A6] dark:text-stone-600">
          Softvent SpiritPOS
        </footer>
      </div>
    </div>
  );
}
