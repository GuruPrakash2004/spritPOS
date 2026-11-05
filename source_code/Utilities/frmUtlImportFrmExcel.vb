Imports Microsoft.Office.Interop
Imports C1.Win.C1FlexGrid
Imports System.IO

Public Class frmUtlImportFrmExcel
    Dim intSelRow As Integer '# A - Starting row
    Dim strErrorMsg As String
    Dim blnValidData As Boolean
    Dim blnfirstload As Boolean
    Dim strRights As String
    Dim intDocID As Integer, intMenuID As Integer

    Dim strFldr As String
    Dim strTemp As String
    Dim clssvt As New svtLibrary
    Dim strDocIDFld As String
    Dim strTblName As String

    Const vTblName = 0
    'Const vTblCaption = 1
    'Const vTblLinkCond = 2
    Const vFldCode = 1
    Const vSubTblCaption = 2

    Const vsfExSlNo = 0
    Const vsfExItemID = 2
    Const vsfExItemCode = 1
    Const vsfExItemName = 3
    Const vsfExUOMID = 4
    Const vsfExUOM = 5
    Const vsfExItemGrpID = 6
    Const vsfExItemGrp = 7
    'Const vsfExInactive = 8
    'Const vsfExInactiveRem = 9
    Const vsfExItemDesc = 8
    Const vsfExVATPer = 9
    Const vsfExBrand = 10
    Const vsfExSubGroup = 11
    Const vsfExHotKey = 12
    'Const vsfExNonStkable = 15
    'Const vsfExComboItem = 16
    'Const vsfExAllowNeg = 17
    Dim vExErr As Integer
    Dim vExBlnErr As Integer

    Private Sub frmUtlImportItemFmExcel_KeyUp(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyUp
        If e.KeyCode = Keys.F9 And e.Control = True Then
            Call initFormSize(Me, Me.Text, False, 577, 349, 0, 0)
            MsgBox("Restored Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
        If e.KeyCode = Keys.F8 And e.Control = True Then
            Call SaveActFormSettings(Me, Me.Text)
            Call SaveActFormVsfDet(Me, Me.Text, "Excel", vsfExcel)
            MsgBox("Saved Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
    End Sub

    Private Sub frmUtlItemImport_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
     
        Call bindCombo(cboMst, "DocName", "DocName", "Select DocName from UtlTransactions where  doctype = 'Master' and docID not in (6,9,197,208,120,17,5,198,155,13,10) order by DocName")

        Call enablebtn(False)
        Call EnableFields(False, 1)
        Call initGridStyles(Me)
        Call fillFxdHdrDtls(Trim(cboMst.Text))
        vsfExcel.Rows.Fixed = 1
        vsfExcel.Styles.Fixed.Font = New Font("Lucida Sans", vsfExcel.Font.Size, FontStyle.Bold)
        vsfExcel.Styles.Fixed.TextAlign = TextAlignEnum.CenterCenter
        Call vsfExcel.AutoSizeCols()
        cboMst.Focus()
    End Sub

    Private Sub wrapGrid()
        With vsfExcel
            .Styles(CellStyleEnum.Normal).WordWrap = True
            .AllowResizing = AllowResizingEnum.Both
            .AutoSizeRows()
        End With
    End Sub

    Private Sub enablebtn(ByVal argBLN As Boolean)
        lblError.Visible = argBLN
        btnRemErr.Visible = False
        'optAppnd.Visible = argBLN
        'optDel.Visible = argBLN
        'ChkInvalidData.Visible = argBLN
    End Sub

    Private Sub btnBrowse_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowse.Click
        Cdlg.DefaultExt = "xlsx"
        Cdlg.Filter = "Excel File (*.xlsx)|*.xlsx"
        Cdlg.FileName = "*.xlsx"

        If Cdlg.ShowDialog() <> DialogResult.OK Then
            Return
            Exit Sub
        End If
        txtXLFilePath.Text = Cdlg.FileName
        Call EnableFields(False, 1)
        btnImportFromXL.Enabled = True
    End Sub

    Private Sub EnableFields(ByVal blnEnable As Boolean, ByVal argType As Integer)
        Select Case argType
            Case 1
                btnSave.Enabled = blnEnable
            Case 2
                btnSave.Enabled = Not (blnEnable)
            Case 3
                btnSave.Enabled = blnEnable
                btnImportFromXL.Enabled = blnEnable
            Case 4

        End Select
    End Sub

    Private Sub btnImportFromXL_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImportFromXL.Click

        If Trim(txtXLFilePath.Text) = "" Then
            MsgBox("Please select excel file path..", MsgBoxStyle.Information + vbOKOnly, Me.Text)
            Exit Sub
        End If

        If Dir(Trim(txtXLFilePath.Text)) = "" Then
            MsgBox("File not found", MsgBoxStyle.Information + vbOKOnly, Me.Text)
            Exit Sub
        End If

        If blnfirstload = True Then
            With vsfExcel
                vsfExcel.Cols.Count = vsfExcel.Cols.Count - 2
            End With
        End If

        Call importFromXL(Trim(txtXLFilePath.Text))

      
        optAppnd.Checked = True

        If Not blnValidData Then

            Call EnableFields(True, 2)
            lblError.Visible = True
            'lblError.Text = "Text displayed in red colour is erroneous data cannot be imported to DB"
            'btnRemErr.Visible = True
            enablebtn(True)

            Call EnableFields(True, 1)
        Else
            'Me.ToolTip1.SetToolTip(Me.vsfDet, "")
            Call EnableFields(True, 1)
            lblError.Visible = False
            btnRemErr.Visible = False
            Call enablebtn(True)
        End If

        txtNoOfInv.Text = vsfExcel.Rows.Count - 1

    End Sub

    Private Sub importFromXL(ByVal argFileName As String)
        Dim xlapp As New Excel.Application
        Dim i As Integer, str As String = ""
        Dim strExcelCaption As String, strCaption As String = ""
        Dim introw As Integer, intcol As Integer
        xlapp.Workbooks.Open(argFileName, , True)
        'Call InitScreenINV


        blnfirstload = False
        With xlapp.Application
            intcol = vsfExcel.Cols.Count - 1
            strExcelCaption = ""
            For i = 1 To intcol
                str = UCase(Trim(xlapp.ActiveSheet.Range(Chr(64 + i) & 1).Value))
                strExcelCaption = strExcelCaption & str & ","
            Next i

            If Trim(strExcelCaption) <> "" Then strExcelCaption = LSet(strExcelCaption, Len(strExcelCaption) - 1)


            With vsfExcel
                For intcol = 1 To .Cols.Count - 1
                    strCaption = strCaption & .GetData(0, intcol) & ","
                Next
            End With

            strCaption = LSet(strCaption, Len(strCaption) - 1)


            'Invalid XLS file selected validation
            If Trim(UCase(strExcelCaption)) <> strCaption Then
                MsgBox("Please select the valid template", vbInformation + vbOKOnly, Me.Text)
                vsfExcel.Clear(C1.Win.C1FlexGrid.ScrollFlags.None)
                vsfExcel.Rows.Count = 1
                Exit Sub
            End If
            vsfExcel.Rows.Count = 1

            i = 2 'ASSIGNING THE FIRST ROW
            Try
                Do While Trim(.Range("A" & i).Value) <> ""
                    introw = vsfExcel.Rows.Count
                    vsfExcel.Rows.Count = vsfExcel.Rows.Count + 1

                    vsfExcel.SetData(introw, vsfExSlNo, introw)
                    For intcol = 1 To vsfExcel.Cols.Count - 1
                        strExcelCaption = UCase(Trim(xlapp.ActiveSheet.Range(Chr(64 + intcol) & i).Value))

                        If Trim(strExcelCaption) <> "" Then
                            vsfExcel.SetData(introw, intcol, strExcelCaption)
                        End If
                    Next intcol

                    txtNoOfInv.Text = i
                    i = i + 1
                Loop
                StMsg("Data Loaded, now Validating the data")
                .Workbooks(1).Close(SaveChanges:=False)
            Catch ex As Exception
                MsgBox("Cannot export to Excel." & vbCrLf & ex.Message, MsgBoxStyle.Critical, "Import Error")
            End Try
        End With

        With vsfExcel
            blnfirstload = True
            vsfExcel.Cols.Add()
            vExErr = .Cols.Count - 1
            vsfExcel.Cols.Add()
            vExBlnErr = .Cols.Count - 1
            .SetData(0, vExErr, "Errors")
            .SetData(0, vExBlnErr, "BlnFound")
        End With

        'Validating the data here - begins
        Call ValidateEntry()
        Call SetHideCol(vsfExcel, vExBlnErr)
        vsfExcel.AutoSizeCols(0, vsfExcel.Cols.Count - 1, 1)
        StMsg("")


        vsfExcel.Redraw = True

        Call wrapGrid()
        Exit Sub
    End Sub

    Private Function ValidateEntry() As Boolean
        Dim i As Integer, strerr As String = "", strCode As String, blnfound As Boolean
        Dim strSQL As String
        blnValidData = True
        lblError.Visible = False
        btnRemErr.Visible = False


        ValidateEntry = False
        blnValidData = True

        With vsfExcel
            If .Rows.Count = 1 Then
                MsgBox("There is no data exists for importing", vbInformation + vbOKOnly, Me.Text)
                Exit Function
            End If

            For i = 1 To .Rows.Count - 1
                .TopRow = i
                'DoEvents()
                strerr = "" 'Refreshing the err messg - each time
                vsfExcel.SetData(i, vExErr, strerr)
                vsfExcel.SetData(i, vExBlnErr, "")
                '.Cell(flexcpBackColor, i, vDetDocNo) = vbWhite
                vsfExcel.SetData(i, vsfExItemCode, Replace(vsfExcel.GetData(i, vsfExItemCode), "'", "`"))
                lblStatus.Text = "Validating " & vsfExcel.GetData(i, vsfExSlNo) & " -- " & i & " / " & txtNoOfInv.Text


                'If Trim(vsfExcel.GetData(i, vsfExItemCode)) = "" Then
                '    strerr = strerr & IIf(i <> 1, vbLf, "") & " " & .GetData(0, vsfExItemCode) & " is Empty "
                '    .SetData(i, vExErr, strerr)
                '    .SetData(i, vExBlnErr, "Y")
                '    blnValidData = False
                '    Dim cs As C1.Win.C1FlexGrid.CellStyle = vsfExcel.Styles.Add("ItemCode")
                '    cs.BackColor = Color.Red

                '    .SetCellStyle(i, vsfExItemCode, cs)
                'End If

                strCode = Replace(strDocIDFld, "ID", "Code")

                strSQL = " Select " & strDocIDFld & " from " & strTblName & " where " & strCode & "='" & vsfExcel.GetData(i, vsfExItemCode) & "'"

                clssvt.getDataLongValue(strSQL, 0, blnfound)

                'strCode = vsfExcel.GetData(i, vsfExItemCode)
                If blnfound Then
                    Dim cs As C1.Win.C1FlexGrid.CellStyle = vsfExcel.Styles.Add("ItemCode")
                    cs.BackColor = Color.Red
                    blnValidData = False
                    strerr = strerr & IIf(i <> 1, vbLf, "") & " " & vsfExcel.GetData(0, vsfExItemCode) & " already exists "
                    vsfExcel.SetData(i, vExErr, strerr)
                    vsfExcel.SetData(i, vExBlnErr, "Y")
                    vsfExcel.SetCellStyle(i, vsfExItemCode, cs)
                    strerr = ""
                End If



                'If Trim(vsfExcel.GetData(i, vsfExItemName)) = "" Then
                '    strerr = strerr & IIf(i <> 1, vbLf, "") & " " & .GetData(0, vsfExItemName) & " is Empty "
                '    .SetData(i, vExErr, strerr)
                '    .SetData(i, vExBlnErr, "Y")
                '    blnValidData = False
                '    Dim cs As C1.Win.C1FlexGrid.CellStyle = vsfExcel.Styles.Add("ItemName")
                '    cs.BackColor = Color.Red

                '    .SetCellStyle(i, vsfExItemName, cs)
                'End If

                'If Trim(vsfExcel.GetData(i, vsfExUOM)) = "" Then
                '    strerr = strerr & IIf(i <> 1, vbLf, "") & " " & .GetData(0, vsfExUOM) & " is Empty "
                '    .SetData(i, vExErr, strerr)
                '    .SetData(i, vExBlnErr, "Y")
                '    blnValidData = False
                '    Dim cs As C1.Win.C1FlexGrid.CellStyle = vsfExcel.Styles.Add("UOM")
                '    cs.BackColor = Color.Red

                '    .SetCellStyle(i, vsfExUOM, cs)
                'End If

                'If Trim(vsfExcel.GetData(i, vsfExItemGrp)) = "" Then
                '    strerr = strerr & IIf(i <> 1, vbLf, "") & " " & .GetData(0, vsfExItemGrp) & " is Empty "
                '    .SetData(i, vExErr, strerr)
                '    .SetData(i, vExBlnErr, "Y")
                '    blnValidData = False
                '    Dim cs As C1.Win.C1FlexGrid.CellStyle = vsfExcel.Styles.Add("ItemGroup")
                '    cs.BackColor = Color.Red

                '    .SetCellStyle(i, vsfExItemGrp, cs)
                'End If


                '    strChk = Trim(vsfExcel.GetData(i, vsfExInactive))

                '    If Not strChk = "Y" Or strChk = "N" Then
                '        Trim(vsfExcel.SetData(i, vsfExInactive, "N"))
                '    End If

                '    strChk = Trim(vsfExcel.GetData(i, vsfExNonStkable))
                '    If Not strChk = "Y" Or strChk = "N" Then
                '        Trim(vsfExcel.SetData(i, vsfExNonStkable, "N"))
                '    End If

                '    strChk = Trim(vsfExcel.GetData(i, vsfExComboItem))
                '    If Not strChk = "Y" Or strChk = "N" Then
                '        Trim(vsfExcel.SetData(i, vsfExComboItem, "N"))
                '    End If

                '    strChk = Trim(vsfExcel.GetData(i, vsfExAllowNeg))
                '    If Not strChk = "Y" Or strChk = "N" Then
                '        Trim(vsfExcel.SetData(i, vsfExAllowNeg, "N"))
                '    End If

            Next i

          
            If clssvt.ChkDuplicateData(vsfExcel, i, vsfExItemCode) Then
                Dim cs As C1.Win.C1FlexGrid.CellStyle = vsfExcel.Styles.Add("ItemCode")
                strerr = strerr & IIf(i <> 1, vbLf, "") & " " & .GetData(0, vsfExItemCode) & " duplication of data "
                .SetData(i, vExErr, strerr)
                .SetData(i, vExBlnErr, "Y")
                cs.BackColor = Color.Red
                blnValidData = False
                vsfExcel.SetCellStyle(i, vsfExItemCode, cs)
                'Exit Function
            End If
            'If clssvt.ChkDuplicateData(vsfExcel, i, vsfExHotKey) Then
            '    Dim cs As C1.Win.C1FlexGrid.CellStyle = vsfExcel.Styles.Add("HotKey")
            '    strerr = strerr & IIf(i <> 1, vbLf, "") & " " & vsfExcel.GetData(0, vsfExHotKey) & " should be unique "
            '    vsfExcel.SetData(i, vExErr, strerr)
            '    vsfExcel.SetData(i, vExBlnErr, "Y")
            '    cs.BackColor = Color.Red
            '    blnValidData = False
            '    vsfExcel.SetCellStyle(i, vsfExHotKey, cs)
            '    'Exit Function
            'End If
            'Call getItemPresent()
        End With


        lblStatus.Visible = False
        vsfExcel.Cols(vExErr).Visible = False
        vsfExcel.Cols(vExBlnErr).Visible = False

        If Not blnValidData Then
            vsfExcel.Cols(vExErr).Visible = True
            vsfExcel.Cols(vExBlnErr).Visible = False
            Call enablebtn(True)
            Exit Function
        End If
    End Function

    Private Function getItemPresent() As Boolean
        getItemPresent = False
        Dim strSQL As String
        Dim rs As OleDb.OleDbDataReader, strItemCode As String, strHotkey As String
        Dim strerr As String = ""
        Dim cs As C1.Win.C1FlexGrid.CellStyle = vsfExcel.Styles.Add("ItemCode")
        strSQL = "select * from MstItem"

        rs = clssvt.getDataReaderFromSQL(strSQL)

        With rs
            Do While .Read
                For i = 1 To vsfExcel.Rows.Count - 1
                    strItemCode = vsfExcel.GetData(i, vsfExItemCode)
                    If !ItemCode = strItemCode Then
                        cs.BackColor = Color.Red
                        blnValidData = False
                        strerr = strerr & IIf(i <> 1, vbLf, "") & " " & vsfExcel.GetData(0, vsfExItemCode) & " already exists "
                        vsfExcel.SetData(i, vExErr, strerr)
                        vsfExcel.SetData(i, vExBlnErr, "Y")
                        vsfExcel.SetCellStyle(i, vsfExItemCode, cs)
                        strerr = ""
                    End If
                    strHotkey = vsfExcel.GetData(i, vsfExHotKey)
                    If strHotkey <> "" Then
                        If GetVal(!Hotkey, "") = strHotkey Then
                            strerr = strerr & IIf(i <> 1, vbLf, "") & " " & vsfExcel.GetData(0, vsfExHotKey) & " should be unique "
                            vsfExcel.SetData(i, vExErr, strerr)
                            vsfExcel.SetData(i, vExBlnErr, "Y")
                            cs.BackColor = Color.Red
                            blnValidData = False
                            vsfExcel.SetCellStyle(i, vsfExHotKey, cs)
                            strerr = ""
                        End If
                    End If
                Next
            Loop
        End With

        getItemPresent = True
    End Function

    Private Sub btnExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        Dim strSQL As String
        Dim blnfound As Boolean
        If cboMst.Text = "" Then
            MsgBox("Select Master cannot be blank", MsgBoxStyle.Information, Me.Text)
            Exit Sub
        End If

        strSQL = "Select  DocName from UtlTransactions where doctype = 'Master' and DocName = '" & cboMst.Text & "'"

        Call clssvt.getDataStringValue(strSQL, "", blnfound)
        If Not blnfound Then
            MsgBox(" Selected Master is not found!!", MsgBoxStyle.Information, Me.Text)
            cboMst.Text = ""
            cboMst.Focus()
            Exit Sub
        End If
        Call fillFldsFromTbl(Trim(cboMst.Text))
        Call exportTemplate()
        'Dim xl As New Excel.Application

        'With xl
        '    .Workbooks.Add()
        '    .Range("A1").Value = "ITEMCODE"
        '    .Range("B1").Value = "ITEMNAME"
        '    .Range("C1").Value = "UOM"
        '    .Range("D1").Value = "ITEMGROUPNAME"
        '    .Range("E1").Value = "ITEMDESC"
        '    .Range("F1").Value = "VATPER"
        '    .Range("G1").Value = "BRAND"
        '    .Range("H1").Value = "SUBGROUP"
        '    .Range("I1").Value = "HOTKEY"
        '    .Visible = True
        'End With
    End Sub

    Private Sub btnRemErr_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRemErr.Click
        Dim i As Integer, j As Integer
        If MsgBox("Do you want to Remove ALL Erroneous Records ?", vbYesNo + vbQuestion, Me.Text) = vbNo Then
            Exit Sub
        End If
        vsfTmp.Rows.Count = 1
        vsfTmp.Cols.Count = vsfExcel.Cols.Count
        For i = 0 To vsfExcel.Cols.Count - 1
            vsfTmp.SetData(0, i, vsfExcel.GetData(0, i))
        Next i
        With vsfExcel
Repeat:
            For i = 1 To (.Rows.Count - 1)
                If .GetData(i, vExBlnErr) = "Y" Then
                    vsfTmp.Rows.Count = vsfTmp.Rows.Count + 1
                    For j = 0 To vsfExcel.Cols.Count - 1
                        Dim cs As C1.Win.C1FlexGrid.CellStyle = vsfExcel.Styles.Add("AllColumns")
                        vsfTmp.SetData(vsfTmp.Rows.Count - 1, j, vsfExcel.GetData(i, j))
                        cs.BackColor = Color.White
                        vsfExcel.SetCellStyle(i, j, cs)
                    Next j
                    .RemoveItem(i)
                    GoTo Repeat
                End If
            Next i
            If vsfTmp.Rows.Count > 1 Then
                If MsgBox("Do you want to Export ALL Erroneous Records to XL?", vbYesNo + vbQuestion, Me.Text) = vbYes Then
                    Call ExportToXL(vsfTmp, "Errorneous Records")
                End If
            End If
        End With
        lblError.Visible = False
        Call EnableFields(True, 1)
    End Sub

    Private Sub btnImpClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImpClose.Click
        Me.Close()
        txtNoOfInv.Text = ""
        txtXLFilePath.Text = ""
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click

        If vsfExcel.Rows.Count <= 1 Then
            MsgBox("No records found in the selected excel file!", vbOKOnly + vbInformation, Me.Text)
            Exit Sub
        End If

        'If Not blnValidData Then
        '    MsgBox("Cannot Import as Erroneous records are present", vbCritical + vbOKOnly, Me.Text)
        '    Exit Sub
        'End If
        ' If Not blnValidData Then Exit Sub
    
        Call ImportData()
        Call EnableFields(False, 3)
        Call enablebtn(False)
        With vsfExcel
            vsfExcel.Cols.Count = vsfExcel.Cols.Count - 2
        End With
      
    End Sub

    Private Sub ImportData()

        Dim i As Integer, strSQL As String = "", intID As Integer, blnfound As Boolean
        Dim strCode As String, j As Integer, strFlds As String = ""
        Dim strFldvalue As String = ""
        Try
            For i = 1 To vsfExcel.Rows.Count - 1
                If vsfExcel.GetData(i, vExBlnErr) <> "Y" Then
                    strCode = Replace(strDocIDFld, "ID", "Code")

                    strSQL = " Select " & strDocIDFld & " from " & strTblName & " where " & strCode & "='" & vsfExcel.GetData(i, vsfExItemCode) & "'"

                    intID = clssvt.getDataLongValue(strSQL, 0, blnfound)

                    If blnfound Then
                        strSQL = "Delete  from " & strTblName & " where " & strDocIDFld & " = " & intID
                        clssvt.execSQLStatement(strSQL)
                    End If

                    intID = clssvt.AutoIncrementedNum(strDocIDFld, strTblName)
                    strSQL = "Insert into " & strTblName & "("
                    With vsfExpTemp
                        For j = 1 To .Rows.Count - 1
                            strFlds = strFlds & vsfExpTemp.GetData(j, vFldCode) & ","
                        Next j
                        strFlds = LSet(strFlds, Len(strFlds) - 1)
                    End With

                    strSQL = strSQL & strDocIDFld & ","
                    strSQL = strSQL & strFlds & ") values ("

                    'strSQL = strSQL & "ItemID, AddedBy, AddedOn, ItemCode, ItemName, ItemDesc,"
                    'strSQL = strSQL & " InActive, InActiveRem,NonStkable,AllowNeg,ComboItem,UOMID,ItemGroupID,VATPER,BRAND,SUBGROUP,HOTKEY) values ("
                    strSQL = strSQL & intID & ","

                    For intcol = 1 To vsfExcel.Cols.Count - 1
                        strFldvalue = strFldvalue & "'" & vsfExcel.GetData(i, intcol) & "',"
                    Next intcol

                    strFldvalue = LSet(strFldvalue, Len(strFldvalue) - 1)
                    strSQL = strSQL & strFldvalue
                    strSQL = strSQL & ")"
                    clssvt.execSQLStatement(strSQL)
                End If
            Next
            MsgBox("Imported Data Successfully!", vbInformation, Me.Text)
        Catch ex As Exception
            MsgBox(ex.Message & vbCrLf & "Error in SQL: " & strSQL)
        End Try

    End Sub

    Private Function getMstID(argType As String, argSearchValue As String) As Integer
        Dim intID As Long, blnFound As Boolean, strSQL As String
        Select Case argType
            Case "UOM"
                intID = clssvt.getDataLongValue("Select UOMID from MstUOM where UOM = '" & argSearchValue & "'", 0, blnFound)
                If Not blnFound Then
                    intID = clssvt.AutoIncrementedNum("UOMID", "MstUOM")
                    strSQL = "insert into MstUOM (UOMID,UOM,Inactive) values("
                    strSQL = strSQL & intID & ","
                    strSQL = strSQL & "'" & argSearchValue & "',"
                    strSQL = strSQL & "'N'" & ")"
                    clssvt.execSQLStatement(strSQL)
                End If

            Case "ItemGroupName"
                intID = clssvt.getDataLongValue("Select ItemGroupID from MstItemGroup where ItemGroupName = '" & argSearchValue & "'", 0, blnFound)
                If Not blnFound Then
                    intID = clssvt.AutoIncrementedNum("ItemGroupID", "MstItemGroup")
                    strSQL = "insert into MstItemGroup (ItemGroupID,ItemGroupName,Inactive) values("
                    strSQL = strSQL & intID & ","
                    strSQL = strSQL & "'" & argSearchValue & "',"
                    strSQL = strSQL & "'N'" & ")"
                    clssvt.execSQLStatement(strSQL)
                End If
        End Select
        getMstID = intID
    End Function

    Private Sub vsfExcel_BeforeEdit(sender As Object, e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfExcel.BeforeEdit
        e.Cancel = True
    End Sub

    Private Sub btnExportToXL_Click(sender As System.Object, e As System.EventArgs) Handles btnExportToXL.Click
        If vsfExcel.Row < 1 Then Exit Sub
        Call ExportToXL(vsfExcel)
    End Sub

    Private Sub fillFldsFromTbl(ByVal argName As String)
        Dim strSQL As String, strSubTblCaption As String
        Dim blnfound As Boolean

        Dim rs As OleDb.OleDbDataReader
        vsfExpTemp.Rows.Count = 1

        strTblName = UCase(Trim(clsSVT.getDataStringValue("Select HdrTblName from UtlTransactions where DocName = '" & argName & "'", "", False)))
        If strTblName = "" Then
            strTblName = UCase(Trim(clsSVT.getDataStringValue("Select HdrTblName from UtlTransactions where HdrTblName = '" & argName & "'", "", False)))
        End If
        If strTblName = "" Then

            If fnTablePresent(argName) Then
                strTblName = argName
            End If
        End If
        If strTblName = "" Then
            MsgBox("Invalid Document!", MsgBoxStyle.Critical)
            Me.Close()
            Exit Sub
        End If

        strDocIDFld = Replace(strTblName, "MST", "")
        strDocIDFld = strDocIDFld & "ID"

        'If strTblName = "MstCur" Then
        '    strDocIDFld = "CURID"
        'End If

        If strTblName = "MSTHDRBOM" Then
            strDocIDFld = "BOMID"
        End If
        'If strTblName = "MSTLOCATION" Then
        '    strDocIDFld = "LOCID"
        'End If

        strSQL = getShowFindQry(strTblName, "")
        rs = clssvt.getDataReaderFromSQL(strSQL)
        For i = 0 To rs.FieldCount - 1
            If UCase(strDocIDFld) <> UCase(rs.GetName(i)) Then
                If DataTypeIsString(rs.GetFieldType(i)) Then
                    If UCase(rs.GetName(i)) <> "IMGPRINT" Then
                        vsfExpTemp.Rows.Add()
                        vsfExpTemp.SetData(vsfExpTemp.Rows.Count - 1, vFldCode, rs.GetName(i))

                        strSubTblCaption = Replace(rs.GetName(i), "CODE", "")
                        strSQL = "select docName from UtlTransactions where HdrTblName = 'MST" & strSubTblCaption & "'"
                        strSubTblCaption = clssvt.getDataStringValue(strSQL, "", blnfound)
                        If blnfound Then
                            vsfExpTemp.SetData(vsfExpTemp.Rows.Count - 1, vSubTblCaption, strSubTblCaption)
                        End If
                    End If
                End If
            End If
        Next
    End Sub

    Private Sub exportTemplate()
        Dim xl As New Excel.Application
        Dim letter As String = "A"
        Dim fn As String = ""
        Dim xlApp As Excel.Application
        Dim xlWorkBook As Excel.Workbook
        Dim xlWorkSheet As Excel.Worksheet
        Dim misValue As Object = System.Reflection.Missing.Value
        Dim i As Integer, a As Integer = 0
        'Dim j As Integer, b As Integer = 0
        Dim lastrow1, lastCol2 As Integer
        Dim SecondFileRange As String = ""

        Try
            xlApp = New Excel.Application
            xlWorkBook = xlApp.Workbooks.Add(misValue)
            xlWorkSheet = xlWorkBook.Sheets("sheet1")

            With vsfExpTemp
                Try
                    For i = 1 To .Rows.Count - 1
                        xlWorkSheet.Cells(1, i) = .GetData(i, vFldCode)
                    Next i
                Catch ex As Exception
                    MsgBox(ex.Message, MsgBoxStyle.Information, "Export to Excel")
                End Try
            End With

            lastrow1 = xlWorkSheet.UsedRange.Rows.Count
            lastCol2 = xlWorkSheet.UsedRange.Columns.Count

            xlWorkSheet.UsedRange.Columns.AutoFit()

            xlApp.Visible = True
            xlApp.UserControl = True

            System.Runtime.InteropServices.Marshal.ReleaseComObject(xlWorkBook)
            System.Runtime.InteropServices.Marshal.ReleaseComObject(xlWorkSheet)
            System.Runtime.InteropServices.Marshal.ReleaseComObject(xlApp)

        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information, "Export to Excel")
        End Try
    End Sub

    Private Sub fillFxdHdrDtls(ByVal argcboMst As String)
        Dim strSQL As String, blnfound As Boolean
        Dim intcol As Integer
        If cboMst.Text = "" Then
            MsgBox("Select Master cannot be blank", MsgBoxStyle.Information, Me.Text)
            Exit Sub
        End If

        strSQL = "Select  DocName from UtlTransactions where doctype = 'Master' and DocName = '" & cboMst.Text & "'"

        Call clssvt.getDataStringValue(strSQL, "", blnfound)
        If Not blnfound Then
            MsgBox(" Selected Master is not found!!", MsgBoxStyle.Information, Me.Text)
            cboMst.Text = ""
            cboMst.Focus()
            Exit Sub
        End If

        vsfExcel.Rows.Count = 1
        Call fillFldsFromTbl(Trim(argcboMst))

        vsfExcel.Cols.Count = vsfExpTemp.Rows.Count
        With vsfExpTemp
            For intcol = 0 To .Rows.Count - 1
                vsfExcel.SetData(0, 0, "SlNo")
                vsfExcel.SetData(0, intcol, GetVal(.GetData(intcol, vFldCode), ""))
            Next intcol
        End With

        vsfExcel.Rows.Fixed = 1
        vsfExcel.Styles.Fixed.Font = New Font("Lucida Sans", vsfExcel.Font.Size, FontStyle.Bold)
        vsfExcel.Styles.Fixed.TextAlign = TextAlignEnum.CenterCenter
        Call vsfExcel.AutoSizeCols()
    End Sub

    Private Sub cboMst_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles cboMst.Validating
        Call fillFxdHdrDtls(Trim(cboMst.Text))
    End Sub

    Private Sub vsfExpTemp_RowColChange(sender As Object, e As System.EventArgs) Handles vsfExpTemp.RowColChange
 
    End Sub
End Class