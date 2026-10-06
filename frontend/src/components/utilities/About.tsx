// About (legacy frmUtlAbout, at the root of the desktop menu): product, software version (Global Settings →
// SWVersion), API version, company, database connection, which data the screens show (live SQL Server or the Test
// data switch's sample data) and the "Designed & developed by" credit. An Administrator can Disconnect the database
// (to test the app without it; the settings are kept) and Reconnect it.
// No licence key or registration (Key Generation is not part of the web app).

import { useEffect, useState } from 'react';
import { Database, Info, PlugZap, RefreshCw, Unplug } from 'lucide-react';
import { errorText, getAbout, getAuthRole, refreshDocDesigner, setDatabaseConnection, type ApiAbout } from '../../lib/api';
import { ADMIN_ROLE } from '../../lib/modules';
import { badge, pageSub, pageTitle, panel, secondaryButton } from '../../lib/styles';
import { toastStore } from '../common/toastStore';
import DevelopedBy from '../layout/DevelopedBy';
import VlqmsLogo from '../layout/VlqmsLogo';

export default function About() {
  const [about, setAbout] = useState<ApiAbout | null>(null);
  const [error, setError] = useState('');

  const [busy, setBusy] = useState(false);
  const isAdmin = getAuthRole() === ADMIN_ROLE;

  useEffect(() => { getAbout().then(setAbout).catch(e => setError(errorText(e))); }, []);

  // Disconnect / Reconnect the database (testing). Both wait for their popup, as every action does.
  const connection = async (action: 'disconnect' | 'reconnect') => {
    const text = action === 'disconnect'
      ? 'The database will be disconnected when this closes. Nothing can be saved and nobody new can sign in until Reconnect.'
      : 'The database will be reconnected when this closes, and the shop\'s data loaded again.';
    if (!(await toastStore.defer(action === 'disconnect' ? 'delete' : 'refresh', text, 'Cancelled. The connection was not changed.'))) return;
    setBusy(true);
    try {
      const next = await setDatabaseConnection(action);
      setAbout(next);
      toastStore.add(action === 'disconnect' ? 'ok' : 'refresh', action === 'disconnect' ? 'Database disconnected.' : 'Database reconnected.');
    } catch (e) {
      toastStore.add('error', errorText(e));
    } finally {
      setBusy(false);
    }
  };

  // Read the Doc Designer's latest print options and screens (it writes them into the database, not into this app).
  const refreshDesigner = async () => {
    if (!(await toastStore.defer('refresh', 'The Doc Designer settings will be read again when this closes.', 'Cancelled. Nothing was refreshed.'))) return;
    setBusy(true);
    try {
      await refreshDocDesigner();
      setAbout(await getAbout());
      toastStore.add('refresh', 'Doc Designer settings refreshed.');
    } catch (e) {
      toastStore.add('error', errorText(e));
    } finally {
      setBusy(false);
    }
  };

  const rows: [string, string][] = about ? [
    ['Software version', about.software_version],
    ['API version', about.api_version],
    ['Company', about.company || '—'],
    ['Doc Designer', `${about.designer?.screens ?? 0} screens · ${about.designer?.print_options ?? 0} print options · ${about.designer?.fields ?? 0} fields`],
    ['Printing (Crystal)', about.rpt_bridge ? `Bridge set up · ${about.rpt_files} .rpt files` : 'Bridge not set up (RPT_BRIDGE_URL)'],
    ['Server time', new Date(about.server_time).toLocaleString('en-IN')],
  ] : [];

  return (
    <div className="mx-auto flex max-w-xl flex-col gap-4">
      <section className={`${panel} flex flex-col items-center gap-4 p-8 text-center`}>
        <VlqmsLogo size="lg" />
        <div>
          <h1 className={pageTitle}>{about?.product ?? 'e-vLQMS ERP'}</h1>
          <p className={pageSub}>Liquor store management: masters, purchase, sales, stock and reports.</p>
        </div>
        {error && <p role="alert" className="text-xs text-[#9A4B45] dark:text-red-400">{error}</p>}
        {about && (
          <>
            <dl className="grid w-full grid-cols-2 gap-x-4 gap-y-2 text-left text-xs">
              {rows.map(([k, v]) => (
                <div key={k} className="contents">
                  <dt className="text-stone-500">{k}</dt>
                  <dd className="font-medium tabular-nums text-stone-800 dark:text-stone-100">{v}</dd>
                </div>
              ))}
            </dl>
            <div className="flex flex-wrap justify-center gap-2">
              <span className={badge(about.db_connected ? 'success' : 'neutral')}>
                <Database size={12} />{about.db_connected ? 'SQL Server connected'
                  : about.db_disconnected_for_testing ? 'SQL Server disconnected (for testing)'
                  : about.db_configured ? 'SQL Server offline' : 'No database set up'}
              </span>
            </div>
            {isAdmin && about.db_connected && (
              <button className={secondaryButton} disabled={busy} onClick={refreshDesigner}><RefreshCw size={14} />Refresh from Doc Designer</button>
            )}
            {isAdmin && about.db_configured && (
              <div className="flex flex-col items-center gap-1.5">
                {about.db_connected
                  ? <button className={secondaryButton} disabled={busy} onClick={() => connection('disconnect')}><Unplug size={14} />Disconnect database</button>
                  : <button className={secondaryButton} disabled={busy} onClick={() => connection('reconnect')}><PlugZap size={14} />Reconnect database</button>}
                <p className="text-[11px] text-stone-500">For testing: the connection settings are kept; a restart also reconnects.</p>
              </div>
            )}
          </>
        )}
        {!about && !error && <p className="flex items-center gap-2 text-xs text-stone-500"><Info size={14} />Loading…</p>}
        <DevelopedBy className="mt-2 border-t border-[#E3D9CD] pt-4 dark:border-line" />
        <a href="https://www.softvent.com" target="_blank" rel="noreferrer noopener" className="text-xs font-medium text-[#64283A] underline-offset-2 hover:underline dark:text-[#D4A847]">www.softvent.com</a>
      </section>
    </div>
  );
}
