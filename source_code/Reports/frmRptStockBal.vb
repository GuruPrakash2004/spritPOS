Imports System.Data.oledb
Imports C1.Win.C1FlexGrid
Imports Microsoft.Office.Interop

Public Class frmRptStockBal
    Dim clsSVT As New svtLibrary

    Public strList As String
    Dim strItemGroupList As String
    Dim strItemList As String
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

    Const vCBItemGrpName = 0
    Const vCBItemBrdName = 1
    Const vCBItemName = 2
    Const vCBUOM = 3
    Const vCBOpQty = 4
    Const vCBRcptQty = 5
    Const vCBIssQty = 6
    Const vCBClQty = 7
    Const vCBClValue = 8
    Const vCBRate = 9

    'Const vCBBPercase = 9
    'Const vCBinCase = 10
    Const vCBTotCol = 10

    Const vGrpCategory = 0
    Const vGrpItemGrpName = 1
    Const vGrpOPValue = 2
    Const vGrpClValue = 3
    Const vGrpTotCol = 4

    Const vDetGrpName = 0
    Const vDetBrdName = 1
    Const vDetItemcode = 2
    Const vDetItemName = 3
    Const vDetItemID = 4
    Const vDetUOM = 5
    Const vDetHSN = 6
    Const vDetRate = 7
    Const vDetRatePerCase = 8
    Const vDetOPQty = 9
    Const vDetOPValue = 10

    Const vDetOPOREDQty = 11
    Const vDetOPTotValue = 12

    Const vDetPBQty = 13
    Const vDetPBValue = 14

    Const vDetPBAOREDQty = 15
    Const vDetPBTotValue = 16

    Const vDetSRQty = 17
    Const vDetSRValue = 18


    Const vDetSRAROEDQty = 19
    Const vDetSRTotValue = 20

    Const vDetInvQty = 21
    Const vDetInvRate = 22
    Const vDetInvValue = 23

    Const vDetInvAROEDQty = 24
    Const vDetInvTotValue = 25

    Const vDetSDQty = 26
    Const vDetSDValue = 27


    Const vDetSDAROEDQty = 28
    Const vDetSDTotValue = 29

    Const vDetPRQty = 30
    Const vDetPRValue = 31

    Const vDetPRAROEDQty = 32
    Const vDetPRTotValue = 33

    Const vDetSAQty = 34
    Const vDetSAValue = 35

    Const vDetSAAROEDQty = 36
    Const vDetSATotValue = 37

    Const vDetClQty = 38
    Const vDetClValue = 39

    Const vDetClAROEDQty = 40
    Const vDetClTotValue = 41

    Const vDetBPercase = 42
    Const vDetInCase = 43
    Const vDetTotal = 44

    Const vPKItemGrpName = 0
    Const vPKItemBrdName = 1
    Const vPKItemName = 2
    Const vPKTotQty = 3

    Const vRIItemGrpName = 0
    Const vRIItemBrdName = 1
    Const vRIItemName = 2
    Const vRIOPQty = 3
    Const vRIRcptQty = 4
    Const vRIIssQty = 5
    Const vRIClQty = 6
    Const vRIClValue = 7
    Const vRIRate = 8

    Dim dtStockRateDate As Date
    Dim strRptName As String = ""
    Dim strCrysRptName As String = ""
    Dim intTotRec As Int32
    Dim blnChanged As Boolean = False

    Const vWarItemID = 0
    Const vWarDocDate = 1
    Const vWarDocType = 2
    Const vWarRowID = 3
    Const vWarDocQty = 4
    Const vWarRate = 5
    Const vWarDocValue = 6
    Const vWarClBal = 7
    Const vWarClValue = 8

    Dim strBrandReq As String = ""
    Public gBlnAutoEmail As Boolean = False
    Public strEmailID As String = ""
    Public strEmailSrvrType As String = ""
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        frmMain.CloseSelectedTabPage(Me)
    End Sub

    Private Function ValidateFlds() As Boolean
        ValidateFlds = False

        If dtpFrom.Value < CDate(gSettings.gCompStDt) Then
            MsgBox("From date cannot be less than " & gSettings.gCompStDt, MsgBoxStyle.Critical, Me.Text)
            Exit Function
        End If

        If dtpTo.Value > CDate(gSettings.gCompEndDt) Then
            MsgBox("To date cannot be greater than " & gSettings.gCompEndDt, MsgBoxStyle.Critical, Me.Text)
            Exit Function
        End If

        'check the date range
        If CDate(dtpTo.Value) < CDate(dtpFrom.Value) Then
            MsgBox("Invalid Date Range!", MsgBoxStyle.Critical, Me.Text)
            Exit Function
        End If

        If chkItemGroup.Checked Then
            If strItemGroupList = "" Then
                MsgBox("Item Group Selection required from the list ", MsgBoxStyle.Critical, Me.Text)
                Exit Function
            End If
        End If
        ValidateFlds = True
    End Function


    Private Sub btnView_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView.Click
        Call showWaitCursor()
        If Not ValidateFlds() Then
            Call showNormalCursor()
            Exit Sub
        End If
        btnPrint.Visible = False
        If optCBal.Checked Then
            If chkValues.Checked Then btnPrint.Visible = True
            Call ViewClData()
        End If
        If optClDetails.Checked Then
            If chkValues.Checked Then btnPrint.Visible = True
            Call ViewClDetailed()
        End If
        If optPkgWiseBal.Checked Then Call ViewPkgData()
        If optGrpSummary.Checked Then
            Call ViewGrpSmryData()
        End If
        'RefreshGrid(False)
        Call showNormalCursor()
        blnChanged = False
    End Sub

    Private Sub RemoveVsfRows(argVSF As C1.Win.C1FlexGrid.C1FlexGrid)
        Dim i As Integer
        If optClDetails.Checked Then
            With argVSF
Repeat:
                For i = 4 To (.Rows.Count - 1)
                    If chkZeroBal.Checked Then
                        If Val(.GetData(i, vDetOPQty)) = 0 And Val(.GetData(i, vDetPBQty)) = 0 And Val(.GetData(i, vDetSRQty)) = 0 And Val(.GetData(i, vDetInvQty)) = 0 And Val(.GetData(i, vDetSDQty)) = 0 And Val(.GetData(i, vDetPRQty)) = 0 And Val(.GetData(i, vDetClQty)) = 0 Then
                            .RemoveItem(i)
                            GoTo Repeat
                        End If
                    End If
                Next i
            End With
        ElseIf optCBal.Checked Then
            With argVSF
RepeatCL:
                For i = 4 To (.Rows.Count - 1)
                    If chkZeroBal.Checked Then
                        If Val(.GetData(i, vCBOpQty)) = 0 And Val(.GetData(i, vCBRcptQty)) = 0 And Val(.GetData(i, vCBIssQty)) = 0 And Val(.GetData(i, vCBClQty)) = 0 And Val(.GetData(i, vCBClValue)) = 0 Then
                            .RemoveItem(i)
                            GoTo RepeatCL
                        End If
                    End If
                Next i
            End With

        Else

            With argVSF
