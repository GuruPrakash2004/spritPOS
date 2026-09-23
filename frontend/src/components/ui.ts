// Shared Tailwind class strings for the API-backed screens (Version5 palette, explicit dark: variants).
export const panel = 'rounded-xl border border-[#e1c9b6] bg-[#fffaf5] shadow-sm dark:border-[#343b59] dark:bg-[#171b2b]';
export const field = 'rounded-lg border border-[#e1c9b6] bg-[#fffaf5] px-2.5 py-1.5 text-xs text-stone-800 outline-none focus:border-[#e8702a] disabled:opacity-60 dark:border-[#414a6e] dark:bg-[#20253a] dark:text-stone-100';
export const label = 'mb-1 block text-xs font-semibold text-stone-600 dark:text-stone-300';
export const primaryButton = 'flex items-center gap-1.5 rounded-lg bg-[#e8702a] px-3.5 py-2 text-xs font-bold text-white shadow-sm hover:bg-[#b84716] disabled:cursor-not-allowed disabled:opacity-40';
export const secondaryButton = 'flex items-center gap-1.5 rounded-lg border border-[#e1c9b6] bg-[#fffaf5] px-3.5 py-2 text-xs font-semibold text-stone-700 hover:border-[#e8702a] hover:text-[#b84716] disabled:cursor-not-allowed disabled:opacity-40 dark:border-[#414a6e] dark:bg-[#20253a] dark:text-stone-200 dark:hover:text-[#ffb07e]';
export const dangerButton = 'flex items-center gap-1.5 rounded-lg bg-red-600 px-3.5 py-2 text-xs font-bold text-white shadow-sm hover:bg-red-700 disabled:cursor-not-allowed disabled:opacity-40';
export const tableHead = 'sticky top-0 whitespace-nowrap bg-[#f1e3d8] text-left text-stone-600 dark:bg-[#20253a] dark:text-stone-300';
export const tableBody = 'divide-y divide-[#f1e3d8] dark:divide-[#343b59]';
export const pageTitle = 'text-lg font-bold text-stone-900 dark:text-stone-100';
export const pageSub = 'text-xs text-stone-500 dark:text-stone-400';

export const money = (n: number) => n.toLocaleString('en-IN', { minimumFractionDigits: 2, maximumFractionDigits: 2 });
