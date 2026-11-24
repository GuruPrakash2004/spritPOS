"""The masters that carry a "File Attachment" button, and the Global Settings row that shows or hides it for each.

Used by routers/attachments.py (which master path belongs to which module) and app/gsettings.py (one Y/N row per master). Pure data, so
both can import it without a cycle."""
from .master_specs import SPECS

# master path -> module name (the same names the rights use)
MASTER_MODULES: dict[str, str] = {
    "items": "Item Master", "item-groups": "Item Group Master", "uoms": "UOM Master", "packages": "Package Master", "brands": "Brand Master",
    "suppliers": "Supplier Master", "customers": "Customer Master", "ledger-groups": "Ledger Group Master", "ledgers": "Ledger Master",
    "sales-counters": "Sales Counter Master", "sales-persons": "Sales Person Master", **{s["path"]: s["title"] for s in SPECS},
}


def setting_code(path: str) -> str:
    """The Global Settings row of a master: Y shows its File Attachment button, N hides it (e.g. item-groups -> AttachFile_item_groups)."""
    return "AttachFile_" + path.replace("-", "_")
