Public Class frmTrnRemoveDuplicates
    Dim clsSVT As New svtLibrary
    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        frmMain.CloseSelectedTabPage(Me)
    End Sub

    Private Sub btnRem_Click(sender As Object, e As EventArgs) Handles btnRem.Click
        Dim strSQL As String

        strSQL = "Select i.ItemID, ItemName, Itemcode "
        strSQL &= " From MstItem i"
        strSQL &= " order by 1"

        txtRemove.Text = ""
        Dim objfrm As New frmUtlSelection

        With objfrm
            .blnMultiColSearch = True
            .FillViewVsf(Me, False, Nothing, strSQL, txtRemove, 1)
            .Text = "Please Select Item"
            .Tag = ""
            .ShowDialog()
            .Close()
        End With
    End Sub

    Private Sub btnRepl_Click(sender As Object, e As EventArgs) Handles btnRepl.Click
        Dim strSQL As String

        strSQL = "Select i.ItemID, ItemName, Itemcode "
        strSQL &= " From MstItem i"
        strSQL &= " order by 1"

        txtReplace.Text = ""
        Dim objfrm As New frmUtlSelection

        With objfrm
            .blnMultiColSearch = True
            .FillViewVsf(Me, False, Nothing, strSQL, txtReplace, 1)
            .Text = "Please Select Item"
            .Tag = ""
            .ShowDialog()
            .Close()
        End With
    End Sub

    Private Sub btnUpdate_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click
        Dim intRemItemID As Long, intReplItemID As Long
        intRemItemID = clsSVT.getDataLongValue("Select ItemID from mstitem where itemname = '" & txtRemove.Text & "'", 0, False)
        intReplItemID = clsSVT.getDataLongValue("Select ItemID from mstitem where itemname = '" & txtReplace.Text & "'", 0, False)
        Dim svtTrn As oledb.oledbTransaction
        svtTrn = svtConn.BeginTransaction(IsolationLevel.ReadCommitted)

        If Not clsSVT.execSQLStatement("update TrnDtlSINV set itemid = " & intReplItemID & " where itemid = " & intRemItemID, svtTrn) Then GoTo errHandler
        Call clsSVT.execSQLStatement("update TrnDtlPB set itemid = " & intReplItemID & " where itemid = " & intRemItemID, svtTrn)
        Call clsSVT.execSQLStatement("update TrnDtlSTKDED set itemid = " & intReplItemID & " where itemid = " & intRemItemID, svtTrn)
        Call clsSVT.execSQLStatement("update TrnDtlPI set itemid = " & intReplItemID & " where itemid = " & intRemItemID, svtTrn)
        Call clsSVT.execSQLStatement("update TrnDtlSR set itemid = " & intReplItemID & " where itemid = " & intRemItemID, svtTrn)
        Call clsSVT.execSQLStatement("update TrnDtlQTN set itemid = " & intReplItemID & " where itemid = " & intRemItemID, svtTrn)
        Call clsSVT.execSQLStatement("Delete from mstitem where itemid = " & intRemItemID, svtTrn)
        Call clsSVT.execSQLStatement("Update UtlStockList set itemid = " & intReplItemID & " where itemid = " & intRemItemID, svtTrn)

        Dim dblQOH As Double = clsSVT.getDataDoubleValue("Select Sum(CLBal) From TrnQOH Where ItemID in (" & Val(intReplItemID) & "," & Val(intRemItemID) & ")", 0, False, svtTrn)

        clsSVT.execSQLStatement("Delete from TrnQOH Where itemID in (" & Val(intReplItemID) & "," & Val(intRemItemID) & ")", svtTrn)

        clsSVT.execSQLStatement("Insert Into TrnQOH (ItemID, LocID, OPQty, TrnQty, CLBal) values (" & Val(intReplItemID) & ", 1, 0, " & dblQOH & ", " & dblQOH & ")", svtTrn)
        svtTrn.Commit()
        MsgBox("Item replaced and removed successfully", MsgBoxStyle.Information, Me.Text)
        txtRemove.Text = ""
        txtReplace.Text = ""
        Exit Sub

errHandler:
        svtTrn.Rollback()
        MsgBox("Error during updation", MsgBoxStyle.Critical, Me.Text)
    End Sub
End Class