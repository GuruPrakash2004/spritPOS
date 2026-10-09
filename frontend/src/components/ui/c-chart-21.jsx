// Third-party donut chart (recharts) used on the Dashboard.
"use client";

import { Cell, Pie, PieChart } from "recharts";

import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from "@/components/ui/card";
import {
  ChartContainer,
  ChartTooltip,
  ChartTooltipContent,

} from "@/components/ui/chart";

export function Pattern({
  data,
  config,
  title = "Inventory Mix",
  description = "Share of stock value",
  dataKey = "share",
  nameKey = "category",
  className = "w-full max-w-xs",
}







) {
  // No made-up slices: an empty chart until the caller passes real data.
  const activeData = data ?? [];
  const activeConfig = config ?? {};

  // 4 Unique Colors matching reference image (media_1790374531785.png):
  // 1: Green (#0CBF82), 2: Blue (#3383FF), 3: Orange (#FE9E0C), 4: Red (#FF2F60)
  const defaultColors = ["#0CBF82", "#3383FF", "#FE9E0C", "#FF2F60"];

  return (
    <Card className={`border-[#DDE3EC] bg-[#FFFFFF] shadow-sm dark:border-[#2A2A2A] dark:bg-[#141414] ${className}`}>
      <CardHeader className="items-center pb-0">
        <CardTitle className="text-base font-semibold text-[#1A2236] dark:text-stone-50">{title}</CardTitle>
        {description && <CardDescription className="text-xs text-[#5F6B80] dark:text-stone-400">{description}</CardDescription>}
      </CardHeader>
      <CardContent className="flex flex-col flex-1 pb-4 pt-1">
        <ChartContainer
          config={activeConfig}
          // Slice separators match the card so they read as gaps: cream in light mode, #141414 in dark mode.
          className="mx-auto aspect-square max-h-[220px] w-full [&_.recharts-pie-sector_path]:stroke-[#FFFFFF] dark:[&_.recharts-pie-sector_path]:stroke-[#141414]"
        >
          <PieChart>
            <defs>
              {/* Pattern for Whisky (Emerald Green) */}
              <pattern
                id="chart-whisky-pattern"
                patternUnits="userSpaceOnUse"
                width="6"
                height="6"
              >
                <rect width="6" height="6" fill="#0CBF82" opacity="0.3" />
                <path d="M0,6 L6,0 M-2,2 L2,-2 M4,8 L8,4" stroke="#0CBF82" strokeWidth="1.2" opacity="0.9" />
              </pattern>

              {/* Pattern for Beer (Vibrant Blue) */}
              <pattern
                id="chart-beer-pattern"
                patternUnits="userSpaceOnUse"
                width="5"
                height="5"
              >
                <rect width="5" height="5" fill="#3383FF" opacity="0.25" />
                <circle cx="2.5" cy="2.5" r="1.2" fill="#3383FF" opacity="0.85" />
              </pattern>

              {/* Pattern for Rum & Vodka (Vibrant Amber Orange) */}
              <pattern
                id="chart-rum-pattern"
                patternUnits="userSpaceOnUse"
                width="6"
                height="6"
              >
                <rect width="6" height="6" fill="#FE9E0C" opacity="0.25" />
                <path d="M0,0 L6,6 M0,6 L6,0" stroke="#FE9E0C" strokeWidth="1" opacity="0.75" />
              </pattern>

              {/* Pattern for Scotch (Vibrant Rose Red) */}
              <pattern
                id="chart-scotch-pattern"
                patternUnits="userSpaceOnUse"
                width="5"
                height="5"
              >
                <rect width="5" height="5" fill="#FF2F60" opacity="0.25" />
                <circle cx="2.5" cy="2.5" r="1.2" fill="#FF2F60" opacity="0.85" />
              </pattern>
            </defs>

            <ChartTooltip
              content={
                <ChartTooltipContent
                  className="min-w-36 gap-2.5 rounded-xl border border-[#DDE3EC] bg-[#FFFFFF] p-2.5 text-xs shadow-xl dark:border-[#2A2A2A] dark:bg-[#141414]"
                  formatter={(value, name) => {
                    const itemConfig = activeConfig[name ];
                    const labelText = itemConfig?.label || String(name);
                    const itemColor = (itemConfig && "color" in itemConfig ? itemConfig.color : undefined)  || "#0CBF82";

                    return (
                      <div className="flex w-full items-center justify-between gap-2">
                        <div className="flex items-center gap-2">
                          <span
                            className="h-2.5 w-2.5 shrink-0 rounded-full"
                            style={{ backgroundColor: itemColor }}
                          />
                          <span className="text-[#5F6B80] dark:text-stone-300 font-medium">
                            {labelText}
                          </span>
                        </div>
                        <span className="font-bold tabular-nums text-[#1A2236] dark:text-stone-50">
                          {Number(value).toLocaleString()}%
                        </span>
                      </div>
                    );
                  }}
                />
              }
            />

            <Pie
              data={activeData}
              dataKey={dataKey}
              nameKey={nameKey}
              innerRadius={42}
              outerRadius={75}
              cornerRadius={5}
              paddingAngle={3}
              stroke="#FFFFFF"
              strokeWidth={2}
            >
              {activeData.map((entry, index) => {
                const rawKey = String(entry[nameKey] ?? "");
                const cfg = activeConfig[rawKey ];
                const sliceColor =
                  (cfg && "color" in cfg ? cfg.color : undefined) ||
                  entry.color ||
                  entry.fill ||
                  defaultColors[index % defaultColors.length];

                return <Cell key={`cell-${index}`} fill={sliceColor} />;
              })}
            </Pie>
          </PieChart>
        </ChartContainer>

        {/* ═══ 4 UNIQUE COLOR LEGENDS MATCHING REFERENCE IMAGE ═══ */}
        <div className="mt-2 grid grid-cols-2 gap-x-4 gap-y-2.5 px-3 py-1">
          {activeData.map((item, idx) => {
            const itemRec = item ;
            const catKey = String(itemRec[nameKey] ?? "");
            const cfg = activeConfig[catKey ];
            const label = cfg?.label || String(item.label || catKey);
            const shareVal = itemRec[dataKey];
            const color =
              (cfg && "color" in cfg ? cfg.color : undefined) ||
              item.color ||
              defaultColors[idx % defaultColors.length];

            return (
              <div key={idx} className="flex items-center justify-between gap-1 text-xs">
                <div className="flex items-center gap-2 min-w-0">
                  <span
                    className="h-3 w-3 shrink-0 rounded-full shadow-xs"
                    style={{ backgroundColor: color }}
                  />
                  <span className="truncate font-medium text-stone-600 dark:text-stone-300" title={String(label)}>
                    {label}
                  </span>
                </div>
                {shareVal !== undefined && (
                  <span className="font-bold tabular-nums text-stone-900 dark:text-stone-100 shrink-0">
                    {String(shareVal)}%
                  </span>
                )}
              </div>
            );
          })}
        </div>
      </CardContent>
    </Card>
  );
}

export default Pattern;
