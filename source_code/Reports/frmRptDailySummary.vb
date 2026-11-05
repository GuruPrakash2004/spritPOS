Imports Microsoft.Office.Interop
Imports C1.Win.C1FlexGrid
Imports System.IO
Imports System.ComponentModel

Public Class frmRptDailySummary

    Dim clsSVT As New svtLibrary
    Dim strRights As String

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
            .Rows.Count = 0
            .Rows.Fixed = 0
            .Cols.Count = 3
            .Cols.Fixed = 0
            .Cols(0).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.LeftCenter
            .Cols(1).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter
            .Cols(2).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter
        End With

        If gStrDBName.ToUpper = "VLQMSBOOZE" Or gStrDBName.ToUpper = "VLQMSBVK" Then
            pnlOptions.Visible = True
        Else
            pnlOptions.Visible = False
        End If
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

    Private Sub FillData()
        Dim rs As oledb.oledbDataReader
        Dim strSQL As String, blnFound As Boolean
        Dim blnIncludeNonLiq As Boolean = False

        If chkNonLiqour.Checked = True Then
            blnIncludeNonLiq = True
        Else
            blnIncludeNonLiq = False
        End If
        'select ITEMID from mstitem where ITEMGroupID in (select ITEMGROUPID from mstitemgroup where LiqItem='N')

        With vsfView
            addRow("Summary for the day " & getDateFormatInCond(dtpDay.Value).Replace("'", ""), "", "", True, False)
            If optPurRate.Checked Then
                strSQL = "select (select count(distinct h.SINVID) from TrnHdrSINV h,TrnDtlSinv d Where d.SINVID=h.SINVID And SINVDate = " & getDateFormatInCond(dtpDay.Value) & " And Cancelled = 'N' AND IsDelete='N'"
                If Not blnIncludeNonLiq Then
                    strSQL &= " And d.itemid in (Select ItemID from MstItem i where i.ITEMGroupID in (select ITEMGROUPID from mstitemgroup where LiqItem='Y')) "
                End If
                strSQL &= " ) as totbills, sum(totSales) as TotSales, sum(TotCashAmt) as TotCashAmt,0 as TotCardAmt, 0 as OtherAmt from (Select Count(SINVNo) as TotBills, (isnull((Select top 1 PBRate from vwPBItemLastPrice v where v.ItemID=d.ITEMID And pbdate <=" & getDateFormatInCond(dtpDay.Value) & "),0))*sum(d.SINVQty)  as TotSales, (isnull((Select top 1 PBRate from vwPBItemLastPrice v where v.ItemID=d.ITEMID And pbdate <=" & getDateFormatInCond(dtpDay.Value) & " order by PBdate Desc),0))*sum(d.SINVQty) as TotCashAmt,"
                    strSQL &= " 0 as TotCardAmt, 0 as OtherAmt  "
                    If gStrDBName = "VLQMSMaduloka" Then
                        strSQL &= ",0 as CreditAmt "
                    End If
                    strSQL &= " from TrnHdrSINV h,TrnDtlSinv d "
                    strSQL &= " Where d.SINVID=h.SINVID And SINVDate = " & getDateFormatInCond(dtpDay.Value) & " And Cancelled = 'N' AND IsDelete='N'"
                    If Not blnIncludeNonLiq Then
                        strSQL &= " and d.ITEMID in (Select distinct ITEMID from MstItem I where d.ItemID=i.ItemID and i.ITEMGroupID in (select ITEMGROUPID from mstitemgroup where LiqItem='Y')) "
                    End If
                    strSQL &= " group by h.SINVID,d.ITEMID)a group by TotBills" 'group by TotBills
                Else
                'strSQL = "Select Count(SINVNo) as TotBills, Sum(INVValue) as TotSales, sum(CashAmt) as TotCashAmt,"
                'strSQL &= " Sum(CardAmt) as TotCardAmt, Sum(OthPayAmt) as OtherAmt  "
                'If gStrDBName = "VLQMSMaduloka" Then
                '    strSQL &= ",Sum(CRED) as CreditAmt "
                'End If
                'strSQL &= " from TrnHdrSINV "
                'strSQL &= " Where SINVDate = " & getDateFormatInCond(dtpDay.Value) & " And Cancelled = 'N' AND IsDelete='N'"
                'If Not blnIncludeNonLiq Then
                '    strSQL &= " and SINVID in (Select distinct SINVID from TrnDtlSINV d, MstItem I where d.ItemID=i.ItemID and i.ITEMGroupID in (select ITEMGROUPID from mstitemgroup where LiqItem='Y')) "
                'End If
                strSQL = "Select Count(distinct SINVNo) as TotBills, Sum(ITEMAmt) as TotSales, sum(d.ITEMAmt) as TotCashAmt,"
                strSQL &= " Sum(CardAmt) as TotCardAmt, Sum(OthPayAmt) as OtherAmt  "
                If gStrDBName = "VLQMSMaduloka" Then
                    strSQL &= ",Sum(CRED) as CreditAmt "
                End If
                strSQL &= " from TrnHdrSINV h,TrnDtlSinv d "
                strSQL &= " Where h.SINVID=d.SINVID and SINVDate = " & getDateFormatInCond(dtpDay.Value) & " And Cancelled = 'N' AND IsDelete='N'"
                If Not blnIncludeNonLiq Then
                    strSQL &= " and d.ITEMID in (Select distinct ITEMID from MstItem I where i.ITEMGroupID in (select ITEMGROUPID from mstitemgroup where LiqItem='Y')) "
                End If
            End If

            rs = clsSVT.getDataReaderFromSQL(strSQL)

            Dim dblLiqAmt As Double = 0
            Dim dblProfitAmt As Double = 0
            Dim dblTotpurchase As Double = 0
            Dim dblTotSales As Double = 0
            Dim dbltotAmt As Double = 0
            Dim dbltotSCAmt As Double = 0

            If rs.HasRows Then
                Do While rs.Read
                    Call addRow("Total Bills", GetVal(rs!TotBills, 0))

                    'Added by Surabhi to get the Total Slaes amount from invoice detail table insted of header
                    If optPurRate.Checked Then
                        strSQL = "Select sum(Val) from (select d.sinvqty*isnull((Select top 1 PBRate from vwPBItemLastPrice v where i.ItemID = v.itemid  And pbdate <= " & getDateFormatInCond(dtpDay.Value) & " order by PBdate Desc),0) as Val From mstitem i, trndtlsinv d, trnhdrsinv h"
                        strSQL &= " Where h.sinvid = d.sinvid and d.itemid = i.itemid  "
                        If Not blnIncludeNonLiq Then
                            strSQL &= " and i.ITEMGroupID in (select ITEMGROUPID from mstitemgroup where LiqItem='Y') "
                        End If

                        strSQL &= " and SINVDate = " & getDateFormatInCond(dtpDay.Value) & " and IsDelete='N' and Cancelled = 'N')a  "
                    Else
                        'strSQL = "Select sum(d.ITEMAMT) From mstitem i, trndtlsinv d, trnhdrsinv h"
                        strSQL = "Select sum(d.ITEMAMT) From mstitem i, trndtlsinv d, trnhdrsinv h"
                        strSQL &= " Where h.sinvid = d.sinvid and d.itemid = i.itemid  "
                        If Not blnIncludeNonLiq Then
                            strSQL &= " and i.ITEMGroupID in (select ITEMGROUPID from mstitemgroup where LiqItem='Y') "
                        End If
                        strSQL &= " and SINVDate = " & getDateFormatInCond(dtpDay.Value) & " and IsDelete='N' and Cancelled = 'N'  "
                    End If

                    dbltotAmt = clsSVT.getDataDoubleValue(strSQL, 0, False)
                    'Call addRow("Total Sales Amount (Incl.GST)", GetVal(rs!TotSales, 0))


                    If Not (UCase(gStrDBName) = "VLQMSTSRR" Or UCase(gStrDBName) = "VLQMSTSYELANKA" Or UCase(gStrDBName) = "VLQMSCLH") Then

                        dbltotAmt = Math.Round(dbltotAmt)
                    End If

                    Call addRow("Total Sales Amount (Incl.GST)", dbltotAmt)
                    If optPurRate.Checked Then
                        strSQL = "Select  sum(val) from (select d.sinvqty*isnull((Select top 1 PBRate from vwPBItemLastPrice v where i.ItemID = v.itemid  And pbdate <= " & getDateFormatInCond(dtpDay.Value) & " order by PBdate Desc),0) as Val From mstitem i, trndtlsinv d, trnhdrsinv h"
                        strSQL &= " Where h.sinvid = d.sinvid and d.itemid = i.itemid And i.LiqType <> 'Others' "
                        If Not blnIncludeNonLiq Then
                            strSQL &= " and i.ITEMGroupID in (select ITEMGROUPID from mstitemgroup where LiqItem='Y') "
                        End If
                        strSQL &= " and SINVDate = " & getDateFormatInCond(dtpDay.Value) & " and IsDelete='N' and Cancelled = 'N')a "
                    Else
                        strSQL = "Select sum(d.ITEMAMT) From mstitem i, trndtlsinv d, trnhdrsinv h"
                        strSQL &= " Where h.sinvid = d.sinvid and d.itemid = i.itemid And i.LiqType <> 'Others' "
                        If Not blnIncludeNonLiq Then
                            strSQL &= " and i.ITEMGroupID in (select ITEMGROUPID from mstitemgroup where LiqItem='Y') "
                        End If
                        strSQL &= " and SINVDate = " & getDateFormatInCond(dtpDay.Value) & " and IsDelete='N' and Cancelled = 'N'  "
                    End If

                    dblLiqAmt = clsSVT.getDataDoubleValue(strSQL, 0, False)
                    Call addRow("Sales Amount - Liquor (BasicAmt)", dblLiqAmt)
                    If optPurRate.Checked Then
                        strSQL = "Select  sum(val) from (select d.sinvqty*isnull((Select top 1 PBRate from vwPBItemLastPrice v where i.ItemID = v.itemid  And pbdate <= " & getDateFormatInCond(dtpDay.Value) & " order by PBdate Desc),0) as Val From mstitem i, trndtlsinv d, trnhdrsinv h"
                        strSQL &= " Where h.sinvid = d.sinvid and d.itemid = i.itemid And i.LiqType = 'Others' "
                        If Not blnIncludeNonLiq Then
                            strSQL &= " and i.ITEMGroupID in (select ITEMGROUPID from mstitemgroup where LiqItem='Y') "
                        End If
                        strSQL &= " and SINVDate = " & getDateFormatInCond(dtpDay.Value) & " and IsDelete='N' and Cancelled = 'N')a "
                    Else
                        strSQL = "Select sum(d.ITEMAMT) From mstitem i, trndtlsinv d, trnhdrsinv h"
                        strSQL &= " Where h.sinvid = d.sinvid and d.itemid = i.itemid And i.LiqType = 'Others' "
                        If Not blnIncludeNonLiq Then
                            strSQL &= " and i.ITEMGroupID in (select ITEMGROUPID from mstitemgroup where LiqItem='Y') "
                        End If
                        strSQL &= " and SINVDate = " & getDateFormatInCond(dtpDay.Value) & " and IsDelete='N' and Cancelled = 'N'  "
                    End If

                    dblLiqAmt = clsSVT.getDataDoubleValue(strSQL, 0, False)
                    Call addRow("Sales Amount - NonLiquor (BasicAmt)", dblLiqAmt)

                    If UCase(gStrDBName) = "VLQMSDROPSJP" Then
                        'not done
                        strSQL = "Select  Sum(INVVALUE) as Amount From TrnHdrSINV h, MstSC s "
                        strSQL &= " Where h.SCID=s.SCID and IsDelete='N' and Cancelled='N'"
                        If Not blnIncludeNonLiq Then
                            strSQL &= " and h.SINVID in (Select distinct SINVID from TrnDtlSINV d, MstItem I where d.ItemID=i.ItemID and i.ITEMGroupID in (select ITEMGROUPID from mstitemgroup where LiqItem='Y')) "
                        End If
                        strSQL &= " and SINVDate =" & getDateFormatInCond(dtpDay.Value) & " "

                        dbltotSCAmt = clsSVT.getDataDoubleValue(strSQL, 0, False)
                        addRow("Total Counter Wise Sales(RoundOff)", dbltotSCAmt)

                    End If

                    If UCase(gStrDBName) = "VLQMSNDROPS" Then
                        strSQL = "select sum(i.PURRATE * sinvqty)  as Purchaserate  from trnhdrsinv h ,trndtlsinv d, mstitem i"
                        strSQL &= " where h.sinvid=d.sinvid and d.itemid=i.ITEMID"
                        strSQL &= " and SINVDate = " & getDateFormatInCond(dtpDay.Value) & " and IsDelete='N' and Cancelled = 'N'  "
                        If Not blnIncludeNonLiq Then
                            strSQL &= " and i.ITEMGroupID in (select ITEMGROUPID from mstitemgroup where LiqItem='Y') "
                        End If
                        dblTotpurchase = clsSVT.getDataDoubleValue(strSQL, 0, False)

                        If optPurRate.Checked Then
                            strSQL = "select sum(i.PURRATE * sinvqty)  as SaleAmt from trnhdrsinv h ,trndtlsinv d, mstitem i"
                            strSQL &= " where h.sinvid=d.sinvid and d.itemid=i.ITEMID"
                        Else
                            strSQL = "select sum(sinvamt) as SaleAmt from trnhdrsinv h ,trndtlsinv d, mstitem i"
                            strSQL &= " where h.sinvid=d.sinvid and d.itemid=i.ITEMID"
                        End If
                        strSQL &= " and SINVDate = " & getDateFormatInCond(dtpDay.Value) & " and IsDelete='N' and Cancelled = 'N'  "
                        If Not blnIncludeNonLiq Then
                            strSQL &= " and i.ITEMGroupID in (select ITEMGROUPID from mstitemgroup where LiqItem='Y') "
                        End If
                        dblTotSales = clsSVT.getDataDoubleValue(strSQL, 0, False)

                        dblProfitAmt = dblTotSales - dblTotpurchase
                        Call addRow("Sales Profit Of the Day", dblProfitAmt)
                    End If


                    'strSQL &= " and d.ItemID in (Select distinct ItemID from MstItem I, ITEMGroupID g where i.ITEMGROUPID=g.ItemGroupID and g.LiqItem='Y') "
                    Call addRow("Opening Stock Qty", clsSVT.getDataDoubleValue("Select Sum(Qty * StkMultiplier) as OpQty from UtlStockList Where DocDate < '" & getDateFormatInInsert(dtpDay.Value) & "' and ItemID in (Select distinct ItemID from MstItem I, MstITEMGroup g where i.ITEMGROUPID=g.ItemGroupID" & IIf(blnIncludeNonLiq = False, " and g.LiqItem='Y'", "") & ") ", 0, False))
                    If optPurRate.Checked Then
                        ' Call addRow("Opening Stock Value", clsSVT.getDataDoubleValue("Select  Sum(Cast(Round(purrate * Qty * StkMultiplier,2) as Numeric(18,2)))  as OpBal from UtlStockList u, MstItem i Where u.ItemID=i.ItemID and DocDate <'" & getDateFormatInInsert(dtpDay.Value) & "' and u.ItemID in (Select distinct ItemID from MstItem I, MstITEMGroup g where i.ITEMGROUPID=g.ItemGroupID" & IIf(blnIncludeNonLiq = False, " and g.LiqItem='Y'", "") & ") ", 0, False))
                        Call addRow("Opening Stock Value", clsSVT.getDataDoubleValue("select sum(opbal) from (Select Cast(Round((isnull((Select top 1 PBRate from vwPBItemLastPrice v where u.ItemID = v.itemid  And pbdate <= " & getDateFormatInCond(dtpDay.Value) & " order by PBdate Desc),0)) * Qty * StkMultiplier,2) as Numeric(18,2))  as OpBal from UtlStockList u, MstItem i Where u.ItemID=i.ItemID and DocDate <'" & getDateFormatInInsert(dtpDay.Value) & "' and u.ItemID in (Select distinct ItemID from MstItem I, MstITEMGroup g where i.ITEMGROUPID=g.ItemGroupID" & IIf(blnIncludeNonLiq = False, " and g.LiqItem='Y'", "") & "))a ", 0, False))
                    Else
                        Call addRow("Opening Stock Value", clsSVT.getDataDoubleValue("Select  Sum(Cast(Round(MRP * Qty * StkMultiplier,2) as Numeric(18,2)))  as OpBal from UtlStockList u, MstItem i Where u.ItemID=i.ItemID and DocDate <'" & getDateFormatInInsert(dtpDay.Value) & "' and u.ItemID in (Select distinct ItemID from MstItem I, MstITEMGroup g where i.ITEMGROUPID=g.ItemGroupID" & IIf(blnIncludeNonLiq = False, " and g.LiqItem='Y'", "") & ") ", 0, False))

                    End If

                    Call addRow("Closing Stock [Stock UOM]", clsSVT.getDataDoubleValue("Select Sum(Qty * StkMultiplier) as ClBal from UtlStockList Where DocDate <= '" & getDateFormatInInsert(dtpDay.Value) & "' and ItemID in (Select distinct ItemID from MstItem I, MstITEMGroup g where i.ITEMGROUPID=g.ItemGroupID" & IIf(blnIncludeNonLiq = False, " and g.LiqItem='Y'", "") & ") ", 0, False))
                    If optPurRate.Checked Then
                        Call addRow("Closing Stock Value", clsSVT.getDataDoubleValue("Select Sum(CLBAL) from (select isnull((Select top 1 PBRate from vwPBItemLastPrice v where i.ItemID = v.itemid  And pbdate <= " & getDateFormatInCond(dtpDay.Value) & " order by PBdate Desc),0) * Qty * StkMultiplier  as ClBal from UtlStockList u, MstItem i Where u.ItemID=i.ItemID and DocDate <='" & getDateFormatInInsert(dtpDay.Value) & "' and i.ItemGroupID in (Select distinct ItemGroupID from MstITEMGroup g where Inactive='N' " & IIf(blnIncludeNonLiq = False, " and g.LiqItem='Y'", "") & "))a ", 0, False))
                    Else
                        Call addRow("Closing Stock Value", clsSVT.getDataDoubleValue("Select Sum(MRP * Qty * StkMultiplier)  as ClBal from UtlStockList u, MstItem i Where u.ItemID=i.ItemID and DocDate <='" & getDateFormatInInsert(dtpDay.Value) & "' and i.ItemGroupID in (Select distinct ItemGroupID from MstITEMGroup g where Inactive='N' " & IIf(blnIncludeNonLiq = False, " and g.LiqItem='Y'", "") & ") ", 0, False))
                    End If

                    If optPurRate.Checked Then
                        'Call addRow("Cash Amount", GetVal(rs!TotCashAmt, 0))
                    Else
                        Call addRow("Cash Amount", GetVal(rs!TotCashAmt, 0))
                    End If

                    Call addRow("Card Amount", GetVal(rs!TotCardAmt, 0))
                    Call addRow("Other Pay Amount", GetVal(rs!OtherAmt, 0))

                    If gStrDBName = "VLQMSMaduloka" Then
                        Call addRow("Credit Amount", GetVal(rs!CreditAmt, 0))
                    End If
                Loop
                rs.Close()
            End If

            Dim dblWriteOffAmt As Double = 0
            strSQL = "Select Sum(WriteOffAmt) as WriteOffAmt"
            strSQL &= " From TrnHdrSINV"
            strSQL &= " Where SINVDate = " & getDateFormatInCond(dtpDay.Value) & " and IsDelete='N' and Cancelled = 'N'  "
            If Not blnIncludeNonLiq Then
                strSQL &= " and SINVID in (Select distinct SINVID from TrnDtlSINV d, MstItem I where d.ItemID=i.ItemID and i.ITEMGroupID in (select ITEMGROUPID from mstitemgroup where LiqItem='Y')) "
            End If
            dblWriteOffAmt = clsSVT.getDataDoubleValue(strSQL, 0, False)
            If dblWriteOffAmt > 0 Then Call addRow("Write Off Amount", dblWriteOffAmt)

            'ADded by surabhi  Sales rate in bracket 
            Call addRow("Total Purchase Amount (Sales rate)", "")
            If optPurRate.Checked Then
                strSQL = "Select Sum(d.PBRate * d.PBQty) from TrnHdrPB h, TrnDtlPB d"
            Else
                strSQL = "Select Sum(d.MRP * d.PBQty) from TrnHdrPB h, TrnDtlPB d"
            End If
            strSQL &= " where h.PBID=d.PBID And Cancelled='N'"
            strSQL &= " And PBDate =" & getDateFormatInCond(dtpDay.Value)
            If Not blnIncludeNonLiq Then
                strSQL &= " and d.ItemID in (Select distinct ItemID from MstItem I, MstITEMGroup g where i.ITEMGROUPID=g.ItemGroupID and g.LiqItem='Y') "
            End If
            .SetData(.Rows.Count - 1, 1, clsSVT.getDataDoubleValue(strSQL, 0, False))

            'ADded by surabhi  for purchase Basic amount 
            'Call addRow("Purchase Amount (Basic)", "")
            'strSQL = "Select Sum(TOTAMT) As PurchaseBasic from TrnHdrPB h, TrnDtlPB d, MstItem i where h.PBID=d.PBID And d.ItemID=i.ItemID And Cancelled='N'"
            'strSQL &= " And PBDate =" & getDateFormatInCond(dtpDay.Value)
            '.SetData(.Rows.Count - 1, 1, clsSVT.getDataDoubleValue(strSQL, 0, False))
            Call addRow("Purchase Amount (Basic)", "")
            strSQL = "Select Sum(TOTAMT) from TrnHdrPB where Cancelled='N'"
            strSQL &= " And PBDate =" & getDateFormatInCond(dtpDay.Value)
            If Not blnIncludeNonLiq Then
                strSQL &= " and PBID in (Select distinct PBID from TrnDtlPB d, MstItem I where d.ItemID=i.ItemID and i.ITEMGroupID in (select ITEMGROUPID from mstitemgroup where LiqItem='Y')) "
            End If
            .SetData(.Rows.Count - 1, 1, clsSVT.getDataDoubleValue(strSQL, 0, False))


            'ADded by surabhi  for purchase Total Amount after including GST 
            Call addRow("Purchase Amount (Incl.GST)", "")
            'strSQL = "Select Sum(GRNDTOT) As PurchaseBasic from TrnHdrPB h, TrnDtlPB d, MstItem i where h.PBID=d.PBID And d.ItemID=i.ItemID And Cancelled='N'"
            'strSQL &= " And PBDate =" & getDateFormatInCond(dtpDay.Value)
            strSQL = "Select Sum(GRNDTOT) from TrnHdrPB where Cancelled='N'"
            strSQL &= " And PBDate =" & getDateFormatInCond(dtpDay.Value)
            If Not blnIncludeNonLiq Then
                strSQL &= " and PBID in (Select distinct PBID from TrnDtlPB d, MstItem I where d.ItemID=i.ItemID and i.ITEMGroupID in (select ITEMGROUPID from mstitemgroup where LiqItem='Y')) "
            End If
            .SetData(.Rows.Count - 1, 1, clsSVT.getDataDoubleValue(strSQL, 0, False))


            If fnTablePresent("TrnHdrSR") Then
                Call addRow("Total Sales Return Amount", "")
                strSQL = "Select Sum(TotAmt) as SalesReturn from TrnHdrSR where Cancelled='N'"
                strSQL &= " And SRDate =" & getDateFormatInCond(dtpDay.Value)
                If Not blnIncludeNonLiq Then
                    strSQL &= " and SRID in (Select distinct SRID from TrnDtlSR d, MstItem I where d.ItemID=i.ItemID and i.ITEMGroupID in (select ITEMGROUPID from mstitemgroup where LiqItem='Y')) "
                End If
                .SetData(.Rows.Count - 1, 1, clsSVT.getDataDoubleValue(strSQL, 0, False))
            End If

            addRow("GST Summary", "", "", True, False)
            strSQL = "Select CgstPer, Sum(CGSTAmt) as CgstAmt, "
            strSQL &= " sgstPer, Sum(sGSTAmt) as sgstAmt "
            strSQL &= " From TrnHdrSINV h, trndtlsinv d"
            strSQL &= " Where SINVDate = " & getDateFormatInCond(dtpDay.Value) & " and IsDelete='N' and Cancelled = 'N'  "
            strSQL &= " and h.sInvId = d.SinvID"
            If Not blnIncludeNonLiq Then
                strSQL &= " and d.ItemID in (Select distinct ITEMID from MstItem i, MstItemGroup g where i.ITEMGroupID=g.ItemGroupID and g.LiqItem='Y') "
            End If
            strSQL &= " Group by CgstPer, SGstPer"
            strSQL &= " having sum(cgstamt) > 0"
            strSQL &= " order By 1"
            rs = clsSVT.getDataReaderFromSQL(strSQL)
            blnFound = False
            If rs.HasRows Then
                Do While rs.Read
                    blnFound = True
                    Call addRow("CGST @" & rs!CgstPer & "% ", GetVal(rs!CGSTAmt, 0))
                    Call addRow("SGST @" & rs!SgstPer & "% ", GetVal(rs!SGSTAmt, 0))
                Loop
                rs.Close()
            End If
            If Not blnFound Then vsfView.RemoveItem(vsfView.Rows.Count - 1)

            addRow("Expense Summary", "", "", True, False)
            strSQL = "Select * from vwexpsmry where "
            strSQL &= "ExpDate =" & getDateFormatInCond(dtpDay.Value)
            rs = clsSVT.getDataReaderFromSQL(strSQL)
            blnFound = False
            If rs.HasRows Then
                Do While rs.Read
                    blnFound = True
                    Call addRow("Cash OP Balance", GetVal(rs!OPAmt, 0))
                    Call addRow("Used Amount (Expense)", GetVal(rs!ExpAmt, 0))
                    Call addRow("Cash Balance", GetVal(rs!BalAmt, 0))
                Loop
                rs.Close()
            End If
            If Not blnFound Then vsfView.RemoveItem(vsfView.Rows.Count - 1)

            addRow("Mode of Payments Summary", "", "", True, False)
            strSQL = "select Particulars, sum(amount) as Amt"
            strSQL &= " From trnmultiamt m, trnhdrsinv h"
            strSQL &= " Where DocID = 1216"
            strSQL &= " And m.trnid = h.SINVID"
            strSQL &= " And h.SINVDate = " & getDateFormatInCond(dtpDay.Value)
            If Not blnIncludeNonLiq Then
                strSQL &= " and h.SINVID in (Select distinct SINVID from TrnDtlSINV d, MstItem I where d.ItemID=i.ItemID and i.ITEMGroupID in (select ITEMGROUPID from mstitemgroup where LiqItem='Y')) "
            End If
            strSQL &= " group by Particulars order by 1"
            rs = clsSVT.getDataReaderFromSQL(strSQL)
            blnFound = False
            If rs.HasRows Then
                Do While rs.Read
                    Call addRow(GetVal(rs!Particulars), GetVal(rs!Amt, 0))
                    blnFound = True
                Loop
                rs.Close()
            End If
            If Not blnFound Then vsfView.RemoveItem(vsfView.Rows.Count - 1)

            'Call addRow("Total Purchase Amount", "")
            'strSQL = "Select Sum(i.MRP * d.PBQty) As Purchase from TrnHdrPB h, TrnDtlPB d, MstItem i where h.PBID=d.PBID And d.ItemID=i.ItemID And Cancelled='N'"
            'strSQL &= " And PBDate =" & getDateFormatInCond(dtpDay.Value)
            '.SetData(.Rows.Count - 1, 1, clsSVT.getDataDoubleValue(strSQL, 0, False))

            'If fnTablePresent("TrnHdrSR") Then
            '    Call addRow("Total Sales Return Amount", "")
            '    strSQL = "Select Sum(TotAmt) as SalesReturn from TrnHdrSR where Cancelled='N'"
            '    strSQL &= " And SRDate =" & getDateFormatInCond(dtpDay.Value)
            '    .SetData(.Rows.Count - 1, 1, clsSVT.getDataDoubleValue(strSQL, 0, False))
            'End If

            If Not optPurRate.Checked Then


                addRow("Counter Wise Sales", "", "", True, False)
                addRow("Counter #", "No of Bills", "Value", False, True)

                strSQL = "Select SCCODE, Count(SCCode) as NoOfBills, Sum(INVVALUE) as Amount From TrnHdrSINV h, MstSC s "
                strSQL &= " Where h.SCID=s.SCID and IsDelete='N' and Cancelled='N'"
                If Not blnIncludeNonLiq Then
                    strSQL &= " and h.SINVID in (Select distinct SINVID from TrnDtlSINV d, MstItem I where d.ItemID=i.ItemID and i.ITEMGroupID in (select ITEMGROUPID from mstitemgroup where LiqItem='Y')) "
                End If
                strSQL &= " and SINVDate =" & getDateFormatInCond(dtpDay.Value) & " Group by SCCode"
                rs = clsSVT.getDataReaderFromSQL(strSQL)
                If rs.HasRows Then
                    Do While rs.Read
                        Call addRow(GetVal(rs!SCCODE, ""), GetVal(rs!NoOfBills, 0), GetVal(rs!Amount, 0))
                    Loop
                    rs.Close()
                End If

                addRow("Cancelled Invoices", "", "", True, False)

                strSQL = "Select SCCODE, Count(SCCode) as NoOfBills, Sum(INVVALUE) as Amount From TrnHdrSINV h, MstSC s "
                strSQL &= " Where h.SCID=s.SCID and Cancelled ='Y'"
                If Not blnIncludeNonLiq Then
                    strSQL &= " and SINVID in (Select distinct SINVID from TrnDtlSINV d, MstItem I where d.ItemID=i.ItemID and i.ITEMGroupID in (select ITEMGROUPID from mstitemgroup where LiqItem='Y')) "
                End If
                strSQL &= " and SINVDate =" & getDateFormatInCond(dtpDay.Value) & " Group by SCCode"
                rs = clsSVT.getDataReaderFromSQL(strSQL)
                If rs.HasRows Then
                    Do While rs.Read
                        Call addRow(GetVal(rs!SCCODE, ""), GetVal(rs!NoOfBills, 0), GetVal(rs!Amount, 0))
                    Loop
                    rs.Close()
                Else
                    .Rows.Count = .Rows.Count - 1
                End If

                addRow("Sales Person Wise Sales", "", "", True, False)
                addRow("Name", "No of Bills", "Value", False, True)

                strSQL = "Select SPCODE, Count(SPCode) as NoOfBills, Sum(INVVALUE) as Amount From TrnHdrSINV h, MstSP s "
                strSQL &= " where h.SPID=s.SPID and IsDelete='N' and Cancelled='N' "
                If Not blnIncludeNonLiq Then
                    strSQL &= " and SINVID in (Select distinct SINVID from TrnDtlSINV d, MstItem I where d.ItemID=i.ItemID and i.ITEMGroupID in (select ITEMGROUPID from mstitemgroup where LiqItem='Y')) "
                End If
                strSQL &= " and SINVDate =" & getDateFormatInCond(dtpDay.Value) & " Group by SPCode"
                rs = clsSVT.getDataReaderFromSQL(strSQL)
                If rs.HasRows Then
                    Do While rs.Read
                        Call addRow(GetVal(rs!SPCODE, ""), GetVal(rs!NoOfBills, 0), GetVal(rs!Amount, 0))
                    Loop
                    rs.Close()
                End If
            End If

            addRow("Group Wise Sales", "", "", True, False)
            addRow("Group Name", "Qty", "Amount", False, True)

            If optPurRate.Checked Then
                strSQL = "select GroupOrderBy,ItemGroupCode, Sum(convert(Int,Qty)) as Qty, Sum(Qty*Rate) as Amount from (Select g.GroupOrderBy,g.ItemGroupCode, (convert(Int,d.SINVQty)) as Qty,"
                strSQL &= " (isnull((Select top 1 PBRate from vwPBItemLastPrice v where v.ItemID=d.ITEMID And pbdate <=" & getDateFormatInCond(dtpDay.Value) & " order by PBdate Desc),0)) as Rate"
                strSQL &= " From TrnHdrSINV h, TrnDtlSINV d, MstItem i, MstItemGroup g "
                strSQL &= " Where h.SINVID=d.SINVID and d.ITEMID=i.ITEMID and i.ITEMGroupID=g.ITEMGROUPID "
                strSQL &= " and IsDelete='N' and Cancelled='N' and SINVDate = " & getDateFormatInCond(dtpDay.Value) & ""
                If Not blnIncludeNonLiq Then
                    strSQL &= " and g.LiqItem='Y' "
                End If
                strSQL &= " )a group by GroupOrderBy,ItemGroupCode"
                'strSQL &= " Group by g.GroupOrderBy,g.ItemGroupCode"
                strSQL &= " Order by GroupOrderBy"

            Else
                strSQL = "Select g.GroupOrderBy,g.ItemGroupCode, Sum(convert(Int,d.SINVQty)) as Qty, Sum(Round(d.ItemAmt,2)) as Amount "
                strSQL &= " From TrnHdrSINV h, TrnDtlSINV d, MstItem i, MstItemGroup g "
                strSQL &= " Where h.SINVID=d.SINVID and d.ITEMID=i.ITEMID and i.ITEMGroupID=g.ITEMGROUPID "
                strSQL &= " and IsDelete='N' and Cancelled='N' and SINVDate = " & getDateFormatInCond(dtpDay.Value) & ""
                If Not blnIncludeNonLiq Then
                    strSQL &= " and g.LiqItem='Y' "
                End If
                strSQL &= " Group by g.GroupOrderBy,g.ItemGroupCode"
                strSQL &= " Order by g.GroupOrderBy"
            End If

            rs = clsSVT.getDataReaderFromSQL(strSQL)
            If rs.HasRows Then
                Do While rs.Read
                    Call addRow(GetVal(rs!ItemGroupCode, ""), GetVal(rs!Qty, 0), GetVal(rs!Amount, 0))
                Loop
                rs.Close()
            End If
            addRow("Group Wise Purchase", "", "", True, False)
            addRow("Group Name", "Qty", "Amount", False, True)

            'isnull((Select top 1 PBRate from vwPBItemLastPrice v where i.ItemID = v.itemid  And pbdate <= " & getDateFormatInCond(dtpDay.Value) & " order by Slno),0)
            If optPurRate.Checked Then
                strSQL = "Select g.GroupOrderBy,g.ItemGroupCode, Sum(convert(Int,d.PBQty)) as Qty,"
                strSQL &= "Sum(Cast(Round(d.PBRate * d.PBQty,2) as Numeric(18,2))) as Amount"
                strSQL &= " From TrnHdrPB h, TrnDtlPB d, MstItem i, MstItemGroup g  "
                strSQL &= " Where h.PBID=d.PBID And d.ITEMID=i.ITEMID And i.ITEMGroupID=g.ITEMGROUPID "
                If Not blnIncludeNonLiq Then
                    strSQL &= " and g.LiqItem='Y' "
                End If
                strSQL &= "And Cancelled='N' and PBDate = " & getDateFormatInCond(dtpDay.Value) & " Group by g.GroupOrderBy,g.ItemGroupCode"
                strSQL &= " Order by g.GroupOrderBy"
            Else
                strSQL = "Select g.GroupOrderBy,g.ItemGroupCode, Sum(convert(Int,d.PBQty)) as Qty,"
                strSQL &= "Sum(Cast(Round(i.MRP * d.PBQty,2) as Numeric(18,2))) as Amount"
                strSQL &= " From TrnHdrPB h, TrnDtlPB d, MstItem i, MstItemGroup g  "
                strSQL &= " Where h.PBID=d.PBID And d.ITEMID=i.ITEMID And i.ITEMGroupID=g.ITEMGROUPID "
                If Not blnIncludeNonLiq Then
                    strSQL &= " and g.LiqItem='Y' "
                End If
                strSQL &= "And Cancelled='N' and PBDate = " & getDateFormatInCond(dtpDay.Value) & " Group by g.GroupOrderBy,g.ItemGroupCode"
                strSQL &= " Order by g.GroupOrderBy"
            End If

            rs = clsSVT.getDataReaderFromSQL(strSQL)
            If rs.HasRows Then
                Do While rs.Read
                    Call addRow(GetVal(rs!ItemGroupCode, ""), GetVal(rs!Qty, 0), GetVal(rs!Amount, 0))
                Loop
                rs.Close()
            End If

            addRow("Expense Ledger Wise", "", "", True, False)
            addRow("Ledger", "Amount", "", False, True)

            strSQL = "Select l.LedCode, Sum(d.Amt) as Amount From TrnHdrEXP h, TrnDtlExp d, MstLed l "
            strSQL &= " Where h.EXPID=d.EXPID and d.LEDID=l.LEDID  and Cancelled='N' "
            strSQL &= " and ExpDate = " & getDateFormatInCond(dtpDay.Value) & " Group by l.LedCode"
            rs = clsSVT.getDataReaderFromSQL(strSQL)
            If rs.HasRows Then
                Do While rs.Read
                    Call addRow(GetVal(rs!LedCode, ""), GetVal(rs!Amount, 0))
                Loop
                rs.Close()
            End If

            If fnTablePresent("TRNHDRRDM") Then
                Dim dblRdmAmt As Double
                dblRdmAmt = clsSVT.getDataDoubleValue("Select sum(CashAmt) from TrnHdrRdm where RdmDate = " & getDateFormatInCond(dtpDay.Value), 0, False)
                addRow("Redemption", "", "", True, False)
                addRow("Redeemed Amount", dblRdmAmt, "", False, False)
            End If
            .AutoSizeCols()
        End With
    End Sub

    Private Sub addRow(argCaption As String, argValue As String, Optional argValue2 As String = "", Optional blnHighlight As Boolean = False, Optional blnSubHeding As Boolean = False)
        With vsfView
            .Rows.Count = .Rows.Count + 1
            .SetData(.Rows.Count - 1, 0, argCaption)
            .SetData(.Rows.Count - 1, 1, argValue)
            .SetData(.Rows.Count - 1, 2, argValue2)

            If blnHighlight Then
                Dim csf1 As CellStyle = .Styles.Add("StyleClr")
                Dim myColor As Color = Color.Moccasin
                csf1.BackColor = myColor
                csf1.Font = New Font("Segoe UI", 11, FontStyle.Bold)
                .Rows(.Rows.Count - 1).Style = csf1
            End If

            If blnSubHeding Then
                Dim csf2 As CellStyle = .Styles.Add("StyleClr")
                Dim myColor2 As Color = Color.Gray
                csf2.BackColor = myColor2
            End If
        End With
    End Sub
    Private Sub btnPrint_Click(sender As Object, e As EventArgs) Handles btnView.Click
        InitScreen()
        showWaitCursor()
        FillData()
        showNormalCursor()
    End Sub

    Private Sub vsfView_BeforeEdit(sender As Object, e As RowColEventArgs) Handles vsfView.BeforeEdit
        e.Cancel = True
    End Sub

    Private Sub btnExportToXL_Click(sender As Object, e As EventArgs) Handles btnExportToXL.Click
        ExportToXL(vsfView,,,, True, Me.Text, "For the period " & getDateFormatInInsert(dtpDay.Value))
    End Sub

    Private Sub btnPrint_Click_1(sender As Object, e As EventArgs) Handles btnPrint.Click

        Call PrintData(True)

    End Sub

    Private Sub PrintData(Optional ByVal blnShowPDF As Boolean = True, Optional ByVal argFileName As String = "", Optional ByVal argBlnWA As Boolean = False, Optional ByVal argBlnEmail As Boolean = False)
        Dim strSQL As String = ""
        Dim strPdfFileName As String = ""

        Dim blnIncludeNonLiq As Boolean = False
        If chkNonLiqour.Checked = True Then
            blnIncludeNonLiq = True
        Else
            blnIncludeNonLiq = False

        End If

        strSQL = "Delete from RptUDR Where RptName='Daily Summary Report' and Username='" & gUserCode & "'"
        clsSVT.execSQLStatement(strSQL)

        'If UCase(gStrDBName) = "VLQMSCNR" Then
        strSQL = "Insert Into RptUDR (RptName, UserName, Fld1, Fld2, Fld3, Fld4,FLD5)"
        If optPurRate.Checked Then
            strSQL &= " Select 'Daily Summary Report', '" & gUserCode & "',ITEMGROUPCODE,Sum(SINVQty) as Qty,Sum(SINVQty*Rate) as Amount,'Sales',GroupOrderBy from (select ITEMGROUPCODE, d.SINVQty,"
            strSQL &= " (isnull((Select top 1 PBRate from vwPBItemLastPrice v where v.ItemID=d.ITEMID And pbdate <=" & getDateFormatInCond(dtpDay.Value) & " order by PBdate Desc),0)) as Rate,GroupOrderBy"
            strSQL &= " from TrnHdrSINV h, TrnDtlSINV d, MstItem i, MstItemGroup ig "
            strSQL &= " Where h.SINVID=d.SINVID and h.Cancelled='N' and isdelete='N' "
            strSQL &= " and h.SINVDate = '" & getDateFormatInInsert(dtpDay.Value) & "' "
            strSQL &= " and d.ItemID=i.ItemID and i.ITEMGroupID=ig.ITEMGROUPID "
            If Not blnIncludeNonLiq Then
                strSQL &= " and ig.ITEMGroupID in (select ITEMGROUPID from mstitemgroup where LiqItem='Y') "
            End If
            strSQL &= " ) a Group by ItemGroupCode,GroupOrderBy "
            strSQL &= " Order by GroupOrderBy"
        Else
            strSQL &= " Select 'Daily Summary Report', '" & gUserCode & "', ITEMGROUPCODE, Sum(d.SINVQty) as Qty, Sum(d.ItemAmt) as Amount, 'Sales',GroupOrderBy "
            strSQL &= " from TrnHdrSINV h, TrnDtlSINV d, MstItem i, MstItemGroup ig "
            strSQL &= " Where h.SINVID=d.SINVID and h.Cancelled='N' and isdelete='N' "
            strSQL &= " and h.SINVDate = '" & getDateFormatInInsert(dtpDay.Value) & "' "
            strSQL &= " and d.ItemID=i.ItemID and i.ITEMGroupID=ig.ITEMGROUPID "
            If Not blnIncludeNonLiq Then
                strSQL &= " and ig.ITEMGroupID in (select ITEMGROUPID from mstitemgroup where LiqItem='Y') "
            End If
            strSQL &= " Group by ItemGroupCode,GroupOrderBy "
            strSQL &= " Order by 1,GroupOrderBy"
        End If
        clsSVT.execSQLStatement(strSQL)

        strSQL = "Insert Into RptUDR (RptName, UserName, Fld1, Fld2, Fld3, Fld4)"
        strSQL &= "Select 'Daily Summary Report', '" & gUserCode & "',g.ItemGroupCode, Sum(convert(Int,d.PBQty)) as Qty, Sum(i.MRP * d.PBQty) as Amount, 'Purchase'"
        strSQL &= " From TrnHdrPB h, TrnDtlPB d, MstItem i, MstItemGroup g  "
        strSQL &= " Where h.PBID=d.PBID and d.ITEMID=i.ITEMID and i.ITEMGroupID=g.ITEMGROUPID "
        If Not blnIncludeNonLiq Then
            strSQL &= " and i.ITEMGroupID in (select ITEMGROUPID from mstitemgroup where LiqItem='Y') "
        End If
        strSQL &= "and Cancelled='N'  and  PBDate = " & getDateFormatInCond(dtpDay.Value) & " Group by g.ItemGroupCode"
        clsSVT.execSQLStatement(strSQL)
        'Else
        '    strSQL = "Insert Into RptUDR (RptName, UserName, Fld1, Fld2, Fld3)"
        '    strSQL &= " Select 'Daily Summary Report', '" & gUserCode & "', ITEMGROUPCODE, Sum(d.SINVQty) as Qty, Sum(d.ItemAmt) as Amount "
        '    strSQL &= " from TrnHdrSINV h, TrnDtlSINV d, MstItem i, MstItemGroup ig "
        '    strSQL &= " Where h.SINVID=d.SINVID and h.Cancelled='N' "
        '    strSQL &= " and h.SINVDate = '" & getDateFormatInInsert(dtpDay.Value) & "' "
        '    strSQL &= " and d.ItemID=i.ItemID and i.ITEMGroupID=ig.ITEMGROUPID "
        '    strSQL &= " Group by ItemGroupCode "
        '    strSQL &= " Order by 1"
        '    clsSVT.execSQLStatement(strSQL)
        'End If

        Dim intNoOfInv As Int32, dblCashSales As Double, dblCardSales As Double, dblTotInvValue As Double, dblCgstAmt As Double, dblSgstAmt As Double, dblCessAmt As Double, dblOPQty As Double, dblOPVal As Double, dblCLQty As Double, dblCLVal As Double, dblUPIAmt As Double

        Dim dblTotSalesReturn As Double, dblTotExpense As Double, dblNetAmount As Double, dblTotpurchase As Double, dblTotSales As Double, dblProfitAmt As Double

        '   intNoOfInv = clsSVT.getDataLongValue("Select Count(*) From TrnHdrSINV Where Cancelled='N' and IsDelete='N' and SINVDate='" & getDateFormatInInsert(dtpDay.Value) & "' ", 0, False)
        strSQL = " Select Count(*) From TrnHdrSINV h Where"
        strSQL &= "  Cancelled='N' and IsDelete='N' and SINVDate='" & getDateFormatInInsert(dtpDay.Value) & "' "
        If Not blnIncludeNonLiq Then
            strSQL &= " And h.SINVID In (Select distinct SINVID from TrnDtlSINV d, MstItem I where d.ItemID=i.ItemID And i.ITEMGroupID In (Select ITEMGROUPID from mstitemgroup where LiqItem='Y')) "
        End If
        intNoOfInv = Math.Round(clsSVT.getDataDoubleValue(strSQL, 0, False), 2)

        If optPurRate.Checked Then
            dblCashSales = 0
            dblCardSales = 0
        Else
            'strSQL = "Select Sum(CashAmt)  From TrnHdrSINV h"
            'strSQL &= " Where IsDelete='N' and Cancelled='N' and SINVDate='" & getDateFormatInInsert(dtpDay.Value) & "' "
            'If Not blnIncludeNonLiq Then
            '    strSQL &= " And h.SINVID In (Select distinct SINVID from TrnDtlSINV d, MstItem I where d.ItemID=i.ItemID And i.ITEMGroupID In (Select ITEMGROUPID from mstitemgroup where LiqItem='Y')) "
            'End If
            '' dblCashSales = clsSVT.getDataDoubleValue("Select Sum(CashAmt)  From TrnHdrSINV Where IsDelete='N' and Cancelled='N' and SINVDate='" & getDateFormatInInsert(dtpDay.Value) & "' ", 0, False)
            strSQL = "Select sum(d.ITEMAMT) From mstitem i, trndtlsinv d, trnhdrsinv h"
            strSQL &= " Where h.sinvid = d.sinvid and d.itemid = i.itemid  "
            If Not blnIncludeNonLiq Then
                strSQL &= " and i.ITEMGroupID in (select ITEMGROUPID from mstitemgroup where LiqItem='Y') "
            End If
            strSQL &= " and SINVDate = " & getDateFormatInCond(dtpDay.Value) & " and IsDelete='N' and Cancelled = 'N'  "

            dblCashSales = Math.Round(clsSVT.getDataDoubleValue(strSQL, 0, False), 2)

            strSQL = "Select Sum(CardAmt)  From TrnHdrSINV h Where IsDelete='N' and Cancelled='N' and "
            strSQL &= "  Cancelled='N' and IsDelete='N' and SINVDate='" & getDateFormatInInsert(dtpDay.Value) & "' "
            If Not blnIncludeNonLiq Then
                strSQL &= " And h.SINVID In (Select distinct SINVID from TrnDtlSINV d, MstItem I where d.ItemID=i.ItemID And i.ITEMGroupID In (Select ITEMGROUPID from mstitemgroup where LiqItem='Y')) "
            End If
            dblCardSales = Math.Round(clsSVT.getDataDoubleValue(strSQL, 0, False), 2)
        End If

        ' dblCardSales = clsSVT.getDataDoubleValue("Select Sum(CardAmt)  From TrnHdrSINV Where IsDelete='N' and Cancelled='N' and SINVDate='" & getDateFormatInInsert(dtpDay.Value) & "' ", 0, False)



        ' dblTotInvValue = clsSVT.getDataDoubleValue("Select Sum(INVValue)  From TrnHdrSINV Where IsDelete='N' and Cancelled='N' and SINVDate='" & getDateFormatInInsert(dtpDay.Value) & "' ", 0, False)

        If optPurRate.Checked Then
            'strSQL = "Select sum(Qty*Rate) from (Select d.SINVQty as Qty,(isnull((Select top 1 PBRate from vwPBItemLastPrice v where v.ItemID=d.ITEMID And pbdate <=" & getDateFormatInCond(dtpDay.Value) & " order by PBdate Desc),0)) as Rate From mstitem i, trndtlsinv d, trnhdrsinv h"
            'strSQL &= " Where h.sinvid = d.sinvid and d.itemid = i.itemid  "
            'If Not blnIncludeNonLiq Then
            '    strSQL &= " And i.ITEMID In (Select distinct ITEMID from MstItem i where i.ITEMGroupID In (Select ITEMGROUPID from mstitemgroup where LiqItem='Y')) "
            'End If
            'strSQL &= " and SINVDate = " & getDateFormatInCond(dtpDay.Value) & " and IsDelete='N' and Cancelled = 'N')a  "

            strSQL = "Select sum(Val) from (select d.sinvqty*isnull((Select top 1 PBRate from vwPBItemLastPrice v where i.ItemID = v.itemid  And pbdate <= " & getDateFormatInCond(dtpDay.Value) & " order by PBdate Desc),0) as Val From mstitem i, trndtlsinv d, trnhdrsinv h"
            strSQL &= " Where h.sinvid = d.sinvid and d.itemid = i.itemid  "
            If Not blnIncludeNonLiq Then
                strSQL &= " and i.ITEMGroupID in (select ITEMGROUPID from mstitemgroup where LiqItem='Y') "
            End If

            strSQL &= " and SINVDate = " & getDateFormatInCond(dtpDay.Value) & " and IsDelete='N' and Cancelled = 'N')a  "
        Else
            strSQL = "Select sum(d.ITEMAMT) From mstitem i, trndtlsinv d, trnhdrsinv h"
            strSQL &= " Where h.sinvid = d.sinvid and d.itemid = i.itemid  "
            If Not blnIncludeNonLiq Then
                strSQL &= " and i.ITEMGroupID in (select ITEMGROUPID from mstitemgroup where LiqItem='Y') "
            End If
            strSQL &= " and SINVDate = " & getDateFormatInCond(dtpDay.Value) & " and IsDelete='N' and Cancelled = 'N'  "
        End If

        dblTotInvValue = clsSVT.getDataDoubleValue(strSQL, 0, False)

        strSQL = "Select Sum(TOTCGSTAMT)  From TrnHdrSINV h Where IsDelete='N' and Cancelled='N' and"
        strSQL &= " SINVDate='" & getDateFormatInInsert(dtpDay.Value) & "' "
        If Not blnIncludeNonLiq Then
            strSQL &= " And h.SINVID In (Select distinct SINVID  from TrnDtlSINV d, MstItem I where d.ItemID=i.ItemID And i.ITEMGroupID In (Select ITEMGROUPID from mstitemgroup where LiqItem='Y')) "
        End If
        dblCgstAmt = clsSVT.getDataDoubleValue(strSQL, 0, False)

        ' dblCgstAmt = clsSVT.getDataDoubleValue("Select Sum(TOTCGSTAMT)  From TrnHdrSINV Where IsDelete='N' and Cancelled='N' and SINVDate='" & getDateFormatInInsert(dtpDay.Value) & "' ", 0, False)

        '  dblSgstAmt = clsSVT.getDataDoubleValue("Select Sum(TOTSGSTAMT)  From TrnHdrSINV Where IsDelete='N' and Cancelled='N' and SINVDate='" & getDateFormatInInsert(dtpDay.Value) & "' ", 0, False)

        strSQL = "Select Sum(TOTSGSTAMT)  From TrnHdrSINV h Where IsDelete='N' and Cancelled='N' and"
        strSQL &= " SINVDate='" & getDateFormatInInsert(dtpDay.Value) & "' "
        If Not blnIncludeNonLiq Then
            strSQL &= " And h.SINVID In (Select distinct SINVID from TrnDtlSINV d, MstItem I where d.ItemID=i.ItemID And i.ITEMGroupID In (Select ITEMGROUPID from mstitemgroup where LiqItem='Y')) "
        End If
        dblSgstAmt = clsSVT.getDataDoubleValue(strSQL, 0, False)

        If UCase(gStrDBName) = "VLQMSNDROPS" Then
            strSQL = "select sum(i.PURRATE * sinvqty)  as Purchaserate  from trnhdrsinv h ,trndtlsinv d, mstitem i"
            strSQL &= " where h.sinvid=d.sinvid and d.itemid=i.ITEMID"
            If Not blnIncludeNonLiq Then
                strSQL &= " And h.SINVID In (Select distinct SINVID from TrnDtlSINV d, MstItem I where d.ItemID=i.ItemID And i.ITEMGroupID In (Select ITEMGROUPID from mstitemgroup where LiqItem='Y')) "
            End If
            strSQL &= " and SINVDate = " & getDateFormatInCond(dtpDay.Value) & " and IsDelete='N' and Cancelled = 'N'  "
            dblTotpurchase = clsSVT.getDataDoubleValue(strSQL, 0, False)

            strSQL = "select sum(sinvamt) as SaleAmt from trnhdrsinv h ,trndtlsinv d, mstitem i"
            strSQL &= " where h.sinvid=d.sinvid and d.itemid=i.ITEMID"
            If Not blnIncludeNonLiq Then
                strSQL &= " and i.ITEMGroupID in (select ITEMGROUPID from mstitemgroup where LiqItem='Y') "
            End If
            strSQL &= " and SINVDate = " & getDateFormatInCond(dtpDay.Value) & " and IsDelete='N' and Cancelled = 'N'  "
            dblTotSales = clsSVT.getDataDoubleValue(strSQL, 0, False)

            dblProfitAmt = dblTotSales - dblTotpurchase
            Call addRow("Sales Profit Of the Day", dblProfitAmt)
        End If



        If fnFldPresent("TOTCESSAMT", "TrnHdrSINV") Then
            ' dblCessAmt = clsSVT.getDataDoubleValue("Select Sum(TOTCESSAMT)  From TrnHdrSINV Where IsDelete='N' and Cancelled='N' and SINVDate='" & getDateFormatInInsert(dtpDay.Value) & "' ", 0, False)

            strSQL = "Select Sum(TOTCESSAMT)  From TrnHdrSINV h Where IsDelete='N'"
            strSQL &= " and Cancelled='N' and SINVDate='" & getDateFormatInInsert(dtpDay.Value) & "' "
            If Not blnIncludeNonLiq Then
                strSQL &= " And h.SINVID In (Select distinct SINVID from TrnDtlSINV d, MstItem I where d.ItemID=i.ItemID And i.ITEMGroupID In (Select ITEMGROUPID from mstitemgroup where LiqItem='Y')) "
            End If
            dblCessAmt = clsSVT.getDataDoubleValue(strSQL, 0, False)
        End If

        Dim blnFound As Boolean = False
        clsSVT.getDataStringValue("select name from sys.tables where name='TrnHdrSR'", "", blnFound)
        If blnFound Then
            ' dblTotSalesReturn = clsSVT.getDataDoubleValue("Select Sum(TotAmt)  From TrnHdrSR Where Cancelled='N' and SRDate='" & getDateFormatInInsert(dtpDay.Value) & "' ", 0, False)
            strSQL = "Select Sum(TotAmt)  From TrnHdrSR h Where Cancelled='N'"
            strSQL &= " and SRDate='" & getDateFormatInInsert(dtpDay.Value) & "' "
            If Not blnIncludeNonLiq Then
                strSQL &= " And h.SRID In (Select distinct SRID from TrnDtlSR d, MstItem I where d.ItemID=i.ItemID And i.ITEMGroupID In (Select ITEMGROUPID from mstitemgroup where LiqItem='Y')) "
            End If
            dblTotSalesReturn = clsSVT.getDataDoubleValue(strSQL, 0, False)

        Else
            dblTotSalesReturn = 0
        End If

        '  dblTotExpense = clsSVT.getDataDoubleValue("Select Sum(TotAmt)  From TrnHdrExp Where Cancelled='N' and EXPDate='" & getDateFormatInInsert(dtpDay.Value) & "' ", 0, False)

        strSQL = "Select Sum(TotAmt)  From TrnHdrExp h Where Cancelled='N'  "
        strSQL &= "and EXPDate='" & getDateFormatInInsert(dtpDay.Value) & "' "
        'If Not blnIncludeNonLiq Then
        '    strSQL &= " And h.ExpID In (Select distinct ExpID from TrnDtlEXP d, MstItem I where d.ItemID=i.ItemID And i.ITEMGroupID In (Select ITEMGROUPID from mstitemgroup where LiqItem='Y')) "
        'End If
        dblTotExpense = clsSVT.getDataDoubleValue(strSQL, 0, False)


        dblNetAmount = dblTotInvValue - dblTotSalesReturn - dblTotExpense

        ' dblOPQty = clsSVT.getDataDoubleValue("Select Sum(Qty * StkMultiplier) as OpQty from UtlStockList Where DocDate < '" & getDateFormatInInsert(dtpDay.Value) & "'", 0, False)


        dblOPQty = clsSVT.getDataDoubleValue("Select Sum(Qty * StkMultiplier) as OpQty from UtlStockList Where DocDate < '" & getDateFormatInInsert(dtpDay.Value) & "' and ItemID in (Select distinct ItemID from MstItem I, MstITEMGroup g where i.ITEMGROUPID=g.ItemGroupID" & IIf(blnIncludeNonLiq = False, " and g.LiqItem='Y'", "") & ") ", 0, False)

        If optPurRate.Checked Then
            dblOPVal = clsSVT.getDataDoubleValue("select sum(opbal) from (Select Cast(Round((isnull((Select top 1 PBRate from vwPBItemLastPrice v where u.ItemID = v.itemid  And pbdate <= " & getDateFormatInCond(dtpDay.Value) & " order by PBdate Desc),0)) * Qty * StkMultiplier,2) as Numeric(18,2))  as OpBal from UtlStockList u, MstItem i Where u.ItemID=i.ItemID and DocDate <'" & getDateFormatInInsert(dtpDay.Value) & "' and u.ItemID in (Select distinct ItemID from MstItem I, MstITEMGroup g where i.ITEMGROUPID=g.ItemGroupID" & IIf(blnIncludeNonLiq = False, " and g.LiqItem='Y'", "") & "))a ", 0, False)
        Else
            dblOPVal = clsSVT.getDataDoubleValue("Select Sum(MRP * Qty * StkMultiplier) as OpBal from UtlStockList  u ,MstItem i Where  u.ItemID=i.ItemID and DocDate < '" & getDateFormatInInsert(dtpDay.Value) & "' and i.ItemID in (Select distinct ItemID from MstItem I, MstITEMGroup g where i.ITEMGROUPID=g.ItemGroupID" & IIf(blnIncludeNonLiq = False, " and g.LiqItem='Y'", "") & ") ", 0, False)
        End If

        ' dblOPVal = clsSVT.getDataDoubleValue("Select Sum(MRP * Qty * StkMultiplier)  as OpBal from UtlStockList u, MstItem i Where u.ItemID=i.ItemID and DocDate <'" & getDateFormatInInsert(dtpDay.Value) & "'", 0, False)

        ' dblCLQty = clsSVT.getDataDoubleValue("Select Sum(Qty * StkMultiplier) as ClBal from UtlStockList Where DocDate <= '" & getDateFormatInInsert(dtpDay.Value) & "'", 0, False)

        dblCLQty = clsSVT.getDataDoubleValue("Select Sum( Qty * StkMultiplier) as ClBal from UtlStockList u  Where  DocDate <= '" & getDateFormatInInsert(dtpDay.Value) & "' and ItemID in (Select distinct ItemID from MstItem I, MstITEMGroup g where i.ITEMGROUPID=g.ItemGroupID" & IIf(blnIncludeNonLiq = False, " and g.LiqItem='Y'", "") & ") ", 0, False)


        '  dblCLVal = clsSVT.getDataDoubleValue("Select Sum(MRP * Qty * StkMultiplier)  as ClBal from UtlStockList u, MstItem i Where u.ItemID=i.ItemID and DocDate <='" & getDateFormatInInsert(dtpDay.Value) & "'", 0, False)

        If optPurRate.Checked Then
            dblCLVal = clsSVT.getDataDoubleValue("Select Sum(CLBAL) from (select isnull((Select top 1 PBRate from vwPBItemLastPrice v where i.ItemID = v.itemid  And pbdate <= " & getDateFormatInCond(dtpDay.Value) & " order by PBdate Desc),0) * Qty * StkMultiplier  as ClBal from UtlStockList u, MstItem i Where u.ItemID=i.ItemID and DocDate <='" & getDateFormatInInsert(dtpDay.Value) & "' and i.ItemGroupID in (Select distinct ItemGroupID from MstITEMGroup g where Inactive='N' " & IIf(blnIncludeNonLiq = False, " and g.LiqItem='Y'", "") & "))a ", 0, False)
        Else
            dblCLVal = clsSVT.getDataDoubleValue("Select Sum(MRP * Qty * StkMultiplier) as ClBal from UtlStockList u,MstItem i  Where  u.ItemID=i.ItemId and DocDate <= '" & getDateFormatInInsert(dtpDay.Value) & "' and i.ItemID in (Select distinct ItemID from MstItem I, MstITEMGroup g where i.ITEMGROUPID=g.ItemGroupID" & IIf(blnIncludeNonLiq = False, " and g.LiqItem='Y'", "") & ") ", 0, False)
        End If

        dblUPIAmt = clsSVT.getDataDoubleValue("Select Sum(Amount) As Amount from vwMultiAmount Where SINVDate = " & getDateFormatInCond(dtpDay.Value) & "", 0, False)

        Dim strSF As String
        Dim strFormulas(,) As String = Nothing

        'If UCase(gStrDBName) = "VLQMSNDROPS" Then

        '    ReDim strFormulas(22, 1)

        '    strFormulas(0, 0) = "compname"
        '    strFormulas(0, 1) = gSettings.gCompName

        '    strFormulas(1, 0) = "add1"
        '    strFormulas(1, 1) = gSettings.gCompAdd1

        '    strFormulas(2, 0) = "add2"
        '    strFormulas(2, 1) = gSettings.gCompAdd2

        '    strFormulas(3, 0) = "add3"
        '    strFormulas(3, 1) = gSettings.gCompAdd3

        '    strFormulas(4, 0) = "add4"
        '    strFormulas(4, 1) = gSettings.gCompAdd4

        '    strFormulas(5, 0) = "phno"
        '    strFormulas(5, 1) = gSettings.gCompPHNO

        '    strFormulas(6, 0) = "gstin"
        '    strFormulas(6, 1) = gSettings.gCompGSTIN

        '    strFormulas(7, 0) = "mNoOfInv"
        '    strFormulas(7, 1) = intNoOfInv

        '    strFormulas(8, 0) = "mCashSales"
        '    strFormulas(8, 1) = dblCashSales

        '    strFormulas(9, 0) = "mCardSales"
        '    strFormulas(9, 1) = dblCardSales

        '    strFormulas(10, 0) = "totProfit"
        '    strFormulas(10, 1) = dblProfitAmt

        '    strFormulas(11, 0) = "mCGSTAMT"
        '    strFormulas(11, 1) = dblCgstAmt

        '    strFormulas(12, 0) = "mSGSTAMT"
        '    strFormulas(12, 1) = dblSgstAmt

        '    strFormulas(13, 0) = "mCESSAMT"
        '    strFormulas(13, 1) = dblCessAmt

        '    strFormulas(14, 0) = "mTotInvValue"
        '    strFormulas(14, 1) = dblTotInvValue

        '    strFormulas(15, 0) = "mSalesRet"
        '    strFormulas(15, 1) = dblTotSalesReturn

        '    strFormulas(16, 0) = "mTotExp"
        '    strFormulas(16, 1) = dblTotExpense

        '    strFormulas(17, 0) = "mNetAmt"
        '    strFormulas(17, 1) = dblNetAmount

        '    strFormulas(18, 0) = "mOpQty"
        '    strFormulas(18, 1) = dblOPQty

        '    strFormulas(19, 0) = "mOpVal"
        '    strFormulas(19, 1) = dblOPVal

        '    strFormulas(20, 0) = "mCLQty"
        '    strFormulas(20, 1) = dblCLQty

        '    strFormulas(21, 0) = "mCLVal"
        '    strFormulas(21, 1) = dblCLVal

        '    strFormulas(22, 0) = "mUPIAmt"
        '    strFormulas(22, 1) = dblUPIAmt
        'Else


        ReDim strFormulas(21, 1)

        strFormulas(0, 0) = "compname"
        strFormulas(0, 1) = gSettings.gCompName

        strFormulas(1, 0) = "add1"
        strFormulas(1, 1) = gSettings.gCompAdd1

        strFormulas(2, 0) = "add2"
        strFormulas(2, 1) = gSettings.gCompAdd2

        strFormulas(3, 0) = "add3"
        strFormulas(3, 1) = gSettings.gCompAdd3

        strFormulas(4, 0) = "add4"
        strFormulas(4, 1) = gSettings.gCompAdd4

        strFormulas(5, 0) = "phno"
        strFormulas(5, 1) = gSettings.gCompPHNO

        strFormulas(6, 0) = "gstin"
        strFormulas(6, 1) = gSettings.gCompGSTIN

        strFormulas(7, 0) = "mNoOfInv"
        strFormulas(7, 1) = intNoOfInv

        strFormulas(8, 0) = "mCashSales"
        strFormulas(8, 1) = dblCashSales

        strFormulas(9, 0) = "mCardSales"
        strFormulas(9, 1) = dblCardSales

        strFormulas(10, 0) = "mCGSTAMT"
        strFormulas(10, 1) = dblCgstAmt

        strFormulas(11, 0) = "mSGSTAMT"
        strFormulas(11, 1) = dblSgstAmt

        strFormulas(12, 0) = "mCESSAMT"
        strFormulas(12, 1) = dblCessAmt

        strFormulas(13, 0) = "mTotInvValue"
        strFormulas(13, 1) = dblTotInvValue

        strFormulas(14, 0) = "mSalesRet"
        strFormulas(14, 1) = dblTotSalesReturn

        strFormulas(15, 0) = "mTotExp"
        strFormulas(15, 1) = dblTotExpense

        strFormulas(16, 0) = "mNetAmt"
        strFormulas(16, 1) = dblNetAmount

        strFormulas(17, 0) = "mOpQty"
        strFormulas(17, 1) = dblOPQty

        strFormulas(18, 0) = "mOpVal"
        strFormulas(18, 1) = dblOPVal

        strFormulas(19, 0) = "mCLQty"
        strFormulas(19, 1) = dblCLQty

        strFormulas(20, 0) = "mCLVal"
        strFormulas(20, 1) = dblCLVal

        strFormulas(21, 0) = "mUPIAmt"
        strFormulas(21, 1) = dblUPIAmt
        ' End If



        strSF = "{RptUDR.RptName} = 'Daily Summary Report' and {RptUDR.Username}='" & gUserCode & "'"

        Dim frmv As New frmUtlReportViewer
        With frmv
            .StartPosition = FormStartPosition.CenterScreen
            If blnShowPDF Then
                Try
                    If Not Directory.Exists(gstrRptPath & "Temp") Then Directory.CreateDirectory(gstrRptPath & "Temp")
                    strPdfFileName = gstrRptPath & "Temp\DSR_" & Date.Now.ToString("ddmmyy") & ".pdf"
                    If File.Exists(strPdfFileName) Then File.Delete(strPdfFileName)
                Catch ex As Exception
                    MsgBox(ex.Message, MsgBoxStyle.Critical, Me.Text)
                End Try
                .showRptFile(Me.Text, "RptDSR.rpt", strSF, strFormulas, False, True, False,,, True, strPdfFileName)
                .Close()
                If Not argBlnWA Then
                    Dim psi As New ProcessStartInfo()
                    With psi
                        .FileName = strPdfFileName
                        .UseShellExecute = True
                    End With
                    Process.Start(psi)
                Else
                    Dim StrMsg As String = ""

                    gstrBold = getGsettings("WhatsApp", "blnBold", "")
                    gstrItalic = getGsettings("WhatsApp", "blnItalic", "")
                    gstrUnderline = getGsettings("WhatsApp", "blnUnderline", "")

                    StrMsg = Me.Text
                    gStrEmoji = clsSVT.getDataStringValue("Select WAEmoji from UtlMisc", "", False)

                    If gstrBold = "Y" AndAlso gstrItalic = "Y" Then
                        StrMsg = "_*" & StrMsg & "*_"
                        GoTo Send
                    End If
                    If gstrBold = "Y" Then
                        StrMsg = "*" & StrMsg & "*"
                    End If
                    If gstrItalic = "Y" Then
                        StrMsg = "_" & StrMsg & "_"
                    End If
                    If gstrUnderline = "Y" Then

                    End If
