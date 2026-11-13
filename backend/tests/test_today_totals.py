"""The footer's today-totals: reused for a few seconds, and a slow database read never makes the other polls queue behind it."""
import threading

from app import db
from app.routers import transactions


def _fake_db(monkeypatch, calls, gate=None):
    monkeypatch.setattr(db, "is_db_available", lambda: True)
    monkeypatch.setattr(transactions, "_totals_cache", {})

    def query(day):
        calls.append(day)
        if gate:
            gate.wait(5)
        return {"date": day.isoformat(), "total_bills": 3, "sales": 1500.0, "source": "db"}
    monkeypatch.setattr(transactions, "_db_today_totals", query)


async def test_an_answer_is_reused_within_its_time(client, monkeypatch):
    calls = []
    _fake_db(monkeypatch, calls)
    first = (await client.get("/transactions/today-totals")).json()
    second = (await client.get("/transactions/today-totals")).json()
    assert first == second and first["source"] == "db" and len(calls) == 1


async def test_a_failed_read_gives_the_last_answer(client, monkeypatch):
    calls = []
    _fake_db(monkeypatch, calls)
    await client.get("/transactions/today-totals")
    monkeypatch.setattr(transactions, "TOTALS_TTL", 0)             # expired: it asks again ...
    monkeypatch.setattr(transactions, "_db_today_totals", lambda day: (_ for _ in ()).throw(RuntimeError("timeout")))
    res = (await client.get("/transactions/today-totals")).json()   # ... the read fails: the last answer stays
    assert res["total_bills"] == 3 and res["source"] == "db"


def test_a_poll_during_a_slow_read_is_answered_with_the_last_answer(monkeypatch):
    calls, gate = [], threading.Event()
    _fake_db(monkeypatch, calls, gate)
    gate.set()
    first = transactions.today_totals(None)                         # one answer is known
    monkeypatch.setattr(transactions, "TOTALS_TTL", 0)
    gate.clear()
    slow = threading.Thread(target=transactions.today_totals, args=(None,))
    slow.start()                                                    # a refresh is now stuck in the database
    while len(calls) < 2:
        threading.Event().wait(0.01)
    assert transactions.today_totals(None) == first                 # this poll does not wait for it
    gate.set(); slow.join(5)
