Imports System.ComponentModel
Imports C1.Win.C1FlexGrid

Public Class frmUtlShortclose
    Dim clsSVT As New svtLibrary

    Dim dtSearch As DataTable
    Dim dvSearch As DataView

    Public strList As String
    Dim strPartyList As String
    Dim strItemNameList As String
    Dim strItemIDList As String

    Dim intDocId As Integer
    Dim strSQL As String

    Dim strFrmCaption As String
    'Basic Details
    Dim strSHForm As String
    Dim strDocHdrTblName As String
    Dim strDocDtlTblName As String
    Dim strDocIDFldName As String
    Dim strDocNoFldName As String
    Dim strDocDateFldName As String
    Dim strQtyFldName As String
    Dim intQtyDecimals As Integer


    'Detail section fields 
    Dim intPartyDocID As Int32
    Dim intItemDocID As Int32

    'Party Related Information
    '========================
    Dim strPartyTblName As String
    Dim strFldPartyID As String
    Dim strFldPartyCode As String
    Dim strFldPartyName As String
    Dim strPartyFlds As String

    'Additional Fields
    Dim strPartyType As String

    'Item Related Information
    '========================
    Dim strItemTblName As String
    Dim strFldItemID As String
    Dim strFldItemSlno As String
    Dim strFldItemCode As String
    Dim strFldItemName As String
    Dim strItemFlds As String
    Dim strItemShForm As String

    ''For linked

    Dim intLinkPartyDocID As Integer
    Dim intLinkItemDocID As Integer

    Dim strCurDocShForm As String
    Dim strSrcDocShForm As String

    Dim strDocQtyFldName As String
    Dim strDocRateFldName As String
    Dim strDocAmtFldName As String
    Dim blnRateColReq As Boolean
    Dim strDestSHForm As String

    Dim strLinkedFromHdrTblName As String
    Dim strLinkedFromDtlTblName As String
    Dim strLinkedFromDocNoFldName As String
    Dim strLinkedFromDocQtyFldName As String
    Dim strLinkedFromDocRateFldName As String
    Dim strLinkedFromDocDateFldName As String

    Dim strPartyShForm As String
    Dim strPMstTblName As String = ""
    Dim strPMstFldCode As String = ""
    Dim strPMstFldName As String = ""
    Dim strPMstFldID As String = ""

    Dim strIMstTblName As String = ""
    Dim strIMstFldCode As String = ""
    Dim strIMstFldName As String = ""
    Dim strIMstFldID As String = ""

    Dim blnPendParty As Boolean
    Dim blnPendDocNo As Boolean
    Dim blnPendSingleDoc As Boolean
    Dim blnPartyPresent As Boolean
    Dim blnItemPresent As Boolean
    Dim blnAuthReq As Boolean
    Dim blnQOH As Boolean

    Dim strPendingQryFlds As String = ""
    Dim strPendingQryTbls As String = ""
    Dim strPendingQryCond As String = ""

    Dim intTrnID As Integer

    Dim dtCumQty As DataTable

    Dim vDetDocID As Integer
    Dim intColItemID As Integer, intColItemSlno As Integer, intColPendQty As Integer
    Dim intColShortclosed As Integer, intcolShortclosedBy As Integer, intColShortclosedDt As Integer, intColShortclosedQty As Integer

    Public Sub New()
        InitializeComponent()
    End Sub

    Private Sub chkSelItemCode_CheckedChanged(sender As Object, e As EventArgs) Handles chkSelItemCode.CheckedChanged
        If cboDocName.Text = "" Then Exit Sub
        Dim strSQL As String

        If chkSelItemCode.Checked = False Then
            strList = ""
            strItemIDList = ""
            Exit Sub
        End If

        Dim frmSel As New frmUtlMultiSelection

        strSQL = "Select distinct 0 as sel, ItemId, ItemCode, ItemName from MstItem"
        strSQL &= " where ItemID > 0"
        strSQL &= " and ItemCode Not in ('-', '')"
        strSQL &= " and InActive = 'N'"
        strSQL &= " And Authorised = 'Y'"
        strSQL &= " order by ItemCode"

        Call frmSel.InitVsfSel(strSQL, 1, 1, True, Me, False)
        frmSel.Text = "Select [Multiple] Item"
        frmSel.vsfDet.SetData(0, 0, "Sel")
        frmSel.ShowDialog()
        strItemIDList = strList

        If Trim(strItemIDList) = "" Then chkSelItemCode.Checked = False
    End Sub

    Private Sub chkParty_CheckedChanged(sender As Object, e As EventArgs) Handles chkSelParty.CheckedChanged
        If cboDocName.Text = "" Then Exit Sub
        Dim strSQL As String

        If chkSelParty.Checked = False Then
            strList = ""
            strPartyList = ""
            Exit Sub
        End If

        Dim frmSel As New frmUtlMultiSelRpt

        strSQL = "Select distinct 0 as sel, " & strFldPartyID & ", " & strFldPartyCode & " from " & strPartyTblName & " where " & strFldPartyID & " > 0 and "
        strSQL = strSQL & " " & strFldPartyCode & " Not in ('-' , '') and InActive = 'N' and Authorised = 'Y' order by " & strFldPartyCode
        Call frmSel.InitVsfSel(strSQL, 1, 1, True, Me, False)
        frmSel.Text = "Select [Multiple] " & strPartyShForm
        frmSel.vsfDet.SetData(0, 0, "Sel")
        frmSel.ShowDialog()
        strPartyList = strList
        If strPartyList = "" Then chkSelParty.Checked = False
    End Sub

    Private Sub frmUtlShortclose_Load(sender As Object, e As EventArgs) Handles Me.Load
        Me.KeyPreview = True
        Call bindCombo(cboDocName, "DocName", "DocName", getDocQry("DocName"))
        cboDocName.Text = ""
        cboDocNo.Text = ""
        pnlDate.Enabled = False
        rbBoth.Checked = True
        chkDate.Checked = True
        dtpFromDate.Value = getFirstDateInMonth(getSystemDate)
        dtpToDate.Value = getSystemDate()
        Call vsfDet.AutoSizeCols()
        Call syncGridCols()
        Call ChangeFontSize(Me)
        Call fillFormProperties(Me)
    End Sub

    Private Sub syncGridCols()
        Dim i As Integer
        With vsfDet
            vsfSearch.Cols.Count = vsfDet.Cols.Count
            For i = 0 To .Cols.Count - 1
                vsfSearch.Cols(i).Width = vsfDet.Cols(i).Width
                vsfSearch.Cols(i).Visible = vsfDet.Cols(i).Visible
            Next i
        End With
    End Sub

    Private Function getDocQry(ByVal argFld As String, Optional ByVal argValidate As Boolean = False)

        strSQL = "Select t." & argFld
        strSQL &= " From UtlTransactions t, MstMenu m"
        strSQL &= " Where t.DocID = m.DocId"
        strSQL &= " And t.DocID in (Select distinct  DocID from UtlTransCum)"
        If argValidate Then
            strSQL &= " And DocName='" & cboDocName.Text & "'"
        End If
        If gUserCode <> "ADMIN" Then
            If gblnUserBased Then
                strSQL &= " and MenuItemID in (Select MenuItemID from MstUserRights where substring(UserRights,2,1)=1 "
                strSQL &= " and UserID = " & gUserID & ")"
            Else
                strSQL &= " and MenuItemID in (Select MenuItemID from MstRoleRights where substring(RoleRights,2,1)=1 "
                strSQL &= " and RoleID in (Select RoleID from MstUser where UserID = " & gUserID & "))"
            End If
        End If
        getDocQry = strSQL
    End Function

    Private Sub chkDate_CheckedChanged(sender As Object, e As EventArgs) Handles chkDate.CheckedChanged
        If chkDate.Checked = True Then
            pnlDate.Enabled = True
        Else
            pnlDate.Enabled = False
        End If
    End Sub

    Private Sub vsfDet_BeforeEdit(sender As Object, e As RowColEventArgs) Handles vsfDet.BeforeEdit
        e.Cancel = True
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Call frmMain.CloseSelectedTabPage(Me)
    End Sub

    Private Sub btnLoadDocNo_Click(sender As Object, e As EventArgs) Handles btnLoadDocNo.Click
        If cboDocName.Text <> "" Then
            strSQL = "Select " & strDocIDFldName & "," & strDocNoFldName & "," & strFldPartyCode & " from " & strDocHdrTblName & " h," & strPartyTblName & " p "
            strSQL = strSQL & " where h." & strFldPartyID & " = p." & strFldPartyID
            strSQL = strSQL & " and Cancelled='N' and h.Authorised='Y'"
            strSQL = strSQL & " and " & getDateCond(strDocDateFldName)
            Dim frmDisplay As New frmUtlSingleSel
            With frmDisplay
                .FillViewVsf(Me, strSQL, cboDocNo, 1, 0)
                .ShowDialog()
            End With
        End If
    End Sub

    Private Sub btnLoad_Click(sender As Object, e As EventArgs) Handles btnLoad.Click
        If cboDocName.Text = "" And cboDocNo.Text = "" Then Exit Sub

        If dtpFromDate.Value > dtpToDate.Value Then
            MsgBox("From date Should Be less than To Date!", vbInformation, Me.Text)
            dtpFromDate.Focus()
            Exit Sub
        End If

        Dim strScrollBarWidth As Integer
        dtCumQty = clsSVT.getDATATable("Select LinkDocID, CumQtyFld from UtlTransCum  Where DocID = " & intDocId)
        vsfDet.DataSource = Nothing
        Call getUtlLinkedTrnData()
        strSQL = getPendQry(intDocId)

        strSQL = getItemSearchGridQuery(strSQL)
        Call showWaitCursor()
        dtSearch = clsSVT.getDATATable(strSQL)
        dvSearch = New DataView(dtSearch)
        vsfDet.DataSource = dvSearch
        vsfSearch.Cols.Count = vsfDet.Cols.Count
        Call vsfDet.AutoSizeCols()
        vsfDet.Cols(0).Visible = False
        vsfSearch.Cols(0).Visible = False
        strScrollBarWidth = SystemInformation.VerticalScrollBarWidth
        vsfSearch.Cols(vsfSearch.Cols.Count - 1).Width = vsfDet.Cols(vsfDet.Cols.Count - 1).Width + strScrollBarWidth
        vsfDet.AllowMerging = AllowMergingEnum.Free
        vsfDet.Cols(0).AllowMerging = True
        vsfDet.Cols(1).AllowMerging = True
        vsfDet.Cols(2).AllowMerging = True
        vsfDet.Cols(3).AllowMerging = True
        For i = 0 To vsfDet.Cols.Count - 1
            If UCase(vsfDet.GetData(0, i)) = "SHORTCLOSED" Then
                vsfDet.Cols(i).DataType = GetType(Boolean)
                Exit For
            End If
            If UCase(vsfDet.GetData(0, i)) = UCase(strIMstFldID) Then
                vsfDet.Cols(i).Visible = False
            End If
        Next
        Call syncGridCols()
        Call showNormalCursor()
    End Sub

    Private Sub btnExport_Click(sender As Object, e As EventArgs) Handles btnExport.Click
        If vsfDet.Rows.Count <= 1 Then Exit Sub
        Call ExportToXL(vsfDet)
    End Sub

    Private Sub cboDocNo_Validating(sender As Object, e As CancelEventArgs) Handles cboDocNo.Validating
        Dim blnFound As Boolean
        If Trim(cboDocNo.Text) <> "" Then
            clsSVT.getDataLongValue("Select " & strDocIDFldName & " from " & strDocHdrTblName & " where Cancelled='N' and Authorised='Y' and " & strDocNoFldName & " = '" & Trim(cboDocNo.Text) & "'", 0, blnFound)
            If Not blnFound Then
                MsgBox("Not a valid " & strDocNoFldName & "! ", MsgBoxStyle.Critical, Me.Text)
                cboDocNo.SelectAll()
                e.Cancel = True
                cboDocNo.Focus()
                Exit Sub
            End If
        End If
    End Sub

    Private Sub cboDocName_Validating(sender As Object, e As CancelEventArgs) Handles cboDocName.Validating
        Dim blnFound As Boolean
        If Trim(cboDocName.Text) <> "" Then
            intDocId = clsSVT.getDataLongValue(getDocQry("DocID", True), 0, blnFound)
            If Not blnFound Then
                MsgBox("Not a valid Document!", MsgBoxStyle.Critical, Me.Text)
                cboDocName.SelectAll()
                e.Cancel = True
                cboDocName.Focus()
                Exit Sub
            Else
                If intDocId > 0 Then
                    Call getUtlTrnData(intDocId)
                    strSQL = "Select Distinct " & strDocNoFldName & " from " & strDocHdrTblName & " where Cancelled='N' and Authorised='Y' order by " & strDocNoFldName
                    Call bindCombo(cboDocNo, strDocNoFldName, strDocNoFldName, strSQL)
                    cboDocNo.Text = ""
                End If
            End If
        End If
    End Sub

    Private Sub cboDocNo_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cboDocNo.KeyPress
        If e.KeyChar = "'" Then
            e.Handled = True
        End If
    End Sub

    Private Sub cboDocName_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cboDocName.KeyPress
        If e.KeyChar = "'" Then
            e.Handled = True
        End If
    End Sub

    Private Sub frmUtlShortclose_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        cboDocName.Focus()
    End Sub

    Private Sub getUtlTrnData(ByVal argDocID As Integer)
        Dim rsDummy As oledb.oledbDataReader
        rsDummy = clsSVT.getDataReaderFromSQL("Select * from UtlTransactions where DocID = " & argDocID)

        With rsDummy
            If .HasRows Then
                .Read()
                strFrmCaption = GetVal(!DocName, "")
                strSHForm = GetVal(!DocShForm, "")
                strDocNoFldName = strSHForm & "No"
                strDocIDFldName = strSHForm & "ID"
                strQtyFldName = strSHForm & "Qty"
                strDocDateFldName = strSHForm & "Date"
                strDocHdrTblName = "TrnHdr" & strSHForm
                strDocDtlTblName = "TrnDtl" & strSHForm

                intPartyDocID = GetVal(!MstDocIDParty, 0)

                If intPartyDocID > 0 Then
                    Dim strPartyShForm As String
                    strPartyShForm = clsSVT.getDataStringValue("Select DocShForm From UtlTransactions where DocID = " & Val(intPartyDocID), "", False)
                    strPartyType = strPartyShForm
                    strPartyTblName = "Mst" & strPartyShForm
                    strFldPartyID = strPartyShForm & "ID"
                    strFldPartyCode = strPartyShForm & "Code"
                    strFldPartyName = strPartyShForm & "Name"
                    Select Case strPartyShForm
                        Case "CUST"
                            strPartyFlds = gCustQryFlds
                        Case "SUPP"
                            strPartyFlds = gSuppQryFlds
                    End Select
                    chkSelParty.Visible = True
                Else
                    chkSelParty.Checked = False
                    chkSelParty.Visible = False
                End If
                intItemDocID = GetVal(!MstDocIDItem, 0)
                If intItemDocID > 0 Then
                    strItemShForm = clsSVT.getDataStringValue("Select DocShForm From UtlTransactions where DocID = " & Val(intItemDocID), "", False)
                    strItemTblName = "Mst" & strItemShForm
                    strFldItemID = strItemShForm & "ID"
                    strFldItemSlno = strItemShForm & "Slno"
                    strFldItemCode = strItemShForm & "Code"
                    strFldItemName = strItemShForm & "Name"
                    chkSelItemCode.Visible = True
                Else
                    chkSelItemCode.Checked = False
                    chkSelItemCode.Visible = False
                End If

            End If
            .Close()
        End With
    End Sub

    Private Sub getUtlLinkedTrnData()
        Dim rsDummy As oledb.oledbDataReader

        strSQL = "Select * from UtlTransactions where LinkedFromDocShForm = '" & strSHForm & "'"
        rsDummy = clsSVT.getDataReaderFromSQL(strSQL)

        With rsDummy
            If .HasRows Then
                .Read()
                strCurDocShForm = GetVal(!DocShForm, "")

                strSrcDocShForm = GetVal(!LinkedFROMDocShForm, "")

                Me.Text = "Select Pending " & rsDummy!DocName
                strLinkedFromHdrTblName = "TrnHdr" & GetVal(!LinkedFROMDocShForm, "")
                strLinkedFromDtlTblName = "TrnDtl" & GetVal(!LinkedFROMDocShForm, "")
                strLinkedFromDocNoFldName = GetVal(!LinkedFROMDocShForm, "") & "No"
                strLinkedFromDocQtyFldName = GetVal(!LinkedFROMDocShForm, "") & "Qty"
                strLinkedFromDocRateFldName = GetVal(!LinkedFROMDocShForm, "") & "Rate"
                strLinkedFromDocDateFldName = GetVal(!LinkedFROMDocShForm, "") & "Date"

                strPendingQryFlds = GetVal(!PendingQryFlds, "")
                strPendingQryTbls = GetVal(!PendingQryTbls, "")
                strPendingQryCond = GetVal(!PendingQryCond, "")

                'intLinkPartyDocID = GetVal(!MstDocIDParty, 0)
                intLinkPartyDocID = clsSVT.getDataLongValue("Select MstDocIDParty from UtlTransactions where DocShForm = '" & strSrcDocShForm & "'", 0, False)
                If intLinkPartyDocID > 0 Then
                    strPartyShForm = clsSVT.getDataStringValue("Select DocShForm From UtlTransactions where DocID = " & intLinkPartyDocID, "", False)
                    strPMstTblName = "Mst" & strPartyShForm
                    strPMstFldID = strPartyShForm & "ID"
                    strPMstFldCode = strPartyShForm & "Code"
                    strPMstFldName = strPartyShForm & "Name"
                End If

                intLinkItemDocID = GetVal(!MstDocIDItem, 0)
                    strItemShForm = clsSVT.getDataStringValue("Select DocShForm From UtlTransactions where DocID = " & intLinkItemDocID, "", False)
                    strIMstTblName = "Mst" & strItemShForm
                    strIMstFldID = strItemShForm & "ID"
                    strIMstFldCode = strItemShForm & "Code"
                    strIMstFldName = strItemShForm & "Name"

                    If strItemShForm <> "" Then blnItemPresent = True
                    If strPartyShForm <> "" Then blnPartyPresent = True

                    blnPendParty = IIf(GetVal(!LinkedPendingParty, "N") = "Y", True, False)
                    blnPendDocNo = IIf(GetVal(!LinkedPendingDocNo, "N") = "Y", True, False)
                    blnPendSingleDoc = IIf(GetVal(!LinkedPendingSingleDoc, "N") = "Y", True, False)

                    Dim strAuth As String
                    strAuth = clsSVT.getDataStringValue("Select AuthReq from UtlTransactions where DocShForm = '" & strSrcDocShForm & "'", "", False)
                    If strAuth = "Y" Then
                        blnAuthReq = True
                    End If

                    If UCase(GetVal(!AffectsStock, "")) = "ADD" Or UCase(GetVal(!AffectsStock, "")) = "REDUCE" Then
                        blnQOH = True
                    End If

                End If
                .Close()
        End With
    End Sub

    Private Function getPendQry(argDocID As Long, Optional ByVal argTrnID As Integer = 0) As String
        Dim strSQL As String
        strSQL = "select d." & strSrcDocShForm & "ID, "
        'If Pending Doc No wise then
        If intLinkPartyDocID > 0 Then
            If blnPartyPresent Then
                strSQL &= strPMstFldCode & "," & strPMstFldName & ","
            Else
                strSQL &= " PartyName as " & strPMstFldCode & ",PartyName as " & strPMstFldName & ","
            End If
        End If

        'Fixed Fields DocNo, DocID, DocDate, ItemSlno
        strSQL &= strSrcDocShForm & "No, " & strSrcDocShForm & "Date, d.ItemSlno,"


        'If Item Present then ItemID, ItemCode, ItemName, ItemGroupCode, UOMCode
        'Else All column created with Empty Value
        If blnItemPresent Then
            strSQL &= " d." & strIMstFldID & ", " & strIMstFldCode & ", " & strIMstFldName & ", ItemGroupCode, UOMCode, "
        Else
            strSQL &= " 0 as " & strIMstFldID & ", '' as " & strIMstFldCode & ", '' as " & strIMstFldName & ", '' as ItemGroupCode, '' as UOMCode, "
        End If

        'Fixed Fields LinkDocQty, CumQty, PendQty
        strSQL &= " d." & strSrcDocShForm & "Qty as " & strSrcDocShForm & "Qty,"
        strSQL &= strCurDocShForm & "CumQty, "
        strSQL &= "d." & strSrcDocShForm & "Qty "
        If rbPending.Checked Then
            strSQL &= "- IsNull(ShortClosedQty,0) "
        End If
        strSQL &= "- ISNULL(" & strCurDocShForm & "CumQty,0) + "
        If argDocID > 0 Then
            strSQL &= "isnull((select " & strCurDocShForm & "QTY from TrnDtl" & strCurDocShForm & " id where id." & strSrcDocShForm & "ID = d." & strSrcDocShForm & "ID and id.ItemSlno=d.ItemSlno and id." & strCurDocShForm & "ID = " & argDocID
            If blnItemPresent Then
                strSQL &= " and id." & strIMstFldID & " = d." & strIMstFldID
            End If
            strSQL &= "),0) "
        Else
            strSQL &= "0"
        End If
        strSQL &= " as PendQty, "

        'If Item Present then take Conversion Factor
        If blnItemPresent Then
            strSQL &= " ConvFact, "
        Else
            strSQL &= " 1 as ConvFact, "
        End If

        'If Stock Affects then reterive QOH values
        If blnQOH Then
            strSQL &= " IsNUll((Select sum(CLBal) from TrnQOH where TrnQOH.ItemID=d.ItemID),0) + "
            If argDocID > 0 Then
                strSQL &= "isnull((select " & strCurDocShForm & "QTY from TrnDtl" & strCurDocShForm & " id where id." & strSrcDocShForm & "ID = d." & strSrcDocShForm & "ID and id.ItemSlno=d.ItemSlno and id." & strCurDocShForm & "ID = " & argDocID
                If blnItemPresent Then
                    strSQL &= " and id." & strIMstFldID & " = d." & strIMstFldID
                End If
                strSQL &= "),0) "
            Else
                strSQL &= " 0 "
            End If
        Else
            strSQL &= " 0 "
        End If
        strSQL &= " as QOH, "

        'Fixed Columns DocQty
        strSQL &= " 0 as DocQty"

        'Key Combination
        'If blnPendParty OrElse blnPendSingleDoc Then
        'strSQL &= " Convert(varchar,d." & strSrcDocShForm & "ID) + '|' + convert(varchar,d.ItemSlno) "
        'End If

        'If blnPendDocNo Then
        '    If blnPartyPresent Then
        '        strSQL &= strPMstFldCode & " + '|' + Convert(varchar, d." & strSrcDocShForm & "ID) + '|' convert(varchar,d.ItemSlno) "
        '    Else
        '        strSQL &= "PartyName + '|' + Convert(varchar, d." & strSrcDocShForm & "ID) + '|' convert(varchar,d.ItemSlno) "
        '    End If
        'End If

        'If blnItemPresent Then
        '    strSQL &= " + '|' + Convert(varchar,d." & strIMstFldID & ") "
        'End If

        'strSQL &= " as ItemRowID"

        'If Additional fields available in utltransactions table
        If strPendingQryFlds <> "" Then
            strSQL &= ", " & strPendingQryFlds
        End If


        strSQL &= ", case when d.Shortclosed = 'Y' then 'True'  else 'false' end  [Shortclosed],  "
        If clsSVT.getDataStringValue("select UPPER(Data_Type) from information_schema.columns  where table_name='" & strLinkedFromDtlTblName & "' and column_Name='ShortclosedBy'", "", False) = "INT" Then
            strSQL &= " isnull((Select UserCode from MstUser where MstUser.UserID=d.ShortclosedBy),'') [ShortClosed By],"
        Else
            strSQL &= " isnull((Select UserCode from MstUser where MstUser.UserCode=d.ShortclosedBy),'') [ShortClosed By],"
        End If
        strSQL &= "  d.ShortClosedDT [ShortClosed Date], d.ShortClosedQty [Shortclosed Qty]"


        'Fixed Tables 
        strSQL &= " from "
        strSQL &= strLinkedFromHdrTblName & " h," & strLinkedFromDtlTblName & " d "

        'If Item Present take Item Master, UOM Master, Item wise UOM Master table
        If blnItemPresent Then
            strSQL &= ", " & strIMstTblName & " i, MstItemGroup g, MstUOM u, MstItemUOM iu"
        End If

        'If party Present
        If blnPartyPresent Then
            strSQL &= "," & strPMstTblName & " p"
        End If

        'If Additional Tables available in utltransactions table
        If strPendingQryTbls <> "" Then
            strSQL &= "," & strPendingQryTbls
        End If

        'Fixed Where Condition
        strSQL &= " where h." & strSrcDocShForm & "ID = d." & strSrcDocShForm & "ID"

        'If additional condition available in utltransactions table
        If strPendingQryCond <> "" Then
            strSQL &= " and " & strPendingQryCond
        End If

        'If item Present
        If blnItemPresent Then
            strSQL &= " and i.ItemGroupID = g.ItemGroupID"
            strSQL &= " and d.UOMId = u.UOMId"
            strSQL &= " and d." & strIMstFldID & " = i." & strIMstFldID
            strSQL &= " and d." & strIMstFldID & " = iu." & strIMstFldID
            strSQL &= " and u.UOMID = iu.UOMID"
        End If

        'If Party Present
        If blnPartyPresent Then
            strSQL &= " and h." & strPMstFldID & " = p." & strPMstFldID
            If strPartyList <> "" Then strSQL &= " and h." & strPMstFldID & " in (" & strPartyList & ")"
        End If


        If rbShortClosed.Checked = True Then
            strSQL &= " and d.Shortclosed='Y'"
        ElseIf rbPending.Checked = True Then
            'Fixed condition
            strSQL &= " and d." & strSrcDocShForm & "Qty - IsNull(ShortClosedQty,0) - ISNULL(" & strCurDocShForm & "CumQty,0) + "
            If argDocID > 0 Then
                strSQL &= "isnull((select " & strCurDocShForm & "QTY from TrnDtl" & strCurDocShForm & " id where id." & strSrcDocShForm & "ID = d." & strSrcDocShForm & "ID and id.ItemSlno=d.ItemSlno and id." & strCurDocShForm & "ID = " & argDocID
                If blnItemPresent Then
                    strSQL &= " and id." & strIMstFldID & " = d." & strIMstFldID
                End If
                strSQL &= "),0) "
            Else
                strSQL &= "0"
            End If
            strSQL &= " > 0 "
            strSQL &= " and d.Shortclosed='N'"
        Else
            'Fixed condition
            strSQL &= " and ISNULL(d." & strSrcDocShForm & "Qty,0) - ISNULL(" & strCurDocShForm & "CumQty,0) + "
            If argDocID > 0 Then
                strSQL &= "isnull((select " & strCurDocShForm & "QTY from TrnDtl" & strCurDocShForm & " id where id." & strSrcDocShForm & "ID = d." & strSrcDocShForm & "ID and id.ItemSlno=d.ItemSlno and id." & strCurDocShForm & "ID = " & argDocID
                If blnItemPresent Then
                    strSQL &= " and id." & strIMstFldID & " = d." & strIMstFldID
                End If
                strSQL &= "),0) "
            Else
                strSQL &= "0"
            End If
            strSQL &= " > 0 "
        End If

        If cboDocNo.Text <> "" Then
            strSQL &= " and h." & strSrcDocShForm & "No = '" & cboDocNo.Text & "'"
        End If

        If argTrnID > 0 Then
            strSQL &= " and h." & strSrcDocShForm & "ID = " & argTrnID
        End If

        If blnItemPresent Then
            If strItemIDList <> "" Then
                strSQL &= " and i." & strIMstFldID & " in (" & strItemIDList & ")"
            End If
        End If


        If chkDate.Checked = True Then
            strSQL &= " and h." & strSrcDocShForm & "Date >= " & getDateFormatInCond(dtpFromDate.Value)
            strSQL &= " and h." & strSrcDocShForm & "Date <= " & getDateFormatInCond(dtpToDate.Value)
        End If

        strSQL &= " order by " & strSrcDocShForm & "Date, " & strSrcDocShForm & "No"

        If blnItemPresent Then
            strSQL &= ", ItemCode, ItemName"
        End If
        strSQL = Replace(strSQL, "False", "")

        getPendQry = strSQL

    End Function

    Private Sub vsfDet_AfterResizeColumn(sender As Object, e As RowColEventArgs) Handles vsfDet.AfterResizeColumn
        Call syncGridCols()
    End Sub

    Private Sub vsfDet_AfterScroll(sender As Object, e As RangeEventArgs) Handles vsfDet.AfterScroll
        vsfSearch.ScrollPosition = vsfDet.ScrollPosition
    End Sub

    Private Sub vsfSearch_AfterScroll(sender As Object, e As RangeEventArgs) Handles vsfSearch.AfterScroll
        Dim intv As Integer = vsfSearch.ScrollPosition.X
        Dim inth As Integer = vsfDet.ScrollPosition.Y
        vsfDet.ScrollPosition = New System.Drawing.Point(intv, inth)
    End Sub

    Private Sub vsfSearch_ChangeEdit(sender As Object, e As EventArgs) Handles vsfSearch.ChangeEdit
        Call applyFilterFromGrid()
    End Sub

    Private Function applyFilterFromGrid() As Boolean
        Dim StrQry As String, strValue As String
        Dim i As Integer
        applyFilterFromGrid = False
        Try
            StrQry = ""
            With vsfSearch
                For i = 0 To .Cols.Count - 1
                    strValue = ""
                    If i = .ColSel Then
                        If .Editor.Text <> "" Then
                            strValue = Trim(.Editor.Text)
                        End If
                    Else
                        If .GetData(0, i) <> "" Then
                            strValue = Trim(.GetData(0, i))
                        End If
                    End If
                    If strValue <> "" Then
                        If StrQry <> "" Then StrQry = StrQry & " and "
                        Select Case gDBTypeCurrent
                            Case gDbTypeAccess
                                StrQry = StrQry & vsfDet.GetData(0, i) & " like '*" & strValue & "*'"
                            Case Else
                                'If vsfDet.Cols(i).DataType = GetType(Decimal) Or vsfDet.Cols(i).DataType = GetType(Integer) Then
                                '    Try
                                '        Dim strTest = Convert.ToDecimal(strValue)
                                '        StrQry = StrQry & vsfDet.GetData(3, i) & " >= " & strValue
                                '    Catch ex As Exception
                                '        ' vsfSearch.SetData(0, i, "")
                                '        dv.RowFilter = "1 <> 1"
                                '        Exit Function
                                '    End Try
                                If vsfDet.Cols(i).DataType = GetType(Decimal) Or vsfDet.Cols(i).DataType = GetType(Integer) Or vsfDet.Cols(i).DataType = GetType(Date) Then
                                    Try
                                        StrQry = StrQry & "Convert(" & vsfDet.GetData(0, i) & ",'System.String')" & " like '" & strValue & "*'"
                                    Catch ex As Exception
                                        dvSearch.RowFilter = "1 <> 1"
                                        Exit Function
                                    End Try
                                Else
                                    StrQry = StrQry & vsfDet.GetData(0, i) & " like '%" & strValue & "%'"
                                End If
                        End Select
                    End If
                Next i
            End With
            'If StrQry <> "" Then
            '    vsfDet.DataSource = dt.Select(StrQry)
            'Else
            '    vsfDet.DataSource = dt
            'End If
            dvSearch.RowFilter = StrQry
            Call syncGridCols()
            applyFilterFromGrid = True
        Catch ex As Exception

        End Try

    End Function

    Private Sub vsfSearch_KeyUp(sender As Object, e As KeyEventArgs) Handles vsfSearch.KeyUp
        If e.KeyCode = Keys.Down Then
            vsfDet.Row = 5
            vsfDet.Col = vsfSearch.Col
            vsfDet.Focus()
        End If
    End Sub

    Private Sub vsfDet_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles vsfDet.MouseDoubleClick
        If vsfDet.Rows.Count <= 1 Then Exit Sub
        Dim ht = vsfDet.HitTest
        If ht.Row = -1 Or ht.Row = 0 Then Exit Sub
        pnlHdr.Enabled = False

        intTrnID = vsfDet.GetData(vsfDet.Row, 0)

        Dim strScrollBarWidth As Integer
        vsfShort.DataSource = Nothing
        strSQL = getPendQry(intDocId, intTrnID)
        Call showWaitCursor()
        dtSearch = clsSVT.getDATATable(strSQL)
        dvSearch = New DataView(dtSearch)
        vsfShort.DataSource = dvSearch
        'vsfSrch.Cols.Count = vsfShort.Cols.Count
        Call vsfShort.AutoSizeCols()
        'Call syncGridCols()
        strScrollBarWidth = SystemInformation.VerticalScrollBarWidth
        'vsfSrch.Cols(vsfSearch.Cols.Count - 1).Width = vsfShort.Cols(vsfShort.Cols.Count - 1).Width + strScrollBarWidth
        vsfShort.Cols(0).AllowMerging = True
        vsfShort.Cols(1).AllowMerging = True
        vsfShort.Cols(2).AllowMerging = True
        vsfShort.Cols(0).Visible = False
        pnlEdit.Visible = True
        For i = 0 To vsfShort.Cols.Count - 1
            If UCase(vsfShort.GetData(0, i)) = "SHORTCLOSED" Then
                vsfShort.Cols(i).DataType = GetType(Boolean)
                intColShortclosed = i
            ElseIf UCase(vsfShort.GetData(0, i)) = "SHORTCLOSED BY" Then
                intcolShortclosedBy = i
            ElseIf UCase(vsfShort.GetData(0, i)) = "SHORTCLOSED DATE" Then
                intColShortclosedDt = i
                vsfShort.Cols(i).DataType = GetType(String)
            ElseIf UCase(vsfShort.GetData(0, i)) = "SHORTCLOSED QTY" Then
                intColShortclosedQty = i
            ElseIf UCase(vsfShort.GetData(0, i)) = UCase(strIMstFldID) Then
                intColItemID = i
                vsfShort.Cols(i).Visible = False
            ElseIf UCase(vsfShort.GetData(0, i)) = "ITEMSLNO" Then
                intColItemSlno = i
            ElseIf UCase(vsfShort.GetData(0, i)) = "PENDQTY" Then
                intColPendQty = i
            End If
        Next
        Call showNormalCursor()
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        pnlEdit.Visible = False
        pnlHdr.Enabled = True
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        If vsfShort.Rows.Count <= 1 Then Exit Sub
        Dim svtTrn As oledb.oledbTransaction
        Call showWaitCursor()

        With vsfShort
            For i = 1 To .Rows.Count - 1
                If .GetCellCheck(i, intColShortclosed) = CheckEnum.Checked Then
                    If GetVal(.GetData(i, intColShortclosedQty), 0) > GetVal(.GetData(i, intColPendQty), 0) Then
                        MsgBox("Short closed Qty should not be greater than pending Qty : " & GetVal(.GetData(.Row, intColPendQty), 0) & " ", MsgBoxStyle.Critical, "Short Close Document")
                        .Col = intColShortclosedQty
                        .Row = i
                        Exit Sub
                    End If
                    If Val(.GetData(i, intColShortclosedQty)) <= 0 Then
                        MsgBox("Qty should not be less than 0 ", MsgBoxStyle.Critical, "Short Close Documents")
                        .Col = intColShortclosedQty
                        .Row = i
                        Exit Sub
                    End If
                End If
            Next
        End With
        svtTrn = svtConn.BeginTransaction(IsolationLevel.ReadCommitted)
        Try
            With vsfShort
                For i = 1 To .Rows.Count - 1
                    strSQL = "Update " & strLinkedFromDtlTblName & " set "
                    If .GetCellCheck(i, intColShortclosed) = CheckEnum.Checked Then
                        strSQL &= " ShortClosed = 'Y',"
                        If clsSVT.getDataStringValue("select UPPER(Data_Type) from information_schema.columns  where table_name='" & strLinkedFromDtlTblName & "' and column_Name='ShortclosedBy'", "", False, svtTrn) = "INT" Then
                            strSQL &= " ShortClosedBy = " & clsSVT.getDataLongValue("Select UserID from MstUser where UserCode='" & Trim(.GetData(i, intcolShortclosedBy)) & "'", 0, False, svtTrn) & ","
                        Else
                            strSQL &= " ShortClosedBy = '" & gUserCode & "',"
                        End If
                        strSQL &= " ShortClosedDt = " & getDateFormatInCond(CDate(.GetData(i, intColShortclosedDt))) & ","
                        strSQL &= " ShortClosedQty = " & Val(.GetData(i, intColShortclosedQty))
                    Else
                        strSQL &= " ShortClosed = 'N',"
                        If clsSVT.getDataStringValue("select UPPER(Data_Type) from information_schema.columns  where table_name='" & strLinkedFromDtlTblName & "' and column_Name='ShortclosedBy'", "", False, svtTrn) = "INT" Then
                            strSQL &= " ShortClosedBy = 0,"
                        Else
                            strSQL &= " ShortClosedBy = '',"
                        End If
                        strSQL &= " ShortClosedDt =  Null,"
                        strSQL &= " ShortClosedQty = 0"
                    End If
                    strSQL &= " where " & strSrcDocShForm & "ID = " & intTrnID
                    If blnItemPresent Then
                        strSQL &= " and " & strIMstFldID & " = " & Val(.GetData(i, intColItemID))
                    End If
                    strSQL &= " and ItemSlno = " & Val(.GetData(i, intColItemSlno))
                    clsSVT.execSQLStatement(strSQL, svtTrn)

                Next
            End With
            MsgBox("Updated successfully!", vbInformation, Me.Text)
            pnlEdit.Visible = False
            pnlHdr.Enabled = True
            svtTrn.Commit()
        Catch ex As Exception
            svtTrn.Rollback()
            MsgBox(ex.Message, MsgBoxStyle.Information, Me.Text)
        Finally
            Call showNormalCursor()
        End Try
        Call btnLoad_Click(sender, e)
    End Sub



    Private Sub vsfShort_BeforeEdit(sender As Object, e As RowColEventArgs) Handles vsfShort.BeforeEdit
        With vsfShort
            Select Case e.Col
                Case intColShortclosed
                Case intColShortclosedQty
                    If .GetCellCheck(.Row, intColShortclosed) = CheckEnum.Unchecked Then
                        e.Cancel = True
                    End If
                Case Else
                    e.Cancel = True
                    Exit Sub
            End Select
        End With
    End Sub

    Private Sub chkSel_CheckedChanged(sender As Object, e As EventArgs) Handles chkSel.CheckedChanged
        With vsfShort
            If .Rows.Count <= 1 Then Exit Sub
            If .Row < 1 Then Exit Sub


            For i As Int32 = 1 To .Rows.Count - 1
                If chkSel.Checked Then
                    .SetData(i, intColShortclosed, True)
                    .SetData(i, intcolShortclosedBy, gUserCode)
                    .SetData(i, intColShortclosedDt, Format(getSystemDate, "dd MMM yyyy").ToString)
                    .SetData(i, intColShortclosedQty, GetVal(.GetData(i, intColPendQty), 0))
                Else
                    .SetData(i, intColShortclosed, False)
                    .SetData(i, intcolShortclosedBy, "")
                    .SetData(i, intColShortclosedDt, "")
                    .SetData(i, intColShortclosedQty, 0)
                End If
            Next

            If chkSel.Checked Then
                chkSel.Text = "Un-Select All"
            Else
                chkSel.Text = "Select All"
            End If
        End With
    End Sub

    Private Sub btnLoadDocNo_Leave(sender As Object, e As EventArgs) Handles btnLoadDocNo.Leave
        rbShortClosed.Focus()
    End Sub

    Private Sub rbShortClosed_Leave(sender As Object, e As EventArgs) Handles rbShortClosed.Leave
        rbPending.Focus()
    End Sub

    Private Sub rbBoth_Leave(sender As Object, e As EventArgs) Handles rbBoth.Leave
        chkDate.Focus()
    End Sub

    Private Sub chkDate_Leave(sender As Object, e As EventArgs) Handles chkDate.Leave
        dtpFromDate.Focus()
    End Sub

    Private Sub dtpToDate_Leave(sender As Object, e As EventArgs) Handles dtpToDate.Leave
        chkSelItemCode.Focus()
    End Sub

    Private Sub btnClose_Leave(sender As Object, e As EventArgs) Handles btnClose.Leave
        If Not e.Equals(Keys.Enter) Then
            Call frmMain.CloseSelectedTabPage(Me)
            Me.Dispose()
        End If
        vsfSearch.Select()
    End Sub

    Private Sub vsfShort_KeyPress(sender As Object, e As KeyPressEventArgs) Handles vsfShort.KeyPress
        With vsfShort
            If .Col = intColShortclosedQty Then
                If Not KeyValidate("Integer", e.KeyChar) Then
                    e.Handled = True
                End If
            End If
        End With
    End Sub

    Private Sub vsfShort_AfterEdit(sender As Object, e As RowColEventArgs) Handles vsfShort.AfterEdit
        If vsfShort.Rows.Count <= 1 Then Exit Sub
        With vsfShort
            For i = 1 To .Rows.Count - 1
                If .GetCellCheck(i, intColShortclosed) = CheckEnum.Checked Then
                    .SetData(i, intcolShortclosedBy, gUserCode)
                    .SetData(i, intColShortclosedDt, Format(getSystemDate, "dd MMM yyyy").ToString)
                    .SetData(i, intColShortclosedQty, GetVal(.GetData(i, intColPendQty), 0))
                Else
                    .SetData(i, intcolShortclosedBy, "")
                    .SetData(i, intColShortclosedDt, "")
                    .SetData(i, intColShortclosedQty, 0)
                End If
            Next
        End With
    End Sub

    Private Sub vsfShort_ValidateEdit(sender As Object, e As ValidateEditEventArgs) Handles vsfShort.ValidateEdit
        Dim intShortCloseQty As Integer
        With vsfShort
            If e.Col = intColShortclosedQty Then
                intShortCloseQty = Val(.Editor.Text)

                If intShortCloseQty > GetVal(.GetData(.Row, intColPendQty), 0) Then
                    MsgBox("Qty cannot Less than Pending Qty: " & GetVal(.GetData(.Row, intColPendQty), 0) & "....", MsgBoxStyle.Critical, Me.Text)
                    e.Cancel = True
                    Exit Sub
                End If

                If intShortCloseQty <= 0 Then
                    MsgBox("Qty cannot Zero or negative....", MsgBoxStyle.Critical, Me.Text)
                    e.Cancel = True
                    Exit Sub
                End If

            End If
        End With
    End Sub

    Private Sub frmUtlShortclose_KeyUp(sender As Object, e As KeyEventArgs) Handles Me.KeyUp
        If e.Alt = True And e.KeyCode = Keys.E Then
            If vsfDet.Rows.Count <= 1 Then Exit Sub
            Call ExportToXL(vsfDet)
        End If
    End Sub

    Private Sub vsfShort_MouseClick(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles vsfShort.MouseClick
        Dim ht = vsfShort.HitTest
        If ht.Row > 1 And ht.Column = intColShortclosed Then
            If vsfShort.GetCellCheck(vsfShort.Row - 1, intColShortclosed) = CheckEnum.Unchecked Then
                vsfShort.SetCellCheck(ht.Row, intColShortclosed, C1.Win.C1FlexGrid.CheckEnum.Unchecked)
                Exit Sub
            End If
        End If
    End Sub

    Private Sub vsfSearch_AfterEdit(sender As Object, e As RowColEventArgs) Handles vsfSearch.AfterEdit
        If vsfDet.Rows.Count > 1 Then
            btnExport.Focus()
        End If
        If vsfSearch.Col = vsfSearch.Cols.Count - 1 Then
            With vsfDet
                .Row = 0
                .Col = 1
                Call showWaitCursor()
            End With
        End If
    End Sub

    Private Sub btnExport_Leave(sender As Object, e As EventArgs) Handles btnExport.Leave
        cboDocName.Focus()
    End Sub


End Class