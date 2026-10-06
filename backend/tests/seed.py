"""The tests' data: tests/sample_data.py with the demo logins and the sample company (conftest.py installs it)."""
import sample_data


def install() -> None:
    sample_data.install(with_users=True, with_company=True)
