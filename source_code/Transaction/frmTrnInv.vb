Imports C1.Win.C1FlexGrid
Imports C1.Win.C1Input
Imports System.ComponentModel
Imports System.IO
Imports ThoughtWorks.QRCode.Codec
Public Class frmTrnInv
    'This form  is applicable for Both Party and Non Party Document
    Dim clsSVT As New svtLibrary
    Dim blnSavingInProgress As Boolean
    Dim dtSearch As DataTable
    Dim dvSearch As DataView
    Dim strQRUrl As String = ""
    Dim strQRUrlCheck As String = ""
    Dim intMaxDocID As Long
    Dim gBlnAllowSplit As Boolean
    Dim svtTrn As oledb.oledbTransaction
    Dim ctrlList As Object 'IEnumerable(Of C1FlexGrid)
    Dim blnFirstTime As Boolean = False
    Dim intDetNextEntryCol As Integer
    Dim strSplitInvRef As String
    Public Mode As String 'Control Transaction Add, Edit and Delete
    Public lngViewDocID As Long
    Dim strDateTime As String
    Dim blnQtySel As Boolean = False
    Dim intRowDocvalue As Integer
    'used for storing Multiline text
    Public StrgetText As String
    Public GetCSEINV As String

    Public strMultiAmtVal As String = ""
    Public dblMultiAmtVal As Double = 0
    Public blnMultiAmtVal As Boolean

    'for audit track
    Dim blnAuditTrack As Boolean = False
    Dim strAuditHdrSQL As String
    Dim strAuditDtlSQL As String

    'UtlTransaction Table Related Information
    '==============================================
    Dim strFrmCaption As String
    Dim intDocID As Int32
    Dim intMenuID As Int32

    Const vQDetItemid = 0
    Const vQQDetItem = 1
    Const vQQOH = 2

    'Basic Details
    Dim strSHForm As String
    Dim strDocHdrTblName As String
    Dim strDocDtlTblName As String
    Dim strDocIDFldName As String
    Dim strDocNoFldName As String
    Dim strPfxTYpe As String
    Dim strDocDateFldName As String
    Dim strQtyFldName As String
    Dim intQtyDecimals As Integer
    Dim strGCustCode As String
    'Additional Fields from UtlTransaction Table
    '===========================================

    'Editing Doc related fields
    Dim blnFreezeDoc As Boolean
    Dim blnEditDocDate As Boolean
    Dim intNumOfDays As Integer

    Dim strTenderFld As String = ""
    Dim blnFreezeQtyCol As Boolean
    Dim blnFreezeRateCol As Boolean
    Dim intFreezeCol As Int32
    Dim strEnvelop As String
    Dim blnBalColReq As Boolean
    Dim blnPartyRelatedFldsPresent As Boolean

    'LoadFromSettings Changes
    Dim blnPartyLoadFromSettingsPresent As Boolean


    'Additional variables used for various purpose
    Dim blnColFreeze As Boolean
    Dim intLastCol As Integer
    Dim blnallowToChangePfx As Boolean
    Dim strPfxWiseNo As String
    Dim strDocStdDtlWidth As String
    Dim intRowTenderFld As Int32

    'Detail section fields 
    Dim intPartyDocID As Int32

    Dim intColQty As Int32 = -1
    Dim intColrate As Int32 = -1

    'variables used for saving
    Dim strValues As String
    Dim strFlds As String
    Public strList As String

    'library used
    Dim strSQL As String
    Dim strDtlSQL As String

    'For security
    Dim strRights As String

    'for doc number format
    Dim strPfx As String, strNo As String
    Dim strAllowDocNoChngADDmode As String
    Dim intdetrows As Integer
    Dim sngDefQty As Single

    'declarations for detail grid
    Const vDetSlNo = 0
    Const vDetItemID = 1
    Const vDetItemSlNo = 2
    Const vDetItemCode = 3
    Const vDetItemName = 4
    Const vDetLiqType = 5
    Const vDetUOM = 6
    Const vDetLtrPerBot = 7
    Const vDetQOH = 8
    Dim intDetCols As Int32 = 9

    'for Reference we have maintain below variables 
    Dim intPartyID As Integer
    Dim intItemId As Long
    Dim blnShortClose As Boolean

    Dim blnTimer As Boolean = False
    Public blnRestore As Boolean = False
    Public strRestoreFileName As String = ""
    Dim strRestorePath As String
    Dim blnItemScan As Boolean = False

    Dim blnVsfMax As Boolean = False
    Dim blnVTot As Boolean = False

    Dim StrFldname As String

    Dim strItemSearchQry As String, strItemPartySearchQry As String, strSearchQry As String
    Dim BlnEnforcePartyItemLink As Boolean
    Dim strRateFldPartyItem As String, strRateFldMstItem As String
    Dim dtPrintFmt As DataTable


    'Print Settings

    Dim blnDefPrint As Boolean
    Dim strPrintOption As String
    Dim strPrintPrompt As String
    Dim intNoofCopies As Integer = 1
    Dim intRowLoc As Int32 = -1

    Public intRow As Integer

    Const vCustID = 0
    Const vCustName = 1
    Const vCustPhNo = 2

    ' invoice print settings
    Dim strInvNoReset As String
    Dim strCounterWsRst As String
    Public strDocNoFmt As String
    Dim strPrefixSettings As String
    Public lngInvCNO As Long
    Public strCounterPfx As String
    Public intSalesCounterID As Integer
    Dim strKOTRights As String
    Private MouseIsDown As Boolean = False
    Public strContactNo As String
    Public strDelAddress As String
    Dim strCounter As String
    Public blnSalesCounter As Boolean = True
    Dim intSPID As Int32 = 0, intRowCashAmt As Int32 = -1
    Dim intLastBillID As Int32 = 0
    Dim csRow As C1.Win.C1FlexGrid.CellStyle
    Public strText As String
    Public strNotesValue As String

    Dim strInvF12 As String = ""
    Dim strInvCtrlF12 As String = ""
    Dim strInvAltF12 As String = ""
    Dim intColItemAmt As Integer
    Dim intRowTolal As Int32
    Dim blnPDFPreview As Boolean = False

    Dim blnShowMesg As Boolean

    Dim intRowTotPointsRdmd As Int32
    Dim intRowInvVal As Int32

    Private Sub AddProc()
        Call clearFlds()
        enableDisableFlds(True)
        Call fillVsfHFTDefValues(Me)
        vsfDet.Rows.Count = 1
        StMsg()
        Call vsfGenHideMndtAllGrids(Me)
    End Sub

    Public Sub clearFlds()
        vsfTemp.Rows.Count = 1
        blnShowMesg = False
        txtPoints.BackColor = Color.Yellow
        enableDisableFlds(True)
        txtRcvdAmt.Text = ""
        txtPayback.Text = ""
        txtDocID.Text = ""
        txtPrintNo.Text = ""
        txtParty.Text = ""
        Mode = ""
        txtScan.BackColor = Color.White
        lblLmtBeer.Text = ""
        lblLmtBeer.ForeColor = Color.White
        lblLmtWine.Text = ""
        lblLmtWine.ForeColor = Color.White
        lblLmtIML.Text = ""
        lblLmtIML.ForeColor = Color.White
        txtNoteValQry.Text = ""
        strNotesValue = ""
        dtDocDate.Value = getSystemDateTime()
        Call ClearAddnlGrid(Me)
        vsfDet.Rows.Count = 1
        ReArrangeSlNo(vsfDet, vDetSlNo)
        lblCancelled.Visible = False
        txtParty.Text = ""
        lblPrevBill.BackColor = Color.Yellow
        If intLastBillID = 0 Then
            intLastBillID = clsSVT.getDataLongValue("Select Max(SINVID) From TrnHdrSInv Where SCID = " & intSalesCounterID, 0, False)
        End If
        If intLastBillID > 0 Then
            Dim strSptInvRf As String = clsSVT.getDataStringValue("Select SplitInvRef  From TrnHdrSInv Where SInvID = " & intLastBillID, "", False)
            If strSptInvRf <> "" Then
                Dim lngCount As Long = clsSVT.getDataStringValue("Select count(sinvid) From TrnHdrSInv Where SplitInvRef = '" & strSptInvRf & "'", "", False)
                lblPrevBill.Text = clsSVT.getDataStringValue("Select convert(varchar," & lngCount & ") + ' SPLIT Bills. ' + ' TotalAmt:' + convert(varchar,sum(INVVALUE)) From TrnHdrSInv Where SplitInvRef = '" & strSptInvRf & "'", "", False)
                lblPrevBill.BackColor = Color.LightBlue
            Else
                lblPrevBill.Text = clsSVT.getDataStringValue("Select 'Prev Bill.'+ SinvNo + ' Time:' + convert(varchar,Convert(Time,SinvDatetime)) + ' Amt:' + convert(varchar,INVVALUE) From TrnHdrSInv Where SINVID=" & intLastBillID, "", False)
                lblPrevBill.BackColor = Color.Yellow
            End If

        End If
        txtNoOfItems.Text = ""
        txtTotalQty.Text = ""
        txtPoints.Text = ""
        txtFinalValue.Text = ""
        txtHdrInvVal.Text = ""
        txtPrintNo.Text = getInvNo()
        txtScan.Focus()
        Call fillVsfHFTDefValues(Me)
        EnableDisableOptBtn(False)
    End Sub

    Public Sub enableDisableFlds(ByVal argFlag As Boolean)
        Dim blnFound As Boolean = False
        If blnEditDocDate Then
            dtDocDate.Enabled = argFlag
        Else
            dtDocDate.Enabled = False
        End If
        If gUserCode <> "ADMIN" Then
            dtDocDate.Enabled = False
        End If
    End Sub

    Private Sub frmTrnPartyDoc_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Call INIStore("Invoice", "SPID", txtSalesPerson.Text)
        e.Cancel = False
        If Mode = "A" Or Mode = "M" Then
            If MsgBox("Are you sure you want to exit the form without saving?", MsgBoxStyle.Question _
                      + MsgBoxStyle.YesNo + vbDefaultButton2, "Confirmation...") = MsgBoxResult.No Then
                e.Cancel = True
                Exit Sub
            End If
        End If
        Mode = "V"
        frmMain.pnlProject.Panels(0).Visible = True
        'Me.Close()
    End Sub


    Private Sub frmTrnPartyDoc_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyUp
        Dim blnFnKey As Boolean = False
        If e.KeyCode = Keys.Escape Then
            'fraAddress.Visible = False
        End If
        If e.KeyCode = Keys.F10 Then
            txtRcvdAmt.Text = ""
            txtRcvdAmt.Focus()
            Exit Sub
        End If

        If e.KeyCode = Keys.F9 And e.Control Then
            Call initFormVsfDet(Me, Me.Text, vsfDet, "DET")
            Call initFormSC(Me, Me.Text, scMain, "Main")
            Call initFormSC(Me, Me.Text, scTot, "Total")
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
            Call SaveActFormSC(Me, Me.Text, scMain, "Main")
            Call SaveActFormSC(Me, Me.Text, scTot, "Total")
            Call INIStore("Invoice", "SPID", txtSalesPerson.Text)
            Dim argDsgrGrids = GetAllDsgrVsf(Me, GetType(C1FlexGrid))
            For Each GridList As C1FlexGrid In argDsgrGrids
                If GridList.Visible Then
                    Call SaveActFormVsfDet(Me, Me.Text, GridList.Name, GridList)
                End If
            Next
            MsgBox("Saved Form & Grid Layout Successfully!!", vbInformation, Me.Text)
            Exit Sub
        End If
        Call raiseShortCutEvent(e)
    End Sub

    Public Sub initForm(ByVal argDocID As Integer)
        intDocID = argDocID
        Call getUtlTrnData(intDocID)
        Call initScreen()
        If clsSVT.getDataStringValue("Select setvalue from UtlGsettings where SetName = 'SPLITINV'", "N", False) = "Y" Then
            gBlnAllowSplit = True
        Else
            gBlnAllowSplit = False
        End If
        blnSavingInProgress = False
        txtPoints.BackColor = Color.Yellow
        If Not blnSalesCounter Then Exit Sub
        intMenuID = clsSVT.getDataLongValue("Select MenuItemID from MstMenu where DocID = " & intDocID, 0, False)
        strRights = getUserRights(intMenuID)
        KeyPreview = True
        Mode = "A"
        ctrlList = GetAllDsgrVsf(Me, GetType(C1.Win.C1FlexGrid.C1FlexGrid))
        Me.WindowState = FormWindowState.Maximized

        If blnLoyalty Then
            lblPoints.Visible = True
            txtPoints.Visible = True
            btnShowPts.Visible = True

        Else
            lblPoints.Visible = False
            txtPoints.Visible = False
            btnShowPts.Visible = False
        End If
        txtScan.Focus()
    End Sub
    Private Sub initScreen()
        Dim j As Integer
        'chkSMS.Checked = True
        Dim Ilistopts As New List(Of String)
        vsfTot.Rows.Count = 0
        If getGsettings("INVOICE", "QTYSELINV", "Y") = "Y" Then
            blnQtySel = True
        Else
            blnQtySel = False
        End If
        'blnQtySel = True
        txtPayback.BackColor = Color.Black
        txtPayback.ForeColor = Color.Lime
        strInvNoReset = getGsettings("Invoice", "InvNoReset", "")
        strCounterWsRst = getGsettings("Invoice", "CounterWsRst", "N")
        strDocNoFmt = getGsettings("Invoice", "DocNoFmt", "<<5>>/<<6>>")
        strPrefixSettings = getGsettings("Invoice", "PrefixSettings", "A,B,C,D,E,F,G,H,I,J,K,L")
        strCounter = clsSVT.getDataStringValue("Select SCCode from MstSC where SysName = '" & gStrSystemName & "'", "", False)
        If strCounter = "" Then
            blnSalesCounter = False
            If MsgBox("Sales Counter Not Defined..., Do you want to create new?", MsgBoxStyle.YesNo, Me.Text) = MsgBoxResult.Yes Then
                Call frmMain.MasterClick("Sales Counter Master")
                Exit Sub
            End If
            Exit Sub
        End If
        txtCount.Text = clsSVT.getDataStringValue("Select SCCODE  from MstSC where SysName = '" & gStrSystemName & "'", "", False)
        strCounterPfx = clsSVT.getDataStringValue("Select PREFIX from MstSC where SysName = '" & gStrSystemName & "'", "", False)
        intSalesCounterID = clsSVT.getDataStringValue("Select SCID from MstSC where SysName = '" & gStrSystemName & "'", 0, False)

        strPfxTYpe = ""
        txtNoOfItems.Text = ""
        txtTotalQty.Text = ""
        txtFinalValue.Text = ""
        txtHdrInvVal.Text = ""

        Call InitAddnlFldGrids(Me, vsfTot, "Total", intDocID)
        lblCancelled.Visible = False

        With vsfDet
            .Cols.Count = intDetCols
            .Rows.Count = 1
            .SetData(0, vDetSlNo, "Sl No")
            .SetData(0, vDetItemName, "Item")
            .SetData(0, vDetUOM, "UOM")
            .SetData(0, vDetQOH, "QOH")

            .Cols(vDetSlNo).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.LeftCenter
            'strDocStdDtlWidth = "40, 15, 15, 150, 150, 75, 100"

            Call SetColAlign(vsfDet, svtAlign.Left, vDetSlNo, vDetItemName)
            Call SetHideCol(vsfDet, True, vDetItemID, vDetItemSlNo, vDetItemCode, vDetLiqType, vDetUOM, vDetLtrPerBot)
            Call initItemVsfCombo()
            Call InitDetAddnlGrid(Me, vsfDet, vsfOptions, intDetCols, strDocDtlTblName, intDocID)
            With vsfOptions
                .SetData(vDetSlNo, vOthFldDataType, "Numeric")
                .SetData(vDetItemSlNo, vOthFldDataType, "Numeric")
                .SetData(vDetItemID, vOthFldDataType, "Numeric")
                .SetData(vDetItemCode, vOthFldDataType, "Text")
                .SetData(vDetItemName, vOthFldDataType, "Text")
                .SetData(vDetItemCode, vOthFldCode, "SVTITEMCODE")
                .SetData(vDetItemCode, vOthFldBlnReadOnly, "N")
                .SetData(vDetItemID, vOthFldCode, "SVTITEM")
                .SetData(vDetItemID, vOthFldCaption, "ITEM")
                .SetData(vDetItemCode, vOthFldCaption, "ITEM")
                .SetData(vDetUOM, vOthFldCaption, "UOM")
                intColQty = getColNoFromAddnlDetGrid(vsfOptions, vsfDet, strSHForm & "QTY")
                intColrate = getColNoFromAddnlDetGrid(vsfOptions, vsfDet, strSHForm & "RATE")
                .SetData(vDetSlNo, vOthFldBlnReadOnly, "Y")
                .SetData(vDetItemSlNo, vOthFldBlnReadOnly, "Y")
                .SetData(vDetItemID, vOthFldBlnReadOnly, "Y")
                .SetData(vDetItemName, vOthFldBlnReadOnly, "Y")
                .SetData(vDetUOM, vOthFldBlnReadOnly, "Y")
                .SetData(vDetQOH, vOthFldBlnReadOnly, "Y")
            End With

            intLastCol = 1
            For j = vsfDet.Cols.Count - 1 To 0 Step -1
                If vsfDet.Cols(j).Visible = True Then
                    intLastCol = j
                    Exit For
                End If
            Next
            Call getAfterFixedNextEntryCol()

            'Hide columns which are marked as TOGGLE HIDE
            For i = 0 To vsfDet.Cols.Count - 1
                If vsfOptions.GetData(i, vOthBlnTogHide) = "Y" Then
                    vsfDet.Cols(i).Visible = Not vsfDet.Cols(i).Visible
                End If
            Next
        End With

        If intPartyDocID > 0 Then
            If clsSVT.getDataStringValue("Select FldPickUpAfterFld from UtlDocDsgrFlds where DocID = " & intDocID & " and fldSection<>'Detail' and FldPickUpAfterFld = '#SVTPARTY#'", "", False) <> "" Then
                blnPartyRelatedFldsPresent = True
            Else
                blnPartyRelatedFldsPresent = False
            End If
        End If

        'LoadFromSettings Changes
        If clsSVT.getDataStringValue("Select FldBlnLoadFrom from UtlDocDsgrFlds where DocID = " & intDocID & " and upper(Ltrim(left(FldLoadFldName,1))) in ('C','S') and fldSection<>'Detail' and FldBlnLoadFrom= 'Y'", "", False) = "Y" Then
            blnPartyLoadFromSettingsPresent = True
        Else
            blnPartyLoadFromSettingsPresent = False
        End If

        If strTenderFld <> "" Then
            intRowTenderFld = getRowNoFromAddnlGrid(vsfTot, strTenderFld)
        End If
        enableDisableFlds(False)

        Call fillCboDtls(btnQuickSel, "MstQuickSelItems")

        Call fillCboDtls(btnLoadTemp, "MstItemTemp")

        With vsfMenu
            .Rows.Fixed = 0
            .Rows.Count = 0
            .Cols.Count = 3
            .Cols(vQDetItemid).Visible = False
        End With

        With vsfTot
            .Styles.EmptyArea.BackColor = Color.Black
            .Styles.Normal.BackColor = Color.Black
            .Styles.Alternate.BackColor = Color.Black
            .Styles.Normal.ForeColor = Color.White
            .Styles.Alternate.ForeColor = Color.White
            .Styles.Fixed.ForeColor = Color.White
        End With
        Me.BackColor = Color.Black
        With vsfDet
            .Styles.EmptyArea.BackColor = Color.Black
            .Styles.Normal.BackColor = Color.Black
            .Styles.Alternate.BackColor = Color.Black
            .Styles.Normal.ForeColor = Color.White
            .Styles.Alternate.ForeColor = Color.White
            .Styles.Fixed.ForeColor = Color.White
            .Styles.Highlight.BackColor = Color.Yellow
            .Styles.Highlight.ForeColor = Color.Black
            .Styles.Focus.BackColor = Color.Yellow
            .Styles.Focus.ForeColor = Color.Black
        End With

        intRowCashAmt = getRowNoFromAddnlGrid(vsfTot, "CASHAMT")
        csRow = vsfDet.Styles.Add("svt")

        strInvF12 = getGsettings("Invoice", "INVF12", "S")
        strInvCtrlF12 = getGsettings("Invoice", "INVCTRLF12", "S")
        strInvAltF12 = getGsettings("Invoice", "INVALTF12", "S")

        Dim strToolMsg As String = ""

        Select Case strInvF12
            Case "S"
                btnPrint.Text = "F12 - Save Bill"
                strToolMsg &= "F12 - Save" & vbNewLine
            Case "SP"
                btnPrint.Text = "F12 - Save & Print"
                strToolMsg &= "F12 - Save & Print" & vbNewLine
            Case "SPP"
                btnPrint.Text = "F12 - Save & Preview"
                strToolMsg &= "F12 - Save & Preview" & vbNewLine
        End Select


        Select Case strInvCtrlF12
            Case "S"
                strToolMsg &= "Ctrl + F12 - Save" & vbNewLine
            Case "SP"
                strToolMsg &= "Ctrl + F12 - Save & Print" & vbNewLine
            Case "SPP"
                strToolMsg &= "Ctrl + F12 - Save & Preview" & vbNewLine
        End Select

        Select Case strInvAltF12
            Case "S"
                strToolMsg &= "Alt + F12 - Save"
            Case "SP"
                strToolMsg &= "Alt + F12 - Save & Print"
            Case "SPP"
                strToolMsg &= "Alt + F12 - Save & Preview"
        End Select

        Me.ToolTip1.SetToolTip(Me.btnPrint, strToolMsg)

        intRowTolal = getRowNoFromAddnlGrid(vsfTot, "TOT")
        intRowTotPointsRdmd = getRowNoFromAddnlGrid(vsfTot, "POINTSRDMD")
        intColItemAmt = getColNoFromAddnlDetGrid(vsfOptions, vsfDet, "ITEMAMT")
        intRowInvVal = getRowNoFromAddnlGrid(vsfTot, "INVVALUE")
        txtPoints.BackColor = Color.Yellow
    End Sub

    Private Sub getAfterFixedNextEntryCol()
        With vsfOptions
            For i As Int32 = intDetCols To .Rows.Count - 1
                If vsfOptions.GetData(i, vOthFldBlnReadOnly) <> "Y" Then
                    intDetNextEntryCol = i
                    Exit Sub
                End If
            Next
        End With
    End Sub

    Private Sub getUtlTrnData(ByVal argDocID As Integer)
        Dim rsDummy As OleDb.OleDbDataReader

        strAllowDocNoChngADDmode = clsSVT.getDataStringValue("Select DocNoChange from UtlDocNoFmt where DocID = " & intDocID, "Y", False)

        rsDummy = clsSVT.getDataReaderFromSQL("Select * from UtlTransactions where DocID = " & argDocID)

        With rsDummy
            If .HasRows Then
                .Read()
                strFrmCaption = GetVal(!DocName, "")
                Me.Text = strFrmCaption
                Me.Tag = strFrmCaption

                'for audit track
                If GetVal(!AuditLogTrack, "") = "Y" Then
                    blnAuditTrack = True
                    strAuditHdrSQL = GetVal(!AuditSQLHdr, "")
                    strAuditDtlSQL = GetVal(!AuditSQLDtl, "")
                End If

                strSHForm = GetVal(!DocShForm, "")
                strDocNoFldName = strSHForm & "No"
                strDocIDFldName = strSHForm & "ID"
                strQtyFldName = strSHForm & "Qty"
                strDocDateFldName = strSHForm & "Date"
                strDocHdrTblName = "TrnHdr" & strSHForm
                strDocDtlTblName = "TrnDtl" & strSHForm
                If GetVal(!PDFPreview, "") = "Y" Then
                    blnPDFPreview = True
                End If

                strRateFldMstItem = GetVal(!RateFldMstItem, "")
                strRateFldPartyItem = GetVal(!RateFldPartyItem, "")

                BlnEnforcePartyItemLink = IIf(GetVal(!FldBlnEnforcePartyItemLink, "N") = "N", False, True)

                blnFreezeDoc = IIf(GetVal(!blnFreezeDoc, "N") = "N", False, True)
                intNumOfDays = GetVal(!AllowEditDeleteForDays, 0)
                blnEditDocDate = IIf(GetVal(!AllowEditDocDate, "N") = "N", False, True)
                strTenderFld = UCase(GetVal(!TenderChangeFld, ""))
                blnFreezeQtyCol = IIf(GetVal(!FreezeQtyCol, "N") = "N", False, True)
                blnFreezeRateCol = IIf(GetVal(!FreezeQtyCol, "N") = "N", False, True)
                strEnvelop = GetVal(!blnEnvPrint, "N")

                intPartyDocID = GetVal(!MstDocIDParty, 0)
            End If
            .Close()
        End With

        dtPrintFmt = clsSVT.getDATATable("Select * from UtlDocNoFmt Where DocID=" & intDocID)
        intNoofCopies = clsSVT.getDataLongValue("Select NOOFCOPIES from UtlOption Where DocID = " & intDocID & " and DefaultPrint='Y'", 1, False)
        strPrintOption = clsSVT.getDataStringValue("Select OptionName from UtlOption Where DocID = " & intDocID & " and DefaultPrint='Y'", "", False)
        If strPrintOption <> "" Then
            blnDefPrint = True
            strPrintPrompt = clsSVT.getDataStringValue("Select AfterSaveAction from UtlOption Where DocID = " & intDocID & " and DefaultPrint='Y'", "", False)
        Else
            If clsSVT.getDataLongValue("Select Count(*) from UtlOption Where DocID = " & intDocID, 0, False) = 1 Then
                strPrintOption = clsSVT.getDataStringValue("Select OptionName from UtlOption Where DocID = " & intDocID, "", False)
                If strPrintOption <> "" Then
                    blnDefPrint = True
                    strPrintPrompt = clsSVT.getDataStringValue("Select AfterSaveAction from UtlOption Where DocID = " & intDocID, "", False)
                End If
            End If
        End If
    End Sub

    Private Sub initItemVsfCombo()
        strItemSearchQry = "Select i.ItemId, ItemCode as [" & gItemCode & "], u.UOMCode, ShortName as [" & gItemName & "] "
        If gstrVsfItemCodeCols <> "" Then
            strItemSearchQry &= "," & gstrVsfItemCodeCols
        End If
        strItemSearchQry &= " From MstItem i, MstUOM u"
        strItemSearchQry &= " where i.uomid = u.uomid "
        strItemSearchQry &= " and i.InActive = 'N' "
        strItemSearchQry &= " and i.Authorised = 'Y' "
    End Sub

    Private Sub vsfDet_AfterEdit(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfDet.AfterEdit
        'If Mode = "V" Or Mode = "" Then Exit Sub
        If e.Col <> vDetSlNo Then Call vsfOptAfterEditBfrCalTot(Me, vsfDet, vsfOptions, vsfDet.Row, vsfDet.Col)
        Call CalTotal()

    End Sub

    Private Sub vsfDet_BeforeEdit(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfDet.BeforeEdit
        'If Not (Mode = "A" Or Mode = "M") Then
        '    e.Cancel = True
        '    Exit Sub
        'End If
        Dim blnFound As Boolean = False
        vsfDet.EditMask = ""
        vsfDet.ComboList = ""

        Select Case vsfDet.Col
            Case vDetSlNo, vDetItemSlNo, vDetQOH, vDetItemName, vDetUOM, vDetItemCode
                e.Cancel = True

            Case intColQty
                If blnFreezeQtyCol And Mode <> "A" Then
                    e.Cancel = True
                    Exit Sub
                End If
            Case intColrate
                If blnFreezeRateCol And Mode <> "A" Then
                    e.Cancel = True
                    Exit Sub
                End If
        End Select

        If e.Col < intDetCols Then Exit Sub
startdsgrchk:
        If vsfOptBeforeEditCancel(Me, vsfDet, vsfOptions, strDocDtlTblName, "A", vsfDet.Row, vsfDet.Col, intDetCols) Then
            e.Cancel = True
            Exit Sub
        End If

    End Sub

    Public Function FillItemDet(ByVal argItem As String, ByVal argRow As Integer, ByVal argUOM As String) As Boolean
        Dim rsDummy As OleDb.OleDbDataReader
        Dim strSQL As String
        Dim blnInActive As Boolean
        FillItemDet = False
        If argRow < 1 Then argRow = 1

        With vsfDet
            If argItem = "" Then
                Exit Function
            End If

            intItemId = clsSVT.getDataLongValue("Select ItemID from MstItem where ItemCode = '" & argItem & "'", 0, blnInActive)
            If intItemId = 0 Then
                MsgBox("ItemCode '" & argItem & "' does not exist!", vbInformation, Me.Text)
                Exit Function
            End If

            Call clsSVT.getDataLongValue("Select ItemID from MstItem where ItemCode = '" & argItem & "' and InActive <> 'N'", 0, blnInActive)

            If blnInActive Then
                MsgBox("Item code '" & argItem & "' is inactive! Please select an active Item from the list.", vbInformation, Me.Text)
                Exit Function
            End If

            strSQL = "Select i.*, UOMCode, LTRPERBOT from "
            strSQL = strSQL & " MstItem i, MstUOM u, MstPKG p "
            strSQL = strSQL & "  where i.UomID = u.UomID  "
            strSQL = strSQL & " and i.PKGID=p.PKGID "
            strSQL = strSQL & " and  ItemCode ='" & argItem & "'"

            rsDummy = clsSVT.getDataReaderFromSQL(strSQL)

            With rsDummy
                If Not .HasRows Then
                    .Close()
                    MsgBox("Item does not exist!!", vbInformation, Me.Text)
                    Exit Function
                End If
                .Read()

                vsfDet.SetData(argRow, vDetSlNo, argRow)
                If Val(vsfDet.GetData(argRow, vDetItemSlNo)) <= 0 Then
                    vsfDet.SetData(argRow, vDetItemSlNo, getmaxNo())
                End If

                'Const vDetSlNo = 0
                'Const vDetItemID = 1
                'Const vDetItemSlNo = 2
                'Const vDetItemCode = 3
                'Const vDetItemName = 4
                'Const vDetUOM = 5
                'Const vDetLtrPerBot = 6
                'Const vDetQOH = 7


                vsfDet.SetData(argRow, vDetItemID, rsDummy("ItemID"))
                vsfDet.SetData(argRow, vDetItemName, rsDummy("Itemname"))
                vsfDet.SetData(argRow, vDetItemCode, rsDummy("ItemCode"))
                vsfDet.SetData(argRow, vDetLtrPerBot, rsDummy("LTRPERBOT"))
                If argUOM <> "" Then
                    vsfDet.SetData(argRow, vDetUOM, argUOM)
                Else
                    vsfDet.SetData(argRow, vDetUOM, !UOMCode)
                End If
                .Close()
            End With
        End With

        Dim blnFound As Boolean
        If intColrate >= 0 Then
            vsfDet.SetData(argRow, intColrate, clsSVT.getDataDoubleValue("Select " & strRateFldMstItem & " From MstItem Where ItemID = " & vsfDet.GetData(argRow, vDetItemID), 0, blnFound))
        End If

        Call fillItemRelatedDataFromDocDsgnr(intDocID, Val(vsfDet.GetData(argRow, vDetItemID)), Me)
        Call vsfOptValidateEdit(Me, vsfDet, vsfOptions, vsfDet.Row, vDetItemCode, "T", strDocDtlTblName, "", "")
        Call fillVsfDetDefValues(vsfDet, vsfOptions, argRow)
        Call CalTotal()
        FillItemDet = True
    End Function

    Private Function getmaxNo() As Integer
        Dim i As Integer, intNo As Integer
        With vsfDet
            intNo = 0
            For i = 1 To .Rows.Count - 1
                If Val(.GetData(i, vDetItemSlNo)) > intNo Then
                    intNo = Val(.GetData(i, vDetItemSlNo))
                End If
            Next i
        End With
        intNo = intNo + 1
        getmaxNo = intNo
    End Function

    Private Sub vsfDet_EnterCell(ByVal sender As Object, ByVal e As System.EventArgs) Handles vsfDet.EnterCell
        ' If Not (Mode = "A" Or Mode = "M") Then Exit Sub

        With vsfDet
            Select Case .Col
                Case Is >= intDetCols
                    If vsfOptions.GetData(.Col, vOthFldBlnComboData) = "Y" Then Exit Sub
                    If vsfOptions.GetData(.Col, vOthFldDataType) = "List String" Then Exit Sub
                    If vsfOptions.GetData(.Col, vOthFldBlnReadOnly) = "Y" Then Exit Sub
                    If vsfOptions.GetData(.Col, vOthFldBlnRefill) = "Y" Then Exit Sub
                    If blnFreezeDoc AndAlso Mode = "M" Then Exit Sub
                    If blnFreezeQtyCol AndAlso .Col = intColQty AndAlso Mode = "M" Then Exit Sub
                    If blnFreezeRateCol AndAlso .Col = intColrate AndAlso Mode = "M" Then Exit Sub
                    Try
                        ' .StartEditing()
                    Catch ex As Exception

                    End Try
                Case Else

            End Select
            'If blnLinkedToDoc Then
            '    If .Rows > 1 Then
            '        Call clsSVT.GetDataValue("ItemID", "", strLinkedTODtlTblName & " d,  " & strLinkedTOHdrTblName & "  h", _
            '                blnFound, _
            '                "where d." & strLinkedToDocNoFldName & "  = h." & strLinkedToDocNoFldName & "  and d.ItemID = " & Val(.TextMatrix(.Row, vDetItemID)) & _
            '                " and d." & strDocNoFldName & " = '" & txtDocNo.Text & "'")
            '        If blnFound Then Exit Sub
            '    End If
            'End If
        End With

    End Sub

    Private Sub vsfDet_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles vsfDet.GotFocus
        'If Mode = "V" Or Mode = "" Then Exit Sub
        If vsfDet.Focused Then Exit Sub
        If vsfDet.Rows.Count > 1 AndAlso Trim(GetVal(vsfDet.GetData(1, intDetNextEntryCol), "")) = "" Then
            vsfDet.Col = intDetNextEntryCol
        End If
    End Sub

    Private Sub vsfDet_KeyPressEdit(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.KeyPressEditEventArgs) Handles vsfDet.KeyPressEdit

        If e.KeyChar = "'" Then
            e.Handled = True
        End If

        If e.Col < intDetCols Then Exit Sub

        If vsfOptKeyPressEdit(Me, vsfDet, vsfOptions, e.Col, e.Row, e.KeyChar) Then
            e.Handled = False
        Else
            e.Handled = True
        End If
    End Sub

    Private Sub vsfDet_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles vsfDet.KeyUp
        'Dim vsfCopyItemCode As String
        If e.KeyCode = Keys.F7 Then
            If blnColFreeze = False Then
                vsfDet.Cols.Frozen = vsfDet.Col
                blnColFreeze = True
                MsgBox("Column Freezing Activated up to " & vsfDet.GetData(0, vsfDet.Col), vbInformation, Me.Text)
            Else
                blnColFreeze = False
                vsfDet.Cols.Frozen = 1
                MsgBox("Column Freezing De-Activated", vbInformation, Me.Text)
            End If
        End If

        If e.KeyCode = Keys.Delete Then
            If vsfDet.Row > 0 Then
                If MsgBox("Are you sure you want to remove item:" & vsfDet.GetData(vsfDet.Row, vDetItemCode), MsgBoxStyle.YesNo, Me.Text) = MsgBoxResult.No Then Exit Sub
                vsfDet.Rows.Remove(vsfDet.Row)
                Call CalTotal()
                txtScan.Focus()
                Exit Sub
            End If
        End If

        If e.KeyCode = Keys.Add AndAlso vsfDet.Row >= 1 Then
            If vsfDet.GetData(vsfDet.Row, vDetItemCode) <> "" Then
                vsfDet.SetData(vsfDet.Row, intColQty, Val(vsfDet.GetData(vsfDet.Row, intColQty) + 1))
                Call CalTotal()
            End If
            Exit Sub
        End If

        If e.KeyCode = Keys.Subtract AndAlso vsfDet.Row >= 1 Then
            If vsfDet.GetData(vsfDet.Row, vDetItemCode) <> "" Then
                If Val(vsfDet.GetData(vsfDet.Row, intColQty) - 1) >= 0 Then
                    vsfDet.SetData(vsfDet.Row, intColQty, Val(vsfDet.GetData(vsfDet.Row, intColQty) - 1))
                    Call CalTotal()
                End If
            End If
            Exit Sub
        End If

        If e.Control = True And e.KeyCode = Keys.C Then
            Call vsfCopyItemCode(vsfDet, vsfDet.Row, vsfDet.Col)
            Exit Sub
        End If

        'If (Not (Mode = "A" Or Mode = "M")) Then Exit Sub

        'If intDetCols < intDetCols Then Exit Sub
    End Sub

    Private Sub vsfDet_SetupEditor(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfDet.SetupEditor
        'If (Not (Mode = "A" Or Mode = "M")) Then Exit Sub
        If vsfDet.Col < intDetCols Then Exit Sub

        Call vsfOptionsSetupEditor(Me, vsfDet, vsfOptions, intDocID, strDocHdrTblName, "A", e.Row, e.Col, "M")
    End Sub

    Public Sub vsfDet_ValidateEdit(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.ValidateEditEventArgs) Handles vsfDet.ValidateEdit
        Dim intDocQty As Double

        'If (Not (Mode = "A" Or Mode = "M")) Then
        '    e.Cancel = True
        '    Exit Sub
        'End If

        With vsfDet

            If e.Col = intColQty Then
                intDocQty = Val(.Editor.Text)
                If GetVal(.GetData(.Row, vDetItemCode), "") = "" Then
                    MsgBox("Please select item first....", MsgBoxStyle.Critical, Me.Text)
                    '.Editor.Text = ""
                    Exit Sub
                End If


                If intDocQty < 1 Then
                    MsgBox("Qty cannot Be zero or less than One ....", MsgBoxStyle.Critical, Me.Text)
                    .Editor.Text = ""
                    Exit Sub
                End If
                'If Not gBlnAllowNegativeStock AndAlso strAddReduceStock = "RED" AndAlso intDocQty > GetVal(.GetData(.Row, vDetQOH), 0) Then
                'MsgBox("Qty cannot more than QOH....", MsgBoxStyle.Critical, Me.Text)
                '.Editor.Text = ""
                'Exit Sub
                'End If
            End If
        End With

        If vsfDet.Col < intDetCols Then Exit Sub

        If vsfOptValidateEdit(Me, vsfDet, vsfOptions, vsfDet.Row, vsfDet.Col, "T", strDocDtlTblName, "", "", "") = False Then
            e.Cancel = True
        End If
    End Sub

    Public Function vsfdetValidateEdit(ByVal argRow As Integer, argCol As Integer) As Boolean
        Dim intDocQty As Double
        vsfdetValidateEdit = False

        If (Not (Mode = "A" Or Mode = "M")) Then
            vsfdetValidateEdit = True
            Exit Function
        End If

        With vsfDet
            If argCol = intColQty Then
                intDocQty = Val(.Editor.Text)
            End If
        End With

        If vsfOptValidateEdit(Me, vsfDet, vsfOptions, argRow, argCol, "T", strDocDtlTblName, "", "") = False Then
            vsfdetValidateEdit = True
            Exit Function
        End If
    End Function

    Public Sub CalTotal()
        Call ReArrangeSlNo(vsfDet, vDetSlNo)
        If gridEmpty() Then
            vsfDet.Rows.Count = 1
        End If
        Call AddnlFldCalTotal(Me, vsfOptions, vsfDet, intDetCols, "", "", argMode:="A")
        Call AddnlFldAppotionateCatTotal(Me, intDetCols, "", "", intDocID)
        Call vsfGenHideMndtAllGrids(Me)

        Dim i As Integer
        txtNoOfItems.Text = ""
        txtTotalQty.Text = ""
        txtFinalValue.Text = ""
        txtHdrInvVal.Text = ""
        With vsfDet
            For i = 1 To .Rows.Count - 1
                If Val(.GetData(i, vDetItemID)) > 0 Then
                    txtNoOfItems.Text = Val(txtNoOfItems.Text) + 1
                    txtTotalQty.Text = Val(txtTotalQty.Text) + Val(.GetData(i, intColQty))
                End If
            Next
        End With


        txtFinalValue.Text = Format(Val(txtFinalValue.Text) + Val(vsfTot.GetData(vsfTot.Rows.Count - 1, vOthFldDataValue)), "0.00")
        txtHdrInvVal.Text = Format(Val(txtFinalValue.Text), "0.00")

        '============
        If gSettings.gCompName = "SVS DROPS" Or gSettings.gCompName = "SVS WINES" Then
            If Val(vsfTot.GetData(intRowTotPointsRdmd, vOthFldDataValue)) > 0 Then
                vsfTot.SetData(intRowInvVal, vOthFldDataValue, (Val(vsfTot.GetData(intRowCashAmt, vOthFldDataValue)) - Val(vsfTot.GetData(intRowTotPointsRdmd, vOthFldDataValue))))
            End If
            txtFinalValue.Text = Format(Val(vsfTot.GetData(vsfTot.Rows.Count - 1, vOthFldDataValue)), "0.00")
        End If
        '===============
        Call CalcLiqLimit()
    End Sub

    Public Function validFlds() As Boolean
        validFlds = False

        Call ReArrangeSlNo(vsfDet, vDetSlNo, vDetItemSlNo)
        Call CalTotal()
        Call INIStore("Invoice", "SPID", txtSalesPerson.Text)
        If intSPID = 0 Then
            intSPID = clsSVT.getDataLongValue("Select SPID from MstSP Where SPCode='" & Trim(txtSalesPerson.Text) & "'", 0, False)
            If intSPID = 0 Then
                MsgBox("Please select sales person", vbInformation, Me.Text)
                txtSalesPerson.Focus()
                Exit Function
            End If
        End If

        If intSalesCounterID = 0 Then
            MsgBox("Sales Counter id is zero!", MsgBoxStyle.Critical, Me.Text)
            Exit Function
        End If


        If vsfDet.Rows.Count = 1 Then
            MsgBox("Nothing to Save. Select at least one item.", MsgBoxStyle.Critical, Me.Text)
            txtScan.Focus()
            Exit Function
        End If

        If Not validDate(dtDocDate.Value) Then
            MsgBox("Doc Date Should be within the financial year," & vbCrLf & "Please go to Company Master And Change Financial year" & vbCrLf & " " & vbCrLf & "1. Click On Create new financial year and Click Create" & vbCrLf & "" & vbCrLf & "2. Click On set Active Financial Year and Select The Year " & vbCrLf & "" & vbCrLf & "3. Click on Set As Active " & vbCrLf & "" & vbCrLf & "Note: Please Take a Database Backup Before Doing Above Steps", vbCritical, Me.Text)

            Dim objFrm As New FrmMstComp
            With objFrm
                frmMain.OpenForms(objFrm, "Company Master")
                objFrm.C1DockingTabPage3.Show()
            End With
            Exit Function
        End If

        With vsfDet
            For i As Int32 = 1 To .Rows.Count - 1
                If Not gBlnAllowNegativeStock Then
                    If Val(.GetData(i, intColQty)) > Val(.GetData(i, vDetQOH)) Then
                        MsgBox("Stock is Less... Cannot Proceed..", MsgBoxStyle.Exclamation, Me.Text)
                        .Row = i
                        .Col = vDetQOH
                        .Focus()
                        Exit Function
                    End If
                End If
                If Val(.GetData(i, intColQty)) < 0 Then
                    MsgBox("Qty is negative... Cannot Proceed..", MsgBoxStyle.Exclamation, Me.Text)
                    .Row = i
                    .Col = intColQty
                    .Focus()
                    Exit Function
                End If


                If Val(.GetData(i, intColQty)) < 1 Then
                    MsgBox("Qty cannot Be zero or less than One....", MsgBoxStyle.Critical, Me.Text)
                    .Row = i
                    .Col = intColQty
                    .Focus()
                    Exit Function
                End If
            Next
        End With

        If Not validOthFLds(Me, strDocHdrTblName, strDocNoFldName, txtPrintNo.Text, Trim(strSHForm)) Then
            Exit Function
        End If

        If Not validOthDetFLds(Me, vsfDet, vsfOptions, intDetCols) Then Exit Function

        If Not gBlnAllowSplit Then
            If Val(Trim(lblLmtBeer.Text).Replace("Beer : ", "").Replace(" Ltr ", "")) > gDblBeerLmt Then
                MsgBox("Beer quantity exceeds more than " & gDblBeerLmt & " Ltr", vbInformation, Me.Text)
                Exit Function
            End If

            If Val(Trim(lblLmtWine.Text).Replace("Wine : ", "").Replace(" Ltr ", "")) > gDblWineLmt Then
                MsgBox("Wine quantity exceeds limit more than " & gDblWineLmt & " Ltr", vbInformation, Me.Text)
                Exit Function
            End If

            If Val(Trim(lblLmtIML.Text).Replace("IML : ", "").Replace(" Ltr ", "")) > gDblIMLlmt Then
                MsgBox("IML quantity exceeds limit more than " & gDblIMLlmt & " Ltr", vbInformation, Me.Text)
                Exit Function
            End If
        End If
        If blnLoyalty Then
            If Val(vsfTot.GetData(intRowTotPointsRdmd, vOthFldDataValue)) > 0 Then
                If Val(txtFinalValue.Text) < Val(vsfTot.GetData(intRowTotPointsRdmd, vOthFldDataValue)) Then
                    MsgBox("Redeem Points exceeds Invoice Value " & gDblIMLlmt & " Ltr", vbInformation, Me.Text)
                    Exit Function
                End If
            End If
        End If


        validFlds = True
    End Function

    Public Function FillDocDetails(ByVal argDocID As Long) As Boolean
        FillDocDetails = False

        txtDocID.Text = argDocID
        StMsg("Filling Header Fields...")
        Call fillDocHdrFlds(argDocID)
        txtDocID.Text = argDocID
        Dim argDsgrGrids = GetAllDsgrVsf(Me, GetType(C1FlexGrid))

        For Each argGrid As C1FlexGrid In argDsgrGrids
            If argGrid.Visible Then
                Call getMultiAmtFld(argGrid)
            End If
        Next

        StMsg("Filling Detail Fields...")
        Call fillDtlFields(argDocID)
        Call fillDenom(argDocID)
        FillDocDetails = True
        'Call showBtns()
        StMsg("")
        Call EnableDisableOptBtn(True)
        Mode = "M"
        txtScan.BackColor = Color.PaleVioletRed
        If gBlnCheckForFraud Then
            'dump vsfdet data to Temp Grid
            vsfTemp.Rows.Count = 0
            vsfTemp.Rows.Count = vsfDet.Rows.Count
            vsfTemp.Cols.Count = vsfDet.Cols.Count
            For i = 0 To vsfDet.Rows.Count - 1
                For j = 0 To vsfDet.Cols.Count - 1
                    vsfTemp.SetData(i, j, vsfDet.GetData(i, j))
                Next j
            Next i
        End If

        'Call showBtns()
    End Function

    Private Function fillDocHdrFlds(ByVal argDocID As Long) As Boolean
        Dim rsDocHdr As OleDb.OleDbDataReader = Nothing

        Call DateFreezing(Me, "")

        '  If intPartyDocID > 0 Then
        '    If 
        '    rsDocHdr = clsSVT.getDataReaderFromSQL("select h.*, " & strFldPartyCode & " from " & strDocHdrTblName & " h, " & strPartyTblName & " p " _
        '                                        & " where h." & strFldPartyID & " = p." & strFldPartyID & " and h." & strDocIDFldName & " = " & argDocID)
        'Else
        rsDocHdr = clsSVT.getDataReaderFromSQL("select * from " & strDocHdrTblName & "  " _
                                                            & " where " & strDocIDFldName & " = " & argDocID)
        '  End If

        Call ClearAddnlGrid(vsfTot)

        With rsDocHdr
            If Not .HasRows Then
                fillDocHdrFlds = False
                .Close()
                Exit Function
            End If
            .Read()

            txtDocID.Text = argDocID
            txtDocNo.Text = rsDocHdr(strDocNoFldName)
            txtSalesPerson.Text = clsSVT.getDataStringValue("Select SPName from MstSP where SPID = " & rsDocHdr!SPID, "", False)
            txtCount.Text = clsSVT.getDataStringValue("Select SCName from MstSC where ScID = " & rsDocHdr!ScID, "", False)
            txtPrintNo.Text = rsDocHdr("Print" & strDocNoFldName)

            'fill the Doc date
            dtDocDate.Value = Format(rsDocHdr(strDocDateFldName), "dd/MM/yyyy")
            If !cancelled = "Y" Then
                lblCancelled.Visible = True
            Else
                lblCancelled.Visible = False
            End If
            For Each argVsf As C1FlexGrid In ctrlList
                For i = 0 To argVsf.Rows.Count - 1
                    For X = 0 To .FieldCount - 1
                        If argVsf.GetData(i, vOthPerAmount) = "Y" Then
                            If UCase(.GetName(X)) = UCase(Trim(argVsf.GetData(i, vOthFldCode))) & "PER" Then
                                argVsf.SetData(i, vOthFldDataPer, GetVal(.GetValue(X), ""))
                            End If
                            If UCase(.GetName(X)) = UCase(Trim(argVsf.GetData(i, vOthFldCode))) & "AMT" Then
                                argVsf.SetData(i, vOthFldDataValue, GetVal(.GetValue(X), ""))
                            End If
                        Else
                            If UCase(.GetName(X)) = UCase(Trim(argVsf.GetData(i, vOthFldCode))) Then
                                If Trim(argVsf.GetData(i, vOthFldCmbTblName)) = "" Then
                                    argVsf.SetData(i, vOthFldDataValue, GetVal(.GetValue(X), ""))
                                Else
                                    Select Case UCase(argVsf.GetData(i, vOthFldDataType))
                                        Case "NUMERIC", "LIST NUMERIC"
                                            argVsf.SetData(i, vOthFldDataValue,
                                        clsSVT.getDataStringValue("Select " & argVsf.GetData(i, vOthFldCmbDisplayCols) & " from " &
                                        argVsf.GetData(i, vOthFldCmbTblName) & " where " &
                                        argVsf.GetData(i, vOthFldCmbStorageFldName) & " = " & GetVal(.GetValue(X), 0), "", False))
                                        Case Else
                                            argVsf.SetData(i, vOthFldDataValue,
                                        clsSVT.getDataStringValue("Select " & argVsf.GetData(i, vOthFldCmbDisplayCols) & " from " &
                                        argVsf.GetData(i, vOthFldCmbTblName) & " where " &
                                        argVsf.GetData(i, vOthFldCmbStorageFldName) & " = '" & GetVal(.GetValue(X), "") & "'", "", False))
                                    End Select


                                    'argVsf.SetData(i, vOthFldDataValue,
                                    '    clsSVT.getDataStringValue("Select " & argVsf.GetData(i, vOthFldCmbDisplayCols) & " from " &
                                    '    argVsf.GetData(i, vOthFldCmbTblName) & " where " &
                                    '    argVsf.GetData(i, vOthFldCmbStorageFldName) & " = " & GetVal(.GetValue(X), 0), "", False))
                                End If
                            End If
                        End If
                    Next X
                Next
            Next
            Call vsfGenHideMndtAllGrids(Me)
            strSearchQry = strItemSearchQry
            'If intPartyDocID > 0 AndAlso BlnEnforcePartyItemLink Then
            '    strSearchQry = strItemPartySearchQry.Replace("<ID>", Val(svtPCtrl.getBillID()))
            'End If

            fillDocHdrFlds = True
            .Close()
        End With
    End Function

    Public Function fillDtlFields(ByVal argDocID As Integer, Optional ByVal blnAppend As Boolean = False) As Boolean
        Dim i As Integer
        Dim strsql As String
        '--6 start CopyFROM
        Dim introw As Integer

        fillDtlFields = False
        Dim rsDtl As OleDb.OleDbDataReader


        strsql = "Select d.* "
        strsql = strsql & ", ItemCode, ItemName, UOMCode, "
        strsql = strsql & " isnull((Select CLBal From TrnQOH q Where q.ItemID=i.ItemID),0) + SInvQty as sQOH"
        strsql = strsql & " from  " & strDocDtlTblName & "  d "
        strsql = strsql & " ,MstItem i, MstUOM u"
        strsql = strsql & " where 1=1 "
        strsql = strsql & " And d.UOMID = u.UOMID "
        strsql = strsql & " And d.ItemID = i.ItemID "
        strsql = strsql & " And  " & strDocIDFldName & "= " & argDocID
        strsql = strsql & " Order by slNo"
        rsDtl = clsSVT.getDataReaderFromSQL(strsql)

        With rsDtl
            If Not .HasRows Then
                fillDtlFields = False
                .Close()
                Exit Function
            End If

            vsfDet.Rows.Count = 1
            Do While .Read
                vsfDet.Rows.Count = vsfDet.Rows.Count + 1
                introw = vsfDet.Rows.Count - 1
                i = introw
                vsfDet.SetData(i, vDetSlNo, !slNo)
                vsfDet.SetData(i, vDetItemSlNo, rsDtl!ItemSLNO)
                vsfDet.SetData(i, vDetItemID, rsDtl("ItemID"))
                vsfDet.SetData(i, vDetItemCode, rsDtl("ItemCode"))
                vsfDet.SetData(i, vDetItemName, rsDtl("Itemname"))
                vsfDet.SetData(i, vDetQOH, rsDtl("sQOH"))
                vsfDet.SetData(i, vDetUOM, !UOMCode)
                vsfDet.SetData(i, vDetLtrPerBot, GetVal(clsSVT.getDataDoubleValue("Select LTRPERBOT From MstPkg p, MstItem i where p.PKGID=i.PKGID and ItemID  = " & GetVal(rsDtl("ItemID"), 0), 0, False), 0))
                Call FillvsfOptDtlFlds(Me, vsfDet, vsfOptions, rsDtl, i, intDetCols)
            Loop
            .Close()
        End With
        Call RemoveEmptVSFRows(vsfDet, 2, vDetItemCode)
        Call ReArrangeSlNo(vsfDet, vDetSlNo)
        Call CalTotal()

        fillDtlFields = True
    End Function

    Private Sub prntCrpt()
        Dim frmv As New frmUtlReportViewer
        With frmv
            ' .MdiParent = frmMain
            .StartPosition = FormStartPosition.CenterScreen
            .ShowRptFileFromUtlOptions(intDocID, Val(txtDocID.Text), "", True)
            .Show()
        End With
    End Sub

    Private Sub deleteDoc()
        If Val(txtDocID.Text) = 0 Then Exit Sub
        Dim strType As String = ""
        Dim frmDelCancel As New frmUtlDelCancelDoc
        With frmDelCancel
            .argCaption = Me.Text
            .StartPosition = FormStartPosition.CenterScreen
            .ShowDialog()
            strType = .strDelCancelStatus
            .Dispose()
            If strType = "" Then Exit Sub
        End With


        Dim strErrMsg As String = ""
        If lblCancelled.Visible Then
            MsgBox("Document is already cancelled", MsgBoxStyle.Critical, "Cancel Document")
            Exit Sub
        End If
        ' End If
        If strType = "CANCELLED" Then
            If MsgBox("Are you sure to CANCEL this Document ?", vbCritical + vbYesNo, Me.Text & " Cancellation") = vbNo Then Exit Sub
        Else
            If MsgBox("Are you sure to DELETE this Document ?", vbCritical + vbYesNo, Me.Text & " Deletion") = vbNo Then Exit Sub
        End If
        svtTrn = svtConn.BeginTransaction(IsolationLevel.ReadCommitted)
        Try
            'If UCase(strAddReduceStock) = "ADD" Or UCase(strAddReduceStock) = "RED" Then

            '    Dim intOldLocID = clsSVT.getDataLongValue("Select LocID from " & strDocHdrTblName & " Where " & strDocIDFldName & "=" & Val(txtDocID.Text), 1, False, svtTrn)
            '    strSQL = "Insert into TrnStkFlow (DocId,TrnID,TrnDate,LocID,ItemId,DocQty) "
            '    strSQL &= " Select " & intDocID & "," & intMaxDocID & "," & getDateFormatInCond(dtDocDate.Value) & ","
            '    strSQL &= intOldLocID & ", ItemID, " & IIf(UCase(strAddReduceStock) = "ADD", " -1 * ", "") & " STKQTY From TrnDtl" & strSHForm & " where " & strDocIDFldName & " = " & Val(txtDocID.Text)
            '    clsSVT.execSQLStatement(strSQL, svtTrn)

            '    strSQL = " Insert Into TrnQOH (ItemID, LocID,  OpQty, TrnQty) "
            '    strSQL &= " Select s.ItemID, s.LocID, 0, 0 "
            '    strSQL &= " From TrnStkFlow s "
            '    strSQL &= " Left Join  TrnQOH q on (s.ItemID=q.ItemID and s.LocID=q.LOCID ) "
            '    strSQL &= " Where q.ItemID is Null "
            '    strSQL &= " Group by s.ItemID, s.LocID "
            '    clsSVT.execSQLStatement(strSQL, svtTrn)

            '    strSQL = " Update TrnQOH Set TrnQty = (Select isnull(sum(DocQty),0) "
            '    strSQL &= " from TrnStkFlow s "
            '    strSQL &= " where TrnQOH.ItemId=s.ItemID "
            '    strSQL &= " And TrnQOH.LocID=s.LocID) "
            '    clsSVT.execSQLStatement(strSQL, svtTrn)

            '    strSQL = " Update TrnQOH Set ClBal = OPQty + TrnQty "
            '    strErrMsg = clsSVT.execSQLStmtMsg(strSQL, svtTrn)
            '    If strErrMsg <> "" Then
            '        If InStr(UCase(strErrMsg), "CHECK CONSTRAINT") > 0 Then
            '            MsgBox("Stock Is going negative. Cannot Save.", MsgBoxStyle.Critical, "No Stock")
            '        Else
            '            MsgBox(strErrMsg & " Error during updating stock", MsgBoxStyle.Critical, "No Stock")
            '        End If
            '        svtTrn.Rollback()
            '        Exit Sub
            '    End If
            'End If

            'for audit track
            If blnAuditTrack Then
                Call RecordAuditLog(strFrmCaption, Val(txtDocID.Text), UserActions.DeleteAction, strDocHdrTblName, strAuditHdrSQL, strAuditDtlSQL, svtTrn)
            End If

            strSQL = " Delete From UtlStockList Where DocType='Inv' and DocID=" & Val(txtDocID.Text)
            strErrMsg = clsSVT.execSQLStmtMsg(strSQL, svtTrn)

            If strType = "DELETED" Then
                clsSVT.execSQLStatement("Delete from " & strDocDtlTblName & " where " & strDocIDFldName & " = " & Val(txtDocID.Text), svtTrn)
                clsSVT.execSQLStatement("Delete from " & strDocHdrTblName & " where " & strDocIDFldName & " = " & Val(txtDocID.Text), svtTrn)
            Else
                clsSVT.execSQLStatement("Update " & strDocHdrTblName & " Set Cancelled = 'Y' where " & strDocIDFldName & " = " & Val(txtDocID.Text), svtTrn)
            End If

            svtTrn.Commit()

            If strType = "DELETED" Then
                If gBlnSMSInvDeleted Then
                    Dim strSMS As String = getSMS(strType)
                    Call SendSMS(strSMS, gStrSalesMobNo, "")
                End If
                Call clearFlds()
            End If
            If strType = "CANCELLED" Then
                If gBlnSMSInvCancelled Then
                    Dim strSMS As String = getSMS(strType)
                    Call SendSMS(strSMS, gStrSalesMobNo, "")
                End If
            End If

            MsgBox(IIf(strType = "CANCELLED", "Cancelled", "Deleted") & Me.Text & " Successfully!", MsgBoxStyle.Critical, "Deleted")
        Catch ex As Exception
            svtTrn.Rollback()
            MsgBox(ex.Message, MsgBoxStyle.Information, "Cannot " & strType)
        End Try
    End Sub

    Public Function gridEmpty() As Boolean
        gridEmpty = False
        If Not (Mode = "A" Or Mode = "M") Then Exit Function
        If vsfDet.Rows.Count > 1 AndAlso vsfDet.Rows.Count = 2 AndAlso Trim(GetVal(vsfDet.GetData(1, vDetItemCode), "")) = "" Then
            gridEmpty = True
        End If


    End Function

    Private Sub frmTrnPartyDoc_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        strGCustCode = clsSVT.getDataStringValue("Select Top 1 CustCode From MstComp", "", False)
        strQRUrl = clsSVT.getDataStringValue("select SetValue from UtlGSettings where SetName = 'UPIurl'", "", False)
        strQRUrlCheck = clsSVT.getDataStringValue("select SetValue from UtlGSettings where SetName = 'APPROVEURL'", "", False)

        Me.KeyPreview = True
        Dim ToolTip As New ToolTip
        Dim blnNewMode As Boolean = False, blnLoadLastTrn As Boolean = False
        Call initFormVsfDet(Me, Me.Text, vsfDet, "DET")
        'Call initFormSC(Me, Me.Text, scMain, "Main")
        'Call initFormSC(Me, Me.Text, scTot, "Total")
        Dim argDsgrGrids = GetAllDsgrVsf(Me, GetType(C1FlexGrid))
        For Each GridList As C1FlexGrid In argDsgrGrids
            If GridList.Visible Then
                Call initFormVsfDet(Me, Me.Text, GridList, GridList.Name)
            End If
        Next
        'initGridStyles(Me)

        'lblTitle.Text = "View - " & Me.Text
        'fillFormProperties(Me)
        lblTitle.ForeColor = Color.White
        If Not vsfTot.Visible Then
            lblTotTip.Visible = False
        End If
        Call GridStylesUpdateFromDocDsgr()
        Call RemoveOldXML()
        Call LoadLastOrNewTrn(intDocID, blnNewMode, blnLoadLastTrn)
        If blnNewMode Then
            blnFirstTime = True
            'svtBtnCtrl_btnClicked(svtBtns.svtNew)
            If intPartyDocID > 0 Then
                txtScan.Focus()
            End If
        ElseIf blnLoadLastTrn Then
            If lngViewDocID = 0 Then
                strSQL = "Select max(" & getSyntax("ISNULL0", strDocIDFldName) & ") from " & strDocHdrTblName
            Else
                strSQL = "Select " & getSyntax("ISNULL0", strDocIDFldName) & " from " & strDocHdrTblName & " Where " & strDocIDFldName & " = " & lngViewDocID
            End If
            Call FillDocDetails(clsSVT.getDataLongValue(strSQL, 0, False))
        End If
        clearFlds()

        txtSalesPerson.Text = INIGet("Invoice", "SPID", "")

        If UCase(getGsettings("INVOICE", "ShowInvVal")) = "Y" Then
            Label12.Visible = True
            txtHdrInvVal.Visible = True
        Else
            Label12.Visible = False
            txtHdrInvVal.Visible = False
        End If

        Call loadQuickSel()
        If blnSMSFeature Then
            chkSMS.Visible = True
        Else
            chkSMS.Visible = False
        End If
        'lblLimit.Visible = False
        Mode = "A"


        lblItemName.Text = ""
        lblItemName.Visible = True
        txtScan.Focus()

        txtPoints.BackColor = Color.Yellow

        'Call chkHdrFlds()   ' to check if there are fields in Header section.

    End Sub

    Private Sub loadQuickSel()
        Dim strQuickSel As String = INIGet("QuickSelection", "QSetName", "")
        If Trim(strQuickSel) = "" Then Exit Sub

        Dim rs As OleDb.OleDbDataReader
        Dim strSQL As String

        strSQL = "Select i.itemid, i.ShortName, IsNull(CLBal,0) QOH "
        strSQL = strSQL & " from MstItem i "
        strSQL = strSQL & " Join MstQuickSelItems q on i.ItemID=q.ItemID "
        strSQL = strSQL & " Left Join TrnQOH s on i.ItemID=s.ItemID"
        strSQL = strSQL & "  where q.QSetName = '" & strQuickSel & "'"
        strSQL = strSQL & "  and i.itemid = q.itemid"
        strSQL = strSQL & " order by 2"

        rs = clsSVT.getDataReaderFromSQL(strSQL)
        vsfMenu.Rows.Count = 0
        vsfMenu.Cols.Count = 3
        With rs
            Do While .Read
                vsfMenu.Rows.Count = vsfMenu.Rows.Count + 1
                vsfMenu.SetData(vsfMenu.Rows.Count - 1, vQDetItemid, !ItemID)
                vsfMenu.SetData(vsfMenu.Rows.Count - 1, vQQDetItem, !ShortName)
                vsfMenu.SetData(vsfMenu.Rows.Count - 1, vQQOH, !QOH)
            Loop
            .Close()
        End With
        vsfMenu.AutoSizeCols()
    End Sub

    Private Sub vsfTot_AfterEdit(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfTot.AfterEdit
        Call vsfGenAfterEditBfrCalTot(Me, vsfTot, e.Row, "M")
        Call CalTotal()
        Call vsfGenAfterEditAftrCalTot(Me, vsfTot, intDocID, Mode, e.Row, False)
    End Sub

    Private Sub vsfTot_BeforeEdit(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfTot.BeforeEdit
        If vsfBeforeEditCancel(Me, vsfTot, intDocID, strDocHdrTblName, Mode, e.Row, e.Col, "M", "") Then
            e.Cancel = True
            Exit Sub
        End If
    End Sub

    Private Sub vsfTot_KeyPressEdit(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.KeyPressEditEventArgs) Handles vsfTot.KeyPressEdit
        If e.KeyChar = "'" Then
            e.Handled = True
            Exit Sub
        End If
        If vsfKeyPressEdit(Me, vsfTot, vsfTot.Row, e.KeyChar) Then
            e.Handled = False
        Else
            e.Handled = True
        End If
    End Sub

    Private Sub vsfTot_RowColChange(ByVal sender As Object, ByVal e As System.EventArgs) Handles vsfTot.RowColChange
        'If Not (Mode = "A" Or Mode = "M") Then Exit Sub
        ''lblToolTip.Text = ""
        ''lblToolTip.Text = vsfRowColChange(Me, vsfTot)
        ''If lblToolTip.Text <> "" Then
        ''    pnlToolTip.Visible = True
        ''Else
        ''    pnlToolTip.Visible = False
        ''End If

        'frmMain.tslblStatus.Text = ""
        'frmMain.tslblStatus.Text = vsfRowColChange(Me, vsfTot)
        ''If frmMain.tslblStatus.Text <> "" Then
        ''    pnlToolTip.Visible = True
        ''Else
        ''    pnlToolTip.Visible = False
        ''End If
        setToolTipMsg(vsfTot, lblTotTip, vsfTot.Row)
    End Sub

    Private Sub vsfTot_SetupEditor(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfTot.SetupEditor
        Call vsfSetupEditor(Me, vsfTot, intDocID, strDocHdrTblName, "A", e.Row, e.Col, "M")
    End Sub

    Private Sub vsfTot_ValidateEdit(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.ValidateEditEventArgs) Handles vsfTot.ValidateEdit
        If Not vsfValidateEdit(Me, vsfTot, e.Row, e.Col, "M", strDocHdrTblName, strDocNoFldName, txtPrintNo.Text) Then
            e.Cancel = True
        End If
    End Sub

    Private Sub vsfTot_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles vsfTot.KeyUp
        If e.KeyCode = Keys.Tab Then
            If vsfTot.Row = vsfTot.Rows.Count - 1 Then
                'svtBtnCtrl.Focus()
            End If
        End If
        If e.Control = True AndAlso e.KeyCode = Keys.G Then
            Call GridFocusChange("F")
        End If
    End Sub

    Private Sub svtBtnCtrl_btnOptionSelected(argOptionName As String, argBlnCtrlClick As Boolean, argBlnAltClick As Boolean)
        If argOptionName = "" Then Exit Sub
        If Val(txtDocID.Text) <= 0 Then Exit Sub

        Select Case argOptionName
            Case "SEND SMS"
                With New frmTrnSMS
                    .intDocId = intDocID
                    .initScreen(txtDocID.Text)
                    .ShowDialog()
                End With

            Case Else
                Dim strSF As String
                Dim strClickEvent As String = ""
                If argBlnCtrlClick Then
                    If argBlnAltClick Then
                        strClickEvent = clsSVT.getDataStringValue("Select CtrlAltClickOperation from UtlOption Where DocID=" & intDocID & " and OptionName='" & argOptionName & "'", "", False)
                    Else
                        strClickEvent = clsSVT.getDataStringValue("Select CtrlClickOperation from UtlOption Where DocID=" & intDocID & " and OptionName='" & argOptionName & "'", "", False)
                    End If
                Else
                    strClickEvent = clsSVT.getDataStringValue("Select ClickOperation from UtlOption Where DocID=" & intDocID & " and OptionName='" & argOptionName & "'", "", False)
                End If
                If strClickEvent = "M" Then
                    Dim obj As New frmRptMultiPrint
                    obj.StartPosition = FormStartPosition.CenterScreen
                    obj.strOptionname = argOptionName
                    obj.intDocId = intDocID
                    obj.LoadPrintSettings()
                    obj.ShowDialog()
                    obj.Dispose()
                    Exit Sub
                End If
                Dim strFormulas(,) As String
                strSF = "{TrnHdr" & strSHForm & "." & strSHForm & "ID} = " & Val(txtDocID.Text)
                ReDim strFormulas(1, 2)
                strFormulas = Nothing
                Dim frmv As New frmUtlReportViewer
                With frmv
                    .StartPosition = FormStartPosition.CenterScreen
                    .ShowRptFileFromUtlOptions_New(intDocID, Val(txtDocID.Text), argOptionName, argBlnCtrlClick, argFileName:=Strings.Replace(Me.Text, " ", "") & "_" & Trim(txtPrintNo.Text))
                    If strClickEvent = "P" Then
                        .Show()
                    End If
                End With
        End Select
    End Sub

    Public Function SaveDoc() As Boolean

        Dim intLocID As Int32 = 0
        Dim intOldLocID As Int32 = 0

        SaveDoc = False
        ' for adding / editing Record
        showWaitCursor()
        Dim strErrMsg As String = ""
        svtTrn = svtConn.BeginTransaction(IsolationLevel.ReadCommitted)
        Try


            intMaxDocID = Val(txtDocID.Text)
            clsSVT.execSQLStatement("Delete from " & strDocDtlTblName & " where " & strDocIDFldName & " = " & Val(txtDocID.Text), svtTrn)

            If Not UpdtDocHdrDtl() Then
                SaveDoc = False
                svtTrn.Rollback()
                Exit Function
            End If


            'for audit track
            If blnAuditTrack Then
                Call RecordAuditLog(strFrmCaption, Val(txtDocID.Text), IIf(Mode = "A", UserActions.AddAction, UserActions.EditAction), strDocHdrTblName, strAuditHdrSQL, strAuditDtlSQL, svtTrn)
            End If

            If gstrChkRateFld = "Y" Then
                Call updateSInvRateToMstItem(intMaxDocID, svtTrn)
            End If

            svtTrn.Commit()
            GC.Collect()
            SaveDoc = True
            If Mode = "M" Then
                MsgBox("Updated successfully!", vbInformation, Me.Text)
            Else
                MsgBox("Data saved successfully!", vbInformation, Me.Text)
            End If

        Catch ex As Exception
            svtTrn.Rollback()
            SaveDoc = False
            MsgBox(ex.Message, MsgBoxStyle.Information, Me.Text)
        Finally
            Cursor.Current = Cursors.Arrow
        End Try
    End Function

    Private Function UpdtDocHdrDtl() As Boolean

        UpdtDocHdrDtl = False

        If Mode = "A" Then
            strSQL = "Insert into " & strDocHdrTblName
            strSQL = strSQL & " (" & strDocNoFldName & ", Prefix, Print" & strDocNoFldName & ", "
            strSQL = strSQL & strDocDateFldName & ", "
            strSQL = strSQL & " AddedBy, AddedOn,"
            strSQL = strSQL & " Authorised, Cancelled"

            Call GetInsertString(Me, strValues, strFlds, svtTrn)

            If gDBTypeCurrent = "ACCESS" Then
                strSQL = strSQL & strFlds & ") values ("
            Else
                strSQL = strSQL & strFlds & ") output inserted." & strDocIDFldName & " into @testTable values ("
            End If
            strSQL = strSQL & "'" & txtPrintNo.Text & "', "
            strSQL = strSQL & "'" & getDateFormatInInsert(dtDocDate.Value) & "', "
            strSQL = strSQL & "'" & gUserCode & "', "
            strSQL = strSQL & "'" & getDateFormatInInsert(getSystemDateTime(svtTrn), True) & "', "
            strSQL = strSQL & " 'N'"
            strSQL = strSQL & strValues

            strDtlSQL = ""
            strDtlSQL = GetDetSqlString()
            intMaxDocID = getMaxHdrDtlInsertID(strSQL, strDtlSQL, strDocHdrTblName, svtTrn)
            txtDocID.Text = intMaxDocID
            If intMaxDocID = -1 Then
                UpdtDocHdrDtl = False
                Exit Function
            End If
        End If

        If Mode = "M" Then
            strSQL = "update " & strDocHdrTblName & " set "
            strSQL = strSQL & "ChangedBy = '" & gUserCode & "', "
            strSQL = strSQL & "ChangedOn = '" & getDateFormatInInsert(getSystemDateTime(svtTrn), True) & "', "

            If lblCancelled.Visible Then
                strSQL = strSQL & "Cancelled='Y' ,"
            Else
                strSQL = strSQL & "Cancelled='N' ,"
            End If

            strSQL = strSQL & strDocDateFldName & " = '" & getDateFormatInInsert(dtDocDate.Value) & "' "

            Call GetUpdateString(Me, strFlds, svtTrn)
            strSQL = strSQL & strFlds

            strSQL = strSQL & " where " & strDocIDFldName & " = " & txtDocID.Text
            If Not clsSVT.execSQLStatement(strSQL, svtTrn) Then
                UpdtDocHdrDtl = False
                Exit Function
            End If

            'Update dtl grid
            strDtlSQL = GetDetSqlString(Val(txtDocID.Text))
            If Not clsSVT.execSQLStatement(strDtlSQL, svtTrn) Then
                UpdtDocHdrDtl = False
                Exit Function
            End If
        End If
        UpdtDocHdrDtl = True
        'end add to Doc header
    End Function

    Private Function GetDetSqlString(Optional argDocID As Integer = 0) As String
        Dim strFlds1 = ""
        strValues = ""
        Dim strColFlds = ""
        Dim strColValues = ""

        Call GetvsfOptHdrDetInsertString(Me, vsfDet, vsfOptions, intDetCols, 1, strColFlds, strColValues, svtTrn)


        strFlds = "Insert Into  " & strDocDtlTblName

        strFlds = strFlds & " (" & strDocIDFldName & " , slNo,ItemSlNo, ItemID, UOMID, "
        For i = 1 To vsfDet.Rows.Count - 1
            If argDocID > 0 AndAlso Mode = "M" Then
                strValues = strValues & " Select  " & argDocID & ", "
            Else
                strValues = strValues & " Select  @MaxID, "
            End If

            With vsfDet
                strValues = strValues & Val(.GetData(i, vDetSlNo)) & ","
                strValues = strValues & Val(.GetData(i, vDetItemSlNo)) & ","
                strValues = strValues & Val(.GetData(i, vDetItemID)) & ","
                strValues = strValues & clsSVT.getDataLongValue("Select UOMID from MstUOM Where UOMCode ='" & GetVal(.GetData(i, vDetUOM), "-") & "'", 0, False, svtTrn) & ","
            End With

            Call GetvsfOptHdrDetInsertString(Me, vsfDet, vsfOptions, intDetCols, i, strFlds1, strValues, svtTrn)
            strValues = Mid(strValues, 1, Len(strValues) - 1)
            strValues = strValues & " Union All"
        Next

        strValues = Mid(strValues, 1, Len(strValues) - 9)

        GetDetSqlString = strFlds & strColFlds & " " & strValues
    End Function


    Private Sub vsfTot_EnterCell(sender As Object, e As System.EventArgs) Handles vsfTot.EnterCell
        If Mode = "V" Or Mode = "" Then Exit Sub
        Try
            With vsfTot
                If .GetData(.Row, vOthFldTxtBlnMultiLine) = "Y" Then Exit Sub
                If .GetData(.Row, vOthFldBlnComboData) = "Y" Then Exit Sub
                If .GetData(.Row, vOthFldDataType) = "List String" Then Exit Sub
                If .GetData(.Row, vOthFldBlnReadOnly) = "Y" Then Exit Sub
                If .GetData(.Row, vOthFldBlnRefill) = "Y" Then Exit Sub
                If .Col = vOthFldDataPer Then
                    If .GetData(.Row, vOthPerAmount) = "N" Then Exit Sub
                End If
                If .Col = vOthFldDataValue Then
                    If .GetData(.Row, vOthPerAmount) = "Y" Then Exit Sub
                End If
                ' .StartEditing()
            End With
        Catch
        End Try
    End Sub

    'Private Sub srcTransaction_TxtChanged(argValue As String) Handles srcTransaction.TxtChanged
    '    If argValue = "" Then
    '        srcTransaction.vsfAnchorReSize(410, 30)
    '        Exit Sub
    '    End If
    '    srcTransaction.LoadGrid(argValue, "", getSrcQry(argValue, True))
    '    srcTransaction.vsfAnchorReSize(655, 480)
    'End Sub

    'Private Sub srcTransaction_TxtKeyUp(argValue As Integer) Handles srcTransaction.TxtKeyUp
    '    If argValue > 0 Then
    '        srcTransaction.fillGrid(argValue, "")
    '        FillDocDetails(srcTransaction.getId())
    '        srcTransaction.vsfAnchorReSize(410, 30)
    '    Else
    '        srcTransaction.LoadGrid("", "", getSrcQry("", False))
    '        srcTransaction.vsfAnchorReSize(655, 480)
    '    End If
    'End Sub

    'Private Sub srcTransaction_vsfDoubleClick(argValue As Integer) Handles srcTransaction.vsfDoubleClick
    '    Call srcTransaction.fillGrid(argValue, "")
    '    FillDocDetails(srcTransaction.getId())
    '    srcTransaction.vsfAnchorReSize(410, 30)
    '    Call NextFocus()
    'End Sub

    'Private Sub srcTransaction_vsfKeyUp(argValue As Integer) Handles srcTransaction.vsfKeyUp
    '    Call srcTransaction.fillGrid(argValue, "")
    '    FillDocDetails(srcTransaction.getId())
    '    srcTransaction.vsfAnchorReSize(410, 30)
    '    Call NextFocus()
    'End Sub
    Private Sub TimerStartEvent()
        blnTimer = True
        strRestoreFileName = "Doc_No_" & Trim(txtPrintNo.Text) & "_" & strDateTime
        Timer1.Interval = 10000
        Timer1.Start()
    End Sub

    Private Sub Timer1_Tick(sender As System.Object, e As System.EventArgs) Handles Timer1.Tick
        If blnTimer = True And strRestoreFileName <> "" Then
            blnTimer = False
            AutoSaveGrid_Update()
        End If

    End Sub

    Private Sub AutoSaveGrid_Update()
        Try
            Dim argDsgrGrids = GetAllDsgrVsf(Me, GetType(C1FlexGrid))

            For Each argGrid As C1FlexGrid In argDsgrGrids
                If argGrid.Visible Then
                    If File.Exists(strRestorePath & strRestoreFileName & "_" & CStr(argGrid.Name) & ".xml") Then File.Delete(strRestorePath & strRestoreFileName & "_" & CStr(argGrid.Name) & ".xml")
                    argGrid.WriteXml(strRestorePath & strRestoreFileName & "_" & CStr(argGrid.Name) & ".xml")
                End If
            Next
            If File.Exists(strRestorePath & strRestoreFileName & ".xml") Then File.Delete(strRestorePath & strRestoreFileName & ".xml")
            If vsfDet.Rows.Count >= 2 Then
                vsfDet.WriteXml(strRestorePath & strRestoreFileName & ".xml")
            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Exclamation, Me.Text)
        End Try
        blnTimer = True
    End Sub

    Private Sub RestoreData()
        Dim strFileName As String = strRestoreFileName
        RestoreData_Grid(vsfDet, strFileName, "")
        Dim argDsgrGrids = GetAllDsgrVsf(Me, GetType(C1FlexGrid))
        For Each argGrid As C1FlexGrid In argDsgrGrids
            If argGrid.Visible Then
                RestoreData_Grid(argGrid, strFileName, "_" & argGrid.Name)
            End If
        Next
        strRestoreFileName = "Doc_No_" & Trim(txtPrintNo.Text) & "_" & strDateTime
    End Sub

    Private Sub RestoreData_Grid(ByVal argVsf As C1FlexGrid, ByVal argPath As String, ByVal argExt As String)
        With argVsf
            Try
                If Not File.Exists(strRestorePath & argPath & argExt & ".xml") Then Exit Sub
                .ReadXml(strRestorePath & argPath & argExt & ".xml")
            Catch ex As Exception
                MsgBox(ex.Message, MsgBoxStyle.Exclamation, Me.Text)
            End Try
        End With
    End Sub

    Private Sub TimerStopEvent()
        Timer1.Stop()
        Try
            If File.Exists(strRestorePath & strRestoreFileName & ".xml") Then
                File.Delete(strRestorePath & strRestoreFileName & ".xml")
            End If
            Dim argDsgrGrids = GetAllDsgrVsf(Me, GetType(C1FlexGrid))
            For Each argGrid As C1FlexGrid In argDsgrGrids
                If argGrid.Visible Then
                    If File.Exists(strRestorePath & strRestoreFileName & "_" & CStr(argGrid.Name) & ".xml") Then
                        File.Delete(strRestorePath & strRestoreFileName & "_" & CStr(argGrid.Name) & ".xml")
                    End If
                End If
            Next
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Exclamation, Me.Text)
        End Try
    End Sub

    Private Sub RemoveOldXML()

        Try
            strRestorePath = gstrAutoSave & ValidateFileName(Me.Text, "_") & "\"

            If Not IO.Directory.Exists(strRestorePath) Then
                IO.Directory.CreateDirectory(strRestorePath)
            Else
                Dim directory As New IO.DirectoryInfo(strRestorePath)
                For Each file As IO.FileInfo In directory.GetFiles
                    If (Now - file.CreationTime).Days > 1 Then file.Delete()
                Next
            End If

            clsSVT.execSQLStatement("Delete From UtlHoldBill Where SCID = " & intSalesCounterID & " And BillDateTime < " & getDateFormatInCond(getSystemDateTime) & "")



        Catch ex As Exception

        End Try
    End Sub

    Private Sub btnInsertfromDoc_Click(sender As System.Object, e As System.EventArgs)
        With frmUtlInsertfromDoc
            .intDocID = intDocID
            .argFrm = Me
            .ShowDialog()
        End With
    End Sub

    Private Sub raiseShortCutEvent(ByVal e As System.Windows.Forms.KeyEventArgs)

        'Focus on Detail grid
        If e.Control = True And e.Shift = True And e.KeyCode = Keys.D Then
            vsfDet.Row = 1
            vsfDet.Col = vDetItemCode
            vsfDet.Focus()
            Exit Sub
        End If

        'Focus on Party Detal

        If (e.KeyCode = Keys.F12 AndAlso e.Control = True) Or (e.KeyCode = Keys.S AndAlso e.Alt = True) Then
            If blnSavingInProgress Then Exit Sub
            blnSavingInProgress = True
            Call saveBill("C")
            blnSavingInProgress = False
            Exit Sub
        End If

        If (e.KeyCode = Keys.F12 AndAlso e.Alt = True) Or (e.KeyCode = Keys.S AndAlso e.Alt = True) Then
            If blnSavingInProgress Then Exit Sub
            blnSavingInProgress = True
            Call saveBill("A")
            blnSavingInProgress = False
            Exit Sub
        End If

        If (e.KeyCode = Keys.F12) Or (e.KeyCode = Keys.S AndAlso e.Alt = True) Then
            If btnPrint.Visible = True Then
                If blnSavingInProgress Then Exit Sub
                blnSavingInProgress = True
                Call saveBill("N")
                blnSavingInProgress = False
                Exit Sub
            End If
        End If



        If e.KeyCode = Keys.F2 Then
            If btnRePrint.Visible = True Then
                Call ViewBill("R")
                Exit Sub
            End If
        End If

        If e.KeyCode = Keys.F3 Then
            If btnCancel.Visible = True Then
                Call ViewBill("C")
                Exit Sub
            End If
        End If

        If e.KeyCode = Keys.F4 Then
            If gStrDBName.ToUpper() = "VLQMSDROPSJP" Then
                Exit Sub
            End If
            If btnDelete.Visible = True Then
                Call ViewBill("D")
                Exit Sub
            End If
        End If

            If e.KeyCode = Keys.F6 Then
            Call AddEditSel()
            Exit Sub
        End If

        If e.KeyCode = Keys.F7 Then
            Call ClearItems()
            dtDocDate.Value = getSystemDateTime()
            Exit Sub
        End If


        'focuse on Total Grid
        If e.Control = True And e.Shift = True And e.KeyCode = Keys.K Then
            vsfTot.Row = 0
            vsfTot.Col = 5
            vsfTot.Focus()
            Exit Sub
        End If


        'Toggle Hide
        If e.Control = True And e.KeyCode = Keys.T Then
            For i = 0 To vsfDet.Cols.Count - 1
                If vsfOptions.GetData(i, vOthBlnTogHide) = "Y" Then
                    If vsfDet.Cols(i).Visible = False Then
                        vsfDet.Cols(i).Visible = True
                    Else
                        vsfDet.Cols(i).Visible = False
                    End If
                End If
            Next
        End If

        If e.KeyCode = Keys.F5 Then
            Call btnHold_Click(Nothing, e)
            Exit Sub
        End If

        If e.KeyCode = Keys.F9 Then
            frmMain.CloseSelectedTabPage(Me)
        End If
    End Sub


    Private Sub SaveThisToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles SaveThisToolStripMenuItem.Click
        ' Call SaveFormSettings(intDocID, blnVHdr, blnVsfMax, blnVFtr, blnVTot)
    End Sub


    Private Sub vsfTot_LostFocus(sender As Object, e As System.EventArgs) Handles vsfTot.LostFocus
        'pnlToolTip.Visible = False
    End Sub


    'Private Sub showBtns()
    '    If Mode = "A" Or Mode = "M" Then Exit Sub
    '    With svtBtnCtrl
    '        If Val(txtDocID.Text) > 0 Then
    '            .btnDelete.Visible = True
    '            .btnEdit.Visible = True

    '            If .btnOptions.Items.Count > 0 Then
    '                .btnPrint.Visible = True
    '            End If
    '        Else
    '            .btnEdit.Visible = False
    '            .btnDelete.Visible = False
    '            .btnPrint.Visible = False
    '        End If

    '        If .btnOptions.Items.Count <= 0 Then
    '            .btnOptions.Visible = False
    '        End If
    '    End With
    'End Sub

    Private Sub setToolTipMsg(ByVal argVSF As C1FlexGrid, ByVal lbl As Label, ByVal argRow As Int32)
        If argRow < 0 Then Exit Sub
        If argVSF.Cols.Count >= vOthFldToolTip Then
            ' lbl.Text = GetVal(argVSF.GetData(argRow, vOthFldToolTip), "")
        Else
            'lbl.Text = ""
        End If
    End Sub


    Private Sub vsfTot_GotFocus(sender As Object, e As EventArgs) Handles vsfTot.GotFocus
        setToolTipMsg(vsfTot, lblTotTip, vsfTot.Row)
    End Sub

    Private Function ChkEditRights() As Boolean
        ChkEditRights = False

        If blnFreezeDoc Then
            MsgBox("Document Cannot edit Since it is freezed!", vbInformation, Me.Text)
            Exit Function
        End If

        If lblCancelled.Visible = True Then
            MsgBox("Current record is cancelled. Cannot edit!", vbInformation, Me.Text)
            Exit Function
        End If


        If intNumOfDays > 0 Then
            If DateAdd(DateInterval.Day, intNumOfDays, dtDocDate.Value) > getSystemDate() Then
                MsgBox("Edit limit days crossed... Cannot edit!", vbInformation, Me.Text)
                Exit Function
            End If
        End If
        Return True
    End Function



    Private Sub vsfTot_MouseClick(sender As Object, e As MouseEventArgs) Handles vsfTot.MouseClick
        If e.Button = MouseButtons.Right Then
            If sender.Rows.Fixed >= 1 Then
                sender.Rows.Fixed = 0
                Call SaveActFormVsfDet(Me, Me.Text, sender.Name, sender)
            Else
                For i = 0 To sender.Rows.Count - 1
                    If sender.Rows(i).Visible = True Then
                        sender.Rows.Fixed = i + 1
                        Exit For
                    End If
                Next
            End If
        End If
    End Sub

    Private Sub GridFocusChange(ByVal argGrid As String)
        Select Case argGrid
            Case "D"
                If SetGridFocuschange(vsfTot, "T") Then Exit Sub
            Case "H"
                If SetGridFocuschange(vsfTot, "T") Then Exit Sub
                If SetGridFocuschange(vsfDet, "D") Then Exit Sub
            Case "F"
                If SetGridFocuschange(vsfTot, "T") Then Exit Sub
                If SetGridFocuschange(vsfDet, "D") Then Exit Sub
            Case "T"
                If SetGridFocuschange(vsfDet, "D") Then Exit Sub
        End Select
    End Sub

    Private Function SetGridFocuschange(ByVal argVSF As C1FlexGrid, argGrid As String) As Boolean
        SetGridFocuschange = False
        If argGrid = "D" Then
            If argVSF.Visible AndAlso argVSF.Rows.Count > 1 Then
                argVSF.Row = 1
                argVSF.Col = vDetItemCode
                argVSF.Focus()
                Return True
            End If
        Else
            If argVSF.Visible Then
                For i As Int32 = 0 To argVSF.Rows.Count - 1
                    If argVSF.GetData(i, vOthFldBlnReadOnly) = "N" Then
                        argVSF.Row = i
                        argVSF.Col = vOthFldDataValue
                        argVSF.Focus()
                        Return True
                    End If
                Next
            End If
        End If
    End Function

    Private Sub vsfDet_KeyDown(sender As Object, e As KeyEventArgs) Handles vsfDet.KeyDown, vsfTot.KeyDown
        If e.Control = True AndAlso e.KeyCode = Keys.G Then
            Select Case UCase(sender.Name)
                Case "VSFHDR"
                    Call GridFocusChange("H")
                Case "VSFFTR"
                    Call GridFocusChange("F")
                Case "VSFTOT"
                    Call GridFocusChange("T")
                Case "VSFDET"
                    Call GridFocusChange("D")
            End Select
            Exit Sub
        End If
    End Sub

    Private Sub GridStylesUpdateFromDocDsgr()
        Dim argDsgrGrids = GetAllDsgrVsf(Me, GetType(C1FlexGrid))

        For Each GridList As C1FlexGrid In argDsgrGrids
            With GridList
                If .Visible Then
                    For i As Int32 = 0 To .Rows.Count - 1
                        If Val(.GetData(i, vOthFldRowClrForeColour)) > 0 Then
                            Dim csf1 As C1.Win.C1FlexGrid.CellStyle = .Styles.Add("StyleClr" & i)
                            Dim myColor As Color = ColorTranslator.FromWin32(Val(.GetData(i, vOthFldRowClrForeColour)))
                            csf1.BackColor = myColor
                            .Rows(i).Style = csf1
                        End If
                    Next
                End If
            End With
        Next

        With vsfOptions
            For i As Int32 = 0 To .Rows.Count - 1
                If Val(.GetData(i, vOthFldRowClrForeColour)) > 0 Then
                    Dim csf1 As C1.Win.C1FlexGrid.CellStyle = vsfDet.Styles.Add("DtlStyleClr" & i)
                    Dim myColor As Color = ColorTranslator.FromWin32(Val(.GetData(i, vOthFldRowClrForeColour)))
                    csf1.BackColor = myColor
                    vsfDet.Cols(i).Style = csf1
                End If
            Next
        End With

    End Sub


    Private Sub vsfTot_OwnerDrawCell(sender As System.Object, e As C1.Win.C1FlexGrid.OwnerDrawCellEventArgs) Handles vsfTot.OwnerDrawCell
        If sender.Rows(e.Row).Style IsNot Nothing Then
            If sender.Rows(e.Row).Style.Name = "StyleClr" & e.Row Then
                e.Style = sender.Styles("StyleClr" & e.Row)
            End If
        End If
    End Sub

    Private Sub vsfDet_OwnerDrawCell(sender As System.Object, e As C1.Win.C1FlexGrid.OwnerDrawCellEventArgs) Handles vsfDet.OwnerDrawCell
        'If vsfDet.Rows.Count <= 0 Then Exit Sub
        'If vsfDet.GetCellStyle(e.Row, e.Col) IsNot Nothing Then
        '    If IsNothing(vsfDet.Cols(e.Col).Style.Name) Then Exit Sub
        '    If vsfDet.Cols(e.Col).Style.Name = "DtlStyleClr" & e.Col Then
        '        e.Style = vsfDet.Styles("DtlStyleClr" & e.Col)
        '    End If
        'End If
    End Sub

    'Private Sub lblSearchClose_Click(sender As Object, e As EventArgs)
    '    srcTransaction.vsfView.Visible = False
    '    lblSearchClose.Visible = False
    '    srcTransaction.txtParty.Text = ""
    '    srcTransaction.txtParty.Focus()
    'End Sub

    Private Sub vsfDet_CellButtonClick(sender As Object, e As RowColEventArgs) Handles vsfDet.CellButtonClick
        If vsfOptions.GetData(e.Col, vOthFldTxtBlnMultiLine) = "Y" Then
            StrgetText = vsfDet.GetData(e.Row, e.Col)
            Call vsfOptCellButtonClick(StrgetText, Val(vsfOptions.GetData(e.Col, vOthFldDataWidth)), vsfDet.GetData(0, e.Col), Me, IIf(GetVal(vsfOptions.GetData(e.Col, vOthFldBlnReadOnly), "") = "Y", True, False))
            vsfDet.SetData(e.Row, e.Col, StrgetText)
        End If
    End Sub

    Private Sub txtScan_TextChanged(sender As Object, e As EventArgs) Handles txtScan.TextChanged
        '  CalTotal()

        If gStrDBName.ToUpper = "VLQMSCLH" Then

            'Dim lngItemID As Integer
            'Dim blnFound As Boolean

            'lngItemID = clsSVT.getDataLongValue("select ItemID from Mstitem Where ItemCode='" & Trim(txtScan.Text) & "'", 0, False)
            'If lngItemID > 0 Then
            '    If blnQtySel Then
            '        txtQty.Text = 1
            '        txtScan.Text = clsSVT.getDataStringValue("Select ItemCode from MstItem Where ItemID = " & lngItemID, "", blnFound)
            '        lblItemName.Text = clsSVT.getDataStringValue("Select ItemName from MstItem Where ItemID = " & lngItemID, "", blnFound)
            '        txtQty.Focus()
            '    Else
            '        'write code to update item to grid with qty as 1
            '        'txtQty.Text = 1
            '        AddItemToGrid(lngItemID)
            '        'txtScan.Text = ""
            '        lblItemName.Text = ""
            '        CalTotal()
            '    End If
            ' Exit Sub
            'endd If
        End If
    End Sub

    Private Sub frmTrnPartyDoc_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.Control AndAlso e.Shift Then
            'Select Case e.KeyCode
            '    Case Keys.L
            '        Call svtbtnctrl_Shortcut("ML")
            '    Case Keys.F
            '        Call svtbtnctrl_Shortcut("MF")
            '    Case Keys.P
            '        Call svtbtnctrl_Shortcut("MP")
            '    Case Keys.N
            '        Call svtbtnctrl_Shortcut("MN")
            '    Case Keys.F
            '        Call svtbtnctrl_Shortcut("")
            'End Select
        End If
    End Sub
    Private Sub vsfMenu_BeforeEdit(sender As Object, e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfMenu.BeforeEdit
        e.Cancel = True
    End Sub

    Private Sub txtScan_KeyUp(sender As Object, e As KeyEventArgs) Handles txtScan.KeyUp
        Dim lngItemID As Integer
        Dim blnFound As Boolean
        If e.KeyCode = Keys.Down Then
            lngItemID = showItemPopup()
            If lngItemID > 0 Then
                If blnQtySel Then
                    txtQty.Text = 1
                    txtScan.Text = clsSVT.getDataStringValue("Select ItemCode from MstItem Where ItemID = " & lngItemID, "", blnFound)
                    lblItemName.Text = clsSVT.getDataStringValue("Select ItemName from MstItem Where ItemID = " & lngItemID, "", blnFound)
                    txtQty.Focus()
                Else
                    'write code to update item to grid with qty as 1
                    txtQty.Text = 1
                    AddItemToGrid(lngItemID)
                    txtScan.Text = ""
                    lblItemName.Text = ""

                    Call showPoints()
                    CalTotal()

                End If
                Exit Sub
            End If
        End If

        If Trim(txtScan.Text) = "+" AndAlso vsfDet.Row >= 1 Then
            txtScan.Text = ""
            If vsfDet.GetData(vsfDet.Row, vDetItemCode) <> "" Then
                vsfDet.SetData(vsfDet.Row, intColQty, Val(vsfDet.GetData(vsfDet.Row, intColQty) + 1))
                Call CalTotal()
            End If
            Exit Sub
        End If

        If Trim(txtScan.Text) = "-" AndAlso vsfDet.Row >= 1 Then
            txtScan.Text = ""
            If vsfDet.GetData(vsfDet.Row, vDetItemCode) <> "" Then
                vsfDet.SetData(vsfDet.Row, intColQty, Val(vsfDet.GetData(vsfDet.Row, intColQty) - 1))
                Call CalTotal()
            End If
            Exit Sub
        End If

        If e.KeyCode = Keys.Return Or e.KeyCode = Keys.Tab Then
            txtScan.Text = Trim(UCase(txtScan.Text))
            If txtScan.Text = "" Then
                If vsfDet.Rows.Count > 1 Then
                    'btnSave.Focus()
                    Exit Sub
                End If
            End If

            If Trim(txtScan.Text) <> "" Then
                If InStr(Trim(txtScan.Text), "SVTQRCODE") > 0 Then
                    Call MultiItemsFromQRCode(Trim(txtScan.Text).Replace("SVTQRCODE", ""))
                    Exit Sub
                End If

                'sngQOH = getQOH(txtScan.Text)
                lngItemID = clsSVT.getDataLongValue("Select ItemID from MstItem where ItemCode = '" & Trim(txtScan.Text) & "'", 0, blnFound)
                If Not blnFound Then
                    lngItemID = clsSVT.getDataLongValue("Select ItemID from vwItemAlias where chkItemCode = '" & Trim(txtScan.Text) & "'", 0, blnFound)
                    txtScan.Text = clsSVT.getDataStringValue("Select ItemCode from vwItemAlias Where chkItemCode = '" & Trim(txtScan.Text) & "'", "", blnFound)
                End If
                If Not blnFound Then
                    MsgBox("Item not found!!", vbInformation, Me.Text)
                    txtScan.Text = ""
                    lblItemName.Text = ""
                    Exit Sub
                Else
                    'If isPriceZero(lngItemID) Then
                    '    Exit Sub
                    'End If
                End If


                lblItemName.Text = clsSVT.getDataStringValue("Select ItemName from MstItem Where ItemCode = '" & Trim(txtScan.Text) & "'", "", blnFound)

                If blnQtySel Then
                    txtQty.Text = 1
                    txtQty.Focus()
                Else
                    'write code to update item to grid with qty as 1
                    txtQty.Text = 1
                    AddItemToGrid(lngItemID)
                    'Call txtQty_KeyUp(vbKeyReturn, 0)
                    txtScan.Text = ""
                    lblItemName.Text = ""
                End If

                Call showPoints()
                CalTotal()

            End If
        End If

        If e.KeyCode = Keys.Escape Then
            btnPrint.Focus()
            Exit Sub
        End If
    End Sub

    Private Sub MultiItemsFromQRCode(ByVal argItemCodeList As String)
        Dim lngItemID As Long = 0, blnFound As Boolean = False
        For Each strItemList As String In argItemCodeList.Split(";")
            Dim intCount As Int32 = 0
            For Each str As String In strItemList.Split(",")
                If intCount = 0 Then
                    'sngQOH = getQOH(txtScan.Text)
                    lngItemID = clsSVT.getDataLongValue("Select ItemID from MstItem where ItemCode = '" & Trim(str) & "'", 0, blnFound)
                    If Not blnFound Then
                        lngItemID = clsSVT.getDataLongValue("Select ItemID from vwItemAlias where chkItemCode = '" & Trim(str) & "'", 0, blnFound)
                        txtScan.Text = clsSVT.getDataStringValue("Select ItemCode from vwItemAlias Where chkItemCode = '" & Trim(str) & "'", "", blnFound)
                    End If
                    If Not blnFound Then
                        MsgBox(str & " Item not found!!", vbInformation, Me.Text)
                        txtScan.Text = ""
                        lblItemName.Text = ""
                        Exit Sub
                    End If
                Else
                    txtQty.Text = Val(str)
                End If
                intCount = intCount + 1
            Next
            AddItemToGrid(lngItemID)
            CalTotal()
        Next
    End Sub


    Private Sub btnAddSet_Click(sender As Object, e As EventArgs)
        Dim objFrm As New frmMstItemQuickSel
        objFrm.ShowDialog()
    End Sub

    Private Sub btnAddParty_Click(sender As Object, e As EventArgs) Handles btnAddParty.Click
        txtParty.Focus()
        Call frmMain.OpenGeneralForm("Customer Master")
        'Dim objFrm As New frmUtlAddParty
        'With objFrm
        '    .initForm(intPartyDocID, Me)
        '    If .ShowDialog() = DialogResult.Cancel Then
        '        txtParty.Focus()
        '        vsfCust.SendToBack()
        '    End If
        'End With
    End Sub
    Private Sub AddItemToGrid(argItemID As Long)
        If gIntInvCtr > 100 Then
            MsgBox("Application needs to be closed. Will get restarted.", MsgBoxStyle.Critical, Me.Text)
            GC.Collect()
            frmMain.Close()
            Exit Sub
        End If


        Dim rs As OleDb.OleDbDataReader
        rs = clsSVT.getDataReaderFromSQL("Select * from MstItem Where ItemID=" & argItemID)

        If Not rs.HasRows Then
            rs.Close()
            MsgBox("Invalid Item....", MsgBoxStyle.Exclamation, Me.Text)
            Exit Sub
        End If

        Dim introw As Integer = 0

        If Val(txtQty.Text) = 0 Then
            txtQty.Text = 1
        End If

        With vsfDet
            If .Rows.Count > 1 Then
                introw = vsfDet.FindRow(argItemID, 1, vDetItemID, False, True, False)
            End If

            If introw > 0 Then
                rs.Close()
                .SetData(introw, intColQty, Val(.GetData(introw, intColQty)) + Val(txtQty.Text))

            Else
                If .Rows.Count > 1 Then
                    introw = .Rows.Count - 1
                    If Val(.GetData(introw, vDetItemID)) > 0 Then
                        .Rows.Count = .Rows.Count + 1
                        introw = .Rows.Count - 1
                    End If
                Else
                    .Rows.Count = .Rows.Count + 1
                    introw = .Rows.Count - 1
                End If

                rs.Read()
                .TopRow = introw
                .Row = introw

                .SetData(introw, vDetItemID, argItemID)
                .SetData(introw, vDetItemCode, rs.Item("ItemCode"))
                .SetData(introw, vDetItemName, rs.Item("Itemname"))
                .SetData(introw, vDetQOH, clsSVT.getDataDoubleValue("Select CLBal From TrnQOH Where ItemID=" & Val(argItemID), 0, False))
                .SetCellImage(introw, vDetSlNo, My.Resources.cancel)
                .SetData(introw, intColQty, Val(.GetData(introw, intColQty)) + Val(txtQty.Text))
                .SetData(introw, intColrate, GetVal(rs.Item("MRP"), 0))
                .SetData(introw, vDetLtrPerBot, clsSVT.getDataDoubleValue("Select LTRPERBOT From MstPkg where PKGID  = " & GetVal(rs.Item("PKGID"), 0), 0, False))
                .SetData(introw, vDetUOM, clsSVT.getDataStringValue("Select UOMCode From MstUOM where UOMID  = " & GetVal(rs.Item("UOMID"), 0), "", False))
                .SetData(introw, vDetLiqType, GetVal(rs.Item("LiqType"), ""))
            End If
            Call ReArrangeSlNo(vsfDet, vDetSlNo)
        End With
        rs.Close()
        Call RemoveEmptVSFRows(vsfDet, 1, vDetItemCode)
        vsfDet.Row = introw
        'Call InsertPrevItemData(dtDetPrevTran, Val(svtPCtrl.getBillID()), vsfDet.GetData(argRow, vDetItemID), argRow)
        Call fillItemRelatedDataFromDocDsgnr(intDocID, Val(vsfDet.GetData(introw, vDetItemID)), Me)
        'Call vsfOptValidateEdit(Me, vsfDet, vsfOptions, introw, vDetItemCode, "T", strDocDtlTblName, "", "")
        Call fillVsfDetDefValues(vsfDet, vsfOptions, introw)
        'Call ItemDetailsFillAfterPartySelection(introw)

        'Change row colour for duplicate items
        'For j As Int32 = 1 To vsfDet.Rows.Count - 1
        '    If Trim(txtScan.Text) = Trim(vsfDet.GetData(j, vDetItemName)) Then
        '        csRow.BackColor = Color.LightGreen
        '        vsfDet.Rows(j).Style = csRow
        '    End If
        'Next
        If vsfDet.Rows.Count > 1 Then
            EnableDisableOptBtn(True)
        Else
            EnableDisableOptBtn(False)
        End If
        If Not gBlnAllowNegativeStock Then
            If Val(vsfDet.GetData(introw, intColQty)) > Val(vsfDet.GetData(introw, vDetQOH)) Then
                MsgBox("Insufficient Qty!", MsgBoxStyle.Critical, "No Stock")
            End If
        End If
        txtScan.Text = ""
        txtScan.Focus()
        Call showPoints()
    End Sub

    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click

        If CDate(dtDocDate.Value) <= gdtFreezeDate Then
            MsgBox("Document cannot be Deleted. Date is before Freezed Date (Global Settings)!!", MsgBoxStyle.Critical, Me.Text)
            Exit Sub
        End If
        If gStrDBName.ToUpper() = "VLQMSDROPSJP" Then Exit Sub
        ViewBill("D")
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        ViewBill("C")
    End Sub

    Private Sub btnRePrint_Click(sender As Object, e As EventArgs) Handles btnRePrint.Click
        ViewBill("R")
    End Sub

    Private Sub btnPrint_Click(sender As Object, e As EventArgs) Handles btnPrint.Click
        If clsSVT.getDataStringValue("Select ReWriteINPROGRESS from UtlMisc", "N", False) = "Y" Then
            MsgBox("re-Writing Stocks.. Try after few mins.", MsgBoxStyle.Critical, Me.Text)
            Exit Sub
        End If

        If blnSavingInProgress Then Exit Sub
        blnSavingInProgress = True
        Call saveBill("N")
        blnSavingInProgress = False

        If vsfDet.Rows.Count > 1 Then
            EnableDisableOptBtn(True)
        Else
            EnableDisableOptBtn(False)
        End If

        gIntInvCtr = gIntInvCtr + 1
        If gIntInvCtr > 100 Then
            MsgBox("Application needs to be closed. Will get restarted.", MsgBoxStyle.Critical, Me.Text)
            GC.Collect()
            frmMain.Close()
        End If
    End Sub

    Private Sub ViewBill(ByVal argType As String)
        Dim objFrm As New frmTrnBill
        With objFrm
            .frmInv = Me
            .strType = argType
            .strCboText = strText
            .strRights = strRights
            .intSalesCounterID = intSalesCounterID
            .blnPDFPreview = blnPDFPreview
            .blnAuditTrack = blnAuditTrack
            .strInvAuditHdrTbl = strDocHdrTblName
            .strInvAuditHdrSql = strAuditHdrSQL
            .strInvAuditDtlSql = strAuditDtlSQL
            .ShowDialog()
        End With
        txtScan.Focus()
    End Sub

    Private Function showItemPopup() As Long
        Dim strSQL As String
        strSQL = "Select ShortName,cast(CLBal  as decimal (10,0)) as QOH, MRP, ItemCode, ItemName, ItemGroupName, i.ItemID "
        strSQL &= " From MstItem i"
        strSQL &= " Join MstItemGroup g on g.ItemGroupID=i.ItemGroupID "
        strSQL &= " Left Join TrnQOH t on i.ItemID=t.ItemID "
        strSQL &= " where  "
        strSQL &= " i.InActive = 'N' "
        strSQL &= " and i.NoSelInvoice <> 'Y' "
        If Not gBlnAllowNegativeStock Then
            strSQL &= " and clBal > 0"
        End If
        strSQL &= " order by 1"
        txtDummy.Text = ""
        txtDummy2.Text = ""
        Dim objfrm As New frmUtlSelection
        With objfrm
            .blnMultiColSearch = True
            .FillViewVsf(Me, False, Nothing, strSQL, txtDummy, 0, txtDummy2, 6)
            .Text = " Please Select Item"
            .Tag = " frmTrnInv "
            .vsfDet.Cols(6).Visible = False
            .ShowDialog()
            .Close()
        End With
        showItemPopup = 0
        If Trim(txtDummy.Text) <> "" Then
            'Call FillItemDet(txtDummy.Text, vsfDet.Row, argUOM:=txtDummy2.Text)
            showItemPopup = Val(txtDummy2.Text)
        End If
    End Function
    Private Sub vsfDet_MouseClick(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles vsfDet.MouseClick
        Dim ht = vsfDet.HitTest

        With vsfDet
            If ht.Row <= 0 Then Exit Sub
            If ht.Column <> vDetSlNo Then Exit Sub
            If MsgBox("Are you sure you want to delete the item '" & .GetData(ht.Row, vDetItemName) & "'?", vbYesNo + vbQuestion + vbDefaultButton2, "Item deletion...") = MsgBoxResult.No Then Exit Sub
            .RemoveItem(ht.Row)
            Call ReArrangeSlNo(vsfDet, vDetSlNo)

            Call showPoints()
            Call CalTotal()


            If ht.Row <= 0 Then

                btnPrint.Visible = False
            End If
            'lblLimit.Visible = False
        End With
    End Sub


    Private Sub txtQty_KeyUp(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles txtQty.KeyUp
        Dim lngItemId As Long, blnFound As Boolean
        'Dim sngQOH As Single, strNonStk As String
        Dim introw As Integer, i As Integer

        If e.KeyCode = Keys.Escape Then
            txtQty.Text = ""
            txtScan.Text = ""
            lblItemName.Text = ""
            txtScan.Focus()
            Exit Sub
        End If

        If e.KeyCode = Keys.Return Then
            If Trim(txtScan.Text) = "" AndAlso vsfDet.Row <= 0 Then
                MsgBox("Please select an item first!", vbInformation, Me.Text)
                txtScan.Focus()
                Exit Sub
            End If

            If Trim(txtScan.Text) = "" AndAlso vsfDet.Rows.Count <= 1 Then Exit Sub

            If Trim(txtQty.Text) = "" Then txtQty.Text = "1"

            If Val(txtQty.Text) <= 0 Then
                MsgBox("Invalid Qty", vbInformation, Me.Text)
                txtQty.Focus()
                Exit Sub
            End If

            lngItemId = clsSVT.getDataLongValue("Select ItemID from MstItem Where ItemCode = '" & txtScan.Text & "'" &
                                                " OR HotKey = '" & txtScan.Text & "' ", 0, False)

            If lngItemId <= 0 Then
                If Val(txtQty.Text) > 0 AndAlso vsfDet.Row > 0 Then
                    vsfDet.SetData(vsfDet.Row, intColQty, Val(txtQty.Text))
                    Call CalTotal()
                    txtScan.Focus()
                    Exit Sub
                Else
                    MsgBox("Invalid Item!!", vbInformation, Me.Text)
                    txtScan.Focus()
                    Exit Sub
                End If
            Else
                'If isPriceZero(lngItemId) Then
                'Exit Sub
                'End If
            End If

            blnFound = False
            With vsfDet
                For i = 1 To .Rows.Count - 1
                    If .GetData(i, vDetItemCode) = txtScan.Text Then
                        blnFound = True
                        introw = i
                        Exit For
                    End If
                Next i
            End With

            If Not blnFound Then
                vsfDet.Rows.Count = vsfDet.Rows.Count + 1
                vsfDet.Row = vsfDet.Rows.Count - 1
                vsfDet.TopRow = vsfDet.Rows.Count - 1

                AddItemToGrid(lngItemId)
            Else
                vsfDet.SetData(introw, intColQty, Val(vsfDet.GetData(introw, intColQty) + Val(txtQty.Text)))
                vsfDet.Row = introw
            End If

            Call showPoints()
            Call CalTotal()

            txtScan.Text = ""
            lblItemName.Text = ""
            txtQty.Text = "1"

            txtScan.Focus()
        End If

        If vsfDet.Rows.Count > 1 Then
            Call RemoveEmptVSFRows(vsfDet, 1, vDetItemCode)
            ReArrangeSlNo(vsfDet, vDetSlNo)
        End If

        'If Val(vsfTot.GetData(intRowTotPointsRdmd, vOthFldDataValue)) > 0 Then
        '    vsfTot.SetData(intRowInvVal, vOthFldDataValue, (Val(vsfTot.GetData(intRowCashAmt, vOthFldDataValue)) - Val(vsfTot.GetData(intRowTotPointsRdmd, vOthFldDataValue))))
        'End If

    End Sub

    Private Sub vsfSize()
        vsfCust.Size = New Point(txtParty.Size.Width + btnAddParty.Size.Width + 6, 300)
    End Sub

    Public Sub LoadFilterParty(ByVal argPartyType As String, ByVal argCode As String, ByVal argCond As String, ByVal argFlds As String)
        Dim dt As DataTable
        Dim strSQL As String = ""
        vsfCust.Visible = True
        Call vsfSize()

        strSQL = "Select Top " & gIntTopNrows & " " & argPartyType & "ID," & argPartyType & "Name as Name "
        If argFlds <> "" Then
            strSQL = strSQL & "," & argFlds
        End If
        strSQL = strSQL & " from Mst" & argPartyType
        strSQL = strSQL & " Where InActive = 'N' "
        strSQL = strSQL & argCond
        strSQL = strSQL & " and (" & argPartyType & "Code like '%" & argCode & "%' or " & argPartyType & "Name like '%" & argCode & "%' or PHNO like '%" & argCode & "%')"
        strSQL = strSQL & " order by 2"

        If strSQL = "" Then Exit Sub

        With vsfCust
            Call showWaitCursor()
            dt = clsSVT.getDATATable(strSQL)
            .DataSource = dt
            'Call SetColWidth(vsfView, 100, 100, 100)
            .Cols(0).Visible = False
            .Refresh()
            Call showNormalCursor()
        End With
    End Sub

    Private Sub txtParty_TextChanged(sender As Object, e As EventArgs) Handles txtParty.TextChanged
        LoadFilterParty("CUST", txtParty.Text, "", "PhNo")
        If txtParty.Text = "" Then vsfCust.Visible = False
    End Sub

    Private Sub vsfCust_BeforeEdit(sender As Object, e As RowColEventArgs) Handles vsfCust.BeforeEdit
        e.Cancel = True
    End Sub

    Private Sub vsfCust_KeyUp(sender As Object, e As KeyEventArgs) Handles vsfCust.KeyUp
        Dim intCustid As Int32, strSendSMS As String
        If e.KeyCode = Keys.Enter And vsfCust.Row > 0 Then
            txtParty.Text = vsfCust.GetData(vsfCust.Row, vCustName) & " - " & vsfCust.GetData(vsfCust.Row, vCustPhNo)

            intCustid = clsSVT.getDataLongValue("Select CustID from MstCust where InActive = 'N' and Custname + ' - ' + PHNo = '" & txtParty.Text & "'", 0, False)

            strSendSMS = clsSVT.getDataStringValue("select sendSms from mstCust where CustID=" & intCustid, "N", False)
            If strSendSMS = "Y" Then
                chkSMS.Checked = True
            Else
                chkSMS.Checked = False
            End If

            txtScan.Focus()
            vsfCust.Visible = False
            If blnLoyalty And txtParty.Text <> "" Then Call showPoints()
        End If
        If e.KeyCode = Keys.Escape Then
            txtParty.Text = ""
            txtScan.Focus()
            vsfCust.Visible = False
        End If
    End Sub

    Private Sub txtParty_KeyUp(sender As Object, e As KeyEventArgs) Handles txtParty.KeyUp
        Select Case e.KeyCode
            Case Keys.Enter
                If txtParty.Text = "" Then Exit Sub
                If blnLoyalty Then
                    Dim blnFound As Boolean, strPrty As String
                    strPrty = clsSVT.getDataStringValue("Select Custname + ' - ' + PHNo from MstCust where  InActive = 'N' and CardNo = '" & txtParty.Text & "'", "", blnFound)
                    If blnFound Then
                        txtParty.Text = strPrty
                    Else
                        strPrty = clsSVT.getDataStringValue("Select Custname + ' - ' + PHNo from MstCust where InActive = 'N' and PHNO = '" & txtParty.Text & "'", "", blnFound)
                        If blnFound Then
                            txtParty.Text = strPrty
                        Else
                            MsgBox("Please enter either card number or Phone Number", MsgBoxStyle.Critical, Me.Text)
                        End If
                    End If
                    If txtParty.Text <> "" Then
                        Call showPoints()
                        Call CalTotal()
                        Exit Sub
                    End If
                End If
                If vsfCust.Rows.Count >= 2 Then
                    txtParty.Text = vsfCust.GetData(1, vCustName) & " - " & vsfCust.GetData(1, vCustPhNo)
                    If blnLoyalty And txtParty.Text <> "" Then
                        Call showPoints()
                        Call CalTotal()
                    End If
                    txtScan.Focus()
                    vsfCust.Visible = False
                End If

            Case Keys.F4
                txtDummy.Text = ""
                txtDummy2.Text = ""

                strSQL = "Select  CustID ,CustName as Name, PHNO"
                If blnLoyalty Then strSQL = strSQL & ", CardNo "
                strSQL = strSQL & " from MstCust"
                strSQL = strSQL & " Where InActive = 'N' "
                strSQL = strSQL & " order by 2"

                Dim objfrm As New frmUtlSelection
                With objfrm
                    .blnMultiColSearch = True
                    .FillViewVsf(Me, False, Nothing, strSQL, txtDummy, 2, txtDummy2, 1)
                    .Text = " Please Select Customer"
                    .Tag = " frmTrnInv "
                    .ShowDialog()
                    .Close()
                End With

                If Trim(txtDummy.Text) <> "" Then
                    txtParty.Text = txtDummy2.Text & " - " & txtDummy.Text
                    If blnLoyalty And txtParty.Text <> "" Then
                        Call showPoints()
                        Call CalTotal()
                    End If
                    txtScan.Focus()
                    vsfCust.Visible = False
                End If

            Case Keys.Down
                If vsfCust.Rows.Count > 1 Then
                    vsfCust.Row = 1
                    vsfCust.Col = 1
                    vsfCust.Focus()
                End If
                If Trim(txtParty.Text) <> "" Then

                Else

                End If
            Case Keys.Escape
                txtParty.Focus()
                vsfCust.Visible = False
            Case Keys.Back
                If Trim(txtParty.Text) = "" Then
                    'vsfCust.BringToFront()
                End If
                vsfCust.BringToFront()
        End Select
    End Sub
    Private Sub vsfCust_DoubleClick(sender As Object, e As EventArgs) Handles vsfCust.DoubleClick
        Dim intCustID As Int32, strSendSMS As String
        If vsfCust.Row > 0 Then
            txtParty.Text = vsfCust.GetData(vsfCust.Row, vCustName) & " - " & vsfCust.GetData(vsfCust.Row, vCustPhNo)

            intCustID = clsSVT.getDataLongValue("Select CustID from MstCust where InActive = 'N' and Custname + ' - ' + PHNo = '" & txtParty.Text & "'", 0, False)

            strSendSMS = clsSVT.getDataStringValue("select sendSms from mstCust where InActive = 'N' and CustID=" & intCustID, "N", False)
            If strSendSMS = "Y" Then
                chkSMS.Checked = True
            Else
                chkSMS.Checked = False
            End If


            If txtParty.Text <> "" Then
                showPoints()
            End If

            txtScan.Focus()
            vsfCust.Visible = False
        End If
    End Sub



    Public Function getInvNo() As String
        Dim strSQL As String, strPfx As String = "", strFldValue As String
        'Invoice()
        'utlgsettings()
        'InvNoReset -- D M blank
        'counterWsRst(--Y / N)
        'DocNoFmt -- pattern with double angle brackets -- <<2>>-<<6>>/<<7>>
        getInvNo = ""


        strSQL = "Select max(invCNo) + 1 from TrnHdrSInv"
        strSQL = strSQL & " where FinYear = " & gSettings.gCompFinYear
        Select Case strInvNoReset
            Case "D"
                strSQL = strSQL & " and month(SInvdate) = " & Month(dtDocDate.Value)
                strSQL = strSQL & " and day(SInvdate) = " & DateAndTime.Day(dtDocDate.Value)
                strPfx = getMnthDayPfx(Month(dtDocDate.Value), strPrefixSettings)
                strPfx = strPfx & DateAndTime.Day(dtDocDate.Value)
            Case "M"
                strSQL = strSQL & " and month(SInvdate) = " & Month(dtDocDate.Value)
                strPfx = getMnthDayPfx(Month(dtDocDate.Value), strPrefixSettings)
        End Select
        If strCounterWsRst = "Y" Then
            strSQL = strSQL & " and SCID = " & intSalesCounterID
        End If
        lngInvCNO = clsSVT.getDataLongValue(strSQL, 1, False)
        strDocNoFmt = getGsettings("Invoice", "DocNoFmt", "<<5>>/<<6>>")
        strCounterPfx = clsSVT.getDataStringValue("Select PREFIX from MstSC where SysName = '" & gStrSystemName & "'", "", False)
        strFldValue = strDocNoFmt
        If strFldValue <> "" Then
            strFldValue = Replace(strFldValue, "<<1>>", strPfx)
            strFldValue = Replace(strFldValue, "<<2>>", strCounterPfx)
            strFldValue = Replace(strFldValue, "<<3>>", Strings.Right(gSettings.gCompStDt, 4))
            strFldValue = Replace(strFldValue, "<<4>>", Strings.Right(gSettings.gCompEndDt, 4))
            strFldValue = Replace(strFldValue, "<<5>>", Strings.Right(gSettings.gCompStDt, 2))
            strFldValue = Replace(strFldValue, "<<6>>", Strings.Right(gSettings.gCompEndDt, 2))
            strFldValue = Replace(strFldValue, "<<7>>", Format(lngInvCNO, "0000000"))
            strFldValue = Replace(strFldValue, "<<8>>", lngInvCNO)
        End If
        strFldValue = Replace(strFldValue, "//", "/")
        strFldValue = Replace(strFldValue, "--", "-")
        strFldValue = Replace(strFldValue, ",", "")
        getInvNo = strFldValue
        GetCSEINV = getInvNo
    End Function

    Private Function getMnthDayPfx(argMonth As Integer, argSet As String) As String
        getMnthDayPfx = ""
        Dim arr() As String = Split(argSet, ",")
        If arr.Length <= 0 Then Exit Function
        Try
            getMnthDayPfx = arr(argMonth - 1)
        Catch ex As Exception

        End Try
    End Function

    Private Sub btnChkVdty_DropDownItemClicked(sender As Object, e As DropDownItemClickedEventArgs) Handles btnQuickSel.DropDownItemClicked
        Select Case e.ClickedItem.Text
            Case "Add/Edit Quick Selection"
                Dim objFrm As New frmMstItemQuickSel
                '  objFrm.fillDtls()
                objFrm.strTblName = "MstQuickSelItems"
                objFrm.lblSelName.Text = "Quick Selection Name"
                objFrm.ShowDialog()
                Call fillCboDtls(btnQuickSel, "MstQuickSelItems")
            Case Else
                Dim rs As OleDb.OleDbDataReader
                Dim strSQL As String
                strSQL = "Select i.itemid, i.ShortName, IsNull(CLBal,0) QOH "
                strSQL = strSQL & " from MstItem i "
                strSQL = strSQL & " Join MstQuickSelItems q on i.ItemID=q.ItemID "
                strSQL = strSQL & " Left Join TrnQOH s on i.ItemID=s.ItemID"
                strSQL = strSQL & "  where q.QSetName = '" & e.ClickedItem.Text & "'"
                strSQL = strSQL & "  and i.itemid = q.itemid"
                strSQL = strSQL & " order by 2"
                rs = clsSVT.getDataReaderFromSQL(strSQL)
                vsfMenu.Rows.Count = 0
                vsfMenu.Cols.Count = 3
                With rs
                    Do While .Read()
                        vsfMenu.Rows.Count = vsfMenu.Rows.Count + 1
                        vsfMenu.SetData(vsfMenu.Rows.Count - 1, vQDetItemid, !ItemID)
                        vsfMenu.SetData(vsfMenu.Rows.Count - 1, vQQDetItem, !ShortName)
                        vsfMenu.SetData(vsfMenu.Rows.Count - 1, vQQOH, !QOH)
                    Loop
                    .Close()
                End With
                vsfMenu.AutoSizeCols()
                Call INIStore("QuickSelection", "QSetName", e.ClickedItem.Text)
        End Select
    End Sub

    Private Sub btnLoadTemp_DropDownItemClicked(sender As Object, e As DropDownItemClickedEventArgs) Handles btnLoadTemp.DropDownItemClicked
        Select Case e.ClickedItem.Text
            Case "Add/Edit Templates"
                Call AddEditSel()
                Call fillCboDtls(btnLoadTemp, "MstItemTemp")
            Case Else
                Dim rs As OleDb.OleDbDataReader
                Dim strSQL As String
                strSQL = "Select ItemID "
                strSQL = strSQL & " from MstItemTemp "
                strSQL = strSQL & "  where QSetName = '" & e.ClickedItem.Text & "'"
                strSQL = strSQL & " order by 1"
                rs = clsSVT.getDataReaderFromSQL(strSQL)

                If rs.HasRows Then
                    Do While rs.Read()
                        AddItemToGrid(rs.Item("ItemID"))
                    Loop
                End If

                Call showPoints()
                Call CalTotal()

                rs.Close()
        End Select
    End Sub
    Private Sub AddEditSel()
        Dim objFrm As New frmMstItemQuickSel
        objFrm.Text = "Templates"
        objFrm.strTblName = "MstItemTemp"
        objFrm.lblSelName.Text = "Template Name"
        objFrm.ShowDialog()
        objFrm.Dispose()
    End Sub

    Private Sub txtSalesPerson_KeyUp(sender As Object, e As KeyEventArgs) Handles txtSalesPerson.KeyUp
        Select Case e.KeyCode
            Case Keys.Down
                txtDummy.Text = ""
                txtDummy2.Text = ""

                strSQL = "Select  SPID, SPCode as Code , SPName as Name "
                strSQL = strSQL & " from MstSP "
                strSQL = strSQL & " Where InActive = 'N' "
                strSQL = strSQL & " order by 2 "

                Dim objfrm As New frmUtlSelection
                With objfrm
                    .blnMultiColSearch = True
                    .FillViewVsf(Me, False, Nothing, strSQL, txtDummy, 0, txtDummy2, 1)
                    .Text = " Please Select Sales Person "
                    .Tag = " frmTrnInv "
                    .ShowDialog()
                    .Close()
                End With

                If Trim(txtDummy.Text) <> "" Then
                    txtSalesPerson.Text = txtDummy2.Text
                    txtScan.Focus()
                End If

            Case Keys.Enter
                txtScan.Focus()
        End Select
    End Sub

    Private Sub btnAddItem_Click(sender As Object, e As EventArgs) Handles btnAddItem.Click
        Dim keya As New Windows.Forms.KeyEventArgs(Keys.Enter)
        Call txtQty_KeyUp(txtQty, keya)
    End Sub

    Private Sub vsfMenu_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles vsfMenu.MouseDoubleClick
        Dim ht = vsfMenu.HitTest
        If ht.Row <> -1 Then
            If ht.Row <= -1 Then
                Exit Sub
            End If
        End If
        'If vsfMenu.Rows.Count > 1 Then
        Dim intRow As Int32 = vsfDet.FindRow(vsfMenu.GetData(vsfMenu.Row, 1), 1, vDetItemName, False, True, False)
            If intRow > 0 Then
                If vsfDet.GetData(intRow, vDetItemCode) <> "" Then
                    vsfDet.SetData(intRow, intColQty, Val(vsfDet.GetData(intRow, intColQty) + 1))
                    vsfDet.Row = intRow
                    Call CalTotal()
                End If
            Else
                AddItemToGrid(Val(vsfMenu.GetData(vsfMenu.Row, 0)))
                Call showPoints()
                Call CalTotal()

            End If
        'End If
    End Sub

    Private Sub txtSalesPerson_Validating(sender As Object, e As CancelEventArgs) Handles txtSalesPerson.Validating
        If Trim(txtSalesPerson.Text) = "" Then Exit Sub
        txtSalesPerson.Text = UCase(txtSalesPerson.Text)
        intSPID = clsSVT.getDataLongValue("Select SPID from MstSP Where SPCode='" & Trim(txtSalesPerson.Text) & "'", 0, False)
        If intSPID = 0 Then
            MsgBox("Invalid Sales Person Code...", MsgBoxStyle.Exclamation, Me.Text)
            txtSalesPerson.Text = ""
            txtSalesPerson.Focus()
            Exit Sub
        End If
        txtScan.Focus()
    End Sub

    Private Sub txtRcvdAmt_TextChanged(sender As Object, e As EventArgs) Handles txtRcvdAmt.TextChanged
        txtPayback.BackColor = Color.Black
        txtPayback.ForeColor = Color.Lime
        If Trim(txtRcvdAmt.Text) = "" Then
            txtPayback.Text = ""
            Exit Sub
        End If

        If Val(txtRcvdAmt.Text) > Val(vsfTot.GetData(intRowCashAmt, vOthFldDataValue)) Then
            txtPayback.Text = "Payback       " & Format(Val(txtRcvdAmt.Text) - Val(vsfTot.GetData(intRowCashAmt, vOthFldDataValue)), "0.00")
        ElseIf Val(txtRcvdAmt.Text) = Val(vsfTot.GetData(intRowCashAmt, vOthFldDataValue)) Then
            txtPayback.Text = ""
        Else
            txtPayback.Text = "Shortage by    " & Format(Math.Abs(Val(txtRcvdAmt.Text) - Val(vsfTot.GetData(intRowCashAmt, vOthFldDataValue))), "0.00")
            txtPayback.ForeColor = Color.Red
        End If
    End Sub

    Public Sub saveBill(ByVal argtype As String)
        If Not chkUserRights(strRights, chkRightsPos.AddRights, Me.Text) Then Exit Sub

        'If gcheckCounter added by Surabhi
        If gCheckCounter Then
            If Not gCounterWiseFmt Then
                MsgBox("Counter wise reset enabled in global settings!" & vbCrLf & "Please ensure counter number is a part of doc no. format!", vbInformation, Me.Text)
                FrmUtlGSettings.MdiParent = frmMain
                FrmUtlGSettings.Show()
                Exit Sub
            End If
        End If

        Call showWaitCursor()
        If Not validFlds() Then
            Call showNormalCursor()
            Exit Sub
        End If

        If Val(Trim(lblLmtBeer.Text).Replace("Beer : ", "").Replace(" Ltr ", "")) > gDblBeerLmt Or
            Val(Trim(lblLmtWine.Text).Replace("Wine : ", "").Replace(" Ltr ", "")) > gDblWineLmt Or
            Val(Trim(lblLmtIML.Text).Replace("IML : ", "").Replace(" Ltr ", "")) > gDblIMLlmt Then
            If gBlnAllowSplit Then
                'Split bill saving logic
                Call saveSplitBills(argtype)
            Else
                'Normal save (single bill)
                Call SaveIndBill(argtype)
            End If
        Else
            'Normal save (single bill)
            Call SaveIndBill(argtype)
        End If

    End Sub

    Private Sub SaveIndBill(argType As String, Optional blnFromSplitInv As Boolean = False)
        Dim lngDInvID As Long
        lngDInvID = saveDInv(blnFromSplitInv)
        If strQRUrl <> "" Then storeQRFile(lngDInvID)
        If strQRUrlCheck <> "" Then storeQRFileCheck(lngDInvID)

        intLastBillID = 0
        If lngDInvID > 0 Then
            intLastBillID = lngDInvID

            Dim strSystem As String = ""
            strSystem = getGsettings("INVOICE", "BLOCKPRINT")

            For Each sys In strSystem.Split(",")
                If UCase(sys) = UCase(gStrSystemName) Then
                    GoTo exitPrint
                End If
            Next

            If strPrintPrompt = "P" And Not blnFromSplitInv Then
                If MsgBox("Do you want to print the invoice...?", MsgBoxStyle.YesNo, Me.Text) = MsgBoxResult.No Then GoTo exitPrint
            End If

            intNoofCopies = clsSVT.getDataLongValue("Select NOOFCOPIES from UtlOption Where DocID = " & intDocID & " and DefaultPrint='Y'", 1, False)

            Select Case argType
                Case "N"
                    Select Case strInvF12
                        Case "SP"
                            Call PrintInvCrpt(lngDInvID, argPreview:=False, argNoofCopies:=intNoofCopies)
                        Case "SPP"
                            Call PrintInvCrpt(lngDInvID, argNoofCopies:=intNoofCopies)
                    End Select
                Case "C"
                    Select Case strInvCtrlF12
                        Case "SP"
                            Call PrintInvCrpt(lngDInvID, argPreview:=False, argNoofCopies:=intNoofCopies)
                        Case "SPP"
                            Call PrintInvCrpt(lngDInvID, argNoofCopies:=intNoofCopies)
                    End Select
                Case "A"
                    Select Case strInvAltF12
                        Case "SP"
                            Call PrintInvCrpt(lngDInvID, argPreview:=False, argNoofCopies:=intNoofCopies)
                        Case "SPP"
                            Call PrintInvCrpt(lngDInvID, argNoofCopies:=intNoofCopies)
                    End Select
            End Select

exitPrint:
            If UCase(gUserCode) <> "ADMIN" Then
                If getGsettings("INVOICE", "SHOWSALESVALUE", "N") = "Y" Then
                    Dim dblTotalSales As Double = clsSVT.getDataDoubleValue("Select Sum(InvValue) From TrnHdrSINV Where IsDelete = 'N' and SINVDate='" & getDateFormatInInsert(getSystemDate()) & "'", 0, False)
                    Dim intTotalBills As Integer = clsSVT.getDataDoubleValue("Select Count(InvValue) From TrnHdrSINV Where IsDelete = 'N' and SINVDate='" & getDateFormatInInsert(getSystemDate()) & "'", 0, False)
                    frmMain.lblSalesValue.Text = "Total Bills: " & intTotalBills & " & Sales: " & dblTotalSales
                End If
            Else
                Dim dblTotalSales As Double = clsSVT.getDataDoubleValue("Select Sum(InvValue) From TrnHdrSINV Where IsDelete = 'N' and SINVDate='" & getDateFormatInInsert(getSystemDate()) & "'", 0, False)
                Dim intTotalBills As Integer = clsSVT.getDataDoubleValue("Select Count(InvValue) From TrnHdrSINV Where IsDelete = 'N' and SINVDate='" & getDateFormatInInsert(getSystemDate()) & "'", 0, False)
                frmMain.lblSalesValue.Text = "Total Bills: " & intTotalBills & " & Sales: " & dblTotalSales
            End If


            If blnSMSFeature Then
                'If chkSMS.Checked Then
                '    txtDocID.Text = lngDInvID.ToString
                '    'Dim strPName As String = clsSVT.getDataStringValue("Select PartyName From TrnHdrSInv where SinvID = " & Val(txtDocID.Text), "", False)
                '    Dim strMobNo As String = clsSVT.getDataStringValue("Select partyCMob From TrnHdrSInv where SinvID = " & Val(txtDocID.Text), "", False)
                '    Dim strSMS As String = getSMS("")
                '    strMobNo = Trim(strMobNo)
                '    If IsNumeric(strMobNo) AndAlso strMobNo.Length = 10 Then
                '        Call SendSMS(strSMS, strMobNo, "")
                '    End If
                'End If

                'If gBlnSMSInvSales Then
                '    If vsfTot.GetData(intRowTolal, vOthFldDataValue) > gdblSalesValLmt Then
                '        Dim strSMS As String = getSMS("SALES")
                '        For Each strMob As String In gStrSalesMobNo.Split(",")
                '            strMob = Trim(strMob)
                '            If IsNumeric(strMob) AndAlso strMob.Length = 10 Then
                '                Call SendSMS(strSMS, strMob, "")
                '            End If
                '        Next
                '    End If
                'End If
            End If
            Call ClearItems()
            Call clearFlds()
            Mode = "A"
        End If
        GC.Collect()
        Call showNormalCursor()
    End Sub

    Public Function saveDInv(blnFromSplitInv As Boolean) As Long
        Dim strSQL As String, intMaxDocID As Long
        Dim strItemStatus As String, blnFound As Boolean
        Dim strEarlierVal As String = "", strNewVal As String = ""
        Dim strRDMNo As String = "", intAuditDocID As Integer, strCompvalue As String = ""
        If blnLoyalty Then

            Call FillAutoGenTrnNo("TRNHDRRDM", "RDMNO", "", 1284)
        End If


        strRDMNo = txtDummy.Text
        saveDInv = 0
        If Mode = "M" Then
            saveDInv = Val(txtDocID.Text)
            strSQL = "Update TrnHdrSInv set "
            strSQL = strSQL & " SPID = " & intSPID & ","
            strSQL = strSQL & " ChangedBy = '" & gUserCode & "',"
            strSQL = strSQL & " ChangedOn = '" & getDateFormatInInsert(getSystemDateTime(), True) & "'"
            Call GetUpdateString(Me, strFlds)
            strSQL = strSQL & strFlds
            strSQL = strSQL & " where SinvID = " & Val(txtDocID.Text)
        Else
            strSQL = "Insert into TrnHdrSInv (FINYEAR, splitInvRef, SINVNO, InvCNo, SCID, BILLSYSTEM, SPID, SINVDate, SINVDATETIME,"
            strSQL = strSQL & "  CUSTID, PartyName, PartyCMob, "
            strSQL = strSQL & "  AddedBy, AddedOn, RCVDAMT, PAYBACKAMT "
            Call GetInsertString(Me, strValues, strFlds)
            strSQL = strSQL & strFlds & ") output inserted.SInvID into @testTable values ("
            strSQL = strSQL & gSettings.gCompFinYear & ","
            If blnFromSplitInv Then
                strSQL = strSQL & "'" & strSplitInvRef & "',"
            Else
                strSQL = strSQL & "'',"
            End If
            strSQL = strSQL & "'" & getInvNo() & "',"
            strSQL = strSQL & lngInvCNO & ","
            strSQL = strSQL & intSalesCounterID & ","
            strSQL = strSQL & "'" & gStrSystemName & "',"
            strSQL = strSQL & intSPID & ","
            'strSQL = strSQL & "'" & getDateFormatInInsert(getSystemDateTime, False) & "', "
            strSQL = strSQL & "'" & getDateFormatInInsert(dtDocDate.Value, False) & "', "
            'strSQL = strSQL & "'" & getDateFormatInInsert(getSystemDateTime, True, True) & "', "
            strSQL = strSQL & "'" & getDateFormatInInsert(dtDocDate.Value, True, True) & "', "
            strSQL = strSQL & clsSVT.getDataLongValue("Select CustID from MstCust Where Custcode + ' - ' + PHNO ='" & Trim(txtParty.Text) & "'", 0, False) & ","
            strSQL = strSQL & "'" & clsSVT.getDataStringValue("Select Custcode from MstCust Where Custcode + ' - ' + PHNO ='" & Trim(txtParty.Text) & "'", "", False) & "',"
            strSQL = strSQL & "'" & clsSVT.getDataStringValue("Select PHNO from MstCust Where Custcode + ' - ' + PHNO ='" & Trim(txtParty.Text) & "'", "", False) & "',"
            strSQL = strSQL & "'" & gUserCode & "', "
            strSQL = strSQL & "'" & getDateFormatInInsert(getSystemDateTime, True, True) & "', "
            strSQL = strSQL & Val(txtRcvdAmt.Text) & ","
            strSQL = strSQL & Val(Trim(txtPayback.Text).Replace("Payback", "").Replace("Shortage", "")) & ""
            strSQL = strSQL & strValues
        End If
        If Mode = "M" And gBlnCheckForFraud Then
            strEarlierVal = "Total Items: "
            strEarlierVal &= clsSVT.getDataLongValue("Select count(itemid) from TrndtlSinv where SinvID = " & Val(txtDocID.Text), 0, False) & ", "
            strEarlierVal &= "Invoice Value: "
            strEarlierVal &= clsSVT.getDataLongValue("Select InvValue from TrnHdrSinv where SinvID = " & Val(txtDocID.Text), 0, False)
        End If

        svtTrn = svtConn.BeginTransaction(IsolationLevel.ReadCommitted)
        Try
            If Mode <> "M" Then
                intMaxDocID = getMaxInsertID(strSQL, "TrnHdrSInv", svtTrn)
                If intMaxDocID = -1 Then
                    saveDInv = 0
                    svtTrn.Rollback()
                    Exit Function
                End If
                saveDInv = intMaxDocID
            Else
                intMaxDocID = Val(txtDocID.Text)
                Call clsSVT.execSQLStatement("Delete from TrndtlSInv where SinvID = " & Val(txtDocID.Text), svtTrn)
                Call clsSVT.execSQLStatement("Delete from TrndtlSInvDenom where SinvID = " & Val(txtDocID.Text), svtTrn)
                Call clsSVT.execSQLStatement("Delete from UtlStockList where DocType = 'INV' and DocID = " & Val(txtDocID.Text), svtTrn)
                Call clsSVT.execSQLStatement(strSQL, svtTrn)
            End If
            'Insert detail

            With vsfDet
                For i = 1 To .Rows.Count - 1
                    strFlds = ""
                    strValues = ""
                    strFlds = "Insert into TrnDtlSInv (SINVID,SlNo,ITEMID,UOMID,ItemSlNO,STKQTY,SINVQtyInLtrs,"
                    strValues = intMaxDocID & ","
                    strValues = strValues & i & ","
                    strValues = strValues & Val(.GetData(i, vDetItemID)) & ","
                    strValues = strValues & clsSVT.getDataLongValue("Select UOMID from MstUOM Where UOMCode='" & Trim(vsfDet.GetData(i, vDetUOM)) & "'", 0, False, svtTrn) & ","
                    strValues = strValues & Val(.GetData(i, vDetItemSlNo)) & ","
                    strValues = strValues & Val(.GetData(i, intColQty)) & ","
                    strValues = strValues & Val(.GetData(i, intColQty)) * Val(.GetData(i, vDetLtrPerBot)) & ","
                    Call GetvsfOptDetInsertString(Me, vsfDet, vsfOptions, intDetCols, i, strFlds, strValues, svtTrn)
                    strSQL = strFlds & strValues
                    If Not clsSVT.execSQLStatement(strSQL, svtTrn) Then
                        MsgBox("Error during save -- " & strSQL, MsgBoxStyle.Critical, Me.Text)
                        saveDInv = 0
                        svtTrn.Rollback()
                        Exit Function
                    End If
                Next
            End With

            strSQL = " Insert Into UtlStockList "
            strSQL &= " (ItemID, StkMultiplier, DocType, DocDate, DocID, DocNo, Particulars, Qty, LtrQty, Rate) "
            strSQL &= " Select ItemID, -1, 'INV' , " & strSHForm & "Date, h." & strSHForm & "ID, " & strSHForm & "NO, PartyName, " & strSHForm & "Qty, SINVQTYINLTRS, " & strSHForm & "Rate "
            strSQL &= " From TrnDtl" & strSHForm & " d, TrnHdr" & strSHForm & " h "
            strSQL &= " Where d." & strSHForm & "ID=h." & strSHForm & "ID "
            strSQL &= " and d." & strSHForm & "ID=" & intMaxDocID
            Dim strErrMsg As String = clsSVT.execSQLStmtMsg(strSQL, svtTrn)
            If strErrMsg <> "" Then
                saveDInv = 0
                If InStr(UCase(strErrMsg), "CHECK CONSTRAINT") > 0 Then
                    MsgBox("Stock Is going negative. Cannot Save.", MsgBoxStyle.Critical, "No Stock")
                Else
                    MsgBox(strErrMsg & " Error during updating stock", MsgBoxStyle.Critical, "No Stock")
                End If
                svtTrn.Rollback()
                Exit Function
            End If

            If Not savePoints(intMaxDocID, svtTrn) Then
                svtTrn.Rollback()
                Exit Function
            End If

            If Not SaveDenom(intMaxDocID) Then
                svtTrn.Rollback()
                Exit Function
            End If
            txtDocID.Text = intMaxDocID
            Dim argDsgrGrids = GetAllDsgrVsf(Me, GetType(C1FlexGrid))

            For Each argGrid As C1FlexGrid In argDsgrGrids
                If argGrid.Visible Then


                    If Not saveMultiAmtFld(argGrid) Then
                        saveDInv = False
                        svtTrn.Rollback()
                        Exit Function
                    End If
                End If
            Next



            If blnLoyalty Then

                If Val(vsfTot.GetData(intRowTotPointsRdmd, vOthFldDataValue)) > 0 Then

                    Dim intcustid As Int32
                    intcustid = clsSVT.getDataLongValue("Select CustID from MstCust Where Custcode + ' - ' + PHNO ='" & Trim(txtParty.Text) & "'", 0, False, svtTrn)
                    If Not clsSVT.execSQLStatement("delete from TRNHDRRDM where Custid=" & intcustid & "And GREMARKS ='" & Trim(txtPrintNo.Text) & "'", svtTrn) Then
                        svtTrn.Rollback()
                        Exit Function
                    End If


                    strSQL = "insert into TRNHDRRDM(RDMNO,Prefix,PrintRDMNO,RDMDate,Authorised,AuthBy,AuthOn,ChangedBy,ChangedOn,Cancelled,AddedBy,AddedOn,CUSTID,RPOINTS,CASHAMT,GREMARKS,REMARKS)"

                    strSQL &= " values ("
                    strSQL &= "'" & strRDMNo & "', '', '" & strRDMNo & "', "
                    strSQL &= "'" & getDateFormatInInsert(getSystemDate(svtTrn)) & "','Y', "
                    strSQL &= "'" & gUserCode & "', "
                    strSQL &= "'" & getDateFormatInInsert(getSystemDateTime(svtTrn), True) & "', "
                    strSQL &= "'" & gUserCode & "', "
                    strSQL &= "'" & getDateFormatInInsert(getSystemDateTime(svtTrn), True) & "','N', "
                    strSQL &= "'" & gUserCode & "', "
                    strSQL &= "'" & getDateFormatInInsert(getSystemDateTime(svtTrn), True) & "', "
                    strSQL &= "" & intcustid & ", "
                    strSQL &= "" & Val(vsfTot.GetData(intRowTotPointsRdmd, vOthFldDataValue)) & ","
                    strSQL &= "" & Val(vsfTot.GetData(intRowTotPointsRdmd, vOthFldDataValue)) & ",'" & Trim(txtPrintNo.Text) & "','')"

                    If Not clsSVT.execSQLStatement(strSQL, svtTrn) Then
                        svtTrn.Rollback()
                        Exit Function
                    End If
                End If
            End If

            If Mode = "M" And gBlnCheckForFraud Then
                strNewVal = "Total Items: "
                strNewVal &= clsSVT.getDataLongValue("Select count(itemid) from TrndtlSinv where SinvID = " & Val(txtDocID.Text), 0, False, svtTrn) & ", "
                strNewVal &= "Invoice Value: "
                strNewVal &= clsSVT.getDataLongValue("Select InvValue from TrnHdrSinv where SinvID = " & Val(txtDocID.Text), 0, False, svtTrn)

                strSQL = "Insert into UtlAdmAudit (ScreenName,DAction,DocDate,DocDateTime,DocSysName,SysUserName,MainFldID,MainFld, EarlierData,ChangedData"
                strSQL &= ") output inserted.DocID into @testTable values ("
                strSQL &= "'Invoice',"
                strSQL &= "'Edited',"
                strSQL &= "'" & getDateFormatInInsert(getSystemDate(svtTrn)) & "',"
                strSQL &= "'" & getDateFormatInInsert(getSystemDateTime(svtTrn), True) & "',"
                strSQL &= "'" & gStrSystemName & "',"
                strSQL &= "'" & gStrSystemUserName & "',"
                strSQL &= "'" & txtDocID.Text & "',"
                strSQL &= "'" & txtDocNo.Text & "',"
                strSQL &= "'" & strEarlierVal & "',"
                strSQL &= "'" & strNewVal & "')"
                intAuditDocID = getMaxHdrDtlInsertID(strSQL, "", strDocHdrTblName, svtTrn)

                'dtl audit 
                For i = 1 To vsfTemp.Rows.Count - 1
                    blnFound = False
                    For j = 1 To vsfDet.Rows.Count - 1
                        If vsfTemp.GetData(i, vDetItemID) = vsfDet.GetData(j, vDetItemID) Then
                            blnFound = True
                            strItemStatus = "Edited"
                            strCompvalue = ""
                            For x = 21 To vsfDet.Cols.Count - 1
                                If vsfDet.GetData(j, x) <> vsfTemp.GetData(i, x).ToString Then
                                    strCompvalue &= "{ Changed " & GetVal(vsfDet.GetData(0, x)) & " from [" & GetVal(vsfTemp.GetData(i, x)) & "] to [" & GetVal(vsfDet.GetData(j, x)) & "] } "
                                End If
                            Next x
                            Exit For
                        End If
                    Next j
                    If Not blnFound Then
                        strItemStatus = "Deleted"
                        strCompvalue = ""
                        For j = vDetQOH + 1 To vsfDet.Cols.Count - 1
                            strCompvalue &= vsfDet.GetData(0, j) & ": " & GetVal(vsfDet.GetData(i, j)).ToString & ", "
                        Next j
                    End If
                    If strCompvalue <> "" Then
                        strSQL = "Insert into UtlAdmAuditItem (DocID, ItemID, ItemCode, ItemName, ActionStatus, ItemDetails) values ("
                        strSQL &= intAuditDocID & ","
                        strSQL &= vsfTemp.GetData(i, vDetItemID) & ","
                        strSQL &= "'" & vsfTemp.GetData(i, vDetItemCode) & "',"
                        strSQL &= "'" & vsfTemp.GetData(i, vDetItemName) & "',"
                        strSQL &= "'" & strItemStatus & "',"
                        strSQL &= "'" & strCompvalue & "')"
                        Call clsSVT.execSQLStatement(strSQL, svtTrn)
                    End If
                Next i

                'for newly added items
                For i = 1 To vsfDet.Rows.Count - 1
                    blnFound = False
                    For j = 1 To vsfTemp.Rows.Count - 1
                        If vsfTemp.GetData(j, vDetItemID) = vsfDet.GetData(i, vDetItemID) Then
                            blnFound = True
                            Exit For
                        End If
                    Next j
                    strItemStatus = ""
                    If Not blnFound Then
                        strItemStatus = "Added"
                        strCompvalue = ""
                        For j = vDetQOH + 1 To vsfDet.Cols.Count - 1
                            strCompvalue &= vsfDet.GetData(0, j) & ": " & GetVal(vsfDet.GetData(i, j)).ToString & ", "
                        Next j
                        strSQL = "Insert into UtlAdmAuditItem (DocID, ItemID, ItemCode, ItemName, ActionStatus, ItemDetails) values ("
                        strSQL &= intAuditDocID & ","
                        strSQL &= vsfDet.GetData(i, vDetItemID) & ","
                        strSQL &= "'" & vsfDet.GetData(i, vDetItemCode) & "',"
                        strSQL &= "'" & vsfDet.GetData(i, vDetItemName) & "',"
                        strSQL &= "'" & strItemStatus & "',"
                        strSQL &= "'" & strCompvalue & "')"
                        Call clsSVT.execSQLStatement(strSQL, svtTrn)
                    End If
                Next i


            End If

            'for audit track
            If blnAuditTrack Then
                Call RecordAuditLog(strFrmCaption, Val(txtDocID.Text), IIf(Mode = "A", UserActions.AddAction, UserActions.EditAction), strDocHdrTblName, strAuditHdrSQL, strAuditDtlSQL, svtTrn)
            End If

            svtTrn.Commit()

            LoyaltySENDMAIL()
            If Not blnFromSplitInv Then Mode = ""
            'MsgBox("Saved successfully", vbInformation, Me.Text)
        Catch ex As Exception
            saveDInv = 0
            MsgBox("Error during save" & vbCrLf & ex.Message, MsgBoxStyle.Critical, Me.Text)
            svtTrn.Rollback()
            Exit Function
        End Try
    End Function


    Public Sub FillAutoGenTrnNo(ByVal argHdrTbl As String, ByVal argHdrfldName As String, ByVal argDocPfx As String, ByVal IntDocid As Int32)
        Dim intTrnNo As Long
        dtPrintFmt = clsSVT.getDATATable("Select * from UtlDocNoFmt Where DocID=" & IntDocid)

        intTrnNo = Val(TrnNoAutoGen(argHdrTbl, argHdrfldName, argDocPfx, getSystemDate(), IntDocid, "", True))
        txtDummy2.Text = intTrnNo
        txtDummy.Text = getFmtDocNo(argDocPfx, Val(txtDummy2.Text), getSystemDate())


    End Sub

    Private Function getUtlDocNoFmt(ByVal argStr As String, ByRef argPrntNo As String, ByVal argPfx As String) As String
        If argStr <> "" Then
            Select Case argStr
                Case "1"
                    argPrntNo = argPrntNo & argPfx
                Case "2"
                    argPrntNo = argPrntNo & Strings.Right(gSettings.gCompStDt, 4)
                Case "3"
                    argPrntNo = argPrntNo & Strings.Right(gSettings.gCompEndDt, 4)
                Case "4"
                    argPrntNo = argPrntNo & Strings.Right(gSettings.gCompStDt, 2)
                Case "5"
                    argPrntNo = argPrntNo & Strings.Right(gSettings.gCompEndDt, 2)
                Case "6"
                    argPrntNo = argPrntNo & Strings.Right(txtDocNo.Text, 5)
                Case "7"
                    argPrntNo = argPrntNo & Val(txtNo.Text)
                Case "8"
                    argPrntNo = argPrntNo & Strings.Right(txtDocNo.Text, 4)
                Case "9"
                    argPrntNo = argPrntNo & Strings.Right(txtDocNo.Text, 3)
                Case "10"
                    argPrntNo = argPrntNo & GetVal(dtPrintFmt.Rows(0).Item("Misc1"), "")
                Case "11"
                    argPrntNo = argPrntNo & GetVal(dtPrintFmt.Rows(0).Item("Misc2"), "")
                Case "12"
                    argPrntNo = argPrntNo & GetVal(dtPrintFmt.Rows(0).Item("Misc3"), "")
                Case Else
                    If argStr = "/" Or argStr = "-" Then
                        If argPrntNo <> "" Then
                            argPrntNo = argPrntNo & argStr
                        End If
                    Else
                        argPrntNo = argPrntNo & argStr
                    End If
            End Select
        End If

        Return argPrntNo
    End Function

    Private Function SaveDenom(ByVal argID As Int32) As Boolean
        SaveDenom = False
        If Trim(txtNoteValQry.Text) = "" Then GoTo endfn

        Dim strSQL As String = "Insert Into TrnDtlSINVDenom (SINVID, CurName, Nos, Value) Values ("
        For Each strLine As String In Trim(txtNoteValQry.Text).Split("■")
            Dim strCur As String = ""
            Dim intCount As Int32 = 0
            strSQL &= argID & ","
            For Each strVal As String In strLine.Split("|")
                Select Case intCount
                    Case 0
                        strSQL &= "'" & strVal & "',"
                    Case 1
                        strSQL &= "" & GetVal(strVal, 0) & ","
                    Case 2
                        strSQL &= "" & GetVal(strVal, 0) & "),("
                End Select
                intCount = intCount + 1
            Next
        Next
        strSQL = Mid(strSQL, 1, Len(strSQL) - 2)
        If Not clsSVT.execSQLStatement(strSQL, svtTrn) Then
            Exit Function
        End If
endfn:
        SaveDenom = True
    End Function

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        frmMain.CloseSelectedTabPage(Me)
        frmMain.pnlProject.Panels(0).Visible = True
    End Sub

    Private Sub CalcLiqLimit()

        Dim dblBeerTolLtr As Double
        Dim dblWineTolLtr As Double
        Dim dblIMLTolLtr As Double
        Dim intQty As Int32

        lblLmtBeer.Text = ""
        lblLmtBeer.ForeColor = Color.White
        lblLmtWine.Text = ""
        lblLmtWine.ForeColor = Color.White
        lblLmtIML.Text = ""
        lblLmtIML.ForeColor = Color.White

        Dim dblTotLtr As Double = 0

        Try
            With vsfDet
                For i = 1 To .Rows.Count - 1
                    intQty = Val(.GetData(i, intColQty))
                    Select Case UCase(.GetData(i, vDetLiqType))
                        Case "BEER"
                            dblBeerTolLtr += (intQty * Val(.GetData(i, vDetLtrPerBot)))
                        Case "WINE"
                            dblWineTolLtr += (intQty * Val(.GetData(i, vDetLtrPerBot)))
                        Case "IML"
                            dblIMLTolLtr += (intQty * Val(.GetData(i, vDetLtrPerBot)))
                    End Select
                Next

                If dblBeerTolLtr <= 0 Then
                    lblLmtBeer.Visible = False
                Else
                    lblLmtBeer.Visible = True
                    lblLmtBeer.Text = "Beer : " & dblBeerTolLtr & " Ltr "

                    If Val(Trim(lblLmtBeer.Text).Replace("Beer : ", "").Replace(" Ltr ", "")) > gDblBeerLmt Then
                        lblLmtBeer.ForeColor = Color.Red
                    End If
                End If

                If dblWineTolLtr <= 0 Then
                    lblLmtWine.Visible = False
                Else
                    lblLmtWine.Visible = True
                    lblLmtWine.Text = "Wine : " & dblWineTolLtr & " Ltr "

                    If Val(Trim(lblLmtWine.Text).Replace("Wine : ", "").Replace(" Ltr ", "")) > gDblWineLmt Then
                        lblLmtWine.ForeColor = Color.Red
                    End If
                End If

                If dblIMLTolLtr <= 0 Then
                    lblLmtIML.Visible = False
                Else
                    lblLmtIML.Visible = True
                    lblLmtIML.Text = "IML : " & dblIMLTolLtr & " Ltr "

                    If Val(Trim(lblLmtIML.Text).Replace("IML : ", "").Replace(" Ltr ", "")) > gDblIMLlmt Then
                        lblLmtIML.ForeColor = Color.Red
                    End If
                End If
            End With
        Catch ex As Exception
            MsgBox(ex.Message, vbInformation, Me.Text)
        End Try
    End Sub

    Private Sub btnClearItems_Click(sender As Object, e As EventArgs) Handles btnClearItems.Click

        Call ClearItems()

    End Sub

    Private Sub ClearItems()
        vsfDet.Rows.Count = 1
        Mode = ""
        txtScan.BackColor = Color.White
        With vsfTot
            For i = 1 To .Rows.Count - 1
                .SetData(i, vOthFldDataValue, "")
                .SetData(i, vOthFldMultiAmtVal, "")
            Next
        End With
        Call CalTotal()
        If vsfDet.Rows.Count > 1 Then
            EnableDisableOptBtn(True)
        Else
            EnableDisableOptBtn(False)
        End If
        Call fillVsfHFTDefValues(Me)
        fillDenom(Val(txtDocID.Text))
        Mode = "A"
        txtScan.Focus()
    End Sub

    Private Sub btnHold_Click(sender As Object, e As EventArgs) Handles btnHold.Click
        Dim objFrm As Object
        objFrm = New frmTrnInv
        objFrm.initForm(intDocID)
        If Not objFrm.blnSalesCounter Then
            Exit Sub
        End If
        objFrm.txtScan.Focus()
        frmMain.OpenForms(objFrm, "Sales Invoice",, True)

        'Dim objFrm As New frmUtlHoldBill
        'With objFrm
        '    .InitForm(intSalesCounterID, Me, strRestorePath)
        '    .ShowDialog()
        'End With
        'If vsfDet.Rows.Count > 1 Then
        '    EnableDisableOptBtn(True)
        'End If

    End Sub

    Private Sub lblPrevBill_DoubleClick(sender As Object, e As EventArgs) Handles lblPrevBill.DoubleClick
        intLastBillID = clsSVT.getDataLongValue("Select Max(SINVID) From TrnHdrSInv Where SCID = " & intSalesCounterID, 0, False)
        If intLastBillID <> 0 Then
            PrintInvCrpt(intLastBillID)
        End If
    End Sub

    Private Sub fillCboDtls(sender As Object, ByVal strTblName As String)
        sender.Items.Clear()
        Dim c1DD As New C1.Win.C1Input.DropDownItem
        If strTblName = "MstItemTemp" Then
            c1DD.Text = "Add/Edit Templates"
        Else
            c1DD.Text = "Add/Edit Quick Selection"
        End If

        sender.Items.Add(c1DD)

        Dim rs As OleDb.OleDbDataReader
        rs = clsSVT.getDataReaderFromSQL("Select distinct QSetName from " & strTblName & " order by 1")
        If rs.HasRows Then
            Do While rs.Read()
                Dim rsItem As New C1.Win.C1Input.DropDownItem
                rsItem.Text = rs.Item(0)
                sender.Items.Add(rsItem)
            Loop
        End If
        rs.Close()
    End Sub

    Private Sub EnableDisableOptBtn(ByVal argBln As Boolean)
        btnPrint.Visible = argBln
        btnDelete.Visible = Not argBln
        btnCancel.Visible = Not argBln
        btnRePrint.Visible = Not argBln
    End Sub

    Private Sub txtRcvdAmt_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtRcvdAmt.KeyPress
        If Not Char.IsNumber(e.KeyChar) AndAlso Not e.KeyChar = Chr(Keys.Delete) AndAlso Not e.KeyChar = Chr(Keys.Back) Then
            e.Handled = True
        End If
    End Sub

    Private Sub txtQty_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtQty.KeyPress
        If Not Char.IsNumber(e.KeyChar) And Not e.KeyChar = Chr(Keys.Delete) And Not e.KeyChar = Chr(Keys.Back) Then
            e.Handled = True
        End If
        If e.KeyChar = "'" Then
            e.Handled = True
        End If
    End Sub

    Private Sub txtParty_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtParty.KeyPress, txtScan.KeyPress
        If e.KeyChar = "'" Then
            e.Handled = True
        End If
    End Sub

    Private Sub updateSInvRateToMstItem(ByVal argID As Int32, ByVal svtTran As OleDb.OleDbTransaction)
        Dim strSQL As String

        strSQL = "Update i Set "
        strSQL &= " i.ItemSell = d.SINVRATE"
        strSQL &= " From Mstitem i, TrnDtlSINV d"
        strSQL &= " Where i.ItemId = d.ItemId"
        strSQL &= " And d.SINVID = " & argID & ""

        If clsSVT.execSQLStatement(strSQL, svtTran) Then
            Exit Sub
        End If

    End Sub

    Private Sub vsfTot_CellButtonClick(sender As Object, e As RowColEventArgs) Handles vsfTot.CellButtonClick
        With vsfTot
            If .GetData(e.Row, vOthFldMultiAmtFld) = "Y" Then

                strMultiAmtVal = ""
                dblMultiAmtVal = 0
                blnMultiAmtVal = False

                Dim frmObj As New frmUtlMultiAmt
                With frmObj
                    .argObj = Me
                    .lblFrm.Text = "Multi Amount - " & vsfTot.GetData(e.Row, vOthFldCode)
                    .intDocID = intDocID
                    .intTrnID = Val(txtDocID.Text)
                    .strList = vsfTot.GetData(e.Row, vOthFldMultiAmtVal)
                    .StartPosition = FormStartPosition.CenterScreen
                    .ShowDialog()
                End With

                If blnMultiAmtVal Then
                    .SetData(e.Row, vOthFldDataValue, dblMultiAmtVal)
                    .SetData(e.Row, vOthFldMultiAmtVal, strMultiAmtVal)
                End If
            End If
        End With
    End Sub

    Private Sub chkSMS_CheckedChanged(sender As Object, e As EventArgs) Handles chkSMS.CheckedChanged

    End Sub

    Private Sub getMultiAmtFld(ByVal argVSF As C1FlexGrid)
        Dim strSQL As String = ""
        With argVSF
            For i As Int32 = 0 To .Rows.Count - 1
                If GetVal(.GetData(i, vOthFldMultiAmtFld)) = "Y" Then
                    strSQL = "Select * From TrnMultiAmt Where DocID=" & Val(intDocID) & " and TrnID=" & Val(txtDocID.Text) & " and FldCode='" & GetVal(.GetData(i, vOthFldCode)) & "' Order by Slno "

                    Dim rs As OleDb.OleDbDataReader
                    rs = clsSVT.getDataReaderFromSQL(strSQL)

                    If Not rs.HasRows Then
                        rs.Close()
                        GoTo nextrecord
                    End If

                    Dim strRecords As String = ""
                    Do While rs.Read
                        strRecords &= Val(rs.Item("Slno")) & "|"
                        strRecords &= Trim(rs.Item("Particulars")) & "|"
                        strRecords &= Val(rs.Item("Amount")) & "■"
                    Loop

                    strRecords = Mid(strRecords, 1, Len(strRecords) - 1)
                    .SetData(i, vOthFldMultiAmtVal, strRecords)
                    rs.Close()
                End If
nextrecord:
            Next
        End With
    End Sub

    Private Sub btnCurr_Click(sender As Object, e As EventArgs) Handles btnCurr.Click
        strNotesValue = ""
        Dim objFrm As New frmMutlCurr
        With objFrm
            .argfrm = Me
            .strDenom = Trim(txtNoteValQry.Text)
            .ShowDialog()
            .Dispose()
        End With
        If Trim(strNotesValue) <> "" Then
            txtNoteValQry.Text = strNotesValue
        End If

    End Sub

    Private Function saveMultiAmtFld(ByVal argVSF As C1FlexGrid) As Boolean
        saveMultiAmtFld = False
        Dim strSQL As String = ""
        Dim blnFound As Boolean = False
        With argVSF
            For i As Int32 = 0 To .Rows.Count - 1
                If GetVal(.GetData(i, vOthFldMultiAmtFld)) = "Y" Then
                    blnFound = False

                    clsSVT.execSQLStatement("Delete From TrnMultiAmt where trnid=" & Val(txtDocID.Text) & " and Docid=" & intDocID & " and FldCode='" & GetVal(.GetData(i, vOthFldCode)) & "'", svtTrn)

                    strSQL = "Insert Into TrnMultiAmt (DocID, TrnID, FldCode, Slno, Particulars, Amount) values ("
                    For Each strLineItem As String In CStr(.GetData(i, vOthFldMultiAmtVal)).Split("■")
                        If strLineItem <> "" Then
                            blnFound = True
                            strSQL &= Val(intDocID) & ","
                            strSQL &= Val(txtDocID.Text) & ","
                            strSQL &= "'" & GetVal(.GetData(i, vOthFldCode)) & "',"
                            Dim intCol As Int32 = 1
                            For Each strval As String In strLineItem.Split("|")
                                Select Case intCol
                                    Case 1
                                        strSQL &= Val(strval) & ","
                                    Case 2
                                        strSQL &= "'" & strval & "',"
                                    Case 3
                                        strSQL &= Val(strval) & "),("
                                End Select
                                intCol = intCol + 1
                            Next
                        End If
                    Next
                    If blnFound Then
                        strSQL = Mid(strSQL, 1, Len(strSQL) - 2)
                        If Not clsSVT.execSQLStatement(strSQL, svtTrn) Then
                            Exit Function
                        End If
                    End If
                End If
            Next
        End With
        Return True
    End Function

    Private Function getSMS(argType As String) As String
        getSMS = ""
        Dim txttemplate As New TextBox, strTName As String
        Dim blnFound As Boolean
        Select Case argType
            Case ""
                strTName = clsSVT.getDataStringValue("Select Template from UtlSMSTemplate where Def='Y' and DocID=" & intDocID & " order by Template", "", blnFound)
                If strTName <> "" Then
                    strTName = clsSVT.getDataStringValue("Select Top 1 Template from UtlSMSTemplate where DocID=" & intDocID & " order by Template", "", blnFound)
                End If
                If strTName = "" Then Exit Function
                txttemplate.Text = clsSVT.getDataStringValue("Select TemplateText from UtlSMSTemplate where DocID=" & intDocID & " and Template='" & strTName & "'", "", False)
                If txttemplate.Text = "" Then Exit Function
            Case "DELETED"
                txttemplate.Text = gstrDeleteSMSMsg
            Case "CANCELLED"
                txttemplate.Text = gstrCancelledSMSMsg
            Case "SALES"
                txttemplate.Text = gstrSalesSmsMsg
        End Select

        txttemplate.Multiline = True
        If Trim(txttemplate.Text) <> "" Then
            Dim strSQL As String = getFieldsFromTbl(strDocHdrTblName, Val(txtDocID.Text))

            Dim dtTbl As DataTable = clsSVT.getDATATable(strSQL)

            For i As Int32 = 0 To dtTbl.Rows.Count - 1
                For j As Int32 = 0 To dtTbl.Columns.Count - 1
                    txttemplate.Text = txttemplate.Text.Replace(UCase("<<" & dtTbl.Columns(j).Caption & ">>"), GetVal(dtTbl.Rows(i).Item(j).ToString(), ""))
                Next
            Next
            getSMS = txttemplate.Text
        End If
    End Function

    Private Function getFieldsFromTbl(ByVal argTblName As String, ByVal argID As Int32) As String
        getFieldsFromTbl = ""
        Dim rsFlds As OleDb.OleDbDataReader
        Dim blnInclude As Boolean, i As Integer
        Dim blnFound As Boolean
        Dim strFldList As String = "", strFindFld As String
        Dim strCondList As String = "", strTblList As String
        Dim strSubTblCaption As String
        Dim strSQL As String
        Dim strFldName As String

        argTblName = UCase$(argTblName)

        strSQL = "Select * from  " & argTblName & " where 1 <> 1"
        rsFlds = clsSVT.getDataReaderFromSQL(strSQL)

        strTblList = argTblName & ","

        With rsFlds
            For i = 0 To .FieldCount - 1
                strFldName = UCase(Trim(.GetName(i)))
                blnInclude = True
                Select Case strFldName
                            'Case "AMMENDMENT", "IMGPRINT", "AMMDETAILS", "AMMNO", "AMMDATE", "AUTHORISED", "AUTHDATE", "AUTHORISEDBY"
                    Case "RGUID", "IMGPRINT"
                        blnInclude = False
                End Select

                If blnInclude Then
                    blnFound = False
                    'list the table name if it is from another table
                    strFindFld = strFldName
                    If InStr(1, strFindFld, "ID") > 0 Then
                        strFindFld = Replace(strFindFld, "ID", "")
                        If "MST" & strFindFld <> argTblName And "TRNHDR" & strFindFld <> argTblName And "TRNDTL" & strFindFld <> argTblName Then
                            '                        'if it is not from the same table MstItem or TrnHdrInv or TrnDtlInv
                            '                        'and if the master exists then 
                            strSubTblCaption = clsSVT.getDataStringValue("select docName from UtlTransactions where HdrTblName = 'MST" & strFindFld & "'", "", blnFound)
                            If blnFound Then
                                If fnTablePresent("MST" & strFindFld) Then
                                    blnFound = False
                                    strTblList &= "MST" & strFindFld & ","
                                    strCondList &= " and IsNull(" & argTblName & "." & strFldName & ",0) = " & "MST" & strFindFld & "." & strFindFld & "ID"

                                End If
                            End If
                        End If
                    End If
                End If
            Next i
            rsFlds.Close()
        End With

        strTblList = Mid(strTblList, 1, Len(strTblList) - 1)

        strSQL = " SELECT * " '
        strSQL = strSQL & " FROM " & strTblList
        strSQL = strSQL & " WHERE 1=1 " & strCondList
        strSQL = strSQL & " and " & strDocHdrTblName & "." & strDocIDFldName & " = " & Val(argID)
        getFieldsFromTbl = strSQL
    End Function

    Private Sub chkSMS_MouseUp(sender As Object, e As MouseEventArgs) Handles chkSMS.MouseUp
        If e.Button = MouseButtons.Right Then
            With New frmTrnSMS
                .intDocId = intDocID
                .initScreen(Val(txtDocID.Text))
                If Trim(txtParty.Text) <> "" Then
                    Dim strMobile As String = ""
                    For Each strMobile In Trim(txtParty.Text).Split("-")
                    Next
                    strMobile = Trim(strMobile)
                    strMobile = strMobile.Replace(" ", "")
                    If IsNumeric(strMobile) AndAlso strMobile.Length = 10 Then
                        .txtTo.Text = strMobile
                    End If
                End If
                .ShowDialog()
            End With
        End If
    End Sub

    Private Sub vsfDet_BeforeResizeColumn(sender As Object, e As RowColEventArgs) Handles vsfDet.BeforeResizeColumn

    End Sub

    Private Sub fillDenom(ByVal argID As Int32)
        Dim rs As OleDb.OleDbDataReader
        Dim str As String = ""
        Dim dbltotal As Double = 0
        rs = clsSVT.getDataReaderFromSQL("Select * from TrnDtlSINVDenom Where SINVID=" & argID)
        If rs.HasRows Then
            Do While rs.Read
                dbltotal = dbltotal + Val(rs.Item("Value"))
                str &= rs.Item("CurName") & "|"
                str &= rs.Item("Nos") & "|"
                str &= rs.Item("Value") & "■"
            Loop
        End If
        rs.Close()
        If str <> "" Then
            str = Mid(str, 1, Len(str) - 1)
            txtNoteValQry.Text = str
            txtRcvdAmt.Text = dbltotal
        Else
            txtNoteValQry.Text = ""
            txtRcvdAmt.Text = ""
        End If
    End Sub


    Private Sub btnShowPts_Click(sender As Object, e As EventArgs) Handles btnShowPts.Click
        Dim intCustID As Int32
        'If txtPoints.Text = "" Then Exit Sub
        If Trim(txtParty.Text) = "" Then Exit Sub
        Dim objfrm As New frmRptLoyalty
        objfrm.optSummary.Checked = True
        objfrm.blnInvoiceVw = True
        intCustID = clsSVT.getDataLongValue("Select CustID from MstCust where InActive = 'N' and Custname + ' - ' + PHNo = '" & txtParty.Text & "'", 0, False)
        objfrm.strCustList = intCustID
        frmMain.OpenForms(objfrm, "Loyalty Report")
        objfrm.PrintRptSmry()


    End Sub

    Private Sub txtFinalValue_TextChanged(sender As Object, e As EventArgs) Handles txtFinalValue.TextChanged
        'showPoints()
    End Sub

    Private Sub frmTrnInv_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        txtScan.Focus()
    End Sub
    Public Sub SetFocus()
        txtScan.Focus()
    End Sub

    Private Sub showPoints()
        vsfCust.Visible = False
        txtPoints.BackColor = Color.Yellow
        Dim sngPoints As Integer
        Dim sngCountRDMDPts As Int32
        Dim sngCountRDMDAmt As Int32

        Dim DblRedeemValue As Double
        txtPoints.Text = ""
        If Not blnLoyalty Then Exit Sub
        If Trim(txtParty.Text) = "" Then Exit Sub
        txtPoints.Text = clsSVT.getDataLongValue("Select sum(InvPoints - RPoints) from vwredeem where Custcode + ' - ' + PHNo = '" & txtParty.Text & "'", 0, False).ToString
        'for current invoice
        Dim i As Integer, sngAmt As Double = 0
        With vsfDet
            For i = 1 To .Rows.Count - 1
                If clsSVT.getDataStringValue("Select Loyalty from mstItemgroup g, mstitem i where g.itemgroupid = i.itemgroupid and i.itemid = " & Val(.GetData(i, vDetItemID)), "", False) = "Y" Then
                    sngAmt = sngAmt + Val(.GetData(i, intColItemAmt))
                End If
            Next
            sngPoints = 0
            If sngAmt >= gdblBillValueGreaterThan Then
                sngPoints = Math.Round(sngAmt * gDblPointsForOneRs, 0)
            End If
            txtPoints.Text = Val(txtPoints.Text) + sngPoints
            If Val(txtPoints.Text) > gIntAlertIfPointsGreaterThan Then
                txtPoints.BackColor = Color.LightGreen
                If vsfDet.Rows.Count = 1 Then Exit Sub
                sngAmt = 0
                For i = 1 To .Rows.Count - 1
                    If clsSVT.getDataStringValue("Select Loyalty from mstItemgroup g, mstitem i where g.itemgroupid = i.itemgroupid and i.itemid = " & Val(.GetData(i, vDetItemID)), "", False) = "Y" Then
                        sngAmt = sngAmt + Val(.GetData(i, intColItemAmt))
                    End If
                Next
                If sngAmt <= 0 Then
                    DblRedeemValue = 0
                    vsfTot.SetData(intRowTotPointsRdmd, vOthFldDataValue, DblRedeemValue)
                    Call CalTotal()
                    Exit Sub
                End If
                Dim a As Single = Val(txtPoints.Text) / gIntAlertIfPointsGreaterThan
                sngCountRDMDPts = Math.Floor(a)
                DblRedeemValue = (gIntAlertIfPointsGreaterThan * sngCountRDMDPts) * gDblRsForOnePoint

                If DblRedeemValue > sngAmt Then
                    sngCountRDMDAmt = 0
                    Dim b As Single = sngAmt / gIntAlertIfPointsGreaterThan
                    sngCountRDMDAmt = Math.Floor(b)
                    DblRedeemValue = (gIntAlertIfPointsGreaterThan * sngCountRDMDAmt) * gDblRsForOnePoint
                    vsfTot.SetData(intRowTotPointsRdmd, vOthFldDataValue, DblRedeemValue)

                    Exit Sub
                End If
            Else
                DblRedeemValue = 0
            End If

            vsfTot.SetData(intRowTotPointsRdmd, vOthFldDataValue, DblRedeemValue)
            Call CalTotal()

            'If Val(vsfTot.GetData(intRowTotPointsRdmd, vOthFldDataValue)) > 0 Then
            '    If Val(txtFinalValue.Text) < Val(vsfTot.GetData(intRowTotPointsRdmd, vOthFldDataValue)) Then
            '        vsfTot.SetData(intRowTotPointsRdmd, vOthFldDataValue, (DblRedeemValue - (gIntAlertIfPointsGreaterThan * gDblRsForOnePoint)))
            '    End If
            'End If

            ''If Val(vsfTot.GetData(intRowTotPointsRdmd, vOthFldDataValue)) > 0 Then
            ''    vsfTot.SetData(intRowInvVal, vOthFldDataValue, (Val(vsfTot.GetData(intRowCashAmt, vOthFldDataValue)) - Val(vsfTot.GetData(intRowTotPointsRdmd, vOthFldDataValue))))
            ''End If

        End With
    End Sub
    Public Function savePoints(argInvID As Long, argTrn As OleDb.OleDbTransaction) As Boolean
        Dim sngPoints As Integer, strSQl As String, intCustID As Integer
        savePoints = True
        'txtPoints.Text = ""
        If Not blnLoyalty Then Exit Function
        If Trim(txtParty.Text) = "" Then Exit Function
        'for current invoice
        Dim i As Integer, sngAmt As Double = 0
        With vsfDet
            Try
                For i = 1 To .Rows.Count - 1
                    If clsSVT.getDataStringValue("Select Loyalty from mstItemgroup g, mstitem i where g.itemgroupid = i.itemgroupid and i.itemid = " & Val(.GetData(i, vDetItemID)), "", False, argTrn) = "Y" Then
                        sngAmt = sngAmt + Val(.GetData(i, intColItemAmt))
                    End If
                Next
                sngPoints = 0
                If sngAmt >= gdblBillValueGreaterThan Then
                    sngPoints = Math.Round(sngAmt * gDblPointsForOneRs, 0)
                End If
                If sngPoints = 0 Then Exit Function
                intCustID = clsSVT.getDataLongValue("Select CustID from MstCust where Custname + ' - ' + PHNo = '" & txtParty.Text & "'", 0, False, argTrn)
                strSQl = "Insert into TrnPoints (DocDate, CustID, SInvID, SInvNo, SinvDate, InvAmt, POints, Remarks) values ("
                strSQl = strSQl & "'" & getDateFormatInInsert(dtDocDate.Value) & "', "
                strSQl &= intCustID & ","
                strSQl &= argInvID & ","
                strSQl = strSQl & "'" & txtPrintNo.Text & "', "
                strSQl = strSQl & "'" & getDateFormatInInsert(dtDocDate.Value) & "', "
                strSQl &= sngAmt & ","
                strSQl &= sngPoints & ","
                strSQl &= "'')"
                Call clsSVT.execSQLStatement(strSQl, argTrn)
            Catch ex As Exception
                savePoints = False
            End Try
        End With
    End Function
    Private Sub LoyaltySENDMAIL()
        Dim sngPoints As Integer, intCustID As Integer, balanceAmt As Int32
        If Not blnLoyalty Then Exit Sub
        If Trim(txtParty.Text) = "" Then Exit Sub
        'for current invoice
        Dim i As Integer, sngAmt As Double = 0
        Dim strSendSMS As String = "", blnEmail As Boolean = True, BlnSMS As Boolean = True
        With vsfDet
            Try
                For i = 1 To .Rows.Count - 1
                    If clsSVT.getDataStringValue("Select Loyalty from mstItemgroup g, mstitem i where g.itemgroupid = i.itemgroupid and i.itemid = " & Val(.GetData(i, vDetItemID)), "", False) = "Y" Then
                        sngAmt = sngAmt + Val(.GetData(i, intColItemAmt))
                    End If
                Next
                sngPoints = 0
                If sngAmt >= gdblBillValueGreaterThan Then
                    sngPoints = Math.Round(sngAmt * gDblPointsForOneRs, 0)
                End If
                If sngPoints = 0 Then Exit Sub

                intCustID = clsSVT.getDataLongValue("Select CustID from MstCust where Custname + ' - ' + PHNo = '" & txtParty.Text & "'", 0, False)
                'balanceAmt = Val(txtPoints.Text) - clsSVT.getDataLongValue("Select sum(InvPoints - RPoints) from vwredeem where custid =" & intCustID, 0, False)
                strSendSMS = clsSVT.getDataStringValue("select sendSms from mstCust where CustID=" & intCustID, "N", False)
                If Not strSendSMS = "Y" Then Exit Sub


                balanceAmt = Val(txtPoints.Text) - getGsettings("Loyalty", "AlertIfPointsGreaterThan")

                Dim strToEmail As String = "", strCCEmail As String = "", strContent As String = "", strSub As String = ""

                strToEmail = clsSVT.getDataStringValue("Select EMAILID from MstCust where CUSTID=" & intCustID, "", False)

                strSub = gSettings.gCompName & " Reward Points"


                strContent = "Thank you For shopping at " & gSettings.gCompName
                strContent &= vbCr & " Total Rewards points achieved As Of Today (" & getDateFormatInCond(getSystemDate) & ") Is " & Val(txtPoints.Text)
                strContent &= vbCr & "  Current Bill Value : " & sngAmt & " Reward Point :" & sngPoints
                strContent &= vbCr & "  Require" & balanceAmt & "  to redeem."
                strContent &= vbCr & vbCr & "Regards "
                strContent &= vbCr & vbCr & gSettings.gCompName

                If strToEmail = "" Or strToEmail = " " Or strContent = "" Then
                    blnEmail = False
                    GoTo SMS
                End If
                Try
                    If Not sendGenMail(strToEmail, strCCEmail, strSub, strContent) Then
                        blnEmail = False
                        'MsgBox("Email Not Sent to " & txtParty.Text & "", vbCritical, Me.Text)
                        'Exit Sub
                    End If
                Catch ex As Exception
                    blnEmail = False
                End Try

SMS:
                If blnSMSFeature Then
                    If chkSMS.Checked Then
                        Dim gStrSalesMobNo As String = ""
                        gStrSalesMobNo = clsSVT.getDataStringValue("Select partyCMob From TrnHdrSInv where SinvID = " & Val(txtDocID.Text), "", False)
                        If gStrSalesMobNo = "" Then
                            blnEmail = False
                        End If
                        Dim strSMS As String = ""
                        strSMS = "You have accrued " & sngPoints & " points for shopping at SVS Drops. Your total bill amount is INR Rs." & sngAmt & " and we thank you for your patronage."

                        If Not SendSMS(strSMS, gStrSalesMobNo, "") Then
                            blnEmail = False
                        End If

                        If Val(vsfTot.GetData(intRowTotPointsRdmd, vOthFldDataValue)) > 0 Then
                            Dim intbalPoints As Int32

                            intbalPoints = Val(txtPoints.Text) - vsfTot.GetData(intRowTotPointsRdmd, vOthFldDataValue)
                            strSMS = ""
                            strSMS = "Congratulations! You have redeemed " & vsfTot.GetData(intRowTotPointsRdmd, vOthFldDataValue) & " points, your balance is " & intbalPoints & ". Thank you for your patronage. Regards, SVS Drops."
                            If Not SendSMS(strSMS, gStrSalesMobNo, "") Then
                                blnEmail = False
                            End If
                        End If

                    End If
                End If
                If BlnSMS And blnEmail Then
                    MsgBox("Email and SMS Sent Successfully", vbInformation, Me.Text)
                End If
                If Not BlnSMS AndAlso blnEmail Then
                    MsgBox("Email Sent Successfully", vbInformation, Me.Text)
                End If

                If Not blnEmail AndAlso BlnSMS Then
                    MsgBox("SMS Sent Successfully", vbInformation, Me.Text)
                End If

            Catch ex As Exception
                MsgBox("Unable to send mail and SMS!", MsgBoxStyle.Critical, Me.Text)
            End Try
        End With
    End Sub
    Private Sub applyRdmPoints()
        If Not blnLoyalty Then Exit Sub
        If Trim(txtParty.Text) = "" Then Exit Sub
        Dim sngPoints As Integer
        sngPoints = clsSVT.getDataLongValue("Select sum(InvPoints - RPoints) from vwredeem where Custcode + ' - ' + PHNo = '" & txtParty.Text & "'", 0, False).ToString

        'for current invoice
        Dim i As Integer, sngAmt As Double = 0
        With vsfDet
            For i = 1 To .Rows.Count - 1
                If clsSVT.getDataStringValue("Select Loyalty from mstItemgroup g, mstitem i where g.itemgroupid = i.itemgroupid and i.itemid = " & Val(.GetData(i, vDetItemID)), "", False) = "Y" Then
                    sngAmt = sngAmt + Val(.GetData(i, intColItemAmt))
                End If
            Next
            If sngAmt >= gdblBillValueGreaterThan Then
                sngPoints = sngPoints + Math.Round(sngAmt * gDblPointsForOneRs, 0)
            End If

            If sngPoints > gIntAlertIfPointsGreaterThan Then

            End If
        End With
    End Sub

    Private Sub storeQRFile(argInvID As Long)
        If gstrQRImagePath = "" Then Exit Sub
        If Trim(strQRUrl) = "" Then Exit Sub
        Dim StrQRImgPath As String
        Dim objQRCode As QRCodeEncoder = New QRCodeEncoder
        Dim img As Image
        Dim btm As Bitmap
        objQRCode.QRCodeScale = 2
        img = objQRCode.Encode(strQRUrl.Replace("<<AMT>>", vsfTot.GetData(intRowInvVal, vOthFldDataValue)))
        btm = New Bitmap(img)
        btm.Save(gstrQRImagePath & argInvID & ".jpg")
        StrQRImgPath = gstrQRImagePath & argInvID & ".jpg"
        clsSVT.execSQLStatement("Update TrnhdrSinv Set QRImage='" & StrQRImgPath & "' where SINVID=" & argInvID)
    End Sub

    Private Sub storeQRFileCheck(argInvID As Long)
        If gstrQRImagePath = "" Then Exit Sub
        If Trim(strQRUrlCheck) = "" Then Exit Sub
        Dim StrQRImgPath As String
        Dim objQRCode As QRCodeEncoder = New QRCodeEncoder
        Dim img As Image
        Dim btm As Bitmap
        objQRCode.QRCodeScale = 2
        img = objQRCode.Encode(strQRUrlCheck.Replace("<<INVID>>", Val(txtDocID.Text)))
        btm = New Bitmap(img)
        btm.Save(gstrQRImagePath & argInvID & "chk.jpg")
        StrQRImgPath = gstrQRImagePath & argInvID & "chk.jpg"
        clsSVT.execSQLStatement("Update TrnhdrSinv Set QRImageCheck='" & StrQRImgPath & "' where SINVID=" & argInvID)
    End Sub

    Private Sub saveSplitBills(argType As String)
        Dim i As Integer, blnIML As Boolean = False, blnWine As Boolean = False, blnBeer As Boolean = False
        Dim sngTotIML As Double, sngTotWine As Double, sngTotBeer As Double

        'copy the existing grid to temp for only those line item categories exceeding limit
        vsfSplit.Rows.Count = 1
        vsfSplit.Cols.Count = vsfDet.Cols.Count
        With vsfDet
            For i = 1 To .Rows.Count - 1
                Call addToSplitGrid(i)
            Next
        End With
        strSplitInvRef = Now.Ticks

saveAnotherInv:
        vsfDet.Rows.Count = 1
        Call CalTotal()
        sngTotBeer = 0
        sngTotIML = 0
        sngTotWine = 0
        With vsfSplit
            For i = 1 To .Rows.Count - 1
                If Val(.GetData(i, intColQty)) > 0 Then
                    Select Case .GetData(i, vDetLiqType).ToString.ToUpper.Trim
                        Case "IML"
                            If sngTotIML + Math.Round((Val(.GetData(i, intColQty)) * Val(.GetData(i, vDetLtrPerBot))), 3) <= gDblIMLlmt Then
                                sngTotIML = sngTotIML + Math.Round((Val(.GetData(i, intColQty)) * Val(.GetData(i, vDetLtrPerBot))), 3)
                                addToDetGridFromSplit(i)
                            End If
                        Case "BEER"
                            If sngTotBeer + Math.Round((Val(.GetData(i, intColQty)) * Val(.GetData(i, vDetLtrPerBot))), 3) <= gDblBeerLmt Then
                                sngTotBeer = sngTotBeer + Math.Round((Val(.GetData(i, intColQty)) * Val(.GetData(i, vDetLtrPerBot))), 3)
                                addToDetGridFromSplit(i)
                            End If
                        Case "WINE"
                            If sngTotWine + Math.Round((Val(.GetData(i, intColQty)) * Val(.GetData(i, vDetLtrPerBot))), 3) <= gDblWineLmt Then
                                sngTotWine = sngTotWine + Math.Round((Val(.GetData(i, intColQty)) * Val(.GetData(i, vDetLtrPerBot))), 3)
                                addToDetGridFromSplit(i)
                            End If
                        Case Else
                            addToDetGridFromSplit(i)
                    End Select
                End If
            Next
            If vsfDet.Rows.Count > 1 Then
                'save current invoice
                Call CalTotal()
                Call SaveIndBill(argType, True)
                GoTo saveAnotherInv
            End If

            'Call printSplitTotalAmt
            'AKSHAY to Call consolidated print here
            'create a view to have all hdr bill detials for the SplitInvRef
            'create a rpt for the same
            'like --- CONSOLIDATED INVOICE

            'Date

            'Inv No     Invoice Value
            '(multiple matching splitinvref)

            'Grand Total


            .Rows.Count = 1
        End With

    End Sub
    Private Sub addToSplitGrid(argRow As Integer)
        Dim i As Integer
        With vsfSplit
            For j = 1 To Val(vsfDet.GetData(argRow, intColQty))
                .Rows.Count = .Rows.Count + 1
                For i = 1 To vsfDet.Cols.Count - 1
                    .SetData(.Rows.Count - 1, i, vsfDet.GetData(argRow, i))
                Next
                .SetData(.Rows.Count - 1, intColQty, 1)
            Next j
        End With
    End Sub
    Private Sub addToDetGridFromSplit(argRow As Integer)
        Dim i As Integer, blnFound As Boolean, sngQty As Integer
        With vsfDet
            blnFound = False
            sngQty = 0
            For i = 1 To .Rows.Count - 1
                If Val(.GetData(i, vDetItemID)) = Val(vsfSplit.GetData(argRow, vDetItemID)) Then
                    blnFound = True
                    intRow = i
                    sngQty = Val(.GetData(i, intColQty))
                    Exit For
                End If
            Next i
            If Not blnFound Then
                .Rows.Count = .Rows.Count + 1
                intRow = .Rows.Count - 1
            End If
            For i = 1 To vsfSplit.Cols.Count - 1
                .SetData(intRow, i, vsfSplit.GetData(argRow, i))
            Next
            .SetData(intRow, intColQty, sngQty + 1)
            vsfSplit.SetData(argRow, intColQty, 0)
        End With
    End Sub

    Private Sub chkHdrFlds()
        If clsSVT.getDataLongValue("Select DOCID from UtlDocDsgrFlds where DocID=" & intDocID & " and FldSection='Header'", 0, False) > 0 Then
            MsgBox("There are few fields in Header Section!" & vbNewLine & "Please move these to Total Section.", MsgBoxStyle.Exclamation, Me.Text)
        End If
    End Sub
End Class