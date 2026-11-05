Imports System.Data.oledb

Public Class frmRptDropsDSR
    Dim clsSvt As New svtLibrary
    Const vDetGrp = 0
    Const vDetQty = 1
    Const vDetAmt = 2
    Private Sub btnView_Click(sender As Object, e As EventArgs) Handles btnView.Click
        Call LoadData()
    End Sub

    Private Sub frmRptDropsDSR_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim strHeading As String
        strHeading = "Sales Summary Report For " & Replace(getDateFormatInCond(dtpFromDate.Value), "'", "")
        Call initVsfForHdrs(strHeading, "")
        Call LoadData()
    End Sub
    Private Sub initVsfForHdrs(ByVal argHdrLine1 As String, ByVal argHdrLine2 As String)
        Dim i As Integer

        With vsfDet
            .Cols.Count = 3
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
            '.Rows(1).Visible = False
            .SetData(3, vDetGrp, "Group")
            .SetData(3, vDetAmt, "Amount")
            .SetData(3, vDetQty, "Qty")

        End With
    End Sub
    Private Sub LoadData()
        Dim strSQL As String = ""
        strSQL = "Select 1 as lOrder,'LIQUOR' as ItemGroup, sum(sinvqty) as sQty, sum(SINVAmt) as SAmt"
        strSQL &= " from trndtlsinv d, trnhdrsinv h, mstitem i, mstitemgroup g"
        strSQL &= " where d.sinvid = h.sinvid"
        strSQL &= " and d.itemid = i.itemid"
        strSQL &= " and i.ITEMGroupID = g.itemgroupid"
        strSQL &= " And sinvdate = " & getDateFormatInCond(dtpFromDate.Value)
        strSQL &= " and G.LiqITEM = 'Y'"
        strSQL &= " UNION ALL"
        strSQL &= " Select 2 as lorder,itemgroupname, sum(sinvqty) as sQty, sum(SINVAmt) as SAmt"
        strSQL &= " from trndtlsinv d, trnhdrsinv h, mstitem i, mstitemgroup g"
        strSQL &= " where d.sinvid = h.sinvid"
        strSQL &= " and d.itemid = i.itemid"
        strSQL &= " and i.ITEMGroupID = g.itemgroupid"
        strSQL &= " And sinvdate = " & getDateFormatInCond(dtpFromDate.Value)
        strSQL &= " and G.LiqITEM <> 'Y'"
        strSQL &= " group by itemgroupname"
        strSQL &= " order by 1,2"

        Dim rs As oledbDataReader = clsSvt.getDataReaderFromSQL(strSQL)
        vsfDet.Rows.Count = 4
        Do While rs.Read()
            vsfDet.Rows.Count = vsfDet.Rows.Count + 1
            vsfDet.SetData(vsfDet.Rows.Count - 1, vDetGrp, rs!ItemGroup)
            vsfDet.SetData(vsfDet.Rows.Count - 1, vDetQty, rs!SQty)
            vsfDet.SetData(vsfDet.Rows.Count - 1, vDetAmt, rs!SAmt)
        Loop
        rs.Close()

        'Add RoundOffAmt
        vsfDet.Rows.Count = vsfDet.Rows.Count + 1
        vsfDet.SetData(vsfDet.Rows.Count - 1, vDetGrp, "Round Off")
        strSQL = "select sum(INVVALUE - tot) from TrnHdrSINV "
        strSQL &= " where Sinvdate = " & getDateFormatInCond(dtpFromDate.Value)
        vsfDet.SetData(vsfDet.Rows.Count - 1, vDetAmt, clsSvt.getDataDoubleValue(strSQL, 0, False))

        'Add Cash Payments
        vsfDet.Rows.Count = vsfDet.Rows.Count + 1
        vsfDet.SetData(vsfDet.Rows.Count - 1, vDetGrp, "Payment Mode: Cash")
        strSQL = "Select sum(Cashamt) from TrnHdrSINV "
        strSQL &= " where Sinvdate = " & getDateFormatInCond(dtpFromDate.Value)
        vsfDet.SetData(vsfDet.Rows.Count - 1, vDetAmt, clsSvt.getDataDoubleValue(strSQL, 0, False))

        'Add Card Payments
        vsfDet.Rows.Count = vsfDet.Rows.Count + 1
        vsfDet.SetData(vsfDet.Rows.Count - 1, vDetGrp, "Card")
        strSQL = "Select sum(Cardamt) from TrnHdrSINV "
        strSQL &= " where Sinvdate = " & getDateFormatInCond(dtpFromDate.Value)
        vsfDet.SetData(vsfDet.Rows.Count - 1, vDetAmt, clsSvt.getDataDoubleValue(strSQL, 0, False))

        'Add other payment modes
        strSQL = "Select Particulars, sum(Amount) as sAmt"
        strSQL &= " From TrnMultiAmt h, trnhdrsinv i"
        strSQL &= " Where h.TrnID = i.SINVID"
        strSQL &= " and Sinvdate = " & getDateFormatInCond(dtpFromDate.Value)
        strSQL &= " Group By Particulars"
        rs = clsSvt.getDataReaderFromSQL(strSQL)
        Do While rs.Read()
            vsfDet.Rows.Count = vsfDet.Rows.Count + 1
            vsfDet.SetData(vsfDet.Rows.Count - 1, vDetGrp, rs!Particulars)
            vsfDet.SetData(vsfDet.Rows.Count - 1, vDetAmt, rs!SAmt)
        Loop
        rs.Close()

        'Add Total Sales value
        vsfDet.Rows.Count = vsfDet.Rows.Count + 1
        vsfDet.SetData(vsfDet.Rows.Count - 1, vDetGrp, "Total Sales")
        strSQL = "Select sum(InvValue) from TrnHdrSINV "
        strSQL &= " where Sinvdate = " & getDateFormatInCond(dtpFromDate.Value)
        vsfDet.SetData(vsfDet.Rows.Count - 1, vDetAmt, clsSvt.getDataDoubleValue(strSQL, 0, False))

        vsfDet.Cols(vDetAmt).Format = "0.00"

        vsfDet.AutoSizeCols()
        vsfDet.ExtendLastCol = False
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        frmMain.CloseSelectedTabPage(Me)
    End Sub

    Private Sub btnExcel_Click(sender As Object, e As EventArgs) Handles btnExcel.Click
        ExportToXL(vsfDet)
    End Sub
End Class