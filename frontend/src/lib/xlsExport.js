// Excel export without a dependency: writes SpreadsheetML 2003 XML, which Excel and LibreOffice open
// as a .xls workbook. Used by Customised Reports → Export XLS.

 


import { getCompany } from './api';

const esc = (v) =>
  String(v ?? '').replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;').replace(/"/g, '&quot;');

const cell = (v, style) => {
  const s = style ? ` ss:StyleID="${style}"` : '';
  return typeof v === 'number'
    ? `<Cell${s}><Data ss:Type="Number">${v}</Data></Cell>`
    : `<Cell${s}><Data ss:Type="String">${esc(v)}</Data></Cell>`;
};

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

// House style of every export: row 1 the company name, row 2 the report title and period (both bold on cream, across the
// whole width), then a burgundy header row with white bold text, then the data. Column widths follow the longest text.
const STYLES = `<Styles>
<Style ss:ID="co"><Font ss:FontName="Calibri" ss:Bold="1" ss:Size="16"/><Interior ss:Color="#F5E9C8" ss:Pattern="Solid"/><Alignment ss:Vertical="Center"/></Style>
<Style ss:ID="sub"><Font ss:FontName="Calibri" ss:Bold="1" ss:Size="12"/><Interior ss:Color="#F5E9C8" ss:Pattern="Solid"/><Alignment ss:Vertical="Center"/></Style>
<Style ss:ID="h"><Font ss:FontName="Calibri" ss:Bold="1" ss:Color="#FFFFFF"/><Interior ss:Color="#7B1E22" ss:Pattern="Solid"/><Alignment ss:Vertical="Center"/></Style>
</Styles>`;

const wide = (text, across, style) => `<Row ss:Height="24"><Cell ss:MergeAcross="${Math.max(0, across - 1)}" ss:StyleID="${style}"><Data ss:Type="String">${esc(text)}</Data></Cell></Row>`;

export async function downloadXls(fileName, title, subtitle, columns, rows) {
  let companyName = currentCompany();
  if (!companyName) {   // not known yet: ask the server (the company profile) so the name always heads the sheet
    try { setExportCompany((await getCompany())?.name); } catch { /* offline: the sheet goes without it */ }
    companyName = currentCompany();
  }
  const across = Math.max(1, columns.length);
  // A caller that already put the company in front of the title ("Shop - Report") keeps it in row 1 only.
  const plain = companyName && String(title).startsWith(`${companyName} - `) ? String(title).slice(companyName.length + 3) : String(title);
  const titleRows = (companyName || subtitle ? 2 : 1) + 1;   // title rows + the header row, kept in view while scrolling (Freeze Panes)
  const top = companyName
    ? wide(companyName, across, 'co') + wide(plain + (subtitle ? ` (${subtitle})` : ''), across, 'sub')
    : wide(plain, across, 'co') + (subtitle ? wide(subtitle, across, 'sub') : '');
  const widths = columns.map(c => {
    const longest = Math.max(String(c.label).length, ...rows.slice(0, 200).map(r => String(r[c.key] ?? '').length));
    return `<Column ss:Width="${Math.min(Math.max(longest, 8), 50) * 6.5 + 12}"/>`;
  }).join('');
  const xml = `<?xml version="1.0"?>
<Workbook xmlns="urn:schemas-microsoft-com:office:spreadsheet" xmlns:ss="urn:schemas-microsoft-com:office:spreadsheet">
${STYLES}
<Worksheet ss:Name="Report"><Table>
${widths}
${top}
<Row ss:Height="22">${columns.map(c => cell(c.label, 'h')).join('')}</Row>
${rows.map(r => `<Row>${columns.map(c => cell(r[c.key])).join('')}</Row>`).join('\n')}
</Table>
<WorksheetOptions xmlns="urn:schemas-microsoft-com:office:excel"><FreezePanes/><FrozenNoSplit/><SplitHorizontal>${titleRows}</SplitHorizontal><TopRowBottomPane>${titleRows}</TopRowBottomPane><ActivePane>2</ActivePane></WorksheetOptions>
</Worksheet></Workbook>`;
  const url = URL.createObjectURL(new Blob([xml], { type: 'application/vnd.ms-excel' }));
  const a = document.createElement('a');
  a.href = url;
  a.download = `${fileName.replace(/[^\w.-]+/g, '_')}.xls`;
  a.click();
  URL.revokeObjectURL(url);
}

/** A plain workbook of rows (no title lines; the first row is bold), e.g. an import template: row 1 is the header an import reads back. */
export function downloadXlsRows(fileName, rows) {
  const xml = `<?xml version="1.0"?>
<Workbook xmlns="urn:schemas-microsoft-com:office:spreadsheet" xmlns:ss="urn:schemas-microsoft-com:office:spreadsheet">
<Styles><Style ss:ID="h"><Font ss:Bold="1"/></Style></Styles>
<Worksheet ss:Name="Sheet1"><Table>
${rows.map((r, i) => `<Row>${r.map(v => cell(v, i === 0 ? 'h' : undefined)).join('')}</Row>`).join('\n')}
</Table></Worksheet></Workbook>`;
  const url = URL.createObjectURL(new Blob([xml], { type: 'application/vnd.ms-excel' }));
  const a = document.createElement('a');
  a.href = url;
  a.download = `${fileName.replace(/[^\w.-]+/g, '_')}.xls`;
  a.click();
  URL.revokeObjectURL(url);
}
