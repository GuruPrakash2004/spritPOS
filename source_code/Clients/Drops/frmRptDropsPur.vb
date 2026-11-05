Imports System.Data.oledb

Public Class frmRptDropsPur
    Dim clsSvt As New svtLibrary
    Const vDetGrp = 0
    Const vDetSupp = 1
    Const vDetBillNo = 2
    Const vDetAmt = 3
    Private Sub btnView_Click(sender As Object, e As EventArgs) Handles btnView.Click
        Call LoadData()
    End Sub

    Private Sub frmRptDropsDSR_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim strHeading As String
        strHeading = "Purchase Report For " & Replace(getDateFormatInCond(dtpFromDate.Value), "'", "")
        Call initVsfForHdrs(strHeading, "")
        Call LoadData()
    End Sub
    Private Sub initVsfForHdrs(ByVal argHdrLine1 As String, ByVal argHdrLine2 As String)
        Dim i As Integer

        With vsfDet
            .Cols.Count = 4
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
            .SetData(3, vDetSupp, "Supplier")
            .SetData(3, vDetBillNo, "BillNo")
        End With
    End Sub
    Private Sub LoadData()
        Dim strSQL As String = ""
        'strSQL = "Select 1 as LOrder, 'LIQUOR' as itemGroup, suppname, h.SUPPINVNO, h.suppinvdate, sum(PBAmt) as SAmt"
        'strSQL &= " from trndtlPB d, trnhdrPB h, mstitem i, mstitemgroup g, mstsupp s"
        'strSQL &= " where d.PBid = h.PBid"
        'strSQL &= " and d.itemid = i.itemid"
        'strSQL &= " and h.suppid = s.suppid"
        'strSQL &= " and i.ITEMGroupID = g.itemgroupid"
        'strSQL &= " And PBdate = " & getDateFormatInCond(dtpFromDate.Value)
        'strSQL &= " and G.LiqITEM = 'Y'"
        'strSQL &= " group by suppname, h.SUPPINVNO, h.suppinvdate"
        'strSQL &= " UNION ALL"
        'strSQL &= " Select 2 as LOrder, itemgroupname, suppname, h.SUPPINVNO, h.suppinvdate, sum(PBAmt) as SAmt"
        'strSQL &= " from trndtlPB d, trnhdrPB h, mstitem i, mstitemgroup g, mstsupp s"
        'strSQL &= " where d.PBid = h.PBid"
        'strSQL &= " and d.itemid = i.itemid"
        'strSQL &= " and h.suppid = s.suppid"
        'strSQL &= " and i.ITEMGroupID = g.itemgroupid"
        'strSQL &= " And PBdate = " & getDateFormatInCond(dtpFromDate.Value)
        'strSQL &= " And G.LiqITEM <> 'Y'"
        'strSQL &= " group by itemgroupname,suppname, h.SUPPINVNO, h.suppinvdate"
        'strSQL &= " order by 1,2"

        strSQL = "Select (Select top 1 case when LiqITEM='Y' then 'LIQUOR' else 'NON LIQUOR' end from trndtlPB d,mstitem i, mstitemgroup g where d.itemid = i.itemid and i.ITEMGroupID = g.itemgroupid and d.PBID=h.PBID) as itemGroup, suppname, h.SUPPINVNO, h.suppinvdate, sum(GRNDTOT) as SAmt"
        strSQL &= " from trnhdrPB h, mstsupp s"
        strSQL &= " where h.suppid = s.suppid"
        strSQL &= " And PBdate = " & getDateFormatInCond(dtpFromDate.Value)
        strSQL &= " group by h.PBID,suppname, h.SUPPINVNO, h.suppinvdate"
        strSQL &= " order by 1,2"

        Dim rs As oledbDataReader = clsSvt.getDataReaderFromSQL(strSQL)
        vsfDet.Rows.Count = 4
        Do While rs.Read()
            vsfDet.Rows.Count = vsfDet.Rows.Count + 1
            vsfDet.SetData(vsfDet.Rows.Count - 1, vDetGrp, rs!ItemGroup)
            vsfDet.SetData(vsfDet.Rows.Count - 1, vDetSupp, rs!SuppName)
            vsfDet.SetData(vsfDet.Rows.Count - 1, vDetBillNo, rs!SuppinvNo & "-" & rs!SuppInvDate)
            vsfDet.SetData(vsfDet.Rows.Count - 1, vDetAmt, rs!SAmt)
        Loop
        rs.Close()


        'Add Total PB value
        vsfDet.Rows.Count = vsfDet.Rows.Count + 1
        vsfDet.SetData(vsfDet.Rows.Count - 1, vDetGrp, "Total Purchase")
        strSQL = "Select sum(GrndTot) from TrnHdrPB "
        strSQL &= " where PBdate = " & getDateFormatInCond(dtpFromDate.Value)
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