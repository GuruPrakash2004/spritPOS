// The exe's "Select [Multiple] ..." window: Sel ticks, filter, Select All, Paste by column, Ok returns the ticked keys, Cancel / X close it.

import { render, screen, within } from '@testing-library/react';
import userEvent from '@testing-library/user-event';
import { describe, expect, it, vi } from 'vitest';
import MultiSelectDialog from './MultiSelectDialog';

const ROWS = [
  { code: 'A1', name: 'WHISKY' },
  { code: 'B2', name: 'RUM' },
  { code: 'C3', name: 'GIN' },
];
const setup = (props = {}) => {
  const onOk = vi.fn();
  const onClose = vi.fn();
  render(<MultiSelectDialog title="Select [Multiple] Items" rows={ROWS} rowKey={r => r.code} onOk={onOk} onClose={onClose}
    columns={[{ key: 'code', label: 'ItemCode' }, { key: 'name', label: 'ItemName' }]}
    pasteBy={[{ label: 'ItemCode', value: r => r.code }, { label: 'ItemName', value: r => r.name }]} {...props} />);
  return { onOk, onClose };
};

describe('MultiSelectDialog', () => {
  it('ticks rows, filters, and Select All ticks only what the filter shows', async () => {
    const user = userEvent.setup();
    const { onOk } = setup();
    await user.click(screen.getByLabelText('Select A1'));
    await user.type(screen.getByLabelText('Filter'), 'u');                       // RUM only
    await user.click(screen.getByLabelText('Select All'));
    await user.click(screen.getByRole('button', { name: /^OK/ }));
    expect(onOk).toHaveBeenCalledWith(expect.arrayContaining(['A1', 'B2']));
    expect(onOk.mock.calls[0][0]).toHaveLength(2);
  });

  it('Paste ticks the rows matching the pasted column', async () => {
    const user = userEvent.setup();
    const { onOk } = setup();
    await user.click(screen.getByRole('button', { name: 'Paste' }));
    const paste = screen.getByRole('dialog', { name: 'Paste Data you want to Select' });
    await user.selectOptions(within(paste).getByRole('combobox'), 'ItemName');
    await user.type(within(paste).getByLabelText('Pasted values'), 'gin{Enter}rum{Enter}nothing');
    await user.click(within(paste).getByRole('button', { name: 'Ok' }));
    expect(screen.getByRole('status')).toHaveTextContent('2 ticked, 1 not found (nothing)');
    await user.click(screen.getByRole('button', { name: /^OK/ }));
    expect(onOk.mock.calls[0][0].sort()).toEqual(['B2', 'C3']);
  });

  it('starts with the given keys ticked, and Cancel and the X close it without choosing', async () => {
    const user = userEvent.setup();
    const { onOk, onClose } = setup({ chosen: ['C3'] });
    expect(screen.getByLabelText('Select C3')).toBeChecked();
    await user.click(screen.getByRole('button', { name: 'Cancel' }));
    await user.click(screen.getByRole('button', { name: 'Close' }));                // the X in the corner
    expect(onClose).toHaveBeenCalledTimes(2);
    expect(onOk).not.toHaveBeenCalled();
  });
});
