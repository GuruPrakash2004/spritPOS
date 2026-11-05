Imports C1.Win.C1FlexGrid

Public Class frmRptReWriteStock

    Dim clsSVT As New svtLibrary
    Dim strRights As String

    Dim dt As DataTable
    Dim dv As DataView

    Private Sub frmRptDailySummary_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        fillFormProperties(Me)
        ChangeFontSize(Me)
        initGridStyles(Me)
        InitScreen()
        initFormVsfDet(Me, Me.Text, vsfView, "VIE")
        FillData()
    End Sub

    Private Sub InitScreen()
        With vsfView
            .Rows.Count = 1
            .Rows.Fixed = 1
            .Cols.Count = 0
            .Cols.Fixed = 0
        End With
        Dim strSQL As String = ""
        strSQL = " Select LocCode as Location, ItemGroupName as ItemGroup, ItemCode, ItemName, ShortName, UOMCode as Uom, "
        strSQL &= " Qty as ClQty, LtrQty as ClQtyLtr, Rate as StkRate "
        strSQL &= " From UtlStockList s, MstItem i, MstITEMGROUP g, MstUOM u, MstLOC l "
        strSQL &= " Where s.ItemID = i.ITEMID "
        strSQL &= " and i.ITEMGroupID = g.ITEMGROUPID "
        strSQL &= " and i.UOMID = u.UOMID "
        strSQL &= " and s.LOCID = l.LOCID "
        strSQL &= " and i.ItemID = -1 "

        dt = clsSVT.getDATATable(strSQL)
        dv = New DataView(dt)
        vsfView.DataSource = dv
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        frmMain.CloseSelectedTabPage(Me)
    End Sub

    Private Sub frmRptReport_KeyUp(sender As Object, e As KeyEventArgs) Handles Me.KeyUp
        If e.KeyCode = Keys.F9 And e.Control = True Then
            Call initFormSize(Me, Me.Text, False, 577, 349, 0, 0)
            MsgBox("Restored Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
        If e.KeyCode = Keys.F8 And e.Control = True Then
            Call SaveActFormVsfDet(Me, Me.Text, "VIE", vsfView)
            MsgBox("Saved Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
    End Sub

    Private Function ReWriteStock() As Boolean
        ReWriteStock = False
        Dim strSQL As String = ""
        Dim svtTrn As OleDb.OleDbTransaction
        svtTrn = Nothing
        svtTrn = svtConn.BeginTransaction(IsolationLevel.ReadCommitted)
        Try

            strSQL = "Truncate table TrnQOH"
            If Not clsSVT.execSQLStatement(strSQL, svtTrn) Then
                svtTrn.Rollback()
                Exit Function
            End If

            strSQL = "Truncate table UtlStockList"
            If Not clsSVT.execSQLStatement(strSQL, svtTrn) Then
                svtTrn.Rollback()
                Exit Function
            End If

            strSQL = "Truncate table UtlStockRate"
            If Not clsSVT.execSQLStatement(strSQL, svtTrn) Then
                svtTrn.Rollback()
                Exit Function
            End If

            '--Purchase Bill - type =  Purchase - Add to Stock
            If clsSVT.getDataStringValue("Select Name From Sys.Tables Where Name = 'TrnHdrPB'", "", False, svtTrn) <> "" Then
                strSQL = " Insert Into UtlStockList  "
                strSQL &= " (ItemID, StkMultiplier, DocType, DocDate, DocID, DocNo, Particulars, Qty, LtrQty, Rate)  "
                strSQL &= " Select ItemID, '1', 'PB', PBDate, h.PBID, PBNO, PartyName, PBQty, IsNull(PBQtyInLtrs,0), PBRate "
                strSQL &= " From TrnDtlPB d, TrnHdrPB h  "
                strSQL &= " Where d.PBID=h.PBID "
                strSQL &= " AND h.Cancelled = 'N'  "
                strSQL &= " And h.PURCHASETYPE = 'PURCHASE' "
                If Not clsSVT.execSQLStatement(strSQL, svtTrn) Then
                    svtTrn.Rollback()
                    Exit Function
                End If
            End If

            '--Purchase Bill - type =  Stock Addition - Add to Stock
            If clsSVT.getDataStringValue("Select Name From Sys.Tables Where Name = 'TrnHdrPB'", "", False, svtTrn) <> "" Then
                strSQL = " Insert Into UtlStockList  "
                strSQL &= " (ItemID, StkMultiplier, DocType, DocDate, DocID, DocNo, Particulars, Qty, LtrQty, Rate)  "
                strSQL &= " Select ItemID, '1', 'PB', PBDate, h.PBID, PBNO, PURCHASETYPE, PBQty, IsNull(PBQtyInLtrs,0), PBRate "
                strSQL &= " From TrnDtlPB d, TrnHdrPB h  "
                strSQL &= " Where d.PBID=h.PBID "
                strSQL &= " AND h.Cancelled = 'N'  "
                strSQL &= " And h.PURCHASETYPE = 'STOCK ADDITION' "
                If Not clsSVT.execSQLStatement(strSQL, svtTrn) Then
                    svtTrn.Rollback()
                    Exit Function
                End If
            End If

            '--Sales Return  - Add to Stock
            If clsSVT.getDataStringValue("Select Name From Sys.Tables Where Name = 'TrnHdrSR'", "", False, svtTrn) <> "" Then
                strSQL = " Insert Into UtlStockList  "
                strSQL &= " (ItemID, StkMultiplier, DocType, DocDate, DocID, DocNo, Particulars, Qty, LtrQty, Rate)  "
                strSQL &= " Select ItemID, '1', 'SR', SRDate, h.SRID, SRNO, '', SRQty, IsNull(SRQtyInLtrs,0), 0  "
                strSQL &= " From TrnDtlSR d, TrnHdrSR h   "
                strSQL &= " Where d.SRID=h.SRID  "
                strSQL &= " AND h.Cancelled = 'N'  "
                If Not clsSVT.execSQLStatement(strSQL, svtTrn) Then
                    svtTrn.Rollback()
                    Exit Function
                End If
            End If

            '--Sales Invoice -  Reduce from Stock
            If clsSVT.getDataStringValue("Select Name From Sys.Tables Where Name = 'TrnHdrSINV'", "", False, svtTrn) <> "" Then
                strSQL = " Insert Into UtlStockList  "
                strSQL &= " (ItemID, StkMultiplier, DocType, DocDate, DocID, DocNo, Particulars, Qty, LtrQty, Rate)  "
                strSQL &= " Select ItemID, '-1', 'INV', SINVDate, h.SINVID, SINVNO, PartyName, SINVQty, IsNull(SINVQtyInLtrs,0), 0   "
                strSQL &= " From TrnDtlSINV d, TrnHdrSINV h   "
                strSQL &= " Where d.SINVID=h.SINVID  "
                strSQL &= " AND h.Cancelled = 'N'  "
                strSQL &= " AND h.IsDelete = 'N'  "
                If Not clsSVT.execSQLStatement(strSQL, svtTrn) Then
                    svtTrn.Rollback()
                    Exit Function
                End If
            End If

            '--Stock Deduction -  Reduce from Stock
            If clsSVT.getDataStringValue("Select Name From Sys.Tables Where Name = 'TrnHdrSTKDED'", "", False, svtTrn) <> "" Then
                strSQL = " Insert Into UtlStockList  "
                strSQL &= " (ItemID, StkMultiplier, DocType, DocDate, DocID, DocNo, Particulars, Qty, LtrQty, Rate)  "
                strSQL &= " Select ItemID, '-1', 'STKDED', STKDEDDate, h.STKDEDID, STKDEDNO, '', STKDEDQty, IsNull(STKDEDQtyInLtrs,0), 0 "
                strSQL &= " From TrnDtlSTKDED d, TrnHdrSTKDED h    "
                strSQL &= " Where d.STKDEDID=h.STKDEDID  "
                strSQL &= " AND h.Cancelled = 'N'  "
                If Not clsSVT.execSQLStatement(strSQL, svtTrn) Then
                    svtTrn.Rollback()
                    Exit Function
                End If
            End If

            '--Purchase Bill - type =  Transfer - Reduce From Stock
            If clsSVT.getDataStringValue("Select Name From Sys.Tables Where Name = 'TrnHdrSTKDED'", "", False, svtTrn) <> "" Then
                strSQL = " Insert Into UtlStockList  "
                strSQL &= " (ItemID, StkMultiplier, DocType, DocDate, DocID, DocNo, Particulars, Qty, LtrQty, Rate)  "
                strSQL &= " Select ItemID, '-1', 'PB', PBDate, h.PBID, PBNO, PURCHASETYPE, PBQty, IsNull(PBQtyInLtrs,0), PBRate  "
                strSQL &= " From TrnDtlPB d, TrnHdrPB h  "
                strSQL &= " Where d.PBID=h.PBID "
                strSQL &= " AND h.Cancelled = 'N'  "
                strSQL &= " And h.PURCHASETYPE = 'STOCK TRANSFER' "
                If Not clsSVT.execSQLStatement(strSQL, svtTrn) Then
                    svtTrn.Rollback()
                    Exit Function
                End If
            End If

            strSQL = " Update u set LtrQty = Qty * LtrPerBot from UtlStockList u, MstItem i, MstPKG p where u.ItemID=i.ItemID and i.PKGID=p.pkgid"
            If Not clsSVT.execSQLStatement(strSQL, svtTrn) Then
                svtTrn.Rollback()
                Exit Function
            End If


            strSQL = "Truncate table TrnQOH"
            If Not clsSVT.execSQLStatement(strSQL, svtTrn) Then
                svtTrn.Rollback()
                Exit Function
            End If

            strSQL = " Insert Into TrnQOH (ItemID, LocID, OPQty, TrnQty, ClBal) "
            strSQL &= " Select ItemID, LocID, 0, 0, Sum(Qty * convert(int,StkMultiplier)) "
            strSQL &= " From UtlStockList "
            strSQL &= " Group By ItemID, LocID "
            If Not clsSVT.execSQLStatement(strSQL, svtTrn) Then
                svtTrn.Rollback()
                Exit Function
            End If

            strSQL = " Insert Into UtlStockRate (StkDate, ItemID, FIFORate, AvgRate, LastPurRate, StkRate) "
            strSQL &= " Select Max(DocDate), ItemID, 0, 0, 0, 0 From UtlStockList Group by ItemID  "
            If Not clsSVT.execSQLStatement(strSQL, svtTrn) Then
                svtTrn.Rollback()
                Exit Function
            End If

            strSQL = " Update u set u.StkRate=isnull(i.PURRATE ,0) from UtlStockRate u, MstITEM i where u.ItemID=i.ItemID "
            If Not clsSVT.execSQLStatement(strSQL, svtTrn) Then
                svtTrn.Rollback()
                Exit Function
            End If

            strSQL = " Update u set u.AvgRate=isnull(i.AvgRate ,0) from UtlStockRate u, "
            strSQL &= " (Select ItemID, Avg(Rate) as AvgRate From UtlStockList Where Rate > 0 Group by ItemID) i where u.ItemID=i.ItemID "
            If Not clsSVT.execSQLStatement(strSQL, svtTrn) Then
                svtTrn.Rollback()
                Exit Function
            End If

            strSQL = " update UtlStockRate set LastPurRate = isnull((Select Top 1 PBRate from TrnHdrPB h, TrnDtlPB d "
            strSQL &= " Where h.PBID = d.PBID  "
            strSQL &= " and h.Cancelled='N' "
            strSQL &= " and UtlStockRate.ItemID = d.itemid "
            strSQL &= " Order by h.PBID Desc),0) "
            If Not clsSVT.execSQLStatement(strSQL, svtTrn) Then
                svtTrn.Rollback()
                Exit Function
            End If

            svtTrn.Commit()
            Return True
        Catch ex As Exception
            svtTrn.Rollback()
        End Try
    End Function

    Public Sub FillData()
        If Not ReWriteStock() Then
            Exit Sub
        End If

        Dim rs As OleDb.OleDbDataReader
        Dim strSQL As String = ""

        strSQL = " Select LocCode as Location, ItemGroupName as ItemGroup, ItemCode, ItemName, ShortName, UOMCode as Uom, "
        strSQL &= " Sum(Qty * Convert(int,StkMultiplier)) as ClQty, Sum(LtrQty * Convert(int,StkMultiplier)) as ClQtyLtr, "
        strSQL &= " IsNull((Select Avg(Rate) From UtlStockList v Where v.ItemID=s.ItemID and v.LocID=s.LocID and v.StkMultiplier = '1' ),0) as StkRate"
        strSQL &= " From UtlStockList s, MstItem i, MstITEMGROUP g, MstUOM u, MstLOC l "
        strSQL &= " Where s.ItemID = i.ITEMID "
        strSQL &= " and i.ITEMGroupID = g.ITEMGROUPID "
        strSQL &= " and i.UOMID = u.UOMID "
        strSQL &= " and s.LOCID = l.LOCID "
        strSQL &= " Group by LocCode, ItemGroupName, Itemcode, Itemname, Shortname, UOMCode, s.ItemID, s.LOCID "
        strSQL &= " Order by 1, 2, 3 "

        dt = clsSVT.getDATATable(strSQL)
        dv = New DataView(dt)
        vsfView.DataSource = dv
    End Sub

    Private Sub btnReWrite_Click(sender As Object, e As EventArgs) Handles btnReWrite.Click
        Call FillData()
    End Sub

    Private Sub vsfView_BeforeEdit(sender As Object, e As RowColEventArgs)
        e.Cancel = True
    End Sub

    Private Sub btnExportToXL_Click(sender As Object, e As EventArgs) Handles btnExportToXL.Click
        ExportToXL(vsfView, Me.Text)
    End Sub
End Class