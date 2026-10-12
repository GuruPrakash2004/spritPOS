"""Backtest the demand forecasts on the shop's own history. READ-ONLY: only SELECTs on ai.ItemDailySales and the
item master; nothing is written to the database. Writes two files to --out (default backtest_out/).

  backtest_results.csv   one row per item class and method: forecast error, bias, missed stockouts, excess stock
  backtest_summary.txt   which method wins for each class

Run from backend/ after ai.ItemDailySales is filled (python -m app.ai.sales_history):
  .venv/Scripts/python.exe scripts/backtest.py --days 120 --step 7
  --no-model   skip the pretrained model (much faster)
  --detail     also write backtest_detail.csv, one row per item, origin and method
"""
import argparse
import csv
import sys
from datetime import date, timedelta
from pathlib import Path

sys.path.insert(0, str(Path(__file__).resolve().parent.parent))

from app import db                                  # noqa: E402
from app.ai import backtest, reorder                # noqa: E402
from app.ai.sales_history import zero_fill          # noqa: E402

CHUNK = 30


def load(start: date, end: date) -> tuple[dict[str, list[float]], dict[str, bool]]:
    """Every active item's zero-filled daily net sales start..end, and whether it is a non-liquor item."""
    items = db.fetchall("SELECT i.ITEMID AS id, ISNULL(g.LiqItem, 'Y') AS liq FROM MstITEM i WITH (NOLOCK) "
                        "LEFT JOIN MstITEMGROUP g WITH (NOLOCK) ON g.ITEMGROUPID = i.ITEMGroupID "
                        "WHERE i.ITEMID > 0 AND i.InActive = 'N'")
    rows: dict[int, list[dict]] = {}
    cur = start
    while cur <= end:                               # a month at a time: gentle on a live database
        stop = min(cur + timedelta(days=CHUNK - 1), end)
        for r in db.fetchall("SELECT ItemID AS id, SaleDate AS day, NetQty AS net FROM ai.ItemDailySales WITH (NOLOCK) "
                             "WHERE SaleDate >= ? AND SaleDate <= ?", (cur.isoformat(), stop.isoformat())):
            rows.setdefault(int(r["id"]), []).append(r)
        cur = stop + timedelta(days=1)
    series = {str(i["id"]): zero_fill(rows.get(int(i["id"]), []), start, end) for i in items}
    accessory = {str(i["id"]): str(i["liq"]).strip().upper() != "Y" for i in items}
    return series, accessory


def write_csv(path: Path, rows: list[dict]) -> None:
    if not rows:
        return
    with path.open("w", newline="", encoding="utf-8") as f:
        w = csv.DictWriter(f, fieldnames=list(rows[0]))
        w.writeheader()
        w.writerows(rows)


def main() -> int:
    ap = argparse.ArgumentParser(description=__doc__, formatter_class=argparse.RawDescriptionHelpFormatter)
    ap.add_argument("--days", type=int, default=120, help="how far back origins go (90 to 180)")
    ap.add_argument("--step", type=int, default=7, help="days between origins")
    ap.add_argument("--history", type=int, default=365, help="days of history to read before the newest origin")
    ap.add_argument("--no-model", action="store_true")
    ap.add_argument("--detail", action="store_true")
    ap.add_argument("--end", help="last day to test (YYYY-MM-DD); default yesterday")
    ap.add_argument("--out", default="backtest_out")
    args = ap.parse_args()
    if not 90 <= args.days <= 180:
        ap.error("--days must be between 90 and 180")

    if not db.init_db():
        print("No database connection (check backend/.env).")
        return 1
    end = date.fromisoformat(args.end) if args.end else date.today() - timedelta(days=1)
    series, accessory = load(end - timedelta(days=args.history + args.days - 1), end)
    if not any(any(s) for s in series.values()):
        print("ai.ItemDailySales has no sales in this period. Run: python -m app.ai.sales_history")
        return 1
    methods = backtest.default_methods(use_model=not args.no_model)
    if "chronos" in methods:
        from app.ai import model_forecast
        if not model_forecast.available():
            print("Pretrained model not available; continuing without it.")
            methods.pop("chronos")
    cfg = reorder.ReorderConfig.from_env()
    rows = backtest.run(series, accessory, end, methods, args.days, args.step, cfg)
    agg = backtest.aggregate(rows)
    out = Path(args.out)
    out.mkdir(parents=True, exist_ok=True)
    write_csv(out / "backtest_results.csv", agg)
    if args.detail:
        write_csv(out / "backtest_detail.csv", rows)
    text = backtest.summary(agg, end, args.days, args.step)
    (out / "backtest_summary.txt").write_text(text, encoding="utf-8")
    print(text)
    print(f"\nWritten to {out.resolve()}")
    return 0


if __name__ == "__main__":
    sys.exit(main())
