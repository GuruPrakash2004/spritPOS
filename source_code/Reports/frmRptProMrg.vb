Imports System.Data.oledb
Imports C1.Win.C1FlexGrid

Public Class frmRptProMrg
    Dim clsSVT As New svtLibrary

    Public strList As String
    Dim strItemGroupList As String
    Dim strItemList As String
    Dim strValMethod As String
    Dim arrRctps(4, 1) As String
    Dim arrFlds() As String
    Dim intAdnlFldCount As String
    Public strRptName As String = ""

    Const vDetItmGrp = 0
    Const vDetItmCod = 1
    Const vDetItNam = 2
    Const vDetPurRat = 3
    Const vDetPurVal = 4
    Const vDetSalQty = 5
    Const vDetSalRat = 6
    Const vDetSalVal = 7

    Const vDetAROEDVal = 8

    Const vDetPofMrg = 9
    Const vDetPofPer = 10
    Const vDetTotCol = 11


    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        frmMain.CloseSelectedTabPage(Me)
    End Sub

    Private Function ValidateFlds() As Boolean
        ValidateFlds = False


        'check the date range
        If CDate(dtpTo.Value) < CDate(dtpFrom.Value) Then
            MsgBox("Invalid Date Range!", MsgBoxStyle.Critical, Me.Text)
            Exit Function
        End If
        ValidateFlds = True
    End Function

    Private Sub btnView_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView.Click
        Call INIStore("PMReport", "CHKFIFO", IIf(chkFIfo.Checked, "Y", "N"))
        If ValidateFlds() Then
            If chkFIfo.Checked Then
                Call fillFifoPM
                Exit Sub
            End If
            Call Fillitems()
        End If
    End Sub

    Private Sub Fillitems()
        Dim rs As oledb.oledbDataReader
        Dim strSQL As String
        Dim intRow As Integer
        Dim strInvFld As String
        Dim strPurRateFld As String

        If fnFldPresent("PMPURRATE", "MstItem") Then
            strPurRateFld = "PMPurRate"
        Else
            strPurRateFld = "PurRate"
        End If

        strInvFld = clsSVT.getDataStringValue("Select ProfitMgnInvFld from UtlMisc", "SINVAMT", False)
        If strInvFld = "" Then strInvFld = "SINVAMT"

        strSQL = "Select g.ItemGroupName, ItemCode, ItemName, " & strPurRateFld & ", sum(SInvQty) as SQty,sum(" & strInvFld & ") as SAmt,ISNULL(I.AROEDPER,0) AS AROEDPER "
        strSQL &= " From TrnDtlSInv d, TrnHdrSInv h, Mstitem i, MstItemGroup g "
        strSQL &= " Where H.SInvDate between " & getDateFormatInCond(dtpFrom.Value) & ""
        strSQL &= " And " & getDateFormatInCond(dtpTo.Value) & ""
        strSQL &= " And D.SInvID = H.SINVID "
        strSQL &= " And D.ItemID = I.ItemID "
        strSQL &= " And g.Itemgroupid = i.Itemgroupid "

        If chkItemGroup.Checked Then
            If strItemGroupList <> "" Then
                strSQL &= " And i.ItemGroupID In (" & strItemGroupList & ") "
            End If
        End If

        If chkItem.Checked Then
            If strItemList <> "" Then
                strSQL &= " And i.ItemID In (" & strItemList & ") "
            End If
        End If

        strSQL &= " group by itemgroupname, itemcode, itemname,AROEDPER, " & strPurRateFld & " having sum(SInvQty) > 0 Order by 1,3 "

        rs = clsSVT.getDataReaderFromSQL(strSQL)
        With vsfView
            .Rows.Count = 1
            If rs.HasRows Then
                Do While rs.Read
                    .Rows.Count = .Rows.Count + 1
                    intRow = .Rows.Count - 1
                    .SetData(intRow, vDetItmGrp, rs("ItemGroupName"))
                    .SetData(intRow, vDetItmCod, rs("ItemCode"))
                    .SetData(intRow, vDetItNam, rs("Itemname"))
                    .SetData(intRow, vDetPurRat, rs("" & strPurRateFld & ""))
                    .SetData(intRow, vDetPurVal, Math.Round(GetVal(rs("" & strPurRateFld & ""), 0) * GetVal(rs("SQty"), 0), 2))
                    .SetData(intRow, vDetSalQty, rs("SQty"))
                    .SetData(intRow, vDetSalVal, rs("SAmt"))

                    If Val(.GetData(.Rows.Count - 1, vDetSalQty)) > 0 Then
                        .SetData(.Rows.Count - 1, vDetSalRat, Math.Round(Val(.GetData(.Rows.Count - 1, vDetSalVal)) / Val(rs("SQty")), 2))
                    End If
                    If Val(rs("aROEDPER")) > 0 Then
                        '.SetData(.Rows.Count - 1, vDetAROEDVal, Math.Round((.GetData(.Rows.Count - 1, vDetSalVal)) - (.GetData(.Rows.Count - 1, vDetSalQty) * Val(rs("aROEDPER"))), 2))
                        Dim dblSalQty As Double = (.GetData(.Rows.Count - 1, vDetSalQty) * Val(rs("aROEDPER")))
                        .SetData(.Rows.Count - 1, vDetAROEDVal, Math.Round((.GetData(.Rows.Count - 1, vDetSalVal) - dblSalQty), 2))
                        .SetData(.Rows.Count - 1, vDetPofMrg, Math.Round(.GetData(.Rows.Count - 1, vDetAROEDVal) - .GetData(.Rows.Count - 1, vDetPurVal), 2))
                    Else
                        .SetData(.Rows.Count - 1, vDetAROEDVal, 0)
                        .SetData(.Rows.Count - 1, vDetPofMrg, Math.Round(.GetData(.Rows.Count - 1, vDetSalVal) - .GetData(.Rows.Count - 1, vDetPurVal), 2))
                    End If

                    If Val(.GetData(.Rows.Count - 1, vDetPurVal)) > 0 Then
                        .SetData(.Rows.Count - 1, vDetPofPer, Math.Round(.GetData(.Rows.Count - 1, vDetPofMrg) / .GetData(.Rows.Count - 1, vDetPurVal) * 100, 2) & "%")
                        '.SetData(intRow, vDetAROEDVal, Math.Round(.GetData(.Rows.Count - 1, vDetSalVal) - rs("aROEDPER")))
                    End If

                Loop
            End If
            rs.Close()


            With vsfView
                .Cols(vDetSalQty).Format = "#"
                .Cols(vDetSalRat).Format = "#.00"
                .AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.RestrictAll
                .Cols(vDetItmGrp).AllowMerging = True

                .SubtotalPosition = C1.Win.C1FlexGrid.SubtotalPositionEnum.BelowData


                .Subtotal(C1.Win.C1FlexGrid.AggregateEnum.Sum, 0, vDetItmGrp, vDetPurVal)
                .Subtotal(C1.Win.C1FlexGrid.AggregateEnum.Sum, 0, vDetItmGrp, vDetSalVal)
                .Subtotal(C1.Win.C1FlexGrid.AggregateEnum.Sum, 0, vDetItmGrp, vDetPofMrg)
                .Subtotal(C1.Win.C1FlexGrid.AggregateEnum.Sum, 0, vDetItmGrp, vDetAROEDVal)

                .Subtotal(C1.Win.C1FlexGrid.AggregateEnum.Sum, 1, -1, vDetPurVal)
                .Subtotal(C1.Win.C1FlexGrid.AggregateEnum.Sum, 1, -1, vDetSalVal)
                .Subtotal(C1.Win.C1FlexGrid.AggregateEnum.Sum, 1, -1, vDetPofMrg)
                .Subtotal(C1.Win.C1FlexGrid.AggregateEnum.Sum, 1, -1, vDetAROEDVal)

                .AutoSizeCols()
            End With

        End With
    End Sub
    Private Sub frmRptStkLtr_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.KeyPreview = True
        strRptName = "RPTPFTMGN"
        dtpFrom.Value = gSettings.gCompStDt
        dtpTo.Value = getSystemDate()
        If INIGet("PMReport", "CHKFIFO", "N") = "Y" Then
            chkFIfo.Checked = True
        End If
        InitScreen()
        initFormSize(Me, Me.Text, False, 958, 579, 0, 0)
        initFormVsfDet(Me, Me.Text, vsfView, "VIE")
        fillFormProperties(Me)
        initGridStyles(Me)
        If gStrDBName.ToUpper = "VLQMSDROPSSR" Or gStrDBName.ToUpper = "VLQMSDROPSJP" Or gStrDBName.ToUpper = "VLQMSDROPSIND1" Or gStrDBName.ToUpper = "VLQMSDROPSEC" Or gStrDBName.ToUpper = "VLQMSDROPSBEL" Or gStrDBName.ToUpper = "VLQMSDROPSIND2" Then
            chkFIfo.Visible = True
        Else
            chkFIfo.Visible = False
        End If
    End Sub

    Private Sub InitScreen()

        With vsfView
            .Rows.Count = 1
            .Cols.Count = vDetTotCol
            .SetData(0, vDetItmGrp, "Item Group")
            .SetData(0, vDetItmCod, "Item Code")
            .SetData(0, vDetItNam, "Item Name")
            .SetData(0, vDetPurRat, "Purchase Rate")
            .SetData(0, vDetPurVal, "Purchase Value")
            .SetData(0, vDetSalQty, "Sales Qty")
            .SetData(0, vDetSalRat, "Sales Rate")
            .SetData(0, vDetSalVal, "Sales Value")
            .SetData(0, vDetAROEDVal, "AROED Value")
            .SetData(0, vDetPofMrg, "Profit Margin")
            .SetData(0, vDetPofPer, "Profit Percentage")

        End With

    End Sub

    Private Sub btnExportToXL_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExportToXL.Click
        ExportToXL(vsfView,,,, True, Me.Text, "For the period " & getDateFormatInInsert(dtpFrom.Value) & " To " & getDateFormatInInsert(dtpTo.Value))
    End Sub

    Private Sub vsfDet_BeforeEdit(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.RowColEventArgs)
        e.Cancel = True
    End Sub

    'Private Sub vsfView_DoubleClick(sender As Object, e As EventArgs) Handles vsfView.DoubleClick
    '    If vsfView.Row <= 0 Then Exit Sub
    '    Dim frmD As New frmUtlViewExport, strFormat As String, strSQL As String
    '    With frmD
    '        .Text = "Customer wise Listing Of Item " & vsfView.GetData(vsfView.Row, 1)

    '        strFormat = " Customer|  Doc Type| Doc Date| Doc No| Pending Qty"

    '        strSQL = "Select CustCode, DocType, docdate, "
    '        strSQL = strSQL & " Case When docType = 'GRN' then (select GrnNo from TrnHdrGRN h where h.GrnID = t.DocID) else '' end  as docNo, "
    '        strSQL = strSQL & " pendQty"
    '        strSQL = strSQL & " From trnstock t, mstcust c, mstitem i"
    '        strSQL = strSQL & " where t.custid = c.custid"
    '        strSQL = strSQL & " And i.itemid =t.ItemID"
    '        strSQL = strSQL & " and i.ItemCode = '" & vsfView.GetData(vsfView.Row, 1) & "'"
    '        strSQL = strSQL & " order by docDate, 4"

    '        Call .initScreenSQL(True, strFormat, strSQL, False, "", "", "", "", 0, 1, 2)
    '        'Call SetColWidth(.vsfView, 1200, 1050, 1830, 1305, 1230)
    '        .Show()
    '    End With
    'End Sub

    Private Sub frmRptStkLtr_KeyUp(sender As Object, e As KeyEventArgs) Handles Me.KeyUp
        If e.KeyCode = Keys.F9 And e.Control = True Then
            Call initFormSize(Me, Me.Text, True, 958, 579, 0, 0)
            MsgBox("Restored Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
        If e.KeyCode = Keys.F8 And e.Control = True Then
            Call SaveActFormSettings(Me, Me.Text)
            Call SaveActFormVsfDet(Me, Me.Text, "DET", vsfView)
            MsgBox("Saved Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
        If e.Control = False And e.Alt = True And e.KeyCode = Keys.E Then
            Call ExportToXL(vsfView)
        End If

    End Sub

    Private Sub updateValuesWtAvg()
        Dim strsql As String
        Dim sngQty As Double, sngValue As Double
        Dim sngClBalQty As Double, sngClBalValue As Double
        Dim sngRate As Double, sngLastRate As Double
        Dim rs As oledbDataReader
        Dim rsStk As oledbDataReader
        strsql = "Select distinct itemID from rptStkBal  where userName = '" & gUserCode & "' and DocValuationReq = 'Y' order by ItemID"
        rs = clsSVT.getDataReaderFromSQL(strsql)
        With rs
            Do While .Read
                strsql = "Select * from rptStkBal "
                strsql = strsql & " where itemID = " & rs!ItemID
                strsql = strsql & " and UserName = '" & gUserCode & "'"
                strsql = strsql & " order by DocDate, DocOrderBy"
                rsStk = clsSVT.getDataReaderFromSQL(strsql)
                sngQty = 0 : sngValue = 0
                sngClBalQty = 0 : sngClBalValue = 0
                sngRate = 0 : sngLastRate = 0
                With rsStk
                    Do While .Read
                        If !docValuationReq <> "Y" Then
                            sngClBalQty = sngClBalQty + !DocQty
                            sngClBalValue = sngClBalValue + !DocValue
                        Else
                            sngClBalQty = sngClBalQty + !DocQty
                            sngClBalValue = sngClBalValue + Math.Round((!DocQty * sngRate), 2)
                            clsSVT.execSQLStatement("Update rptStkBal set DocValue = " & Math.Round((!DocQty * sngRate), 2) & " where RowID = " & !RowID)
                        End If
                        If sngClBalQty > 0 Then sngRate = Math.Round(sngClBalValue / sngClBalQty, 4)
                        If sngRate = 0 Then sngRate = sngLastRate
                        If sngRate > 0 Then sngLastRate = sngRate
                    Loop
                End With
            Loop
        End With
    End Sub

    Private Sub updateValuesStdRate()
        Dim strsql As String, strRateFld As String
        strRateFld = getGsettings("Inventory", "ItemStdRate", "")
        If strRateFld = "" Then Exit Sub
        strsql = "update rptStkBal set "
        strsql = strsql & " docValue = DocQty * i." & strRateFld
        strsql = strsql & " from mstItem i"
        strsql = strsql & " Where rptStkBal.ItemID = i.ItemID"
        strsql = strsql & " and userName = '" & gUserCode & "'"
        clsSVT.execSQLStatement(strsql)
    End Sub

    Private Sub initGrid()
        Dim rsDummy As oledbDataReader
        Dim blnFound As Boolean
        Dim i As Integer
        With vsfView
            'strTag1 = "Stock Balance Report"
            'strTag2 = "For the Period of {" & dtFdt.value & "} To {" & dtTDt.value & "}"
            vsfView.Cols.Count = 4
            vsfView.Rows.Count = 1
            vsfView.SetData(0, 0, "Item Group")
            vsfView.SetData(0, 1, "Item Code")
            vsfView.Cols(1).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.LeftCenter
            vsfView.Cols(2).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.LeftCenter
            vsfView.SetData(0, 2, "Item Name")
            vsfView.SetData(0, 3, "Item Desc")

            'here you have to add additional flds of item IF ANY

            vsfView.Cols.Count = vsfView.Cols.Count + 1
            vsfView.Col = vsfView.Cols.Count - 1
            vsfView.SetData(0, vsfView.Col, "UOM")
        End With

        rsDummy = clsSVT.getDataReaderFromSQL("Select DocOrderBy, StkBalColName from utlStock order by DocOrderBy")
        With vsfView
            Do While rsDummy.Read
                blnFound = False
                For i = 1 To .Cols.Count - 1
                    If .GetData(0, i) = rsDummy!StkbalColName & " Qty" Then
                        blnFound = True
                        Exit For
                    End If
                Next i
                If Not blnFound Then
                    .Cols.Count = .Cols.Count + 1
                    .SetData(0, .Cols.Count - 1, rsDummy!StkbalColName & " Qty")
                    .Cols.Count = .Cols.Count + 1
                    .SetData(0, .Cols.Count - 1, rsDummy!StkbalColName & " Value")
                End If
            Loop
            .Cols.Count = .Cols.Count + 1
            .SetData(0, .Cols.Count - 1, "Cl Qty")
            .Cols.Count = .Cols.Count + 1
            .SetData(0, .Cols.Count - 1, "Cl Value")
            rsDummy.Close()
        End With
    End Sub

    Private Sub updateValuesFIFO()
        Dim strsql As String, blnFlag As Boolean
        Dim intFactor As Integer, sngPrevRate As Single
        Dim intItemId As Long, i As Integer
        Dim rsDummy As oledbDataReader, rsItem As oledbDataReader
        Dim sngIssQty As Double, sngIssValue As Double, sngBalQty As Double

        strsql = "Select distinct itemid from rptStkBal where userName = '" & gUserCode & "' and DocValuationReq = 'Y' order by ItemID"
        rsItem = clsSVT.getDataReaderFromSQL(strsql)
        With rsItem
            Do While .Read
                StMsg("Valuating for item " & intItemId)
                intItemId = !ItemID
                blnFlag = fillRcptsArr(intItemId)
                sngIssValue = 0
                sngPrevRate = 0
                strsql = "Select * from rptStkBal where itemid = " & !ItemID & " and userName = '" & gUserCode & "' and DocValuationReq = 'Y' order by ItemID, docdate, DocOrderBy, docno"
                rsDummy = clsSVT.getDataReaderFromSQL(strsql)
                With rsDummy
                    Do While .Read
                        sngIssQty = Math.Abs(GetVal(!DocQty, 0))
                        'If sngIssQty <> 0 Then
                        intFactor = sngIssQty / GetVal(!DocQty, 0)
                        'End If
                        If blnFlag Then
                            For i = 0 To UBound(arrRctps, 2)
                                If Val(arrRctps(2, i)) > 0 And CDate(arrRctps(4, i)) <= CDate(!DocDate) Then
                                    sngBalQty = arrRctps(2, i)
                                    If sngIssQty <= sngBalQty Then
                                        sngIssValue = sngIssValue + (sngIssQty * arrRctps(3, i))
                                        sngPrevRate = arrRctps(3, i)
                                        sngBalQty = sngBalQty - sngIssQty
                                        arrRctps(2, i) = sngBalQty
                                        sngIssQty = 0
                                        Exit For
                                    Else
                                        sngIssValue = sngIssValue + (sngBalQty * arrRctps(3, i))
                                        sngPrevRate = arrRctps(3, i)
                                        sngIssQty = sngIssQty - sngBalQty
                                        arrRctps(2, i) = 0
                                    End If
                                End If
                            Next i
                        End If
                        If sngIssQty > 0 Then
                            sngIssValue = sngIssValue + (sngIssQty * sngPrevRate)
                        End If
                        clsSVT.execSQLStatement("Update rptStkBal set DocValue = " & intFactor * Math.Round(sngIssValue, 2) & " where RowID = " & !RowID)
                        sngIssValue = 0
                    Loop
                    .Close()
                End With

            Loop
        End With

        StMsg()
    End Sub

    Private Function fillRcptsArr(argItemID As Long) As Boolean
        Dim intCtr As Integer, strSQL As String
        Dim rsR As oledbDataReader
        Dim sgTot As Double
        strSQL = "Select * from rptStkBal where userName = '" & gUserCode & "'" _
            & " and itemID = " & argItemID _
            & " and docValuationReq <> 'Y' order by docDate"
        rsR = clsSVT.getDataReaderFromSQL(strSQL)
        fillRcptsArr = False
        With rsR
            intCtr = -1
            sgTot = 0
            Do While .Read
                intCtr = intCtr + 1
                fillRcptsArr = True
                ReDim Preserve arrRctps(4, intCtr)
                arrRctps(0, intCtr) = GetVal(!DocQty, 0)
                sgTot = sgTot + GetVal(!DocQty, 0)
                arrRctps(1, intCtr) = GetVal(!DocValue, 0)
                arrRctps(2, intCtr) = arrRctps(0, intCtr)
                If arrRctps(0, intCtr) > 0 Then
                    arrRctps(3, intCtr) = Math.Round(CDbl(arrRctps(1, intCtr) / arrRctps(0, intCtr)), 6)
                Else
                    arrRctps(3, intCtr) = 0
                End If
                arrRctps(4, intCtr) = GetVal(!DocDate, "")
            Loop
        End With
    End Function

    Private Sub txtValBelow_KeyPress(sender As Object, e As KeyPressEventArgs)
        If Not KeyValidate("Integer", e.KeyChar) Then
            e.Handled = True
        End If
    End Sub


    Private Sub chkItemGroup_CheckedChanged(sender As Object, e As EventArgs) Handles chkItemGroup.CheckedChanged

        Dim strSQL As String
        If chkItemGroup.Checked = False Then
            strList = ""
            strItemGroupList = ""
            Exit Sub
        End If

        Dim frmSel As New frmUtlMultiSelection

        strSQL = "Select distinct 0 as sel, ItemGroupID, ItemGroupName from MstItemGroup where ItemGroupID > 0 and "
        strSQL = strSQL & " InActive = 'N' order by ItemGroupName"
        Call frmSel.InitVsfSel(strSQL, 1, 1, True, Me, False)
        frmSel.Text = "Select [Multiple] Item Groups"
        frmSel.vsfDet.SetData(0, 0, "Sel")
        frmSel.ShowDialog()
        strItemGroupList = strList

        If Trim(strItemGroupList) = "" Then chkItemGroup.Checked = False
    End Sub

    Private Sub chkItem_CheckedChanged(sender As Object, e As EventArgs) Handles chkItem.CheckedChanged
        Dim strSQL As String
        If chkItem.Checked = False Then
            strList = ""
            strItemList = ""
            Exit Sub
        End If

        Dim frmSel As New frmUtlMultiSelection

        strSQL = "Select 0 as sel, ItemID, ItemName from MstItem where ItemID > 0 and "
        strSQL = strSQL & " InActive = 'N' order by ItemName"
        Call frmSel.InitVsfSel(strSQL, 1, 1, True, Me, False)
        frmSel.Text = "Select [Multiple] Item"
        frmSel.vsfDet.SetData(0, 0, "Sel")
        frmSel.ShowDialog()
        strItemList = strList
        If Trim(strItemList) = "" Then chkItem.Checked = False
    End Sub

    Private Sub vsfView_BeforeEdit(sender As Object, e As RowColEventArgs) Handles vsfView.BeforeEdit
        e.Cancel = True
    End Sub

    Private Sub fillFifoPM()
        Dim rs As OleDb.OleDbDataReader
        Dim strSQL As String
        Dim intRow As Integer
        Dim cmd As OleDbCommand = New OleDbCommand("msp_getProfitMarginFifo")
        showWaitCursor()

        With cmd
            cmd.Connection = svtConn
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add("@mUserName", OleDbType.VarChar, 50, ParameterDirection.Input)
            .Parameters(0).Value = gUserCode
            .Parameters.Add("@mFdt", OleDbType.DBDate)
            .Parameters(1).Value = dtpFrom.Value
            .Parameters.Add("@mTDt", OleDbType.DBDate)
            .Parameters(2).Value = dtpTo.Value
            .CommandTimeout = 0
            .ExecuteNonQuery()
        End With
        cmd.Dispose()

        strSQL = "Select g.ItemGroupName, ItemCode, ItemName, SalesQty, SalesRate, SalesAmt, PRate,i.AROEDPER"
        strSQL &= " From rptProfitMargin r , Mstitem i, MstItemGroup g "
        strSQL &= " Where r.UserName = '" & gUserCode & "'"
        strSQL &= " And r.ItemID = I.ItemID "
        strSQL &= " And g.Itemgroupid = i.Itemgroupid "
        If chkItemGroup.Checked Then
            If strItemGroupList <> "" Then
                strSQL &= " And i.ItemGroupID In (" & strItemGroupList & ") "
            End If
        End If
        If chkItem.Checked Then
            If strItemList <> "" Then
                strSQL &= " And i.ItemID In (" & strItemList & ") "
            End If
        End If
        strSQL &= " Order by 1,3 "

        rs = clsSVT.getDataReaderFromSQL(strSQL)
        With vsfView
            .Rows.Count = 1
            If rs.HasRows Then
                Do While rs.Read
                    .Rows.Count = .Rows.Count + 1
                    intRow = .Rows.Count - 1
                    .SetData(intRow, vDetItmGrp, rs("ItemGroupName"))
                    .SetData(intRow, vDetItmCod, rs("ItemCode"))
                    .SetData(intRow, vDetItNam, rs("Itemname"))
                    .SetData(intRow, vDetPurRat, rs!pRate)
                    .SetData(intRow, vDetSalRat, rs!SalesRate)
                    .SetData(intRow, vDetPurVal, Math.Round(GetVal(rs!Prate, 0) * GetVal(rs("SalesQty"), 0), 2))
                    .SetData(intRow, vDetSalQty, rs("SalesQty"))
                    .SetData(intRow, vDetSalVal, rs("SalesAmt"))
                    .SetData(intRow, vDetAROEDVal, .GetData(.Rows.Count - 1, vDetSalVal) - rs("aroedper"))
                    '.SetData(.Rows.Count - 1, vDetPofMrg, Math.Round(.GetData(.Rows.Count - 1, vDetSalVal) - .GetData(.Rows.Count - 1, vDetPurVal), 2))
                    .SetData(.Rows.Count - 1, vDetPofMrg, Math.Round(.GetData(.Rows.Count - 1, vDetSalVal) - .GetData(.Rows.Count - 1, vDetPurVal), 2))
                    If Val(.GetData(.Rows.Count - 1, vDetPurVal)) > 0 Then
                        .SetData(.Rows.Count - 1, vDetPofPer, Math.Round(.GetData(.Rows.Count - 1, vDetPofMrg) / .GetData(.Rows.Count - 1, vDetPurVal) * 100, 2) & "%")
                    End If
                Loop
            End If
            rs.Close()
            With vsfView
                .Cols(vDetSalQty).Format = "#"
                .Cols(vDetSalRat).Format = "#.00"
                .AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.RestrictAll
                .Cols(vDetItmGrp).AllowMerging = True

                .SubtotalPosition = C1.Win.C1FlexGrid.SubtotalPositionEnum.BelowData


                .Subtotal(C1.Win.C1FlexGrid.AggregateEnum.Sum, 0, vDetItmGrp, vDetPurVal)
                .Subtotal(C1.Win.C1FlexGrid.AggregateEnum.Sum, 0, vDetItmGrp, vDetSalVal)
                .Subtotal(C1.Win.C1FlexGrid.AggregateEnum.Sum, 0, vDetItmGrp, vDetPofMrg)

                .Subtotal(C1.Win.C1FlexGrid.AggregateEnum.Sum, 1, -1, vDetPurVal)
                .Subtotal(C1.Win.C1FlexGrid.AggregateEnum.Sum, 1, -1, vDetSalVal)
                .Subtotal(C1.Win.C1FlexGrid.AggregateEnum.Sum, 1, -1, vDetAROEDVal)
                .Subtotal(C1.Win.C1FlexGrid.AggregateEnum.Sum, 1, -1, vDetPofMrg)

                .AutoSizeCols()
            End With
            showNormalCursor()
        End With
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