"""Utilities -> Key Generation (the vendor's tool, frmKeyReg): Administrator only, and only where the vendor's key file exists."""
import json

import pytest

from app import licence_keys


@pytest.fixture
def keys(tmp_path, monkeypatch):
    f = tmp_path / "licence_keys.json"
    f.write_text(json.dumps({"59183": "CWZMTD", "67225": "E66L31"}))
    monkeypatch.setattr(licence_keys, "PATH", f)
    monkeypatch.setattr(licence_keys, "_cache", None)
    return f


async def test_gives_the_key_for_a_reg_no(client, keys):
    res = await client.get("/utilities/key-generator", params={"reg_no": "59183"})

    assert res.status_code == 200
    assert res.json() == {"reg_no": "59183", "key": "CWZMTD"}


async def test_unknown_reg_no_gives_a_dash_like_the_exe(client, keys):
    assert (await client.get("/utilities/key-generator", params={"reg_no": "11111"})).json()["key"] == "-"


async def test_numbers_only(client, keys):
    assert (await client.get("/utilities/key-generator", params={"reg_no": "59A83"})).status_code == 422


async def test_switched_off_without_the_key_file(client, tmp_path, monkeypatch):
    monkeypatch.setattr(licence_keys, "PATH", tmp_path / "none.json")
    monkeypatch.setattr(licence_keys, "_cache", None)

    assert (await client.get("/utilities/key-generator/available")).json() == {"available": False}
    assert (await client.get("/utilities/key-generator", params={"reg_no": "59183"})).status_code == 404


async def test_a_manager_cannot_use_it(manager_client, keys):
    assert (await manager_client.get("/utilities/key-generator", params={"reg_no": "59183"})).status_code == 403
    assert (await manager_client.get("/utilities/key-generator/available")).status_code == 403


async def test_a_cashier_cannot_use_it(cashier_client, keys):
    assert (await cashier_client.get("/utilities/key-generator", params={"reg_no": "59183"})).status_code == 403


async def test_needs_sign_in(anon_client, keys):
    assert (await anon_client.get("/utilities/key-generator", params={"reg_no": "59183"})).status_code == 401
