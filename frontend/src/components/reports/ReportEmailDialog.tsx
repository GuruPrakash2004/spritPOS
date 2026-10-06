// Customised Reports → Email: asks who gets the report, then POSTs /reports/custom/email. The backend
// builds the .xls itself from the same report and filters and sends it through the SMTP server in
// backend/.env; without one it answers that email isn't set up. Mistakes show inline; the server's
// answer shows as a popup.

import { useState, type FormEvent } from 'react';
import Modal from '../common/Modal';
import { TextField } from '../common/FormField';
import { toast } from '../common/toastStore';
import { emailCustomReport, errorText } from '../../lib/api';
import { primaryButton, secondaryButton } from '../../lib/styles';

export type ReportEmailRequest = {
  name: string; from_date: string; to_date: string; ledger: string[]; supplier: string[]; group: string[];
};

const EMAIL = /^[^\s@,;<>]+@[^\s@,;<>]+\.[^\s@,;<>]+$/;
const MAX_RECIPIENTS = 5;

type Errors = Partial<Record<'to' | 'message', string>>;

export default function ReportEmailDialog({ request, defaultTo, onClose }: {
  request: ReportEmailRequest; defaultTo: string; onClose: () => void;
}) {
  const [to, setTo] = useState(defaultTo);
  const [message, setMessage] = useState('');
  const [errors, setErrors] = useState<Errors>({});
  const [sending, setSending] = useState(false);

  const submit = async (e: FormEvent) => {
    e.preventDefault();
    const list = to.split(/[,;\s]+/).map(s => s.trim()).filter(Boolean);
    const errs: Errors = {};
    const bad = list.find(a => !EMAIL.test(a));
    if (!list.length) errs.to = 'Enter at least one email address';
    else if (bad) errs.to = `"${bad}" is not a valid email address`;
    else if (list.length > MAX_RECIPIENTS) errs.to = `Send to at most ${MAX_RECIPIENTS} addresses at a time`;
    if (message.length > 1000) errs.message = 'Keep the message under 1000 characters';
    setErrors(errs);
    if (Object.keys(errs).length) {
      document.getElementById(`re-${Object.keys(errs)[0]}`)?.focus();
      return;
    }
    setSending(true);
    if (!(await toast.later('ok', `${request.name} will be emailed to ${list.join(', ')} when this closes.`, 'Email not sent.'))) {
      setSending(false);
      return;
    }
    emailCustomReport({ ...request, to: list, message })
      .then(res => {
        toast.ok(`${request.name} (${res.rows} rows) emailed to ${res.sent_to.join(', ')}`);
        onClose();
      })
      .catch(err => toast.error(errorText(err)))
      .finally(() => setSending(false));
  };

  return (
    <Modal
      title={`Email · ${request.name}`}
      onClose={onClose}
      footer={<>
        <button type="button" onClick={onClose} className={secondaryButton}>Cancel</button>
        <button type="submit" form="report-email" disabled={sending} className={primaryButton}>{sending ? 'Sending…' : 'Send'}</button>
      </>}
    >
      <form id="report-email" onSubmit={submit} className="space-y-4" noValidate>
        <TextField id="re-to" label="To" value={to} required placeholder="name@example.com, …"
          hasError={!!errors.to} errorMessage={errors.to}
          onChange={e => { setTo(e.target.value); setErrors(p => ({ ...p, to: undefined })); }} />
        <TextField id="re-message" label="Message (optional)" value={message}
          hasError={!!errors.message} errorMessage={errors.message}
          onChange={e => { setMessage(e.target.value); setErrors(p => ({ ...p, message: undefined })); }} />
        <p className="text-xs text-stone-500 dark:text-stone-400">
          The report is attached as an Excel (.xls) file, built from the same period and filters as Generate.
        </p>
      </form>
    </Modal>
  );
}
