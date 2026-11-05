// Which screens the desktop's menu (MstMenu) does not show (MenuShow = N): the sidebar leaves them out, like the desktop's
// own menu, and shows them again when the database says so. Matched by the screen's name or its menu caption
// (lib/menuAliases.js); a screen the menu doesn't mention stays listed.

import { useEffect, useState, useSyncExternalStore } from 'react';
import { getDesignerSnapshot, loadMenu, subscribeDesigner } from '../lib/designerStore';
import { hiddenModules } from '../lib/menuAliases';

export function useMenuHidden() {
  const { changes } = useSyncExternalStore(subscribeDesigner, getDesignerSnapshot);
  const [hidden, setHidden] = useState(() => new Set());
  useEffect(() => {
    let live = true;
    loadMenu().then(menu => {
      if (live) setHidden(hiddenModules(menu));
    });
    return () => { live = false; };
  }, [changes]);
  return hidden;
}
