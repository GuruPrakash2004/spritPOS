Imports C1.Win.C1FlexGrid
Imports System.IO
Imports System.Text

Public Class frmTrnRDM
    'This form  is applicable for Both Party and Non Party Document
    Dim clsSVT As New svtLibrary
    Dim dtSearch As DataTable
    Dim dvSearch As DataView
    Dim intMaxDocID As Long
    Dim svtTrn As oledb.oledbTransaction
    Dim ctrlList As Object 'IEnumerable(Of C1FlexGrid)
    Dim blnFirstTime As Boolean = False

    'for audit track
    Dim blnAuditTrack As Boolean = False
    Dim strAuditHdrSQL As String
    Dim strAuditDtlSQL As String

    Public Mode As String 'Control Transaction Add, Edit and Delete
    Public lngViewDocID As Long

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
    Dim strDocIDFldName As String
    Dim strDocNoFldName As String
    Dim strPfxTYpe As String
    Dim strDocDateFldName As String
    Dim strQtyFldName As String
    Dim intQtyDecimals As Integer
    Dim strGCustCode As String
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

    Dim strTenderFld As String = ""
    Dim blnFreezeQtyCol As Boolean
    Dim blnFreezeRateCol As Boolean
    Dim intFreezeCol As Int32
    Dim strEnvelop As String
    Dim blnBalColReq As Boolean
    Dim blnPartyRelatedFldsPresent As Boolean

    'LoadFromSettings Changes
    Dim blnPartyLoadFromSettingsPresent As Boolean

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
    Dim intPartyDocID As Int32
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


    'Party Control Related Variables.
    '---------------------------------
    Const intColPartyCtrlBill = 0
    Const intColPartyCtrlShip = 1

    Const intRowPartyCtrlID = 1
    Const intRowPartyCtrlName = 2
    Const intRowPartyCtrlAdd1 = 3
    Const intRowPartyCtrlAdd2 = 4
    Const intRowPartyCtrlAdd3 = 5
    Const intRowPartyCtrlAdd4 = 6
    Const intRowPartyCtrlCity = 7
    Const intRowPartyCtrlPincode = 8
    Const intRowPartyCtrlState = 9
    Const intRowPartyCtrlCountry = 10
    Const intRowPartyCtrlCName = 11
    Const intRowPartyCtrlCMob = 12
    Const intRowPartyCtrlCDesig = 13
    Const intRowPartyCtrlCEmail = 14
    '----------------------------------

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


    'declarations for detail grid
    Const vDetSlNo = 0
    Const vDetItemID = 1
    Const vDetItemSlNo = 2
    Const vDetItemCode = 3
    Const vDetItemName = 4
    Const vDetUOM = 5
    Const vDetShortClosed = 6
    Const vDetShortClosedDT = 7
    Const vDetShortClosedBy = 8
    Const vDetShortClosedQty = 9
    Const vDetQOH = 10
    Const vDetSrcDocID = 11
    Const vDetSrcDocNo = 12
    Const vDetSrcItemSlNo = 13
    Const vDetSrcDocQty = 14
    Const vDetCumQtyLinkedDoc = 15
    Const vDetPendQty = 16
    Const vDetConvFact = 17
    Const vDetDocCumQty = 18
    Dim intDetCols As Int32 = 19
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

    Dim strItemSearchQry As String, strItemPartySearchQry As String, strSearchQry As String
    Dim BlnEnforcePartyItemLink As Boolean
    Dim strRateFldPartyItem As String, strRateFldMstItem As String
    Dim dtPrintFmt As DataTable

    'Accounting Code BEGIN
    Dim intRowCrDays As Integer
    Dim blnAccTrn As Boolean = False
    Dim strAcLedFld As String
    Dim strAccValueFld As String

    Dim strTrnDrCr As String
    Dim intAcLedID As Integer
    Dim dblAccValue As Double
    Dim strDateTime As String = ""
    'Accounting Code END

    'Print Settings

    Dim blnDefPrint As Boolean
    Dim strPrintOption As String
    Dim strPrintPrompt As String
    Dim intRowLoc As Int32 = -1

    Public intRow As Integer
    '===================

    Dim blnPDFPreview As Boolean = False

    Dim intRowCustID As Int32
    Dim intRowPHNO As Int32
    Dim intRowCardNo As Int32
    Dim intRowRPoints As Int32
    Dim intRowTotPoints As Int32
    Public Sub svtBtnCtrl_btnClicked(ByVal argBtnClicked As Integer) Handles svtBtnCtrl.btnClicked
        svtBtnCtrl.Focus()
        Select Case argBtnClicked
            Case svtBtns.svtNew
                If Not chkUserRights(strRights, chkRightsPos.AddRights, Me.Text) Then Exit Sub
                vsfDet.RowSel = -1
                Call AddProc()

                Dim argDsgrGrids = GetAllDsgrVsf(Me, GetType(C1FlexGrid))
                For Each argVSF As C1FlexGrid In argDsgrGrids
                    Call chkSplRights_DOC(Me, intDocID, strRights, argVSF)
                Next
                Call chkSplRights_DOC(Me, intDocID, strRights, vsfOptions)

                svtBtnCtrl.PerformAction(argBtnClicked)
                svtFileAttach.ViewMode = False
                lblTitle.Text = "New - " & Me.Text

                Mode = "A"
                dtDocDate.Enabled = True
                If intPartyDocID > 0 Then
                    svtPCtrl.setFocus()
                Else
                    If blnManualPartySel Then
                        svtPCtrl.lblBill.Text = "Add"
                        svtPCtrl.lblShp.Text = "Add"
                    End If
                    If cboPfx.Visible And blnDetSection = False Then
                        cboPfx.Focus()
                    Else
                        dtDocDate.Focus()
                    End If
                End If
                strDateTime = Date.Now.ToString("yyyyMMddHHmmss")
                TimerStartEvent()
            Case svtBtns.svtClose
                Call frmMain.CloseSelectedTabPage(Me)

            Case svtBtns.svtEdit
                'Below code is for time being'
                If UCase(strSHForm) = "STKDED" Then
                    Exit Sub
                End If

                'chk edit user rights
                If Not chkUserRights(strRights, chkRightsPos.EditRights, Me.Text, True, dtDocDate.Value) Then Exit Sub

                'chk docid is present
                If Val(txtDocID.Text) <= 0 Then Exit Sub

                'chk basic parameters from utltransactions table.
                If ChkEditRights() = False Then Exit Sub

                Dim argDsgrGrids = GetAllDsgrVsf(Me, GetType(C1FlexGrid))
                For Each argVSF As C1FlexGrid In argDsgrGrids
                    Call chkSplRights_DOC(Me, intDocID, strRights, argVSF)
                Next
                Call chkSplRights_DOC(Me, intDocID, strRights, vsfOptions)


                'Start editing
                enableDisableFlds(True)
                svtBtnCtrl.PerformAction(argBtnClicked)

                lblTitle.Text = "Edit - " & Me.Text


                srcTransaction.Visible = False
                Mode = "M"

                If CheckforLinkedToDoc() Then
                    svtPCtrl.setFocus()
                Else
                    vsfDet.Focus()
                End If

                If blnManualPartySel Then
                    svtPCtrl.lblBill.Text = "Change"
                    svtPCtrl.lblShp.Text = "Change"
                End If

                strDateTime = Date.Now.ToString("yyyyMMddHHmmss")
                TimerStartEvent()


            Case svtBtns.svtSave
                If Val(txtNo.Text) <= 0 Then Exit Sub
                If validFlds() Then
                    If SaveDoc() Then
                        svtBtnCtrl.PerformAction(argBtnClicked)
                        Call INIStore("Prefix", Me.Text, cboPfx.Text)

                        'If blnDefPrint Then Call PrintAfterSave(strPrintOption)
                        'Call clearFlds()
                        If chkAmmendment.Checked = False Then
                            fraAmm.Visible = False
                        End If
                        enableDisableFlds(False)

                        svtFileAttach.ViewMode = True
                        ' lblTitle.ForeColor = Color.White
                        lblTitle.Text = "View - " & Me.Text
                        TimerStopEvent()
                        srcTransaction.setFocus()
                        srcTransaction.Visible = True
                        Mode = "V"

                        If Val(txtDocID.Text) > 0 Then
                            FillDocDetails(Val(txtDocID.Text))
                        End If
                        Call showBtns()
                        If blnDefPrint AndAlso strPrintPrompt <> "DNP" Then
                            If strPrintPrompt = "P" Then
                                If MsgBox("Do you want to print the document....?", MsgBoxStyle.YesNo, Me.Text) = MsgBoxResult.No Then
                                    Exit Sub
                                End If
                            End If
                            Call svtBtnCtrl_btnOptionSelected(strPrintOption, False, False)
                        End If
                    End If
                End If

            Case svtBtns.svtPrint
                If Not chkUserRights(strRights, chkRightsPos.PrintRights, Me.Text) Then Exit Sub
                If Val(txtDocID.Text) <= 0 Then Exit Sub
                If lblCancelled.Visible = True Then
                    MsgBox("Current record is cancelled. Printing not possible!", vbInformation, Me.Text)
                    Exit Sub
                End If
                If strAuthReq = "Y" Then
                    If lblAuthorised.Text <> "AUTHORISED" Then
                        MsgBox("Current record is not Authorised. Cannot Print!", vbInformation, Me.Text)
                        Exit Sub
                    End If
                End If
                Call prntCrpt()


            Case svtBtns.svtCancel
                Mode = "V"
                Call clearFlds()
                enableDisableFlds(False)
                Call DateFreezing(Me, Mode)
                vsfView.Visible = False
                svtFileAttach.ViewMode = True
                fraAttach.Visible = False
                lblTitle.Text = "View - " & Me.Text
                svtBtnCtrl.Focus()
                svtBtnCtrl.PerformAction(argBtnClicked)
                TimerStopEvent()
                svtPCtrl.vsfView.Visible = False
                '  svtPartyCtrl1.frmNormalSize()
                srcTransaction.Visible = True
                srcTransaction.setFocus()
                Call showBtns()

            Case svtBtns.svtMoveFirst, svtBtns.svtMoveLast, svtBtns.svtMoveNext, svtBtns.svtMovePrev
                If Not chkUserRights(strRights, chkRightsPos.ViewRights, Me.Text) Then Exit Sub
                Call FillDocDetails(gMoveRecords(argBtnClicked, strDocHdrTblName, strDocIDFldName, True, txtDocID.Text, Trim(cboPfx.Text), strDocDateFldName))
                Call showBtns()
                If Mode = "V" Then
                    'With vsfTop
                    '    For i = 1 To .Cols.Count - 1
                    '        Call SetColAlign(vsfTop, svtAlign.Center, i)
                    '    Next
                    'End With
                    Call updateDtlFldTotal()
                End If

            Case svtBtns.svtDelete
                If txtDocID.Text = "" Then Exit Sub
                If Val(txtDocID.Text) <= 0 Then Exit Sub
                If lblCancelled.Visible = True Then
                    MsgBox("Current record is cancelled. Deletion not possible!", vbInformation, Me.Text)
                    Exit Sub
                End If
                If Not chkUserRights(strRights, chkRightsPos.DelRights, Me.Text, True, dtDocDate.Value) Then Exit Sub
                Dim strLinkedToDocNo As String = ""
                If blnLinkedToDoc Then
                    strLinkedToDocNo = clsSVT.getDataStringValue("Select Top 1 " & strLinkedToDocNoFldName & " from " & strLinkedTODtlTblName & " d, " & strLinkedTOHdrTblName & " h" &
                                    " where h.Cancelled = 'N' and h." & strLinkedTODocFldIDName & " = d." & strLinkedTODocFldIDName &
                                    " and " & strDocIDFldName & " = " & txtDocID.Text, "", False)
                    If strLinkedToDocNo <> "" Then
                        MsgBox(strSHForm & "No already exist in " & strLinkedToDocNoFldName & " : '" & strLinkedToDocNo & "'", MsgBoxStyle.Critical, Me.Text)
                        Exit Sub
                    End If
                End If
                With vsfDet
                    If blnDetSection Then
                        For i As Int32 = 1 To .Rows.Count - 1
                            If Val(.GetData(i, vDetDocCumQty)) > 0 Then
                                If intItemDocID > 0 Then
                                    MsgBox("Deletion not possible single item linked with other document. Item:" & .GetData(i, vDetItemCode) & " LinkedQty:" & Val(.GetData(i, vDetDocCumQty)), MsgBoxStyle.Critical, Me.Text)
                                Else
                                    MsgBox("Deletion not possible single item linked with other document. Item:" & .GetData(i, intDetCols + 1) & " LinkedQty:" & Val(.GetData(i, vDetDocCumQty)), MsgBoxStyle.Critical, Me.Text)
                                End If
                                Exit Sub
                            End If
                        Next
                    End If
                End With

                'Accounting Code BEGIN
                'If blnAccTrn Then
                '    If Not ChkAccTransaction() Then Exit Sub
                'End If
                'Accounting Code END

                Call deleteDoc()

            Case svtBtns.svtFind
                Mode = "V"
                If Not chkUserRights(strRights, chkRightsPos.ViewRights, Me.Text) Then Exit Sub
                Call showViewTrnForm(Me, strDocHdrTblName, "Listing of " & strFrmCaption, "", strDocDateFldName, "", Me.Text, "", intDocID)
                Call showBtns()
        End Select
    End Sub

    Private Sub AddProc()
        Call clearFlds()
        fraAmm.Visible = False
        'Mode = "A"
        enableDisableFlds(True)
        Call DateFreezing(Me, Mode)
        Call fillVsfHFTDefValues(Me)
        Call FillAutoGenTrnNo()
        vsfDet.Rows.Count = 1
        If blnDetSection AndAlso Not blnLinkedDocFrom Then
            vsfDet.Rows.Count = 2
            If intItemDocID <= 0 Then
                vsfDet.SetData(vsfDet.Rows.Count - 1, vDetItemSlNo, getmaxNo())
            End If
        End If
        ReArrangeSlNo(vsfDet, vDetSlNo)
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
    End Sub

    Private Sub clearFlds()
        Mode = "V"
        enableDisableFlds(True)
        chkAmmendment.Checked = False
        fraAmm.Visible = False
        txtDocID.Text = ""
        txtDocNo.Text = ""
        txtNo.Text = ""
        txtPrintNo.Text = ""
        svtFileAttach.ClearFiles()
        dtDocDate.Value = getSystemDate()
        Call ClearAddnlGrid(Me)
        vsfDet.Rows.Count = 1
        If Not blnLinkedDocFrom Then
            vsfDet.Rows.Count = 2
        End If
        ReArrangeSlNo(vsfDet, vDetSlNo)
        Call clearAddress()
        fraAttach.Visible = False
        lblCancelled.Visible = False
        lblAuthorised.Visible = False
        svtBtnCtrl.Focus()
        'svtPCtrl.lblParty.ForeColor = Color
        svtPCtrl.ClearFlds()
        srcTransaction.ClearSearch("")
        srcTransaction.Visible = True
        vsfTop.Rows.Count = 0
        vsfTop.Rows.Count = 1
        With svtPCtrl
            .ClearFlds()
            .vsfVisisble(False)
            If blnOnlycode Then
                Call .frmSize_Code()
            End If
            If blnOnlyBill Then
                Call .frmSize_Billing()
            End If
            If blnBillShip Then
                Call .frmSize_BillShip()
            End If
            '.setFocus()
            If btnPendingDoc.Visible Then
                btnPendingDoc.BringToFront()
            End If
            Exit Sub
        End With
    End Sub

    Public Sub enableDisableFlds(ByVal argFlag As Boolean)
        Dim blnFound As Boolean = False
        If blnEditDocDate Then
            dtDocDate.Enabled = argFlag
        Else
            dtDocDate.Enabled = False
        End If
        btnPendingDoc.Enabled = argFlag
        svtPCtrl.EnableDisable(argFlag)
        srcTransaction.enabledisable("", True)
        ' Lock(txtNo, Not argFlag)
        ucBtnTools.btnVisible("DETAIL", argFlag, blnLinkedDocFrom)

        With vsfDet
            If blnDetSection And vsfDet.Visible Then
                If blnLinkedDocFrom Then
                    Call SetHideCol(vsfDet, Not argFlag, vDetSrcDocNo, vDetSrcDocQty, vDetCumQtyLinkedDoc, vDetPendQty)
                Else
                    Call SetHideCol(vsfDet, True, vDetSrcDocNo, vDetSrcDocQty, vDetCumQtyLinkedDoc, vDetPendQty)
                End If

                If strAddReduceStock <> "" Then
                    Call SetHideCol(vsfDet, Not argFlag, vDetQOH)
                Else
                    Call SetHideCol(vsfDet, True, vDetQOH)
                End If

                If IsNothing(dtCumQty) OrElse dtCumQty.Rows.Count <= 0 Then
                    Call SetHideCol(vsfDet, True, vDetShortClosed, vDetDocCumQty)
                Else
                    Call SetHideCol(vsfDet, Not argFlag, vDetShortClosed, vDetDocCumQty)
                End If
            End If
        End With
    End Sub

    Private Sub frmTrnPartyDoc_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
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

    Private Sub frmTrnPartyDoc_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyUp
        Dim blnFnKey As Boolean = False
        If e.KeyCode = Keys.F5 Then
            bindPartyCombo()
        End If
        If e.KeyCode = Keys.Escape Then
            'fraAddress.Visible = False
        End If
        If e.KeyCode = Keys.F9 And e.Control Then
            If blnDetSection Then
                Call initFormVsfDet(Me, Me.Text, vsfDet, "DET")
                Call initFormVsfDet(Me, Me.Text, vsfTop, "TOP")
                Call initFormVsfDet(Me, Me.Text, svtPCtrl.vsfView, "PVIEW")
            End If
            Dim argDsgrGrids = GetAllDsgrVsf(Me, GetType(C1FlexGrid))
            For Each GridList As C1FlexGrid In argDsgrGrids
                If GridList.Visible Then
                    Call initFormVsfDet(Me, Me.Text, GridList, GridList.Name)
                End If
            Next
            MsgBox("Restored Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
        If e.KeyCode = Keys.F8 And e.Control = True Then
            If blnDetSection Then
                Call SaveActFormVsfDet(Me, Me.Text, "DET", vsfDet)
                Call SaveActFormVsfDet(Me, Me.Text, "TOP", vsfTop)
                Call SaveActFormVsfDet(Me, Me.Text, "PVIEW", svtPCtrl.vsfView)
            End If
            Dim argDsgrGrids = GetAllDsgrVsf(Me, GetType(C1FlexGrid))
            For Each GridList As C1FlexGrid In argDsgrGrids
                If GridList.Visible Then
                    Call SaveActFormVsfDet(Me, Me.Text, GridList.Name, GridList)
                End If
            Next
            MsgBox("Saved Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If

        If vsfDet.Rows.Count > 1 AndAlso e.KeyCode = Keys.D AndAlso e.Control = True Then
            Dim i As Int32 = vsfDet.Rows.Count - 1
            vsfDet.Rows.Add()
            For j = 1 To vsfDet.Cols.Count - 1
                vsfDet.SetData(i + 1, j, vsfDet.GetData(i, j))
            Next
            Call ReArrangeSlNo(vsfDet, vDetSlNo)
        End If

        If e.Control Or blnFnKey Then
            Call svtBtnCtrl.btnCtrlKeyUp(sender, e)
        End If
        Call raiseShortCutEvent(e)

        If e.Shift = True And e.KeyCode = Keys.Right Then
            Call FillDocDetails(gMoveRecords(svtBtns.svtMoveNext, strDocHdrTblName, strDocIDFldName, True, txtDocID.Text, Trim(cboPfx.Text), strDocDateFldName))
        End If

        If e.Shift = True And e.KeyCode = Keys.Left Then
            Call FillDocDetails(gMoveRecords(svtBtns.svtMovePrev, strDocHdrTblName, strDocIDFldName, True, txtDocID.Text, Trim(cboPfx.Text), strDocDateFldName))
        End If

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
        If blnBalColReq Then Call syncGridCols()

        If intPartyDocID = 0 AndAlso Not blnManualPartySel AndAlso Not blnDetSection Then
            fraMain.Size = New Point(fraMain.Size.Width, dtDocDate.Size.Height + 15)
            btnHdrPin.Visible = False
            tbAdnlFlds.Location = New Point(fraMain.Location.X, fraMain.Location.Y + 10 + fraMain.Size.Height)
            tbAdnlFlds.Size = New Point(fraMain.Size.Width, tbAdnlFldsFtr.Bottom - 25)
        End If

    End Sub

    Private Sub setVsfTopColor()
        With vsfTop
            '.Styles.Normal.BackColor = Color.FromArgb(64, 64, 64)
            '.Styles.Normal.ForeColor = Color.White
            '' .Styles.Normal.Font.Bold = True
        End With
    End Sub

    Private Sub initScreen()
        Dim j As Integer
        vsfView.Visible = False
        fraAmm.Visible = False
        fraAttach.Visible = False
        cboPfx.Text = ""

        chkAmmendment.Checked = False
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

        If blnSMSFeature Then
            Dim strFldSms As String = clsSVT.getDataStringValue("Select FldSMS from utltransactions where DocID = '" & intDocID & "'", "", False)  'CNUD
            If strFldSms = "Y" Then
                svtBtnCtrl.addOption("SEND SMS")
            End If
        End If

        If clsSVT.getDataStringValue("Select blnEnvPrint From UtlTransactions Where DocID=" & intDocID, "", False) = "Y" Then
            svtBtnCtrl.addOption("Envelope Print")
        End If

        Dim strFldEmail As String = clsSVT.getDataStringValue("Select FldEmail from utltransactions where DocID = '" & intDocID & "'", "", False)
        If strFldEmail = "Y" Then
            svtBtnCtrl.addOption("SEND EMAIL")
        End If

        If blnDetSection Then
            With vsfDet
                If Not IsNothing(dtCumQty) Then
                    intDetCols = intDetCols + dtCumQty.Rows.Count
                End If
                .Cols.Count = intDetCols
                .Rows.Count = 1
                .SetData(0, vDetSlNo, "Sl No")
                .SetData(0, vDetItemCode, gItemCode)
                .SetData(0, vDetItemName, gItemName)
                .SetData(0, vDetShortClosed, "ShortClosed")
                .SetData(0, vDetShortClosedDT, "ShortClosed Date")
                .SetData(0, vDetShortClosedBy, "ShortClosed By")
                .SetData(0, vDetShortClosedQty, "ShortClosed Qty")
                .SetData(0, vDetUOM, "UOM")
                .SetData(0, vDetCumQtyLinkedDoc, "Cum Qty")
                .SetData(0, vDetSrcDocQty, strPendQtyFldName)
                .SetData(0, vDetSrcDocNo, strPendNoFldName)
                .SetData(0, vDetSrcDocID, strPendIDFldName)
                .SetData(0, vDetSrcItemSlNo, strPendDocShForm & " Slno")
                .SetData(0, vDetQOH, "QOH")
                .SetData(0, vDetPendQty, "Pend Qty")
                .SetData(0, vDetConvFact, "Conv Fact")
                .SetData(0, vDetDocCumQty, "Doc Cum Qty")
                .SetData(0, vDetItemSlNo, strItemShForm & "Slno")

                If Not IsNothing(dtCumQty) Then
                    For k As Int32 = 0 To dtCumQty.Rows.Count - 1
                        .SetData(0, intDetCols + k - dtCumQty.Rows.Count, dtCumQty.Rows(k).Item(1))
                        Call SetHideCol(vsfDet, True, intDetCols + k - dtCumQty.Rows.Count)
                    Next
                End If


                .Cols(vDetSlNo).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.LeftCenter
                strDocStdDtlWidth = "40, 15, 15, 150, 150, 75, 100,100,100,100,100,100,100,100,100,100,100,100,100"

                Call SetColAlign(vsfDet, svtAlign.Left, vDetItemCode, vDetItemName)


                Call SetHideCol(vsfDet, True, vDetItemID, vDetItemSlNo, vDetShortClosedBy, vDetShortClosedDT, vDetShortClosedQty, vDetSrcDocID, vDetSrcItemSlNo, vDetConvFact, vDetShortClosed, vDetDocCumQty)

                Call SetFlexColor(vsfDet, "", vDetItemName, vDetDocCumQty, vDetUOM, vDetSlNo)

                Call SetFlexColor(vsfDet, "M", vDetItemCode)

                If IsNothing(dtCumQty) OrElse dtCumQty.Rows.Count <= 0 Then
                    Call SetHideCol(vsfDet, True, vDetShortClosed, vDetDocCumQty)
                End If

                If Not blnLinkedDocFrom Then
                    Call SetHideCol(vsfDet, True, vDetSrcDocNo, vDetSrcDocQty, vDetCumQtyLinkedDoc, vDetPendQty)
                End If

                If blnLinkedDocFrom Then
                    Call SetFlexColor(vsfDet, "", vDetItemCode)
                End If

                If intItemDocID = 0 Then
                    Call SetHideCol(vsfDet, True, vDetItemCode, vDetItemName, vDetUOM)
                End If

                If Not (strAddReduceStock = "ADD" Or strAddReduceStock = "RED") Then
                    Call SetHideCol(vsfDet, True, vDetQOH)
                Else
                    Call SetFlexColor(vsfDet, "", vDetQOH)
                End If

                If intItemDocID > 0 Then Call initItemVsfCombo()
                Call assignDefPFX()
                Call InitDetAddnlGrid(Me, vsfDet, vsfOptions, intDetCols, strDocDtlTblName, intDocID)

                Call SetFlexColor(vsfDet, "", vDetItemName, vDetShortClosed, vDetShortClosedBy, vDetShortClosedDT, vDetShortClosedQty, vDetQOH, vDetSrcDocNo, vDetSrcDocQty, vDetCumQtyLinkedDoc, vDetDocCumQty, vDetPendQty)
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
                    If intItemDocID > 0 Then
                        .SetData(vDetItemCode, vOthFldBlnChkDuplicate, clsSVT.getDataStringValue("Select case isnull(BlnAllowDuplicateItems,'N') when 'N' then 'Y' else 'N' end from UtlTransactions where DocID=" & intDocID, "N", False))
                        .SetData(vDetUOM, vOthFldBlnChkDuplicate, .GetData(vDetItemCode, vOthFldBlnChkDuplicate))
                    End If
                    intColQty = getColNoFromAddnlDetGrid(vsfOptions, vsfDet, strSHForm & "QTY")
                    intColrate = getColNoFromAddnlDetGrid(vsfOptions, vsfDet, strSHForm & "RATE")
                    intColStkQty = getColNoFromAddnlDetGrid(vsfOptions, vsfDet, "STKQTY")
                    .SetData(vDetSlNo, vOthFldBlnReadOnly, "Y")
                    .SetData(vDetItemSlNo, vOthFldBlnReadOnly, "Y")
                    .SetData(vDetItemID, vOthFldBlnReadOnly, "Y")
                    .SetData(vDetItemName, vOthFldBlnReadOnly, "Y")
                    .SetData(vDetUOM, vOthFldBlnReadOnly, "Y")
                    .SetData(vDetShortClosed, vOthFldBlnReadOnly, "Y")
                    .SetData(vDetShortClosedDT, vOthFldBlnReadOnly, "Y")
                    .SetData(vDetShortClosedBy, vOthFldBlnReadOnly, "Y")
                    .SetData(vDetShortClosedQty, vOthFldBlnReadOnly, "Y")
                    .SetData(vDetQOH, vOthFldBlnReadOnly, "Y")
                    .SetData(vDetSrcDocID, vOthFldBlnReadOnly, "Y")
                    .SetData(vDetSrcDocNo, vOthFldBlnReadOnly, "Y")
                    .SetData(vDetSrcItemSlNo, vOthFldBlnReadOnly, "Y")
                    .SetData(vDetSrcDocQty, vOthFldBlnReadOnly, "Y")
                    .SetData(vDetCumQtyLinkedDoc, vOthFldBlnReadOnly, "Y")
                    .SetData(vDetPendQty, vOthFldBlnReadOnly, "Y")
                    .SetData(vDetConvFact, vOthFldBlnReadOnly, "Y")
                    .SetData(vDetDocCumQty, vOthFldBlnReadOnly, "Y")
                    If strAddReduceStock <> "" Then
                        intColStkQty = getColNoFromAddnlDetGrid(vsfOptions, vsfDet, "STKQTY")
                    End If
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
            End With
        Else
            vsfDet.Visible = False
            vsfTop.Visible = False
        End If



        If intPartyDocID > 0 Then
            If clsSVT.getDataStringValue("Select FldPickUpAfterFld from UtlDocDsgrFlds where DocID = " & intDocID & " and fldSection<>'Detail' and FldPickUpAfterFld = '#SVTPARTY#'", "", False) <> "" Then
                blnPartyRelatedFldsPresent = True
            Else
                blnPartyRelatedFldsPresent = False
            End If
        End If

        If strAddReduceStock <> "" AndAlso vsfHdr.Rows.Count >= 1 Then
            intRowLoc = getRowNoFromAddnlGrid(vsfHdr, "LOCID")
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
        Call showBtns()
        svtPCtrl.lblBill.BackColor = Color.Transparent
        If blnDetSection Then
            ucBtnTools.Visible = True
        End If

        If intDocID = 1220 Then
            svtPCtrl.txtPartyCode.Size = New Point(286, 100)
        End If
        If intDocID = 1277 Then
            svtBtnCtrl.addOption("Day's OP-Balance")
        End If
        intRowCustID = getRowNoFromAddnlGrid(vsfHdr, "CUSTID")
        intRowPHNO = getRowNoFromAddnlGrid(vsfHdr, "PHNO")
        intRowCardNo = getRowNoFromAddnlGrid(vsfHdr, "CARDNO")
        intRowTotPoints = getRowNoFromAddnlGrid(vsfHdr, "TOTPOINTS")
        intRowRPoints = getRowNoFromAddnlGrid(vsfHdr, "RPOINTS")
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

    Private Sub syncGridCols()
        Dim i As Integer
        With vsfDet
            vsfTop.Font = .Font
            vsfTop.Cols.Count = vsfDet.Cols.Count
            vsfTop.Size = New Point(vsfDet.Width, vsfTop.Height)
            For i = 0 To .Cols.Count - 1
                vsfTop.Cols(i).Width = vsfDet.Cols(i).Width
                vsfTop.Cols(i).Visible = vsfDet.Cols(i).Visible
            Next i
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
                If GetVal(!PDFPreview, "") = "Y" Then
                    blnPDFPreview = True
                End If

                strAuthReq = GetVal(!AuthReq, "N")
                strAuthQuery = GetVal(!NoOfAuthQueryCond, "")
                intNoOfAuth = GetVal(!NoOfAuth, 0)

                'Accounting Code BEGIN
                If GetVal(!AccTrn, "N") = "Y" Then
                    blnAccTrn = True
                    strAcLedFld = GetVal(!ACLedFld, "")
                    strAccValueFld = GetVal(!AccValueFld, "")
                    strTrnDrCr = GetVal(!TrnDrCr, "")
                End If
                'Accounting Code END

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

                strRateFldMstItem = GetVal(!RateFldMstItem, "")
                strRateFldPartyItem = GetVal(!RateFldPartyItem, "")

                strInsertDocNo = GetVal(!LoadPendingDocNos, "N")
                strInsertParty = GetVal(!LoadPendingPartyWise, "N")

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
                strTenderFld = UCase(GetVal(!TenderChangeFld, ""))
                blnFreezeQtyCol = IIf(GetVal(!FreezeQtyCol, "N") = "N", False, True)
                blnFreezeRateCol = IIf(GetVal(!FreezeQtyCol, "N") = "N", False, True)
                strEnvelop = GetVal(!blnEnvPrint, "N")

                If GetVal(!OfflineCalc, "N") = "Y" Then
                    blnCalc = True
                End If

                intPartyDocID = GetVal(!MstDocIDParty, 0)

                If intPartyDocID > 0 Then
                    Dim strPartyShForm As String
                    strPartyShForm = clsSVT.getDataStringValue("Select DocShForm From UtlTransactions where DocID = " & Val(intPartyDocID), "", False)
                    strPartyType = strPartyShForm
                    strPartyTblName = "Mst" & strPartyShForm
                    strFldPartyID = strPartyShForm & "ID"
                    strFldPartyCode = strPartyShForm & "Code"
                    strFldPartyName = strPartyShForm & "Name"
                    strPartyFilterCond = GetVal(!PartyFilterCond, "")
                    Select Case strPartyShForm
                        Case "CUST"
                            strPartyFlds = gCustQryFlds
                        Case "SUPP"
                            strPartyFlds = gSuppQryFlds
                    End Select
                    blnManualPartySel = IIf(GetVal(!AllowManualPartySel, "N") = "Y", True, False)
                    blnSavePartyData = IIf(GetVal(!SavePartyData, "Y") = "Y", True, False)
                    blnShipAddress = IIf(GetVal(!ShowShipAdd, "N") = "Y", True, False)
                Else
                    If GetVal(!AllowManualPartySel, "N") = "Y" Then
                        blnManualPartySel = IIf(GetVal(!AllowManualPartySel, "N") = "Y", True, False)
                        blnSavePartyData = IIf(GetVal(!SavePartyData, "Y") = "Y", True, False)
                        blnShipAddress = IIf(GetVal(!ShowShipAdd, "N") = "Y", True, False)
                    End If
                End If

                If intPartyDocID > 0 Then
                    With svtPCtrl
                        .ClearFlds()
                        If blnShipAddress Then
                            blnBillShip = True
                            .TrnGSettings("BS")
                            Call .frmSize_BillShip()
                        Else
                            blnOnlyBill = True
                            .TrnGSettings("B")
                            Call .frmSize_Billing()
                        End If
                        .SetPartyType(intPartyDocID)
                        If blnLinkedDocFrom Then
                            .ResizeCode()
                        End If

                    End With
                Else
                    If blnManualPartySel Then
                        With svtPCtrl
                            .ClearFlds()
                            If blnShipAddress Then
                                blnBillShip = True
                                .TrnGSettings("BS")
                                Call .frmSize_BillShip()
                            Else
                                blnOnlyBill = True
                                .TrnGSettings("B")
                                Call .frmSize_Billing()
                            End If
                            .SetPartyType(intPartyDocID)
                            If blnLinkedDocFrom Then
                                .ResizeCode()
                            End If
                            .txtPartyCode.Visible = False
                        End With
                    Else
                        svtPCtrl.Visible = False
                    End If
                End If

                intItemDocID = GetVal(!MstDocIDItem, 0)
                blnManualItemSel = IIf(GetVal(!NonItemDoc, "N") = "Y", True, False)
                blnDetSection = IIf(GetVal(!DetSecPresent, "N") = "N", False, True)

                If intItemDocID = 0 Then
                    blnItemScan = True
                    'MsgBox("No Item Table Present.. Application Closed...", MsgBoxStyle.Information, Me.Text)
                    'Me.Close()
                Else
                    strItemShForm = clsSVT.getDataStringValue("Select DocShForm From UtlTransactions where DocID = " & Val(intItemDocID), "", False)
                    strItemTblName = "Mst" & strItemShForm
                    strFldItemID = strItemShForm & "ID"
                    strFldItemSlno = strItemShForm & "Slno"
                    strFldItemCode = strItemShForm & "Code"
                    strFldItemName = strItemShForm & "Name"
                    strItemFilterCond = GetVal(!ItemFilterCond, "")
                End If

                If GetVal(!LinkedFromDoc, "N") = "Y" Then
                    btnPendingDoc.Visible = True
                    ''''Changed on 9Feb19
                    btnPendingDoc.Text = "Load Pending " & strPendDocShForm
                    ''''Changed on 9Feb19
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
                Call ucBtnTools.HideSMS(GetVal(!FldSMS, "N"))
            End If
            .Close()
        End With

        dtCumQty = clsSVT.getDATATable("Select LinkDocID,CumQtyFld from UtlTransCum  Where DocID = " & intDocID)
        dtPrintFmt = clsSVT.getDATATable("Select * from UtlDocNoFmt Where DocID=" & intDocID)

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
        Call setVSFTop()
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
        txtPrintNo.Text = getPrintDocFmtNo()
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
        strItemSearchQry = "Select distinct top " & gIntTopNrows & " " & strFldItemCode & " as [" & gItemCode & "], " & strFldItemName & " as [" & gItemName & "], "
        strItemSearchQry &= "i.ShortName, i.HotKey, u.uomcode "
        If gstrVsfItemCodeCols <> "" Then
            strItemSearchQry &= "," & gstrVsfItemCodeCols
        End If
        strItemSearchQry &= " From " & strItemTblName & " i, MstUOM u, MstItemGroup g"
        strItemSearchQry &= " where i.UOMID=u.UOMID "
        strItemSearchQry &= " and g.ItemGroupID=i.ItemGroupID "
        strItemSearchQry &= " and i.InActive = 'N' "
        'strItemSearchQry &= " and i.Authorised = 'Y' "
        strItemSearchQry &= strItemFilterCond
        'strItemSearchQry &= " Order by " & strFldItemCode

        strItemPartySearchQry = "Select top " & gIntTopNrows & " " & strFldItemCode & " as [" & gItemCode & "], u.UOMCode, " & strFldItemName & " as [" & gItemName & "] "
        If gstrVsfItemCodeCols <> "" Then
            strItemPartySearchQry &= "," & gstrVsfItemCodeCols
        End If
        strItemPartySearchQry &= " From " & strItemTblName & " i, MstUOM u, MstItemGroup g, Mst" & strPartyType & "Item pi"
        strItemPartySearchQry &= " where i.UOMID=u.UOMID "
        strItemPartySearchQry &= " and g.ItemGroupID=i.ItemGroupID "
        strItemPartySearchQry &= " and pi.ItemID=iu.ItemID "
        strItemPartySearchQry &= " and pi.UOMID=u.UOMID "
        strItemPartySearchQry &= " And i.InActive = 'N' "
        'strItemPartySearchQry &= " And i.Authorised = 'Y' "
        strItemPartySearchQry &= " And pi." & strPartyType & "ID = <ID>"
        strItemPartySearchQry &= strItemFilterCond
        'strItemPartySearchQry &= " Order by " & strFldItemCode
    End Sub

    Private Sub vsfDet_AfterEdit(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfDet.AfterEdit
        If Mode = "V" Or Mode = "" Then Exit Sub
        If intPartyDocID > 0 AndAlso svtPCtrl.Visible Then
            If Trim(svtPCtrl.getCode()) = "" Then Exit Sub
        End If
        If blnManualPartySel Then
            If Trim(svtPCtrl.lblBillCName.Text) = "" Then Exit Sub
        End If

        If e.Col <> vDetSlNo Then Call vsfOptAfterEditBfrCalTot(Me, vsfDet, vsfOptions, vsfDet.Row, vsfDet.Col)
        If blnCalc = False Then
            Call CalTotal()
        End If
    End Sub

    Private Sub vsfDet_BeforeEdit(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfDet.BeforeEdit
        If (Not (Mode = "A" Or Mode = "M")) Then
            e.Cancel = True
            Exit Sub
        End If
        Dim blnFound As Boolean
        If intItemDocID = 0 Then GoTo startdsgrchk
        vsfDet.EditMask = ""
        vsfDet.ComboList = ""


        Select Case vsfDet.Col
            Case vDetSlNo, vDetItemSlNo, vDetShortClosed, vDetShortClosedBy, vDetShortClosedDT, vDetShortClosedQty,
                 vDetCumQtyLinkedDoc, vDetQOH, vDetSrcDocID, vDetSrcDocNo, vDetSrcItemSlNo, vDetSrcDocQty, vDetPendQty, vDetConvFact, vDetItemName, vDetDocCumQty, vDetUOM
                e.Cancel = True
            'Case vDetUOM
            '    If blnLinkedDocFrom Then
            '        e.Cancel = True
            '        Exit Sub
            '    End If
            '    If Val(vsfDet.GetData(vsfDet.Row, vDetItemID)) <= 0 Then
            '        e.Cancel = True
            '        Exit Sub
            '    End If
            '    vsfDet.ComboList = GenVSfComboStringCond("UOMCode", "vwItemWiseUOM", "", " Where ItemID=" & Val(vsfDet.GetData(vsfDet.Row, vDetItemID)))
            '    Exit Sub

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

        If e.Col = vDetItemCode Then
            If blnLinkedDocFrom Then
                e.Cancel = True
                Exit Sub
            End If

            If Mode = "M" AndAlso vsfDet.Cols.Count - 1 >= vDetDocCumQty AndAlso Val(vsfDet.GetData(e.Row, vDetDocCumQty)) > 0 Then
                e.Cancel = True
                Exit Sub
            End If

            If blnLinkedToDoc And Mode = "M" Then
                If vsfDet.Rows.Count > 1 Then
                    If Val(vsfDet.GetData(vsfDet.Row, vDetItemID)) > 0 And Val(vsfDet.GetData(vsfDet.Row, vDetItemSlNo)) > 0 Then
                        Call clsSVT.getDataLongValue("Select ItemId from " & strLinkedTODtlTblName & " d, " & strLinkedTOHdrTblName & " h" &
                            " where h.Cancelled = 'N' and h." & strLinkedTODocFldIDName & " = d." & strLinkedTODocFldIDName &
                            " and ItemID = " & vsfDet.GetData(vsfDet.Row, vDetItemID) &
                            " And " & strDocIDFldName & " = " & txtDocID.Text & " And ItemSlno = " & vsfDet.GetData(vsfDet.Row, vDetItemSlNo), 0, blnFound)
                        If blnFound Then
                            e.Cancel = True
                            Exit Sub
                        End If
                    End If
                End If
            End If
        End If

        If e.Col < intDetCols Then Exit Sub
startdsgrchk:
        If vsfOptBeforeEditCancel(Me, vsfDet, vsfOptions, strDocDtlTblName, Mode, vsfDet.Row, vsfDet.Col, intDetCols) Then
            e.Cancel = True
            Exit Sub
        End If

    End Sub

    Private Sub vsfView_BeforeEdit(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfView.BeforeEdit
        e.Cancel = True
    End Sub

    Private Function applyFilterFromGrid() As Boolean
        applyFilterFromGrid = True
        Dim StrQry As String = ""
        Dim blnFound As Boolean = False

        With vsfDet
            If .Editor.Text = "" Then
                vsfView.Visible = False
                Exit Function
            End If

            If intPartyDocID > 0 Then
                StrQry = strSearchQry
            Else
                StrQry = strItemSearchQry
            End If

            strSQL = clsSVT.getDataLongValue("Select DocID from UtlTransactions where DocID= " & intDocID & " and AllowLoadFromBOM = 'Y'", 0, blnFound)
            If blnFound Then
                StrQry &= " And i.BOMItem = 'Y'"
            End If

            If StrQry <> "" Then
                StrQry &= " And (" & strFldItemCode & " Like '%" & .Editor.Text & "%' Or "
                StrQry &= strFldItemName & " Like '%" & .Editor.Text & "%' or "
                StrQry &= " ShortName Like '%" & .Editor.Text & "%' or "
                StrQry &= "HotKey Like '%" & .Editor.Text & "%')"
            End If
            '---- Changed
            'StrQry &= "Order by " & strFldItemCode
            '-------
        End With
        Call showWaitCursor()
        If StrQry <> "" Then
            dtSearch = clsSVT.getDATATable(StrQry)
            dvSearch = New DataView(dtSearch)
            vsfView.DataSource = dvSearch
        End If
        Call showNormalCursor()
        vsfView.Refresh()
    End Function

    Private Sub vsfView_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles vsfView.DoubleClick
        If vsfView.Row > 0 Then
            Call loadItemToDtl()
        End If
    End Sub

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

    Private Sub loadItemToDtl()
        If Not FillItemDet(vsfView.GetData(vsfView.Row, 0), vsfDet.Row, argUOM:=vsfView.GetData(vsfView.Row, 4)) Then
            MsgBox("Invalid Item!!", vbInformation, Me.Text)
            vsfView.Visible = True
            Exit Sub
        Else
            vsfView.Visible = False
            If intDetNextEntryCol <= -1 Then Exit Sub
            vsfDet.Focus()
            vsfDet.Col = intDetNextEntryCol
            'vsfDet.StartEditing()''''''Changed on 7Feb19
            vsfView.Visible = False
        End If
    End Sub

    Public Function FillItemDet(ByVal argItem As String, ByVal argRow As Integer, ByVal argUOM As String) As Boolean
        Dim rsDummy As oledb.oledbDataReader
        Dim strSQL As String
        Dim blnInActive As Boolean
        FillItemDet = False
        If argRow < 1 Then argRow = 1

        With vsfDet
            If argItem = "" Then
                Exit Function
            End If

            intItemId = clsSVT.getDataLongValue("Select " & strFldItemID & " from " & strItemTblName & " where " & strFldItemCode & " = '" & argItem & "'", 0, blnInActive)
            If intItemId = 0 Then
                MsgBox(strItemShForm & " code '" & argItem & "' does not exist!", vbInformation, Me.Text)
                Exit Function
            End If

            Call clsSVT.getDataLongValue("Select " & strFldItemID & " from " & strItemTblName & " where " & strFldItemCode & " = '" & argItem & "' and InActive <> 'N'", 0, blnInActive)

            If blnInActive Then
                MsgBox(strItemShForm & " code '" & argItem & "' is inactive! Please select an active " & strItemShForm & " from the list.", vbInformation, Me.Text)
                Exit Function
            End If

            'strSQL = "Select i.*, UOMCode from "
            'strSQL = strSQL & strItemTblName & " i, MstUOM u"
            'strSQL = strSQL & "  where i.UomID = u.UomID  "
            'strSQL = strSQL & " and  " & strFldItemCode & " = '" & argItem & "'"

            strSQL = "Select i.*, UOMCode, 1 ConvFact from "
            strSQL = strSQL & strItemTblName & " i, MstUOM u "
            strSQL = strSQL & "  where i.UomID = u.UomID  "
            strSQL = strSQL & " and  " & strFldItemCode & " = '" & argItem & "'"

            rsDummy = clsSVT.getDataReaderFromSQL(strSQL)

            With rsDummy
                If Not .HasRows Then
                    .Close()
                    MsgBox(strItemShForm & " does not exist!!", vbInformation, Me.Text)
                    Exit Function
                End If
                .Read()

                vsfDet.SetData(argRow, vDetSlNo, argRow)
                If Val(vsfDet.GetData(argRow, vDetItemSlNo)) <= 0 Then
                    vsfDet.SetData(argRow, vDetItemSlNo, getmaxNo())
                End If
                vsfDet.SetData(argRow, vDetItemID, rsDummy(strFldItemID))
                vsfDet.SetData(argRow, vDetItemName, rsDummy(strFldItemName))
                vsfDet.SetData(argRow, vDetItemCode, rsDummy(strFldItemCode))
                If argUOM <> "" Then
                    vsfDet.SetData(argRow, vDetUOM, argUOM)
                    vsfDet.SetData(argRow, vDetConvFact, clsSVT.getDataDoubleValue("Select ConvFact From MstItemUOM Where ItemID=" & Val(rsDummy(strFldItemID)) & " and UOMID = (Select UOMID from MstUOM where UOMcode='" & argUOM & "')", 1, False))
                Else
                    vsfDet.SetData(argRow, vDetUOM, !UOMCode)
                    vsfDet.SetData(argRow, vDetConvFact, !ConvFact)
                End If

                If strAddReduceStock <> "" AndAlso intRowLoc > -1 Then
                    vsfDet.SetData(argRow, vDetQOH, clsSVT.getDataDoubleValue("Select CLBal from TrnQOH q where LocID = (Select LocID from MstLoc Where LocName='" & vsfHdr.GetData(intRowLoc, vOthFldDataValue) & "') and  ItemID = " & Val(rsDummy(strFldItemID)), 0, False))
                End If

                .Close()
            End With
        End With

        Dim blnFound As Boolean
        If intColrate >= 0 Then
            'vsfDet.SetData(argRow, intColrate, 0)   ----Changed
            If strRateFldPartyItem <> "" Then
                vsfDet.SetData(argRow, intColrate, clsSVT.getDataDoubleValue("Select " & strRateFldPartyItem & " From Mst" & strPartyType & strItemShForm & " Where UOMID = (Select UOMID from MstUOM where UOMCode = '" & vsfDet.GetData(argRow, vDetUOM) & "') and " & strFldItemID & " = " & vsfDet.GetData(argRow, vDetItemID) & " and " & strPartyType & "ID =" & Val(svtPCtrl.getBillID()), 0, blnFound))
            End If

            If Not blnFound AndAlso strRateFldMstItem <> "" Then
                vsfDet.SetData(argRow, intColrate, clsSVT.getDataDoubleValue("Select " & strRateFldMstItem & " From " & strItemTblName & " Where " & strFldItemID & " = " & vsfDet.GetData(argRow, vDetItemID), 0, blnFound))
            End If
        End If

        Call InsertPrevItemData(dtDetPrevTran, Val(svtPCtrl.getBillID()), vsfDet.GetData(argRow, vDetItemID), argRow)
        Call fillItemRelatedDataFromDocDsgnr(intDocID, Val(vsfDet.GetData(argRow, vDetItemID)), Me)
        Call vsfOptValidateEdit(Me, vsfDet, vsfOptions, vsfDet.Row, vDetItemCode, "T", strDocDtlTblName, "", "")
        Call fillVsfDetDefValues(vsfDet, vsfOptions, argRow)
        Call ItemDetailsFillAfterPartySelection(argRow)
        If blnCalc = False Then
            Call CalTotal()
        End If
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

    Private Sub vsfDet_ChangeEdit(sender As Object, e As System.EventArgs) Handles vsfDet.ChangeEdit
        If blnItemScan = True Then Exit Sub
        If vsfDet.Col = vDetItemCode Then
            Select Case vsfDet.Editor.Text
                Case ""
                    vsfView.Visible = False
                Case Else
                    If fraMain.Location = vsfDet.Location Or fraMain.Location = vsfTop.Location Then
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
            End Select
        End If
    End Sub

    Private Sub vsfDet_EnterCell(ByVal sender As Object, ByVal e As System.EventArgs) Handles vsfDet.EnterCell
        If (Not (Mode = "A" Or Mode = "M")) Then Exit Sub

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
                    If intItemDocID > 0 AndAlso .Col = vDetItemCode Then
                        Try
                            If blnLinkedDocFrom Then Exit Sub
                            If blnFreezeDoc AndAlso Mode = "M" Then Exit Sub
                            '  .StartEditing()
                        Catch ex As Exception

                        End Try
                    End If
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
        If Mode = "V" Or Mode = "" Then Exit Sub
        If vsfDet.Focused Then Exit Sub
        If intItemDocID > 0 Then
            If vsfDet.Rows.Count > 1 AndAlso Trim(GetVal(vsfDet.GetData(1, vDetItemCode), "")) = "" Then
                vsfDet.Col = vDetItemCode
            End If
        Else
            If vsfDet.Rows.Count > 1 AndAlso Trim(GetVal(vsfDet.GetData(1, intDetNextEntryCol), "")) = "" Then
                vsfDet.Col = intDetNextEntryCol
            End If
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

        If e.Control = True And e.KeyCode = Keys.C Then
            Call vsfCopyItemCode(vsfDet, vsfDet.Row, vsfDet.Col)
        End If

        If (Not (Mode = "A" Or Mode = "M")) Then Exit Sub

        With vsfDet
            If intItemDocID > 0 Then
                If e.KeyCode = Keys.Escape AndAlso .Row = .Rows.Count - 1 AndAlso (.Col = vDetItemCode Or .Col = vDetSlNo) Then
                    If Trim(.GetData(.Row, vDetItemCode)) = "" Then

                        Call ReArrangeSlNo(vsfDet, vDetSlNo)

                        If vsfTot.Visible Then
                            vsfTot.Focus()
                            vsfTot.StartEditing()
                            Exit Sub
                        End If
                        If vsfFtr.Visible Then
                            vsfFtr.Focus()
                            vsfFtr.StartEditing()
                            Exit Sub
                        End If

                        svtBtnCtrl.Focus()
                        Exit Sub
                    End If
                End If

                If .Col = vDetItemCode AndAlso GetVal(.GetData(.Row, .Col), "") <> "" AndAlso e.KeyCode = Keys.Enter Then
                    Call loadItemToDtl()
                    Exit Sub
                End If

                If Not blnLinkedDocFrom And .Col = intLastCol And e.KeyCode = Keys.Tab And .Row = .Rows.Count - 1 Then
                    If Trim(.GetData(.Row, vDetItemCode)) <> "" Then
                        .Rows.Count = .Rows.Count + 1
                        .SetData(.Rows.Count - 1, vDetSlNo, .Rows.Count - 1)
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

                If .Col = vDetItemCode Then
                    If e.KeyCode = Keys.Escape Then
                        vsfView.Visible = False
                    End If
                End If

                If e.KeyCode = Keys.Down AndAlso vsfView.Visible Then
                    vsfView.Focus()
                    vsfView.Row = 1
                    vsfView.Col = 1
                End If

            Else

                If (e.KeyCode = Keys.Escape) And .Row = .Rows.Count - 1 And (.Col = intDetNextEntryCol Or .Col = vDetSlNo) Then
                    If Trim(.GetData(.Row, intDetNextEntryCol)) = "" Then

                        Call ReArrangeSlNo(vsfDet, vDetSlNo)

                        If vsfTot.Visible Then
                            vsfTot.Focus()
                            vsfTot.StartEditing()
                            Exit Sub
                        End If

                        If vsfFtr.Visible Then
                            vsfFtr.Focus()
                            vsfFtr.StartEditing()
                            Exit Sub
                        End If

                        svtBtnCtrl.Focus()
                        Exit Sub
                    End If
                End If

                If Not blnLinkedDocFrom And .Col = intLastCol And e.KeyCode = Keys.Tab And .Row = .Rows.Count - 1 Then
                    If Trim(.GetData(.Row, intDetNextEntryCol)) <> "" Then
                        .Rows.Count = .Rows.Count + 1
                        Call fillVsfDetDefValues(vsfDet, vsfOptions, .Rows.Count - 1)
                        .SetData(.Rows.Count - 1, vDetSlNo, .Rows.Count - 1)
                        If Val(vsfDet.GetData(.Rows.Count - 1, vDetItemSlNo)) <= 0 Then
                            vsfDet.SetData(.Rows.Count - 1, vDetItemSlNo, getmaxNo())
                        End If
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
            End If

        End With
        If intDetCols < intDetCols Then Exit Sub
    End Sub

    Private Sub vsfDet_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles vsfDet.LostFocus
        If (Not (Mode = "A" Or Mode = "M")) Then Exit Sub
        With vsfDet
            If intItemDocID > 0 AndAlso vsfView.Visible AndAlso vsfDet.Col <> vDetItemCode Then
                vsfView.Visible = False
            End If
            ' If If vsfView.Visible Then Exit Sub
            If Not blnLinkedDocFrom AndAlso .Rows.Count = 1 Then
                .Rows.Count = 2
                ReArrangeSlNo(vsfDet, vDetSlNo)
                If intItemDocID <= 0 Then
                    ReArrangeSlNo(vsfDet, vDetItemSlNo)
                End If
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
        Dim intDocQty As Double

        If (Not (Mode = "A" Or Mode = "M")) Then
            e.Cancel = True
            Exit Sub
        End If

        With vsfDet
            If intItemDocID > 0 AndAlso e.Col = vDetItemCode AndAlso Not vsfView.Visible Then

            End If

            If e.Col = intColQty Then
                intDocQty = Val(.Editor.Text)
                If intItemDocID > 0 AndAlso GetVal(.GetData(.Row, vDetItemCode), "") = "" Then
                    MsgBox("Please select item first....", MsgBoxStyle.Critical, Me.Text)
                    '.Editor.Text = ""
                    Exit Sub
                End If

                If Not gBlnAllowNegativeStock AndAlso strAddReduceStock = "RED" AndAlso intDocQty > GetVal(.GetData(.Row, vDetQOH), 0) Then
                    MsgBox("Qty cannot more than QOH....", MsgBoxStyle.Critical, Me.Text)
                    '.Editor.Text = ""
                    Exit Sub
                End If

                If Not gBlnAllowNegativeStock AndAlso Mode = "M" AndAlso intDocQty < GetVal(.GetData(.Row, vDetDocCumQty), 0) Then
                    MsgBox("Qty cannot Less than Cum. Qty: " & GetVal(.GetData(.Row, vDetDocCumQty), 0) & "....", MsgBoxStyle.Critical, Me.Text)
                    '.Editor.Text = intDocQty
                    Exit Sub
                End If


                If blnLinkedDocFrom AndAlso intDocQty > GetVal(.GetData(.Row, vDetPendQty), 0) Then
                    MsgBox("Qty cannot more than Pending Qty....", MsgBoxStyle.Critical, Me.Text)
                    '.Editor.Text = intDocQty
                    Exit Sub
                End If
            End If
        End With

        If vsfDet.Col < intDetCols Then Exit Sub

        If vsfOptValidateEdit(Me, vsfDet, vsfOptions, vsfDet.Row, vsfDet.Col, "T", strDocDtlTblName, "", "", svtPCtrl.getCode()) = False Then
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
        Call ReArrangeSlNo(vsfDet, vDetSlNo)

        If Not blnManualPartySel AndAlso intPartyDocID > 0 AndAlso svtPCtrl.getCode() = "" Then
            MsgBox("Please select Party....", MsgBoxStyle.Information, Me.Text)
            vsfView.Visible = False
            svtPCtrl.txtPartyCode.Focus()
            Exit Sub
        End If

        If gridEmpty() Then
            If svtPCtrl.getCode() = "" Then
                vsfDet.Rows.Count = 1
                If Not blnLinkedDocFrom Then
                    vsfDet.Rows.Count = 2
                End If
                Call ReArrangeSlNo(vsfDet, vDetSlNo)
                Exit Sub
            End If
        End If
        If blnDetSection Then
            Call AddnlFldCalTotal(Me, vsfOptions, vsfDet, intDetCols, svtPCtrl.getCode(), "", argMode:=Mode)
            Call AddnlFldCalTotal(Me, vsfOptions, vsfDet, intDetCols, svtPCtrl.getCode(), "", argMode:=Mode)
        Else
            Call AddnlFldCalTotal(Me, Trim(cboPfx.Text), "", Mode)
        End If
        Call AddnlFldAppotionateCatTotal(Me, intDetCols, "", "", intDocID)
        Call vsfGenHideMndtAllGrids(Me)
        If strAddReduceStock <> "" And intColStkQty > -1 Then
            With vsfDet
                For i As Int32 = 1 To .Rows.Count - 1
                    If GetVal(.GetData(i, vDetItemID), 0) > 0 AndAlso GetVal(.GetData(i, intColQty), 0) > 0 Then
                        .SetData(i, intColStkQty, IIf(Val(.GetData(i, vDetConvFact)) = 0, 1, Val(.GetData(i, vDetConvFact))) * Val(.GetData(i, intColQty)))
                    End If
                Next
            End With
        End If

        Call updateDtlFldTotal()
        Call syncGridCols()
    End Sub

    Public Function validFlds() As Boolean
        Dim i As Integer
        Dim blnFound As Boolean
        Dim intDupRow As Integer, strDupDataCaption As String, strDupDataValue As String
        validFlds = False
        '============
        'header validations
        '============

        If blnDetSection Then
            Call ReArrangeSlNo(vsfDet, vDetSlNo, vDetItemSlNo)
        End If


        If blnCalc = True Then
            Call CalTotal()
        End If

        If Not validDate(dtDocDate.Value) Then
            MsgBox("Doc Date Should be within the financial year," & vbCrLf & "Please go to Company Master And Change Financial year" & vbCrLf & " " & vbCrLf & "1. Click On Create new financial year and Click Create" & vbCrLf & "" & vbCrLf & "2. Click On set Active Financial Year and Select The Year " & vbCrLf & "" & vbCrLf & "3. Click on Set As Active " & vbCrLf & "" & vbCrLf & "Note: Please Take a Database Backup Before Doing Above Steps", vbCritical, Me.Text)

            Dim objFrm As New FrmMstComp
            With objFrm
                frmMain.OpenForms(objFrm, "Company Master")
                objFrm.C1DockingTabPage3.Show()
            End With
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

        If blnManualPartySel Then
            If GetVal(svtPCtrl.lblBillCName.Text, "") = "" Then
                MsgBox("Party Name should not be blank!!", vbInformation, Me.Text)
                svtPCtrl.setFocus()
                Exit Function
            End If
        End If

        If intPartyDocID > 0 Then
            With svtPCtrl
                If Not blnManualPartySel Then
                    If Val(.getBillID()) = 0 Then
                        MsgBox("Party Name should not be blank!!", vbInformation, Me.Text)
                        svtPCtrl.setFocus()
                        Exit Function
                    End If
                End If
            End With
        End If

        If Val(vsfHdr.GetData(intRowRPoints, vOthFldDataValue)) > Val(vsfHdr.GetData(intRowTotPoints, vOthFldDataValue)) Then
            MsgBox("REDEEM POINTS cannot exceed TOTAL POINTS", MsgBoxStyle.Critical, Me.Text)
            Exit Function
        End If
        If Val(vsfHdr.GetData(intRowTotPoints, vOthFldDataValue)) < gIntAlertIfPointsGreaterThan Then
            MsgBox("Balance points should be greater than " & gIntAlertIfPointsGreaterThan & " points to Redeem", MsgBoxStyle.Critical, Me.Text)
            Exit Function
        End If

        If blnDetSection Then
            With vsfDet
                For intGridRow As Int32 = 0 To .Rows.Count - 1
                    For intGridCol As Int32 = 0 To .Cols.Count - 1
                        .SetData(intGridRow, intGridCol, CStr(GetVal(.GetData(intGridRow, intGridCol), "")).Replace("'", "`"))
                    Next
                Next
                strDupDataValue = ""
                strDupDataCaption = ""
                If ChkDuplicateData(vsfOptions, vsfDet, intDupRow, strDupDataCaption, strDupDataValue) Then
                    MsgBox("Duplicate Data present (in row " & intDupRow & ") " & vbCrLf & "for combination of columns " & strDupDataCaption & vbCrLf & "for the values " & strDupDataValue, vbInformation, Me.Text)
                    .Row = intDupRow
                    .TopRow = intDupRow
                    .Col = vDetItemCode
                    .Focus()
                    Exit Function
                End If
            End With
        End If

        With vsfDet
            If intItemDocID > 0 Then
                If Not blnLinkedDocFrom Then
                    Call RemoveEmptVSFRows(vsfDet, 1, vDetItemCode)
                End If

                If vsfDet.Rows.Count = 1 Then
                    MsgBox("Please select an item!", vbInformation, Me.Text)

                    If Not blnLinkedDocFrom Then
                        vsfDet.Rows.Count = vsfDet.Rows.Count + 1
                        vsfDet.Row = 1
                        vsfDet.Col = vDetItemCode
                        vsfDet.Focus()
                    End If
                    Exit Function
                End If

                For i = 1 To .Rows.Count - 1
                    If Trim(.GetData(i, vDetItemCode)) = "" Then
                        MsgBox("Please Enter Itemcode..", vbInformation, Me.Text)
                        .Row = i
                        .TopRow = i
                        .Col = vDetItemCode
                        .Focus()
                        Exit Function
                    End If

                    intItemId = clsSVT.getDataLongValue("Select " & strFldItemID & " from " & strItemTblName & " where " & strFldItemID & "= " & Val(.GetData(i, vDetItemID)), 0, False)
                    If intItemId = 0 Then
                        MsgBox(strFldItemCode & " does not exist!!", MsgBoxStyle.Critical, Me.Text)
                        .Row = i
                        .TopRow = i
                        .Col = vDetItemCode
                        .Focus()
                        Exit Function
                    End If

                    If Not gBlnAllowNegativeStock AndAlso intColQty >= 0 AndAlso Val(.GetData(i, intColQty)) <= 0 Then
                        MsgBox("Please enter the qty greater then 0", MsgBoxStyle.Critical, Me.Text)
                        .Focus()
                        .Row = i
                        .Col = intColQty
                        .StartEditing()
                        Exit Function
                    End If

                    If strAddReduceStock = "RED" AndAlso intColQty > -1 AndAlso Val(.GetData(i, intColQty)) > Val(.GetData(i, vDetQOH)) Then
                        MsgBox("Please enter the QTY Less than QOH", MsgBoxStyle.Critical, Me.Text)
                        .Focus()
                        .Row = i
                        .Col = intColQty
                        .StartEditing()
                        Exit Function
                    End If

                    If Mode = "M" AndAlso intColQty >= 0 AndAlso Val(.GetData(i, intColQty)) < Val(.GetData(i, vDetDocCumQty)) Then
                        MsgBox("Please enter the QTY GREATER than Doc Cum Qty", MsgBoxStyle.Critical, Me.Text)
                        .Focus()
                        .Row = i
                        .Col = intColQty
                        .StartEditing()
                        Exit Function
                    End If

                    If blnLinkedDocFrom Then
                        If intColQty >= 0 AndAlso Val(.GetData(i, intColQty)) > GetVal(.GetData(i, vDetPendQty), 0) Then
                            MsgBox("Please enter the QTY Less than Pending Qty", MsgBoxStyle.Critical, Me.Text)
                            .Focus()
                            .Row = i
                            .Col = intColQty
                            .StartEditing()
                            Exit Function
                        End If
                    End If
                Next i
            Else
                If intDetNextEntryCol > 0 Then
                    Call RemoveEmptVSFRows(vsfDet, 1, intDetNextEntryCol)
                End If
            End If
        End With

        'Accounting Code BEGIN
        If blnAccTrn Then
            Call setAccVariables()
            If intAcLedID = 0 Or dblAccValue = 0 Then
                'If Not calAccTrnFlds(vsfAcc, vsfHdr, vsfFtr, vsfTot, vsfOptions, vsfDet, svtPCtrl.getCode(), intDocID) Then
                MsgBox("Invalid Accounting values. Please check Transaction-Accounting settings.", MsgBoxStyle.Critical, Me.Text)
                Exit Function
                'End If
            End If
        End If
        'Accounting Code END

        If Not validOthFLds(Me, strDocHdrTblName, strDocNoFldName, txtDocNo.Text, Trim(strSHForm)) Then
            Exit Function
        End If
        If blnDetSection Then
            If Not validOthDetFLds(Me, vsfDet, vsfOptions, intDetCols) Then Exit Function
        End If
        validFlds = True
    End Function

    Private Sub setAccVariables()
        Dim intLedID As Integer = 0
        Dim intRow As Integer

        'assign the ACLEDID to the variable for saving to TrnHdrAcc
        intAcLedID = 0
        Select Case strAcLedFld
            Case "<<P>>"
                intAcLedID = clsSVT.getDataLongValue("Select AcLedID from MstAcLed where MstDocID = " & intPartyDocID & " and LedMstID = " & svtPCtrl.getBillID(), 0, False)
            Case Else
                intRow = getRowNoFromAddnlGrid(vsfHdr, strAcLedFld)
                If intRow >= 0 Then
                    intAcLedID = clsSVT.getDataLongValue("Select AcLedID from MstAcLed where LedType = 'L' and AcLedName = '" & getValueFromAddnlGrid(strAcLedFld, vsfHdr) & "'", 0, False)
                Else
                    intRow = getRowNoFromAddnlGrid(vsfFtr, strAcLedFld)
                    If intRow >= 0 Then
                        intAcLedID = clsSVT.getDataLongValue("Select AcLedID from MstAcLed where LedType = 'L' and AcLedName = '" & getValueFromAddnlGrid(strAcLedFld, vsfFtr) & "'", 0, False)
                    End If
                End If
        End Select

        'assign the dblAccValue  to the variable for saving to TrnHdrAcc
        dblAccValue = 0
        intRow = getRowNoFromAddnlGrid(vsfTot, strAccValueFld)
        If intRow >= 0 Then
            dblAccValue = Val(getValueFromAddnlGrid(strAccValueFld, vsfTot))
        Else
            intRow = getRowNoFromAddnlGrid(vsfFtr, strAccValueFld)
            If intRow >= 0 Then
                dblAccValue = Val(getValueFromAddnlGrid(strAccValueFld, vsfFtr))
            Else
                intRow = getRowNoFromAddnlGrid(vsfHdr, strAccValueFld)
                If intRow >= 0 Then
                    dblAccValue = Val(getValueFromAddnlGrid(strAccValueFld, vsfHdr))
                End If
            End If
        End If
    End Sub

    Public Function FillDocDetails(ByVal argDocID As Long) As Boolean
        FillDocDetails = False
        If Trim(argDocID) = "" Or Val(argDocID) = 0 Then
            Call cboPfx_Validating(Nothing, Nothing)
            Exit Function
        End If
        txtDocID.Text = argDocID
        StMsg("Filling Header Fields...")
        Call fillDocHdrFlds(argDocID)
        If blnDetSection Then
            StMsg("Filling Detail Fields...")
            Call fillDtlFields(argDocID)
        End If
        FillDocDetails = True
        Call showBtns()
        StMsg("")
        With svtFileAttach
            .ClearFiles()
            .LoadFilesFromData("Select AFilename from UtlAttachFiles Where DocID = " & intDocID & " and DocValueID = '" & argDocID & "'")
        End With
        Call refreshAttachBtn()
        svtPCtrl.Focus()
        Call showBtns()
    End Function

    Private Function fillDocHdrFlds(ByVal argDocID As Long) As Boolean
        Dim rsDocHdr As oledb.oledbDataReader = Nothing
        Dim intCustID As Integer
        Call DateFreezing(Me, "")

        '  If intPartyDocID > 0 Then
        '    If 
        '    rsDocHdr = clsSVT.getDataReaderFromSQL("select h.*, " & strFldPartyCode & " from " & strDocHdrTblName & " h, " & strPartyTblName & " p " _
        '                                        & " where h." & strFldPartyID & " = p." & strFldPartyID & " and h." & strDocIDFldName & " = " & argDocID)
        'Else
        rsDocHdr = clsSVT.getDataReaderFromSQL("select * from " & strDocHdrTblName & "  " _
                                                            & " where " & strDocIDFldName & " = " & argDocID)
        '  End If

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
            txtPrintNo.Text = rsDocHdr("Print" & strDocNoFldName)
            Call getPfxNo(txtDocNo.Text, strPfx, strNo)
            cboPfx.Text = strPfx
            txtNo.Text = Val(strNo)

            If intPartyDocID > 0 Or blnManualPartySel Then
                intPartyID = 0
                If Not blnManualPartySel Then intPartyID = rsDocHdr(strFldPartyID)
                With svtPCtrl
                    If intPartyID > 0 Then
                        ' Call .LoadFromGrid(strPartyType, intPartyDocID, blnOnlycode, blnBillShip, blnOnlyBill)
                        .setCode(clsSVT.getDataStringValue("Select " & strFldPartyCode & " from " & strPartyTblName & " where " & strFldPartyID & " = " & intPartyID, "", False))
                    Else
                        .setCode("")
                    End If
                    .setGridData(intRowPartyCtrlID, intColPartyCtrlBill, 0)
                    If Not blnManualPartySel Then .setGridData(intRowPartyCtrlID, intColPartyCtrlBill, Val(rsDocHdr.Item(strFldPartyID)))



                    If blnSavePartyData Then
                        .setGridData(intRowPartyCtrlName, intColPartyCtrlBill, GetVal(rsDocHdr.Item("PartyName"), ""))
                        .setGridData(intRowPartyCtrlAdd1, intColPartyCtrlBill, GetVal(rsDocHdr.Item("PartyAdd1"), ""))
                        .setGridData(intRowPartyCtrlAdd2, intColPartyCtrlBill, GetVal(rsDocHdr.Item("PartyAdd2"), ""))
                        .setGridData(intRowPartyCtrlAdd3, intColPartyCtrlBill, GetVal(rsDocHdr.Item("PartyAdd3"), ""))
                        .setGridData(intRowPartyCtrlAdd4, intColPartyCtrlBill, GetVal(rsDocHdr.Item("PartyAdd4"), ""))
                        .setGridData(intRowPartyCtrlCity, intColPartyCtrlBill, GetVal(rsDocHdr.Item("PartyCity"), ""))
                        .setGridData(intRowPartyCtrlState, intColPartyCtrlBill, GetVal(rsDocHdr.Item("PartyState"), ""))
                        .setGridData(intRowPartyCtrlPincode, intColPartyCtrlBill, GetVal(rsDocHdr.Item("PartyPincode"), ""))
                        .setGridData(intRowPartyCtrlCountry, intColPartyCtrlBill, GetVal(rsDocHdr.Item("PartyCountry"), ""))
                        .setGridData(intRowPartyCtrlCName, intColPartyCtrlBill, GetVal(rsDocHdr.Item("PartyCName"), ""))
                        .setGridData(intRowPartyCtrlCMob, intColPartyCtrlBill, GetVal(rsDocHdr.Item("PartyCMob"), ""))
                        .setGridData(intRowPartyCtrlCEmail, intColPartyCtrlBill, GetVal(rsDocHdr.Item("PartyCEmail"), ""))
                        .setGridData(intRowPartyCtrlCDesig, intColPartyCtrlBill, GetVal(rsDocHdr.Item("PartyCDesig"), ""))
                    End If
                    If blnShipAddress Then
                        .setGridData(intRowPartyCtrlName, intColPartyCtrlShip, GetVal(rsDocHdr.Item("PartySName"), ""))
                        .setGridData(intRowPartyCtrlAdd1, intColPartyCtrlShip, GetVal(rsDocHdr.Item("PartySAdd1"), ""))
                        .setGridData(intRowPartyCtrlAdd2, intColPartyCtrlShip, GetVal(rsDocHdr.Item("PartySAdd2"), ""))
                        .setGridData(intRowPartyCtrlAdd3, intColPartyCtrlShip, GetVal(rsDocHdr.Item("PartySAdd3"), ""))
                        .setGridData(intRowPartyCtrlAdd4, intColPartyCtrlShip, GetVal(rsDocHdr.Item("PartySAdd4"), ""))
                        .setGridData(intRowPartyCtrlCity, intColPartyCtrlShip, GetVal(rsDocHdr.Item("PartySCity"), ""))
                        .setGridData(intRowPartyCtrlState, intColPartyCtrlShip, GetVal(rsDocHdr.Item("PartySState"), ""))
                        .setGridData(intRowPartyCtrlPincode, intColPartyCtrlShip, GetVal(rsDocHdr.Item("PartySPincode"), ""))
                        .setGridData(intRowPartyCtrlCountry, intColPartyCtrlShip, GetVal(rsDocHdr.Item("PartySCountry"), ""))
                        .setGridData(intRowPartyCtrlCName, intColPartyCtrlShip, GetVal(rsDocHdr.Item("PartySCName"), ""))
                        .setGridData(intRowPartyCtrlCMob, intColPartyCtrlShip, GetVal(rsDocHdr.Item("PartySCMob"), ""))
                        .setGridData(intRowPartyCtrlCEmail, intColPartyCtrlShip, GetVal(rsDocHdr.Item("PartySCEmail"), ""))
                        .setGridData(intRowPartyCtrlCDesig, intColPartyCtrlShip, GetVal(rsDocHdr.Item("PartySCDesig"), ""))
                    End If
                    .UpdateFromGrid()
                End With
            Else
                If blnSavePartyData Then

                End If
            End If

            'fill the Doc date
            dtDocDate.Value = Format(rsDocHdr(strDocDateFldName), "dd/MM/yyyy")
            fraAmm.Visible = False
            chkAmmendment.Checked = False
            If ucBtnTools.tsbAmmend.Visible = True Then
                If !Ammendment = "Y" Then
                    fraAmm.Visible = True
                    txtAmmNo.Text = !AmmNo
                    dtAmmDate.Value = Format(!AmmDate, "dd/MM/yyyy")
                    chkAmmendment.Checked = True
                Else
                End If
            End If
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

            Call vsfGenHideMndtAllGrids(Me)

            strSearchQry = strItemSearchQry
            If intPartyDocID > 0 AndAlso BlnEnforcePartyItemLink Then
                strSearchQry = strItemPartySearchQry.Replace("<ID>", Val(svtPCtrl.getBillID()))
            End If

            If Mode = "V" Then
                fraAmm.Enabled = False
            Else
                fraAmm.Enabled = True
            End If
            intCustID = clsSVT.getDataStringValue("Select CustID From mstCust where CustCode='" & vsfHdr.GetData(intRowCustID, vOthFldDataValue) & "'", "", False)
            vsfHdr.SetData(intRowPHNO, vOthFldDataValue, clsSVT.getDataStringValue("Select PHNO From mstCust where Custid=" & intCustID, "", False))
            vsfHdr.SetData(intRowCardNo, vOthFldDataValue, clsSVT.getDataStringValue("Select CardNo From mstCust where Custid=" & intCustID, "", False))
            vsfHdr.SetData(intRowTotPoints, vOthFldDataValue, clsSVT.getDataLongValue("Select sum(InvPoints - RPoints) from vwredeem where CustID = " & intCustID, 0, False))

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
        Dim rsDtl As oledb.oledbDataReader


        strsql = "Select d.* "
        If intItemDocID > 0 Then
            strsql = strsql & ", ItemCode, ItemName, UOMCode,1 AS ConvFact  "
        End If
        If blnLinkedDocFrom Then
            strsql = strsql & "," & strPendNoFldName
            strsql = strsql & ",ph." & strPendIDFldName
            If intItemDocID > 0 Then
                strsql = strsql & ", (Select " & strPendQtyFldName & " from " & strPendDtlTblName & " p where p." & strPendIDFldName & " = d." & strPendIDFldName & " and p.ItemID = d.ItemID and p.ItemSlno = d." & strPendDocShForm & "ItemSlno) as PQty "
                strsql = strsql & ", (Select " & strSHForm & "CumQty from " & strPendDtlTblName & " p where p." & strPendIDFldName & " = d." & strPendIDFldName & " and p.ItemID = d.ItemID and p.ItemSlno = d." & strPendDocShForm & "ItemSlno) - " & strQtyFldName & " as CQty "
            Else
                strsql = strsql & ", (Select " & strPendQtyFldName & " from " & strPendDtlTblName & " p where p." & strPendIDFldName & " = d." & strPendIDFldName & " and p.ItemSlno = d." & strPendDocShForm & "ItemSlno) as PQty "
                strsql = strsql & ", (Select " & strSHForm & "CumQty from " & strPendDtlTblName & " p where p." & strPendIDFldName & " = d." & strPendIDFldName & " and p.ItemSlno = d." & strPendDocShForm & "ItemSlno) - " & strQtyFldName & " as CQty "
            End If
        End If
        strsql = strsql & " from  " & strDocDtlTblName & "  d "
        If intItemDocID > 0 Then
            strsql = strsql & " ," & strItemTblName & " i, MstUOM u "
        End If
        If blnLinkedDocFrom Then
            strsql = strsql & " ," & strPendHdrTblName & " ph"
        End If
        strsql = strsql & " where 1=1 "
        If intItemDocID > 0 Then
            strsql = strsql & " And d.UOMID = u.UOMID "
            strsql = strsql & " And d.ItemID = i.ItemID "
            strsql = strsql & " And d.ItemID = d." & strFldItemID
        End If
        If blnLinkedDocFrom Then
            strsql = strsql & " And d." & strPendIDFldName & " = ph." & strPendIDFldName
        End If
        'If intItemDocID = 0 Then
        strsql = strsql & " And  " & strDocIDFldName & "= " & argDocID
        'End If
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
                If intItemDocID > 0 Then
                    vsfDet.SetData(i, vDetItemID, rsDtl(strFldItemID))
                    vsfDet.SetData(i, vDetItemCode, rsDtl(strFldItemCode))
                    vsfDet.SetData(i, vDetItemName, rsDtl(strFldItemName))

                    vsfDet.SetData(i, vDetUOM, !UOMCode)
                    vsfDet.SetData(i, vDetConvFact, IIf(Val(!ConvFact) = 0, 1, !ConvFact))
                End If

                For intC As Int32 = 0 To .FieldCount - 1
                    Select Case UCase(.GetName(intC))
                        Case "SHORTCLOSED", "SHORTCLOSEDDT", "SHORTCLOSEDBY"
                            vsfDet.SetData(i, vDetShortClosed, rsDtl("ShortClosed"))
                            vsfDet.SetData(i, vDetShortClosedDT, rsDtl("ShortClosedDT"))
                            vsfDet.SetData(i, vDetShortClosedBy, clsSVT.getDataStringValue("Select Username from MstUser where UserID=" & GetVal(rsDtl("ShortClosedBY"), 0), "", False))
                            'vsfDet.SetData(i, vDetShortClosedBy, clsSVT.getDataStringValue("Select Username from MstUser where UserCode=" & GetVal(rsDtl("ShortClosedBY"), 0), "", False))
                            vsfDet.SetData(i, vDetShortClosedQty, rsDtl("ShortClosedQty"))
                            Exit For
                    End Select
                Next

                If dtCumQty.Rows.Count > 0 Then
                    strsql = "Select Max(CumFld) as CumFld From ( "
                    For k As Int32 = 0 To dtCumQty.Rows.Count - 1
                        strsql &= " Select IsNull(" & dtCumQty.Rows(k).Item(1) & ",0) as CumFld "
                        strsql &= " From " & strDocDtlTblName
                        strsql &= " Where " & strDocIDFldName & " = " & rsDtl(strDocIDFldName)
                        strsql &= " And ItemSlno = " & rsDtl("ItemSlno")
                        strsql &= " UNION"

                        vsfDet.SetData(i, intDetCols + k - dtCumQty.Rows.Count, rsDtl.Item(dtCumQty.Rows(k).Item(1)))
                    Next
                    strsql = Mid(strsql, 1, Len(strsql) - 5)
                    strsql &= " ) a "
                    vsfDet.SetData(i, vDetDocCumQty, clsSVT.getDataDoubleValue(strsql, 0, False))
                End If

                If blnLinkedDocFrom Then
                    vsfDet.SetData(i, vDetSrcDocID, rsDtl(strPendIDFldName))
                    vsfDet.SetData(i, vDetSrcDocNo, rsDtl(strPendNoFldName))
                    vsfDet.SetData(i, vDetSrcDocQty, rsDtl("PQty"))
                    vsfDet.SetData(i, vDetSrcItemSlNo, rsDtl!ItemSLNO)
                    vsfDet.SetData(i, vDetCumQtyLinkedDoc, GetVal(rsDtl("CQty"), 0))
                    vsfDet.SetData(i, vDetPendQty, GetVal(rsDtl("PQty"), 0) - GetVal(rsDtl("CQty"), 0))
                End If
                Call FillvsfOptDtlFlds(Me, vsfDet, vsfOptions, rsDtl, i, intDetCols)
            Loop
            .Close()
            If intItemDocID > 0 Then
                SetHideCol(vsfDet, True, vDetShortClosed, vDetShortClosedBy, vDetShortClosedDT, vDetShortClosedQty)
                For i = 1 To vsfDet.Rows.Count - 1
                    If (vsfDet.GetData(i, vDetShortClosed)) = "Y" Then
                        SetHideCol(vsfDet, False, vDetShortClosed, vDetShortClosedBy, vDetShortClosedDT, vDetShortClosedQty)
                    End If
                Next
            End If
        End With
        If intItemDocID > 0 Then
            Call RemoveEmptVSFRows(vsfDet, 2, vDetItemCode)
        End If
        Call ReArrangeSlNo(vsfDet, vDetSlNo)

        If strAddReduceStock <> "" AndAlso intColStkQty > -1 Then
            Dim intLocID As Int32 = clsSVT.getDataLongValue("Select LocID from " & strDocHdrTblName & " Where " & strDocIDFldName & " = " & Val(txtDocID.Text), 0, False)

            With vsfDet
                Dim dblQOH As Double = 0
                For i = 1 To .Rows.Count - 1
                    dblQOH = clsSVT.getDataDoubleValue("Select CLBal from TrnQOH where LocID = " & intLocID & " and ItemID = " & Val(.GetData(i, vDetItemID)), 0, False)
                    Dim dblInvQty As Double = 0
                    For k = 1 To .Rows.Count - 1
                        If Val(.GetData(i, vDetItemID)) = Val(.GetData(k, vDetItemID)) Then
                            dblInvQty = dblInvQty + Val(.GetData(k, intColStkQty))
                        End If
                    Next
                    If strAddReduceStock = "ADD" Then
                        .SetData(i, vDetQOH, dblQOH)
                    Else
                        .SetData(i, vDetQOH, dblQOH + dblInvQty)
                    End If
                Next
            End With
        End If

        Call CalTotal()

        fillDtlFields = True
    End Function

    Private Sub bindPartyCombo()

    End Sub

    Private Sub prntCrpt()
        Dim frmv As New frmUtlReportViewer
        With frmv
            ' .MdiParent = frmMain
            .StartPosition = FormStartPosition.CenterScreen
            .ShowRptFileFromUtlOptions(intDocID, Val(txtDocID.Text), "", True)
            .Show()
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
            If UCase(strAddReduceStock) = "ADD" Or UCase(strAddReduceStock) = "RED" Then
                strSQL = " Delete From UtlStockList Where DocType='" & strSHForm & "' and DocID=" & Val(txtDocID.Text)
                strErrMsg = clsSVT.execSQLStmtMsg(strSQL, svtTrn)
                If strErrMsg <> "" Then
                    If InStr(UCase(strErrMsg), "CHECK CONSTRAINT") > 0 Then
                        MsgBox("Stock Is going negative. Cannot Save.", MsgBoxStyle.Critical, "No Stock")
                    Else
                        MsgBox(strErrMsg & " Error during updating stock", MsgBoxStyle.Critical, "No Stock")
                    End If
                    svtTrn.Rollback()
                    Exit Sub
                End If
            End If


            If blnLinkedDocFrom Then
                strSQL = "Update PT  Set PT." & strSHForm & "CumQty = PT." & strSHForm & "CumQty - d." & strQtyFldName
                strSQL &= " from " & strDocDtlTblName & " d,  " & strPendDtlTblName & " PT "
                strSQL &= " Where d." & strPendDocShForm & "ItemSlno=PT.ItemSlno  "
                If intItemDocID > 0 Then strSQL &= " And d.ItemId=PT.ItemID  "
                strSQL &= " And d." & strPendIDFldName & " = PT." & strPendIDFldName & " "
                strSQL &= " And d." & strDocIDFldName & " = " & Val(txtDocID.Text)

                If Not clsSVT.execSQLStatement(strSQL, svtTrn) Then
                    svtTrn.Rollback()
                    Exit Sub
                End If
            End If

            'for audit track
            If blnAuditTrack Then
                Call RecordAuditLog(strFrmCaption, Val(txtDocID.Text), UserActions.DeleteAction, strDocHdrTblName, strAuditHdrSQL, strAuditDtlSQL, svtTrn)
            End If

            If strType = "DELETED" Then
                If blnDetSection Then
                    clsSVT.execSQLStatement("Delete from " & strDocDtlTblName & " where " & strDocIDFldName & " = " & Val(txtDocID.Text), svtTrn)
                End If
                clsSVT.execSQLStatement("Delete from " & strDocHdrTblName & " where " & strDocIDFldName & " = " & Val(txtDocID.Text), svtTrn)
            Else
                clsSVT.execSQLStatement("Update " & strDocHdrTblName & " Set Cancelled = 'Y' where " & strDocIDFldName & " = " & Val(txtDocID.Text), svtTrn)
            End If

            svtTrn.Commit()
            If strType = "DELETED" Then Call clearFlds()

            MsgBox(IIf(strType = "CANCELLED", "Cancelled", "Deleted") & Me.Text & " Successfully!", MsgBoxStyle.Critical, "Deleted")
        Catch ex As Exception
            svtTrn.Rollback()
            MsgBox(ex.Message, MsgBoxStyle.Information, "Cannot " & strType)
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

    Public Function gridEmpty() As Boolean
        gridEmpty = False
        If Not (Mode = "A" Or Mode = "M") Then Exit Function
        If Not blnDetSection Then Exit Function
        If intItemDocID > 0 Then
            If vsfDet.Rows.Count > 1 AndAlso vsfDet.Rows.Count = 2 AndAlso Trim(GetVal(vsfDet.GetData(1, vDetItemCode), "")) = "" Then
                gridEmpty = True
            End If
        Else
            If vsfDet.Rows.Count > 1 AndAlso vsfDet.Rows.Count = 2 AndAlso Trim(GetVal(vsfDet.GetData(1, intDetNextEntryCol), "")) = "" Then
                gridEmpty = True
            End If
        End If

    End Function

    'Public Sub fillCopyFromDoc(ByVal argDocNo As String, ByVal argDocId As Integer, ByVal argHdrTbl As String, ByVal argDtlTbl As String, ByVal argDocNoFld As String, ByVal argDocIdFld As String, ByVal argCustSuppReq As String, ByVal argDocSHForm As String, ByVal blnAppend As Boolean)
    '    Dim strSql As String = ""
    '    Dim rs As oledb.oledbDataReader
    '    Dim strItemCode As String
    '    Dim intDetColCount As Integer = 0
    '    Dim intItemID As Integer = 0

    '    strSql = "Select * from " & argHdrTbl & " where " & argDocNoFld & " = '" & argDocNo & "'"
    '    rs = clsSVT.getDataReaderFromSQL(strSql)

    '    With rs
    '        Do While .Read()
    '            For i = 0 To .FieldCount - 1
    '                'Fill Values into Designer grid for matchin fieldnames
    '                Dim ctrlList = GetAllDsgrVsf(Me, GetType(C1.Win.C1FlexGrid.C1FlexGrid))

    '                For Each argVsf As C1FlexGrid In ctrlList
    '                    With argVsf
    '                        For j = 0 To .Rows.Count - 1
    '                            If .GetData(j, vOthPerAmount) = "Y" Then
    '                                If UCase(rs.GetName(i)) = UCase(Trim(.GetData(j, vOthFldCode))) & "PER" Then
    '                                    .SetData(j, vOthFldDataPer, GetVal(rs.GetValue(i), ""))
    '                                End If
    '                                If UCase(rs.GetName(i)) = UCase(Trim(.GetData(j, vOthFldCode))) & "AMT" Then
    '                                    .SetData(j, vOthFldDataValue, GetVal(rs.GetValue(i), ""))
    '                                End If
    '                            End If

    '                            If .GetData(j, vOthPerAmount) = "N" Then
    '                                If UCase(rs.GetName(i)) = UCase(Trim(.GetData(j, vOthFldCode))) Then
    '                                    If Trim(.GetData(j, vOthFldCmbTblName)) = "" Then
    '                                        .SetData(j, vOthFldDataValue, GetVal(rs.GetValue(i), ""))
    '                                    Else
    '                                        .SetData(j, vOthFldDataValue,
    '                               clsSVT.getDataStringValue("Select " & .GetData(j, vOthFldCmbDisplayCols) & " from " &
    '                                .GetData(j, vOthFldCmbTblName) & " where " &
    '                                .GetData(j, vOthFldCmbStorageFldName) & " = " & GetVal(rs.GetValue(i), 0), "", False))
    '                                    End If
    '                                    Exit For
    '                                End If
    '                            End If
    '                        Next j

    '                    End With
    '                Next

    '            Next i
    '        Loop
    '        .Close()
    '        strSql = ""

    '        'fill detail section
    '        strSql = "Select * from " & argDtlTbl & " where " & argDocIdFld & " = " & argDocId & ""
    '        rs = clsSVT.getDataReaderFromSQL(strSql)

    '        For i = 0 To vsfDet.Cols.Count - 1
    '            If vsfDet.Cols(i).Visible Then
    '                intDetColCount += 1
    '            End If
    '        Next

    '        Dim l% = 1

    '        If Not blnAppend Then
    '            vsfDet.Rows.Count = 1
    '        ElseIf blnAppend And vsfDet.Rows.Count = 2 Then
    '            If vsfDet.GetData(1, vDetItemCode) = "" Then
    '                vsfDet.Rows.Count = 1
    '            Else
    '                vsfDet.Rows.Count = 2
    '                l = vsfDet.Rows.Count
    '            End If
    '        Else
    '            vsfDet.Rows.Count = vsfDet.Rows.Count
    '            l = vsfDet.Rows.Count
    '        End If

    '        Dim dt As New DataTable()
    '        dt.Load(rs)

    '        For Each rv As DataRow In dt.Rows
    '            Dim j = 0
    '            vsfDet.Rows.Count = vsfDet.Rows.Count + 1
    '            For i = 0 To rv.ItemArray.Length - 1
    '                For k = 0 To vsfDet.Cols.Count - 1
    '                    If UCase(Trim(vsfOptions.GetData(k, vOthFldCode))) = UCase(Trim(dt.Columns(j).ToString())) Then
    '                        vsfDet.SetData(vsfDet.Rows.Count - 1, k, GetVal(rv.ItemArray(i).ToString(), ""))
    '                        Exit For
    '                    End If
    '                Next k
    '                If UCase(Trim(dt.Columns(j).ToString())) = strFldItemID Then
    '                    intItemID = Convert.ToInt32(rv.ItemArray(i).ToString())
    '                    strItemCode = clsSVT.getDataStringValue("Select " & strFldItemCode & " from " & strItemTblName & " where " & strFldItemID & " = " & intItemID, 0, False)
    '                    FillItemDet(strItemCode, 0, l)
    '                End If

    '                'If UCase(Trim(dt.Columns(j).ToString())) = strFldItemID Then
    '                '    intItemID = Convert.ToInt32(rv.ItemArray(i).ToString())
    '                '    strItemCode = clsSVT.getDataStringValue("Select " & strFldItemCode & " from " & strItemTblName & " where " & strFldItemID & " = " & intItemID, 0, False)
    '                '    FillItemDet(strItemCode, 0, l)
    '                '    'ElseIf UCase(Trim(dt.Columns(j).ToString())) = UCase(Trim(argDocSHForm & "QTY")) Then
    '                '    'vsfDet.SetData(l, intColQty, GetVal(rv.ItemArray(i).ToString(), ""))
    '                'Else
    '                '    For k = 0 To vsfDet.Cols.Count - 1
    '                '        If UCase(Trim(vsfDet.GetData(0, k))).Replace(" ", "") = UCase(Trim(dt.Columns(j).ToString())).Replace(" ", "") And vsfDet.Cols(k).Visible = True Then
    '                '            vsfDet.SetData(l, k, GetVal(rv.ItemArray(i).ToString(), ""))
    '                '            Exit For
    '                '        End If
    '                '    Next k
    '                'End If
    '                j = j + 1
    '            Next i
    '            l = l + 1
    '        Next

    '        Call CalTotal()

    '        If vsfHdr.Rows.Count > 0 Then
    '            Call vsfGenAfterEditBfrCalTot(Me, vsfHdr, vsfHdr.Rows.Count - 1, "T")
    '        End If

    '        If vsfFtr.Rows.Count > 0 Then
    '            Call vsfGenAfterEditBfrCalTot(Me, vsfFtr, vsfFtr.Rows.Count - 1, "T")
    '        End If

    '        If vsfTot.Rows.Count > 0 Then
    '            Call vsfGenAfterEditBfrCalTot(Me, vsfTot, vsfTot.Rows.Count - 1, "T")
    '        End If

    '        ReArrangeSlNo(vsfDet, vDetSlNo)
    '    End With
    'End Sub

    Public Sub fillCopyFromDoc(ByVal argDocNo As String, ByVal argDocId As Integer, ByVal argHdrTbl As String, ByVal argDtlTbl As String, ByVal argDocNoFld As String, ByVal argDocIdFld As String, ByVal argCustSuppReq As String, ByVal argDocSHForm As String, ByVal blnAppend As Boolean)
        Dim strSql As String = ""
        Dim rs As oledb.oledbDataReader
        Dim strItemCode As String
        Dim intDetColCount As Integer = 0
        Dim intItemID As Integer = 0

        strSql = "Select * from " & argHdrTbl & " where " & argDocNoFld & " = '" & argDocNo & "'"
        rs = clsSVT.getDataReaderFromSQL(strSql)

        With rs
            Do While .Read()
                For i = 0 To .FieldCount - 1
                    'Fill Values into Designer grid for matchin fieldnames
                    Dim ctrlList = GetAllDsgrVsf(Me, GetType(C1.Win.C1FlexGrid.C1FlexGrid))

                    For Each argVsf As C1FlexGrid In ctrlList
                        With argVsf
                            For j = 0 To .Rows.Count - 1
                                If .GetData(j, vOthPerAmount) = "Y" Then
                                    If UCase(rs.GetName(i)) = UCase(Trim(.GetData(j, vOthFldCode))) & "PER" Then
                                        .SetData(j, vOthFldDataPer, GetVal(rs.GetValue(i), ""))
                                    End If
                                    If UCase(rs.GetName(i)) = UCase(Trim(.GetData(j, vOthFldCode))) & "AMT" Then
                                        .SetData(j, vOthFldDataValue, GetVal(rs.GetValue(i), ""))
                                    End If
                                End If

                                If .GetData(j, vOthPerAmount) = "N" Then
                                    If UCase(rs.GetName(i)) = UCase(Trim(.GetData(j, vOthFldCode))) Then
                                        If Trim(.GetData(j, vOthFldCmbTblName)) = "" Then
                                            .SetData(j, vOthFldDataValue, GetVal(rs.GetValue(i), ""))
                                        Else
                                            .SetData(j, vOthFldDataValue,
                                   clsSVT.getDataStringValue("Select " & .GetData(j, vOthFldCmbDisplayCols) & " from " &
                                    .GetData(j, vOthFldCmbTblName) & " where " &
                                    .GetData(j, vOthFldCmbStorageFldName) & " = " & GetVal(rs.GetValue(i), 0), "", False))
                                        End If
                                        Exit For
                                    End If
                                End If
                            Next j

                        End With
                    Next

                Next i
            Loop
            .Close()
            strSql = ""

            'fill detail section
            strSql = "Select * from " & argDtlTbl & " where " & argDocIdFld & " = " & argDocId & ""
            rs = clsSVT.getDataReaderFromSQL(strSql)

            For i = 0 To vsfDet.Cols.Count - 1
                If vsfDet.Cols(i).Visible Then
                    intDetColCount += 1
                End If
            Next

            Dim l% = 1

            If Not blnAppend Then
                vsfDet.Rows.Count = 1
            ElseIf blnAppend And vsfDet.Rows.Count = 2 Then
                If vsfDet.GetData(1, vDetItemCode) = "" Then
                    vsfDet.Rows.Count = 1
                Else
                    vsfDet.Rows.Count = 2
                    l = vsfDet.Rows.Count
                End If
            Else
                vsfDet.Rows.Count = vsfDet.Rows.Count
                l = vsfDet.Rows.Count
            End If

            Dim dt As New DataTable()
            dt.Load(rs)

            For Each rv As DataRow In dt.Rows
                Dim j = 0
                vsfDet.Rows.Count = vsfDet.Rows.Count + 1
                For i = 0 To rv.ItemArray.Length - 1
                    For k = 0 To vsfDet.Cols.Count - 1
                        If UCase(Trim(vsfOptions.GetData(k, vOthFldCode))) = UCase(Trim(dt.Columns(j).ToString())) Then
                            If Trim(vsfOptions.GetData(k, vOthFldCmbTblName)) = "" Then
                                vsfDet.SetData(vsfDet.Rows.Count - 1, k, GetVal(rv.ItemArray(i).ToString(), ""))
                                Exit For
                            Else
                                Try
                                    vsfDet.SetData(vsfDet.Rows.Count - 1, k,
                                   clsSVT.getDataStringValue("Select " & vsfOptions.GetData(k, vOthFldCmbDisplayCols) _
                                       & " from " & vsfOptions.GetData(k, vOthFldCmbTblName) _
                                       & " where " & vsfOptions.GetData(k, vOthFldCmbStorageFldName) _
                                       & " = " & GetVal(rv.ItemArray(i), 0), "", False))
                                Catch ex As Exception
                                End Try
                                Exit For
                            End If
                        End If
                    Next k
                    If UCase(Trim(dt.Columns(j).ToString())) = strFldItemID Then
                        intItemID = Convert.ToInt32(rv.ItemArray(i).ToString())
                        strItemCode = clsSVT.getDataStringValue("Select " & strFldItemCode & " from " & strItemTblName & " where " & strFldItemID & " = " & intItemID, 0, False)
                        FillItemDet(strItemCode, vsfDet.Rows.Count - 1, "")
                    End If

                    'If UCase(Trim(dt.Columns(j).ToString())) = strFldItemID Then
                    '    intItemID = Convert.ToInt32(rv.ItemArray(i).ToString())
                    '    strItemCode = clsSVT.getDataStringValue("Select " & strFldItemCode & " from " & strItemTblName & " where " & strFldItemID & " = " & intItemID, 0, False)
                    '    FillItemDet(strItemCode, 0, l)
                    '    'ElseIf UCase(Trim(dt.Columns(j).ToString())) = UCase(Trim(argDocSHForm & "QTY")) Then
                    '    'vsfDet.SetData(l, intColQty, GetVal(rv.ItemArray(i).ToString(), ""))
                    'Else
                    '    For k = 0 To vsfDet.Cols.Count - 1
                    '        If UCase(Trim(vsfDet.GetData(0, k))).Replace(" ", "") = UCase(Trim(dt.Columns(j).ToString())).Replace(" ", "") And vsfDet.Cols(k).Visible = True Then
                    '            vsfDet.SetData(l, k, GetVal(rv.ItemArray(i).ToString(), ""))
                    '            Exit For
                    '        End If
                    '    Next k
                    'End If
                    j = j + 1
                Next i
                l = l + 1
            Next

            Call CalTotal()

            If vsfHdr.Rows.Count > 0 Then
                Call vsfGenAfterEditBfrCalTot(Me, vsfHdr, vsfHdr.Rows.Count - 1, "T")
            End If

            If vsfFtr.Rows.Count > 0 Then
                Call vsfGenAfterEditBfrCalTot(Me, vsfFtr, vsfFtr.Rows.Count - 1, "T")
            End If

            If vsfTot.Rows.Count > 0 Then
                Call vsfGenAfterEditBfrCalTot(Me, vsfTot, vsfTot.Rows.Count - 1, "T")
            End If

            ReArrangeSlNo(vsfDet, vDetSlNo)
        End With
    End Sub

    Private Sub frmTrnPartyDoc_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        strGCustCode = clsSVT.getDataStringValue("Select Top 1 CustCode From MstComp", "", False)
        Me.KeyPreview = True
        Call ReSizeHdrFtr()
        Dim ToolTip As New ToolTip
        Dim blnNewMode As Boolean = False, blnLoadLastTrn As Boolean = False

        If blnDetSection Then
            Call initFormVsfDet(Me, Me.Text, vsfDet, "DET")
            Call initFormVsfDet(Me, Me.Text, vsfTop, "TOP")
            Call initFormVsfDet(Me, Me.Text, svtPCtrl.vsfView, "PVIEW")
        End If
        Dim argDsgrGrids = GetAllDsgrVsf(Me, GetType(C1FlexGrid))
        For Each GridList As C1FlexGrid In argDsgrGrids
            If GridList.Visible Then
                Call initFormVsfDet(Me, Me.Text, GridList, GridList.Name)
            End If
        Next
        initGridStyles(Me)
        lblTitle.Text = "View - " & Me.Text
        Call getCtrlSize()
        srcTransaction.Visible = True
        Call srcTransaction.setFocus()
        Call GridPinVisible()
        If intPartyDocID <= 0 Then
            Call NonPartyItemFormResize()
        End If
        Call setFormSetttings()
        fillFormProperties(Me)
        lblTitle.ForeColor = Color.White
        srcTransaction.lblParty.ForeColor = Color.White
        vsfTop.Enabled = False
        If Not blnDetSection Then
            btnDetPin.Visible = False
            ucBtnTools.Visible = False
        End If
        If Not vsfTot.Visible Then
            lblTotTip.Visible = False
        End If
        svtPCtrl.lblParty.ForeColor = Color.Maroon
        Call GridStylesUpdateFromDocDsgr()
        Call RemoveOldXML()
        Call svtBtnCtrl.reSizeBtnCtrl()
        srcTransaction.setFocus()
        Call LoadLastOrNewTrn(intDocID, blnNewMode, blnLoadLastTrn)
        If blnNewMode Then
            blnFirstTime = True
            svtBtnCtrl_btnClicked(svtBtns.svtNew)
            If intPartyDocID > 0 Then
                svtPCtrl.txtPartyCode.Focus()
            End If
        ElseIf blnLoadLastTrn Then
            If lngViewDocID = 0 Then
                strSQL = "Select max(" & getSyntax("ISNULL0", strDocIDFldName) & ") from " & strDocHdrTblName
            Else
                strSQL = "Select " & getSyntax("ISNULL0", strDocIDFldName) & " from " & strDocHdrTblName & " Where " & strDocIDFldName & " = " & lngViewDocID
            End If
            Call FillDocDetails(clsSVT.getDataLongValue(strSQL, 0, False))
        End If
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

    Private Sub chkAmmendment_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkAmmendment.CheckedChanged
        dtAmmDate.Value = getSystemDate()
        If Mode <> "A" And Mode <> "M" Then Exit Sub
        If chkAmmendment.Checked = False And Val(txtAmmNo.Text) < 1 Then
            txtAmmNo.Text = ""
        ElseIf chkAmmendment.Checked = True Then
            txtAmmNo.Text = clsSVT.getDataLongValue("Select " & getSyntax("ISNULL", "AmmNo") & " + 1 from " & strDocHdrTblName & " where " & strDocIDFldName & " = " & Val(txtDocID.Text), 1, False)
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
        If intPartyDocID > 0 Then
            svtPCtrl.Focus()
            svtPCtrl.txtPartyCode.Focus()
        Else
            If vsfDet.Visible Then vsfDet.Focus()
        End If

    End Sub

    Private Sub svtBtnCtrl_btnOptionSelected(argOptionName As String, argBlnCtrlClick As Boolean, argBlnAltClick As Boolean) Handles svtBtnCtrl.btnOptionSelected
        If argOptionName = "" Then Exit Sub
        'If Val(txtDocID.Text) <= 0 Then Exit Sub

        Select Case argOptionName
            Case "Day's OP-Balance"
                Dim objfrmExp As New frmUtlDaysExpOPBalance
                objfrmExp.dtDocDate.Value = getSystemDate()
                objfrmExp.txtOPAmt.Text = clsSVT.getDataDoubleValue("Select OPBalAmt from TrnExpOpBal where ExpDate = " & getDateFormatInCond(getSystemDate), 0, False)
                objfrmExp.ShowDialog()
                objfrmExp.Dispose()
            Case "Envelope Print"
                Dim objfrm As New frmUtlEnv
                objfrm.strDocName = Me.Text
                objfrm.intPartyId = Val(txtDocID.Text)
                objfrm.ShowDialog()
                objfrm.Dispose()
            Case "SEND SMS"
                With New frmTrnSMS
                    .intDocId = intDocID
                    .initScreen(txtDocID.Text)
                    .ShowDialog()
                End With
            Case "SEND EMAIL"
                With New frmTrnEmail
                    .intDocId = intDocID
                    .initScreen(txtDocID.Text)
                    .ShowDialog()
                End With
            Case "Label Print"
                Dim objfrm As New frmRptLblPrint
                objfrm.intDocID = intDocID
                objfrm.ShowDialog()
                objfrm.Dispose()
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

                'Dim frmv As New frmUtlReportViewer
                'With frmv
                '    .StartPosition = FormStartPosition.CenterScreen
                '    .ShowRptFileFromUtlOptions_New(intDocID, Val(txtDocID.Text), argOptionName, argBlnCtrlClick, argFileName:=Strings.Replace(Me.Text, " ", "") & "_" & Trim(txtDocNo.Text))
                '    If strClickEvent = "P" Then
                '        .Show()
                '    End If
                'End With

                Dim frmv As New frmUtlReportViewer
                With frmv
                    .StartPosition = FormStartPosition.CenterScreen
                    Dim strfname As String = strSHForm & "_" & Trim(txtDocNo.Text) & "_" & Date.Now.ToString("yyyyMMddHHmmssfff")
                    .ShowRptFileFromUtlOptions_New(intDocID, Val(txtDocID.Text), argOptionName, argBlnCtrlClick, argFileName:=strfname)
                    If strClickEvent = "P" Then
                        If blnPDFPreview Then
                            Process.Start(Application.StartupPath & "\Temp\" & strfname & ".pdf")
                        Else
                            .WindowState = FormWindowState.Maximized
                            .Show()
                        End If
                    End If
                End With


        End Select
    End Sub

    Private Sub PrintAfterSave(ByVal argOptionName As String)

        'If Not chkUserRights(strRights, chkRightsPos.PrintRights, Me.Text) Then Exit Sub
        'Dim strPdfPath As String = Application.StartupPath & "\Temp\" & txtDocNo.Text & "_" & Date.Now.ToString("yyyyMMddHHmmssfff") & ".pdf"
        'Dim blnPreview As Boolean
        'If blnDefPrint And argOptionName = strPrintOption Then
        '    Select Case Trim(strPrintAfterSave)
        '        Case "P"
        '            If MsgBox("Do You want to Print the Document...., Click Yes to Print..", MsgBoxStyle.YesNo, Me.Text) = MsgBoxResult.No Then
        '                Exit Sub
        '            End If
        '        Case "DNP"
        '            Exit Sub
        '    End Select
        '    If UCase(Trim(strPrintType)) = "PREVIEW" Then blnPreview = True
        'End If

        Dim frmv As New frmUtlReportViewer
        'Using frmv
        '    With frmv
        '        .StartPosition = FormStartPosition.CenterScreen
        '        If blnDefPrint And argOptionName = strPrintOption Then
        '            .ShowRptFileFromUtlOptions(intDocID, Val(txtDocID.Text), argOptionName, False,
        '              blnOutlook, intNoOfCopies, strSelFormula, strAdnlFormula, strArrFormulaFld, strArrFormulaVal, strMultiRpt,
        '              strPrinter, strPrintType, IIf(strPrintType = "PDF", True, False), strPdfPath)
        '        Else
        '            .ShowRptFileFromUtlOptions(intDocID, Val(txtDocID.Text), argOptionName, False)
        '        End If
        '        .ShowDialog()
        '    End With
        'End Using
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
            If strAddReduceStock <> "" AndAlso intRowLoc > -1 Then
                intLocID = clsSVT.getDataLongValue("Select LocID from MstLoc Where LocName='" & vsfHdr.GetData(intRowLoc, vOthFldDataValue) & "'", 1, False, svtTrn)
            End If


            If Mode = "M" Then
                intMaxDocID = Val(txtDocID.Text)
                If UCase(strAddReduceStock) = "ADD" Or UCase(strAddReduceStock) = "RED" Then
                    intOldLocID = clsSVT.getDataLongValue("Select LocID from " & strDocHdrTblName & " Where " & strDocIDFldName & "=" & intMaxDocID, 1, False, svtTrn)

                    'strSQL = " Insert Into UtlStockList "
                    'strSQL &= " (ItemID, StkMultiplier, DocType, DocDate, DocID, DocNo, Particulars, Qty, LtrQty, Rate, DelFlag) "
                    'strSQL &= " Select ItemID, '"
                    'strSQL &= IIf(UCase(strAddReduceStock) = "ADD", "-1", "1") & "', '" & strSHForm & "' , " & strSHForm & "Date, "
                    'strSQL &= "h." & strSHForm & "ID, " & strSHForm & "NO, "
                    'strSQL &= IIf(intPartyDocID > 0, "PartyName", "''") & ", " & strSHForm & "Qty, 0, "
                    'strSQL &= IIf(intColrate > 0, strSHForm & "Rate ", "0") & ", 'D'"
                    'strSQL &= " From TrnDtl" & strSHForm & " d, TrnHdr" & strSHForm & " h "
                    'strSQL &= " Where d." & strSHForm & "ID=h." & strSHForm & "ID "
                    'strSQL &= " And d." & strSHForm & "ID=" & intMaxDocID
                    'strErrMsg = clsSVT.execSQLStmtMsg(strSQL, svtTrn)
                    'If strErrMsg <> "" Then
                    '    SaveDoc = False
                    '    If InStr(UCase(strErrMsg), "CHECK CONSTRAINT") > 0 Then
                    '        MsgBox("Stock Is going negative. Cannot Save.", MsgBoxStyle.Critical, "No Stock")
                    '    Else
                    '        MsgBox(strErrMsg & " Error during updating stock", MsgBoxStyle.Critical, "No Stock")
                    '    End If
                    '    svtTrn.Rollback()
                    '    Exit Function
                    'End If

                    strSQL = " UPDATE UtlStockList SET DELFLAG = 'D' Where DocType='" & strSHForm & "' and DocID=" & intMaxDocID
                    clsSVT.execSQLStatement(strSQL, svtTrn)
                End If

                If blnLinkedDocFrom Then
                    strSQL = "Update PT  Set PT." & strSHForm & "CumQty = PT." & strSHForm & "CumQty - d." & strQtyFldName
                    strSQL &= " from " & strDocDtlTblName & " d,  " & strPendDtlTblName & " PT "
                    strSQL &= " Where d." & strPendDocShForm & "ItemSlno=PT.ItemSlno  "
                    If intItemDocID > 0 Then strSQL &= " And d.ItemId=PT.ItemID  "
                    strSQL &= " And d." & strPendIDFldName & " = PT." & strPendIDFldName & " "
                    strSQL &= " And d." & strDocIDFldName & " = " & Val(intMaxDocID)

                    If Not clsSVT.execSQLStatement(strSQL, svtTrn) Then
                        SaveDoc = False
                        svtTrn.Rollback()
                        Exit Function
                    End If
                End If
                If blnDetSection Then
                    clsSVT.execSQLStatement("Delete from trndtl" & strSHForm & " where " & strDocIDFldName & " = " & Val(txtDocID.Text), svtTrn)
                End If
            End If
            If Not UpdtDocHdrDtl() Then
                SaveDoc = False
                svtTrn.Rollback()
                Exit Function
            End If

            If UCase(strAddReduceStock) = "ADD" Or UCase(strAddReduceStock) = "RED" Then
                strSQL = " Insert Into UtlStockList "
                strSQL &= " (ItemID, StkMultiplier, DocType, DocDate, DocID, DocNo, Particulars, Qty, LtrQty, Rate) "
                strSQL &= " Select ItemID, '"
                strSQL &= IIf(UCase(strAddReduceStock) = "ADD", "1", "-1") & "', '" & strSHForm & "' , " & strSHForm & "Date, "
                strSQL &= "h." & strSHForm & "ID, " & strSHForm & "NO, "
                strSQL &= IIf(intPartyDocID > 0, "PartyName", "''") & ", " & strSHForm & "Qty, 0, "
                strSQL &= IIf(intColrate > 0, strSHForm & "Rate ", "0")
                strSQL &= " From TrnDtl" & strSHForm & " d, TrnHdr" & strSHForm & " h "
                strSQL &= " Where d." & strSHForm & "ID=h." & strSHForm & "ID "
                strSQL &= " And d." & strSHForm & "ID=" & intMaxDocID
                strErrMsg = clsSVT.execSQLStmtMsg(strSQL, svtTrn)
                If strErrMsg <> "" Then
                    SaveDoc = False
                    If InStr(UCase(strErrMsg), "CHECK CONSTRAINT") > 0 Then
                        MsgBox("Stock Is going negative. Cannot Save.", MsgBoxStyle.Critical, "No Stock")
                    Else
                        MsgBox(strErrMsg & " Error during updating stock", MsgBoxStyle.Critical, "No Stock")
                    End If
                    svtTrn.Rollback()
                    Exit Function
                End If

                If Mode = "M" Then
                    strSQL = " DELETE FROM UtlStockList  Where DELFLAG='D' AND DocType='" & strSHForm & "' and DocID=" & intMaxDocID
                    strErrMsg = clsSVT.execSQLStmtMsg(strSQL, svtTrn)
                    If strErrMsg <> "" Then
                        If InStr(UCase(strErrMsg), "CHECK CONSTRAINT") > 0 Then
                            MsgBox("Stock Is going negative. Cannot Save.", MsgBoxStyle.Critical, "No Stock")
                        Else
                            MsgBox(strErrMsg & " Error during updating stock", MsgBoxStyle.Critical, "No Stock")
                        End If
                        SaveDoc = False
                        svtTrn.Rollback()
                        Exit Function
                    End If
                End If
            End If

            If blnLinkedDocFrom Then
                strSQL = "Update " & strPendDtlTblName & " Set " & strSHForm & "CumQty = (Select isnull(sum(" & strQtyFldName & "),0) "
                strSQL &= " from " & strDocDtlTblName & " d, " & strDocHdrTblName & " h "
                strSQL &= " Where " & strPendDtlTblName & ".ItemSlno=d." & strPendDocShForm & "ItemSlno "
                If intItemDocID > 0 Then strSQL &= " And " & strPendDtlTblName & ".ItemId=d.ItemID  "
                strSQL &= " And d." & strDocIDFldName & " = h." & strDocIDFldName & " "
                strSQL &= " And h.Cancelled = 'N' "
                strSQL &= " And " & strPendDtlTblName & "." & strPendIDFldName & " = d." & strPendIDFldName & ")"
                strSQL &= " Where " & strPendIDFldName & " in (Select " & strPendIDFldName & " from " & strDocDtlTblName & " where " & strDocIDFldName & " = " & Val(txtDocID.Text) & ") "
                If Not clsSVT.execSQLStatement(strSQL, svtTrn) Then
                    SaveDoc = False
                    svtTrn.Rollback()
                    Exit Function
                End If
            End If

            'Accounting Code BEGIN
            If blnAccTrn Then
                Dim intCreditdays As Integer = 0
                If intRowCrDays >= 0 Then
                    intCreditdays = Val(getValueFromAddnlGrid("CREDITDAYS", vsfFtr))
                End If
                If Not UpdtTrnToAccVch(intMaxDocID, strSHForm, strDocHdrTblName, txtDocNo.Text, dtDocDate.Value, intCreditdays, strTrnDrCr, intAcLedID, dblAccValue, svtTrn) Then
                    SaveDoc = False
                    svtTrn.Rollback()
                    Exit Function
                End If
            End If
            'Accounting Code END

            'for audit track
            If blnAuditTrack Then
                Call RecordAuditLog(strFrmCaption, Val(txtDocID.Text), IIf(Mode = "A", UserActions.AddAction, UserActions.EditAction), strDocHdrTblName, strAuditHdrSQL, strAuditDtlSQL, svtTrn)
            End If

            svtTrn.Commit()
            If strGCustCode = "SMPBILL" And intDocID = 1256 Then
                Dim intFrom As Integer, intTO As Integer, intctr As Integer
                strSQL = "Delete from TrnDtlPLBOX where PLID = " & Val(txtDocID.Text)
                clsSVT.execSQLStatement(strSQL)

                For i = 1 To vsfDet.Rows.Count - 1
                    intFrom = Val(vsfDet.GetData(i, getColNoFromAddnlDetGrid(vsfOptions, vsfDet, "BOXFROM")))
                    intTO = Val(vsfDet.GetData(i, getColNoFromAddnlDetGrid(vsfOptions, vsfDet, "BOXTO")))
                    If intTO = 0 Then intTO = intFrom
                    For x = intFrom To intTO
                        intctr = intctr + 1
                        strSQL = "Insert into TrnDtlPLBOX(PLID, SlNo, ItemId, BoxNo) values ("
                        strSQL = strSQL & Val(txtDocID.Text) & ","
                        strSQL = strSQL & Val(vsfDet.GetData(i, vDetSlNo)) & ","
                        strSQL = strSQL & Val(vsfDet.GetData(i, vDetItemID)) & ","
                        strSQL = strSQL & x & ")"
                        clsSVT.execSQLStatement(strSQL)
                    Next x
                Next i
            End If

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
            If intPartyDocID > 0 Or blnManualPartySel Then
                If Not blnManualPartySel Then strSQL = strSQL & strFldPartyID & ", "
                If blnSavePartyData Then
                    strSQL = strSQL & "PartyName,PartyAdd1,PartyAdd2,PartyAdd3,PartyAdd4,PartyCITY,PartySTATE,PartyPINCODE,PartyCOUNTRY,PartyCName,PartyCMob,PartyCEmail,PartyCDesig,"
                End If
                If blnShipAddress Then
                    strSQL = strSQL & "PartySName,PartySAdd1,PartySAdd2,PartySAdd3,PartySAdd4,PartySCITY,PartySSTATE,PartySPINCODE,PartySCOUNTRY,PartySCName,PartySCMob,PartySCEmail,PartySCDesig,"
                End If
            End If
            strSQL = strSQL & " AddedBy, AddedOn,"
            If ucBtnTools.tsbAmmend.Visible Then
                strSQL = strSQL & " Ammendment, AmmDate, AmmNo,"
            End If
            strSQL = strSQL & " Authorised, Cancelled"

            Call GetInsertString(Me, strValues, strFlds, svtTrn)

            If gDBTypeCurrent = "ACCESS" Then
                strSQL = strSQL & strFlds & ") values ("
            Else
                strSQL = strSQL & strFlds & ") output inserted." & strDocIDFldName & " into @testTable values ("
            End If


            strSQL = strSQL & "'" & txtDocNo.Text & "', "
            strSQL = strSQL & "'" & cboPfx.Text & "', "
            strSQL = strSQL & "'" & getPrintDocFmtNo() & "', "
            strSQL = strSQL & "'" & getDateFormatInInsert(dtDocDate.Value) & "', "
            If intPartyDocID > 0 Or blnManualPartySel Then
                With svtPCtrl
                    If Not blnManualPartySel Then strSQL = strSQL & Val(svtPCtrl.getBillID()) & ", "
                    If blnSavePartyData Then
                        strSQL &= "'" & .getGridData(intRowPartyCtrlName, intColPartyCtrlBill) & "',"
                        strSQL &= "'" & .getGridData(intRowPartyCtrlAdd1, intColPartyCtrlBill) & "',"
                        strSQL &= "'" & .getGridData(intRowPartyCtrlAdd2, intColPartyCtrlBill) & "',"
                        strSQL &= "'" & .getGridData(intRowPartyCtrlAdd3, intColPartyCtrlBill) & "',"
                        strSQL &= "'" & .getGridData(intRowPartyCtrlAdd4, intColPartyCtrlBill) & "',"
                        strSQL &= "'" & .getGridData(intRowPartyCtrlCity, intColPartyCtrlBill) & "',"
                        strSQL &= "'" & .getGridData(intRowPartyCtrlState, intColPartyCtrlBill) & "',"
                        strSQL &= "'" & .getGridData(intRowPartyCtrlPincode, intColPartyCtrlBill) & "',"
                        strSQL &= "'" & .getGridData(intRowPartyCtrlCountry, intColPartyCtrlBill) & "',"
                        strSQL &= "'" & .getGridData(intRowPartyCtrlCName, intColPartyCtrlBill) & "',"
                        strSQL &= "'" & .getGridData(intRowPartyCtrlCMob, intColPartyCtrlBill) & "',"
                        strSQL &= "'" & .getGridData(intRowPartyCtrlCEmail, intColPartyCtrlBill) & "',"
                        strSQL &= "'" & .getGridData(intRowPartyCtrlCDesig, intColPartyCtrlBill) & "',"
                    End If
                    If blnShipAddress Then
                        strSQL &= "'" & .getGridData(intRowPartyCtrlName, intColPartyCtrlShip) & "',"
                        strSQL &= "'" & .getGridData(intRowPartyCtrlAdd1, intColPartyCtrlShip) & "',"
                        strSQL &= "'" & .getGridData(intRowPartyCtrlAdd2, intColPartyCtrlShip) & "',"
                        strSQL &= "'" & .getGridData(intRowPartyCtrlAdd3, intColPartyCtrlShip) & "',"
                        strSQL &= "'" & .getGridData(intRowPartyCtrlAdd4, intColPartyCtrlShip) & "',"
                        strSQL &= "'" & .getGridData(intRowPartyCtrlCity, intColPartyCtrlShip) & "',"
                        strSQL &= "'" & .getGridData(intRowPartyCtrlState, intColPartyCtrlShip) & "',"
                        strSQL &= "'" & .getGridData(intRowPartyCtrlPincode, intColPartyCtrlShip) & "',"
                        strSQL &= "'" & .getGridData(intRowPartyCtrlCountry, intColPartyCtrlShip) & "',"
                        strSQL &= "'" & .getGridData(intRowPartyCtrlCName, intColPartyCtrlShip) & "',"
                        strSQL &= "'" & .getGridData(intRowPartyCtrlCMob, intColPartyCtrlShip) & "',"
                        strSQL &= "'" & .getGridData(intRowPartyCtrlCEmail, intColPartyCtrlShip) & "',"
                        strSQL &= "'" & .getGridData(intRowPartyCtrlCDesig, intColPartyCtrlShip) & "',"
                    End If
                End With
            End If
            strSQL = strSQL & "'" & gUserCode & "', "
            strSQL = strSQL & "'" & getDateFormatInInsert(getSystemDateTime(svtTrn), True) & "', "

            If ucBtnTools.tsbAmmend.Visible Then
                If chkAmmendment.Checked Then
                    strSQL = strSQL & "'Y',"
                    strSQL = strSQL & "'" & getDateFormatInInsert(dtAmmDate.Value) & "',"
                Else
                    strSQL = strSQL & "'N',"
                    strSQL = strSQL & "Null,"
                    txtAmmNo.Text = ""
                End If
                strSQL = strSQL & Val(txtAmmNo.Text) & ", "
            End If


            If strAuthReq = "N" Then
                strSQL = strSQL & "'Y',"
            Else
                strSQL = strSQL & "'N',"
            End If

            strSQL = strSQL & " 'N'"

            strSQL = strSQL & strValues

            If gDBTypeCurrent = "ACCESS" Then
                clsSVT.execSQLStatement(strSQL, svtTrn)
                strDtlSQL = GetDetSqlString()
                clsSVT.execSQLStatement(strDtlSQL, svtTrn)
                intMaxDocID = clsSVT.getDataLongValue("Select max(" & strDocIDFldName & ") from " & strDocHdrTblName, 0, False, svtTrn)
            Else
                strDtlSQL = ""
                If blnDetSection Then
                    strDtlSQL = GetDetSqlString()

                End If

                intMaxDocID = getMaxHdrDtlInsertID(strSQL, strDtlSQL, strDocHdrTblName, svtTrn)
            End If
            txtDocID.Text = intMaxDocID
            If intMaxDocID = -1 Then
                UpdtDocHdrDtl = False
                Exit Function
            End If
        End If

        If Mode = "M" Then
            strSQL = "update " & strDocHdrTblName & " set "
            If ucBtnTools.tsbAmmend.Visible Then
                If chkAmmendment.Checked Then
                    strSQL = strSQL & "Ammendment = 'Y',"
                    strSQL = strSQL & "AmmNo = '" & Val(txtAmmNo.Text) & "' ,"
                    strSQL = strSQL & "AmmDate = '" & getDateFormatInInsert(dtAmmDate.Value) & "' ,"
                Else
                    strSQL = strSQL & "Ammendment = '',"
                    strSQL = strSQL & " AmmNO = '', "
                    strSQL = strSQL & " AmmDate = Null, "
                End If
            End If
            strSQL = strSQL & "Authorised = 'N', "
            strSQL = strSQL & "ChangedBy = '" & gUserCode & "', "
            strSQL = strSQL & "ChangedOn = '" & getDateFormatInInsert(getSystemDateTime(svtTrn), True) & "', "

            If intPartyDocID > 0 Or blnManualPartySel Then
                With svtPCtrl
                    If Not blnManualPartySel Then strSQL = strSQL & strFldPartyID & " = " & Val(svtPCtrl.getBillID()) & ", "
                    If blnSavePartyData Then
                        strSQL &= "PartyName='" & .getGridData(intRowPartyCtrlName, intColPartyCtrlBill) & "',"
                        strSQL &= "PartyAdd1='" & .getGridData(intRowPartyCtrlAdd1, intColPartyCtrlBill) & "',"
                        strSQL &= "PartyAdd2='" & .getGridData(intRowPartyCtrlAdd2, intColPartyCtrlBill) & "',"
                        strSQL &= "PartyAdd3='" & .getGridData(intRowPartyCtrlAdd3, intColPartyCtrlBill) & "',"
                        strSQL &= "PartyAdd4='" & .getGridData(intRowPartyCtrlAdd4, intColPartyCtrlBill) & "',"
                        strSQL &= "PartyCity='" & .getGridData(intRowPartyCtrlCity, intColPartyCtrlBill) & "',"
                        strSQL &= "PartyState='" & .getGridData(intRowPartyCtrlState, intColPartyCtrlBill) & "',"
                        strSQL &= "PartyPincode='" & .getGridData(intRowPartyCtrlPincode, intColPartyCtrlBill) & "',"
                        strSQL &= "PartyCountry='" & .getGridData(intRowPartyCtrlCountry, intColPartyCtrlBill) & "',"
                        strSQL &= "PartyCName='" & .getGridData(intRowPartyCtrlCName, intColPartyCtrlBill) & "',"
                        strSQL &= "PartyCMob='" & .getGridData(intRowPartyCtrlCMob, intColPartyCtrlBill) & "',"
                        strSQL &= "PartyCEmail='" & .getGridData(intRowPartyCtrlCEmail, intColPartyCtrlBill) & "',"
                        strSQL &= "PartyCDesig='" & .getGridData(intRowPartyCtrlCDesig, intColPartyCtrlBill) & "',"
                    End If
                    If blnShipAddress Then
                        strSQL &= "PartySName='" & .getGridData(intRowPartyCtrlName, intColPartyCtrlShip) & "',"
                        strSQL &= "PartySAdd1='" & .getGridData(intRowPartyCtrlAdd1, intColPartyCtrlShip) & "',"
                        strSQL &= "PartySAdd2='" & .getGridData(intRowPartyCtrlAdd2, intColPartyCtrlShip) & "',"
                        strSQL &= "PartySAdd3='" & .getGridData(intRowPartyCtrlAdd3, intColPartyCtrlShip) & "',"
                        strSQL &= "PartySAdd4='" & .getGridData(intRowPartyCtrlAdd4, intColPartyCtrlShip) & "',"
                        strSQL &= "PartySCity='" & .getGridData(intRowPartyCtrlCity, intColPartyCtrlShip) & "',"
                        strSQL &= "PartySState='" & .getGridData(intRowPartyCtrlState, intColPartyCtrlShip) & "',"
                        strSQL &= "PartySPincode='" & .getGridData(intRowPartyCtrlPincode, intColPartyCtrlShip) & "',"
                        strSQL &= "PartySCountry='" & .getGridData(intRowPartyCtrlCountry, intColPartyCtrlShip) & "',"
                        strSQL &= "PartySCName='" & .getGridData(intRowPartyCtrlCName, intColPartyCtrlShip) & "',"
                        strSQL &= "PartySCMob='" & .getGridData(intRowPartyCtrlCMob, intColPartyCtrlShip) & "',"
                        strSQL &= "PartySCEmail='" & .getGridData(intRowPartyCtrlCEmail, intColPartyCtrlShip) & "',"
                        strSQL &= "PartySCDesig='" & .getGridData(intRowPartyCtrlCDesig, intColPartyCtrlShip) & "',"
                    End If
                End With
            End If

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
            If blnDetSection Then
                strDtlSQL = GetDetSqlString(Val(txtDocID.Text))
                If Not clsSVT.execSQLStatement(strDtlSQL, svtTrn) Then
                    UpdtDocHdrDtl = False
                    Exit Function
                End If
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

        'Call GetvsfOptHdrDetInsertString(Me, vsfDet, vsfOptions, intDetCols, 1, strColFlds, strColValues, svtTrn)


        'strFlds = "Insert Into  " & strDocDtlTblName

        'If intItemDocID > 0 Then
        '    strFlds = strFlds & " (" & strDocIDFldName & " , slNo, " & strFldItemID & "," & strFldItemSlno & ", UOMID, CumQty,"
        '    If blnLinkedDocFrom Then
        '        strFlds &= strPendIDFldName & ","
        '    End If
        'Else
        '    strFlds = strFlds & " (" & strDocIDFldName & " , slNo, "
        'End If


        'For i = 1 To vsfDet.Rows.Count - 1
        '    If argDocID > 0 AndAlso Mode = "M" Then
        '        strValues = strValues & "(" & argDocID & ", "
        '    Else
        '        strValues = strValues & "(@MaxID, "
        '    End If

        '    With vsfDet
        '        strValues = strValues & Val(.GetData(i, vDetSlNo)) & ","
        '        If intItemDocID > 0 Then
        '            strValues = strValues & Val(.GetData(i, vDetItemID)) & ","
        '            strValues = strValues & Val(.GetData(i, vDetItemSlNo)) & ","
        '            strValues = strValues & clsSVT.getDataLongValue("Select UOMID from MstUOM Where UOMCode ='" & GetVal(.GetData(i, vDetUOM), "-") & "'", 0, False, svtTrn) & ","
        '            strValues = strValues & Val(.GetData(i, vDetDocCumQty)) & ","
        '            If blnLinkedDocFrom Then
        '                strValues = strValues & Val(.GetData(i, vDetSrcDocID)) & ","
        '            End If
        '        End If
        '    End With

        '    Call GetvsfOptHdrDetInsertString(Me, vsfDet, vsfOptions, intDetCols, i, strFlds1, strValues, svtTrn)

        '    strValues = strValues & ","
        'Next

        'strValues = Mid(strValues, 1, Len(strValues) - 1)

        'GetDetSqlString = strFlds & strColFlds & " values " & strValues

        Call GetvsfOptHdrDetInsertString(Me, vsfDet, vsfOptions, intDetCols, 1, strColFlds, strColValues, svtTrn)


        strFlds = "Insert Into  " & strDocDtlTblName

        If intItemDocID > 0 Then
            strFlds = strFlds & " (" & strDocIDFldName & " , slNo," & strFldItemSlno & ", " & strFldItemID & ", UOMID, "
        Else
            strFlds = strFlds & " (" & strDocIDFldName & " , slNo, ItemSlno, "
        End If

        If blnLinkedDocFrom Then
            strFlds &= strPendIDFldName & "," & strPendDocShForm & "ItemSlno,"
        End If

        If Not IsNothing(dtCumQty) Then
            For k As Int32 = 0 To dtCumQty.Rows.Count - 1
                strFlds = strFlds & dtCumQty.Rows(k).Item(1) & ", "
            Next
        End If

        For i = 1 To vsfDet.Rows.Count - 1
            If argDocID > 0 AndAlso Mode = "M" Then
                strValues = strValues & " Select  " & argDocID & ", "
            Else
                strValues = strValues & " Select  @MaxID, "
            End If

            With vsfDet
                strValues = strValues & Val(.GetData(i, vDetSlNo)) & ","
                strValues = strValues & Val(.GetData(i, vDetItemSlNo)) & ","
                If intItemDocID > 0 Then
                    strValues = strValues & Val(.GetData(i, vDetItemID)) & ","
                    strValues = strValues & clsSVT.getDataLongValue("Select UOMID from MstUOM Where UOMCode ='" & GetVal(.GetData(i, vDetUOM), "-") & "'", 0, False, svtTrn) & ","
                    ' strValues = strValues & Val(.GetData(i, vDetDocCumQty)) & ","
                End If
                If blnLinkedDocFrom Then
                    strValues = strValues & Val(.GetData(i, vDetSrcDocID)) & ","
                    strValues = strValues & Val(.GetData(i, vDetSrcItemSlNo)) & ","
                End If

                If Not IsNothing(dtCumQty) Then
                    For k As Int32 = 0 To dtCumQty.Rows.Count - 1
                        strValues = strValues & Val(.GetData(i, intDetCols + k - dtCumQty.Rows.Count)) & ", "
                    Next
                End If
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

    Public Sub vsfHdr_AfterEdit(sender As Object, e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfHdr.AfterEdit
        If e.Row = intRowRPoints Then
            If Val(vsfHdr.GetData(e.Row, vOthFldDataValue)) > Val(vsfHdr.GetData(intRowTotPoints, vOthFldDataValue)) Then
                MsgBox("REDEEM POINTS cannot exceed TOTAL POINTS", MsgBoxStyle.Critical, Me.Text)
                Exit Sub
            End If
        End If
            Call vsfGenAfterEditBfrCalTot(Me, sender, e.Row, "M")
        Call fillVsfHFTDefValues(Me)
        If blnCalc = False Then
            Call CalTotal()
        End If
        Call vsfGenAfterEditAftrCalTot(Me, sender, intDocID, Mode, e.Row, False)
    End Sub

    Public Sub vsfHdr_BeforeEdit(sender As Object, e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfHdr.BeforeEdit
        If (Mode = "A" Or Mode = "M") Then
            If e.Row = intRowCustID Then
                vsfHdr.ComboList = "..."
                Exit Sub
            End If
        End If
        If vsfBeforeEditCancel(Me, sender, intDocID, strDocHdrTblName, Mode, e.Row, e.Col, "M", "") Then
            e.Cancel = True
            Exit Sub
        End If
    End Sub

    Public Sub vsfHdr_EnterCell(sender As Object, e As System.EventArgs) Handles vsfHdr.EnterCell
        If Mode = "V" Or Mode = "" Then Exit Sub

        With sender
            If .GetData(.Row, vOthFldTxtBlnMultiLine) = "Y" Then Exit Sub
            If .GetData(.Row, vOthFldBlnComboData) = "Y" Then Exit Sub
            If .GetData(.Row, vOthFldDataType) = "List String" Then Exit Sub
            If .GetData(.Row, vOthFldBlnReadOnly) = "Y" Then Exit Sub
            If .GetData(.Row, vOthFldBlnRefill) = "Y" Then Exit Sub

            '.StartEditing()
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
                If intItemDocID > 0 Then
                    vsfDet.Col = vDetItemCode
                Else
                    If intDetNextEntryCol > 0 Then
                        vsfDet.Col = intDetNextEntryCol
                    End If
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

    Private Function getSrcQry(argValue As String, ByVal argCond As Boolean) As String
        Dim strSQL As String = "", strTblList As String = ""
        strSQL = "Select Top " & gIntTopNrows & "  h." & strDocIDFldName & "," & strDocNoFldName & "," & strSHForm & "Date "
        If intPartyDocID > 0 Then
            strSQL &= "," & strFldPartyCode & "," & strFldPartyName
        End If

        For Each argVsf As C1FlexGrid In ctrlList
            If argVsf.Visible = True Then
                For k As Int32 = 0 To argVsf.Rows.Count - 1
                    If GetVal(argVsf.GetData(k, vOthFldBlnMandatory), "") = "Y" Then
                        If GetVal(argVsf.GetData(k, vOthFldBlnComboData), "") = "Y" Then
                            strSQL &= "," & argVsf.GetData(k, vOthFldCmbTblName) & "." & argVsf.GetData(k, vOthFldCmbDisplayCols)
                            strTblList &= argVsf.GetData(k, vOthFldCmbTblName) & ","
                        Else
                            strSQL &= "," & argVsf.GetData(k, vOthFldCode)
                        End If
                    End If
                Next
            End If
        Next


        strSQL &= " From "
        If intPartyDocID > 0 Then
            strSQL &= strPartyTblName & " s, "
        End If

        If strTblList <> "" Then
            strSQL &= strTblList
        End If

        strSQL &= strDocHdrTblName & " h where 1 = 1 "
        If intPartyDocID > 0 Then
            strSQL &= " and h." & strFldPartyID & "=s." & strFldPartyID
        End If
        If Trim(argValue) <> "" AndAlso argCond = True Then
            strSQL &= " And ("
            strSQL &= strDocNoFldName & " Like '%" & argValue & "%'"
            If gDBTypeCurrent = "ACCESS" Then
                strSQL &= " or Cstr(" & strSHForm & "Date)" & " like '%" & argValue & "%'"
            Else
                strSQL &= " or Convert(varchar(25)," & strSHForm & "Date,103)" & " like '%" & argValue & "%'"
            End If
            If intPartyDocID > 0 Then
                strSQL &= " or " & strFldPartyName & "  like '%" & argValue & "%'"
                strSQL &= " or " & strFldPartyCode & "  like '%" & argValue & "%'"
            End If

            For Each argVsf As C1FlexGrid In ctrlList
                If argVsf.Visible = True Then
                    For k As Int32 = 0 To argVsf.Rows.Count - 1
                        If GetVal(argVsf.GetData(k, vOthFldBlnMandatory), "") = "Y" Then
                            If GetVal(argVsf.GetData(k, vOthFldBlnComboData), "") = "Y" Then
                                strSQL &= " or Convert(varchar," & argVsf.GetData(k, vOthFldCmbTblName) & "." & argVsf.GetData(k, vOthFldCmbDisplayCols) & ")  like '%" & argValue & "%'"
                            Else
                                strSQL &= " or Convert(varchar," & argVsf.GetData(k, vOthFldCode) & ")  like '%" & argValue & "%'"
                            End If
                        End If
                    Next
                End If
            Next

            strSQL &= ")"
        End If
        strSQL &= " order by 1,2"
        Return strSQL
    End Function

    Private Sub srcTransaction_TxtChanged(argValue As String) Handles srcTransaction.TxtChanged
        If argValue = "" Then
            srcTransaction.vsfAnchorReSize(410, 30)
            Exit Sub
        End If
        srcTransaction.LoadGrid(argValue, "", getSrcQry(argValue, True))
        srcTransaction.vsfAnchorReSize(655, 480)
    End Sub

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

    Private Sub svtInsert()
        With ucBtnTools
            If intItemDocID > 0 Then
                If Trim(vsfDet.GetData(vsfDet.Row, vDetItemCode)) = "" Then
                    vsfDet.Enabled = True
                    Exit Sub
                Else
                    Call .vsfInsert(vsfDet, vsfDet.Row, vDetItemCode)
                End If
                If Val(vsfDet.GetData(vsfDet.Row, vDetItemSlNo)) <= 0 Then
                    vsfDet.SetData(vsfDet.Row, vDetItemSlNo, getmaxNo())
                End If
            Else
                Call .vsfInsert(vsfDet, vsfDet.Row, intDetCols)
                If Val(vsfDet.GetData(vsfDet.Row, vDetItemSlNo)) <= 0 Then
                    vsfDet.SetData(vsfDet.Row, vDetItemSlNo, getmaxNo())
                End If
            End If
            Call fillVsfDetDefValues(vsfDet, vsfOptions, vsfDet.Row)
        End With
    End Sub

    Private Sub svtRemove()
        With ucBtnTools
            Dim intLinkedDocID As Integer = 0
            Dim strLinkedToDocNo As String = ""

            If blnLinkedToDoc And Mode = "M" Then
                strLinkedToDocNo = clsSVT.getDataStringValue("Select Top 1 " & strLinkedToDocNoFldName & " from " & strLinkedTODtlTblName & " d, " & strLinkedTOHdrTblName & " h" &
                        " where h.Cancelled = 'N' and h." & strLinkedTODocFldIDName & " = d." & strLinkedTODocFldIDName &
                        " and " & strDocIDFldName & " = " & txtDocID.Text, "", False)
                If strLinkedToDocNo <> "" Then
                    MsgBox(strSHForm & "No already exist in " & strLinkedToDocNoFldName & " : '" & strLinkedToDocNo & "'", MsgBoxStyle.Critical, Me.Text)
                    vsfDet.Enabled = True
                    Exit Sub
                End If
            End If
            If Mode = "M" AndAlso Val(vsfDet.GetData(vsfDet.Row, vDetDocCumQty)) > 0 Then
                MsgBox("Already exist in Linked document. Deletion not possible.", MsgBoxStyle.Critical, Me.Text)
                vsfDet.Enabled = True
                Exit Sub
            End If
            Call .vsfRemove(vsfDet, vsfDet.Row, vDetItemCode)
            Call CalTotal()
        End With
    End Sub

    Private Sub svtCopy()
        With ucBtnTools
            Call .vsfCopy(vsfDet, vsfDet.Row, vDetItemCode)
        End With
    End Sub

    Private Sub svtPaste()
        With ucBtnTools
            If intItemDocID > 0 Then
                Call .vsfPaste(vsfDet, vsfDet.Row, vDetItemCode)
                vsfDet.SetData(vsfDet.Row, vDetItemSlNo, getmaxNo())
            Else
                Call .vsfPaste(vsfDet, vsfDet.Row, vDetSlNo + 1)
            End If
        End With
    End Sub

    Private Sub svtUp()
        Call MoveOneRowUp(vsfDet, vDetSlNo)
        vsfDet.Enabled = True
    End Sub

    Private Sub svtDown()
        Call MoveOneRowDown(vsfDet, vDetSlNo)
    End Sub

    Private Sub svtReplace()
        With ucBtnTools
            Call .vsfReplace(vsfDet, vsfDet.Row, vsfDet.Col, vDetItemCode)
            Call CalTotal()
        End With
    End Sub

    Private Sub svtExport()
        With ucBtnTools
            Call .vsfExport(vsfDet, Me.Text & " " & txtDocNo.Text)
        End With
    End Sub

    Private Sub svtImport()
        With ucBtnTools
            vsfDet.Col = vDetItemName
            If gBlnMSOffice Then
                If intItemDocID > 0 Then
                    If .vsfImport(vsfDet, vDetItemCode, vsfOptions, intDetCols) Then
                        With vsfDet
                            For i As Int32 = 1 To .Rows.Count - 1
                                If Not FillItemDet(.GetData(i, vDetItemCode), i, "") Then
                                    vsfDet.Rows.Count = 1
                                    vsfDet.Rows.Count = 2
                                    vsfDet.Enabled = True
                                    Exit Sub
                                End If
                            Next
                            MsgBox("XL imported into Grid...", MsgBoxStyle.Information, Me.Text)
                        End With
                    End If
                Else
                    If .vsfImport(vsfDet, intDetNextEntryCol, vsfOptions, intDetCols) Then
                        MsgBox("XL imported into Grid...", MsgBoxStyle.Information, Me.Text)
                    End If
                End If

            Else
                If intItemDocID > 0 Then
                    If .vsfImport_ClosedXML(vsfDet, vDetItemCode, vsfOptions, intDetCols) Then
                        With vsfDet
                            For i As Int32 = 1 To .Rows.Count - 1
                                If Not FillItemDet(.GetData(i, vDetItemCode), i, "") Then
                                    vsfDet.Rows.Count = 1
                                    vsfDet.Rows.Count = 2
                                    vsfDet.Enabled = True
                                    Exit Sub
                                End If
                            Next
                            MsgBox("XL imported into Grid...", MsgBoxStyle.Information, Me.Text)
                        End With
                    End If
                Else
                    If .vsfImport_ClosedXML(vsfDet, intDetNextEntryCol, vsfOptions, intDetCols) Then
                        MsgBox("XL imported into Grid...", MsgBoxStyle.Information, Me.Text)
                    End If
                End If
            End If

            Call CalTotal()
        End With
    End Sub

    Private Sub svtTemplate()
        With ucBtnTools
            Dim strPath As String, strFileName As String
            strPath = Application.StartupPath() & "\Temp\"
            strFileName = Date.Now.ToString("yyyyMMddHHmmss") & "_" & Trim(txtDocNo.Text)
            If gBlnMSOffice Then
                Call .vsfTemplate(vsfDet, strPath & strFileName, vsfOptions)
            Else
                Call .vsfTemplate_ClosedXML(vsfDet, strPath & strFileName, vsfOptions)
            End If
        End With
    End Sub

    Private Sub svtAttach()
        With ucBtnTools
            If Mode = "M" Or Mode = "V" Then
                .vsfAttach(Me, strRights, intDocID, Val(txtDocID.Text))
            End If
            'fraAttach.Visible = True
        End With
    End Sub

    Private Sub svtRestore()
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
    End Sub

    Private Sub svtCopyFrom()
        Dim obj As New frmUtlCopyFrom
        With obj
            .argForm = Me
            .strFrmCaption = strFrmCaption
            .argvsfOptions = vsfOptions
            .argvsf = vsfDet
            .ShowDialog()
        End With
    End Sub

    Private Sub svtAmmend()
        If fraAmm.Visible Then
            fraAmm.Enabled = False
            fraAmm.Visible = False
        Else
            fraAmm.Enabled = True
            fraAmm.Visible = True
        End If
    End Sub

    Private Sub svtFreeze()
        With ucBtnTools
            Call .vsfFreeze(vsfDet, vsfDet.Col)
        End With
    End Sub

    Private Sub svtAddItems()
        If intItemDocID > 0 Then
            vsfDet.Col = vDetItemName
        Else
            vsfDet.Col = vDetItemSlNo + 1
        End If
        Dim obj As New frmUtlAddItems
        obj.frm = Me
        obj.argItemCode = vDetItemCode
        obj.ShowDialog()
        obj.Dispose()
        obj.Close()
    End Sub

    Private Sub svtTender()
        With ucBtnTools
            Call .vsfTenderChange(intDocID, Val(txtDocID.Text), Trim(txtDocNo.Text), Val(vsfTot.GetData(intRowTenderFld, vOthFldDataValue)))
        End With
    End Sub

    Private Sub svtInsertDoc()
        With ucBtnTools
            If strInsertParty = "Y" Then
                If Trim(svtPCtrl.getCode()) = "" Then
                    MsgBox("Please select party....", MsgBoxStyle.Information, Me.Text)
                    Exit Sub
                End If
            End If
            Call .vsfInsertDoc(intDocID, Me, intItemDocID, intPartyDocID, intColQty, intColrate, intDetCols, svtPCtrl.getCode())
        End With
    End Sub

    Private Sub svtMultiDoc()
        With ucBtnTools
            Call .vsfMultDoc(intDocID, Me, Trim(txtDocNo.Text))
        End With
    End Sub

    Private Sub svtTrnExport()
        With ucBtnTools
            If gBlnMSOffice Then
                Call .DocXlExp(Me, strSHForm, tbAdnlFlds, vsfDet, tbAdnlFldsFtr, vsfTot, intDocID, txtDocNo.Text, dtDocDate.Value, cboPfx.Text, svtPCtrl.getBillAddress(), IIf(UCase(strFldPartyCode) = "CUSTCODE", "Customer", "Supplier"))
            Else
                Call .DocXlExp_ClosedXML(Me, strSHForm, tbAdnlFlds, vsfDet, tbAdnlFldsFtr, vsfTot, intDocID, txtDocNo.Text, dtDocDate.Value, cboPfx.Text, svtPCtrl.getBillAddress(), IIf(UCase(strFldPartyCode) = "CUSTCODE", "Customer", "Supplier"))
            End If
        End With
    End Sub

    Private Sub svtBOM()
        With ucBtnTools
            Call .vsfBOM(Me, vDetItemCode, intColQty)
        End With
    End Sub

    Private Sub svtCalTotal()
        Call CalTotal()
    End Sub

    Private Sub svtSMS()
        With ucBtnTools
            'Call .vsfSMS()
        End With
    End Sub

    Private Sub Pending()

    End Sub

    Private Sub ucBtnTools_btnClicked(btnValue As Integer) Handles ucBtnTools.btnClicked
        Dim intLinkedDocID As Integer = 0
        Dim strLinkedToDocNo As String = ""
        Select Case btnValue
            Case 9, 10, 11, 12, 13, 16, 17, 18, 19, 20, 21
            Case Else
                If gridEmpty() Then Exit Sub
        End Select
        Select Case btnValue
            Case 8, 10, 11, 15, 20, 28
            Case Else
                If Not (Mode = "A" Or Mode = "M") Then Exit Sub
        End Select
        Try
            vsfDet.FinishEditing()
            vsfDet.Enabled = False
            With ucBtnTools
                Select Case btnValue
                    Case svtDtlBtns.svtInsert
                        Call svtInsert()

                    Case svtDtlBtns.svtRemove
                        Call svtRemove()

                    Case svtDtlBtns.svtCopy
                        Call svtCopy()

                    Case svtDtlBtns.svtPaste
                        Call svtPaste()

                    Case svtDtlBtns.svtUp
                        Call svtUp()

                    Case svtDtlBtns.svtDown
                        Call svtDown()

                    Case svtDtlBtns.svtReplace
                        Call svtReplace()

                    Case svtDtlBtns.svtExport
                        Call svtExport()

                    Case svtDtlBtns.svtImport
                        Call svtImport()

                    Case svtDtlBtns.svtTemplate
                        Call svtTemplate()

                    Case svtDtlBtns.svtAttach
                        Call svtAttach()

                    Case svtDtlBtns.svtRestore
                        Call svtRestore()

                    Case svtDtlBtns.svtCopyFrom
                        Call svtCopyFrom()

                    Case svtDtlBtns.svtAmmend
                        Call svtAmmend()

                    Case svtDtlBtns.svtFreeze
                        Call svtFreeze()

                    Case svtDtlBtns.svtAddItems
                        Call svtAddItems()

                    Case svtDtlBtns.svtTender
                        Call svtTender()

                    Case svtDtlBtns.svtInsertDoc
                        Call svtInsertDoc()

                    Case svtDtlBtns.svtMultiDoc
                        Call svtMultiDoc()

                    Case svtDtlBtns.svtTrnExport
                        Call svtTrnExport()

                    Case svtDtlBtns.svtBOM
                        Call svtBOM()

                    Case svtDtlBtns.svtCalTotal
                        Call svtCalTotal()

                    Case svtDtlBtns.svtSMS
                        Call svtSMS()

                    Case svtDtlBtns.svtPending

                End Select
                vsfDet.Enabled = True
            End With
            ReArrangeSlNo(vsfDet, vDetSlNo, vDetItemSlNo)
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information, Me.Text)
        End Try
    End Sub

    Private Sub TimerStartEvent()
        blnTimer = True
        strRestoreFileName = "Doc_No_" & Trim(txtDocNo.Text) & "_" & strDateTime
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
            If blnDetSection Then
                If File.Exists(strRestorePath & strRestoreFileName & ".xml") Then File.Delete(strRestorePath & strRestoreFileName & ".xml")
                If vsfDet.Rows.Count >= 2 Then
                    vsfDet.WriteXml(strRestorePath & strRestoreFileName & ".xml")
                End If
            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Exclamation, Me.Text)
        End Try
        blnTimer = True
    End Sub

    Private Sub RestoreData()
        Dim strFileName As String = strRestoreFileName
        If blnDetSection Then RestoreData_Grid(vsfDet, strFileName, "")
        Dim argDsgrGrids = GetAllDsgrVsf(Me, GetType(C1FlexGrid))
        For Each argGrid As C1FlexGrid In argDsgrGrids
            If argGrid.Visible Then
                RestoreData_Grid(argGrid, strFileName, "_" & argGrid.Name)
            End If
        Next
        strRestoreFileName = "Doc_No_" & Trim(txtDocNo.Text) & "_" & strDateTime
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
                    If (Now - file.CreationTime).Days > 7 Then file.Delete()
                Next
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub svtPartyCtrl1_LostFocus(sender As Object, e As System.EventArgs) Handles svtPCtrl.LostFocus
        If blnFirstTime Then
            blnFirstTime = False
            Exit Sub
        End If
        Call NextFocus()
    End Sub

    Private Sub SvtPartyCtrl1_LinkClicked(argType As String, argID As Integer) Handles svtPCtrl.LinkClicked
        If Mode = "A" Or Mode = "M" Then
            Call svtPCtrl.SendPartyData(GetVal(strPartyType, ""), Val(argID), intPartyDocID, argType, Me)
        End If
    End Sub

    Private Sub svtPartyCtrl1_TxtChanged(argValue As String) Handles svtPCtrl.TxtChanged
        If Mode = "A" Or Mode = "M" Then
            With svtPCtrl
                If argValue = "" Then
                    .ClearFlds()
                    .vsfVisisble(False)
                    If blnOnlycode Then
                        Call .frmSize_Code()
                    End If
                    If blnOnlyBill Then
                        Call .frmSize_Billing()
                    End If
                    If blnBillShip Then
                        Call .frmSize_BillShip()
                    End If
                    .setFocus()
                    Exit Sub
                End If
                svtPCtrl.LoadFilterParty(strPartyType, argValue, strPartyFilterCond, strPartyFlds)
                svtPCtrl.BringToFront()
                vsfTop.SendToBack()
            End With
        End If
    End Sub

    Private Sub svtPartyCtrl1_TxtKeyUp(argValue As String) Handles svtPCtrl.TxtKeyUp
        If Mode = "V" Then Exit Sub
        With svtPCtrl
            If argValue = "" Then
                .ClearFlds()
                .vsfVisisble(False)
                If blnOnlycode Then
                    Call .frmSize_Code()
                End If
                If blnOnlyBill Then
                    Call .frmSize_Billing()
                End If
                If blnBillShip Then
                    Call .frmSize_BillShip()
                End If
                .setFocus()
                If blnLinkedDocFrom Then
                    btnPendingDoc.BringToFront()
                End If
                Exit Sub
            End If
        End With
        Select Case argValue
            Case "ENTER"
                If Val(svtPCtrl.vsfView.Rows.Count) <= 1 Then
                    MsgBox("No Party....", MsgBoxStyle.Exclamation, Me.Text)
                    Exit Sub
                End If

                svtPCtrl.LoadFromGrid(strPartyType, intPartyDocID, blnOnlycode, blnBillShip, blnOnlyBill)
                svtPCtrl.vsfView.Visible = False
                If blnLinkedDocFrom Then
                    btnPendingDoc.Focus()
                    Exit Sub
                End If
                vsfDet.Col = vDetItemCode
                vsfDet.Row = vsfDet.Rows.Count - 1
                'vsfDet.StartEditing()
            Case "DOWN_LIST"
                svtPCtrl.LoadAllParty(strPartyType, strPartyFilterCond, strPartyFlds)
            Case "F4"
                txtDummy.Text = ""
                txtDummy2.Text = ""

                If Not CheckforLinkedToDoc() Then Exit Sub
                strSQL = "Select " & strPartyType & "ID," & strPartyType & "Code as Code," & strPartyType & "Name as Name "
                If strPartyFlds <> "" Then
                    strSQL = strSQL & ", " & strPartyFlds
                End If
                strSQL = strSQL & " from Mst" & strPartyType
                strSQL = strSQL & " Where InActive = 'N' "
                strSQL = strSQL & strPartyFilterCond
                strSQL = strSQL & " order by 2"

                Dim objfrm As New frmUtlSelection
                With objfrm
                    .blnMultiColSearch = True
                    .FillViewVsf(Me, False, Nothing, strSQL, txtDummy, 0, txtDummy2, 1)
                    .Text = " Please Select " & strPartyType
                    .ShowDialog()
                    .Close()
                End With

                If Trim(txtDummy.Text) <> "" Then
                    Call svtPCtrl.setCode(txtDummy2.Text)
                    svtPCtrl.LoadFilterParty(strPartyType, txtDummy2.Text, strPartyFilterCond, strPartyFlds)
                    svtPCtrl.LoadFromGrid(strPartyType, intPartyDocID, blnOnlycode, blnBillShip, blnOnlyBill)
                    Call svtPCtrl.vsfVisisble(False)
                    Call NextFocus()
                End If

                If btnPendingDoc.Visible Then
                    btnPendingDoc.BringToFront()
                    Exit Sub
                End If

            Case Else
                svtPCtrl.LoadFilterParty(strPartyType, argValue, strPartyFilterCond, strPartyFlds)
        End Select
        svtPCtrl.BringToFront()
        vsfTop.SendToBack()
    End Sub

    Private Sub svtPartyCtrl1_TxtValidation(argValue As String) Handles svtPCtrl.TxtValidation
        If Mode = "A" Or Mode = "M" Then
            Call svtPCtrl.ValidateText(strPartyType, argValue)
        End If
    End Sub

    Private Sub svtPartyCtrl1_vsfDoubleClick(argValue As Integer) Handles svtPCtrl.vsfDoubleClick
        Call svtPCtrl.fillGrid(argValue, strPartyType, blnOnlyBill, blnBillShip, blnOnlycode, intPartyDocID)
        Call svtPCtrl.vsfVisisble(False)
        Call NextFocus()
    End Sub

    Private Sub svtPartyCtrl1_vsfKeyUp(argValue As Integer) Handles svtPCtrl.vsfKeyUp
        If argValue > -1 Then
            Call svtPCtrl.fillGrid(argValue, strPartyType, blnOnlyBill, blnBillShip, blnOnlycode, intPartyDocID)
            Call svtPCtrl.vsfVisisble(False)
            Call NextFocus()
        Else
            svtPCtrl.ChangeFormSize(blnOnlyBill, blnBillShip, blnOnlycode)
        End If
    End Sub

    Private Sub btnInsertfromDoc_Click(sender As System.Object, e As System.EventArgs)
        With frmUtlInsertfromDoc
            .intDocID = intDocID
            .argFrm = Me
            .ShowDialog()
        End With
    End Sub

    Private Sub raiseShortCutEvent(ByVal e As System.Windows.Forms.KeyEventArgs)
        If blnDetSection = False Then Exit Sub
        If Mode = "A" Or Mode = "M" Or Mode = "V" Then
            'Insert Row event
            If e.Control = True And e.Shift = True And e.KeyCode = Keys.I Then
                Call svtInsert()
                ReArrangeSlNo(vsfDet, vDetSlNo)
                Exit Sub
            End If

            'Remove Row event
            If e.Control = True And e.Shift = True And e.KeyCode = Keys.R Then
                Call svtRemove()
                ReArrangeSlNo(vsfDet, vDetSlNo)
                Exit Sub
            End If

            'Copy  event
            If e.Control = True And e.Shift = True And e.KeyCode = Keys.C Then
                Call svtCopy()
                Exit Sub
            End If

            'Paste Event
            If e.Control = True And e.Alt = True And e.KeyCode = Keys.P Then
                Call svtPaste()
                ReArrangeSlNo(vsfDet, vDetSlNo)
                Exit Sub
            End If

            'MoveUp Event
            If e.Control = True And e.Shift = True And e.KeyCode = Keys.U Then
                Call svtUp()
                Exit Sub
            End If

            'MoveDown Event
            If e.Control = True And e.Shift = True And e.KeyCode = Keys.V Then
                Call svtDown()
                Exit Sub
            End If

            'RePlace Rest Event
            If e.Control = True And e.Alt = True And e.KeyCode = Keys.N Then

                'Call ucBtnTools.vsfReplace(vsfDet, vsfDet.Row, vsfDet.Col, vDetItemCode)
                ''------changed 
                'svtBtnCtrl.btnEdit.Visible = False
                'svtBtnCtrl.btnDelete.Visible = False
                ''-------
                Call svtReplace()
                Exit Sub
            End If

            'Exprot to Excel
            If e.Control = True And e.Shift = True And e.KeyCode = Keys.E Then
                Call svtExport()
                Exit Sub
            End If

            'Import from Excel
            If e.Control = True And e.Shift = True And e.KeyCode = Keys.H Then
                Call svtImport()
                Exit Sub
            End If


            'Template
            If e.Control = True And e.Shift = True And e.KeyCode = Keys.T Then
                Call svtTemplate()
                Exit Sub
            End If

            'Attachment
            If e.Control = True And e.Shift = True And e.KeyCode = Keys.A Then
                Call svtAttach()
                Exit Sub
            End If

            'Restore
            If e.Control = True And e.Shift = True And e.KeyCode = Keys.O Then
                Call svtRestore()
                Exit Sub
            End If

            'Copy From
            If e.Control = True And e.Alt = True And e.KeyCode = Keys.F Then
                Call svtCopyFrom()
                Exit Sub
            End If

            'Append
            If e.Control = True And e.Shift = True And e.KeyCode = Keys.J Then
                Call svtAmmend()
                Exit Sub
            End If
            'Freeze
            If e.Control = True And e.Shift = True And e.KeyCode = Keys.Z Then
                Call svtFreeze()
                Exit Sub
            End If

            'Add List of Items
            If e.Control = True And e.Shift = True And e.KeyCode = Keys.Q Then
                Call svtAddItems()
                Exit Sub
            End If

            'Doc XL Export
            If e.Control = True And e.Shift = True And e.KeyCode = Keys.X Then
                Call svtTrnExport()
                Exit Sub
            End If

            'Tender Change
            If e.Control = True And e.Shift = True And e.KeyCode = Keys.Y Then
                Call svtTender()
                Exit Sub
            End If

            'Insert Doc
            If e.Control = True And e.Shift = True And e.KeyCode = Keys.B Then
                Call svtInsertDoc()
                Exit Sub
            End If

            'Multi Doc Generation
            If e.Control = True And e.Shift = True And e.KeyCode = Keys.G Then
                Call svtMultiDoc()
                Exit Sub
            End If

            If e.Control = True And e.Alt = True And e.KeyCode = Keys.L Then
                svtPCtrl.LoadAllParty(CStr(intPartyDocID), strPartyFilterCond, strPartyFlds)
            End If
        End If

        'Max size Detail grid
        If e.Control = True And e.Shift = True And e.KeyCode = Keys.M Then
            Call reSizeVSF("D")
        End If


        'Focus on Detail grid
        If e.Control = True And e.Shift = True And e.KeyCode = Keys.D Then
            vsfDet.Row = 1
            vsfDet.Col = vDetItemCode
            vsfDet.Focus()
        End If

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
                    tbAdnlFlds.Size = New Point(tbAdnlFlds.Width, fraMain.Height)
                    vsfDet.Size = New Point(fraMain.Width + tbAdnlFlds.Width + 4, vsfDet.Height)
                    btnHdrPin.BackgroundImage = My.Resources.PinDownArrow
                    blnVHdr = False
                Else
                    btnHdrPin.BackgroundImage = My.Resources.PinUpArrow
                    If vsfDet.Visible Then
                        vsfDet.Size = New Point(fraMain.Width, vsfDet.Height)
                        If vsfTop.Visible Then
                            tbAdnlFlds.Size = New Point(tbAdnlFlds.Width, tbAdnlFlds.Height + vsfDet.Height + 2 + vsfTop.Height)
                        Else
                            tbAdnlFlds.Size = New Point(tbAdnlFlds.Width, tbAdnlFlds.Height + vsfDet.Height + 2)
                        End If
                    Else
                        tbAdnlFlds.Size = New Point(tbAdnlFlds.Width, svtBtnCtrl.Top - 35)
                    End If
                    tbAdnlFlds.BringToFront()
                    blnVHdr = True
                End If
            Case "F"
                If blnVFtr Then
                    If vsfTop.Visible Then
                        vsfTop.Location = New Point(fraMain.Location.X, vsfTop.Location.Y)
                        vsfDet.Location = New Point(fraMain.Location.X, vsfDet.Location.Y)
                        'ucBtnTools.Location = vsfDet.Location
                        ucBtnTools.Location = New Point(tbAdnlFldsFtr.Left - 1, ucBtnTools.Location.Y)
                        vsfDet.Size = New Point(fraMain.Width + tbAdnlFlds.Width + 3, vsfDet.Height)
                        tbAdnlFldsFtr.Location = New Point(vsfDet.Location.X, vsfTot.Location.Y)
                        tbAdnlFldsFtr.Size = New Point(tbAdnlFldsFtr.Width, vsfTot.Height + lblTotTip.Height)
                    Else
                        vsfDet.Location = New Point(tbAdnlFldsFtr.Location.X, tbAdnlFldsFtr.Location.Y)
                        vsfDet.Size = New Point(fraMain.Width + tbAdnlFlds.Width + 3, vsfDet.Height)
                        ucBtnTools.Location = New Point(tbAdnlFldsFtr.Left - 1, ucBtnTools.Location.Y)
                        tbAdnlFldsFtr.Location = New Point(vsfDet.Location.X, vsfTot.Location.Y)
                        vsfTot.Height = 120 '''''''Changed
                        tbAdnlFldsFtr.Size = New Point(tbAdnlFldsFtr.Width, vsfTot.Height + lblTotTip.Height)
                    End If
                    btnFtrPin.BackgroundImage = My.Resources.PinUpArrow
                    blnVFtr = False
                Else
                    If vsfTop.Visible Then
                        tbAdnlFldsFtr.Location = New Point(vsfTop.Location.X, vsfTop.Location.Y)
                        tbAdnlFldsFtr.Size = New Point(tbAdnlFldsFtr.Width, vsfDet.Height + vsfTop.Height + tbAdnlFldsFtr.Height + ucBtnTools.Height + 10)
                        vsfDet.Location = New Point(tbAdnlFldsFtr.Right + 10, vsfDet.Location.Y)
                        ucBtnTools.Location = New Point(tbAdnlFldsFtr.Right + 10, ucBtnTools.Location.Y)
                        vsfTop.Location = New Point(tbAdnlFldsFtr.Right + 10, vsfTop.Location.Y)
                        vsfDet.Size = New Point(vsfDet.Width - tbAdnlFldsFtr.Width - 10, vsfDet.Height)
                    Else
                        tbAdnlFldsFtr.Location = New Point(vsfDet.Location.X, vsfDet.Location.Y)
                        tbAdnlFldsFtr.Size = New Point(tbAdnlFldsFtr.Width, vsfDet.Height + tbAdnlFldsFtr.Height + ucBtnTools.Height + 10)
                        vsfDet.Location = New Point(tbAdnlFldsFtr.Right + 10, vsfDet.Location.Y)
                        ucBtnTools.Location = New Point(tbAdnlFldsFtr.Right + 10, ucBtnTools.Location.Y)
                        vsfDet.Size = New Point(vsfDet.Width - tbAdnlFldsFtr.Width - 10, vsfDet.Height)
                    End If
                    btnFtrPin.BackgroundImage = My.Resources.PinDownArrow
                    blnVFtr = True
                End If
            Case "T"
                If blnVTot Then
                    If vsfTop.Visible Then
                        vsfDet.Size = New Point(fraMain.Width + tbAdnlFlds.Width + 3, vsfDet.Height)
                        vsfTot.Location = New Point(vsfTot.Location.X, tbAdnlFldsFtr.Location.Y)
                        vsfTot.Size = New Point(vsfTot.Width, tbAdnlFldsFtr.Height)
                    Else
                        vsfDet.Size = New Point(fraMain.Width + tbAdnlFlds.Width + 3, vsfDet.Height)
                        vsfTot.Location = New Point(vsfTot.Location.X, tbAdnlFldsFtr.Location.Y)
                        tbAdnlFldsFtr.Height = 120 '''''''Changed
                        vsfTot.Size = New Point(vsfTot.Width, tbAdnlFldsFtr.Height)
                        tbAdnlFldsFtr.Height = 141 '''''''Changed
                    End If
                    blnVTot = False
                    btnTotPin.BackgroundImage = My.Resources.PinUpArrow
                Else
                    If vsfTop.Visible Then
                        vsfTot.Location = New Point(vsfTot.Location.X, vsfTop.Location.Y)
                        vsfTot.Size = New Point(vsfTot.Width, vsfDet.Height + vsfTot.Height + vsfTop.Height + fraAmm.Height + 5)
                    Else
                        vsfTot.Location = New Point(vsfTot.Location.X, vsfDet.Location.Y)
                        vsfTot.Size = New Point(vsfTot.Width, vsfDet.Height + vsfTot.Height + fraAmm.Height + 5)
                    End If
                    vsfDet.Size = New Point(vsfTot.Left - 10, vsfDet.Height)
                    blnVTot = True
                    btnTotPin.BackgroundImage = My.Resources.PinDownArrow
                End If
            Case Else
                If blnVsfMax Then
                    If vsfTop.Visible Then
                        vsfTop.Location = New Point(vsfTop.Location.X, fraMain.Location.Y + fraMain.Height + 5)
                        vsfDet.Location = New Point(vsfDet.Location.X, fraMain.Location.Y + fraMain.Height + 5 + vsfTop.Height)
                        vsfDet.Size = New Point(vsfDet.Width, ucBtnTools.Top - fraMain.Bottom - 10 - vsfTop.Height)
                        vsfDet.BringToFront()
                        vsfTop.BringToFront()
                    Else
                        vsfDet.Location = New Point(vsfDet.Location.X, fraMain.Location.Y + fraMain.Height + 5)
                        vsfDet.Size = New Point(vsfDet.Width, ucBtnTools.Top - fraMain.Bottom - 10)
                        vsfDet.BringToFront()
                    End If
                    btnDetPin.Location = New Point(vsfDet.Right - 25, vsfDet.Bottom - 15)
                    btnDetPin.BringToFront()
                    blnVsfMax = False
                    btnDetPin.BackgroundImage = My.Resources.PinUpArrow
                Else
                    If vsfTop.Visible Then
                        vsfTop.Location = New Point(vsfTop.Location.X, fraMain.Location.Y)
                        vsfDet.Location = New Point(vsfDet.Location.X, fraMain.Location.Y + vsfTop.Height)
                        vsfDet.Size = New Point(vsfDet.Width, fraMain.Height + vsfDet.Height + fraAmm.Height + tbAdnlFldsFtr.Height + 10)
                        vsfDet.BringToFront()
                        vsfTop.BringToFront()
                    Else
                        vsfDet.Location = New Point(vsfDet.Location.X, fraMain.Location.Y)
                        vsfDet.Size = New Point(vsfDet.Width, fraMain.Height + vsfDet.Height + fraAmm.Height + tbAdnlFldsFtr.Height)
                        vsfDet.BringToFront()
                    End If
                    btnDetPin.Location = New Point(vsfDet.Right - 25, vsfDet.Bottom - 15)
                    btnDetPin.BringToFront()
                    blnVsfMax = True
                    btnDetPin.BackgroundImage = My.Resources.PinDownArrow
                End If
        End Select
        If vsfTop.Visible Then Call syncGridCols()
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

    Private Function CheckforLinkedToDoc() As Boolean
        CheckforLinkedToDoc = False

        If IsNothing(dtCumQty) Then
            CheckforLinkedToDoc = True
            Exit Function
        End If
        If Mode <> "M" Then
            CheckforLinkedToDoc = True
            Exit Function
        End If
        If Trim(strLinkedTODtlTblName) = "" Then Exit Function
        Dim blnFound As Boolean = False
        clsSVT.getDataStringValue("Select Top 1 " & strLinkedToDocNoFldName & " from " & strLinkedTODtlTblName & " d, " & strLinkedTOHdrTblName & " h" &
                                    " where h.Cancelled = 'N' and h." & strLinkedTODocFldIDName & " = d." & strLinkedTODocFldIDName &
                                    " and " & strDocIDFldName & " = " & txtDocID.Text, "", blnFound)
        If blnFound Then
            svtPCtrl.lblParty.ForeColor = Color.Blue
            svtPCtrl.EnableDisable(False)
            Exit Function
        End If
        CheckforLinkedToDoc = True
    End Function

    Private Sub vsfTot_LostFocus(sender As Object, e As System.EventArgs) Handles vsfTot.LostFocus
        pnlToolTip.Visible = False
    End Sub

    Private Sub svtPartyCtrl1_afterPartySelection(argPartyID As Long) Handles svtPCtrl.afterPartySelection
        If argPartyID = -1 Then Exit Sub
        If Not (Mode = "A" Or Mode = "M") Then Exit Sub
        For intTb As Int32 = 0 To tbAdnlFlds.TabPages.Count - 1
            Call InsertPrevTrnData(IIf(intTb = 0, "vsfHdr", "VSFHDR" & intTb + 1), dtHdrPrevTran, argPartyID, intTb)
        Next
        For intTb As Int32 = 0 To tbAdnlFldsFtr.TabPages.Count - 1
            Call InsertPrevTrnData(IIf(intTb = 0, "vsfFtr", "VSFFTR" & intTb + 1), dtFtrPrevTran, argPartyID, intTb)
        Next
        Call InsertPrevTrnData(vsfTot, intTotPrevTran, argPartyID)
        'If Not IsNothing(intDetPrevTran) AndAlso vsfDet.Rows.Count > 1 Then
        '    With vsfDet
        '        For i As Int32 = 1 To .Rows.Count - 1
        '            Call InsertPrevItemData(intDetPrevTran, argPartyID, Val(.GetData(i, vDetItemID)), i)
        '        Next
        '    End With
        'End If

        strSearchQry = strItemSearchQry
        If intPartyDocID > 0 AndAlso BlnEnforcePartyItemLink Then
            strSearchQry = strItemPartySearchQry.Replace("<ID>", CStr(argPartyID))
        End If

        If blnPartyRelatedFldsPresent Then
            Call FillAfterPartySelection()
        End If

        'LoadFromSettings Changes
        If blnPartyLoadFromSettingsPresent Then
            Call fillLoadFromSettingsFlds(argPartyID)
        End If

        If blnLinkedDocFrom Then
            btnPendingDoc.Focus()
            btnPendingDoc.BringToFront()
        End If

        If Not blnCalc Then
            Call CalTotal()
        End If

        Dim strsql As String
        Dim strMsg As String
        If Mode = "A" And intPartyDocID > 0 Then
            strsql = "Select  Message from MstPartyAlertMessage  where MstDocID = " & intDocID & " and PartyId = " & argPartyID & " and PartyDocId=" & intPartyDocID
            strMsg = clsSVT.getDataStringValue(strsql, "", False)
            If strMsg <> "" Then
                MsgBox(strMsg, MsgBoxStyle.Information, "Party Selection Alert Message.")
            End If
        End If

    End Sub

    Private Sub frmTrnSO_Resize(sender As Object, e As EventArgs) Handles Me.Resize
        If Me.Width = 0 Then Exit Sub
        'btnFtrPin.Top = vsfFtr.Top + vsfFtr.Height - btnFtrPin.Height
        'btnFtrPin.Left = vsfFtr.Left + vsfFtr.Width - btnFtrPin.Width

        'btnHdrPin.Top = vsfHdr.Top + vsfHdr.Height - btnHdrPin.Height
        'btnHdrPin.Left = vsfHdr.Left + vsfHdr.Width - btnHdrPin.Width

        'btnTotPin.Top = vsfTot.Top + vsfTot.Height - btnTotPin.Height
        'btnTotPin.Left = vsfTot.Left + vsfTot.Width - btnTotPin.Width

        'btnDetPin.Top = vsfDet.Top + vsfDet.Height - btnDetPin.Height
        'btnDetPin.Left = vsfDet.Left + vsfDet.Width - btnDetPin.Width

        If vsfHdr.Cols(vOthFldCaption).Width >= vsfHdr.Width Then
            vsfHdr.Cols(vOthFldCaption).Width = vsfHdr.Width - 20
        End If

        If vsfFtr.Cols(vOthFldCaption).Width >= vsfFtr.Width Then
            vsfFtr.Cols(vOthFldCaption).Width = vsfFtr.Width - 20
        End If
    End Sub

    Private Sub showBtns()
        If Mode = "A" Or Mode = "M" Then Exit Sub
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

    Private Sub vsfDet_AfterScroll(sender As Object, e As RangeEventArgs) Handles vsfDet.AfterScroll
        vsfTop.ScrollPosition = vsfDet.ScrollPosition
    End Sub

    Private Sub vsfDet_AfterResizeColumn(sender As Object, e As RowColEventArgs) Handles vsfDet.AfterResizeColumn
        Call syncGridCols()
    End Sub

    Private Sub setVSFTop()
        If clsSVT.getDataLongValue("Select Count(*) from UtlDocDsgrFlds where DocID=" & intDocID & " and blnTotalReq='Y'", 0, False) > 0 Then
            blnBalColReq = True
            vsfTop.Font = vsfDet.Font
            vsfTop.Visible = True
            vsfTop.Cols.Count = vsfDet.Cols.Count
            vsfTop.Rows.Count = 1
            Call syncGridCols()
            vsfTop.Cols(vsfTop.Cols.Count - 1).Width = vsfDet.Cols(vsfDet.Cols.Count - 1).Width + SystemInformation.VerticalScrollBarWidth

        Else
            vsfTop.Visible = False
            vsfDet.Top = vsfTop.Top
            vsfDet.Height = vsfDet.Height + vsfTop.Height
        End If
    End Sub

    Private Sub vsfTop_BeforeEdit(sender As Object, e As RowColEventArgs) Handles vsfTop.BeforeEdit
        e.Cancel = True
    End Sub

    Private Sub vsfTop_AfterScroll(sender As Object, e As RangeEventArgs) Handles vsfTop.AfterScroll
        Dim intv As Integer = vsfTop.ScrollPosition.X
        Dim inth As Integer = vsfDet.ScrollPosition.Y
        vsfDet.ScrollPosition = New System.Drawing.Point(intv, inth)
    End Sub

    'Private Sub vsfHdr_Click(sender As Object, e As EventArgs) Handles vsfHdr.Click
    '    StrFldname = vsfHdr.GetData(vsfHdr.Row, vOthFldCode)
    '    Call StatusMsg(StrFldname, intDocID)
    'End Sub

    'Private Sub vsfTot_Click(sender As Object, e As EventArgs) Handles vsfTot.Click
    '    StrFldname = vsfTot.GetData(vsfTot.Row, vOthFldCode)
    '    Call StatusMsg(StrFldname, intDocID)
    'End Sub

    'Private Sub vsfFtr_Click(sender As Object, e As EventArgs) Handles vsfFtr.Click
    '    StrFldname = vsfFtr.GetData(vsfFtr.Row, vOthFldCode)
    '    Call StatusMsg(StrFldname, intDocID)
    'End Sub

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

        If strAuthReq = "Y" AndAlso lblAuthorised.Text = "AUTHORISED" Then
            MsgBox("Current record is Authorised. Cannot edit!", vbInformation, Me.Text)
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
                        strSQL = "Select Top 1 " & CStr(.GetData(argint.GetValue(i), vOthFldPrevTranFldName)).Replace("#", "") & " from " & strDocHdrTblName & " h, " & .GetData(argint.GetValue(i), vOthFldCmbTblName) & " v "
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
                        strSQL = "Select Top 1 " & CStr(.GetData(argint.GetValue(i), vOthFldPrevTranFldName)).Replace("#", "") & " from " & strDocHdrTblName & " where " & strDocIDFldName & " <> " & Val(txtDocID.Text)
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
                            strSQL = "Select Top 1 " & CStr(.GetData(argDT.Rows(i).Item(1), vOthFldPrevTranFldName)).Replace("#", "") & " from " & strDocHdrTblName & " h, " & .GetData(argDT.Rows(i).Item(1), vOthFldCmbTblName) & " v "
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
                            strSQL = "Select Top 1 " & CStr(.GetData(argDT.Rows(i).Item(1), vOthFldPrevTranFldName)).Replace("#", "") & " from " & strDocHdrTblName & " where " & strDocIDFldName & " <> " & Val(txtDocID.Text)
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
                        strSQL = "Select Top 1 " & CStr(.GetData(argDT.Rows(i).Item(0), vOthFldPrevTranFldName)).Replace("#", "") & " from " & strDocHdrTblName & " h, "
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
                        strSQL = "Select Top 1 " & CStr(.GetData(argDT.Rows(i).Item(0), vOthFldPrevTranFldName)).Replace("#", "") & " from " & strDocHdrTblName & " h, "
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

    Private Sub updateDtlFldTotal()
        If blnBalColReq = False Then Exit Sub
        If vsfTop.Visible = False Then Exit Sub
        If vsfDet.Rows.Count <= 1 Then Exit Sub
        With vsfTop             '----Changed
            For i As Int32 = 1 To .Cols.Count - 1
                Call SetColAlign(vsfTop, svtAlign.Center, i)
            Next
        End With
        With vsfDet
            For k As Int32 = intDetCols To .Cols.Count - 1
                If vsfOptions.GetData(k, vOthBlnTotalReq) = "Y" Then
                    Dim dblResult As Double = 0
                    For i As Int32 = 1 To .Rows.Count - 1
                        dblResult = dblResult + Val(.GetData(i, k))
                    Next
                    vsfTop.SetData(0, k, dblResult)
                End If
            Next
        End With
    End Sub

    Private Sub FillAfterPartySelection()
        If Not blnPartyRelatedFldsPresent Then Exit Sub
        Dim ctrlList = GetAllDsgrVsf(Me, GetType(C1.Win.C1FlexGrid.C1FlexGrid))
        For Each argVsf As C1FlexGrid In ctrlList
            If argVsf.Rows.Count <= -1 Then GoTo EndLoop
            With argVsf
                For i As Int32 = 0 To .Rows.Count - 1
                    If UCase(.GetData(i, vOthFldPickUPAfterFld)) = "#SVTPARTY#" Then
                        If .GetData(i, vOthPerAmount) = "Y" Then
                            .SetData(i, vOthFldDataPer, clsSVT.getDataStringValue(Replace(.GetData(i, vOthFldPickUP), "#SVTPARTY#", Val(svtPCtrl.getBillID())), "", False))
                        Else
                            .SetData(i, vOthFldDataValue, clsSVT.getDataStringValue(Replace(.GetData(i, vOthFldPickUP), "#SVTPARTY#", Val(svtPCtrl.getBillID())), "", False))
                        End If
                    End If
                Next
            End With
EndLoop:
        Next
    End Sub

    Private Sub ItemDetailsFillAfterPartySelection(ByVal argRow As Int32)
        With vsfOptions
            For i As Int32 = 0 To .Rows.Count - 1
                If UCase(.GetData(i, vOthFldPickUPAfterFld)) = "#SVTPARTY#" Then
                    vsfDet.SetData(argRow, i, clsSVT.getDataStringValue(Replace(.GetData(i, vOthFldPickUP), "#SVTPARTY#", Val(svtPCtrl.getBillID())), "", False))
                End If
            Next
        End With
    End Sub

    Private Sub vsfHdr_MouseClick(sender As Object, e As MouseEventArgs) Handles vsfHdr.MouseClick, vsfFtr.MouseClick, vsfTot.MouseClick
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
                If blnLinkedDocFrom Then
                    If intItemDocID > 0 AndAlso intColQty > -1 Then
                        argVSF.Col = intColQty
                    Else
                        argVSF.Col = intDetCols + 1
                    End If
                Else
                    argVSF.Col = vDetItemCode
                End If
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

    Private Sub vsfDet_KeyDown(sender As Object, e As KeyEventArgs) Handles vsfDet.KeyDown, vsfHdr.KeyDown, vsfTot.KeyDown, vsfFtr.KeyDown
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

        If blnDetSection AndAlso intItemDocID > 0 AndAlso Not blnLinkedDocFrom AndAlso vsfDet.Col = vDetItemCode AndAlso vsfDet.Rows.Count > 1 AndAlso e.KeyCode = Keys.F4 Then
            If Not (Mode = "M" Or Mode = "A") Then Exit Sub

            strSQL = "Select distinct top " & gIntTopNrows & " " & strFldItemCode & " as [" & gItemCode & "], " & strFldItemName & " as [" & gItemName & "], "
            strSQL &= "i.ShortName, i.HotKey, u.uomcode "
            If gstrVsfItemCodeCols <> "" Then
                strSQL &= "," & gstrVsfItemCodeCols
            End If
            strSQL &= " From " & strItemTblName & " i, MstUOM u, MstItemGroup g"
            strSQL &= " where i.UOMID=iu.UOMID "
            strSQL &= " and g.ItemGroupID=i.ItemGroupID "
            strSQL &= " and i.InActive = 'N' and i.Authorised = 'Y' "
            strSQL &= strItemFilterCond
            strSQL &= " order by " & strFldItemCode

            If intPartyDocID > 0 AndAlso BlnEnforcePartyItemLink Then
                strSQL = "Select " & strFldItemCode & " as [" & gItemCode & "], u.UOMCode, " & strFldItemName & " as [" & gItemName & "] "
                If gstrVsfItemCodeCols <> "" Then
                    strSQL &= "," & gstrVsfItemCodeCols
                End If
                strSQL &= " From " & strItemTblName & " i, MstItemUOM iu, MstUOM u, MstItemGroup g, Mst" & strPartyType & "Item pi"
                strSQL &= " where i.ItemID=iu.ItemID "
                strSQL &= " and u.UOMID=iu.UOMID "
                strSQL &= " and g.ItemGroupID=i.ItemGroupID "
                strSQL &= " and pi.ItemID=iu.ItemID "
                strSQL &= " and pi.UOMID=iu.UOMID "
                strSQL &= " And i.InActive = 'N' and i.Authorised = 'Y' "
                strSQL &= " And pi." & strPartyType & "ID  = " & Val(svtPCtrl.getBillID())
                strSQL &= strItemFilterCond
                strSQL &= " order by " & strFldItemCode
            End If
            'strSQL &= " Order by " & strFldItemCode


            txtDummy.Text = ""
            txtDummy2.Text = ""

            Dim objfrm As New frmUtlSelection
            With objfrm
                .blnMultiColSearch = True
                .FillViewVsf(Me, False, Nothing, strSQL, txtDummy, 0, txtDummy2, 1)
                .Text = " Please Select " & strItemShForm
                .ShowDialog()
                .Close()
            End With

            If Trim(txtDummy.Text) <> "" Then
                Call FillItemDet(txtDummy.Text, vsfDet.Row, argUOM:=txtDummy2.Text)
            End If
        End If
    End Sub

    Private Sub fillSrcDocFlds()
        Dim i As Integer, strOrgQry As String, strSQL As String
        Try
            'New field in UtlTransactions : PickFldsFromSrcDocQry
            'value in the field will be like this
            'select poid, itemslno, porate from trndtlpo where poid = #SRCDOCID# and itemslno = #SRCITEMSLNO#

            strOrgQry = clsSVT.getDataStringValue("Select PickFldsFromSrcDocQry from UTLTransactions where Docid =" & intDocID, 0, False)
            If Trim(strOrgQry) = "" Then Exit Sub
            For i = 1 To vsfDet.Rows.Count - 1
                Dim rs As Data.oledb.oledbDataReader
                strSQL = Replace(strOrgQry, "#SRCDOCID#", vsfDet.GetData(i, vDetSrcDocID))
                strSQL = Replace(strSQL, "#SRCITEMSLNO#", vsfDet.GetData(i, vDetSrcItemSlNo))
                rs = clsSVT.getDataReaderFromSQL(strSQL)
                If Not rs.HasRows Then
                    rs.Close()
                    Exit Sub
                End If
                With rs
                    Do While .Read()
                        For x = 0 To rs.FieldCount - 1
                            For y = intDetCols To vsfOptions.Rows.Count - 1
                                If UCase(vsfOptions.GetData(y, vOthFldCode)) = UCase(rs.GetName(x)) Then
                                    vsfDet.SetData(i, y, GetVal(rs.GetValue(x), ""))
                                    GoTo finishLoop
                                End If
                            Next y

                            For y = 0 To vsfHdr.Rows.Count - 1
                                If UCase(vsfHdr.GetData(y, vOthFldCode)) = UCase(rs.GetName(x)) Then
                                    vsfHdr.SetData(y, vOthFldDataValue, GetVal(rs.GetValue(x), ""))
                                    GoTo finishLoop
                                End If
                            Next y

                            For y = 0 To vsfFtr.Rows.Count - 1
                                If UCase(vsfFtr.GetData(y, vOthFldCode)) = UCase(rs.GetName(x)) Then
                                    vsfFtr.SetData(y, vOthFldDataValue, GetVal(rs.GetValue(x), ""))
                                    GoTo finishLoop
                                End If
                            Next y

                            For y = 0 To vsfTot.Rows.Count - 1
                                If UCase(vsfTot.GetData(y, vOthFldCode)) = UCase(rs.GetName(x)) Then
                                    vsfTot.SetData(y, vOthFldDataValue, GetVal(rs.GetValue(x), ""))
                                    GoTo finishLoop
                                End If
                            Next y
finishLoop:
                        Next x
                    Loop
                    .Close()
                End With
                'If intColrate > -1 Then
                '    strSQL = "Select " & strPendDocShForm & "RATE from " & strPendDtlTblName & " where " & strPendIDFldName & " = " & Val(vsfDet.GetData(i, vDetSrcDocID)) & " and ItemSlno = " & Val(vsfDet.GetData(i, vDetSrcItemSlNo))
                '    vsfDet.SetData(i, intColrate, clsSVT.getDataDoubleValue(strSQL, 0, False))
                'End If
            Next i
            Call FillColfromOtherCols()
        Catch ex As Exception
            MsgBox(ex.Message, vbInformation, Me.Text)
        End Try
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

    Private Sub btnPendingDoc_Click(sender As Object, e As EventArgs) Handles btnPendingDoc.Click
        If Mode = "V" Or Mode = "" Then
            Exit Sub
        End If

        If strPendParty = "Y" AndAlso intPartyDocID > 0 Then
            If svtPCtrl.txtPartyCode.Text = "" Then
                MsgBox("Party Name should not be blank!!", vbInformation, Me.Text)
                svtPCtrl.txtPartyCode.Focus()
                Exit Sub
            End If
        End If

        Call showWaitCursor()
        vsfDet.EndUpdate()
        vsfDet.FinishEditing()
        vsfDet.Col = vDetItemName
        vsfDet.Enabled = False

        Dim objFrm As New frmTrnPendPopUp

        With objFrm
            If intColQty > -1 Then
                .fillPendQry(intDocID, Val(txtDocID.Text), Val(svtPCtrl.getBillID()), vsfDet, intColQty, Me)
            Else
                .fillPendQry(intDocID, Val(txtDocID.Text), Val(svtPCtrl.getBillID()), vsfDet, intDetCols + 1, Me)
            End If
            .ShowDialog()
        End With
        objFrm.Dispose()
        Call fillSrcDocFlds()
        vsfDet.Enabled = True
        Call CalTotal()
        vsfDet.EndUpdate()
        vsfDet.FinishEditing()

        vsfDet.Enabled = True
        If vsfDet.Rows.Count > 1 Then
            For i As Int32 = 1 To vsfDet.Rows.Count - 1
                Call fillVsfDetDefValues(vsfDet, vsfOptions, i)
                If intPartyDocID > 0 Then
                    Call InsertPrevItemData(dtDetPrevTran, Val(svtPCtrl.getBillID()), vsfDet.GetData(i, vDetItemID), i)
                    Call ItemDetailsFillAfterPartySelection(i)
                Else
                    Call InsertPrevItemData(dtDetPrevTran, 0, vsfDet.GetData(i, vDetItemID), i)
                End If
                If intItemDocID > 0 Then
                    Call fillItemRelatedDataFromDocDsgnr(intDocID, Val(vsfDet.GetData(i, vDetItemID)), Me, argRow:=i)
                End If
                Call vsfOptValidateEdit(Me, vsfDet, vsfOptions, i, vDetItemCode, "T", strDocDtlTblName, "", "")
            Next
            vsfDet.Col = intColQty
            vsfDet.Row = 1
            vsfDet.StartEditing()
        End If
        Call showNormalCursor()
    End Sub

    Private Sub frmTrnPartyDoc_Leave(sender As Object, e As EventArgs) Handles Me.Leave

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
        If e.KeyCode = Keys.Tab Then
            If intPartyDocID > 0 Then
                svtPCtrl.Focus()
                svtPCtrl.txtPartyCode.Focus()
            Else
                If vsfDet.Visible Then vsfDet.Focus()
            End If
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

    Public Sub vsfHdr_CellButtonClick(sender As Object, e As RowColEventArgs) Handles vsfHdr.CellButtonClick
        With vsfHdr
            If .GetData(e.Row, vOthFldTxtBlnMultiLine) = "Y" Then
                StrgetText = .GetData(e.Row, vOthFldDataValue)
                Call vsfOptCellButtonClick(StrgetText, Val(.GetData(e.Row, vOthFldDataWidth)), .GetData(e.Row, vOthFldCaption), Me)
                .SetData(e.Row, vOthFldDataValue, StrgetText)
            End If


            If e.Row = intRowCustID Then
                If Not ((Mode = "A") Or (Mode = "M")) Then Exit Sub
                Dim strSQL As String

                Dim frmOBJ As New frmUtlSingleSel
                txtDummy.Text = ""

                strSQL = "Select  CustID,CustCode,PhNo,CardNo"
                strSQL &= " From MstCust"
                strSQL &= " where CustID > 0"
                strSQL &= " Order by 1"

                With frmOBJ
                    Call .FillViewVsf(Me, strSQL, txtDummy, 0, 0)
                    .StartPosition = FormStartPosition.CenterScreen
                    .ShowDialog()
                End With

                If Val(txtDummy.Text) > 0 Then

                    vsfHdr.SetData(intRowCustID, vOthFldDataValue, clsSVT.getDataStringValue("Select CustCode From mstCust where Custid=" & Val(txtDummy.Text), "", False))
                    vsfHdr.SetData(intRowPHNO, vOthFldDataValue, clsSVT.getDataStringValue("Select PHNO From mstCust where Custid=" & Val(txtDummy.Text), "", False))
                    vsfHdr.SetData(intRowCardNo, vOthFldDataValue, clsSVT.getDataStringValue("Select CardNo From mstCust where Custid=" & Val(txtDummy.Text), "", False))
                    vsfHdr.SetData(intRowTotPoints, vOthFldDataValue, clsSVT.getDataLongValue("Select sum(InvPoints - RPoints) from vwredeem where CustID = " & Val(txtDummy.Text), 0, False))
                    If Val(vsfHdr.GetData(intRowTotPoints, vOthFldDataValue)) < gIntAlertIfPointsGreaterThan Then
                        MsgBox("Balance points should be greater than " & gIntAlertIfPointsGreaterThan & " points to Redeem", MsgBoxStyle.Critical, Me.Text)
                    End If
                End If
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

    Private Function getPrintDocFmtNo() As String
        getPrintDocFmtNo = ""
        If IsNothing(dtPrintFmt) Then Return ""
        If dtPrintFmt.Rows.Count <= 0 Then Return ""
        Dim strPrntNo As String
        Dim i As Integer

        strPrntNo = ""
        For i = 0 To dtPrintFmt.Columns.Count - 1
            Select Case GetVal(dtPrintFmt.Columns(i).ColumnName, "")
                Case "F1", "F2", "F3", "F4", "F5", "F6", "F7", "F8", "F9", "F10", "F11", "F12", "F13", "F14", "Misc1", "Misc2", "Misc3"
                    strPrntNo = getUtlDocNoFmt(GetVal(dtPrintFmt.Rows(0).Item(i), ""), strPrntNo)
            End Select
        Next

        If strPrntNo = "" Then Return Trim(txtDocNo.Text)
        Return strPrntNo
    End Function

    Private Function getUtlDocNoFmt(ByVal argStr As String, ByRef argPrntNo As String) As String
        If argStr <> "" Then
            Select Case argStr
                Case "1"
                    argPrntNo = argPrntNo & Trim(cboPfx.Text)
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

    Private Sub vsfDet_KeyPress(sender As Object, e As KeyPressEventArgs) Handles vsfDet.KeyPress

    End Sub

    Private Sub svtbtnctrl_Shortcut(ByVal argoption As String)
        Select Case argoption
            Case "ML"
                svtBtnCtrl.PerformAction(svtBtns.svtMoveLast)
            Case "MN"
                svtBtnCtrl.PerformAction(svtBtns.svtMoveNext)
            Case "MP"
                svtBtnCtrl.PerformAction(svtBtns.svtMovePrev)
            Case "MF"
                svtBtnCtrl.PerformAction(svtBtns.svtMoveFirst)

            Case Else
                svtBtnCtrl.PerformAction(svtBtns.svtFind)
        End Select
    End Sub

    Private Sub frmTrnPartyDoc_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.Control AndAlso e.Shift Then
            Select Case e.KeyCode
                Case Keys.L
                    Call svtbtnctrl_Shortcut("ML")
                Case Keys.F
                    Call svtbtnctrl_Shortcut("MF")
                Case Keys.P
                    Call svtbtnctrl_Shortcut("MP")
                Case Keys.N
                    Call svtbtnctrl_Shortcut("MN")
                Case Keys.F
                    Call svtbtnctrl_Shortcut("")
            End Select
        End If
    End Sub

    Private Sub vsfHdr_AfterResizeColumn(sender As Object, e As RowColEventArgs) Handles vsfHdr.AfterResizeColumn
        If vsfHdr.Cols(vOthFldCaption).Width >= vsfHdr.Width Then
            vsfHdr.Cols(vOthFldCaption).Width = vsfHdr.Width - 20
        End If
    End Sub

    Private Sub vsfFtr_AfterResizeColumn(sender As Object, e As RowColEventArgs) Handles vsfFtr.AfterResizeColumn
        If vsfFtr.Cols(vOthFldCaption).Width >= vsfFtr.Width Then
            vsfFtr.Cols(vOthFldCaption).Width = vsfFtr.Width - 20
        End If
    End Sub

    'LoadFromSettings Changes
    Private Sub fillLoadFromSettingsFlds(argPartyID As Long)
        If Not blnPartyLoadFromSettingsPresent Then Exit Sub
        Dim strResult As String
        Dim ctrlList = GetAllDsgrVsf(Me, GetType(C1.Win.C1FlexGrid.C1FlexGrid))
        For Each argVsf As C1FlexGrid In ctrlList
            If argVsf.Rows.Count <= -1 Then GoTo EndLoop
            With argVsf
                For i As Int32 = 0 To .Rows.Count - 1
                    Select Case Mid(UCase(.GetData(i, vOthFldLoadFldName)), 1, 1)
                        Case "S", "C"
                            strResult = clsSVT.getDataStringValue("Select " & .GetData(i, vOthFldLoadFldName) &
                                   " from " & strPartyTblName & " " & Mid(UCase(.GetData(i, vOthFldLoadFldName)), 1, 1) &
                                   " Where " & strFldPartyID & " = " & argPartyID, "", False)
                            .SetData(i, vOthFldDataValue, strResult)
                    End Select
                Next
            End With
EndLoop:
        Next
    End Sub

    Private Sub vsfHdr_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles vsfHdr.MouseDoubleClick, vsfFtr.MouseDoubleClick
        Dim ht = sender.HitTest
        With sender
            Try
                If ht.Column <> vOthFldDataValue Then
                    If InStr(UCase(GetVal(.GetData(.Row, vOthFldCaption))), "[MST]") > 0 Then
                        Call getMasterFlds(GetVal(.GetData(.Row, vOthFldCmbTblName)), GetVal(.GetData(.Row, vOthFldDataValue)))
                    End If
                End If
            Catch ex As Exception
            End Try
        End With
    End Sub

    Private Sub vsfDet_AfterFreezeRow(sender As Object, e As RowColEventArgs) Handles vsfDet.AfterFreezeRow

    End Sub

    Private Function ChkAccTransaction() As Boolean
        ChkAccTransaction = False

        Dim blnFound As Boolean = False, strSQL As String = ""
        strSQL = " Select TrnID From TrnHdrAcc "
        strSQL &= " Where DocType='" & strSHForm & "' "
        strSQL &= " and TrnID=" & Val(txtDocID.Text)
        strSQL &= " and TrnDate=" & getDateFormatInCond(dtDocDate.Value)
        Call clsSVT.getDataLongValue(strSQL, 0, blnFound)

        If blnFound Then
            MsgBox(Me.Text & " is available in accounting transactions..." & vbNewLine & " Can't Delete...", MsgBoxStyle.Exclamation, Me.Text)
            Exit Function
        End If

        ChkAccTransaction = True
    End Function

    Private Sub svtBtnCtrl_SystemColorsChanged(sender As Object, e As EventArgs) Handles svtBtnCtrl.SystemColorsChanged

    End Sub

    'Private Sub btnROL_Click(sender As Object, e As EventArgs)
    '    Call getROLItems()
    'End Sub

    'Private Sub btnPB_Click(sender As Object, e As EventArgs)

    'End Sub

    'Private Function getROLItems() As Boolean
    '    getROLItems = False
    '    Dim rs As oledb.oledbDataReader

    '    strSQL = " Select ROL from MstItem i, TrnQOH q "
    '    strSQL &= " where i.ItemId = q.ItemId "
    '    strSQL &= " and i.ItemId = '" & intItemId & "' "

    '    rs = clsSVT.getDataReaderFromSQL(strSQL)

    '    If Not rs.HasRows Then
    '        Exit Function
    '    End If

    '    Do While rs.Read

    '        With vsfDet
    '            For i As Int32 = 1 To .Rows.Count - 1
    '                If .GetData(i, vDetQOH) < rs!ROL Then
    '                    Call fillDtlFields(intDocID)
    '                Else
    '                    MsgBox("QOH is greater than ROL!", MsgBoxStyle.Information, Me.Text)
    '                    Exit Function
    '                End If
    '            Next i
    '        End With
    '    Loop
    '    getROLItems = True
    'End Function
End Class