import Pattern from './ui/c-chart-21';
import { useEffect, useState } from 'react';
import {
  ArrowUpRight, ArrowDownRight,
  Package, Receipt, TrendingUp, Truck, Wine,
} from 'lucide-react';
import { api, qs, todayIso, type ApiDailySale, type ApiFullDsr, type ApiIndent, type ApiItem, type ApiStockMovement } from './api';
import { money, panel } from './ui';
import { IMAGES, productImage } from './constants/images';
import { useCompanyProfile } from './useCompanyProfile';

const LiveBadge = ({ live }: { live: boolean }) => (
  <span className={`inline-flex items-center gap-1.5 rounded-md border px-2 py-0.5 text-[10px] font-semibold uppercase tracking-wider ${live
    ? 'border-emerald-500/35 bg-emerald-50 text-emerald-700 shadow-[0_0_14px_rgba(16,185,129,0.28)] dark:bg-emerald-950/50 dark:text-emerald-300 dark:shadow-[0_0_16px_rgba(16,185,129,0.24)]'
    : 'border-line bg-muted text-[#70675F] dark:text-stone-400'
  }`}>
    {live && <span className="h-1.5 w-1.5 rounded-full bg-emerald-500 shadow-[0_0_7px_rgba(16,185,129,0.8)]" />}
    {live ? 'Live' : 'Unavailable'}
  </span>
);

