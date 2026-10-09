



import { field, fieldError, fieldTone, label } from '../../lib/styles';
import { cn } from '../../lib/utils';
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
  designerReadOnly = false,   // locked by the Doc Designer (blue), not just a screen in view mode
  hasError = false,
  errorMessage,
  onChange,
  inputMode,
  maxLength,
}













) {
  return (
    <div className={wide ? 'sm:col-span-2' : ''}>
      <label htmlFor={id} className={cn(label, fieldTone(required, designerReadOnly))}>
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
        inputMode={inputMode}
        maxLength={maxLength}
        required={required}
        readOnly={readOnly}
        aria-invalid={hasError}
        aria-errormessage={hasError && errorMessage ? `${id}-err` : undefined}
        className={cn(field, className, designerReadOnly && 'text-blue-700 dark:text-blue-300', hasError && fieldError)}
      />
      <InlineError message={errorMessage} id={`${id}-err`} />
    </div>
  );
}
