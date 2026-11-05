Imports System.Data.oledb
Imports C1.Win.C1FlexGrid
Imports System.IO
Imports System.ComponentModel

Public Class FrmUtlPrintOptionname
    ' Dim svtTrn As oledb.oledbTransaction
    'For Grid vsfPrintoptions
    Const vDetFormulaField = 0
    Const vDetTable = 1
    Const vDetFormulaValue = 2
    Const intvDetTotCols = 3

    Const vESlno = 0
    Const vEFixed = 1
    Const vEType = 2
    Const vEMailID = 3
    Const vETrnFld = 4
    Const vECond = 5
    Const vEDtls = 6
    Const vETotal = 7

    Dim strSQL As String
    Dim clsSVT As New svtLibrary

    Dim intDocID As Integer
    Dim strOptioName As String

    Dim strFrmName As String = "Print option Name"
    Public Mode As String

    Private Sub setCheckBox(ByVal argChkBox As CheckBox, argColName As String)
        If argColName = "Y" Then
            argChkBox.Checked = True
        Else
            argChkBox.Checked = False
        End If
    End Sub

    Private Function LoadFormList() As Boolean
        Dim strSQL As String
        Dim arrFrmlFld, arrFrmlVal As Array
        Dim rsDummy As oledbDataReader
        Dim ipos As Integer

        btnExistTrans.Enabled = False
        btnDiffTrans.Enabled = False

        Try
            LoadFormList = False
            txtPrintName.Text = strOptioName
            strSQL = "Select * from utloption"
            strSQL = strSQL & " Where DocID = " & intDocID & " and OptionName = '" & strOptioName & "'"
            rsDummy = clsSVT.getDataReaderFromSQL(strSQL)
            Using rsDummy
                With rsDummy
                    If Not rsDummy.HasRows Then
                        Exit Function
                    End If
                    If .Read() Then
                        txtNoOfCopy.Text = CInt(GetVal(!NoOfCopies, 0))
                        Call setCheckBox(ChkDefault, !DefaultPrint)
                        Select Case GetVal(!ClickOperation, "")
                            Case "E"
                                cboPrintClick.Text = "Email"
                            Case "P"
                                cboPrintClick.Text = "Preview"
                            Case "D"
                                cboPrintClick.Text = "Direct Print"
                            Case "M"
                                cboPrintClick.Text = "Multi Print"
                            Case Else
                                cboPrintClick.Text = ""
                        End Select

                        Select Case GetVal(!CtrlClickOperation, "")
                            Case "E"
                                cboPrintCtrlClick.Text = "Email"
                            Case "P"
                                cboPrintCtrlClick.Text = "Preview"
                            Case "D"
                                cboPrintCtrlClick.Text = "Direct Print"
                            Case "M"
                                cboPrintCtrlClick.Text = "Multi Print"
                            Case Else
                                cboPrintCtrlClick.Text = ""
                        End Select

                        Select Case GetVal(!CtrlAltClickOperation, "")
                            Case "E"
                                cboPrintCtrlAltClick.Text = "Email"
                            Case "P"
                                cboPrintCtrlAltClick.Text = "Preview"
                            Case "D"
                                cboPrintCtrlAltClick.Text = "Direct Print"
                            Case "M"
                                cboPrintCtrlAltClick.Text = "Multi Print"
                            Case Else
                                cboPrintCtrlAltClick.Text = ""
                        End Select

                        Select Case Trim(GetVal(!AfterSaveAction, ""))
                            Case "P"
                                RdoPrompt.Checked = True
                            Case "PP"
                                RdoWithoutPrompt.Checked = True
                            Case Else
                                RdoDontPrint.Checked = True
                        End Select

                        If GetVal(!EmailRouting, "") = "S" Then
                            rdoSMTP.Checked = True
                        Else
                            rdoOutLook.Checked = True
                        End If
                        Call setCheckBox(chkAttachCopy, GetVal(!DoNotAttach, ""))
                        txtPrinterName.Text = GetVal(!DirectPrinterName, "")
                        txtSubject.Text = GetVal(!EmailSubject, "")
                        txtBody.Text = GetVal(!EmailBody, "")
                        txtFooter.Text = GetVal(!EmailFooter, "")
                        txtCond.Text = GetVal(!EmailCond, "")

                        Call setCheckBox(chkDateFilter, GetVal(!AllowDateRange))
                        Call setCheckBox(chkDocNoRange, GetVal(!AllowDocNoRange))
                        Call setCheckBox(chkTrackPrinted, GetVal(!TrackPrinted))
                        txtSqlQuery.Text = GetVal(!MultiPrintQuery, "")


                        Dim fileNames = My.Computer.FileSystem.GetFiles(gstrRptPath, FileIO.SearchOption.SearchTopLevelOnly, "*.rpt")
                        cboRpt.Text = GetVal(!CrptFileName, "")
                        cboRpt.Items.Clear()
                        For Each fileName As String In fileNames
                            cboRpt.Items.Add(Path.GetFileName(fileName))
                        Next
                        txtFormula.Text = GetVal(!SelFormula, "")
                    End If

                    arrFrmlFld = Split(!FormulasFld, ";")
                    arrFrmlVal = Split(!Formulasval, ";")
                    If arrFrmlFld.Length >= 1 Then
                        vsfDtl.Rows.Count = 1
                        For i = 0 To UBound(arrFrmlFld)
                            With vsfDtl
                                .Rows.Count = .Rows.Count + 1
                                .SetData(.Rows.Count - 1, vDetFormulaField, arrFrmlFld(i))
                                ipos = InStr(1, arrFrmlVal(i), ".")
                                If ipos > 1 Then
                                    .SetData(.Rows.Count - 1, vDetTable, Strings.Left(arrFrmlVal(i), ipos - 1))
                                    .SetData(.Rows.Count - 1, vDetFormulaValue, Mid(arrFrmlVal(i), ipos + 1))
                                Else
                                    .SetData(.Rows.Count - 1, vDetTable, (arrFrmlVal(i)))
                                    .SetData(.Rows.Count - 1, vDetFormulaValue, arrFrmlVal(i))
                                End If
                            End With
                        Next i
                    End If
                End With
            End Using

            strSQL = "Select * from UtlOptionEmailDtls"
            strSQL = strSQL & " Where DocID = " & intDocID
            strSQL = strSQL & " and OptionOrder = (Select OptionOrder From UtlOption Where  DocID = " & intDocID & " and OptionName = '" & strOptioName & "') "
            strSQL = strSQL & " Order by Slno "
            rsDummy = clsSVT.getDataReaderFromSQL(strSQL)

            With vsfeMail
                .Rows.Count = 1
                If rsDummy.HasRows Then
                    Do While rsDummy.Read
                        .Rows.Count = .Rows.Count + 1

                        .SetData(.Rows.Count - 1, vESlno, rsDummy.Item("Slno"))
                        .SetData(.Rows.Count - 1, vEFixed, rsDummy.Item("FixedEmail"))
                        .SetData(.Rows.Count - 1, vEType, rsDummy.Item("eMailType"))
                        .SetData(.Rows.Count - 1, vEMailID, rsDummy.Item("EmailID"))
                        .SetData(.Rows.Count - 1, vETrnFld, rsDummy.Item("TxnFldName"))
                        .SetData(.Rows.Count - 1, vECond, rsDummy.Item("TxnCondition"))

                        Dim strDtl As String = ""
                        strDtl &= "Sending Type: " & .GetData(.Rows.Count - 1, vEType)
                        strDtl &= "; Fixed: " & .GetData(.Rows.Count - 1, vEFixed)
                        If .GetData(.Rows.Count - 1, vEFixed) = "Y" Then
                            strDtl &= "; Fixed Email: " & .GetData(.Rows.Count - 1, vEMailID)
                        Else
                            strDtl &= "; Field: " & .GetData(.Rows.Count - 1, vETrnFld)
                            strDtl &= "; Field Cond: " & .GetData(.Rows.Count - 1, vECond)
                        End If
                        .SetData(.Rows.Count - 1, vEDtls, strDtl)
                    Loop
                End If
                rsDummy.Close()
            End With



            LoadFormList = True
            Exit Function
        Catch ex As Exception
            MsgBox("Error during Load" & vbCrLf & ex.Message, MsgBoxStyle.Critical, Me.Text & " [LoadFormList]")
            LoadFormList = False
        End Try
    End Function

    Private Sub LoadFormnew()
        Dim fileNames = My.Computer.FileSystem.GetFiles(gstrRptPath, FileIO.SearchOption.SearchTopLevelOnly, "*.rpt")
        cboRpt.Items.Clear()
        For Each fileName As String In fileNames
            cboRpt.Items.Add(Path.GetFileName(fileName))
        Next
        'RdoPrintPreview.Checked = True
        RdoDontPrint.Checked = True
        txtNoOfCopy.Text = 1
        btnExistTrans.Enabled = True
        btnDiffTrans.Enabled = True
    End Sub

    Private Sub clearflds()
        ChkDefault.Checked = False
        RdoPrompt.Checked = False
        RdoWithoutPrompt.Checked = False
        RdoDontPrint.Checked = False
        txtPrintName.Text = ""
        txtPrinterName.Text = ""
        cboRpt.SelectedIndex = -1
        txtFormula.Text = ""
        txtNoOfCopy.Text = 0
        cboPrintClick.SelectedIndex = -1
        cboPrintCtrlClick.SelectedIndex = -1


        rdoOutLook.Checked = True
        chkAttachCopy.Checked = False

        vsfeMail.Rows.Count = 1

        txtSubject.Text = ""
        txtBody.Text = ""
        txtFooter.Text = ""
        txtCond.Text = ""

        txtSqlQuery.Text = ""
        chkDateFilter.Checked = False
        chkDocNoRange.Checked = False
        chkTrackPrinted.Checked = False
        cboPrintCtrlAltClick.SelectedIndex = -1
    End Sub

    Private Sub initGrid()
        With vsfDtl
            .Rows.Count = 1
            .Cols.Count = intvDetTotCols
            .SetData(0, vDetFormulaField, "Formula Field")
            .SetData(0, vDetTable, "Table")
            .SetData(0, vDetFormulaValue, "Formula Value")
        End With
        Call SetRowAlign(vsfDtl, svtAlign.Center, 0)

        With vsfeMail
            .Rows.Count = 1
            .Cols.Count = vETotal

            .SetData(0, vEDtls, "eMail Details")

            Call SetFlexColor(vsfeMail, "", vEDtls)
            Call SetHideCol(vsfeMail, True, vESlno, vEFixed, vEType, vEMailID, vETrnFld, vECond)
        End With
    End Sub

    Private Sub EnableCntrls(argType As Integer, argBln As Boolean)
        'Enable Controls
        Select Case argType
            Case 1
                btnSave.Visible = argBln
                btnCancel.Visible = argBln
            Case 2
                btnSave.Visible = argBln
                btnCancel.Visible = argBln
        End Select
    End Sub

    Private Sub FrmPrintOptionname_KeyUp(sender As Object, e As KeyEventArgs) Handles Me.KeyUp
        If e.KeyCode = Keys.F9 And e.Control = True Then
            Call initFormSize(Me, strFrmName, True, 594, 505, 0, 0)
            Call initFormVsfDet(Me, strFrmName, vsfDtl, "DET")
            Call initFormVsfDet(Me, strFrmName, vsfeMail, "MAIL")
            MsgBox("Restored Form & Grid Layout Successfully!!", vbInformation, strFrmName)
        End If
        If e.KeyCode = Keys.F8 And e.Control = True Then
            Call SaveActFormSettings(Me, strFrmName)
            Call SaveActFormVsfDet(Me, strFrmName, "DET", vsfDtl)
            Call initFormVsfDet(Me, strFrmName, vsfeMail, "MAIL")
            MsgBox("Saved Form & Grid Layout Successfully!!", vbInformation, strFrmName)
        End If
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Call clearflds()
        'FrmUtlPrintSettings.ShowDialog()
        Me.Close()
    End Sub

    Private Function validFlds() As Boolean
        validFlds = False

        If Trim(txtPrintName.Text) = "" Then

            Exit Function
        End If

    End Function

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Dim strPrintsave As String = ""
        Dim strDefltprint As String
        Dim strPrintName As String
        Dim strFormulasFld As String = ""
        Dim strFormulasval As String = ""

        Dim strPrintClick As String
        Dim strPrintCtrlClick As String
        Dim strPrintCtrlAltClick As String

        Try
            If Mode = "A" Then
                strPrintName = clsSVT.getDataStringValue("select distinct Optionname from  UTLOption  
                            where DocID = " & txtID.Text & " And
                            optionName in ('" & GetVal(txtPrintName.Text, "") & "')", "", False)

                If Not String.IsNullOrEmpty(strPrintName) Then
                    MsgBox("option Name Already Exist!", vbInformation, Me.Text)
                    Exit Sub
                End If

            End If

            Select Case Trim(cboPrintClick.Text)
                Case "Email"
                    strPrintClick = "E"
                Case "Preview"
                    strPrintClick = "P"
                Case "Direct Print"
                    strPrintClick = "D"
                Case "Multi Print"
                    strPrintClick = "M"
                Case Else
                    MsgBox("Invalid Print Click....", MsgBoxStyle.Information, Me.Text)
                    Exit Sub
            End Select

            Select Case Trim(cboPrintCtrlClick.Text)
                Case "Email"
                    strPrintCtrlClick = "E"
                Case "Preview"
                    strPrintCtrlClick = "P"
                Case "Direct Print"
                    strPrintCtrlClick = "D"
                Case "Multi Print"
                    strPrintCtrlClick = "M"
                Case Else
                    strPrintCtrlClick = ""
            End Select

            Select Case Trim(cboPrintCtrlAltClick.Text)
                Case "Email"
                    strPrintCtrlAltClick = "E"
                Case "Preview"
                    strPrintCtrlAltClick = "P"
                Case "Direct Print"
                    strPrintCtrlAltClick = "D"
                Case "Multi Print"
                    strPrintCtrlAltClick = "M"
                Case Else
                    strPrintCtrlAltClick = ""
            End Select

            If RdoPrompt.Checked Then
                strPrintsave = "P"
            ElseIf RdoWithoutPrompt.Checked Then
                strPrintsave = "PP"
            ElseIf RdoDontPrint.Checked Then
                strPrintsave = "DNP"
            End If

            If ChkDefault.Checked Then
                strDefltprint = "Y"
                strSQL = "Update  UTLOption set DefaultPrint='N' where DocID = " & Val(txtID.Text)
                clsSVT.execSQLStatement(strSQL)
            Else
                strDefltprint = "N"
            End If

            For i = 1 To vsfDtl.Rows.Count - 1
                If Not String.IsNullOrEmpty(vsfDtl.GetData(i, vDetFormulaField)) Then
                    strFormulasFld &= vsfDtl.GetData(i, vDetFormulaField) & ";"
                    strFormulasval &= vsfDtl.GetData(i, vDetTable) & "." & vsfDtl.GetData(i, vDetFormulaValue) & ";"
                    strFormulasFld = strFormulasFld.Replace("'", "`")
                    strFormulasval = strFormulasval.Replace("'", "`")
                End If
            Next

            If strFormulasFld <> "" Then
                strFormulasFld = Mid(strFormulasFld, 1, Len(strFormulasFld) - 1)
            End If
            If strFormulasval <> "" Then
                strFormulasval = Mid(strFormulasval, 1, Len(strFormulasval) - 1)
            End If

            If Mode = "A" Then
                strSQL = "insert into UTLOption ("
                strSQL = strSQL & " DocID, OptionOrder, OptionName, CrptFileName,"
                strSQL = strSQL & " SelFormula, FormulasFld, FormulasVal, NoOfCopies, "
                strSQL = strSQL & " Defaultprint, AfterSaveAction,  "
                strSQL = strSQL & " DirectPrinterName,ClickOperation,CtrlClickOperation,EmailRouting, "
                strSQL = strSQL & " DoNotAttach,EmailSubject,EmailBody,EmailFooter,EmailCond,CtrlAltClickOperation,AllowDateRange,AllowDocNoRange,TrackPrinted,MultiPrintQuery "
                strSQL = strSQL & " ) values ( "
                strSQL = strSQL & GetVal(txtID.Text, 0) & ","
                strSQL = strSQL & clsSVT.getDataLongValue("select isnull(max(Optionorder),0) + 1 from utloption where DocID=" & Val(txtID.Text), 1, False) & ","
                strSQL = strSQL & "'" & GetVal(Trim(txtPrintName.Text), "") & "',"
                strSQL = strSQL & "'" & GetVal(Trim(cboRpt.Text), "") & "',"
                strSQL = strSQL & "'" & GetVal(Trim(txtFormula.Text), "") & "',"
                strSQL = strSQL & "'" & strFormulasFld & "',"
                strSQL = strSQL & "'" & strFormulasval & "',"
                strSQL = strSQL & "" & GetVal(txtNoOfCopy.Text, 0) & ","
                strSQL = strSQL & "'" & strDefltprint & "',"
                strSQL = strSQL & "'" & strPrintsave & "',"
                strSQL = strSQL & "'" & GetVal(txtPrinterName.Text, "") & "',"
                strSQL = strSQL & "'" & strPrintClick & "',"
                strSQL = strSQL & "'" & strPrintCtrlClick & "',"
                strSQL = strSQL & "'" & strPrintCtrlAltClick & "',"

                If strPrintClick = "E" Or strPrintCtrlClick = "E" Or strPrintCtrlAltClick = "E" Then
                    If rdoOutLook.Checked Then
                        strSQL = strSQL & "'O',"
                    Else
                        strSQL = strSQL & "'S',"
                    End If
                    strSQL = strSQL & "'" & getCheckString(chkAttachCopy) & "',"
                    strSQL = strSQL & "'" & CStr(txtSubject.Text & "").Replace("'", "''") & "',"
                    strSQL = strSQL & "'" & CStr(txtBody.Text & "").Replace("'", "''") & "',"
                    strSQL = strSQL & "'" & CStr(txtFooter.Text & "").Replace("'", "''") & "',"
                    strSQL = strSQL & "'" & CStr(txtCond.Text & "").Replace("'", "''") & "',"
                Else
                    strSQL = strSQL & "'N',"
                    strSQL = strSQL & "'',"
                    strSQL = strSQL & "'',"
                    strSQL = strSQL & "'',"
                    strSQL = strSQL & "'',"
                    strSQL = strSQL & "'N',"
                End If

                If strPrintClick = "M" Or strPrintCtrlClick = "M" Or strPrintCtrlAltClick = "E" Then
                    strSQL = strSQL & ",'" & getCheckString(chkDateFilter) & "',"
                    strSQL = strSQL & "'" & getCheckString(chkDocNoRange) & "',"
                    strSQL = strSQL & "'" & getCheckString(chkTrackPrinted) & "',"
                    strSQL = strSQL & "'" & Trim(txtSqlQuery.Text) & "')"
                Else
                    strSQL = strSQL & "'N',"
                    strSQL = strSQL & "'N',"
                    strSQL = strSQL & "'N',"
                    strSQL = strSQL & "'')"
                End If
            Else
                strSQL = "Update  UTLOption set "
                strSQL = strSQL & " OptionName = '" & GetVal(Trim(txtPrintName.Text), "") & "',"
                strSQL = strSQL & " CrptFileName= '" & GetVal(Trim(cboRpt.Text), "") & "',"
                strSQL = strSQL & " SelFormula='" & GetVal(Trim(txtFormula.Text), "") & "',"
                strSQL = strSQL & " FormulasFld='" & strFormulasFld & "',"
                strSQL = strSQL & " FormulasVal='" & strFormulasval & "',"
                strSQL = strSQL & " NoOfCopies=" & GetVal(txtNoOfCopy.Text, 0) & ","
                strSQL = strSQL & " Defaultprint='" & strDefltprint & "',"
                strSQL = strSQL & " DirectPrinterName='" & GetVal(Trim(txtPrinterName.Text), "") & "',"
                strSQL = strSQL & " AfterSaveAction='" & strPrintsave & "',"
                strSQL = strSQL & " ClickOperation='" & strPrintClick & "',"
                strSQL = strSQL & " CtrlClickOperation='" & strPrintCtrlClick & "',"
                strSQL = strSQL & " CtrlAltClickOperation='" & strPrintCtrlAltClick & "',"

                If strPrintClick = "E" Or strPrintCtrlClick = "E" Or strPrintCtrlAltClick = "E" Then
                    If rdoOutLook.Checked Then
                        strSQL = strSQL & " EmailRouting='O',"
                    Else
                        strSQL = strSQL & " EmailRouting='S',"
                    End If
                    strSQL = strSQL & " DoNotAttach='" & getCheckString(chkAttachCopy) & "',"
                    strSQL = strSQL & " EmailSubject='" & CStr(txtSubject.Text & "").Replace("'", "''") & "',"
                    strSQL = strSQL & " EmailBody='" & CStr(txtBody.Text & "").Replace("'", "''") & "',"
                    strSQL = strSQL & " EmailFooter='" & CStr(txtFooter.Text & "").Replace("'", "''") & "',"
                    strSQL = strSQL & " EmailCond='" & CStr(txtCond.Text & "").Replace("'", "''") & "'"
                Else
                    strSQL = strSQL & " EmailRouting='N',"
                    strSQL = strSQL & " DoNotAttach='',"
                    strSQL = strSQL & " EmailSubject='',"
                    strSQL = strSQL & " EmailBody='',"
                    strSQL = strSQL & " EmailFooter='',"
                    strSQL = strSQL & " EmailCond=''"
                End If


                If strPrintClick = "M" Or strPrintCtrlClick = "M" Or strPrintCtrlAltClick = "M" Then
                    strSQL = strSQL & ", AllowDateRange='" & getCheckString(chkDateFilter) & "',"
                    strSQL = strSQL & "AllowDocNoRange='" & getCheckString(chkDocNoRange) & "',"
                    strSQL = strSQL & "TrackPrinted='" & getCheckString(chkTrackPrinted) & "',"
                    strSQL = strSQL & "MultiPrintQuery='" & Trim(txtSqlQuery.Text) & "'"
                Else
                    strSQL = strSQL & ", AllowDateRange='N',"
                    strSQL = strSQL & "AllowDocNoRange='N',"
                    strSQL = strSQL & "TrackPrinted='N',"
                    strSQL = strSQL & "MultiPrintQuery=''"
                End If
                strSQL = strSQL & " where DocID=" & GetVal((txtID.Text), "") & " and OptionName='" & strOptioName & "'"
            End If

            If Not clsSVT.execSQLStatement(strSQL) Then
                ' svtTrn.Rollback()
                Exit Sub
            End If

            With vsfeMail
                strSQL = "Delete from UtlOptionEmailDtls where DocID=" & GetVal((txtID.Text), 0) & " and OptionOrder = (Select OptionOrder From UtlOption Where DocID=" & GetVal((txtID.Text), 0) & " and OptionName='" & Trim(txtPrintName.Text) & "')"
                If Not clsSVT.execSQLStatement(strSQL) Then
                    '  svtTrn.Rollback()
                    Exit Sub
                End If

                If .Rows.Count > 1 Then
                    Dim blnEmailExec As Boolean = False
                    If strPrintClick = "E" Or strPrintCtrlClick = "E" Or strPrintCtrlAltClick = "E" Then
                        Call ReArrangeSlNo(vsfeMail, vESlno)
                        Call RemoveEmptVSFRows(vsfeMail, 1, vEType)
                        strSQL = " Insert into UtlOptionEmailDtls (DocID,OptionOrder,Slno,FixedEmail,eMailType,EmailID,TxnFldName,TxnCondition) "
                        With vsfeMail
                            For i As Int32 = 1 To .Rows.Count - 1
                                If GetVal(.GetData(i, vEType), "") <> "" Then
                                    blnEmailExec = True
                                    strSQL &= " Select DocID, OptionOrder, "
                                    strSQL &= .GetData(i, vESlno) & ","
                                    strSQL &= "'" & .GetData(i, vEFixed) & "',"
                                    strSQL &= "'" & .GetData(i, vEType) & "',"
                                    If GetVal(.GetData(i, vEFixed), "") = "Y" Then
                                        strSQL &= "'" & .GetData(i, vEMailID) & "',"
                                        strSQL &= "'',"
                                        strSQL &= "'' "
                                    Else
                                        strSQL &= "'',"
                                        strSQL &= "'" & CStr(.GetData(i, vETrnFld) & "").Replace("'", "''") & "',"
                                        strSQL &= "'" & CStr(.GetData(i, vECond) & "").Replace("'", "''") & "' "
                                    End If
                                    strSQL &= " From UtlOption Where DocID=" & GetVal((txtID.Text), 0) & " and OptionName='" & Trim(txtPrintName.Text) & "' "
                                    strSQL &= " Union All"
                                End If
                            Next
                        End With
                    End If

                    If blnEmailExec Then
                        strSQL = Mid(strSQL, 1, Len(strSQL) - 9)
                        If Not clsSVT.execSQLStatement(strSQL) Then
                            ' svtTrn.Rollback()
                            Exit Sub
                        End If
                    End If
                End If

            End With

            If Mode = "A" Then
                MsgBox("Data saved successfully!", vbInformation, Me.Text)
            Else
                MsgBox("Updated Successfully!", vbInformation, Me.Text)
            End If

            '  svtTrn.Commit()
            Me.Close()
        Catch ex As Exception
            MsgBox("Error during " & btnSave.Text & "" & vbCrLf & ex.Message, MsgBoxStyle.Critical, Me.Text & " [BtnSave]")
            ' svtTrn.Rollback()
        End Try
    End Sub

    Private Sub btnremv_Click(sender As Object, e As EventArgs) Handles btnRemove.Click
        If vsfDtl.Rows.Count <= 1 Then Exit Sub
        If MsgBox("Are you sure you want to remove", MsgBoxStyle.YesNo + vbDefaultButton2 + MsgBoxStyle.Critical, "deletion confirmation") = vbNo Then Exit Sub

        vsfDtl.Rows.Remove(vsfDtl.Row)
        If vsfDtl.Rows.Count = 1 Then vsfDtl.Rows.Count = 2
    End Sub

    Private Sub vsfdtl_BeforeEdit(sender As Object, e As RowColEventArgs) Handles vsfDtl.BeforeEdit
        vsfDtl.ComboList = ""
        If e.Col = vDetTable Then
            vsfDtl.ComboList = GenVSfComboStringCond("name", "sys.tables", "name")
            Exit Sub
        End If

        If e.Col = vDetFormulaValue Then
            vsfDtl.ComboList = GenVSfComboStringCond("column_name", "Information_schema.Columns", " order by Column_name", " where table_name='" & vsfDtl.GetData(vsfDtl.Row, vDetTable) & "'")
            Exit Sub
        End If
    End Sub

    Private Sub txtCopies_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtNoOfCopy.KeyPress
        If Not KeyValidate("Integer", e.KeyChar) Then
            e.Handled = True
        End If

        If e.KeyChar = "'" Or e.KeyChar = "," Then
            e.Handled = True
        End If
    End Sub

    Private Sub txtPrtName_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtPrintName.KeyPress
        If e.KeyChar = "'" Then
            e.Handled = True
        End If
    End Sub

    Private Sub txtPrntName_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtPrinterName.KeyPress
        If e.KeyChar = "'" Then
            e.Handled = True
        End If
    End Sub

    Private Sub txtFrmla_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtFormula.KeyPress
        If e.KeyChar = "'" Then
            e.Handled = True
        End If
    End Sub

    Public Sub loadOptionDetails(argDociD As Integer, argOptionname As String)
        'Initialize with Default columns in Grid and grid Styles
        Call initGridStyles(Me)
        Call initGrid()
        Call initFormVsfDet(Me, strFrmName, vsfDtl, "DET")
        Call initFormVsfDet(Me, strFrmName, vsfeMail, "MAIL")
        'Initialize Form Properties with default settings
        Call fillFormProperties(Me)
        'Fill Data
        intDocID = argDociD
        strOptioName = argOptionname
        txtID.Text = argDociD
        If Mode = "M" Then
            Call clearflds()
            txtPrintName.Text = argOptionname
            Call LoadFormList()
        ElseIf Mode = "A" Then
            Call LoadFormnew()
            Dim strShForm As String = clsSVT.getDataStringValue("Select DocShForm from UtlTransactions Where DocID=" & intDocID, "", False)
            If strShForm <> "" Then
                txtFormula.Text = "{TrnHdr" & strShForm & "." & strShForm & "ID}="
            End If
        End If
        vsfDtl.Rows.Count = vsfDtl.Rows.Count + 1
        Call ChangeFontSize(Me)
    End Sub

    Private Sub cboRpt_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cboRpt.KeyPress
        If e.KeyChar = "'" Then
            e.Handled = True
        End If
    End Sub

    Private Sub btnAddRow_Click(sender As Object, e As EventArgs) Handles btnAddRow.Click
        gStrDummy = ""
        Dim argFrm As New frmUtlFixedEmail
        argFrm.Text = "Add Email"
        argFrm.StartPosition = FormStartPosition.CenterScreen
        Call argFrm.LoadDataFromGrid()
        argFrm.ShowDialog()
        argFrm.Dispose()

        If Trim(gStrDummy) <> "" Then
            With vsfeMail
                .Rows.Count = .Rows.Count + 1
                Dim i As Int32 = .Rows.Count - 1
                Dim j As Int32 = 0

                .SetData(i, vESlno, .Rows.Count)
                For Each strValue As String In gStrDummy.Split("■")
                    .SetData(i, j + 1, strValue)
                    j = j + 1
                Next

                Dim strDtl As String = ""
                strDtl &= "Sending Type: " & .GetData(i, vEType)
                strDtl &= "; Fixed: " & .GetData(i, vEFixed)
                If .GetData(i, vEFixed) = "Y" Then
                    strDtl &= "; Fixed Email: " & .GetData(i, vEMailID)
                Else
                    strDtl &= "; Field: " & .GetData(i, vETrnFld)
                    strDtl &= "; Field Cond: " & .GetData(i, vECond)
                End If
                .SetData(i, vEDtls, strDtl)
            End With
        End If
    End Sub

    Private Sub btnEmailSettings_Click(sender As Object, e As EventArgs) Handles btnEmailSettings.Click
        gbEmail.Visible = True
        gbEmail.Dock = DockStyle.Fill
    End Sub

    Private Sub cboPrintClick_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboPrintClick.SelectedIndexChanged
        Call ChkCtrl()
        If cboPrintClick.SelectedIndex > 0 Then
            If cboPrintClick.SelectedIndex = cboPrintCtrlClick.SelectedIndex Or cboPrintClick.SelectedIndex = cboPrintCtrlAltClick.SelectedIndex Then
                MsgBox("Cannot be selected !", vbInformation, Me.Text)
                cboPrintClick.SelectedIndex = 0
            End If
        End If
    End Sub

    Private Sub cboPrintCtrlClick_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboPrintCtrlClick.SelectedIndexChanged
        Call ChkCtrl()
        If cboPrintCtrlClick.SelectedIndex > 0 Then
            If cboPrintCtrlClick.SelectedIndex = cboPrintClick.SelectedIndex Or cboPrintCtrlClick.SelectedIndex = cboPrintCtrlAltClick.SelectedIndex Then
                MsgBox("Cannot be selected !", vbInformation, Me.Text)
                cboPrintCtrlClick.SelectedIndex = 0
            End If
        End If
    End Sub

    Private Sub cboPrintCtrlAltClick_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboPrintCtrlAltClick.SelectedIndexChanged
        Call ChkCtrl()
        If cboPrintCtrlAltClick.SelectedIndex > 0 Then
            If cboPrintCtrlAltClick.SelectedIndex = cboPrintClick.SelectedIndex Or cboPrintCtrlAltClick.SelectedIndex = cboPrintCtrlClick.SelectedIndex Then
                MsgBox("Cannot be selected !", vbInformation, Me.Text)
                cboPrintCtrlAltClick.SelectedIndex = 0
            End If
        End If
    End Sub

    Private Sub ChkCtrl()

        'If Trim(cboPrintClick.Text) <> Trim(cboPrintCtrlClick.Text) AndAlso Trim(cboPrintClick.Text) <> Trim(cboPrintCtrlAltClick.Text) AndAlso Trim(cboPrintCtrlClick.Text) <> Trim(cboPrintCtrlAltClick.Text) Then

        If Trim(cboPrintClick.Text) = "" AndAlso Trim(cboPrintCtrlClick.Text) = "" AndAlso Trim(cboPrintCtrlAltClick.Text) = "" Then
            btnMultiPrint.Visible = False
            btnEmailSettings.Visible = False
            txtPrinterName.Visible = False
            lblPrinterName.Visible = False
            Exit Sub
        End If

        If Trim(cboPrintClick.Text) = "Direct Print" OrElse Trim(cboPrintCtrlClick.Text) = "Direct Print" OrElse Trim(cboPrintCtrlAltClick.Text) = "Direct Print" Then
            txtPrinterName.Visible = True
            lblPrinterName.Visible = True
        Else
            txtPrinterName.Visible = False
            lblPrinterName.Visible = False
        End If

        If Trim(cboPrintClick.Text) = "Email" OrElse Trim(cboPrintCtrlClick.Text) = "Email" OrElse Trim(cboPrintCtrlAltClick.Text) = "Email" Then
            btnEmailSettings.Visible = True
        Else
            btnEmailSettings.Visible = False
        End If

        If Trim(cboPrintClick.Text) = "Multi Print" OrElse Trim(cboPrintCtrlClick.Text) = "Multi Print" OrElse Trim(cboPrintCtrlAltClick.Text) = "Multi Print" Then
            btnMultiPrint.Visible = True
        Else
            btnMultiPrint.Visible = False
        End If
        'End If

    End Sub

    Private Sub btnEMailOK_Click(sender As Object, e As EventArgs) Handles btnEMailOK.Click
        gbEmail.Visible = False
    End Sub

    Private Sub btnEmailCancel_Click(sender As Object, e As EventArgs) Handles btnEmailCancel.Click
        gbEmail.Visible = False
    End Sub

    Private Sub vsfeMail_DoubleClick(sender As Object, e As EventArgs) Handles vsfeMail.DoubleClick
        If vsfeMail.Rows.Count <= 1 Then Exit Sub
        If vsfeMail.Row <= 0 Then Exit Sub

        gStrDummy = ""
        Dim argFrm As New frmUtlFixedEmail
        argFrm.Text = "Edit Email"
        argFrm.StartPosition = FormStartPosition.CenterScreen
        With vsfeMail
            Call argFrm.LoadDataFromGrid(GetVal(.GetData(.Row, vEType), ""), GetVal(.GetData(.Row, vEFixed), ""), GetVal(.GetData(.Row, vEMailID), ""), GetVal(.GetData(.Row, vETrnFld), ""), GetVal(.GetData(.Row, vECond), ""))
        End With
        argFrm.ShowDialog()
        argFrm.Dispose()

        If gStrDummy <> "" Then
            With vsfeMail
                Dim i As Int32 = .Row
                Dim j As Int32 = 0

                For Each strValue As String In gStrDummy.Split("■")
                    .SetData(i, j + 1, strValue)
                    j = j + 1
                Next

                Dim strDtl As String = ""
                strDtl &= "Sending Type: " & .GetData(i, vEType)
                strDtl &= "; Fixed: " & .GetData(i, vEFixed)
                If .GetData(i, vEFixed) = "Y" Then
                    strDtl &= "; Fixed Email: " & .GetData(i, vEMailID)
                Else
                    strDtl &= "; Field: " & .GetData(i, vETrnFld)
                    strDtl &= "; Field Cond: " & .GetData(i, vECond)
                End If
                .SetData(i, vEDtls, strDtl)
            End With
        End If

    End Sub

    Private Sub vsfeMail_BeforeEdit(sender As Object, e As RowColEventArgs) Handles vsfeMail.BeforeEdit
        e.Cancel = True
    End Sub

    Private Sub vsfDtl_KeyUp(sender As Object, e As KeyEventArgs) Handles vsfDtl.KeyUp
        If e.KeyCode = Keys.Tab Then
            If vsfDtl.Rows.Count > 1 AndAlso vsfDtl.Row = vsfDtl.Rows.Count - 1 AndAlso vsfDtl.Col = vDetFormulaValue Then
                If vsfDtl.GetData(vsfDtl.Row, vDetFormulaField) <> "" Then
                    vsfDtl.Rows.Count = vsfDtl.Rows.Count + 1
                End If
            End If
        End If
    End Sub

    Private Sub btnSMTP_Click(sender As Object, e As EventArgs) Handles btnSMTP.Click
        Dim obj As New frmUtlSMTPSettings
        obj.ShowDialog()
        obj.Dispose()
    End Sub

    Private Sub rdoOutLook_CheckedChanged(sender As Object, e As EventArgs) Handles rdoOutLook.CheckedChanged
        If rdoOutLook.Checked Then
            btnSMTP.Visible = False
        Else
            btnSMTP.Visible = True
        End If
    End Sub

    Private Sub rdoSMTP_CheckedChanged(sender As Object, e As EventArgs) Handles rdoSMTP.CheckedChanged
        If Not rdoSMTP.Checked Then
            btnSMTP.Visible = False
        Else
            btnSMTP.Visible = True
        End If
    End Sub

    Private Sub btnMultiPrint_Click(sender As Object, e As EventArgs) Handles btnMultiPrint.Click
        gbMultiPrint.Visible = True
        gbMultiPrint.Dock = DockStyle.Fill
    End Sub

    Private Sub btnMultiPrintOK_Click(sender As Object, e As EventArgs) Handles btnMultiPrintOK.Click
        gbMultiPrint.Visible = False
    End Sub

    Private Sub btnMultiPrintCancel_Click(sender As Object, e As EventArgs) Handles btnMultiPrintCancel.Click
        gbMultiPrint.Visible = False
    End Sub

    Private Sub btnExistTrans_Click(sender As Object, e As EventArgs) Handles btnExistTrans.Click
        If intDocID <= 0 Then Exit Sub

        txtdummy1.Text = ""

        Dim strSQL As String = " select DocID, OptionName from utloption Where DocID=" & intDocID & ""

        Dim frmDisplay As New frmUtlSingleSel

        With frmDisplay
            .Text = "Select Print Option..."
            Call .FillViewVsf(Me, strSQL, txtdummy1, 1, 0)
            .ShowDialog()
        End With

        If Trim(txtdummy1.Text) <> "" Then
            Dim strPname As String = txtPrintName.Text
            Dim strRpt As String = cboRpt.Text
            Dim strFormula As String = txtFormula.Text

            Call clearflds()
            strOptioName = txtdummy1.Text
            Call LoadFormList()
            txtPrintName.Text = ""
            'cboRpt.Text = ""
            txtFormula.Text = ""
            txtPrintName.Enabled = True

            txtPrintName.Text = strPname
            cboRpt.Text = strRpt
            txtFormula.Text = strFormula
            Mode = "A"
        End If

    End Sub

    Private Sub btnRemoveRow_Click(sender As Object, e As EventArgs) Handles btnRemoveRow.Click
        If vsfeMail.Rows.Count <= 1 Then Exit Sub
        If MsgBox("Are you sure you want to remove", MsgBoxStyle.YesNo + vbDefaultButton2 + MsgBoxStyle.Critical, "deletion confirmation") = vbNo Then Exit Sub

        vsfeMail.Rows.Remove(vsfeMail.Row)
        If vsfeMail.Rows.Count = 1 Then vsfeMail.Rows.Count = 2
    End Sub

    Private Sub btnDiffTrans_Click(sender As Object, e As EventArgs) Handles btnDiffTrans.Click
        If intDocID <= 0 Then Exit Sub

        txtdummy2.Text = ""

        Dim strSQL As String = "select Distinct t.docid , t.DocName from UtlOption u , UtlTransactions t where u.DocId=t.DocId order by DocName"

        Dim frmDisplay As New frmUtlSingleSel

        With frmDisplay
            .Text = "Select Document..."
            Call .FillViewVsf(Me, strSQL, txtdummy2, 0, 0)
            .ShowDialog()
        End With
        frmDisplay.Dispose()

        If Val(txtdummy2.Text) > 0 Then

            If intDocID <= 0 Then Exit Sub

            strSQL = " select  OptionName from utloption Where DocID=" & txtdummy2.Text & ""

            Dim frmDisplay2 As New frmUtlSingleSel

            txtdummy1.Text = ""

            With frmDisplay2
                .Text = "Select Print Option..."
                Call .FillViewVsf(Me, strSQL, txtdummy1, 0, -1)
                .ShowDialog()

            End With

            If Trim(txtdummy1.Text) <> "" Then
                Dim strPname As String = txtPrintName.Text
                Dim strRpt As String = cboRpt.Text
                Dim strFormula As String = txtFormula.Text

                Call clearflds()
                intDocID = txtdummy2.Text
                strOptioName = txtdummy1.Text
                Call LoadFormList()
                txtPrintName.Enabled = True

                txtPrintName.Text = strPname
                cboRpt.Text = strRpt
                txtFormula.Text = strFormula
                Mode = "A"
            End If

        End If
    End Sub

    Private Sub FrmUtlPrintOptionname_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        cboPrintClick.Text = "Preview"
    End Sub

    Private Sub btnTemplate_Click(sender As Object, e As EventArgs) Handles btnTemplate.Click
        If Mode = "A" Then
            Dim rs As oledb.oledbDataReader
            rs = clsSVT.getDataReaderFromSQL("Select * from UtlParameters")
            If Not rs.HasRows Then
                rs.Close()
                Exit Sub
            End If

            Do While rs.Read
                vsfDtl.SetData(vsfDtl.Rows.Count - 1, vDetFormulaField, GetVal(rs!FormulaFld, ""))
                vsfDtl.SetData(vsfDtl.Rows.Count - 1, vDetTable, GetVal(rs!TableName, ""))
                vsfDtl.SetData(vsfDtl.Rows.Count - 1, vDetFormulaValue, GetVal(rs!FormulaValue, ""))
                vsfDtl.Rows.Count = vsfDtl.Rows.Count + 1
            Loop
            'vsfDtl.AutoSizeCols()
            vsfDtl.Cols(0).Width = 200
            vsfDtl.Cols(1).Width = 200
            vsfDtl.Cols(2).Width = 200
            vsfDtl.ExtendLastCol = True
            rs.Close()
        End If
    End Sub
End Class