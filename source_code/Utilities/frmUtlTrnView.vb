Imports System.Data.oledb
Imports System.IO
Imports C1.Win.C1FlexGrid

Public Class frmUtlTrnView
    Dim clsSVT As New svtLibrary
    Dim strSQL As String
    Dim argCForm As Object
    Dim dt As DataTable
    Dim dv As DataView
    Dim intTotRec As Long
    Const vDetSlNo = 0
    Dim docName As String
    Dim tblName As String
    Dim mainField As String
    Dim intDOCID As Integer
    Dim strHDRTBL As String = "", strDTLTBL As String = "", strDocSHR As String = ""

    Public Sub FillViewVsf(ByVal argFrm As Form, ByVal argSQL As String, ByVal argHdrLine1 As String, ByVal argHdrLine2 As String, Optional ByVal argDOCID As Integer = 0, Optional ByVal argDocName As String = "")
        Dim strScrollBarWidth As Integer, strSelCols As String, strFldsOrderBy As String
        Dim i As Integer
        argDocName = argDocName & "VIEW"

        strSelCols = clsSVT.getDataStringValue("Select SelCols from UtlSelCols " _
                & " where UserCode = '" & gUserCode & "' and formTag= '" & argDocName & "'", "", False)

        strSQL = "Select FldsOrderby from UtlSelCols where formTag='" & argDocName & "' and UserCode = '" & UCase(gUserCode) & "'"
        strFldsOrderBy = clsSVT.getDataStringValue(strSQL, "", False)

        intDOCID = argDOCID
        strSQL = argSQL
        Cursor.Current = Cursors.WaitCursor
        dt = clsSVT.getDATATable(strSQL)
        dv = New DataView(dt)

        vsfDet.Rows.Fixed = 5
        vsfDet.Rows.GlyphRow = 3
        vsfDet.Rows(4).Visible = False
        vsfDet.BeginUpdate()

        vsfDet.DataSource = dv
        Cursor.Current = Cursors.Arrow
        argCForm = argFrm
        Call initVsfForHdrs(argHdrLine1, argHdrLine2)

        vsfDet.EndUpdate()
        vsfSearch.Cols.Count = vsfDet.Cols.Count

        If Trim(strSelCols) <> "" Then
            With vsfDet
                For i = 0 To .Cols.Count - 1
                    .Cols(i).Visible = False
                Next
                Dim intRow As Int32 = 1
                For Each strCol As String In strSelCols.Split(",")
                    If Trim(strCol) <> "" Then
                        For i = 1 To .Cols.Count - 1
                            If .GetData(4, i) = strCol Then
                                .Cols(i).Visible = True
                                .Cols(i).Move(intRow)
                                intRow = intRow + 1
                            End If
                        Next
                    End If
                Next
            End With
        End If

        'If Trim(strFldsOrderBy) <> "" Then
        '    With vsfDet
        '        Dim strFldCode As String()
        '        strFldCode = strFldsOrderBy.Split(",")
        '        For i = 0 To UBound(strFldCode) - 1
        '            For j = 0 To .Cols.Count - 1
        '                If .GetData(4, j).ToString = strFldCode(i) Then
        '                    Try
        '                        .Cols.Move(j, i)
        '                    Catch
        '                    End Try
        '                End If
        '            Next
        '        Next
        '    End With
        'End If

        'If Trim(strSelCols) <> "" Then
        '    With vsfDet
        '        For i = 0 To .Cols.Count - 1
        '            .Cols(i).Visible = False
        '        Next
        '        For i = 0 To .Cols.Count - 1
        '            If InStr(1, UCase(strSelCols), UCase(.GetData(4, i))) > 0 Then
        '                .Cols(i).Visible = True
        '            End If
        '        Next
        '    End With
        'End If

        Call fillColCaptions(intDOCID)
        For i = 0 To vsfDet.Cols.Count - 1
            If Trim(vsfDet.GetData(3, i)) = "" Then
                vsfDet.SetData(3, i, Trim(vsfDet.GetData(4, i)))
            End If
        Next
        Call vsfDet.AutoSizeCols()
        Call syncGridCols()
        strScrollBarWidth = SystemInformation.VerticalScrollBarWidth
        vsfSearch.Cols(vsfSearch.Cols.Count - 1).Width = vsfDet.Cols(vsfDet.Cols.Count - 1).Width + strScrollBarWidth

        intTotRec = Val(vsfDet.Rows.Count) - 5
        lblTotRec.Text = "Total Records: " & Val(Val(vsfDet.Rows.Count) - 5) & "/" & intTotRec
        vsfDet.Rows(0).TextAlignFixed = TextAlignEnum.LeftCenter
        vsfDet.Rows(1).TextAlignFixed = TextAlignEnum.LeftCenter
    End Sub

    Private Sub fillColCaptions(ByVal argDocID As Integer)
        Call fillCaptionGbl()
        Dim i As Integer
        Dim strSQL As String
        Dim rs As oledb.oledbDataReader
        strSQL = "Select FldCode, FldCaption from UtlDocDsgrFlds where DocId = " & argDocID
        rs = clsSVT.getDataReaderFromSQL(strSQL)

        With rs
            If Not .HasRows Then
                rs.Close()
                Exit Sub
            End If
            Do While .Read
                For i = 0 To vsfDet.Cols.Count - 1
                    If Trim(vsfDet.GetData(4, i)) = rs("FldCode") Then
                        vsfDet.SetData(3, i, rs("FldCaption"))
                        Exit For
                    End If
                Next
            Loop
        End With
        rs.Close()
    End Sub
    Private Sub fillCaptionGbl()
        Dim i As Integer
        Dim strSQL As String
        Dim rs As oledb.oledbDataReader
        strSQL = "Select Code, Caption from UtlFldCaptions order by 1"
        rs = clsSVT.getDataReaderFromSQL(strSQL)

        With rs
            If Not .HasRows Then
                rs.Close()
                Exit Sub
            End If
            Do While .Read
                For i = 0 To vsfDet.Cols.Count - 1
                    If Trim(vsfDet.GetData(4, i)) = rs("Code") Then
                        vsfDet.SetData(3, i, rs("Caption"))
                        Exit For
                    End If
                Next
            Loop
        End With
        rs.Close()
    End Sub


    Public Sub searchSettings(ByVal argDocName As String, ByVal argTblName As String, ByVal argMainFld As String)
        With Me
            docName = argDocName
            tblName = argTblName
            mainField = argMainFld
        End With
    End Sub

    Private Sub syncGridCols()
        Dim i As Integer
        With vsfDet
            vsfSearch.Cols.Count = vsfDet.Cols.Count
            For i = 0 To .Cols.Count - 1
                vsfSearch.Cols(i).Width = vsfDet.Cols(i).Width
                vsfSearch.Cols(i).Visible = vsfDet.Cols(i).Visible
            Next i
        End With
    End Sub

    Private Sub initVsfForHdrs(ByVal argHdrLine1 As String, ByVal argHdrLine2 As String)
        Dim i As Integer

        With vsfDet
            For i = 0 To .Cols.Count - 1
                .SetData(4, i, .GetData(0, i))

                .SetData(0, i, gSettings.gCompName)
                .SetData(1, i, argHdrLine1)
                .SetData(2, i, argHdrLine2)
            Next
            Dim csf1 As C1.Win.C1FlexGrid.CellStyle = vsfDet.Styles.Add("F1")
            csf1.BackColor = Color.Beige

            .Rows(0).Style = csf1
            .Rows(1).Style = csf1
            .Rows(2).Style = csf1

            If argHdrLine1 = "" Then .Rows(1).Visible = False
            If argHdrLine2 = "" Then .Rows(2).Visible = False

            .Rows(0).AllowMerging = True
            .Rows(1).AllowMerging = True
            .Rows(2).AllowMerging = True
            .Rows(3).AllowMerging = True
            .Rows(4).AllowMerging = True
        End With
    End Sub

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub vsfDet_AfterResizeColumn(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfDet.AfterResizeColumn
        Call syncGridCols()
    End Sub

    Private Sub vsfDet_BeforeEdit(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfDet.BeforeEdit
        e.Cancel = True
    End Sub

    'Private Sub vsfDet_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles vsfDet.DoubleClick
    '    'Call argCForm.FillDocDetails(vsfDet.GetData(vsfDet.Row, 0))
    '    'If chkClose.Checked = False Then Me.Close()
    '    If vsfDet.Row <= 2 Then
    '        Exit Sub
    '    End If

    '    If argCForm.visible = False Then
    '        Me.Close()
    '        Exit Sub
    '    End If

    '    Call argCForm.FillDocDetails(vsfDet.GetData(vsfDet.Row, 0))

    '    If chkClose.Checked = False Then
    '        Me.Close()
    '    Else
    '        argCForm.bringtoFront()
    '    End If
    'End Sub

    Private Sub btnExportPDF_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        'Dim strFile As String
        'Try
        '    Dim pdfCreator As New GridToPdf(vsfDet)
        '    strFile = Application.StartupPath & "\Temp\PDFExport" & Date.Now.Ticks & ".pdf"
        '    pdfCreator.Save(strFile)

        '    Dim p As New System.Diagnostics.Process
        '    Dim s As New System.Diagnostics.ProcessStartInfo(strFile)
        '    s.UseShellExecute = True
        '    s.WindowStyle = ProcessWindowStyle.Normal
        '    p.StartInfo = s
        '    p.Start()
        'Catch ex As Exception
        '    MsgBox("Cannot export to Pdf." & vbCrLf & ex.Message, MsgBoxStyle.Critical, Me.Text)
        'End Try
    End Sub

    Private Sub vsfDet_AfterScroll(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.RangeEventArgs) Handles vsfDet.AfterScroll
        'vsfSearch.LeftCol = vsfDet.LeftCol
        vsfSearch.ScrollPosition = vsfDet.ScrollPosition
    End Sub

    Private Sub vsfSearch_AfterScroll(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.RangeEventArgs) Handles vsfSearch.AfterScroll
        'vsfSearch.ScrollPosition = vsfDet.ScrollPosition

        Dim intv As Integer = vsfSearch.ScrollPosition.X
        Dim inth As Integer = vsfDet.ScrollPosition.Y
        vsfDet.ScrollPosition = New System.Drawing.Point(intv, inth)
    End Sub

    Private Sub vsfSearch_ChangeEdit(ByVal sender As Object, ByVal e As System.EventArgs) Handles vsfSearch.ChangeEdit
        Call applyFilterFromGrid()
    End Sub

    Private Function applyFilterFromGrid() As Boolean
        Dim StrQry As String, strValue As String
        Dim i As Integer
        applyFilterFromGrid = False
        Try
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
                        Select Case gDBTypeCurrent
                            Case gDbTypeAccess
                                StrQry = StrQry & vsfDet.GetData(4, i) & " like '*" & strValue & "*'"
                            Case Else
                                'If vsfDet.Cols(i).DataType = GetType(Decimal) Or vsfDet.Cols(i).DataType = GetType(Integer) Then
                                '    Try
                                '        Dim strTest = Convert.ToDecimal(strValue)
                                '        StrQry = StrQry & vsfDet.GetData(3, i) & " >= " & strValue
                                '    Catch ex As Exception
                                '        ' vsfSearch.SetData(0, i, "")
                                '        dv.RowFilter = "1 <> 1"
                                '        Exit Function
                                '    End Try
                                If vsfDet.Cols(i).DataType = GetType(Decimal) Or vsfDet.Cols(i).DataType = GetType(Integer) Or vsfDet.Cols(i).DataType = GetType(Date) Then
                                    Try
                                        StrQry = StrQry & "Convert(" & vsfDet.GetData(4, i) & ",'System.String')" & " like '" & strValue & "*'"
                                    Catch ex As Exception
                                        dv.RowFilter = "1 <> 1"
                                        Exit Function
                                    End Try
                                Else
                                    StrQry = StrQry & vsfDet.GetData(4, i) & " like '%" & strValue & "%'"
                                End If
                        End Select
                    End If
                Next i
            End With
            'If StrQry <> "" Then
            '    vsfDet.DataSource = dt.Select(StrQry)
            'Else
            '    vsfDet.DataSource = dt
            'End If
            Dim k As Integer = vsfDet.Rows.Count
            dv.RowFilter = StrQry
            lblTotRec.Text = "Total Records: " & Val(Val(vsfDet.Rows.Count) - 5) & "/" & intTotRec
            Call syncGridCols()
            applyFilterFromGrid = True
        Catch ex As Exception

        End Try

    End Function

    Private Sub frmUtlView_KeyUp(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyUp
        If e.KeyCode = Keys.Enter Then
            With vsfDet
                If .Row <> -1 Then
                    If .Row <= 4 Then
                        Exit Sub
                    End If

                    If argCForm.visible = False Then
                        Me.Close()
                        Exit Sub
                    End If

                    Call argCForm.FillDocDetails(Val(vsfDet.GetData(.Row, 0)))

                    If chkClose.Checked = False Then
                        Me.Close()
                    Else
                        argCForm.bringtoFront()
                    End If
                End If
            End With
        End If

    End Sub

    Private Sub frmUtlView_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim tooltip As New ToolTip
        initGridStyles(Me)
        KeyPreview = True
        tooltip.SetToolTip(Me.btnClose, "Close:Alt+O")

    End Sub

    Public Sub bindToolStripCombo(ByVal argComboBox As System.Windows.Forms.ToolStripComboBox, ByVal argValueMember As String, ByVal argDisplayMember As String, ByVal argSQL As String, Optional ByVal BranchConn As oledbConnection = Nothing)
        Dim dt As DataTable
        dt = clsSVT.getDATATable(argSQL, BranchConn)
        argComboBox.ComboBox.DataSource = dt
        argComboBox.ComboBox.DisplayMember = argDisplayMember
        argComboBox.ComboBox.ValueMember = argValueMember
    End Sub

    Private Sub fillDtlData()
        Dim rs As oledb.oledbDataReader
        Dim strSQL As String

        strSQL = "select * from utltransactions where DocID = " & intDOCID

        rs = clsSVT.getDataReaderFromSQL(strSQL)


        Do While rs.Read
            strHDRTBL = GetVal(rs!HDRTblName)
            strDTLTBL = GetVal(rs!DtlTBLName)
            strDocSHR = GetVal(rs!DocSHForm)
        Loop
        rs.Close()
    End Sub

    Private Sub vsfDet_MouseDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles vsfDet.MouseDoubleClick
        'Dim ht = vsfVwDet.HitTest

        'If ht.Row <> -1 Then
        '    'If (Not chkUserRights(strRights, chkRightsPos.EditRights, Me.Text)) Then Exit Sub
        '    If vsfVwDet.Rows.Count > 1 And ht.Row > 0 Then
        '        With vsfVwDet
        '            pnlEdit.Visible = True
        '            FillEditDtlGrid(.GetData(ht.Row, vVwDetRMTypeID))
        '            vsfDet.Focus()
        '        End With
        Try


            Dim ht = vsfDet.HitTest

            If ht.Row <> -1 Then
                If ht.Row <= 4 Then
                    Exit Sub
                End If

                If argCForm.visible = False Then
                    '''Changed Code 01Feb2019
                    'Me.Close()
                    'Exit Sub
                    MsgBox("Please Open " & Me.Text & " Form", vbInformation, Me.Text)
                    '''Changed Code 01Feb2019
                End If

                Call argCForm.FillDocDetails(vsfDet.GetData(ht.Row, 0))

                If chkClose.Checked = False Then
                    Me.Close()
                Else
                    Me.WindowState = FormWindowState.Minimized
                    'argCForm.bringtoFront()
                End If
            End If
        Catch ex As Exception
            MsgBox("Data Load Issues" & vbNewLine & ex.Message, MsgBoxStyle.Exclamation, Me.Text)
        End Try
    End Sub

    Private Sub vsfSearch_KeyUpEdit(sender As Object, e As C1.Win.C1FlexGrid.KeyEditEventArgs) Handles vsfSearch.KeyUpEdit
        If e.KeyCode = Keys.Down Then
            vsfDet.Row = 4
            vsfDet.Col = vsfSearch.Col
            vsfDet.Focus()
        End If
    End Sub

    Private Sub TStripFreeze_Click(sender As System.Object, e As System.EventArgs) Handles TStripFreeze.Click
        If TStripFreeze.Text = "&Freeze Columns" Then
            TStripFreeze.ToolTipText = "UnFreeze Columns : Alt+U"
            TStripFreeze.Text = "&UnFreeze Columns"
            vsfDet.Cols.Frozen = vsfDet.Col + 1
            vsfDet.Cols.Frozen = vsfDet.Col + 1

            vsfSearch.Cols.Frozen = vsfDet.Col + 1
            vsfSearch.Cols.Frozen = vsfDet.Col + 1
        Else
            TStripFreeze.Text = "&Freeze Columns"
            TStripFreeze.ToolTipText = "Freeze Columns : Alt+F"
            vsfDet.Cols.Frozen = 0
            vsfDet.Cols.Frozen = 0

            vsfSearch.Cols.Frozen = 0
            vsfSearch.Cols.Frozen = 0
        End If
    End Sub

    Private Sub TStripExportXL_Click(sender As System.Object, e As System.EventArgs) Handles TStripExportXL.Click
        Call ExportToXL(vsfDet)
    End Sub

    Private Sub TStripSelCols_Click(sender As System.Object, e As System.EventArgs) Handles TStripSelCols.Click
        Dim frmUtlSearch As New frmUtlSearchSettings
        With frmUtlSearch
            .strDocName = docName & "VIEW"
            .strTblName = tblName
            .strMainFld = mainField
            .blnSRCHFrom = True
            .dblTotCol = vsfDet.Cols.Count
            .argForm = Me
            '.MdiParent = frmMain
            '.Show()
            .ShowDialog()
            Call RefreshGrid()
        End With
    End Sub

    Private Sub TStripVwDetGrid_Click(sender As System.Object, e As System.EventArgs) Handles TStripVwDetGrid.Click
        Call fillDtlData()
        strSQL = "select h.*, h.Print" & strDocSHR & "NO, d.*"
        strSQL = strSQL & " from " & strHDRTBL & "  h, " & strDTLTBL & " d"
        strSQL = strSQL & " where h." & strDocSHR & "ID = d." & strDocSHR & "ID"
        Call FillViewVsf(Me, strSQL, "", "", intDOCID)
    End Sub

    Private Sub RefreshGrid()
        Dim strSelcols As String, strFldsOrderBy As String
        strSelcols = clsSVT.getDataStringValue("Select SelCols from UtlSelCols " _
                & " where UserCode = '" & gUserCode & "' and formTag= '" & docName & "VIEW" & "'", "", False)

        strSQL = "Select FldsOrderby from UtlSelCols where formTag='" & docName & "VIEW" & "' and UserCode = '" & UCase(gUserCode) & "'"
        strFldsOrderBy = clsSVT.getDataStringValue(strSQL, "", False)
        If Trim(strFldsOrderBy) <> "" Then
            With vsfDet
                Dim strFldCode As String()
                strFldCode = strFldsOrderBy.Split(",")
                For i = 0 To UBound(strFldCode) - 1
                    For j = 0 To .Cols.Count - 1
                        If .GetData(4, j).ToString = strFldCode(i) Then
                            Try
                                .Cols.Move(j, i)
                            Catch
                            End Try
                        End If
                    Next
                Next
            End With
        End If

        Dim intColsVisible As Int32
        If Trim(strSelcols) <> "" Then
            With vsfDet
                For i = 0 To .Cols.Count - 1
                    .Cols(i).Visible = False
                Next
                For i = 0 To .Cols.Count - 1
                    If InStr(1, strSelcols, .GetData(4, i)) > 0 Then
                        .Cols(i).Visible = True
                        intColsVisible = intColsVisible + 1
                    End If
                Next
            End With
        End If
        If intColsVisible = 0 Then
            For i = 0 To vsfDet.Cols.Count - 1
                vsfDet.Cols(i).Visible = True
            Next
        End If



        Call fillColCaptions(intDOCID)
        For i = 0 To vsfDet.Cols.Count - 1
            If Trim(vsfDet.GetData(3, i)) = "" Then
                vsfDet.SetData(3, i, Trim(vsfDet.GetData(4, i)))
            End If
        Next
        Call vsfDet.AutoSizeCols()
        Call syncGridCols()
        intTotRec = Val(vsfDet.Rows.Count) - 5
        lblTotRec.Text = "Total Records: " & Val(Val(vsfDet.Rows.Count) - 5) & "/" & intTotRec
        vsfDet.Rows(0).TextAlignFixed = TextAlignEnum.LeftCenter
        vsfDet.Rows(1).TextAlignFixed = TextAlignEnum.LeftCenter
    End Sub

End Class