Imports C1.Win.C1FlexGrid
Imports Microsoft.Office.Interop
Imports ClosedXML.Excel

Public Class ucBtn
    Dim clsSVT As New svtLibrary
    Event btnClicked(ByVal btnValue As Int32)
    Event btnOptionClicked(ByVal argValue As String)

    Public intCopiedRow As Int32

    Private Sub tsbInsert_Click(sender As System.Object, e As System.EventArgs) Handles tsbInsert.Click
        RaiseEvent btnClicked(svtDtlBtns.svtInsert)
    End Sub

    Private Sub tsbRemove_Click(sender As System.Object, e As System.EventArgs) Handles tsbRemove.Click
        RaiseEvent btnClicked(svtDtlBtns.svtRemove)
    End Sub

    Private Sub tsbCopy_Click(sender As System.Object, e As System.EventArgs) Handles tsbCopy.Click
        RaiseEvent btnClicked(svtDtlBtns.svtCopy)
    End Sub

    Private Sub tsbPaste_Click(sender As System.Object, e As System.EventArgs) Handles tsbPaste.Click
        RaiseEvent btnClicked(svtDtlBtns.svtPaste)
    End Sub

    Private Sub tsbMoveUp_Click(sender As System.Object, e As System.EventArgs) Handles tsbMoveUp.Click
        RaiseEvent btnClicked(svtDtlBtns.svtUp)
    End Sub

    Private Sub tsbMoveDown_Click(sender As System.Object, e As System.EventArgs) Handles tsbMoveDown.Click
        RaiseEvent btnClicked(svtDtlBtns.svtDown)
    End Sub

    Private Sub tsbReplace_Click(sender As System.Object, e As System.EventArgs) Handles tsbReplace.Click
        RaiseEvent btnClicked(svtDtlBtns.svtReplace)
    End Sub

    Private Sub tsbExport_Click(sender As System.Object, e As System.EventArgs) Handles tsbExport.Click
        RaiseEvent btnClicked(svtDtlBtns.svtExport)
    End Sub

    Private Sub tsbImport_Click(sender As System.Object, e As System.EventArgs) Handles tsbImport.Click
        RaiseEvent btnClicked(svtDtlBtns.svtImport)
    End Sub

    Private Sub tsbTemplate_Click(sender As System.Object, e As System.EventArgs) Handles tsbTemplate.Click
        RaiseEvent btnClicked(svtDtlBtns.svtTemplate)
    End Sub

    Private Sub tsbAttachment_Click(sender As System.Object, e As System.EventArgs) Handles tsbAttachment.Click
        RaiseEvent btnClicked(svtDtlBtns.svtAttach)
    End Sub

    Private Sub tsbRestore_Click(sender As System.Object, e As System.EventArgs) Handles tsbRestore.Click
        RaiseEvent btnClicked(svtDtlBtns.svtRestore)
    End Sub

    Public Sub btnVisible(ByVal argName As String, ByVal argFlag As String, ByVal argLinkedDoc As Boolean)
        'For Each btn As ToolStripButton In tsBtn.Items
        '    btn.Visible = False
        'Next
        'If UCase(argName) = "DETAIL" Then
        '    For Each btn As ToolStripButton In tsBtn.Items
        '        btn.Visible = True
        '    Next
        'Else
        '    tsbAttachment.Visible = True
        'End If
        If argLinkedDoc = False Then
            tsbInsert.Visible = argFlag
            tsbCopy.Visible = argFlag
            tsbPaste.Visible = argFlag
            tsbAddItem.Visible = argFlag
            tsbMoveDown.Visible = argFlag
            tsbMoveUp.Visible = argFlag
        Else
            tsbInsert.Visible = False
            tsbCopy.Visible = False
            tsbPaste.Visible = False
            tsbAddItem.Visible = False
            tsbMoveDown.Visible = False
            tsbMoveUp.Visible = False
        End If
        tsbRemove.Visible = argFlag
        tsbAddnl.Visible = argFlag
        tsbReplace.Visible = argFlag
        tsbRestore.Visible = argFlag
        tsbAttachment.Visible = argFlag
        GSeparator1.Visible = argFlag
        GSeparator2.Visible = argFlag
        GSeparator3.Visible = argFlag
        GSeparator4.Visible = argFlag
        GSeparator5.Visible = argFlag
        GSeparator6.Visible = argFlag
        GSeparator7.Visible = argFlag
    End Sub

    Public Function vsfInsert(ByVal argVSF As C1FlexGrid, ByVal argRow As Int32, ByVal argCol As Int32) As Boolean
        vsfInsert = False
        Try
            If GetVal(argVSF.GetData(argRow, argCol), "") = "" Then
                MsgBox("Insert failed! No item present in Selected row.", MsgBoxStyle.Exclamation, Me.Text)
                Exit Function
            End If

            With argVSF
                .Rows.Insert(argRow + 1)
            End With
            vsfInsert = True
        Catch ex As Exception
            vsfInsert = False
        End Try
    End Function

    Public Function vsfRemove(ByVal argVSF As C1FlexGrid, ByVal argRow As Int32, ByRef argcol As Int32) As Boolean
        vsfRemove = False
        Try
            If GetVal(argVSF.GetData(argRow, argcol), "") <> "" Then
                If MsgBox("Are you sure, you want to remove selected item?", vbYesNo + MsgBoxStyle.DefaultButton2, "Deletion Confirmation...") = vbNo Then Exit Function
            End If
            With argVSF
                .Rows.Remove(argRow)
            End With
            If argVSF.Rows.Count = 1 Then argVSF.Rows.Count = 2
            vsfRemove = True
        Catch ex As Exception
            vsfRemove = False
        End Try
    End Function

    Public Function vsfCopy(ByVal argVSF As C1FlexGrid, ByVal argRow As Int32, argCol As Int32) As Boolean
        vsfCopy = False
        Try
            If argRow < 1 Then
                MsgBox("You can copy data from second row onwards!", MsgBoxStyle.Exclamation, "Invalid Selection")
                Exit Function
            End If

            If GetVal(argVSF.GetData(argRow, argCol), "") = "" Then
                MsgBox("Copy failed! No item present in Selected row.", MsgBoxStyle.Exclamation, Me.Text)
                Exit Function
            End If

            If intCopiedRow > 0 Then
                If MsgBox("Already some data is available. Click yes to clear the old data", MsgBoxStyle.Information, vbYesNo) = MsgBoxResult.No Then
                    Exit Function
                End If
            End If

            intCopiedRow = argRow

            MsgBox("Selected Row Copied...", MsgBoxStyle.Information, "Copy Process")
            vsfCopy = True
        Catch ex As Exception
            vsfCopy = False
        End Try
    End Function

    Public Function vsfPaste(ByVal argVSF As C1FlexGrid, ByVal argRow As Int32, argCol As Int32) As Boolean
        vsfPaste = False
        Try
            If intCopiedRow = 0 Then
                MsgBox("No Copied Row available in the memory....!", MsgBoxStyle.Exclamation, "Invalid Selection")
                Exit Function
            End If

            Dim intRow As Integer


            With argVSF
                If GetVal(.GetData(argRow, argCol), "") <> "" Then
                    .Rows.Insert(argRow + 1)
                    intRow = argRow + 1
                Else
                    intRow = argRow
                End If
                .Row = intRow


                'If argRow <= intCopiedRow Then
                '    intCopiedRow = intCopiedRow + 1
                'End If

                For i = 0 To .Cols.Count - 1
                    .SetData(intRow, i, .GetData(intCopiedRow, i))
                Next
            End With
            intCopiedRow = 0
            MsgBox("Selected Row Pasted...", MsgBoxStyle.Information, "Copy Process")
            vsfPaste = True
        Catch ex As Exception
            vsfPaste = False
        End Try
    End Function

    Public Function vsfReplace(ByVal argVSF As C1FlexGrid, ByVal argRow As Int32, ByVal argCol As Int32, ByVal argIdCol As Int32) As Boolean
        vsfReplace = False
        Try
            With argVSF
                'If argVSF.Col < intDetCols Then
                '    MsgBox("Replace rest not allowed for fixed columns!", vbInformation, Me.Text)
                '    Exit Function
                'End If
                If argCol = argIdCol Then
                    MsgBox("Replace rest not allowed for ItemCode column!", vbInformation, Me.Text)
                    Exit Function
                End If
                If GetVal(.GetData(argRow, argCol), "") = "" Then
                    MsgBox("No data present!", MsgBoxStyle.Exclamation, "Invalid Selection")
                    Exit Function
                End If

                If .Rows.Count <= 2 Then
                    MsgBox("Please ensure there are 2 or more records before using replace rest!", MsgBoxStyle.Exclamation, "Invalid Selection")
                    Exit Function
                End If


                Dim i As Integer, strTxt As String

                Dim rgBlue As CellRange = .GetCellRange(0, argCol)
                If rgBlue.Style.ForeColor.Name = "Blue" Then
                    MsgBox("Column is readonly! Cannot use Replace Rest!", MsgBoxStyle.Information, Me.Text)
                    Exit Function
                End If


                strTxt = GetVal(.GetData(argRow, argCol), "")
                If strTxt = "" Then GetVal(.Editor.Text, "")
                For i = argRow To .Rows.Count - 1
                    If .GetData(i, argIdCol) <> "" Then
                        .SetData(i, argCol, strTxt)
                    End If
                Next i
            End With

            vsfReplace = True
        Catch ex As Exception
            vsfReplace = False
        End Try
    End Function

    Public Function vsfExport(ByVal argVSF As C1FlexGrid, Optional ByVal argFileName As String = "") As Boolean
        vsfExport = False
        Try
            Call ExportToXL(argVSF, argFileName:=argFileName)
            vsfExport = True
        Catch ex As Exception
            vsfExport = False
        End Try
    End Function

    Public Function vsfImport(ByVal argVSF As C1FlexGrid, ByVal argCol As Int32, ByVal argVSFOpt As C1FlexGrid, ByVal argintDetCols As Int32) As Boolean
        vsfImport = False
        Try
            Dim strPath As String
            Cdlg.DefaultExt = "xlsx"
            Cdlg.Filter = "Excel File (*.xlsx)|*.xlsx"
            Cdlg.FileName = "*.xlsx"

            If Cdlg.ShowDialog() <> DialogResult.OK Then
                Return False
            End If
            strPath = Cdlg.FileName
            argVSF.Rows.Count = 1

            Dim xlWS As New Excel.Worksheet
            Dim xlapp As New Excel.Application
            xlapp.Workbooks.Open(strPath, , True)
            xlWS = xlapp.Sheets(1)

            With argVSF
                Dim intRow As Int32 = 0
                Dim intCol As Int32 = 0
                For intCol = 0 To .Cols.Count - 1
                    If UCase(GetVal(.GetData(intRow, intCol), "")) <> UCase(GetVal(xlWS.Cells(intRow + 1, intCol + 1).Value, "")) Then
                        MsgBox("Column '" & GetVal(xlWS.Cells(intRow + 1, intCol + 1).Value, "") & "' does not match with grid...", MsgBoxStyle.Exclamation, "XL Import Failed")
                        Return False
                    End If
                Next

                For intRow = 1 To 500
                    If GetVal(xlWS.Cells(intRow + 1, argCol + 1).Value, "") = "" Then Exit For
                    For intCol = 0 To .Cols.Count - 1
                        If argVSFOpt.GetData(intCol, vOthFldBlnReadOnly) = "N" Then
                            If intCol >= argintDetCols AndAlso argVSFOpt.GetData(intCol, vOthFldBlnMandatory) = "Y" Then
                                Select Case .Cols(intCol).DataType
                                    Case GetType(Boolean), GetType(Date)
                                    Case GetType(Double), GetType(Integer), GetType(Decimal)
                                        If GetVal(xlWS.Cells(intRow + 1, intCol + 1).Value, 0) = 0 Then
                                            MsgBox("Mandatory Column '" & .GetData(0, intCol) & "' Value should be greater than 0 in excel...", MsgBoxStyle.Exclamation, "XL Import Failed")
                                            Return False
                                        End If
                                    Case Else
                                        If GetVal(CStr(xlWS.Cells(intRow + 1, intCol + 1).Value), "") = "" Then
                                            MsgBox("Mandatory Column '" & .GetData(0, intCol) & "' does not contain value in excel...", MsgBoxStyle.Exclamation, "XL Import Failed")
                                            Return False
                                        End If
                                End Select
                            End If
                        End If
                    Next
                Next

                For intRow = 1 To 500
                    If GetVal(xlWS.Cells(intRow + 1, argCol + 1).Value, "") = "" Then Exit For
                    .Rows.Count = .Rows.Count + 1
                    For intCol = 0 To .Cols.Count - 1
                        Select Case .Cols(intCol).DataType
                            Case GetType(Boolean)
                                .SetData(intRow, intCol, GetVal(xlWS.Cells(intRow + 1, intCol + 1).Value, False))
                            Case GetType(Date)
                                .SetData(intRow, intCol, GetVal(xlWS.Cells(intRow + 1, intCol + 1).Value, ""))
                            Case GetType(Double), GetType(Integer), GetType(Decimal)
                                .SetData(intRow, intCol, GetVal(xlWS.Cells(intRow + 1, intCol + 1).Value, ""))
                            Case Else
                                .SetData(intRow, intCol, GetVal(xlWS.Cells(intRow + 1, intCol + 1).Value, ""))
                        End Select
                    Next
                Next
                vsfImport = True
            End With
        Catch ex As Exception
            vsfImport = False
        End Try
    End Function

    Public Function vsfImport_ClosedXML(ByVal argVSF As C1FlexGrid, ByVal argCol As Int32, ByVal argVSFOpt As C1FlexGrid, ByVal argintDetCols As Int32) As Boolean
        vsfImport_ClosedXML = False
        Try
            Dim strPath As String
            Cdlg.DefaultExt = "xlsx"
            Cdlg.Filter = "Excel File (*.xlsx)|*.xlsx"
            Cdlg.FileName = "*.xlsx"

            If Cdlg.ShowDialog() <> DialogResult.OK Then
                Return False
            End If
            strPath = Cdlg.FileName
            argVSF.Rows.Count = 1

            Dim xlWB As New XLWorkbook(strPath)
            Dim xlWS As IXLWorksheet = xlWB.Worksheet(1)

            With argVSF
                Dim intRow As Int32 = 0
                Dim intCol As Int32 = 0
                For intCol = 0 To .Cols.Count - 1
                    If UCase(GetVal(.GetData(intRow, intCol), "")) <> UCase(GetVal(xlWS.Cell(intRow + 1, intCol + 1).Value, "")) Then
                        MsgBox("Column '" & GetVal(xlWS.Cell(intRow + 1, intCol + 1).Value, "") & "' does not match with grid...", MsgBoxStyle.Exclamation, "XL Import Failed")
                        Return False
                    End If
                Next

                For intRow = 1 To 500
                    If GetVal(xlWS.Cell(intRow + 1, argCol + 1).Value, "") = "" Then Exit For
                    For intCol = 0 To .Cols.Count - 1
                        If argVSFOpt.GetData(intCol, vOthFldBlnReadOnly) = "N" Then
                            If intCol >= argintDetCols AndAlso argVSFOpt.GetData(intCol, vOthFldBlnMandatory) = "Y" Then
                                Select Case .Cols(intCol).DataType
                                    Case GetType(Boolean), GetType(Date)
                                    Case GetType(Double), GetType(Integer), GetType(Decimal)
                                        If GetVal(xlWS.Cell(intRow + 1, intCol + 1).Value, 0) = 0 Then
                                            MsgBox("Mandatory Column '" & .GetData(0, intCol) & "' Value should be greater than 0 in excel...", MsgBoxStyle.Exclamation, "XL Import Failed")
                                            Return False
                                        End If
                                    Case Else
                                        If GetVal(CStr(xlWS.Cell(intRow + 1, intCol + 1).Value), "") = "" Then
                                            MsgBox("Mandatory Column '" & .GetData(0, intCol) & "' does not contain value in excel...", MsgBoxStyle.Exclamation, "XL Import Failed")
                                            Return False
                                        End If
                                End Select
                            End If
                        End If
                    Next
                Next

                For intRow = 1 To 500
                    If GetVal(xlWS.Cell(intRow + 1, argCol + 1).Value, "") = "" Then Exit For
                    .Rows.Count = .Rows.Count + 1
                    For intCol = 0 To .Cols.Count - 1
                        Select Case .Cols(intCol).DataType
                            Case GetType(Boolean)
                                .SetData(intRow, intCol, GetVal(xlWS.Cell(intRow + 1, intCol + 1).Value, False))
                            Case GetType(Date)
                                .SetData(intRow, intCol, GetVal(xlWS.Cell(intRow + 1, intCol + 1).Value, ""))
                            Case GetType(Double), GetType(Integer), GetType(Decimal)
                                .SetData(intRow, intCol, GetVal(xlWS.Cell(intRow + 1, intCol + 1).Value, ""))
                            Case Else
                                .SetData(intRow, intCol, GetVal(xlWS.Cell(intRow + 1, intCol + 1).Value, ""))
                        End Select
                    Next
                Next
                vsfImport_ClosedXML = True
            End With
        Catch ex As Exception
            vsfImport_ClosedXML = False
        End Try
    End Function

    Public Function vsfTemplate(ByVal argVSF As C1FlexGrid, argFileName As String, ByVal argVSFOpt As C1FlexGrid) As Boolean
        vsfTemplate = False
        Dim xlapp As New Excel.Application
        Dim xlWB As Excel.Workbook = xlapp.Workbooks.Add()
        Dim xlWS As Excel.Worksheet = xlWB.Worksheets.Add
        Try
            Dim i As Int32 = 0
            With argVSF
                For j = 0 To .Cols.Count - 1
                    If GetVal(argVSFOpt.GetData(j, vOthFldBlnReadOnly), "") <> "N" Then
                        xlWS.Columns(j + 1).entirecolumn.hidden = True
                        If GetVal(.GetData(i, j), "") <> "" Then xlWS.Cells(i + 1, j + 1).Value = .GetData(i, j)
                    Else
                        xlWS.Cells(i + 1, j + 1).Value = .GetData(i, j)
                        Dim rgBlue As CellRange = .GetCellRange(i, j)
                        If Not IsNothing(rgBlue) AndAlso Not IsNothing(rgBlue.Style) Then
                            Dim strColor As String = CStr(rgBlue.Style.ForeColor.Name & "")
                            If UCase(strColor) = "BROWN" Then
                                xlWS.Cells(i + 1, j + 1).Style.Font.Color = Color.FromName(strColor)
                            Else
                                xlWS.Cells(i + 1, j + 1).Style.Font.Color = Color.Black
                            End If
                        End If
                    End If
                Next
            End With
            xlWB.SaveAs(argFileName & ".xlsx")
            xlWB.Close()
            xlapp.Quit()
            'xlWS.Dispose()
            MsgBox("Templated Created", MsgBoxStyle.Information, "Process Completed")
            System.Diagnostics.Process.Start(argFileName & ".xlsx")
        Catch ex As Exception
            xlWB.Close()
            'xlWS.Dispose()
            xlapp.Quit()
            MsgBox("Template Excel Generation Failed. Details" & vbNewLine & ex.Message, MsgBoxStyle.Information, "Failed")
            vsfTemplate = False
        End Try
    End Function

    Public Function vsfTemplate_ClosedXML(ByVal argVSF As C1FlexGrid, argFileName As String, ByVal argVSFOpt As C1FlexGrid) As Boolean
        vsfTemplate_ClosedXML = False
        Dim xlWB As New XLWorkbook()
        xlWB.Worksheets.Add("Sheet1")
        Dim xlWS As IXLWorksheet = xlWB.Worksheet("Sheet1")
        Try
            Dim i As Int32 = 0
            With argVSF
                For j = 0 To .Cols.Count - 1
                    If GetVal(argVSFOpt.GetData(j, vOthFldBlnReadOnly), "") <> "N" Then
                        xlWS.Column(j + 1).Hide()
                        If GetVal(.GetData(i, j), "") <> "" Then xlWS.Cell(i + 1, j + 1).Value = .GetData(i, j)
                    Else
                        xlWS.Cell(i + 1, j + 1).Value = .GetData(i, j)
                        Dim rgBlue As CellRange = .GetCellRange(i, j)
                        If rgBlue.Style.ForeColor.Name.ToString().ToUpper() = "RED" Then
                            xlWS.Cell(i + 1, j + 1).Style.Font.FontColor = XLColor.Red
                        Else
                            xlWS.Cell(i + 1, j + 1).Style.Font.FontColor = XLColor.Black
                        End If
                    End If
                Next
            End With
            xlWB.SaveAs(argFileName & ".xlsx")
            xlWB.Dispose()
            xlWS.Dispose()
            MsgBox("Templated Created", MsgBoxStyle.Information, "Process Completed")
            System.Diagnostics.Process.Start(argFileName & ".xlsx")
        Catch ex As Exception
            xlWB.Dispose()
            xlWS.Dispose()
            MsgBox("Template Excel Generation Failed. Details" & vbNewLine & ex.Message, MsgBoxStyle.Information, "Failed")
            vsfTemplate_ClosedXML = False
        End Try
    End Function

    Public Function vsfRestore(ByVal argVSF As C1FlexGrid) As Boolean
        vsfRestore = False
    End Function

    Public Sub vsfAttach(ByVal argobj As Object, ByVal argRights As String, ByVal argDocID As Integer, argDocValueID As Integer)   ' attachment code
        If Not chkUserRights(argRights, chkRightsPos.ViewRights, Me.Text) Then Exit Sub
        With frmUtlFileControl
            .strRights = argRights
            .argOwnerCaption = argobj.Text
            .argDocID = argDocID
            .argDocValueID = argDocValueID
            .StartPosition = FormStartPosition.CenterScreen
            .ShowDialog()
        End With
    End Sub

    Private Sub tsbAddItem_Click(sender As System.Object, e As System.EventArgs) Handles tsbAddItem.Click
        RaiseEvent btnClicked(svtDtlBtns.svtAddItems)
    End Sub

    Public Function getItems() As String
        getItems = ""
        Dim obj As New frmUtlAddItems
        obj.WindowState = FormWindowState.Normal
        obj.StartPosition = FormStartPosition.CenterScreen
        obj.ShowDialog()
    End Function

    Private Sub tsbCopyFrom_Click(sender As System.Object, e As System.EventArgs) Handles tsbCopyFrom.Click
        RaiseEvent btnClicked(svtDtlBtns.svtCopyFrom)
    End Sub

    Private Sub tsbAppend_Click(sender As System.Object, e As System.EventArgs) Handles tsbAmmend.Click
        RaiseEvent btnClicked(svtDtlBtns.svtAmmend)
    End Sub

    Private Sub tsbFreeze_Click(sender As System.Object, e As System.EventArgs) Handles tsbFreeze.Click
        RaiseEvent btnClicked(svtDtlBtns.svtFreeze)
    End Sub

    Public Function vsfFreeze(ByVal argVSF As C1FlexGrid, ByVal argCol As Int32) As Boolean
        vsfFreeze = False
        Dim tooltip As New ToolTip
        If InStr(UCase(tsbFreeze.Text), "UNFREEZE") > 0 Then
            tsbFreeze.Text = "Freeze : Ctrl+Shift+Z"
            argVSF.Cols.Frozen = 0
        Else
            tsbFreeze.Text = "UnFreeze : Ctrl+Shift+Z"
            argVSF.Cols.Frozen = argCol
        End If
    End Function

    Private Sub tsbTender_Click(sender As System.Object, e As System.EventArgs) Handles tsbTender.Click
        RaiseEvent btnClicked(svtDtlBtns.svtTender)
    End Sub

    Private Sub tsbInserDoc_Click(sender As System.Object, e As System.EventArgs) Handles tsbInserDoc.Click
        RaiseEvent btnClicked(svtDtlBtns.svtInsertDoc)
    End Sub

    Private Sub tsbMultiDoc_Click(sender As System.Object, e As System.EventArgs) Handles tsbMultiDoc.Click
        RaiseEvent btnClicked(svtDtlBtns.svtMultiDoc)
    End Sub

    Public Function vsfMultDoc(ByVal argDocID As String, ByVal argFrm As Form, ByVal argDocNo As String) As Boolean
        vsfMultDoc = False
        With frmUtlGenDoc
            .strDocNo = argDocNo
            .getUtlDetails(argDocID)
            .intDocID = argDocID
            .argFrm = argFrm
            .ShowDialog()
        End With
    End Function

    Public Function vsfInsertDoc(ByVal argDocID As String, ByVal argFrm As Form, ByVal argItemDocID As Int32, ByVal argPartyDocID As Int32, ByVal argColQty As Int32,
                                 ByVal argColRate As Int32, ByVal argTotalCol As Int32, ByVal argParty As String) As Boolean
        vsfInsertDoc = False
        Dim frmInsertFromDoc As New frmUtlInsertfromDoc
        With frmInsertFromDoc
            .intDocID = argDocID
            .argFrm = argFrm
            .strDocHdrTblName = clsSVT.getDataStringValue("Select HdrTblName from utlTransactions Where DocID=" & argDocID, "", False)
            .intPartyDocID = argPartyDocID
            .intItemDocID = argItemDocID
            .intColQty = argColQty
            .intColrate = argColRate
            .intDetCols = argTotalCol
            .strPartyCode = argParty
            .ShowDialog()
        End With
    End Function

    Public Function vsfTenderChange(ByVal argDocID As Int32, ByVal argTrnID As Int32, ByVal argBill As String, ByVal argBillAmt As Double) As Boolean
        'vsfTenderChange = True
        'Dim obj As New frmTrnTender
        'With obj
        '    .intTrnID = argTrnID
        '    .intDocID = argDocID
        '    .strBillNo = argBill
        '    .dblBillAmt = argBillAmt
        '    .ShowDialog()
        '    .Dispose()
        '    .Close()
        'End With
    End Function

    Public Sub setFocus()

    End Sub

    Public Sub HideTotal(ByVal argVal As String)
        If argVal <> "Y" Then
            tsbCalTotal.Visible = False
        End If
    End Sub

    Public Sub HideBOM(ByVal argVal As String)
        If argVal <> "Y" Then
            tsbBOM.Visible = False
        End If
    End Sub

    Public Sub HideCopyFrom(ByVal argVal As String)
        If argVal <> "Y" Then
            tsbCopyFrom.Visible = False
        End If
    End Sub

    Public Sub HideAttachment(ByVal argVal As String)
        If argVal <> "Y" Then
            tsbAttachment.Visible = False
        End If
    End Sub

    Public Sub HideAmmend(ByVal argVal As String)
        If argVal <> "Y" Then
            tsbAmmend.Visible = False
        End If
    End Sub

    Public Sub HideMultiDoc(ByVal argVal As String)
        If argVal <> "Y" Then
            tsbMultiDoc.Visible = False
        End If
    End Sub

    Public Sub HideInsertDoc(ByVal argVal As String)
        If argVal <> "Y" Then
            tsbInserDoc.Visible = False
        End If
    End Sub

    Public Sub HideFreeze(ByVal argVal As String)
        If argVal <> "Y" Then
            tsbFreeze.Visible = False
        End If
    End Sub

    Public Sub HidePending(ByVal argVal As String)
        If argVal <> "Y" Then
            tsbPending.Visible = False
        End If
    End Sub

    Public Sub HideTender(ByVal argVal As String)
        If argVal <> "Y" Then
            tsbTender.Visible = False
        End If
    End Sub

    Public Sub HideImpExp(ByVal argVal As String)
        If argVal <> "Y" Then
            tsbTemplate.Visible = False
            tsbImport.Visible = False
        End If
    End Sub

    Public Sub HideSMS(ByVal argVal As String)
        If argVal <> "Y" Then
            tsbSMS.Visible = False
        Else
            tsbSMS.Visible = True
        End If
    End Sub

    Public Sub vsfBOM(ByVal argFrm As Object, ByVal argItemCodeCol As Int32, argQtyCol As Integer)
        Dim frmTrnBOMDetails As New frmTrnBOMDetails
        With frmTrnBOMDetails
            .argForm = argFrm
            .intColItemCode = argItemCodeCol
            .intColQty = argQtyCol
            .ShowDialog()
        End With
    End Sub

    Public Sub DocXlExport(argFrm As Form, strSHForm As String, argHdr As C1FlexGrid, argDet As C1FlexGrid, argFtr As C1FlexGrid, argTot As C1FlexGrid,
                        argDocId As Int32, argDocNo As String, argDocDate As Date, argPFX As String, argParty As String)
        Dim i As Int32, j As Int32
        Dim RowNo As Int32
        Dim intDetCol As Int32 = 0
        Dim strFileName As String = argFrm.Text & " " & argDocNo & ".xlsx"
        If System.IO.File.Exists(strFileName) Then
            IO.File.Delete(strFileName)
        End If


        Dim xlapp As New Excel.Application
        Dim xlWB As Excel.Worksheet = xlapp.Workbooks.Add()
        Dim xlWS As Excel.Worksheets = xlWB.Worksheet()

        ' Dim xlWB As New XLWorkbook()
        ' xlWB.Worksheets.Add("Sheet1")
        ' Dim xlWS As IXLWorksheet = xlWB.Worksheet("Sheet1")
        Try
            RowNo = 3

            'Get Total visible columns in Detail section
            For i = 0 To argDet.Cols.Count - 1
                If argDet.Cols(i).Visible = True Then
                    intDetCol = intDetCol + 1
                End If
            Next
            intDetCol = intDetCol
            If intDetCol < 11 Then intDetCol = 11

            With xlWS
                'Transaction Header
                .Range(1, 1, 1, intDetCol).Merge()
                .Range(1, 1, 1, intDetCol).Value = argFrm.Text
                .Range(1, 1, 1, intDetCol).Style.Font.FontSize = 14
                .Range(1, 1, 1, intDetCol).Style.Font.Bold = True
                .Range(1, 1, 1, intDetCol).Style.Alignment.Horizontal = xlapp.XLAlignmentHorizontalValues.Center
                .Range(1, 1, 1, intDetCol).Style.Border.OutsideBorder = xlapp.XLBorderStyleValues.Thick

                'Transaction Document Number
                .Range(RowNo, 1, RowNo, 2).Merge()
                .Range(RowNo, 1, RowNo, 2).Value = strSHForm & " No."
                .Range(RowNo, 1, RowNo, 2).Style.Border.OutsideBorder = xlapp.XLBorderStyleValues.Thin
                .Range(RowNo, 1, RowNo, 2).Style.Font.Bold = True
                .Range(RowNo, 3, RowNo, 5).Merge()
                .Range(RowNo, 3, RowNo, 5).Value = argDocNo
                .Range(RowNo, 3, RowNo, 5).Style.Border.OutsideBorder = xlapp.XLBorderStyleValues.Thin

                'Transaction Document Date
                RowNo = RowNo + 1
                .Range(RowNo, 1, RowNo, 2).Merge()
                .Range(RowNo, 1, RowNo, 2).Value = strSHForm & " Date."
                .Range(RowNo, 1, RowNo, 2).Style.Border.OutsideBorder = xlapp.XLBorderStyleValues.Thin
                .Range(RowNo, 1, RowNo, 2).Style.Font.Bold = True
                .Range(RowNo, 3, RowNo, 5).Merge()
                .Range(RowNo, 3, RowNo, 5).Value = argDocDate
                .Range(RowNo, 3, RowNo, 5).Style.Border.OutsideBorder = xlapp.XLBorderStyleValues.Thin


                'Transaction Party Caption
                'Code, Name, Address, City, State, Pincode, Country
                RowNo = RowNo + 2
                .Range(RowNo, 1, RowNo, 2).Merge()
                .Range(RowNo, 1, RowNo, 2).Value = "Code"
                .Range(RowNo, 1, RowNo, 2).Style.Font.Bold = True
                .Range(RowNo, 1, RowNo, 2).Style.Border.OutsideBorder = xlapp.XLBorderStyleValues.Thin

                .Range(RowNo + 1, 1, RowNo + 1, 2).Merge()
                .Range(RowNo + 1, 1, RowNo + 1, 2).Value = "Name"
                .Range(RowNo + 1, 1, RowNo + 1, 2).Style.Font.Bold = True
                .Range(RowNo + 1, 1, RowNo + 1, 2).Style.Border.OutsideBorder = xlapp.XLBorderStyleValues.Thin

                .Range(RowNo + 2, 1, RowNo + 2, 2).Merge()
                .Range(RowNo + 2, 1, RowNo + 2, 2).Value = "Address"
                .Range(RowNo + 2, 1, RowNo + 2, 2).Style.Font.Bold = True
                .Range(RowNo + 2, 1, RowNo + 2, 2).Style.Border.OutsideBorder = xlapp.XLBorderStyleValues.Thin

                .Range(RowNo + 3, 1, RowNo + 3, 2).Merge()
                .Range(RowNo + 3, 1, RowNo + 3, 2).Style.Border.OutsideBorder = xlapp.XLBorderStyleValues.Thin

                .Range(RowNo + 4, 1, RowNo + 4, 2).Merge()
                .Range(RowNo + 4, 1, RowNo + 4, 2).Style.Border.OutsideBorder = xlapp.XLBorderStyleValues.Thin

                .Range(RowNo + 5, 1, RowNo + 5, 2).Merge()
                .Range(RowNo + 5, 1, RowNo + 5, 2).Value = "City"
                .Range(RowNo + 5, 1, RowNo + 5, 2).Style.Font.Bold = True
                .Range(RowNo + 5, 1, RowNo + 5, 2).Style.Border.OutsideBorder = xlapp.XLBorderStyleValues.Thin

                .Range(RowNo + 6, 1, RowNo + 6, 2).Merge()
                .Range(RowNo + 6, 1, RowNo + 6, 2).Value = "PinCode"
                .Range(RowNo + 6, 1, RowNo + 6, 2).Style.Font.Bold = True
                .Range(RowNo + 6, 1, RowNo + 6, 2).Style.Border.OutsideBorder = xlapp.XLBorderStyleValues.Thin

                .Range(RowNo + 7, 1, RowNo + 7, 2).Merge()
                .Range(RowNo + 7, 1, RowNo + 7, 2).Value = "State"
                .Range(RowNo + 7, 1, RowNo + 7, 2).Style.Font.Bold = True
                .Range(RowNo + 7, 1, RowNo + 7, 2).Style.Border.OutsideBorder = xlapp.XLBorderStyleValues.Thin

                .Range(RowNo + 8, 1, RowNo + 8, 2).Merge()
                .Range(RowNo + 8, 1, RowNo + 8, 2).Value = "Country"
                .Range(RowNo + 8, 1, RowNo + 8, 2).Style.Font.Bold = True
                .Range(RowNo + 8, 1, RowNo + 8, 2).Style.Border.OutsideBorder = xlapp.XLBorderStyleValues.Thin

                'Transaction Party values
                'Code, Name, Address, City, State, Pincode, Country
                RowNo = RowNo - 1
                For Each strParty As String In argParty.Split("ø")
                    RowNo = RowNo + 1
                    .Range(RowNo, 3, RowNo, 5).Merge()
                    .Range(RowNo, 3, RowNo, 5).Value = GetVal(strParty, "'")
                    .Range(RowNo, 3, RowNo, 5).Style.Border.OutsideBorder = xlapp.XLBorderStyleValues.Thin
                Next

                Dim intHdrRow As Int32 = 2
                Dim intHdrCol As Int32 = intDetCol - 4

                'Transaction Header
                For i = 0 To argHdr.Rows.Count - 1
                    intHdrRow = intHdrRow + 1
                    .Range(intHdrRow, intHdrCol, intHdrRow, intHdrCol + 1).Merge()
                    .Range(intHdrRow, intHdrCol, intHdrRow, intHdrCol + 1).Value = argHdr.GetData(i, vOthFldCaption)
                    .Range(intHdrRow, intHdrCol, intHdrRow, intHdrCol + 1).Style.Border.OutsideBorder = xlapp.XLBorderStyleValues.Thin
                    .Range(intHdrRow, intHdrCol, intHdrRow, intHdrCol + 1).Style.Font.Bold = True

                    .Range(intHdrRow, intHdrCol + 2, intHdrRow, intHdrCol + 4).Merge()
                    .Range(intHdrRow, intHdrCol + 2, intHdrRow, intHdrCol + 4).Value = argHdr.GetData(i, vOthFldDataValue)
                    .Range(intHdrRow, intHdrCol + 2, intHdrRow, intHdrCol + 4).Style.Border.OutsideBorder = xlapp.XLBorderStyleValues.Thin
                Next

                If intHdrRow > RowNo Then
                    RowNo = intHdrRow
                End If
                RowNo = RowNo + 1

                'Transaction Details
                For i = 0 To argDet.Rows.Count - 1
                    RowNo = RowNo + 1
                    intHdrCol = 0
                    For j = 0 To argDet.Cols.Count - 1
                        If argDet.Cols(j).Visible = True Then
                            intHdrCol = intHdrCol + 1
                            .Cell(RowNo, intHdrCol).Value = argDet.GetData(i, j)
                            If i = 0 Then
                                .Cell(RowNo, intHdrCol).Style.Font.Bold = True
                            End If
                            .Cell(RowNo, intHdrCol).Style.Border.OutsideBorder = xlapp.XLBorderStyleValues.Thin
                        End If
                    Next
                Next

                RowNo = RowNo + 1
                intHdrRow = RowNo
                intHdrCol = 1

                'Transaction Footer Details
                For i = 0 To argFtr.Rows.Count - 1
                    intHdrRow = intHdrRow + 1
                    .Range(intHdrRow, intHdrCol, intHdrRow, intHdrCol + 1).Merge()
                    .Range(intHdrRow, intHdrCol, intHdrRow, intHdrCol + 1).Value = argFtr.GetData(i, vOthFldCaption)
                    .Range(intHdrRow, intHdrCol, intHdrRow, intHdrCol + 1).Style.Border.OutsideBorder = xlapp.XLBorderStyleValues.Thin
                    .Range(intHdrRow, intHdrCol, intHdrRow, intHdrCol + 1).Style.Font.Bold = True

                    .Range(intHdrRow, intHdrCol + 2, intHdrRow, intHdrCol + 4).Merge()
                    .Range(intHdrRow, intHdrCol + 2, intHdrRow, intHdrCol + 4).Value = argFtr.GetData(i, vOthFldDataValue)
                    .Range(intHdrRow, intHdrCol + 2, intHdrRow, intHdrCol + 4).Style.Border.OutsideBorder = xlapp.XLBorderStyleValues.Thin
                Next

                intHdrRow = RowNo
                intHdrCol = intDetCol - 4

                'Transaction Total Section
                For i = 0 To argTot.Rows.Count - 1
                    intHdrRow = intHdrRow + 1
                    .Range(intHdrRow, intHdrCol, intHdrRow, intHdrCol + 1).Merge()
                    .Range(intHdrRow, intHdrCol, intHdrRow, intHdrCol + 1).Value = argTot.GetData(i, vOthFldCaption)
                    .Range(intHdrRow, intHdrCol, intHdrRow, intHdrCol + 1).Style.Border.OutsideBorder = xlapp.XLBorderStyleValues.Thin
                    .Range(intHdrRow, intHdrCol, intHdrRow, intHdrCol + 1).Style.Font.Bold = True

                    .Range(intHdrRow, intHdrCol + 2, intHdrRow, intHdrCol + 4).Merge()
                    .Range(intHdrRow, intHdrCol + 2, intHdrRow, intHdrCol + 4).Value = argTot.GetData(i, vOthFldDataValue)
                    .Range(intHdrRow, intHdrCol + 2, intHdrRow, intHdrCol + 4).Style.Border.OutsideBorder = xlapp.XLBorderStyleValues.Thin
                Next

                'Outer Border thick
                .Range(1, 1, intHdrRow, intDetCol).Style.Border.OutsideBorder = xlapp.XLBorderStyleValues.Thick

                'Disable Gridlines
                '.ShowGridLines = False
                xlapp.ActiveWindow.DisplayGridlines = False
                'Autofit columns
                For i = 1 To intDetCol
                    .Columns(i).AdjustToContents()
                Next
            End With

            xlWB.SaveAs(Application.StartupPath & "\" & strFileName)
            xlWB.Dispose()
            xlWS.Dispose()
            System.Diagnostics.Process.Start(Application.StartupPath & "\" & strFileName)
        Catch ex As Exception
            xlWB.Dispose()
            xlWS.Dispose()
            MsgBox("Doc Export Error..." & vbNewLine & ex.Message, MsgBoxStyle.Information, "Failed")
        End Try
    End Sub

    Public Sub DocXlExport_ClosedXML(argFrm As Form, strSHForm As String, argHdr As C1FlexGrid, argDet As C1FlexGrid, argFtr As C1FlexGrid, argTot As C1FlexGrid,
                        argDocId As Int32, argDocNo As String, argDocDate As Date, argPFX As String, argParty As String)
        Dim i As Int32, j As Int32
        Dim RowNo As Int32
        Dim intDetCol As Int32 = 0
        Dim strFileName As String = argFrm.Text & " " & argDocNo & ".xlsx"
        If System.IO.File.Exists(strFileName) Then
            System.IO.File.Delete(strFileName)
        End If

        Dim xlWB As New XLWorkbook()
        xlWB.Worksheets.Add("Sheet1")
        Dim xlWS As IXLWorksheet = xlWB.Worksheet("Sheet1")
        Try
            RowNo = 3

            'Get Total visible columns in Detail section
            For i = 0 To argDet.Cols.Count - 1
                If argDet.Cols(i).Visible = True Then
                    intDetCol = intDetCol + 1
                End If
            Next
            intDetCol = intDetCol
            If intDetCol < 11 Then intDetCol = 11

            With xlWS
                'Transaction Header
                .Range(1, 1, 1, intDetCol).Merge()
                .Range(1, 1, 1, intDetCol).Value = argFrm.Text
                .Range(1, 1, 1, intDetCol).Style.Font.FontSize = 14
                .Range(1, 1, 1, intDetCol).Style.Font.Bold = True
                .Range(1, 1, 1, intDetCol).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center
                .Range(1, 1, 1, intDetCol).Style.Border.OutsideBorder = XLBorderStyleValues.Thick

                'Transaction Document Number
                .Range(RowNo, 1, RowNo, 2).Merge()
                .Range(RowNo, 1, RowNo, 2).Value = strSHForm & " No."
                .Range(RowNo, 1, RowNo, 2).Style.Border.OutsideBorder = XLBorderStyleValues.Thin
                .Range(RowNo, 1, RowNo, 2).Style.Font.Bold = True
                .Range(RowNo, 3, RowNo, 5).Merge()
                .Range(RowNo, 3, RowNo, 5).Value = argDocNo
                .Range(RowNo, 3, RowNo, 5).Style.Border.OutsideBorder = XLBorderStyleValues.Thin

                'Transaction Document Date
                RowNo = RowNo + 1
                .Range(RowNo, 1, RowNo, 2).Merge()
                .Range(RowNo, 1, RowNo, 2).Value = strSHForm & " Date."
                .Range(RowNo, 1, RowNo, 2).Style.Border.OutsideBorder = XLBorderStyleValues.Thin
                .Range(RowNo, 1, RowNo, 2).Style.Font.Bold = True
                .Range(RowNo, 3, RowNo, 5).Merge()
                .Range(RowNo, 3, RowNo, 5).Value = argDocDate
                .Range(RowNo, 3, RowNo, 5).Style.Border.OutsideBorder = XLBorderStyleValues.Thin


                'Transaction Party Caption
                'Code, Name, Address, City, State, Pincode, Country
                RowNo = RowNo + 2
                .Range(RowNo, 1, RowNo, 2).Merge()
                .Range(RowNo, 1, RowNo, 2).Value = "Code"
                .Range(RowNo, 1, RowNo, 2).Style.Font.Bold = True
                .Range(RowNo, 1, RowNo, 2).Style.Border.OutsideBorder = XLBorderStyleValues.Thin

                .Range(RowNo + 1, 1, RowNo + 1, 2).Merge()
                .Range(RowNo + 1, 1, RowNo + 1, 2).Value = "Name"
                .Range(RowNo + 1, 1, RowNo + 1, 2).Style.Font.Bold = True
                .Range(RowNo + 1, 1, RowNo + 1, 2).Style.Border.OutsideBorder = XLBorderStyleValues.Thin

                .Range(RowNo + 2, 1, RowNo + 2, 2).Merge()
                .Range(RowNo + 2, 1, RowNo + 2, 2).Value = "Address"
                .Range(RowNo + 2, 1, RowNo + 2, 2).Style.Font.Bold = True
                .Range(RowNo + 2, 1, RowNo + 2, 2).Style.Border.OutsideBorder = XLBorderStyleValues.Thin

                .Range(RowNo + 3, 1, RowNo + 3, 2).Merge()
                .Range(RowNo + 3, 1, RowNo + 3, 2).Style.Border.OutsideBorder = XLBorderStyleValues.Thin

                .Range(RowNo + 4, 1, RowNo + 4, 2).Merge()
                .Range(RowNo + 4, 1, RowNo + 4, 2).Style.Border.OutsideBorder = XLBorderStyleValues.Thin

                .Range(RowNo + 5, 1, RowNo + 5, 2).Merge()
                .Range(RowNo + 5, 1, RowNo + 5, 2).Value = "City"
                .Range(RowNo + 5, 1, RowNo + 5, 2).Style.Font.Bold = True
                .Range(RowNo + 5, 1, RowNo + 5, 2).Style.Border.OutsideBorder = XLBorderStyleValues.Thin

                .Range(RowNo + 6, 1, RowNo + 6, 2).Merge()
                .Range(RowNo + 6, 1, RowNo + 6, 2).Value = "PinCode"
                .Range(RowNo + 6, 1, RowNo + 6, 2).Style.Font.Bold = True
                .Range(RowNo + 6, 1, RowNo + 6, 2).Style.Border.OutsideBorder = XLBorderStyleValues.Thin

                .Range(RowNo + 7, 1, RowNo + 7, 2).Merge()
                .Range(RowNo + 7, 1, RowNo + 7, 2).Value = "State"
                .Range(RowNo + 7, 1, RowNo + 7, 2).Style.Font.Bold = True
                .Range(RowNo + 7, 1, RowNo + 7, 2).Style.Border.OutsideBorder = XLBorderStyleValues.Thin

                .Range(RowNo + 8, 1, RowNo + 8, 2).Merge()
                .Range(RowNo + 8, 1, RowNo + 8, 2).Value = "Country"
                .Range(RowNo + 8, 1, RowNo + 8, 2).Style.Font.Bold = True
                .Range(RowNo + 8, 1, RowNo + 8, 2).Style.Border.OutsideBorder = XLBorderStyleValues.Thin

                'Transaction Party values
                'Code, Name, Address, City, State, Pincode, Country
                RowNo = RowNo - 1
                For Each strParty As String In argParty.Split("ø")
                    RowNo = RowNo + 1
                    .Range(RowNo, 3, RowNo, 5).Merge()
                    .Range(RowNo, 3, RowNo, 5).Value = GetVal(strParty, "'")
                    .Range(RowNo, 3, RowNo, 5).Style.Border.OutsideBorder = XLBorderStyleValues.Thin
                Next

                Dim intHdrRow As Int32 = 2
                Dim intHdrCol As Int32 = intDetCol - 4

                'Transaction Header
                For i = 0 To argHdr.Rows.Count - 1
                    intHdrRow = intHdrRow + 1
                    .Range(intHdrRow, intHdrCol, intHdrRow, intHdrCol + 1).Merge()
                    .Range(intHdrRow, intHdrCol, intHdrRow, intHdrCol + 1).Value = argHdr.GetData(i, vOthFldCaption)
                    .Range(intHdrRow, intHdrCol, intHdrRow, intHdrCol + 1).Style.Border.OutsideBorder = XLBorderStyleValues.Thin
                    .Range(intHdrRow, intHdrCol, intHdrRow, intHdrCol + 1).Style.Font.Bold = True

                    .Range(intHdrRow, intHdrCol + 2, intHdrRow, intHdrCol + 4).Merge()
                    .Range(intHdrRow, intHdrCol + 2, intHdrRow, intHdrCol + 4).Value = argHdr.GetData(i, vOthFldDataValue)
                    .Range(intHdrRow, intHdrCol + 2, intHdrRow, intHdrCol + 4).Style.Border.OutsideBorder = XLBorderStyleValues.Thin
                Next

                If intHdrRow > RowNo Then
                    RowNo = intHdrRow
                End If
                RowNo = RowNo + 1

                'Transaction Details
                For i = 0 To argDet.Rows.Count - 1
                    RowNo = RowNo + 1
                    intHdrCol = 0
                    For j = 0 To argDet.Cols.Count - 1
                        If argDet.Cols(j).Visible = True Then
                            intHdrCol = intHdrCol + 1
                            .Cell(RowNo, intHdrCol).Value = argDet.GetData(i, j)
                            If i = 0 Then
                                .Cell(RowNo, intHdrCol).Style.Font.Bold = True
                            End If
                            .Cell(RowNo, intHdrCol).Style.Border.OutsideBorder = XLBorderStyleValues.Thin
                        End If
                    Next
                Next

                RowNo = RowNo + 1
                intHdrRow = RowNo
                intHdrCol = 1

                'Transaction Footer Details
                For i = 0 To argFtr.Rows.Count - 1
                    intHdrRow = intHdrRow + 1
                    .Range(intHdrRow, intHdrCol, intHdrRow, intHdrCol + 1).Merge()
                    .Range(intHdrRow, intHdrCol, intHdrRow, intHdrCol + 1).Value = argFtr.GetData(i, vOthFldCaption)
                    .Range(intHdrRow, intHdrCol, intHdrRow, intHdrCol + 1).Style.Border.OutsideBorder = XLBorderStyleValues.Thin
                    .Range(intHdrRow, intHdrCol, intHdrRow, intHdrCol + 1).Style.Font.Bold = True

                    .Range(intHdrRow, intHdrCol + 2, intHdrRow, intHdrCol + 4).Merge()
                    .Range(intHdrRow, intHdrCol + 2, intHdrRow, intHdrCol + 4).Value = argFtr.GetData(i, vOthFldDataValue)
                    .Range(intHdrRow, intHdrCol + 2, intHdrRow, intHdrCol + 4).Style.Border.OutsideBorder = XLBorderStyleValues.Thin
                Next

                intHdrRow = RowNo
                intHdrCol = intDetCol - 4

                'Transaction Total Section
                For i = 0 To argTot.Rows.Count - 1
                    intHdrRow = intHdrRow + 1
                    .Range(intHdrRow, intHdrCol, intHdrRow, intHdrCol + 1).Merge()
                    .Range(intHdrRow, intHdrCol, intHdrRow, intHdrCol + 1).Value = argTot.GetData(i, vOthFldCaption)
                    .Range(intHdrRow, intHdrCol, intHdrRow, intHdrCol + 1).Style.Border.OutsideBorder = XLBorderStyleValues.Thin
                    .Range(intHdrRow, intHdrCol, intHdrRow, intHdrCol + 1).Style.Font.Bold = True

                    .Range(intHdrRow, intHdrCol + 2, intHdrRow, intHdrCol + 4).Merge()
                    .Range(intHdrRow, intHdrCol + 2, intHdrRow, intHdrCol + 4).Value = argTot.GetData(i, vOthFldDataValue)
                    .Range(intHdrRow, intHdrCol + 2, intHdrRow, intHdrCol + 4).Style.Border.OutsideBorder = XLBorderStyleValues.Thin
                Next

                'Outer Border thick
                .Range(1, 1, intHdrRow, intDetCol).Style.Border.OutsideBorder = XLBorderStyleValues.Thick

                'Disable Gridlines
                .ShowGridLines = False

                'Autofit columns
                For i = 1 To intDetCol
                    .Columns(i).AdjustToContents()
                Next
            End With

            xlWB.SaveAs(Application.StartupPath & "\" & strFileName)
            xlWB.Dispose()
            xlWS.Dispose()
            System.Diagnostics.Process.Start(Application.StartupPath & "\" & strFileName)
        Catch ex As Exception
            xlWB.Dispose()
            xlWS.Dispose()
            MsgBox("Doc Export Error..." & vbNewLine & ex.Message, MsgBoxStyle.Information, "Failed")
        End Try
    End Sub

    Private Sub tsbDocExport_Click(sender As System.Object, e As System.EventArgs) Handles tsbDocExport.Click
        RaiseEvent btnClicked(svtDtlBtns.svtTrnExport)
    End Sub

    Private Sub UpdateFixedRowForDocXl(ByVal xlwb As Excel.Workbook, ByVal xlws As Excel.Worksheet, ByVal argtext As String, ByVal intRowno As Int32, ByVal intEndcol As Int32, ByVal intFontsize As Int32, ByVal blnLine As Boolean, ByVal blnBackColor As Boolean)
        With xlws
            .Range("A" & intRowno & ":" & NumToAlphabet(intEndcol) & intRowno).Merge()
            .Range("A" & intRowno & ":" & NumToAlphabet(intEndcol) & intRowno).Value = argtext
            .Range("A" & intRowno & ":" & NumToAlphabet(intEndcol) & intRowno).Font.Size = intFontsize
            .Range("A" & intRowno & ":" & NumToAlphabet(intEndcol) & intRowno).Font.Bold = True
            ' .Range("A" & intRowno & ":" & NumToAlphabet(intEndcol) & intRowno).Style.Equals(HorizontalAlignment.Center)
            If blnBackColor Then
                Dim style As Excel.Style = xlwb.Styles.Add("NewStyle")
                style.Interior.Color = ColorTranslator.ToOle(Color.LightGray)
                .Range("A" & intRowno & ":" & NumToAlphabet(intEndcol) & intRowno).Style = "NewStyle"
            End If
            '.Range("A" & intRowno & ":" & NumToAlphabet(intEndcol) & intRowno).VerticalAlignment = XLAlignmentVerticalValues.Center
            If blnLine Then
                .Range("A" & intRowno & ":" & NumToAlphabet(intEndcol) & intRowno).BorderAround2(Excel.XlBorderWeight.xlHairline)
            End If
        End With
    End Sub

    Private Sub UpdateFixedRowForDocXlXML(ByVal xlws As IXLWorksheet, ByVal argtext As String, ByVal intRowno As Int32, ByVal intEndcol As Int32, ByVal intFontsize As Int32, ByVal blnLine As Boolean, ByVal blnBackColor As Boolean)
        '.Range(1, 1, 1, intDetCol).Merge()
        '.Range(1, 1, 1, intDetCol).Value = argFrm.Text
        '.Range(1, 1, 1, intDetCol).Style.Font.FontSize = 14
        '.Range(1, 1, 1, intDetCol).Style.Font.Bold = True
        '.Range(1, 1, 1, intDetCol).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center
        '.Range(1, 1, 1, intDetCol).Style.Border.OutsideBorder = XLBorderStyleValues.Thick

        With xlws

            .Range("A" & intRowno & ":" & NumToAlphabet(intEndcol) & intRowno).Merge()
            .Range("A" & intRowno & ":" & NumToAlphabet(intEndcol) & intRowno).Value = argtext
            .Range("A" & intRowno & ":" & NumToAlphabet(intEndcol) & intRowno).Style.Font.FontSize = intFontsize
            .Range("A" & intRowno & ":" & NumToAlphabet(intEndcol) & intRowno).Style.Font.Bold = True
            .Range("A" & intRowno & ":" & NumToAlphabet(intEndcol) & intRowno).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center
            If blnBackColor Then
                .Range("A" & intRowno & ":" & NumToAlphabet(intEndcol) & intRowno).Style.Fill.BackgroundColor = XLColor.LightGray
            End If

            If blnLine Then
                .Range("A" & intRowno & ":" & NumToAlphabet(intEndcol) & intRowno).Style.Border.OutsideBorder = XLBorderStyleValues.Thick
            End If
        End With
    End Sub

    Public Sub DocXlExp(argFrm As Form, strSHForm As String, argHdr As TabControl, argDet As C1FlexGrid, argFtr As TabControl, argTot As C1FlexGrid,
                       argDocId As Int32, argDocNo As String, argDocDate As Date, argPFX As String, argParty As String, Optional ByVal argCustSupp As String = "")
        Dim i As Int32, j As Int32
        Dim RowNo As Int32
        Dim intDetCol As Int32 = 0
        Dim strFileName As String = argFrm.Text & " " & argDocNo & ".xlsx"
        If System.IO.File.Exists(strFileName) Then
            System.IO.File.Delete(strFileName)
        End If

        Dim xlapp As New Excel.Application
        Dim xlWB As Excel.Workbook = xlapp.Workbooks.Add()
        Dim xlWS As Excel.Worksheet = xlWB.Worksheets.Add()
        Try
            RowNo = 11

            'Get Total visible columns in Detail section
            For i = 0 To argDet.Cols.Count - 1
                If argDet.Cols(i).Visible = True Then
                    intDetCol = intDetCol + 1
                End If
            Next
            intDetCol = intDetCol
            If intDetCol < 11 Then intDetCol = 11

            With xlWS
                'Company Details
                Call UpdateFixedRowForDocXl(xlWB, xlWS, gSettings.gCompName, 1, intDetCol, 14, False, False)
                Call UpdateFixedRowForDocXl(xlWB, xlWS, gSettings.gCompAdd1, 2, intDetCol, 11, False, False)
                Call UpdateFixedRowForDocXl(xlWB, xlWS, gSettings.gCompAdd2, 3, intDetCol, 11, False, False)
                Call UpdateFixedRowForDocXl(xlWB, xlWS, gSettings.gCompAdd3 & " " & gSettings.gCompAdd4 & " " & gSettings.gCompAdd5, 4, intDetCol, 11, False, False)
                Call UpdateFixedRowForDocXl(xlWB, xlWS, gSettings.gCompPHNO, 5, intDetCol, 11, False, False)
                Call UpdateFixedRowForDocXl(xlWB, xlWS, clsSVT.getDataStringValue("select EMAIL from mstcomp", "", False), 6, intDetCol, 11, False, False)
                Call UpdateFixedRowForDocXl(xlWB, xlWS, clsSVT.getDataStringValue("select GSTNO from mstcomp", "", False), 7, intDetCol, 11, False, False)

                'Transaction Header
                Call UpdateFixedRowForDocXl(xlWB, xlWS, argFrm.Text, 9, intDetCol, 14, False, True)

                ''Transaction Header
                '.Range("A1:" & NumToAlphabet(intDetCol) & "1").Merge()
                '.Range("A1:" & NumToAlphabet(intDetCol) & "1").Value = argFrm.Text
                '.Range("A1:" & NumToAlphabet(intDetCol) & "1").Font.Size = 14
                '.Range("A1:" & NumToAlphabet(intDetCol) & "1").Font.Bold = True
                '.Range("A1:" & NumToAlphabet(intDetCol) & "1").HorizontalAlignment = HorizontalAlignment.Center
                '.Range("A1:" & NumToAlphabet(intDetCol) & "1").BorderAround2(Excel.XlBorderWeight.xlHairline)

                'Transaction Document Number
                .Range("A" & RowNo & ":B" & RowNo).Merge()
                .Range("A" & RowNo & ":B" & RowNo).Value = strSHForm & " No."
                .Range("A" & RowNo & ":B" & RowNo).BorderAround2(Excel.XlBorderWeight.xlHairline)
                .Range("A" & RowNo & ":B" & RowNo).Font.Bold = True
                .Range("C" & RowNo & ":E" & RowNo).Merge()
                .Range("C" & RowNo & ":E" & RowNo).Value = argDocNo
                .Range("C" & RowNo & ":E" & RowNo).BorderAround2(Excel.XlBorderWeight.xlHairline)

                'Transaction Document Date
                RowNo = RowNo + 1

                .Range("A" & RowNo & ":B" & RowNo).Merge()
                .Range("A" & RowNo & ":B" & RowNo).Value = strSHForm & " Date."
                .Range("A" & RowNo & ":B" & RowNo).BorderAround2(Excel.XlBorderWeight.xlHairline)
                .Range("A" & RowNo & ":B" & RowNo).Font.Bold = True
                .Range("C" & RowNo & ":E" & RowNo).Merge()
                .Range("C" & RowNo & ":E" & RowNo).Value = argDocDate
                .Range("C" & RowNo & ":E" & RowNo).BorderAround2(Excel.XlBorderWeight.xlHairline)

                'Transaction Party Caption
                'Code, Name, Address, City, State, Pincode, Country
                RowNo = RowNo + 2
                .Range("A" & RowNo & ":B" & RowNo).Merge()
                .Range("A" & RowNo & ":B" & RowNo).Value = argCustSupp & " Code"
                .Range("A" & RowNo & ":B" & RowNo).Font.Bold = True
                .Range("A" & RowNo & ":B" & RowNo).BorderAround2(Excel.XlBorderWeight.xlHairline)

                .Range("A" & RowNo + 1 & ":B" & RowNo + 1).Merge()
                .Range("A" & RowNo + 1 & ":B" & RowNo + 1).Value = argCustSupp & " Name"
                .Range("A" & RowNo + 1 & ":B" & RowNo + 1).Font.Bold = True
                .Range("A" & RowNo + 1 & ":B" & RowNo + 1).BorderAround2(Excel.XlBorderWeight.xlHairline)

                .Range("A" & RowNo + 2 & ":B" & RowNo + 2).Merge()
                .Range("A" & RowNo + 2 & ":B" & RowNo + 2).Value = argCustSupp & " Address"
                .Range("A" & RowNo + 2 & ":B" & RowNo + 2).Font.Bold = True
                .Range("A" & RowNo + 2 & ":B" & RowNo + 2).BorderAround2(Excel.XlBorderWeight.xlHairline)

                .Range("A" & RowNo + 3 & ":B" & RowNo + 3).Merge()
                .Range("A" & RowNo + 3 & ":B" & RowNo + 3).BorderAround2(Excel.XlBorderWeight.xlHairline)

                .Range("A" & RowNo + 4 & ":B" & RowNo + 4).Merge()
                .Range("A" & RowNo + 4 & ":B" & RowNo + 4).BorderAround2(Excel.XlBorderWeight.xlHairline)

                '.Range("A" & RowNo + 5 & ":B" & RowNo + 5).Merge()
                '.Range("A" & RowNo + 5 & ":B" & RowNo + 5).Value = ""
                '.Range("A" & RowNo + 5 & ":B" & RowNo + 5).Font.Bold = True
                '.Range("A" & RowNo + 5 & ":B" & RowNo + 5).BorderAround2(Excel.XlBorderWeight.xlHairline)


                '.Range("A" & RowNo + 6 & ":B" & RowNo + 6).Merge()
                '.Range("A" & RowNo + 6 & ":B" & RowNo + 6).Value = ""
                '.Range("A" & RowNo + 6 & ":B" & RowNo + 6).Font.Bold = True
                '.Range("A" & RowNo + 6 & ":B" & RowNo + 6).BorderAround2(Excel.XlBorderWeight.xlHairline)

                '.Range("A" & RowNo + 7 & ":B" & RowNo + 7).Merge()
                '.Range("A" & RowNo + 7 & ":B" & RowNo + 7).Value = ""
                '.Range("A" & RowNo + 7 & ":B" & RowNo + 7).Font.Bold = True
                '.Range("A" & RowNo + 7 & ":B" & RowNo + 7).BorderAround2(Excel.XlBorderWeight.xlHairline)

                '.Range("A" & RowNo + 8 & ":B" & RowNo + 8).Merge()
                '.Range("A" & RowNo + 8 & ":B" & RowNo + 8).Value = ""
                '.Range("A" & RowNo + 8 & ":B" & RowNo + 8).Font.Bold = True
                '.Range("A" & RowNo + 8 & ":B" & RowNo + 8).BorderAround2(Excel.XlBorderWeight.xlHairline)


                'Transaction Party values
                'Code, Name, Address, City, State, Pincode, Country
                RowNo = RowNo - 1
                For Each strParty As String In argParty.Split(ChrW(13))
                    RowNo = RowNo + 1

                    .Range("C" & RowNo & ":E" & RowNo).Merge()
                    .Range("C" & RowNo & ":E" & RowNo).Value = GetVal(strParty, "'")
                    .Range("C" & RowNo & ":E" & RowNo).BorderAround2(Excel.XlBorderWeight.xlHairline)
                Next

                RowNo = 18

                Dim intHdrRow As Int32 = 10
                Dim intHdrCol As Int32 = intDetCol - 4

                'Transaction Header

                If argHdr.Visible = True Then
                    For Each tb As TabPage In argHdr.TabPages
                        Dim FlexCtrl = GetAllCtrls(tb, GetType(C1FlexGrid))
                        Dim argvsf As C1FlexGrid = FlexCtrl.First
                        For i = 0 To argvsf.Rows.Count - 1
                            intHdrRow = intHdrRow + 1
                            .Range(NumToAlphabet(intHdrCol) & intHdrRow & ":" & NumToAlphabet(intHdrCol + 1) & intHdrRow).Merge()
                            .Range(NumToAlphabet(intHdrCol) & intHdrRow & ":" & NumToAlphabet(intHdrCol + 1) & intHdrRow).Value = argvsf.GetData(i, vOthFldCaption)
                            .Range(NumToAlphabet(intHdrCol) & intHdrRow & ":" & NumToAlphabet(intHdrCol + 1) & intHdrRow).BorderAround2(Excel.XlBorderWeight.xlHairline)
                            .Range(NumToAlphabet(intHdrCol) & intHdrRow & ":" & NumToAlphabet(intHdrCol + 1) & intHdrRow).Font.Bold = True

                            .Range(NumToAlphabet(intHdrCol + 2) & intHdrRow & ":" & NumToAlphabet(intHdrCol + 4) & intHdrRow).Merge()
                            .Range(NumToAlphabet(intHdrCol + 2) & intHdrRow & ":" & NumToAlphabet(intHdrCol + 4) & intHdrRow).Value = argvsf.GetData(i, vOthFldDataValue)
                            .Range(NumToAlphabet(intHdrCol + 2) & intHdrRow & ":" & NumToAlphabet(intHdrCol + 4) & intHdrRow).BorderAround2(Excel.XlBorderWeight.xlHairline)

                        Next
                    Next
                End If

                If intHdrRow > RowNo Then
                    RowNo = intHdrRow
                End If
                RowNo = RowNo + 1
                'Transaction Details

                Dim style As Excel.Style = xlWB.Styles.Add("NewStyle1")
                style.Interior.Color = ColorTranslator.ToOle(Color.LightGray)
                For i = 0 To argDet.Rows.Count - 1
                    RowNo = RowNo + 1
                    intHdrCol = 0
                    For j = 0 To argDet.Cols.Count - 1
                        If argDet.Cols(j).Visible = True Then
                            intHdrCol = intHdrCol + 1
                            .Range(NumToAlphabet(intHdrCol) & RowNo).Value = argDet.GetData(i, j)
                            If i = 0 Then
                                .Range(NumToAlphabet(intHdrCol) & RowNo).Font.Bold = True
                                .Range(NumToAlphabet(intHdrCol) & RowNo).Style = "NewStyle1"
                            End If
                            .Range(NumToAlphabet(intHdrCol) & RowNo).BorderAround2(Excel.XlBorderWeight.xlHairline)
                        End If
                    Next
                Next
                RowNo = RowNo + 1
                intHdrRow = RowNo
                intHdrCol = 1

                'Transaction Footer Details
                If argFtr.Visible = True Then
                    For Each tb As TabPage In argFtr.TabPages
                        Dim FlexCtrl = GetAllCtrls(tb, GetType(C1FlexGrid))
                        Dim argvsf As C1FlexGrid = FlexCtrl.First
                        For i = 0 To argvsf.Rows.Count - 1
                            intHdrRow = intHdrRow + 1
                            .Range(NumToAlphabet(intHdrCol) & intHdrRow & ":" & NumToAlphabet(intHdrCol + 1) & intHdrRow).Merge()
                            .Range(NumToAlphabet(intHdrCol) & intHdrRow & ":" & NumToAlphabet(intHdrCol + 1) & intHdrRow).Value = argvsf.GetData(i, vOthFldCaption)
                            .Range(NumToAlphabet(intHdrCol) & intHdrRow & ":" & NumToAlphabet(intHdrCol + 1) & intHdrRow).BorderAround2(Excel.XlBorderWeight.xlHairline)
                            .Range(NumToAlphabet(intHdrCol) & intHdrRow & ":" & NumToAlphabet(intHdrCol + 1) & intHdrRow).Font.Bold = True

                            .Range(NumToAlphabet(intHdrCol + 2) & intHdrRow & ":" & NumToAlphabet(intHdrCol + 4) & intHdrRow).Merge()
                            .Range(NumToAlphabet(intHdrCol + 2) & intHdrRow & ":" & NumToAlphabet(intHdrCol + 4) & intHdrRow).Value = argvsf.GetData(i, vOthFldDataValue)
                            .Range(NumToAlphabet(intHdrCol + 2) & intHdrRow & ":" & NumToAlphabet(intHdrCol + 4) & intHdrRow).BorderAround2(Excel.XlBorderWeight.xlHairline)

                        Next
                    Next
                End If

                Dim intLastRow As Int32 = intHdrRow
                intHdrRow = RowNo
                intHdrCol = intDetCol - 4

                'Transaction Total Section
                For i = 0 To argTot.Rows.Count - 1
                    intHdrRow = intHdrRow + 1
                    .Range(NumToAlphabet(intHdrCol) & intHdrRow & ":" & NumToAlphabet(intHdrCol + 1) & intHdrRow).Merge()
                    .Range(NumToAlphabet(intHdrCol) & intHdrRow & ":" & NumToAlphabet(intHdrCol + 1) & intHdrRow).Value = argTot.GetData(i, vOthFldCaption)
                    .Range(NumToAlphabet(intHdrCol) & intHdrRow & ":" & NumToAlphabet(intHdrCol + 1) & intHdrRow).BorderAround2(Excel.XlBorderWeight.xlHairline)
                    .Range(NumToAlphabet(intHdrCol) & intHdrRow & ":" & NumToAlphabet(intHdrCol + 1) & intHdrRow).Font.Bold = True

                    .Range(NumToAlphabet(intHdrCol + 2) & intHdrRow & ":" & NumToAlphabet(intHdrCol + 4) & intHdrRow).Merge()
                    .Range(NumToAlphabet(intHdrCol + 2) & intHdrRow & ":" & NumToAlphabet(intHdrCol + 4) & intHdrRow).Value = argTot.GetData(i, vOthFldDataValue)
                    .Range(NumToAlphabet(intHdrCol + 2) & intHdrRow & ":" & NumToAlphabet(intHdrCol + 4) & intHdrRow).BorderAround2(Excel.XlBorderWeight.xlHairline)
                    ' .Range(NumToAlphabet(intHdrCol + 2) & intHdrRow & ":" & NumToAlphabet(intHdrCol + 4) & intHdrRow).ColumnWidth = 50
                Next

                If intLastRow > intHdrRow Then
                    intHdrRow = intLastRow
                End If

                'Outer Border thick
                '.Range(1, 1, intHdrRow, intDetCol).Style.Border.OutsideBorder = xlapp.XLBorderStyleValues.Thick
                '.Range("A1:A" & intHdrRow & intDetCol).BorderAround2(Excel.XlBorderWeight.xlHairline)

                'Disable Gridlines
                '.ShowGridLines = False              
                xlapp.ActiveWindow.DisplayGridlines = False

                'Autofit columns
                For i = 1 To intDetCol
                    '.Columns(i).AdjustToContents()
                    .Columns(i).Autofit()
                Next

                Call UpdateFixedRowForDocXl(xlWB, xlWS, "For " & gSettings.gCompName, intHdrRow + 2, 3, 11, False, False)
                Call UpdateFixedRowForDocXl(xlWB, xlWS, "Authorised Signatory", intHdrRow + 6, 3, 11, False, False)
            End With

            xlWB.SaveAs(Application.StartupPath & "\" & strFileName)
            xlWB.Close()
            xlapp.Quit()
            'xlWS.Dispose()
            System.Diagnostics.Process.Start(Application.StartupPath & "\" & strFileName)
            Dim p As New System.Diagnostics.Process
            Dim s As New System.Diagnostics.ProcessStartInfo(strFileName)
            s.UseShellExecute = True
            s.WindowStyle = ProcessWindowStyle.Normal
            p.StartInfo = s
            p.Start()
        Catch ex As Exception
            xlWB.Dispose()
            xlapp.Quit()
            MsgBox("Doc Export Error..." & vbNewLine & ex.Message, MsgBoxStyle.Information, "Failed")
        End Try
    End Sub

    Public Sub DocXlExp_ClosedXML(argFrm As Form, strSHForm As String, argHdr As TabControl, argDet As C1FlexGrid, argFtr As TabControl, argTot As C1FlexGrid,
                       argDocId As Int32, argDocNo As String, argDocDate As Date, argPFX As String, argParty As String, Optional ByVal argCustSupp As String = "")
        Dim i As Int32, j As Int32
        Dim RowNo As Int32
        Dim intDetCol As Int32 = 0
        Dim strFileName As String = argFrm.Text & " " & argDocNo & ".xlsx"
        If System.IO.File.Exists(strFileName) Then
            System.IO.File.Delete(strFileName)
        End If

        Dim xlWB As New XLWorkbook()
        xlWB.Worksheets.Add("Sheet1")
        Dim xlWS As IXLWorksheet = xlWB.Worksheet("Sheet1")
        Try
            RowNo = 11

            'Get Total visible columns in Detail section
            For i = 0 To argDet.Cols.Count - 1
                If argDet.Cols(i).Visible = True Then
                    intDetCol = intDetCol + 1
                End If
            Next
            intDetCol = intDetCol
            If intDetCol < 11 Then intDetCol = 11

            With xlWS
                'Company Details
                Call UpdateFixedRowForDocXlXML(xlWS, gSettings.gCompName, 1, intDetCol, 14, False, False)
                Call UpdateFixedRowForDocXlXML(xlWS, gSettings.gCompAdd1, 2, intDetCol, 11, False, False)
                Call UpdateFixedRowForDocXlXML(xlWS, gSettings.gCompAdd2, 3, intDetCol, 11, False, False)
                Call UpdateFixedRowForDocXlXML(xlWS, gSettings.gCompAdd3 & " " & gSettings.gCompAdd4 & " " & gSettings.gCompAdd5, 4, intDetCol, 11, False, False)
                Call UpdateFixedRowForDocXlXML(xlWS, gSettings.gCompPHNO, 5, intDetCol, 11, False, False)
                Call UpdateFixedRowForDocXlXML(xlWS, clsSVT.getDataStringValue("select EMAILID from mstcomp", "", False), 6, intDetCol, 11, False, False)
                Call UpdateFixedRowForDocXlXML(xlWS, clsSVT.getDataStringValue("select GSTNO from mstcomp", "", False), 7, intDetCol, 11, False, False)

                'Transaction Header
                Call UpdateFixedRowForDocXlXML(xlWS, argFrm.Text, 9, intDetCol, 14, False, True)

                'Transaction Header
                '.Range(1, 1, 1, intDetCol).Merge()
                '.Range(1, 1, 1, intDetCol).Value = argFrm.Text
                '.Range(1, 1, 1, intDetCol).Style.Font.FontSize = 14
                '.Range(1, 1, 1, intDetCol).Style.Font.Bold = True
                '.Range(1, 1, 1, intDetCol).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center
                '.Range(1, 1, 1, intDetCol).Style.Border.OutsideBorder = XLBorderStyleValues.Thick

                'Transaction Document Number
                .Range(RowNo, 1, RowNo, 2).Merge()
                .Range(RowNo, 1, RowNo, 2).Value = strSHForm & " No."
                .Range(RowNo, 1, RowNo, 2).Style.Border.OutsideBorder = XLBorderStyleValues.Thin
                .Range(RowNo, 1, RowNo, 2).Style.Font.Bold = True
                .Range(RowNo, 3, RowNo, 5).Merge()
                .Range(RowNo, 3, RowNo, 5).Value = argDocNo
                .Range(RowNo, 3, RowNo, 5).Style.Border.OutsideBorder = XLBorderStyleValues.Thin

                'Transaction Document Date
                RowNo = RowNo + 1
                .Range(RowNo, 1, RowNo, 2).Merge()
                .Range(RowNo, 1, RowNo, 2).Value = strSHForm & " Date."
                .Range(RowNo, 1, RowNo, 2).Style.Border.OutsideBorder = XLBorderStyleValues.Thin
                .Range(RowNo, 1, RowNo, 2).Style.Font.Bold = True
                .Range(RowNo, 3, RowNo, 5).Merge()
                .Range(RowNo, 3, RowNo, 5).Value = argDocDate
                .Range(RowNo, 3, RowNo, 5).Style.Border.OutsideBorder = XLBorderStyleValues.Thin


                'Transaction Party Caption
                'Code, Name, Address, City, State, Pincode, Country
                RowNo = RowNo + 2
                '.Range(RowNo, 1, RowNo, 2).Merge()
                '.Range(RowNo, 1, RowNo, 2).Value = argCustSupp & " Code"
                '.Range(RowNo, 1, RowNo, 2).Style.Font.Bold = True
                '.Range(RowNo, 1, RowNo, 2).Style.Border.OutsideBorder = XLBorderStyleValues.Thin

                .Range("A" & RowNo & ":B" & RowNo).Merge()
                .Range("A" & RowNo & ":B" & RowNo).Value = argCustSupp & " Code"
                .Range("A" & RowNo & ":B" & RowNo).Style.Font.Bold = True
                .Range("A" & RowNo & ":B" & RowNo).Style.Border.OutsideBorder = XLBorderStyleValues.Thin

                .Range(RowNo + 1, 1, RowNo + 1, 2).Merge()
                .Range(RowNo + 1, 1, RowNo + 1, 2).Value = argCustSupp & " Name"
                .Range(RowNo + 1, 1, RowNo + 1, 2).Style.Font.Bold = True
                .Range(RowNo + 1, 1, RowNo + 1, 2).Style.Border.OutsideBorder = XLBorderStyleValues.Thin

                .Range(RowNo + 2, 1, RowNo + 2, 2).Merge()
                .Range(RowNo + 2, 1, RowNo + 2, 2).Value = argCustSupp & " Address"
                .Range(RowNo + 2, 1, RowNo + 2, 2).Style.Font.Bold = True
                .Range(RowNo + 2, 1, RowNo + 2, 2).Style.Border.OutsideBorder = XLBorderStyleValues.Thin

                .Range(RowNo + 3, 1, RowNo + 3, 2).Merge()
                .Range(RowNo + 3, 1, RowNo + 3, 2).Style.Border.OutsideBorder = XLBorderStyleValues.Thin

                .Range(RowNo + 4, 1, RowNo + 4, 2).Merge()
                .Range(RowNo + 4, 1, RowNo + 4, 2).Style.Border.OutsideBorder = XLBorderStyleValues.Thin

                '.Range(RowNo + 5, 1, RowNo + 5, 2).Merge()
                '.Range(RowNo + 5, 1, RowNo + 5, 2).Value = "City"
                '.Range(RowNo + 5, 1, RowNo + 5, 2).Style.Font.Bold = True
                '.Range(RowNo + 5, 1, RowNo + 5, 2).Style.Border.OutsideBorder = XLBorderStyleValues.Thin

                '.Range(RowNo + 6, 1, RowNo + 6, 2).Merge()
                '.Range(RowNo + 6, 1, RowNo + 6, 2).Value = "PinCode"
                '.Range(RowNo + 6, 1, RowNo + 6, 2).Style.Font.Bold = True
                '.Range(RowNo + 6, 1, RowNo + 6, 2).Style.Border.OutsideBorder = XLBorderStyleValues.Thin

                '.Range(RowNo + 7, 1, RowNo + 7, 2).Merge()
                '.Range(RowNo + 7, 1, RowNo + 7, 2).Value = "State"
                '.Range(RowNo + 7, 1, RowNo + 7, 2).Style.Font.Bold = True
                '.Range(RowNo + 7, 1, RowNo + 7, 2).Style.Border.OutsideBorder = XLBorderStyleValues.Thin

                '.Range(RowNo + 8, 1, RowNo + 8, 2).Merge()
                '.Range(RowNo + 8, 1, RowNo + 8, 2).Value = "Country"
                '.Range(RowNo + 8, 1, RowNo + 8, 2).Style.Font.Bold = True
                '.Range(RowNo + 8, 1, RowNo + 8, 2).Style.Border.OutsideBorder = XLBorderStyleValues.Thin

                'Transaction Party values
                'Code, Name, Address, City, State, Pincode, Country
                RowNo = RowNo - 1
                For Each strParty As String In argParty.Split(ChrW(13))
                    RowNo = RowNo + 1
                    '.Range(RowNo, 3, RowNo, 5).Merge()
                    '.Range(RowNo, 3, RowNo, 5).Value = GetVal(strParty, "'")
                    '.Range(RowNo, 3, RowNo, 5).Style.Border.OutsideBorder = XLBorderStyleValues.Thin

                    .Range("C" & RowNo & ":E" & RowNo).Merge()
                    .Range("C" & RowNo & ":E" & RowNo).Value = GetVal(strParty, "'")
                    .Range("C" & RowNo & ":E" & RowNo).Style.Border.OutsideBorder = XLBorderStyleValues.Hair
                Next

                RowNo = 18

                Dim intHdrRow As Int32 = 10
                Dim intHdrCol As Int32 = intDetCol - 4

                'Transaction Header

                If argHdr.Visible = True Then
                    For Each tb As TabPage In argHdr.TabPages
                        Dim FlexCtrl = GetAllCtrls(tb, GetType(C1FlexGrid))
                        Dim argvsf As C1FlexGrid = FlexCtrl.First
                        For i = 0 To argvsf.Rows.Count - 1
                            intHdrRow = intHdrRow + 1
                            .Range(intHdrRow, intHdrCol, intHdrRow, intHdrCol + 1).Merge()
                            .Range(intHdrRow, intHdrCol, intHdrRow, intHdrCol + 1).Value = argvsf.GetData(i, vOthFldCaption)
                            .Range(intHdrRow, intHdrCol, intHdrRow, intHdrCol + 1).Style.Border.OutsideBorder = XLBorderStyleValues.Thin
                            .Range(intHdrRow, intHdrCol, intHdrRow, intHdrCol + 1).Style.Font.Bold = True

                            .Range(intHdrRow, intHdrCol + 2, intHdrRow, intHdrCol + 4).Merge()
                            .Range(intHdrRow, intHdrCol + 2, intHdrRow, intHdrCol + 4).Value = argvsf.GetData(i, vOthFldDataValue)
                            .Range(intHdrRow, intHdrCol + 2, intHdrRow, intHdrCol + 4).Style.Border.OutsideBorder = XLBorderStyleValues.Thin
                        Next
                    Next
                End If

                If intHdrRow > RowNo Then
                    RowNo = intHdrRow
                End If
                RowNo = RowNo + 1

                'Transaction Details
                For i = 0 To argDet.Rows.Count - 1
                    RowNo = RowNo + 1
                    intHdrCol = 0
                    For j = 0 To argDet.Cols.Count - 1
                        If argDet.Cols(j).Visible = True Then
                            intHdrCol = intHdrCol + 1
                            .Cell(RowNo, intHdrCol).Value = argDet.GetData(i, j)
                            If i = 0 Then
                                .Cell(RowNo, intHdrCol).Style.Font.Bold = True
                            End If
                            .Cell(RowNo, intHdrCol).Style.Border.OutsideBorder = XLBorderStyleValues.Thin
                        End If
                    Next
                Next

                RowNo = RowNo + 1
                intHdrRow = RowNo
                intHdrCol = 1

                'Transaction Footer Details
                If argFtr.Visible = True Then
                    For Each tb As TabPage In argFtr.TabPages
                        Dim FlexCtrl = GetAllCtrls(tb, GetType(C1FlexGrid))
                        Dim argvsf As C1FlexGrid = FlexCtrl.First
                        For i = 0 To argvsf.Rows.Count - 1
                            intHdrRow = intHdrRow + 1
                            .Range(intHdrRow, intHdrCol, intHdrRow, intHdrCol + 1).Merge()
                            .Range(intHdrRow, intHdrCol, intHdrRow, intHdrCol + 1).Value = argvsf.GetData(i, vOthFldCaption)
                            .Range(intHdrRow, intHdrCol, intHdrRow, intHdrCol + 1).Style.Border.OutsideBorder = XLBorderStyleValues.Thin
                            .Range(intHdrRow, intHdrCol, intHdrRow, intHdrCol + 1).Style.Font.Bold = True

                            .Range(intHdrRow, intHdrCol + 2, intHdrRow, intHdrCol + 4).Merge()
                            .Range(intHdrRow, intHdrCol + 2, intHdrRow, intHdrCol + 4).Value = argvsf.GetData(i, vOthFldDataValue)
                            .Range(intHdrRow, intHdrCol + 2, intHdrRow, intHdrCol + 4).Style.Border.OutsideBorder = XLBorderStyleValues.Thin
                        Next
                    Next
                End If

                Dim intLastRow As Int32 = intHdrRow
                intHdrRow = RowNo
                intHdrCol = intDetCol - 4

                'Transaction Total Section
                For i = 0 To argTot.Rows.Count - 1
                    intHdrRow = intHdrRow + 1
                    .Range(intHdrRow, intHdrCol, intHdrRow, intHdrCol + 1).Merge()
                    .Range(intHdrRow, intHdrCol, intHdrRow, intHdrCol + 1).Value = argTot.GetData(i, vOthFldCaption)
                    .Range(intHdrRow, intHdrCol, intHdrRow, intHdrCol + 1).Style.Border.OutsideBorder = XLBorderStyleValues.Thin
                    .Range(intHdrRow, intHdrCol, intHdrRow, intHdrCol + 1).Style.Font.Bold = True

                    .Range(intHdrRow, intHdrCol + 2, intHdrRow, intHdrCol + 4).Merge()
                    .Range(intHdrRow, intHdrCol + 2, intHdrRow, intHdrCol + 4).Value = argTot.GetData(i, vOthFldDataValue)
                    .Range(intHdrRow, intHdrCol + 2, intHdrRow, intHdrCol + 4).Style.Border.OutsideBorder = XLBorderStyleValues.Thin
                Next

                If intLastRow > intHdrRow Then
                    intHdrRow = intLastRow
                End If
                'Outer Border thick
                .Range(1, 1, intHdrRow, intDetCol).Style.Border.OutsideBorder = XLBorderStyleValues.Thick

                'Disable Gridlines
                .ShowGridLines = False

                'Autofit columns
                For i = 1 To intDetCol
                    .Columns(i).AdjustToContents()
                Next

                Call UpdateFixedRowForDocXlXML(xlWS, "For " & gSettings.gCompName, intHdrRow + 2, 3, 11, False, False)
                Call UpdateFixedRowForDocXlXML(xlWS, "Authorised Signatory", intHdrRow + 6, 3, 11, False, False)
            End With

            xlWB.SaveAs(Application.StartupPath & "\" & strFileName)
            'xlWB.Dispose()
            'xlWS.Dispose()
            System.Diagnostics.Process.Start(Application.StartupPath & "\" & strFileName)
            Dim p As New System.Diagnostics.Process
            Dim s As New System.Diagnostics.ProcessStartInfo(strFileName)
            s.UseShellExecute = True
            s.WindowStyle = ProcessWindowStyle.Normal
            p.StartInfo = s
            p.Start()
        Catch ex As Exception
            xlWB.Dispose()
            xlWS.Dispose()
            MsgBox("Doc Export Error..." & vbNewLine & ex.Message, MsgBoxStyle.Information, "Failed")
        End Try
    End Sub

    Public Function GetAllCtrls(control As Control, type As Type) As IEnumerable(Of Control)
        Dim controls = control.Controls.Cast(Of Control)()
        Return controls.SelectMany(Function(ctrl) GetAllCtrls(ctrl, type)).Concat(controls).Where(Function(c) c.[GetType]() = type)
    End Function

    Private Sub tsbBOM_Click(sender As System.Object, e As System.EventArgs) Handles tsbBOM.Click
        RaiseEvent btnClicked(svtDtlBtns.svtBOM)
    End Sub

    Private Sub tsbCalTotal_Click(sender As System.Object, e As System.EventArgs) Handles tsbCalTotal.Click
        RaiseEvent btnClicked(svtDtlBtns.svtCalTotal)
    End Sub

    Private Sub txbPending_Click(sender As Object, e As EventArgs) Handles tsbPending.Click
        RaiseEvent btnClicked(svtDtlBtns.svtPending)
    End Sub

    Private Sub tsbResize_Click(sender As Object, e As EventArgs) Handles tsbResize.Click
        RaiseEvent btnClicked(svtDtlBtns.svtResize)
    End Sub

    Private Sub GSeparator1_Paint(sender As Object, e As PaintEventArgs) Handles GSeparator1.Paint, GSeparator2.Paint, GSeparator3.Paint, GSeparator4.Paint, GSeparator5.Paint, GSeparator6.Paint, GSeparator7.Paint
        e.Graphics.DrawLine(Pens.Gray, 4, 6, 4, 22)
    End Sub

    Private Sub tsbAddnl_Click(sender As Object, e As EventArgs) Handles tsbAddnl.Click
        RaiseEvent btnClicked(svtDtlBtns.svtAddnl)
    End Sub

    Private Sub tsbAddnl_DropDownItemClicked(sender As Object, e As ToolStripItemClickedEventArgs) Handles tsbAddnl.DropDownItemClicked
        RaiseEvent btnOptionClicked(e.ClickedItem.Text)
    End Sub

    Public Sub SendTrnSMS(ByVal argObj As Object, ByVal argDocID As Int32, ByVal argTrnID As Int32)
        With New frmTrnSMS
            .intDocId = argDocID
            .initScreen(argTrnID)
            .ShowDialog()
        End With
    End Sub

    Private Sub tsbSMS_Click(sender As Object, e As EventArgs) Handles tsbSMS.Click
        RaiseEvent btnClicked(svtDtlBtns.svtSMS)
    End Sub
End Class
