Imports C1.Win.C1FlexGrid

Public Class frmTrnAddMultiItem
    Dim clsSVT As New svtLibrary

    Const vMSlNo = 0
    Const vMItemID = 1
    Const vMItemCode = 2
    Const vMShortName = 3
    Const vMCases = 4
    Const vMLooseBottle = 5
    Const vMTotBtl = 6
    Const vMBotPerCases = 7
    Const vMPurRate = 8
    Const vMSelPrice = 9
    Const vMAmt = 10
    Const vMDiff = 11
    Const vMQOH = 12
    Const vMOrderLevel = 13
    Const vMMinLevel = 14
    Const vMTot = 15

    Const vDetConvFact = 17

    Public intColQty As Int32 = -1
    Public intColrate As Int32 = -1

    Public objFrm As Object

    Dim intColCase As Int32, intColBot As Int32, intColStkQty As Int32, intColMRP As Int32

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Call clearFlds()
    End Sub

    Private Sub frmMstContactPerson_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'For Fixed Columns getting size from tables
        Call initFormVsfDet(Me, Me.Text, vsfDet, "DET")

        'Initialize with Default columns in Grid
        Call initGridStyles(Me)

        'Initialize Form Properties with default settings
        Call fillFormProperties(Me)

        'Form initialize event
        Call initScreen()
        Call ChangeFontSize(Me)

        Call bindCombo(cboItemGroup, "ItemGroupName", "ItemGroupName", "Select ItemGroupName From MstITEMGROUP")

        'Call bindCombo(cboSel, "QSetName", "QSetName", " Select distinct QSetName from " & strTblName & " order by QSetName")
        ReArrangeSlNo(vsfDet, vMSlNo)

        Call clearFlds()
        Call getColNoFromPBForm()
        Call initFormVsfDet(Me, Me.Text, vsfDet, "DET")
    End Sub

    Private Sub clearFlds()
        txtShortName.Text = ""
        txtPAckingItem.Text = ""
        txtBCScan.Text = ""
        cboItemGroup.SelectedIndex = -1
        vsfDet.Rows.Count = 1
    End Sub

    Public Sub initScreen()
        Me.KeyPreview = True
        Call initFormSize(Me, Me.Text, False, 1043, 700, 0, 0)

        With vsfDet
            .Rows.Count = 1
            .Cols.Count = vMTot

            .SetData(0, vMSlNo, "SlNo")
            .SetData(0, vMItemID, "ItemID")
            .SetData(0, vMShortName, "ShortName")
            .SetData(0, vMItemCode, "ItemCode")
            .SetData(0, vMCases, "Cases")
            .SetData(0, vMLooseBottle, "Loose Bottles")
            .SetData(0, vMBotPerCases, "BotPerCases")
            .SetData(0, vMTotBtl, "Total Bottles")
            .SetData(0, vMPurRate, "Purchase Rate")
            .SetData(0, vMSelPrice, "Selling Price")
            .SetData(0, vMAmt, "Amount")
            .SetData(0, vMDiff, "Diff %")
            .SetData(0, vMQOH, "QOH")
            .SetData(0, vMOrderLevel, "Order Level")
            .SetData(0, vMMinLevel, "Min Level")

            .Cols(vMCases).DataType = GetType(Double)
            .Cols(vMLooseBottle).DataType = GetType(Double)
            .Cols(vMTotBtl).DataType = GetType(Double)

            .Cols(vMPurRate).DataType = GetType(Double)
            .Cols(vMSelPrice).DataType = GetType(Double)
            .Cols(vMAmt).DataType = GetType(Double)
            .Cols(vMDiff).DataType = GetType(Double)

        End With

        Call SetFlexColor(vsfDet, "", vMSlNo, vMShortName, vMBotPerCases, vMItemCode, vMDiff, vMQOH, vMOrderLevel, vMMinLevel)
        Call SetFlexColor(vsfDet, "M", vMCases, vMPurRate, vMSelPrice, vMAmt, vMTotBtl)
        Call SetHideCol(vsfDet, True, vMItemID)
    End Sub

    Private Sub getColNoFromPBForm()
        With objFrm
            intColCase = getColNoFromAddnlDetGrid(.vsfOptions, .vsfDet, "NOOFCASES")
            intColBot = getColNoFromAddnlDetGrid(.vsfOptions, .vsfDet, "NOOFBOTTLES")
            intColStkQty = getColNoFromAddnlDetGrid(.vsfOptions, .vsfDet, "STKQTY")
            intColMRP = getColNoFromAddnlDetGrid(.vsfOptions, .vsfDet, "MRP")
        End With
    End Sub

    Public Sub fillDtls(ByVal argType As String, ByVal argValue As String)
        Dim strSQL As String
        Dim rs As oledb.oledbDataReader

        strSQL = "Select * from MstItem i, MstItemGroup g"
        strSQL &= " Where i.ITEMGroupID = g.ITEMGROUPID "

        Select Case argType
            Case "G"
                strSQL &= " And g.ItemGroupCode = '" & argValue & "'"
            Case "S"
                strSQL &= " And i.ShortName = '" & argValue & "'"
            Case "P"
                strSQL &= " And i.PKGITEM = '" & argValue & "'"
            Case "B"
                strSQL &= " And (i.ItemCode = '" & argValue & "' Or i.ShortName = '" & argValue & "' Or i.HotKey = '" & argValue & "')"
        End Select
        strSQL &= " and BotPerCases > 0 and PKGID > 0 "
        strSQL &= " Order by 1"
        rs = clsSVT.getDataReaderFromSQL(strSQL)

        If Not rs.HasRows Then
            rs.Close()
            Exit Sub
        End If

        Dim i As Int32 = vsfDet.Rows.Count

        With rs
            Do While .Read
                Dim intRow As Int32 = 0
                intRow = vsfDet.FindRow(!ItemID, 1, vMItemID, True, True, False)
                If intRow > 0 Then GoTo nextrecord
                vsfDet.Rows.Count = vsfDet.Rows.Count + 1
                vsfDet.SetData(i, vMItemID, !ItemID)
                vsfDet.SetData(i, vMShortName, !ShortName)
                vsfDet.SetData(i, vMItemCode, !ItemCode)
                vsfDet.SetData(i, vMBotPerCases, !BotPerCases)
                vsfDet.SetData(i, vMPurRate, !PurRate)
                vsfDet.SetData(i, vMSelPrice, !ItemSell)
                vsfDet.SetData(i, vMOrderLevel, !MAXLEVEL)
                vsfDet.SetData(i, vMMinLevel, !MINLEVEL)
                i = i + 1
