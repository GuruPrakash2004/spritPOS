
Public Class frmUtlDeleteNonLiqItems
    Dim clsSVT As New svtLibrary
    Const vDetItemName = 0
    Const vDetLiqType = 1
    Const vDetStatus = 2

    Const vGrpID = 0
    Const vGrpName = 1

    Public strList As String
    Dim strItemGroupList As String
    Private Sub btnSelItemGroup_Click(sender As Object, e As EventArgs) Handles btnSelItemGroup.Click
        Dim frmUS As New frmUtlSingleSel
        Dim strsql As String

        strsql = "Select ItemGroupCode from MstItemGroup  where itemgroupid in (select itemgroupid from mstitem) order by 1"
        With frmUS
            .Text = "Select ItemGroup"
            Call .FillViewVsf(Me, strsql, txtItemGroup, 0, -1)
            .ShowDialog()

            Call loadItems()
        End With
    End Sub
    Private Sub loadItems()
        btnDelete.Visible = False
        If txtItemGroup.Text = "" Then Exit Sub
        Dim rs As OleDb.OleDbDataReader
        rs = clsSVT.getDataReaderFromSQL("Select itemname, liqType from mstitem i, mstitemgroup g where itemgroupcode = '" & txtItemGroup.Text & "' and i.itemgroupid = g.itemgroupid order by 1")
        btnDelete.Visible = True
        vsfDet.Rows.Count = 1
        Do While rs.Read
            vsfDet.Rows.Count = vsfDet.Rows.Count + 1
            vsfDet.SetData(vsfDet.Rows.Count - 1, vDetItemName, rs!itemName)
            vsfDet.SetData(vsfDet.Rows.Count - 1, vDetLiqType, rs!LiqType)
            Select Case rs!LiqType.ToString.ToUpper
                Case "OTHERS", ""
                Case Else
                    vsfDet.SetData(vsfDet.Rows.Count - 1, vDetStatus, "Type is of LIQUOUR, cannot be deleted!")
                    btnDelete.Visible = False
            End Select
        Loop
        If vsfDet.Rows.Count = 1 Then btnDelete.Visible = False
        lblCnt.Text = "No. of Items : " & vsfDet.Rows.Count - 1
        rs.Close()
    End Sub

    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        If MsgBox("Do you want to delete the listed items." & vbCrLf & "This will delete items, Stock and transactions. Are you Sure?", MsgBoxStyle.Question + MsgBoxStyle.YesNo, Me.Text) = MsgBoxResult.No Then Exit Sub
        Dim strSQL As String
        Dim intItemGroupID As Integer
        intItemGroupID = clsSVT.getDataLongValue("Select ItemGroupID from mstitemgroup where itemGroupCode = '" & txtItemGroup.Text & "'", 0, False)
        Dim svtTrn As OleDb.OleDbTransaction
        svtTrn = svtConn.BeginTransaction(IsolationLevel.ReadCommitted)
        Try
            strSQL = "Delete From mstitem Where itemgroupid = " & intItemGroupID
            clsSVT.execSQLStatement(strSQL, svtTrn)

            strSQL = "Delete From mstitemgroup Where itemgroupid = " & intItemGroupID
            clsSVT.execSQLStatement(strSQL, svtTrn)

            clsSVT.execSQLStatement("Delete From TrnDtlSINV Where itemid Not in (select itemid from mstitem)", svtTrn)
            clsSVT.execSQLStatement("Delete From trnhdrsinv Where sinvid Not in (select sinvid from TrnDtlSINV)", svtTrn)

            clsSVT.execSQLStatement("Delete From TrnDtlPB Where itemid Not in (select itemid from mstitem)", svtTrn)
            clsSVT.execSQLStatement("Delete From TrnHdrPB Where pbid Not in (select pbid from TrnDtlPB)", svtTrn)

            clsSVT.execSQLStatement("Update UtlStocklist set qty = 0 where itemid Not in (select itemid from mstitem)", svtTrn)
            If Not clsSVT.execSQLStatement("Delete From UtlStockList Where itemid Not in (select itemid from mstitem)", svtTrn) Then
                svtTrn.Rollback()
                Exit Sub
            End If
            clsSVT.execSQLStatement("Delete From TrnQOH Where itemid Not in (select itemid from mstitem)", svtTrn)

            clsSVT.execSQLStatement("Truncate table tmpDeleteNL", svtTrn)
            strSQL = "insert into TmpDeleteNL (sinvid, totqty, dtlqty, dtlamt, cashamt, totamt, totbasicamt, tot, invvalue) "
            strSQL &= " Select h.sinvid, TotQty, sum(SINVQty) As dtlQty, sum(SINVAmt) As dtlAmt, cashamt, totamt, "
            strsql &= " totbasicamt, tot, invvalue"
            strSQL &= " From trnhdrsinv h, trndtlsinv d"
            strSQL &= " Where h.sinvid = d.sinvid"
            strSQL &= " Group By h.sinvid, totqty, cashamt, totamt, totbasicamt, tot, invvalue"
            strSQL &= " having totqty <> sum(sinvqty)"
            clsSVT.execSQLStatement(strSQL, svtTrn)

            strSQL = "Update trnhdrsinv Set totqty = t.dtlQty, CASHAMT = t.dtlAmt, TOTAMT = t.dtlAmt, "
            strSQL &= " TOTBASICAMT = t.dtlAmt, tot = t.dtlAmt, INVVALUE = t.dtlAmt "
            strSQL &= " from tmpDeleteNL t"
            strSQL &= " where trnhdrsinv.sinvid = t.sinvid"
            clsSVT.execSQLStatement(strSQL, svtTrn)

            svtTrn.Commit()
            btnDelete.Visible = False
            MsgBox("Successfully deleted the above list of items!", MsgBoxStyle.Information, Me.Text)
            vsfDet.Rows.Count = 1
            txtItemGroup.Text = ""

        Catch ex As Exception
            svtTrn.Rollback()
            MsgBox(ex.Message, MsgBoxStyle.Critical, Me.Text)
        End Try
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub frmUtlDeleteNonLiqItems_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim strSQL As String
        Dim rsG As OleDb.OleDbDataReader
        If Not fnTablePresent("tmpDeleteNL") Then
            strsql = "create table tmpDeleteNL (sinvid int, TotQty numeric(18,2), dtlQty numeric(18,2), dtlAmt numeric(18,2), cashamt numeric(18,2), totamt numeric(18,2), totbasicamt numeric(18,2), tot numeric(18,2), invvalue numeric(18,2))"
            clsSVT.execSQLStatement(strsql)
        End If
        If Not fnTablePresent("TmpGrpQOH") Then
            strSql = "Create table TmpGrpQOH (ItemID int, ClBal numeric(18,2))"
            Call clsSVT.execSQLStatement(strSql)
        End If
        If Not fnTablePresent("TmpGrpList") Then
            strSQL = "Create table TmpGrpList (ItemGroupID int)"
            Call clsSVT.execSQLStatement(strSQL)
        End If

        With vsfDet
            .Rows.Count = 1
            .Cols.Count = 3
            .Cols.Fixed = 0
            .SetData(0, vDetItemName, "ItemName")
            .Cols(vDetItemName).Width = 300
            .SetData(0, vDetLiqType, "LiqType")
            .Cols(vDetLiqType).Width = 100
            .SetData(0, vDetStatus, "Status")
            .ExtendLastCol = True
        End With
        With vsfGroup
            .Rows.Count = 1
            .Cols.Count = 2
            .Cols.Fixed = 0
            .ExtendLastCol = True
            .Cols(0).Visible = False
            .SetData(0, vGrpName, "Group")
        End With
        btnCarryOver.Visible = False
        btnDelete.Visible = False
        strItemGroupList = ""
        rsG = clsSVT.getDataReaderFromSQL("Select distinct itemGroupID from TmpGrpList")
        With rsG
            Do While .Read
                strItemGroupList = strItemGroupList & !ItemGroupID & ","
            Loop
        End With
        If strItemGroupList <> "" Then
            strItemGroupList = Mid(strItemGroupList, 1, Len(strItemGroupList) - 1)
            Call loadItemGroups()
            btnCarryOver.Visible = True
        End If
    End Sub

    Private Sub chkItemGroup_CheckedChanged(sender As Object, e As System.EventArgs) Handles chkItemGroup.CheckedChanged
        Dim strSQL As String
        strList = ""
        strItemGroupList = ""
        If Not chkItemGroup.Checked Then Exit Sub
        Dim frmSel As New frmUtlMultiSelection

        strSQL = "Select distinct 0 as sel, ItemGroupID, ItemGroupName, GroupOrderBy from MstItemGroup where ItemGroupID > 0 and "
        strSQL = strSQL & " InActive = 'N' order by GroupOrderBy"
        Call frmSel.InitVsfSel(strSQL, 1, 1, True, Me, False)
        frmSel.Text = "Select [Multiple] Item Groups"
        frmSel.vsfDet.SetData(0, 0, "Sel")
        frmSel.ShowDialog()
        strItemGroupList = strList
        Call loadItemGroups
    End Sub

    Private Sub loadItemGroups()
        Dim strSQL As String
        If strItemGroupList = "" Then
            Exit Sub
        End If
        strSQL = "Select itemGroupID, itemgroupName from mstitemgroup where itemGroupid in (" & strItemGroupList & ") order by 1"
        Dim rs As OleDb.OleDbDataReader
        rs = clsSVT.getDataReaderFromSQL(strSQL)
        vsfGroup.Rows.Count = 1
        With rs
            Do While .Read
                vsfGroup.Rows.Count = vsfGroup.Rows.Count + 1
                vsfGroup.SetData(vsfGroup.Rows.Count - 1, vGrpID, !ItemGroupID)
                vsfGroup.SetData(vsfGroup.Rows.Count - 1, vGrpName, !ItemGroupName)
                btnCarryOver.Visible = True
            Loop
            .Close()
        End With
    End Sub


    Private Sub btnCarryOver_Click(sender As Object, e As EventArgs) Handles btnCarryOver.Click
        Dim strSql As String
        If strItemGroupList = "" Then
            MsgBox("No groups selected!", MsgBoxStyle.Information, Me.Text)
            Exit Sub
        End If

        If MsgBox("Do you want to delete the transactions of listed item groups." & vbCrLf & "This will delete transactions and carry over stock. Are you Sure?", MsgBoxStyle.Question + MsgBoxStyle.YesNo, Me.Text) = MsgBoxResult.No Then Exit Sub

        Call clsSVT.execSQLStatement("Truncate table TmpGrpQOH")
        strSql = "Insert into TmpGrpQOH (ItemID, ClBal)"
        strSql &= " Select u.itemid, sum(Qty * StkMultiplier) from UtlstockList u, mstitem i "
        strSql &= " where u.itemid = i.itemid"
        strSql &= " and i.itemgroupid in (" & strItemGroupList & ")"
        strSql &= " Group by u.itemid"
        strSql &= " having sum(Qty * StkMultiplier) > 0"
        Call clsSVT.execSQLStatement(strSql)

        Call clsSVT.execSQLStatement("Truncate table TmpGrpList")
        strSql = "Insert into TmpGrpList (ItemGroupID)"
        strSql &= " Select ItemGroupID from Mstitem"
        strSql &= " where itemgroupid in (" & strItemGroupList & ")"
        Call clsSVT.execSQLStatement(strSql)

        Dim svtTrn As OleDb.OleDbTransaction
        svtTrn = svtConn.BeginTransaction(IsolationLevel.ReadCommitted)
        Try
            clsSVT.execSQLStatement("Delete From TrnDtlSINV Where itemid in (select itemid from mstitem where itemgroupid in (" & strItemGroupList & "))", svtTrn)
            clsSVT.execSQLStatement("Delete From trnhdrsinv Where sinvid Not in (select sinvid from TrnDtlSINV)", svtTrn)

            clsSVT.execSQLStatement("Delete From TrnDtlPB Where itemid in (select itemid from mstitem where itemgroupid in (" & strItemGroupList & "))", svtTrn)
            clsSVT.execSQLStatement("Delete From TrnHdrPB Where pbid Not in (select pbid from TrnDtlPB)", svtTrn)

            clsSVT.execSQLStatement("Update UtlStocklist set qty = 0 where itemid in (select itemid from mstitem where itemgroupid in (" & strItemGroupList & "))", svtTrn)
            If Not clsSVT.execSQLStatement("Delete From UtlStockList Where itemid in (select itemid from mstitem where itemgroupid in (" & strItemGroupList & "))", svtTrn) Then
                svtTrn.Rollback()
                Exit Sub
            End If
            clsSVT.execSQLStatement("Delete From TrnQOH Where itemid in (select itemid from mstitem where itemgroupid in (" & strItemGroupList & "))", svtTrn)

            Call updatePB(svtTrn)

            clsSVT.execSQLStatement("Truncate table tmpDeleteNL", svtTrn)
            strSQL = "insert into TmpDeleteNL (sinvid, totqty, dtlqty, dtlamt, cashamt, totamt, totbasicamt, tot, invvalue) "
            strSQL &= " Select h.sinvid, TotQty, sum(SINVQty) As dtlQty, sum(SINVAmt) As dtlAmt, cashamt, totamt, "
            strSQL &= " totbasicamt, tot, invvalue"
            strSQL &= " From trnhdrsinv h, trndtlsinv d"
            strSQL &= " Where h.sinvid = d.sinvid"
            strSQL &= " Group By h.sinvid, totqty, cashamt, totamt, totbasicamt, tot, invvalue"
            strSQL &= " having totqty <> sum(sinvqty)"
            clsSVT.execSQLStatement(strSQL, svtTrn)

            strSQL = "Update trnhdrsinv Set totqty = t.dtlQty, CASHAMT = t.dtlAmt, TOTAMT = t.dtlAmt, "
            strSQL &= " TOTBASICAMT = t.dtlAmt, tot = t.dtlAmt, INVVALUE = t.dtlAmt "
            strSQL &= " from tmpDeleteNL t"
            strSQL &= " where trnhdrsinv.sinvid = t.sinvid"
            clsSVT.execSQLStatement(strSQL, svtTrn)

            svtTrn.Commit()
            btnCarryOver.Visible = False
            clsSVT.execSQLStatement("truncate table tmpGrpList")
            strSql = "Insert into TmpGrpList (ItemGroupID) "
            strSql &= "select itemgroupid from mstitem where itemgroupid in (" & strItemGroupList & ")"
            clsSVT.execSQLStatement(strSql)

            MsgBox("Successfully deleted the transactions and carried op balance.!", MsgBoxStyle.Information, Me.Text)

        Catch ex As Exception
            svtTrn.Rollback()
            MsgBox(ex.Message, MsgBoxStyle.Critical, Me.Text)
        End Try
    End Sub

    Private Sub updatePB(argTrn As OleDb.OleDbTransaction)
        Dim strSQL As String = "", intPBID As Long, intSuppID As Integer
        Dim strTNo As String
        strTNo = TrnNoAutoGen("TrnHdrPB", "PBNO", "", getSystemDate(argTrn), 1276,,, argTrn)

        intSuppID = 0

        strTNo = getFmtDocNo("", strTNo, getSystemDate(argTrn))
        strSQL = "insert into trnhdrpb "
        strSQL &= " (PBNO,Prefix,PrintPBNO,PBDate,SUPPID, "
        strSQL &= " PartyName,PartyAdd1,PartyAdd2,PartyAdd3,PartyAdd4,PartyCity,PartyState,PartyPinCode, "
        strSQL &= " PartyCountry,PartyCName,PartyCMob,PartyCEmail,PartyCDesig,Authorised,AuthBy,AuthOn, "
        strSQL &= " LOCID,ChangedBy,ChangedOn,Cancelled,AddedBy,AddedOn,AMTINWRDS,RMKS,SUPPINVNO, "
        strSQL &= " SUPPINVDATE,SUPPINVVALUE,TOTAMT,OTHAMT,GRNDTOT,TOTCGST,TOTSGST,TOTIGST,PURCHASETYPE) values ("
        strSQL &= "'" & strTNo & "',"
        strSQL &= "'',"
        strSQL &= "'" & strTNo & "',"
        strSQL &= "'" & getDateFormatInInsert(getSystemDate(argTrn)) & "',"
        strSQL &= intSuppID & ",'-','-','-','-','-','-','-','-','-','-','-','-','-','Y','ADMIN',"
        strSQL &= "'" & getDateFormatInInsert(getSystemDate(argTrn)) & "',1,'',Null,'N','ADMIN',"
        strSQL &= "'" & getDateFormatInInsert(getSystemDate(argTrn)) & "',"
        strSQL &= "'','Stock','',Null,0,0,0,0,0,0,0,'')"
        Call clsSVT.execSQLStatement(strSQL, argTrn)

        intPBID = clsSVT.getDataLongValue("Select max(PBID) from TrnHdrPB", 0, False, argTrn)

        strSQL = "Insert into TrnDtlPB (PBID, SlNo, ITEMID,UOMID,ItemSlNO,StkQty,PBQty,PBRate,PBAmt,NOOFCASES,NOOFBOTTLES,"
        strSQL &= "CGSTPER,CGSTAMT,SGSTPER,SGSTAMT,IGSTPER,IGSTAMT,NETAMT,MRP,PBQtyInLtrs) "
        strSQL &= "Select " & intPBID & ", ROW_NUMBER() over (order by t.itemid), t.itemid, uomid, ROW_NUMBER() over (order by t.itemid),"
        strSQL &= "clbal,clbal,i.purrate, i.purrate * clbal, 1, 1, "
        strSQL &= "0,0," 'cgst
        strSQL &= "0,0," 'sgst
        strSQL &= "0,0," 'igst
        strSQL &= "0,0,0 "
        strSQL &= " from tmpGrpQOH t, mstitem i"
        strSQL &= " where t.itemid = i.itemid"
        Call clsSVT.execSQLStatement(strSQL, argTrn)

        strSQL = " Insert Into UtlStockList "
        strSQL &= " (ItemID, StkMultiplier, DocType, DocDate, DocID, DocNo, Particulars, Qty, LtrQty, Rate, LocID) "
        strSQL &= " Select ItemID, '1', 'PB' , PBDate, h.PBID, PBNO, PartyName, PBQty, PBQTYINLTRS, PBRate, h.LOCID "
        strSQL &= " From TrnDtlPB d, TrnHdrPB h "
        strSQL &= " Where d.PBID = h.PBID "
        strSQL &= " and d.PBID = " & intPBID
        Call clsSVT.execSQLStatement(strSQL, argTrn)
    End Sub
End Class