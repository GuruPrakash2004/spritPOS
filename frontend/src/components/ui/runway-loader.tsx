// Third-party animated progress bar (truck / plane on a runway). Toast.tsx uses it as the popup countdown bar.
"use client";

import { useLayoutEffect, useEffect, useRef, useState } from "react";
import { motion, AnimatePresence } from "framer-motion";
import softventTruckImg from "@/img/softvent_truck.png";

const useIsomorphicLayoutEffect =
  typeof window !== "undefined" ? useLayoutEffect : useEffect;

/* ═══════════════════════════════════════════════════════════════════════════
   SOFTVENT COMMERCIAL DELIVERY TRUCK
   Faithfully based on the reference image (media_1790372313758.png):
   - Freightliner-style medium-duty commercial box truck
   - "SOFTVENT CONCEPT ORIENTED" & "EST. SOFTVENT ADVISORY SERVICES 1999"
   - Aerodynamic cab with chrome horizontal grille & teardrop headlights
   - Dual chrome side mirrors and amber cab clearance lights
   - Heavy-duty chassis, chrome wheel lugs & radial dual rear tires
   - Dynamic save (green) and delete (red) underglow & track lighting
   ═══════════════════════════════════════════════════════════════════════════ */

export function TruckVehicle({
  isDark = false,
  color = "green",
  compact = false,
  className = "",
}: {
  isDark?: boolean;
  color?: "green" | "red" | "blue" | "gold";
  compact?: boolean;
  className?: string;
}) {
  const isGreen = color === "green";
  const isRed = color === "red";
  const isGold = color === "gold";
  const isBlue = color === "blue";

  // Dynamic ambient rim-light and ground underglow matching status
  const glowStyle = isGold
    ? isDark
      ? "drop-shadow(0 4px 12px rgba(212,168,71,0.8)) drop-shadow(0 2px 4px rgba(0,0,0,0.8))"
      : "drop-shadow(0 3px 8px rgba(212,168,71,0.6)) drop-shadow(0 1px 3px rgba(0,0,0,0.25))"
    : isBlue
    ? isDark
      ? "drop-shadow(0 4px 12px rgba(59,130,246,0.8)) drop-shadow(0 2px 4px rgba(0,0,0,0.8))"
      : "drop-shadow(0 3px 8px rgba(59,130,246,0.6)) drop-shadow(0 1px 3px rgba(0,0,0,0.25))"
    : isRed
    ? isDark
      ? "drop-shadow(0 4px 10px rgba(239,68,68,0.75)) drop-shadow(0 2px 4px rgba(0,0,0,0.8))"
      : "drop-shadow(0 3px 8px rgba(239,68,68,0.55)) drop-shadow(0 1px 3px rgba(0,0,0,0.25))"
    : isGreen && isDark
    ? "drop-shadow(0 4px 10px rgba(16,185,129,0.75)) drop-shadow(0 2px 4px rgba(0,0,0,0.8))"
    : "drop-shadow(0 3px 8px rgba(16,185,129,0.55)) drop-shadow(0 1px 3px rgba(0,0,0,0.25))";

  return (
    <div
      className={`relative inline-flex flex-col items-center select-none ${className}`}
      style={{ filter: glowStyle }}
    >
      <img
        src={softventTruckImg}
        alt="Softvent Concept Oriented Commercial Truck"
        draggable={false}
        className={`pointer-events-none select-none max-w-none transition-transform duration-75 ${
          compact
            ? "h-8 sm:h-9 w-auto object-contain"
            : "h-20 sm:h-24 w-auto object-contain"
        }`}
      />

      {/* Dynamic underbody road contact neon glow synchronized with loader color */}
      <div
        className={`pointer-events-none absolute -bottom-0.5 left-3 right-3 h-1.5 rounded-full blur-[3px] opacity-80 transition-colors ${
          isGold ? "bg-[#D4A847]" : isBlue ? "bg-blue-500" : isRed ? "bg-red-500" : "bg-emerald-500"
        }`}
      />
    </div>
  );
}

