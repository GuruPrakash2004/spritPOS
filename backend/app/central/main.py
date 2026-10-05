"""The central site's API: shops push here (Web Sync), owners read every shop here.

Run on the hosted server, next to (not instead of) nothing else of the shop app:
    .venv/bin/python -m uvicorn app.central.main:app --port 8002
Settings: app/central/config.py (CENTRAL_DB_*, CENTRAL_JWT_SECRET in that server's backend/.env). Shops and
users are added with `tools/central.py` (it prints each shop's sync key and each user's first password
once).

  POST /sync           a shop's day (sync key): replaces that shop's rows for the day, as the desktop's
                       deleteForTodaysData + pushData do in its central database
  POST /sync/stock     a shop's current stock (sync key): replaces that shop's RptWebStk
  GET  /auth/transport-key, POST /auth/login, POST /auth/change-password, GET /auth/me
  GET  /clients        the shops this user sees, with when each last synced
  GET  /dsr            one shop's day, or every visible shop's together (client blank)
  GET  /stock          an item's stock in each shop
  GET  /indents        the day's purchase indents
"""
import logging
import os
from collections import defaultdict
from datetime import date, timedelta

from fastapi import Depends, FastAPI, HTTPException, Query, Request
from fastapi.responses import JSONResponse
from pydantic import BaseModel, Field

from .. import passwords, secure_fields
from . import auth, config, tables as T
from .repo import Unavailable

logger = logging.getLogger("vlqms.central")
MAX_BODY_BYTES = 25 * 1024 * 1024   # a day's push is small; the stock of 100 000 items fits well inside

_docs = os.environ.get("CENTRAL_DOCS", "") == "1"
app = FastAPI(title="vLQMS Central", version="1.0.0", docs_url="/docs" if _docs else None,
              redoc_url=None, openapi_url="/openapi.json" if _docs else None)
# Compared against when the user name is unknown, so a wrong name takes as long as a wrong password.
_DUMMY_HASH = passwords.hash_password("not-a-real-password-1")


@app.exception_handler(Unavailable)
async def _unavailable(request: Request, exc: Unavailable):
    return JSONResponse({"detail": str(exc)}, status_code=503)


@app.on_event("startup")
def _startup() -> None:
    config.connect()


@app.middleware("http")
async def limit_body_and_headers(request: Request, call_next):
    length = request.headers.get("content-length")
    if length and length.isdigit() and int(length) > MAX_BODY_BYTES:
        return JSONResponse({"detail": "The push is too large"}, status_code=413)
    response = await call_next(request)
    response.headers["X-Content-Type-Options"] = "nosniff"
    response.headers["X-Frame-Options"] = "DENY"
    response.headers["Referrer-Policy"] = "no-referrer"
    response.headers["Cache-Control"] = "no-store"
    return response


@app.get("/")
def health():
    return {"service": "vLQMS central", "store": config.REPO.kind}


# ─── Pushes from the shops ───────────────────────────────────────────────────

class DayPush(BaseModel):
    day: date
    tables: dict


class StockPush(BaseModel):
    as_of: str | None = Field(None, max_length=40)
    items: list


@app.post("/sync")
def sync_day(body: DayPush, client: dict = Depends(auth.pushing_client)):
    if body.day > date.today() + timedelta(days=1):   # a day of slack for clocks and time zones
        raise HTTPException(422, "That day hasn't come yet")
    try:
        rows = T.clean_day(body.tables)
    except T.BadPush as exc:
        raise HTTPException(422, str(exc)) from None
    config.REPO.replace_day(client["id"], body.day.isoformat(), rows)
    return {"ok": True, "client": client["code"], "day": body.day.isoformat(),
            "rows": {name: len(r) for name, r in rows.items() if r}}


@app.post("/sync/stock")
def sync_stock(body: StockPush, client: dict = Depends(auth.pushing_client)):
    try:
        rows = T.clean_stock(body.items)
        config.REPO.replace_stock(client["id"], rows)
    except (T.BadPush, ValueError) as exc:
        raise HTTPException(422, str(exc)) from None
    return {"ok": True, "client": client["code"], "items": len(rows)}


# ─── Sign-in ─────────────────────────────────────────────────────────────────

class LoginIn(BaseModel):
    user: str = Field(max_length=30)
    password: str = Field(max_length=1000)


class ChangePasswordIn(BaseModel):
    current_password: str = Field(max_length=1000)
    new_password: str = Field(max_length=1000)


@app.get("/auth/transport-key")
def transport_key():
    """The public key the browser encrypts passwords with (as on the shop app's sign-in page)."""
    return secure_fields.public_key()


