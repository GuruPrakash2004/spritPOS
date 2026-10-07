// Excel export without a dependency: writes SpreadsheetML 2003 XML, which Excel and LibreOffice open
// as a .xls workbook. Used by Customised Reports → Export XLS.

 


const esc = (v) =>
  String(v ?? '').replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;').replace(/"/g, '&quot;');

const cell = (v, style) => {
  const s = style ? ` ss:StyleID="${style}"` : '';
  return typeof v === 'number'
    ? `<Cell${s}><Data ss:Type="Number">${v}</Data></Cell>`
    : `<Cell${s}><Data ss:Type="String">${esc(v)}</Data></Cell>`;
};

export function downloadXls(fileName, title, subtitle, columns, rows) {
  const xml = `<?xml version="1.0"?>
<Workbook xmlns="urn:schemas-microsoft-com:office:spreadsheet" xmlns:ss="urn:schemas-microsoft-com:office:spreadsheet">
<Styles><Style ss:ID="h"><Font ss:Bold="1"/></Style><Style ss:ID="t"><Font ss:Bold="1" ss:Size="14"/></Style></Styles>
<Worksheet ss:Name="Report"><Table>
<Row>${cell(title, 't')}</Row>
<Row>${cell(subtitle)}</Row>
<Row/>
<Row>${columns.map(c => cell(c.label, 'h')).join('')}</Row>
${rows.map(r => `<Row>${columns.map(c => cell(r[c.key])).join('')}</Row>`).join('\n')}
</Table></Worksheet></Workbook>`;
  const url = URL.createObjectURL(new Blob([xml], { type: 'application/vnd.ms-excel' }));
  const a = document.createElement('a');
  a.href = url;
  a.download = `${fileName.replace(/[^\w.-]+/g, '_')}.xls`;
  a.click();
  URL.revokeObjectURL(url);
}
