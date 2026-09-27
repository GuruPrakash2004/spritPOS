# Notification and Form Validation Guidelines

This repository follows standard UX patterns for notifications, form mistakes, and role permissions.

## 1. Popups / Notifications (Walmart-Style Bottom-Centre Snackbars)
- **Positioning**: Fixed at bottom centre (`fixed bottom-6 left-1/2 -translate-x-1/2 z-[1000]`), sliding up without blocking the search bar or header buttons.
- **Card Aesthetics**: Calm surface card (`bg-surface dark:bg-[#141414] border border-line shadow-2xl rounded-xl`) with a 1.5px coloured left stripe (green for success, red for errors).
- **Dismissal & Timing**:
  - Every popup (success and error) auto-dismisses after **3 seconds** (`TOAST_DURATION_MS` in `toastStore.ts`) with a thin bottom countdown bar. Hovering pauses it (`onMouseEnter`) so the user can read a long message, and `onMouseLeave` resumes it. The `✕` button closes it early.
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
