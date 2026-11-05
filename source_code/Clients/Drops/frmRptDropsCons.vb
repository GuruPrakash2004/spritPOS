Imports System.Data.oledb
Imports C1.Win.C1FlexGrid

Public Class frmRptDropsCons
    Dim clsSvt As New svtLibrary

    Const vDetCol0 = 0
    Const vDetCol1 = 1
    Const vDetCol2 = 2
    Const vDetCol3 = 3
    Const vDetCol4 = 4
    Const vDetTotCol = 5

    Private Sub btnExcel_Click(sender As Object, e As EventArgs) Handles btnExcel.Click
        ExportToXL(vsfDet)
    End Sub

    Private Sub btnView_Click(sender As Object, e As EventArgs) Handles btnView.Click
        vsfDet.Rows.Count = 4

        Dim strHeading As String
        strHeading = "Consolidated Report For the period " & Replace(getDateFormatInCond(dtpFromDate.Value), "'", "") & " - " & Replace(getDateFormatInCond(dtpToDate.Value), "'", "")
        Call initVsfForHdrs(gSettings.gCompAdd1 & " " & gSettings.gCompAdd2 & " " & gSettings.gCompAdd3, strHeading)

        showWaitCursor()
        Call LoadSalData()
        Call LoadPurData()
        Call LoadPettyCashData()
        Call ViewGrpSmryData()
        showNormalCursor()
    End Sub
    Private Sub LoadPurData()
        Dim strSQL As String = ""
        strSQL = "Select (Select top 1 case when LiqITEM='Y' then 'LIQUOR' else 'NON LIQUOR' end from trndtlPB d,mstitem i, mstitemgroup g where d.itemid = i.itemid and i.ITEMGroupID = g.itemgroupid and d.PBID=h.PBID) as itemGroup, suppname, h.SUPPINVNO, h.suppinvdate, sum(GRNDTOT) as SAmt"
        strSQL &= " from trnhdrPB h, mstsupp s"
        strSQL &= " where h.suppid = s.suppid"
        strSQL &= " And PBdate >= " & getDateFormatInCond(dtpFromDate.Value)
        strSQL &= " And PBdate <= " & getDateFormatInCond(dtpToDate.Value)
        'strSQL &= " and G.LiqITEM = 'Y'"
        strSQL &= " group by h.PBID,suppname, h.SUPPINVNO, h.suppinvdate"
        strSQL &= " order by 1,2"

        Dim rs As oledbDataReader = clsSvt.getDataReaderFromSQL(strSQL)
        'vsfDet.Rows.Count = 4
        vsfDet.Rows.Count = vsfDet.Rows.Count + 1
        vsfDet.SetData(vsfDet.Rows.Count - 1, vDetCol0, "Group")
        vsfDet.SetData(vsfDet.Rows.Count - 1, vDetCol1, "Supplier")
        vsfDet.SetData(vsfDet.Rows.Count - 1, vDetCol2, "Bill No.")
        vsfDet.SetData(vsfDet.Rows.Count - 1, vDetCol3, "Amount")
        setVsfCellRangeColour(vsfDet, vsfDet.Rows.Count - 1, vDetCol0, vsfDet.Rows.Count - 1, vDetCol3, Color.LightCoral)

        Do While rs.Read()
            vsfDet.Rows.Count = vsfDet.Rows.Count + 1
            vsfDet.SetData(vsfDet.Rows.Count - 1, vDetCol0, rs!ItemGroup)
            vsfDet.SetData(vsfDet.Rows.Count - 1, vDetCol1, rs!SuppName)
            vsfDet.SetData(vsfDet.Rows.Count - 1, vDetCol2, rs!SuppinvNo & "-" & rs!SuppInvDate)
            vsfDet.SetData(vsfDet.Rows.Count - 1, vDetCol3, rs!SAmt)
        Loop
        rs.Close()


        'Add Total PB value
        vsfDet.Rows.Count = vsfDet.Rows.Count + 2
        vsfDet.SetData(vsfDet.Rows.Count - 1, vDetCol0, "Total Purchase")
        setVsfCellColour(vsfDet, vsfDet.Rows.Count - 1, vDetCol0, Color.GreenYellow)
        strSQL = "Select sum(GrndTot) from TrnHdrPB "
        strSQL &= " where PBdate >= " & getDateFormatInCond(dtpFromDate.Value)
        strSQL &= " and PBdate <= " & getDateFormatInCond(dtpToDate.Value)
        vsfDet.SetData(vsfDet.Rows.Count - 1, vDetCol3, clsSvt.getDataDoubleValue(strSQL, 0, False))

        vsfDet.Cols(vDetCol3).Format = "0.00"

        vsfDet.AutoSizeCols()
        vsfDet.ExtendLastCol = False
    End Sub

    Private Sub LoadSalData()
        Dim strSQL As String = ""
        If gStrDBName.ToUpper <> "VLQMSQUENCH" Then
            strSQL = "Select 1 as lOrder,'LIQUOR' as ItemGroup, sum(sinvqty) as sQty, sum(ActItemAmt) as SAmt"
        Else
            strSQL = "Select 1 as lOrder,'LIQUOR' as ItemGroup, sum(sinvqty) as sQty,  sum((sinvqty * i.aroedper)  + ActItemAmt ) as SAmt"
        End If
        strSQL &= " from trndtlsinv d, trnhdrsinv h, mstitem i, mstitemgroup g"
        strSQL &= " where d.sinvid = h.sinvid"
        strSQL &= " and d.itemid = i.itemid"
        strSQL &= " and h.cancelled = 'N'"
        strSQL &= " and h.ISDelete = 'N'"
        strSQL &= " and i.ITEMGroupID = g.itemgroupid"
        strSQL &= " And sinvdate >= " & getDateFormatInCond(dtpFromDate.Value)
        strSQL &= " And sinvdate <= " & getDateFormatInCond(dtpToDate.Value)
        strSQL &= " and G.LiqITEM = 'Y'"
        strSQL &= " UNION ALL"
        If gStrDBName.ToUpper <> "VLQMSQUENCH" Then
            strSQL &= " Select 2 as lOrder,itemgroupname, sum(sinvqty) as sQty, sum(ActItemAmt) as SAmt"
        Else
            strSQL &= " Select 2 as lorder,itemgroupname, sum(sinvqty) as sQty,  sum((sinvqty * i.aroedper)  + ActItemAmt )as SAmt"
        End If
        strSQL &= " from trndtlsinv d, trnhdrsinv h, mstitem i, mstitemgroup g"
        strSQL &= " where d.sinvid = h.sinvid"
        strSQL &= " and d.itemid = i.itemid"
        strSQL &= " and h.cancelled = 'N'"
        strSQL &= " and h.ISDelete = 'N'"
        strSQL &= " and i.ITEMGroupID = g.itemgroupid"
        strSQL &= " And sinvdate >= " & getDateFormatInCond(dtpFromDate.Value)
        strSQL &= " And sinvdate <= " & getDateFormatInCond(dtpToDate.Value)
        strSQL &= " and G.LiqITEM <> 'Y'"
        strSQL &= " group by itemgroupname"
        strSQL &= " order by 1,2"

        Dim rs As OleDbDataReader = clsSvt.getDataReaderFromSQL(strSQL)
        vsfDet.Rows.Count = vsfDet.Rows.Count + 2
        vsfDet.SetData(vsfDet.Rows.Count - 1, vDetCol0, "Sales Data")
        setVsfCellColour(vsfDet, vsfDet.Rows.Count - 1, vDetCol0, Color.GreenYellow)

        vsfDet.Rows.Count = vsfDet.Rows.Count + 1
        vsfDet.SetData(vsfDet.Rows.Count - 1, vDetCol0, "Group")
        vsfDet.SetData(vsfDet.Rows.Count - 1, vDetCol1, "Qty")
        vsfDet.SetData(vsfDet.Rows.Count - 1, vDetCol2, "Amount")
        setVsfCellRangeColour(vsfDet, vsfDet.Rows.Count - 1, vDetCol0, vsfDet.Rows.Count - 1, vDetCol2, Color.LightCoral)

        Do While rs.Read()
            vsfDet.Rows.Count = vsfDet.Rows.Count + 1
            vsfDet.SetData(vsfDet.Rows.Count - 1, vDetCol0, rs!itemgroup)
            vsfDet.SetData(vsfDet.Rows.Count - 1, vDetCol1, rs!SQty)
            If Not IsDBNull(rs!samt) Then
                vsfDet.SetData(vsfDet.Rows.Count - 1, vDetCol2, Format(rs!samt, "#0.00"))

            End If
        Loop
        rs.Close()

        'Add RoundOffAmt
        vsfDet.Rows.Count = vsfDet.Rows.Count + 1
        vsfDet.SetData(vsfDet.Rows.Count - 1, vDetCol0, "Round Off")
        setVsfCellColour(vsfDet, vsfDet.Rows.Count - 1, vDetCol0, Color.Yellow)
        strSQL = "select sum(INVVALUE - tot) from TrnHdrSINV "
        strSQL &= " where ISDelete = 'N' and sinvdate >= " & getDateFormatInCond(dtpFromDate.Value)
        strSQL &= " And sinvdate <= " & getDateFormatInCond(dtpToDate.Value)
        vsfDet.SetData(vsfDet.Rows.Count - 1, vDetCol2, Format(clsSvt.getDataDoubleValue(strSQL, 0, False), "#0.00"))

        'Add Cash Payments
        vsfDet.Rows.Count = vsfDet.Rows.Count + 1
        vsfDet.SetData(vsfDet.Rows.Count - 1, vDetCol0, "Payment Mode: Cash")
        setVsfCellColour(vsfDet, vsfDet.Rows.Count - 1, vDetCol0, Color.Yellow)
        strSQL = "Select sum(Cashamt) from TrnHdrSINV "
        strSQL &= " where ISDelete = 'N' and sinvdate >= " & getDateFormatInCond(dtpFromDate.Value)
        strSQL &= " And sinvdate <= " & getDateFormatInCond(dtpToDate.Value)
        vsfDet.SetData(vsfDet.Rows.Count - 1, vDetCol2, Format(clsSvt.getDataDoubleValue(strSQL, 0, False), "#0.00"))

        'Add Card Payments
        vsfDet.Rows.Count = vsfDet.Rows.Count + 1
        vsfDet.SetData(vsfDet.Rows.Count - 1, vDetCol0, "Card")
        setVsfCellColour(vsfDet, vsfDet.Rows.Count - 1, vDetCol0, Color.Yellow)
        strSQL = "Select sum(Cardamt) from TrnHdrSINV "
        strSQL &= " where ISDelete = 'N' and sinvdate >= " & getDateFormatInCond(dtpFromDate.Value)
        strSQL &= " And sinvdate <= " & getDateFormatInCond(dtpToDate.Value)
        vsfDet.SetData(vsfDet.Rows.Count - 1, vDetCol2, Format(clsSvt.getDataDoubleValue(strSQL, 0, False), "#0.00"))

        'Add Credit Payments
        vsfDet.Rows.Count = vsfDet.Rows.Count + 1
        vsfDet.SetData(vsfDet.Rows.Count - 1, vDetCol0, "Credit Amt")
        setVsfCellColour(vsfDet, vsfDet.Rows.Count - 1, vDetCol0, Color.Yellow)
        strSQL = "Select sum(isnull(CreditAmt,0)) from TrnHdrSINV "
        strSQL &= " where ISDelete = 'N' and sinvdate >= " & getDateFormatInCond(dtpFromDate.Value)
        strSQL &= " And sinvdate <= " & getDateFormatInCond(dtpToDate.Value)
        vsfDet.SetData(vsfDet.Rows.Count - 1, vDetCol2, Format(clsSvt.getDataDoubleValue(strSQL, 0, False), "#0.00"))

        'Add Cheque Payments
        vsfDet.Rows.Count = vsfDet.Rows.Count + 1
        vsfDet.SetData(vsfDet.Rows.Count - 1, vDetCol0, "Cheque Amt")
        setVsfCellColour(vsfDet, vsfDet.Rows.Count - 1, vDetCol0, Color.Yellow)
        strSQL = "Select sum(isnull(ChqAmt,0)) from TrnHdrSINV "
        strSQL &= " where ISDelete = 'N' and sinvdate >= " & getDateFormatInCond(dtpFromDate.Value)
        strSQL &= " And sinvdate <= " & getDateFormatInCond(dtpToDate.Value)
        vsfDet.SetData(vsfDet.Rows.Count - 1, vDetCol2, Format(clsSvt.getDataDoubleValue(strSQL, 0, False), "#0.00"))

        'Add other payment modes
        strSQL = "Select Particulars, sum(Amount) as sAmt"
        strSQL &= " From TrnMultiAmt h, trnhdrsinv i"
        strSQL &= " Where h.TrnID = i.SINVID"
        strSQL &= " and ISDelete = 'N' and sinvdate >= " & getDateFormatInCond(dtpFromDate.Value)
        strSQL &= " And sinvdate <= " & getDateFormatInCond(dtpToDate.Value)
        strSQL &= " Group By Particulars"
        rs = clsSvt.getDataReaderFromSQL(strSQL)
        Do While rs.Read()
            vsfDet.Rows.Count = vsfDet.Rows.Count + 1
            vsfDet.SetData(vsfDet.Rows.Count - 1, vDetCol0, rs!Particulars)
            vsfDet.SetData(vsfDet.Rows.Count - 1, vDetCol2, Format(Val(rs!samt), "#0.00"))
        Loop
        rs.Close()

        'Add Total Sales value
        vsfDet.Rows.Count = vsfDet.Rows.Count + 1
        vsfDet.SetData(vsfDet.Rows.Count - 1, vDetCol0, "Total Sales")
        setVsfCellColour(vsfDet, vsfDet.Rows.Count - 1, vDetCol0, Color.Yellow)
        strSQL = "Select sum(InvValue) from TrnHdrSINV "
        strSQL &= " where ISDelete = 'N' and sinvdate >= " & getDateFormatInCond(dtpFromDate.Value)
        strSQL &= " And sinvdate <= " & getDateFormatInCond(dtpToDate.Value)
        vsfDet.SetData(vsfDet.Rows.Count - 1, vDetCol2, Format(clsSvt.getDataDoubleValue(strSQL, 0, False), "#0.00"))

        'vsfDet.Cols(vDetCol2).Format = "0.00"

        vsfDet.AutoSizeCols()
        vsfDet.ExtendLastCol = False
    End Sub

    Private Sub LoadPettyCashData()
        Dim strSQL As String = ""
        Dim rs As OleDbDataReader
        'vsfDet.Rows.Count = 4
        vsfDet.Rows.Count = vsfDet.Rows.Count + 2
        vsfDet.SetData(vsfDet.Rows.Count - 1, vDetCol0, "Petty Cash Data")
        setVsfCellColour(vsfDet, vsfDet.Rows.Count - 1, vDetCol0, Color.GreenYellow)

        'Add Opbalance
        vsfDet.Rows.Count = vsfDet.Rows.Count + 1
        vsfDet.SetData(vsfDet.Rows.Count - 1, vDetCol0, "OP Balance")
        setVsfCellColour(vsfDet, vsfDet.Rows.Count - 1, vDetCol0, Color.Yellow)
        strSQL = "Select sum(ramt-amt) as opBal "
        strSQL &= " From trnhdrexp h, trndtlexp d"
        strSQL &= " where h.expid = d.expid"
        strSQL &= " and h.EXPDate < " & getDateFormatInCond(dtpFromDate.Value)
        'vsfDet.SetData(vsfDet.Rows.Count - 1, vDetCol2, clsSvt.getDataDoubleValue(strSQL, 0, False))
        vsfDet.SetData(vsfDet.Rows.Count - 1, vDetCol2, Format(clsSvt.getDataDoubleValue(strSQL, 0, False), "#0.00"))

        vsfDet.Rows.Count = vsfDet.Rows.Count + 1
        vsfDet.SetData(vsfDet.Rows.Count - 1, vDetCol0, "Particulars")
        vsfDet.SetData(vsfDet.Rows.Count - 1, vDetCol1, "Expense")
        vsfDet.SetData(vsfDet.Rows.Count - 1, vDetCol2, "Receipt")
        setVsfCellRangeColour(vsfDet, vsfDet.Rows.Count - 1, vDetCol0, vsfDet.Rows.Count - 1, vDetCol2, Color.LightCoral)

        'Add Rcpts and exp for the day
        Dim sngEAmt As Double = 0, sngRAmt As Double = 0
        strSQL = "Select ledname, sum(AMT) as eAmt, sum(RAMT) as rcptAmt"
        If dtpFromDate.Value = dtpToDate.Value Then
            strSQL &= " ,Remarks"
        End If
        strSQL &= " From trnhdrexp h, trndtlexp d, mstled l"
        strSQL &= " where h.expid = d.expid"
        strSQL &= " and d.ledid = l.ledid"
        strSQL &= " and expdate >= " & getDateFormatInCond(dtpFromDate.Value)
        strSQL &= " and expdate <= " & getDateFormatInCond(dtpToDate.Value)
        strSQL &= " group by ledname"
        If dtpFromDate.Value = dtpToDate.Value Then
            strSQL &= " ,Remarks"
        End If
        strSQL &= " order by 1"
        rs = clsSvt.getDataReaderFromSQL(strSQL)
        Do While rs.Read()
            vsfDet.Rows.Count = vsfDet.Rows.Count + 1
            vsfDet.SetData(vsfDet.Rows.Count - 1, vDetCol0, rs!LedName)
            vsfDet.SetData(vsfDet.Rows.Count - 1, vDetCol2, Format(rs!rcptAmt, "#0.00"))
            vsfDet.SetData(vsfDet.Rows.Count - 1, vDetCol1, Format(rs!eAmt, "#0.00"))
            If dtpFromDate.Value = dtpToDate.Value Then
                vsfDet.SetData(vsfDet.Rows.Count - 1, vDetCol3, rs!Remarks)
            End If
            sngEAmt = sngEAmt + rs!EAmt
            sngRAmt = sngRAmt + rs!RcptAmt
        Loop
        rs.Close()

        'Add Closing Balance
        vsfDet.Rows.Count = vsfDet.Rows.Count + 1
        vsfDet.SetData(vsfDet.Rows.Count - 1, vDetCol0, "Closing Balance")
        setVsfCellColour(vsfDet, vsfDet.Rows.Count - 1, vDetCol0, Color.Yellow)
        strSQL = "Select sum(ramt-amt) as opBal "
        strSQL &= " From trnhdrexp h, trndtlexp d"
        strSQL &= " where h.expid = d.expid"
        strSQL &= " and h.EXPDate <= " & getDateFormatInCond(dtpToDate.Value)
        'vsfDet.SetData(vsfDet.Rows.Count - 1, vDetCol2, clsSvt.getDataDoubleValue(strSQL, 0, False))
        vsfDet.SetData(vsfDet.Rows.Count - 1, vDetCol3, Format(clsSvt.getDataDoubleValue(strSQL, 0, False), "#0.00"))
        vsfDet.SetData(vsfDet.Rows.Count - 1, vDetCol1, Format(sngEAmt, "#0.00"))
        vsfDet.SetData(vsfDet.Rows.Count - 1, vDetCol2, Format(sngRAmt, "#0.00"))

        'vsfDet.Cols(vDetRAmt).Format = "0.00"
        'vsfDet.Cols(vDetExpAmt).Format = "0.00"

        vsfDet.AutoSizeCols()
        vsfDet.ExtendLastCol = False
    End Sub

    Private Sub frmRptDropsCons_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim strHeading As String
        dtpFromDate.Value = getSystemDate()
        dtpToDate.Value = dtpFromDate.Value
        strHeading = "Consolidated Report For the period " & Replace(getDateFormatInCond(dtpFromDate.Value), "'", "") & " - " & Replace(getDateFormatInCond(dtpToDate.Value), "'", "")
        Call initVsfForHdrs(strHeading, "")

    End Sub

    Private Sub initVsfForHdrs(ByVal argHdrLine1 As String, ByVal argHdrLine2 As String)
        Dim i As Integer

        With vsfDet
            .Cols.Count = vDetTotCol
            For i = 0 To .Cols.Count - 1
                ' .SetData(3, i, .GetData(0, i))

                .SetData(0, i, gSettings.gCompName)
                .SetData(1, i, argHdrLine1)
                .SetData(2, i, argHdrLine2)
            Next
            Dim csf1 As C1.Win.C1FlexGrid.CellStyle = vsfDet.Styles.Add("F1")
            csf1.BackColor = Color.Beige

            .Rows(0).Style = csf1
            .Rows(1).Style = csf1
            .Rows(2).Style = csf1

            If argHdrLine1 = "" Then
                .Rows(1).Visible = False
            Else
                .Rows(1).Visible = True
            End If

            If argHdrLine2 = "" Then
                .Rows(2).Visible = False
            Else
                .Rows(2).Visible = True
            End If

            .Rows(0).AllowMerging = True
            .Rows(1).AllowMerging = True
            .Rows(2).AllowMerging = True
            .Rows(3).AllowMerging = True

        End With
        vsfDet.Rows.Count = 4
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        frmMain.CloseSelectedTabPage(Me)
    End Sub

    Private Sub vsfDet_BeforeEdit(sender As Object, e As RowColEventArgs) Handles vsfDet.BeforeEdit
        e.Cancel = True
    End Sub

    Private Sub ViewGrpSmryData()
        Dim strSQL As String = ""
        Dim strItemList As String = ""
        Dim rs As OleDbDataReader
        showWaitCursor()
        If chkUpdateFIFOrate.Checked Then Call clsSvt.execSQLStatement("exec mSp_UpdateFIFORate")
        'showNormalCursor()

        vsfDet.Rows.Count = vsfDet.Rows.Count + 1
        vsfDet.SetData(vsfDet.Rows.Count - 1, vDetCol0, "Group Wise Summary Balance")
        setVsfCellRangeColour(vsfDet, vsfDet.Rows.Count - 1, vDetCol0, vsfDet.Rows.Count - 1, vDetCol3, Color.LightGreen)

        vsfDet.Rows.Count = vsfDet.Rows.Count + 1
        vsfDet.SetData(vsfDet.Rows.Count - 1, vDetCol0, "Category")
        vsfDet.SetData(vsfDet.Rows.Count - 1, vDetCol1, "ItemGroup Name")
        vsfDet.SetData(vsfDet.Rows.Count - 1, vDetCol2, "Opening Value")
        vsfDet.SetData(vsfDet.Rows.Count - 1, vDetCol3, "Closing Value")
        setVsfCellRangeColour(vsfDet, vsfDet.Rows.Count - 1, vDetCol0, vsfDet.Rows.Count - 1, vDetCol3, Color.LightCoral)
        If gStrDBName.ToUpper <> "VLQMSQUENCH" Then
            strSQL = "Select itemcat, itemgroupname,sum(opqty * StkRate)as opValue,sum(clqty * stkrate)  as clvalue from ("
        Else
            strSQL = "Select itemcat, itemgroupname,sum(opqty * StkRate)+SUM(OPAROEDPER) as opValue,sum(clqty * stkrate) +SUM(CLAROEDPER) as clvalue from ("

        End If
        strSQL &= " Select 'Liquor' as ItemCat, ItemGroupName, "

        If gStrDBName.ToUpper <> "VLQMSQUENCH" Then
            strSQL = strSQL & " FifoRate as StkRate "
            strSQL &= ", sum(Qty * StkMultiplier) as opQty, 0.00 as clQty "
        Else
            strSQL = strSQL & " FifoRate as StkRate "
            strSQL &= ", sum(Qty * StkMultiplier) as opQty, 0.00 as clQty,sum(Qty * StkMultiplier)*AROEDPER AS OPAROEDPER,0 AS CLAROEDPER"
        End If

        strSQL = strSQL & " from MstItemGroup g, MstItem i, UtlStockList s"
        strSQL = strSQL & " where s.ItemID = i.ItemID"
        strSQL = strSQL & " And i.ItemGroupID = g.ItemGroupId "
        strSQL = strSQL & " And DocDate < " & getDateFormatInCond(dtpFromDate.Value)
        strSQL = strSQL & " And i.Inactive='N'"
        strSQL = strSQL & " And g.LiqItem = 'Y'"
        If gStrDBName.ToUpper <> "VLQMSQUENCH" Then
            strSQL = strSQL & " group by ItemGroupName, s.itemid, FifoRate "
        Else
            strSQL = strSQL & " group by ItemGroupName, s.itemid, fiforate ,AROEDPER"
        End If
        strSQL &= " Union All "

        strSQL &= "Select 'Liquor' as ItemCat, ItemGroupName, "

        If gStrDBName.ToUpper <> "VLQMSQUENCH" Then
            strSQL = strSQL & " FifoRate as StkRate "
            strSQL &= ", 0.00, sum(Qty * StkMultiplier) as clQty  "
        Else
            strSQL = strSQL & " FifoRate as StkRate "
            strSQL &= ", 0.00, sum(Qty * StkMultiplier) as clQty,0 AS  OPAROEDPER,sum(Qty * StkMultiplier)*AROEDPER AS CLAROEDPER "
        End If

        strSQL = strSQL & " from MstItemGroup g, MstItem i, UtlStockList s"
        strSQL = strSQL & " where s.ItemID = i.ItemID"
        strSQL = strSQL & " And i.ItemGroupID = g.ItemGroupId "
        strSQL = strSQL & " And DocDate <= " & getDateFormatInCond(dtpToDate.Value)
        strSQL = strSQL & " And i.Inactive='N'"
        strSQL = strSQL & " And g.LiqItem = 'Y'"
        If gStrDBName.ToUpper <> "VLQMSQUENCH" Then
            strSQL = strSQL & " group by ItemGroupName, s.itemid, FifoRate "
        Else
            strSQL = strSQL & " group by ItemGroupName, s.itemid, fiforate,AROEDPER"
        End If
        strSQL &= " union all "
        strSQL &= "Select 'Non-Liquor' as ItemCat, '',"

        If gStrDBName.ToUpper <> "VLQMSQUENCH" Then
            strSQL = strSQL & " FifoRate as StkRate "
            strSQL &= ", sum(Qty * StkMultiplier) as opQty, 0.00 as clQty "
        Else
            strSQL = strSQL & " FifoRate as StkRate "
            strSQL &= ", sum(Qty * StkMultiplier) as opQty, 0.00 as clQty,sum(Qty * StkMultiplier)*AROEDPER AS OPAROEDPER,0 AS CLAROEDPER"
        End If

        strSQL = strSQL & " from MstItemGroup g, MstItem i, UtlStockList s"
        strSQL = strSQL & " where s.ItemID = i.ItemID"
        strSQL = strSQL & " And i.ItemGroupID = g.ItemGroupId "
        strSQL = strSQL & " And DocDate < " & getDateFormatInCond(dtpToDate.Value)
        strSQL = strSQL & " And i.Inactive='N'"
        strSQL = strSQL & " And g.LiqItem = 'N'"
        If gStrDBName.ToUpper <> "VLQMSQUENCH" Then
            strSQL = strSQL & " group by s.itemid, FifoRate "
        Else
            strSQL = strSQL & " group by s.itemid, fiforate,AROEDPER "
        End If
        strSQL &= " Union All "
        strSQL &= "Select 'Non-Liquor' as ItemCat, '',"
        If gStrDBName.ToUpper <> "VLQMSQUENCH" Then
            strSQL = strSQL & " FifoRate as StkRate "
            strSQL &= ",0.00, sum(Qty * StkMultiplier) as clQty "
        Else
            strSQL = strSQL & " FifoRate as StkRate "
            strSQL &= ",0.00, sum(Qty * StkMultiplier) as clQty,0 AS  OPAROEDPER,sum(Qty * StkMultiplier)*AROEDPER AS CLAROEDPER"
        End If

        strSQL = strSQL & " from MstItemGroup g, MstItem i, UtlStockList s"
        strSQL = strSQL & " where s.ItemID = i.ItemID"
        strSQL = strSQL & " And i.ItemGroupID = g.ItemGroupId "
        strSQL = strSQL & " And DocDate <= " & getDateFormatInCond(dtpToDate.Value)
        strSQL = strSQL & " And i.Inactive='N'"
        strSQL = strSQL & " And g.LiqItem = 'N' "
        If gStrDBName.ToUpper <> "VLQMSQUENCH" Then
            strSQL = strSQL & " group by s.itemid, fiforate) a"
        Else
            strSQL = strSQL & " group by s.itemid, fiforate,AROEDPER ) a"
        End If


        strSQL = strSQL & " group by itemcat, itemgroupname order by 1,2"

        rs = clsSvt.getDataReaderFromSQL(strSQL)

        If Not rs.HasRows Then
            rs.Close()
            Exit Sub
        End If

        vsfDet.SetData(vsfDet.Rows.Count - 1, vDetCol0, "Category")
        vsfDet.SetData(vsfDet.Rows.Count - 1, vDetCol1, "ItemGroup Name")
        vsfDet.SetData(vsfDet.Rows.Count - 1, vDetCol2, "Opening Value")
        vsfDet.SetData(vsfDet.Rows.Count - 1, vDetCol3, "Closing Value")

        Dim dblOpBal As Double = 0, dblClBal As Double = 0
        Dim strCat As String = ""

        With rs
            Do While .Read
                If strCat = "" Then
                    strCat = !Itemcat
                Else
                    If strCat <> !Itemcat Then
                        strCat = !Itemcat
                        vsfDet.Rows.Count = vsfDet.Rows.Count + 1
                        vsfDet.SetData(vsfDet.Rows.Count - 1, vDetCol1, "Total")
                        vsfDet.SetData(vsfDet.Rows.Count - 1, vDetCol2, Math.Round(dblOpBal, 2))
                        vsfDet.SetData(vsfDet.Rows.Count - 1, vDetCol3, Math.Round(dblClBal, 2))
                        setVsfCellRangeColour(vsfDet, vsfDet.Rows.Count - 1, vDetCol0, vsfDet.Rows.Count - 1, vDetCol3, Color.LightPink)
                        dblOpBal = 0
                        dblClBal = 0
                    End If
                End If
                vsfDet.Rows.Count = vsfDet.Rows.Count + 1
                vsfDet.SetData(vsfDet.Rows.Count - 1, vDetCol0, !ItemCat)
                vsfDet.SetData(vsfDet.Rows.Count - 1, vDetCol1, !ItemGroupName)
                vsfDet.SetData(vsfDet.Rows.Count - 1, vDetCol2, Math.Round(!opValue, 2))
                vsfDet.SetData(vsfDet.Rows.Count - 1, vDetCol3, Math.Round(!ClValue, 2))
                dblOpBal = dblOpBal + !opValue
                dblClBal = dblClBal + !ClValue
            Loop
            .Close()

            vsfDet.Rows.Count = vsfDet.Rows.Count + 1
            vsfDet.SetData(vsfDet.Rows.Count - 1, vDetCol1, "Total")
            vsfDet.SetData(vsfDet.Rows.Count - 1, vDetCol2, Math.Round(dblOpBal, 2))
            vsfDet.SetData(vsfDet.Rows.Count - 1, vDetCol3, Math.Round(dblClBal, 2))
            setVsfCellRangeColour(vsfDet, vsfDet.Rows.Count - 1, vDetCol0, vsfDet.Rows.Count - 1, vDetCol3, Color.LightPink)
        End With

        vsfDet.AutoSizeCols()
    End Sub
End Class