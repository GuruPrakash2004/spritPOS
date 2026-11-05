Imports System.Data.oledb
Imports C1.Win.C1FlexGrid

Public Class frmRptStkLevel
    Dim clsSVT As New svtLibrary

    Public strList As String
    Dim strItemGroupList As String
    Dim strItemList As String
    Dim strValMethod As String
    Dim arrRctps(4, 1) As String
    Dim arrFlds() As String
    Dim intAdnlFldCount As String
    Dim strPkgItemList As String
    Dim strListItem As String

    Const vItemCancel = 0
    Const vItemID = 1
    Const vItemShortName = 2
    Const vItemTotCol = 3

    Const vPkgCancel = 0
    Const vPkgID = 1
    Const vPkgName = 2
    Const vPkgTotCol = 3

    Const vDetItemGrpName = 0
    Const vDetItemName = 1
    Const vDetOptName = 2
    Const vDetClQty = 3
    Const vDetValue = 4
    Const vDetRate = 5

    Const vPKItemGrpName = 0
    Const vPKItemName = 1
    Const vPKTotQty = 2

    Const vRIItemGrpName = 0
    Const vRIItemName = 1
    Const vRIOPQty = 2
    Const vRIRcptQty = 3
    Const vRIIssQty = 4
    Const vRIClQty = 5
    Const vRIClValue = 6
    Const vRIRate = 7

    Dim strRateFld As String
    Dim dtStockRateDate As Date
    Dim strRptName As String = ""

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        frmMain.CloseSelectedTabPage(Me)
    End Sub

    Private Function ValidateFlds() As Boolean
        ValidateFlds = False

        If chkItemGroup.Checked Then
            If strItemGroupList = "" Then
                MsgBox("Item Group Selection required from the list ", MsgBoxStyle.Critical, Me.Text)
                Exit Function
            End If
        End If

        ValidateFlds = True
    End Function


    Private Sub btnView_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView.Click
        If Not ValidateFlds() Then
            Exit Sub
        End If

        dtStockRateDate = clsSVT.getDataDateValue("Select Max(stkDate) from UtlStockRate where StkDate <= " & getDateFormatInCond(dtpFrom.Value), getSystemDate, False)

        If optBlwMin.Checked Then Call ViewBelowMinLevel()
        If optBlwMinROL.Checked Then Call ViewBelowROL()
        If optMaxLevel.Checked Then Call viewAboveMaxLevel()
    End Sub

    Private Sub RemoveVsfRows(argVSF As C1.Win.C1FlexGrid.C1FlexGrid)
        Dim i As Integer
        With argVSF
