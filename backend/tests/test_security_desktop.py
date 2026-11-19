"""The desktop's Login Settings (UtlMisc password policy): rule counts, validation messages and expiry, as the exe does them."""
from datetime import date

import pytest
from fastapi import HTTPException

from app import security, store

POLICY = {"PwdPolicy": "Y", "PwdRules": "Y", "NeverExpires": "N", "PwdExpiryDays": 30, "PwdMinSplChrs": 1, "PwdMinNumeric": 2, "PwdMinAlpha": 3,
          "PwdMinLen": 8, "PwdMinLCase": 1, "PwdMinUCase": 2}


@pytest.fixture
def policy(monkeypatch):
    monkeypatch.setattr(store, "UTL_MISC", dict(POLICY))


def test_no_policy_columns_means_no_desktop_rules(monkeypatch):
    monkeypatch.setattr(store, "UTL_MISC", {})
    assert security.desktop_login() is None and security.desktop_rules_problem("x") is None


def test_rules_count_digits_letters_and_specials(policy):
    assert security.desktop_rules_problem("AB#12xyz") is None            # 1 special, 2 digits, 5 letters, 2 capitals
    assert "capital" in security.desktop_rules_problem("ab#12xyz")       # needs 2 capital letters
    assert "special" in security.desktop_rules_problem("ABc12xyz")
    assert "number" in security.desktop_rules_problem("ABc#1xyz")
    assert "characters" in security.desktop_rules_problem("ABc#12")


def test_rules_off_when_policy_or_rules_flag_is_off(monkeypatch):
    monkeypatch.setattr(store, "UTL_MISC", {**POLICY, "PwdRules": "N"})
    assert security.desktop_rules_problem("a") is None
    monkeypatch.setattr(store, "UTL_MISC", {**POLICY, "PwdPolicy": "N"})
    assert security.desktop_rules_problem("a") is None


@pytest.mark.parametrize("body,message", [
    ({"policy": True, "rules": False, "expiry_days": 0}, "Please enter the Password Expiry Days!!"),
    ({"policy": True, "rules": True, "never_expires": True, "min_special": 5, "min_numeric": 5, "min_alpha": 5, "min_length": 8, "min_lower": 2, "min_upper": 3},
     "Min. no of Special Character + Alphabets + Numeric should not be less than the Password length!!"),
    ({"policy": True, "rules": True, "never_expires": True, "min_alpha": 4, "min_length": 8, "min_lower": 1, "min_upper": 1},
     "Mni. No of LowerCase + UpperCase should be equal to Alphabets Length!!"),
])
def test_login_settings_validation_messages(body, message):
    with pytest.raises(HTTPException) as e:
        security.desktop_login_check(body)
    assert e.value.detail == message


def test_policy_off_clears_everything_as_the_exe_does():
    out = security.desktop_login_check({"policy": False, "rules": True, "never_expires": True, "expiry_days": 9, "min_length": 8})
    assert out["policy"] is False and out["rules"] is False and out["expiry_days"] == 0 and out["min_length"] == 0


def test_expiry_counts_from_the_last_change_and_exempts_admin(policy):
    user = {"user": "RAVI", "password_reset_on": "2026-01-01"}
    assert security.desktop_expired(user, date(2026, 2, 5)) is True       # 30 days passed
    assert security.desktop_expired(user, date(2026, 1, 20)) is False
    assert security.desktop_days_left(user, date(2026, 1, 20)) == 11
    assert security.desktop_expired({**user, "user": "ADMIN"}, date(2026, 6, 1)) is False
