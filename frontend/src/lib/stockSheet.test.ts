// Tests for stock sheets (Compare Stock): CSV and Excel XML are read, the columns are found from the headers,
// .xlsx is refused with advice, and the comparison gives the desktop statuses.
import { describe, expect, it } from 'vitest'
import { compareSheets, guessColumns, readSheet, sheetItems } from './stockSheet'

const XML = `<?xml version="1.0"?><Workbook xmlns="urn:schemas-microsoft-com:office:spreadsheet" xmlns:ss="urn:schemas-microsoft-com:office:spreadsheet">
<Worksheet ss:Name="Report"><Table><Row><Cell><Data ss:Type="String">Stock Balance</Data></Cell></Row><Row/>
<Row><Cell><Data ss:Type="String">Item Code</Data></Cell><Cell><Data ss:Type="String">Item Name</Data></Cell><Cell><Data ss:Type="String">Op Qty</Data></Cell><Cell><Data ss:Type="String">Cl Qty</Data></Cell><Cell><Data ss:Type="String">Cl Litres</Data></Cell><Cell><Data ss:Type="String">Cl Value</Data></Cell></Row>
<Row><Cell><Data ss:Type="String">VIN-750</Data></Cell><Cell><Data ss:Type="String">Vintage</Data></Cell><Cell><Data ss:Type="Number">9</Data></Cell><Cell><Data ss:Type="Number">10</Data></Cell><Cell><Data ss:Type="Number">7.5</Data></Cell><Cell><Data ss:Type="Number">8000</Data></Cell></Row>
</Table></Worksheet></Workbook>`

describe('stock sheets', () => {
  it('reads Excel XML and finds the closing qty and value columns (not litres)', () => {
    const sheet = readSheet('old.xls', XML)
    const cols = guessColumns(sheet)
    expect(cols).toEqual({ header: 2, code: 0, name: 1, qty: 3, value: 5 })
    expect(sheetItems(sheet, cols)).toEqual([{ key: 'vin-750', code: 'VIN-750', name: 'Vintage', qty: 10, value: 8000 }])
  })

  it('reads CSV, skips total rows and adds up repeated items', () => {
    const sheet = readSheet('new.csv', 'ItemCode,ItemName,ClosingQty,Value\nVIN-750,Vintage,"1,000",10\nVIN-750,Vintage,5,1\nTotal,,1005,11\n')
    expect(sheetItems(sheet, guessColumns(sheet))).toEqual([{ key: 'vin-750', code: 'VIN-750', name: 'Vintage', qty: 1005, value: 11 }])
  })

  it('refuses .xlsx with advice', () => {
    expect(() => readSheet('stock.xlsx', 'PK\u0003\u0004...')).toThrow(/Save it as CSV/)
  })

  it('gives the desktop statuses', () => {
    const it = (code: string, qty: number, value: number) => ({ key: code.toLowerCase(), code, name: code, qty, value })
    const rows = compareSheets([it('A', 5, 50), it('B', 2, 20), it('C', 1, 10), it('D', 1, 10)], [it('A', 5, 50), it('B', 3, 30), it('C', 1, 12), it('E', 1, 1)])
    const by = Object.fromEntries(rows.map(r => [r.code, r]))
    expect(by.A.status).toBe('OK')
    expect(by.B).toMatchObject({ status: 'Qty Mismatch', qtyDiff: 1, valueDiff: 10 })
    expect(by.C.status).toBe('Value Mismatch')
    expect(by.D.status).toBe('Item Not found (New)')
    expect(by.E.status).toBe('Item Not found (Old)')
    expect(rows[rows.length - 1].status).toBe('OK')
  })
})
