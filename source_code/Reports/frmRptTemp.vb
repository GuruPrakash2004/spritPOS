Imports System.Data.oledb
Imports C1.Win.C1FlexGrid

Public Class frmRptTemp
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

    Public strRptName As String = ""
    Dim blnProcedure As Boolean = False
    Dim clsSVT As New svtLibrary
    Dim strHeading As String = ""
    Dim argvsf As C1FlexGrid
    Dim dtSearch As DataTable
    Dim dvSearch As DataView
    Public IntCompID As Int32 = 0

    Private Sub btnClose_Click(sender As System.Object, e As System.EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub chkItemGroup_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkItemGroup.CheckedChanged
        Dim strSQL As String

        If chkItemGroup.Checked = False Then
            strList = ""
            strItemGroupList = ""
            Exit Sub
        End If

        Dim frmSel As New frmUtlMultiSelRpt

        strSQL = "Select distinct 0 as sel, ItemGroupId, ItemGroupCode from MstItemGroup where ItemGroupID > 0 and "
        strSQL = strSQL & " ItemGroupCode Not in ('-' , '') and InActive = 'N' order by ItemGroupCode"
        Call frmSel.InitVsfSel(strSQL, 1, 1, True, Me, False)
        frmSel.Text = "Select [Multiple] Item Groups"
        frmSel.vsfDet.SetData(0, 0, "Sel")
        frmSel.ShowDialog()
        strItemGroupList = strList
        If strItemGroupList = "" Then chkItemGroup.Checked = False
    End Sub

    Private Sub chkMake_CheckedChanged(sender As Object, e As System.EventArgs) Handles chkMake.CheckedChanged
        Dim strSQL As String

        If chkMake.Checked = False Then
            strList = ""
            strMakeList = ""
            Exit Sub
        End If

        Dim frmSel As New frmUtlMultiSelRpt

        strSQL = "Select distinct 0 as sel, MakeId, MakeCode from MstMake where MakeID > 0 and "
        strSQL = strSQL & " MakeCode Not in ('-' , '') and InActive = 'N' order by MakeCode"
        Call frmSel.InitVsfSel(strSQL, 1, 1, True, Me, False)
        frmSel.Text = "Select [Multiple] Make"
        frmSel.vsfDet.SetData(0, 0, "Sel")
        frmSel.ShowDialog()
        strMakeList = strList
        If strMakeList = "" Then chkMake.Checked = False
    End Sub

    Private Sub chkZone_CheckedChanged(sender As Object, e As System.EventArgs) Handles chkZone.CheckedChanged
        Dim strSQL As String

        If chkZone.Checked = False Then
            strList = ""
            strZOneList = ""
            Exit Sub
        End If

        Dim frmSel As New frmUtlMultiSelRpt

        strSQL = "Select distinct 0 as sel, ZoneId, ZoneCode from MstZone where ZoneID > 0 and "
        strSQL = strSQL & " ZoneCode Not in ('-' , '') and InActive = 'N' order by ZoneCode"
        Call frmSel.InitVsfSel(strSQL, 1, 1, True, Me, False)
        frmSel.Text = "Select [Multiple] Zone"
        frmSel.vsfDet.SetData(0, 0, "Sel")
        frmSel.ShowDialog()
        strZOneList = strList
        If strZOneList = "" Then chkZone.Checked = False
    End Sub

    Private Sub chkSupp_CheckedChanged(sender As Object, e As System.EventArgs) Handles chkSupp.CheckedChanged
        Dim strSQL As String

        If chkSupp.Checked = False Then
            strList = ""
            strSuppList = ""
            Exit Sub
        End If

        Dim frmSel As New frmUtlMultiSelRpt

        strSQL = "Select distinct 0 as sel, SuppId, SuppCode from MstSupp where SuppID > 0 and "
        strSQL = strSQL & " SuppCode Not in ('-' , '') and InActive = 'N' order by SuppCode"
        Call frmSel.InitVsfSel(strSQL, 1, 1, True, Me, False)
        frmSel.Text = "Select [Multiple] Supp"
        frmSel.vsfDet.SetData(0, 0, "Sel")
        frmSel.ShowDialog()
        strSuppList = strList
        If strSuppList = "" Then chkSupp.Checked = False
    End Sub

    Private Sub chkCustName_CheckedChanged(sender As Object, e As System.EventArgs) Handles chkCustName.CheckedChanged
        Dim strSQL As String

        If chkCustName.Checked = False Then
            strList = ""
            strCustList = ""
            Exit Sub
        End If

        Dim frmSel As New frmUtlMultiSelRpt

        strSQL = "Select distinct 0 as sel, CustId, CustCode from MstCust where CustID > 0 and "
        strSQL = strSQL & " CustCode Not in ('-' , '') and InActive = 'N' order by CustCode"
        Call frmSel.InitVsfSel(strSQL, 1, 1, True, Me, False)
        frmSel.Text = "Select [Multiple] Cust"
        frmSel.vsfDet.SetData(0, 0, "Sel")
        frmSel.ShowDialog()
        strCustList = strList
        If strCustList = "" Then chkCustName.Checked = False
    End Sub

    Private Sub btnView_Click(sender As System.Object, e As System.EventArgs) Handles btnView.Click
        Dim strSQL As String = ""
        strHeading = ""
        Dim dtSearch As DataTable, dvSearch As DataView

        If Format(dtpFromDate.Value, "dd/MM/yyyy") > Format(dtpToDate.Value, "dd/MM/yyyy") Then
            MsgBox("From date Should Be less than To Date!", vbInformation, Me.Text)
            Exit Sub
        End If

        Application.DoEvents()
        Select Case UCase(strRptName)
            Case "STOCK REPORT"
                strSQL = getStkRpt()

            Case "STOCK LEDGER REPORT"
                strSQL = getStkLedRpt()

            Case "PENDING PURCHASE ORDER"
                strSQL = getPendPORpt()

            Case "PENDING SALES ORDER"
                strSQL = getpendSORpt()

            Case "ITEM WISE SALES REGISTER"
                strSQL = getItemWiseSalesRpt()

            Case "LANDED RATE REPORT"
                strSQL = getLandedRateRpt()

            Case "PURCHASE LANDING PRICE"
                strSQL = getPurLandPriceRpt()

            Case "CUSTOMER SALES PLANNING REPORT"
                strSQL = getCustSalesRpt()

            Case "SALES INVOICE REPORT"
                strSQL = getSalesInvRpt()

            Case "SALES INVOICE DETAIL REPORT"
                strSQL = getDltSalesInvRpt()

            Case "LOCAL QUOTATION REPORT"
                strSQL = getLocQtnRpt()

            Case "IMPORT QUOTATION REPORT"
                strSQL = getImpQtnRpt()

            Case "PRODUCT WISE ORDER REPORT"
                strSQL = getProdWiseRpt()

            Case "CONSOLIDATED QUOTATION REPORT"
                strSQL = getConsolidatedQtnRpt()

            Case "SAMPLE SUBMISSION REPORT"
                strSQL = getSampleSubmissionRpt()

            Case "ORDER REPORT"
                strSQL = getOrdRpt()

            Case "DPO SHIPMENT REPORT"
                strSQL = getDPOShpRpt()

            Case "COD REPORT"
                strSQL = getCODRpt()
        End Select

        If strSQL = "" Then Exit Sub

        Call showWaitCursor()

        vsfDet.Rows.Fixed = 4
        If Not (vsfDet.Rows.Count > 0) Then
            vsfDet.Rows.Count = 4
        End If

        vsfDet.Rows.GlyphRow = 3
        vsfDet.BeginUpdate()

        If blnProcedure = False Then
            dtSearch = clsSVT.getDATATable(strSQL)
            dvSearch = New DataView(dtSearch)
            vsfDet.DataSource = dvSearch
        Else
            Dim rs As oledbDataReader = clsSVT.getDataReaderFromSQL(strSQL)
            If IsNothing(rs) Then
                rs.Close()
                MsgBox("No Records!", MsgBoxStyle.Critical, Me.Text)
                Exit Sub
            End If

            If Not rs.HasRows Then
                MsgBox("No Records!", MsgBoxStyle.Critical, Me.Text)
            End If

            dtSearch = New DataTable
            dtSearch.Load(rs)
            rs.Close()
            dvSearch = New DataView(dtSearch)
            vsfDet.DataSource = dvSearch
        End If
        vsfDet.Refresh()
        Call initVsfForHdrs(strRptName, strHeading)

        vsfDet.EndUpdate()

        If vsfDet.Rows.Count > 4 Then
            Call HideIDCols()
        End If

        vsfDet.Rows(0).TextAlignFixed = TextAlignEnum.LeftCenter
        vsfDet.Rows(1).TextAlignFixed = TextAlignEnum.LeftCenter

        'If chkMerge.Checked Then
        '    vsfDet.AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.RestrictAll
        '    For i = 0 To 10
        '        vsfDet.Cols(i).AllowMerging = True
        '    Next i
        'Else
        '    vsfDet.AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.None
        'End If

        'If UCase(strRptName) = "CUSTOMER SALES PLANNING REPORT" Then
        '    Call CustSuppRename()
        'End If
        With vsfDet
            If strRptName = "Sales Invoice Detail Report" Then
                .SubtotalPosition = C1.Win.C1FlexGrid.SubtotalPositionEnum.BelowData
                .AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.RestrictAll
                For i = 0 To 12
                    .Cols(i).AllowMerging = True
                Next i

                .Subtotal(C1.Win.C1FlexGrid.AggregateEnum.Sum, 0, -1, 17, "Grand Total")
                .Subtotal(C1.Win.C1FlexGrid.AggregateEnum.Sum, 0, -1, 19, "Grand Total")
                .Subtotal(C1.Win.C1FlexGrid.AggregateEnum.Sum, 0, -1, 20, "Grand Total")
            End If
        End With
        Call HideIDCols()
        Call showNormalCursor()
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

        Call initFormSize(Me, strRptName, False,577, 349, 0, 0)
        Me.Text = strRptName
        vsfView.Visible = False
        'Call LoadHeadings()
        Call fillFormProperties(Me)
        Call initFormVsfDet(Me, strRptName, vsfDet, "DET")
        initGridStyles(Me)

    End Sub

    Private Sub chkSalesPerson_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkUser.CheckedChanged
        Dim strSQL As String

        If chkUser.Checked = False Then
            strList = ""
            strUserList = ""
            ' txtUser.Text = ""
            Exit Sub
        End If

        Dim frmSel As New frmUtlMultiSelRpt

        strSQL = "Select distinct 0 as sel, SIID as UserID, SICODE from MstSI where  "
        strSQL = strSQL & " SICODE Not in ('-' , '') order by SICODE"
        Call frmSel.InitVsfSel(strSQL, 1, 1, True, Me, False)
        frmSel.Text = "Select [Multiple] Sales Person"
        frmSel.vsfDet.SetData(0, 0, "Sel")
        frmSel.ShowDialog()
        strUserList = strList


        Dim strUser As String = ""
        If strUserList <> "" Then
            For Each strtempuser As String In strUserList.Split(",")
                strUser = strUser & "'" & strtempuser & "',"
            Next
            strUser = Mid(strUser, 1, Len(strUser) - 1)
            strUserList = strUser
        End If
        If strUserList = "" Then chkUser.Checked = False
    End Sub

    Public Sub disableall()
        chkDate.Visible = False
        pnlDate.Visible = False
        lblItem.Visible = False
        txtItemCode.Visible = False
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
    End Sub

    Public Sub enableall()
        chkDate.Visible = True
        pnlDate.Visible = True
        lblItem.Visible = True
        txtItemCode.Visible = True
        chkItemGroup.Visible = True
        chkZone.Visible = True
        chkUser.Visible = True
        chkCustName.Visible = True
        chkSupp.Visible = True
        chkMake.Visible = True
        pnStk.Visible = True
        pnPending.Visible = True
        chkPrincipals.Visible = True
        ChkRegion.Visible = True
    End Sub

    Public Sub enableCtrl()
        Select Case UCase(strRptName)
            Case "STOCK REPORT"
                enableall()
                pnPending.Visible = False
                chkPrincipals.Visible = False
                ChkRegion.Visible = False

            Case "STOCK LEDGER REPORT"
                pnlDate.Visible = True
                chkZone.Visible = True
                chkCustName.Visible = True

            Case "PENDING PURCHASE ORDER"
                enableall()
                pnStk.Visible = False
                chkDate.Visible = False
                chkUser.Visible = False
                chkCustName.Visible = False
                chkPrincipals.Visible = False
                ChkRegion.Visible = False

            Case "PENDING SALES ORDER"
                enableall()
                pnStk.Visible = False
                chkDate.Visible = False
                chkUser.Visible = False
                chkSupp.Visible = False
                chkPrincipals.Visible = False
                ChkRegion.Visible = False

            Case "ITEM WISE SALES REGISTER"
                chkCustName.Visible = True
                chkZone.Visible = True
                pnlDate.Visible = True

            Case "LANDED RATE REPORT"
                pnlDate.Visible = True

            Case "PURCHASE LANDING PRICE"
                pnlDate.Visible = True

            Case "CUSTOMER SALES PLANNING REPORT"
                chkCustName.Visible = True
                chkMake.Visible = True
                chkItemGroup.Visible = True
                txtItemCode.Visible = True
                lblItem.Visible = True
                chkSupp.Visible = True

            Case "DPO SHIPMENT REPORT"
                chkCustName.Visible = True
                chkMake.Visible = True
                chkItemGroup.Visible = True
                txtItemCode.Visible = True
                lblItem.Visible = True
                chkSupp.Visible = True
                chkZone.Visible = True
                pnlDate.Visible = True

            Case "SALES INVOICE REPORT"
                chkCustName.Visible = True
                chkZone.Visible = True
                pnlDate.Visible = True

            Case "SALES INVOICE DETAIL REPORT"
                chkCustName.Visible = True
                pnlDate.Visible = True
                txtItemCode.Visible = True
                lblItem.Visible = True

            Case "LOCAL QUOTATION REPORT"

                chkCustName.Visible = True
                chkZone.Visible = True
                pnlDate.Visible = True
                chkMake.Visible = True
                chkItemGroup.Visible = True

            Case "IMPORT QUOTATION REPORT"

                chkCustName.Visible = True
                chkZone.Visible = True
                pnlDate.Visible = True
                chkMake.Visible = True
                chkItemGroup.Visible = True

            Case "PRODUCT WISE ORDER REPORT"
                chkCustName.Visible = True
                chkUser.Visible = True
                pnlDate.Visible = True
                chkPrincipals.Visible = True
                ChkRegion.Visible = True

            Case "CONSOLIDATED QUOTATION REPORT"

                chkCustName.Visible = True
                chkZone.Visible = True
                pnlDate.Visible = True
                chkMake.Visible = True
                chkItemGroup.Visible = True
                txtItemCode.Visible = True
                lblItem.Visible = True

            Case "SAMPLE SUBMISSION REPORT"
                chkCustName.Visible = True
                chkZone.Visible = True
                pnlDate.Visible = True

            Case "ORDER REPORT"
                pnlDate.Visible = True
                pnStk.Visible = True
                optIZ.Visible = False
                optICZ.Visible = False
                optAll1.Checked = True
                chkCustName.Visible = True
                optItem.Text = "Region Wise"
                optIC.Text = "Sales Incharge Wise"
                optIS.Text = "Principal Wise"

            Case "COD REPORT"
                pnCOD.Visible = True
                pnlDate.Visible = True
                chkDate.Visible = True
        End Select

    End Sub

    Public Function getSalesInvRpt() As String
        getSalesInvRpt = ""
        Dim strSQL As String = "", strCond As String = ""

        strSQL = " SELECT H.SINVNO AS InvNo,H.SINVDate AS InvDate,C.CustCode AS CustomerName,z.ZoneCode AS ZoneName,c.Region,"
        strSQL &= " H.CGSTAMT,H.SGSTAMT,H.IGSTAMT,H.ADDNLAMT,H.FRTAMT,H.INVVALUE,"
        strSQL &= " (CASE WHEN H.Cancelled ='Y' THEN  H.INVVALUE ELSE 0 END) AS Cancelled,"
        strSQL &= " H.INVVALUE -  (CASE WHEN H.Cancelled ='Y' THEN  H.INVVALUE ELSE 0 END) AS TotalInvValue,"
        strSQL &= " c.salesIncha AS SalesInCharge,c.SalesPer AS SalesPerson,H.STFORM,C.CUSTTYPE,"
        strSQL &= " (CASE WHEN H.EDAMT > 0 THEN 'YES' ELSE 'NO' END) AS Duty "
        strSQL &= " FROM TrnHdrSINV H"
        strSQL &= " INNER JOIN dbo.MstCUST C ON H.PartyID=C.CUSTID"
        strSQL &= " INNER JOIN dbo.MstZONE Z ON C.ZONEID=Z.ZONEID "

        ' strSQL = "Exec USP_GET_SALESINVOICE_REPORT "

        strSQL = strSQL & " where "

        If chkCustName.Checked Then
            strCond &= " C.CustID in (" & strCustList & ") and"
        End If

        If chkZone.Checked Then
            strCond &= " Z.ZONEID in (" & strZOneList & ") and"
        End If

        'If chkDate.Checked Then
        strCond &= " H.SINVDate between  " & getDateFormatInCond(dtpFromDate.Value) & " and " & getDateFormatInCond(dtpToDate.Value) & " and"
        strHeading = "From " & dtpFromDate.Value.ToString("dd-MMM-yyyy") & " to " & dtpToDate.Value.ToString("dd-MMM-yyyy")
        'End If

        If strCond <> "" Then
            strCond = Mid(strCond, 1, Len(strCond) - 3)
            ' strCond = strCond.Replace("'", "''")
            strSQL = strSQL & strCond
        Else
            strSQL = strSQL
        End If

        Return strSQL
    End Function

    Public Function getSampleSubmissionRpt() As String
        getSampleSubmissionRpt = ""
        blnProcedure = True
        Dim strSQL As String = "", strCond As String = ""
        Dim intCount As Integer = 0

        strSQL = "Exec USP_GET_SAMPLE_SUBMISSION_REPORT "

        strSQL = strSQL & "@UserID=" & gUserID & ","

        If chkCustName.Checked Then
            strSQL = strSQL & " @PartyID = " & strCustList & ","
            intCount = intCount + 1
        Else
            strSQL = strSQL & " @PartyID = 0,"
        End If


        If chkZone.Checked Then
            strSQL = strSQL & " @ZoneID = " & strZOneList & ","
            intCount = intCount + 2
        Else
            strSQL = strSQL & " @ZoneID = 0,"
        End If

        strSQL = strSQL & " @FromDate =  " & getDateFormatInCond(dtpFromDate.Value) & ","
        strSQL = strSQL & " @ToDate=" & getDateFormatInCond(dtpToDate.Value) & ","
        strHeading = "From " & dtpFromDate.Value.ToString("dd-MMM-yyyy") & " to " & dtpToDate.Value.ToString("dd-MMM-yyyy")

        Select Case intCount
            Case 1
                strSQL = strSQL & "@Type = 2"
            Case 2
                strSQL = strSQL & "@Type = 3"
            Case 3
                strSQL = strSQL & "@Type = 1"
            Case Else
                strSQL = strSQL & "@Type = 4"
        End Select


        Return strSQL
    End Function

    Public Function getConsolidatedQtnRpt() As String
        getConsolidatedQtnRpt = ""
        Dim strSQL As String = "", strCond As String = ""

        strSQL = " select * from VW_CONSOLIDATED_QTN_RPT "

        'strSQL = strSQL & 

        If chkCustName.Checked Then
            strCond &= " Custid in (" & strCustList & ") and"
        End If

        If chkMake.Checked Then
            strCond &= " MakeID in (" & strMakeList & ") and"
        End If

        If chkItemGroup.Checked Then
            strCond &= " ItemGroupId in (" & strItemGroupList & ") and"
        End If

        If chkZone.Checked Then
            strCond &= " ZONEID in (" & strZOneList & ") and"
        End If

        strItemList = ""
        If Trim(txtItemCode.Text) <> "" Then
            strItemList = clsSVT.getDataLongValue("Select ItemId from mstitem where itemcode = '" & Trim(txtItemCode.Text) & "'", 0, False)
            strCond = strCond & " ItemID in (" & GetVal(strItemList, "0") & ")" & " and"
        End If

        strCond &= " QtnDate between  " & getDateFormatInCond(dtpFromDate.Value) & " and " & getDateFormatInCond(dtpToDate.Value) & " and"
        strHeading = "From " & dtpFromDate.Value.ToString("dd-MMM-yyyy") & " to " & dtpToDate.Value.ToString("dd-MMM-yyyy")

        If strCond <> "" Then
            strCond = Mid(strCond, 1, Len(strCond) - 3)
            strSQL = strSQL & " Where " & strCond & ""
        Else
            strSQL = strSQL & ""
        End If


        Return strSQL
    End Function

    Public Function getProdWiseRpt() As String
        getProdWiseRpt = ""
        Dim strSQL As String = "", strCond As String = ""


        strSQL = " SELECT right(CONVERT(VARCHAR(12),h.CpoDate,13),9) AS  CPODate ,h.CustPoNo AS PoNo,"
        strSQL &= " CONVERT(VARCHAR(12),h.CUSTPODT,13) AS PoDate, s.suppCode AS Principal,"
        strSQL &= " c.CUSTCode AS Customer,m.MAKECODE AS Make,i.ITEMCODE,i.ITEMNAME,d.CustPart AS CustPartNo,"
        strSQL &= "  d.CommDt AS CommitDate,d.CPOQty  ,d.CostPrice AS CP,d.SellPrice AS SP, d.SELLAMT AS Value,"
        strSQL &= " d.SMCOMMAmt AS Earnings,si.SICode AS SalesIncharge,r.Regioncode,z.ZONECODE AS Zone,cr.CurCode AS Currency "
        strSQL &= " FROM TrnHdrCPO h WITH(NOLOCK) "
        strSQL &= " INNER JOIN  TrnDtlCPO d WITH(NOLOCK) ON d.CPOID = h.CPOID "
        strSQL &= " INNER JOIN MstCust c WITH(NOLOCK) ON c.CUSTID = h.CustID "
        strSQL &= " INNER JOIN MstSUPP s WITH(NOLOCK) ON s.SUPPID = h.SuppID "
        strSQL &= " INNER JOIN MstITEM I WITH(NOLOCK) ON I.ITEMID = d.ItemID "
        strSQL &= " INNER Join MstMAKE m WITH(NOLOCK) ON m.MAKEID = i.MakeID "
        strSQL &= " INNER Join MstZone z WITH(NOLOCK) ON z.ZoneID = h.ZONEID "
        strSQL &= " INNER Join MstCur cr WITH(NOLOCK) ON cr.CurID = h.CurID "
        strSQL &= " INNER Join MstRegion r WITH(NOLOCK) ON r.RegionId=c.RegionID  "
        strSQL &= " INNER Join MstSI si WITH(NOLOCK) ON Si.SIId= c.SIID"

        strSQL = strSQL & " Where "

        If chkCustName.Checked Then
            strCond &= " c.CUSTID in (" & strCustList & ") and"
        End If

        If chkMake.Checked Then
            strCond &= " m.MakeID in (" & strMakeList & ") and"
        End If

        If ChkRegion.Checked Then
            strCond &= " r.RegionID in (" & strRegionList & ") and"
        End If

        If chkPrincipals.Checked Then
            strCond &= " s.SUPPID in (" & strPrincipalList & ") and"
        End If
        If chkUser.Checked Then
            strCond &= " si.SIID in (" & strUserList & ") and"
        End If
        strCond &= " h.CpoDate between  " & getDateFormatInCond(dtpFromDate.Value) & " and " & getDateFormatInCond(dtpToDate.Value) & " and"
        strHeading = "From " & dtpFromDate.Value.ToString("dd-MMM-yyyy") & " to " & dtpToDate.Value.ToString("dd-MMM-yyyy")


        If strCond <> "" Then
            strCond = Mid(strCond, 1, Len(strCond) - 3)
            strSQL = strSQL & strCond & " ORDER BY h.CpoDate DESC "
        Else
            strSQL = strSQL & " ORDER BY h.CpoDate DESC "
        End If



        Return strSQL
    End Function

    Public Function getImpQtnRpt() As String
        getImpQtnRpt = ""
        Dim strSQL As String = "", strCond As String = ""

        strSQL = "SELECT * FROM VW_DIV_QTN_RPT_NEW "

        ' strSQL = strSQL & "@Condition='"

        If chkCustName.Checked Then
            strCond &= " CUSTID in (" & strCustList & ") and"
        End If

        If chkMake.Checked Then
            strCond &= " MakeID in (" & strMakeList & ") and"
        End If

        If chkItemGroup.Checked Then
            strCond &= " ItemGroupId in (" & strItemGroupList & ") and"
        End If

        If chkZone.Checked Then
            strCond &= " ZONEID in (" & strZOneList & ") and"
        End If

        'If chkItem.Checked Then
        '    strCond &= " ITEMID in (" & strItemList & ") and"
        'End If

        'If chkDate.Checked Then
        strCond &= " QtnDate between  " & getDateFormatInCond(dtpFromDate.Value) & " and " & getDateFormatInCond(dtpToDate.Value) & " and"
        strHeading = "From " & dtpFromDate.Value.ToString("dd-MMM-yyyy") & " to " & dtpToDate.Value.ToString("dd-MMM-yyyy")
        'End If


        If strCond <> "" Then
            strCond = Mid(strCond, 1, Len(strCond) - 3)
            '  strCond = strCond.Replace("'", "''")
            strSQL = strSQL & " Where " & strCond & ""
        Else
            strSQL = strSQL & ""
        End If


        Return strSQL
    End Function

    Public Function getLocQtnRpt() As String
        getLocQtnRpt = ""
        Dim strSQL As String = "", strCond As String = ""

        strSQL = "SELECT * FROM VW_QTN_RPT_NEW "

        '  strSQL = strSQL & "@Condition='"

        If chkCustName.Checked Then
            strCond &= " CUSTID in (" & strCustList & ") and"
        End If

        If chkMake.Checked Then
            strCond &= " MakeID in (" & strMakeList & ") and"
        End If

        If chkItemGroup.Checked Then
            strCond &= " ItemGroupId in (" & strItemGroupList & ") and"
        End If

        If chkZone.Checked Then
            strCond &= " ZONEID in (" & strZOneList & ") and"
        End If

        'If chkItem.Checked Then
        '    strCond &= " ITEMID in (" & strItemList & ") and"
        'End If

        'If chkDate.Checked Then
        strCond &= " QtnDate between  " & getDateFormatInCond(dtpFromDate.Value) & " and " & getDateFormatInCond(dtpToDate.Value) & " and"
        strHeading = "From " & dtpFromDate.Value.ToString("dd-MMM-yyyy") & " to " & dtpToDate.Value.ToString("dd-MMM-yyyy")
        'End If


        If strCond <> "" Then
            strCond = Mid(strCond, 1, Len(strCond) - 3)
            ' strCond = strCond.Replace("'", "''")
            strSQL = strSQL & " Where " & strCond & ""
        Else
            strSQL = strSQL & ""
        End If


        Return strSQL
    End Function

    Public Function getDltSalesInvRpt() As String
        getDltSalesInvRpt = ""
        Dim strSQL As String = "", strCond As String = ""

        strSQL = "SELECT H.INVNO AS InvNo,(case when substring(h.PrintInvNo,3,1)='/' then '' else substring(h.PrintInvNo,3,1) End) as Prefix ,"
        strSQL &= " CONVERT(VARCHAR(12), H.INVDate,13) AS InvDate, so.CUSTREFNO AS CustRefNo, "
        strSQL &= " CONVERT(VARCHAR(12), so.CUSTREFDATE,13) AS CustRefDate, SIcode AS SalesInCharge, "
        strSQL &= " sp.SPcode AS SalesPerson, z.ZONECODE AS ZoneName, Regioncode, C.CustCode AS CustName, PMTTERMS,"
        strSQL &= " H.INVVALUE, ig.ITEMGROUPCODE AS ITEMGROUPNAME, i.ITEMCODE, I.ITEMNAME, I.TARIFFNO, D.CUSTPARTNo, "
        strSQL &= " D.INVQTY AS InvQty, CONVERT(VARCHAR(12),D.SCHDATE,13) AS SCHDATE, D.INVRATE AS Rate, D.INVAMT AS Amount,"
        strSQL &= " M.MAKECODE,  MODETRN,  FRTAmt, D.LOTNO, D.BoxNo "
        strSQL &= " FROM TrnHdrINV H "
        strSQL &= " INNER JOIN MstCUST C ON H.CustID=C.CUSTID "
        strSQL &= " INNER JOIN TrnDtlINV D ON D.INVID=H.INVID "
        strSQL &= " INNER JOIN MstSI si ON si.SIID = C.SIID "
        strSQL &= " INNER JOIN MstITEM I ON I.ITEMID=D.ItemID"
        strSQL &= " INNER JOIN MstSP sp ON sp.SpID = C.SpID "
        strSQL &= " INNER JOIN MstMAKE M ON M.MAKEID=I.MakeID"
        strSQL &= " INNER JOIN MstITEMGROUP ig ON ig.ITEMGROUPID = i.ITEMGROUPID"
        strSQL &= " INNER JOIN MstZONE z ON z.ZONEID = C.ZONEID"
        strSQL &= " INNER JOIN TrnHdrSO so ON so.SOID = d.SOID "
        strSQL &= " INNER JOIN MstRegion r ON r.RegionID = C.RegionID  "

        strSQL = strSQL & " where "

        If chkCustName.Checked Then
            strCond &= " C.CUSTID in (" & strCustList & ") and"
        End If

        strItemList = ""
        If Trim(txtItemCode.Text) <> "" Then
            strItemList = clsSVT.getDataLongValue("Select ItemId from mstitem where itemcode = '" & Trim(txtItemCode.Text) & "'", 0, False)
            strCond = strCond & " i.ItemID in (" & GetVal(strItemList, "0") & ")" & " and"
        End If
        strCond &= " H.INVDate between  " & getDateFormatInCond(dtpFromDate.Value) & " and " & getDateFormatInCond(dtpToDate.Value) & " and"
        strHeading = "From " & dtpFromDate.Value.ToString("dd-MMM-yyyy") & " to " & dtpToDate.Value.ToString("dd-MMM-yyyy")


        If strCond <> "" Then
            strCond = Mid(strCond, 1, Len(strCond) - 3)
            '  strCond = strCond.Replace("'", "''")
            strSQL = strSQL & strCond
        Else
            strSQL = strSQL
        End If


        Return strSQL
    End Function

    Public Function getDPOShpRpt() As String
        getDPOShpRpt = ""
        Dim strSQL As String = "", strCond As String = ""

        strSQL = "Select suppcode as Supplier, c.CustCode as CustomerName, SEPONo ShippingNo, SEPODate ShippingDt, DPODate, hp.DPONO,  h.Invno, h.InvDate, hp.CustRef, "
        strSQL &= " i.ItemCode, i.ItemName, m.MakeCode, ItemGroupCode,   "
        strSQL &= " dp.CustPartNo,   dp.COMMDate as CommitedDt, dp.SchDate, dp.SELLRATE, d.SEPORate as Rate, dp.DPOQty, d.SEPOQty as ShippingQty, d.SEPOAmount as Amount, "
        strSQL &= " hp.RMKS as Remarks "
        strSQL &= " From TrnHdrSEPO h "
        strSQL &= " Join TrnDtlSEPO d on h.SEPOID=d.SEPOID "
        strSQL &= " Join TrnDtlDPO dp on d.DPOID=dp.DPOID and d.ItemID=dp.ItemID and d.CustPartNo=dp.CustPartNo and d.SchDate=dp.SchDate "
        strSQL &= " Join TrnHdrDPO hp on dp.DPOID=hp.DPOID "
        strSQL &= " Join mstitem i on i.itemid = d.itemid "
        strSQL &= " join mstmake m on i.makeid = m.makeid "
        strSQL &= " join mstitemgroup g on i.itemgroupid = g.itemgroupid "
        strSQL &= " join mstSupp s on hp.SuppID = s.suppid "
        strSQL &= " join MstCust c on  hp.CustID = c.CustID "

        strSQL &= " Where 1=1 "
        strSQL &= " and h.CompId=" & IntCompID

        strSQL = strSQL & " and SEPODate between " & getDateFormatInCond(dtpFromDate.Value) & " and " & getDateFormatInCond(dtpToDate.Value)
        strHeading = "From " & dtpFromDate.Value.ToString("dd-MMM-yyyy") & " to " & dtpToDate.Value.ToString("dd-MMM-yyyy")

        If chkCustName.Checked Then
            strSQL = strSQL & " and c.CustID in (" & strCustList & ")"
        End If
        If chkItemGroup.Checked Then
            strSQL = strSQL & " and i.ItemGroupID in (" & strItemGroupList & ")"
        End If
        If chkMake.Checked Then
            strSQL = strSQL & " and i.MakeID in (" & strMakeList & ")"
        End If
        If chkSupp.Checked Then
            strSQL = strSQL & " and s.suppid in (" & strSuppList & ")"
        End If

        strItemList = ""
        If Trim(txtItemCode.Text) <> "" Then
            strItemList = clsSVT.getDataLongValue("Select ItemId from mstitem where itemcode = '" & Trim(txtItemCode.Text) & "'", 0, False)
            strSQL = strSQL & " and i.ItemID in (" & GetVal(strItemList, "0") & ")"
        End If

        strSQL = strSQL & " Order by 1,2,5,6,3,4"

        Return strSQL
    End Function

    Public Function getCODRpt() As String
        getCODRpt = ""
        Dim strSQL As String = "", strCond As String = ""

        strSQL = " select InvNo, InvDate,GrpText as GroupText,CustCode as Customer, BAdd1 as Address1, BAdd2 as Address2, BAdd3 as Add3, "
        strSQL = strSQL & " iif(City='','',City + ' ') + iif(State='','',State + ' ') + iif(PinCode='','',PinCode + ' ')+ "
        strSQL = strSQL & " iif(Country='','',Country + ' ') as Address4,RegionCode, InvValue,"
        strSQL = strSQL & "  MarkRmks as MarkingRmks,codNo as DocketNo, codDate as DocketDate,CODRemarks,CODChqDtls "
        strSQL = strSQL & "  from TrnHdrInv s, MstCust c ,MstRegion r"
        strSQL = strSQL & " Where 1=1"
        strSQL = strSQL & " and s.CustID=c.CustID"
        strSQL = strSQL & "  and r.regionID=c.RegionId"
        strSQL = strSQL & " and codinv = 'Y'"
        If chkDate.Checked Then
            strSQL = strSQL & " and INVDate between " & getDateFormatInCond(dtpFromDate.Value) & " and " & getDateFormatInCond(dtpToDate.Value)
            strHeading = "From " & dtpFromDate.Value.ToString("dd-MMM-yyyy") & " to " & dtpToDate.Value.ToString("dd-MMM-yyyy")
        End If
        If OptPendCOd.Checked Then
            strSQL = strSQL & " and codrcvd = 'N'"
            strSQL = strSQL & " and cancelledCOD<>'Y'"
        End If

        If optCODCancel.Checked Then
            strSQL = strSQL & " and cancelledCOD='Y'"
        End If

        If optCODRcptList.Checked Then
            strSQL = strSQL & " and codrcvd = 'Y'"
        End If

        strSQL = strSQL & " order by 1"

        Return strSQL
    End Function

    Public Function getItemWiseSalesRpt() As String
        blnProcedure = True
        getItemWiseSalesRpt = ""
        Dim strSQL As String = "", strCond As String = ""
        strHeading = "From " & dtpFromDate.Value.ToString("dd-MMM-yyyy") & " to " & dtpToDate.Value.ToString("dd-MMM-yyyy")
        strSQL = "Exec SMC_ITEMWISESALESREG "

        strSQL = strSQL & "@mFdt='" & Format(dtpFromDate.Value, "yyyy-MM-dd") & "',"
        strSQL = strSQL & "@mTdt='" & Format(dtpToDate.Value, "yyyy-MM-dd") & "',"

        strSQL = strSQL & "@Condition= ' INVDate between  ''" & Format(dtpFromDate.Value, "yyyy-MM-dd") & "'' and ''" & Format(dtpToDate.Value, "yyyy-MM-dd") & "''"

        If chkCustName.Checked Then
            strSQL &= " and C.CustID in (" & strCustList & ")"
        End If

        If chkZone.Checked Then
            strSQL &= " and Z.ZONEID in (" & strZOneList & ")"
        End If

        strSQL &= "'"

        Return strSQL
    End Function

    Public Function getCustSalesRpt() As String
        blnProcedure = True
        getCustSalesRpt = ""
        Dim strSQL As String = "", strCond As String = "", strCond1 As String = ""

        Dim blnCustChk As Boolean = False
        Dim blnSuppChk As Boolean = False

        strSQL = "Exec USP_GET_PENDING_PO_SO_NEW "

        strSQL = strSQL & "@UserName='" & gUserCode & "',"
        strSQL = strSQL & "@mFromDT=" & getDateFormatInCond(dtpFromDate.Value) & ","
        strSQL = strSQL & "@Condition='"

        If chkCustName.Checked Then
            '  strCond &= " CustID in (" & strCustList & ") and"
            strCond1 &= " CustID in (" & strCustList & ") and"
            blnCustChk = True
        End If

        If chkMake.Checked Then
            strCond &= " MakeID in (" & strMakeList & ") and"
            strCond1 &= " MakeID in (" & strMakeList & ") and"
        End If

        If chkItemGroup.Checked Then
            strCond &= " ItemGroupId in (" & strItemGroupList & ") and"
            strCond1 &= " ItemGroupId in (" & strItemGroupList & ") and"
        End If

        If chkSupp.Checked Then
            strCond &= " SUPPID in (" & strSuppList & ") and"
            '  strCond1 &= " SUPPID in (" & strSuppList & ") and"
            blnSuppChk = True
        End If

        strItemList = ""
        If Trim(txtItemCode.Text) <> "" Then
            strItemList = clsSVT.getDataLongValue("Select ItemId from mstitem where itemcode = '" & Trim(txtItemCode.Text) & "'", 0, False)
            strCond = strCond & " ItemID in (" & GetVal(strItemList, "0") & ")" & " and"
            strCond1 = strCond1 & " ItemID in (" & GetVal(strItemList, "0") & ")" & " and"
        End If

        If strCond <> "" Then
            strCond = Mid(strCond, 1, Len(strCond) - 3)
            strCond = strCond.Replace("'", "''")
            strSQL = strSQL & " Where " & strCond & "',"
        Else
            strSQL = strSQL & "',"
        End If

        strSQL = strSQL & "@Condition1='"
        If strCond1 <> "" Then
            strCond1 = Mid(strCond1, 1, Len(strCond1) - 3)
            strCond1 = strCond1.Replace("'", "''")
            strSQL = strSQL & " Where " & strCond1 & "',"
        Else
            strSQL = strSQL & "',"
        End If

        If blnCustChk = False Then
            strSQL = strSQL & "@CustID='0',"
        Else
            strSQL = strSQL & "@CustID='1',"
        End If

        If blnSuppChk = False Then
            strSQL = strSQL & "@SuppID='0'"
        Else
            strSQL = strSQL & "@SuppID='1'"
        End If


        Return strSQL
    End Function

    Public Function getPurLandPriceRpt() As String
        blnProcedure = True
        getPurLandPriceRpt = ""
        Dim strSQL As String = "", strCond As String = ""

        strSQL = "Exec USP_GET_PENDING_PO_REPORT_NEW "

        strSQL = strSQL & "@Condition='"

        If chkCustName.Checked Then
            strCond &= " s.SUPPID in (" & strCustList & ") and"
        End If

        If chkMake.Checked Then
            strCond &= " m.MakeID in (" & strMakeList & ") and"
        End If

        If chkItemGroup.Checked Then
            strCond &= " g.ItemGroupId in (" & strItemGroupList & ") and"
        End If

        If chkZone.Checked Then
            strCond &= " z.ZONEID in (" & strZOneList & ") and"
        End If

        'If chkItem.Checked Then
        '    strCond &= " i.ITEMID in (" & strItemList & ") and"
        'End If

        'If chkDate.Checked Then
        strCond &= " PODate between  " & getDateFormatInCond(dtpFromDate.Value) & " and " & getDateFormatInCond(dtpToDate.Value) & " and"
        strHeading = "Pending Result From " & dtpFromDate.Value.ToString("dd-MMM-yyyy") & " to " & dtpToDate.Value.ToString("dd-MMM-yyyy")
        'End If


        If strCond <> "" Then
            strCond = Mid(strCond, 1, Len(strCond) - 3)
            strCond = strCond.Replace("'", "''")
            strSQL = strSQL & " Where " & strCond & "'"
        Else
            strSQL = strSQL & "'"
        End If

        '     MsgBox("Report Not Designed", MsgBoxStyle.Information, Me.Text)


        Return strSQL
    End Function

    Public Function getLandedRateRpt() As String
        getLandedRateRpt = ""
        blnProcedure = True
        Dim strSQL As String = "", strCond As String = ""

        strSQL = "Exec USP_GET_LANDED_RATE_REPORT "
        strSQL = strSQL & "@mUserName='" & gUserCode & "',"
        strSQL = strSQL & "@mFromDT=" & getDateFormatInCond(dtpFromDate.Value) & ","
        strSQL = strSQL & "@mToDT=" & getDateFormatInCond(dtpToDate.Value)
        strHeading = "From " & dtpFromDate.Value.ToString("dd-MMM-yyyy") & " to " & dtpToDate.Value.ToString("dd-MMM-yyyy")
        Return strSQL

    End Function

    Public Function getpendSORpt() As String
        getpendSORpt = ""
        Dim strSQL As String = "", strCond As String = ""
        blnProcedure = True

        strSQL = " Select SONo, SoDate, CustRefNo, CustRefDate, CustCode As CustomerName, SPCode,  ItemCode, ItemName, ITEMGROUPCODE As ItemgroupName, CustPartNo,"
        strSQL &= " ZoneCode AS ZoneName, RegionCode, SchDate, SOQty, CumInvQty InvQy, SOQty - CumInvQty As PendQty, SoRate, MakeCode As MakeName ,"
        strSQL &= " BQOH as Stock"
        strSQL &= " From TrnHdrSO h"
        strSQL &= " Left Join TrnDtlSO d on h.SOID=d.SOID"
        strSQL &= " Left Join MSTCUST c ON h.CUSTID=c.CustID"
        strSQL &= " Left Join MstITEM I on d.ItemID = I.ItemId"
        strSQL &= " Left Join mstItemgroup g on I.ItemGroupId = g.ITEMGROUPID"
        strSQL &= " Left Join MstMAKE M on I.MAKEid=M.makeid"
        strSQL &= " Left Join MstSP sp on c.spid=sp.spid"
        strSQL &= " Left Join MstZone Z on c.ZONEID=Z.ZONEID"
        strSQL &= " Left Join MstRegion r on c.RegionId=r.RegionID"
        strSQL &= " Left Join TrnQOH q on d.ItemId=q.ItemID"
        strSQL &= " where SOQty - CumInvQty > 0"
        strSQL &= " and h.CompId=" & IntCompID
        strSQL &= " And h.Cancelled='N' and"


        If chkCustName.Checked Then
            strCond &= " c.CustID in (" & strCustList & ") And"
        End If

        If chkMake.Checked Then
            strCond &= " m.MakeCode in (Select MakeCode From MstMake Where MakeID In (" & strMakeList & ")) And"
        End If

        If chkItemGroup.Checked Then
            strCond &= " g.ItemGroupCode In (Select ItemGroupCode from MstItemGroup where ItemGroupID In (" & strItemGroupList & ")) And"
        End If

        If chkZone.Checked Then
            strCond &= " z.ZONECode In (Select ZoneCode from MstZone where ZoneID In (" & strZOneList & ")) And"
        End If

        strItemList = ""
        If Trim(txtItemCode.Text) <> "" Then
            strItemList = clsSVT.getDataLongValue("Select ItemId from mstitem where itemcode = '" & Trim(txtItemCode.Text) & "'", 0, False)
            strCond = strCond & " i.ItemID in (" & GetVal(strItemList, "0") & ")" & " and"
        End If

        'If chkDate.Checked Then
        strCond &= " sodate between  " & getDateFormatInCond(dtpFromDate.Value) & " and " & getDateFormatInCond(dtpToDate.Value)
        'End If

        If optAll.Checked = False Then
            strCond &= " and SOQty - CumInvQty  > 0  "
            strHeading = "Pending From " & dtpFromDate.Value.ToString("dd-MMM-yyyy") & " to " & dtpToDate.Value.ToString("dd-MMM-yyyy")
        Else
            strHeading = "From " & dtpFromDate.Value.ToString("dd-MMM-yyyy") & " to " & dtpToDate.Value.ToString("dd-MMM-yyyy")
        End If

        If strCond <> "" Then
            ' strCond = Mid(strCond, 1, Len(strCond) - 3)
            '  strCond = strCond.Replace("'", "''")
            strSQL = strSQL & strCond
        End If

        Return strSQL & " order by Sodate,Sono"
    End Function

    Public Function getStkLedRpt() As String
        getStkLedRpt = ""
        Dim strSQL As String = "", strCond As String = ""
        blnProcedure = True
        strSQL = "Exec USP_GET_STOCK_LEDGER_REPORT_NEW "

        If chkCustName.Checked = True And chkZone.Checked = True Then
            strSQL &= "@Type = 1,"
        ElseIf chkCustName.Checked = True And chkZone.Checked = False Then
            strSQL &= "@Type = 2,"
        ElseIf chkCustName.Checked = False And chkZone.Checked = True Then
            strSQL &= "@Type = 3,"
        Else
            strSQL &= "@Type = 4,"
        End If

        If chkCustName.Checked Then
            strSQL &= "@CustID = " & GetVal(strCustList, "0") & ","
        Else
            strSQL &= "@CustID = 0,"
        End If

        If chkZone.Checked Then
            strSQL &= "@ZoneID = " & GetVal(strZOneList, "0") & ","
        Else
            strSQL &= "@ZoneID = 0,"
        End If
        strHeading = "From " & dtpFromDate.Value.ToString("dd-MMM-yyyy") & " to " & dtpToDate.Value.ToString("dd-MMM-yyyy")
        strSQL &= "@FromDate = " & getDateFormatInCond(dtpFromDate.Value) & ","
        strSQL &= "@ToDate = " & getDateFormatInCond(dtpToDate.Value) & ","
        ' If chkDate.Checked Then
        strSQL &= "@IsDateRange = 'Y',"
        'Else
        'strSQL &= "@IsDateRange = 'N',"
        'End If

        strSQL &= "@UserID = " & gUserID

        Return strSQL
    End Function

    Public Function getPendPORpt() As String
        getPendPORpt = ""
        Dim strSQL As String, strCond As String = ""

        strSQL = "SELECT * FROM VW_PENDING_PO_REPORT_NEW "

        strSQL = strSQL & " Where "

        If chkSupp.Checked Then
            strCond &= " SUPPID in (" & strSuppList & ") and"
        End If

        If chkMake.Checked Then
            strCond &= " MakeID in (" & strMakeList & ") and"
        End If

        If chkItemGroup.Checked Then
            strCond &= " ItemGroupId in (" & strItemGroupList & ") and"
        End If

        If chkZone.Checked Then
            strCond &= " ZONEID in (" & strZOneList & ") and"
        End If

        If optAll.Checked = False Then
            strCond &= " PendQty > 0 and"
            strHeading = "Pending From " & dtpFromDate.Value.ToString("dd-MMM-yyyy") & " to " & dtpToDate.Value.ToString("dd-MMM-yyyy")
        Else
            strHeading = "From " & dtpFromDate.Value.ToString("dd-MMM-yyyy") & " to " & dtpToDate.Value.ToString("dd-MMM-yyyy")
        End If



        strItemList = ""
        If Trim(txtItemCode.Text) <> "" Then
            strItemList = clsSVT.getDataLongValue("Select ItemId from mstitem where itemcode = '" & Trim(txtItemCode.Text) & "'", 0, False)
            strCond = strCond & " ItemID in (" & GetVal(strItemList, "0") & ")" & " and"
        End If
        strCond &= " BillDate between  " & getDateFormatInCond(dtpFromDate.Value) & " and " & getDateFormatInCond(dtpToDate.Value) & " and"
        If strCond <> "" Then
            strCond = Mid(strCond, 1, Len(strCond) - 3)
            strSQL = strSQL & strCond
        End If


        Return strSQL
    End Function

    Public Function getStkRpt() As String
        getStkRpt = ""
        Dim strSQL As String, strCond As String = ""

        'strSQL = "Exec mSp_StkBal '" & gUserCode & "', " & getDateFormatInCond(getSystemDate())

        'If Not clsSVT.execSQLStatement(strSQL) Then
        '    MsgBox("Stock Report Loading Failed...", MsgBoxStyle.Exclamation, Me.Text)
        '    Exit Function
        'End If
        blnProcedure = True
        strSQL = "Exec USP_GET_STOCK_REPORT_FROM_VIEW_NEW "

        strSQL &= "@UserCode = '" & gUserCode & "',"
        strSQL &= "@mTdt = " & getDateFormatInCond(getSystemDate()) & ","
        strSQL &= "@UserID = " & gUserID & ","
        strSQL &= "@FromDate = " & getDateFormatInCond(getSystemDate()) & ","

        If optItem.Checked Then
            strSQL &= "@Type = 1,"
        ElseIf optIZ.Checked Then
            strSQL &= "@Type = 2,"
        ElseIf optIC.Checked Then
            strSQL &= "@Type = 3,"
        ElseIf optIS.Checked Then
            strSQL &= "@Type = 4,"
        ElseIf optICZ.Checked Then
            strSQL &= "@Type = 5,"
        Else
            strSQL &= "@Type = 6,"
        End If

        'If chkDate.Checked Then
        strSQL &= "@IsDateRange = 'Y',"
        ' End If
        strHeading = "From " & dtpFromDate.Value.ToString("dd-MMM-yyyy") & " to " & dtpToDate.Value.ToString("dd-MMM-yyyy")
        strCond = strCond & " r.DocDate between " & getDateFormatInCond(dtpFromDate.Value) & " and " & getDateFormatInCond(dtpToDate.Value) & " and"

        If chkCustName.Checked Then
            strCond = strCond & " c.custid in (" & GetVal(strCustList, "0") & ")" & " and"
        End If
        If chkZone.Checked Then
            strCond = strCond & " Z.ZoneID in (" & GetVal(strZOneList, "0") & ")" & " and"
        End If
        If chkItemGroup.Checked Then
            strCond = strCond & " g.ItemGroupID in (" & GetVal(strItemGroupList, "0") & ")" & " and"
        End If
        If chkMake.Checked Then
            strCond = strCond & " e.MakeID in (" & GetVal(strMakeList, "0") & ")" & " and"
        End If
        If chkUser.Checked Then
            strCond = strCond & " c.SalesPer in (" & GetVal(strUserList, "0") & ")" & " and"
        End If
        strItemList = ""
        If Trim(txtItemCode.Text) <> "" Then
            strItemList = clsSVT.getDataLongValue("Select ItemId from mstitem where itemcode = '" & Trim(txtItemCode.Text) & "'", 0, False)
            strCond = strCond & " i.ItemID in (" & GetVal(strItemList, "0") & ")" & " and"
        End If

        If Trim(strCond) <> "" Then
            strCond = strCond.Replace("'", "''")
            strCond = Mid(strCond, 1, Len(strCond) - 3)
            strSQL = strSQL & "@Condition='" & strCond & "'"
        End If
        Return strSQL
    End Function

    Public Function getOrdRpt() As String
        getOrdRpt = ""
        Dim strSQL As String = "", strCond As String = "", strGroup As String = ""

        blnProcedure = True
        ' strSQL = "Exec USP_GET_ORDERS_REPORT "

        If optAll1.Checked = True Then
            strSQL = "SELECT c.CustCode AS Customer,r.Regioncode as RegionCode,z.ZoneCOde AS Zone,"
            strSQL &= " cr.CurCode AS Currency,si.SICode AS SalesIncharge,h.CustPONo AS [PO Ref],"
            strSQL &= " CONVERT(VARCHAR(12),h.CUSTPoDT,13) AS PODate,s.SuppCode AS  Principals ,"
            strSQL &= " h.POVal AS [Value IN USD],SUM(SMCommAmt) AS Profit"
            strSQL &= " from TrnhDrCPO h WITH(NOLOCK)"
            strSQL &= " INNER JOIN MstCust c WITH(NOLOCK) ON c.CustID = h.CustId"
            strSQL &= " INNER JOIN MstZone z WITH(NOLOCK) ON z.ZoneID = h.ZoneID"
            strSQL &= " INNER JOIN MstSUpp s WITH(NOLOCK) ON s.SuppID =h.SuppID"
            strSQL &= " INNER JOIN TrnDtlCPO d WITH(NOLOCK)  ON d.CPOID = h.CPOID"
            strSQL &= " INNER JOIN MstRegion r WITH(NOLOCK)  ON r.RegionId = c.RegionID "
            strSQL &= " INNER JOIN MstSI si WITH(NOLOCK)  ON si.SIId = c.SIID "
            strSQL &= " INNER JOIN MSTCUR cr WITH(NOLOCK)  ON cr.CurID = h.CurID "
            strGroup = "GROUP BY c.CustCode,r.RegionCode,z.ZoneCOde,si.SICode,h.CustPONo,h.CUSTPoDT,s.SuppCode,h.POVal,cr.CurCode  ORDER BY c.CustCode"
        End If
        If optItem.Checked = True Then
            strSQL = " SELECT r.RegionCode,cr.CurCode AS Currency,SUM(h.POVal) AS [Total Booking],SUM(SMCommAmt) AS Profit"
            strSQL &= " From TrnhDrCPO h WITH(NOLOCK) "
            strSQL &= " INNER JOIN MstCust c WITH(NOLOCK) ON c.CustID = h.CustID"
            strSQL &= " INNER JOIN TrnDtlCPO d WITH(NOLOCK) ON d.CPOID = h.CPOID "
            strSQL &= " INNER JOIN MSTCUR cr WITH(NOLOCK)  ON cr.CurID = h.CurID "
            strSQL &= " INNER JOIN MSTRegion r WITH(NOLOCK)  ON r.RegionID = c.RegionID "
            strGroup = "GROUP BY r.RegionCode,cr.CurCode  ORDER BY r.RegionCode"
        End If
        If optIC.Checked = True Then
            strSQL = " SELECT si.SICode AS SalesIncharge,cr.CurCode AS Currency,SUM(h.POVal) AS [Total Booking],SUM(SMCommAmt) AS Profit"
            strSQL &= " FROM TrnhDrCPO h WITH(NOLOCK)"
            strSQL &= " INNER JOIN MstCust c WITH(NOLOCK) ON c.CustID = h.CustID"
            strSQL &= " INNER JOIN TrnDtlCPO d WITH(NOLOCK) ON d.CPOID = h.CPOID "
            strSQL &= " INNER JOIN MSTSI si WITH(NOLOCK)  ON Si.SIID = c.SIID"
            strSQL &= " INNER JOIN MSTCUR cr WITH(NOLOCK)  ON cr.CurID = h.CurID "
            strGroup = " GROUP BY si.SICODe,cr.CurCode ORDER BY si.SICODE "
        End If
        If optIS.Checked = True Then
            strSQL = " SELECT 'Booking For : '+  s.SuppCode AS  Principal,cr.CurCode AS Currency ,SUM(h.POVal) AS [Total Booking],SUM(SMCommAmt) AS Profit"
            strSQL &= " FROM TrnhDrCPO h WITH(NOLOCK) "
            strSQL &= " INNER JOIN MstSUpp s WITH(NOLOCK) ON s.SuppID =h.SuppID"
            strSQL &= " INNER JOIN TrnDtlCPO  d WITH(NOLOCK) ON d.CPOID = h.CPOID "
            strSQL &= " INNER JOIN MSTCUR cr WITH(NOLOCK)  ON cr.CurID = h.CurID"
            strGroup = " GROUP BY s.SuppCode,cr.CurCode ORDER BY s.SuppCode"
        End If

        strCond &= "  h.CPODate between  " & getDateFormatInCond(dtpFromDate.Value) & " and " & getDateFormatInCond(dtpToDate.Value)
        strHeading = "From " & dtpFromDate.Value.ToString("dd-MMM-yyyy") & " to " & dtpToDate.Value.ToString("dd-MMM-yyyy")

        If chkCustName.Checked Then
            strCond &= " and c.CustID in (" & strCustList & ") "
        End If

        If ChkRegion.Checked Then
            strCond &= " and c.Region in (" & strRegionList & ") "
        End If

        If chkUser.Checked Then
            strCond &= " and c.salesincha in (" & strUserList & ") "
        End If

        If chkPrincipals.Checked Then
            strCond &= " and s.SuppCode in (" & strPrincipalList & ") "
        End If

        strCond &= " and h.CompId=" & IntCompID

        If strCond <> "" Then
            strSQL = strSQL & " Where " & strCond
        End If

        strSQL &= strGroup


        Return strSQL
    End Function

    Private Sub ChkRegion_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles ChkRegion.CheckedChanged
        Dim strSQL As String

        If ChkRegion.Checked = False Then
            strList = ""
            strRegionList = ""
            Exit Sub
        End If

        Dim frmSel As New frmUtlMultiSelRpt

        strSQL = "Select distinct 0 as sel, RegionID as RegionId, RegionCode from MstRegion where RegionID > 0 and "
        strSQL = strSQL & " Regioncode Not in ('-' , '') and InActive = 'N' order by Regioncode"
        Call frmSel.InitVsfSel(strSQL, 1, 1, True, Me, False)
        frmSel.Text = "Select [Multiple] Region"
        frmSel.vsfDet.SetData(0, 0, "Sel")
        frmSel.ShowDialog()
        strRegionList = strList

        Dim strRegion As String = ""
        If strRegionList <> "" Then
            For Each strtempregion As String In strRegionList.Split(",")
                strRegion = strRegion & "'" & strtempregion & "',"
            Next
            strRegion = Mid(strRegion, 1, Len(strRegion) - 1)
            strRegionList = strRegion
            ' txtUser.Text = txtUser.Text.Replace("'", "")
        End If
        If strRegionList = "" Then ChkRegion.Checked = False
    End Sub

    Private Sub chkPrincipals_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkPrincipals.CheckedChanged
        Dim strSQL As String

        If chkPrincipals.Checked = False Then
            strList = ""
            strPrincipalList = ""
            Exit Sub
        End If

        Dim frmSel As New frmUtlMultiSelRpt

        strSQL = "Select distinct 0 as sel, SuppId, SuppCode from MstSupp where SuppID > 0 and "
        strSQL = strSQL & " SuppCode Not in ('-' , '') and InActive = 'N' order by SuppCode"
        Call frmSel.InitVsfSel(strSQL, 1, 1, True, Me, False)
        frmSel.Text = "Select [Multiple] Zone"
        frmSel.vsfDet.SetData(0, 0, "Sel")
        frmSel.ShowDialog()
        strPrincipalList = strList
        If strPrincipalList = "" Then chkPrincipals.Checked = False
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

    Private Sub CustSuppRename()
        With vsfDet
            Dim intSupp As Int32
            Dim intCust As Int32
            If .Cols.Count < 1 Then Exit Sub
            If .Rows.Count < 2 Then Exit Sub
            For i As Int32 = 0 To .Cols.Count - 1
                If UCase(GetVal(.GetData(0, i), "")) = "CUSTOMERNAME" Then
                    intCust = i
                ElseIf UCase(GetVal(.GetData(0, i), "")) = "SUPPLIERNAME" Then
                    intSupp = i
                Else

                End If
            Next

            For i As Int32 = 1 To .Rows.Count - 1
                If GetVal(.GetData(i, intCust), "").ToString = "&#x20;" Then
                    .SetData(i, intCust, "")
                End If
                If GetVal(.GetData(i, intSupp), "") = "&#x20;" Then
                    .SetData(i, intSupp, "")
                End If
            Next
        End With
    End Sub

    Private Sub LoadHeadings()
        'vsfDet.Rows.Fixed = 4
        'vsfDet.Rows.GlyphRow = 3
    End Sub

    Private Sub initVsfForHdrs(ByVal argHdrLine1 As String, ByVal argHdrLine2 As String)
        Dim i As Integer

        With vsfDet
            For i = 0 To .Cols.Count - 1
                .SetData(3, i, .GetData(0, i))

                .SetData(0, i, gSettings.gCompName & ",  " & gSettings.gCompAdd1 & gSettings.gCompAdd2 & gSettings.gCompAdd3 & _
                          gSettings.gCompAdd4 & gSettings.gCompAdd5)
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

        End With
    End Sub

    Private Sub vsfDet_BeforeEdit(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfDet.BeforeEdit
        e.Cancel = True
    End Sub

    Private Sub txtItemCode_KeyUp(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles txtItemCode.KeyUp
        If e.KeyCode = Keys.Escape Then
            vsfView.Visible = False
            txtItemCode.Focus()
        End If

        If e.KeyCode = Keys.Enter Or e.KeyCode = Keys.Return Then
            If vsfView.Visible Then
                If vsfView.Rows.Count > 1 Then
                    txtItemCode.Text = vsfView.GetData(vsfView.Row, 1)
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

    Private Sub txtItemCode_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtItemCode.TextChanged
        Call applyFilterFromGrid()
    End Sub

    Private Sub vsfView_BeforeEdit(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfView.BeforeEdit
        e.Cancel = True
    End Sub

    Private Sub vsfView_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles vsfView.KeyUp
        If e.KeyCode = Keys.Escape Then
            txtItemCode.Focus()
            vsfView.Visible = False
        End If
        If vsfView.Row > 0 Then
            If e.KeyCode = Keys.Return Or e.KeyCode = Keys.Tab Then
                txtItemCode.Text = vsfView.GetData(vsfView.Row, 1)
                vsfView.Visible = False
            End If
        End If

    End Sub

    Private Sub vsfView_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles vsfView.DoubleClick
        If vsfView.Row > 0 Then
            txtItemCode.Text = vsfView.GetData(vsfView.Row, 1)
            vsfView.Visible = False
        End If
    End Sub

    Private Function applyFilterFromGrid() As Boolean
        applyFilterFromGrid = False
        Dim strSQL As String = ""
        With txtItemCode
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
        strSQL &= " and  ItemCode like '%" & txtItemCode.Text & "%'"
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

    Private Sub optAll1_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles optAll1.CheckedChanged
        If UCase(strRptName) = "ORDER REPORT" Then
            If optAll1.Checked Then
                chkCustName.Visible = True
                ChkRegion.Visible = False
                chkUser.Visible = False
                chkPrincipals.Visible = False
                chkCustName.Checked = False
                ChkRegion.Checked = False
                chkUser.Checked = False
                chkPrincipals.Checked = False
            End If
        End If
    End Sub

    Private Sub optItem_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles optItem.CheckedChanged
        If UCase(strRptName) = "ORDER REPORT" Then
            If optItem.Checked Then
                chkCustName.Visible = False
                ChkRegion.Visible = True
                chkUser.Visible = False
                chkPrincipals.Visible = False
                chkCustName.Checked = False
                ChkRegion.Checked = False
                chkUser.Checked = False
                chkPrincipals.Checked = False
            End If
        End If
    End Sub

    Private Sub optIC_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles optIC.CheckedChanged
        If UCase(strRptName) = "ORDER REPORT" Then
            If optIC.Checked Then
                chkCustName.Visible = False
                ChkRegion.Visible = False
                chkUser.Visible = True
                chkPrincipals.Visible = False
                chkCustName.Checked = False
                ChkRegion.Checked = False
                chkUser.Checked = False
                chkPrincipals.Checked = False
            End If
        End If
    End Sub

    Private Sub optIS_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles optIS.CheckedChanged
        If UCase(strRptName) = "ORDER REPORT" Then
            If optIS.Checked Then
                chkCustName.Visible = False
                ChkRegion.Visible = False
                chkUser.Visible = False
                chkPrincipals.Visible = True
                chkCustName.Checked = False
                ChkRegion.Checked = False
                chkUser.Checked = False
                chkPrincipals.Checked = False
            End If
        End If
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

    Private Sub vsfStk_Click(sender As Object, e As EventArgs) Handles vsfView.Click
        If (Control.ModifierKeys And Keys.Control) = Keys.Control AndAlso (Control.ModifierKeys And Keys.Alt) = Keys.Alt Then
            pnlCopyShortCut.Show()
        End If
        argvsf = vsfView
    End Sub
End Class