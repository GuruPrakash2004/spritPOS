// Supplier Master (/masters/suppliers). The screen is the generic master screen: its fields, captions, order and rules
// come from the Doc Designer's definition of MstSUPP, read live from the database (hooks/useScreenFields.js), so a field
// added, renamed, hidden or made mandatory there shows here without a release. Opened with ?new=1 (the Purchase Bill's
// New supplier button) it starts on a blank record; a new supplier starts in the shop's own state.

import { useMemo } from 'react';
import MasterCrud from './MasterCrud';
import { useCompanyProfile } from '../../hooks/useCompanyProfile';

// Only used when the database has no definition for this master (offline, no designer rows): code and name.
const FALLBACK = [
  { key: 'code', label: 'Supplier Code', required: true },
  { key: 'name', label: 'Supplier Name', required: true },
];

export default function SupplierMaster() {
  const { company } = useCompanyProfile();
  const startNew = useMemo(() => new URLSearchParams(window.location.search).has('new'), []);
  const defaults = useMemo(() => (company?.state ? { state: company.state } : undefined), [company]);
  return (
    <MasterCrud title="Supplier Master" subtitle="Distributor accounts, excise warehouse mappings and GST details."
      path="suppliers" fields={FALLBACK} newDefaults={defaults} startNew={startNew && !!company} />
  );
}