CRepeat:
                For i = 4 To (.Rows.Count - 1)
                    If chkZeroBal.Checked Then
                        If Val(.GetData(i, .Cols.Count - 2)) = 0 Then
                            .RemoveItem(i)
                            GoTo CRepeat
                        End If
                    End If

                Next i
            End With
        End If
    End Sub

    Private Sub frmRptStock_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.KeyPreview = True
        strBrandReq = getGsettings("INVENTORY", "BRANDREQ", "")
        Call initFormSize(Me, Me.Text, False, 958, 579, 0, 0)
        Call initFormVsfDet(Me, Me.Text, vsfItem, "ITEM")
        Call initFormVsfDet(Me, Me.Text, vsfPkgItem, "PKG")
        Call fillFormProperties(Me)
        Call initGridStyles(Me)
        Call initScreen()
        btnPrint.Visible = False
        optPkgWiseBal_CheckedChanged(Nothing, Nothing)
        gStrRptEmailID = getGsettings("REPORTS", "RPTEMAILIDS", "")
        Call hideWar()

        If gBlnAutoEmail Then
            Dim strFilePath As String
            optSalesRate.Checked = True
            optClDetails.Checked = True
            chkTotGW.Checked = True
            btnView.PerformClick()
            'strEmailID = "akshay@softvent.com"
            If strEmailID = "" Then Exit Sub

            strFilePath = gSysTempPath & "Stock Balance Report as on_" & Format(dtpTo.Value, "ddMMMyyyy") & ".xlsx"
            'ExportToXL(vsfView, argFileName:=strFilePath)

            vsfView.SaveExcel(strFilePath, C1.Win.C1FlexGrid.FileFlags.AsDisplayed Or C1.Win.C1FlexGrid.FileFlags.IncludeFixedCells)

            If strEmailSrvrType.ToUpper = "OUTLOOK" Then
                strEmailID = Replace(strEmailID, ",", ";")
                If Not sendOutLookMail(strEmailID, "", "Stock Balance Report as on " & Format(dtpTo.Value, "dd MMM yyyy"), "Please Find the attached file of Stock Balance Report as on " & Format(dtpTo.Value, "dd MMM yyyy"), argAttFileName:=strFilePath) Then
                    Me.Close()
                End If
            Else
                strEmailID = Replace(strEmailID, ";", ",")
                If Not sendGenMail(strEmailID, "", "Stock Balance Report as on " & Format(dtpTo.Value, "dd MMM yyyy"), "Please Find the attached file of Stock Balance Report as on " & Format(dtpTo.Value, "dd MMM yyyy"), strFilePath) Then
                    Me.Close()
                End If
            End If


        End If
        'If gStrDBName.ToUpper = "VLQMSDROPSJP" Then
        If Format(getSystemDate(), "yyyy-MM-dd") = CDate("2026-05-30") Then
            btnStk.Visible = True
        Else
            btnStk.Visible = False
        End If
        'End If
    End Sub

    Private Sub initScreen()

        Dim strGrpTot As String = ""
        strGrpTot = INIGet("GroupTotal", "", "N")

        If strGrpTot = "Y" Then
            chkTotGW.Checked = True
        End If


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
            .Cols.Count = vCBTotCol
            .SetData(0, vCBItemGrpName, "Item Group Name")
            .SetData(0, vCBItemBrdName, "Item Brand Name")
            .SetData(0, vCBItemName, "Item Name")
            .SetData(0, vCBUOM, "Pkg")
            .SetData(0, vCBOpQty, "Opening Quantity")
            .SetData(0, vCBRcptQty, "Receipt Quantity")
            .SetData(0, vCBIssQty, "Issued Quantity")
            .SetData(0, vCBClQty, "Closing Quantity")
            .SetData(0, vCBClValue, "Closing Value")

            .SetData(0, vCBRate, "Rate")
            '.SetData(0, vCBBPercase, "Bottle Percase")
            '.SetData(0, vCBinCase, "In Case")
        End With
        ' SetHideCol(vsfView, True, vCBBPercase)
        Call SetFlexColor(vsfView, "", vCBItemGrpName, vCBItemBrdName, vCBItemName, vCBOpQty, vCBClQty, vCBClValue, vCBRate, vCBRcptQty, vCBIssQty, vCBUOM)
        'Changed by Surabhi
        dtpFrom.Value = getSystemDate()
        'dtpFrom.Value = getFirstDateInMonth(getSystemDate)
        dtpTo.Value = getSystemDate()
    End Sub

    Private Sub UpdateFixedRowForDocXl(ByVal xlwb As Excel.Workbook, ByVal xlws As Excel.Worksheet, ByVal argtext As String, ByVal intRowno As Int32, ByVal intEndcol As Int32, ByVal intFontsize As Int32, ByVal blnLine As Boolean, ByVal blnBackColor As Boolean)
        With xlws
            .Range("A" & intRowno & ":" & NumToAlphabet(intEndcol) & intRowno).Merge()
            .Range("A" & intRowno & ":" & NumToAlphabet(intEndcol) & intRowno).Value = argtext
            ' .Range("A" & intRowno & ":" & NumToAlphabet(intEndcol) & intRowno).Style.Equals(HorizontalAlignment.Center)
            If blnBackColor Then
                Dim style As Excel.Style = xlwb.Styles.Add("NewStyle")
                style.Interior.Color = ColorTranslator.ToOle(Color.LightGray)
                style.Interior.Color = ColorTranslator.ToOle(Color.LightGray)
                .Range("A" & intRowno & ":" & NumToAlphabet(intEndcol) & intRowno).Style = "NewStyle"
            End If
            .Range("A" & intRowno & ":" & NumToAlphabet(intEndcol) & intRowno).Font.Size = intFontsize
            .Range("A" & intRowno & ":" & NumToAlphabet(intEndcol) & intRowno).Font.Bold = True
            '.Range("A" & intRowno & ":" & NumToAlphabet(intEndcol) & intRowno).VerticalAlignment = XLAlignmentVerticalValues.Center
            If blnLine Then
                .Range("A" & intRowno & ":" & NumToAlphabet(intEndcol) & intRowno).BorderAround2(Excel.XlBorderWeight.xlHairline)
            End If
        End With
    End Sub

    Private Sub btnExportToXL_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExportToXL.Click
        Call ExportToXL(vsfView,,,, True, "Stock Balance Report", "For the period " & getDateFormatInInsert(dtpFrom.Value) & " to " & getDateFormatInInsert(dtpTo.Value))
    End Sub

    Private Sub chkItemGroup_CheckedChanged(sender As Object, e As System.EventArgs) Handles chkItemGroup.CheckedChanged
        Dim strSQL As String
        If chkItemGroup.Checked = False Then
            strList = ""
            strItemGroupList = ""
            Exit Sub
        End If

        Dim frmSel As New frmUtlMultiSelection
        'GroupOrderBy 
        strSQL = "Select distinct 0 as sel, ItemGroupID, ItemGroupCode,GroupOrderBy from MstItemGroup where ItemGroupID > 0 and "
        strSQL = strSQL & " InActive = 'N' ORDER BY GroupOrderBy  "
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
            If optCBal.Checked Then Call SaveActFormVsfDet(Me, Me.Text, "VIEWCLBAL", vsfView)
            If optClDetails.Checked Then Call SaveActFormVsfDet(Me, Me.Text, "VIEWCLDETBAL", vsfView)
            If optPkgWiseBal.Checked Then Call SaveActFormVsfDet(Me, Me.Text, "VIEWPKGBAL", vsfView)
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

    Private Sub txtValBelow_KeyPress(sender As Object, e As KeyPressEventArgs)
        If Not KeyValidate("Integer", e.KeyChar) Then
            e.Handled = True
        End If
    End Sub

    Private Sub btnItemCodelike_Click(sender As Object, e As EventArgs)
        Dim strSql As String
        strSql = "Select  ItemID ,ItemCode, ItemName,UOMName "
        strSql = strSql & " from MstItem i,Mstuom u"
        strSql = strSql & " Where i.InActive = 'N' and u.uomid=i.uomid"
        strSql = strSql & " order by 2"


        Dim frmDisplay As New frmUtlSingleSel
        With frmDisplay
            .Text = "Select Short Name... "
            'Call .FillViewVsf(Me, strSql, txtItemCode, 2, 0)
            .ShowDialog()
            .Close()
        End With
    End Sub

    Private Sub btnItemName_Click(sender As Object, e As EventArgs)
        Dim strSql As String
        strSql = "Select  ItemID , ItemName, ItemCode,UOMName "
        strSql = strSql & " from MstItem i,Mstuom u"
        strSql = strSql & " Where i.InActive = 'N' and u.uomid=i.uomid "
        strSql = strSql & " order by 2"


        Dim frmDisplay As New frmUtlSingleSel
        With frmDisplay
            .Text = "Select Short Name... "
            'Call .FillViewVsf(Me, strSql, txtItemName, 2, 0)
            .ShowDialog()
            .Close()
        End With
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

    Private Sub vsfView_DoubleClick(sender As Object, e As EventArgs) Handles vsfView.DoubleClick
        If vsfView.Col = 0 Then
            If vsfView.Cols(0).AllowMerging = False Then
                vsfView.Cols(0).AllowMerging = True
            Else
                vsfView.Cols(0).AllowMerging = False
            End If
        End If
    End Sub

    Private Sub ViewClData()
        Dim strSQL As String = ""
        Dim strItemList As String = ""
        Dim rs As oledbDataReader
        With vsfView
            .Rows.Count = 1
            .Rows.Fixed = 1
            .Cols.Count = vCBTotCol
            .SetData(0, vCBItemGrpName, "Item Group Name")
            .SetData(0, vCBItemBrdName, "Item Brand Name")
            .SetData(0, vCBItemName, "Item Name")
            .SetData(0, vCBUOM, "PKG")
            .SetData(0, vCBOpQty, "Opening Quantity")
            .SetData(0, vCBRcptQty, "Receipt Quantity")
            .SetData(0, vCBIssQty, "Issued Quantity")
            If chkQtyInLtr.Checked Then
                .SetData(0, vCBClQty, "Closing Quantity-Ltr")
                .SetData(0, vCBOpQty, "Opening Quantity-Ltr")
                .SetData(0, vCBRcptQty, "Receipt Quantity-Ltr")
                .SetData(0, vCBIssQty, "Issued Quantity-Ltr")
            Else
                .SetData(0, vCBClQty, "Closing Quantity")
            End If
            .SetData(0, vCBClValue, "Closing Value")

            If optPurRate.Checked Then
                .SetData(0, vCBRate, "Rate [Last Purchase]")
            Else
                .SetData(0, vCBRate, "Rate [MRP]")
            End If
            '.SetData(0, vCBBPercase, "Bootle Percase")
            '.SetData(0, vCBinCase, "In Case")
            'If chkQtyInLtr.Checked Then
            '    vsfView.Cols(vCBinCase).Visible = False
            'Else
            '    vsfView.Cols(vCBinCase).Visible = True
            'End If
            If strBrandReq <> "Y" Then
                vsfView.Cols(vCBItemBrdName).Visible = False
            End If
            If Not chkValues.Checked Then

                vsfView.Cols(vCBClValue).Visible = False
                vsfView.Cols(vCBRate).Visible = False

            Else
                vsfView.Cols(vCBClValue).Visible = True
                vsfView.Cols(vCBRate).Visible = True

            End If
        End With

        Dim strRate As String = "PBRate"
        If chkTax.Checked Then
            strRate = "Round(NetAmt/PBQty,2)"
        End If

        strSQL = "Select PkgItem, GroupOrderBy, PkgOrderBy,ItemGroupID,ItemGroupName, BrandName,ItemID, ItemName, PkgCode, Sum(OpBalQty) as OpBalQty, Sum(OpBalQtyLtr) as OpBalQtyLtr,"
        strSQL &= "Sum(clQty) as clQty, sum(RcptQty) as RcptQty, Sum(RcptQtyLtr) as RcptQtyLtr, Sum(IssQty) as IssQty, sum(IssQtyLtr) as IssQtyLtr, Sum(clQtyLtr) as clQtyLtr, "

        If optPurRate.Checked Then
            strSQL = strSQL & " isnull((Select top 1 " & strRate & " from vwPBItemLastPrice v where a.ItemID = v.itemid  And pbdate <= " & getDateFormatInCond(dtpTo.Value) & " order by Slno),0) as StkRate,BotPerCases "
        Else
            strSQL = strSQL & " a.MRP as StkRate,BotPerCases  "
        End If


        strSQL &= "from ("
        If UCase(gStrDBName) = "VLQMSCLH" Then
            strSQL &= "Select case when upper(i.LIQTYPE)='OTHERS' then i.PMPURRATE else i.MRP end as MRP, i.Pkgitem, g.ItemGroupID,GroupOrderBy,PkgOrderBy,ItemGroupName, B.BrandName,s.itemid, ItemName, PkgCode,"
        Else
            strSQL &= "Select i.MRP, i.Pkgitem, g.ItemGroupID,GroupOrderBy,PkgOrderBy, ItemGroupName, BrandName,s.itemid, ItemName, PkgCode,"
        End If

        strSQL &= " Sum(Qty*StkMultiplier) as OpBalQty, sum(LtrQty*StkMultiplier) as OpBalQtyLtr,"
        strSQL &= " 0 as RcptQty, 0 as RcptQtyLtr, 0 as IssQty, 0 as IssQtyLtr, Sum(Qty*StkMultiplier) as clQty, sum(LtrQty*StkMultiplier) as clQtyLtr ,BotPerCases"
        strSQL = strSQL & " from MstItemGroup g, MstItem i,MstPkg u, UtlStockList s,mstBrand b"
        strSQL = strSQL & " where s.ItemID = i.ItemID"
        strSQL = strSQL & " And i.PkgID = u.PkgID"
        strSQL = strSQL & " And i.BrandID = b.BrandID"
        strSQL = strSQL & " And i.ItemGroupID = g.ItemGroupId "
        strSQL = strSQL & " And DocDate < " & getDateFormatInCond(dtpFrom.Value)
        strSQL = strSQL & " And i.Inactive='N'"

        strItemList = ""
        strItemList = getItemList()
        If strItemList <> "" Then
            strSQL = strSQL & " And i.ItemID In (" & strItemList & ") "
        End If
        If chkItemGroup.Checked Then
            If strItemGroupList <> "" Then
                strSQL = strSQL & " and i.ItemGroupID in (" & strItemGroupList & ") "
            End If
        End If
        strItemList = ""
        strItemList = getPkgList()
        If strItemList <> "" Then
            strSQL = strSQL & " And i.PKGITEM In (" & strItemList & ") "
        End If
        If UCase(gStrDBName) = "VLQMSCLH" Then
            strSQL = strSQL & " group by i.PkgItem,i.LIQTYPE,i.MRP, g.ItemGroupID,GroupOrderBy,PkgOrderBy,ItemGroupName,B.BrandName,s.itemid, ItemName, PkgCode, i.PMPURRATE,BotPerCases "
        Else
            strSQL = strSQL & " group by i.PkgItem, g.ItemGroupID,GroupOrderBy,PkgOrderBy,ItemGroupName, s.itemid, ItemName, PkgCode, i.MRP,BotPerCases ,b.BrandName"
        End If

        strSQL &= " Union All "

        If UCase(gStrDBName) = "VLQMSCLH" Then
            strSQL &= "Select case when upper(i.LIQTYPE)='OTHERS' then i.PMPURRATE else i.MRP end as MRP, i.Pkgitem, g.ItemGroupID,GroupOrderBy,PkgOrderBy,ItemGroupName,B.BrandName, s.itemid, ItemName, PkgCode,"
        Else
            strSQL &= " Select i.MRP, i.Pkgitem, g.ItemGroupID,GroupOrderBy,PkgOrderBy,ItemGroupName,b.brandName, s.itemid,ItemName,PkgCode,"
        End If

        strSQL &= " 0 as OpBalQty, 0 as OpBalQtyLtr,"
        strSQL &= " Sum(Qty*StkMultiplier) as RcptQty, sum(LtrQty*StkMultiplier) as RcptQtyLtr, "
        strSQL &= " 0 as IssQty, 0 as IssQtyLtr, "
        strSQL &= " Sum(Qty*StkMultiplier) as clQty, sum(LtrQty*StkMultiplier) as clQtyLtr,BotPerCases "
        strSQL = strSQL & " from MstItemGroup g, MstItem i,MstPkg u, UtlStockList s,mstBrand b"
        strSQL = strSQL & " where s.ItemID = i.ItemID"
        strSQL = strSQL & " And i.PkgID = u.PkgID"
        strSQL = strSQL & " And i.BrandID = b.BrandID"
        strSQL = strSQL & " And i.ItemGroupID = g.ItemGroupId "
        strSQL = strSQL & " And StkMultiplier = 1"
        strSQL = strSQL & " And DocDate between " & getDateFormatInCond(dtpFrom.Value)
        strSQL = strSQL & " And  " & getDateFormatInCond(dtpTo.Value)
        strSQL = strSQL & " And i.Inactive='N'"

        strItemList = ""
        strItemList = getItemList()
        If strItemList <> "" Then
            strSQL = strSQL & " And i.ItemID In (" & strItemList & ") "
        End If
        If chkItemGroup.Checked Then
            If strItemGroupList <> "" Then
                strSQL = strSQL & " and i.ItemGroupID in (" & strItemGroupList & ") "
            End If
        End If
        strItemList = ""
        strItemList = getPkgList()
        If strItemList <> "" Then
            strSQL = strSQL & " And i.PKGITEM In (" & strItemList & ") "
        End If
        If UCase(gStrDBName) = "VLQMSCLH" Then
            strSQL = strSQL & " group by i.PkgItem, i.LIQTYPE,i.MRP,g.ItemGroupID,GroupOrderBy,PkgOrderBy,ItemGroupName,B.BrandName, s.itemid, ItemName, PkgCode, i.PMPURRATE,BotPerCases "
        Else
            strSQL = strSQL & " group by i.PkgItem, g.ItemGroupId,GroupOrderBy,PkgOrderBy, ItemGroupName,b.BrandNAme, s.itemid, ItemName, PkgCode, i.MRP,BotPerCases "
        End If
        strSQL = strSQL & " "

        strSQL &= " Union All "
        If UCase(gStrDBName) = "VLQMSCLH" Then
            strSQL &= "Select case when upper(i.LIQTYPE)='OTHERS' then i.PMPURRATE else i.MRP end as MRP, i.Pkgitem, g.ItemGroupID,GroupOrderBy,PkgOrderBy,ItemGroupName, B.BrandName,s.itemid, ItemName, PkgCode,"
        Else
            strSQL &= " Select i.MRP, i.PkgItem, g.ItemGroupID,GroupOrderBy,PkgOrderBy,ItemGroupName,B.BrandNAme , s.itemid,ItemName,PkgCode,"
        End If
        strSQL &= " 0 as OpBalQty, 0 as OpBalQtyLtr,"
        strSQL &= " 0 as RcptQty, 0 as RcptQtyLtr, "
        strSQL &= " Sum(Qty*StkMultiplier) as IssQty, sum(LtrQty*StkMultiplier) as IssQtyLtr, "
        strSQL &= " Sum(Qty*StkMultiplier) as clQty, sum(LtrQty*StkMultiplier) as clQtyLtr ,BotPerCases "
        strSQL = strSQL & " from MstItemGroup g, MstItem i,MstPkg u, UtlStockList s,MstBrand b"
        strSQL = strSQL & " where s.ItemID = i.ItemID"
        strSQL = strSQL & " And i.PkgID = u.PkgID"
        strSQL = strSQL & " And i.BrandID = b.BrandID"
        strSQL = strSQL & " And i.ItemGroupID = g.ItemGroupId "
        strSQL = strSQL & " And DocDate between " & getDateFormatInCond(dtpFrom.Value)
        strSQL = strSQL & " And  " & getDateFormatInCond(dtpTo.Value)
        strSQL = strSQL & " And StkMultiplier = -1"
        strSQL = strSQL & " And i.Inactive='N'"
        strItemList = ""
        strItemList = getItemList()

        If strItemList <> "" Then
            strSQL = strSQL & " And i.ItemID In (" & strItemList & ") "
        End If
        strItemList = ""
        strItemList = getPkgList()
        If strItemList <> "" Then
            strSQL = strSQL & " And i.PKGITEM In (" & strItemList & ") "
        End If
        If chkItemGroup.Checked Then
            If strItemGroupList <> "" Then
                strSQL = strSQL & " and i.ItemGroupID in (" & strItemGroupList & ") "
            End If
        End If
        If UCase(gStrDBName) = "VLQMSCLH" Then
            strSQL = strSQL & " group by i.PkgItem, i.LIQTYPE,i.MRP,g.ItemGroupID,GroupOrderBy,PkgOrderBy,ItemGroupName, B.BrandName,s.itemid, ItemName, PkgCode, i.PMPURRATE,BotPerCases "
        Else
            strSQL = strSQL & " group by i.PkgItem, g.ItemGroupID,GroupOrderBy,PkgOrderBy,ItemGroupName, s.itemid, ItemName, PkgCode, i.MRP ,BotPerCases,BrandName"
        End If

        strSQL = strSQL & ") a "

        strSQL = strSQL & " group by PkgItem, GroupOrderBy, PkgOrderBy,ItemGroupID, ItemGroupName, Itemid, ItemName, PkgCode, a.MRP ,BotPerCases,BrandName"

        If UCase(gStrDBName) = "VLQMSSVS" Then

            If chkZeroBal.Checked Then
                strSQL = strSQL & " having  Sum(clQty) > 0"
            Else
                'strSQL = strSQL & " having  Sum(clQty) > 0"
            End If
        End If

        'strSQL = strSQL & " order by GroupOrderBy,ItemGroupName,BrandName, Pkgitem, pkgorderby, StkRate Desc "
        strSQL = strSQL & " order by GroupOrderBy,ItemGroupName, Pkgitem, pkgorderby, StkRate Desc "

        rs = clsSVT.getDataReaderFromSQL(strSQL)

        If Not rs.HasRows Then
            rs.Close()
            Exit Sub
        End If

        With rs
            Do While .Read
                vsfView.Rows.Count = vsfView.Rows.Count + 1
                vsfView.SetData(vsfView.Rows.Count - 1, vCBItemGrpName, !ItemGroupName)
                vsfView.SetData(vsfView.Rows.Count - 1, vCBItemBrdName, !BrandName)
                'vsfView.SetData(vsfView.Rows.Count - 1, vCBItemName, !ItemGName)
                vsfView.SetData(vsfView.Rows.Count - 1, vCBItemName, !ItemName)
                vsfView.SetData(vsfView.Rows.Count - 1, vCBUOM, !PkgCode)
                If chkQtyInLtr.Checked = True Then
                    vsfView.SetData(vsfView.Rows.Count - 1, vCBOpQty, GetVal(rs!OpBalQtyLtr, ""))
                    vsfView.SetData(vsfView.Rows.Count - 1, vCBClQty, GetVal(rs!ClQtyLtr, ""))
                    vsfView.SetData(vsfView.Rows.Count - 1, vCBRcptQty, GetVal(rs!RcptQtyLtr, ""))
                    vsfView.SetData(vsfView.Rows.Count - 1, vCBIssQty, Math.Abs(GetVal(rs!IssQtyLtr, "")))
                Else
                    vsfView.SetData(vsfView.Rows.Count - 1, vCBOpQty, GetVal(!OpBalQty, ""))
                    vsfView.SetData(vsfView.Rows.Count - 1, vCBClQty, GetVal(!ClQty, ""))
                    vsfView.SetData(vsfView.Rows.Count - 1, vCBRcptQty, GetVal(!RcptQty, ""))
                    vsfView.SetData(vsfView.Rows.Count - 1, vCBIssQty, Math.Abs(GetVal(!IssQty, "")))
                End If
                If chkValues.Checked = True Then
                    vsfView.SetData(vsfView.Rows.Count - 1, vCBRate, Math.Round(GetVal(!StkRate, 0), 2))
                    vsfView.SetData(vsfView.Rows.Count - 1, vCBClValue, Math.Round(GetVal(!ClQty * !StkRate, ""), 2))

                End If
                'vsfView.SetData(vsfView.Rows.Count - 1, vCBBPercase, GetVal(!BotPerCases, ""))

                'Dim dblresult As Double = 0
                'Dim dblreminder As Double = 0

                'dblresult = vsfView.GetData(vsfView.Rows.Count - 1, vCBRcptQty) / vsfView.GetData(vsfView.Rows.Count - 1, vCBBPercase)

                'dblreminder = vsfView.GetData(vsfView.Rows.Count - 1, vCBRcptQty) Mod vsfView.GetData(vsfView.Rows.Count - 1, vCBBPercase)

                'vsfView.SetData(vsfView.Rows.Count - 1, vCBinCase, Math.Floor(dblresult) & " Cases " & dblreminder & " Bottles")

            Loop
            .Close()
        End With


        With vsfView

            If chkQtyInLtr.Checked = False Then

                .Cols(vCBClQty).Format = "#"
                .Cols(vCBOpQty).Format = "#"
                .Cols(vCBRcptQty).Format = "#"
                .Cols(vCBIssQty).Format = "#"
            Else
                .Cols(vCBClQty).Format = "0.000"
                .Cols(vCBOpQty).Format = "0.000"
                .Cols(vCBRcptQty).Format = "0.000"
                .Cols(vCBIssQty).Format = "0.000"

            End If
            RemoveVsfRows(vsfView)

            .SubtotalPosition = C1.Win.C1FlexGrid.SubtotalPositionEnum.BelowData
            If chkTotGW.Checked Then

                For k = vCBOpQty To vsfView.Cols.Count - 2
                    .Subtotal(C1.Win.C1FlexGrid.AggregateEnum.Sum, 0, 0, k)
                Next k

            End If

            For k = vCBOpQty To vsfView.Cols.Count - 2
                .Subtotal(C1.Win.C1FlexGrid.AggregateEnum.Sum, -1, -1, k)
            Next k
            .AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.RestrictAll
            .Cols(vCBItemGrpName).AllowMerging = True
        End With
        vsfView.AutoSizeCols()
        'Call initFormVsfDet(Me, Me.Text, vsfView, "VIEWCLBAL")

        lblTotRec.Text = "Total Records: " & vsfView.Rows.Count - 1
    End Sub

    Private Sub ViewPkgData()
        Dim strSQL As String = ""
        Dim rs As oledbDataReader
        With vsfView
            .Rows.Count = 1
            .Rows.Fixed = 1
            .Cols.Count = 5
            .SetData(0, vPKItemGrpName, "Item Group Name")
            .SetData(0, vPKItemBrdName, "Item Brand Name")
            .SetData(0, vPKItemName, "Main Item")
            .SetData(0, vPKTotQty, "Total Quantity")
            If strBrandReq <> "Y" Then
                vsfView.Cols(vPKItemBrdName).Visible = False
            End If
            If chkQtyInLtr.Checked Then .SetData(0, vPKTotQty, "Total Quantity(Ltr)")
        End With

        'fill Columns with PkgCode
        strSQL = "Select distinct PkgOrderBy, PkgCode"
        strSQL = strSQL & " from MstItemGroup g, MstItem i, UtlStockList s,MstPKG p,mstBrand b"
        strSQL = strSQL & " where s.ItemID = i.ItemID"
        strSQL = strSQL & " and i.PkgID = p.PKGID"
        strSQL = strSQL & " and i.BrandID = b.BrandID"
        strSQL = strSQL & " And i.ItemGroupID = g.ItemGroupId "
        strSQL = strSQL & " And docDate <= " & getDateFormatInCond(dtpTo.Value)
        strSQL = strSQL & " And i.Inactive='N'"
        If chkItemGroup.Checked Then
            If strItemGroupList <> "" Then
                strSQL = strSQL & " and i.ItemGroupID in (" & strItemGroupList & ") "
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


        strSQL = strSQL & " Order by PkgOrderBy,PkgCode"
        rs = clsSVT.getDataReaderFromSQL(strSQL)
        vsfView.Cols.Count = 3
        With rs
            Do While .Read
                vsfView.Cols.Count = vsfView.Cols.Count + 1
                vsfView.SetData(0, vsfView.Cols.Count - 1, !PkgCode)
            Loop
            .Close()
        End With


        strSQL = "Select ItemGroupName,GroupOrderBy, BrandName,PkgOrderBy,PkgItem,PkgCode, sum(Qty*StkMultiplier) as clQty, sum(LtrQty*StkMultiplier) as clQtyLtr"
        strSQL = strSQL & " from MstItemGroup g, MstItem i, UtlStockList s,MstPKG p,mstBrand b"
        strSQL = strSQL & " where s.ItemID = i.ItemID"
        strSQL = strSQL & " and i.PkgID = p.PKGID"
        strSQL = strSQL & " and i.BrandID = b.BrandID"
        strSQL = strSQL & " And i.ItemGroupID = g.ItemGroupId "
        strSQL = strSQL & " And i.Inactive='N'"
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
        strSQL = strSQL & " group by ItemGroupName,BrandName, GroupOrderBy,PkgOrderBy, PkgItem,PkgCode"
        strSQL = strSQL & " Order by GroupOrderBy, pkgItem,PkgOrderBy,PkgCode"
        rs = clsSVT.getDataReaderFromSQL(strSQL)

        If Not rs.HasRows Then
            rs.Close()
            Exit Sub
        End If

        With rs
            Do While .Read
                vsfView.Rows.Count = vsfView.Rows.Count + 1
                vsfView.SetData(vsfView.Rows.Count - 1, vPKItemGrpName, !ItemGroupName)
                vsfView.SetData(vsfView.Rows.Count - 1, vPKItemBrdName, !BrandName)
                vsfView.SetData(vsfView.Rows.Count - 1, vPKItemName, !PkgItem)

                If chkQtyInLtr.Checked = True Then
                    vsfView.SetData(vsfView.Rows.Count - 1, vPKTotQty, Val(vsfView.GetData(vsfView.Rows.Count - 1, vPKTotQty)) + GetVal(!clQtyLtr, ""))
                Else
                    vsfView.SetData(vsfView.Rows.Count - 1, vPKTotQty, Val(vsfView.GetData(vsfView.Rows.Count - 1, vPKTotQty)) + GetVal(!clQty, ""))
                End If
                For i = 1 To vsfView.Cols.Count - 1
                    If vsfView.GetData(0, i) = !PkgCode Then
                        If chkQtyInLtr.Checked = True Then
                            vsfView.SetData(vsfView.Rows.Count - 1, i, !clQtyLtr)
                        Else
                            vsfView.SetData(vsfView.Rows.Count - 1, i, !clQty)
                        End If
                        Exit For
                    End If
                Next
            Loop
            .Close()
        End With

        With vsfView
            .SubtotalPosition = C1.Win.C1FlexGrid.SubtotalPositionEnum.BelowData

            If chkTotGW.Checked Then
                For k = vPKTotQty To vsfView.Cols.Count - 1
                    .Subtotal(C1.Win.C1FlexGrid.AggregateEnum.Sum, 0, 0, k)
                Next k
            End If

            For k = vPKTotQty To vsfView.Cols.Count - 1
                .Subtotal(C1.Win.C1FlexGrid.AggregateEnum.Sum, -1, -1, k)
            Next k
            .AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.RestrictAll
            .Cols(vCBItemGrpName).AllowMerging = True
        End With
        vsfView.AutoSizeCols()
        'Call initFormVsfDet(Me, Me.Text, vsfView, "VIEWPKGBAL")
    End Sub

    Private Sub optPkgWiseBal_CheckedChanged(sender As Object, e As EventArgs) Handles optPkgWiseBal.CheckedChanged, optCBal.CheckedChanged
        Call hideWar()

        If optPkgWiseBal.Checked Then
            chkValues.Visible = False
            'chkRate.Visible = False
            chkValues.Checked = False
            'chkRate.Checked = False
        Else
            chkValues.Visible = True
            'chkRate.Visible = True
        End If
        blnChanged = True
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

        If optClDetails.Checked Then
            If Not TransferGridData() Then
                Exit Sub
            End If
            PrintData(False, "{RptStk.UserName} = '" & gStrSystemUserName & "'")
        Else
            PrintClosingQty()
            PrintData(False, "{RptHdrStk.UserName} ='" & gStrSystemUserName & "' and {RptHdrStk.RptName} = 'Stock Balance Report'", "RptStkBalDtl.Rpt")
        End If

    End Sub

    Private Sub PrintClosingQty()
        Dim strSQL As String
        Dim strRptName As String = "Stock Balance Report"
        Dim i As Int32, j As Int32, x As Int32

        With vsfView
            strSQL = "Delete from RptHdrStk where UserName = '" & gStrSystemUserName & "' and RptName = '" & Trim(strRptName) & "'"
            clsSVT.execSQLStatement(strSQL)

            strSQL = "Delete from RptDtlStk where UserName = '" & gStrSystemUserName & "' and RptName = '" & Trim(strRptName) & "'"
            clsSVT.execSQLStatement(strSQL)

            strSQL = "Insert Into RptHdrStk (RptName,UserName,Fld1,Fld2,Fld3,Fld4,Fld5,Fld6,Fld7,Fld8,Fld9"
            strSQL &= " ,Fld10,Fld11,Fld12,Fld13,Fld14,Fld15,Fld16,Fld17,Fld18,Fld19,Fld20,Compname,Address,HdrText,Pricevariation,FilterText) values ("
            strSQL &= "'" & Trim(strRptName) & "',"
            strSQL &= "'" & gStrSystemUserName & "'"
            For j = 0 To .Cols.Count - 1
                If j <= 20 Then
                    strSQL &= ",'" & .GetData(0, j) & "'"
                End If
            Next j
            For x = j + 1 To 20
                strSQL &= ",''"
            Next x
            strSQL &= ",'" & gSettings.gCompName & "'"
            strSQL &= ",'" & gSettings.gCompAdd1 & " " & gSettings.gCompAdd2 & " " & gSettings.gCompAdd3 & "'"
            strSQL &= ",'Stock Balance Report From " & getDateFormatInInsert(dtpFrom.Value) & " and " & getDateFormatInInsert(dtpTo.Value) & "'"
            strSQL &= ",''"
            strSQL &= ",'')"
            clsSVT.execSQLStatement(strSQL)

            For i = 1 To .Rows.Count - 2
                strSQL = "Insert Into RptDtlStk (RptName,UserName,Fld1,Fld2,Fld3,Fld4,Fld5,Fld6,Fld7,Fld8,Fld9"
                strSQL &= " ,Fld10,Fld11,Fld12,Fld13,Fld14,Fld15,Fld16,Fld17,Fld18,Fld19,Fld20) values ("
                strSQL &= "'" & strRptName & "',"
                strSQL &= "'" & gStrSystemUserName & "'"
                For j = 0 To .Cols.Count - 1
                    If j <= 20 Then
                        strSQL = strSQL & ",'" & GetVal(.GetData(i, j)) & "'"
                    End If
                Next j
                For x = j + 1 To 20
                    strSQL = strSQL & ",''"
                Next x
                strSQL = strSQL & ")"
                clsSVT.execSQLStatement(strSQL)
            Next i
        End With
    End Sub

    Private Function TransferGridData() As Boolean
        TransferGridData = False
        With vsfView
            If .Rows.Count <= 1 Then
                MsgBox("No Data...", MsgBoxStyle.Exclamation, Me.Text)
                Exit Function
            End If

            ' Call ReplaceSingleQuoteInGrid()

            Dim strSQL As String = ""
            Dim strSQL2 As String = ""

            strSQL = " Delete from RptStk Where UserName='" & gStrSystemUserName & "' "
            If Not clsSVT.execSQLStatement(strSQL) Then
                Exit Function
            End If

            For i = 1 To .Rows.Count - 1
                Application.DoEvents()
                .TopRow = i
                If Val(.GetData(i, vDetItemID)) > 0 Then
                    strSQL = "Insert Into Rptstk (UserName,hdrName, ItemID,Rate ,RatePerCase ,"
                    strSQL &= "OPQty ,OPValue ,PBQty ,PBValue ,SRQty ,SRValue ,"
                    strSQL &= "InvQty ,InvRate ,InvValue ,"
                    strSQL &= "SDQty ,SDValue ,PRQty ,PRValue ,"
                    strSQL &= "ClQty ,ClValue) "
                    strSQL &= " Values ("
                    strSQL &= "'" & gStrSystemUserName & "',"
                    strSQL &= "'Stock Balance Report From " & getDateFormatInCond(dtpFrom.Value).Replace("'", "") & " To " & getDateFormatInCond(dtpTo.Value).Replace("'", "") & "',"
                    strSQL &= Val(.GetData(i, vDetItemID)) & ","
                    strSQL &= Val(.GetData(i, vDetRate)) & ","
                    strSQL &= Val(.GetData(i, vDetRatePerCase)) & ","
                    strSQL &= Val(.GetData(i, vDetOPQty)) & ","
                    strSQL &= Val(.GetData(i, vDetOPValue)) & ","
                    strSQL &= Val(.GetData(i, vDetPBQty)) & ","
                    strSQL &= Val(.GetData(i, vDetPBValue)) & ","
                    strSQL &= Val(.GetData(i, vDetSRQty)) & ","
                    strSQL &= Val(.GetData(i, vDetSRValue)) & ","
                    strSQL &= Val(.GetData(i, vDetInvQty)) & ","
                    strSQL &= Val(.GetData(i, vDetInvRate)) & ","
                    strSQL &= Val(.GetData(i, vDetInvValue)) & ","
                    strSQL &= Val(.GetData(i, vDetSDQty)) & ","
                    strSQL &= Val(.GetData(i, vDetSDValue)) & ","
                    strSQL &= Val(.GetData(i, vDetPRQty)) & ","
                    strSQL &= Val(.GetData(i, vDetPRValue)) & ","
                    strSQL &= Val(.GetData(i, vDetClQty)) & ","
                    strSQL &= Val(.GetData(i, vDetClValue)) & ")"
                    Call clsSVT.execSQLStatement(strSQL)
                End If
            Next i
        End With
        TransferGridData = True
    End Function

    Private Sub PrintData(Optional ByVal argBlnEmail As Boolean = False, Optional ByVal argselFormula As String = "", Optional ByVal argRptName As String = "rptStkBal.Rpt")
        Dim strSelFormula As String = ""
        strSelFormula = argselFormula
        Try
            Dim frmv As New frmUtlReportViewer
            With frmv
                .StartPosition = FormStartPosition.CenterScreen
                .showRptFile(Me.Text, argRptName, strSelFormula, Nothing, argBlnEmail, True, False, "", 1)
                If Not argBlnEmail Then .Show()
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

    Private Sub ViewClDetailed()
        If chkValues.Checked And optWAR.Checked Then
            Call valuateWAR()
        End If
        If chkValues.Checked And optFIFO.Checked Then
            Call showFifoClDetailed()
            Exit Sub
        End If

        Dim strSQL As String = "", intcolActSales As Integer, intcolDiff As Integer
        Dim strItemList As String = ""
        Dim sngPurRate As Single, sngMRPRate As Single
        Dim sngStkRate As Single, sngInvRate As Single
        Dim rs As OleDbDataReader
        With vsfView
            .Rows.Count = 1
            .Rows.Count = 4
            .Rows.Fixed = 4
            .Cols.Count = 0
            .Cols.Count = vDetTotal
            .SetData(0, vDetGrpName, "Item Group Name")
            .SetData(0, vDetBrdName, "Item Brand Name")
            .SetData(0, vDetItemcode, "Item Code")
            .SetData(0, vDetItemName, "Item Name")
            .SetData(0, vDetItemID, "ItemID")
            .SetData(0, vDetUOM, "PKG")
            .SetData(0, vDetHSN, "HSNCODE")
            If optPurRate.Checked Then
                .SetData(0, vDetRate, "Pur.Rate")
            Else
                .SetData(0, vDetRate, "MRP")
            End If

            .SetData(0, vDetRatePerCase, "Rate Per Case")
            .SetData(0, vDetOPValue, "OP Value")

            .SetData(0, vDetPBValue, "PB Value")
            .SetData(0, vDetSRValue, "SR Value")
            .SetData(0, vDetInvValue, "Inv Value")
            .SetData(0, vDetSDValue, "Ded Value")
            .SetData(0, vDetPRValue, "PR Value")
            .SetData(0, vDetSAValue, "SA Value")

            .Cols(vDetOPValue).Format = "0.00"
            .Cols(vDetPBValue).Format = "0.00"
            .Cols(vDetSRValue).Format = "0.00"
            .Cols(vDetInvValue).Format = "0.00"
            .Cols(vDetSDValue).Format = "0.00"
            .Cols(vDetPRValue).Format = "0.00"
            .Cols(vDetSAValue).Format = "0.00"
            .Cols(vDetInvRate).Format = "0.00"
            .Cols(vDetClValue).Format = "0.00"

            .SetData(0, vDetInvRate, "Inv Rate")
            .SetData(0, vDetBPercase, "Bottle PerCase")
            .SetData(0, vDetInCase, "In Case")
            If chkQtyInLtr.Checked Then
                .SetData(0, vDetOPQty, "OP Qty in Ltrs")
                .SetData(0, vDetPBQty, "PB Qty in Ltrs")
                .SetData(0, vDetSRQty, "SR Qty in Ltrs")
                .SetData(0, vDetInvQty, "Inv Qty in Ltrs")
                .SetData(0, vDetSDQty, "Ded Qty in Ltrs")
                .SetData(0, vDetPRQty, "PR Qty in Ltrs")
                .SetData(0, vDetSAQty, "SA Qty in Ltrs")
                .SetData(0, vDetClQty, "Cl Qty in Ltrs")
            Else
                .SetData(0, vDetOPQty, "OP Qty")
                .SetData(0, vDetPBQty, "PB Qty")
                .SetData(0, vDetSRQty, "SR Qty")
                .SetData(0, vDetInvQty, "Inv Qty")
                .SetData(0, vDetSDQty, "Ded Qty")
                .SetData(0, vDetPRQty, "PR Qty")
                .SetData(0, vDetPRQty, "PR Qty")
                .SetData(0, vDetSAQty, "SA Qty")
                .SetData(0, vDetClQty, "Cl Qty")
            End If
            .SetData(0, vDetClValue, "Cl Value")
            If ChkAROED.Checked Then
                .SetData(0, vDetOPOREDQty, "OP AROED Value")
                .SetData(0, vDetOPTotValue, "OP Total Value")

                .SetData(0, vDetPBAOREDQty, "PB AROED Value")
                .SetData(0, vDetPBTotValue, "PB Total Value")

                .SetData(0, vDetSRAROEDQty, "SP AROED Value")
                .SetData(0, vDetSRTotValue, "SP Total Value")

                .SetData(0, vDetInvAROEDQty, "INV AROED Value")
                .SetData(0, vDetInvTotValue, "INV Total Value")

                .SetData(0, vDetSDAROEDQty, "SD AROED Value")
                .SetData(0, vDetSDTotValue, "SD Total Value")


                .SetData(0, vDetPRAROEDQty, "PR AROED Value")
                .SetData(0, vDetPRTotValue, "PR Total Value")

                .SetData(0, vDetSAAROEDQty, "SA AROED Value")
                .SetData(0, vDetSATotValue, "SA Total Value")

                .SetData(0, vDetClAROEDQty, "CL AROED Value")
                .SetData(0, vDetClTotValue, "CL Total Value")
            End If
            If strBrandReq <> "Y" Then
                vsfView.Cols(vDetBrdName).Visible = False
            End If
            SetHideCol(vsfView, True, vDetItemID, vDetBPercase, vDetInvTotValue)
            If Not chkValues.Checked Or chkQtyInLtr.Checked = True Then
                SetHideCol(vsfView, True, vDetOPValue, vDetPBValue, vDetSRValue, vDetInvValue, vDetInvRate, vDetSDValue, vDetPRValue, vDetSAValue, vDetClValue)
            End If
            'If Not chkValues.Checked = True And ChkAROED.Checked = True Then
            '    SetHideCol(vsfView, True, vDetOPOREDQty, vDetOPTotValue, vDetPBAOREDQty, vDetPBTotValue, vDetSRAROEDQty, vDetSRTotValue, vDetInvAROEDQty, vDetInvTotValue, vDetPRAROEDQty, vDetPRTotValue, vDetClAROEDQty, vDetClTotValue, vDetSDAROEDQty, vDetSDTotValue)
            'End If
            If chkValues.Checked = False Or ChkAROED.Checked = False Then
                SetHideCol(vsfView, True, vDetOPOREDQty, vDetOPTotValue, vDetPBAOREDQty, vDetPBTotValue, vDetSRAROEDQty, vDetSRTotValue, vDetInvAROEDQty, vDetInvTotValue, vDetPRAROEDQty, vDetPRTotValue, vDetClAROEDQty, vDetClTotValue, vDetSDAROEDQty, vDetSDTotValue, vDetSAAROEDQty, vDetSATotValue, vDetBrdName)
            End If

            SetColAlign(vsfView, svtAlign.Left, vDetGrpName, vDetItemcode, vDetItemName, vDetUOM)
            SetFlexColorFullGrid(vsfView, "", 0)
            SetFlexColorFullGrid(vsfView, "", 1)
            SetFlexColorFullGrid(vsfView, "", 2)
            SetFlexColorFullGrid(vsfView, "", 3)
        End With

        Dim strRate As String = "PBRate"
        If chkTax.Checked Then
            strRate = "Round(NetAmt/PBQty,2)"
        End If

        strSQL = " Select i.PkgItem,GroupOrderBy,PkgOrderBy, g.Itemgroupname,BrandName,i.itemid, i.Itemcode, i.ItemName, i.mrp,"
        strSQL = strSQL & " i.ShortName, p.Pkgcode, 'OP' as DocType, 1 as StkMultiply,i.purrate, "
        strSQL = strSQL & " sum(StkMultiplier * u.Qty) as qty, sum(StkMultiplier * u.LtrQty) as LtrQty,"
        strSQL = strSQL & " case when sum(docvalue) > 0 and sum(stkmultiplier * u.qty) > 0 then sum(docvalue)/sum(stkmultiplier * u.qty) else 0 end as docRate, sum(docValue) as docValue, "
        strSQL = strSQL & " 0 as CasePrice "
        If optPurRate.Checked Then
            strSQL = strSQL & " ,isnull((Select top 1 " & strRate & " from vwPBItemLastPrice v where i.ItemID = v.itemid And pbdate <= " & getDateFormatInCond(dtpTo.Value) & " order by Slno),0) As stkRate"
            'strSQL = strSQL & " ,0 As StkRate"
        ElseIf optWAR.Checked Or optFIFO.Checked Then
            strSQL = strSQL & " ,0 As StkRate"
        Else
            strSQL = strSQL & " ,i.mrp As StkRate"
        End If
        strSQL = strSQL & " ,0 As ActSalesValue,BotPerCases ,HSNCode,isnull(I.AROEDPER,0) as AROEDPER"
        strSQL &= " From MstItem i, MstItemGroup g, MstPkg p, UtlStockList u,mstBrand b "
        strSQL = strSQL & " where i.ItemID = u.ItemID"
        strSQL = strSQL & " And i.PkgID = p.PkgID"
        strSQL = strSQL & " And i.BrandID = b.BrandID"
        strSQL = strSQL & " And i.ItemGroupID = g.ItemGroupId "
        strSQL = strSQL & " And DocDate < " & getDateFormatInCond(dtpFrom.Value)
        strSQL = strSQL & " And i.Inactive='N'"
        'strSQL = strSQL & " And i.itemid = 1221"
        strItemList = ""
        strItemList = getItemList()
        If strItemList <> "" Then
            strSQL = strSQL & " And i.ItemID In (" & strItemList & ") "
        End If
        If chkItemGroup.Checked Then
            If strItemGroupList <> "" Then
                strSQL = strSQL & " And i.ItemGroupID In (" & strItemGroupList & ") "
            End If
        End If
        strItemList = ""
        strItemList = getPkgList()
        If strItemList <> "" Then
            strSQL = strSQL & " And i.PKGITEM In (" & strItemList & ") "
        End If
        strSQL = strSQL & " group by i.PkgItem,GroupOrderBy, PkgOrderBy,g.Itemgroupname,BrandName,i.itemid, i.Itemcode, i.ItemName, i.mrp, i.ShortName, p.Pkgcode, i.PURRATE,BotPerCases,HSNCode,isnull(I.AROEDPER,0) "
        strSQL &= " Union All "

        strSQL &= " Select i.PkgItem,GroupOrderBy, PkgOrderBy,g.Itemgroupname,BrandName,i.itemid, i.Itemcode, i.ItemName, i.mrp, i.ShortName, p.Pkgcode, DocType, StkMultiplier As StkMultiply, i.purrate, StkMultiplier * u.Qty As Qty, StkMultiplier * u.LtrQty As LtrQty, u.rate As docrate, docvalue, i.BotPerCases * i.PURRATE As CasePrice  "



        If optPurRate.Checked Then
            strSQL = strSQL & " ,isnull((Select top 1 " & strRate & " from vwPBItemLastPrice v where i.ItemID = v.itemid  And pbdate <= " & getDateFormatInCond(dtpTo.Value) & " order by Slno),0) As StkRate"
        ElseIf optWAR.Checked Then
            strSQL = strSQL & " ,rate As StkRate"
        ElseIf optFIFO.Checked Then
            strSQL = strSQL & " ,Fiforate As StkRate"

        Else
            strSQL = strSQL & " ,i.MRP As StkRate"
        End If

        strSQL = strSQL & " , Case When DocType = 'INV' then (select sum(ItemAmt) from TrndtlSinv d where u.DocType ='INV' and d.sinvid = u.docid and u.ItemID = d.ItemID) else 0 end as ActSalesValue,BotPerCases,HSNCode ,isnull(I.AROEDPER,0) "
        strSQL &= " From MstItem i, MstItemGroup g, MstPkg p, UtlStockList u,MstBrand b "
        strSQL = strSQL & " where i.ItemID = u.ItemID"
        strSQL = strSQL & " And i.PkgID = p.PkgID"
        strSQL = strSQL & " And i.BrandID = b.brandID"
        strSQL = strSQL & " And i.ItemGroupID = g.ItemGroupId "
        strSQL = strSQL & " And DocDate between " & getDateFormatInCond(dtpFrom.Value)
        strSQL = strSQL & " And  " & getDateFormatInCond(dtpTo.Value)
        strSQL = strSQL & " And i.Inactive='N'"
        strItemList = ""
        strItemList = getItemList()
        'strSQL = strSQL & " and i.itemid = 1221"
        If strItemList <> "" Then
            strSQL = strSQL & " And i.ItemID In (" & strItemList & ") "
        End If
        If chkItemGroup.Checked Then
            If strItemGroupList <> "" Then
                strSQL = strSQL & " and i.ItemGroupID in (" & strItemGroupList & ") "
            End If
        End If
        strItemList = ""
        strItemList = getPkgList()
        If strItemList <> "" Then
            strSQL = strSQL & " And i.PKGITEM In (" & strItemList & ") "
        End If
        strSQL &= " Order by GroupOrderBy,PkgItem asc, PkgOrderBy,  StkRate desc,itemname"

        rs = clsSVT.getDataReaderFromSQL(strSQL)

        If Not rs.HasRows Then
            rs.Close()
            Exit Sub
        End If

        Dim blnMetroWines As Boolean

        clsSVT.getDataStringValue("Select ClientCode from UtlMisc	where ClientCode = 'METROWINES' ", "", blnMetroWines)

        Dim intDupRow As Int32 = -1
        Dim intColNo As Int32 = -1, intValColNo As Integer
        Dim intRow As Int32

        With rs
            Do While .Read
                Application.DoEvents()
                intDupRow = vsfView.FindRow(GetVal(!itemid, ""), 1, vDetItemID, False, True, False)
                sngPurRate = !PurRate
                sngMRPRate = !MRP
                sngStkRate = !StkRate
                If !stkrate = 0 Then sngStkRate = !docrate
                sngInvRate = 0

                If optPurRate.Checked Then
                    'sngStkRate = sngPurRate
                ElseIf optFIFO.Checked = False Then
                    sngStkRate = !MRP
                End If


                If intDupRow = -1 Then
                    vsfView.Rows.Count = vsfView.Rows.Count + 1
                    vsfView.TopRow = vsfView.Rows.Count - 1
                    vsfView.SetData(vsfView.Rows.Count - 1, vDetGrpName, !Itemgroupname)
                    vsfView.SetData(vsfView.Rows.Count - 1, vDetBrdName, !Brandname)
                    vsfView.SetData(vsfView.Rows.Count - 1, vDetItemcode, !Itemcode)
                    vsfView.SetData(vsfView.Rows.Count - 1, vDetItemName, !ItemName)
                    vsfView.SetData(vsfView.Rows.Count - 1, vDetBPercase, !BotPerCases)
                    If chkValues.Checked = True Then
                        vsfView.SetData(vsfView.Rows.Count - 1, vDetRatePerCase, clsSVT.getDataDoubleValue("Select IsNull(BotPerCases * PURRATE,0) From MstITEM Where Itemcode='" & !Itemcode & "'", 0, False))
                    End If

                    vsfView.SetData(vsfView.Rows.Count - 1, vDetItemID, !ItemId)
                    vsfView.SetData(vsfView.Rows.Count - 1, vDetUOM, !Pkgcode)
                    vsfView.SetData(vsfView.Rows.Count - 1, vDetHSN, GetVal(!HSNCODE, ""))
                    vsfView.SetData(vsfView.Rows.Count - 1, vDetRate, sngStkRate)
                    'vsfView.SetData(vsfView.Rows.Count - 1, vDetRatePerCase, !CasePrice)
                    intRow = vsfView.Rows.Count - 1

                Else
                    intRow = intDupRow
                End If
                intColNo = -1
                intcolActSales = 0
                intcolDiff = 0
                Select Case UCase(GetVal(!DocType, ""))
                    Case "OP"
                        intColNo = vDetOPQty
                        intValColNo = vDetOPValue
                    Case "PB", "ST"
                        intColNo = vDetPBQty
                        intValColNo = vDetPBValue
                    Case "INV"
                        intColNo = vDetInvQty
                        intValColNo = vDetInvValue
                    Case "STKDED"
                        intColNo = vDetSDQty
                        intValColNo = vDetSDValue
                    Case "SR"
                        intColNo = vDetSRQty
                        intValColNo = vDetSRValue
                    Case "PR"
                        intColNo = vDetPRQty
                        intValColNo = vDetPRValue
                    Case "SA"
                        intColNo = vDetSAQty
                        intValColNo = vDetSAValue
                End Select

                If intColNo > -1 Then
                    If Not chkQtyInLtr.Checked Then
                        vsfView.SetData(intRow, intColNo, Val(vsfView.GetData(intRow, intColNo)) +
                                        (IIf(intColNo <> vDetOPQty, Math.Abs(Val(!Qty)), Val(!Qty))))

                        vsfView.SetData(intRow, vDetClQty, Val(vsfView.GetData(intRow, vDetClQty)) + (Val(!Qty)))

                        If optWAR.Checked Then
                            vsfView.SetData(intRow, intValColNo, Val(vsfView.GetData(intRow, intValColNo)) +
                                        (IIf(intValColNo <> vDetOPValue, Math.Abs(Val(!Docvalue)), Math.Round(!docvalue, 2))))
                            vsfView.SetData(intRow, vDetClValue, Val(vsfView.GetData(intRow, vDetClValue)) + (Math.Round(!docValue, 2)))
                        End If
                    Else
                        vsfView.SetData(intRow, intColNo, Val(vsfView.GetData(intRow, intColNo)) +
                                       (IIf(intColNo <> vDetOPQty, Math.Abs(Val(!LtrQty)), Val(!LtrQty))))
                        vsfView.SetData(intRow, vDetClQty, Val(vsfView.GetData(intRow, vDetClQty)) + (Val(!LtrQty)))
                    End If
                End If
                If chkValues.Checked = True And chkQtyInLtr.Checked = False Then
                    If Not optWAR.Checked Then
                        vsfView.SetData(intRow, vDetOPValue, Math.Round(Val(vsfView.GetData(intRow, vDetOPQty)) * sngStkRate, 2))
                        vsfView.SetData(intRow, vDetPBValue, Math.Round(Val(vsfView.GetData(intRow, vDetPBQty)) * sngStkRate, 2))
                        vsfView.SetData(intRow, vDetSRValue, Math.Round(Val(vsfView.GetData(intRow, vDetSRQty)) * sngStkRate, 2))
                        'If optFIFO.Checked Then

                        If chkActSales.Checked Then
                            vsfView.SetData(intRow, vDetInvValue, Math.Round((Val(vsfView.GetData(intRow, vDetInvValue)) + !ActSalesValue), 2))
                        Else
                            vsfView.SetData(intRow, vDetInvValue, Math.Round(Val(vsfView.GetData(intRow, vDetInvQty)) * sngStkRate, 2))
                        End If
                        If Val(vsfView.GetData(intRow, vDetInvQty)) > 0 Then
                            vsfView.SetData(intRow, vDetInvRate, Math.Round(Val(vsfView.GetData(intRow, vDetInvValue)) / Val(vsfView.GetData(intRow, vDetInvQty)), 2))
                        End If
                        vsfView.SetData(intRow, vDetSDValue, Math.Round(Val(vsfView.GetData(intRow, vDetSDQty)) * sngStkRate, 2))
                        vsfView.SetData(intRow, vDetPRValue, Math.Round(Val(vsfView.GetData(intRow, vDetPRQty)) * sngStkRate, 2))
                        vsfView.SetData(intRow, vDetSAValue, Math.Round(Val(vsfView.GetData(intRow, vDetSAQty)) * sngStkRate, 2))
                        vsfView.SetData(intRow, vDetClValue, Math.Round(Val(vsfView.GetData(intRow, vDetClQty)) * sngStkRate, 2))
                    End If


                End If
                If chkValues.Checked = True And ChkAROED.Checked = True Then
                    vsfView.SetData(intRow, vDetOPOREDQty, Math.Round(Val(vsfView.GetData(intRow, vDetOPQty)) * (rs!AROEDPER), 2))
                    vsfView.SetData(intRow, vDetOPTotValue, Math.Round(Val(vsfView.GetData(intRow, vDetOPOREDQty)) + Val(vsfView.GetData(intRow, vDetOPValue)), 2))

                    vsfView.SetData(intRow, vDetPBAOREDQty, Math.Round(Val(vsfView.GetData(intRow, vDetPBQty)) * (rs!AROEDPER), 2))
                    vsfView.SetData(intRow, vDetPBTotValue, Math.Round(Val(vsfView.GetData(intRow, vDetPBAOREDQty)) + Val(vsfView.GetData(intRow, vDetPBValue)), 2))

                    vsfView.SetData(intRow, vDetSRAROEDQty, Math.Round(Val(vsfView.GetData(intRow, vDetSRQty)) * !AROEDPER, 2))
                    vsfView.SetData(intRow, vDetSRTotValue, Math.Round(Val(vsfView.GetData(intRow, vDetSRAROEDQty)) + Val(vsfView.GetData(intRow, vDetSRValue)), 2))
                    vsfView.SetData(intRow, vDetInvAROEDQty, Math.Round(Val(vsfView.GetData(intRow, vDetInvQty)) * !AROEDPER, 2))
                    vsfView.SetData(intRow, vDetInvTotValue, Math.Round(Val(vsfView.GetData(intRow, vDetInvAROEDQty)) + Val(vsfView.GetData(intRow, vDetInvValue)), 2))

                    vsfView.SetData(intRow, vDetSDAROEDQty, Math.Round(Val(vsfView.GetData(intRow, vDetSDQty)) * !AROEDPER, 2))
                    vsfView.SetData(intRow, vDetSDTotValue, Math.Round(Val(vsfView.GetData(intRow, vDetSDAROEDQty)) + Val(vsfView.GetData(intRow, vDetSDValue)), 2))

                    vsfView.SetData(intRow, vDetPRAROEDQty, Math.Round(Val(vsfView.GetData(intRow, vDetPRQty)) * !AROEDPER, 2))
                    vsfView.SetData(intRow, vDetPRTotValue, Math.Round(Val(vsfView.GetData(intRow, vDetPRAROEDQty)) + Val(vsfView.GetData(intRow, vDetPRValue)), 2))
                    vsfView.SetData(intRow, vDetSATotValue, Math.Round(Val(vsfView.GetData(intRow, vDetSAAROEDQty)) + Val(vsfView.GetData(intRow, vDetSAValue)), 2))

                    vsfView.SetData(intRow, vDetClAROEDQty, Math.Round(Val(vsfView.GetData(intRow, vDetClQty)) * !AROEDPER, 2))
                    vsfView.SetData(intRow, vDetClTotValue, Math.Round(Val(vsfView.GetData(intRow, vDetClAROEDQty)) + Val(vsfView.GetData(intRow, vDetClValue)), 2))
                End If

                Dim dblresult As Double = 0
                Dim dblreminder As Double = 0

                If intColNo > -1 Then

                    If Not vsfView.GetData(intRow, intColNo) = 0 And intColNo = vDetPBQty Then
                        dblresult = vsfView.GetData(intRow, intColNo) / vsfView.GetData(intRow, vDetBPercase)
                        dblreminder = vsfView.GetData(intRow, intColNo) Mod vsfView.GetData(intRow, vDetBPercase)
                        vsfView.SetData(intRow, vDetInCase, Math.Floor(dblresult) & " Cases " & dblreminder & " Bottles")
                    End If
                End If



            Loop
            .Close()
        End With
        If chkQtyInLtr.Checked = True Then
            vsfView.Cols(vDetInCase).Visible = False
        Else
            vsfView.Cols(vDetInCase).Visible = True
        End If
        With vsfView
            For k = vDetOPQty To vDetClQty
                If chkQtyInLtr.Checked = False Then
                    If InStr(.GetData(0, k), "Qty") > 0 Then
                        .Cols(k).Format = "#"
                    End If
                Else
                    .Cols(k).Format = "0.000"
                End If
            Next

            '            If chkZeroBal.Checked Then
            'chkagain:
            '                If .Rows.Count > 4 Then
            '                    For k = 4 To .Rows.Count - 1
            '                        If GetVal(.GetData(k, vDetClQty), 0) = 0 Then
            '                            .RemoveItem(k)
            '                            GoTo chkagain
            '                        End If
            '                    Next
            '                End If
            'End If
            RemoveVsfRows(vsfView)


            '.SubtotalPosition = C1.Win.C1FlexGrid.SubtotalPositionEnum.BelowData
            'If chkTotGW.Checked Then

            '    For k = vCBOpQty To vsfView.Cols.Count - 2
            '        .Subtotal(C1.Win.C1FlexGrid.AggregateEnum.Sum, 0, 0, k)
            '    Next k

            'End If

            'For k = vCBOpQty To vsfView.Cols.Count - 2
            '    .Subtotal(C1.Win.C1FlexGrid.AggregateEnum.Sum, -1, -1, k)
            'Next k
            '.AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.RestrictAll
            '.Cols(vCBItemGrpName).AllowMerging = True


            .SubtotalPosition = C1.Win.C1FlexGrid.SubtotalPositionEnum.BelowData

            For k = vDetOPQty To vsfView.Cols.Count - 1
                If Strings.Right(.GetData(0, k), 4) <> "Rate" Then

                    If chkTotGW.Checked Then
                        .Subtotal(C1.Win.C1FlexGrid.AggregateEnum.Sum, 0, vDetGrpName, k)
                    End If

                    .Subtotal(C1.Win.C1FlexGrid.AggregateEnum.Sum, -1, -1, k)


                End If
            Next k
            .AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.RestrictAll
            .Cols(vDetGrpName).AllowMerging = True
        End With
        If chkValues.Checked = False Or ChkAROED.Checked = False Then
            SetHideCol(vsfView, True, vDetOPOREDQty, vDetOPTotValue, vDetPBAOREDQty, vDetPBTotValue, vDetSRAROEDQty, vDetSRTotValue, vDetInvAROEDQty, vDetInvTotValue, vDetPRAROEDQty, vDetPRTotValue, vDetClAROEDQty, vDetClTotValue, vDetSDAROEDQty, vDetSDTotValue, vDetSATotValue, vDetBrdName, vDetInvTotValue)
        End If
        If Not gBlnAutoEmail Then
            Call initVsfForHdrs(vsfView, Me.Text, "Stock Balance Date From " & getDateFormatInInsert(dtpFrom.Value) & " To " & getDateFormatInInsert(dtpTo.Value))
        End If

        vsfView.AutoSizeCols()

    End Sub


    Private Sub btnPrintZeroStk_Click(sender As Object, e As EventArgs) Handles btnPrintZeroStk.Click
        Dim strFormulas(,) As String = Nothing
        ReDim strFormulas(6, 1)

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

        Dim strSelFormula As String = ""
        strSelFormula = ""
        Try
            Dim frmv As New frmUtlReportViewer
            With frmv
                .StartPosition = FormStartPosition.CenterScreen
                .showRptFile(Me.Text, "RptZBItem.rpt", strSelFormula, strFormulas, False, True, False)
                .Show()
            End With
        Catch ex As Exception
            MsgBox(ex.Message(), MsgBoxStyle.Exclamation, Me.Text)
        End Try
    End Sub

    Private Sub btnColFrz_Click(sender As Object, e As EventArgs) Handles btnColFrz.Click
        If btnColFrz.Text = "&Col Freez" Then
            btnColFrz.Text = "&Col UnFreez"

            If optClDetails.Checked Then
                vsfView.Rows.Frozen = 3
                vsfView.Cols.Frozen = vsfView.Col + 1
                vsfView.Cols.Frozen = vsfView.Col + 1
            Else
                vsfView.Cols.Frozen = vsfView.Col + 1
                vsfView.Cols.Frozen = vsfView.Col + 1
            End If

        Else
            btnColFrz.Text = "&Col Freez"
            vsfView.Cols.Frozen = 0
            vsfView.Cols.Frozen = 0
            vsfView.Rows.Frozen = 0
        End If
    End Sub

    Private Sub btnColSel_Click(sender As Object, e As EventArgs) Handles btnColSel.Click
        If Not blnChanged AndAlso vsfView.Rows.Count > 1 Then
            Dim intRow As Int32
            With vsfCol
                .Cols.Count = 2
                .Rows.Count = 1
                .SetData(0, 0, "Select")
                .SetData(0, 1, "Column Name")
                .Cols(0).DataType = GetType(Boolean)
                .Rows.Count += vsfView.Cols.Count

                Dim i As Int32 = 1

                If optClDetails.Checked Then
                    intRow = 3
                Else
                    intRow = 0
                End If

                For j As Int32 = 0 To vsfView.Cols.Count - 1
                    'If Trim(vsfView.GetData(intRow, j)) <> "" Then
                    .SetData(i, 1, vsfView.GetData(intRow, j))
                        If i = .Rows.Count - 1 Then Exit For
                        i = i + 1
                    'End If
                Next

            End With
            RefreshGrid(True)

            'For i = 0 To vsfView.Cols.Count - 1
            '    If Trim(vsfView.GetData(intRow, i)) <> "" Then
            '        vsfView.Cols(i).Visible = True
            '    Else
            '        vsfView.Cols(i).Visible = False
            '    End If
            'Next

            pnlCol.Visible = True
            pnlCol.BringToFront()
        Else
            MsgBox("Combination Changed! Set your combination and load by clicking View button! then select columns...", MsgBoxStyle.Exclamation, Me.Text)
        End If

    End Sub

    Private Sub RefreshGrid(argFromSel As Boolean)
        Dim strSelcols As String
        Dim strSQL As String


        If optCBal.Checked Then

            If optPurRate.Checked AndAlso chkValues.Checked Then
                strSQL = "Select SelCols from UtlSelCols Where UserCode = '" & gUserCode & "' and formTag= 'StkBalViewCPV'"
            ElseIf optPurRate.Checked AndAlso chkQtyInLtr.Checked Then
                strSQL = "Select SelCols from UtlSelCols Where UserCode = '" & gUserCode & "' and formTag= 'StkBalViewCPL'"
            ElseIf (optSalesRate.Checked) AndAlso chkValues.Checked Then
                strSQL = "Select SelCols from UtlSelCols Where UserCode = '" & gUserCode & "' and formTag= 'StkBalViewCSV'"
            ElseIf (optSalesRate.Checked) AndAlso chkQtyInLtr.Checked Then
                strSQL = "Select SelCols from UtlSelCols Where UserCode = '" & gUserCode & "' and formTag= 'StkBalViewCSL'"
            ElseIf optPurRate.Checked Then
                strSQL = "Select SelCols from UtlSelCols Where UserCode = '" & gUserCode & "' and formTag= 'StkBalViewCP'"
            ElseIf (optSalesRate.Checked) Then
                strSQL = "Select SelCols from UtlSelCols Where UserCode = '" & gUserCode & "' and formTag= 'StkBalViewCS'"
            End If

        ElseIf optClDetails.Checked Then

            If optPurRate.Checked AndAlso chkValues.Checked Then
                strSQL = "Select SelCols from UtlSelCols Where UserCode = '" & gUserCode & "' and formTag= 'StkBalViewDPV'"
            ElseIf optPurRate.Checked AndAlso chkQtyInLtr.Checked Then
                strSQL = "Select SelCols from UtlSelCols Where UserCode = '" & gUserCode & "' and formTag= 'StkBalViewDPL'"
            ElseIf (optSalesRate.Checked) AndAlso chkValues.Checked Then
                strSQL = "Select SelCols from UtlSelCols Where UserCode = '" & gUserCode & "' and formTag= 'StkBalViewDSV'"
            ElseIf (optSalesRate.Checked) AndAlso chkQtyInLtr.Checked Then
                strSQL = "Select SelCols from UtlSelCols Where UserCode = '" & gUserCode & "' and formTag= 'StkBalViewDSL'"
            ElseIf optPurRate.Checked Then
                strSQL = "Select SelCols from UtlSelCols Where UserCode = '" & gUserCode & "' and formTag= 'StkBalViewDP'"
            ElseIf (optSalesRate.Checked) Then
                strSQL = "Select SelCols from UtlSelCols Where UserCode = '" & gUserCode & "' and formTag= 'StkBalViewDS'"
            End If

        Else
            If optPurRate.Checked AndAlso chkQtyInLtr.Checked Then
                strSQL = "Select SelCols from UtlSelCols Where UserCode = '" & gUserCode & "' and formTag= 'StkBalViewPPL'"
            ElseIf (optSalesRate.Checked) AndAlso chkValues.Checked Then
                strSQL = "Select SelCols from UtlSelCols Where UserCode = '" & gUserCode & "' and formTag= 'StkBalViewPSV'"
            ElseIf (optSalesRate.Checked) AndAlso chkQtyInLtr.Checked Then
                strSQL = "Select SelCols from UtlSelCols Where UserCode = '" & gUserCode & "' and formTag= 'StkBalViewPSL'"
            ElseIf optPurRate.Checked Then
                strSQL = "Select SelCols from UtlSelCols Where UserCode = '" & gUserCode & "' and formTag= 'StkBalViewPP'"
            ElseIf (optSalesRate.Checked) Then
                strSQL = "Select SelCols from UtlSelCols Where UserCode = '" & gUserCode & "' and formTag= 'StkBalViewPS'"
            End If

        End If
        strSelcols = ""
        If strSQL <> "" Then
            strSelcols = clsSVT.getDataStringValue(strSQL, "", False)
        End If
        Dim intColsVisible As Int32
        If Trim(strSelcols) <> "" Then
            If argFromSel Then
                With vsfCol
                    For x As Int32 = 1 To .Rows.Count - 1
                        If Trim(.GetData(x, 1)) <> "" Then .SetCellCheck(x, 0, CheckEnum.Checked)
                    Next

                    For i = 1 To .Rows.Count - 1
                        If InStr(1, strSelcols, .GetData(i, 1)) = 0 Then
                            .SetCellCheck(i, 0, CheckEnum.Unchecked)
                        End If
                    Next
                End With
                Exit Sub
            End If

            With vsfView
                For i = 0 To .Cols.Count - 1
                    .Cols(i).Visible = False
                Next

                For i = 0 To .Cols.Count - 1
                    If optClDetails.Checked Then
                        If InStr(1, strSelcols, .GetData(3, i)) > 0 Then
                            .Cols(i).Visible = True
                            intColsVisible = intColsVisible + 1
                        End If
                    Else
                        If InStr(1, strSelcols, .GetData(0, i)) > 0 Then
                            .Cols(i).Visible = True
                            intColsVisible = intColsVisible + 1
                        End If
                    End If
                Next
            End With
        Else
            For x As Int32 = 1 To vsfCol.Rows.Count - 1
                If Trim(vsfCol.GetData(x, 1)) <> "" Then vsfCol.SetCellCheck(x, 0, CheckEnum.Checked)
            Next
        End If

        If intColsVisible = 0 Then
            For i = 0 To vsfView.Cols.Count - 1
                vsfView.Cols(i).Visible = True
            Next
        End If

        If optCBal.Checked Then
            'If Not chkValues.Checked Then
            '    vsfView.Cols(vCBClValue).Visible = False
            '    vsfView.Cols(vCBRate).Visible = False
            'Else
            '    vsfView.Cols(vCBClValue).Visible = True
            '    vsfView.Cols(vCBRate).Visible = True
            'End If
        ElseIf optClDetails.Checked Then
            SetHideCol(vsfView, True, vDetItemID, vDetBPercase)
            If Not chkValues.Checked Or chkQtyInLtr.Checked = True Then
                SetHideCol(vsfView, True, vDetOPValue, vDetPBValue, vDetSRValue, vDetInvValue, vDetInvRate, vDetSDValue, vDetPRValue, vDetClValue)
            End If
        End If


        If optClDetails.Checked Then
            intTotRec = Val(vsfView.Rows.Count) - 5
        Else
            intTotRec = Val(vsfView.Rows.Count) - 2
        End If



        lblTotRec.Text = "Total Records: " & intTotRec
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSaveCol.Click
        Dim strSQL As String
        Dim strFld As String = ""
        Dim x As Int32 = 1
        Dim intRow As Int32

        If optClDetails.Checked Then
            intRow = 3
        Else
            intRow = 0
        End If

        For y As Int32 = 0 To vsfView.Cols.Count - 1

            If vsfView.GetData(intRow, y) = vsfCol.GetData(x, 1) AndAlso vsfCol.GetCellCheck(x, 0) = CheckEnum.Unchecked Then
                vsfView.Cols(y).Visible = False
            ElseIf vsfView.GetData(intRow, y) = vsfCol.GetData(x, 1) AndAlso vsfCol.GetCellCheck(x, 0) = CheckEnum.Checked Then
                strFld &= vsfCol.GetData(x, 1) & ","
                vsfView.Cols(y).Visible = True
            End If
            If x = vsfCol.Rows.Count - 1 Then Exit For
            x = x + 1

        Next
        vsfView.Refresh()

        If strFld <> "" Then strFld = Mid(strFld, 1, Len(strFld) - 1)

        If optCBal.Checked Then
            If optPurRate.Checked AndAlso chkQtyInLtr.Checked Then
                strSQL = "Delete from UtlSelCols where UserCode = '" & gUserCode & "' and FormTag = 'StkBalViewCPL'"
            ElseIf optPurRate.Checked AndAlso chkValues.Checked Then
                strSQL = "Delete from UtlSelCols where UserCode = '" & gUserCode & "' and FormTag = 'StkBalViewCPV'"
            ElseIf (optSalesRate.Checked) AndAlso chkQtyInLtr.Checked Then
                strSQL = "Delete from UtlSelCols where UserCode = '" & gUserCode & "' and FormTag = 'StkBalViewCSL'"
            ElseIf (optSalesRate.Checked) AndAlso chkValues.Checked Then
                strSQL = "Delete from UtlSelCols where UserCode = '" & gUserCode & "' and FormTag = 'StkBalViewCSV'"
            ElseIf optPurRate.Checked Then
                strSQL = "Delete from UtlSelCols where UserCode = '" & gUserCode & "' and FormTag = 'StkBalViewCP'"
            ElseIf (optSalesRate.Checked) Then
                strSQL = "Delete from UtlSelCols where UserCode = '" & gUserCode & "' and FormTag = 'StkBalViewCS'"
            End If

        ElseIf optClDetails.Checked Then
            If optPurRate.Checked AndAlso chkQtyInLtr.Checked Then
                strSQL = "Delete from UtlSelCols where UserCode = '" & gUserCode & "' and FormTag = 'StkBalViewDPL'"
            ElseIf optPurRate.Checked AndAlso chkValues.Checked Then
                strSQL = "Delete from UtlSelCols where UserCode = '" & gUserCode & "' and FormTag = 'StkBalViewDPV'"
            ElseIf (optSalesRate.Checked) AndAlso chkQtyInLtr.Checked Then
                strSQL = "Delete from UtlSelCols where UserCode = '" & gUserCode & "' and FormTag = 'StkBalViewDSL'"
            ElseIf (optSalesRate.Checked) AndAlso chkValues.Checked Then
                strSQL = "Delete from UtlSelCols where UserCode = '" & gUserCode & "' and FormTag = 'StkBalViewDSV'"
            ElseIf optPurRate.Checked Then
                strSQL = "Delete from UtlSelCols where UserCode = '" & gUserCode & "' and FormTag = 'StkBalViewDP'"
            ElseIf (optSalesRate.Checked) Then
                strSQL = "Delete from UtlSelCols where UserCode = '" & gUserCode & "' and FormTag = 'StkBalViewDS'"
            End If
        Else
            If optPurRate.Checked AndAlso chkQtyInLtr.Checked Then
                strSQL = "Delete from UtlSelCols where UserCode = '" & gUserCode & "' and FormTag = 'StkBalViewCPL'"
            ElseIf (optSalesRate.Checked) AndAlso chkQtyInLtr.Checked Then
                strSQL = "Delete from UtlSelCols where UserCode = '" & gUserCode & "' and FormTag = 'StkBalViewCSL'"
            ElseIf (optSalesRate.Checked) AndAlso chkValues.Checked Then
                strSQL = "Delete from UtlSelCols where UserCode = '" & gUserCode & "' and FormTag = 'StkBalViewCSV'"
            ElseIf optPurRate.Checked Then
                strSQL = "Delete from UtlSelCols where UserCode = '" & gUserCode & "' and FormTag = 'StkBalViewCP'"
            ElseIf (optSalesRate.Checked) Then
                strSQL = "Delete from UtlSelCols where UserCode = '" & gUserCode & "' and FormTag = 'StkBalViewCS'"
            End If

        End If

        clsSVT.execSQLStatement(strSQL)

        strSQL = "Insert Into UtlSelCols (UserCode, FormTag, SelCols,FldsOrderBy) Values ("
        strSQL &= "'" & gUserCode & "',"

        If optCBal.Checked Then
            If optPurRate.Checked AndAlso chkQtyInLtr.Checked Then
                strSQL &= "'StkBalViewCPL',"
            ElseIf optPurRate.Checked AndAlso chkValues.Checked Then
                strSQL &= "'StkBalViewCPV',"
            ElseIf (optSalesRate.Checked) AndAlso chkQtyInLtr.Checked Then
                strSQL &= "'StkBalViewCSL',"
            ElseIf (optSalesRate.Checked) AndAlso chkValues.Checked Then
                strSQL &= "'StkBalViewCSV',"
            ElseIf optPurRate.Checked Then
                strSQL &= "'StkBalViewCP',"
            ElseIf (optSalesRate.Checked) Then
                strSQL &= "'StkBalViewCS',"
            End If
        ElseIf optClDetails.Checked Then
            If optPurRate.Checked AndAlso chkQtyInLtr.Checked Then
                strSQL &= "'StkBalViewDPL',"
            ElseIf optPurRate.Checked AndAlso chkValues.Checked Then
                strSQL &= "'StkBalViewDPV',"
            ElseIf (optSalesRate.Checked) AndAlso chkQtyInLtr.Checked Then
                strSQL &= "'StkBalViewDSL',"
            ElseIf (optSalesRate.Checked) AndAlso chkValues.Checked Then
                strSQL &= "'StkBalViewDSV',"
            ElseIf optPurRate.Checked Then
                strSQL &= "'StkBalViewDP',"
            ElseIf (optSalesRate.Checked) Then
                strSQL &= "'StkBalViewDS',"
            End If
        Else
            If optPurRate.Checked AndAlso chkQtyInLtr.Checked Then
                strSQL &= "'StkBalViewCPL',"
            ElseIf (optSalesRate.Checked) AndAlso chkQtyInLtr.Checked Then
                strSQL &= "'StkBalViewCSL',"
            ElseIf (optSalesRate.Checked) AndAlso chkValues.Checked Then
                strSQL &= "'StkBalViewCSV',"
            ElseIf optPurRate.Checked Then
                strSQL &= "'StkBalViewCP',"
            ElseIf (optSalesRate.Checked) Then
                strSQL &= "'StkBalViewCS',"
            End If
        End If
        strSQL &= "'" & strFld & "',"
        strSQL &= "'')"
        clsSVT.execSQLStatement(strSQL)

        pnlCol.Visible = False
        pnlCol.SendToBack()

        If optCBal.Checked Then
            'If Not chkValues.Checked Then
            '    vsfView.Cols(vCBClValue).Visible = False
            '    vsfView.Cols(vCBRate).Visible = False
            'Else
            '    vsfView.Cols(vCBClValue).Visible = True
            '    vsfView.Cols(vCBRate).Visible = True
            'End If
        ElseIf optClDetails.Checked Then
            SetHideCol(vsfView, True, vDetItemID, vDetBPercase)
            If Not chkValues.Checked Or chkQtyInLtr.Checked = True Then
                SetHideCol(vsfView, True, vDetOPValue, vDetPBValue, vDetSRValue, vDetInvValue, vDetInvRate, vDetSDValue, vDetPRValue, vDetClValue)
            End If
        End If

        'For i = 0 To vsfView.Cols.Count - 1
        '    If Trim(vsfView.GetData(intRow, i)) <> "" Then
        '        vsfView.Cols(i).Visible = True
        '    Else
        '        vsfView.Cols(i).Visible = False
        '    End If
        'Next

    End Sub

    Private Sub btnCloseCol_Click(sender As Object, e As EventArgs) Handles btnCloseCol.Click

        pnlCol.Visible = False
        pnlCol.SendToBack()
    End Sub

    Private Sub vsfCol_BeforeEdit(sender As Object, e As RowColEventArgs) Handles vsfCol.BeforeEdit
        Select Case e.Col
            Case 1
                e.Cancel = True
        End Select
    End Sub

    Private Sub chkSelAll_CheckedChanged_1(sender As Object, e As EventArgs) Handles chkSelAll.CheckedChanged
        Dim x As Int32
        With vsfCol
            If chkSelAll.Checked AndAlso chkSelAll.Text = "Check All" Then
                For x = 1 To .Rows.Count - 1
                    .SetCellCheck(x, 0, CheckEnum.Checked)
                Next
                chkSelAll.Text = "UnCheck All"
                chkSelAll.Checked = False
            ElseIf chkSelAll.Checked AndAlso chkSelAll.Text = "UnCheck All" Then
                For x = 1 To .Rows.Count - 1
                    .SetCellCheck(x, 0, CheckEnum.Unchecked)
                Next
                chkSelAll.Text = "Check All"
                chkSelAll.Checked = False
            End If
        End With
    End Sub

    Private Sub chkQtyInLtr_CheckedChanged(sender As Object, e As EventArgs) Handles chkQtyInLtr.CheckedChanged
        blnChanged = True
    End Sub

    Private Sub chkValues_CheckedChanged(sender As Object, e As EventArgs) Handles chkValues.CheckedChanged
        blnChanged = True
        Call hideWar()
    End Sub

    Private Sub optPurRate_CheckedChanged(sender As Object, e As EventArgs) Handles optPurRate.CheckedChanged
        blnChanged = True
        Call hideWar()
    End Sub

    Private Sub optSalesRate_CheckedChanged(sender As Object, e As EventArgs) Handles optSalesRate.CheckedChanged
        blnChanged = True
        Call hideWar()
    End Sub

    Private Sub optClDetails_CheckedChanged(sender As Object, e As EventArgs) Handles optClDetails.CheckedChanged
        Call hideWar()
        blnChanged = True
    End Sub

    Private Sub chkTotGW_CheckedChanged(sender As Object, e As EventArgs) Handles chkTotGW.CheckedChanged
        If chkTotGW.Checked = True Then
            Call INIStore("GroupTotal", "", "Y")
        End If
        If chkTotGW.Checked = False Then
            Call INIStore("GroupTotal", "", "N")
        End If
        Call frmMain.saveGridToINI()
    End Sub
    Private Sub valuateWAR()
        Dim strSQL As String, dt As DataTable
        Dim x As Integer, sngQty As Double
        Dim sngValue As Double, sngPrevRate As Double, dblRate As Double
        Dim sngIssValue As Double, sngBalQty As Double, sngBalValue As Double
        Dim intItemid As Long
        vsfWAR.Visible = True
        strSQL = " Select i.itemid, docdate, doctype, RowID, StkMultiplier * u.Qty As Qty, u.rate As docrate,StkMultiplier * u.qty * u.rate as docvalue, 0.00, 0.00"
        strSQL &= " From MstItem i, MstItemGroup g, MstPkg p, UtlStockList u "
        strSQL = strSQL & " where i.ItemID = u.ItemID"
        strSQL = strSQL & " And i.PkgID = p.PkgID"
        strSQL = strSQL & " And i.ItemGroupID = g.ItemGroupId "
        'strSQL = strSQL & " and i.itemid = 1221"
        'strSQL = strSQL & " And DocDate between " & getDateFormatInCond(dtpFrom.Value)
        'strSQL = strSQL & " And  " & getDateFormatInCond(dtpTo.Value)
        strItemList = getItemList()
        If strItemList <> "" Then
            strSQL = strSQL & " And i.ItemID In (" & strItemList & ") "
        End If
        If chkItemGroup.Checked Then
            If strItemGroupList <> "" Then
                strSQL = strSQL & " and i.ItemGroupID in (" & strItemGroupList & ") "
            End If
        End If
        strItemList = getPkgList()
        If strItemList <> "" Then
            strSQL = strSQL & " And i.PKGITEM In (" & strItemList & ") "
        End If
        strSQL &= " Order by itemID, DocDate, RowId"

        dt = clsSVT.getDATATable(strSQL)
        vsfWAR.DataSource = dt
        vsfWAR.Refresh()

        intItemid = 0

        With vsfWAR
            For x = 1 To .Rows.Count - 1
                StMsg("Valuating for item " & .GetData(x, vWarItemID))
                If intItemid <> .GetData(x, vWarItemID) Then
                    sngIssValue = 0
                    sngPrevRate = 0
                    sngValue = 0
                    sngQty = 0
                    sngBalQty = 0 : sngBalValue = 0
                    intItemid = .GetData(x, vWarItemID)
                    Application.DoEvents()
                    .TopRow = x
                End If

                If .GetData(x, vWarDocType) = "PB" Then
                    sngQty = sngQty + .GetData(x, vWarDocQty)
                    sngValue = sngValue + .GetData(x, vWarDocValue)
                    clsSVT.execSQLStatement("Update UtlStockList set DocValue = " & (.GetData(x, vWarDocValue)) & " where RowID = " & .GetData(x, vWarRowID))
                End If

                'If Math.Abs(sngQty) > 0 And Math.Abs(sngValue) > 0 Then
                '    vsfWAR.SetData(x, vWarRate, Math.Abs(Math.Round(sngValue / sngQty, 2)))
                '    sngPrevRate = Math.Abs(Math.Round(sngValue / sngQty, 2))
                'End If

                If .GetData(x, vWarDocType) <> "PB" Then
                    If sngQty > 0 And sngValue > 0 Then
                        dblRate = Math.Round(sngValue / sngQty, 2)
                        vsfWAR.SetData(x, vWarDocValue, Math.Abs(.GetData(x, vWarDocQty)) * dblRate)
                        vsfWAR.SetData(x, vWarRate, Math.Round(dblRate, 2))
                        sngQty = sngQty + .GetData(x, vWarDocQty)
                        sngValue = sngValue + (.GetData(x, vWarDocQty) * dblRate)
                        clsSVT.execSQLStatement("Update UtlStockList set DocValue = " & (.GetData(x, vWarDocQty) * dblRate) & " where RowID = " & .GetData(x, vWarRowID))
                    End If
                End If
                vsfWAR.SetData(x, vWarClBal, sngQty)
                vsfWAR.SetData(x, vWarClValue, sngValue)
            Next x
            vsfWAR.Visible = False
            StMsg()
        End With
    End Sub

    Private Sub vsfWAR_DoubleClick(sender As Object, e As EventArgs) Handles vsfWAR.DoubleClick
        Call ExportToXL(vsfWAR)
    End Sub

    Private Sub hideWar()
        If (optClDetails.Checked And chkValues.Checked) Or optGrpSummary.Checked Then
            optWAR.Visible = True
            optFIFO.Visible = True
        Else
            optWAR.Visible = False
            optFIFO.Visible = False
            If Not optPurRate.Checked And Not optSalesRate.Checked Then
                optPurRate.Checked = True
            End If
        End If
        If optPurRate.Checked Then
            chkTax.Visible = True
        Else
            chkTax.Visible = False
        End If
    End Sub

    Private Sub optWAR_CheckedChanged(sender As Object, e As EventArgs) Handles optWAR.CheckedChanged
        Call hideWar()
    End Sub

    Private Sub optFIFO_CheckedChanged(sender As Object, e As EventArgs) Handles optFIFO.CheckedChanged
        Call hideWar()
    End Sub

    Private Sub ViewGrpSmryData()
        Dim strSQL As String = ""
        Dim strItemList As String = ""
        Dim rs As OleDbDataReader
        With vsfView
            .Rows.Count = 1
            .Rows.Fixed = 1
            .Cols.Count = vGrpTotCol
            .SetData(0, vGrpItemGrpName, "Item Group Name")
            '.SetData(0, vGrpItembrdName, "Item Brand Name")
            .SetData(0, vGrpCategory, "Category")
            .SetData(0, vGrpOPValue, "Opening Value")
            .SetData(0, vGrpClValue, "Closing Value")
        End With
        strSQL = "Select itemcat, itemgroupname, sum(opqty * StkRate) as opValue, sum(clqty * stkrate) as clvalue from ("
        strSQL &= " Select 'Liquor' as ItemCat, ItemGroupName, "
        If optPurRate.Checked Then
            strSQL = strSQL & " isnull((Select top 1 PBRate from vwPBItemLastPrice v where s.ItemID = v.itemid  And pbdate <= " & getDateFormatInCond(dtpTo.Value) & " order by Slno),0) as StkRate "
        ElseIf optFIFO.Checked Then
            strSQL = strSQL & " FifoRate as StkRate"
        Else
            strSQL = strSQL & " IsNull((Select MRP From MstItem x Where x.ItemID=s.ItemID),0) as StkRate"
        End If
        strSQL &= ", sum(Qty * StkMultiplier) as opQty, 0.00 as clQty"
        strSQL = strSQL & " from MstItemGroup g, MstItem i, UtlStockList s "
        strSQL = strSQL & " where s.ItemID = i.ItemID"
        strSQL = strSQL & " And i.ItemGroupID = g.ItemGroupId "
        'strSQL = strSQL & " And i.BrandID = b.BrandId "
        strSQL = strSQL & " And DocDate < " & getDateFormatInCond(dtpFrom.Value)
        strSQL = strSQL & " And i.Inactive='N'"
        strSQL = strSQL & " And g.LiqItem = 'Y'"
        strSQL = strSQL & " group by ItemGroupName, s.itemid"
        If optFIFO.Checked Then strSQL = strSQL & ", FIFORate"

        strSQL &= " Union All "

        strSQL &= "Select 'Liquor' as ItemCat, ItemGroupName, "
        If optPurRate.Checked Then
            strSQL = strSQL & " isnull((Select top 1 PBRate from vwPBItemLastPrice v where s.ItemID = v.itemid  And pbdate <= " & getDateFormatInCond(dtpTo.Value) & " order by Slno),0) as StkRate "
        ElseIf optFIFO.Checked Then
            strSQL = strSQL & " FifoRate as StkRate"
        Else
            strSQL = strSQL & " IsNull((Select MRP From MstItem x Where x.ItemID=s.ItemID),0) as StkRate"
        End If
        strSQL &= ", 0.00, sum(Qty * StkMultiplier) as clQty"
        strSQL = strSQL & " from MstItemGroup g, MstItem i, UtlStockList s"
        strSQL = strSQL & " where s.ItemID = i.ItemID"
        strSQL = strSQL & " And i.ItemGroupID = g.ItemGroupId "
        'strSQL = strSQL & " And i.BrandID = b.BrandId "
        strSQL = strSQL & " And DocDate <= " & getDateFormatInCond(dtpFrom.Value)
        strSQL = strSQL & " And i.Inactive='N'"
        strSQL = strSQL & " And g.LiqItem = 'Y'"
        strSQL = strSQL & " group by ItemGroupName, s.itemid"
        If optFIFO.Checked Then strSQL = strSQL & ", FIFORate"

        strSQL &= " union all "

        strSQL &= "Select 'Non-Liquor' as ItemCat, '',"
        If optPurRate.Checked Then
            strSQL = strSQL & " isnull((Select top 1 PBRate from vwPBItemLastPrice v where s.ItemID = v.itemid  And pbdate <= " & getDateFormatInCond(dtpTo.Value) & " order by Slno),0) as StkRate "
        ElseIf optFIFO.Checked Then
            strSQL = strSQL & " FifoRate as StkRate"
        Else
            strSQL = strSQL & " IsNull((Select MRP From MstItem x Where x.ItemID=s.ItemID),0) as StkRate"
        End If
            strSQL &= ", sum(Qty * StkMultiplier) as opQty, 0.00 as clQty "
            strSQL = strSQL & " from MstItemGroup g, MstItem i, UtlStockList s"
        strSQL = strSQL & " where s.ItemID = i.ItemID"
        strSQL = strSQL & " And i.ItemGroupID = g.ItemGroupId "
        strSQL = strSQL & " And DocDate < " & getDateFormatInCond(dtpFrom.Value)
        strSQL = strSQL & " And i.Inactive='N'"
        strSQL = strSQL & " And g.LiqItem = 'N'"
        strSQL = strSQL & " group by s.itemid"
        If optFIFO.Checked Then strSQL = strSQL & ", FIFORate"

        strSQL &= " Union All "

        strSQL &= "Select 'Non-Liquor' as ItemCat, '',"
        If optPurRate.Checked Then
            strSQL = strSQL & " isnull((Select top 1 PBRate from vwPBItemLastPrice v where s.ItemID = v.itemid  And pbdate <= " & getDateFormatInCond(dtpTo.Value) & " order by Slno),0) as StkRate "
        ElseIf optFIFO.Checked Then
            strSQL = strSQL & " FifoRate as StkRate"
        Else
            strSQL = strSQL & " IsNull((Select MRP From MstItem x Where x.ItemID=s.ItemID),0) as StkRate"
        End If
        strSQL &= ",0.00, sum(Qty * StkMultiplier) as clQty"
        strSQL = strSQL & " from MstItemGroup g, MstItem i, UtlStockList s"
        strSQL = strSQL & " where s.ItemID = i.ItemID"
        strSQL = strSQL & " And i.ItemGroupID = g.ItemGroupId "
        strSQL = strSQL & " And DocDate <= " & getDateFormatInCond(dtpFrom.Value)
        strSQL = strSQL & " And i.Inactive='N'"
        strSQL = strSQL & " And g.LiqItem = 'N' "
        strSQL = strSQL & " group by s.itemid"
        If optFIFO.Checked Then strSQL = strSQL & ", FIFORate"
        strSQL = strSQL & " ) a"

        strSQL = strSQL & " group by itemcat, itemgroupname order by 1,2"

        rs = clsSVT.getDataReaderFromSQL(strSQL)

        If Not rs.HasRows Then
            rs.Close()
            Exit Sub
        End If

        With rs
            Do While .Read
                vsfView.Rows.Count = vsfView.Rows.Count + 1
                vsfView.SetData(vsfView.Rows.Count - 1, vGrpItemGrpName, !ItemGroupName)
                'vsfView.SetData(vsfView.Rows.Count - 1, vGrpItemBrdName, !BrandName)
                vsfView.SetData(vsfView.Rows.Count - 1, vGrpCategory, !ItemCat)
                vsfView.SetData(vsfView.Rows.Count - 1, vGrpOPValue, Format(!opValue, "0.00"))
                vsfView.SetData(vsfView.Rows.Count - 1, vGrpClValue, Format(!ClValue, "0.00"))
            Loop
            .Close()
        End With
        With vsfView
            .SubtotalPosition = C1.Win.C1FlexGrid.SubtotalPositionEnum.BelowData
            .Subtotal(C1.Win.C1FlexGrid.AggregateEnum.Sum, 0, 0, vGrpOPValue)
            .Subtotal(C1.Win.C1FlexGrid.AggregateEnum.Sum, 0, 0, vGrpClValue)
            .AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.RestrictAll
            .Cols(vGrpItemGrpName).AllowMerging = True
            .Cols(vGrpCategory).AllowMerging = True
        End With
        vsfView.AutoSizeCols()
    End Sub

    Private Sub showFifoClDetailed()
        If optFIFO.Checked Then
            Call clsSVT.execSQLStatement("exec mSp_UpdateFIFORate")
        End If


        Dim strSQL As String = ""
        Dim strItemList As String = "", strPkgItemList As String
        Dim sngQty As Double, sngValue As Double
        Dim intRow As Int32

        Dim rs As OleDbDataReader
        With vsfView
            .Rows.Count = 1
            .Rows.Count = 4
            .Rows.Fixed = 4
            .Cols.Count = 0
            .Cols.Count = vDetTotal
            .SetData(0, vDetGrpName, "Item Group Name")
            .SetData(0, vDetItemcode, "Item Code")
            .SetData(0, vDetItemName, "Item Name")
            .SetData(0, vDetItemID, "ItemID")
            .SetData(0, vDetUOM, "PKG")
            .SetData(0, vDetHSN, "HSNCODE")
            .SetData(0, vDetRate, "Rate")
            .SetData(0, vDetRatePerCase, "Rate Per Case")
            .SetData(0, vDetOPValue, "OP Value")
            .SetData(0, vDetPBValue, "PB Value")
            .SetData(0, vDetSRValue, "SR Value")
            .SetData(0, vDetInvValue, "Inv Value")
            .SetData(0, vDetSDValue, "Ded Value")
            .SetData(0, vDetPRValue, "PR Value")

            .Cols(vDetOPValue).Format = "0.00"
            .Cols(vDetPBValue).Format = "0.00"
            .Cols(vDetSRValue).Format = "0.00"
            .Cols(vDetInvValue).Format = "0.00"
            .Cols(vDetSDValue).Format = "0.00"
            .Cols(vDetPRValue).Format = "0.00"
            .Cols(vDetInvRate).Format = "0.00"
            .Cols(vDetClValue).Format = "0.00"

            .SetData(0, vDetInvRate, "Inv Rate")
            .SetData(0, vDetBPercase, "Bottle PerCase")
            .SetData(0, vDetInCase, "In Case")
            .SetData(0, vDetOPQty, "OP Qty")
            .SetData(0, vDetPBQty, "PB Qty")
            .SetData(0, vDetSRQty, "SR Qty")
            .SetData(0, vDetInvQty, "Inv Qty")
            .SetData(0, vDetSDQty, "Ded Qty")
            .SetData(0, vDetPRQty, "PR Qty")
            .SetData(0, vDetClQty, "Cl Qty")
            .SetData(0, vDetClValue, "Cl Value")

            If ChkAROED.Checked Then
                .SetData(0, vDetOPOREDQty, "OP AROED Value")
                .SetData(0, vDetOPTotValue, "OP Total Value")

                .SetData(0, vDetPBAOREDQty, "PB AROED Value")
                .SetData(0, vDetPBTotValue, "PB Total Value")

                .SetData(0, vDetSRAROEDQty, "SP AROED Value")
                .SetData(0, vDetSRTotValue, "SP Total Value")

                .SetData(0, vDetInvAROEDQty, "INV AROED Value")
                .SetData(0, vDetInvTotValue, "INV Total Value")

                .SetData(0, vDetSDAROEDQty, "SD AROED Value")
                .SetData(0, vDetSDTotValue, "SD Total Value")


                .SetData(0, vDetPRAROEDQty, "PR AROED Value")
                .SetData(0, vDetPRTotValue, "PR Total Value")


                .SetData(0, vDetClAROEDQty, "CL AROED Value")
                .SetData(0, vDetClTotValue, "CL Total Value")
            End If

            If strBrandReq <> "Y" Then
                vsfView.Cols(vDetBrdName).Visible = False
            End If

            SetHideCol(vsfView, True, vDetItemID, vDetBPercase, vDetBrdName)
            If Not chkValues.Checked Then
                SetHideCol(vsfView, True, vDetOPValue, vDetPBValue, vDetSRValue, vDetInvValue, vDetInvRate, vDetSDValue, vDetPRValue, vDetClValue, vDetBrdName)
            End If

            SetColAlign(vsfView, svtAlign.Left, vDetGrpName, vDetItemcode, vDetItemName, vDetUOM)
            SetFlexColorFullGrid(vsfView, "", 0)
            SetFlexColorFullGrid(vsfView, "", 1)
            SetFlexColorFullGrid(vsfView, "", 2)
            SetFlexColorFullGrid(vsfView, "", 3)
        End With
        strItemList = getItemList()
        strPkgItemList = getPkgList()
        strSQL = "select i.PkgItem,GroupOrderBy,PkgOrderBy, g.Itemgroupname,i.itemid, i.Itemcode, i.ItemName, "
        strSQL = strSQL & " i.ShortName, p.Pkgcode, BotPerCases, HSNCode,"
        strSQL = strSQL & " sum(opqty) as opqty, sum(OPValue) as opvalue, "
        strSQL = strSQL & " sum(pbqty) as pbqty, sum(pbValue) as pbvalue, "
        strSQL = strSQL & " sum(invqty) as invqty, sum(invValue) as invvalue, "
        strSQL = strSQL & " sum(sdqty) as sdqty, sum(sdValue) as sdvalue, "
        strSQL = strSQL & " sum(srqty) as srqty, sum(srValue) as srvalue, "
        strSQL = strSQL & " sum(prqty) as prqty, sum(PrValue) as prvalue, "
        strSQL = strSQL & " ISNULL(I.AROEDPER,0) AS AROEDPER "
        strSQL = strSQL & " from (	"
        strSQL = strSQL & " Select u.itemid, "
        strSQL = strSQL & " sum(StkMultiplier * u.Qty) as opqty, "
        strSQL = strSQL & " sum(StkMultiplier * docValue) as OPValue, "
        strSQL = strSQL & " convert(numeric(18,2),0) as PBQty, convert(numeric(18,2),0) as PBValue,"
        strSQL = strSQL & " convert(numeric(18,2),0) as invQty, convert(numeric(18,2),0) as invValue,"
        strSQL = strSQL & " convert(numeric(18,2),0) as SDQty, convert(numeric(18,2),0) as SDvalue,"
        strSQL = strSQL & " convert(numeric(18,2),0) as SRQty, convert(numeric(18,2),0) as SRValue,"
        strSQL = strSQL & " convert(numeric(18,2),0) as PRQty, convert(numeric(18,2),0) as PRValue,ISNULL(I.AROEDPER,0) AS AROEDPER "
        strSQL = strSQL & " From MstItem i, MstItemGroup g, MstPkg p, UtlStockList u "
        strSQL = strSQL & " where i.ItemID = u.ItemID"
        strSQL = strSQL & " And i.PkgID = p.PkgID"
        'strSQL = strSQL & " And i.itemid = 3"
        If strItemList <> "" Then
            strSQL = strSQL & " And i.ItemID In (" & strItemList & ") "
        End If
        If chkItemGroup.Checked Then
            If strItemGroupList <> "" Then
                strSQL = strSQL & " And i.ItemGroupID In (" & strItemGroupList & ") "
            End If
        End If
        If strPkgItemList <> "" Then
            strSQL = strSQL & " And i.PKGITEM In (" & strItemList & ") "
        End If
        strSQL = strSQL & " And i.ItemGroupID = g.ItemGroupId"
        strSQL = strSQL & " And DocDate < " & getDateFormatInCond(dtpFrom.Value)
        strSQL = strSQL & " And i.Inactive ='N'"
        strSQL = strSQL & " group by u.itemid,I.AROEDPER "

        strSQL = strSQL & " Union All "

        strSQL = strSQL & " Select u.itemid,"
        strSQL = strSQL & " case when doctype in ('OP') then sum(StkMultiplier * u.Qty) else 0 end  as opQty, case when doctype in ('OP') then sum(StkMultiplier * docvalue) else 0 end as opValue,"
        strSQL = strSQL & " case when doctype in ('PB','ST') then sum(StkMultiplier * u.Qty) else 0 end as PBqty, "
        strSQL = strSQL & " case when doctype in ('PB','ST') then sum(StkMultiplier * docvalue) else 0 end as PBValue, "

        strSQL = strSQL & " case when doctype in ('INV') then sum(StkMultiplier * u.Qty) else 0 end as Invqty, "
        strSQL = strSQL & " 		case when doctype in ('INV') then sum(StkMultiplier * docvalue) else 0 end as InvValue, "

        strSQL = strSQL & " case when doctype in ('STKDED') then sum(StkMultiplier * u.Qty) else 0 end as SDqty, "
        strSQL = strSQL & " case when doctype in ('STKDED') then sum(StkMultiplier * docvalue) else 0 end as SDValue, "

        strSQL = strSQL & " case when doctype in ('SR') then sum(StkMultiplier * u.Qty) else 0 end as SRqty, "
        strSQL = strSQL & " case when doctype in ('SR') then sum(StkMultiplier * docvalue) else 0 end as SRValue, "

        strSQL = strSQL & " case when doctype in ('PR') then sum(StkMultiplier * u.Qty) else 0 end as PRqty, "
        strSQL = strSQL & " case when doctype in ('PR') then sum(StkMultiplier * docvalue) else 0 end as PRValue ,ISNULL(I.AROEDPER,0) "

        strSQL = strSQL & " From MstItem i, MstItemGroup g, MstPkg p, UtlStockList u "
        strSQL = strSQL & " where i.ItemID = u.ItemID"
        strSQL = strSQL & " And i.PkgID = p.PkgID"
        If strItemList <> "" Then
            strSQL = strSQL & " And i.ItemID In (" & strItemList & ") "
        End If
        If chkItemGroup.Checked Then
            If strItemGroupList <> "" Then
                strSQL = strSQL & " And i.ItemGroupID In (" & strItemGroupList & ") "
            End If
        End If
        If strPkgItemList <> "" Then
            strSQL = strSQL & " And i.PKGITEM In (" & strItemList & ") "
        End If
        strSQL = strSQL & " And i.ItemGroupID = g.ItemGroupId "
        strSQL = strSQL & " And DocDate between  " & getDateFormatInCond(dtpFrom.Value) & " And " & getDateFormatInCond(dtpTo.Value)
        strSQL = strSQL & " And i.Inactive='N'"
        strSQL = strSQL & " group by doctype, u.itemid,I.AROEDPER "
        strSQL = strSQL & " ) a, MstItem i, MstItemGroup g, MstPkg p"
        strSQL = strSQL & " where a.itemid = i.itemid"
        strSQL = strSQL & " and i.ITEMGroupID = g.itemgroupid"
        strSQL = strSQL & " and i.PKGID = p.PKGID"
        strSQL = strSQL & " group by i.PkgItem,GroupOrderBy,PkgOrderBy, g.Itemgroupname,i.itemid, i.Itemcode, i.ItemName, "
        strSQL = strSQL & " i.ShortName, p.Pkgcode, BotPerCases, HSNCode,I.AROEDPER"
        strSQL = strSQL & " Order by GroupOrderBy,PkgItem asc, PkgOrderBy, itemname"

        rs = clsSVT.getDataReaderFromSQL(strSQL)

        If Not rs.HasRows Then
            rs.Close()
            Exit Sub
        End If

        With rs
            Do While .Read
                Application.DoEvents()
                vsfView.Rows.Count = vsfView.Rows.Count + 1
                vsfView.TopRow = vsfView.Rows.Count - 1
                intRow = vsfView.Rows.Count - 1

                vsfView.SetData(intRow, vDetGrpName, !Itemgroupname)
                vsfView.SetData(intRow, vDetItemcode, !Itemcode)
                vsfView.SetData(intRow, vDetItemName, !ItemName)
                'vsfView.SetData(intRow, vDetBPercase, !BotPerCases)
                vsfView.SetData(intRow, vDetItemID, !ItemId)
                vsfView.SetData(intRow, vDetUOM, !Pkgcode)
                vsfView.SetData(intRow, vDetHSN, GetVal(!HSNCODE, ""))

                vsfView.SetData(intRow, vDetOPQty, !OPQty)
                vsfView.SetData(intRow, vDetPBQty, !PBQty)
                vsfView.SetData(intRow, vDetInvQty, Math.Abs(!InvQty))
                vsfView.SetData(intRow, vDetSDQty, Math.Abs(!SDQty))
                vsfView.SetData(intRow, vDetSRQty, !SRQty)
                vsfView.SetData(intRow, vDetPRQty, !PRQty)

                sngQty = !OPQty + !PBQty + !InvQty + !SDQty + !SRQty + !PRQty
                vsfView.SetData(intRow, vDetClQty, Val(vsfView.GetData(intRow, vDetClQty)) + sngQty)

                If sngQty = 0 Then
                    sngValue = 0
                Else
                    sngValue = rs!OPValue + rs!PBValue + rs!InvValue + rs!SDValue + rs!SRValue + rs!PRValue
                End If

                '  sngValue = !OPValue + !PBValue + !InvValue + !SDValue + !SRValue + !PRValue
                If Val(vsfView.GetData(intRow, vDetClQty)) <> 0 Then
                    vsfView.SetData(intRow, vDetClValue, Val(vsfView.GetData(intRow, vDetClValue)) + sngValue)
                End If
                If chkValues.Checked = True And chkQtyInLtr.Checked = False Then
                    If Val(vsfView.GetData(intRow, vDetOPQty)) <> 0 Then vsfView.SetData(intRow, vDetOPValue, !OPValue)
                    If Val(vsfView.GetData(intRow, vDetPBQty)) <> 0 Then vsfView.SetData(intRow, vDetPBValue, !PBValue)
                    If Val(vsfView.GetData(intRow, vDetInvQty)) <> 0 Then vsfView.SetData(intRow, vDetInvValue, Math.Abs(!InvValue))
                    If Val(vsfView.GetData(intRow, vDetSDQty)) <> 0 Then vsfView.SetData(intRow, vDetSDValue, !SDValue)
                    If Val(vsfView.GetData(intRow, vDetSRQty)) <> 0 Then vsfView.SetData(intRow, vDetSRValue, !SRValue)
                    If Val(vsfView.GetData(intRow, vDetPRQty)) <> 0 Then vsfView.SetData(intRow, vDetPRValue, !PRValue)
                End If
                If chkValues.Checked = True And ChkAROED.Checked = True Then
                    vsfView.SetData(intRow, vDetOPOREDQty, Math.Round(Val(vsfView.GetData(intRow, vDetOPQty)) * (rs!AROEDPER), 2))
                    vsfView.SetData(intRow, vDetOPTotValue, Math.Round(Val(vsfView.GetData(intRow, vDetOPOREDQty)) + Val(vsfView.GetData(intRow, vDetOPValue)), 2))

                    vsfView.SetData(intRow, vDetPBAOREDQty, Math.Round(Val(vsfView.GetData(intRow, vDetPBQty)) * (rs!AROEDPER), 2))
                    vsfView.SetData(intRow, vDetPBTotValue, Math.Round(Val(vsfView.GetData(intRow, vDetPBAOREDQty)) + Val(vsfView.GetData(intRow, vDetPBValue)), 2))

                    vsfView.SetData(intRow, vDetSRAROEDQty, Math.Round(Val(vsfView.GetData(intRow, vDetSRQty)) * !AROEDPER, 2))
                    vsfView.SetData(intRow, vDetSRTotValue, Math.Round(Val(vsfView.GetData(intRow, vDetSRAROEDQty)) + Val(vsfView.GetData(intRow, vDetSRValue)), 2))
                    vsfView.SetData(intRow, vDetInvAROEDQty, Math.Round(Val(vsfView.GetData(intRow, vDetInvQty)) * !AROEDPER, 2))
                    vsfView.SetData(intRow, vDetInvTotValue, Math.Round(Val(vsfView.GetData(intRow, vDetInvAROEDQty)) + Val(vsfView.GetData(intRow, vDetInvValue)), 2))

                    vsfView.SetData(intRow, vDetSDAROEDQty, Math.Round(Val(vsfView.GetData(intRow, vDetSDQty)) * !AROEDPER, 2))
                    vsfView.SetData(intRow, vDetSDTotValue, Math.Round(Val(vsfView.GetData(intRow, vDetSDAROEDQty)) + Val(vsfView.GetData(intRow, vDetSDValue)), 2))

                    vsfView.SetData(intRow, vDetPRAROEDQty, Math.Round(Val(vsfView.GetData(intRow, vDetPRQty)) * !AROEDPER, 2))
                    vsfView.SetData(intRow, vDetPRTotValue, Math.Round(Val(vsfView.GetData(intRow, vDetPRAROEDQty)) + Val(vsfView.GetData(intRow, vDetPRValue)), 2))

                    vsfView.SetData(intRow, vDetClAROEDQty, Math.Round(Val(vsfView.GetData(intRow, vDetClQty)) * !AROEDPER, 2))
                    If (vsfView.GetData(intRow, vDetClQty)) <> 0 Then
                        vsfView.SetData(intRow, vDetClTotValue, Math.Round(Val(vsfView.GetData(intRow, vDetClAROEDQty)) + Val(vsfView.GetData(intRow, vDetClValue)), 2))
                    End If
                End If

            Loop


            .Close()
        End With

        With vsfView
            For k = vDetOPQty To vDetClQty
                If InStr(.GetData(0, k), "Qty") > 0 Then
                    .Cols(k).Format = "#"
                End If
            Next
            RemoveVsfRows(vsfView)
            .SubtotalPosition = C1.Win.C1FlexGrid.SubtotalPositionEnum.BelowData
            For k = vDetOPQty To vsfView.Cols.Count - 1
                If Strings.Right(.GetData(0, k), 4) <> "Rate" Then
                    If chkTotGW.Checked Then
                        .Subtotal(C1.Win.C1FlexGrid.AggregateEnum.Sum, 0, vDetGrpName, k)
                    End If
                    .Subtotal(C1.Win.C1FlexGrid.AggregateEnum.Sum, 1, -1, k)
                End If
            Next k
            .AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.RestrictAll
            .Cols(vDetGrpName).AllowMerging = True
        End With
        Call initVsfForHdrs(vsfView, Me.Text, "Stock Balance Date From " & getDateFormatInInsert(dtpFrom.Value) & " To " & getDateFormatInInsert(dtpTo.Value))
        vsfView.AutoSizeCols()
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

    Private Sub btnStk_Click(sender As Object, e As EventArgs) Handles btnStk.Click
        Dim strsql As String = ""
        Call showWaitCursor()
        clsSVT.execSQLStatement("Delete from MstOpBalance  Where FinYear = 25")
        clsSVT.execSQLStatement("Delete from UtlStockList Where DocType = 'OP' and DocDate = '2025-04-01'")

        With vsfView
            Dim DblOpRate As Double = 0
            For i As Int32 = 4 To .Rows.Count - 1
                If Val(.GetData(i, vDetClQty)) > 0 Then
                    strsql = "Insert Into MstOpBalance (FinYear, LocID, ItemID, RcptDate, RcptRef, OpQty, OpRate, Remarks )"
                    strsql &= " Values ("
                    strsql &= " 25, 1, "
                    strsql &= Val(.GetData(i, vDetItemID)) & ","
                    strsql &= "'2025-04-01',"
                    strsql &= "'Stock Carry Forward From Previous Fin Year',"
                    strsql &= "" & Val(.GetData(i, vDetClQty)) & ","
                    If Val(.GetData(i, vDetClValue)) <> 0 Then
                        DblOpRate = 0
                        DblOpRate = Math.Abs(Val(.GetData(i, vDetClValue)) / Val(.GetData(i, vDetClQty)))
                        strsql &= "" & DblOpRate & ","
                    Else
                        strsql &= "0,"
                    End If
                    strsql &= "'Stock Carry Forward From Previous Fin Year')"
                    clsSVT.getDataReaderFromSQL(strsql)
                End If
            Next

            strsql = ""

            strsql = "Insert Into UtlStockList (DocID, DocType, DocNo, DocDate, ItemID, LocID, StkMultiplier, "
            strsql &= " Qty, Rate, Particulars, FifoRate, DelFlag, ReWrite, DocValue, FifoUsedQty) "
            strsql &= " select recid, 'OP', recid, RcptDate, itemid, locid, 1, OpQty, oprate, Remarks, oprate, 'N', 'N',"
            strsql &= " OpQty * OpRate, 0 from MstOpBalance  Where FinYear = 25"
            clsSVT.execSQLStatement(strsql)

            clsSVT.execSQLStatement("Truncate Table TrnQOH")

            strsql = ""

            strsql ="Insert Into TrnQOH (ItemID, LocID,  CLBal) Select ItemID, LocID,  Sum(StkMultiplier * Qty) as Qty from UtlStockList"
            strsql &= " Where DocID = 25 And DocDate = '2025-04-01' "
            strsql &= " Group by ItemID, LocID  having Sum(StkMultiplier * Qty) > 0"
            strsql &= " order by locid,ItemID"
            clsSVT.execSQLStatement(strsql)


            'clsSVT.execSQLStatement("Delete from UtlStockList Where DocDate < '2026-04-01'")
            Call showNormalCursor()
            MsgBox("updated")
        End With
        MsgBox("Stock Carry Forward SucessFully", MsgBoxStyle.Information, Me.Text)
    End Sub
End Class
