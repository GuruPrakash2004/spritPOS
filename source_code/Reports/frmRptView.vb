Imports System.Data.oledb
Imports System.IO

Public Class frmRptView
    Dim clsSVT As New svtLibrary
    Dim strSQL As String
    Dim argCForm As Object
    Dim dt As DataTable
    Dim dv As DataView
    Public blnDirectExport As Boolean = False
    Public Sub FillViewVsf(ByVal argFrm As Form, ByVal argSQL As String, ByVal argHdrLine1 As String, ByVal argHdrLine2 As String, Optional ByVal argConn As oledbConnection = Nothing, Optional ByVal argblnSlno As Boolean = False)
        strSQL = argSQL
        Cursor.Current = Cursors.WaitCursor
        dt = clsSVT.getDATATable(strSQL, argConn)
        dv = New DataView(dt)

        vsfDet.Rows.Fixed = 4
        vsfDet.Rows.GlyphRow = 3
        vsfDet.BeginUpdate()

        vsfDet.DataSource = dv
        Cursor.Current = Cursors.Arrow
        argCForm = argFrm


        vsfDet.EndUpdate()
        vsfSearch.Cols.Count = vsfDet.Cols.Count
        If argblnSlno Then
            vsfDet.SetData(3, 0, "SlNo")
            For j = 4 To vsfDet.Rows.Count - 1
                vsfDet.SetData(j, 0, j - 3)
            Next j
        End If

        Call syncGridCols()
        Call initVsfForHdrs(argHdrLine1, argHdrLine2)
    End Sub
    Public Sub syncGridCols()
        Dim i As Integer
        With vsfDet
            vsfSearch.Cols.Count = vsfDet.Cols.Count
            For i = 0 To .Cols.Count - 1
                vsfSearch.Cols(i).Width = vsfDet.Cols(i).Width
                vsfSearch.Cols(i).Visible = vsfDet.Cols(i).Visible
            Next i
        End With
    End Sub
    Private Sub initVsfForHdrs(ByVal argHdrLine1 As String, ByVal argHdrLine2 As String)
        Dim i As Integer

        With vsfDet
            For i = 0 To .Cols.Count - 1
                '.SetData(3, i, .GetData(0, i))
                .SetData(1, i, gSettings.gCompName & " - " & argHdrLine1)
                .SetData(2, i, argHdrLine2)
            Next
            Dim csf1 As C1.Win.C1FlexGrid.CellStyle = vsfDet.Styles.Add("F1")
            csf1.BackColor = Color.Beige

            .Rows(0).Style = csf1
            .Rows(1).Style = csf1
            .Rows(2).Style = csf1
            .Rows(0).Visible = False
            'If argHdrLine1 = "" Then .Rows(1).Visible = False
            If argHdrLine2 = "" Then .Rows(2).Visible = False

            .Rows(0).AllowMerging = True
            .Rows(1).AllowMerging = True
            .Rows(2).AllowMerging = True
            '.Rows(3).AllowMerging = True

        End With
    End Sub

    Public Sub ExportToXlPDF(ByVal argXLName As String, ByVal argPDFName As String)
        vsfDet.SaveExcel(argXLName, C1.Win.C1FlexGrid.FileFlags.AsDisplayed Or C1.Win.C1FlexGrid.FileFlags.IncludeMergedRanges Or C1.Win.C1FlexGrid.FileFlags.IncludeFixedCells)
    End Sub

    Private Sub btnExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click

        Call ExportToXL(vsfDet)


    End Sub

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub btnFreeze_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFreeze.Click
        If btnFreeze.Text = "&Freeze Columns" Then
            btnFreeze.Text = "&UnFreeze Columns"
            vsfDet.Cols.Frozen = vsfDet.Col + 1
            vsfDet.Cols.Frozen = vsfDet.Col + 1
        Else
            btnFreeze.Text = "&Freeze Columns"
            vsfDet.Cols.Frozen = 0
            vsfDet.Cols.Frozen = 0
        End If
    End Sub

    Private Sub vsfDet_AfterResizeColumn(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfDet.AfterResizeColumn
        Call syncGridCols()
    End Sub

    Private Sub vsfDet_BeforeEdit(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfDet.BeforeEdit
        e.Cancel = True
    End Sub

    Private Sub btnExpCSV_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExpCSV.Click
        Call ExportToXL(vsfDet, "CSV")
    End Sub

    Private Sub vsfDet_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles vsfDet.DoubleClick
        'Call argCForm.FillDocDetails(vsfDet.GetData(vsfDet.Row, 0))
        'If chkRemoveSubTot.Checked = False Then Me.Close()
    End Sub

    Private Sub btnExportPDF_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        'Dim strFile As String
        'Try
        '    Dim pdfCreator As New GridToPdf(vsfDet)
        '    strFile = Application.StartupPath & "\Temp\PDFExport" & Date.Now.Ticks & ".pdf"
        '    pdfCreator.Save(strFile)

        '    Dim p As New System.Diagnostics.Process
        '    Dim s As New System.Diagnostics.ProcessStartInfo(strFile)
        '    s.UseShellExecute = True
        '    s.WindowStyle = ProcessWindowStyle.Normal
        '    p.StartInfo = s
        '    p.Start()
        'Catch ex As Exception
        '    MsgBox("Cannot export to Pdf." & vbCrLf & ex.Message, MsgBoxStyle.Critical, Me.Text)
        'End Try
    End Sub

    Private Sub vsfDet_AfterScroll(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.RangeEventArgs) Handles vsfDet.AfterScroll
        Call syncGridCols()
        vsfSearch.LeftCol = vsfDet.LeftCol
    End Sub

    Private Sub vsfSearch_AfterScroll(sender As Object, e As C1.Win.C1FlexGrid.RangeEventArgs) Handles vsfSearch.AfterScroll
        Call syncGridCols()
        vsfDet.LeftCol = vsfSearch.LeftCol
    End Sub

    Private Sub vsfSearch_ChangeEdit(ByVal sender As Object, ByVal e As System.EventArgs) Handles vsfSearch.ChangeEdit
        Call applyFilterFromGrid()
    End Sub

    Private Function applyFilterFromGrid() As Boolean
        Dim StrQry As String, strValue As String
        Dim i As Integer

        Try
            applyFilterFromGrid = False
            StrQry = ""
            With vsfSearch
                For i = 0 To .Cols.Count - 1
                    strValue = ""
                    If i = .ColSel Then
                        If .Editor.Text <> "" Then
                            strValue = Trim(.Editor.Text)
                        End If
                    Else
                        If .GetData(0, i) <> "" Then
                            strValue = Trim(.GetData(0, i))
                        End If
                    End If
                    If strValue <> "" Then
                        If StrQry <> "" Then StrQry = StrQry & " and "
                        StrQry = StrQry & "[" & vsfDet.GetData(0, i) & "] like '%" & strValue & "%'"
                    End If
                Next i
            End With

            'If StrQry <> "" Then
            '    vsfDet.DataSource = dt.Select(StrQry)
            'Else
            '    vsfDet.DataSource = dt
            'End If

            dv.RowFilter = StrQry
            lblTotRec.Text = "Total Records: " & vsfDet.Rows.Count - 4
            Call syncGridCols()

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
        applyFilterFromGrid = True
        
    End Function

    Private Sub frmUtlView_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.KeyPreview = True
        Dim ctrl As Control = Nothing

        initGridStyles(Me)
        Call initFormVsfDet(Me, Me.Text, vsfDet, "DET")

        Call syncGridCols()
    End Sub

    Private Sub chkRemoveSubTot_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkRemoveSubTot.CheckedChanged
        If chkRemoveSubTot.Checked Then
            vsfDet.Subtotal(C1.Win.C1FlexGrid.AggregateEnum.Clear)
            chkRemoveSubTot.Visible = False
        End If
    End Sub

    Private Sub frmRptView_KeyUp(sender As Object, e As KeyEventArgs) Handles Me.KeyUp
        If e.KeyCode = Keys.F9 And e.Control = True Then

            Call initFormVsfDet(Me, Me.Text, vsfDet, "DET")
            ' Call initFormVsfDet(Me, Me.Text, vsfSearch, "SEARCH")
            MsgBox("Restored Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If

        If e.KeyCode = Keys.F8 And e.Control = True Then
            Call SaveActFormSettings(Me, Me.Text)
            Call SaveActFormVsfDet(Me, Me.Text, "DET", vsfDet)
            'Call SaveActFormVsfDet(Me, Me.Text, "SEARCH", vsfSearch)

            MsgBox("Saved Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
    End Sub


    Private Sub pnlbtnClose_Click(sender As Object, e As EventArgs) Handles pnlbtnClose.Click
        pnlCopyShortCut.Visible = False
    End Sub

    Private Sub btnCopy_Click(sender As Object, e As EventArgs) Handles btnCopy.Click

        If RbnCopyCell.Checked = True Then
            CopyGrid(vsfDet, "Cell")
        ElseIf RbnCopyColumn.Checked = True Then

            CopyGrid(vsfDet, "Column")

        ElseIf RbnCopyRow.Checked = True Then

            CopyGrid(vsfDet, "Row")
        ElseIf RbnCopyGrid.Checked = True Then

            CopyGrid(vsfDet, "Full Grid")
        End If

        pnlCopyShortCut.Visible = False
    End Sub

    Private Sub vsfDet_Click(sender As Object, e As EventArgs) Handles vsfDet.Click

        If (Control.ModifierKeys And Keys.Control) = Keys.Control AndAlso (Control.ModifierKeys And Keys.Alt) = Keys.Alt Then
            pnlCopyShortCut.Show()
        End If
    End Sub
End Class