@app.post("/auth/login")
def login(body: LoginIn, request: Request):
    name = body.user.strip().upper()
    password = secure_fields.decrypt(body.password, "login")
    user_key, addr_key = f"user:{name}", f"addr:{auth.address(request)}"
    auth.check_not_locked([(user_key, auth.MAX_FAILS_PER_USER), (addr_key, auth.MAX_FAILS_PER_ADDRESS)])
    user = config.REPO.user(name)
    ok = passwords.verify_password(password or "", user["password_hash"] if user else _DUMMY_HASH)
    if not user or not ok:
        auth.record_failure([user_key, addr_key])
        raise HTTPException(401, "Invalid user name or password")
    if not user["active"]:
        raise HTTPException(403, "This user is inactive. Ask the central administrator.")
    auth.clear_failures(user_key)
    return auth.session(user)


@app.post("/auth/change-password")
def change_password(body: ChangePasswordIn, request: Request, user: dict = Depends(auth.signed_in_user)):
    current = secure_fields.decrypt(body.current_password, "current", "Current password") or ""
    new = secure_fields.decrypt(body.new_password, "new", "New password") or ""
    user_key, addr_key = f"user:{user['user']}", f"addr:{auth.address(request)}"
    auth.check_not_locked([(user_key, auth.MAX_FAILS_PER_USER), (addr_key, auth.MAX_FAILS_PER_ADDRESS)])
    if not passwords.verify_password(current, user["password_hash"]):
        auth.record_failure([user_key, addr_key])
        raise HTTPException(422, "Your current password is not correct")
    if new == current:
        raise HTTPException(422, "Choose a password different from the current one")
    problem = passwords.password_problem(new, user["user"])
    if problem:
        raise HTTPException(422, problem)
    config.REPO.set_password(user["user"], passwords.hash_password(new), must_change=False)
    return auth.session(config.REPO.user(user["user"]))


@app.get("/auth/me")
def me(user: dict = Depends(auth.signed_in_user)):
    return {"user": user["user"], "role": user["role"], "must_change": user["must_change"]}


# ─── What the owner sees ─────────────────────────────────────────────────────

def _clients(user: dict, code: str | None) -> list[dict]:
    """The visible shops, or just `code` (404 when it isn't one of them)."""
    ids = auth.visible_client_ids(user)
    shops = config.REPO.clients(ids)
    if code:
        shops = [c for c in shops if c["code"] == code.strip().upper()]
        if not shops:
            raise HTTPException(404, f"No shop '{code}' for you")
    return shops


@app.get("/clients")
def clients(user: dict = Depends(auth.current_user)):
    return _clients(user, None)


SUMMED_TEXT = {"LastPurBillNo", "LastPurDate"}


def _merge(rows: list[dict], key: str, value_cols: list[str], top: int | None = None, sort_by: str | None = None) -> list[dict]:
    """Rows of several shops added up by `key` (all shops together)."""
    acc: dict[str, dict] = {}
    for r in rows:
        k = r[key] or ""
        cur = acc.setdefault(k, {key: k, **{c: 0 for c in value_cols}})
        for c in value_cols:
            cur[c] += r.get(c) or 0
    out = list(acc.values())
    for r in out:
        for c in value_cols:
            r[c] = round(r[c], 2)
    if sort_by:
        out.sort(key=lambda r: (-r[sort_by], r[key]))
    else:
        out.sort(key=lambda r: str(r[key]))
    return out[:top] if top else out


