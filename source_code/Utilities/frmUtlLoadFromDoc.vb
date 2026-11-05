Public Class frmUtlLoadFromDoc
    Public argSQL As String
    Public argRetCol As Integer
    Public argDispCol As Integer
    Public argF As Object
    Private Sub btnSelect_Click(sender As Object, e As EventArgs) Handles btnSelect.Click
        Dim objfrm As New frmUtlSelection
        With objfrm
            .blnMultiColSearch = True
            .FillViewVsf(Me, False, Nothing, argSQL, txtDocNo, argDispCol, txtRet, argRetCol)
            .Text = Me.Text
            .Tag = " frmUtlLoadFromDoc "
            .ShowDialog()
            .Close()
        End With

        If Trim(txtRet.Text) <> "" Then
            argF.strLoadedDocNo = txtRet.Text
        End If
    End Sub
End Class