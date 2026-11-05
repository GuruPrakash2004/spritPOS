
Imports System.ComponentModel


Public Class FrmUtlTrnSettings
    Dim clsSVT As New svtLibrary
    Public intDocID As Int32
    Private Sub FrmUtlTrnSettings_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Call bindCombo(CboTrnDocType, "DocName", "DocName", "Select DocName from UtlTransactions where DocType in('Transaction','Voucher') order by 1")
        Call initFormSize(Me, "UtlTrnSettings", False, 995, 624, 0, 0)
        Call ChangeFontSize(Me)
        GrpMisc.Visible = False
        btnSave.Visible = False
        btnCancel.Visible = False
        lblDays.Visible = False
        CboTrnDocType.Text = ""
        LblType.Visible = False
        CboAcctMstType.Visible = False
        LblAcctLedgerFldName.Visible = False
        TxtAcLedgerFldName.Visible = False
        LblAcVoucherValFld.Visible = False
        TxtAcVoucherValFld.Visible = False
        LblTenderChange.Visible = False
        TxtTenderChangeFld.Visible = False
        lblNoOfAuth.Visible = False
        txtNoOfAuth.Visible = False
        txtNoOfAuthQueryCond.Visible = False
    End Sub

    Public Sub FillDocDetails()
        Call clearflds()
        Dim RsTemp As oledb.oledbDataReader
        Dim strSQL As String
        '  Dim i As Integer, X As Integer

        ' If argCustID = 0 Then Exit Sub

        strSQL = " select * from utlTransactions"
        strSQL = strSQL & " where DocName  = '" & CboTrnDocType.Text & "'"
        RsTemp = clsSVT.getDataReaderFromSQL(strSQL)

        With RsTemp
            If Not .HasRows Then
                RsTemp.Close()
                Exit Sub
            End If
            If .Read() Then
                Dim colremovetabs As New Collection
                If GetVal(RsTemp("DocType"), "") = "Voucher" Then
                    TPVouchers.Enabled = True
                Else
                    TPVouchers.Enabled = False
                End If

                CboAddReduce.Text = GetVal(RsTemp("AffectsStock"), "")
                If CboAddReduce.Text = "" Then
                    chkAffectsStock.Checked = False
                Else
                    chkAffectsStock.Checked = True
                End If

                ChkLinkedPendParty.Checked = (GetVal(RsTemp.Item("LinkedPendingParty"), "") = "Y")
                ChkLinkedPendSingleDoc.Checked = (GetVal(RsTemp.Item("LinkedPendingSingleDoc"), "") = "Y")
                ChkLinkedPendDocNo.Checked = (GetVal(RsTemp.Item("LinkedPendingDocNo"), "") = "Y")
                ChkShowGraceQty.Checked = (GetVal(RsTemp.Item("ShowGraceQty"), "") = "Y")

                ChkAuthorizationReq.Checked = (GetVal(RsTemp.Item("AuthReq"), "") = "Y")
                txtNoOfAuth.Text = GetVal(RsTemp("NoOfAuth"), "")
                txtNoOfAuthQueryCond.Text = GetVal(RsTemp("NoOfAuthQueryCond"), "")

                chkLoadfrmBOM.Checked = (GetVal(RsTemp.Item("AllowLoadFromBOM"), "") = "Y")
                ChkAllowCopyFrm.Checked = (GetVal(RsTemp.Item("AllowCopyFrom"), "") = "Y")

                ChkAmmendmentReq.Checked = (GetVal(RsTemp.Item("AmmReq"), "") = "Y")
                ChkAllowTenderChange.Checked = (GetVal(RsTemp.Item("AllowTenderChange"), "") = "Y")
                TxtTenderChangeFld.Text = GetVal(RsTemp("TenderChangeFld"))
                ChkMultiDocGen.Checked = (GetVal(RsTemp.Item("MultiDocGen"), "") = "Y")

                ChkAllowEditDocDate.Checked = (GetVal(RsTemp.Item("AllowEditDocDate"), "") = "Y")
                ChkBlnEnvPrint.Checked = (GetVal(RsTemp.Item("blnEnvPrint"), "") = "Y")
                ChkFileAttachReq.Checked = (GetVal(RsTemp.Item("FileAttachReq"), "") = "Y")

                ChkOfflineCalc.Checked = (GetVal(RsTemp.Item("OfflineCalc"), "") = "Y")

                ChkAllowImportExport.Checked = (GetVal(RsTemp.Item("AllowImpExp"), "") = "Y")

                TxtPartyFilterCond.Text = GetVal(RsTemp.Item("PartyFilterCond"), "")

                'chkSavePartyData.Checked = (GetVal(RsTemp.Item("SavePartyData"), "") = "Y")
                'ChkManualPartySel.Checked = (GetVal(RsTemp.Item("AllowManualPartySel"), "") = "Y")
                'ChkShowShipAdd.Checked = (GetVal(RsTemp.Item("ShowShipAdd"), "") = "Y")
                ChkEnforcePartyItemLink.Checked = (GetVal(RsTemp.Item("FldBlnEnforcePartyItemLink"), "") = "Y")
                TxtRatePartyItem.Text = GetVal(RsTemp.Item("RateFldPartyItem"))

                TxtQuryFlds.Text = GetVal(RsTemp("PendingQryFlds"))
                txtQuryTbls.Text = GetVal(RsTemp("PendingQryTbls"))
                txtQuryCdtn.Text = GetVal(RsTemp("PendingQryCond"))
                txtPickFldsFrmDocQry.Text = GetVal(RsTemp("PickFldsFromSrcDocQry"))

                ChkAllowDuplicateItems.Checked = (GetVal(RsTemp.Item("BlnAllowDuplicateItems"), "") = "Y")
                TxtRateMstItem.Text = GetVal(RsTemp.Item("RateFldMstItem"))
                txtItemFilterCondition.Text = GetVal(RsTemp.Item("ItemFilterCond"))

                ChkFreezeDoc.Checked = (GetVal(RsTemp.Item("blnFreezeDoc"), "") = "Y")
                txtAllowEditDel.Text = CInt(RsTemp.Item("AllowEditDeleteForDays"))

                ChkAcctTrn.Checked = (GetVal(RsTemp.Item("AccTrn"), "") = "Y")
                chkPdfPrintP.Checked = (GetVal(RsTemp.Item("PDFPreview"), "") = "Y")

                CboAcctMstType.Text = GetVal(RsTemp("AccMstType"))
                TxtAcLedgerFldName.Text = GetVal(RsTemp("AcLedFld"))
                TxtAcVoucherValFld.Text = GetVal(RsTemp("AccValueFld"))

                ChkBalColReq.Checked = (GetVal(RsTemp.Item("BalanceColReq"), "") = "Y")
                ChkBillWiseFeature.Checked = (GetVal(RsTemp.Item("BillWiseFeatureReq"), "") = "Y")
                ChkChqPrinting.Checked = (GetVal(RsTemp.Item("ChqPrintReq"), "") = "Y")
                ChkChqPrinting.Checked = (GetVal(RsTemp.Item("ChqPrintReq"), "") = "Y")
                CboDefFirstRowType.Text = GetVal(RsTemp.Item("DefFirstRowAmtType"))
                RsTemp.Close()
            End If
        End With
    End Sub

    Private Sub btnLoad_Click(sender As Object, e As EventArgs)
        Call FillDocDetails()
    End Sub

    Private Function saveDetails() As Boolean
        Dim strSQL As String = ""
        Dim strValues As String = ""
        Dim strFlds As String = ""
        saveDetails = False
        Dim lngDocID As Long
        saveDetails = False

        Try
            lngDocID = clsSVT.getDataLongValue("Select DocID from UtlTransactions where DocName='" & UCase(CboTrnDocType.Text) & "'", 0, False)
            strSQL = "Update  utltransactions set "

            If chkAffectsStock.Checked Then
                strSQL = strSQL & " AffectsStock = '" & Trim(CboAddReduce.Text) & "',"
            Else
                strSQL = strSQL & " AffectsStock ='',"
            End If

            strSQL = strSQL & " LinkedPendingParty = '" & getCheckString(ChkLinkedPendParty) & "',"
            strSQL = strSQL & " LinkedPendingDocNo = '" & getCheckString(ChkLinkedPendDocNo) & "',"
            strSQL = strSQL & " LinkedPendingSingleDoc = '" & getCheckString(ChkLinkedPendSingleDoc) & "',"
            strSQL = strSQL & " ShowGraceQty = '" & getCheckString(ChkShowGraceQty) & "',"

            If ChkAuthorizationReq.Checked Then
                strSQL = strSQL & " AuthReq = 'Y',"
                strSQL = strSQL & " NoOfAuth = '" & Trim(txtNoOfAuth.Text) & "',"
                strSQL = strSQL & " NoOfAuthQueryCond = '" & Trim(txtNoOfAuthQueryCond.Text.Replace("'", "''")) & "',"
            Else
                strSQL = strSQL & " AuthReq = 'N',"
                strSQL = strSQL & " NoOfAuth = 0,"
                strSQL = strSQL & " NoOfAuthQueryCond = '',"
            End If

            strSQL = strSQL & " AllowLoadFromBOM = '" & getCheckString(chkLoadfrmBOM) & "',"
            strSQL = strSQL & " AllowCopyFrom = '" & getCheckString(ChkAllowCopyFrm) & "',"

            strSQL = strSQL & " AmmReq = '" & getCheckString(ChkAmmendmentReq) & "',"
            strSQL = strSQL & " MultiDocGen = '" & getCheckString(ChkMultiDocGen) & "',"

            If ChkAllowTenderChange.Checked Then
                strSQL = strSQL & " AllowTenderChange = 'Y',"
                strSQL = strSQL & " TenderChangeFld = '" & Trim(TxtTenderChangeFld.Text.Replace("'", "''")) & "',"
            Else
                strSQL = strSQL & " AllowTenderChange = 'N',"
                strSQL = strSQL & " TenderChangeFld = '',"
            End If

            strSQL = strSQL & " OffLineCalc = '" & getCheckString(ChkOfflineCalc) & "',"
            strSQL = strSQL & " AllowImpExp = '" & getCheckString(ChkAllowImportExport) & "',"
            strSQL = strSQL & " PartyFilterCond = '" & Trim(TxtPartyFilterCond.Text.Replace("'", "''")) & "',"
            'strSQL = strSQL & " SavePartyData = '" & getCheckString(chkSavePartyData) & "',"
            'strSQL = strSQL & " AllowManualPartySel = '" & getCheckString(ChkManualPartySel) & "',"
            'strSQL = strSQL & " ShowShipAdd = '" & getCheckString(ChkShowShipAdd) & "',"

            If ChkEnforcePartyItemLink.Checked = True Then
                strSQL = strSQL & " FldBlnEnforcePartyItemLink = 'Y',"
                strSQL = strSQL & " RateFldPartyItem = '" & Trim(TxtRatePartyItem.Text) & "',"
            Else
                strSQL = strSQL & " FldBlnEnforcePartyItemLink = 'N',"
                strSQL = strSQL & " RateFldPartyItem = '',"
            End If

            strSQL = strSQL & " PendingQryFlds = '" & Trim(TxtQuryFlds.Text.Replace("'", "''")) & "',"
            strSQL = strSQL & " PendingQryTbls = '" & Trim(txtQuryTbls.Text.Replace("'", "''")) & "',"
            strSQL = strSQL & " PendingQryCond = '" & Trim(txtQuryCdtn.Text.Replace("'", "''")) & "',"
            strSQL = strSQL & " PickFldsFromSrc = '" & Trim(txtPickFldsFrmSrc.Text.Replace("'", "''")) & "',"
            strSQL = strSQL & " PickFldsFromSrcDocQry = '" & Trim(txtPickFldsFrmDocQry.Text.Replace("'", "''")) & "',"
            strSQL = strSQL & " blnEnvPrint = '" & getCheckString(ChkBlnEnvPrint) & "',"
            strSQL = strSQL & " AllowEditDocDate = '" & getCheckString(ChkAllowEditDocDate) & "',"
            strSQL = strSQL & " BlnAllowDuplicateItems = '" & getCheckString(ChkAllowDuplicateItems) & "',"
            strSQL = strSQL & " RateFldMstItem = '" & Trim(TxtRateMstItem.Text) & "',"
            strSQL = strSQL & " ItemFilterCond = '" & Trim(txtItemFilterCondition.Text.Replace("'", "''")) & "',"

            If ChkFreezeDoc.Checked Then
                strSQL = strSQL & " blnFreezeDoc = 'Y',"
                strSQL = strSQL & " AllowEditDeleteForDays = '" & CInt(Trim(txtAllowEditDel.Text)) & "',"
            Else
                strSQL = strSQL & " blnFreezeDoc = 'N',"
                strSQL = strSQL & " AllowEditDeleteForDays = 0,"
            End If

            If ChkAcctTrn.Checked = True Then
                strSQL = strSQL & " AccTrn = 'Y',"
                strSQL = strSQL & " AccMstType = '" & Trim(CboAcctMstType.Text) & "',"
                strSQL = strSQL & " AcLedFld = '" & Trim(TxtAcLedgerFldName.Text) & "',"
                strSQL = strSQL & " AccValueFld = '" & Trim(TxtAcVoucherValFld.Text) & "',"
            Else
                strSQL = strSQL & " AccTrn = 'N',"
                strSQL = strSQL & " AccMstType = '',"
                strSQL = strSQL & " AcLedFld = '',"
                strSQL = strSQL & " AccValueFld = '',"
            End If

            If chkPdfPrintP.Checked Then
                strSQL = strSQL & " PDFPreview = 'Y',"

            Else
                strSQL = strSQL & "PDFPreview = 'N',"

            End If

            strSQL = strSQL & " BalanceColReq = '" & getCheckString(ChkBalColReq) & "',"
            strSQL = strSQL & " BillWiseFeatureReq = '" & getCheckString(ChkBillWiseFeature) & "',"
            strSQL = strSQL & " ChqPrintReq = '" & getCheckString(ChkChqPrinting) & "',"
            strSQL = strSQL & " DefFirstRowAmtType = '" & Trim(CboDefFirstRowType.Text) & "',"
            strSQL = strSQL & " FileAttachReq = '" & getCheckString(ChkFileAttachReq) & "'"

            strSQL = strSQL & " where DocID = " & lngDocID
            strSQL = Replace(Replace(strSQL, vbCrLf, ""), vbCr, "")

            If Not clsSVT.execSQLStatement(strSQL) Then
                MsgBox("Error during save", MsgBoxStyle.Critical, Me.Text)
            Else
                MsgBox("Updated successfully.", vbInformation, Me.Text)
            End If

            If ChkAmmendmentReq.Checked Then
                'Dim strHdrTblName As String =  gettable name from doc
            End If

            saveDetails = True
            StMsg("Status")
            Call FillDocDetails()
        Catch ex As Exception
            MsgBox("Error during save" & vbCrLf & ex.Message, MsgBoxStyle.Critical, Me.Text & "[saveDetails]")
        End Try
    End Function

    Private Sub clearflds()
        chkAffectsStock.Checked = False
        CboAddReduce.Text = ""
        ChkAllowCopyFrm.Checked = False
        chkLoadfrmBOM.Checked = False

        ChkAuthorizationReq.Checked = False
        txtNoOfAuth.Text = ""
        txtNoOfAuthQueryCond.Text = ""
        TxtTenderChangeFld.Text = ""
        ChkAmmendmentReq.Checked = False
        ChkMultiDocGen.Checked = False
        ChkFreezeDoc.Checked = False
        ChkAllowTenderChange.Checked = False
        ChkOfflineCalc.Checked = False
        ChkShowGraceQty.Checked = False
        ChkBlnEnvPrint.Checked = False
        '  ChkApplyAvgRate.Checked = False
        ChkLinkedPendSingleDoc.Checked = False
        ChkLinkedPendDocNo.Checked = False
        ChkLinkedPendParty.Checked = False
        ChkAllowEditDocDate.Checked = False
        ChkAllowDuplicateItems.Checked = False
        ChkFileAttachReq.Checked = False
        ChkAllowImportExport.Checked = False
        ChkManualPartySel.Checked = False
        ChkShowShipAdd.Checked = False
        chkSavePartyData.Checked = False
        ChkEnforcePartyItemLink.Checked = False
        chkPdfPrintP.Checked = False
        TxtRatePartyItem.Text = ""
        TxtPartyFilterCond.Text = ""
        TxtQuryFlds.Text = ""
        txtQuryTbls.Text = ""
        txtQuryCdtn.Text = ""
        txtPickFldsFrmSrc.Text = ""
        txtPickFldsFrmDocQry.Text = ""
        TxtRateMstItem.Text = ""
        txtItemFilterCondition.Text = ""
        txtAllowEditDel.Text = ""
        ChkAcctTrn.Text = ""
        CboAcctMstType.Text = ""
        TxtAcLedgerFldName.Text = ""
        TxtAcVoucherValFld.Text = ""
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        If CboTrnDocType.Text = "" Then Exit Sub
        Call saveDetails()
        Call clearflds()
        CboTrnDocType.Text = ""
        TCTrnsSettings.Visible = False
        btnSave.Visible = False
        btnCancel.Visible = False
        btnLoad.Enabled = True
        CboTrnDocType.Enabled = True
        btnMisc.Visible = True
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Call clearflds()
        CboTrnDocType.Text = ""
        TCTrnsSettings.Visible = False
        btnSave.Visible = False
        btnCancel.Visible = False
        btnLoad.Enabled = True
        CboTrnDocType.Enabled = True
        CboTrnDocType.Focus()
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub validFlds()
        Dim blnFound As Boolean
        clsSVT.getDataLongValue("Select DocID  from UtlTransactions where DocName='" & CboTrnDocType.Text & "'", 0, blnFound)
        If Not blnFound Then
            MsgBox("Document Not Found", vbInformation, Me.Text)
            CboTrnDocType.Focus()
        End If
    End Sub



    Private Sub CboTrnDocType_KeyPress(sender As Object, e As KeyPressEventArgs)
        If e.KeyChar = "'" Then e.KeyChar = "`"
        If Not KeyValidate("String", e.KeyChar, 100) Then
            e.Handled = True
        End If
    End Sub

    Private Sub ChkShowGraceQty_MouseEnter(sender As Object, e As EventArgs) Handles ChkShowGraceQty.MouseEnter
        showmsg(sender.NAME, sender.tag)

    End Sub

    Private Sub showmsg(ByVal ARGCTRLNAME As String, argMsg As String)
        If GetVal(argMsg, "") = "" Then
            Exit Sub
        End If
        Dim strMsg As String = argMsg
        Dim strArr() As String = strMsg.Split("▄")
        If strArr.Length >= 2 Then
            txtTitle.Text = strArr(0)
            txtContent.Text = strArr(1)
        End If

    End Sub


    Private Sub ChkAllowCopyFrm_MouseEnter(sender As Object, e As EventArgs) Handles ChkAllowCopyFrm.MouseEnter, ChkAllowDuplicateItems.MouseEnter, ChkAllowTenderChange.MouseEnter, ChkAmmendmentReq.MouseEnter, chkAffectsStock.MouseEnter, ChkAllowImportExport.MouseEnter, ChkAuthorizationReq.MouseEnter, ChkBlnEnvPrint.MouseEnter, ChkEnforcePartyItemLink.MouseEnter, ChkFileAttachReq.MouseEnter, ChkFreezeDoc.MouseEnter, ChkLinkedPendDocNo.MouseEnter, ChkLinkedPendParty.MouseEnter, ChkLinkedPendSingleDoc.MouseEnter, chkLoadfrmBOM.MouseEnter, ChkManualPartySel.MouseEnter, ChkMultiDocGen.MouseEnter, ChkOfflineCalc.MouseEnter, chkSavePartyData.MouseEnter, ChkShowGraceQty.MouseEnter, ChkShowShipAdd.MouseEnter, chkPdfPrintP.MouseEnter
        showmsg(sender.NAME, sender.tag)
    End Sub

    Private Sub FrmUtlTrnSettings_MouseEnter(sender As Object, e As EventArgs) Handles Me.MouseEnter
        txtTitle.Text = ""
        txtContent.Text = ""
    End Sub

    Private Sub btnPendingQuery_Click(sender As Object, e As EventArgs)
        GrpPendQuryFlds.Visible = True
    End Sub

    Private Sub ChkFreezeDoc_CheckedChanged(sender As Object, e As EventArgs) Handles ChkFreezeDoc.CheckedChanged
        If ChkFreezeDoc.Checked = True Then
            lblAllowEditDlt.Visible = True
            txtAllowEditDel.Visible = True
            lblDays.Visible = True
        Else

            lblAllowEditDlt.Visible = False
            txtAllowEditDel.Visible = False
            lblDays.Visible = False
        End If
    End Sub


    Private Sub btnLoad_Click_1(sender As Object, e As EventArgs) Handles btnLoad.Click
        'Dim strsql1 As String
        'strsql1 = "Select DocName from UtlTransactions where  DocType='Voucher'"
        'If CboTrnDocType.Text = strsql1 Then
        '    TabControl1.TabPages.Remove(Vouchers)
        'Else
        '    TabControl1.TabPages.Insert(5, Vouchers)
        'End If
        If CboTrnDocType.Text = String.Empty Then
            MsgBox("Please Select a Document!!", MsgBoxStyle.Information, "Transaction Settings")
        End If

        If CboTrnDocType.Text <> String.Empty Then
            TCTrnsSettings.Visible = True
            btnSave.Visible = True
            btnCancel.Visible = True
            btnMisc.Visible = False
            btnLoad.Enabled = False
            CboTrnDocType.Enabled = False
            Call FillDocDetails()
        End If
    End Sub

    Private Sub txtNoOfAuth_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtNoOfAuth.KeyPress, txtAllowEditDel.KeyPress, txtSearchTopN.KeyPress
        If Not KeyValidate("Integer", e.KeyChar) Then
            e.Handled = True
        End If
        If e.KeyChar = "'" Then e.Handled = True
    End Sub

    Private Sub CboAddReduce_KeyPress(sender As Object, e As KeyPressEventArgs) Handles CboAddReduce.KeyPress, CboTrnDocType.KeyPress, txtAllowEditDel.KeyPress, txtNoOfAuth.KeyPress, CboDefFirstRowType.KeyPress
        If e.KeyChar = "'" Then e.Handled = True
    End Sub

    Private Sub ChkAcctTrn_CheckedChanged(sender As Object, e As EventArgs) Handles ChkAcctTrn.CheckedChanged
        If ChkAcctTrn.Checked = True Then
            LblType.Visible = True
            CboAcctMstType.Visible = True
            LblAcctLedgerFldName.Visible = True
            TxtAcLedgerFldName.Visible = True
            LblAcVoucherValFld.Visible = True
            TxtAcVoucherValFld.Visible = True
        Else
            LblType.Visible = False
            CboAcctMstType.Visible = False
            LblAcctLedgerFldName.Visible = False
            TxtAcLedgerFldName.Visible = False
            LblAcVoucherValFld.Visible = False
            TxtAcVoucherValFld.Visible = False
        End If
    End Sub

    Private Sub ChkAuthorizationReq_CheckedChanged(sender As Object, e As EventArgs) Handles ChkAuthorizationReq.CheckedChanged
        If ChkAuthorizationReq.Checked = True Then
            lblNoOfAuth.Visible = True
            txtNoOfAuth.Visible = True
            txtNoOfAuthQueryCond.Visible = True
        Else
            lblNoOfAuth.Visible = False
            txtNoOfAuth.Visible = False
            txtNoOfAuthQueryCond.Visible = False
        End If
    End Sub

    Private Sub ChkAllowTenderChange_CheckedChanged(sender As Object, e As EventArgs) Handles ChkAllowTenderChange.CheckedChanged
        If ChkAllowTenderChange.Checked = True Then
            LblTenderChange.Visible = True
            TxtTenderChangeFld.Visible = True
        Else
            LblTenderChange.Visible = False
            TxtTenderChangeFld.Visible = False
        End If
    End Sub

    Private Sub ChkEnforcePartyItemLink_CheckedChanged(sender As Object, e As EventArgs) Handles ChkEnforcePartyItemLink.CheckedChanged
        If ChkEnforcePartyItemLink.Checked = True Then
            LblRatePartyItem.Visible = True
            TxtRatePartyItem.Visible = True
        Else
            LblRatePartyItem.Visible = False
            TxtRatePartyItem.Visible = False
        End If
    End Sub

    Private Sub ChkAllowDuplicateItems_CheckedChanged(sender As Object, e As EventArgs) Handles ChkAllowDuplicateItems.CheckedChanged
        If ChkAllowDuplicateItems.Checked = True Then
            lblRateMstItem.Visible = True
            TxtRateMstItem.Visible = True
            LblItemFilterCond.Visible = True
            txtItemFilterCondition.Visible = True
        Else
            lblRateMstItem.Visible = False
            TxtRateMstItem.Visible = False
            LblItemFilterCond.Visible = False
            txtItemFilterCondition.Visible = False

        End If
    End Sub

    Private Sub chkAffectsStock_CheckedChanged(sender As Object, e As EventArgs) Handles chkAffectsStock.CheckedChanged
        If chkAffectsStock.Checked = True Then
            CboAddReduce.Visible = True
        Else
            CboAddReduce.Visible = False
        End If
    End Sub

    Private Sub CboTrnDocType_Validating(sender As Object, e As CancelEventArgs) Handles CboTrnDocType.Validating
        If CboTrnDocType.Text = "" Then Exit Sub
        Dim blnFound As Boolean
        intDocID = clsSVT.getDataLongValue("Select DocID from UtlTransactions where DocName = '" & CboTrnDocType.Text & "'", 0, blnFound)
        If intDocID <= 0 Then
            MsgBox("Document not found!", vbInformation, Me.Text)
            CboTrnDocType.Text = String.Empty
            CboTrnDocType.Focus()
            Exit Sub
        End If

    End Sub

    Private Sub CboAddReduce_Validating(sender As Object, e As CancelEventArgs) Handles CboAddReduce.Validating
        Dim index As Integer = CboAddReduce.FindString(CboAddReduce.Text)

        If (index < 0) Then
            MessageBox.Show("This is not a valid Affect stock value, Please select value from the list")
        Else
            CboAddReduce.SelectedIndex = index
        End If
    End Sub

    Private Sub btnMisc_Click(sender As Object, e As EventArgs) Handles btnMisc.Click
        GrpMisc.Visible = True
        Call fillMiscDetails()
    End Sub

    Public Sub fillMiscDetails()
        Call clearflds()
        Dim RsTemp As oledb.oledbDataReader
        Dim strSQL As String
        '  Dim i As Integer, X As Integer

        ' If argCustID = 0 Then Exit Sub

        strSQL = " select * from UtlMisc"

        RsTemp = clsSVT.getDataReaderFromSQL(strSQL)

        With RsTemp
            If Not .HasRows Then
                RsTemp.Close()
                Exit Sub
            End If



            If .Read() Then

                txtItemCaption.Text = GetVal(RsTemp.Item("ItemCaption"))
                txtItemNameCaption.Text = GetVal(RsTemp.Item("ItemNameCaption"))
                txtItemDescCaption.Text = GetVal(RsTemp.Item("ItemDescCaption"))
                txtSearchTopN.Text = GetVal(RsTemp.Item("FldSearchTopN"))
                chkShowMenuIcons.Checked = (GetVal(RsTemp.Item("ShowMenuIcons"), "") = "Y")
                RsTemp.Close()
            End If
        End With


    End Sub

    Private Function SaveMiscFlds()
        Dim strSQL As String = ""
        Dim strValues As String = ""
        Dim strFlds As String = ""
        Dim svtTrn As oledb.oledbTransaction
        SaveMiscFlds = False
        'Dim lngDocID As Long
        'lngDocID = clsSVT.getDataLongValue("Select * from UtlMisc", 0, False)

        strSQL = "Update  UtlMisc set "
        strSQL = strSQL & " ItemCaption = '" & Trim(txtItemCaption.Text) & "',"
        strSQL = strSQL & " ItemNameCaption = '" & Trim(txtItemNameCaption.Text) & "',"
        strSQL = strSQL & " ItemDescCaption = '" & Trim(txtItemDescCaption.Text) & "',"
        strSQL = strSQL & " FldSearchTopN = '" & Trim(txtSearchTopN.Text) & "',"
        strSQL = strSQL & " ShowMenuIcons = '" & getCheckString(chkShowMenuIcons) & "'"


        strSQL = Replace(Replace(strSQL, vbCrLf, ""), vbCr, "")
        SaveMiscFlds = False
        svtTrn = svtConn.BeginTransaction(IsolationLevel.ReadCommitted)
        Try
            If Not clsSVT.execSQLStatement(strSQL, svtTrn) Then

                MsgBox("Error during save", MsgBoxStyle.Critical, Me.Text)
            Else
                MsgBox("Updated successfully.", vbInformation, Me.Text)

            End If
            gItemCode = txtItemCaption.Text
            gItemName = txtItemNameCaption.Text
            gItemDesc = txtItemDescCaption.Text
            gIntTopNrows = txtSearchTopN.Text
            '  End If
            svtTrn.Commit()

            SaveMiscFlds = True
            StMsg("Status")

            Call FillDocDetails()
        Catch ex As Exception
            MsgBox("Error during save" & vbCrLf & ex.Message, MsgBoxStyle.Critical, Me.Text & "[saveDetails]")
            svtTrn.Rollback()
        End Try
    End Function

    Private Sub clearMiscFlds()
        txtItemCaption.Text = ""
        txtItemNameCaption.Text = ""
        txtItemDescCaption.Text = ""
        txtSearchTopN.Text = ""
        chkShowMenuIcons.Checked = False
    End Sub



    Private Sub BtnCancelMisc_Click(sender As Object, e As EventArgs) Handles BtnCancelMisc.Click
        GrpMisc.Visible = False
    End Sub



    Private Sub BtnSaveMisc_Click(sender As Object, e As EventArgs) Handles BtnSaveMisc.Click
        If SaveMiscFlds() Then
            Call clearMiscFlds()
            GrpMisc.Visible = False
        End If



    End Sub

    Private Sub txtItemCaption_KeyUp(sender As Object, e As KeyEventArgs) Handles txtItemCaption.KeyUp
        txtItemCaption.Text = Replace(txtItemCaption.Text, "'", "`")
    End Sub

    Private Sub txtItemDescCaption_KeyUp(sender As Object, e As KeyEventArgs) Handles txtItemDescCaption.KeyUp
        txtItemDescCaption.Text = Replace(txtItemDescCaption.Text, "'", "`")
    End Sub

    Private Sub txtItemNameCaption_KeyUp(sender As Object, e As KeyEventArgs) Handles txtItemNameCaption.KeyUp
        txtItemNameCaption.Text = Replace(txtItemNameCaption.Text, "'", "`")
    End Sub



    Private Sub CboTrnDocType_KeyUp(sender As Object, e As KeyEventArgs) Handles CboTrnDocType.KeyUp
        If e.KeyCode = Keys.Return Then


            If CboTrnDocType.Text = String.Empty Then
                MsgBox("Please Select a Document!!", MsgBoxStyle.Information, "Transaction Settings")
            End If

            If CboTrnDocType.Text <> String.Empty Then
                TCTrnsSettings.Visible = True
                btnSave.Visible = True
                btnCancel.Visible = True
                btnMisc.Visible = False
                btnLoad.Enabled = False
                CboTrnDocType.Enabled = False
                Call FillDocDetails()
            End If
        End If
    End Sub

    Private Sub chkPdfPrintP_MouseEnter(sender As Object, e As EventArgs) Handles chkPdfPrintP.MouseEnter
        showmsg(sender.NAME, sender.tag)
    End Sub
End Class