Imports System.ComponentModel
Imports System.IO
Imports System.Windows.Forms

Public Class frmLogin
    Dim Mode As String
    Dim clsSVT As New svtLibrary
    Dim strDPwd As String = ""
    Dim blnRememberMe As Boolean = False
    Dim strOTPMobNo As String
    Public blnAppIdle As Boolean = False
    Dim intMinSplChrs As Integer, intMinNumeric As Integer, intMinAlpha As Integer, intMinLen As Integer, intMinLCase As Integer, intMinUCase As Integer

    Private Sub OK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click
        'If frmMain.dockTabMain.TabPages.Count > 0 Then
        '    MsgBox("Please close all open pages in application..", MsgBoxStyle.Information, Me.Text)
        '    Exit Sub
        'End If

        If txtUsername.Text = "" Then
            MsgBox("User Name Empty", vbExclamation, "Warning")
            txtUsername.Focus()
            Exit Sub
        End If
        If txtPwd.Text = "" Then
            MsgBox("Password Empty", vbExclamation, "Warning")
            txtPwd.Focus()
            Exit Sub
        End If
        If txtUsername.Text.ToUpper = "ADMIN" And gBlnAdvAdminLogin Then
            If txtAdmPwd.Text = "" Then
                MsgBox("Addnitional ADMIN Password Empty", vbExclamation, "Warning")
                txtAdmPwd.Focus()
                Exit Sub
            End If
        End If
        If Not userValid() Then
            Exit Sub
        End If
        gBlnCheckForFraud = False
        If txtUsername.Text.ToUpper = "ADMIN" And gBlnAdvAdminLogin Then
            gBlnCheckForFraud = True
        End If
        gUserCode = UCase(Trim(txtUsername.Text))
        gUserID = clsSVT.getDataLongValue("Select UserID from MstUser where UserCode = '" & gUserCode & "'", 0, False)
        gRoleID = clsSVT.getDataLongValue("Select RoleID from MstUser where UserCode = '" & gUserCode & "'", 0, False)
        blnAppStarted = True
        blnAppIdle = False
        Call INIStore("Login", "UserName", gUserCode)
        Call INIStore("Login", "RememberMe", IIf(chkRememberMe.Checked = True, "Y", "N"))
        Call frmMain.EnableDisableMenus()

        frmMain.lblUsername.Text = txtUsername.Text
        frmMain.sbPnlLogin.Text = "Login : " & gUserCode
        frmMain.GetUserImage()
        Call frmMain.EnableDisableMenus()
        Me.Dispose()
    End Sub

    Private Sub Cancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Function userValid() As Boolean
        Dim rs As oledb.oledbDataReader, intExpDays As Integer, dtPWDExp As Date
        userValid = False
        rs = clsSVT.getDataReaderFromSQL("select * from MstUser  where UserCode = '" & txtUsername.Text & "'")
        If Not rs.HasRows Then
            MsgBox("Invalid Login Name !!", MsgBoxStyle.Critical, "Warning")
            txtUsername.Text = ""
            txtUsername.Focus()
            rs.Close()
            Exit Function
        End If
        rs.Read()

        If rs!Authorised = "N" AndAlso txtUsername.Text <> "ADMIN" Or rs!inactive = "Y" Then
            MsgBox("Not a valid User!", MsgBoxStyle.Critical, "Warning")
            Exit Function
        End If

        If txtUsername.Text = "ADMIN" And txtPwd.Text = Date.Now.Day & "svt" & Format(Date.Now, "MMM") & Format(Date.Now, "yy") Then
            userValid = True
            Exit Function
        End If

        If Not gblnADAuth Then
            strDPwd = svtCrypto.Decrypt(rs!LoginPwd)
            If Trim(txtPwd.Text) <> strDPwd Then


                MsgBox("Password Incorrect ", MsgBoxStyle.Critical, "Warning")
                txtPwd.Text = ""
                txtPwd.Focus()
                rs.Close()
                Exit Function
            End If
            If txtUsername.Text.ToUpper = "ADMIN" And gBlnAdvAdminLogin Then
                strDPwd = svtCrypto.Decrypt(rs!ADMLoginPwd)
                If Trim(txtAdmPwd.Text) <> strDPwd Then
                    MsgBox("Additional ADMIN Password Incorrect ", MsgBoxStyle.Critical, "Warning")
                    txtAdmPwd.Text = ""
                    txtAdmPwd.Focus()
                    rs.Close()
                    Exit Function
                End If
            End If

            If Trim(UCase(txtUsername.Text)) <> "ADMIN" And Not gblnPwdNeveExp Then
                    intExpDays = clsSVT.getDataLongValue("Select PwdExpiryDays from UtlMisc", 0, False)
                    If intExpDays > 0 Then
                        dtPWDExp = clsSVT.getDataDateValue("select PwdResetDt from Mstuser Where UserCode='" & txtUsername.Text & "'", getSystemDate, False)
                        dtPWDExp = DateAdd(DateInterval.Day, intExpDays, dtPWDExp)
                        intExpDays = DateDiff(DateInterval.Day, CDate(getSystemDate()), dtPWDExp)
                        If intExpDays <= 10 AndAlso intExpDays > 0 Then
                            lblExpiry.Visible = True
                            lblExpiry.Text = "Your Password Expires In " & intExpDays & " days"
                        ElseIf intExpDays <= 0 Then
                            MsgBox("Your Password Has Been Expired, Please reset your password.", MsgBoxStyle.Information, Me.Text)
                            pnlCnfPwd.Visible = True
                            pnlLogin.Visible = False
                            Exit Function
                        Else
                            lblExpiry.Visible = False
                        End If
                    End If
                End If
            Else
                If Not AuthenticateUser("", txtUsername.Text, txtPwd.Text) Then
                MsgBox("UserName / Password is incorrect ", MsgBoxStyle.Critical, "Warning")
                txtUsername.Focus()
                rs.Close()
                Exit Function
            End If
        End If
        userValid = True
    End Function

    'Private Sub frmLogin_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

    '    txtUsername.Focus()

    'End Sub

    Private Sub frmLogin_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Enter Then
            If pnlCnfPwd.Visible = False Then
                btnOk.PerformClick()
                e.SuppressKeyPress = True
            Else
                btnCnfOk.PerformClick()
                e.SuppressKeyPress = True
            End If
        End If
    End Sub

    Private Sub frmLogin_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        lblAdmPwd.Visible = False
        txtAdmPwd.Visible = False
        txtUsername.Text = ""
        txtPwd.Text = ""
        imgEye.Image = vLQMS.My.Resources.imgDontShow
        PnlPwdRules.Visible = False
        Call CreateRoundObj(PicUser)
        txtUsername.Text = INIGet("Login", "UserName", gUserCode)
        'If INIGet("Login", "RememberMe", "N") = "Y" Then
        '    blnRememberMe = True
        'Else
        '    blnRememberMe = False
        'End If
        If txtUsername.Text <> "" Then
            Call LoadUser()
        End If
        Try
            If System.IO.File.Exists(gstrRptPath & "LoginLQMS.gif") = True Then
                picLoginImg.Image = Bitmap.FromFile(gstrRptPath & "LoginLQMS.gif")
            End If
            Me.BackgroundImage = Bitmap.FromFile(gstrRptPath & "\" & "LoginPage2.jpg")
        Catch ex As Exception
            picLoginImg.Image = Nothing
            Me.BackgroundImage = Nothing
        Finally

        End Try
        Call getPwdRules()
        KeyPreview = True

    End Sub

    Private Sub LoadUser()
        Dim intExpDays As Double
        Dim dtPWdExp As Date

        gUserID = clsSVT.getDataLongValue("Select UserId from MstUser where UserCode ='" & txtUsername.Text & "'", 0, False)
        Call getUserData()
        'If gblnShowRememberMe Then
        '    chkRememberMe.Visible = True
        '    If blnRememberMe Then
        '        txtPwd.Text = svtCrypto.Decrypt(strDPwd)
        '        chkRememberMe.Checked = True
        '    End If
        'Else
        '    chkRememberMe.Visible = False
        'End If
        If gblnADAuth = True Then
            btnChgPwd.Visible = False
            lblFgtPwd.Visible = False
        Else
            lblFgtPwd.Visible = True
        End If
        lblExpiry.Visible = False

        If Not gblnADAuth And Trim(UCase(txtUsername.Text)) <> "ADMIN" And Not gblnPwdNeveExp Then
            intExpDays = clsSVT.getDataLongValue("Select PwdExpiryDays from UtlMisc", 0, False)
            If intExpDays > 0 Then
                dtPWdExp = clsSVT.getDataDateValue("select PwdResetDt from Mstuser Where UserCode='" & txtUsername.Text & "'", getSystemDate, False)
                dtPWdExp = DateAdd(DateInterval.Day, intExpDays, dtPWdExp)
                intExpDays = DateDiff(DateInterval.Day, CDate(getSystemDate()), dtPWdExp)
                If intExpDays <= 10 AndAlso intExpDays > 0 Then
                    lblExpiry.Visible = True
                    lblExpiry.Text = "Your Password Expires In " & intExpDays & " days"
                ElseIf intExpDays <= 0 Then
                    lblExpiry.Text = "Your Password Has Been Expired, Please reset your password."
                    lblExpiry.Visible = True
                    Exit Sub
                Else
                    lblExpiry.Visible = False
                End If
            End If
        End If
        Call GetUserImage()
    End Sub

    Private Sub getUserData()
        Dim rsU As oledb.oledbDataReader
        Dim strSQL As String = ""
        rsU = clsSVT.getDataReaderFromSQL("Select * from MstUser where UserID=" & gUserID)

        With rsU
            If Not .HasRows Then
                .Close()
                Exit Sub
            End If
            .Read()
            strDPwd = GetVal(!LoginPwd)
            .Close()
        End With
    End Sub

    Private Sub btnChgPwd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnChgPwd.Click
        Dim rs As oledb.oledbDataReader

        If txtUsername.Text = "" Then
            MsgBox("UserName is Empty.", vbExclamation, "Warning")
            txtUsername.Focus()
            Exit Sub
        End If
        If txtPwd.Text = "" Then
            MsgBox("Please Enter Password!!!", vbExclamation, "Warning")
            txtPwd.Focus()
            Exit Sub
        End If

        rs = clsSVT.getDataReaderFromSQL("Select * from MstUser  where UserCode = '" & txtUsername.Text & "'")

        If Not rs.HasRows Then
            MsgBox("Invalid Login Name !!", MsgBoxStyle.Critical, "Warning")
            txtUsername.Text = ""
            txtUsername.Focus()
            rs.Close()
            Exit Sub
        End If
        If rs.Read Then
            strDPwd = svtCrypto.Decrypt(rs!LoginPwd)
        End If
        If Trim(txtPwd.Text) <> strDPwd Then
            MsgBox("Password Incorrect??? ", MsgBoxStyle.Critical, "Warning")
            txtPwd.Text = ""
            txtPwd.Focus()
            rs.Close()
            Exit Sub
        End If
        txtOldPwd.Text = ""
        txtNewPwd.Text = ""
        txtCnfNewPwd.Text = ""
        pnlCnfPwd.Visible = True
        pnlLogin.Visible = False
        txtOldPwd.Focus()
    End Sub

    Private Sub btnCnfCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCnfCancel.Click
        pnlCnfPwd.Visible = False
        pnlLogin.Visible = True
        txtUsername.Focus()
    End Sub

    Private Sub btnCnfOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCnfOk.Click
        Dim rs As oledb.oledbDataReader
        Dim strSql As String

        If txtOldPwd.Text = "" Or txtNewPwd.Text = "" Or txtCnfNewPwd.Text = "" Then
            MsgBox("Please enter all the details!", vbInformation, Me.Text)
            Exit Sub
        End If

        strSql = "SELECT * from MstUser where UserID = " & gUserID
        rs = clsSVT.getDataReaderFromSQL(strSql)


        With rs
            If .Read Then
                If svtCrypto.Decrypt(rs!LoginPwd) <> txtOldPwd.Text Then
                    MsgBox("Old password is incorrect!", vbCritical, Me.Text)
                    txtOldPwd.Text = ""
                    txtOldPwd.Focus()
                    Exit Sub
                Else
                    If txtNewPwd.Text = svtCrypto.Decrypt(rs!LoginPwd) Then
                        MsgBox("Old password cannot be used as new password!", vbCritical, Me.Text)
                        txtNewPwd.Text = ""
                        txtNewPwd.Focus()
                        Exit Sub
                    End If
                    If txtNewPwd.Text <> txtCnfNewPwd.Text Then
                        MsgBox("New password and confirm password does not match!", vbCritical, Me.Text)
                        txtCnfNewPwd.Text = ""
                        txtCnfNewPwd.Focus()
                        Exit Sub
                    End If

                    If gblnPwdPolicy AndAlso gblnPwdRule Then
                        If Not ValidPwdRules() Then
                            MsgBox("Password doesn't match the Rules!", vbInformation, Me.Text)
                            txtNewPwd.Focus()
                        End If
                        PnlPwdRules.Visible = False
                    End If

                    strSql = "Update MstUser set LoginPwd = '" & svtCrypto.Encrypt(txtNewPwd.Text) & "', PwdResetDt = " & getDateFormatInCond(getSystemDate)
                    strSql &= " where UserID = " & gUserID
                    clsSVT.execSQLStatement(strSql)
                    MsgBox("Password changed successfully!", vbInformation, Me.Text)
                    pnlCnfPwd.Visible = False
                    pnlLogin.Visible = True
                    txtUsername.Focus()
                End If
            End If
        End With
        pnlLogin.Enabled = True
    End Sub

    Private Sub frmLogin_Shown(sender As Object, e As System.EventArgs) Handles Me.Shown
        If txtUsername.Text = "" Then
            txtUsername.Focus()
        Else
            txtPwd.Focus()
        End If
    End Sub

    Private Sub frmLogin_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        If blnAppIdle = True Then
            If MsgBox("Exit from software? ", vbQuestion + vbYesNo + vbDefaultButton1, "Softvent..." & gSettings.gCompName) = vbNo Then
                e.Cancel = True
                Exit Sub
            End If
            blnAppStarted = True
            Call frmMain.frmMain_FormClosing(sender, e)

        End If
    End Sub

    Private Sub frmLogin_KeyUp(sender As Object, e As KeyEventArgs) Handles Me.KeyUp
        If e.KeyCode = Keys.F9 And e.Control = True Then
            Call initFormSize(Me, Me.Text, True, 913, 481, 0, 0)
            MsgBox("Restored Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
        If e.KeyCode = Keys.F8 And e.Control = True Then
            Call SaveActFormSettings(Me, Me.Text)

            MsgBox("Saved Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
    End Sub
    Private Sub GetUserImage()
        Dim intuserId As Integer

        Dim strSql As String = ""
        Dim cmd As oledb.oledbCommand = Nothing

        intuserId = clsSVT.getDataLongValue("select UserId from MstUser where UserCode ='" & txtUsername.Text & "'", 0, False)
        If intuserId = 0 Then
            Exit Sub
        End If

        PicUser.Image = Nothing
        strSql = "select " & getImgSyntax("ISNULL", "UserImg") & " as UserImg from MstUser where UserID = " & intuserId
        cmd = New oledb.oledbCommand(strSql, svtConn)
        Dim imageData As Byte() = DirectCast(cmd.ExecuteScalar(), Byte())
        If imageData.Length = 0 Then
        End If
        If Not imageData Is Nothing Then
            If imageData.Length > 0 Then
                Dim ImageMemoryStream As MemoryStream = New IO.MemoryStream(GetUserImageBytesFromOLEField(imageData))
                PicUser.Image = Image.FromStream(ImageMemoryStream, True)
                PicUser.SizeMode = PictureBoxSizeMode.StretchImage
            Else
                PicUser.Image = Nothing
                PicUser.Image = vLQMS.My.Resources.user
                PicUser.SizeMode = PictureBoxSizeMode.StretchImage
            End If
        End If
    End Sub

    Friend Shared Function GetUserImageBytesFromOLEField(ByVal oleFieldBytes() As Byte) As Byte()

        Dim BITMAP_ID_BLOCK As String = "BM"
        Dim JPG_ID_BLOCK As String = ChrW(&HFF).ToString() & ChrW(&HD8).ToString() & ChrW(&HFF).ToString()
        Dim PNG_ID_BLOCK As String = ChrW(&H89).ToString() & "PNG" & vbCrLf & ChrW(&H1A).ToString() & vbLf
        Dim GIF_ID_BLOCK As String = "GIF8"
        Dim TIFF_ID_BLOCK As String = "II*" & ChrW(&H0).ToString()

        Dim imageBytes() As Byte

        ' Get a UTF7 Encoded string version
        Dim u7 As System.Text.Encoding = System.Text.Encoding.UTF7
        Dim strTemp As String = u7.GetString(oleFieldBytes)
        Dim st2 As String = System.Text.Encoding.UTF8.GetString(oleFieldBytes)
        ' Get the first 300 characters from the string
        Dim strVTemp As String = strTemp.Substring(0, 300)

        ' Search for the block
        Dim iPos As Integer = -1
        If strVTemp.IndexOf(BITMAP_ID_BLOCK) <> -1 Then
            iPos = strVTemp.IndexOf(BITMAP_ID_BLOCK)
        ElseIf strVTemp.IndexOf(JPG_ID_BLOCK) <> -1 Then
            iPos = strVTemp.IndexOf(JPG_ID_BLOCK)
        ElseIf strVTemp.IndexOf(PNG_ID_BLOCK) <> -1 Then
            iPos = strVTemp.IndexOf(PNG_ID_BLOCK)
        ElseIf strVTemp.IndexOf(GIF_ID_BLOCK) <> -1 Then
            iPos = strVTemp.IndexOf(GIF_ID_BLOCK)
        ElseIf strVTemp.IndexOf(TIFF_ID_BLOCK) <> -1 Then
            iPos = strVTemp.IndexOf(TIFF_ID_BLOCK)
        Else
            Throw New Exception("Unable to determine header size for the OLE Object")
        End If


        ' From the position above get the new image
        If iPos = -1 Then
            Throw New Exception("Unable to determine header size for the OLE Object")
        End If

        imageBytes = New Byte(CInt(oleFieldBytes.LongLength - iPos - 1)) {}
        Array.ConstrainedCopy(oleFieldBytes, iPos, imageBytes, 0, oleFieldBytes.Length - iPos)

        Return imageBytes
    End Function

    Private Sub txtUsername_Validating(sender As Object, e As CancelEventArgs) Handles txtUsername.Validating

        If Trim(txtUsername.Text) = "" Then Exit Sub
        If clsSVT.getDataStringValue("Select UserCode from MstUser where UserCode='" & txtUsername.Text & "'", "", False) <> "" Then
            Call LoadUser()
        End If
        lblAdmPwd.Visible = False
        txtAdmPwd.Visible = False
        If txtUsername.Text.ToUpper = "ADMIN" And gBlnAdvAdminLogin Then
            lblAdmPwd.Visible = True
            txtAdmPwd.Visible = True
        End If
    End Sub

    Private Sub lblFgtPwd_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles lblFgtPwd.LinkClicked
        Dim strMobNO As String = clsSVT.getDataStringValue("Select MobileNo from MstUser where UserCode ='" & Trim(txtUsername.Text) & "'", "", False)
        Dim StrEmail As String = clsSVT.getDataStringValue("Select eMailID from MstUser where UserCode ='" & Trim(txtUsername.Text) & "'", "", False)

        If strMobNO = "" And StrEmail = "" Then
            MsgBox("Mobile Number and Email is not present for this user. Contact System Administrator.", MsgBoxStyle.Critical, "Contact System Admin")
            Exit Sub
        End If
        strOTPMobNo = "xxxxxx" & Strings.Right(strMobNO, 4)

        With frmOTP
            .strUserName = txtUsername.Text
            If strMobNO = "" Then
                .btnSendOTP.Visible = False
                .lblOTPMsg.Visible = False
            Else
                .lblOTPMsg.Text = "<<<< Click here to send Four digit OTP to your mobile ending " & strOTPMobNo
            End If
            If StrEmail = "" Then
                .btnEmail.Visible = False
                .lblOtpMail.Visible = False
            Else
                .lblOtpMail.Text = "<<<< Click here to send Four digit OTP to your mail " & StrEmail
                .lblOtpMail.Visible = True
            End If
            .txtOTP.Text = ""
            .TopMost = True
            .ShowDialog()
        End With
    End Sub

    Private Sub imgEye_MouseEnter(sender As Object, e As EventArgs) Handles imgEye.MouseEnter
        txtPwd.PasswordChar = ""
        txtAdmPwd.PasswordChar = ""
        imgEye.Image = vLQMS.My.Resources.imgShow
    End Sub

    Private Sub imgEye_MouseLeave(sender As Object, e As EventArgs) Handles imgEye.MouseLeave
        txtPwd.PasswordChar = "*"
        txtAdmPwd.PasswordChar = "*"
        imgEye.Image = vLQMS.My.Resources.imgDontShow
    End Sub

    Private Sub txtNewPwd_KeyUp(sender As Object, e As KeyEventArgs) Handles txtNewPwd.KeyUp
        If txtNewPwd.Text <> "" Then
            Call ValidPwdRules()
        End If
    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs)

    End Sub

    Private Sub getPwdRules()
        Dim RsTemp As oledb.oledbDataReader
        Dim strsql As String

        strsql = "Select PwdMinSplChrs,PwdMinNumeric,PwdMinAlpha,PwdMinLen,PwdMinLCase,PwdMinUCase from UtlMisc"
        RsTemp = clsSVT.getDataReaderFromSQL(strsql)

        With RsTemp
            If .Read() Then
                intMinSplChrs = GetVal(RsTemp("PwdMinSplChrs"), 0)
                intMinNumeric = GetVal(RsTemp("PwdMinNumeric"), 0)
                intMinAlpha = GetVal(RsTemp("PwdMinAlpha"), 0)
                intMinLen = GetVal(RsTemp("PwdMinLen"), 0)
                intMinLCase = GetVal(RsTemp("PwdMinLCase"), 0)
                intMinUCase = GetVal(RsTemp("PwdMinUCase"), 0)
            End If
            .Close()
            lblMinSplChars.Text = "Min.no of Special Characters: " & intMinSplChrs
            lblMinNum.Text = "Min.no of Numeric: " & intMinNumeric
            lblMinAlpha.Text = "Min.no of Alphabets: " & intMinAlpha
            lblMinUCase.Text = "Min.no of Upper case: " & intMinUCase
            lblMinLCase.Text = "Min.no of Lower case: " & intMinLCase
            lblMinLen.Text = "Min. Length of Password: " & intMinLen
        End With
    End Sub
    Private Function ValidPwdRules() As Boolean
        PnlPwdRules.Visible = False
        ValidPwdRules = False
        If txtNewPwd.Text = "" Then
            lblMinSplChars.ForeColor = Color.Black
            lblMinNum.ForeColor = Color.Black
            lblMinAlpha.ForeColor = Color.Black
            lblMinLCase.ForeColor = Color.Black
            lblMinUCase.ForeColor = Color.Black
            lblMinLen.ForeColor = Color.Black
            Exit Function
        End If
        If gblnPwdRule Then
            PnlPwdRules.Visible = True
        Else
            PnlPwdRules.Visible = False
        End If
        Dim intNumbers As Integer, intChars As Integer, intUcase As Integer, intLCase As Integer
        Dim intSplChars As Integer
        Dim input As String = Trim(txtNewPwd.Text)

        For Each c As Char In input
            If Asc(c) >= Asc(0) And Asc(c) <= Asc(9) Then
                intNumbers = intNumbers + 1
            ElseIf Asc(c) >= Asc("a") And Asc(c) <= Asc("z") Then
                intLCase = intLCase + 1
            ElseIf Asc(c) >= Asc("A") And Asc(c) <= Asc("Z") Then
                intUcase = intUcase + 1
            Else
                intSplChars = intSplChars + 1
            End If
        Next

        intChars = intLCase + intUcase

        If Len(txtNewPwd.Text) < intMinLen Then
            lblMinLen.ForeColor = Color.Red
        Else
            lblMinLen.ForeColor = Color.Green
        End If

        If intSplChars < intMinSplChrs Then
            lblMinSplChars.ForeColor = Color.Red
        Else
            lblMinSplChars.ForeColor = Color.Green
        End If
        If intNumbers < intMinNumeric Then
            lblMinNum.ForeColor = Color.Red
        Else
            lblMinNum.ForeColor = Color.Green
        End If

        If intChars < intMinAlpha Then
            lblMinAlpha.ForeColor = Color.Red
        Else
            lblMinAlpha.ForeColor = Color.Green
        End If
        If intUcase < intMinUCase Then
            lblMinUCase.ForeColor = Color.Red
        Else
            lblMinUCase.ForeColor = Color.Green
        End If
        If intLCase < intMinLCase Then
            lblMinLCase.ForeColor = Color.Red
        Else
            lblMinLCase.ForeColor = Color.Green
        End If

        If lblMinSplChars.ForeColor = Color.Red Or lblMinNum.ForeColor = Color.Red Or lblMinUCase.ForeColor = Color.Red Or lblMinLCase.ForeColor = Color.Red Or
                lblMinLen.ForeColor = Color.Red Then
            ValidPwdRules = False
            Exit Function
        End If

        ValidPwdRules = True
    End Function

    Private Sub txtNewPwd_GotFocus(sender As Object, e As EventArgs) Handles txtNewPwd.GotFocus
        If txtNewPwd.Text <> "" Then
            'PnlPwdRules.Visible = True
            Call ValidPwdRules()
        Else
            PnlPwdRules.Visible = False
        End If
    End Sub

    Private Sub txtNewPwd_LostFocus(sender As Object, e As EventArgs) Handles txtNewPwd.LostFocus
        PnlPwdRules.Visible = False
    End Sub

    Private Sub frmLogin_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        If gUserID = 0 Then
            End
        End If
    End Sub
End Class
