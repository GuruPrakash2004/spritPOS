"""Field checks as the desktop forms enforce them (app/validation.py): 10-digit phone numbers (not on Company Master), PIN, GSTIN, PAN, HSN, tax %."""
import pytest

SUPPLIER = {"code": "VAL1", "name": "Val Supplier", "address": "MG Road", "city": "Bengaluru", "pin": "560001",
            "state": "Karnataka", "phone": "9900000000", "gstin": "29ABCDE1234F1Z5"}


@pytest.mark.parametrize("phone", ["99000", "+919900765776 / +918884888529", "080-26793693", "9900000000, 8800000000, 7700000000"])
async def test_company_phone_numbers_are_free_text(client, phone):
    """Company Master's Tel / Mobile / Excise phone take any text, so several numbers can be entered (correction #1)."""
    company = (await client.get("/masters/company")).json()
    for field in ("phone", "mobile", "excise_phone"):
        res = await client.put("/masters/company", json=company | {field: phone})
        assert res.status_code == 200, (field, phone, res.text)
        assert (await client.get("/masters/company")).json()[field] == phone


async def test_company_accepts_ten_digits_and_blank(client):
    company = (await client.get("/masters/company")).json()
    res = await client.put("/masters/company", json=company | {"phone": "8026793693", "mobile": "", "excise_phone": "9900196276"})
    assert res.status_code == 200


@pytest.mark.parametrize("field,value", [("gstin", "29ABCDE1234F1"), ("gstin", "ABCDE1234F1Z5XX"), ("pan", "ABCDE1234"), ("pan", "12345ABCDE")])
async def test_company_gstin_and_pan_patterns(client, field, value):
    company = (await client.get("/masters/company")).json()
    res = await client.put("/masters/company", json=company | {field: value})
    assert res.status_code == 422


async def test_company_gstin_and_pan_are_kept_in_capitals(client):
    company = (await client.get("/masters/company")).json()
    res = await client.put("/masters/company", json=company | {"gstin": "29abcde1234f1z5", "pan": "abcde1234f"})
    assert res.status_code == 200
    assert res.json()["gstin"] == "29ABCDE1234F1Z5" and res.json()["pan"] == "ABCDE1234F"


@pytest.mark.parametrize("change", [{"phone": "98-4500"}, {"pin": "5600"}, {"pin": "56000A"}, {"gstin": "NOTAGSTIN"}])
async def test_supplier_rejects_bad_fields(client, change):
    res = await client.post("/masters/suppliers", json=SUPPLIER | change)
    assert res.status_code == 422


async def test_supplier_accepts_good_fields(client):
    assert (await client.post("/masters/suppliers", json=SUPPLIER)).status_code == 201


@pytest.mark.parametrize("mobile", ["98450", "98450000011", "98450x0001"])
async def test_customer_mobile_must_be_ten_digits(client, mobile):
    res = await client.post("/masters/customers", json={"code": "VC1", "name": "Val Customer", "mobile": mobile})
    assert res.status_code == 422
    assert "10 digits" in res.json()["detail"]


async def test_sales_person_mobile_must_be_ten_digits(client):
    assert (await client.post("/masters/sales-persons", json={"code": "VS1", "name": "Val SP", "mobile": "12345"})).status_code == 422
    assert (await client.post("/masters/sales-persons", json={"code": "VS2", "name": "Val SP2", "mobile": "9845012345"})).status_code == 201


async def test_item_hsn_and_tax_percentages(client):
    item = (await client.get("/masters/items/VIN-750")).json()
    assert (await client.put("/masters/items/VIN-750", json=item | {"hsn_code": "22A8"})).status_code == 422
    assert (await client.put("/masters/items/VIN-750", json=item | {"cgst_per": 120})).status_code == 422
    assert (await client.put("/masters/items/VIN-750", json=item | {"sgst_per": -1})).status_code == 422
    assert (await client.put("/masters/items/VIN-750", json=item | {"hsn_code": "2208", "cgst_per": 9, "sgst_per": 9})).status_code == 200


async def test_mandatory_fields_of_the_desktop_masters(client):
    """Supplier GSTIN, Tax Type and the bank's Display Name are mandatory in the desktop's designer (UtlDocDsgrFlds)."""
    assert (await client.post("/masters/suppliers", json=SUPPLIER | {"gstin": ""})).status_code == 422
    assert (await client.post("/masters/taxes", json={"code": "T1", "name": "T1", "tax_type": ""})).status_code == 422
    bank = {"code": "B1", "name": "Bank", "display": "", "ifsc": "SBIN0001234", "account_no": "123456789012"}
    assert (await client.post("/masters/banks", json=bank)).status_code == 422
    assert (await client.post("/masters/banks", json=bank | {"display": "Bank"})).status_code == 201


@pytest.mark.parametrize("change", [{"ifsc": "SBIN1001234"}, {"ifsc": "SBI00012345"}, {"micr": "12345"}, {"account_no": "12AB"}, {"pan": "ABCDE1234"}])
async def test_bank_codes_are_checked(client, change):
    bank = {"code": "B2", "name": "Bank", "display": "Bank"}
    assert (await client.post("/masters/banks", json=bank | change)).status_code == 422
