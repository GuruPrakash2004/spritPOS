Public Class frmMstUserAdmPwdChange
    Dim clsSVT As New svtLibrary
    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub btnUpdate_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click
        If txtOldAdmPwd.Text = "" Or txtAdmPwd.Text = "" Then
            MsgBox("Additional Admin Password should not be blank", MsgBoxStyle.Critical, Me.Text)
            Exit Sub
        End If

        Dim strDPwd As String
        strDPwd = clsSVT.getDataStringValue("Select admloginpwd from Mstuser where UserID = 1", "", False)
        If svtCrypto.Decrypt(strDPwd) <> txtOldAdmPwd.Text Then
            MsgBox("Incorrect Old Additional Admin Password", MsgBoxStyle.Critical, Me.Text)
            Exit Sub
        End If
        If txtOldAdmPwd.Text = txtAdmPwd.Text Then
            MsgBox("Enter different Old & New Password", MsgBoxStyle.Critical, Me.Text)
            Exit Sub
        End If
        Dim strSQL As String
        strSQL = "Update MstUser set admloginpwd = '" & svtCrypto.Encrypt(txtAdmPwd.Text) & "'"
        strSQL &= " where UserID = 1"
        Call clsSVT.execSQLStatement(strSQL)
        MsgBox("Successfuly updated Additional ADMIN Password", MsgBoxStyle.Information, Me.Text)
        Me.Close()
    End Sub


    Private Sub imgEye_MouseLeave(sender As Object, e As EventArgs) Handles imgEye.MouseLeave
        txtAdmPwd.PasswordChar = "*"
        txtOldAdmPwd.PasswordChar = "*"
        Try
            imgEye.Image = Image.FromFile(gstrRptPath & "imgDontShow.png")
        Catch ex As Exception
        End Try
    End Sub

    Private Sub imgEye_MouseEnter(sender As Object, e As EventArgs) Handles imgEye.MouseEnter
        txtAdmPwd.PasswordChar = ""
        txtOldAdmPwd.PasswordChar = ""
        Try
            imgEye.Image = Image.FromFile(gstrRptPath & "imgShow.png")
        Catch
        End Try
    End Sub
End Class