Send:
                    If Not SendWhatsApp(StrMsg, gStrWatsAppRcvrNo, strPdfFileName, gStrEmoji) Then
                        MsgBox("Not Sent....", MsgBoxStyle.Exclamation, Me.Text)
                        Exit Sub
                    End If
                End If
            Else
                .showRptFile(Me.Text, "RptDSR.rpt", strSF, strFormulas, False, True, True, "", 1, False, "")
                .ShowDialog()
            End If
        End With
    End Sub
    'Private Sub btnEmailPDF_Click(sender As Object, e As EventArgs) Handles btnEmailPDF.Click
    '    showWaitCursor()
    '    Dim strFileName As String = ""
    '    strFileName = gstrRptPath & "DSR" & CStr(Format(Date.Now, "yyyyMMddHHmmss")) & ".pdf"
    '    Call PrintData(False, strFileName)

    '    Call showWaitCursor()

    '    Try
    '        If gSmtpMailID = "" Then
    '            MsgBox("SMTP Configuration Empty....", MsgBoxStyle.Exclamation, Me.Text)
    '            Exit Sub
    '        End If

    '        If gStrRptEmailID = "" Then
    '            MsgBox("Please configure email 'TO' Address..", MsgBoxStyle.Exclamation, Me.Text)
    '            Exit Sub
    '        End If


    '        Dim Smtp_Server As New Net.Mail.SmtpClient
    '        Dim e_mail As New Net.Mail.MailMessage()
    '        Smtp_Server.UseDefaultCredentials = False
    '        Smtp_Server.Credentials = New Net.NetworkCredential(CStr(gSmtpMailID & ""), CStr(gSmtpMailPwd & ""))
    '        Smtp_Server.Port = CStr(gSmtpServerPort & "")
    '        If CStr(gSmtpEnableSSL & "") = "Y" Then
    '            Smtp_Server.EnableSsl = True
    '        Else
    '            Smtp_Server.EnableSsl = False
    '        End If
    '        Smtp_Server.Host = CStr(gSmtpHostName & "")
    '        e_mail.From = New Net.Mail.MailAddress(CStr(gSmtpMailID & ""))
    '        For Each str As String In gStrRptEmailID.Split(";")
    '            e_mail.To.Add(str)
    '        Next
    '        e_mail.Subject = "Daily Summary Report"
    '        e_mail.IsBodyHtml = False
    '        e_mail.Body = "Automatic Email Pls don't reply....."
    '        Dim attachment As Net.Mail.Attachment
    '        attachment = New System.Net.Mail.Attachment(strFileName)
    '        e_mail.Attachments.Add(attachment)
    '        Smtp_Server.Send(e_mail)
    '        MsgBox("Email Sent....", MsgBoxStyle.Information, Me.Text)
    '        showNormalCursor()
    '    Catch ex As Exception
    '        showNormalCursor()
    '    End Try
    'End Sub

    Private Sub btnEmailPDF_Click(sender As Object, e As EventArgs) Handles btnEmailPDF.Click
        Call PrintData(False)
    End Sub

    Private Sub btnWAMsg_Click(sender As Object, e As EventArgs) Handles btnWAMsg.Click
        If gStrWatsAppFile = "" Then
            MsgBox("Whats App Settings Not Configured....", MsgBoxStyle.Exclamation, Me.Text)
            Exit Sub
        End If

        If gStrWatsAppRcvrNo = "" Then
            MsgBox("Whats App Receiver No is Empty....", MsgBoxStyle.Exclamation, Me.Text)
            Exit Sub
        End If
        Call PrintData(True, "", True)
    End Sub

    Private Sub btnEmail_Click(sender As Object, e As EventArgs) Handles btnEmail.Click
        Call PDFSendMail()
    End Sub

    Public Sub PDFSendMail()

        Dim strSQL As String = ""
        Dim strPdfFileName As String = ""
        strSQL = "Delete from RptUDR Where RptName='Daily Summary Report' and Username='" & gUserCode & "'"
        clsSVT.execSQLStatement(strSQL)

        'If UCase(gStrDBName) = "VLQMSCNR" Then
        strSQL = "Insert Into RptUDR (RptName, UserName, Fld1, Fld2, Fld3, Fld4)"
        strSQL &= " Select 'Daily Summary Report', '" & gUserCode & "', ITEMGROUPCODE, Sum(d.SINVQty) as Qty, Sum(d.ItemAmt) as Amount, 'Sales' "
        strSQL &= " from TrnHdrSINV h, TrnDtlSINV d, MstItem i, MstItemGroup ig "
        strSQL &= " Where h.SINVID=d.SINVID and h.Cancelled='N' and isdelete='N' "
        strSQL &= " and h.SINVDate = '" & getDateFormatInInsert(dtpDay.Value) & "' "
        strSQL &= " and d.ItemID=i.ItemID and i.ITEMGroupID=ig.ITEMGROUPID "
        strSQL &= " Group by ItemGroupCode "
        strSQL &= " Order by 1"
        clsSVT.execSQLStatement(strSQL)

        strSQL = "Insert Into RptUDR (RptName, UserName, Fld1, Fld2, Fld3, Fld4)"
        strSQL &= "Select 'Daily Summary Report', '" & gUserCode & "',g.ItemGroupCode, Sum(convert(Int,d.PBQty)) as Qty, Sum(i.MRP * d.PBQty) as Amount, 'Purchase'"
        strSQL &= " From TrnHdrPB h, TrnDtlPB d, MstItem i, MstItemGroup g  "
        strSQL &= " Where h.PBID=d.PBID and d.ITEMID=i.ITEMID and i.ITEMGroupID=g.ITEMGROUPID "
        strSQL &= "and Cancelled='N'  and PBDate = " & getDateFormatInCond(dtpDay.Value) & " Group by g.ItemGroupCode"
        clsSVT.execSQLStatement(strSQL)
        'Else
        '    strSQL = "Insert Into RptUDR (RptName, UserName, Fld1, Fld2, Fld3)"
        '    strSQL &= " Select 'Daily Summary Report', '" & gUserCode & "', ITEMGROUPCODE, Sum(d.SINVQty) as Qty, Sum(d.ItemAmt) as Amount "
        '    strSQL &= " from TrnHdrSINV h, TrnDtlSINV d, MstItem i, MstItemGroup ig "
        '    strSQL &= " Where h.SINVID=d.SINVID and h.Cancelled='N' "
        '    strSQL &= " and h.SINVDate = '" & getDateFormatInInsert(dtpDay.Value) & "' "
        '    strSQL &= " and d.ItemID=i.ItemID and i.ITEMGroupID=ig.ITEMGROUPID "
        '    strSQL &= " Group by ItemGroupCode "
        '    strSQL &= " Order by 1"
        '    clsSVT.execSQLStatement(strSQL)
        'End If

        Dim intNoOfInv As Int32, dblCashSales As Double, dblCardSales As Double, dblTotInvValue As Double, dblCgstAmt As Double, dblSgstAmt As Double, dblCessAmt As Double, dblOPQty As Double, dblOPVal As Double, dblCLQty As Double, dblCLVal As Double, dblUPIAmt As Double

        Dim dblTotSalesReturn As Double, dblTotExpense As Double, dblNetAmount As Double, dblTotpurchase As Double, dblTotSales As Double, dblProfitAmt As Double

        intNoOfInv = clsSVT.getDataLongValue("Select Count(*) From TrnHdrSINV Where Cancelled='N' and IsDelete='N' and SINVDate='" & getDateFormatInInsert(dtpDay.Value) & "' ", 0, False)

        dblCashSales = clsSVT.getDataDoubleValue("Select Sum(CashAmt)  From TrnHdrSINV Where IsDelete='N' and Cancelled='N' and SINVDate='" & getDateFormatInInsert(dtpDay.Value) & "' ", 0, False)

        dblCardSales = clsSVT.getDataDoubleValue("Select Sum(CardAmt)  From TrnHdrSINV Where IsDelete='N' and Cancelled='N' and SINVDate='" & getDateFormatInInsert(dtpDay.Value) & "' ", 0, False)

        ' dblTotInvValue = clsSVT.getDataDoubleValue("Select Sum(INVValue)  From TrnHdrSINV Where IsDelete='N' and Cancelled='N' and SINVDate='" & getDateFormatInInsert(dtpDay.Value) & "' ", 0, False)


        strSQL = "Select sum(d.ITEMAMT) From mstitem i, trndtlsinv d, trnhdrsinv h"
        strSQL &= " Where h.sinvid = d.sinvid and d.itemid = i.itemid  "
        strSQL &= " and SINVDate = " & getDateFormatInCond(dtpDay.Value) & " and IsDelete='N' and Cancelled = 'N'  "
        dblTotInvValue = clsSVT.getDataDoubleValue(strSQL, 0, False)

        dblCgstAmt = clsSVT.getDataDoubleValue("Select Sum(TOTCGSTAMT)  From TrnHdrSINV Where IsDelete='N' and Cancelled='N' and SINVDate='" & getDateFormatInInsert(dtpDay.Value) & "' ", 0, False)

        dblSgstAmt = clsSVT.getDataDoubleValue("Select Sum(TOTSGSTAMT)  From TrnHdrSINV Where IsDelete='N' and Cancelled='N' and SINVDate='" & getDateFormatInInsert(dtpDay.Value) & "' ", 0, False)


        If UCase(gStrDBName) = "VLQMSNDROPS" Then
            strSQL = "select sum(i.PURRATE * sinvqty)  as Purchaserate  from trnhdrsinv h ,trndtlsinv d, mstitem i"
            strSQL &= " where h.sinvid=d.sinvid and d.itemid=i.ITEMID"
            strSQL &= " and SINVDate = " & getDateFormatInCond(dtpDay.Value) & " and IsDelete='N' and Cancelled = 'N'  "
            dblTotpurchase = clsSVT.getDataDoubleValue(strSQL, 0, False)

            strSQL = "select sum(sinvamt) as SaleAmt from trnhdrsinv h ,trndtlsinv d, mstitem i"
            strSQL &= " where h.sinvid=d.sinvid and d.itemid=i.ITEMID"
            strSQL &= " and SINVDate = " & getDateFormatInCond(dtpDay.Value) & " and IsDelete='N' and Cancelled = 'N'  "
            dblTotSales = clsSVT.getDataDoubleValue(strSQL, 0, False)

            dblProfitAmt = dblTotSales - dblTotpurchase
            Call addRow("Sales Profit Of the Day", dblProfitAmt)
        End If



        If fnFldPresent("TOTCESSAMT", "TrnHdrSINV") Then
            dblCessAmt = clsSVT.getDataDoubleValue("Select Sum(TOTCESSAMT)  From TrnHdrSINV Where IsDelete='N' and Cancelled='N' and SINVDate='" & getDateFormatInInsert(dtpDay.Value) & "' ", 0, False)
        End If

        Dim blnFound As Boolean = False
        clsSVT.getDataStringValue("select name from sys.tables where name='TrnHdrSR'", "", blnFound)
        If blnFound Then
            dblTotSalesReturn = clsSVT.getDataDoubleValue("Select Sum(TotAmt)  From TrnHdrSR Where Cancelled='N' and SRDate='" & getDateFormatInInsert(dtpDay.Value) & "' ", 0, False)
        Else
            dblTotSalesReturn = 0
        End If

        dblTotExpense = clsSVT.getDataDoubleValue("Select Sum(TotAmt)  From TrnHdrExp Where Cancelled='N' and EXPDate='" & getDateFormatInInsert(dtpDay.Value) & "' ", 0, False)

        dblNetAmount = dblTotInvValue - dblTotSalesReturn - dblTotExpense


        dblOPQty = clsSVT.getDataDoubleValue("Select Sum(Qty * StkMultiplier) as OpQty from UtlStockList Where DocDate < '" & getDateFormatInInsert(dtpDay.Value) & "'", 0, False)


        dblOPVal = clsSVT.getDataDoubleValue("Select Sum(MRP * Qty * StkMultiplier)  as OpBal from UtlStockList u, MstItem i Where u.ItemID=i.ItemID and DocDate <'" & getDateFormatInInsert(dtpDay.Value) & "'", 0, False)

        dblCLQty = clsSVT.getDataDoubleValue("Select Sum(Qty * StkMultiplier) as ClBal from UtlStockList Where DocDate <= '" & getDateFormatInInsert(dtpDay.Value) & "'", 0, False)

        dblCLVal = clsSVT.getDataDoubleValue("Select Sum(MRP * Qty * StkMultiplier)  as ClBal from UtlStockList u, MstItem i Where u.ItemID=i.ItemID and DocDate <='" & getDateFormatInInsert(dtpDay.Value) & "'", 0, False)

        dblUPIAmt = clsSVT.getDataDoubleValue("Select Sum(Amount) As Amount from vwMultiAmount Where SINVDate = " & getDateFormatInCond(dtpDay.Value) & "", 0, False)

        Dim strSF As String
        Dim strFormulas(,) As String = Nothing
        ReDim strFormulas(21, 1)

        strFormulas(0, 0) = "compname"
        strFormulas(0, 1) = gSettings.gCompName

        strFormulas(1, 0) = "add1"
        strFormulas(1, 1) = gSettings.gCompAdd1

        strFormulas(2, 0) = "add2"
        strFormulas(2, 1) = gSettings.gCompAdd2

        strFormulas(3, 0) = "add3"
        strFormulas(3, 1) = gSettings.gCompAdd3

        strFormulas(4, 0) = "add4"
        strFormulas(4, 1) = gSettings.gCompAdd4

        strFormulas(5, 0) = "phno"
        strFormulas(5, 1) = gSettings.gCompPHNO

        strFormulas(6, 0) = "gstin"
        strFormulas(6, 1) = gSettings.gCompGSTIN

        strFormulas(7, 0) = "mNoOfInv"
        strFormulas(7, 1) = intNoOfInv

        strFormulas(8, 0) = "mCashSales"
        strFormulas(8, 1) = dblCashSales

        strFormulas(9, 0) = "mCardSales"
        strFormulas(9, 1) = dblCardSales

        strFormulas(10, 0) = "mCGSTAMT"
        strFormulas(10, 1) = dblCgstAmt

        strFormulas(11, 0) = "mSGSTAMT"
        strFormulas(11, 1) = dblSgstAmt

        strFormulas(12, 0) = "mCESSAMT"
        strFormulas(12, 1) = dblCessAmt

        strFormulas(13, 0) = "mTotInvValue"
        strFormulas(13, 1) = dblTotInvValue

        strFormulas(14, 0) = "mSalesRet"
        strFormulas(14, 1) = dblTotSalesReturn

        strFormulas(15, 0) = "mTotExp"
        strFormulas(15, 1) = dblTotExpense

        strFormulas(16, 0) = "mNetAmt"
        strFormulas(16, 1) = dblNetAmount

        strFormulas(17, 0) = "mOpQty"
        strFormulas(17, 1) = dblOPQty

        strFormulas(18, 0) = "mOpVal"
        strFormulas(18, 1) = dblOPVal

        strFormulas(19, 0) = "mCLQty"
        strFormulas(19, 1) = dblCLQty

        strFormulas(20, 0) = "mCLVal"
        strFormulas(20, 1) = dblCLVal

        strFormulas(21, 0) = "mUPIAmt"
        strFormulas(21, 1) = dblUPIAmt
        ' End If



        strSF = "{RptUDR.RptName} = 'Daily Summary Report' and {RptUDR.Username}='" & gUserCode & "'"

        Dim frmv As New frmUtlReportViewer
        With frmv
            .StartPosition = FormStartPosition.CenterScreen
            Try
                If Not Directory.Exists(gstrRptPath & "Temp") Then Directory.CreateDirectory(gstrRptPath & "Temp")
                strPdfFileName = gstrRptPath & "Temp\DSR_" & Date.Now.ToString("ddmmyy") & ".pdf"
                If File.Exists(strPdfFileName) Then File.Delete(strPdfFileName)
            Catch ex As Exception
                MsgBox(ex.Message, MsgBoxStyle.Critical, Me.Text)
            End Try
            .showRptFile(Me.Text, "RptDROPSDSR.rpt", strSF, strFormulas, False, True, False,,, True, strPdfFileName)
            .Close()

            Dim strToEmail As String = "", strCCEmail As String = "", strContent As String = "", strSub As String = ""
            strSub = "Daily Summary Report and DisCount Scheme Report Pdf"
            strToEmail = getGsettings("Company", "CHEADEID")
            If strToEmail = "" Then Exit Sub

            ''If Not sendOutLookMail(strToEmail, strCCEmail, strSub, strContent, strPdfFileName) Then
            ''    Exit Sub
            ''End If

            'If Not sendGenMail(strToEmail, strCCEmail, strSub, strContent, strPdfFileName) Then
            '    MsgBox("Email Not Sent to " & strToEmail & "", vbCritical, Me.Text)
            '    Exit Sub
            'End If

            showWaitCursor()
            'the below code is to send mail through SMTP or Outlook - Akshay 26/09/2024
            'if SMTP mail fails it will try to send through Outlook if configured else error
            If Not sendGenMail(strToEmail, strCCEmail, strSub, strContent, strPdfFileName, False) Then
                If Not sendOutLookMail(strToEmail, strCCEmail, strSub, strContent, strPdfFileName, False) Then
                    MsgBox("Email Not Sent to " & strToEmail & "", vbCritical, Me.Text)
                    Exit Sub
                End If
            End If
            showNormalCursor()

            MsgBox("Email Sent Successfully", vbInformation, Me.Text)

            'Try
            '    strSF = "{vwdiscschmdetail.SINVDate} = '" & getFirstDateInMonth(getSystemDate) & "' between {vwdiscschmdetail.SINVDate}='" & getLastDateInMonth(getSystemDate) & "' "
            '    If Not Directory.Exists(gstrRptPath & "Temp") Then Directory.CreateDirectory(gstrRptPath & "Temp")
            '    strPdfFileName = gstrRptPath & "Temp\DISCSCHM_" & Date.Now.ToString("ddmmyy") & ".pdf"
            '    If File.Exists(strPdfFileName) Then File.Delete(strPdfFileName)
            'Catch ex As Exception
            '    MsgBox(ex.Message, MsgBoxStyle.Critical, Me.Text)
            'End Try
            '.showRptFile(Me.Text, "RptDISCSCHM.rpt", strSF, strFormulas, False, True, False,,, True, strPdfFileName)
            '.Close()
            'If Not sendOutLookMail(strToEmail, strCCEmail, strSub, strContent, strPdfFileName) Then
            '    Exit Sub
            'End If



        End With
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

    Private Sub vsfDet_Click(sender As Object, e As EventArgs) Handles vsfView.Click

        If (Control.ModifierKeys And Keys.Control) = Keys.Control AndAlso (Control.ModifierKeys And Keys.Alt) = Keys.Alt Then
            pnlCopyShortCut.Show()
        End If

    End Sub

End Class