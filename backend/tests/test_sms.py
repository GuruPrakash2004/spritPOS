"""SMS (the desktop's UtlMsgSettings / TrnSMS): with no database there are no providers and nothing is sent."""


async def test_no_database_means_sms_off(client):
    res = await client.get("/sms/providers")
    assert res.status_code == 200 and res.json() == {"enabled": False, "providers": []}
    assert (await client.get("/sms/history", params={"from": "2026-11-01", "to": "2026-11-02"})).json() == []
    sent = (await client.post("/sms/send", json={"mobiles": ["9876543210"], "message": "hi"})).json()
    assert sent["sent"] == 0 and sent["failed"][0]["reason"]


async def test_history_dates_are_checked_and_login_is_needed(client, anon_client):
    assert (await client.get("/sms/history", params={"from": "2026-11-03", "to": "2026-11-02"})).status_code == 422
    assert (await anon_client.get("/sms/providers")).status_code == 401
