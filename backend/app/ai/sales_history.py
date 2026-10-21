"""Daily sales per item for the AI forecast: net of returns, cancelled bills left out, in bottles.

Reads the desktop's sales tables (read-only, WITH (NOLOCK), always limited by date) and keeps the result
in ai.ItemDailySales (migrations/ai_001.sql). The refresh is incremental: it asks the table for its
latest date and reads only the days after it, plus the last few days again (a bill cancelled or edited
the next morning changes a day already stored). The first run reads BACKFILL_DAYS back, in small date
chunks, one transaction per chunk, so a live shop is never held up and a failed run resumes where it stopped.

Run after billing hours:   python -m app.ai.sales_history        (from backend/, needs AI_FORECAST_ENABLED=1)

What counts as a sale (the same rules as the desktop's own Re Write Stock check):
  TrnHdrSINV.Cancelled = 'N' AND IsDelete = 'N', quantity TrnDtlSINV.SINVQty (bottles).
Returns are separate documents: TrnHdrSR / TrnDtlSR (SRQty) with Cancelled = 'N'. That table is optional
in the desktop; where it is missing, returns are taken as 0.

Only days that had sales or returns are stored. zero_fill() / load_series() add the empty days when a
series is read, which keeps the table small on a live database.
Items are keyed by MstITEM.ITEMID. Items merged away in the web app (store.MERGED_ITEM_IDS) keep their old
id here; the forecast layer maps ids to item codes.
"""
import logging
from collections import defaultdict
from datetime import date, datetime, timedelta

from .. import db
from . import enabled

logger = logging.getLogger("vlqms.ai")

BACKFILL_DAYS = 365      # first run: how far back to read
LOADED_THROUGH: date | None = None   # the day the last refresh read up to (days without a sale are not stored, so last_loaded() can be older)
LOOKBACK_DAYS = 3        # every run re-reads this many of the latest stored days
CHUNK_DAYS = 14          # dates read and saved per transaction

_SALES_SQL = """
    SELECT d.ITEMID AS item_id, CAST(h.SINVDate AS date) AS day, SUM(d.SINVQty) AS qty
    FROM TrnHdrSINV h WITH (NOLOCK)
    JOIN TrnDtlSINV d WITH (NOLOCK) ON d.SINVID = h.SINVID
    WHERE h.Cancelled = 'N' AND h.IsDelete = 'N' AND h.SINVDate >= ? AND h.SINVDate < ?
    GROUP BY d.ITEMID, CAST(h.SINVDate AS date)"""

_RETURNS_SQL = """
    SELECT d.ITEMID AS item_id, CAST(h.SRDate AS date) AS day, SUM(d.SRQty) AS qty
    FROM TrnHdrSR h WITH (NOLOCK)
    JOIN TrnDtlSR d WITH (NOLOCK) ON d.SRID = h.SRID
    WHERE h.Cancelled = 'N' AND h.SRDate >= ? AND h.SRDate < ?
    GROUP BY d.ITEMID, CAST(h.SRDate AS date)"""


def _day(value) -> date:
    if isinstance(value, datetime):
        return value.date()
    if isinstance(value, date):
        return value
    return date.fromisoformat(str(value)[:10])


# ---------- pure helpers (unit tested without a database) ----------

def plan_window(last_loaded: date | None, through: date, backfill_days: int = BACKFILL_DAYS,
                lookback_days: int = LOOKBACK_DAYS) -> tuple[date, date] | None:
    """The inclusive date range to read, or None when there is nothing new. through is the last day to include."""
    if last_loaded is None:
        start = through - timedelta(days=max(backfill_days, 1) - 1)
    else:
        start = last_loaded - timedelta(days=max(lookback_days, 1) - 1)
    return (start, through) if start <= through else None


def chunk_dates(start: date, end: date, size: int = CHUNK_DAYS) -> list[tuple[date, date]]:
    """Split start..end (inclusive) into consecutive inclusive ranges of at most `size` days."""
    out, cur = [], start
    while cur <= end:
        stop = min(cur + timedelta(days=max(size, 1) - 1), end)
        out.append((cur, stop))
        cur = stop + timedelta(days=1)
    return out


def combine(sales: dict[tuple[int, date], float], returns: dict[tuple[int, date], float]) -> list[dict]:
    """One row per (item, day) that has sales or returns: sold, returned and net = sold - returned."""
    rows = []
    for key in sorted(set(sales) | set(returns), key=lambda k: (k[1], k[0])):
        sold, ret = float(sales.get(key, 0)), float(returns.get(key, 0))
        rows.append({"item_id": key[0], "day": key[1], "sold": sold, "returned": ret, "net": sold - ret})
    return rows


