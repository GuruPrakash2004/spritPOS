Imports System.Data.oledb

Public Class frmRptSPurPkgwise
    Dim clsSVT As New svtLibrary

    Public strList As String
    Dim strItemGroupList As String
    Dim strItemList As String
    Dim strValMethod As String
    Dim arrRctps(4, 1) As String
    Dim arrFlds() As String
    Dim intAdnlFldCount As String
    Public strRptName As String = ""

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Me.Close()
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
        ValidateFlds = True
    End Function

    Private Sub btnView_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        If Not ValidateFlds() Then
            Exit Sub
        End If

        strValMethod = getGsettings("Inventory", "VaulationMethod", "S")
        Try
            Using cmd As New oledbCommand("mSp_stkBal")
                cmd.Connection = svtConn
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.AddWithValue("@mOPdt", CDate(gSettings.gCompStDt))
                cmd.Parameters.AddWithValue("@mFdt", dtpFrom.Value)
                cmd.Parameters.AddWithValue("@mTdt", dtpTo.Value)
                cmd.Parameters.AddWithValue("@mUserName", gUserCode)
                cmd.ExecuteScalar()
            End Using
        Catch ex As Exception
            MsgBox("Error during processing stock" & vbCrLf & ex.Message, MsgBoxStyle.Critical, "Stock report error")
            Exit Sub
        End Try
        Call initGrid()
        Call RetreiveData()
    End Sub

    Private Sub RetreiveData()
        Dim strsql As String, iCount As Integer
        Dim rs As oledbDataReader
        Dim sngClQty As Double, sngClVal As Double
        Dim intItemId As Integer, i As Integer

        strsql = "update rptStkbal set stkBalColName = 'OP Bal',  DocDate = " & getDateFormatInCond(dtpFrom.Value) & ", docOrderBy = 1 "
        strsql = strsql & " where docDate < " & getDateFormatInCond(dtpFrom.Value) & ""
        clsSVT.execSQLStatement(strsql)

        strsql = "select i.itemID, ItemCode, ItemDesc, itemName, i.UomID "

        strsql = strsql & " ,UOMCode, ItemGroupName, stkBalColName, "
        strsql = strsql & " IsNull(sum(docQty),0) as qtyDoc, IsNull(sum(DocValue),0) as valDoc"
        strsql = strsql & " From rptStkBal r, mstItem i, MstItemGroup g, MstUOM u"
        strsql = strsql & " where userName = '" & gUserCode & "' and "
        strsql = strsql & " r.ItemID = i.ItemID and"
        strsql = strsql & " i.NonStkable<>'Y' and "
        strsql = strsql & " u.UomID = i.UomID and "
        'strsql = strsql & " u.UomID = i.UomID and "
        strsql = strsql & " g.ItemGroupID = i.ItemGroupID "

        ' strsql = strsql & " Having IsNull(sum(docQty),0) > 0"
        strsql = strsql & " Union"
        strsql = strsql & " select i.itemID, ItemCode, ItemDesc, itemName, i.UomID  "
        strsql = strsql & " ,UOMCode, ItemGroupName, '', 0, 0"
        strsql = strsql & " From mstItem i, MstItemGroup g, MstUOM u"
        strsql = strsql & " where i.NonStkable<>'Y' and  u.UomID = i.UomID and  g.ItemGroupID = i.ItemGroupID"
        strsql = strsql & " and itemid not in (select itemid from rptstkbal where username = '" & gUserCode & "')"
        strsql = strsql & " and itemid > 0"


        strsql = strsql & " Order by itemGroupName, i.ItemID "
        rs = clsSVT.getDataReaderFromSQL(strsql)
        If Not rs.HasRows Then
            rs.Close()
            Exit Sub
        End If

        intItemId = -1
        With rs
            'Dim iFld As Integer
            Do While .Read
                If intItemId <> !ItemID Then
                    sngClQty = 0 : sngClVal = 0
                    vsfView.Rows.Count = vsfView.Rows.Count + 1
                    vsfView.SetData(vsfView.Rows.Count - 1, 0, !ItemGroupName)
                    vsfView.SetData(vsfView.Rows.Count - 1, 1, !ItemCode)
                    vsfView.SetData(vsfView.Rows.Count - 1, 2, !ItemName)
                    vsfView.SetData(vsfView.Rows.Count - 1, 3, GetVal(!ItemDesc, ""))
                End If
                intItemId = !itemid
                iCount = 3
                'here you have to add additional flds of item IF ANY

                iCount = iCount + 1
                vsfView.SetData(vsfView.Rows.Count - 1, iCount, !Uomcode)
                For i = iCount + 1 To (vsfView.Cols.Count - 1)
                    If !StkbalColName & " Qty" = vsfView.GetData(0, i) Then
                        If !StkbalColName = "OP Bal" Then
                            vsfView.SetData(vsfView.Rows.Count - 1, i, !QtyDoc)
                            vsfView.SetData(vsfView.Rows.Count - 1, i + 1, GetVal(!ValDoc, 0))
                        Else
                            vsfView.SetData(vsfView.Rows.Count - 1, i, Math.Abs(Val(!QtyDoc)))
                            vsfView.SetData(vsfView.Rows.Count - 1, i + 1, GetVal(Math.Abs(GetVal(rs!ValDoc, 0)), 0))
                        End If
                        Exit For
                    End If
                Next i
                sngClQty = sngClQty + !QtyDoc
                sngClVal = sngClVal + Math.Round(GetVal(!ValDoc, 0), 2)
                vsfView.SetData(vsfView.Rows.Count - 1, vsfView.Cols.Count - 2, Math.Round(sngClQty, 2))
                vsfView.SetData(vsfView.Rows.Count - 1, vsfView.Cols.Count - 1, Math.Round(sngClVal, 2))
                intItemId = !ItemID
            Loop
            'vsfView.ColFormat(vsfView.Cols - 1) = "#.00"
            'vsfView.ColFormat(vsfView.Cols - 2) = "#.00"
            .Close()
        End With

        Dim k As Integer
        With vsfView
            .SubtotalPosition = C1.Win.C1FlexGrid.SubtotalPositionEnum.BelowData
            For k = iCount + 1 To .Cols.Count - 1
                .Subtotal(C1.Win.C1FlexGrid.AggregateEnum.Sum, 0, -1, k)
            Next k
            .AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.RestrictAll
            .Cols(0).AllowMerging = True
        End With
    End Sub

    Private Sub frmRptStkLtr_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.KeyPreview = True
        strRptName = "RPTPKGPR"
        Call initScreen()
        ' Call initFormSize(Me, Me.Text, False, 958, 579, 0, 0)
        Call initFormVsfDet(Me, Me.Text, vsfView, "DET")
        Call fillFormProperties(Me)
        Call initGridStyles(Me)
    End Sub

    Private Sub initScreen()
        ' dtpFrom.Value = getFirstDateInMonth(getSystemDate)
        dtpFrom.Value = gSettings.gCompStDt
        dtpTo.Value = getSystemDate()
    End Sub

    Private Sub btnExportToXL_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Call ExportToXL(vsfView)
    End Sub

    Private Sub vsfDet_BeforeEdit(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.RowColEventArgs)
        e.Cancel = True
    End Sub

    'Private Sub vsfView_DoubleClick(sender As Object, e As EventArgs) Handles vsfView.DoubleClick
    '    If vsfView.Row <= 0 Then Exit Sub
    '    Dim frmD As New frmUtlViewExport, strFormat As String, strSQL As String
    '    With frmD
    '        .Text = "Customer wise Listing of Item " & vsfView.GetData(vsfView.Row, 1)

    '        strFormat = " Customer|  Doc Type| Doc Date| Doc No| Pending Qty"

    '        strSQL = "Select CustCode, DocType, docdate, "
    '        strSQL = strSQL & " case when docType = 'GRN' then (select GrnNo from TrnHdrGRN h where h.GrnID = t.DocID) else '' end  as docNo, "
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

    Private Sub frmRptStkLtr_KeyUp(sender As Object, e As KeyEventArgs) Handles MyBase.KeyUp
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

        strSQL = "Select distinct 0 as sel, ItemGroupID, ItemGroupCode from MstItemGroup where ItemGroupID > 0 and "
        strSQL = strSQL & " InActive = 'N' order by ItemGroupCode"
        Call frmSel.InitVsfSel(strSQL, 1, 1, True, Me, False)
        frmSel.Text = "Select [Multiple] Item Groups"
        frmSel.vsfDet.SetData(0, 0, "Sel")
        frmSel.ShowDialog()
        strItemGroupList = strList

        If Trim(strItemGroupList) = "" Then chkItemGroup.Checked = False
    End Sub

    Private Sub btnPrint_Click(sender As Object, e As EventArgs) Handles btnPrint.Click
        Dim strCond As String = ""
        Dim strFormulaFld As String = "", strFormulaVal As String = "", strSelFormula As String = ""
        Dim strPkgIDlist As String = ""

        strCond = "{TrnHdrPB.PBDate} >= " & getCrptDateFmt(dtpFrom.Value) & " AND {TrnHdrPB.PBDate} <= " & getCrptDateFmt(dtpTo.Value)

        If strItemGroupList <> "" Then
            strCond = strCond & " and {MstItem.ItemGroupID} in [" & strItemGroupList & "]"
        End If

        strSelFormula = strCond

        Dim frmv As New frmUtlReportViewer
        With frmv
            .StartPosition = FormStartPosition.CenterScreen
            strFormulaFld = "mrptheader"
            strFormulaVal = Me.Text
            .ShowRptFile(Me.Text, "RptPurchasePkg.rpt", strFormulaFld, strFormulaVal, strSelFormula)
            .Show()
        End With
    End Sub
End Class