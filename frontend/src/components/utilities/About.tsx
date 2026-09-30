// About (legacy frmUtlAbout, at the root of the desktop menu): product, software version (Global Settings →
// SWVersion), API version, company, database connection and the "Designed & developed by" credit.
// No licence key or registration (Key Generation is not part of the web app).

import { useEffect, useState } from 'react';
import { Database, Info } from 'lucide-react';
import { api, errorText, type ApiAbout } from '../../lib/api';
import { badge, pageSub, pageTitle, panel } from '../../lib/styles';
import DevelopedBy from '../layout/DevelopedBy';
import VlqmsLogo from '../layout/VlqmsLogo';

export default function About() {
  const [about, setAbout] = useState<ApiAbout | null>(null);
  const [error, setError] = useState('');

  useEffect(() => { api<ApiAbout>('/utilities/about').then(setAbout).catch(e => setError(errorText(e))); }, []);

  const rows: [string, string][] = about ? [
    ['Software version', about.software_version],
    ['API version', about.api_version],
    ['Company', about.company || '—'],
    ['Server time', new Date(about.server_time).toLocaleString('en-IN')],
  ] : [];

  return (
    <div className="mx-auto flex max-w-xl flex-col gap-4">
      <section className={`${panel} flex flex-col items-center gap-4 p-8 text-center`}>
        <VlqmsLogo size="lg" />
        <div>
          <h1 className={pageTitle}>{about?.product ?? 'vLQMS Web'}</h1>
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
            <span className={badge(about.db_connected ? 'success' : 'neutral')}>
              <Database size={12} />{about.db_connected ? 'vLQMS SQL Server connected' : 'Running on the web app\'s own data (SQL Server not connected)'}
            </span>
          </>
        )}
        {!about && !error && <p className="flex items-center gap-2 text-xs text-stone-500"><Info size={14} />Loading…</p>}
        <DevelopedBy className="mt-2 border-t border-[#E3D9CD] pt-4 dark:border-line" />
        <a href="https://www.softvent.com" target="_blank" rel="noreferrer noopener" className="text-xs font-medium text-[#64283A] underline-offset-2 hover:underline dark:text-[#D4A847]">www.softvent.com</a>
      </section>
    </div>
  );
}
