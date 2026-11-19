// usePdfPreview: every time a print window opens it must ask the server for the PDF again. The key (a format name, or document + format)
// does not say what is being printed (a changed label list, another day's report, an edited bill), so no earlier PDF may be reused.

import { renderHook, waitFor } from '@testing-library/react';
import { afterEach, beforeEach, describe, expect, it, vi } from 'vitest';
import { usePdfPreview } from './usePdfPreview';

const reply = (text) => ({ blob: async () => new Blob([text]), headers: new Headers({ 'X-Page-Size': '100,50' }) });

describe('usePdfPreview', () => {
  beforeEach(() => {
    URL.createObjectURL = vi.fn(() => 'blob:fake');
    URL.revokeObjectURL = vi.fn();
  });
  afterEach(() => vi.restoreAllMocks());

  it('asks the server again when the window is opened a second time with the same key', async () => {
    const load = vi.fn().mockResolvedValueOnce(reply('first labels')).mockResolvedValueOnce(reply('second labels'));

    const one = renderHook(() => usePdfPreview('RPTBC.rpt', load));
    await waitFor(() => expect(one.result.current.url).toBe('blob:fake'));
    one.unmount();

    const two = renderHook(() => usePdfPreview('RPTBC.rpt', load));
    await waitFor(() => expect(two.result.current.url).toBe('blob:fake'));

    expect(load).toHaveBeenCalledTimes(2);
  });

  it('does not ask for anything without a key', () => {
    const load = vi.fn();
    renderHook(() => usePdfPreview('', load));
    expect(load).not.toHaveBeenCalled();
  });
});
