// downloadXls paints the marked cells red (style 5: red fill, white bold text) and leaves the others alone.
import { describe, expect, it, vi } from 'vitest'
import { downloadXls } from './xlsExport'

const text = (blob) => new Promise(res => { const f = new FileReader(); f.onload = () => res(String(f.result)); f.readAsText(blob) })

describe('downloadXls marks', () => {
  it('gives only the marked cells the red style', async () => {
    let blob
    vi.stubGlobal('URL', { createObjectURL: (b) => { blob = b; return 'blob:x' }, revokeObjectURL: () => {} })
    vi.spyOn(HTMLAnchorElement.prototype, 'click').mockImplementation(() => {})
    await downloadXls('t', 'Title', '', [{ key: '0', label: 'A' }, { key: '1', label: 'B' }, { key: '2', label: 'C' }],
      [['ok', 'bad', 'ok'], ['ok', 'ok', 'ok']], [[1], []])
    const xml = await text(blob)
    expect(xml).toContain('fgColor rgb="FFFF0000"')
    expect((xml.match(/ s="5"/g) ?? []).length).toBe(1)
    expect(xml).toMatch(/<c r="B\d+" s="5" t="inlineStr"><is><t xml:space="preserve">bad<\/t>/)
    vi.unstubAllGlobals(); vi.restoreAllMocks()
  })
})