Repeat:
            For i = 1 To (.Rows.Count - 1)
                If chkZeroBal.Checked Then
                    If Val(.GetData(i, .Cols.Count - 2)) = 0 Then
                        .RemoveItem(i)
                        GoTo Repeat
                    End If
                End If
                'If chkZeroValue.Checked Then
                '    If Val(.GetData(i, .Cols.Count - 1)) < Val(txtValBelow.Text) Then
                '        .RemoveItem(i)
                '        GoTo Repeat
                '    End If
                'End If
            Next i
        End With
    End Sub

    Private Sub frmRptStock_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.KeyPreview = True
        Call initFormSize(Me, Me.Text, False, 958, 579, 0, 0)
        Call initFormVsfDet(Me, Me.Text, vsfItem, "ITEM")
        Call initFormVsfDet(Me, Me.Text, vsfPkgItem, "PKG")
        Call fillFormProperties(Me)
        Call initGridStyles(Me)
        Call initScreen()
    End Sub

    Private Sub initScreen()
        With vsfItem
            .Rows.Count = 1
            .Cols.Count = vItemTotCol
            .Rows.Fixed = 1
            .SetData(0, vItemCancel, "Selected Item")
            .SetData(0, vItemID, "Selected Item")
            .SetData(0, vItemShortName, "Selected Item")
            .Rows(0).AllowMerging = True
            SetRowAlign(vsfItem, svtAlign.Center, 0)
            SetColAlign(vsfItem, svtAlign.Left, vItemShortName)
            SetHideCol(vsfItem, True, vItemID)
        End With

        With vsfPkgItem
            .Rows.Count = 1
            .Cols.Count = vPkgTotCol
            .Rows.Fixed = 1
            .SetData(0, vPkgCancel, "Selected Package")
            .SetData(0, vPkgID, "Selected Package")
            .SetData(0, vPkgName, "Selected Package")
            .Rows(0).AllowMerging = True
            SetRowAlign(vsfPkgItem, svtAlign.Center, 0)
            SetColAlign(vsfPkgItem, svtAlign.Left, vPkgName)
            SetHideCol(vsfPkgItem, True, vPkgID)
        End With

        With vsfView
            .Rows.Count = 1
            .Cols.Count = 4
            .SetData(0, vDetItemGrpName, "Item Group Name")
            .SetData(0, vDetItemName, "Item Name")
            .SetData(0, vDetOptName, "Min Level")
            .SetData(0, vDetClQty, "Closing Quantity")
        End With

        Call SetFlexColor(vsfView, "", vDetItemGrpName, vDetItemName, vDetClQty, vDetOptName)
        ' dtpFrom.Value = getFirstDateInMonth(getSystemDate)
        dtpFrom.Value = Date.Today
        ' Call bindCombo(cboLocation, "LocID", "LocName", "Select locid, LocName from MstLoc where LocID > 0 order by 1")
    End Sub

    Private Sub btnExportToXL_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExportToXL.Click
        Call ExportToXL(vsfView,,,, True, Me.Text, "For the period " & getDateFormatInInsert(dtpFrom.Value))
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

    Private Sub vsfDet_BeforeEdit(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfView.BeforeEdit
        e.Cancel = True
    End Sub


    Private Sub frmRptStock_KeyUp(sender As Object, e As KeyEventArgs) Handles MyBase.KeyUp
        If e.KeyCode = Keys.F9 And e.Control = True Then
            Call initFormSize(Me, Me.Text, True, 958, 579, 0, 0)
            MsgBox("Restored Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If

        If e.KeyCode = Keys.F8 And e.Control = True Then
            Call SaveActFormSettings(Me, Me.Text)
            If optBlwMin.Checked Then Call SaveActFormVsfDet(Me, Me.Text, "VIEWMINLEVEL", vsfView)
            If optBlwMinROL.Checked Then Call SaveActFormVsfDet(Me, Me.Text, "VIEWROL", vsfView)
            If optMaxLevel.Checked Then Call SaveActFormVsfDet(Me, Me.Text, "VIEWMAXLEVEL", vsfView)
            Call SaveActFormVsfDet(Me, Me.Text, "ITEM", vsfItem)
            Call SaveActFormVsfDet(Me, Me.Text, "PKG", vsfPkgItem)
            MsgBox("Saved Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If

        If e.Control = False And e.Alt = True And e.KeyCode = Keys.E Then
            Call ExportToXL(vsfView)
        End If

        If e.Alt = True AndAlso e.KeyCode = Keys.I Then
            Call btnItem_Click(sender, e)
        End If

        'If e.Alt = True AndAlso e.KeyCode = Keys.P Then
        '    Call btnPkgItem_Click(sender, e)
        'End If
    End Sub


    Private Sub chkItem_Click(sender As Object, e As EventArgs)
        Dim strSQL As String
        'If chkItem.Checked = False Then
        '    strList = ""
        '    strItemList = ""
        '    Exit Sub
        'End If

        Dim frmSel As New frmUtlMultiSelection

        strSQL = "Select 0 as sel, ItemID, ItemCode, ItemName from MstItem where ItemID > 0 and "
        strSQL = strSQL & " InActive = 'N' order by ItemCode"
        Call frmSel.InitVsfSel(strSQL, 1, 1, True, Me, False)
        frmSel.Text = "Select [Multiple] Item"
        frmSel.vsfDet.SetData(0, 0, "Sel")
        frmSel.ShowDialog()
        strItemList = strList
        'If Trim(strItemList) = "" Then chkItem.Checked = False
    End Sub

    Private Sub txtItem_KeyUp(sender As Object, e As KeyEventArgs) Handles txtItem.KeyUp
        If e.KeyCode = Keys.Enter Then
            If Trim(txtItem.Text) = "" Then Exit Sub
            fillItemDtls(Trim(txtItem.Text))
            txtItem.Text = ""
        End If

        If e.KeyCode = Keys.Down Then
            showItemPopup()
        End If
    End Sub

    Public Sub fillItemDtls(ByVal argItem As String)
        With vsfPkgItem
            .Rows.Count = 1
        End With
        Dim rs As oledbDataReader
        Dim strSQL As String

        strSQL = "Select ItemID, ShortName from MstItem "
        strSQL &= " Where ItemName = '" & Trim(argItem) & "'"
        strSQL &= " Or ItemCode = '" & Trim(argItem) & "'"
        strSQL &= " Or ShortName = '" & Trim(argItem) & "'"
        strSQL &= " Or HotKey = '" & Trim(argItem) & "'"

        rs = clsSVT.getDataReaderFromSQL(strSQL)

        If Not rs.HasRows Then
            MsgBox("Invalid item", vbExclamation, Me.Text)
            txtItem.Text = ""
            Exit Sub
        End If

        rs.Read()

        With vsfItem
            .Rows.Count += 1
            .SetCellImage(.Rows.Count - 1, vItemCancel, My.Resources.cancel)
            .SetData(.Rows.Count - 1, vItemID, Val(rs!ItemID))
            .SetData(.Rows.Count - 1, vItemShortName, GetVal(rs!ShortName))
            .Row = .Rows.Count - 1
        End With

        rs.Close()
    End Sub

    Private Sub txtPkgItem_KeyUp(sender As Object, e As KeyEventArgs) Handles txtPkgItem.KeyUp
        If e.KeyCode = Keys.Enter Then
            If Trim(txtPkgItem.Text) = "" Then Exit Sub
            fillPkgDtls(Trim(txtPkgItem.Text))
            txtPkgItem.Text = ""
        End If

        If e.KeyCode = Keys.Down Then
            showPkgPopup()
        End If
    End Sub

    Public Sub fillPkgDtls(ByVal argPkg As String)
        With vsfItem
            .Rows.Count = 1
        End With
        Dim rs As oledbDataReader
        Dim strSQL As String

        strSQL = "Select ItemID, PKGItem from MstItem"
        strSQL &= " Where PKGItem = '" & Trim(argPkg) & "'"

        rs = clsSVT.getDataReaderFromSQL(strSQL)

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
            .SetData(.Rows.Count - 1, vPkgName, GetVal(rs!PKGItem))
            .Row = .Rows.Count - 1
        End With
        rs.Close()

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
            Case vItemID, vItemCancel, vItemShortName
                e.Cancel = True
        End Select
    End Sub

    Private Sub vsfPkgItem_BeforeEdit(sender As Object, e As RowColEventArgs) Handles vsfPkgItem.BeforeEdit
        Select Case e.Col
            Case vPkgID, vPkgCancel, vPkgName
                e.Cancel = True
        End Select
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

    Private Sub btnItem_Click(sender As Object, e As EventArgs) Handles btnItem.Click
        Dim obj As New frmUtlAddRptItems
        obj.Text = "Add Items"
        obj.frm = Me
        obj.strType = "I"
        obj.argItemCode = vItemShortName
        obj.ShowDialog()
        obj.Dispose()
        obj.Close()
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
            .Tag = "frmRptStockBal"
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

    Private Sub vsfView_DoubleClick(sender As Object, e As EventArgs) Handles vsfView.DoubleClick
        If vsfView.Col = 0 Then
            If vsfView.Cols(0).AllowMerging = False Then
                vsfView.Cols(0).AllowMerging = True
            Else
                vsfView.Cols(0).AllowMerging = False
            End If
        End If
    End Sub

    Private Sub ViewBelowMinLevel()
        Dim strSQL As String = ""
        Dim rs As oledbDataReader
        With vsfView
            .Rows.Count = 1
            .Cols.Count = 4
            .SetData(0, vDetItemGrpName, "Item Group Name")
            .SetData(0, vDetItemName, "Item Name")
            .SetData(0, vDetOptName, "Min Level")
            If chkQtyInLtr.Checked Then
                .SetData(0, vDetClQty, "Closing Quantity-Ltr")
            Else
                .SetData(0, vDetClQty, "Closing Quantity")
            End If
            Call SetFlexColor(vsfView, "", vDetItemGrpName, vDetItemName, vDetClQty, vDetOptName)
        End With

        strSQL = "Select ItemGroupName, s.itemid, ItemName, i.MINLevel, sum(Qty*StkMultiplier) as clQty,sum(LtrQty*StkMultiplier) as clQtyLtr "
        strSQL = strSQL & " from MstItemGroup g, MstItem i, UtlStockList s"
        strSQL = strSQL & " where s.ItemID = i.ItemID"
        strSQL = strSQL & " And i.ItemGroupID = g.ItemGroupId "
        strSQL = strSQL & " And i.MinLevel > 0 "
        strSQL = strSQL & " And DocDate <= " & getDateFormatInCond(dtpFrom.Value)

        If chkItemGroup.Checked Then
            If strItemGroupList <> "" Then
                strSQL = strSQL & " And i.ItemGroupID In (" & strItemGroupList & ") "
            End If
        End If

        Dim strItemList As String = ""
        strItemList = getItemList()

        If strItemList <> "" Then
            strSQL = strSQL & " And i.ItemID In (" & strItemList & ") "
        End If

        strItemList = ""
        strItemList = getPkgList()
        If strItemList <> "" Then
            strSQL = strSQL & " And i.PKGITEM In (" & strItemList & ") "
        End If

        strSQL = strSQL & " group by ItemGroupName, s.itemid, ItemName, i.MinLevel,GroupOrderBy "
        strSQL = strSQL & " Having sum(Qty*StkMultiplier) < i.MinLevel "
        strSQL = strSQL & " order by GroupOrderBy"
        rs = clsSVT.getDataReaderFromSQL(strSQL)

        If Not rs.HasRows Then
            rs.Close()
            Exit Sub
        End If

        With rs
            Do While .Read
                vsfView.Rows.Count = vsfView.Rows.Count + 1
                vsfView.SetData(vsfView.Rows.Count - 1, vDetItemGrpName, !ItemGroupName)
                vsfView.SetData(vsfView.Rows.Count - 1, vDetItemName, !ItemName)
                vsfView.SetData(vsfView.Rows.Count - 1, vDetOptName, !MinLevel)
                If chkQtyInLtr.Checked = True Then
                    vsfView.SetData(vsfView.Rows.Count - 1, vDetClQty, GetVal(!ClQtyLtr, ""))
                Else
                    vsfView.SetData(vsfView.Rows.Count - 1, vDetClQty, GetVal(!ClQty, ""))
                End If
            Loop
            .Close()
        End With
        vsfView.Cols(vDetItemGrpName).AllowMerging = True
        'Call initFormVsfDet(Me, Me.Text, vsfView, "VIEWCLBAL")
    End Sub

    Private Sub ViewBelowROL()
        Dim strSQL As String = ""
        Dim rs As oledbDataReader
        With vsfView
            .Rows.Count = 1
            .Cols.Count = 4
            .SetData(0, vDetItemGrpName, "Item Group Name")
            .SetData(0, vDetItemName, "Item Name")
            .SetData(0, vDetOptName, "Re-Order Level")
            If chkQtyInLtr.Checked Then
                .SetData(0, vDetClQty, "Closing Quantity-Ltr")
            Else
                .SetData(0, vDetClQty, "Closing Quantity")
            End If
            Call SetFlexColor(vsfView, "", vDetItemGrpName, vDetItemName, vDetClQty, vDetOptName)
        End With

        strSQL = "Select ItemGroupName, s.itemid, ItemName, i.ROL, sum(Qty*StkMultiplier) as clQty,sum(LtrQty*StkMultiplier) as clQtyLtr "
        ' strSQL = strSQL & "isnull( (select top 1 " & strRateFld & " from UtlStockrate r where r.itemid = s.itemid and r.stkdate = " & getDateFormatInCond(dtStockRateDate) & "),0) as stkRate"
        strSQL = strSQL & " from MstItemGroup g, MstItem i, UtlStockList s"
        strSQL = strSQL & " where s.ItemID = i.ItemID"
        strSQL = strSQL & " And i.ItemGroupID = g.ItemGroupId "
        strSQL = strSQL & " And i.ROL > 0 "
        strSQL = strSQL & " And DocDate <= " & getDateFormatInCond(dtpFrom.Value)
        If chkItemGroup.Checked Then
            If strItemGroupList <> "" Then
                strSQL = strSQL & " And i.ItemGroupID In (" & strItemGroupList & ") "
            End If
        End If

        Dim strItemList As String = ""
        strItemList = getItemList()

        If strItemList <> "" Then
            strSQL = strSQL & " And i.ItemID In (" & strItemList & ") "
        End If

        strItemList = ""
        strItemList = getPkgList()
        If strItemList <> "" Then
            strSQL = strSQL & " And i.PKGITEM In (" & strItemList & ") "
        End If

        strSQL = strSQL & " group by ItemGroupName, GroupOrderBy,s.itemid, ItemName, i.ROL "
        strSQL = strSQL & " Having sum(Qty*StkMultiplier) < i.ROL "
        strSQL = strSQL & " order by  GroupOrderBy"
        rs = clsSVT.getDataReaderFromSQL(strSQL)

        If Not rs.HasRows Then
            rs.Close()
            Exit Sub
        End If

        With rs
            Do While .Read
                vsfView.Rows.Count = vsfView.Rows.Count + 1
                vsfView.SetData(vsfView.Rows.Count - 1, vDetItemGrpName, !ItemGroupName)
                vsfView.SetData(vsfView.Rows.Count - 1, vDetItemName, !ItemName)
                vsfView.SetData(vsfView.Rows.Count - 1, vDetOptName, !ROL)
                If chkQtyInLtr.Checked = True Then
                    vsfView.SetData(vsfView.Rows.Count - 1, vDetClQty, GetVal(!ClQtyLtr, ""))
                Else
                    vsfView.SetData(vsfView.Rows.Count - 1, vDetClQty, GetVal(!ClQty, ""))
                End If
            Loop
            .Close()
        End With
        vsfView.Cols(vDetItemGrpName).AllowMerging = True
        'Call initFormVsfDet(Me, Me.Text, vsfView, "VIEWCLBAL")
    End Sub

    Private Sub viewAboveMaxLevel()
        Dim strSQL As String = ""
        Dim rs As oledbDataReader
        With vsfView
            .Rows.Count = 1
            .Cols.Count = 4
            .SetData(0, vDetItemGrpName, "Item Group Name")
            .SetData(0, vDetItemName, "Item Name")
            .SetData(0, vDetOptName, "Max Level")
            If chkQtyInLtr.Checked Then
                .SetData(0, vDetClQty, "Closing Quantity-Ltr")
            Else
                .SetData(0, vDetClQty, "Closing Quantity")
            End If
            Call SetFlexColor(vsfView, "", vDetItemGrpName, vDetItemName, vDetClQty, vDetOptName)
        End With

        strSQL = "Select ItemGroupName, s.itemid, ItemName, i.MaxLevel, sum(Qty*StkMultiplier) as clQty,sum(LtrQty*StkMultiplier) as clQtyLtr "
        'strSQL = strSQL & "isnull( (select top 1 " & strRateFld & " from UtlStockrate r where r.itemid = s.itemid and r.stkdate = " & getDateFormatInCond(dtStockRateDate) & "),0) as stkRate"
        strSQL = strSQL & " from MstItemGroup g, MstItem i, UtlStockList s"
        strSQL = strSQL & " where s.ItemID = i.ItemID"
        strSQL = strSQL & " And i.ItemGroupID = g.ItemGroupId "
        strSQL = strSQL & " And i.MaxLevel > 0 "
        strSQL = strSQL & " And DocDate <= " & getDateFormatInCond(dtpFrom.Value)
        If chkItemGroup.Checked Then
            If strItemGroupList <> "" Then
                strSQL = strSQL & " And i.ItemGroupID In (" & strItemGroupList & ") "
            End If
        End If

        Dim strItemList As String = ""
        strItemList = getItemList()

        If strItemList <> "" Then
            strSQL = strSQL & " And i.ItemID In (" & strItemList & ") "
        End If

        strItemList = ""
        strItemList = getPkgList()
        If strItemList <> "" Then
            strSQL = strSQL & " And i.PKGITEM In (" & strItemList & ") "
        End If

        strSQL = strSQL & " group by ItemGroupName,GroupOrderBy, s.itemid, ItemName, i.MaxLevel "
        strSQL = strSQL & " Having sum(Qty*StkMultiplier) > i.MaxLevel "
        strSQL = strSQL & " order by GroupOrderBy"
        rs = clsSVT.getDataReaderFromSQL(strSQL)

        If Not rs.HasRows Then
            rs.Close()
            Exit Sub
        End If

        With rs
            Do While .Read
                vsfView.Rows.Count = vsfView.Rows.Count + 1
                vsfView.SetData(vsfView.Rows.Count - 1, vDetItemGrpName, !ItemGroupName)
                vsfView.SetData(vsfView.Rows.Count - 1, vDetItemName, !ItemName)
                vsfView.SetData(vsfView.Rows.Count - 1, vDetOptName, !MaxLevel)
                If chkQtyInLtr.Checked = True Then
                    vsfView.SetData(vsfView.Rows.Count - 1, vDetClQty, GetVal(!ClQtyLtr, ""))
                Else
                    vsfView.SetData(vsfView.Rows.Count - 1, vDetClQty, GetVal(!ClQty, ""))
                End If
            Loop
            .Close()
        End With
        vsfView.Cols(vDetItemGrpName).AllowMerging = True
        'Call initFormVsfDet(Me, Me.Text, vsfView, "VIEWCLBAL")
    End Sub

    Private Sub ReplaceSingleQuoteInGrid()
        With vsfView
            For i As Int32 = 1 To .Rows.Count - 1
                For j As Int32 = 0 To .Cols.Count - 1
                    .SetData(i, j, CStr(.GetData(i, j) & "").Replace("'", "`"))
                Next
            Next
        End With
    End Sub

    Private Sub btnPrint_Click(sender As Object, e As EventArgs) Handles btnPrint.Click
        If Not TransferGridData() Then
            Exit Sub
        End If

        Call PrintData()
    End Sub

    Private Sub btnEmailPDF_Click(sender As Object, e As EventArgs) Handles btnEmailPDF.Click
        If Not TransferGridData() Then
            Exit Sub
        End If

        Call PrintData(True)
    End Sub

    Private Function TransferGridData() As Boolean
        TransferGridData = False
        With vsfView
            If .Rows.Count <= 1 Then
                MsgBox("No Data...", MsgBoxStyle.Exclamation, Me.Text)
                Exit Function
            End If

            Call ReplaceSingleQuoteInGrid()

            Dim strSQL As String = ""
            Dim strSQL2 As String = ""

            If optBlwMin.Checked Then
                strRptName = " Stock Level - Below Min Level "
            ElseIf optBlwMinROL.Checked Then
                strRptName = " Stock Level - Below ROL "
            ElseIf optMaxLevel.Checked Then
                strRptName = " Stock Level - Above Max Level "
            Else
                strRptName = "Stock Level - Below Min Level"
            End If

            strSQL = " Delete from RptHdrStk Where RptName='" & strRptName & "' and UserName='" & gStrSystemName & "' "
            If Not clsSVT.execSQLStatement(strSQL) Then
                Exit Function
            End If

            strSQL = " Delete from RptDtlStk Where RptName='" & strRptName & "' and UserName='" & gStrSystemName & "' "
            If Not clsSVT.execSQLStatement(strSQL) Then
                Exit Function
            End If

            strSQL = "Insert Into RptHdrStk (RptName, UserName, Compname, Address, PriceVariation, HdrText, FilterText, "
            strSQL &= "Fld1, Fld2, Fld3, Fld4, Fld5, "
            strSQL &= "Fld6, Fld7, Fld8, Fld9, Fld10, "
            strSQL &= "Fld11, Fld12, Fld13, Fld14, Fld15, "
            strSQL &= "Fld16, Fld17, Fld18, Fld19, Fld20) "
            strSQL &= " Values "

            strSQL2 = "Insert Into RptDtlStk (RptName, UserName, Fld1, Fld2, Fld3, Fld4, Fld5, "
            strSQL2 &= "Fld6, Fld7, Fld8, Fld9, Fld10, "
            strSQL2 &= "Fld11, Fld12, Fld13, Fld14, Fld15, "
            strSQL2 &= "Fld16, Fld17, Fld18, Fld19, Fld20)  "

            Dim k As Int32 = 0

            For p As Int32 = 0 To .Cols.Count - 1
                If Not .Cols(p).Visible Then
                    k = k + 1
                End If
            Next

            For i As Int32 = 1 To .Rows.Count - 1
                If i = 1 Then
                    strSQL &= "('" & strRptName & "', '" & gStrSystemName & "','" & gSettings.gCompName & "',"
                    strSQL &= "'" & gSettings.gCompAdd1 & vbNewLine & gSettings.gCompAdd2 & vbNewLine & gSettings.gCompAdd3 & vbNewLine & gSettings.gCompAdd4 & "',"
                    strSQL &= "'', 'Stock Level As On " & getDateFormatInInsert(dtpFrom.Value) & "',"
                    strSQL &= "'',"
                End If

                strSQL2 &= " Select '" & strRptName & "','" & gStrSystemName & "',"

                For j As Int32 = 0 To 19 + k
                    If j <= .Cols.Count - 1 Then
                        If .Cols(j).Visible Then
                            If i = 1 Then
                                strSQL &= " '" & CStr(.GetData(0, j) & "") & "',"
                            End If
                            strSQL2 &= " '" & CStr(.GetData(i, j) & "") & "',"
                        End If
                    Else
                        If i = 1 Then
                            strSQL &= " '',"
                        End If
                        strSQL2 &= " '',"
                    End If
                Next
                strSQL2 = Mid(strSQL2, 1, Len(strSQL2) - 1)
                strSQL2 &= " Union All"
            Next

            strSQL = Mid(strSQL, 1, Len(strSQL) - 1) & ")"
            strSQL2 = Mid(strSQL2, 1, Len(strSQL2) - 9)

            If Not clsSVT.execSQLStatement(strSQL) Then
                Exit Function
            End If

            If Not clsSVT.execSQLStatement(strSQL2) Then
                Exit Function
            End If
        End With
        TransferGridData = True
    End Function

    Private Sub PrintData(Optional ByVal argBlnEmail As Boolean = False)
        Dim strSelFormula As String = ""
        Dim strFileName As String = RemoveSplChar(strRptName) & Date.Now.Ticks & ".pdf"

        strSelFormula = "{RptHdrStk.Rptname} = '" & strRptName & "' and {RptHdrStk.UserName} = '" & gStrSystemName & "' "

        Try
            Dim frmv As New frmUtlReportViewer
            With frmv
                .StartPosition = FormStartPosition.CenterScreen
                If argBlnEmail Then
                    If IO.File.Exists(gstrRptPath & "Temp\" & strFileName) Then
                        IO.File.Delete(gstrRptPath & "Temp\" & strFileName)
                    End If
                End If
                .showRptFile(Me.Text, "RptStkBal.rpt", strSelFormula, Nothing, argBlnEmail, True, False, "", 1, blnExportToPDF:=True, strPdfPath:=gstrRptPath & "Temp\" & strFileName)
                If Not argBlnEmail Then
                    .Show()
                Else
                    If gSmtpMailID = "" Then
                        MsgBox("SMTP Configuration Empty....", MsgBoxStyle.Exclamation, Me.Text)
                        Exit Sub
                    End If

                    If gStrRptEmailID = "" Then
                        MsgBox("Please configure email 'TO' Address..", MsgBoxStyle.Exclamation, Me.Text)
                        Exit Sub
                    End If


                    Dim Smtp_Server As New Net.Mail.SmtpClient
                    Dim e_mail As New Net.Mail.MailMessage()
                    Smtp_Server.UseDefaultCredentials = False
                    Smtp_Server.Credentials = New Net.NetworkCredential(CStr(gSmtpMailID & ""), CStr(gSmtpMailPwd & ""))
                    Smtp_Server.Port = CStr(gSmtpServerPort & "")
                    If CStr(gSmtpEnableSSL & "") = "Y" Then
                        Smtp_Server.EnableSsl = True
                    Else
                        Smtp_Server.EnableSsl = False
                    End If
                    Smtp_Server.Host = CStr(gSmtpHostName & "")
                    e_mail.From = New Net.Mail.MailAddress(CStr(gSmtpMailID & ""))
                    e_mail.To.Add(gStrRptEmailID)
                    e_mail.Subject = "Stock Balance Report"
                    e_mail.IsBodyHtml = False
                    e_mail.Body = "Automatic Email Pls don't reply....."
                    Dim attachment As Net.Mail.Attachment
                    attachment = New System.Net.Mail.Attachment(gstrRptPath & "Temp\" & strFileName)
                    e_mail.Attachments.Add(attachment)
                    Smtp_Server.Send(e_mail)
                    MsgBox("Email Sent....", MsgBoxStyle.Information, Me.Text)
                End If
            End With
        Catch ex As Exception
            MsgBox(ex.Message(), MsgBoxStyle.Exclamation, Me.Text)
        End Try
    End Sub

    Private Function getPkgList() As String
        getPkgList = ""
        Dim strItemList As String = ""
        For i = 1 To vsfPkgItem.Rows.Count - 1
            strItemList &= "'" & Trim(vsfPkgItem.GetData(i, vPkgName)) & "',"
        Next
        If strItemList <> "" Then strItemList = Mid(strItemList, 1, Len(strItemList) - 1)
        Return strItemList
    End Function

    Private Function getItemList() As String
        getItemList = ""
        Dim strItemList As String = ""
        For i = 1 To vsfItem.Rows.Count - 1
            strItemList &= Val(vsfItem.GetData(i, vItemID)) & ","
        Next
        If strItemList <> "" Then strItemList = Mid(strItemList, 1, Len(strItemList) - 1)
        Return strItemList
    End Function

    Private Sub optBlwMin_CheckedChanged(sender As Object, e As EventArgs) Handles optBlwMin.CheckedChanged

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