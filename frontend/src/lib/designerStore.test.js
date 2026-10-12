import { afterEach, describe, expect, it, vi } from 'vitest';

vi.mock('./api', () => ({ getSchemaVersion: vi.fn(), getDesignerDocs: vi.fn(), getScreenDef: vi.fn(), streamDesignerEvents: vi.fn(() => new Promise(() => undefined)) }));
import { getDesignerDocs, getSchemaVersion, streamDesignerEvents } from './api';
import { forceDesignerReload, getDesignerSnapshot, loadDocs, subscribeDesigner } from './designerStore';

describe('designerStore', () => {
  let off;
  afterEach(() => { off?.(); off = undefined; vi.useRealTimers(); });

  it('asks the server nothing on its own: no check when subscribed, no poll, no check when the window gets focus', async () => {
    vi.useFakeTimers();
    off = subscribeDesigner(() => {});
    await vi.advanceTimersByTimeAsync(120000);
    window.dispatchEvent(new Event('focus'));
    await vi.advanceTimersByTimeAsync(0);
    expect(getSchemaVersion).not.toHaveBeenCalled();
    expect(streamDesignerEvents).not.toHaveBeenCalled();
    expect(getDesignerSnapshot()).toMatchObject({ changes: 0, buildChanged: false });
  });

  it('forceDesignerReload (the top bar button) drops the cached lists and counts a change', async () => {
    getDesignerDocs.mockResolvedValue([{ doc_id: 1 }]);
    off = subscribeDesigner(() => {});
    await loadDocs();
    await loadDocs();
    expect(getDesignerDocs).toHaveBeenCalledTimes(1);   // cached
    forceDesignerReload('v2', '');
    expect(getDesignerSnapshot()).toMatchObject({ version: 'v2', changes: 1 });
    await loadDocs();
    expect(getDesignerDocs).toHaveBeenCalledTimes(2);   // read again after the refresh
  });

  it('notes a newer front-end build reported by the refresh', () => {
    off = subscribeDesigner(() => {});
    forceDesignerReload('v1', '100');
    expect(getDesignerSnapshot().buildChanged).toBe(false);   // the first build seen is the page's own
    forceDesignerReload('v2', '101');
    expect(getDesignerSnapshot().buildChanged).toBe(true);
  });
});
