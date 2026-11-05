Imports C1.Win.C1FlexGrid

Public Class frmUtlDocNumber

    Dim clsSvt As New svtLibrary

    Const vPfxPrefix = 0
    Const vPfxFldCode = 1
    Const vPfxCondOperator = 2
    Const vPfxFldValue = 3

    Const vDetDocName = 0
    Const vDetDocId = 1
    Const vDetContDocNo = 2
    Const vDetPfxWiseResetNo = 3
    Const vDetDocNoChange = 4
    Const vDetExample = 19

    Const vDetColStart = 5

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub btnPClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPClose.Click
        fraPrefix.Visible = False
    End Sub

    Private Sub btnPrefixSettings_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrefixSettings.Click
        If Val(vsfDet.GetData(vsfDet.Row, vDetDocId)) <= 0 Then
            MsgBox("Please select a document!", vbInformation, Me.Text)
            Exit Sub
        End If

        Call fillPfxGrid(Val(vsfDet.GetData(vsfDet.Row, vDetDocId)))
        fraPrefix.Visible = True
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim i As Integer, j As Integer
        Dim strSql As String
        Dim strSql1 As String
        If CheckForDocNo() <> True Then
            MsgBox("Invalid combinations!", vbInformation, Me.Text)
            Exit Sub
        End If

        clsSvt.execSQLStatement("Delete from UtlDocNoFmt")

        With vsfDet
            For i = 1 To .Rows.Count - 1
                strSql = "Insert into UtlDocNoFmt (DocID, PfxWiseResetNo, DocNoChange, F1, F2, F3, F4, F5, F6, "
                strSql = strSql & "F7, F8, F9, F10, F11, F12, F13, F14) values ("
                strSql = strSql & Val(.GetData(i, vDetDocId)) & ", "
                If .GetCellCheck(i, vDetPfxWiseResetNo) = C1.Win.C1FlexGrid.CheckEnum.Checked Then
                    strSql = strSql & "'Y',"
                Else
                    strSql = strSql & "'N',"
                End If
                If .GetCellCheck(i, vDetDocNoChange) = C1.Win.C1FlexGrid.CheckEnum.Checked Then
                    strSql = strSql & "'Y',"
                Else
                    strSql = strSql & "'N',"
                End If
                For j = vDetColStart To .Cols.Count - 2
                    strSql = strSql & "'" & .GetData(i, j) & "',"
                Next j
                strSql = Mid(strSql, 1, Len(strSql) - 1) & ")"

                clsSvt.execSQLStatement(strSql)
            Next i
        End With

        With vsfDet
            For i = 1 To .Rows.Count - 1
                ' If GetVal(.GetData(i, vDetContDocNo), "") <> "" Then
                strSql1 = "Update UtlTransactions set "
                strSql1 = strSql1 & " ContDocNoGroup = '" & (vsfDet.GetData(i, vDetContDocNo)) & "'"
                strSql1 = strSql1 & " where DocID = " & vDetDocId
                clsSvt.execSQLStatement(strSql1)
                ' End If
            Next
        End With

        MsgBox("Saved successfully!", vbInformation, Me.Text)
    End Sub

    Private Sub frmUtlDocNumber_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim i As Integer

        With vsfEx
            .Cols.Count = 9

            For i = 0 To .Cols.Count - 1
                .SetData(0, i, i + 1)
                .Cols(i).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter
                .Cols(i).TextAlignFixed = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter
            Next i
            For i = 1 To 2
                .SetData(i, 0, "Prefix")
                .SetData(i, 1, "Year From")
                .SetData(i, 2, "Year To")
                .SetData(i, 3, "Sh YearFrom")
                .SetData(i, 4, "Sh YearTo")
                .SetData(i, 5, "DocNo")
                .SetData(i, 6, "ShDocNo")
                .SetData(i, 7, "Adnl Doc No 1")
                .SetData(i, 8, "Adnl Doc No 2")
            Next i
            .AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.Free
            For i = 0 To 8
                .Cols(i).AllowMerging = True
            Next i

            '.Cell(flexcpBackColor, 0, 0, 0, .Cols - 1) = &HE2F2FE
            '.Cell(flexcpBackColor, 1, 0, 1, .Cols - 1) = &HD6E4E4
            '.Cell(flexcpFontBold, 1, 0, 1, .Cols - 1) = True
            '.Cell(flexcpBackColor, 3, 0, 3, .Cols - 1) = &HD2CDB9
            .SetData(3, 0, "LOC")
            .SetData(3, 1, Year(gSettings.gCompStDt))
            .SetData(3, 2, Year(gSettings.gCompEndDt))
            .SetData(3, 3, Microsoft.VisualBasic.Strings.Right(.GetData(3, 1).ToString, 2))
            .SetData(3, 4, Microsoft.VisualBasic.Strings.Right(.GetData(3, 2).ToString, 2))
            .SetData(3, 5, "00001")
            .SetData(3, 6, "1")
            .SetData(3, 7, "0001")
            .SetData(3, 8, "001")
        End With

        'Call SetColWidth(vsfEx, 900, 960, 960, 1395, 1005, 960, 960)

        With vsfDet
            .Rows.Count = 1
            .Cols.Count = 20

            .SetData(0, vDetDocName, "Document")
            .SetData(0, vDetContDocNo, "Cont.Document No")
            .SetData(0, vDetExample, "Example")
            .SetData(0, vDetPfxWiseResetNo, "Pfx-Wise Reset No")
            .SetData(0, vDetDocNoChange, "Allow DocNo Change")

            .Cols(vDetPfxWiseResetNo).DataType = GetType(Boolean)
            .Cols(vDetDocNoChange).DataType = GetType(Boolean)

            For i = vDetColStart To .Cols.Count - 2
                .SetData(0, i, "Sel")
            Next i

            Call fillDocGrid()
            '.Cols(vDetDocName).Width = 2490
            '.Cols(vDetPfxWiseResetNo).Width = 1500
            '.Cols(vDetDocNoChange).Width = 1500

            SetHideCol(vsfDet, True, vDetDocId)
            Call SetFlexColor(vsfDet, "", vDetDocName, vDetExample)

            For i = vDetColStart To .Cols.Count - 2
                '.Cols(i).Width = 300
                .Cols(i).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter
            Next i
        End With
        Call fillData()
        fraPrefix.Visible = False

        With vsfPfx
            .Cols.Count = 4
            .SetData(0, vPfxPrefix, "Prefix")
            .SetData(0, vPfxFldCode, "Field Code")
            .SetData(0, vPfxCondOperator, "Operator")
            .SetData(0, vPfxFldValue, "Value")

            'SetColWidth(vsfPfx, 930, 2340, 720, 540)
            .Rows.Count = 2
        End With

        initGridStyles(Me)
        vsfDet.AutoSizeCols()
        vsfPfx.AutoSizeCols()
        vsfEx.AutoSizeCols()

        KeyPreview = True
        lblInformation.Visible = False
    End Sub

    Private Sub vsfDet_AfterEdit(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfDet.AfterEdit
        Dim i As Integer
        Dim j As Integer
        Dim strDocNo As String
        strDocNo = ""
        With vsfDet
            For i = vDetColStart To .Cols.Count - 2
                If Trim(.GetData(.Row, i)) <> "" Then
                    Select Case Val(.GetData(.Row, i))
                        Case 1, 2, 3, 4, 5, 6, 7, 8, 9
                            j = Val(.GetData(.Row, i) - 1)
                            strDocNo = strDocNo & vsfEx.GetData(3, j)
                        Case Else
                            strDocNo = strDocNo & .GetData(.Row, i)
                    End Select
                End If
                .SetData(.Row, vDetExample, strDocNo)
            Next i
        End With
    End Sub

    Private Sub vsfDetAfterEdit(ByVal Row As Integer, ByVal Col As Integer)
        Dim i As Integer
        Dim j As Integer
        Dim strDocNo As String
        strDocNo = ""
        With vsfDet
            For i = vDetColStart To .Cols.Count - 2
                If Trim(.GetData(Row, i)) <> "" Then
                    Select Case Val(.GetData(Row, i))
                        Case 1, 2, 3, 4, 5, 6, 7, 8, 9
                            j = Val(.GetData(Row, i) - 1)
                            strDocNo = strDocNo & vsfEx.GetData(3, j)
                        Case Else
                            strDocNo = strDocNo & .GetData(Row, i)
                    End Select
                End If
                .SetData(Row, vDetExample, strDocNo)
            Next i
        End With
    End Sub

    Private Sub vsfDet_BeforeEdit(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfDet.BeforeEdit
        Select Case e.Col
            Case vDetDocId, vDetDocName, vDetExample
                e.Cancel = True
        End Select
    End Sub

    Private Sub vsfDet_EnterCell(ByVal sender As Object, ByVal e As System.EventArgs) Handles vsfDet.EnterCell
        If vsfDet.Col <> vDetPfxWiseResetNo And vsfDet.Col <> vDetDocNoChange Then vsfDet.AllowEditing() = True
    End Sub

    Private Sub fillData()
        Dim i As Integer
        Dim rsDummy As oledb.oledbDataReader

        rsDummy = clsSvt.getDataReaderFromSQL("Select * from UtlDocNoFmt")

        With rsDummy
            Do While .Read()
                For i = 1 To vsfDet.Rows.Count - 1
                    If !docId = Val(vsfDet.GetData(i, vDetDocId)) Then
                        If GetVal(!PfxWiseResetNo, "Y") = "Y" Then
                            vsfDet.SetCellCheck(i, vDetPfxWiseResetNo, C1.Win.C1FlexGrid.CheckEnum.Checked)
                        Else
                            vsfDet.SetCellCheck(i, vDetPfxWiseResetNo, C1.Win.C1FlexGrid.CheckEnum.Unchecked)
                        End If

                        If GetVal(!DocNoChange, "Y") = "Y" Then
                            vsfDet.SetCellCheck(i, vDetDocNoChange, C1.Win.C1FlexGrid.CheckEnum.Checked)
                        Else
                            vsfDet.SetCellCheck(i, vDetDocNoChange, C1.Win.C1FlexGrid.CheckEnum.Unchecked)
                        End If

                        vsfDet.SetData(i, 5, GetVal(!F1, ""))
                        vsfDet.SetData(i, 6, GetVal(!F2, ""))
                        vsfDet.SetData(i, 7, GetVal(!F3, ""))
                        vsfDet.SetData(i, 8, GetVal(!F4, ""))
                        vsfDet.SetData(i, 9, GetVal(!F5, ""))
                        vsfDet.SetData(i, 10, GetVal(!F6, ""))
                        vsfDet.SetData(i, 11, GetVal(!F7, ""))
                        vsfDet.SetData(i, 12, GetVal(!F8, ""))
                        vsfDet.SetData(i, 13, GetVal(!F9, ""))
                        vsfDet.SetData(i, 14, GetVal(!F10, ""))
                        vsfDet.SetData(i, 15, GetVal(!F11, ""))
                        vsfDet.SetData(i, 16, GetVal(!F12, ""))
                        vsfDet.SetData(i, 17, GetVal(!F13, ""))
                        vsfDet.SetData(i, 18, GetVal(!F14, ""))
                        vsfDet.Row = i



                        Call vsfDetAfterEdit(i, vDetDocName)
                        'Call vsfDet_AfterEdit(Me, vDetDocName)
                        '                    vsfEx.TextMatrix(2, 7) = !Misc1 arun
                        '                    vsfEx.TextMatrix(2, 8) = !Misc2
                        '                    vsfEx.TextMatrix(2, 9) = !Misc3
                        Exit For
                    End If
                Next i
            Loop
        End With

    End Sub

    Private Sub btnResetNoFmt_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnResetNoFmt.Click
        Dim introw As Integer
        With vsfDet
            introw = .Row
            If Trim(.GetData(introw, vDetDocId)) = "" Then Exit Sub
            If MsgBox("Do you want to reset the printing number format of " & .GetData(introw, vDetDocName) & "?", vbYesNo, Me.Text) = vbNo Then
                Exit Sub
            End If
            Call updtDocNo(.GetData(introw, vDetDocId), introw)
        End With
    End Sub

    Private Sub updtDocNo(ByVal argDocID As Integer, ByVal argRow As Integer)
        Dim strDocFld As String, strPrntDocFld As String, strTblName As String
        Dim strSQL As String, i As Integer
        strDocFld = "" : strPrntDocFld = "" : strTblName = ""
        ' On Error GoTo ErrLabel
        Dim strSHForm As String

        Try
            strSHForm = clsSvt.getDataStringValue("Select DocShForm from UtlTransactions where DocID = " & argDocID, "", False)

            strDocFld = strSHForm & "NO"
            strPrntDocFld = "Print" & strSHForm & "NO"
            strTblName = "TrnHdr" & strSHForm
            If strTblName = "" Then Exit Sub
            strSQL = "Update " & strTblName
            strSQL = strSQL & " set " & strPrntDocFld & " = "
            With vsfDet
                For i = vDetColStart To .Cols.Count - 2
                    If Trim(.GetData(argRow, i)) <> "" Then
                        Select Case Val(.GetData(argRow, i))
                            Case 1
                                strSQL = strSQL & " prefix + "
                            Case 2
                                strSQL = strSQL & "'" & Microsoft.VisualBasic.Strings.Right(gSettings.gCompStDt, 4) & "' + "
                            Case 3
                                strSQL = strSQL & "'" & Microsoft.VisualBasic.Strings.Right(gSettings.gCompEndDt, 4) & "' + "
                            Case 4
                                strSQL = strSQL & "'" & Microsoft.VisualBasic.Strings.Right(gSettings.gCompStDt, 2) & "' + "
                            Case 5
                                strSQL = strSQL & "'" & Microsoft.VisualBasic.Strings.Right(gSettings.gCompEndDt, 2) & "' + "
                            Case 6
                                strSQL = strSQL & " Right(" & strDocFld & ", 5) + "
                            Case 7
                                strSQL = strSQL & " convert(int,(Right(" & strDocFld & ", 5))) + "
                            Case 8
                                strSQL = strSQL & "'" & vsfEx.GetData(2, 7) & "' + "
                            Case 9
                                strSQL = strSQL & "'" & vsfEx.GetData(2, 8) & "' + "
                            Case 10
                                strSQL = strSQL & "'" & vsfEx.GetData(2, 9) & "' + "
                            Case Else
                                strSQL = strSQL & "'" & vsfDet.GetData(argRow, i) & "' + "
                        End Select
                    End If
                Next i
                strSQL = Mid(strSQL, 1, Len(strSQL) - 2)

                clsSvt.execSQLStatement("update " & strTblName & " set Prefix = '' where Prefix is null")
                clsSvt.execSQLStatement(strSQL)
                MsgBox("Updated all the records Successfully!!", vbInformation, Me.Text)
            End With
            Exit Sub
        Catch ex As Exception
        End Try
        'ErrLabel:
        'MsgBox(Err.Description, vbInformation, Me.Text)
    End Sub

    Private Sub fillDocGrid()
        Dim rsDummy As oledb.oledbDataReader

        Dim strSql As String


        strSql = "Select * from UtlTransactions where DocType <> 'Master' order by DocName"


        rsDummy = clsSvt.getDataReaderFromSQL(strSql)

        With rsDummy
            Do While .Read()
                vsfDet.Rows.Count = vsfDet.Rows.Count + 1
                vsfDet.SetData(vsfDet.Rows.Count - 1, vDetDocName, GetVal(!DocName))
                vsfDet.SetData(vsfDet.Rows.Count - 1, vDetDocId, GetVal(!DocId))
                vsfDet.SetData(vsfDet.Rows.Count - 1, vDetContDocNo, GetVal(!ContDocNoGroup, ""))
            Loop
            .Close()
        End With
    End Sub



    Private Sub fillPfxGrid(ByVal argDocID As Integer)
        Dim rsDummy As oledb.oledbDataReader
        Dim strSql As String

        strSql = "Select * from UtlDocNoPrefix where docID = " & argDocID & " order by prefix"

        rsDummy = clsSvt.getDataReaderFromSQL(strSql)

        vsfPfx.Rows.Count = 1

        With rsDummy
            Do While .Read()
                vsfPfx.Rows.Count = vsfPfx.Rows.Count + 1
                vsfPfx.SetData(vsfPfx.Rows.Count - 1, vPfxPrefix, GetVal(!Prefix, ""))
                vsfPfx.SetData(vsfPfx.Rows.Count - 1, vPfxCondOperator, GetVal(!CndOperator, ""))
                vsfPfx.SetData(vsfPfx.Rows.Count - 1, vPfxFldCode, GetVal(!CndFldCode, ""))
                vsfPfx.SetData(vsfPfx.Rows.Count - 1, vPfxFldValue, GetVal(rsDummy("CndFldValue")))
            Loop
            .Close()
        End With

        If vsfPfx.Rows.Count = 1 Then vsfPfx.Rows.Count = 2

    End Sub

    Private Sub vsfEx_BeforeEdit(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfEx.BeforeEdit
        e.Cancel = True
    End Sub

    Private Sub vsfPfx_BeforeEdit(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfPfx.BeforeEdit
        Dim strTblName As String
        With vsfPfx
            .ComboList = ""
            Select Case e.Col
                Case vPfxCondOperator
                    .ComboList = " |=|>|<|>=|<=|<>"
                Case vPfxFldCode
                    strTblName = clsSvt.getDataStringValue("select HdrtblName from UtlTransactions where DocID = " & Val(vsfDet.GetData(vsfDet.Row, 1)), "", False)
                    If Trim(strTblName) <> "" Then
                        .ComboList = fillComboList(strTblName)
                    End If
                Case vPfxPrefix
                    '.EditMask = "aaa; "
                    '.EditMaxLength = 3
            End Select
        End With
    End Sub

    Private Function fillComboList(ByVal argTblName As String) As String
        fillComboList = ""
        Dim strList As String, i As Integer
        Dim rsDummy As oledb.oledbDataReader
        strList = ""

        rsDummy = clsSvt.getDataReaderFromSQL("Select * from " & argTblName & " where 1 <> 1")

        With rsDummy
            For i = 0 To .FieldCount - 1
                strList = strList & .GetName(i) & "|"
            Next i
            .Close()
        End With

        fillComboList = strList
    End Function

    Private Sub vsfPfx_EnterCell(ByVal sender As Object, ByVal e As System.EventArgs) Handles vsfPfx.EnterCell
        vsfPfx.AllowEditing = True

    End Sub

    Private Sub vsfPfx_KeyPressEdit(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.KeyPressEditEventArgs) Handles vsfPfx.KeyPressEdit
        Dim KeyAscii As Integer
        KeyAscii = Asc(UCase(Chr(KeyAscii)))
    End Sub

    Private Function pfxValid() As Boolean
        Dim i As Integer
        pfxValid = False
        If vsfPfx.Rows.Count = 1 Then Exit Function

        If clsSvt.ChkDuplicateData(vsfPfx, i, vPfxPrefix) Then
            MsgBox("Duplicate prefix at row " & i, vbInformation, Me.Text)
            Exit Function
        End If
        With vsfPfx
            For i = 1 To .Rows.Count - 1
                If .GetData(i, vPfxFldCode) <> "" Then
                    If .GetData(i, vPfxCondOperator) = "" Then
                        MsgBox("Please select the operator !!", vbInformation, Me.Text)
                        Exit Function
                    End If
                Else
                    .SetData(i, vPfxCondOperator, "")
                    .SetData(i, vPfxFldValue, "")
                End If
            Next i
        End With
        pfxValid = True
    End Function

    Private Sub btnPSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPSave.Click
        Dim strSQL As String, i As Integer
        If Not pfxValid() Then Exit Sub

        clsSvt.execSQLStatement("Delete from UtlDocNoPrefix where DocID = " & Val(vsfDet.GetData(vsfDet.Row, 1)))

        With vsfPfx
            For i = 1 To .Rows.Count - 1
                If Trim(.GetData(i, vPfxPrefix)) <> "" Then
                    strSQL = "Insert into UtlDocNoPrefix (DocID, Prefix, "
                    strSQL = strSQL & " cndOperator, CndFldCode, CndFldValue) Values ("
                    strSQL = strSQL & Val(vsfDet.GetData(vsfDet.Row, vDetDocId)) & ", "
                    strSQL = strSQL & "'" & Trim(.GetData(i, vPfxPrefix)) & "',"
                    strSQL = strSQL & "'" & Trim(.GetData(i, vPfxCondOperator)) & "',"
                    strSQL = strSQL & "'" & Trim(.GetData(i, vPfxFldCode)) & "',"
                    strSQL = strSQL & "'" & Trim(.GetData(i, vPfxFldValue)) & "')"

                    clsSvt.execSQLStatement(strSQL)
                End If
            Next i
        End With

        With vsfPfx
            If GetVal(.GetData(.Row, vPfxPrefix), "") = "" And GetVal(.GetData(.Row, vPfxFldCode), "") = "" And GetVal(.GetData(.Row, vPfxCondOperator), "") = "" And GetVal(.GetData(.Row, vPfxFldValue), "") = "" Then
                MsgBox("Please Fill Details")
            Else
                MsgBox("Saved Successfully!!", vbInformation, Me.Text)
            End If
        End With
        fraPrefix.Visible = False
    End Sub

    Private Sub vsfPfx_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles vsfPfx.KeyUp
        With vsfPfx
            If e.KeyCode = Keys.Tab And .Row = .Rows.Count - 1 And .Col = vPfxFldValue Then
                .Col = vPfxPrefix
                .Rows.Count = .Rows.Count + 1
                .Row = .Rows.Count - 1
            End If

            If e.KeyCode = Keys.D And Keys.Shift = 2 Then
                If .Rows.Count > 2 Then
                    If MsgBox("Do you want to delete " & .GetData(.Row, 0) & Chr(10) & _
                        " Proceed..?", vbYesNo + vbCritical, Me.Text) = vbYes Then
                        .RemoveItem(.Row)
                    End If
                Else
                    MsgBox("Atleast 1 Row should be present", , Me.Text)
                End If
            End If
        End With
    End Sub

    Private Sub btnPRemove_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPRemove.Click
        Try
            If MsgBox("Do You want to Remove the Prefix --> " & vsfPfx.GetData(vsfPfx.Row, vPfxPrefix) & " ?", vbInformation + vbYesNo, Me.Text) = vbYes Then
                vsfPfx.RemoveItem(vsfPfx.Row)
                If vsfPfx.Rows.Count = 1 Then vsfPfx.Rows.Count = 2
            End If
            Exit Sub
        Catch ex As Exception
        End Try
        'err:
        '            If Err.Number = 381 Then
        '                MsgBox("Please select the row in the Grid", vbOKOnly, Me.Text)
        '            End If
    End Sub

    Private Function CheckForDocNo() As Boolean
        Dim i As Integer, j As Integer
        Dim blnYesFlag As Boolean
        With vsfDet
            For i = 1 To .Rows.Count - 1
                If .GetData(i, 18) <> "" Then
                    blnYesFlag = False
                    For j = vDetColStart To 18
                        If .GetData(i, j) = "7" Or .GetData(i, j) = "8" Or .GetData(i, j) = "9" Or .GetData(i, j) = "10" Then
                            blnYesFlag = True
                            Exit For
                        End If
                    Next j
                    If Not blnYesFlag Then
                        CheckForDocNo = False
                        Exit Function
                    End If
                End If
            Next i
        End With
        CheckForDocNo = True
    End Function



    Private Sub vsfPfx_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles vsfPfx.Click

    End Sub

    Private Sub vsfDet_MouseHoverCell(sender As Object, e As EventArgs) Handles vsfDet.MouseEnterCell
        If vsfDet.Rows.Count > 1 Then
            If vsfDet.Col = vDetContDocNo Then
                lblInformation.Visible = True
            Else
                lblInformation.Visible = False
            End If
        End If

    End Sub

    'Private Sub vsfPfx_ValidateEdit(sender As Object, e As ValidateEditEventArgs) Handles vsfPfx.ValidateEdit
    '    With vsfPfx
    '        If GetVal(.GetData(.Row, vPfxPrefix), "") = "" And GetVal(.GetData(.Row, vPfxFldCode), "") = "" And GetVal(.GetData(.Row, vPfxCondOperator), "") = "" And GetVal(.GetData(.Row, vPfxFldValue), "") = "" Then
    '            MsgBox("row empty")
    '        Else
    '            btnPSave.Enabled = True
    '        End If
    '    End With
    'End Sub
End Class