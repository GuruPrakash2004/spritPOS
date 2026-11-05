Imports System.ComponentModel
Imports System.Data.OleDb
Imports System.Text.RegularExpressions
Imports C1.Win.C1FlexGrid

Public Class frmTrnDocEntry
    Public argDocID As Integer
    Public argTag As String
    Dim strSHForm As String
    Dim blnColFreeze As Boolean

    Dim strFrmCaption As String
    Dim strDocHdrTblName As String
    Dim strDocNoFldName As String
    Dim strDocDateFldName As String
    Dim intPartyDocID As Integer
    Dim strPartyFilterCond As String

    Dim strMstTblName As String
    Dim strMstFldCode As String
    Dim strMstFldName As String
    Dim strMstFldId As String
    Dim AdoCustSupp As String
    Dim FrozenCols As String

    Dim iColNo As Integer

    Dim strPendReq As String, strPendQry As String

    Dim strRights As String
    Const vDetDocNo = 0
    Const vDetDocDate = 1
    Const vDetCustSupp = 2
    Const vDetUpdate = 3

    Dim argDetCols As Integer
    Dim vOthFldToUpdate As Integer
    Dim strDocStdDtlWidth As String

    Dim clsSVT As New svtLibrary


    Private Sub frmTrnDocEntry_KeyUp(sender As Object, e As KeyEventArgs) Handles Me.KeyUp
        If e.KeyCode = Keys.F9 And e.Control = True Then
            Call initFormSize(Me, Me.Text, True, 756, 483, 0, 0)
            MsgBox("Restored Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
        If e.KeyCode = Keys.F8 And e.Control = True Then
            Call SaveActFormSettings(Me, Me.Text)
            Call SaveActFormVsfDet(Me, Me.Text, "DET", vsfDet)
            MsgBox("Saved Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
    End Sub

    Private Sub btnExport_Click(sender As Object, e As EventArgs) Handles btnExport.Click
        If vsfDet.Rows.Count < 1 Then
            Exit Sub
        End If
        ExportToXL(vsfDet)
    End Sub

    Private Sub frmTrnDocEntry_Load(sender As Object, e As EventArgs) Handles Me.Load
        Me.KeyPreview = True
        Call initFormSize(Me, Me.Text, False, 854, 440, 0, 0)
        Call initFormVsfDet(Me, Me.Text, vsfDet, "DET")
        Call initScreen()
        Call getUtlTrnData(argDocID)


        Call InitDetAddnlGrid(Me, vsfDet, vsfOptions, argDetCols, strMstTblName, argDocID, False)
        If Not chkUserRights(strRights, chkRightsPos.ViewRights, Me.Text, False) Then Exit Sub
    End Sub

    Private Sub getUtlTrnData(ByVal argDocID As Integer)
        Dim rsDummy As OleDb.OleDbDataReader
        Dim cmdDummy As New OleDb.OleDbCommand
        Dim strSQL As String

        'With cmdDummy
        strSQL = "Select u.*, h.PendReq, h.PendQry from UtlTransactions u, UtlHdrPostDocEntry h where u.DocID = " & argDocID & " and u.DocID = h.DocID "
            rsDummy = clsSVT.getDataReaderFromSQL(strSQL)
        'End With

        With rsDummy
            If .HasRows Then
                .Read()
                strSHForm = GetVal(!DocShForm, "")
                strDocNoFldName = strSHForm & "No"
                strDocDateFldName = strSHForm & "Date"
                strDocHdrTblName = "TrnHdr" & strSHForm
                'Me.Caption = "Post Doc Entry for " & clsSvt.GetVal(!DocName, "")

                '# A
                Me.Tag = argTag
                Me.Text = argTag

                strPendReq = GetVal(!PendReq, "N")
                strPendQry = Replace(GetVal(!PendQry, ""), "`", "'")

                intPartyDocID = GetVal(!MstDocIDParty, 0)

                If intPartyDocID > 0 Then
                    Dim strPartyShForm As String
                    strPartyShForm = clsSVT.getDataStringValue("Select DocShForm From UtlTransactions where DocID = " & Val(intPartyDocID), "", False)
                    lblSupp.Text = strPartyShForm
                    strMstTblName = "Mst" & strPartyShForm
                    strMstFldId = strPartyShForm & "ID"
                    strMstFldCode = strPartyShForm & "Code"
                    strMstFldName = strPartyShForm & "Name"
                    strPartyFilterCond = GetVal(!PartyFilterCond, "")
                End If

            End If
        End With

        If strMstTblName <> "" Then
            Call bindCombo(cboSupp, strMstFldCode, strMstFldCode, "Select distinct " & strMstFldCode & " from " & strMstTblName & " where InActive <> 'Y' " & " order by 1")
        End If
        rsDummy = Nothing
        cmdDummy = Nothing
    End Sub

    Private Sub initScreen()
        dtpFromDate.Text = getSystemDate()
        dtpToDate.Text = getSystemDate()
        optPending.Checked = True
        If optAll.Checked = True Then
            optPending.Checked = False
            fraDate.Visible = True
        Else
            fraDate.Visible = False
        End If
        If optPending.Checked = True Then
            optAll.Checked = False
            fraDate.Visible = True
        Else
            fraDate.Visible = False
        End If
        With vsfDet
            .Rows.Count = 1
            .Cols.Count = 10

            '.Cols.Count = argDetCols
            .SetData(0, vDetDocNo, " strDocNoFldName")
            .SetData(0, vDetDocDate, "strDocDateFldName")
            ' .SetData(0, vDetCustSupp) = lblCode.text          
            .SetData(0, vDetUpdate, "Enter")
            .Cols(vDetUpdate).DataType = GetType(Boolean)
            '# A - Resize
            strDocStdDtlWidth = "1350, 1080, 4830, 460"
            SetColWidth(vsfDet, 1350, 1080, 4830, 460)
            If strMstFldCode = "" Then
                Call SetHideCol(vsfDet, True, vDetCustSupp)
            End If
            SetFlexColor(vsfDet, "", vDetDocNo, vDetDocDate, vDetCustSupp)
        End With
    End Sub

    Private Sub vsfDet_BeforeEdit(sender As Object, e As RowColEventArgs) Handles vsfDet.BeforeEdit
        If vsfDet.Cols.Count < 3 Then
            e.Cancel = True
            Exit Sub
        End If
        If vsfOptions.SetData(vsfDet.Col, vOthFldToUpdate, "N") Then
            e.Cancel = True
            Exit Sub
        End If
        If vsfOptBeforeEditCancel(Me, vsfDet, vsfOptions, strDocHdrTblName, "A", vsfDet.Row, vsfDet.Col, argDetCols) Then
            e.Cancel = True
            Exit Sub
        End If
    End Sub

    Private Sub vsfDet_AfterEdit(sender As Object, e As RowColEventArgs) Handles vsfDet.AfterEdit
        Dim i As Integer
        If vsfDet.Col = vDetUpdate Then
            If vsfDet.GetCellCheck(vsfDet.Row, vDetUpdate) = CheckEnum.Checked Then
                With vsfDet
                    For i = argDetCols To .Cols.Count - 1
                        If vsfOptions.GetData(i, vOthFldToUpdate) = "Y" Then
                            .SetData(.Row, i, "")
                        End If
                    Next i
                End With
            End If
        End If
        Call vsfOptAfterEditBfrCalTot(Me, vsfDet, vsfOptions, vsfDet.Row, vsfDet.Col)
        'Call CalTotal
        ' Call vsfOptAfterEditAftrCalTot(Me, vsfDet, vsfOptions, vsfDet.Row, vsfDet.Col)
        If vsfDet.Cols.Count > argDetCols Then
            If vsfOptions.SetData(vsfDet.Col, vOthFldToUpdate, "Y") Then
                If Trim(vsfDet.GetData(vsfDet.Row, vsfDet.Col)) <> "" Then
                    vsfDet.SetCellCheck(vsfDet.Row, vDetUpdate, CheckEnum.Checked)
                End If
            End If
        End If

    End Sub

    Private Sub vsfDet_EnterCell(sender As Object, e As EventArgs) Handles vsfDet.EnterCell
        With vsfDet
            ' .EditCell
        End With
    End Sub

    Private Sub vsfDet_KeyUp(sender As Object, e As KeyEventArgs) Handles vsfDet.KeyUp
        'Call vsfOptKeyUp(Me, argDetCols)
        If e.KeyCode = Keys.F7 Then
            If blnColFreeze = False Then
                'vsfDet.FrozenCols = vsfDet.Col
                blnColFreeze = True
                MsgBox("Column Freezed Activated up to " & vsfDet.GetData(0, vsfDet.Col), vbInformation, Me.Text)
            Else
                blnColFreeze = False
                'vsfDet.FrozenCols = 1
                MsgBox("Column Freezed De-Activated", vbInformation, Me.Text)
            End If
        End If
    End Sub

    Private Sub vsfDet_ValidateEdit(sender As Object, e As ValidateEditEventArgs) Handles vsfDet.ValidateEdit
        'If vsfOptValidateEdit(Me, vsfDet.Row, vsfDet.Col, "T", strDocHdrTblName, "", "") = False Then
        e.Cancel = True
        'End If
    End Sub

    Private Sub btnLoad_Click(sender As Object, e As EventArgs) Handles btnLoad.Click
        Dim strsql As String

        strsql = "Select h.*"
        If strMstFldCode <> "" Then strsql = strsql & ", m." & strMstFldCode & " as partyCode"
        strsql = strsql & " from " & strDocHdrTblName & " h"
        If strMstFldCode <> "" Then strsql = strsql & ", " & strMstTblName & " m"
        strsql = strsql & " where 1 = 1 "
        If strMstFldCode <> "" Then
            strsql = strsql & " and h." & strMstFldId & " = m." & strMstFldId
            If Trim(cboSupp.Text) <> "" Then
                strsql = strsql & " and " & strMstFldCode & " = '" & Trim(cboSupp.Text) & "'"
            End If
        End If
        If optPending.Checked = True Then
            If Trim(strPendQry) <> "" Then strsql = strsql & " And " & strPendQry
        End If
        If optAll.Checked = True Then
            strsql = strsql & " and " & strDocDateFldName & " >= " & getDateFormatInCond(dtpFromDate.Value)
            strsql = strsql & " and " & strDocDateFldName & " <= " & getDateFormatInCond(dtpToDate.Value)
        End If
        Call fillDetails(strsql)
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Dim i As Integer
        Dim strsql As String
        Dim RecordsAffected As Integer
        Dim svtTrn As OleDb.OleDbTransaction = Nothing

        If Not chkUserRights(strRights, chkRightsPos.EditRights, Me.Text) Then
            Exit Sub
        End If

        If Not validDetFLds() Then Exit Sub

        StMsg("Saving Data....")
        Try
            svtTrn = svtConn.BeginTransaction(IsolationLevel.ReadCommitted)
            With vsfDet
                For i = 1 To .Rows.Count - 1
                    'If ValidateRow(i) = True Then
                    strsql = "Update " & strDocHdrTblName & " set "
                    strsql = strsql & GetDetUpdateString(i)
                    strsql = strsql & " where " & strDocNoFldName & " = '" & .GetData(i, vDetDocNo) & "'"
                    If Not clsSVT.execSQLStatement(strsql, svtTrn) Then
                        svtTrn.Rollback()
                        Exit Sub
                    End If
                    'End If
                Next
                svtTrn.Commit()
                If RecordsAffected > 0 Then
                    MsgBox(Me.Text & " Saved Successfully", vbInformation, Me.Text)
                End If
            End With
            Call btnLoad_Click(btnLoad, e)
            StMsg("Ready..")
        Catch ex As Exception
            MsgBox("Error during Save" & vbCrLf & ex.Message, MsgBoxStyle.Critical, Me.Text)
            svtTrn.Rollback()
        End Try
    End Sub

    Private Sub vsfDet_KeyPressEdit(sender As Object, e As KeyPressEditEventArgs) Handles vsfDet.KeyPressEdit
        Dim KeyAscii As Integer
        If KeyAscii = Asc("'") Then KeyAscii = Asc("`")
        If KeyAscii = Keys.Escape Then Exit Sub
        If vsfDet.Col < argDetCols Then Exit Sub
        'KeyAscii = vsfOptKeyPressEdit(Me, vsfDet.Col, vsfDet.Row, KeyAscii)
    End Sub

    Private Function GetDetUpdateString(argRow As Integer) As String
        Dim iColNo As Integer, strFirstDispCol As String
        Dim strFldValue As String, strValues As String
        Dim strIndStrings As String
        strFldValue = ""
        strValues = ""
        Dim strFldBlnComboData As String, strFldCmbTblName As String
        Dim strFldCmbStorageFldName As String, strFldCmbDisplayCols As String, strFldCmbAddnlCond As String

        With vsfDet
            For iColNo = argDetCols To .Cols.Count - 1
                If vsfOptions.SetData(iColNo, vOthFldToUpdate, "Y") Then
                    strIndStrings = ""
                    strFldValue = Trim(.GetData(argRow, iColNo))
                    If Trim(vsfOptions.GetData(iColNo, vOthFldCmbTblName)) <> "" Then
                        If InStr(1, vsfOptions.GetData(iColNo, vOthFldCmbDisplayCols), ",") > 0 Then
                            strFirstDispCol = Strings.Left(vsfOptions.GetData(iColNo, vOthFldCmbDisplayCols), InStr(1, vsfOptions.GetData(iColNo, vOthFldCmbDisplayCols), ",") - 1)
                        Else
                            strFirstDispCol = vsfOptions.GetData(iColNo, vOthFldCmbDisplayCols)
                        End If
                        strFldValue = clsSVT.getDataStringValue("Select " & vsfOptions.GetData(iColNo, vOthFldCmbStorageFldName) & " From " & vsfOptions.GetData(iColNo, vOthFldCmbTblName) & " where " & strFirstDispCol &
                            " = '" & strFldValue & "'", "", False)
                    End If
                    If Trim(vsfOptions.SetData(iColNo, vOthFldBlnLoadFrom, "Y")) Then
                        'if ledname is shown and ledname is to be stored [using load from option]
                        ' till here
                        If vsfOptions.SetData(iColNo, vOthFldLoadFromType, "I") Then
                            If UCase(Strings.Mid(Trim(vsfOptions.GetData(iColNo, vOthFldLoadFldName)), 1, 1)) = "I" Then
                                'find if it is addnlfld, and if it is loaded from combo
                                'example i.LedID
                                'strFldBlnComboData = clsSVT.GetDataValue("FldBlnComboData", "N",
                                '"UtlDocDsgrFlds", False, "where DocID = 2 and FldCode = '" &
                                'UCase(Mid(Trim(vsfOptions.SetData(iColNo, vOthFldLoadFldName)), 3)) & "'")

                                strFldBlnComboData = clsSVT.getDataStringValue("select FldBlnComboData from UtlDocDsgrFlds where DocID = 2 and FldCode = '" & UCase(Mid(Trim(vsfOptions.GetData(iColNo, vOthFldLoadFldName)), 3)) & "'", "N", False)

                                If strFldBlnComboData = "Y" Then
                                    strFldCmbTblName = clsSVT.getDataStringValue("select FldCmbTblName from UtlDocDsgrFlds where DocID = 2 and FldCode = '" & UCase(Mid(Trim(vsfOptions.GetData(iColNo, vOthFldLoadFldName)), 3)) & "'", "N", False)
                                    strFldCmbStorageFldName = clsSVT.getDataStringValue("select FldCmbStorageFldName from UtlDocDsgrFlds where DocID = 2 and FldCode = '" & UCase(Mid(Trim(vsfOptions.GetData(iColNo, vOthFldLoadFldName)), 3)) & "'", "N", False)

                                    strFldCmbDisplayCols = clsSVT.getDataStringValue("select FldCmbDisplayCols from UtlDocDsgrFlds where DocID = 2 and FldCode = '" & UCase(Mid(Trim(vsfOptions.GetData(iColNo, vOthFldLoadFldName)), 3)) & "'", "N", False)

                                    strFldCmbAddnlCond = clsSVT.getDataStringValue("select FldCmbAddnlCond from UtlDocDsgrFlds where DocID = 2 and FldCode = '" & UCase(Mid(Trim(vsfOptions.GetData(iColNo, vOthFldLoadFldName)), 3)) & "'", "N", False)

                                    If strFldCmbTblName <> "" Then
                                        If InStr(1, strFldCmbDisplayCols, ",") > 0 Then
                                            strFirstDispCol = Strings.Left(strFldCmbDisplayCols, InStr(1, strFldCmbDisplayCols, ",") - 1)
                                        Else
                                            strFirstDispCol = strFldCmbDisplayCols
                                        End If
                                        strFldValue = clsSVT.getDataStringValue("Select " & strFldCmbStorageFldName & " from " & strFldCmbTblName & " where " & strFirstDispCol & " = '" & strFldValue & "'", "N", False)
                                    End If
                                End If
                            End If
                        End If
                    End If
                    strIndStrings = Trim(vsfOptions.GetData(iColNo, vOthFldCode)) & " = "
                    Select Case vsfOptions.GetData(iColNo, vOthFldDataType)
                        Case "Text", "List String"
                            If strFldValue = "" Then strFldValue = " "
                            strIndStrings = strIndStrings & "'" & strFldValue & "'"
                        Case "Date"
                            If strFldValue <> "" Then
                                strIndStrings = strIndStrings & "'" & getDateFormatInInsert(strFldValue) & "'"
                            Else
                                strIndStrings = strIndStrings & "Null"
                            End If
                        Case Else
                            strIndStrings = strIndStrings & Val(strFldValue) & ""
                    End Select
                    strValues = strValues & strIndStrings & ","
                End If
            Next iColNo
        End With
        strValues = Mid(strValues, 1, Len(strValues) - 1)
        GetDetUpdateString = strValues
    End Function

    Private Function validDetFLds() As Boolean
        Dim i As Integer, blnFlag As Boolean, x As Integer
        Dim strFirstDispCol As String

        validDetFLds = True
        With VsfOptions
            For x = 1 To vsfDet.Rows.Count - 1
                If vsfDet.GetCellCheck(x, vDetUpdate) = CheckEnum.Checked Then
                    For i = argDetCols To .Rows.Count - 1
                        If .GetData(i, vOthFldDataType) = "Date" Then
                            If Trim(vsfDet.GetData(x, vOthFldDataValue)) = "/  /" Then
                                .SetData(x, i, "")
                            End If
                        End If

                        '# Code for mandatory field
                        If .GetData(i, vOthFldBlnMandatory) = "Y" Then
                            If fnExpEvaluate(.GetData(i, vOthFldMndtryCompareVal),
                                                        .GetData(i, vOthFldMndtryWithVal),
                                                        .GetData(i, vOthFldMndtryOperator),
                                                        "T", Me.Text, x) Then
                                If Trim(vsfDet.GetData(x, i)) = "" _
                                 And Val(vsfDet.GetData(x, i)) = 0 Then
                                    MsgBox("Field " & .GetData(i, vOthFldCaption) & " cannot be blank ", vbInformation, Me.Text)
                                    vsfDet.Row = x
                                    vsfDet.Col = i
                                    vsfDet.TopRow = x
                                    vsfDet.Focus()
                                    GoTo EndFunc
                                End If
                            End If
                        End If
                        '# Code for mandatory field

                        If .GetData(i, vOthFldBlnComboData) = "Y" Then
                            If Trim(.GetData(x, i)) <> "" Then
                                If InStr(1, .GetData(i, vOthFldCmbDisplayCols), ",") > 0 Then
                                    strFirstDispCol = Strings.Left(.GetData(i, vOthFldCmbDisplayCols) _
                                                        , InStr(1, .GetData(i, vOthFldCmbDisplayCols), ",") - 1)
                                Else
                                    strFirstDispCol = .GetData(i, vOthFldCmbDisplayCols)
                                End If
                                Call clsSVT.getDataStringValue("Select " & .GetData(i, vOthFldCmbStorageFldName) & " From " & .GetData(i, vOthFldCmbTblName) & " where " & strFirstDispCol &
                                    " = '" & .GetData(x, i) & "'", "", blnFlag)
                                If Not blnFlag Then
                                    MsgBox(.GetData(i, vOthFldCaption) & "[" & .GetData(x, i) & "] ...  Not found in " & .GetData(i, vOthFldCmbTblName) & " !!", vbInformation, Me.Text)
                                    vsfDet.Row = x
                                    vsfDet.TopRow = x
                                    vsfDet.Col = i
                                    vsfDet.Focus()
                                    GoTo EndFunc
                                End If
                            End If
                        End If
                    Next i
                End If
            Next x
        End With
        Exit Function
EndFunc:
        validDetFLds = False
    End Function


    Private Function fillDetails(Sql As String) As Boolean
        Dim intAddCol As Integer, x As Integer
        Dim clsSVT As New svtLibrary, strFirstDispCol As String
        Dim strFldValue As String
        Dim strFldBlnComboData As String, strFldCmbTblName As String
        Dim strFldCmbStorageFldName As String, strFldCmbDisplayCols As String, strFldCmbAddnlCond As String

        Dim i As Integer
        Dim CmdSO As New OleDb.OleDbCommand
        Dim RsSO As OleDbDataReader
        'On Error GoTo cleanUp

        vsfDet.Clear()
        vsfDet.Rows.Count = 2
        fillDetails = True
        'With CmdSO
        '.ActiveConnection = svtConn
        '    .CommandType = adCmdText
        '    .CommandText = Sql
        '    RsSO = .Execute
        RsSO = clsSVT.getDataReaderFromSQL(Sql)
        'End With

        With RsSO
            If Not .HasRows Then
                fillDetails = False
                CmdSO = Nothing
                RsSO = Nothing
                Exit Function
            End If
            vsfDet.Rows.Count = 1
            Do While .Read
                vsfDet.Rows.Count = vsfDet.Rows.Count + 1
                i = vsfDet.Rows.Count - 1
                vsfDet.SetData(i, vDetDocNo, Trim(GetVal(.GetValue(strDocNoFldName))))
                vsfDet.SetData(i, vDetDocDate, Trim(GetVal(.GetValue(strDocDateFldName))))
                vsfDet.SetData(i, vDetCustSupp, GetVal(!PartyCode))
                For intAddCol = argDetCols To vsfOptions.Rows.Count - 1
                    For x = 0 To .FieldCount() - 1
                        If UCase$(.GetValue(x).Name) = vsfOptions.GetData(intAddCol, vOthFldCode) Then
                            If Trim(vsfOptions.GetData(intAddCol, vOthFldBlnComboData)) = "N" Then
                                vsfDet.SetData(i, intAddCol, GetVal(.GetValue(x).value, ""))
                            Else
                                If InStr(1, vsfOptions.GetData(intAddCol, vOthFldCmbDisplayCols), ",") > 0 Then
                                    strFirstDispCol = Strings.Left(vsfOptions.GetData(intAddCol, vOthFldCmbDisplayCols) _
                                                        , InStr(1, vsfOptions.GetData(intAddCol, vOthFldCmbDisplayCols), ",") - 1)
                                Else
                                    strFirstDispCol = vsfOptions.GetData(intAddCol, vOthFldCmbDisplayCols)
                                End If
                                vsfDet.SetData(i, intAddCol, clsSVT.getDataStringValue("Select " & strFirstDispCol & " From " & vsfOptions.GetData(intAddCol, vOthFldCmbTblName) & " where " & vsfOptions.GetData(intAddCol, vOthFldCmbStorageFldName) &
                                    " = '" & GetVal(.GetValue(x).value, 0) & "'", "", False))
                            End If
                            If Trim(vsfOptions.SetData(intAddCol, vOthFldBlnLoadFrom, "Y")) Then
                                strFldValue = GetVal(.GetValue(x).value, "")
                                If vsfOptions.SetData(intAddCol, vOthFldLoadFromType, "I") Then
                                    If UCase(Mid(Trim(vsfOptions.GetData(intAddCol, vOthFldLoadFldName)), 1, 1)) = "I" Then
                                        'find if it is addnlfld, and if it is loaded from combo
                                        'example i.LedID
                                        strFldBlnComboData = clsSVT.getDataStringValue("Select FldBlnComboData from UtlDocDsgrFlds where DocID = 2 and FldCode = '" & UCase(Mid(Trim(vsfOptions.GetData(intAddCol, vOthFldLoadFldName)), 3)) & "'", "N", False)
                                        If strFldBlnComboData = "Y" Then
                                            strFldCmbTblName = clsSVT.getDataStringValue("Select FldCmbTblName from UtlDocDsgrFlds where DocID = 2 and FldCode = '" & UCase(Mid(Trim(vsfOptions.GetData(intAddCol, vOthFldLoadFldName)), 3)) & "'", "N", False)
                                            strFldCmbStorageFldName = clsSVT.getDataStringValue("Select FldCmbStorageFldName from UtlDocDsgrFlds where DocID = 2 and FldCode = '" & UCase(Mid(Trim(vsfOptions.GetData(intAddCol, vOthFldLoadFldName)), 3)) & "'", "N", False)

                                            strFldCmbDisplayCols = clsSVT.getDataStringValue("Select FldCmbDisplayCols from UtlDocDsgrFlds where DocID = 2 and FldCode = '" & UCase(Mid(Trim(vsfOptions.GetData(intAddCol, vOthFldLoadFldName)), 3)) & "'", "N", False)

                                            strFldCmbAddnlCond = clsSVT.getDataStringValue("Select FldCmbAddnlCond from UtlDocDsgrFlds where DocID = 2 and FldCode = '" & UCase(Mid(Trim(vsfOptions.GetData(intAddCol, vOthFldLoadFldName)), 3)) & "'", "N", False)

                                            If strFldCmbTblName <> "" Then
                                                If InStr(1, strFldCmbDisplayCols, ",") > 0 Then
                                                    strFirstDispCol = Strings.Left(strFldCmbDisplayCols, InStr(1,
                                                    strFldCmbDisplayCols, ",") - 1)
                                                Else
                                                    strFirstDispCol = strFldCmbDisplayCols
                                                End If
                                                strFldValue = clsSVT.getDataStringValue(" Select " & strFirstDispCol & " From " & strFldCmbTblName & " where " & strFldCmbStorageFldName & " = " & Val(strFldValue), "", False)
                                                vsfDet.SetData(i, intAddCol, strFldValue)
                                            End If
                                        End If
                                    End If
                                End If

                            End If

                            'If vsfOptions.SetData(intAddCol, vOthFldBlnFontBold, "Y") Then
                            '    vsfDet.Cell(flexcpFontBold, i, intAddCol) = True
                            'End If
                            'If vsfOptions.SetData(intAddCol, vOthFldBlnRowClr, "Y") Then
                            '    If vsfOptions.GetData(intAddCol, vOthFldRowClrForeColour) <> "" Then
                            '        vsfDet.Cell(flexcpBackColor, i, intAddCol) =
                            '           vsfOptions.GetData(intAddCol, vOthFldRowClrForeColour)
                            '    End If
                            'End If

                            'If vsfOptions.GetData(intAddCol, vOthFldTxtAlignment) <> "" Then
                            '    Select Case vsfOptions.GetData(intAddCol, vOthFldTxtAlignment)
                            '        Case "Left"
                            '            'vsfDet.Cell(flexcpAlignment, i, intAddCol) = flexAlignLeftCenter
                            '        Case "Right"
                            '            vsfDet.Cell(flexcpAlignment, i, intAddCol) = flexAlignRightCenter
                            '        Case "Centre"
                            '            vsfDet.Cell(flexcpAlignment, i, intAddCol) = flexAlignCenterCenter
                            '    End Select
                            'End If
                            ' # R End
                            Exit For
                        End If
                    Next x
                Next
            Loop
        End With
        Exit Function

        MsgBox(Err.Description, vbCritical, Me.Text)
        CmdSO = Nothing
        RsSO = Nothing
    End Function

    Private Sub frmTrnDocEntry_Resize(sender As Object, e As EventArgs) Handles Me.Resize

    End Sub
End Class