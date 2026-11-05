# "Restock immediately": how it works, the model behind it, and how accurate it is

This explains the Dashboard's **Restock immediately** panel and the forecasting engine under it (`backend/app/ai/`).
Everything here was read from the source and from the shop's own backtest (`backend/backtest_out/`, run to 2026-09-10).
Where a number is not measured, it says so.

## 1. Why it matters

A liquor shop loses money in two ways: **running out** of a fast seller (a lost sale, often a lost customer) and
**over-buying** (cash tied up in slow bottles). "Restock immediately" is the daily answer to *"what must I order
today, and how much?"*. It puts the items that are about to run out, ranked by how fast they sell, in front of the
owner or manager the moment the Dashboard opens, with a quantity already worked out in whole cases. It also feeds
the draft Purchase Indent flow, so the answer turns into an order in a few clicks.

## 2. What the user sees

- **Dashboard panel** (`frontend/src/components/dashboard/RestockCarousel.jsx`): up to 15 item cards in a sliding
  carousel, most urgent first. Each card has a status badge, days of stock left, average daily sales and a
  suggested order quantity.
- **Statuses**: `OUT_OF_STOCK`, `CRITICAL`, `LOW`, `OK`. Only items that actually sell can need restock.
- **Fast / Medium / Slow tag**: relative to the shop's own sellers (top fifth that sold = Fast, next 30% = Medium,
  the rest = Slow), so it suits a small or a large shop.
- Access follows Admin Master → Field Rights → Dashboard: a role with the panel hidden gets `403` from the endpoint.

## 3. The request path (what happens when the panel loads)

1. The Dashboard calls `GET /reports/restock` (`backend/app/routers/reports.py`).
2. **Live stock and sales.** With SQL Server connected, current stock (`TrnQOH.CLBal`) and the last 15 days of
   sales are read live (cached 20 s). Bills saved in this app since it started are added on top, so a sale made one
   minute ago is already counted. Cancelled and held bills never count.
3. **The forecast overlay** (`ai_service.restock_overlay`). If forecasting is on and the forecasts are ready, each
   item the model could forecast takes its demand, days left, status and order quantity from the forecast.
   Everything else keeps the older formula (section 7). The panel **never waits** for the forecast: while it is
   building in the background, the older formula answers, so the page is always fast.
4. Rows are ranked by `priority = avg_daily / (days_left + 1)`: it grows with how fast an item sells and how close
   it is to running out.
5. Each row says `forecast: "ai"` or `"formula"` and carries a plain-language reason (`ai_reason`).

## 4. How the forecast is made

Pipeline, in `backend/app/ai/`:

| Step | File | What it does |
|---|---|---|
| Sales history | `sales_history.py` | Daily net sales per item in bottles (cancelled and deleted bills out, returns taken off), kept in `ai.ItemDailySales`. First run backfills 365 days in small chunks; after that only new days, plus the last few days again because a bill can be cancelled the next morning. |
| Bootstrap | `bootstrap.py` | At backend start, in a background thread: creates the `ai.*` tables if missing, loads history, builds forecasts, and repeats every 6 hours. A failure is only logged. |
| Forecast | `forecast.py`, `model_forecast.py` | A 14-day demand forecast per item (below). |
| Reorder rules | `reorder.py` | Turns forecast + stock into status and quantity (section 5). |
| Service | `service.py` | Joins them. Forecasts are kept for 1 hour; recommendations are recomputed on every call from the stock at that moment. Refuses history more than 3 days old, because missing days would read as "no sales". |

Items are first **classified** from their own history:

| Class | Rule | Method used |
|---|---|---|
| `insufficient` | under 28 days of history, or fewer than 3 days with a sale | no model; average daily sales if anything sold, otherwise the item's ROL/ROQ |
| `accessory` | the item's group is not liquor | day-of-week weighted average |
| `fast` | sold on at least 50% of the last 90 days | day-of-week weighted average |
| `slow` | everything else | TSB |

### Models actually in use

