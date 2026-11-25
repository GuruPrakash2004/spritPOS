// Stock Balance screen: the "Select [Multiple] ..." windows, the rate radio buttons, and the big result window that View opens.
import { fireEvent, render, screen, waitFor, within } from '@testing-library/react';
import userEvent from '@testing-library/user-event';
import { beforeEach, describe, expect, it, vi } from 'vitest';
import * as api from '../../lib/api';
import StockBalanceReport from './StockBalanceReport';

const view = {
  columns: [{ key: 'group', label: 'Item Group Name' }, { key: 'item_name', label: 'Item Name' }, { key: 'op', label: 'Opening Quantity', kind: 'number' }, { key: 'cl', label: 'Closing Quantity', kind: 'number' }],
  rows: [{ group: '1 BRANDY', item_name: 'A', op: 5, cl: 5 }, { group: '1 BRANDY', item_name: 'B', op: 0, cl: 2 }],
};

describe('StockBalanceReport', () => {
  beforeEach(() => {
    vi.spyOn(api, 'getItemGroups').mockResolvedValue([
      { code: '1 BRANDY', name: '1 BRANDY', active: true }, { code: '2 WHISKY', name: '2 WHISKY', active: true }, { code: 'OLD', name: 'OLD', active: false },
    ]);
    vi.spyOn(api, 'getMasterOrder').mockResolvedValue([
      { id: 2, slno: 1, code: '2 WHISKY' }, { id: 1, slno: 2, code: '1 BRANDY' }, { id: 3, slno: 3, code: 'OLD' },
    ]);
    vi.spyOn(api, 'getItems').mockResolvedValue([
      { code: 'I2', name: 'RUM 750', package: '750ML', active: true }, { code: 'I1', name: 'GIN 180', package: '180ML', active: true }, { code: 'I3', name: 'DEAD', package: '1L', active: false },
    ]);
    vi.spyOn(api, 'getStockBalanceFormats').mockResolvedValue({ formats: ['RptStkBal.rpt'] });
    vi.spyOn(api, 'getStockBalancePackages').mockResolvedValue(['BURNETTS GIN', 'ROYAL STAG']);
    vi.spyOn(api, 'renderStockBalancePrint').mockResolvedValue(new Blob(['pdf']));
    vi.spyOn(api, 'getCompany').mockResolvedValue({ name: 'SHOP' });
    vi.spyOn(api, 'getStockBalanceView').mockResolvedValue(view);
  });

  it('lists the active groups in database order in the popup and shows the pick', async () => {
    const user = userEvent.setup();
    render(<StockBalanceReport />);
    await user.click(await screen.findByLabelText('Selected Group'));
    const dialog = await screen.findByRole('dialog');
    await waitFor(() => expect(within(dialog).getAllByRole('row').length).toBe(3));
    const rows = within(dialog).getAllByRole('row');
    expect(rows[1]).toHaveTextContent('2 WHISKY');
    expect(rows[2]).toHaveTextContent('1 BRANDY');
    expect(within(dialog).queryByText('OLD')).toBeNull();
    await user.click(within(dialog).getByLabelText('Select 1 BRANDY'));
    await user.click(within(dialog).getByRole('button', { name: /OK/ }));
    expect(screen.getByLabelText('Selected Group')).toHaveTextContent('1 BRANDY');
  });

  it('Select Item opens a multi-select window, active rows only; there is no Select Package', async () => {
    const user = userEvent.setup();
    render(<StockBalanceReport />);
    await user.click(await screen.findByLabelText('Select Item'));
    let dialog = await screen.findByRole('dialog');
    await waitFor(() => expect(within(dialog).getAllByRole('row').length).toBe(3));
    expect(within(dialog).getAllByRole('row')[1]).toHaveTextContent('I1');
    expect(within(dialog).queryByText('DEAD')).toBeNull();
    await user.click(within(dialog).getByLabelText('Select I2'));
    await user.click(within(dialog).getByRole('button', { name: /OK/ }));
    expect(screen.getByLabelText('Select Item')).toHaveTextContent('RUM 750');
    expect(within(screen.getByRole('region', { name: 'Selected Item' })).getByText('RUM 750')).toBeInTheDocument();

    expect(screen.queryByLabelText('Select Package')).toBeNull();
  });

  it('the rate is radio buttons: FIFO and Wt.Avg Rate appear only with Show Value, With Tax only with Last Purchase Rate', async () => {
    const user = userEvent.setup();
    render(<StockBalanceReport />);
    expect(screen.getByRole('radio', { name: 'Last Purchase Rate' })).toBeChecked();
    expect(screen.getByRole('radio', { name: 'Sales Rate' })).toBeInTheDocument();
    expect(screen.queryByRole('radio', { name: 'FIFO' })).toBeNull();
    expect(screen.queryByRole('radio', { name: 'Wt.Avg Rate' })).toBeNull();
    expect(screen.getByLabelText('With Tax')).toBeInTheDocument();
    await user.click(screen.getByLabelText('Show Value'));   // Closing Balance Only
    expect(screen.getByRole('radio', { name: 'FIFO' })).toBeInTheDocument();
    await user.click(screen.getByRole('radio', { name: 'Wt.Avg Rate' }));
    expect(screen.getByRole('radio', { name: 'Wt.Avg Rate' })).toBeChecked();
    expect(screen.queryByLabelText('With Tax')).toBeNull();
    await user.click(screen.getByLabelText('Show Value'));   // Show Value off again: back to Last Purchase Rate
    expect(screen.getByRole('radio', { name: 'Last Purchase Rate' })).toBeChecked();
    expect(screen.queryByRole('radio', { name: 'Wt.Avg Rate' })).toBeNull();
    expect(screen.getByLabelText('With Tax')).toBeInTheDocument();
  });

  it('View opens a big window with the grid (with Opening Quantity), Export, Col Select, Col Freeze and Print, and no Copy', async () => {
    const user = userEvent.setup();
    render(<StockBalanceReport />);
    await user.click(screen.getByRole('button', { name: /^View/ }));
    const dialog = await screen.findByRole('dialog');
    await within(dialog).findByText('Total Records: 2');
    expect(within(dialog).getAllByRole('table')).toHaveLength(1);
    expect(within(dialog).getAllByText('1 BRANDY')).toHaveLength(1);   // the group name once, merged down its rows
    expect(within(dialog).getByText('Opening Quantity')).toBeInTheDocument();
    for (const name of [/Export Excel/, /Col Select/, /Col Freeze/, /^Print$/, /Print Zero Stock Items/]) expect(within(dialog).getByRole('button', { name })).toBeInTheDocument();
    expect(within(dialog).queryByRole('button', { name: /^Copy/ })).toBeNull();
    await user.click(within(dialog).getByRole('button', { name: 'Col Freeze' }));
    expect(within(dialog).getByRole('button', { name: 'Col UnFreeze' })).toBeInTheDocument();
    await user.click(within(dialog).getByRole('button', { name: /Col Select/ }));
    expect(await screen.findAllByRole('dialog')).toHaveLength(2);
  });

  it('every heading has a filter button: tick a value (or give a condition) and Apply hides the other rows; Clear brings them back', async () => {
    const user = userEvent.setup();
    render(<StockBalanceReport />);
    await user.click(screen.getByRole('button', { name: /^View/ }));
    const dialog = await screen.findByRole('dialog');
    await within(dialog).findByText('Total Records: 2');
    await user.click(within(dialog).getByRole('button', { name: 'Filter Item Name' }));
    const box = within(dialog).getByRole('group', { name: 'Filter Item Name' });
    await user.click(within(box).getByLabelText('B'));
    await user.click(within(box).getByRole('button', { name: 'Apply' }));
    expect(await within(dialog).findByText('Total Records: 1')).toBeInTheDocument();
    await user.click(within(dialog).getByRole('button', { name: 'Filter Item Name' }));
    await user.click(within(screen.getByRole('group', { name: 'Filter Item Name' })).getByRole('button', { name: 'Clear' }));
    expect(await within(dialog).findByText('Total Records: 2')).toBeInTheDocument();
  });

  it('sends the chosen rate and the AROED / Actual Sales options for the detailed view', async () => {
    const user = userEvent.setup();
    render(<StockBalanceReport />);
    await user.selectOptions(screen.getByLabelText('View'), 'detailed');
    await user.click(screen.getByLabelText('Show Value'));
    await user.click(screen.getByRole('radio', { name: 'FIFO' }));
    await user.click(screen.getByLabelText('Actual Sales Value'));
    await user.click(screen.getByLabelText('AROED Value'));
    await user.click(screen.getByRole('button', { name: /^View/ }));
    await waitFor(() => expect(api.getStockBalanceView).toHaveBeenLastCalledWith(expect.objectContaining({ view: 'detailed', values: true, rate: 'fifo', act_sales: true, aroed: true })));
  });

  it('a column can be resized by dragging the edge of its heading, and double-click puts the widths back', async () => {
    const user = userEvent.setup();
    render(<StockBalanceReport />);
    await user.click(screen.getByRole('button', { name: /^View/ }));
    const dialog = await screen.findByRole('dialog');
    await within(dialog).findByText('Total Records: 2');
    const table = within(dialog).getByRole('table');
    expect(table.style.tableLayout).toBe('');
    const edge = within(dialog).getByRole('separator', { name: 'Resize Item Name' });
    fireEvent.mouseDown(edge, { clientX: 100 });
    fireEvent.mouseMove(window, { clientX: 160 });
    fireEvent.mouseUp(window);
    expect(table.style.tableLayout).toBe('fixed');
    fireEvent.doubleClick(edge);
    expect(table.style.tableLayout).toBe('');
  });

  it('Print in the result window opens the Crystal print window for the loaded period and picks', async () => {
    const user = userEvent.setup();
    render(<StockBalanceReport />);
    await user.click(await screen.findByLabelText('Selected Group'));
    let dialog = await screen.findByRole('dialog');
    await waitFor(() => expect(within(dialog).getAllByRole('row').length).toBe(3));
    await user.click(within(dialog).getByLabelText('Select 1 BRANDY'));
    await user.click(within(dialog).getByRole('button', { name: /OK/ }));
    await user.click(screen.getByRole('button', { name: /^View/ }));
    dialog = await screen.findByRole('dialog');
    await within(dialog).findByText('Total Records: 2');
    await user.click(within(dialog).getByRole('button', { name: /^Print$/ }));
    expect(screen.queryByRole('button', { name: /Print directly/ })).toBeNull();   // one Print button here, and it prints on the chosen printer
    await waitFor(() => expect(api.renderStockBalancePrint).toHaveBeenCalledWith(expect.any(String), expect.any(String), '', 'RptStkBal.rpt', undefined, expect.objectContaining({ groups: '1 BRANDY' })));
  });
});
