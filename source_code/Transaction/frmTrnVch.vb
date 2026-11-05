Imports C1.Win.C1FlexGrid
Imports System.IO

Public Class frmTrnVch
    'This form  is applicable for Both Party and Non Party Document
    Dim clsSVT As New svtLibrary
    Dim dtSearch As DataTable
    Dim dvSearch As DataView
    Dim intMaxDocID As Long
    Dim svtTrn As oledb.oledbTransaction
    Dim ctrlList As Object 'IEnumerable(Of C1FlexGrid)

    'for audit track
    Dim blnAuditTrack As Boolean = False
    Dim strAuditHdrSQL As String
    Dim strAuditDtlSQL As String

    Public Mode As String 'Control Transaction Add, Edit and Delete
    Public lngViewDocID As Long

    Dim intAcLedID As Long
    Dim lngVchTypeID As Integer

    'used for storing Multiline text
    Public StrgetText As String

    'UtlTransaction Table Related Information
    '==============================================
    Dim strFrmCaption As String
    Dim intDocID As Int32
    Dim intMenuID As Int32

    'Basic Details
    Dim strSHForm As String
    Dim strDocHdrTblName As String
    Dim strDocDtlTblName As String
    Dim strDocDtlBillTblName As String
    Dim strDocIDFldName As String
    Dim strDocNoFldName As String
    Dim strPfxTYpe As String
    Dim strDocDateFldName As String
    Dim strDocAmtFldName As String
    Dim strQtyFldName As String
    Dim intQtyDecimals As Integer

    'Additional Fields from UtlTransaction Table
    '===========================================

    'Authorization related fields
    Dim strAuthReq As String
    Dim strAuthQuery As String
    Dim intNoOfAuth As Integer

    'Linked Documnet Detaisl
    Dim blnLinkedToDoc As Boolean
    Dim strLinkedDocShForm As String = ""

    'Additional fields for Linked Document
    Dim strLinkedTOHdrTblName As String = ""
    Dim strLinkedTODtlTblName As String = ""
    Dim strLinkedToDocNoFldName As String = ""
    Dim strLinkedTODocQtyFldName As String = ""
    Dim strLinkedTODocFldIDName As String = ""

    'Stock related fields
    Dim strAddReduceStock As String = ""

    'Pending Doc related fields
    Dim blnLinkedDocFrom As Boolean
    Dim strPendDocShForm As String
    Dim strPendHdrTblName As String
    Dim strPendDtlTblName As String
    Dim strPendQtyFldName As String
    Dim strPendNoFldName As String
    Dim strPendIDFldName As String
    Dim strPendRateFldName As String


    Dim strPendDocNo As String
    Dim strPendParty As String
    Dim strPendSingleDoc As String
    Dim strPendingQryFlds As String = ""
    Dim strPendingTbls As String = ""
    Dim strPendingQryCond As String = ""

    'Editing Doc related fields
    Dim blnFreezeDoc As Boolean
    Dim blnEditDocDate As Boolean
    Dim intNumOfDays As Integer


    Dim strPrintAfterSave As String
    Dim strTenderFld As String = ""
    Dim blnFreezeQtyCol As Boolean
    Dim blnFreezeRateCol As Boolean
    Dim intFreezeCol As Int32
    Dim strEnvelop As String
    Dim blnBalColReq As Boolean
    Dim blnPartyRelatedFldsPresent As Boolean
    Dim blnDetSection As Boolean = True
    '================== End of UtlTransaction fields


    'Additional variables used for various purpose
    Dim blnColFreeze As Boolean
    Dim intLastCol As Integer
    Dim blnallowToChangePfx As Boolean
    Dim strPfxWiseNo As String
    Dim strDocStdDtlWidth As String
    Dim intRowTenderFld As Int32

    'Detail section fields 
    Dim intItemDocID As Int32
    Dim blnCalc As Boolean
    Dim intColQty As Int32 = -1
    Dim intColrate As Int32 = -1
    Dim intColStkQty As Int32 = -1

    Dim strInsertDocNo As String
    Dim strInsertParty As String

    'Party Related Information
    '========================
    Dim strPartyTblName As String
    Dim strFldPartyID As String
    Dim strFldPartyCode As String
    Dim strFldPartyName As String
    Dim strPartyAddressTblName As String = "MstPartyAddress"
    Dim strPartyContactTbl As String = "MstPartyContacts"
    Dim strPartyFilterCond As String
    Dim strPartyFlds As String

    'Additional Fields
    Dim blnManualItemSel As Boolean
    Dim blnManualPartySel As Boolean
    Dim blnSavePartyData As Boolean
    Dim blnShipAddress As Boolean
    Dim strPartyType As String
    Dim blnOnlycode As Boolean, blnOnlyBill As Boolean, blnBillShip As Boolean

    'Item Related Information
    '========================
    Dim strItemTblName As String
    Dim strFldItemID As String
    Dim strFldItemSlno As String
    Dim strFldItemCode As String
    Dim strFldItemName As String
    Dim strItemFilterCond As String
    Dim strItemFlds As String
    Dim strItemShForm As String

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

    Dim strAmtType As String
    Dim strFirstAmtType As String
    Dim strBillWiseFeatureReq As String
    Dim strBalanceColReq As String


    'declarations for detail grid
    Const vDetAmtType = 0
    Const vDetAcLedName = 1
    Const vDetDebit = 2
    Const vDetCredit = 3
    Const vDetPendBtn = 4
    Const vDetAcLedID = 5
    Const vDetAgRefAmt = 6
    Const vDetNewRefAmt = 7
    Const vDetBalAmt = 8
    Dim intDetCols As Int32 = 9

    Const vSumryVchAmt = 0
    Const vSumryDocAmt = 1
    Const vSumryCumAmt = 2
    Const vSumryBalAmt = 3
    Const vSumryAdjAmt = 4

    Dim intDetNextEntryCol As Int32 = -1

    'for Reference we have maintain below variables 
    Dim intPartyID As Integer
    Dim intItemId As Long
    Dim blnShortClose As Boolean

    Dim dtHdrPrevTran As New DataTable
    Dim dtFtrPrevTran As New DataTable
    Dim dtDetPrevTran As New DataTable

    Dim intHdrPrevTran() As Int32
    Dim intFtrPrevTran() As Int32
    Dim intTotPrevTran() As Int32
    Dim intDetPrevTran() As Int32



    Dim blnTimer As Boolean = False
    Public blnRestore As Boolean = False
    Public strRestoreFileName As String = ""
    Dim strRestorePath As String
    Dim blnItemScan As Boolean = False

    Dim blnVsfMax As Boolean = False
    Dim blnVHdr As Boolean = False
    Dim blnVFtr As Boolean = False
    Dim blnVTot As Boolean = False

    Dim StrFldname As String

    Dim dtCumQty As DataTable

    Dim strItemSearchQry As String, blnPartyItem As Boolean, strItemPartySearchQry As String, strSearchQry As String
    Dim BlnEnforcePartyItemLink As Boolean

    '-----Total grid fields-------'
    Dim intRowVchType As Int32, strVchType = "Cash|Card|Bank"
    Dim intRowCRAmt As Int32
    Dim intRowDRAmt As Int32

    Const vAdjAccLedID = 0
    Const vAdjAccID = 1
    Const vAdjAdjAmt = 2
    Const vAdjDrCr = 3
    Const vAdjType = 4
    Const vAdjNewRefNo = 5

    Public Sub svtBtnCtrl_btnClicked(ByVal argBtnClicked As Integer) Handles svtBtnCtrl.btnClicked
        chkShowBills.Visible = True
        svtBtnCtrl.Focus()
        Dim blnPrint As Boolean
        Select Case argBtnClicked

            Case svtBtns.svtNew

                If Not chkUserRights(strRights, chkRightsPos.AddRights, Me.Text) Then Exit Sub
                vsfDet.RowSel = -1
                Call AddProc()
                svtBtnCtrl.PerformAction(argBtnClicked)
                svtFileAttach.ViewMode = False
                lblTitle.Text = "New - " & Me.Text
                chkShowBills.Visible = False
                Mode = "A"
                cboPfx.SelectedIndex = -1
                cboPfx.Enabled = True
                dtDocDate.Enabled = True
                If cboPfx.Visible And blnDetSection = False Then
                    cboPfx.Focus()
                Else
                    dtDocDate.Focus()
                End If

                TimerStartEvent()
                Me.Parent.Tag = "A"
            Case svtBtns.svtClose
                Call frmMain.CloseSelectedTabPage(Me)

            Case svtBtns.svtEdit

                'chk edit user rights
                If Not chkUserRights(strRights, chkRightsPos.EditRights, Me.Text) Then Exit Sub

                'chk docid is present
                If Val(txtDocID.Text) <= 0 Then Exit Sub

                'chk basic parameters from utltransactions table.
                If ChkEditRights() = False Then Exit Sub

                chkShowBills.Visible = False
                'Start editing
                enableDisableFlds(True)
                vsfVwDet.Visible = False
                vsfDet.Visible = True
                svtBtnCtrl.PerformAction(argBtnClicked)

                lblTitle.Text = "Edit - " & Me.Text
                TimerStartEvent()

                srcTransaction.Visible = False
                Mode = "M"
                cboPfx.Enabled = True
                Me.Parent.Tag = "M"
            Case svtBtns.svtSave

                If Val(txtNo.Text) <= 0 Then Exit Sub
                If validFlds() Then
                    If SaveDoc() Then
                        svtBtnCtrl.PerformAction(argBtnClicked)
                        Call INIStore("Prefix", Me.Text, cboPfx.Text)
                        blnPrint = False
                        Select Case strPrintAfterSave
                            Case "Y"
                                blnPrint = True
                            Case "YP"
                                If MsgBox("Do you want to print this document ?", MsgBoxStyle.YesNo + MsgBoxStyle.Question, Me.Text) = MsgBoxResult.Yes Then
                                    blnPrint = True
                                End If
                        End Select
                        'Call clearFlds()
                        vsfVwDet.Visible = True
                        vsfDet.Visible = False
                        enableDisableFlds(False)

                        svtFileAttach.ViewMode = True
                        ' lblTitle.ForeColor = Color.White
                        lblTitle.Text = "View - " & Me.Text
                        TimerStopEvent()
                        srcTransaction.setFocus()
                        srcTransaction.Visible = True
                        Mode = "V"
                        Call showBtns()
                        If Val(txtDocID.Text) > 0 Then
                            FillDocDetails(Val(txtDocID.Text))
                        End If
                    End If
                End If
                Me.Parent.Tag = ""
            Case svtBtns.svtPrint

                If Not chkUserRights(strRights, chkRightsPos.PrintRights, Me.Text) Then Exit Sub
                If Val(txtDocID.Text) <= 0 Then Exit Sub
                Call prntCrpt()


            Case svtBtns.svtCancel

                Mode = "V"
                Me.Parent.Tag = ""
                Call clearFlds()
                enableDisableFlds(False)
                Call DateFreezing(Me, Mode)
                vsfView.Visible = False
                svtFileAttach.ViewMode = True
                fraAttach.Visible = False
                lblTitle.Text = "View - " & Me.Text
                vsfVwDet.Visible = True
                vsfDet.Visible = False
                svtBtnCtrl.Focus()
                svtBtnCtrl.PerformAction(argBtnClicked)
                TimerStopEvent()
                srcTransaction.Visible = True
                srcTransaction.setFocus()
                Call showBtns()

            Case svtBtns.svtMoveFirst, svtBtns.svtMoveLast, svtBtns.svtMoveNext, svtBtns.svtMovePrev

                If Not chkUserRights(strRights, chkRightsPos.ViewRights, Me.Text) Then Exit Sub
                Call FillDocDetails(gMoveRecords(argBtnClicked, strDocHdrTblName, strDocIDFldName, True, txtDocID.Text, Trim(cboPfx.Text)))
                Call showBtns()

            Case svtBtns.svtDelete

                If txtDocID.Text = "" Then Exit Sub
                If Val(txtDocID.Text) <= 0 Then Exit Sub
                If lblCancelled.Visible = True Then
                    MsgBox("Current record is cancelled. Deletion not possible!", vbInformation, Me.Text)
                    Exit Sub
                End If
                If Not chkUserRights(strRights, chkRightsPos.DelRights, Me.Text) Then Exit Sub
                Call deleteDoc("DELETE")

            Case svtBtns.svtFind

                If Not chkUserRights(strRights, chkRightsPos.ViewRights, Me.Text) Then Exit Sub
                Call showViewTrnForm(Me, strDocHdrTblName, "Listing of " & strFrmCaption, "", "", "", Me.Text, "", intDocID)
                Call showBtns()
        End Select
    End Sub

    Private Sub AddProc()
        Call clearFlds()
        'Mode = "A"
        enableDisableFlds(True)
        vsfVwDet.Visible = False
        Call DateFreezing(Me, Mode)
        Call fillVsfHFTDefValues(Me)
        Call FillAutoGenTrnNo()
        vsfDet.Rows.Count = 1
        vsfDet.Rows.Count = 2
        vsfDet.Visible = True
        srcTransaction.Visible = False
        StMsg()
        For intTb As Int32 = 0 To tbAdnlFlds.TabPages.Count - 1
            Call InsertPrevTrnData(IIf(intTb = 0, "vsfHdr", "VSFHDR" & intTb + 1), dtHdrPrevTran, 0, intTb)
        Next
        For intTb As Int32 = 0 To tbAdnlFldsFtr.TabPages.Count - 1
            Call InsertPrevTrnData(IIf(intTb = 0, "vsfFtr", "VSFFTR" & intTb + 1), dtFtrPrevTran, 0, intTb)
        Next
        Call InsertPrevTrnData(vsfTot, intTotPrevTran, 0)
        Call vsfGenHideMndtAllGrids(Me)
        vsfDet.SetData(1, vDetAmtType, strFirstAmtType)
        vsfHdr.SetData(intRowVchType, vOthFldDataValue, "") '--------changed--------'
    End Sub

    Private Sub clearFlds()
        Mode = "V"
        enableDisableFlds(True)
        lblTotCrValue.Text = ""
        lblTotDrValue.Text = ""
        txtDocID.Text = ""
        txtDocNo.Text = ""
        txtNo.Text = ""
        svtFileAttach.ClearFiles()
        dtDocDate.Value = getSystemDate()
        Call ClearAddnlGrid(Me)
        vsfDet.Rows.Count = 1
        vsfVwDet.Rows.Count = 1
        If Not blnLinkedDocFrom Then
            vsfDet.Rows.Count = 2
        End If
        Call clearAddress()
        fraAttach.Visible = False
        lblCancelled.Visible = False
        lblAuthorised.Visible = False
        svtBtnCtrl.Focus()
        srcTransaction.ClearSearch("")
        srcTransaction.Visible = True
    End Sub

    Public Sub enableDisableFlds(ByVal argFlag As Boolean)
        Dim blnFound As Boolean = False
        If blnEditDocDate Then
            dtDocDate.Enabled = argFlag
        Else
            dtDocDate.Enabled = False
        End If
        srcTransaction.enabledisable("", True)
        ' Lock(txtNo, Not argFlag)
        'ucBtnTools.btnVisible("DETAIL", argFlag, blnLinkedDocFrom)
        With ucBtnTools
            .tsbInsert.Visible = True
            .tsbExport.Visible = True
            .tsbDocExport.Visible = True
            .tsbFreeze.Visible = False
        End With
    End Sub

    Private Sub frmTrnVch_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        If cboPfx.Text <> "" Then
            Call INIStore("Prefix", Me.Text, cboPfx.Text)
        End If

        e.Cancel = False
        If Mode = "A" Or Mode = "M" Then
            If MsgBox("Are you sure you want to exit the form without saving?", MsgBoxStyle.Question _
                      + MsgBoxStyle.YesNo + vbDefaultButton2, "Confirmation...") = MsgBoxResult.No Then
                e.Cancel = True
                Exit Sub
            End If
        End If
        Mode = "V"
        'Me.Close()
    End Sub

    Private Sub frmTrnVch_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyUp
        Dim blnFnKey As Boolean = False
        If e.KeyCode = Keys.Escape Then
            'fraAddress.Visible = False
        End If
        If e.KeyCode = Keys.F9 And e.Control Then
            Call initFormSize(Me, strDocHdrTblName, True, 1130, 631, 0, 0)
            Call initFormVsfDet(Me, strDocHdrTblName, vsfDet, "DET")
            Call initFormVsfDet(Me, strDocHdrTblName, vsfVwDet, "vWDET")
            Call initFormVsfDet(Me, strDocHdrTblName, vsfView, "VIEW")
            Call initFormVsfDet(Me, Me.Text, vsfFtr, "FTR")
            Call initFormVsfDet(Me, Me.Text, vsfTot, "TOT")
            MsgBox("Restored Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
        If e.KeyCode = Keys.F8 And e.Control = True Then
            Call SaveActFormSettings(Me, strDocHdrTblName)
            Call SaveActFormVsfDet(Me, strDocHdrTblName, "DET", vsfDet)
            Call SaveActFormVsfDet(Me, strDocHdrTblName, "VWDET", vsfVwDet)
            Call SaveActFormVsfDet(Me, strDocHdrTblName, "VIEW", vsfView)
            Call SaveActFormVsfDet(Me, Me.Text, "FTR", vsfFtr)
            Call SaveActFormVsfDet(Me, Me.Text, "TOT", vsfTot)
            Call SaveActFormVsfDet(Me, Me.Text, "HDR", vsfHdr)
            MsgBox("Saved Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
        If (e.Control) Or blnFnKey Then
            Call svtBtnCtrl.btnCtrlKeyUp(sender, e)
        End If
        Call raiseShortCutEvent(e)

    End Sub

    Public Sub initForm(ByVal argDocID As Integer)
        ucBtnTools.Visible = True
        intDocID = argDocID
        Call getUtlTrnData(intDocID)
        Call initScreen()
        intMenuID = clsSVT.getDataLongValue("Select MenuItemID from MstMenu where DocID = " & intDocID, 0, False)
        strRights = getUserRights(intMenuID)
        KeyPreview = True
        Mode = "V"
        ctrlList = GetAllDsgrVsf(Me, GetType(C1.Win.C1FlexGrid.C1FlexGrid))
        srcTransaction.SetName("")
        srcTransaction.vsfAnchor()
        srcTransaction.vsfAnchorReSize(410, 30)
    End Sub

    Private Sub initScreen()
        Dim j As Integer
        vsfView.Visible = False
        fraAttach.Visible = False
        cboPfx.Text = ""
        cboPfx.SelectedIndex = -1
        Dim Ilistopts As New List(Of String)
        vsfHdr.Rows.Count = 0
        vsfFtr.Rows.Count = 0
        vsfTot.Rows.Count = 0

        strPfxTYpe = ""

        With dtHdrPrevTran.Columns
            .Add("GridNo", GetType(Integer))
            .Add("ColNo", GetType(Integer))
        End With

        With dtFtrPrevTran.Columns
            .Add("GridNo", GetType(Integer))
            .Add("ColNo", GetType(Integer))
        End With

        With dtDetPrevTran.Columns
            .Add("RowColNo", GetType(Integer))
        End With

        Call InitAddnlFldGrids(Me, vsfHdr, "Header", intDocID, tbAdnlFlds)
        For intTb As Int32 = 0 To tbAdnlFlds.TabPages.Count - 1
            Call getIntArrayPrevTran(IIf(intTb = 0, "vsfHdr", "VSFHDR" & intTb + 1), dtHdrPrevTran, intTb)
        Next

        Call InitAddnlFldGrids(Me, vsfFtr, "Footer", intDocID, tbAdnlFldsFtr)
        For intTb As Int32 = 0 To tbAdnlFldsFtr.TabPages.Count - 1
            Call getIntArrayPrevTran(IIf(intTb = 0, "vsfFtr", "VSFFTR" & intTb + 1), dtFtrPrevTran, intTb)
        Next

        Call InitAddnlFldGrids(Me, vsfTot, "Total", intDocID)
        Call getIntArrayPrevTran(vsfTot, dtDetPrevTran)

        With tbAdnlFlds
            If .TabPages.Count = 1 Then
                .Appearance = TabAppearance.FlatButtons
                .ItemSize = New Size(0, 1)
                .SizeMode = TabSizeMode.Fixed
            End If
        End With

        With tbAdnlFldsFtr
            If .TabPages.Count = 1 Then
                .Appearance = TabAppearance.FlatButtons
                .ItemSize = New Size(0, 1)
                .SizeMode = TabSizeMode.Fixed
            End If
        End With

        lblCancelled.Visible = False
        lblAuthorised.Visible = False
        enableDisableFlds(False)

        'fill prefix
        Call bindCombo(cboPfx, "Prefix", "Prefix", "Select Prefix from UtlDocNoPrefix where DocID = " & intDocID & " order by 1")

        'cboPfx.Text = ""

        If cboPfx.Items.Count <= 0 Then
            Call insertBlankPfx(intDocID)
            cboPfx.Enabled = False
        End If

        strSQL = "Select distinct Prefix from UtlDocNoPrefix where DocID = " & intDocID
        strSQL = clsSVT.getDataStringValue(strSQL, "", False)
        If Trim(strSQL) = "" Then
            cboPfx.Enabled = False
        End If

        svtBtnCtrl.ClearOption()
        Ilistopts = FillOptionBTNDet(Me, intDocID)
        If Not IsNothing(Ilistopts) Then
            For Each ls As String In Ilistopts
                svtBtnCtrl.addOption(ls)
            Next
        End If

        If blnShortClose Then
            svtBtnCtrl.addOption("ShortClose")
        End If

        svtBtnCtrl.addOption("Label Printing")

        With vsfAdj
            .Rows.Count = 1
            .Cols.Count = 6

            .SetData(0, vAdjAccID, "Acc ID")
            .SetData(0, vAdjAccLedID, "Ledger ID")
            .SetData(0, vAdjAdjAmt, "Adj Amt")
            .SetData(0, vAdjType, "Adj Type")
            .SetData(0, vAdjDrCr, "DrCR")
            .SetData(0, vAdjNewRefNo, "NewRefNo")
        End With


        With vsfDet
            If Not IsNothing(dtCumQty) Then
                intDetCols = intDetCols + dtCumQty.Rows.Count
            End If
            .Cols.Count = intDetCols
            .Rows.Count = 2
            .Cols.Fixed = 0
            .SetData(0, vDetAmtType, "Dr/Cr")
            .Cols(vDetAmtType).ComboList = "DR|CR"

            .Cols(vDetAgRefAmt).AllowEditing = False
            .Cols(vDetNewRefAmt).AllowEditing = False
            .Cols(vDetBalAmt).AllowEditing = False
            .SetData(0, vDetAcLedName, "Ledger")
            .SetData(0, vDetDebit, "Debit Amount")
            .SetData(0, vDetCredit, "Credit Amount")
            .SetData(0, vDetPendBtn, "Sel")
            .SetData(0, vDetBalAmt, "Balance")
            .SetData(0, vDetAgRefAmt, "Agnst.Ref Amt")
            .SetData(0, vDetNewRefAmt, "New.Ref Amt")
            .Cols(vDetPendBtn).ComboList = "..."
            strDocStdDtlWidth = "525, 105, 15, 2505, 1665, 2500, 2000, 1155, 1125, 915, 960"

            Call SetColAlign(vsfDet, svtAlign.Left, vDetAcLedName, vDetAmtType)
            Call SetColAlign(vsfDet, svtAlign.Right, vDetCredit, vDetDebit)
            Call SetHideCol(vsfDet, True, vDetAcLedID, vDetAgRefAmt)
            If strBalanceColReq = "N" Then vsfDet.Cols(vDetBalAmt).Visible = False

            intLastCol = vDetAgRefAmt
            If strBillWiseFeatureReq = "N" Then
                Call SetHideCol(vsfDet, True, vDetAgRefAmt, vDetPendBtn, vDetNewRefAmt)
                intLastCol = vDetCredit
            End If
            .Cols(vDetCredit).Format = "#.00"
            .Cols(vDetDebit).Format = "#.00"
            .Cols(vDetBalAmt).Format = "#.00"
            .Cols(vDetAgRefAmt).Format = "#.00"
            .Cols(vDetNewRefAmt).Format = "#.00"

            If Not IsNothing(dtCumQty) Then
                For k As Int32 = 0 To dtCumQty.Rows.Count - 1
                    .SetData(0, intDetCols + k - dtCumQty.Rows.Count, dtCumQty.Rows(k).Item(1))
                    Call SetHideCol(vsfDet, True, intDetCols + k - dtCumQty.Rows.Count)
                Next
            End If

            If intItemDocID > 0 Then Call initItemVsfCombo()
            Call assignDefPFX()
            Call InitDetAddnlGrid(Me, vsfDet, vsfOptions, intDetCols, strDocDtlTblName, intDocID)

        End With

        intLastCol = 1
        For j = vsfDet.Cols.Count - 1 To 0 Step -1
            If vsfDet.Cols(j).Visible = True Then
                intLastCol = j
                Exit For
            End If
        Next
        Call getIntArrayPrevTran(vsfOptions, dtDetPrevTran)
        Call getAfterFixedNextEntryCol()

        'Hide columns which are marked as TOGGLE HIDE
        For i = 0 To vsfDet.Cols.Count - 1
            If vsfOptions.GetData(i, vOthBlnTogHide) = "Y" Then
                vsfDet.Cols(i).Visible = Not vsfDet.Cols(i).Visible
            End If
        Next


        With vsfVwDet
            .Cols.Count = intDetCols
            .Cols.Fixed = 0
            .Rows.Count = 2
            Call SetFlexColor(vsfVwDet, "", vDetAgRefAmt, vDetNewRefAmt, vDetBalAmt)
            Call SetFlexColor(vsfVwDet, "", vDetAmtType, vDetAcLedName, vDetDebit, vDetCredit)
            .SetData(0, vDetAmtType, "Dr/Cr")
            .Cols(vDetAgRefAmt).AllowEditing = False
            .Cols(vDetNewRefAmt).AllowEditing = False
            .Cols(vDetBalAmt).AllowEditing = False
            .SetData(0, vDetAcLedName, "Ledger")
            .SetData(0, vDetDebit, "Debit Amount")
            .SetData(0, vDetCredit, "Credit Amount")
            .SetData(0, vDetBalAmt, "Balance")
            .SetData(0, vDetAgRefAmt, "Agnst.Ref Amt")
            .SetData(0, vDetNewRefAmt, "New.Ref Amt")

            strDocStdDtlWidth = "525, 105, 15, 2505, 1665, 2500, 2000, 1155, 1125, 915, 960"
            Call SetColAlign(vsfVwDet, svtAlign.Left, vDetAcLedName, vDetAmtType)
            Call SetColAlign(vsfVwDet, svtAlign.Right, vDetCredit, vDetDebit)
            Call SetHideCol(vsfVwDet, True, vDetAcLedID, vDetAgRefAmt, vDetNewRefAmt, vDetBalAmt, vDetPendBtn)
            .Cols(vDetCredit).Format = "#.00"
            .Cols(vDetDebit).Format = "#.00"
            .Cols(vDetBalAmt).Format = "#.00"
            .Cols(vDetAgRefAmt).Format = "#.00"
            .Cols(vDetNewRefAmt).Format = "#.00"
        End With

        If strTenderFld <> "" Then
            intRowTenderFld = getRowNoFromAddnlGrid(vsfTot, strTenderFld)
        End If
        Call showBtns()
        If blnDetSection Then
            ucBtnTools.Visible = True
        End If
        Call initFormVsfDet(Me, strDocHdrTblName, vsfDet, "DET")
        Call initFormVsfDet(Me, strDocHdrTblName, vsfView, "VIEW")
        Call initFormVsfDet(Me, strDocHdrTblName, vsfVwDet, "VWDET")


        intRowVchType = getRowNoFromAddnlGrid(vsfHdr, "VCHTYP")
        intRowCRAmt = getRowNoFromAddnlGrid(vsfTot, "TOTCR")
        intRowDRAmt = getRowNoFromAddnlGrid(vsfTot, "TOTDR")

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
        Dim rsDummy As oledb.oledbDataReader

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
                lblDocNo.Text = strSHForm & " No."
                strDocNoFldName = strSHForm & "No"
                strDocIDFldName = strSHForm & "ID"
                strQtyFldName = strSHForm & "Qty"
                strDocDateFldName = strSHForm & "Date"
                strDocHdrTblName = "TrnHdr" & strSHForm
                strDocDtlTblName = "TrnDtl" & strSHForm
                strDocAmtFldName = strSHForm & "Amt"
                strDocDtlBillTblName = "TrnDtl" & strSHForm & "BillDet"

                strAmtType = "DR|CR" 'GetVal(!RepresentAmtAS, "BY|TO")
                strBillWiseFeatureReq = GetVal(!BillWiseFeatureReq, "N")
                strBalanceColReq = GetVal(!BalanceColReq, "N")

                strFirstAmtType = UCase(GetVal(!DefFirstRowAmtType, Mid(strAmtType, 1, 2)))

                strAuthReq = GetVal(!AuthReq, "N")
                'intNoOfAuth = GetVal(!NoOfAuth, 0)
                'strAuthQuery = GetVal(!NoOfAuthQuerycond, "")
                'If Trim(strAuthQuery) <> "" Then
                'blnAuthCalcluate = True
                'Else

                strAuthReq = GetVal(!AuthReq, "N")
                strAuthQuery = GetVal(!NoOfAuthQueryCond, "")
                intNoOfAuth = GetVal(!NoOfAuth, 0)

                If GetVal(!LinkedTODoc, "N") = "Y" Then
                    blnLinkedToDoc = True
                    strLinkedDocShForm = GetVal(!LinkedTODocShForm, "")
                    strLinkedTOHdrTblName = "TrnHdr" & strLinkedDocShForm
                    strLinkedTODtlTblName = "TrnDtl" & strLinkedDocShForm
                    strLinkedToDocNoFldName = strLinkedDocShForm & "No"
                    strLinkedTODocQtyFldName = strLinkedDocShForm & "Qty"
                    strLinkedTODocFldIDName = strLinkedDocShForm & "ID"
                Else
                    blnLinkedToDoc = False
                End If

                strAddReduceStock = ""
                If GetVal(!AffectsStock, "") = "Add" Then
                    strAddReduceStock = "ADD"
                End If
                If GetVal(!AffectsStock, "") = "Reduce" Then
                    strAddReduceStock = "RED"
                End If

                strInsertDocNo = GetVal(!LoadPendingDocNos, "N")
                strInsertParty = GetVal(!LoadPendingPartyWise, "N")

                blnPartyItem = IIf(GetVal(!FldBlnPartyItem, "N") = "N", False, True)
                BlnEnforcePartyItemLink = IIf(GetVal(!FldBlnEnforcePartyItemLink, "N") = "N", False, True)
                blnLinkedDocFrom = IIf(GetVal(!LinkedFromDoc, "N") = "N", False, True)
                strPendDocShForm = GetVal(!LinkedFromDocShForm, "")
                If strPendDocShForm <> "" Then
                    strPendHdrTblName = "TrnHdr" & strPendDocShForm
                    strPendDtlTblName = "TrnDtl" & strPendDocShForm
                    strPendQtyFldName = strPendDocShForm & "QTY"
                    strPendNoFldName = strPendDocShForm & "NO"
                    strPendIDFldName = strPendDocShForm & "ID"
                    strPendRateFldName = strPendDocShForm & "RATE"
                End If

                strPendDocNo = GetVal(!LinkedPendingDocNo, "N")
                strPendParty = GetVal(!LinkedPendingParty, "N")
                strPendSingleDoc = GetVal(!LinkedPendingSingleDoc, "N")
                strPendingQryFlds = GetVal(!PendingQryFlds, "")
                strPendingTbls = GetVal(!PendingQryTbls, "")
                strPendingQryCond = GetVal(!PendingQryCond, "")


                blnFreezeDoc = IIf(GetVal(!blnFreezeDoc, "N") = "N", False, True)
                intNumOfDays = GetVal(!AllowEditDeleteForDays, 0)
                blnEditDocDate = IIf(GetVal(!AllowEditDocDate, "N") = "N", False, True)
                strPrintAfterSave = GetVal(!PrintAfterSave, "N")
                strTenderFld = UCase(GetVal(!TenderChangeFld, ""))
                blnFreezeQtyCol = IIf(GetVal(!FreezeQtyCol, "N") = "N", False, True)
                blnFreezeRateCol = IIf(GetVal(!FreezeQtyCol, "N") = "N", False, True)
                strEnvelop = GetVal(!blnEnvPrint, "N")

                If GetVal(!OfflineCalc, "N") = "Y" Then
                    blnCalc = True
                End If

                Call ucBtnTools.HideCopyFrom(GetVal(!AllowCopyFrom, "N"))
                'Call ucBtnTools.HideCopyFrom("Y")
                Call ucBtnTools.HideAttachment(GetVal(!FileAttachReq, "N"))
                Call ucBtnTools.HideAmmend(GetVal(!AmmReq, "N"))
                Call ucBtnTools.HideMultiDoc(GetVal(!MultiDocGen, "N"))
                Call ucBtnTools.HideInsertDoc(GetVal(!AllowLoadFromAnotherDoc, "N"))
                Call ucBtnTools.HideTender(GetVal(!AllowTenderChange, "N"))
                Call ucBtnTools.HideImpExp(GetVal(!AllowImpExp, "N"))
                Call ucBtnTools.HideBOM(GetVal(!AllowLoadFromBOM, "N"))
                Call ucBtnTools.HideTotal(GetVal(!OfflineCalc, "N"))
                Call ucBtnTools.HidePending(GetVal(!LinkedFromDoc, "N"))
            End If
            .Close()
        End With

        dtCumQty = clsSVT.getDATATable(" Select LinkDocID,CumQtyFld from UtlTransCum  Where DocID = " & intDocID)
    End Sub

    Private Sub assignDefPFX()
        Dim itm As Object
        Dim strDefPfx As String
        strDefPfx = INIGet("Prefix", Me.Text, "")

        If strDefPfx <> "" Then
            For Each itm In cboPfx.Items
                Dim row As DataRowView = DirectCast(itm, DataRowView)
                If row("Prefix").ToString = strDefPfx Then
                    cboPfx.Text = strDefPfx
                    Exit For
                End If
            Next
        Else
            cboPfx.Text = ""
        End If
    End Sub

    Private Sub clearAddress()

    End Sub

    Public Sub FillAutoGenTrnNo()
        Dim intTrnNo As Long
        intTrnNo = Val(TrnNoAutoGen(strDocHdrTblName, strDocNoFldName, cboPfx.Text, dtDocDate.Value, intDocID, "", IIf(strPfxWiseNo = "Y", True, False)))
        txtNo.Text = intTrnNo
        txtDocNo.Text = getFmtDocNo(Trim(cboPfx.Text), Val(txtNo.Text), dtDocDate.Value)
        If Trim(cboPfx.Text) <> "" Then
            Call getPfxNo(txtDocNo.Text, cboPfx.Text, txtNo.Text)
        End If
        txtNo.Text = Val(txtNo.Text)
    End Sub

    Private Sub cboPfx_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cboPfx.KeyPress
        e.Handled = True
    End Sub

    Private Sub txtNo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtNo.GotFocus
        With txtNo
            .SelectAll()
        End With
    End Sub

    Private Sub txtNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtNo.KeyPress
        'If (Mode = "A" Or Mode = "M") Then
        '    e.Handled = True
        'End If
    End Sub

    Private Sub initItemVsfCombo()
        strSQL = "Select AcLedName"
        strSQL = strSQL & " from MstAcLed"
        strSQL = strSQL & " where InActive = 'N'"
        strSQL = strSQL & " order by 1"
    End Sub

    Private Sub vsfDet_AfterEdit(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfDet.AfterEdit
        If Mode = "V" Or Mode = "" Then Exit Sub
        With vsfDet
            Select Case e.Col
                Case vDetAmtType
                    If .GetData(e.Row, vDetAmtType) = "DR" Then
                        '.SetData(e.Row, vDetBYTO, "BY")
                        .SetData(e.Row, vDetCredit, 0)
                    Else
                        '.SetData(e.Row, vDetBYTO, "TO")
                        .SetData(e.Row, vDetDebit, 0)
                    End If

                Case vDetCredit, vDetDebit
                    Call showPendVch(e.Row)
                    'If Val(.GetData(e.Row, vDetAgRefAmt)) + Val(.GetData(e.Row, vDetNewRefAmt)) = 0 Then
                    '    .SetData(e.Row, vDetAgRefAmt, 0)
                    '    .SetData(e.Row, vDetNewRefAmt, Val(.GetData(e.Row, vDetCredit)) + Val(.GetData(e.Row, vDetDebit)))
                    'End If
            End Select
        End With

        Call vsfOptAfterEditBfrCalTot(Me, vsfDet, vsfOptions, vsfDet.Row, vsfDet.Col)
        Call CalTotal()

    End Sub



    Private Sub vsfDet_BeforeEdit(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfDet.BeforeEdit
        If (Not (Mode = "A" Or Mode = "M")) Then
            e.Cancel = True
            Exit Sub
        End If
        vsfDet.EditMask = ""
        vsfDet.ComboList = ""

        Select Case e.Col
            'Case vDetAgRefAmt, vDetNewRefAmt
            '   e.Cancel = True
            '   Exit Sub
            Case vDetDebit
                If vsfDet.GetData(vsfDet.Row, vDetAmtType) = "CR" Then
                    e.Cancel = True
                    Exit Sub
                End If
            Case vDetCredit
                If vsfDet.GetData(vsfDet.Row, vDetAmtType) = "DR" Then
                    e.Cancel = True

                    Exit Sub
                End If
        End Select

        If e.Col < intDetCols Then Exit Sub

        If vsfOptBeforeEditCancel(Me, vsfDet, vsfOptions, strDocDtlTblName, Mode, vsfDet.Row, vsfDet.Col, intDetCols) Then
            e.Cancel = True
            Exit Sub
        End If
    End Sub

    Private Sub vsfView_BeforeEdit(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfView.BeforeEdit
        e.Cancel = True
    End Sub

    Private Sub vsfSearch_ChangeEdit(ByVal sender As Object, ByVal e As System.EventArgs)
        Call applyFilterFromGrid()
    End Sub

    Private Function applyFilterFromGrid() As Boolean
        applyFilterFromGrid = True
        Dim StrQry As String = ""
        Dim strAccType As String = ""
        With vsfDet
            If .Editor.Text = "" Then
                vsfView.Visible = False
                Exit Function
            End If
            'Select Case Trim(.GetData(.Row, vDetAmtType))
            '    Case "BY", "DR"
            '        strAccType = "TO"
            '    Case "TO", "CR"
            '        strAccType = "BY"
            'End Select

            Select Case Trim(.GetData(.Row, vDetAmtType))
                Case "BY", "DR"
                    strAccType = "BY"
                Case "TO", "CR"
                    strAccType = "TO"
            End Select

            strSQL = "WITH tblChild AS"
            strSQL = strSQL & " ("
            strSQL = strSQL & "     SELECT *"
            strSQL = strSQL & "         FROM MstLedGroup WHERE LedGroupID in ("
            strSQL = strSQL & " Select LedGroupID from UtlAccAmtTypeLedGrps where DocID = " & intDocID
            strSQL = strSQL & " and AmtType = '" & strAccType & "' "
            strSQL = strSQL & " )"
            strSQL = strSQL & "     Union All"
            strSQL = strSQL & "     SELECT MstLedGroup.* FROM MstLedGroup JOIN tblChild  ON mstledgroup.PLedGroupID = tblChild.LedGroupId"
            strSQL = strSQL & " )"
            strSQL = strSQL & " SELECT AcLedName, t.LedGroupName"
            strSQL = strSQL & "     FROM tblChild t, mstAcLed a"
            strSQL = strSQL & "     Where t.LedGroupID = A.LedGroupID"
            strSQL &= " And ACLedName Like '%" & .Editor.Text & "%'"
            strSQL = strSQL & " OPTION(MAXRECURSION 32767)"
        End With
        Call showWaitCursor()
        dtSearch = clsSVT.getDATATable(strSQL)
        dvSearch = New DataView(dtSearch)
        vsfView.DataSource = dvSearch
        Call showNormalCursor()
        vsfView.Refresh()


    End Function

    Private Sub vsfView_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles vsfView.DoubleClick
        If vsfView.Row > 0 Then
            Call loadItemToDtl()
        End If
    End Sub

    Private Sub loadItemToDtl()
        If Not FillItemDet(vsfView.GetData(vsfView.Row, 0), vsfDet.Row) Then
            MsgBox("Invalid Item!!", vbInformation, Me.Text)
            Exit Sub
        End If
        vsfView.Visible = False
        vsfDet.Focus()

        If vsfDet.GetData(vsfDet.Row, vDetAmtType) = "CR" Then
            If Val(vsfDet.GetData(vsfDet.Row, vDetCredit)) = 0 Then
                vsfDet.SetData(vsfDet.Row, vDetCredit, getDiffAmtFromGrid)
            End If
            vsfDet.Col = vDetCredit
            vsfDet.SetData(vsfDet.Row, vDetDebit, 0)
            vsfDet.StartEditing()
        Else
            If Val(vsfDet.GetData(vsfDet.Row, vDetDebit)) = 0 Then
                vsfDet.SetData(vsfDet.Row, vDetDebit, getDiffAmtFromGrid)
            End If
            vsfDet.Col = vDetDebit
            vsfDet.SetData(vsfDet.Row, vDetCredit, 0)
            vsfDet.StartEditing()
        End If
    End Sub

    Private Function getDiffAmtFromGrid() As Double
        Dim i As Integer
        Dim sngTot As Double
        With vsfDet
            sngTot = 0
            For i = 1 To .Rows.Count - 1
                sngTot = sngTot + Val(.GetData(i, vDetDebit)) - Val(.GetData(i, vDetCredit))
            Next i
        End With
        getDiffAmtFromGrid = Math.Abs(sngTot)
    End Function

    Private Function FillItemDet(ByVal argItem As String, ByVal argRow As Long) As Boolean
        Dim blnInActive As Boolean
        FillItemDet = False

        With vsfDet
            If argItem = "" Then
                Exit Function
            End If
            intAcLedID = clsSVT.getDataLongValue("Select AcLedID from MstAcLed where AcLedName = '" & Trim(argItem) & "'", 0, blnInActive)
            If intAcLedID = 0 Then
                MsgBox("A/c Head does not exist!!", vbInformation, Me.Text)
                Exit Function
            End If

            Call clsSVT.getDataLongValue("Select AcLedID from MstAcLed where InActive = 'Y' and AcLedName = '" & Trim(argItem) & "'", 0, blnInActive)

            If blnInActive Then
                MsgBox("This Account Head is InActive! Please select an Active one from the list.", vbInformation, Me.Text)
                Exit Function
            End If
            vsfDet.SetData(vsfDet.Row, vDetAcLedID, intAcLedID)
            vsfDet.SetData(vsfDet.Row, vDetAcLedName, argItem)
            If strBalanceColReq = "Y" Then
                vsfDet.SetData(vsfDet.Row, vDetBalAmt, Format(getAcBalance(intAcLedID), AccStringFmt))
            End If
        End With

        FillItemDet = True
    End Function

    Private Sub vsfView_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles vsfView.KeyUp
        If e.KeyCode = Keys.Escape Then
            vsfView.Visible = False
        End If
        If vsfView.Row > 0 Then
            If e.KeyCode = Keys.Return Or e.KeyCode = Keys.Tab Then
                Call loadItemToDtl()
            End If
        End If
    End Sub

    Private Sub vsfDet_ChangeEdit(sender As Object, e As System.EventArgs) Handles vsfDet.ChangeEdit
        With vsfDet
            If .Col = vDetAcLedName Then

                If fraMain.Location = vsfDet.Location Then
                    If vsfDet.Rows(vsfDet.Row).Top < ucBtnTools.Top Then
                        vsfView.Top = vsfDet.Top + vsfDet.Rows(vsfDet.Row).Top + (vsfDet.Rows(vsfDet.Row).Height) + 20
                    Else
                        vsfView.Top = ucBtnTools.Top + 5
                    End If
                    vsfView.Left = vsfDet.Left + vsfDet.Cols(vsfDet.Col).Left
                    vsfView.Size = New Point(vsfView.Width, tbAdnlFldsFtr.Bottom - vsfView.Top - 10)
                Else
                    If vsfDet.ScrollBarsVisible = ScrollBars.Both Or vsfDet.ScrollBarsVisible = ScrollBars.Vertical Then
                        vsfView.Top = vsfDet.Bottom + 5
                    Else
                        vsfView.Top = vsfDet.Top + vsfDet.Rows(vsfDet.Row).Top + (vsfDet.Rows(vsfDet.Row).Height) + 20
                    End If
                    vsfView.Left = vsfDet.Left + vsfDet.Cols(vsfDet.Col).Left
                    vsfView.Size = New Point(vsfView.Width, tbAdnlFldsFtr.Bottom - vsfView.Top - 10)
                End If
                vsfView.BringToFront()
                vsfView.Visible = True
                Call applyFilterFromGrid()
            End If
        End With
    End Sub

    Private Sub vsfDet_EnterCell(ByVal sender As Object, ByVal e As System.EventArgs) Handles vsfDet.EnterCell
        If (Not (Mode = "A" Or Mode = "M")) Then Exit Sub

        With vsfDet
            Select Case .Col
                Case vDetCredit, vDetDebit
                    If vsfDet.Row < 0 Then Exit Sub
                    '.StartEditing()


                Case Is >= intDetCols
                    If vsfOptions.GetData(.Col, vOthFldBlnComboData) = "Y" Then Exit Sub
                    If vsfOptions.GetData(.Col, vOthFldDataType) = "List String" Then Exit Sub
                    If vsfOptions.GetData(.Col, vOthFldBlnReadOnly) = "Y" Then Exit Sub
                    If vsfOptions.GetData(.Col, vOthFldBlnRefill) = "Y" Then Exit Sub
                    Try
                        .StartEditing()
                    Catch ex As Exception

                    End Try

            End Select
        End With
    End Sub

    Private Sub vsfDet_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles vsfDet.GotFocus
        If Mode = "V" Or Mode = "" Then Exit Sub
        '''Changed by sandhya
        If vsfDet.Rows.Count = 2 AndAlso vsfDet.GetData(1, vDetAcLedName) = "" Then
            If vsfDet.Col = vDetAcLedName Then
                Exit Sub
            End If
            vsfDet.Col = vDetAmtType
        End If
    End Sub

    Private Sub vsfDet_KeyPressEdit(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.KeyPressEditEventArgs) Handles vsfDet.KeyPressEdit

        If e.KeyChar = "'" Then
            e.Handled = True
            Exit Sub
        End If
        Select Case e.Col
            Case vDetCredit, vDetDebit
                e.Handled = Not KeyValidate("Decimal", e.KeyChar, 10)
        End Select

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

        If e.Control = True And e.KeyCode = Keys.C Then
            Call vsfCopyItemCode(vsfDet, vsfDet.Row, vsfDet.Col)
        End If

        If (Not (Mode = "A" Or Mode = "M")) Then Exit Sub

        With vsfDet
            If e.KeyCode = Keys.Escape And .Row = .Rows.Count - 1 And (.Col = vDetAcLedName Or .Col = vDetAmtType) Then

                If Trim(.GetData(.Row, vDetAcLedName)) = "" Then

                    If vsfFtr.Visible Then
                        vsfFtr.Focus()
                        vsfFtr.StartEditing()
                        Exit Sub
                    End If
                    If vsfTot.Visible Then
                        vsfTot.Focus()
                        vsfTot.StartEditing()
                        Exit Sub
                    End If
                    svtBtnCtrl.Focus()
                    Exit Sub
                End If
            End If

            If .Col = vDetAcLedName And e.KeyCode = Keys.Enter Then
                If vsfView.Row > 0 Then
                    Call loadItemToDtl()
                End If
                Exit Sub
            End If
            If .Col = vDetAcLedName And e.KeyCode = Keys.Down AndAlso vsfView.Visible Then
                vsfView.Focus()
                vsfView.Row = 1
                vsfView.Col = 1
            End If

            If .Col = intLastCol And e.KeyCode = Keys.Tab And .Row = .Rows.Count - 1 Then
                If Trim(.GetData(.Row, vDetAcLedName)) <> "" Then
                    .LeftCol = vDetAmtType
                    .Col = vDetAmtType
                    .Col = vDetAmtType
                    .Rows.Count = .Rows.Count + 1
                    .Row = .Rows.Count - 1
                    .TopRow = .Rows.Count - 1
                Else
                    If .Rows.Count > 2 Then
                        .Rows.Count = .Rows.Count - 1
                    End If

                    If vsfFtr.Visible Then
                        vsfFtr.Focus()
                        vsfFtr.StartEditing()
                        Exit Sub
                    End If
                    If vsfTot.Visible Then
                        vsfTot.Focus()
                        vsfTot.StartEditing()
                        Exit Sub
                    End If
                    svtBtnCtrl.Focus()
                    Exit Sub
                End If
            End If
        End With
        If intDetCols < intDetCols Then Exit Sub
    End Sub

    Private Sub vsfDet_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles vsfDet.LostFocus
        If (Not (Mode = "A" Or Mode = "M")) Then Exit Sub
        If (Not (Mode = "A" Or Mode = "M")) Then Exit Sub
        With vsfDet
            If .Rows.Count = 1 Then
                .Rows.Count = 2
            End If
        End With
    End Sub

    Private Sub vsfDet_RowColChange(sender As Object, e As System.EventArgs) Handles vsfDet.RowColChange
        If Not (Mode = "A" Or Mode = "M") Then Exit Sub
    End Sub

    Private Sub vsfDet_SetupEditor(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfDet.SetupEditor
        If (Not (Mode = "A" Or Mode = "M")) Then Exit Sub
        If vsfDet.Col < intDetCols Then Exit Sub

        Call vsfOptionsSetupEditor(Me, vsfDet, vsfOptions, intDocID, strDocHdrTblName, Mode, e.Row, e.Col, "M")
    End Sub

    Public Sub vsfDet_ValidateEdit(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.ValidateEditEventArgs) Handles vsfDet.ValidateEdit
        If (Not (Mode = "A" Or Mode = "M")) Then
            e.Cancel = True
            Exit Sub
        End If
        If vsfDet.Col < intDetCols Then Exit Sub

        If vsfOptValidateEdit(Me, vsfDet, vsfOptions, vsfDet.Row, vsfDet.Col, "T", strDocDtlTblName, "", "") = False Then
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
        If Not (Mode = "A" Or Mode = "M") Then Exit Sub
        Call AddnlFldCalTotal(Me, vsfOptions, vsfDet, intDetCols, "", "", argMode:=Mode)
        Call showDrCrTot()
    End Sub

    Public Function validFlds() As Boolean
        Dim i As Integer, strType As String
        Dim blnFound As Boolean
        'Dim intDupRow As Integer, strDupDataCaption As String, strDupDataValue As String
        validFlds = False
        '============
        'header validations
        '============
        If blnCalc = True Then
            Call CalTotal()
        End If

        If Not validDate(dtDocDate.Value) Then
            MsgBox("Doc Date Should be with the financial year", vbExclamation, Me.Text)
            dtDocDate.Focus()
            Exit Function
        End If
        'DOCno number has to be generated
        If Trim(txtDocNo.Text) = "" Then
            MsgBox(strSHForm & " Number is empty!!", vbInformation, Me.Text)
            Exit Function
        End If

        If Mode = "A" Then
            Call clsSVT.getDataStringValue("Select " & strDocNoFldName & " from " & strDocHdrTblName & " where " &
                    strDocNoFldName & " = '" & txtDocNo.Text & "'", "", blnFound)
            If blnFound Then Call FillAutoGenTrnNo()
        End If

        With vsfDet
            For intGridRow As Int32 = 0 To .Rows.Count - 1
                For intGridCol As Int32 = 0 To .Cols.Count - 1
                    .SetData(intGridRow, intGridCol, CStr(GetVal(.GetData(intGridRow, intGridCol), "")).Replace("'", "`"))
                Next
            Next
        End With

        With vsfDet
            If intItemDocID > 0 Then
                Call RemoveEmptVSFRows(vsfDet, 1, vDetAcLedName)
                For i = 1 To .Rows.Count - 1
                    intAcLedID = clsSVT.getDataLongValue("Select AcLedID from MstAcLed where AcLedName = '" & Trim(.GetData(i, vDetAcLedName)) & "'", 0, False)
                    If intAcLedID = 0 Then
                        MsgBox("AcLedName does not exist!!", vbInformation, Me.Text)
                        .Row = i
                        .TopRow = i
                        .Col = vDetAcLedName
                        .Focus()
                        Exit Function
                    End If
                    If Trim(.GetData(i, vDetAcLedName)) = "" Then
                        MsgBox("Item Code Not Selected", vbInformation, Me.Text)
                        .Focus()
                        .Row = i
                        .Col = vDetAcLedName
                        .StartEditing()
                        Exit Function
                    End If
                    strType = clsSVT.getDataStringValue("Select LedType from MstAcLed Where AcLedID = " & Val(.GetData(i, vDetAcLedID)), "", False)
                Next i

            Else
                If intDetNextEntryCol > 0 Then
                    Call RemoveEmptVSFRows(vsfDet, 1, intDetNextEntryCol)
                End If
            End If
            Dim sngCrTot As Double = 0, sngDrTot As Double = 0, strPartyBillWise As String = ""
            For i = 1 To .Rows.Count - 1
                sngCrTot = sngCrTot + Val(.GetData(i, vDetCredit))
                sngDrTot = sngDrTot + Val(.GetData(i, vDetDebit))
            Next
            If sngCrTot <> sngDrTot Then
                MsgBox("Debit and Credit totals are NOT matching!", MsgBoxStyle.Critical, Me.Text)
                Exit Function
            End If
        End With


        If Not validOthFLds(Me, strDocHdrTblName, strDocNoFldName, txtDocNo.Text, Trim(strSHForm)) Then
            Exit Function
        End If
        If blnDetSection Then
            If Not validOthDetFLds(Me, vsfDet, vsfOptions, intDetCols) Then Exit Function
        End If
        validFlds = True
    End Function

    Public Function FillDocDetails(ByVal argDocID As Long) As Boolean
        FillDocDetails = False
        If Trim(argDocID) = "" Or Val(argDocID) = 0 Then
            Call cboPfx_Validating(Nothing, Nothing)
            Exit Function
        End If
        txtDocID.Text = argDocID
        StMsg("Filling Header Fields...")
        Call fillDocHdrFlds(argDocID)
        StMsg("Filling Detail Fields...")
        Call fillDtlFields(argDocID)
        Call fillAdjDtlFields(argDocID)
        FillDocDetails = True

        StMsg("")
        With svtFileAttach
            .ClearFiles()
            .LoadFilesFromData("Select AFilename from UtlAttachFiles Where DocID = " & intDocID & " and DocValueID = '" & argDocID & "'")
        End With
        Call refreshAttachBtn()
        Call showDrCrTot()
    End Function

    Public Function fillAdjDtlFields(ByVal argDocID As Long) As Boolean
        Dim i As Integer
        Dim strsql As String
        '--6 start CopyFROM
        Dim introw As Integer, lngAccID As Long

        lngAccID = clsSVT.getDataLongValue("Select AccID from TrnhdrACC where TrnID = " & argDocID & " and DocType = '" & strSHForm & "'", 0, False)
        fillAdjDtlFields = False
        Dim rsDtl As oledb.oledbDataReader

        strsql = "Select * from  TrnDtlAccAdj where AccID = " & lngAccID
        rsDtl = clsSVT.getDataReaderFromSQL(strsql)
        With rsDtl
            If Not .HasRows Then
                fillAdjDtlFields = False
                .Close()
                Exit Function
            End If

            vsfAdj.Rows.Count = 1
            Do While .Read
                vsfAdj.Rows.Count = vsfAdj.Rows.Count + 1
                introw = vsfAdj.Rows.Count - 1
                i = introw
                vsfAdj.SetData(i, vAdjAccID, rsDtl("AdjAgnstAccID"))
                vsfAdj.SetData(i, vAdjAccLedID, rsDtl("AcLedID"))
                If rsDtl!CRAMT > 0 Then
                    vsfAdj.SetData(i, vAdjAdjAmt, rsDtl!CrAmt)
                    vsfAdj.SetData(i, vAdjDrCr, "CR")
                Else
                    vsfAdj.SetData(i, vAdjAdjAmt, rsDtl!drAmt)
                    vsfAdj.SetData(i, vAdjDrCr, "DR")
                End If
                vsfAdj.SetData(i, vAdjType, rsDtl!AdjType)
                vsfAdj.SetData(i, vAdjNewRefNo, rsDtl!NewRefNo)
            Loop
            .Close()
        End With
        fillAdjDtlFields = True
    End Function

    Private Function fillDocHdrFlds(ByVal argDocID As Long) As Boolean
        Dim rsDocHdr As oledb.oledbDataReader = Nothing

        Call DateFreezing(Me, "")
        rsDocHdr = clsSVT.getDataReaderFromSQL("select * from " & strDocHdrTblName & "  " _
                                                            & " where " & strDocIDFldName & " = " & argDocID)
        Call ClearAddnlGrid(vsfHdr)

        With rsDocHdr
            If Not .HasRows Then
                fillDocHdrFlds = False
                .Close()
                Exit Function
            End If
            .Read()

            txtDocID.Text = argDocID
            txtDocNo.Text = rsDocHdr(strDocNoFldName)
            Call getPfxNo(txtDocNo.Text, strPfx, strNo)
            cboPfx.Text = strPfx
            txtNo.Text = Val(strNo)

            'fill the Doc date
            dtDocDate.Value = Format(rsDocHdr(strDocDateFldName), "dd/MM/yyyy")
            If !cancelled = "Y" Then
                lblCancelled.Visible = True
            Else
                lblCancelled.Visible = False
            End If
            If strAuthReq = "Y" Then
                lblAuthorised.Visible = True
                If !Authorised = "Y" Then
                    lblAuthorised.Text = "AUTHORISED"
                    lblAuthorised.ForeColor = Color.ForestGreen
                Else
                    lblAuthorised.Text = "UN-AUTHORISED"
                    lblAuthorised.ForeColor = Color.Red
                End If
            Else
                lblAuthorised.Visible = False
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

            .Close()
            fillDocHdrFlds = True
        End With
    End Function

    Public Function fillDtlFields(ByVal argDocID As Integer, Optional ByVal blnAppend As Boolean = False) As Boolean
        Dim i As Integer
        Dim strsql As String
        '--6 start CopyFROM
        Dim introw As Integer

        fillDtlFields = False
        Dim rsDtl As oledb.oledbDataReader

        strsql = "Select d.*, AcLedName"
        strsql = strsql & " from  " & strDocDtlTblName & "  d, MstAcLed i"
        strsql = strsql & " where " & strDocIDFldName & "= " & argDocID
        strsql = strsql & " and d.AcLedID  =  i.ACLedID"
        strsql = strsql & " Order by slNo"
        rsDtl = clsSVT.getDataReaderFromSQL(strsql)

        With rsDtl
            If Not .HasRows Then
                fillDtlFields = False
                .Close()
                Exit Function
            End If

            vsfDet.Rows.Count = 1
            vsfVwDet.Rows.Count = 1
            Do While .Read
                vsfDet.Rows.Count = vsfDet.Rows.Count + 1
                introw = vsfDet.Rows.Count - 1
                i = introw
                vsfDet.SetData(i, vDetAcLedID, rsDtl("AcLedID"))
                vsfDet.SetData(i, vDetAcLedName, rsDtl("AcLedName"))


                vsfDet.SetData(i, vDetAmtType, rsDtl!AmtType)

                vsfDet.SetData(i, vDetCredit, rsDtl!CrAmt)
                If Val(vsfDet.GetData(i, vDetCredit)) = 0 Then vsfDet.SetData(i, vDetCredit, "")
                vsfDet.SetData(i, vDetDebit, rsDtl!DrAmt)
                If Val(vsfDet.GetData(i, vDetDebit)) = 0 Then vsfDet.SetData(i, vDetDebit, "")
                vsfDet.SetData(i, vDetAgRefAmt, GetVal(rsDtl!AgRefAmt, ""))
                vsfDet.SetData(i, vDetNewRefAmt, GetVal(!NewRefAmt, ""))
                If Val(vsfDet.GetData(i, vDetAgRefAmt)) = 0 Then vsfDet.SetData(i, vDetAgRefAmt, "")
                If Val(vsfDet.GetData(i, vDetNewRefAmt)) = 0 Then vsfDet.SetData(i, vDetNewRefAmt, "")
                vsfVwDet.Rows.Count = vsfVwDet.Rows.Count + 1
                For j = 0 To vsfDet.Cols.Count - 1
                    vsfVwDet.SetData(vsfVwDet.Rows.Count - 1, j, vsfDet.GetData(vsfDet.Rows.Count - 1, j))
                Next
                If chkShowBills.Checked Then
                    Call fillAjdFldsInView(rsDtl(strDocIDFldName), rsDtl!AcLedID)
                End If
                Call FillvsfOptDtlFlds(Me, vsfDet, vsfOptions, rsDtl, i, intDetCols)
            Loop
            .Close()
        End With
        Call RemoveEmptVSFRows(vsfDet, 2, vDetAcLedName)
        Call CalTotal()

        fillDtlFields = True
    End Function
    Private Sub fillAjdFldsInView(ByVal argDocID As Long, ByVal argAcLedID As Long)
        Dim i As Integer
        Dim strsql As String
        '--6 start CopyFROM
        Dim introw As Integer
        Dim lngAccID As Long
        lngAccID = clsSVT.getDataLongValue("Select AccID from TrnhdrACC where TrnID = " & Val(txtDocID.Text) & " and DocType = '" & strSHForm & "'", 0, False)
        Dim rsDtl As oledb.oledbDataReader

        strsql = "Select h.doctype, h.trnno, h.trndate, a.dramt, a.cramt, adjtype, '' as nrefNo"
        strsql = strsql & " from trndtlaccadj a, trndtlacc d, trnhdracc h"
        strsql = strsql & " where(a.accid = d.accid)"
        strsql = strsql & " and a.adjagnstaccid = h.accid"
        strsql = strsql & " and a.acledid  = d.acledid"
        strsql = strsql & " and a.accid = " & lngAccID
        strsql = strsql & " and d.acledid = " & argAcLedID
        strsql = strsql & " union all"
        strsql = strsql & " Select h.doctype, h.trnno, h.trndate, a.dramt, a.cramt, adjtype, NewRefNo"
        strsql = strsql & " from trndtlaccadj a,  trnhdracc h"
        strsql = strsql & " where a.accid = h.accid"
        strsql = strsql & " and a.adjagnstaccid = 0 and AdjType = 'N'"
        strsql = strsql & " and a.accid = " & lngAccID
        strsql = strsql & " and a.acledid = " & argAcLedID

        'strsql = strsql & " and adjAgnstaccid > 0"
        rsDtl = clsSVT.getDataReaderFromSQL(strsql)
        With rsDtl
            If Not .HasRows Then
                .Close()
                Exit Sub
            End If
            Do While .Read
                vsfVwDet.Rows.Count = vsfVwDet.Rows.Count + 1
                introw = vsfVwDet.Rows.Count - 1
                i = introw
                If !AdjType = "A" Then
                    vsfVwDet.SetData(i, vDetAcLedName, "Against Ref : " & !TrnNo & "  -  " & !TrnDate & "  -  " & Format(!DrAmt + !CrAmt, "0.00") & (IIf(!drAmt <> 0, "Dr", "Cr")))
                Else
                    vsfVwDet.SetData(i, vDetAcLedName, "New Ref : " & !nrefNo & "  -  " & dtDocDate.Value & "  -  " & Format(!DrAmt + !CrAmt, "0.00") & (IIf(!drAmt <> 0, "Dr", "Cr")))
                End If
            Loop
            .Close()
        End With
    End Sub

    Private Sub prntCrpt()
        Dim strSF As String
        Dim strFormulas(,) As String = Nothing
        Dim j As Integer = 0
        Dim i As Integer = 0, intAccID As Long
        Dim intFieldCount = 0, strRptPath As String = ""
        ReDim strFormulas(7, 1)


        'If intDocID = 207 Then
        '    ReDim strFormulas(8, 1)
        'End If

        strFormulas(0, 0) = "compname"
        strFormulas(0, 1) = gSettings.gCompName

        strFormulas(1, 0) = "add1"
        strFormulas(1, 1) = gSettings.gCompAdd1

        strFormulas(2, 0) = "add2"
        strFormulas(2, 1) = gSettings.gCompAdd2

        strFormulas(3, 0) = "add3"
        strFormulas(3, 1) = gSettings.gCompAdd3

        strFormulas(4, 0) = "add4"
        strFormulas(4, 1) = gSettings.gCompAdd4

        'strFormulas(5, 0) = "frmlVchName"
        'strFormulas(5, 1) = "Payment Voucher"

        Call PrintVch(Me, Val(txtDocID.Text))

        If Not intDocID = 204 Or intDocID = 205 Then
            With vsfFtr
                For j = 5 To 9
                    If i <= .Rows.Count - 1 AndAlso Trim(.GetData(i, vOthFldCaption)) <> "" Then
                        strFormulas(j, 0) = "ftr" & i + 1
                        strFormulas(j, 1) = .GetData(i, vOthFldCaption) & ":" & .GetData(i, vOthFldDataValue)
                        intFieldCount += 1
                    End If
                    i += 1
                Next

                'If intFieldCount < 4 Then
                '    For j = intFieldCount To 4
                '        strFormulas(j + 5, 0) = "ftr" & intFieldCount + 1
                '        strFormulas(j + 5, 1) = ""
                '        intFieldCount += 1
                '    Next
                'End If

            End With


            'If intDocID = 203 Or intDocID = 206 Then
            '    strFormulas(8, 0) = "ftr4"
            '    strFormulas(8, 1) = ""
            'End If

        End If
        If intDocID = 204 Or intDocID = 205 Then
            strFormulas(5, 0) = "ftr1"
            strFormulas(5, 1) = ""

            strFormulas(6, 0) = "ftr2"
            strFormulas(6, 1) = ""

            strFormulas(7, 0) = "ftr3"
            strFormulas(7, 1) = ""

            strFormulas(8, 0) = "ftr4"
            strFormulas(8, 1) = ""
        End If
        'strFormulas(5, 0) = "ftr1"
        'strFormulas(5, 1) = "test:1"

        'strFormulas(6, 0) = "ftr2"
        'strFormulas(6, 1) = "test:1"

        'strFormulas(7, 0) = "ftr3"
        'strFormulas(7, 1) = "test:1"

        'strFormulas(8, 0) = "ftr4"
        'strFormulas(8, 1) = "test:1"

        'strFormulas(9, 0) = "ftr5"
        'strFormulas(9, 1) = "test:1"

        'strSF = "{TrnHdr" & strSHForm & "." & strSHForm & "ID} = " & Val(txtDocID.Text)

        intAccID = clsSVT.getDataLongValue("Select accid from trnhdracc where doctype = '" & strSHForm & "' and TrnID = " & Val(txtDocID.Text), 0, False)
        strSF = "{TrnHdrAcc.AccID} = " & intAccID


        'SelectionFormula = "{RptIncentive.UserName}= '" & gUserCode & "'"

        Dim frmv As New frmUtlReportViewer
        With frmv
            '.MdiParent = frmMain
            .StartPosition = FormStartPosition.CenterScreen
            .showRptFile(Me.Text, gstrRptPath & "fmtVCH.rpt", strSF, strFormulas, False, True, False)
            .ShowDialog()
        End With
    End Sub

    Private Sub refreshAttachBtn()
        Dim intCtr As Integer = 0
        With svtFileAttach.vsfDet
            For i = 1 To .Rows.Count - 1
                If .Rows(i).Visible Then
                    intCtr = intCtr + 1
                End If
            Next
        End With
        If intCtr > 0 Then
        End If
    End Sub

    Private Sub deleteDoc(argType As String)
        If Val(txtDocID.Text) = 0 Then Exit Sub
        Dim strErrMsg As String = ""
        Dim intAccID As Integer
        If lblCancelled.Visible Then
            MsgBox("Document is already cancelled", MsgBoxStyle.Critical, "Cancel Document")
            Exit Sub
        End If
        ' End If
        If argType = "CANCEL" Then
            If MsgBox("Are you sure to CANCEL this Document ?", vbCritical + vbYesNo, Me.Text & " Cancellation") = vbNo Then Exit Sub
        Else
            If MsgBox("Are you sure to DELETE this Document ?", vbCritical + vbYesNo, Me.Text & " Deletion") = vbNo Then Exit Sub
        End If

        Try
            svtTrn = svtConn.BeginTransaction(IsolationLevel.ReadCommitted)

            'for audit track
            If blnAuditTrack Then
                Call RecordAuditLog(strFrmCaption, Val(txtDocID.Text), UserActions.DeleteAction, strDocHdrTblName, strAuditHdrSQL, strAuditDtlSQL, svtTrn)
            End If

            If argType = "DELETE" Then
                If blnDetSection Then
                    clsSVT.execSQLStatement("Delete from " & strDocDtlTblName & " where " & strDocIDFldName & " = " & Val(txtDocID.Text), svtTrn)
                End If
                clsSVT.execSQLStatement("Delete from " & strDocHdrTblName & " where " & strDocIDFldName & " = " & Val(txtDocID.Text), svtTrn)
                clsSVT.execSQLStatement("Delete from " & strDocDtlTblName & "BillDet where " & strDocIDFldName & " = " & Val(txtDocID.Text), svtTrn)

                intAccID = clsSVT.getDataLongValue("Select AccID from TrnhdrACC where TrnID = " & Val(txtDocID.Text) & " and DocType = '" & strSHForm & "'", 0, False, svtTrn)
                clsSVT.execSQLStatement("Delete from TrnHdrACC where AccID = " & intAccID, svtTrn)
                clsSVT.execSQLStatement("Delete from TrnDtlACC where AccID = " & intAccID, svtTrn)
                clsSVT.execSQLStatement("Delete from TrnDtlACCAdj where AccID = " & intAccID, svtTrn)
            Else
                clsSVT.execSQLStatement("Update " & strDocHdrTblName & " Set Cancelled = 'Y' where " & strDocIDFldName & " = " & Val(txtDocID.Text), svtTrn)
            End If

            svtTrn.Commit()
            If argType = "DELETE" Then Call clearFlds()

            MsgBox(IIf(argType = "CANCEL", "Cancelled", "Deleted") & " " & Me.Text & " Successfully!", MsgBoxStyle.Critical, "Deleted")
        Catch ex As Exception
            svtTrn.Rollback()
            MsgBox(ex.Message, MsgBoxStyle.Information, "Cannot Delete / Cancel")
        End Try
    End Sub

    Public Sub fillCustSupp(ByVal argCustSuppReq As String, ByVal argHdrTbl As String, ByVal argDocFldName As String, ByVal argDocNo As String)
        Dim strParty As String

        If argCustSuppReq = "C" Then
            strParty = "Customer"
        ElseIf argCustSuppReq = "S" Then
            strParty = "Supplier"
        Else
            Exit Sub
        End If

        Select Case argCustSuppReq
            Case "C"
                strPartyTblName = "MstCust"
                strPartyAddressTblName = "MstCustAddress"
                strFldPartyID = "CustID"
                strFldPartyCode = "CustCode"
                strFldPartyName = "CustName"
            Case "S"
                strPartyTblName = "MstSupp"
                strPartyAddressTblName = "MstCustAddress"
                strFldPartyCode = "SuppCode"
                strFldPartyID = "SuppID"
                strFldPartyName = "SuppName"
        End Select

        Dim strSql As String, strPartyCode As String

        intPartyID = clsSVT.getDataLongValue("Select distinct " & strFldPartyID & " from " & argHdrTbl & " where " & argDocFldName & " = '" & argDocNo & "'", 0, False)

        strSql = "Select " & strFldPartyCode & " from " & strPartyTblName & " where " & strFldPartyID & " = "
        strSql = strSql & "(select distinct " & strFldPartyID & " from " & argHdrTbl & " where " & strFldPartyID & " = " & intPartyID & ")"

        strPartyCode = clsSVT.getDataStringValue(strSql, "", False)

        If strPartyCode <> "" Then
        Else
            Exit Sub
        End If
    End Sub

    Private Sub frmTrnVch_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Call ReSizeHdrFtr()
        Dim ToolTip As New ToolTip
        Dim blnNewMode As Boolean = False, blnLoadLastTrn As Boolean = False
        Call initFormSize(Me, strDocHdrTblName, False, 958, 579, 0, 0)
        Call initFormVsfDet(Me, Me.Text, vsfFtr, "FTR")
        Call initFormVsfDet(Me, Me.Text, vsfTot, "TOT")
        Call initFormVsfDet(Me, Me.Text, vsfHdr, "HDR")
        Call initFormVsfDet(Me, Me.Text, vsfDet, "DET")
        initGridStyles(Me)
        lblTitle.Text = "View - " & Me.Text
        Call getCtrlSize()
        srcTransaction.Visible = True
        Call srcTransaction.setFocus()
        Call GridPinVisible()
        Call setFormSetttings()
        Call LoadLastOrNewTrn(intDocID, blnNewMode, blnLoadLastTrn)
        If blnNewMode Then
            svtBtnCtrl_btnClicked(svtBtns.svtNew)
        ElseIf blnLoadLastTrn Then
            If lngViewDocID = 0 Then
                strSQL = "Select max(" & getSyntax("ISNULL0", strDocIDFldName) & ") from " & strDocHdrTblName
            Else
                strSQL = "Select " & getSyntax("ISNULL0", strDocIDFldName) & " from " & strDocHdrTblName & " Where " & strDocIDFldName & " = " & lngViewDocID
            End If
            Call FillDocDetails(clsSVT.getDataLongValue(strSQL, 0, False))

        End If

        fillFormProperties(Me)
        lblTitle.ForeColor = Color.White
        srcTransaction.lblParty.ForeColor = Color.White
        'srcTransaction.ForeColor = Color.White
        If Not blnDetSection Then
            btnDetPin.Visible = False
            ucBtnTools.Visible = False
        End If
        If Not vsfTot.Visible Then
            lblTotTip.Visible = False
        End If
        Call GridStylesUpdateFromDocDsgr()
        lblDebitTotal.ForeColor = Color.White
        lblCreditTotal.ForeColor = Color.White
        lblTotDrValue.ForeColor = Color.White
        lblTotCrValue.ForeColor = Color.White
        chkShowBills.Checked = True
        lblFrm.Font = New Font("Microsoft Sans Serif", 14, FontStyle.Bold)
        ucBtnTools.btnVisible("", False, False)
        cboPfx.SelectedIndex = -1
        cboPfx.Enabled = False
    End Sub

    Private Sub setFormSetttings()
        If LoadFormSettings(intDocID, "Header") Then
            blnVHdr = False
            Call reSizeVSF("H")
        End If
        If LoadFormSettings(intDocID, "Footer") Then
            blnVFtr = False
            Call reSizeVSF("F")
        End If
        If LoadFormSettings(intDocID, "Detail") Then
            blnVsfMax = False
            Call reSizeVSF("D")
        End If
        If LoadFormSettings(intDocID, "Total") Then
            blnVTot = False
            Call reSizeVSF("T")
        End If
    End Sub

    Private Sub vsfTot_AfterEdit(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfTot.AfterEdit
        Call vsfGenAfterEditBfrCalTot(Me, vsfTot, e.Row, "M")
        If blnCalc = False Then
            Call CalTotal()
        End If
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
        Call vsfSetupEditor(Me, vsfTot, intDocID, strDocHdrTblName, Mode, e.Row, e.Col, "M")
    End Sub

    Private Sub vsfTot_ValidateEdit(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.ValidateEditEventArgs) Handles vsfTot.ValidateEdit
        If Not vsfValidateEdit(Me, vsfTot, e.Row, e.Col, "M", strDocHdrTblName, strDocNoFldName, txtDocNo.Text) Then
            e.Cancel = True
        End If
    End Sub

    Private Sub vsfTot_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles vsfTot.KeyUp
        If e.KeyCode = Keys.Tab Then
            If vsfTot.Row = vsfTot.Rows.Count - 1 Then
                svtBtnCtrl.Focus()
            End If
        End If
        If e.Control = True AndAlso e.KeyCode = Keys.G Then
            Call GridFocusChange("F")
        End If
    End Sub

    Private Sub cboPfx_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles cboPfx.Validating
        If Mode = "A" Then
            If strPfxTYpe = "" Then
                Call cboPfxValidate(Me, blnallowToChangePfx, intDocID, Mode, False)
            End If
            Exit Sub
        Else
            If strPfxTYpe = "" Then
                Call cboPfxValidate(Me, blnallowToChangePfx, intDocID, Mode, False)
            End If
            If Val(txtNo.Text) > 1 Then
                txtNo.Text = Val(txtNo.Text) - 1
                Call txtNo_Validated(Nothing, Nothing)
            Else
                Call clearFlds()
                Lock(txtNo, True)
                txtNo.Focus()
            End If
            Call showBtns()
            Exit Sub
        End If

    End Sub

    Private Sub txtNo_Validated(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtNo.Validating
        Dim strDocNo As String, blnFound As Boolean
        If Val(txtNo.Text) <= 0 Then
            Call showBtns()
            Exit Sub
        End If
        If Mode = "A" Then
            strDocNo = getFmtDocNo(Trim(cboPfx.Text), Val(txtNo.Text), dtDocDate.Value)
            Call clsSVT.getDataStringValue("Select " & strDocNoFldName & " from " & strDocHdrTblName & " where " & strDocNoFldName & " = '" & strDocNo & "'", "", blnFound)

            If blnFound Then
                MsgBox(strSHForm & " Number already exists! Cannot Add!!", vbInformation, Me.Text)
                txtNo.Focus()
                Exit Sub
            End If
            txtDocNo.Text = getFmtDocNo(cboPfx.Text, Val(txtNo.Text), dtDocDate.Value)
            Exit Sub
        End If

        If Mode = "M" Then Exit Sub
        txtDocNo.Text = getFmtDocNo(Trim(cboPfx.Text), Val(txtNo.Text), dtDocDate.Value)

        Call clsSVT.getDataStringValue("Select " & strDocNoFldName & " from " & strDocHdrTblName & " where " & strDocNoFldName & " = '" & txtDocNo.Text & "'", "", blnFound)

        If blnFound Then
            txtDocID.Text = clsSVT.getDataStringValue("Select " & strDocIDFldName & " from " & strDocHdrTblName & " where " & strDocNoFldName & " = '" & txtDocNo.Text & "'", "", blnFound)
            Call FillDocDetails(Val(txtDocID.Text))
        Else
            If Mode = "" Or Mode = "V" Then
                MsgBox(strFrmCaption & " No.: " & txtDocNo.Text & " does not exist!", vbInformation, Me.Text)
                txtDocNo.Text = ""
                Call clearFlds()
                Lock(txtNo, True)
                txtNo.Focus()
            End If
        End If
        Call showBtns()
        Call getPfxNo(txtDocNo.Text, strPfx, strNo)

    End Sub

    Private Sub svtBtnCtrl_btnOptionSelected(argOptionName As String, argBlnCtrlClick As Boolean, argBlnAltClick As Boolean) Handles svtBtnCtrl.btnOptionSelected
        Dim strSF As String
        Dim strFormulas(,) As String
        strSF = "{TrnHdr" & strSHForm & "." & strSHForm & "ID} = " & Val(txtDocID.Text)

        ReDim strFormulas(1, 2)

        strFormulas = Nothing
        'strSHForm = argOptionName

        If txtDocNo.Text <> "" Then
            Select Case UCase(argOptionName)
                Case "SHORTCLOSE"
                    If Not chkUserRights(strRights, chkRightsPos.EditRights, Me.Text) Then Exit Sub
                    If lblCancelled.Visible Then
                        MsgBox("Cancelled Order cannot be shortclosed!", MsgBoxStyle.Critical, Me.Text)
                    Else
                        Call FillDocDetails(txtDocID.Text)
                        'With frmUtlShortClosed
                        '    .loadShortCloseDtls(Me, Val(txtDocID.Text), Trim(txtDocNo.Text), Me.Text, strSHForm, vsfDet)
                        '    .ShowDialog()
                        'End With
                    End If
                Case "LABEL PRINTING"
                    Dim objfrm As New frmRptLblPrint
                    objfrm.ShowDialog()
                    objfrm.Dispose()

                Case Else
                    If Not chkUserRights(strRights, chkRightsPos.PrintRights, Me.Text) Then Exit Sub
                    Dim frmv As New frmUtlReportViewer
                    With frmv
                        '.MdiParent = frmMain
                        .StartPosition = FormStartPosition.CenterScreen
                        .ShowRptFileFromUtlOptions(intDocID, Val(txtDocID.Text), argOptionName, True)
                        .Show()
                    End With
            End Select
        Else
            If UCase(argOptionName) = "LABEL PRINTING" Then
                Dim objfrm As New frmRptLblPrint
                objfrm.ShowDialog()
                objfrm.Dispose()
            End If
            Exit Sub
        End If
    End Sub

    Public Function SaveDoc() As Boolean
        SaveDoc = False
        ' for adding / editing Record
        showWaitCursor()
        Dim strErrMsg As String = ""
        svtTrn = svtConn.BeginTransaction(IsolationLevel.ReadCommitted)
        Try
            If Mode = "M" Then
                intMaxDocID = Val(txtDocID.Text)
                If Mode = "M" Then
                    clsSVT.execSQLStatement("Delete from " & strDocDtlTblName & " where " & strDocIDFldName & " = " & Val(txtDocID.Text), svtTrn)
                    clsSVT.execSQLStatement("Delete from " & strDocDtlTblName & "BillDet where " & strDocIDFldName & " = " & Val(txtDocID.Text), svtTrn)
                    intMaxDocID = Val(txtDocID.Text)
                End If
            End If
            If Not UpdtDocHdrDtl() Then
                SaveDoc = False
                svtTrn.Rollback()
                Exit Function
            End If

            If Not UpdtDocAdjDtl(intMaxDocID) Then
                SaveDoc = False
                svtTrn.Rollback()
                Exit Function
            End If
            If Not UpdtVchHdr(intMaxDocID) Then
                SaveDoc = False
                svtTrn.Rollback()
                Exit Function
            End If

            'for audit track
            If blnAuditTrack Then
                Call RecordAuditLog(strFrmCaption, Val(txtDocID.Text), IIf(Mode = "A", UserActions.AddAction, UserActions.EditAction), strDocHdrTblName, strAuditHdrSQL, strAuditDtlSQL, svtTrn)
            End If

            svtTrn.Commit()

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
            strSQL = strSQL & strFlds & ") output inserted." & strDocIDFldName & " into @testTable values ("
            strSQL = strSQL & "'" & txtDocNo.Text & "', "
            strSQL = strSQL & "'" & cboPfx.Text & "', "
            strSQL = strSQL & "'" & getPrintDocFmtNo(intDocID, txtDocNo.Text,
                                    cboPfx.Text, txtNo.Text, svtTrn) & "', "
            strSQL = strSQL & "'" & getDateFormatInInsert(dtDocDate.Value) & "', "
            strSQL = strSQL & "'" & gUserCode & "', "
            strSQL = strSQL & "'" & getDateFormatInInsert(getSystemDateTime(svtTrn), True) & "', "
            If strAuthReq = "N" Then
                strSQL = strSQL & "'Y',"
            Else
                strSQL = strSQL & "'N',"
            End If

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
            strSQL = strSQL & "Authorised = 'N', "
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

    Private Function UpdtDocAdjDtl(ByVal argHdrID As Long) As Boolean
        Dim strsql As String, intFactor As Integer
        Dim i As Integer
        UpdtDocAdjDtl = False
        intFactor = 0
        With vsfAdj
            For i = 1 To .Rows.Count - 1
                If Val(.GetData(i, vAdjAccLedID)) > 0 Then
                    'add to detail file
                    strsql = "Insert into " & strDocDtlBillTblName & " (" & strDocIDFldName & ",AcLedID,AccID,AdjAmt,AdjType) values ("
                    strsql = strsql & argHdrID & ","
                    strsql = strsql & Val(.GetData(i, vAdjAccLedID)) & ","
                    strsql = strsql & Val(.GetData(i, vAdjAccID)) & ","
                    strsql = strsql & Val(.GetData(i, vAdjAdjAmt)) & ",'A')"
                    If Not clsSVT.execSQLStatement(strsql, svtTrn) Then
                        UpdtDocAdjDtl = False
                        Exit Function
                    End If
                End If
            Next i
        End With
        UpdtDocAdjDtl = True
    End Function

    Private Function UpdtVchHdr(ByVal argHdrID As Long) As Boolean
        Dim strsql As String, intFactor As Integer, lngAccID As Long
        Dim insMode As String
        Dim intAccID As Long
        intAccID = clsSVT.getDataLongValue("Select AccID from TrnhdrACC where TrnID = " & argHdrID & " and DocType = '" & strSHForm & "'", 0, False, svtTrn)
        insMode = "A"
        If intAccID > 0 Then
            insMode = "M"
            Call clsSVT.execSQLStatement("Delete from TrnDtlAcc where AccID = " & intAccID, svtTrn)
            Call clsSVT.execSQLStatement("Delete from TrnDtlAccAdj where AccID = " & intAccID, svtTrn)
        End If
        Dim i As Integer
        UpdtVchHdr = False
        intFactor = 0

        'add to header file
        If insMode = "A" Then
            strsql = "insert into trnHdrAcc (DocType,AccType, "
            strsql = strsql & " TrnID, TrnNo, TrnDate, DueDate,Narrn"
            strsql = strsql & ") output inserted.AccID into @testTable values ("
            strsql = strsql & "'" & strSHForm & "',"
            strsql = strsql & "'V',"
            strsql = strsql & argHdrID & ","
            strsql = strsql & "'" & txtDocNo.Text & "',"
            strsql = strsql & "'" & getDateFormatInInsert(dtDocDate.Value) & "', "
            strsql = strsql & "'" & getDateFormatInInsert(dtDocDate.Value) & "', "
            strsql = strsql & "'')"
            lngAccID = getMaxInsertID(strsql, strDocHdrTblName, svtTrn)
            If lngAccID = -1 Then
                UpdtVchHdr = False
                Exit Function
            End If
        End If
        If insMode = "M" Then
            lngAccID = intAccID
        End If

        With vsfDet
            For i = 1 To .Rows.Count - 1
                If Trim(.GetData(i, vDetAcLedName)) <> "" Then
                    'add to detail file
                    strsql = "insert into trnDtlAcc (AccID,AcLedID,DrAmt, CrAmt) values ("
                    strsql = strsql & lngAccID & ","
                    strsql = strsql & Val(.GetData(i, vDetAcLedID)) & ","
                    strsql = strsql & Val(.GetData(i, vDetDebit)) & ","
                    strsql = strsql & Val(.GetData(i, vDetCredit)) & ")"
                    If Not clsSVT.execSQLStatement(strsql, svtTrn) Then
                        UpdtVchHdr = False
                        Exit Function
                    End If
                End If
            Next i
        End With

        'for adjusted bills
        With vsfAdj
            For i = 1 To .Rows.Count - 1
                If Val(.GetData(i, vAdjAccLedID)) > 0 Then
                    'add to detail file
                    strsql = "Insert into TrnDtlAccAdj (AccID,AcLedID,DrAmt,CrAmt,AdjAgnstAccID,AdjType,NewRefNo) values ("
                    strsql = strsql & lngAccID & ","
                    strsql = strsql & Val(.GetData(i, vAdjAccLedID)) & ","
                    If .GetData(i, vAdjDrCr) = "Dr" Then
                        strsql = strsql & Val(.GetData(i, vAdjAdjAmt)) & ","
                        strsql = strsql & "0,"
                    Else
                        strsql = strsql & "0,"
                        strsql = strsql & Val(.GetData(i, vAdjAdjAmt)) & ","
                    End If
                    strsql = strsql & Val(.GetData(i, vAdjAccID)) & ","
                    strsql = strsql & " '" & .GetData(i, vAdjType) & "',"
                    strsql = strsql & "'" & .GetData(i, vAdjNewRefNo) & "')"
                    If Not clsSVT.execSQLStatement(strsql, svtTrn) Then
                        UpdtVchHdr = False
                        Exit Function
                    End If
                End If
            Next i
        End With
        UpdtVchHdr = True
    End Function

    Private Function GetDetSqlString(Optional argDocID As Integer = 0) As String
        Dim strFlds1 = ""
        strValues = ""
        Dim strColFlds = ""
        Dim strColValues = ""

        strFlds = ""
        strValues = ""
        Call GetvsfOptHdrDetInsertString(Me, vsfDet, vsfOptions, intDetCols, 1, strColFlds, strColValues, svtTrn)
        strFlds = "Insert Into  " & strDocDtlTblName

        strFlds = strFlds & " (" & strDocIDFldName & " , slNo,AcLedID,"
        strFlds = strFlds & strDocAmtFldName & " , CrAmt, DrAmt, AmtType,"
        strFlds = strFlds & " AgRefAmt, NewRefAmt"

        For i = 1 To vsfDet.Rows.Count - 1
            If argDocID > 0 AndAlso Mode = "M" Then
                strValues = strValues & " Select  " & argDocID & ", "
            Else
                strValues = strValues & " Select  @MaxID, "
            End If

            With vsfDet
                strValues = strValues & i & ","
                strValues = strValues & Val(.GetData(i, vDetAcLedID)) & ","
                strValues = strValues & Val(.GetData(i, vDetCredit)) + Val(.GetData(i, vDetDebit)) & ","
                strValues = strValues & Val(.GetData(i, vDetCredit)) & ","
                strValues = strValues & Val(.GetData(i, vDetDebit)) & ","
                strValues = strValues & "'" & Trim(.GetData(i, vDetAmtType)) & "',"
                strValues = strValues & Val(.GetData(i, vDetAgRefAmt)) & ","
                strValues = strValues & Val(.GetData(i, vDetNewRefAmt)) & ","
            End With

            Call GetvsfOptHdrDetInsertString(Me, vsfDet, vsfOptions, intDetCols, i, strFlds1, strValues, svtTrn)
            strValues = Mid(strValues, 1, Len(strValues) - 1)
            strValues = strValues & " Union All"
        Next

        strValues = Mid(strValues, 1, Len(strValues) - 9)

        GetDetSqlString = strFlds & strColFlds & " " & strValues
    End Function

    Private Sub btnAttachClose_Click(sender As Object, e As System.EventArgs) Handles btnAttachClose.Click
        fraAttach.Visible = False
        Call refreshAttachBtn()
    End Sub

    Private Sub vsfTot_EnterCell(sender As Object, e As System.EventArgs) Handles vsfTot.EnterCell
        If Mode = "V" Or Mode = "" Then Exit Sub

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
    End Sub

    Public Sub vsfHdr_AfterEdit(sender As Object, e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfHdr.AfterEdit
        Call vsfGenAfterEditBfrCalTot(Me, sender, e.Row, "M")
        If e.Row = intRowVchType Then Call UpdatePFX()   '----------changed--------'
        If blnCalc = False Then
            Call CalTotal()
        End If
        Call vsfGenAfterEditAftrCalTot(Me, sender, intDocID, Mode, e.Row, False)
    End Sub

    Public Sub vsfHdr_BeforeEdit(sender As Object, e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfHdr.BeforeEdit
        '-------changed-----'
        If e.Row = intRowVchType Then
            vsfHdr.ComboList = ""
            vsfHdr.ComboList = strVchType
            Exit Sub
        End If
        '---------------------'
        If vsfBeforeEditCancel(Me, sender, intDocID, strDocHdrTblName, Mode, e.Row, e.Col, "M", "") Then
            e.Cancel = True
            Exit Sub
        End If
    End Sub

    Public Sub vsfHdr_EnterCell(sender As Object, e As System.EventArgs) Handles vsfHdr.EnterCell
        If Mode = "V" Or Mode = "" Then Exit Sub

        With sender
            If .row < 0 Then Exit Sub
            If GetVal(.GetData(.Row, vOthFldTxtBlnMultiLine), "") = "Y" Then Exit Sub
            'If .GetData(.Row, vOthFldTxtBlnMultiLine) = "Y" Then Exit Sub
            If .GetData(.Row, vOthFldBlnComboData) = "Y" Then Exit Sub
            If .GetData(.Row, vOthFldDataType) = "List String" Then Exit Sub
            If .GetData(.Row, vOthFldBlnReadOnly) = "Y" Then Exit Sub
            If .GetData(.Row, vOthFldBlnRefill) = "Y" Then Exit Sub
            Try
                '.StartEditing()
                Exit Sub
            Catch ex As Exception
                MsgBox(ex.Message, MsgBoxStyle.Critical, Me.Text)
            End Try
        End With
    End Sub

    Public Sub vsfHdr_KeyPressEdit(sender As Object, e As C1.Win.C1FlexGrid.KeyPressEditEventArgs) Handles vsfHdr.KeyPressEdit
        If e.KeyChar = "'" Then
            e.Handled = True
            Exit Sub
        End If
        If vsfKeyPressEdit(Me, sender, sender.Row, e.KeyChar) Then
            e.Handled = False
        Else
            e.Handled = True
        End If
    End Sub

    Public Sub vsfHdr_KeyUp(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles vsfHdr.KeyUp
        If e.KeyCode = Keys.Escape Then
            If vsfDet.Enabled Then vsfDet.Focus()
            Exit Sub
        End If
        If e.KeyCode = Keys.Tab Then
            If vsfHdr.Row = vsfHdr.Rows.Count - 1 Then
                vsfDet.Enabled = True
                vsfDet.Focus()
            End If
        End If
        If e.Control = True AndAlso e.KeyCode = Keys.G Then
            Call GridFocusChange("H")
        End If
    End Sub

    Public Sub vsfHdr_LostFocus(sender As Object, e As System.EventArgs) Handles vsfHdr.LostFocus
        pnlToolTip.Visible = False
    End Sub

    Public Sub vsfHdr_RowColChange(sender As Object, e As System.EventArgs) Handles vsfHdr.RowColChange
        'If Not (Mode = "A" Or Mode = "M") Then Exit Sub
        'lblToolTip.Text = ""
        'lblToolTip.Text = vsfRowColChange(Me, vsfHdr)
        'If lblToolTip.Text <> "" Then
        '    pnlToolTip.Visible = True
        'Else
        '    pnlToolTip.Visible = False
        'End If
        setToolTipMsg(sender, lblHdrTip, sender.Row)
    End Sub

    Public Sub vsfHdr_SetupEditor(sender As Object, e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfHdr.SetupEditor
        Call vsfSetupEditor(Me, sender, intDocID, strDocHdrTblName, Mode, e.Row, e.Col, "M")
    End Sub

    Public Sub vsfHdr_ValidateEdit(sender As Object, e As C1.Win.C1FlexGrid.ValidateEditEventArgs) Handles vsfHdr.ValidateEdit
        If Not vsfValidateEdit(Me, sender, e.Row, e.Col, "T", strDocHdrTblName, strDocNoFldName, txtDocNo.Text) Then
            e.Cancel = True
        End If
    End Sub

    Public Sub vsfFtr_AfterEdit(sender As Object, e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfFtr.AfterEdit
        Call vsfGenAfterEditBfrCalTot(Me, vsfFtr, e.Row, "M")
        If blnCalc = False Then
            Call CalTotal()
        End If
        Call vsfGenAfterEditAftrCalTot(Me, vsfFtr, intDocID, Mode, e.Row, False)
    End Sub

    Public Sub vsfFtr_BeforeEdit(sender As Object, e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfFtr.BeforeEdit
        If vsfBeforeEditCancel(Me, vsfFtr, intDocID, strDocHdrTblName, Mode, e.Row, e.Col, "M", "") Then
            e.Cancel = True
            Exit Sub
        End If
    End Sub

    Public Sub vsfFtr_EnterCell(sender As Object, e As System.EventArgs) Handles vsfFtr.EnterCell
        If Mode = "V" Or Mode = "" Then Exit Sub
        With sender
            If .row < 0 Then Exit Sub
            If GetVal(.GetData(.Row, vOthFldTxtBlnMultiLine), "") = "Y" Then Exit Sub

            If .GetData(.Row, vOthFldBlnComboData) = "Y" Then Exit Sub
            If .GetData(.Row, vOthFldDataType) = "List String" Then Exit Sub
            If .GetData(.Row, vOthFldBlnReadOnly) = "Y" Then Exit Sub
            If .GetData(.Row, vOthFldBlnRefill) = "Y" Then Exit Sub
            Try
                'infinite loop---- satish
                '.StartEditing()
                Exit Sub
            Catch ex As Exception
                MsgBox(ex.Message, MsgBoxStyle.Critical, Me.Text)
            End Try
        End With
    End Sub

    Public Sub vsfFtr_KeyPressEdit(sender As Object, e As C1.Win.C1FlexGrid.KeyPressEditEventArgs) Handles vsfFtr.KeyPressEdit
        If e.KeyChar = "'" Then
            e.Handled = True
            Exit Sub
        End If
        If vsfKeyPressEdit(Me, sender, sender.Row, e.KeyChar) Then
            e.Handled = False
        Else
            e.Handled = True
        End If
    End Sub

    Public Sub vsfFtr_KeyUp(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles vsfFtr.KeyUp
        If e.KeyCode = Keys.Tab Then
            If vsfFtr.Row = vsfFtr.Rows.Count - 1 Then
                If vsfTot.Visible = True Then
                    vsfTot.Focus()
                Else
                    svtBtnCtrl.Focus()
                End If
            End If
        End If
        If e.Control = True AndAlso e.KeyCode = Keys.G Then
            Call GridFocusChange("F")
        End If
    End Sub

    Public Sub vsfFtr_LostFocus(sender As Object, e As System.EventArgs) Handles vsfFtr.LostFocus
        pnlToolTip.Visible = False
    End Sub

    Public Sub vsfFtr_RowColChange(sender As Object, e As System.EventArgs) Handles vsfFtr.RowColChange
        'If Not (Mode = "A" Or Mode = "M") Then Exit Sub
        ''lblToolTip.Text = ""
        ''lblToolTip.Text = vsfRowColChange(Me, vsfFtr)
        ''If lblToolTip.Text <> "" Then
        ''    pnlToolTip.Visible = True
        ''Else
        '    pnlToolTip.Visible = False
        'End If
        setToolTipMsg(sender, lblFtrTip, sender.Row)
    End Sub

    Public Sub vsfFtr_SetupEditor(sender As Object, e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfFtr.SetupEditor
        Call vsfSetupEditor(Me, sender, intDocID, strDocHdrTblName, Mode, e.Row, e.Col, "M")
    End Sub

    Public Sub vsfFtr_ValidateEdit(sender As Object, e As C1.Win.C1FlexGrid.ValidateEditEventArgs) Handles vsfFtr.ValidateEdit
        If Not vsfValidateEdit(Me, sender, e.Row, e.Col, "M", strDocHdrTblName, strDocNoFldName, txtDocNo.Text) Then
            e.Cancel = True
        End If
    End Sub

    Private Sub NextFocus()
        If vsfDet.Visible Then
            If vsfDet.Rows.Count > 1 Then
                vsfDet.Row = 1

                If intDetNextEntryCol > 0 Then
                    vsfDet.Col = intDetNextEntryCol
                End If
                vsfDet.Focus()
            End If
        Else
            If vsfHdr.Visible Then
                vsfHdr.Focus()
                Exit Sub
            End If

            If vsfFtr.Visible Then
                vsfFtr.Focus()
                Exit Sub
            End If

            If vsfTot.Visible Then
                vsfTot.Focus()
                Exit Sub
            End If
        End If
        'If intItemDocID > 0 Then
        '    If vsfDet.Rows.Count > 1 Then
        '        vsfDet.Row = 1
        '        vsfDet.Col = vDetItemCode
        '        vsfDet.Focus()
        '    End If
        'Else
        '    vsfDet.Row = 1
        '    vsfDet.Col = intDetNextEntryCol
        '    vsfDet.Focus()
        'End If
    End Sub

    Private Sub srcTransaction_TxtChanged(argValue As String) Handles srcTransaction.TxtChanged
        If argValue = "" Then
            srcTransaction.vsfAnchorReSize(410, 30)
            Exit Sub
        End If
        srcTransaction.LoadGrid(argValue, "", getSrcQry(argValue, True))
        srcTransaction.vsfAnchorReSize(655, 480)
    End Sub

    Private Function getSrcQry(argValue As String, ByVal argCond As Boolean) As String
        Dim strSQL As String = ""
        strSQL = "Select Top " & gIntTopNrows & "  h." & strDocIDFldName & "," & strDocNoFldName & "," & strSHForm & "Date "
        strSQL &= " From "
        strSQL &= strDocHdrTblName & " h where 1 = 1 "
        If argCond = True Then
            strSQL &= " And ("
            strSQL &= strDocNoFldName & " Like '%" & argValue & "%'"
            If gDBTypeCurrent = "ACCESS" Then
                strSQL &= " or Cstr(" & strSHForm & "Date)" & " like '%" & argValue & "%'"
            Else
                strSQL &= " or Convert(varchar(25)," & strSHForm & "Date,103)" & " like '%" & argValue & "%'"
            End If
            strSQL &= ")"
        End If
        strSQL &= " order by 1,2"
        Return strSQL
    End Function

    Private Sub srcTransaction_TxtKeyUp(argValue As Integer) Handles srcTransaction.TxtKeyUp
        If argValue > 0 Then
            srcTransaction.fillGrid(argValue, "")
            FillDocDetails(srcTransaction.getId())
            srcTransaction.vsfAnchorReSize(410, 30)
        Else
            srcTransaction.LoadGrid("", "", getSrcQry("", False))
            srcTransaction.vsfAnchorReSize(655, 480)
        End If
    End Sub

    Private Sub srcTransaction_vsfDoubleClick(argValue As Integer) Handles srcTransaction.vsfDoubleClick
        Call srcTransaction.fillGrid(argValue, "")
        FillDocDetails(srcTransaction.getId())
        srcTransaction.vsfAnchorReSize(410, 30)
        Call NextFocus()
    End Sub

    Private Sub srcTransaction_vsfKeyUp(argValue As Integer) Handles srcTransaction.vsfKeyUp
        Call srcTransaction.fillGrid(argValue, "")
        FillDocDetails(srcTransaction.getId())
        srcTransaction.vsfAnchorReSize(410, 30)
        Call NextFocus()
    End Sub

    Private Sub ucBtnTools_btnClicked(btnValue As Integer) Handles ucBtnTools.btnClicked
        Dim intLinkedDocID As Integer = 0
        Dim strLinkedToDocNo As String = ""
        Select Case btnValue
            Case 9, 10, 11, 12, 13, 16, 17, 18, 19, 20, 21
            Case Else
                'If gridEmpty() Then Exit Sub
        End Select
        Select Case btnValue
            Case 8, 10, 11, 15, 20
            Case Else
                If Not (Mode = "A" Or Mode = "M") Then Exit Sub
        End Select
        Try
            vsfDet.FinishEditing()
            vsfDet.Enabled = False
            With ucBtnTools
                Select Case btnValue
                    Case svtDtlBtns.svtInsert
                        Call ucBtnTools.vsfInsert(vsfDet, vsfDet.Row, vDetAmtType)
                        'If intItemDocID > 0 Then
                        '    If Trim(vsfDet.GetData(vsfDet.Row, vDetAcLedID)) = "" Then
                        '        vsfDet.Enabled = True
                        '        Exit Sub
                        '    Else
                        '        Call .vsfInsert(vsfDet, vsfDet.Row, vDetAcLedID)
                        '    End If
                        '    If Val(vsfDet.GetData(vsfDet.Row, vDetAcLedID)) <= 0 Then
                        '        vsfDet.SetData(vsfDet.Row, vDetItemSlNo, getmaxNo())
                        '    End If
                        'Else
                        'vsfDet.SetData(vsfDet.Row, vDetItemSlNo, getmaxNo())
                        ' Call .vsfInsert(vsfDet, vsfDet.Row, vDetSlNo + 1)
                        'End If
                        'Call fillVsfDetDefValues(vsfDet, vsfOptions, vsfDet.Row)
                    Case svtDtlBtns.svtRemove
                        If blnLinkedDocFrom And Mode = "M" Then
                            strLinkedToDocNo = clsSVT.getDataStringValue("Select Top 1 " & strLinkedToDocNoFldName & " from " & strLinkedTODtlTblName & " d, " & strLinkedTOHdrTblName & " h" &
                                    " where h.Cancelled = 'N' and h." & strLinkedTODocFldIDName & " = d." & strLinkedTODocFldIDName &
                                    " and " & strDocIDFldName & " = " & txtDocID.Text, "", False)
                            If strLinkedToDocNo <> "" Then
                                MsgBox(strSHForm & "No already exist in " & strLinkedToDocNoFldName & " : '" & strLinkedToDocNo & "'", MsgBoxStyle.Critical, Me.Text)
                                vsfDet.Enabled = True
                                Exit Sub
                            End If
                        End If
                        'If Mode = "M" AndAlso Val(vsfDet.GetData(vsfDet.Row, vDetDocCumQty)) > 0 Then
                        '    MsgBox("Already exist in Linked document. Deletion not possible.", MsgBoxStyle.Critical, Me.Text)
                        '    vsfDet.Enabled = True
                        '    Exit Sub
                        'End If
                        'Call .vsfRemove(vsfDet, vsfDet.Row, vDetItemCode)
                    Case svtDtlBtns.svtCopy
                        'Call .vsfCopy(vsfDet, vsfDet.Row, vDetItemCode)
                    Case svtDtlBtns.svtPaste
                        'If intItemDocID > 0 Then
                        '    Call .vsfPaste(vsfDet, vsfDet.Row, vDetItemCode)
                        '    vsfDet.SetData(vsfDet.Row, vDetItemSlNo, getmaxNo())
                        'Else
                        '    Call .vsfPaste(vsfDet, vsfDet.Row, vDetSlNo + 1)
                        'End If
                    Case svtDtlBtns.svtUp
                        'Call MoveOneRowUp(vsfDet, vDetSlNo)
                        vsfDet.Enabled = True
                    Case svtDtlBtns.svtDown
                        'Call MoveOneRowDown(vsfDet, vDetSlNo)
                    Case svtDtlBtns.svtReplace
                        'Call .vsfReplace(vsfDet, vsfDet.Row, vsfDet.Col, vDetItemCode)
                    Case svtDtlBtns.svtExport
                        Call .vsfExport(vsfDet)
                    Case svtDtlBtns.svtImport
                        'vsfDet.Col = vDetItemName
                        'If .vsfImport(vsfDet, vDetItemCode, vsfOptions, intDetCols) Then
                        '    With vsfDet
                        '        For i As Int32 = 1 To .Rows.Count - 1
                        '            If Not FillItemDet(.GetData(i, vDetItemCode), i, i) Then
                        '                vsfDet.Rows.Count = 1
                        '                vsfDet.Rows.Count = 2
                        '                vsfDet.Enabled = True
                        '                Exit Sub
                        '            End If
                        '        Next
                        '        MsgBox("XL imported into Grid...", MsgBoxStyle.Information, Me.Text)
                        '    End With
                        'End If
                    Case svtDtlBtns.svtTemplate
                        Dim strPath As String, strFileName As String
                        strPath = Application.StartupPath() & "\Temp\"
                        strFileName = Date.Now.ToString("yyyyMMddHHmmss") & "_" & Trim(txtDocNo.Text)
                        Call .vsfTemplate(vsfDet, strPath & strFileName, vsfOptions)
                    Case svtDtlBtns.svtAttach
                        fraAttach.Visible = True
                    Case svtDtlBtns.svtRestore
                        blnRestore = False
                        Dim obj As New frmUtlRestore
                        obj.strDocument = Me.Text
                        obj.frm = Me
                        obj.blnDetSection = blnDetSection
                        obj.ShowDialog()
                        obj.Dispose()
                        obj.Close()
                        If blnRestore = False Then
                            RestoreData()
                        End If
                    Case svtDtlBtns.svtCopyFrom
                        Dim obj As New frmUtlCopyFrom
                        With obj
                            .argForm = Me
                            .strFrmCaption = strFrmCaption
                            .argvsfOptions = vsfOptions
                            .argvsf = vsfDet
                            .ShowDialog()
                        End With
                    Case svtDtlBtns.svtFreeze
                        Call .vsfFreeze(vsfDet, vsfDet.Col)
                    Case svtDtlBtns.svtAddItems
                        'If intItemDocID > 0 Then
                        '    vsfDet.Col = vDetItemName
                        'Else
                        '    vsfDet.Col = vDetItemSlNo + 1
                        'End If
                        'Dim obj As New frmUtlAddItems
                        'obj.frm = Me
                        'obj.argItemCode = vDetItemCode
                        'obj.ShowDialog()
                        'obj.Dispose()
                        'obj.Close()
                    Case svtDtlBtns.svtTender
                        'Call .vsfTenderChange(intDocID, Val(txtDocID.Text), Trim(txtDocNo.Text), Val(vsfTot.GetData(intRowTenderFld, vOthFldDataValue)))
                    Case svtDtlBtns.svtMultiDoc
                        Call .vsfMultDoc(intDocID, Me, Trim(txtDocNo.Text))
                    Case svtDtlBtns.svtBOM
                        'Call .vsfBOM(Me, vsfDet.GetData(vsfDet.Row, vDetItemID), vsfDet.Row, vsfDet.GetData(vsfDet.Row, intColQty))
                    Case svtDtlBtns.svtCalTotal
                        Call CalTotal()
                    Case svtDtlBtns.svtPending

                    Case svtDtlBtns.svtExport
                        Call .vsfExport(vsfDet)
                    Case svtDtlBtns.svtTrnExport
                        If gBlnMSOffice Then
                            Call .DocXlExp(Me, strSHForm, tbAdnlFlds, vsfDet, tbAdnlFldsFtr, vsfTot, intDocID, txtDocNo.Text, dtDocDate.Value, cboPfx.Text, "")
                        Else
                            Call .DocXlExp_ClosedXML(Me, strSHForm, tbAdnlFlds, vsfDet, tbAdnlFldsFtr, vsfTot, intDocID, txtDocNo.Text, dtDocDate.Value, cboPfx.Text, "", "")
                        End If

                End Select
                vsfDet.Enabled = True
            End With
            'ReArrangeSlNo(vsfDet, vDetSlNo)
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information, Me.Text)
        End Try
    End Sub

    Private Sub TimerStartEvent()
        blnTimer = True
        strRestoreFileName = "Doc_No_" & Trim(txtDocNo.Text) & "_" & Date.Now.ToString("yyyyMMddHHmmss")
        strRestoreFileName = strRestoreFileName.Replace(" ", "_")

        Try
            strRestorePath = System.Windows.Forms.Application.StartupPath & "\AutoSave\" & Me.Text & "\"
            If Not Directory.Exists(strRestorePath) Then
                Directory.CreateDirectory(strRestorePath)
            End If
        Catch ex As Exception

        End Try
        Timer1.Interval = 10000
        Timer1.Start()
    End Sub

    'Private Sub btnCopyFrom_Click(sender As Object, e As EventArgs) Handles btnCopyFrom.Click
    '    Dim frmUS As New frmUtlSingleSel
    '    Dim strsql As String

    '    'txtToLoadId.Text = ""

    '    If intPartyDocID > 0 Then
    '        strsql = "select h.*, " & strFldPartyCode & " from " & strDocHdrTblName & " h, " & strPartyTblName & " p " _
    '                                            & " where h." & strFldPartyID & " = p." & strFldPartyID & " order by " & strDocDateFldName
    '    Else
    '        strsql = "select * from " & strDocHdrTblName & " order by " & strDocDateFldName
    '    End If

    '    With frmUS
    '        .Text = "Select from... "
    '        Call .FillViewVsf(Me, strsql, txtToLoadId, 0, 0)
    '        .ShowDialog()
    '        .vsfDet.Focus()
    '    End With

    '    If Val(txtToLoadId.Text) > 0 Then
    '        Call FillDocDetails(Val(txtToLoadId.Text))
    '        txtDocID.Text = ""
    '        dtDocDate.Value = getSystemDate()
    '        Mode = "A"
    '        Call FillAutoGenTrnNo()
    '    End If
    'End Sub

    Private Sub btnCloseSearch_Click(sender As Object, e As EventArgs)
        'fraAddress.Visible = False
    End Sub

    Private Sub cboShpAddress_SelectedIndexChanged(sender As Object, e As EventArgs)

    End Sub

    Private Sub Timer1_Tick(sender As System.Object, e As System.EventArgs) Handles Timer1.Tick
        If blnTimer = True And strRestoreFileName <> "" Then
            blnTimer = False
            AutoSaveGrid_Update()
        End If
    End Sub

    Private Sub AutoSaveGrid_Update()
        If Not vsfDet.Visible Then
            blnTimer = True
            Exit Sub
        End If
        With vsfDet
            Try
                .WriteXml(strRestorePath & strRestoreFileName & ".xml")
            Catch ex As Exception
                MsgBox(ex.Message, MsgBoxStyle.Exclamation, Me.Text)
            End Try
        End With
        blnTimer = True
    End Sub

    Private Sub btnCloseAddress_Click(sender As Object, e As EventArgs)
        'fraAddress.Visible = False
    End Sub

    Private Sub lblShipAddress_Click(sender As Object, e As EventArgs)
        'fraAddress.Visible = True
    End Sub

    Private Sub RestoreData()
        Dim strFileName As String = strRestoreFileName
        RestoreData_Grid(vsfDet, strFileName)
        strRestoreFileName = "Doc_No_" & Trim(txtDocNo.Text) & "_" & Date.Now.ToString("yyyyMMddHHmmss")
        strRestoreFileName = strRestoreFileName.Replace(" ", "_")
    End Sub

    Private Sub RestoreData_Grid(ByVal argVsf As C1FlexGrid, ByVal argPath As String)
        With argVsf
            Try
                If Not File.Exists(strRestorePath & argPath) Then Exit Sub
                .ReadXml(strRestorePath & argPath)
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
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Exclamation, Me.Text)
        End Try
    End Sub

    Private Sub raiseShortCutEvent(ByVal e As System.Windows.Forms.KeyEventArgs)
        If blnDetSection = False Then Exit Sub
        Try
            If Mode = "A" Or Mode = "M" Then

                'Insert Row event
                If e.Control = True And e.Shift = True And e.KeyCode = Keys.I Then
                    Call ucBtnTools.vsfInsert(vsfDet, vsfDet.Row, vDetAmtType)
                    'ReArrangeSlNo(vsfDet, vDetSlNo)
                End If

                ''Remove Row event
                'If e.Control = True And e.Shift = True And e.KeyCode = Keys.R Then
                '    Call ucBtnTools.vsfRemove(vsfDet, vsfDet.Row, vDetItemCode)
                '    ReArrangeSlNo(vsfDet, vDetSlNo)
                'End If

                ''Copy  event
                'If e.Control = True And e.Shift = True And e.KeyCode = Keys.C Then
                '    Call ucBtnTools.vsfCopy(vsfDet, vsfDet.Row, vDetItemCode)
                'End If
                ''Paste Event
                'If e.Control = True And e.Shift = True And e.KeyCode = Keys.P Then
                '    Call ucBtnTools.vsfPaste(vsfDet, vsfDet.Row, vDetItemCode)
                '    ReArrangeSlNo(vsfDet, vDetSlNo)
                'End If

                ''MoveUp Event
                'If e.Control = True And e.Shift = True And e.KeyCode = Keys.U Then
                '    Call MoveOneRowUp(vsfDet, vDetSlNo)
                'End If

                ''MoveDown Event
                'If e.Control = True And e.Shift = True And e.KeyCode = Keys.V Then
                '    Call MoveOneRowDown(vsfDet, vDetSlNo)
                'End If

                ''RePlace Rest Event
                'If e.Control = True And e.Shift = True And e.KeyCode = Keys.S Then
                '    Call ucBtnTools.vsfReplace(vsfDet, vsfDet.Row, vsfDet.Col, vDetItemCode)
                'End If

                'Exprot to Excel
                If e.Control = True And e.Shift = True And e.KeyCode = Keys.E Then
                    Call ucBtnTools.vsfExport(vsfDet)
                End If

                ''Import from Excel
                'If e.Control = True And e.Shift = True And e.KeyCode = Keys.F Then
                '    If ucBtnTools.vsfImport(vsfDet, vDetItemCode, vsfOptions, intDetCols) Then
                '        With vsfDet
                '            For i As Int32 = 1 To .Rows.Count - 1
                '                If Not FillItemDet(.GetData(i, vDetItemCode), i, i) Then
                '                    vsfDet.Rows.Count = 1
                '                    vsfDet.Rows.Count = 2
                '                    Exit Sub
                '                End If
                '            Next
                '            MsgBox("XL imported into Grid...", MsgBoxStyle.Information, Me.Text)
                '        End With
                '    End If
                'End If


                'Template
                If e.Control = True And e.Shift = True And e.KeyCode = Keys.T Then
                    Dim strPath As String, strFileName As String
                    strPath = Application.StartupPath() & "\Temp\"
                    strFileName = Date.Now.ToString("yyyyMMddHHmmss") & "_" & Trim(txtDocNo.Text)
                    Call ucBtnTools.vsfTemplate(vsfDet, strPath & strFileName, vsfOptions)
                End If

                'Attachment
                If e.Control = True And e.Shift = True And e.KeyCode = Keys.A Then
                    fraAttach.Visible = True
                End If

                'Restore
                If e.Control = True And e.Shift = True And e.KeyCode = Keys.O Then
                    blnRestore = False
                    Dim obj As New frmUtlRestore
                    obj.strDocument = Me.Text
                    obj.frm = Me
                    obj.ShowDialog()
                    obj.Dispose()
                    obj.Close()
                    If blnRestore = False Then
                        RestoreData()
                    End If
                End If

                'Copy From
                If e.Control = True And e.Shift = True And e.KeyCode = Keys.F Then
                    Dim obj As New frmUtlCopyFrom
                    With obj
                        .argForm = Me
                        .strFrmCaption = strFrmCaption
                        .argvsfOptions = vsfOptions
                        .argvsf = vsfDet
                        .ShowDialog()
                    End With
                End If

                'Freeze
                If e.Control = True And e.Shift = True And e.KeyCode = Keys.Z Then
                    Call ucBtnTools.vsfFreeze(vsfDet, vsfDet.Col)
                End If

                'Add List of Items
                If e.Control = True And e.Shift = True And e.KeyCode = Keys.Q Then
                    Dim obj As New frmUtlAddItems
                    obj.frm = Me
                    obj.ShowDialog()
                    obj.Dispose()
                    obj.Close()
                End If

                'Tender Change
                If e.Control = True And e.Shift = True And e.KeyCode = Keys.Y Then
                    'Dim obj As New frmTrnTender
                    '' obj.frm = Me
                    'obj.txtInvId.Text = txtDocID.Text
                    'obj.ShowDialog()
                    '' obj.strSHForm = strSHForm
                    'obj.Dispose()
                    'obj.Close()
                    'Call ucBtnTools.vsfTenderChange(intDocID, Val(txtDocID.Text), Trim(txtDocNo.Text), Val(vsfTot.GetData(intRowTenderFld, vOthFldDataValue)))
                End If

                'Insert Doc
                If e.Control = True And e.Shift = True And e.KeyCode = Keys.B Then
                    'Call ucBtnTools.vsfInsertDoc(intDocID, Me)
                End If

                'Multi Doc Generation
                If e.Control = True And e.Shift = True And e.KeyCode = Keys.G Then
                    Call ucBtnTools.vsfMultDoc(intDocID, Me, Trim(txtDocNo.Text))
                End If
            End If

            'Max size Detail grid
            If e.Control = True And e.Shift = True And e.KeyCode = Keys.M Then
                Call reSizeVSF("D")
            End If


            'Focus on Detail grid
            'If e.Control = True And e.Shift = True And e.KeyCode = Keys.D Then
            '    vsfDet.Row = 1
            '    vsfDet.Col = vDetItemCode
            '    vsfDet.Focus()
            'End If

            'Focus on Party Detal

            'If e.Control = True And e.KeyCode = Keys.P Then
            '    svtPartyCtrl1.Focus()
            'End If

            'Focus on Header Grid
            If e.Control = True And e.Shift = True And e.KeyCode = Keys.H Then
                vsfHdr.Row = 0
                vsfHdr.Col = 5
                vsfHdr.Focus()
            End If

            'focuse on Total Grid
            If e.Control = True And e.Shift = True And e.KeyCode = Keys.K Then
                vsfTot.Row = 0
                vsfTot.Col = 5
                vsfTot.Focus()
            End If
            'focuse on Footer Grid
            If e.Control = True And e.Shift = True And e.KeyCode = Keys.W Then
                vsfFtr.Row = 0
                vsfFtr.Col = 5
                vsfFtr.Focus()
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

            'Doc XL Export
            If e.Control = True And e.Shift = True And e.KeyCode = Keys.X Then
                If gBlnMSOffice Then
                    Call ucBtnTools.DocXlExp(Me, strSHForm, tbAdnlFlds, vsfDet, tbAdnlFldsFtr, vsfTot, intDocID, txtDocNo.Text, dtDocDate.Value, cboPfx.Text, "")
                Else
                    Call ucBtnTools.DocXlExp_ClosedXML(Me, strSHForm, tbAdnlFlds, vsfDet, tbAdnlFldsFtr, vsfTot, intDocID, txtDocNo.Text, dtDocDate.Value, cboPfx.Text, "")

                End If
            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information, Me.Text)
        End Try
    End Sub

    Private Sub getCtrlSize()
        'intVsfSize = vsfDet.Size.Width
        'intVsfSize2 = vsfDet.Size.Height
        'intVsfPos = vsfDet.Location.X
        'intVsfPos2 = vsfDet.Location.Y

        'intVHdrSize = tbAdnlFlds.Size.Width
        'intVHdrSize2 = tbAdnlFlds.Size.Height
        'intVHdrPos = tbAdnlFlds.Location.X
        'intVHdrPos2 = tbAdnlFlds.Location.Y

        'intVFtrSize = tbAdnlFldsFtr.Size.Width
        'intVFtrSize2 = tbAdnlFldsFtr.Size.Height
        'intVFtrPos = tbAdnlFldsFtr.Location.X
        'intVFtrPos2 = tbAdnlFldsFtr.Location.Y

        'intVTotSize = vsfTot.Size.Width
        'intVTotSize2 = vsfTot.Size.Height
        'intVTotPos = vsfTot.Location.X
        'intVTotPos2 = vsfTot.Location.Y

        'intBtnSize = ucBtnTools.Size.Width
        'intBtnSize2 = ucBtnTools.Size.Height
        'intBtnPos = ucBtnTools.Location.X
        'intBtnPos2 = ucBtnTools.Location.Y

        'intfraSize = fraMain.Size.Width
        'intfraSize2 = fraMain.Size.Height
        'intSvtPos = svtBtnCtrl.Location.X
        'intSvtPos2 = svtBtnCtrl.Location.Y

        'intDPinSize = btnDetPin.Size.Width
        'intDPinSize2 = btnDetPin.Size.Height
        'intDPinPos = btnDetPin.Location.X
        'intDPinPos2 = btnDetPin.Location.Y
    End Sub

    Private Sub pinDisable(ByVal argType As String)
        Select Case argType
            Case "H"
                If blnVHdr Then
                    btnDetPin.Visible = False
                    btnFtrPin.Visible = False
                    btnTotPin.Visible = False
                Else
                    btnDetPin.Visible = True
                    btnFtrPin.Visible = True
                    btnTotPin.Visible = True
                End If
            Case "F"
                If blnVFtr Then
                    btnDetPin.Visible = False
                    btnHdrPin.Visible = False
                    btnTotPin.Visible = False
                Else
                    btnDetPin.Visible = True
                    btnHdrPin.Visible = True
                    btnTotPin.Visible = True
                End If
            Case "T"
                If blnVTot Then
                    btnDetPin.Visible = False
                    btnFtrPin.Visible = False
                    btnHdrPin.Visible = False
                Else
                    btnDetPin.Visible = True
                    btnFtrPin.Visible = True
                    btnHdrPin.Visible = True
                End If
            Case Else
                If blnVsfMax Then
                    btnHdrPin.Visible = False
                    btnFtrPin.Visible = False
                    btnTotPin.Visible = False
                Else
                    btnHdrPin.Visible = True
                    btnFtrPin.Visible = True
                    btnTotPin.Visible = True
                End If
        End Select
        Call GridPinVisible()
    End Sub

    Private Sub reSizeVSF(ByVal argType As String)
        Select Case argType
            Case "H"
                If blnVHdr Then
                    ''vsfDet.Location = New Point(tbAdnlFlds.Location.X, tbAdnlFlds.Location.Y)
                    'vsfDet.Size = New Point(fraMain.Width + tbAdnlFlds.Width + 3, vsfDet.Height)
                    'tbAdnlFlds.Location = New Point(vsfDet.Location.X, vsfHdr.Location.Y)
                    '' tbAdnlFlds.Size = New Point(tbAdnlFlds.Width, fraMain.Height)
                    'tbAdnlFlds.Size = New Point(tbAdnlFlds.Width, vsfHdr.Height + lblHdrTip.Height)
                    'btnHdrPin.BackgroundImage = My.Resources.PinDownArrow
                    'blnVHdr = False
                    tbAdnlFlds.Size = New Point(tbAdnlFlds.Width, tbAdnlFlds.Height - vsfDet.Height - 2)
                    vsfDet.Size = New Point(fraMain.Width + tbAdnlFlds.Width + 4, vsfDet.Height)
                    btnHdrPin.BackgroundImage = My.Resources.PinDownArrow
                    blnVHdr = False
                Else
                    btnHdrPin.BackgroundImage = My.Resources.PinUpArrow
                    If vsfDet.Visible Then
                        vsfDet.Size = New Point(fraMain.Width, vsfDet.Height)
                        'vsfDet.Size = New Point(vsfDet.Width - tbAdnlFlds.Width - 10, vsfDet.Height)
                        ' vsfDet.Location = New Point(tbAdnlFlds.Right + 10, vsfDet.Location.Y)
                        tbAdnlFlds.Size = New Point(tbAdnlFlds.Width, tbAdnlFlds.Height + vsfDet.Height + 2)
                    Else
                        tbAdnlFlds.Size = New Point(tbAdnlFlds.Width, svtBtnCtrl.Top - 35)
                    End If
                    tbAdnlFlds.BringToFront()
                    blnVHdr = True
                End If
            Case "F"
                If blnVFtr Then
                    vsfDet.Location = New Point(tbAdnlFldsFtr.Location.X, tbAdnlFldsFtr.Location.Y)
                    vsfDet.Size = New Point(fraMain.Width + tbAdnlFlds.Width + 3, vsfDet.Height)
                    tbAdnlFldsFtr.Location = New Point(vsfDet.Location.X, vsfTot.Location.Y)
                    tbAdnlFldsFtr.Size = New Point(tbAdnlFldsFtr.Width, vsfTot.Height + lblTotTip.Height)
                    btnFtrPin.BackgroundImage = My.Resources.PinUpArrow
                    blnVFtr = False
                Else
                    tbAdnlFldsFtr.Location = New Point(vsfDet.Location.X, vsfDet.Location.Y)
                    tbAdnlFldsFtr.Size = New Point(tbAdnlFldsFtr.Width, vsfDet.Height + tbAdnlFldsFtr.Height + ucBtnTools.Height + 10)
                    vsfDet.Location = New Point(tbAdnlFldsFtr.Right + 10, vsfDet.Location.Y)
                    vsfDet.Size = New Point(vsfDet.Width - tbAdnlFldsFtr.Width - 10, vsfDet.Height)
                    btnFtrPin.BackgroundImage = My.Resources.PinDownArrow
                    blnVFtr = True
                End If
            Case "T"
                If blnVTot Then
                    vsfDet.Size = New Point(fraMain.Width + tbAdnlFlds.Width + 3, vsfDet.Height)
                    vsfTot.Location = New Point(vsfTot.Location.X, tbAdnlFldsFtr.Location.Y)
                    vsfTot.Size = New Point(vsfTot.Width, tbAdnlFldsFtr.Height)
                    blnVTot = False
                    btnTotPin.BackgroundImage = My.Resources.PinUpArrow
                Else
                    vsfTot.Location = New Point(vsfTot.Location.X, vsfDet.Location.Y)
                    vsfTot.Size = New Point(vsfTot.Width, vsfDet.Height + vsfTot.Height + 5)
                    vsfDet.Size = New Point(vsfTot.Left - 10, vsfDet.Height)
                    blnVTot = True
                    btnTotPin.BackgroundImage = My.Resources.PinDownArrow
                End If
            Case Else
                If blnVsfMax Then
                    vsfDet.Location = New Point(vsfDet.Location.X, fraMain.Location.Y + fraMain.Height + 5)
                    vsfDet.Size = New Point(vsfDet.Width, ucBtnTools.Top - fraMain.Bottom - 10)
                    vsfDet.BringToFront()
                    btnDetPin.Location = New Point(vsfDet.Right - 15, vsfDet.Bottom - 15)
                    btnDetPin.BringToFront()
                    blnVsfMax = False
                    btnDetPin.BackgroundImage = My.Resources.PinUpArrow
                Else
                    vsfDet.Location = New Point(vsfDet.Location.X, fraMain.Location.Y)
                    vsfDet.Size = New Point(vsfDet.Width, fraMain.Height + vsfDet.Height + tbAdnlFldsFtr.Height)
                    vsfDet.BringToFront()
                    btnDetPin.Location = New Point(vsfDet.Right - 15, vsfDet.Bottom - 15)
                    btnDetPin.BringToFront()
                    blnVsfMax = True
                    btnDetPin.BackgroundImage = My.Resources.PinDownArrow
                End If
        End Select
        Call pinDisable(argType)
        'Call frmTrnSO_Resize(Nothing, Nothing)
    End Sub

    Private Sub btnPin_Click(sender As System.Object, e As System.EventArgs) Handles btnHdrPin.Click
        Call reSizeVSF("H")
    End Sub

    Private Sub btnDetPin_Click(sender As System.Object, e As System.EventArgs) Handles btnDetPin.Click
        Call reSizeVSF("D")
    End Sub

    Private Sub btnFtrPin_Click(sender As Object, e As System.EventArgs) Handles btnFtrPin.Click
        Call reSizeVSF("F")

    End Sub

    Private Sub btnTotPin_Click(sender As Object, e As System.EventArgs) Handles btnTotPin.Click
        Call reSizeVSF("T")
    End Sub

    Public Function FillBOMItem(ByVal argItem As String, ByVal argQty As Double, ByVal rowIndex As Int32, Optional ByVal blnRowInsert As Boolean = True) As Boolean
        FillBOMItem = True
    End Function

    Private Sub GridPinVisible()
        If Not vsfHdr.Visible Then
            btnHdrPin.Visible = False
        End If

        If Not vsfFtr.Visible Then
            btnFtrPin.Visible = False
        End If

        If Not vsfTot.Visible Then
            btnTotPin.Visible = False
        End If

        If Not blnDetSection Then
            btnDetPin.Visible = False
        End If
    End Sub

    Private Sub NonPartyItemFormResize()
        'tbAdnlFlds.Size = New Point(intVHdrSize, intVHdrSize2 / 2)
        'fraMain.Size = New Point(intfraSize, intfraSize2 / 2)
        'vsfDet.Location = New Point(intVsfPos, fraMain.Location.Y + (intVHdrSize2 / 2) + 5)
        'vsfDet.Size = New Point(intVsfSize, intVsfSize2 + (intVHdrSize2 / 2))
        'Call getCtrlSize()
    End Sub

    Private Sub SaveThisToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles SaveThisToolStripMenuItem.Click
        Call SaveFormSettings(intDocID, blnVHdr, blnVsfMax, blnVFtr, blnVTot)
    End Sub

    Private Sub vsfTot_LostFocus(sender As Object, e As System.EventArgs) Handles vsfTot.LostFocus
        pnlToolTip.Visible = False
    End Sub

    Private Sub showBtns()
        With svtBtnCtrl
            If Val(txtDocID.Text) > 0 Then
                .btnDelete.Visible = True
                .btnEdit.Visible = True

                If .btnOptions.Items.Count > 0 Then
                    .btnPrint.Visible = True
                End If
            Else
                .btnEdit.Visible = False
                .btnDelete.Visible = False
                .btnPrint.Visible = False
            End If

            If .btnOptions.Items.Count <= 0 Then
                .btnOptions.Visible = False
            End If
        End With
    End Sub


    Private Sub setToolTipMsg(ByVal argVSF As C1FlexGrid, ByVal lbl As Label, ByVal argRow As Int32)
        If argRow < 0 Then Exit Sub
        If argVSF.Cols.Count >= vOthFldToolTip Then
            lbl.Text = GetVal(argVSF.GetData(argRow, vOthFldToolTip), "")
        Else
            lbl.Text = ""
        End If
    End Sub

    Private Sub vsfHdr_GotFocus(sender As Object, e As EventArgs) Handles vsfHdr.GotFocus
        setToolTipMsg(sender, lblHdrTip, sender.Row)
    End Sub

    Private Sub vsfFtr_GotFocus(sender As Object, e As EventArgs) Handles vsfFtr.GotFocus
        setToolTipMsg(sender, lblFtrTip, sender.Row)
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

        'If lblAuthorised.Text = "AUTHORISED" Then
        '    MsgBox("Current record is Authorised. Cannot edit!", vbInformation, Me.Text)
        '    Exit Function
        'End If

        If intNumOfDays > 0 Then
            If DateAdd(DateInterval.Day, intNumOfDays, dtDocDate.Value) > getSystemDate() Then
                MsgBox("Edit limit days crossed... Cannot edit!", vbInformation, Me.Text)
                Exit Function
            End If
        End If
        Return True
    End Function

    Private Sub getIntArrayPrevTran(ByVal argVSF As C1FlexGrid, ByRef argDt As DataTable)
        With argVSF
            If argVSF.Rows.Count < 0 Then Exit Sub
            Dim k As Int32 = 0
            For i As Int32 = 0 To .Rows.Count - 1
                If GetVal(.GetData(i, vOthBlnLoadPrevTrnData), "") = "Y" Then
                    'ReDim argint(k)
                    'argint(k) = i
                    'k = k + 1
                    Dim dtRow As DataRow = argDt.NewRow
                    dtRow("RowColNo") = i
                    argDt.Rows.Add(dtRow)
                End If
            Next
        End With
    End Sub

    Private Sub getIntArrayPrevTran(ByVal argStr As String, ByRef argDt As DataTable, argGridNo As Int32)
        Dim ctrlList = GetAllDsgrVsf(Me, GetType(C1.Win.C1FlexGrid.C1FlexGrid))
        Dim argVSF As C1FlexGrid = Nothing
        For Each ctrl As C1FlexGrid In ctrlList
            If ctrl.Name = argStr Then
                argVSF = ctrl
                Exit For
            End If
        Next
        If IsNothing(argVSF) Then Exit Sub
        With argVSF
            If argVSF.Rows.Count < 0 Then Exit Sub
            Dim k As Int32 = 0
            For i As Int32 = 0 To .Rows.Count - 1
                If GetVal(.GetData(i, vOthBlnLoadPrevTrnData), "") = "Y" Then
                    Dim dtRow As DataRow = argDt.NewRow
                    dtRow("GridNo") = argGridNo
                    dtRow("ColNo") = i
                    argDt.Rows.Add(dtRow)
                End If
            Next
        End With
    End Sub

    Private Sub InsertPrevTrnData(ByVal argVSF As C1FlexGrid, ByRef argint() As Int32, ByVal argPartyID As Int32)
        '========== FOR tOTAL GRID
        With argVSF
            If IsNothing(argint) Then Exit Sub
            Dim strSQL As String = ""
            For i As Int32 = 0 To argint.Count - 1

                If UCase(.GetData(argint.GetValue(i), vOthFldBlnComboData)) = "Y" Then
                    If Trim(.GetData(argint.GetValue(i), vOthFldPrevTranFldName)) = "" Then
                        strSQL = "Select Top 1 " & .GetData(argint.GetValue(i), vOthFldCmbDisplayCols) & " from " & strDocHdrTblName & " h, " & .GetData(argint.GetValue(i), vOthFldCmbTblName) & " v "
                    Else
                        strSQL = "Select Top 1 " & .GetData(argint.GetValue(i), vOthFldPrevTranFldName) & " from " & strDocHdrTblName & " h, " & .GetData(argint.GetValue(i), vOthFldCmbTblName) & " v "
                    End If
                    strSQL &= "  where " & strDocIDFldName & " <> " & Val(txtDocID.Text)
                    strSQL &= " and h." & .GetData(argint.GetValue(i), vOthFldCmbStorageFldName) & " = t." & .GetData(argint.GetValue(i), vOthFldCmbStorageFldName)
                    If .GetData(argint.GetValue(i), vOthBlnLoadPrevTrnParty) = "Y" Then strSQL &= " and h." & strFldPartyID & " = " & argPartyID
                    strSQL &= " and h.cancelled='N' "
                    strSQL &= " Order by h." & strDocIDFldName & " desc"

                    .SetData(argint.GetValue(i), vOthFldDataValue, clsSVT.getDataStringValue(strSQL, "", False))
                Else
                    If Trim(.GetData(argint.GetValue(i), vOthFldPrevTranFldName)) = "" Then
                        strSQL = "Select Top 1 " & .GetData(argint.GetValue(i), vOthFldCode) & " from " & strDocHdrTblName & " where " & strDocIDFldName & " <> " & Val(txtDocID.Text)
                    Else
                        strSQL = "Select Top 1 " & .GetData(argint.GetValue(i), vOthFldPrevTranFldName) & " from " & strDocHdrTblName & " where " & strDocIDFldName & " <> " & Val(txtDocID.Text)
                    End If
                    If .GetData(argint.GetValue(i), vOthBlnLoadPrevTrnParty) = "Y" Then strSQL &= " and " & strFldPartyID & " = " & argPartyID
                    strSQL &= " and cancelled='N' "
                    strSQL &= " Order by " & strDocIDFldName & " desc"

                    Select Case UCase(.GetData(argint.GetValue(i), vOthFldDataType))
                        Case "TEXT", "LIST STRING"
                            .SetData(argint.GetValue(i), vOthFldDataValue, clsSVT.getDataStringValue(strSQL, "", False))
                        Case "NUMERIC", "LIST NUMERIC"
                            .SetData(argint.GetValue(i), vOthFldDataValue, clsSVT.getDataDoubleValue(strSQL, 0, False))
                        Case Else
                            .SetData(argint.GetValue(i), vOthFldDataValue, clsSVT.getDataDateValue(strSQL, Nothing, False))
                    End Select
                End If

            Next
        End With
    End Sub

    Private Sub InsertPrevTrnData(ByVal argStr As String, ByVal argDT As DataTable, ByVal argPartyID As Int32, ByVal argGridNo As Int32)
        'for dynamic grids hdr and ftr
        Dim ctrlList = GetAllDsgrVsf(Me, GetType(C1.Win.C1FlexGrid.C1FlexGrid))
        Dim argVSF As C1FlexGrid = Nothing
        For Each ctrl As C1FlexGrid In Me.ctrlList
            If ctrl.Name = argStr Then
                argVSF = ctrl
                Exit For
            End If
        Next
        If IsNothing(argVSF) Then Exit Sub
        If IsNothing(argDT) Then Exit Sub
        If argDT.Rows.Count <= -1 Then Exit Sub
        With argVSF
            Dim strSQL As String = ""
            For i As Int32 = 0 To argDT.Rows.Count - 1
                If argDT.Rows(i).Item(0) = argGridNo Then
                    If UCase(.GetData(argDT.Rows(i).Item(1), vOthFldBlnComboData)) = "Y" Then
                        If Trim(.GetData(argDT.Rows(i).Item(1), vOthFldPrevTranFldName)) = "" Then
                            strSQL = "Select Top 1 " & .GetData(argDT.Rows(i).Item(1), vOthFldCmbDisplayCols) & " from " & strDocHdrTblName & " h, " & .GetData(argDT.Rows(i).Item(1), vOthFldCmbTblName) & " v "
                        Else
                            strSQL = "Select Top 1 " & .GetData(argDT.Rows(i).Item(1), vOthFldPrevTranFldName) & " from " & strDocHdrTblName & " h, " & .GetData(argDT.Rows(i).Item(1), vOthFldCmbTblName) & " v "
                        End If
                        strSQL &= "  where " & strDocIDFldName & " <> " & Val(txtDocID.Text)
                        strSQL &= " and h." & .GetData(argDT.Rows(i).Item(1), vOthFldCmbStorageFldName) & " = v." & .GetData(argDT.Rows(i).Item(1), vOthFldCmbStorageFldName)
                        If .GetData(argDT.Rows(i).Item(1), vOthBlnLoadPrevTrnParty) = "Y" Then strSQL &= " and h." & strFldPartyID & " = " & argPartyID
                        strSQL &= " and h.cancelled='N' "
                        strSQL &= " Order by h." & strDocIDFldName & " desc"


                        .SetData(argDT.Rows(i).Item(1), vOthFldDataValue, clsSVT.getDataStringValue(strSQL, "", False))
                    Else
                        If Trim(.GetData(argDT.Rows(i).Item(1), vOthFldPrevTranFldName)) = "" Then
                            strSQL = "Select Top 1 " & .GetData(argDT.Rows(i).Item(1), vOthFldCode) & " from " & strDocHdrTblName & " where " & strDocIDFldName & " <> " & Val(txtDocID.Text)
                        Else
                            strSQL = "Select Top 1 " & .GetData(argDT.Rows(i).Item(1), vOthFldPrevTranFldName) & " from " & strDocHdrTblName & " where " & strDocIDFldName & " <> " & Val(txtDocID.Text)
                        End If

                        If .GetData(argDT.Rows(i).Item(1), vOthBlnLoadPrevTrnParty) = "Y" Then strSQL &= " and " & strFldPartyID & " = " & argPartyID
                        strSQL &= " and cancelled='N' "
                        strSQL &= " Order by " & strDocIDFldName & " desc"



                        Select Case UCase(.GetData(argDT.Rows(i).Item(1), vOthFldDataType))
                            Case "TEXT", "LIST STRING"
                                .SetData(argDT.Rows(i).Item(1), vOthFldDataValue, clsSVT.getDataStringValue(strSQL, "", False))
                            Case "NUMERIC", "LIST NUMERIC"
                                .SetData(argDT.Rows(i).Item(1), vOthFldDataValue, clsSVT.getDataDoubleValue(strSQL, 0, False))
                            Case Else
                                .SetData(argDT.Rows(i).Item(1), vOthFldDataValue, clsSVT.getDataDateValue(strSQL, Nothing, False))
                        End Select
                    End If

                End If
            Next
        End With
    End Sub

    Private Sub InsertPrevItemData(ByVal argDT As DataTable, ByVal argPartyID As Int32, ByVal argItemID As Int32, argRow As Int32)
        'For detail section
        With vsfOptions
            If IsNothing(argDT) Then Exit Sub
            Dim strSQL As String = ""
            For i As Int32 = 0 To argDT.Rows.Count - 1
                If UCase(.GetData(argDT.Rows(i).Item(0), vOthFldBlnComboData)) = "Y" Then

                    If Trim(.GetData(argDT.Rows(i).Item(0), vOthFldPrevTranFldName)) = "" Then
                        strSQL = "Select Top 1 " & .GetData(argDT.Rows(i).Item(0), vOthFldCmbDisplayCols) & " from " & strDocHdrTblName & " h, "
                    Else
                        strSQL = "Select Top 1 " & Trim(.GetData(argDT.Rows(i).Item(0), vOthFldPrevTranFldName)) & " from " & strDocHdrTblName & " h, "
                    End If
                    strSQL &= .GetData(argDT.Rows(i).Item(0), vOthFldCmbTblName) & " v, " & strDocDtlTblName & " d "
                    strSQL &= " where h." & strDocIDFldName & " <> " & Val(txtDocID.Text)
                    strSQL &= " and h." & strDocIDFldName & " = d." & strDocIDFldName
                    strSQL &= " and h.cancelled='N' "
                    strSQL &= " and d." & .GetData(argDT.Rows(i).Item(0), vOthFldCmbStorageFldName) & " = t." & .GetData(argDT.Rows(i).Item(0), vOthFldCmbStorageFldName)
                    If .GetData(argDT.Rows(i).Item(0), vOthBlnLoadPrevTrnParty) = "Y" Then strSQL &= " and h." & strFldPartyID & " = " & argPartyID
                    If .GetData(argDT.Rows(i).Item(0), vOthBlnLoadPrevTrnItem) = "Y" Then strSQL &= " and d." & strFldItemID & " = " & argItemID
                    strSQL &= " Order by h." & strDocIDFldName & " desc"

                    vsfDet.SetData(argRow, argDT.Rows(i).Item(0), clsSVT.getDataStringValue(strSQL, "", False))
                Else
                    If Trim(.GetData(argDT.Rows(i).Item(0), vOthFldPrevTranFldName)) = "" Then
                        strSQL = "Select Top 1 " & .GetData(argDT.Rows(i).Item(0), vOthFldCode) & " from " & strDocHdrTblName & " h, "
                    Else
                        strSQL = "Select Top 1 " & .GetData(argDT.Rows(i).Item(0), vOthFldPrevTranFldName) & " from " & strDocHdrTblName & " h, "
                    End If
                    strSQL &= strDocDtlTblName & " d "
                    strSQL &= " where h." & strDocIDFldName & " <> " & Val(txtDocID.Text)
                    strSQL &= " and h." & strDocIDFldName & " = d." & strDocIDFldName
                    If .GetData(argDT.Rows(i).Item(0), vOthBlnLoadPrevTrnParty) = "Y" Then strSQL &= " and " & strFldPartyID & " = " & argPartyID
                    If .GetData(argDT.Rows(i).Item(0), vOthBlnLoadPrevTrnItem) = "Y" Then strSQL &= " and " & strFldItemID & " = " & argItemID
                    strSQL &= " and cancelled='N' "
                    strSQL &= " Order by h." & strDocIDFldName & " desc"

                    Select Case UCase(.GetData(argDT.Rows(i).Item(0), vOthFldDataType))
                        Case "TEXT", "LIST STRING"
                            vsfDet.SetData(argRow, argDT.Rows(i).Item(0), clsSVT.getDataStringValue(strSQL, "", False))
                        Case "NUMERIC", "LIST NUMERIC"
                            vsfDet.SetData(argRow, argDT.Rows(i).Item(0), clsSVT.getDataDoubleValue(strSQL, 0, False))
                        Case Else
                            vsfDet.SetData(argRow, argDT.Rows(i).Item(0), clsSVT.getDataDateValue(strSQL, Nothing, False))
                    End Select
                End If
            Next
        End With
    End Sub

    'Private Sub InsertPrevItemData(ByRef argint() As Int32, ByVal argPartyID As Int32, ByVal argItemID As Int32, argRow As Int32)
    '    With vsfOptions
    '        If IsNothing(argint) Then Exit Sub
    '        Dim strSQL As String = ""
    '        For i As Int32 = 0 To argint.Count - 1
    '            If UCase(.GetData(argint.GetValue(i), vOthFldBlnComboData)) = "Y" Then
    '                strSQL = "Select Top 1 " & .GetData(argint.GetValue(i), vOthFldCmbDisplayCols) & " from " & strDocHdrTblName & " h, "
    '                strSQL &= .GetData(argint.GetValue(i), vOthFldCmbTblName) & " v, " & strDocDtlTblName & " d "
    '                strSQL &= " where h." & strDocIDFldName & " <> " & Val(txtDocID.Text)
    '                strSQL &= " and h." & strDocIDFldName & " = d." & strDocIDFldName
    '                strSQL &= " and h.cancelled='N' "
    '                strSQL &= " and d." & .GetData(argint.GetValue(i), vOthFldCmbStorageFldName) & " = t." & .GetData(argint.GetValue(i), vOthFldCmbStorageFldName)
    '                If .GetData(argint.GetValue(i), vOthBlnLoadPrevTrnParty) = "Y" Then strSQL &= " and h." & strFldPartyID & " = " & argPartyID
    '                If .GetData(argint.GetValue(i), vOthBlnLoadPrevTrnItem) = "Y" Then strSQL &= " and d." & strFldItemID & " = " & argItemID
    '                strSQL &= " Order by h." & strDocIDFldName & " desc"

    '                vsfDet.SetData(argRow, argint.GetValue(i), clsSVT.getDataStringValue(strSQL, "", False))
    '            Else
    '                strSQL = "Select Top 1 " & .GetData(argint.GetValue(i), vOthFldCode) & " from " & strDocHdrTblName & " h, "
    '                strSQL &= strDocDtlTblName & " d "
    '                strSQL &= " where h." & strDocIDFldName & " <> " & Val(txtDocID.Text)
    '                strSQL &= " and h." & strDocIDFldName & " = d." & strDocIDFldName
    '                If .GetData(argint.GetValue(i), vOthBlnLoadPrevTrnParty) = "Y" Then strSQL &= " and " & strFldPartyID & " = " & argPartyID
    '                If .GetData(argint.GetValue(i), vOthBlnLoadPrevTrnItem) = "Y" Then strSQL &= " and " & strFldItemID & " = " & argItemID
    '                strSQL &= " and cancelled='N' "
    '                strSQL &= " Order by h." & strDocIDFldName & " desc"

    '                Select Case UCase(.GetData(argint.GetValue(i), vOthFldDataType))
    '                    Case "TEXT", "LIST STRING"
    '                        vsfDet.SetData(argRow, argint.GetValue(i), clsSVT.getDataStringValue(strSQL, "", False))
    '                    Case "NUMERIC", "LIST NUMERIC"
    '                        vsfDet.SetData(argRow, argint.GetValue(i), clsSVT.getDataDoubleValue(strSQL, 0, False))
    '                    Case Else
    '                        vsfDet.SetData(argRow, argint.GetValue(i), clsSVT.getDataDateValue(strSQL, Nothing, False))
    '                End Select
    '            End If
    '        Next
    '    End With
    'End Sub

    Private Sub vsfHdr_MouseClick(sender As Object, e As MouseEventArgs) Handles vsfHdr.MouseClick
        If e.Button = MouseButtons.Right Then
            If vsfHdr.Rows.Fixed = 1 Then
                vsfHdr.Rows.Fixed = 0
                Call SaveActFormVsfDet(Me, Me.Text, "Hdr", vsfHdr)
            Else
                vsfHdr.Rows.Fixed = 1
            End If
        End If
        'If e.Button = MouseButtons.Right Then
        '    If vsfHdr.Rows.Fixed >= 1 Then
        '        vsfHdr.Rows.Fixed = 0
        '        Call SaveActFormVsfDet(Me, Me.Text, vsfHdr.Name, vsfHdr)
        '    Else
        '        For i = 0 To vsfHdr.Rows.Count - 1
        '            If vsfHdr.Rows(i).Visible = True Then
        '                vsfHdr.Rows.Fixed = i + 1
        '                Exit For
        '            End If
        '        Next
        '    End If
        'End If
    End Sub

    Private Sub vsfFtr_MouseClick(sender As Object, e As MouseEventArgs) Handles vsfFtr.MouseClick
        'If e.Button = MouseButtons.Right Then
        '    If vsfFtr.Rows.Fixed = 1 Then
        '        vsfFtr.Rows.Fixed = 0
        '    Else
        '        vsfFtr.Rows.Fixed = 1
        '    End If
        'End If

        If e.Button = MouseButtons.Right Then
            If vsfFtr.Rows.Fixed >= 1 Then
                vsfFtr.Rows.Fixed = 0
                Call SaveActFormVsfDet(Me, Me.Text, vsfFtr.Name, vsfFtr)
            Else
                For i = 0 To vsfFtr.Rows.Count - 1
                    If vsfFtr.Rows(i).Visible = True Then
                        vsfFtr.Rows.Fixed = i + 1
                        Exit For
                    End If
                Next
            End If
        End If
    End Sub

    Private Sub vsfTot_MouseClick(sender As Object, e As MouseEventArgs) Handles vsfTot.MouseClick
        If e.Button = MouseButtons.Right Then
            If vsfTot.Rows.Fixed = 1 Then
                vsfTot.Rows.Fixed = 0
            Else
                vsfTot.Rows.Fixed = 1
            End If
        End If
    End Sub

    Private Sub GridFocusChange(ByVal argGrid As String)
        Select Case argGrid
            Case "D"
                If SetGridFocuschange(vsfHdr, "H") Then Exit Sub
                If SetGridFocuschange(vsfFtr, "F") Then Exit Sub
                If SetGridFocuschange(vsfTot, "T") Then Exit Sub
            Case "H"
                If SetGridFocuschange(vsfFtr, "F") Then Exit Sub
                If SetGridFocuschange(vsfTot, "T") Then Exit Sub
                If SetGridFocuschange(vsfDet, "D") Then Exit Sub
            Case "F"
                If SetGridFocuschange(vsfTot, "T") Then Exit Sub
                If SetGridFocuschange(vsfDet, "D") Then Exit Sub
                If SetGridFocuschange(vsfHdr, "H") Then Exit Sub
            Case "T"
                If SetGridFocuschange(vsfDet, "D") Then Exit Sub
                If SetGridFocuschange(vsfHdr, "H") Then Exit Sub
                If SetGridFocuschange(vsfFtr, "F") Then Exit Sub
        End Select
    End Sub

    Private Function SetGridFocuschange(ByVal argVSF As C1FlexGrid, argGrid As String) As Boolean
        SetGridFocuschange = False
        If argGrid = "D" Then
            If argVSF.Visible AndAlso argVSF.Rows.Count > 1 Then
                argVSF.Row = 1
                argVSF.Col = 0
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

    Private Sub vsfDet_KeyDown(sender As Object, e As KeyEventArgs) Handles vsfDet.KeyDown, vsfHdr.KeyDown, vsfTot.KeyDown, vsfFtr.KeyDown, vsfVwDet.KeyDown
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

        End If

        If sender.Name = "vsfDet" AndAlso vsfDet.Col = vDetAcLedName AndAlso vsfDet.Rows.Count > 1 AndAlso e.KeyCode = Keys.F4 Then
            Dim strAccType As String = ""
            With vsfDet
                Select Case Trim(.GetData(.Row, vDetAmtType))
                    Case "BY", "DR"
                        strAccType = "BY"
                    Case "TO", "CR"
                        strAccType = "TO"
                End Select

                strSQL = "WITH tblChild AS"
                strSQL = strSQL & " ("
                strSQL = strSQL & "     SELECT *"
                strSQL = strSQL & "         FROM MstLedGroup WHERE LedGroupID in ("
                strSQL = strSQL & " Select LedGroupID from UtlAccAmtTypeLedGrps where DocID = " & intDocID
                strSQL = strSQL & " and AmtType = '" & strAccType & "' "
                strSQL = strSQL & " )"
                strSQL = strSQL & "     Union All"
                strSQL = strSQL & "     SELECT MstLedGroup.* FROM MstLedGroup JOIN tblChild  ON mstledgroup.PLedGroupID = tblChild.LedGroupId"
                strSQL = strSQL & " )"
                strSQL = strSQL & " SELECT AcLedName, t.LedGroupName"
                strSQL = strSQL & "     FROM tblChild t, mstAcLed a"
                strSQL = strSQL & "     Where t.LedGroupID = A.LedGroupID"
                strSQL = strSQL & " OPTION(MAXRECURSION 32767)"
            End With

            txtDummy.Text = ""
            txtDummy2.Text = ""

            Dim objfrm As New frmUtlSelection
            With objfrm
                .blnMultiColSearch = True
                .FillViewVsf(Me, False, Nothing, strSQL, txtDummy, 0, txtDummy2, 1)
                .Text = " Please Select " & strItemShForm
                .Tag = " frmTrnVch "
                .ShowDialog()
                .Close()
            End With

            If Trim(txtDummy.Text) <> "" Then
                Call FillItemDet(txtDummy.Text, vsfDet.Row)

                '''''''''''''''''''''''''''''''Changed Code''''''''
                If vsfDet.GetData(vsfDet.Row, vDetAmtType) = "CR" Then
                    If Val(vsfDet.GetData(vsfDet.Row, vDetCredit)) = 0 Then
                        vsfDet.SetData(vsfDet.Row, vDetCredit, getDiffAmtFromGrid)
                    End If
                    vsfDet.Col = vDetCredit
                    vsfDet.SetData(vsfDet.Row, vDetDebit, 0)
                    vsfDet.StartEditing()
                Else
                    If Val(vsfDet.GetData(vsfDet.Row, vDetDebit)) = 0 Then
                        vsfDet.SetData(vsfDet.Row, vDetDebit, getDiffAmtFromGrid)
                    End If
                    vsfDet.Col = vDetDebit
                    vsfDet.SetData(vsfDet.Row, vDetCredit, 0)
                    vsfDet.StartEditing()
                End If
                ''''''''''''''''''''''''''''''Changed Code'''''''''''

                vsfDet.Col = vDetDebit

            End If
        End If
    End Sub

    Private Sub fillSrcDocFlds()
        '        Dim i As Integer, strOrgQry As String, strSQL As String
        '        Try
        '            'New field in UtlTransactions : PickFldsFromSrcDocQry
        '            'value in the field will be like this
        '            'select poid, itemslno, porate from trndtlpo where poid = #SRCDOCID# and itemslno = #SRCITEMSLNO#

        '            strOrgQry = clsSVT.getDataStringValue("Select PickFldsFromSrcDocQry from UTLTransactions where Docid =" & intDocID, 0, False)
        '            If Trim(strOrgQry) = "" Then Exit Sub
        '            For i = 1 To vsfDet.Rows.Count - 1
        '                Dim rs As Data.oledb.oledbDataReader
        '                strSQL = Replace(strOrgQry, "#SRCDOCID#", vsfDet.GetData(i, vDetSrcDocID))
        '                strSQL = Replace(strSQL, "#SRCITEMSLNO#", vsfDet.GetData(i, vDetSrcItemSlNo))
        '                rs = clsSVT.getDataReaderFromSQL(strSQL)
        '                If Not rs.HasRows Then
        '                    rs.Close()
        '                    Exit Sub
        '                End If
        '                With rs
        '                    Do While .Read()
        '                        For x = 0 To rs.FieldCount - 1
        '                            For y = intDetCols To vsfOptions.Rows.Count - 1
        '                                If UCase(vsfOptions.GetData(y, vOthFldCode)) = UCase(rs.GetName(x)) Then
        '                                    vsfDet.SetData(i, y, GetVal(rs.GetValue(x), ""))
        '                                    GoTo finishLoop
        '                                End If
        '                            Next y

        '                            For y = 0 To vsfHdr.Rows.Count - 1
        '                                If UCase(vsfHdr.GetData(y, vOthFldCode)) = UCase(rs.GetName(x)) Then
        '                                    vsfHdr.SetData(y, vOthFldDataValue, GetVal(rs.GetValue(x), ""))
        '                                    GoTo finishLoop
        '                                End If
        '                            Next y

        '                            For y = 0 To vsfFtr.Rows.Count - 1
        '                                If UCase(vsfFtr.GetData(y, vOthFldCode)) = UCase(rs.GetName(x)) Then
        '                                    vsfFtr.SetData(y, vOthFldDataValue, GetVal(rs.GetValue(x), ""))
        '                                    GoTo finishLoop
        '                                End If
        '                            Next y

        '                            For y = 0 To vsfTot.Rows.Count - 1
        '                                If UCase(vsfTot.GetData(y, vOthFldCode)) = UCase(rs.GetName(x)) Then
        '                                    vsfTot.SetData(y, vOthFldDataValue, GetVal(rs.GetValue(x), ""))
        '                                    GoTo finishLoop
        '                                End If
        '                            Next y
        'finishLoop:
        '                        Next x
        '                    Loop
        '                    .Close()
        '                End With
        '                'If intColrate > -1 Then
        '                '    strSQL = "Select " & strPendDocShForm & "RATE from " & strPendDtlTblName & " where " & strPendIDFldName & " = " & Val(vsfDet.GetData(i, vDetSrcDocID)) & " and ItemSlno = " & Val(vsfDet.GetData(i, vDetSrcItemSlNo))
        '                '    vsfDet.SetData(i, intColrate, clsSVT.getDataDoubleValue(strSQL, 0, False))
        '                'End If
        '            Next i
        '            Call FillColfromOtherCols()
        '        Catch ex As Exception
        '            MsgBox(ex.Message, vbInformation, Me.Text)
        '        End Try
    End Sub

    Private Sub FillColfromOtherCols()
        Dim i As Integer, strF As String, blnCalculate As Boolean
        Dim x As Integer, z As Integer
        With vsfOptions
            For x = 1 To vsfDet.Rows.Count - 1
                For i = 0 To .Rows.Count - 1
                    'MsgBox(Trim(.GetData(i, vOthFldCode)))
                    If Trim(.GetData(i, vOthBlnLoadColFrom)) = "Y" Then
                        strF = Trim(.GetData(i, vOthFldLoadColFrom))
                        If strF <> "" Then
                            blnCalculate = True
                            If Trim(.GetData(i, vOthFldBlnReadOnly)) = "N" Then
                                Select Case .GetData(i, vOthFldDataType)
                                    Case "Numeric"
                                        If Trim(.GetData(i, vOthFldBlnMandatory)) = "Y" Then
                                            If Val(vsfDet.GetData(x, i)) = 0 Then
                                                blnCalculate = True
                                            Else
                                                blnCalculate = False
                                            End If
                                        Else
                                            If Val(vsfDet.GetData(x, i)) > 0 Then
                                                blnCalculate = True
                                            ElseIf Trim(.GetData(i, vOthBlnLoadColFrom)) = "Y" Then  'satish 14feb2018
                                                blnCalculate = True
                                            Else
                                                blnCalculate = False
                                            End If
                                        End If

                                    Case Else
                                        If Trim(vsfDet.GetData(x, i)) = "" Then
                                            blnCalculate = True
                                        ElseIf Trim(.GetData(i, vOthBlnLoadColFrom)) = "Y" Then  'satish 14feb2018
                                            blnCalculate = True
                                        Else
                                            blnCalculate = False
                                        End If

                                End Select
                            End If
                            If Mid(strF, 1, 9) = "SVTGETQOH" Then
                                If Val(vsfDet.GetData(x, i)) = 0 Then
                                    blnCalculate = True
                                Else
                                    blnCalculate = False
                                End If
                            End If
                            If blnCalculate Then
                                For z = 0 To .Rows.Count - 1
                                    'MsgBox(.GetData(z, vOthFldCode))
                                    strF = Replace(strF, "<<" & z + 1 & ">>", Val(Trim(GetVal(vsfDet.GetData(x, z)))))

                                    If UCase(.GetData(z, vOthFldDataType)) = "DATE" Then
                                        strF = Replace(strF, "#" & .GetData(z, vOthFldCode) & "#", Format(vsfDet.GetData(x, z), "dd/MM/yyyy"))
                                    ElseIf UCase(.GetData(z, vOthFldDataType)) = "NUMERIC" Then
                                        strF = Replace(strF, "#" & .GetData(z, vOthFldCode) & "#", Format(Val(vsfDet.GetData(x, z)), "0.0000"))
                                    Else
                                        strF = Replace(strF, "#" & .GetData(z, vOthFldCode) & "#", GetVal(vsfDet.GetData(x, z)))
                                    End If
                                Next z

                                vsfDet.SetData(x, i, fnParse(Me, vsfDet, vsfOptions, strF, .GetData(i, vOthFldCaption), Me.cboPfx.Text,
                                                                Me.txtNo.Text, Me.txtDocNo.Text, "", Me.dtDocDate.Value))
                            End If
                        End If
                    End If
                Next i
            Next x
        End With
    End Sub

    Private Sub ReSizeHdrFtr()
        lblHdrTip.Location = New Point(lblHdrTip.Location.X, TabPage1.Bottom - lblHdrTip.Height - 5)
        btnHdrPin.Location = New Point(btnHdrPin.Location.X, TabPage1.Bottom - btnHdrPin.Height - 5)
        vsfHdr.Size = New Point(vsfHdr.Width, lblHdrTip.Top - 5)

        lblFtrTip.Location = New Point(lblFtrTip.Location.X, TabPage2.Bottom - lblFtrTip.Height - 5)
        btnFtrPin.Location = New Point(btnFtrPin.Location.X, TabPage2.Bottom - btnHdrPin.Height - 5)
        vsfFtr.Size = New Point(vsfFtr.Width, lblFtrTip.Top - 5)
    End Sub

    Private Sub txtNo_KeyUp(sender As Object, e As KeyEventArgs) Handles txtNo.KeyUp
        If e.KeyCode = Keys.Enter Then
            txtNo_Validated(Nothing, Nothing)
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

        If blnDetSection Then
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
        End If

    End Sub

    Private Sub vsfHdr_OwnerDrawCell(sender As System.Object, e As C1.Win.C1FlexGrid.OwnerDrawCellEventArgs) Handles vsfHdr.OwnerDrawCell
        If sender.Rows(e.Row).Style IsNot Nothing Then
            If sender.Rows(e.Row).Style.Name = "StyleClr" & e.Row Then
                e.Style = sender.Styles("StyleClr" & e.Row)
            End If
        End If
    End Sub

    Private Sub vsfFtr_OwnerDrawCell(sender As System.Object, e As C1.Win.C1FlexGrid.OwnerDrawCellEventArgs) Handles vsfFtr.OwnerDrawCell
        If sender.Rows(e.Row).Style IsNot Nothing Then
            If sender.Rows(e.Row).Style.Name = "StyleClr" & e.Row Then
                e.Style = sender.Styles("StyleClr" & e.Row)
            End If
        End If
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

    Private Sub vsfDet_CellButtonClick(sender As Object, e As RowColEventArgs) Handles vsfDet.CellButtonClick
        If Mode = "V" Or Mode = "" Then Exit Sub
        If e.Col = vDetPendBtn AndAlso e.Row > 0 Then
            Call showPendVch(e.Row)
        End If

        If vsfOptions.GetData(e.Col, vOthFldTxtBlnMultiLine) = "Y" Then
            StrgetText = vsfDet.GetData(e.Row, e.Col)
            Call vsfOptCellButtonClick(StrgetText, Val(vsfOptions.GetData(e.Col, vOthFldDataWidth)), vsfDet.GetData(0, e.Col), Me, IIf(GetVal(vsfOptions.GetData(e.Col, vOthFldBlnReadOnly), "") = "Y", True, False))
            vsfDet.SetData(e.Row, e.Col, StrgetText)
        End If
    End Sub

    Public Sub vsfHdr_CellButtonClick(sender As Object, e As RowColEventArgs) Handles vsfHdr.CellButtonClick
        With vsfHdr
            If .GetData(e.Row, vOthFldTxtBlnMultiLine) = "Y" Then
                StrgetText = .GetData(e.Row, vOthFldDataValue)
                Call vsfOptCellButtonClick(StrgetText, Val(.GetData(e.Row, vOthFldDataWidth)), .GetData(e.Row, vOthFldCaption), Me)
                .SetData(e.Row, vOthFldDataValue, StrgetText)
            End If
        End With
    End Sub

    Public Sub vsfFtr_CellButtonClick(sender As Object, e As RowColEventArgs) Handles vsfFtr.CellButtonClick
        With vsfFtr
            If .GetData(e.Row, vOthFldTxtBlnMultiLine) = "Y" Then
                StrgetText = .GetData(e.Row, vOthFldDataValue)
                Call vsfOptCellButtonClick(StrgetText, Val(.GetData(e.Row, vOthFldDataWidth)), .GetData(e.Row, vOthFldCaption), Me)
                .SetData(e.Row, vOthFldDataValue, StrgetText)
            End If
        End With
    End Sub

    Private Sub vsfVwDet_BeforeEdit(sender As Object, e As RowColEventArgs) Handles vsfVwDet.BeforeEdit
        If (Not (Mode = "A" Or Mode = "M")) Then
            e.Cancel = True
            Exit Sub
        End If
    End Sub

    Private Sub showPendVch(ByVal argRow As Integer)
        Dim strQry As String = ""
        Dim strType As String = ""
        Dim sngTot As Double, lngAccID As Long
        If strBillWiseFeatureReq = "N" Then Exit Sub
        lngAccID = 0
        If Mode = "M" Then
            lngAccID = clsSVT.getDataLongValue("Select AccID from TrnhdrACC where TrnID = " & Val(txtDocID.Text) & " and DocType = '" & strSHForm & "'", 0, False)
        End If
        With vsfDet
            If Val(.GetData(argRow, vDetCredit)) + Val(.GetData(argRow, vDetDebit)) > 0 Then
                strType = clsSVT.getDataStringValue("Select LedType from MstAcLed Where AcLedID = " & Val(.GetData(argRow, vDetAcLedID)), "", False)
                If strType = "C" Or strType = "S" Then
                    Dim objFrm As New frmTrnVchPend
                    With objFrm
                        .txtTOAdjust.Text = Format((-1 * Val(vsfDet.GetData(argRow, vDetCredit))) + Val(vsfDet.GetData(argRow, vDetDebit)), AccStringFmt)
                        .txtRemAmt.Text = Format((-1 * Val(vsfDet.GetData(argRow, vDetCredit))) + Val(vsfDet.GetData(argRow, vDetDebit)), AccStringFmt)
                        .optOnAccount.Checked = False
                        .optNewRef.Checked = False
                        .txtNewRef.Text = ""
                        .txtNewRef.Visible = False
                        .lblBalance.Text = "Balance : " & vsfDet.GetData(argRow, vDetBalAmt)
                        Call .LoadPendBills(Val(vsfDet.GetData(argRow, vDetAcLedID)), Me, lngAccID, strSHForm, vsfAdj)
                        .lblParty.Text = "Party: " & vsfDet.GetData(argRow, vDetAcLedName)
                        .ShowDialog()
                    End With

                    With vsfAdj
                        sngTot = 0
                        For i = 1 To .Rows.Count - 1
                            If Val(.GetData(i, vAdjAccLedID)) = Val(vsfDet.GetData(argRow, vDetAcLedID)) Then
                                sngTot = sngTot + Val(.GetData(i, vAdjAdjAmt))
                            End If
                        Next
                        vsfDet.SetData(argRow, vDetAgRefAmt, sngTot)
                        If vsfAdj.GetData(0, 0) = "N" Then
                            vsfDet.SetData(argRow, vDetNewRefAmt, Val(vsfDet.GetData(argRow, vDetCredit)) + Val(vsfDet.GetData(argRow, vDetDebit)) - sngTot)
                        End If
                    End With
                End If
            End If
        End With
        Exit Sub
    End Sub

    Private Sub showDrCrTot()
        lblTotCrValue.Text = ""
        lblTotDrValue.Text = ""

        Dim i As Integer
        With vsfDet
            For i = 1 To .Rows.Count - 1
                lblTotCrValue.Text = Val(lblTotCrValue.Text) + Val(.GetData(i, vDetCredit))
                lblTotDrValue.Text = Val(lblTotDrValue.Text) + Val(.GetData(i, vDetDebit))
            Next
            '--------Changed----------'
            vsfTot.SetData(intRowCRAmt, vOthFldDataValue, lblTotCrValue.Text)
            vsfTot.SetData(intRowDRAmt, vOthFldDataValue, lblTotDrValue.Text)
            '-------------------------'
        End With
    End Sub

    Private Sub frmTrnVch_Resize(sender As Object, e As EventArgs) Handles Me.Resize
        If Me.Width = 0 Then Exit Sub
        If vsfFtr.Cols(vOthFldCaption).Width >= vsfFtr.Width Then
            vsfFtr.Cols(vOthFldCaption).Width = vsfFtr.Width - 20
        End If
        If vsfHdr.Cols(vOthFldCaption).Width >= vsfHdr.Width Then
            vsfHdr.Cols(vOthFldCaption).Width = vsfHdr.Width - 20
        End If
    End Sub

    Private Sub vsfFtr_AfterResizeColumn(sender As Object, e As RowColEventArgs) Handles vsfFtr.AfterResizeColumn
        If vsfFtr.Cols(vOthFldCaption).Width >= vsfFtr.Width Then
            vsfFtr.Cols(vOthFldCaption).Width = vsfFtr.Width - 20
        End If
    End Sub

    Private Sub vsfHdr_AfterResizeColumn(sender As Object, e As RowColEventArgs) Handles vsfHdr.AfterResizeColumn
        If vsfHdr.Cols(vOthFldCaption).Width >= vsfHdr.Width Then
            vsfHdr.Cols(vOthFldCaption).Width = vsfHdr.Width - 20
        End If
    End Sub

    Public Sub PrintVch(argForm As Object, argDocID As Integer)
        'On Error GoTo ErrHandler
        Dim strSQL As String, strInsSql As String
        Dim i As Integer, j As Integer
        Dim strSection As String
        Dim svtTrn As oledb.oledbTransaction = Nothing
        Const vDetBYTO = 1
        Const vDetLedCustSupp = 2
        Const vDetDebit = 3
        Const vDetCredit = 4
        Const vDetAgRefAmt = 6
        'Const vDetOnAcAmt = 10
        'Const vDetBalAmt = 11

        'Const vDetAmtType = 0
        'Const vDetAcLedName = 1
        'Const vDetDebit = 2
        'Const vDetCredit = 3
        'Const vDetPendBtn = 4
        'Const vDetAcLedID = 5
        'Const vDetAgRefAmt = 6
        'Const vDetNewRefAmt = 7
        'Const vDetBalAmt = 8
        'Dim intDetCols As Int32 = 9
        Try
            'svtTrn = svtConn.BeginTransaction(IsolationLevel.ReadCommitted)
            clsSVT.execSQLStatement("delete from TempDtlVchPrint")
            clsSVT.execSQLStatement("delete from TempAddnlFldsVchPrint")

            strInsSql = "insert into TempDtlVchPrint ("
            strInsSql = strInsSql & " DocNo , DocDate, ByTo, LedgerDesc, DebitAmt, CreditAmt, AgntRefAmt, OnAcAmt)"
            strInsSql = strInsSql & " values("
            strInsSql = strInsSql & argDocID & ","
            strInsSql = strInsSql & "'" & Format(argForm.dtDocDate.value, "dd MMM yyyy") & "',"
            For i = 1 To argForm.vsfDet.Rows.count - 1
                strSQL = "'" & argForm.vsfDet.getdata(i, vDetBYTO) & "',"
                'If gCustCode = "NIITECH" Then
                '    If InStr(argForm.vsfDet.TextMatrix(i, vDetLedCustSupp), "Against Ref :") > 0 Then
                '        strLedDesc = Trim(Mid(Trim(Mid(argForm.vsfDet.TextMatrix(i, vDetLedCustSupp), 1, InStr(argForm.vsfDet.TextMatrix(i, vDetLedCustSupp), " - ") - 1)), 14))
                '        strSupprefNo = clsSVT.GetDataValue("SuppInvNo", "", "TrnHdrPB", False, "Where PBNO = '" & strLedDesc & "'")
                '        strSQL = strSQL & "'" & Replace(argForm.vsfDet.TextMatrix(i, vDetLedCustSupp), strLedDesc, strSupprefNo & " [" & strLedDesc & "]") & "',"
                '    Else
                '        strSQL = strSQL & "'" & argForm.vsfDet.TextMatrix(i, vDetLedCustSupp) & "',"
                '    End If
                'Else
                strSQL = strSQL & "'" & argForm.vsfDet.getdata(i, vDetLedCustSupp) & "',"
                'End If
                strSQL = strSQL & argForm.vsfDet.getdata(i, vDetDebit) & ","
                strSQL = strSQL & argForm.vsfDet.getdata(i, vDetCredit) & ","
                strSQL = strSQL & argForm.vsfDet.getdata(i, vDetAgRefAmt) & ","
                strSQL = strSQL & argForm.vsfDet.getdata(i, vDetAgRefAmt) & ""
                strSQL = strSQL & ")"
                clsSVT.execSQLStatement(strInsSql & strSQL)
            Next i

            strSection = ""
            For j = 0 To 2
                Select Case j
                    Case 0 'H
                        strSection = "H"
                    Case 1 'F
                        strSection = "F"
                    Case 2 'T
                        strSection = "T"
                End Select
                strInsSql = "insert into TempAddnlFldsVchPrint (RowNum,FldName,FldValue,FldSection, DocNum) values("

                For i = 0 To argForm.vsfGen(j).Rows - 1
                    strSQL = i
                    strSQL = strSQL & ",'" & argForm.vsfGen(j).getdata(i, vOthFldCaption) & "'"
                    strSQL = strSQL & ",'" & argForm.vsfGen(j).getdata(i, vOthFldDataValue) & "'"
                    strSQL = strSQL & ",'" & strSection & "'"
                    strSQL = strSQL & "," & argDocID & ""
                    strSQL = strSQL & ")"
                    'Debug.Print strSQL
                    clsSVT.execSQLStatement(strInsSql & strSQL)
                Next i
            Next j
            'svtTrn.Commit()
        Catch ex As Exception
            MsgBox(ex.Message & " [PrintVch]", vbInformation, argForm.text)
            'svtTrn.Rollback()
        End Try

        '        With argForm.CRpt
        '            If Not fo.FileExists(gstrRptPath & "fmtVchPrint.rpt") Then
        '                MsgBox "FmtVchPrint.rpt not exist in " & gstrRptPath, vbInformation + vbOKOnly, "Print Voucher"
        '            Exit Sub
        '            End If
        '            StMsg "Preparing " & argForm.Caption & " Report..."
        '        .ReportFileName = gstrRptPath & "FmtVchPrint.rpt"
        '            .WindowTitle = argForm.Caption & " Report"
        '            '.DataFiles(0) = gstrDataPath & gSettings.gMDBName
        '            For i = 0 To 20
        '                .Formulas(i) = ""
        '            Next i
        '            .Formulas(0) = "CompName='" & gSettings.gCompName & "'"
        '            .Formulas(1) = "Add1 = '" & gSettings.gCompAdd1 & "'"
        '            .Formulas(2) = "Add2 = '" & gSettings.gCompAdd2 & "'"
        '            .Formulas(3) = "Add3 = '" & gSettings.gCompAdd3 & "'"
        '            .Formulas(4) = "Add4 = '" & gSettings.gCompAdd4 & "'"
        '            .Formulas(5) = "frmlVCHName = '" & argForm.Caption & "'"
        '            .Formulas(6) = "frmlDocNo = '" & CStr(Trim(argForm.cboPfx.Text) & IIf(Trim(argForm.cboPfx.Text) <> "", "/", "") & Val(Right(Trim(argForm.txtDocNo.Text), 5))) & "'"
        '            '        .WindowState = crptMaximized
        '            '        .SubreportToChange = .GetNthSubreportName(0)
        '            '        .DataFiles(0) = gstrDataPath & gSettings.gMDBName
        '            '        .SubreportToChange = .GetNthSubreportName(1)
        '            '        .DataFiles(0) = gstrDataPath & gSettings.gMDBName
        '            '        .SubreportToChange = ""
        '            .Connect = strCrpConnect
        '            .Action = 1
        '            StMsg ""
        '    End With

        '        Exit Sub
        'errHandler:
        '        MsgBox Err.Description, vbInformation + vbOKOnly, argForm.Caption
        '    'svtConn.CommitTrans
    End Sub

    Private Sub dtDocDate_LostFocus(sender As Object, e As EventArgs) Handles dtDocDate.LostFocus
        vsfHdr.Focus()
    End Sub

    '----------------Added-------------'
    Private Sub UpdatePFX()
        Select Case UCase(vsfHdr.GetData(intRowVchType, vOthFldDataValue))
            Case "CASH", "CARD"
                cboPfx.Text = "CPV"
            Case "BANK"
                cboPfx.Text = "BPV"
        End Select
        cboPfx_Validating(Nothing, Nothing)
    End Sub
    '-----------------------------------'
End Class