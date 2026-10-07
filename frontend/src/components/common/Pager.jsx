// Controls for usePage() (./usePage.js). Long lists show one page at a time so a table of thousands of rows opens fast (the rendering, not the
// fetch, is what makes big lists slow). usePage(rows, size) returns the current page's rows and the controls
// for <Pager />; the page returns to 1 whenever the list itself changes (a new filter or report run).
import { ChevronLeft, ChevronRight } from 'lucide-react';


export default function Pager({ p }) {
  if (p.pages <= 1) return null;
  const btn = 'inline-flex h-7 w-7 items-center justify-center rounded-full border border-line text-stone-600 hover:bg-subtle disabled:opacity-40 dark:text-stone-300';
  return (
    <nav aria-label="Pages" className="flex items-center justify-end gap-2 border-t border-line px-3 py-2 text-xs tabular-nums text-stone-500">
      <span>{p.from + 1}–{Math.min(p.from + p.size, p.total)} of {p.total}</span>
      <button type="button" className={btn} disabled={p.page === 0} onClick={() => p.setPage(p.page - 1)} aria-label="Previous page"><ChevronLeft size={14} /></button>
      <span>Page {p.page + 1} / {p.pages}</span>
      <button type="button" className={btn} disabled={p.page >= p.pages - 1} onClick={() => p.setPage(p.page + 1)} aria-label="Next page"><ChevronRight size={14} /></button>
    </nav>
  );
}
