"""Delete Non-Liquor Items → Carry Over: the guards and the preview. The SQL itself needs the shop's database, so it is not run here."""
import pytest
from fastapi import HTTPException

from app import delete_nonliquor, sqlstore


def test_carry_over_needs_the_typed_confirmation(monkeypatch):
    with pytest.raises(HTTPException) as e:
        delete_nonliquor.carry_over(["BEER"], "yes", {"user": "ADMIN"})
    assert e.value.status_code == 422 and "CARRY OVER" in e.value.detail


def test_no_groups_selected(monkeypatch):
    monkeypatch.setattr(sqlstore, "enabled", lambda: True)
    with pytest.raises(HTTPException) as e:
        delete_nonliquor.carry_preview([])
    assert e.value.detail == "No groups selected!"


def test_offline_database_refuses(monkeypatch):
    monkeypatch.setattr(sqlstore, "enabled", lambda: False)
    with pytest.raises(HTTPException) as e:
        delete_nonliquor.carry_preview(["BEER"])
    assert e.value.status_code == 503


def test_preview_counts_and_blocks_a_group_without_items(monkeypatch):
    monkeypatch.setattr(sqlstore, "enabled", lambda: True)
    monkeypatch.setattr(delete_nonliquor, "_group_ids", lambda codes: [1])
    monkeypatch.setattr(delete_nonliquor, "_item_ids_of_groups", lambda ids: [])
    monkeypatch.setattr(delete_nonliquor, "_counts", lambda ids: {"sales_bills": 0, "sales_lines": 0, "purchase_bills": 0, "purchase_lines": 0, "stock_rows": 0, "first_sale": None})
    monkeypatch.setattr(delete_nonliquor, "_closing_balances", lambda ids: [])
    info = delete_nonliquor.carry_preview(["BEER", "beer "])
    assert info["groups"] == ["beer"] and info["can_carry"] is False and info["blockers"] == ["The groups have no items"]


def test_preview_lists_what_is_carried(monkeypatch):
    monkeypatch.setattr(sqlstore, "enabled", lambda: True)
    monkeypatch.setattr(delete_nonliquor, "_group_ids", lambda codes: [1])
    monkeypatch.setattr(delete_nonliquor, "_item_ids_of_groups", lambda ids: [10, 11, 12])
    monkeypatch.setattr(delete_nonliquor, "_counts", lambda ids: {"sales_bills": 4, "sales_lines": 9, "purchase_bills": 2, "purchase_lines": 3, "stock_rows": 14, "first_sale": "2026-01-02"})
    monkeypatch.setattr(delete_nonliquor, "_closing_balances", lambda ids: [(10, 5.0), (12, 7.5)])
    info = delete_nonliquor.carry_preview(["BEER"])
    assert info["can_carry"] and info["carried_items"] == 2 and info["carried_qty"] == 12.5 and info["items"] == 3
