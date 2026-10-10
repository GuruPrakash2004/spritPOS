// Screen preferences kept only in this browser (localStorage), never in the database: they follow the computer, not the shop.
// Previous purchase bills: how many past days the History panel lists (edited in the "Show last N days" box of the History panel itself).

const DAYS_KEY = 'vlqms_purchase_history_days';
export const DEFAULT_PURCHASE_DAYS = 10;

/** Days of purchase bills the History panel shows; 0 = the latest 60 bills. */
export const getPurchaseDays = () => {
  try {
    const raw = localStorage.getItem(DAYS_KEY);
    if (raw === null) return DEFAULT_PURCHASE_DAYS;
    return Math.max(0, Math.min(3650, Math.floor(Number(raw)) || 0));
  } catch { return DEFAULT_PURCHASE_DAYS; }
};

export const setPurchaseDays = (days) => {
  try { localStorage.setItem(DAYS_KEY, String(Math.max(0, Math.min(3650, Math.floor(Number(days)) || 0)))); } catch { /* storage blocked: not remembered */ }
};
