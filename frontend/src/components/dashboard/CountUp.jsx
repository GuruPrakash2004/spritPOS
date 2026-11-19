// A figure that counts up to its value when it appears or changes (the Dashboard's key figures).
// `text` is the finished text, e.g. "₹ 64,04,109.97" or "12"; its first number is animated (same decimals, Indian digit grouping)
// and the rest is kept as it is. A value with no number ("—") is shown as it is. With reduced motion, or where the browser can't tell
// (tests), the finished text shows at once.

import { useEffect, useRef, useState } from 'react';

const NUMBER = /\d[\d,]*(\.\d+)?/;
const DURATION = 900;

function parts(text) {
  const m = NUMBER.exec(text);
  if (!m) return null;
  return { before: text.slice(0, m.index), after: text.slice(m.index + m[0].length), value: Number(m[0].replaceAll(',', '')), decimals: m[1] ? m[1].length - 1 : 0 };
}

const calm = () => typeof window.matchMedia !== 'function' || window.matchMedia('(prefers-reduced-motion: reduce)').matches;
const format = (p, n) => `${p.before}${n.toLocaleString('en-IN', { minimumFractionDigits: p.decimals, maximumFractionDigits: p.decimals })}${p.after}`;

export default function CountUp({ text }) {
  const p = parts(text);
  const animate = !!p && !calm();
  const [n, setN] = useState(animate ? 0 : null);   // the number on screen while counting; null = show the finished text
  const last = useRef(0);
  useEffect(() => {
    if (!animate) return undefined;
    const target = parts(text).value;
    const begin = last.current;
    const start = performance.now();
    let raf;
    const step = (now) => {
      const t = Math.min(1, (now - start) / DURATION);
      const v = begin + (target - begin) * (1 - (1 - t) ** 3);
      last.current = v;
      setN(t < 1 ? v : null);
      if (t < 1) raf = requestAnimationFrame(step);
    };
    raf = requestAnimationFrame(step);
    return () => cancelAnimationFrame(raf);
  }, [text, animate]);
  return <span aria-label={text}>{animate && n !== null ? format(p, n) : text}</span>;
}