def zero_fill(rows: list[dict], start: date, end: date) -> list[float]:
    """Net bottles for every day start..end inclusive, 0 where the item had no sales (one item's rows)."""
    by_day = defaultdict(float)
    for r in rows:
        by_day[_day(r["day"])] += float(r["net"])
    return [by_day.get(start + timedelta(days=i), 0.0) for i in range((end - start).days + 1)]


# ---------- database ----------

def last_loaded() -> date | None:
    row = db.fetchone("SELECT MAX(SaleDate) AS d FROM ai.ItemDailySales")
    return _day(row["d"]) if row and row.get("d") else None


def _read(sql: str, start: date, end: date) -> dict[tuple[int, date], float]:
    """Sum per (item, day) for start..end inclusive (the query takes an exclusive upper bound)."""
    rows = db.fetchall(sql, (start.isoformat(), (end + timedelta(days=1)).isoformat()))
    out: dict[tuple[int, date], float] = {}
    for r in rows:
        if r.get("item_id") is None or r.get("day") is None:
            continue
        key = (int(r["item_id"]), _day(r["day"]))
        out[key] = out.get(key, 0.0) + float(r.get("qty") or 0)
    return out


def read_sales(start: date, end: date) -> dict[tuple[int, date], float]:
    return _read(_SALES_SQL, start, end)


def read_returns(start: date, end: date) -> dict[tuple[int, date], float]:
    """Returns per (item, day); {} when this shop's database has no TrnDtlSR."""
    try:
        return _read(_RETURNS_SQL, start, end)
    except Exception as exc:   # table missing or unreadable: sales still load, the log says why
        logger.warning("Sales returns not read (%s); treated as 0", db.safe_error(exc)[:200])
        return {}


def save_rows(start: date, end: date, rows: list[dict]) -> None:
    """Replace start..end in ai.ItemDailySales with `rows`, in one transaction (all or nothing)."""
    with db.transaction() as cur:
        cur.execute("DELETE FROM ai.ItemDailySales WHERE SaleDate >= ? AND SaleDate <= ?",
                    (start.isoformat(), end.isoformat()))
        for r in rows:
            cur.execute("INSERT INTO ai.ItemDailySales (ItemID, SaleDate, SoldQty, ReturnQty, NetQty) VALUES (?, ?, ?, ?, ?)",
                        (r["item_id"], r["day"].isoformat(), r["sold"], r["returned"], r["net"]))


def _mark(through: date) -> None:
    global LOADED_THROUGH
    LOADED_THROUGH = through


def refresh(through: date | None = None, backfill_days: int = BACKFILL_DAYS) -> dict:
    """Bring ai.ItemDailySales up to `through` (default: yesterday, the last complete day).
    Returns {"status", "from", "to", "days", "rows"}; status is "skipped" (flag off, no database), "up-to-date" or "ok"."""
    if not enabled():
        return {"status": "skipped", "reason": "AI_FORECAST_ENABLED is off"}
    if not db.is_db_available():
        return {"status": "skipped", "reason": "no database connection"}
    through = through or (date.today() - timedelta(days=1))
    window = plan_window(last_loaded(), through, backfill_days)
    if window is None:
        _mark(through)
        return {"status": "up-to-date", "to": through.isoformat(), "days": 0, "rows": 0}
    total = 0
    for start, end in chunk_dates(*window):
        rows = combine(read_sales(start, end), read_returns(start, end))
        save_rows(start, end, rows)
        total += len(rows)
        logger.info("AI sales history %s..%s: %d rows", start, end, len(rows))
    _mark(through)
    return {"status": "ok", "from": window[0].isoformat(), "to": window[1].isoformat(),
            "days": (window[1] - window[0]).days + 1, "rows": total}


def load_series(item_id: int, start: date, end: date) -> list[float]:
    """One item's net bottles per day, start..end inclusive, zero-filled."""
    rows = db.fetchall("SELECT SaleDate AS day, NetQty AS net FROM ai.ItemDailySales "
                       "WHERE ItemID = ? AND SaleDate >= ? AND SaleDate <= ?",
                       (item_id, start.isoformat(), end.isoformat()))
    return zero_fill(rows, start, end)


if __name__ == "__main__":   # the after-hours job (Windows Task Scheduler / cron)
    import json
    import sys

    logging.basicConfig(level=logging.INFO, format="%(asctime)s %(name)s %(message)s")
    if not db.init_db():
        print(json.dumps({"status": "skipped", "reason": "database not configured or unreachable"}))
        sys.exit(1)
    print(json.dumps(refresh()))
