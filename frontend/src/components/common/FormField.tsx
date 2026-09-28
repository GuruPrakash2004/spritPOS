// TextField: a labelled input that shows the inline-error style (red border, shake, message below)
// when `error` is set. Part of the form-validation pattern in .claude/rules/popups-and-errors.md.

import type { ChangeEvent } from 'react';
import { field, fieldError, label } from '../../lib/styles';
import { InlineError } from './FormErrorSummary';

// Labelled input supporting Amazon-style error highlight and inline errors.
export function TextField({
  id,
  label: text,
  value,
  defaultValue,
  placeholder,
  type = 'text',
  className = '',
  wide = false,
  required = false,
  readOnly = false,
  hasError = false,
  errorMessage,
  onChange,
}: {
  id: string;
  label: string;
  value?: string;
  defaultValue?: string;
  placeholder?: string;
  type?: string;
  className?: string;
  wide?: boolean;
  required?: boolean;
  readOnly?: boolean;
  hasError?: boolean;
  errorMessage?: string;
  onChange?: (e: ChangeEvent<HTMLInputElement>) => void;
}) {
  return (
    <div className={wide ? 'sm:col-span-2' : ''}>
      <label htmlFor={id} className={label}>
        {text}
        {required && <span className="ml-0.5 text-red-600" aria-hidden="true">*</span>}
      </label>
      <input
        id={id}
        type={type}
        value={value}
        defaultValue={defaultValue}
        placeholder={placeholder}
        onChange={onChange}
        required={required}
        readOnly={readOnly}
        aria-invalid={hasError}
        aria-errormessage={hasError && errorMessage ? `${id}-err` : undefined}
        className={`${field} ${className} ${hasError ? fieldError : ''}`}
      />
      <InlineError message={errorMessage} id={`${id}-err`} />
    </div>
  );
}
