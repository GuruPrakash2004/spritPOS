Imports System.Data.oledb
Imports C1.Win.C1FlexGrid
Imports System.IO
Imports System.ComponentModel
Imports C1.Win.C1Input

Public Class frmTrnNonPartyNonItem
    'This form  is applicable for simple Header and Detail table without linking of party and with or without linking of item.
    Dim clsSVT As New svtLibrary
    Dim dtSearch As DataTable
    Dim dvSearch As DataView
    Dim intMaxDocID As Long
    Dim svtTrn As oledb.oledbTransaction
    Dim ctrlList As Object 'IEnumerable(Of C1FlexGrid)

    Public Mode As String 'Control Transaction Add, Edit and Delete
    Public lngViewDocID As Long

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
    Dim intItemDocID As Int32
    Dim blnCalc As Boolean
    Dim intColQty As Int32
    Dim intColrate As Integer
    Dim strTenderFld As String = ""
    Dim intRowTenderFld As Int32

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

    'declarations for detail grid
    Const vDetSlNo = 0
    Const vDetItemSlNo = 1
    Const vDetItemID = 2
    Const vDetItemCode = 3
    Const vDetItemName = 4
    Const vDetShortClosed = 5
    Const vDetShortClosedDT = 6
    Const vDetShortClosedBy = 7
    Const vDetShortClosedQty = 8
    Public intDetCols As Int32 = 9

    'for Reference we have maintain below variables 
    Dim intItemId As Long

    Dim blnDetSection As Boolean = True

    Dim blnTimer As Boolean = False
    Public blnRestore As Boolean = False
    Public strRestoreFileName As String = ""
    Dim strRestorePath As String
    Dim blnNonItem As Boolean = False

    Dim blnVsfMax As Boolean = False
    Dim blnVHdr As Boolean = False
    Dim blnVFtr As Boolean = False
    Dim blnVTot As Boolean = False

    Dim intVsfSize, intVsfSize2, intVsfPos, intVsfPos2, intBtnSize, intBtnSize2, intBtnPos, intBtnPos2, intSvtSize, intSvtSize2, intSvtPos, intSvtPos2 As Int32
    Dim intVHdrSize, intVHdrSize2, intVHdrPos, intVHdrPos2, intVFtrSize, intVFtrSize2, intVFtrPos, intVFtrPos2 As Int32
    Dim intVTotSize, intVTotSize2, intVTotPos, intVTotPos2 As Int32
    Dim intDPinSize, intDPinSize2, intDPinPos, intDPinPos2 As Int32
    Dim intfraSize, intfraSize2

    Public Sub svtBtnCtrl_btnClicked(ByVal argBtnClicked As Integer) Handles svtBtnCtrl.btnClicked
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
                If cboPfx.Enabled = True Then
                    cboPfx.Focus()
                End If
                TimerStartEvent()
                dtDocDate.Focus()
                Mode = "A"
            Case svtBtns.svtClose
                Dim tb As New C1.Win.C1Command.C1DockingTabPage()
                Dim dockTabMain As New C1.Win.C1Command.C1DockingTab()
                Me.Dispose()
                tb.Controls.Clear()
                'frmMain.dockTabMain.SelectedTab.Dispose()
                'frmMain.txtSearch.Focus()

            Case svtBtns.svtEdit
                If Not chkUserRights(strRights, chkRightsPos.EditRights, Me.Text) Then Exit Sub
                If Val(txtDocID.Text) <= 0 Then Exit Sub
                If lblCancelled.Visible = True Then
                    MsgBox("Current record is cancelled. Cannot edit!", vbInformation, Me.Text)
                    Exit Sub
                End If

                If lblCancelled.Visible = True Then Exit Sub
                enableDisableFlds(True)
                svtBtnCtrl.PerformAction(argBtnClicked)
                svtFileAttach.ViewMode = False
                If strAmmReq = "Y" Then
                    fraAmm.Visible = True
                Else
                    fraAmm.Visible = False
                End If
                fraAmm.Enabled = True
                lblTitle.Text = "Edit - " & Me.Text
                TimerStartEvent()
                srcTransaction.Visible = False
                dtDocDate.Focus()
                btnCopyFrom.Enabled = False
                Mode = "M"

            Case svtBtns.svtSave
                If Val(txtNo.Text) <= 0 Then Exit Sub
                If validFlds() Then
                    If SaveDoc() Then
                        svtBtnCtrl.PerformAction(argBtnClicked)
                        Call INIStore("Prefix", Me.Text, cboPfx.Text)
                        blnPrint = False
                        Select Case strPrintType
                            Case "Y"
                                blnPrint = True
                            Case "YP"
                                If MsgBox("Do you want to print this document ?", MsgBoxStyle.YesNo + MsgBoxStyle.Question, Me.Text) = MsgBoxResult.Yes Then
                                    blnPrint = True
                                End If
                        End Select

                        'Call clearFlds()
                        If chkAmmendment.Checked = False Then
                            fraAmm.Visible = False
                        End If
                        enableDisableFlds(False)
                        svtFileAttach.ViewMode = True
                        lblTitle.Text = "View - " & Me.Text
                    Else
                        Exit Sub
                    End If
                    TimerStopEvent()
                    srcTransaction.setFocus()
                    btnCopyFrom.Enabled = False
                    Mode = "V"
                End If
            Case svtBtns.svtPrint
                If Not chkUserRights(strRights, chkRightsPos.PrintRights, Me.Text) Then Exit Sub
                If Val(txtDocID.Text) <= 0 Then Exit Sub
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
                btnCopyFrom.Enabled = False
                srcTransaction.setFocus()
            Case svtBtns.svtMoveFirst, svtBtns.svtMoveLast, svtBtns.svtMoveNext, svtBtns.svtMovePrev
                If Not chkUserRights(strRights, chkRightsPos.ViewRights, Me.Text) Then Exit Sub
                Call FillDocDetails(gMoveRecords(argBtnClicked, strDocHdrTblName, strDocIDFldName, True, txtDocID.Text, ""))

            Case svtBtns.svtDelete
                If txtDocID.Text = "" Then Exit Sub
                If lblCancelled.Visible = True Then
                    MsgBox("Current record is cancelled. Deletion not possible!", vbInformation, Me.Text)
                    Exit Sub
                End If
                If Not chkUserRights(strRights, chkRightsPos.DelRights, Me.Text) Then Exit Sub
                Call deleteDoc()

            Case svtBtns.svtFind
                If Not chkUserRights(strRights, chkRightsPos.ViewRights, Me.Text) Then Exit Sub
                Call showViewTrnForm(Me, strDocHdrTblName, "Listing of " & strFrmCaption, "", "", , "", "", intDocID)
        End Select
    End Sub

    Private Sub AddProc()
        Call clearFlds()
        fraAmm.Visible = False
        enableDisableFlds(True)
        Call DateFreezing(Me, Mode)
        Call fillVsfHFTDefValues(Me)
        Call FillAutoGenTrnNo()
        vsfDet.Rows.Count = 1
        vsfDet.Rows.Count = 2
        ReArrangeSlNo(vsfDet, vDetSlNo)
        srcTransaction.Visible = False
        StMsg()
    End Sub

    Private Sub clearFlds()
        Mode = "V"
        enableDisableFlds(True)
        chkAmmendment.Checked = False
        fraAmm.Visible = False
        txtDocID.Text = ""
        txtDocNo.Text = ""
        txtNo.Text = ""
        svtFileAttach.ClearFiles()
        dtDocDate.Value = getSystemDate()
        Call ClearAddnlGrid(Me)
        vsfDet.Rows.Count = 1
        vsfDet.Rows.Count = 2
        ReArrangeSlNo(vsfDet, vDetSlNo)
        fraAttach.Visible = False
        lblCancelled.Visible = False
        lblAuthorised.Visible = False
        srcTransaction.ClearSearch("")
        srcTransaction.Visible = True
    End Sub

    Public Sub enableDisableFlds(ByVal argFlag As Boolean)
        Dim blnFound As Boolean = False
        If blnLinkedToDoc Then
            Call clsSVT.getDataStringValue("Select " & strDocIDFldName & " from " & strLinkedTODtlTblName & " where " &
                        strDocIDFldName & " = '" & txtDocID.Text & "'", "", blnFound)
        End If
        btnImpXL.Enabled = argFlag
        btnCopyFrom.Enabled = argFlag
        srcTransaction.enabledisable("", True)
        Lock(txtNo, Not argFlag)
    End Sub

    Private Sub frmTrnPartyDoc_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
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

    Private Sub frmTrnPartyDoc_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyUp
        Dim blnFnKey As Boolean = False
        If e.KeyCode = Keys.F5 Then
            bindPartyCombo()
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
            Call SaveActFormVsfDet(Me, strDocHdrTblName, "View", vsfView)
            MsgBox("Saved Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
        If (e.Control) Or blnFnKey Then
            Call svtBtnCtrl.btnCtrlKeyUp(sender, e)
        End If
        Call raiseShortCutEvent(e)
    End Sub

    Public Sub initForm(ByVal argDocID As Integer)
        Dim intMenuID As Integer
        intDocID = argDocID
        Call getUtlTrnData(intDocID)
        Call initScreen()
        intMenuID = clsSVT.getDataLongValue("Select MenuItemID from MstMenu where DocID = " & intDocID, 0, False)
        strRights = getUserRights(intMenuID)
        KeyPreview = True
        Mode = "V"
        ControlMoverOrResizer.Init(fraAttach)
        ControlMoverOrResizer.WorkType = ControlMoverOrResizer.MoveOrResize.MoveAndResize
        ctrlList = GetAllDsgrVsf(Me, GetType(C1.Win.C1FlexGrid.C1FlexGrid))
        srcTransaction.SetName("")
        srcTransaction.vsfAnchor()
        srcTransaction.vsfAnchorReSize(410, 30)
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

        lblCancelled.Visible = False
        lblAuthorised.Visible = False
        enableDisableFlds(False)

        'fill prefix
        Call bindCombo(cboPfx, "Prefix", "Prefix", "Select Prefix from UtlDocNoPrefix where DocID = " & intDocID & " order by 1")

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
        For Each ls As String In Ilistopts
            svtBtnCtrl.addOption(ls)
        Next

        'svtBtnCtrl.addOption("Short Close")

        Dim strItemShForm As String
        strItemShForm = clsSVT.getDataStringValue("Select DocShForm From UtlTransactions where DocID = " & Val(intItemDocID), "", False)

        With vsfDet
            If blnNonItem Then
                .Cols.Count = 2
                intDetCols = 2
            Else
                .Cols.Count = intDetCols
            End If
            .Rows.Count = 2

            If blnNonItem Then
                .SetData(0, vDetSlNo, "Sl No")
                .SetData(0, vDetItemSlNo, strItemShForm & "Slno")

                Call SetColAlign(vsfDet, svtAlign.Left, vDetSlNo)
                Call SetFlexColor(vsfDet, "", vDetSlNo)
                Call InitDetAddnlGrid(Me, vsfDet, vsfOptions, intDetCols, strDocDtlTblName, intDocID)
                .Cols(vDetItemSlNo).Visible = False
            Else
                .SetData(0, vDetSlNo, "Sl No")
                .SetData(0, vDetItemCode, strItemShForm & " Code")
                .SetData(0, vDetItemName, strItemShForm & " Name")
                .SetData(0, vDetShortClosed, "ShortClosed")
                .SetData(0, vDetShortClosedDT, "ShortClosed Date")
                .SetData(0, vDetShortClosedBy, "ShortClosed By")
                .SetData(0, vDetShortClosedQty, "ShortClosed Qty")
                .SetData(0, vDetItemSlNo, strItemShForm & "Slno")
                strDocStdDtlWidth = "525, 105, 15, 2505, 1665, 810, 930, 1155, 1125"

                Call SetColAlign(vsfDet, svtAlign.Left, vDetSlNo, vDetItemCode, vDetItemName)
                Call SetHideCol(vsfDet, True, vDetItemID, vDetItemSlNo, vDetShortClosed, vDetShortClosedBy, vDetShortClosedDT, vDetShortClosedQty)
                Call SetFlexColor(vsfDet, "", vDetSlNo, vDetItemSlNo, vDetItemName, vDetShortClosed, vDetShortClosedBy, vDetShortClosedDT, vDetShortClosedQty)
                Call SetFlexColor(vsfDet, "M", vDetItemCode)

                Call assignDefPFX()
                initItemVsfCombo()

                Call InitDetAddnlGrid(Me, vsfDet, vsfOptions, intDetCols, strDocDtlTblName, intDocID)
            End If

        End With

        intLastCol = 1
        For j = vsfDet.Cols.Count - 1 To 0 Step -1
            If vsfDet.Cols(j).Visible = True Then
                intLastCol = j
                Exit For
            End If
        Next

        With vsfOptions
            .SetData(vDetSlNo, vOthFldDataType, "Numeric")
            .SetData(vDetItemSlNo, vOthFldDataType, "Numeric")
            If Not blnNonItem Then
                .SetData(vDetItemID, vOthFldDataType, "Numeric")
                .SetData(vDetItemCode, vOthFldDataType, "Text")
                .SetData(vDetItemName, vOthFldDataType, "Text")
                .SetData(vDetItemCode, vOthFldCode, "ITEMCODE")
            End If
            intColQty = getColNoFromAddnlDetGrid(vsfOptions, vsfDet, strSHForm & "QTY")
            intColrate = getColNoFromAddnlDetGrid(vsfOptions, vsfDet, strSHForm & "RATE")
        End With

        If clsSVT.getDataStringValue("Select FldPickUpAfterFld from UtlDocDsgrFlds where DocID = " & intDocID & " and FldPickUpAfterFld = '#SVTPARTY#'", "", False) <> "" Then
            blnPartyRelatedFldsPresent = True
        Else
            blnPartyRelatedFldsPresent = False
        End If
        Call bindPartyCombo()
        strAllowDuplicateItem = clsSVT.getDataStringValue("Select blnAllowDuplicateItems from UtlTransactions where Docid=" & intDocID, "", False)
        If strTenderFld <> "" Then
            intRowTenderFld = getRowNoFromAddnlGrid(vsfTot, strTenderFld)
        End If
    End Sub

    Private Sub getUtlTrnData(ByVal argDocID As Integer)
        Dim rsDummy As oledb.oledbDataReader

        rsDummy = clsSVT.getDataReaderFromSQL("Select * from UtlTransactions where DocID = " & argDocID)

        With rsDummy
            If .HasRows Then
                .Read()
                strSHForm = GetVal(!DocShForm, "")
                strDocNoFldName = strSHForm & "No"
                strDocIDFldName = strSHForm & "ID"
                strQtyFldName = strSHForm & "Qty"
                strDocDateFldName = strSHForm & "Date"
                strDocHdrTblName = "TrnHdr" & strSHForm
                strDocDtlTblName = "TrnDtl" & strSHForm
                strAmmReq = GetVal(!AmmReq, "N")
                strAuthReq = GetVal(!AuthReq, "N")
                If clsSVT.getDataStringValue("Select FileAttachReq from UtlTransactions where DocID = " & intDocID, "N", False) = "Y" Then
                    blnShowFileAttach = True
                Else
                    blnShowFileAttach = False
                End If
                blnAuthCalcluate = False
                strFrmCaption = !DocName
                strItemQuery = ""
                If strItemQueryPresent = "Y" Then strItemQuery = !ItemQuery

                Me.Text = GetVal(!DocName, "")
                If GetVal(!LinkedTODoc, "N") = "Y" Then
                    blnLinkedToDoc = True
                Else
                    blnLinkedToDoc = False
                End If
                'lblFrm.Text = strFrmCaption
                strLinkedTOHdrTblName = "TrnHdr" & GetVal(!LinkedTODocShForm, "")
                strLinkedTODtlTblName = "TrnDtl" & GetVal(!LinkedTODocShForm, "")
                strLinkedToDocNoFldName = GetVal(!LinkedTODocShForm, "") & "No"
                strLinkedTODocQtyFldName = GetVal(!LinkedTODocShForm, "") & "Qty"
                strLinkedTODocFldIDName = GetVal(!LinkedTODocShForm, "") & "ID"

                Me.Tag = Me.Text
                lblDocNo.Text = strSHForm & " No."

                strAddReduceStock = ""
                If GetVal(!AffectsStock, "") = "Add" Then
                    strAddReduceStock = "ADD"
                End If
                If GetVal(!AffectsStock, "") = "Reduce" Then
                    strAddReduceStock = "RED"
                End If
                sngDefQty = GetVal(!DefaultQty, 0)
                strPrintType = GetVal(!PrintAfterSave, "N")

                If GetVal(!OfflineCalc, "N") = "Y" Then
                    blnCalc = True
                End If

                intItemDocID = GetVal(!MstDocIDItem, 0)
                If intItemDocID = 0 Then
                    blnNonItem = True
                    strFldItemSlno = "ItemSlno"
                Else
                    strItemShForm = clsSVT.getDataStringValue("Select DocShForm From UtlTransactions where DocID = " & Val(intItemDocID), "", False)
                    strItemTblName = "Mst" & strItemShForm
                    strFldItemID = strItemShForm & "ID"
                    strFldItemSlno = strItemShForm & "Slno"
                    strFldItemCode = strItemShForm & "Code"
                    strFldItemName = strItemShForm & "Name"
                    strItemFilterCond = GetVal(!ItemFilterCond, "")
                End If

                If GetVal(!AllowTenderChange, "N") = "Y" Then
                    strTenderFld = UCase(GetVal(!TenderChangeFld, ""))
                End If

                Call ucBtnTools.HideCopyFrom(GetVal(!AllowCopyFrom, "N"))
                Call ucBtnTools.HideAttachment(GetVal(!FileAttachReq, "N"))
                Call ucBtnTools.HideAmmend(GetVal(!AmmReq, "N"))
                Call ucBtnTools.HideMultiDoc(GetVal(!MultiDocGen, "N"))
                Call ucBtnTools.HideInsertDoc(GetVal(!AllowLoadFromAnotherDoc, "N"))
                Call ucBtnTools.HideTender(GetVal(!AllowTenderChange, "N"))
                Call ucBtnTools.HideImpExp(GetVal(!AllowImpExp, "N"))
                Call ucBtnTools.HideBOM(GetVal(!AllowLoadFromBOM, "N"))
                Call ucBtnTools.HideTotal(GetVal(!OfflineCalc, "N"))

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
        If Mode = "A" Or Mode = "M" Then
            e.Handled = True
        End If
    End Sub

    Private Sub initItemVsfCombo()
        Dim strSQL As String

        If blnNonItem = True Then Exit Sub

        strSQL = "Select " & strFldItemCode & "," & strFldItemName
        strSQL &= " From " & strItemTblName
        strSQL &= " where InActive = 'N'  "
        strSQL &= strItemFilterCond
        strSQL &= " order by " & strFldItemCode

        strSQL = getItemSearchGridQuery(strSQL)

        Call showWaitCursor()
        dtSearch = clsSVT.getDATATable(strSQL)
        dvSearch = New DataView(dtSearch)
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
        Dim blnFound As Boolean
        vsfDet.EditMask = ""
        vsfDet.ComboList = ""

        If blnNonItem Then
            Select Case e.Col
                Case vDetSlNo, vDetItemSlNo
                    e.Cancel = True
                    'we can set the text length of the cell here
            End Select
        Else
            Select Case e.Col
                Case vDetSlNo, vDetItemSlNo, vDetShortClosed, vDetShortClosedBy, vDetShortClosedDT, vDetShortClosedQty, vDetItemName
                    e.Cancel = True
                    'we can set the text length of the cell here
            End Select
            If e.Col = vDetItemCode Then
                If blnLinkedToDoc And Mode = "M" Then
                    If vsfDet.Rows.Count > 1 Then
                        If Val(vsfDet.GetData(vsfDet.Row, vDetItemID)) > 0 And Val(vsfDet.GetData(vsfDet.Row, vDetItemSlNo)) > 0 Then
                            Call clsSVT.getDataLongValue("Select ItemId from " & strLinkedTODtlTblName &
                                " where ItemID = " & vsfDet.GetData(vsfDet.Row, vDetItemID) &
                            " and " & strDocIDFldName & " = " & txtDocID.Text & " and " & strSHForm & "ItemSlno = " & vsfDet.GetData(vsfDet.Row, vDetItemSlNo), 0, blnFound, svtTrn, Nothing)
                            If blnFound Then
                                e.Cancel = True
                                Exit Sub
                            End If
                        End If
                    End If
                End If
            End If
        End If

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
                    StrQry = StrQry & strFldItemCode & " like '*" & .Editor.Text & "*'"
                Case Else
                    If .Editor.Text = " " Then
                        dvSearch.RowFilter = "1 = 1"
                        Exit Function
                    Else
                        StrQry = StrQry & strFldItemCode & " like '%" & .Editor.Text & "%'"
                    End If
            End Select
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
            MsgBox("Invalid Item!!", vbInformation, Me.Text)
            vsfView.Visible = True
            Exit Sub
        Else
            vsfView.Visible = False
            vsfDet.Focus()
            vsfDet.Col = intColQty
            vsfDet.StartEditing()
        End If
    End Sub

    Public Function FillItemDet(ByVal argItem As String, ByVal argRow As Long, Optional ByVal rowIndex As Integer = -1) As Boolean
        Dim rsDummy As oledb.oledbDataReader
        Dim strSQL As String
        Dim blnInActive As Boolean
        FillItemDet = False

        If rowIndex = -1 Then
            rowIndex = vsfDet.Row
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

            strSQL = "Select i.* from "
            strSQL = strSQL & strItemTblName & " i"
            strSQL = strSQL & "  where i.UomID = u.UomID and "
            strSQL = strSQL & " Where " & strFldItemCode & " = '" & argItem & "'"

            rsDummy = clsSVT.getDataReaderFromSQL(strSQL)

            With rsDummy
                If Not .HasRows Then
                    MsgBox(strItemShForm & " does not exist!!", vbInformation, Me.Text)
                    Exit Function
                End If
                .Read()

                vsfDet.SetData(rowIndex, vDetSlNo, rowIndex)
                If Val(vsfDet.GetData(rowIndex, vDetItemSlNo)) <= 0 Then
                    vsfDet.SetData(rowIndex, vDetItemSlNo, getmaxNo())
                End If
                vsfDet.SetData(rowIndex, vDetItemID, rsDummy(strFldItemID))
                vsfDet.SetData(rowIndex, vDetItemName, rsDummy(strFldItemName))
                vsfDet.SetData(rowIndex, vDetItemCode, rsDummy(strFldItemCode))

                .Close()
            End With
        End With
        Call fillItemRelatedDataFromDocDsgnr(intDocID, Val(vsfDet.GetData(rowIndex, vDetItemID)), Me)
        Call vsfOptValidateEdit(Me, vsfDet, vsfOptions, vsfDet.Row, vDetItemCode, "T", strDocDtlTblName, "", "")
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
        If blnNonItem = True Then Exit Sub
        If vsfDet.Col = vDetItemCode Then
            Select Case vsfDet.Editor.Text
                Case ""
                    vsfView.Visible = False
                Case Else
                    vsfView.Top = vsfDet.Top + vsfDet.Rows(vsfDet.Row).Top + (vsfDet.Rows(vsfDet.Row).Height) + 20
                    vsfView.Left = vsfDet.Left + vsfDet.Cols(vsfDet.Col).Left
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
                    Catch ex As Exception

                    End Try
            End Select
        End With
    End Sub

    Private Sub vsfDet_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles vsfDet.GotFocus
        If Mode = "V" Or Mode = "" Then Exit Sub
        '''Changed by sandhya
        If vsfDet.Rows.Count = 1 AndAlso Trim(GetVal(vsfDet.GetData(1, vDetItemCode), "")) = "" Then
            vsfDet.Col = vDetItemCode
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
            If (e.KeyCode = Keys.Escape) And .Row = .Rows.Count - 1 And (.Col = vDetItemCode Or .Col = vDetSlNo) Then
                If blnNonItem = False AndAlso Trim(.GetData(.Row, vDetItemCode)) = "" Then
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
                If blnNonItem = True AndAlso Trim(.GetData(.Row, vDetItemSlNo + 1)) = "" Then
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
            If blnNonItem = False AndAlso .Col = vDetItemCode And e.KeyCode = Keys.Enter Then
                Call loadItemToDtl()
                Exit Sub
            End If
            If .Col = intLastCol And e.KeyCode = Keys.Tab And .Row = .Rows.Count - 1 Then
                If blnNonItem = False AndAlso Trim(.GetData(.Row, vDetItemCode)) <> "" Then
                    .LeftCol = vDetSlNo
                    .Col = vDetSlNo
                    .Col = vDetItemCode
                    .Rows.Count = .Rows.Count + 1
                    .Row = .Rows.Count - 1
                    .TopRow = .Rows.Count - 1
                    .SetData(.Row, vDetSlNo, .Rows.Count - 1)
                Else
                    If blnNonItem = False Then
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
                    Else
                        If blnNonItem AndAlso Trim(.GetData(.Row, vDetItemSlNo + 1)) <> "" Then
                            .LeftCol = vDetSlNo
                            .Col = vDetSlNo
                            .Col = vDetItemSlNo + 1
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
        Dim intDocQty As Double = 0

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
        ' Dim i As Integer
        Call ReArrangeSlNo(vsfDet, vDetSlNo)

        If gridEmpty() Then
            vsfDet.Rows.Count = 1
            vsfDet.Rows.Count = 2
            Call ReArrangeSlNo(vsfDet, vDetSlNo)
            Exit Sub
        End If

        Call AddnlFldCalTotal(Me, vsfOptions, vsfDet, intDetCols, "Customer", "")
    End Sub

    Public Function validFlds() As Boolean
        Dim i As Integer
        Dim blnFound As Boolean
        Dim intDupRow As Integer
        validFlds = False
        '============
        'header validations
        '============

        Call ReArrangeSlNo(vsfDet, vDetSlNo)

        If blnNonItem Then
            Call ReArrangeSlNo(vsfDet, vDetItemSlNo)
        End If

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

                If blnNonItem = False Then
                    intItemId = clsSVT.getDataLongValue("Select " & strFldItemID & " from " & strItemTblName & " where " & strFldItemCode & "= '" & .GetData(i, vDetItemCode) & "'", 0, False)
                    If intItemId = 0 Then
                        MsgBox(strFldItemCode & " does not exist!!", MsgBoxStyle.Critical, Me.Text)
                        .Row = i
                        .TopRow = i
                        .Col = vDetItemCode
                        .Focus()
                        Exit Function
                    End If
                End If

                If Val(.GetData(i, intColQty)) <= 0 Then
                    MsgBox("Please enter the qty greater then 0", MsgBoxStyle.Critical, Me.Text)
                    .Focus()
                    .Row = i
                    .Col = intColQty
                    .StartEditing()
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
        FillDocDetails = False
        If Trim(argDocID) = "" Or Val(argDocID) = 0 Then Exit Function
        txtDocID.Text = argDocID
        StMsg("Filling Header Fields...")
        Call fillDocHdrFlds(argDocID)
        StMsg("Filling Detail Fields...")
        Call fillDtlFields(argDocID)
        If blnShowFileAttach Then
            With svtFileAttach
                .ViewMode = True
                .ClearFiles()
                .LoadFilesFromData("Select AFilename from UtlAttachFiles Where DocID = " & intDocID & " and DocValueID = '" & argDocID & "'")
            End With
        End If

        FillDocDetails = True
        StMsg("")
        With svtFileAttach
            .ClearFiles()
            .LoadFilesFromData("Select AFilename from UtlAttachFiles Where DocID = " & intDocID & " and DocValueID = '" & argDocID & "'")
        End With
        Call refreshAttachBtn()
    End Function

    Private Function fillDocHdrFlds(ByVal argDocID As Long) As Boolean
        Dim rsDocHdr As oledb.oledbDataReader

        Call DateFreezing(Me, "")

        Call ClearAddnlGrid(vsfHdr)
        rsDocHdr = clsSVT.getDataReaderFromSQL("select * from " & strDocHdrTblName & "  " _
                                                            & " where " & strDocIDFldName & " = " & argDocID)
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
            fraAmm.Visible = False
            chkAmmendment.Checked = False
            If strAmmReq = "Y" Then
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

            If Mode = "V" Then
                fraAmm.Enabled = False
            Else
                fraAmm.Enabled = True
            End If
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

        If blnNonItem Then
            strsql = "Select d.* "
            strsql = strsql & " from  " & strDocDtlTblName & "  d "
            strsql = strsql & " where " & strDocIDFldName & "= " & argDocID
            strsql = strsql & " Order by slNo"
        Else
            strsql = "Select d.*, ItemCode, ItemName "
            strsql = strsql & " from  " & strDocDtlTblName & "  d," & strItemTblName & " i"
            strsql = strsql & " where " & strDocIDFldName & "= " & argDocID
            strsql = strsql & " and d.ItemID  =  i." & strFldItemID & ""
            strsql = strsql & " Order by slNo"
        End If

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
                vsfDet.SetData(i, vDetSlNo, i)
                vsfDet.SetData(i, vDetItemSlNo, !ItemSLNO)

                If blnNonItem = False Then
                    vsfDet.SetData(i, vDetItemID, rsDtl(strFldItemID))
                    vsfDet.SetData(i, vDetItemCode, rsDtl(strFldItemCode))
                    vsfDet.SetData(i, vDetItemName, rsDtl(strFldItemName))
                    vsfDet.SetData(i, vDetShortClosed, rsDtl("ShortClosed"))
                    vsfDet.SetData(i, vDetShortClosedDT, rsDtl("ShortClosedDT"))
                    vsfDet.SetData(i, vDetShortClosedBy, rsDtl("ShortClosedBY"))
                    vsfDet.SetData(i, vDetShortClosedQty, rsDtl("ShortClosedQty"))
                End If
                Call FillvsfOptDtlFlds(Me, vsfDet, vsfOptions, rsDtl, i, intDetCols)
            Loop
            .Close()

            If blnNonItem = False Then
                SetHideCol(vsfDet, True, vDetShortClosed, vDetShortClosedBy, vDetShortClosedDT, vDetShortClosedQty)
                For i = 1 To vsfDet.Rows.Count - 1
                    If (vsfDet.GetData(i, vDetShortClosed)) = "Y" Then
                        SetHideCol(vsfDet, False, vDetShortClosed, vDetShortClosedBy, vDetShortClosedDT, vDetShortClosedQty)
                    End If
                Next
            End If
        End With

        Call RemoveEmptVSFRows(vsfDet, 2, vDetItemCode)
        Call ReArrangeSlNo(vsfDet, vDetSlNo)
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

    Private Sub lblAuthorised_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblAuthorised.Click

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
                If .strDelCancelStatus = "CANCELLED" Then
                    lblCancelled.Visible = True
                End If

            End If
        End With
    End Sub

    Public Sub fillCustSupp(ByVal argCustSuppReq As String, ByVal argHdrTbl As String, ByVal argDocFldName As String, ByVal argDocNo As String)

    End Sub

    Public Function gridEmpty() As Boolean
        gridEmpty = False
        If blnNonItem Then
            '''Changed by sandhya
            If vsfDet.Rows.Count = 2 AndAlso Trim(GetVal(vsfDet.GetData(1, vDetSlNo), "")) = "" Then
                gridEmpty = True
            End If
        Else
            If vsfDet.Rows.Count = 2 AndAlso Trim(GetVal(vsfDet.GetData(1, vDetItemCode), "")) = "" Then
                gridEmpty = True
            End If
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
                        FillItemDet(strItemCode, 0, l)
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

    Private Sub frmTrnPartyDoc_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim ToolTip As New ToolTip
        Dim blnNewMode As Boolean = False, blnLoadLastTrn As Boolean = False
        Call initFormSize(Me, strDocHdrTblName, False, 1149, 614, 0, 0)
        Call initFormVsfDet(Me, strDocHdrTblName, vsfDet, "DET")

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

        If blnNonItem Then
            vsfDet.Cols(vDetItemSlNo).Visible = False
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

        If txtDocNo.Text <> "" Then
            Select Case argOptionName
                Case "Short Close"
                    If Not chkUserRights(strRights, chkRightsPos.EditRights, Me.Text) Then Exit Sub
                    If lblCancelled.Visible Then
                        MsgBox("Cancelled Order cannot be shortclosed!", MsgBoxStyle.Critical, Me.Text)
                    Else
                        Call FillDocDetails(txtDocID.Text)
                        'With frmUtlSOShortClose
                        '    .loadSODtls(Me, Val(txtDocID.Text), txtDocNo.Text, vsfDet)
                        '    .ShowDialog()
                        'End With
                    End If


                Case Else
                    If Not chkUserRights(strRights, chkRightsPos.PrintRights, Me.Text) Then Exit Sub
                    Dim frmv As New frmUtlReportViewer
                    With frmv
                        .StartPosition = FormStartPosition.CenterScreen
                        .ShowRptFileFromUtlOptions(intDocID, Val(txtDocID.Text), argOptionName, True)
                        .Show()
                    End With
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
            If Mode = "M" Then
                clsSVT.execSQLStatement("Delete from " & strDocDtlTblName & " where " & strDocIDFldName & " = " & Val(txtDocID.Text), svtTrn)
                intMaxDocID = Val(txtDocID.Text)
            End If

            If Not UpdtDocHdrDtl() Then
                SaveDoc = False
                svtTrn.Rollback()
                Exit Function
            End If

            Call svtFileAttach.saveAttachments(intDocID, Val(intMaxDocID), True, svtTrn)

            'If Mode = "A" Then
            '    Call RecordAuditLog(strFrmCaption, Val(txtDocID.Text), UserActions.AddAction, svtTrn)
            'Else
            '    Call RecordAuditLog(strFrmCaption, Val(txtDocID.Text), UserActions.EditAction, svtTrn)
            'End If

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
            If strAmmReq = "Y" Then
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
            strSQL = strSQL & "'" & getPrintDocFmtNo(intDocID, txtDocNo.Text,
                                    cboPfx.Text, txtNo.Text, svtTrn) & "', "
            strSQL = strSQL & "'" & getDateFormatInInsert(dtDocDate.Value) & "', "

            strSQL = strSQL & "'" & gUserCode & "', "
            strSQL = strSQL & "'" & getDateFormatInInsert(getSystemDateTime(svtTrn), True) & "', "

            If strAmmReq = "Y" Then
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
                strDtlSQL = GetDetSqlString()
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
            If strAmmReq = "Y" Then
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
            strSQL = strSQL & "EditedBy = '" & gUserCode & "', "
            strSQL = strSQL & "EditedOn = '" & getDateFormatInInsert(getSystemDateTime(svtTrn), True) & "', "


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
        strFlds = strFlds & " (" & strDocIDFldName & " , slNo, "

        If blnNonItem = False Then
            strFlds = strFlds & strFldItemID & ","
        End If

        strFlds = strFlds & strFldItemSlno & ","

        For i = 1 To vsfDet.Rows.Count - 1
            If argDocID > 0 AndAlso Mode = "M" Then
                strValues = strValues & "(" & argDocID & ", "
            Else
                strValues = strValues & "(@MaxID, "
            End If

            With vsfDet
                strValues = strValues & Val(.GetData(i, vDetSlNo)) & ","
                If blnNonItem = False Then
                    strValues = strValues & Val(.GetData(i, vDetItemID)) & ","
                End If
                strValues = strValues & Val(.GetData(i, vDetItemSlNo)) & ","
            End With

            Call GetvsfOptHdrDetInsertString(Me, vsfDet, vsfOptions, intDetCols, i, strFlds1, strValues, svtTrn)

            strValues = strValues & ","

            ' Call chkCustItemQOH(Val(svtPartyDtl.getID()), Val(vsfDet.GetData(i, vDetItemID)), True, svtTrn)
        Next


        'strFlds = Mid(strFlds, 1, Len(strFlds) - 1)
        strValues = Mid(strValues, 1, Len(strValues) - 1)

        GetDetSqlString = strFlds & strColFlds & " values " & strValues
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
            If .GetData(.Row, vOthPerAmount) = "Y" And .Col <> vOthFldDataPer Then Exit Sub
            .StartEditing()
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

    Private Sub NextFocus()
        vsfDet.Row = 1
        vsfDet.Col = vDetItemCode
        vsfDet.Focus()
    End Sub

    Private Function getSrcQry(argValue As String, ByVal argCond As Boolean) As String
        Dim strSQL As String = "Select h." & strDocIDFldName & "," & strDocNoFldName & "," & strSHForm & "Date "
        strSQL &= " from "
        strSQL &= strDocHdrTblName & " h "
        strSQL &= " where "
        strSQL &= " 1=1 "
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
        strSQL &= " order by 1,2,3"
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

    Private Sub ucBtnTools_btnClicked(btnValue As Integer) Handles ucBtnTools.btnClicked
        Dim intLinkedDocID As Integer = 0
        Dim strLinkedDocNo As String = ""
        Select Case btnValue
            Case 9, 10, 11, 12, 13, 16, 17, 18, 19, 20, 21
            Case Else
                If gridEmpty() Then Exit Sub
        End Select
        Select Case btnValue
            Case 8, 10, 11, 15, 20
            Case Else
                If Not (Mode = "A" Or Mode = "M") Then Exit Sub
        End Select
        Try
            With ucBtnTools
                Select Case btnValue
                    Case svtDtlBtns.svtInsert
                        Call .vsfInsert(vsfDet, vsfDet.Row, vDetItemCode)
                    Case svtDtlBtns.svtRemove
                        If blnLinkedToDoc And Mode = "M" Then
                            If Val(vsfDet.GetData(vsfDet.Row, vDetItemID)) > 0 And Val(vsfDet.GetData(vsfDet.Row, vDetItemSlNo)) > 0 Then
                                intLinkedDocID = clsSVT.getDataLongValue("Select " & strLinkedTODocFldIDName & " from " & strLinkedTODtlTblName &
                                    " where ItemID = " & Val(vsfDet.GetData(vsfDet.Row, vDetItemID)) &
                                " and " & strDocIDFldName & " = " & txtDocID.Text & " and " & strSHForm & "ITemSlno = " & vsfDet.GetData(vsfDet.Row, vDetItemSlNo), 0, False, svtTrn)
                                If intLinkedDocID > 0 Then
                                    strLinkedDocNo = clsSVT.getDataStringValue("Select " & strLinkedToDocNoFldName & " from " & strLinkedTOHdrTblName & " where " & strLinkedTODocFldIDName & " = " & intLinkedDocID, "", False)
                                    MsgBox(strSHForm & "No already exist in " & strLinkedToDocNoFldName & " : '" & strLinkedDocNo & "'", MsgBoxStyle.Critical, Me.Text)
                                    Exit Sub
                                End If
                            End If
                        End If
                        Call .vsfRemove(vsfDet, vsfDet.Row, vDetItemCode)
                        Call CalTotal()
                    Case svtDtlBtns.svtCopy
                        Call .vsfCopy(vsfDet, vsfDet.Row, vDetItemCode)
                    Case svtDtlBtns.svtPaste
                        Call .vsfPaste(vsfDet, vsfDet.Row, vDetItemCode)
                    Case svtDtlBtns.svtUp
                        Call MoveOneRowUp(vsfDet, vDetSlNo)
                    Case svtDtlBtns.svtDown
                        Call MoveOneRowDown(vsfDet, vDetSlNo)
                    Case svtDtlBtns.svtReplace
                        Call .vsfReplace(vsfDet, vsfDet.Row, vsfDet.Col, vDetItemCode)
                    Case svtDtlBtns.svtExport
                        Call .vsfExport(vsfDet)
                    Case svtDtlBtns.svtImport
                        If .vsfImport(vsfDet, vDetItemCode, vsfOptions, intDetCols) Then
                            With vsfDet
                                For i As Int32 = 1 To .Rows.Count - 1
                                    If Not FillItemDet(.GetData(i, vDetItemCode), i, i) Then
                                        vsfDet.Rows.Count = 1
                                        vsfDet.Rows.Count = 2
                                        Exit Sub
                                    End If
                                Next
                                MsgBox("XL imported into Grid...", MsgBoxStyle.Information, Me.Text)
                            End With
                        End If
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
                    Case svtDtlBtns.svtAmmend
                        If fraAmm.Visible Then
                            fraAmm.Visible = False
                        Else
                            fraAmm.Visible = True
                        End If
                    Case svtDtlBtns.svtFreeze
                        Call .vsfFreeze(vsfDet, vsfDet.Col)
                    Case svtDtlBtns.svtAddItems
                        Dim obj As New frmUtlAddItems
                        obj.frm = Me
                        obj.argItemCode = vDetItemCode
                        obj.ShowDialog()
                        obj.Dispose()
                        obj.Close()
                    Case svtDtlBtns.svtTender
                        'Call .vsfTenderChange(intDocID, Val(txtDocID.Text), Trim(txtDocNo.Text), Val(vsfTot.GetData(intRowTenderFld, vOthFldDataValue)))
                    Case svtDtlBtns.svtInsertDoc
                      '  Call .vsfInsertDoc(intDocID, Me)
                    Case svtDtlBtns.svtMultiDoc
                        Call .vsfMultDoc(intDocID, Me, Trim(txtDocNo.Text))
                    Case svtDtlBtns.svtTrnExport
                        Dim strPartyDetails As String = ""
                        Call .DocXlExp(Me, strSHForm, tbAdnlFlds, vsfDet, tbAdnlFldsFtr, vsfTot, intDocID, txtDocNo.Text, dtDocDate.Value, cboPfx.Text, strPartyDetails)
                    Case svtDtlBtns.svtBOM
                        'Call .vsfBOM(Me, vsfDet.GetData(vsfDet.Row, vDetItemID), vsfDet.Row, vsfDet.GetData(vsfDet.Row, intColQty))
                    Case svtDtlBtns.svtCalTotal
                        Call CalTotal()
                End Select
            End With
            ReArrangeSlNo(vsfDet, vDetSlNo)
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

    Private Sub btnCopyFrom_Click(sender As Object, e As EventArgs) Handles btnCopyFrom.Click
        Dim frmUS As New frmUtlSingleSel
        Dim strsql As String

        strsql = "SELECT  SOID, SONo, SODate, CustCode, CustRefNo, OrderFrm, SoValue"
        strsql = strsql & " FROM TrnHdrSO h, MstCust c"
        strsql = strsql & " where h.CustID = c.CustID"
        strsql = strsql & " order by soid desc"
        'With frmUS
        '    .Text = "Select from... "
        '    Call .FillViewVsf(Me, strsql, txtToLoadId, 0, 0)
        '    .ShowDialog()
        '    .vsfDet.Focus()
        'End With

        'If Val(txtToLoadId.Text) > 0 Then
        '    Call FillDocDetails(Val(txtToLoadId.Text))
        '    txtDocID.Text = ""
        '    Call FillAutoGenTrnNo()
        'End If

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
            End If

            'Remove Row event
            If e.Control = True And e.Shift = True And e.KeyCode = Keys.R Then
                Call ucBtnTools.vsfRemove(vsfDet, vsfDet.Row, vDetItemCode)
            End If

            'Copy  event
            If e.Control = True And e.Shift = True And e.KeyCode = Keys.C Then
                Call ucBtnTools.vsfCopy(vsfDet, vsfDet.Row, vDetItemCode)
            End If
            'Paste Event
            If e.Control = True And e.Shift = True And e.KeyCode = Keys.P Then
                Call ucBtnTools.vsfPaste(vsfDet, vsfDet.Row, vDetItemCode)
            End If

            'MoveUp Event
            If e.Control = True And e.Shift = True And e.KeyCode = Keys.U Then
                Call MoveOneRowUp(vsfDet, vDetSlNo)
            End If

            'MoveDown Event
            If e.Control = True And e.Shift = True And e.KeyCode = Keys.V Then
                Call MoveOneRowDown(vsfDet, vDetSlNo)
            End If

            'RePlace Rest Event
            If e.Control = True And e.Shift = True And e.KeyCode = Keys.S Then
                Call ucBtnTools.vsfReplace(vsfDet, vsfDet.Row, vsfDet.Col, vDetItemCode)
            End If

            'Exprot to Excel
            If e.Control = True And e.Shift = True And e.KeyCode = Keys.E Then
                Call ucBtnTools.vsfExport(vsfDet)
            End If

            'Import from Excel
            If e.Control = True And e.Shift = True And e.KeyCode = Keys.F Then
                If ucBtnTools.vsfImport(vsfDet, vDetItemCode, vsfOptions, intDetCols) Then
                    With vsfDet
                        For i As Int32 = 1 To .Rows.Count - 1
                            If Not FillItemDet(.GetData(i, vDetItemCode), i, i) Then
                                vsfDet.Rows.Count = 1
                                vsfDet.Rows.Count = 2
                                Exit Sub
                            End If
                        Next
                        MsgBox("XL imported into Grid...", MsgBoxStyle.Information, Me.Text)
                    End With
                End If
            End If


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
            If e.Control = True And e.Shift = True And e.KeyCode = Keys.X Then
                Dim obj As New frmUtlCopyFrom
                With obj
                    .argForm = Me
                    .strFrmCaption = strFrmCaption
                    .argvsfOptions = vsfOptions
                    .argvsf = vsfDet
                    .ShowDialog()
                End With
            End If

            'Append
            If e.Control = True And e.Shift = True And e.KeyCode = Keys.J Then
                If fraAmm.Visible Then
                    fraAmm.Visible = False
                Else
                    fraAmm.Visible = True
                End If
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
                ' Call ucBtnTools.vsfInsertDoc(intDocID, Me)
            End If

            'Multi Doc Generation
            If e.Control = True And e.Shift = True And e.KeyCode = Keys.G Then
                Call ucBtnTools.vsfMultDoc(intDocID, Me, Trim(txtDocNo.Text))
            End If
            If e.Control = True And e.Shift = True And e.KeyCode = Keys.L Then

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
        '    svtPartyDtl.Focus()
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
        intVsfSize = vsfDet.Size.Width
        intVsfSize2 = vsfDet.Size.Height
        intVsfPos = vsfDet.Location.X
        intVsfPos2 = vsfDet.Location.Y

        intVHdrSize = tbAdnlFlds.Size.Width
        intVHdrSize2 = tbAdnlFlds.Size.Height
        intVHdrPos = tbAdnlFlds.Location.X
        intVHdrPos2 = tbAdnlFlds.Location.Y

        intVFtrSize = tbAdnlFldsFtr.Size.Width
        intVFtrSize2 = tbAdnlFldsFtr.Size.Height
        intVFtrPos = tbAdnlFldsFtr.Location.X
        intVFtrPos2 = tbAdnlFldsFtr.Location.Y

        intVTotSize = vsfTot.Size.Width
        intVTotSize2 = vsfTot.Size.Height
        intVTotPos = vsfTot.Location.X
        intVTotPos2 = vsfTot.Location.Y

        intBtnSize = ucBtnTools.Size.Width
        intBtnSize2 = ucBtnTools.Size.Height
        intBtnPos = ucBtnTools.Location.X
        intBtnPos2 = ucBtnTools.Location.Y

        intfraSize = fraMain.Size.Width
        intfraSize2 = fraMain.Size.Height
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
        'Dim intTempSize, intTempSize2 As Int32
        'Dim intTempPos, intTempPos2 As Int32
        Select Case argType
            Case "H"
                If blnVHdr Then
                    tbAdnlFlds.Size = New Point(intVHdrSize, intVHdrSize2)
                    vsfDet.Size = New Point(intVsfSize, intVsfSize2)
                    ' btnDetPin.Location = New Point(intDPinPos, intDPinPos2)
                    ' btnDetPin.Visible = True
                    blnVHdr = False
                Else
                    ' btnDetPin.Location = New Point(intVHdrPos - intDPinSize, intDPinPos2)
                    ' btnDetPin.Visible = False
                    vsfDet.Size = New Point(intVsfSize - intVHdrSize, intVsfSize2)
                    tbAdnlFlds.Size = New Point(intVHdrSize, intVHdrSize2 + intVsfSize2)
                    blnVHdr = True
                End If
            Case "F"
                If blnVFtr Then
                    tbAdnlFldsFtr.Size = New Point(intVFtrSize, intVFtrSize2)
                    tbAdnlFldsFtr.Location = New Point(intVFtrPos, intVFtrPos2)
                    vsfDet.Size = New Point(intVsfSize, intVsfSize2)
                    vsfDet.Location = New Point(intVsfPos, intVsfPos2)
                    blnVFtr = False
                Else
                    tbAdnlFldsFtr.Location = New Point(intVFtrPos, intVsfPos2)
                    tbAdnlFldsFtr.Size = New Point(intVFtrSize, intVsfSize2 + intVFtrSize2 + ucBtnTools.Size.Height)
                    vsfDet.Location = New Point(tbAdnlFldsFtr.Location.X + tbAdnlFldsFtr.Size.Width, intVsfPos2)
                    vsfDet.Size = New Point(intVsfSize - intVFtrSize, intVsfSize2)
                    blnVFtr = True
                End If
            Case "T"
                If blnVTot Then
                    vsfDet.Size = New Point(intVsfSize, intVsfSize2)
                    vsfTot.Location = New Point(intVTotPos, intVTotPos2)
                    vsfTot.Size = New Point(intVTotSize, intVTotSize2)
                    ' btnDetPin.Visible = True
                    blnVTot = False
                Else
                    vsfDet.Size = New Point(intVsfSize - intVTotSize, intVsfSize2)
                    vsfTot.Location = New Point(intVTotPos, intVsfPos2)
                    vsfTot.Size = New Point(intVTotSize, intVTotSize2 + fraAmm.Size.Height + intVsfSize2 + 10)
                    ' btnDetPin.Visible = False
                    blnVTot = True
                End If
            Case Else
                If blnVsfMax Then
                    vsfDet.Location = New Point(intVsfPos, intVsfPos2)
                    vsfDet.Size = New Point(intVsfSize, intVsfSize2)
                    ucBtnTools.Location = New Point(intBtnPos, intBtnPos2)
                    btnDetPin.Location = New Point(intDPinPos, intDPinPos2)
                    blnVsfMax = False
                Else
                    vsfDet.Location = New Point(intVsfPos, fraMain.Location.Y)
                    vsfDet.Size = New Point(intVsfSize, intVHdrSize2 + intVsfSize2 + intVTotSize2 + fraAmm.Size.Height + 15)
                    vsfDet.BringToFront()
                    ucBtnTools.Location = New Point(intBtnPos, svtBtnCtrl.Location.Y)
                    svtBtnCtrl.BringToFront()
                    btnDetPin.Location = New Point(intDPinPos, intVTotPos2 + intVTotSize2 - 15)
                    btnDetPin.BringToFront()
                    blnVsfMax = True
                End If
                'If blnVsfMax = False Then
                '    ucBtnTools.Dock = DockStyle.Bottom
                '    ucBtnTools.BringToFront()
                '    svtBtnCtrl.Dock = DockStyle.Bottom

                '    intTempSize2 = vsfDet.Size.Height + fraMain.Size.Height + tbAdnlFldsFtr.Size.Height
                '    intTempPos = fraMain.Location.X
                '    intTempPos2 = fraMain.Location.Y


                '    vsfDet.Location = New Point(intTempPos, intTempPos2)
                '    vsfDet.Size = New Point(intVsfSize, intTempSize2)

                '    vsfDet.BringToFront()
                '    blnVsfMax = True
                'Else
                '    vsfDet.Dock = DockStyle.None
                '    ucBtnTools.Dock = DockStyle.None
                '    svtBtnCtrl.Dock = DockStyle.None

                '    vsfDet.Size = New Point(intVsfSize, intVsfSize2)
                '    ucBtnTools.Size = New Point(intBtnSize, intBtnSize2)
                '    svtBtnCtrl.Size = New Point(intSvtSize, intSvtSize2)

                '    vsfDet.Location = New Point(intVsfPos, intVsfPos2)
                '    ucBtnTools.Location = New Point(intBtnPos, intBtnPos2)
                '    svtBtnCtrl.Location = New Point(intSvtPos, intSvtPos2)
                '    blnVsfMax = False
                'End If
        End Select
        Call pinDisable(argType)
        Call frmTrnSO_Resize(Nothing, Nothing)
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
    End Sub

    Private Sub NonPartyItemFormResize()
        tbAdnlFlds.Size = New Point(intVHdrSize, intVHdrSize2 / 2)
        fraMain.Size = New Point(intfraSize, intfraSize2 / 2)
        vsfDet.Location = New Point(intVsfPos, fraMain.Location.Y + (intVHdrSize2 / 2) + 5)
        vsfDet.Size = New Point(intVsfSize, intVsfSize2 + (intVHdrSize2 / 2))
        Call getCtrlSize()
    End Sub

    Private Sub SaveThisToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles SaveThisToolStripMenuItem.Click
        Call SaveFormSettings(intDocID, blnVHdr, blnVsfMax, blnVFtr, blnVTot)
    End Sub

    Private Function CheckforLinkedDoc() As Boolean
        CheckforLinkedDoc = False
        If blnLinkedToDoc = False Then Exit Function
        If Mode <> "M" Then Exit Function
        Dim blnFound As Boolean = False
        Call clsSVT.getDataLongValue("Select " & strLinkedTODocFldIDName & " from " & strLinkedTODtlTblName &
            " where " & strDocIDFldName & " = " & txtDocID.Text, 0, blnFound)
        If blnFound Then
            Exit Function
        End If
        CheckforLinkedDoc = True
    End Function

    Private Sub vsfTot_LostFocus(sender As Object, e As System.EventArgs) Handles vsfTot.LostFocus
        pnlToolTip.Visible = False
    End Sub

    Private Sub frmTrnSO_Resize(sender As Object, e As EventArgs) Handles Me.Resize
        If Me.Width = 0 Then Exit Sub
        btnFtrPin.Top = vsfFtr.Top + vsfFtr.Height - btnFtrPin.Height
        btnFtrPin.Left = vsfFtr.Left + vsfFtr.Width - btnFtrPin.Width

        btnHdrPin.Top = vsfHdr.Top + vsfHdr.Height - btnHdrPin.Height
        btnHdrPin.Left = vsfHdr.Left + vsfHdr.Width - btnHdrPin.Width

        btnTotPin.Top = vsfTot.Top + vsfTot.Height - btnTotPin.Height
        btnTotPin.Left = vsfTot.Left + vsfTot.Width - btnTotPin.Width

        btnDetPin.Top = vsfDet.Top + vsfDet.Height - btnDetPin.Height
        btnDetPin.Left = vsfDet.Left + vsfDet.Width - btnDetPin.Width

        If vsfHdr.Cols(vOthFldCaption).Width >= vsfHdr.Width Then
            vsfHdr.Cols(vOthFldCaption).Width = vsfHdr.Width - 20
        End If
        If vsfFtr.Cols(vOthFldCaption).Width >= vsfFtr.Width Then
            vsfFtr.Cols(vOthFldCaption).Width = vsfFtr.Width - 20
        End If
    End Sub

    Private Sub optMouser_Click(sender As Object, e As EventArgs)
        Call initItemVsfCombo()
    End Sub

    Private Sub btnImpXL_DropDownItemClicked(sender As Object, e As C1.Win.C1Input.DropDownItemClickedEventArgs) Handles btnImpXL.DropDownItemClicked
        If UCase(e.ClickedItem.Text) = "EXPORT TEMPLATE" And My.Computer.Keyboard.CtrlKeyDown Then
            'ExpToXL("OLD")
        ElseIf UCase(e.ClickedItem.Text) = "EXPORT TEMPLATE" Then
            ' ExpToXL()
        ElseIf UCase(e.ClickedItem.Text) = "IMPORT FROM XL" And My.Computer.Keyboard.CtrlKeyDown Then
            '  Call OldImportExcel()
        ElseIf UCase(e.ClickedItem.Text) = "IMPORT FROM XL" Then
            ' Call NewImportExcel()
        End If
    End Sub

    Private Sub vsfHdr_AfterResizeColumn(sender As Object, e As RowColEventArgs) Handles vsfHdr.AfterResizeColumn
        If vsfHdr.Cols(vOthFldCaption).Width >= vsfHdr.Width Then
            vsfHdr.Cols(vOthFldCaption).Width = vsfHdr.Width - 20
        End If
    End Sub

    Private Sub svtBtnCtrl_SizeChanged(sender As Object, e As EventArgs) Handles svtBtnCtrl.SizeChanged

    End Sub

    Private Sub vsfFtr_AfterResizeColumn(sender As Object, e As RowColEventArgs) Handles vsfFtr.AfterResizeColumn
        If vsfFtr.Cols(vOthFldCaption).Width >= vsfFtr.Width Then
            vsfFtr.Cols(vOthFldCaption).Width = vsfFtr.Width - 20
        End If
    End Sub
End Class