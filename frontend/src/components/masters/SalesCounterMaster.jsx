// Sales Counter Master (/masters/sales-counters): POS counters with their bill prefix and system user. The screen is the
// generic master screen drawn from the Doc Designer's definition of MstSC (hooks/useScreenFields.js). The legacy app needs
// a counter before any Sales Invoice, so saving a counter opens Sales Invoice setup (onSave).

import MasterCrud from './MasterCrud';
import { counterFromApi } from '../../data/salesCounters';

// Only used when the database has no definition for this master (offline, no designer rows).
const FALLBACK = [
  { key: 'code', label: 'SC code', required: true },
  { key: 'name', label: 'SC name', required: true },
  { key: 'prefix', label: 'Bill prefix', required: true },
];

export default function SalesCounterMaster({ onSave }) {
  return (
    <MasterCrud title="Sales Counter Master" subtitle="POS counters, their bill prefix and the machine that runs them."
      path="sales-counters" fields={FALLBACK} onSaved={saved => onSave?.(counterFromApi(saved))} />
  );
}
