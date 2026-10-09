// Which screens the desktop's menu (MstMenu) does not show (MenuShow = N): the sidebar leaves them out, like the desktop's
// own menu, and shows them again when the database says so. Matched by the screen's name (MenuItemCaption); a screen
// the menu doesn't mention stays listed.

import { useEffect, useState, useSyncExternalStore } from 'react';
import { getDesignerSnapshot, loadMenu, subscribeDesigner } from '../lib/designerStore';

export function useMenuHidden() {
  const { changes } = useSyncExternalStore(subscribeDesigner, getDesignerSnapshot);
  const [hidden, setHidden] = useState(() => new Set());
  useEffect(() => {
    let live = true;
    loadMenu().then(menu => {
      if (live) setHidden(new Set(menu.filter(m => !m.show && m.doc_id > 0 && m.caption).map(m => m.caption.trim().toLowerCase())));
    });
    return () => { live = false; };
  }, [changes]);
  return hidden;
}
