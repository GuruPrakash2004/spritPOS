"""/ai endpoints. The database layer (app/ai/suggestions.py) and the forecast (app/ai/service.py) are faked; the
indent itself is made by the real existing indent code, in memory."""
import pytest

from app import store
from app.ai import service, suggestions

REC = {"item_code": "RC-750", "item_name": "Royal Challenge 750", "group": "", "bottles_per_case": 12, "qoh": 10.0,
       "status": "Reorder now", "days_remaining": 1.0, "reorder_point": 50.0, "target_stock": 190.0,
       "suggested_cases": 3, "suggested_bottles": 36, "reason": "Stock 10 lasts 1.0 days", "item_class": "fast", "method": "chronos-bolt-tiny"}
MON = {**REC, "item_code": "VIN-750", "status": "Monitor", "suggested_cases": 0, "suggested_bottles": 0}


class FakeSuggestions:
    def __init__(self):
        self.rows: dict[int, dict] = {}

    def create_drafts(self, recs, user):
        ids = []
        for r in recs:
            i = len(self.rows) + 1
            self.rows[i] = {"id": i, "item_code": r["item_code"], "cases": r["suggested_cases"], "reason": r["reason"],
                            "status": "draft", "indent_doc_no": None, "decided_on": None, "decided_by": None}
            ids.append(i)
        return ids

    def list_(self, status=None):
        return [dict(r) for r in self.rows.values() if not status or r["status"] == status]

    def get(self, i):
        return dict(self.rows[i]) if i in self.rows else None

    def mark_approved(self, i, user, doc_no):
        if self.rows[i]["status"] != "draft":
            return False
        self.rows[i].update(status="approved", indent_doc_no=doc_no)
        return True

    def mark_rejected(self, i, user):
        if self.rows[i]["status"] != "draft":
            return False
        self.rows[i]["status"] = "rejected"
        return True

    def reopen(self, i):
        self.rows[i].update(status="draft", indent_doc_no=None)


@pytest.fixture
def ai(monkeypatch):
    monkeypatch.setenv("AI_FORECAST_ENABLED", "1")
    fake = FakeSuggestions()
    for name in ("create_drafts", "list_", "get", "mark_approved", "mark_rejected", "reopen"):
        monkeypatch.setattr(suggestions, name, getattr(fake, name))
    monkeypatch.setattr(service, "compute", lambda refresh=False: {"as_of": "2026-10-10", "data_through": "2026-10-09",
                                                                   "history_days": 120, "items": [REC, MON]})
    monkeypatch.setattr(service, "forecast_for", lambda code: {"item_code": code, "forecast": [{"date": "2026-10-10", "qty": 2.5}]})
    return fake


@pytest.mark.parametrize("method,path", [("get", "/ai/recommendations"), ("get", "/ai/suggestions"), ("get", "/ai/forecast/RC-750"),
                                          ("post", "/ai/suggestions/generate"), ("post", "/ai/suggestions/1/approve"),
                                          ("post", "/ai/suggestions/1/reject")])
async def test_everything_is_off_while_the_flag_is_off(client, monkeypatch, method, path):
    monkeypatch.delenv("AI_FORECAST_ENABLED", raising=False)
    res = await getattr(client, method)(path, **({"json": {}} if method == "post" else {}))
    assert res.status_code == 404 and "switched off" in res.json()["detail"]


async def test_login_is_still_needed(anon_client, ai):
    assert (await anon_client.get("/ai/recommendations")).status_code == 401


async def test_recommendations_hide_monitor_items_unless_asked(client, ai):
    res = await client.get("/ai/recommendations")
    assert [i["item_code"] for i in res.json()["items"]] == ["RC-750"]
    res = await client.get("/ai/recommendations", params={"include_monitor": True})
    assert len(res.json()["items"]) == 2


async def test_forecast_of_one_item(client, ai):
    res = await client.get("/ai/forecast/RC-750")
    assert res.status_code == 200 and res.json()["forecast"][0]["qty"] == 2.5


async def test_generate_drafts_only_items_with_a_quantity(client, ai):
    res = await client.post("/ai/suggestions/generate")
    assert res.status_code == 200 and res.json()["created"] == 1
    drafts = (await client.get("/ai/suggestions", params={"status": "draft"})).json()
    assert [d["item_code"] for d in drafts] == ["RC-750"]


async def test_approving_makes_a_purchase_indent_with_the_existing_code(client, ai):
    await client.post("/ai/suggestions/generate")
    before = set(store.DOCS["purchase_indent"])
    res = await client.post("/ai/suggestions/1/approve", json={"supplier": "USL"})
    assert res.status_code == 200, res.text
    new = set(store.DOCS["purchase_indent"]) - before
    assert len(new) == 1
    doc = store.DOCS["purchase_indent"][new.pop()]
    assert res.json()["indent"]["doc_no"] == doc["doc_no"]
    line = doc["lines"][0]
    assert (line["item_code"], line["cases"], line["qty"]) == ("RC-750", 3, 3 * store.ITEMS["RC-750"]["bottles_per_case"])
    assert ai.rows[1]["status"] == "approved" and ai.rows[1]["indent_doc_no"] == doc["doc_no"]


async def test_the_cases_can_be_changed_on_approval(client, ai):
    await client.post("/ai/suggestions/generate")
    res = await client.post("/ai/suggestions/1/approve", json={"supplier": "USL", "cases": 5})
    assert res.json()["indent"]["lines"][0]["cases"] == 5


async def test_a_suggestion_is_approved_only_once(client, ai):
    await client.post("/ai/suggestions/generate")
    assert (await client.post("/ai/suggestions/1/approve", json={"supplier": "USL"})).status_code == 200
    count = len(store.DOCS["purchase_indent"])
    again = await client.post("/ai/suggestions/1/approve", json={"supplier": "USL"})
    assert again.status_code == 409 and len(store.DOCS["purchase_indent"]) == count


async def test_a_bad_supplier_leaves_the_suggestion_a_draft(client, ai):
    await client.post("/ai/suggestions/generate")
    res = await client.post("/ai/suggestions/1/approve", json={"supplier": "NOBODY"})
    assert res.status_code == 422 and ai.rows[1]["status"] == "draft"


async def test_if_marking_fails_the_indent_is_taken_back_out(client, ai, monkeypatch):
    await client.post("/ai/suggestions/generate")
    before = set(store.DOCS["purchase_indent"])

    def boom(*a):
        raise RuntimeError("connection lost")
    monkeypatch.setattr(suggestions, "mark_approved", boom)
    res = await client.post("/ai/suggestions/1/approve", json={"supplier": "USL"})
    assert res.status_code == 503 and set(store.DOCS["purchase_indent"]) == before


async def test_rejecting(client, ai):
    await client.post("/ai/suggestions/generate")
    assert (await client.post("/ai/suggestions/1/reject")).json()["status"] == "rejected"
    assert (await client.post("/ai/suggestions/1/approve", json={"supplier": "USL"})).status_code == 409
    assert (await client.post("/ai/suggestions/99/reject")).status_code == 404


async def test_an_approved_suggestion_without_its_indent_goes_back_to_draft(client, ai):
    await client.post("/ai/suggestions/generate")
    ai.rows[1].update(status="approved", indent_doc_no="26/PI-0009999")      # the save failed after marking
    rows = (await client.get("/ai/suggestions")).json()
    assert rows[0]["status"] == "draft" and ai.rows[1]["status"] == "draft"
