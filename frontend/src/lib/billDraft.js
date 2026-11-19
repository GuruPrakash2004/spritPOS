// A purchase bill prepared by another screen and waiting for the Purchase Bill screen to take it (Update Item Stock: the stock sheet's items become the
// lines of an opening-stock bill). Nothing is saved when it is made: the user opens Purchase Bill, enters the supplier invoice no., date and value, and
// the bill is saved there when its total equals the invoice value, as every purchase bill. { supplier, remarks, lines: [{ item_code, qty, rate }] }.

let draft = null;

export const setOpeningDraft = (next) => { draft = next; };

/** The waiting draft (once): Purchase Bill takes it when it opens. */
export const takeOpeningDraft = () => { const d = draft; draft = null; return d; };
