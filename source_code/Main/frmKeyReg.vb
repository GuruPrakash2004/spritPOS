Public Class frmKeyReg
    Private Sub txtRegNo_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtRegNo.KeyPress
        If Not IsNumeric(e.KeyChar) OrElse e.KeyChar = vbBack Then
            e.KeyChar = Nothing
        End If
    End Sub

    Private Sub txtRegNo_KeyUp(sender As Object, e As KeyEventArgs) Handles txtRegNo.KeyUp
        If Trim(txtRegNo.Text) = "" Then Exit Sub
        If e.KeyCode = Keys.Enter Then
            Dim cls As New clsReg
            txtKey.Text = cls.getKey(txtRegNo.Text)
        End If
    End Sub

    Private Sub frmKeyReg_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing

    End Sub
End Class