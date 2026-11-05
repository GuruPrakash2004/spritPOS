Imports System.IO
Imports C1.Win.C1FlexGrid
Imports System.Net
Imports System.Net.Mail
Public Class frmUtlSMTPSettings

    Dim clsSVT As New svtLibrary

    Dim dlg As New OpenFileDialog()

    Dim strSHForm As String
    Dim strDocHdrTblName As String
    Dim strDocDtlTblName As String
    Dim strDocIDFldName As String
    Dim strDocNoFldName As String
    Dim strDocDateFldName As String

    Dim intPartyDocID As Int32
    Dim strPartyType As String
    Dim strPartyTblName As String
    Dim strFldPartyID As String
    Dim strFldPartyCode As String
    Dim strFldPartyName As String


    Private Sub btnSmtpSave_Click(sender As System.Object, e As System.EventArgs) Handles btnSmtpSave.Click
        'If Not validFlds() Then Exit Sub

        'Dim svtTrn As OleDb.OleDbTransaction = Nothing
        'svtTrn = svtConn.BeginTransaction(IsolationLevel.ReadCommitted)
        'Try
        '    clsSVT.execSQLStatement("Update UtlGSettings set SetValue = '" & Trim(txtSmtpHost.Text) & "' where SetName= 'SMTPHostName' and SetGroup = 'SMTP'", svtTrn)
        '    clsSVT.execSQLStatement("Update UtlGSettings set SetValue = '" & Trim(txtSmtpMailID.Text) & "' where SetName= 'SMTPMailID' and SetGroup = 'SMTP'", svtTrn)
        '    clsSVT.execSQLStatement("Update UtlGSettings set SetValue = '" & Trim(txtSmtpMailPwd.Text) & "' where SetName= 'SMTPMailPWD' and SetGroup = 'SMTP'", svtTrn)
        '    clsSVT.execSQLStatement("Update UtlGSettings set SetValue = '" & Trim(txtSmtpServerPort.Text) & "' where SetName= 'SMTPServerPort' and SetGroup = 'SMTP'", svtTrn)
        '    'strSql = "Update utlGSettings set "
        '    'strSql = strSql & "SMTPHostName = '" & Trim(txtSmtpHost.Text) & "',"
        '    'strSql = strSql & "SMTPMailID = '" & Trim(txtSmtpMailID.Text) & "',"
        '    'strSql = strSql & "SMTPMailPwd = '" & Trim(txtSmtpMailPwd.Text) & "',"
        '    'strSql = strSql & "SMTPServerPort = " & Val(txtSmtpServerPort.Text) & ""

        '    If chkSSL.Checked Then
        '        clsSVT.execSQLStatement("Update UtlGSettings set SetValue = 'Y' where SetName= 'EnableSSL' and SetGroup = 'SMTP'", svtTrn)
        '    Else
        '        clsSVT.execSQLStatement("Update UtlGSettings set SetValue = 'N' where SetName= 'EnableSSL' and SetGroup = 'SMTP'", svtTrn)
        '    End If

        '    svtTrn.Commit()
        '    MsgBox("Updated smtp details successfully!", vbInformation, Me.Text)
        'Catch ex As Exception
        '    MsgBox("Error while updating smtp details!" & vbCrLf & ex.Message, vbCritical, Me.Text)
        'End Try
    End Sub


    Private Function validFlds() As Boolean
        validFlds = False

        If Trim(txtSmtpHost.Text) = "" Then
            MsgBox("Please enter host name!", vbInformation, Me.Text)
            txtSmtpHost.Focus()
            Exit Function
        End If
        If Trim(txtSmtpMailID.Text) = "" Then
            MsgBox("Please enter smtp email ID!", vbInformation, Me.Text)
            txtSmtpMailID.Focus()
            Exit Function
        End If
        If Trim(txtSmtpMailPwd.Text) = "" Then
            MsgBox("Please enter smtp email password!", vbInformation, Me.Text)
            txtSmtpMailPwd.Focus()
            Exit Function
        End If
        If Trim(txtSmtpHost.Text) = "" Then
            MsgBox("Please enter host name!", vbInformation, Me.Text)
            txtSmtpHost.Focus()
            Exit Function
        End If
        validFlds = True
    End Function
    Private Sub ClearFlds()
        txtSmtpHost.Text = ""
        txtSmtpMailID.Text = ""
        txtSmtpMailPwd.Text = ""
        txtSmtpServerPort.Text = ""
        txtTestMailID.Text = ""
        chkSSL.Checked = False
    End Sub

    Private Sub btnSmtpClose_Click(sender As System.Object, e As System.EventArgs) Handles btnSmtpClose.Click
        'ClearFlds()
        'Me.Close()
    End Sub

    Private Sub frmUtlSMTPSettings_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        Call fillFormProperties(Me)
        Call ChangeFontSize(Me)
        fillSmtpDtls()
        txtSmtpHost.Focus()
    End Sub

    Private Sub fillSmtpDtls()
        Dim strSql As String
        Dim rs As OleDb.OleDbDataReader

        strSql = "SELECT "
        strSql &= " max(case when SetName='SMTPHostName' then SetVAlue else '' end) as SMTPHostName , "
        strSql &= " max(case when SetName='SMTPMailID' then SetVAlue else '' end) as SMTPMAILID , "
        strSql &= " max(case when SetName='SMTPMailPWD' then SetVAlue else '' end) as SMTPMAILPWD, "
        strSql &= " max(case when SetName='SMTPServerPort' then SetVAlue else '' end) as SMTPServerPort, "
        strSql &= " max(case when SetName='EnableSSL' then SetVAlue else '' end) as EnableSSL  "
        strSql &= " FROM UtlGSettings "

        rs = clsSVT.getDataReaderFromSQL(strSql)

        If rs.Read() Then
            txtSmtpHost.Text = GetVal(rs("SMTPHostName"), "")
            txtSmtpMailID.Text = GetVal(rs("SMTPMailID"), "")
            txtSmtpMailPwd.Text = GetVal(rs("SMTPMAILPWD"), "")
            txtSmtpServerPort.Text = GetVal(rs("SMTPServerPort"), "")

            If rs("EnableSSL") = "Y" Then
                chkSSL.Checked = True
            Else
                chkSSL.Checked = False
            End If
        End If
        rs.Close()
    End Sub

    Private Sub btnTestMail_Click(sender As Object, e As EventArgs) Handles btnTestMail.Click

        'Try
        '    If Trim(txtSmtpHost.Text) = String.Empty Then
        '        MsgBox("SMTP Host Name is Empty.. Please Enter the Host Name..", MsgBoxStyle.Exclamation, Me.Text)
        '        txtSmtpHost.Focus()
        '        Exit Sub
        '    End If

        '    If Trim(txtSmtpMailID.Text) = "" Then
        '        MsgBox("SMTP MailID is Empty.. Please Enter the SMTP Mail-ID", MsgBoxStyle.Exclamation, Me.Text)
        '        txtSmtpMailID.Focus()
        '        Exit Sub
        '    End If

        '    If Trim(txtSmtpMailPwd.Text) = String.Empty Then
        '        MsgBox("Password is Empty.. Please Enter the Password..", MsgBoxStyle.Exclamation, Me.Text)
        '        txtSmtpMailPwd.Focus()
        '        Exit Sub
        '    End If

        '    If Trim(txtSmtpServerPort.Text) = String.Empty Then
        '        MsgBox("Server Port is Empty.. Please Enter the Server Port Number..", MsgBoxStyle.Exclamation, Me.Text)
        '        txtSmtpServerPort.Focus()
        '        Exit Sub
        '    End If

        '    Dim strToEmail, strSubject, strBody As String
        '    strSubject = "Test Email"
        '    strBody = "Test Email"

        '    Dim Smtp_Server As New Net.Mail.SmtpClient
        '    Dim e_mail As New Net.Mail.MailMessage()
        '    Smtp_Server.UseDefaultCredentials = False
        '    Smtp_Server.Credentials = New Net.NetworkCredential(CStr(Trim(txtSmtpMailID.Text) & ""), CStr(Trim(txtSmtpMailPwd.Text) & ""))
        '    Smtp_Server.Port = CStr(Trim(txtSmtpServerPort.Text) & "")
        '    If CStr(gSmtpEnableSSL & "") = "Y" Then
        '        Smtp_Server.EnableSsl = True
        '    Else
        '        Smtp_Server.EnableSsl = False
        '    End If
        '    Smtp_Server.Host = CStr(Trim(txtSmtpHost.Text) & "")
        '    e_mail.From = New Net.Mail.MailAddress(CStr(Trim(txtSmtpMailID.Text) & ""))
        '    strToEmail = Trim(txtSmtpMailID.Text)
        '    e_mail.To.Add(strToEmail)
        '    e_mail.Subject = strSubject
        '    e_mail.IsBodyHtml = False
        '    e_mail.Body = strBody

        '    Smtp_Server.Send(e_mail)
        '    MsgBox("Email Sent....", MsgBoxStyle.Information, Me.Text)
        'Catch ex As Exception
        '    MsgBox("Error in SMTP eMail!" & vbCrLf & ex.Message, MsgBoxStyle.Critical, "ERROR")
        'End Try
        ''End If
        ''Catch ex As Exception
        ''MsgBox("Error in Sending Mail!" & vbCrLf & ex.Message, MsgBoxStyle.Critical, "ERROR")
        ''End Try

        Try
            If Trim(txtSmtpHost.Text) = String.Empty Then
                MsgBox("SMTP Host Name is Empty.. Please Enter the Host Name..", MsgBoxStyle.Exclamation, Me.Text)
                txtSmtpHost.Focus()
                Exit Sub
            End If

            If Trim(txtSmtpMailID.Text) = "" Then
                MsgBox("SMTP MailID is Empty.. Please Enter the SMTP Mail-ID", MsgBoxStyle.Exclamation, Me.Text)
                txtSmtpMailID.Focus()
                Exit Sub
            End If

            If Trim(txtSmtpMailPwd.Text) = String.Empty Then
                MsgBox("Password is Empty.. Please Enter the Password..", MsgBoxStyle.Exclamation, Me.Text)
                txtSmtpMailPwd.Focus()
                Exit Sub
            End If

            If Trim(txtSmtpServerPort.Text) = String.Empty Then
                MsgBox("Server Port is Empty.. Please Enter the Server Port Number..", MsgBoxStyle.Exclamation, Me.Text)
                txtSmtpServerPort.Focus()
                Exit Sub
            End If

            If Trim(txtTestMailID.Text) = String.Empty Then
                txtTestMailID.Text = Trim(txtSmtpMailID.Text)
            End If

            Dim strToEmail, strSubject, strBody As String
            strSubject = "Test Email"
            strBody = "Test Email"

            '    Dim Smtp_Server As New Net.Mail.SmtpClient
            '    Dim e_mail As New Net.Mail.MailMessage()
            '    Smtp_Server.UseDefaultCredentials = False
            '    Smtp_Server.Credentials = New Net.NetworkCredential(CStr(Trim(txtSmtpMailID.Text) & ""), CStr(Trim(txtSmtpMailPwd.Text) & ""))
            '    Smtp_Server.Port = CStr(Trim(txtSmtpServerPort.Text) & "")
            '    If CStr(gSmtpEnableSSL & "") = "Y" Then
            '        Smtp_Server.EnableSsl = True
            '    Else
            '        Smtp_Server.EnableSsl = False
            '    End If
            '    Smtp_Server.Host = CStr(Trim(txtSmtpHost.Text) & "")
            '    e_mail.From = New Net.Mail.MailAddress(CStr(Trim(txtSmtpMailID.Text) & ""))
            '    strToEmail = Trim(txtSmtpMailID.Text)
            '    e_mail.To.Add(strToEmail)
            '    e_mail.Subject = strSubject
            '    e_mail.IsBodyHtml = False
            '    e_mail.Body = strBody

            '    Smtp_Server.Send(e_mail)
            '    MsgBox("Email Sent....", MsgBoxStyle.Information, Me.Text)

            'Create the msg object to be sent
            Dim msg As New MailMessage()
            '//Add your email address to the recipients
            msg.To.Add(txtTestMailID.Text)

            '//Configure the address we are sending the mail from
            Dim address As New MailAddress(txtSmtpMailID.Text) ' (mail@softvent.com)
            msg.From = address
            msg.Subject = "subject " & Now.ToString
            msg.Body = "body"

            '//Configure an SmtpClient to send the mail.
            Dim client As New SmtpClient()
            client.DeliveryMethod = SmtpDeliveryMethod.Network
            If chkSSL.Checked Then
                client.EnableSsl = True
            Else
                client.EnableSsl = False
            End If
            client.Host = txtSmtpHost.Text ' "mail.softvent.com"
            client.Port = txtSmtpServerPort.Text '25

            If chkTLS.Checked = True Then
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
            End If

            '//Setup credentials to login to our sender email address ("UserName", "Password")
            Dim credentials As New NetworkCredential(txtSmtpMailID.Text, txtSmtpMailPwd.Text)
            client.UseDefaultCredentials = True
            client.Credentials = credentials

            '//Send the msg
            client.Send(msg)

            MsgBox("Test Mail Sent Successfully!", MsgBoxStyle.Information, Me.Text)
        Catch ex As Exception
            MsgBox(txtSmtpMailID.Text & "," & txtSmtpMailPwd.Text & "," & txtSmtpServerPort.Text & "," & chkssl.checked & "," & txtSmtpHost.Text)
            MsgBox("Error in SMTP eMail!" & vbCrLf & ex.Message, MsgBoxStyle.Critical, "ERROR")
        End Try

    End Sub

    Private Function getEmailDtls(ByVal argDocID As Int32, ByVal argOrder As Int32, ByVal argEmailType As String, ByVal argTrnID As Int32) As String
        getEmailDtls = ""

        Dim rs As OleDb.OleDbDataReader
        rs = clsSVT.getDataReaderFromSQL("Select * from UtlOptionEmailDtls Where DocID=" & argDocID & " and OptionOrder=" & argOrder & " and emailType='" & argEmailType & "' Order by Slno")

        Dim strEmailID As String = ""
        If rs.HasRows Then
            Do While rs.Read
                If GetVal(rs.Item("FixedEmail"), "") = "Y" Then
                    strEmailID &= GetVal(rs.Item("EmailID"), "") & ";"
                Else

                    Dim strQry As String
                    strQry = "Select "
                    strQry &= GetVal(rs.Item("TxnFldName"), "")
                    strQry &= " From " & strDocHdrTblName
                    If strDocDtlTblName <> "" Then
                        strQry &= "," & strDocDtlTblName
                    End If
                    If strPartyType <> "" Then
                        strQry &= "," & strPartyTblName
                    End If

                    strQry &= " Where 1=1 "
                    If strDocDtlTblName <> "" Then
                        'strQry &= " and " & strDocHdrTblName & "." & strDocIDFldName & "=" & strDocHdrTblName & "." & strDocIDFldName
                        strQry &= " and " & strDocHdrTblName & "." & strDocIDFldName & "=" & strDocDtlTblName & "." & strDocIDFldName
                    End If
                    strQry &= " and " & " " & GetVal(rs.Item("TxnCondition"), "")

                    strQry = strQry.Replace("<<" & UCase(strDocIDFldName) & ">>", argTrnID)
                    strQry = strQry.Replace("<<" & UCase(strDocHdrTblName) & " ." & UCase(strDocIDFldName) & ">>", argTrnID)
                    strQry = strQry.Replace("`", "'")
                    strEmailID = clsSVT.getDataStringValue(strQry, "", False) & ";"
                End If
            Loop
        End If
        rs.Close()
        If strEmailID <> "" Then
            strEmailID = Mid(strEmailID, 1, Len(strEmailID) - 1)
        End If
        Return strEmailID
    End Function

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        If Not validFlds() Then Exit Sub

        Dim svtTrn As OleDb.OleDbTransaction = Nothing
        svtTrn = svtConn.BeginTransaction(IsolationLevel.ReadCommitted)
        Try
            clsSVT.execSQLStatement("Update UtlGSettings set SetValue = '" & Trim(txtSmtpHost.Text) & "' where SetName= 'SMTPHostName' and SetGroup = 'SMTP'", svtTrn)
            clsSVT.execSQLStatement("Update UtlGSettings set SetValue = '" & Trim(txtSmtpMailID.Text) & "' where SetName= 'SMTPMailID' and SetGroup = 'SMTP'", svtTrn)
            clsSVT.execSQLStatement("Update UtlGSettings set SetValue = '" & Trim(txtSmtpMailPwd.Text) & "' where SetName= 'SMTPMailPWD' and SetGroup = 'SMTP'", svtTrn)
            clsSVT.execSQLStatement("Update UtlGSettings set SetValue = '" & Trim(txtSmtpServerPort.Text) & "' where SetName= 'SMTPServerPort' and SetGroup = 'SMTP'", svtTrn)
            If chkSSL.Checked Then
                clsSVT.execSQLStatement("Update UtlGSettings set SetValue = 'Y' where SetName= 'EnableSSL'", svtTrn)
            Else
                clsSVT.execSQLStatement("Update UtlGSettings set SetValue = 'N' where SetName= 'EnableSSL'", svtTrn)
            End If

            'strSql = "Update utlGSettings set "
            'strSql = strSql & "SMTPHostName = '" & Trim(txtSmtpHost.Text) & "',"
            'strSql = strSql & "SMTPMailID = '" & Trim(txtSmtpMailID.Text) & "',"
            'strSql = strSql & "SMTPMailPwd = '" & Trim(txtSmtpMailPwd.Text) & "',"
            'strSql = strSql & "SMTPServerPort = " & Val(txtSmtpServerPort.Text) & ""

            If chkSSL.Checked Then
                clsSVT.execSQLStatement("Update UtlGSettings set SetValue = 'Y' where SetName= 'EnableSSL' and SetGroup = 'SMTP'", svtTrn)
            Else
                clsSVT.execSQLStatement("Update UtlGSettings set SetValue = 'N' where SetName= 'EnableSSL' and SetGroup = 'SMTP'", svtTrn)
            End If

            svtTrn.Commit()
            MsgBox("Updated smtp details successfully!", vbInformation, Me.Text)
        Catch ex As Exception
            MsgBox("Error while updating smtp details!" & vbCrLf & ex.Message, vbCritical, Me.Text)
        End Try
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        ClearFlds()
        Me.Close()
    End Sub
End Class