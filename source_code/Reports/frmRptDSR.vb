Imports System.Data.oledb
Imports C1.Win.C1FlexGrid

Public Class frmRptDailySales
    Public strList As String
    Dim strItemGroupList As String
    Dim strMakeList As String
    Dim strSuppList As String
    Dim strCustList As String
    Dim strZOneList As String
    Dim strUserList As String
    Dim strItemList As String
    Dim strRegionList As String
    Dim strPrincipalList As String
    Dim argvsf As C1FlexGrid
    Public strRptName As String = ""
    Dim blnProcedure As Boolean = False
    Dim clsSVT As New svtLibrary
    Dim strHeading As String = ""

    Dim dtSearch As DataTable
    Dim dvSearch As DataView
    Public IntCompID As Int32 = 0

    ''vsfDET'''''
    Const VDetSINVNO = 0
    Const VDetCancelled = 1
    Const VDetCustName = 2
    Const VDetGSTNO = 3
    Const VDetGST = 4
    Const VDetNetTOTAL = 5
    Const VDetCash = 6
    'Const intColFixTotal = 7

    Const vdetSalesPerson = 7
    Const vdetVat = 8
    Const vdetCredit = 9
    Const intColTotal = 10

    Private Sub btnClose_Click(sender As System.Object, e As System.EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub



    Private Sub btnView_Click(sender As System.Object, e As System.EventArgs) Handles btnView.Click
        Dim strSQL As String = ""
        strHeading = ""

        If Format(dtpFromDate.Value, "dd/MM/yyyy") > Format(dtpToDate.Value, "dd/MM/yyyy") Then
            MsgBox("From date Should Be less than To Date!", vbInformation, Me.Text)
            Exit Sub
        End If
        If cboTripNo.Text <> "" Then
            If Not clsSVT.execSQLStatement("Select * from TRNHDRSINVDCT where TripNo = '" & Trim(cboTripNo.Text) & "'") Then
                MsgBox("Please Select Valid TripNo", vbInformation, Me.Text)
                Exit Sub
            End If

        End If

        Call initscreen()
        If CreateGridColumns() Then
            Call FillDocDtls()
        End If
        strHeading = "Daily Sales Report For Date Range  " & Replace(getDateFormatInCond(dtpFromDate.Value), "'", "") & "  To  " & Replace(getDateFormatInCond(dtpToDate.Value), "'", "")
        Call initVsfForHdrs(strRptName, strHeading)

        If Trim(cboTripNo.Text) = "" Then
            SetHideCol(vsfDet, True, vdetVat, vdetCredit)
        Else
            SetHideCol(vsfDet, True, vdetVat, vdetCredit)
        End If

        If chkUser.Checked Then
            SetHideCol(vsfDet, False, vdetSalesPerson)
        Else
            SetHideCol(vsfDet, True, vdetSalesPerson)
        End If
        If Trim(cboTripNo.Text) <> "" AndAlso chkUser.Checked Then
            SetHideCol(vsfDet, False, vdetVat, vdetCredit)
        End If
        'Call initVsfForHdrs(strRptName, strHeading)

        'If vsfDet.Rows.Count > 4 Then
        '    Call HideIDCols()
        'End If

        'Call HideIDCols()

        vsfDet.SubtotalPosition = C1.Win.C1FlexGrid.SubtotalPositionEnum.BelowData
        vsfDet.Subtotal(C1.Win.C1FlexGrid.AggregateEnum.Sum, 0, -1, VDetNetTOTAL)
        vsfDet.Subtotal(C1.Win.C1FlexGrid.AggregateEnum.Sum, 0, -1, vdetCredit)
        vsfDet.Subtotal(C1.Win.C1FlexGrid.AggregateEnum.Sum, 0, -1, vdetVat)
        vsfDet.Subtotal(C1.Win.C1FlexGrid.AggregateEnum.Sum, 0, -1, VDetCash)
        For i = intColTotal To vsfDet.Cols.Count - 1
            vsfDet.Subtotal(C1.Win.C1FlexGrid.AggregateEnum.Sum, 0, -1, i)
        Next
        'Call showNormalCursor()
    End Sub

    Private Sub btnExcel_Click(sender As System.Object, e As System.EventArgs) Handles btnExcel.Click
        Call ExportToXL(vsfDet,,,, True, Me.Text, "For the period " & getDateFormatInInsert(dtpFromDate.Value) & " to " & getDateFormatInInsert(dtpToDate.Value))
    End Sub

    Private Sub frmRptPendCPO_KeyUp(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyUp
        If e.KeyCode = Keys.F9 And e.Control Then
            '958, 579
            Call initFormSize(Me, strRptName, True, 958, 579, 0, 0)
            MsgBox("Restored Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
        If e.KeyCode = Keys.F8 And e.Control = True Then
            Call SaveActFormSettings(Me, Me.Text)
            Call SaveActFormVsfDet(Me, Me.Text, "DET", vsfDet)
            MsgBox("Saved Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
    End Sub

    Private Sub frmRptPendCPO_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        strRptName = "DSR"
        vsfView.Visible = False
        Call fillFormProperties(Me)
        Call initFormVsfDet(Me, strRptName, vsfDet, "DET")
        initGridStyles(Me)

        'Call bindCombo(cboTripNo, "", "TripNO", "Select Distinct TripNO from TrnHdrSINVDCT order by 1")
        chkDate.Checked = True
        cboTripNo.Text = ""
    End Sub
    Public Sub initscreen()

        With vsfDet
            .Visible = True
            .Rows.Count = 4
            .Rows.Fixed = 4
            .Rows.GlyphRow = 3
            .Cols.Count = intColTotal

            .SetData(3, VDetSINVNO, "Inv No")
            .SetData(3, VDetCancelled, "Cancelled")
            .SetData(3, VDetCustName, "Customer Name")
            .SetData(3, VDetGSTNO, "GST NO")
            .SetData(3, VDetGST, "GST")
            .SetData(3, VDetNetTOTAL, "Net Total")
            .SetData(3, VDetCash, "Cash")
            .SetData(3, vdetVat, "VAT")
            .SetData(3, vdetCredit, "Credit")
            .SetData(3, vdetSalesPerson, "Sales Person")

            If Trim(cboTripNo.Text) = "" Then
                SetHideCol(vsfDet, True, vdetVat, vdetCredit)
            Else
                SetHideCol(vsfDet, False, vdetVat, vdetCredit)
            End If

            If chkUser.Checked Then
                SetHideCol(vsfDet, False, vdetSalesPerson)
            Else
                SetHideCol(vsfDet, True, vdetSalesPerson)
            End If

        End With
    End Sub

    'Private Sub chkSalesPerson_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkUser.CheckedChanged
    '    Dim strSQL As String

    '    If chkUser.Checked = False Then
    '        strList = ""
    '        strUserList = ""
    '        ' txtUser.Text = ""
    '        Exit Sub
    '    End If

    '    Dim frmSel As New frmUtlMultiSelRpt

    '    strSQL = "Select distinct 0 As sel, SpName As SPNAME from MstSP where Spid>0 "
    '    'strSQL = strSQL & " SPCODE Not In ('-' , '') order by SPCODE"
    '    Call frmSel.InitVsfSel(strSQL, 0, 1, True, Me, False)
    '    frmSel.Text = "Select [Multiple] Sales Person"
    '    frmSel.vsfDet.SetData(0, 0, "Sel")
    '    frmSel.StartPosition = FormStartPosition.CenterScreen
    '    frmSel.ShowDialog()
    '    strUserList = strList


    '    Dim strUser As String = ""
    '    If strUserList <> "" Then
    '        For Each strtempuser As String In strUserList.Split(",")
    '            strUser = strUser & "'" & strtempuser & "',"
    '        Next
    '        strUser = Mid(strUser, 1, Len(strUser) - 1)
    '        strUserList = strUser
    '    End If
    '    If strUserList = "" Then chkUser.Checked = False
    'End Sub

    Public Sub disableall()
        chkDate.Visible = False
        pnlDate.Visible = False
        lblTripNo.Visible = False
        cboTripNo.Visible = False
        chkItemGroup.Visible = False
        chkZone.Visible = False
        chkUser.Visible = False
        chkCustName.Visible = False
        chkSupp.Visible = False
        chkMake.Visible = False
        pnStk.Visible = False
        pnPending.Visible = False
        chkPrincipals.Visible = False
        ChkRegion.Visible = False
        pnCOD.Visible = False
        vsfDet.Visible = False
    End Sub

    Public Sub enableall()
        chkDate.Visible = True
        pnlDate.Visible = True
        chkUser.Visible = True
        lblTripNo.Visible = True
        cboTripNo.Visible = True
    End Sub



    Private Sub HideIDCols()
        ' Dim strSave
        With vsfDet
            Dim strCol As String = ""
            If .Cols.Count >= 1 Then
                For i As Int32 = 0 To .Cols.Count - 1
                    strCol = GetVal(.GetData(3, i), "")
                    If strCol <> "" Then
                        If UCase(Mid(strCol, Len(strCol) - 1, 2)) = "ID" Then
                            .Cols(i).Visible = False
                        End If
                    End If
                Next
            End If
        End With
    End Sub


    'Private Sub LoadHeadings()
    '    'vsfDet.Rows.Fixed = 4
    '    'vsfDet.Rows.GlyphRow = 3
    'End Sub

    Private Sub initVsfForHdrs(ByVal argHdrLine1 As String, ByVal argHdrLine2 As String)
        Dim i As Integer

        With vsfDet
            For i = 0 To .Cols.Count - 1
                ' .SetData(3, i, .GetData(0, i))

                .SetData(0, i, gSettings.gCompName)
                .SetData(1, i, argHdrLine1)
                .SetData(2, i, argHdrLine2)
            Next
            Dim csf1 As C1.Win.C1FlexGrid.CellStyle = vsfDet.Styles.Add("F1")
            csf1.BackColor = Color.Beige

            .Rows(0).Style = csf1
            .Rows(1).Style = csf1
            .Rows(2).Style = csf1

            If argHdrLine1 = "" Then
                .Rows(1).Visible = False
            Else
                .Rows(1).Visible = True
            End If

            If argHdrLine2 = "" Then
                .Rows(2).Visible = False
            Else
                .Rows(2).Visible = True
            End If

            .Rows(0).AllowMerging = True
            .Rows(1).AllowMerging = True
            .Rows(2).AllowMerging = True
            .Rows(3).AllowMerging = True
            .Rows(1).Visible = False
        End With
    End Sub

    Private Sub vsfDet_BeforeEdit(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfDet.BeforeEdit
        e.Cancel = True
    End Sub

    Private Sub txtItemCode_KeyUp(sender As Object, e As System.Windows.Forms.KeyEventArgs)
        If e.KeyCode = Keys.Escape Then
            vsfView.Visible = False
            cboTripNo.Focus()
        End If

        If e.KeyCode = Keys.Enter Or e.KeyCode = Keys.Return Then
            If vsfView.Visible Then
                If vsfView.Rows.Count > 1 Then
                    cboTripNo.Text = vsfView.GetData(vsfView.Row, 1)
                End If
            End If
        End If
        If e.KeyCode = Keys.Down Then
            If vsfView.Visible Then
                If vsfView.Rows.Count > 1 Then
                    vsfView.Focus()
                    vsfView.Col = 1
                    vsfView.Row = 1
                    vsfView.TopRow = 1
                End If
            End If
        End If
    End Sub

    'Private Sub txtItemCode_TextChanged(sender As System.Object, e As System.EventArgs)
    '    Call applyFilterFromGrid()
    'End Sub

    Private Sub vsfView_BeforeEdit(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfView.BeforeEdit
        e.Cancel = True
    End Sub

    Private Sub vsfView_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles vsfView.KeyUp
        If e.KeyCode = Keys.Escape Then
            cboTripNo.Focus()
            vsfView.Visible = False
        End If
        If vsfView.Row > 0 Then
            If e.KeyCode = Keys.Return Or e.KeyCode = Keys.Tab Then
                cboTripNo.Text = vsfView.GetData(vsfView.Row, 1)
                vsfView.Visible = False
            End If
        End If

    End Sub

    Private Sub vsfView_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles vsfView.DoubleClick
        If vsfView.Row > 0 Then
            cboTripNo.Text = vsfView.GetData(vsfView.Row, 1)
            vsfView.Visible = False
        End If
    End Sub

    Private Function applyFilterFromGrid() As Boolean
        applyFilterFromGrid = False
        Dim strSQL As String = ""
        With cboTripNo
            If Trim(.Text) = "" Then
                vsfView.Visible = False
                Exit Function
            End If
            If Len(.Text) <= 2 Then
                vsfView.Visible = False
                Exit Function
            End If
        End With

        vsfView.Visible = True

        Dim dtSearch As DataTable = Nothing

        Application.DoEvents()


        strSQL = "SELECT top 50 itemid, ItemCode, ItemName, MakeCode "
        strSQL &= " FROM MstItem i, MstMake m "
        strSQL &= " WHERE i.InActive = 'N'"
        strSQL &= " and i.MakeID = m.MakeID"
        strSQL &= " and  ItemCode like '%" & cboTripNo.Text & "%'"
        strSQL &= " and ItemID > 0"
        strSQL &= " order by 2, 3"

        Call showWaitCursor()
        dtSearch = clsSVT.getDATATable(strSQL)

        dvSearch = New DataView(dtSearch)

        vsfView.DataSource = dvSearch
        vsfView.Visible = True
        vsfView.Cols(0).Visible = False
        vsfView.Cols(1).Width = 150
        Call showNormalCursor()

        applyFilterFromGrid = True
    End Function



    Public Function getDSR() As String
        getDSR = ""
        Dim strSQL As String = "", strCond As String = ""

        strSQL = "Select SINVDCTNO, Cancelled, CustName, GstNO, SUM(CGSTAmt) as GST, NETTOT, NETTOT "
        strSQL = strSQL & "From TrnHdrSINVDCT h,TrndtlSINVDCT d, MstCust c "
        strSQL = strSQL & "where h.custid=c.Custid and h.SINVDCTID=d.SINVDCTID "
        If chkUser.Checked = False Or cboTripNo.Text = "" Then
            strSQL = strSQL & " And  H.BILLTYPE='CASH'"
        End If

        strSQL = strSQL & "group by SINVDCTNO,Cancelled,CustName, GstNO, NETTOT,NETTOT"

        Call showWaitCursor()

        Dim rs As oledbDataReader = clsSVT.getDataReaderFromSQL(strSQL)
        If IsNothing(rs) Then
            rs.Close()
            MsgBox("No Records!", MsgBoxStyle.Critical, Me.Text)
            Exit Function
        End If
        vsfDet.Rows.Count = 5
        Do While rs.Read()

            vsfDet.Rows.Count = vsfDet.Rows.Count + 1
            vsfDet.SetData(vsfDet.Rows.Count - 1, VDetSINVNO, rs!SINVDCTNO)
            vsfDet.SetData(vsfDet.Rows.Count - 1, VDetCancelled, rs!Cancelled)
            vsfDet.SetData(vsfDet.Rows.Count - 1, VDetCustName, rs!CustName)
            vsfDet.SetData(vsfDet.Rows.Count - 1, VDetGSTNO, rs!GstNO)
            vsfDet.SetData(vsfDet.Rows.Count - 1, VDetGST, rs!GST)
            vsfDet.SetData(vsfDet.Rows.Count - 1, VDetNetTOTAL, rs!NETTOT)
            vsfDet.SetData(vsfDet.Rows.Count - 1, VDetCash, rs!NETTOT)
        Loop
        rs.Close()
        vsfDet.AutoSizeCols()
        vsfDet.ExtendLastCol = False


        For i = 5 To vsfDet.Rows.Count - 1
            For j = intColTotal To vsfDet.Cols.Count - 1
                'strSQL = "Select m.ItemCode from TrndtlSINVDCT d,TrnHdrSINVDCT h,MstItem m "
                'strSQL = strSQL & "where h.SINVDCTID=d.SINVDCTID and d.Itemid=m.itemid"
                'Dim strItemcode As String
                'strItemcode = clsSVT.getDataStringValue(strSQL, "", False)

                'Dim str As String = "Select sum(SINVDCTQTY) from TrndtlSINVDCT d,TrnHdrSINVDCT h,
                'MstItem m where h.SINVDCTNO='" & vsfDet.GetData(i, VDetSINVNO) & "' and  M.ITEMcode= '" & strItemcode & "'
                'group  by h.SINVDCTNo,d.Itemid"


                vsfDet.SetData(i, j, clsSVT.getDataLongValue("Select d.SINVDCTQTY from TrndtlSINVDCT d,TrnHdrSINVDCT h,
                MstItem m where h.SINVDCTNO='" & vsfDet.GetData(i, VDetSINVNO) & "' and  M.ITEMcode= '" & vsfDet.GetData(4, j) & "' and h.SINVDCTID=d.SINVDCTID 
                group  by h.SINVDCTNo,d.Itemid ,d.SINVDCTQTY", 0, False))
            Next

        Next
        vsfDet.SubtotalPosition = C1.Win.C1FlexGrid.SubtotalPositionEnum.BelowData
        vsfDet.Subtotal(C1.Win.C1FlexGrid.AggregateEnum.Sum, 0, -1, VDetNetTOTAL)
        rs.Close()
    End Function

    Private Function CreateGridColumns() As Boolean
        CreateGridColumns = False
        Dim strsql As String
        strsql = " Select distinct ITEMCODE "
        strsql = strsql & " from TrnHdrSINVDCT h, TrnDtlSINVDCT d, MstITEM i  "
        strsql = strsql & " Where h.SINVDCTID = d.SINVDCTID"
        strsql = strsql & " And d.ITEMID=i.ITEMID "
        strsql = strsql & " And h.SINVDCTDate BETWEEN " & getDateFormatInCond(dtpFromDate.Value) & " and " & getDateFormatInCond(dtpToDate.Value)
        'If chkUser.Checked = False Or cboTripNo.Text = "" Then
        'strsql = strsql & " And  H.BILLTYPE='CASH'"
        'End If

        If Trim(cboTripNo.Text) <> "" AndAlso chkUser.Checked Then
            strsql = strsql & " And  H.BILLTYPE='CREDIT'  "
        End If
        If Trim(cboTripNo.Text) <> "" Then
            strsql = strsql & " And h.TripNO= '" & Trim(cboTripNo.Text) & "'"
        End If
        strsql = strsql & " order by ITEMCODE"

        Dim rs As oledb.oledbDataReader
        rs = clsSVT.getDataReaderFromSQL(strsql)

        If Not rs.HasRows Then
            rs.Close()
            MsgBox("NO Items available for this SINVNO", vbInformation, Me.Text)
            CreateGridColumns = False
            Exit Function
        End If
        'vsfDet.Rows.Count = 4

        Do While rs.Read
            vsfDet.Cols.Count = vsfDet.Cols.Count + 1
            vsfDet.SetData(3, vsfDet.Cols.Count - 1, rs!ItemCode)
        Loop
        rs.Close()
        CreateGridColumns = True
    End Function

    Private Sub FillDocDtls()
        Dim strSQL As String = ""

        strSQL = "Select SINVDCTNO, Cancelled, CustName, GstNO, SUM(CGSTAmt) as GST,  "
        strSQL = strSQL & " NETTOTRNDOFF,ToTGST,SPCODE "
        strSQL = strSQL & " From TrnHdrSINVDCT h, TrndtlSINVDCT d, MstCust c,MstSp s "
        strSQL = strSQL & " where h.custid=c.Custid and h.SINVDCTID=d.SINVDCTID "
        strSQL = strSQL & " and s.SPID=h.SPID "

        'If chkUser.Checked = False Or cboTripNo.Text = "" Then
        'strSQL = strSQL & " And  H.BILLTYPE='CASH'"
        'End If

        If Trim(cboTripNo.Text) <> "" AndAlso chkUser.Checked Then
            strSQL = strSQL & " And  H.BILLTYPE='CREDIT'  "
        End If
        If Trim(cboTripNo.Text) <> "" Then
            strSQL = strSQL & " And h.TripNO= '" & Trim(cboTripNo.Text) & "'"
        End If
        strSQL = strSQL & " And h.SINVDCTDate BETWEEN " & getDateFormatInCond(dtpFromDate.Value) & " and " & getDateFormatInCond(dtpToDate.Value)

        strSQL = strSQL & " group by SINVDCTNO,Cancelled,CustName, GstNO, NETTOTRNDOFF,ToTGST,SPCODE"

        Call showWaitCursor()

        Dim rs As oledbDataReader = clsSVT.getDataReaderFromSQL(strSQL)
        If IsNothing(rs) Then
            rs.Close()
            MsgBox("No Records!", MsgBoxStyle.Critical, Me.Text)
            Exit Sub
        End If
        vsfDet.Rows.Count = 4
        Do While rs.Read()
            vsfDet.Rows.Count = vsfDet.Rows.Count + 1
            vsfDet.SetData(vsfDet.Rows.Count - 1, VDetSINVNO, rs!SINVDCTNO)
            vsfDet.SetData(vsfDet.Rows.Count - 1, VDetCancelled, rs!Cancelled)
            vsfDet.SetData(vsfDet.Rows.Count - 1, VDetCustName, rs!CustName)
            vsfDet.SetData(vsfDet.Rows.Count - 1, VDetGSTNO, rs!GstNO)
            vsfDet.SetData(vsfDet.Rows.Count - 1, VDetGST, rs!GST)
            vsfDet.SetData(vsfDet.Rows.Count - 1, VDetNetTOTAL, rs!NETTOTRNDOFF)
            vsfDet.SetData(vsfDet.Rows.Count - 1, VDetCash, "")

            If Trim(cboTripNo.Text) <> "" AndAlso chkUser.Checked Then
                vsfDet.SetData(vsfDet.Rows.Count - 1, vdetVat, rs!ToTGST)
                vsfDet.SetData(vsfDet.Rows.Count - 1, vdetCredit, rs!NETTOTRNDOFF)
                vsfDet.SetData(vsfDet.Rows.Count - 1, vdetSalesPerson, rs!SPCODE)
            End If

            If chkUser.Checked Then
                vsfDet.SetData(vsfDet.Rows.Count - 1, vdetSalesPerson, rs!SPCODE)
            End If
        Loop
        rs.Close()
        vsfDet.AutoSizeCols()
        vsfDet.ExtendLastCol = False

        With vsfDet
            For i As Int32 = 4 To .Rows.Count - 1
                strSQL = " Select  i.Itemcode, sum(SINVDCTQty) Qty "
                strSQL &= " From TrnHdrSINVDCT h, TrndtlSINVDCT d, MstItem i "
                strSQL &= " Where h.SINVDCTID = d.SINVDCTID "
                strSQL &= " And d.ITEMID = i.ITEMID "
                strSQL &= " And SINVDCTNo = '" & GetVal(.GetData(i, VDetSINVNO), "") & "' "
                strSQL &= " Group By  i.Itemcode "

                rs = clsSVT.getDataReaderFromSQL(strSQL)

                If Not rs.HasRows Then
                    rs.Close()
                    GoTo nextrecord
                End If

                Do While rs.Read
                    For k As Int32 = intColTotal To .Cols.Count - 1
                        If UCase(.GetData(3, k)) = UCase(rs!Itemcode) Then
                            .SetData(i, k, rs!Qty)
                        End If
                    Next
                Loop
                rs.Close()
nextrecord:
            Next
        End With
    End Sub
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

    Private Sub vsfDet_Click(sender As Object, e As EventArgs) Handles vsfDet.Click

        If (Control.ModifierKeys And Keys.Control) = Keys.Control AndAlso (Control.ModifierKeys And Keys.Alt) = Keys.Alt Then
            pnlCopyShortCut.Show()
        End If
        argvsf = vsfDet

    End Sub

    Private Sub vsfView_Click(sender As Object, e As EventArgs) Handles vsfView.Click
        If (Control.ModifierKeys And Keys.Control) = Keys.Control AndAlso (Control.ModifierKeys And Keys.Alt) = Keys.Alt Then
            pnlCopyShortCut.Show()
        End If
        argvsf = vsfView
    End Sub
End Class