// Tests for SQL Query Execution: a query runs only after its waiting popup, F5 runs the selected text,
// rows and the server's error text are shown, nothing runs when the popup is cancelled, a change to a web
// table asks first and reports the rows affected, and without SQL Server nothing can run.
import { render, screen, waitFor } from '@testing-library/react'
import userEvent from '@testing-library/user-event'
import { afterEach, beforeEach, describe, expect, it, vi } from 'vitest'
import { toastStore } from '../common/toastStore'
import SqlQueryExecution from './SqlQueryExecution'

const json = (body, status = 200) =>
  new Response(JSON.stringify(body), { status, headers: { 'Content-Type': 'application/json' } })

let fetchMock
const bodies = () => fetchMock.mock.calls
  .filter(([url]) => String(url).endsWith('/tools/sql/execute'))
  .map(([, init]) => JSON.parse(String(init.body)))
const executed = () => bodies().map(b => b.sql)
const STATUS = { db_connected: true, database: 'vLQMSLHOUSE', driver: 'pymssql' }

beforeEach(() => {
  fetchMock = vi.fn(async (url, init) => {
    if (url.includes('/tools/sql/status')) return json(STATUS)
    if (url.includes('/tools/sql/objects')) return json([{ kind: 'Tables', schema: 'dbo', name: 'MstItem' }])
    if (url.endsWith('/tools/sql/execute')) {
      const sql = JSON.parse(String(init?.body)).sql 
      if (sql.includes('BOOM')) return json({ detail: "SQL Server: Invalid column name 'BOOM'." }, 400)
      if (/^update/i.test(sql)) return json({ kind: 'write', table: 'Web_DocHeader', rows_affected: 3, columns: [], rows: [], row_count: 0, truncated: false, elapsed_ms: 4 })
      return json({ columns: ['ItemCode', 'MRP'], rows: [['VIN-750', 850], ['RC-750', null]], row_count: 2, truncated: false, elapsed_ms: 12 })
    }
    return json({})
  })
  vi.stubGlobal('fetch', fetchMock)
  vi.spyOn(toastStore, 'defer').mockResolvedValue(true)
})

afterEach(() => { vi.unstubAllGlobals(); vi.restoreAllMocks() })

describe('SqlQueryExecution', () => {
  it('runs the query after its popup and shows the rows', async () => {
    const user = userEvent.setup()
    render(<SqlQueryExecution />)
    await screen.findByText('vLQMSLHOUSE')

    await user.type(screen.getByLabelText('SQL query'), 'SELECT ItemCode, MRP FROM MstItem')
    await user.click(screen.getByRole('button', { name: /execute query/i }))

    expect(await screen.findByText('VIN-750')).toBeInTheDocument()
    expect(screen.getByText('NULL')).toBeInTheDocument()
    expect(screen.getByText('Total no of rows : 2')).toBeInTheDocument()
    expect(toastStore.defer).toHaveBeenCalledWith('ok', 'Query will run when this closes.', expect.any(String))
    expect(executed()).toEqual(['SELECT ItemCode, MRP FROM MstItem'])
  })

  it('F5 runs only the selected text', async () => {
    const user = userEvent.setup()
    render(<SqlQueryExecution />)
    await screen.findByText('vLQMSLHOUSE')
    const box = screen.getByLabelText('SQL query') 
    await user.type(box, 'SELECT 1{enter}SELECT ItemCode FROM MstItem')

    box.setSelectionRange(9, box.value.length)
    await user.keyboard('{F5}')

    await waitFor(() => expect(executed()).toEqual(['SELECT ItemCode FROM MstItem']))
  })

  it('shows the SQL Server error under the query box', async () => {
    const user = userEvent.setup()
    render(<SqlQueryExecution />)
    await screen.findByText('vLQMSLHOUSE')

    await user.type(screen.getByLabelText('SQL query'), 'SELECT BOOM FROM MstItem')
    await user.click(screen.getByRole('button', { name: /execute query/i }))

    expect(await screen.findByRole('alert')).toHaveTextContent("Invalid column name 'BOOM'.")
  })

  it('runs nothing when the waiting popup is cancelled', async () => {
    vi.mocked(toastStore.defer).mockResolvedValue(false)
    const user = userEvent.setup()
    render(<SqlQueryExecution />)
    await screen.findByText('vLQMSLHOUSE')

    await user.type(screen.getByLabelText('SQL query'), 'SELECT 1')
    await user.click(screen.getByRole('button', { name: /execute query/i }))

    await waitFor(() => expect(toastStore.defer).toHaveBeenCalled())
    expect(executed()).toEqual([])
  })

  it('asks before a change to a web table and shows the rows affected', async () => {
    const user = userEvent.setup()
    render(<SqlQueryExecution />)
    await screen.findByText('vLQMSLHOUSE')

    await user.type(screen.getByLabelText('SQL query'), "UPDATE Web_DocHeader SET Status = 'X'")
    await user.click(screen.getByRole('button', { name: /execute query/i }))
    await user.click(await screen.findByRole('button', { name: 'Confirm' }))

    expect(await screen.findByText('3 row(s) affected in Web_DocHeader')).toBeInTheDocument()
    expect(toastStore.defer).toHaveBeenCalledWith('ok', 'Changes will be saved when this closes.', expect.any(String))
    expect(bodies()).toEqual([{ sql: "UPDATE Web_DocHeader SET Status = 'X'", target: 'sqlserver' }])
  })

  it('runs nothing while SQL Server is not connected', async () => {
    fetchMock.mockImplementation(async () => json({ ...STATUS, db_connected: false, database: null, driver: null }))
    render(<SqlQueryExecution />)

    expect(await screen.findByText(/SQL Server is not connected/)).toBeInTheDocument()
    expect(screen.getByRole('button', { name: /execute query/i })).toBeDisabled()
    expect(screen.queryByRole('radio')).toBeNull()   // no second (local) database any more
  })
})
