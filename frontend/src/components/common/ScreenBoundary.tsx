// Catches an error thrown while a screen draws or starts up, so the user sees what went wrong (and can retry or
// go to the Dashboard) instead of a blank page. App.tsx wraps the open screen in it, keyed by the screen's name.
import { Component, type ReactNode } from 'react';

type Props = { children: ReactNode; onHome: () => void };

export default class ScreenBoundary extends Component<Props, { error: Error | null }> {
  state = { error: null as Error | null };

  static getDerivedStateFromError(error: Error) { return { error }; }

  componentDidCatch(error: Error) { console.error('Screen failed to open:', error); }

  render() {
    const { error } = this.state;
    if (!error) return this.props.children;
    return (
      <div role="alert" className="mx-auto mt-16 max-w-md rounded-xl border border-red-500/40 bg-surface p-6 text-center">
        <h2 className="text-sm font-semibold text-red-700 dark:text-red-300">This screen could not open</h2>
        <p className="mt-2 break-words text-xs text-stone-600 dark:text-stone-300">{error.message || 'Unknown error'}</p>
        <div className="mt-4 flex justify-center gap-2">
          <button type="button" onClick={() => this.setState({ error: null })} className="rounded-lg border border-line px-3 py-1.5 text-xs font-medium">Try again</button>
          <button type="button" onClick={() => { this.setState({ error: null }); this.props.onHome(); }} className="rounded-lg border border-line px-3 py-1.5 text-xs font-medium">Go to Dashboard</button>
        </div>
      </div>
    );
  }
}
