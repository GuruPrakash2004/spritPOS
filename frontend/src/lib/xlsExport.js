// Excel export without a dependency: writes a real .xlsx workbook (a zip of XML parts, stored without compression).
// Used by every list, report and import template that offers Export to Excel.

import { getCompany } from './api';

// Characters XML 1.0 can't carry are dropped.
const clean = (t) => Array.from(t).filter(ch => { const n = ch.charCodeAt(0); return n >= 32 || n === 9 || n === 10 || n === 13; }).join('');
const esc = (v) =>
  clean(String(v ?? '')).replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;').replace(/"/g, '&quot;');

// The shop's name for the first title row of every export (set once by App.jsx when the company profile loads).
let companyName = '';
const KEY = 'vlqms_export_company';
// Kept in sessionStorage too, so a reloaded module (or a screen opened before the profile arrived) still knows the name.
export const setExportCompany = (name) => {
  companyName = String(name || '').trim();
  if (companyName) { try { sessionStorage.setItem(KEY, companyName); } catch { /* storage blocked */ } }
};
const currentCompany = () => {
  if (companyName) return companyName;
  try { return sessionStorage.getItem(KEY) || ''; } catch { return ''; }
};

// ---- the zip container (method 0 = stored) ----
const CRC_TABLE = (() => {
  const t = new Uint32Array(256);
  for (let n = 0; n < 256; n++) { let c = n; for (let k = 0; k < 8; k++) c = c & 1 ? 0xEDB88320 ^ (c >>> 1) : c >>> 1; t[n] = c >>> 0; }
  return t;
})();
const crc32 = (bytes) => { let c = 0xFFFFFFFF; for (let i = 0; i < bytes.length; i++) c = CRC_TABLE[(c ^ bytes[i]) & 0xFF] ^ (c >>> 8); return (c ^ 0xFFFFFFFF) >>> 0; };

