Imports System.ComponentModel
Imports System.Data.OleDb
Imports C1.Win.C1FlexGrid
Imports Microsoft.Office.Interop
Imports System.IO
Imports System.Text.RegularExpressions

Public Class frmUtlMstTrnBulkUpdate

    Public strList As String
    Dim clsSVT As New svtLibrary
    Dim strFlds As String
    Dim svtTrn As OleDb.OleDbTransaction

    Dim dvFlds As DataView
    Dim dtFlds As DataTable
    Dim dvTemp As DataView
    Dim dtTemp As DataTable

    Dim strSHForm As String = ""
    Dim strDocType As String = ""
    Dim intDocID As Int32 = 0
    Dim strTblName As String = ""
    Dim strKeyField As String = ""

    Dim strUpdateCol As String = ""
    Dim strFilterCol As String = ""
    Dim strDispCol As String = ""
    Dim strCondTblName As String

    Const vFldName = 0
    Const vFldSel = 1
    Const vFldDispSel = 2
    Const vFldFilReq = 3
    Const vFldAdnlTbl = 4
    Const vFldAdnlDisp = 5
    Const vFldAdnlCond = 6
    Const vFldDataType = 7
    Const vFldTotCols = 8

    Const vFilFldCode = 0
    Const vFilFldCaption = 1
    Const vFilFldDataType = 2
    Const vFilTblName = 3
    Const vFilBtn = 4
    Const vFilCol1 = 5
    Const vFilCol2 = 6
    Const vFilTotCols = 7

    Const vSelFilFldCode = 0
    Const vSelFldCaption = 1
    Const vSelFldFilterValue = 2
    Const vSelFilProcessed = 3
    Const vSelFilTblName = 4
    Const vSelFilTotCols = 5

    Const vDetRowFldName = 0
    Const vDetRowDatatype = 1
    Const vDetRowForKey = 2
    Const vDetRowMaxLen = 3
    Const vDetRowUpdate = 4

    Dim dt As DataTable
    Dim dv As DataView
    Dim blnDirUpdate As Boolean = False
    Dim strCond As String
    Dim blnGenTemplate As Boolean = False
    Dim strSqlTemp As String
    Dim strSqlTempTbl As String
    Dim blnValidData As Boolean

    Private Sub fillFilterFields()
        Dim rg As CellRange
        Dim cs As CellStyle

        Dim blnDateFldPresent As Boolean
        Dim blnFilterPresent As Boolean
        cs = vsfFilter.Styles.Add("date")
        cs.DataType = GetType(DateTime)
        cs.Format = "dd/MM/yyyy"
        cs.ForeColor = Color.DarkGoldenrod

        Dim rsDummy As OleDb.OleDbDataReader
        rsDummy = clsSVT.getDataReaderFromSQL("Select * from UtlReportsCRDtl where FilterReq = 'Y' and CRID = " & intDocID & " order by FldOrderBy ")
        vsfFilter.Rows.Count = 1
        vsfSelFilters.Rows.Count = 1
        blnDateFldPresent = False
        blnFilterPresent = False
        With rsDummy
            If .HasRows Then
                Do While .Read
                    vsfFilter.Rows.Count = vsfFilter.Rows.Count + 1
                    vsfFilter.SetData(vsfFilter.Rows.Count - 1, vFilTblName, !TblName)
                    vsfFilter.SetData(vsfFilter.Rows.Count - 1, vFilFldCode, !Fldcode)
                    vsfFilter.SetData(vsfFilter.Rows.Count - 1, vFilFldCaption, !Fldcaption)
                    vsfFilter.SetData(vsfFilter.Rows.Count - 1, vFilFldDataType, !FldDataType)

                    If !fldDataType = "DATE" Then
                        vsfFilter.SetData(vsfFilter.Rows.Count - 1, vFilCol1, Format(gSettings.gCompStDt, "dd/MM/yyyy"))
                        vsfFilter.SetData(vsfFilter.Rows.Count - 1, vFilCol2, Format(getSystemDate, "dd/MM/yyyy"))
                        rg = vsfFilter.GetCellRange(vsfFilter.Rows.Count - 1, vFilCol1, vsfFilter.Rows.Count - 1, vFilCol2)
                        rg.Style = vsfFilter.Styles("date")
                        blnDateFldPresent = True
                    Else
                        blnFilterPresent = True
                    End If
                Loop
            End If
            .Close()
        End With
        If Not blnDateFldPresent Then
            vsfFilter.Cols(vFilCol1).Visible = False
            vsfFilter.Cols(vFilCol2).Visible = False
        Else
            vsfFilter.Cols(vFilCol1).Visible = True
            vsfFilter.Cols(vFilCol2).Visible = True
        End If
        If Not blnFilterPresent Then
            vsfFilter.Cols(vFilBtn).Visible = False
            lblSelFilters.Visible = False
            vsfSelFilters.Visible = False
            btnRemFilter.Visible = False
            btnClearFilters.Visible = False
        Else
            vsfFilter.Cols(vFilBtn).Visible = True
            lblSelFilters.Visible = True
            vsfSelFilters.Visible = True
            btnRemFilter.Visible = True
            btnClearFilters.Visible = True
            ' btnPrintReport.Visible = True
            btnRemFilter.Enabled = False
            btnClearFilters.Enabled = False
        End If
    End Sub
    Private Sub ClearFlds()
        'vsfDet.Rows.Count = 1
        vsfFlds.Rows.Count = 1
        vsfFilter.Rows.Count = 1
        vsfSelFilters.Rows.Count = 1

        strSHForm = ""
        strDocType = ""
        intDocID = 0
        strTblName = ""
        strKeyField = ""

        strUpdateCol = ""
        strFilterCol = ""
        strDispCol = ""

        cboDocName.SelectedIndex = -1

        btnLoad.Enabled = True
        txtSelect.Text = ""
        txtNoofRec.Text = ""
        btnSave.Visible = False
        cboDocName.Enabled = True

        initScreen()
        lblTotNoOfRec.Text = "Total Number of Records Loaded : "
    End Sub

    Private Sub frmUtlMstTrnBulkUpdate_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.KeyPreview = True
        Call fillFormProperties(Me)
        Call initGridStyles(Me)
        Call initScreen()
        Call initFormVsfDet(Me, Me.Text, vsfFlds, "FLDS")
        Call initFormVsfDet(Me, Me.Text, vsfFilter, "FILTER")
        Call initFormVsfDet(Me, Me.Text, vsfSelFilters, "SELFILTER")
    End Sub

    Private Sub initScreen()
        With vsfFlds
            .Cols.Count = vFldTotCols
            .Rows.Count = 1
            .SetData(0, vFldName, "Field Names")
            .SetData(0, vFldSel, "Update Flds")
            .SetData(0, vFldDispSel, "Display Flds")
            .SetData(0, vFldFilReq, "Filter Required")
            .SetData(0, vFldAdnlCond, "Adnl Condition")
            .SetData(0, vFldAdnlDisp, "Adnl Display Col")
            .SetData(0, vFldAdnlTbl, "Table Names")
            .SetData(0, vFldDataType, "Data Type")

            .Cols(vFldSel).DataType = GetType(Boolean)
            .Cols(vFldDispSel).DataType = GetType(Boolean)
            .Cols(vFldFilReq).DataType = GetType(Boolean)
            'SetHideCol(vsfFlds, True, vFldsDataType)
        End With

        With vsfFilter
            .Cols.Count = vFilTotCols
            .SetData(0, vFilFldCaption, "Filter on")
            .SetData(0, 4, "Select Filter")
            .SetData(0, 5, "From")
            .SetData(0, 6, "To")
            .Cols(vFilFldDataType).Visible = False
            .Cols(vFilTblName).Visible = False
            .Cols(vFilFldCode).Visible = False
            .Cols(vFilBtn).ComboList = "..."
            .Cols(vFilBtn).Width = 70
            .Cols(vFilFldCaption).Width = 150
            .Cols(vFilCol1).Width = 100
            .Cols(vFilCol2).Width = 100
        End With
        bindCombo(cboDocName, "DocName", "DocName", "Select DocName From UtlMstTrnBulkUpdate Order by 1")
        cboDocName.Text = ""

        With vsfSelFilters
            .Rows.Count = 1
            .Cols.Count = vSelFilTotCols
            .SetData(0, vSelFldCaption, "Filtered On")
            .SetData(0, vSelFldFilterValue, "Filtered Value")
            .Cols(vSelFilFldCode).Visible = False
            .Cols(vSelFilProcessed).Visible = False
            .Cols(vSelFilTblName).Visible = False
        End With

        Call initFormVsfDet(Me, Me.Text, vsfFlds, "FLDS")

        With vsfDet
            .DataBindings.Clear()
            .DataSource = Nothing
            .Rows.Count = 5
            .SetData(vDetRowFldName, 0, "Field Name")
            .SetData(vDetRowDatatype, 0, "Data Type")
            .SetData(vDetRowForKey, 0, "Foreign Key")
            .SetData(vDetRowMaxLen, 0, "MaxLength")
            .SetData(vDetRowUpdate, 0, "Update Fld")
        End With
    End Sub

    Private Sub btnLoad_Click(sender As Object, e As EventArgs) Handles btnLoad.Click
        If cboDocName.Text = "" Then
            MsgBox("Please Select a Table!", MsgBoxStyle.Information, Me.Text)
            Exit Sub
        End If
        Dim rg As CellRange
        Dim strSql As String
        Dim rsTbl As OleDb.OleDbDataReader

        strSql = "Select StorageCols,FilterCols,TblName,KeyFields from UTLMSTTRNBULKUPDATE where DocName='" & cboDocName.Text & "'"
        rsTbl = clsSVT.getDataReaderFromSQL(strSql)

        If Not rsTbl.HasRows Then
            rsTbl.Close()
            Exit Sub
        End If

        vsfFlds.Rows.Count = 1
        vsfFilter.Rows.Count = 1

        Dim intColCtr As Integer = 0

        Do While rsTbl.Read
            txtTblName.Text = rsTbl!TblName
            txtIDfld.Text = rsTbl!KeyFields

            If rsTbl!StorageCols <> "" Then
                For Each strFld As String In rsTbl!StorageCols.Split("^")
                    intColCtr = vFldName + 1
                    vsfFlds.Rows.Count = vsfFlds.Rows.Count + 1
                    For Each strFldName As String In strFld.Split(",")
                        Select Case intColCtr
                            Case 1
                                'vsfFlds.SetData(vsfFlds.Rows.Count - 1, intColCtr, strFldName)
                                vsfFlds.SetData(vsfFlds.Rows.Count - 1, vFldName, strFldName)
                            Case 2
                                If strFldName = "Y" Then
                                    vsfFlds.SetCellCheck(vsfFlds.Rows.Count - 1, vFldSel, CheckEnum.Checked)
                                Else
                                    vsfFlds.SetCellCheck(vsfFlds.Rows.Count - 1, vFldSel, CheckEnum.Unchecked)
                                End If
                            Case 3
                                If strFldName = "Y" Then
                                    vsfFlds.SetCellCheck(vsfFlds.Rows.Count - 1, vFldDispSel, CheckEnum.Checked)
                                Else
                                    vsfFlds.SetCellCheck(vsfFlds.Rows.Count - 1, vFldDispSel, CheckEnum.Unchecked)
                                End If
                            Case 4
                                If strFldName = "Y" Then
                                    vsfFlds.SetCellCheck(vsfFlds.Rows.Count - 1, vFldFilReq, CheckEnum.Checked)
                                Else
                                    vsfFlds.SetCellCheck(vsfFlds.Rows.Count - 1, vFldFilReq, CheckEnum.Unchecked)
                                End If
                            Case 5
                                vsfFlds.SetData(vsfFlds.Rows.Count - 1, vFldAdnlTbl, strFldName)
                            Case 6
                                vsfFlds.SetData(vsfFlds.Rows.Count - 1, vFldAdnlDisp, strFldName)
                            Case 7
                                vsfFlds.SetData(vsfFlds.Rows.Count - 1, vFldAdnlCond, strFldName)
                            Case 8
                                vsfFlds.SetData(vsfFlds.Rows.Count - 1, vFldDataType, strFldName)
                        End Select

                        intColCtr = intColCtr + 1
                    Next
                Next
            End If
            If rsTbl!FilterCols <> "" Then
                For Each strFld As String In rsTbl!FilterCols.Split("^")
                    intColCtr = 1
                    vsfFilter.Rows.Count = vsfFilter.Rows.Count + 1
                    For Each strFldName As String In strFld.Split(",")
                        Select Case intColCtr
                            Case 1
                                vsfFilter.SetData(vsfFilter.Rows.Count - 1, vFilFldCaption, strFldName)
                                vsfFilter.SetData(vsfFilter.Rows.Count - 1, vFilFldCode, strFldName)
                            Case 2
                                vsfFilter.SetData(vsfFilter.Rows.Count - 1, vFilTblName, strFldName)
                            Case 3
                                vsfFilter.SetData(vsfFilter.Rows.Count - 1, vFilFldCode, strFldName)
                                'vsfFilter.SetData(vsfFilter.Rows.Count - 1, vFilFldCaption, strFldName)
                            Case 4
                                vsfFilter.SetData(vsfFilter.Rows.Count - 1, vFilFldDataType, strFldName)
                                If strFldName = "DATE" Then
                                    vsfFilter.SetData(vsfFilter.Rows.Count - 1, vFilCol1, Format(gSettings.gCompStDt, "dd/MM/yyyy"))
                                    vsfFilter.SetData(vsfFilter.Rows.Count - 1, vFilCol2, Format(getSystemDate, "dd/MM/yyyy"))
                                    rg = vsfFilter.GetCellRange(vsfFilter.Rows.Count - 1, vFilCol1, vsfFilter.Rows.Count - 1, vFilCol2)
                                    rg.Style = vsfFilter.Styles("date")
                                End If

                            Case Else
                                Exit For
                        End Select


                        intColCtr = intColCtr + 1
                    Next
                Next

                'vsfFilter.Rows.Count = vsfFilter.Rows.Count + 1
                'vsfFilter.SetData(vsfFilter.Rows.Count - 1, vFilFldCode, rsTbl!FilterCols)
            End If
        Loop

        rsTbl.Close()

        With vsfFlds
            For i As Int32 = 1 To .Rows.Count - 1
                If .GetData(i, vFldName) = txtIDfld.Text Then
                    .Rows(i).Visible = False
                    .SetCellCheck(i, vFldSel, CheckEnum.Unchecked)
                End If
            Next
        End With

        showBtns("LOAD")
        btnDBData.PerformClick()

        cboDocName.Enabled = False
    End Sub

    Private Sub btnDesign_Click(sender As Object, e As EventArgs) Handles btnDesign.Click
        Dim argfrm As New frmUtlMstTrnBulkUpdateDesign
        frmMain.OpenForms(argfrm, "Design Screen")
    End Sub

    Private Sub btnTemplate_Click(sender As Object, e As EventArgs) Handles btnTemplate.Click
        Dim strSql As String, strIDfldName As String, strIDfldQry As String, strFname As String
        Dim rsItem As OleDb.OleDbDataReader

        blnGenTemplate = True
        Call retreiveFilters()
        Call loadData()
        blnGenTemplate = False

        dtTemp = clsSVT.getDATATable(strSqlTemp)
        dvTemp = New DataView(dtTemp)
        vsfTemp.DataSource = dvTemp

        SetHideCol(vsfTemp, True, 0)
        strFname = Application.StartupPath() & "\Temp\BulkImportTemplate" & Date.Now.ToString("yyyyMMddHHmmss") & ".xlsx"
        vsfTemp.SaveExcel(strFname, C1.Win.C1FlexGrid.FileFlags.AsDisplayed Or C1.Win.C1FlexGrid.FileFlags.IncludeFixedCells)

        Process.Start(strFname)

    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Call ClearFlds()
        showBtns("CANCEL")
    End Sub

    Private Sub btnDirUpload_Click(sender As Object, e As EventArgs) Handles btnDBData.Click
        blnDirUpdate = True
        Call retreiveFilters()
        Call loadData()
    End Sub

    Private Sub loadData()
        Dim strFldNames As String, strSql As String, rsFld As OleDb.OleDbDataReader
        Dim strTblName As String, strDispCol As String, strAdnlQry As String
        strTblName = ""
        strFldNames = ""
        strDispCol = ""

        With vsfFlds
            For i As Int32 = 1 To .Rows.Count - 1
                strAdnlQry = ""
                If .GetCellCheck(i, vFldDispSel) = CheckEnum.Checked And .GetCellCheck(i, vFldSel) = CheckEnum.Unchecked Then
                    If .GetData(i, vFldAdnlTbl) <> "" Then
                        strTblName = .GetData(i, vFldAdnlTbl)
                        strDispCol = .GetData(i, vFldAdnlDisp)
                        strAdnlQry = "(select " & strDispCol & " from " & strTblName & " where " & .GetData(i, vFldName) & "=" & txtTblName.Text & "." & .GetData(i, vFldName) & ") as " & .GetData(i, vFldName)
                        strFldNames = strFldNames & "," & strAdnlQry
                    Else
                        strFldNames = strFldNames & "," & .GetData(i, vFldName)
                    End If
                End If
            Next
            For i As Int32 = 1 To .Rows.Count - 1
                'strAdnlQry = ""
                If .GetCellCheck(i, vFldSel) = CheckEnum.Checked Then
                    If .GetData(i, vFldAdnlTbl) <> "" Then
                        strTblName = .GetData(i, vFldAdnlTbl)
                        strDispCol = .GetData(i, vFldAdnlDisp)
                        strAdnlQry = "(select " & strDispCol & " from " & strTblName & " where " & .GetData(i, vFldName) & "=" & txtTblName.Text & "." & .GetData(i, vFldName) & ") as " & .GetData(i, vFldName)
                        strFldNames = strFldNames & "," & strAdnlQry
                    Else
                        strFldNames = strFldNames & "," & .GetData(i, vFldName)
                    End If
                End If
            Next
        End With

        If strFldNames = "" Then
            MsgBox("Please select atleast one field!", MsgBoxStyle.Information, Me.Text)
            Exit Sub
        End If

        'strFldNames = Strings.Right(strFldNames, Len(strFldNames) - 1)

        '& txtIDfld.Text
        strSql = "Select '' as  FieldName" & strFldNames & " from " & txtTblName.Text & " where " & txtIDfld.Text & "<>0"
        If strCond <> "" Then
            strSql &= strCond
        End If
        '& txtIDfld.Text
        If blnGenTemplate Then
            strSqlTemp = "Select " & Strings.Right(strFldNames, Len(strFldNames) - 1) & " from " & txtTblName.Text & " where " & txtIDfld.Text & "<>0"
            If strCond <> "" Then
                strSqlTemp &= strCond
            End If
            Exit Sub
        End If

        rsFld = clsSVT.getDataReaderFromSQL(strSql)
        dtFlds = clsSVT.getDATATable(strSql)
        dvFlds = New DataView(dtFlds)
        vsfDet.DataSource = dvFlds

        rsFld.Close()

        vsfDet.Cols(1).Visible = False
        With vsfDet

            .SetData(vDetRowFldName, 0, "Field Name")
            .SetData(vDetRowDatatype, 0, "Data Type")
            .SetData(vDetRowForKey, 0, "Foreign Key")
            .SetData(vDetRowMaxLen, 0, "MaxLength")
            .SetData(vDetRowUpdate, 0, "Update Fld")

        End With

        Dim strName As String
        With vsfDet
            For i As Int32 = 2 To .Cols.Count - 1
                strName = .GetData(0, i)
                For j = 1 To vsfFlds.Rows.Count - 1

                    If strName = vsfFlds.GetData(j, vFldName) Then
                        If vsfFlds.GetData(j, vFldDataType) <> "" Then
                            .SetData(vDetRowDatatype, i, vsfFlds.GetData(j, vFldDataType))
                            If vsfFlds.GetData(j, vFldAdnlTbl) <> "" And vsfFlds.GetData(j, vFldAdnlDisp) <> "" Then
                                .SetData(vDetRowForKey, i, vsfFlds.GetData(j, vFldAdnlTbl) & "|" & vsfFlds.GetData(j, vFldAdnlDisp))
                            End If
                        End If
                        If vsfFlds.GetCellCheck(j, vFldSel) = CheckEnum.Checked Then
                            .SetData(vDetRowUpdate, i, "Y")
                        Else
                            .SetData(vDetRowUpdate, i, "N")
                        End If
                    End If
                Next
            Next

            For x As Int32 = 0 To .Cols.Count - 1
                If x > 0 Then
                    vsfDet.SetData(3, x, clsSVT.getDataLongValue("select character_maximum_length From INFORMATION_SCHEMA.COLUMNS Where table_name = '" & txtTblName.Text & "' and column_name='" & .GetData(vDetRowFldName, x) & "'", 0, False))
                End If
                If .GetData(vDetRowUpdate, x) = "N" Or .GetData(vDetRowUpdate, x) = "Update Fld" Then
                    For r As Int32 = 5 To .Rows.Count - 1
                        setVsfCellColour(vsfDet, r, x, Color.LightGray, argBorderClr:=Color.Black)
                    Next
                End If
            Next
        End With
        btnSave.Visible = True
        lblTotNoOfRec.Text = "Total Number of Records Loaded : " & vsfDet.Rows.Count - 5

    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        If Not ValidateEntry() Then Exit Sub

        svtTrn = svtConn.BeginTransaction(IsolationLevel.ReadCommitted)
        Try
            Dim strSql As String
            With vsfDet

                For i = 5 To .Rows.Count - 1
                    For j = 2 To .Cols.Count - 1

                        If .GetData(vDetRowUpdate, j) = "N" Then GoTo nxt

                        strSql = " Update " & txtTblName.Text & " Set "

                        Select Case UCase(Trim(.GetData(vDetRowDatatype, j)))
                            Case "NUMERIC"

                                If .GetData(vDetRowForKey, j) <> "" Then
                                    Dim strSql1 As String
                                    Dim strID As String
                                    Dim strArr() As String
                                    strID = .GetData(vDetRowFldName, j)
                                    strArr = UCase(Trim(.GetData(vDetRowForKey, j))).Split("|")
                                    strSql1 = "select " & strID & " from " & strArr(0)
                                    strSql1 = strSql1 & " where " & strArr(1) & " = '" & .GetData(i, j) & "'"

                                    strSql = strSql & .GetData(vDetRowFldName, j) & "=" & clsSVT.getDataLongValue(strSql1, 0, False, svtTrn)

                                Else
                                    strSql = strSql & .GetData(vDetRowFldName, j) & "=" & GetVal(Val(.GetData(i, j)), 0)
                                End If

                            Case "TEXT"
                                If GetVal(.GetData(i, j), "") <> "" Then
                                    strSql = strSql & .GetData(vDetRowFldName, j) & "='" & GetVal(.GetData(i, j), "") & "'"
                                End If

                            Case "DATE"
                                If IsDate(.GetData(i, j)) Then
                                    strSql = strSql & .GetData(vDetRowFldName, j) & "='" & getDateFormatInInsert(.GetData(i, j)) & "'"
                                Else
                                    strSql = strSql & .GetData(vDetRowFldName, j) & "='Null'"
                                End If
                        End Select

                        If Strings.Right(Trim(strSql), 3) = "Set" Then

                        Else
                            strSql = strSql & " where " & .GetData(vDetRowFldName, 1) & " = " & Val(.GetData(i, 1))

                            If Not clsSVT.execSQLStatement(strSql, svtTrn) Then
                                svtTrn.Rollback()
                                Exit Sub
                            End If
                        End If

