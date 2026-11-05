Imports C1.Win.C1FlexGrid
Imports System.IO
Imports System.Data.oledb
Imports System.ComponentModel

Public Class frmMstBOM
    'This form  is applicable for Both Party and Non Party Document
    Dim clsSVT As New svtLibrary
    Dim dtSearch As DataTable
    Dim dvSearch As DataView
    Dim intMaxDocID As Long
    Dim svtTrn As oledb.oledbTransaction
    Dim ctrlList As Object 'IEnumerable(Of C1FlexGrid)

    Public Mode As String 'Control Transaction Add, Edit and Delete
    Public lngViewDocID As Long
    Dim strPmtTermsList As String

    'for audit track
    Dim blnAuditTrack As Boolean = False
    Dim strAuditHdrSQL As String
    Dim strAuditDtlSQL As String

    'Transaction Table Related Information
    Dim strFrmCaption As String
    Dim strDocHdrTblName As String
    Dim strDocDtlTblName As String
    Dim strDocIDFldName As String
    Dim strDocNoFldName As String
    Dim strPfxTYpe As String
    Dim strDocDateFldName As String
    Dim strQtyFldName As String

    'UtlTransactions Related Information
    Dim intDocID As Integer
    Dim intQtyDecimals As Integer
    Dim blnShowFileAttach As Boolean
    Dim blnPartyRelatedFldsPresent As Boolean
    Dim strPrintType As String
    Dim strAuthReq As String
    Dim strAuthQuery As String
    Dim intNoOfAuth As Integer
    Dim blnAuthCalcluate As Boolean
    Dim strAddReduceStock As String
    Dim blnLinkedToDoc As Boolean
    Dim strLinkedTOHdrTblName As String
    Dim strLinkedTODtlTblName As String
    Dim strLinkedToDocNoFldName As String
    Dim strLinkedTODocQtyFldName As String
    Dim strLinkedTODocFldIDName As String

    Dim intLastCol As Integer
    Dim blnColFreeze As Boolean
    Dim strItemQueryPresent As String
    Dim strItemQuery As String
    Dim blnShowBOM As Boolean
    Dim blnallowToChangePfx As Boolean
    Dim strPfxWiseNo As String
    Dim strDocStdDtlWidth As String
    Dim blnFreezeDoc As Boolean
    Dim iNumOfDays As Integer
    Dim strSHForm As String
    Dim intPartyDocID As Int32
    Dim intItemDocID As Int32
    Dim blnCalc As Boolean
    Dim intColQty As Int32
    Dim intColrate As Integer
    Dim intColCustPart As Integer
    Dim strTenderFld As String = ""
    Dim intRowTenderFld As Int32

    'Party Related Information
    Dim strPartyTblName As String
    Dim strFldPartyID As String
    Dim strFldPartyCode As String
    Dim strFldPartyName As String
    Dim strPartyAddressTblName As String = "MstPartyAddress"
    Dim strPartyContactTbl As String = "MstPartyContacts"
    Dim strPartyFilterCond As String
    Dim strPartyFlds As String

    'Item Related Information
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
    Dim strHDRQry As String, strDTLQry As String
    Public strList As String

    'library used
    Dim strSQL As String
    Dim strDtlSQL As String

    'For security
    Dim strRights As String

    'for doc number format
    Dim strPfx As String, strNo As String
    Dim strAllowDocNoChngADDmode As String
    Dim strAllowDuplicateItem As String
    Dim intdetrows As Integer
    Dim sngDefQty As Single
    Dim strAmmReq As String

    Dim frmUtlExp As New frmUtlExplosion
    Dim intLevel As Integer

    'declarations for detail grid
    Const vDetSlNo = 0
    Const vDetItemID = 1
    Const vDetItemCode = 2
    Const vDetItemName = 3
    Const vDetUOM = 4
    Const vDetBOM = 5
    Const vDetQty = 6
    Public Const intDetCols = 7

    Const arrItemID = 0
    Const arrQty = 1
    Const arrLevel = 2
    Const arrKey = 3
    Const arrPrntKey = 4
    Const arrItemCode = 5
    Const arrDocDt = 6
    Const arrParentCode = 6
    Const arrParentID = 7
    Const arrTotCols = 8

    Const vEffID = 0
    Const vEffFrom = 1
    Const vEffTo = 2

    'for Reference we have maintain below variables 
    Dim intPartyID As Integer
    Dim intItemId As Long

    Dim blnTimer As Boolean = False
    Public blnRestore As Boolean = False
    Public strRestoreFileName As String = ""
    Dim strRestorePath As String
    Dim blnItemScan As Boolean = False

    Dim blnVsfMax As Boolean = False
    Dim blnVHdr As Boolean = False
    Dim blnVFtr As Boolean = False
    Dim blnVTot As Boolean = False

    Dim intVsfSize, intVsfSize2, intVsfPos, intVsfPos2, intBtnSize, intBtnSize2, intBtnPos, intBtnPos2, intSvtSize, intSvtSize2, intSvtPos, intSvtPos2 As Int32
    Dim intVHdrSize, intVHdrSize2, intVHdrPos, intVHdrPos2, intVFtrSize, intVFtrSize2, intVFtrPos, intVFtrPos2 As Int32
    Dim intVTotSize, intVTotSize2, intVTotPos, intVTotPos2 As Int32
    Dim intDPinSize, intDPinSize2, intDPinPos, intDPinPos2 As Int32
    Dim intfraSize, intfraSize2

    Dim BlnRevised As Boolean, intOLDBOMID As Long, intBOMItemID As Long
    Dim strEffDateReq As String
    Dim strBOMItemCond As String
    Dim strChildItemCond As String
    Dim strShowTreeView As String
    Dim blnChildForm As Boolean
    Dim blnAuthReq As Boolean = False

    Dim dtUser As DataTable
    Dim dvUser As DataView

    Dim strItemText As String = ""
    Dim intBomPId As Int32

    Public Sub svtBtnCtrl_btnClicked(ByVal argBtnClicked As Integer) Handles svtBtnCtrl.btnClicked
        Select Case argBtnClicked
            Case svtBtns.svtNew
                If Not chkUserRights(strRights, chkRightsPos.AddRights, Me.Text) Then Exit Sub
                Call AddProc()
                Mode = "A"
                Call ResizePanels()
                lblfrm.Text = "New - " & Me.Text
                TimerStartEvent()
                cboItemCode.Focus()

            Case svtBtns.svtClose
                Call frmMain.CloseSelectedTabPage(Me)

            Case svtBtns.svtEdit
                If Not chkUserRights(strRights, chkRightsPos.EditRights, Me.Text) Then Exit Sub
                If Val(txtDocID.Text) <= 0 Then Exit Sub
                If strEffDateReq = "Y" Then
                    If vsfEff.Rows.Count > 1 Then
                        If Not lblLatest.Visible Then
                            MsgBox("Only latest bom can be edited!", vbInformation, Me.Text)
                            Exit Sub
                        End If
                    End If
                    dtEffFrom.Enabled = False
                    dtEffTo.Enabled = False
                End If
                pnlSearch.Visible = False
                enableDisableFlds(True)
                svtBtnCtrl.PerformAction(argBtnClicked)
                lblfrm.Text = "Edit - " & Me.Text
                TimerStartEvent()
                Mode = "M"
                Call ResizePanels()


            Case svtBtns.svtSave
                vsfHdr.Enabled = False
                If Val(txtNo.Text) <= 0 Then Exit Sub
                If validFlds() Then
                    If SaveDoc() Then
                        svtBtnCtrl.PerformAction(argBtnClicked)
                        Dim intTreeId As Int32
                        intTreeId = clsSVT.getDataLongValue("Select Bomid from MstHdrBOM where BOmNo= " & txtDocNo.Text, 0, False)
                        Call ShowTreeView(intTreeId)
                        enableDisableFlds(False)
                        lblfrm.Text = "View - " & Me.Text
                        pnlSearch.Visible = True
                        TimerStopEvent()
                        Mode = "V"
                        Call ResizePanels()
                    End If
                End If
                vsfHdr.Enabled = False

            Case svtBtns.svtPrint
                If Not chkUserRights(strRights, chkRightsPos.PrintRights, Me.Text) Then Exit Sub
                If Val(txtDocID.Text) <= 0 Then Exit Sub
                Call prntCrpt()


            Case svtBtns.svtCancel
                Mode = "V"
                Call clearFlds()
                enableDisableFlds(False)
                pnlSearch.Visible = True
                txtSearch.Focus()
                Call DateFreezing(Me, Mode)
                vsfView.Visible = False
                lblfrm.Text = "View - " & Me.Text
                svtBtnCtrl.Focus()
                svtBtnCtrl.PerformAction(argBtnClicked)
                Call ResizePanels()
                PnlCopyFrom.Visible = False
                TimerStopEvent()
            Case svtBtns.svtMoveFirst, svtBtns.svtMoveLast, svtBtns.svtMoveNext, svtBtns.svtMovePrev

                If Not chkUserRights(strRights, chkRightsPos.ViewRights, Me.Text) Then Exit Sub
                Call FillDocDetails(gMoveRecords(argBtnClicked, strDocHdrTblName, strDocIDFldName, True, txtDocID.Text, ""))
                lblfrm.Text = "View - " & Me.Text

            Case svtBtns.svtDelete
                If txtDocID.Text = "" Then Exit Sub
                If Not chkUserRights(strRights, chkRightsPos.DelRights, Me.Text) Then Exit Sub
                If vsfEff.Rows.Count > 1 Then
                    If Not lblLatest.Visible Then
                        MsgBox("Only latest bom can be Deleted!", vbInformation, Me.Text)
                        Exit Sub
                    End If
                End If
                svtTrn = svtConn.BeginTransaction(IsolationLevel.ReadCommitted)
                If deleteDocument() Then
                    'Call UpdtAuditTrailTable(0, "D", cboItemCode.Text, svtTrn)
                    svtTrn.Commit()
                    MsgBox("BOM Deleted successfully", vbInformation, Me.Text)
                    clearFlds()
                Else
                    svtTrn.Rollback()
                End If

            Case svtBtns.svtFind
                Mode = ""
                pnlSearch.Visible = True
                txtSearch.Focus()
                If Not chkUserRights(strRights, chkRightsPos.ViewRights, Me.Text) Then Exit Sub
                Call showViewTrnForm(Me, strDocHdrTblName, "Listing of " & strFrmCaption, "", "", , strFrmCaption, "", intDocID)
                ' Call showViewForm(Me, strDocHdrTblName, "Listing of " & Me.Text, "", "", " and " & strCodeName & " <> '-'", Me.Text, strCodeName, intDocID)

        End Select
        svtBtnCtrl.PerformAction(argBtnClicked)
    End Sub

    Private Sub ucBtnEnabledisable()
        With ucBtnTools
            .btnVisible("", False, False)
            .tsbExport.Visible = True
            .tsbFreeze.Visible = False
            .tsbDocExport.Visible = False
            .tsbCopyFrom.Visible = False
            .tsbCopy.Visible = False
            .tsbPaste.Visible = False
            .tsbCalTotal.Visible = False
            .tsbMultiDoc.Visible = False
            .tsbBOM.Visible = False
            .tsbInserDoc.Visible = False
            .tsbImport.Visible = False
            .tsbTemplate.Visible = False
            .tsbAmmend.Visible = False
            .tsbTender.Visible = False
            .tsbMoveUp.Visible = False
            .tsbMoveDown.Visible = False
            .tsbInsert.Visible = True
            .tsbRemove.Visible = True
        End With
    End Sub

    Private Function deleteDocument() As Boolean
        Dim strItemCode As String = "", intBOMID As Long

        deleteDocument = False

        If cboItemCode.Text <> "" Then
            strItemCode = cboItemCode.Text
        End If

        If dtEffTo.Value <> "31/12/9998" Then
            MsgBox("Only Latese bom will be allowed to delete...", vbInformation, Me.Text)
            Exit Function
        End If

        If strItemCode <> "" Then
            If MsgBox("Are you sure you want to delete the Item: '" & Trim(strItemCode) & "'?", MsgBoxStyle.YesNo + vbDefaultButton2 + MsgBoxStyle.Critical, "Deletion Confirmation...") = vbNo Then Exit Function

            Try
                'for audit track
                Call RecordAuditLog(strFrmCaption, Val(txtDocID.Text), UserActions.DeleteAction, "MstHdrBOM", strAuditHdrSQL, "", svtTrn)

                strSQL = "select top 1 BOMID from MstHdrBOM where BOMID < " & Val(txtBOMID.Text) & " order by BOMID desc"
                intBOMID = clsSVT.getDataLongValue(strSQL, 0, False, svtTrn)

                strSQL = "Delete from MstHdrBOM where BOMID = " & Val(txtBOMID.Text)
                If Not clsSVT.execSQLStatement(strSQL, svtTrn) Then
                    Exit Function
                End If

                strSQL = "Delete from MstDtlBOM where BOMID = " & txtBOMID.Text
                If Not clsSVT.execSQLStatement(strSQL, svtTrn) Then
                    Exit Function
                End If

                strSQL = "update MstHdrBOM set EffTo = '31 Dec 9998', LatestBOM='Y' where BOMId = " & intBOMID
                If Not clsSVT.execSQLStatement(strSQL, svtTrn) Then
                    Exit Function
                End If

            Catch ex As Exception
                MsgBox("Error while Deleting data!" & vbCrLf & ex.Message, vbCritical, Me.Text)
                Exit Function
            End Try

            'Call fillDocDetails(txtFGItemCode.Text)
        End If

        deleteDocument = True
    End Function

    Private Sub AddProc()
        vsfDet.RowSel = -1
        Call clearFlds()
        Call enableDisableFlds(True)
        Call DateFreezing(Me, Mode)
        Call fillVsfHFTDefValues(Me)
        Call FillAutoGenTrnNo()
        vsfDet.Rows.Count = 1
        vsfDet.Rows.Count = 2
        PnlCopyFrom.Visible = True
        PnlCopyFrom.BringToFront()
        ReArrangeSlNo(vsfDet, vDetSlNo)
        StMsg()
        dtEffFrom.Enabled = True
    End Sub

    Private Sub clearFlds()
        Mode = "V"
        enableDisableFlds(False)
        txtDocID.Text = ""
        txtDocNo.Text = ""
        txtNo.Text = ""
        txtSearch.Text = ""
        dtDocDate.Value = getSystemDate()
        Call ClearAddnlGrid(Me)
        vsfDet.Rows.Count = 1
        vsfDet.Rows.Count = 2
        ReArrangeSlNo(vsfDet, vDetSlNo)
        Call clearAddress()
        lblApproved.Visible = False
        pnlSearch.Visible = True
        cboItemCode.Text = ""
        lblItemName.Text = ""
        lblUOM.Text = ""
        If strEffDateReq = "Y" Then
            dtEffFrom.MinDate = "01/01/1900"
            dtEffFrom.Value = getSystemDate()
            dtEffTo.Value = CDate("31/12/9998")
            dtEffFrom.Enabled = False
        End If
        PnlCopyFrom.Visible = False
        tvBOM.Nodes.Clear()
        lblLatest.Visible = False
        vsfEff.Rows.Count = 1
        txtSearch.Focus()
        Call ResizePanels()
    End Sub

    Public Sub enableDisableFlds(ByVal argFlag As Boolean)
        cboItemCode.Enabled = argFlag
        Lock(txtNo, Not argFlag)
        btnItemCode.Enabled = argFlag
        PnlCopyFrom.Visible = argFlag
        pnlSearch.Visible = Not argFlag
    End Sub

    Private Sub frmMstBOM_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        If cboPfx.Text <> "" Then
            Call INIStore("Prefix", Me.Text, cboPfx.Text)
        End If
        e.Cancel = False
        If Mode = "A" Or Mode = "M" Then
            If MsgBox("Are you sure you want to exit the form without saving?!", MsgBoxStyle.Question _
                      + MsgBoxStyle.YesNo + vbDefaultButton2, "Confirmation...") = MsgBoxResult.No Then
                e.Cancel = True
                Exit Sub
            End If
        End If
    End Sub

    Private Sub frmMstBOM_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyUp
        Dim blnFnKey As Boolean = False
        If e.KeyCode = Keys.F5 Then
            Dim strSQL As String
            strSQL = "Select itemcode "
            strSQL &= "from MstItem where InActive <> 'Y' and BOMItem='Y'"
            strSQL &= " and ItemID not in(Select BOMItemID from MstHdrBOM) "
            If strBOMItemCond <> "" Then
                strSQL &= strBOMItemCond
            End If
            strSQL &= "order by itemcode"
            Call bindCombo(cboItemCode, "itemcode", "itemcode", strSQL)
            strSQL = " Select distinct ItemCode from MstItem i, MstHdrBOM b where i.ItemCode <> '' and i.ItemID = b.BOMItemID and InActive <> 'Y'"
        End If
        If e.KeyCode = Keys.F9 And e.Control Then
            '958, 579
            Call initFormSize(Me, strDocHdrTblName, True, 1149, 614, 0, 0)
            Call initFormVsfDet(Me, strDocHdrTblName, vsfDet, "DET")
            Call initFormVsfDet(Me, strDocHdrTblName, vsfDet, "View")
            MsgBox("Restored Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
        If e.KeyCode = Keys.F8 And e.Control = True Then
            Call SaveActFormSettings(Me, strDocHdrTblName)
            Call SaveActFormVsfDet(Me, strDocHdrTblName, "DET", vsfDet)
            Call SaveActFormVsfDet(Me, strDocHdrTblName, "HDR", vsfHdr)
            Call SaveActFormVsfDet(Me, strDocHdrTblName, "Search", vsfSearch)
            Call SaveActFormVsfDet(Me, strDocHdrTblName, "View", vsfView)
            MsgBox("Saved Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
        If (e.Control) Or blnFnKey Then
            Call svtBtnCtrl.btnCtrlKeyUp(sender, e)
        End If
        Call raiseShortCutEvent(e)

    End Sub

    Public Sub initForm(ByVal argDocID As Integer)
        intDocID = argDocID
        Dim intMenuID As Integer = clsSVT.getDataLongValue("Select MenuItemID from MstMenu where DocID = " & intDocID, 0, False)
        strRights = getUserRights(intMenuID)
        Call getUtlTrnData(intDocID)
        Call initScreen()
        KeyPreview = True
        Mode = "V"
        ctrlList = GetAllDsgrVsf(Me, GetType(C1.Win.C1FlexGrid.C1FlexGrid))
    End Sub

    Private Sub initScreen()
        Dim j As Integer

        strEffDateReq = UCase(getGsettings("BOM", "EffDateReq", "N"))
        strBOMItemCond = getGsettings("BOM", "BOMItemCond", "")
        strChildItemCond = getGsettings("BOM", "ChilItemCond", "")
        strShowTreeView = UCase(getGsettings("BOM", "ShowTreeView", "N"))

        vsfView.Visible = False
        cboPfx.Text = ""
        Dim Ilistopts As New List(Of String)
        vsfHdr.Rows.Count = 0
        vsfFtr.Rows.Count = 0
        vsfTot.Rows.Count = 0

        strAllowDocNoChngADDmode = clsSVT.getDataStringValue("Select DocNoChange from UtlDocNoFmt where DocID = " & intDocID, "Y", False)

        strPfxTYpe = ""

        Call InitAddnlFldGrids(Me, vsfHdr, "Header", intDocID, tbAdnlFlds)
        Call InitAddnlFldGrids(Me, vsfFtr, "Footer", intDocID, tbAdnlFldsFtr)
        Call InitAddnlFldGrids(Me, vsfTot, "Total", intDocID)

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

        If clsSVT.getDataStringValue("Select BlnFreezeDoc from UTLTransactions where DocID =" & intDocID, "N", False) = "Y" Then
            blnFreezeDoc = True
            iNumOfDays = clsSVT.getDataStringValue("Select AllowEditDeleteForDays from UTLTransactions where DocID =" & intDocID, 0, False)
        End If

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

        PnlCopyFrom.Visible = False
        Dim strItemShForm As String
        strItemShForm = clsSVT.getDataStringValue("Select DocShForm From UtlTransactions where DocID = " & Val(intItemDocID), "", False)


        With vsfEff
            .Cols.Count = 3
            .Rows.Count = 1
            .Cols(vEffID).Visible = False
            .SetData(0, vEffFrom, "Eff.From")
            .SetData(0, vEffTo, "Eff.To")
            SetColWidth(vsfEff, -1, 161, 45)
        End With

        With vsfDet
            .Cols.Count = intDetCols
            .Rows.Count = 2
            .SetData(0, vDetSlNo, "Sl No")
            .SetData(0, vDetItemCode, strItemShForm & " Code")
            .SetData(0, vDetItemName, strItemShForm & " Name")
            .SetData(0, vDetQty, "Qty")
            .SetData(0, vDetUOM, "UOM")
            .SetData(0, vDetBOM, "BOM")
            .Cols(vDetBOM).DataType = GetType(Boolean)
            .Cols(vDetQty).DataType = GetType(Decimal)
            .Cols(vDetSlNo).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.LeftCenter
            strDocStdDtlWidth = "525, 105, 15, 2505, 1665, 810, 930, 1155, 1125, 915"

            Call SetColAlign(vsfDet, svtAlign.Left, vDetItemCode, vDetItemName)
            '.Cols(vDetItemCode).ComboList = "..."
            Call SetHideCol(vsfDet, True, vDetItemID)
        End With
        Call assignDefPFX()
        initItemVsfCombo()

        Call InitDetAddnlGrid(Me, vsfDet, vsfOptions, intDetCols, strDocDtlTblName, intDocID)
        Call SetFlexColor(vsfDet, "", vDetSlNo, vDetItemName, vDetUOM, vDetBOM)
        Call SetFlexColor(vsfDet, "M", vDetItemCode, vDetQty)
        intLastCol = 1
        For j = vsfDet.Cols.Count - 1 To 0 Step -1
            If vsfDet.Cols(j).Visible = True Then
                intLastCol = j
                Exit For
            End If
        Next

        strSQL = "Select itemcode "
        strSQL &= "from MstItem where InActive <> 'Y' and BOMItem='Y'"
        strSQL &= " and ItemID not in(Select BOMItemID from MstHdrBOM) "
        If strBOMItemCond <> "" Then
            strSQL &= strBOMItemCond
        End If
        strSQL &= "order by itemcode"
        Call bindCombo(cboItemCode, "itemcode", "itemcode", strSQL)
        cboItemCode.Text = ""

        'intQtyDecimals = clsSVT.getDataLongValue("Select NoOfDecimalsInQtyFld from UtlTransactions where docID = " & intDocID, 0, False)
        'If intQtyDecimals > 0 Then
        '    vsfDet.Cols(vDetQty).Format = "#####." & Strings.StrDup(intQtyDecimals, "0")
        'Else
        '    vsfDet.Cols(vDetQty).Format = "#####"
        'End If

        With vsfOptions
            .SetData(vDetSlNo, vOthFldDataType, "Numeric")
            .SetData(vDetItemID, vOthFldDataType, "Numeric")
            .SetData(vDetItemCode, vOthFldDataType, "Text")
            .SetData(vDetItemName, vOthFldDataType, "Text")
            .SetData(vDetItemCode, vOthFldCode, "ITEMCODE")
            intColQty = getColNoFromAddnlDetGrid(vsfOptions, vsfDet, strSHForm & "QTY")
            intColrate = getColNoFromAddnlDetGrid(vsfOptions, vsfDet, strSHForm & "RATE")
            intColCustPart = getColNoFromAddnlDetGrid(vsfOptions, vsfDet, "CUSTPARTNO")
            '.SetData(intColQty, vOthFldCode, "SVTPRODQTY")
            '.SetData(intColQty, vOthFldDataType, "Numeric")
        End With

        If clsSVT.getDataStringValue("Select FldPickUpAfterFld from UtlDocDsgrFlds where DocID = " & intDocID & " and FldPickUpAfterFld = '#SVTPARTY#'", "", False) <> "" Then
            blnPartyRelatedFldsPresent = True
        Else
            blnPartyRelatedFldsPresent = False
        End If
        'strAllowDuplicateItem = clsSVT.getDataStringValue("Select blnAllowDuplicateItems from UtlTransactions where Docid=" & intDocID, "", False)
        If strTenderFld <> "" Then
            intRowTenderFld = getRowNoFromAddnlGrid(vsfTot, strTenderFld)
        End If

        If blnAuthReq Then
            svtBtnCtrl.addOption("Approve")
        End If

        If strEffDateReq = "Y" Then
            svtBtnCtrl.addOption("Revise")
            gbEff.Visible = True
            btnRevision.Visible = True
        Else
            gbEff.Visible = False
            btnRevision.Visible = False
        End If

        svtBtnCtrl.addOption("Show Tree View")
        If strShowTreeView = "Y" Then
            tvBOM.Visible = True
        Else
            'tbAdnlFlds.Height = vsfHdr.Rows(0).Height * 2
            pnlDet.Width = pnlDet.Width + pnlTree.Width
            pnlDet.BringToFront()
            tvBOM.Visible = False
        End If
        PnlCopyFrom.Visible = False
    End Sub

    Private Sub getUtlTrnData(ByVal argDocID As Integer)
        'Dim rsDummy As oledb.oledbDataReader

        'rsDummy = clsSVT.getDataReaderFromSQL("Select * from UtlTransactions where DocID = " & argDocID)

        'With rsDummy
        '    If .HasRows Then
        '        .Read()
        '        strSHForm = GetVal(!DocShForm, "")
        '        strDocNoFldName = strSHForm & "No"
        '        strDocIDFldName = strSHForm & "ID"
        '        strQtyFldName = strSHForm & "Qty"
        '        strDocDateFldName = strSHForm & "Date"
        '        strDocHdrTblName = "MstHdr" & strSHForm
        '        strDocDtlTblName = "MstDtl" & strSHForm
        '        strAmmReq = GetVal(!AmmReq, "N")
        '        strAuthReq = GetVal(!AuthReq, "N")
        '        If clsSVT.getDataStringValue("Select FileAttachReq from UtlTransactions where DocID = " & intDocID, "N", False) = "Y" Then
        '            blnShowFileAttach = True
        '        Else
        '            blnShowFileAttach = False
        '        End If
        '        blnAuthCalcluate = False
        '        strFrmCaption = !DocName
        '        '    strItemQueryPresent = !BlnItemQuery
        '        strItemQuery = ""
        '        If strItemQueryPresent = "Y" Then strItemQuery = !ItemQuery

        '        Me.Text = GetVal(!DocName, "")
        '        If GetVal(!LinkedTODoc, "N") = "Y" Then
        '            blnLinkedToDoc = True
        '        Else
        '            blnLinkedToDoc = False
        '        End If

        '        If GetVal(!AuthReq, "N") = "Y" Then
        '            blnAuthReq = True
        '        Else
        '            blnAuthReq = False
        '        End If

        '        'lblFrm.Text = strFrmCaption
        '        strLinkedTOHdrTblName = "TrnHdr" & GetVal(!LinkedTODocShForm, "")
        '        strLinkedTODtlTblName = "TrnDtl" & GetVal(!LinkedTODocShForm, "")
        '        strLinkedToDocNoFldName = GetVal(!LinkedTODocShForm, "") & "No"
        '        strLinkedTODocQtyFldName = GetVal(!LinkedTODocShForm, "") & "Qty"
        '        strLinkedTODocFldIDName = GetVal(!LinkedTODocShForm, "") & "ID"

        '        Me.Tag = Me.Text
        '        lblDocNo.Text = strSHForm & " No."
        '        'intPartyDocID = GetVal(!MstDocIDParty, 0)

        '        'If intPartyDocID > 0 Then
        '        '    Dim strPartyShForm As String
        '        '    strPartyShForm = clsSVT.getDataStringValue("Select DocShForm From UtlTransactions where DocID = " & Val(intPartyDocID), "", False)
        '        '    strPartyTblName = "Mst" & strPartyShForm
        '        '    strFldPartyID = strPartyShForm & "ID"
        '        '    strFldPartyCode = strPartyShForm & "Code"
        '        '    strFldPartyName = strPartyShForm & "Name"
        '        '    strPartyFilterCond = GetVal(!PartyFilterCond, "")
        '        'End If

        '        strAddReduceStock = ""
        '        If GetVal(!AffectsStock, "") = "Add" Then
        '            strAddReduceStock = "ADD"
        '        End If
        '        If GetVal(!AffectsStock, "") = "Reduce" Then
        '            strAddReduceStock = "RED"
        '        End If
        '        ' sngDefQty = GetVal(!DefaultQty, 0)
        '        strPrintType = GetVal(!PrintAfterSave, "N")

        '        If GetVal(!OfflineCalc, "N") = "Y" Then
        '            blnCalc = True
        '        End If

        '        intItemDocID = GetVal(!MstDocIDItem, 0)
        '        If intItemDocID = 0 Then
        '            MsgBox("No Item Table Present.. Application Closed...", MsgBoxStyle.Information, Me.Text)
        '            Me.Close()
        '        Else
        '            strItemShForm = clsSVT.getDataStringValue("Select DocShForm From UtlTransactions where DocID = " & Val(intItemDocID), "", False)
        '            strItemTblName = "Mst" & strItemShForm
        '            strFldItemID = strItemShForm & "ID"
        '            strFldItemSlno = strItemShForm & "Slno"
        '            strFldItemCode = strItemShForm & "Code"
        '            strFldItemName = strItemShForm & "Name"
        '            strItemFilterCond = GetVal(!ItemFilterCond, "")
        '        End If

        '        If GetVal(!AllowTenderChange, "N") = "Y" Then
        '            strTenderFld = UCase(GetVal(!TenderChangeFld, ""))
        '        End If

        '        Call ucBtnTools.HideCopyFrom(GetVal(!AllowCopyFrom, "N"))
        '        Call ucBtnTools.HideAttachment(GetVal(!FileAttachReq, "N"))
        '        Call ucBtnTools.HideAmmend(GetVal(!AmmReq, "N"))
        '        Call ucBtnTools.HideMultiDoc(GetVal(!MultiDocGen, "N"))
        '        Call ucBtnTools.HideInsertDoc(GetVal(!AllowLoadFromAnotherDoc, "N"))
        '        Call ucBtnTools.HideTender(GetVal(!AllowTenderChange, "N"))
        '        Call ucBtnTools.HideImpExp(GetVal(!AllowImpExp, "N"))
        '        Call ucBtnTools.HideBOM(GetVal(!AllowLoadFromBOM, "N"))
        '        Call ucBtnTools.HideTotal(GetVal(!OfflineCalc, "N"))

        '    End If
        '    .Close()
        'End With

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
                'strDocHdrTblName = "TrnHdr" & strSHForm
                'strDocDtlTblName = "TrnDtl" & strSHForm
                strDocHdrTblName = "MstHdr" & strSHForm
                strDocDtlTblName = "MstDtl" & strSHForm

                strAuthReq = GetVal(!AuthReq, "N")
                strAuthQuery = GetVal(!NoOfAuthQueryCond, "")
                intNoOfAuth = GetVal(!NoOfAuth, 0)

                strAddReduceStock = ""
                If GetVal(!AffectsStock, "") = "Add" Then
                    strAddReduceStock = "ADD"
                End If
                If GetVal(!AffectsStock, "") = "Reduce" Then
                    strAddReduceStock = "RED"
                End If


                blnFreezeDoc = IIf(GetVal(!blnFreezeDoc, "N") = "N", False, True)
                strTenderFld = UCase(GetVal(!TenderChangeFld, ""))

                If GetVal(!OfflineCalc, "N") = "Y" Then
                    blnCalc = True
                End If

                intPartyDocID = GetVal(!MstDocIDParty, 0)

                intItemDocID = GetVal(!MstDocIDItem, 0)

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

    'Private Sub fillPartyAddress()
    '    Dim rs As oledb.oledbDataReader, strSQL As String
    '    Dim intRow As Integer
    '    Call clearAddress()
    '    intRow = 0
    '    intRow = intRow + 1
    '    If Mode = "" Then
    '        Exit Sub
    '    End If
    '    strSQL = "Select * from " & strPartyAddressTblName & " where " & strFldPartyID & " = " & intPartyID
    '    rs = clsSVT.getDataReaderFromSQL(strSQL)
    '    Do While rs.Read
    '    Loop

    '    If Mode = "" Then
    '        Exit Sub
    '    End If

    '    If blnPartyRelatedFldsPresent Then
    '    End If
    'End Sub

    Private Sub clearAddress()

    End Sub

    Public Sub FillAutoGenTrnNo()
        Dim intTrnNo As Long
        intTrnNo = Val(TrnNoAutoGen("MstHdrBOM", strDocNoFldName, cboPfx.Text, dtDocDate.Value, intDocID, "", IIf(strPfxWiseNo = "Y", True, False)))
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
        If Mode = "A" Or Mode = "M" Then
            e.Handled = True
        End If
    End Sub

    Private Sub initItemVsfCombo()
        Dim strSQL As String

        strSQL = "Select ItemCode, ItemName "
        strSQL &= " From MstItem "
        strSQL &= " where InActive = 'N'  "
        strSQL &= " and Authorised = 'Y'  "
        strSQL &= " order by ItemCode   "

        strSQL = getItemSearchGridQuery(strSQL)

        Call showWaitCursor()
        dtSearch = clsSVT.getDATATable(strSQL)
        dvSearch = New DataView(dtSearch)
        vsfView.DataSource = dvSearch
        Call showNormalCursor()
    End Sub

    Private Sub vsfDet_AfterEdit(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfDet.AfterEdit
        If Mode = "V" Or Mode = "" Then Exit Sub
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
        Select Case e.Col
            Case vDetSlNo, vDetUOM, vDetBOM, vDetItemName
                e.Cancel = True
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
        Dim StrQry As String

        applyFilterFromGrid = False
        StrQry = ""
        With vsfDet
            If .Editor.Text = "" Then
                vsfView.Visible = False
                Exit Function
            End If
            Select Case gDBTypeCurrent
                Case gDbTypeAccess
                    StrQry = StrQry & strFldItemCode & " like '*" & .Editor.Text & "*' "
                    'StrQry = StrQry & strFldItemName & " like '*" & .Editor.Text & "*'"
                Case Else
                    If .Editor.Text = " " Then
                        dvSearch.RowFilter = "1 = 1"
                        Exit Function
                    Else
                        StrQry = StrQry & strFldItemCode & " like '%" & .Editor.Text & "%'" ' Or "
                        'StrQry = StrQry & strFldItemName & " Like '%" & .Editor.Text & "%'"
                    End If
            End Select
            StrQry = StrQry & " and " & strFldItemCode & " <> '" & Trim(cboItemCode.Text) & "'"
            dvSearch.RowFilter = StrQry
        End With
        applyFilterFromGrid = True
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
        If Not FillItemDet(vsfView.GetData(vsfView.Row, 0), vsfDet.Row) Then
            'MsgBox("Invalid Item!!", vbInformation, Me.Text)
            vsfView.Visible = True
            Exit Sub
        Else
            vsfView.Visible = False
            vsfDet.Focus()
            vsfDet.Col = vDetQty
            vsfDet.StartEditing()
        End If
    End Sub

    Public Function FillItemDet(ByVal argItem As String, ByVal argRow As Integer) As Boolean
        Dim rsDummy As oledb.oledbDataReader
        Dim strSQL As String
        Dim blnInActive As Boolean, blnFound As Boolean
        FillItemDet = False

        If argRow <= 0 Then
            argRow = 1
        End If

        With vsfDet
            If argItem = "" Then
                Exit Function
            End If

            intItemId = clsSVT.getDataLongValue("Select " & strFldItemID & " from " & strItemTblName & " where " & strFldItemCode & " = '" & argItem & "'", 0, blnInActive)
            If intItemId = 0 Then
                MsgBox(strItemShForm & " code '" & argItem & "' does not exist!", vbInformation, Me.Text)
                Exit Function
            End If

            Call clsSVT.getDataLongValue("Select " & strFldItemID & " from " & strItemTblName & " where " & strFldItemCode & " = '" & argItem & "' and InActive='Y'", 0, blnInActive)

            If blnInActive Then
                MsgBox(strItemShForm & " code '" & argItem & "' is inactive! Please select an active " & strItemShForm & " from the list.", vbInformation, Me.Text)
                Exit Function
            End If

            clsSVT.getDataLongValue("Select ItemID from MstItem where ItemCode = '" & Trim(argItem) & "'", 0, blnFound)

            If Trim(cboItemCode.Text) = argItem Then
                MsgBox("Item is already present!", MsgBoxStyle.Information, Me.Text)
                Exit Function
            End If

            For i = 1 To .Rows.Count - 1
                If .GetData(i, vDetItemCode) = argItem And i <> argRow Then
                    MsgBox("Item is already present!", MsgBoxStyle.Information, Me.Text)
                    Exit Function
                End If
            Next i

            strSQL = "Select i.*, uomcode from "
            strSQL = strSQL & " MstItem i, MstUom u"
            strSQL = strSQL & "  where i.UomID = u.UomID and "
            strSQL = strSQL & " ItemCode = '" & Trim(argItem) & "'"
            rsDummy = clsSVT.getDataReaderFromSQL(strSQL)

            With rsDummy
                If Not .HasRows Then
                    MsgBox(strItemShForm & " does not exist!!", vbInformation, Me.Text)
                    Exit Function
                End If
                .Read()

                vsfDet.SetData(argRow, vDetSlNo, argRow)
                vsfDet.SetData(argRow, vDetItemID, rsDummy(strFldItemID))
                vsfDet.SetData(argRow, vDetItemName, rsDummy(strFldItemName))
                vsfDet.SetData(argRow, vDetItemCode, rsDummy(strFldItemCode))
                vsfDet.SetData(argRow, vDetUOM, rsDummy("UOMCode"))
                If vsfDet.GetData(argRow, vDetQty) <= 0 Then
                    vsfDet.SetData(argRow, vDetQty, 1)
                End If

                Call clsSVT.getDataStringValue(" Select BOMID from MstHdrBOM  where BOMItemID = " & intItemId, 0, blnFound)
                If blnFound Then
                    vsfDet.SetCellCheck(argRow, vDetBOM, C1.Win.C1FlexGrid.CheckEnum.Checked)
                Else
                    vsfDet.SetCellCheck(argRow, vDetBOM, C1.Win.C1FlexGrid.CheckEnum.Unchecked)
                End If
                .Close()
            End With
        End With
        Call fillItemRelatedDataFromDocDsgnr(intDocID, Val(vsfDet.GetData(argRow, vDetItemID)), Me)
        Call vsfOptValidateEdit(Me, vsfDet, vsfOptions, vsfDet.Row, vDetItemCode, "T", strDocDtlTblName, "", "")
        If blnCalc = False Then
            Call CalTotal()
        End If
        FillItemDet = True
    End Function

    Private Sub vsfDet_ChangeEdit(sender As Object, e As System.EventArgs) Handles vsfDet.ChangeEdit
        If blnItemScan = True Then Exit Sub
        If vsfDet.Col = vDetItemCode Then
            Select Case vsfDet.Editor.Text
                Case ""
                    vsfView.Visible = False
                Case Else
                    vsfView.Top = vsfDet.Top + vsfDet.Rows(vsfDet.Row).Top + (vsfDet.Rows(vsfDet.Row).Height) + 145
                    ' vsfView.Location = Cursor.Position
                    vsfView.Left = vsfDet.Left + vsfDet.Cols(vsfDet.Col).Left + 10
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
                    Try
                        ' .StartEditing()
                    Catch ex As Exception

                    End Try
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
        '''Changed by sandhya
        'If vsfDet.Rows.Count = 1 AndAlso Trim(GetVal(vsfDet.GetData(1, vDetItemCode), "")) = "" Then
        '    vsfDet.Col = vDetItemCode
        'End If
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
            If (e.KeyCode = Keys.Escape) And .Row = .Rows.Count - 1 And (.Col = vDetItemCode Or .Col = vDetSlNo) Then
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
            If .Col = vDetItemCode And e.KeyCode = Keys.Enter Then
                Call loadItemToDtl()
                Exit Sub
            End If
            If .Col = intLastCol And e.KeyCode = Keys.Tab And .Row = .Rows.Count - 1 Then
                If Trim(.GetData(.Row, vDetItemCode)) <> "" Then
                    .LeftCol = vDetSlNo
                    .Col = vDetSlNo
                    .Col = vDetItemCode
                    .Rows.Count = .Rows.Count + 1
                    .Row = .Rows.Count - 1
                    .TopRow = .Rows.Count - 1
                    .SetData(.Row, vDetSlNo, .Rows.Count - 1)
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
            Exit Sub

        End With
        If intDetCols < intDetCols Then Exit Sub
    End Sub

    Private Sub vsfDet_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles vsfDet.LostFocus
        If (Not (Mode = "A" Or Mode = "M")) Then Exit Sub
        With vsfDet
            If .Rows.Count = 1 Then
                .Rows.Count = 2
                ReArrangeSlNo(vsfDet, vDetSlNo)
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
            If e.Col = intColQty Then
                intDocQty = Val(.Editor.Text)
                If GetVal(.GetData(.Row, vDetItemCode), "") <> "" Then
                    If Mode = "M" Then
                        intDocQty = Val(.Editor.Text)

                    End If
                End If
            End If
        End With
        If e.Col = intColrate Then
            Dim sngRate As Double = 0
            Dim intGRNID As Long = clsSVT.getDataLongValue("Select Top 1 h.GrnID from TrnHdrGRN h, TrndtlGrn d where h.GrnID = d.GrnID and d.Itemid = " & Val(vsfDet.GetData(e.Row, vDetItemID)) & "  order by GrnDate desc, h.GrnID desc", 0, False)
            If intGRNID > 0 Then
                sngRate = clsSVT.getDataDoubleValue("Select INRRate from TrndtlGrn Where GrnID =  " & intGRNID & " and itemid = " & Val(vsfDet.GetData(e.Row, vDetItemID)), 0, False)
            End If
            If Val(vsfDet.Editor.Text) < sngRate Then
                MsgBox("Rate entered " & Val(vsfDet.Editor.Text) & " is Less than GRN Rate " & Format(sngRate, "0.000000") & " in GRNNO " & clsSVT.getDataStringValue("select grnno from trnhdrgrn where grnid = " & intGRNID, "", False), vbCritical, Me.Text)
            End If
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
        ' Dim i As Integer
        Call ReArrangeSlNo(vsfDet, vDetSlNo)

        If gridEmpty() Then
            vsfDet.Rows.Count = 1
            vsfDet.Rows.Count = 2
            Call ReArrangeSlNo(vsfDet, vDetSlNo)
            Exit Sub
        End If

        'With vsfDet
        '    For i = 1 To .Rows.Count - 1
        '        .SetData(i, intColQty, Math.Round(Val(.GetData(i, intColQty)), intQtyDecimals))
        '    Next
        'End With
        Call AddnlFldCalTotal(Me, vsfOptions, vsfDet, intDetCols, "", "")
        '   dblTotAmt = vsfTot.GetData(intRowTot, vOthFldDataValue)
    End Sub

    Public Function validFlds() As Boolean
        Dim i As Integer, sngQty As Double, intItemID As Long
        Dim intDupRow As Integer
        validFlds = False
        '============
        'header validations
        '============

        Call ReArrangeSlNo(vsfDet, vDetSlNo)

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

        If Trim(cboItemCode.Text) = "" Then
            MsgBox("Item code cannot be blank!", MsgBoxStyle.Information, Me.Text)
            cboItemCode.Focus()
            Exit Function
        End If

        frmUtlExp.vsfExp.Rows.Count = 1

        For i = 1 To vsfDet.Rows.Count - 1
            sngQty = Val(GetVal(vsfDet.GetData(i, vDetQty)))

            If vsfDet.GetData(i, vDetItemCode) = cboItemCode.Text Then
                MsgBox(" Item Already Used in.." & vsfDet.GetData(i, vDetItemCode), vbInformation, Me.Text)
                vsfDet.TopRow = i
                vsfDet.Col = vDetItemCode
                vsfDet.Row = i
                vsfDet.Focus()
                Exit Function
            End If

            strSQL = "ItemId - " & vsfDet.GetData(i, vDetItemID) & " ItemCode " & vsfDet.GetData(i, vDetItemCode)
            ''Commented earlier
            If Not explodeActMBOM(i, vsfDet.GetData(i, vDetItemID), sngQty, frmUtlExp.vsfExp) Then
                Exit Function
            End If
        Next i

        For i = 1 To frmUtlExp.vsfExp.Rows.Count - 1
            sngQty = Val(GetVal(frmUtlExp.vsfExp.GetData(i, vDetQty)))
            strSQL = "Grid Item : " & frmUtlExp.vsfExp.GetData(i, arrItemCode) & "    Selc Item : " & cboItemCode.Text
            'Commented earlier
            If Not explodeActMBOM(i, frmUtlExp.vsfExp.GetData(i, arrItemID), sngQty, frmUtlExp.vsfExp) Then
                Exit Function
            End If
        Next i

        strSQL = "select ItemId from MstItem where ItemCode = '" & cboItemCode.Text & "'"
        intItemID = clsSVT.getDataLongValue(strSQL, 0, False)

        If strEffDateReq = "Y" Then
            If dtEffFrom.Value > dtEffTo.Value Then
                MsgBox("Effective dates not valid", vbInformation, Me.Text)
                dtEffFrom.Focus()
                Exit Function
            End If
        End If

        With vsfDet
            For intGridRow As Int32 = 0 To .Rows.Count - 1
                For intGridCol As Int32 = 0 To .Cols.Count - 1
                    .SetData(intGridRow, intGridCol, CStr(GetVal(.GetData(intGridRow, intGridCol), "")).Replace("'", "`"))
                Next
            Next

            Call RemoveEmptVSFRows(vsfDet, 1, vDetItemCode)

            If vsfDet.Rows.Count = 1 Then
                MsgBox("Please select an item!", vbInformation, Me.Text)
                vsfDet.Rows.Count = vsfDet.Rows.Count + 1
                vsfDet.Row = 1
                vsfDet.Col = vDetItemCode
                vsfDet.Focus()
                Exit Function
            End If

            If clsSVT.ChkDuplicateData(vsfDet, intDupRow, vDetItemCode) Then
                MsgBox("Duplicate Item " & .GetData(intDupRow, vDetItemCode) & "!!", vbInformation, Me.Text)
                .Row = intDupRow
                .TopRow = intDupRow
                .Col = vDetItemCode
                .Focus()
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

                intItemID = clsSVT.getDataLongValue("Select " & strFldItemID & " from " & strItemTblName & " where " & strFldItemCode & "= '" & .GetData(i, vDetItemCode) & "'", 0, False)
                If intItemID = 0 Then
                    MsgBox(strFldItemCode & " does not exist!!", MsgBoxStyle.Critical, Me.Text)
                    .Row = i
                    .TopRow = i
                    .Col = vDetItemCode
                    .Focus()
                    Exit Function
                End If


                If Trim(cboItemCode.Text) = Trim(.GetData(i, vDetItemCode)) Then
                    MsgBox("BOM Item and Child item both should not be same...", vbInformation, Me.Text)
                    .Focus()
                    .Row = i
                    .Col = vDetItemCode
                    Exit Function
                End If

                If Trim(.GetData(i, vDetItemCode)) = "" Then
                    MsgBox("Item Code Not Selected", MsgBoxStyle.Critical, Me.Text)
                    .Focus()
                    .Row = i
                    .Col = vDetItemCode
                    .StartEditing()
                    Exit Function
                End If
                If intColQty > 0 AndAlso Val(.GetData(i, intColQty)) <= 0 Then
                    MsgBox("Please enter the qty greater then 0", MsgBoxStyle.Critical, Me.Text)
                    .Focus()
                    .Row = i
                    .Col = intColQty
                    .StartEditing()
                    Exit Function
                End If
                'Commented
                If chkItemPresentInDownliner(intItemID, Val(vsfDet.GetData(i, vDetItemID))) Then
                    MsgBox("Item " & .GetData(i, vDetItemCode) & " is already a Parent Item!!", MsgBoxStyle.Information, Me.Text)
                    Exit Function
                End If


            Next i
        End With


        If Not validOthFLds(Me, strDocHdrTblName, strDocNoFldName, txtDocNo.Text, Trim(strSHForm)) Then
            Exit Function
        End If

        If Not validOthDetFLds(Me, vsfDet, vsfOptions, intDetCols) Then Exit Function


        svtBtnCtrl.Focus()

        validFlds = True
    End Function

    Public Function FillDocDetails(ByVal argDocID As Long) As Boolean
        Dim rs As oledb.oledbDataReader
        FillDocDetails = False
        If Trim(argDocID) = "" Or Val(argDocID) = 0 Then Exit Function
        txtDocID.Text = argDocID
        StMsg("Filling Header Fields...")
        Call fillDocHdrFlds(argDocID)
        StMsg("Filling Detail Fields...")
        Call fillDtlFields(argDocID)

        strSQL = "Select * from MsthdrBOM where BomItemID = " & intBOMItemID & " order by EffTo desc"
        rs = clsSVT.getDataReaderFromSQL(strSQL)

        With rs
            vsfEff.Rows.Count = 1
            If Not .HasRows Then
            End If
            Do While .Read
                vsfEff.Rows.Count = vsfEff.Rows.Count + 1
                vsfEff.SetData(vsfEff.Rows.Count - 1, vEffID, !BomID)
                vsfEff.SetData(vsfEff.Rows.Count - 1, vEffFrom, !EffFrom)
                vsfEff.SetData(vsfEff.Rows.Count - 1, vEffTo, !EffTo)
            Loop
            .Close()
        End With
        pnlSearch.Visible = True
        'If blnShowFileAttach Then
        '    With svtFileAttach
        '        .ViewMode = True
        '        .ClearFiles()
        '        .LoadFilesFromData("Select AFilename from UtlAttachFiles Where DocID = " & intDocID & " and DocValueID = '" & argDocID & "'")
        '    End With
        'End If

        'FillDocDetails = True
        'StMsg("")
        'With svtFileAttach
        '    .ClearFiles()
        '    .LoadFilesFromData("Select AFilename from UtlAttachFiles Where DocID = " & intDocID & " and DocValueID = '" & argDocID & "'")
        'End With
        Call ShowTreeView(argDocID)
        txtSearch.Focus()
    End Function

    Private Function fillDocHdrFlds(ByVal argDocID As Long) As Boolean
        Dim rsDocHdr As oledb.oledbDataReader
        Dim intBOMID As Integer
        Call DateFreezing(Me, "")

        If argDocID > 0 Then
            strSQL = "select ItemCode from MstItem i, MstHdrBOM h where h.BOMItemId = i.ItemId and BOMID = " & argDocID
            cboItemCode.Text = clsSVT.getDataStringValue(strSQL, "", False)
        End If

        strSQL = "Select h.*, ItemCode, ItemName, UOMCode, LatestBOM "
        strSQL &= " from MstHdrBOM h, MstItem i, MstUOM u "
        strSQL &= " where i.ItemID = h.BOMItemID "
        strSQL &= " and i.UOMID = u.UOMID "
        strSQL &= " and ItemCode = '" & Trim(cboItemCode.Text) & "'"
        If argDocID > 0 Then strSQL &= " and BOMId = " & argDocID
        strSQL &= " order by BOMID desc"
        rsDocHdr = clsSVT.getDataReaderFromSQL(strSQL)

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

            If blnAuthReq Then
                If GetVal(!Authorised, "N") = "Y" Then
                    lblApproved.Text = "APPROVED"
                    lblApproved.ForeColor = Color.Green
                Else
                    lblApproved.Text = "UN-APPROVED"
                    lblApproved.ForeColor = Color.FromArgb(192, 0, 0)
                End If
                lblApproved.Visible = True
            Else
                lblApproved.Visible = False
            End If


            intBOMID = !Bomid
            intBOMItemID = !BOMItemID
            txtBOMID.Text = intBOMID
            cboItemCode.Text = !ItemCode
            lblItemName.Text = GetVal(!ItemName, "")
            lblUOM.Text = "To Make 1 " & GetVal(!UOMCode, " ") & " Of  { " & Trim(cboItemCode.Text) & " }"

            If strEffDateReq = "Y" Then
                If Not BlnRevised Then dtEffFrom.Value = CDate(GetVal(!EffFrom, getSystemDate))
                'dtEffTo.Value = GetVal(!EffTo, "31 DEC 9998")
            End If
            If strEffDateReq = "Y" Then
                If GetVal(!LatestBOM, "N") = "Y" Then
                    lblLatest.Visible = True
                Else
                    lblLatest.Visible = False
                End If
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
                                    argVsf.SetData(i, vOthFldDataValue,
                                        clsSVT.getDataStringValue("Select " & argVsf.GetData(i, vOthFldCmbDisplayCols) & " from " &
                                        argVsf.GetData(i, vOthFldCmbTblName) & " where " &
                                        argVsf.GetData(i, vOthFldCmbStorageFldName) & " = " & GetVal(.GetValue(X), 0), "", False))
                                End If
                            End If
                        End If
                    Next X
                Next
            Next

            fillDocHdrFlds = True
        End With

    End Function

    Public Function fillDtlFields(ByVal argDocID As Integer, Optional ByVal blnAppend As Boolean = False) As Boolean
        Dim i As Integer, blnFound As Boolean
        Dim strsql As String
        '--6 start CopyFROM
        Dim introw As Integer

        fillDtlFields = False
        Dim rsDtl As oledb.oledbDataReader

        strsql = "Select d.*, itemCode, i.ItemName, UOMCode"
        strsql = strsql & " from MstDtlBOM d, mstUOM u, mstItem i "
        strsql = strsql & " where d.itemid = i.itemid"
        strsql = strsql & " and i.UomID = u.UomID"
        strsql = strsql & " and d.BomId =" & argDocID
        strsql = strsql & " order by slNo"
        rsDtl = clsSVT.getDataReaderFromSQL(strsql)

        With rsDtl
            If Not .HasRows Then
                vsfDet.Rows.Count = 2
                fillDtlFields = False
                .Close()
                Exit Function
            End If

            vsfDet.Rows.Count = 1
            Do While .Read
                vsfDet.Rows.Count = vsfDet.Rows.Count + 1
                introw = vsfDet.Rows.Count - 1
                i = introw
                vsfDet.SetData(i, vDetSlNo, i)
                vsfDet.SetData(i, vDetItemID, rsDtl("ItemID"))
                vsfDet.SetData(i, vDetItemCode, rsDtl("ItemCode"))
                vsfDet.SetData(i, vDetItemName, rsDtl("ItemName"))
                vsfDet.SetData(i, vDetUOM, GetVal(!UomCode, ""))
                Call clsSVT.getDataStringValue(" Select BOMID from MstHdrBOM  where BOMItemID = " & !ItemID, "", blnFound)

                If blnFound Then
                    vsfDet.SetCellCheck(i, vDetBOM, C1.Win.C1FlexGrid.CheckEnum.Checked)
                End If

                vsfDet.SetData(i, vDetQty, Format(GetVal(!Qty, 0), "0.0000"))
                Call FillvsfOptDtlFlds(Me, vsfDet, vsfOptions, rsDtl, i, intDetCols)
            Loop
            .Close()

        End With

        Call RemoveEmptVSFRows(vsfDet, 2, vDetItemCode)
        Call ReArrangeSlNo(vsfDet, vDetSlNo)
        Call CalTotal()

        fillDtlFields = True
    End Function

    Private Sub bindPartyCombo()
        'Dim strSQL As String
        'strSQL = "Select " & strFldPartyID & ", " & strFldPartyCode & ", " & strFldPartyName & ""
        'strSQL = strSQL & " from " & strPartyTblName
        'strSQL = strSQL & " where " & strFldPartyID & " > 0"
        'strSQL = strSQL & " and INActive = 'N'"
        'strSQL = strSQL & " order by " & strFldPartyCode

        'Dim frmDisplay As New frmUtlSingleSel

        'With frmDisplay
        '    .MdiParent = frmMain
        '    .Text = "Select from... "
        '    .Show()
        'End With
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

    Private Sub deleteDoc()
        Dim frmDelCancel As New frmUtlDelCancel
        Dim strDelCan As String = ""

        If txtDocID.Text = 0 Then Exit Sub
        With frmDelCancel
            .argCaption = strFrmCaption
            .argDocID = intDocID
            .argDocIDFldName = strDocIDFldName
            .argDocNoFldValue = txtDocID.Text
            .argHdrTblName = strDocHdrTblName
            .argMenu = strFrmCaption
            .argBlnLog = True

            .argTblList = strDocHdrTblName & "," & strDocDtlTblName
            .ShowDialog()

            If .strDelCancelStatus = "DELETED" Then
                Call clearFlds()
            Else
                FillDocDetails(Val(txtDocID.Text))
            End If
            txtSearch.Focus()
        End With
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
        ' Call fillPartyAddress()
    End Sub

    Public Function gridEmpty() As Boolean
        gridEmpty = False
        '''Changed by sandhya
        If vsfDet.Rows.Count = 2 AndAlso Trim(GetVal(vsfDet.GetData(1, vDetItemCode), "")) = "" Then
            gridEmpty = True
        End If
    End Function

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
                    'Fill Header Table
                    With vsfHdr
                        For j = 0 To .Rows.Count - 1
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
                        Next j
                    End With

                    'Fill Total Table
                    With vsfTot
                        For j = 0 To .Rows.Count - 1
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
                        Next j
                    End With

                    'Fill Footer Table
                    With vsfFtr
                        For j = 0 To .Rows.Count - 1
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
                        Next j
                    End With
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
                    If UCase(Trim(dt.Columns(j).ToString())) = strFldItemID Then
                        intItemID = Convert.ToInt32(rv.ItemArray(i).ToString())
                        strItemCode = clsSVT.getDataStringValue("Select " & strFldItemCode & " from " & strItemTblName & " where " & strFldItemID & " = " & intItemID, 0, False)
                        FillItemDet(strItemCode, vsfDet.Rows.Count - 1)
                    ElseIf UCase(Trim(dt.Columns(j).ToString())) = UCase(Trim(argDocSHForm & "QTY")) Then
                        vsfDet.SetData(l, intColQty, GetVal(rv.ItemArray(i).ToString(), ""))
                    Else
                        For k = 0 To vsfDet.Cols.Count - 1
                            If UCase(Trim(vsfDet.GetData(0, k))).Replace(" ", "") = UCase(Trim(dt.Columns(j).ToString())).Replace(" ", "") And vsfDet.Cols(k).Visible = True Then
                                vsfDet.SetData(l, k, GetVal(rv.ItemArray(i).ToString(), ""))
                                Exit For
                            End If
                        Next k
                    End If
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

    Private Sub setSearchGridHt(ByVal argFlag As String)
        If argFlag = "FULL" Then
            vsfSearch.Visible = True
            vsfSearch.BringToFront()
            vsfSearch.Width = vsfHdr.Width
            pnlSearch.BringToFront()
        Else
            vsfSearch.Visible = False
        End If
    End Sub

    Private Sub frmMstBOM_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Mode = "V"
        Dim intBOMId As Long
        Dim ToolTip As New ToolTip
        Dim blnNewMode As Boolean = False, blnLoadLastTrn As Boolean = False

        Call initFormVsfDet(Me, strDocHdrTblName, vsfDet, "DET")
        Call initFormVsfDet(Me, strDocHdrTblName, vsfSearch, "SEARCH")
        Call initFormVsfDet(Me, strDocHdrTblName, vsfHdr, "HDR")
        Call initFormVsfDet(Me, strDocHdrTblName, vsfDet, "DET")
        Call fillFormProperties(Me)
        Call initGridStyles(Me)
        Call FillBOMItemInCombo()
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
        If Not blnChildForm Then
            strSQL = "select max(BOMID) from MstHdrBOM"
            intBOMId = clsSVT.getDataLongValue(strSQL, 0, False)
            If intBOMId > 0 Then Call FillDocDetails(intBOMId)
        End If
        lblSearch.ForeColor = Color.White
        'lblfrm.Text = "View - " & Me.Text
        lblfrm.Text = Me.Text
        lblfrm.ForeColor = Color.White
        lblSearch.ForeColor = Color.White
        txtSearch.Focus()
        Call ChangeFontSize(Me)
        lblfrm.Font = New Font("Microsoft Sans Serif", 14, FontStyle.Bold)
        'cboItemCode.Text = String.Empty
        lblSearch.Font = New Font("Segoe UI", 9, FontStyle.Bold)
        Call ResizePanels()
        Call ucBtnEnabledisable()
    End Sub

    Private Sub vsfTot_AfterEdit(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfTot.AfterEdit
        '  If vsfTot.GetData(e.Row, vOthPerAmount) = "Y" And e.Col = vOthFldDataPer Then
        Call vsfGenAfterEditBfrCalTot(Me, vsfTot, e.Row, "M")
        If blnCalc = False Then
            Call CalTotal()
        End If
        Call vsfGenAfterEditAftrCalTot(Me, vsfTot, intDocID, Mode, e.Row, False)
        '  End If
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
        If Not (Mode = "A" Or Mode = "M") Then Exit Sub
        lblToolTip.Text = ""
        lblToolTip.Text = vsfRowColChange(Me, vsfTot)
        If lblToolTip.Text <> "" Then
            pnlToolTip.Visible = True
        Else
            pnlToolTip.Visible = False
        End If
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
    End Sub

    Private Sub cboPfx_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles cboPfx.Validating
        If strPfxTYpe = "" Then
            Call cboPfxValidate(Me, blnallowToChangePfx, intDocID, Mode, False)
        End If
    End Sub

    Private Sub txtNo_Validated(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtNo.Validated
        Dim strDocNo As String, blnFound As Boolean
        If Val(txtNo.Text) <= 0 Then
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
            End If
        End If

        Call getPfxNo(txtDocNo.Text, strPfx, strNo)
        txtNo.Focus()
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
                Case "REVISE"

                    If dtEffTo.Value <> "31/12/9998" Then
                        MsgBox("Only Latest BOM will be allowed to delete...", vbInformation, Me.Text)
                        Exit Sub
                    End If

                    If MsgBox("Are you sure to Revise BOM", vbQuestion + vbYesNo + MsgBoxStyle.DefaultButton2, Me.Text) = vbNo Then
                        Exit Sub
                    End If

                    If Not chkUserRights(strRights, chkRightsPos.opt2Rights, Me.Text) Then Exit Sub

                    svtBtnCtrl_btnClicked(svtBtns.svtEdit)

                    If dtEffFrom.Value < getSystemDate() Then
                        dtEffFrom.Value = getSystemDate()
                    End If

                    BlnRevised = True
                    Mode = "A"
                    intOLDBOMID = Val(txtBOMID.Text)
                    dtEffFrom.MinDate = DateAdd("d", 1, CDate(vsfEff.GetData(1, vEffFrom)))
                    dtEffFrom.Enabled = True
                    dtEffTo.Enabled = False
                    'Call FillAutoGenTrnNo()
                Case "SHOW TREE VIEW"
                    If cboItemCode.Text <> "" Then
                        With frmUtlBOMTreeview
                            .Text = "TreeView for [ " & cboItemCode.Text & " ]"
                            .StartPosition = FormStartPosition.CenterScreen
                            .ShowTreeView(Val(txtBOMID.Text))
                            .Show()
                        End With
                    End If

                Case "APPROVE"
                    If Trim(cboItemCode.Text) = "" Then Exit Sub
                    If Not chkUserRights(strRights, chkRightsPos.AuthRights, Me.Text) Then Exit Sub
                    If UCase(lblApproved.Text) <> "APPROVED" Then
                        If MsgBox("Do you want to Approve this BOM Item '" & cboItemCode.Text & "' ?", vbQuestion + vbYesNo + vbDefaultButton1, Me.Text) = vbYes Then
                            clsSVT.execSQLStatement("Update MstHdrBOM set Authorised='Y', AuthBy='" & gUserCode & "', AuthOn=" & getDateFormatInCond(getSystemDate) & " where BOMID =" & Val(txtDocID.Text))
                            'for audit track
                            Call RecordAuditLog(strFrmCaption, Val(txtDocID.Text), UserActions.EditAction, "MstHdrBOM", strAuditHdrSQL, strAuditDtlSQL)
                            lblApproved.Text = "APPROVED"
                            lblApproved.ForeColor = Color.Green
                            Exit Sub
                        End If
                    ElseIf UCase(lblApproved.Text) <> "UN-APPROVED" Then
                        If MsgBox("BOM Item : '" & cboItemCode.Text & "' is already approved." & vbCrLf & "Do you want to revoke it?", vbQuestion + vbYesNo + vbDefaultButton1, Me.Text) = vbYes Then
                            clsSVT.execSQLStatement("Update MstHdrBOM set Authorised='N', AuthBy='', AuthOn=NULL where BOMID =" & Val(txtDocID.Text))
                            'for audit track
                            Call RecordAuditLog(strFrmCaption, Val(txtDocID.Text), UserActions.EditAction, "MstHdrBOM", strAuditHdrSQL, strAuditDtlSQL)
                            lblApproved.Text = "UN-APPROVED"
                            lblApproved.ForeColor = Color.FromArgb(192, 0, 0)
                            Exit Sub
                        End If
                    End If
                Case Else
                    'If Not chkUserRights(strRights, chkRightsPos.PrintRights, Me.Text) Then Exit Sub
                    'Dim frmv As New frmUtlReportViewer
                    'With frmv
                    '    '.MdiParent = frmMain
                    '    .StartPosition = FormStartPosition.CenterScreen
                    '    .ShowRptFileFromUtlOptions(intDocID, Val(txtDocID.Text), argOptionName, True)
                    '    .Show()
                    'End With
            End Select
        Else
            Exit Sub
        End If
    End Sub

    Public Function SaveDoc() As Boolean
        SaveDoc = False
        ' for adding / editing Record
        showWaitCursor()

        svtTrn = svtConn.BeginTransaction(IsolationLevel.ReadCommitted)
        Try

            If Not UpdtDocHdrDtl() Then
                SaveDoc = False
                svtTrn.Rollback()
                Exit Function
            End If

            'for audit track
            If blnAuditTrack Then
                Call RecordAuditLog(strFrmCaption, Val(txtDocID.Text), IIf(Mode = "A", UserActions.AddAction, UserActions.EditAction), "MstHdrBOM", strAuditHdrSQL, strAuditDtlSQL, svtTrn)
            End If

            svtTrn.Commit()

            SaveDoc = True
            If Mode = "M" Then
                MsgBox("Updated successfully!", vbInformation, Me.Text)
            Else
                MsgBox("Data saved successfully!", vbInformation, Me.Text)
            End If

            'If strAuthReq = "Y" Then
            '    Call saveInAuthTable(intDocID, Val(txtDocID.Text), blnAuthCalcluate, intNoOfAuth, strAuthQuery, strDocHdrTblName, strDocIDFldName, svtTrn)
            'End If

        Catch ex As Exception
            svtTrn.Rollback()
            SaveDoc = False
            MsgBox(ex.Message, MsgBoxStyle.Information, Me.Text)
        Finally
            Cursor.Current = Cursors.Arrow
        End Try
    End Function

    Private Function UpdtDocHdrDtl() As Boolean
        Dim intBOMID As Integer, dtEffTODate As Date
        UpdtDocHdrDtl = False

        If Mode = "A" Then

            ''''''''''''''Changed by sandhya
            If intBomPId > 0 Then
                If Validationdate() Then
                    'strSQL = "Update MstHdrBOM Set LatestBom='N' where BomitemId=" & intBomPId
                    'If Not clsSVT.execSQLStatement(strSQL, svtTrn) Then
                    '    Exit Function
                    'End If
                    If Trim(txtNo.Text) > 0 Then
                        dtEffTODate = DateAdd(DateInterval.Day, -1, dtEffFrom.Value)
                        strSQL = "Update MstHdrBOM Set EffTo = '" & getDateFormatInInsert(dtEffTODate) & "', LatestBOM = 'N' where BOMID = " & intBomPId
                        If Not clsSVT.execSQLStatement(strSQL, svtTrn) Then
                            Exit Function
                        End If
                    End If
                Else
                    Exit Function
                End If
            End If
            ''''Changed by sandhya

            If BlnRevised = True Then
                dtEffTODate = DateAdd(DateInterval.Day, -1, dtEffFrom.Value)
                strSQL = "Update MstHdrBom set EffTo = '" & getDateFormatInInsert(dtEffTODate) & "', LatestBOM = 'N' where BOMID = " & intOLDBOMID
                If Not clsSVT.execSQLStatement(strSQL, svtTrn) Then
                    Exit Function
                End If
            End If

            '''''''''''Changed by Sandhya

            '    If intBomPId > 0 Then
            '        If dtEffFrom.Enabled Then
            '            Dim rs As oledb.oledbDataReader
            '            Dim blnBomidres As Boolean = False

            '            rs = clsSVT.getDataReaderFromSQL("Select * from MstHdrBOM where LatestBom='Y' and  BomId=" & intBomPId, svtTrn)
            '            If Not rs.HasRows Then
            '                rs.Close()
            '                Exit Function
            '            End If
            '            Do While rs.Read
            '                With vsfEff
            '                    If vsfEff.FindRow(rs!BomID, 1, vEffID, True) > 0 Then
            '                        If Not vsfEff.FindRow(rs!EffFrom, 1, vEffFrom, True) AndAlso
            '                        vsfEff.FindRow(rs!EffTo, 1, vEffTo, True) Then
            '                            MsgBox("Effective from or Effective To Date has been changed", vbInformation, Me.Text)
            '                            blnBomidres = True
            '                        End If
            '                        If blnBomidres Then
            '                            dtEffTODate = DateAdd(DateInterval.Day, -1, dtEffFrom.Value)
            '                            strSQL = "update MstHdrBom set EffTo = '" & getDateFormatInInsert(dtEffTODate) & "', LatestBOM = 'N' where BOMID = " & intBomPId
            '                            If Not clsSVT.execSQLStatement(strSQL, svtTrn) Then
            '                                Exit Function
            '                            End If
            '                        End If
            '                    End If
            '                    '     If .GetData(vsfEff.Row, vEffID) = GetVal(rs!BomID) Then
            '                    '     If Not .GetData(vsfEff.Row, vEffFrom) = rs!EffFrom AndAlso
            '                    '.GetData(vsfEff.Row, vEffTo) = rs!EffTo Then
            '                    '         MsgBox("Effective from or Effective To Date has been changed", vbInformation, Me.Text)
            '                    '     Else
            '                    '         dtEffTODate = DateAdd(DateInterval.Day, -1, dtEffFrom.Value)
            '                    '         strSQL = "update MstHdrBom set EffTo = '" & getDateFormatInInsert(dtEffTODate) & "', LatestBOM = 'N' where BOMID = " & intBomPId
            '                    '         If Not clsSVT.execSQLStatement(strSQL, svtTrn) Then
            '                    '             Exit Function
            '                    '         End If
            '                    '     End If
            '                    'End If
            '                End With
            '            Loop
            '        End If
            '    End If
        End If
        '''''''''''''''''Changed by sandhya'''



        If Mode = "M" Then
            intBOMID = Val(txtBOMID.Text)
            If Not clsSVT.execSQLStatement("Delete from " & strDocDtlTblName & " where " & strDocIDFldName & " =" & intBOMID, svtTrn) Then
                Exit Function
            End If
        End If

        If Mode = "A" Then
            ' If intBomPId = 0 Then
            strSQL = "Insert into " & strDocHdrTblName
                strSQL = strSQL & " (" & strDocNoFldName & ", Prefix, Print" & strDocNoFldName & ", "
                strSQL = strSQL & strDocDateFldName & ", " & strSHForm & "ItemID,"
                strSQL = strSQL & " AddedBy, AddedOn,"
                strSQL = strSQL & " Cancelled,"
                strSQL = strSQL & " EffFrom, EffTo, LatestBOM"
                If Not blnAuthReq Then
                    strSQL = strSQL & ", Authorised,AuthBy,AuthOn"
                End If

                Call GetInsertString(Me, strValues, strFlds, svtTrn)

                If gDBTypeCurrent = "ACCESS" Then
                    strSQL = strSQL & strFlds & ") values ("
                Else
                    strSQL = strSQL & strFlds & ") output inserted." & strDocIDFldName & " into @testTable values ("
                End If

                strSQL = strSQL & "'" & txtDocNo.Text & "', "
                strSQL = strSQL & "'" & cboPfx.Text & "', "
                strSQL = strSQL & "'" & getPrintDocFmtNo(intDocID, txtDocNo.Text,
                                    cboPfx.Text, txtNo.Text, svtTrn) & "', "
                strSQL = strSQL & "'" & getDateFormatInInsert(dtDocDate.Value) & "', "
                strSQL = strSQL & clsSVT.getDataLongValue("select ItemId from MstItem where ItemCode = '" & Trim(cboItemCode.Text) & "'", 0, False, svtTrn) & ","
                strSQL = strSQL & "'" & gUserCode & "', "
                strSQL = strSQL & "'" & getDateFormatInInsert(getSystemDateTime(svtTrn), True) & "', "
                strSQL = strSQL & " 'N',"
                strSQL &= "'" & getDateFormatInInsert(dtEffFrom.Value) & "',"
                strSQL &= "'" & getDateFormatInInsert(dtEffTo.Value) & "',"
                strSQL = strSQL & "'Y'"

                If Not blnAuthReq Then
                    strSQL = strSQL & ", 'Y','" & gUserCode & "'," & getDateFormatInCond(getSystemDate(svtTrn)) & ""
                End If

                strSQL = strSQL & strValues

                If gDBTypeCurrent = "ACCESS" Then
                    clsSVT.execSQLStatement(strSQL, svtTrn)
                    strDtlSQL = GetDetSqlString()
                    clsSVT.execSQLStatement(strDtlSQL, svtTrn)
                    intMaxDocID = clsSVT.getDataLongValue("Select max(" & strDocIDFldName & ") from " & strDocHdrTblName, 0, False, svtTrn)
                Else
                    strDtlSQL = GetDetSqlString()
                    intMaxDocID = getMaxHdrDtlInsertID(strSQL, strDtlSQL, strDocHdrTblName, svtTrn)
                End If
                txtDocID.Text = intMaxDocID
                If intMaxDocID = -1 Then
                    UpdtDocHdrDtl = False
                    Exit Function
                End If
            End If
        'End If

        If Mode = "M" Then
            strSQL = "select ItemId from MStItem where ItemCode = '" & cboItemCode.Text & "'"
            intItemId = clsSVT.getDataLongValue(strSQL, 0, False, svtTrn)

            strSQL = "update " & strDocHdrTblName & " set "
            strSQL = strSQL & " BOMItemID = " & intItemId & ","
            strSQL = strSQL & "Authorised = 'N', "
            strSQL = strSQL & "changedBy = '" & gUserCode & "', "
            strSQL = strSQL & "ChangedOn = '" & getDateFormatInInsert(getSystemDateTime(svtTrn), True) & "', "

            strSQL &= " EffFrom = '" & getDateFormatInInsert(dtEffFrom.Value) & "',"
            strSQL &= " EffTo = '" & getDateFormatInInsert(dtEffTo.Value) & "',"

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
        strFlds = strFlds & " (" & strDocIDFldName & ", slNo, ItemID, Qty,"

        For i = 1 To vsfDet.Rows.Count - 1
            If argDocID > 0 AndAlso Mode = "M" Then
                strValues = strValues & "(" & argDocID & ", "
            Else
                strValues = strValues & "(@MaxID, "
            End If

            With vsfDet
                strValues = strValues & Val(.GetData(i, vDetSlNo)) & ","
                strValues = strValues & Val(.GetData(i, vDetItemID)) & ","
                strValues = strValues & Val(.GetData(i, vDetQty)) & ","
            End With

            Call GetvsfOptHdrDetInsertString(Me, vsfDet, vsfOptions, intDetCols, i, strFlds1, strValues, svtTrn)
            strValues = strValues & ","
        Next


        strFlds = Mid(strFlds, 1, Len(strFlds) - 1)
        strValues = Mid(strValues, 1, Len(strValues) - 1)

        GetDetSqlString = strFlds & strColFlds & " values " & strValues
    End Function

    Private Sub vsfTot_EnterCell(sender As Object, e As System.EventArgs) Handles vsfTot.EnterCell
        If Mode = "V" Or Mode = "" Then Exit Sub

        With vsfTot
            If .GetData(.Row, vOthFldTxtBlnMultiLine) = "Y" Then Exit Sub

            If .GetData(.Row, vOthFldBlnComboData) = "Y" Then Exit Sub
            If .GetData(.Row, vOthFldDataType) = "List String" Then Exit Sub
            If .GetData(.Row, vOthFldBlnReadOnly) = "Y" Then Exit Sub
            If .GetData(.Row, vOthFldBlnRefill) = "Y" Then Exit Sub
            If .GetData(.Row, vOthPerAmount) = "Y" And .Col <> vOthFldDataPer Then Exit Sub
            Try
                .StartEditing()
            Catch ex As Exception

            End Try
        End With
    End Sub

    Public Sub vsfHdr_AfterEdit(sender As Object, e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfHdr.AfterEdit
        Call vsfGenAfterEditBfrCalTot(Me, vsfHdr, e.Row, "M")
        If blnCalc = False Then
            Call CalTotal()
        End If
        Call vsfGenAfterEditAftrCalTot(Me, vsfHdr, intDocID, Mode, e.Row, False)
    End Sub

    Public Sub vsfHdr_BeforeEdit(sender As Object, e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfHdr.BeforeEdit
        If vsfBeforeEditCancel(Me, vsfHdr, intDocID, strDocHdrTblName, Mode, e.Row, e.Col, "M", "") Then
            e.Cancel = True
            Exit Sub
        End If
    End Sub

    Public Sub vsfHdr_EnterCell(sender As Object, e As System.EventArgs) Handles vsfHdr.EnterCell
        If Mode = "V" Or Mode = "" Then Exit Sub

        With vsfHdr
            If .GetData(.Row, vOthFldTxtBlnMultiLine) = "Y" Then Exit Sub

            If .GetData(.Row, vOthFldBlnComboData) = "Y" Then Exit Sub
            If .GetData(.Row, vOthFldDataType) = "List String" Then Exit Sub
            If .GetData(.Row, vOthFldBlnReadOnly) = "Y" Then Exit Sub
            If .GetData(.Row, vOthFldBlnRefill) = "Y" Then Exit Sub

            .StartEditing()
        End With
    End Sub

    Public Sub vsfHdr_KeyPressEdit(sender As Object, e As C1.Win.C1FlexGrid.KeyPressEditEventArgs) Handles vsfHdr.KeyPressEdit
        If e.KeyChar = "'" Then
            e.Handled = True
            Exit Sub
        End If
        If vsfKeyPressEdit(Me, vsfHdr, vsfHdr.Row, e.KeyChar) Then
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
    End Sub

    Public Sub vsfHdr_LostFocus(sender As Object, e As System.EventArgs) Handles vsfHdr.LostFocus
        pnlToolTip.Visible = False
    End Sub

    Public Sub vsfHdr_RowColChange(sender As Object, e As System.EventArgs) Handles vsfHdr.RowColChange
        If Not (Mode = "A" Or Mode = "M") Then Exit Sub
        lblToolTip.Text = ""
        lblToolTip.Text = vsfRowColChange(Me, vsfHdr)
        If lblToolTip.Text <> "" Then
            pnlToolTip.Visible = True
        Else
            pnlToolTip.Visible = False
        End If
    End Sub

    Public Sub vsfHdr_SetupEditor(sender As Object, e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfHdr.SetupEditor
        Call vsfSetupEditor(Me, vsfHdr, intDocID, strDocHdrTblName, Mode, e.Row, e.Col, "M")
    End Sub

    Public Sub vsfHdr_ValidateEdit(sender As Object, e As C1.Win.C1FlexGrid.ValidateEditEventArgs) Handles vsfHdr.ValidateEdit
        If Not vsfValidateEdit(Me, vsfHdr, e.Row, e.Col, "M", strDocHdrTblName, strDocNoFldName, txtDocNo.Text) Then
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

        With vsfFtr
            If .GetData(.Row, vOthFldTxtBlnMultiLine) = "Y" Then Exit Sub

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
        If vsfKeyPressEdit(Me, vsfFtr, vsfFtr.Row, e.KeyChar) Then
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
    End Sub

    Public Sub vsfFtr_LostFocus(sender As Object, e As System.EventArgs) Handles vsfFtr.LostFocus
        pnlToolTip.Visible = False
    End Sub

    Public Sub vsfFtr_RowColChange(sender As Object, e As System.EventArgs) Handles vsfFtr.RowColChange
        If Not (Mode = "A" Or Mode = "M") Then Exit Sub
        lblToolTip.Text = ""
        lblToolTip.Text = vsfRowColChange(Me, vsfFtr)
        If lblToolTip.Text <> "" Then
            pnlToolTip.Visible = True
        Else
            pnlToolTip.Visible = False
        End If
    End Sub

    Public Sub vsfFtr_SetupEditor(sender As Object, e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfFtr.SetupEditor
        Call vsfSetupEditor(Me, vsfFtr, intDocID, strDocHdrTblName, Mode, e.Row, e.Col, "M")
    End Sub

    Public Sub vsfFtr_ValidateEdit(sender As Object, e As C1.Win.C1FlexGrid.ValidateEditEventArgs) Handles vsfFtr.ValidateEdit
        If Not vsfValidateEdit(Me, vsfFtr, e.Row, e.Col, "M", strDocHdrTblName, strDocNoFldName, txtDocNo.Text) Then
            e.Cancel = True
        End If
    End Sub

    'Private Sub ucSearch_txtChanged(argTextChanged As String)
    '    ucSearch.LoadGrid(argTextChanged, True)
    'End Sub

    'Private Sub ucSearch_txtKeyUp(argValue As Integer)
    '    With ucSearch
    '        If .vsfPartySrc.Rows.Count > 1 And .vsfPartySrc.Row > 0 Then
    '            .txtKey(.txtParty.Text, .vsfPartySrc.Row, argValue, True)
    '            If argValue = 1 Then vsfHdr.Focus()
    '        End If
    '    End With
    'End Sub

    'Private Sub ucSearch_txtValid(argTextChanged As String)
    '    ucSearch.ValidateText(argTextChanged, True)
    'End Sub

    'Private Sub ucSearch_vsfDoubleClick(argValue As Integer)
    '    ucSearch.fillGrid(argValue, True)
    'End Sub

    'Private Sub ucSearch_vsfKeyUp(argValue As Integer)
    '    Select Case argValue
    '        Case 1
    '            ucSearch.fillGrid(ucSearch.vsfPartySrc.Row, True)
    '    End Select
    'End Sub

    'Private Sub UcTrnSearch1_txtChanged(argTextChanged As String)
    '    UcTrnSearch1.LoadGrid(argTextChanged, "Select POID, PODate, PONo, SuppCode, SuppName, Tot as Total From TrnHdrPO p, MstSupp s where p.SuppID=s.SuppID ", "PONo", "PODate", "SuppCode", "SuppName")
    'End Sub

    'Private Sub UcTrnSearch1_txtKeyUp(argValue As Integer)
    '    UcTrnSearch1.txtKey(argValue)
    '    If argValue = 1 Then
    '        Dim intRowID As Int32
    '        intRowID = UcTrnSearch1.getID(UcTrnSearch1.intRowSlno)
    '        If intRowID > 0 Then
    '            FillDocDetails(intRowID)
    '            UcTrnSearch1.vsfSearch.Visible = False
    '        End If
    '    End If
    'End Sub

    'Private Sub UcTrnSearch1_vsfDoubleClick(argValue As Integer)
    '    Dim intRowID As Int32
    '    intRowID = UcTrnSearch1.getID(UcTrnSearch1.vsfSearch.Row)
    '    If intRowID > 0 Then
    '        FillDocDetails(intRowID)
    '        UcTrnSearch1.vsfSearch.Visible = False
    '    End If
    'End Sub

    'Private Sub UcTrnSearch1_vsfKeyUp(argValue As Integer)
    '    Select Case argValue
    '        Case 1
    '            Dim intRowID As Int32
    '            intRowID = UcTrnSearch1.getID(UcTrnSearch1.vsfSearch.Row)
    '            If intRowID > 0 Then
    '                FillDocDetails(intRowID)
    '                UcTrnSearch1.vsfSearch.Visible = False
    '            End If
    '    End Select
    'End Sub

    Private Sub NextFocus()
        'If vsfHdr.Visible Then
        '    vsfHdr.Row = 0
        '    vsfHdr.Col = vOthFldDataValue
        '    If vsfHdr.GetData(0, vOthFldBlnReadOnly) = "Y" Then
        '        vsfHdr.Row = 1
        '    End If
        '    vsfHdr.StartEditing()

        'Else
        vsfDet.Row = 1
        vsfDet.Col = vDetItemCode
        vsfDet.Focus()
        'End If
    End Sub

    Private Function getSrcQry(argValue As String, ByVal argCond As Boolean) As String
        Dim strSQL As String = "Select h." & strDocIDFldName & "," & strDocNoFldName & "," & strSHForm & "Date,"
        strSQL &= strFldPartyCode & "," & strFldPartyName & " from "
        strSQL &= strDocHdrTblName & " h,"
        strSQL &= strPartyTblName & " s where "
        strSQL &= "h." & strFldPartyID & "=s." & strFldPartyID & " and 1=1 "
        If argCond = True Then
            strSQL &= " and ("
            strSQL &= strDocNoFldName & " like '%" & argValue & "%'"
            If gDBTypeCurrent = "ACCESS" Then
                strSQL &= " or Cstr(" & strSHForm & "Date)" & " like '%" & argValue & "%'"
            Else
                strSQL &= " or Convert(varchar(25)," & strSHForm & "Date,103)" & " like '%" & argValue & "%'"
            End If
            strSQL &= " or " & strFldPartyName & "  like '%" & argValue & "%'"
            strSQL &= " or " & strFldPartyCode & "  like '%" & argValue & "%')"
        End If
        strSQL &= " order by 1,2,3"
        Return strSQL
    End Function

    Private Sub btnItemCode_Click(sender As Object, e As EventArgs) Handles btnItemCode.Click

        txtDummy.Text = ""
        Dim strSQL As String = ""

        strSQL = "Select ItemID, ItemCode, ItemName "
        strSQL &= "from MstItem where InActive = 'N' and BOMItem='Y'"
        If strBOMItemCond <> "" Then
            strSQL &= strBOMItemCond
        End If
        strSQL &= "order by itemcode"

        Dim frmDisplay As New frmUtlSingleSel
        With frmDisplay
            .Text = "Select BOM Item... "
            Call .FillViewVsf(Me, strSQL, txtDummy, 0, 0)
            .ShowDialog()
            cboItemCode.Text = clsSVT.getDataStringValue("Select ItemCode From Mstitem Where ItemID=" & Val(txtDummy.Text), "", False)
            .vsfDet.Focus()
        End With

        If Val(txtDummy.Text) > 0 Then
            vsfDet.Rows.Count = 1
            If chkItem(clsSVT.getDataStringValue("Select ItemCode From Mstitem Where ItemID=" & Val(txtDummy.Text), "", False)) Then
                cboItemCode.Enabled = False
                btnLoad_Click(sender, e)
                Call FillBOMItems()
            End If
        End If
    End Sub

    'Private Sub ucBtnTools_btnClicked(btnValue As Integer) Handles ucBtnTools.btnClicked
    '    Dim intLinkedDocID As Integer = 0
    '    Dim strLinkedDocNo As String = ""
    '    Select Case btnValue
    '        Case 9, 10, 11, 12, 13, 16, 17, 18, 19, 20, 21
    '        Case Else
    '            If gridEmpty() Then Exit Sub
    '    End Select
    '    Select Case btnValue
    '        Case 8, 10, 11, 15, 20
    '        Case Else
    '            If Not (Mode = "A" Or Mode = "M") Then Exit Sub
    '    End Select
    '    Try
    '        With ucBtnTools
    '            Select Case btnValue
    '                Case svtDtlBtns.svtInsert
    '                    Call .vsfInsert(vsfDet, vsfDet.Row, vDetItemCode)
    '                Case svtDtlBtns.svtRemove
    '                    Call .vsfRemove(vsfDet, vsfDet.Row, vDetItemCode)
    '                Case svtDtlBtns.svtCopy
    '                    Call .vsfCopy(vsfDet, vsfDet.Row, vDetItemCode)
    '                Case svtDtlBtns.svtPaste
    '                    Call .vsfPaste(vsfDet, vsfDet.Row, vDetItemCode)
    '                Case svtDtlBtns.svtUp
    '                    Call MoveOneRowUp(vsfDet, vDetSlNo)
    '                Case svtDtlBtns.svtDown
    '                    Call MoveOneRowDown(vsfDet, vDetSlNo)
    '                Case svtDtlBtns.svtReplace
    '                    Call .vsfReplace(vsfDet, vsfDet.Row, vsfDet.Col, vDetItemCode)
    '                Case svtDtlBtns.svtExport
    '                    Call .vsfExport(vsfDet)
    '                Case svtDtlBtns.svtImport
    '                    If .vsfImport(vsfDet, vDetItemCode, vsfOptions, intDetCols) Then
    '                        With vsfDet
    '                            For i As Int32 = 1 To .Rows.Count - 1
    '                                If Not FillItemDet(.GetData(i, vDetItemCode), i) Then
    '                                    vsfDet.Rows.Count = 1
    '                                    vsfDet.Rows.Count = 2
    '                                    Exit Sub
    '                                End If
    '                            Next
    '                            MsgBox("XL imported into Grid...", MsgBoxStyle.Information, Me.Text)
    '                        End With
    '                    End If
    '                Case svtDtlBtns.svtTemplate
    '                    Dim strPath As String, strFileName As String
    '                    strPath = Application.StartupPath() & "\Temp\"
    '                    strFileName = Date.Now.ToString("yyyyMMddHHmmss") & "_" & Trim(txtDocNo.Text)
    '                    Call .vsfTemplate(vsfDet, strPath & strFileName, vsfOptions)
    '                Case svtDtlBtns.svtRestore
    '                    blnRestore = False

    '                    Dim obj As New frmUtlRestore
    '                    obj.strDocument = Me.Text
    '                    obj.frm = Me
    '                    obj.ShowDialog()
    '                    obj.Dispose()
    '                    obj.Close()
    '                    If blnRestore = False Then
    '                        RestoreData()
    '                    End If
    '                Case svtDtlBtns.svtCopyFrom
    '                    Dim obj As New frmUtlCopyFrom
    '                    With obj
    '                        .argForm = Me
    '                        .strFrmCaption = strFrmCaption
    '                        .argvsfOptions = vsfOptions
    '                        .argvsf = vsfDet
    '                        .ShowDialog()
    '                    End With
    '                Case svtDtlBtns.svtFreeze
    '                    Call .vsfFreeze(vsfDet, vsfDet.Col)
    '                Case svtDtlBtns.svtAddItems
    '                    Dim obj As New frmUtlAddItems
    '                    obj.frm = Me
    '                    obj.argItemCode = vDetItemCode
    '                    obj.ShowDialog()
    '                    obj.Dispose()
    '                    obj.Close()
    '                Case svtDtlBtns.svtTender
    '                    'Call .vsfTenderChange(intDocID, Val(txtDocID.Text), Trim(txtDocNo.Text), Val(vsfTot.GetData(intRowTenderFld, vOthFldDataValue)))
    '                Case svtDtlBtns.svtInsertDoc
    '                   ' Call .vsfInsertDoc(intDocID, Me)
    '                Case svtDtlBtns.svtMultiDoc
    '                    Call .vsfMultDoc(intDocID, Me, Trim(txtDocNo.Text))
    '                Case svtDtlBtns.svtTrnExport
    '                    'Dim strPartyDetails As String = ""
    '                    'With svtPartyDtl
    '                    '    strPartyDetails &= .getCode() & "ø"
    '                    '    strPartyDetails &= .getParty & "ø"
    '                    '    strPartyDetails &= .getAdd1() & "ø"
    '                    '    strPartyDetails &= .getAdd2() & "ø"
    '                    '    strPartyDetails &= .getAdd3() & "ø"
    '                    '    strPartyDetails &= .getCity() & "ø"
    '                    '    strPartyDetails &= .getPincode() & "ø"
    '                    '    strPartyDetails &= .getState() & "ø"
    '                    '    strPartyDetails &= .getCountry()
    '                    'End With
    '                    'Call .DocXlExp(Me, strSHForm, tbAdnlFlds, vsfDet, tbAdnlFldsFtr, vsfTot, intDocID, txtDocNo.Text, dtDocDate.Value, cboPfx.Text, strPartyDetails)
    '                Case svtDtlBtns.svtBOM
    '                    'Call .vsfBOM(Me, vsfDet.GetData(vsfDet.Row, vDetItemID), vsfDet.Row, vsfDet.GetData(vsfDet.Row, intColQty))
    '                Case svtDtlBtns.svtCalTotal
    '                    Call CalTotal()
    '            End Select
    '        End With
    '        ReArrangeSlNo(vsfDet, vDetSlNo)
    '    Catch ex As Exception
    '        MsgBox(ex.Message, MsgBoxStyle.Information, Me.Text)
    '    End Try
    'End Sub

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

    Private Sub btnRevision_Click(sender As Object, e As EventArgs) Handles btnRevision.Click
        If Mode = "A" Then Exit Sub
        If Trim(cboItemCode.Text) = "" Then Exit Sub
        With New frmUtlBomShowRev
            .argFrm = Me
            .lblBOM.Text = "All BOM Revisions for Item ----> " & cboItemCode.Text
            .InitScreen()
            .intBOMItemID = intBOMItemID
            .ShowRev(cboItemCode.Text)
            .Show()
        End With
    End Sub

    Private Sub Timer1_Tick(sender As System.Object, e As System.EventArgs) Handles Timer1.Tick
        If blnTimer = True And strRestoreFileName <> "" Then
            blnTimer = False
            AutoSaveGrid_Update()
        End If
    End Sub

    Private Sub AutoSaveGrid_Update()
        With vsfDet
            Try
                .WriteXml(strRestorePath & strRestoreFileName & ".xml")
            Catch ex As Exception
                MsgBox(ex.Message, MsgBoxStyle.Exclamation, Me.Text)
            End Try
        End With
        blnTimer = True
    End Sub

    Private Sub btnMulti_Click(sender As Object, e As EventArgs) Handles btnMulti.Click
        If Mode <> "M" Then Exit Sub
        With vsfDet
            If .Row <= 0 Then Exit Sub
            frmMstBOMMulti.cboItem.Text = .GetData(.Row, vDetItemCode)
            frmMstBOMMulti.cboItemGrp.Text = clsSVT.getDataStringValue("Select ItemGroupName from MstItemGroup g, MstItem i Where i.itemgroupid = g.itemgroupid and ItemCode = '" & .GetData(.Row, vDetItemCode) & "'", "", False, svtTrn)
            frmMstBOMMulti.Show()
            'frmMstBOMMulti.FillBOMItems()
        End With
    End Sub

    Private Sub RestoreData()
        Dim strFileName As String = strRestoreFileName
        RestoreData_Grid(vsfDet, strFileName)
        strRestoreFileName = "Doc_No_" & Trim(txtDocNo.Text) & "_" & Date.Now.ToString("yyyyMMddHHmmss")
        strRestoreFileName = strRestoreFileName.Replace(" ", "_")
    End Sub

    Private Sub fillCopyFromDetails(ByVal argBOMItemID As Integer)

        Dim strsQL As String, blnFound As Boolean
        Dim rs As oledb.oledbDataReader
        Dim intRow As Integer, i As Integer

        strsQL = "Select d.*, itemCode, i.ItemName, UOMCode"
        strsQL = strsQL & " from MstDtlBOM d, MstHdrBOM h, mstUOM u, mstItem i "
        strsQL = strsQL & " where d.itemid = i.itemid"
        strsQL = strsQL & " and i.UomID = u.UomID"
        strsQL = strsQL & " and d.BOMID = h.BOMID "
        strsQL = strsQL & " and d.BomId =" & argBOMItemID
        strsQL = strsQL & " order by slNo"

        rs = clsSVT.getDataReaderFromSQL(strsQL)
        'vsfDet.Rows.Count = 1
        If Not rs.HasRows Then Exit Sub

        With rs
            Do While .Read
                blnFound = False
                For i = 1 To vsfDet.Rows.Count - 1
                    If rs!ItemCode = vsfDet.GetData(i, vDetItemCode) Then
                        blnFound = True
                        intRow = i
                        Exit For
                    End If
                Next i

                If Not blnFound Then
                    vsfDet.Rows.Count = vsfDet.Rows.Count + 1
                    intRow = vsfDet.Rows.Count - 1
                End If

                vsfDet.SetData(intRow, vDetSlNo, intRow)
                vsfDet.SetData(intRow, vDetItemCode, GetVal(!Itemcode, ""))

                vsfDet.SetData(intRow, vDetItemID, GetVal(!ItemID, ""))

                Call clsSVT.getDataStringValue(" Select BOMID from MstHdrBOM  where BOMItemID = " & !ItemID, "", blnFound)

                If blnFound Then
                    vsfDet.SetCellCheck(intRow, vDetBOM, C1.Win.C1FlexGrid.CheckEnum.Checked)
                End If
                vsfDet.SetData(intRow, vDetUOM, GetVal(!UomCode, ""))
                vsfDet.SetData(intRow, vDetItemName, !ItemName)
                vsfDet.SetData(intRow, vDetQty, Format(GetVal(!Qty, 0), "0.0000"))
            Loop

            .Close()
        End With
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

    Private Sub btnInsertfromDoc_Click(sender As System.Object, e As System.EventArgs)
        With frmUtlInsertfromDoc
            .intDocID = intDocID
            .argFrm = Me
            .ShowDialog()
        End With
    End Sub

    Private Sub raiseShortCutEvent(ByVal e As System.Windows.Forms.KeyEventArgs)

        'Short cut key combination is Ctrl + Shift + required key (1 char)

        If Mode = "A" Or Mode = "M" Then
            'Insert Row event
            If e.Control = True And e.Shift = True And e.KeyCode = Keys.I Then
                Call ucBtnTools.vsfInsert(vsfDet, vsfDet.Row, vDetItemCode)
                ReArrangeSlNo(vsfDet, vDetSlNo)
            End If

            'Remove Row event
            If e.Control = True And e.Shift = True And e.KeyCode = Keys.R Then
                Call ucBtnTools.vsfRemove(vsfDet, vsfDet.Row, vDetItemCode)
                ReArrangeSlNo(vsfDet, vDetSlNo)
            End If

            ''Copy  event
            'If e.Control = True And e.Shift = True And e.KeyCode = Keys.C Then
            '    Call ucBtnTools.vsfCopy(vsfDet, vsfDet.Row, vDetItemCode)
            'End If
            ''Paste Event
            'If e.Control = True And e.Shift = True And e.KeyCode = Keys.P Then
            '    Call ucBtnTools.vsfPaste(vsfDet, vsfDet.Row, vDetItemCode)
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
                If vsfDet.Rows.Count > 1 Then
                    Call ucBtnTools.vsfExport(vsfDet)
                End If
            End If

            '    'Import from Excel
            '    If e.Control = True And e.Shift = True And e.KeyCode = Keys.F Then
            '        If ucBtnTools.vsfImport(vsfDet, vDetItemCode, vsfOptions, intDetCols) Then
            '            With vsfDet
            '                For i As Int32 = 1 To .Rows.Count - 1
            '                    If Not FillItemDet(.GetData(i, vDetItemCode), i) Then
            '                        vsfDet.Rows.Count = 1
            '                        vsfDet.Rows.Count = 2
            '                        Exit Sub
            '                    End If
            '                Next
            '                MsgBox("XL imported into Grid...", MsgBoxStyle.Information, Me.Text)
            '            End With
            '        End If
            '    End If


            '    'Template
            '    If e.Control = True And e.Shift = True And e.KeyCode = Keys.T Then
            '        Dim strPath As String, strFileName As String
            '        strPath = Application.StartupPath() & "\Temp\"
            '        strFileName = Date.Now.ToString("yyyyMMddHHmmss") & "_" & Trim(txtDocNo.Text)
            '        Call ucBtnTools.vsfTemplate(vsfDet, strPath & strFileName, vsfOptions)
            '    End If

            '    'Restore
            '    If e.Control = True And e.Shift = True And e.KeyCode = Keys.O Then
            '        blnRestore = False
            '        Dim obj As New frmUtlRestore
            '        obj.strDocument = Me.Text
            '        obj.frm = Me
            '        obj.ShowDialog()
            '        obj.Dispose()
            '        obj.Close()
            '        If blnRestore = False Then
            '            RestoreData()
            '        End If
            '    End If

            '    'Copy From
            '    If e.Control = True And e.Shift = True And e.KeyCode = Keys.X Then
            '        Dim obj As New frmUtlCopyFrom
            '        With obj
            '            .argForm = Me
            '            .strFrmCaption = strFrmCaption
            '            .argvsfOptions = vsfOptions
            '            .argvsf = vsfDet
            '            .ShowDialog()
            '        End With
            '    End If

            '    'Freeze
            '    If e.Control = True And e.Shift = True And e.KeyCode = Keys.Z Then
            '        Call ucBtnTools.vsfFreeze(vsfDet, vsfDet.Col)
            '    End If

            '    'Add List of Items
            '    If e.Control = True And e.Shift = True And e.KeyCode = Keys.Q Then
            '        Dim obj As New frmUtlAddItems
            '        obj.frm = Me
            '        obj.ShowDialog()
            '        obj.Dispose()
            '        obj.Close()
            '    End If

            '    'Tender Change
            '    If e.Control = True And e.Shift = True And e.KeyCode = Keys.Y Then
            '        'Dim obj As New frmTrnTender
            '        '' obj.frm = Me
            '        'obj.txtInvId.Text = txtDocID.Text
            '        'obj.ShowDialog()
            '        '' obj.strSHForm = strSHForm
            '        'obj.Dispose()
            '        'obj.Close()
            '        'Call ucBtnTools.vsfTenderChange(intDocID, Val(txtDocID.Text), Trim(txtDocNo.Text), Val(vsfTot.GetData(intRowTenderFld, vOthFldDataValue)))
            '    End If

            '    'Insert Doc
            '    If e.Control = True And e.Shift = True And e.KeyCode = Keys.B Then
            '        ' Call ucBtnTools.vsfInsertDoc(intDocID, Me)
            '    End If

            '    'Multi Doc Generation
            '    If e.Control = True And e.Shift = True And e.KeyCode = Keys.G Then
            '        Call ucBtnTools.vsfMultDoc(intDocID, Me, Trim(txtDocNo.Text))
            '    End If
            'End If

            ''Focus on Detail grid
            'If e.Control = True And e.Shift = True And e.KeyCode = Keys.D Then
            '    vsfDet.Row = 1
            '    vsfDet.Col = vDetItemCode
            '    vsfDet.Focus()
            'End If

            ''Focus on Party Detal

            ''If e.Control = True And e.KeyCode = Keys.P Then
            ''    svtPartyDtl.Focus()
            ''End If

            ''Focus on Header Grid
            'If e.Control = True And e.Shift = True And e.KeyCode = Keys.H Then
            '    vsfHdr.Row = 0
            '    vsfHdr.Col = 5
            '    vsfHdr.Focus()
            'End If

            ''focuse on Total Grid
            'If e.Control = True And e.Shift = True And e.KeyCode = Keys.K Then
            '    vsfTot.Row = 0
            '    vsfTot.Col = 5
            '    vsfTot.Focus()
            'End If
            ''focuse on Footer Grid
            'If e.Control = True And e.Shift = True And e.KeyCode = Keys.W Then
            '    vsfFtr.Row = 0
            '    vsfFtr.Col = 5
            '    vsfFtr.Focus()
            'End If

            ''Toggle Hide
            'If e.Control = True And e.KeyCode = Keys.T Then
            '    For i = 0 To vsfDet.Cols.Count - 1
            '        If vsfOptions.GetData(i, vOthBlnTogHide) = "Y" Then
            '            If vsfDet.Cols(i).Visible = False Then
            '                vsfDet.Cols(i).Visible = True
            '            Else
            '                vsfDet.Cols(i).Visible = False
            '            End If
            '        End If
            '    Next
        End If

    End Sub

    Private Sub btnCopyFrom_Click(sender As Object, e As EventArgs) Handles btnCopyFrom.Click
        Dim strSQL As String, strFMT As String
        strFMT = ""

        strSQL = " Select distinct BOMID, ItemCode from MstItem i, MstHdrBOM b where i.ItemCode <> '' and i.ItemID = b.BOMItemID and InActive <> 'Y'"

        Dim frmDisplay As New frmUtlSingleSel

        With frmDisplay
            .Text = "Select the BOM Item to Copy"
            Call .FillViewVsf(Me, strSQL, txtCopyItem, 0, 0)
            .ShowDialog()
            .vsfDet.Focus()
        End With

        Call FillDocDetails(Val(txtCopyItem.Text))
        PnlCopyFrom.Visible = True
        enableDisableFlds(True)
    End Sub

    Public Function FillBOMItem(ByVal argItem As String, ByVal argQty As Double, ByVal argRow As Int32, Optional ByVal blnRowInsert As Boolean = True) As Boolean
        FillBOMItem = True
    End Function

    Private Sub NonPartyItemFormResize()
        tbAdnlFlds.Size = New Point(intVHdrSize, intVHdrSize2 / 2)
        fraMain.Size = New Point(intfraSize, intfraSize2 / 2)
        vsfDet.Location = New Point(intVsfPos, fraMain.Location.Y + (intVHdrSize2 / 2) + 5)
        vsfDet.Size = New Point(intVsfSize, intVsfSize2 + (intVHdrSize2 / 2))
    End Sub

    Private Sub cboItemCode_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboItemCode.SelectedIndexChanged

    End Sub

    Private Sub vsfEff_BeforeEdit_1(sender As Object, e As RowColEventArgs) Handles vsfEff.BeforeEdit
        e.Cancel = True
    End Sub

    Private Sub ucBtnTools_btnClicked(btnValue As Integer) Handles ucBtnTools.btnClicked
        Try
            With ucBtnTools
                Select Case btnValue
                    Case svtDtlBtns.svtInsert
                        If Trim(vsfDet.GetData(vsfDet.Row, vDetItemCode)) = "" Then
                            vsfDet.Enabled = True
                            Exit Sub
                        Else
                            Call .vsfInsert(vsfDet, vsfDet.Row, vDetItemCode)
                        End If
                        If Val(vsfDet.GetData(vsfDet.Row, vDetSlNo)) <= 0 Then
                            vsfDet.SetData(vsfDet.Row, vDetSlNo, getmaxNo())
                        End If
                        ReArrangeSlNo(vsfDet, vDetSlNo)
                        'Call fillVsfDetDefValues(vsfDet, vsfOptions, vsfDet.Row)

                    Case svtDtlBtns.svtRemove
                        If Not vsfDet.Rows.Count > 1 Then
                            MsgBox("No data to Remove", vbExclamation)
                        End If
                        'vsfDet.SetData(vsfDet.Row, 0, "")
                        Call .vsfRemove(vsfDet, vsfDet.Row, vDetItemCode)
                        ReArrangeSlNo(vsfDet, vDetSlNo)

                    Case svtDtlBtns.svtExport
                        If vsfDet.Rows.Count > 1 Then
                            Call .vsfExport(vsfDet)
                        End If

                End Select
            End With
        Catch ex As Exception

        End Try
    End Sub

    Private Sub SaveThisToolStripMenuItem_Click(sender As System.Object, e As EventArgs) Handles SaveThisToolStripMenuItem.Click
        Call SaveFormSettings(intDocID, blnVHdr, blnVsfMax, blnVFtr, blnVTot)
    End Sub

    Private Sub vsfTot_LostFocus(sender As Object, e As System.EventArgs) Handles vsfTot.LostFocus
        pnlToolTip.Visible = False
    End Sub

    Private Sub txtSearch_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles txtSearch.KeyPress
        If e.KeyChar = "'" Then
            e.Handled = True
        End If
    End Sub

    Private Sub txtSearch_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtSearch.KeyUp
        Dim StrQry As String, strCond As String = ""
        Dim width As Integer = Me.Width
        ''MsgBox(width)
        '' Dim frmsize As Integer = width - 20
        'Dim width1 As Integer = Panel4.Left
        '' MsgBox(width1)
        ''Dim width2 As Integer
        'width2 = width - width1 - 30
        'vsfSearch.Width = width2

        If e.Alt = Keys.Tab Then
            Exit Sub
        End If

        txtSearch.Text = Replace(txtSearch.Text, "'", "`")

        If e.KeyCode = Keys.Enter Then
            If Trim(txtSearch.Text) <> "" And vsfSearch.Rows.Count > 1 Then
                Call vsfSearchSelectedItem()
                Exit Sub
            End If
        End If

        If e.KeyCode = Keys.Down Then
            vsfSearch.Focus()
            Exit Sub
        End If

        If e.KeyCode = Keys.Escape Then
            txtSearch.Text = ""
            Call setSearchGridHt("")
            Exit Sub
        End If

        If Len(txtSearch.Text) < 1 Then
            Call setSearchGridHt("")
            Exit Sub
        End If

        Call setSearchGridHt("FULL")
        If strItemText = Trim(txtSearch.Text) Then Exit Sub
        StrQry = "Select Top " & gIntTopNrows & " ItemCode, ItemName"
        StrQry &= " from " & strDocHdrTblName & " h, MstItem i"
        StrQry &= " where h.BOMItemID=i.ItemID and (ItemCode like '%" & Trim(txtSearch.Text) & "%'"
        StrQry &= " or ItemName like '%" & Trim(txtSearch.Text) & "%')"
        StrQry &= " order by 1"
        Call showWaitCursor()
        dtSearch = clsSVT.getDATATable(StrQry)
        dvSearch = New DataView(dtSearch)
        vsfSearch.DataSource = dvSearch
        Call showNormalCursor()
        vsfSearch.Refresh()
        vsfSearch.ScrollBars = ScrollBars.Both
        vsfSearch.AutoSizeCols()
        strItemText = Trim(txtSearch.Text)
    End Sub

    Private Sub vsfSearchSelectedItem()
        Dim intPartyID As Integer
        If vsfSearch.Rows.Count > 1 Then
            intPartyID = clsSVT.getDataLongValue("Select ItemID from MstItem where ItemCode = '" & vsfSearch.GetData(vsfSearch.Row, 0) & "'", 0, False)
            Call FillDocDetails(intPartyID)
            'txtSearch.Text = ""
            Call setSearchGridHt("")
            txtSearch.Focus()

            Mode = ""
            enableDisableFlds(False)
        End If
    End Sub

    Private Sub vsfSearch_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles vsfSearch.KeyUp
        If e.KeyCode = Keys.Return Then
            Call vsfSearchSelectedItem()
            Exit Sub
        End If
        If e.KeyCode = Keys.Escape Then
            txtSearch.Text = ""
            Call setSearchGridHt("")
            Exit Sub
        End If
    End Sub

    Private Sub txtSearch_LostFocus(sender As Object, e As System.EventArgs) Handles txtSearch.LostFocus
        If vsfSearch.Focused Then
            Exit Sub
        Else
            Call setSearchGridHt("")
        End If
        svtBtnCtrl.btnNew.Focus()
    End Sub

    Private Sub vsfSearch_BeforeEdit(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfSearch.BeforeEdit
        e.Cancel = True
    End Sub

    Private Sub vsfSearch_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles vsfSearch.DoubleClick
        Call vsfSearchSelectedItem()
    End Sub

    Private Sub vsfSearch_LostFocus(sender As Object, e As System.EventArgs) Handles vsfSearch.LostFocus
        Call setSearchGridHt("")
    End Sub

    Private Sub vsfEff_DoubleClick(sender As Object, e As EventArgs)
        If Mode = "M" Then Exit Sub
        With vsfEff
            If .Row <= 0 Then Exit Sub
            Call FillDocDetails(.GetData(.Row, vEffID))
        End With
    End Sub

    Private Sub vsfEff_BeforeEdit(sender As Object, e As RowColEventArgs)
        e.Cancel = True
    End Sub

    Private Sub vsfDet_DoubleClick(sender As Object, e As EventArgs) Handles vsfDet.DoubleClick
        If Mode = "A" Or Mode = "M" Then Exit Sub

        Dim intBOMId As Long
        If vsfDet.GetCellCheck(vsfDet.Row, vDetBOM) = C1.Win.C1FlexGrid.CheckEnum.Checked Then

            strSQL = "select BOMID from MStHdrBOM where BOMItemId = " & vsfDet.GetData(vsfDet.Row, vDetItemID) & " order by Effto desc"
            intBOMId = clsSVT.getDataLongValue(strSQL, 0, False)
            'Call frmMain.MasterForms(frmBOM)
            Dim frmNewBOM As New frmMstBOM
            With frmNewBOM
                Call .initForm(intDocID)
                Call .FillDocDetails(intBOMId)
                .blnChildForm = True
                .Show()
            End With
        End If
    End Sub

    Private Function explodeActMBOM(argRowNum As Integer, argItemID As Long, argQty As Double, argVSF As Object) As Boolean
        Dim rsDummy As oledb.oledbDataReader
        Dim lngBomID As Long, i As Integer

        explodeActMBOM = True

        'argVSF.Rows.count = 1
        'argVSF.cols.count = arrTotCols

        lngBomID = clsSVT.getDataLongValue("Select BOMID from MstHdrBOM where BOMItemID = " & argItemID, 0, False)
        'argVSF.Rows.count = 1

        If lngBomID = 0 Then Exit Function
        Try
            explodeActMBOM = False
            strSQL = "Select i.Itemname, ih.ItemCode as ParentItemCode, i.ItemCode, d.*"
            strSQL &= " from MstDtlBOM d, MstItem i, msthdrbom h, mstitem ih "
            strSQL &= " where d.bomid = h.bomid"
            strSQL &= " and ih.itemid = h.bomitemid"
            strSQL &= " and d.BOMID = " & lngBomID
            strSQL &= " and d.ItemID = i.ItemID ORDER BY SLNO"
            rsDummy = clsSVT.getDataReaderFromSQL(strSQL)

            intLevel = 0
            '##
            With rsDummy
                If Not .HasRows Then
                    With argVSF
                        .Rows.count = .Rows.count + 1
                        .setdata(.Rows.count - 1, arrItemID, argItemID)
                        .setdata(.Rows.count - 1, arrQty, Format(argQty, "#.000"))
                        .setdata(.Rows.count - 1, arrLevel, intLevel)
                        .setdata(.Rows.count - 1, arrKey, "A" & .Rows.count - 1)
                        .setdata(.Rows.count - 1, arrPrntKey, "NONE")
                        .setdata(.Rows.count - 1, arrItemCode, cboItemCode.Text)
                    End With
                End If

                intLevel = intLevel + 1

                If .HasRows Then
                    Do While .Read
                        argVSF.Rows.count = argVSF.Rows.count + 1

                        argVSF.setdata(argVSF.Rows.count - 1, arrItemID, !ItemID)
                        argVSF.setdata(argVSF.Rows.count - 1, arrItemCode, !ItemCode)
                        argVSF.setdata(argVSF.Rows.count - 1, arrParentCode, !ParentItemCode)
                        argVSF.setdata(argVSF.Rows.count - 1, arrParentID, argItemID)
                        argVSF.setdata(argVSF.Rows.count - 1, arrLevel, intLevel)
                        argVSF.setdata(argVSF.Rows.count - 1, arrKey, "A" & argVSF.Rows.count - 1)
                        argVSF.setdata(argVSF.Rows.count - 1, arrPrntKey, "A1")
                        argVSF.setdata(argVSF.Rows.count - 1, arrQty, Format(!qty * argQty, "#.000"))
                    Loop
                End If
            End With

            For i = 1 To argVSF.Rows.count - 1
                strSQL = "Grid Item : " & argVSF.getdata(i, arrItemCode) & "    Selc Item : " & cboItemCode.Text
                If argVSF.getdata(i, arrItemCode) = cboItemCode.Text Then
                    MsgBox(" Item Already Used in.." & vsfDet.GetData(argRowNum, vDetItemCode), vbInformation, Me.Text)
                    vsfDet.TopRow = argRowNum
                    vsfDet.Col = vDetItemCode
                    vsfDet.Row = argRowNum
                    vsfDet.Focus()
                    Exit Function
                End If
            Next i

            explodeActMBOM = True

        Catch
            rsDummy = Nothing
        End Try
    End Function

    Public Sub ShowTreeView(argBOMID As Long)
        tvBOM.Nodes.Clear()
        'Cursor.Current = Cursors.WaitCursor

        strSQL = "select ItemCode, h.*"
        strSQL &= " from MstHdrBOM h, MstItem i"
        strSQL &= " where h.BOMItemId = i.ItemID"
        strSQL &= " and h.BOMID = " & argBOMID

        Dim groupDS As DataSet = addDS(strSQL)

        For Each dr As DataRow In groupDS.Tables(0).Rows
            Dim grpNode As New TreeNode
            grpNode.Text = "Item " & dr("ItemCode").ToString()
            grpNode.Tag = dr("BOMID").ToString()
            Dim value As String = dr("BOMID").ToString()
            tvBOM.Nodes.Add(grpNode)

            addNode(grpNode, value)

            tvBOM.ExpandAll()
        Next
    End Sub

    Private Function addDS(ByVal strQry As String) As DataSet
        Dim strConn As String
        strConn = "Persist Security Info=False"
        strConn = strConn & ";User ID=" & gStrDBLoginName
        strConn = strConn & ";Pwd=" & gStrDBLoginPwd
        strConn = strConn & ";Initial Catalog=" & gStrDBName
        strConn = strConn & ";Data Source=" & gStrServerName
        Dim SqlConn As New oledbConnection(strConn)
        Dim da As New oledbDataAdapter(strQry, SqlConn)
        Dim ds As New DataSet()
        da.Fill(ds)
        Return ds
    End Function

    Private Function addNode(node As TreeNode, argBOMID As Long) As Integer
        Dim intBOMID As Integer
        strSQL = ""

        strSQL = "select * from MstDtlBOM d, MstItem i"
        strSQL &= " where d.ItemID = i.ItemID"
        strSQL &= " and d.BOMID = " & argBOMID
        strSQL &= " order by d.SlNo"

        Dim ledgerDS As DataSet = addDS(strSQL)
        If ledgerDS.Tables(0).Rows.Count > 0 Then
            For Each dr As DataRow In ledgerDS.Tables(0).Rows
                Dim ledNode As New TreeNode()
                ledNode.Text = dr("ItemCode").ToString() & Space(3) & " Qty - " & dr("Qty").ToString()
                ledNode.Tag = dr("ItemID").ToString().Trim()
                Dim temp As String = dr("BOMID").ToString()
                node.Nodes.Add(ledNode)

                strSQL = "select BOMID from MstHdrBOM where BOMItemId = " & dr("ItemID")
                intBOMID = clsSVT.getDataLongValue(strSQL, 0, False)
                'addNode(ledNode, temp)
                addNode(ledNode, intBOMID)
            Next
            Return 0
        Else
            Return 0
        End If
    End Function

    Private Sub tvBOM_NodeMouseDoubleClick(sender As Object, e As TreeNodeMouseClickEventArgs) Handles tvBOM.NodeMouseDoubleClick
        Dim intBOMId As Long
        strSQL = "select BOMID from MStHdrBOM where BOMItemID = " & Val(e.Node.Tag)
        intBOMId = clsSVT.getDataLongValue(strSQL, 0, False)
        If intBOMId > 0 Then Call FillDocDetails(intBOMId)
    End Sub

    Private Sub ResizePanels()
        If strShowTreeView = "Y" Then
            If Mode = "A" Or Mode = "M" Then
                pnlTree.Visible = False
                'pnlDet.Width = pnlDet.Width + pnlTree.Width
                'vsfDet.BorderStyle = BorderStyle.FixedSingle
                'pnlDet.BringToFront()
                pnlTree.SendToBack()
            Else
                pnlTree.Visible = True
                pnlTree.BringToFront()
                'pnlDet.Width = pnlDet.Width - pnlTree.Width
            End If
        End If
    End Sub

    Private Sub vsfHdr_AfterResizeColumn(sender As Object, e As RowColEventArgs) Handles vsfHdr.AfterResizeColumn
        If vsfHdr.Cols(vOthFldCaption).Width >= vsfHdr.Width Then
            vsfHdr.Cols(vOthFldCaption).Width = vsfHdr.Width - 20
        End If
    End Sub

    Private Sub frmMstBOM_Resize(sender As Object, e As EventArgs) Handles Me.Resize
        If Me.Width = 0 Then Exit Sub
        If vsfHdr.Cols(vOthFldCaption).Width >= vsfHdr.Width Then
            vsfHdr.Cols(vOthFldCaption).Width = vsfHdr.Width - 20
        End If
    End Sub

    Private Sub vsfHdr_MouseClick(sender As Object, e As MouseEventArgs) Handles vsfHdr.MouseClick
        If e.Button = MouseButtons.Right Then
            If vsfHdr.Rows.Fixed = 1 Then
                vsfHdr.Rows.Fixed = 0
                Call SaveActFormVsfDet(Me, Me.Text, "Hdr", vsfHdr)
            Else
                vsfHdr.Rows.Fixed = 1
            End If
        End If
    End Sub

    Private Sub lblSearchImage_Click(sender As Object, e As EventArgs) Handles lblSearchImage.Click
        Dim strQry As String
        txtSearch.Text = ""
        If vsfSearch.Visible = True Then
            vsfSearch.Visible = False
        Else
            Call setSearchGridHt("FULL")
            strQry = "select top 30 ITEMCODE,ITEMNAME from MstITEM"
            Call showWaitCursor()
            dtUser = clsSVT.getDATATable(strQry)
            dvUser = New DataView(dtUser)
            vsfSearch.DataSource = dvUser
            Call showNormalCursor()
            vsfSearch.Refresh()
            vsfSearch.ScrollBars = ScrollBars.Both
            vsfSearch.AutoSizeCols()
        End If
    End Sub

    Private Sub FillBOMItemInCombo()
        strSQL = "Select itemcode "
        strSQL &= "from MstItem where InActive = 'N' and BOMItem='Y'"
        If strBOMItemCond <> "" Then
            strSQL &= strBOMItemCond
        End If
        strSQL &= "order by itemcode"
        Call bindCombo(cboItemCode, "itemcode", "itemcode", strSQL)
        cboItemCode.SelectedIndex = -1
        cboItemCode.Focus()
    End Sub

    Private Function chkItem(ByVal argItem As String) As Boolean
        chkItem = False
        Dim strSQL As String, blnFound As Boolean
        cboItemCode.Text = Replace(cboItemCode.Text, "'", "`")

        strSQL = " Select itemcode from MstItem"
        strSQL = strSQL & " where ItemCode = '" & Trim(cboItemCode.Text) & "'"
        strSQL = strSQL & " and InActive = 'N' order by itemCode "

        clsSVT.getDataStringValue(strSQL, "", blnFound)

        If Not blnFound Then
            MsgBox("Item Not Found / Invalid Item.... ", MsgBoxStyle.Information, Me.Text)
            cboItemCode.Focus()
            Exit Function
        End If

        strSQL = "select ItemName from MstItem i, MstUOM u where i.UOMID = i.UOMID and ItemCode = '" & cboItemCode.Text & "'"
        lblItemName.Text = clsSVT.getDataStringValue(strSQL, "", False)

        strSQL = "select UOMCode from MstItem i, MstUOM u where i.UOMID = u.UOMID and ItemCode = '" & cboItemCode.Text & "'"
        lblUOM.Text = "To Make 1 " & clsSVT.getDataStringValue(strSQL, "", False) & " Of { " & Trim(cboItemCode.Text) & " }"
        chkItem = True
    End Function

    Private Sub cboItemCode_Validating(sender As Object, e As CancelEventArgs) Handles cboItemCode.Validating

        If Trim(cboItemCode.Text) = "" Then
            lblItemName.Text = ""
            cboItemCode.Focus()
            Exit Sub
        End If
        vsfDet.Rows.Count = 2
        If chkItem(cboItemCode.Text) Then
            Call FillBOMItems()
        Else
            cboItemCode.Focus()
        End If
    End Sub

    Private Sub FillBOMItems()

    End Sub

    Private Sub btnLoad_Click(sender As Object, e As EventArgs) Handles btnLoad.Click

        cboItemCode.Enabled = False
        If Not chkItem(cboItemCode.Text) Then
            MsgBox("Invalid BOmItem", vbInformation, Me.Text)
            Exit Sub
        End If
        Dim strSQL As String = ""
        Dim strItemcode As String = ""

        Dim intItemId As Int32
        Dim rs As oledb.oledbDataReader

        vsfDet.Rows.Count = 1
        vsfDet.Focus()
        intItemId = clsSVT.getDataLongValue("Select ItemId From MstItem where ItemCode= '" & cboItemCode.Text & "'", 0, False)
        strSQL = "Select BomId from MstHdrBOM where BomItemID= " & intItemId
        strSQL &= " and EffTo = '9998-12-31'"
        intBomPId = clsSVT.getDataLongValue(strSQL, 0, False)
        'Call FillDocDetails(intBomPId)
        'Call FillAutoGenTrnNo()
        fillCopyFromDetails(intBomPId)
        Call RemoveEmptVSFRows(vsfDet, 1, vDetItemCode)
        Call ReArrangeSlNo(vsfDet, vDetSlNo)

        vsfEff.Rows.Count = 1
        strSQL = "Select * from MsthdrBOM where BomID = " & intBomPId
        strSQL &= " and LatestBOM ='Y' and Cancelled='N' order by EffFrom "
        rs = clsSVT.getDataReaderFromSQL(strSQL)
        With rs
            vsfEff.Rows.Count = 1
            If Not .HasRows Then
                rs.Close()
                Exit Sub
            End If
            Do While .Read
                vsfEff.Rows.Count = vsfEff.Rows.Count + 1
                vsfEff.SetData(vsfEff.Rows.Count - 1, vEffID, !BomID)
                vsfEff.SetData(vsfEff.Rows.Count - 1, vEffFrom, !EffFrom)
                vsfEff.SetData(vsfEff.Rows.Count - 1, vEffTo, !EffTo)
            Loop
            .Close()
        End With


    End Sub

    Private Sub cboItemCode_Validated(sender As Object, e As EventArgs) Handles cboItemCode.Validated
        If Trim(cboItemCode.Text) = "" Then
            lblItemName.Text = ""
            cboItemCode.Focus()
            Exit Sub
        End If
        vsfDet.Rows.Count = 2
        If chkItem(cboItemCode.Text) Then
            Call FillBOMItems()
        Else
            cboItemCode.Focus()
        End If
    End Sub
    Private Function getmaxNo() As Integer
        Dim i As Integer, intNo As Integer
        With vsfDet
            intNo = 0
            For i = 1 To .Rows.Count - 1
                If Val(.GetData(i, vDetSlNo)) > intNo Then
                    intNo = Val(.GetData(i, vDetSlNo))
                End If
            Next i
        End With
        intNo = intNo + 1
        getmaxNo = intNo
    End Function


    Private Function Validationdate() As Boolean
        Validationdate = False
        'svtTrn = svtConn.BeginTransaction(IsolationLevel.ReadCommitted)
        Dim strSql As String = ""
        Dim strValidDate As String = ""
        strSql = "Select EffFrom from MstHdrBom where Bomid="
        If intBomPId > 0 Then
            strSql &= intBomPId
        Else
            strSql &= Val(txtNo.Text)
        End If
        strValidDate = clsSVT.getDataStringValue(strSql, "", False, svtTrn)
        If Not CDate(dtEffFrom.Value) >= CDate(strValidDate) Then
            MsgBox("Select Valid Date", vbInformation, Me.Text)
            Exit Function
        End If
        Validationdate = True
    End Function

End Class