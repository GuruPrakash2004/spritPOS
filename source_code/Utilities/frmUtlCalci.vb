Public Class frmUtlCalci
    Dim clsSVT As New svtLibrary
    Private Sub frmUtlCalci_KeyUp(sender As Object, e As KeyEventArgs) Handles Me.KeyUp
        If e.KeyCode = Keys.Escape Then
            Clipboard.SetText(txtResult.Text)
            Me.Close()
            Me.Dispose()
        End If
    End Sub

    Private Sub frmUtlCalci_Load(sender As Object, e As EventArgs) Handles Me.Load
        Me.KeyPreview = True
    End Sub

    Private Sub txtFormula_TextChanged(sender As Object, e As EventArgs) Handles txtFormula.TextChanged
        Dim rs As oledb.oledbDataReader
        Dim strSQL As String
        Try
            txtResult.Text = ""
            lblError.Text = ""
            strsql = "Select " & txtFormula.Text
            Using cmd As New oledb.oledbCommand ()
                cmd.Connection = svtConn
                cmd.CommandType = CommandType.Text
                cmd.CommandText = strSQL
                rs = cmd.ExecuteReader()
            End Using
            Do While rs.Read
                txtResult.Text = GetVal(rs.GetValue(0), 0)
                Exit Do
            Loop
            rs.Close()
        Catch ex As Exception
            lblError.Text = "There is some error in the entered expression. Please rectify it."
        End Try

    End Sub
End Class