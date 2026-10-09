// Placeholders for data that is still loading, used instead of spinners or an empty gap so the page keeps its
// shape (no layout jump when the data arrives). The pulse is the `.skeleton` class in index.css and stops for
// reduced motion. Each one carries a visually hidden "Loading…" for screen readers.

/** One grey bar. */
export function SkeletonLine({ className = 'h-3 w-full' }) {
  return <div className={`skeleton ${className}`} aria-hidden="true" />;
}

const WIDTHS = ['w-3/4', 'w-1/2', 'w-2/3', 'w-5/6', 'w-1/3'];

/** Table body rows: `rows` rows of `cols` cells, as a <tbody> child. */
export function SkeletonTableRows({ rows = 6, cols = 4 }) {
  return Array.from({ length: rows }, (_, r) => (
    <tr key={r} aria-hidden={r > 0 || undefined}>
      {Array.from({ length: cols }, (_, c) => (
        <td key={c} className="px-4 py-3">
          {r === 0 && c === 0 && <span className="sr-only" role="status">Loading…</span>}
          <SkeletonLine className={`h-3 ${WIDTHS[(r + c) % WIDTHS.length]}`} />
        </td>
      ))}
    </tr>
  ));
}

/** A whole screen while its code or first data loads: a title bar and a panel of rows. */
export function ScreenSkeleton({ rows = 7 }) {
  return (
    <div role="status" aria-busy="true" className="mx-auto max-w-6xl">
      <span className="sr-only">Loading…</span>
      <div className="mb-5 space-y-2" aria-hidden="true">
        <SkeletonLine className="h-5 w-48" />
        <SkeletonLine className="h-3 w-72 max-w-full" />
      </div>
      <div className="space-y-3 rounded-2xl border border-line p-5" aria-hidden="true">
        {Array.from({ length: rows }, (_, i) => <SkeletonLine key={i} className={`h-4 ${WIDTHS[i % WIDTHS.length]}`} />)}
      </div>
    </div>
  );
}
