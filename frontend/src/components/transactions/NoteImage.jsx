// A small picture of an Indian note (or a pile of coins) for the Denomination window, drawn in the note's real colour with its value,
// because the desktop's bitmaps are not available. Used by CashCountDialog.jsx.
//   <NoteImage value="500" />   value: '2000' | '500' | '200' | '100' | '50' | '20' | '10' | 'coins'

const NOTES = {
  2000: { bg: '#C9559F', fg: '#7A1F5C' },
  500: { bg: '#A9A79B', fg: '#55554C' },
  200: { bg: '#F0A33A', fg: '#8A5410' },
  100: { bg: '#A89CD8', fg: '#4B3F86' },
  50: { bg: '#5CC6D4', fg: '#176674' },
  20: { bg: '#C9D45A', fg: '#66701A' },
  10: { bg: '#B98A62', fg: '#5E3A1C' },
};

export default function NoteImage({ value, className = 'h-12 w-24' }) {
  if (value === 'coins') {
    return (
      <svg viewBox="0 0 96 48" className={className} role="img" aria-label="Coins">
        <rect width="96" height="48" rx="3" fill="#8D969E" />
        {[[26, 28, 15], [50, 20, 13], [68, 30, 14], [40, 36, 10], [78, 14, 9]].map(([x, y, r], i) => (
          <g key={i}><circle cx={x} cy={y} r={r} fill="#D9DDE1" stroke="#5F676E" strokeWidth="1.5" /><circle cx={x} cy={y} r={r * 0.62} fill="none" stroke="#9AA2A9" strokeWidth="1" /></g>
        ))}
      </svg>
    );
  }
  const n = NOTES[value] ?? NOTES[10];
  return (
    <svg viewBox="0 0 96 48" className={className} role="img" aria-label={`₹${value} note`}>
      <rect width="96" height="48" rx="3" fill={n.bg} />
      <rect x="3" y="3" width="90" height="42" rx="2" fill="none" stroke={n.fg} strokeOpacity="0.55" strokeWidth="1" />
      <circle cx="66" cy="24" r="13" fill={n.fg} fillOpacity="0.28" />
      <circle cx="66" cy="19" r="5" fill={n.fg} fillOpacity="0.5" />
      <path d="M55 36c2-8 20-8 22 0z" fill={n.fg} fillOpacity="0.5" />
      <text x="9" y="19" fontSize="13" fontWeight="700" fill={n.fg} fontFamily="sans-serif">₹{value}</text>
      <text x="9" y="40" fontSize="6" fill={n.fg} fontFamily="sans-serif" letterSpacing="0.5">RESERVE BANK OF INDIA</text>
    </svg>
  );
}
