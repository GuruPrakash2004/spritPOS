// Dashboard banner content: a glowing "Designed & developed by Softvent" ribbon that circles endlessly from right to
// left. The whole banner is one link to softvent.com so a client who wants to reach Softvent just clicks it. Motion is
// transform/opacity only; the endless loops are switched off by the global reduced-motion rule in index.css.

import { ArrowUpRight, Mail, Phone } from 'lucide-react';
import DevelopedBy from '../layout/DevelopedBy';

const PHRASE = ['Designed & developed by', 'SOFTVENT', 'Software that powers your business'];

function Strip({ hidden = false }) {
  return (
    <ul className="flex shrink-0 items-center" aria-hidden={hidden || undefined}>
      {PHRASE.map((word, i) => (
        <li key={word} className="flex items-center">
          <span className={i === 1 ? 'sv-marquee-brand' : 'sv-marquee-text'}>{word}</span>
          <span className="sv-marquee-dot" aria-hidden="true">✦</span>
        </li>
      ))}
    </ul>
  );
}

const SPARKS = ['left-[8%] top-[18%]', 'left-[28%] bottom-[16%]', 'right-[26%] top-[14%]', 'right-[9%] bottom-[22%]', 'left-[48%] top-[10%]'];

export default function SoftventMarquee() {
  return (
    <a
      href="https://softvent.com/"
      target="_blank"
      rel="noopener noreferrer"
      title="Open softvent.com"
      aria-label="Designed and developed by Softvent. Open softvent.com"
      className="sv-banner group/sv absolute inset-0 z-10 flex flex-col items-center justify-center gap-3 px-4 text-center focus-visible:outline focus-visible:outline-2 focus-visible:-outline-offset-4 focus-visible:outline-[#F6E3B4]"
    >
      {SPARKS.map((pos, i) => (
        <span key={pos} className={`sv-spark absolute ${pos}`} style={{ animationDelay: `${i * 0.7}s` }} aria-hidden="true" />
      ))}

      <div className="sv-halo"><DevelopedBy bare size="lg" /></div>

      <div className="sv-ribbon relative w-full max-w-5xl overflow-hidden rounded-full">
        <div className="sv-marquee-track flex w-max items-center py-2.5">
          <Strip />
          <Strip hidden />
          <Strip hidden />
          <Strip hidden />
        </div>
      </div>

      <span className="sv-cta inline-flex items-center gap-1.5 rounded-full border border-[#F6E3B4]/60 bg-black/30 px-4 py-1.5 text-xs font-semibold text-[#F6E3B4] backdrop-blur-sm sm:text-sm">
        Visit softvent.com <ArrowUpRight size={14} aria-hidden="true" />
      </span>
      <p className="flex flex-wrap items-center justify-center gap-x-4 gap-y-1 text-[11px] font-medium text-[#F6E3B4]/80">
        <span className="inline-flex items-center gap-1"><Phone size={11} aria-hidden="true" />9945036934 / 9880606365</span>
        <span className="inline-flex items-center gap-1"><Mail size={11} aria-hidden="true" />mail@softvent.com</span>
      </p>
    </a>
  );
}
