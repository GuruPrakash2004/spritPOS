# Notification and Form Validation Guidelines

This repository follows standard UX patterns for notifications, form mistakes, and role permissions.

## 1. Popups / Notifications (Walmart-Style Top-Right Snackbars)
- **Positioning**: Fixed at the top right (`fixed right-4 top-4 z-[1000]`), stacked downwards, sliding in from the right; short-lived so a busy counter is never held up.
- **Card Aesthetics**: Calm surface card (`bg-surface dark:bg-[#141414] border border-line shadow-2xl rounded-xl`) with a 1.5px coloured left stripe (green for success, red for errors).
- **Dismissal & Timing**:
  - Every popup (success and error) auto-dismisses after **1.5 seconds on large screens (1024 px wide and up, Tailwind `lg`) and 2 seconds on smaller ones** (`toastDuration()` in `toastStore.js`, decided when the popup opens) with a thin bottom countdown bar. Hovering pauses it (`onMouseEnter`) so the user can read a long message, and `onMouseLeave` resumes it. The `✕` button closes it early.
- **Every action waits for its popup (decide before it happens)**: every user action that changes data or fetches/exports on request — save, delete, refresh, hold, recall, cancel bill, generate, upload, import, export, email, password change, rate update, backup — first shows a waiting popup and runs **only when that popup closes**. Call `toastStore.defer(kind, text, cancelledText)` (or `toast.later`) after the form has passed validation (and after any confirm dialog) and return if it resolves `false`:
  `if (!(await toastStore.defer('ok', 'Bill will be saved when this closes.', 'Save cancelled. The bill is still open.'))) return;`
  The popup text says what will happen ("… will be saved/deleted/refreshed when this closes."); use kind `ok` for saves, `delete` for deletes and cancels, `refresh` for refreshes, `backup` for backups. It shows a **Cancel** button and stays the same time as every popup (1.5 s / 2 s, `toastDuration()`; hovering pauses it). The action runs when the popup closes (the countdown ends or `✕`); Cancel skips it and shows `cancelledText`. Build the request body **before** awaiting, so what is saved is what was on screen when the button was pressed. Pressing the button again while its popup is waiting only shakes that popup, so the action runs once. From the keyboard, **Enter** runs the waiting action at once and **Esc** cancels it (the newest waiting popup; `SnackbarContainer` takes these keys before any screen or dialog, except Enter in a search box with text, which is a scan); while a sales bill is being sent it can't be changed or sent again, so a cashier can save a bill with F10 then Enter without waiting for the countdown. Validation mistakes are still shown inline straight away (they never wait), and the server's result is a normal popup after the request. Component tests mock it: `vi.spyOn(toastStore, 'defer').mockResolvedValue(true)`.
- **Motion**: the countdown bar runs on `requestAnimationFrame` (every frame). Cards slide in and fade out (`toast-enter` / `toast-leaving` in `index.css`; `toastStore.remove()` waits for the exit): 0.25 s in and 0.22 s out on large screens, and a softer 0.48 s in and 0.38 s out below `lg`. With reduced motion turned on, cards appear and close without animating.
- **De-duplication**: Repeated triggers of the exact same message shake the existing card, display a multiplier (`×2`, `×3`), and restart the timer rather than spawning duplicates.
- **Stacking**: Maximum 3 cards stacked vertically.
- **Dark Mode**: Both snackbars and modals follow the active dark mode theme classes (`dark:bg-[#141414] dark:border-[#2A2A2A]`).

## 2. Form Mistakes (Amazon-Style Inline Validation)
- Client-side validation errors must **NOT** be displayed as popups/toasts.
- **Visual Feedback**:
  - The faulty input border turns red and shakes (`!border-red-500 !ring-2 !ring-red-500/20 animate-shake`).
  - An inline error appears directly beneath the input (`<InlineError message="Enter the..." />`).
  - Cursor automatically jumps into the first invalid input box upon submit.
- **"There was a problem" Summary Card**:
  - Displayed at the top of the form with an alert icon, listing all errors in a bullet list.
  - Clicking any error line smoothly scrolls to and focuses that exact input field.
- **Instant Clearing**:
  - The red border and error message clear as soon as the user begins typing in that field (`onChange`).

## 3. Role Permissions Hierarchy
- **Administrator**: Unrestricted system authority (`*`).
- **Manager**: Supervisory role: pricing, operational reports, item creation, and item deletion.
- **Cashier**: Front-counter POS billing, daily closing, and **Item Arrival & Creation (Add Items)** when managers are absent; **Delete Items is strictly blocked** both in UI and backend API (`403 Forbidden`).

## 4. ERP Keyboard Actions (Mac & Windows)
- **ArrowUp / ArrowDown**: Moves focus consecutively between form fields; in table list mode, selects previous / next records.
- **Enter**: Submits and saves the active form.
- **Escape**: Cancels current edit / new mode or closes dialogs.
- **Cmd+S (Mac) / Ctrl+S (Windows)**: Save shortcut.
- **Cmd+N (Mac) / Ctrl+N (Windows)**, or **Alt+N**: New record shortcut.
- The full set is implemented in `MasterCrud.jsx`, and Company and Supplier Master have their own handlers. New forms should copy that handler. Shortcuts are ignored while a `[role="dialog"]` is open.
- **Billing runs from the keyboard** (the mouse is the exception). Screen shortcuts use `useShortcuts()` from `lib/keys.js` (`"F10"`, `"Mod+S"` = ⌘/Ctrl, `"Alt+R"` matched on the physical key so ⌥ works on a Mac); entry screens put `enterMovesNext` on their wrapper (Enter = next field, Shift+Enter = previous, Enter on a grid's last field adds a row); buttons show their key with `<Kbd combo="F10" />`. Sales Invoice: F2 item search (suggestions ↑↓ Enter; `3*` sets the qty), F3 quick pick, F4 customer, F6/Alt+P payment type, F7 hold, F8 held bills, F9 cash tendered (Enter saves), F10 save & print, ⌘/Ctrl+S save only, Alt+R re-print, Alt+C cancel invoice, Alt+X cancel bill; in the empty search ↑↓ choose a line, + / − change its qty, Delete removes it. Purchase Bill: F2 item lookup (↑↓ Enter; the cursor goes to PB Qty), Alt+A add row, ⌘/Ctrl+S save, F10 save & print, Alt+X cancel; Purchase Indent / Sales Return / Stock Deduction: ⌘/Ctrl+S save, F10 save & print, Alt+A add row. Alt+B opens a new sales bill from any screen (it asks first when you leave an entry screen) and `?` the shortcuts sheet (`KeyboardShortcutsModal.jsx`, keep it in step).
- **Dialogs** (`Modal`, the print dialog) use `common/useDialogKeys.js`: focus moves in (autoFocus / `data-autofocus`, else the first field, else the last footer button), Tab stays inside, Esc closes only the top dialog, and focus returns afterwards. Put a dialog's main action last in its footer. The print dialog opens with Print focused (Enter prints, ↑↓ choose the format).
