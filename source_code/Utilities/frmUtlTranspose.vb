Imports C1.Win.C1FlexGrid
Imports System.IO

Public Class frmUtlTranspose
    Public vsfdet As C1FlexGrid

    Private Sub btnpnlClose_Click(sender As Object, e As EventArgs) Handles btnpnlClose.Click
        Me.Close()
    End Sub

    Private Sub btnExpExcel_Click(sender As Object, e As EventArgs) Handles btnExpExcel.Click
        Call ExportToXL(vsfTranspose)

    End Sub

    Private Sub btnExpNotepad_Click(sender As Object, e As EventArgs) Handles btnExpNotepad.Click
        Call ExportToXL(vsfTranspose, "CSV")
    End Sub

    Private Sub frmUtlTranspose_Load(sender As Object, e As EventArgs) Handles Me.Load
        KeyPreview = True
        Dim i, j As Integer

        If vsfdet.Rows.Count <= 1 Then Exit Sub
        If vsfdet.Selection.TopRow < 0 Then Exit Sub
        If vsfdet.Selection.BottomRow < 0 Then Exit Sub


        Dim intStartRow As Integer = vsfdet.Selection.TopRow
        Dim intEndRow As Integer = vsfdet.Selection.BottomRow
        Dim intRowCount As Integer = intEndRow - intStartRow

        With vsfTranspose
            .Cols.Count = 0
            .Rows.Count = 0
            .Cols.Fixed = 0
            .Cols.Count = intRowCount + 2

            For i = 0 To vsfdet.Cols.Count - 1
                If vsfdet.Cols(i).Visible = True Then
                    Dim intCol As Int32 = 0
                    .Rows.Count = .Rows.Count + 1
                    .SetData(.Rows.Count - 1, intCol, vsfdet.GetData(3, i))
                    For j = intStartRow To intEndRow
                        intCol = intCol + 1
                        .SetData(.Rows.Count - 1, intCol, vsfdet.GetData(j, i))
                    Next
                End If
            Next
            .AutoSizeCols()
        End With

    End Sub

    Private Sub vsfTranspose_BeforeEdit(sender As Object, e As RowColEventArgs) Handles vsfTranspose.BeforeEdit
        e.Cancel = True
    End Sub

    Private Sub frmUtlTranspose_KeyUp(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyUp
        'If e.KeyCode = Keys.Alt And e.KeyCode = Keys.E Then
        If e.Alt = True And e.KeyCode = Keys.E Then
            'If e.KeyCode = Keys.F12 And e.KeyCode <> Keys.Alt And e.KeyCode <> Keys.Control Then
            Call ExportToXL(vsfTranspose)
        End If

        If e.Alt = True And e.KeyCode = Keys.N Then
            Call ExportToXL(vsfTranspose, "CSV")
        End If

        If e.KeyCode = Keys.Alt And e.KeyCode = Keys.C Then
            Me.Close()
        End If
    End Sub
End Class