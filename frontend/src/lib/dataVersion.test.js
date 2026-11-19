// The data-version signal: only a change of the server's counter (not the first value) tells screens to read again.
import { describe, expect, it } from 'vitest';
import { getDataVersion, noteDataVersion } from './dataVersion';

describe('dataVersion', () => {
  it('goes up only when the server counter changes', () => {
    noteDataVersion('5');
    const start = getDataVersion();
    noteDataVersion('5');
    expect(getDataVersion()).toBe(start);
    noteDataVersion(null);
    expect(getDataVersion()).toBe(start);
    noteDataVersion('6');
    expect(getDataVersion()).toBe(start + 1);
  });
});
