import React from 'react';

interface VlqmsLogoProps {
  size?: 'sm' | 'md' | 'lg';
  showText?: boolean;
  className?: string;
  onDark?: boolean;
}

export const VlqmsLogo: React.FC<VlqmsLogoProps> = ({ size = 'md', showText = false, className = '', onDark = false }) => {
  // Proportions
  const dimensions = {
    sm: { crown: 11, fontSize: '9px', padding: 'px-2 py-1' },
    md: { crown: 14, fontSize: '11px', padding: 'px-2.5 py-1.5' },
    lg: { crown: 18, fontSize: '15px', padding: 'px-3.5 py-2' },
  }[size];

  return (
    <div className={`inline-flex items-center gap-3 p-[4px] ${className}`}>
      <div
        className="relative flex items-center justify-center rounded-xl p-[2px] shadow-lg transition-transform duration-300 hover:scale-105 active:scale-95"
        style={{
          background: 'linear-gradient(135deg, #E8C36A 0%, #D4A847 45%, #8E6C32 100%)',
          boxShadow: '0 4px 15px rgba(212, 168, 71, 0.25)',
        }}
      >
        <div className={`flex flex-col items-center justify-center rounded-[10px] bg-gradient-to-b from-[#542030] to-[#36121C] dark:from-[#181818] dark:to-[#0C0C0C] ${dimensions.padding} transition-colors`}>
          {/* Subtle Crown & Liquor Drop Crest Apex */}
          <svg
            width={dimensions.crown}
            height={dimensions.crown * 0.65}
            viewBox="0 0 24 15"
            fill="none"
            className="mb-0.5"
          >
            <path
              d="M2 13L4 3.5L9 8.5L12 1L15 8.5L20 3.5L22 13H2Z"
              fill="url(#vlqmsGoldGrad)"
              stroke="#E8C36A"
              strokeWidth="0.8"
            />
            <circle cx="12" cy="1" r="1.5" fill="#FFE8A3" />
            <circle cx="4" cy="3.5" r="1" fill="#FFE8A3" />
            <circle cx="20" cy="3.5" r="1" fill="#FFE8A3" />
            <defs>
              <linearGradient id="vlqmsGoldGrad" x1="0%" y1="0%" x2="100%" y2="100%">
                <stop offset="0%" stopColor="#FFF0C2" />
                <stop offset="50%" stopColor="#D4A847" />
                <stop offset="100%" stopColor="#9E7628" />
              </linearGradient>
            </defs>
          </svg>

          {/* VLQMS Luxury Typography */}
          <span
            className="font-black tracking-[0.16em] text-transparent bg-clip-text drop-shadow-[0_1px_2px_rgba(0,0,0,0.7)] leading-none select-none"
            style={{
              fontSize: dimensions.fontSize,
              fontFamily: "'Playfair Display', Georgia, serif",
              backgroundImage: 'linear-gradient(135deg, #FFF3D1 0%, #D4A847 50%, #B08A45 100%)',
            }}
          >
            VLQMS
          </span>

          {/* Micro Subtitle */}
          <span className="mt-0.5 text-[6.5px] font-bold tracking-[0.25em] text-[#E8C36A]/90 dark:text-[#E8C36A] uppercase leading-none">
            ✦ ERP ✦
          </span>
        </div>
      </div>

      {showText && (
        <div className="leading-tight text-left">
          {onDark ? (
            <>
              <div className="text-sm font-bold tracking-tight text-white drop-shadow-[0_1px_3px_rgba(0,0,0,0.8)]">
                Softvent <span className="text-[#E8C36A] font-extrabold">vLQMS</span>
              </div>
              <div className="text-[10px] font-bold tracking-[0.2em] uppercase text-[#D4A847] drop-shadow-[0_1px_2px_rgba(0,0,0,0.6)]">
                Cloud ERP
              </div>
            </>
          ) : (
            <>
              <div className="text-sm font-bold tracking-tight text-[#1A120B] dark:text-white">
                Softvent <span className="text-[#64283A] dark:text-[#E8C36A] font-extrabold">vLQMS</span>
              </div>
              <div className="text-[10px] font-bold tracking-[0.2em] uppercase text-[#966B24] dark:text-[#D4A847]">
                Cloud ERP
              </div>
            </>
          )}
        </div>
      )}
    </div>
  );
};

export default VlqmsLogo;
