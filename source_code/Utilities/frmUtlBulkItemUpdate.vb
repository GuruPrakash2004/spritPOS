Imports C1.Win.C1FlexGrid

Public Class frmUtlBulkItemUpdate
    Dim intPresent As Integer
    Dim arr(3) As Double
    Dim clsSVT As New svtLibrary
    Public strList As String = ""
    Dim strItemList As String = ""
    Dim strItemGroupList As String = ""
    Dim dv As DataView
    Dim dt As DataTable
    Dim intTotRec As Integer
    Dim isFilterApplied As Boolean = False

    Dim intScrollBarWidth As Integer
    Dim gComputerName As String

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.Close()
    End Sub

    Private Sub btnClrGridFilters_Click(sender As System.Object, e As System.EventArgs) Handles btnClrGridFilters.Click
        With vsfSearch
            .Row = 0
            .Col = 0
            .Clear(C1.Win.C1FlexGrid.ClearFlags.All)
        End With
        If Not dv Is Nothing Then
            btnLoad_Click(Me, Nothing)
        End If
    End Sub

    Private Sub btnLoad_Click(sender As System.Object, e As System.EventArgs) Handles btnLoad.Click
        initVsfView()
    End Sub

    Private Sub initVsfView()
        Dim strSql As String = ""

        strSql = " SELECT * "
        strSql &= " FROM MstItem "
        strSql &= " WHERE 1 = 1 "
        strSql &= strCond()
        strSql &= " And ItemID > 0 "
        strSql &= " And Inactive = 'N'"
        strSql &= " ORDER BY ItemName "

        Cursor.Current = Cursors.WaitCursor
        dt = clsSVT.getDATATable(strSql)
        dv = New DataView(dt)

        vsfDet.Rows.Fixed = 1
        vsfDet.BeginUpdate()

        vsfDet.DataSource = dv
        Cursor.Current = Cursors.Arrow

        vsfDet.EndUpdate()
        vsfSearch.Cols.Count = vsfDet.Cols.Count


        intTotRec = vsfDet.Rows.Count - 1
        intScrollBarWidth = SystemInformation.VerticalScrollBarWidth
        vsfSearch.Cols(vsfSearch.Cols.Count - 1).Width = vsfDet.Cols(vsfDet.Cols.Count - 1).Width + intScrollBarWidth
        vsfDet.Rows(0).TextAlignFixed = TextAlignEnum.LeftCenter
        lblCount.Text = "No. of items: " & Val(Val(vsfDet.Rows.Count) - 1) & "/" & intTotRec
        Call syncGridCols()
    End Sub

    Private Function strCond() As String
        strCond = ""

        If chkSelItem.Checked Then
            strCond &= " AND ItemID in (" & Trim(strItemList) & ")"
        End If

        If chkSelIG.Checked Then
            strCond &= " AND ItemGroupID in (" & Trim(strItemGroupList) & ")"
        End If

    End Function

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
                    Select Case gDBTypeCurrent
                        Case gDbTypeAccess
                            StrQry = StrQry & vsfDet.GetData(0, i) & " like '*" & strValue & "*'"
                        Case Else
                            'If vsfDet.Cols(i).DataType = GetType(Decimal) Or vsfDet.Cols(i).DataType = GetType(Integer) Then
                            '    Try
                            '        Dim strTest = Convert.ToDecimal(strValue)
                            '        StrQry = StrQry & vsfDet.GetData(0, i) & " >= " & strValue
                            '    Catch ex As Exception
                            '        ' vsfSearch.SetData(0, i, "")
                            '        dv.RowFilter = "1 <> 1"
                            '        Exit Function
                            '    End Try

                            'Else
                            '    StrQry = StrQry & vsfDet.GetData(0, i) & " like '%" & strValue & "%'"
                            'End If
                            If vsfDet.Cols(i).DataType = GetType(Decimal) Or vsfDet.Cols(i).DataType = GetType(Integer) Then
                                Try
                                    Dim strTest = Convert.ToDecimal(strValue)
                                    StrQry = StrQry & vsfDet.GetData(0, i) & " >= " & strValue
                                Catch ex As Exception
                                    ' vsfSearch.SetData(0, i, "")
                                    dv.RowFilter = "1 <> 1"
                                    Exit Function
                                End Try
                            ElseIf vsfDet.Cols(i).DataType = GetType(DateTime) Then
                                StrQry = StrQry & "CONVERT (" & vsfDet.GetData(0, i) & ", System.String) LIKE '%" & strValue & "%'"
                            Else
                                StrQry = StrQry & vsfDet.GetData(0, i) & " like '%" & strValue & "%'"
                            End If
                    End Select
                End If
            Next i
        End With
        'If StrQry <> "" Then
        '    vsfDet.DataSource = dt.Select(StrQry)
        'Else
        '    vsfDet.DataSource = dt
        'End If
        Dim k As Integer = vsfDet.Rows.Count
        dv.RowFilter = StrQry
        lblCount.Text = "No. of items: " & Val(Val(vsfDet.Rows.Count) - 1) & "/" & intTotRec
        Call syncGridCols()
        applyFilterFromGrid = True
    End Function

    Private Sub syncGridCols()
        Dim i As Integer
        With vsfDet
            vsfSearch.Cols.Count = vsfDet.Cols.Count
            vsfDet.Cols(0).Visible = False
            vsfDet.AutoSizeCols()
            For i = 0 To .Cols.Count - 1
                vsfSearch.Cols(i).Width = vsfDet.Cols(i).Width
                vsfSearch.Cols(i).Visible = vsfDet.Cols(i).Visible
            Next i
        End With
    End Sub

    Private Sub vsfDet_AfterScroll(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.RangeEventArgs) Handles vsfDet.AfterScroll
        'vsfSearch.LeftCol = vsfDet.LeftCol
        vsfSearch.ScrollPosition = vsfDet.ScrollPosition
    End Sub

    Private Sub vsfSearch_AfterScroll(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.RangeEventArgs) Handles vsfSearch.AfterScroll
        vsfSearch.ScrollPosition = vsfDet.ScrollPosition
    End Sub

    Private Sub vsfSearch_ChangeEdit(ByVal sender As Object, ByVal e As System.EventArgs) Handles vsfSearch.ChangeEdit
        Call applyFilterFromGrid()
    End Sub

    Private Sub vsfDet_AfterResizeColumn(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfDet.AfterResizeColumn
        Call syncGridCols()
    End Sub

    Private Sub vsfDet_BeforeEdit(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfDet.BeforeEdit
        e.Cancel = True
    End Sub

    Private Sub vsfSearch_EnterCell(sender As Object, e As System.EventArgs) Handles vsfSearch.EnterCell
        With vsfSearch
            .StartEditing()
        End With
    End Sub

    Private Sub frmUtlBulkItemUpdate_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        initScreen()
        syncGridCols()
    End Sub

    Private Sub initScreen()
        vsfDet.AllowEditing = False
    End Sub

    Private Sub chkSelItem_CheckedChanged(sender As Object, e As System.EventArgs) Handles chkSelItem.CheckedChanged
        If chkSelItem.Checked Then
            Dim strSQL As String
            Dim frmSel As New frmUtlMultiSelection
            strSQL = "Select distinct 0 as Sel, ItemID, ShortName from MstItem where ItemID > 0 and ItemCode <> '-' and Inactive = 'N'"
            strList = ""
            Call frmSel.InitVsfSel(strSQL, 1, 1, False, Me, False)
            frmSel.Text = "Select [Multiple] Items"
            frmSel.StartPosition = FormStartPosition.CenterScreen
            frmSel.ShowDialog()
            strItemList = strList
        End If
    End Sub

    Private Sub chkSelIG_CheckedChanged(sender As Object, e As System.EventArgs) Handles chkSelIG.CheckedChanged
        If chkSelIG.Checked Then
            Dim strSQL As String
            Dim frmSel As New frmUtlMultiSelection
            strSQL = "Select distinct 0 as Sel, ItemGroupID, ItemGroupCode from MstItemGroup where ItemGroupID > 0 and ItemGroupCode <> '-' and Inactive = 'N'"
            strList = ""
            Call frmSel.InitVsfSel(strSQL, 1, 1, False, Me, False)
            frmSel.Text = "Select [Multiple] Item Groups"
            frmSel.StartPosition = FormStartPosition.CenterScreen
            frmSel.ShowDialog()
            strItemGroupList = strList
        End If
    End Sub

    Private Sub vsfDet_GridChanged(sender As Object, e As C1.Win.C1FlexGrid.GridChangedEventArgs) Handles vsfDet.GridChanged
        If vsfDet.Rows.Count > 1 Then
            lblSelected.Visible = True
            lblSelected.Text = "For the " & Val(Val(vsfDet.Rows.Count) - 1) & " selected item(s)..."
        Else
            lblSelected.Visible = False
        End If
    End Sub

    Private Sub txtReplFrom_Enter(sender As Object, e As System.EventArgs) Handles txtReplFrom.Enter
        Label1.Focus()
    End Sub

    Private Sub chkCondition_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkCondition.CheckedChanged
        If chkCondition.Checked Then
            txtCondition.Enabled = True
            txtCondition.Focus()
        Else
            txtCondition.Text = ""
            txtCondition.Enabled = False
        End If
    End Sub

    Private Sub vsfDet_MouseClick(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles vsfDet.MouseClick
        Dim ht = vsfDet.HitTest(e.X, e.Y)

        If vsfDet.Rows.Count > 1 Then
            If ht.Column <> -1 Then
                txtReplFrom.Text = vsfDet.GetData(0, ht.Column)
                txtReplFrom.Tag = vsfDet.Cols(ht.Column).DataType.ToString
                txtReplTo.Text = ""
                txtReplTo.MaxLength = intColLength(vsfDet.GetData(0, ht.Column))
                If txtReplFrom.Tag = "System.DateTime" Then

                End If
            End If
        End If
    End Sub

    'Private Sub setTextboxType(argType As String)
    '    Select Case argType
    '        Case "System.String"
    '            txtReplTo.Tag = GetType(System.String)
    '        Case "System.Int32"
    '            txtrepltest.ValidatingType = GetType(System.Int32)
    '        Case "System.Decimal"
    '            txtrepltest.Mask = count
    '            txtrepltest.ValidatingType = GetType(System.Decimal)
    '        Case "System.DateTime"
    '            txtrepltest.Mask = "00/00/0000"
    '            txtrepltest.ValidatingType = GetType(System.DateTime)
    '    End Select
    'End Sub

    Private Sub txtReplTo_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles txtReplTo.KeyPress
        If txtReplFrom.Tag = "System.Int32" Or txtReplFrom.Tag = "System.Decimal" Then
            'If Asc(e.KeyChar) <> 8 Then
            '    If Asc(e.KeyChar) < 48 Or Asc(e.KeyChar) > 57 Then
            '        e.Handled = True
            '    End If
            'End If
            If e.KeyChar = "."c Then
                e.Handled = (CType(sender, TextBox).Text.IndexOf("."c) <> -1)
            ElseIf e.KeyChar <> ControlChars.Back Then
                e.Handled = ("0123456789".IndexOf(e.KeyChar) = -1)
            End If
        End If
    End Sub

    Private Sub txtReplTo_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles txtReplTo.Validating
        If txtReplFrom.Tag.ToString.Contains("Int32") Or txtReplFrom.Tag.ToString.Contains("Decimal") Then
            Dim control As TextBox = CType(sender, TextBox)
            Dim result As Decimal = 0
            Decimal.TryParse(control.Text, result)
            control.Text = result.ToString()
        End If
    End Sub

    Private Sub btnClear_Click(sender As System.Object, e As System.EventArgs) Handles btnClear.Click
        txtReplFrom.Text = ""
        txtReplTo.Text = ""
        chkCondition.Checked = False
        txtCondition.Text = ""
    End Sub

    Private Sub btnUpdate_Click(sender As System.Object, e As System.EventArgs) Handles btnUpdate.Click
        Dim strSql As String = ""
        Dim strMsg As String = ""

        strMsg = "Item Master field '" & Trim(txtReplFrom.Text) & "' will be updated with '" & Trim(txtReplTo.Text) & "'"
        If chkCondition.Checked AndAlso Trim(txtCondition.Text) <> "" Then
            strMsg &= " for the given condition"
        End If
        strMsg &= "!! Are you sure you want to continue!?!?!"

        If Not isValid() Then Exit Sub

        'If MsgBox("You will be making changes to the database that cannot be reverted!! Are you sure you want to continue!?!?", _
        '          vbCritical + vbYesNo + MsgBoxStyle.DefaultButton2, Me.Text) = MsgBoxResult.No Then Exit Sub

        If MsgBox(strMsg, vbCritical + vbYesNo + MsgBoxStyle.DefaultButton2, Me.Text) = MsgBoxResult.No Then Exit Sub

        If Not isFilterApplied Then
            If MsgBox("You have not used any filters!! Are you sure you want to continue?!?", _
                  vbCritical + vbYesNo + MsgBoxStyle.DefaultButton2, Me.Text) = MsgBoxResult.No Then Exit Sub
        End If

        Try
            For i = 1 To vsfDet.Rows.Count - 1
                strSql = "UPDATE MstItem set "
                strSql &= Trim(txtReplFrom.Text) & " = "

                If txtReplFrom.Tag.ToString.Contains("Int32") Or txtReplFrom.Tag.ToString.Contains("Decimal") Then
                    strSql &= Val(txtReplTo.Text)
                ElseIf txtReplFrom.Tag.ToString.Contains("String") Then
                    strSql &= "'" & Trim(txtReplTo.Text) & "'"
                End If

                strSql &= " WHERE 1 = 1"
                If chkCondition.Checked Then
                    If Trim(txtCondition.Text) <> "" Then
                        strSql &= " AND " & Trim(txtCondition.Text)
                    End If
                End If
                strSql &= " AND ItemID = " & vsfDet.GetData(i, 0)

                clsSVT.execSQLStatement(strSql)
            Next

            MsgBox("Successfully updated item master table!", vbInformation, Me.Text)
            btnClear_Click(Me, Nothing)
            btnClrGridFilters_Click(Me, Nothing)
            btnLoad_Click(Me, Nothing)
        Catch ex As Exception
            MsgBox("Error while updating! Please check if credentials entered are valid!" & vbCrLf & vbCrLf & "Error: " & ex.Message, vbCritical, Me.Text)
            Exit Sub
        End Try
    End Sub

    Private Function isValid() As Boolean
        isValid = False
        Dim blnfound As Boolean = False

        If vsfDet.Rows.Count <= 1 Then
            MsgBox("Please load data to the grid first before updating!", vbInformation, Me.Text)
            btnLoad.Focus()
            Exit Function
        End If

        If Trim(txtReplFrom.Text) = "" Then
            MsgBox("Please select a column for which data needs to be updated!", vbInformation, Me.Text)
            vsfDet.Focus()
            Exit Function
        End If


        If Trim(txtReplTo.Text) = "" Then
            MsgBox("Please enter any value to update!", vbInformation, Me.Text)
            txtReplTo.Focus()
            Exit Function
        End If


        If UCase(Trim(txtReplFrom.Text)) = "BRANDID" Then
            clsSVT.getDataLongValue("SELECT BrandID from MstBrand where BrandID = " & Val(txtReplTo.Text), 0, blnfound)
            If Not blnfound Then
                MsgBox("Invalid Brand ID! Please enter a valid Brand ID that exists in brand master table!", vbCritical, Me.Text)
                txtReplTo.Focus()
                Exit Function
            End If
        End If

        If UCase(Trim(txtReplFrom.Text)) = "ITEMGROUPID" Then
            clsSVT.getDataLongValue("SELECT ItemGroupID from MstItemGroup where ItemGroupID = " & Val(txtReplTo.Text), 0, blnfound)
            If Not blnfound Then
                MsgBox("Invalid ItemGroup ID! Please enter a valid ItemGroup ID that exists in item group master table!", vbCritical, Me.Text)
                txtReplTo.Focus()
                Exit Function
            End If
        End If

        If UCase(Trim(txtReplFrom.Text)) = "UOMID" Then
            clsSVT.getDataLongValue("SELECT UOMID from MstUOM where UOMID = " & Val(txtReplTo.Text), 0, blnfound)
            If Not blnfound Then
                MsgBox("Invalid UOM ID! Please enter a valid UOM ID that exists in UOM master table!", vbCritical, Me.Text)
                txtReplTo.Focus()
                Exit Function
            End If
        End If


        If UCase(Trim(txtReplFrom.Text)) = "PKGID" Then
            clsSVT.getDataLongValue("SELECT PkgID from MstPkg where PkgID = " & Val(txtReplTo.Text), 0, blnfound)
            If Not blnfound Then
                MsgBox("Invalid package ID! Please enter a valid package ID that exists in package master table!", vbCritical, Me.Text)
                txtReplTo.Focus()
                Exit Function
            End If
        End If

        If chkCondition.Checked Or chkSelItem.Checked Or chkSelIG.Checked Then
            isFilterApplied = True
        Else
            isFilterApplied = False
        End If

        isValid = True
    End Function

    Private Sub btnCancel_Click(sender As System.Object, e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub txtReplFrom_TextChanged(sender As Object, e As System.EventArgs) Handles txtReplFrom.TextChanged, txtReplTo.TextChanged
        If Trim(txtReplFrom.Text) <> "" AndAlso Trim(txtReplTo.Text) <> "" Then
            btnUpdate.Enabled = True
        Else
            btnUpdate.Enabled = False
        End If
    End Sub

    Private Function intColLength(argColName As String) As Long
        Dim strSql As String = ""

        strSql = "SELECT syscolumns.LENGTH from sysobjects INNER JOIN syscolumns ON sysobjects.id = syscolumns.id "
        strSql &= " INNER JOIN systypes ON syscolumns.xtype = systypes.xtype "
        strSql &= " WHERE (sysobjects.xtype = 'U') AND sysobjects.name = 'MstItem' "
        strSql &= " AND syscolumns.name = '" & Trim(argColName) & "'"

        intColLength = clsSVT.getDataLongValue(strSql, 0, False)

    End Function
End Class