// ─── DASHBOARD ───
export const Dashboard = () => {
  const { company } = useCompanyProfile();
  const [dsr, setDsr] = useState<ApiFullDsr | null>(null);
  const [dailySales, setDailySales] = useState<ApiDailySale[] | null>(null);
  const [stockMovement, setStockMovement] = useState<ApiStockMovement | null>(null);
  const [indents, setIndents] = useState<ApiIndent[] | null>(null);
  const [items, setItems] = useState<ApiItem[] | null>(null);
  const [storeImage, setStoreImage] = useState<string>(IMAGES.dashboard.store);

  // Real-time store operational hours (CL-2 Retail License: 10:00 AM to 10:30 PM)
  const [isStoreOpen, setIsStoreOpen] = useState(() => {
    const now = new Date();
    const minutes = now.getHours() * 60 + now.getMinutes();
    return minutes >= 10 * 60 && minutes < 22 * 60 + 30;
  });

  useEffect(() => {
    const checkStoreStatus = () => {
      const now = new Date();
      const minutes = now.getHours() * 60 + now.getMinutes();
      setIsStoreOpen(minutes >= 10 * 60 && minutes < 22 * 60 + 30);
    };
    const timer = setInterval(checkStoreStatus, 20000);
    return () => clearInterval(timer);
  }, []);

  useEffect(() => {
    const loadDsr = () => api<ApiFullDsr>(`/reports/dsr${qs({ on: todayIso() })}`).then(setDsr).catch(() => setDsr(null));
    const loadIndents = () => api<ApiIndent[]>('/transactions/purchase-indent').then(setIndents).catch(() => setIndents(null));
    const loadItems = () => api<ApiItem[]>('/masters/items').then(setItems).catch(() => setItems(null));
    const loadSales = () => {
      const now = new Date();
      const from = new Date(now);
      from.setDate(now.getDate() - 6);
      api<ApiDailySale[]>(`/reports/sales/daily${qs({ from_date: from.toISOString().slice(0, 10), to_date: now.toISOString().slice(0, 10) })}`)
        .then(setDailySales)
        .catch(() => setDailySales(null));
    };
    const loadStockMovement = () => api<ApiStockMovement>(`/reports/stock-movement${qs({ on: todayIso() })}`)
      .then(setStockMovement)
      .catch(() => setStockMovement(null));

    const loadDashboardData = () => {
      loadDsr();
      loadIndents();
      loadSales();
      loadStockMovement();
    };
    loadDashboardData();
    loadItems();
    const handleFocus = () => {
      loadDashboardData();
      loadItems();
    };
    window.addEventListener('focus', handleFocus);
    const itemInterval = setInterval(loadItems, 8000);
    const dashboardInterval = setInterval(loadDashboardData, 10000);
    api<Array<{ id: string; kind: string }>>('/company/files?kind=image')
      .then(files => {
        const first = files.find(f => f.kind === 'image');
        if (first) setStoreImage("/api/company/files/" + first.id + "/content");
      })
      .catch(() => {});

    return () => {
      window.removeEventListener('focus', handleFocus);
      clearInterval(itemInterval);
      clearInterval(dashboardInterval);
    };
  }, []);

  const counters = dsr ? Object.entries(dsr.counter_wise).map(([c, v]) => `${c} ₹ ${money(v)}`).join(' · ') : '';
  const stats = [
    { label: "Today's Revenue", value: dsr ? `₹ ${money(dsr.sales_incl_gst)}` : '—', sub: dsr ? `+₹ ${money(dsr.profit_of_the_day)} profit` : 'Unavailable', icon: TrendingUp },
    { label: 'Bills Issued', value: dsr ? String(dsr.total_bills) : '—', sub: counters || 'No bills yet today', icon: Receipt },
    { label: 'Inventory Value', value: dsr ? `₹ ${money(dsr.closing_stock.value)}` : '—', sub: dsr ? `${dsr.closing_stock.qty} bottles · ${dsr.closing_stock.litres} L` : '—', icon: Package },
    { label: 'KSBCL Indents', value: indents ? String(indents.length) : '—', sub: indents ? `${indents.reduce((acc, i) => acc + i.totals.qty, 0)} bottles ordered` : 'Unavailable', icon: Truck },
  ];

  const weekData = (dailySales ?? []).map(sale => ({
    ...sale,
    amtK: Math.round(sale.amount / 1000),
  }));
  const max = Math.max(...weekData.map(day => day.amtK), 1);
  const latest = weekData[weekData.length - 1];
  const previous = weekData[weekData.length - 2];
  const pctChange = previous?.amount > 0
    ? Math.round(((latest.amount - previous.amount) / previous.amount) * 1000) / 10
    : null;
  const isPositiveGrowth = pctChange !== null && pctChange >= 0;
  const today = new Date().toLocaleDateString('en-IN', { weekday: 'long', day: 'numeric', month: 'long', year: 'numeric' });
  const featured = (items ?? []).filter(i => (i.qoh ?? 0) > 0).slice(0, 4);
  const hasInventoryValue = items?.some(item => item.qoh > 0 && item.mrp > 0) ?? false;

  // Dynamic Inventory Mix calculation from real inventory stocks
  const inventoryMixData = (() => {
    const groupTotals = { whisky: 0, beer: 0, rum_vodka: 0, scotch: 0 };
    items?.forEach(it => {
      const g = (it.group || '').toLowerCase();
      const value = Math.max(0, it.qoh || 0) * Math.max(0, it.mrp || 0);
      if (g.includes('beer')) groupTotals.beer += value;
      else if (g.includes('rum') || g.includes('vodka') || g.includes('gin')) groupTotals.rum_vodka += value;
      else if (g.includes('scotch') || g.includes('wine')) groupTotals.scotch += value;
      else groupTotals.whisky += value;
    });

    const total = Object.values(groupTotals).reduce((a, b) => a + b, 0);
    if (!total) {
      return [
        { category: 'whisky', share: 0, fill: '#0CBF82', color: '#0CBF82', label: 'Whisky' },
        { category: 'beer', share: 0, fill: '#3383FF', color: '#3383FF', label: 'Beer' },
        { category: 'rum_vodka', share: 0, fill: '#FE9E0C', color: '#FE9E0C', label: 'Rum & Vodka' },
        { category: 'scotch', share: 0, fill: '#FF2F60', color: '#FF2F60', label: 'Scotch' },
      ];
    }
    const w = Math.round((groupTotals.whisky / total) * 100);
    const b = Math.round((groupTotals.beer / total) * 100);
    const r = Math.round((groupTotals.rum_vodka / total) * 100);
    const s = Math.max(0, 100 - w - b - r);

    return [
      { category: 'whisky', share: w, fill: '#0CBF82', color: '#0CBF82', label: 'Whisky' },
      { category: 'beer', share: b, fill: '#3383FF', color: '#3383FF', label: 'Beer' },
      { category: 'rum_vodka', share: r, fill: '#FE9E0C', color: '#FE9E0C', label: 'Rum & Vodka' },
      { category: 'scotch', share: s, fill: '#FF2F60', color: '#FF2F60', label: 'Scotch' },
    ];
  })();

  const inventoryMixConfig = {
    share: { label: 'Share of Stock' },
    whisky: { label: 'Whisky', color: '#0CBF82' },
    beer: { label: 'Beer', color: '#3383FF' },
    rum_vodka: { label: 'Rum & Vodka', color: '#FE9E0C' },
    scotch: { label: 'Scotch', color: '#FF2F60' },
  } as const;

  return (
    <div className="mx-auto max-w-7xl space-y-6">

      {/* ═══ HERO BANNER ═══ */}
      <section className="animate-fadeInUp relative overflow-hidden rounded-2xl dark:rounded-3xl border border-line dark:border-[#2A2A2A]">
        <div className="absolute inset-0">
          <img
            src={IMAGES.dashboard.hero}
            alt=""
            className="h-full w-full object-cover object-[center_30%] ken-burns contrast-[1.5] brightness-[0.93] dark:contrast-[1.1] dark:brightness-[1.0]"
            style={{ opacity: 0.88 }}
            loading="lazy"
          />
        </div>
        {/* Heritage warm gradient overlay */}
        <div className="absolute inset-0 bg-gradient-to-r from-[#180F0B]/90 via-[#180F0B]/50 to-black/20
                        dark:from-[#0A0A0A]/95 dark:via-[#0A0A0A]/60 dark:to-transparent" />
        <div className="absolute inset-0 bg-gradient-to-t from-surface/30 to-transparent
                        dark:from-[#0A0A0A]/40 dark:to-transparent" />

        <div className="relative z-10 flex flex-col justify-between gap-6 p-7 sm:p-10 lg:min-h-[260px] lg:flex-row lg:items-center lg:p-12">
          <div className="max-w-lg">
            <p className="text-[10px] font-bold tracking-[0.35em] text-[#D3BB89] uppercase">
              Business Overview
            </p>
            <h2 className="mt-3 text-2xl sm:text-3xl lg:text-4xl font-extrabold text-white leading-tight"
                style={{ fontFamily: 'var(--font-display)' }}>
              Everything under{' '}
              <span className="text-[#D3BB89]">one roof.</span>
            </h2>
            <p className="mt-3 text-sm text-white/70 max-w-md leading-relaxed">
              Monitor inventory, sales, procurement and compliance across your entire operation.
            </p>

          </div>
          <div className="hidden lg:flex flex-col items-end">
            <div className="rounded-xl border border-white/25 bg-[#1A100B]/80 dark:bg-black/80 backdrop-blur-md px-4 py-3 shadow-[0_8px_30px_rgba(0,0,0,0.35)] text-right">
              <div className="text-xs font-bold text-[#E8C36A] flex items-center justify-end gap-1.5 tracking-wide">
                <span className="h-1.5 w-1.5 rounded-full bg-[#E8C36A] animate-pulse" />
                {today}
              </div >
              <div className="mt-1 text-xs font-semibold text-white/95 tracking-wide">
                {company?.address || 'Company address unavailable'}
              </div>
              <div className="mt-0.5 text-[11px] font-medium text-amber-200/90 dark:text-amber-300/90 tracking-wide">
                Excise licence {company?.licence_type || 'not configured'}
              </div>
            </div>
          </div>
        </div>
      </section>

      {/* ═══ KPI STAT CARDS ═══ */}
      <section aria-label="Key figures" className="stagger-enter grid grid-cols-2 gap-3 sm:gap-4 xl:grid-cols-4">
        {stats.map(s => (
          <div key={s.label} className="card-3d rounded-2xl border p-4 sm:p-5
                                        bg-[#FFFCF7] border-[#E3D9CD] shadow-[0_8px_30px_rgba(72,53,38,0.06)]
                                        hover:shadow-[0_12px_35px_rgba(72,53,38,0.10)] hover:border-[#D4C8BA]
                                        dark:glass-card dark:glow-gold dark:border-[#2A2A2A] dark:bg-transparent">
            <div className="flex items-center justify-between">
              <span className="text-[11px] font-semibold uppercase tracking-[0.08em] text-[#70675F] dark:text-stone-400">{s.label}</span>
              <span className="flex h-8 w-8 items-center justify-center rounded-lg bg-[#64283A]/8 dark:bg-[#D4A847]/10">
                <s.icon size={15} className="text-[#64283A] dark:text-[#D4A847]" aria-hidden="true" />
              </span>
            </div>
            <div className="mt-3 truncate text-lg font-extrabold tracking-tight sm:text-2xl tabular-nums text-[#29231F] dark:text-stone-50">{s.value}</div>
            <div className="mt-1 truncate text-xs font-medium text-[#55745A] dark:text-[#D4A847]" title={s.sub}>{s.sub}</div>
          </div>
        ))}
      </section>

      {/* ═══ FEATURED PRODUCTS ═══ */}
      {featured.length > 0 && (
        <section className="animate-fadeInUp animate-fadeInUp-d2">
          <div className="mb-4">
            <p className="text-[10px] font-bold tracking-[0.25em] text-[#B08A45] dark:text-[#D4A847] uppercase">Inventory</p>
            <h3 className="mt-1 text-lg font-bold text-[#29231F] dark:text-white">Featured products</h3>
          </div>

          <div className="stagger-enter grid grid-cols-2 gap-3 sm:gap-4 lg:grid-cols-4">
            {featured.map(item => {
              const isLow = item.qoh <= 12;
              return (
                <div key={item.code}
                     className="group img-zoom card-3d overflow-hidden rounded-2xl border
                                bg-[#FFFCF7] border-[#E3D9CD]
                                dark:border-[#2A2A2A] dark:bg-[#141414]">
                  <div className="relative h-36 sm:h-44 overflow-hidden bg-[#F0EBE3] dark:bg-[#1A1A1A]">
                    <img src={productImage(item.name, item.group)} alt={item.name}
                      className="h-full w-full object-cover
                                 transition-transform duration-500 ease-out group-hover:scale-[1.035]"
                      style={{ filter: 'saturate(0.95) contrast(1.0) brightness(1.0)' }}
                      loading="lazy" />
                    <span className={`absolute top-3 right-3 rounded-full px-2.5 py-1 text-[10px] font-bold uppercase tracking-wider shadow-md tabular-nums
                      ${isLow
                        ? 'bg-[#F8EDDA] text-[#875D24] dark:bg-amber-500/90 dark:text-black'
                        : 'bg-[#EAF1EA] text-[#426147] dark:bg-emerald-500/90 dark:text-white'}`}>
                      {item.qoh > 0 ? `${item.qoh} in stock` : 'Out of Stock'}
                    </span>
                    <div className="absolute inset-x-0 bottom-0 h-16 bg-gradient-to-t from-[#FFFCF7] dark:from-[#141414] to-transparent" />
                  </div>
                  <div className="p-4">
                    <p className="text-[10px] font-bold tracking-[0.15em] text-[#B08A45] dark:text-[#D4A847] uppercase">
                      {(item.group || '').replace(/^\d+[.\s_-]*/, '').trim() || 'LIQUOR'}
                    </p>
                    <h4 className="mt-1 text-sm font-semibold text-[#29231F] dark:text-white truncate" title={item.name}>
                      {item.name.replace(/^\d+[.\s_-]*/, '').trim()}
                    </h4>
                    <div className="mt-3 flex items-center justify-between">
                      <span className="text-base font-extrabold tabular-nums text-[#29231F] dark:text-white">₹{money(item.mrp)}</span>
                      <span className="text-[10px] font-bold text-[#B08A45] dark:text-[#D4A847] uppercase tracking-wider">{item.brand || item.code}</span>
                    </div>
                  </div>
                </div>
              );
            })}
          </div>
        </section>
      )}

      {/* ═══ FULL-SPAN WAREHOUSE BANNER ═══ */}
      <section className="animate-fadeInUp animate-fadeInUp-d3">
        <div className="group relative overflow-hidden rounded-2xl dark:rounded-3xl h-64 sm:h-72 lg:h-80 img-zoom border border-[#E3D9CD] dark:border-[#2A2A2A] shadow-[0_8px_30px_rgba(72,53,38,0.06)] dark:shadow-card">
          <img src={IMAGES.dashboard.warehouse} alt="Warehouse"
            className="absolute inset-0 h-full w-full object-cover object-center
                       transition-transform duration-700 ease-out group-hover:scale-[1.035]"
            style={{ opacity: 0.85, filter: 'saturate(0.9) brightness(0.95)' }}
            loading="lazy" />
          {/* Dual-theme atmospheric overlay gradients */}
          <div className="absolute inset-0 bg-gradient-to-r from-[#1E130E]/90 via-[#1E130E]/50 to-transparent
                          dark:from-[#0A0A0A]/95 dark:via-[#0A0A0A]/60 dark:to-transparent" />
          <div className="absolute inset-0 bg-gradient-to-t from-[#1E130E]/85 via-transparent to-transparent
                          dark:from-[#0A0A0A]/85" />
          <div className="absolute top-6 right-6 animate-float opacity-30" aria-hidden="true">
            <Wine size={36} className="text-[#D3BB89]" />
          </div>
          <div className="absolute bottom-0 left-0 right-0 p-6 sm:p-10 flex flex-col sm:flex-row sm:items-end justify-between gap-4">
            <div>
              <p className="text-[10px] font-bold tracking-[0.25em] text-[#D3BB89] uppercase">Warehouse & Logistics</p>
              <h3 className="mt-1 text-2xl sm:text-3xl font-bold text-white" style={{ fontFamily: 'var(--font-display)' }}>
                {company?.name ? `${company.name} Distribution Center` : 'Distribution Center'}
              </h3>
              <p className="mt-2 text-xs sm:text-sm text-stone-300/80">{company?.address || 'Company address unavailable'} · Excise Licence {company?.licence_type || 'not configured'} · Primary Bonded Depot</p>
            </div>
            <div className="flex items-center gap-2.5 text-xs text-white/80">
              <span className="rounded-full bg-white/10 backdrop-blur px-3.5 py-1.5 border border-white/15 font-medium">Active Depot</span>
              <span className="rounded-full bg-[#B08A45]/20 backdrop-blur px-3.5 py-1.5 border border-[#B08A45]/30 text-[#D3BB89] font-medium">Bonded Storage</span>
            </div>
          </div>
        </div>
      </section>

      {/* ═══ CHARTS ═══ */}
      <div className="grid grid-cols-1 gap-4 xl:grid-cols-3 animate-fadeInUp animate-fadeInUp-d4">
        <section className="xl:col-span-2 rounded-2xl border p-5
                            bg-[#FFFCF7] border-[#E3D9CD] shadow-[0_8px_30px_rgba(72,53,38,0.06)]
                            dark:glass-card dark:border-[#2A2A2A] dark:bg-transparent">
          <div className="mb-5 flex items-start justify-between gap-3">
            <div>
              <h2 className="text-sm font-semibold text-[#29231F] dark:text-stone-50">Sales, last 7 days</h2>
              <p className="mt-0.5 text-xs text-[#70675F] dark:text-stone-400">Bill value in ₹ thousands</p>
            </div>
            <div className="flex items-center gap-2">
              {pctChange !== null && (
                <span className={`inline-flex items-center gap-1 rounded-md px-1.5 py-0.5 text-[11px] font-medium ${isPositiveGrowth ? 'bg-[#EAF1EA] text-[#426147]' : 'bg-rose-100 text-rose-800 dark:bg-rose-950/60 dark:text-rose-300'}`}>
                  {isPositiveGrowth ? <ArrowUpRight size={12} /> : <ArrowDownRight size={12} />}
                  {isPositiveGrowth ? '+' : ''}{pctChange}%
                </span>
              )}
              <LiveBadge live={dailySales !== null} />
            </div>
          </div>
          <div className="flex h-48 items-end gap-3 border-b border-line pb-px sm:gap-5" role="img" aria-label={weekData.length ? `Sales per day: ${weekData.map(day => `${day.day ?? day.date} ₹${day.amtK}k`).join(', ')}` : 'Sales data unavailable'}>
            {weekData.map((day, i) => (
              <div key={day.date} className="group flex h-full flex-1 flex-col items-center justify-end gap-1.5">
                <span className="text-[11px] tabular-nums text-[#70675F] opacity-100 transition-opacity dark:text-stone-400 lg:opacity-0 lg:group-hover:opacity-100">₹{day.amtK}k</span>
                <div
                  className={`w-full max-w-10 rounded-t-md transition-all duration-300 ${
                    i === weekData.length - 1
                      ? 'bg-[#64283A] dark:bg-gradient-to-t dark:from-[#C8952E] dark:to-[#D4A847]'
                      : 'bg-[#D4C8BA] group-hover:bg-[#B08A45]/50 dark:bg-stone-700 dark:group-hover:bg-[#D4A847]/50'
                  }`}
                  style={{ height: `${day.amount > 0 ? Math.max(4, (day.amtK / max) * 82) : 0}%` }}
                  title={`${day.date}: ₹ ${money(day.amount)} (${day.bills} bills)`}
                />
              </div>
            ))}
          </div>
          <div className="mt-2 flex gap-3 sm:gap-5">
            {weekData.map(day => <span key={day.date} className="flex-1 text-center text-[11px] text-[#70675F] dark:text-stone-400">{day.day ?? day.date}</span>)}
            {!weekData.length && <span className="w-full text-center text-xs text-[#70675F] dark:text-stone-400">Sales data unavailable</span>}
          </div>
        </section>

        <div className="h-full">
          {hasInventoryValue ? (
            <Pattern
              data={inventoryMixData}
              config={inventoryMixConfig}
              title="Inventory Mix"
              description="Dynamic share of stock value"
              dataKey="share"
              nameKey="category"
              className="h-full flex flex-col justify-between"
            />
          ) : (
            <div className={`${panel} flex h-full min-h-48 flex-col justify-center`}>
              <h2 className="text-sm font-semibold text-[#29231F] dark:text-stone-50">Inventory Mix</h2>
              <p className="mt-2 text-xs text-[#70675F] dark:text-stone-400">
                {items ? 'No inventory value on record.' : 'Inventory data unavailable.'}
              </p>
            </div>
          )}
        </div>
      </div>

      {/* ═══ STOCK MOVEMENT ═══ */}
      <section className="rounded-2xl border p-5 animate-fadeInUp animate-fadeInUp-d5
                          bg-[#FFFCF7] border-[#E3D9CD] shadow-[0_8px_30px_rgba(72,53,38,0.06)]
                          dark:glass-card dark:border-[#2A2A2A] dark:bg-transparent">
        <div className="mb-5 flex items-start justify-between gap-3">
          <div>
            <h2 className="text-sm font-semibold text-[#29231F] dark:text-stone-50">Stock movement</h2>
            <p className="mt-0.5 text-xs text-[#70675F] dark:text-stone-400">Today's inventory flow across the store, in bottles</p>
          </div>
          <LiveBadge live={stockMovement !== null} />
        </div>
        <div className="grid grid-cols-1 gap-6 sm:grid-cols-3">
          {(stockMovement?.flow ?? []).map(item => (
            <div key={item.label}>
              <p className="text-xs text-[#70675F] dark:text-stone-400">{item.label}</p>
              <p className="mt-1 text-xl font-semibold tabular-nums text-[#29231F] dark:text-stone-50">{item.value}</p>
              <div className="mt-2 h-1.5 rounded-full bg-muted"><div className={`h-full rounded-full ${item.color}`} style={{ width: `${item.pct}%` }} /></div>
            </div>
          ))}
          {!stockMovement && <p className="text-xs text-[#70675F] dark:text-stone-400">Stock movement data unavailable.</p>}
        </div>
      </section>

      {/* ═══ FOOTER STORE BANNER (COMPANY MASTER 1ST IMAGE) ═══ */}
      <section className="animate-fadeInUp animate-fadeInUp-d5">
        <div className="group relative overflow-hidden rounded-2xl dark:rounded-3xl h-48 sm:h-60 lg:h-72 img-zoom border border-[#E3D9CD] dark:border-[#2A2A2A] shadow-[0_8px_30px_rgba(72,53,38,0.06)] dark:shadow-card">
          <img
            src={storeImage}
            alt={company?.name ? `${company.name} store` : 'Company store'}
            className="absolute inset-0 h-full w-full object-cover object-[center_35%]
                       transition-transform duration-700 ease-out group-hover:scale-[1.03]"
            style={{ opacity: 0.88, filter: 'saturate(0.95) contrast(1.0) brightness(0.95)' }}
            loading="lazy"
          />
          {/* Dual luxury atmosphere gradient overlays */}
          <div className="absolute inset-0 bg-gradient-to-r from-[#1A110D]/92 via-[#1A110D]/50 to-[#1A110D]/75
                          dark:from-[#0A0A0A]/95 dark:via-[#0A0A0A]/55 dark:to-[#0A0A0A]/85" />
          <div className="absolute inset-0 bg-gradient-to-t from-[#1A110D]/85 via-transparent to-transparent
                          dark:from-[#0A0A0A]/85" />

          <div className="absolute bottom-0 left-0 right-0 p-4 pt-3 sm:p-10 flex flex-col sm:flex-row sm:items-end justify-between gap-3 sm:gap-4">
            <div>
              <p className="mt-2 sm:mt-0 text-[10px] font-bold tracking-[0.25em] text-[#D3BB89] uppercase">
                Retail Outlet{company?.licence_type ? ` · ${company.licence_type}` : ' · Company Master'}
              </p>
              <h3 className="mt-1 text-2xl sm:text-3xl font-bold text-white" style={{ fontFamily: 'var(--font-display)' }}>
                {company?.name || 'Company profile unavailable'}
              </h3>
              <p className="mt-1.5 sm:mt-2 text-xs sm:text-sm text-stone-300/85 max-w-lg">
                {company?.address || 'Company address unavailable'} · Primary Retail Billing Counter
              </p>
            </div>
            <div className="flex flex-wrap items-center gap-2.5 text-xs text-white/90">
              <span className={`inline-flex items-center gap-1.5 rounded-full backdrop-blur px-3.5 py-1.5 border font-semibold ${
                isStoreOpen
                  ? 'bg-emerald-500/20 border-emerald-500/30 text-emerald-300'
                  : 'bg-rose-500/20 border-rose-500/30 text-rose-300'
              }`}>
                <span className={`h-1.5 w-1.5 rounded-full ${isStoreOpen ? 'bg-emerald-400 animate-pulse' : 'bg-rose-400'}`} />
                {isStoreOpen ? 'Store Open' : 'Store Closed'}
              </span>
              <span className="inline-flex items-center gap-1.5 rounded-full border border-emerald-500/30 bg-emerald-500/20 px-3.5 py-1.5 font-medium text-emerald-300 shadow-[0_0_14px_rgba(16,185,129,0.22)] backdrop-blur">
                <span className="h-1.5 w-1.5 rounded-full bg-emerald-400 shadow-[0_0_7px_rgba(16,185,129,0.8)]" />
                POS Counters Online
              </span>
            </div>
          </div>
        </div>
      </section>
    </div>
  );
};

