import { render, screen } from '@testing-library/react';
import userEvent from '@testing-library/user-event';
import { afterEach, describe, expect, it, vi } from 'vitest';
import ScreenPreview from './ScreenPreview';

const data = {
  version: 'v1',
  screens: [
    { doc_id: 1, name: 'Item', type: 'Master', short_form: 'ITEM', header_table: 'MstITEM', detail_table: '', master_path: 'items',
      designer_fields: 2, shown_fields: 2, problems: [], fields: [{ code: 'ITEMNAME', caption: 'Name', section: 'Header', sub_section: '', type: 'text', width: 60, mandatory: true }] },
    { doc_id: 2, name: 'Bank', type: 'Master', short_form: 'BANK', header_table: 'MstBANK', detail_table: '', master_path: '',
      designer_fields: 1, shown_fields: 0, problems: [{ level: 'warn', field: 'GONE', text: 'The column is not in the table any more, so the field is left out.' }], fields: [] },
  ],
};

afterEach(() => vi.unstubAllGlobals());

describe('ScreenPreview', () => {
  it('lists the screens, filters to the ones with problems and shows the details', async () => {
    vi.stubGlobal('fetch', vi.fn(async () => new Response(JSON.stringify(data), { status: 200, headers: { 'Content-Type': 'application/json' } })));
    const user = userEvent.setup();
    render(<ScreenPreview />);
    expect(await screen.findByText('Item')).toBeInTheDocument();
    expect(screen.getByText('Bank')).toBeInTheDocument();

    await user.selectOptions(screen.getByLabelText('Show'), 'problems');
    expect(screen.queryByText('Item')).not.toBeInTheDocument();

    await user.click(screen.getByText('Bank'));
    expect(await screen.findByText(/not in the table any more/)).toBeInTheDocument();
  });

  it('marks a screen without problems as clean', async () => {
    vi.stubGlobal('fetch', vi.fn(async () => new Response(JSON.stringify(data), { status: 200, headers: { 'Content-Type': 'application/json' } })));
    render(<ScreenPreview />);
    expect(await screen.findByText('clean')).toBeInTheDocument();
    expect(screen.getAllByText('1 warnings')).toHaveLength(2);   // the total and the screen's own badge
  });
});
