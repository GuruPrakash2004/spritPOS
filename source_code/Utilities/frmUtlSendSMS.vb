Imports System.ComponentModel
Imports System.IO
Imports C1.Win.C1FlexGrid

Public Class frmUtlSendSMS
    Dim strSQL As String
    Dim clsSVT As New svtLibrary
    Const vDetID = 0
    Const vDetProvider = 1
    Const vDetDefault = 2
    Const vDetDailyLimit = 3
    Const vDetURL = 4

    Private Sub btnSave_Click_1(sender As Object, e As EventArgs) Handles btnSave.Click
        If Not validFlds() Then Exit Sub
        Dim strSQL As String
        'save entry
        If Val(txtID.Text) <= 0 Then
            txtID.Text = clsSVT.AutoIncrementedNum("RowID", "UtlMsgSettings")
            strSQL = "Insert into UtlMsgSettings (RowID, Provider, DailyLimit, SMSUrl, Def) values ("
            strSQL = strSQL & Val(txtID.Text) & ","
            strSQL = strSQL & "'" & txtCode.Text & "',"
            strSQL = strSQL & Val(txtDailyLimit.Text) & ","
            strSQL = strSQL & "'" & Trim(txtSMSURL.Text) & "',"
            strSQL = strSQL & getCheckString(chkDefault, True) & ")"
        Else
            strSQL = "Update UtlMsgSettings set "
            strSQL = strSQL & " Provider = '" & txtCode.Text & "',"
            strSQL = strSQL & " DailyLimit  = " & Val(txtDailyLimit.Text) & ","
            strSQL = strSQL & " SMSUrl = '" & Trim(txtSMSURL.Text) & "',"
            strSQL = strSQL & " Def = " & getCheckString(chkDefault, True)
            strSQL = strSQL & " Where RowID = " & Val(txtID.Text)
        End If
        Try
            clsSVT.execSQLStatement(strSQL)
            If chkDefault.Checked Then
                clsSVT.execSQLStatement("Update UtlMsgSettings set Def = 'N' where RowID <> " & Val(txtID.Text))
            End If
            MsgBox("Updated Provider details successfully!", vbInformation, Me.Text)
            Call fillSmsDtls()
        Catch ex As Exception
            MsgBox("Error while updating Provider details!" & vbCrLf & ex.Message, vbCritical, Me.Text)
        End Try
        Call clearFlds()
        pnlGrid.Enabled = True
    End Sub

    Private Function validFlds() As Boolean
        validFlds = False
        Dim blnFound As Boolean
        If Trim(txtCode.Text) = "" Then
            MsgBox("Please enter Provider!", vbInformation, Me.Text)
            txtCode.Focus()
            Exit Function
        End If
        If Trim(txtDailyLimit.Text) = "" Then
            MsgBox("Please enter Daily Limit of messages!", vbInformation, Me.Text)
            txtDailyLimit.Focus()
            Exit Function
        End If
        If Trim(txtSMSURL.Text) = "" Then
            MsgBox("Please enter SMS URL!", vbInformation, Me.Text)
            txtSMSURL.Focus()
            Exit Function
        End If

        Call clsSVT.getDataStringValue("Select provider from UtlMsgSettings where Provider = '" & Trim(txtCode.Text) & "' and RowID <> " & Val(txtID.Text), "", blnfound)
        If blnFound Then
            MsgBox("Provider already exists..", MsgBoxStyle.Information, Me.Text)
            txtCode.Text = ""
            txtCode.Focus()
            Exit Function
        End If

        validFlds = True
    End Function

    Private Sub Send_SMS_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.KeyPreview = True
        With vsfDet
            .Rows.Count = 1
            .Cols.Count = 5
            .SetData(0, vDetProvider, "Provider")
            .SetData(0, vDetDailyLimit, "Daily Limit")
            .SetData(0, vDetDefault, "Default")
            .SetData(0, vDetURL, "URL")
        End With
        Call fillFormProperties(Me)
        fillSmsDtls()
        fraEntry.Enabled = False
        pnlGrid.Enabled = True
        initFormSize(Me, Me.Text, False, 1097, 530, 150, 150)
        initFormVsfDet(Me, Me.Text, vsfDet, "DET")
    End Sub

    Private Sub fillSmsDtls()
        Dim strSql As String
        Dim rs As oledb.oledbDataReader

        strSql = "Select * from UtlMsgSettings order by RowID"

        rs = clsSVT.getDataReaderFromSQL(strSql)
        vsfDet.Rows.Count = 1
        Do While rs.Read
            With vsfDet
                .Rows.Count = .Rows.Count + 1
                .SetData(.Rows.Count - 1, vDetID, rs!RowID)
                .SetData(.Rows.Count - 1, vDetProvider, rs!Provider)
                .SetData(.Rows.Count - 1, vDetDailyLimit, rs!DailyLimit)
                .SetData(.Rows.Count - 1, vDetDefault, rs!Def)
                .SetData(.Rows.Count - 1, vDetURL, rs!SMSURL)
            End With
        Loop
        Call clearFlds()
        rs.Close()
    End Sub

    Private Sub clearFlds()
        txtID.Text = ""
        txtCode.Text = ""
        txtDailyLimit.Text = ""
        txtSMSURL.Text = ""
        chkDefault.Checked = False
        btnSave.Visible = False
        btnClose.Visible = False
        pnlTestSMS.Visible = False
    End Sub

    Private Sub btnRemoveRow_Click(sender As Object, e As EventArgs) Handles btnRemoveRowAUOM.Click
        With vsfDet
            If .Row <= 0 Then Exit Sub
            If Val(.GetData(.Row, vDetID)) <= 0 Then Exit Sub
            If MsgBox("Do you want to delete this provider settings?", MsgBoxStyle.Critical + MsgBoxStyle.YesNo, "Delete ?") = MsgBoxResult.No Then Exit Sub
            clsSVT.execSQLStatement("Delete from UtlMsgSettings where RowID = " & Val(.GetData(.Row, vDetID)))
            .Rows.Remove(.Row)
        End With
    End Sub

    Private Sub txtDailyLimit_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtDailyLimit.KeyPress
        If KeyValidate("Decimal", e.KeyChar) Then
            e.Handled = False
        Else
            e.Handled = True
        End If
    End Sub

    Private Sub frmUtlSendSMS_KeyUp(sender As Object, e As KeyEventArgs) Handles Me.KeyUp
        If e.KeyCode = Keys.F9 And e.Control = True Then
            Call initFormSize(Me, Me.Text, True, 514, 489, 0, 0)
            MsgBox("Restored Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
        If e.KeyCode = Keys.F8 And e.Control = True Then
            Call SaveActFormSettings(Me, Me.Text)
            Call SaveActFormVsfDet(Me, Me.Text, "DET", vsfDet)
            MsgBox("Saved Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
    End Sub

    Private Sub btnDetVertical_Click(sender As Object, e As EventArgs) Handles btnDetVertical.Click
        fraEntry.Enabled = True
        Call clearFlds()
        txtCode.Focus()
        pnlGrid.Enabled = False
        btnSave.Visible = True
        btnClose.Visible = True
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Call clearFlds()
        pnlGrid.Enabled = True
        fraEntry.Enabled = False
    End Sub
    Private Sub vsfDet_DoubleClick(sender As Object, e As EventArgs) Handles vsfDet.DoubleClick
        With vsfDet
            If .Row <= 0 Then Exit Sub
            If Val(.GetData(.Row, vDetID)) <= 0 Then Exit Sub
            Call clearFlds()
            txtID.Text = .GetData(.Row, vDetID)
            txtSMSURL.Text = .GetData(.Row, vDetURL)
            txtCode.Text = .GetData(.Row, vDetProvider)
            txtDailyLimit.Text = .GetData(.Row, vDetDailyLimit)
            Call setCheckBox(chkDefault, .GetData(.Row, vDetDefault))
            btnSave.Visible = True
            btnClose.Visible = True
            fraEntry.Enabled = True
            txtCode.Focus()
            pnlGrid.Enabled = False
            pnlTestSMS.Visible = True
        End With
    End Sub

    Private Sub btnTestSMS_Click(sender As Object, e As EventArgs) Handles btnTestSMS.Click
        If Trim(txtMobile.Text) = "" Then
            MsgBox("Mobile No. is Empty....", MsgBoxStyle.Critical, Me.Text)
            Exit Sub
        End If

        If Not IsNumeric(txtMobile.Text) Then
            MsgBox("Invalid Mobile No...", MsgBoxStyle.Critical, Me.Text)
            Exit Sub
        End If

        If txtMobile.Text.Length <> 10 Then
            MsgBox("Invalid Mobile No...", MsgBoxStyle.Critical, Me.Text)
            Exit Sub
        End If

        If Val(txtID.Text) <= 0 Then
            MsgBox("Invalid Provider...", MsgBoxStyle.Critical, Me.Text)
            Exit Sub
        End If
        Dim strMsg As String = ""
        strMsg = "Test SMS" & vbNewLine
        strMsg &= "From Provider: " & clsSVT.getDataStringValue("Select Provider From utlMsgSettings Where RowID=" & Val(txtID.Text), "", False) & vbNewLine
        strMsg &= "Sending Time:" & CStr(Format(Date.Now, "dd-MMM-yyyy HH:mm:ss tt"))
        If SendSMS(strMsg, txtMobile.Text, clsSVT.getDataStringValue("Select Provider From utlMsgSettings Where RowID=" & Val(txtID.Text), "", False)) Then
            MsgBox("Test Message Sent....", MsgBoxStyle.Information, Me.Text)
        Else
            MsgBox("Failed to Sent Message...", MsgBoxStyle.Information, Me.Text)
        End If
    End Sub

    Private Sub btnOpenHistory_Click(sender As Object, e As EventArgs) Handles btnOpenHistory.Click
        Dim argfrm As New frmRptSMSHistory
        argfrm.ShowDialog()
    End Sub

    Private Sub btnInvSMS_Click(sender As Object, e As EventArgs) Handles btnInvSMS.Click
        frmUtlInvSMS.ShowDialog()
    End Sub
End Class