/* ─── AIRPLANE FALLBACK (for reference / alternate use) ─── */
export function Airplane({ isDark }: { isDark: boolean }) {
  const bodyTop = isDark ? "#ececec" : "#c8c8c8";
  const bodyMid = isDark ? "#ffffff" : "#dedede";
  const bodyBot = isDark ? "#d0d0d0" : "#b4b4b4";
  const wingLight = isDark ? "#f8f8f8" : "#d8d8d8";
  const wingDark = isDark ? "#bababa" : "#909090";
  const tail = isDark ? "#c8c8c8" : "#a8a8a8";
  const nose = isDark ? "#f4f4f4" : "#d8d8d8";

  return (
    <svg
      className="w-[52px] sm:w-[72px] h-auto"
      viewBox="0 0 72 46"
      fill="none"
      style={{
        filter: isDark
          ? "drop-shadow(0 4px 12px rgba(0,0,0,0.75)) drop-shadow(0 2px 4px rgba(0,0,0,0.5))"
          : "drop-shadow(0 2px 8px rgba(0,0,0,0.35)) drop-shadow(0 1px 3px rgba(0,0,0,0.2))",
      }}
    >
      <defs>
        <linearGradient id="rl-body" x1="0" y1="0" x2="0" y2="1">
          <stop offset="0%" stopColor={bodyTop} />
          <stop offset="40%" stopColor={bodyMid} />
          <stop offset="100%" stopColor={bodyBot} />
        </linearGradient>
        <linearGradient id="rl-wing-t" x1="0" y1="1" x2="1" y2="0">
          <stop offset="0%" stopColor={wingLight} />
          <stop offset="100%" stopColor={wingDark} />
        </linearGradient>
        <linearGradient id="rl-wing-b" x1="0" y1="0" x2="1" y2="1">
          <stop offset="0%" stopColor={wingLight} />
          <stop offset="100%" stopColor={wingDark} />
        </linearGradient>
      </defs>

      <path d="M44,21 L28,21 L8,2 L18,0 Z" fill="url(#rl-wing-t)" />
      <path d="M44,25 L28,25 L8,44 L18,46 Z" fill="url(#rl-wing-b)" />
      <path d="M8,21 L2,14 L6,14 L12,21 Z" fill={tail} />
      <path d="M8,25 L2,32 L6,32 L12,25 Z" fill={tail} />
      <ellipse cx="34" cy="23" rx="29" ry="7" fill="url(#rl-body)" />
      <path
        d="M61,19.5 C66,21.5 67.5,23 67,23 C67.5,23 66,24.5 61,26.5 L58,23 Z"
        fill={nose}
      />
      <ellipse cx="57" cy="23" rx="4" ry="3" fill="#7ec8e8" opacity="0.9" />
    </svg>
  );
}

type Phase = "taxiing" | "takeoff" | "resetting";

export interface RunwayLoaderProps {
  vehicle?: "truck" | "airplane";
  color?: "green" | "red" | "blue" | "gold";
  progress?: number;
  compact?: boolean;
  className?: string;
  label?: string;
}

