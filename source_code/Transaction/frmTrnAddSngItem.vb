Imports System.ComponentModel
Imports C1.Win.C1FlexGrid

Public Class frmTrnAddSngItem

    Public objFrm As Object
    Dim dt As DataTable
    Dim dv As DataView
    Dim clsSVT As New svtLibrary
    Dim blnCase As Boolean = True

    Const vDetSlNo = 0
    Const vDetItemID = 1
    Const vDetItemSlNo = 2
    Const vDetItemCode = 3
    Const vDetItemName = 4
    Const vDetUOM = 5
    Const vDetConvFact = 17
    Dim intDetCols As Int32 = 19

    Public intColQty As Int32 = -1
    Public intColrate As Int32 = -1
    Public dblBalance As Double = 0

    Public intColCase As Int32, intColBot As Int32, intColStkQty As Int32, intColMRP As Int32, intColBotPerCase As Int32, intColAroedPer As Int32
    Dim dblLtrPerBot As Double = 0
    Public strRights As String

    Private Sub frmTrnAddSngItem_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'Call fillFormProperties(Me)
        'Call ChangeFontSize(Me)
        Me.KeyPreview = True
        initFormVsfDet(Me, Me.Text, vsfView, "VIE")
        If vsfView.Visible = True Then
            lblType.Visible = True
            lblInfo.Visible = False
        Else
            lblType.Visible = False
            lblInfo.Visible = True
        End If



    End Sub

    Private Sub btnClear_Click(sender As Object, e As EventArgs) Handles btnClear.Click
        Call ClearFlds()
    End Sub

    Public Sub ClearFlds()
        txtAmt.Text = ""
        txtBotPerCases.Text = ""
        txtCases.Text = ""
        txtItemName.Text = ""
        txtLooseBot.Text = ""
        txtPurRate.Text = ""
        If Not vsfView.Visible Then txtSearch.Text = ""
        txtSearch.Text = ""
        txtSellPrice.Text = ""
        txtItemType.Text = ""
        txtTotBot.Text = ""
        txtDummy.Text = ""
        lblOrderLevel.Text = ""
        lblMinLevel.Text = ""
        lblPresentStock.Text = ""
        txtItemInfo.Text = ""
        lblSalesPer.Text = ""
        dblLtrPerBot = 0
        lblType.Visible = True
        lblInfo.Visible = False
        txtAromedPer.Text = ""

    End Sub

    Private Sub frmTrnAddSngItem_KeyUp(sender As Object, e As KeyEventArgs) Handles Me.KeyUp
        raiseShortCutEvent(e)
        If e.KeyCode = Keys.F12 Then
            btnOK_Click(Nothing, Nothing)
            Exit Sub
        End If
        If e.KeyCode = Keys.F9 And e.Control Then
            Call initFormVsfDet(Me, Me.Text, vsfView, "VIE")
            MsgBox("Restored Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If

        If e.KeyCode = Keys.F8 And e.Control = True Then
            Call SaveActFormVsfDet(Me, Me.Text, "VIE", vsfView)
            MsgBox("Saved Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
    End Sub

        Private Sub raiseShortCutEvent(e As KeyEventArgs)
        If e.KeyCode = Keys.Escape Then
            objFrm.vsfHdr.Focus()
            objFrm.vsfHdr.Row = 0
            Me.Close()
            Exit Sub
        End If


    End Sub

    Private Sub getColNoFromPBForm()
        With objFrm
            intColCase = getColNoFromAddnlDetGrid(.vsfOptions, .vsfDet, "NOOFCASES")
            intColBot = getColNoFromAddnlDetGrid(.vsfOptions, .vsfDet, "NOOFBOTTLES")
            intColStkQty = getColNoFromAddnlDetGrid(.vsfOptions, .vsfDet, "STKQTY")
            intColMRP = getColNoFromAddnlDetGrid(.vsfOptions, .vsfDet, "MRP")
            intColBotPerCase = getColNoFromAddnlDetGrid(.vsfOptions, .vsfDet, "BOTPERCASE")
            ' If gStrDBName.ToUpper = "VLQMSAKSHAYA" Then
            intColAroedPer = getColNoFromAddnlDetGrid(.vsfOptions, .vsfDet, "AROEDPERBOT")
            ' End If
        End With
    End Sub

    Public Sub LoadItemsInGrid()
        Dim strSQL As String
        strSQL = "Select ItemID, ItemCode, ShortName, HotKey, ItemName, ItemGroupName, MRP "
        strSQL &= " From MstItem i, MstItemGroup g"
        strSQL &= " where g.ItemGroupID=i.ItemGroupID "
        'strSQL &= " And i.InActive = 'N' and BotPerCases > 0 and PKGID > 0  "
        strSQL &= " And i.InActive = 'N' and BotPerCases > 0 "
        strSQL &= " order by 1"

        dt = clsSVT.getDATATable(strSQL)
        dv = New DataView(dt)

        vsfView.Rows.Fixed = 1
        vsfView.Rows.GlyphRow = -1

        vsfView.BeginUpdate()
        vsfView.DataSource = dv
        vsfView.EndUpdate()
        vsfView.Cols(0).Visible = False
        Call getColNoFromPBForm()
    End Sub

    Private Sub FillDetails(ByVal argItemId As Integer)
        Dim rsDummy As oledb.oledbDataReader
        rsDummy = clsSVT.getDataReaderFromSQL("Select * from MstItem where ItemID=" & argItemId & "")

        If Not rsDummy.HasRows Then
            rsDummy.Close()
            Exit Sub
        End If

        rsDummy.Read()
        dblLtrPerBot = clsSVT.getDataDoubleValue("Select LtrPerBot From MstPKG Where PKGID=" & Val(rsDummy!PKGID), 1, False)
        txtItemName.Text = GetVal(rsDummy!ItemCode, "")
        txtBotPerCases.Text = GetVal(rsDummy!BotPerCases, 0)
        txtPurRate.Text = GetVal(rsDummy!PurRate, 0)
        txtPurDummy.Text = GetVal(rsDummy!PurRate, 0)
        txtSellPrice.Text = GetVal(rsDummy!ItemSell, 0)
        txtItemType.Text = GetVal(rsDummy!LiqType, "")
        lblOrderLevel.Text = "Order Level : " & GetVal(rsDummy!MAXLEVEL, 0)
        lblMinLevel.Text = "Min level : " & GetVal(rsDummy!MINLEVEL, 0)
        txtDummy.Text = GetVal(rsDummy!ITEMID, 0)
        txtAromedPer.Text = GetVal(rsDummy!aroedper, 0)
        lblPresentStock.Text = "Present Stock : " & clsSVT.getDataDoubleValue("Select CLBal from TrnQOH where ItemID = " & argItemId & "", 0, False)
        txtItemInfo.Text = GetVal(Trim(rsDummy!ItemName), "") & vbNewLine & GetVal(Trim(rsDummy!ShortName), "") & vbNewLine & GetVal(Trim(rsDummy!HotKey), "") & vbNewLine & Val(rsDummy!BotPerCases) & " Bottles per case"
        rsDummy.Close()

        CalTotal()
        'Try
        '    lblSalesPer.Text = "Price variation : " & Math.Round((((Val(txtSellPrice.Text) / Val(txtPurRate.Text)) * 100) - 100), 2) & "%"
        'Catch ex As Exception

        'End Try
        vsfView.Visible = False
    End Sub

    Private Sub ShowSearch()
        Dim strSql As String
        strSql = "Select  ItemID ,ItemCode  ,ItemName as Name, PHNO "
        strSql = strSql & " from MstItem"
        strSql = strSql & " Where InActive = 'N' "
        strSql = strSql & " order by 2"

        Dim objfrm As New frmUtlSelection
        With objfrm
            .blnMultiColSearch = True
            .FillViewVsf(Me, False, Nothing, strSql, txtDummy, 1)
            .Text = " Please Select Customer"
            .Tag = " frmTrnAddSngItem "
            .ShowDialog()
            .Close()
        End With

        If Trim(txtDummy.Text) <> "" Then
            FillDetails(txtDummy.Text)
        End If
    End Sub

    Private Sub vsfView_BeforeEdit(sender As Object, e As RowColEventArgs) Handles vsfView.BeforeEdit
        e.Cancel = True
    End Sub

    Private Sub txtSearch_TextChanged(sender As Object, e As EventArgs) Handles txtSearch.TextChanged
        vsfView.Visible = True
        Call applyFilterFromGrid()
    End Sub

    Private Sub txtSearch_KeyUp(sender As Object, e As KeyEventArgs) Handles txtSearch.KeyUp
        If Trim(txtSearch.Text) <> "" Then vsfView.BringToFront()
        Select Case e.KeyCode
            Case Keys.Enter
                If vsfView.Rows.Count = 2 Then
                    Call FillDetails(Val(vsfView.GetData(1, 0)))
                    vsfView.SendToBack()
                    txtSearch.Text = ""
                    txtCases.Focus()
                    Exit Select
                End If
                If vsfView.Rows.Count > 1 AndAlso vsfView.Row > 0 Then
                    Call FillDetails(Val(vsfView.GetData(1, 0)))
                    lblType.Visible = False
                    lblInfo.Visible = True
                    ClearFlds()
                Else
                    Call FillDetails(0)
                End If
            'Case Keys.Escape
            '    txtSearch.Text = ""
            Case Keys.Down
                If vsfView.Rows.Count > 1 Then
                    vsfView.Focus()
                Else
                    txtSearch.Focus()
                End If
            Case Else
                raiseShortCutEvent(e)
        End Select
    End Sub

    Private Function applyFilterFromGrid() As Boolean
        Dim StrQry As String = "", strValue As String
        Dim i As Integer = 0
        applyFilterFromGrid = False

        Try
            strValue = Trim(txtSearch.Text)
            If strValue <> "" Then
                With vsfView
                    For i = 1 To .Cols.Count - 1
                        If StrQry <> "" Then StrQry = StrQry & " or "
                        If vsfView.Cols(i).DataType = GetType(Decimal) Or vsfView.Cols(i).DataType = GetType(Integer) Or vsfView.Cols(i).DataType = GetType(Date) Then
                            Try
                                StrQry = StrQry & "Convert(" & vsfView.GetData(0, i) & ",'System.String')" & " like '" & strValue & "*'"
                            Catch ex As Exception
                                dv.RowFilter = "1 = 1"
                                Exit Function
                            End Try
                        Else
                            StrQry = StrQry & vsfView.GetData(0, i) & " like '%" & strValue & "%'"
                        End If
                    Next
                End With
                dv.RowFilter = StrQry
            Else
                dv.RowFilter = " 1 = 1 "
            End If
        Catch ex As Exception

        End Try
    End Function

    Private Sub vsfView_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles vsfView.MouseDoubleClick
        Try
            If vsfView.Rows.Count > 1 AndAlso vsfView.Row > 0 Then
                Call FillDetails(Val(vsfView.GetData(vsfView.Row, 0)))
                lblInfo.Visible = True
                lblType.Visible = False
                vsfView.SendToBack()
                txtSearch.Text = ""
                txtCases.Focus()
            Else
                Call FillDetails(0)
            End If

            'With vsfView
            '    If .Row < 1 Then
            '        Call FillDetails(0)
            '        lblType.Visible = False
            '        lblInfo.Visible = True
            '        Exit Sub
            '    Else
            '        ClearFlds()
            '        Call FillDetails(Val(.GetData(.Row, 0)))
            '        lblInfo.Visible = True
            '        lblType.Visible = False
            '        txtCases.Focus()
            '    End If
            'End With

            'Dim ht = vsfView.HitTest

            'If ht.Row <> -1 Then
            '    If ht.Row <= 0 Then
            '        Exit Sub
            '    End If
            '    ClearFlds()
            '    Call FillDetails(Val(vsfView.GetData(ht.Row, 0)))
            '    lblInfo.Visible = True
            '    lblType.Visible = False
            'Else
            '    Call FillDetails(0)
            '    lblType.Visible = False
            '    lblInfo.Visible = True
            'End If
        Catch ex As Exception
            MsgBox("Data Load Issues" & vbNewLine & ex.Message, MsgBoxStyle.Exclamation, Me.Text)
        End Try
    End Sub

    Private Sub vsfView_KeyUp(sender As Object, e As KeyEventArgs) Handles vsfView.KeyUp
        Select Case e.KeyCode
            Case Keys.Enter
                If vsfView.Rows.Count > 1 AndAlso vsfView.Row > 0 Then
                    Call FillDetails(Val(vsfView.GetData(vsfView.Row, 0)))
                    lblInfo.Visible = True
                    lblType.Visible = False
                    vsfView.SendToBack()
                    txtSearch.Text = ""
                    txtCases.Focus()
                Else
                    Call FillDetails(0)
                End If
            Case Keys.Escape
                txtSearch.Focus()
        End Select
    End Sub

    Private Sub btnOK_Click(sender As Object, e As EventArgs) Handles btnOK.Click
        If Val(txtDummy.Text) <= 0 Then
            'itemid msg
            MsgBox("Please select proper item", MsgBoxStyle.Information, Me.Text)
            Exit Sub
        End If

        If Val(txtAmt.Text) <= 0 Then
            'amt msg
            MsgBox("Please enter proper value", MsgBoxStyle.Information, Me.Text)
            Exit Sub
        End If

        If Not (Val(txtCases.Text) > 0 Or Val(txtLooseBot.Text) > 0 Or Val(txtTotBot.Text) > 0) Then
            'qty empty
            MsgBox("Please Enter the Qty", MsgBoxStyle.Information, Me.Text)
            Exit Sub
        End If
        If dblBalance > 0 Then
            If dblBalance - Val(txtAmt.Text) < 0 Then
                MsgBox("Balance amount is going negative... please check...", MsgBoxStyle.Exclamation, Me.Text)
            End If
        End If

        'purrate

        If txtPurRate.Text <= 0 Then
            MsgBox("Please enter proper value", MsgBoxStyle.Information, Me.Text)
        End If

        If txtSellPrice.Text <= 0 Then
            MsgBox("", MsgBoxStyle.Information, Me.Text)
        End If
        'sell rate
        If gDblPurSellVarPer > 0 And txtItemType.Text.ToUpper <> "OTHERS" Then
            Dim dblPer As Double = Math.Round((((Val(txtSellPrice.Text) / Val(txtPurRate.Text)) * 100) - 100), 2)
            If dblPer < gDblPurSellVarPer Then
                MsgBox("Purchase and Sales Variation to be greater than " & gDblPurSellVarPer & "%", MsgBoxStyle.Critical, Me.Text)
                Exit Sub
            End If
        End If

        Dim intRow As Int32
        With objFrm.vsfDet
            If .Rows.Count = 1 Then
                .Rows.Count = .Rows.Count + 1
                intRow = .Rows.Count - 1
            Else
                If GetVal(.GetData(.Rows.Count - 1, vDetItemID), 0) <= 0 Then
                    intRow = .Rows.Count - 1
                Else
                    .Rows.Count = .Rows.Count + 1
                    intRow = .Rows.Count - 1
                End If

            End If
            '.row = intRow
            Dim strUOM As String = clsSVT.getDataStringValue("Select UOMCode From MstUOM Where UOMID = (Select UOMID From MstItem Where ItemID=" & Val(txtDummy.Text) & ")", "", False)
            objFrm.FillItemDet(Trim(txtItemName.Text), intRow, strUOM)
            .SetData(intRow, vDetConvFact, 1)
            .SetData(intRow, intColQty, Val(txtTotBot.Text))
            .SetData(intRow, intColrate, Val(txtPurRate.Text))
            .SetData(intRow, intColCase, Val(txtCases.Text))
            .SetData(intRow, intColBot, Val(txtLooseBot.Text))
            .SetData(intRow, intColStkQty, Val(txtTotBot.Text))
            If gStrDBName.ToUpper = "VLQMSALC" Then
                .SetData(intRow, intColMRP, Val(txtSellPrice.Text)) '+ Val(txtAromedPer.Text))
            Else
                .SetData(intRow, intColMRP, Val(txtSellPrice.Text) + Val(txtAromedPer.Text))
            End If
            'If gStrDBName.ToUpper = "VLQMSAKSHAYA" Then
            If intColAroedPer >= 0 Then .SetData(intRow, intColAroedPer, Val(txtAromedPer.Text))
            ' End If

            objFrm.CalTotal(intRow)
            dblBalance = dblBalance - Val(txtAmt.Text)
            Call ClearFlds()

            vsfView.Visible = True
            vsfView.BringToFront()
            lblInfo.Visible = False
            lblType.Visible = True
            .col = vDetItemCode
            .row = .rows.count - 1
            .focus()
            txtSearch.Focus()

        End With
    End Sub

    Private Sub CalTotal()
        Try
            lblSalesPer.Text = "Price variation : " & Math.Round((((Val(txtSellPrice.Text) / Val(txtPurRate.Text)) * 100) - 100), 2) & "%"

            Dim dblTotBot As Double = 0
            dblTotBot = Val(txtTotBot.Text) '+ Val(txtLooseBot.Text)

            'txtAmt.Text = (Val(txtPurRate.Text) - Val(txtAromedPer.Text)) * dblTotBot
            txtAmt.Text = Val(txtPurRate.Text) * dblTotBot

        Catch ex As Exception

        End Try
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub txtTotalBottles_KeyUp(sender As Object, e As KeyEventArgs) Handles txtTotBot.KeyUp
        Try
            txtCases.Text = Math.Floor(Val(txtTotBot.Text) / Val(txtBotPerCases.Text))
            Dim intRem As Double
            intRem = Val(txtTotBot.Text) Mod Val(txtBotPerCases.Text)
            txtLooseBot.Text = IIf(intRem < 0, 0, Math.Floor(intRem))
            CalTotal()
            raiseShortCutEvent(e)
        Catch ex As Exception

        End Try
    End Sub

    Private Sub txtCases_KeyUp(sender As Object, e As KeyEventArgs) Handles txtCases.KeyUp
        Try
            txtTotBot.Text = (Val(txtCases.Text) * Val(txtBotPerCases.Text)) + Val(txtLooseBot.Text)
            CalTotal()
            raiseShortCutEvent(e)
        Catch ex As Exception

        End Try
    End Sub

    Private Sub txtLooseBottles_KeyUp(sender As Object, e As KeyEventArgs) Handles txtLooseBot.KeyUp
        Try
            If Val(txtLooseBot.Text) > Val(txtBotPerCases.Text) Then
                txtCases.Text = Val(txtCases.Text) + Math.Floor(Val(txtLooseBot.Text) / Val(txtBotPerCases.Text))
                Dim intRem As Double
                intRem = Val(txtLooseBot.Text) Mod Val(txtBotPerCases.Text)
                txtLooseBot.Text = IIf(intRem < 0, 0, Math.Floor(intRem))
            End If
            txtTotBot.Text = (Val(txtCases.Text) * Val(txtBotPerCases.Text)) + Val(txtLooseBot.Text)
            CalTotal()
            raiseShortCutEvent(e)
        Catch ex As Exception

        End Try
    End Sub

    Private Sub txtPurRate_KeyUp(sender As Object, e As KeyEventArgs) Handles txtPurRate.KeyUp
        Try
            CalTotal()
            raiseShortCutEvent(e)
        Catch ex As Exception

        End Try
    End Sub

    Private Sub txtCases_TextChanged(sender As Object, e As EventArgs) Handles txtCases.TextChanged

    End Sub

    Private Sub txtSellPrice_KeyUp(sender As Object, e As KeyEventArgs) Handles txtSellPrice.KeyUp
        Try
            CalTotal()
            raiseShortCutEvent(e)
        Catch ex As Exception

        End Try
    End Sub

    Private Sub txtAmt_KeyUp(sender As Object, e As KeyEventArgs) Handles txtAmt.KeyUp
        Try
            If Val(txtTotBot.Text) <> 0 Then
                'txtPurRate.Text = Math.Round(Val(txtAmt.Text) / Val(txtTotBot.Text), 2) + Val(txtAromedPer.Text)
                If gBlnPurBill4Deci Then
                    txtPurRate.Text = Math.Round(Val(txtAmt.Text) / Val(txtTotBot.Text), 4)
                Else
                    txtPurRate.Text = Math.Round(Val(txtAmt.Text) / Val(txtTotBot.Text), 2)
                End If
            End If
            lblSalesPer.Text = "Price variation : " & Math.Round((((Val(txtSellPrice.Text) / Val(txtPurRate.Text)) * 100) - 100), 2) & "%"
            raiseShortCutEvent(e)
        Catch ex As Exception

        End Try
    End Sub

    Private Sub txtCases_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtCases.KeyPress, txtLooseBot.KeyPress, txtTotBot.KeyPress
        If Not KeyValidate("Decimal_NonNegative", e.KeyChar) Then
            e.Handled = True
        End If

    End Sub

    Private Sub txtAmt_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtAmt.KeyPress, txtPurRate.KeyPress, txtSellPrice.KeyPress
        If Not chkUserRights(strRights, chkRightsPos.opt1Rights, Me.Text, False) Then
            e.Handled = True
            Exit Sub
        End If

        If Not KeyValidate("Decimal_NonNegative", e.KeyChar) Then
            e.Handled = True
        End If

    End Sub

    Private Sub txtAromedPer_KeyUp(sender As Object, e As KeyEventArgs) Handles txtAromedPer.KeyUp

    End Sub

    Private Sub txtAromedPer_TextChanged(sender As Object, e As EventArgs) Handles txtAromedPer.TextChanged

        'txtPurRate.Text = Math.Round(Val(txtAmt.Text) / Val(txtTotBot.Text), 2) + Val(txtAromedPer.Text)
        'Try
        '    If Val(txtAromedPer.Text) <> 0 Then
        '        txtPurRate.ReadOnly = True

        '        CalTotal()
        '    Else
        '        txtPurRate.ReadOnly = False
        '        CalTotal()
        '    End If


        'Catch ex As Exception

        'End Try
    End Sub


End Class