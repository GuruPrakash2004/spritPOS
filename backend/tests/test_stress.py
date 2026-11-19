"""Stress tests: many bills at once, stock never oversold or lost, reads stay fast and correct under load.

Runs in-process on the sample data (no SQL Server). Sizes can be raised with STRESS_N (default 300).
"""
import asyncio
import os
import time
from datetime import date

import pytest

from app import store

N = int(os.environ.get("STRESS_N", "300"))
TODAY = date.today().isoformat()


@pytest.fixture(autouse=True)
def fresh_write_lock():
    """main._write_lock is one asyncio.Lock per process; each pytest test has its own event loop, so give it a new lock."""
    from app import main
    main._write_lock = asyncio.Lock()


def bill(item, qty=1, **extra):
    return {"date": TODAY, "counter": "counter10", "salesperson": "GURU", "lines": [{"item_code": item, "bottles": qty}], **extra}


async def qoh(client, code):
    return (await client.get(f"/masters/items/{code}")).json()["qoh"]


async def test_concurrent_sales_never_oversell(client):
    """More buyers than stock: exactly `stock` bills succeed, the rest are refused, stock ends at 0."""
    stock = int(await qoh(client, "OM-750"))
    buyers = stock + 40

    res = await asyncio.gather(*[client.post("/transactions/sales-invoice", json=bill("OM-750")) for _ in range(buyers)])

    codes = [r.status_code for r in res]
    assert codes.count(201) == stock, {c: codes.count(c) for c in set(codes)}
    assert all(c in (201, 422, 409) for c in codes), set(codes)
    assert await qoh(client, "OM-750") == 0


async def test_concurrent_bills_get_unique_numbers_and_exact_stock(client):
    before = int(await qoh(client, "VIN-750"))
    n = min(N, before)

    res = await asyncio.gather(*[client.post("/transactions/sales-invoice", json=bill("VIN-750")) for _ in range(n)])

    assert all(r.status_code == 201 for r in res), [r.text for r in res if r.status_code != 201][:3]
    numbers = [r.json()["doc_no"] for r in res]
    assert len(set(numbers)) == n, "duplicate bill numbers under load"
    assert await qoh(client, "VIN-750") == before - n


async def test_same_idempotency_key_sent_many_times_saves_once(client):
    before = int(await qoh(client, "RC-750"))
    headers = {"Idempotency-Key": "stress-key-1"}

    res = await asyncio.gather(*[client.post("/transactions/sales-invoice", json=bill("RC-750"), headers=headers) for _ in range(25)])

    ok = [r for r in res if r.status_code == 201]
    assert ok, [r.text for r in res][:2]
    assert len({r.json()["doc_no"] for r in ok}) == 1
    assert await qoh(client, "RC-750") == before - 1


async def test_cancel_and_sell_race_keeps_stock_consistent(client):
    start = int(await qoh(client, "VIN-750"))
    made = [(await client.post("/transactions/sales-invoice", json=bill("VIN-750", 2))).json() for _ in range(10)]
    assert await qoh(client, "VIN-750") == start - 20

    jobs = [client.post(f"/transactions/sales-invoice/{b['doc_no']}/cancel", json={"remarks": "stress"}) for b in made]
    jobs += [client.post("/transactions/sales-invoice", json=bill("VIN-750", 1)) for _ in range(10)]
    res = await asyncio.gather(*jobs)

    cancelled = sum(r.status_code in (200, 201) for r in res[:10])
    sold = sum(r.status_code == 201 for r in res[10:])
    assert await qoh(client, "VIN-750") == start - 20 + cancelled * 2 - sold


async def test_double_cancel_returns_stock_once(client):
    start = int(await qoh(client, "RC-750"))
    inv = (await client.post("/transactions/sales-invoice", json=bill("RC-750", 3))).json()

    res = await asyncio.gather(*[client.post(f"/transactions/sales-invoice/{inv['doc_no']}/cancel", json={"remarks": "stress"}) for _ in range(15)])

    assert sum(r.status_code in (200, 201) for r in res) == 1
    assert await qoh(client, "RC-750") == start


async def test_stock_ledger_matches_item_qoh_after_a_burst(client):
    code = "RC-750"
    before = int(await qoh(client, code))
    n = min(60, before)
    await asyncio.gather(*[client.post("/transactions/sales-invoice", json=bill(code)) for _ in range(n)])

    sold = [m for m in store.MOVEMENTS if m["item"] == code and m["kind"] == "sale"]
    assert len(sold) >= n
    assert len({m["doc_no"] for m in sold}) == len(sold), "a bill number has two stock rows"
    assert await qoh(client, code) == before - n


@pytest.mark.parametrize("path", ["/masters/items", "/masters/customers", "/transactions/sales-invoice", "/reports/dsr?on=" + TODAY])
async def test_read_endpoints_hold_up_under_parallel_load(client, path):
    t = time.perf_counter()
    res = await asyncio.gather(*[client.get(path) for _ in range(N)])
    secs = time.perf_counter() - t

    assert all(r.status_code == 200 for r in res), {r.status_code for r in res}
    assert len({r.text for r in res}) == 1, "same read gave different answers"
    print(f"\n{path}: {N} parallel reads in {secs:.2f}s ({N / secs:.0f}/s)")
    assert secs < 60


async def test_mixed_traffic_for_a_busy_counter(client):
    """Bills, reads, master edits and logins all at once; nothing 5xx."""
    jobs = []
    for i in range(80):
        jobs.append(client.post("/transactions/sales-invoice", json=bill("KF-650")))
        jobs.append(client.get("/masters/items"))
        jobs.append(client.get("/reports/dsr", params={"on": TODAY}))
        jobs.append(client.post("/masters/brands", json={"code": f"STR{i}", "name": f"Stress brand {i}"}))
    t = time.perf_counter()

    res = await asyncio.gather(*jobs, return_exceptions=True)

    errors = [r for r in res if isinstance(r, Exception)]
    assert not errors, errors[:3]
    assert not [r for r in res if r.status_code >= 500], {r.status_code for r in res}
    assert sum(1 for i in range(80) if f"STR{i}" in store.BRANDS) == 80
    print(f"\nmixed: {len(jobs)} requests in {time.perf_counter() - t:.2f}s")


async def test_bad_input_flood_is_refused_not_crashed(client):
    junk = [
        {}, {"lines": None}, {"lines": [{}]}, {"lines": [{"item_code": "X" * 5000, "bottles": 1}]},
        {"date": "not-a-date", "lines": [{"item_code": "VIN-750", "bottles": 1}]},
        {"lines": [{"item_code": "VIN-750", "bottles": 10 ** 12}]}, {"lines": [{"item_code": "VIN-750", "bottles": "NaN"}]},
        {"lines": [{"item_code": "'; DROP TABLE MstItem;--", "bottles": 1}]},
    ]
    res = await asyncio.gather(*[client.post("/transactions/sales-invoice", json=j) for j in junk * 20])

    assert all(r.status_code in (400, 404, 409, 422) for r in res), {r.status_code for r in res}
    assert await qoh(client, "VIN-750") == store.ITEMS["VIN-750"]["qoh"]


async def test_login_flood_locks_out_and_does_not_hang(client):
    from conftest import enc
    res = await asyncio.gather(*[client.post("/auth/login", json={"user": "RAVI", "password": enc("wrong-pass-1")}) for _ in range(40)])

    assert set(r.status_code for r in res) <= {401, 429}
    assert 429 in {r.status_code for r in res}