nxt:

                    Next
                Next
            End With

            svtTrn.Commit()

            MsgBox(vsfDet.Rows.Count - 5 & " Records Updated Successfully!", MsgBoxStyle.Information, Me.Text)

            btnCancel.PerformClick()

        Catch ex As Exception
            svtTrn.Rollback()
            MsgBox("Error while importing..." & vbNewLine & ex.Message, MsgBoxStyle.Critical, Me.Text)
            Exit Sub

        End Try

    End Sub

    Private Sub vsfFilter_CellButtonClick(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfFilter.CellButtonClick
        Dim strSQL As String, strTbName As String

        With vsfFilter
            'show the selection form From respective master
            Dim frmSel As New frmUtlMultiSelection
            If .GetData(e.Row, vFilTblName) = "" Then
                strTbName = txtTblName.Text
            Else
                strTbName = .GetData(e.Row, vFilTblName)
            End If
            If .GetData(e.Row, vFilFldCode) <> "" Then
                strSQL = "Select distinct 0 as Sel, " & .GetData(e.Row, vFilFldCode) & " from " & strTbName & " order by 1"
            Else
                strSQL = "Select distinct 0 as Sel, " & .GetData(e.Row, vFilFldCaption) & " from " & strTbName & " order by 1"
            End If
            If .GetData(e.Row, vFilFldCode) <> "" And .GetData(e.Row, vFilFldCaption) <> "" Then
                strSQL = "Select distinct 0 as Sel, " & .GetData(e.Row, vFilFldCaption) & ", " & .GetData(e.Row, vFilFldCode) & " from " & strTbName & " order by 1"
            End If
            strList = ""
            vsfRetGrid.Rows.Count = 1
            If .GetData(.Row, vFilFldDataType) = "NUMERIC" Then
                Call frmSel.InitVsfSel(strSQL, -1, 1, True, Me, True)
            Else
                Call frmSel.InitVsfSel(strSQL, -1, 1, False, Me, True)
            End If
            frmSel.Text = "Select [Multiple]" & .GetData(e.Row, vFilFldCaption)
            frmSel.StartPosition = FormStartPosition.CenterScreen
            If .GetData(e.Row, vFilFldCode) <> "" And .GetData(e.Row, vFilFldCaption) <> "" Then
                frmSel.vsfDet.Cols(1).Visible = False
            End If
            frmSel.ShowDialog()
            Call FillListInSelectedGrid(.GetData(.Row, vFilFldCode), .GetData(.Row, vFilFldCaption), .GetData(.Row, vFilFldDataType), strTbName)
        End With
    End Sub

    Private Sub FillListInSelectedGrid(ByVal argFldCode As String, ByVal argFldCaption As String, ByVal argDataType As String, ByVal argFldTblName As String)
        Dim i As Integer, blnAdded As Boolean, blnFound As Boolean
        With vsfSelFilters
            'add from the grid which are selected
            blnAdded = False
            blnFound = False
            For i = 1 To vsfRetGrid.Rows.Count - 1
                If vsfRetGrid.GetCellCheck(i, 0) = C1.Win.C1FlexGrid.CheckEnum.Checked Then
                    For x = 1 To .Rows.Count - 1
                        If .GetData(x, vSelFilFldCode) = argFldCode And Replace(.GetData(x, vSelFldFilterValue), "'", "") = vsfRetGrid.GetData(i, 1) Then
                            blnFound = True
                            blnAdded = True
                            Exit For
                        End If
                    Next

                    If Not blnFound Then
                        blnAdded = True
                        .Rows.Count = .Rows.Count + 1
                        .SetCellImage(.Rows.Count - 1, vSelFldCaption, My.Resources.cancel)
                        .SetData(.Rows.Count - 1, vSelFilFldCode, argFldCode)
                        .SetData(.Rows.Count - 1, vSelFilTblName, argFldTblName)
                        .SetData(.Rows.Count - 1, vSelFldCaption, argFldCaption)
                        If argDataType = "NUMERIC" Then
                            .SetData(.Rows.Count - 1, vSelFldFilterValue, vsfRetGrid.GetData(i, 1))
                        Else
                            .SetData(.Rows.Count - 1, vSelFldFilterValue, "'" & vsfRetGrid.GetData(i, 1) & "'")
                        End If
                    End If
                End If
            Next i

            .AutoSizeRows()
            Dim order As C1.Win.C1FlexGrid.SortFlags = C1.Win.C1FlexGrid.SortFlags.Ascending
            .Sort(order, vSelFilFldCode)
            If .Rows.Count > 1 Then
                btnRemFilter.Enabled = True
                btnClearFilters.Enabled = True
                btnViewReport.Enabled = True
                pnlFilter.Visible = False
            Else
                btnRemFilter.Enabled = False
                btnClearFilters.Enabled = False
                btnViewReport.Enabled = True
                pnlFilter.Visible = True
            End If
        End With
    End Sub

    Private Sub btnSelect_Click(sender As Object, e As EventArgs) Handles btnSelect.Click

        Dim dialog As New OpenFileDialog
        Dim strFilename As String

        If dialog.ShowDialog() = DialogResult.OK Then
            strFilename = dialog.FileName
        Else
            Exit Sub
        End If
        If Not File.Exists(strFilename) Or strFilename = "" Then
            MsgBox("Please select a valid file for importing", MsgBoxStyle.Critical, "Error")
            Exit Sub
        End If
        txtSelect.Text = strFilename

        showWaitCursor()
        Call importData(strFilename)
        showNormalCursor()

    End Sub

    Private Sub importData(argFilename As String)
        vsfDet.DataSource = Nothing
        With vsfDet
            .Cols.Count = 0
            .Cols.Count = 1
            .Rows.Count = 1
            .Rows.Count = 5
            .Rows.Fixed = 5

            .SetData(vDetRowFldName, 0, "Field Name")
            .SetData(vDetRowDatatype, 0, "Data Type")
            .SetData(vDetRowForKey, 0, "Foreign Key")
            .SetData(vDetRowMaxLen, 0, "MaxLength")
            .SetData(vDetRowUpdate, 0, "Update Fld")

        End With

        'SetFlexColor(vsfDet, "", 0) 'vXlSlNo

        Dim xlapp As New Excel.Application
        Dim i As Integer, intRow As Integer, intChar As Integer = 67, intCol As Integer, c As Integer
        Dim strs As String
        Dim strTemplateStrings As String = ""

        strTemplateStrings = ""

        xlapp.Workbooks.Open(argFilename, , True)
        With xlapp.Application
            strs = ""

            strTemplateStrings = .Range("A1").Value

            If InStr(strTemplateStrings.ToUpper, "ID") = 0 Then
                MsgBox("Please Select a valid Template!", MsgBoxStyle.Information, Me.Text)
                Exit Sub
            End If

            'vsfDet.Rows.Count = 1

            i = 1
            Dim blnRunLoop As Boolean = True
            Try
                Do Until .Range(Chr(64 + i) & "1").Value = ""
                    vsfDet.Cols.Count = vsfDet.Cols.Count + 1
                    vsfDet.SetData(0, vsfDet.Cols.Count - 1, .Range(Chr(64 + i) & "1").Value)
                    For x As Int32 = 1 To vsfFlds.Rows.Count - 1
                        If vsfFlds.GetData(x, vFldName) = .Range(Chr(64 + i) & "1").Value Then
                            vsfDet.SetData(vDetRowDatatype, vsfDet.Cols.Count - 1, vsfFlds.GetData(x, vFldDataType))
                            If vsfFlds.GetData(x, vFldAdnlTbl) <> "" Then
                                vsfDet.SetData(vDetRowForKey, vsfDet.Cols.Count - 1, vsfFlds.GetData(x, vFldAdnlTbl) & "|" & vsfFlds.GetData(x, vFldAdnlDisp))
                            End If
                            vsfDet.SetData(vDetRowMaxLen, vsfDet.Cols.Count - 1, clsSVT.getDataLongValue("select character_maximum_length From INFORMATION_SCHEMA.COLUMNS Where table_name = '" & txtTblName.Text & "' and column_name='" & vsfFlds.GetData(x, vFldName) & "'", 0, False))

                            If vsfFlds.GetCellCheck(x, vFldSel) = CheckEnum.Checked Then
                                vsfDet.SetData(vDetRowUpdate, vsfDet.Cols.Count - 1, "Y")
                            Else
                                vsfDet.SetData(vDetRowUpdate, vsfDet.Cols.Count - 1, "N")
                            End If
                        End If
                    Next
                    i = i + 1
                Loop

                i = 2
                intChar = 63

                'Do While Not IsDBNull(.Range(Chr(intChar + c) & i).Value)
                Do Until .Range("B" & i).Value = ""
                    intCol = 1
                    'For a As Int32 = 0 To vsfDet.Cols.Count - 1
                    '    If vsfDet.GetData(0, a) = GetVal(.Range(Chr(intChar + c) & "1").Value, "") Then
                    '        intCol = a
                    '        Exit For
                    '    End If
                    'Next
                    'If intCol = -1 Then
                    '    vsfDet.Cols.Count = vsfDet.Cols.Count + 1
                    '    intCol = vsfDet.Cols.Count - 1
                    '    vsfDet.SetData(0, intCol, GetVal(.Range(Chr(intChar + c) & "1").Value, ""))
                    'End If

                    vsfDet.Rows.Count = vsfDet.Rows.Count + 1
                    intRow = vsfDet.Rows.Count - 1
                    c = 2
                    Do While Not IsDBNull(.Range(Chr(intChar + c) & i).Value)
                        If intRow = vsfDet.Rows.Count - 1 And intCol <= vsfDet.Cols.Count - 1 Then
                            vsfDet.SetData(vsfDet.Rows.Count - 1, intCol, .Range(Chr(intChar + c) & i).Value)
                            'Exit Do
                        Else
                            Exit Do
                        End If
                        c = c + 1
                        intCol = intCol + 1
                    Loop
                    i = i + 1
                Loop

                .Workbooks(1).Close(SaveChanges:=False)
            Catch ex As Exception
                MsgBox("Cannot export to Excel." & vbCrLf & ex.Message, MsgBoxStyle.Critical, "Import Error")
            End Try
        End With
        vsfDet.Cols(1).Visible = False
        ValidateEntry()

    End Sub

    Private Sub btnRemFilter_Click(sender As Object, e As EventArgs) Handles btnRemFilter.Click
        'If Not chkUserRights(strRights, chkRightsPos.EditRights, Me.Text) Then Exit Sub
        With vsfSelFilters
            If .Row <= 0 Then Exit Sub
            .RemoveItem(.Row)
            If .Rows.Count = 1 Then
                btnRemFilter.Enabled = False
                btnClearFilters.Enabled = False
                btnViewReport.Enabled = False
                pnlFilter.Visible = True
            End If
        End With
    End Sub

    Private Sub btnClearFilters_Click(sender As Object, e As EventArgs) Handles btnClearFilters.Click
        'If Not chkUserRights(strRights, chkRightsPos.EditRights, Me.Text) Then Exit Sub
        'Dim strCodeName As String, i As Integer
        With vsfSelFilters
            .Rows.Count = 1
            btnRemFilter.Enabled = False
            btnClearFilters.Enabled = False
            btnViewReport.Enabled = False
            pnlFilter.Visible = True
        End With
    End Sub

    Private Sub btnViewReport_Click(sender As Object, e As EventArgs) Handles btnViewReport.Click
        Call retreiveFilters()
        Call loadData()
    End Sub

    Private Sub retreiveFilters()
        Dim intCtr As String, strFMT As String = "", blnFound As Boolean
        Dim strCodeName As String, strFldList As String, strCodeCap As String
        Dim strSQL As String, strOrderBy As String, strTbls As String
        Dim rsDummy As OleDb.OleDbDataReader
        Dim strGroupBy As String = "", i As Integer

        strCond = ""

        With vsfFilter
            For i = 1 To .Rows.Count - 1
                If .GetData(i, vFilFldDataType) = "DATE" Then
                    strCond = strCond & " and " & txtTblName.Text & "." & .GetData(i, vFilFldCaption) &
                            " between " & getDateFormatInCond(.GetData(i, vFilCol1)) &
                            " and " & getDateFormatInCond(.GetData(i, vFilCol2))
                    'ElseIf .GetData(i, vFilFldDataType) = "TEXT" Then
                    '    strCond = strCond & " and " & .GetData(i, vFilTblName) & "." & .GetData(i, vFilFldCode) &
                    '    strCond = strCond & "'" & Trim(.GetData(i, vFilCol1)) & "'"
                End If
            Next
        End With

        With vsfSelFilters
            Do While True
                blnFound = False
                strCodeName = ""
                strCodeCap = ""
                For i = 1 To .Rows.Count - 1
                    If .GetData(i, vSelFilProcessed) = "" Then
                        blnFound = True
                        strCodeName = .GetData(i, vSelFilFldCode)
                        strCodeCap = .GetData(i, vSelFldCaption)
                        strTblName = .GetData(i, vSelFilTblName)
                        strCondTblName = .GetData(i, vSelFilTblName)
                        Exit For
                    End If
                Next
                If strCodeName = "" And strCodeCap = "" Then Exit Do
                strFldList = ""
                For i = 1 To .Rows.Count - 1
                    If strCodeName = .GetData(i, vSelFilFldCode) Then
                        strFldList = strFldList & .GetData(i, vSelFldFilterValue) & ","
                        .SetData(i, vSelFilProcessed, "Y")
                        'strFldList = Replace(strFldList, "'", "")
                    ElseIf strCodeName = .GetData(i, vSelFldCaption) Then
                        strFldList = strFldList & .GetData(i, vSelFldFilterValue) & ","
                        .SetData(i, vSelFilProcessed, "Y")
                    End If
                Next
                If strFldList <> "" Then
                    strFldList = Mid(strFldList, 1, Len(strFldList) - 1)
                    'strCond = strCond & " and " & strTblName & "." & strCodeName & " in (" & strFldList & ")"
                    strCond = strCond & " and " & txtTblName.Text & "." & strCodeCap & " in (" & strFldList & ")"
                    'strCond = strCond & " and " & strCodeName & " in (" & strFldList & ")"
                End If
            Loop

            'reset processed for next run
            For i = 1 To .Rows.Count - 1
                .SetData(i, vSelFilProcessed, "")
            Next
        End With

    End Sub

    Private Sub vsfFilter_BeforeEdit(sender As Object, e As RowColEventArgs) Handles vsfFilter.BeforeEdit

        If e.Col = vFilBtn And vsfFilter.GetData(e.Row, vFilFldDataType) = "DATE" Then
            e.Cancel = True
            Exit Sub
        End If
        If e.Col = vFilCol1 And vsfFilter.GetData(e.Row, vFilFldDataType) <> "DATE" Then
            e.Cancel = True
            Exit Sub
        End If
        If e.Col = vFilCol2 And vsfFilter.GetData(e.Row, vFilFldDataType) <> "DATE" Then
            e.Cancel = True
            Exit Sub
        End If
        If e.Col <> vFilCol1 And e.Col <> vFilCol2 And e.Col <> vFilBtn Then
            e.Cancel = True
            Exit Sub
        End If

    End Sub

    Private Sub vsfDet_BeforeEdit(sender As Object, e As RowColEventArgs) Handles vsfDet.BeforeEdit
        e.Cancel = True

        If vsfDet.Col <= 1 Then
            e.Cancel = True
            Exit Sub
        End If

        With vsfDet
            If .GetData(vDetRowUpdate, .Col) = "Y" Then
                e.Cancel = False
            End If
        End With
    End Sub

    Private Function ValidateEntry() As Boolean
        Dim i As Integer, strerr As String = "", blnfound As Boolean, intXLCols As Integer = 2
        Dim pattern As String = "[0-9]"

        blnValidData = False

        ValidateEntry = False


        With vsfDet
            If vsfDet.Rows.Count < 2 Then
                MsgBox("There is no data exists for importing", vbInformation + vbOKOnly, Me.Text)
                Exit Function
            End If

            For i = 5 To .Rows.Count - 1
                .TopRow = i
                'DoEvents()
                strerr = "" 'Refreshing the err messg - each time
                'vsfDet.SetData(i, vXLColBlnErr, strerr)
                ' vsfDet.SetData(i, vXLColErrDesc, "")

                For j = intXLCols To .Cols.Count - 1

                    'If UCase(Trim(.GetData(vXLRowUnique, j))) = "Y" Then
                    '    clsSVT.getDataStringValue("select " & UCase(Trim(.GetData(vXLRowFldName, j))) & " from " & strHdrTblName & " where " & UCase(Trim(.GetData(vXLRowFldName, j))) & " = '" & UCase(Trim(.GetData(i, j))) & "'", "", blnfound, svtTrn)
                    '    If blnfound Then
                    '        'strerr = Replace(strHdrTblName, "MST", " ") & "CODE" & " already exists."
                    '        strerr = UCase(Trim(.GetData(vXLRowFldName, j))) & " already exists ;"
                    '        vsfDet.SetData(i, vXLColErrDesc, strerr)
                    '    End If
                    '    '
                    'End If
                    'If UCase(Trim(.GetData(vXLRowMandatory, j))) = "Y" Then
                    '    If UCase(Trim(vsfdet.GetData(i, j))) = "" Then
                    '        ' If blnfound Then
                    '        'strerr = Replace(strHdrTblName, "MST", " ") & "CODE" & " already exists."
                    '        strerr = UCase(Trim(.GetData(vXLRowFldName, j))) & " is Mandatory."
                    '        vsfdet.SetData(i, vXLColErrDesc, strerr)
                    '    End If

                    'End If

                    Select Case UCase(Trim(.GetData(vDetRowDatatype, j)))
                        Case "TEXT"

                            'If UCase(Trim(.GetData(vXLRowMandatory, j))) = "Y" Then
                            '    If UCase(Trim(.GetData(i, j))) = "" AndAlso GetVal(.GetData(vXLRowDefaultVal, j), "") = "" Then
                            '        If strerr <> "" Then
                            '            strerr = strerr & " Mandatory Field should not be empty.;"
                            '        Else
                            '            strerr = " Mandatory Field should not be empty.;"
                            '        End If
                            '        vsfDet.SetData(i, vXLColErrDesc, strerr)
                            '    End If
                            'End If

                            If Not IsDBNull(.GetData(i, j)) Then
                                If Trim(.GetData(i, j)) = "" Then

                                    .Col = j
                                    .Row = i
                                    .Focus()
                                    MessageBox.Show(" Text Data Should not be blank")
                                    Exit Function
                                End If

                                If Len(UCase(Trim(.GetData(i, j)))) > Val(Trim(.GetData(vDetRowMaxLen, j))) Then
                                    If strerr <> "" Then
                                        strerr = strerr & " MaxLength of Field " & UCase(Trim(.GetData(vDetRowFldName, j))) & " should not exceed.;"
                                    Else
                                        strerr = " MaxLength of Field " & UCase(Trim(.GetData(vDetRowFldName, j))) & " should not exceed.;"
                                    End If
                                    'vsfDet.SetData(i, vXLColErrDesc, strerr)
                                End If
                            End If
                            If UCase(Trim(.GetData(vDetRowForKey, j))) <> "" Then
                                Dim strArr() As String
                                strArr = UCase(Trim(.GetData(vDetRowForKey, j))).Split("|")
                                'clsSVT.getDataLongValue("select " & strArr(1) & " from " & strArr(0) & " where " & strArr(2) & " = '" & UCase(Trim(.GetData(i, j))) & "'", 0, blnfound, svtTrn)
                                If Not blnfound Then
                                    If strerr <> "" Then
                                        strerr = strerr & strArr(2) & " not found in the Masters.;"
                                    Else
                                        strerr = strArr(2) & " not found in the Masters.;"
                                    End If
                                    'vsfDet.SetData(i, vXLColErrDesc, strerr)
                                End If
                            End If

                        Case "NUMERIC"
                            Dim output As Integer
                            Dim strSQL As String
                            Dim strID As String
                            'If UCase(Trim(.GetData(vXLRowMandatory, j))) = "Y" Then
                            '    If UCase(Trim(.GetData(i, j))) = "" Then
                            '        If strerr <> "" Then
                            '            strerr = strerr & "Mandatory Field should not be empty.;"
                            '        Else
                            '            strerr = " Mandatory Field should not be empty.;"
                            '        End If
                            '        vsfDet.SetData(i, vXLColErrDesc, strerr)
                            '    End If
                            'End If
                            If UCase(Trim(.GetData(vDetRowForKey, j))) <> "" Then
                                If Not IsDBNull(.GetData(i, j)) Then
                                    Dim str As String = .GetData(i, j)
                                    If .GetData(i, j) <> "-" Then
                                        Dim strArr() As String
                                        strID = .GetData(0, j, vDetRowFldName)
                                        strArr = UCase(Trim(.GetData(vDetRowForKey, j))).Split("|")
                                        strSQL = "select " & strID & " from " & strArr(0) & " where " & strArr(1) & " = '"
                                        strSQL = strSQL & .GetData(i, j) & "'"


                                        If clsSVT.getDataLongValue(strSQL, 0, False) = 0 Then
                                            .Col = j
                                            .Row = i
                                            .Focus()
                                            MsgBox(Replace(.GetData(vDetRowFldName, j), "ID", "") & " Not Found In Table", MsgBoxStyle.Information, Me.Text)
                                            Exit Function
                                        End If
                                    End If
                                End If
                            Else
                                Dim match As System.Text.RegularExpressions.Match = Regex.Match(.GetData(i, j), pattern)
                                If match.Success = False Then

                                    .Col = j
                                    .Row = i
                                    .Focus()
                                    MessageBox.Show("Please enter a valid Data ", "Checking")
                                    Exit Function

                                End If


                            End If
                            If Trim(.GetData(i, j)) = "" Then

                                .Col = j
                                .Row = i
                                .Focus()
                                MessageBox.Show(" Numeric Data Should not be blank")
                                Exit Function
                            End If


                            If InStr(.GetData(vDetRowMaxLen, j), ",") > 0 Then
                                If Not Double.TryParse(.GetData(i, j), output) Then
                                    If strerr <> "" Then
                                        strerr = strerr & " Please enter the " & .GetData(vDetRowFldName, j) & " value according to the Maxlength.;"
                                    Else
                                        strerr = " Please enter the " & .GetData(vDetRowFldName, j) & " value according to the Maxlength.;"
                                    End If
                                    ' vsfDet.SetData(i, vXLColErrDesc, strerr)
                                End If
                            Else
                                If Integer.TryParse(.GetData(i, j), output) Then
                                    'If Len(Val(.GetData(i, j))) > Val(Trim(.GetData(vDetRowMaxLen, j))) Then
                                    '    If strerr <> "" Then
                                    '        strerr = strerr & " MaxLength of Field " & UCase(Trim(.GetData(vDetRowFldName, j))) & " should not exceed.;"
                                    '    Else
                                    '        strerr = " MaxLength of Field " & UCase(Trim(.GetData(vDetRowFldName, j))) & " should not exceed.;"
                                    '    End If
                                    '    ' vsfDet.SetData(i, vXLColErrDesc, strerr)
                                    'End If
                                End If
                            End If

                        Case "DATE"
                            'If Not Date.TryParse(.GetData(i, j), "DD/MM/YYYY") Then
                            '    If strerr <> "" Then
                            '        strerr = strerr & "Please enter the valid date"
                            '    Else
                            '        strerr = "Please enter the valid date"
                            '    End If
                            'vsfDet.SetData(i, vXLColErrDesc, strerr)
                            ' End If
                    End Select
                Next j
                'lblStatus.Text = "Validating " & vsfdet.GetData(i, intXLRows + 1) & " -- " & i & " / " & txtNoOfInv.Text

            Next i


        End With

        'For i = 1 To vsfDet.Rows.Count - 1
        '    'If vsfDet.GetData(i, vXLColErrDesc) <> "" Then
        '    '    vsfDet.SetData(i, vXLColBlnErr, "Y")
        '    'End If
        'Next
        blnValidData = False
        Dim csRed As CellStyle = vsfDet.Styles.Add("RED")
        csRed.BackColor = Color.Red
        For i = 1 To vsfDet.Rows.Count - 1
            'If Trim(vsfDet.GetData(i, vXLColBlnErr)) = "Y" Then
            '    Dim rgRed As CellRange = vsfDet.GetCellRange(i, vXLColRowName)
            '    rgRed.Style = vsfDet.Styles("RED")
            '    blnValidData = True
            'End If
        Next
        'With vsfdet
        '    For i = intXLRows To .Rows.Count - 1
        '        If ChkXLDuplicateData(vsfdet, i, intXLCols) Then
        '            strerr = strerr & "Duplicate " & UCase(Trim(.GetData(vXLRowFldName, intXLCols)))
        '            vsfdet.SetData(intXLRows, vXLColErrDesc, strerr)

        '            Exit For
        '        End If
        '    Next i
        'End With
        If blnValidData = False Then
            btnSave.Visible = True
            ValidateEntry = True
        Else
            btnSave.Visible = False
        End If
    End Function

    Private Sub vsfDet_KeyUp(sender As Object, e As KeyEventArgs) Handles vsfDet.KeyUp
        ' ValidateEntry()
    End Sub

    Private Sub vsfDet_Validated(sender As Object, e As EventArgs) Handles vsfDet.Validated
        'ValidateEntry()
    End Sub

    Private Sub vsfDet_ValidateEdit(sender As Object, e As ValidateEditEventArgs) Handles vsfDet.ValidateEdit
        ' ValidateEntry()
    End Sub

    Private Sub vsfDet_AfterEdit(sender As Object, e As RowColEventArgs) Handles vsfDet.AfterEdit

    End Sub

    Private Sub vsfDet_KeyPress(sender As Object, e As KeyPressEventArgs) Handles vsfDet.KeyPress
        ' ValidateEntry()
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        frmMain.CloseSelectedTabPage(Me)
    End Sub

    Private Sub vsfDet_KeyPressEdit(sender As Object, e As KeyPressEditEventArgs) Handles vsfDet.KeyPressEdit
        Select Case GetVal(vsfDet.GetData(vDetRowDatatype, e.Col))
            Case "NUMERIC"
                If vsfDet.GetData(vDetRowForKey, e.Col) <> "" Then Exit Sub
                If Not KeyValidate("Decimal", e.KeyChar) Then
                    e.Handled = True
                    Exit Sub
                End If
        End Select
    End Sub

    Private Sub frmUtlMstTrnBulkUpdate_KeyUp(sender As Object, e As KeyEventArgs) Handles Me.KeyUp
        If e.KeyCode = Keys.F9 And e.Control = True Then
            Call initFormVsfDet(Me, Me.Text, vsfFlds, "FLDS")
            Call initFormVsfDet(Me, Me.Text, vsfFilter, "FILTER")
            Call initFormVsfDet(Me, Me.Text, vsfSelFilters, "SELFILTER")
            MsgBox("Restored Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
        If e.KeyCode = Keys.F8 And e.Control = True Then
            Call SaveActFormSettings(Me, Me.Text)
            Call SaveActFormVsfDet(Me, Me.Text, "FLDS", vsfFlds)
            'Call SaveActFormVsfDet(Me, Me.Text, "FILTER", vsfFilter)
            'Call SaveActFormVsfDet(Me, Me.Text, "SELFILTER", vsfSelFilters)
            MsgBox("Saved Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
    End Sub

    Private Sub btnExcel_Click(sender As Object, e As EventArgs) Handles btnExcel.Click
        'Call ExportToXL(vsfDet)
        btnTemplate.PerformClick()
    End Sub

    Private Sub showBtns(argType As String)
        btnLoad.Visible = True
        btnCancel.Visible = True
        btnDesign.Visible = True
        lblLegend.Visible = True
        btnViewReport.Enabled = False
        btnReplRest.Visible = False

        Select Case UCase(argType)
            Case "LOAD"
                btnCancel.Visible = True
                btnLoad.Visible = False
                btnDesign.Visible = False
                lblLegend.Visible = False
                btnReplRest.Visible = True

            Case "CANCEL"
                btnCancel.Visible = False
                btnLoad.Visible = True
                btnDesign.Visible = True
                lblLegend.Visible = True

            Case "DESIGN"

            Case Else

        End Select
    End Sub

    Private Sub vsfSelFilters_MouseClick(sender As Object, e As MouseEventArgs) Handles vsfSelFilters.MouseClick
        Dim ht = vsfSelFilters.HitTest

        With vsfSelFilters
            If ht.Row < 1 Then Exit Sub
            If .GetData(ht.Row, vSelFldCaption) = "" Then Exit Sub
            If ht.Column <> vSelFldCaption Then Exit Sub
            .RemoveItem(ht.Row)

            If .Rows.Count = 1 Then
                btnRemFilter.Enabled = False
                btnClearFilters.Enabled = False
                btnViewReport.Enabled = False
                pnlFilter.Visible = True
            End If

        End With
    End Sub

    Private Sub cboDocName_KeyUp(sender As Object, e As KeyEventArgs) Handles cboDocName.KeyUp
        If e.KeyCode = Keys.F5 Then
            bindCombo(cboDocName, "DocName", "DocName", "Select DocName From UtlMstTrnBulkUpdate Order by 1")
        End If
    End Sub

    Private Sub btnReplRest_Click(sender As Object, e As EventArgs) Handles btnReplRest.Click
        Dim strTxt As String

        With vsfDet
            strTxt = GetVal(.GetData(.Row, .Col), "")
            If strTxt = "" Then GetVal(.Editor.Text, "")
            For i = .Row To .Rows.Count - 1
                .SetData(i, .Col, strTxt)
            Next i
        End With

    End Sub

    '    Dim clsSVT As New svtLibrary
    '    Dim mode As String
    '    Dim strTable As String
    '    Dim intTotNoOfRec As String
    '    Dim strDataType As Type

    '    Dim dt As DataTable
    '    Dim dv As DataView
    '    Dim tooltip As New ToolTip


    '    Private Sub frmUtlBulkUpdate_Load(sender As Object, e As EventArgs) Handles Me.Load
    '        Dim strSQL As String
    '        Call initScreen()
    '        strSQL = "Select Name from Sys.Tables where (upper(Name) like'MST%' or upper(Name) like'TRN%') "
    '        strSQL = strSQL & " and upper(Name) not in ('MSTMENU','MSTCOMP','MSTCOMPFINYEAR','MSTROLE','MSTROLERIGHTS','MSTSTATE',"
    '        strSQL = strSQL & " 'MSTUDRRIGHTS''MSTUSER','MSTUSERRIGHTS','TRNQOH''TRNSTKFLOW','TRNSTOCK') "
    '        strSQL = strSQL & " order by Name "
    '        Call bindCombo(cboTable, "Name", "Name", strSQL)
    '        cboTable.Text = ""
    '        cboTable.Focus()
    '        tooltip.SetToolTip(Me.btnFreeze, "Freeze : Alt + F")
    '        'Call SetHideCol(vsfDet, True, 3, 4, 5, 6)
    '    End Sub

    '    Private Sub initScreen()
    '        With vsfDet
    '            .Rows.Count = 1
    '        End With
    '        Me.KeyPreview = True
    '        Call initFormVsfDet(Me, Me.Text, vsfDet, "DET")
    '        cboTable.SelectedIndex = -1
    '        initGridStyles(Me)
    '        fillFormProperties(Me)
    '        txtNoofRec.Text = ""
    '        GrpUpdate.Visible = False
    '        pnlExp.Visible = False
    '        GrpDelete.Visible = False
    '    End Sub

    '    Private Sub btnShowRec_Click(sender As Object, e As EventArgs)
    '        If cboTable.Text = "" Then Exit Sub
    '        Dim i As Integer, blnFound As Boolean
    '        blnFound = False
    '        For i = 1 To cboTable.Items.Count - 1
    '            If UCase(cboTable.GetItemText(cboTable.Items(i))) = UCase(cboTable.Text) Then
    '                blnFound = True
    '                Exit For
    '            End If
    '        Next
    '        If Not blnFound Then
    '            MsgBox("Please select a table from the drop down!", MsgBoxStyle.Critical, "ERROR")
    '            Exit Sub
    '        End If
    '        Call fillData()
    '        txtReplaceValue.Text = ""
    '        txtCurValue.Text = ""
    '        txtFields.Text = ""

    '    End Sub
    '    Public Sub fillData()

    '        Dim strSQL As String


    '        strSQL = " select top 1000 * from " & cboTable.Text & " where 1=1"



    '        Call showWaitCursor()
    '        With vsfDet
    '            .BeginUpdate()

    '            clsSVT.setGridDataBind(strSQL, vsfDet)
    '            .Cols(0).Visible = False
    '            .Refresh()
    '            .Visible = True
    '            Call showNormalCursor()
    '            .EndUpdate()
    '        End With
    '        Call SetFlexRowColor(vsfDet, "", 0)
    '        ' Call SetHideCol(vsfDet, True, 3, 4, 5, 6)

    '        intTotNoOfRec = Val(vsfDet.Rows.Count - 1)
    '        txtNoofRec.Text = intTotNoOfRec
    '        If vsfDet.Rows.Count > 1 Then
    '            GrpUpdate.Visible = True
    '            pnlExp.Visible = True
    '            GrpDelete.Visible = True
    '            btnUpdate.Visible = False
    '        Else
    '            GrpUpdate.Visible = False
    '            pnlExp.Visible = False
    '            GrpDelete.Visible = False
    '            btnUpdate.Visible = False
    '        End If
    '        If UCase(Trim(cboTable.Text)) = "MSTCUST" Or UCase(Trim(cboTable.Text)) = "MSTSUPP" Then
    '            For i = 1 To vsfDet.Cols.Count - 1
    '                If UCase(vsfDet.GetData(0, i)) = "REMARKS" Then
    '                    vsfDet.SetData(0, i, "Party Notes")
    '                    Exit For
    '                End If
    '            Next
    '        End If
    '        If UCase(Trim(cboTable.Text)) = "TRNDTLBR" Then
    '            Call SetHideCol(vsfDet, True, 3, 4, 5, 6)
    '        End If
    '    End Sub

    '    Private Sub vsfDet_BeforeEdit(sender As Object, e As RowColEventArgs) Handles vsfDet.BeforeEdit
    '        e.Cancel = True
    '    End Sub

    '    Private Sub cboTable_Validating(sender As Object, e As CancelEventArgs) Handles cboTable.Validating
    '        If cboTable.Text = "" Then Exit Sub
    '        Dim blnFound As Boolean
    '        clsSVT.getDataStringValue("Select Name from Sys.Tables where Name='" & Trim(cboTable.Text) & "'", "", blnFound)

    '        If Not blnFound Then
    '            MsgBox("Table Not found in the list!!", MsgBoxStyle.Information, Me.Text)
    '            cboTable.Focus()
    '            Exit Sub
    '        End If
    '    End Sub

    '    Private Sub vsfDet_AfterFilter(sender As Object, e As EventArgs) Handles vsfDet.AfterFilter
    '        Dim i As Integer, intno As Integer = 0

    '        For i = 1 To vsfDet.Rows.Count - 1
    '            If vsfDet.Rows(i).Visible = True Then
    '                intno = intno + 1
    '            End If
    '        Next
    '        txtNoofRec.Text = intno & " / " & intTotNoOfRec
    '    End Sub


    '    Private Sub vsfDet_DoubleClick(sender As Object, e As EventArgs) Handles vsfDet.DoubleClick
    '        Dim intLength As Integer = 0

    '        With vsfDet
    '            If .Col = 0 Or .Col = 1 Then
    '                MsgBox(Trim(.GetData(0, .Col)) & " Coloumn cannot be editable", MsgBoxStyle.Information, Me.Text)
    '                Exit Sub
    '            End If


    '            txtFields.Text = vsfDet.GetData(0, vsfDet.Col)

    '            If RdoComplete.Checked = True Then
    '                txtCurValue.Text = vsfDet.GetData(vsfDet.Row, vsfDet.Col).ToString
    '            Else
    '                txtPartial.Text = vsfDet.GetData(vsfDet.Row, vsfDet.Col).ToString
    '            End If

    '            strDataType = vsfDet.Cols(vsfDet.Col).DataType
    '            Select Case strDataType
    '                Case GetType(Integer), GetType(Decimal), GetType(Double), GetType(String)
    '                    txtReplaceValue.Visible = True
    '                    dtpDate.Visible = False
    '                Case GetType(DateTime)
    '                    txtReplaceValue.Visible = False
    '                    dtpDate.Visible = True
    '            End Select

    '        End With

    '        txtReplaceValue.Text = ""
    '        If txtReplaceValue.Visible = True Then
    '            txtReplaceValue.Focus()
    '        Else
    '            dtpDate.Focus()
    '        End If
    '        If txtFields.Text <> "" Then
    '            btnUpdate.Visible = True
    '        Else
    '            btnUpdate.Visible = False
    '        End If
    '    End Sub

    '    Private Sub PartialUpdate()
    '        Dim svtTrn As OleDb.OleDbTransaction
    '        Dim strSql As String = ""
    '        Dim intlength As Integer
    '        Dim i As Integer, intCount As Integer
    '        Dim blnfound As Boolean

    '        intCount = 0
    '        If txtReplaceValue.Text = "" AndAlso txtFields.Text = "" Then Exit Sub
    '        If txtPartial.Text = "" Then Exit Sub

    '        svtTrn = svtConn.BeginTransaction(IsolationLevel.ReadCommitted)
    '        Try
    '            intlength = clsSVT.getDataLongValue("select character_Maximum_Length from INFORMATION_SCHEMA.COLUMNS where table_Name='" & cboTable.Text & "'And Column_Name = '" & Trim(txtFields.Text) & " '", 0, False, svtTrn)
    '            For i = 1 To vsfDet.Rows.Count - 1

    '                If vsfDet.Rows(i).Visible = True Then

    '                    Select Case strDataType
    '                        Case GetType(Integer), GetType(Decimal), GetType(Double)

    '                        Case GetType(DateTime)

    '                        Case Else

    '                            clsSVT.getDataLongValue("select " & vsfDet.GetData(0, 0) & " from " & cboTable.Text & " where " & txtFields.Text & " LIKE '%" & txtPartial.Text & "%' And " & vsfDet.GetData(0, 0) & "=" & vsfDet.GetData(i, 0), 0, blnfound, svtTrn)

    '                            If Not blnfound Then
    '                                GoTo nextloop
    '                            End If

    '                            If Len(txtReplaceValue.Text) > intlength Then
    '                                MsgBox("Lenght Of replace value should Not be more than " & intlength, MsgBoxStyle.Information, Me.Text)
    '                                txtReplaceValue.Focus()
    '                                svtTrn.Rollback()
    '                                Exit Sub
    '                            End If

    '                            If txtPartial.Text <> "" Then

    '                                strSql = " Update " & cboTable.Text
    '                                strSql &= " Set " & Trim(txtFields.Text) & " = Replace( " & Trim(txtFields.Text) & ",'" & txtPartial.Text & "','" & txtReplaceValue.Text & "') "
    '                                strSql &= " where  " & Trim(txtFields.Text) & " Like '%" & Trim(txtPartial.Text) & "%' and " & vsfDet.GetData(0, 0) & " = " & vsfDet.GetData(i, 0)

    '                            Else

    '                                strSql = "Update " & cboTable.Text
    '                                strSql &= " Set " & Trim(txtFields.Text) & " = Replace( " & Trim(txtFields.Text) & ",'" & txtPartial.Text & "','" & txtReplaceValue.Text & "') "
    '                                strSql &= " where " & vsfDet.GetData(0, 0) & " = " & vsfDet.GetData(i, 0) & " And  (" & txtFields.Text & " ='" & Trim(txtPartial.Text) & "') Or   (" & Trim(txtFields.Text) & " is Null)"
    '                            End If
    '                    End Select
    '                End If
    '                If strSql <> "" Then
    '                    intCount = intCount + 1
    '                    If Not clsSVT.execSQLStatement(strSql, svtTrn) Then
    '                        svtTrn.Rollback()
    '                        Exit Sub
    '                    End If
    '                End If
    '                strSql = ""
    'nextloop:
    '            Next i

    '            MsgBox(intCount & " Rows updated Successfully", MsgBoxStyle.Information, Me.Text)
    '            svtTrn.Commit()
    '            Call fillData()

    '        Catch ex As Exception
    '            MsgBox("Error during Update" & vbCrLf & ex.Message, MsgBoxStyle.Critical, Me.Text)
    '            svtTrn.Rollback()
    '        End Try
    '    End Sub

    '    Private Sub CompleteUpdate()
    '        Dim svtTrn As OleDb.OleDbTransaction
    '        Dim strSql As String = ""
    '        Dim intlength As Integer
    '        Dim i As Integer, intCount As Integer
    '        Dim blnfound As Boolean

    '        intCount = 0
    '        If txtReplaceValue.Text = "" AndAlso txtFields.Text = "" Then Exit Sub
    '        svtTrn = svtConn.BeginTransaction(IsolationLevel.ReadCommitted)
    '        Try
    '            intlength = clsSVT.getDataLongValue("select character_Maximum_Length from INFORMATION_SCHEMA.COLUMNS where table_Name='" & cboTable.Text & "'And Column_Name = '" & Trim(txtFields.Text) & " '", 0, False, svtTrn)
    '            For i = 1 To vsfDet.Rows.Count - 1

    '                If vsfDet.Rows(i).Visible = True Then

    '                    Select Case strDataType
    '                        Case GetType(Integer), GetType(Decimal), GetType(Double)

    '                            clsSVT.getDataLongValue("select " & vsfDet.GetData(0, 0) & " from " & cboTable.Text & " where " & txtFields.Text & "=" & txtCurValue.Text & " And " & vsfDet.GetData(0, 0) & "=" & vsfDet.GetData(i, 0), 0, blnfound, svtTrn)
    '                            If Not blnfound Then
    '                                GoTo nextloop
    '                            End If

    '                            strSql = "Update " & cboTable.Text & " set " & txtFields.Text & " = " & Val(Trim(Replace(Replace(txtReplaceValue.Text, vbCrLf, ""), vbCr, ""))) & " where " & txtFields.Text & " =" & Trim(txtCurValue.Text) & " and " & vsfDet.GetData(0, 0) & " = " & vsfDet.GetData(i, 0)

    '                        Case GetType(DateTime)

    '                            If Trim(txtCurValue.Text) = "" Then
    '                                clsSVT.getDataLongValue("select " & vsfDet.GetData(0, 0) & " from " & cboTable.Text & " where " & txtFields.Text & "='" & (txtCurValue.Text) & "'Or   (" & Trim(txtFields.Text) & " is Null) And " & vsfDet.GetData(0, 0) & "=" & vsfDet.GetData(i, 0), 0, blnfound, svtTrn)
    '                                If Not blnfound Then
    '                                    GoTo nextloop
    '                                End If
    '                                strSql = "Update " & cboTable.Text & " Set " & txtFields.Text & " = " & getDateFormatInCond(dtpDate.Value) & "" & " where " & Trim(txtFields.Text) & " ='" & (txtCurValue.Text) & "'Or   (" & Trim(txtFields.Text) & " is Null)  and " & vsfDet.GetData(0, 0) & " = " & vsfDet.GetData(i, 0)

    '                            Else
    '                                clsSVT.getDataLongValue("select " & vsfDet.GetData(0, 0) & " from " & cboTable.Text & " where " & txtFields.Text & "=" & getDateFormatInCond(CDate(txtCurValue.Text)) & " And " & vsfDet.GetData(0, 0) & "=" & vsfDet.GetData(i, 0), 0, blnfound, svtTrn)
    '                                If Not blnfound Then
    '                                    GoTo nextloop
    '                                End If
    '                                strSql = "Update " & cboTable.Text & " Set " & txtFields.Text & " = " & getDateFormatInCond(dtpDate.Value) & "" & " where " & Trim(txtFields.Text) & " =" & getDateFormatInCond(CDate(txtCurValue.Text)) & " And " & vsfDet.GetData(0, 0) & "=" & vsfDet.GetData(i, 0)
    '                            End If

    '                        Case Else
    '                            clsSVT.getDataLongValue("select " & vsfDet.GetData(0, 0) & " from " & cboTable.Text & " where " & txtFields.Text & "='" & txtCurValue.Text & "' And " & vsfDet.GetData(0, 0) & "=" & vsfDet.GetData(i, 0), 0, blnfound, svtTrn)
    '                            If Not blnfound Then
    '                                GoTo nextloop
    '                            End If

    '                            If Len(txtReplaceValue.Text) > intlength Then
    '                                MsgBox("Lenght Of replace value should Not be more than " & intlength, MsgBoxStyle.Information, Me.Text)
    '                                txtReplaceValue.Focus()
    '                                svtTrn.Rollback()
    '                                Exit Sub
    '                            End If
    '                            If txtCurValue.Text = "" Then
    '                                strSql = "Update " & cboTable.Text & " Set " & Trim(txtFields.Text) & " = '" & Trim(Replace(Replace(txtReplaceValue.Text, vbCrLf, ""), vbCr, "")) & "' where " & vsfDet.GetData(0, 0) & " = " & vsfDet.GetData(i, 0) & " And  (" & txtFields.Text & " ='" & Trim(txtCurValue.Text) & "') Or   (" & Trim(txtFields.Text) & " is Null)"
    '                            Else
    '                                strSql = "Update " & cboTable.Text & " set " & Trim(txtFields.Text) & " = '" & Trim(Replace(Replace(txtReplaceValue.Text, vbCrLf, ""), vbCr, "")) & "' where " & Trim(txtFields.Text) & " ='" & Trim(txtCurValue.Text) & "' and " & vsfDet.GetData(0, 0) & " = " & vsfDet.GetData(i, 0)
    '                            End If
    '                    End Select
    '                End If
    '                If strSql <> "" Then
    '                    intCount = intCount + 1
    '                    If Not clsSVT.execSQLStatement(strSql, svtTrn) Then
    '                        svtTrn.Rollback()
    '                        Exit Sub
    '                    End If
    '                End If
    '                strSql = ""
    'nextloop:
    '            Next i

    '            MsgBox(intCount & " Rows updated Successfully", MsgBoxStyle.Information, Me.Text)
    '            svtTrn.Commit()
    '            Call fillData()




    '        Catch ex As Exception
    '            MsgBox("Error during Update" & vbCrLf & ex.Message, MsgBoxStyle.Critical, Me.Text)
    '            svtTrn.Rollback()
    '        End Try
    '    End Sub

    '    Private Sub btnUpdate_Click(sender As Object, e As EventArgs)
    '        If RdoComplete.Checked = True Then
    '            Call CompleteUpdate()
    '        Else
    '            Call PartialUpdate()

    '        End If
    '    End Sub

    '    Private Sub frmUtlBulkUpdate_KeyUp(sender As Object, e As KeyEventArgs) Handles Me.KeyUp
    '        If e.KeyCode = Keys.F9 And e.Control = True Then
    '            Call initFormSize(Me, Me.Text, True, 1100, 682, 0, 0)
    '            MsgBox("Restored Form & Grid Layout Successfully!!", vbInformation, Me.Text)
    '        End If
    '        If e.KeyCode = Keys.F8 And e.Control = True Then
    '            Call SaveActFormSettings(Me, Me.Text)
    '            Call SaveActFormVsfDet(Me, Me.Text, "DET", vsfDet)
    '            MsgBox("Saved Form & Grid Layout Successfully!!", vbInformation, Me.Text)
    '        End If

    '        If e.Alt = True And e.KeyCode = Keys.E Then
    '            Call ExportToXL(vsfDet)
    '        End If

    '    End Sub

    '    Private Sub txtReplaceValue_KeyPress(sender As Object, e As KeyPressEventArgs)
    '        If strDataType = GetType(Integer) Or strDataType = GetType(Decimal) Or strDataType = GetType(Double) Then
    '            If KeyValidate("Decimal", e.KeyChar) Then
    '                e.Handled = False
    '            Else
    '                e.Handled = True
    '            End If
    '        End If
    '        If e.KeyChar = "'" Then
    '            e.Handled = True
    '        End If

    '    End Sub

    '    Private Sub txtPartial_KeyPress(sender As Object, e As KeyPressEventArgs)

    '        If strDataType = GetType(Integer) Or strDataType = GetType(Decimal) Or strDataType = GetType(Double) Then
    '            If KeyValidate("Decimal", e.KeyChar) Then
    '                e.Handled = False
    '            Else
    '                e.Handled = True
    '            End If
    '        End If
    '        If e.KeyChar = "'" Then
    '            e.Handled = True
    '        End If

    '    End Sub


    '    Private Sub btnDelete_Click(sender As Object, e As EventArgs)

    '        If Trim(cboTable.Text) = "" Then Exit Sub
    '        For i = 1 To vsfDet.Rows.Count - 1
    '            If vsfDet.Rows(i).Visible = True Then
    '                If Not blnDeleteOK(cboTable.Text, Val(vsfDet.GetData(i, 0)), True, True,) Then
    '                    Exit Sub
    '                End If
    '            End If
    '        Next i
    '        If MsgBox("Are you sure you want to delete the Record from ? " &
    '            cboTable.Text, vbCritical + MsgBoxStyle.YesNo + vbDefaultButton2, "Delete Confirmation") = MsgBoxResult.Yes Then
    '            If MsgBox("You can't undo this Delete operation ! " &
    '            "Continue?", vbCritical + MsgBoxStyle.YesNo + vbDefaultButton2, "Delete Confirmation") = MsgBoxResult.Yes Then
    '                Dim svtTrn As OleDb.OleDbTransaction = Nothing
    '                Try
    '                    svtTrn = svtConn.BeginTransaction(IsolationLevel.ReadCommitted)
    '                    For i = 1 To vsfDet.Rows.Count - 1
    '                        If vsfDet.Rows(i).Visible = True Then
    '                            If Not clsSVT.execSQLStatement("Delete from " & cboTable.Text & " where " & vsfDet.GetData(0, 0) & " = " & vsfDet.GetData(i, 0), svtTrn) Then
    '                                svtTrn.Rollback()
    '                                Exit Sub
    '                            End If
    '                        End If
    '                    Next i
    '                    MsgBox("Deleted Successfully!", MsgBoxStyle.Information, Me.Text)

    '                    svtTrn.Commit()
    '                    Call fillData()
    '                Catch ex As Exception
    '                    svtTrn.Rollback()
    '                    Exit Sub
    '                End Try
    '            Else
    '                Exit Sub

    '            End If
    '        End If


    '    End Sub

    '    Private Sub btnExcel_Click(sender As Object, e As EventArgs) Handles btnExcel.Click
    '        Call ExportToXL(vsfDet)
    '    End Sub

    '    Private Sub btnFreeze_Click(sender As Object, e As EventArgs) Handles btnFreeze.Click
    '        'If btnFreeze.Text = "&Freeze" Then
    '        '    btnFreeze.Text = "&UnFreeze"
    '        '    ToolTip.SetToolTip(Me.btnFreeze, "UnFreeze : Alt + U")
    '        '    vsfDet.Cols.Frozen = vsfDet.Col + 1
    '        '    vsfDet.Cols.Frozen = vsfDet.Col + 1
    '        'Else
    '        '    btnFreeze.Text = "&Freeze"
    '        '    tooltip.Active = False
    '        '    tooltip.SetToolTip(Me.btnFreeze, "Freeze : Alt + F")
    '        '    tooltip.Active = True
    '        '    vsfDet.Cols.Frozen = 0
    '        '    vsfDet.Cols.Frozen = 0
    '        'End If

    '        If btnFreeze.Text = "&Freeze" Then
    '            ToolTip1.SetToolTip(Me.btnFreeze, "UnFreeze : Alt+N")
    '            btnFreeze.Text = "U&nFreeze"
    '            vsfDet.Cols.Frozen = vsfDet.Col + 1
    '        Else
    '            btnFreeze.Text = "&Freeze"
    '            ToolTip1.SetToolTip(Me.btnFreeze, "Freeze : Alt+F")
    '            vsfDet.Cols.Frozen = 0
    '        End If
    '    End Sub

    '    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
    '        Me.Close()
    '    End Sub

    '    Private Sub frmUtlBulkUpdate_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
    '        Call frmMain.CloseSelectedTabPage(Me)
    '    End Sub

    '    Private Sub RdoComplete_CheckedChanged(sender As Object, e As EventArgs)
    '        txtPartial.Visible = False
    '    End Sub

    '    Private Sub RdoPartial_CheckedChanged(sender As Object, e As EventArgs)
    '        txtPartial.Visible = True
    '    End Sub

    '    Private Sub vsfDet_KeyUp(sender As Object, e As KeyEventArgs) Handles vsfDet.KeyUp
    '        With vsfDet
    '            If e.KeyCode = Keys.Tab Then
    '                If vsfDet.Row = vsfDet.Rows.Count - 1 AndAlso vsfDet.Col = vsfDet.Cols.Count - 1 Then
    '                    btnFreeze.Focus()
    '                End If
    '            End If
    '        End With
    '    End Sub

    '    'Private Sub vsfDet_KeyDown(sender As Object, e As KeyEventArgs) Handles vsfDet.KeyDown
    '    '    If vsfDet.Row = vsfDet.Rows.Count - 1 AndAlso vsfDet.Col = vsfDet.Cols.Count - 1 Then
    '    '        btnFreeze.Focus()
    '    '    End If
    '    'End Sub

    '    Private Sub btnDelete_Leave(sender As Object, e As EventArgs)
    '        btnClose.Focus()
    '    End Sub

    '    Private Sub btnShowRec_Leave(sender As Object, e As EventArgs)
    '        With vsfDet
    '            If Not (String.IsNullOrEmpty(cboTable.Text)) Then
    '                .Row = 1
    '                .Col = 1
    '                Call showWaitCursor()
    '                'Else
    '                'Row = 0
    '                '.Col = 1
    '                'l showWaitCursor()
    '            End If
    '        End With
    '    End Sub

    '    Private Sub btnClose_Leave(sender As Object, e As EventArgs) Handles btnClose.Leave
    '        cboTable.Focus()
    '    End Sub

    '    Private Sub txtNoofRec_Leave(sender As Object, e As EventArgs) Handles txtNoofRec.Leave
    '        RdoComplete.Focus()
    '    End Sub

    '    Private Sub btnExcel_Leave(sender As Object, e As EventArgs) Handles btnExcel.Leave
    '        RdoComplete.Focus()
    '    End Sub

    '    Private Sub RdoComplete_Leave(sender As Object, e As EventArgs)
    '        txtPartial.Focus()
    '    End Sub

    '    Private Sub txtPartial_Leave(sender As Object, e As EventArgs)
    '        txtReplaceValue.Focus()
    '    End Sub

    '    Private Sub txtReplaceValue_Leave(sender As Object, e As EventArgs)
    '        btnUpdate.Focus()
    '    End Sub

    '    Private Sub btnUpdate_Leave(sender As Object, e As EventArgs)
    '        btnDelete.Focus()
    '    End Sub

    '    Private Sub frmUtlBulkUpdate_Shown(sender As Object, e As EventArgs) Handles Me.Shown
    '        cboTable.Focus()
    '    End Sub
End Class