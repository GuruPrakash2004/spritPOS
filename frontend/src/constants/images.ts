// Bundled images (login hero, banners, product photos) and the product-photo picker.
//
// Product photos: brand photos for a few known items, then a pool of photos per drink category
// (whisky, scotch, rum, vodka, gin, brandy, wine, beer, soft drinks, accessories). All are bundled
// from src/img/products/ so they work offline; credits are in src/img/products/stock/CREDITS.md.
// distinctProductPhotos() gives every item on screen its own photo (no repeats), and an item keeps
// the same photo across refreshes because the pool is rotated by a hash of its code.

import imgLoginHero from '../img/login_hero.jpg';
import bannerDistillery from '../img/banner_distillery.jpg';
import imgRoyalChallenge from '../img/products/royal_challenge.jpg';
import imgVinGreenLabel from '../img/products/vin_green_label.jpg';
import imgMcDowells from '../img/products/mcdowells.jpg';
import imgKingfisher from '../img/products/kingfisher.jpg';
import imgCompanyStore from '../img/company_store_1.webp';

import whiskyGlassSphere from '../img/products/stock/whisky_glass_sphere.jpg';
import whiskyTimelessCraft from '../img/products/stock/whisky_timeless_craft.jpg';
import lighterPureClass from '../img/products/stock/lighter_pure_class.jpg';
import bottleOpenerBar from '../img/products/stock/bottle_opener_bar.jpg';
import whiskyRocksMarble from '../img/products/stock/whisky_rocks_marble.jpg';
import whiskyBarBokeh from '../img/products/stock/whisky_bar_bokeh.jpg';
import scotchRocksTable from '../img/products/stock/scotch_rocks_table.jpg';
import brandySnifters from '../img/products/stock/brandy_snifters.jpg';
import rumRocksAmber from '../img/products/stock/rum_rocks_amber.jpg';
import rumDarkIce from '../img/products/stock/rum_dark_ice.jpg';
import rumCrystalGlass from '../img/products/stock/rum_crystal_glass.jpg';
import vodkaShotsBar from '../img/products/stock/vodka_shots_bar.jpg';
import vodkaShotGlasses from '../img/products/stock/vodka_shot_glasses.jpg';
import vodkaBarGlass from '../img/products/stock/vodka_bar_glass.jpg';
import ginTonicLemon from '../img/products/stock/gin_tonic_lemon.jpg';
import ginTonicLime from '../img/products/stock/gin_tonic_lime.jpg';
import ginCopaLime from '../img/products/stock/gin_copa_lime.jpg';
import winePourBottle from '../img/products/stock/wine_pour_bottle.jpg';
import wineRedGlass from '../img/products/stock/wine_red_glass.jpg';
import wineBottleStudio from '../img/products/stock/wine_bottle_studio.jpg';
import beerBottleNeck from '../img/products/stock/beer_bottle_neck.jpg';
import beerAmberBottle from '../img/products/stock/beer_amber_bottle.jpg';
import sodaGlassIce from '../img/products/stock/soda_glass_ice.jpg';
import sodaStrawGlass from '../img/products/stock/soda_straw_glass.jpg';

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
  },
} as const;

// A photo plus the CSS object-position that keeps its subject in view when the card crops it
// (the "Timeless Craft" and "Pure Class" posters have headline text at the top, so show the bottom).
export type ProductPhoto = { src: string; position: string };
const photo = (src: string, position = 'center'): ProductPhoto => ({ src, position });

type Category = 'scotch' | 'whisky' | 'brandy' | 'rum' | 'vodka' | 'gin' | 'wine' | 'beer' | 'soft' | 'accessory' | 'liquor';

const SPHERE = photo(whiskyGlassSphere, 'center 55%');
const CRAFT = photo(whiskyTimelessCraft, 'center 95%');
const PURE_CLASS = photo(lighterPureClass, 'center bottom');
const OPENER = photo(bottleOpenerBar, 'center 55%');

// Known brands and specific items first: these win over the category pools.
const BRAND_PHOTOS: [RegExp, ProductPhoto][] = [
  [/bottle ?opener|\bopener\b/, OPENER],
  [/lighter|zippo/, PURE_CLASS],
  [/royal challenge|\brc-750\b/, photo(imgRoyalChallenge)],
  [/green label|vin green|\bvin-750\b/, photo(imgVinGreenLabel)],
  [/mcdowell|\bmcd1\b|no\.1/, photo(imgMcDowells)],
  [/kingfisher|\bkfu?-\d/, photo(imgKingfisher)],
];

