// "Designed & developed by" credit with the Softvent logo, shown at the bottom of the sign-in card
// and the sidebar.

import softventLogo from '../../img/brand/softvent_logo.png';
import softventLogoGold from '../../img/brand/softvent_logo_gold.png';

const HEIGHT = { xs: 'h-7', sm: 'h-9', md: 'h-11', lg: 'h-16' };

export default function DevelopedBy({ className = '', size = 'md' }) {
  const img = `${HEIGHT[size]} w-auto select-none`;
  return (
    <div className={`flex flex-col items-center gap-1.5 ${className}`}>
      <span className="text-[10px] font-bold uppercase tracking-[0.12em] text-[#7B879B] dark:text-stone-500">
        Designed &amp; developed by
      </span>
      <img src={softventLogo} alt="Softvent Services Pvt. Ltd." className={`${img} dark:hidden`} draggable={false} />
      <img src={softventLogoGold} alt="Softvent Services Pvt. Ltd." className={`${img} hidden dark:block`} draggable={false} />
    </div>
  );
}