- **Day-of-week weighted moving average (`dow_wma`)** for fast sellers. Each weekday's forecast is the average of
  that weekday's last 8 values, newest weighted most. It captures the weekly rhythm of a shop (weekends sell more).
- **TSB (Teunter-Syntetos-Babai)** for slow, intermittent sellers: smoothed sale size × smoothed probability of a
  sale on any day. Unlike Croston's method it decays when sales stop, so a dead item stops being suggested.
- **Fallbacks** for new items: the older average-sales formula, then ROL/ROQ.
- **Optional pretrained model: Amazon Chronos-Bolt-Tiny** (`amazon/chronos-bolt-tiny`, about 9 million parameters,
  zero-shot, runs on CPU, downloaded once from Hugging Face then offline). It is **off by default** (`AI_USE_MODEL=1`
  turns it on). Its forecast is the average of its 10% to 90% quantile forecasts, because its own "mean" output is
  only the median and collapses to about 0 for items that sell on some days only. If the packages or weights are
  missing, or anything fails, the statistical methods take over silently, so the shop never loses its forecast.

**Why the model is off by default:** the shop's backtest favoured the statistical methods on stockouts and bias
(comment in `forecast.py` and `.env.example`). Honest limit: the saved backtest results contain no Chronos rows, so
a head-to-head number for Chronos is not on file here.

## 5. From forecast to "order this many"

```
reorder point = forecast demand over the lead time + safety stock
target stock  = forecast demand over lead time + cover days + safety stock
order qty     = target - stock on hand, kept within MINLEVEL / MAXLEVEL when set, rounded UP to whole cases
```

Defaults (all in `backend/.env`): lead time 3 days (`AI_LEAD_TIME_DAYS`), safety 2 days (`AI_SAFETY_DAYS`), cover
14 days (`AI_COVER_DAYS`), "soon" window 7 days (`AI_SOON_DAYS`), service level 0.92 (`AI_SERVICE_LEVEL`).

**Safety stock** = z × the item's own day-to-day variation × √(days), never below 2 days of average demand
(and at least Poisson variation for sparse items). With a 0.92 service level, z is about 1.41. It learns from each
item's own sales, so it needs no per-shop tuning; one setting moves the whole shop up or down.

Status: **Reorder now** (stock at or under the reorder point) → `CRITICAL`; **Reorder soon** (will reach it within
7 days) → `LOW`; otherwise `OK`; stock at 0 → `OUT_OF_STOCK`. Stock below zero is treated as zero. There is no
purchase-order table in the shop's database, so pending orders are ignored (owner's decision): if you already
ordered an item, the panel still shows it until the stock arrives.

## 6. Accuracy: what was measured

The shop's data was backtested read-only (`backend/scripts/backtest.py`, results in `backend/backtest_out/`): origins
every 7 days over the last 120 days, 14-day forecasts, each method seeing only history before the origin. Data to
2026-09-10; 4,143 insufficient-history items, 887 slow items and 210 fast items.

Measures: **WAPE** = total absolute error ÷ total actual sales (lower is better; 0.40 means the 14-day forecast is
off by 40% of what sold, per item, in total). **Bias** = over/under forecast overall. **Coverage** = share of
order cycles where the order-up-to level would have covered what actually sold in lead + cover days (higher means
fewer stockouts). It tests the stock level each method would aim for; it does not replay the shop's real stock
history.

| Class | Method | WAPE | Bias | Coverage | Excess units |
|---|---|---|---|---|---|
| fast (210 items) | **dow_wma (used)** | **0.402** | -1.9% | 82.8% | 100,400 |
| fast | current formula | 0.421 | -0.8% | 82.1% | 105,772 |
| fast | TSB | 0.418 | +0.9% | 84.6% | 110,407 |
| fast | Croston | 0.430 | +2.5% | 85.4% | 116,487 |
| slow (887 items) | **TSB (used)** | **1.411** | -1.4% | 86.2% | 52,507 |
| slow | current formula | 1.415 | -4.2% | 83.1% | 52,187 |
| slow | dow_wma | 1.444 | -0.9% | 84.9% | 52,198 |
| slow | Croston | 2.601 | +119.3% | 90.4% | 116,773 |
| insufficient (4,143 items) | current formula | 1.344 | -11.0% | 46.5% | 6,047 |

