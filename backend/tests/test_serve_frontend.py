"""The shop backend serves the built frontend (frontend/dist) and answers the API under /api too, so a shop
PC needs no Node.js; nothing outside frontend/dist is ever served."""
import pytest

from app import main


@pytest.fixture
def dist(tmp_path, monkeypatch):
    (tmp_path / "assets").mkdir()
    (tmp_path / "index.html").write_text("<html>vLQMS</html>")
    (tmp_path / "assets" / "app.js").write_text("console.log(1)")
    (tmp_path.parent / "secret.txt").write_text("nope")
    monkeypatch.setattr(main, "FRONTEND_DIST", tmp_path)
    return tmp_path


async def test_api_answers_under_api_prefix(client, dist):
    direct = await client.get("/masters/uoms")
    prefixed = await client.get("/api/masters/uoms")
    assert prefixed.status_code == 200 and prefixed.json() == direct.json()
    assert (await client.get("/api/")).json()["status"] == "ok"


async def test_page_loads_get_the_app_and_files_are_served(anon_client, dist):
    page = await anon_client.get("/", headers={"Accept": "text/html"})
    assert page.status_code == 200 and "vLQMS" in page.text
    deep = await anon_client.get("/some/screen", headers={"Accept": "text/html"})
    assert "vLQMS" in deep.text
    js = await anon_client.get("/assets/app.js")
    assert js.status_code == 200 and "console" in js.text
    assert (await anon_client.get("/")).json()["status"] == "ok"   # the API's health check without text/html


async def test_nothing_outside_dist_is_served(anon_client, dist):
    for path in ("/../secret.txt", "/assets/../../secret.txt", "/%2e%2e/secret.txt"):
        res = await anon_client.get(path)
        assert "nope" not in res.text, path


async def test_serving_can_be_turned_off(anon_client, dist, monkeypatch):
    monkeypatch.setenv("SERVE_FRONTEND", "0")
    res = await anon_client.get("/", headers={"Accept": "text/html"})
    assert res.json()["status"] == "ok"


async def test_pages_get_the_security_headers_and_docs_stay_the_api(anon_client, dist):
    page = await anon_client.get("/", headers={"Accept": "text/html"})
    assert page.headers["x-frame-options"] == "DENY" and page.headers["x-content-type-options"] == "nosniff"
    docs = await anon_client.get("/docs", headers={"Accept": "text/html"})
    assert "swagger" in docs.text.lower()
    assert (await anon_client.get("/a%00b", headers={"Accept": "text/html"})).status_code < 500
