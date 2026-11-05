Imports System.ComponentModel
Imports C1.Win.C1FlexGrid

Public Class frmUtlMstTrnBulkUpdateDesign

    Const vDisSel = 0
    Const vDisFldName = 1
    Const vDisFilReq = 2
    Const vDisAdnlTbl = 3
    Const vDisAdnlDisp = 4
    Const vDisDataType = 5
    Const vDisAdnlCond = 6
    Const vDisTotCols = 7

    Const vDetFldName = 0
    Const vDetSel = 1
    Const vDetDispSel = 2
    Const vDetFilReq = 3
    Const vDetAdnlTbl = 4
    Const vDetAdnlDisp = 5
    Const vDetAdnlCond = 6
    Const vDetDataType = 7
    Const vDetTotCols = 8

    Public strList As String
    Dim clsSVT As New svtLibrary
    Dim Mode As String = ""

    Dim strShForm As String = ""
    Dim strDocType As String = ""

    Private Sub frmUtlMstTrnBulkUpdate_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.KeyPreview = True
        Call fillFormProperties(Me)
        Call initGridStyles(Me)
        Call initScreen()
        Call initFormVsfDet(Me, Me.Text, vsfDet, "DET")
        Call initFormVsfDet(Me, Me.Text, vsfDisp, "DISP")
        ShowBtns()
    End Sub

    Private Sub initScreen()
        With vsfDet
            .Rows.Count = 1
            .Cols.Count = vDetTotCols

            .SetData(0, vDetFldName, "Storage Field")
            .SetData(0, vDetSel, "Update Fld")
            .SetData(0, vDetDispSel, "Display Fld")
            .SetData(0, vDetFilReq, "Filter Req")
            .SetData(0, vDetAdnlDisp, "Adnl Display Field")
            .SetData(0, vDetAdnlTbl, "Table Name")
            .SetData(0, vDetAdnlCond, "Condition")
            .SetData(0, vDetDataType, "Data Type")

            SetFlexColor(vsfDet, "M", vDetSel, vDetFldName, vDetDispSel)
            SetColAlign(vsfDet, svtAlign.Left, vDetFldName, vDetAdnlDisp, vDetAdnlTbl, vDetAdnlCond, vDetDataType)
            SetColAlign(vsfDet, svtAlign.Center, vDetSel)
            SetRowAlign(vsfDet, svtAlign.Center, 0)
            .Cols(vDetSel).DataType = GetType(Boolean)
            .Cols(vDetDispSel).DataType = GetType(Boolean)
            .Cols(vDetFilReq).DataType = GetType(Boolean)
        End With

        With vsfDisp
            .Rows.Count = 1
            .Cols.Count = vDisTotCols

            .SetData(0, vDisSel, "Sel")
            .SetData(0, vDisFldName, "Display Field")
            .SetData(0, vDisAdnlDisp, "Addnl Display")
            .SetData(0, vDisAdnlTbl, "Table Name")
            .SetData(0, vDisAdnlCond, "Condition")
            .SetData(0, vDisDataType, "Data Type")
            .SetData(0, vDisFilReq, "Filter Req")

            SetFlexColor(vsfDisp, "M", vDisSel, vDisFldName, vDisFilReq)
            SetColAlign(vsfDisp, svtAlign.Left, vDisFldName, vDisAdnlDisp, vDisAdnlTbl, vDisAdnlCond, vDisDataType)
            SetColAlign(vsfDisp, svtAlign.Center, vDisSel, vDisFilReq)
            SetRowAlign(vsfDisp, svtAlign.Center, 0)
            .Cols(vDisSel).DataType = GetType(Boolean)
            .Cols(vDisFilReq).DataType = GetType(Boolean)
        End With

        'bindCombo(cboTable, "TABLE_NAME", "TABLE_NAME", "Select TABLE_NAME From INFORMATION_SCHEMA.TABLES Where TABLE_NAME Like 'MST%' or TABLE_NAME Like 'TRNHDR%' Order by 1 ")
        bindCombo(cboTable, "TABLE_NAME", "TABLE_NAME", "Select TABLE_NAME From INFORMATION_SCHEMA.TABLES Where TABLE_NAME Like 'MST%' Order by 1 ")
        cboTable.Text = ""
    End Sub

    Private Sub ClearFlds()
        Mode = ""
        txtDocName.Text = ""
        cboTable.Text = ""
        cboPKFld.SelectedIndex = -1
        txtDocID.Text = ""
        EnableDisableFlds(False)
        strShForm = ""
        strDocType = ""
        vsfDet.Rows.Count = 1
        vsfDisp.Rows.Count = 1
    End Sub

    Private Sub ShowBtns()
        btnNew.Visible = False
        btnSave.Visible = False
        btnCancel.Visible = False
        btnBCList.Visible = False
        btnDelete.Visible = False
        btnSelTblName.Visible = False

        If Mode = "A" Then
            btnSave.Visible = True
            btnCancel.Visible = True
            btnSelTblName.Visible = True
        ElseIf Mode = "M" Then
            btnSave.Visible = True
            btnCancel.Visible = True
            btnDelete.Visible = True
        Else
            btnNew.Visible = True
            btnDelete.Visible = True
            btnBCList.Visible = True
        End If
    End Sub

    Private Function ValidateDispEntry() As Boolean

    End Function

    Private Sub EnableDisableFlds(ByVal argBln As Boolean)
        txtDocName.Enabled = argBln
        cboTable.Enabled = argBln
        'cboPKFld.Enabled = argBln
    End Sub

    Private Sub vsfDet_BeforeEdit(sender As Object, e As RowColEventArgs) Handles vsfDet.BeforeEdit
        If Not (Mode = "A" Or Mode = "M") Then
            e.Cancel = True
            Exit Sub
        End If

        e.Cancel = True
        With vsfDet
            ' If .Rows.Count - 1 <= 1 Then Exit Sub

            .ComboList = ""
            Select Case .Col
                Case vDetFldName
                    '.ComboList = GenVSfComboStringCond("COLUMN_NAME", "INFORMATION_SCHEMA.COLUMNS", "", " Where Table_Name = '" & Trim(cboTable.Text) & "' ")
                Case vDetAdnlTbl
                    e.Cancel = False
                    .ComboList = GenVSfComboStringCond("Table_Name", "INFORMATION_SCHEMA.TABLES", "", " Where TABLE_NAME Like 'MST%' or TABLE_NAME Like 'TRNHDR%' ")
                Case vDetAdnlDisp
                    e.Cancel = False
                    .ComboList = GenVSfComboStringCond("COLUMN_NAME", "INFORMATION_SCHEMA.COLUMNS", "", " Where Table_Name = '" & Trim(.GetData(.Row, vDetAdnlTbl)) & "' ")
                Case vDetDataType
                    e.Cancel = False
                    .ComboList = " |TEXT|NUMERIC|DATE"
                Case Else
                    e.Cancel = False
            End Select

            If .Row = 1 Then
                e.Cancel = True
            End If
        End With

    End Sub

    Private Sub vsfDisp_BeforeEdit(sender As Object, e As RowColEventArgs) Handles vsfDisp.BeforeEdit
        If Not (Mode = "A" Or Mode = "M") Then
            e.Cancel = True
            Exit Sub
        End If

        With vsfDisp
            .ComboList = ""
            Select Case .Col

                Case vDisFldName
                    .ComboList = GenVSfComboStringCond("COLUMN_NAME", "INFORMATION_SCHEMA.COLUMNS", "", " Where Table_Name = '" & Trim(cboTable.Text) & "' ")
                Case vDisAdnlTbl
                    .ComboList = GenVSfComboStringCond("Table_Name", "INFORMATION_SCHEMA.TABLES", "", " Where TABLE_NAME Like 'MST%' or TABLE_NAME Like 'TRNHDR%' ")
                Case vDisAdnlDisp
                    .ComboList = GenVSfComboStringCond("COLUMN_NAME", "INFORMATION_SCHEMA.COLUMNS", "", " Where Table_Name = '" & Trim(.GetData(.Row, vDisAdnlTbl)) & "' ")
                Case vDisDataType
                    .ComboList = " |TEXT|NUMERIC|DATE"
            End Select



        End With


    End Sub

    Private Sub btnNew_Click(sender As Object, e As EventArgs) Handles btnNew.Click
        ClearFlds()
        Mode = "A"
        EnableDisableFlds(True)
        ShowBtns()

    End Sub

    Private Function ValidFlds() As Boolean
        ValidFlds = False

        Dim k As Integer, strerr As String = ""

        'With vsfDet
        '    For k = 0 To .Rows.Count - 1

        '        For j = 1 To .Cols.Count - 1
        '            If .GetCellCheck(k, vDetSel) = CheckEnum.Checked Then
        '                If Trim(.GetData(k, 5)) = "" Then
        '                    .Col = 5
        '                    .Row = k
        '                    .Focus()
        '                    MsgBox("DataType Should not be blank", MsgBoxStyle.Exclamation, Me.Text)
        '                    Exit Function
        '                End If
        '            End If
        '        Next j
        '    Next k
        'End With

        If Trim(txtDocName.Text) = "" Then
            MsgBox("Document Name is Empty...", MsgBoxStyle.Exclamation, Me.Text)
            txtDocName.Focus()
            Exit Function
        End If

        If clsSVT.getDataLongValue("Select count(*) From UtlMstTrnBulkUpdate Where rowid <> " & Val(txtDocID.Text) & " and DocName = '" & Trim(txtDocName.Text) & "'", 0, False) > 0 Then
            MsgBox("Document Already Present in the table...", MsgBoxStyle.Exclamation, Me.Text)
            Exit Function
        End If

        If Trim(cboTable.Text) = "" Then
            MsgBox("Table Name is Empty...", MsgBoxStyle.Exclamation, Me.Text)
            cboTable.Focus()
            Exit Function
        End If

        If Trim(cboPKFld.Text) = "" Then
            MsgBox("Table Key Name is Empty...", MsgBoxStyle.Exclamation, Me.Text)
            cboPKFld.Focus()
            Exit Function
        End If

        If vsfDet.Rows.Count <= 1 Then
            MsgBox("Update Fields are empty...", MsgBoxStyle.Exclamation, Me.Text)
            txtDocName.Focus()
            Exit Function
        End If

        'If vsfDisp.Rows.Count <= 1 Then
        '    MsgBox("Display Fields are empty...", MsgBoxStyle.Exclamation, Me.Text)
        '    txtDocName.Focus()
        '    Exit Function
        'End If

        Dim blnFound As Boolean = False

        With vsfDet
            For i As Int32 = 1 To .Rows.Count - 1
                If .GetCellCheck(i, vDetSel) = CheckEnum.Checked Then
                    blnFound = True
                    Exit For
                End If
            Next

            If Not blnFound Then
                MsgBox("Please Select Atleast One Field From Update Field Grid...", MsgBoxStyle.Exclamation, Me.Text)
                vsfDet.Focus()
                Exit Function
            End If
        End With

        With vsfDisp
            For i As Int32 = 1 To .Rows.Count - 1
                If .GetCellCheck(i, vDisSel) = CheckEnum.Checked Then
                    blnFound = True
                    Exit For
                End If
            Next

            If Not blnFound Then
                MsgBox("Please Select Atleast One Field From Display Field Grid...", MsgBoxStyle.Exclamation, Me.Text)
                vsfDisp.Focus()
                Exit Function
            End If
        End With

        ValidFlds = True
    End Function

    Private Function SaveDetails() As Boolean
        SaveDetails = False

        Dim strSQL As String = ""

        Dim strDispCol As String = ""
        Dim strUpdateCol As String = ""
        Dim strFilterCol As String = ""

        With vsfDet
            For i As Int32 = 1 To .Rows.Count - 1
                If .GetCellCheck(i, vDetSel) = CheckEnum.Checked Or .GetCellCheck(i, vDetDispSel) = CheckEnum.Checked Then
                    strUpdateCol &= GetVal(.GetData(i, vDetFldName), "") & ","
                    If .GetCellCheck(i, vDetSel) = CheckEnum.Checked Then
                        strUpdateCol &= "Y,"
                    Else
                        strUpdateCol &= "N,"
                    End If
                    If .GetCellCheck(i, vDetDispSel) = CheckEnum.Checked Then
                        strUpdateCol &= "Y,"
                    Else
                        strUpdateCol &= "N,"
                    End If
                    If .GetCellCheck(i, vDetFilReq) = CheckEnum.Checked Then
                        strUpdateCol &= "Y,"
                    Else
                        strUpdateCol &= "N,"
                    End If
                    strUpdateCol &= GetVal(.GetData(i, vDetAdnlTbl), "") & ","
                    strUpdateCol &= GetVal(.GetData(i, vDetAdnlDisp), "") & ","
                    strUpdateCol &= GetVal(.GetData(i, vDetAdnlCond), "") & ","
                    strUpdateCol &= GetVal(.GetData(i, vDetDataType), "") & "^"
                End If
            Next

            If strUpdateCol <> "" Then strUpdateCol = Mid(strUpdateCol, 1, Len(strUpdateCol) - 1)

            For i As Int32 = 1 To .Rows.Count - 1
                'If .GetCellCheck(i, vDisSel) = CheckEnum.Checked Then
                '    strDispCol &= GetVal(.GetData(i, vDisFldName), "") & ","
                '    strDispCol &= GetVal(.GetData(i, vDisAdnlTbl), "") & ","
                '    strDispCol &= GetVal(.GetData(i, vDisAdnlDisp), "") & ","
                '    strDispCol &= GetVal(.GetData(i, vDisDataType), "") & ","
                '    strDispCol &= GetVal(.GetData(i, vDisAdnlCond), "") & "^"
                'End If

                If .GetCellCheck(i, vDetFilReq) = CheckEnum.Checked Then
                    strFilterCol &= GetVal(.GetData(i, vDetFldName), "") & ","
                    strFilterCol &= GetVal(.GetData(i, vDetAdnlTbl), "") & ","
                    strFilterCol &= GetVal(.GetData(i, vDetAdnlDisp), "") & ","
                    strFilterCol &= GetVal(.GetData(i, vDetDataType), "") & ","
                    strFilterCol &= GetVal(.GetData(i, vDetAdnlCond), "") & "^"
                End If
            Next

            'If strDispCol <> "" Then strDispCol = Mid(strDispCol, 1, Len(strDispCol) - 1)
            If strFilterCol <> "" Then strFilterCol = Mid(strFilterCol, 1, Len(strFilterCol) - 1)

        End With

        If Mode = "A" Then
            strSQL = " Insert Into UtlMstTrnBulkUpdate(DocName, TblName, KeyFields, DispCols, FilterCols, StorageCols) "
            strSQL &= " Values ( "
            strSQL &= "'" & Trim(txtDocName.Text) & "',"
            strSQL &= "'" & Trim(cboTable.Text) & "',"
            strSQL &= "'" & Trim(cboPKFld.Text) & "',"
            'strSQL &= "'" & Trim(strDispCol) & "',"
            strSQL &= "'',"
            strSQL &= "'" & Trim(strFilterCol) & "',"
            strSQL &= "'" & Trim(strUpdateCol) & "')"

        Else
            strSQL = " Update UtlMstTrnBulkUpdate Set "
            strSQL &= " DocName = '" & Trim(txtDocName.Text) & "',"
            strSQL &= " TblName = '" & Trim(cboTable.Text) & "',"
            strSQL &= " KeyFields = '" & Trim(cboPKFld.Text) & "',"
            'strSQL &= " DispCols = '" & Trim(strDispCol) & "',"
            strSQL &= " FilterCols = '" & Trim(strFilterCol) & "',"
            strSQL &= " StorageCols = '" & Trim(strUpdateCol) & "' "
            strSQL &= " Where rowid = " & Val(txtDocID.Text)
        End If

        If Not clsSVT.execSQLStatement(strSQL) Then
            Exit Function
        End If

        If Mode = "A" Then
            MsgBox("Added Successfully...", MsgBoxStyle.Information, Me.Text)
            txtDocID.Text = clsSVT.getDataLongValue("Select Max(rowid) From UtlMstTrnBulkUpdate", 0, False)
        Else
            MsgBox("Updated Successfully...", MsgBoxStyle.Information, Me.Text)
        End If

        SaveDetails = True
    End Function

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        If ValidFlds() Then
            If SaveDetails() Then
                'MsgBox("Saved Successfully....", MsgBoxStyle.Exclamation, Me.Text)
                Mode = ""
                EnableDisableFlds(False)
                ShowBtns()
            End If
        End If
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Mode = ""
        ClearFlds()
        ShowBtns()
    End Sub

    Private Sub btnBCList_Click(sender As Object, e As EventArgs) Handles btnBCList.Click
        ClearFlds()
        txtDummy.Text = ""

        Dim strSQL As String = ""
        strSQL = " Select rowid, DocName, Tblname, KeyFields "
        strSQL &= " From UtlMstTrnBulkUpdate order by 1 "

        Dim objfrm As New frmUtlSelection
        With objfrm
            .FillViewVsf(Me, False, Nothing, strSQL, txtDummy, 0)
            .Text = " Please Select Document "
            .ShowDialog()
            .Close()
        End With

        If Val(txtDummy.Text) > 0 Then
            Call FillDocDetails(Val(txtDummy.Text))
            ShowBtns()
        End If

    End Sub

    Private Sub FillDocDetails(ByVal argID As Int32)
        Dim strSQL As String = ""
        Dim rs As OleDb.OleDbDataReader

        Dim strUpdateCol As String = ""
        Dim strFilterCol As String = ""
        Dim strDispCol As String = ""
        Mode = "M"

        strSQL = "Select * From UtlMstTrnBulkUpdate Where rowid = " & argID
        rs = clsSVT.getDataReaderFromSQL(strSQL)
        If rs.HasRows Then
            rs.Read()
            txtDocID.Text = rs!rowid
            txtDocName.Text = rs!DocName
            cboTable.Text = rs!TblName
            Call cboTable_Validating(Nothing, Nothing)
            'cboPKFld.Items.Clear()
            'cboPKFld.Text = rs!KeyFields
            strDispCol = rs!DispCols
            strUpdateCol = rs!StorageCols
            strFilterCol = rs!Filtercols
        End If
        rs.Close()




        vsfDet.Rows.Count = 1
        vsfDisp.Rows.Count = 1

        If InStr(Trim(cboTable.Text).ToUpper, "MST") > 0 Then
            strDocType = "M"
        Else
            strDocType = "T"
        End If

        strShForm = Trim(cboTable.Text).ToUpper().Replace("MST", "").Replace("TRNHDR", "")

        FillData()

        Dim intRow As Int32 = 0, intCol = 0
        With vsfDet
            For Each strLine As String In strUpdateCol.Split("^")
                intRow = -1
                intCol = 0
                For Each str As String In strLine.Split(",")
                    If intCol = 0 Then
                        intRow = .FindRow(str, 1, vDetFldName, False, True, False)
                    End If
                    If intRow > -1 Then
                        Select Case intCol
                            Case 0
                                .SetData(intRow, vDetFldName, str)
                            Case 1
                                If str = "Y" Then
                                    .SetCellCheck(intRow, vDetSel, CheckEnum.Checked)
                                Else
                                    .SetCellCheck(intRow, vDetSel, CheckEnum.Unchecked)
                                End If
                                '.SetCellCheck(intRow, vDetSel, CheckEnum.Checked)
                            Case 2
                                If str = "Y" Then
                                    .SetCellCheck(intRow, vDetDispSel, CheckEnum.Checked)
                                Else
                                    .SetCellCheck(intRow, vDetDispSel, CheckEnum.Unchecked)
                                End If

                            Case 3
                                If str = "Y" Then
                                    .SetCellCheck(intRow, vDetFilReq, CheckEnum.Checked)
                                Else
                                    .SetCellCheck(intRow, vDetFilReq, CheckEnum.Unchecked)
                                End If

                            Case 4
                                .SetData(intRow, vDetAdnlTbl, str)
                            Case 5
                                .SetData(intRow, vDetAdnlDisp, str)
                            Case 6
                                .SetData(intRow, vDetAdnlCond, str)
                        End Select
                    End If
                    intCol = intCol + 1
                Next
            Next
        End With

        With vsfDisp
            For Each strLine As String In strDispCol.Split("^")
                intRow = -1
                intCol = 0
                For Each str As String In strLine.Split(",")
                    If intCol = 0 Then
                        intRow = .FindRow(str, 1, vDisFldName, False, True, False)
                    End If
                    If intRow > -1 Then
                        Select Case intCol
                            Case 0
                                .SetCellCheck(intRow, vDisSel, CheckEnum.Checked)
                            Case 1
                                .SetData(intRow, vDisAdnlTbl, str)
                            Case 2
                                .SetData(intRow, vDisAdnlDisp, str)
                            Case 3
                                .SetData(intRow, vDisDataType, str)
                            Case 4
                                .SetData(intRow, vDisAdnlCond, str)
                        End Select
                    End If
                    intCol = intCol + 1
                Next
            Next
        End With

        With vsfDisp
            For Each strLine As String In strFilterCol.Split("^")
                intRow = -1
                intCol = 0
                For Each str As String In strLine.Split(",")
                    If intCol = 0 Then
                        intRow = .FindRow(str, 1, vDisFldName, False, True, False)
                    End If
                    If intRow > -1 Then
                        Select Case intCol
                            Case 0
                                .SetCellCheck(intRow, vDisFilReq, CheckEnum.Checked)
                            Case 1
                                .SetData(intRow, vDisAdnlTbl, str)
                            Case 2
                                .SetData(intRow, vDisAdnlDisp, str)
                            Case 3
                                .SetData(intRow, vDisDataType, str)
                            Case 4
                                .SetData(intRow, vDisAdnlCond, str)
                        End Select
                    End If
                    intCol = intCol + 1
                Next
            Next
        End With
    End Sub

    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        If Val(txtDocID.Text) > 0 Then
            If MsgBox("Are you sure? Do you want to delete this document....", MsgBoxStyle.YesNo, Me.Text) = MsgBoxResult.No Then Exit Sub

            clsSVT.execSQLStatement("Delete From UtlMstTrnBulkUpdate Where RowID = " & Val(txtDocID.Text))

            MsgBox("Removed Successfully...", MsgBoxStyle.Exclamation, Me.Text)


            Mode = ""
            ClearFlds()
            ShowBtns()
        End If
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        frmMain.CloseSelectedTabPage(Me)
    End Sub

    Private Sub cboTable_Validating(sender As Object, e As CancelEventArgs) Handles cboTable.Validating
        If Not (Mode = "A" Or Mode = "M") Then
            Exit Sub
        End If
        If cboTable.Text = "" Then Exit Sub

        If Not fnTablePresent(cboTable.Text) Then
            MsgBox("Please Select a valid SQL Table!", MsgBoxStyle.Exclamation, Me.Text)
            Exit Sub
        End If

        bindCombo(cboPKFld, "COLUMN_NAME", "COLUMN_NAME", "Select COLUMN_NAME From INFORMATION_SCHEMA.COLUMNS Where COLUMN_NAME like '%ID' and TABLE_NAME = '" & Trim(cboTable.Text) & "'")

        vsfDet.Rows.Count = 1
        'vsfDet.Rows.Count = 2
        'vsfDisp.Rows.Count = 1
        ' vsfDisp.Rows.Count = 2

        If InStr(Trim(cboTable.Text).ToUpper, "MST") > 0 Then
            strDocType = "M"
        Else
            strDocType = "T"
        End If

        strShForm = Trim(cboTable.Text).ToUpper().Replace("MST", "").Replace("TRNHDR", "")

        cboPKFld.Text = strShForm & "ID"

        Call FillData()
    End Sub

    Private Sub txtDocName_Validating(sender As Object, e As CancelEventArgs) Handles txtDocName.Validating
        If Not (Mode = "A" Or Mode = "M") Then
            Exit Sub
        End If

        If Mode = "A" Then
            If clsSVT.getDataLongValue("Select count(*) From UtlMstTrnBulkUpdate Where DocName = '" & Trim(txtDocName.Text) & "'", 0, False) > 0 Then
                MsgBox("Document Already Present in the table...", MsgBoxStyle.Exclamation, Me.Text)
                Exit Sub
            End If
        Else
            If clsSVT.getDataLongValue("Select Count(*) From UtlMstTrnBulkUpdate Where rowid <> " & Val(txtDocID.Text) & " and DocName = '" & Trim(txtDocName.Text) & "'", 0, False) > 0 Then
                MsgBox("Document Already Present in the table...", MsgBoxStyle.Exclamation, Me.Text)
                Exit Sub
            End If
        End If
    End Sub

    Private Sub FillData()
        Dim strSQL As String = ""
        Dim rs As OleDb.OleDbDataReader

        'strSQL = "Select COLUMN_NAME,DATA_TYPE From INFORMATION_SCHEMA.COLUMNS Where table_name = '" & Trim(cboTable.Text) & "' AND ISNULL(CHARACTER_MAXIMUM_LENGTH,0) <> -1 ORDER BY ORDINAL_POSITION "


        strSQL = "Select COLUMN_NAME,Case when DATA_TYPE='varchar' or DATA_TYPE='Char' then 'TEXT'when DATA_TYPE='int' or DATA_TYPE='numeric' Then 'NUMERIC'when DATA_TYPE Like '%date%' then 'DATE'
  End as DATA_TYPE From INFORMATION_SCHEMA.COLUMNS Where table_name =  '" & Trim(cboTable.Text) & "' AND ISNULL( CHARACTER_MAXIMUM_LENGTH,0) <> -1 ORDER BY ORDINAL_POSITION "


        rs = clsSVT.getDataReaderFromSQL(strSQL)
        With vsfDet
            Do While rs.Read
                .Rows.Count = .Rows.Count + 1
                .SetData(.Rows.Count - 1, vDetFldName, rs!Column_name)
                .SetData(.Rows.Count - 1, vDetDataType, rs!Data_Type)

                If Trim(rs!Column_name).ToUpper = strShForm & "ID" Or Trim(rs!Column_name).ToUpper = strShForm & "CODE" Or Trim(rs!Column_name).ToUpper = strShForm & "NAME" Or Trim(rs!Column_name).ToUpper = strShForm & "DATE" Then 'Or Trim(rs!Column_name).ToUpper = strShForm & "ID" Or Trim(rs!Column_name).ToUpper = strShForm & "ID" Then
                    'Or Trim(rs!Column_name).ToUpper = strShForm & "NO" 

                    .SetCellCheck(.Rows.Count - 1, vDetDispSel, CheckEnum.Checked)
                End If
                If UCase(rs!Column_name) = UCase(cboPKFld.Text) Then
                    .Rows(.Rows.Count - 1).Visible = False
                End If
            Loop
        End With
        rs.Close()
    End Sub


    Private Sub vsfDet_MouseClick(sender As Object, e As MouseEventArgs) Handles vsfDet.MouseClick
        Dim strFldName As String

        With vsfDet
            If .Rows.Count - 1 <= 1 Then Exit Sub

            If .GetCellCheck(.Row, vDetSel) = CheckEnum.Checked Or .GetCellCheck(.Row, vDetDispSel) = CheckEnum.Checked Then
                If .GetData(.Row, vDetFldName) = cboPKFld.Text Then Exit Sub
                If InStr(.GetData(.Row, vDetFldName), "ID") > 0 Then
                    strFldName = Strings.Replace(.GetData(.Row, vDetFldName), "ID", "")
                    If fnTablePresent("Mst" & strFldName) Then
                        .SetData(.Row, vDetAdnlTbl, "Mst" & strFldName)
                        .SetData(.Row, vDetAdnlDisp, strFldName & "Code")
                        .SetData(.Row, vDetAdnlCond, "Mst" & strFldName & "." & strFldName & "ID=" & cboTable.Text & "." & strFldName & "ID")
                    End If
                End If
            End If

            If .GetCellCheck(.Row, vDetSel) = CheckEnum.Unchecked And .GetCellCheck(.Row, vDetDispSel) = CheckEnum.Unchecked Then
                .SetData(.Row, vDetAdnlTbl, "")
                .SetData(.Row, vDetAdnlDisp, "")
                .SetData(.Row, vDetAdnlCond, "")
            End If

            If .Col = vDetSel Then
                If .GetCellCheck(.Row, vDetSel) = CheckEnum.Checked Then
                    .SetCellCheck(.Row, vDetDispSel, CheckEnum.Checked)
                    Exit Sub
                End If
            End If
            If .Col = vDetDispSel Then
                If .GetCellCheck(.Row, vDetDispSel) = CheckEnum.Unchecked Then
                    .SetCellCheck(.Row, vDetSel, CheckEnum.Unchecked)
                    Exit Sub
                End If
            End If
        End With
    End Sub

    Private Sub vsfDet_KeyUp(sender As Object, e As KeyEventArgs) Handles vsfDet.KeyUp
        If e.KeyCode = Keys.F9 And e.Control = True Then
            Call initFormVsfDet(Me, Me.Text, vsfDet, "DET")
            MsgBox("Restored Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
        If e.KeyCode = Keys.F8 And e.Control = True Then
            Call SaveActFormVsfDet(Me, Me.Text, "DET", vsfDet)
            MsgBox("Saved Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
    End Sub

    Private Sub btnSelTblName_Click(sender As Object, e As EventArgs) Handles btnSelTblName.Click
        Dim strSql As String
        strSql = "Select isnull((Select top 1 DocName from UtlTransactions where HdrTblName=INFORMATION_SCHEMA.TABLES.TABLE_NAME),replace(TABLE_NAME,'Mst','')) as TableReference,TABLE_NAME as TableName From INFORMATION_SCHEMA.TABLES Where TABLE_NAME Like 'MST%' Order by 2"

        Dim frmDisplay As New frmUtlSingleSel
        With frmDisplay
            .Text = "Select Table Name"
            Call .FillViewVsf(Me, strSql, cboTable, 1, -1)
            .ShowDialog()
        End With
        cboTable.Focus()
    End Sub

    Private Sub cboTable_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboTable.SelectedIndexChanged

    End Sub

    'Private Sub FillData()
    '    Dim strSQL As String = ""
    '    Dim rs As OleDb.OleDbDataReader

    '    strSQL = "Select COLUMN_NAME From INFORMATION_SCHEMA.COLUMNS Where table_name = '" & Trim(cboTable.Text) & "' AND ISNULL(CHARACTER_MAXIMUM_LENGTH,0) <> -1 ORDER BY ORDINAL_POSITION "

    '    rs = clsSVT.getDataReaderFromSQL(strSQL)
    '    With vsfDet
    '        Do While rs.Read
    '            .Rows.Count = .Rows.Count + 1
    '            .SetData(.Rows.Count - 1, vDetFldName, rs!Column_name)
    '            'If Trim(rs!Column_name).ToUpper = strShForm & "ID" Or Trim(rs!Column_name).ToUpper = strShForm & "CODE" Or Trim(rs!Column_name).ToUpper = strShForm & "NAME" Or Trim(rs!Column_name).ToUpper = strShForm & "NO" Or Trim(rs!Column_name).ToUpper = strShForm & "DATE" Then 'Or Trim(rs!Column_name).ToUpper = strShForm & "ID" Or Trim(rs!Column_name).ToUpper = strShForm & "ID" Then
    '            '    .SetCellCheck(.Rows.Count - 1, vDetSel, CheckEnum.Checked)
    '            'End If
    '        Loop
    '    End With
    '    rs.Close()

    '    rs = clsSVT.getDataReaderFromSQL(strSQL)
    '    With vsfDisp
    '        Do While rs.Read
    '            .Rows.Count = .Rows.Count + 1
    '            .SetData(.Rows.Count - 1, vDisFldName, rs!Column_name)
    '            If Trim(rs!Column_name).ToUpper = strShForm & "ID" Or Trim(rs!Column_name).ToUpper = strShForm & "CODE" Or Trim(rs!Column_name).ToUpper = strShForm & "NAME" Or Trim(rs!Column_name).ToUpper = strShForm & "NO" Or Trim(rs!Column_name).ToUpper = strShForm & "DATE" Then 'Or Trim(rs!Column_name).ToUpper = strShForm & "ID" Or Trim(rs!Column_name).ToUpper = strShForm & "ID" Then
    '                .SetCellCheck(.Rows.Count - 1, vDisSel, CheckEnum.Checked)
    '            End If
    '        Loop
    '    End With
    'End Sub

End Class