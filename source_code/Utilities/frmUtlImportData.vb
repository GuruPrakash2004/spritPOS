Imports C1.Win.C1FlexGrid.CheckEnum
Imports C1.Win.C1FlexGrid
Imports System.Windows.Forms
Imports System.IO
Imports System.Data.OleDb


Public Class frmUtlImportData
    Dim clsSVT As New svtLibrary
    Dim svtTrn As OleDb.OleDbTransaction

    Const vdetslno = 0
    Const vdetSel = 1
    Const vdetMasters = 2
    Const vdetTblName = 3

    Const intDetCols = 4

    Const vTrnSlno = 0
    Const vTrnSel = 1
    Const vTrnName = 2
    Const vTrnHdrTbl = 3
    Const vTrnDtlTbl = 4
    Const intTrnCols = 5

    Dim blnImpTbl As Boolean


    Private Sub btnLoadItem_Click(sender As System.Object, e As System.EventArgs) Handles btnLoadItem.Click
        If optMasters.Checked = True Then
            Call ImportMasterData()
        Else
            Call ImportTrnData()
        End If
    End Sub
    Private Sub ImportTrnData()
        Dim blnTrnChk As Boolean = False
        Dim blnImpTrnTbl As Boolean = False

        Dim svtTrn As OleDb.OleDbTransaction
        Cursor = Cursors.WaitCursor
        svtTrn = Nothing
        svtTrn = svtConn.BeginTransaction(IsolationLevel.ReadCommitted)
        Try
            With vsfTrn
                For i = 1 To .Rows.Count - 1
                    If .GetCellCheck(i, vTrnSel) = Checked Then
                        blnTrnChk = True
                        If ImportHdrDtlData(.GetData(i, vTrnName), .GetData(i, vTrnHdrTbl), .GetData(i, vTrnDtlTbl), svtTrn) Then
                            blnImpTrnTbl = True
                        End If
                    End If
                Next

                If blnTrnChk = False Then
                    MsgBox("Please Select atlease one Transaction to Import!!", vbInformation, Me.Text)
                    Exit Sub
                End If
            End With

            If blnImpTrnTbl = True Then
                svtTrn.Commit()
                MsgBox("Data Imported Successfully!!", vbInformation, Me.Text)
            Else
                svtTrn.Rollback()
                MsgBox("Some of the Data," & vbCrLf & "Not Imported!!", vbInformation, Me.Text)
                Exit Sub
            End If

        Catch ex As Exception
            MsgBox("Error on Execution" & vbCrLf & ex.Message, MsgBoxStyle.Critical, Me.Text)
            svtTrn.Rollback()
        End Try
        Cursor = Cursors.Default
    End Sub
    Private Sub ImportMasterData()
        Dim blnTblChk As Boolean = False

        Dim svtTrn As OleDb.OleDbTransaction

        svtTrn = Nothing
        svtTrn = svtConn.BeginTransaction(IsolationLevel.ReadCommitted)
        Try
            With vsfDet
                For i = 1 To .Rows.Count - 1
                    If .GetCellCheck(i, vdetSel) = Checked Then
                        blnTblChk = True
                        Call ImportData(.GetData(i, vdetMasters), .GetData(i, vdetTblName), svtTrn)
                        If blnImpTbl = False Then
                            Exit For
                        End If
                    End If
                Next

                If blnTblChk = False Then
                    MsgBox("Please Select atlease one Master to Import!!", vbInformation, Me.Text)
                    Exit Sub
                End If
            End With

            If blnImpTbl = True Then
                svtTrn.Commit()
                MsgBox("Data Imported Successfully!!", vbInformation, Me.Text)
            Else
                svtTrn.Rollback()
                MsgBox("Some of data Not get Imported!!", vbInformation, Me.Text)
                Exit Sub
            End If

        Catch ex As Exception
            MsgBox("Error on Execution" & vbCrLf & ex.Message, MsgBoxStyle.Critical, Me.Text)
            svtTrn.Rollback()
        End Try

    End Sub

    Private Sub frmUtlImportData_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        With vsfDet
            .Rows.Count = 27
            .Cols.Count = intDetCols
            .SetData(0, vdetSel, "Sel")
            .SetData(0, vdetMasters, "Masters")
            .SetData(0, vdetTblName, "Table")
            .Cols(vdetSel).DataType = GetType(Boolean)

            .SetData(1, vdetMasters, "BRANCH MASTER")
            .SetData(1, vdetTblName, "MstBRANCH")
            .SetData(2, vdetMasters, "CURRENCY MASTER")
            .SetData(2, vdetTblName, "MstCur")
            .SetData(3, vdetMasters, "BANK MASTER")
            .SetData(3, vdetTblName, "MstBankDtl")
            .SetData(4, vdetMasters, "DEVICE MASTER")
            .SetData(4, vdetTblName, "MstDevice")
            .SetData(5, vdetMasters, "LEDGER GROUP MASTER")
            .SetData(5, vdetTblName, "MstLedgerGroup")
            .SetData(6, vdetMasters, "LEDGER MASTER")
            .SetData(6, vdetTblName, "MstLedger")
            .SetData(7, vdetMasters, "JW CATEGORY MASTER")
            .SetData(7, vdetTblName, "MSTJWCategory")
            .SetData(8, vdetMasters, "JOBWORKER MASTER")
            .SetData(8, vdetTblName, "MSTJW")
            .SetData(9, vdetMasters, "ITEM GROUP MASTER")
            .SetData(9, vdetTblName, "MstItemGroup")
            .SetData(10, vdetMasters, "BOARD GROUP")
            .SetData(10, vdetTblName, "MstItemBoardGroup")
            .SetData(11, vdetMasters, "BOARD SIZE MASTER")
            .SetData(11, vdetTblName, "MstBOARDSIZE")
            .SetData(12, vdetMasters, "UOM MASTER")
            .SetData(12, vdetTblName, "MstUOM")
            .SetData(13, vdetMasters, "ITEM MASTER")
            .SetData(13, vdetTblName, "MstItem")
            .SetData(14, vdetMasters, "STATE MASTER")
            .SetData(14, vdetTblName, "MstState")
            .SetData(15, vdetMasters, "AGENT MASTER")
            .SetData(15, vdetTblName, "MstAgent")
            .SetData(16, vdetMasters, "SALES TYPE MASTER")
            .SetData(16, vdetTblName, "MstST")
            .SetData(17, vdetMasters, "TRANSPORTER MASTER")
            .SetData(17, vdetTblName, "MstTRANSPORT")
            .SetData(18, vdetMasters, "SUPPLIER MASTER")
            .SetData(18, vdetTblName, "MstSupp")
            .SetData(19, vdetMasters, "CUSTOMER MASTER")
            .SetData(19, vdetTblName, "MstCust")
            .SetData(20, vdetMasters, "EXPORTER DETAIL MASTER")
            .SetData(20, vdetTblName, "MstEXPORTER")
            .SetData(21, vdetMasters, "BOM MASTER")
            .SetData(21, vdetTblName, "MstHDRBOM")
            .SetData(22, vdetMasters, "AGENT VS ITEM PRICE MASTER")
            .SetData(22, vdetTblName, "MstAgentItemPrice")
            .SetData(23, vdetMasters, "ITEM OPENING BALANCE")
            .SetData(23, vdetTblName, "MstOPBalance")
            .SetData(24, vdetMasters, "ITEM VS BOARDSIZE MASTER")
            .SetData(24, vdetTblName, "MstItemvsBoardSize")
            .SetData(25, vdetMasters, "SUPPLIER VS ITEMPRIORITY MASTER")
            .SetData(25, vdetTblName, "MstSuppItem")
            .SetData(25, vdetMasters, "CUSTOMER VS ITEM MASTER")
            .SetData(25, vdetTblName, "MstcustItem")
            .SetData(26, vdetMasters, "ITEM OPENING BALANCE")
            .SetData(26, vdetTblName, "MstOPBalance")


            Call SetHideCol(vsfDet, True, vdetslno)
            Call SetFlexColor(vsfDet, "", vdetMasters, vdetTblName)

            .ExtendLastCol = False
            Call SetColAlign(vsfDet, svtAlign.Left, vdetMasters, vdetTblName)
            Call SetColWidth(vsfDet, 90, 150, 250, 250)
        End With
        Call initTrnGrid()
    End Sub

    Private Sub initTrnGrid()
        With vsfTrn
            .Rows.Count = 24
            .Cols.Count = intTrnCols
            .SetData(0, vTrnSel, "Sel")
            .SetData(0, vTrnName, "Transactions")
            .SetData(0, vTrnHdrTbl, "HdrTbl")
            .SetData(0, vTrnDtlTbl, "DtlTbl")
            .Cols(vTrnSel).DataType = GetType(Boolean)
            Call SetHideCol(vsfTrn, True, vTrnSlno)

            .SetData(1, vTrnName, "Purchase Order")
            .SetData(1, vTrnHdrTbl, "TrnHdrPO")
            .SetData(1, vTrnDtlTbl, "TrnDtlPO")
            .SetData(2, vTrnName, "JW Purchase Order")
            .SetData(2, vTrnHdrTbl, "TrnHdrJWPO")
            .SetData(2, vTrnDtlTbl, "TrnDtlJWPO")
            .SetData(3, vTrnName, "JW Production Entry")
            .SetData(3, vTrnHdrTbl, "TrnHdrJWPRDN")
            .SetData(3, vTrnDtlTbl, "TrnDtlJWPRDN")
            .SetData(4, vTrnName, "Issue From JobWork")
            .SetData(4, vTrnHdrTbl, "TrnHdrIssue")
            .SetData(4, vTrnDtlTbl, "TrnDtlIssue")
            .SetData(5, vTrnName, "VAN Sales Issue")
            .SetData(5, vTrnHdrTbl, "TrnHdrTDC")
            .SetData(5, vTrnDtlTbl, "TrnDtlTDC")
            .SetData(6, vTrnName, "VAN Sales Receipt")
            .SetData(6, vTrnHdrTbl, "TrnHdrTDCR")
            .SetData(6, vTrnDtlTbl, "TrnDtlTDCR")
            .SetData(7, vTrnName, "VAN Sales")
            .SetData(7, vTrnHdrTbl, "TrnHdrVINV")
            .SetData(7, vTrnDtlTbl, "TrnDtlVINV")
            .SetData(8, vTrnName, "JW ADDN/DEDN ADVANCE")
            .SetData(8, vTrnHdrTbl, "TrnHdrJWAdvance")
            .SetData(8, vTrnDtlTbl, "TrndtlJWAdvance")
            .SetData(9, vTrnName, "Local Sales Order")
            .SetData(9, vTrnHdrTbl, "TrnHdrLSO")
            .SetData(9, vTrnDtlTbl, "TrnDtlLSO")
            .SetData(10, vTrnName, "Export Invoice")
            .SetData(10, vTrnHdrTbl, "TrnHdrExpInv")
            .SetData(10, vTrnDtlTbl, "TrnDtlExpInv")
            .SetData(11, vTrnName, "Dispatch Memo")
            .SetData(11, vTrnHdrTbl, "TrnHDrDM")
            .SetData(11, vTrnDtlTbl, "TrnDtlDM")
            .SetData(12, vTrnName, "Packing Slip")
            .SetData(12, vTrnHdrTbl, "TrnHDrPS")
            .SetData(12, vTrnDtlTbl, "TrnDtlPS")
            .SetData(13, vTrnName, "Proforma Invoice")
            .SetData(13, vTrnHdrTbl, "TrnHDrProInv")
            .SetData(13, vTrnDtlTbl, "TrnDtlProInv")
            .SetData(14, vTrnName, "Purchse Order-For PRS")
            .SetData(14, vTrnHdrTbl, "TrnHDrPOPRS")
            .SetData(14, vTrnDtlTbl, "TrnDtlPOPRS")
            .SetData(15, vTrnName, "Receipts")
            .SetData(15, vTrnHdrTbl, "TrnHdrRcptChem")
            .SetData(15, vTrnDtlTbl, "TrnDtlRcptChem")
            .SetData(16, vTrnName, "Stock Issue Note")
            .SetData(16, vTrnHdrTbl, "TrnHdrSIN")
            .SetData(16, vTrnDtlTbl, "TrnDtlSIN")
            .SetData(17, vTrnName, "Contra Entry")
            .SetData(17, vTrnHdrTbl, "TrnHdrContra")
            .SetData(17, vTrnDtlTbl, "TrnDtlContra")
            .SetData(18, vTrnName, "Misc Issue Note")
            .SetData(18, vTrnHdrTbl, "TrnHdrMiscIssue")
            .SetData(18, vTrnDtlTbl, "TrnDtlMiscIssue")
            .SetData(19, vTrnName, "Goods Receipt Note against PO")
            .SetData(19, vTrnHdrTbl, "TrnHdrPOGRN")
            .SetData(19, vTrnDtlTbl, "TrnDtlPOGRN")
            .SetData(20, vTrnName, "Goods Receipt Note Without PO")
            .SetData(20, vTrnHdrTbl, "TrnHdrGrnWOPO")
            .SetData(20, vTrnDtlTbl, "TrnDtlGrnWOPO")
            .SetData(21, vTrnName, "Receipts/Purchase")
            .SetData(21, vTrnHdrTbl, "TrnHdrPurchase")
            .SetData(21, vTrnDtlTbl, "TrnDtlPurchase")
            .SetData(22, vTrnName, "Local Sales Invoice")
            .SetData(22, vTrnHdrTbl, "TrnHdrLINV")
            .SetData(22, vTrnDtlTbl, "TrnDtlLINV")
            .SetData(23, vTrnName, "Item Comments Entry")
            .SetData(23, vTrnHdrTbl, "TrnHdrIC")
            .SetData(23, vTrnDtlTbl, "TrnDtlIC")

        End With
        optMasters.Checked = True
    End Sub
    Private Function ImportHdrDtlData(argTran As String, argHdrTblname As String, argdtlTblName As String, ByVal sTrans As OleDbTransaction) As Boolean
        Dim Strsql As String
        ImportHdrDtlData = False

        Strsql = "Truncate Table " & argHdrTblname
        If Not clsSVT.execSQLStatement(Strsql, sTrans) Then
            ImportHdrDtlData = False
            Exit Function
        End If

        Strsql = "Truncate Table " & argdtlTblName
        If Not clsSVT.execSQLStatement(Strsql, sTrans) Then
            ImportHdrDtlData = False
            Exit Function
        End If

        Select Case UCase(argTran)
            Case "PURCHASE ORDER"
                Strsql = " insert into TrnHdrPO(PONO,Prefix,PrintPONO,PODate,Ammendment,AmmDetails,AmmNo,AmmDate,"
                Strsql = Strsql & " SuppID,PartyName,PartyAdd1,PartyAdd2,PartyAdd3,PartyCity,PartyState,PartyPinCode,PartyCountry,"
                Strsql = Strsql & " Authorised,AuthBy,AuthDate,Editedby,EditedOn,Cancelled,AddedBy,AddedOn,"
                Strsql = Strsql & " Narr,SORef,TotGVal,TotDiscAmt,TotCGSTAmt,TotSGSTAmt,TotIGSTAmt,GrandTot)"
                Strsql = Strsql & " select PONO,''as Prefix,PONO as PrintPONO,PODate,'' as Ammendment,'' as AmmDetails,0 as AmmNo,Null as AmmDate,SuppID,"
                Strsql = Strsql & " '' as PartyName, '' as PartyAdd1,'' as PartyAdd2,'' as PartyAdd3,'' as PartyCity, ''PartyState,''PartyPinCode, '' as PartyCountry,"
                Strsql = Strsql & " 'N' as Authorised,'' as AuthBy, null as AuthDate,'' as EditedBy,Null as EditedOn,'N' as Cancelled,"
                Strsql = Strsql & " isnull((select MstUser.UserCode from SacInstk..MstUser where MstUser.UserID=h.UserCode),'') as AddedBy,PoDate as AddedOn,"
                Strsql = Strsql & " Narr,SORef,TotGValue as TotGVal,DiscValue as TotDiscAmt,CGSTVal as TotCGSTamt,SGSTval as TotSGSTamt,IGSTval as TotIGSTamt,"
                Strsql = Strsql & " (TotGValue+CGSTVal+SGSTVal+IGSTval-DiscValue) as GrandTot from SacInstk..TrnHdrPO h"
                If Not clsSVT.execSQLStatement(Strsql, sTrans) Then
                    ImportHdrDtlData = False
                    Exit Function
                End If

                Strsql = " Insert into TrnDtlPO(POID,SlNo,ItemiD,ItemSlno,Shortclosed,ShortclosedBy,ShortClosedDt,"
                Strsql = Strsql & " POQty,PORate,Packets,SchDate,GoodsVal,DiscPer,DiscAmt,CGSTPer,CGSTAmt,SGSTPer,SGSTAmt,IGSTPer,IGSTAmt)"
                Strsql = Strsql & " select "
                Strsql = Strsql & " (select POID from TrnHdrPO where TrnHdrPo.PONO=h.PONO) as POID,"
                Strsql = Strsql & " SlNo"
                Strsql = Strsql & " ,v.ItemID,"
                Strsql = Strsql & " (row_number() over(partition by d.PONO order by d.PONO)) as ItemSlNO,"
                Strsql = Strsql & " 'N' as ShortClosed,0 as ShortClosedBy,null as shortClosedDT,"
                Strsql = Strsql & " Qty,PORate,Packets,v.Schdate,GoodsValue,DiscPer,(GoodsValue*DiscPer/100) as DiscAmt,"
                Strsql = Strsql & "  CGSTPer, d.CGSTval as CGSTAmt, SGSTPer, d.SGSTVal as SGSTAmt, IGSTPer, d.IGSTval as IGSTAmt "
                Strsql = Strsql & " from("
                Strsql = Strsql & " select d.PONO as PONO,d.ItemID as ItemID ,d.SchDate as SchDate,POQty as Qty from SacInstk..TrnDtlPO d, SacInstk..TrnSchPO s "
                Strsql = Strsql & " where(d.PONO = s.PONO)"
                Strsql = Strsql & " and d.ItemId=s.ItemId"
                Strsql = Strsql & " and d.SchDate=s.SchDate"
                Strsql = Strsql & " and d.POQty=s.SchQty"
                Strsql = Strsql & " UNION all"
                Strsql = Strsql & " select s.PONO as PONO,s.ItemID as ItemID,s.SchDate as SchDate,SchQty as Qty from SacInstk..TrnDtlPO d, SacInstk..TrnSchPO s "
                Strsql = Strsql & "  where(d.PONO = s.PONO)"
                Strsql = Strsql & " and d.ItemId=s.ItemId"
                Strsql = Strsql & " and d.SchDate<>s.SchDate"
                Strsql = Strsql & " and d.POQty<>s.SchQty"
                Strsql = Strsql & " ) v, SacInstk..TrnDtlPO d,SacInstk..TrnHdrPO h"
                Strsql = Strsql & " where(d.PONO = h.PONO)"
                Strsql = Strsql & " and v.PONO=d.PONO"
                Strsql = Strsql & " and v.ItemID=d.ItemID"
                If Not clsSVT.execSQLStatement(Strsql, sTrans) Then
                    ImportHdrDtlData = False
                    Exit Function
                End If

                Strsql = "update TrnHdrPO set PONO=Replace(PONO,'/',0), PrintPONO=Replace(PONO,'/',0)"
                If Not clsSVT.execSQLStatement(Strsql, sTrans) Then
                    ImportHdrDtlData = False
                    Exit Function
                End If

            Case "JW PURCHASE ORDER"
                Strsql = " insert into TrnHdrJWPO(JWPONO,Prefix,PrintJWPONO,JWPODate,SuppID,"
                Strsql = Strsql & " PartyName,PartyAdd1,PartyAdd2,PartyAdd3,PartyCity,PartyState,PartyPinCode,PartyCountry,"
                Strsql = Strsql & " Authorised,AuthBy,AuthDate,Editedby,EditedOn,Cancelled,AddedBy,AddedOn,"
                Strsql = Strsql & " Narr,SORef,TotAMT,TotDiscAmt,TotCGSTAmt,TotSGSTAmt,TotIGSTAmt,GrandTot)"

                Strsql = Strsql & " select JWPONO,'J' as Prefix,JWPONO as PrintJWPONO,JWPODate,SuppID,"
                Strsql = Strsql & " '' as PartyName, '' as PartyAdd1,'' as PartyAdd2,'' as PartyAdd3,'' as PartyCity, ''PartyState,''PartyPinCode, '' as PartyCountry,"
                Strsql = Strsql & " 'N' as Authorised,'' as AuthBy, null as AuthDate,'' as EditedBy,Null as EditedOn,'N' as Cancelled,"
                Strsql = Strsql & " isnull((select MstUser.UserCode from SacInstk..MstUser where MstUser.UserID=h.UserCode),'') as AddedBy,JWPoDate as AddedOn,"
                Strsql = Strsql & " Narr,SORef,TotGValue as TotAMT,DiscValue as TotDiscAmt,CGSTVal as TotCGSTamt,SGSTval as TotSGSTamt,IGSTval as TotIGSTamt,"
                Strsql = Strsql & " (TotGValue+CGSTVal+SGSTVal+IGSTval-DiscValue) as GrandTot from SacInstk..TrnHdrJWPO h"
                clsSVT.execSQLStatement(Strsql, sTrans)

                Strsql = "Insert into TrnDtlJWPO(JWPOID,SlNo,ItemiD,ItemSlno,Shortclosed,ShortclosedBy,ShortClosedDt,"
                Strsql = Strsql & " Packets,SchDate,JWPORate,JWPOQty,DiscPer,CGSTPer,CGSTAmt,SGSTPer,SGSTAmt,IGSTPer,IGSTAmt,BoardSizeID,GoodsAmt)"

                Strsql = Strsql & " select distinct(select JWPOID from TrnHdrjwPO where TrnHdrJWPo.jWPONO=d.JWPONO) as JWPOID,"
                Strsql = Strsql & " Slno,ItemId,"
                Strsql = Strsql & " (row_number() over(partition by d.JWPONO order by d.JwPONO)) as ItemSlNO,"

                Strsql = Strsql & " Shortclosed,0 as ShortclosedBy,null as ShortclosedDt,"
                Strsql = Strsql & " Packets, schDate, PORate, POqty, DiscPer, CGSTper, d.CGSTVal, SGSTPer, d.SGSTVal, IGSTPer, d.IGSTval, BoardSizeID, GoodsValue"
                Strsql = Strsql & " from SacInstk..TrnDtljwpo d,SacInstk..TrnHdrJWPo h"
                Strsql = Strsql & " where(d.jwpoNO = h.JwpoNO)"

                clsSVT.execSQLStatement(Strsql, sTrans)

                Strsql = "update TrnHdrjwpo set jwpono =SUBSTRING (Jwpono,1,3) + '0' + SUBSTRING(Jwpono,4,5) "
                clsSVT.execSQLStatement(Strsql, sTrans)

            Case "JW PRODUCTION ENTRY"
                Strsql = " insert into TrnHdrJWPrdn(JWPrdnNO,Prefix,PrintJWPrdnNO,JWPrdnDate,"
                Strsql = Strsql & " Authorised, AuthBy, AuthDate, Editedby, EditedOn, Cancelled,"
                Strsql = Strsql & " AddedBy, AddedOn, Rmks)"
                Strsql = Strsql & " select Docno as JWPRDNNo,'' as Prefix,'' as PrintJWPRDNNO,DocDate,"
                Strsql = Strsql & " 'N' as Authorised,'' as AuthBy, null as AuthDate,'' as EditedBy,Null as EditedOn,'N' as Cancelled,"
                Strsql = Strsql & " '' as Addedby,docdate as AddedOn,"
                Strsql = Strsql & " Remarks as RMKS from SacInstk..TrnHdrJWPRDN h"
                clsSVT.execSQLStatement(Strsql, sTrans)

                Strsql = " Insert into TrnDtlJWPRDN(JWPRDNID,SlNo,ItemSlno,JWID,RMITEMID,FGITEMID,QTY,RATE,TIME)"
                Strsql = Strsql & " select "
                Strsql = Strsql & " (select JWPRDNID from TrnHdrJWPRDN where TrnHdrJWPRDN.JWPRDNNO=h.DocNO) as JWPRNID,"
                Strsql = Strsql & " SlNo,"
                Strsql = Strsql & " (row_number() over(partition by d.DocNO order by d.DocNO) )as ItemSlNO,"
                Strsql = Strsql & " jwid,RmItemId,FgItemID,Qty,RAte,JobTime as Time"
                Strsql = Strsql & " from SacInstk..TrnDtlJWPRDN d,SacInstk..TrnHdrJWPRDN h"
                Strsql = Strsql & " where(d.DocNO = h.DocNO)"
                clsSVT.execSQLStatement(Strsql, sTrans)

                Strsql = "update TrnHdrJWPRDN set JWPRDNNO=Replace(JWPRDNNO,'/',0), PrintJWPRDNNO=Replace(JWPRDNNO,'/',0)"
                clsSVT.execSQLStatement(Strsql, sTrans)

            Case "ISSUE FROM JOBWORK"
                Strsql = " insert into TrnHdrIssue(IssueNo,Prefix,PrintIssueNo,IssueDate,"
                Strsql = Strsql & " Authorised, AuthBy, AuthDate, Editedby, EditedOn, Cancelled,"
                Strsql = Strsql & " AddedBy, AddedOn, Narration)"
                Strsql = Strsql & " select  IssNo as IssueNo,'ATM' as Prefix,'' as PrintIssueNo,IssDate as IssueDate,"
                Strsql = Strsql & " 'N' as Authorised,'' as AuthBy, null as AuthDate,'' as EditedBy,Null as EditedOn,'N' as Cancelled,"
                Strsql = Strsql & " isnull((select MstUser.UserCode from SacInstk..MstUser where MstUser.UserID=h.UserCode),'') as AddedBy,"
                Strsql = Strsql & " IssDate as AddedOn,Remarks as Narration from sacinstk..TrnHdrISSUe h  "
                clsSVT.execSQLStatement(Strsql, sTrans)

                'Strsql = " Insert Into TrndtlIssue (IssueID,Slno,ItemId,ITemSlno,Shortclosed,ShortclosedBy,ShortClosedDt,Qty,Rate) "
                'Strsql = Strsql & " select (select IssueID from TrnHdrIssue where convert(int,Right(IssueNo,5))in(select IssNo from Sacinstk..TrnHDrIssue"
                'Strsql = Strsql & " where Sacinstk..TrnHdrIssue.Issid=d.Issid)) as IssueID"
                'Strsql = Strsql & " ,Slno,ItemId,"
                'Strsql = Strsql & " (row_number() over(partition by d.IssId order by d.IssId)) as ItemSlNO, 'N' as ShortClosed,0 as ShortClosedBy,"
                'Strsql = Strsql & " null as shortClosedDT,Issqty as Qty,IssRate as Rate "
                'Strsql = Strsql & " from sacinstk..TrnDtlIssue d"
                'clsSVT.execSQLStatement(Strsql, sTrans)

            Case "VAN SALES ISSUE"
                Strsql = " insert into TrnHdrTDC(TDCNO,Prefix,PrintTDCNo,TDCDate, AddedBy, AddedOn, VHCL, TotCtrn, TotDzN, TotVal, Rmks) "

                Strsql = Strsql & " select substring(Convert(varchar(4),year(TDCDAte)),3,2) +  right('00000'+Convert(varchar(5),TDCNO),5) as TDCNO,"
                Strsql = Strsql & " '' as Prefix,substring(Convert(varchar(4),year(TDCDAte)),3,2) +  right('00000'+Convert(varchar(5),TDCNO),5) as PrintTDCNO,"
                Strsql = Strsql & " TDCDate, isnull((select MstUser.UserCode from SacInstk..MstUser where MstUser.UserID=h.UserCode),'') as AddedBy, "
                Strsql = Strsql & " TDCDate as AddedOn,VehNo,TotCtrn,TotDZN,TotAmt as Totval,Remarks "
                Strsql = Strsql & " from sacinstk..TrnHdrTDC h "
                clsSVT.execSQLStatement(Strsql, sTrans)

                Strsql = " Insert Into TrnDtlTDC "
                Strsql = Strsql & " (TDCId,slno,ItemID,ItemSlno,QtyPCTRN,Rate,NoCTRN,Amt,DTOTDZN,UOMID)"
                Strsql = Strsql & " Select "
                Strsql = Strsql & " (Select TDCID from TrnHdrTDC "
                Strsql = Strsql & " where Convert(int,right(TDCNO,5)) in (Select TDCNO from SacInstk..TrnHdrTDC "
                Strsql = Strsql & " where SacInstk..TrnHdrTDC.TDCID=h.TDCID))"
                Strsql = Strsql & " ,SlNo,h.ItemID,(row_number() over(partition by TDCID order by h.TDCID)) as ItemSlNO,"
                Strsql = Strsql & " Issqty, IssRate, NoofCtrn, TDCAmt, TotDetDZN, i.UOMID"
                Strsql = Strsql & " from SacInstk..TrnDtlTDC h,mstitem i"
                Strsql = Strsql & " where(h.itemid = i.itemid)"

                clsSVT.execSQLStatement(Strsql, sTrans)
            Case "VAN SALES RECEIPT"

                Strsql = " insert into TrnHdrTDCR(TDCRNO,Prefix,PrintTDCRNo,TDCRDate, AddedBy, AddedOn, VHCL, TotCtrn, TotDzN, TotVal, Rmks) "

                Strsql = Strsql & " select substring(Convert(varchar(4),year(TDCRDAte)),3,2) +  right('00000'+Convert(varchar(5),TDCRNO),5) as TDCRNO"
                Strsql = Strsql & " ,'' as Prefix,substring(Convert(varchar(4),year(TDCRDAte)),3,2) +  right('00000'+Convert(varchar(5),TDCRNO),5) as PrintTDCRNO,"
                Strsql = Strsql & " TDCRDate, isnull((select MstUser.UserCode from SacInstk..MstUser where MstUser.UserID=h.UserCode),'') as AddedBy, "
                Strsql = Strsql & " TDCRDate as AddedOn,VehNo,TotCtrn,TotDZN,TotAmt as Totval,Remarks "
                Strsql = Strsql & " from sacinstk..TrnHdrTDCR h "
                clsSVT.execSQLStatement(Strsql, sTrans)

                Strsql = " Insert into TrnDtlTDCR(TDCRID,Slno,ITemId,ItemSlno,QTYPCTRN,RAte,NoCtrn,Amt,DTOTDZN,UOMID)"
                Strsql = Strsql & " Select "
                Strsql = Strsql & " (Select TDCRID from TrnHdrTDCR "
                Strsql = Strsql & " where Convert(int,right(TDCRNO,5)) in (Select TDCRNO from SacInstk..TrnHdrTDCR "
                Strsql = Strsql & " where SacInstk..TrnHdrTDCR.TDCRID=h.TDCRID))"
                Strsql = Strsql & " ,SlNo,h.ItemID,(row_number() over(partition by TDCRID order by h.TDCRID)) as ItemSlNO,"
                Strsql = Strsql & " Issqty, IssRate, NoofCtrn, TDCRAmt, TotDetDZN, i.UOMID"
                Strsql = Strsql & " from SacInstk..TrnDtlTDCR h,mstitem i"
                Strsql = Strsql & " where(h.itemid = i.itemid)"
                clsSVT.execSQLStatement(Strsql, sTrans)

            Case "VAN SALES"
                Strsql = " insert into TrnHdrVINV(VINVNO,Prefix,PrintVINVNo,VINVDate,CustID"
                Strsql = Strsql & " ,PartyName,PartyAdd1,PartyAdd2,PartyAdd3,PartyCity,PartyState,PartyPinCode,PartyCountry,"
                Strsql = Strsql & " AddedBy,AddedOn,Time,BillNo,GrossAmt,TDISCAmt,TOTCGSTAMT,TOTSGSTAMT,Total,RoundOff,"
                Strsql = Strsql & " GrndTot,Rmks,Cashcollected,Balance,AmtInWrds,DEVID)"

                Strsql = Strsql & " select   STUFF (replace(VINVNO,'/',''),5,3,'') as VINVNO,"
                Strsql = Strsql & " case when substring(VINVNO,4,2)='VA' then 'VA' "
                Strsql = Strsql & " when  substring(VINVNO,4,2)='PR' then 'PR'end as Prefix,"
                Strsql = Strsql & " STUFF (replace(VINVNO,'/',''),5,3,'') as PrintVINVNo,VINVDate,custid,"
                Strsql = Strsql & " '' as PartyName, '' as PartyAdd1,'' as PartyAdd2,'' as PartyAdd3,'' as PartyCity, ''PartyState,''PartyPinCode, '' as PartyCountry,"
                Strsql = Strsql & " 'ADMIN' as AddedBy,VINVDate as AddedOn,VINVTime,BillNo,GrossAmt,DiscAmt as TDISCAmt,CGSTAmt As TotCGSTAmt,SGSTAmt as TotSGSTAmt,"
                Strsql = Strsql & " Total,RNDOffAmt,GrandTot,Remarks as RMKS,CAshCollected,Balance,AmtInWrds,DEvID"
                Strsql = Strsql & " from sacinstk..TrnHdrVINV h "
                clsSVT.execSQLStatement(Strsql, sTrans)


                Strsql = "Insert Into TrndtlVINV (VINVID,Slno,ItemId,ITemSlno,"
                Strsql = Strsql & " Shortclosed,ShortclosedBy,ShortClosedDt,VINVqty,VINVRate,VINVAmt,DiscPer,DiscAmt,NETAmt,"
                Strsql = Strsql & " CGSTPer,CGSTAmt,SGSTPER,SGSTAmt,ITMTOTAmt,UOMID)"
                Strsql = Strsql & " select(select VINVID from TrnHdrVINV where left(TrnHdrVINV.VINVNO,2) +'/'+ substring(TrnHdrVINV.VINVNO,3,2) + '/' +  right('00000000'+substring(TrnHdrVINV.VINVNO,5,5),8) =h.vinvno)as vinvid,"
                Strsql = Strsql & " (row_number() over(partition by d.VINVNO order by d.VINVNO) )as SlNO,ItemId,ItemSlNO,"
                Strsql = Strsql & " 'N' as ShortClosed,0 as ShortClosedBy,null as shortClosedDT,VINVQty,VINVRate,VINVAmt,DiscPEr,d.DiscAmt,NetAmt,"
                Strsql = Strsql & " CGSTPer,d.CGSTAmt,SGSTPer,d.SGSTAmt,NetAmt as ITMTOTAmt,UOMID"
                Strsql = Strsql & " from sacinstk..TrndtlVINV d ,sacinstk ..TrnHdrVINV h"
                Strsql = Strsql & " where(d.VINVNO = h.VINVNO)"
                clsSVT.execSQLStatement(Strsql, sTrans)

            Case "JW ADDN/DEDN ADVANCE"
                Strsql = "insert into TrnHdrJWAdvance(JWAdvanceNO,Prefix,PrintJWAdvanceNo,JWAdvanceDate,"
                Strsql = Strsql & " Authorised,AuthBy,AuthDate,Editedby,EditedOn,Cancelled,"
                Strsql = Strsql & " AddedBy,AddedOn,TOTDEDN,TOTADDITION)"
                Strsql = Strsql & " select  DocNo as JWAdvanceNO,replace(DocNo,'/','')  as Prefix,'' as PrintJWAdvanceNO,DocDate as JWAdvanceDate,"
                Strsql = Strsql & " 'N' as Authorised,'' as AuthBy, null as AuthDate,'' as EditedBy,Null as EditedOn,'N' as Cancelled,"
                Strsql = Strsql & " 'ADMIN' as AddedBy,DocDate as AddedOn,TotDednAmount as TotDEDN,TotAddAmount as TotAddition"
                Strsql = Strsql & " from sacinstk..TrnHdrJWAdvance h "
                clsSVT.execSQLStatement(Strsql, sTrans)

                Strsql = "update TrnHdrJWAdvance set JWAdvanceNO=Replace(JWAdvanceNO,'/',0), PrintJWAdvanceNO=Replace(JWAdvanceNO,'/',0)"
                clsSVT.execSQLStatement(Strsql, sTrans)


                Strsql = " Insert Into TrnDtlJWAdvance (JWAdvanceID,Slno,ITemSlno,JWID,"
                Strsql = Strsql & " Amount,Remarks,AddOrDEDN)"
                Strsql = Strsql & " select (select JWAdvanceID from TrnHdrJWAdvance where TrnHdrJWAdvance.JWAdvanceNO=replace(d.DocNo,'/','0')) as JWAdvanceID,d.Slno,"
                Strsql = Strsql & " (row_number() over(partition by replace(d.docno,'/','0') order by d.docno) )as ItemSlNO,d.JWID,"
                Strsql = Strsql & " d.AdvAmt as Amount,d.Remarks,d.AddOrDedn"
                Strsql = Strsql & " from sacinstk..TrnDtlJWAdvance d ,Sacinstk..TrnHdrJWAdvance h"
                Strsql = Strsql & " where(d.DocNo = h.DocNo)"
                clsSVT.execSQLStatement(Strsql, sTrans)

            Case "LOCAL SALES ORDER"
                Strsql = " insert into TrnHdrLSO(LSONO,Prefix,PrintLSONO,LSODate,CustID,STID,AGENTID,Bank,"
                Strsql = Strsql & " PartyName,PartyAdd1,PartyAdd2,PartyAdd3,PartyCity,PartyState,PartyPinCode,PartyCountry,"
                Strsql = Strsql & " Authorised,AuthBy,AuthDate,Editedby,EditedOn,Cancelled,AddedBy,AddedOn,"
                Strsql = Strsql & " GrosAmt,Discper,DiscAmt,NetAmt,GrdTot,Narration,TotNoCtns,Destin,CGSTPer,CGSTamt,SGSTPer,SGSTAmt,IGSTPer,IGSTAmt)"
                Strsql = Strsql & " select stuff (replace(LSONo,'/',''),6,0,0)as LSONO ,"
                Strsql = Strsql & " case when substring(lsono,4,3) ='LOC' then 'LOC'"
                Strsql = Strsql & " else '' end as Prefix,"
                Strsql = Strsql & " stuff (replace(LSONo,'/',''),6,0,0) as PrintLSONO,LSODate,CustID,STID,AgentID,Bank,"
                Strsql = Strsql & " '' as PartyName, '' as PartyAdd1,'' as PartyAdd2,'' as PartyAdd3,'' as PartyCity, ''PartyState,''PartyPinCode, '' as PartyCountry,"
                Strsql = Strsql & " 'N' as Authorised,'' as AuthBy, null as AuthDate,'' as EditedBy,Null as EditedOn,'N' as Cancelled,"
                Strsql = Strsql & " 'ADMIN'as AddedBy,LSODate as AddedOn,"
                Strsql = Strsql & " Grossamt, Discper, DiscAmt, NetAmt, GrandTot, Narration, TotNoOfCtns, Destination,"
                Strsql = Strsql & " CGSTPer, CGSTAmt, SGSTPer, SGSTAmt, Igstper, IGSTAmt"
                Strsql = Strsql & " from SacInstk..TrnHdrLSO h"
                clsSVT.execSQLStatement(Strsql, sTrans)

                Strsql = " Insert into TrnDtlLSO(LSOID,SlNo,ItemiD,UOMID,ItemSlno,LSOQty,Shortclosed,ShortclosedBy,ShortClosedDt,"
                Strsql = Strsql & " Rate,Amt,NoCortns,TotDOZ,DRMKS)"
                Strsql = Strsql & " select (select LSOID from TrnHdrLSO where TrnHdrLSO.LSONO=stuff (replace(d.LSONo,'/',''),6,0,0)) as LSOID,"
                Strsql = Strsql & " Slno,ItemId,UOMID,"
                Strsql = Strsql & " (row_number() over(partition by d.LSONO order by d.LSONO)) as ItemSlNO,SOQty,"
                Strsql = Strsql & " Shortclosed,0 as ShortclosedBy,null as ShortclosedDt,"
                Strsql = Strsql & " Rate,Amount as Amt,NoofCtns,TotDOZ ,Remarks as DRmks"
                Strsql = Strsql & " from SacInstk..TrnDtlLSO d,SacInstk..TrnHdrLSO h"
                Strsql = Strsql & " where(d.LSoNO = h.LSoNO)"
                clsSVT.execSQLStatement(Strsql, sTrans)

            Case "LOCAL SALES INVOICE"

                Strsql = "Insert InTo TrnHdrLInv( LInvNo,Prefix,PrintLInvNO,LInvDate,"
                Strsql = Strsql & " CustId,PartyName,PartyAdd1,PartyAdd2,PartyAdd3,PartyCity,PartyState,PartyPinCode,"
                Strsql = Strsql & " PartyCountry,Authorised,AuthBy,AuthDate,Editedby,EditedOn,Cancelled,AddedBy,AddedOn,"
                Strsql = Strsql & " STID,AgentId,TransporterID,Bank,Narration,GrossAmt,"
                Strsql = Strsql & " splDiscAmt,TotalDisc,NetAmt,FrtAmt,Postalchg,"
                Strsql = Strsql & " RoundOffAmt,GRANDTOT,CreditDays,DueDate,LRNO,LRDate,FormIssue,PermitNo,TruckNo,"
                Strsql = Strsql & " MobileNo,AmtInWrds,OrderNos,OrderDate,TotCtns,TotalfreeDoz,intrRmks,ESugamNo,"
                Strsql = Strsql & " vehicleNo,InvTime,TDiscAmt,SaleAmt,PrintInvNo,PrintInvOldNo,CGSTPer,CGSTAmt,SGSTPer,SGSTAmt,IGSTPer,IGSTAmt,HSNCode )"

                Strsql = Strsql & " select  replace(LINVNO,'/','')as LInvNo,'' as Prefix, "
                Strsql = Strsql & " replace(LINVNO,'/','')  as PrinLInvNo,LINVDate as LINVDate,"
                Strsql = Strsql & " Custid, '' as PartyName, '' as PartyAdd1,'' as PartyAdd2,'' as PartyAdd3,'' as PartyCity, ''PartyState,''PartyPinCode,"
                Strsql = Strsql & " '' as PartyCountry, 'N' as Authorised,'' as AuthBy, null as AuthDate,'' as EditedBy,Null as EditedOn,'N' as Cancelled,"
                Strsql = Strsql & " 'ADMIN' as AddedBy,LINVDate as AddedOn,"
                Strsql = Strsql & " STID, AgentId, TransporterID, Bank, Remarks, GrossAmt,"
                Strsql = Strsql & " SplDiscAmt, TotalDiscAmt, NetAmt, FrtAmt, Postalcharge,"
                Strsql = Strsql & " RndOffAmt, InvValue, CreditDays, DueDate, LRNO, LRDate, FormIssue, PermitNo, TruckNo,"
                Strsql = Strsql & " MobileNo, AmountInWords, OrderNos, OrderDates, TotCtns, TotfreeDzns, intrRemarks, ESugamNo,"
                Strsql = Strsql & " vechNo, InvTime, TDiscAmt, SaleAmt, PrintInvNo, PrintInvNoOld, CGSTPer, CGSTAmt, SGSTPer, SGSTAmt, IGSTPer, IGSTAmt, HSNCode"
                Strsql = Strsql & " from sacinstk..TrnHdrLInv h"
                clsSVT.execSQLStatement(Strsql, sTrans)

            Case "EXPORT INVOICE"
                Strsql = " insert into TrnHdrExpInv(ExpInvNO,Prefix,PrintExpInvNO,ExpInvDate,TallyRefNo,CustID,Agent,"
                Strsql = Strsql & " PartyName,PartyAdd1,PartyAdd2,PartyAdd3,PartyCity,PartyState,PartyPinCode,PartyCountry,"
                Strsql = Strsql & " AddedBy,AddedOn,"
                Strsql = Strsql & " ExportInvNo,ExporterID,Hsncode,OrdREfNo,OrderNo,OrderDate,TotNoCRTNS,Form,DispTched,"
                Strsql = Strsql & " GCNo,Dated,Vehicleno,DueDate,BLNO,HFORMNO,SaleLedID,PerMiTNo,PolicyNo,BillOfLeading,Rmks,"
                Strsql = Strsql & " Daddress1,Daddress2,Daddress3,BAsicAmt,IGSTPer,IGSTAmt,RoundOffAcc,Total,"
                Strsql = Strsql & " SALEAmt,RoundofAmt,State,TotAmtInWords,HBRcvd)"

                Strsql = Strsql & " select stuff (replace(ExpInvNO,'/',''),6,1,0) ExpInvNO,"
                Strsql = Strsql & " case when substring(ExpInvNO,4,3) ='EXP' then 'EXP'"
                Strsql = Strsql & " when substring(ExpInvNO,4,3) ='GST' then 'GST'"
                Strsql = Strsql & " end as Prefix,"
                Strsql = Strsql & " stuff (replace(ExpInvNO,'/','') ,6,1,0) as PrintExpInvNO,ExpInvDate,stuff (replace(ExpInvNO,'/',''),6,1,0) as TallyRefno,CustID,Agent,"
                Strsql = Strsql & " '' as PartyName, '' as PartyAdd1,'' as PartyAdd2,'' as PartyAdd3,'' as PartyCity, ''PartyState,''PartyPinCode, '' as PartyCountry,"
                Strsql = Strsql & " isnull((select MstUser.UserCode from SacInstk..MstUser where MstUser.UserID=h.UserCode),'') as AddedBy,ExpInvDate as AddedOn,"
                Strsql = Strsql & " ExpinvNoUser, Expid, HSNCode, OrderRefNo, OrderNo, OrderDate, NoofCases, Form, DispatchThru,"
                Strsql = Strsql & " GCNumber, GCNumDate, VehicleNo, DueDate, BLNo, HFORMNo, SaleLedID, permitNo, PolicyNo, BillofLeading, remarks,"
                Strsql = Strsql & " Daddress1, Daddress2, Daddress3, BasicAmt, Igstper, IGSTAmt, Roundaccid, SubTotal,"
                Strsql = Strsql & " TotGvalue, TotRoundoffValue, State, TotAmtInWords, HBRcvd"
                Strsql = Strsql & " from SacInstk..TrnHdrExpInv h"
                clsSVT.execSQLStatement(Strsql, sTrans)

                Strsql = " Insert into TrnDtlExpInv(ExpInvID,SlNo,ItemiD,ItemSlno,Shortclosed,ShortclosedBy,ShortClosedDt,"
                Strsql = Strsql & " DMID,StkExpInvQty,Rate, Amount,OldDtlInvNo)"

                Strsql = Strsql & " select (select ExpInvID from TrnHdrExpInv where TrnHdrExpInv.ExpInvNO=stuff (replace(h.ExpInvNO,'/',''),6,1,0) ) as ExpInvID,"
                Strsql = Strsql & " Slno,ItemId,"
                Strsql = Strsql & " (row_number() over(partition by d.ExpInvNO order by d.ExpInvNO)) as ItemSlNO,"
                Strsql = Strsql & " 'N' as Shortclosed,0 as ShortclosedBy,null as ShortclosedDt,"
                Strsql = Strsql & " replace(h.DMNO,'/',0)  as DMID,ExpInvQty,Rate,GoodsValue as Amount,d.OldInvNo"
                Strsql = Strsql & " from SacInstk..TrnDtlExpInv d,SacInstk..TrnHdrExpInv h"
                Strsql = Strsql & " where(d.ExpInvNO = h.ExpInvNO)"
                clsSVT.execSQLStatement(Strsql, sTrans)

            Case "DISPATCH MEMO"

                Strsql = " insert into TrnHdrDM(DMNO,Prefix,PrintDMNO,DMDate,CustID,"
                Strsql = Strsql & " PartyName,PartyAdd1,PartyAdd2,PartyAdd3,PartyCity,PartyState,PartyPinCode,PartyCountry,"
                Strsql = Strsql & " Authorised,AuthBy,AuthDate,Editedby,EditedOn,Cancelled,AddedBy,AddedOn,"
                Strsql = Strsql & " InvNo,InvDAte,OrderSpec,Rmks,ShowINInv,SONO,SOStatus)"


                Strsql = Strsql & " select replace(DMNO,'/',0) as DMNO,'' as Prefix,replace(DMNO,'/',0)  as PrintDMNO,DMDate,CustID,"
                Strsql = Strsql & " '' as PartyName, '' as PartyAdd1,'' as PartyAdd2,'' as PartyAdd3,'' as PartyCity, ''PartyState,''PartyPinCode, '' as PartyCountry,"
                Strsql = Strsql & " 'N' as Authorised,'' as AuthBy, null as AuthDate,'' as EditedBy,Null as EditedOn,'N' as Cancelled,"
                Strsql = Strsql & " 'ADMIN' as AddedBy,DMDate as AddedOn,"
                Strsql = Strsql & " INVNo, InvDate, OrderSpec, Remarks, ShowinInvoice, replace(SONO,'/',0) as SONO, SOStatus"
                Strsql = Strsql & " from SacInstk..TrnHdrDM h"
                clsSVT.execSQLStatement(Strsql, sTrans)


                Strsql = " Insert into TrnDtlDM(DMID,SlNo,ItemiD,ItemSlno,Shortclosed,ShortclosedBy,ShortClosedDt,"
                Strsql = Strsql & " Rate, Dispctns,Noofctns,SOqty,DispQty,Remarks,SOID)"

                Strsql = Strsql & " select (select DMID from TrnHdrDM where TrnHdrDM.DMNO=replace( h.DMNO,'/',0)) as DMID,"
                Strsql = Strsql & " Slno,ItemId,"
                Strsql = Strsql & " (row_number() over(partition by d.DMNO order by d.DMNO)) as ItemSlNO,"
                Strsql = Strsql & " 'N' as Shortclosed,0 as ShortclosedBy,null as ShortclosedDt,"
                Strsql = Strsql & " Rate,DispCtns,NoofCtns,Soqty,DispQty,Rmks,replace(h.SOno,'/','0') as sono"
                Strsql = Strsql & " from SacInstk..TrnDtlDM d,SacInstk..TrnHDrDM h"
                Strsql = Strsql & " where(d.DMNO = h.DMNO)"
                clsSVT.execSQLStatement(Strsql, sTrans)

            Case "PACKING SLIP"
                Strsql = " insert into TrnHdrPS(PSNO,Prefix,PrintPSNO,PSDate,CustID,"
                Strsql = Strsql & " PartyName,PartyAdd1,PartyAdd2,PartyAdd3,PartyCity,PartyState,PartyPinCode,PartyCountry,"
                Strsql = Strsql & " AddedBy,AddedOn,InvNo,invDt,"
                Strsql = Strsql & " BRANCHID,SoStatus,RMKS)"
                Strsql = Strsql & " select replace(PSNO,'/','0')as PSNO,'' as Prefix,replace(PSNO,'/','0')as PrintPSNO,PSDate,CustID,"
                Strsql = Strsql & " '' as PartyName, '' as PartyAdd1,'' as PartyAdd2,'' as PartyAdd3,'' as PartyCity, ''PartyState,''PartyPinCode, '' as PartyCountry,"
                Strsql = Strsql & " isnull((select MstUser.UserCode from SacInstk..MstUser where MstUser.UserID=h.UserCode),'') as AddedBy,PSDate as AddedOn,"
                Strsql = Strsql & " InvNo, InvDate, BranchID, SoStatus, Remarks"
                Strsql = Strsql & " from SacInstk..TrnHdrPS h"
                clsSVT.execSQLStatement(Strsql, sTrans)

                Strsql = " insert into TrndtlPS(PSID ,Slno,ItemID,ItemSlNo,SOID,NoofCTNS,Start,Ending)"
                Strsql = Strsql & " select	"
                Strsql = Strsql & " (select PSID from TrnHdrPS where TrnHdrPS.PSNO=replace( h.PSNO,'/',0)) as PSID,"
                Strsql = Strsql & " slno,ItemId,"
                Strsql = Strsql & " (row_number() over(partition by d.PSNO order by d.PSNO)) as ItemSlNO,"
                Strsql = Strsql & " replace(h.SOno,'/','0') as SONO,NoofCtns,CtnFrom,CtnTo"
                Strsql = Strsql & " from SacInstk..TrnDtlPS d,SacInstk..TrnHDrPS h"
                Strsql = Strsql & " where(d.PSNO = h.PSNO)"
                clsSVT.execSQLStatement(Strsql, sTrans)

            Case "PROFORMA INVOICE"
                Strsql = " insert into TrnHdrProInv(ProInvNO,Prefix,PrintProInvNO,ProInvdate,CustID, "
                Strsql = Strsql & " PartyName,PartyAdd1,PartyAdd2,PartyAdd3,PartyCity,PartyState,PartyPinCode,PartyCountry, "
                Strsql = Strsql & " AddedBy,AddedOn,ExporterID,CurId,PaymentTerms,PriceTerms,OtherReference,CountryOfOrigin,COuntryofFinalDest,"
                Strsql = Strsql & " PrecarriageBy,PlaceofRcpt,SaleAmt,OtherAmt,"
                Strsql = Strsql & " OtherAmtType,TotCTNS,TotQty,TotalCBM,PortOfLoad,PortOfDisch,FinalDest,"
                Strsql = Strsql & " TermsOfDelivery,BuyerOrderNo,BuyerOrdDT,Rmks,VesselNo,"
                Strsql = Strsql & " NotifyName,Daddress1,Daddress2,Daddress3,Daddress4,TotAmtInWrds,BranchId) "

                Strsql = Strsql & " select stuff(replace(ProInvNO,'/',''),6,0,0) as ProInvNO,"
                Strsql = Strsql & " case when substring(PROINVNo,4,3) ='GRP' then 'GRP'"
                Strsql = Strsql & " when substring(PROINVNO,4,3)='SPR'Then 'SPR' end as Prefix,"
                Strsql = Strsql & " stuff(replace(ProInvNO,'/',''),6,0,0)   as PrintProInvNO,ProInvDate,CustID, "
                Strsql = Strsql & " '' as PartyName, '' as PartyAdd1,'' as PartyAdd2,'' as PartyAdd3,'' as PartyCity, ''PartyState,''PartyPinCode, '' as PartyCountry, "
                Strsql = Strsql & " 'ADMIN' as AddedBy,ProInvDate as AddedOn,Expid,CurrId,PaymentTerms,PriceTerms,"
                Strsql = Strsql & " OtherReference, CountryOfOrigin, COuntryofFinalDest, "
                Strsql = Strsql & " PrecarriageBy, PlaceofReceipt, TotValue, OtherAmt,"
                Strsql = Strsql & " OtherAmtType, TotCartons, TotQty, TotCBM, PortofLoading, Portofdischarge, FinalDestination,"
                Strsql = Strsql & " TermsOfDelivery, BuyerOrderNo, BuyerOrderDate, Remarks, VesselFlightNo, NotifyName,"
                Strsql = Strsql & " DAddress1, DAddress2, DAddress3, State, TotAmtInWords, BranchID"
                Strsql = Strsql & " from SacInstk..TrnHdrProInv h"
                clsSVT.execSQLStatement(Strsql, sTrans)


                Strsql = " Insert into TrndtlProInv(ProInvID,SlNo,ItemiD,ItemSlno,NoofCTNS,QtyPerCTNS,Qty,Rate,Amount,"
                Strsql = Strsql & " CBMPERCTN,TOTCBM,CUMCBN)"
                Strsql = Strsql & " select (select ProInvID from TrnHdrProInv where TrnHdrProInv.ProInvNO=stuff(replace(h.ProInvNO,'/',''),6,0,0) ) as ProInvID, "
                Strsql = Strsql & " Slno,ItemId, (row_number() over(partition by d.ProInvNO order by d.ProInvNO)) as ItemSlNO, "
                Strsql = Strsql & " NoofCTns, QtyperCtn, ProInvQty, Rate, GoodsValue, CBMCTN, TOTCBM, CUMCBM"
                Strsql = Strsql & " from SacInstk..TrndtlProInv d,SacInstk..TrnHdrProInv h "
                Strsql = Strsql & " where(d.ProInvNO = h.ProInvNO)"
                clsSVT.execSQLStatement(Strsql, sTrans)

            Case "PURCHSE ORDER-FOR PRS"

                Strsql = " insert into TrnHdrPOPRS(POPRSNO,Prefix,PrintPOPRSNO,POPRSdate,SuppID, "
                Strsql = Strsql & " PartyName,PartyAdd1,PartyAdd2,PartyAdd3,PartyCity,PartyState,PartyPinCode,PartyCountry, "
                Strsql = Strsql & " AddedBy,AddedOn,"
                Strsql = Strsql & " Narration,SOREF,TotGoodsVal,TotDiscVal,TotCGSTVal,TOTSGSTVAL,TOTIGSTVAL) "

                Strsql = Strsql & " select stuff(replace(PONO,'/',''),6,0,0) as PONO,"
                Strsql = Strsql & " '' as Prefix, stuff(replace(PONO,'/',''),6,0,0)   as PrintPONO,PODate,SuppID, "
                Strsql = Strsql & " '' as PartyName, '' as PartyAdd1,'' as PartyAdd2,'' as PartyAdd3,'' as PartyCity, ''PartyState,''PartyPinCode, '' as PartyCountry, "
                Strsql = Strsql & " isnull((select MstUser.UserCode from SacInstk..MstUser where MstUser.UserID=h.UserCode),'') as AddedBy,PoDate as AddedOn,"
                Strsql = Strsql & " Narr, SOREF, TotGvalue, DiscValue, CGSTVAL, SGSTVAL, IGSTVAL"
                Strsql = Strsql & " from SacInstk..TrnHdrPOPRS h"
                clsSVT.execSQLStatement(Strsql, sTrans)

                Strsql = " Insert into TrndtlPOPRS(POPRSID,SlNo,ItemiD,ItemSlno,"
                Strsql = Strsql & " Packets, SchDate, POqty, PORate, GoodsVal, DiscPEr,"
                Strsql = Strsql & " d.CGSTPer, d.CGSTVal, d.SGSTPer, d.SGSTVal, d.IGSTPer, d.IGSTVal)"

                Strsql = Strsql & " select (select POPRSID from TrnHdrPOPRS where TrnHdrPOPRS.POPRSNO=stuff(replace(h.PONO,'/',''),6,0,0))as POPRSID ,"
                Strsql = Strsql & " Slno,ItemId, (row_number() over(partition by d.PONO order by d.PONO)) as ItemSlNO,"
                Strsql = Strsql & " Packets, SchDate, POqty, PORate, GoodsValue, DiscPEr,"
                Strsql = Strsql & " d.CGSTPer, d.CGSTVal, d.SGSTPer, d.SGSTVal, d.IGSTPer, d.IGSTVal"
                Strsql = Strsql & " from SacInstk..TrndtlPOPRS d,SacInstk..TrnHdrPOPRS h "
                Strsql = Strsql & " where(d.PONO = h.PONO)"
                clsSVT.execSQLStatement(Strsql, sTrans)

            Case "RECEIPTS"

                Strsql = " insert into TrnHdrRcptChem(RcptChemNO,Prefix,PrintRcptChemNO,RcptChemdate,SuppID, "
                Strsql = Strsql & " PartyName, PartyAdd1, PartyAdd2, PartyAdd3, PartyCity, PartyState, PartyPinCode, PartyCountry,"
                Strsql = Strsql & " AddedBy, AddedOn,"
                Strsql = Strsql & " Narration, BillNo, BillDate, DiscPer, DiscAMT, SubTot, EDPer, EDAmt, CessPer,"
                Strsql = Strsql & "  CEssAmt, OtherAmt, TotVAl, TAXPer, TAXAmt)"
                Strsql = Strsql & " select replace(DocNo,'/','') as RcptChemNO,"
                Strsql = Strsql & " '' as Prefix, replace(DocNo,'/','')   as PrintRcptChemNO,DOCDate,SuppID, "
                Strsql = Strsql & " '' as PartyName, '' as PartyAdd1,'' as PartyAdd2,'' as PartyAdd3,'' as PartyCity, ''PartyState,''PartyPinCode, '' as PartyCountry, "
                Strsql = Strsql & " isnull((select MstUser.UserCode from SacInstk..MstUser where MstUser.UserID=h.UserCode),'') as AddedBy,DocDate as AddedOn,"
                Strsql = Strsql & " Remarks, BillNo, BillDate, DiscPer, DiscAmount, SubTotal, EDPer, EDAmt, CessPer, CEssAmt, OtherAmt, GrandTotal, TAXPer, TAXAmt"
                Strsql = Strsql & " from SacInstk..TrnHdrRcptChem h"
                clsSVT.execSQLStatement(Strsql, sTrans)


                Strsql = "Insert into TrnDtlRcptChem(RcptChemID,SlNo,ItemiD,ItemSlno,QTY,Rate,Amount)"
                Strsql = Strsql & " select (select RcptChemID from TrnHdrRcptChem where TrnHdrRcptChem.RcptChemNO=replace(h.DocNo,'/',''))as RcptChemID ,"
                Strsql = Strsql & " Slno,ItemId, (row_number() over(partition by d.DocNo order by d.DocNo)) as ItemSlNO,Qty,Rate,Amount"
                Strsql = Strsql & " from SacInstk..TrnDtlRcptChem d,SacInstk..TrnHdrRcptChem h "
                Strsql = Strsql & " where(d.DocNo = h.DocNo)"
                clsSVT.execSQLStatement(Strsql, sTrans)

            Case "STOCK ISSSUE NOTE"
                Strsql = " insert into TrnHdrSIN(SINNO,Prefix,PrintSINNO,SINdate, "
                Strsql = Strsql & " AddedBy,AddedOn,Narration,DEPARTMENT)"
                Strsql = Strsql & " select replace(DocNo,'/','') as SINNO,"
                Strsql = Strsql & " '' as Prefix, replace(DocNo,'/','')   as PrintRcptSINNO,DOCDate, "
                Strsql = Strsql & " isnull((select MstUser.UserCode from SacInstk..MstUser where MstUser.UserID=h.UserCode),'') as AddedBy,DocDate as AddedOn,"
                Strsql = Strsql & " Remarks, dept"
                Strsql = Strsql & " from SacInstk..TrnHdrSIN h"
                clsSVT.execSQLStatement(Strsql, sTrans)

                Strsql = " Insert into TrnDTlSIN(SINID,SlNo,ItemiD,ItemSlno,QTY)"
                Strsql = Strsql & " select (select SINID from TrnHdrSIN  where TrnHdrSIN.SINNO=replace(h.DocNo,'/',''))as SINID ,"
                Strsql = Strsql & " Slno,ItemId, (row_number() over(partition by d.DocNo order by d.DocNo)) as ItemSlNO,Qty"
                Strsql = Strsql & " from SacInstk..TrnDTlSIN d,SacInstk..TrnHdrSIN h "
                Strsql = Strsql & " where(d.DocNo = h.DocNo)"
                clsSVT.execSQLStatement(Strsql, sTrans)

            Case "CONTRA ENTRY"
                Strsql = " insert into TrnHdrContra(ContraNo,Prefix,PrintContraNO,Contradate, "
                Strsql = Strsql & " AddedBy,AddedOn,Narration)"
                Strsql = Strsql & " select replace(DocNo,'/','') as SINNO,"
                Strsql = Strsql & " '' as Prefix, replace(DocNo,'/','')   as PrintRcptSINNO,DOCDate, "
                Strsql = Strsql & " isnull((select MstUser.UserCode from SacInstk..MstUser where MstUser.UserID=h.UserCode),'') as AddedBy,DocDate as AddedOn,"
                Strsql = Strsql & " Remarks"
                Strsql = Strsql & " from SacInstk..TrnHdrAdminContra h"
                clsSVT.execSQLStatement(Strsql, sTrans)


                Strsql = " Insert into TrnDtlContra(ContraID,SlNo,ItemiD,ItemSlno,AddQTY,SubQty)"
                Strsql = Strsql & " select (select ContraID from TrnHdrContra  where TrnHdrContra.ContraNO=replace(h.DocNo,'/',''))as SINID ,"
                Strsql = Strsql & " Slno,ItemId, (row_number() over(partition by d.DocNo order by d.DocNo)) as ItemSlNO,AddQty,SubQty"
                Strsql = Strsql & " from SacInstk..TrnDtlAdminContra d,SacInstk..TrnHdrAdminContra h "
                Strsql = Strsql & " where(d.DocNo = h.DocNo)"
                clsSVT.execSQLStatement(Strsql, sTrans)

            Case "MISC ISSUE NOTE"
                Strsql = " insert into TrnHdrMiscIssue(MiscIssueNo,Prefix,PrintMiscIssueNO,MiscIssuedate, "
                Strsql = Strsql & " AddedBy,AddedOn,Narration,Department)"
                Strsql = Strsql & " select replace(DocNo,'/','') as SINNO,"
                Strsql = Strsql & " '' as Prefix, replace(DocNo,'/','')   as PrintRcptSINNO,DOCDate, "
                Strsql = Strsql & " isnull((select MstUser.UserCode from SacInstk..MstUser where MstUser.UserID=h.UserCode),'') as AddedBy,DocDate as AddedOn,"
                Strsql = Strsql & " Remarks, Dept"
                Strsql = Strsql & " from SacInstk..TrnHdrMiscIssue h"
                clsSVT.execSQLStatement(Strsql, sTrans)

                Strsql = " Insert into TrndtlMiscIssue(MiscIssueID,SlNo,ItemiD,ItemSlno,Qty,Rmks)"
                Strsql = Strsql & " select (select MiscIssueID from TrnHdrMiscIssue  where TrnHdrMiscIssue.MiscIssueNO=replace(h.DocNo,'/',''))as MiscIssueID ,"
                Strsql = Strsql & " Slno,ItemId, (row_number() over(partition by d.DocNo order by d.DocNo)) as ItemSlNO,Qty,d.Remarks"
                Strsql = Strsql & " from SacInstk..TrndtlMiscIssue d,SacInstk..TrnHdrMiscIssue h "
                Strsql = Strsql & " where(d.DocNo = h.DocNo)"
                clsSVT.execSQLStatement(Strsql, sTrans)

            Case "GOODS RECEIPT NOTE AGAINST PO"

                Strsql = "insert into TrnHdrPOGrn(POGrnNO,Prefix,PrintPOGrnNO,POGrnDate,SuppID, "
                Strsql = Strsql & " PartyName,PartyAdd1,PartyAdd2,PartyAdd3,PartyCity,PartyState,PartyPinCode,PartyCountry,"
                Strsql = Strsql & " Authorised,AuthBy,AuthDate,Editedby,EditedOn,Cancelled,AddedBy,AddedOn, "
                Strsql = Strsql & " SuppDCNo,SuppInvNo,SuppRefDate,PressID,TOTALAMT,Rmks,DiscPer,DISCAMT,"
                Strsql = Strsql & " Subtotal,OTHAMT,GrandTotal,Grading1,Grading2,Grading3,"
                Strsql = Strsql & " CGSTPer,CGSTAmt,SGSTPer,SGSTAmt,IGSTPer,IGSTAmt)"

                Strsql = Strsql & " select replace(substring(Convert(varchar(4),year(GrnDate)),3,2)+GRNNO,'/','') as GRNNO ,"
                Strsql = Strsql & " case when substring(GRNNO,1,3)='SAC' then 'SAC' "
                Strsql = Strsql & " else '' end as Prefix,"
                Strsql = Strsql & " replace(substring(Convert(varchar(4),year(GrnDate)),3,2)+GRNNO,'/','') as PrintPOGrnNO,GrnDate,SuppID,"
                Strsql = Strsql & " '' as PartyName, '' as PartyAdd1,'' as PartyAdd2,'' as PartyAdd3,'' as PartyCity, ''PartyState,''PartyPinCode, '' as PartyCountry, "
                Strsql = Strsql & " 'N' as Authorised,'' as AuthBy, null as AuthDate,'' as EditedBy,Null as EditedOn,'N' as Cancelled,"
                Strsql = Strsql & " 'ADMIN' as AddedBy,GrnDate as AddedOn,"
                Strsql = Strsql & " SuppDCNo, SuppInvNo, SuppRefDate, PressID, Total, Rmks, DiscPer, DiscAmount,"
                Strsql = Strsql & " Subtotal, OtherAmt, GrandTotal, Grading1, Grading2, Grading3,"
                Strsql = Strsql & " CGSTPer, CGSTAmt, SGSTPer, SGSTAmt, IGSTPer, IGSTVal"
                Strsql = Strsql & " from SacInstk..TrnHdrPOGrn h"
                clsSVT.execSQLStatement(Strsql, sTrans)

                Strsql = " Insert Into TrnDtlPOGRN(POGrnId,Slno,POID,ItemId,ItemSlno,POItemSlno,"
                Strsql = Strsql & " StkPOGRNQty,PORate,QtySupp,QtyAcc,Excess,Shortage)"
                Strsql = Strsql & " select POGRNID ,d.slno,p.POID ,d.ItemId,"
                Strsql = Strsql & " (row_number() over(partition by d.GRNNO order by d.GRNNO)) as ItemSlNO,"
                Strsql = Strsql & " dp.ItemSLNo as POItemSlno,d.POqty,d.PORate,QtySupp,QtyAcc,Excess,Shortage"
                Strsql = Strsql & " from SacInstk..TrnDtlPOGRN d,TrnDtlPO dp, TrnHdrPO p,TrnHdrPOGRN h"
                Strsql = Strsql & " where(p.POID = dp.POID And dp.ItemID = d.ItemID)"
                Strsql = Strsql & " and substring(p.PONO,1,2) + '/' + right(p.PONO,4) = d.pono"
                Strsql = Strsql & " and  substring(h.POGRNNO,3,3)+'/'+ right(h.POGRNNO,5)=d.GRNNO"
                clsSVT.execSQLStatement(Strsql, sTrans)
            Case "GOODS RECEIPT NOTE WITHOUT PO"
                Strsql = " Insert InTo TrnHdrGRNWOPO(GRNWOPONo,Prefix,PrintGRNWOPONO,GRNWOPODate,"
                Strsql = Strsql & " SuppID,PartyName,PartyAdd1,PartyAdd2,PartyAdd3,PartyCity,PartyState,PartyPinCode,"
                Strsql = Strsql & " PartyCountry,Authorised,AuthBy,AuthDate,Editedby,EditedOn,Cancelled,AddedBy,AddedOn,"
                Strsql = Strsql & " SuppDcNo,SuppInvNo,SuppREfDate,PressID,Rmks,Total,SubTot,CGSTPer,CGSTAmt,SGSTPer,"
                Strsql = Strsql & " SGSTAmt,IGSTPer,IGSTAmt,OtherAmt,TotVal,DiscPer,DiscAmt)"
                Strsql = Strsql & " select   substring(Convert(varchar(4),year(GRNDate)),3,2)+'0'+GRNNO as GRNWOPONo,'' as Prefix, "
                Strsql = Strsql & " substring(Convert(varchar(4),year(GRNDate)),3,2)+'0'+GRNNO  as PrinGRNWOPONO,GRNdate as GRNWOPODate,"
                Strsql = Strsql & " SuppID, '' as PartyName, '' as PartyAdd1,'' as PartyAdd2,'' as PartyAdd3,'' as PartyCity, ''PartyState,''PartyPinCode,"
                Strsql = Strsql & " '' as PartyCountry, 'N' as Authorised,'' as AuthBy, null as AuthDate,'' as EditedBy,Null as EditedOn,'N' as Cancelled,"
                Strsql = Strsql & " 'ADMIN' as ADDEDBY,GRNdate as AddedOn,"
                Strsql = Strsql & " SuppDCNo, SuppInvNo, SuppRefDate, PressID, Rmks, Total, SubTotal, CGSTPer, CGSTAmt, SGSTPer, SGSTAmt,"
                Strsql = Strsql & " IGSTPer, IGSTAmt, OtherAmt, GrandTotal, DiscPer, DiscAmount"
                Strsql = Strsql & " from sacinstk..TrnHdrGrnWOPO"
                clsSVT.execSQLStatement(Strsql, sTrans)

                Strsql = " insert Into TrndtlGrnWOPO(GRNWoPOId,Slno,ItemId,ItemSlno,Packets,QtyAcc,Rate)"
                Strsql = Strsql & " select (select GRnWoPOID from TrnhdrGRNWoPO where substring(GRNWOPONo,4,4)=d.GRNNO) as GrnWoPOId,Slno,ItemId,"
                Strsql = Strsql & " (row_number() over(partition by d.GRNNo order by d.GRNNo)) as ItemSlNO,"
                Strsql = Strsql & " Packets, QtyAcc, Rate"
                Strsql = Strsql & " from SacInStk..TrnDtlGRnWOPO d"
                clsSVT.execSQLStatement(Strsql, sTrans)

            Case "RECEIPTS/PURCHASE"
                Strsql = "Insert InTo TrnHdrPurchase(PurchaseNo,Prefix,PrintPurchaseNO,PurchaseDate,"
                Strsql = Strsql & " SuppID,PartyName,PartyAdd1,PartyAdd2,PartyAdd3,PartyCity,PartyState,PartyPinCode,"
                Strsql = Strsql & " PartyCountry,Authorised,AuthBy,AuthDate,Editedby,EditedOn,Cancelled,AddedBy,AddedOn,"
                Strsql = Strsql & " Narration,Refno,RefDate,DiscPer,DiscAmt,CGSTPer,CGSTAmt,SGSTPer,IgstPer,IGSTAmt,SUBTOT,TotVal)"

                Strsql = Strsql & " select  substring(Convert(varchar(4),year(PurDate)),3,2) +  right('00000'+Convert(varchar(5),PurNo),5) as PurchaseNO,'' as Prefix, "
                Strsql = Strsql & " substring(Convert(varchar(4),year(PurDate)),3,2) +  right('00000'+Convert(varchar(5),PurNo),5)  as PrinPurchaseNo,PurDate as PurchaseDate,"
                Strsql = Strsql & " SuppID, '' as PartyName, '' as PartyAdd1,'' as PartyAdd2,'' as PartyAdd3,'' as PartyCity, ''PartyState,''PartyPinCode,"
                Strsql = Strsql & " '' as PartyCountry, 'N' as Authorised,'' as AuthBy, null as AuthDate,'' as EditedBy,Null as EditedOn,'N' as Cancelled,"
                Strsql = Strsql & " isnull((select MstUser.UserCode from SacInstk..MstUser where MstUser.UserID=h.UserCode),'') as AddedBy,PurDate as AddedOn,"
                Strsql = Strsql & " Remarks, Refno, RefDate, DiscPer, DiscAmount, CGSTPer, CGSTAmt, SGSTPer, IgstPer, IGSTAmt, SUBTOTal, GrandTotal"
                Strsql = Strsql & " from sacinstk..TrnHdrPurchase h"
                clsSVT.execSQLStatement(Strsql, sTrans)


                Strsql = " insert Into TrnDtlPurchase(PurchaseId,Slno,ItemId,ItemSlno,PurchaseQty,PurchaseRate,PurchaseAmt)"
                Strsql = Strsql & " select (select PurchaseID from TrnHdrPurchase where substring(PurchaseNo,4,4)=h.PurNO) as PurchaseId,Slno,ItemId,"
                Strsql = Strsql & " (row_number() over(partition by h.PurNo order by h.PurNo)) as ItemSlNO,"
                Strsql = Strsql & " PurQty, Rate, Amount"
                Strsql = Strsql & " from SacInStk..TrnDtlPurchase d,Sacinstk..TrnHdrPurchase h"
                Strsql = Strsql & " where(d.purId = h.PurID)"
                clsSVT.execSQLStatement(Strsql, sTrans)

            Case "ITEM COMMENTS ENTRY"
                Strsql = "insert into TrnHdrIC(ICNO, Prefix, PrintICNo, ICdate, Authorised, AuthBy, AuthDate, EditedBy, EditedOn, Cancelled, AddedBy, AddedOn)"
                Strsql = Strsql & " Select left(ICNO,2) + Right('00000'+right(ICNO,4),5) as ICNO,'',ICNO,ICDate,'N','',Null,'ADMIN',getdate(),'N','ADMIN',getdate() from SacInstk..TrnHdrIC"
                clsSVT.execSQLStatement(Strsql, sTrans)

                Strsql = "insert into TrnDtlIC(ICID, Slno, ItemID, ItemSlno, ShortClosed, ShortClosedBy, ShortClosedDT, IComments, TaskCompletedPO, TaskCompletedJWPO)"
                Strsql = Strsql & " Select  (Select ICID from TrnHdrIC i where i.ICNO=left(d.ICNO,2) + Right('00000'+right(d.ICNO,4),5)) as ICID,"
                Strsql = Strsql & " Slno, ItemiD, (row_number() over(partition by d.ICNO order by d.ICNO)) as ItemSlNO,"
                Strsql = Strsql & " 'N',0,Null,IComments,TaskCompletedPO,TaskCompletedJWPO "
                Strsql = Strsql & " From SacInstk..TrnDtlIC d, SacInstk..TrnHdrIC h"
                Strsql = Strsql & " where h.ICNO = d.ICNO"
                clsSVT.execSQLStatement(Strsql, sTrans)
            Case "SALES ORDER"
                Strsql = Strsql & " insert into TrnHdrSO(SONO, Prefix, PrintSONO, SODate, CustID, PartyName, PartyAdd1, PartyAdd2, PartyAdd3, PartyCity,"
                Strsql = Strsql & " PartyState, PartyPinCode, PartyCountry, Authorised, AuthBy, AuthDate, EditedBy, EditedOn, Cancelled, AddedBy, AddedOn,"
                Strsql = Strsql & " RMKS, ORDERREF, ORDERDATE, DISPDUEDT, ORDERSPEC, TOTALAMT, TOTALQTY, TOTALCRTNS, TOTALCBM, PINVID, BRANCHID, OrderType, [Status])"
                Strsql = Strsql & " Select Left(SONO, 2) + right('00000' + right(SONO,4),5) as SONO,'' as Prefix,left(SONO,2) + right('00000' + right(SONO,4),5) as PrintSONO,"
                Strsql = Strsql & " SoDate, CustID,'' as PartyName, '' as PartyAdd1, '' as PartyAdd2, '' as PartyAdd3, '' as PartyCity,'' as PartyState,"
                Strsql = Strsql & " 0 As PartyPinCode,'' as PartyCountry,'N' as Authorised,'' as AuthBy, Null as AuthDate,'' as EditedBy,"
                Strsql = Strsql & " Null as EditedOn,'N' as Cancelled,'ADMIN' as AddedBy,SoDate as AddedOn,"
                Strsql = Strsql & " Narr As Rmks, OrdRefNo As OrderRef, OrdRefDate As OrderDate, DISPDUEDT, ORDERSPEC, 0 As TotalAmt, 0 As TotalQty, 0 As TotalCrtns,"
                Strsql = Strsql & " TotCBM as TotalCBM,"
                Strsql = Strsql & " (Select ProInvID from TrnHdrProInv h where h.ProInvNo = stuff(replace(s.ProfInvNO,'/',''),6,0,'0')) as PInvID,"
                Strsql = Strsql & " BranchId, OrderType, Status"
                Strsql = Strsql & " From SacInstk..TrnHdrSO s"
                clsSVT.execSQLStatement(Strsql, sTrans)

                Strsql = Strsql & " insert into TrnDtlSO(SOID, SlNo, ITEMID, ItemSlNO, ShortClosed, ShortClosedBy, ShortClosedDT, NOOFCRTN, Rate,"
                Strsql = Strsql & " AMT, CBMCTN, TOTCBM, CUMCBM, SOQTY, DOZENNO)"
                Strsql = Strsql & " Select (Select SOID from TrnHdrSO h where h.SONO= left(d.SONO,2) + right('00000' + right(d.SONO,4),5)) as SOID,"
                Strsql = Strsql & " SlNo, ItemID, Slno as ItemSlNO,"
                Strsql = Strsql & " 'N' as ShortClosed, 0 as ShortClosedBy,null as ShortClosedDt,NOOFCTNs as NOOFCRTN,Rate,Rate*SOQty as Amt,"
                Strsql = Strsql & " CBMCTN, TOTLCBM As TotCBM, CUMCBM, SOQty, 0 as DozenNo"
                Strsql = Strsql & " From SacInstk..TrnDtlSO d"
                clsSVT.execSQLStatement(Strsql, sTrans)

            Case "EXPORT INVOICE STANDARD"
                Strsql = "insert into TrnHdrEXPINVSTD(EXPINVSTDNO, Prefix, PrintEXPINVSTDNO, EXPINVSTDDate, CustID, PartyName, PartyAdd1, PartyAdd2,"
                Strsql = Strsql & " PartyAdd3, PartyCity, PartyState, PartyPinCode, PartyCountry, Authorised, AuthBy, AuthDate, EditedBy, EditedOn, Cancelled, AddedBy,"
                Strsql = Strsql & " AddedOn, RMKS, MARKS, DUEDAYS, AMTRCVD, NOTIFYNAME, NFAdd1, NFAdd2, NFAdd3, NFAdd4, BRANCHID, CURID, PAYMENTTERMS, PRICETERMS,"
                Strsql = Strsql & " EXPORTERID, OTHREF, COUNTRYOFORIGIN, COUNTRYOFFINALDEST, PRECARRIAGEBY, PLACEOFRECEIPT, VESSELFLIGHTNO, PORTOFLOADING, PORTOFDISCHARGE,"
                Strsql = Strsql & " FINALDESTINATION, TERMSOFDELIVERY, BUYERORDERNO, BUYERORDERDATE, ORDERSPEC, TOTVALUE, EXCHRATE, VALININR, OTHAMTTYPE, OTHAMT, SUBTOTAMT,"
                Strsql = Strsql & " TOTCTNS, TOTQTY, TOTCBM, TOTAMTINUSD, CONVFACT, TAXABLEINR, IGSTPER, IGSTAMT, TOTINRVALUE)"
                Strsql = Strsql & " Select Replace(InvNo,'/','') as EXPINVSTDNO,substring(InvNo,4,3) as Prefix,Replace(InvNo,'/','') as PrintEXPINVSTDNO,"
                Strsql = Strsql & " InvDate As EXPINVSTDDate, CustID As CustID,"
                Strsql = Strsql & " '' as PartyName,'' as PartyAdd1,'' as PartyAdd2,'' as PartyAdd3,'' as PartyCity,'' as PartyState,"
                Strsql = Strsql & " 0 as PartyPinCode,'' as PartyCountry,'N' as Authorised,'' as AuthBy, Null as AuthDate,'' as EditedBy,"
                Strsql = Strsql & " Null as EditedOn,'N' as Cancelled,'ADMIN' as AddedBy,InvDate as AddedOn,"
                Strsql = Strsql & " Remarks As Rmks, Marks, DueDays, AMTRCVD, NOTIFYNAME, NFAdd1, NFAdd2, NFAdd3, NFAdd4, BranchID, CurrID As CURID, PAYMENTTERMS, PRICETERMS, ExpID As ExporterID,"
                Strsql = Strsql & " OtherReference as othRef, COUNTRYOFORIGIN, COUNTRYOFFINALDEST, PRECARRIAGEBY, PLACEOFRECEIPT, VESSELFLIGHTNO, PORTOFLOADING,"
                Strsql = Strsql & " PORTOFDISCHARGE, FINALDESTINATION, TERMSOFDELIVERY, BUYERORDERNO, BUYERORDERDATE, ORDERSPEC As ORDERSPEC, TOTVALUE, EXCHRATE, VALININR,"
                Strsql = Strsql & " OtherAmtType As OTHAMTTYPE, OtherAmt As OthAmt, TotAmt As SUBTOTAMT, TotCartons As TOTCTNS, TOTQTY, TOTCBM, TotAmt As TOTAMTINUSD,"
                Strsql = Strsql & " CONVFACT, TAXABLEINR, IGSTPER, IGSTAMT, TOTINRVALUE"
                Strsql = Strsql & " From SacInstk..TrnHdrEXPINV_STD "
                clsSVT.execSQLStatement(Strsql, sTrans)

                Strsql = "insert into TrnDtlEXPINVSTD(EXPINVSTDID, SlNo, ITEMID, ItemSlNO, ShortClosed, ShortClosedBy, ShortClosedDT,"
                Strsql = Strsql & " SOID, SOItemSlno, StkEXPINVSTDQty, NOOFCTNS, QTYPERCTN, Rate, AMT, MARKSNOS, NOANDKINDOFPKG, BOXSLNO, NETWEIGHT, GROSSWEIGHT,"
                Strsql = Strsql & " HSNCODE, CBMCTN, CUMULATIVECBM, TOTALCBM)"
                Strsql = Strsql & " Select (Select EXPINVSTDID from TrnHdrEXPINVSTD h where h.EXPINVSTDNO= Replace(d.InvNo,'/','')) as EXPINVSTDID,"
                Strsql = Strsql & " SlNo, ItemID, Slno As itemSlno,'N' as Shortclosed,0 as ShortClosedBy,null as ShortclosedDt,"
                Strsql = Strsql & " isnull((Select SOID from TrnHdrSO s where s.SONO = left(d.SONO, 2) + right('00000' + right(d.SONO,4),5)),0) as SOID,"
                Strsql = Strsql & " 0 as SOItemSlno,InvQty as StkEXPINVSTDQty,NoofCtns as NOOFCTNS,QtyPerCtn as QTYPERCTN,Rate, InvQty*Rate as Amt,"
                Strsql = Strsql & " MarksNos, NOANDKINDOFPKG, BoxSlno, NetWT As NETWEIGHT, GRSWT As GROSSWEIGHT, HSNCode, CBMCtn, CumCBM As CUMULATIVECBM,"
                Strsql = Strsql & " TotlCBM as TotalCBM from SacInstk..TrnDtlEXPINV_STD d"
                clsSVT.execSQLStatement(Strsql, sTrans)
            Case Else
                ImportHdrDtlData = False
                Exit Function
        End Select
        ImportHdrDtlData = True
    End Function
    Private Sub ImportData(argMaster As String, argTblname As String, ByVal sTrans As OleDbTransaction)
        Dim Strsql As String

        blnImpTbl = False
        Strsql = "Truncate Table " & argTblname

        clsSVT.execSQLStatement(Strsql, sTrans)

        Select Case UCase(argMaster)
            Case "BRANCH MASTER"

                Strsql = "Insert into MstBRANCH"
                Strsql = Strsql & " (BRANCHID,BRANCHSeries,BRANCHNO,BRANCHCode,BRANCHName,Inactive,InactiveRmks,"
                Strsql = Strsql & " AddedBy,AddedOn,Add1,Add2,Add3,Add4,Add5,ITNo,IECode,DSGNAME,GSTNO,ITAXNO,TINNO)"
                Strsql = Strsql & " Values(0,'',0,'-','-','N','-','ADMIN',Getdate(),'-','-','-','-','-','-','-','-','-','-','-')"
               
                clsSVT.execSQLStatement(Strsql, sTrans)

                Strsql = "Insert into MstBRANCH"
                Strsql = Strsql & " (BRANCHID,BRANCHSeries,BRANCHNO,BRANCHCode,BRANCHName,Inactive,InactiveRmks,"
                Strsql = Strsql & " AddedBy,AddedOn,Add1,Add2,Add3,Add4,Add5,ITNo,IECode,DSGNAME,GSTNO,ITAXNO,TINNO)"

                Strsql = Strsql & " Select row_number() over(order by BranchName) Slno,'',0,BranchName,BranchName,'N' as inactive,'-' as InactiveRmks,"
                Strsql = Strsql & "'ADMIN' as AddedBy,getdate() as AddedOn,"
                Strsql = Strsql & " Add1, Add2, Add3, Add4, Add5, ITNo, IECode, DSGNAME,GSTNO,CSTNO,LSTNO"
                Strsql = Strsql & " from  Sacinstk..MstBRANCH"
                clsSVT.execSQLStatement(Strsql, sTrans)

            Case "CURRENCY MASTER"
                Strsql = "Insert into MstCur"
                Strsql = Strsql & " (CurID,CurCode,CurName,Inactive,InactiveRmks,"
                Strsql = Strsql & " AddedBy,AddedOn,"
                Strsql = Strsql & " CURDECIMAL,EXGRATE )"
                Strsql = Strsql & "  Values(0,'-','-','N','-',"
                Strsql = Strsql & " 'ADMIN',Getdate(),"
                Strsql = Strsql & " 0,0)"
                clsSVT.execSQLStatement(Strsql, sTrans)

                Strsql = "Insert into MstCur"
                Strsql = Strsql & " (CurID,CurCode,CurName,Inactive,InactiveRmks,"
                Strsql = Strsql & " AddedBy,AddedOn,"
                Strsql = Strsql & " CURDECIMAL,EXGRATE )"

                Strsql = Strsql & " Select row_number() over(order by CurName) Slno,CurName as CurCode,CurName,'N' as inactive,'-' as InactiveRmks,"
                Strsql = Strsql & " 'ADMIN' as AddedBy,getdate() as AddedOn,"
                Strsql = Strsql & " CURDECIMAL, EXGRATE"
                Strsql = Strsql & " from  Sacinstk..MStcurrency"
                clsSVT.execSQLStatement(Strsql, sTrans)

            Case "BANK MASTER"
                Strsql = " Insert into MstBankDtl (BankdtlId,BankDtlCode, BankDtlName,"
                Strsql = Strsql & " inactive,inactiveRmks,AddedBy, AddedOn,"
                Strsql = Strsql & " BranchId,CurId,REcvAdd1,RecvAdd2,RecvAdd3,RecSwiftNo,"
                Strsql = Strsql & " RecvAccNo,IntBnkAdd1,IntBnkAdd2,RecSwiftNo2,instAcctAddr1,instAcctAddr2,BenAcctNo)"

                Strsql = Strsql & " values(0,'-','-','N','-','-','',"
                Strsql = Strsql & " 0,0,'-','-','-','-',"
                Strsql = Strsql & " '-','-','-','-','-','-','-')"
                clsSVT.execSQLStatement(Strsql, sTrans)

                Strsql = " Insert into MstBankDtl (BankdtlId,BankDtlCode, BankDtlName,"
                Strsql = Strsql & " inactive,inactiveRmks,AddedBy, AddedOn,"
                Strsql = Strsql & " BranchId,CurId,REcvAdd1,RecvAdd2,RecvAdd3,RecSwiftNo,"
                Strsql = Strsql & " RecvAccNo,IntBnkAdd1,IntBnkAdd2,RecSwiftNo2,instAcctAddr1,instAcctAddr2,BenAcctNo)"

                Strsql = Strsql & " select BankID,'-' as BankDtlCode,'-' as BankDtlName,"
                Strsql = Strsql & " 'N' as inactive,'' as inactiveRmks,'ADMIN' as AddedBy,getdate() as AddedOn,"
                Strsql = Strsql & " BranchId, CurId, REcvAdd1, RecvAdd2, RecvAdd3, RecSwiftNo1,"
                Strsql = Strsql & " RecAcctNo, IntBnkAdd1, IntBnkAdd2, RecSwiftNo2, instAcctAddr1, instAcctAddr2, BenAcctNo"
                Strsql = Strsql & " from sacinstk..MstBankDet"
                clsSVT.execSQLStatement(Strsql, sTrans)


            Case "DEVICE MASTER"
                Strsql = " Insert into MstDEVICE(DeviceID,DeviceSeries,DeviceNO,DeviceCode,DeviceName,Inactive,InactiveRmks,"
                Strsql = Strsql & "  AddedBy,AddedOn,Prefix,Owner)"
                Strsql = Strsql & " Values(0,'',0,'-','-','N','-','ADMIN',Getdate(),'-','-')"

                Strsql = Strsql & " Insert into MstDEVICE(DeviceID,DeviceSeries,DeviceNO,DeviceCode,DeviceName,Inactive,InactiveRmks,"
                Strsql = Strsql & " AddedBy,AddedOn,Prefix,Owner)"
                Strsql = Strsql & " Select DeviceId,'',0,DeviceName,DeviceName,'N' as inactive,'-' as InactiveRmks,"
                Strsql = Strsql & " 'ADMIN' as AddedBy,getdate() as AddedOn,DevPrf,OwnerName"
                Strsql = Strsql & " from  Sacinstk..MstDevice"

            Case "LEDGER GROUP MASTER"
                Strsql = " insert into MstLedgerGroup(LEDGERGROUPID,LEDGERGROUPCODE,LEDGERGROUPNAME,LEDGERGROUPSeries,LEDGERGROUPNO,InActiveRmks,"
                Strsql = Strsql & " InActive,AddedBy,AddedOn,ChangedBy,ChangedOn)"
                Strsql = Strsql & " values(0,'-','-','',0,'','N','ADMIN',getdate(),'',NUll)"

                Strsql = Strsql & " insert into MstLedgerGroup(LEDGERGROUPID,LEDGERGROUPCODE,LEDGERGROUPNAME,LEDGERGROUPSeries,LEDGERGROUPNO,InActiveRmks,"
                Strsql = Strsql & " InActive,AddedBy,AddedOn,ChangedBy,ChangedOn)"
                Strsql = Strsql & " Select row_Number() over(order by LedGroupName) Slno,LedGroupName,LedGroupName,'',0,'','N','ADMIN',getdate(),'',Null"
                Strsql = Strsql & " from SacInstk..MstLedgerGroup"

            Case "LEDGER MASTER"
                Strsql = " Insert into MstLedger(LEDGERID,LEDGERCODE,LEDGERName,LEDGERSeries,LEDGERNo,InActive,AddedBy,AddedOn,ChangedBy,ChangedOn,"
                Strsql = Strsql & " TAXPER,OPBAL,InActiveRmks,ALIASNAME,CRORDR,BILLWISETRACKREQ,LEDGERGROUPID)"
                Strsql = Strsql & " Values(0,'-','-','',0,'N','ADMIN',getdate(),'',Null,0,0,'','','','N',0)"

                Strsql = Strsql & " Insert into MstLedger(LEDGERID,LEDGERCODE,LEDGERName,LEDGERSeries,LEDGERNo,InActive,AddedBy,AddedOn,ChangedBy,ChangedOn,"
                Strsql = Strsql & " TAXPER,OPBAL,InActiveRmks,ALIASNAME,CRORDR,BILLWISETRACKREQ,LEDGERGROUPID)"
                Strsql = Strsql & " Select row_number() over(order by LedName) slno, LedName,LedName,'',0,'N','ADMIN',getdate(),'',NUll,Percentage,OpBalance,"
                Strsql = Strsql & " '',LedAliasName,CRorDR,isnull(BillWiseTrack,'N'),"
                Strsql = Strsql & " (select LedgerGroupID from MstLedgerGroup g where g.LedgerGroupCode=lg.LedGroupname) as LedgerGroupID"
                Strsql = Strsql & " from SacInstk..MstLedger l left outer Join SacInstk..MstLedgerGroup lg"
                Strsql = Strsql & " on l.LedGroupID=lg.LedGroupID"

            Case "JW CATEGORY MASTER"
                Strsql = "INSERT INTO MstJwcategory(JwcategoryID,JwcategoryCode,JwcategoryName, Inactive,AddedBy,AddedOn)"
                Strsql = Strsql & " values(0,'-','-','N', 'ADMIN',Getdate())"
                clsSVT.execSQLStatement(Strsql, sTrans)

                Strsql = "INSERT INTO MstJwcategory(JwcategoryID,JwcategoryCode,JwcategoryName, Inactive,AddedBy,AddedOn)"
                Strsql = Strsql & "SELECT row_number() over(order by JwcategoryName) slno,JwcategoryName as JwcategoryCode,JwcategoryName, "
                Strsql = Strsql & "'N'  as inactive,'ADMIN' as AddedBy,"
                Strsql = Strsql & " getdate() as AddedON "
                Strsql = Strsql & " FROM sacinstk..MstJwcategory"
                clsSVT.execSQLStatement(Strsql, sTrans)

            Case "JOBWORKER MASTER"
                Strsql = "INSERT INTO MstJw(JwID,JwCode,JwName, Inactive,AddedBy,AddedOn)"
                Strsql = Strsql & " values(0,'-','-','N', 'ADMIN',Getdate())"
                clsSVT.execSQLStatement(Strsql, sTrans)

                Strsql = " INSERT INTO MstJW(JWID, JWCode, JWName, Inactive, AddedBy, AddedOn, JWCategoryId, JWBarcode, DEfChkJWRPT)"
                Strsql = Strsql & " Select row_Number() over (order by JWName) SLNo,"
                Strsql = Strsql & " JWName as JWCode, JWName, case when active='Y' then 'N' when active ='N' then 'Y' end  as inactive,"
                Strsql = Strsql & " 'ADMIN' as AddedBy,getdate() as AddedON ,"
                Strsql = Strsql & " (Select JWCategoryID from mstJWCAtegory c where c.JWCategoryCode=jc.JWCategoryname) as JWCategoryID,"
                Strsql = Strsql & " JWBarcode, DEfChkJWRPT FROM sacinstk..MstJobWorker j left outer join SacInstk..MstJWCategory jc"
                Strsql = Strsql & " On j.JWCategoryID=jc.JwcategoryId"
                clsSVT.execSQLStatement(Strsql, sTrans)

            Case "ITEM GROUP MASTER"

                Strsql = "INSERT INTO MstItemGroup(ITEMGROUPID,ITEMGROUPSeries,ITEMGROUPNO,ITEMGROUPCODE,ITEMGROUPNAME,InActive,InActiveRmks,"
                Strsql = Strsql & " AddedBy,AddedOn,NOSPERCARTON,KGSPERLAKH,DOZPERCARTON,CBMSPERCARTON,ITEMTYPEID)"
               
                Strsql = Strsql & " Select row_number() over(order by ItemGrpname) Slno,'',0,ItemGrpName,ItemGrpName,'N','','ADMIN',cast(getdate() as date),"
                Strsql = Strsql & " NOSPERCARTON,KGSPERLAKH,DOZPERCARTON,CBMSPERCARTON,ITEMTYPEID "
                Strsql = Strsql & " from SacInStk..MstGrpItem ig, MstItemType t"
                Strsql = Strsql & " where ig.itemType=t.type and itemgrpname not in ('','-')"
                clsSVT.execSQLStatement(Strsql, sTrans)

            Case "BOARD GROUP"
                Strsql = "Insert Into MstItemBoardGroup "
                Strsql = Strsql & " (ItemBOARDGROUPID,ItemBOARDGROUPCODE,ItemBOARDGROUPNAME,ItemBOARDGROUPSeries,ItemBOARDGROUPNO,"
                Strsql = Strsql & " InActiveRmks,InActive,AddedBy,AddedOn)"
                Strsql = Strsql & " values (0,'-','-','-',0,'-','N','-','')"
                clsSVT.execSQLStatement(Strsql, sTrans)

                Strsql = "Insert Into MstBoardGroup "
                Strsql = Strsql & " (ItemBOARDGROUPID,ItemBOARDGROUPCODE,ItemBOARDGROUPNAME,ItemBOARDGROUPSeries,ItemBOARDGROUPNO,"
                Strsql = Strsql & " InActiveRmks,InActive,AddedBy,AddedOn)"

                Strsql = Strsql & " Select row_number() over(order by BoardGrpName) Slno,BoardGrpName,BoardGrpName,'',0,'','N','ADMIN',cast(getdate() as date)"
                Strsql = Strsql & " from SacInStk..mstGrpBoardName "
                Strsql = Strsql & " where BoardGrpName not in ('','-')"

                clsSVT.execSQLStatement(Strsql, sTrans)

            Case "BOARD SIZE MASTER"
                Strsql = " Insert into MStBoardSize"
                Strsql = Strsql & " (BOARDSIZEID,BOARDSIZESeries,BOARDSIZENO,BOARDSIZECODE,BOARDSIZENAME,"
                Strsql = Strsql & " InActive,InActiveRmks,AddedBy,AddedOn)"

                Strsql = Strsql & " Select row_number() over(order by BoardSizeName) Slno,'',0,BoardSizeName,BoardSizeName,'N','','ADMIN',cast(getdate() as date)"
                Strsql = Strsql & " from SacInStk..mstBoardSize "
                Strsql = Strsql & " where BoardSizeName not in ('','-','--')"
                clsSVT.execSQLStatement(Strsql, sTrans)

                'Strsql = " Insert into MStBoardSize"
                'Strsql = Strsql & " (BoardSizeID,BoardSizeCode,BoardSizeName,Inactive,InactiveRmks,"
                'Strsql = Strsql & " AddedBy,AddedOn)"
                'Strsql = Strsql & " Values(0,'-','-','N','-',"
                'Strsql = Strsql & " 'ADMIN',Getdate())"
                'clsSVT.execSQLStatement(Strsql, sTrans)
            Case "UOM MASTER"

                Strsql = "INSERT INTO MstUOM(UOMID,UOMCode,UOMName,"
                Strsql = Strsql & " Inactive,AddedBy,AddedOn,PerUnit)"
                Strsql = Strsql & " Select row_number() over(order by UOM) Slno,UOM as UOMCode,UOM as UOMName,"
                Strsql = Strsql & " 'N' as inactive,'ADMIN' as AddedBy,getdate() as AddedON ,UOMqty as PerUnit"
                Strsql = Strsql & " FROM sacinstk..MstUOM"
                clsSVT.execSQLStatement(Strsql, sTrans)

            Case "ITEM MASTER"
                ' Import Item Master

                Strsql = "Insert Into MstItem "
                Strsql = Strsql & " (ItemID,ItemSeries,ItemNo,ItemCode,ItemName,ItemAliasName,ItemGroupID,UOMID,"
                Strsql = Strsql & " InActive,InActiveRmks,AddedBy,AddedOn,ITEMSRATE,SPEC,BOMItem,NonStkable,HSNCODE,HSNDESC,ITEMBOARDID,"
                Strsql = Strsql & " OTRATE,DOZENNO,PRINTNAME,NOOFUPS,REGULARRATE,FREEITEM,FILLINSO,BARCODE,MinLevel,MaxLevel,CriticalLevel,ROL,ReOrderLevel,"
                Strsql = Strsql & " ReOrderQty,MOQ,ITEMTYPEID)"

                Strsql = Strsql & " values (0,'-',0,'-','-','-','-',0,0,'N','-','ADMIN','',0,0,'N','N',"
                Strsql = Strsql & " '-','-',0,0,0,'-',0,0,'N','N','-',0,0,0,0,0,"
                Strsql = Strsql & " 0,0,0)"
                clsSVT.execSQLStatement(Strsql, sTrans)

                Strsql = " Insert into MstItem (ItemID,ItemSeries,ItemNo,ItemCode,ItemName,ItemAliasName,ItemGroupID,UOMID,"
                Strsql = Strsql & " InActive,InActiveRmks,AddedBy,AddedOn,ITEMSRATE,SPEC,BOMItem,NonStkable,HSNCODE,HSNDESC,ITEMBOARDID,"
                Strsql = Strsql & " OTRATE,DOZENNO,PRINTNAME,NOOFUPS,REGULARRATE,FREEITEM,FILLINSO,BARCODE,MinLevel,MaxLevel,CriticalLevel,ROL,ReOrderLevel,"
                Strsql = Strsql & " ReOrderQty,MOQ,ITEMTYPEID)"
                Strsql = Strsql & " Select row_number() over(order by i.ItemName) Slno,'',0,i.ItemName,ItemName,ItemAliasName,Isnull(g.ItemGroupID,0),"
                Strsql = Strsql & " Isnull(u.UOMID,0),case i.Active when 'Y' then 'N' else 'Y' end,'','ADMIN',cast(getdate() as date),cast(itemRate as numeric(19,2)),'',"
                Strsql = Strsql & " case isnull(bo.bomitemid,0) when 0 then 'N' else 'Y' end,'N',HSNCode,HSNDesc,isnull(bg.BoardGroupID,0),"
                Strsql = Strsql & " cast(otrate as numeric(19,2)),i.dozpercarton,LocInvName,NoOfUPS,cast(RegularRate as numeric(19,2)),FreeItem,FillInSO,ItemCode,MInLevel,MaxLevel,0,"
                Strsql = Strsql & " case when reorderlevel >0 then'Y' else 'N' end,reorderlevel,0,0,isnull(t.ITEMTYPEID,0)  "
                Strsql = Strsql & " from SacInStk..MstItem i"
                Strsql = Strsql & " Left Join SacInStk..mstGrpBoardName b on i.BoardGrpID=b.BoardGrpID"
                Strsql = Strsql & " Left Join MstBoardGroup bg on b.BoardGrpName=bg.BoardGroupCode"
                Strsql = Strsql & " Left Join SacInStk..MstGrpItem gi on i.ItemGrpID=gi.ItemGrpID"
                Strsql = Strsql & " Left Join MstITEMGROUP g on gi.ItemGrpName=g.ITEMGROUPCode"
                Strsql = Strsql & " Left Join MstUOM u on i.UOMID=u.UOMID"
                Strsql = Strsql & " Left Join SacInStk..UtlHdrBOM bo on i.ItemID=bo.BomItemID and effdateto >= getdate()"
                Strsql = Strsql & " Left Join MstitemType t on i.ItemType=t.Type"
                Strsql = Strsql & " where ItemName not in ('','-','--')"
                clsSVT.execSQLStatement(Strsql, sTrans)

