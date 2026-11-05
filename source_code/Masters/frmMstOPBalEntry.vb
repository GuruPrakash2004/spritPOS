Imports System.Drawing.Printing
Imports C1.Win.C1FlexGrid

Public Class frmMstOPBalEntry
    Dim clsSVt As New svtLibrary
    Dim Mode As String
    Dim strRights As String
    Dim strOrgQry As String

    'declarations for detail grid

    Const vEditRecID = 0
    Const vEditItemId = 1
    Const vEditLocID = 2
    Const vEditLocCode = 3
    Const vEditItemCode = 4
    Const vEditItemName = 5
    Const vEditUOM = 6
    Const vEditRcptDate = 7
    Const vEditRcptRef = 8
    Const vEditQty = 9
    Const vEditRate = 10
    Const vEditVal = 11
    Const vEditRmks = 12
    Const vEditTotal = 13

    Public FrmType As String
    Dim strFrmName As String
    Dim strFrmCaption As String
    Dim strFrmTblName As String
    Dim strFrmMstTblName As String
    Dim strFrmMsg As String
    Dim strFldCode As String
    Dim strFldId As String
    Dim strFrmPfx As String
    Dim strParameter As String

    'vsfEdit consts

    Const vDetRecID = 0
    Const vDetItemID = 1
    Const vDetLocID = 2
    Const vDetLocCode = 3
    Const vDetItemCode = 4
    Const vDetItemName = 5
    Const vDetUOM = 6
    Const vDetQty = 7
    Const vdetRate = 8
    Const vDetVal = 9
    Const vDetRcptDate = 10
    Const vDetRcptRef = 11
    Const vDetRmks = 12
    Const vDetTotal = 13

    Const vVItemID = 0
    Const vVItemCode = 1
    Const vVItemName = 2
    Const vVItemGroupName = 3

    Dim blnFreezeDoc As Boolean
    Dim iNumOfDays As Integer
    Dim strSHForm As String
    Dim dtSearch As DataTable
    Dim dvSearch As DataView
    Dim dtDetSearch As DataTable
    Dim dvDetSearch As DataView
    Dim intMaxDocID As Long
    Dim svtTrn As oledb.oledbTransaction

    Dim strItemRate As String
    Dim blnItemName As Boolean
    Dim glngItemId As Long
    Dim glngLocId As Long
    Dim intdetrows As Integer
    Dim intItemID As Integer
    Dim intRecID As Integer
    Dim intLocID As Integer
    Dim intMenuID As Int32

    Private Sub frmMstOPBalEntry_FormClosing(sender As Object, e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        If Mode = "A" Or Mode = "M" Then
            If MsgBox("Are you sure you want to exit the form without saving?!", MsgBoxStyle.Question _
                      + MsgBoxStyle.YesNo + vbDefaultButton2, Me.Text) = MsgBoxResult.No Then
                e.Cancel = True
                Exit Sub
            End If
        End If
    End Sub

    Private Sub frmMstOPBalEntry_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        'If e.KeyCode = Keys.F8 And e.Control = True Then
        '    If validFlds() Then
        '        Call SaveDetails()
        '        btnSave.PerformClick()
        '    End If
        'End If
    End Sub

    Private Sub frmMstOPBalEntry_KeyUp(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyUp
        If e.KeyCode = Keys.F9 And e.Control = True Then
            Call initFormSize(Me, "MstOPBalEntry", True, 913, 481, 0, 0)
            MsgBox("Restored Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
        If e.KeyCode = Keys.F8 And e.Control = True Then
            Call SaveActFormSettings(Me, "MstOPBalEntry")
            Call SaveActFormVsfDet(Me, "OP-BalEntry", "Detail", vsfDet)
            Call SaveActFormVsfDet(Me, "OP-BalEntry", "Search", vsfSearch)
            Call SaveActFormVsfDet(Me, "OP-BalEntry", "Edit", vsfEdit)
            MsgBox("Saved Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
        If e.KeyCode = Keys.E And e.Alt = True Or e.Control = True Then
            Call ExportToXL(vsfDet)
        End If
    End Sub

    Private Sub frmMstOPBalEntry_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        Dim ToolTip As New ToolTip

        intMenuID = clsSVt.getDataLongValue("select MenuItemID from MstMenu where MenuItemCaption = 'Item Opening Balance'", 0, False)
        strRights = getUserRights(intMenuID)

        Call initScreen()
        Mode = "V"
        Me.KeyPreview = True

        fraSearch.Visible = False
        vsfDet.AllowEditing = False
        EnableFields(True, 1, False, 2)
        initItemVsfCombo()

        syncGridCols()
        Call initGridStyles(Me)
        KeyPreview = True
        ToolTip.SetToolTip(Me.btnAdd, "Add:Alt+A")
        ToolTip.SetToolTip(Me.btnRemoveRow, "RemoveRow:Alt+R")
        ToolTip.SetToolTip(Me.btnExportToXL, "ExportToXL:Alt+E")
        ToolTip.SetToolTip(Me.btnClose, "Close:Alt+O")

        Call initFormSize(Me, "MstOPBalEntry", False, 913, 481, 0, 0)
        Call initFormVsfDet(Me, "OP-BalEntry", vsfDet, "Detail")
        Call initFormVsfDet(Me, "OP-BalEntry", vsfSearch, "Search")
        Call ColorChange()
        Call fillFormProperties(Me)
        lblFrm.ForeColor = Color.White
        Call ChangeFontSize(Me)
        lblFrm.Font = New Font("Microsoft Sans Serif", 14, FontStyle.Bold)

    End Sub

    Private Sub EnableFields(blnEnable As Boolean, argType As Integer, Optional ByVal blnEnable2 As Boolean = False, Optional ByVal argType2 As Integer = 0)
        Select Case argType
            Case 1
                pnlCtrls.Visible = blnEnable

            Case 2
                btnSave.Visible = blnEnable
                btnCancel.Visible = blnEnable
                vsfEdit.AllowEditing = blnEnable
        End Select
        Select Case argType2
            Case 1
                pnlCtrls.Visible = blnEnable2

            Case 2
                btnSave.Visible = blnEnable2
                btnCancel.Visible = blnEnable2
                vsfEdit.AllowEditing = blnEnable2
        End Select
    End Sub

    Private Sub initScreen()
        Mode = "V"

        With vsfEdit
            .Cols.Count = 2
            .Rows.Count = vEditTotal

            .SetData(vEditRecID, 0, "Rec ID")
            .SetData(vEditItemId, 0, "Item ID")
            .SetData(vEditLocID, 0, "Loc ID")
            .SetData(vEditUOM, 0, "UOM")
            .SetData(vEditItemCode, 0, "Item Code")
            .SetData(vEditItemName, 0, "Item Name")
            .SetData(vEditLocCode, 0, "Location")
            .SetData(vEditRcptDate, 0, "Receipt Date")
            .SetData(vEditRcptRef, 0, "Receipt Ref")
            .SetData(vEditQty, 0, "Qty")
            .SetData(vEditRate, 0, "Rate")
            .SetData(vEditVal, 0, "Value")
            .SetData(vEditRmks, 0, "Remarks")

            SetHideRow(vsfEdit, True, vEditItemId, vEditRecID, vEditLocID)

            Call SetFlexRowColor(vsfEdit, "", vEditVal, vEditItemName, vEditUOM)
            Call SetFlexRowColor(vsfEdit, "M", vEditItemCode, vEditQty, vEditRate, vEditLocCode, vEditRcptDate)

            .Rows(vEditRate).Format = "0.00"
            .Rows(vEditRate).DataType = GetType(Double)
            .Rows(vEditQty).DataType = GetType(Double)
            .Rows(vEditQty).Format = "0.00"
            .Rows(vEditVal).DataType = GetType(Double)
            .Rows(vEditVal).Format = "0.00"
            SetColWidth(vsfEdit, 150, 200)
        End With


        Call loadMenuIcon()
        Call fillDtlGrid()
    End Sub
    Public Sub loadMenuIcon()
        'Dim cmd As oledb.oledbCommand 
        'Dim strSQL As String
        'strSQL = "Select isnull(MenuImage,'') from MstMenu where MenuItemId=" & intMenuID
        'cmd = New oledb.oledbCommand (strSQL, svtConn)
        'Dim imageData As Byte() = DirectCast(cmd.ExecuteScalar(), Byte())

        'If imageData.Length = 0 Then Exit Sub
        'If Not imageData Is Nothing Then
        '    Try
        '        Dim ms As New MemoryStream(imageData)
        '        Dim pic As Icon = Icon.ToBitmap
        '        'Me.Icon = Image.FromStream(ms, True)
        '        ' Me.Icon = CType(Icon.FromStream(ms, True), System.Drawing.Icon)
        '    Catch
        '    End Try
        'End If

    End Sub
    Private Sub vsfEdit_AfterEdit(sender As Object, e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfEdit.AfterEdit
        Mode = "M"
        'If Trim(vsfEdit.EditMask) = "/  /" Or Trim(vsfEdit.EditMask) = "" Or Trim(vsfEdit.EditMask) = "##/##/####" Then
        '    vsfEdit.SetData(e.Row, e.Col, "")
        '    Exit Sub
        'End If
        With vsfEdit
            If e.Row = vEditQty Or e.Row = vEditRate Then
                .SetData(vEditVal, 1, svtModule.SVTROUND(Val(.GetData(vEditRate, 1)) * Val(.GetData(vEditQty, 1)), 2))
            End If

            'If e.Row = vEditRate And Val(.GetData(vEditRate, 1)) > 0 Then
            '    .SetData(vEditBatchNo, 1, "B-" & Format(Val(.GetData(vEditRate, 1)), "0.00"))
            'End If

        End With

        'mpdified by chandan
        If vsfEdit.Col = vsfEdit.Cols.Count - 1 AndAlso vsfEdit.Row = vsfEdit.Rows.Count - 1 AndAlso btnSave.Visible AndAlso btnCancel.Visible Then
            btnSave.Focus()
        End If
    End Sub

    Private Sub vsfEdit_BeforeEdit(sender As Object, e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfEdit.BeforeEdit
        If Not (Mode = "M" Or Mode = "A") Then Exit Sub
        If (Not chkUserRights(strRights, chkRightsPos.AddRights, Me.Text)) Then Exit Sub
        vsfEdit.EditMask = ""
        vsfEdit.ComboList = ""

        Select Case e.Row
            Case vEditItemName, vEditLocID, vEditItemId, vEditRecID, vEditVal, vDetUOM
                e.Cancel = True
            Case vEditLocCode
                vsfEdit.ComboList = "..."
            Case vEditVal
                e.Cancel = True
            Case vEditRcptRef
                'vsfEdit.MaximumSize = 10
            Case vEditItemCode
                vsfEdit.ComboList = "..."

        End Select
    End Sub

    Private Sub initItemVsfCombo()
        Call showWaitCursor()
        Dim StrQry As String
        StrQry = "Select  ItemCode, ItemName"
        StrQry &= " from MstItem"
        StrQry &= " where InActive='N' and Itemid>0 "
        StrQry &= " order by 1"
        dtSearch = clsSVt.getDATATable(StrQry)
        dvSearch = New DataView(dtSearch)
        vsfView.DataSource = dvSearch
        Call showNormalCursor()

        vsfSearch.Cols.Count = vsfDet.Cols.Count
    End Sub

    Private Sub enableSaveCancel(ByVal argBln As Boolean)
        btnSave.Visible = argBln
        btnCancel.Visible = argBln
    End Sub

    Private Sub vsfSearch_ChangeEdit(ByVal sender As Object, ByVal e As System.EventArgs) Handles vsfSearch.ChangeEdit
        Call applyFilterFromGrid()
    End Sub

    Private Sub vsfSearchDet_ChangeEdit(ByVal sender As Object, ByVal e As System.EventArgs) Handles vsfSearchDet.ChangeEdit
        Call applyFilterFromSearchGrid()
    End Sub

    Private Function applyFilterFromGrid() As Boolean
        Dim StrQry As String, strValue As String
        Dim i As Integer

        applyFilterFromGrid = False
        StrQry = ""
        With vsfSearch
            For i = 0 To .Cols.Count - 1
                strValue = ""
                If i = .ColSel Then
                    If .Editor.Text <> "" Then
                        strValue = Trim(.Editor.Text)
                    End If
                Else
                    If .GetData(0, i) <> "" Then
                        strValue = Trim(.GetData(0, i))
                    End If
                End If
                If strValue <> "" Then
                    If StrQry <> "" Then StrQry = StrQry & " and "
                    StrQry = StrQry & vsfView.GetData(0, i) & " like '*" & strValue & "*'"
                End If
            Next i
        End With
        dvSearch.RowFilter = StrQry
        Call syncGridCols()
        applyFilterFromGrid = True
    End Function

    Private Function applyFilterFromSearchGrid() As Boolean
        Dim StrQry As String, strValue As String
        Dim i As Integer
        applyFilterFromSearchGrid = False
        StrQry = ""
        With vsfSearchDet
            For i = 0 To .Cols.Count - 1
                strValue = ""
                If i = .ColSel Then
                    If .Editor.Text <> "" Then
                        strValue = Trim(.Editor.Text)
                    End If
                Else
                    If .GetData(0, i) <> "" Then
                        strValue = Trim(.GetData(0, i))
                    End If
                End If
                If strValue <> "" Then
                    If StrQry <> "" Then StrQry = StrQry & " and "
                    If vsfDet.Cols(i).DataType = GetType(Decimal) Or vsfDet.Cols(i).DataType = GetType(Integer) Or vsfDet.Cols(i).DataType = GetType(Date) Then
                        Try
                            StrQry = StrQry & "Convert(" & vsfDet.GetData(0, i) & ",'System.String')" & " like '" & strValue.Replace("'", "`") & "*'"
                        Catch ex As Exception
                            dvDetSearch.RowFilter = "1 <> 1"
                            Exit Function
                        End Try
                    Else
                        StrQry = StrQry & vsfDet.GetData(0, i) & " like '%" & strValue.Replace("'", "`") & "%'"
                    End If
                End If
            Next i
        End With
        dvDetSearch.RowFilter = StrQry
        Call syncGridCols()
        applyFilterFromSearchGrid = True
    End Function

    Private Sub vsfSearch_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles vsfSearch.KeyUp
        If e.KeyCode = Keys.Down Then
            vsfView.Focus()
            If vsfView.Rows.Count > 1 Then
                vsfView.Row = 1
            End If
        End If

        If e.KeyCode = Keys.Return Then
            If vsfView.Rows.Count > 1 Then
                vsfView.Row = 1
                Call loadItemToDtl()
                Exit Sub
            End If
        End If

        If e.KeyCode = Keys.Escape Then
            Call closefraComboItem(False)
        End If

    End Sub

    Private Sub loadItemToDtl()
        If Not FillItemDet(vsfView.GetData(vsfView.Row, 0), vsfDet.Row) Then
            'MsgBox("Invalid Item!!", vbInformation, Me.Text)
            Exit Sub
        End If
        Call closefraComboItem(True)
    End Sub

    Private Sub closefraComboItem(ByVal argSetFocus As Boolean)
        fraMain.Enabled = True
        vsfDet.Enabled = True
        fraSearch.Visible = False
        vsfDet.Focus()
    End Sub

    Private Sub vsfSearch_KeyUpEdit(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.KeyEditEventArgs) Handles vsfSearch.KeyUpEdit
        If e.KeyCode = Keys.Down Then
            vsfView.Focus()
            If vsfView.Rows.Count > 1 Then
                vsfView.Row = 1
            End If
        End If
        If e.KeyCode = Keys.Return Then
            If vsfView.Rows.Count > 1 Then
                vsfView.Row = 1
                Call loadItemToDtl()
                Exit Sub
            End If
        End If

        If e.KeyCode = Keys.Escape Then
            Call closefraComboItem(False)
        End If

    End Sub

    Private Function FillItemDet(ByVal argItem As String, ByVal argRow As Long) As Boolean
        Dim rsDummy As oledb.oledbDataReader
        Dim strsql As String
        Dim blnInActive As Boolean
        FillItemDet = False
        Dim intRateVal As Integer ',strRateFld As String

        With vsfEdit
            If argItem = "" Then
                Exit Function
            End If
            intItemID = clsSVt.getDataLongValue("Select ItemID from MstItem where ItemCode = '" & Trim(argItem) & "'", 0, blnInActive)
            If intItemID = 0 Then
                'MsgBox("ItemCode does not exist!!", vbInformation, Me.Text)
                Exit Function
            End If

            Call clsSVt.getDataLongValue("Select ItemID from MstItem where InActive = 'Y' and ItemCode = '" & Trim(argItem) & "'", 0, blnInActive)

            If blnInActive Then
                MsgBox("This Item is InActive! Please select an Active Item from the list.", vbInformation, Me.Text)
                Exit Function
            End If

            strsql = "Select i.*,UOMCode from "
            strsql = strsql & "MstItem i,MstUOM u"

            strsql = strsql & " Where ItemCode = '" & Trim(argItem) & "'"
            strsql = strsql & " and i.UOMID=u.UOMID"

            rsDummy = clsSVt.getDataReaderFromSQL(strsql)

            With rsDummy
                If Not .HasRows Then
                    MsgBox("Item does not exist!!", vbInformation, Me.Text)
                    Exit Function
                End If
                If .Read() Then
                    vsfEdit.SetData(vEditItemId, vsfEdit.Col, rsDummy("ItemID"))
                    vsfEdit.SetData(vEditItemName, vsfEdit.Col, rsDummy("Itemname"))
                    vsfEdit.SetData(vEditItemCode, vsfEdit.Col, rsDummy("ItemCode"))
                    vsfEdit.SetData(vEditUOM, vsfEdit.Col, rsDummy("UOMCode"))
                    vsfEdit.SetData(vEditRate, vsfEdit.Col, intRateVal)
                    If vsfEdit.GetData(vEditRcptDate, vsfEdit.Col) Is Nothing Then
                        vsfEdit.SetData(vEditRcptDate, vsfEdit.Col, Format(gSettings.gCompStDt, "dd/MM/yyyy"))
                    End If
                Else
                    FillItemDet = False
                    Exit Function
                End If
                .Close()
            End With
        End With
        vsfEdit.Row = vEditRcptDate
        vsfEdit.Col = 1
        vsfEdit.StartEditing()

        'modified by chandan
        'For i = 1 To vsfSearchDet.Cols.Count - 1
        '    vsfSearchDet.SetData(0, i, "")
        'Next

        rsDummy = Nothing
        FillItemDet = True
    End Function

    Private Sub vsfSearch_RowColChange(ByVal sender As Object, ByVal e As System.EventArgs) Handles vsfSearch.RowColChange
        vsfView.Col = vsfSearch.Col
        vsfView.Refresh()
    End Sub

    Private Sub vsfEdit_CellButtonClick(sender As Object, e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfEdit.CellButtonClick
        If Not (Mode = "M" Or Mode = "A") Then Exit Sub
        If e.Row = vEditItemCode Then

            fraSearch.Top = vsfEdit.Top + vsfEdit.Rows(e.Row).Top + (vsfEdit.Rows(e.Row).Height) + 20
            fraSearch.Left = vsfEdit.Left + vsfEdit.Cols(e.Col).Left
            fraSearch.BringToFront()

            fraSearch.Visible = True

            vsfSearch.Focus()
            vsfSearch.Col = 0
            vsfSearch.LeftCol = 0
            vsfSearch.Text = ""
            vsfSearch.Clear(C1.Win.C1FlexGrid.ClearFlags.All)
            vsfView.Col = 0
            vsfView.LeftCol = 0
            Call syncGridCols()
            vsfSearch.Text = vsfEdit.GetData(vDetItemCode, e.Col)
            vsfSearch.StartEditing()
            
            Call applyFilterFromGrid()
            '''Changed
            'With vsfEdit
            '    .Row = vEditRcptRef
            '    .Col = 1
            '    .Focus()
            'End With
            '''Changed
            Exit Sub
        End If

        If e.Row = vEditLocCode Then
            Dim strSQL As String
            Dim frmUS As New frmUtlSingleSel

            txtLocID.Text = ""

            strSQL = "SELECT  LocID, LocCode, LocName"
            strSQL = strSQL & " FROM MstLoc "
            strSQL = strSQL & " where LocID > 0 "
            strSQL = strSQL & " and InActive <> 'Y' "
            strSQL = strSQL & " order by Loccode "

            With frmUS
                .Text = "Select from... "
                Call .FillViewVsf(Me, strSQL, txtLocID, 0, 0)
                .StartPosition = FormStartPosition.CenterScreen
                .ShowDialog()
                .vsfDet.Focus()
            End With

            If Val(txtLocID.Text) > 0 Then
                vsfEdit.SetData(vEditLocCode, 1, clsSVt.getDataStringValue("Select Loccode from MstLoc where LocID =" & Val(txtLocID.Text), "", False))
                vsfEdit.SetData(vEditLocID, 1, Val(txtLocID.Text))
            End If
            Exit Sub
        End If
    End Sub

    Private Sub syncGridCols()
        Dim i As Integer
        With vsfView
            vsfSearch.Cols.Count = vsfView.Cols.Count
            For i = 0 To .Cols.Count - 1
                vsfSearch.Cols(i).Width = vsfView.Cols(i).Width
                vsfSearch.Cols(i).Visible = vsfView.Cols(i).Visible
            Next i
        End With
        With vsfDet
            vsfSearchDet.Cols.Count = vsfDet.Cols.Count
            For i = 0 To .Cols.Count - 1
                vsfSearchDet.Cols(i).Width = vsfDet.Cols(i).Width
                vsfSearchDet.Cols(i).Visible = vsfDet.Cols(i).Visible
            Next
        End With
    End Sub

    Private Sub btnAdd_Click(sender As Object, e As System.EventArgs) Handles btnAdd.Click

        If (Not chkUserRights(strRights, chkRightsPos.AddRights, Me.Text)) Then Exit Sub

        btnAdd.Enabled = True
        btnRemoveRow.Enabled = True
        btnSave.Enabled = True
        EnableFields(True, 2, False, 1)
        With vsfEdit
            .Cols.Count = 1
            .Cols.Count = 2
            .Focus()
            .Row = vEditLocCode
            .Col = 1
        End With
        Mode = "A"
        lblFrm.Text = "Add:" & Me.Text
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As System.EventArgs) Handles btnCancel.Click
        With vsfEdit
            .Cols.Count = 1
            .Cols.Count = 2

            .Row = 0
            .Col = 1
            .Cols(1).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.LeftCenter
            .Focus()
        End With
        EnableFields(True, 1, False, 2)
        btnAdd.Focus()
        lblFrm.Text = "View:" & Me.Text
    End Sub

    Private Sub vsfView_AfterScroll(sender As Object, e As C1.Win.C1FlexGrid.RangeEventArgs) Handles vsfView.AfterScroll
        vsfSearch.LeftCol = vsfView.LeftCol
    End Sub

    Private Sub vsfDet_AfterEdit(sender As Object, e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfDet.AfterEdit
        With vsfDet
            .SetData(e.Row, vDetVal, GetVal(.GetData(e.Row, vDetQty)) * GetVal(.GetData(e.Row, vdetRate)))
        End With
    End Sub

    Private Sub vsfDet_AfterResizeColumn(sender As Object, e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfDet.AfterResizeColumn
        syncGridCols()
    End Sub

    Private Sub vsfDet_AfterScroll(sender As Object, e As C1.Win.C1FlexGrid.RangeEventArgs) Handles vsfDet.AfterScroll
        vsfSearchDet.LeftCol = vsfDet.LeftCol
        syncGridCols()
    End Sub

    Private Sub fraSearch_Leave(sender As Object, e As System.EventArgs) Handles fraSearch.Leave
        Call closefraComboItem(False)
    End Sub

    Private Sub vsfView_DoubleClick(sender As Object, e As System.EventArgs) Handles vsfView.DoubleClick
        If vsfView.Row > 0 Then
            If Not FillItemDet(vsfView.GetData(vsfView.Row, 0), vsfDet.Row) Then
                MsgBox("Invalid Item!!", vbInformation, Me.Text)
                Exit Sub
            End If
            Call closefraComboItem(True)
            ''''Changed Code
            With vsfEdit
                .Row = vEditRcptRef
                .Col = 1
                .Focus()
            End With
            ''''Changed Code
        End If
    End Sub

    Private Sub vsfView_KeyUp(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles vsfView.KeyUp
        If e.KeyCode = Keys.Escape Then
            Call closefraComboItem(False)
        End If

        If vsfView.Row > 0 Then
            If e.KeyCode = Keys.Return Or e.KeyCode = Keys.Tab Then
                If Not FillItemDet(vsfView.GetData(vsfView.Row, 0), vsfDet.Row) Then
                    MsgBox("Invalid Item!!", vbInformation, Me.Text)
                    Exit Sub
                End If
                Call closefraComboItem(True)
                '''''Changed Code
                With vsfEdit
                    .Row = vEditRcptRef
                    .Col = 1
                    .Focus()
                End With
                '''''Changed Code
            End If
        End If
    End Sub

    Private Sub btnSave_Click(sender As Object, e As System.EventArgs) Handles btnSave.Click

        ' If Not chkUserRights(strRights, chkRightsPos.ViewRights, Me.Text) Then Exit Sub
        If validFlds() Then
            If SaveDetails() Then
                MsgBox("Data is saved!!", vbInformation, Me.Text)
                'For i = 1 To vsfDet.Rows.Count - 1
                '    For j = 1 To vsfDet.Cols.Count - 1
                '        Select Case j
                '            Case vDetItemCode, vDetItemName, vDetRcptRef
                '                vsfDet.SetData(i, j, Replace(vsfDet.GetData(i, j), "`", "'"))
                '        End Select
                '    Next j
                'Next i
                btnCancel.PerformClick()
                fillDtlGrid()
                Mode = "V"
            End If
        End If
        lblFrm.Text = "View:" & Me.Text
    End Sub

    Private Sub vsfEdit_EnterCell()
        If (Not (Mode = "A" Or Mode = "M")) Then Exit Sub
        With vsfEdit
            Select Case .Row
                Case vEditRate, vEditRcptDate
                    .StartEditing()
            End Select
        End With

    End Sub

    Private Function validFlds() As Boolean
        validFlds = False

        With vsfEdit
            If Trim(vsfEdit.GetData(vEditLocCode, 1)) = "" Then
                MsgBox("Location not selected", vbInformation, Me.Text)
                .Focus()
                .Row = vEditLocCode
                .Col = 1
                vsfEdit.Focus()
                Exit Function
            End If

            Dim intLocID As Int32
            intLocID = clsSVt.getDataLongValue("Select LocID from MstLoc where LocCode = '" & Trim(vsfEdit.GetData(vEditLocCode, 1)) & "'", 0, False)
            If intLocID <= 0 Then
                MsgBox("Location not selected", vbInformation, Me.Text)
                .Focus()
                .Row = vEditLocCode
                .Col = 1
                vsfEdit.Focus()
                Exit Function
            Else
                .SetData(vEditLocID, 1, intLocID)
            End If

            If .GetData(vEditItemCode, 1) = "" Or
                Trim(.GetData(vEditItemName, 1)) = "" Then
                MsgBox("Item not selected", vbExclamation, Me.Text)
                .Focus()
                .Row = vEditItemCode
                .Col = 1
                vsfEdit.Focus()
                Exit Function
            End If


            If Trim(.GetData(vEditRcptDate, 1)) = "##/##/####" Or Trim(.GetData(vEditRcptDate, 1)) = "/  /" Or Trim(.GetData(vEditRcptDate, 1)) = "" Then
                MsgBox("Receipt Date cannot be empty", vbExclamation, Me.Text)
                .Focus()
                .Row = vEditRcptDate
                .Col = 1
                Exit Function
            End If

            If Val(.GetData(vEditQty, 1)) <= 0 Then
                MsgBox("Please enter Qty (Positive integer greater than 0)!", vbExclamation, Me.Text)
                .Focus()
                .Row = vEditQty
                .Col = 1
                Exit Function
            End If

            If Val(.GetData(vEditRate, 1)) <= 0 Or svtModule.SVTROUND(Val(.GetData(vEditRate, 1)), 4) < 0 Then
                MsgBox("Rate cannot be empty or zero!! Please enter a valid number greater than 0", vbExclamation, Me.Text)
                .Focus()
                .Row = vEditRate
                .Col = 1
                Exit Function
            End If

            If Val(Mid(.GetData(vEditRcptDate, 1), 4, 2)) > 12 Or Year(.GetData(vEditRcptDate, 1)) <= 1900 Or Year(.GetData(vEditRcptDate, 1)) >= 2099 Then
                MsgBox("Invalid date!", vbInformation, Me.Text)
                .Focus()
                .Row = vEditRcptDate
                .Col = 1
                Exit Function
            End If

            intItemID = Val(vsfEdit.GetData(vEditItemId, 1))

            If intItemID <= 0 Then
                MsgBox("Item not selected", vbExclamation, Me.Text)
                .Focus()
                .Row = vEditItemCode
                .Col = 1
                vsfEdit.Focus()
                Exit Function
            End If

            Dim blnFound As Boolean
            clsSVt.getDataStringValue("Select ItemCode from MstOPBalance m, MstItem i, MstLoc l Where m.LocID=l.LocID and m.ItemID=i.ItemID and i.ItemCode='" & .GetData(vEditItemCode, 1) & "' and l.LocCode='" & .GetData(vEditLocCode, 1) & "' and m.RecID <> " & Val(.GetData(vEditRecID, 1)) & "", "", blnFound)
            If blnFound Then
                MsgBox("OP Balance available for the item....", vbExclamation, Me.Text)
                .Focus()
                .Row = vEditItemCode
                .Col = 1
                vsfEdit.Focus()
                Exit Function
            End If

            vsfEdit.EndUpdate()

        End With
        validFlds = True
    End Function

    Private Function SaveDetails() As Boolean
        SaveDetails = False
        Dim strSql As String
        Dim intRecID As Int32
        Dim strErrMsg As String
        Try
            svtTrn = svtConn.BeginTransaction(IsolationLevel.ReadCommitted)

            With vsfEdit

                If Val(.GetData(vEditRecID, 1)) > 0 Then
                    intRecID = Val(.GetData(vEditRecID, 1))

                    strSql = " Insert into TrnStkFlow (DocId,TrnID,TrnDate,LocID,ItemId,DocQty) "
                    strSql &= " Select 0,RecID,getdate(),"
                    strSql &= " LocID, ItemID, -1 * OPQty "
                    strSql &= " from MstOPBalance where "
                    strSql &= " RecID = " & intRecID
                    clsSVt.execSQLStatement(strSql, svtTrn)

                    strSql = "Delete from MstOPBalance where "
                    strSql &= " RecID = " & intRecID
                    clsSVt.execSQLStatement(strSql, svtTrn)
                End If

                strSql = "Insert Into MstOPBalance "
                strSql = strSql & " (FinYear,ItemID,LocID,RcptDate,RcptRef,OpQty,OPRate,Remarks)"
                strSql = strSql & " Values ( "
                strSql = strSql & Val(gSettings.gCompFinYear) & ","
                strSql = strSql & Val(.GetData(vEditItemId, 1)) & ", "
                strSql = strSql & Val(.GetData(vEditLocID, 1)) & ", "
                strSql = strSql & "'" & getDateFormatInInsert(Trim(.GetData(vEditRcptDate, 1))) & "', "
                strSql = strSql & "'" & Trim(.GetData(vEditRcptRef, 1)) & "', "
                strSql = strSql & Val(.GetData(vEditQty, 1)) & ","
                strSql = strSql & Val(.GetData(vEditRate, 1)) & ","
                strSql = strSql & "'" & Trim(.GetData(vEditRmks, 1)) & "'"
                strSql = strSql & ") "
                clsSVt.execSQLStatement(strSql, svtTrn)

                intRecID = clsSVt.getDataLongValue("Select Max(RecID) from MstOpBalance", 0, False, svtTrn)

                strSql = "Insert into TrnStkFlow (DocId,TrnID,TrnDate,LocID,ItemId,DocQty) "
                strSql &= " Select 0, RecID,GetDAte(),"
                strSql &= " LocID,  ItemID, OPQTY "
                strSql &= " from MstOPBalance where "
                strSql &= " RecID = " & intRecID
                clsSVt.execSQLStatement(strSql, svtTrn)

                strSql = " Insert Into TrnQOH (ItemID, LocID,  OpQty, TrnQty) "
                strSql &= " Select s.ItemID, s.LocID, 0, 0 "
                strSql &= " From TrnStkFlow s "
                strSql &= " Left Join  TrnQOH q on (s.ItemID=q.ItemID and s.LocID=q.LOCID ) "
                strSql &= " Where q.ItemID is Null "
                strSql &= " Group by s.ItemID, s.LocID "
                clsSVt.execSQLStatement(strSql, svtTrn)

                strSql = " Update TrnQOH Set TrnQty = (Select isnull(sum(DocQty),0) "
                strSql &= " from TrnStkFlow s "
                strSql &= " where TrnQOH.ItemId=s.ItemID "
                strSql &= " And TrnQOH.LocID=s.LocID) "
                clsSVt.execSQLStatement(strSql, svtTrn)


                strSql = " Update TrnQOH Set CLBal = OPQty + TrnQty"
                strErrMsg = clsSVt.execSQLStmtMsg(strSql, svtTrn)
                If strErrMsg <> "" Then
                    If InStr(UCase(strErrMsg), "CHECK CONSTRAINT") > 0 Then
                        MsgBox("Stock Is going negative. Cannot Save.", MsgBoxStyle.Critical, "No Stock")
                    Else
                        MsgBox(strErrMsg & " Error during updating stock", MsgBoxStyle.Critical, "No Stock")
                    End If
                    svtTrn.Rollback()
                    Exit Function
                End If


                'Call RecordAuditLog("Item Opening Balance", intRecID, UserActions.AddAction, svtTrn) 'Satish

                svtTrn.Commit()

                SaveDetails = True
            End With
        Catch ex As Exception
            SaveDetails = False
            svtTrn.Rollback()
            MsgBox(ex.Message, MsgBoxStyle.Critical, Me.Text)
        End Try
    End Function

    Private Sub vsfEdit_KeyPressEdit(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.KeyPressEditEventArgs) Handles vsfEdit.KeyPressEdit
        If Not (Mode = "M" Or Mode = "A") Then Exit Sub
        If e.KeyChar = "'" Then
            e.Handled = True
            Exit Sub
        End If

        Select Case e.Row
            Case vEditQty, vEditRate

                If (Asc(e.KeyChar) >= 45 And Asc(e.KeyChar) <= 58) Or Asc(e.KeyChar) = 8 Then
                    e.Handled = False
                Else
                    e.Handled = True
                End If
        End Select

    End Sub

    Private Sub vsfEdit_SetupEditor(sender As Object, e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfEdit.SetupEditor
        If Not (Mode = "M" Or Mode = "A") Then Exit Sub
        If e.Row = vEditRcptRef Then
            If TypeOf (vsfEdit.Editor) Is TextBox Then
                Dim tb As TextBox = vsfEdit.Editor
                tb.MaxLength = 50
            End If
        End If
        If e.Row = vEditRmks Then
            If TypeOf (vsfEdit.Editor) Is TextBox Then
                Dim tb As TextBox = vsfEdit.Editor
                tb.MaxLength = 250
            End If
        End If
        If e.Row = vEditQty Or e.Row = vEditRate Then
            If TypeOf (vsfEdit.Editor) Is TextBox Then
                Dim TB As TextBox = vsfEdit.Editor
                TB.MaxLength = 12
            End If
        End If

    End Sub

    Private Sub vsfEdit_ValidateEdit(sender As Object, e As C1.Win.C1FlexGrid.ValidateEditEventArgs) Handles vsfEdit.ValidateEdit
        If Not (Mode = "M" Or Mode = "A") Then Exit Sub
        If e.Row = vEditLocCode And e.Col = 1 Then
            intLocID = clsSVt.getDataLongValue("Select LocID from MstLoc where LocCode = '" & vsfEdit.GetData(vEditLocCode, 1) & "'", 0, False)
            If intLocID > 0 Then
                vsfEdit.SetData(vEditLocID, 1, intLocID)
            End If
        End If

    End Sub

    Private Sub btnClose_Click(sender As System.Object, e As System.EventArgs) Handles btnClose.Click
        Call frmMain.CloseSelectedTabPage(Me)
    End Sub

    Private Sub btnRemoveRow_Click(sender As Object, e As System.EventArgs) Handles btnRemoveRow.Click
        Dim strSql As String

        If Not chkUserRights(strRights, chkRightsPos.DelRights, Me.Text) Then Exit Sub
        If vsfDet.Row < 1 Then
            Exit Sub
        End If

        '# R - 01/10/2007
        If vsfDet.Rows.Count = 2 And Trim(vsfDet.GetData(vsfDet.Row, vDetItemCode)) = "" Then
            MsgBox("No item present to delete!", vbInformation + vbOKOnly, Me.Text)
            Exit Sub
        End If

        If MsgBox("Are you sure you want to delete the item :'" & vsfDet.GetData(vsfDet.Row, vDetItemCode) & "'" & "?",
                vbYesNo + vbQuestion + vbDefaultButton2, Me.Text) = vbYes Then


            svtTrn = svtConn.BeginTransaction(IsolationLevel.ReadCommitted)
            Try
                With vsfDet
                    strSql = " Insert into TrnStkFlow (DocId,TrnID,TrnDate,LocID,ItemId,DocQty) "
                    strSql &= " Select 0,RecID,getdate(),"
                    strSql &= " LocID, ItemID, -1 * OPQty "
                    strSql &= " from MstOPBalance where "
                    strSql &= " RecID = " & Val(.GetData(.Row, vDetRecID))
                    clsSVt.execSQLStatement(strSql, svtTrn)

                    Dim strErrMsg As String
                    strSql = " Insert Into TrnQOH (ItemID, LocID,  OpQty, TrnQty) "
                    strSql &= " Select s.ItemID, s.LocID, 0, 0 "
                    strSql &= " From TrnStkFlow s "
                    strSql &= " Left Join  TrnQOH q on (s.ItemID=q.ItemID and s.LocID=q.LOCID ) "
                    strSql &= " Where q.ItemID is Null "
                    strSql &= " Group by s.ItemID, s.LocID "
                    clsSVt.execSQLStatement(strSql, svtTrn)

                    strSql = " Update TrnQOH Set TrnQty = (Select isnull(sum(DocQty),0) "
                    strSql &= " from TrnStkFlow s "
                    strSql &= " where TrnQOH.ItemId=s.ItemID "
                    strSql &= " And TrnQOH.LocID=s.LocID) "
                    clsSVt.execSQLStatement(strSql, svtTrn)


                    strSql = " Update TrnQOH Set CLBal = OPQty + TrnQty"
                    strErrMsg = clsSVt.execSQLStmtMsg(strSql, svtTrn)
                    If strErrMsg <> "" Then
                        If InStr(UCase(strErrMsg), "CHECK CONSTRAINT") > 0 Then
                            MsgBox("Stock Is going negative. Cannot Save.", MsgBoxStyle.Critical, "No Stock")
                        Else
                            MsgBox(strErrMsg & " Error during updating stock", MsgBoxStyle.Critical, "No Stock")
                        End If
                        svtTrn.Rollback()
                        Exit Sub
                    End If

                    'Call RecordAuditLog("Item Opening Balance", .GetData(.Row, vDetRecID), UserActions.DeleteAction, svtTrn)

                    strSql = "Delete from MstOPBalance where RecID = " & Val(.GetData(.Row, vDetRecID))
                    clsSVt.execSQLStatement(strSql, svtTrn)
                End With


                svtTrn.Commit()
            Catch ex As Exception
                svtTrn.Rollback()
                Exit Sub
            End Try


        End If
        fillDtlGrid()
    End Sub

    Private Sub fillDtlGrid()
        Dim strSql As String

        strSql = "Select o.RecID, i.ItemId, l.LocID, l.LocCode, i.itemcode as ItemCode, i.ItemName, UOMCode as UOM,"
        strSql = strSql & " OPQty, OPRate, (OPQty * OPRate) as OPValue,"
        strSql = strSql & " RcptDate, RcptRef, o.Remarks"
        strSql = strSql & " from MstOPBalance o, mstItem i, MstLoc l, MstUOM u "
        strSql = strSql & " where o.ItemID = i.ItemID"
        strSql = strSql & " and u.UOMID = i.UOMID"
        strSql = strSql & " and finYear = " & Val(gSettings.gCompFinYear)
        strSql = strSql & " and l.LocID = o.LocID"
        strSql = strSql & " order by i.ItemCode"

        Call showWaitCursor()
        dtDetSearch = clsSVt.getDATATable(strSql)
        dvDetSearch = New DataView(dtDetSearch)
        vsfDet.DataSource = dvDetSearch
        Call showNormalCursor()
        SetHideCol(vsfDet, True, vDetItemID, vDetRcptDate, vDetRcptRef, vDetLocID, vDetRecID)
        vsfSearchDet.Cols.Count = vsfDet.Cols.Count
        syncGridCols()

        With vsfEdit
            .Cols.Count = 1
            .Cols.Count = 2
            .Row = vEditLocCode
            .Col = 1
        End With
        Call syncGridCols()
    End Sub

    Private Sub vsfDet_BeforeEdit(sender As Object, e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfDet.BeforeEdit
        e.Cancel = True
    End Sub


    Private Sub vsfDet_EnterCell(sender As Object, e As System.EventArgs) Handles vsfDet.EnterCell
        If (Not (Mode = "A" Or Mode = "M")) Then Exit Sub
    End Sub

    Private Sub vsfDet_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles vsfDet.DoubleClick
        If (Not chkUserRights(strRights, chkRightsPos.EditRights, Me.Text)) Then Exit Sub
        If vsfDet.Rows.Count > 1 Then
            With vsfDet
                Dim i As Integer = .Row
                Call FillEditDtlGrid(.GetData(i, vDetItemID), .GetData(i, vDetLocID))
                btnSave.Visible = True
                btnCancel.Visible = True

                btnAdd.Enabled = True
                btnRemoveRow.Enabled = True
                btnSave.Enabled = True
                EnableFields(True, 2, False, 1)

                'modified by chandan
                For i = 0 To vsfSearchDet.Cols.Count - 1
                    vsfSearchDet.SetData(0, i, "")
                Next

                vsfEdit.Cols(1).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.LeftCenter
            End With
        End If

        If (Not chkUserRights(strRights, chkRightsPos.EditRights, Me.Text)) Then Exit Sub

        If btnSave.Visible = True Then
            Exit Sub
        End If

        If vsfDet.Rows.Count > 1 Then
            With vsfDet
                Dim i As Integer = .Row
                Call FillEditDtlGrid(.GetData(i, vDetItemID), .GetData(i, vDetLocID))
            End With

            btnAdd.Enabled = True
            btnRemoveRow.Enabled = True
            btnCancel.Enabled = True
            btnSave.Enabled = True
        End If

        Mode = "M"
    End Sub

    Private Sub vsfDet_GridChanged(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.GridChangedEventArgs) Handles vsfDet.GridChanged
        txtRowCount.Text = vsfDet.Rows.Count - 1
    End Sub

    Private Sub FillEditDtlGrid(ByVal argItem As Int32, ByVal argCust As Int32)
        Dim strSql As String
        Dim rsDummy As oledb.oledbDataReader

        strSql = "Select o.*, l.LocCode,  i.ItemCode, i.ItemName, UOMCode, (OPQty * OPRate) as OPValue "
        strSql = strSql & " from MstOPBalance o, MstItem i, MstLoc l, MstUOM u "
        strSql = strSql & " where o.ItemID = i.ItemID "
        strSql = strSql & " and FinYear = " & Val(gSettings.gCompFinYear)
        strSql = strSql & " and l.LocID = o.LocID "
        strSql = strSql & " and u.UOMID = i.UOMID "
        strSql = strSql & " and i.ItemID = " & argItem
        strSql = strSql & " and l.LocID = " & argCust
        strSql = strSql & " order by i.ItemCode"

        rsDummy = clsSVt.getDataReaderFromSQL(strSql)

        With rsDummy
            If .Read() Then
                vsfEdit.SetData(vEditRecID, 1, !RecID)
                vsfEdit.SetData(vEditItemId, 1, !ItemID)
                vsfEdit.SetData(vEditItemCode, 1, !ItemCode)
                vsfEdit.SetData(vEditItemName, 1, !ItemName)
                vsfEdit.SetData(vEditLocCode, 1, !LocCode)
                vsfEdit.SetData(vEditLocID, 1, !LocID)
                vsfEdit.SetData(vEditUOM, 1, !UOMCode)
                vsfEdit.SetData(vEditRcptDate, 1, !RcptDate)
                vsfEdit.SetData(vEditRcptRef, 1, !RcptRef)
                vsfEdit.SetData(vEditQty, 1, !OPQty)
                vsfEdit.SetData(vEditRate, 1, !OPRate)
                vsfEdit.SetData(vEditVal, 1, !OPValue)
                vsfEdit.SetData(vEditRmks, 1, !Remarks)
            End If
            .Close()
        End With
    End Sub

    Private Sub btnExportToXL_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExportToXL.Click
        Call ExportToXL(vsfDet)
    End Sub

    Private Sub vsfDet_KeyPressEdit(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.KeyPressEditEventArgs) Handles vsfDet.KeyPressEdit
        If e.KeyChar = "'" Then
            e.Handled = True
            Exit Sub
        End If
    End Sub

    Private Sub ColorChange()
        btnAdd.ForeColor = Color.Black
        btnRemoveRow.ForeColor = Color.Black
        btnExportToXL.ForeColor = Color.Black
        btnClose.ForeColor = Color.Black
        btnSave.ForeColor = Color.Black
        btnCancel.ForeColor = Color.Black
    End Sub

    Private Sub btnCloseSearch_Click(sender As Object, e As System.EventArgs) Handles btnCloseSearch.Click
        fraSearch.Visible = False
    End Sub

    Private Sub vsfView_BeforeEdit(sender As Object, e As RowColEventArgs) Handles vsfView.BeforeEdit
        e.Cancel = True
    End Sub

    Private Sub vsfSearch_KeyPressEdit(sender As Object, e As KeyPressEditEventArgs) Handles vsfSearch.KeyPressEdit
        If e.KeyChar = "'" Then
            e.Handled = True
        End If
    End Sub

    Private Sub frmMstOPBalEntry_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        Call syncGridCols()
    End Sub

    Private Sub vsfEdit_MouseClick(sender As Object, e As MouseEventArgs) Handles vsfEdit.MouseClick
        If e.Button = MouseButtons.Right Then
            If sender.Rows.Fixed >= 1 Then
                sender.Rows.Fixed = 0
                Call SaveActFormVsfDet(Me, Me.Text, sender.Name, sender)
            Else
                For i = 0 To sender.Rows.Count - 1
                    If sender.Rows(i).Visible = True Then
                        sender.Rows.Fixed = i + 1
                        Exit For
                    End If
                Next
            End If
        End If
    End Sub

    Private Sub vsfDet_KeyUp(sender As Object, e As KeyEventArgs) Handles vsfDet.KeyUp

        If e.KeyCode = Keys.Enter Then
            vsfDet_DoubleClick(e, e)
        End If
    End Sub

    Private Sub btnCancel_Leave(sender As Object, e As EventArgs) Handles btnCancel.Leave
        vsfSearchDet.Select()
        With vsfSearchDet
            .Col = 0
            .Row = 0
        End With
    End Sub

    Private Sub vsfSearchDet_AfterEdit(sender As Object, e As RowColEventArgs) Handles vsfSearchDet.AfterEdit
        If vsfDet.Rows.Count > 0 Then
            If vsfSearchDet.Row = vsfSearchDet.Rows.Count - 1 AndAlso vsfSearchDet.Col = vsfSearchDet.Cols.Count - 1 Then
                vsfDet.Select()
                With vsfDet
                    .Col = 0
                    .Row = 1
                End With
            End If
        End If
    End Sub

    Private Sub vsfEdit_KeyDown(sender As Object, e As KeyEventArgs) Handles vsfEdit.KeyDown
        If vsfEdit.Col = vsfEdit.Cols.Count - 1 AndAlso vsfEdit.Row = vsfEdit.Rows.Count - 1 AndAlso btnSave.Visible = False AndAlso btnCancel.Visible = False AndAlso e.KeyCode = Keys.Tab Then
            vsfSearchDet.Select()
            With vsfSearchDet
                .Col = 0
                .Row = 0
            End With
        End If
    End Sub

    Private Sub vsfDet_KeyDown(sender As Object, e As KeyEventArgs) Handles vsfDet.KeyDown
        If vsfDet.Col = vsfDet.Cols.Count - 1 AndAlso vsfDet.Row = vsfDet.Rows.Count - 1 AndAlso e.KeyCode = Keys.Tab Then
            txtRowCount.Focus()
        End If

        If vsfDet.Col = vsfDet.Cols.Count - 1 AndAlso vsfDet.Row = vsfDet.Rows.Count - 1 AndAlso btnSave.Visible = True AndAlso btnCancel.Visible = True AndAlso e.KeyCode = Keys.Tab Then
            vsfEdit.Select()
            With vsfEdit
                .Col = 0
                .Row = 1
            End With
        End If
    End Sub

    Private Sub btnClose_Leave(sender As Object, e As EventArgs) Handles btnClose.Leave

        With vsfEdit
            .Row = 0
            .Col = 1
            .Focus()
        End With
    End Sub

    Private Sub vsfSearchDet_KeyDown(sender As Object, e As KeyEventArgs) Handles vsfSearchDet.KeyDown
        If vsfSearchDet.Col = vsfSearchDet.Cols.Count - 1 AndAlso vsfSearchDet.Row = vsfSearchDet.Rows.Count - 1 AndAlso btnSave.Visible = False AndAlso btnCancel.Visible = False AndAlso e.KeyCode = Keys.Tab Then
            txtRowCount.Focus()
        End If
    End Sub

    Private Sub btnClose_LostFocus(sender As Object, e As EventArgs) Handles btnClose.LostFocus
        With vsfEdit
            .Row = 0
            .Col = 1
            Call showWaitCursor()
        End With

    End Sub

    Private Sub vsfEdit_ChangeEdit(sender As Object, e As EventArgs) Handles vsfEdit.ChangeEdit
        '''''Changed
        With vsfEdit
            Try
                If .Row = vEditRate Then
                    .SetData(vEditRate, 1, .Editor.Text)
                    If Val(.GetData(vEditRate, 1)) <> 0 Then
                        .SetData(vEditVal, 1, svtModule.SVTROUND(Val(.GetData(vEditRate, 1)) * Val(.GetData(vEditQty, 1)), 2))
                    End If
                End If
            Catch ex As Exception

            End Try

        End With
        '''''Changed
    End Sub

    Private Sub vsfEdit_LostFocus(sender As Object, e As EventArgs) Handles vsfEdit.LostFocus
        '''''Changed
        If vsfEdit.Row = vEditItemCode Then
            With vsfEdit
                .Row = vEditRcptRef
                .Col = 1
                .Focus()

            End With
        End If
        '''''Changed
    End Sub

    Private Sub vsfView_LostFocus(sender As Object, e As EventArgs) Handles vsfView.LostFocus
        '''''Changed
        With vsfEdit
            .Row = vEditRcptRef
            .Col = 1
            .Focus()
        End With
        '''''Changed
    End Sub



End Class