@app.get("/dsr")
def dsr(day: date, client: str | None = None, user: dict = Depends(auth.current_user)):
    """One shop's day as it pushed it, or (client blank) every visible shop's together: the summary added
    up, lists combined by name, and one line per shop."""
    shops = _clients(user, client)
    by_id = {c["id"]: c for c in shops}
    rows = config.REPO.day_rows(list(by_id), day.isoformat())
    code = lambda r: by_id[r["ClientID"]]["code"]
    strip = lambda r: {k: v for k, v in r.items() if k != "ClientID"}

    smry_rows = rows["RptDSRSmry"]
    per_shop = []
    for c in shops:
        s = next((r for r in smry_rows if r["ClientID"] == c["id"]), None)
        per_shop.append({"client": c["code"], "name": c["name"], "last_sync": c["last_sync"], "synced": s is not None,
                         **({k: s[k] for k in ("TotBills", "TotSalesAmt", "SalesProfitAmt", "ClStkQty", "CLStkValue",
                                                "TotCashAmt", "ExpenseAmt", "CashBalanceAmt")} if s else {})})

    if client:
        result_tables = {name: [strip(r) for r in rs] for name, rs in rows.items() if name != "RptDSRSmry"}
        summary = strip(smry_rows[0]) if smry_rows else None
    else:
        summary = None
        if smry_rows:
            summary = {col: 0 for col, kind in T.DAY_TABLES["RptDSRSmry"].items() if col not in SUMMED_TEXT}
            for r in smry_rows:
                for col in summary:
                    summary[col] = round(summary[col] + (r.get(col) or 0), 2)
        tag = lambda rs, col: [{**strip(r), col: f"{code(r)} / {r[col]}"} for r in rs]
        result_tables = {
            "RptDSRCtrSales": tag(rows["RptDSRCtrSales"], "CounterNo"),
            "RptDSRInvCancelled": tag(rows["RptDSRInvCancelled"], "CounterNo"),
            "RptDSRSPWise": tag(rows["RptDSRSPWise"], "CounterNo"),
            "RptDSRGroupWiseSales": _merge(rows["RptDSRGroupWiseSales"], "GroupName", ["TotQty", "TotAmt"]),
            "RptDSRGroupWisePur": _merge(rows["RptDSRGroupWisePur"], "GroupName", ["TotQty", "TotAmt"]),
            "RptDSRExpLedWise": _merge(rows["RptDSRExpLedWise"], "LedgerName", ["LedAmt"]),
            "RptDSR_Gst": _merge(rows["RptDSR_Gst"], "GST", ["GSTAmt"]),
            "RptDSRPmt": _merge(rows["RptDSRPmt"], "Particulars", ["PmtAmt"]),
            "RptFastMov": _merge(rows["RptFastMov"], "ItemName", ["SoldQty"], top=20, sort_by="SoldQty"),
            "RptMinStk": tag(rows["RptMinStk"], "ItemName"),
            "TrnHdrPI": [{**strip(r), "client": code(r)} for r in rows["TrnHdrPI"]],
            "TrnDtlPI": [{**strip(r), "client": code(r)} for r in rows["TrnDtlPI"]],
        }
    return {"day": day.isoformat(), "client": client.strip().upper() if client else None, "summary": summary,
            "shops": per_shop, "tables": result_tables}


@app.get("/stock")
def stock(q: str = Query("", max_length=60), client: str | None = None, limit: int = Query(300, ge=1, le=2000),
          user: dict = Depends(auth.current_user)):
    """Each matching item with its stock in every visible shop (or one shop). Without `q` and `client`, the
    list would be every item of every shop: ask for at least two letters then."""
    q = q.strip()
    if not client and len(q) < 2:
        raise HTTPException(422, "Type at least two letters of the item, or pick a shop")
    shops = _clients(user, client)
    by_id = {c["id"]: c for c in shops}
    rows = config.REPO.stock(list(by_id), q, limit * max(1, len(by_id)))
    items: dict[str, dict] = defaultdict(dict)
    for r in rows:
        it = items[r["ItemCode"]]
        if not it:
            it.update(item_code=r["ItemCode"], item_name=r["ItemName"], group=r["ItemGroupName"], brand=r["Brand"],
                      package=r["Package"], mrp=r["MRP"], total_qoh=0, shops=[])
        it["shops"].append({"client": by_id[r["ClientID"]]["code"], "qoh": r["QOH"], "min_level": r["MinLevel"],
                            "short": (r["MinLevel"] or 0) > 0 and r["QOH"] < r["MinLevel"]})
        it["total_qoh"] = round(it["total_qoh"] + (r["QOH"] or 0), 3)
    out = sorted(items.values(), key=lambda i: i["item_name"] or "")[:limit]
    return {"shops": [{"client": c["code"], "name": c["name"], "last_stock_sync": c["last_stock_sync"]} for c in shops],
            "items": out}


@app.get("/indents")
def indents(day: date, client: str | None = None, user: dict = Depends(auth.current_user)):
    shops = _clients(user, client)
    by_id = {c["id"]: c for c in shops}
    rows = config.REPO.day_rows(list(by_id), day.isoformat())
    lines = defaultdict(list)
    for d in rows["TrnDtlPI"]:
        lines[(d["ClientID"], d["PIID"])].append({k: v for k, v in d.items() if k != "ClientID"})
    out = []
    for h in sorted(rows["TrnHdrPI"], key=lambda h: (by_id[h["ClientID"]]["code"], h["PINO"] or "")):
        out.append({"client": by_id[h["ClientID"]]["code"], **{k: v for k, v in h.items() if k != "ClientID"},
                    "lines": sorted(lines[(h["ClientID"], h["PIID"])], key=lambda d: d["SlNo"])})
    return out
