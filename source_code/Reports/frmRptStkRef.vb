Imports System.ComponentModel
Imports System.Data.oledb
Imports C1.Win.C1FlexGrid

Public Class frmRptStkRef
    Private Sub frmRptStkRef_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub
    'Public strList As String
    'Dim strItemGroupList As String
    'Dim strSuppList As String
    'Dim strCustList As String
    'Dim strItemList As String

    'Dim strSQL As String
    'Dim intItemID As Integer

    'Public strRptName As String = ""
    'Dim blnProcedure As Boolean = False
    'Dim clsSVT As New svtLibrary
    'Dim strHeading As String = ""

    'Dim dtSearch As DataTable
    'Dim dvSearch As DataView

    'Const vTrnID = 0
    'Const vTrnName = 1
    'Const vTrnValue = 2

    'Private Sub btnClose_Click(sender As System.Object, e As System.EventArgs) Handles btnClose.Click
    '    Dim tb As New C1.Win.C1Command.C1DockingTabPage()
    '    Dim dockTabMain As New C1.Win.C1Command.C1DockingTab()
    '    Me.Dispose()
    '    tb.Controls.Clear()
    '    'frmMain.dockTabMain.SelectedTab.Dispose()
    'End Sub


    'Private Sub vwStockDtls()
    '    Dim strSQL As String = ""

    '    If Trim(txtItemCode.Text) <> "" Then
    '        intItemID = clsSVT.getDataLongValue("Select ItemID from MstItem where ItemCode = '" & Trim(txtItemCode.Text) & "'", 0, False)
    '    End If

    '    strHeading = ""
    '    Dim dtSearch As DataTable, dvSearch As DataView
    '    Application.DoEvents()

    '    Call fillTransactions()
    '    Call showNormalCursor()
    'End Sub

    'Private Sub frmRptPendCPO_KeyUp(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyUp
    '    If e.KeyCode = Keys.F9 And e.Control Then
    '        '958, 579
    '        Call initFormSize(Me, strRptName, True, 958, 579, 0, 0)
    '        MsgBox("Restored Form & Grid Layout Successfully!!", vbInformation, Me.Text)
    '    End If
    '    If e.KeyCode = Keys.F8 And e.Control = True Then
    '        Call SaveActFormSettings(Me, Me.Text)
    '        MsgBox("Saved Form & Grid Layout Successfully!!", vbInformation, Me.Text)
    '    End If
    'End Sub

    'Private Sub frmRptPendCPO_Load(sender As Object, e As System.EventArgs) Handles Me.Load
    '    Call initFormSize(Me, strRptName, False, 577, 349, 0, 0)
    '    Me.Text = strRptName
    '    vsfView.Visible = False
    '    'Call LoadHeadings()
    '    Call fillFormProperties(Me)
    '    cboTrn.Enabled = False
    '    'Call bindCombo(cboTrn, "DocName", "DocName", "Select DocName from UtlTransactions where Doctype='Transaction' and DocID not in(100,1121) order by DocName")
    '    txtNos.Text = 5
    '    dvSearch = New DataView(dtItems)
    '    vsfView.DataSource = dvSearch
    '    vsfView.Refresh()
    '    vsfView.ScrollBars = ScrollBars.Both
    '    vsfView.AutoSizeCols()
    '    initGridStyles(Me)
    'End Sub

    'Public Sub disableall()
    '    chkDate.Visible = False
    '    pnlDate.Visible = False
    '    lblItem.Visible = False
    '    txtItemCode.Visible = False

    'End Sub

    'Public Sub enableall()
    '    chkDate.Visible = True
    '    pnlDate.Visible = True
    '    lblItem.Visible = True
    '    txtItemCode.Visible = True

    'End Sub


    'Private Sub HideIDCols()
    '    ' Dim strSave
    '    With vsfStk
    '        Dim strCol As String = ""
    '        If .Cols.Count >= 1 Then
    '            For i As Int32 = 0 To .Cols.Count - 1
    '                strCol = GetVal(.GetData(3, i), "")
    '                If strCol <> "" Then
    '                    If UCase(Mid(strCol, Len(strCol) - 1, 2)) = "ID" Then
    '                        .Cols(i).Visible = False
    '                    End If
    '                End If
    '            Next
    '        End If
    '    End With
    'End Sub

    'Private Sub initVsfForHdrs(ByVal argHdrLine1 As String, ByVal argHdrLine2 As String)
    '    Dim i As Integer

    '    With vsfStk
    '        For i = 0 To .Cols.Count - 1
    '            .SetData(3, i, .GetData(0, i))

    '            .SetData(0, i, gSettings.gCompName & ",  " & gSettings.gCompAdd1 & gSettings.gCompAdd2 & gSettings.gCompAdd3 &
    '                      gSettings.gCompAdd4 & gSettings.gCompAdd5)
    '            .SetData(1, i, argHdrLine1)
    '            .SetData(2, i, argHdrLine2)
    '        Next
    '        Dim csf1 As C1.Win.C1FlexGrid.CellStyle = vsfStk.Styles.Add("F1")
    '        csf1.BackColor = Color.Beige

    '        .Rows(0).Style = csf1
    '        .Rows(1).Style = csf1
    '        .Rows(2).Style = csf1

    '        If argHdrLine1 = "" Then
    '            .Rows(1).Visible = False
    '        Else
    '            .Rows(1).Visible = True
    '        End If

    '        If argHdrLine2 = "" Then
    '            .Rows(2).Visible = False
    '        Else
    '            .Rows(2).Visible = True
    '        End If

    '        .Rows(0).AllowMerging = True
    '        .Rows(1).AllowMerging = True
    '        .Rows(2).AllowMerging = True
    '        .Rows(3).AllowMerging = True

    '    End With
    'End Sub

    'Private Sub txtItemCode_KeyUp(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles txtItemCode.KeyUp
    '    'If e.KeyCode = Keys.Escape Then
    '    '    vsfView.Visible = False
    '    '    txtItemCode.Focus()
    '    'End If

    '    'If e.KeyCode = Keys.Enter Or e.KeyCode = Keys.Return Then
    '    '    If vsfView.Visible Then
    '    '        If vsfView.Rows.Count > 1 Then
    '    '            txtItemCode.Text = vsfView.GetData(vsfView.Row, 1)
    '    '        End If
    '    '    End If
    '    'End If
    '    'If e.KeyCode = Keys.Down Then
    '    '    If vsfView.Visible Then
    '    '        If vsfView.Rows.Count > 1 Then
    '    '            vsfView.Focus()
    '    '            vsfView.Col = 1
    '    '            vsfView.Row = 1
    '    '            vsfView.TopRow = 1
    '    '        End If
    '    '    End If
    '    'End If
    '    Dim StrQry As String, strCond As String = ""

    '    If e.Alt = Keys.Tab Then
    '        Exit Sub
    '    End If

    '    If e.KeyCode = Keys.Enter Then
    '        If Trim(txtItemCode.Text) <> "" And vsfView.Rows.Count > 1 Then
    '            Call vsfSearchSelectedItem()
    '            Exit Sub
    '        End If
    '    End If

    '    If e.KeyCode = Keys.Down Then
    '        vsfView.Focus()
    '        Exit Sub
    '    End If

    '    If e.KeyCode = Keys.Escape Then
    '        txtItemCode.Text = ""
    '        Call setSearchGridHt("")
    '        Exit Sub
    '    End If

    '    If Len(txtItemCode.Text) < 1 Then
    '        Call setSearchGridHt("")
    '        Exit Sub
    '    End If

    '    Call setSearchGridHt("FULL")

    '    StrQry = ""
    '    With vsfView
    '        Select Case gDBTypeCurrent
    '            Case gDbTypeAccess
    '                'StrQry = StrQry & strFldItemCode & " like '*" & .Editor.Text & "*' "
    '                'StrQry = StrQry & strFldItemName & " like '*" & .Editor.Text & "*'"
    '            Case Else
    '                If txtItemCode.Text = " " Then
    '                    dvSearch.RowFilter = "1 = 1"
    '                    Exit Sub
    '                Else
    '                    StrQry = StrQry & " ItemCode like '%" & txtItemCode.Text & "%'" & " Or "
    '                    StrQry = StrQry & "ItemName" & " Like '%" & txtItemCode.Text & "%'"
    '                End If
    '        End Select
    '        dvSearch.RowFilter = StrQry
    '    End With
    'End Sub

    'Private Sub vsfView_BeforeEdit(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfView.BeforeEdit
    '    e.Cancel = True
    'End Sub

    'Private Sub vsfView_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles vsfView.KeyUp
    '    If e.KeyCode = Keys.Escape Then
    '        txtItemCode.Focus()
    '        vsfView.Visible = False
    '    End If
    '    If vsfView.Row > 0 Then
    '        If e.KeyCode = Keys.Return Or e.KeyCode = Keys.Tab Then
    '            vsfSearchSelectedItem()
    '            vsfView.Visible = False
    '        End If
    '    End If

    'End Sub

    'Private Sub vsfView_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles vsfView.DoubleClick
    '    If vsfView.Row > 0 Then
    '        Call FillItemDtls(vsfView.GetData(vsfView.Row, 0))
    '        vsfView.Visible = False
    '    End If
    'End Sub

    'Private Sub txtNos_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtNos.KeyPress
    '    If Not KeyValidate("Integer", e.KeyChar) Then
    '        e.Handled = True
    '    End If
    'End Sub

    'Private Sub vsfTrn_BeforeEdit(sender As Object, e As RowColEventArgs) Handles vsfTrn.BeforeEdit
    '    e.Cancel = True
    'End Sub

    'Private Sub vsfDet_BeforeEdit(sender As Object, e As RowColEventArgs) Handles vsfDet.BeforeEdit
    '    e.Cancel = True
    'End Sub

    'Private Sub vsfStk_BeforeEdit(sender As Object, e As RowColEventArgs) Handles vsfStk.BeforeEdit
    '    e.Cancel = True
    'End Sub

    'Private Sub fillTransactions(ByVal argIntCompID As Integer)
    '    Dim rs As oledb.oledbDataReader = Nothing
    '    With vsfTrn
    '        .Rows.Count = 1
    '        .Cols.Count = 3
    '        .SetData(0, vTrnName, "Document")
    '        .SetData(0, vTrnValue, "Doc-Info")
    '        .Cols(vTrnID).Visible = False

    '        strSQL = "Select DocId,DocName from UtlTransactions where Doctype='Transaction' and DocID not in(100,1121) order by DocName"
    '        rs = clsSVT.getDataReaderFromSQL(strSQL)
    '        If Not rs.HasRows Then
    '            .Rows.Count = 1
    '            Exit Sub
    '        End If
    '        Do While rs.Read
    '            .Rows.Count = .Rows.Count + 1
    '            .SetData(.Rows.Count - 1, vTrnID, rs!DocId)
    '            .SetData(.Rows.Count - 1, vTrnName, rs!DocName)
    '        Loop
    '        .AutoSizeCols()
    '        rs.Close()
    '    End With


    '    Dim i As Integer = 0
    '    Dim strValue As String
    '    For i = 1 To vsfTrn.Rows.Count - 1
    '        strSQL = ""
    '        Select Case UCase(vsfTrn.GetData(i, vTrnName))
    '            Case "PURCHASE ORDER"
    '                strSQL = "Select top 1 'NO: ' + PONo + ', ' + convert(varchar(20),PODate,103) + ', ' + SuppCode + ', Qty: ' + convert(varchar(15),POQty) + ', Rate: ' + Convert(varchar(15),PORate)"
    '                strSQL &= " from TrnHdrPO h, TrnDtlPO d, MstSupp s"
    '                strSQL &= " where h.POID = d.POID"
    '                strSQL &= " And s.SuppID=h.SuppID"
    '                strSQL &= " and ItemID = " & intItemID
    '                If UCase(cboComp.Text) <> "ALL" Then
    '                    strSQL &= " And CompID = " & argIntCompID
    '                End If
    '                strSQL &= " order by PONO desc "

    '            Case "SALES ORDER"
    '                strSQL = "Select top 1 'NO: ' + SONo + ', ' + convert(varchar(20),SODate,103) + ', ' + CustCode + ', Qty: ' + convert(varchar(15),SOQty) + ', Rate: ' + Convert(varchar(15),SORate)"
    '                strSQL &= " from TrnHdrSO h, TrnDtlSO d, MstCust c"
    '                strSQL &= " where h.SOID = d.SOID"
    '                strSQL &= " And c.CustID=h.CustID"
    '                strSQL &= " and ItemID = " & intItemID
    '                If UCase(cboComp.Text) <> "ALL" Then
    '                    strSQL &= " And CompID = " & argIntCompID
    '                End If
    '                strSQL &= " order by SONO desc "

    '            Case "SALES INVOICE"
    '                strSQL = "Select top 1 'NO: ' + InvNo + ', ' + convert(varchar(20),InvDate,103) + ', ' + CustCode + ', Qty: ' + convert(varchar(15),InvQty) + ', Rate: ' + Convert(varchar(15),InvRate)"
    '                strSQL &= " from TrnHdrInv h, TrnDtlInv d, MstCust c"
    '                strSQL &= " where h.InvID = d.InvID"
    '                strSQL &= " And c.CustID=h.CustID"
    '                strSQL &= " and ItemID = " & intItemID
    '                If UCase(cboComp.Text) <> "ALL" Then
    '                    strSQL &= " And CompID = " & argIntCompID
    '                End If
    '                strSQL &= " order by InvNO desc "


    '            Case "GOODS RECEIVED NOTE"
    '                strSQL = "Select top 1 'NO: ' + GRNNo + ', ' + convert(varchar(20),GRNDate,103) + ', ' + SuppCode + ', Qty: ' + convert(varchar(15),GRNQty) + ', Rate: ' + Convert(varchar(15),GRNRate)"
    '                strSQL &= " from TrnHdrGRN h, TrnDtlGRN d, MstSupp s"
    '                strSQL &= " where h.GRNID = d.GRNID"
    '                strSQL &= " And s.SuppID=h.SuppID"
    '                strSQL &= " and ItemID = " & intItemID
    '                If UCase(cboComp.Text) <> "ALL" Then
    '                    strSQL &= " And CompID = " & argIntCompID
    '                End If
    '                strSQL &= " order by GRNNO desc "


    '            Case "LOCAL QUOTATION"
    '                strSQL = "Select top 1 'NO: ' + LQTNNo + ', ' + convert(varchar(20),LQTNDate,103) + ', ' + CustCode + ', Qty: ' + convert(varchar(15),LQTNQty) + ', Rate: ' + Convert(varchar(15),LQTNRate)"
    '                strSQL &= " from TrnHdrLQTN h, TrnDtlLQTN d, MstCust c"
    '                strSQL &= " where h.LQTNID = d.LQTNID"
    '                strSQL &= " And c.CustID=h.CustID"
    '                strSQL &= " and ItemID = " & intItemID
    '                If UCase(cboComp.Text) <> "ALL" Then
    '                    strSQL &= " And CompID = " & argIntCompID
    '                End If
    '                strSQL &= " order by LQTNNO desc "

    '            Case "IMPORT QUOTATION"
    '                strSQL = "Select top 1 'NO: ' + IQTNNo + ', ' + convert(varchar(20),IQTNDate,103) + ', ' + CustCode + ', Qty: ' + convert(varchar(15),IQTNQty) + ', Rate: ' + Convert(varchar(15),IQTNRate)"
    '                strSQL &= " from TrnHdrIQTN h, TrnDtlIQTN d, MstCust c"
    '                strSQL &= " where h.IQTNID = d.IQTNID"
    '                strSQL &= " And c.CustID=h.CustID"
    '                strSQL &= " and ItemID = " & intItemID
    '                If UCase(cboComp.Text) <> "ALL" Then
    '                    strSQL &= " And CompID = " & argIntCompID
    '                End If
    '                strSQL &= " order by IQTNNO desc "

    '            Case "PROFORMA INVOICE"
    '                strSQL = "Select top 1 'NO: ' + PINVNo + ', ' + convert(varchar(20),PINVDate,103) + ', ' + CustCode + ', Qty: ' + convert(varchar(15),PINVQty) + ', Rate: ' + Convert(varchar(15),PINVRate)"
    '                strSQL &= " from TrnHdrPINV h, TrnDtlPINV d, MstCust c"
    '                strSQL &= " where h.PINVID = d.PINVID"
    '                strSQL &= " And c.CustID=h.CustID"
    '                strSQL &= " and ItemID = " & intItemID
    '                If UCase(cboComp.Text) <> "ALL" Then
    '                    strSQL &= " And CompID = " & argIntCompID
    '                End If
    '                strSQL &= " order by PINVNO desc "

    '            Case "SAMPLE SUBMISSION"
    '                strSQL = "Select top 1 'NO: ' + SSUBNo + ', ' + convert(varchar(20),SSUBDate,103) + ', ' + CustCode + ', Qty: ' + convert(varchar(15),Qty)"
    '                strSQL &= " from TrnHdrSSUB h, TrnDtlSSUB d, MstCust c"
    '                strSQL &= " where h.SSUBID = d.SSUBID"
    '                strSQL &= " And c.CustID=h.CustID"
    '                strSQL &= " and ItemID = " & intItemID
    '                If UCase(cboComp.Text) <> "ALL" Then
    '                    strSQL &= " And CompID = " & argIntCompID
    '                End If
    '                strSQL &= " order by SSUBNO desc "

    '            Case "SALES RETURN"
    '                strSQL = "Select top 1 'NO: ' + SRNo + ', ' + convert(varchar(20),SRDate,103) + ', ' + CustCode + ', Qty: ' + convert(varchar(15),SRQty)"
    '                strSQL &= " from TrnHdrSR h, TrnDtlSR d, MstCust c"
    '                strSQL &= " where h.SRID = d.SRID"
    '                strSQL &= " And c.CustID=h.CustID"
    '                strSQL &= " and ItemID = " & intItemID
    '                If UCase(cboComp.Text) <> "ALL" Then
    '                    strSQL &= " And CompID = " & argIntCompID
    '                End If
    '                strSQL &= " order by SRNO desc "

    '            Case "CUSTOMER ORDER"
    '                strSQL = "Select top 1 'NO: ' + CustPONo + ', ' + convert(varchar(20),CustPODt,103) + ', ' + CustCode + ', Qty: ' + convert(varchar(15),CPOQty)"
    '                strSQL &= " from TrnHdrCPO h, TrnDtlCPO d, MstCust c"
    '                strSQL &= " where h.CPOID = d.CPOID"
    '                strSQL &= " And c.CustID=h.CustID"
    '                strSQL &= " and ItemID = " & intItemID
    '                If UCase(cboComp.Text) <> "ALL" Then
    '                    strSQL &= " And CompID = " & argIntCompID
    '                End If
    '                strSQL &= " order by CPONO desc "

    '            Case "SHIPMENT ENTRY CPO"
    '                strSQL = "Select top 1 'NO: ' + CSHPNo + ', ' + convert(varchar(20),CSHPDate,103) + ', ' + CustCode + ', Qty: ' + convert(varchar(15),CSHPQty)"
    '                strSQL &= " from TrnHdrCSHP h, TrnDtlCSHP d, MstCust c"
    '                strSQL &= " where h.CSHPID = d.CSHPID"
    '                strSQL &= " And c.CustID=h.CustID"
    '                strSQL &= " and ItemID = " & intItemID
    '                If UCase(cboComp.Text) <> "ALL" Then
    '                    strSQL &= " And CompID = " & argIntCompID
    '                End If
    '                strSQL &= " order by CSHPNO desc "

    '            Case "DELIVERY CHALLAN"
    '                strSQL = "Select top 1 'NO: ' + DCNo + ', ' + convert(varchar(20),DCDate,103) + ', ' + PartyName + ', Qty: ' + convert(varchar(15),DCQty)"
    '                strSQL &= " from TrnHdrDC h, TrnDtlDC d"
    '                strSQL &= " where h.DCID = d.DCID"
    '                strSQL &= " and ItemID = " & intItemID
    '                If UCase(cboComp.Text) <> "ALL" Then
    '                    strSQL &= " And CompID = " & argIntCompID
    '                End If
    '                strSQL &= " order by DCNO desc "

    '            Case "DELIVERY CHALLAN RECEIPT"
    '                strSQL = "Select top 1 'NO: ' + DCRNo + ', ' + convert(varchar(20),DCRDate,103) + ', ' + PartyName + ', Qty: ' + convert(varchar(15),DCRQty)"
    '                strSQL &= " from TrnHdrDCR h, TrnDtlDCR d"
    '                strSQL &= " where h.DCRID = d.DCRID"
    '                strSQL &= " and ItemID = " & intItemID
    '                If UCase(cboComp.Text) <> "ALL" Then
    '                    strSQL &= " And CompID = " & argIntCompID
    '                End If
    '                strSQL &= " order by DCRNO desc "

    '            Case "DIVERSION PO"
    '                strSQL = "Select top 1 'NO: ' + DPONo + ', ' + convert(varchar(20),DPODate,103) + ', ' + SuppCode + ', Qty: ' + convert(varchar(15),DPOQty) + ', Rate: ' + Convert(varchar(15),DPORate)"
    '                strSQL &= " from TrnHdrDPO h, TrnDtlDPO d, MstSupp s"
    '                strSQL &= " where h.DPOID = d.DPOID"
    '                strSQL &= " And s.SuppID=h.SuppID"
    '                strSQL &= " and ItemID = " & intItemID
    '                If UCase(cboComp.Text) <> "ALL" Then
    '                    strSQL &= " And CompID = " & argIntCompID
    '                End If
    '                strSQL &= " order by DPONO desc "

    '            Case "DIV PO SHIPENT ENTRY"
    '                strSQL = "Select top 1 'NO: ' + SEPONo + ', ' + convert(varchar(20),SEPODate,103) + ', ' + SuppCode + ', Qty: ' + convert(varchar(15),SEPOQty) + ', Rate: ' + Convert(varchar(15),SEPORate)"
    '                strSQL &= " from TrnHdrSEPO h, TrnDtlSEPO d, MstSupp s"
    '                strSQL &= " where h.SEPOID = d.SEPOID"
    '                strSQL &= " And s.SuppID=h.SuppID"
    '                strSQL &= " and ItemID = " & intItemID
    '                If UCase(cboComp.Text) <> "ALL" Then
    '                    strSQL &= " And CompID = " & argIntCompID
    '                End If
    '                strSQL &= " order by SEPONO desc "

    '            Case "STOCK ADDITION AND DEDUCTION"
    '                strSQL = "Select top 1 'NO: ' + STKADJNo + ', ' + convert(varchar(20),STKADJDate,103) + ', ' + CustCode + ', Qty: ' + convert(varchar(15),Qty) "
    '                strSQL &= " from TrnHdrSTKADJ h, TrnDtlSTKADJ d, MstCust c"
    '                strSQL &= " where h.STKADJID = d.STKADJID"
    '                strSQL &= " And c.CustID=h.CustID"
    '                strSQL &= " and ItemID = " & intItemID
    '                If UCase(cboComp.Text) <> "ALL" Then
    '                    strSQL &= " And CompID = " & argIntCompID
    '                End If
    '                strSQL &= " order by STKADJNO desc "

    '        End Select
    '        If strSQL <> "" Then
    '            strValue = clsSVT.getDataStringValue(strSQL, "", False)
    '            vsfTrn.SetData(i, vTrnValue, strValue)
    '        End If
    '    Next
    '    cboTrn.Items.Clear()

    '    With vsfTrn
    '        For i = 1 To .Rows.Count - 1
    '            .Rows(i).Visible = True
    '            If .GetData(i, vTrnValue) = "" Then
    '                .Rows(i).Visible = False
    '            Else
    '                cboTrn.Items.Add(.GetData(i, vTrnName))
    '            End If
    '        Next
    '    End With
    '    vsfTrn.AutoSizeCols()
    '    If cboTrn.Items.Count >= 1 Then
    '        cboTrn.Enabled = True
    '    Else
    '        cboTrn.Enabled = False
    '    End If
    'End Sub

    'Private Sub txtItemCode_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtItemCode.KeyPress
    '    If e.KeyChar = "'" Then
    '        e.Handled = True
    '    End If
    'End Sub

    'Private Sub txtItemCode_LostFocus(sender As Object, e As EventArgs) Handles txtItemCode.LostFocus
    '    If vsfView.Focused Then
    '        Exit Sub
    '    Else
    '        Call setSearchGridHt("")
    '    End If
    'End Sub

    'Private Sub vsfSearchSelectedItem()
    '    If vsfView.Rows.Count > 1 Then
    '        Call FillItemDtls(vsfView.GetData(vsfView.Row, 0))
    '        Call setSearchGridHt("")
    '    End If
    'End Sub

    'Private Sub setSearchGridHt(ByVal argFlag As String)
    '    If argFlag = "FULL" Then
    '        vsfView.Visible = True
    '        vsfView.BringToFront()
    '    Else
    '        vsfView.Visible = False
    '    End If
    'End Sub

    'Private Function FillDocValue(ByVal argSQL As String) As String
    '    FillDocValue = ""
    '    Dim rsDocVal As oledb.oledbDataReader
    '    Try
    '        rsDocVal = clsSVT.getDataReaderFromSQL(argSQL)
    '        With rsDocVal
    '            If Not .HasRows Then
    '                FillDocValue = ""
    '                Exit Function
    '            End If
    '            .Read()

    '            FillDocValue = "NO: " & !DocNo & ", " & !DocDate & ", " & !PartyCode & ", Qty: " & !DocQty & ", Rate: " & !DocRate
    '        End With
    '    Catch ex As Exception
    '        MsgBox(ex.Message, vbInformation, Me.Text)
    '    End Try
    'End Function

    'Private Sub txtNos_KeyUp(sender As Object, e As KeyEventArgs) Handles txtNos.KeyUp
    '    If e.KeyCode = Keys.Enter Then
    '        Call btnLoad_Click(sender, e)
    '    End If
    'End Sub

    'Private Sub btnLoad_Click(sender As Object, e As EventArgs) Handles btnLoad.Click
    '    Dim IntCompID As Int32 = 0
    '    If Trim(txtItemCode.Text) <> "" Then
    '        intItemID = clsSVT.getDataLongValue("Select ItemID from MstItem where ItemCode = '" & Trim(txtItemCode.Text) & "'", 0, False)
    '    End If
    '    If pnlComp.Visible And UCase(cboComp.Text) <> "ALL" Then
    '        IntCompID = clsSVT.getDataLongValue("Select CompID from MstComp where CompLoc = '" & cboComp.Text & "'", 1, False)
    '    End If

    '    If cboTrn.Text = "" Then
    '        MsgBox("Please select Document to be loaded!!", vbInformation, Me.Text)
    '        cboTrn.Focus()
    '        Exit Sub
    '    End If
    '    If Val(txtNos.Text) <= 0 Then Exit Sub

    '    Dim dtSearch As DataTable, dvSearch As DataView
    '    Application.DoEvents()

    '    strSQL = getStrSQL(IntCompID)


    '    If strSQL = "" Then Exit Sub

    '    Call showWaitCursor()
    '    Try
    '        vsfDet.BeginUpdate()

    '        If blnProcedure = False Then
    '            dtSearch = clsSVT.getDATATable(strSQL)
    '            dvSearch = New DataView(dtSearch)
    '            vsfDet.DataSource = dvSearch
    '        Else
    '            Dim rs As oledbDataReader = clsSVT.getDataReaderFromSQL(strSQL)
    '            If IsNothing(rs) Then
    '                rs.Close()
    '                MsgBox("No Records!", MsgBoxStyle.Critical, Me.Text)
    '                Exit Sub
    '            End If

    '            If Not rs.HasRows Then
    '                MsgBox("No Records!", MsgBoxStyle.Critical, Me.Text)
    '            End If

    '            dtSearch = New DataTable
    '            dtSearch.Load(rs)
    '            rs.Close()
    '            dvSearch = New DataView(dtSearch)
    '            vsfDet.DataSource = dvSearch
    '        End If
    '        vsfDet.Refresh()
    '        vsfDet.EndUpdate()
    '        Call showNormalCursor()
    '    Catch ex As Exception
    '        MsgBox(ex.Message, vbInformation, Me.Text)
    '    End Try

    'End Sub

    'Private Function getStrSQL(ByVal argCompID As Integer) As String
    '    getStrSQL = ""
    '    strSQL = ""
    '    Select Case UCase(cboTrn.Text)
    '        Case "PURCHASE ORDER"
    '            strSQL = "Select top " & Val(txtNos.Text) & " PONo , PODate, SuppCode, POQty,schDate, PORate"
    '            strSQL &= " from TrnHdrPO h, TrnDtlPO d, MstSupp s"
    '            strSQL &= " where h.POID = d.POID"
    '            strSQL &= " And s.SuppID=h.SuppID"
    '            strSQL &= " and d.itemID = " & intItemID
    '            If UCase(cboComp.Text) <> "ALL" Then
    '                strSQL &= " And CompID = " & argCompID
    '            End If
    '            strSQL &= " order by PONO desc "

    '        Case "SALES ORDER"
    '            strSQL = "Select top " & Val(txtNos.Text) & " SONo, SODate, CustCode, SOQty,SchDate, SORate"
    '            strSQL &= " from TrnHdrSO h, TrnDtlSO d, MstCust c"
    '            strSQL &= " where h.SOID = d.SOID"
    '            strSQL &= " And c.CustID=h.CustID"
    '            strSQL &= " and d.itemID = " & intItemID
    '            If UCase(cboComp.Text) <> "ALL" Then
    '                strSQL &= " And CompID = " & argCompID
    '            End If
    '            strSQL &= " order by SONO desc "

    '        Case "SALES INVOICE"
    '            strSQL = "Select top " & Val(txtNos.Text) & " InvNo, InvDate, CustCode, InvQty, InvRate"
    '            strSQL &= " from TrnHdrInv h, TrnDtlInv d, MstCust c"
    '            strSQL &= " where h.InvID = d.InvID"
    '            strSQL &= " and d.itemID = " & intItemID
    '            strSQL &= " And c.CustID=h.CustID"
    '            If UCase(cboComp.Text) <> "ALL" Then
    '                strSQL &= " And CompID = " & argCompID
    '            End If
    '            strSQL &= " order by InvNO desc "


    '        Case "GOODS RECEIVED NOTE"
    '            strSQL = "Select top " & Val(txtNos.Text) & " GRNNo, GRNDate, SuppCode, GRNQty,SchDate, GRNRate"
    '            strSQL &= " from TrnHdrGRN h, TrnDtlGRN d, MstSupp s"
    '            strSQL &= " where h.GRNID = d.GRNID"
    '            strSQL &= " and d.itemID = " & intItemID
    '            strSQL &= " And s.SuppID=h.SuppID"
    '            If UCase(cboComp.Text) <> "ALL" Then
    '                strSQL &= " And CompID = " & argCompID
    '            End If
    '            strSQL &= " order by GRNNO desc "


    '        Case "LOCAL QUOTATION"
    '            strSQL = "Select top " & Val(txtNos.Text) & " LQTNNo, LQTNDate, CustCode, LQTNQty, LQTNRate"
    '            strSQL &= " from TrnHdrLQTN h, TrnDtlLQTN d, MstCust c"
    '            strSQL &= " where h.LQTNID = d.LQTNID"
    '            strSQL &= " And c.CustID=h.CustID"
    '            strSQL &= " and d.itemID = " & intItemID
    '            If UCase(cboComp.Text) <> "ALL" Then
    '                strSQL &= " And CompID = " & argCompID
    '            End If
    '            strSQL &= " order by LQTNNO desc "

    '        Case "IMPORT QUOTATION"
    '            strSQL = "Select top " & Val(txtNos.Text) & " IQTNNo, IQTNDate, CustCode, IQTNQty, IQTNRate"
    '            strSQL &= " from TrnHdrIQTN h, TrnDtlIQTN d, MstCust c"
    '            strSQL &= " where h.IQTNID = d.IQTNID"
    '            strSQL &= " And c.CustID=h.CustID"
    '            strSQL &= " and d.itemID = " & intItemID
    '            If UCase(cboComp.Text) <> "ALL" Then
    '                strSQL &= " And CompID = " & argCompID
    '            End If
    '            strSQL &= " order by IQTNNO desc "

    '        Case "PROFORMA INVOICE"
    '            strSQL = "Select top " & Val(txtNos.Text) & " PINVNo, PINVDate, CustCode, PINVQty, PINVRate"
    '            strSQL &= " from TrnHdrPINV h, TrnDtlPINV d, MstCust c"
    '            strSQL &= " where h.PINVID = d.PINVID"
    '            strSQL &= " And c.CustID=h.CustID"
    '            strSQL &= " and d.itemID = " & intItemID
    '            If UCase(cboComp.Text) <> "ALL" Then
    '                strSQL &= " And CompID = " & argCompID
    '            End If
    '            strSQL &= " order by PINVNO desc "

    '        Case "SAMPLE SUBMISSION"
    '            strSQL = "Select top " & Val(txtNos.Text) & " SSUBNo, SSUBDate, CustCode, Qty"
    '            strSQL &= " from TrnHdrSSUB h, TrnDtlSSUB d, MstCust c"
    '            strSQL &= " where h.SSUBID = d.SSUBID"
    '            strSQL &= " And c.CustID=h.CustID"
    '            strSQL &= " and d.itemID = " & intItemID
    '            If UCase(cboComp.Text) <> "ALL" Then
    '                strSQL &= " And CompID = " & argCompID
    '            End If
    '            strSQL &= " order by SSUBNO desc "

    '        Case "SALES RETURN"
    '            strSQL = "Select top " & Val(txtNos.Text) & " SRNo, SRDate, CustCode, SRQty"
    '            strSQL &= " from TrnHdrSR h, TrnDtlSR d, MstCust c"
    '            strSQL &= " where h.SRID = d.SRID"
    '            strSQL &= " And c.CustID=h.CustID"
    '            strSQL &= " and d.itemID = " & intItemID
    '            If UCase(cboComp.Text) <> "ALL" Then
    '                strSQL &= " And CompID = " & argCompID
    '            End If
    '            strSQL &= " order by SRNO desc "

    '        Case "CUSTOMER ORDER"
    '            strSQL = "Select top " & Val(txtNos.Text) & " CustPONO, custPODt, CustCode, CPOQty"
    '            strSQL &= " from TrnHdrCPO h, TrnDtlCPO d, MstCust c"
    '            strSQL &= " where h.CPOID = d.CPOID"
    '            strSQL &= " And c.CustID=h.CustID"
    '            strSQL &= " and d.itemID = " & intItemID
    '            If UCase(cboComp.Text) <> "ALL" Then
    '                strSQL &= " And CompID = " & argCompID
    '            End If
    '            strSQL &= " order by CPONO desc "

    '        Case "SHIPMENT ENTRY CPO"
    '            strSQL = "Select top " & Val(txtNos.Text) & " CSHPNo, CSHPDate, CustCode, CSHPQty"
    '            strSQL &= " from TrnHdrCSHP h, TrnDtlCSHP d, MstCust c"
    '            strSQL &= " where h.CSHPID = d.CSHPID"
    '            strSQL &= " And c.CustID=h.CustID"
    '            strSQL &= " and d.itemID = " & intItemID
    '            If UCase(cboComp.Text) <> "ALL" Then
    '                strSQL &= " And CompID = " & argCompID
    '            End If
    '            strSQL &= " order by CSHPNO desc "

    '        Case "DELIVERY CHALLAN"
    '            strSQL = "Select top " & Val(txtNos.Text) & " DCNo, DCDate, PartyName, DCQty"
    '            strSQL &= " from TrnHdrDC h, TrnDtlDC d"
    '            strSQL &= " where h.DCID = d.DCID"
    '            strSQL &= " and d.itemID = " & intItemID
    '            If UCase(cboComp.Text) <> "ALL" Then
    '                strSQL &= " And CompID = " & argCompID
    '            End If
    '            strSQL &= " order by DCNO desc "

    '        Case "DELIVERY CHALLAN RECEIPT"
    '            strSQL = "Select top " & Val(txtNos.Text) & " DCRNo, DCRDate, PartyName, DCRQty"
    '            strSQL &= " from TrnHdrDCR h, TrnDtlDCR d"
    '            strSQL &= " where h.DCRID = d.DCRID"
    '            strSQL &= " and d.itemID = " & intItemID
    '            If UCase(cboComp.Text) <> "ALL" Then
    '                strSQL &= " And CompID = " & argCompID
    '            End If
    '            strSQL &= " order by DCRNO desc "

    '        Case "DIVERSION PO"
    '            strSQL = "Select top " & Val(txtNos.Text) & " DPONo, DPODate, SuppCode, DPOQty, DPORate"
    '            strSQL &= " from TrnHdrDPO h, TrnDtlDPO d, MstSupp s"
    '            strSQL &= " where h.DPOID = d.DPOID"
    '            strSQL &= " And s.SuppID=h.SuppID"
    '            strSQL &= " and d.itemID = " & intItemID
    '            If UCase(cboComp.Text) <> "ALL" Then
    '                strSQL &= " And CompID = " & argCompID
    '            End If
    '            strSQL &= " order by DPONO desc "

    '        Case "DIV PO SHIPENT ENTRY"
    '            strSQL = "Select top " & Val(txtNos.Text) & " SEPONo, SEPODate, SuppCode, SEPOQty, SEPORate"
    '            strSQL &= " from TrnHdrSEPO h, TrnDtlSEPO d, MstSupp s"
    '            strSQL &= " where h.SEPOID = d.SEPOID"
    '            strSQL &= " And s.SuppID=h.SuppID"
    '            strSQL &= " and d.itemID = " & intItemID
    '            If UCase(cboComp.Text) <> "ALL" Then
    '                strSQL &= " And CompID = " & argCompID
    '            End If
    '            strSQL &= " order by SEPONO desc "

    '        Case "STOCK ADDITION AND DEDUCTION"
    '            strSQL = "Select top " & Val(txtNos.Text) & " STKADJNo, STKADJDate, CustCode, Qty "
    '            strSQL &= " from TrnHdrSTKADJ h, TrnDtlSTKADJ d, MstCust c"
    '            strSQL &= " where h.STKADJID = d.STKADJID"
    '            strSQL &= " And c.CustID=h.CustID"
    '            strSQL &= " and d.itemID = " & intItemID
    '            If UCase(cboComp.Text) <> "ALL" Then
    '                strSQL &= " And CompID = " & argCompID
    '            End If
    '            strSQL &= " order by STKADJNO desc "
    '    End Select
    '    getStrSQL = strSQL
    'End Function

    'Private Sub cboTrn_Validated(sender As Object, e As EventArgs) Handles cboTrn.Validated
    '    Call btnLoad_Click(sender, e)
    'End Sub

    'Private Sub FillItemDtls(argItemCode As String)
    '    txtDet.Text = ""
    '    'vsfView.Rows.Count = 1
    '    txtQOH.Text = ""

    '    txtItemCode.Text = argItemCode
    '    intItemID = clsSVT.getDataLongValue("Select ItemID from MstItem where ItemCode = '" & vsfView.GetData(vsfView.Row, 0) & "'", 0, False)
    '    txtDet.Text = clsSVT.getDataStringValue("Select ItemName from MstItem where ItemID = " & intItemID, "", False)
    '    txtDet.Text = txtDet.Text & vbCrLf & "Grouped Under :" & clsSVT.getDataStringValue("Select ItemGroupcode from MstItem i, MstItemGroup g where i.ItemGroupID = g.ItemGroupID and ItemID = " & intItemID, "", False)
    '    txtQOH.Text = clsSVT.getDataDoubleValue("Select QOH from TrnQOH where ItemID = " & intItemID, 0, False)
    '    Call vwStockDtls()
    'End Sub
    Dim argvsf As C1FlexGrid


    Private Sub pnlbtnClose_Click(sender As Object, e As EventArgs) Handles pnlbtnClose.Click
        pnlCopyShortCut.Visible = False
    End Sub

    Private Sub btnCopy_Click(sender As Object, e As EventArgs) Handles btnCopy.Click

        If RbnCopyCell.Checked = True Then
            CopyGrid(argvsf, "Cell")
        ElseIf RbnCopyColumn.Checked = True Then
            CopyGrid(argvsf, "Column")
        ElseIf RbnCopyRow.Checked = True Then
            CopyGrid(argvsf, "Row")
        ElseIf RbnCopyGrid.Checked = True Then
            CopyGrid(argvsf, "Full Grid")

        End If

        pnlCopyShortCut.Visible = False
    End Sub

    Private Sub vsfView_Click(sender As Object, e As EventArgs) Handles vsfView.Click
        If (Control.ModifierKeys And Keys.Control) = Keys.Control AndAlso (Control.ModifierKeys And Keys.Alt) = Keys.Alt Then
            pnlCopyShortCut.Show()
        End If
        argvsf = vsfView
    End Sub
    Private Sub vsfDet_Click(sender As Object, e As EventArgs) Handles vsfDet.Click

        If (Control.ModifierKeys And Keys.Control) = Keys.Control AndAlso (Control.ModifierKeys And Keys.Alt) = Keys.Alt Then
            pnlCopyShortCut.Show()
        End If
        argvsf = vsfDet

    End Sub


    Private Sub vsfTrn_Click(sender As Object, e As EventArgs) Handles vsfTrn.Click

        If (Control.ModifierKeys And Keys.Control) = Keys.Control AndAlso (Control.ModifierKeys And Keys.Alt) = Keys.Alt Then
            pnlCopyShortCut.Show()
        End If
        argvsf = vsfDet

    End Sub
End Class