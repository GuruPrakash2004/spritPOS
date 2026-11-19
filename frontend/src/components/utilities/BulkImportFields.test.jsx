import { afterEach, describe, expect, it, vi } from 'vitest';
import { render, screen, waitFor } from '@testing-library/react';
import userEvent from '@testing-library/user-event';
import BulkImportFields from './BulkImportFields';
import { toastStore } from '../common/toastStore';
import * as api from '../../lib/api';

const DATA = { fields: [{ name: 'MRP', datatype: 'NUMERIC' }, { name: 'HSNCode', datatype: 'TEXT' }], columns: ['ItemCode', 'MRP', 'HSNCode', 'ROL'], types: ['Text', 'Integer', 'Date', 'Numeric'] };

afterEach(() => vi.restoreAllMocks());

describe('Bulk Item Import Fields', () => {
  it('lists the saved fields and saves an added one, replacing the list', async () => {
    vi.spyOn(api, 'getBulkImportFields').mockResolvedValue(DATA);
    const save = vi.spyOn(api, 'saveBulkImportFields').mockResolvedValue({ message: 'Saved successfully.', fields: [] });
    vi.spyOn(toastStore, 'defer').mockResolvedValue(true);
    render(<BulkImportFields />);
    expect(await screen.findByLabelText('Field name 1')).toHaveValue('MRP');
    expect(screen.getByLabelText('Data type 1')).toHaveValue('Numeric');   // stored in capitals, shown as the exe's choice
    expect(screen.getByRole('button', { name: 'Save' })).toBeDisabled();

    await userEvent.click(screen.getByRole('button', { name: /Add field/ }));
    await userEvent.selectOptions(screen.getByLabelText('Field name 3'), 'ROL');
    await userEvent.selectOptions(screen.getByLabelText('Data type 3'), 'Numeric');
    await userEvent.click(screen.getByRole('button', { name: 'Save' }));
    await waitFor(() => expect(save).toHaveBeenCalledWith([
      { name: 'MRP', datatype: 'Numeric' }, { name: 'HSNCode', datatype: 'Text' }, { name: 'ROL', datatype: 'Numeric' },
    ]));
  });

  it('refuses a duplicate field and a missing data type inline, sending nothing', async () => {
    vi.spyOn(api, 'getBulkImportFields').mockResolvedValue(DATA);
    const save = vi.spyOn(api, 'saveBulkImportFields').mockResolvedValue({});
    render(<BulkImportFields />);
    await screen.findByLabelText('Field name 1');
    await userEvent.click(screen.getByRole('button', { name: /Add field/ }));
    await userEvent.selectOptions(screen.getByLabelText('Field name 3'), 'MRP');
    await userEvent.click(screen.getByRole('button', { name: 'Save' }));
    expect(await screen.findByText("Duplicate field 'MRP'")).toBeInTheDocument();
    await userEvent.selectOptions(screen.getByLabelText('Field name 3'), 'ROL');
    await userEvent.click(screen.getByRole('button', { name: 'Save' }));
    expect(await screen.findByText('Select the Data Type of ROL')).toBeInTheDocument();
    expect(save).not.toHaveBeenCalled();
  });

  it('removing a row drops it from the saved list', async () => {
    vi.spyOn(api, 'getBulkImportFields').mockResolvedValue(DATA);
    const save = vi.spyOn(api, 'saveBulkImportFields').mockResolvedValue({ message: 'Saved successfully.' });
    vi.spyOn(toastStore, 'defer').mockResolvedValue(true);
    render(<BulkImportFields />);
    await screen.findByLabelText('Field name 1');
    await userEvent.click(screen.getByRole('button', { name: 'Remove row 1' }));
    await userEvent.click(screen.getByRole('button', { name: 'Save' }));
    await waitFor(() => expect(save).toHaveBeenCalledWith([{ name: 'HSNCode', datatype: 'Text' }]));
  });
});