export function zipStore(files) {   // files: [[name, text]]
  const enc = new TextEncoder();
  const parts = [], central = [];
  let offset = 0;
  for (const [name, text] of files) {
    const n = enc.encode(name), d = enc.encode(text), crc = crc32(d);
    const local = new DataView(new ArrayBuffer(30));
    local.setUint32(0, 0x04034b50, true); local.setUint16(4, 20, true); local.setUint16(6, 0x0800, true); local.setUint16(8, 0, true);
    local.setUint16(10, 0, true); local.setUint16(12, 0x21, true); local.setUint32(14, crc, true); local.setUint32(18, d.length, true);
    local.setUint32(22, d.length, true); local.setUint16(26, n.length, true); local.setUint16(28, 0, true);
    parts.push(new Uint8Array(local.buffer), n, d);
    const c = new DataView(new ArrayBuffer(46));
    c.setUint32(0, 0x02014b50, true); c.setUint16(4, 20, true); c.setUint16(6, 20, true); c.setUint16(8, 0x0800, true); c.setUint16(10, 0, true);
    c.setUint16(12, 0, true); c.setUint16(14, 0x21, true); c.setUint32(16, crc, true); c.setUint32(20, d.length, true); c.setUint32(24, d.length, true);
    c.setUint16(28, n.length, true); c.setUint32(42, offset, true);
    central.push(new Uint8Array(c.buffer), n);
    offset += 30 + n.length + d.length;
  }
  const size = central.reduce((a, p) => a + p.length, 0);
  const end = new DataView(new ArrayBuffer(22));
  end.setUint32(0, 0x06054b50, true); end.setUint16(8, files.length, true); end.setUint16(10, files.length, true);
  end.setUint32(12, size, true); end.setUint32(16, offset, true);
  return new Blob([...parts, ...central, new Uint8Array(end.buffer)], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
}

// ---- the workbook ----
// House style of every export: row 1 the company name, row 2 the report title and period (both bold on cream, across the
// whole width), then a burgundy header row with white bold text, then the data. Style ids: 1 company, 2 subtitle, 3 header, 4 bold,
// 5 error cell (red fill, white bold text: a cell a check refused, see `marks` of downloadXls).
const STYLES = `<?xml version="1.0" encoding="UTF-8" standalone="yes"?>
<styleSheet xmlns="http://schemas.openxmlformats.org/spreadsheetml/2006/main">
<fonts count="5"><font><sz val="11"/><name val="Calibri"/></font><font><b/><sz val="16"/><name val="Calibri"/></font><font><b/><sz val="12"/><name val="Calibri"/></font><font><b/><sz val="11"/><color rgb="FFFFFFFF"/><name val="Calibri"/></font><font><b/><sz val="11"/><name val="Calibri"/></font></fonts>
<fills count="5"><fill><patternFill patternType="none"/></fill><fill><patternFill patternType="gray125"/></fill><fill><patternFill patternType="solid"><fgColor rgb="FFF5E9C8"/><bgColor indexed="64"/></patternFill></fill><fill><patternFill patternType="solid"><fgColor rgb="FF7B1E22"/><bgColor indexed="64"/></patternFill></fill><fill><patternFill patternType="solid"><fgColor rgb="FFFF0000"/><bgColor indexed="64"/></patternFill></fill></fills>
<borders count="1"><border><left/><right/><top/><bottom/><diagonal/></border></borders>
<cellStyleXfs count="1"><xf numFmtId="0" fontId="0" fillId="0" borderId="0"/></cellStyleXfs>
<cellXfs count="6"><xf numFmtId="0" fontId="0" fillId="0" borderId="0" xfId="0"/><xf numFmtId="0" fontId="1" fillId="2" borderId="0" xfId="0" applyFont="1" applyFill="1" applyAlignment="1"><alignment vertical="center"/></xf><xf numFmtId="0" fontId="2" fillId="2" borderId="0" xfId="0" applyFont="1" applyFill="1" applyAlignment="1"><alignment vertical="center"/></xf><xf numFmtId="0" fontId="3" fillId="3" borderId="0" xfId="0" applyFont="1" applyFill="1" applyAlignment="1"><alignment vertical="center"/></xf><xf numFmtId="0" fontId="4" fillId="0" borderId="0" xfId="0" applyFont="1"/><xf numFmtId="0" fontId="3" fillId="4" borderId="0" xfId="0" applyFont="1" applyFill="1"/></cellXfs>
<cellStyles count="1"><cellStyle name="Normal" xfId="0" builtinId="0"/></cellStyles>
</styleSheet>`;

const colName = (i) => { let s = ''; for (let n = i + 1; n > 0; n = Math.floor((n - 1) / 26)) s = String.fromCharCode(65 + ((n - 1) % 26)) + s; return s; };

const cell = (v, r, c, style) => {
  const ref = `${colName(c)}${r}`, s = style ? ` s="${style}"` : '';
  if (v === null || v === undefined || v === '') return style ? `<c r="${ref}"${s}/>` : '';
  if (typeof v === 'number' && Number.isFinite(v)) return `<c r="${ref}"${s}><v>${v}</v></c>`;
  return `<c r="${ref}"${s} t="inlineStr"><is><t xml:space="preserve">${esc(v)}</t></is></c>`;
};

// rows: [{ cells: [value], style?, cellStyles?: [style id per cell, 0 = the row's], height? }]; merges: ['A1:C1']; widths: characters per column; freezeRows: rows kept in view.
function workbook({ sheetName, rows, merges = [], widths = [], freezeRows = 0 }) {
  const data = rows.map((row, i) => {
    const r = i + 1, h = row.height ? ` ht="${row.height}" customHeight="1"` : '';
    return `<row r="${r}"${h}>${row.cells.map((v, c) => cell(v, r, c, row.cellStyles?.[c] || row.style)).join('')}</row>`;
  }).join('');
  const cols = widths.length ? `<cols>${widths.map((w, i) => `<col min="${i + 1}" max="${i + 1}" width="${w}" customWidth="1"/>`).join('')}</cols>` : '';
  const pane = freezeRows ? `<sheetViews><sheetView workbookViewId="0"><pane ySplit="${freezeRows}" topLeftCell="A${freezeRows + 1}" activePane="bottomLeft" state="frozen"/><selection pane="bottomLeft"/></sheetView></sheetViews>` : '';
  const merge = merges.length ? `<mergeCells count="${merges.length}">${merges.map(m => `<mergeCell ref="${m}"/>`).join('')}</mergeCells>` : '';
  const sheet = `<?xml version="1.0" encoding="UTF-8" standalone="yes"?>
<worksheet xmlns="http://schemas.openxmlformats.org/spreadsheetml/2006/main">${pane}<sheetFormatPr defaultRowHeight="15"/>${cols}<sheetData>${data}</sheetData>${merge}</worksheet>`;
  const X = '<?xml version="1.0" encoding="UTF-8" standalone="yes"?>\n';
  return zipStore([
    ['[Content_Types].xml', `${X}<Types xmlns="http://schemas.openxmlformats.org/package/2006/content-types"><Default Extension="rels" ContentType="application/vnd.openxmlformats-package.relationships+xml"/><Default Extension="xml" ContentType="application/xml"/><Override PartName="/xl/workbook.xml" ContentType="application/vnd.openxmlformats-officedocument.spreadsheetml.sheet.main+xml"/><Override PartName="/xl/worksheets/sheet1.xml" ContentType="application/vnd.openxmlformats-officedocument.spreadsheetml.worksheet+xml"/><Override PartName="/xl/styles.xml" ContentType="application/vnd.openxmlformats-officedocument.spreadsheetml.styles+xml"/></Types>`],
    ['_rels/.rels', `${X}<Relationships xmlns="http://schemas.openxmlformats.org/package/2006/relationships"><Relationship Id="rId1" Type="http://schemas.openxmlformats.org/officeDocument/2006/relationships/officeDocument" Target="xl/workbook.xml"/></Relationships>`],
    ['xl/workbook.xml', `${X}<workbook xmlns="http://schemas.openxmlformats.org/spreadsheetml/2006/main" xmlns:r="http://schemas.openxmlformats.org/officeDocument/2006/relationships"><sheets><sheet name="${esc(sheetName)}" sheetId="1" r:id="rId1"/></sheets></workbook>`],
    ['xl/_rels/workbook.xml.rels', `${X}<Relationships xmlns="http://schemas.openxmlformats.org/package/2006/relationships"><Relationship Id="rId1" Type="http://schemas.openxmlformats.org/officeDocument/2006/relationships/worksheet" Target="worksheets/sheet1.xml"/><Relationship Id="rId2" Type="http://schemas.openxmlformats.org/officeDocument/2006/relationships/styles" Target="styles.xml"/></Relationships>`],
    ['xl/styles.xml', STYLES],
    ['xl/worksheets/sheet1.xml', sheet],
  ]);
}

function save(blob, fileName) {
  const url = URL.createObjectURL(blob);
  const a = document.createElement('a');
  a.href = url;
  a.download = `${fileName.replace(/[^\w.-]+/g, '_')}.xlsx`;
  a.click();
  URL.revokeObjectURL(url);
}

/** `marks` (optional): for each row, the indexes (in `columns` order) of the cells to paint red, e.g. the cells an import check refused. */
export async function downloadXls(fileName, title, subtitle, columns, rows, marks = []) {
  let companyName = currentCompany();
  if (!companyName) {   // not known yet: ask the server (the company profile) so the name always heads the sheet
    try { setExportCompany((await getCompany())?.name); } catch { /* offline: the sheet goes without it */ }
    companyName = currentCompany();
  }
  const across = Math.max(1, columns.length);
  const span = (r) => (across > 1 ? [`A${r}:${colName(across - 1)}${r}`] : []);
  // A caller that already put the company in front of the title ("Shop - Report") keeps it in row 1 only.
  const plain = companyName && String(title).startsWith(`${companyName} - `) ? String(title).slice(companyName.length + 3) : String(title);
  const top = companyName
    ? [{ cells: [companyName], style: 1, height: 24 }, { cells: [plain + (subtitle ? ` (${subtitle})` : '')], style: 2, height: 24 }]
    : [{ cells: [plain], style: 1, height: 24 }, ...(subtitle ? [{ cells: [subtitle], style: 2, height: 24 }] : [])];
  const widths = columns.map(c => Math.min(Math.max(Math.max(String(c.label).length, ...rows.slice(0, 200).map(r => String(r[c.key] ?? '').length)), 8), 50) + 2);
  const all = [
    ...top,
    { cells: columns.map(c => c.label), style: 3, height: 22 },
    ...rows.map((r, i) => ({ cells: columns.map(c => r[c.key]), ...(marks[i]?.length ? { cellStyles: columns.map((_, c) => (marks[i].includes(c) ? 5 : 0)) } : {}) })),
  ];
  const merges = top.flatMap((_, i) => span(i + 1));
  save(workbook({ sheetName: 'Report', rows: all, merges, widths, freezeRows: top.length + 1 }), fileName);
}

/** A plain workbook of rows (no title lines; the header row, the first unless `headerRow` says otherwise, is bold), e.g. an import template. */
export function downloadXlsRows(fileName, rows, headerRow = 0) {
  const widths = (rows[0] || []).map((_, c) => Math.min(Math.max(...rows.slice(0, 200).map(r => String(r[c] ?? '').length), 8), 50) + 2);
  save(workbook({ sheetName: 'Sheet1', rows: rows.map((r, i) => ({ cells: r, style: i === headerRow ? 4 : 0 })), widths }), fileName);
}
