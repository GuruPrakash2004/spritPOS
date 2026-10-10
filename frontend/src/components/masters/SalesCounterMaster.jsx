// Sales Counter Master (/masters/sales-counters): POS counters with their bill prefix and system user. The screen is the
// generic master screen drawn from the Doc Designer's definition of MstSC (hooks/useScreenFields.js). The legacy app needs
// a counter before any Sales Invoice, so saving a counter opens Sales Invoice setup (onSave).
// Opened from Sales Invoice with no counter defined (startNew), it starts a new counter with System Name set to the
// machine's name (GET /utilities/about → system_name, else ADMIN) and the prefix S1.

import { useEffect, useState } from 'react';
import MasterCrud from './MasterCrud';
import { counterFromApi } from '../../data/salesCounters';
import { getAbout } from '../../lib/api';

// Only used when the database has no definition for this master (offline, no designer rows).
const FALLBACK = [
  { key: 'code', label: 'SC code', required: true },
  { key: 'name', label: 'SC name', required: true },
  { key: 'prefix', label: 'Bill prefix', required: true },
];

export default function SalesCounterMaster({ onSave, startNew = false }) {
  // Wait for the system name so the new-counter form opens already filled in.
  const [defaults, setDefaults] = useState(null);
  useEffect(() => {
    if (!startNew) return;
    getAbout().then(a => a?.system_name || 'ADMIN').catch(() => 'ADMIN')
      .then(system_name => setDefaults({ system_name, prefix: 'S1' }));
  }, [startNew]);
  if (startNew && !defaults) return null;
  return (
    <MasterCrud title="Sales Counter Master" subtitle="POS counters, their bill prefix and the machine that runs them."
      path="sales-counters" fields={FALLBACK} onSaved={saved => onSave?.(counterFromApi(saved))}
      startNew={startNew} newDefaults={defaults ?? undefined} />
  );
}
