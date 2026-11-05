Imports System.ComponentModel

Public Class frmUtlCancelInvRmks
    Public argFrm As Object
    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        If txtRemarks.Text = "" Then
            MsgBox("Please enter the Reason for Cancellation!", MsgBoxStyle.Critical, Me.Text)
            Exit Sub
        End If
        argFrm.strCancelRemarks = txtRemarks.Text
    End Sub


End Class