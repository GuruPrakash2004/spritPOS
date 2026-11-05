Public Class frmUtlMultilineNote

    Public objfrm As Object
    Public StrgetText As String

    Private Sub frmUtlMultilineNote_KeyUp(sender As Object, e As KeyEventArgs) Handles Me.KeyUp
        If (e.Alt AndAlso (e.KeyCode = Keys.C)) Then
            Me.Close()
        End If
        If e.KeyCode = Keys.Escape Then
            Me.Close()
        End If
    End Sub

    Private Sub txtNote_KeyUp(sender As Object, e As KeyEventArgs) Handles txtNote.KeyUp
        If (e.KeyCode = Keys.Return And e.Control = True) Or (e.KeyCode = Keys.W And e.Control = True) Then
            e.Handled = True
            Call saveAndExit()
        End If
    End Sub

    Private Sub frmUtlMultilineNote_Load(sender As Object, e As EventArgs) Handles Me.Load
        Me.KeyPreview = True
    End Sub
    Private Sub saveAndExit()
        objfrm.StrgetText = txtNote.Text
        Replace(StrgetText, "'", "`")
        Me.Close()
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Call saveAndExit()
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub txtNote_TextChanged(sender As Object, e As EventArgs) Handles txtNote.TextChanged
        txtNote.ScrollBars = ScrollBars.None
        TextBoxSizeChanged(txtNote, e)
    End Sub
End Class