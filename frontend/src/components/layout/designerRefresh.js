// "Refresh from Doc Designer" action shared by the top-bar button and the user menu: asks through the waiting popup,
// then reloads the designer's tables on the server and redraws the screens.

import { errorText, reloadFromDocDesigner } from '../../lib/api';
import { forceDesignerReload } from '../../lib/designerStore';
import { toastStore } from '../common/toastStore';

export async function runDesignerRefresh() {
  if (!(await toastStore.defer('refresh', 'The Doc Designer settings will be read again when this closes.', 'Cancelled. Nothing was refreshed.'))) return;
  try {
    const state = await reloadFromDocDesigner();
    forceDesignerReload(state?.version, state?.build);
    toastStore.add('refresh', 'Refreshed from the Doc Designer.');
  } catch (e) {
    toastStore.add('error', errorText(e));
  }
}
