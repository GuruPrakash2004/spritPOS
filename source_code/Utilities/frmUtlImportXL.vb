Imports Microsoft.Office.Interop
Imports C1.Win.C1FlexGrid
Imports ClosedXML.Excel


Public Class frmUtlImportXL
    Dim intSelRow As Integer '# A - Starting row
    Dim strErrorMsg As String
    Dim blnValidData As Boolean
    Dim blnfirstload As Boolean
    Dim strRights As String
    Dim intMenuID As Integer
    Dim intGrpID As Integer

    Dim strFileName As String
    Dim strFldr As String
    Dim strTemp As String
    Dim clssvt As New svtLibrary
    Dim strDocIDFld As String
    Dim strTblName As String
    Dim strSQL As String

    Dim svtTrn As oledb.oledbTransaction

    Const vTblName = 0
    'Const vTblCaption = 1
    'Const vTblLinkCond = 2
    Const vFldCode = 1
    Const vSubTblCaption = 2


    'Consts for vsfExcel rows
    Const vXLRowWarning = 0
    Const vXLRowImpInfo = 1
    Const vXLRowCRC = 2
    Const vXLRowFldName = 3
    Const vXLRowDataType = 4
    Const vXLRowMandatory = 5
    Const vXLRowFrgnKey = 6
    Const vXLRowMaxLen = 7
    Const vXLRowUnique = 8
    Const vXLRowExample = 9
    Const vXLRowDefaultVal = 10

    Const intXLRows = 20

    'Consts for vsfExcel Cols
    Const vXLColRowName = 0
    Const vXLColBlnErr = 1
    Const vXLColErrDesc = 2
    Const vXLColStatus = 3

    Const intXLCols = 10


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

    Dim blnNonTemplateColumns As Boolean
    Dim blnNonTemplateNonColumns As Boolean

    Const vNXlFld = 0
    Const vNXlMapFld = 1

    Const vNXlTotal = 2
    Dim intXLNonTemplateCols As Int32

    Dim strHdrTblName As String, strDocType As String, intDocID As Integer, strMstAutoGenCode As String

    Private Sub frmUtlImportItemFmExcel_KeyUp(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyUp
        If e.KeyCode = Keys.F9 And e.Control = True Then
            Call initFormSize(Me, Me.Text, False, 1150, 675, 0, 0)
            MsgBox("Restored Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
        If e.KeyCode = Keys.F8 And e.Control = True Then
            Call SaveActFormSettings(Me, Me.Text)
            Call SaveActFormVsfDet(Me, Me.Text, "Excel", vsfXL)
            MsgBox("Saved Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
    End Sub

    Private Sub frmUtlItemImport_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Call initScreen()
        Call enablebtn(False)
        Call EnableFields(False, 1)
        Call initGridStyles(Me)

        vsfXL.Rows.Fixed = intXLRows
        'vsfXL.Styles.Fixed.Font = New Font("Lucida Sans", vsfXL.Font.Size, FontStyle.Bold)
        'vsfXL.Styles.Fixed.TextAlign = TextAlignEnum.CenterCenter
        Call vsfXL.AutoSizeCols()
        btnGenTemplate.Enabled = False
        cboDoc.Focus()
    End Sub

    Private Sub wrapGrid()
        With vsfXL
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
        strFileName = Cdlg.SafeFileName
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
        'If chkClientXL.Checked Then
        '    pnlImportClientXL.visible = True
        '    Exit Sub
        'End If
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



        If Not chkClientXL.Checked Then
            If gBlnMSOffice Then
                Call importFromXL(Trim(txtXLFilePath.Text))
            Else
                Call importFromXL_ClosedSXML(Trim(txtXLFilePath.Text))
            End If

        Else
            vsf.Rows.Count = 1
            vsf.Cols.Count = vNXlTotal
            pnlImportClientXL.Visible = True
        End If

        'If blnValidData Then
        '    Call EnableFields(True, 2)
        '    lblError.Visible = True
        '    enablebtn(True)
        'Else
        '    Call EnableFields(True, 1)
        '    lblError.Visible = False
        '    btnRemErr.Visible = False
        '    Call enablebtn(True)
        'End If
    End Sub

    Private Sub importFromXL(ByVal argFileName As String)
        Dim xlapp As New Excel.Application
        Dim xlSheet As New Excel.Worksheet
        Dim i As Integer, j As Integer, str As String = ""
        Dim strExcelCaption As String, strCaption As String = ""
        Dim introw As Integer = 0
        Dim intcol As Integer = 0
        xlapp.Workbooks.Open(argFileName, , True)
        'Call InitScreenINV

        xlSheet = xlapp.Sheets(1)

        If xlSheet.Name <> "DataSheet" Then
            MsgBox("Please select the valid Excel file, as the first worksheet should be named as 'DataSheet'!", vbInformation, Me.Text)
            Exit Sub
        End If

        blnfirstload = False
        With xlapp.Application
            intcol = vsfXL.Cols.Count - 1
            strExcelCaption = ""

            For i = intXLCols + 1 To vsfXL.Cols.Count - 1
                strExcelCaption = ""
                For j = vXLRowFldName + 1 To vXLRowDefaultVal
                    'MsgBox(UCase(Trim(xlapp.ActiveSheet.cells(j, i).Value)))
                    If j = vXLRowDefaultVal + 1 Then Exit For
                    strExcelCaption = strExcelCaption & (Trim(xlapp.ActiveSheet.cells(j, i).Value))
                Next
                strExcelCaption = getCheckSum(strExcelCaption)
                If vsfXL.GetData(vXLRowCRC, i - 1) <> strExcelCaption Then
                    MsgBox("Please select a valid template", MsgBoxStyle.Information, Me.Text)
                    Exit Sub
                End If
            Next

            vsfXL.Rows.Count = intXLRows
            i = intXLRows + 1 'ASSIGNING THE FIRST ROW
            j = intXLCols + 1
            Try
                Do While Trim(.Cells(i, j).Value).ToString <> ""
                    vsfXL.Rows.Count = vsfXL.Rows.Count + 1
                    For intcol = j To vsfXL.Cols.Count
                        vsfXL.SetData(i - 1, intcol - 1, Replace(Replace(Trim(.Cells(i, j).Value), "'", "`"), Chr(10), ""))
                        j = j + 1
                    Next intcol

                    lblRecNo.Text = i - intXLRows
                    i = i + 1
                    j = intXLCols + 1
                    vsfXL.TopRow = vsfXL.Rows.Count - 1
                Loop
                StMsg("Data Loaded, now Validating the data")
                .Workbooks(1).Close(SaveChanges:=False)
            Catch ex As Exception
                MsgBox("Cannot export to Excel." & vbCrLf & ex.Message, MsgBoxStyle.Critical, "Import Error")
            End Try
        End With

        With vsfXL
            .Cols(vXLColBlnErr).Visible = True
            .Cols(vXLColErrDesc).Visible = True
            .Cols(vXLColStatus).Visible = True
            .SetData(vXLRowFldName, vXLColBlnErr, "Errors")
            .SetData(vXLRowFldName, vXLColErrDesc, "Err. Desc")
            .SetData(vXLRowFldName, vXLColStatus, "Status")
        End With

        'Validating the data here - begins
        Call ValidateEntry()
        vsfXL.AutoSizeCols()
        StMsg("")


        vsfXL.Redraw = True

        'Call wrapGrid()
        Exit Sub
    End Sub

    Private Sub importFromXL_ClosedSXML(ByVal argFileName As String)
        Try


            Dim xlWB As New XLWorkbook(argFileName)
            Dim xlWS As IXLWorksheet = xlWB.Worksheet(1)
            Dim i As Integer, j As Integer, str As String = ""
            Dim strExcelCaption As String, strCaption As String = ""
            Dim introw As Integer = 0
            Dim intcol As Integer = 0


            If xlWS.Name <> "DataSheet" Then
                MsgBox("Please select the valid Excel file, as the first worksheet should be named as 'DataSheet'!", vbInformation, Me.Text)
                Exit Sub
            End If

            blnfirstload = False
            With xlWS
                intcol = vsfXL.Cols.Count - 1
                strExcelCaption = ""

                For i = intXLCols + 1 To vsfXL.Cols.Count - 1
                    strExcelCaption = ""
                    For j = vXLRowFldName + 1 To vXLRowDefaultVal
                        'MsgBox(UCase(Trim(xlapp.ActiveSheet.cells(j, i).Value)))
                        If j = vXLRowDefaultVal + 1 Then Exit For
                        strExcelCaption = strExcelCaption & (Trim(xlWS.Cell(j, i).Value))
                    Next
                    strExcelCaption = getCheckSum(strExcelCaption)
                    If vsfXL.GetData(vXLRowCRC, i - 1) <> strExcelCaption Then
                        MsgBox("Please select a valid template", MsgBoxStyle.Information, Me.Text)
                        Exit Sub
                    End If
                Next

                vsfXL.Rows.Count = intXLRows
                i = intXLRows + 1 'ASSIGNING THE FIRST ROW
                j = intXLCols + 1
                Try
                    Do While Trim(xlWS.Cell(i, j).Value).ToString <> ""
                        vsfXL.Rows.Count = vsfXL.Rows.Count + 1
                        For intcol = j To vsfXL.Cols.Count
                            vsfXL.SetData(i - 1, intcol - 1, Replace(Replace(Trim(xlWS.Cell(i, j).Value), "'", "`"), Chr(10), ""))
                            j = j + 1
                        Next intcol

                        lblRecNo.Text = i - intXLRows
                        i = i + 1
                        j = intXLCols + 1
                        vsfXL.TopRow = vsfXL.Rows.Count - 1
                    Loop
                    StMsg("Data Loaded, now Validating the data")
                    '.Workbooks(1).Close(SaveChanges:=False)
                Catch ex As Exception
                    MsgBox("Cannot export to Excel." & vbCrLf & ex.Message, MsgBoxStyle.Critical, "Import Error")
                End Try
            End With

            With vsfXL
                .Cols(vXLColBlnErr).Visible = True
                .Cols(vXLColErrDesc).Visible = True
                .Cols(vXLColStatus).Visible = True
                .SetData(vXLRowFldName, vXLColBlnErr, "Errors")
                .SetData(vXLRowFldName, vXLColErrDesc, "Err. Desc")
                .SetData(vXLRowFldName, vXLColStatus, "Status")
            End With

            'Validating the data here - begins
            Call ValidateEntry()
            vsfXL.AutoSizeCols()
            StMsg("")


            vsfXL.Redraw = True

            'Call wrapGrid()
            Exit Sub
        Catch ex As Exception
            MsgBox("Closed XML... " & ex.Message, MsgBoxStyle.Critical, Me.Text)
        End Try
    End Sub

    Private Function ValidateEntry() As Boolean
        Dim i As Integer, strerr As String = "", blnfound As Boolean

        blnValidData = False
        lblError.Visible = False
        btnRemErr.Visible = False

        ValidateEntry = False


        With vsfXL
            If vsfXL.Rows.Count < intXLRows + 1 Then
                MsgBox("There is no data exists for importing", vbInformation + vbOKOnly, Me.Text)
                Exit Function
            End If

            For i = intXLRows To .Rows.Count - 1
                .TopRow = i
                'DoEvents()
                strerr = "" 'Refreshing the err messg - each time
                vsfXL.SetData(i, vXLColBlnErr, strerr)
                vsfXL.SetData(i, vXLColErrDesc, "")
                vsfXL.SetData(i, vXLColStatus, "")

                For j = intXLCols To .Cols.Count - 1

                    If UCase(Trim(.GetData(vXLRowUnique, j))) = "Y" Then
                        clssvt.getDataStringValue("select " & UCase(Trim(.GetData(vXLRowFldName, j))) & " from " & strHdrTblName & " where " & UCase(Trim(.GetData(vXLRowFldName, j))) & " = '" & UCase(Trim(.GetData(i, j))) & "'", "", blnfound, svtTrn)
                        If blnfound Then
                            'strerr = Replace(strHdrTblName, "MST", " ") & "CODE" & " already exists."
                            strerr = UCase(Trim(.GetData(vXLRowFldName, j))) & " already exists ;"
                            vsfXL.SetData(i, vXLColErrDesc, strerr)
                        End If
                        '
                    End If
                    'If UCase(Trim(.GetData(vXLRowMandatory, j))) = "Y" Then
                    '    If UCase(Trim(vsfXL.GetData(i, j))) = "" Then
                    '        ' If blnfound Then
                    '        'strerr = Replace(strHdrTblName, "MST", " ") & "CODE" & " already exists."
                    '        strerr = UCase(Trim(.GetData(vXLRowFldName, j))) & " is Mandatory."
                    '        vsfXL.SetData(i, vXLColErrDesc, strerr)
                    '    End If

                    'End If

                    Select Case UCase(Trim(.GetData(vXLRowDataType, j)))
                        Case "TEXT"

                            If UCase(Trim(.GetData(vXLRowMandatory, j))) = "Y" Then
                                If UCase(Trim(.GetData(i, j))) = "" AndAlso GetVal(.GetData(vXLRowDefaultVal, j), "") = "" Then
                                    If strerr <> "" Then
                                        strerr = strerr & " Mandatory Field should not be empty.;"
                                    Else
                                        strerr = " Mandatory Field should not be empty.;"
                                    End If
                                    vsfXL.SetData(i, vXLColErrDesc, strerr)
                                End If
                            End If
                            If Len(UCase(Trim(.GetData(i, j)))) > Val(Trim(.GetData(vXLRowMaxLen, j))) Then
                                If strerr <> "" Then
                                    strerr = strerr & " MaxLength of Field " & UCase(Trim(.GetData(vXLRowFldName, j))) & " should not exceed.;"
                                Else
                                    strerr = " MaxLength of Field " & UCase(Trim(.GetData(vXLRowFldName, j))) & " should not exceed.;"
                                End If
                                vsfXL.SetData(i, vXLColErrDesc, strerr)
                            End If
                            If UCase(Trim(.GetData(vXLRowFrgnKey, j))) <> "" Then
                                Dim strArr() As String
                                strArr = UCase(Trim(.GetData(vXLRowFrgnKey, j))).Split("|")
                                clssvt.getDataLongValue("select " & strArr(1) & " from " & strArr(0) & " where " & strArr(2) & " = '" & UCase(Trim(.GetData(i, j))) & "'", 0, blnfound, svtTrn)
                                If Not blnfound Then
                                    If strerr <> "" Then
                                        strerr = strerr & strArr(2) & " not found in the Masters.;"
                                    Else
                                        strerr = strArr(2) & " not found in the Masters.;"
                                    End If
                                    vsfXL.SetData(i, vXLColErrDesc, strerr)
                                End If
                            End If

                        Case "NUMERIC"
                            Dim output As Integer
                            If UCase(Trim(.GetData(vXLRowMandatory, j))) = "Y" Then
                                If UCase(Trim(.GetData(i, j))) = "" Then
                                    If strerr <> "" Then
                                        strerr = strerr & "Mandatory Field should not be empty.;"
                                    Else
                                        strerr = " Mandatory Field should not be empty.;"
                                    End If
                                    vsfXL.SetData(i, vXLColErrDesc, strerr)
                                End If
                            End If
                            If InStr(.GetData(vXLRowMaxLen, j), ",") > 0 Then
                                If Not Double.TryParse(.GetData(i, j), output) Then
                                    If strerr <> "" Then
                                        strerr = strerr & " Please enter the " & .GetData(vXLRowFldName, j) & " value according to the Maxlength.;"
                                    Else
                                        strerr = " Please enter the " & .GetData(vXLRowFldName, j) & " value according to the Maxlength.;"
                                    End If
                                    vsfXL.SetData(i, vXLColErrDesc, strerr)
                                End If
                            Else
                                If Integer.TryParse(.GetData(i, j), output) Then
                                    If Len(Val(.GetData(i, j))) > Val(Trim(.GetData(vXLRowMaxLen, j))) Then
                                        If strerr <> "" Then
                                            strerr = strerr & " MaxLength of Field " & UCase(Trim(.GetData(vXLRowFldName, j))) & " should not exceed.;"
                                        Else
                                            strerr = " MaxLength of Field " & UCase(Trim(.GetData(vXLRowFldName, j))) & " should not exceed.;"
                                        End If
                                        vsfXL.SetData(i, vXLColErrDesc, strerr)
                                    End If
                                End If
                            End If
                        Case "DATE"
                            'If Not Date.TryParse(.GetData(i, j), "DD/MM/YYYY") Then
                            '    If strerr <> "" Then
                            '        strerr = strerr & "Please enter the valid date"
                            '    Else
                            '        strerr = "Please enter the valid date"
                            '    End If
                            '    vsfXL.SetData(i, vXLColErrDesc, strerr)
                            'End If
                    End Select
                Next j
                'lblStatus.Text = "Validating " & vsfXL.GetData(i, intXLRows + 1) & " -- " & i & " / " & txtNoOfInv.Text

            Next i


        End With

        For i = intXLRows To vsfXL.Rows.Count - 1
            If vsfXL.GetData(i, vXLColErrDesc) <> "" Then
                vsfXL.SetData(i, vXLColBlnErr, "Y")
            End If
        Next
        blnValidData = False
        Dim csRed As CellStyle = vsfXL.Styles.Add("RED")
        csRed.BackColor = Color.Red
        For i = intXLRows To vsfXL.Rows.Count - 1
            If Trim(vsfXL.GetData(i, vXLColBlnErr)) = "Y" Then
                Dim rgRed As CellRange = vsfXL.GetCellRange(i, vXLColRowName)
                rgRed.Style = vsfXL.Styles("RED")
                blnValidData = True
            End If
        Next
        'With vsfXL
        '    For i = intXLRows To .Rows.Count - 1
        '        If ChkXLDuplicateData(vsfXL, i, intXLCols) Then
        '            strerr = strerr & "Duplicate " & UCase(Trim(.GetData(vXLRowFldName, intXLCols)))
        '            vsfXL.SetData(intXLRows, vXLColErrDesc, strerr)

        '            Exit For
        '        End If
        '    Next i
        'End With
        If blnValidData = False Then
            btnSave.Enabled = True
        Else
            btnSave.Enabled = False
        End If
    End Function

    Private Function ChkXLDuplicateData(ByVal argVsfGrid As C1.Win.C1FlexGrid.C1FlexGrid, ByRef argDupRow As Integer, ByVal ParamArray intChkCols() As Integer) As Boolean
        Dim intCols As Object
        Dim strUniqChk As String, strUniqKey As String
        ChkXLDuplicateData = False
        Dim X As Integer, j As Integer
        argDupRow = 0
        With argVsfGrid
            For X = intXLRows To .Rows.Count - 1
                strUniqKey = ""
                For Each intCols In intChkCols
                    strUniqKey = strUniqKey & "|" & Trim(.GetData(X, intCols))
                Next intCols
                For j = intXLRows To .Rows.Count - 1
                    strUniqChk = ""
                    For Each intCols In intChkCols
                        strUniqChk = strUniqChk & "|" & Trim(.GetData(j, intCols))
                    Next intCols
                    If X <> j Then
                        If strUniqKey = strUniqChk And strUniqChk <> "|" Then
                            argDupRow = X
                            ChkXLDuplicateData = True
                            Exit For
                        End If
                    End If
                Next j
            Next X
        End With
    End Function

    'Private Function getItemPresent() As Boolean
    '    getItemPresent = False
    '    Dim strSQL As String
    '    Dim rs As oledb.oledbDataReader, strItemCode As String, strHotkey As String
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

    Private Function ValdtBfrTemplate() As Boolean
        ValdtBfrTemplate = False
        If Trim(cboDoc.Text) = "" Then
            MsgBox("Please select Document!", vbInformation, Me.Text)
            cboDoc.Focus()
            Exit Function
        End If
        If Trim(cboActionType.Text) = "" Then
            MsgBox("Please select Action type!", vbInformation, Me.Text)
            cboActionType.Focus()
            Exit Function
        End If
        ValdtBfrTemplate = True
    End Function

    Private Sub btnExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGenTemplate.Click
        Try
            vsfXL.Rows.Count = intXLRows
            vsfXL.Rows.Count = intXLRows + 1
            Dim csGreen As CellStyle = vsfXL.Styles.Add("GREEN")
            csGreen.BackColor = Color.LawnGreen
            Dim rgRed As CellRange = vsfXL.GetCellRange(vsfXL.Rows.Count - 1, intXLCols)
            rgRed.Style = vsfXL.Styles("GREEN")
            vsfXL.Rows(vXLRowWarning).Visible = True
            vsfXL.Rows(vXLRowImpInfo).Visible = True
            Call SetFlexRowColor(vsfXL, "M", vXLRowWarning, vXLRowImpInfo)
            Call ExportTemplate(vsfXL)
            vsfXL.Rows(vXLRowWarning).Visible = False
            vsfXL.Rows(vXLRowImpInfo).Visible = False
            vsfXL.Rows.Count = intXLRows
        Catch
        End Try
    End Sub

    Private Sub fillTblFlds(ByVal dt As DataTable, ByVal strHdrTblName As String, ByRef intcols As Integer, ByVal argRowID As Integer)
        Dim blnFound As Boolean = False
        If dt.Rows(argRowID)(32) = True Then Exit Sub

        vsfXL.Cols.Count = vsfXL.Cols.Count + 1
        'MsgBox(dt.Rows(argRowID)(3).ToString)
        vsfXL.SetData(vXLRowFldName, intcols, UCase(dt.Rows(argRowID)(3).ToString))
        If UCase(dt.Rows(argRowID)(3).ToString) <> "PINCODE" AndAlso InStr(UCase(dt.Rows(argRowID)(3).ToString), "CODE") > 0 Or InStr(UCase(dt.Rows(argRowID)(3).ToString), "NAME") > 0 Then
            vsfXL.SetData(vXLRowMandatory, intcols, "Y")
        Else
            vsfXL.SetData(vXLRowMandatory, intcols, "N")
        End If
        If InStr(UCase(dt.Rows(argRowID)(3).ToString), "ID") > 0 And Replace(UCase(dt.Rows(argRowID)(3).ToString), "ID", "") <> Replace(UCase(strHdrTblName), "MST", "") Then
            Call clssvt.getDataStringValue("Select StorageFld from UtlFindFlds where StorageFld='" & UCase(dt.Rows(argRowID)(3).ToString) & "'", "", blnFound)
            If blnFound Then
                vsfXL.SetData(vXLRowFrgnKey, intcols, "MST" & Replace(UCase(dt.Rows(argRowID)(3).ToString), "ID", "") & "|" & UCase(dt.Rows(argRowID)(3).ToString) & "|" & Replace(UCase(dt.Rows(argRowID)(3).ToString), "ID", "CODE"))
                vsfXL.SetData(vXLRowFldName, intcols, Replace(UCase(dt.Rows(argRowID)(3).ToString), "ID", "Code"))
                vsfXL.SetData(vXLRowDataType, intcols, "Text")
                vsfXL.SetData(vXLRowMaxLen, intcols, 50)
                strSQL = "Select " & Replace(UCase(dt.Rows(argRowID)(3).ToString), "ID", "CODE")
                strSQL &= " from " & "MST" & Replace(UCase(dt.Rows(argRowID)(3).ToString), "ID", "")
                strSQL &= " where 1=1"
                strSQL &= " and " & Replace(UCase(dt.Rows(argRowID)(3).ToString), "ID", "CODE") & " is not null"
                strSQL &= " and " & Replace(UCase(dt.Rows(argRowID)(3).ToString), "ID", "CODE") & " <> '-'"
                strSQL &= " and " & Replace(UCase(dt.Rows(argRowID)(3).ToString), "ID", "CODE") & " <> ''"
            Else
                strSQL = "Select " & UCase(dt.Rows(argRowID)(3).ToString)
                strSQL &= " from " & strHdrTblName
                strSQL &= " where 1=1"
              
                Select Case dt.Rows(argRowID)(11)
                    Case 130, 129
                        strSQL &= " and " & UCase(dt.Rows(argRowID)(3).ToString) & " is not null"
                        strSQL &= " and " & UCase(dt.Rows(argRowID)(3).ToString) & " <> '-'"
                        strSQL &= " and " & UCase(dt.Rows(argRowID)(3).ToString) & " <> ''"
                        vsfXL.SetData(vXLRowDataType, intcols, "Text")
                        vsfXL.SetData(vXLRowMaxLen, intcols, dt.Rows(argRowID)(13))
                    Case 131
                        strSQL &= " and " & UCase(dt.Rows(argRowID)(3).ToString) & " is not null"
                        strSQL &= " and " & UCase(dt.Rows(argRowID)(3).ToString) & " > 0 "
                        vsfXL.SetData(vXLRowDataType, intcols, "Numeric")
                        vsfXL.SetData(vXLRowMaxLen, intcols, "(" & dt.Rows(argRowID)(15) & "," & dt.Rows(argRowID)(16) & ")")
                    Case 3
                        strSQL &= " and " & UCase(dt.Rows(argRowID)(3).ToString) & " is not null"
                        strSQL &= " and " & UCase(dt.Rows(argRowID)(3).ToString) & " > 0 "
                        vsfXL.SetData(vXLRowDataType, intcols, "Numeric")
                        vsfXL.SetData(vXLRowMaxLen, intcols, dt.Rows(argRowID)(15))
                    Case 7, 135
                        vsfXL.SetData(vXLRowDataType, intcols, "Date")
                        strSQL &= " and " & UCase(dt.Rows(argRowID)(3).ToString) & " is not null"
                End Select
            End If
        Else
            strSQL = "Select " & UCase(dt.Rows(argRowID)(3).ToString)
            strSQL &= " from " & strHdrTblName
            strSQL &= " where 1=1"
            Select Case dt.Rows(argRowID)(11)
                Case 130, 129
                    strSQL &= " and " & UCase(dt.Rows(argRowID)(3).ToString) & " is not null"
                    strSQL &= " and " & UCase(dt.Rows(argRowID)(3).ToString) & " <> '-'"
                    strSQL &= " and " & UCase(dt.Rows(argRowID)(3).ToString) & " <> ''"
                    vsfXL.SetData(vXLRowDataType, intcols, "Text")
                    vsfXL.SetData(vXLRowMaxLen, intcols, dt.Rows(argRowID)(13))
                Case 131
                    strSQL &= " and " & UCase(dt.Rows(argRowID)(3).ToString) & " is not null"
                    strSQL &= " and " & UCase(dt.Rows(argRowID)(3).ToString) & " > 0 "
                    vsfXL.SetData(vXLRowDataType, intcols, "Numeric")
                    vsfXL.SetData(vXLRowMaxLen, intcols, "(" & dt.Rows(argRowID)(15) & "," & dt.Rows(argRowID)(16) & ")")
                Case 3
                    strSQL &= " and " & UCase(dt.Rows(argRowID)(3).ToString) & " is not null"
                    strSQL &= " and " & UCase(dt.Rows(argRowID)(3).ToString) & " > 0 "
                    vsfXL.SetData(vXLRowDataType, intcols, "Numeric")
                    vsfXL.SetData(vXLRowMaxLen, intcols, dt.Rows(argRowID)(15))
                Case 7, 135
                    vsfXL.SetData(vXLRowDataType, intcols, "Date")
                    strSQL &= " and " & UCase(dt.Rows(argRowID)(3).ToString) & " is not null"
            End Select
        End If

        vsfXL.SetData(vXLRowExample, intcols, clssvt.getDataStringValue(strSQL & " order by " & UCase(dt.Rows(argRowID)(3).ToString) & " Desc", "", False))
        If UCase(dt.Rows(argRowID)(3).ToString) = "INACTIVE" Then
            vsfXL.SetData(vXLRowDefaultVal, intcols, "N")
            vsfXL.SetData(vXLRowExample, intcols, "N")
        Else
            vsfXL.SetData(vXLRowDefaultVal, intcols, dt.Rows(argRowID)(8))
        End If
        If UCase(dt.Rows(argRowID)(3).ToString) <> "PINCODE" AndAlso InStr(UCase(dt.Rows(argRowID)(3).ToString), "CODE") > 0 Then
            vsfXL.SetData(vXLRowUnique, intcols, "Y")
        Else
            vsfXL.SetData(vXLRowUnique, intcols, "N")
        End If
        intcols = intcols + 1
    End Sub

    Private Function createCRC(ByVal argCol As Integer) As String
        createCRC = ""
        Dim strColString As String = ""
        For i = vXLRowFldName To vXLRowDefaultVal - 1
            'MsgBox(vsfXL.GetData(i, 0))
            If UCase(Trim(vsfXL.GetData(i, argCol))) <> "EXAMPLE" Then
                strColString = strColString & Replace(Trim(vsfXL.GetData(i, argCol)), Chr(13), "")
            End If
        Next
        createCRC = getCheckSum(strColString)
    End Function

    Public Sub ExportTemplate(ByVal argVSF As C1.Win.C1FlexGrid.C1FlexGrid, Optional ByRef FilePath As String = "")
        Dim strFile As String
        Try

            strFile = Application.StartupPath & "\Temp\XLExport" & Date.Now.Ticks & ".xlsx"
            argVSF.SaveExcel(strFile, "DataSheet", C1.Win.C1FlexGrid.FileFlags.AsDisplayed Or C1.Win.C1FlexGrid.FileFlags.IncludeMergedRanges Or C1.Win.C1FlexGrid.FileFlags.IncludeFixedCells)

            If FilePath = "Y" Then
                FilePath = strFile
                Exit Sub
            End If

            Dim p As New System.Diagnostics.Process
            Dim s As New System.Diagnostics.ProcessStartInfo(strFile)
            s.UseShellExecute = True
            s.WindowStyle = ProcessWindowStyle.Normal
            p.StartInfo = s
            p.Start()
        Catch ex As Exception
            MsgBox("Cannot export to Excel." & vbCrLf & ex.Message, MsgBoxStyle.Critical, "Export Error")
        End Try
    End Sub

    Private Sub btnRemErr_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRemErr.Click
        Dim i As Integer, j As Integer
        If MsgBox("Do you want to Remove ALL Erroneous Records ?", vbYesNo + vbQuestion, Me.Text) = vbNo Then
            Exit Sub
        End If
        vsfTmp.Rows.Count = 1
        vsfTmp.Cols.Count = vsfXL.Cols.Count
        For i = 0 To vsfXL.Cols.Count - 1
            vsfTmp.SetData(0, i, vsfXL.GetData(0, i))
        Next i
        With vsfXL
Repeat:
            For i = 1 To (.Rows.Count - 1)
                If .GetData(i, vExBlnErr) = "Y" Then
                    vsfTmp.Rows.Count = vsfTmp.Rows.Count + 1
                    For j = 0 To vsfXL.Cols.Count - 1
                        Dim cs As C1.Win.C1FlexGrid.CellStyle = vsfXL.Styles.Add("AllColumns")
                        vsfTmp.SetData(vsfTmp.Rows.Count - 1, j, vsfXL.GetData(i, j))
                        cs.BackColor = Color.White
                        vsfXL.SetCellStyle(i, j, cs)
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
        'Me.Close()
        lblRecNo.Text = ""
        txtXLFilePath.Text = ""
        Dim tb As New C1.Win.C1Command.C1DockingTabPage()
        Dim dockTabMain As New C1.Win.C1Command.C1DockingTab()
        Me.Dispose()
        tb.Controls.Clear()
        frmMain.dockTabMain.SelectedTab.Dispose()
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click

        If vsfXL.Rows.Count <= intXLRows Then
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
    End Sub

    Private Sub ImportData()

        Dim i As Integer, strSQL As String = "", intID As Long
        Dim strFlds As String = ""
        Dim strFldvalue As String = ""
        Dim IntNo As Integer
        Cursor.Current = Cursors.WaitCursor

        svtTrn = svtConn.BeginTransaction(IsolationLevel.ReadCommitted)
        Try


            strSQL = "Insert into " & strHdrTblName & " ("
            strSQL = strSQL & Replace(UCase(strHdrTblName), "MST", "") & "ID" & ","
            strFlds = ""
            For j = intXLCols To vsfXL.Cols.Count - 1
                If strMstAutoGenCode = "Y" And UCase(Trim(vsfXL.GetData(vXLRowFldName, j))) = UCase(Replace(UCase(strHdrTblName), "MST", "") & "SERIES") Then
                    strFlds = strFlds & UCase(Trim(vsfXL.GetData(vXLRowFldName, j))) & ","
                    strFlds = strFlds & Replace(UCase(Trim(vsfXL.GetData(vXLRowFldName, j))), "SERIES", "NO") & ","
                    strFlds = strFlds & Replace(UCase(Trim(vsfXL.GetData(vXLRowFldName, j))), "SERIES", "CODE") & ","
                Else
                    If UCase(Trim(vsfXL.GetData(vXLRowFrgnKey, j))) <> "" Then
                        Dim strArr() As String
                        strArr = UCase(Trim(vsfXL.GetData(vXLRowFrgnKey, j))).Split("|")
                        strFlds = strFlds & strArr(1) & ","
                    Else
                        strFlds = strFlds & UCase(Trim(vsfXL.GetData(vXLRowFldName, j))) & ","
                    End If
                End If
            Next j
            strFlds = Mid(strFlds, 1, Len(strFlds) - 1)
            strSQL = strSQL & strFlds & ") values ("

            strFldvalue = ""
            For i = intXLRows To vsfXL.Rows.Count - 1
                If vsfXL.GetData(i, vExBlnErr) <> "Y" Then
                    intID = clssvt.AutoIncrementedNum(Replace(UCase(strHdrTblName), "MST", "") & "ID", strHdrTblName, "", svtTrn)
                    strFldvalue = intID & ","
                    For j = intXLCols To vsfXL.Cols.Count - 1
                        'MsgBox(UCase(Trim(vsfXL.GetData(vXLRowFldName, j))))
                        Select Case UCase(Trim(vsfXL.GetData(vXLRowDataType, j)))
                            Case "TEXT", "LIST STRING"
                                If strMstAutoGenCode = "Y" And UCase(Trim(vsfXL.GetData(vXLRowFldName, j))) = UCase(Replace(UCase(strHdrTblName), "MST", "") & "SERIES") Then
                                    IntNo = 0
                                    IntNo = clssvt.getDataLongValue("Select Max(" & getSyntax("ISNULL0", Replace(UCase(Trim(vsfXL.GetData(vXLRowFldName, j))), "SERIES", "NO")) & ")+1 from " & strHdrTblName & " where " & Trim(vsfXL.GetData(vXLRowFldName, j)) & " ='" & Trim(vsfXL.GetData(i, j)) & "'", 0, False, svtTrn)
                                    If IntNo = 0 Then IntNo = 1
                                    strFldvalue = strFldvalue & "'" & Trim(vsfXL.GetData(i, j)) & "',"
                                    strFldvalue = strFldvalue & IntNo & ","
                                    strFldvalue = strFldvalue & "'" & Trim(vsfXL.GetData(i, j)) & Microsoft.VisualBasic.Right("000000" & IntNo, 6) & "',"
                                Else
                                    If UCase(Trim(vsfXL.GetData(vXLRowFrgnKey, j))) <> "" Then
                                        Dim strArr() As String
                                        strArr = UCase(Trim(vsfXL.GetData(vXLRowFrgnKey, j))).Split("|")
                                        strFldvalue = strFldvalue & (clssvt.getDataLongValue("select " & strArr(1) & " from " & strArr(0) & " where " & strArr(2) & " = '" & UCase(Trim(vsfXL.GetData(i, j))) & "'", 0, False, svtTrn)) & ","
                                    Else
                                        strFldvalue = strFldvalue & "'" & Trim(vsfXL.GetData(i, j)) & "',"
                                    End If
                                End If

                            Case "NUMERIC", "LIST NUMERIC"
                                strFldvalue = strFldvalue & Val(vsfXL.GetData(i, j)) & ","
                            Case "DATE"
                                strFldvalue = strFldvalue & (getDateFormatInCond(GetVal(vsfXL.GetData(i, j), getSystemDateTime(svtTrn)))) & ","
                        End Select
                    Next j

                End If
                strFldvalue = Mid(strFldvalue, 1, Len(strFldvalue) - 1)
                If Not clssvt.execSQLStatement(strSQL & strFldvalue & ")", svtTrn) Then
                    svtTrn.Rollback()
                    Exit Sub
                End If
            Next i
            svtTrn.Commit()

            MsgBox("Imported Data Successfully!", vbInformation, Me.Text)
        Catch ex As Exception
            svtTrn.Rollback()
            MsgBox(ex.Message & vbCrLf & "Error in SQL: " & strSQL)
        Finally
            Cursor.Current = Cursors.Arrow
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

    Private Sub vsfExcel_BeforeEdit(sender As Object, e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfXL.BeforeEdit
        e.Cancel = True
    End Sub

    Private Sub btnExportToXL_Click(sender As System.Object, e As System.EventArgs) Handles btnExportToXL.Click
        ' If vsfXL.Row < 1 Then Exit Sub
        Call ExportToXL(vsfXL)
    End Sub

    Private Sub fillFldsFromTbl(ByVal argName As String)
        Dim strSQL As String, strSubTblCaption As String
        Dim blnfound As Boolean

        Dim rs As oledb.oledbDataReader
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

            With vsfXL
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

        Call bindCombo(cboDoc, "DocName", "DocName", "select DocName from UtlTransactions where DocType='Master' order by docName")

        With vsfXL
            .Rows.Count = intXLRows
            .Cols.Count = intXLCols
            .SetData(vXLRowWarning, vXLColRowName, "Warning! Please do not modify the template heading. Start entering the data from Green coloured cell.")
            .SetData(vXLRowFldName, vXLColRowName, "Field Name")
            .SetData(vXLRowDataType, vXLColRowName, "Data Type")
            .SetData(vXLRowMandatory, vXLColRowName, "Mandatory")
            .SetData(vXLRowFrgnKey, vXLColRowName, "Foreign Key")
            .SetData(vXLRowMaxLen, vXLColRowName, "Max length")
            .SetData(vXLRowUnique, vXLColRowName, "Unique")
            .SetData(vXLRowExample, vXLColRowName, "Example")
            .SetData(vXLRowDefaultVal, vXLColRowName, "Default Value")

            Call SetHideCol(vsfXL, True, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9)
            Call SetHideRow(vsfXL, True, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19)
            For i = vXLRowFldName To vXLRowDefaultVal
                .Rows(i).Visible = True
            Next
            .Rows(vXLRowFrgnKey).Visible = False
            .Cols(vXLColRowName).Visible = True
        End With
    End Sub

    Private Sub cboDoc_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles cboDoc.KeyPress
        If e.KeyChar = "'" Then
            e.Handled = True
        End If
    End Sub

    Private Sub cboActionType_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles cboActionType.KeyPress
        If e.KeyChar = "'" Then
            e.Handled = True
        End If
    End Sub

    Private Sub cboActionType_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles cboActionType.Validating
        If Not validateCombo(cboActionType) Then
            MsgBox("Please select valid Action type!!", vbExclamation, Me.Text)
            cboActionType.Focus()
            Exit Sub
        End If
    End Sub

    Private Sub cboDoc_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles cboDoc.Validating
        Dim blnFound As Boolean

        If Trim(cboDoc.Text) = "" Then Exit Sub

        Call clssvt.getDataStringValue("select DocName from UtlTransactions where docName = '" & Trim(cboDoc.Text) & "'", 0, blnFound)
        If blnFound = False Then
            MsgBox("Selected Document not found in master!", vbCritical, Me.Text)
            e.Cancel = True
            Exit Sub
        End If

        If cboDoc.Text = "-" Or cboDoc.Text = "0" Then
            MsgBox("Selected Document is not found!", vbCritical, Me.Text)
            e.Cancel = True
            Exit Sub
        End If
    End Sub

    Private Sub btnLoadTemplate_Click(sender As System.Object, e As System.EventArgs) Handles btnLoadTemplate.Click
        If Not ValdtBfrTemplate() Then
            Exit Sub
        End If
        vsfXL.Cols(vXLColBlnErr).Visible = False
        vsfXL.Cols(vXLColErrDesc).Visible = False
        vsfXL.Cols(vXLColStatus).Visible = False

        strHdrTblName = ""
        strDocType = ""
        intDocID = 0
        strMstAutoGenCode = ""
        Dim blnFound As Boolean = False
        Dim rs As oledb.oledbDataReader = Nothing
        Dim dtSchemaTbl As DataTable = Nothing
        Dim intcols As Integer
        Dim j As Integer = 1

        strHdrTblName = clssvt.getDataStringValue("select HdrTblName from UtlTransactions where DocName='" & Trim(cboDoc.Text) & "'", "", False)
        strDocType = clssvt.getDataStringValue("select DocType from UtlTransactions where DocName='" & Trim(cboDoc.Text) & "'", "", False)
        intDocID = clssvt.getDataStringValue("select DocID from UtlTransactions where DocName='" & Trim(cboDoc.Text) & "'", "", False)
        strMstAutoGenCode = clssvt.getDataStringValue("select MstAutoGenCode from UtlTransactions where DocName='" & Trim(cboDoc.Text) & "'", "", False)

        Dim Rest() As String = {Nothing, Nothing, strHdrTblName, Nothing}
        Dim columns As String = "Columns"
        Dim dt As DataTable = svtConn.GetSchema(columns, Rest)
        'vsf.DataSource = dt
        'ExportToXL(vsf)
        vsfXL.Cols.Frozen = vXLColRowName
        intcols = intXLCols
        vsfXL.Cols.Count = intXLCols
        vsfXL.Rows.Count = intXLRows
        vsfXL.SetData(vXLRowImpInfo, vXLColRowName, "Importing data for " & Trim(cboDoc.Text))
        j = 1
forloop:
        For i = 0 To dt.Rows.Count - 1
            'MsgBox(dt.Rows(i)(6) & "-" & dt.Rows(i)(3).ToString)
            If j + 1 > dt.Rows.Count + 1 Then Exit For
            If dt.Rows(i)(6) = j Then
                If (UCase(dt.Rows(i)(3).ToString)) <> UCase(Replace(UCase(strHdrTblName), "MST", "") & "ID") Then
                    'MsgBox(dt.Rows(i)(3).ToString)
                    If strMstAutoGenCode = "Y" Then
                        If (UCase(dt.Rows(i)(3).ToString) = UCase(Replace(UCase(strHdrTblName), "MST", "") & "SERIES")) Then
                            fillTblFlds(dt, strHdrTblName, intcols, i)
                        Else
                            If (UCase(dt.Rows(i)(3).ToString) <> UCase(Replace(UCase(strHdrTblName), "MST", "") & "NO")) And (UCase(dt.Rows(i)(3).ToString) <> UCase(Replace(UCase(strHdrTblName), "MST", "") & "CODE")) Then
                                fillTblFlds(dt, strHdrTblName, intcols, i)
                            End If
                        End If
                    Else
                        If (UCase(dt.Rows(i)(3).ToString) <> UCase(Replace(UCase(strHdrTblName), "MST", "") & "SERIES")) And (UCase(dt.Rows(i)(3).ToString) <> UCase(Replace(UCase(strHdrTblName), "MST", "") & "NO")) Then
                            fillTblFlds(dt, strHdrTblName, intcols, i)
                        End If
                    End If
                End If
                j = j + 1
                GoTo forloop
            End If
            If j + 1 > dt.Rows.Count + 1 Then GoTo forloop
        Next

        rs = clssvt.getDataReaderFromSQL("select * from UtlDocDsgrFlds where DocID = " & intDocID & " order by FldOrderBy")

        With rs
            Do While .Read
                For i = intXLCols To vsfXL.Cols.Count - 1
                    If vsfXL.GetData(vXLRowFldName, i) = rs("FldCode") Then
                        vsfXL.SetData(vXLRowDataType, i, rs("FldDataType"))
                        vsfXL.SetData(vXLRowMandatory, i, rs("FldBlnMandatory"))
                        If UCase(rs("FldDataType")) = "TEXT" Then
                            vsfXL.SetData(vXLRowMaxLen, i, rs("FldDataWidth"))
                        Else
                            vsfXL.SetData(vXLRowMaxLen, i, "(" & rs("FldDataWidth") & "," & rs("FldNumDecimals") & ")")
                        End If
                        vsfXL.SetData(vXLRowUnique, i, rs("FldUnique"))
                        If rs("FldUseDefault") = "Y" Then
                            vsfXL.SetData(vXLRowDefaultVal, i, rs("FldDefStdDefault"))
                        End If
                    End If
                Next
            Loop
            .Close()
        End With
        For i = intXLCols To vsfXL.Cols.Count - 1
            vsfXL.SetData(vXLRowCRC, i, createCRC(i))
        Next
        'vsfXL.Rows(vXLRowCRC).Visible = True
        Call vsfXL.AutoSizeCols()
        If vsfXL.Cols.Count > 2 Then
            btnGenTemplate.Enabled = True
        End If
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        pnlImportClientXL.Visible = False
    End Sub

    Private Sub btnOK_Click(sender As Object, e As EventArgs) Handles btnOK.Click
        Call CleanXLNonTemplateGrid()
        If gBlnMSOffice Then
            Call LoadNonTemplateXL(Trim(txtXLFilePath.Text), Val(txtColHdr.Text), Val(txtRowStart.Text))
        Else
            Call LoadNonTemplateXL_ClosedXML(Trim(txtXLFilePath.Text), Val(txtColHdr.Text), Val(txtRowStart.Text))
        End If

        pnlImportClientXL.Visible = False
    End Sub

    Private Sub btnRefersh_Click(sender As Object, e As EventArgs) Handles btnRefersh.Click
        blnNonTemplateColumns = True

        With vsf
            .Rows.Count = 1
            .Cols.Count = vNXlTotal

            .SetData(0, vNXlFld, "Fld Code")
            .SetData(0, vNXlMapFld, "Map Column")

            If gBlnMSOffice Then
                .Cols(vNXlMapFld).ComboList = GetColumnFromXL(Trim(txtXLFilePath.Text), Val(txtColHdr.Text))
            Else
                .Cols(vNXlMapFld).ComboList = GetColumnFromXL_ClosedXML(Trim(txtXLFilePath.Text), Val(txtColHdr.Text))
            End If



            With vsfXL
                For i As Int32 = intXLCols To .Cols.Count - 1
                    If GetVal(.GetData(vXLRowFldName, i)) <> "" Then
                        vsf.Rows.Count = vsf.Rows.Count + 1
                        vsf.SetData(vsf.Rows.Count - 1, vNXlFld, .GetData(vXLRowFldName, i))
                    End If
                Next
            End With
        End With
    End Sub

    Private Sub vsf_BeforeEdit(sender As Object, e As RowColEventArgs) Handles vsf.BeforeEdit
        If e.Col = vNXlFld Then
            e.Cancel = True
        End If
    End Sub

    Private Function GetColumnFromXL(ByVal argFileName As String, ByVal argColHdr As Int32) As String
        Dim strCaption As String = " |"

        Dim xlapp As New Excel.Application
        Dim xlSheet As New Excel.Worksheet
        intXLNonTemplateCols = 0
        xlapp.Workbooks.Open(argFileName, , True)
        xlSheet = xlapp.Sheets(1)
        With xlapp.Application
            If argColHdr <= 0 Then
                blnNonTemplateNonColumns = True
                For i As Int32 = 1 To 100
                    If CStr(xlapp.ActiveSheet.cells(1, i).Value & "") <> "" Then
                        strCaption &= ColumnIndexToColumnLetter(i) & "|"
                        intXLNonTemplateCols = intXLNonTemplateCols + 1
                    End If
                Next
            Else
                For i As Int32 = 1 To 100
                    If CStr(xlapp.ActiveSheet.cells(argColHdr, i).Value & "") <> "" Then
                        strCaption &= xlapp.ActiveSheet.cells(argColHdr, i).Value & "|"
                        intXLNonTemplateCols = intXLNonTemplateCols + 1
                    End If
                Next
            End If
        End With
        xlapp.Workbooks(1).Close(SaveChanges:=False)

exitfunction:
        Return strCaption
    End Function

    Private Function GetColumnFromXL_ClosedXML(ByVal argFileName As String, ByVal argColHdr As Int32) As String
        Dim strCaption As String = " |"

        'Dim xlapp As New Excel.Application
        'Dim xlSheet As New Excel.Worksheet
        Dim xlWB As New XLWorkbook(argFileName)
        Dim xlWS As IXLWorksheet = xlWB.Worksheet(1)
        intXLNonTemplateCols = 0
        'xlWB.Workbooks.Open(argFileName, , True)
        'xlSheet = xlapp.Sheets(1)
        With xlWS
            If argColHdr <= 0 Then
                blnNonTemplateNonColumns = True
                For i As Int32 = 1 To 100
                    'If CStr(xlWS.ActiveSheet.cells(1, i).Value & "") <> "" Then
                    If CStr(xlWS.Cell(1, i).Value & "") <> "" Then
                        strCaption &= ColumnIndexToColumnLetter(i) & "|"
                        intXLNonTemplateCols = intXLNonTemplateCols + 1
                    End If
                Next
            Else
                For i As Int32 = 1 To 100
                    If CStr(xlWS.Cell(argColHdr, i).Value & "") <> "" Then
                        strCaption &= xlWS.Cell(argColHdr, i).Value & "|"
                        intXLNonTemplateCols = intXLNonTemplateCols + 1
                    End If
                Next
            End If
        End With
        'xlWB.Workbooks(1).Close(SaveChanges:=False)

        'exitfunction:
        '        Return strCaption
    End Function

    Private Function ColumnIndexToColumnLetter(colIndex As Integer) As String
        Dim div As Integer = colIndex
        Dim colLetter As String = String.Empty
        Dim modnum As Integer = 0

        While div > 0
            modnum = (div - 1) Mod 26
            colLetter = Chr(65 + modnum) & colLetter
            div = CInt((div - modnum) \ 26)
        End While

        Return colLetter
    End Function

    Private Function getTotalColumnsFromVSF() As Int32
        Dim intCount As Int32 = 0
        With vsf
            For i As Int32 = 1 To .Rows.Count
                If Trim(.GetData(i, vNXlMapFld)) <> "" Then
                    intCount = intCount + 1
                End If
            Next
        End With
        Return intCount
    End Function

    Private Function getXLColumnNumber(ByVal argCol As String) As Int32
        Dim intCol As Int32 = 0
        For i = 1 To Len(argCol)
            intCol = intCol * 26 + (Asc(UCase(Mid(argCol, i, 1))) - 64)
        Next
        Return intCol
    End Function

    Private Sub CleanXLNonTemplateGrid()
        With vsf
startCleanProcess:
            For i As Int32 = 1 To .Rows.Count - 1
                If GetVal(.GetData(i, vNXlFld), "") = "" Or GetVal(.GetData(i, vNXlMapFld), "") = "" Then
                    .Rows.Remove(i)
                    GoTo startCleanProcess
                End If
            Next
        End With
    End Sub

    Private Sub LoadNonTemplateXL(ByVal argFileName As String, ByVal argColHdr As Int32, ByVal argRowNo As Int32)
        Dim xlapp As New Excel.Application
        Dim xlSheet As New Excel.Worksheet
        xlapp.Workbooks.Open(argFileName, , True)

        xlSheet = xlapp.Sheets(1)

        blnfirstload = False
        With xlapp.Application
            vsfXL.Rows.Count = intXLRows
            Try
                Do While Trim(.Cells(argRowNo, 1).Value).ToString <> ""

                    'Add new row into xl grid
                    vsfXL.Rows.Count = vsfXL.Rows.Count + 1

                    'Check whether non template non columns
                    If Not blnNonTemplateNonColumns Then

                        'start for loop column wise from vsfXL columns
                        For intVsfXLCol As Int32 = intXLCols To vsfXL.Cols.Count - 1
                            Dim blnUpdateValue As Boolean = False

                            'start for loop Row wise from vsf grid rows
                            For intRowVSF As Int32 = 1 To vsf.Rows.Count - 1

                                ' check whether vsfxl grid fldcode = vsf grid fldcode
                                If vsfXL.GetData(vXLRowFldName, intVsfXLCol) = vsf.GetData(intRowVSF, vNXlFld) Then
                                    blnUpdateValue = True

                                    'start for loop in xl template file
                                    For intXLCol As Int32 = 1 To intXLNonTemplateCols

                                        'check whether column are equal then update values into vsfXL grid
                                        If .Cells(argColHdr, intXLCol).Value = vsf.GetData(intRowVSF, vNXlMapFld) Then
                                            vsfXL.SetData(vsfXL.Rows.Count - 1, intVsfXLCol, Replace(Replace(Trim(.Cells(argRowNo, intXLCol).Value), "'", "`"), Chr(10), ""))
                                            GoTo nextfld
                                        End If

                                    Next

                                End If

                            Next
                            If Not blnUpdateValue Then
                                vsfXL.SetData(vsfXL.Rows.Count - 1, intVsfXLCol, Replace(Replace(Replace(GetVal(vsfXL.GetData(vXLRowDefaultVal, intVsfXLCol), ""), "('')", ""), "('", ""), "')", ""))
                            End If
nextfld:
                        Next
                    Else
                        'start for loop column wise from vsfXL columns
                        For intVsfXLCol As Int32 = intXLCols To vsfXL.Cols.Count - 1
                            Dim blnUpdateValue As Boolean = False

                            'start for loop Row wise from vsf grid rows
                            For intRowVSF As Int32 = 1 To vsf.Rows.Count - 1

                                ' check whether vsfxl grid fldcode = vsf grid fldcode
                                If vsfXL.GetData(vXLRowFldName, intVsfXLCol) = vsf.GetData(intRowVSF, vNXlFld) Then
                                    blnUpdateValue = True

                                    'start for loop in xl template file
                                    For intXLCol As Int32 = 1 To intXLNonTemplateCols

                                        'check whether column are equal then update values into vsfXL grid
                                        If intXLCol = getXLColumnNumber(vsf.GetData(intRowVSF, vNXlMapFld)) Then
                                            vsfXL.SetData(vsfXL.Rows.Count - 1, intVsfXLCol, Replace(Replace(Trim(.Cells(argRowNo, intXLCol).Value), "'", "`"), Chr(10), ""))
                                            GoTo nextfld1
                                        End If

                                    Next

                                End If

                            Next
                            If Not blnUpdateValue Then
                                vsfXL.SetData(vsfXL.Rows.Count - 1, intVsfXLCol, Replace(Replace(Replace(GetVal(vsfXL.GetData(vXLRowDefaultVal, intVsfXLCol), ""), "('')", ""), "('", ""), "')", ""))
                            End If
nextfld1:
                        Next

                    End If

                    argRowNo = argRowNo + 1
                Loop
                StMsg("Data Loaded, now Validating the data")
                .Workbooks(1).Close(SaveChanges:=False)
            Catch ex As Exception
                MsgBox("Cannot Import from Excel." & vbCrLf & ex.Message, MsgBoxStyle.Critical, "Import Error")
                Exit Sub
            End Try
        End With

        With vsfXL
            For i As Int32 = intXLRows + 1 To .Rows.Count - 1
                For j As Int32 = intXLCols + 1 To .Cols.Count - 1
                    If UCase(.GetData(vXLRowFldName, j)) = "ADDEDBY" Or UCase(.GetData(vXLRowFldName, j)) = "EDITEDBY" Or UCase(.GetData(vXLRowFldName, j)) = "CHANGEDBY" Then
                        .SetData(i, j, gUserCode)
                    End If
                Next
            Next


            .Cols(vXLColBlnErr).Visible = True
            .Cols(vXLColErrDesc).Visible = True
            .Cols(vXLColStatus).Visible = True
            .SetData(vXLRowFldName, vXLColBlnErr, "Errors")
            .SetData(vXLRowFldName, vXLColErrDesc, "Err. Desc")
            .SetData(vXLRowFldName, vXLColStatus, "Status")
        End With

        'Validating the data here - begins
        Call ValidateEntry()
        vsfXL.AutoSizeCols()
        StMsg("")
        vsfXL.Redraw = True
        Exit Sub
    End Sub

    Private Sub LoadNonTemplateXL_ClosedXML(ByVal argFileName As String, ByVal argColHdr As Int32, ByVal argRowNo As Int32)
        'Dim xlapp As New Excel.Application
        'Dim xlSheet As New Excel.Worksheet
        Dim xlWB As New XLWorkbook(argFileName)
        Dim xlWS As IXLWorksheet = xlWB.Worksheet(1)
        'xlapp.Workbooks.Open(argFileName, , True)

        'xlSheet = xlapp.Sheets(1)

        blnfirstload = False
        With xlWS
            vsfXL.Rows.Count = intXLRows
            Try
                Do While Trim(xlWS.Cell(argRowNo, 1).Value).ToString <> ""

                    'Add new row into xl grid
                    vsfXL.Rows.Count = vsfXL.Rows.Count + 1

                    'Check whether non template non columns
                    If Not blnNonTemplateNonColumns Then

                        'start for loop column wise from vsfXL columns
                        For intVsfXLCol As Int32 = intXLCols To vsfXL.Cols.Count - 1
                            Dim blnUpdateValue As Boolean = False

                            'start for loop Row wise from vsf grid rows
                            For intRowVSF As Int32 = 1 To vsf.Rows.Count - 1

                                ' check whether vsfxl grid fldcode = vsf grid fldcode
                                If vsfXL.GetData(vXLRowFldName, intVsfXLCol) = vsf.GetData(intRowVSF, vNXlFld) Then
                                    blnUpdateValue = True

                                    'start for loop in xl template file
                                    For intXLCol As Int32 = 1 To intXLNonTemplateCols

                                        'check whether column are equal then update values into vsfXL grid
                                        If .Cell(argColHdr, intXLCol).Value = vsf.GetData(intRowVSF, vNXlMapFld) Then
                                            vsfXL.SetData(vsfXL.Rows.Count - 1, intVsfXLCol, Replace(Replace(Trim(.Cell(argRowNo, intXLCol).Value), "'", "`"), Chr(10), ""))
                                            GoTo nextfld
                                        End If

                                    Next

                                End If

                            Next
                            If Not blnUpdateValue Then
                                vsfXL.SetData(vsfXL.Rows.Count - 1, intVsfXLCol, Replace(Replace(Replace(GetVal(vsfXL.GetData(vXLRowDefaultVal, intVsfXLCol), ""), "('')", ""), "('", ""), "')", ""))
                            End If
nextfld:
                        Next
                    Else
                        'start for loop column wise from vsfXL columns
                        For intVsfXLCol As Int32 = intXLCols To vsfXL.Cols.Count - 1
                            Dim blnUpdateValue As Boolean = False

                            'start for loop Row wise from vsf grid rows
                            For intRowVSF As Int32 = 1 To vsf.Rows.Count - 1

                                ' check whether vsfxl grid fldcode = vsf grid fldcode
                                If vsfXL.GetData(vXLRowFldName, intVsfXLCol) = vsf.GetData(intRowVSF, vNXlFld) Then
                                    blnUpdateValue = True

                                    'start for loop in xl template file
                                    For intXLCol As Int32 = 1 To intXLNonTemplateCols

                                        'check whether column are equal then update values into vsfXL grid
                                        If intXLCol = getXLColumnNumber(vsf.GetData(intRowVSF, vNXlMapFld)) Then
                                            vsfXL.SetData(vsfXL.Rows.Count - 1, intVsfXLCol, Replace(Replace(Trim(.Cell(argRowNo, intXLCol).Value), "'", "`"), Chr(10), ""))
                                            GoTo nextfld1
                                        End If

                                    Next

                                End If

                            Next
                            If Not blnUpdateValue Then
                                vsfXL.SetData(vsfXL.Rows.Count - 1, intVsfXLCol, Replace(Replace(Replace(GetVal(vsfXL.GetData(vXLRowDefaultVal, intVsfXLCol), ""), "('')", ""), "('", ""), "')", ""))
                            End If
nextfld1:
                        Next

                    End If

                    argRowNo = argRowNo + 1
                Loop
                StMsg("Data Loaded, now Validating the data")
                '.Workbooks(1).Close(SaveChanges:=False)
            Catch ex As Exception
                MsgBox("Cannot Import from Excel." & vbCrLf & ex.Message, MsgBoxStyle.Critical, "Import Error")
                Exit Sub
            End Try
        End With

        With vsfXL
            For i As Int32 = intXLRows + 1 To .Rows.Count - 1
                For j As Int32 = intXLCols + 1 To .Cols.Count - 1
                    If UCase(.GetData(vXLRowFldName, j)) = "ADDEDBY" Or UCase(.GetData(vXLRowFldName, j)) = "EDITEDBY" Or UCase(.GetData(vXLRowFldName, j)) = "CHANGEDBY" Then
                        .SetData(i, j, gUserCode)
                    End If
                Next
            Next


            .Cols(vXLColBlnErr).Visible = True
            .Cols(vXLColErrDesc).Visible = True
            .Cols(vXLColStatus).Visible = True
            .SetData(vXLRowFldName, vXLColBlnErr, "Errors")
            .SetData(vXLRowFldName, vXLColErrDesc, "Err. Desc")
            .SetData(vXLRowFldName, vXLColStatus, "Status")
        End With

        'Validating the data here - begins
        Call ValidateEntry()
        vsfXL.AutoSizeCols()
        StMsg("")
        vsfXL.Redraw = True
        Exit Sub
    End Sub

    Private Sub frmUtlImportXL_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        cboDoc.Focus()
    End Sub

    Private Sub chkClientXL_Leave(sender As Object, e As EventArgs) Handles chkClientXL.Leave
        If btnSave.Enabled Then
            btnSave.Focus()
        ElseIf btnRemErr.Visible Then
            btnRemErr.Focus()
        Else
            btnExportToXL.Focus()

        End If
    End Sub

    Private Sub btnImpClose_Leave(sender As Object, e As EventArgs) Handles btnImpClose.Leave
        cboDoc.Focus()
    End Sub
End Class