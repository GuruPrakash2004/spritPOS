Imports System.Data.oledb

Public Class frmRPtInvalidItems
    Dim clsSVT As New svtLibrary

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Me.Close()
        Me.Dispose()

    End Sub

    Private Sub frmRptStkLtr_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim dtSQL As DataTable, dtView As DataView
        Dim strSQL As String
        strSQL = "select * from vwInvalidItems  order by 1,2"
        dtSQL = clsSVT.getDATATable(strSQL)
        dtView = New DataView(dtSQL)
        vsfView.Cols.Fixed = 0
        vsfView.DataSource = dtView
        vsfView.BeginUpdate()
        vsfView.DataSource = dtView
        vsfView.EndUpdate()
    End Sub

    Private Sub btnExportToXL_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExportToXL.Click
        Call ExportToXL(vsfView,,,, True, Me.Text)
    End Sub

    Private Sub vsfDet_BeforeEdit(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.RowColEventArgs)
        e.Cancel = True
    End Sub

    Private Sub pnlbtnClose_Click(sender As Object, e As EventArgs) Handles pnlbtnClose.Click
        pnlCopyShortCut.Visible = False
    End Sub

    Private Sub btnCopy_Click(sender As Object, e As EventArgs) Handles btnCopy.Click

        If RbnCopyCell.Checked = True Then
            CopyGrid(vsfView, "Cell")
        ElseIf RbnCopyColumn.Checked = True Then

            CopyGrid(vsfView, "Column")

        ElseIf RbnCopyRow.Checked = True Then

            CopyGrid(vsfView, "Row")
        ElseIf RbnCopyGrid.Checked = True Then

            CopyGrid(vsfView, "Full Grid")

        End If
        pnlCopyShortCut.Visible = False
    End Sub

    Private Sub vsfView_Click(sender As Object, e As EventArgs) Handles vsfView.Click

        If (Control.ModifierKeys And Keys.Control) = Keys.Control AndAlso (Control.ModifierKeys And Keys.Alt) = Keys.Alt Then
            pnlCopyShortCut.Show()
        End If

    End Sub
End Class