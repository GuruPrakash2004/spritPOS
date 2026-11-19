// Import Item Master Fields (the exe's frmUtlImportAnyFld) as a page: the three filter tick boxes open small "Select [Multiple] ..." popups,
// the chosen fields load into a grid, UOM is changed there and sent with Import Data, then the page returns to Item Master.

import { render, screen, waitFor, within } from '@testing-library/react';
import userEvent from '@testing-library/user-event';
import { afterEach, beforeEach, describe, expect, it, vi } from 'vitest';
import * as api from '../../lib/api';
import * as xls from '../../lib/xlsExport';
import { toastStore } from '../common/toastStore';
import { OPEN_MODULE } from '../../lib/nav';
import ImportItemMasterFields from './ImportItemMasterFields';

const MASTERS = {
  'item-groups': [{ code: 'WHISKY', active: true }, { code: 'OLD', active: false }],
  packages: [{ code: '750ML', litres_per_bottle: 0.75, active: true }, { code: '180ML', litres_per_bottle: 0.18, active: true }],
  uoms: [{ code: 'BTL', active: true }, { code: 'CASE', active: true }, { code: '-', active: true }],
};

beforeEach(() => {
  vi.spyOn(api, 'getBulkFieldDefs').mockResolvedValue([{ code: 'MRP', datatype: 'NUMERIC' }, { code: 'UOMID', datatype: 'INTEGER' }]);
  vi.spyOn(api, 'listMaster').mockImplementation(async (path) => MASTERS[path] ?? []);
  vi.spyOn(api, 'getItems').mockResolvedValue([{ code: 'A1', name: 'ITEM A1', active: true }, { code: 'Z9', name: 'GONE', active: false }]);
  vi.spyOn(toastStore, 'defer').mockResolvedValue(true);
});
afterEach(() => vi.restoreAllMocks());

describe('ImportItemMasterFields page', () => {
  it('opens the exe windows from the tick boxes: only active rows, Cancel unticks, Ok keeps the pick', async () => {
    const user = userEvent.setup();
    render(<ImportItemMasterFields />);

    await user.click(await screen.findByLabelText('Select Item Group'));
    let win = await screen.findByRole('dialog', { name: 'Select [Multiple] Item Groups' });
    expect(within(win).getByText('WHISKY')).toBeInTheDocument();
    expect(within(win).queryByText('OLD')).not.toBeInTheDocument();                        // inactive groups are not listed
    await user.click(within(win).getByRole('button', { name: 'Cancel' }));
    expect(screen.getByLabelText('Select Item Group')).not.toBeChecked();                   // nothing picked: the box unticks

    await user.click(await screen.findByLabelText('Select Item Packing'));
    win = await screen.findByRole('dialog', { name: 'Select [Multiple] Item Packaging' });
    const codes = within(win).getAllByRole('row').slice(1).map(r => r.cells[1].textContent);
    expect(codes).toEqual(['180ML', '750ML']);                                              // ordered by litres per bottle
    await user.click(within(win).getByLabelText('Select 750ML'));
    await user.click(within(win).getByRole('button', { name: /^OK/ }));
    expect(screen.getByLabelText(/Select Item Packing/)).toBeChecked();

    await user.click(await screen.findByLabelText('Select Item'));
    win = await screen.findByRole('dialog', { name: 'Select [Multiple] Item' });
    expect(within(win).getByText('ITEM A1')).toBeInTheDocument();
    expect(within(win).queryByText('GONE')).not.toBeInTheDocument();                        // inactive items are not listed
  });

  it('asks for a field before loading, then loads, changes UOM in the grid and imports it', async () => {
    const user = userEvent.setup();
    const load = vi.spyOn(api, 'loadBulkFields').mockResolvedValue({
      columns: ['ItemID', 'ItemCode', 'ItemName', 'UOMID'], rows: [{ ItemID: 7, ItemCode: 'A1', ItemName: 'ITEM A1', UOMID: 'BTL' }],
    });
    const imp = vi.spyOn(api, 'importBulkFields').mockResolvedValue({ updated: 1 });
    const opened = vi.fn();
    window.addEventListener(OPEN_MODULE, opened);
    render(<ImportItemMasterFields />);

    await user.click(await screen.findByRole('button', { name: 'Load to Grid' }));
    expect(screen.getByRole('status')).toHaveTextContent('Please Select atleast one field!');

    await user.click(await screen.findByLabelText('Import UOMID'));                          // UOM is always on the list
    await user.click(screen.getByRole('button', { name: 'Load to Grid' }));
    await waitFor(() => expect(load).toHaveBeenCalledWith({ fields: ['UOMID'], groups: [], packages: [], items: [] }));

    const uom = await screen.findByLabelText('UOM of ITEM A1');
    expect(within(uom).queryByRole('option', { name: '-' })).not.toBeInTheDocument();       // the '-' placeholder is not offered
    await user.selectOptions(uom, 'CASE');
    await user.click(screen.getByRole('button', { name: 'Import Data' }));
    await waitFor(() => expect(imp).toHaveBeenCalledWith([{ ItemID: 7, ItemCode: 'A1', ItemName: 'ITEM A1', UOMID: 'CASE' }]));
    await waitFor(() => expect(opened).toHaveBeenCalled());                                  // back to Item Master, as the exe closes the form
    expect(opened.mock.calls[0][0].detail).toBe('Item Master');
    window.removeEventListener(OPEN_MODULE, opened);
  });

  it('Generate Excel Template only makes the file and the notice: the grid fills only on Load to Grid', async () => {
    const user = userEvent.setup();
    const rows = { columns: ['ItemID', 'ItemCode', 'ItemName', 'MRP'], rows: [{ ItemID: 7, ItemCode: 'A1', ItemName: 'ITEM A1', MRP: 50 }] };
    const load = vi.spyOn(api, 'loadBulkFields').mockResolvedValue(rows);
    const sheet = vi.spyOn(xls, 'downloadXlsRows').mockImplementation(() => {});
    const toast = vi.spyOn(toastStore, 'add').mockImplementation(() => {});
    render(<ImportItemMasterFields />);

    await user.click(await screen.findByLabelText('Import MRP'));
    await user.click(screen.getByRole('button', { name: 'Generate Excel Template' }));
    await waitFor(() => expect(sheet).toHaveBeenCalledTimes(1));
    expect(sheet.mock.calls[0][1][0]).toEqual(['ItemID', 'Item Name', 'MRP']);                  // the template's header row
    expect(toast).toHaveBeenCalledWith('ok', 'Template Created');
    expect(screen.queryByLabelText('MRP of ITEM A1')).not.toBeInTheDocument();                    // nothing in the grid yet
    expect(screen.getByRole('button', { name: 'Import Data' })).toBeDisabled();

    await user.click(screen.getByRole('button', { name: 'Load to Grid' }));
    expect(await screen.findByLabelText('MRP of ITEM A1')).toHaveValue('50');                     // now it is
    expect(sheet).toHaveBeenCalledTimes(1);                                                         // and Load to Grid made no file
    expect(load).toHaveBeenCalledTimes(2);
  });
});
