"""The shop's opening hours from Google Maps, for the Dashboard's "Store Open / Closed" chip.

The shop is the place picked on the map in Company Master (COMPANY["place_id"], read with Place Details), or else
it is looked up on Google Maps (Places API (New), Text Search) by the company's name and address from Company
Master; its regular opening hours are kept for SHOP_HOURS_CACHE_HOURS, and whether it is open is worked out from
them at every request, in the shop's own time zone (the place's UTC offset), so it changes on time without asking
Google again. The key is GOOGLE_MAPS_API_KEY in backend/.env only (never sent to the browser; it needs "Places API
(New)" enabled in Google Cloud). Without a key, without an address, or when Google doesn't know the shop's hours,
status() says so and the Dashboard shows no Open / Closed chip.
"""
import logging
import os
import threading
import time
from datetime import datetime, timedelta, timezone

import httpx

from . import store

logger = logging.getLogger("vlqms.shop_hours")

SEARCH_URL = "https://places.googleapis.com/v1/places:searchText"
DETAILS_URL = "https://places.googleapis.com/v1/places/"
FIELDS = ("places.id,places.displayName,places.formattedAddress,places.googleMapsUri,places.utcOffsetMinutes,"
          "places.regularOpeningHours,places.businessStatus")
CACHE_SECONDS = float(os.environ.get("SHOP_HOURS_CACHE_HOURS", "6") or 6) * 3600
RETRY_SECONDS = 600     # after a failed lookup, ask Google again at most every 10 minutes
TIMEOUT = 10

_lock = threading.Lock()
_cache: dict = {"query": None, "at": 0.0, "place": None, "error": None}


def _key() -> str:
    return os.environ.get("GOOGLE_MAPS_API_KEY", "").strip()


def _query() -> str:
    """What identifies the shop on Google Maps: the picked place, or its name and address (Company Master)."""
    if (store.COMPANY.get("place_id") or "").strip():
        return "place:" + store.COMPANY["place_id"].strip()
    name, address = (store.COMPANY.get("name") or "").strip(), (store.COMPANY.get("address") or "").strip()
    return ", ".join(p for p in (name, address) if p) if address else ""


def _lookup(query: str) -> dict | None:
    """The shop on Google Maps: the picked place, or the best match for the text; None when there is none.
    Raises httpx errors / RuntimeError for a refused request."""
    place_id = (store.COMPANY.get("place_id") or "").strip()
    if place_id:
        res = httpx.get(DETAILS_URL + place_id, timeout=TIMEOUT, params={"languageCode": "en"},
                        headers={"X-Goog-Api-Key": _key(), "X-Goog-FieldMask": FIELDS.replace("places.", "")})
    else:
        res = httpx.post(SEARCH_URL, timeout=TIMEOUT,
                         headers={"X-Goog-Api-Key": _key(), "X-Goog-FieldMask": FIELDS},
                         json={"textQuery": query, "languageCode": "en", "regionCode": "IN"})
    if res.status_code != 200:
        detail = ""
        try:
            detail = res.json().get("error", {}).get("message", "")
        except ValueError:
            pass
        raise RuntimeError(f"Google Maps answered {res.status_code}{': ' + detail if detail else ''}")
    if place_id:
        return res.json() or None
    places = res.json().get("places") or []
    return places[0] if places else None


def _place() -> tuple[dict | None, str | None]:
    """The cached place (looked up again when the address changed or the cache is old) and the last error."""
    query = _query()
    with _lock:
        age = time.monotonic() - _cache["at"]
        fresh = _cache["query"] == query and (age < CACHE_SECONDS if _cache["place"] else age < RETRY_SECONDS)
        if fresh:
            return _cache["place"], _cache["error"]
        place, error = None, None
        try:
            place = _lookup(query)
            if place is None:
                error = "The shop's address wasn't found on Google Maps"
        except (httpx.HTTPError, RuntimeError) as exc:
            error = str(exc) if isinstance(exc, RuntimeError) else "Google Maps can't be reached"
            logger.warning("Shop hours lookup failed: %s", error)
        _cache.update(query=query, at=time.monotonic(), place=place, error=error)
        return place, error


def _minute_of_week(day: int, hour: int, minute: int) -> int:
    """Google's day is 0 = Sunday."""
    return (day % 7) * 1440 + hour * 60 + minute


def open_now(periods: list[dict], now_local: datetime) -> bool:
    """Whether the shop is open at this local time, from Google's weekly periods (a period may run past midnight;
    one period with no close means open all the time)."""
    if len(periods) == 1 and "close" not in periods[0]:
        return True
    week = 7 * 1440
    now = _minute_of_week((now_local.weekday() + 1) % 7, now_local.hour, now_local.minute)
    for p in periods:
        o, c = p.get("open"), p.get("close")
        if not o or not c:
            continue
        start = _minute_of_week(o.get("day", 0), o.get("hour", 0), o.get("minute", 0))
        end = _minute_of_week(c.get("day", 0), c.get("hour", 0), c.get("minute", 0))
        if end <= start:
            end += week
        if start <= now < end or start <= now + week < end:
            return True
    return False


def status(now_utc: datetime | None = None) -> dict:
    """{"configured", "open_now" (True/False, or None when unknown), "today" (e.g. "10:00 AM – 10:30 PM"),
    "place", "maps_url", "error"}."""
    if not _key():
        return {"configured": False, "open_now": None, "today": "", "place": "", "maps_url": "",
                "error": "GOOGLE_MAPS_API_KEY is not set in backend/.env"}
    if not _query():
        return {"configured": True, "open_now": None, "today": "", "place": "", "maps_url": "",
                "error": "Fill in the company address in Company Master"}
    place, error = _place()
    out = {"configured": True, "open_now": None, "today": "", "place": "", "maps_url": "", "error": error}
    if not place:
        return out
    out["place"] = (place.get("displayName") or {}).get("text", "")
    out["maps_url"] = place.get("googleMapsUri", "")
    if place.get("businessStatus") in ("CLOSED_PERMANENTLY", "CLOSED_TEMPORARILY"):
        return out | {"open_now": False, "today": "Closed on Google Maps"}
    hours = place.get("regularOpeningHours") or {}
    periods = hours.get("periods") or []
    if not periods:
        return out | {"error": "Google Maps has no opening hours for this shop"}
    now_local = (now_utc or datetime.now(timezone.utc)) + timedelta(minutes=place.get("utcOffsetMinutes") or 330)
    days = hours.get("weekdayDescriptions") or []   # Monday first, e.g. "Monday: 10:00 AM – 10:30 PM"
    today = days[now_local.weekday()].split(":", 1)[-1].strip() if len(days) == 7 else ""
    return out | {"open_now": open_now(periods, now_local), "today": today, "error": None}


def reset() -> None:
    with _lock:
        _cache.update(query=None, at=0.0, place=None, error=None)
