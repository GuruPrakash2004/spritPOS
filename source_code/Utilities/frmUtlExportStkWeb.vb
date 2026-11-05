
Imports System.IO
Imports C1.Win.C1FlexGrid
Imports System.Data.oledb

Public Class frmUtlExportStkWeb
    Dim webConn As New oledbConnection
    Dim Clssvt As New svtLibrary
    Dim strCmd As String
    Dim strValMethod As String
    Dim strItem As String
    Dim strItemList As String
    Public strList As String


    Dim strItemFldList As String 'Item fld selection
    Dim iCount As Integer
    Dim arrFlds
    Dim strGrpList As String
    Dim intBranchID As Integer
    Private Sub UpdateDSR()

        Dim strSQL As String
        StMsg("Updating DSR ")
        strSQL = "Delete from RptDSR "
        Clssvt.execSQLStatement(strSQL, Nothing, webConn)

        With vsfDSR
            Dim rs As oledb.oledbDataReader

            strSQL = "Select Count(SINVNo) as TotBills, Sum(INVValue) as TotSales, sum(CashAmt) as TotCashAmt,"
            strSQL &= " Sum(CardAmt) as TotCardAmt, Sum(OthPayAmt) as OtherAmt  "
            If gStrDBName = "VLQMSMaduloka" Then
                strSQL &= ",Sum(CRED) as CreditAmt "
            End If
            strSQL &= " from TrnHdrSINV "
            strSQL &= " Where SINVDate = " & getDateFormatInCond(dtTDt.Value) & " And Cancelled = 'N' AND IsDelete='N'"
            rs = Clssvt.getDataReaderFromSQL(strSQL)
            Dim dblLiqAmt As Double = 0
            Dim dblProfitAmt As Double = 0
            Dim dblTotpurchase As Double = 0
            Dim dblTotSales As Double = 0
            If rs.HasRows Then
                Do While rs.Read
                    Call addRow("Total Bills", GetVal(rs!TotBills, 0))
                    Call addRow("Total Sales Amount (Incl.GST)", GetVal(rs!TotSales, 0))

                    strSQL = "Select sum(d.ITEMAMT) From mstitem i, trndtlsinv d, trnhdrsinv h"
                    strSQL &= " Where h.sinvid = d.sinvid and d.itemid = i.itemid And i.LiqType <> 'Others' "
                    strSQL &= " and SINVDate = " & getDateFormatInCond(dtTDt.Value) & " and IsDelete='N' and Cancelled = 'N'  "
                    dblLiqAmt = Clssvt.getDataDoubleValue(strSQL, 0, False)
                    Call addRow("Sales Amount - Liquor (BasicAmt)", dblLiqAmt)

                    strSQL = "Select sum(d.ITEMAMT) From mstitem i, trndtlsinv d, trnhdrsinv h"
                    strSQL &= " Where h.sinvid = d.sinvid and d.itemid = i.itemid And i.LiqType = 'Others' "
                    strSQL &= " and SINVDate = " & getDateFormatInCond(dtTDt.Value) & " and IsDelete='N' and Cancelled = 'N'  "
                    dblLiqAmt = Clssvt.getDataDoubleValue(strSQL, 0, False)
                    Call addRow("Sales Amount - NonLiquor (BasicAmt)", dblLiqAmt)
                    If UCase(gStrDBName) = "VLQMSNDROPS" Then
                        strSQL = "select sum(i.PURRATE * sinvqty)  as Purchaserate  from trnhdrsinv h ,trndtlsinv d, mstitem i"
                        strSQL &= " where h.sinvid=d.sinvid and d.itemid=i.ITEMID"
                        strSQL &= " and SINVDate = " & getDateFormatInCond(dtTDt.Value) & " and IsDelete='N' and Cancelled = 'N'  "
                        dblTotpurchase = Clssvt.getDataDoubleValue(strSQL, 0, False)

                        strSQL = "select sum(sinvamt) as SaleAmt from trnhdrsinv h ,trndtlsinv d, mstitem i"
                        strSQL &= " where h.sinvid=d.sinvid and d.itemid=i.ITEMID"
                        strSQL &= " and SINVDate = " & getDateFormatInCond(dtTDt.Value) & " and IsDelete='N' and Cancelled = 'N'  "
                        dblTotSales = Clssvt.getDataDoubleValue(strSQL, 0, False)

                        dblProfitAmt = dblTotSales - dblTotpurchase
                        Call addRow("Sales Profit Of the Day", dblProfitAmt)
                    End If
                    Call addRow("Opening Stock Qty", Clssvt.getDataDoubleValue("Select Sum(Qty * StkMultiplier) as OpQty from UtlStockList Where DocDate < '" & getDateFormatInInsert(dtTDt.Value) & "'", 0, False))
                    Call addRow("Opening Stock Value", Clssvt.getDataDoubleValue("Select  Sum(Cast(Round(MRP * Qty * StkMultiplier,2) as Numeric(18,2)))  as OpBal from UtlStockList u, MstItem i Where u.ItemID=i.ItemID and DocDate <'" & getDateFormatInInsert(dtTDt.Value) & "'", 0, False))

                    Call addRow("Closing Stock [Stock UOM]", Clssvt.getDataDoubleValue("Select Sum(Qty * StkMultiplier) as ClBal from UtlStockList Where DocDate <= '" & getDateFormatInInsert(dtTDt.Value) & "'", 0, False))
                    Call addRow("Closing Stock Value", Clssvt.getDataDoubleValue("Select Sum(MRP * Qty * StkMultiplier)  as ClBal from UtlStockList u, MstItem i Where u.ItemID=i.ItemID and DocDate <='" & getDateFormatInInsert(dtTDt.Value) & "'", 0, False))

                    Call addRow("Cash Amount", GetVal(rs!TotCashAmt, 0))
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
            strSQL &= " Where SINVDate = " & getDateFormatInCond(dtTDt.Value) & " and IsDelete='N' and Cancelled = 'N'  "
            dblWriteOffAmt = Clssvt.getDataDoubleValue(strSQL, 0, False)
            If dblWriteOffAmt > 0 Then Call addRow("Write Off Amount", dblWriteOffAmt)


            Call addRow("Total Purchase Amount", "")
            strSQL = "Select Sum(i.MRP * d.PBQty) As Purchase from TrnHdrPB h, TrnDtlPB d, MstItem i where h.PBID=d.PBID And d.ItemID=i.ItemID And Cancelled='N'"
            strSQL &= " And PBDate =" & getDateFormatInCond(dtTDt.Value)
            .SetData(.Rows.Count - 1, 1, Clssvt.getDataDoubleValue(strSQL, 0, False))

            If fnTablePresent("TrnHdrSR") Then
                Call addRow("Total Sales Return Amount", "")
                strSQL = "Select Sum(TotAmt) as SalesReturn from TrnHdrSR where Cancelled='N'"
                strSQL &= " And SRDate =" & getDateFormatInCond(dtTDt.Value)
                .SetData(.Rows.Count - 1, 1, Clssvt.getDataDoubleValue(strSQL, 0, False))
            End If
        End With





        For i = 0 To vsfDSR.Rows.Count - 1

            strSQL = "Insert into RptDSR (Heading, Value) "
            strSQL = strSQL & " values ("
            strSQL = strSQL & "'" & GetVal(vsfDSR.GetData(i, 0), "") & "',"
            strSQL = strSQL & GetVal(vsfDSR.GetData(i, 1), 0) & ")"
            Clssvt.execSQLStatement(strSQL, Nothing, webConn)
        Next
        StMsg("")
    End Sub

    Private Sub addRow(argCaption As String, argValue As String, Optional argValue2 As String = "", Optional blnHighlight As Boolean = False, Optional blnSubHeding As Boolean = False)
        With vsfDSR
            .Rows.Count = .Rows.Count + 1
            .SetData(.Rows.Count - 1, 0, argCaption)
            .SetData(.Rows.Count - 1, 1, argValue)
            '.SetData(.Rows.Count - 1, 2, argValue2)

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

    Private Sub UpdateDtl()
        Dim rsDumy As oledb.oledbDataReader
        Dim strSQL As String
        strSQL = "Delete from TrnDtls where BranchID = " & intBranchID
        strSQL = strSQL & " and DocDate between " & getDateFormatInCond(dtFdt.Value)
        strSQL = strSQL & " and " & getDateFormatInCond(dtFdt.Value)
        Clssvt.execSQLStatement(strSQL, Nothing, webConn)

        strSQL = " Select ItemName, Itemgroupname, PBDate as docDate, 'PB' as docType,"
        strSQL = strSQL & " Sum (PBQty) as docQty, Sum(PBAmt) as docAmt, PBRate as docRate"
        strSQL = strSQL & " from trnhdrpb h, trndtlpb d, mstitem i, mstitemgroup g"
        strSQL = strSQL & " Where h.pbID = d.pbID"
        strSQL = strSQL & " and d.itemid = i.itemid"
        strSQL = strSQL & " and i.itemgroupid = g.itemgroupid"
        strSQL = strSQL & " and PBDate between " & getDateFormatInCond(dtFdt.Value)
        strSQL = strSQL & " and " & getDateFormatInCond(dtTDt.Value)
        strSQL = strSQL & " group by itemName, ItemGroupName, PBDate, PBRate"
        strSQL = strSQL & " union all"
        strSQL = strSQL & " Select ItemName, Itemgroupname, sInvDate, 'Inv',"


        If UCase(gStrDBName) = "VLQMSSAMDROPSRR" Then

            strSQL = strSQL & " Sum (sInvQty), Sum(ITEMAMT), sInvRate"
        Else
            strSQL = strSQL & " Sum (sInvQty), Sum(sInvAmt), sInvRate"

        End If

        strSQL = strSQL & " from trnhdrsInv h, trndtlsInv d, mstitem i, mstitemgroup g"
        strSQL = strSQL & " Where h.sInvID = d.sInvID"
        strSQL = strSQL & " and d.itemid = i.itemid"
        strSQL = strSQL & " and i.itemgroupid = g.itemgroupid"
        strSQL = strSQL & " and sInvDate between " & getDateFormatInCond(dtFdt.Value)
        strSQL = strSQL & " and " & getDateFormatInCond(dtTDt.Value)
        strSQL = strSQL & " group by itemName, ItemGroupName, sInvDate, sInvRate"

        rsDumy = Clssvt.getDataReaderFromSQL(strSQL)
        With rsDumy
            Do While .Read
                StMsg("Updating transaction " & !DocType & " for Item " & !ItemName)
                strSQL = "Insert into TrnDtls (BranchID, Itemname, ItemGroupName, DocDate, DocType, "
                strSQL = strSQL & " DocQty, DocAmt, Docrate) values ("
                strSQL = strSQL & intBranchID & ","
                strSQL = strSQL & "'" & !ItemName & "',"
                strSQL = strSQL & "'" & !ItemGroupName & "',"
                strSQL = strSQL & "'" & getDateFormatInInsert(!DocDate) & "',"
                strSQL = strSQL & "'" & !DocType & "',"
                strSQL = strSQL & !DocQty & ","
                strSQL = strSQL & !DocAmt & ","
                strSQL = strSQL & !DocRate & ")"

                Clssvt.execSQLStatement(strSQL, Nothing, webConn)
            Loop
            .Close()
        End With
        StMsg("")
    End Sub

    Private Sub retreiveData()
        Dim strSQL As String
        Dim rsDumy As oledb.oledbDataReader
        Dim cmd As oledbCommand = New oledbCommand("msp_GenWebStock")
        With cmd
            cmd.Connection = svtConn
            .CommandType = CommandType.StoredProcedure

            .Parameters.Add("@mDate", OleDbType.DBDate)
            .Parameters(0).Value = dtFdt.Value
            .Parameters.Add("@mBranchID", OleDbType.integer)
            .Parameters(1).Value = intBranchID
            .CommandTimeout = 0
            .ExecuteNonQuery()
        End With
        cmd.Dispose() 'CNUD

        strSQL = "Select * from rptwebstk"
        rsDumy = Clssvt.getDataReaderFromSQL(strSQL)

        strSQL = "Delete from rptWebStk where BranchID = " & intBranchID
        Clssvt.execSQLStatement(strSQL, argBranchConn:=webConn)

        With rsDumy
            Do While .Read
                StMsg("Exporting " & dtFdt.Value & " - " & !ItemName)

                lblStatus.Text = "Exporting " & dtFdt.Value & " - " & !ItemName

                strSQL = "Insert into rptwebstk (StkDate, BranchID, ItemName, ItemGroupName, "
                strSQL = strSQL & " OPQty, OPAmt, RcptQty,RcptAmt, IssQty, IssAmt, ClQty, ClAmt) values ("
                strSQL = strSQL & "'" & getDateFormatInInsert(dtFdt.Value) & "',"
                strSQL = strSQL & !BranchID & ","
                strSQL = strSQL & "'" & !ItemName & "',"
                strSQL = strSQL & "'" & !ItemGroupName & "',"
                strSQL = strSQL & !OPQty & ","
                strSQL = strSQL & !OPAmt & ","
                strSQL = strSQL & !RcptQty & ","
                strSQL = strSQL & !RcptAmt & ","
                strSQL = strSQL & !IssQty & ","
                strSQL = strSQL & !IssAmt & ","
                strSQL = strSQL & !ClQty & ","
                strSQL = strSQL & !ClAmt & ")"

                Clssvt.execSQLStatement(strSQL, argBranchConn:=webConn)

            Loop
            .Close()
        End With
    End Sub

    Private Sub txtItemcodeLike_KeyPress(KeyAscii As Integer)
        If KeyAscii = Asc("'") Then KeyAscii = Asc("`")
    End Sub

    Private Sub txtItemNameLike_KeyPress(KeyAscii As Integer)
        If KeyAscii = Asc("'") Then KeyAscii = Asc("`")
    End Sub


    Private Sub initINIGrid()
        With vsfINI
            .Cols.Fixed = 0
            .Cols.Count = 3
            .Rows.Count = 0
            .ClipSeparators = ";" & vbCr
            If File.Exists(Application.StartupPath & "\" & gINIFileName) Then
                .LoadGrid(Application.StartupPath & "\" & gINIFileName, FileFormatEnum.TextCustom)
            End If
        End With
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        frmMain.CloseSelectedTabPage(Me)
    End Sub

    Private Sub btnClose_LostFocus(sender As Object, e As EventArgs) Handles btnClose.LostFocus
        dtFdt.Focus()
    End Sub

    Private Sub frmUtlExportStkWeb_Load(sender As Object, e As EventArgs) Handles Me.Load
        'dtFdt.Value = getSystemDate()
        ' dtFdt.Value = DateAdd("D", -1, dtFdt.Value)
        dtTDt.Value = getSystemDate()
        lblStatus.Text = ""
        If Not fnFldPresent("BranchID", "UtlMisc") Then
            Clssvt.execSQLStatement("Alter Table UtlMisc add BranchID int not null default 0")
        End If

        intBranchID = Clssvt.getDataLongValue("select BranchID from UtlMisc", 0, False)

        If gblnAuto Then
            btnExportToXL.PerformClick()
            gblnAuto = False
        End If
        With vsfDSR
            .Rows.Count = 0
            .Rows.Fixed = 0
            .Cols.Count = 2
            .Cols.Fixed = 0
            .Cols(0).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.LeftCenter
            .Cols(1).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter
        End With


    End Sub

    Private Sub btnExportToXL_Click(sender As Object, e As EventArgs) Handles btnExportToXL.Click
        Dim ConStr As String
        Dim strDBName As String = gStrDBName
        If chkStock.Checked = False Then
            MsgBox("Please Check Export Stock Data to WEB To Export Data To Cloud (203)", vbInformation, Me.Text)
            chkStock.Focus()
            Exit Sub
        End If

        If UCase(gStrDBName.Substring(0, 5)) <> "VLQMS" Then
            MsgBox("Local DB Name Should Consists  VLQMS  for Ex: VLQMSSVT " & vbCrLf & "Change Db Name And Try Again ", vbInformation, Me.Text)
            Exit Sub
        End If


        strDBName = Replace(UCase(strDBName), "VLQMS", "")



        ConStr = "Provider=SQLOLEDB.1;Persist Security Info=False"
        ConStr = ConStr & ";User ID=sa"
        ConStr = ConStr & ";Pwd=svt@May$$_128"
        ConStr = ConStr & ";Initial Catalog=eLQMS" & strDBName
        ConStr = ConStr & ";Data Source=47.23.106.203"

        lblStatus.Text = "Exporting Data To Cloud (203)"
        Try

            webConn.ConnectionString = ConStr
            webConn.Open()
            showWaitCursor()

            If chkStock.Checked = True Then
                Call retreiveData()
            End If
            Call UpdateDtl()
            Call UpdateDSR()
            showNormalCursor()

            MsgBox("Data Exported To Cloud (203) Successfully. " & vbCrLf & "Now, You Can Check Mob\Tablet\System Using Cloud Link ", vbInformation, Me.Text)

            lblStatus.Text = ""
            webConn.Close
            Me.Close()
        Catch ex As Exception
            MsgBox("Error While Exporting Data To Cloud (203)", vbCritical, Me.Text)
        End Try

        If gblnAuto = True Then
            Me.Close()
        End If
    End Sub

    Private Sub dtTDt_ValueChanged(sender As Object, e As EventArgs) Handles dtTDt.ValueChanged
        dtFdt.Value = dtTDt.Value
    End Sub

    Private Sub frmUtlExportStkWeb_Resize(sender As Object, e As EventArgs) Handles Me.Resize

        Panel3.Top = PictureBox1.Height / 2
    End Sub
End Class