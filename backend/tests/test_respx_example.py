"""How to test code that calls an outside HTTP service, using respx.

The backend makes no outgoing HTTP calls yet. When one is added (an SMS gateway for loyalty
messages, a payment provider, …), mock the service like this instead of calling it for real.
The `block_network` fixture in conftest.py already has respx switched on; take it as an argument to
add routes to it.
"""
import httpx
import pytest
from respx.models import AllMockedAssertionError

SMS_URL = "https://sms.example.com/send"


async def send_sms(mobile: str, text: str) -> str:
    """Stand-in for a future app helper that posts to an SMS provider."""
    async with httpx.AsyncClient() as http:
        res = await http.post(SMS_URL, json={"to": mobile, "text": text})
        res.raise_for_status()
        return res.json()["id"]


async def test_mocked_service_returns_the_canned_response(block_network):
    route = block_network.post(SMS_URL).mock(return_value=httpx.Response(200, json={"id": "msg-1"}))

    assert await send_sms("9876543210", "You earned 12 points") == "msg-1"
    assert route.call_count == 1
    assert route.calls.last.request.read() == b'{"to":"9876543210","text":"You earned 12 points"}'


async def test_service_errors_can_be_simulated(block_network):
    block_network.post(SMS_URL).mock(return_value=httpx.Response(503))

    with pytest.raises(httpx.HTTPStatusError):
        await send_sms("9876543210", "hello")


async def test_unmocked_calls_never_reach_the_network():
    with pytest.raises(AllMockedAssertionError):
        await send_sms("9876543210", "hello")
