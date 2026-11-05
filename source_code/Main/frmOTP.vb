Public Class frmOTP
    Dim clsSVT As New svtLibrary
    Public strUserName As String

    Private Sub btnContinue_Click(sender As Object, e As EventArgs) Handles btnContinue.Click
        If Trim(txtOTP.Text) = "" Then
            MsgBox("Please enter the OTP!!", vbInformation, Me.Text)
            'Err.SetError(txtOTP, "Not a valid OTP!!")
            Exit Sub
        End If
        Dim strOTP As String = clsSVT.getDataStringValue("Select OTP from MstUser where UserCode ='" & Trim(strUserName) & "'", "", False)
        If strOTP <> Trim(txtOTP.Text) Then
            MsgBox("Not a valid OTP!!", vbInformation, Me.Text)
            'Err.SetError(txtOTP, "Not a valid OTP!!")
            txtOTP.Focus()
            Exit Sub
        End If
        Call clsSVT.execSQLStatement("update MstUser set OTP='', OTPValidity='' where UserCode ='" & Trim(strUserName) & "'")
        frmLogin.txtOldPwd.Text = svtCrypto.Decrypt(clsSVT.getDataStringValue("Select LoginPwd from MstUser where UserCode ='" & Trim(strUserName) & "'", "", False))
        frmLogin.pnlLogin.Visible = False
        frmLogin.pnlCnfPwd.Visible = True
        frmLogin.txtNewPwd.Focus()
        Me.Close()
    End Sub

    Private Sub txtOTP_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtOTP.KeyPress
        If e.KeyChar = vbBack Then Exit Sub
        If e.KeyChar = "'" Then
            e.Handled = True
            Exit Sub
        End If
        If Control.ModifierKeys = Keys.Control And e.KeyChar = ChrW(22) Then
            If System.Windows.Forms.Clipboard.GetData(DataFormats.Text).ToString() <> "" Then
                Dim strOTP As String = System.Windows.Forms.Clipboard.GetData(DataFormats.Text).ToString()
                If Len(strOTP) <> 4 Then
                    MsgBox("Not a valid OTP!", vbInformation, Me.Text)
                    e.KeyChar = ""
                    Exit Sub
                End If
            End If
        Else
            If Not KeyValidate("String", e.KeyChar, 4) Then
                e.Handled = True
            End If
            If Len(txtOTP.Text) > 3 Then
                e.Handled = True
            End If
        End If

    End Sub
    Private Sub btnSendOTP_Click(sender As Object, e As EventArgs) Handles btnSendOTP.Click
        Dim strMobNO As String = "", strMsg As String = "", strOTP As String = ""
        strMsg = "Your OTP for security is <<OTP>> , do not share for security reasons."
        strOTP = GenerateOTP()
        strMsg = Replace(strMsg, "<<OTP>>", strOTP)
        strMobNO = clsSVT.getDataStringValue("Select MobileNo from MstUser where UserCode ='" & Trim(strUserName) & "'", "", False)
        Call SendSMS(strMsg, strMobNO, "")
        'txtOTP.Text = strOTP
        Call clsSVT.execSQLStatement("update MstUser set OTP='" & strOTP & "', OTPValidity='" & getSystemDateTime.AddHours(1) & "' where UserCode ='" & Trim(strUserName) & "'")
        txtOTP.Focus()
    End Sub

    Private Sub lblGoBack_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles lblGoBack.LinkClicked
        Me.Close()
    End Sub

    Private Sub btnEmail_Click(sender As Object, e As EventArgs) Handles btnEmail.Click
        Dim strEMail As String = "", strSub As String = "", strOTP As String = "", strBody As String = ""
        strSub = "OTP for your Login password reset"
        'strBody = "OTP for your resetting your password is <<OTP>>"
        strBody = "Your OTP for security is <<OTP>> , do not share for security reasons."
        strBody = strBody & vbCrLf & ""
        strBody = strBody & vbCrLf & ""
        strBody = strBody & vbCrLf & "From Softvent MfgSys"
        strOTP = GenerateOTP()
        strBody = Replace(strBody, "<<OTP>>", strOTP)
        strEMail = clsSVT.getDataStringValue("Select EMailID from MstUser where UserCode ='" & Trim(strUserName) & "'", "", False)
        Call sendGenMail(strEMail, "", strSub, strBody)
        Call clsSVT.execSQLStatement("update MstUser set OTP='" & strOTP & "', OTPValidity='" & getSystemDateTime.AddHours(1) & "' where UserCode ='" & Trim(strUserName) & "'")

        txtOTP.Focus()
    End Sub

    Private Sub txtOTP_KeyUp(sender As Object, e As KeyEventArgs) Handles txtOTP.KeyUp

        If e.KeyCode = Keys.Enter Then
            Call btnContinue_Click(sender, e)
        End If
    End Sub

End Class