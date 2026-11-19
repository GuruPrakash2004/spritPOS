"""item_rates.sync: the Item Selling Rate follows MRP / GST / Cess even when they were changed in the database."""
from app import db, item_rates, store
from app.routers import masters


def test_a_stale_selling_rate_is_recalculated_and_written_back_once(monkeypatch):
    fields = [{"FldCode": "ITEMSELL", "FldFormula": "SVTROUND2((#MRP#-#COMPCESS#)/(100+#CGSTPER#+#SGSTPER#+#CESSPER#)*100)", "FldBlnReadOnly": "Y", "FldNumDecimals": 2}]
    have = {"ITEMSELL": "ITEMSELL", "MRP": "MRP"}
    monkeypatch.setattr(masters, "_item_formula_context", lambda: (fields, have))
    monkeypatch.setattr(db, "is_db_available", lambda: True)
    monkeypatch.setattr(db, "READ_ONLY", False)
    writes = []

    class Cur:
        def execute(self, sql, params=()):
            writes.append(params)

    class Tx:
        def __enter__(self): return Cur()
        def __exit__(self, *a): return False

    monkeypatch.setattr(db, "transaction", lambda: Tx())
    saved = dict(store.ITEMS)
    store.ITEMS.clear()
    store.ITEMS["T-STALE"] = {"code": "T-STALE", "_itemid": "9001", "mrp": 395.0, "cgst_per": 2.0, "sgst_per": 2.0,
                              "extras": {"CESSPER": 10, "COMPCESS": 100, "ITEMSELL": 230.47}}   # GST edited in the database, rate left behind
    store.ITEMS["T-OK"] = {"code": "T-OK", "_itemid": "9002", "mrp": 99.0, "cgst_per": 6.0, "sgst_per": 0.0,
                           "extras": {"CESSPER": 6, "COMPCESS": 0, "ITEMSELL": 88.39}}
    try:
        assert item_rates.sync() == 1
        assert store.ITEMS["T-STALE"]["extras"]["ITEMSELL"] == 258.77
        assert writes == [(258.77, 9001)]
        assert item_rates.sync() == 0          # already right: nothing more to write
    finally:
        store.ITEMS.clear()
        store.ITEMS.update(saved)


def test_reloading_items_after_a_bulk_change_reads_their_extra_columns_again(monkeypatch):
    """Update Item Rates / Update Item Master Fields reload the items; load_items() alone leaves them without extras (Selling Rate, Cess ...)."""
    from app import db_store, sqlstore
    order = []
    monkeypatch.setattr(db_store, "load_items", lambda: order.append("items"))
    monkeypatch.setattr(sqlstore, "reload_extras", lambda: order.append("extras"))
    db_store.reload_items()
    assert order == ["items", "extras"]
