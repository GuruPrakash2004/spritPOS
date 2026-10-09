// Home screen after login: today's KPIs and charts.
//
// Reads GET /reports/dsr (today), /reports/sales/daily (last 7 days), /reports/stock-movement,
// /transactions/purchase-indent, /masters/items and the first company image. Each panel shows a
// "Live" badge when its data came from the API and "Unavailable" when the call failed.
// The "Restock immediately" carousel (RestockCarousel.jsx) shows up to 10 items from /reports/restock,
// ranked by the backend so fast sellers that are out of stock or about to run out come first; it is
// re-polled every 8 s with the items.

import { Suspense, lazy, useEffect, useState } from 'react';
import { SkeletonLine } from '../common/Skeleton';
import {
  ArrowUpRight, ArrowDownRight,
  Package, Receipt, TrendingUp, Truck, Wine,
} from 'lucide-react';
import { companyFileUrl, fieldRight, getDailySales, getDsr, getItems, getPurchaseIndents, getRestock, getStockMovement, listCompanyFiles, todayIso, } from '../../lib/api';
import { money, panel } from '../../lib/styles';
import { IMAGES } from '../../constants/images';
import RestockCarousel from './RestockCarousel';
import { useCompanyProfile } from '../../hooks/useCompanyProfile';
import { useShopHours } from '../../hooks/useShopHours';

// The pie chart pulls in the whole charts library (the biggest piece of the app): it loads after the rest of the Dashboard has painted.
const Pattern = lazy(() => import('../ui/c-chart-21'));

// Sales window (days) for the restock analysis.
const RESTOCK_DAYS = 15;

const LiveBadge = ({ live }) => (
  <span className={`inline-flex items-center gap-1.5 rounded-md border px-2 py-0.5 text-[10px] font-semibold uppercase tracking-wider ${live
    ? 'border-emerald-500/35 bg-emerald-50 text-emerald-700 shadow-[0_0_14px_rgba(16,185,129,0.28)] dark:bg-emerald-950/50 dark:text-emerald-300 dark:shadow-[0_0_16px_rgba(16,185,129,0.24)]'
    : 'border-line bg-muted text-[#5F6B80] dark:text-stone-400'
  }`}>
    {live && <span className="h-1.5 w-1.5 rounded-full bg-emerald-500 shadow-[0_0_7px_rgba(16,185,129,0.8)]" />}
    {live ? 'Live' : 'Unavailable'}
  </span>
);

// ─── DASHBOARD ───
// A panel is left out when the role's Field Level Rights hide it (Admin Master → Field rights → Dashboard).
const shown = (panel) => fieldRight('dashboard', panel) !== 'hidden';