export default function RunwayLoader({
  vehicle = "truck",
  color = "green",
  progress: externalProgress,
  compact = false,
  className = "",
  label,
}: RunwayLoaderProps) {
  const containerRef = useRef<HTMLDivElement>(null);
  const [isDark, setIsDark] = useState(() =>
    typeof window !== "undefined"
      ? document.documentElement.classList.contains("dark")
      : false,
  );

  useIsomorphicLayoutEffect(() => {
    const el = containerRef.current;
    if (!el) return;
    const check = () => {
      const card = el.closest("[data-card-theme]");
      setIsDark(
        card
          ? card.classList.contains("dark")
          : document.documentElement.classList.contains("dark"),
      );
    };
    check();
    const observer = new MutationObserver(check);
    observer.observe(document.documentElement, {
      attributes: true,
      attributeFilter: ["class"],
    });
    const cardWrapper = el.closest("[data-card-theme]");
    if (cardWrapper)
      observer.observe(cardWrapper, {
        attributes: true,
        attributeFilter: ["class"],
      });
    return () => observer.disconnect();
  }, []);

  const [internalProgress, setInternalProgress] = useState(0);
  const [phase, setPhase] = useState<Phase>("taxiing");
  const [cycle, setCycle] = useState(0);

  const isControlled = typeof externalProgress === "number";
  const progress = isControlled ? Math.max(0, Math.min(100, externalProgress)) : internalProgress;

  const rafRef = useRef<number>(0);
  const startRef = useRef<number | undefined>(undefined);
  const aliveRef = useRef(true);
  const disappearedRef = useRef(false);

  const WALL_PCT = 92;

  useEffect(() => {
    if (isControlled) return;

    aliveRef.current = true;
    disappearedRef.current = false;
    const TAXI_MS = 5500;

    function tick(ts: number) {
      if (!aliveRef.current) return;
      if (!startRef.current) startRef.current = ts;
      const t = Math.min((ts - startRef.current) / TAXI_MS, 1);

      const eased =
        t <= 0.75
          ? (t / 0.75) * 82
          : 82 + (1 - Math.pow(1 - (t - 0.75) / 0.25, 2)) * 18;
      const p = Math.min(eased, 100);
      setInternalProgress(p);

      if (p >= WALL_PCT && !disappearedRef.current) {
        disappearedRef.current = true;
        setPhase("takeoff");
      }

      if (p < 100) {
        rafRef.current = requestAnimationFrame(tick);
      } else {
        setTimeout(() => {
          if (!aliveRef.current) return;
          setPhase("resetting");
          setTimeout(() => {
            if (!aliveRef.current) return;
            disappearedRef.current = false;
            startRef.current = undefined;
            setInternalProgress(0);
            setPhase("taxiing");
            setCycle((c) => c + 1);
            rafRef.current = requestAnimationFrame(tick);
          }, 400);
        }, 600);
      }
    }

    rafRef.current = requestAnimationFrame(tick);
    return () => {
      aliveRef.current = false;
      if (rafRef.current) cancelAnimationFrame(rafRef.current);
    };
  }, [isControlled, WALL_PCT]);

  const isGreen = color === "green";
  const isRed = color === "red";
  const isGold = color === "gold";
  const isBlue = color === "blue";

  // Road Track Gradients & Shadows
  const trackBg = isDark
    ? "linear-gradient(to bottom, #2B2621, #171412)"
    : "linear-gradient(to bottom, #D9D3CA, #C4BEB4)";
  const trackShadow = isDark
    ? "inset 0 2px 6px rgba(0,0,0,0.85), 0 1px 3px rgba(255,255,255,0.04)"
    : "inset 0 2px 4px rgba(0,0,0,0.18), inset 0 -1px 2px rgba(255,255,255,0.4)";
  const dashColor = isDark
    ? "rgba(255,255,255,0.3)"
    : "rgba(255,255,255,0.55)";

  // Colors: gold for refresh, metallic blue for backup, red for delete/error, green for save/success
  const fillGradient = isGold
    ? "linear-gradient(to right, #92400E, #D4A847 50%, #FDE047)"
    : isBlue
    ? "linear-gradient(to right, #1E3A8A, #3B82F6 50%, #93C5FD)"
    : isRed
    ? "linear-gradient(to right, #7F1D1D, #EF4444 50%, #F87171)"
    : "linear-gradient(to right, #065F46, #10B981 50%, #34D399)";

  const fillShadow = isGold
    ? isDark ? "0 0 14px rgba(212,168,71,0.65)" : "0 0 10px rgba(212,168,71,0.45)"
    : isBlue
    ? isDark ? "0 0 14px rgba(59,130,246,0.65)" : "0 0 10px rgba(59,130,246,0.45)"
    : isRed
    ? isDark ? "0 0 12px rgba(239,68,68,0.55)" : "0 0 8px rgba(239,68,68,0.35)"
    : isDark ? "0 0 12px rgba(16,185,129,0.55)" : "0 0 8px rgba(16,185,129,0.35)";

  /* ─── COMPACT MODE: Designed for Popups & Snackbars ─── */
  if (compact) {
    return (
      <div ref={containerRef} className={`relative w-full overflow-visible pt-7 pb-1 ${className}`}>
        {/* Runway / Road Track */}
        <div
          className="relative h-2.5 w-full overflow-hidden rounded-full sm:h-3"
          style={{ background: trackBg, boxShadow: trackShadow }}
        >
          {/* Dashed Center Road Line */}
          <div
            className="pointer-events-none absolute top-1/2 -translate-y-px"
            style={{
              left: 6,
              right: 6,
              height: 1.5,
              backgroundImage: `repeating-linear-gradient(to right, ${dashColor} 0, ${dashColor} 6px, transparent 6px, transparent 14px)`,
            }}
          />

          {/* Dynamic Green (Save) / Red (Delete) Progress Fill */}
          <div
            className="absolute inset-y-0 left-0 rounded-full will-change-[width]"
            style={{
              width: `${progress}%`,
              background: fillGradient,
              boxShadow: fillShadow,
            }}
          />
        </div>

        {/* Softvent Commercial Delivery Truck Moving Along Highway */}
        <div
          className="pointer-events-none absolute bottom-[5px] z-10 will-change-[left,transform]"
          style={{
            left: `${progress}%`,
            transform: `translateX(-${progress}%)`,
          }}
        >
          {vehicle === "truck" ? (
            <TruckVehicle isDark={isDark} color={color} compact={true} />
          ) : (
            <div className="scale-75">
              <Airplane isDark={isDark} />
            </div>
          )}
        </div>
      </div>
    );
  }

  /* ─── FULL MODE: Standalone Staged Runway Component ─── */
  return (
    <div
      ref={containerRef}
      className={`flex h-full w-full items-center justify-center ${className}`}
      style={{ background: isDark ? "#110F0C" : "#F5F1EA" }}
    >
      <div className="flex w-full max-w-[480px] flex-col items-center gap-4 px-4 sm:gap-6 sm:px-6">
        {/* Track container */}
        <div className="relative w-full pt-16 pb-2">
          {/* Road Highway Surface */}
          <div
            className="relative h-10 w-full overflow-hidden rounded-full sm:h-12"
            style={{ background: trackBg, boxShadow: trackShadow }}
          >
            {/* Road center dashes */}
            <div
              className="pointer-events-none absolute top-1/2 -translate-y-px"
              style={{
                left: 18,
                right: 18,
                height: 2,
                backgroundImage: `repeating-linear-gradient(to right, ${dashColor} 0, ${dashColor} 10px, transparent 10px, transparent 24px)`,
              }}
            />

            {/* Dynamic Progress Fill */}
            <div
              className="absolute inset-y-0 left-0 rounded-full transition-all"
              style={{
                width: `${progress}%`,
                background: fillGradient,
                boxShadow: fillShadow,
              }}
            />
          </div>

          {/* Dust / Road Spray behind truck */}
          {phase === "taxiing" && progress > 20 && (
            <motion.div
              className="pointer-events-none absolute bottom-[14px] rounded-full"
              style={{
                right: `calc(${100 - progress}% + 30px)`,
                width: Math.min(((progress - 20) / 80) * 60, 60),
                height: 16,
                background: isGreen
                  ? "radial-gradient(ellipse at right, rgba(34,197,94,0.45), transparent)"
                  : isRed
                  ? "radial-gradient(ellipse at right, rgba(239,68,68,0.45), transparent)"
                  : "radial-gradient(ellipse at right, rgba(59,130,246,0.45), transparent)",
                filter: "blur(6px)",
              }}
              animate={{ opacity: [0.3, 0.8, 0.2, 0.7, 0.3] }}
              transition={{ duration: 0.2, repeat: Infinity }}
            />
          )}

          {/* Softvent Truck Vehicle Animation */}
          <AnimatePresence>
            {phase !== "resetting" && (
              <div
                key={`veh-${cycle}`}
                className="pointer-events-none absolute bottom-[10px] z-20"
                style={{
                  left: `${progress}%`,
                  transform: `translateX(-${progress}%)`,
                }}
              >
                {phase === "taxiing" ? (
                  vehicle === "truck" ? (
                    <TruckVehicle isDark={isDark} color={color} />
                  ) : (
                    <Airplane isDark={isDark} />
                  )
                ) : (
                  <motion.div
                    initial={{ opacity: 1, x: 0 }}
                    animate={{ opacity: 0, x: 140, scale: 1.15 }}
                    transition={{ duration: 0.55, ease: "easeIn" }}
                  >
                    {vehicle === "truck" ? (
                      <TruckVehicle isDark={isDark} color={color} />
                    ) : (
                      <Airplane isDark={isDark} />
                    )}
                  </motion.div>
                )}
              </div>
            )}
          </AnimatePresence>
        </div>

        {/* Counter and Labels */}
        <div className="flex flex-col items-center gap-1">
          <div className="flex items-baseline gap-1">
            <span
              className="text-2xl font-bold tabular-nums sm:text-[28px]"
              style={{ color: isDark ? "#FAF7F2" : "#1C1916" }}
            >
              {Math.round(progress)}
            </span>
            <span
              className="text-sm font-semibold sm:text-base"
              style={{ color: isGreen ? "#10B981" : isRed ? "#EF4444" : "#3B82F6" }}
            >
              %
            </span>
          </div>
          <span
            className="text-[10px] font-semibold uppercase tracking-widest sm:text-[11px]"
            style={{ color: isDark ? "#A8A29E" : "#78716C" }}
          >
            {label ||
              (phase === "takeoff"
                ? vehicle === "truck" ? "Delivered!" : "Taking off!"
                : phase === "resetting"
                ? "..."
                : vehicle === "truck"
                ? isGreen ? "Dispatching Softvent delivery…" : "Removing record…"
                : "Preparing for takeoff")}
          </span>
        </div>
      </div>
    </div>
  );
}
