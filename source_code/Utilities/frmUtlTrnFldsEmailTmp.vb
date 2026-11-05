Imports System.ComponentModel
Imports System.Data.oledb
Imports C1.Win.C1FlexGrid

Public Class frmUtlTrnFldsEmailTmp
    Dim clsSVT As New svtLibrary
    Public strMstTblName As String
    Public intDocID As Integer
    Public blnAddressTemplate As Boolean
    Dim strSQL As String

    Const vFldTableCaption = 0
    Const vFldTableName = 1
    Const vFldCode = 2
    Const vFldOrder = 3
    Const vFldDataType = 4
    Const vFldTotal = 5

    Const vTblName = 0
    Const vTblProcessed = 1
    Const vTblCaption = 2
    Const vTblLinkCond = 3
    Const vTblTotal = 4

    Dim strDocName As String
    Dim strDocType As String

    Dim blnTo As Boolean
    Dim blnCC As Boolean
    Dim blnSub As Boolean
    Dim blnBody As Boolean

    Private Sub btnClear_Click(sender As Object, e As EventArgs) Handles btnClear.Click
        Call clrFlds()
    End Sub

    Private Sub clrFlds()
        txtTo.Text = ""
        txtCC.Text = ""
        txtSub.Text = ""
        txtBody.Text = ""
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Dispose()
    End Sub

    Private Sub frmUtlTemp_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Call fillFormProperties(Me)
        Call initGridStyles(Me)
        Call ChangeFontSize(Me)
        If blnAddressTemplate Then
            lblTemplate.Visible = False
            cboTemplate.Visible = False
            btnDelTemplate.Visible = False
        End If
        'cboTemplate.Focus()

        Call bindCombo(cboPrint, "OptionName", "OptionName", "Select Distinct OptionName from UtlOption where DocID = " & intDocID & " order by 1")

        Call bindCombo(cboTemplate, "Template", "Template", "Select Distinct Template from UtlEmailTemplate where DocID = " & intDocID & " order by 1")

        Dim strTableNme As String = clsSVT.getDataStringValue("select DocName from UtlTransactions where DocId = '" & intDocID & "'", "", False)
        Call FillFldSettings(strTableNme)

        If cboTemplate.SelectedIndex > 0 Then
            cboTemplate.SelectedIndex = 0
        End If
    End Sub

    Private Sub FillMstFlds()
        Dim dt As DataTable

        If blnAddressTemplate Then
            strSQL = "Select Column_Name [Field Code] from Information_schema.Columns where Table_Name='" & strMstTblName & "'"
        Else
            strSQL = "Select Column_Name [Field Code] from Information_schema.Columns where Table_Name in (Select HdrTblName from UtlTransactions where DocID=" & intDocID & ")"
        End If
        dt = clsSVT.getDATATable(strSQL)
        vsfFlds.DataSource = dt
    End Sub

    Public Sub FillFldSettings(ByVal argName As String)
        With vsfTbls
            .Rows.Count = 1
            .Cols.Count = vTblTotal
        End With
        With vsfFlds
            .Cols.Count = vFldTotal
            .Cols.Fixed = 1
            .Rows.Count = 1
            .Cols(vFldTableName).Visible = False
            .Cols(vFldOrder).Visible = False
            .Cols(vFldDataType).Visible = False
            .SetData(0, vFldCode, "Source")
            .Cols(vFldCode).AllowSorting = False
            .Cols(vFldTableCaption).AllowMerging = True
        End With

        'strDocType = argType
        'Select Case argType
        '    Case "T"
        strDocName = argName
        Call fillFldsFromTbl(argName)

        'Case "E"
        '    strRptName = argName
        '    txtRptID.Text = clsSVT.getDataLongValue("Select CRID from UtlReportsCRHdr where CRName = '" & strRptName & "'", 0, False)
        '    Call fillFldsFromExist(Val(txtRptID.Text))

        'End Select
        vsfFlds.AutoSizeCols()
    End Sub

    Private Sub fillFldsFromTbl(ByVal argName As String)
        Dim strSQL As String, strFMT As String = ""
        Dim strDocIDFld As String
        Dim strTblName As String, blnFound As Boolean
        Dim strDtlTblName As String
        Dim rs As oledb.oledbDataReader
        vsfFlds.Rows.Count = 1

        strTblName = UCase(Trim(clsSVT.getDataStringValue("Select HdrTblName from UtlTransactions where DocName = '" & argName & "'", "", False)))
        If strTblName = "" Then
            strTblName = UCase(Trim(clsSVT.getDataStringValue("Select HdrTblName from UtlTransactions where HdrTblName = '" & argName & "'", "", False)))
        End If
        If strTblName = "" Then
            'must be from view
            If fnTablePresent(argName) Then
                strTblName = argName
            End If
        End If
        If strTblName = "" Then
            MsgBox("Invalid Document!", MsgBoxStyle.Critical)
            Me.Close()
            Exit Sub
        End If
        'txtHdrTblName.Text = strTblName

        strDocIDFld = Replace(strTblName, "MST", "")
        strDocIDFld = Replace(strDocIDFld, "TRNHDR", "")
        strDocIDFld = strDocIDFld & "ID"

        vsfTbls.Rows.Count = vsfTbls.Rows.Count + 1
        vsfTbls.SetData(vsfTbls.Rows.Count - 1, vTblName, strTblName)
        vsfTbls.SetData(vsfTbls.Rows.Count - 1, vTblCaption, argName)

        If strDocType <> "A" Then
            strDtlTblName = clsSVT.getDataStringValue("Select DtlTblName from UtlTransactions where HdrTblName = '" & strTblName & "'", "", False)
            If Trim(strDtlTblName) <> "" Then
                vsfTbls.Rows.Count = vsfTbls.Rows.Count + 1
                vsfTbls.SetData(vsfTbls.Rows.Count - 1, vTblName, strDtlTblName)
                vsfTbls.SetData(vsfTbls.Rows.Count - 1, vTblCaption, "Detail " & argName)
                vsfTbls.SetData(vsfTbls.Rows.Count - 1, vTblLinkCond, strTblName & "." & strDocIDFld & " = " & strDtlTblName & "." & strDocIDFld)
            End If
        End If

        Do While True
            With vsfTbls
                If .Rows.Count = 1 Then Exit Do
                blnFound = False
                For i = 1 To .Rows.Count - 1
                    If .GetData(i, vTblProcessed) = "" Then
                        blnFound = True
                    End If
                    If .GetData(i, vTblProcessed) = "" Then
                        strSQL = getFieldsFromTbl(.GetData(i, vTblName))
                        rs = clsSVT.getDataReaderFromSQL(strSQL)
                        rs.Read()
                        For x = 0 To rs.FieldCount - 1
                            vsfFlds.Rows.Count = vsfFlds.Rows.Count + 1
                            vsfFlds.SetData(vsfFlds.Rows.Count - 1, vFldTableCaption, .GetData(i, vTblCaption))
                            vsfFlds.SetData(vsfFlds.Rows.Count - 1, vFldTableName, .GetData(i, vTblName))
                            vsfFlds.SetData(vsfFlds.Rows.Count - 1, vFldCode, rs.GetName(x))
                            vsfFlds.SetData(vsfFlds.Rows.Count - 1, vFldOrder, vsfFlds.Rows.Count - 1)
                            vsfFlds.SetData(vsfFlds.Rows.Count - 1, vFldDataType, getFldDataType(rs.GetFieldType(x).Name))
                        Next x
                        .SetData(i, vTblProcessed, "Y")
                        rs.Close()
                    End If
                Next
            End With
            If Not blnFound Then Exit Do
        Loop
        'vsfFlds.Cols(vFldTableName).AllowMerging = AllowMergingEnum.FixedOnly
        vsfFlds.Cols(vFldTableCaption).AllowMerging = True
    End Sub

    Private Function getFieldsFromTbl(ByVal argTblName As String) As String
        getFieldsFromTbl = ""
        Dim rsFlds As oledb.oledbDataReader
        Dim blnInclude As Boolean, i As Integer, x As Integer
        Dim blnFound As Boolean
        Dim strFldList As String = "", strTblList As String, strFindFld As String
        Dim strCondList As String = ""
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
                    If strDocType <> "A" Then
                        If InStr(1, strFindFld, "ID") > 0 Then
                            strFindFld = Replace(strFindFld, "ID", "")
                            If "MST" & strFindFld <> argTblName And "TRNHDR" & strFindFld <> argTblName And "TRNDTL" & strFindFld <> argTblName Then
                                'if it is not from the same table MstItem or TrnHdrInv or TrnDtlInv
                                'and if the master exists then 
                                strSubTblCaption = clsSVT.getDataStringValue("select docName from UtlTransactions where HdrTblName = 'MST" & strFindFld & "'", "", blnFound)
                                If blnFound Then
                                    If fnTablePresent("MST" & strFindFld) Then
                                        blnFound = False
                                        With vsfTbls
                                            For x = 1 To .Rows.Count - 1
                                                If UCase("MST" & strFindFld) = UCase(.GetData(x, vTblName)) Then
                                                    blnFound = True
                                                End If
                                            Next
                                        End With
                                        If Not blnFound Then
                                            vsfTbls.Rows.Count = vsfTbls.Rows.Count + 1
                                            vsfTbls.SetData(vsfTbls.Rows.Count - 1, vTblName, "MST" & strFindFld)
                                            vsfTbls.SetData(vsfTbls.Rows.Count - 1, vTblCaption, strSubTblCaption)
                                            vsfTbls.SetData(vsfTbls.Rows.Count - 1, vTblLinkCond, argTblName & "." & strFldName & " = " & "MST" & strFindFld & "." & strFindFld & "ID")
                                        End If
                                    End If
                                End If
                            End If
                        End If
                    End If
                    strFldList = strFldList & argTblName & "." & GetVal(strFldName, "") & " AS " & strFldName & ","
                End If
            Next i
            rsFlds.Close()
        End With
        strFldList = Mid(strFldList, 1, Len(strFldList) - 1)
        strTblList = Mid(strTblList, 1, Len(strTblList) - 1)

        strSQL = "SELECT " & strFldList

        strSQL = strSQL & " FROM " & strTblList
        strSQL = strSQL & " WHERE 1=1 " & strCondList
        getFieldsFromTbl = strSQL
    End Function

    Private Sub vsfFlds_DoubleClick(sender As Object, e As EventArgs) Handles vsfFlds.DoubleClick
        If vsfFlds.Rows.Count <= 1 Then Exit Sub

        If blnTo Then
            txtTo.Text = txtTo.Text & "<<" & UCase(vsfFlds.GetData(vsfFlds.Row, vsfFlds.Col)) & ">>"
        End If

        If blnCC Then
            txtCC.Text = txtCC.Text & "<<" & UCase(vsfFlds.GetData(vsfFlds.Row, vsfFlds.Col)) & ">>"
        End If

        If blnSub Then
            txtSub.Text = txtSub.Text & "<<" & UCase(vsfFlds.GetData(vsfFlds.Row, vsfFlds.Col)) & ">>"
        End If

        If blnBody Then
            txtBody.Text = txtBody.Text & "<<" & UCase(vsfFlds.GetData(vsfFlds.Row, vsfFlds.Col)) & ">>"
        End If
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click

        If Not validFlds() Then Exit Sub

        If blnAddressTemplate Then
            'clsSVT.execSQLStatement("Delete from UtlPartyCopy where docID=" & intDocID)
            'strSQL = "Insert into UtlPartyCopy (DocID, CopyFlds) values("
            'strSQL &= intDocID & ","
            'strSQL &= " '" & Replace(txtFlds.Text, vbLf, vbCrLf) & "')"
            'clsSVT.execSQLStatement(strSQL)
            'MsgBox("Data Saved Successfully!", vbInformation, Me.Text)
            'Me.Dispose()
            'Exit Sub
        End If

        clsSVT.execSQLStatement("Delete from UtlEmailTemplate where docID=" & intDocID & " and Template='" & Trim(cboTemplate.Text) & "'")
        strSQL = "Insert into UtlEmailTemplate (DocID, Template, EmailTo, EmailCC, EmailSub, EmailBody, BlnOutlook"
        If chkAttachPrintOut.Checked Then
            strSQL &= " , AttachPrintOut, PrintName"
        End If
        If optQry.Checked Then
            strSQL &= " , FldQuery, QueryText"
        End If
        strSQL &= ") values ("
        strSQL &= intDocID & ","
        strSQL &= "'" & Trim(cboTemplate.Text) & "',"
        strSQL &= "'" & Replace(txtTo.Text, vbLf, vbCrLf) & "',"
        strSQL &= "'" & Replace(txtCC.Text, vbLf, vbCrLf) & "',"
        strSQL &= "'" & Replace(txtSub.Text, vbLf, vbCrLf) & "',"
        strSQL &= " '" & Replace(txtBody.Text, vbLf, vbCrLf) & "',"

        If optOutlook.Checked Then
            strSQL &= " 'Y'"
        Else
            strSQL &= " 'N'"
        End If


        If chkAttachPrintOut.Checked Then
            strSQL &= ", 'Y',"
            strSQL &= " '" & Trim(cboPrint.Text) & "'"
        End If
        If optQry.Checked Then
            strSQL &= ", 'Y',"
            strSQL &= " '" & Replace(txtQuery.Text, vbLf, vbCrLf) & "'"
        End If
        strSQL &= ")"
        clsSVT.execSQLStatement(strSQL)
        MsgBox("Data Saved Successfully!", vbInformation, Me.Text)
        Me.Dispose()
    End Sub

    Private Sub vsfFlds_BeforeEdit(sender As Object, e As RowColEventArgs) Handles vsfFlds.BeforeEdit
        e.Cancel = True
    End Sub

    Private Sub cboTemp_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cboTemplate.KeyPress, cboPrint.KeyPress
        If e.KeyChar = "'" Then e.Handled = True
    End Sub

    Private Sub txtFlds_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtTo.KeyPress, txtCC.KeyPress, txtSub.KeyPress, txtBody.KeyPress, txtQuery.KeyPress
        If e.KeyChar = "'" Then e.KeyChar = "`"
    End Sub

    'Private Sub cboTemplate_Validating(sender As Object, e As CancelEventArgs) Handles cboTemplate.Validating
    '    If Trim(cboTemplate.Text) = "" Then Exit Sub
    '    txtFlds.Text = clsSVT.getDataStringValue("Select TemplateText from UtlSMSTemplate where DocID=" & intDocID & " and Template='" & cboTemplate.Text & "'", "", False)
    '    txtFlds.Focus()
    'End Sub

    Private Sub btnDelTemplate_Click(sender As Object, e As EventArgs) Handles btnDelTemplate.Click
        If Trim(cboTemplate.Text) = "" Then Exit Sub
        If MsgBox("Do You want to Delete this '" & cboTemplate.Text & "'", vbInformation + vbYesNo + vbDefaultButton2, Me.Text) = vbYes Then
            clsSVT.execSQLStatement("Delete from UtlEmailTemplate where docID=" & intDocID & " and Template='" & Trim(cboTemplate.Text) & "'")
            MsgBox("Template deleted successfully!", MsgBoxStyle.Information, Me.Text)
            Call bindCombo(cboTemplate, "Template", "Template", "Select Distinct Template from UtlEmailTemplate where DocID = " & intDocID & " order by 1")
            Call clrFlds()
            Exit Sub
        End If
    End Sub

    Private Sub vsfFlds_KeyDown(sender As Object, e As KeyEventArgs) Handles vsfFlds.KeyDown
        If e.KeyCode = Keys.Tab Then
            If vsfFlds.Row = vsfFlds.Rows.Count - 1 Then
                txtTo.Focus()
            End If
        End If
    End Sub

    Private Sub btnDelTemplate_LostFocus(sender As Object, e As EventArgs) Handles btnDelTemplate.LostFocus
        If Not e.Equals(Keys.Tab) Then
            With vsfFlds
                .Row = 1
                .Col = vFldCode
                .Focus()
            End With
        End If
    End Sub

    Private Sub cboTemplate_LostFocus(sender As Object, e As EventArgs) Handles cboTemplate.LostFocus
        If e.Equals(Keys.Tab) Then
            btnNewTem.Focus()
        End If
    End Sub

    Public Function validFlds() As Boolean
        validFlds = False
        If Trim(cboTemplate.Text) = "" Then
            MsgBox("Template name should not be empty!", MsgBoxStyle.Information, Me.Text)
            cboTemplate.Focus()
            Exit Function
        End If

        If Trim(txtTo.Text) = "" Then
            MsgBox("Add To template text!", MsgBoxStyle.Information, Me.Text)
            txtTo.Focus()
            Exit Function
        End If

        'If Trim(txtCC.Text) = "" Then
        '    MsgBox("Add CC template text!", MsgBoxStyle.Information, Me.Text)
        '    txtCC.Focus()
        '    Exit Function
        'End If

        If Trim(txtSub.Text) = "" Then
            MsgBox("Add Subject template text!", MsgBoxStyle.Information, Me.Text)
            txtSub.Focus()
            Exit Function
        End If

        If Trim(txtBody.Text) = "" Then
            MsgBox("Add Body template text!", MsgBoxStyle.Information, Me.Text)
            txtBody.Focus()
            Exit Function
        End If

        If chkAttachPrintOut.Checked Then
            If clsSVT.getDataLongValue("Select DocID from UtlOption where DocID = " & intDocID & " And OptionName = '" & cboPrint.Text & "'", 0, False) <= 0 Then
                MsgBox("Invalid Print Name!", MsgBoxStyle.Information, Me.Text)
                cboPrint.Focus()
                Exit Function
            End If
        End If
        validFlds = True
    End Function

    Private Sub btnNewTem_Click(sender As Object, e As EventArgs) Handles btnNewTem.Click
        cboTemplate.Text = ""
        Call clrFlds()
    End Sub

    Private Sub cboTemplate_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboTemplate.SelectedIndexChanged
        Dim rs As oledb.oledbDataReader
        Dim strSQL As String

        strSQL = "Select * "
        strSQL &= " From UtlEmailTemplate"
        strSQL &= " Where DocID = " & intDocID & " And Template = '" & Trim(cboTemplate.Text) & "'"

        rs = clsSVT.getDataReaderFromSQL(strSQL)

        With rs
            If rs.HasRows Then
                .Read()
                txtTo.Text = GetVal(!EmailTo, "")
                txtCC.Text = GetVal(!EmailCC, "")
                txtSub.Text = GetVal(!EmailSub, "")
                txtBody.Text = GetVal(!EmailBody, "")
                setCheckBox(chkAttachPrintOut, GetVal(!AttachPrintOut, "N"))
                cboPrint.Text = GetVal(!PrintName, "")
                If GetVal(!FldQuery, "N") = "Y" Then
                    optQry.Checked = True
                    enableDisableFlds(True)
                Else
                    optStdTbls.Checked = True
                    enableDisableFlds(False)
                End If

                If GetVal(!BlnOutlook, "N") = "Y" Then
                    optOutlook.Checked = True
                Else
                    optSMTP.Checked = True
                End If

                txtQuery.Text = GetVal(!QueryText, "")
            End If
            .Close()
        End With
    End Sub

    Private Sub txtTo_GotFocus(sender As Object, e As EventArgs) Handles txtTo.GotFocus
        blnTo = True
        blnCC = False
        blnSub = False
        blnBody = False
    End Sub

    Private Sub txtCC_GotFocus(sender As Object, e As EventArgs) Handles txtCC.GotFocus
        blnTo = False
        blnCC = True
        blnSub = False
        blnBody = False
    End Sub

    Private Sub txtSub_GotFocus(sender As Object, e As EventArgs) Handles txtSub.GotFocus
        blnTo = False
        blnCC = False
        blnSub = True
        blnBody = False
    End Sub

    Private Sub txtBody_GotFocus(sender As Object, e As EventArgs) Handles txtBody.GotFocus
        blnTo = False
        blnCC = False
        blnSub = False
        blnBody = True
    End Sub

    Private Sub enableDisableFlds(ByVal argFlag As Boolean)
        txtQuery.Visible = argFlag
        vsfFlds.Visible = Not argFlag
        lblInfo.Visible = Not argFlag
    End Sub

    Private Sub optQry_CheckedChanged(sender As Object, e As EventArgs) Handles optQry.CheckedChanged
        enableDisableFlds(True)
    End Sub

    Private Sub optStdTbls_CheckedChanged(sender As Object, e As EventArgs) Handles optStdTbls.CheckedChanged
        enableDisableFlds(False)
    End Sub
End Class