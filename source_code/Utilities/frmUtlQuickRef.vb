Imports C1.Win.C1FlexGrid

Public Class frmUtlQuickRef

    Dim strSql As String
    Dim dt As DataTable
    Dim clsSvt As New svtLibrary

    Const vDetColName = 0
    Const vDetColValue = 1
    Dim intColCount As Int32 = 2

    Const vDetRowPur = 1
    Const vDetRowSale = 2
    Const vDetStkDeduct = 3
    Const vDetStkAdd = 4
    Const vDetPI = 5
    Dim intRowCount As Int32 = 6

    Dim rs As oledb.oledbDataReader

    Private Sub frmUtlQuickRef_Load(sender As Object, e As EventArgs) Handles Me.Load
        Call initScreen()
    End Sub

    Private Sub initScreen()

        Me.KeyPreview = True
        Call initGridStyles(Me)
        Call fillFormProperties(Me)
        'Call ChangeFontSize(Me)     
        Call initFormVsfDet(Me, Me.Text, vsfDet, "DET")
        With vsfDet
            .Rows.Fixed = 1
            '.Cols.Fixed = 1
            .Cols.Count = intColCount
            .Rows.Count = intRowCount

            .SetData(0, vDetColName, "Document")
            .SetData(0, vDetColValue, "Document Info")

            .SetData(vDetRowPur, vDetColName, "Purchase")
            .SetData(vDetRowSale, vDetColName, "Sales")
            .SetData(vDetStkDeduct, vDetColName, "Stock Deduce")
            .SetData(vDetStkAdd, vDetColName, "Stock Add")
            .SetData(vDetPI, vDetColName, "Purchase Indent")
        End With
    End Sub

    Private Sub txtItem_KeyUp(sender As Object, e As KeyEventArgs) Handles txtItem.KeyUp

        'If e.KeyCode = Keys.Escape Then
        '    txtItem.Text = ""
        '    vsfSearch.Visible = False
        '    vsfSearch.SendToBack()
        'End If


        If txtItem.Text = "" Then
            vsfSearch.Visible = False
            vsfSearch.SendToBack()
            Exit Sub
        Else
            vsfSearch.Visible = True
            vsfSearch.BringToFront()
            Call showWaitCursor()
            strSql = " Select Itemid, ItemCode, ItemName, ShortName, HotKey "
            strSql &= " from MstItem "
            strSql &= " where ItemCode like '%" & txtItem.Text & "%' or "
            strSql &= " ItemName like '%" & txtItem.Text & "%' or "
            strSql &= " ShortName like '%" & txtItem.Text & "%' or "
            strSql &= " HotKey like '%" & txtItem.Text & "%' "

            dt = clsSvt.getDATATable(strSql)
            vsfSearch.DataSource = dt
            Call SetHideCol(vsfSearch, True, 0, 1)
            Call showNormalCursor()
        End If

        If e.KeyCode = Keys.Down AndAlso vsfSearch.Visible = True Then
            With vsfSearch
                .Row = 1
                .Col = 1
                .Focus()
            End With
        End If

        If e.KeyCode = Keys.Enter Then
            If vsfSearch.Visible = True AndAlso vsfSearch.Rows.Count > 1 Then
                With vsfSearch
                    If .Rows.Count <= 1 Then Exit Sub

                    strSql = "Select ItemID from MstItem where ItemID ='" & vsfSearch.GetData(1, 0) & "'"
                    txtDummy.Text = clsSvt.getDataLongValue(strSql, 0, False)

                    strSql = ""
                    strSql = " Select ItemId, ItemCode, ItemName, ShortName, HotKey, MRP, ITEMSELL  "
                    strSql &= " from MstItem "
                    strSql &= " where ItemId = '" & txtDummy.Text & "'"

                    rs = clsSvt.getDataReaderFromSQL(strSql)

                    If Not rs.HasRows() Then
                        txtItem.Focus()
                        Exit Sub
                    End If
                    Do While rs.Read
                        txtItemDet.Text = Trim(GetVal(rs!ItemCode, "")) & vbCrLf
                        txtItemDet.Text &= Trim(GetVal(rs!ItemName, "")) & vbCrLf
                        txtItemDet.Text &= GetVal(rs!ShortName, "") & vbCrLf
                        txtItemDet.Text &= GetVal(rs!HotKey, "")
                        txtMRP.Text = GetVal(rs!MRP, "")
                        txtSP.Text = GetVal(rs!ITEMSELL, "")
                        txtQOH.Text = clsSvt.getDataDoubleValue("Select CLBAL from TrnQOH where ItemId='" & txtDummy.Text & "'", 0, False)
                    Loop
                    vsfSearch.Visible = False
                    txtItem.Text = ""
                    txtItem.Focus()
                    Call PurchaseFill()
                    Call SalesFill()
                End With
            Else
                vsfSearch.Rows.Count = 1
                vsfSearch.Visible = True
            End If
        End If
    End Sub

    Private Sub txtItem_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtItem.KeyPress
        If e.KeyChar = "'" Then
            e.Handled = True
        End If
    End Sub

    Private Sub vsfSearch_DoubleClick(sender As Object, e As EventArgs) Handles vsfSearch.DoubleClick

        With vsfSearch
            If .Rows.Count <= 1 Then Exit Sub

            strSql = "Select ItemID from MstItem where ItemID ='" & vsfSearch.GetData(vsfSearch.Row, 0) & "'"
            txtDummy.Text = clsSvt.getDataLongValue(strSql, 0, False)

            strSql = ""
            strSql = " Select ItemId, ItemCode, ItemName, ShortName, HotKey, MRP, ITEMSELL  "
            strSql &= " from MstItem "
            strSql &= " where ItemId = '" & txtDummy.Text & "'"

            rs = clsSvt.getDataReaderFromSQL(strSql)

            If Not rs.HasRows() Then
                txtItem.Focus()
                Exit Sub
            End If
            Do While rs.Read
                txtItemDet.Text = Trim(GetVal(rs!ItemCode, "")) & vbCrLf
                txtItemDet.Text &= Trim(GetVal(rs!ItemName, "")) & vbCrLf
                txtItemDet.Text &= GetVal(rs!ShortName, "") & vbCrLf
                txtItemDet.Text &= GetVal(rs!HotKey, "")
                txtMRP.Text = GetVal(rs!MRP, "")
                txtSP.Text = GetVal(rs!ITEMSELL, "")
                txtQOH.Text = clsSvt.getDataDoubleValue("Select CLBAL from TrnQOH where ItemId='" & txtDummy.Text & "'", 0, False)
            Loop
            vsfSearch.Visible = False
            txtItem.Text = ""
            txtItem.Focus()
            Call PurchaseFill()
            Call SalesFill()
        End With
    End Sub

    Private Sub vsfSearch_KeyUp(sender As Object, e As KeyEventArgs) Handles vsfSearch.KeyUp

        If e.Control = True AndAlso e.KeyCode = Keys.Tab Then
            txtItem.Text = ""
            txtItem.Focus()
        End If

        If e.KeyCode = Keys.Enter Then
            With vsfSearch
                If .Rows.Count <= 1 Then Exit Sub

                strSql = "Select ItemID from MstItem where ItemID ='" & vsfSearch.GetData(vsfSearch.Row, 0) & "'"
                txtDummy.Text = clsSvt.getDataLongValue(strSql, 0, False)

                strSql = ""
                strSql = " Select ItemId, ItemCode, ItemName, ShortName, HotKey, MRP, ITEMSELL  "
                strSql &= " from MstItem "
                strSql &= " where ItemId = '" & txtDummy.Text & "'"

                rs = clsSvt.getDataReaderFromSQL(strSql)

                If Not rs.HasRows() Then
                    txtItem.Focus()
                    Exit Sub
                End If
                Do While rs.Read
                    txtItemDet.Text = Trim(GetVal(rs!ItemCode, "")) & vbCrLf
                    txtItemDet.Text &= Trim(GetVal(rs!ItemName, "")) & vbCrLf
                    txtItemDet.Text &= GetVal(rs!ShortName, "") & vbCrLf
                    txtItemDet.Text &= GetVal(rs!HotKey, "")
                    txtMRP.Text = GetVal(rs!MRP, "")
                    txtSP.Text = GetVal(rs!ITEMSELL, "")
                    txtQOH.Text = clsSvt.getDataDoubleValue("Select CLBAL from TrnQOH where ItemId='" & txtDummy.Text & "'", 0, False)
                Loop
                vsfSearch.Visible = False
                txtItem.Text = ""
                txtItem.Focus()
                Call PurchaseFill()
                Call SalesFill()
            End With
        End If
    End Sub

    Private Sub PurchaseFill()

        strSql = " Select Top 1 ('No: '+ h.PBNo + ',' + cast(h.PBDate as varchar(50)) + ', Qty:' + cast(d.PBQty as varchar(50)) + "
        strSql &= " ', Rate: ' + cast(d.PBRate as varchar(50))) as FldValues from "
        strSql &= " TrnHdrPB h, TrnDtlPB d, MstItem i "
        strSql &= " Where h.PBID = d.PBID "
        strSql &= " And d.ItemId = i.ItemId "
        strSql &= " And d.ItemId = '" & txtDummy.Text & "'"
        strSql &= " order by h.PBID desc "

        rs = clsSvt.getDataReaderFromSQL(strSql)
        If Not rs.HasRows() Then
            rs.Close()
            Exit Sub
        End If

        Do While rs.Read
            With vsfDet
                .SetData(vDetRowPur, vDetColValue, rs!FldValues)
            End With
        Loop
        rs.Close()
    End Sub

    Private Sub SalesFill()

        strSql = " Select Top 1 ('No: '+ h.SINVNo + ',' + cast(h.SINVDate as varchar(50)) + "
        strSql &= " ', Qty:' + cast(d.SINVQty as varchar(50)) + ', Rate: ' + cast(d.SINVRate as varchar(50))) as FldValues from "
        strSql &= " TrnHdrSINV h, TrnDtlSINV d, MstItem i "
        strSql &= " Where h.SINVID = d.SINVID "
        strSql &= " and d.ItemId = i.ItemId "
        strSql &= " and d.ItemId = '" & txtDummy.Text & "'"
        strSql &= " order by h.SINVID desc "

        rs = clsSvt.getDataReaderFromSQL(strSql)
        If Not rs.HasRows() Then
            rs.Close()
            Exit Sub
        End If

        Do While rs.Read
            With vsfDet
                .SetData(vDetRowSale, vDetColValue, rs!FldValues)
            End With
        Loop
        rs.Close()
    End Sub
    Private Sub frmUtlQuickRef_KeyUp(sender As Object, e As KeyEventArgs) Handles Me.KeyUp
        If e.KeyCode = Keys.F9 And e.Control Then
            Call initFormVsfDet(Me, Me.Text, vsfDet, "DET")
            Dim argDsgrGrids = GetAllDsgrVsf(Me, GetType(C1FlexGrid))
            For Each GridList As C1FlexGrid In argDsgrGrids
                If GridList.Visible Then
                    Call initFormVsfDet(Me, Me.Text, GridList, GridList.Name)
                End If
            Next
            MsgBox("Restored Form & Grid Layout Successfully!!", vbInformation, Me.Text)
            Exit Sub
        End If

        If e.KeyCode = Keys.F8 And e.Control = True Then
            Call SaveActFormVsfDet(Me, Me.Text, "DET", vsfDet)
            Dim argDsgrGrids = GetAllDsgrVsf(Me, GetType(C1FlexGrid))
            For Each GridList As C1FlexGrid In argDsgrGrids
                If GridList.Visible Then
                    Call SaveActFormVsfDet(Me, Me.Text, GridList.Name, GridList)
                End If
            Next
            MsgBox("Saved Form & Grid Layout Successfully!!", vbInformation, Me.Text)
            Exit Sub
        End If

        If e.KeyCode = Keys.Escape Then
            Me.Close()
        End If
    End Sub

    Private Sub vsfSearch_BeforeEdit(sender As Object, e As RowColEventArgs) Handles vsfSearch.BeforeEdit
        e.Cancel = True
    End Sub

    Private Sub txtItem_LostFocus(sender As Object, e As EventArgs) Handles txtItem.LostFocus

        If vsfSearch.Visible = False OrElse vsfSearch.Rows.Count = 1 Then
            txtItem.Focus()
            Exit Sub
        Else
            With vsfSearch
                .Row = 1
                .Col = 2
                .Focus()
            End With
        End If
    End Sub

    Private Sub vsfDet_BeforeEdit(sender As Object, e As RowColEventArgs) Handles vsfDet.BeforeEdit
        e.Cancel = True
    End Sub

End Class