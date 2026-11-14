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


async def test_document_needs_a_group_and_a_new_file_name(client):
    doc = {"kind": "document", "file_name": "licence.pdf", "data": PNG}
    assert (await client.post("/company/files", json=doc)).status_code == 422           # Group name cannot be empty!
    assert (await client.post("/company/files", json={**doc, "group": "Excise"})).status_code == 201
    again = await client.post("/company/files", json={**doc, "group": "Bank"})
    assert again.status_code == 409 and "Document already exists!" in again.text


async def test_company_save_needs_only_a_name_and_makes_back_quotes(client):
    current = (await client.get("/masters/company")).json()
    body = {**current, "name": "D'Souza Wines", "email": "", "address": "", "address1": "", "address2": "", "address3": ""}
    res = await client.put("/masters/company", json=body)
    assert res.status_code == 200
    assert res.json()["name"] == "D`Souza Wines" and res.json()["email"] == ""
    assert (await client.put("/masters/company", json={**body, "email": "not-an-email"})).status_code == 422
    assert (await client.put("/masters/company", json={**body, "name": "x" * 251})).status_code == 422
    await client.put("/masters/company", json=current)
