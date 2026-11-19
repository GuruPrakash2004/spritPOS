// "Designed & developed by" credit card with the Softvent logo and contact details, shown at the bottom of the
// sign-in card, the sidebar and About. A navy panel (the sidebar's theme colour) in both light and dark mode, so the
// gold-and-ivory logo always sits on a dark ground. size="xs" is the compact card that fits the sidebar exactly:
// one-line heading, small logo and single-line contact rows.

import { Mail, Phone } from 'lucide-react';
import softventLogoGold from '../../img/brand/softvent_logo_gold.png';

const HEIGHT = { xs: 'h-5', sm: 'h-8', md: 'h-10', lg: 'h-14' };

export default function DevelopedBy({ className = '', size = 'md', bare = false }) {
  const compact = size === 'xs';
  // bare: just the gold logo, for a panel that already supplies its own dark ground and credit text (Dashboard banner).
  if (bare) {
    return <img src={softventLogoGold} alt="Softvent Services Pvt. Ltd." className={`${HEIGHT[size]} w-auto select-none ${className}`} draggable={false} />;
  }
  return (
    <div
      className={`flex w-full flex-col items-center rounded-xl border border-[#3A1622] bg-gradient-to-b from-[#64283A] to-[#3A1622] text-center shadow-sm dark:border-[#2A2A2A] dark:from-[#1A1A1A] dark:to-[#141414] ${compact ? 'gap-1 px-2 py-2' : 'gap-2 rounded-2xl px-4 py-3.5'} ${className}`}
    >
      <span className={`whitespace-nowrap font-bold uppercase text-[#F0E2E6]/80 dark:text-[#D4A847]/70 ${compact ? 'text-[8px] tracking-[0.12em]' : 'text-[10px] tracking-[0.18em]'}`}>
        Designed &amp; developed by
      </span>
      <a href="https://softvent.com/" target="_blank" rel="noopener noreferrer" title="Open softvent.com" className="rounded focus-visible:outline focus-visible:outline-2 focus-visible:outline-[#F6E3B4]">
        <img src={softventLogoGold} alt="Softvent Services Pvt. Ltd." className={`${HEIGHT[size]} w-auto select-none`} draggable={false} />
      </a>
      <div className={`w-full whitespace-nowrap border-t border-white/15 font-semibold text-[#F6E3B4] ${compact ? 'pt-1 text-[9.5px]' : 'pt-2 text-[11px]'}`}>
        <p className="flex items-center justify-center gap-1">
          <Phone className={`shrink-0 ${compact ? 'h-2.5 w-2.5' : 'h-3 w-3'}`} aria-hidden="true" />
          <span>9945036934 <span className="text-white/40">/</span> 9880606365</span>
        </p>
        <p className={`flex items-center justify-center gap-1 ${compact ? 'mt-0.5' : 'mt-1'}`}>
          <Mail className={`shrink-0 ${compact ? 'h-2.5 w-2.5' : 'h-3 w-3'}`} aria-hidden="true" />
          <a href="mailto:mail@softvent.com" className="hover:underline">mail@softvent.com</a>
        </p>
      </div>
    </div>
  );
}
