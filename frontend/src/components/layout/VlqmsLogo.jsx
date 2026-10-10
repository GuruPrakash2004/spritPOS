// The SpiritPOS product logo, used everywhere the app shows its brand (sidebar, login page).
//
// Two bundled transparent PNGs (src/img/brand/): spiritpos_logo_transparent.png (navy lettering, for light surfaces)
// and spiritpos_logo_light.png (white lettering, for dark mode and photos). Both have no background, so no blending
// or card is needed. onDark forces the light one (on a photo or dark panel in both themes).
// showText / textOnly are kept for the existing call sites; the logo already carries the product name.
// The Softvent company logo (softvent_logo*.png) is no longer drawn here; import it directly where it is needed.
// The browser-tab icon is public/favicon.png.

import logoDarkText from '../../img/brand/spiritpos_logo_transparent.png';
import logoLightText from '../../img/brand/spiritpos_logo_light.png';

// Logo height per size; the width follows the image's proportions (about 2.3:1).
const HEIGHT = { xs: 'h-8', sm: 'h-11', md: 'h-14', lg: 'h-20' };

export const VlqmsLogo = ({ size = 'md', className = '', onDark = false }) => {
  const cls = `${HEIGHT[size]} w-auto select-none`;
  const alt = 'SpiritPOS · Liquor Retail Management System';
  return (
    <div className={`inline-flex items-center ${className}`}>
      {onDark
        ? <img src={logoLightText} alt={alt} className={`${cls} drop-shadow-[0_2px_10px_rgba(0,0,0,0.5)]`} draggable={false} />
        : <>
            <img src={logoDarkText} alt={alt} className={`${cls} dark:hidden`} draggable={false} />
            <img src={logoLightText} alt="" aria-hidden="true" className={`${cls} hidden dark:block`} draggable={false} />
          </>}
    </div>
  );
};

export default VlqmsLogo;
