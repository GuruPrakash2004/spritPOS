// "Designed & developed by" credit with the Softvent logo, shown at the bottom of the sign-in card
// and the sidebar.

import VlqmsLogo from './VlqmsLogo';

export default function DevelopedBy({ className = '' }: { className?: string }) {
  return (
    <div className={`flex flex-col items-center gap-1.5 ${className}`}>
      <span className="text-[10px] font-bold uppercase tracking-[0.12em] text-[#8E847A] dark:text-stone-500">
        Designed &amp; developed by
      </span>
      <VlqmsLogo size="xs" />
    </div>
  );
}
