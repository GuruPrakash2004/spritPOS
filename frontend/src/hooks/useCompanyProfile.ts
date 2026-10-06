// useCompanyProfile(): the company profile (name, address, GSTIN, …) from GET /masters/company.
//
// Used by the header, login page, dashboard and printouts. After Company Master saves, it calls
// notifyCompanyProfileUpdated(), which fires a window event so every mounted hook re-fetches.

import { useEffect, useState } from 'react';
import { getCompany, type ApiCompany } from '../lib/api';

export const COMPANY_PROFILE_UPDATED = 'company-profile-updated';

export const notifyCompanyProfileUpdated = () => {
  window.dispatchEvent(new Event(COMPANY_PROFILE_UPDATED));
};

export const useCompanyProfile = () => {
  const [company, setCompany] = useState<ApiCompany | null>(null);
  const [isOnline, setIsOnline] = useState<boolean | null>(null);

  useEffect(() => {
    const load = () => getCompany()
      .then(profile => {
        setCompany(profile);
        setIsOnline(true);
      })
      .catch(() => {
        setCompany(null);
        setIsOnline(false);
      });
    load();
    const interval = setInterval(load, 30000);
    window.addEventListener('focus', load);
    window.addEventListener(COMPANY_PROFILE_UPDATED, load);
    return () => {
      clearInterval(interval);
      window.removeEventListener('focus', load);
      window.removeEventListener(COMPANY_PROFILE_UPDATED, load);
    };
  }, []);

  return { company, isOnline };
};