export const Dashboard = () => {
  const { company, isOnline } = useCompanyProfile();
  const hours = useShopHours();   // Open / Closed (Karnataka timings, Holiday Master), next to the Live chip
  const [dsr, setDsr] = useState(null);
  const [dailySales, setDailySales] = useState(null);
  const [stockMovement, setStockMovement] = useState(null);
  const [indents, setIndents] = useState(null);
  const [items, setItems] = useState(null);
  const [restockRows, setRestockRows] = useState(null);
  const [storeImage, setStoreImage] = useState(IMAGES.dashboard.store);

  useEffect(() => {
    const loadDsr = () => getDsr(todayIso()).then(setDsr).catch(() => setDsr(null));
    const loadIndents = () => getPurchaseIndents().then(setIndents).catch(() => setIndents(null));
    const loadItems = () => {
      getItems().then(setItems).catch(() => setItems(null));
      if (shown('restock')) {
        getRestock(RESTOCK_DAYS, 10)
          .then(setRestockRows)
          .catch(() => setRestockRows(null));
      }
    };
    const loadSales = () => {
      const now = new Date();
      const from = new Date(now);
      from.setDate(now.getDate() - 6);
      getDailySales(from.toISOString().slice(0, 10), now.toISOString().slice(0, 10))
        .then(setDailySales)
        .catch(() => setDailySales(null));
    };
    const loadStockMovement = () => getStockMovement(todayIso())
      .then(setStockMovement)
      .catch(() => setStockMovement(null));

    const loadDashboardData = () => {
      loadDsr();
      loadIndents();
      loadSales();
      if (shown('stock_movement')) loadStockMovement();
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
    listCompanyFiles('image')
      .then(files => {
        const first = files.find(f => f.kind === 'image');
        if (first) setStoreImage(companyFileUrl(first.id));
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
    { label: "Today's Revenue", value: dsr ? `₹ ${money(dsr.sales_incl_gst)}` : '—', sub: !dsr ? 'Unavailable' : dsr.profit_of_the_day === null ? `${dsr.total_bills} bills today` : `+₹ ${money(dsr.profit_of_the_day)} profit`, icon: TrendingUp },
    { label: 'Bills Issued', value: dsr ? String(dsr.total_bills) : '—', sub: counters || 'No bills yet today', icon: Receipt },
    { label: dsr?.closing_stock.value === null ? 'Inventory' : 'Inventory Value', value: !dsr ? '—' : dsr.closing_stock.value === null ? `${dsr.closing_stock.qty} btl` : `₹ ${money(dsr.closing_stock.value)}`, sub: dsr ? `${dsr.closing_stock.qty} bottles · ${dsr.closing_stock.litres} L` : '—', icon: Package },
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
  } ;

  return (
    <div className="mx-auto max-w-7xl space-y-6">

      {/* ═══ HERO BANNER ═══ */}
      {shown('hero') && (
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
            <p className="text-[10px] font-bold tracking-[0.35em] text-[#F0CF8A] uppercase">
              Business Overview
            </p>
            <h2 className="mt-3 text-2xl sm:text-3xl lg:text-4xl font-extrabold text-white leading-tight"
                style={{ fontFamily: 'var(--font-display)' }}>
              Fast Billing. Accurate Stock.{' '}
              Better Control.
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
      )}

      {/* ═══ KPI STAT CARDS ═══ */}
      {shown('kpis') && (
<section aria-label="Key figures" className="stagger-enter grid grid-cols-2 gap-3 sm:gap-4 xl:grid-cols-4">
        {stats.map(s => (
          <div key={s.label} className="card-3d rounded-2xl border p-4 sm:p-5
                                        bg-[#FFFFFF] border-[#DDE3EC] shadow-[0_8px_30px_rgba(72,53,38,0.06)]
                                        hover:shadow-[0_12px_35px_rgba(72,53,38,0.10)] hover:border-[#C8D1DF]
                                        dark:glass-card dark:glow-gold dark:border-[#2A2A2A] dark:bg-transparent">
            <div className="flex items-center justify-between">
              <span className="text-[11px] font-semibold uppercase tracking-[0.08em] text-[#5F6B80] dark:text-stone-400">{s.label}</span>
              <span className="flex h-8 w-8 items-center justify-center rounded-lg bg-[#1B2B4B]/8 dark:bg-[#D4A847]/10">
                <s.icon size={15} className="text-[#1B2B4B] dark:text-[#D4A847]" aria-hidden="true" />
              </span>
            </div>
            <div className="mt-3 truncate text-lg font-extrabold tracking-tight sm:text-2xl tabular-nums text-[#1A2236] dark:text-stone-50">{s.value}</div>
            <div className="mt-1 truncate text-xs font-medium text-[#55745A] dark:text-[#D4A847]" title={s.sub}>{s.sub}</div>
          </div>
        ))}
      </section>
      )}

      {/* ═══ RESTOCK IMMEDIATELY (fast sellers running out first, from /reports/restock) ═══ */}
      {shown('restock') && <RestockCarousel rows={restockRows} days={RESTOCK_DAYS} badge={<LiveBadge live={restockRows !== null} />} />}

      {/* ═══ FULL-SPAN WAREHOUSE BANNER ═══ */}
      {shown('company_banner') && (
<section data-panel="company_banner" className="animate-fadeInUp animate-fadeInUp-d3">
        <div className="group relative overflow-hidden rounded-2xl dark:rounded-3xl h-64 sm:h-72 lg:h-80 img-zoom border border-[#DDE3EC] dark:border-[#2A2A2A] shadow-[0_8px_30px_rgba(72,53,38,0.06)] dark:shadow-card">
          <img src={IMAGES.dashboard.warehouse} alt="" loading="lazy" decoding="async"
            className="absolute inset-0 h-full w-full object-cover object-center
                       transition-transform duration-400 ease-out group-hover:scale-[1.035]"
            style={{ opacity: 0.85, filter: 'saturate(0.9) brightness(0.95)' }}
            loading="lazy" />
          {/* Dual-theme atmospheric overlay gradients */}
          <div className="absolute inset-0 bg-gradient-to-r from-[#1E130E]/90 via-[#1E130E]/50 to-transparent
                          dark:from-[#0A0A0A]/95 dark:via-[#0A0A0A]/60 dark:to-transparent" />
          <div className="absolute inset-0 bg-gradient-to-t from-[#1E130E]/85 via-transparent to-transparent
                          dark:from-[#0A0A0A]/85" />
          <div className="absolute top-6 right-6 animate-float opacity-30" aria-hidden="true">
            <Wine size={36} className="text-[#F0CF8A]" />
          </div>
          <div className="absolute bottom-0 left-0 right-0 p-6 sm:p-10 flex flex-col sm:flex-row sm:items-end justify-between gap-4">
            <div>
              <h3 className="mt-1 text-2xl sm:text-3xl font-bold text-white" style={{ fontFamily: 'var(--font-display)' }}>
                {company?.name || 'Company profile'}
              </h3>
              <p className="mt-2 text-xs sm:text-sm text-stone-300/80">
                {[company?.address, company?.licence_type && `Excise Licence ${company.licence_type}`, company?.gstin && `GSTIN ${company.gstin}`].filter(Boolean).join(' · ') || 'Fill in the company details in Company Master.'}
              </p>
            </div>
          </div>
        </div>
      </section>
      )}

      {/* ═══ CHARTS ═══ */}
      <div className="grid grid-cols-1 gap-4 xl:grid-cols-3 animate-fadeInUp animate-fadeInUp-d4">
        {shown('sales_chart') && (
<section className="xl:col-span-2 rounded-2xl border p-5
                            bg-[#FFFFFF] border-[#DDE3EC] shadow-[0_8px_30px_rgba(72,53,38,0.06)]
                            dark:glass-card dark:border-[#2A2A2A] dark:bg-transparent">
          <div className="mb-5 flex items-start justify-between gap-3">
            <div>
              <h2 className="text-sm font-semibold text-[#1A2236] dark:text-stone-50">Sales, last 7 days</h2>
              <p className="mt-0.5 text-xs text-[#5F6B80] dark:text-stone-400">Bill value in ₹ thousands</p>
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
                <span className="text-[11px] tabular-nums text-[#5F6B80] opacity-100 transition-opacity dark:text-stone-400 lg:opacity-0 lg:group-hover:opacity-100">₹{day.amtK}k</span>
                <div
                  className={`bar-grow w-full max-w-10 rounded-t-md transition-colors ${
                    i === weekData.length - 1
                      ? 'bg-[#1B2B4B] dark:bg-gradient-to-t dark:from-[#C8952E] dark:to-[#D4A847]'
                      : 'bg-[#C8D1DF] group-hover:bg-[#D99A2B]/50 dark:bg-stone-700 dark:group-hover:bg-[#D4A847]/50'
                  }`}
                  style={{ height: `${day.amount > 0 ? Math.max(4, (day.amtK / max) * 82) : 0}%`, '--i': i }}
                  title={`${day.date}: ₹ ${money(day.amount)} (${day.bills} bills)`}
                />
              </div>
            ))}
          </div>
          <div className="mt-2 flex gap-3 sm:gap-5">
            {weekData.map(day => <span key={day.date} className="flex-1 text-center text-[11px] text-[#5F6B80] dark:text-stone-400">{day.day ?? day.date}</span>)}
            {!weekData.length && <span className="w-full text-center text-xs text-[#5F6B80] dark:text-stone-400">Sales data unavailable</span>}
          </div>
        </section>
      )}

        {shown('inventory_mix') && (
<div data-panel="inventory_mix" className="h-full">
          {hasInventoryValue ? (
            <Suspense fallback={
              <div className={`${panel} flex h-full min-h-48 flex-col gap-3`} role="status" aria-busy="true">
                <span className="sr-only">Loading…</span>
                <SkeletonLine className="h-4 w-32" /><SkeletonLine className="h-3 w-48" /><SkeletonLine className="mx-auto mt-4 h-32 w-32 !rounded-full" />
              </div>}>
              <Pattern
                data={inventoryMixData}
                config={inventoryMixConfig}
                title="Inventory Mix"
                description="Dynamic share of stock value"
                dataKey="share"
                nameKey="category"
                className="h-full flex flex-col justify-between"
              />
            </Suspense>
          ) : (
            <div className={`${panel} flex h-full min-h-48 flex-col justify-center`}>
              <h2 className="text-sm font-semibold text-[#1A2236] dark:text-stone-50">Inventory Mix</h2>
              <p className="mt-2 text-xs text-[#5F6B80] dark:text-stone-400">
                {items ? 'No inventory value on record.' : 'Inventory data unavailable.'}
              </p>
            </div>
          )}
        </div>
      )}
      </div>

      {/* ═══ STOCK MOVEMENT ═══ */}
      {shown('stock_movement') && (
<section data-panel="stock_movement" className="rounded-2xl border p-5 animate-fadeInUp animate-fadeInUp-d5
                          bg-[#FFFFFF] border-[#DDE3EC] shadow-[0_8px_30px_rgba(72,53,38,0.06)]
                          dark:glass-card dark:border-[#2A2A2A] dark:bg-transparent">
        <div className="mb-5 flex items-start justify-between gap-3">
          <div>
            <h2 className="text-sm font-semibold text-[#1A2236] dark:text-stone-50">Stock movement</h2>
            <p className="mt-0.5 text-xs text-[#5F6B80] dark:text-stone-400">Today's inventory flow across the store, in bottles</p>
          </div>
          <LiveBadge live={stockMovement !== null} />
        </div>
        <div className="grid grid-cols-1 gap-6 sm:grid-cols-3">
          {(stockMovement?.flow ?? []).map(item => (
            <div key={item.label}>
              <p className="text-xs text-[#5F6B80] dark:text-stone-400">{item.label}</p>
              <p className="mt-1 text-xl font-semibold tabular-nums text-[#1A2236] dark:text-stone-50">{item.value}</p>
              <div className="mt-2 h-1.5 rounded-full bg-muted"><div className={`h-full rounded-full ${item.color}`} style={{ width: `${item.pct}%` }} /></div>
            </div>
          ))}
          {!stockMovement && <p className="text-xs text-[#5F6B80] dark:text-stone-400">Stock movement data unavailable.</p>}
        </div>
      </section>
      )}

      {/* ═══ FOOTER STORE BANNER (COMPANY MASTER 1ST IMAGE) ═══ */}
      {shown('store_banner') && (
<section data-panel="store_banner" className="animate-fadeInUp animate-fadeInUp-d5">
        <div className="group relative overflow-hidden rounded-2xl dark:rounded-3xl h-48 sm:h-60 lg:h-72 img-zoom border border-[#DDE3EC] dark:border-[#2A2A2A] shadow-[0_8px_30px_rgba(72,53,38,0.06)] dark:shadow-card">
          <img
            src={storeImage}
            alt={company?.name ? `${company.name} store` : 'Company store'}
            loading="lazy" decoding="async"
            className="absolute inset-0 h-full w-full object-cover object-[center_35%]
                       transition-transform duration-400 ease-out group-hover:scale-[1.03]"
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
              <p className="mt-2 sm:mt-0 text-[10px] font-bold tracking-[0.25em] text-[#F0CF8A] uppercase">
                Retail Outlet{company?.licence_type ? ` · ${company.licence_type}` : ' · Company Master'}
              </p>
              <h3 className="mt-1 text-2xl sm:text-3xl font-bold text-white" style={{ fontFamily: 'var(--font-display)' }}>
                {company?.name || 'Company profile unavailable'}
              </h3>
              <p className="mt-1.5 sm:mt-2 text-xs sm:text-sm text-stone-300/85 max-w-lg">
                {company?.address || 'Company address unavailable'}
              </p>
            </div>
            <div className="flex flex-wrap items-center gap-2.5 text-xs text-white/90">
              {/* The backend's real state (GET /masters/company every 30 s), not a fixed timetable */}
              <span className={`inline-flex items-center gap-1.5 rounded-full backdrop-blur px-3.5 py-1.5 border font-semibold ${
                isOnline
                  ? 'bg-emerald-500/20 border-emerald-500/30 text-emerald-300'
                  : 'bg-rose-500/20 border-rose-500/30 text-rose-300'
              }`}>
                <span className={`h-1.5 w-1.5 rounded-full ${isOnline ? 'bg-emerald-400 animate-pulse' : 'bg-rose-400'}`} />
                {isOnline === false ? 'Server offline' : isOnline === null ? 'Connecting…' : 'Live data'}
              </span>
              {hours && (
                <span title="Karnataka liquor shop timings; closed on the Holiday Master dates"
                  className={`inline-flex items-center gap-1.5 rounded-full backdrop-blur px-3.5 py-1.5 border font-semibold ${
                    hours.open_now
                      ? 'bg-emerald-500/20 border-emerald-500/30 text-emerald-300'
                      : 'bg-rose-500/20 border-rose-500/30 text-rose-300'
                  }`}>
                  <span className={`h-1.5 w-1.5 rounded-full ${hours.open_now ? 'bg-emerald-400 animate-pulse' : 'bg-rose-400'}`} />
                  {hours.open_now ? 'Store Open' : 'Store Closed'}
                  {hours.today && <span className="font-medium opacity-80">· {hours.today}</span>}
                </span>
              )}
            </div>
          </div>
        </div>
      </section>
      )}
    </div>
  );
};

