# vLQMS Print Agent (printing on a counter PC's own printers)

A web page can't list or use the printers of the PC it is open on. This small program runs **on each counter PC**
(listens on `127.0.0.1:8766` only) so the web app can:

- list every printer connected to that PC (USB or network) and show it under **This PC** in the Print window,
- print **directly**, with no print window, to any of them,
- use that PC's **default printer** for a Save & Print / direct print.

The server draws the report (Crystal, as before) and sends page pictures; the agent prints them on the PC.
No database is touched, and the choice of printer is kept in the browser (localStorage).

## The easy way (no files to copy, nothing to build)

The server builds the agent by itself when the backend starts. On a counter PC open vLQMS → any bill → Print →
**Download Print Agent for this PC**, then double-click the downloaded `vLQMS-Print-Agent-Setup.cmd` once. It is already set
to your app's address and installs and starts the agent; the Print window finds it by itself within a few seconds.
From then on it starts at every sign-in. The manual steps below are only for special cases.

## Install by hand (once per counter PC, no administrator needed)

1. Copy the `printagent` folder to the counter PC (it holds `bin\VlqmsPrintAgent.exe`; run `build.cmd` on any Windows PC to
   make it again, it needs nothing but the .NET Framework that comes with Windows).
2. On the counter PC, signed in as the cashier, run `install.cmd` and type the address the shop opens vLQMS at
   (for example `http://192.168.1.10`, exactly as in the browser's address bar, no path).
   It copies the agent to `%LOCALAPPDATA%\VlqmsPrintAgent`, starts it at every sign-in and starts it now.
3. Check: open `http://127.0.0.1:8766/health` in that PC's browser. It shows the PC's name, its default printer and its printers.
4. Open vLQMS, any bill → Print. The dropdown lists **This PC**; the default printer is chosen first.

`uninstall.cmd` removes it.

## Safety

Only the web addresses listed in `printagent.txt` (next to the exe, one per line) may use the agent; any other page
gets 403. Nothing outside the PC can reach it (loopback only).

## If it doesn't show

- `/health` doesn't open: the agent isn't running. Run `%LOCALAPPDATA%\VlqmsPrintAgent\VlqmsPrintAgent.exe`; see `printagent.log` there.
- `/health` opens but the Print window still shows no "This PC": the address in `printagent.txt` doesn't match the browser's
  address bar exactly (http vs https, the port, a name vs an IP). Fix the line and sign out and in again (or restart the agent).
- The browser asks to "access devices on your local network": allow it.
- Printing fails with "no printer called …": the printer was removed or renamed on that PC.
- A receipt prints too small or cut: set the printer's paper to the roll width in Windows (Printing preferences); the agent uses the report's own page size.
