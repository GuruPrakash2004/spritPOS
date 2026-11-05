// Screens ask the app shell to open another screen without importing it: the shell (App.jsx) listens for this event.
// The action bar's Close button uses goHome(), as the exe's Close returns to the main window.

export const OPEN_MODULE = 'vlqms-open-module';
export const openModule = (name) => window.dispatchEvent(new CustomEvent(OPEN_MODULE, { detail: name }));
export const goHome = () => openModule('Dashboard');