'                Insert into MstItem (ItemId, ItemCode, ItemName, UOMId, Inactive, InActiveRmks, AddedBy, AddedOn, MOQ, RSP,
'SPQ, LeadTime, Genericpartno, TariffNo, DivItem, ItemGroupId, MakeId)
' Select  row_number() over(order by i.ItemName) Slno,ItemCode,ItemName,isnull(u.Uomid,0),
' Case i.InActive When 'Y' then 'N' else 'Y' end,'',i.AddedBy,i.AddedOn,MOq,RSP,SPQ,leadTime,GenericPartNo,TariffNo,DivItem,
' Isnull(g.ItemGroupID, 0) As ItemGroupId,Isnull(m.MakeID,0) As MakeId 
' From SMIC..MstItem i  Left Join MstITEMGROUP g on i.ItemGroupid=g.ITEMGROUPid
' Left Join MstUOM u on i.UOMID=u.UOMID 
' Left Join MstMake m on i.MakeId=m.MakeID 
' order by i.itemID


            Case "STATE MASTER"
                Strsql = "Insert into MstState"
                Strsql = Strsql & " (STATEID,STATESeries,STATENo,STATECODE,STATENAME,InActive,InActiveRmks,ChangedBy,ChangedOn,AddedBy,AddedOn)"
               
                Strsql = Strsql & "select row_number() over(order by StateName) slno,'',0,stateName,StateName,'N','',null,null,'ADMIN',getdate() "
                Strsql = Strsql & " from  Sacinstk..MstState"
                clsSVT.execSQLStatement(Strsql, sTrans)

                'Strsql = "Insert into MstState"
                'Strsql = Strsql & " (STATEID,STATESeries,STATENo,STATECODE,STATENAME,InActive,InActiveRmks,ChangedBy,ChangedOn,AddedBy,AddedOn)"
                'Strsql = Strsql & "  Values (0,'',0,'-','-','N','',null,null,'ADMIN',getdate())
                'clsSVT.execSQLStatement(Strsql, sTrans)
          
            Case "AGENT MASTER"
                Strsql = " Insert into MstAgent"
                Strsql = Strsql & " (AGENTID,AGENTCODE,AGENTName,AGENTSeries,AGENTNo,InActive,InActiveRmks,ChangedBy,ChangedOn,AddedBy,AddedOn)"
                Strsql = Strsql & " Select row_Number() over (order by AgentName) slno, AgentName,AgentName,'',0,'N','',Null,NUll,'ADMIN',getdate()"
                Strsql = Strsql & " from  Sacinstk..MstAgent"
                clsSVT.execSQLStatement(Strsql, sTrans)

                'Strsql = "Insert into MstAgent"
                'Strsql = Strsql & " (AGENTID,AGENTCODE,AGENTName,AGENTSeries,AGENTNo,InActive,InActiveRmks,ChangedBy,ChangedOn,AddedBy,AddedOn"
                'Strsql = Strsql & "  Values(0,'-','-','',0,'N','',Null,Null,'ADMIN',getdate())"
                'clsSVT.execSQLStatement(Strsql, sTrans)

            Case "SALES TYPE MASTER"
                Strsql = "Insert into MstST"
                Strsql = Strsql & " (STID,STSeries,STNo,STCODE,STNAME,InActive,InActiveRmks,ChangedBy,ChangedOn,AddedBy,AddedOn,CGSTPER,SGSTPER,"
                Strsql = Strsql & " IGSTPER,CSTPER,VATPER)"
                Strsql = Strsql & "  Values(0,'',0,'-','-','N','',Null,Null,'ADMIN',getdate(),0,0,0,0,0)"
                clsSVT.execSQLStatement(Strsql, sTrans)

                Strsql = "Insert into MstST"
                Strsql = Strsql & " (STID,STSeries,STNo,STCODE,STNAME,InActive,InActiveRmks,ChangedBy,ChangedOn,AddedBy,AddedOn,CGSTPER,SGSTPER,"
                Strsql = Strsql & " IGSTPER,CSTPER,VATPER)"
              
                Strsql = Strsql & " Select row_number() over(order by Stname) slno,'',0,STName,STName,'N','',Null,Null,'ADMIN',getdate(),CGSTPer,SGSTPer,IGSTPer,CSTPER,VATPER"
                Strsql = Strsql & " from  Sacinstk..MstSalesType"
                clsSVT.execSQLStatement(Strsql, sTrans)

            Case "TRANSPORTER MASTER"

                'Strsql = "Insert into MStTRANSPORT"
                'Strsql = Strsql & "(TRANSPORTID,TRANSPORTSeries,TRANSPORTNo,TRANSPORTCODE,TRANSPORTNAME,InActive,InActiveRmks,ChangedBy,"
                'Strsql = Strsql & " ChangedOn,AddedBy,AddedOn)"
                'Strsql = Strsql & " values(0,'',0,'-','-','N','',Null,NUll,'ADMIN',getdate())
                'clsSVT.execSQLStatement(Strsql, sTrans)

                Strsql = "Insert into MStTRANSPORT"
                Strsql = Strsql & "(TRANSPORTID,TRANSPORTSeries,TRANSPORTNo,TRANSPORTCODE,TRANSPORTNAME,InActive,InActiveRmks,ChangedBy,"
                Strsql = Strsql & " ChangedOn,AddedBy,AddedOn)"

                Strsql = Strsql & " select Row_number() over(order by TransporterName) slno, '',0,TransporterName, TransporterName,'N','',Null,Null,'ADMIN',getdate()"
                Strsql = Strsql & " from  Sacinstk..MStTRANSPORTER"
                clsSVT.execSQLStatement(Strsql, sTrans)

            Case "CUSTOMER MASTER"
                Strsql = " insert into MstCust(CustID,CustCode,CustName,InActive,InActiveRmks,BAdd1,BAdd2,BAdd3,AddedBy,AddedOn,ChangedBy,ChangedOn,"
                Strsql = Strsql & " CustGrpID,CUSTTYPE,CustAliasName,CUSTOFFPHNO,CUSTOFFFAXNO,CUSTFACTPHNO,EMAILID,ORDERSPEC,STATEID,CRDAYS,STID,AGENTID,PANNO,"
                Strsql = Strsql & " WEB,PHNO,MOBILE,STATION,BOOKOFF,CCMAILID,WEF,TYPE,COUNTRY,CITY,STATE,PINCODE,TrnWarning,Defaultcust,AddrChange,BillWiseTrack,"
                Strsql = Strsql & " CRLmtWrng,CrLimitReq,CrLimitAmt,CustSeries,CustNo,TRANSPORTID,GSTNO,CUSTLSTNO,CSTNO,FAX,CATEGORY,INVTYPE)"
                Strsql = Strsql & " values(0,'-','-','N','','','','','ADMIN',getdate(),Null,NUll,"
                Strsql = Strsql & " 0,'','','','','','','',0,0,0,0,'',"
                Strsql = Strsql & " '','','','','','','','','','','',0,'','','','',"
                Strsql = Strsql & " '','',0,'',0,0,'','','','','','')"
                clsSVT.execSQLStatement(Strsql, sTrans)

                Strsql = "Insert into MstCust"
                Strsql = Strsql & " Select row_Number() over(order by CustName) CustID,CustName as CustCode,CustName as CustName,'N' as InActive,'' as InActiveRemarks,"
                Strsql = Strsql & " '' as BAdd1,'' as Badd2,'' as BAdd3,'ADMIN' as Addedby,getdate() as AddedOn,NUll,NUll,"
                Strsql = Strsql & " 0 as CustGrpID,[Type] as CustType,CustAliasName,CustOffPhNo,CustFaxNo,CustFactPhNo,EmailID,OrderSpec,"
                Strsql = Strsql & " (Select stateID from MstState s where s.StateID=st.StateID) as StateID,"
                Strsql = Strsql & " CRDays,"
                Strsql = Strsql & " (Select STID from MstST a where a.STID=sst.STID) as STID,"
                Strsql = Strsql & " (Select AgentID from MstAgent a where a.AgentID=at.AgentID) as AgentID,PANNO,"
                Strsql = Strsql & " WEB,PHNO,MObile,Station,BookOff,CCMailID,WEF,Type,Country,'' as City,'' as State,0 as PinCode,'' as TrnWarning,"
                Strsql = Strsql & " 'N' as defautlCust,'N' as AddrChange,'N' as BillWiseTrack,'' as CrLmtWrng,"
                Strsql = Strsql & " 'N' as CrLimitReq,0 as CrLimitAmt,'' as CustSeries,0 as CustNo,"
                Strsql = Strsql & " (Select TRANSPORTID from Msttransport t where t.TransportID=tt.TransporterID) as TransportID,"
                Strsql = Strsql & " GSTNO, CustLStNO, CustCSTNO, Fax, CustCat, InvType"
                Strsql = Strsql & " from SacInstk..MstCust c left outer join SacInstk..MstState st on c.StateID=st.StateID"
                Strsql = Strsql & " left outer join SacInstk..MstSalesType sst on c.STID=sst.STID"
                Strsql = Strsql & " left outer join SacInstk..MstAgent at on c.AgentID=at.AgentID"
                Strsql = Strsql & " left outer join SacInstk..MstTransporter tt on c.TransporterID=tt.TransporterID"
                clsSVT.execSQLStatement(Strsql, sTrans)

                Strsql = "delete from MstPartyAddress where MstDocId=7"
                clsSVT.execSQLStatement(Strsql, sTrans)

                Strsql = " Insert Into MstPartyAddress(MstDocId,Partyid,AddCat ,Add1,Add2,Add3)"
                Strsql = Strsql & " select 7 as MstDocId,custid as PartyID,CustFactAdd1 as AddCat,CustFactAdd2 as add1,"
                Strsql = Strsql & " CustFactAdd3 as add2,CustFactAdd4 as add3 from sacinstk..Mstcust"
                Strsql = Strsql & " where CustFactAdd1 <>''"
                Strsql = Strsql & " or CustFactAdd2 <>''"
                Strsql = Strsql & " or CustFactAdd3 <>''"
                Strsql = Strsql & " or CustFactAdd4 <>''"
                clsSVT.execSQLStatement(Strsql, sTrans)

            Case "SUPPLIER MASTER"
                'Strsql = " insert into Mstsupp (SuppID,SuppCode,SuppName,InActive,InActiveRmks,BAdd1,BAdd2,BAdd3,AddedBy,AddedOn,ChangedBy,ChangedOn,SUPPGRPID,"
                'Strsql = Strsql & " SuppAliasName,SUPPOFFPHNO,SUPPOFFFAXNO,SUPPFACTPHNO,EMAILID,SUPPMOBNO,GSTNO,SGSTPER,CGSTPER,IGSTPER,BADDEBTORS,"
                'Strsql = Strsql & " Country,City,State,PinCode,TrnWarning,Defaultcust,AddrChange,BillWiseTrack,CRLmtWrng,CrLimitReq,CrLimitAmt,SuppSeries,SuppNo,DISCPER,"
                'Strsql = Strsql & " TINNO,SUPPCSTNO,SUPPEDPER,SPPSTPER,SUPPSTPER,SUPPEDCESS,ORDERRAISEDIN)"
                'Strsql = Strsql & " values(0,'-','-','N','','','','','ADMIN',getdate(),null,null,0,"
                'Strsql = Strsql & " '','','','','','','',0,0,0,'',"
                'Strsql = Strsql & " '','','',0,'','N','N','N','','N',0,'',0,0,"
                'Strsql = Strsql & " '','',0,0,0,0,'')"
                'clsSVT.execSQLStatement(Strsql, sTrans)

                Strsql = " insert into Mstsupp (SuppID,SuppCode,SuppName,InActive,InActiveRmks,BAdd1,BAdd2,BAdd3,AddedBy,AddedOn,ChangedBy,ChangedOn,SUPPGRPID,"
                Strsql = Strsql & " SuppAliasName,SUPPOFFPHNO,SUPPOFFFAXNO,SUPPFACTPHNO,EMAILID,SUPPMOBNO,GSTNO,SGSTPER,CGSTPER,IGSTPER,BADDEBTORS,"
                Strsql = Strsql & " Country,City,State,PinCode,TrnWarning,Defaultcust,AddrChange,BillWiseTrack,CRLmtWrng,CrLimitReq,CrLimitAmt,SuppSeries,SuppNo,DISCPER,"
                Strsql = Strsql & " TINNO,SUPPCSTNO,SUPPEDPER,SPPSTPER,SUPPSTPER,SUPPEDCESS,ORDERRAISEDIN)"

                Strsql = Strsql & " Select row_Number() over (order by SuppName) slno, SuppName, SuppName,'N','','','','','ADMIN',getdate(),Null,NUll,0,"
                Strsql = Strsql & " SuppAliasName,SuppOffPhNo,SuppFaxNo,SuppFactPhNo,Email,SuppMobNo,GSTNo,SGSTPer,CGSTPer,IGSTPer,BadDebtor,"
                Strsql = Strsql & " '','','',0,'','N','','','','N',0,'',0,DiscPer,"
                Strsql = Strsql & " SUPPLSTNO,SUPPCSTNO,SUPPEDPER,SPPSTPER,SUPPSTPER,SUPPEDCESS,"
                Strsql = Strsql & " Case when RaisePO='Y' then 'PO' "
                Strsql = Strsql & " when RaisePO='N' then 'JWPO' end as ORDERRAISEDIN froM sAcInstk..MstSupp"
                clsSVT.execSQLStatement(Strsql, sTrans)

                '  Strsql = " select * from INformation_schema.columns where Column_Name ='HSNCode'"
                'Strsql = " Select d.* from SacInstk..UtlAddnlDocFlds d, SacInstk..UtlAddnlInfoDocTbls t"
                'Strsql = Strsql & " where d.DocId = t.DocID and t.TableName = 'MstCust'"
                'Strsql = Strsql & " order by FldOrderBy"
                'clsSVT.execSQLStatement(Strsql, sTrans)

                Strsql = "delete from MstPartyAddress where MstDocId=10"
                clsSVT.execSQLStatement(Strsql, sTrans)

                Strsql = " Insert Into MstPartyAddress(MstDocId,Partyid,AddCat ,Add1,Add2,Add3)"
                Strsql = Strsql & " select 10 as MstDocId,Suppid as PartyID,SuppFactAdd1 as AddCat,SuppFactAdd2 as add1,SuppFactAdd3 as add2,SuppFactAdd4 as add3 "
                Strsql = Strsql & " from sacinstk..MstSupp"
                Strsql = Strsql & " where SuppFactAdd2 <>''"
                Strsql = Strsql & "or SuppFactAdd1 <>''"
                Strsql = Strsql & " or SuppFactAdd3 <>''"
                Strsql = Strsql & " or SuppFactAdd4 <>''"
                clsSVT.execSQLStatement(Strsql, sTrans)

            Case "EXPORTER DETAIL MASTER"

                'Strsql = "Insert into MstExporter"
                'Strsql = Strsql & " (ExporterID,ExporterCode,ExporterName,Inactive,"
                'Strsql = Strsql & " AddedBy,AddedOn,"
                'Strsql = Strsql & " LSTNO,CSTNO,CONTACTINFO,MISC1,MISC2,MISC3,MISC4,"
                'Strsql = Strsql & " COUNTRY,CustOffAdd1,CustOffAdd2,CustOffAdd3,CustOffAdd4,custoffphno,"
                'Strsql = Strsql & " custOffFaxNo,custFActAdd1,CustFActAdd2,CustFactAdd3,CustFactAdd4,CustFactPHNo )"
                'Strsql = Strsql & "  Values(0,'-','-','N',"
                'Strsql = Strsql & " 'ADMIN',Getdate(),"
                'Strsql = Strsql & " '-','-','-','-','-','-','-',"
                'Strsql = Strsql & " '-','-','-','-','-','-',"
                'Strsql = Strsql & " '-','-','-','-','-','-')"
                'clsSVT.execSQLStatement(Strsql, sTrans)

                Strsql = " Insert into MstExporter(ExporterID,ExporterSeries,ExporterNO,ExporterCODE,ExporterNAME,InActive,InActiveRmks,"
                Strsql = Strsql & " AddedBy,AddedOn,LSTNO,CSTNO,ContactInfo,Misc1,Misc2,Misc3,Misc4,country,"
                Strsql = Strsql & " BAdd1,BAdd2,Badd3,custOffPHNO,CustOffFAxNo,CustFactPHNo)"

                Strsql = Strsql & " Select row_number() over(order by CustName) Slno,'',0,CustName,custName,'N','','ADMIN',cast(getdate() as date),"
                Strsql = Strsql & " CustLSTNo,CustCSTNo,CustContact,CustMisc1,CustMisc2,CustMisc3,CustMisc4,CustCountry,"
                Strsql = Strsql & " CustOffAdd1,CustOffAdd2,custoffadd3+' '+  custoffadd4 as custoffAdd3,CustoffPhno,custfaxno,custFactPHNO"
                Strsql = Strsql & " from SacInStk..MstExporterdet"
                Strsql = Strsql & " where CustName not in ('','-')"
                clsSVT.execSQLStatement(Strsql, sTrans)

                Strsql = "delete from MstPartyAddress where MstDocId=1004"
                clsSVT.execSQLStatement(Strsql, sTrans)

                Strsql = " Insert Into MstPartyAddress(MstDocId,Partyid,AddCat ,Add1,Add2,Add3)"
                Strsql = Strsql & " select 1004 as MstDocId, row_number() over(order by CustName)  PartyID,CustFactAdd1 as AddCat,CustFactAdd2 as add1,"
                Strsql = Strsql & " CustFactAdd3 as add2,CustFactAdd1 as add4"
                Strsql = Strsql & " from sacinstk..MstExporterdet"
                clsSVT.execSQLStatement(Strsql, sTrans)

            Case "BOM MASTER"
                Strsql = " Insert into MstHdrBOM (BOMID,BOMItemID,EffFrom,EffTo,BomDate,BOMCreatedBy,EditedBy,BOMEditDAte)"
                Strsql = Strsql & " select BOMID,BOMITEmId,EffDAteFrom,getdate() as EffDAteTo,getdate() as BOMDate,'ADMIN' as BOMCreatedBy,"
                Strsql = Strsql & "  'ADMIN' as BOMEditedBy,Getdate() as BOMEditDate"
                Strsql = Strsql & "  from sacinstk..UtlHdrBOM"
                clsSVT.execSQLStatement(Strsql, sTrans)

                Strsql = "Truncate Table MstDtlBOM"
                clsSVT.execSQLStatement(Strsql, sTrans)

                Strsql = " Insert into MstDtlBOM (BOMID,itemID,slno,Qty,Remarks)"
                Strsql = Strsql & " select BOMID,ITemId,MakeQty,"
                Strsql = Strsql & " (row_number() over(partition by BOMID order by BOMID)) as Slno,"
                Strsql = Strsql & " '' as Remarks"
                Strsql = Strsql & " from sacinstk..UtlDtlBOM"
                clsSVT.execSQLStatement(Strsql, sTrans)

            Case "AGENT VS ITEM PRICE MASTER"
                Strsql = " Insert into MstAgentItemPrice(ITemId,AgentId,Price) "
                Strsql = Strsql & " select (select i.ItemId  from MstItem i where i.itemName=si.ItemName ) as ItemId, "
                Strsql = Strsql & " (select a.agentId from MstAgent a where a.agentname=au.agentName) as AgentID ,u.Price"
                Strsql = Strsql & " from sacinstk..MstAgentItemPrice u  "
                Strsql = Strsql & " left join sacinstk..Mstitem si on u.itemId=si.itemid  "
                Strsql = Strsql & " left join sacinstk..Mstagent au  on u.agentid=au.agentid "
                Strsql = Strsql & " where(si.itemid Is Not Null)"
                Strsql = Strsql & " and au.agentid is  Not Null"

                clsSVT.execSQLStatement(Strsql, sTrans)

            Case "ITEM VS BOARDSIZE MASTER"
                Strsql = " select (select i.ItemId from MstITem i where i.itemName=si.itemName) as itemId,"
                Strsql = Strsql & " (select b.boardsizeId  from MstBoardSize b where b.boardsizeNAme=sb.boardsizeNAme) as BoardSizeID,DefSize"
                Strsql = Strsql & " from sacinstk..MstItemvsBoardSize sz"
                Strsql = Strsql & " left join sacinstk..Mstitem si on si.itemId=sz.itemid"
                Strsql = Strsql & " left join sacinstk..MstBoardsize sb on sb.Boardsizeid=sz.boardsizeid"

            Case "SUPPLIER VS ITEMPRIORITY MASTER"
                Strsql = " Insert into MstSuppItem(itemId,ITemGroupId,SuppId,ItemTypeId,effdatefrom,effdateto)"
                Strsql = Strsql & " select (select i.ItemId  from MstItem i where i.itemName=si.ItemName ) as ItemId,"
                Strsql = Strsql & " (select g.ItemGroupId from MstItem g where g.itemName=si.ItemName) as itemGroupId, "
                Strsql = Strsql & " (select s.SuppId from MstSupp s where s.Suppname=su.SuppName) as SuppId,"
                Strsql = Strsql & " (select t.ItemTypeId from MstITemType t  where  t.Type=u.chritemType ) as ItemTypeId,getdate() as EffDateFrom,getdate() as EffDateTo"
                Strsql = Strsql & " from sacinstk..utldtlSuppvsItem u "
                Strsql = Strsql & " left join sacinstk..Mstitem si on u.itemId=si.itemid  "
                Strsql = Strsql & " left join sacinstk..MstSupp su  on u.Suppid=su.Suppid"
                Strsql = Strsql & " where(u.itemid > 0)"
                Strsql = Strsql & " and su.suppid is  Not Null "
                Strsql = Strsql & " order by u.itemid"
                clsSVT.execSQLStatement(Strsql, sTrans)

            Case "CUSTOMER VS ITEM MASTER"
                Strsql = " Insert into MstCustItem(ITemId,CustId,ITemRate) "
                Strsql = Strsql & " select (select i.ItemId  from MstItem i where i.itemName=si.ItemName ) as ItemId, "
                Strsql = Strsql & " (select s.custId from Mstcust s where s.custname=su.custName) as custID ,u.ItemRAte"
                Strsql = Strsql & " from sacinstk..MstCustItem u  "
                Strsql = Strsql & " left join sacinstk..Mstitem si on u.itemId=si.itemid  "
                Strsql = Strsql & " left join sacinstk..Mstcust su  on u.custid=su.custid "
                clsSVT.execSQLStatement(Strsql, sTrans)
            Case "ITEM OPENING BALANCE"

                Strsql = " insert into MstopBalance(ItemID, FinYear, OPqty, RcptRef, UOMID, OPRate)"
                Strsql = Strsql & " Select  (Select i.ItemId  from MstItem i where i.itemName=si.ItemName ) As ItemId,FinYear,OPQty,'-' as RcptRef,"
                Strsql = Strsql & " (select i.UOMID from Mstitem i where i.itemNAme=si.itemName) as UOMID,0 as OPRate"
                Strsql = Strsql & " From sacinstk..MstOPBalance u  "
                Strsql = Strsql & " Left Join sacinstk..Mstitem si on u.itemId=si.itemid "
                Strsql = Strsql & " where si.ItemID Is Not null"
                clsSVT.execSQLStatement(Strsql, sTrans)
            Case Else

                blnImpTbl = False
                Exit Sub
        End Select
        blnImpTbl = True

    End Sub

    Private Sub chkAll_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkAll.CheckedChanged
        With vsfDet
            If .Rows.Count <= 1 Then Exit Sub
            If chkAll.Checked = True Then
                For i As Int32 = 1 To .Rows.Count - 1
                    .SetData(i, 1, True)
                Next
            Else
                For i As Int32 = 1 To .Rows.Count - 1
                    .SetData(i, 1, False)
                Next
            End If
        End With
    End Sub


    Private Sub btnCancel_Click(sender As System.Object, e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub vsfDet_BeforeEdit(sender As Object, e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfDet.BeforeEdit
        With vsfDet
            Select Case .Col
                Case vdetMasters, vdetTblName
                    e.Cancel = True

            End Select
        End With

    End Sub

    Private Sub vsfDet_SetupEditor(sender As Object, e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfDet.SetupEditor
        With vsfDet
            Select Case .Col
                Case vdetSel
                    Try
                        .StartEditing()
                        e.Cancel = False

                    Catch ex As Exception
                        MsgBox("Error" & vbCrLf & ex.Message, MsgBoxStyle.Critical, Me.Text)

                    End Try

            End Select
        End With
    End Sub

    Private Sub btnMasters_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles optMasters.CheckedChanged
        pnlMaster.Visible = True
        pnlTrn.Visible = False
    End Sub

    Private Sub btnTransactions_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles optTransaction.CheckedChanged
        pnlTrn.Visible = True
        pnlMaster.Visible = False
    End Sub
End Class