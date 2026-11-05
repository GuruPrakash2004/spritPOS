Imports System.ComponentModel

Public Class frmUtlDaysExpOPBalance
    Dim clsSvt As New svtLibrary

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Dim strSQL As String
        clsSvt.execSQLStatement("Delete from TrnExpOPBal where expDate = " & getDateFormatInCond(dtDocDate.Value))
        strSQL = "Insert into TrnExpOPBal (ExpDate, OPBalAmt) values ("
        strSQL &= "'" & getDateFormatInInsert(dtDocDate.Value) & "',"
        strSQL &= Val(txtOPAmt.Text) & ")"
        clsSvt.execSQLStatement(strSQL)
        MsgBox("Saved Opening balance!", MsgBoxStyle.Information, Me.Text)
        Me.Close()
    End Sub


    Private Sub dtDocDate_Validating(sender As Object, e As CancelEventArgs) Handles dtDocDate.Validating
        If dtDocDate.Value > DateAdd(DateInterval.Day, 1, getSystemDate) Then
            MsgBox("Date cannot be greater than tomorrow!", MsgBoxStyle.Critical, Me.Text)
            e.Cancel = True
            Exit Sub
        End If
        txtOPAmt.Text = clsSvt.getDataDoubleValue("Select OPBalAmt from TrnExpOpBal where ExpDate = " & getDateFormatInCond(dtDocDate.Value), 0, False)
    End Sub

    Private Sub txtOPAmt_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtOPAmt.KeyPress
        If Not KeyValidate("Integer", e.KeyChar) Then
            e.Handled = True
        End If
    End Sub
End Class