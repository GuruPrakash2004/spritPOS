Imports System.Data.oledb

Public Class frmRptDropsPettyCash
    Dim clsSvt As New svtLibrary
    Const vDetParticulars = 0
    Const vDetExpAmt = 1
    Const vDetRAmt = 2
    Private Sub btnView_Click(sender As Object, e As EventArgs) Handles btnView.Click
        Call LoadData()
    End Sub

    Private Sub frmRptDropsDSR_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim strHeading As String
        strHeading = "Petty Cash Report For " & Replace(getDateFormatInCond(dtpFromDate.Value), "'", "")
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
            .SetData(3, vDetParticulars, "Particulars")
            .SetData(3, vDetRAmt, "Reciept")
            .SetData(3, vDetExpAmt, "Expense")

        End With
    End Sub
    Private Sub LoadData()
        Dim strSQL As String = ""
        Dim rs As oledbDataReader
        vsfDet.Rows.Count = 4

        'Add Opbalance
        vsfDet.Rows.Count = vsfDet.Rows.Count + 1
        vsfDet.SetData(vsfDet.Rows.Count - 1, vDetParticulars, "OP Balance")
        strSQL = "Select sum(ramt-amt) as opBal "
        strSQL &= " From trnhdrexp h, trndtlexp d"
        strSQL &= " where h.expid = d.expid"
        strSQL &= " and h.EXPDate < " & getDateFormatInCond(dtpFromDate.Value)
        vsfDet.SetData(vsfDet.Rows.Count - 1, vDetRAmt, clsSvt.getDataDoubleValue(strSQL, 0, False))

        'Add Rcpts and exp for the day
        strSQL = "Select ledname, sum(AMT) as eAmt, sum(RAMT) as rcptAmt"
        strSQL &= " From trnhdrexp h, trndtlexp d, mstled l"
        strSQL &= " where h.expid = d.expid"
        strSQL &= " and d.ledid = l.ledid"
        strSQL &= " and expdate = " & getDateFormatInCond(dtpFromDate.Value)
        strSQL &= " group by ledname"
        strSQL &= " order by 1"
        rs = clsSvt.getDataReaderFromSQL(strSQL)
        Do While rs.Read()
            vsfDet.Rows.Count = vsfDet.Rows.Count + 1
            vsfDet.SetData(vsfDet.Rows.Count - 1, vDetParticulars, rs!LedName)
            vsfDet.SetData(vsfDet.Rows.Count - 1, vDetRAmt, rs!rcptAmt)
            vsfDet.SetData(vsfDet.Rows.Count - 1, vDetExpAmt, rs!eAmt)
        Loop
        rs.Close()

        'Add Closing Balance
        vsfDet.Rows.Count = vsfDet.Rows.Count + 1
        vsfDet.SetData(vsfDet.Rows.Count - 1, vDetParticulars, "Closing Balance")
        strSQL = "Select sum(ramt-amt) as opBal "
        strSQL &= " From trnhdrexp h, trndtlexp d"
        strSQL &= " where h.expid = d.expid"
        strSQL &= " and h.EXPDate <= " & getDateFormatInCond(dtpFromDate.Value)
        vsfDet.SetData(vsfDet.Rows.Count - 1, vDetRAmt, clsSvt.getDataDoubleValue(strSQL, 0, False))

        vsfDet.Cols(vDetRAmt).Format = "0.00"
        vsfDet.Cols(vDetExpAmt).Format = "0.00"

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