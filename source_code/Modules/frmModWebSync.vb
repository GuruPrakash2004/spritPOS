Imports System.IO
Imports System.Data.OleDb
Public Module frmModWebSync
    Const strWebConn As String = ""
    Dim clsSVT As New svtLibrary
    Dim intWebClientID As Integer
    Dim dtStart As Date, dtDateTime As DateTime
    Dim svtTrn As OleDbTransaction
    Dim webConn As New OleDb.OleDbConnection
    Public Sub syncDataToWeb(Optional argShowMsg As Boolean = True)
        Dim dt As Date
        intWebClientID = 0
        dtDateTime = getSystemDateTime()
        If Not fnFldPresent("WebClientID", "UtlMisc") Then
            Call clsSVT.execSQLStatement("Alter table UtlMisc add WebClientID int not null default 0")
        End If
        intWebClientID = clsSVT.getDataLongValue("Select WebClientID  from UtlMisc", 0, False)
        If intWebClientID = 0 Then
            If argShowMsg Then
                MsgBox("NO Web Client ID. Please contact Softvent Administrator", MsgBoxStyle.Critical, "ERROR")
            End If
            Exit Sub
        End If

        webConn.ConnectionString = webConnString
        Try
            webConn.Open()
            'Call clsSVT.execSQLStatement("Use WebVLQMS",, webConn)
            svtTrn = webConn.BeginTransaction(IsolationLevel.ReadCommitted)
            Call deleteForTodaysData()

            dt = DateAdd(DateInterval.Day, -8, getSystemDate())
            For i = 1 To 8
                dt = DateAdd(DateInterval.Day, 1, dt)
                If Not pushData(dt, argShowMsg) Then
                    svtTrn.Rollback()
                    If argShowMsg Then
                        MsgBox("Sync not done!", MsgBoxStyle.Critical, "ERROR")
                    End If
                    Exit Sub
                End If
            Next i

            Dim strSQL As String, strIQry As String
            Dim rs As OleDbDataReader
            '===============================================
            'TrnHdrPI
            '===============================================

            strSQL = " select PIID,PINO,Prefix,PrintPINO,PIDate,PIDateYear,PIDateMonth,SUPPName,"
            strSQL &= " PartyName,PartyAdd1,PartyAdd2,PartyAdd3,PartyAdd4,PartyCity,PartyState,PartyPinCode,"
            strSQL &= " PartyCountry,PartyCName,PartyCMob,PartyCEmail,PartyCDesig,h.Authorised,h.AuthBy,"
            strSQL &= " h.AuthOn,h.ChangedBy,h.ChangedOn,Cancelled,h.AddedBy,h.AddedOn,AMTINWRDS,RMKS,"
            strSQL &= " LOCID,TOTAMT,OTHAMT,GRNDTOT,TOTCGST,TOTSGST,TOTIGST,BILLSYSTEM,TEST from TRNHDRPI H, MstSupp S where H.Suppid=S.Suppid "
            rs = clsSVT.getDataReaderFromSQL(strSQL)
            If rs.HasRows Then
                Do While rs.Read
                    strIQry = " Insert into TRNHDRPI (ClientID,PIID,PINO,Prefix,PrintPINO,PIDate,SUPPName,"
                    strIQry &= " PartyName,PartyAdd1,PartyAdd2,PartyAdd3,PartyAdd4,PartyCity,PartyState,PartyPinCode,"
                    strIQry &= " PartyCountry,PartyCName,PartyCMob,PartyCEmail,PartyCDesig,h.Authorised,h.AuthBy,"
                    strIQry &= " h.AuthOn,h.ChangedBy,h.ChangedOn,Cancelled,h.AddedBy,h.AddedOn,AMTINWRDS,RMKS,"
                    strIQry &= " LOCID,TOTAMT,OTHAMT,GRNDTOT,TOTCGST,TOTSGST,TOTIGST,BILLSYSTEM,TEST)"
                    strIQry &= " values ("
                    strIQry &= intWebClientID & ","
                    strIQry &= rs!PIID & ","
                    strIQry &= rs!PINO & ","
                    strIQry &= "'" & rs!Prefix & "',"
                    strIQry &= "'" & rs!PrintPINO & "',"
                    strIQry &= "'" & getDateFormatInInsert(rs!PIDate) & "',"
                    strIQry &= "'" & rs!SUPPName & "',"
                    strIQry &= "'" & rs!PartyName & "',"
                    strIQry &= "'" & rs!PartyAdd1 & "',"
                    strIQry &= "'" & rs!PartyAdd2 & "',"
                    strIQry &= "'" & rs!PartyAdd3 & "',"
                    strIQry &= "'" & rs!PartyAdd4 & "',"
                    strIQry &= "'" & rs!PartyCity & "',"
                    strIQry &= "'" & rs!PartyState & "',"
                    strIQry &= "'" & rs!PartyPinCode & "',"
                    strIQry &= "'" & rs!PartyCountry & "',"
                    strIQry &= "'" & rs!PartyCName & "',"
                    strIQry &= "'" & rs!PartyCMob & "',"
                    strIQry &= "'" & rs!PartyCEmail & "',"
                    strIQry &= "'" & rs!PartyCDesig & "',"
                    strIQry &= "'" & rs!Authorised & "',"
                    strIQry &= "'" & rs!AuthBy & "',"
                    If IsDate(rs!AuthOn) Then
                        strIQry &= "'" & getDateFormatInInsert(rs!AuthOn) & "',"
                    Else
                        strIQry &= " Null,"
                    End If

                    strIQry &= "'" & rs!ChangedBy & "',"
                    If IsDate(rs!ChangedOn) Then
                        strIQry &= "'" & getDateFormatInInsert(rs!ChangedOn) & "',"
                    Else
                        strIQry &= " Null,"
                    End If
                    strIQry &= "'" & rs!Cancelled & "',"
                    strIQry &= "'" & rs!AddedBy & "',"
                    If IsDate(rs!AddedOn) Then
                        strIQry &= "'" & getDateFormatInInsert(rs!AddedOn) & "',"
                    Else
                        strIQry &= " Null,"
                    End If
                    strIQry &= "'" & rs!AMTINWRDS & "',"
                    strIQry &= "'" & rs!RMKS & "',"
                    strIQry &= "" & rs!LOCID & ","
                    strIQry &= "" & rs!TOTAMT & ","
                    strIQry &= "" & rs!OTHAMT & ","
                    strIQry &= "" & rs!GRNDTOT & ","
                    strIQry &= "" & rs!TOTCGST & ","
                    strIQry &= "" & rs!TOTSGST & ","
                    strIQry &= "" & rs!TOTIGST & ","
                    strIQry &= "'" & rs!BILLSYSTEM & "',"
                    strIQry &= "'" & rs!TEST & "')"

                    Call clsSVT.execSQLStatement(strIQry, svtTrn, webConn)
                Loop
                rs.Close()
            End If

            '===============================================
            'TrnDtlPI
            '===============================================

            strSQL = " select PIID,PIDtlID,SlNo,ITEMCode,ItemName,d.UOMID,ItemSlNO,ShortClosed,ShortClosedBy,"
            strSQL &= " ShortClosedDT,ShortClosedQty,StkQty,PIQty,PIRate,PIAmt,NOOFCASES,"
            strSQL &= " NOOFBOTTLES,d.CGSTPER,CGSTAMT,d.SGSTPER,SGSTAMT,IGSTPER,IGSTAMT,NETAMT,d.MRP from TrnDtlPI D, MstItem M where D.ItemID=M.ItemID "
            rs = clsSVT.getDataReaderFromSQL(strSQL)
            If rs.HasRows Then
                Do While rs.Read
                    strIQry = " Insert into TRNDtlPI (ClientID,PIID,PIDtlID,SlNo,ItemCode,ITEMName,UOMID,ItemSlNO,ShortClosed,ShortClosedBy,"
                    strIQry &= " ShortClosedDT,ShortClosedQty,StkQty,PIQty,PIRate,PIAmt,NOOFCASES,"
                    strIQry &= " NOOFBOTTLES,CGSTPER,CGSTAMT,SGSTPER,SGSTAMT,IGSTPER,IGSTAMT,NETAMT,MRP)"
                    strIQry &= " values ("
                    strIQry &= intWebClientID & ","
                    strIQry &= rs!PIID & ","
                    strIQry &= rs!PIDtlID & ","
                    strIQry &= "" & rs!SlNo & ","
                    strIQry &= "'" & rs!ItemCode & "',"
                    strIQry &= "'" & rs!ITEMName & "',"
                    strIQry &= "" & rs!UOMID & ","
                    strIQry &= "" & rs!ItemSlNO & ","
                    strIQry &= "'" & rs!ShortClosed & "',"
                    strIQry &= "'" & rs!ShortClosedBy & "',"
                    If IsDate(rs!ShortClosedDT) Then
                        strIQry &= "'" & getDateFormatInInsert(rs!ShortClosedDT) & "',"
                    Else
                        strIQry &= " Null,"
                    End If
                    strIQry &= "" & GetVal(rs!ShortClosedQty, 0) & ","
                    strIQry &= "" & rs!StkQty & ","
                    strIQry &= "" & rs!PIQty & ","
                    strIQry &= "" & rs!PIRate & ","
                    strIQry &= "" & rs!PIAmt & ","
                    strIQry &= "" & rs!NOOFCASES & ","
                    strIQry &= "" & rs!NOOFBOTTLES & ","
                    strIQry &= "" & rs!CGSTPER & ","
                    strIQry &= "" & rs!CGSTAMT & ","
                    strIQry &= "" & rs!SGSTPER & ","
                    strIQry &= "" & rs!SGSTAMT & ","
                    strIQry &= "" & rs!IGSTPER & ","
                    strIQry &= "" & rs!IGSTAMT & ","
                    strIQry &= "" & rs!NETAMT & ","
                    strIQry &= "" & rs!MRP & ")"

                    Call clsSVT.execSQLStatement(strIQry, svtTrn, webConn)
                Loop
                rs.Close()
            End If

            svtTrn.Commit()

            webConn.Close()
        Catch ex As Exception
            If argShowMsg Then
                MsgBox("Error connecting to web." & ex.Message, MsgBoxStyle.Critical, "ERROR")
            End If
        End Try
    End Sub

    Private Sub deleteForTodaysData()
        clsSVT.execSQLStatement("delete from RptDSR_Gst	where ClientID = " & intWebClientID, svtTrn, webConn)
        clsSVT.execSQLStatement(" delete from RptDSRCtrSales where  ClientID = " & intWebClientID, svtTrn, webConn)
        clsSVT.execSQLStatement(" delete from RptDSRExpLedWise where  ClientID = " & intWebClientID, svtTrn, webConn)
        clsSVT.execSQLStatement(" delete from RptDSRGroupWisePur where  ClientID = " & intWebClientID, svtTrn, webConn)
        clsSVT.execSQLStatement(" delete from RptDSRGroupWiseSales where  ClientID = " & intWebClientID, svtTrn, webConn)
        clsSVT.execSQLStatement(" delete from RptDSRInvCancelled where  ClientID = " & intWebClientID, svtTrn, webConn)
        clsSVT.execSQLStatement(" delete from RptDSRPmt where  ClientID = " & intWebClientID, svtTrn, webConn)
        clsSVT.execSQLStatement(" delete from RptDSRSmry where  ClientID = " & intWebClientID, svtTrn, webConn)
        clsSVT.execSQLStatement(" delete from RptDSRSPWise where  ClientID = " & intWebClientID, svtTrn, webConn)
        clsSVT.execSQLStatement(" delete from RptFastMov where  ClientID = " & intWebClientID, svtTrn, webConn)
        clsSVT.execSQLStatement(" delete from RptMinStk where  ClientID = " & intWebClientID, svtTrn, webConn)
        clsSVT.execSQLStatement(" delete from trnhdrpi where  ClientID = " & intWebClientID, svtTrn, webConn)
        clsSVT.execSQLStatement(" delete from trnDtlpi where  ClientID = " & intWebClientID, svtTrn, webConn)
    End Sub
    Private Function pushData(argDate As Date, argShwMsg As Boolean) As Boolean
        Dim strSQL As String, strIQry As String
        Dim rs As OleDbDataReader, intCtr As Integer
        Dim dtDate As Date, dblTotInvvalue As Double, dblRndOff As Double, dblItemTot As Double
        dtDate = argDate
        dtStart = getFirstDateInMonth(dtDate)
        pushData = False
        Try
            '===============================================
            'RptDSRSmry 
            '===============================================
            strIQry = "Insert into RptDSRSmry (ClientID,	DocDate,	TotBills,	TotSalesAmt,TotRndOffAmt,	"
            strIQry &= " SalesAmtLiq,	SalesAmtNonLiq,	SalesAmtCtrWise,	SalesProfitAmt,	"
            strIQry &= " OPStkQty,	OPStkValue,	ClStkQty,	CLStkValue,	TotCashAmt,	TotCardAmt,	OtherAmt,"
            strIQry &= " CreditAmt,	WriteOffAmt,	TotPurAmt_SalesRate,	PurAmtBasic,	PurAmtInclGST,	"
            strIQry &= " TotSalesRetnAmt,	CashOPAmt,	ExpenseAmt,	CashBalanceAmt,	RedemAmount,	"
            strIQry &= " LastPurBillNo,	LastPurDate,	LastPurValue,	MnthPurValue,	YearPurValue,	"
            strIQry &= " DaySalesValue,	MnthSalesValue,	YearSalesValue) values ("
            strIQry &= intWebClientID & ","
            strIQry &= getDateFormatInCond(dtDate) & ","

            'TotBills
            strSQL = "Select Count(SINVNo) as TotBills"
            strSQL &= " from TrnHdrSINV "
            strSQL &= " Where SINVDate = " & getDateFormatInCond(dtDate) & " And Cancelled = 'N' AND IsDelete='N'"
            strIQry &= clsSVT.getDataDoubleValue(strSQL, 0, False) & ","

            'TotSalesAmt, ItemAmt, RndOff
            strSQL = "Select Sum(INVValue) as TotSales"
            strSQL &= " from TrnHdrSINV "
            strSQL &= " Where SINVDate = " & getDateFormatInCond(dtDate) & " And Cancelled = 'N' AND IsDelete='N'"
            dblTotInvvalue = clsSVT.getDataDoubleValue(strSQL, 0, False)
            strSQL = "Select sum(d.ITEMAMT) From mstitem i, trndtlsinv d, trnhdrsinv h"
            strSQL &= " Where h.sinvid = d.sinvid and d.itemid = i.itemid "
            strSQL &= " and SINVDate = " & getDateFormatInCond(dtDate) & " and IsDelete='N' and Cancelled = 'N'  "
            dblItemTot = clsSVT.getDataDoubleValue(strSQL, 0, False)
            dblRndOff = dblItemTot - dblTotInvvalue

            strIQry &= dblItemTot & ","
            strIQry &= dblRndOff & ","

            'SalesAmtLiq
            strSQL = "Select sum(d.ITEMAMT) From mstitem i, trndtlsinv d, trnhdrsinv h"
            strSQL &= " Where h.sinvid = d.sinvid and d.itemid = i.itemid And i.LiqType <> 'Others' "
            strSQL &= " and SINVDate = " & getDateFormatInCond(dtDate) & " and IsDelete='N' and Cancelled = 'N'  "
            strIQry &= clsSVT.getDataDoubleValue(strSQL, 0, False) & ","

            'SalesAmtNonLiq
            strSQL = "Select sum(d.ITEMAMT) From mstitem i, trndtlsinv d, trnhdrsinv h"
            strSQL &= " Where h.sinvid = d.sinvid and d.itemid = i.itemid And i.LiqType = 'Others' "
            strSQL &= " and SINVDate = " & getDateFormatInCond(dtDate) & " and IsDelete='N' and Cancelled = 'N'  "
            strIQry &= clsSVT.getDataDoubleValue(strSQL, 0, False) & ","

            'SalesAmtCtrWise
            strSQL = "Select  Sum(INVVALUE) as Amount From TrnHdrSINV h, MstSC s "
            strSQL &= " Where h.SCID=s.SCID and IsDelete='N' and Cancelled='N'"
            strSQL &= " and SINVDate =" & getDateFormatInCond(dtDate) & " "
            strIQry &= clsSVT.getDataDoubleValue(strSQL, 0, False) & ","

            'SalesProfitAmt
            Dim dblTotpurchase As Double
            strSQL = "select sum(i.PURRATE * sinvqty)  as Purchaserate  from trnhdrsinv h ,trndtlsinv d, mstitem i"
            strSQL &= " where h.sinvid=d.sinvid and d.itemid=i.ITEMID"
            strSQL &= " and SINVDate = " & getDateFormatInCond(dtDate) & " and IsDelete='N' and Cancelled = 'N'  "
            dblTotpurchase = clsSVT.getDataDoubleValue(strSQL, 0, False)

            Dim dblTotSales As Double
            strSQL = "select sum(sinvamt) as SaleAmt from trnhdrsinv h ,trndtlsinv d, mstitem i"
            strSQL &= " where h.sinvid=d.sinvid and d.itemid=i.ITEMID"
            strSQL &= " and SINVDate = " & getDateFormatInCond(dtDate) & " and IsDelete='N' and Cancelled = 'N'  "
            dblTotSales = clsSVT.getDataDoubleValue(strSQL, 0, False)

            strIQry &= dblTotSales - dblTotpurchase & ","

            'OPStkQty
            strSQL = "Select Sum(Qty * StkMultiplier) as OpQty from UtlStockList Where DocDate < " & getDateFormatInCond(dtDate)
            strIQry &= clsSVT.getDataDoubleValue(strSQL, 0, False) & ","

            'OPStkValue
            strSQL = "Select  Sum(Cast(Round(MRP * Qty * StkMultiplier,2) as Numeric(18,2)))  as OpBal from UtlStockList u, MstItem i Where u.ItemID=i.ItemID and DocDate < " & getDateFormatInCond(dtDate)
            strIQry &= clsSVT.getDataDoubleValue(strSQL, 0, False) & ","

            'ClStkQty
            strSQL = "Select Sum(Qty * StkMultiplier) as ClBal from UtlStockList Where DocDate <= " & getDateFormatInCond(dtDate)
            strIQry &= clsSVT.getDataDoubleValue(strSQL, 0, False) & ","

            'CLStkValue
            strSQL = "Select Sum(MRP * Qty * StkMultiplier)  as ClBal from UtlStockList u, MstItem i Where u.ItemID=i.ItemID and DocDate <= " & getDateFormatInCond(dtDate)
            strIQry &= clsSVT.getDataDoubleValue(strSQL, 0, False) & ","

            'TotCashAmt
            strSQL = "Select sum(CashAmt) as TotCashAmt"
            strSQL &= " from TrnHdrSINV "
            strSQL &= " Where SINVDate = " & getDateFormatInCond(dtDate) & " And Cancelled = 'N' AND IsDelete='N'"
            strIQry &= clsSVT.getDataDoubleValue(strSQL, 0, False) & ","

            'TotCardAmt
            strSQL = "Select Sum(CardAmt) as TotCardAmt"
            strSQL &= " from TrnHdrSINV "
            strSQL &= " Where SINVDate = " & getDateFormatInCond(dtDate) & " And Cancelled = 'N' AND IsDelete='N'"
            strIQry &= clsSVT.getDataDoubleValue(strSQL, 0, False) & ","

            'OtherAmt
            strSQL = "Select Sum(OthPayAmt) as TotCardAmt"
            strSQL &= " from TrnHdrSINV "
            strSQL &= " Where SINVDate = " & getDateFormatInCond(dtDate) & " And Cancelled = 'N' AND IsDelete='N'"
            strIQry &= clsSVT.getDataDoubleValue(strSQL, 0, False) & ","

            'CreditAmt
            If fnFldPresent("CRED", "TrnHdrSINV") Then
                strSQL = "Select Sum(CRED) as CreditAmt "
                strSQL &= " from TrnHdrSINV "
                strSQL &= " Where SINVDate = " & getDateFormatInCond(dtDate) & " And Cancelled = 'N' AND IsDelete='N'"
                strIQry &= clsSVT.getDataDoubleValue(strSQL, 0, False) & ","
            Else
                strIQry &= "0,"
            End If

            'WriteOffAmt
            strSQL = "Select Sum(WriteOffAmt) as WriteOffAmt"
            strSQL &= " From TrnHdrSINV"
            strSQL &= " Where SINVDate = " & getDateFormatInCond(dtDate) & " and IsDelete='N' and Cancelled = 'N'  "
            strIQry &= clsSVT.getDataDoubleValue(strSQL, 0, False) & ","

            'TotPurAmt_SalesRate
            strSQL = "Select Sum(d.MRP * d.PBQty) from TrnHdrPB h, TrnDtlPB d"
            strSQL &= " where h.PBID=d.PBID And Cancelled='N'"
            strSQL &= " And PBDate =" & getDateFormatInCond(dtDate)
            strIQry &= clsSVT.getDataDoubleValue(strSQL, 0, False) & ","

            'PurAmtBasic
            strSQL = "Select Sum(TOTAMT) from TrnHdrPB where Cancelled='N'"
            strSQL &= " And PBDate =" & getDateFormatInCond(dtDate)
            strIQry &= clsSVT.getDataDoubleValue(strSQL, 0, False) & ","

            'PurAmtInclGST
            strSQL = "Select Sum(GRNDTOT) from TrnHdrPB where Cancelled='N'"
            strSQL &= " And PBDate =" & getDateFormatInCond(dtDate)
            strIQry &= clsSVT.getDataDoubleValue(strSQL, 0, False) & ","

            'TotSalesRetnAmt
            If fnTablePresent("TrnHdrSR") Then
                strSQL = "Select Sum(TotAmt) as SalesReturn from TrnHdrSR where Cancelled='N'"
                strSQL &= " And SRDate =" & getDateFormatInCond(dtDate)
                strIQry &= clsSVT.getDataDoubleValue(strSQL, 0, False) & ","
            Else
                strIQry &= "0,"
            End If

            'CashOPAmt
            strSQL = "Select OPAmt from vwexpsmry where "
            strSQL &= "ExpDate =" & getDateFormatInCond(dtDate)
            strIQry &= clsSVT.getDataDoubleValue(strSQL, 0, False) & ","

            'ExpenseAmt
            strSQL = "Select ExpAmt from vwexpsmry where "
            strSQL &= "ExpDate =" & getDateFormatInCond(dtDate)
            strIQry &= clsSVT.getDataDoubleValue(strSQL, 0, False) & ","

            'CashBalanceAmt
            strSQL = "Select BalAmt from vwexpsmry where "
            strSQL &= "ExpDate =" & getDateFormatInCond(dtDate)
            strIQry &= clsSVT.getDataDoubleValue(strSQL, 0, False) & ","

            'RedemAmount
            If fnTablePresent("TRNHDRRDM") Then
                strSQL = "Select sum(CashAmt) from TrnHdrRdm where RdmDate = " & getDateFormatInCond(dtDate)
                strIQry &= clsSVT.getDataDoubleValue(strSQL, 0, False) & ","
            Else
                strIQry &= "0,"
            End If

            'LastPurBillNo
            strSQL = "select top 1 pbno from trnhdrpb where PBDate <= " & getDateFormatInCond(dtDate) & " order by pbdate desc, pbid desc"
            strIQry &= "'" & clsSVT.getDataStringValue(strSQL, "", False) & "',"

            'LastPurDate
            strSQL = "select top 1 convert(varchar,pbdate,106) from trnhdrpb where PBDate <= " & getDateFormatInCond(dtDate) & " order by pbdate desc, pbid desc"
            strIQry &= "'" & clsSVT.getDataStringValue(strSQL, "", False) & "',"

            'LastPurValue
            strSQL = "select top 1 GrndTot from trnhdrpb where PBDate <= " & getDateFormatInCond(dtDate) & " order by pbdate desc, pbid desc"
            strIQry &= clsSVT.getDataDoubleValue(strSQL, 0, False) & ","

            'MnthPurValue
            strSQL = "select sum(GrndTot) from trnhdrpb where pbdate between " & getDateFormatInCond(dtStart) & " and " & getDateFormatInCond(dtDate)
            strIQry &= clsSVT.getDataDoubleValue(strSQL, 0, False) & ","

            'YearPurValue
            strSQL = "select sum(GrndTot) from trnhdrpb where pbdate between " & getDateFormatInCond(gSettings.gCompStDt) & " and " & getDateFormatInCond(dtDate)
            strIQry &= clsSVT.getDataDoubleValue(strSQL, 0, False) & ","

            'DaySalesValue
            strSQL = "Select sum(d.ITEMAMT) From mstitem i, trndtlsinv d, trnhdrsinv h"
            strSQL &= " Where h.sinvid = d.sinvid and d.itemid = i.itemid "
            strSQL &= " and SINVDate = " & getDateFormatInCond(dtDate) & " and IsDelete='N' and Cancelled = 'N'  "
            strIQry &= clsSVT.getDataDoubleValue(strSQL, 0, False) & ","

            'MnthSalesValue
            strSQL = "select sum(InvValue) from trnhdrSinv where SinvDate between " & getDateFormatInCond(dtStart) & " and " & getDateFormatInCond(dtDate)
            strIQry &= clsSVT.getDataDoubleValue(strSQL, 0, False) & ","

            'YearSalesValue
            strSQL = "select sum(InvValue) from trnhdrSinv where SinvDate between " & getDateFormatInCond(gSettings.gCompStDt) & " and " & getDateFormatInCond(dtDate)
            strIQry &= clsSVT.getDataDoubleValue(strSQL, 0, False) & ")"

            If Not clsSVT.execSQLStatement(strIQry, svtTrn, webConn) Then
                Exit Function
            End If

            '===============================================
            'RptDSRCtrSales 
            '===============================================
            strSQL = "Select SCCODE, Count(SCCode) as NoOfBills, Sum(INVVALUE) as Amount From TrnHdrSINV h, MstSC s "
            strSQL &= " Where h.SCID=s.SCID and IsDelete='N' and Cancelled='N'"
            strSQL &= " and SINVDate =" & getDateFormatInCond(dtDate) & " Group by SCCode"
            rs = clsSVT.getDataReaderFromSQL(strSQL)
            intCtr = 0
            If rs.HasRows Then
                Do While rs.Read
                    intCtr = intCtr + 1
                    strIQry = "Insert into RptDSRCtrSales (ClientID,	DocDate,slno, CounterNo, NoOfBills, BillValue) values ("
                    strIQry &= intWebClientID & ","
                    strIQry &= getDateFormatInCond(dtDate) & ","
                    strIQry &= intCtr & ","
                    strIQry &= "'" & rs!SCCode & "',"
                    strIQry &= rs!NoOfBills & ","
                    strIQry &= rs!Amount & ")"
                    Call clsSVT.execSQLStatement(strIQry, svtTrn, webConn)
                Loop
                rs.Close()
            End If


            '===============================================
            'RptDSRInvCancelled 
            '===============================================
            strSQL = "Select SCCODE, Count(SCCode) as NoOfBills, Sum(INVVALUE) as Amount From TrnHdrSINV h, MstSC s "
            strSQL &= " Where h.SCID=s.SCID and Cancelled = 'Y'"
            strSQL &= " and SINVDate =" & getDateFormatInCond(dtDate) & " Group by SCCode"
            rs = clsSVT.getDataReaderFromSQL(strSQL)
            intCtr = 0
            If rs.HasRows Then
                Do While rs.Read
                    intCtr = intCtr + 1
                    strIQry = "Insert into RptDSRInvCancelled (ClientID,	DocDate,slno, CounterNo, NoOfBills, BillValue) values ("
                    strIQry &= intWebClientID & ","
                    strIQry &= getDateFormatInCond(dtDate) & ","
                    strIQry &= intCtr & ","
                    strIQry &= "'" & rs!SCCode & "',"
                    strIQry &= rs!NoOfBills & ","
                    strIQry &= rs!Amount & ")"
                    Call clsSVT.execSQLStatement(strIQry, svtTrn, webConn)
                Loop
                rs.Close()
            End If

            '===============================================
            'RptDSRSPWise 
            '===============================================
            strSQL = "Select SPCODE, Count(SPCode) as NoOfBills, Sum(INVVALUE) as Amount From TrnHdrSINV h, MstSP s "
            strSQL &= " where h.SPID=s.SPID and IsDelete='N' and Cancelled='N' "
            strSQL &= " and SINVDate =" & getDateFormatInCond(dtDate) & " Group by SPCode"
            rs = clsSVT.getDataReaderFromSQL(strSQL)
            intCtr = 0
            If rs.HasRows Then
                Do While rs.Read
                    intCtr = intCtr + 1
                    strIQry = "Insert into RptDSRSPWise (ClientID,	DocDate,slno, CounterNo, NoOfBills, BillValue) values ("
                    strIQry &= intWebClientID & ","
                    strIQry &= getDateFormatInCond(dtDate) & ","
                    strIQry &= intCtr & ","
                    strIQry &= "'" & rs!SPCode & "',"
                    strIQry &= rs!NoOfBills & ","
                    strIQry &= rs!Amount & ")"
                    Call clsSVT.execSQLStatement(strIQry, svtTrn, webConn)
                Loop
                rs.Close()
            End If

            '===============================================
            'RptDSRGroupWiseSales 
            '===============================================
            strSQL = "Select g.ItemGroupCode, Sum(convert(Int,d.SINVQty)) as Qty, Sum(Round(d.ItemAmt,2)) as Amount "
            strSQL &= " From TrnHdrSINV h, TrnDtlSINV d, MstItem i, MstItemGroup g "
            strSQL &= " Where h.SINVID=d.SINVID and d.ITEMID=i.ITEMID and i.ITEMGroupID=g.ITEMGROUPID "
            strSQL &= " and IsDelete='N' and Cancelled='N' and SINVDate = " & getDateFormatInCond(dtDate) & " Group by g.ItemGroupCode"
            rs = clsSVT.getDataReaderFromSQL(strSQL)
            intCtr = 0
            If rs.HasRows Then
                Do While rs.Read
                    intCtr = intCtr + 1
                    strIQry = "Insert into RptDSRGroupWiseSales (ClientID,	DocDate,slno, GroupName, TotQty, TotAmt) values ("
                    strIQry &= intWebClientID & ","
                    strIQry &= getDateFormatInCond(dtDate) & ","
                    strIQry &= intCtr & ","
                    strIQry &= "'" & rs!ItemGroupCode & "',"
                    strIQry &= rs!Qty & ","
                    strIQry &= rs!Amount & ")"
                    Call clsSVT.execSQLStatement(strIQry, svtTrn, webConn)
                Loop
                rs.Close()
            End If

            '===============================================
            'RptDSRGroupWisePur 
            '===============================================
            strSQL = "Select g.ItemGroupCode, Sum(convert(Int,d.PBQty)) as Qty,"
            strSQL &= "Sum(Cast(Round(i.MRP * d.PBQty,2) as Numeric(18,2))) as Amount"
            strSQL &= " From TrnHdrPB h, TrnDtlPB d, MstItem i, MstItemGroup g  "
            strSQL &= " Where h.PBID=d.PBID and d.ITEMID=i.ITEMID and i.ITEMGroupID=g.ITEMGROUPID "
            strSQL &= "and Cancelled='N' and PBDate = " & getDateFormatInCond(dtDate) & " Group by g.ItemGroupCode"
            rs = clsSVT.getDataReaderFromSQL(strSQL)
            intCtr = 0
            If rs.HasRows Then
                Do While rs.Read
                    intCtr = intCtr + 1
                    strIQry = "Insert into RptDSRGroupWisePur (ClientID,	DocDate,slno, GroupName, TotQty, TotAmt) values ("
                    strIQry &= intWebClientID & ","
                    strIQry &= getDateFormatInCond(dtDate) & ","
                    strIQry &= intCtr & ","
                    strIQry &= "'" & rs!ItemGroupCode & "',"
                    strIQry &= rs!Qty & ","
                    strIQry &= rs!Amount & ")"
                    Call clsSVT.execSQLStatement(strIQry, svtTrn, webConn)
                Loop
                rs.Close()
            End If

            '===============================================
            'RptDSRExpLedWise 
            '===============================================
            strSQL = "Select l.LedCode, Sum(d.Amt) as Amount From TrnHdrEXP h, TrnDtlExp d, MstLed l "
            strSQL &= " Where h.EXPID=d.EXPID and d.LEDID=l.LEDID  and Cancelled='N' "
            strSQL &= " and ExpDate = " & getDateFormatInCond(dtDate) & " Group by l.LedCode"

            rs = clsSVT.getDataReaderFromSQL(strSQL)
            intCtr = 0
            If rs.HasRows Then
                Do While rs.Read
                    intCtr = intCtr + 1
                    strIQry = "Insert into RptDSRExpLedWise (ClientID,	DocDate,slno, LedgerName, LedAmt) values ("
                    strIQry &= intWebClientID & ","
                    strIQry &= getDateFormatInCond(dtDate) & ","
                    strIQry &= intCtr & ","
                    strIQry &= "'" & rs!LedgerName & "',"
                    strIQry &= rs!LedAmt & ")"
                    Call clsSVT.execSQLStatement(strIQry, svtTrn, webConn)
                Loop
                rs.Close()
            End If


            '===============================================
            'RptDSR_Gst
            '===============================================
            strSQL = "select 'CGST @' + convert(varchar,cgstper) as GST, sum(cgstamt) as gstAmt "
            strSQL &= " from TrnDtlSINV d, trnhdrsinv h "
            strSQL &= " where d.sinvid = h.sinvid and h.Cancelled = 'N' and IsDelete = 'N'"
            strSQL &= " and sinvdate = " & getDateFormatInCond(dtDate)
            strSQL &= " group by cgstper"
            strSQL &= " having sum(cgstamt) > 0"
            strSQL &= " union all"
            strSQL &= " select 'SGST @' + convert(varchar,sgstper) as GST, sum(sgstamt) "
            strSQL &= " from TrnDtlSINV d, trnhdrsinv h "
            strSQL &= " where d.sinvid = h.sinvid and h.Cancelled = 'N' and IsDelete = 'N'"
            strSQL &= " and sinvdate = " & getDateFormatInCond(dtDate)
            strSQL &= " group by sgstper"
            strSQL &= " having sum(sgstamt) > 0"
            strSQL &= " order by 1"

            rs = clsSVT.getDataReaderFromSQL(strSQL)
            intCtr = 0
            If rs.HasRows Then
                Do While rs.Read
                    intCtr = intCtr + 1
                    strIQry = "Insert into RptDSR_Gst (ClientID,	DocDate,slno, GST, GSTAmt) values ("
                    strIQry &= intWebClientID & ","
                    strIQry &= getDateFormatInCond(dtDate) & ","
                    strIQry &= intCtr & ","
                    strIQry &= "'" & rs!GST & "',"
                    strIQry &= rs!GstAmt & ")"
                    Call clsSVT.execSQLStatement(strIQry, svtTrn, webConn)
                Loop
                rs.Close()
            End If

            '===============================================
            'RptFastMov
            '===============================================
            strSQL = "Select top 20 itemname, sum(sinvqty) as qty"
            strSQL &= " from TrnDtlSINV d, trnhdrsinv h , mstitem i"
            strSQL &= " where d.sinvid = h.sinvid and d.itemid = i.itemid and h.Cancelled = 'N' and IsDelete = 'N'"
            strSQL &= " and sinvdate between " & getDateFormatInCond(DateAdd(DateInterval.Day, -30, dtDate)) & " and " & getDateFormatInCond(dtDate)
            strSQL &= " Group by ITEMNAME"
            strSQL &= " Order by 2 desc"

            rs = clsSVT.getDataReaderFromSQL(strSQL)
            intCtr = 0
            If rs.HasRows Then
                Do While rs.Read
                    intCtr = intCtr + 1
                    strIQry = "Insert into RptFastMov (ClientID,	DocDate,ItemName, SoldQty) values ("
                    strIQry &= intWebClientID & ","
                    strIQry &= getDateFormatInCond(dtDate) & ","
                    strIQry &= "'" & rs!ItemName & "',"
                    strIQry &= rs!Qty & ")"
                    Call clsSVT.execSQLStatement(strIQry, svtTrn, webConn)
                Loop
                rs.Close()
            End If

            '===============================================
            'RptMinStk
            '===============================================
            strSQL = "Select top 20 itemname, clbal as QOH, minlevel, clbal - MINLEVEL as shortage"
            strSQL &= " From TrnQoh h , mstitem i"
            strSQL &= " where h.itemid = i.itemid and locid = 1"
            strSQL &= " and isnull(MINLEVEL,0) > 0"
            strSQL &= " and clbal - isnull(MINLEVEL,0) < 0"
            strSQL &= " Group by ITEMNAME, clbal, MINLEVEL"
            strSQL &= " Order by 4 desc"
            rs = clsSVT.getDataReaderFromSQL(strSQL)

            intCtr = 0
            If rs.HasRows Then
                Do While rs.Read
                    intCtr = intCtr + 1
                    strIQry = "Insert into RptMinStk (ClientID,	DocDate,ItemName, QOH, MinLEvel, ShortageQty) values ("
                    strIQry &= intWebClientID & ","
                    strIQry &= getDateFormatInCond(dtDate) & ","
                    strIQry &= "'" & rs!ItemName & "',"
                    strIQry &= rs!QOH & ","
                    strIQry &= rs!MinLevel & ","
                    strIQry &= rs!Shortage & ")"
                    Call clsSVT.execSQLStatement(strIQry, svtTrn, webConn)
                Loop
                rs.Close()
            End If

            '===============================================
            'RptDSRPmt
            '===============================================
            strSQL = "select Particulars, sum(amount) as Amt"
            strSQL &= " From trnmultiamt m, trnhdrsinv h"
            strSQL &= " Where DocID = 1216"
            strSQL &= " And m.trnid = h.SINVID"
            strSQL &= " And h.SINVDate = " & getDateFormatInCond(dtDate)
            strSQL &= " group by Particulars order by 1"
            rs = clsSVT.getDataReaderFromSQL(strSQL)

            intCtr = 0
            If rs.HasRows Then
                Do While rs.Read
                    intCtr = intCtr + 1
                    strIQry = "Insert into RptDSRPmt (ClientID,	DocDate,SlNo, Particulars,PmtAmt) values ("
                    strIQry &= intWebClientID & ","
                    strIQry &= getDateFormatInCond(dtDate) & ","
                    strIQry &= intCtr & ","
                    strIQry &= "'" & rs!Particulars & "',"
                    strIQry &= rs!Amt & ")"
                    Call clsSVT.execSQLStatement(strIQry, svtTrn, webConn)
                Loop
                rs.Close()
            End If

            strSQL = "Update MstClient set LastSyncDateTime = '" & getDateFormatInInsert(dtDateTime, True, True) & "'"
            strSQL &= " where ClientID = " & intWebClientID
            Call clsSVT.execSQLStatement(strSQL, svtTrn, webConn)




            pushData = True

        Catch ex As Exception
            pushData = False
            If argShwMsg Then
                MsgBox("Error : " & ex.Message, MsgBoxStyle.Critical, "Error in PushData")
            End If
        End Try
    End Function

End Module
