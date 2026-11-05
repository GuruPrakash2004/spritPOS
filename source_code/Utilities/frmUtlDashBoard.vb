Imports System.IO
Imports C1.Win.C1FlexGrid

Public Class frmUtlDashboard
    Dim clsSVT As New svtLibrary
    Dim strRight As String
    Dim strSQL As String

    Private Sub frmUtlDashboard_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.KeyPreview = True
        ' fillFormProperties(Me)
        'initGridStyles(Me)
        FillSettingClr()
        Call loadData()
        FillChart()
    End Sub
    Private Sub FillChart()

        Dim intDayExpense As Double
        Dim intWeekExpense As Double
        Dim intMonthExpense As Double
        Dim StrExpense As String

        Dim intDaySale As Double
        Dim intWeekSale As Double
        Dim intMonthSale As Double
        Dim StrSale As String

        Dim intDayPB As Double
        Dim intWeekPB As Double
        Dim intMonthPB As Double
        Dim StrPB As String

        Dim rs As oledb.oledbDataReader
        rs = clsSVT.getDataReaderFromSQL("select infoheader,infoqry from UtlDashQry where sectionHdr in('SALES','PURCHASE')  order by sectionHdr")
        If rs.HasRows Then
            Do While rs.Read
                '--Expense
                If rs("infoheader") = "Expenses For The Day" Then
                    StrExpense = rs("infoqry")
                    intDayExpense = clsSVT.getDataLongValue(StrExpense, 0, False)
                End If
                If rs("infoheader") = "Expenses For The Week" Then
                    StrExpense = rs("infoqry")
                    intWeekExpense = clsSVT.getDataLongValue(StrExpense, 0, False)
                End If
                If rs("infoheader") = "Expenses For The Month" Then
                    StrExpense = rs("infoqry")
                    intMonthExpense = clsSVT.getDataLongValue(StrExpense, 0, False)
                End If
                '--Sales
                If rs("infoheader") = "Sales For The Day" Then
                    StrSale = rs("infoqry")
                    intDaySale = clsSVT.getDataLongValue(StrSale, 0, False)
                End If
                If rs("infoheader") = "Sales For The Week" Then
                    StrSale = rs("infoqry")
                    intWeekSale = clsSVT.getDataLongValue(StrSale, 0, False)
                End If
                If rs("infoheader") = "Sales For The Month" Then
                    StrSale = rs("infoqry")
                    intMonthSale = clsSVT.getDataLongValue(StrSale, 0, False)
                End If

                '--Purchase
                If rs("infoheader") = "Purchase For The Day" Then
                    StrPB = rs("infoqry")
                    intDayPB = clsSVT.getDataLongValue(StrPB, 0, False)
                End If
                If rs("infoheader") = "Purchase For The Week" Then
                    StrPB = rs("infoqry")
                    intWeekPB = clsSVT.getDataLongValue(StrPB, 0, False)
                End If
                If rs("infoheader") = "Purchase For The Month" Then
                    StrPB = rs("infoqry")
                    intMonthPB = clsSVT.getDataLongValue(StrPB, 0, False)
                End If
            Loop
            rs.Close()
        End If


        ChartExpense.Series("Expenses").Points.AddXY("Day", intDayExpense)
        ChartExpense.Series("Expenses").Points.AddXY("Week", intWeekExpense)
        ChartExpense.Series("Expenses").Points.AddXY("Month", intMonthExpense)

        ChartSaleVsPB.Series("Sales").Points.AddXY("Day", intDaySale)
        ChartSaleVsPB.Series("Sales").Points.AddXY("Week", intWeekSale)
        ChartSaleVsPB.Series("Sales").Points.AddXY("Month", intMonthSale)

        ChartSaleVsPB.Series("Purchase").Points.AddXY("Day", intDayPB)
        ChartSaleVsPB.Series("Purchase").Points.AddXY("Week", intWeekPB)
        ChartSaleVsPB.Series("Purchase").Points.AddXY("Month", intMonthPB)

    End Sub

    Private Sub chartype()

        ChartExpense.Series("Expenses").ChartType = DataVisualization.Charting.SeriesChartType.Line

    End Sub
    Private Sub loadData()
        initFormVsfDet(Me, Me.Text, vsfDet1, "D1")
        initFormVsfDet(Me, Me.Text, vsfDet2, "D2")
        initFormVsfDet(Me, Me.Text, vsfDet3, "D3")
        initFormVsfDet(Me, Me.Text, vsfDet4, "D4")
        With vsfDet1
            .Rows.Count = 0
            .Cols.Count = 4
            .Cols(0).Width = 200
        End With

        With vsfDet2
            .Rows.Count = 0
            .Cols.Count = 4
            .Cols(0).Width = 200
        End With

        With vsfDet3
            .Rows.Count = 0
            .Cols.Count = 4
            .Cols(0).Width = 200
        End With

        With vsfDet4
            .Rows.Count = 0
            .Cols.Count = 4
            .Cols(0).Width = 200
        End With

        showWaitCursor()
        FillDetDtl()

        'If clsSVT.getDataStringValue("Select DashOnStartup From UtlMisc", "N", False) = "Y" Then
        '    chkShow.Checked = True
        'Else
        '    chkShow.Checked = False
        'End If

        showNormalCursor()
    End Sub

    Private Sub FillDetDtl()
        Dim rs As oledb.oledbDataReader
        Dim rsx As oledb.oledbDataReader
        Dim strsql As String
        Dim intRow As Int32
        Dim intDashID As Int32
        Dim argVSF As C1.Win.C1FlexGrid.C1FlexGrid = Nothing
        strsql = "Select * from UtldashQry "
        strsql &= " where DashShow='Y'"
        strsql &= " Order By SectionHdr,Orderno "
        rs = clsSVT.getDataReaderFromSQL(strSQL)

        If rs.HasRows Then
            Do While rs.Read
                Select Case GetVal(rs("SectionHdr"))
                    Case "SALES"
                        argVSF = vsfDet1
                    Case "PURCHASE"
                        argVSF = vsfDet2
                    Case "STOCK"
                        argVSF = VsfDet3
                    Case "MISC"
                        argVSF = vsfDet4
                End Select
                lbl1.Text = "Sales Summary Info"
                lbl2.Text = "Purchase Summary Info"
                lbl3.Text = "Stock Summary Info"
                lbl4.Text = "Other/Misc Info"
                Call filldata(argVSF, GetVal(rs("infoqry")), GetVal(rs("InfoType")), GetVal(rs("GroupHeader")), GetVal(rs("InfoHeader")))
            Loop
        End If
        rs.Close()
    End Sub


    Private Sub filldata(ByVal Argvsf As Object, ByVal argStr As String, ByVal arginfoType As String, ByVal argGrpHdr As String, ByVal ArgInfoHDr As String)
        Dim rsx As oledb.oledbDataReader
        Dim intRow As Int32
        addRow(Argvsf, argGrpHdr, "", "", True, True)
        strSQL = Replace(argStr, "`", "'")
        Try
            rsx = clsSVT.getDataReaderFromSQL(strSQL)


            If arginfoType = "GRIDVALUE" Then
                If rsx.HasRows Then
                    Argvsf.Cols.Count = rsx.FieldCount
                    Do While rsx.Read()
                        Argvsf.Rows.Count = Argvsf.Rows.Count + 1
                        intRow = Argvsf.Rows.Count - 1
                        For x As Int32 = 0 To Argvsf.Cols.Count - 1
                            Argvsf.SetData(intRow, x, rsx.GetValue(x))
                        Next
                    Loop
                End If
            Else
                If rsx.HasRows Then
                    Do While rsx.Read()
                        For x As Int32 = 0 To 0
                            addRow(Argvsf, ArgInfoHDr, GetVal(rsx.GetValue(x), 0))

                        Next
                    Loop
                End If
            End If

            rsx.Close()
        Catch ex As Exception
            MsgBox("Error In RecordID " & ArgInfoHDr & argStr, MsgBoxStyle.Critical, Me.Text)
        End Try

    End Sub

    Private Sub frmUtlDashboard_KeyUp(sender As Object, e As KeyEventArgs) Handles Me.KeyUp
        If e.KeyCode = Keys.F6 Then
            Call loadData()
            Exit Sub
        End If

        If e.KeyCode = Keys.F4 Then
            Dim objFrm As New frmUtlDashSettings
            objFrm.StartPosition = FormStartPosition.CenterParent
            objFrm.ShowDialog()
        End If

        If e.KeyCode = Keys.F9 And e.Control = True Then
            Call initFormSize(Me, Me.Text, True, 642, 469, 0, 0)
            Call initFormVsfDet(Me, Me.Text, vsfDet1, "D1")
            Call initFormVsfDet(Me, Me.Text, vsfDet2, "D2")
            Call initFormVsfDet(Me, Me.Text, vsfDet3, "D3")
            Call initFormVsfDet(Me, Me.Text, vsfDet4, "D4")
            MsgBox("Restored Form & Grid Layout Successfully!!", MsgBoxStyle.Information, Me.Text)
        End If

        If e.KeyCode = Keys.F8 And e.Control = True Then
            Call SaveActFormSettings(Me, Me.Text)
            Call SaveActFormVsfDet(Me, Me.Text, "D1", vsfDet1)
            Call SaveActFormVsfDet(Me, Me.Text, "D2", vsfDet2)
            Call SaveActFormVsfDet(Me, Me.Text, "D3", vsfDet3)
            Call SaveActFormVsfDet(Me, Me.Text, "D4", vsfDet4)
            MsgBox("Saved Form & Grid Layout Successfully!!", MsgBoxStyle.Information, Me.Text)
        End If
    End Sub

    Public Sub FillSettingClr()
        Dim rs As oledb.oledbDataReader
        Dim strSQL As String


        strSQL = "Select * from UtlDashSettings"
        Try
            rs = clsSVT.getDataReaderFromSQL(strSQL)
            If rs.HasRows Then
                rs.Read()
                If rs("ShowDashBoard") = "Y" Then
                    filgridClrs(vsfDet1, pnl1, System.Drawing.Color.FromArgb(rs("SalesBGClr")), System.Drawing.Color.FromArgb(rs("SalesfrClr")), GetVal(rs("infoFont"), "Segoe UI"), GetVal(rs("infoSize"), 12), GetVal(rs("infoStyle"), "Regular"), GetVal(rs("HdrFont"), "Segoe UI"), GetVal(rs("HdrSize"), 14), GetVal(rs("HdrStyle"), "Bold"))
                    filgridClrs(vsfDet2, pnl2, System.Drawing.Color.FromArgb(rs("PurchaseBGClr")), System.Drawing.Color.FromArgb(rs("PurchaseFrClr")), GetVal(rs("infoFont"), "Segoe UI"), GetVal(rs("infoSize"), 12), GetVal(rs("infoStyle"), "Regular"), GetVal(rs("HdrFont"), "Segoe UI"), GetVal(rs("HdrSize"), 14), GetVal(rs("HdrStyle"), "Bold"))
                    filgridClrs(VsfDet3, pnl3, System.Drawing.Color.FromArgb(rs("StockBGClr")), System.Drawing.Color.FromArgb(rs("StockFrClr")), GetVal(rs("infoFont"), "Segoe UI"), GetVal(rs("infoSize"), 12), GetVal(rs("infoStyle"), "Regular"), GetVal(rs("HdrFont"), "Segoe UI"), GetVal(rs("HdrSize"), 14), GetVal(rs("HdrStyle"), "Bold"))
                    filgridClrs(vsfDet4, pnl4, System.Drawing.Color.FromArgb(rs("MiscBGClr")), System.Drawing.Color.FromArgb(rs("MiscFrClr")), GetVal(rs("infoFont"), "Segoe UI"), GetVal(rs("infoSize"), 12), GetVal(rs("infoStyle"), "Regular"), GetVal(rs("HdrFont"), "Segoe UI"), GetVal(rs("HdrSize"), 14), GetVal(rs("HdrStyle"), "Bold"))
                Else
                    DefaultColors(vsfDet1, pnl1, System.Drawing.Color.FromArgb(233, 187, 41), System.Drawing.Color.FromName("Black"))
                    DefaultColors(vsfDet2, pnl2, System.Drawing.Color.FromArgb(239, 83, 173), System.Drawing.Color.FromName("Black"))
                    DefaultColors(VsfDet3, pnl3, System.Drawing.Color.FromArgb(69, 137, 85), System.Drawing.Color.FromName("Black"))
                    DefaultColors(vsfDet4, pnl4, System.Drawing.Color.FromArgb(90, 173, 163), System.Drawing.Color.FromName("Black"))


                End If
            End If
        Catch ex As Exception
            MsgBox("Error While Applying Colors", MsgBoxStyle.Information, Me.Text)
        End Try
    End Sub
    Private Sub filgridClrs(ByVal argvsf As Object, ByVal pnl As Panel, argbgclr As Color, argfrclr As Color, argFontVname As String, argfontVSize As Int32, argfontVstyle As String, argFontHname As String, argfontHSize As Int32, argfontHstyle As String)
        pnl.BackColor = argbgclr
        With argvsf
            .BackColor = pnl1.BackColor
            .Styles.Alternate.BackColor = pnl.BackColor
            .Styles.Normal.BackColor = pnl.BackColor
            .Styles.Focus.BackColor = pnl.BackColor
            .Styles.Fixed.BackColor = pnl.BackColor
            .Styles.Highlight.BackColor = pnl.BackColor
            .Styles.EmptyArea.BackColor = pnl.BackColor
            .ForeColor = argfrclr
            .Font = New Font(argFontVname, argfontVSize)

            Dim csf1 As CellStyle = .Styles.Add("StyleClr")
            csf1.Font = New Font(argFontHname, argfontHSize, FontStyle.Bold)
        End With
    End Sub

    Private Sub DefaultColors(ByVal argvsf As Object, ByVal pnl As Panel, argbgclr As Color, argfrclr As Color)

        pnl.BackColor = argbgclr
        With argvsf
            .BackColor = pnl1.BackColor
            .Styles.Alternate.BackColor = pnl.BackColor
            .Styles.Normal.BackColor = pnl.BackColor
            .Styles.Focus.BackColor = pnl.BackColor
            .Styles.Fixed.BackColor = pnl.BackColor
            .Styles.Highlight.BackColor = pnl.BackColor
            .Styles.EmptyArea.BackColor = pnl.BackColor
            .ForeColor = argfrclr
            .Font = New Font("Segoe UI", 12)

            Dim csf1 As CellStyle = .Styles.Add("StyleClr")
            csf1.Font = New Font("Segoe UI", 14, FontStyle.Bold)
        End With

    End Sub

    Private Sub lblSettings_Click(sender As Object, e As EventArgs) Handles lblSettings.Click
        Dim objFrm As New frmUtlDashSettings
        objFrm.StartPosition = FormStartPosition.CenterScreen
        objFrm.argFrm = Me
        objFrm.ShowDialog()
    End Sub

    Private Sub btnSettings_Click(sender As Object, e As EventArgs) Handles btnSettings.Click
        Dim objFrm As New frmUtlDashSettings
        objFrm.StartPosition = FormStartPosition.CenterScreen
        objFrm.argFrm = Me
        objFrm.ShowDialog()
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        frmMain.CloseSelectedTabPage(Me)
    End Sub

    Private Sub lblRefresh_Click(sender As Object, e As EventArgs) Handles lblRefresh.Click
        Call loadData()
    End Sub

    Private Sub addRow(argvsf As Object, argCaption As String, argValue As String, Optional argValue2 As String = "", Optional blnHighlight As Boolean = False, Optional blnSubHeding As Boolean = False)
        With argvsf
            For i = 0 To .rows.count - 1
                If argCaption = .getData(i, 0) Then
                    Exit Sub
                End If
            Next

            .Rows.Count = .Rows.Count + 1
            .SetData(.Rows.Count - 1, 0, argCaption)
            .SetData(.Rows.Count - 1, 1, argValue)
            .SetData(.Rows.Count - 1, 2, argValue2)


            If blnHighlight Then
                Dim csf1 As CellStyle = .Styles.Add("StyleClr")
                Dim StrFont As String
                Dim StrSize As Int32
                Dim rs As oledb.oledbDataReader
                rs = clsSVT.getDataReaderFromSQL("Select showdashBoard ,HdrFont, HdrSize,HdrStyle From UtlDashSettings where showdashBoard='Y'")

                If rs.HasRows Then
                    rs.Read()

                    StrFont = GetVal(rs("HdrFont"), "Segoe UI")
                    StrSize = GetVal(rs("HdrSize"), 12)
                    csf1.Font = New Font(StrFont, StrSize, FontStyle.Bold)
                    .Rows(.Rows.Count - 1).Style = csf1
                Else

                    Dim myColor As Color = Color.Moccasin
                    csf1.BackColor = myColor
                    csf1.Font = New Font("Segoe UI", 12, FontStyle.Bold)
                    .Rows(.Rows.Count - 1).Style = csf1
                End If

                rs.Close()
            End If

            If blnSubHeding Then
                Dim csf2 As CellStyle = .Styles.Add("StyleClr")
                Dim myColor2 As Color = Color.Gray
                csf2.BackColor = myColor2
            End If
        End With
    End Sub

    Private Sub btnXLSales_Click(sender As Object, e As EventArgs) Handles btnXLSales.Click
        Call ExportToXL(vsfDet1, lbl1.Text)
    End Sub

    Private Sub btnXLPB_Click(sender As Object, e As EventArgs) Handles btnXLPB.Click
        Call ExportToXL(vsfDet2, lbl2.Text)
    End Sub

    Private Sub btnXLStock_Click(sender As Object, e As EventArgs) Handles btnXLStock.Click
        Call ExportToXL(vsfDet3, lbl3.Text)
    End Sub

    Private Sub btnXLMisc_Click(sender As Object, e As EventArgs) Handles btnXLMisc.Click
        Call ExportToXL(vsfDet4, lbl4.Text)
    End Sub

    Private Sub vsfDet1_BeforeEdit(sender As Object, e As RowColEventArgs) Handles vsfDet1.BeforeEdit, vsfDet2.BeforeEdit, VsfDet3.BeforeEdit, vsfDet4.BeforeEdit
        e.Cancel = True
    End Sub


End Class