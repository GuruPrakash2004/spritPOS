"""A master's code may hold any character: no rule on / ? # % \ or length, and codes travel in URLs escaped (app/codeseg.py)."""
import re
from urllib.parse import quote

import pytest

from app import codeseg, store


def seg(code: str) -> str:
    """What frontend/src/lib/api.js `seg` writes."""
    return quote(code.replace("~", "~7E").replace("/", "~2F"), safe="")


@pytest.mark.parametrize("code", ["A~B", "~2F", "~7E", "a/b", "plain", "ACCESSORIES 18% GST", "x?y#z\w", "50% off/1/2~"])
def test_escape_round_trips(code):
    assert codeseg.dec(code.replace("~", "~7E").replace("/", "~2F")) == code


def test_dec_leaves_ordinary_codes_alone():
    assert codeseg.dec("ACCESSORIES 18% GST") == "ACCESSORIES 18% GST"
    assert codeseg.dec("A~B") == "A~B"        # a "~" that isn't ~2F / ~7E stays


async def test_a_group_whose_code_holds_percent_slash_and_hash_saves_and_deletes(client):
    code = "ACC 18% GST/#1?x\y"
    body = {"code": code, "name": code, "liquor": False, "consider_for_loyalty": False, "active": True}
    res = await client.post("/masters/item-groups", json=body)
    assert res.status_code == 201, res.text
    assert code in store.ITEM_GROUPS
    res = await client.put(f"/masters/item-groups/{seg(code)}", json={**body, "name": "Renamed"})
    assert res.status_code == 200, res.text
    assert store.ITEM_GROUPS[code]["name"] == "Renamed"
    res = await client.get(f"/masters/item-groups/{seg(code)}")
    assert res.status_code == 200 and res.json()["code"] == code
    assert (await client.delete(f"/masters/item-groups/{seg(code)}")).status_code == 204
    assert code not in store.ITEM_GROUPS


async def test_a_long_code_is_not_refused_by_the_web(client):
    code = "G" * 80
    res = await client.post("/masters/item-groups", json={"code": code, "name": "n", "liquor": False, "consider_for_loyalty": False, "active": True})
    assert res.status_code == 201, res.text
