Imports Microsoft.Office.Interop
Imports C1.Win.C1FlexGrid
Imports System.IO

Public Class frmUtlItemImport
    Dim intSelRow As Integer '# A - Starting row
    Dim strErrorMsg As String
    Dim blnValidData As Boolean
    Dim blnfirstload As Boolean
    Dim strRights As String
    Dim intDocID As Integer, intMenuID As Integer
    Dim intGrpID As Integer

    Dim strFileName As String
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

    Const vsfExSLNO = 0
    Const vsfExManualCode = 1
    Const vsfExItemCode = 2
    Const vsfExItemNameGeneral = 3
    Const vsfExItemNameInvoice = 4
    Const vsfEXDepName = 5
    Const vsfExCategory = 6
    Const vsfExBrand = 7
    Const vsfExSubGrp = 8
    Const vsfExCustomSize = 9
    Const vsfExCustSizeType = 10
    Const vsfExMPU = 11
    Const vsfExEU = 12
    Const vsfExConvFact = 13
    Const vsfExBU = 14
    Const vsfExConvFactInv = 15
    Const vsfExFactoryCode = 16
    Const vsfExPODesc = 17
    Const vsfExDisplayPanel = 18
    ' Const vsfExVATPer = 19
    Const vsfExMRP = 19
    Const vsfExNotes = 20
    Const vsfExInclTAX = 21
    Const vsfCGST = 22
    Const vsfSGST = 23
    Const vsfIGST = 24
    Const vsfHSN = 25
    Const vsfHSNDesc = 26

    Const vsfExTotCols = 27
    Dim vExErr As Integer
    Dim vExBlnErr As Integer

    Private Sub frmUtlImportItemFmExcel_KeyUp(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyUp
        If e.KeyCode = Keys.F9 And e.Control = True Then
            Call initFormSize(Me, Me.Text, False, 930, 646, 0, 0)
            MsgBox("Restored Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
        If e.KeyCode = Keys.F8 And e.Control = True Then
            Call SaveActFormSettings(Me, Me.Text)
            Call SaveActFormVsfDet(Me, Me.Text, "Excel", vsfExcel)
            MsgBox("Saved Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
    End Sub

    Private Sub frmUtlItemImport_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Call initScreen()
        Call enablebtn(False)
        Call EnableFields(False, 1)
        Call initGridStyles(Me)

        vsfExcel.Rows.Fixed = 1
        vsfExcel.Styles.Fixed.Font = New Font("Lucida Sans", vsfExcel.Font.Size, FontStyle.Bold)
        vsfExcel.Styles.Fixed.TextAlign = TextAlignEnum.CenterCenter
        Call vsfExcel.AutoSizeCols()

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
        Dim blnFnd As Boolean
        Cdlg.DefaultExt = "xlsx"
        Cdlg.Filter = "Excel File (*.xlsx)|*.xlsx"
        Cdlg.FileName = "*.xlsx"

        If Cdlg.ShowDialog() <> DialogResult.OK Then
            Return
            Exit Sub
        End If
        txtXLFilePath.Text = Cdlg.FileName
        strFileName = Cdlg.SafeFileName
        Call EnableFields(False, 1)
        btnImportFromXL.Enabled = True
        vsfExcel.Cols.Count = vsfExTotCols
        clssvt.getDataStringValue("select ItemRmks from MstItem where ItemRmks ='" & strFileName & "'", "", blnFnd)
        If blnFnd Then
            MsgBox("Some items has been already imported with the XL file having this name." & _
                  " Please change the XL file name, if it contains Different data to import.", vbExclamation, Me.Text)
            Exit Sub
        End If

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

        Dim blnError As Boolean
        blnError = False

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

            ' Call EnableFields(True, 1)
        Else
            'Me.ToolTip1.SetToolTip(Me.vsfDet, "")
            Call EnableFields(True, 1)
            lblError.Visible = False
            btnRemErr.Visible = False
            Call enablebtn(True)
        End If

        txtNoOfInv.Text = vsfExcel.Rows.Count - 1


        For i = 1 To vsfExcel.Rows.Count - 1
            If vsfExcel.GetData(i, vExBlnErr) = "Y" Then
                blnError = True
            End If
        Next

        If blnError Then
            btnSave.Enabled = False
        Else
            btnSave.Enabled = True
        End If

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
            If Trim(UCase(strExcelCaption)) <> UCase(strCaption) Then
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
                        strExcelCaption = (Trim(xlapp.ActiveSheet.Range(Chr(64 + intcol) & i).Value))

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
            vsfExcel.Cols.Add()
            vExErr = .Cols.Count - 1
            vsfExcel.Cols.Add()
            vExBlnErr = .Cols.Count - 1
            .SetData(0, vExErr, "Errors")
            .SetData(0, vExBlnErr, "BlnErrorFound")
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
        Dim i As Integer, strerr As String = "", blnfound As Boolean

        blnValidData = False
        lblError.Visible = False
        btnRemErr.Visible = False

        ValidateEntry = False


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

                If vsfExcel.GetData(i, vsfExManualCode) = "Y" Then
                    If vsfExcel.GetData(i, vsfExItemCode) <> "" Then
                        Call clssvt.getDataLongValue("select ItemID from MstItem where ItemCode ='" & vsfExcel.GetData(i, vsfExItemCode) & "'", 0, blnfound)
                        If blnfound Then
                            strerr = strerr & IIf(i <> 1, vbLf, "") & " " & vsfExcel.GetData(0, vsfExItemCode) & " already exists "
                            vsfExcel.SetData(i, vExErr, strerr)
                            vsfExcel.SetData(i, vExBlnErr, "Y")
                            strerr = ""
                        End If
                    ElseIf vsfExcel.GetData(i, vsfExItemCode) = "" Then
                        vsfExcel.SetData(i, vExErr, " ""ItemCode could not be Null if ManualCode is 'Y'""")
                        vsfExcel.SetData(i, vExBlnErr, "Y")
                        blnValidData = True
                    End If
                ElseIf vsfExcel.GetData(i, vsfExManualCode) = "N" Then
                    vsfExcel.SetData(i, vsfExItemCode, "")
                Else
                    vsfExcel.SetData(i, vExErr, " ""ManualCode should either be 'Y' or 'N'""")
                    blnValidData = True
                End If

                If vsfExcel.GetData(i, vsfExItemNameGeneral) = "" Then
                    vsfExcel.SetData(i, vExErr, vsfExcel.GetData(i, vExErr) & " ""ItemNameGeneral cannot be Null""")
                    blnValidData = True
                End If

                If vsfExcel.GetData(i, vsfExItemNameInvoice) = "" Then
                    vsfExcel.SetData(i, vExErr, vsfExcel.GetData(i, vExErr) & " ""ItemNameInvoice cannot be Null")
                    blnValidData = True
                End If

                intGrpID = clssvt.getDataLongValue("select GrpID from MstIGroup where" & _
                            " DeptName='" & vsfExcel.GetData(i, vsfEXDepName) & "'" & _
                            " and CatName='" & vsfExcel.GetData(i, vsfExCategory) & "'" & _
                            " and BrandName='" & vsfExcel.GetData(i, vsfExBrand) & "'" & _
                            " and SubGroupName ='" & vsfExcel.GetData(i, vsfExSubGrp) & "'", 0, blnfound)
                If Not blnfound Then
                    strerr = strerr & IIf(i <> 1, vbLf, "") & " Dept + Cat + Brand + SubGroup does not exist in Item Group Master"
                    vsfExcel.SetData(i, vExErr, vsfExcel.GetData(i, vExErr) & " """ & strerr & """")
                    vsfExcel.SetData(i, vExBlnErr, "Y")
                    strerr = ""
                    blnValidData = True
              
                End If

                Call clssvt.getDataLongValue("select UOMID from MstUOM where UOMCode ='" & vsfExcel.GetData(i, vsfExMPU) & "'", 0, blnfound)
                If Not blnfound Then
                    vsfExcel.SetData(i, vExErr, vsfExcel.GetData(i, vExErr) & " ""Not valid MPUOMCode""")
                    .SetData(i, vExBlnErr, "Y")
                    blnValidData = True
                End If

                Call clssvt.getDataLongValue("select UOMID from MstUOM where UOMCode ='" & vsfExcel.GetData(i, vsfExEU) & "'", 0, blnfound)
                If Not blnfound Then
                    vsfExcel.SetData(i, vExErr, vsfExcel.GetData(i, vExErr) & " ""Not valid EUOMCode""")
                    .SetData(i, vExBlnErr, "Y")
                    blnValidData = True
                End If


                If vsfExcel.GetData(i, vsfExCustomSize) = "Y" Then
                    If UCase(vsfExcel.GetData(i, vsfExCustSizeType)) = "INCH" Then
                    ElseIf UCase(vsfExcel.GetData(i, vsfExCustSizeType)) = "METER" Then
                    Else
                        .SetData(i, vExErr, vsfExcel.GetData(i, vExErr) & " ""CustomSizeType should be either INCH or METER""")
                        .SetData(i, vExBlnErr, "Y")
                        blnValidData = True
                    End If
                    If vsfExcel.GetData(i, vsfExConvFact) <> "" Then
                        vsfExcel.SetData(i, vExErr, vsfExcel.GetData(i, vExErr) & " ""ConvFact should not be entered when CustSize is 'Y'""")
                        .SetData(i, vExBlnErr, "Y")
                        blnValidData = True
                    End If
                    If vsfExcel.GetData(i, vsfExConvFactInv) <> "" Then
                        vsfExcel.SetData(i, vExErr, vsfExcel.GetData(i, vExErr) & " ""ConvFactInv should not be entered when CustSize is 'Y'""")
                        .SetData(i, vExBlnErr, "Y")
                        blnValidData = True
                    End If

                ElseIf vsfExcel.GetData(i, vsfExCustomSize) = "N" Then
                    If vsfExcel.GetData(i, vsfExCustSizeType) <> "" Then
                        .SetData(i, vExErr, vsfExcel.GetData(i, vExErr) & " ""CustomSizeType should not be entered when CustSize is 'N'""")
                    End If
                    If Val(vsfExcel.GetData(i, vsfExConvFactInv)) <= 0 Then
                        vsfExcel.SetData(i, vExErr, vsfExcel.GetData(i, vExErr) & " ""ConvFactInv should be greater than 0""")
                        .SetData(i, vExBlnErr, "Y")
                        blnValidData = True
                    End If
                    If vsfExcel.GetData(i, vsfExMPU) = vsfExcel.GetData(i, vsfExEU) Then
                        If Val(vsfExcel.GetData(i, vsfExConvFactInv)) > 1 Then
                            vsfExcel.SetData(i, vExErr, vsfExcel.GetData(i, vExErr) & " ""ConvFact should not exceed 1""")
                            .SetData(i, vExBlnErr, "Y")
                            blnValidData = True
                        End If
                    ElseIf vsfExcel.GetData(i, vsfExMPU) <> vsfExcel.GetData(i, vsfExEU) Then
                        If Val(vsfExcel.GetData(i, vsfExConvFact)) <= 1 Then
                            vsfExcel.SetData(i, vExErr, vsfExcel.GetData(i, vExErr) & " ""ConvFact should be greater than 1""")
                            .SetData(i, vExBlnErr, "Y")
                            blnValidData = True
                        End If
                    End If
                Else
                    vsfExcel.SetData(i, vExErr, vsfExcel.GetData(i, vExErr) & " ""CustomSize should be either 'Y' or 'N'""")
                    .SetData(i, vExBlnErr, "Y")
                    blnValidData = True
                End If

                Call clssvt.getDataLongValue("select UOMID from MstUOM where UOMCode ='" & vsfExcel.GetData(i, vsfExBU) & "'", 0, blnfound)
                If Not blnfound Then
                    vsfExcel.SetData(i, vExErr, vsfExcel.GetData(i, vExErr) & " ""Not valid BUOMCode""")
                    .SetData(i, vExBlnErr, "Y")
                    blnValidData = True
                End If


                If vsfExcel.GetData(i, vsfExDisplayPanel) = "Y" Then
                ElseIf vsfExcel.GetData(i, vsfExDisplayPanel) = "N" Then
                Else
                    vsfExcel.SetData(i, vExErr, vsfExcel.GetData(i, vExErr) & " ""DisplayPanel should either 'Y' or 'N'""")
                    .SetData(i, vExBlnErr, "Y")
                    blnValidData = True
                End If

                If vsfExcel.GetData(i, vsfExInclTAX) = "Y" Then
                ElseIf vsfExcel.GetData(i, vsfExInclTAX) = "N" Then
                Else
                    vsfExcel.SetData(i, vExErr, vsfExcel.GetData(i, vExErr) & " ""InclTAX should either 'Y' or 'N'""")
                    .SetData(i, vExBlnErr, "Y")
                    blnValidData = True
                End If






                vsfExcel.SetData(i, vsfExItemCode, Replace(vsfExcel.GetData(i, vsfExItemCode), "'", "`"))
                vsfExcel.SetData(i, vsfExItemNameGeneral, Replace(vsfExcel.GetData(i, vsfExItemNameGeneral), "'", "`"))
                vsfExcel.SetData(i, vsfExItemNameInvoice, Replace(vsfExcel.GetData(i, vsfExItemNameInvoice), "'", "`"))
                vsfExcel.SetData(i, vsfExFactoryCode, Replace(vsfExcel.GetData(i, vsfExFactoryCode), "'", "`"))
                vsfExcel.SetData(i, vsfExPODesc, Replace(vsfExcel.GetData(i, vsfExPODesc), "'", "`"))
                vsfExcel.SetData(i, vsfExNotes, Replace(vsfExcel.GetData(i, vsfExNotes), "'", "`"))
                vsfExcel.SetData(i, vsfHSN, Replace(vsfExcel.GetData(i, vsfHSN), "'", "`"))
                vsfExcel.SetData(i, vsfHSNDesc, Replace(vsfExcel.GetData(i, vsfHSNDesc), "'", "`"))
                lblStatus.Text = "Validating " & vsfExcel.GetData(i, vsfExSLNO) & " -- " & i & " / " & txtNoOfInv.Text
            Next i


            If clssvt.ChkDuplicateData(vsfExcel, i, vsfExItemCode) Then
                strerr = strerr & IIf(i <> 1, vbLf, "") & " " & .GetData(0, vsfExItemCode) & " duplication of data "
                .SetData(i, vExErr, vsfExcel.GetData(i, vExErr) & "; " & strerr)
                .SetData(i, vExBlnErr, "Y")
                blnValidData = True
                'Exit Function
            End If
        End With

        lblStatus.Visible = False
        vsfExcel.Cols(vExErr).Visible = False
        vsfExcel.Cols(vExBlnErr).Visible = False

        If blnValidData Then
            vsfExcel.Cols(vExErr).Visible = True
            vsfExcel.Cols(vExBlnErr).Visible = True
            Call enablebtn(True)
            Exit Function
        End If
    End Function

    'Private Function getItemPresent() As Boolean
    '    getItemPresent = False
    '    Dim strSQL As String
    '    Dim rs As OleDb.OleDbDataReader, strItemCode As String, strHotkey As String
    '    Dim strerr As String = ""
    '    Dim cs As C1.Win.C1FlexGrid.CellStyle = vsfExcel.Styles.Add("ItemCode")
    '    strSQL = "select * from MstItem"

    '    rs = clssvt.getDataReaderFromSQL(strSQL)

    '    With rs
    '        Do While .Read
    '            For i = 1 To vsfExcel.Rows.Count - 1
    '                strItemCode = vsfExcel.GetData(i, vsfExItemCode)
    '                If !ItemCode = strItemCode Then
    '                    cs.BackColor = Color.Red
    '                    blnValidData = False
    '                    strerr = strerr & IIf(i <> 1, vbLf, "") & " " & vsfExcel.GetData(0, vsfExItemCode) & " already exists "
    '                    vsfExcel.SetData(i, vExErr, strerr)
    '                    vsfExcel.SetData(i, vExBlnErr, "Y")
    '                    vsfExcel.SetCellStyle(i, vsfExItemCode, cs)
    '                    strerr = ""
    '                End If
    '                strHotkey = vsfExcel.GetData(i, vsfExHotKey)
    '                If strHotkey <> "" Then
    '                    If GetVal(!Hotkey, "") = strHotkey Then
    '                        strerr = strerr & IIf(i <> 1, vbLf, "") & " " & vsfExcel.GetData(0, vsfExHotKey) & " should be unique "
    '                        vsfExcel.SetData(i, vExErr, strerr)
    '                        vsfExcel.SetData(i, vExBlnErr, "Y")
    '                        cs.BackColor = Color.Red
    '                        blnValidData = False
    '                        vsfExcel.SetCellStyle(i, vsfExHotKey, cs)
    '                        strerr = ""
    '                    End If
    '                End If
    '            Next
    '        Loop
    '    End With

    '    getItemPresent = True
    'End Function

    Private Sub btnExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
  

        Call exportTemplate()
   
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

        Dim i As Integer, strSQL As String = "", intID As Long
        Dim strFlds As String = ""
        Dim strFldvalue As String = ""
        
        Try
            For i = 1 To vsfExcel.Rows.Count - 1
                If vsfExcel.GetData(i, vExBlnErr) <> "Y" Then
                    intID = clssvt.AutoIncrementedNum("ItemID", "MstItem")
                    strSQL = "Insert into MstItem ("
                    strSQL = strSQL & "ItemID, GrpId, AddedBy, ChangedBy,ITEMRMKS, ItemCode, ItemNameGeneral, ItemNameInvoice,MPUOMID,EUOMID,ConvFact,InvMPUOMID,ConvFactInv,"
                    strSQL = strSQL & " FctryCode,PODesc,DispPnl,MRP,IsCustomSize,CustomSizeType,Notes,INCLTAX,CGSTPER,SGSTPER,IGSTPER,HSNSACCODE,HSNSACNAME"
                    strSQL = strSQL & ") values ("

                    strSQL = strSQL & intID & ","

                    intGrpID = clssvt.getDataLongValue("select GrpID from MstIGroup where" & _
                          " DeptName='" & vsfExcel.GetData(i, vsfEXDepName) & "'" & _
                          " and CatName='" & vsfExcel.GetData(i, vsfExCategory) & "'" & _
                          " and BrandName='" & vsfExcel.GetData(i, vsfExBrand) & "'" & _
                          " and SubGroupName ='" & vsfExcel.GetData(i, vsfExSubGrp) & "'", 0, False)
                    If intGrpID = 0 Then
                        MsgBox("Item In Row:" & CStr(i) & " does not contain valid Item Group... Next Item will be started to import..!", MsgBoxStyle.Information, Me.Text)
                        GoTo NextItem
                    End If

                    strSQL = strSQL & intGrpID & ","
                    strSQL = strSQL & "'" & gUserCode & "',"
                    strSQL = strSQL & "'" & gUserCode & "',"
                    strSQL = strSQL & "'" & strFileName & "',"
                    If vsfExcel.GetData(i, vsfExManualCode) = "Y" Then
                        strSQL = strSQL & "'" & Trim(vsfExcel.GetData(i, vsfExItemCode)) & "',"
                    Else
                        strSQL = strSQL & "'" & clssvt.getDataStringValue("Exec Sp_GenerateItemCode @GrpId=" & intGrpID, "", False) & "',"
                    End If
                    strSQL = strSQL & "'" & Trim(vsfExcel.GetData(i, vsfExItemNameGeneral)) & "',"
                    strSQL = strSQL & "'" & Trim(vsfExcel.GetData(i, vsfExItemNameInvoice)) & "',"
                    strSQL = strSQL & clssvt.getDataLongValue("Select UOMID from MstUOM where UOMCode ='" & vsfExcel.GetData(i, vsfExMPU) & "'", 0, False) & ","
                    strSQL = strSQL & clssvt.getDataLongValue("Select UOMID from MstUOM where UOMCode ='" & vsfExcel.GetData(i, vsfExEU) & "'", 0, False) & ","
                    If vsfExcel.GetData(i, vsfExCustomSize) = "Y" Then
                        strSQL = strSQL & "0,"
                    Else
                        strSQL = strSQL & Val(vsfExcel.GetData(i, vsfExConvFact)) & ","
                    End If
                    strSQL = strSQL & clssvt.getDataLongValue("Select UOMID from MstUOM where UOMCode ='" & vsfExcel.GetData(i, vsfExBU) & "'", 0, False) & ","
                    If vsfExcel.GetData(i, vsfExCustomSize) = "Y" Then
                        strSQL = strSQL & "0,"
                    Else
                        strSQL = strSQL & Val(vsfExcel.GetData(i, vsfExConvFactInv)) & ","
                    End If
                    strSQL = strSQL & "'" & Trim(vsfExcel.GetData(i, vsfExFactoryCode)) & "',"
                    strSQL = strSQL & "'" & Trim(vsfExcel.GetData(i, vsfExPODesc)) & "',"
                    strSQL = strSQL & "'" & Trim(vsfExcel.GetData(i, vsfExDisplayPanel)) & "',"
                    ' strSQL = strSQL & Val(vsfExcel.GetData(i, vsfExVATPer)) & ","
                    strSQL = strSQL & Val(vsfExcel.GetData(i, vsfExMRP)) & ","

                    If vsfExcel.GetData(i, vsfExCustomSize) = "Y" Then
                        strSQL = strSQL & "'Y',"
                        If UCase(vsfExcel.GetData(i, vsfExCustSizeType)) = "INCH" Then
                            strSQL = strSQL & "'inch',"
                        ElseIf UCase(vsfExcel.GetData(i, vsfExCustSizeType)) = "METER" Then
                            strSQL = strSQL & "'meter',"
                        Else
                            strSQL = strSQL & "'',"
                        End If

                    Else
                        strSQL = strSQL & "'N',"
                        strSQL = strSQL & "'',"
                    End If



                    strSQL = strSQL & "'" & Trim(vsfExcel.GetData(i, vsfExNotes)) & "',"

                    If vsfExcel.GetData(i, vsfExInclTAX) = "Y" Then
                        strSQL = strSQL & " 'Y',"
                    Else
                        strSQL = strSQL & " 'N',"
                    End If

                    strSQL = strSQL & Val(vsfExcel.GetData(i, vsfCGST)) & ","
                    strSQL = strSQL & Val(vsfExcel.GetData(i, vsfSGST)) & ","
                    strSQL = strSQL & Val(vsfExcel.GetData(i, vsfIGST)) & ","
                    strSQL = strSQL & "'" & GetVal(vsfExcel.GetData(i, vsfHSN), "") & "',"
                    strSQL = strSQL & "'" & GetVal(vsfExcel.GetData(i, vsfHSNDesc), "") & "'"
                    strSQL = strSQL & ")"
                    clssvt.execSQLStatement(strSQL)

                End If
NextItem:
            Next
            MsgBox("Imported Data Successfully!", vbInformation, Me.Text)

            vsfExcel.Cols(vExErr).Visible = False
            vsfExcel.Cols(vExBlnErr).Visible = False
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

        strTblName = UCase(Trim(clssvt.getDataStringValue("Select HdrTblName from UtlTransactions where DocName = '" & argName & "'", "", False)))
        If strTblName = "" Then
            strTblName = UCase(Trim(clssvt.getDataStringValue("Select HdrTblName from UtlTransactions where HdrTblName = '" & argName & "'", "", False)))
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

            With vsfExcel
                Try
                    For i = 1 To .Cols.Count - 1
                        xlWorkSheet.Cells(1, i) = .GetData(0, i)
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

    Private Sub initScreen()
        With vsfExcel
            .Rows.Count = 1
            .Cols.Count = vsfExTotCols
            .SetData(0, vsfExSLNO, "SLNO")
            .SetData(0, vsfExManualCode, "ManualCode")
            .SetData(0, vsfExItemCode, "ItemCode")
            .SetData(0, vsfExItemNameGeneral, "ItemNameGeneral")
            .SetData(0, vsfExItemNameInvoice, "ItemNameInvoice")
            .SetData(0, vsfEXDepName, "DepartmentName")
            .SetData(0, vsfExCategory, "Category")
            .SetData(0, vsfExBrand, "Brand")
            .SetData(0, vsfExSubGrp, "Sub-Group")
            .SetData(0, vsfExCustomSize, "CustomSize")
            .SetData(0,vsfExCustSizeType ,"CustomSizeType")
            .SetData(0, vsfExMPU, "MPU")
            .SetData(0, vsfExEU, "EU")
            .SetData(0, vsfExConvFact, "ConvFact")
            .SetData(0, vsfExBU, "BU")
            .SetData(0, vsfExConvFactInv, "ConvFactInv")
            .SetData(0, vsfExFactoryCode, "FactoryCode")
            .SetData(0, vsfExPODesc, "PODesc")
            .SetData(0, vsfExDisplayPanel, "DislayPanel")
            '.SetData(0, vsfExVATPer, "VAT%")
            .SetData(0, vsfExMRP, "MRP")
            .SetData(0, vsfExNotes, "Notes")
            .SetData(0, vsfExInclTAX, "InclTAX")
            .SetData(0, vsfCGST, "CGST%")
            .SetData(0, vsfSGST, "SGST%")
            .SetData(0, vsfIGST, "IGST%")
            .SetData(0, vsfHSN, "HSN CODE")
            .SetData(0, vsfHSNDesc, "HSN DESC")

        End With
    End Sub
End Class