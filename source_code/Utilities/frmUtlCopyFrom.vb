Imports C1.Win.C1FlexGrid
Imports Microsoft.Office.Interop
Imports ClosedXML.Excel

Public Class frmUtlCopyFrom
    Dim clsSVT As New svtLibrary
    Public argvsf As C1FlexGrid
    Public argvsfOptions As C1FlexGrid
    Public strFrmCaption As String

    Dim strSHForm As String
    Dim strHdrTbl As String
    Dim strDtlTbl As String
    Dim strDocIDFldName As String
    Dim strDocNoFldName As String
    Dim strCustSuppReq As String

    Dim blnFound As Boolean = False
    Public argForm As Object

    Dim strColList As String = ""

    Private Sub frmUtlCopyFrom_KeyUp(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyUp
        If e.KeyCode = Keys.F9 And e.Control Then
            Call initFormSize(Me, Me.Text, True, 450, 292, 0, 0)
            MsgBox("Restored Form Layout Successfully!!", vbInformation, Me.Text)
        End If
        If e.KeyCode = Keys.F8 And e.Control = True Then
            Call SaveActFormSettings(Me, Me.Text)
            MsgBox("Saved Form Layout Successfully!!", vbInformation, Me.Text)
        End If
    End Sub

    Private Sub frmUtlCopyFrom_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        initScreen()
        TabControl1.TabPages.Remove(TabPage2)
        Call initFormSize(Me, Me.Text, False, 450, 292, 0, 0)
    End Sub

    Private Sub initScreen()
        bindCombo(cboDocName, "DocName", "DocName", "Select DocName from UtlTransactions where DocType = 'Transaction'")
    End Sub

    Private Sub btnImport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnImport.Click
        'On Error GoTo Err
        'Dim i As Integer, colCount As Integer = 0, j As Integer
        'Dim xlApp As New Excel.Application
        'Dim strs As String = ""
        'strColList = ""
        ''Dim introw As Integer

        ''xlApp = New Excel.Application
        ''xlWorkBook = xlApp.Workbooks.Add(misValue)



        ''        If Trim(txtFilePath.Text) = "" Then
        ''            MsgBox("Please select file to import!", MsgBoxStyle.Information, Me.Text)
        ''            Exit Sub
        ''        End If

        'For i = 1 To argvsf.Cols.Count - 1
        '    Try
        '        If argvsf.GetCellStyle(0, i).ForeColor <> Color.Blue And argvsf.Cols(i).Visible = True Then
        '            strColList += UCase(Trim(argvsf.GetData(0, i)))
        '            colCount += 1
        '        End If
        '    Catch ex As Exception
        '    End Try
        'Next
        'strColList = strColList.Replace(" ", "")

        'xlApp.Workbooks.Open(txtFilePath.Text, , True)

        'For Each c In xlApp.Range("A1", Chr(64 + colCount Mod 26) & 1)
        '    strs += UCase(Trim(c.Value().ToString))
        'Next

        'strs = strs.Replace(" ", "")

        'If strColList <> strs Then
        '    MsgBox("Invalid file selected, please select the right file/refer template for correct column format!", vbInformation + vbOKOnly, Me.Text)
        '    xlApp = Nothing
        '    Exit Sub
        'End If

        'Dim k%, vCols%
        'With frmTrnSA.vsfDet
        '    vCols = 0
        '    For k = 0 To .Cols.Count - 1
        '        If .Cols(k).Visible Then
        '            vCols += 1
        '        End If
        '    Next
        'End With

        'frmTrnSA.vsfDet.Redraw = False

        'Dim m As Integer

        'If chkImpAppend.Checked = True Then
        '    frmTrnSA.vsfDet.Rows.Count = frmTrnSA.vsfDet.Rows.Count
        '    m = frmTrnSA.vsfDet.Rows.Count
        'Else
        '    m = 1
        '    frmTrnSA.vsfDet.Rows.Count = 1
        'End If

        'Dim r% = 2

        'Dim sheet As Excel.Worksheet = xlApp.Worksheets(1)

        'With xlApp
        '    Do While Trim(.Range("A" & r).Value) <> ""
        '        frmTrnSA.vsfDet.Rows.Count = frmTrnSA.vsfDet.Rows.Count + 1
        '        For i = 1 To colCount
        '            For j = 0 To frmTrnSA.vsfDet.Cols.Count - 1
        '                If UCase(Trim(sheet.Cells(1, i).value.ToString)).Replace(" ", "") = UCase(Trim(frmTrnSA.vsfDet.GetData(0, j))).Replace(" ", "") Then
        '                    frmTrnSA.vsfDet.SetData(m, j, sheet.Cells(r, i).value)

        '                    If UCase(Trim(sheet.Cells(1, i).value.ToString)).Replace(" ", "") = "ITEMCODE" Then
        '                        frmTrnSA.FillItemDet(sheet.Cells(r, i).value.ToString, 0, m)
        '                    End If

        '                    Exit For
        '                End If
        '            Next
        '        Next
        '        r += 1
        '        m = m + 1
        '    Loop
        'End With
        'ReArrangeSlNo(frmTrnSA.vsfDet, 0)
        'frmTrnSA.CalTotal()

        'frmTrnSA.vsfDet.Redraw = True

        ''i = 2
        ''With xlApp
        ''    frmTrnPartyDoc.vsfDet.Redraw = False

        ''    Do While Trim(.Range("B" & i).Value) <> ""
        ''        introw = frmTrnPartyDoc.vsfDet.Rows.Count
        ''        frmTrnPartyDoc.vsfDet.Rows.Count = frmTrnPartyDoc.vsfDet.Rows.Count + 1

        ''        frmTrnPartyDoc.vsfDet.SetData(introw, vDetSlno, introw)
        ''        frmTrnPartyDoc.vsfDet.SetData(introw, vDetItemCode, Trim(UCase(CType(.Range("B" & i).Value, String))))
        ''        frmTrnPartyDoc.vsfDet.SetData(introw, vDetItemName, Trim(UCase(CType(.Range("C" & i).Value, String))))
        ''        frmTrnPartyDoc.vsfDet.SetData(introw, vDetPriceRate, If(CType((.Range("D" & i).Value), String) = "" Or IsNumeric(CType((.Range("D" & i).Value), String)) = False, "0.00", CType((.Range("D" & i).Value), Integer)))
        ''        frmTrnPartyDoc.vsfDet.SetData(introw, vDetRemarks, Trim(UCase(CType(.Range("E" & i).Value, String))))

        ''        intItemId = clsSVT.getDataLongValue("select ItemId from MstItem where ItemCode  = '" & Trim(UCase(CType(.Range("B" & i).Value, String))) & "'", 0, False)

        ''        frmTrnPartyDoc.vsfDet.SetData(introw, vDetItemID, intItemId)

        ''        i += 1
        ''    Loop
        ''End With



        ' ''           
        ''i = 2

        ''With xlApp

        ''    vsfDet.Redraw = False

        ''    Do While Trim(.Range("B" & i).Value) <> ""
        ''        introw = vsfDet.Rows.Count
        ''        vsfDet.Rows.Count = vsfDet.Rows.Count + 1

        ''        vsfDet.SetData(introw, vDetSlno, introw)
        ''        vsfDet.SetData(introw, vDetItemCode, Trim(UCase(CType(.Range("B" & i).Value, String))))
        ''        vsfDet.SetData(introw, vDetItemName, Trim(UCase(CType(.Range("C" & i).Value, String))))
        ''        vsfDet.SetData(introw, vDetPriceRate, If(CType((.Range("D" & i).Value), String) = "" Or IsNumeric(CType((.Range("D" & i).Value), String)) = False, "0.00", CType((.Range("D" & i).Value), Integer)))
        ''        vsfDet.SetData(introw, vDetRemarks, Trim(UCase(CType(.Range("E" & i).Value, String))))

        ''        intItemId = clsSVT.getDataLongValue("select ItemId from MstItem where ItemCode  = '" & Trim(UCase(CType(.Range("B" & i).Value, String))) & "'", 0, False)

        ''        vsfDet.SetData(introw, vDetItemID, intItemId)

        ''        i += 1
        ''    Loop

        ''    vsfDet.AutoSizeCols(0, 1, 1)
        ''End With
        ''vsfDet.Redraw = True

        ''        Call ValidateEntry()
        ''        vsfDet.AutoSizeCols(0, 1, 1)
        ''        Call vsfDet.AutoSizeCols()
        ''        xlApp.Workbooks(1).Close(False)
        ''        xlApp = Nothing
        ''        Exit Sub
        'Me.Close()
    End Sub

    Private Sub txtFilePath_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtFilePath.DoubleClick
        Dim dlg As New OpenFileDialog()
        dlg.DefaultExt = "xlsx"
        dlg.Filter = "Excel File (*.xlsx)|*.xlsx"
        dlg.FileName = "*.xlsx"
        If dlg.ShowDialog() <> DialogResult.OK Then
            Return
            Exit Sub
        End If

        txtFilePath.Text = dlg.FileName
    End Sub

    Private Sub txtFilePath_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtFilePath.TextChanged
        If Trim(txtFilePath.Text) = "" Then
            btnImport.Enabled = False
        Else
            btnImport.Enabled = True
        End If
    End Sub

    Private Sub btnBrowse_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowse.Click
        Dim dlg As New OpenFileDialog()
        dlg.DefaultExt = "xlsx"
        dlg.Filter = "Excel File (*.xlsx)|*.xlsx"
        dlg.FileName = "*.xlsx"
        If dlg.ShowDialog() <> DialogResult.OK Then
            Return
            Exit Sub
        End If

        txtFilePath.Text = dlg.FileName
    End Sub

    Public Sub btnTemplate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnTemplate.Click
        'If gBlnMSOffice Then
        Dim xl As New Excel.Application
            Dim letter As String = "A"
            Dim fn As String = ""
            Dim xlApp As Excel.Application
            Dim xlWorkBook As Excel.Workbook
            Dim xlWorkSheet As Excel.Worksheet
            Dim misValue As Object = System.Reflection.Missing.Value
            Dim i As Integer, a As Integer = 0
            Dim j As Integer, b As Integer = 1
            Dim lastrow1, lastCol2 As Integer
            Dim SecondFileRange As String = ""

            Try
                xlApp = New Excel.Application
                xlWorkBook = xlApp.Workbooks.Add(misValue)
                xlWorkSheet = xlWorkBook.Sheets("sheet1")

                With argvsf
                    For i = 1 To .Cols.Count - 1
                        xlWorkSheet.Cells(1, i) = .Cols(i).Name.Trim
                    Next
                    i = 0
                    'For i = 0 To .Rows.Count
                    For j = 1 To .Cols.Count - 1
                        Try
                            a = i
                            If .GetCellStyle(0, j).ForeColor <> Color.Blue And .Cols(j).Visible = True Then
                                xlWorkSheet.Cells(a + 1, b) = argvsf.Rows(i).Item(j).ToString
                                a += 1
                                b += 1
                            End If

                        Catch ex As Exception
                        End Try
                    Next
                    'Next

                End With

                lastrow1 = xlWorkSheet.UsedRange.Rows.Count
                lastCol2 = xlWorkSheet.UsedRange.Columns.Count

                xlWorkSheet.UsedRange.Columns.AutoFit()

                If CInt(lastCol2 / 26) = 0 Then
                    xlWorkSheet.Range("A1", Chr(64 + lastCol2 Mod 26) & 1).Font.Bold = True
                    xlWorkSheet.Range("A1", Chr(64 + lastCol2 Mod 26) & lastrow1).Font.Size = 8
                    xlWorkSheet.Range("A1", Chr(64 + lastCol2 Mod 26) & 1).Font.Color = RGB(255, 255, 255)
                    xlWorkSheet.Range("A1", Chr(64 + lastCol2 Mod 26) & 1).Interior.Color = RGB(77, 93, 108)
                    xlWorkSheet.Range("A" & lastrow1, Chr(64 + lastCol2 Mod 26) & lastrow1).Font.Color = RGB(255, 255, 255)
                    xlWorkSheet.Range("A" & lastrow1, Chr(64 + lastCol2 Mod 26) & lastrow1).Interior.Color = RGB(77, 93, 108)
                Else
                    xlWorkSheet.Range("A1", Chr(64 + CInt(lastCol2 / 26)) & Chr(64 + lastCol2 Mod 26) & 1).Font.Bold = True
                    xlWorkSheet.Range("A1", Chr(64 + CInt(lastCol2 / 26)) & Chr(64 + lastCol2 Mod 26) & lastrow1).Font.Size = 8
                    xlWorkSheet.Range("A1", Chr(64 + IIf(CInt(lastCol2 / 26) = 0, 1, CInt(lastCol2 / 26))) & Chr(64 + lastCol2 Mod 26) & 1).Interior.Color = RGB(77, 93, 108)
                    xlWorkSheet.Range("A1", Chr(64 + IIf(CInt(lastCol2 / 26) = 0, 1, CInt(lastCol2 / 26))) & Chr(64 + lastCol2 Mod 26) & 1).Font.Color = RGB(255, 255, 255)
                    xlWorkSheet.Range("A" & lastrow1, Chr(64 + CInt(lastCol2 / 26)) & Chr(64 + lastCol2 Mod 26) & lastrow1).Font.Color = RGB(255, 255, 255)
                    xlWorkSheet.Range("A" & lastrow1, Chr(64 + CInt(lastCol2 / 26)) & Chr(64 + lastCol2 Mod 26) & lastrow1).Interior.Color = RGB(77, 93, 108)
                End If

                'xlWorkBook.Application.ActiveWindow.SplitColumn = 1
                'xlWorkBook.Application.ActiveWindow.FreezePanes = True

                xlApp.Visible = True
                xlApp.UserControl = True

                System.Runtime.InteropServices.Marshal.ReleaseComObject(xlWorkBook)
                System.Runtime.InteropServices.Marshal.ReleaseComObject(xlWorkSheet)
                System.Runtime.InteropServices.Marshal.ReleaseComObject(xlApp)

            Catch ex As Exception
                MsgBox(ex.Message, MsgBoxStyle.Information, "Export to Excel")
            End Try
        'Else

        ''Dim xl As New Excel.Application
        'Dim letter As String = "A"
        '    Dim fn As String = ""
        '    'Dim xlApp As Excel.Application
        '    'Dim xlWorkBook As Excel.Workbook
        '    'Dim xlWorkSheet As Excel.Worksheet

        '    Dim xlWB As New XLWorkbook()
        '    xlWB.Worksheets.Add("Sheet1")
        '    Dim xlWS As IXLWorksheet = xlWB.Worksheet("Sheet1")
        '    Dim usedRange As  = xlWS

        '    Dim misValue As Object = System.Reflection.Missing.Value
        '    Dim i As Integer, a As Integer = 0
        '    Dim j As Integer, b As Integer = 1
        '    Dim lastrow1, lastCol2 As Integer
        '    Dim SecondFileRange As String = ""

        '    Try
        '        'xlApp = New Excel.Application
        '        'xlWorkBook = xlApp.Workbooks.Add(misValue)
        '        'xlWorkSheet = xlWorkBook.Sheets("sheet1")

        '        With argvsf
        '            For i = 1 To .Cols.Count - 1
        '                xlWS.Cell(1, i).Value = .Cols(i).Name.Trim
        '            Next
        '            i = 0
        '            'For i = 0 To .Rows.Count
        '            For j = 1 To .Cols.Count - 1
        '                Try
        '                    a = i
        '                    If .GetCellStyle(0, j).ForeColor <> Color.Blue And .Cols(j).Visible = True Then
        '                        xlWS.Cell(a + 1, b).Value = argvsf.Rows(i).Item(j).ToString
        '                        a += 1
        '                        b += 1
        '                    End If

        '                Catch ex As Exception
        '                End Try
        '            Next
        '            'Next

        '        End With

        '        'lastrow1 = act.UsedRange.Rows.Count
        '        'lastCol2 = xlWS.UsedRange.Columns.Count

        '        'xlWS.UsedRange.Columns.AutoFit()

        '        'If CInt(lastCol2 / 26) = 0 Then
        '        '    xlWS.Range.Select("A1", Chr(64 + lastCol2 Mod 26) & 1).Font.Bold = True
        '        '    XLWorksheet.Range("A1", Chr(64 + lastCol2 Mod 26) & lastrow1).Font.Size = 8
        '        '    XLWorksheet.Range("A1", Chr(64 + lastCol2 Mod 26) & 1).Font.Color = RGB(255, 255, 255)
        '        '    XLWorksheet.Range("A1", Chr(64 + lastCol2 Mod 26) & 1).Interior.Color = RGB(77, 93, 108)
        '        '    XLWorksheet.Range("A" & lastrow1, Chr(64 + lastCol2 Mod 26) & lastrow1).Font.Color = RGB(255, 255, 255)
        '        '    XLWorksheet.Range("A" & lastrow1, Chr(64 + lastCol2 Mod 26) & lastrow1).Interior.Color = RGB(77, 93, 108)
        '        'Else
        '        '    XLWorksheet.Range("A1", Chr(64 + CInt(lastCol2 / 26)) & Chr(64 + lastCol2 Mod 26) & 1).Font.Bold = True
        '        '    XLWorksheet.Range("A1", Chr(64 + CInt(lastCol2 / 26)) & Chr(64 + lastCol2 Mod 26) & lastrow1).Font.Size = 8
        '        '    XLWorksheet.Range("A1", Chr(64 + IIf(CInt(lastCol2 / 26) = 0, 1, CInt(lastCol2 / 26))) & Chr(64 + lastCol2 Mod 26) & 1).Interior.Color = RGB(77, 93, 108)
        '        '    XLWorksheet.Range("A1", Chr(64 + IIf(CInt(lastCol2 / 26) = 0, 1, CInt(lastCol2 / 26))) & Chr(64 + lastCol2 Mod 26) & 1).Font.Color = RGB(255, 255, 255)
        '        '    XLWorksheet.Range("A" & lastrow1, Chr(64 + CInt(lastCol2 / 26)) & Chr(64 + lastCol2 Mod 26) & lastrow1).Font.Color = RGB(255, 255, 255)
        '        '    XLWorksheet.Range("A" & lastrow1, Chr(64 + CInt(lastCol2 / 26)) & Chr(64 + lastCol2 Mod 26) & lastrow1).Interior.Color = RGB(77, 93, 108)
        '        'End If

        '        'xlWorkBook.Application.ActiveWindow.SplitColumn = 1
        '        'xlWorkBook.Application.ActiveWindow.FreezePanes = True

        '        'xlApp.Visible = True
        '        'xlApp.UserControl = True

        '        System.Runtime.InteropServices.Marshal.ReleaseComObject(xlWB)
        '        System.Runtime.InteropServices.Marshal.ReleaseComObject(xlWS)
        '        'System.Runtime.InteropServices.Marshal.ReleaseComObject(xlApp)

        '    Catch ex As Exception
        '        MsgBox(ex.Message, MsgBoxStyle.Information, "Export to Excel")
        '    End Try
        'End If



    End Sub

    Private Sub cboDocName_SelectedValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboDocName.SelectedValueChanged
        txtDocNo.Text = ""
        txtDocID.Text = clsSVT.getDataLongValue("Select DocID from UtlTransactions where DocName = '" & Trim(cboDocName.Text) & "'", 0, False)
    End Sub

    Private Sub btnSelDoc_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSelDoc.Click
        Dim strSQL As String, strFMT As String
        strFMT = ""
        Dim rs As oledb.oledbDataReader


        If txtDocID.Text > 0 Then
            rs = clsSVT.getDataReaderFromSQL("Select * from UtlTransactions where DocID = " & Trim(txtDocID.Text))
            With rs
                If .HasRows Then
                    .Read()
                    strSHForm = GetVal(!DocShForm, "")
                    strDocNoFldName = strSHForm & "NO"
                    strDocIDFldName = strSHForm & "ID"
                    strHdrTbl = "TrnHdr" & strSHForm
                    strDtlTbl = "TrnDtl" & strSHForm
                    'strCustSuppReq = GetVal(!CustSuppReq, "")
                End If
                .Close()
            End With
        Else
            Exit Sub
        End If

        strSQL = getShowFindQry(strHdrTbl, strFMT, "")
        Dim frmDisplay As New frmUtlSingleSel
        With frmDisplay
            '.MdiParent = frmMain
            .Text = "Select from... "
            Call .FillViewVsf(Me, strSQL, txtDocNo, 1, 0)
            .Show()
            .vsfDet.Focus()
        End With

    End Sub

    Private Sub txtDocNo_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDocNo.DoubleClick
        btnSelDoc.PerformClick()
    End Sub

    Private Sub btnLoad_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnLoad.Click
        Dim blnAppend As Boolean = False
        If Trim(txtDocNo.Text) = "" Then
            MsgBox("Please select a document to load!", vbInformation, Me.Text)
            btnSelDoc.Focus()
            Exit Sub
        End If

        If chkAppendData.Checked Then
            blnAppend = True
        End If

        With argForm
            .fillCopyFromDoc(Trim(txtDocNo.Text), Trim(txtID.Text), strHdrTbl, strDtlTbl, strDocNoFldName, strDocIDFldName, strCustSuppReq, strSHForm, blnAppend)
        End With

        If chkCloseWindow.Checked = False Then
            Me.Close()
        End If
    End Sub

    Private Sub txtDocNo_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDocNo.TextChanged
        txtID.Text = clsSVT.getDataLongValue("Select " & strDocIDFldName & " from " & strHdrTbl & " where " & strDocNoFldName & " = '" & Trim(txtDocNo.Text) & "'", 0, False)
    End Sub
End Class