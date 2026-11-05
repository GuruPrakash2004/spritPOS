Imports System.Data.oledb
Public Class FrmRptAccGL
    Dim clsSVT As New svtLibrary
    Const vDetDocDate = 0
    Const vDetDocType = 1
    Const vDetDocNo = 2
    Const vDetDocNarration = 3
    Const vDetDebit = 4
    Const vDetCredit = 5
    Const vDetBalance = 6
    Const VDetTotCols = 7

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
            .Cols.Count = VDetTotCols
            .SetData(0, vDetDebit, "Debit")
            .SetData(0, vDetCredit, "Credit")
            .SetData(0, vDetDocNo, "DocNo")
            .SetData(0, vDetDocDate, "DocDate")
            .SetData(0, vDetDocNarration, "Narration")
            .SetData(0, vDetDocType, "DocType")
            .SetData(0, vDetBalance, "Balance")
            .Rows.Count = 1
        End With

        Call SetColWidth(vsfView, 75, 200, 200, 120)
        KeyPreview = True
        initGridStyles(Me)
        Call initGridStyles(Me)
        Call ChangeFontSize(Me)
        Call fillFormProperties(Me)
    End Sub

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Call frmMain.CloseSelectedTabPage(Me)
    End Sub

    Private Sub btnExportToXL_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExportToXL.Click
        Call ExportToXL(vsfView,,,, True, Me.Text, "For the period " & getDateFormatInInsert(dtpFromDate.Value) & " to " & getDateFormatInInsert(dtpTo.Value))
    End Sub

    Private Sub vsfview_BeforeEdit(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfView.BeforeEdit
        e.Cancel = True
    End Sub

    Private Sub btnView_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView.Click
        Dim rs As oledbDataReader
        Dim strSQL As String
        Dim lngAcLedID As Long
        Dim sngCLAmt As Double

        lngAcLedID = clsSVT.getDataLongValue("Select AcLedID from MstAcLed where AcLedName = '" & txtAcLed.Text & "'", 0, False)
        If lngAcLedID = 0 Then
            MsgBox("Please select a valid Ledger!", MsgBoxStyle.Critical, Me.Text)
            Exit Sub
        End If

        strSQL = " Select 'OP' as docType, '" & Format(dtpFromDate.Value, "dd MMM yyyy") & "' as TrnDate,"
        strSQL = strSQL & "'-' as TrnNo,'-' as narrn,"
        strSQL = strSQL & " DAmt = case when sum(DrAmt - crAmt) >= 0 then sum(DrAmt - CrAmt) else 0 end,"
        strSQL = strSQL & " cAmt = case when sum(DrAmt - crAmt) < 0 then sum(DrAmt - CrAmt) else 0 end "
        strSQL = strSQL & " from trnhdracc h, trndtlacc d"
        strSQL = strSQL & " Where AcLedID = " & lngAcLedID
        strSQL = strSQL & " and h.AccID = d.AccID"
        strSQL = strSQL & " and trndate < " & getDateFormatInCond(dtpFromDate.Value)
        strSQL = strSQL & " Union all"
        strSQL = strSQL & " Select doctype, trndate, trnno, narrn,"
        strSQL = strSQL & " DrAmt, CrAmt"
        strSQL = strSQL & " from trnhdracc h, trndtlacc d"
        strSQL = strSQL & " where(h.accid = d.accid)"
        strSQL = strSQL & " and d.acledid = " & lngAcLedID
        strSQL = strSQL & " and trndate between "
        strSQL = strSQL & getDateFormatInCond(dtpFromDate.Value)
        strSQL = strSQL & " and " & getDateFormatInCond(dtpTo.Value)
        strSQL = strSQL & " order by trndate"

        vsfView.Rows.Count = 1
        rs = clsSVT.getDataReaderFromSQL(strSQL)
        Me.Cursor = Cursors.WaitCursor
        With rs
            If .HasRows Then
                Do While .Read
                    vsfView.Rows.Count = vsfView.Rows.Count + 1
                    vsfView.SetData(vsfView.Rows.Count - 1, vDetDocNo, !TrnNo)
                    vsfView.SetData(vsfView.Rows.Count - 1, vDetDocType, !DocType)
                    vsfView.SetData(vsfView.Rows.Count - 1, vDetDocDate, !TrnDate)
                    vsfView.SetData(vsfView.Rows.Count - 1, vDetDebit, Math.Abs(!DAmt))
                    vsfView.SetData(vsfView.Rows.Count - 1, vDetCredit, Math.Abs(!CAmt))
                    sngCLAmt = sngCLAmt + !DAmt + !CAmt
                    vsfView.SetData(vsfView.Rows.Count - 1, vDetBalance, Format(sngCLAmt, AccStringFmt))
                Loop
            End If
            .Close()
        End With

        With vsfView
            .SubtotalPosition = C1.Win.C1FlexGrid.SubtotalPositionEnum.BelowData

            .Subtotal(C1.Win.C1FlexGrid.AggregateEnum.Sum, -1, -1, vDetDebit)
            .Subtotal(C1.Win.C1FlexGrid.AggregateEnum.Sum, -1, -1, vDetCredit)

            .Cols(vDetDebit).Format = "0.00"
            .Cols(vDetCredit).Format = "0.00"
            .Cols(vDetBalance).Format = "0.00"
            .Cols(vDetDebit).TextAlignFixed = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter
            .Cols(vDetCredit).TextAlignFixed = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter
        End With
        Me.Cursor = Cursors.Default
        Call vsfView.AutoSizeCols()
    End Sub

    Private Sub btnSelAcLed_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSelAcLed.Click
        Dim frmUS As New frmUtlSelection, strSQL As String
        With frmUS
            .Tag = " frmRptAccGL "

            strSQL = "Select AcLedName, LedGroupName "
            strSQL = strSQL & " from MstAcLed a, MstLedGroup g"
            strSQL = strSQL & " where AcLedID > 0"
            strSQL = strSQL & " and a.LedGroupID = g.LedGroupID"
            strSQL = strSQL & " order by 1"

            'Call .FillViewVsf(Me, strSQL, txtAcLed.Text, 0)
            .FillViewVsf(Me, False, Nothing, strSQL, txtAcLed, 0)
            Call SetColWidth(.vsfDet, 150)
        End With
        frmUS.ShowDialog()
    End Sub

    Private Sub txtAcLed_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles txtAcLed.KeyPress
        If e.KeyChar = "'" Then
            e.Handled = True
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

    Private Sub vsfView_Click(sender As Object, e As EventArgs) Handles vsfView.Click

        If (Control.ModifierKeys And Keys.Control) = Keys.Control AndAlso (Control.ModifierKeys And Keys.Alt) = Keys.Alt Then
            pnlCopyShortCut.Show()
        End If

    End Sub
End Class