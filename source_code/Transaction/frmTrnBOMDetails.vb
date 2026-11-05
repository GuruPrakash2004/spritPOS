Imports C1.Win.C1FlexGrid

Public Class frmTrnBOMDetails
    Dim clsSVT As New svtLibrary
    Dim strSQL As String = ""

    Dim lngItemID As Long

    Const vDetSlNo = 0
    Const vDetItemID = 1
    Const vDetItemCode = 2
    Const vDetItemName = 3
    Const vDetUOM = 4
    Const vDetUnitQty = 5
    Const vDetQty = 6
    Const vTot = 7

    Public argForm As Object
    Public intColQty As Int32
    Public intColItemCode As Integer

    Dim dt As DataTable
    Dim dv As DataView

    Private Sub frmTrnBOMDetails_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        initGridStyles(Me)
        Me.KeyPreview = True
        Call initScreen()
        Call initFormVsfDet(Me, Me.Text, vsfDet, "DET")
        Call ChangeFontSize(Me)

    End Sub

    Private Sub fillDtls()
        Dim strSQL As String = ""
        Dim rs As oledb.oledbDataReader

        strSQL = "Select SlNo,ItemCode, ItemName, UOMCode, Qty"
        strSQL &= " from MstITEM i, MstHdrBOM h, MstDtlBOM d, MstUOM u"
        strSQL &= " where h.BOMID = d.BOMID "
        strSQL &= " and i.ItemID = d.ItemID"
        strSQL &= " and i.UOMID = u.UOMID "
        strSQL &= " And h.Bomitemid = " & lngItemID
        strSQL &= "AND  LatestBOm='Y' and YEAR(Effto)=9998 order by SlNo"

        Try
            rs = clsSVT.getDataReaderFromSQL(strSQL)

            vsfDet.Rows.Count = 1

            If Not rs.HasRows Then
                rs.Close()
                Exit Sub
            End If

            With vsfDet
                Do While rs.Read
                    .Rows.Count = .Rows.Count + 1
                    .SetData(.Rows.Count - 1, vDetSlNo, GetVal(rs!SlNo, 0))
                    .SetData(.Rows.Count - 1, vDetItemCode, GetVal(rs!ItemCode, ""))
                    .SetData(.Rows.Count - 1, vDetItemName, GetVal(rs!ItemName, ""))
                    .SetData(.Rows.Count - 1, vDetUOM, GetVal(rs!UOMCode, ""))
                    '.SetData(.Rows.Count - 1, vDetUnitQty, GetVal(rs!Qty, 0))
                    If txtBOMQty.Text <> "" Then
                        .SetData(.Rows.Count - 1, vDetQty, GetVal(rs!Qty, 0) * Val(txtBOMQty.Text))
                    Else
                        .SetData(.Rows.Count - 1, vDetQty, GetVal(rs!Qty, 0))
                    End If
                Loop
                rs.Close()
            End With

        Catch ex As Exception
            MsgBox(ex.Message & "[fillDtls()]", vbInformation, Me.Text)
        End Try
    End Sub


    Private Sub initScreen()
        With vsfDet
            .Rows.Count = 1
            .Cols.Count = vTot

            .SetData(0, vDetSlNo, "SlNo")
            .SetData(0, vDetItemID, "Item ID")
            .SetData(0, vDetItemCode, "Item Code")
            .SetData(0, vDetItemName, "Item Name")
            .SetData(0, vDetUOM, "UOM")
            .SetData(0, vDetUnitQty, "Unit Qty")
            .SetData(0, vDetQty, "QTY")
        End With

        Call SetHideCol(vsfDet, True, vDetItemID, vDetUnitQty)
        Call ReArrangeSlNo(vsfDet, vDetSlNo)
    End Sub

    Private Sub vsfDet_BeforeEdit(sender As Object, e As RowColEventArgs) Handles vsfDet.BeforeEdit
        e.Cancel = True
    End Sub

    Private Sub frmTrnBOMDetails_KeyUp(sender As Object, e As KeyEventArgs) Handles Me.KeyUp
        If e.KeyCode = Keys.F9 And e.Control = True Then
            Call initFormVsfDet(Me, Me.Text, vsfDet, "DET")
            MsgBox("Restored Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
        If e.KeyCode = Keys.F8 And e.Control = True Then
            Call SaveActFormVsfDet(Me, Me.Text, "DET", vsfDet)
            MsgBox("Saved Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If

        'If e.Alt = True And e.KeyCode = Keys.E Then
        '    If vsfDet.Rows.Count > 1 Then Call ExportToXL(vsfDet)
        'End If
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        txtBOMDetails.Text = ""
        txtBOMQty.Text = ""
        txtBOMDetails.Focus()
        vsfDet.Rows.Count = 1
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub txtBOMDetails_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtBOMDetails.KeyPress
        If e.KeyChar = "`" Then
            e.Handled = True
        End If
    End Sub

    Private Sub vsfSearchSelectedItem()
        Dim strsQL As String
        If vsfsearch.Rows.Count > 1 Then
            strsQL = "Select ItemID from MstItem where ItemCode ='" & vsfsearch.GetData(vsfsearch.Row, 0) & "'"

            txtID.Text = clsSVT.getDataLongValue(strsQL, 0, False)
            txtBOMDetails.Text = clsSVT.getDataStringValue("Select ItemCode from MstItem where ItemID=" & Trim(txtID.Text), 0, False)
            'lblI.Text = clsSVT.getDataStringValue("Select ItemName from MstItem where ItemID =" & Trim(txtID.Text), 0, False)
            Call setSearchGridHt("")
        End If
    End Sub

    Private Sub setSearchGridHt(ByVal argFlag As String)
        If argFlag = "FULL" Then
            vsfsearch.Visible = True
            vsfsearch.BringToFront()
        Else
            vsfsearch.Visible = False
        End If

    End Sub

    Private Sub vsfsearch_BeforeEdit(sender As Object, e As RowColEventArgs) Handles vsfsearch.BeforeEdit
        e.Cancel = True
    End Sub

    Private Sub vsfsearch_DoubleClick(sender As Object, e As EventArgs) Handles vsfsearch.DoubleClick
        Call vsfSearchSelectedItem()
    End Sub

    Private Sub btnLoad_Click(sender As Object, e As EventArgs) Handles btnLoad.Click
        If validFlds() = True Then
            Call fillDtls()
            Call ReArrangeSlNo(vsfDet, vDetSlNo)
        End If
    End Sub

    Private Function validFlds() As Boolean
        Dim blnFound As Boolean
        validFlds = False

        If Trim(txtBOMDetails.Text) = String.Empty Then
            MsgBox("Please Enter BOMDetails..", vbInformation, Me.Text)
            txtBOMDetails.Focus()
            Exit Function
        End If

        lngItemID = clsSVT.getDataLongValue("Select ItemID from MstItem where ItemCode ='" & Trim(txtBOMDetails.Text) & "'", 0, blnFound)
        If Not blnFound Then
            MsgBox("Entered Item not found in Item Master..", vbInformation, Me.Text)
            txtBOMDetails.Focus()
            Exit Function
        End If

        validFlds = True

    End Function

    Private Sub txtBOMDetails_KeyUp(sender As Object, e As KeyEventArgs) Handles txtBOMDetails.KeyUp
        Dim StrQry As String, strCond As String = ""
        If e.KeyCode = Keys.Return Then
            Call vsfSearchSelectedItem()
            Exit Sub
        End If
        If e.KeyCode = Keys.Down Then
            vsfsearch.Focus()
            Exit Sub
        End If

        txtBOMDetails.Text = Replace(txtBOMDetails.Text, "'", "`")

        If e.KeyCode = Keys.Escape Then
            'txtBOMItemCode.Text = ""
            Call setSearchGridHt("")
            Exit Sub
        End If

        If Len(txtBOMDetails.Text) < 1 Then
            Call setSearchGridHt("")
            Exit Sub
        End If

        Call setSearchGridHt("FULL")
        StrQry = "Select Top " & gIntTopNrows & " ItemCode, ItemName "
        StrQry &= " from MstItem i Join  MstHdrBOM h on i.ItemID=BOMItemID "
        StrQry &= " Left Join MstDtlBOM d on h.BOMID=d.BOMID"
        StrQry &= " where ItemCode Like '%" & Trim(txtBOMDetails.Text) & "%'"
        StrQry &= " Or ItemName Like '%" & Trim(txtBOMDetails.Text) & "%'"
        StrQry &= " and i.ItemID = h.BOMItemID "
        StrQry &= " And BOMItem = 'Y'"
        StrQry &= " and d.BOMID is Not Null "
        StrQry &= " Group by Itemcode, Itemname "
        StrQry &= " order by ItemCode"
        Call showWaitCursor()
        dt = clsSVT.getDATATable(StrQry)
        dv = New DataView(dt)
        vsfsearch.DataSource = dv
        Call showNormalCursor()
        vsfsearch.Refresh()
        vsfsearch.ScrollBars = ScrollBars.Both
        vsfsearch.AutoSizeCols()
    End Sub

    Private Sub btnSel_Click(sender As Object, e As EventArgs) Handles btnSel.Click
        Dim frmDisplay As New frmUtlSingleSel
        txtBOMDetails.Text = ""
        'lblItemName.Text = ""
        'strList = ""
        Dim frmSel As New frmUtlMultiSelection

        Dim StrQry As String = "Select  ItemCode, ItemName "
        StrQry &= " from MstItem i Join  MstHdrBOM h on i.ItemID=BOMItemID "
        StrQry &= " Left Join MstDtlBOM d on h.BOMID=d.BOMID"
        StrQry &= " and i.ItemID = h.BOMItemID "
        StrQry &= " And BOMItem = 'Y'"
        StrQry &= " and d.BOMID is Not Null "
        StrQry &= " Group by Itemcode, Itemname "
        StrQry &= " order by ItemCode"

        With frmDisplay
            '.MdiParent = frmMain

            .Text = "Select from... "
            Call .FillViewVsf(Me, StrQry, txtBOMDetails, 1, 0)
            .ShowDialog()
            .vsfDet.Focus()
        End With

        'If cboItemCode.Text <> "" Then
        '    Call cboItemCode_Validated(sender, e)
        'End If
    End Sub

    Private Sub txtBOMQty_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtBOMQty.KeyPress
        If Not KeyValidate("Decimal_NonNegative", e.KeyChar) Then
            e.Handled = True
        End If
    End Sub

    Private Sub btnNext_Click(sender As Object, e As EventArgs) Handles btnNext.Click
        Dim introw As Int32
        With argForm
            Call RemoveEmptVSFRows(.vsfdet, 0, intColItemCode)
            For i As Int32 = 1 To vsfDet.Rows.Count - 1
                .vsfDet.Rows.Count = .vsfDet.Rows.Count + 1
                introw = .vsfDet.Rows.Count - 1
                Call .FillItemDet(vsfDet.GetData(i, vDetItemCode), introw, vsfDet.GetData(i, vDetUOM))
                .vsfDet.SetData(introw, intColQty, vsfDet.GetData(i, vDetQty))
            Next
        End With
        Call RemoveEmptVSFRows(argForm.vsfDet, 1, vDetItemCode)
        Me.Close()
    End Sub

    Private Sub vsfsearch_KeyUp(sender As Object, e As KeyEventArgs) Handles vsfsearch.KeyUp
        If vsfsearch.Rows.Count <= 1 Then Exit Sub
        Call vsfSearchSelectedItem()
    End Sub
End Class