// About (legacy frmUtlAbout, at the root of the desktop menu): product, software version (Global Settings →
// SWVersion), API version, company, database connection, which data the screens show (live SQL Server or the Test
// data switch's sample data) and the "Designed & developed by" credit. An Administrator can Disconnect the database

// No licence key or registration (Key Generation is not part of the web app).

import { useEffect, useState } from 'react';
import { Database, Eye, EyeOff, Info, RefreshCw } from 'lucide-react';
import { errorText, getAbout, getAuthRole, refreshDocDesigner, type ApiAbout } from '../../lib/api';
import { ADMIN_ROLE } from '../../lib/modules';
import { badge, pageSub, pageTitle, panel, secondaryButton } from '../../lib/styles';
import { toastStore } from '../common/toastStore';
import DevelopedBy from '../layout/DevelopedBy';
import VlqmsLogo from '../layout/VlqmsLogo';

export default function About() {
  const [about, setAbout] = useState<ApiAbout | null>(null);
  // The confidential connection details (server, port, login) stay masked until their eye is pressed.
  const [shown, setShown] = useState<Set<string>>(new Set());
  const toggle = (k: string) => setShown(s => { const n = new Set(s); if (n.has(k)) n.delete(k); else n.add(k); return n; });
  const [error, setError] = useState('');

  const [busy, setBusy] = useState(false);
  const isAdmin = getAuthRole() === ADMIN_ROLE;

  useEffect(() => { getAbout().then(setAbout).catch(e => setError(errorText(e))); }, []);

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
            {about.database && (
              <section aria-label="Database" className="w-full rounded-lg border border-[#E3D9CD] p-3 text-left dark:border-[#2A2A2A]">
                <h2 className="mb-2 text-[11px] font-semibold uppercase tracking-wide text-stone-500">Database</h2>
                <dl className="grid grid-cols-[7rem_1fr] items-center gap-x-3 gap-y-1.5 text-xs">
                  <dt className="text-stone-500">Database</dt>
                  <dd className="font-medium text-stone-800 dark:text-stone-100">{about.database}</dd>
                  {about.connection && (
                    <>
                      <dt className="text-stone-500">Login type</dt><dd className="font-medium">{about.connection.auth}</dd>
                      {about.connection.driver && <><dt className="text-stone-500">Driver</dt><dd className="font-medium">{about.connection.driver}</dd></>}
                      {about.connection.edition && <><dt className="text-stone-500">SQL Server</dt><dd className="font-medium">{about.connection.edition} · {about.connection.version}</dd></>}
                      {([['server', 'Server', about.connection.server], ['port', 'Port', about.connection.port], ['user', 'Login user', about.connection.user]] as const)
                        .filter(([, , v]) => v)
                        .map(([k, name, v]) => (
                          <div key={k} className="contents">
                            <dt className="text-stone-500">{name}</dt>
                            <dd className="flex items-center gap-2 font-medium">
                              <span className="font-mono" data-testid={`conn-${k}`}>{shown.has(k) ? v : '••••••••'}</span>
                              <button type="button" onClick={() => toggle(k)} aria-pressed={shown.has(k)} aria-label={`${shown.has(k) ? 'Hide' : 'Show'} ${name.toLowerCase()}`}
                                title={shown.has(k) ? 'Hide' : 'Show'} className="rounded p-1 text-stone-500 hover:bg-stone-100 hover:text-stone-900 dark:hover:bg-[#1c1c1c] dark:hover:text-stone-100">
                                {shown.has(k) ? <EyeOff size={14} /> : <Eye size={14} />}
                              </button>
                            </dd>
                          </div>
                        ))}
                    </>
                  )}
                </dl>
                {about.connection && <p className="mt-2 text-[11px] text-stone-500">The password is never shown. Only an Administrator sees the server, port and login.</p>}
              </section>
            )}
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
          </>
        )}
        {!about && !error && <p className="flex items-center gap-2 text-xs text-stone-500"><Info size={14} />Loading…</p>}
        <DevelopedBy className="mt-2 border-t border-[#E3D9CD] pt-4 dark:border-line" />
        <a href="https://www.softvent.com" target="_blank" rel="noreferrer noopener" className="text-xs font-medium text-[#64283A] underline-offset-2 hover:underline dark:text-[#D4A847]">www.softvent.com</a>
      </section>
    </div>
  );
}
