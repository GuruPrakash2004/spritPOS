// The central site's shell (central.html): sign-in, then Day summary / Stock / Purchase indents / Shops for
// the shops this user may see. The API is backend/app/central/main.py, reached at /api like the shop app's
// (Vite proxies it to :8002 in `npm run dev:central`). The token is kept for this tab only; a 401 returns to
// the sign-in page with the server's reason, and a temporary password must be changed before anything else.

import { useCallback, useEffect, useState } from 'react';
import { Boxes, ClipboardList, KeyRound, LayoutDashboard, LogOut, Moon, Store, Sun } from 'lucide-react';
import { SnackbarContainer } from '../components/common/Toast';
import { toastStore } from '../components/common/toastStore';
import VlqmsLogo from '../components/layout/VlqmsLogo';
import { centralMe, centralShops, errorText, getAuthToken, setAuthUser, setSessionEndedHandler } from '../lib/api';
import { iconButton } from '../lib/styles';
import ChangePassword from './ChangePassword';
import CentralLogin from './CentralLogin';
import Indents from './Indents';
import ShopDashboard from './ShopDashboard';
import StockLookup from './StockLookup';
import SyncStatus from './SyncStatus';
import type { CentralSession, CentralShop } from './types';

const TABS = [
  { id: 'dsr', label: 'Day summary', icon: LayoutDashboard },
  { id: 'stock', label: 'Stock', icon: Boxes },
  { id: 'indents', label: 'Purchase indents', icon: ClipboardList },
  { id: 'shops', label: 'Shops', icon: Store },
] as const;
type Tab = (typeof TABS)[number]['id'];

export default function CentralApp() {
  const [session, setSession] = useState<CentralSession | null>(null);
  const [restoring, setRestoring] = useState(!!getAuthToken());
  const [reason, setReason] = useState('');
  const [shops, setShops] = useState<CentralShop[]>([]);
  const [tab, setTab] = useState<Tab>('dsr');
  const [changing, setChanging] = useState(false);
  const [dark, setDark] = useState(false);

  const signIn = (s: CentralSession) => {
    setAuthUser(s.user, s.role, s.access_token);
    setSession(s);
    setReason('');
  };
  const signOut = useCallback((why = '') => {
    setAuthUser('', '', '');
    setSession(null);
    setShops([]);
    setReason(why);
  }, []);

  useEffect(() => {
    setSessionEndedHandler(why => signOut(why));
    return () => setSessionEndedHandler(null);
  }, [signOut]);

  useEffect(() => {   // after a refresh: the stored token's user
    if (!getAuthToken()) return;
    centralMe<CentralSession>().then(s => setSession(s)).catch(() => signOut()).finally(() => setRestoring(false));
  }, [signOut]);

  const loadShops = useCallback(() =>
    centralShops<CentralShop>().then(s => { setShops(s); return true; })
      .catch(e => { toastStore.add('error', errorText(e)); return false; }), []);
  const refreshShops = async () => {
    if (!(await toastStore.defer('refresh', 'The shop list will be refreshed when this closes.', 'Refresh cancelled.'))) return;
    if (await loadShops()) toastStore.add('refresh', 'Shop list refreshed.');
  };
  useEffect(() => { if (session && !session.must_change) loadShops(); }, [session, loadShops]);

  useEffect(() => {
    document.documentElement.classList.toggle('dark', dark);
    document.body.classList.toggle('dark', dark);
  }, [dark]);

  if (restoring) return <div className="flex min-h-screen items-center justify-center text-sm text-stone-500">Signing you back in…</div>;
  if (!session) return <><CentralLogin onSignedIn={signIn} reason={reason} /><SnackbarContainer /></>;

  return (
    <div className={`min-h-screen ${dark ? 'dark bg-[#0A0A0A] text-stone-100' : 'bg-[#F5F1E9] text-[#29231F]'}`}>
      <header className="sticky top-0 z-10 border-b border-[#E3D9CD] bg-[#FFFCF7]/95 backdrop-blur dark:border-[#1E1E1E] dark:bg-[#0E0E0E]/95">
        <div className="mx-auto flex max-w-7xl flex-wrap items-center gap-3 px-4 py-2">
          <VlqmsLogo size="xs" />
          <span className="text-sm font-bold">Central</span>
          {!session.must_change && (
            <nav className="flex flex-wrap gap-1" aria-label="Sections">
              {TABS.map(t => (
                <button key={t.id} onClick={() => setTab(t.id)} aria-current={tab === t.id ? 'page' : undefined}
                  className={`inline-flex items-center gap-1.5 rounded-lg px-2.5 py-1.5 text-xs font-medium ${tab === t.id
                    ? 'bg-[#64283A] text-white dark:bg-[#D4A847] dark:text-[#180E09]' : 'text-[#70675F] hover:bg-[#EFE9DF] dark:text-stone-400 dark:hover:bg-[#1C1C1C]'}`}>
                  <t.icon size={14} />{t.label}
                </button>
              ))}
            </nav>
          )}
          <div className="ml-auto flex items-center gap-1 text-xs">
            <span className="mr-1 text-[#70675F] dark:text-stone-400">{session.user} · {session.role}</span>
            <button className={iconButton} onClick={() => setChanging(true)} title="Change password" aria-label="Change password"><KeyRound size={15} /></button>
            <button className={iconButton} onClick={() => setDark(!dark)} title="Theme" aria-label={dark ? 'Switch to light theme' : 'Switch to dark theme'}>{dark ? <Sun size={15} /> : <Moon size={15} />}</button>
            <button className={iconButton} onClick={() => signOut()} title="Sign out" aria-label="Sign out"><LogOut size={15} /></button>
          </div>
        </div>
      </header>
      <main className="mx-auto max-w-7xl p-4">
        {session.must_change || changing ? (
          <ChangePassword user={session.user} temporary={session.must_change}
            onChanged={s => { signIn(s); setChanging(false); }}
            onCancel={() => (session.must_change ? signOut() : setChanging(false))} />
        ) : tab === 'dsr' ? <ShopDashboard shops={shops} />
          : tab === 'stock' ? <StockLookup shops={shops} />
          : tab === 'indents' ? <Indents shops={shops} />
          : <SyncStatus shops={shops} onRefresh={refreshShops} />}
      </main>
      <SnackbarContainer />
    </div>
  );
}