How to read this honestly:

- **Fast sellers, the ones that matter most, are forecast to roughly 60% accuracy per item per 14 days** (WAPE 0.40),
  with almost no overall bias (-1.9%). That is a better result than the previous 15-day average (0.42) and every
  other method tested.
- **Slow sellers are inherently noisy** (WAPE above 1.0 for every method: a bottle that sells twice a month cannot
  be predicted to the day). TSB is the best of them and, importantly, almost unbiased. Croston was rejected: it
  over-forecast by 119%.
- **Coverage is about 83% to 86% for the methods in use**, which is the target band (80% to 90%) the service level
  was tuned for: `AI_SERVICE_LEVEL=0.92` was chosen because it gave 83-86% covered cycles on this shop's data.
  Practically: roughly 5 in 6 reorder cycles would not have run out. Raise `AI_SERVICE_LEVEL` if a shop wants fewer
  stockouts and accepts more stock; lower it to hold less cash in stock.
- **Items with little history (the bulk of the catalogue by count) are the weak spot**: WAPE 1.34 and only 46.5%
  coverage with the old formula. They sell rarely, and the app does not pretend otherwise: they are labelled
  "Insufficient history" and fall back to the item's own ROL/ROQ.
- The coverage figures in the saved CSV and the 0.92 comment were measured on different runs; treat 83-86% as the
  measured range, not a guarantee. Re-run the backtest on the shop's current history to refresh it:
  `.venv/Scripts/python.exe scripts/backtest.py --days 120 --step 7` from `backend/`.
- **Not measured:** the effect of festivals, price changes and supply gaps (a stocked-out day looks like a no-sale
  day and can pull the forecast down), and any live A/B result of orders actually placed from the suggestions.

## 7. When the forecast isn't available

The panel degrades, it never breaks. Forecasting off (`AI_FORECAST_ENABLED=0`), history missing or stale, database
offline, or forecasts still building: `/reports/restock` uses the original formula (average daily sales of the last
15 days; CRITICAL at 3 days of stock or less, LOW at 7 days or under the ROL; suggested quantity covers 14 days,
never below the ROQ). Items with too little history keep this formula even when the AI is on.

## 8. From suggestion to purchase order

`/ai/*` (`backend/app/routers/ai.py`): `GET /ai/recommendations`, `GET /ai/forecast/{item}` (the 14-day curve and
the reason), `POST /ai/suggestions/generate` (saves drafts in `ai.IndentSuggestion`, one open draft per item) and
approve / reject. **A suggestion is only a draft.** Approving one creates a normal Purchase Indent through the
existing indent code (same rights, same audit), where the user can change the quantity. Nothing is ordered
automatically, and the AI never writes to the desktop's own tables: it uses its own `ai.*` tables, created
automatically if the database login may.

## 9. Settings cheat sheet (`backend/.env`)

| Setting | Default | Effect |
|---|---|---|
| `AI_FORECAST_ENABLED` | 1 | Turns the whole feature on/off (404 on `/ai/*` when off; panel uses the old formula) |
| `AI_USE_MODEL` | off | Use Chronos-Bolt instead of the statistical methods |
| `AI_FORECAST_MODEL` | `amazon/chronos-bolt-tiny` | Another Chronos-Bolt model |
| `AI_HISTORY_DAYS` | 365 | Days of past sales learned from |
| `AI_LEAD_TIME_DAYS` / `AI_SAFETY_DAYS` / `AI_COVER_DAYS` / `AI_SOON_DAYS` | 3 / 2 / 14 / 7 | Reorder rules |
| `AI_SERVICE_LEVEL` | 0.92 | Safety stock confidence (0.50 to 0.99) |

Extra install for the optional model: `backend/requirements-ai.txt` (torch, chronos-forecasting).
