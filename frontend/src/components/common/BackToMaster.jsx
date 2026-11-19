// "Back to Item Master": the one button every page opened from the Item Master's Options menu (Import Item from Excel, Update Stock from Excel,
// Item Vs Brand, Update Item Master Fields, Print Barcode) has in its header, to return to the master it came from.

import { ArrowLeft } from 'lucide-react';
import { openModule } from '../../lib/nav';
import { secondaryButton } from '../../lib/styles';

export default function BackToMaster({ master = 'Item Master' }) {
  return (
    <button type="button" onClick={() => openModule(master)} className={secondaryButton} title={`Back to ${master}`}>
      <ArrowLeft size={14} aria-hidden="true" />Back to {master}
    </button>
  );
}