nextrecord:
            Loop
            .Close()
        End With
    End Sub

    Private Sub vsfDet_BeforeEdit(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfDet.BeforeEdit

        With vsfDet
            Select Case e.Col
                Case vMSlNo, vMShortName, vMDiff, vMQOH, vMOrderLevel, vMBotPerCases, vMMinLevel, vMItemCode
                    e.Cancel = True
                    Exit Sub
            End Select
        End With
    End Sub

    Private Sub frmTrnAddMultiItem_KeyUp(sender As Object, e As KeyEventArgs) Handles Me.KeyUp
        If e.KeyCode = Keys.F9 And e.Control = True Then
            Call initFormVsfDet(Me, Me.Text, vsfDet, "DET")
            MsgBox("Restored Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
        If e.KeyCode = Keys.F8 And e.Control = True Then
            Call SaveActFormVsfDet(Me, Me.Text, "DET", vsfDet)
            MsgBox("Saved Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
    End Sub

    Private Sub btnApply_Click(sender As Object, e As EventArgs) Handles btnApply.Click
        Dim strQry As String

        stringReplace(Me)

        Call RemoveEmptVSFRows(vsfDet, 1, vMShortName)

        If validFlds() Then

            If vsfDet.Rows.Count <= 1 Then
                MsgBox("Grid Empty...", MsgBoxStyle.Critical, Me.Text)
                vsfDet.Focus()
                Exit Sub
            End If

            Call loadOnToPB()
        End If
    End Sub

    Private Sub loadOnToPB()
        Dim intRow As Int32

        With objFrm.vsfDet
            If .Rows.Count = 1 Then
                .Rows.Count = .Rows.Count + 1
                intRow = .Rows.Count - 1
            Else
                If GetVal(.GetData(.Rows.Count - 1, vMItemID), 0) <= 0 Then
                    intRow = .Rows.Count - 1
                Else
                    .Rows.Count = .Rows.Count + 1
                    intRow = .Rows.Count - 1
                End If

            End If

            For i As Int32 = 1 To vsfDet.Rows.Count - 1

                Dim strUOM As String = clsSVT.getDataStringValue("Select UOMCode From MstUOM Where UOMID = (Select UOMID From MstItem Where ItemID=" & Val(vsfDet.GetData(i, vMItemID)) & ")", "", False)

                Dim strItemCode As String = clsSVT.getDataStringValue("Select ItemCode From MstItem Where ItemId = " & Val(vsfDet.GetData(i, vMItemID)) & "", "", False)

                objFrm.vsfDet.rows.count += 1
                objFrm.FillItemDet(Trim(strItemCode), intRow, strUOM)
                .SetData(intRow, vDetConvFact, 1)
                .SetData(intRow, intColQty, Val(vsfDet.GetData(i, vMTotBtl)))
                .SetData(intRow, intColrate, Val(vsfDet.GetData(i, vMPurRate)))
                .SetData(intRow, intColCase, Val(vsfDet.GetData(i, vMCases)))
                .SetData(intRow, intColBot, Val(vsfDet.GetData(i, vMLooseBottle)))
                .SetData(intRow, intColStkQty, Val(vsfDet.GetData(i, vMTotBtl)))
                .SetData(intRow, intColMRP, Val(vsfDet.GetData(i, vMSelPrice)))
                intRow += 1
            Next
        End With

        objFrm.CalTotal()
        Call clearFlds()
        cboItemGroup.Focus()
    End Sub

    Public Function validFlds() As Boolean
        validFlds = False

        With vsfDet
            For i = 1 To .Rows.Count - 1
                If Trim(.GetData(i, vMCases)) = String.Empty Then
                    MsgBox("Please Enter Cases..", vbInformation, Me.Text)
                    .Row = i
                    .TopRow = i
                    .Col = vMCases
                    .Focus()
                    Exit Function
                End If

                If Val(.GetData(i, vMTotBtl)) <= 0 Then
                    MsgBox("Please Enter Total Number of Bottles..", vbInformation, Me.Text)
                    .Row = i
                    .TopRow = i
                    .Col = vMTotBtl
                    .Focus()
                    Exit Function
                End If

                If Val(.GetData(i, vMPurRate)) <= 0 Then
                    MsgBox("Please Enter valid Purchase rate..", vbInformation, Me.Text)
                    .Row = i
                    .TopRow = i
                    .Col = vMPurRate
                    .Focus()
                    Exit Function
                End If

                If Trim(.GetData(i, vMSelPrice)) <= 0 Then
                    MsgBox("Please Enter valid Selling Price..", vbInformation, Me.Text)
                    .Row = i
                    .TopRow = i
                    .Col = vMSelPrice
                    .Focus()
                    Exit Function
                End If

                If Trim(.GetData(i, vMAmt)) <= 0 Then
                    MsgBox("Amount cannot be less than zero..", vbInformation, Me.Text)
                    .Row = i
                    .TopRow = i
                    .Col = vMAmt
                    .Focus()
                    Exit Function
                End If

            Next
        End With

        validFlds = True
    End Function

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Call loadOnToPB()
        Me.Close()
    End Sub

    Private Function chkFilterItem() As Boolean
        chkFilterItem = False

        If Trim(cboItemGroup.Text) = "" AndAlso
                Trim(txtShortName.Text) = "" AndAlso
                Trim(txtBCScan.Text) = "" AndAlso
                Trim(txtPAckingItem.Text) = "" Then
            MsgBox("Filter is empty....", MsgBoxStyle.Information, Me.Text)
            cboItemGroup.Focus()
            Exit Function
        End If

        If Trim(cboItemGroup.Text) <> "" AndAlso (Trim(txtShortName.Text) <> "" OrElse Trim(txtPAckingItem.Text) <> "" OrElse Trim(txtBCScan.Text) <> "") Then
            MsgBox("Please enter only one filter text....", MsgBoxStyle.Exclamation, Me.Text)
            Exit Function
        Else
            If Trim(txtShortName.Text) <> "" AndAlso (Trim(txtPAckingItem.Text) <> "" OrElse Trim(txtBCScan.Text) <> "") Then
                MsgBox("Please enter only one filter text....", MsgBoxStyle.Exclamation, Me.Text)
                Exit Function
            Else
                If Trim(txtPAckingItem.Text) <> "" AndAlso Trim(txtBCScan.Text) <> "" Then
                    MsgBox("Please enter only one filter text....", MsgBoxStyle.Exclamation, Me.Text)
                    Exit Function
                End If
            End If
        End If

        Return True
    End Function

    Private Sub btnView_Click(sender As Object, e As EventArgs) Handles btnView.Click

        If Not chkFilterItem() Then Exit Sub

        If cboItemGroup.Text <> "" Then
            Call fillDtls("G", cboItemGroup.Text)
        End If

        If txtShortName.Text <> "" Then
            Call fillDtls("S", txtShortName.Text)
        End If

        If txtPAckingItem.Text <> "" Then
            Call fillDtls("P", txtPAckingItem.Text)
        End If

        If txtBCScan.Text <> "" Then
            Call fillDtls("B", txtBCScan.Text)
        End If

        Call RemoveEmptVSFRows(vsfDet, 1, vMItemID)
        ReArrangeSlNo(vsfDet, vMSlNo)
    End Sub

    Private Sub btnItemGroup_Click(sender As Object, e As EventArgs) Handles btnItemGroup.Click
        Dim strSql As String
        strSql = "Select  ItemGroupID, ItemGroupCode as Code, ItemGroupName as Name "
        strSql = strSql & " from MstItemGroup "
        strSql = strSql & " Where InActive = 'N' and ItemGroupID > 0 "
        strSql = strSql & " order by 2"

        Dim frmDisplay As New frmUtlSingleSel
        With frmDisplay
            .Text = "Select Item Group... "
            Call .FillViewVsf(Me, strSql, cboItemGroup, 1, 0)
            .ShowDialog()
            .Close()
        End With

    End Sub

    Private Sub btnShrtName_Click(sender As Object, e As EventArgs) Handles btnShrtName.Click
        Dim strSql As String
        strSql = "Select  Distinct ShortName, ShortName as Name"
        strSql = strSql & " from MstItem "
        strSql = strSql & " Where InActive = 'N' and ItemID > 0 "
        strSql = strSql & " order by 2"


        Dim frmDisplay As New frmUtlSingleSel
        With frmDisplay
            .Text = "Select Short Name... "
            Call .FillViewVsf(Me, strSql, txtShortName, 1, 0)
            .ShowDialog()
            .Close()
        End With
    End Sub

    Private Sub btnPackingItem_Click(sender As Object, e As EventArgs) Handles btnPackingItem.Click
        Dim strSql As String
        strSql = " Select Distinct PKGITEM as PKG, PKGITEM  "
        strSql = strSql & " from MstItem "
        strSql = strSql & " Where InActive = 'N' AND ItemID > 0 "
        strSql = strSql & " order by 2"

        Dim frmDisplay As New frmUtlSingleSel
        With frmDisplay
            .Text = "Select Packing Item... "
            Call .FillViewVsf(Me, strSql, txtPAckingItem, 1, 0)
            .ShowDialog()
            .Close()
        End With
    End Sub

    Private Sub CalTotal()
        With vsfDet
            For i As Int32 = 1 To .Rows.Count - 1
                Dim dblTotBot As Double = 0
                .SetData(i, vMTotBtl, (Val(.GetData(i, vMBotPerCases)) * Val(.GetData(i, vMCases)) + Val(.GetData(i, vMLooseBottle))))
                .SetData(i, vMAmt, Math.Round(Val(.GetData(i, vMTotBtl)) * Val(.GetData(i, vMPurRate)), 2))
                .SetData(i, vMDiff, Math.Round((((Val(.GetData(i, vMSelPrice)) / Val(.GetData(i, vMPurRate))) * 100) - 100), 2))
            Next
        End With
    End Sub

    Private Sub btnReplace_Click(sender As Object, e As EventArgs) Handles btnReplace.Click
        Try
            With vsfDet
                .EndUpdate()
                .FinishEditing()

                If CStr(.GetData(.Row, .Col) & "") = "" Then
                    MsgBox("No data present!", MsgBoxStyle.Exclamation, "Invalid Selection")
                    Exit Sub
                End If

                If .Rows.Count <= 2 Then
                    MsgBox("Please ensure there are 2 or more records before using replace rest!", MsgBoxStyle.Exclamation, "Invalid Selection")
                    Exit Sub
                End If

                If .Row < 1 Then
                    MsgBox("Please ensure there are 2 or more records before using replace rest!", MsgBoxStyle.Exclamation, "Invalid Selection")
                    Exit Sub
                End If

                Dim i As Integer, strTxt As String

                Dim rgBlue As CellRange = .GetCellRange(0, .Col)
                If rgBlue.Style.ForeColor.Name = "Blue" Then
                    MsgBox("Column is readonly! Cannot use Replace Rest!", MsgBoxStyle.Information, Me.Text)
                    Exit Sub
                End If

                strTxt = GetVal(.GetData(.Row, .Col), "")
                If strTxt = "" Then strTxt = GetVal(.Editor.Text, "")
                For i = .Row To .Rows.Count - 1
                    If GetVal(.GetData(i, vMItemCode), "") <> "" Then
                        .SetData(i, .Col, strTxt)
                    End If
                Next i
            End With
            Call CalTotal()
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Exclamation, Me.Text & " Replace ")
        End Try
    End Sub

    Private Sub vsfDet_AfterEdit(sender As Object, e As RowColEventArgs) Handles vsfDet.AfterEdit
        Try
            With vsfDet
                Select Case e.Col
                    Case vMCases
                        .SetData(e.Row, vMTotBtl, (Val(.GetData(e.Row, vMBotPerCases)) * Val(.GetData(e.Row, vMCases)) + Val(.GetData(e.Row, vMLooseBottle))))
                    Case vMLooseBottle
                        If Val(.GetData(e.Row, vMLooseBottle)) > Val(.GetData(e.Row, vMBotPerCases)) Then
                            .SetData(e.Row, vMCases, Val(.GetData(e.Row, vMCases)) + Math.Floor(Val(.GetData(e.Row, vMLooseBottle)) / Val(.GetData(e.Row, vMBotPerCases))))
                            Dim intRem As Int32
                            Math.DivRem(CInt(.GetData(e.Row, vMLooseBottle)), CInt(.GetData(e.Row, vMBotPerCases)), intRem)
                            .SetData(e.Row, vMLooseBottle, intRem)
                        End If
                        .SetData(e.Row, vMTotBtl, (Val(.GetData(e.Row, vMBotPerCases)) * Val(.GetData(e.Row, vMCases)) + Val(.GetData(e.Row, vMLooseBottle))))
                    Case vMTotBtl
                        .SetData(e.Row, vMCases, Math.Floor(Val(.GetData(e.Row, vMTotBtl)) / Val(.GetData(e.Row, vMBotPerCases))))
                        Dim intRem As Int32
                        Math.DivRem(CInt(.GetData(e.Row, vMTotBtl)), CInt(.GetData(e.Row, vMBotPerCases)), intRem)
                        .SetData(e.Row, vMLooseBottle, intRem)
                End Select
            End With
            CalTotal()
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Exclamation, Me.Text)
        End Try
    End Sub

    Private Sub raiseShortCutEvent(e As KeyEventArgs)
        Select Case e.KeyCode
            Case Keys.Delete
                If vsfDet.Row > 0 Then
                    vsfDet.RemoveItem(vsfDet.Row)
                    Call ReArrangeSlNo(vsfDet, vMSlNo)
                End If
                CalTotal()
            Case Keys.Escape
                Me.Close()
            Case Keys.F4
                btnView_Click(Nothing, Nothing)
            Case Keys.F10
                btnApply_Click(Nothing, Nothing)
            Case Keys.F11
                btnClose_Click(Nothing, Nothing)
        End Select
    End Sub

    Private Sub txtBCScan_KeyUp(sender As Object, e As KeyEventArgs) Handles txtBCScan.KeyUp
        raiseShortCutEvent(e)
    End Sub
End Class

