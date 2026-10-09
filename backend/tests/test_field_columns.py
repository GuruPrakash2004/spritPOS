from app import field_columns


def test_web_key_for_known_column_any_case():
    assert field_columns.web_key_for("items", "MRP") == "mrp"
    assert field_columns.web_key_for("items", "hsncode") == "hsn_code"


def test_simple_masters_come_from_their_spec():
    assert field_columns.web_key_for("banks", "IFSCCOD") == "ifsc"
    assert field_columns.web_key_for("states", "STATENAME") == "name"


def test_unknown_column_or_master_has_no_key():
    assert field_columns.web_key_for("items", "Colour") == ""
    assert field_columns.web_key_for("nothing", "MRP") == ""
