"""Company Master → Images/Documents. Files go to a temp folder, never backend/uploads/."""
import base64

import pytest

from app.routers import company_files

PNG = base64.b64encode(b"\x89PNG\r\n\x1a\n fake image").decode()


@pytest.fixture(autouse=True)
def temp_upload_folder(tmp_path, monkeypatch):
    monkeypatch.setattr(company_files, "STORE", tmp_path)
    monkeypatch.setattr(company_files, "INDEX", tmp_path / "index.json")


async def upload(c, **overrides):
    return await c.post("/company/files", json={"kind": "image", "file_name": "shop.png", "data": PNG, **overrides})


async def test_admin_uploads_edits_and_deletes_a_file(client, anon_client):
    res = await upload(client)
    assert res.status_code == 201
    row = res.json()
    assert row["uploaded_by"] == "ADMIN"

    # Images are shown in <img> tags, which can't send a token, so the content is public.
    content = await anon_client.get(f"/company/files/{row['id']}/content")
    assert content.status_code == 200
    assert content.content == base64.b64decode(PNG)

    edited = await client.put(f"/company/files/{row['id']}", json={"title": "Front", "show_on_pos": False})
    assert edited.status_code == 200
    assert edited.json()["title"] == "Front"

    assert (await client.delete(f"/company/files/{row['id']}")).status_code == 204
    assert (await client.get("/company/files")).json() == []


async def test_cashier_cannot_upload(cashier_client):
    assert (await upload(cashier_client)).status_code == 403


async def test_script_files_are_refused(client):
    res = await upload(client, file_name="evil.svg")

    assert res.status_code == 422
