Imports System.Data.oledb
Public Class frmRptStockLed
    Dim clsSVT As New svtLibrary
    Dim strValMethod As String

    Const vVwItem = 0
    Const vVwDocType = 1
    Const vVwDocNo = 2
    Const vVwDocDate = 3
    Const vVwParticulars = 4
    Const vVwQtyRcpt = 5
    Const vVwQtyIss = 6
    Const vVwRate = 7

    Public strList As String
    Dim strItemGroupList As String

    Public intCount As Long

    Dim arrRctps(4, 1) As String

    Public argType As String, strSQL As String, blnBatchPresent As Boolean

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        frmMain.CloseSelectedTabPage(Me)
    End Sub

    Private Function ValidateFlds() As Boolean
        ValidateFlds = False
        If dtpFromDate.Value < CDate(gSettings.gCompStDt) Then
            MsgBox("From date cannot be less than " & gSettings.gCompStDt, MsgBoxStyle.Critical, Me.Text)
            Exit Function
        End If
        If dtpToDate.Value > CDate(gSettings.gCompEndDt) Then
            MsgBox("To date cannot be greater than " & gSettings.gCompEndDt, MsgBoxStyle.Critical, Me.Text)
            Exit Function
        End If
        'check the date range
        If CDate(dtpToDate.Value) < CDate(dtpFromDate.Value) Then
            MsgBox("Invalid Date Range!", MsgBoxStyle.Critical, Me.Text)
            Exit Function
        End If
        'If cboItemCode.Text = "" Then
        '    MsgBox("Itemcode has to be selected!", MsgBoxStyle.Critical, Me.Text)
        '    cboItemCode.Focus()
        '    Exit Function
        'End If
        ValidateFlds = True
    End Function

    Private Sub btnView_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView.Click
        Dim rsDummy As oledbDataReader
        Dim intItemID As Integer
        intItemID = clsSVT.getDataLongValue("Select ItemID from mstItem where ItemCode = '" & txtItemCode.Text & "'", 0, False)
        If Not ValidateFlds() Then
            Exit Sub
        End If
        Dim strsql As String
        strsql = "Select op.itemid, ItemGroupName, ItemName,'.OP' as docType, 'OP' as docNo, "
        strsql = strsql & getDateFormatInCond(dtpFromDate.Value) & " as DocDate, "
        strsql = strsql & " '' as Particulars, sum(Qty*StkMultiplier) as rcpt, 0 as Iss,0 as Rate"
        strsql = strsql & " from UtlStockList op, MstItem i, MstItemGroup g "
        strsql = strsql & " where op.ItemID = i.ItemID And i.ItemGroupID = g.ItemGroupId  "
        strsql = strsql & " and op.ItemID = " & intItemID
        strsql = strsql & " And op.DocDate < " & getDateFormatInCond(dtpFromDate.Value)
        strsql = strsql & " Group by op.itemid, ItemGroupName, ItemName"
        strsql = strsql & " having sum(Qty * StkMultiplier) <> 0"
        strsql = strsql & " Union all "
        strsql = strsql & " Select s.itemid, ItemGroupName, ItemName,docType, DocNo,DocDate, Particulars, "
        strsql = strsql & " case when Stkmultiplier = 1 then Qty else 0 end, "
        strsql = strsql & " case when Stkmultiplier = 1 then 0 else Qty end, Rate"
        strsql = strsql & " from UtlStockList s, MstItem i, MstItemGroup g "
        strsql = strsql & " where s.ItemID = i.ItemID And i.ItemGroupID = g.ItemGroupId  "
        strsql = strsql & " and s.ItemID = " & intItemID
        strsql = strsql & " and s.DocDate >= " & getDateFormatInCond(dtpFromDate.Value)
        strsql = strsql & " and s.DocDate <= " & getDateFormatInCond(dtpToDate.Value)

        strsql = strsql & " order by ItemGroupName, 1, docdate,docType"
        vsfDet.Rows.Count = 1
        rsDummy = clsSVT.getDataReaderFromSQL(strsql)
        With rsDummy
            Do While .Read
                vsfDet.Rows.Count = vsfDet.Rows.Count + 1
                vsfDet.SetData(vsfDet.Rows.Count - 1, vVwDocType, !DocType)
                vsfDet.SetData(vsfDet.Rows.Count - 1, vVwDocNo, !DocNo)
                vsfDet.SetData(vsfDet.Rows.Count - 1, vVwDocDate, Format(!DocDate, "dd/MM/yyyy"))
                vsfDet.SetData(vsfDet.Rows.Count - 1, vVwParticulars, !Particulars)
                vsfDet.SetData(vsfDet.Rows.Count - 1, vVwItem, !ItemName)
                vsfDet.SetData(vsfDet.Rows.Count - 1, vVwQtyRcpt, Math.Abs(!Rcpt))
                vsfDet.SetData(vsfDet.Rows.Count - 1, vVwQtyIss, Math.Abs(!Iss))
                vsfDet.SetData(vsfDet.Rows.Count - 1, vVwRate, GetVal(!Rate))
            Loop
            .Close()
        End With
        With vsfDet
            .SubtotalPosition = C1.Win.C1FlexGrid.SubtotalPositionEnum.BelowData
            For k = 5 To 6
                .Subtotal(C1.Win.C1FlexGrid.AggregateEnum.Sum, 0, -1, k, "0.00")
            Next k
            'If Condition added by surabhi
            If .Rows.Count > 2 Then
                .SetData(.Rows.Count - 1, .Cols.Count - 1, Val(.GetData(.Rows.Count - 1, vVwQtyRcpt)) - Val(.GetData(.Rows.Count - 1, vVwQtyIss)))
                setVsfCellColour(vsfDet, .Rows.Count - 1, .Cols.Count - 1, Color.Yellow, Color.Black, True)
            End If
            '.MergeCells = flexMergeRestrictAll
            '.MergeCol(0) = True
            '.MergeCol(1) = True
        End With
        'Call SetColWidth(vsfDet, 1830, 1425, 555, 1125, 1320, 1050, 3255, 1200, 675, 990, 1200, 1200, 1215)
    End Sub

    Private Sub frmRptStockLed_KeyUp(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyUp
        If e.KeyCode = Keys.F9 And e.Control = True Then
            '958, 579
            Call initFormSize(Me, "RptStkDetails", True, 958, 579, 0, 0)
            'Call SetColWidthFromString(vsfDet, strDocStdDtlWidth)
            MsgBox("Restored Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
        If e.KeyCode = Keys.F8 And e.Control = True Then
            Call SaveActFormSettings(Me, "RptStkDetails")
            Call SaveActFormVsfDet(Me, "RptStkDetails", "", vsfDet)
            'Call SaveItemComboFrameSettings(fraSearch, vsfView,  intDocID)
            MsgBox("Saved Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
        If e.Control = False And e.Alt = True And e.KeyCode = Keys.E Then
            Call ExportToXL(vsfDet)
        End If
    End Sub

    Private Sub frmRptStockBal_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Call initScreen()
        Call fillFormProperties(Me)
        Call ChangeFontSize(Me)
    End Sub

    Private Sub initScreen()
        dtpFromDate.Value = gSettings.gCompStDt
        dtpToDate.Value = getSystemDate()

        initGridStyles(Me)

        With vsfDet
            .Cols.Count = 8
            .Rows.Count = 1
            .SetData(0, vVwItem, "Item")
            .SetData(0, vVwDocType, "Doc Type")
            .SetData(0, vVwDocNo, "Doc No")
            .SetData(0, vVwDocDate, "Doc Date")
            .SetData(0, vVwParticulars, "Particulars")
            .SetData(0, vVwQtyRcpt, "Receipts")
            .SetData(0, vVwQtyIss, "Issues")
            .SetData(0, vVwRate, "Rate")
        End With

        Call bindCombo(cboItemCode, "itemID", "ItemCode", "Select itemid, itemcode from mstitem where Inactive = 'N'  order by itemcode")
        cboItemCode.Text = ""

        Dim ctrl As Control = Nothing
        Call initFormSize(Me, "RptStkLed", False, 958, 579, 0, 0)
        Call initFormVsfDet(Me, "RptStkLed", vsfDet, Me.Text)
        KeyPreview = True
    End Sub

    Private Sub btnExportToXL_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExportToXL.Click
        Call ExportToXL(vsfDet,,,, True, Me.Text, "For the period " & getDateFormatInInsert(dtpFromDate.Value) & " to " & getDateFormatInInsert(dtpToDate.Value))
    End Sub

    Private Sub vsfDet_BeforeEdit(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfDet.BeforeEdit
        e.Cancel = True
    End Sub

    Private Sub txtItemCode_KeyUp(sender As Object, e As KeyEventArgs) Handles txtItemCode.KeyUp
        If e.KeyCode = Keys.Down Then
            Call showItemPopup()
            txtItemCode.Text = txtDummy2.Text
        End If
    End Sub

    Private Sub showItemPopup()
        Dim strSQL As String

        strSQL = "Select ItemID, ItemCode, ItemName, ShortName, HotKey"
        strSQL &= " From MstItem "
        strSQL &= " order by 1"

        txtDummy.Text = ""
        txtDummy2.Text = ""

        Dim objfrm As New frmUtlSelection

        With objfrm
            .blnMultiColSearch = True
            .FillViewVsf(Me, False, Nothing, strSQL, txtDummy, 0, txtDummy2, 1)
            .Text = " Please Select Item"
            .Tag = " frmRptStockLed "
            .ShowDialog()
            .Close()
        End With

    End Sub



    Private Sub pnlbtnClose_Click(sender As Object, e As EventArgs) Handles pnlbtnClose.Click
        pnlCopyShortCut.Visible = False
    End Sub

    Private Sub btnCopy_Click(sender As Object, e As EventArgs) Handles btnCopy.Click

        If RbnCopyCell.Checked = True Then
            CopyGrid(vsfDet, "Cell")
        ElseIf RbnCopyColumn.Checked = True Then

            CopyGrid(vsfDet, "Column")

        ElseIf RbnCopyRow.Checked = True Then

            CopyGrid(vsfDet, "Row")
        ElseIf RbnCopyGrid.Checked = True Then

            CopyGrid(vsfDet, "Full Grid")
        End If

        pnlCopyShortCut.Visible = False
    End Sub

    Private Sub vsfDet_Click(sender As Object, e As EventArgs) Handles vsfDet.Click

        If (Control.ModifierKeys And Keys.Control) = Keys.Control AndAlso (Control.ModifierKeys And Keys.Alt) = Keys.Alt Then
            pnlCopyShortCut.Show()
        End If
    End Sub
End Class