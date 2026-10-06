// useShopHours(): whether the shop is open now, from its opening hours on Google Maps (GET /company/hours, backend
// app/shop_hours.py). Checked every 5 minutes. null until known, or when Google Maps isn't set up or has no hours.
// Used by the Dashboard's store banner (the "Store Open / Closed" chip next to Live data).

import { useEffect, useState } from 'react';
import { api, type ApiShopHours } from '../lib/api';

const CHECK_MS = 5 * 60_000;

export function useShopHours(): ApiShopHours | null {
  const [hours, setHours] = useState<ApiShopHours | null>(null);
  useEffect(() => {
    const load = () => api<ApiShopHours>('/company/hours').then(setHours).catch(() => setHours(null));
    load();
    const timer = setInterval(load, CHECK_MS);
    return () => clearInterval(timer);
  }, []);
  return hours && hours.open_now !== null ? hours : null;
}