// Softvent's own photos: always offered first for these categories (before the rotated pool).
const FEATURED: Partial<Record<Category, ProductPhoto[]>> = {
  scotch: [SPHERE, CRAFT],
  whisky: [SPHERE, CRAFT],
  liquor: [SPHERE, CRAFT],
  accessory: [PURE_CLASS, OPENER],
};

const POOLS: Record<Category, ProductPhoto[]> = {
  scotch: [photo(scotchRocksTable), photo(whiskyRocksMarble)],
  whisky: [photo(whiskyRocksMarble), photo(whiskyBarBokeh), photo(scotchRocksTable)],
  brandy: [photo(brandySnifters), SPHERE, photo(whiskyBarBokeh)],
  rum: [photo(rumRocksAmber), photo(rumDarkIce), photo(rumCrystalGlass)],
  vodka: [photo(vodkaShotsBar), photo(vodkaShotGlasses), photo(vodkaBarGlass)],
  gin: [photo(ginTonicLemon), photo(ginTonicLime), photo(ginCopaLime)],
  wine: [photo(winePourBottle), photo(wineRedGlass), photo(wineBottleStudio)],
  beer: [photo(beerBottleNeck), photo(beerAmberBottle)],
  soft: [photo(sodaGlassIce), photo(sodaStrawGlass)],
  accessory: [],
  liquor: [photo(whiskyBarBokeh), photo(rumCrystalGlass), photo(brandySnifters)],
};

// Every photo once, used as the last resort so a screen never has to repeat one.
const ALL_PHOTOS: ProductPhoto[] = [
  ...Object.values(FEATURED).flat(),
  ...Object.values(POOLS).flat(),
  ...BRAND_PHOTOS.map(([, p]) => p),
].filter((p, i, list) => list.findIndex(q => q.src === p.src) === i);

// Which drink category an item belongs to, from its group and name (e.g. group "SCOTCH", "BEER",
// "SOFT DRINKS"). Order matters: "soft drinks" must not fall into a liquor bucket.
function categoryOf(text: string): Category {
  if (/soft drink|soda|cola|thums|pepsi|sprite|water|juice|energy/.test(text)) return 'soft';
  if (/lighter|cigar|tobacco|accessor|opener|non[- ]?liquor/.test(text)) return 'accessory';
  if (/beer|lager|ale\b|stout/.test(text)) return 'beer';
  if (/wine|champagne|sparkling/.test(text)) return 'wine';
  if (/scotch|single malt|blended malt|johnnie|chivas|ballantine|teacher/.test(text)) return 'scotch';
  if (/whisk/.test(text)) return 'whisky';
  if (/\brum\b|old monk/.test(text)) return 'rum';
  if (/vodka/.test(text)) return 'vodka';
  if (/\bgin\b/.test(text)) return 'gin';
  if (/brandy|cognac/.test(text)) return 'brandy';
  return 'liquor';
}

// Small stable string hash, so the same item always starts at the same point in its pool.
const hash = (s: string) => [...s].reduce((h, ch) => (h * 31 + ch.charCodeAt(0)) >>> 0, 7);
const rotate = <T,>(list: T[], by: number) => list.map((_, i) => list[(i + by) % list.length]);

type PhotoItem = { code?: string; name: string; group?: string };

// Best photos for one item, best first: its brand photo, Softvent's featured photos for its category,
// then its category pool (rotated per item), then every other photo.
export function productPhotoCandidates(item: PhotoItem): ProductPhoto[] {
  const text = `${item.group ?? ''} ${item.name} ${item.code ?? ''}`.toLowerCase();
  const category = categoryOf(text);
  const brand = BRAND_PHOTOS.filter(([re]) => re.test(text)).map(([, p]) => p);
  const pool = POOLS[category].length ? rotate(POOLS[category], hash(item.code || item.name)) : [];
  return [...brand, ...(FEATURED[category] ?? []), ...pool, ...ALL_PHOTOS];
}

// One photo per item with no repeats among `items` (e.g. the 4 restock cards). Items earlier in the
// list get first pick; later ones fall back to the next-best unused photo.
export function distinctProductPhotos(items: PhotoItem[]): ProductPhoto[] {
  const used = new Set<string>();
  return items.map(item => {
    const candidates = productPhotoCandidates(item);
    const pick = candidates.find(p => !used.has(p.src)) ?? candidates[0];
    used.add(pick.src);
    return pick;
  });
}

// Single best photo URL for an item (repeats are possible; use distinctProductPhotos for a set).
export function productImage(groupOrName: string, nameOrCode?: string): string {
  return productPhotoCandidates({ name: `${groupOrName} ${nameOrCode ?? ''}` })[0].src;
}
