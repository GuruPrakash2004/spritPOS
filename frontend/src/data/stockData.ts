export const SAMPLE_STOCK_DATA = [
  { code: 'VIN-750', group: 'WHISKY', brand: 'ABD', name: 'VIN GREEN LABEL BARREL SPECIAL CLASIC WHISKY', pkg: '750ml', opQty: 48, recQty: 24, issQty: 16, clQty: 56, rate: 835.00, purchaseRate: 759.29, cat: 'IML', ksbcl: 'KSBCL-102948' },
  { code: 'RC-750', group: 'WHISKY', brand: 'Royal Challenge', name: 'ROYAL CHALLENGE SELECT PREMIUM WHISKY', pkg: '750ml', opQty: 60, recQty: 36, issQty: 28, clQty: 68, rate: 950.00, purchaseRate: 860.00, cat: 'IML', ksbcl: 'KSBCL-104412' },
  { code: 'MCD1-180', group: 'WHISKY', brand: 'McDowells No.1', name: 'MCDOWELLS NO.1 LUXURY PREMIUM WHISKY', pkg: '180ml', opQty: 120, recQty: 96, issQty: 84, clQty: 132, rate: 220.00, purchaseRate: 198.50, cat: 'IML', ksbcl: 'KSBCL-108821' },
  { code: 'KF-650', group: 'BEER', brand: 'Kingfisher', name: 'KINGFISHER PREMIUM LAGER BEER', pkg: '650ml', opQty: 144, recQty: 120, issQty: 96, clQty: 168, rate: 180.00, purchaseRate: 155.00, cat: 'BEER', ksbcl: 'KSBCL-201103' },
  { code: 'KFU-650', group: 'BEER', brand: 'Kingfisher', name: 'KINGFISHER ULTRA MAX STRONG BEER', pkg: '650ml', opQty: 48, recQty: 48, issQty: 32, clQty: 64, rate: 230.00, purchaseRate: 198.00, cat: 'BEER', ksbcl: 'KSBCL-201994' },
  { code: 'OM-750', group: 'RUM', brand: 'Old Monk', name: 'OLD MONK THE LEGEND XXX RUM', pkg: '750ml', opQty: 36, recQty: 24, issQty: 18, clQty: 42, rate: 850.00, purchaseRate: 770.00, cat: 'IML', ksbcl: 'KSBCL-301140' },
  { code: 'MM-750', group: 'VODKA', brand: 'Magic Moments', name: 'MAGIC MOMENTS REMIX GRAIN VODKA', pkg: '750ml', opQty: 24, recQty: 12, issQty: 11, clQty: 25, rate: 750.00, purchaseRate: 678.00, cat: 'IML', ksbcl: 'KSBCL-400192' },
  { code: 'JWB-750', group: 'SCOTCH', brand: 'Johnnie Walker', name: 'JOHNNIE WALKER BLACK LABEL 12 YO', pkg: '750ml', opQty: 18, recQty: 12, issQty: 6, clQty: 24, rate: 3850.00, purchaseRate: 3450.00, cat: 'IMPORTED', ksbcl: 'KSBCL-508891' },
] as const;

// Litres in one bottle, from the package size ("750ml" → 0.75). Mirrors Package Master's litres per bottle.
export const litresPerBottle = (pkg: string) => (Number.parseFloat(pkg) || 0) / 1000;
