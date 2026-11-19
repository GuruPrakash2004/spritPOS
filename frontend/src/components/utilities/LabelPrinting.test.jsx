// Label Printing grid: the label count follows the Qty typed for an item, and No of Labels can still be changed afterwards.

import { fireEvent, render, screen, waitFor } from '@testing-library/react';
import userEvent from '@testing-library/user-event';
import { afterEach, describe, expect, it, vi } from 'vitest';
import * as api from '../../lib/api';
import LabelPrinting from './LabelPrinting';

describe('LabelPrinting', () => {
  afterEach(() => vi.restoreAllMocks());

  it('sets the number of labels from the Qty typed, and lets No of Labels be changed after', async () => {
    vi.spyOn(api, 'getItems').mockResolvedValue([{ code: '012', name: 'MCDOWELLS WHISKY 90ML', uom: 'Bottle', mrp: 94 }]);
    vi.spyOn(api, 'getBarcodeFormats').mockResolvedValue({ formats: ['RPTBC.rpt'] });
    render(<LabelPrinting />);

    await userEvent.type(await screen.findByLabelText(/add an item/i), '012{Enter}');
    const labels = await screen.findByLabelText('Labels for 012');
    expect(labels).toHaveValue(1);

    fireEvent.change(screen.getByLabelText('Qty for 012'), { target: { value: '6' } });
    await waitFor(() => expect(screen.getByLabelText('Labels for 012')).toHaveValue(6));

    fireEvent.change(screen.getByLabelText('Labels for 012'), { target: { value: '2' } });
    expect(screen.getByLabelText('Qty for 012')).toHaveValue(6);   // Qty stays what was typed
    expect(screen.getByLabelText('Labels for 012')).toHaveValue(2);
  });
});
