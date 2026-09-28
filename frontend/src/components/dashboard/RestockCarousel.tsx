// Dashboard "Restock immediately" panel: up to 10 items as a sliding card carousel.
//
// Rows come from GET /reports/restock (backend/app/routers/reports.py), already sorted by restock
// priority: fast sellers that are out of stock or about to run out come first. The window is always the
// last N days up to today, and with SQL Server connected the stock and sales are read live (real time).
// The top 10 items that sell are shown: those that need restock first, then the next-lowest-stock
// sellers. Items that didn't sell are never shown; if nothing sold, the panel says so.
//
// Carousel: a horizontal scroll-snap track (swipe/trackpad work natively), prev/next arrows, clickable
// dots (one per scroll stop), ←/→ keys on the focused track, and a gentle auto-advance every 5 s that
// pauses on hover, focus or touch and is off when the user prefers reduced motion.

import { useCallback, useEffect, useRef, useState, type ReactNode } from 'react';
import { ChevronLeft, ChevronRight } from 'lucide-react';
import type { ApiRestockRow, ApiRestockStatus } from '../../lib/api';
import { iconButton, money } from '../../lib/styles';
import { distinctProductPhotos } from '../../constants/images';

const MAX_CARDS = 10;
const AUTO_ADVANCE_MS = 5000;

const BADGE: Record<ApiRestockStatus, string> = {
  OUT_OF_STOCK: 'bg-[#F6E3E1] text-[#9A4B45] dark:bg-red-600/90 dark:text-white',
  CRITICAL: 'bg-[#FBE3CF] text-[#A04A12] dark:bg-orange-500/90 dark:text-black',
  LOW: 'bg-[#F8EDDA] text-[#875D24] dark:bg-amber-500/90 dark:text-black',
  OK: 'bg-[#EAF1EA] text-[#426147] dark:bg-emerald-500/90 dark:text-white',
};

const num = (n: number) => String(Math.round(n * 100) / 100);
const clean = (s: string) => s.replace(/^\d+[.\s_-]*/, '').trim();
const shortDate = (iso: string) => new Date(`${iso}T00:00:00`).toLocaleDateString('en-IN', { day: 'numeric', month: 'short' });

function badgeText(r: ApiRestockRow): string {
  if (r.status === 'OUT_OF_STOCK') return 'Out of stock';
  if (r.status === 'CRITICAL') return r.days_left !== null && r.days_left < 1 ? '< 1 day left' : `~${Math.round(r.days_left ?? 0)} days left`;
  if (r.status === 'LOW') return `${num(r.qoh)} left`;
  if (r.days_left !== null) return `~${Math.round(r.days_left)} days left`;
  return `${num(r.qoh)} in stock`;
}

type Props = {
  rows: ApiRestockRow[] | null; // null = the API call failed
  days: number;                 // sales window the rows were computed over
  badge: ReactNode;             // the Live / Unavailable badge
};

