// Item alias rules of the exe's alias grid (frmMstItem): at most 30 characters, no apostrophe.

export const ALIAS_MAX = 30;

export const cleanAlias = (v) => String(v ?? '').replace(/'/g, '').slice(0, ALIAS_MAX);
