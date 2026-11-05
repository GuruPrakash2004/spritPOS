Imports System.ComponentModel
Imports C1.Win.C1FlexGrid
Imports System.IO

Public Class frmRptStockAgeing
    Dim clsSvt As New svtLibrary
    Dim strSql As String
    Dim strPkgItemList As String
    Public strList As String
    Dim strItemGroupList As String

    Const vItemCancel = 0
    Const vItemID = 1
    Const vItemCode = 2
    Const vItemTotCol = 3


    Const vPkgCancel = 0
    Const vPkgID = 1
    Const vPkgCode = 2
    Const vPkgTotCol = 3

    Const vColName = 0
    Const vColValue = 1
    Const vTotCols = 2

    Const vFldRow = 0
    Const vComRow = 1
    Const vValRow = 2
    Const vTotRow = 3

    Const vRngFrom = 0
    Const vRngTo = 1
    Const vRngDetCol = 2

    Const vDetItemGrp = 0
    Const vDetItemID = 1
    Const vDetItemName = 2
    Const vDetQty = 3
    Const vDetValue = 4
    Const vDetTotCols = 5

    Const vPBItemGroup = 0
    Const vPBItemID = 1
    Const vPBItemName = 2
    Const vPBClQty = 3
    Const vPBNo = 4
    Const vPBDate = 5
    Const vPBAgeDays = 6
    Const vPBParticulars = 7
    Const vPBQty = 8
    Const vPBQtyLeft = 9
    Const vPBRate = 10
    Const vPBAmt = 11
    Const vDetPBCols = 12
    Dim strSymbols As String = "=|<>|<=|>=|>|<"

    Dim strPath As String = Application.StartupPath & "\Temp\StkAge.xml"
    Dim strRptName As String = ""

    Private Sub frmRptStockAgeing_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.KeyPreview = True
        Call fillFormProperties(Me)
        Call ChangeFontSize(Me)
        Call initGridStyles(Me)
        Call initScreen()
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs)
        frmMain.CloseSelectedTabPage(Me)
    End Sub

    Private Sub initScreen()
        cboValuation.Items.Clear()
        cboValuation.Items.Add("Standard Rate")
        cboValuation.Items.Add("FIFO Rate")
        cboValuation.Items.Add("Wt.Avg. Rate")
        cboValuation.Items.Add("Last Purchase Rate")
        cboValuation.Text = INIGet("Valuation", "Valuation", "")
        cboValuation.SelectedIndex = 0
        With vsfItem
            .Rows.Count = 1
            .Cols.Count = vItemTotCol
            .Rows.Fixed = 1
            .SetData(0, vItemCancel, "Selected Items")
            .SetData(0, vItemID, "Selected Items")
            .SetData(0, vItemCode, "Selected Items")
            .Rows(0).AllowMerging = True
            SetRowAlign(vsfItem, svtAlign.Center, 0)
            SetColAlign(vsfItem, svtAlign.Left, vItemCode)
            SetHideCol(vsfItem, True, vItemID)
        End With

        With vsfPkgItem
            .Rows.Count = 1
            .Cols.Count = vPkgTotCol
            .Rows.Fixed = 1
            .SetData(0, vPkgCancel, "Selected Packages")
            .SetData(0, vPkgID, "Selected Packages")
            .SetData(0, vPkgCode, "Selected Packages")
            .Rows(0).AllowMerging = True
            SetRowAlign(vsfPkgItem, svtAlign.Center, 0)
            SetHideCol(vsfPkgItem, True, vPkgID)
        End With

        With vsfRange
            .Rows.Count = 6
            .Cols.Count = 3
            .SetData(0, vRngFrom, "From")
            .SetData(0, vRngTo, "To")
            .Cols(vRngDetCol).Visible = False
            .Cols(vRngFrom).DataType = GetType(Integer)
            .Cols(vRngTo).DataType = GetType(Integer)


            If File.Exists(strPath) Then
                .ReadXml(strPath)
            Else
                .SetData(1, vRngFrom, "0")
                .SetData(1, vRngTo, "30")


                .SetData(2, vRngFrom, "31")
                .SetData(2, vRngTo, "60")

                .SetData(3, vRngFrom, "61")
                .SetData(3, vRngTo, "90")

                .SetData(4, vRngFrom, "91")
                .SetData(4, vRngTo, "180")

                .SetData(5, vRngFrom, "180")
            End If
        End With

        With vsfDet
            .Rows.Count = 1
            .Rows.Fixed = 1
            .Cols.Fixed = 0
            .Rows.GlyphRow = -1
            .Cols.Count = vDetTotCols

            .SetData(0, vDetItemGrp, "Item Group")
            .SetData(0, vDetItemName, "Item Name")
            .SetData(0, vDetQty, "Cl.Qty")
            .SetData(0, vDetValue, "Cl.Value")
            .Cols(vDetItemID).Visible = False
        End With

        Call SetFlexColor(vsfDet, "", vDetItemGrp, vDetItemName, vDetQty, vDetValue)

        Call SetColWidth(vsfRange, 80, 80)
        Call SetColAlign(vsfRange, svtAlign.Center, vRngFrom, vRngTo)

        With vsfFldVal
            .Cols.Count = 2
            .Rows.Count = vTotRow
            .Cols(0).AllowEditing = False
            .SetData(vFldRow, vColName, "Field")
            .SetData(vComRow, vColName, "Compare")
            .SetData(vValRow, vColName, "Value")
            SetRowAlign(vsfFldVal, svtAlign.Center, 0)
            .AllowResizing = True
        End With
        dtpDate.Value = getSystemDate()
        cboSelectLocation.Items.Add(clsSvt.getDataStringValue("Select LOCNAME from MstLOC", "", False))
    End Sub

    Private Sub btnItem_Click(sender As Object, e As EventArgs) Handles btnItem.Click
        Dim obj As New frmUtlAddRptItems
        obj.frm = Me
        obj.strType = "I"
        obj.argItemCode = vItemCode
        obj.ShowDialog()
        obj.Dispose()
        obj.Close()
    End Sub

    Private Sub btnPkgItem_Click(sender As Object, e As EventArgs) Handles btnPkgItem.Click
        Dim obj As New frmUtlAddRptItems
        obj.frm = Me
        obj.Text = "Add Package"
        obj.strType = "P"
        obj.argItemCode = vPkgCode
        obj.ShowDialog()
        obj.Dispose()
        obj.Close()
    End Sub

    Private Sub btnExportToXL_Click(sender As Object, e As EventArgs) Handles btnExportToXL.Click
        Call ExportToXL(vsfDet,,,, True, "Stock Ageing Report", "For the period " & getDateFormatInInsert(dtpDate.Value))
    End Sub

    Private Function ValidateFlds() As Boolean
        ValidateFlds = False

        If dtpDate.Value > CDate(gSettings.gCompEndDt) Then
            MsgBox("To date cannot be greater than " & gSettings.gCompEndDt, MsgBoxStyle.Critical, Me.Text)
            Exit Function
        End If

        ValidateFlds = True
    End Function

    Private Sub frmRptStockAgeing_KeyUp(sender As Object, e As KeyEventArgs) Handles Me.KeyUp
        If e.KeyCode = Keys.F9 And e.Control = True Then
            Call initFormSize(Me, Me.Text, True, 1015, 669, 0, 0)
            Call initFormVsfDet(Me, Me.Text, vsfItem, "ITEM")
            Call initFormVsfDet(Me, Me.Text, vsfPkgItem, "PKG")
            Call initFormVsfDet(Me, Me.Text, vsfFldVal, "FLDVAL")
            Call initFormVsfDet(Me, Me.Text, vsfRange, "RANGE")
            MsgBox("Restored Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
        If e.KeyCode = Keys.F8 And e.Control = True Then
            If File.Exists(strPath) Then
                File.Delete(strPath)
                vsfRange.WriteXml(strPath)
            End If
            Call SaveActFormSettings(Me, Me.Text)
            Call SaveActFormVsfDet(Me, Me.Text, "DET", vsfDet)
            Call SaveActFormVsfDet(Me, Me.Text, "ITEM", vsfItem)
            Call SaveActFormVsfDet(Me, Me.Text, "PKG", vsfPkgItem)
            Call SaveActFormVsfDet(Me, Me.Text, "FLDVAL", vsfFldVal)
            Call SaveActFormVsfDet(Me, Me.Text, "RANGE", vsfRange)
            MsgBox("Saved Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
        If e.Control = False And e.Alt = True And e.KeyCode = Keys.E Then
            Call ExportToXL(vsfDet)
        End If
    End Sub

    Private Sub txtItem_KeyUp(sender As Object, e As KeyEventArgs) Handles txtItem.KeyUp
        If e.KeyCode = Keys.Enter Then
            If txtItem.Text = "" Then Exit Sub
            fillItemDtls(txtItem.Text)
            txtItem.Text = ""
        End If
        If e.KeyCode = Keys.Down Then
            showItemPopup()
        End If
    End Sub

    Private Sub showItemPopup()
        Dim strSQL As String

        strSQL = "Select i.ItemID, ShortName, 0 as QOH, MRP, ItemCode, ItemName, ItemGroupName"
        strSQL &= " From MstItem i, MstItemGroup g"
        strSQL &= " where g.ItemGroupID=i.ItemGroupID "
        strSQL &= " And i.InActive = 'N' "
        strSQL &= " order by 1"

        txtDummy.Text = ""
        txtDummy2.Text = ""

        Dim objfrm As New frmUtlSelection

        With objfrm
            .blnMultiColSearch = True
            .FillViewVsf(Me, False, Nothing, strSQL, txtDummy, 0, txtDummy2, 1)
            .Text = " Please Select Item"
            .Tag = " frmRptStockBal "
            .ShowDialog()
            .Close()
        End With

        If Trim(txtDummy2.Text) <> "" Then
            fillItemDtls(Trim(txtDummy2.Text))
        End If
    End Sub

    Private Sub showPkgPopup()
        Dim strSQL As String

        strSQL = "Select Distinct PKGItem"
        strSQL &= " From MstItem"
        strSQL &= " where InActive = 'N'"
        strSQL &= " order by 1"

        txtDummy.Text = ""
        txtDummy2.Text = ""

        Dim objfrm As New frmUtlSelection

        With objfrm
            .blnMultiColSearch = True
            .FillViewVsf(Me, False, Nothing, strSQL, txtDummy, 0, txtDummy2, 2)
            .Text = " Please Select Package"
            .Tag = " frmRptStockBal "
            .ShowDialog()
            .Close()
        End With

        If Trim(txtDummy.Text) <> "" Then
            fillPkgDtls(Trim(txtDummy.Text))
        End If
    End Sub

    Public Sub fillItemDtls(Optional ByVal argItem As String = "")
        Dim strSQL As String
        Dim rs As oledb.oledbDataReader
        strSQL = "Select ItemID, ItemCode,ShortName from MstItem "
        strSQL &= " Where ItemName = '" & Trim(argItem) & "'"
        strSQL &= " Or ItemCode = '" & Trim(argItem) & "'"
        strSQL &= " Or ShortName = '" & Trim(argItem) & "'"
        strSQL &= " Or HotKey = '" & Trim(argItem) & "'"

        rs = clsSvt.getDataReaderFromSQL(strSQL)

        If Not rs.HasRows Then
            MsgBox("Invalid item", vbExclamation, Me.Text)
            txtItem.Text = ""
            rs.Close()
            Exit Sub
        End If

        rs.Read()

        With vsfItem
            .Rows.Count += 1
            .SetCellImage(.Rows.Count - 1, vItemCancel, My.Resources.cancel)
            .SetData(.Rows.Count - 1, vItemID, Val(rs!ItemID))
            .SetData(.Rows.Count - 1, vItemCode, GetVal(rs!ItemCode))
        End With
        rs.Close()
    End Sub

    Private Sub txtPkgItem_KeyUp(sender As Object, e As KeyEventArgs) Handles txtPkgItem.KeyUp
        If e.KeyCode = Keys.Enter Then
            If txtPkgItem.Text = "" Then Exit Sub
            fillPkgDtls(txtPkgItem.Text)
            txtPkgItem.Text = ""
        End If
        If e.KeyCode = Keys.Down Then
            showPkgPopup()
        End If
    End Sub

    Private Sub vsfItem_MouseClick(sender As Object, e As MouseEventArgs) Handles vsfItem.MouseClick
        Dim ht = vsfItem.HitTest

        With vsfItem
            If ht.Row < 1 Then Exit Sub
            If Val(.GetData(ht.Row, vItemID)) = 0 Then Exit Sub
            If ht.Column <> vItemCancel Then Exit Sub
            .RemoveItem(ht.Row)
        End With
    End Sub

    Private Sub vsfPkgItem_MouseClick(sender As Object, e As MouseEventArgs) Handles vsfPkgItem.MouseClick
        Dim ht = vsfPkgItem.HitTest

        With vsfPkgItem
            If ht.Row < 1 Then Exit Sub
            If Val(.GetData(ht.Row, vItemID)) = 0 Then Exit Sub
            If ht.Column <> vItemCancel Then Exit Sub
            .RemoveItem(ht.Row)
        End With
    End Sub

    Private Sub txtItem_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtItem.KeyPress
        If e.KeyChar = "'" Then
            e.Handled = True
        End If
    End Sub

    Private Sub txtPkgItem_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtPkgItem.KeyPress
        If e.KeyChar = "'" Then
            e.Handled = True
        End If
    End Sub
    Private Sub vsfItem_BeforeEdit(sender As Object, e As RowColEventArgs) Handles vsfItem.BeforeEdit
        Select Case e.Col
            Case vItemID, vItemCancel, vItemCode
                e.Cancel = True
        End Select
    End Sub

    Private Sub vsfPkgItem_BeforeEdit(sender As Object, e As RowColEventArgs) Handles vsfPkgItem.BeforeEdit
        Select Case e.Col
            Case vPkgID, vPkgCancel, vPkgCode
                e.Cancel = True
        End Select
    End Sub

    Private Sub vsfFldVal_BeforeEdit(sender As Object, e As RowColEventArgs) Handles vsfFldVal.BeforeEdit
        vsfFldVal.ComboList = ""
        vsfFldVal.EditMask = ""
        Select Case e.Row
            Case vFldRow
                vsfFldVal.Rows(vFldRow).ComboList = GenVSfComboStringCond(" COLUMN_NAME", " Information_schema.columns", " order by 1", " Where TABLE_NAME='MstItem'")
            Case vComRow
                vsfFldVal.Rows(vComRow).ComboList = strSymbols
        End Select
    End Sub

    Private Sub vsfRange_AfterEdit(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfRange.AfterEdit
        Dim i As Integer, intTONo As Long
        With vsfRange
            If e.Col = vRngTo Then
                If Trim(.GetData(e.Row, vRngTo)) = 0 Then
                    For i = e.Row + 1 To .Rows.Count - 1
                        .SetData(i, 1, "")
                        .SetData(i, 0, "")
                    Next i
                    Exit Sub
                End If

                intTONo = Val(.GetData(e.Row, 1)) + 1

                If e.Row < vsfRange.Rows.Count - 1 Then
                    .SetData(e.Row + 1, 0, intTONo)
                End If
            End If
        End With
    End Sub

    Private Sub vsfRange_BeforeEdit(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfRange.BeforeEdit
        Select Case e.Col
            Case vRngFrom
                If e.Row > 1 Then
                    e.Cancel = True
                End If
        End Select
    End Sub

    Private Sub vsfRange_EnterCell(ByVal sender As Object, ByVal e As System.EventArgs) Handles vsfRange.EnterCell
        With vsfRange
            If .Col = vRngTo Then
                .Focus()
                .StartEditing()
            End If
        End With
    End Sub

    Private Sub vsfRange_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles vsfRange.GotFocus
        With vsfRange
            If .GetData(1, vRngFrom) = 0 Then
                .Row = 1
                .Col = vRngFrom
                .Focus()
                .StartEditing()
            End If
        End With
    End Sub

    Private Sub vsfRange_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles vsfRange.KeyUp
        Dim intRow As Integer, intTONo As Long
        With vsfRange
            If e.KeyCode = Keys.Tab And .Row = .Rows.Count - 1 And .ColSel = vRngTo Then
                intTONo = Val(.GetData(.Row, vRngTo)) + 1
                intRow = .Rows.Count
                .Rows.Count = .Rows.Count + 1
                .Col = vRngTo
                .Row = intRow
                .SetData(.Row, vRngFrom, intTONo)
                .LeftCol = vRngTo
                .Focus()
            Else
                .Col = vRngTo
                .Focus()
            End If
        End With
    End Sub

    Private Sub vsfRange_LeaveCell(ByVal sender As Object, ByVal e As System.EventArgs) Handles vsfRange.LeaveCell
        With vsfRange
            If .Row <= 1 Then Exit Sub
            If .Col = vRngTo And Trim(.GetData(.Row, vRngTo)) = "" Then
                btnView.Focus()
                Exit Sub
            End If
        End With
    End Sub

    Private Sub vsfRange_ValidateEdit(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.ValidateEditEventArgs) Handles vsfRange.ValidateEdit
        If e.Col < 1 Then Exit Sub
        With vsfRange
            If Trim(.Editor.Text) = "" Then Exit Sub
            If Val(.Editor.Text) < Val(.GetData(e.Row, 0)) Then
                MsgBox(" Invalid Range! Check Entry...", MsgBoxStyle.Information + vbOKOnly, Me.Text)
                .StartEditing()
                e.Cancel = True
                Exit Sub
            End If
        End With
    End Sub

    Private Sub btnView_Click(sender As Object, e As EventArgs) Handles btnView.Click
        If chkShowPB.Checked Then
            Call PBAgeing
            Exit Sub
        End If
        Dim strSQL As String = ""
        Dim strRateFld As String = "", dtStockRateDate As Date
        'Call fillPkgDtls()
        Call INIStore("Valuation", "Valuation", cboValuation.Text)
        Select Case cboValuation.Text
            Case "Standard Rate"
                strRateFld = "StkRate"
            Case "FIFO Rate"
                strRateFld = "FIFORate"
            Case "Wt.Avg. Rate"
                strRateFld = "AvgRate"
            Case "Last Purchase Rate"
                strRateFld = "LastPurRate"
        End Select
        dtStockRateDate = clsSvt.getDataDateValue("Select Max(stkDate) from UtlStockRate where StkDate <= " & getDateFormatInCond(dtpDate.Value), getSystemDate, False)

        Dim rs As oledb.oledbDataReader
        With vsfDet
            .Rows.Count = 1
            .Cols.Count = vDetTotCols
            .SetData(0, vDetItemGrp, "Item Group Name")
            .SetData(0, vDetItemName, "Item Name")
            .SetData(0, vDetQty, "Closing Quantity")
            .SetData(0, vdetValue, "Closing Value")
        End With

        strSQL = "Select ItemGroupName, s.itemid, ItemName, sum(Qty * stkmultiplier) as clQty,sum(LtrQty * stkmultiplier) as clQtyLtr, "
        strSQL = strSQL & "isnull( (select Top 1 " & strRateFld & " from UtlStockrate r where r.itemid = s.itemid and r.stkdate = " & getDateFormatInCond(dtStockRateDate) & "),0) as stkRate"
        strSQL = strSQL & " from MstItemGroup g, MstItem i, UtlStockList s"
        strSQL = strSQL & " where s.ItemID = i.ItemID"
        strSQL = strSQL & " And i.ItemGroupID = g.ItemGroupId "
        strSQL = strSQL & " And DocDate <= " & getDateFormatInCond(dtpDate.Value)
        If chkItemGroup.Checked Then
            If strItemGroupList <> "" Then
                strSQL = strSQL & " and i.ItemGroupID in (" & strItemGroupList & ") "
            End If
        End If

        With vsfPkgItem
            If strPkgItemList <> "" Then
                strSQL = strSQL & " and i.ItemID in (" & strPkgItemList & ") "
            End If
        End With
        strSQL = strSQL & " group by ItemGroupName,GroupOrderBy, s.itemid, ItemName"
        strSQL = strSQL & " having sum(Qty * stkmultiplier)  > 0"
        strSQL = strSQL & " order by GroupOrderBy"
        rs = clsSvt.getDataReaderFromSQL(strSQL)

        If Not rs.HasRows Then
            rs.Close()
            Exit Sub
        End If

        With rs
            Do While .Read
                vsfDet.Rows.Count = vsfDet.Rows.Count + 1
                vsfDet.SetData(vsfDet.Rows.Count - 1, vDetItemGrp, !ItemGroupName)
                vsfDet.SetData(vsfDet.Rows.Count - 1, vDetItemID, !ItemID)
                vsfDet.SetData(vsfDet.Rows.Count - 1, vDetItemName, !ItemName)
                vsfDet.SetData(vsfDet.Rows.Count - 1, vDetQty, GetVal(!ClQty, ""))
                vsfDet.SetData(vsfDet.Rows.Count - 1, vDetValue, GetVal(!ClQty * !StkRate, ""))
            Loop
            .Close()
        End With
        With vsfDet
            For i = 1 To vsfRange.Rows.Count - 1
                If Val(vsfRange.GetData(i, 0)) <> 0 Or Val(vsfRange.GetData(i, 1)) <> 0 Then
                    .Cols.Count = .Cols.Count + 1
                    If vsfRange.Rows.Count - 1 = i Then
                        .SetData(0, .Cols.Count - 1, Val(vsfRange.GetData(i, 0)))
                    Else
                        .SetData(0, .Cols.Count - 1, Val(vsfRange.GetData(i, 0)) & " - " & Val(vsfRange.GetData(i, 1)))
                    End If
                    .Cols.Count = .Cols.Count + 1
                    .SetData(0, .Cols.Count - 1, "Value")
                End If
            Next i
        End With
        Call DispData()

        vsfDet.Cols(vDetItemGrp).AllowMerging = True
        Call initFormVsfDet(Me, Me.Text, vsfDet, "VIEWCLBAL")
    End Sub

    Private Sub DispData()
        Dim rs As oledb.oledbDataReader
        Dim strSQL As String, intRow As Integer
        Dim intAgeDays As Integer, intCol As Integer, sngQty As Double

        strSQL = "Select s.itemid, sum(Qty) as rQty, datediff(d,docdate," & getDateFormatInCond(dtpDate.Value) & ")  as AgeDays"
        strSQL = strSQL & " from UtlStockList s"
        strSQL = strSQL & " where StkMultiplier = 1"
        If chkItemGroup.Checked Then
            If strItemGroupList <> "" Then
                strSQL = strSQL & " And i.ItemGroupID In (" & strItemGroupList & ") "
            End If
        End If
        If strPkgItemList <> "" Then
            strSQL = strSQL & " And i.ItemID In (" & strPkgItemList & ") "
        End If
        strSQL = strSQL & " group by s.itemid, docDate"
        strSQL = strSQL & " order by 1"

        rs = clsSvt.getDataReaderFromSQL(strSQL)

        With vsfDet
            Do While rs.Read
                With vsfDet
                    intRow = 0
                    For i = 1 To .Rows.Count - 1
                        If .GetData(i, vDetItemID) = rs!ItemID Then
                            intRow = i
                            Exit For
                        End If
                    Next
                End With
                If intRow > 0 Then
                    intAgeDays = GetVal(rs!AgeDays, 0)
                    With vsfRange
                        For i = 1 To .Rows.Count - 1
                            If i = 1 Then
                                If intAgeDays <= Val(.GetData(i, 1)) Then
                                    intCol = vDetTotCols
                                End If
                            ElseIf i = .Rows.Count - 1 Then
                                If intAgeDays >= Val(.GetData(i, 0)) Then
                                    intCol = vDetTotCols + (i * 2)
                                End If
                            Else
                                If intAgeDays >= Val(.GetData(i, 0)) And intAgeDays <= Val(.GetData(i, 1)) Then
                                    intCol = vDetTotCols + (i * 2)
                                End If
                            End If
                        Next i
                    End With
                    vsfDet.SetData(intRow, intCol - 1, Val(vsfDet.GetData(intRow, intCol - 1)) + GetVal(rs!RQty, 0))
                End If
            Loop
            'For i = 1 To .Rows.Count - 1
            '    Application.DoEvents()
            '    .TopRow = i
            '    sngQty = Math.Round(Val(.GetData(i, vDetQty)), 3)
            '    For j = vDetQty + 2 To .Cols.Count - 1 Step 2
            '        If Val(.GetData(i, j)) <= sngQty Then
            '            sngQty = sngQty - Val(.GetData(i, j))
            '        Else
            '            .SetData(i, j, Math.Round(sngQty, 3))
            '            For k = j + 1 To .Cols.Count - 1
            '                .SetData(i, k, "")
            '            Next k
            '            Exit For
            '        End If
            '    Next j
            'Next i
        End With
    End Sub

    Private Sub chkItemGroup_CheckedChanged(sender As Object, e As System.EventArgs) Handles chkItemGroup.CheckedChanged
        Dim strSQL As String
        If chkItemGroup.Checked = False Then
            strList = ""
            strItemGroupList = ""
            Exit Sub
        End If

        Dim frmSel As New frmUtlMultiSelection


        strSQL = "Select distinct 0 as sel, ItemGroupID, ItemGroupCode,GroupOrderBy from MstItemGroup where ItemGroupID > 0 and "
        strSQL = strSQL & " InActive = 'N' order by GroupOrderBy"
        Call frmSel.InitVsfSel(strSQL, 1, 1, True, Me, False)
        frmSel.Text = "Select [Multiple] Item Groups"
        frmSel.vsfDet.SetData(0, 0, "Sel")
        frmSel.ShowDialog()
        strItemGroupList = strList

        If Trim(strItemGroupList) = "" Then chkItemGroup.Checked = False
    End Sub
    Private Sub vsfDet_BeforeEdit(sender As Object, e As RowColEventArgs) Handles vsfDet.BeforeEdit
        e.Cancel = True
    End Sub

    Public Sub fillPkgDtls(ByVal argPkg As String)
        Dim rs As oledb.oledbDataReader
        Dim strSQL As String

        strSQL = "Select ItemID, PKGItem from MstItem"
        strSQL &= " Where PKGItem = '" & Trim(argPkg) & "'"

        rs = clsSvt.getDataReaderFromSQL(strSQL)

        If Not rs.HasRows Then
            MsgBox("Invalid package item", vbExclamation, Me.Text)
            txtPkgItem.Text = ""
            Exit Sub
        End If

        rs.Read()

        With vsfPkgItem
            .Rows.Count += 1
            .SetCellImage(.Rows.Count - 1, vPkgCancel, My.Resources.cancel)
            .SetData(.Rows.Count - 1, vPkgID, Val(rs!ItemID))
            .SetData(.Rows.Count - 1, vPkgCode, GetVal(rs!PKGItem))
            .Row = .Rows.Count - 1
        End With
        rs.Close()

        strPkgItemList = ""
        For i = 1 To vsfPkgItem.Rows.Count - 1
            strPkgItemList &= clsSvt.getDataLongValue("Select ItemID from MstItem where PKGITEM = '" & Trim(vsfPkgItem.GetData(i, vPkgCode) & "'"), 0, False) & ","
        Next
        strPkgItemList = strPkgItemList.Remove(strPkgItemList.Length - 1)
    End Sub

    Private Sub btnClose_Click_1(sender As Object, e As EventArgs) Handles btnClose.Click
        frmMain.CloseSelectedTabPage(Me)
    End Sub
    Private Sub PBAgeing()
        Dim strSQL As String = "", sngBalQty As Double
        Dim rs As OleDb.OleDbDataReader
        Dim rsD As OleDb.OleDbDataReader
        Dim intCtr As Integer
        With vsfDet
            .Rows.Count = 1
            .Cols.Count = vDetPBCols
            .SetData(0, vPBItemGroup, "Item Group Name")
            .SetData(0, vPBItemName, "Item Name")
            .SetData(0, vPBQty, "PB Qty")
            .SetData(0, vPBQtyLeft, "Qty Left")
            .SetData(0, vPBAmt, "Amount")
            .SetData(0, vPBParticulars, "Particulars")
            .SetData(0, vPBNo, "PB No")
            .SetData(0, vPBDate, "PB Date")
            .SetData(0, vPBRate, "Rate")
            .SetData(0, vPBClQty, "ClQty")
            .SetData(0, vPBAgeDays, "Age Days")
            .Cols(vPBNo).TextAlign = TextAlignEnum.LeftCenter
        End With

        strSQL = "Select ItemGroupName, s.itemid, ItemName, sum(Qty * stkmultiplier) as clQty"
        strSQL = strSQL & " from MstItemGroup g, MstItem i, UtlStockList s"
        strSQL = strSQL & " where s.ItemID = i.ItemID"
        strSQL = strSQL & " And i.ItemGroupID = g.ItemGroupId "
        strSQL = strSQL & " And DocDate <= " & getDateFormatInCond(dtpDate.Value)
        If chkItemGroup.Checked Then
            If strItemGroupList <> "" Then
                strSQL = strSQL & " and i.ItemGroupID in (" & strItemGroupList & ") "
            End If
        End If
        With vsfPkgItem
            If strPkgItemList <> "" Then
                strSQL = strSQL & " and i.ItemID in (" & strPkgItemList & ") "
            End If
        End With
        strSQL = strSQL & " group by ItemGroupName,GroupOrderBy, s.itemid, ItemName"
        strSQL = strSQL & " having sum(Qty * stkmultiplier)  > 0"
        strSQL = strSQL & " order by GroupOrderBy"
        rs = clsSvt.getDataReaderFromSQL(strSQL)

        If Not rs.HasRows Then
            rs.Close()
            Exit Sub
        End If

        With rs
            Do While .Read
                vsfDet.Rows.Count = vsfDet.Rows.Count + 1
                vsfDet.SetData(vsfDet.Rows.Count - 1, vPBItemGroup, !ItemGroupName)
                vsfDet.SetData(vsfDet.Rows.Count - 1, vPBItemName, !ItemName)
                vsfDet.SetData(vsfDet.Rows.Count - 1, vPBClQty, GetVal(!ClQty, ""))

                sngBalQty = Val(vsfDet.GetData(vsfDet.Rows.Count - 1, vPBClQty))

                'fill pbdtls till cum pbqty <= sngbalqty
                strSQL = "Select rowid, docdate, doctype, docno, particulars, qty, rate  "
                strsql &= " from UtlStockList s "
                strSQL &= " where StkMultiplier = 1 "
                strSQL &= " and itemid = " & !itemID
                strSQL &= " order by docdate desc, rowid desc"
                rsD = clsSvt.getDataReaderFromSQL(strSQL)
                intCtr = 0
                Do While rsD.Read
                    If sngBalQty <= 0 Then Exit Do
                    If intCtr > 0 Then
                        vsfDet.Rows.Count = vsfDet.Rows.Count + 1
                        vsfDet.SetData(vsfDet.Rows.Count - 1, vPBItemGroup, !ItemGroupName)
                        vsfDet.SetData(vsfDet.Rows.Count - 1, vPBItemName, !ItemName)
                        vsfDet.SetData(vsfDet.Rows.Count - 1, vPBItemID, !ItemID)
                        vsfDet.SetData(vsfDet.Rows.Count - 1, vPBClQty, GetVal(!ClQty, ""))
                    End If
                    vsfDet.SetData(vsfDet.Rows.Count - 1, vPBNo, rsD!DocNO)
                    vsfDet.SetData(vsfDet.Rows.Count - 1, vPBDate, rsD!Docdate)
                    vsfDet.SetData(vsfDet.Rows.Count - 1, vPBAgeDays, DateDiff(DateInterval.Day, rsD!Docdate, getSystemDate))
                    vsfDet.SetData(vsfDet.Rows.Count - 1, vPBParticulars, rsD!Particulars)
                    vsfDet.SetData(vsfDet.Rows.Count - 1, vPBRate, rsD!Rate)
                    vsfDet.SetData(vsfDet.Rows.Count - 1, vPBQty, rsD!Qty)
                    If sngBalQty > rsD!Qty Then
                        vsfDet.SetData(vsfDet.Rows.Count - 1, vPBQtyLeft, rsD!Qty)
                    Else
                        vsfDet.SetData(vsfDet.Rows.Count - 1, vPBQtyLeft, sngBalQty)
                    End If
                    vsfDet.SetData(vsfDet.Rows.Count - 1, vPBAmt, Math.Round(Val(vsfDet.GetData(vsfDet.Rows.Count - 1, vPBQtyLeft)) * rsD!rate, 2))
                    intCtr = intCtr + 1
                    sngBalQty = sngBalQty - rsD!Qty
                Loop
                rsD.Close()
            Loop
            .Close()
        End With
        vsfDet.Cols(vPBItemGroup).AllowMerging = True
        vsfDet.Cols(vPBItemID).AllowMerging = True
        vsfDet.Cols(vPBItemName).AllowMerging = True
        vsfDet.Cols(vPBClQty).AllowMerging = True
        Call initFormVsfDet(Me, Me.Text, vsfDet, "vwCLPB")
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
End Class