Public Class frmUtlMergeItems
    Dim clsSvt As New svtLibrary
    Private Sub frmUtlMergeItems_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub btnMergeItm_Click(sender As Object, e As EventArgs) Handles btnMergeItm.Click
        txtMItemId.Text = ""
        Dim strSQL As String = ""

        strSQL = "Select ItemID, ItemCode, ItemName "
        strSQL &= "from MstItem where InActive = 'N'"
        strSQL &= "order by itemcode"

        Dim frmDisplay As New frmUtlSingleSel
        With frmDisplay
            .Text = "Select Merge Item... "
            Call .FillViewVsf(Me, strSQL, txtMItemId, 0, 0, txtMergeItem, 1)
            .ShowDialog()
            .vsfDet.Focus()
        End With

    End Sub

    Private Sub btnToItem_Click(sender As Object, e As EventArgs) Handles btnToItem.Click
        txtTItemID.Text = ""
        Dim strSQL As String = ""

        strSQL = "Select ItemID, ItemCode, ItemName "
        strSQL &= "from MstItem where InActive = 'N'"
        strSQL &= "order by itemcode"

        Dim frmDisplay As New frmUtlSingleSel
        With frmDisplay
            .Text = "Select To Item... "
            Call .FillViewVsf(Me, strSQL, txtTItemID, 0, 0, txtToItem, 1)
            .ShowDialog()
            .vsfDet.Focus()
        End With

    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Dim tb As New C1.Win.C1Command.C1DockingTabPage()
        Dim dockTabMain As New C1.Win.C1Command.C1DockingTab()
        Me.Dispose()
        tb.Controls.Clear()
        frmMain.dockTabMain.SelectedTab.Dispose()
    End Sub

    Private Sub btnMerge_Click(sender As Object, e As EventArgs) Handles btnMerge.Click
        Dim strSQL As String = ""
        Dim blnFound As Boolean = False
        showWaitCursor()
        If txtTItemID.Text = "" Then
            MsgBox("To Merge Item is invalid")
            Exit Sub
        End If
        If txtMItemId.Text = "" Then
            MsgBox("Merge Item is invalid")
            Exit Sub
        End If

        Try
            'check in Utl StockList
            clsSvt.getDataLongValue("Select ItemId From utlStockList Where ItemId=" & txtMItemId.Text, 0, blnFound)
            If blnFound Then
                strSQL = "update utlStockList set ItemId=" & txtTItemID.Text & " where ItemId=" & txtMItemId.Text
                clsSvt.execSQLStatement(strSQL)

                Dim dblQOH As Double = clsSvt.getDataDoubleValue("Select Sum(CLQty) From TrnQOH Where ItemID in (" & Val(txtTItemID.Text) & "," & Val(txtMItemId.Text) & ")", 0, False)

                clsSvt.execSQLStatement("Delete from TrnQOH Where itemID in (" & Val(txtTItemID.Text) & "," & Val(txtMItemId.Text) & ")")

                clsSvt.execSQLStatement("Insert Into TrnQOH (ItemID, LocID, OPQty, TrnQty, CLBal) values (" & Val(txtTItemID.Text) & ", 1, 0, " & dblQOH & ", " & dblQOH & ")")
            End If
            'TrnDtlPB
            clsSvt.getDataLongValue("Select ItemId From TrnDtlPB Where ItemId=" & txtMItemId.Text, 0, blnFound)
            If blnFound Then
                strSQL = "update TrnDtlPB set ItemId=" & txtTItemID.Text & " where ItemId=" & txtMItemId.Text
                clsSvt.execSQLStatement(strSQL)
            End If
            'TrnDtlPI
            clsSvt.getDataLongValue("Select ItemId From TrnDtlPI Where ItemId=" & txtMItemId.Text, 0, blnFound)
            If blnFound Then
                strSQL = "update TrnDtlPI set ItemId=" & txtTItemID.Text & " where ItemId=" & txtMItemId.Text
                clsSvt.execSQLStatement(strSQL)
            End If

            'TrnDtlPR
            clsSvt.getDataLongValue("Select ItemId From TrnDtlPR Where ItemId=" & txtMItemId.Text, 0, blnFound)
            If blnFound Then
                strSQL = "update TrnDtlPR set ItemId=" & txtTItemID.Text & " where ItemId=" & txtMItemId.Text
                clsSvt.execSQLStatement(strSQL)
            End If

            'TrnDtlQTN
            clsSvt.getDataLongValue("Select ItemId From TrnDtlQTN Where ItemId=" & txtMItemId.Text, 0, blnFound)
            If blnFound Then
                strSQL = "update TrnDtlQTN set ItemId=" & txtTItemID.Text & " where ItemId=" & txtMItemId.Text
                clsSvt.execSQLStatement(strSQL)
            End If

            'TrnDtlSINV
            clsSvt.getDataLongValue("Select ItemId From TrnDtlSINV Where ItemId=" & txtMItemId.Text, 0, blnFound)
            If blnFound Then
                strSQL = "update TrnDtlSINV set ItemId=" & txtTItemID.Text & " where ItemId=" & txtMItemId.Text
                clsSvt.execSQLStatement(strSQL)
            End If

            'TrnDtlSR
            clsSvt.getDataLongValue("Select ItemId From TrnDtlSR Where ItemId=" & txtMItemId.Text, 0, blnFound)
            If blnFound Then
                strSQL = "update TrnDtlSR set ItemId=" & txtTItemID.Text & " where ItemId=" & txtMItemId.Text
                clsSvt.execSQLStatement(strSQL)
            End If

            'TrnDtlSTKDED
            clsSvt.getDataLongValue("Select ItemId From TrnDtlSTKDED Where ItemId=" & txtMItemId.Text, 0, blnFound)
            If blnFound Then
                strSQL = "update TrnDtlSTKDED set ItemId=" & txtTItemID.Text & " where ItemId=" & txtMItemId.Text
                clsSvt.execSQLStatement(strSQL)
            End If

            'TrnStock
            clsSvt.getDataLongValue("Select ItemId From TrnStock Where ItemId=" & txtMItemId.Text, 0, blnFound)
            If blnFound Then
                strSQL = "update TrnStock set ItemId=" & txtTItemID.Text & " where ItemId=" & txtMItemId.Text
                clsSvt.execSQLStatement(strSQL)
            End If

            'TrnStkFlow
            clsSvt.getDataLongValue("Select ItemId From TrnStkFlow Where ItemId=" & txtMItemId.Text, 0, blnFound)
            If blnFound Then
                strSQL = "update TrnStkFlow set ItemId=" & txtTItemID.Text & " where ItemId=" & txtMItemId.Text
                clsSvt.execSQLStatement(strSQL)
            End If

            'TrnQOH
            clsSvt.getDataLongValue("Select ItemId From TrnQOH Where ItemId=" & txtMItemId.Text, 0, blnFound)
            If blnFound Then
                strSQL = "update TrnQOH set ItemId=" & txtTItemID.Text & " where ItemId=" & txtMItemId.Text
                clsSvt.execSQLStatement(strSQL)
            End If

            'TrnMultiDocGen
            clsSvt.getDataLongValue("Select ItemId From TrnMultiDocGen Where ItemId=" & txtMItemId.Text, 0, blnFound)
            If blnFound Then
                strSQL = "update TrnMultiDocGen set ItemId=" & txtTItemID.Text & " where ItemId=" & txtMItemId.Text
                clsSvt.execSQLStatement(strSQL)
            End If

            '==========================
            'to delete in all the masters
            strSQL = "Delete from MstCUSTITEM where ItemId=" & txtMItemId.Text
            clsSvt.execSQLStatement(strSQL)

            strSQL = "Delete from MstDtlBOM where ItemId=" & txtMItemId.Text
            clsSvt.execSQLStatement(strSQL)

            strSQL = "Delete from MstDtlCUSTITEM where ItemId=" & txtMItemId.Text
            clsSvt.execSQLStatement(strSQL)

            strSQL = "Delete from MstDtlSUPPITEM where ItemId=" & txtMItemId.Text
            clsSvt.execSQLStatement(strSQL)

            strSQL = "Delete from MstItemAlias where ItemId=" & txtMItemId.Text
            clsSvt.execSQLStatement(strSQL)

            strSQL = "Delete from MstItemTemp where ItemId=" & txtMItemId.Text
            clsSvt.execSQLStatement(strSQL)

            strSQL = "Delete from MstItemUOM where ItemId=" & txtMItemId.Text
            clsSvt.execSQLStatement(strSQL)

            strSQL = "Delete from MstPriceList where ItemId=" & txtMItemId.Text
            clsSvt.execSQLStatement(strSQL)

            strSQL = "Delete from MstQuickSelItems where ItemId=" & txtMItemId.Text
            clsSvt.execSQLStatement(strSQL)

            strSQL = "Delete from MstSUPPITEM where ItemId=" & txtMItemId.Text
            clsSvt.execSQLStatement(strSQL)

            strSQL = "Delete from MstSuppRef where ItemId=" & txtMItemId.Text
            clsSvt.execSQLStatement(strSQL)

            strSQL = "Delete from MstSuppRef where ItemId=" & txtMItemId.Text
            clsSvt.execSQLStatement(strSQL)

            strSQL = "Delete from MstItem where ItemId=" & txtMItemId.Text
            clsSvt.execSQLStatement(strSQL)

            showNormalCursor()
            MsgBox("Successfully Merged!", MsgBoxStyle.Information, Me.Text)

        Catch ex As Exception
            showNormalCursor()
            MsgBox(ex.Message, MsgBoxStyle.Critical, Me.Text)
        End Try
    End Sub

End Class