export default function RestockCarousel({ rows, days, badge }: Props) {
  const cards = (rows ?? []).filter(r => r.avg_daily > 0).slice(0, MAX_CARDS);
  const healthy = rows !== null && !cards.some(r => r.needs_restock);
  // One photo per card, never repeated (see constants/images.ts).
  const photos = distinctProductPhotos(cards.map(r => ({ code: r.item_code, name: r.item_name, group: r.group })));

  const trackRef = useRef<HTMLDivElement>(null);
  const [stop, setStop] = useState(0);        // index of the first visible card
  const [stops, setStops] = useState(1);      // number of distinct scroll positions
  const [canPrev, setCanPrev] = useState(false);
  const [canNext, setCanNext] = useState(false);
  // Auto-advance pauses while the pointer is over the panel, focus is inside the track, or a touch is active.
  const [hovered, setHovered] = useState(false);
  const [focused, setFocused] = useState(false);
  const [touching, setTouching] = useState(false);
  const paused = hovered || focused || touching;
  const touchResume = useRef<number | undefined>(undefined);

  // Width of one card plus the gap, from the rendered layout.
  const step = () => {
    const el = trackRef.current;
    const first = el?.children[0] as HTMLElement | undefined;
    const second = el?.children[1] as HTMLElement | undefined;
    if (!first) return 1;
    return second ? second.offsetLeft - first.offsetLeft : first.offsetWidth;
  };

  const measure = useCallback(() => {
    const el = trackRef.current;
    if (!el) return;
    const s = step();
    const visible = Math.max(1, Math.round(el.clientWidth / s));
    setStops(Math.max(1, el.children.length - visible + 1));
    setStop(Math.round(el.scrollLeft / s));
    setCanPrev(el.scrollLeft > 4);
    setCanNext(el.scrollLeft + el.clientWidth < el.scrollWidth - 4);
  }, []);

  const goTo = useCallback((i: number) => {
    const el = trackRef.current;
    if (el) el.scrollTo({ left: i * step(), behavior: 'smooth' });
  }, []);

  const next = useCallback((loop = false) => {
    const el = trackRef.current;
    if (!el) return;
    const atEnd = el.scrollLeft + el.clientWidth >= el.scrollWidth - 4;
    if (atEnd) { if (loop) goTo(0); return; }
    goTo(Math.round(el.scrollLeft / step()) + 1);
  }, [goTo]);

  const prev = () => {
    const el = trackRef.current;
    if (el) goTo(Math.max(0, Math.round(el.scrollLeft / step()) - 1));
  };

  // Re-measure when the cards change or the track's width changes (window resize, sidebar toggle).
  useEffect(() => {
    measure();
    const el = trackRef.current;
    if (!el || typeof ResizeObserver === 'undefined') return;
    const ro = new ResizeObserver(measure);
    ro.observe(el);
    return () => ro.disconnect();
  }, [measure, cards.length]);

  // Auto-advance, looping back to the first card; off for reduced motion or a single stop.
  useEffect(() => {
    const reduced = window.matchMedia?.('(prefers-reduced-motion: reduce)').matches;
    if (paused || reduced || stops <= 1) return;
    const t = window.setInterval(() => next(true), AUTO_ADVANCE_MS);
    return () => window.clearInterval(t);
  }, [paused, stops, next]);

  useEffect(() => () => window.clearTimeout(touchResume.current), []);

  if (rows !== null && rows.length === 0) return null;

  const noSales = rows !== null && cards.length === 0;
  const heading = healthy && !noSales ? 'Fast movers' : 'Restock immediately';
  // Days the shop has actually been selling within the window (a new shop has fewer than `days`).
  const spanDays = rows?.[0]?.window_days ?? days;
  const period = `last ${spanDays} day${spanDays === 1 ? '' : 's'}`;
  const live = rows?.[0]?.source === 'live' ? ' · live from store database' : '';
  const subtitle = rows === null
    ? 'Stock data is unavailable right now.'
    : noSales
      ? `No sales in the last ${days} days yet — nothing to restock.${live}`
      : healthy
        ? `Stock is healthy · fastest sellers · ${period}${live}`
        : `Fast sellers running out first · ${period}${live}`;

  return (
    <section className="animate-fadeInUp animate-fadeInUp-d2"
      onMouseEnter={() => setHovered(true)} onMouseLeave={() => setHovered(false)}>
      <div className="mb-4 flex items-end justify-between gap-3">
        <div>
          <p className="text-[10px] font-bold tracking-[0.25em] text-[#B08A45] dark:text-[#D4A847] uppercase">
            {healthy ? 'Best sellers' : 'Low stock'}
          </p>
          <h3 className="mt-1 text-lg font-bold text-[#29231F] dark:text-white">{heading}</h3>
          <p className="mt-0.5 text-xs text-[#70675F] dark:text-stone-400">{subtitle}</p>
        </div>
        <div className="flex items-center gap-1.5">
          {badge}
          {cards.length > 1 && (
            <>
              <button type="button" className={`${iconButton} disabled:opacity-30 disabled:pointer-events-none`} onClick={prev} disabled={!canPrev} aria-label="Previous items">
                <ChevronLeft size={16} />
              </button>
              <button type="button" className={`${iconButton} disabled:opacity-30 disabled:pointer-events-none`} onClick={() => next()} disabled={!canNext} aria-label="Next items">
                <ChevronRight size={16} />
              </button>
            </>
          )}
        </div>
      </div>

      {cards.length > 0 && (
        <>
          <div
            ref={trackRef}
            role="region"
            aria-roledescription="carousel"
            aria-label={heading}
            tabIndex={0}
            onScroll={measure}
            onFocus={() => setFocused(true)}
            onBlur={e => { if (!e.currentTarget.contains(e.relatedTarget as Node)) setFocused(false); }}
            onPointerDown={e => { if (e.pointerType !== 'mouse') { window.clearTimeout(touchResume.current); setTouching(true); } }}
            onPointerUp={e => { if (e.pointerType !== 'mouse') touchResume.current = window.setTimeout(() => setTouching(false), 6000); }}
            onPointerCancel={e => { if (e.pointerType !== 'mouse') touchResume.current = window.setTimeout(() => setTouching(false), 6000); }}
            onKeyDown={e => {
              if (e.key === 'ArrowRight') { e.preventDefault(); next(); }
              if (e.key === 'ArrowLeft') { e.preventDefault(); prev(); }
            }}
            className="relative flex gap-4 overflow-x-auto snap-x snap-mandatory scroll-smooth pb-1 outline-none rounded-2xl
                       focus-visible:ring-2 focus-visible:ring-[#B08A45]/50
                       [scrollbar-width:none] [&::-webkit-scrollbar]:hidden"
          >
            {cards.map((r, idx) => {
              const photo = photos[idx];
              return (
                <article key={r.item_code} aria-label={`${idx + 1} of ${cards.length}`}
                  className="group img-zoom card-3d w-[80%] shrink-0 snap-start overflow-hidden rounded-2xl border
                             sm:w-[calc((100%-1rem)/2)] lg:w-[calc((100%-3rem)/4)]
                             bg-[#FFFCF7] border-[#E3D9CD] dark:border-[#2A2A2A] dark:bg-[#141414]">
                  <div className="relative h-36 sm:h-44 overflow-hidden bg-[#F0EBE3] dark:bg-[#1A1A1A]">
                    <img src={photo.src} alt={r.item_name} draggable={false}
                      className="h-full w-full object-cover transition-transform duration-500 ease-out group-hover:scale-[1.035]"
                      style={{ objectPosition: photo.position }} loading="lazy" />
                    <span className="absolute top-3 left-3 rounded-full bg-[#29231F]/80 px-2 py-0.5 text-[10px] font-bold text-white tabular-nums shadow-md backdrop-blur-sm dark:bg-black/70">
                      #{idx + 1}
                    </span>
                    <span className={`absolute top-3 right-3 rounded-full px-2.5 py-1 text-[10px] font-bold uppercase tracking-wider shadow-md tabular-nums ${BADGE[r.status]}`}>
                      {badgeText(r)}
                    </span>
                    <div className="absolute inset-x-0 bottom-0 h-16 bg-gradient-to-t from-[#FFFCF7] dark:from-[#141414] to-transparent" />
                  </div>
                  <div className="p-4">
                    <p className="text-[10px] font-bold tracking-[0.15em] text-[#B08A45] dark:text-[#D4A847] uppercase">
                      {clean(r.group || '') || 'LIQUOR'}
                    </p>
                    <h4 className="mt-1 truncate text-sm font-semibold text-[#29231F] dark:text-white" title={r.item_name}>
                      {clean(r.item_name)}
                    </h4>
                    <div className="mt-3 flex items-center justify-between gap-2">
                      <span className="text-base font-extrabold tabular-nums text-[#29231F] dark:text-white">₹{money(r.mrp)}</span>
                      <span className="text-[10px] font-bold uppercase tracking-wider text-[#B08A45] dark:text-[#D4A847]" title="Suggested order: two weeks of sales, at least the reorder qty">
                        {r.needs_restock && r.suggested_qty > 0 ? `Order ${num(r.suggested_qty)}` : 'No order needed'}
                      </span>
                    </div>
                    <p className="mt-1 text-[10px] tabular-nums text-[#70675F] dark:text-stone-400">
                      {r.avg_daily > 0 ? `Sells ${num(r.avg_daily)}/day · ` : 'No sales · '}
                      {r.status === 'OUT_OF_STOCK' && r.last_sold ? `last sold ${shortDate(r.last_sold)}` : `sold ${num(r.sold_qty)} in ${spanDays} day${spanDays === 1 ? '' : 's'}`}
                    </p>
                  </div>
                </article>
              );
            })}
          </div>

          {stops > 1 && (
            <div className="mt-3 flex justify-center gap-1.5" role="tablist" aria-label="Choose items">
              {Array.from({ length: stops }, (_, i) => (
                <button key={i} type="button" role="tab" aria-selected={i === stop} aria-label={`Show from item ${i + 1}`}
                  onClick={() => goTo(i)}
                  className={`h-1.5 rounded-full transition-all duration-300 ${i === stop
                    ? 'w-5 bg-[#64283A] dark:bg-[#D4A847]'
                    : 'w-1.5 bg-[#D8CCBE] hover:bg-[#B08A45] dark:bg-[#2A2A2A] dark:hover:bg-[#D4A847]/60'}`} />
              ))}
            </div>
          )}
        </>
      )}
    </section>
  );
}
