"""File Attachment on masters: kept on disk by the web app, per master and code."""
import base64
from pathlib import Path

import pytest

from app.routers import attachments


@pytest.fixture(autouse=True)
def _tmp_store(tmp_path, monkeypatch):
    monkeypatch.setattr(attachments, "STORE", tmp_path)


def body(name="licence.pdf", data=b"%PDF-1.4 hi"):
    return {"file_name": name, "data": base64.b64encode(data).decode(), "remarks": "scan"}


async def test_attach_list_download_and_delete(client):
    res = await client.post("/attachments/items", params={"code": "VIN-750"}, json=body())
    assert res.status_code == 201
    row = res.json()
    assert (await client.get("/attachments/items", params={"code": "VIN-750"})).json()[0]["id"] == row["id"]
    assert (await client.get("/attachments/items", params={"code": "OTHER"})).json() == []
    assert (await client.get("/attachments/items/counts")).json() == {"VIN-750": 1}
    got = await client.get(f"/attachments/items/{row['id']}/content")
    assert got.status_code == 200 and got.content.startswith(b"%PDF")
    assert (await client.delete(f"/attachments/items/{row['id']}")).status_code == 204
    assert (await client.get("/attachments/items", params={"code": "VIN-750"})).json() == []


async def test_bad_files_and_masters_are_refused(client, anon_client):
    assert (await client.post("/attachments/items", params={"code": "X"}, json=body("x.svg"))).status_code == 422
    assert (await client.post("/attachments/items", params={"code": "X"}, json={"file_name": "a.pdf", "data": "!!"})).status_code == 422
    assert (await client.get("/attachments/nothing", params={"code": "X"})).status_code == 404
    assert (await anon_client.get("/attachments/items", params={"code": "X"})).status_code == 401
