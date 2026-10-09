"""The formula engine against the shared vectors (also run by frontend/src/lib/formulas.test.js)."""
import json
from decimal import Decimal
from pathlib import Path

import pytest

from app import formulas

VECTORS = json.loads((Path(__file__).parent / "formula_vectors.json").read_text())


@pytest.mark.parametrize("amount,text", VECTORS["words"])
def test_amount_in_words(amount, text):
    assert formulas.amount_in_words(amount) == text


@pytest.mark.parametrize("amount,expected", VECTORS["round50"])
def test_round_to_the_rupee_half_up(amount, expected):
    assert str(formulas.round_half_up(Decimal(str(amount)), 0)) == expected


def test_a_document_is_worked_out_in_dependency_order():
    case = VECTORS["doc"]
    res = formulas.compute(case["fields"], case["values"], case["rows"])
    for code, want in case["expect"]["values"].items():
        got = res["values"][code]
        assert (None if got is None else str(got)) == want, code
    assert [{k: str(v) for k, v in r.items()} for r in res["rows"]] == case["expect"]["rows"]
    assert {p.split(":")[0] for p in res["problems"]} == {p.split(":")[0] for p in case["expect"]["problems"]}


def test_unreadable_formulas_leave_the_field_as_typed():
    res = formulas.compute([{"code": "X", "section": "Total", "type": "number", "decimals": 2, "formula": "#A# +"}], {"X": "7"}, [])
    assert res["values"]["X"] == "7" and res["problems"]


def test_a_formula_that_refers_to_itself_does_not_loop():
    res = formulas.compute([{"code": "A", "section": "Total", "type": "number", "decimals": 2, "formula": "#A# + 1"}], {}, [])
    assert res["problems"]


@pytest.mark.parametrize("case", VECTORS["functions"], ids=lambda c: c["formula"] + str(c["values"]))
def test_the_other_designer_functions(case):
    fields = [{"code": "R", "section": "Total", "type": "text", "decimals": 2, "formula": case["formula"]},
              {"code": "X", "section": "Detail", "type": "number", "decimals": 2, "formula": ""}]
    got = formulas.compute(fields, case["values"], case["rows"], {})["values"]["R"]
    assert (str(got.normalize()) if isinstance(got, Decimal) else str(got)) == case["expect"]
