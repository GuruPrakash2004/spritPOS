
Imports System.ComponentModel
Imports C1.Win.C1FlexGrid
Imports System.IO
Imports Microsoft.Office

Public Class frmTrnEmail
    Dim clsSVT As New svtLibrary

    Public argForm As Object
    Public intDocId As Integer

    Dim strFormCaption As String
    Dim strSHForm As String
    Dim strDocHdrTblName As String
    Dim strDocDtlTblName As String
    Dim strDocIDFldName As String
    Dim strDocNoFldName As String
    Dim strDocDateFldName As String
    Dim strQtyFldName As String

    Dim blnFromQuery As Boolean
    Dim strPrintOptionName As String
    Dim strPDFFileName As String
    Dim blnOutlook As Boolean


    Public Sub frmTrnEmail_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.KeyPreview = True
    End Sub

    Public Sub initScreen(ByVal argCustid As Long)
        Dim strFirstSender As String = ""
        Call ClearFlds()
        txtTranID.Text = argCustid

        Call bindCombo(cboTemplate, "Template", "Template", "Select Distinct Template from UtlEmailTemplate where DocID = " & intDocId & " order by 1")
        cboTemplate.Text = ""

        Call initForm(intDocId)
        Call fillFormProperties(Me)
        Call initGridStyles(Me)
        Call ChangeFontSize(Me)
    End Sub

    Public Sub getUtlGlobeData()
        Dim strBtnProp As String = ""
        Dim strPnlcolor As String = ""
        Dim rs As oledb.oledbDataReader
        rs = clsSVT.getDataReaderFromSQL("Select * from UtlGSettings")
        With rs
            If Not .HasRows Then
                .Close()
                Exit Sub
            End If
            If .Read Then
                gIntDailyLimit = GetVal(rs.Item("DailyLimit"), 0)
                gIntLimit = GetVal(rs.Item("TotLimit"), 0)
                gIntMsgLen = GetVal(rs.Item("SingleMsgLength"), 0)
                gFormColor = GetVal(rs.Item("FormBGColor"), "")

                'gSENDER = GetVal(rs.Item("SENDER"), "")
                gSMSURL = GetVal(rs.Item("SMSURL"))

                strBtnProp = GetVal(rs.Item("ButtonProp"), "")
                If strBtnProp <> "" Then
                    Dim strArr() As String = strBtnProp.Split("|")
                    gBtnForeColor = Color.FromArgb(Convert.ToInt32(strArr(0)))
                    gBtnBackColor = Color.FromArgb(Convert.ToInt32(strArr(1)))
                    gBtnFlatStyle = UCase(strArr(2))
                Else
                    gBtnForeColor = Color.White
                    gBtnBackColor = Color.Teal
                    gBtnFlatStyle = FlatStyle.Standard
                End If
                strPnlcolor = GetVal(rs.Item("PnlColor"), "")
                gPnlBackColor = Color.FromArgb(Convert.ToInt32(strPnlcolor))
            End If
        End With
    End Sub

    Private Sub ClearFlds()
        cboTemplate.Text = ""
        txtTo.Text = ""
        txtCC.Text = ""
        txtSub.Text = ""
        txtBody.Text = ""
        txtAttach.Text = ""
    End Sub

    Private Function validFields() As Boolean
        validFields = False

        If Trim(txtTo.Text) = "" Then
            MsgBox("To is Empty....", MsgBoxStyle.Exclamation, Me.Text)
            txtBody.Focus()
            Exit Function
        End If

        If Trim(txtBody.Text) = "" Then
            MsgBox("Body is Empty....", MsgBoxStyle.Exclamation, Me.Text)
            txtBody.Focus()
            Exit Function
        End If

        validFields = True
    End Function

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub btnClr_Click(sender As Object, e As EventArgs)
        Call ClearFlds()
    End Sub

    Private Sub txtBody_TextChanged(sender As Object, e As EventArgs) Handles txtBody.TextChanged
        lblCount.Text = "Body Length: " & Len(txtBody.Text)
        TextBoxSizeChanged(txtBody, e)
    End Sub

    Public Sub setfldvalues(ByVal argFldCaption As String)
        txtBody.Text = argFldCaption
    End Sub

    Private Sub frmTrnEmail_KeyUp(sender As Object, e As KeyEventArgs) Handles Me.KeyUp
        If e.KeyCode = Keys.F9 And e.Control Then
            '958, 579
            Call initFormSize(Me, "TrnEmail", True, 650, 620, 0, 0)
            'Call SetColWidthFromString(vsfDet, strDocStdDtlWidth)
            MsgBox(("Restored Form & Grid Layout Successfully!!"), vbInformation, Me.Text)
        End If

        If e.KeyCode = Keys.F8 And e.Control = True Then
            Call SaveActFormSettings(Me, "TrnEmail")
            MsgBox(("Saved Form & Grid Layout Successfully!!"), vbInformation, Me.Text)
        End If

        If e.KeyCode = Keys.F6 Then
            '  Call btnSend_Click(sender, e)
        End If
    End Sub

    Private Sub cboTemplate_Validating(sender As Object, e As CancelEventArgs) Handles cboTemplate.Validating
        If Trim(cboTemplate.Text) = "" Then Exit Sub
        Dim blnFound As Boolean

        Call clsSVT.getDataStringValue("Select Template from UtlEmailTemplate where DocID=" & intDocId & " and Template ='" & cboTemplate.Text & "'", "", blnFound)

        If Not blnFound Then
            cboTemplate.Text = ""
            Exit Sub
        End If

        Call fillDtls()
    End Sub

    Private Sub btnTemplateSettings_Click(sender As Object, e As EventArgs) Handles btnTemplateSettings.Click
        Dim obj As New frmUtlTrnFldsEmailTmp
        With obj
            .Text = "Email Template"
            .intDocID = intDocId
            .ShowDialog()
        End With
        Call bindCombo(cboTemplate, "Template", "Template", "Select Distinct Template from UtlEmailTemplate where DocID = " & intDocId & " order by 1")
    End Sub

    Private Sub initForm(ByVal argDocID As Integer)
        If argDocID > 0 Then intDocId = argDocID
        Dim rs As oledb.oledbDataReader
        rs = clsSVT.getDataReaderFromSQL(" Select * from UtlTransactions where DocID = " & argDocID)
        With rs
            .Read()
            strFormCaption = GetVal(!DocName, "")
            strSHForm = GetVal(!DocShForm, "")
            strDocNoFldName = strSHForm & "No"
            strDocIDFldName = strSHForm & "ID"
            strQtyFldName = strSHForm & "Qty"
            strDocDateFldName = strSHForm & "Date"
            strDocHdrTblName = "TrnHdr" & strSHForm
            strDocDtlTblName = "TrnDtl" & strSHForm
            .Close()
        End With
    End Sub

    Private Sub txtBody_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtBody.KeyPress
        If e.KeyChar = "'" Then e.KeyChar = "`"
    End Sub

    Private Sub cboTemplate_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboTemplate.SelectedIndexChanged
        Call fillDtls()
    End Sub

    Private Sub fillDtls()
        Dim strSQL As String
        Dim rs As oledb.oledbDataReader
        Dim txtTempTo As New TextBox
        Dim txtTempCC As New TextBox
        Dim txtTempSub As New TextBox
        Dim txtTempBody As New TextBox
        Dim strQuery As String = ""
        strSQL = "Select * "
        strSQL &= " From UtlEmailTemplate"
        strSQL &= " Where DocID=" & intDocId & ""
        strSQL &= " And Template='" & cboTemplate.Text & "'"

        rs = clsSVT.getDataReaderFromSQL(strSQL)

        If Not rs.HasRows Then
            rs.Close()
            Exit Sub
        End If

        With rs
            If rs.HasRows Then
                .Read()
                txtTempTo.Text = GetVal(!EmailTo, "")
                txtTempCC.Text = GetVal(!EmailCC, "")
                txtTempSub.Text = GetVal(!EmailSub, "")
                txtTempBody.Text = GetVal(!EmailBody, "")

                If GetVal(!BlnOutlook, "N") = "Y" Then
                    blnOutlook = True
                Else
                    blnOutlook = False
                End If

                If GetVal(!FldQuery, "") = "Y" Then
                    blnFromQuery = True
                Else
                    blnFromQuery = False
                End If

                strQuery = GetVal(!QueryText, "")
                strPrintOptionName = GetVal(!PrintName, "")

                If GetVal(!AttachPrintOut, "N") = "Y" Then
                    txtAttach.Text = ""
                    txtAttach.Enabled = False
                    btnAttch.Enabled = False
                Else
                    txtAttach.Enabled = True
                    btnAttch.Enabled = True
                End If
            End If
            .Close()
        End With

        Dim dtTbl As DataTable

        If blnFromQuery Then
            strSQL = strQuery.ToUpper().Replace("<<DOCID>>", txtTranID.Text)
        Else
            strSQL = getFieldsFromTbl(strDocHdrTblName, Val(txtTranID.Text))
        End If

        dtTbl = clsSVT.getDATATable(strSQL)

        For i As Int32 = 0 To dtTbl.Rows.Count - 1
            For j As Int32 = 0 To dtTbl.Columns.Count - 1
                Dim strActual As String = ""
                For Each str As String In Trim(txtTempTo.Text).Split(";")
                    str = str.Replace(UCase("<<" & dtTbl.Columns(j).Caption & ">>"), GetVal(dtTbl.Rows(i).Item(j).ToString(), ""))
                    strActual = strActual & str & ";"
                Next
                txtTempTo.Text = Mid(strActual, 1, Len(strActual) - 1)
                strActual = ""
                For Each str As String In Trim(txtTempCC.Text).Split(";")
                    str = str.Replace(UCase("<<" & dtTbl.Columns(j).Caption & ">>"), GetVal(dtTbl.Rows(i).Item(j).ToString(), ""))
                    strActual = strActual & str & ";"
                Next
                txtTempCC.Text = Mid(strActual, 1, Len(strActual) - 1)
                'txtTempTo.Text = txtTempTo.Text.Replace(UCase("<<" & dtTbl.Columns(j).Caption & ">>"), GetVal(dtTbl.Rows(i).Item(j).ToString(), ""))
                'txtTempCC.Text = txtTempCC.Text.Replace(UCase("<<" & dtTbl.Columns(j).Caption & ">>"), GetVal(dtTbl.Rows(i).Item(j).ToString(), ""))
                txtTempSub.Text = txtTempSub.Text.Replace(UCase("<<" & dtTbl.Columns(j).Caption & ">>"), GetVal(dtTbl.Rows(i).Item(j).ToString(), ""))
                txtTempBody.Text = txtTempBody.Text.Replace(UCase("<<" & dtTbl.Columns(j).Caption & ">>"), GetVal(dtTbl.Rows(i).Item(j).ToString(), ""))
            Next
        Next
        txtTo.Text = txtTempTo.Text
        txtCC.Text = txtTempCC.Text
        txtSub.Text = txtTempSub.Text
        txtBody.Text = txtTempBody.Text

    End Sub

    Private Function genPrintOUt() As Boolean
        genPrintOUt = False
        Try
            If strPrintOptionName = "" Then
                Return True
            End If

            If strPrintOptionName <> "" Then
                strPDFFileName = Strings.Replace(strFormCaption, " ", "") & "_" & clsSVT.getDataStringValue("Select " & strDocNoFldName & " From " & strDocHdrTblName & " Where " & strDocIDFldName & " = " & Val(txtTranID.Text), "", False) & "_" & Format(Date.Now, "yyyyMMddHHmmss")
            End If

            Call showWaitCursor()

            Dim strSF As String
            Dim strFormulas(,) As String
            strSF = "{TrnHdr" & strSHForm & "." & strSHForm & "ID} = " & Val(txtTranID.Text)
            ReDim strFormulas(1, 2)
            strFormulas = Nothing
            Dim frmv As New frmUtlReportViewer
            With frmv
                .StartPosition = FormStartPosition.CenterScreen
                .ShowRptFileFromUtlOptions(intDocId, Val(txtTranID.Text), strPrintOptionName, False, False, True, strPDFFileName)
            End With
            frmv.Dispose()

            If File.Exists(Application.StartupPath & "\Temp\" & strPDFFileName & ".pdf") Then
                txtAttach.Text = Application.StartupPath & "\Temp\" & strPDFFileName & ".pdf"
            Else
                txtAttach.Text = ""
            End If

            Call showNormalCursor()
            Return True
        Catch ex As Exception
            Call showNormalCursor()
            MsgBox("Print out not generated..." & vbNewLine & ex.Message, MsgBoxStyle.Critical, Me.Text)
        End Try
    End Function

    Private Function getFieldsFromTbl(ByVal argTblName As String, ByVal argID As Int32) As String
        getFieldsFromTbl = ""
        Dim rsFlds As oledb.oledbDataReader
        Dim blnInclude As Boolean, i As Integer, x As Integer
        Dim blnFound As Boolean
        Dim strFldList As String = "", strFindFld As String
        Dim strCondList As String = "", strTblList As String
        Dim strSubTblCaption As String
        Dim strSQL As String
        Dim strFldName As String

        argTblName = UCase$(argTblName)

        strSQL = "Select * from  " & argTblName & " where 1 <> 1"
        rsFlds = clsSVT.getDataReaderFromSQL(strSQL)

        strTblList = argTblName & ","

        With rsFlds
            For i = 0 To .FieldCount - 1
                strFldName = UCase(Trim(.GetName(i)))
                blnInclude = True
                Select Case strFldName
                            'Case "AMMENDMENT", "IMGPRINT", "AMMDETAILS", "AMMNO", "AMMDATE", "AUTHORISED", "AUTHDATE", "AUTHORISEDBY"
                    Case "RGUID", "IMGPRINT"
                        blnInclude = False
                End Select

                If blnInclude Then
                    blnFound = False
                    'list the table name if it is from another table
                    strFindFld = strFldName
                    If InStr(1, strFindFld, "ID") > 0 Then
                        strFindFld = Replace(strFindFld, "ID", "")
                        If "MST" & strFindFld <> argTblName And "TRNHDR" & strFindFld <> argTblName And "TRNDTL" & strFindFld <> argTblName Then
                            '                        'if it is not from the same table MstItem or TrnHdrInv or TrnDtlInv
                            '                        'and if the master exists then 
                            strSubTblCaption = clsSVT.getDataStringValue("Select docName from UtlTransactions where HdrTblName = 'MST" & strFindFld & "'", "", blnFound)
                            If blnFound Then
                                If fnTablePresent("MST" & strFindFld) Then
                                    blnFound = False
                                    strTblList &= "MST" & strFindFld & ","
                                    strCondList &= " and " & argTblName & "." & strFldName & " = " & "MST" & strFindFld & "." & strFindFld & "ID"

                                End If
                            End If
                        End If
                    End If
                End If
            Next i
            rsFlds.Close()
        End With

        strTblList = Mid(strTblList, 1, Len(strTblList) - 1)

        strSQL = " SELECT * " '
        strSQL = strSQL & " FROM " & strTblList
        strSQL = strSQL & " WHERE 1=1 " & strCondList
        strSQL = strSQL & " and " & strDocHdrTblName & "." & strDocIDFldName & " = " & Val(argID)
        getFieldsFromTbl = strSQL
    End Function

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Call ClearFlds()
    End Sub

    Private Sub btnAttch_Click(sender As Object, e As EventArgs) Handles btnAttch.Click
        Dim ofd As OpenFileDialog = New OpenFileDialog
        'ofd.DefaultExt = "txt"
        'ofd.FileName = "defaultname"
        'ofd.InitialDirectory = "c:\"
        'ofd.Filter = "All files|*.*|Text files|*.txt"
        ofd.Title = "Select file"
        If ofd.ShowDialog() <> DialogResult.Cancel Then
            txtAttach.Text = ofd.FileName
        End If
    End Sub

    Private Sub btnSend_Click_1(sender As Object, e As EventArgs) Handles btnSend.Click
        Call stringReplace(Me)
        If Not genPrintOUt() Then
            Exit Sub
        End If

        If InStr(Trim(txtTo.Text), "<<") < 0 Then
            MsgBox("Invalid 'To' Email Address...", MsgBoxStyle.Exclamation, Me.Text)
            txtTo.Focus()
            Exit Sub
        End If

        'If InStr(Trim(txtCC.Text), "<<") < 0 Then
        '    MsgBox("Invalid 'CC' Email Address...", MsgBoxStyle.Exclamation, Me.Text)
        '    txtCC.Focus()
        '    Exit Sub
        'End If

        For Each str As String In Trim(txtTo.Text).Split(";")
            If Not validateEmail(str) Then
                MsgBox("The below email address is not valid..." & vbNewLine & str, MsgBoxStyle.Exclamation, Me.Text)
                txtTo.Focus()
                Exit Sub
            End If
        Next

        If txtCC.Text <> "" Then
            For Each str As String In Trim(txtCC.Text).Split(";")
                If Not validateEmail(str) Then
                    MsgBox("The below email address is not valid..." & vbNewLine & str, MsgBoxStyle.Exclamation, Me.Text)
                    txtCC.Focus()
                    Exit Sub
                End If
            Next
        End If

        Try

            If Trim(txtAttach.Text) <> "" AndAlso Not File.Exists(Trim(txtAttach.Text)) Then
                MsgBox("Invalid attachment....", MsgBoxStyle.Exclamation, Me.Text)
                txtAttach.Focus()
                Exit Sub
            End If

            If blnOutlook Then
                Dim oApp As Interop.Outlook._Application
                oApp = New Interop.Outlook.Application

                Dim oMsg As Interop.Outlook._MailItem
                oMsg = oApp.CreateItem(Interop.Outlook.OlItemType.olMailItem)

                oMsg.Subject = Trim(txtSub.Text)
                oMsg.Body = Trim(txtBody.Text)
                If Trim(txtTo.Text) <> "" Then
                    oMsg.To = Trim(txtTo.Text)
                End If
                If Trim(txtCC.Text) <> "" Then
                    oMsg.CC = Trim(txtCC.Text)
                End If

                If Trim(txtAttach.Text) <> "" Then
                    oMsg.Attachments.Add(Trim(txtAttach.Text))
                End If

                Dim strMailName As String = Application.StartupPath & "\Temp\" & Date.Now.ToString("yyyyMMddHHmmssfff") & ".msg"
                If File.Exists(strMailName) Then
                    File.Delete(strMailName)
                End If
                oMsg.SaveAs(strMailName)
                oMsg.Close(Microsoft.Office.Interop.Outlook.OlInspectorClose.olSave)
                Process.Start(strMailName)

            Else

                If gSmtpMailID = "" Then
                    MsgBox("SMTP Configuration Empty....", MsgBoxStyle.Exclamation, Me.Text)
                    Exit Sub
                End If

                Dim Smtp_Server As New Net.Mail.SmtpClient
                Dim e_mail As New Net.Mail.MailMessage()
                Smtp_Server.UseDefaultCredentials = False
                Smtp_Server.Credentials = New Net.NetworkCredential(CStr(gSmtpMailID & ""), CStr(gSmtpMailPwd & ""))
                Smtp_Server.Port = CStr(gSmtpServerPort & "")
                If CStr(gSmtpEnableSSL & "") = "Y" Then
                    Smtp_Server.EnableSsl = True
                Else
                    Smtp_Server.EnableSsl = False
                End If
                Smtp_Server.Host = CStr(gSmtpHostName & "")
                e_mail.From = New Net.Mail.MailAddress(CStr(gSmtpMailID & ""))
                If Trim(txtTo.Text) <> "" Then e_mail.To.Add(Trim(txtTo.Text))
                If Trim(txtCC.Text) <> "" Then e_mail.CC.Add(Trim(txtCC.Text))

                e_mail.Subject = Trim(txtSub.Text)
                e_mail.IsBodyHtml = False
                e_mail.Body = Trim(txtBody.Text)
                If Trim(txtAttach.Text) <> "" Then
                    Dim attachment As Net.Mail.Attachment
                    attachment = New System.Net.Mail.Attachment(Trim(txtAttach.Text))
                    e_mail.Attachments.Add(attachment)
                End If
                Smtp_Server.Send(e_mail)
                MsgBox("Email Sent....", MsgBoxStyle.Information, Me.Text)

            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

    End Sub
End Class