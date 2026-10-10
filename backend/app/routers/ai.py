"""/ai: AI stock forecast, reorder recommendations and draft purchase-indent suggestions.

Everything is behind AI_FORECAST_ENABLED (404 while off). Suggestions are drafts only: approving one makes a normal
Purchase Indent through the existing indent code (transactions.create_indent_doc); no other indent logic lives here.
Generating only changes the ai.* tables and can take a while, so main.NOT_WRITES lets it run without holding up bills;
approving makes an indent and goes through the normal write path (saved to SQL Server in the same request).
"""
from datetime import date as date_type

from fastapi import APIRouter, Depends, HTTPException, Query
from pydantic import BaseModel, Field

from .. import db, store
from ..ai import enabled, service, suggestions
from ..schemas import Line, PurchaseIndentIn
from . import transactions
from .auth import require_module, require_right

MODULE = "Purchase Indent"


def _on() -> None:
    if not enabled():
        raise HTTPException(404, "AI forecasting is switched off (AI_FORECAST_ENABLED)")


router = APIRouter(prefix="/ai", tags=["AI forecast"], dependencies=[Depends(_on)])


class ApproveIn(BaseModel):
    supplier: str = Field("KSBCL", description="Supplier of the indent")
    date: date_type | None = Field(None, description="Indent date; today if omitted")
    cases: int | None = Field(None, ge=1, le=100_000, description="Change the suggested number of cases")
    remarks: str = Field("", max_length=300)


def _reconciled(rows: list[dict]) -> list[dict]:
    """An approved suggestion whose indent is not in the store (the save failed after it was marked) goes back to draft."""
    for r in rows:
        if r["status"] == "approved" and r["indent_doc_no"] not in store.DOCS["purchase_indent"]:
            suggestions.reopen(r["id"])
            r["status"], r["indent_doc_no"], r["decided_on"], r["decided_by"] = "draft", None, None, None
    return rows


@router.get("/recommendations", dependencies=[Depends(require_module(MODULE))])
def recommendations(include_monitor: bool = False, refresh: bool = False, limit: int | None = Query(None, ge=1, le=5000)):
    """Reorder recommendations for every active item, most urgent first. Items that only need monitoring are left out
    unless include_monitor is set. Cached for 10 minutes; refresh=true recalculates."""
    data = service.compute(refresh)
    items = data["items"] if include_monitor else [i for i in data["items"] if i["status"] != "Monitor"]
    return {**data, "items": items[:limit] if limit else items}


@router.get("/forecast/{item_code:path}", dependencies=[Depends(require_module(MODULE))])
def item_forecast(item_code: str):
    """The next 14 days of expected sales for one item, and its recommendation."""
    return service.forecast_for(item_code)


@router.post("/suggestions/generate", dependencies=[Depends(require_right("add", MODULE))])
def generate(user: dict = Depends(require_module(MODULE))):
    """Save a draft suggestion for every item that needs a reorder now or soon (replacing its earlier draft)."""
    data = service.compute(refresh=True)
    wanted = [i for i in data["items"] if i["suggested_cases"] > 0]
    ids = suggestions.create_drafts(wanted, user.get("user", ""))
    return {"created": len(ids), "ids": ids, "data_through": data["data_through"]}


@router.get("/suggestions", dependencies=[Depends(require_module(MODULE))])
def list_suggestions(status: str | None = Query(None, pattern="^(draft|approved|rejected)$")):
    return _reconciled(suggestions.list_(status))


@router.post("/suggestions/{suggestion_id}/approve")
def approve(suggestion_id: int, body: ApproveIn, user: dict = Depends(require_right("add", MODULE, "Purchase Bill"))):
    """Make a Purchase Indent from the draft with the existing indent code, then mark the draft approved."""
    s = suggestions.get(suggestion_id)
    if not s:
        raise HTTPException(404, "Suggestion not found")
    if s["status"] != "draft":
        raise HTTPException(409, f"This suggestion is already {s['status']}")
    item = store.find(store.ITEMS, s["item_code"])
    if not item:
        raise HTTPException(422, f"Item {s['item_code']} no longer exists")
    cases = body.cases or s["cases"]
    indent = PurchaseIndentIn(date=body.date or date_type.today(), supplier=body.supplier,
                              lines=[Line(item_code=item["code"], cases=cases)],
                              remarks=(body.remarks or f"AI suggestion #{suggestion_id}: {s['reason']}")[:500])
    doc = transactions.create_indent_doc(indent)
    try:
        marked = suggestions.mark_approved(suggestion_id, user.get("user", ""), doc["doc_no"])
        problem = None if marked else (409, "Not approved: someone else decided this suggestion at the same time")
    except Exception as exc:
        problem = (503, f"Not approved: the suggestion could not be updated ({db.safe_error(exc)[:200]})")
    if problem:   # the indent must not exist without its suggestion being marked: take it back out (the write middleware drops its audit entry)
        store.DOCS["purchase_indent"].pop(doc["doc_no"], None)
        raise HTTPException(*problem)
    return {"suggestion_id": suggestion_id, "status": "approved", "indent": doc}


@router.post("/suggestions/{suggestion_id}/reject", dependencies=[Depends(require_right("add", MODULE, "Purchase Bill"))])
def reject(suggestion_id: int, user: dict = Depends(require_module(MODULE))):
    s = suggestions.get(suggestion_id)
    if not s:
        raise HTTPException(404, "Suggestion not found")
    if not suggestions.mark_rejected(suggestion_id, user.get("user", "")):
        raise HTTPException(409, f"This suggestion is already {s['status']}")
    return {"suggestion_id": suggestion_id, "status": "rejected"}
