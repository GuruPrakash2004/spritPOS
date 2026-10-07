// Shared by the print windows: asks for a report's PDF (laid out by Crystal through the bridge) whenever `key` changes
// and keeps it as a blob URL for an <iframe>, with the page size (CSS px) the server reports in X-Page-Size. `print(frame)` prints what the frame shows.

import { useEffect, useState, } from 'react';
import { errorText } from '../../lib/api';

export function usePdfPreview(key, load) {
  const [url, setUrl] = useState('');
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');
  const [size, setSize] = useState(null);

  // Fetching on a key change legitimately starts by resetting the preview state.
  /* eslint-disable react-hooks/set-state-in-effect */
  useEffect(() => {
    if (!key) { setUrl(''); return; }
    let live = true;
    let made = '';
    setLoading(true);
    setError('');
    load()
      .then(async res => {
        if (!live) return;
        made = URL.createObjectURL(await res.blob());
        const [w, h] = (res.headers.get('X-Page-Size') ?? '').split(',').map(Number);
        setSize(w > 0 && h > 0 ? { w: w * 96 / 72, h: h * 96 / 72 } : null);
        setUrl(made);
      })
      .catch(e => { if (live) { setUrl(''); setError(errorText(e)); } })
      .finally(() => { if (live) setLoading(false); });
    return () => { live = false; if (made) URL.revokeObjectURL(made); };
    // `load` is rebuilt on every render by the callers; the preview is asked for again only when the key changes.
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [key]);
  /* eslint-enable react-hooks/set-state-in-effect */

  return { url, loading, error, size };
}

/** The frame's size: the report's own page, as wide as fits (a receipt stays narrow, an A4 fills the pane). */
export const sheetStyle = (size) => size
  ? { aspectRatio: `${size.w} / ${size.h}`, width: `min(100%, ${Math.round(size.w)}px)`, maxHeight: '68vh' }
  : { width: '100%', height: '60vh' };

/** Print what the frame shows; if the browser's PDF viewer refuses, open the PDF in its own tab (it has Print). */
export function printFrame(frame, url) {
  const w = frame.current?.contentWindow;
  try {
    if (!w) throw new Error('no frame');
    w.focus();
    w.print();
  } catch {
    window.open(url, '_blank');
  }
}
