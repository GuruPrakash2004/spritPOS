// The Softvent company logo, used everywhere the app shows its brand (sidebar, login page).
//
// Two bundled variants in src/img/brand/: the original (maroon + dark grey) for light backgrounds and
// a gold version (gold swoosh and S, ivory lettering) that matches the black-and-gold dark theme. On normal app surfaces it switches
// with the app's dark mode; pass onDark when the logo sits on a dark photo or panel in both themes.
// showText adds the product name "vLQMS" next to the logo. The browser-tab icon is public/favicon.png.

import React from 'react';
import softventLogo from '../../img/brand/softvent_logo.png';
import softventLogoGold from '../../img/brand/softvent_logo_gold.png';

interface VlqmsLogoProps {
  size?: 'xs' | 'sm' | 'md' | 'lg';
  showText?: boolean;
  className?: string;
  onDark?: boolean;
}

// Logo height per size; the width follows the logo's 3:1 proportions.
const HEIGHT = { xs: 'h-7', sm: 'h-9', md: 'h-11', lg: 'h-16' } as const;

export const VlqmsLogo: React.FC<VlqmsLogoProps> = ({ size = 'md', showText = false, className = '', onDark = false }) => {
  const img = `${HEIGHT[size]} w-auto select-none`;
  return (
    <div className={`inline-flex items-center gap-2.5 ${className}`}>
      {onDark ? (
        <img src={softventLogoGold} alt="Softvent Services Pvt. Ltd." className={`${img} drop-shadow-[0_1px_3px_rgba(0,0,0,0.6)]`} draggable={false} />
      ) : (
        <>
          <img src={softventLogo} alt="Softvent Services Pvt. Ltd." className={`${img} dark:hidden`} draggable={false} />
          <img src={softventLogoGold} alt="Softvent Services Pvt. Ltd." className={`${img} hidden dark:block`} draggable={false} />
        </>
      )}

      {showText && (
        <div className={`border-l pl-2.5 leading-tight ${onDark ? 'border-white/25' : 'border-[#D8CCBE] dark:border-[#2A2A2A]'}`}>
          <div className={`text-sm font-extrabold tracking-tight ${onDark ? 'text-[#E8C36A]' : 'text-[#64283A] dark:text-[#E8C36A]'}`}>vLQMS</div>
          <div className={`text-[9px] font-bold uppercase tracking-[0.18em] ${onDark ? 'text-stone-200/80' : 'text-[#966B24] dark:text-[#D4A847]'}`}>Cloud ERP</div>
        </div>
      )}
    </div>
  );
};

export default VlqmsLogo;
