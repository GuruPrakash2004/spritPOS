"""Screen definitions built from the designer's rows and the table's real columns (app/schema_meta.py)."""
from app import schema_meta as sm


def col(name, type_="varchar", length=50, nullable=True, identity=False, scale=0):
    return {"name": name, "type": type_, "max_length": length, "precision": 0, "scale": scale, "is_nullable": nullable,
            "is_identity": identity, "is_computed": False, "column_id": 1}


DOC = {"DocID": 1114, "DocName": "Item", "DocType": "Master", "DocShForm": "ITEM", "HdrTblName": "MstITEM", "DtlTblName": ""}
HDR = [col("ID", "int", 4, False, True), col("ITEMNAME", "varchar", 60, False), col("ROL", "decimal", 9, True, scale=2),
       col("AddedBy", "varchar", 20), col("NEWCOL", "varchar", 30)]


def row(code, order, **kw):
    return {"FldCode": code, "FldCaption": code.title(), "FldSection": "Header", "FldOrderBy": order, "FldDataType": "Text",
            "FldDataWidth": 20, **kw}


def test_designer_fields_follow_order_and_flags():
    rows = [row("ROL", 20, FldDataType="Numeric", FldNumDecimals=3, FldBlnMandatory="Y"), row("ITEMNAME", 10, FLdBlnHide="Y")]
    s = sm.build_screen(DOC, rows, HDR, [])
    designer = [f for f in s["fields"] if not f["standard"]]
    assert [f["code"] for f in designer] == ["ITEMNAME", "ROL"]
    assert designer[0]["hidden"] and designer[1]["mandatory"]
    assert designer[1]["type"] == "number" and designer[1]["decimals"] == 3


def test_column_added_to_the_table_appears_and_audit_ones_are_hidden():
    s = sm.build_screen(DOC, [row("ROL", 20)], HDR, [])
    by = {f["code"]: f for f in s["fields"]}
    assert by["NEWCOL"]["standard"] and by["NEWCOL"]["width"] == 30 and not by["NEWCOL"]["hidden"]
    assert by["AddedBy"]["hidden"] and by["AddedBy"]["readonly"]
    assert by["ID"]["readonly"]


def test_column_dropped_from_the_table_disappears():
    s = sm.build_screen(DOC, [row("GONE", 5), row("ROL", 20)], HDR, [])
    assert "GONE" not in {f["code"] for f in s["fields"]}


def test_caption_override_and_conditional_rule():
    rows = [row("ROL", 20, FldHideCompareVal="#ITEMNAME#", FldHideOperator="=", FldHideWithVal="X")]
    s = sm.build_screen(DOC, rows, HDR, [], {"rol": "Re-order level"})
    f = next(f for f in s["fields"] if f["code"] == "ROL")
    assert f["caption"] == "Re-order level"
    assert f["hide_when"] == {"field": "ITEMNAME", "op": "=", "value": "X"}


def test_table_name_is_never_unsafe():
    assert sm._safe("MstITEM; DROP TABLE x") == ""
    assert sm._safe("MstITEM") == "MstITEM"


def test_clean_extras_converts_drops_and_keeps_defaults(monkeypatch):
    cols = [col("ID", "int", 4, False, True), col("QTY", "int", 4, True), col("NOTE", "varchar", 5, True),
            col("WHEN", "datetime", 8, True), col("DUEDATE", "date", 3, False), col("TS", "timestamp", 8, False),
            col("QOH2", "int", 4, True)]
    monkeypatch.setattr(sm, "columns", lambda t: cols)
    out = sm.clean_extras("MstX", {"QTY": "7", "NOTE": "abc", "WHEN": "2026-10-09T10:30:00", "DUEDATE": "",
                                   "TS": "x", "QOH2": "5", "ID": "9", "NOPE": 1})
    assert out == {"QTY": 7, "NOTE": "abc", "WHEN": "2026-10-09 10:30:00"}   # blank NOT NULL date left to the default
    import pytest
    with pytest.raises(ValueError):
        sm.clean_extras("MstX", {"NOTE": "toolong"})
    with pytest.raises(ValueError):
        sm.clean_extras("MstX", {"QTY": "abc"})


def test_review_flags_what_an_administrator_should_look_at():
    rows = [row("ITEMNAME", 10, FldBlnMandatory="Y", FLdBlnHide="Y"), row("ITEMNAME", 11), row("GONE", 12),
            row("ID", 13, FldBlnMandatory="Y"), row("NEWCOL", 14, FldDataWidth=99),
            row("ROL", 15, FldDataType="Numeric", FldNumDecimals=2, FldFormula="#A#*2")]
    hdr = [c if c["name"] != "ROL" else col("ROL", "int", 4) for c in HDR]
    msgs = sm.review(DOC, rows, hdr, [], "")
    text = " | ".join(f"{m['level']}:{m['field']}:{m['text']}" for m in msgs)
    assert "defined twice" in text
    assert "not in the table any more" in text            # GONE
    assert "never written" in text and "error:ID" in text  # mandatory key column
    assert "also hidden or read-only" in text
    assert "holds 30" in text                              # width 99 vs varchar(30)
    assert "whole-number" in text and "formula" in text
    assert "no screen for this master" in text


def test_review_of_a_clean_screen_has_no_warnings():
    msgs = sm.review(DOC, [row("ITEMNAME", 10), row("ROL", 20, FldDataType="Numeric")], HDR, [], "items")
    assert not [m for m in msgs if m["level"] in ("error", "warn")]
