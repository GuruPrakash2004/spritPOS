// The SpiritPOS product logo, used everywhere the app shows its brand (sidebar, login page).
//
// One bundled image (src/img/brand/spiritpos_logo.png: navy bottle, amber swoosh, white background). In light mode
// the white is blended away (mix-blend-multiply) so the logo sits straight on the page; in dark mode, and with
// onDark (on a photo or dark panel in both themes), it sits on a small white card so the navy lettering stays readable.
// showText / textOnly are kept for the existing call sites; the logo already carries the product name.
// The Softvent company logo (softvent_logo*.png) is no longer drawn here; import it directly where it is needed.
// The browser-tab icon is public/favicon.png.

import spiritLogo from '../../img/brand/spiritpos_logo.png';

// Logo height per size; the width follows the image's proportions (about 2.3:1).
const HEIGHT = { xs: 'h-8', sm: 'h-11', md: 'h-14', lg: 'h-20' };

// Dark surfaces: invert the picture (white → black, navy → light) and turn the hue back half way round so the amber
// swoosh stays amber, then `screen` blending drops the black, leaving light lettering and an amber swoosh with no box.
const DARK_LOOK = 'invert hue-rotate-180 brightness-110 mix-blend-screen';

export const VlqmsLogo = ({ size = 'md', className = '', onDark = false }) => (
  <div className={`inline-flex items-center ${onDark ? 'rounded-2xl bg-black/40 px-4 py-2 ring-1 ring-white/10 backdrop-blur-md' : ''} ${className}`}>
    <img src={spiritLogo} alt="SpiritPOS · Liquor Retail Management System"
      className={`${HEIGHT[size]} w-auto select-none ${onDark
        ? DARK_LOOK
        : 'mix-blend-multiply dark:invert dark:hue-rotate-180 dark:brightness-110 dark:mix-blend-screen'}`}
      draggable={false} />
  </div>
);

export default VlqmsLogo;
