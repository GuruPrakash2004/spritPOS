Imports System.Text
Imports System.ComponentModel
Imports C1.Win.C1FlexGrid
Imports Microsoft.Office.Interop.Excel
Public Class frmRptLblPrint

    Dim clsSVT As New svtLibrary
    Dim strSQL As String
    Dim sb As New StringBuilder()
    Dim dtSearch As DataTable
    Dim dvSearch As DataView
    Dim strBCFormatFile As String
    Dim strAdnlCond As String
    Dim Mode As String
    Dim argvsf As C1FlexGrid
    Dim strDocType As String
    Dim strSHForm As String
    Dim strDocHdrTblName As String
    Dim strDocDtlTblName As String
    Dim strDocIDFldName As String
    Dim strDocNoFldName As String
    Public intDocID As Int32

    Private Sub frmRptBCPrint_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.KeyPreview = True
        initScreen()
        initGridStyles(Me)
        Call bindCombo(cboDoc, "DocName", "DocName", "Select DocName from UtlTransactions order by DocName")
        cboTemplate.SelectedIndex = -1
        txtNoOfCopies.Text = 1
        Call initGridStyles(Me)
        Call fillFormProperties(Me)
        Call ChangeFontSize(Me)
        cboDoc.Text = ""
        cboTemplate.Text = ""
        If intDocID > 0 Then
            cboDoc.Enabled = False
            Call getUtlTrnData(intDocID)
            Call cboDoc_Validating(sender, Nothing)
            If cboTemplate.Items.Count > 0 Then
                Call LoadQry()
                Call ReplaceData()
            End If
        End If
    End Sub


    Private Sub getUtlTrnData(ByVal argDocID As Integer)
        Dim rsDummy As oledb.oledbDataReader
        rsDummy = clsSVT.getDataReaderFromSQL("Select * from UtlTransactions where DocID = " & argDocID)

        With rsDummy
            If .HasRows Then
                .Read()
                cboDoc.Text = GetVal(!DocName, "")
                strDocType = GetVal(!DocType, "")
                strSHForm = GetVal(!DocShForm, "")
                strDocNoFldName = strSHForm & "No"
                strDocIDFldName = strSHForm & "ID"
                strDocHdrTblName = GetVal(!HdrTblName, "")
                strDocDtlTblName = GetVal(!DtlTblName, "")
            End If
            .Close()
        End With
    End Sub

    Private Sub initScreen()
        With vsfDet
            .Rows.Count = 1
            .Cols.Count = 1
        End With

        With vsfView
            .Rows.Count = 1
            .Cols.Count = 1
        End With

        With vsfSearch
            .Rows.Count = 1
            .Cols.Count = 1
        End With

        With vsfSqry
            .Rows.Count = 21
            .Cols.Count = 1
            .SetData(0, 0, "Label Fields")
        End With
    End Sub

    Private Sub cboTemplate_Validating(sender As Object, e As CancelEventArgs) Handles cboTemplate.Validating
        If Trim(cboDoc.Text = "") Or Trim(cboTemplate.Text) = "" Then Exit Sub
        Dim blnFound As Boolean
        Call clsSVT.getDataLongValue("Select LblID from UtlLabelPrint where DocName = '" & cboDoc.Text & "' and TemplateName ='" & Trim(cboTemplate.Text) & "'", 0, blnFound)

        If Not blnFound Then
            MsgBox("Template Not found in the list!!", MsgBoxStyle.Information, Me.Text)
            cboTemplate.Focus()
            Exit Sub
        End If
    End Sub

    Private Sub LoadQry()
        If Trim(cboTemplate.Text) = String.Empty Then Exit Sub
        With vsfSqry
            For i As Int32 = 1 To .Rows.Count - 1
                .SetData(i, 0, "")
            Next

            Dim rs As oledb.oledbDataReader
            rs = clsSVT.getDataReaderFromSQL("Select * from UtlLabelPrint where TemplateName = '" & Trim(cboTemplate.Text) & "'")
            If Not rs.HasRows Then
                rs.Close()
                Exit Sub
            End If

            With rs
                .Read()
                For i = 1 To vsfSqry.Rows.Count - 1
                    vsfSqry.SetData(i, 0, GetVal(.Item("FLDContent" & i), ""))
                Next
                .Close()
            End With
        End With
    End Sub

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        If Me.WindowState = FormWindowState.Normal Then
            Me.Close()
            Exit Sub
        End If
        Call frmMain.CloseSelectedTabPage(Me)
    End Sub

    Private Sub btnDesign_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDesign.Click
        If cboDoc.Text = "" Then

            Exit Sub
        End If
        Dim objFrm As New frmUtlLabelDesign
        objFrm.StartPosition = FormStartPosition.CenterScreen
        objFrm.strDocName = cboDoc.Text
        objFrm.ShowDialog()
        objFrm.Dispose()
        Call bindCombo(cboTemplate, "TemplateName", "TemplateName", "Select TemplateName from UtlLabelPrint where DocName = '" & cboDoc.Text & "'")
        cboTemplate.Text = ""
    End Sub

    Private Sub btnLoad_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLoad.Click
        If Trim(cboDoc.Text) = String.Empty And Trim(cboTemplate.Text) = String.Empty Then Exit Sub
        Call getUtlTrnData(intDocID)
        If Trim(cboTemplate.Text) = String.Empty Then Exit Sub
        If Trim(txtNoOfCopies.Text) = String.Empty Then
            MsgBox("Please enter the number of copies.", vbInformation, Me.Text)
            txtNoOfCopies.Focus()
        End If
        Call LoadQry()
        Call ReplaceData()
    End Sub

    Private Sub ReplaceData()
        Dim strval As String
        Dim strFlds() As String
        Dim strResult As String = ""
        Dim strQuery As String = ""
        Dim rs As oledb.oledbDataReader
        Dim i As Integer, j As Integer

        strSQL = "select * from " & strDocHdrTblName & " where 1=1 "
        If strDocType.ToUpper = "MASTER" Then
            If fnFldPresent(strDocIDFldName, strDocHdrTblName) Then
                strSQL &= " and " & strDocIDFldName & ">0 "
            End If
            If fnFldPresent("InActive", strDocHdrTblName) Then
                strSQL &= " And InActive='N' "
            Else
                'Conditions if any
            End If
        End If
        strSQL &= " Order by 1 desc"

        rs = clsSVT.getDataReaderFromSQL(strSQL)
        strQuery = ""

        If Not rs.HasRows Then
            rs.Close()
            Exit Sub
        End If

        With vsfSqry
            For i = 1 To vsfSqry.Rows.Count - 1
                If GetVal(.GetData(i, 0), "") <> "" Then
                    strQuery &= GetVal(.GetData(i, 0), "") & vbNewLine
                End If
            Next
        End With

        strFlds = Split(getStrFlds, ",")

        Try
            With vsfView
                .Rows.Count = 1
                .Cols.Count = 1
                .SetData(0, 0, "Labels ")

                Do While rs.Read
                    strResult = strQuery
                    For j = 0 To UBound(strFlds)
                        If InStr(UCase(strResult), UCase(strFlds(j))) > 0 Then
                            strval = Replace(strFlds(j), "{", "")
                            strval = Replace(strval, "}", "")
                            strResult = Replace(strResult, strFlds(j), GetVal(rs(strval)))
                        End If
                    Next
                    .Rows.Count = .Rows.Count + 1
                    .SetData(.Rows.Count - 1, 0, strResult)
                Loop
                rs.Close()
                .AutoSizeRows()
                .AutoSizeCols()
            End With

        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, Me.Text)
            rs.Close()
            Cursor.Current = Cursors.Default
        End Try
    End Sub

    Private Sub UpdateString(ByVal str As String, ByVal str1 As String, ByRef strval As String)

    End Sub

    Private Function getStrFlds() As String
        getStrFlds = ""
        Dim rsFldsCodes As oledb.oledbDataReader
        rsFldsCodes = clsSVT.getDataReaderFromSQL("Select Column_Name from Information_schema.Columns where Table_name='" & strDocHdrTblName & "'")
        Try
            With rsFldsCodes
                Do While .Read()
                    getStrFlds = getStrFlds & "{" & Trim(UCase(!Column_Name)) & "}" & ","
                Loop
                getStrFlds = Mid(getStrFlds, 1, Len(getStrFlds) - 1)
                .Close()
            End With
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, Me.Text)
            rsFldsCodes.Close()
            Cursor.Current = Cursors.Default
        End Try
    End Function

    Private Sub vsfView_AfterResizeColumn(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfView.AfterResizeColumn
        Call syncGridCols()
    End Sub

    Private Sub vsfView_BeforeEdit(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfView.BeforeEdit
        e.Cancel = True
    End Sub

    Private Sub syncGridCols()
        Dim i As Integer
        With vsfView
            vsfSearch.Cols.Count = vsfView.Cols.Count
            For i = 0 To .Cols.Count - 1
                vsfSearch.Cols(i).Width = vsfView.Cols(i).Width
                vsfSearch.Cols(i).Visible = vsfView.Cols(i).Visible
            Next i
        End With
    End Sub

    Private Sub vsfView_AfterScroll(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.RangeEventArgs) Handles vsfView.AfterScroll
        vsfSearch.LeftCol = vsfView.LeftCol
    End Sub

    Private Sub vsfSearch_ChangeEdit(ByVal sender As Object, ByVal e As System.EventArgs) Handles vsfSearch.ChangeEdit
        Call applyFilterFromGrid()
    End Sub

    Private Function applyFilterFromGrid() As Boolean
        Dim StrQry As String, strValue As String
        Dim i As Integer

        applyFilterFromGrid = False
        StrQry = ""
        With vsfSearch
            For i = 0 To .Cols.Count - 1
                strValue = ""
                If i = .ColSel Then
                    If .Editor.Text <> "" Then
                        strValue = Trim(.Editor.Text)
                    End If
                Else
                    If .GetData(0, i) <> "" Then
                        strValue = Trim(.GetData(0, i))
                    End If
                End If
                If strValue <> "" Then
                    If StrQry <> "" Then StrQry = StrQry & " and "
                    StrQry = StrQry & vsfView.GetData(0, i) & " like '*" & strValue & "*'"
                End If
            Next i
        End With
        'If StrQry <> "" Then
        '    vsfView.DataSource = dt.Select(StrQry)
        'Else
        '    vsfView.DataSource = dt
        'End If
        dvSearch.RowFilter = StrQry
        Call syncGridCols()
        applyFilterFromGrid = True
    End Function

    Private Sub vsfSearch_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles vsfSearch.KeyUp
        If e.KeyCode = Keys.Down Then
            vsfView.Focus()
            If vsfView.Rows.Count > 1 Then
                vsfView.Row = 1
            End If
        End If

        If e.KeyCode = Keys.Return Then
            If vsfView.Rows.Count > 1 Then
                vsfView.Row = 1
                Exit Sub
            End If
        End If

    End Sub

    Private Sub vsfSearch_KeyUpEdit(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.KeyEditEventArgs) Handles vsfSearch.KeyUpEdit
        If e.KeyCode = Keys.Down Then
            vsfView.Focus()
            If vsfView.Rows.Count > 1 Then
                vsfView.Row = 1
            End If
        End If
        If e.KeyCode = Keys.Return Then
            If vsfView.Rows.Count > 1 Then
                vsfView.Row = 1
                Exit Sub
            End If
        End If

    End Sub

    Private Sub vsfSearch_RowColChange(ByVal sender As Object, ByVal e As System.EventArgs) Handles vsfSearch.RowColChange
        vsfView.Col = vsfSearch.Col
        vsfView.Refresh()
    End Sub


    Private Sub btnSelAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSelAll.Click
        Dim i As Integer, j As Integer
        vsfDet.Rows.Count = 1
        With vsfView
            For i = 1 To .Rows.Count - 1
                vsfDet.Rows.Count = vsfDet.Rows.Count + 1
                For j = 0 To .Cols.Count - 1
                    vsfDet.SetData(i, j, .GetData(i, j))
                Next j
            Next i
        End With
        vsfDet.AutoSizeCols()
        vsfDet.AutoSizeRows()
    End Sub

    Private Sub txtNoOfCopies_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtNoOfCopies.TextChanged

    End Sub

    Private Sub vsfView_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles vsfView.DoubleClick
        If vsfView.Row <= 0 Then Exit Sub
        Dim i As Integer, intCol As Integer
        Dim strPendGrid As String, strSelectedGrid As String

        With vsfDet
            strPendGrid = ""
            For intCol = 0 To .Cols.Count - 1
                strPendGrid = strPendGrid & "-" & vsfView.GetData(vsfView.Row, intCol)
            Next intCol

            For i = 1 To .Rows.Count - 1
                strSelectedGrid = ""
                For intCol = 0 To vsfView.Cols.Count - 1
                    strSelectedGrid = strSelectedGrid & "-" & .GetData(i, intCol)
                Next intCol

                If strSelectedGrid = strPendGrid Then
                    MsgBox("Item Already present!!", vbInformation, Me.Text)
                    Exit Sub
                End If
            Next i
        End With

        With vsfView
            vsfDet.Rows.Count = vsfDet.Rows.Count + 1
            i = vsfDet.Rows.Count - 1

            For intCol = 0 To .Cols.Count - 1
                vsfDet.SetData(i, intCol, .GetData(.Row, intCol))
            Next intCol
            vsfDet.AutoSizeRows()
            vsfDet.AutoSizeCols()
        End With
    End Sub

    Public Sub loadCustom(ByVal argCond As String)
        strAdnlCond = argCond
        ' Call fillViewGrid()
    End Sub

    Private Sub btnExportToXL_Click(sender As Object, e As EventArgs) Handles btnExportToXL.Click
        If gBlnMSOffice Then
            Call ExpToMSExcel()
        Else
            Call ExpToOpenOfficeExcel()
        End If
    End Sub
    Private Sub expToOpenOfficeExcel()
        Try
            Dim xlWB As New ClosedXML.Excel.XLWorkbook()
            xlWB.Worksheets.Add("Sheet1")
            Dim xlWS As ClosedXML.Excel.IXLWorksheet = xlWB.Worksheet("Sheet1")

            Dim intRow As Int32 = 2
            Dim intCol As Int32 = 2

            Dim NoOfCol As Int32 = Val(txtNoOfCopies.Text)
            Dim intRunningCol As Int32 = 1
            Dim strval As String = ""

            With vsfDet
                For i As Int32 = 1 To .Rows.Count - 1
                    If intRunningCol > NoOfCol Then
                        intRunningCol = 1
                        intRow = intRow + 2
                        intCol = 2
                    End If
                    strval = ""
                    For k As Int32 = 0 To .Cols.Count - 1
                        strval &= .GetData(i, k) & vbNewLine
                    Next
                    xlWS.Cell(intRow, intCol).Value = strval
                    intRunningCol = intRunningCol + 1
                    intCol = intCol + 2
                Next
            End With
            Dim strFileName As String = cboDoc.Text & "_" & cboTemplate.Text & Date.Now.Ticks & ".xlsx"
            xlWB.SaveAs(System.Windows.Forms.Application.StartupPath & "\Temp\" & strFileName)
            Process.Start(System.Windows.Forms.Application.StartupPath & "\Temp\" & strFileName)
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, Me.Text)
        End Try

    End Sub
    Public Sub ExpToMSExcel()
        Try
            Dim xlappl As New Microsoft.Office.Interop.Excel.Application
            Dim xlBook As Microsoft.Office.Interop.Excel.Workbook = xlappl.Workbooks.Add()
            xlBook = xlappl.Workbooks.Add
            Dim intRow As Int32 = 2
            Dim intCol As Int32 = 2

            Dim NoOfCol As Int32 = Val(txtNoOfCopies.Text)
            Dim intRunningCol As Int32 = 1
            Dim strval As String = ""

            With vsfDet
                For i As Int32 = 1 To .Rows.Count - 1
                    If intRunningCol > NoOfCol Then
                        intRunningCol = 1
                        intRow = intRow + 2
                        intCol = 2
                    End If
                    strval = ""
                    For k As Int32 = 0 To .Cols.Count - 1
                        strval &= .GetData(i, k) & vbNewLine
                    Next
                    xlappl.ActiveSheet.Cells(intRow, intCol).Value = strval
                    'xlappl.ActiveSheet.Column(intCol).AdjustToContents()
                    intRunningCol = intRunningCol + 1
                    intCol = intCol + 2
                Next
            End With
            Dim strFileName As String = cboDoc.Text & "_" & cboTemplate.Text & Date.Now.Ticks & ".xlsx"
            xlBook.SaveAs(System.Windows.Forms.Application.StartupPath & "\Temp\" & strFileName)
            xlBook.Close(False)
            xlappl.Quit()
            releaseObject(xlappl)
            releaseObject(xlBook)
            Process.Start(System.Windows.Forms.Application.StartupPath & "\Temp\" & strFileName)
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, Me.Text)
        End Try
    End Sub
    Private Sub releaseObject(ByVal obj As Object)
        Try
            System.Runtime.InteropServices.Marshal.ReleaseComObject(obj)
            obj = Nothing
        Catch ex As Exception
            obj = Nothing
        Finally
            GC.Collect()
        End Try
    End Sub
    Private Sub chkEditor_CheckedChanged(sender As Object, e As EventArgs) Handles chkEditor.CheckedChanged
        If chkEditor.Checked = True Then
            vsfSqry.Visible = True
            vsfSqry.BringToFront()
        Else
            vsfSqry.Visible = False
        End If
    End Sub

    Private Sub frmRptLblPrint_KeyUp(sender As Object, e As KeyEventArgs) Handles Me.KeyUp
        Dim blnFnKey As Boolean = False
        If e.KeyCode = Keys.F9 And e.Control = True Then
            Call initFormSize(Me, Me.Text, True, 972, 573, 0, 0)
            Call initFormVsfDet(Me, Me.Text, vsfView, "View")
            Call initFormVsfDet(Me, Me.Text, vsfDet, "det")
            MsgBox("Restored Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
        If e.KeyCode = Keys.F8 And e.Control = True Then
            Call SaveActFormSettings(Me, Me.Text)
            Call SaveActFormVsfDet(Me, Me.Text, "View", vsfView)
            Call initFormVsfDet(Me, Me.Text, vsfDet, "det")
            MsgBox("Saved Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
        If e.Control = False And e.Alt = True And e.KeyCode = Keys.E Then
            If gBlnMSOffice Then
                Call ExpToMSExcel()
            Else
                Call expToOpenOfficeExcel()
            End If
        End If
    End Sub

    Private Sub cboTemplate_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cboTemplate.KeyPress
        If e.KeyChar = "'" Then e.Handled = True
    End Sub

    Private Sub cboDoc_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cboDoc.KeyPress
        If e.KeyChar = "'" Then e.Handled = True
    End Sub

    Private Sub cboDoc_Validating(sender As Object, e As CancelEventArgs) Handles cboDoc.Validating
        If cboDoc.Text = "" Then Exit Sub
        Dim blnFound As Boolean
        intDocID = clsSVT.getDataLongValue("Select DocID from UtlTransactions where DocName = '" & cboDoc.Text & "'", 0, blnFound)
        If intDocID <= 0 Then
            MsgBox("Document not found!", vbInformation, Me.Text)
            cboDoc.Focus()
            Exit Sub
        End If
        Call bindCombo(cboTemplate, "TemplateName", "TemplateName", "Select TemplateName from UtlLabelPrint where DocName = '" & cboDoc.Text & "'")
    End Sub

    Private Sub vsfDet_BeforeEdit(sender As Object, e As RowColEventArgs) Handles vsfDet.BeforeEdit
        e.Cancel = True
    End Sub

    Private Sub txtNoOfCopies_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtNoOfCopies.KeyPress
        If Not KeyValidate("Integer", e.KeyChar) Then
            e.Handled = True
        End If
    End Sub

    Private Sub vsfDet_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles vsfDet.MouseDoubleClick
        If vsfDet.Rows.Count <= 1 Then Exit Sub
        With vsfDet
            .RemoveItem(vsfDet.Row)
        End With
    End Sub
    Private Sub vsfView_Click(sender As Object, e As EventArgs) Handles vsfView.Click
        If (Control.ModifierKeys And Keys.Control) = Keys.Control AndAlso (Control.ModifierKeys And Keys.Alt) = Keys.Alt Then
            pnlCopyShortCut.Show()
        End If
        argvsf = vsfView
    End Sub
    Private Sub pnlbtnClose_Click(sender As Object, e As EventArgs) Handles pnlbtnClose.Click
        pnlCopyShortCut.Visible = False
    End Sub

    Private Sub btnCopy_Click(sender As Object, e As EventArgs) Handles btnCopy.Click

        If RbnCopyCell.Checked = True Then
            CopyGrid(argvsf, "Cell")
        ElseIf RbnCopyColumn.Checked = True Then
            CopyGrid(argvsf, "Column")
        ElseIf RbnCopyRow.Checked = True Then
            CopyGrid(argvsf, "Row")
        ElseIf RbnCopyGrid.Checked = True Then
            CopyGrid(argvsf, "Full Grid")

        End If

        pnlCopyShortCut.Visible = False
    End Sub

    Private Sub vsfDet_Click(sender As Object, e As EventArgs) Handles vsfDet.Click

        If (Control.ModifierKeys And Keys.Control) = Keys.Control AndAlso (Control.ModifierKeys And Keys.Alt) = Keys.Alt Then
            pnlCopyShortCut.Show()
        End If
        argvsf = vsfDet

    End Sub
End Class