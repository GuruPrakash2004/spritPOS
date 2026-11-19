"""master_watch: a change made to a master table outside the app reloads the masters; the app's own saves do not."""
from app import db, master_watch, setup_db, sqlstore


def _setup(monkeypatch):
    state = {"print": {}, "reloads": 0}
    monkeypatch.setattr(db, "is_db_available", lambda: True)
    monkeypatch.setattr(setup_db, "is_ready", lambda: True)
    monkeypatch.setattr(master_watch, "_fingerprint", lambda: dict(state["print"]))
    monkeypatch.setattr(sqlstore, "reload", lambda: state.__setitem__("reloads", state["reloads"] + 1))
    master_watch.forget()
    master_watch._rebase = False
    return state


def test_an_outside_change_reloads_once_and_bumps_the_version(monkeypatch):
    state = _setup(monkeypatch)
    state["print"] = {"MstITEM": (10, 111)}
    before = master_watch.VERSION
    master_watch._run()                       # first check: only the baseline
    assert state["reloads"] == 0
    master_watch._run()                       # nothing changed
    assert state["reloads"] == 0
    state["print"] = {"MstITEM": (10, 222)}   # someone edited a row in SQL Server
    master_watch._run()
    assert state["reloads"] == 1 and master_watch.VERSION == before + 1
    master_watch._run()                       # the new state is the baseline now
    assert state["reloads"] == 1


def test_the_apps_own_master_save_is_not_an_outside_change(monkeypatch):
    state = _setup(monkeypatch)
    state["print"] = {"MstITEM": (10, 111)}
    master_watch._run()
    state["print"] = {"MstITEM": (11, 333)}   # the app saved a new item
    master_watch.own_write()
    master_watch._run()
    assert state["reloads"] == 0
    state["print"] = {"MstITEM": (11, 444)}   # then an outside edit
    master_watch._run()
    assert state["reloads"] == 1


def test_a_reload_waits_while_a_write_is_in_flight(monkeypatch):
    state = _setup(monkeypatch)
    monkeypatch.setattr(master_watch.time, "sleep", lambda s: None)
    ticks = iter(range(0, 1000))
    monkeypatch.setattr(master_watch.time, "monotonic", lambda: next(ticks) * 0.5)
    state["print"] = {"MstITEM": (1, 1)}
    master_watch._run()
    state["print"] = {"MstITEM": (1, 2)}
    master_watch.IN_FLIGHT = 1
    try:
        master_watch._run()
    finally:
        master_watch.IN_FLIGHT = 0
    assert state["reloads"] == 0 and master_watch.RELOADING is False
    master_watch._run()                       # the write finished: the change is picked up now
    assert state["reloads"] == 1
