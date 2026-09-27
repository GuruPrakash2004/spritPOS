import imgLoginHero from '../../img/login_hero.jpg';
// High-resolution assets for the Heritage Luxury Light & Dark Enterprise theme.
import bannerDistillery from '../../img/banner_distillery.jpg';
import imgRoyalChallenge from '../../img/products/royal_challenge.jpg';
import imgVinGreenLabel from '../../img/products/vin_green_label.jpg';
import imgMcDowells from '../../img/products/mcdowells.jpg';
import imgKingfisher from '../../img/products/kingfisher.jpg';
import imgCompanyStore from '../../img/company_store_1.webp';

export const IMAGES = {
  login: {
    hero: imgLoginHero,
  },
  dashboard: {
    hero: bannerDistillery,
    warehouse: 'https://images.unsplash.com/photo-1516594915697-87eb3b1c14ea?w=1600&q=85&auto=format&fit=crop',
    store: imgCompanyStore,
    barAtmosphere: imgCompanyStore,
  },
  products: {
    royalChallenge: imgRoyalChallenge,
    vinGreenLabel: imgVinGreenLabel,
    mcdowells: imgMcDowells,
    kingfisher: imgKingfisher,
    whisky: 'https://images.unsplash.com/photo-1527281400683-1aae145822b8?w=600&q=80&auto=format&fit=crop',
    wine: 'https://images.unsplash.com/photo-1510812431401-41d2bd2722f3?w=600&q=80&auto=format&fit=crop',
    cocktail: 'https://images.unsplash.com/photo-1514362545857-3bc16c4c7d1b?w=600&q=80&auto=format&fit=crop',
    beer: 'https://images.unsplash.com/photo-1535958636474-b021ee887b13?w=600&q=80&auto=format&fit=crop',
  },
} as const;

// Match item image by name, code, or item group
export function productImage(groupOrName: string, nameOrCode?: string): string {
  const query = ((groupOrName || "") + " " + (nameOrCode || "")).toLowerCase();

  // 1. Specific brand / item name matches
  if (query.includes('royal challenge') || query.includes('rc-750')) {
    return IMAGES.products.royalChallenge;
  }
  if (query.includes('green label') || query.includes('vin-750') || query.includes('vin green')) {
    return IMAGES.products.vinGreenLabel;
  }
  if (query.includes('mcdowell') || query.includes('mcd1') || query.includes('no.1')) {
    return IMAGES.products.mcdowells;
  }
  if (query.includes('kingfisher') || query.includes('kf-') || query.includes('kfu-')) {
    return IMAGES.products.kingfisher;
  }

  // 2. Generic category fallbacks
  if (query.includes('beer')) return IMAGES.products.beer;
  if (query.includes('wine')) return IMAGES.products.wine;
  if (query.includes('whisky') || query.includes('rum') || query.includes('brandy') || query.includes('scotch')) return IMAGES.products.whisky;
  return IMAGES.products.cocktail;
}
