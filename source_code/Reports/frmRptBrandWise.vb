Imports C1.Win.C1FlexGrid

Public Class frmRptBrandWise
    Dim clsSvt As New svtLibrary
    Dim strSQL As String
    Public strList As String
    Dim strBrandList As String
    Dim strItemGroup As String

    Const VDetBrand = 0
    Const VDetItemGroup = 1
    Const VDetItemCode = 2
    Const VDetItemName = 3
    Const VDetUOM = 4
    Const VDetPurQty = 5
    Const VDetPurRate = 6
    Const VDetPurAmt = 7
    Const vDetSalQty = 8
    Const vDetSalRate = 9
    Const vDetSalAmt = 10
    Const vDetBPercase = 11
    Const vDetCase = 12
    Const vDetBottles = 13
    Const VDetTotCols = 14

    Private Sub initscreen()
        With vsfDet
            .Rows.Count = 1
            .Cols.Count = VDetTotCols
            .SetData(0, VDetBrand, "Brand")
            .SetData(0, VDetItemGroup, "ItemGroup")
            .SetData(0, VDetItemCode, "ItemCode")
            .SetData(0, VDetItemName, "ItemName")
            .SetData(0, VDetUOM, "Uom")
            .SetData(0, VDetPurQty, "Purchase Qty")
            .SetData(0, VDetPurRate, "Purchase Rate")
            .SetData(0, VDetPurAmt, "Purchase Value")
            .SetData(0, vDetSalQty, "Sales Qty")
            .SetData(0, vDetSalRate, "Sales Rate")
            .SetData(0, vDetSalAmt, "Sales Value")
            .SetData(0, vDetBPercase, "Bottle per Case")
            .SetData(0, vDetCase, "No of Case")
            .SetData(0, vDetBottles, "No of Bottles")
            SetFlexColorFullGrid(vsfDet, "")
            SetHideCol(vsfDet, True, vDetBPercase)
        End With
    End Sub

    Private Sub frmRptPendingPO_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.KeyPreview = True
        fillFormProperties(Me)
        initGridStyles(Me)
        initscreen()
        initFormVsfDet(Me, Me.Text, vsfDet, "DET")
        dtpFrom.Value = gSettings.gCompStDt
    End Sub

    Private Sub chkBrand_CheckedChanged(sender As Object, e As EventArgs) Handles chkBrand.CheckedChanged
        Dim strSQL As String
        If chkBrand.Checked = False Then
            strBrandList = ""
            Exit Sub
        End If
        Dim frmSel As New frmUtlMultiSelection

        strSQL = "Select 0 as sel, BrandID,BrandCode,BrandName from MstBRAND Where BrandID > 0 Order by 1"

        Call frmSel.InitVsfSel(strSQL, 1, 1, True, Me, False)
        frmSel.Text = "Select [Multiple] Brand"
        frmSel.vsfDet.SetData(0, 0, "Sel")
        frmSel.StartPosition = FormStartPosition.CenterScreen
        frmSel.ShowDialog()
        strBrandList = strList
        If strBrandList = "" Then chkBrand.Checked = False
    End Sub

    Private Sub chkItemGroup_CheckedChanged(sender As Object, e As EventArgs) Handles chkItemGroup.CheckedChanged
        Dim strSQL As String
        If chkItemGroup.Checked = False Then
            strList = ""
            strItemGroup = ""
            Exit Sub
        End If

        Dim frmSel As New frmUtlMultiSelection
        frmSel.StartPosition = FormStartPosition.CenterScreen
        strSQL = "Select 0 as sel,ItemGroupID,ItemGroupCode,ItemGroupName From MstItemGroup Where ItemGroupID > 0 Order By 1"
        Call frmSel.InitVsfSel(strSQL, 1, 1, True, Me, False)
        frmSel.Text = "Select [Multiple] ItemGroup"
        frmSel.vsfDet.SetData(0, 0, "Sel")
        frmSel.ShowDialog()
        strItemGroup = strList
        If strItemGroup = "" Then chkItemGroup.Checked = False
    End Sub

    Private Sub btnView_Click(sender As Object, e As EventArgs) Handles btnView.Click
        showWaitCursor()
        PrintRpt()
        showNormalCursor()
    End Sub

    Private Sub btnExportToXL_Click(sender As Object, e As EventArgs) Handles btnExportToXL.Click
        ExportToXL(vsfDet,,,, True, Me.Text, "For the period " & getDateFormatInInsert(dtpFrom.Value) & " to " & getDateFormatInInsert(dtpTo.Value))
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        frmMain.CloseSelectedTabPage(Me)
    End Sub

    Private Sub PrintRpt()

        Dim rs As oledb.oledbDataReader
        Dim strSQL As String
        Dim i As Int32

        strSQL = "Select BrandID,BrandCode,ItemGroupID,ItemGroupCode,ItemCode,ItemName, "
        strSQL &= " UomCode,sum(PBQty) as pbQty,max(PBRate) as pbRate,sum(PBAmt) as pbAmt,sum(SInvQty) as sinvqty,max(SInvRate) as sinvrate,sum(SINVAMT) as sinvAmt,BotPerCases"
        strSQL &= " From vwBrandReport Where 1=1"
        If optPur.Checked Then
            strSQL &= " And DocType='Purchase'"
        ElseIf optSales.Checked Then
            strSQL &= " and DocType='Sales'"
        End If
        If chkBrand.Checked AndAlso strBrandList <> "" Then
            strSQL &= " and BrandID In (" & strBrandList & ")"
        End If
        If chkItemGroup.Checked AndAlso strItemGroup <> "" Then
            strSQL &= " and ItemGroupID In (" & strItemGroup & ")"
        End If
        strSQL &= " and " & SetDtCondn("DocDate", dtpFrom.Value, dtpTo.Value)
        strSQL &= " Group by BrandID,BrandCode,ItemGroupID,ItemGroupCode,ItemCode,ItemName, uomCode,BotPerCases"
        strSQL &= " Order by brandcode, itemname"
        rs = clsSvt.getDataReaderFromSQL(strSQL)
        Dim dblCase As Double
        With vsfDet
            .Rows.Count = 1
            If rs.HasRows Then
                Do While rs.Read
                    .Rows.Count = .Rows.Count + 1
                    i = .Rows.Count - 1
                    .SetData(i, VDetBrand, GetVal(rs("BrandCode"), ""))
                    .SetData(i, VDetItemGroup, GetVal(rs("ItemGroupCode"), ""))
                    .SetData(i, VDetItemCode, GetVal(rs("ItemCode"), ""))
                    .SetData(i, VDetItemName, GetVal(rs("Itemname"), ""))
                    .SetData(i, VDetUOM, GetVal(rs("UOMCode"), ""))

                    .SetData(i, VDetPurQty, GetVal(rs("PBQTY"), 0))
                    .SetData(i, VDetPurRate, GetVal(rs("PBRate"), 0))
                    .SetData(i, VDetPurAmt, GetVal(rs("PBAmt"), 0))
                    .SetData(i, vDetSalQty, GetVal(rs("SINVQty"), 0))
                    .SetData(i, vDetSalRate, GetVal(rs("SINVRate"), 0))
                    .SetData(i, vDetSalAmt, GetVal(rs("SINVAmt"), 0))
                    .SetData(i, vDetBPercase, GetVal(rs("BotPerCases"), 0))
                    'If GetVal(rs("BotPerCases"), 0)) >  0 Then 
                    '    dblCase = Math.Round(GetVal(rs("SINVQty"), 0) / GetVal(rs("BotPerCases"), 0)),2)
                    '    .SetData(i, vDetBPercase, dblCase)
                    'End If

                    Dim dblresult As Double = 0
                    Dim dblreminder As Double = 0

                    'If condition added by Surabhi to handle division by 0 exeception

                    If optPur.Checked Then
                        If vsfDet.GetData(i, vDetBPercase) > 0 Then
                            dblresult = Math.Round(vsfDet.GetData(i, VDetPurQty) / vsfDet.GetData(i, vDetBPercase), 2)
                        End If

                        If vsfDet.GetData(i, vDetBPercase) <> 0 Then
                            dblreminder = vsfDet.GetData(i, VDetPurQty) Mod vsfDet.GetData(i, vDetBPercase)
                        End If
                    End If

                    If optSales.Checked Then
                        If vsfDet.GetData(i, vDetBPercase) > 0 Then
                            dblresult = Math.Round(vsfDet.GetData(i, vDetSalQty) / vsfDet.GetData(i, vDetBPercase), 2)
                        End If

                        If vsfDet.GetData(i, vDetBPercase) <> 0 Then
                            dblreminder = vsfDet.GetData(i, vDetSalQty) Mod vsfDet.GetData(i, vDetBPercase)
                        End If
                    End If

                    'vsfDet.SetData(i, vDetInCase, Math.Floor(dblresult) & " Cases " & dblreminder & " Bottles")
                    vsfDet.SetData(i, vDetCase, Math.Floor(dblresult))
                    vsfDet.SetData(i, vDetBottles, dblreminder)
                Loop
                rs.Close()
            Else
                MsgBox("No Records Found!", MsgBoxStyle.Information, Me.Text)
                .Rows.Count = 1
            End If

            For c As Int32 = 0 To .Cols.Count - 1
                .Cols(c).Visible = True
            Next
            .Cols(vDetBPercase).Visible = False



            If optPur.Checked Then
                .Cols(vDetSalQty).Visible = False
                .Cols(vDetSalRate).Visible = False
                .Cols(vDetSalAmt).Visible = False
            ElseIf optSales.Checked Then
                .Cols(VDetPurQty).Visible = False
                .Cols(VDetPurRate).Visible = False
                .Cols(VDetPurAmt).Visible = False
            Else
                .Cols(vDetSalRate).Visible = False
                .Cols(vDetSalAmt).Visible = False
                .Cols(VDetPurRate).Visible = False
                .Cols(VDetPurAmt).Visible = False
            End If

            If Not ChkWithVal.Checked Then
                .Cols(VDetPurRate).Visible = False
                .Cols(VDetPurAmt).Visible = False
                .Cols(vDetSalRate).Visible = False
                .Cols(vDetSalAmt).Visible = False
            Else
                If optPur.Checked Then
                    .Cols(VDetPurRate).Visible = True
                    .Cols(VDetPurAmt).Visible = True
                ElseIf optSales.Checked Then
                    .Cols(vDetSalRate).Visible = True
                    .Cols(vDetSalAmt).Visible = True
                Else
                    .Cols(VDetPurRate).Visible = True
                    .Cols(VDetPurAmt).Visible = True
                    .Cols(vDetSalRate).Visible = True
                    .Cols(vDetSalAmt).Visible = True
                End If
            End If

            For i = VDetBrand To VDetUOM
                vsfDet.Cols(i).AllowMerging = True
            Next
            For i = 1 To .Rows.Count - 1
                If UCase(vsfDet.GetData(i, VDetUOM)) = "QTY" Then
                    .Cols(VDetPurQty).Format = "#.00"
                    .Cols(vDetSalQty).Format = "#.00"
                Else
                    .Cols(vDetSalQty).Format = "#"
                    .Cols(VDetPurQty).Format = "#"
                End If
            Next



            .Tree.Column = 0
            .Tree.Style = C1.Win.C1FlexGrid.TreeStyleFlags.Simple
            .Subtotal(C1.Win.C1FlexGrid.AggregateEnum.Clear)
            .SubtotalPosition = C1.Win.C1FlexGrid.SubtotalPositionEnum.BelowData
            For x As Int32 = VDetPurQty To .Cols.Count - 1
                .Subtotal(C1.Win.C1FlexGrid.AggregateEnum.Sum, 0, VDetBrand, x, "Total- {0}")
                .Subtotal(C1.Win.C1FlexGrid.AggregateEnum.Sum, -1, -1, x, "Grand Total")
            Next

        End With
    End Sub

    Private Sub frmRptPendingPO_KeyUp(sender As Object, e As KeyEventArgs) Handles Me.KeyUp
        If e.KeyCode = Keys.F9 And e.Control = True Then
            Call initFormVsfDet(Me, Me.Text, vsfDet, "DET")
            MsgBox("Restored Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
        If e.KeyCode = Keys.F8 And e.Control = True Then
            Call SaveActFormSettings(Me, Me.Text)
            Call SaveActFormVsfDet(Me, Me.Text, "DET", vsfDet)
            MsgBox("Saved Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
    End Sub

    Private Sub vsfDet_BeforeEdit(sender As Object, e As RowColEventArgs) Handles vsfDet.BeforeEdit
        e.Cancel = True
    End Sub

    Private Sub pnlbtnClose_Click(sender As Object, e As EventArgs) Handles pnlbtnClose.Click
        pnlCopyShortCut.Visible = False
    End Sub

    Private Sub btnCopy_Click(sender As Object, e As EventArgs) Handles btnCopy.Click

        If RbnCopyCell.Checked = True Then
            CopyGrid(vsfDet, "Cell")
        ElseIf RbnCopyColumn.Checked = True Then
            CopyGrid(vsfDet, "Column")
        ElseIf RbnCopyRow.Checked = True Then
            CopyGrid(vsfDet, "Row")
        ElseIf RbnCopyGrid.Checked = True Then
            CopyGrid(vsfDet, "Full Grid")

        End If
        pnlCopyShortCut.Visible = False
    End Sub

    Private Sub vsfDet_Click(sender As Object, e As EventArgs) Handles vsfDet.Click

        If (Control.ModifierKeys And Keys.Control) = Keys.Control AndAlso (Control.ModifierKeys And Keys.Alt) = Keys.Alt Then
            pnlCopyShortCut.Show()
        End If

    End Sub
    'Private Sub ChkWithVal_CheckedChanged(sender As Object, e As EventArgs) Handles ChkWithVal.CheckedChanged

    '    If ChkWithVal.Checked Then
    '        cboRateType.Visible = True
    '    Else
    '        cboRateType.Visible = False
    '    End If

    'End Sub

    'Private Sub optPur_CheckedChanged(sender As Object, e As EventArgs) Handles optPur.CheckedChanged

    'End Sub

    'Private Sub optSales_CheckedChanged(sender As Object, e As EventArgs) Handles optSales.CheckedChanged

    'End Sub

    'Private Sub BindCombo()

    '    'If optPur.Checked Then
    '    '    BindCombo(cboRateType, "Column_Name", "Column_Name", "Select Column_Name From INFORMATION_SCHEMA.COLUMNS Where TABLE_NAME = 'MstItem' and COLUMN_NAME='PURRATE'")
    '    'ElseIf optSales.Checked Then
    '    '    BindCombo(cboRateType, "Column_Name", "Column_Name", "Select Column_Name From INFORMATION_SCHEMA.COLUMNS Where TABLE_NAME = 'MstItem' and COLUMN_NAME ='MPR'")
    '    'End If
    'End Sub
End Class