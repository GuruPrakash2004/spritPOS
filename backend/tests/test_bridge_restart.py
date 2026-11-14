"""The Crystal bridge restarts itself when Crystal runs out of memory, and is recycled after many reports."""
import httpx
import pytest
from fastapi import HTTPException

from app import rpt_bridge


@pytest.fixture(autouse=True)
def real_bridge_path(monkeypatch):
    monkeypatch.setattr(rpt_bridge, "using_mock", lambda: False)
    monkeypatch.setattr(rpt_bridge, "url", lambda: "http://127.0.0.1:8765")
    monkeypatch.setattr(rpt_bridge, "_reports_since_start", 0)


def test_memory_error_restarts_the_bridge_and_retries_once(monkeypatch):
    calls, restarts = [], []

    def once(path, job, method="POST"):
        calls.append(path)
        if len(calls) == 1:
            raise HTTPException(502, "Crystal Reports: Memory full. Failed to export the report. Not enough memory for operation.")
        return httpx.Response(200, content=b"%PDF")

    monkeypatch.setattr(rpt_bridge, "_call_once", once)
    monkeypatch.setattr(rpt_bridge, "_restart_bridge", lambda why: restarts.append(why) or True)

    assert rpt_bridge._call("/render", {"rpt": "fmtPB.rpt"}).content == b"%PDF"
    assert calls == ["/render", "/render"] and restarts == ["Crystal ran out of memory"]


def test_other_errors_and_a_bridge_that_is_not_ours_are_not_retried(monkeypatch):
    def once(path, job, method="POST"):
        raise HTTPException(502, "Crystal Reports: Memory full")

    monkeypatch.setattr(rpt_bridge, "_call_once", once)
    monkeypatch.setattr(rpt_bridge, "_restart_bridge", lambda why: False)   # started by hand / a service: not restarted here
    with pytest.raises(HTTPException) as err:
        rpt_bridge._call("/render", {"rpt": "x.rpt"})
    assert err.value.status_code == 502

    def other(path, job, method="POST"):
        raise HTTPException(502, "Crystal Reports: The printer is offline")

    monkeypatch.setattr(rpt_bridge, "_call_once", other)
    monkeypatch.setattr(rpt_bridge, "_restart_bridge", lambda why: pytest.fail("must not restart for a printer error"))
    with pytest.raises(HTTPException):
        rpt_bridge._call("/render", {"rpt": "x.rpt"})


def test_the_bridge_is_recycled_after_many_reports(monkeypatch):
    restarts = []
    monkeypatch.setattr(rpt_bridge, "_call_once", lambda path, job, method="POST": httpx.Response(200, content=b"x"))
    monkeypatch.setattr(rpt_bridge, "_restart_bridge", lambda why: restarts.append(why) or True)
    monkeypatch.setattr(rpt_bridge, "_reports_since_start", rpt_bridge.RECYCLE_AFTER)
    rpt_bridge._call("/print", {"rpt": "x.rpt"})
    assert len(restarts) == 1 and "recycled" in restarts[0]
    rpt_bridge._call("/health", None)   # a health check never counts
    assert len(restarts) == 1
