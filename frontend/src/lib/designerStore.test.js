import { afterEach, beforeEach, describe, expect, it, vi } from 'vitest';

vi.mock('./api', () => ({ getSchemaVersion: vi.fn(), getDesignerDocs: vi.fn(), getScreenDef: vi.fn(), streamDesignerEvents: vi.fn(() => new Promise(() => undefined)) }));
import { getSchemaVersion } from './api';
import { getDesignerSnapshot, subscribeDesigner } from './designerStore';

const flush = async () => { await vi.advanceTimersByTimeAsync(0); };

describe('designerStore', () => {
  let off;
  beforeEach(() => { vi.useFakeTimers(); getSchemaVersion.mockReset(); });
  afterEach(() => { off?.(); vi.useRealTimers(); });

  it('takes the first version as the baseline and counts later moves', async () => {
    getSchemaVersion.mockResolvedValue({ version: 'a', build: '1' });
    off = subscribeDesigner(() => {});
    await flush();
    expect(getDesignerSnapshot()).toMatchObject({ version: 'a', changes: 0, buildChanged: false });
    getSchemaVersion.mockResolvedValue({ version: 'b', build: '2' });
    await vi.advanceTimersByTimeAsync(30000);
    expect(getDesignerSnapshot()).toMatchObject({ version: 'b', changes: 1, buildChanged: true });
  });

  it('keeps the same snapshot object while nothing changes, and stops polling with the last subscriber', async () => {
    getSchemaVersion.mockResolvedValue({ version: 'a', build: '' });
    off = subscribeDesigner(() => {});
    await flush();
    const first = getDesignerSnapshot();
    await vi.advanceTimersByTimeAsync(30000);
    expect(getDesignerSnapshot()).toBe(first);
    off(); off = undefined;
    const calls = getSchemaVersion.mock.calls.length;
    await vi.advanceTimersByTimeAsync(90000);
    expect(getSchemaVersion.mock.calls.length).toBe(calls);
    expect(getDesignerSnapshot().version).toBe('');
  });
});
