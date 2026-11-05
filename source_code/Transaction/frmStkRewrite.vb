Imports C1.Win.C1FlexGrid
Imports System.Data.oledb

Public Class frmStkRewrite
    Dim clssvt As New svtLibrary
    Dim strType As String

    Const vDetDocType = 0
    Const vDetItemName = 1
    Const vDetTrnStock = 2
    Const vDetSysStock = 3
    Const vDetDiff = 4

    Const vQOHItemName = 0
    Const vQOHLedStock = 1
    Const vQOHSysStock = 2
    Const vQOHDiff = 3

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub vsfDet_BeforeEdit(sender As Object, e As RowColEventArgs) Handles vsfDet.BeforeEdit
        e.Cancel = True
    End Sub

    Private Sub vsfQOH_BeforeEdit(sender As Object, e As RowColEventArgs) Handles vsfQOH.BeforeEdit
        e.Cancel = True
    End Sub
    Public Sub initScreen(argType As String)
        Me.KeyPreview = True
        strType = argType
        Call fillData()
    End Sub
    Private Sub fillData()
        Dim strSQL As String, rs As oledb.oledbDataReader

        With vsfDet
            .Rows.Count = 1
            .Cols.Count = 5
            .SetData(0, vDetDocType, "Doc Type")
            .SetData(0, vDetItemName, "Item Name")
            .SetData(0, vDetTrnStock, "Trn.Stock")
            .SetData(0, vDetSysStock, "Sys.Stock")
            .SetData(0, vDetDiff, "Difference")
        End With
        With vsfQOH
            .Rows.Count = 1
            .Cols.Count = 4
            .SetData(0, vQOHItemName, "Item Name")
            .SetData(0, vQOHLedStock, "Ledger.Stock")
            .SetData(0, vQOHSysStock, "QOH")
            .SetData(0, vQOHDiff, "Difference")
        End With
        Call initFormSize(Me, Me.Text, False, 958, 579, 0, 0)
        Call initFormVsfDet(Me, Me.Text, vsfDet, "DET")
        Call initFormVsfDet(Me, Me.Text, vsfQOH, "QOH")

        'Purchase Bill
        strSQL = "Select Itemname, i.itemid, sum(PQty) as DocQty, sum(UslPBQty) as SysQty, sum(pQty) - sum(uslpbqty) as diffQty from ("
        strSQL &= " Select itemid, sum(pbQty) pqty, 0 As USLPbQty from trndtlpb d group by itemid"
        strSQL &= " union all"
        strSQL &= " Select itemid, 0, sum(Qty) As USLPbQty from UtlStockList where upper(DocType) in ('PB','SA','ST')"
        strSQL &= " group by itemid) a, MstItem i where i.itemid = a.itemid "
        strSQL &= " group by i.itemid, ItemName"
        strSQL &= " having sum(pQty) <> sum(uslpbqty) "
        rs = clssvt.getDataReaderFromSQL(strSQL)
        Do While rs.Read
            vsfDet.Rows.Count = vsfDet.Rows.Count + 1
            vsfDet.SetData(vsfDet.Rows.Count - 1, vDetDocType, "Purchase Bill")
            vsfDet.SetData(vsfDet.Rows.Count - 1, vDetItemName, rs!Itemname)
            vsfDet.SetData(vsfDet.Rows.Count - 1, vDetTrnStock, rs!DocQty)
            vsfDet.SetData(vsfDet.Rows.Count - 1, vDetSysStock, rs!SysQty)
            vsfDet.SetData(vsfDet.Rows.Count - 1, vDetDiff, rs!DiffQty)
        Loop
        rs.Close()

        'Invoice
        strSQL = "Select Itemname, i.itemid, sum(PQty) as DocQty, sum(UslInvQty) as SysQty, sum(pQty) - sum(uslInvqty) as diffQty from ("
        strSQL &= " Select itemid, sum(SInvQty) pqty, 0 As USLInvQty from TrndtlSInv d, TrnhdrSInv h "
        strSQL &= " where h.SinvID = d.SinvID"
        strSQL &= " AND h.Cancelled = 'N' "
        strSQL &= " And h.IsDelete = 'N'"
        strSQL &= " group by itemid"
        strSQL &= " union all"
        strSQL &= " Select itemid, 0, sum(Qty) As USLPbQty from UtlStockList where upper(DocType) = 'INV'"
        strSQL &= " group by itemid) a, MstItem i where i.itemid = a.itemid "
        strSQL &= " group by i.itemid, ItemName"
        strSQL &= " having sum(pQty) <> sum(uslInvqty) "
        rs = clssvt.getDataReaderFromSQL(strSQL)
        Do While rs.Read
            vsfDet.Rows.Count = vsfDet.Rows.Count + 1
            vsfDet.SetData(vsfDet.Rows.Count - 1, vDetDocType, "Sales Invoice")
            vsfDet.SetData(vsfDet.Rows.Count - 1, vDetItemName, rs!Itemname)
            vsfDet.SetData(vsfDet.Rows.Count - 1, vDetTrnStock, rs!DocQty)
            vsfDet.SetData(vsfDet.Rows.Count - 1, vDetSysStock, rs!SysQty)
            vsfDet.SetData(vsfDet.Rows.Count - 1, vDetDiff, rs!DiffQty)
        Loop
        rs.Close()

        'Stock Deduction
        strSQL = "Select Itemname, i.itemid, sum(PQty) as DocQty, sum(UslStkDedQty) as SysQty, sum(pQty) - sum(uslStkDedqty) as diffQty from ("
        strSQL &= " Select itemid, sum(StkDedQty) pqty, 0 As USLStkDedQty from trndtlStkDed d group by itemid"
        strSQL &= " union all"
        strSQL &= " Select itemid, 0, sum(Qty) As USLStkDedQty from UtlStockList where upper(DocType) = 'STKDED'"
        strSQL &= " group by itemid) a, MstItem i where i.itemid = a.itemid "
        strSQL &= " group by i.itemid, ItemName"
        strSQL &= " having sum(pQty) <> sum(uslStkDedqty) "
        rs = clssvt.getDataReaderFromSQL(strSQL)
        Do While rs.Read
            vsfDet.Rows.Count = vsfDet.Rows.Count + 1
            vsfDet.SetData(vsfDet.Rows.Count - 1, vDetDocType, "Stock Deduction")
            vsfDet.SetData(vsfDet.Rows.Count - 1, vDetItemName, rs!Itemname)
            vsfDet.SetData(vsfDet.Rows.Count - 1, vDetTrnStock, rs!DocQty)
            vsfDet.SetData(vsfDet.Rows.Count - 1, vDetSysStock, rs!SysQty)
            vsfDet.SetData(vsfDet.Rows.Count - 1, vDetDiff, rs!DiffQty)
        Loop
        rs.Close()

        If fnTablePresent("TrnHdrSR") Then
            'Sales return
            strSQL = "Select Itemname, i.itemid, sum(PQty) as DocQty, sum(UslSRQty) as SysQty, sum(pQty) - sum(uslSRqty) as diffQty from ("
            strSQL &= " Select itemid, sum(SRQty) pqty, 0 As USLSRQty from trndtlSR d group by itemid"
            strSQL &= " union all"
            strSQL &= " Select itemid, 0, sum(Qty) As USLSRQty from UtlStockList where upper(DocType) = 'SR'"
            strSQL &= " group by itemid) a, MstItem i where i.itemid = a.itemid "
            strSQL &= " group by i.itemid, ItemName"
            strSQL &= " having sum(pQty) <> sum(uslSRqty) "
            rs = clssvt.getDataReaderFromSQL(strSQL)
            Do While rs.Read
                vsfDet.Rows.Count = vsfDet.Rows.Count + 1
                vsfDet.SetData(vsfDet.Rows.Count - 1, vDetDocType, "Sales Return")
                vsfDet.SetData(vsfDet.Rows.Count - 1, vDetItemName, rs!Itemname)
                vsfDet.SetData(vsfDet.Rows.Count - 1, vDetTrnStock, rs!DocQty)
                vsfDet.SetData(vsfDet.Rows.Count - 1, vDetSysStock, rs!SysQty)
                vsfDet.SetData(vsfDet.Rows.Count - 1, vDetDiff, rs!DiffQty)
            Loop
            rs.Close()
        End If

        strSQL = "select i.itemid, ItemName, Sum(PQty) as docQty, sum(uslqty) as sysQty, sum(pQty) - sum(uslqty) as diffQty from ("
        strSQL &= " Select itemid, sum(StkMultiplier * Qty) pqty, 0  as UslQty from UtlStockList d group by itemid"
        strSQL &= " union all"
        strSQL &= " Select itemid, 0, clbal from TrnQOH) a, mstitem i "
        strSQL &= " where a.itemid = i.itemid"
        strSQL &= " group by i.itemid, itemName"
        strSQL &= " having sum(pQty) <> sum(uslqty)"
        rs = clssvt.getDataReaderFromSQL(strSQL)
        Do While rs.Read
            vsfQOH.Rows.Count = vsfQOH.Rows.Count + 1
            vsfQOH.SetData(vsfQOH.Rows.Count - 1, vQOHItemName, rs!Itemname)
            vsfQOH.SetData(vsfQOH.Rows.Count - 1, vQOHLedStock, rs!DocQty)
            vsfQOH.SetData(vsfQOH.Rows.Count - 1, vQOHSysStock, rs!SysQty)
            vsfQOH.SetData(vsfQOH.Rows.Count - 1, vQOHDiff, rs!DiffQty)
        Loop
        rs.Close()

        If vsfDet.Rows.Count = 1 And vsfQOH.Rows.Count = 1 Then
            btnReWrite.BackColor = Color.Gray
            btnReWrite.Text = "Re-Write not Required"
        End If
    End Sub

    Private Sub btnReWrite_Click(sender As Object, e As EventArgs) Handles btnReWrite.Click
        Call alterReWriteStkSP()
        If vsfDet.Rows.Count = 1 And vsfQOH.Rows.Count = 1 Then
            Exit Sub
        End If
        Dim intRes As Integer
        Dim cmd As OleDbCommand = New OleDbCommand("mSp_RewriteStkList")
        Try
            Call showWaitCursor()
            Using cmd
                With cmd
                    cmd.Connection = svtConn
                    cmd.CommandTimeout = 0
                    .CommandType = CommandType.StoredProcedure
                    .Parameters.Add("@argType", OleDbType.VarChar, 20, ParameterDirection.Input)
                    .Parameters(0).Value = strType
                    .Parameters.Add("@Retval", OleDbType.Integer)
                    .Parameters("@Retval").Direction = ParameterDirection.Output
                    .ExecuteNonQuery()
                    intRes = GetVal(.Parameters("@Retval").Value, 1)
                    If intRes = 0 Then
                        Call showNormalCursor()
                        MsgBox("Error in Re-Writing...", MsgBoxStyle.Critical, Me.Text)
                        Exit Sub
                    End If
                End With
            End Using
            'vsfDet.Rows.Count = 1
            'vsfQOH.Rows.Count = 1

            'Dim strSQL As String, rs As oledbDataReader
            'strSQL = "select i.itemid, ItemName, Sum(PQty) as docQty, sum(uslqty) as sysQty, sum(pQty) - sum(uslqty) as diffQty from ("
            'strSQL &= " Select itemid, sum(StkMultiplier * Qty) pqty, 0  as UslQty from UtlStockList d group by itemid"
            'strSQL &= " union all"
            'strSQL &= " Select itemid, 0, clbal from TrnQOH) a, mstitem i "
            'strSQL &= " where a.itemid = i.itemid"
            'strSQL &= " group by i.itemid, itemName"
            'strSQL &= " having sum(pQty) <> sum(uslqty)"
            'rs = clssvt.getDataReaderFromSQL(strSQL)
            'Do While rs.Read
            '    vsfQOH.Rows.Count = vsfQOH.Rows.Count + 1
            '    vsfQOH.SetData(vsfQOH.Rows.Count - 1, vQOHItemName, rs!Itemname)
            '    vsfQOH.SetData(vsfQOH.Rows.Count - 1, vQOHLedStock, rs!DocQty)
            '    vsfQOH.SetData(vsfQOH.Rows.Count - 1, vQOHSysStock, rs!SysQty)
            '    vsfQOH.SetData(vsfQOH.Rows.Count - 1, vQOHDiff, rs!DiffQty)
            'Loop
            'rs.Close()

            Call showNormalCursor()
            MsgBox("Successfully re-Written Stock!", MsgBoxStyle.Information, Me.Text)
            Call fillData()
        Catch ex As Exception
            MsgBox(ex)
        End Try
    End Sub

    Private Sub btnExportToXL_Click(sender As Object, e As EventArgs) Handles btnExportToXL.Click
        Call ExportToXL(vsfDet, "Diff. in Transaction Qty  and System Stock Qty")
        Call ExportToXL(vsfQOH, "Diff. in Stock Ledger and QOH")
    End Sub

    Private Sub frmStkRewrite_KeyUp(sender As Object, e As KeyEventArgs) Handles Me.KeyUp
        If e.KeyCode = Keys.F8 And e.Control = True Then
            Call SaveActFormSettings(Me, Me.Text)
            Call SaveActFormVsfDet(Me, Me.Text, "DET", vsfDet)
            Call SaveActFormVsfDet(Me, Me.Text, "QOH", vsfQOH)
            MsgBox("Saved Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
    End Sub

    Private Sub frmStkRewrite_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub
    Private Sub alterReWriteStkSP()
        clssvt.execSQLStatement("
ALTER PROCEDURE [dbo].[mSp_RewriteStkList] 
	@argType varchar(50), @Retval int output
AS
--exec mSp_RewriteStkList 
BEGIN
		set @Retval = 0; --FAIL

		delete from trnDtlPB where PBID not in (Select PBID from trnHdrPB)
		delete from trnDtlsInv where sInvID not in (Select SinvID from TrnHdrSINV)
		delete from TrnDtlSTKDED where STKDEDID not in (Select stkdedID from TrnHdrSTKDED)

		if @argType = 'FULL' 
		BEGIN
			BEGIN TRY
				
				update UtlMisc set ReWriteINPROGRESS = 'Y'
				BEGIN TRAN TRANStkList
					select * into #UList from UtlStockList where 1 <> 1

					Insert Into #UList 
					(ItemID, StkMultiplier, DocType, DocDate, DocID, DocNo, Particulars, Qty, LtrQty, Rate, locid, delflag, rewrite, FifoUsedQty, DocValue, FifoRate) 
					Select ItemID, 1 as StkMultiplier, 
					case when Purchasetype = 'Purchase' then 'PB' 
						 when Purchasetype = 'Stock Addition' then 'SA' 
						 when Purchasetype = 'Stock Transfer' then 'ST' end 
					as docType, PBDate, h.PBID, PBNO, PartyName, PBQty, IsNull(PBQtyInLtrs,0), PBRate, 0 , 'N', 'N', 0, 0, 0
					From TrnDtlPB d, TrnHdrPB h 
					Where d.PBID=h.PBID 
					AND h.Cancelled = 'N' 
					select  *from trnhdrpb

					--Sales Invoice -  Reduce from Stock
					Insert Into #UList 
					(ItemID, StkMultiplier, DocType, DocDate, DocID, DocNo, Particulars, Qty, LtrQty, Rate, locid, delflag, rewrite, FifoUsedQty, DocValue, FifoRate) 
					Select ItemID, '-1', 'INV', SINVDate, h.SINVID, SINVNO, PartyName, SINVQty, IsNull(SINVQtyInLtrs,0), 0,0, 'N', 'N' , 0, 0, 0
					From TrnDtlSINV d, TrnHdrSINV h 
					Where d.SINVID=h.SINVID 
					AND h.Cancelled = 'N' 
					AND h.IsDelete = 'N'

					--Stock Deduction -  Reduce from Stock
					Insert Into #UList 
					(ItemID, StkMultiplier, DocType, DocDate, DocID, DocNo, Particulars, Qty, LtrQty, Rate, locid, delflag, rewrite, FifoUsedQty, DocValue, FifoRate) 
					Select ItemID, '-1', 'STKDED', STKDEDDate, h.STKDEDID, STKDEDNO, '', STKDEDQty, IsNull(STKDEDQtyInLtrs,0), 0,0, 'N', 'N' , 0, 0, 0
					From TrnDtlSTKDED d, TrnHdrSTKDED h 
					Where d.STKDEDID=h.STKDEDID 
					AND h.Cancelled = 'N' 

					--sales return
					if exists(select * from sys.tables t where upper(t.name) = 'TRNHDRSR')
					begin
						Insert Into #UList 
						(ItemID, StkMultiplier, DocType, DocDate, DocID, DocNo, Particulars, Qty, LtrQty, Rate, LOCID, delflag, rewrite, FifoUsedQty, DocValue, FifoRate)  
						Select ItemID, 1 as StkMultiplier, 
						'SR' as docType, SRDate, h.SRID, SRNO, '', SRQty, IsNull(SRQtyInLtrs,0), SRRate, 0, 'N', 'N' , 0, 0, 0
						From TrnDtlSR d, TrnHdrSR h 
						Where d.SRID=h.SRID 
						AND h.Cancelled = 'N' 
					end

					select itemid, sum(stkmultiplier *  qty) as negStk 
					into #Neg
					from #uList 
					group by itemid 
					having sum(stkmultiplier *  qty) < 0

					if exists(select top 1 * from #neg where ItemID > 0) 
						begin
							--do not insert data for negative items
							declare @i int;
						end
					else
						begin
							--write to utlStock
							truncate table trnqoh
							truncate table UtlStockList

							-- Disable all table constraints
							ALTER TABLE TrnQOH NOCHECK CONSTRAINT ALL

							Insert Into UtlStockList
							(ItemID, StkMultiplier, DocType, DocDate, DocID, DocNo, Particulars, Qty, LtrQty, Rate, locid, delflag, rewrite) 
							select ItemID, StkMultiplier, DocType, DocDate, DocID, DocNo, Particulars, Qty, LtrQty, Rate, locid, delflag, rewrite
							from #UList 
							order by docdate, itemid, StkMultiplier desc, doctype, docid

							truncate table TrnQoh
		
							insert into TrnQOH (itemid, LocId, clBal)
							select itemid, 1, sum(qty * stkmultiplier) from #UList group by itemid having sum(qty * stkmultiplier) >= 0

							-- Enable all table constraints
							ALTER TABLE TrnQOH CHECK CONSTRAINT ALL
						end
 
					/*SELECT rowid, docdate, itemid, doctype, StkMultiplier, qty,  SUM (qty * stkmultiplier) OVER  (PARTITION BY ItemID 
					ORDER BY docdate, itemid, StkMultiplier desc, doctype, docid)  AS RunningAgeTotal
					FROM #ulist
					order by docdate, itemid, StkMultiplier desc, doctype, docid
					*/
					
				COMMIT TRAN TRANStkList
				update UtlMisc set ReWriteINPROGRESS = 'N'
				set @Retval = 1; --SUCCESSS
			END TRY

			BEGIN CATCH
				update UtlMisc set ReWriteINPROGRESS = 'N'
				set @Retval = 0; --FAIL
				select ERROR_MESSAGE()
				ROLLBACK TRAN TRANStkList
			END CATCH
		END
		if @argType = 'QOH' 
		BEGIN
			truncate table TrnQoh
			
			insert into TrnQOH (itemid, LocID, clBal)
			select itemid, 1, sum(qty * stkmultiplier) from UtlStockList group by itemid having sum(qty * stkmultiplier) >= 0
			set @Retval = 1; --SUCCESSS
			update UtlMisc set ReWriteINPROGRESS = 'N'
		END
END")
    End Sub
End Class