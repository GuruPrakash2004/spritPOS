
Imports System.ComponentModel
Imports C1.Win.C1FlexGrid

Public Class frmTrnSMS
    Dim clsSVT As New svtLibrary
    Dim blnContact As Boolean
    Public argID As Integer
    Public argForm As Object
    Public intDocId As Integer

    Dim docName As String
    Dim tblName As String
    Dim MainField As String


    Dim strDocType As String
    Dim strTblName As String
    Dim strIDName As String
    Dim strCodeName As String
    Dim strPartyName As String
    Dim strFormCaption As String
    Dim strFldCaption As String
    Dim blnShowFileAttach As Boolean
    Dim strRights As String
    Dim strGroupID As String
    Dim strGroupName As String
    Dim strGroupTblName As String
    Dim strPartyType As String
    Dim lngLedGrpID As Long
    Dim strFrmCRFrmCaption As String
    Dim strFrmMsg As String
    Dim strParameter As String
    Dim strFrmPfx As String
    Dim strMstPartyAccounts As String
    Dim strFldSeries As String
    Dim strFldNo As String

    Const vDel = 0
    Const vTo = 1
    Const vName = 2

    Dim blnFromQuery As Boolean
    Dim strFixedMobile As String = ""
    Dim strQueryText As String = ""

    Public Sub frmTrnSMS_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.KeyPreview = True
        Call initFormSize(Me, "TrnSMS", False, 650, 620, 0, 0)
        'cboTemplate.SelectedIndex = 0
    End Sub

    Public Sub initScreen(ByVal argCustid As Long)
        Dim strFirstSender As String = ""
        Call ClearFlds()
        txtTranID.Text = argCustid
        With vsfContact
            .Cols.Count = 3
            .Cols(vDel).Width = 28
            .Rows.Count = 0
            .Height = 100
        End With
        'Call LoadLastMsgDtls()
        Call bindCombo(cboProvider, "Provider", "Provider", "Select Provider from UtlMsgSettings")
        cboProvider.Text = gDefProvider

        Call initForm(intDocId)
        Call fillFormProperties(Me)
        Call initGridStyles(Me)
        Call ChangeFontSize(Me)
        Call bindCombo(cboTemplate, "Template", "Template", "Select Distinct Template from UtlSMSTemplate where DocID = " & intDocId & " order by 1")
        If cboTemplate.Items.Count > 0 Then
            cboTemplate.SelectedIndex = 0
        End If
    End Sub

    Private Sub ClearFlds()
        txtTo.Text = ""
        txtMsg.Text = ""
        lblCount.Text = ""
        txtTo.Focus()
        txtMsg.MaxLength = 250
        vsfContact.Rows.Count = 0
        vsfContact.Height = (vsfContact.Rows.MinSize * 3) + 10
        pnlContact.Height = vsfContact.Height + 10
    End Sub

    Private Function validFields() As Boolean
        validFields = False

        If vsfContact.Rows.Count < 1 Then
            MsgBox("Please enter mobile number....", MsgBoxStyle.Exclamation, Me.Text)
            txtTo.Focus()
            Exit Function
        End If

        If Trim(txtMsg.Text) = "" Then
            MsgBox("Message is Empty....", MsgBoxStyle.Exclamation, Me.Text)
            txtMsg.Focus()
            Exit Function
        End If
        If Trim(cboProvider.Text) = "" Then
            MsgBox("Provider has to be selected....", MsgBoxStyle.Exclamation, Me.Text)
            cboProvider.Focus()
            Exit Function
        End If


        validFields = True
    End Function
    Private Sub btnSend_Click(sender As Object, e As EventArgs) Handles btnSend.Click

        If validFields() = False Then Exit Sub

        Dim strToNums As String = ""

        Call showWaitCursor()

        For i = 0 To vsfContact.Rows.Count - 1

            strToNums = strToNums & vsfContact.GetData(i, 1).ToString & ","
        Next
        strToNums = Mid(strToNums, 1, Len(strToNums) - 1)

        For Each strMob As String In Trim(strToNums).Split(",")
            Call SendSMS(txtMsg.Text, strMob, cboProvider.ValueMember)
        Next

        Call showNormalCursor()

        MsgBox("Message Sent.", MsgBoxStyle.Information, Me.Text)
        Call ClearFlds()
        txtTo.Focus()
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub btnClr_Click(sender As Object, e As EventArgs) Handles btnClr.Click
        Call ClearFlds()
    End Sub

    Private Sub txtMsg_TextChanged(sender As Object, e As EventArgs) Handles txtMsg.TextChanged
        lblCount.Text = "Message Length: " & Len(txtMsg.Text)
        TextBoxSizeChanged(txtMsg, e)
    End Sub

    Private Sub txtTo_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtTo.TextChanged
        If Trim(txtTo.Text) <> "" Then
            If Len(txtTo.Text) > 1 Then
                If vsfSearch.Rows.Count <= 1 Then
                    vsfSearch.Visible = False
                    Exit Sub
                End If
            End If
            vsfSearch.BringToFront()
            'vsfSearch.Visible = True
        Else
            vsfSearch.Visible = False
        End If
    End Sub

    Public Sub setfldvalues(ByVal argFldCaption As String)
        txtMsg.Text = argFldCaption
    End Sub

    Private Sub frmTrnSMS_KeyUp(sender As Object, e As KeyEventArgs) Handles Me.KeyUp
        If e.KeyCode = Keys.F9 And e.Control Then
            '958, 579
            Call initFormSize(Me, "TrnSMS", True, 650, 620, 0, 0)
            'Call SetColWidthFromString(vsfDet, strDocStdDtlWidth)
            MsgBox(("Restored Form & Grid Layout Successfully!!"), vbInformation, Me.Text)
        End If
        If e.KeyCode = Keys.F8 And e.Control = True Then
            Call SaveActFormSettings(Me, "TrnSMS")
            MsgBox(("Saved Form & Grid Layout Successfully!!"), vbInformation, Me.Text)
        End If
        If e.KeyCode = Keys.F6 Then
            Call btnSend_Click(sender, e)
        End If
    End Sub

    Private Sub vsfContact_BeforeEdit(sender As Object, e As RowColEventArgs) Handles vsfContact.BeforeEdit
        e.Cancel = True
    End Sub

    Private Sub txtTo_KeyDown(sender As Object, e As KeyEventArgs) Handles txtTo.KeyDown
        If txtTo.Text = "" Then
            If e.KeyCode = Keys.Enter Then
                txtMsg.Focus()
            End If
        End If
    End Sub

    Private Sub vsfContact_MouseClick(sender As Object, e As MouseEventArgs) Handles vsfContact.MouseClick
        Dim ht = vsfContact.HitTest

        With vsfContact
            If ht.Row < 0 Then Exit Sub
            If .GetData(ht.Row, vTo) = "" Then Exit Sub
            If ht.Column <> vDel Then Exit Sub
            'If MsgBox("Do you want to delete this Contact -- " & .GetData(ht.Row, vTo) & " ?", vbYesNo + vbDefaultButton2 + vbCritical, "Remove Contact") = MsgBoxResult.No Then Exit Sub
            .RemoveItem(ht.Row)
            If vsfContact.Rows.Count > 3 Then
                vsfContact.Height = (vsfContact.Rows.MinSize * vsfContact.Rows.Count) + 10
                pnlContact.Height = vsfContact.Height
            End If
        End With
    End Sub

    Private Sub FlowLayoutPanel1_SizeChanged(sender As Object, e As EventArgs) Handles FlowLayoutPanel1.SizeChanged
        Panel1.Height = FlowLayoutPanel1.Height - pnlContact.Height
        txtMsg.Height = Panel1.Height
    End Sub

    Private Sub txtTo_Validating(sender As Object, e As CancelEventArgs) Handles txtTo.Validating

        Dim blnEmpty As Boolean
        If Trim(txtTo.Text) = "" Then Exit Sub

        'If vsfSearch.Rows.Count <= 1 And Len(txtTo.Text) = 10 Then
        For i = 0 To vsfContact.Rows.Count - 1
            If vsfContact.GetData(i, vTo) = "" Then Exit For
            If vsfContact.GetData(i, vTo) = Trim(txtTo.Text) Then
                MsgBox("'" & txtTo.Text & "' already present!!")
                txtTo.Text = ""
                txtTo.Focus()
                Exit Sub
            End If
        Next
        For i = 0 To vsfContact.Rows.Count - 1
            If vsfContact.GetData(i, vTo) = "" Then
                vsfContact.SetCellImage(i, vDel, My.Resources.cancel)
                vsfContact.SetData(i, vTo, Trim(txtTo.Text))

                blnEmpty = True
                Exit For
            End If
        Next
        If Not blnEmpty Then
            vsfContact.Rows.Count = vsfContact.Rows.Count + 1
            vsfContact.SetCellImage(vsfContact.Rows.Count - 1, vDel, My.Resources.cancel)
            vsfContact.SetData(vsfContact.Rows.Count - 1, vTo, Trim(txtTo.Text))
        End If
        txtTo.Text = ""
        'End If

    End Sub

    'Private Sub frmTrnSMS_Shown(sender As Object, e As EventArgs) Handles Me.Shown
    '    Call cboTemplate_Validating(cboTemplate, Nothing)
    '    txtTo.Focus()
    'End Sub

    Private Sub btnTemplate_Click(sender As Object, e As EventArgs) Handles btnTemplate.Click
        Dim frmUtlSearch As New frmUtlSMSSearch
        With frmUtlSearch
            .strDocName = "Supplier Master" & "VIEW"
            .strTblName = "MstSUPP"
            .strMainFld = "SUPPCODE"
            .blnSRCHFrom = False
            '.dblTotCol = vsfDet.Cols.Count
            .argForm = Me
            '.MdiParent = frmMain
            '.Show()
            .ShowDialog()
        End With
    End Sub

    Private Sub txtTo_KeyUp(sender As Object, e As KeyEventArgs) Handles txtTo.KeyUp
        If txtTo.Text = "" Then Exit Sub
        If e.KeyCode = Keys.Enter Then
            Dim blnEmpty As Boolean


            'If vsfSearch.Rows.Count <= 1 And Len(txtTo.Text) = 10 Then
            For i = 0 To vsfContact.Rows.Count - 1
                If vsfContact.GetData(i, vTo) = "" Then Exit For
                If vsfContact.GetData(i, vTo) = Trim(txtTo.Text) Then
                    MsgBox("'" & txtTo.Text & "' already present!!")
                    txtTo.Text = ""
                    txtTo.Focus()
                    Exit Sub
                End If
            Next
            For i = 0 To vsfContact.Rows.Count - 1
                If vsfContact.GetData(i, vTo) = "" Then
                    vsfContact.SetCellImage(i, vDel, My.Resources.cancel)
                    vsfContact.SetData(i, vTo, Trim(txtTo.Text))

                    blnEmpty = True
                    Exit For
                End If
            Next
            If Not blnEmpty Then
                vsfContact.Rows.Count = vsfContact.Rows.Count + 1
                vsfContact.SetCellImage(vsfContact.Rows.Count - 1, vDel, My.Resources.cancel)
                vsfContact.SetData(vsfContact.Rows.Count - 1, vTo, Trim(txtTo.Text))
            End If
            txtTo.Text = ""
            'End If
        End If
    End Sub

    Private Sub cboTemplate_Validating(sender As Object, e As CancelEventArgs) Handles cboTemplate.Validating
        If Trim(cboTemplate.Text) = "" Then Exit Sub
        Dim txtTemplate As New TextBox


        Dim rs As oledb.oledbDataReader
        rs = clsSVT.getDataReaderFromSQL("Select * from UtlSMSTemplate where DocID=" & intDocId & " and Template ='" & cboTemplate.Text & "'")
        'Call clsSVT.getDataStringValue("Select Template from UtlSMSTemplate where DocID=" & intDocId & " and Template ='" & cboTemplate.Text & "'", "", blnFound)
        If Not rs.HasRows Then
            cboTemplate.Text = ""
            Exit Sub
        End If

        rs.Read()

        txtTemplate.Text = rs.Item("TemplateText")
        strQueryText = rs.Item("QueryText")
        strFixedMobile = rs.Item("FixedMobNo")
        If rs.Item("FromQuery") = "Y" Then
            blnFromQuery = True
        Else
            blnFromQuery = False
        End If
        rs.Close()
        If txtTemplate.Text = "" Then Exit Sub
        txtTemplate.Multiline = True
        If Trim(txtTemplate.Text) <> "" Then
            Dim strSQL As String = ""
            If blnFromQuery Then
                strSQL = strQueryText.ToUpper().Replace("<<DOCID>>", txtTranID.Text)
            Else
                strSQL = getFieldsFromTbl(strTblName, Val(txtTranID.Text))
            End If

            Dim dtTbl As DataTable = clsSVT.getDATATable(strSQL)

            For i As Int32 = 0 To dtTbl.Rows.Count - 1
                For j As Int32 = 0 To dtTbl.Columns.Count - 1
                    txtTemplate.Text = txtTemplate.Text.Replace(UCase("<<" & dtTbl.Columns(j).Caption & ">>"), GetVal(dtTbl.Rows(i).Item(j).ToString(), ""))
                    strFixedMobile = strFixedMobile.Replace(UCase("<<" & dtTbl.Columns(j).Caption & ">>"), GetVal(dtTbl.Rows(i).Item(j).ToString(), ""))
                Next
            Next
            txtMsg.Text = txtTemplate.Text
            txtTo.Text = strFixedMobile
            Exit Sub
        End If
        'For Each myField In dtTbl.Rows
        '    For Each myProperty In dtTbl.Columns
        '        txtTemplate.Text = Replace(txtTemplate.Text, UCase("<<" & myProperty.ColumnName & ">>"), GetVal(myField(myProperty).ToString(), ""))
        '    Next
        'Next
        'txtMsg.Text = txtTemplate.Text
        'Exit Sub

    End Sub

    Private Sub btnTemplateSettings_Click(sender As Object, e As EventArgs) Handles btnTemplateSettings.Click
        Dim obj As New frmUtlTrnFldsTemplate

        With obj
            .Text = "SMS Party Template"
            .intDocID = intDocId
            .ShowDialog()
        End With
        Call bindCombo(cboTemplate, "Template", "Template", "Select Distinct Template from UtlSMSTemplate where DocID = " & intDocId & " order by 1")
    End Sub

    Private Sub initForm(ByVal argDocID As Integer)
        If argDocID > 0 Then intDocId = argDocID
        Dim rs As oledb.oledbDataReader
        rs = clsSVT.getDataReaderFromSQL(" Select * from UtlTransactions where DocID = " & argDocID)
        With rs
            .Read()
            strTblName = !HdrTblName
            strCodeName = Trim(Replace(!HdrTblName, "TrnHdr", "")) & "Code"
            strIDName = Trim(Replace(!HdrTblName, "TrnHdr", "")) & "ID"
            strFrmPfx = Trim(Replace(!HdrTblName, "TrnHdr", ""))
            strFldSeries = Replace(!HdrTblName, "TrnHdr", "") & "Series"
            strFldNo = Replace(!HdrTblName, "TrnHdr", "") & "No"
            .Close()
        End With
    End Sub

    Private Sub txtMsg_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtMsg.KeyPress
        If e.KeyChar = "'" Then e.KeyChar = "`"
    End Sub

    Private Sub vsfContact_Click(sender As Object, e As EventArgs) Handles vsfContact.Click

    End Sub

    Private Sub frmTrnSMS_Layout(sender As Object, e As LayoutEventArgs) Handles Me.Layout

    End Sub

    Private Sub cboSender_LostFocus(sender As Object, e As EventArgs) Handles cboProvider.LostFocus
        If Not e.Equals(Keys.Tab) Then
            If vsfContact.Rows.Count <= 0 Then

                cboTemplate.Focus()
            Else
                With vsfContact
                    .Row = 0
                    .Col = 0
                    .Focus()
                End With
            End If

        End If
    End Sub

    Private Sub vsfContact_KeyDown(sender As Object, e As KeyEventArgs) Handles vsfContact.KeyDown
        If e.KeyCode = Keys.Tab Then
            If vsfContact.Row = vsfContact.Rows.Count - 1 AndAlso vsfContact.Col = vsfContact.Cols.Count - 1 Then
                cboTemplate.Focus()
            End If
        End If
    End Sub

    Private Sub cboTemplate_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboTemplate.SelectedIndexChanged
        cboTemplate_Validating(Nothing, Nothing)
        Exit Sub
        '    txtMsg.Text = txtTemplate.Text
        If Trim(cboTemplate.Text) = "" Then Exit Sub
        Dim txttemplate As New TextBox
        Dim blnFound As Boolean

        Call clsSVT.getDataStringValue("Select Template from UtlSMSTemplate where DocID=" & intDocId & " and Template ='" & cboTemplate.Text & "'", "", blnFound)
        If Not blnFound Then
            cboTemplate.Text = ""
            Exit Sub
        End If
        txttemplate.Text = clsSVT.getDataStringValue("Select TemplateText from UtlSMSTemplate where DocID=" & intDocId & " and Template='" & cboTemplate.Text & "'", "", False)
        If txttemplate.Text = "" Then Exit Sub
        txttemplate.Multiline = True
        If Trim(txttemplate.Text) <> "" Then
            Dim strSQL As String = getFieldsFromTbl(strTblName, Val(txtTranID.Text))

            Dim dtTbl As DataTable = clsSVT.getDATATable(strSQL)

            For i As Int32 = 0 To dtTbl.Rows.Count - 1
                For j As Int32 = 0 To dtTbl.Columns.Count - 1
                    txttemplate.Text = txttemplate.Text.Replace(UCase("<<" & dtTbl.Columns(j).Caption & ">>"), GetVal(dtTbl.Rows(i).Item(j).ToString(), ""))
                Next
            Next
            txtMsg.Text = txttemplate.Text
            Exit Sub
        End If
        'For Each myField In dtTbl.Rows
        '    For Each myProperty In dtTbl.Columns
        '        txttemplate.Text = Replace(txttemplate.Text, UCase("<<" & myProperty.ColumnName & ">>"), GetVal(myField(myProperty).ToString(), ""))
        '    Next
        'Next
        'txtMsg.Text = txttemplate.Text
        'Exit Sub
    End Sub

    Private Sub txtTo_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtTo.KeyPress
        If Not Char.IsDigit(e.KeyChar) And Not Char.IsControl(e.KeyChar) Then
            e.Handled = True
        End If
    End Sub

    Private Function getFieldsFromTbl(ByVal argTblName As String, ByVal argID As Int32) As String
        getFieldsFromTbl = ""
        Dim rsFlds As oledb.oledbDataReader
        Dim blnInclude As Boolean, i As Integer
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
                            strSubTblCaption = clsSVT.getDataStringValue("select docName from UtlTransactions where HdrTblName = 'MST" & strFindFld & "'", "", blnFound)
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
        strSQL = strSQL & " and " & strTblName & "." & strIDName & " = " & Val(argID)
        getFieldsFromTbl = strSQL
    End Function

    Private Sub cboTemplate_RightToLeftChanged(sender As Object, e As EventArgs) Handles cboTemplate.RightToLeftChanged

    End Sub
End Class