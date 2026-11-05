Imports System.Data.OleDb
Imports Microsoft.Office

Public Class frmUtlCompKey
    Dim clsSVT As New svtLibrary
    Dim Mode As String
    Public strPartyIDFld As String
    Public strfrmCaption As String
    Dim intCustID As Integer

    Private Sub frmMstAddress_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        KeyPreview = True
        txtCmpAddr.Text = gSettings.gCompAdd1
        txtCmpName.Text = gSettings.gCompName
    End Sub



    Private Sub btnSave_ClickButtonArea(Sender As System.Object, e As System.Windows.Forms.MouseEventArgs)
        Dim strSQL As String
        Dim svtTrn As OleDb.OleDbTransaction

        If getCheckSum(getCleanString(txtCmpName.Text)) <> txtNameKey.Text Or
           getCheckSum(getCleanString(txtCmpAddr.Text)) <> txtAddrKey.Text Then
            MsgBox("Sorry. Invalid Key!", vbCritical, Me.Text)
            End
        End If

        strSQL = "Update MstComp set CompName = '" & txtCmpName.Text & "',"
        strSQL = strSQL & " Address1 = '" & txtCmpAddr.Text & "'"
        strSQL = strSQL & " where CompNo = " & gSettings.gCompNo
        svtTrn = svtConn.BeginTransaction(IsolationLevel.ReadCommitted)
        Try
            If Not clsSVT.execSQLStatement(strSQL, svtTrn) Then
                svtTrn.Rollback()
                Exit Sub
            End If

            strSQL = "Update UtlGlobe set CompNameKey = '" & txtNameKey.Text & "',"
            strSQL = " CompAddKey = '" & txtAddrKey.Text & "'"

            If Not clsSVT.execSQLStatement(strSQL, svtTrn) Then
                svtTrn.Rollback()
                Exit Sub
            End If
            MsgBox("Thank you. Updated Successfully! You can use the software now!!", vbExclamation, Me.Text)
        Catch ex As Exception
            svtTrn.Rollback()
            MsgBox("Error during Saving." & vbCrLf & ex.Message, MsgBoxStyle.Critical, Me.Text)
            Exit Sub
        End Try
    End Sub

    Private Sub btnSend_ClickButtonArea(Sender As System.Object, e As System.Windows.Forms.MouseEventArgs) Handles btnSend.ClickButtonArea
        Call setEmailSend("", Trim(txtCmpName.Text) & vbCrLf & Trim(txtCmpAddr.Text), "softvent@gmail.com", "", "", "")
    End Sub

    Private Sub setEmailSend(sSubject As String, sBody As String, _
                         sTo As String, sCC As String, _
                         sFilename As String, sDisplayname As String)
        Dim Outl As Object
        Outl = CreateObject("Outlook.Application")

        Dim oApp As Interop.Outlook._Application
        oApp = New Interop.Outlook.Application

        Dim oMsg As Interop.Outlook._MailItem
        oMsg = oApp.CreateItem(Interop.Outlook.OlItemType.olMailItem)

        Dim omsg1 As Object
        omsg1 = Outl.CreateItem(0)
        oMsg.Subject = sSubject
        oMsg.Body = sBody

        oMsg.To = sTo
        oMsg.CC = sCC

        Dim strS As String = sFilename
        Dim strN As String = sDisplayname
        If sFilename <> "" Then
            Dim sBodyLen As Integer = Int(sBody.Length)
            Dim oAttachs As Interop.Outlook.Attachments = oMsg.Attachments
            Dim oAttach As Interop.Outlook.Attachment

            oAttach = oAttachs.Add(strS, , sBodyLen, strN)

        End If
        oMsg.Display(True)
        'oMsg.Send()
        'MessageBox.Show("Email Send", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

    End Sub

    Private Sub btnCancelKey_Click(sender As Object, e As EventArgs) Handles btnCancelKey.Click
        Me.Close()
    End Sub

    Private Sub btnSaveKey_Click(sender As Object, e As EventArgs) Handles btnSaveKey.Click
        Dim strSQL As String
        Dim svtTrn As OleDb.OleDbTransaction

        If getCheckSum(getCleanString(txtCmpName.Text)) <> txtNameKey.Text Or
           getCheckSum(getCleanString(txtCmpAddr.Text)) <> txtAddrKey.Text Then
            MsgBox("Sorry. Invalid Key!", vbCritical, Me.Text)
            End
        End If

        strSQL = "Update MstComp set CompName = '" & txtCmpName.Text & "',"
        strSQL = strSQL & " Address1 = '" & txtCmpAddr.Text & "'"
        strSQL = strSQL & " where CompNo = " & gSettings.gCompNo
        svtTrn = svtConn.BeginTransaction(IsolationLevel.ReadCommitted)
        Try
            If Not clsSVT.execSQLStatement(strSQL, svtTrn) Then
                svtTrn.Rollback()
                Exit Sub
            End If

            strSQL = "Update UtlGlobe set CompNameKey = '" & txtNameKey.Text & "',"
            strSQL = " CompAddKey = '" & txtAddrKey.Text & "'"

            If Not clsSVT.execSQLStatement(strSQL, svtTrn) Then
                svtTrn.Rollback()
                Exit Sub
            End If
            MsgBox("Thank you. Updated Successfully! You can use the software now!!", vbExclamation, Me.Text)
        Catch ex As Exception
            svtTrn.Rollback()
            MsgBox("Error during Saving." & vbCrLf & ex.Message, MsgBoxStyle.Critical, Me.Text)
            Exit Sub
        End Try
    End Sub



    'Private Sub btnCancel_ClickButtonArea(Sender As System.Object, e As System.Windows.Forms.MouseEventArgs) Handles btnCancel.ClickButtonArea
    '    Me.Close()
    'End Sub


End Class