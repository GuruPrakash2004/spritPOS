Imports System.Data.OleDb
Imports C1.Win.C1FlexGrid

Public Class frmUtlTemp
    Dim clsSVT As New svtLibrary
    Public strMstTblName As String
    Public intDocID As Integer
    Dim strSQL As String

    Private Sub btnClear_Click(sender As Object, e As EventArgs) Handles btnClear.Click
        txtFlds.Text = ""
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Dispose()
    End Sub

    Private Sub frmUtlTemp_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Call FillMstFlds()
    End Sub

    Private Sub FillMstFlds()
        Dim dt As DataTable
        strSQL = "Select Column_Name [Field Code] from Information_schema.Columns where Table_Name='" & strMstTblName & "'"
        dt = clsSVT.getDATATable(strSQL)
        vsfFlds.DataSource = dt
        txtFlds.Text = clsSVT.getDataStringValue("Select CopyFlds from UtlPartyCopy where DocID=" & intDocID, "", False)
    End Sub

    Private Sub vsfFlds_DoubleClick(sender As Object, e As EventArgs) Handles vsfFlds.DoubleClick
        If vsfFlds.Rows.Count <= 1 Then Exit Sub
        txtFlds.Text = txtFlds.Text & "<<" & UCase(vsfFlds.GetData(vsfFlds.Row, vsfFlds.Col)) & ">>"
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        clsSVT.execSQLStatement("Delete from UtlPartyCopy where docID=" & intDocID)
        strSQL = "Insert into UtlPartyCopy(DocID,CopyFlds)values("
        strSQL &= intDocID & ","
        strSQL &= " '" & Replace(txtFlds.Text, vbLf, vbCrLf) & "')"
        clsSVT.execSQLStatement(strSQL)
        Me.Dispose()
    End Sub

    Private Sub vsfFlds_BeforeEdit(sender As Object, e As RowColEventArgs) Handles vsfFlds.BeforeEdit
        e.Cancel = True
    End Sub

    Private Sub cboTemp_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cboTemp.KeyPress
        If e.KeyChar = "'" Then e.Handled = True
    End Sub

    Private Sub txtFlds_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtFlds.KeyPress
        If e.KeyChar = "'" Then e.KeyChar = "`"
    End Sub

End Class