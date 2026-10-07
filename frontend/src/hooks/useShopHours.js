// useShopHours(): whether the shop is open now (GET /company/hours, backend app/shop_hours.py): Karnataka liquor shop timings,
// closed on the Holiday Master dates. Checked every 5 minutes. null until known.
// Used by the Dashboard's store banner (the "Store Open / Closed" chip next to Live data).

import { useEffect, useState } from 'react';
import { getCompanyHours, } from '../lib/api';

const CHECK_MS = 5 * 60_000;

export function useShopHours() {
  const [hours, setHours] = useState(null);
  useEffect(() => {
    const load = () => getCompanyHours().then(setHours).catch(() => setHours(null));
    load();
    const timer = setInterval(load, CHECK_MS);
    return () => clearInterval(timer);
  }, []);
  return hours && hours.open_now !== null ? hours : null;
}
