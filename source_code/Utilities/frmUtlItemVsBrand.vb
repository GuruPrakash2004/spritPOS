Imports C1.Win.C1FlexGrid

Public Class frmUtlItemVsBrand
    Dim clsSvt As New svtLibrary
    Dim strSQL As String
    Dim Mode As String
    Dim dtiSearch As New DataTable
    Dim dviSearch As New DataView
    Dim intTotRec As Long

    Const VDetSlNo = 0
    Const VDetItemGroup = 1
    Const vDetItemID = 2
    Const VDetItemCode = 3
    Const VDetItemName = 4
    Const VDetUOM = 5
    Const vDetBrandID = 6
    Const vDetBrand = 7
    Const vDetEdited = 8
    Const VDetTotCols = 9

    Private Sub initscreen()
        With vsfDet
            .Rows.Count = 1
            .Cols.Count = VDetTotCols
            .SetData(0, VDetSlNo, "SlNo")
            .SetData(0, VDetItemGroup, "ItemGroup")
            .SetData(0, VDetItemCode, "ItemCode")
            .SetData(0, VDetItemName, "ItemName")
            .SetData(0, VDetUOM, "Uom")
            .SetData(0, vDetBrand, "Under Brand")
            SetFlexColor(vsfDet, "M", vDetBrand)
            SetFlexColor(vsfDet, "", VDetSlNo, VDetItemGroup, VDetItemCode, VDetItemName, VDetUOM)
            SetHideCol(vsfDet, True, vDetItemID, vDetBrandID, vDetEdited)
        End With
    End Sub

    Private Sub frmRptPendingPO_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.KeyPreview = True
        fillFormProperties(Me)
        initGridStyles(Me)
        initscreen()
        initFormVsfDet(Me, Me.Text, vsfDet, "DET")
    End Sub

    Private Sub btnLoad_Click(sender As Object, e As EventArgs) Handles btnLoad.Click
        showWaitCursor()
        FillData()
        showNormalCursor()

    End Sub

    Private Sub FillData()
        Dim rs As oledb.oledbDataReader
        Dim strSQL As String
        Dim i As Int32

        strSQL = "Select ig.ItemGroupID,ItemGroupCode,i.ItemID,ItemCode,ItemName,UomCode"
        If optItemLinked.Checked Then strSQL &= " ,b.BrandID,b.BrandCode"
        strSQL &= " From MstItem i, MstItemGroup ig, MstUOM u"
        If optItemLinked.Checked Then strSQL &= ",MstBrand b"
        strSQL &= " Where i.ItemID>0 "
        strSQL &= " and i.ItemGroupID=ig.ItemGroupID"
        strSQL &= " and i.UOMID=u.UOMID"
        If optItemUnlinked.Checked Then
            strSQL &= " and i.BrandID=0"
        Else
            strSQL &= " and i.BrandID=b.BrandID and b.BrandID>0"
        End If
        strSQL &= " Order by ItemGroupCode"

        rs = clsSvt.getDataReaderFromSQL(strSQL)

        With vsfDet
            .Rows.Count = 1
            If rs.HasRows Then
                Do While rs.Read
                    .Rows.Count = .Rows.Count + 1
                    i = .Rows.Count - 1
                    .SetData(i, VDetSlNo, i)
                    .SetData(i, VDetItemGroup, GetVal(rs("ItemGroupCode"), ""))
                    .SetData(i, vDetItemID, GetVal(rs("ItemID"), 0))
                    .SetData(i, VDetItemCode, GetVal(rs("ItemCode"), ""))
                    .SetData(i, VDetItemName, GetVal(rs("ItemName"), ""))
                    .SetData(i, VDetUOM, GetVal(rs("UOMCode"), ""))
                    If optItemLinked.Checked Then .SetData(i, vDetBrand, GetVal(rs("BrandCode"), ""))
                Loop
                rs.Close()
                txtItemSearch.Visible = True
                lblSer.Visible = True
                txtItemSearch.Text = ""
            Else
                MsgBox("No Records Found!", MsgBoxStyle.Information, Me.Text)
                .Rows.Count = 1
                txtItemSearch.Visible = False
                lblSer.Visible = False
                txtItemSearch.Text = ""
            End If
        End With

        dtiSearch = clsSvt.getDATATable(strSQL)
        dviSearch = New DataView(dtiSearch)

        vsfItemSearch.DataSource = dviSearch

        For x As Int32 = 0 To vsfItemSearch.Cols.Count - 1
            vsfItemSearch.Cols(x).Visible = False
        Next
        SetHideCol(vsfItemSearch, False, 3, 4)


        intTotRec = Val(vsfDet.Rows.Count) - 1
        lblTotRec.Text = "Total Records: " & Val(Val(vsfDet.Rows.Count) - 1) & "/" & intTotRec
        vsfDet.Rows(0).TextAlignFixed = TextAlignEnum.LeftCenter
        'vsfDet.Rows(1).TextAlignFixed = TextAlignEnum.LeftCenter
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Dim strSQL As String
        Dim i As Int32

        For i = 1 To vsfDet.Rows.Count - 1
            'If Trim(vsfDet.GetData(i, vDetEdited)) = "Y" Then
            strSQL = "Update MstItem Set BrandID=" & clsSvt.getDataLongValue("Select BrandID From MstBrand Where BrandCode='" & Trim(vsfDet.GetData(i, vDetBrand)) & "'", 0, False)
                strSQL &= " Where  ItemID=" & vsfDet.GetData(i, vDetItemID)
                If Not clsSvt.execSQLStatement(strSQL) Then
                    Exit Sub
                End If
            ' End If
        Next
        MsgBox("Successfully Saved!", MsgBoxStyle.Information, Me.Text)
        btnLoad.PerformClick()

    End Sub

    Private Sub frmRptPendingPO_KeyUp(sender As Object, e As KeyEventArgs) Handles Me.KeyUp

        If e.Alt AndAlso e.KeyCode = Keys.S Then
            btnSave.PerformClick()
            Exit Sub
        End If

        If e.Alt AndAlso e.KeyCode = Keys.C Then
            btnClose.PerformClick()
            Exit Sub
        End If

        If e.Alt AndAlso e.KeyCode = Keys.L Then
            btnLoad.PerformClick()
            Exit Sub
        End If

        If e.Alt AndAlso e.KeyCode = Keys.R Then
            btnReplace.PerformClick()
            Exit Sub
        End If

        If e.KeyCode = Keys.Escape Then
            vsfItemSearch.Visible = False
            txtItemSearch.Text = ""
            Exit Sub
        End If

        If e.KeyCode = Keys.F9 And e.Control = True Then
            Call initFormVsfDet(Me, Me.Text, vsfDet, "DET")
            MsgBox("Restored Form & Grid Layout Successfully!!", vbInformation, Me.Text)
            Exit Sub
        End If

        If e.KeyCode = Keys.F8 And e.Control = True Then
            Call SaveActFormSettings(Me, Me.Text)
            Call SaveActFormVsfDet(Me, Me.Text, "DET", vsfDet)
            MsgBox("Saved Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
    End Sub

    Private Sub vsfDet_BeforeEdit(sender As Object, e As RowColEventArgs) Handles vsfDet.BeforeEdit

        Select Case e.Col
            Case VDetSlNo, VDetItemGroup, VDetItemCode, VDetItemName, VDetUOM
                e.Cancel = True
            Case vDetBrand
                vsfDet.ComboList = GenVSfComboStringCond("BrandCode", "MstBrand", "Order By 1")
        End Select

    End Sub

    Private Sub btnExportToXL_Click(sender As Object, e As EventArgs)
        ExportToXL(vsfDet, Me.Text)
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        frmMain.CloseSelectedTabPage(Me)
    End Sub

    Private Sub vsfDet_AfterEdit(sender As Object, e As RowColEventArgs) Handles vsfDet.AfterEdit
        vsfDet.SetData(vsfDet.Row, vDetEdited, "Y")

    End Sub

    Private Sub txtItemSearch_TextChanged(sender As Object, e As EventArgs) Handles txtItemSearch.TextChanged
        If Trim(txtItemSearch.Text) = String.Empty Then
            vsfItemSearch.SendToBack()
            vsfItemSearch.Visible = False
        Else
            vsfItemSearch.BringToFront()
            vsfItemSearch.Visible = True
        End If
        applyFilter()
    End Sub

    Private Function applyFilter() As Boolean
        Dim StrQry As String

        applyFilter = False
        StrQry = ""
        With vsfItemSearch
            Try
                If txtItemSearch.Text = " " Then
                    dviSearch.RowFilter = "1 = 1"
                    Exit Function
                Else
                    StrQry &= " ItemCode like '%" & Trim(txtItemSearch.Text) & "%' Or "
                    StrQry &= " ItemName like '%" & Trim(txtItemSearch.Text) & "%'"
                End If
                dviSearch.RowFilter = StrQry
            Catch ex As Exception

            End Try
            intTotRec = Val(vsfDet.Rows.Count) - 1
            lblTotRec.Text = "Total Records: " & Val(Val(vsfDet.Rows.Count) - 1) & "/" & intTotRec
            vsfDet.Rows(0).TextAlignFixed = TextAlignEnum.LeftCenter
            vsfDet.Rows(1).TextAlignFixed = TextAlignEnum.LeftCenter
        End With
        applyFilter = True
    End Function

    Private Sub txtItemSearch_KeyUp(sender As Object, e As KeyEventArgs) Handles txtItemSearch.KeyUp
        If e.KeyCode = Keys.Down Then
            vsfItemSearch.Focus()
            Exit Sub
        End If

        If e.KeyCode = Keys.Enter Then
            If txtItemSearch.Text = String.Empty Then Exit Sub
            SetFocus()
        End If
    End Sub

    Private Sub vsfItemSearch_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtItemSearch.KeyPress
        If e.KeyChar = "'" Then
            e.Handled = True
        End If
    End Sub

    Private Sub vsfItemSearch_KeyUp(sender As Object, e As KeyEventArgs) Handles vsfItemSearch.KeyUp
        If e.KeyCode = Keys.Enter Then
            If Trim(txtItemSearch.Text) = "" Then Exit Sub
            If vsfItemSearch.Row < 0 Then Exit Sub
            SetFocus()
            txtItemSearch.Text = ""
        ElseIf e.KeyCode = Keys.Escape Then
            txtItemSearch.Text = ""
            vsfItemSearch.Visible = False
        End If
    End Sub

    Private Sub vsfItemSearch_BeforeEdit(sender As Object, e As RowColEventArgs) Handles vsfItemSearch.BeforeEdit
        e.Cancel = True

    End Sub

    Private Sub vsfItemSearch_DoubleClick(sender As Object, e As EventArgs) Handles vsfItemSearch.DoubleClick
        SetFocus()
    End Sub

    Private Sub SetFocus()
        If vsfItemSearch.Rows.Count < 1 Then Exit Sub
        If vsfItemSearch.Row < 0 Then Exit Sub
        Dim intItemID As Int32 = Val(vsfItemSearch.GetData(vsfItemSearch.Row, vDetItemID))
        vsfItemSearch.Visible = False
        With vsfDet
            For i As Int32 = 1 To .Rows.Count - 1
                If Val(.GetData(i, vDetItemID)) = intItemID Then
                    .Col = VDetItemCode
                    .Row = i
                    .Focus()
                    txtItemSearch.Text = ""
                    Exit Sub
                End If
            Next
        End With
        txtItemSearch.Text = ""

        intTotRec = Val(vsfDet.Rows.Count) - 1
        lblTotRec.Text = "Total Records: " & Val(Val(vsfDet.Rows.Count) - 1) & "/" & intTotRec
        vsfDet.Rows(0).TextAlignFixed = TextAlignEnum.LeftCenter
        vsfDet.Rows(1).TextAlignFixed = TextAlignEnum.LeftCenter
    End Sub

    Private Sub btnReplace_Click(sender As Object, e As EventArgs) Handles btnReplace.Click
        With vsfDet
            For i As Int32 = vsfDet.Row To .Rows.Count - 1
                If .Rows(i).Visible Then
                    .SetData(i, vDetBrand, .GetData(vsfDet.Row, vDetBrand))
                End If
            Next
        End With
    End Sub

    Private Sub vsfDet_AfterFilter(sender As Object, e As EventArgs) Handles vsfDet.AfterFilter
        Dim intRowCount As Int32 = 0

        For i As Int32 = 1 To vsfDet.Rows.Count - 1
            If vsfDet.Rows(i).Visible Then
                intRowCount = intRowCount + 1
            End If
        Next

        lblTotRec.Text = "Total Records: " & intRowCount
    End Sub

    Private Sub vsfDet_KeyPress(sender As Object, e As KeyPressEventArgs) Handles vsfDet.KeyPress


    End Sub

    Private Sub vsfDet_KeyPressEdit(sender As Object, e As KeyPressEditEventArgs) Handles vsfDet.KeyPressEdit
        'Adedd by surabhi
        If sender.col = vDetBrand Then
            e.Handled = True
        End If
    End Sub
End Class