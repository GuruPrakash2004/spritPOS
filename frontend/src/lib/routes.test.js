import { describe, expect, it } from 'vitest';
import { MODULE_GROUPS } from './modules';
import { moduleForPath, pathForModule } from './routes';

describe('routes', () => {
  it('gives every module its own address and maps it back', () => {
    const seen = new Set();
    for (const g of MODULE_GROUPS) for (const m of g.modules) {
      const p = pathForModule(m);
      expect(seen.has(p)).toBe(false);
      seen.add(p);
      expect(moduleForPath(p)).toBe(m);
    }
  });
  it('keeps the old Admin Master address and falls back to the Dashboard', () => {
    expect(pathForModule('Admin Master')).toBe('/adminMaster');
    expect(moduleForPath('/adminMaster')).toBe('Admin Master');
    expect(moduleForPath('/sales-invoice/')).toBe('Sales Invoice');
    expect(moduleForPath('/nothing-here')).toBe('Dashboard');
    expect(pathForModule('Customised Reports')).toBe(pathForModule('Reports Dashboard'));
  });
});
