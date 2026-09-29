# Notification and Form Validation Guidelines

This repository follows standard UX patterns for notifications, form mistakes, and role permissions.

## 1. Popups / Notifications (Walmart-Style Bottom-Centre Snackbars)
- **Positioning**: Fixed at bottom centre (`fixed bottom-6 left-1/2 -translate-x-1/2 z-[1000]`), sliding up without blocking the search bar or header buttons.
- **Card Aesthetics**: Calm surface card (`bg-surface dark:bg-[#141414] border border-line shadow-2xl rounded-xl`) with a 1.5px coloured left stripe (green for success, red for errors).
- **Dismissal & Timing**:
  - Every popup (success and error) auto-dismisses after **2.5 seconds on large screens (1024 px wide and up, Tailwind `lg`) and 3.2 seconds on smaller ones** (`toastDuration()` in `toastStore.ts`, decided when the popup opens) with a thin bottom countdown bar. Hovering pauses it (`onMouseEnter`) so the user can read a long message, and `onMouseLeave` resumes it. The `✕` button closes it early.
- **Every action waits for its popup (decide before it happens)**: every user action that changes data or fetches/exports on request — save, delete, refresh, hold, recall, cancel bill, generate, upload, import, export, email, password change, rate update, backup — first shows a waiting popup and runs **only when that popup closes**. Call `toastStore.defer(kind, text, cancelledText)` (or `toast.later`) after the form has passed validation (and after any confirm dialog) and return if it resolves `false`:
  `if (!(await toastStore.defer('ok', 'Bill will be saved when this closes.', 'Save cancelled. The bill is still open.'))) return;`
  The popup text says what will happen ("… will be saved/deleted/refreshed when this closes."); use kind `ok` for saves, `delete` for deletes and cancels, `refresh` for refreshes, `backup` for backups. It shows a **Cancel** button and stays the same time as every popup (2.5 s / 3.2 s, `toastDuration()`; hovering pauses it). The action runs when the popup closes (the countdown ends or `✕`); Cancel skips it and shows `cancelledText`. Build the request body **before** awaiting, so what is saved is what was on screen when the button was pressed. Pressing the button again while its popup is waiting only shakes that popup, so the action runs once. Validation mistakes are still shown inline straight away (they never wait), and the server's result is a normal popup after the request. Component tests mock it: `vi.spyOn(toastStore, 'defer').mockResolvedValue(true)`.
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
- The full set is implemented in `MasterCrud.tsx`, and Company and Supplier Master have their own handlers. New forms should copy that handler. Shortcuts are ignored while a `[role="dialog"]` is open.
