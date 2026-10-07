"""Whether the shop is open, for the Dashboard's "Store Open / Closed" chip.

No Google call and no key: the shop follows the Karnataka retail liquor shop timings (10:00 AM to 10:30 PM, India time,
every day of the week) and is closed on the dates in Holiday Master (the dry days and holidays the owner enters, saved in
the desktop's MstHoliday). Different timings can be set in backend/.env with SHOP_OPEN_TIME and SHOP_CLOSE_TIME
("HH:MM", 24-hour). status() works the answer out at every request, so the chip changes exactly on time.
"""
import os
from datetime import datetime, time, timedelta, timezone

from . import store

IST = timezone(timedelta(hours=5, minutes=30))
DEFAULT_OPEN = time(10, 0)
DEFAULT_CLOSE = time(22, 30)


def _time(name: str, default: time) -> time:
    raw = os.environ.get(name, "").strip()
    try:
        h, m = raw.split(":")
        return time(int(h), int(m))
    except ValueError:   # not set, or not "HH:MM"
        return default


def hours() -> tuple[time, time]:
    return _time("SHOP_OPEN_TIME", DEFAULT_OPEN), _time("SHOP_CLOSE_TIME", DEFAULT_CLOSE)


def _label(t: time) -> str:
    return t.strftime("%I:%M %p").lstrip("0")


def status(now_utc: datetime | None = None) -> dict:
    """{"configured": True, "open_now" (True / False), "today" (e.g. "10:00 AM – 10:00 PM", or the holiday), "place", "maps_url",
    "error", "holiday" (its name when today is one)}. `place` and `maps_url` are kept so the Dashboard's old response shape holds."""
    now = (now_utc or datetime.now(timezone.utc)).astimezone(IST)
    opens, closes = hours()
    holiday = store.HOLIDAYS.get(now.date().isoformat())
    if holiday:
        name = (holiday.get("name") or "Holiday").strip()
        return {"configured": True, "open_now": False, "today": f"Closed: {name}", "holiday": name, "place": "", "maps_url": "", "error": None}
    is_open = opens <= now.time().replace(second=0, microsecond=0) < closes
    return {"configured": True, "open_now": is_open, "today": f"{_label(opens)} – {_label(closes)}", "holiday": "",
            "place": "", "maps_url": "", "error": None}


def reset() -> None:
    """Nothing is cached any more; kept so older callers still work."""
