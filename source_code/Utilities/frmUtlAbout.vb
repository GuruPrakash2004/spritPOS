Imports System.Net

Public Class frmUtlAbout
    Private Sub frmUtlAbout_KeyUp(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyUp
        If e.KeyCode = Keys.Escape Then
            Me.Close()
        End If
    End Sub

    Private Sub frmUtlAbout_Load(sender As Object, e As EventArgs) Handles Me.Load
        'Dim address As String = "file:" & gstrRptPath.Replace("\", "/") & "media/index.html"
        'WB.Navigate(New Uri("R:/SqlDBRpt/vLQMS/Rpts/Media/index.html"))

        WB.AllowNavigation = True

        If Not System.IO.File.Exists(gstrRptPath & "media\index.html") Then
            MsgBox("Create Media Folder under Rpts Folder And Make Sure All the Files are exists....!", MsgBoxStyle.Exclamation, Me.Text)
        Else
            Dim OBJURL As New System.Uri(gstrRptPath & "media\index.html")
            WB.Navigate(OBJURL)
        End If
    End Sub
End Class