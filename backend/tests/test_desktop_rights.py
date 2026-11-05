"""Rights from the desktop's own tables (MstUserRights / MstRoleRights flags per menu screen)."""
from app import desktop_rights, security, store

MENU = {1: "Sales Invoice", 2: "Item Price Maintenance Master", 3: "Purchase Bill", 4: "Customised Reports..."}


def test_a_screen_opens_only_with_a_row_that_is_not_all_zeros():
    got = desktop_rights.compute(MENU, {1: "111110000000", 2: "000000000000", 3: "110010000000"}, ["Sales Invoice"])
    assert "Sales Invoice" in got["modules"] and "Purchase Bill" in got["modules"]
    assert "Price List Maintenance" not in got["modules"]      # an all-zero row: greyed out
    assert "Reports Dashboard" not in got["modules"]           # no row at all: greyed out
    assert got["rights"]["Sales Invoice"] == {"add": True, "edit": True, "delete": True, "print": True}
    assert got["rights"]["Purchase Bill"] == {"add": True, "edit": True, "delete": False, "print": False}


def test_caption_names_map_to_web_modules_and_web_only_screens_keep_the_role():
    got = desktop_rights.compute(MENU, {2: "100010000000", 4: "000010000000"}, ["Cash Denomination", "Sales Invoice"])
    assert "Price List Maintenance" in got["modules"] and "Reports Dashboard" in got["modules"]
    assert "Cash Denomination" in got["modules"]               # not in the menu: the web role's choice stands
    assert "Sales Invoice" not in got["modules"]               # in the menu with no row: the desktop's choice stands
    assert {"Dashboard", "About"} <= set(got["modules"])


def test_security_uses_the_desktop_rights_unless_the_role_was_set_in_the_web():
    user = {"role": "Cashier", "desktop": {"modules": ["Dashboard", "Sales Invoice"], "rights": {"Sales Invoice": {"add": True, "edit": False, "delete": False, "print": True}}}}
    assert security.modules_for(user) == ["Dashboard", "Sales Invoice"]
    assert security.rights_for_user(user)["Sales Invoice"]["edit"] is False
    assert security.modules_for({**user, "role_set_in_web": True}) == store.ROLES["Cashier"]
    assert security.modules_for({"role": store.ADMIN_ROLE, "desktop": user["desktop"]}) == ["*"]


def test_the_print_right_is_checked_for_purchase_documents_not_sales_bills():
    from fastapi import HTTPException

    from app.routers.printing import _check_print_right
    user = {"role": "Cashier", "desktop": {"modules": ["Purchase Bill", "Sales Invoice"], "rights": {
        "Purchase Bill": {"add": True, "edit": True, "delete": False, "print": False},
        "Sales Invoice": {"add": True, "edit": True, "delete": False, "print": False}}}}
    _check_print_right(user, "sales_invoice")   # the exe's sales screens do not check it
    try:
        _check_print_right(user, "purchase_bill")
        raise AssertionError("expected a refusal")
    except HTTPException as e:
        assert e.status_code == 403
    user["desktop"]["rights"]["Purchase Bill"]["print"] = True
    _check_print_right(user, "purchase_bill")


async def test_desktop_rights_admin_needs_an_administrator_and_the_database(client, cashier_client):
    assert (await cashier_client.get("/admin/desktop-rights")).status_code == 403
    assert (await client.get("/admin/desktop-rights")).status_code == 503      # the tests have no database
    bad = await client.put("/admin/desktop-rights/2", json={"rows": [{"menu_id": 1, "flags": "12"}]})
    assert bad.status_code == 422                                              # flags are twelve 0/1 characters
