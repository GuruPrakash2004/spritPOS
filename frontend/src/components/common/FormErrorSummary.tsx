// Form-validation widgets (see .claude/rules/popups-and-errors.md):
// - FormErrorSummary: the "There was a problem" card at the top of a form; clicking a line focuses that field.
// - InlineError: the red message shown under a single field.

import type { FC } from 'react';
import { AlertCircle } from 'lucide-react';

interface FormErrorSummaryProps {
  errors: Record<string, string>;
  onFocusField?: (fieldKey: string) => void;
  className?: string;
}

export const FormErrorSummary: FC<FormErrorSummaryProps> = ({ errors, onFocusField, className = '' }) => {
  const errorKeys = Object.keys(errors);
  if (errorKeys.length === 0) return null;

  return (
    <div
      role="alert"
      className={`mb-5 rounded-xl border border-red-500/50 bg-red-500/[0.06] p-4 text-xs dark:border-red-500/40 dark:bg-red-950/25 animate-shake shadow-sm ${className}`}
    >
      <div className="flex items-start gap-2.5">
        <AlertCircle size={18} className="mt-0.5 shrink-0 text-red-600 dark:text-red-400" />
        <div className="flex-1">
          <h3 className="font-bold text-sm text-red-900 dark:text-red-300">
            There was a problem
          </h3>
          <ul className="mt-2 list-disc list-inside space-y-1 text-red-800 dark:text-red-300">
            {errorKeys.map(k => (
              <li key={k}>
                <button
                  type="button"
                  onClick={() => onFocusField?.(k)}
                  className="inline text-left underline decoration-red-400 underline-offset-2 hover:text-red-950 dark:hover:text-white cursor-pointer transition-colors"
                >
                  {errors[k]}
                </button>
              </li>
            ))}
          </ul>
        </div>
      </div>
    </div>
  );
};

export const InlineError: FC<{ message?: string; id?: string }> = ({ message, id }) => {
  if (!message) return null;
  return (
    <p
      id={id}
      role="alert"
      className="mt-1.5 flex items-center gap-1.5 text-[11px] font-medium text-red-600 dark:text-red-400 animate-slideUp"
    >
      <AlertCircle size={12} className="shrink-0" />
      <span>{message}</span>
    </p>
  );
};

export default FormErrorSummary;
