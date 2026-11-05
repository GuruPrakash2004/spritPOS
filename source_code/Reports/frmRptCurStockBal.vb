Imports System.Data.oledb
Imports C1.Win.C1FlexGrid

Public Class frmRptCurStockBal
    Public strList As String
    Dim strLocList As String
    Dim clsSVT As New svtLibrary
    Dim dvSearch As DataView
    Dim dtSearch As DataTable
    Dim argvsf As C1FlexGrid
    Private Sub frmRptCurStockBal_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.KeyPreview = True
        'Call initScreen()
        Call initFormSize(Me, Me.Text, False, 816, 489, 0, 0)
        Call initFormVsfDet(Me, Me.Text, vsfDet, "DET")
        Call fillFormProperties(Me)
        Call initGridStyles(Me)
        vsfView.Visible = False
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
        Me.Dispose()
    End Sub

    Private Sub frmRptCurStockBal_KeyUp(sender As Object, e As KeyEventArgs) Handles Me.KeyUp
        If e.KeyCode = Keys.F9 And e.Control = True Then
            '958, 579
            Call initFormSize(Me, "RptStkDetails", True, 816, 489, 0, 0)
            'Call SetColWidthFromString(vsfDet, strDocStdDtlWidth)
            MsgBox("Restored Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
        If e.KeyCode = Keys.F8 And e.Control = True Then
            Call SaveActFormSettings(Me, "RptStkDetails")
            Call SaveActFormVsfDet(Me, "RptStkDetails", "", vsfDet)
            'Call SaveItemComboFrameSettings(fraSearch, vsfView,  intDocID)
            MsgBox("Saved Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If

        If e.KeyCode = Keys.Alt And e.KeyCode = Keys.C Then
            Me.Close()
        End If

        If e.KeyCode = Keys.Alt And e.KeyCode = Keys.E Then
            Call ExportToXL(vsfDet)
        End If
    End Sub

    Private Sub btnExportToXL_Click(sender As Object, e As EventArgs) Handles btnExportToXL.Click
        Call ExportToXL(vsfDet,,,, True, Me.Text)
    End Sub

    Private Sub btnView_Click(sender As Object, e As EventArgs) Handles btnView.Click
        'vsfDet.DataSource = dvSearch
        FillGrid()
    End Sub

    Private Sub vsfView_BeforeEdit(sender As Object, e As RowColEventArgs) Handles vsfView.BeforeEdit
        e.Cancel = True
    End Sub

    Private Sub txtItemCode_KeyUp(sender As Object, e As KeyEventArgs) Handles txtItemCode.KeyUp
        If e.KeyCode = Keys.Escape Then
            vsfView.Visible = False
            txtItemCode.Focus()
        End If
        If e.KeyCode = Keys.Enter Or e.KeyCode = Keys.Return Then
            If vsfView.Visible Then
                If vsfView.Rows.Count > 1 Then
                    txtItemCode.Text = vsfView.GetData(vsfView.Row, 1)
                End If
            End If
        End If
        If e.KeyCode = Keys.Down Then
            If vsfView.Visible Then
                If vsfView.Rows.Count > 1 Then
                    vsfView.Focus()
                    vsfView.Col = 1
                    vsfView.Row = 1
                    vsfView.TopRow = 1
                End If
            End If
        End If
    End Sub

    Private Sub vsfView_KeyUp(sender As Object, e As KeyEventArgs) Handles vsfView.KeyUp
        If e.KeyCode = Keys.Escape Then
            txtItemCode.Focus()
            vsfView.Visible = False
        End If
        If vsfView.Row > 0 Then
            If e.KeyCode = Keys.Return Or e.KeyCode = Keys.Tab Then
                txtItemCode.Text = vsfView.GetData(vsfView.Row, 1)
                vsfView.Visible = False
            End If
        End If
    End Sub

    Private Sub txtItemCode_TextChanged(sender As Object, e As EventArgs) Handles txtItemCode.TextChanged
        Call applyFilterFromGrid()
    End Sub

    Private Sub vsfDet_BeforeEdit(sender As Object, e As RowColEventArgs) Handles vsfDet.BeforeEdit
        e.Cancel = True
    End Sub

    Private Sub vsfView_DoubleClick(sender As Object, e As EventArgs) Handles vsfView.DoubleClick
        If vsfView.Row > 0 Then
            txtItemCode.Text = vsfView.GetData(vsfView.Row, 1)
            vsfView.Visible = False
        End If
    End Sub

    Private Sub chkLoc_CheckedChanged(sender As Object, e As EventArgs) Handles chkLoc.CheckedChanged
        Dim strSQL As String
        If chkLoc.Checked = False Then
            strList = ""
            strLocList = ""
            Exit Sub
        End If

        'Dim frmSel As New frmUtlSelLoc
        Dim frmSel As New frmUtlMultiSelRpt
        strSQL = " Select distinct 0 as sel, locId, locName from Mstloc where locID > 0 and locName Not in ('-' , '')order by locName"
        Call frmSel.InitVsfSel(strSQL, 1, 1, True, Me, False)
        frmSel.Text = "Select [Multiple] Make"
        frmSel.vsfDet.SetData(0, 0, "Sel")
        'frmSel.vsfDet.SetData(0, 1, "Location")
        frmSel.ShowDialog()
        strLocList = strList
        'strSQL = "select LOCNAME from MstLOC"
        '    dtSearch = clsSVT.getDATATable(strSQL)

        '    dvSearch = New DataView(dtSearch)
        '    'vsfDet.DataSource = dvSearch

    End Sub

    Private Function applyFilterFromGrid() As Boolean
        applyFilterFromGrid = False
        Dim strSQL As String = ""
        With txtItemCode
            If Trim(.Text) = "" Then
                vsfView.Visible = False
                Exit Function
            End If
            If Len(.Text) <= 2 Then
                vsfView.Visible = False
                Exit Function
            End If
        End With

        vsfView.Visible = True

        Dim dtSearch As DataTable = Nothing

        Application.DoEvents()


        strSQL = "SELECT top 50 ItemId, ItemCode, ItemName"
        strSQL &= " FROM MstItem i "
        strSQL &= " WHERE i.InActive = 'N'"
        'strSQL &= " and i.MakeID = m.MakeID"
        strSQL &= " and  ItemCode like '%" & txtItemCode.Text & "%'"
        strSQL &= " and ItemID > 0"
        strSQL &= " order by 2, 3"

        Call showWaitCursor()
        dtSearch = clsSVT.getDATATable(strSQL)

        dvSearch = New DataView(dtSearch)

        vsfView.DataSource = dvSearch
        vsfView.Visible = True
        vsfView.Cols(0).Visible = False

        vsfView.Cols(1).Width = 150
        Call showNormalCursor()

        applyFilterFromGrid = True
    End Function

    Private Function FillGrid() As Boolean
        FillGrid = False
        Dim strSQL As String = ""
        Dim dtSearch As DataTable = Nothing

        Application.DoEvents()


        strSQL = "SELECT ItemCode, ItemName,LOCCode, LOCName,LOTNO,OPQty"
        strSQL &= " FROM MstItem i, MstLOC l,TrnQOH q "
        strSQL &= " WHERE q.ItemId = i.ItemId"
        strSQL &= " and q.LOCId = l.LOCId"
        strSQL &= " and  ItemCode like '%" & txtItemCode.Text & "%'"
        'strSQL &= " and ItemID > 0"
        strSQL &= " order by 2, 3,4,5"

        Call showWaitCursor()
        dtSearch = clsSVT.getDATATable(strSQL)

        dvSearch = New DataView(dtSearch)

        vsfDet.DataSource = dvSearch
        vsfDet.Visible = True
        vsfDet.Cols(1).Width = 150
        Call showNormalCursor()

        Dim rs As oledbDataReader = clsSVT.getDataReaderFromSQL(strSQL)
        If IsNothing(rs) Then
            rs.Close()
            MsgBox("No Records!", MsgBoxStyle.Critical, Me.Text)
            Exit Function
        End If

        If Not rs.HasRows Then
            MsgBox("No Records!", MsgBoxStyle.Critical, Me.Text)
        End If

        FillGrid = True
    End Function

    Private Sub pnlbtnClose_Click(sender As Object, e As EventArgs) Handles pnlbtnClose.Click
        pnlCopyShortCut.Visible = False
    End Sub

    Private Sub btnCopy_Click(sender As Object, e As EventArgs) Handles btnCopy.Click
        If RbnCopyCell.Checked = True Then
            If vsfView.RowSel >= 0 AndAlso vsfView.ColSel >= 0 Then
                Dim cellValue As String = vsfView(vsfView.RowSel, vsfView.ColSel).ToString()
                Clipboard.SetText(cellValue)
            End If

        ElseIf RbnCopyColumn.Checked = True Then

            If vsfView.ColSel >= 0 Then
                Dim colIndex As Integer = vsfView.ColSel
                Dim result As String = ""
                For rowIndex As Integer = 0 To vsfView.Rows.Count - 1
                    If vsfView(rowIndex, colIndex) IsNot Nothing Then
                        result &= vsfView(rowIndex, colIndex).ToString() & vbCrLf
                    Else
                        result &= vbCrLf
                    End If
                Next
                Clipboard.SetText(result)
            End If

        ElseIf RbnCopyRow.Checked = True Then

            If vsfView.RowSel >= 0 Then
                Dim rowIndex As Integer = vsfView.RowSel
                Dim result As String = ""
                For colIndex As Integer = 0 To vsfView.Cols.Count - 1
                    If vsfView(rowIndex, colIndex) IsNot Nothing Then
                        result &= vsfView(rowIndex, colIndex).ToString() & vbTab
                    Else
                        result &= vbTab
                    End If
                Next
                result = result.TrimEnd(vbTab)
                Clipboard.SetText(result)
            End If
        ElseIf RbnCopyGrid.Checked = True Then

            Dim result As String = ""
            For rowIndex As Integer = 3 To vsfView.Rows.Count - 1
                Dim rowResult As String = ""
                For colIndex As Integer = 0 To vsfView.Cols.Count - 1
                    If vsfView.Cols(colIndex).Visible Then
                        If vsfView(rowIndex, colIndex) IsNot Nothing Then
                            Dim cellValue As Object = vsfView(rowIndex, colIndex)
                            rowResult &= vsfView(rowIndex, colIndex).ToString() & vbTab
                        Else
                            rowResult &= vbTab
                        End If
                    End If
                Next
                If rowResult.Length > 0 Then
                    rowResult = rowResult.TrimEnd(vbTab.ToCharArray())
                End If
                result &= rowResult & vbCrLf
            Next
            result = result.TrimEnd(vbCrLf.ToCharArray())
            Clipboard.SetText(result)

        End If

        pnlCopyShortCut.Visible = False
    End Sub

    Private Sub vsfDet_Click(sender As Object, e As EventArgs) Handles vsfDet.Click

        If (Control.ModifierKeys And Keys.Control) = Keys.Control AndAlso (Control.ModifierKeys And Keys.Alt) = Keys.Alt Then
            pnlCopyShortCut.Show()
        End If
        argvsf = vsfDet

    End Sub

    Private Sub vsfSearch_Click(sender As Object, e As EventArgs) Handles vsfView.Click
        If (Control.ModifierKeys And Keys.Control) = Keys.Control AndAlso (Control.ModifierKeys And Keys.Alt) = Keys.Alt Then
            pnlCopyShortCut.Show()
        End If
        argvsf = vsfView
    End Sub
End Class