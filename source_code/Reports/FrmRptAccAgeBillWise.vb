Imports System.Data.oledb
Public Class FrmRptAccAgeBillWise
    Dim clsSVT As New svtLibrary

    Public strRcvblPbl As String
    Public strList As String
    Const vDetParty = 0
    Const vDetDocType = 1
    Const vDetDocNo = 2
    Const vDetDocDate = 3
    Const vDetDueDate = 4
    Const vDetAge = 5

    Dim intTotCols As Integer

    'Decl for vsfRange
    Const vRngFrom = 0
    Const vRngTo = 1
    Const vRngDetCol = 2

    Dim strPartyShForm As String
    Dim strPartyType As String
    Dim strPartyName As String
    Dim sngSign As Single

    Dim strPartyGroupList As String
    Dim strPartyList As String = ""

    Private Sub frmRptAccOutstanding_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyUp
        If e.KeyCode = Keys.F9 And e.Control = True Then
            Call initFormSize(Me, Me.Text, True, 958, 579, 0, 0)
            'Call SetColWidthFromString(vsfDet, strDocStdDtlWidth)
            MsgBox("Restored Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
        If e.KeyCode = Keys.F8 And e.Control = True Then
            Call SaveActFormSettings(Me, Me.Text)
            Call SaveActFormVsfDet(Me, Me.Text, "", vsfView)
            MsgBox("Saved Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If

        If e.Alt = True And e.KeyCode = Keys.E Then
            If vsfView.Rows.Count > 1 Then
                Call ExportToXL(vsfView)
            End If
        End If
    End Sub

    Private Sub frmRptAccOutstanding_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        dtpFromDate.Value = getSystemDate()
        With vsfView
            intTotCols = 6
            .Cols.Count = intTotCols
            .SetData(0, vDetParty, "Party")
            .SetData(0, vDetDocNo, "DocNo")
            .SetData(0, vDetDocDate, "DocDate")
            .SetData(0, vDetDueDate, "DueDate")
            .SetData(0, vDetDocType, "DocType")
            .SetData(0, vDetAge, "Age")
            .Rows.Count = 1
        End With
        Call SetColWidth(vsfView, 250, 75, 75, 75, 75, 75)

        With vsfRange
            .Rows.Count = 6
            .Cols.Count = 3
            .SetData(0, vRngFrom, "From")
            .SetData(0, vRngTo, "To")
            .Cols(vRngDetCol).Visible = False
            .Cols(vRngFrom).DataType = GetType(Integer)
            .Cols(vRngTo).DataType = GetType(Integer)

            .SetData(1, vRngFrom, "0")
            .SetData(1, vRngTo, "30")


            .SetData(2, vRngFrom, "31")
            .SetData(2, vRngTo, "60")

            .SetData(3, vRngFrom, "61")
            .SetData(3, vRngTo, "90")

            .SetData(4, vRngFrom, "91")
            .SetData(4, vRngTo, "180")

            .SetData(5, vRngFrom, "180")

        End With

        Call SetColWidth(vsfRange, 80, 80)
        Call SetColAlign(vsfRange, svtAlign.Center, vRngFrom, vRngTo)
        KeyPreview = True
        initGridStyles(Me)
        strRcvblPbl = "R"
        If strRcvblPbl = "R" Then
            strPartyShForm = "Cust"
            strPartyType = "C"
            sngSign = -1
            strPartyName = "Customer"
            Me.Text = "Ageing Report - Receivable"
        Else
            sngSign = 1
            strPartyShForm = "Supp"
            strPartyType = "S"
            strPartyName = "Supplier"
            Me.Text = "Ageing Report - Payable "
        End If
        Call initFormSize(Me, Me.Text, False, 958, 579, 0, 0)
        chkSelCust.Text = "Select " & strPartyName
        Call initGridStyles(Me)
        Call fillFormProperties(Me)
        Call ChangeFontSize(Me)
    End Sub

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Call frmMain.CloseSelectedTabPage(Me)
    End Sub

    Private Sub btnExportToXL_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExportToXL.Click
        Call ExportToXL(vsfView,,,, True, Me.Text, "For the period " & getDateFormatInInsert(dtpFromDate.Value))
    End Sub

    Private Sub vsfview_BeforeEdit(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfView.BeforeEdit
        e.Cancel = True
    End Sub

    Private Sub btnView_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView.Click
        If optDet.Checked Then
            Call showBillWise()
        Else
            Call showSummary()
        End If
    End Sub

    Private Sub showBillWise()
        Dim rs As oledbDataReader, cmd As New oledbCommand
        Dim blnAdd As Boolean, dtDueDate As Date, intAge As Integer
        Dim intCol As Integer
        intTotCols = 6
        vsfView.Cols.Count = 6
        With vsfView
            .Cols.Count = 6
            .SetData(0, vDetParty, "Party")
            .SetData(0, vDetDocNo, "DocNo")
            .SetData(0, vDetDocDate, "DocDate")
            .SetData(0, vDetDueDate, "DueDate")
            .SetData(0, vDetDocType, "DocType")
            .SetData(0, vDetAge, "Age")
            .Rows.Count = 1
        End With
        Call SetColWidth(vsfView, 250, 75, 75, 75, 75, 75)

        With vsfRange
            For i = 1 To .Rows.Count - 1
                vsfView.Cols.Count = vsfView.Cols.Count + 1

                vsfView.SetData(0, vsfView.Cols.Count - 1, .GetData(i, 0) & " to " & .GetData(i, 1))
                If i = .Rows.Count - 1 Then
                    vsfView.SetData(0, vsfView.Cols.Count - 1, ">= " & .GetData(i, 0))
                End If
                .SetData(i, vRngDetCol, vsfView.Cols.Count - 1)
            Next
        End With

        With cmd
            .Connection = svtConn
            .CommandText = "mSP_AcBillWisePendReport"
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add("@AcType", OleDbType.VarChar, 1)
            .Parameters(0).Value = strRcvblPbl
            .Parameters.Add("@AcLedgers", OleDbType.VarChar, -1)
            .Parameters(1).Value = strPartyList
            .CommandTimeout = 0
            rs = .ExecuteReader
        End With

        vsfView.Rows.Count = 1
        Me.Cursor = Cursors.WaitCursor

        If Not rs.HasRows Then
            MsgBox("No Data to load", MsgBoxStyle.Information, Me.Text)
            btnExportToXL.Visible = False
            Me.Cursor = Cursors.Default
            Exit Sub
        End If

        btnExportToXL.Visible = True

        With rs
            If .HasRows Then
                Do While .Read
                    blnAdd = True
                    If Val(txtCr.Text) > 0 Then
                        If Math.Abs(!balAmt) < 0 Then
                            If Math.Abs(!BalAmt) <= Val(txtCr.Text) Then
                                blnAdd = False
                            End If
                        End If
                    End If
                    If Val(txtDr.Text) > 0 Then
                        If Math.Abs(!balAmt) > 0 Then
                            If Math.Abs(!balAmt) <= Val(txtDr.Text) Then
                                blnAdd = False
                            End If
                        End If
                    End If
                    If blnAdd Then
                        vsfView.Rows.Count = vsfView.Rows.Count + 1
                        vsfView.SetData(vsfView.Rows.Count - 1, vDetParty, !AcLedName)
                        vsfView.SetData(vsfView.Rows.Count - 1, vDetDocNo, !TrnNo)
                        vsfView.SetData(vsfView.Rows.Count - 1, vDetDocType, !DocType)
                        vsfView.SetData(vsfView.Rows.Count - 1, vDetDueDate, !DueDate)
                        vsfView.SetData(vsfView.Rows.Count - 1, vDetDocDate, !TrnDate)
                        dtDueDate = !DueDate
                        intAge = DateDiff(DateInterval.Day, dtDueDate, getSystemDate)
                        vsfView.SetData(vsfView.Rows.Count - 1, vDetAge, intAge)
                        For i = 1 To vsfRange.Rows.Count - 1
                            If i <> vsfRange.Rows.Count - 1 Then
                                If intAge >= Val(vsfRange.GetData(i, 0)) And intAge <= Val(vsfRange.GetData(i, 1)) Then
                                    intCol = Val(vsfRange.GetData(i, vRngDetCol))
                                    vsfView.SetData(vsfView.Rows.Count - 1, intCol, Math.Abs(!balamt))
                                End If
                            Else
                                If intAge >= Val(vsfRange.GetData(i, 0)) Then
                                    intCol = Val(vsfRange.GetData(i, vRngDetCol))
                                    vsfView.SetData(vsfView.Rows.Count - 1, intCol, Math.Abs(!balamt))
                                End If

                            End If
                        Next
                    End If
                Loop
            End If
        End With

        With vsfView
            .AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.RestrictAll
            .Cols(vDetParty).AllowMerging = True

            .SubtotalPosition = C1.Win.C1FlexGrid.SubtotalPositionEnum.BelowData
            For i = intTotCols To .Cols.Count - 1
                .Subtotal(C1.Win.C1FlexGrid.AggregateEnum.Sum, -1, -1, i)
                .Subtotal(C1.Win.C1FlexGrid.AggregateEnum.Sum, -1, -1, i)

                .Cols(i).Format = "0.00"
                .Cols(i).TextAlignFixed = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter
            Next i
        End With
        Me.Cursor = Cursors.Default
        MsgBox("Data loaded successfully", MsgBoxStyle.Information, Me.Text)
        btnExportToXL.Visible = True
    End Sub

    Private Sub chkSelCust_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkSelCust.CheckedChanged
        Dim strSQL As String
        If chkSelCust.Checked = False Then
            strList = ""
            strPartyList = ""
            Exit Sub
        End If

        Dim frmSel As New frmUtlMultiSelection

        strSQL = "Select distinct 0 as sel,AcLedID,AcLedName from MstAcLed"
        strSQL = strSQL & " where AcLedID > 0 and "
        If strRcvblPbl = "R" Then
            strSQL = strSQL & "LedType = 'C'"
        Else
            strSQL = strSQL & "LedType = 'S'"
        End If
        strSQL = strSQL & " and InActive = 'N' order by AcLedName"
        Call frmSel.InitVsfSel(strSQL, 1, 1, True, Me, False)
        frmSel.Text = "Select [Multiple] " & strPartyName & ""
        frmSel.vsfDet.SetData(0, 0, "Sel")
        frmSel.ShowDialog()
        strPartyList = strList

        If Trim(strPartyList) = "" Then chkSelCust.Checked = False
    End Sub

    Private Sub vsfRange_AfterEdit(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfRange.AfterEdit
        Dim i As Integer, intTONo As Long
        With vsfRange
            If e.Col = vRngTo Then
                If Trim(.GetData(e.Row, vRngTo)) = 0 Then
                    For i = e.Row + 1 To .Rows.Count - 1
                        .SetData(i, 1, "")
                        .SetData(i, 0, "")
                    Next i
                    Exit Sub
                End If

                intTONo = Val(.GetData(e.Row, 1)) + 1

                If e.Row < vsfRange.Rows.Count - 1 Then
                    .SetData(e.Row + 1, 0, intTONo)
                End If
            End If
        End With
    End Sub

    Private Sub vsfRange_BeforeEdit(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfRange.BeforeEdit
        Select Case e.Col
            Case vRngFrom
                If e.Row > 1 Then
                    e.Cancel = True
                End If
        End Select
    End Sub

    Private Sub vsfRange_EnterCell(ByVal sender As Object, ByVal e As System.EventArgs) Handles vsfRange.EnterCell
        With vsfRange
            If vsfRange.Rows.Count <= 1 OrElse vsfRange.Row <= 1 Then Exit Sub
            If .Col = vRngTo Then
                .Focus()
                .StartEditing()
            End If
        End With
    End Sub

    Private Sub vsfRange_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles vsfRange.GotFocus
        With vsfRange
            If .GetData(1, vRngFrom) = 0 Then
                .Row = 1
                .Col = vRngFrom
                .Focus()
                .StartEditing()
            End If
        End With
    End Sub

    Private Sub vsfRange_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles vsfRange.KeyUp
        Dim intRow As Integer, intTONo As Long
        With vsfRange
            If e.KeyCode = Keys.Tab And .Row = .Rows.Count - 1 And .ColSel = vRngTo Then
                intTONo = Val(.GetData(.Row, vRngTo)) + 1
                intRow = .Rows.Count
                .Rows.Count = .Rows.Count + 1
                .Col = vRngTo
                .Row = intRow
                .SetData(.Row, vRngFrom, intTONo)
                .LeftCol = vRngTo
                .Focus()
            Else
                .Col = vRngTo
                .Focus()
            End If
        End With
    End Sub

    Private Sub vsfRange_LeaveCell(ByVal sender As Object, ByVal e As System.EventArgs) Handles vsfRange.LeaveCell
        With vsfRange
            If vsfRange.Rows.Count <= 1 OrElse vsfRange.Row <= 1 Then Exit Sub
            If .Col = vRngTo And Trim(.GetData(vsfRange.Row, vRngTo)) = "" Then
                btnView.Focus()
                Exit Sub
            End If
        End With
    End Sub

    Private Sub vsfRange_ValidateEdit(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.ValidateEditEventArgs) Handles vsfRange.ValidateEdit
        If e.Col < 1 Then Exit Sub
        With vsfRange
            If Trim(.Editor.Text) = "" Then Exit Sub
            If Val(.Editor.Text) < Val(.GetData(e.Row, 0)) Then
                MsgBox(" Invalid Range! Check Entry...", MsgBoxStyle.Information + vbOKOnly, Me.Text)
                .StartEditing()
                e.Cancel = True
                Exit Sub
            End If
        End With
    End Sub

    Private Sub btnClear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClear.Click
        vsfRange.Rows.Count = 1
        vsfRange.Rows.Count = 2
    End Sub

    Private Sub showSummary()
        Dim rs As OleDbDataReader, cmd As New OleDbCommand
        Dim blnAdd As Boolean, dtDueDate As Date, intAge As Integer
        Dim intCol As Integer, blnFound As Boolean, intRow As Integer
        intTotCols = 1
        vsfView.Cols.Count = 1
        With vsfView
            .Cols.Count = 1
            .SetData(0, vDetParty, "Party")
            .Rows.Count = 1
        End With
        Call SetColWidth(vsfView, 250)

        With vsfRange
            For i = 1 To .Rows.Count - 1
                vsfView.Cols.Count = vsfView.Cols.Count + 1

                vsfView.SetData(0, vsfView.Cols.Count - 1, .GetData(i, 0) & " to " & .GetData(i, 1))
                If i = .Rows.Count - 1 Then
                    vsfView.SetData(0, vsfView.Cols.Count - 1, ">= " & .GetData(i, 0))
                End If
                .SetData(i, vRngDetCol, vsfView.Cols.Count - 1)
            Next
        End With

        With cmd
            .Connection = svtConn
            .CommandText = "mSP_AcBillWisePendReport"
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add("@AcType", OleDbType.VarChar, 1).Value = strRcvblPbl
            .Parameters.Add("@AcLedgers", OleDbType.VarChar, -1).Value = strPartyList
            .CommandTimeout = 0
            rs = .ExecuteReader
        End With
        vsfView.Rows.Count = 1
        Me.Cursor = Cursors.WaitCursor
        With rs
            If .HasRows Then
                Do While .Read
                    blnAdd = True
                    If Val(txtCr.Text) > 0 Then
                        If Math.Abs(!balAmt) < 0 Then
                            If Math.Abs(!BalAmt) <= Val(txtCr.Text) Then
                                blnAdd = False
                            End If
                        End If
                    End If
                    If Val(txtDr.Text) > 0 Then
                        If Math.Abs(!balAmt) > 0 Then
                            If Math.Abs(!balAmt) <= Val(txtDr.Text) Then
                                blnAdd = False
                            End If
                        End If
                    End If
                    If blnAdd Then
                        blnFound = False
                        For i = 1 To vsfView.Rows.Count - 1
                            If vsfView.GetData(vsfView.Rows.Count - 1, vDetParty) = !AcLedName Then
                                blnFound = True
                                intRow = i
                                Exit For
                            End If
                        Next
                        If Not blnFound Then
                            vsfView.Rows.Count = vsfView.Rows.Count + 1
                            intRow = vsfView.Rows.Count - 1
                            vsfView.SetData(vsfView.Rows.Count - 1, vDetParty, !AcLedName)
                        End If
                        dtDueDate = !DueDate
                        intAge = DateDiff(DateInterval.Day, dtDueDate, getSystemDate)
                        For i = 1 To vsfRange.Rows.Count - 1
                            If i <> vsfRange.Rows.Count - 1 Then
                                If intAge >= Val(vsfRange.GetData(i, 0)) And intAge <= Val(vsfRange.GetData(i, 1)) Then
                                    intCol = Val(vsfRange.GetData(i, vRngDetCol))
                                    vsfView.SetData(intRow, intCol, Val(vsfView.GetData(intRow, intCol)) + Math.Abs(!balamt))
                                End If
                            Else
                                If intAge >= Val(vsfRange.GetData(i, 0)) Then
                                    intCol = Val(vsfRange.GetData(i, vRngDetCol))
                                    vsfView.SetData(intRow, intCol, Val(vsfView.GetData(intRow, intCol)) + Math.Abs(!balamt))
                                End If
                            End If
                        Next
                    End If
                Loop
            End If
        End With

        With vsfView
            .AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.RestrictAll
            .Cols(vDetParty).AllowMerging = True

            .SubtotalPosition = C1.Win.C1FlexGrid.SubtotalPositionEnum.BelowData
            For i = intTotCols To .Cols.Count - 1
                .Subtotal(C1.Win.C1FlexGrid.AggregateEnum.Sum, -1, -1, i)
                .Subtotal(C1.Win.C1FlexGrid.AggregateEnum.Sum, -1, -1, i)

                .Cols(i).Format = "0.00"
                .Cols(i).TextAlignFixed = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter
            Next i
        End With
        Me.Cursor = Cursors.Default
    End Sub

    Private Sub txtCr_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles txtCr.KeyPress, txtDr.KeyPress
        If e.KeyChar = "'" Then
            e.Handled = True
        End If
    End Sub

    Private Sub optReceivable_CheckedChanged(sender As Object, e As EventArgs) Handles optReceivable.CheckedChanged
        If optReceivable.Checked Then
            chkSelCust.Checked = False
            chkSelCust.Text = "Select Customer"
            strRcvblPbl = "R"
            strPartyShForm = "Cust"
            strPartyType = "C"
            sngSign = -1
            strPartyName = "Customer"
        End If
    End Sub

    Private Sub optPayable_CheckedChanged(sender As Object, e As EventArgs) Handles optPayable.CheckedChanged
        If optPayable.Checked Then
            chkSelCust.Checked = False
            chkSelCust.Text = "Select Supplier"
            strRcvblPbl = "P"
            sngSign = 1
            strPartyShForm = "Supp"
            strPartyType = "S"
            strPartyName = "Supplier"
        End If
    End Sub

    Private Sub pnlbtnClose_Click(sender As Object, e As EventArgs) Handles pnlbtnClose.Click
        pnlCopyShortCut.Visible = False
    End Sub

    Private Sub btnCopy_Click(sender As Object, e As EventArgs) Handles btnCopy.Click

        If RbnCopyCell.Checked = True Then
            CopyGrid(vsfView, "Cell")
        ElseIf RbnCopyColumn.Checked = True Then

            CopyGrid(vsfView, "Column")

        ElseIf RbnCopyRow.Checked = True Then

            CopyGrid(vsfView, "Row")
        ElseIf RbnCopyGrid.Checked = True Then

            CopyGrid(vsfView, "Full Grid")

        End If

        pnlCopyShortCut.Visible = False
    End Sub

    Private Sub vsfDet_Click(sender As Object, e As EventArgs) Handles vsfView.Click

        If (Control.ModifierKeys And Keys.Control) = Keys.Control AndAlso (Control.ModifierKeys And Keys.Alt) = Keys.Alt Then
            pnlCopyShortCut.Show()
        End If

    End Sub
End Class