// DateInput: the app's date box with its own calendar (typed dd/mm/yyyy, calendar button, Today / Clear, today written out, min / max).
import { render, screen, within } from '@testing-library/react';
import userEvent from '@testing-library/user-event';
import { useState } from 'react';
import { describe, expect, it } from 'vitest';
import DateInput from './DateInput';

function Box(props) {
  const [v, setV] = useState(props.start ?? '2026-11-25');
  return <DateInput id="d" aria-label="Day" value={v} onChange={e => setV(e.target.value)} {...props} />;
}

describe('DateInput', () => {
  it('shows the date as dd/mm/yyyy and takes a typed date when the box is left', async () => {
    const user = userEvent.setup();
    render(<Box />);
    const box = screen.getByLabelText('Day');
    expect(box).toHaveValue('25/11/2026');
    await user.clear(box); await user.type(box, '3/2/27'); await user.tab();
    expect(box).toHaveValue('03/02/2027');
    await user.clear(box); await user.type(box, '31/02/2027'); await user.tab();
    expect(box).toHaveValue('03/02/2027');   // not a real date: the old one stays
  });

  it('opens a calendar with the month, today written out, and picks a day', async () => {
    const user = userEvent.setup();
    render(<Box />);
    await user.click(screen.getByRole('button', { name: 'Open calendar' }));
    const cal = screen.getByRole('application', { name: 'Calendar' });
    expect(within(cal).getByLabelText('Month')).toHaveValue('10');
    expect(within(cal).getByText(/^Today:/)).toBeInTheDocument();
    await user.click(within(cal).getByLabelText('2026-11-18'));
    expect(screen.queryByRole('application')).toBeNull();
    expect(screen.getByLabelText('Day')).toHaveValue('18/11/2026');
  });

  it('Today and Clear work, and days outside min / max can not be picked', async () => {
    const user = userEvent.setup();
    render(<Box max="2026-11-25" start="2026-11-10" />);
    await user.click(screen.getByRole('button', { name: 'Open calendar' }));
    expect(screen.getByLabelText('2026-11-26')).toBeDisabled();
    await user.click(screen.getByRole('button', { name: 'Clear' }));
    expect(screen.getByLabelText('Day')).toHaveValue('');
    await user.click(screen.getByRole('button', { name: 'Open calendar' }));
    await user.click(screen.getByRole('button', { name: 'Today' }));
    expect(screen.getByLabelText('Day').value).toMatch(/^\d{2}\/\d{2}\/\d{4}$/);
  });

  it('Esc closes only the calendar', async () => {
    const user = userEvent.setup();
    render(<Box />);
    await user.click(screen.getByRole('button', { name: 'Open calendar' }));
    await user.keyboard('{Escape}');
    expect(screen.queryByRole('application')).toBeNull();
  });
});
