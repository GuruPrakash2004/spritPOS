// Pages a long list: returns the current page's rows and the controls for <Pager />. The page returns to 1 whenever
// the list's length changes (a new filter or report run).
import { useEffect, useState } from 'react';

export const PAGE_SIZE = 100;

export function usePage<T>(rows: T[], size = PAGE_SIZE) {
  const [page, setPage] = useState(0);
  const pages = Math.max(1, Math.ceil(rows.length / size));
  // eslint-disable-next-line react-hooks/set-state-in-effect
  useEffect(() => { setPage(0); }, [rows.length, size]);
  const at = Math.min(page, pages - 1);
  return { slice: rows.slice(at * size, at * size + size), page: at, pages, setPage, from: at * size, total: rows.length, size };
}
