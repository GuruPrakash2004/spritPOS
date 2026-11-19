// The Invoice Settings screen's own rules (frmUtlInvNumberFmt): the month prefix styles, the help table's headings and the number the 15 cells make.

export const MONTHS = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];
// fillDefMnthStyle
export const STYLES = { 'Style-1': 'ABCDEFGHIJKL'.split(''), 'Style-2': 'JFMAYULGSOND'.split(''), 'Style-3': '123456789OND'.split('') };
export const CHOICES = ['Save', 'Save and Print', 'Save and Preview'];
export const HEAD = ['Prefix', 'Counter', 'Year From', 'Year To', 'Sh YearFrom', 'Sh YearTo', 'DocNo', 'ShDocNo'];
export const SAMPLE_KEYS = ['prefix', 'counter', 'year_from', 'year_to', 'sh_year_from', 'sh_year_to', 'doc_no', 'sh_doc_no'];

/** SetDocInvFmt: a cell holding 1 to 8 gives that column of the help table's sample row, any other cell is taken as typed. */
export function previewNumber(cells, sample) {
  return cells.map(c => {
    const n = Number.parseFloat(c);
    return Number.isInteger(n) && n >= 1 && n <= 8 ? String(sample?.[SAMPLE_KEYS[n - 1]] ?? '') : c;
  }).join('');
}
