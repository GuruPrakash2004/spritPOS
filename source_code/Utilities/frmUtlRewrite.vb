Imports C1.Win.C1FlexGrid

Public Class frmUtlRewrite
    Public strList As String
    Dim strCategoryList As String
    Dim strCustList As String
    Dim StrPORefList As String
    Dim strSuppList As String
    Dim clsSVT As New svtLibrary

    Dim dtsearch As DataTable
    Dim dvsearch As DataView
    'vdet
    Const vDetSlno = 0
    Const vDetItemID = 1
    Const vDetItemCode = 2
    Const vDetItemName = 3
    Const vDetBQty = 4
    Const vDetDQty = 5
    Const vDetPQty = 6
    Const vDetSQty = 7
    Const intDetCols = 8

    Dim intCompID As Int32 = 0

    'view
    Const vVItemID = 0
    Const vVCustID = 1
    Const vVDocType = 2
    Const vVDocID = 3
    Const vVDocDate = 4
    Const vVDocQty = 5
    Const vVRate = 6
    Const vVCumQty = 7
    Const vVPendQty = 8
    Const vVCompID = 9
    Const intViewCols = 10

    Dim StrRights As String, blnProcess As Boolean = False, blnItemLoad As Boolean = False

    Private Sub btnClose_Click(sender As System.Object, e As System.EventArgs) Handles btnClose.Click
        Dim tb As New C1.Win.C1Command.C1DockingTabPage()
        Dim dockTabMain As New C1.Win.C1Command.C1DockingTab()
        Me.Dispose()
        tb.Controls.Clear()
        'frmMain.dockTabMain.SelectedTab.Dispose()
    End Sub

    Private Sub btnView_Click(sender As System.Object, e As System.EventArgs) Handles btnView.Click
        Dim strSQL As String = ""
        With vsfDet
            .Rows.Count = 1
            .Cols.Count = intDetCols - 2
        End With


        lblCurItem.Text = ""
        lblItemStk.Text = ""
        lblTotItems.Text = ""
        lblTotStkItems.Text = ""

        'pnlDet.Visible = True
        blnProcess = True
        blnItemLoad = False
        Call showWaitCursor()


        'BackgroundWorker1.RunWorkerAsync()
        Call LoadData()
    End Sub

    Private Sub LoadData()
        Dim strSQL As String = ""

        'strSQL = "Truncate table RptStkBal;" & vbNewLine
        'strSQL &= "Truncate table TrnQOH;" & vbNewLine
        'clsSVT.execSQLStatement(strSQL)

        'strSQL = "Exec mSp_StkBal '" & gUserCode & "', '" & Format(Date.Today, "dd MMM yyyy") & "'"
        'clsSVT.execSQLStatement(strSQL)

        'strSQL = " Insert into TrnQOH (ItemID,CustID, BQOH, DQOH, PQOH)  "
        'strSQL &= " Select ItemID, CustID, Sum(BQOH), Sum(DQOH), Sum(PQOH)   from (  "
        'strSQL &= " Select i.ItemID,  custid, case compid when 1 then Sum(DocQty) else 0 end as bQoH,"
        'strSQL &= " case compid when 2 then Sum(DocQty) else 0 end as dQoH,"
        'strSQL &= " case compid when 3 then Sum(DocQty) else 0 end as pQoH"
        'strSQL &= " from MstItem i , RptStkBal r "
        'strSQL &= " where i.ItemID=r.ItemID "
        'strSQL &= " and userName = '" & gUserCode & "'"
        'strSQL &= " Group by i.ItemID, custid,compid"
        'strSQL &= " having sum(docqty) > 0"
        'strSQL &= " ) a Group by ItemID,custid  "

        strSQL = " EXEC mSp_ReWriteStock"

        clsSVT.execSQLStatement(strSQL)

        strSQL = "Select row_number() over(order by itemCode) Slno, i.ItemID, ItemCode, ItemName "
        strSQL &= " From MstItem i, TrnQOH q, MstLoc l where i.ItemID = q.itemID "
        'strSQL = "Select row_number() over(order by itemCode) Slno, i.ItemID, ItemCode, ItemName,CustCode, BQOH as QOH "
        'strSQL &= " From MstItem i, TrnQOH q, MstCust c where i.ItemID = q.itemID "
        'strSQL &= " and c.CustID = q.CustID"
        'strSQL &= " And BQOH + DQOH + PQOH > 0 "
        strSQL &= " Order by ItemCode "
        dtsearch = clsSVT.getDATATable(strSQL)

        dvsearch = New DataView(dtsearch)

        lblTotItems.Text = clsSVT.getDataLongValue("Select count(*) from TrnQOH", 0, False)
        lblTotStkItems.Text = dtsearch.Rows.Count - 1
        vsfDet.BeginUpdate()
        vsfDet.DataSource = dvsearch
        vsfDet.EndUpdate()
        Call SetHideCol(vsfDet, True, vDetItemID)
        vsfSearch.Cols.Count = vsfDet.Cols.Count
        Call SetHideCol(vsfSearch, True, vDetItemID)
        Call SetColWidth(vsfDet, 100, 25, 150, 150, 100)
        syncGridCols()
        MsgBox("Re-Written Stock Table", MsgBoxStyle.Information, "Stock update")
    End Sub


    Private Sub frmRptPendCPO_KeyUp(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyUp
        If e.KeyCode = Keys.F9 And e.Control Then
            '958, 579
            Call initFormSize(Me, Me.Text, True, 958, 579, 0, 0)
            'Call SetColWidthFromString(vsfDet, strDocStdDtlWidth)
            MsgBox("Restored Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
        If e.KeyCode = Keys.F8 And e.Control = True Then
            Call SaveActFormSettings(Me, Me.Text)
            Call SaveActFormVsfDet(Me, Me.Text, "REPORT", vsfDet)
            'Call SaveItemComboFrameSettings(fraSearch, vsfView,  intDocID)
            MsgBox("Saved Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
    End Sub

    Private Sub frmRptPendCPO_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        Call initFormSize(Me, Me.Text, False, 958, 579, 0, 0)
    End Sub

    Private Sub vsfView_AfterScroll(sender As Object, e As C1.Win.C1FlexGrid.RangeEventArgs) Handles vsfDet.AfterScroll
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

    Private Sub vsfSearch_KeyUpEdit(sender As Object, e As C1.Win.C1FlexGrid.KeyEditEventArgs) Handles vsfSearch.KeyUpEdit
        If e.KeyCode = Keys.Down Then
            vsfDet.Row = 1
            vsfDet.Col = vsfSearch.Col
            vsfDet.Focus()
        End If
    End Sub

    Private Function applyFilterFromGrid() As Boolean
        Dim StrQry As String, strValue As String
        Dim i As Integer

        applyFilterFromGrid = False
        StrQry = ""
        Try


            With vsfSearch
                For i = 0 To .Cols.Count - 1
                    strValue = ""
                    If i = .ColSel Then

                        If Not .Editor Is Nothing AndAlso .Editor.Text <> "" Then
                            strValue = Trim(.Editor.Text)
                        End If
                    Else
                        If .GetData(0, i) <> "" Then
                            strValue = Trim(.GetData(0, i))
                        End If
                    End If
                    If strValue <> "" Then
                        If StrQry <> "" Then StrQry = StrQry & " And "
                        Select Case gDBTypeCurrent
                            Case gDbTypeAccess
                                StrQry = StrQry & vsfDet.GetData(0, i) & " Like '*" & strValue & "*'"
                            Case Else

                                If vsfDet.Cols(i).DataType = GetType(Decimal) Or vsfDet.Cols(i).DataType = GetType(Integer) Or vsfDet.Cols(i).DataType = GetType(Date) Then
                                    Try
                                        StrQry = StrQry & "Convert(" & vsfDet.GetData(0, i) & ",'System.String')" & " like '" & strValue & "*'"
                                    Catch ex As Exception
                                        dvsearch.RowFilter = "1 <> 1"
                                        Exit Function
                                    End Try
                                Else
                                    StrQry = StrQry & vsfDet.GetData(0, i) & " like '%" & strValue & "%'"
                                End If
                        End Select
                    End If
                Next i
            End With

            Dim k As Integer = vsfDet.Rows.Count

            dvsearch.RowFilter = StrQry
            Call syncGridCols()
            applyFilterFromGrid = True
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

    End Function

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

    Private Sub vsfDet_BeforeEdit(sender As Object, e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfDet.BeforeEdit
        e.Cancel = True
    End Sub

    Private Sub vsfDet_DoubleClick(sender As Object, e As System.EventArgs) Handles vsfDet.DoubleClick
        If vsfDet.Rows.Count <= 1 Then Exit Sub
        'If Val(vsfDet.GetData(vsfDet.Row, vDPMTID)) <= 0 Then Exit Sub
        'FillData(Val(vsfDet.GetData(vsfDet.Row, vDPMTID)))

    End Sub

    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles btnCancel.Click
        pnlDet.Visible = False
    End Sub

    Private Sub vsfView_BeforeEdit(sender As Object, e As RowColEventArgs)
        '  If Not (e.Col = vVClaimedOn Or e.Col = vVIsClaimed) Then
        e.Cancel = True
        '  End If
    End Sub


    Private Sub btnClear_Click(sender As Object, e As EventArgs) Handles btnClear.Click
        If GetVal(vsfDet.GetData(0, 0), "") = "" Then Exit Sub
        With vsfSearch
            For i As Int32 = 0 To .Cols.Count - 1
                .SetData(0, i, "")
            Next
            .Update()
        End With
        applyFilterFromGrid()
    End Sub
End Class