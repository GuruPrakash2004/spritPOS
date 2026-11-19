"""A dropped SQL Server connection is retried by the watchdog instead of staying down until the backend restarts."""
import pytest

from app import db


@pytest.fixture
def down(monkeypatch):
    monkeypatch.setattr(db, "configured", lambda: True)
    monkeypatch.setattr(db, "_db_available", False)
    monkeypatch.setattr(db, "MANUAL_OFFLINE", False)
    monkeypatch.setattr(db, "_password_unreadable", False)
    monkeypatch.setattr(db, "ON_RECONNECT", None)


def test_it_reconnects_when_the_database_is_down(down, monkeypatch):
    tries = []
    monkeypatch.setattr(db, "init_db", lambda: tries.append(1) or True)
    called = []
    monkeypatch.setattr(db, "ON_RECONNECT", lambda: called.append(1))

    assert db.watch_once() is True
    assert tries == [1] and called == [1]


def test_a_failed_try_is_just_tried_again_next_time(down, monkeypatch):
    results = iter([False, True])
    monkeypatch.setattr(db, "init_db", lambda: next(results))

    assert db.watch_once() is False
    assert db.watch_once() is True


def test_it_leaves_alone_a_database_that_is_up_not_set_up_or_disconnected_on_purpose(down, monkeypatch):
    monkeypatch.setattr(db, "init_db", lambda: pytest.fail("must not connect"))

    monkeypatch.setattr(db, "_db_available", True)
    assert db.watch_once() is False
    monkeypatch.setattr(db, "_db_available", False)
    monkeypatch.setattr(db, "configured", lambda: False)
    assert db.watch_once() is False
    monkeypatch.setattr(db, "configured", lambda: True)
    monkeypatch.setattr(db, "MANUAL_OFFLINE", True)
    assert db.watch_once() is False
    monkeypatch.setattr(db, "MANUAL_OFFLINE", False)
    monkeypatch.setattr(db, "_password_unreadable", True)
    assert db.watch_once() is False


def test_an_error_while_loading_after_the_reconnect_does_not_undo_the_reconnect(down, monkeypatch):
    monkeypatch.setattr(db, "init_db", lambda: True)

    def boom():
        raise RuntimeError("load failed")

    monkeypatch.setattr(db, "ON_RECONNECT", boom)

    assert db.watch_once() is True


def test_print_formats_are_read_late_when_start_up_missed_them(monkeypatch):
    """The database was down at start-up, so the designer's rows were never read: the first print / Print Settings read loads them."""
    from app import designer, store
    saved = (list(store.UTL_OPTIONS), dict(store.UTL_DOCS))
    store.UTL_OPTIONS.clear()
    store.UTL_DOCS.clear()
    calls = []
    monkeypatch.setattr(designer.db, "is_db_available", lambda: True)
    monkeypatch.setattr(designer, "load", lambda: calls.append(1))
    monkeypatch.setattr(designer, "_last_try", 0.0)
    try:
        designer.ensure_loaded()
        designer.ensure_loaded()   # a second call within 10 s does not read again
        assert calls == [1]
    finally:
        store.UTL_OPTIONS[:] = saved[0]
        store.UTL_DOCS.update(saved[1])
