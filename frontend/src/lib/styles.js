// Shared Tailwind class strings (the vLQMS design system): panels, fields, buttons, tables.
//
// Use these instead of writing new class lists so every screen looks the same. Colours are hex values
// with explicit dark: variants (light: burgundy #1B2B4B on cream; dark: gold #D4A847 on #141414).
// money() formats a rupee amount for tables.

const btn = 'inline-flex items-center justify-center gap-1.5 rounded-lg px-3 py-2 text-xs font-semibold whitespace-nowrap transition duration-150 active:scale-[0.98] disabled:pointer-events-none disabled:opacity-45';

export const panel = 'rounded-2xl border border-[#DDE3EC] bg-[#FFFFFF] shadow-[0_8px_30px_rgba(72,53,38,0.06)] dark:border-[#2A2A2A] dark:bg-[#141414] dark:shadow-card';
export const field = 'w-full rounded-xl border border-[#D5DCE7] bg-[#FFFFFF] px-3 py-2 text-xs text-[#1A2236] outline-none transition placeholder:text-[#9AA5B8] hover:border-[#CBBEAF] focus:border-[#1B2B4B] focus:ring-[3px] focus:ring-[#1B2B4B]/10 disabled:cursor-not-allowed disabled:bg-[#F0F3F8] disabled:opacity-70 dark:border-[#2A2A2A] dark:bg-[#1A1A1A] dark:text-stone-50 dark:placeholder:text-stone-600 dark:hover:border-[#3A3A3A] dark:focus:border-[#D4A847] dark:focus:ring-[#D4A847]/20';
// Added to a field's classes while it holds a mistake (red border + shake; .claude/rules/popups-and-errors.md).
export const fieldError = '!border-red-500 !ring-2 !ring-red-500/20 dark:!border-red-400 dark:!ring-red-400/20 animate-shake';
export const label = 'mb-1.5 block text-xs font-semibold uppercase tracking-[0.08em] text-[#5F6B80] dark:text-stone-400';
export const primaryButton = `${btn} bg-[#1B2B4B] text-white shadow-[0_4px_12px_rgba(100,40,58,0.18)] hover:bg-[#121D36] dark:bg-gradient-to-r dark:from-[#D4A847] dark:via-[#F5D88A] dark:to-[#C8952E] dark:text-[#180E09] dark:font-extrabold dark:shadow-[0_0_18px_rgba(212,168,71,0.4)] dark:hover:brightness-110`;
export const secondaryButton = `${btn} border border-[#D99A2B] bg-transparent text-[#1B2B4B] hover:bg-[#D99A2B]/8 dark:border-[#D4A847]/40 dark:text-[#D4A847] dark:hover:bg-[#D4A847]/10`;
export const ghostButton = `${btn} text-[#5F6B80] hover:bg-[#EAEEF5] hover:text-[#1A2236] dark:text-stone-400 dark:hover:bg-[#1C1C1C] dark:hover:text-stone-200`;
export const dangerButton = `${btn} bg-[#9A4B45] text-white shadow-[0_4px_12px_rgba(154,75,69,0.2)] hover:bg-[#823D38] dark:bg-red-600 dark:hover:bg-red-700`;
export const exportButton = `${btn} border border-emerald-600/35 bg-emerald-50 text-emerald-800 shadow-xs hover:bg-emerald-100/90 active:scale-[0.98] dark:border-emerald-500/40 dark:bg-emerald-950/40 dark:text-emerald-300 dark:hover:bg-emerald-900/50 dark:hover:brightness-110`;
export const printButton = `${btn} border border-sky-600/35 bg-sky-50 text-sky-800 shadow-xs hover:bg-sky-100/90 active:scale-[0.98] dark:border-sky-500/40 dark:bg-sky-950/40 dark:text-sky-300 dark:hover:bg-sky-900/50 dark:hover:brightness-110`;
export const iconButton = 'inline-flex h-8 w-8 items-center justify-center rounded-lg text-[#5F6B80] transition hover:bg-[#EAEEF5] hover:text-[#1A2236] active:scale-[0.96] disabled:pointer-events-none disabled:opacity-45 dark:text-stone-400 dark:hover:bg-[#1C1C1C] dark:hover:text-stone-50';
export const tableHead = 'sticky top-0 z-[1] whitespace-nowrap border-b border-[#DDE3EC] bg-[#F0F3F8] text-left text-[11px] font-semibold uppercase tracking-[0.04em] text-[#5F6B80] dark:border-line dark:bg-subtle dark:text-stone-400';
export const tableBody = 'divide-y divide-[#E8ECF3] dark:divide-line-soft';
export const tableRow = 'transition-colors hover:bg-[#F7F9FC] dark:hover:bg-subtle/70';
export const pageHeader = 'flex flex-wrap items-end justify-between gap-3';
export const pageTitle = 'text-xl font-bold tracking-tight text-[#1A2236] dark:text-stone-50';
export const pageSub = 'mt-0.5 text-xs text-[#5F6B80] dark:text-stone-400';
export const sectionTitle = 'text-sm font-semibold text-[#1A2236] dark:text-stone-50';

 
const TONES = {
  neutral: 'bg-[#EBEFF5] text-[#5F6B80] dark:bg-muted dark:text-stone-200',
  accent: 'bg-[#E6EBF4] text-[#1B2B4B] dark:bg-[#D4A847]/12 dark:text-[#E8C36A]',
  success: 'bg-[#EAF1EA] text-[#426147] dark:bg-emerald-500/12 dark:text-emerald-300',
  warning: 'bg-[#F8EDDA] text-[#875D24] dark:bg-amber-500/15 dark:text-amber-300',
  danger: 'bg-[#F5E5E2] text-[#823D38] dark:bg-red-500/12 dark:text-red-300',
  info: 'bg-[#E7EEF3] text-[#445F6F] dark:bg-sky-500/12 dark:text-sky-300',
};
export const badge = (tone = 'neutral') => `inline-flex items-center gap-1 rounded-md px-1.5 py-0.5 text-[11px] font-medium ${TONES[tone]}`;

// Indian-format rupee amount; "—" for a value the server hid from this role (null) or doesn't have.
export const money = (n) =>
  typeof n === 'number' && Number.isFinite(n) ? n.toLocaleString('en-IN', { minimumFractionDigits: 2, maximumFractionDigits: 2 }) : '—';

// The desktop vLQMS colours: a mandatory field's caption is red, a read-only (worked-out or locked) field's blue.
// A field that is both is read-only, so blue wins. `fieldTone(required, readOnly)` gives the text colour classes.
export const toneMandatory = 'text-red-600 dark:text-red-400';
export const toneReadonly = 'text-blue-600 dark:text-blue-400';
export const fieldTone = (required, readOnly) => (readOnly ? toneReadonly : required ? toneMandatory : '');
