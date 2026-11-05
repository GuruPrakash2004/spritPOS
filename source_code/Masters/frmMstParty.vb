Imports System.Globalization
Imports System.IO
Imports System.Data.oledb
Imports C1.Win.C1FlexGrid
Imports System.ComponentModel
Imports System.Text.RegularExpressions
Imports System.Text


Public Class frmMstParty
    Dim svtTrn As oledb.oledbTransaction
    Public Mode As String
    Public intDocID As Integer
    Dim intMenuItemID As Integer
    Dim imagePath As String
    Dim clsSVT As New svtLibrary

    'for audit track
    Dim blnAuditTrack As Boolean = False
    Dim strAuditHdrSQL As String
    Dim strAuditDtlSQL As String

    Dim strDocType As String
    Dim strTblName As String
    Dim strIDName As String
    Dim strCodeName As String
    Dim strPartyName As String
    Dim strFormCaption As String
    Dim strFldCaption As String
    Dim blnShowFileAttach As Boolean
    Dim strRights As String
    Dim strGroupID As String
    Dim strGroupName As String
    Dim strGroupTblName As String
    Dim strPartyType As String
    Dim lngLedGrpID As Long
    Dim strFrmCRFrmCaption As String
    Dim strFrmMsg As String
    Dim strParameter As String
    Dim strFrmPfx As String
    Dim strMstPartyAccounts As String
    Dim strFldSeries As String
    Dim strFldNo As String


    Const vASCtrlName = 0
    Const vASCtrlValue = 1

    Const vCPName = 0
    Const vCPDesg = 1
    Const vCPMobNo = 2
    Const vCPeMail = 3
    Const intCOlsCP = 4

    'Consts for vsfFiles
    Const vFSlno = 0
    Const vFDesc = 1
    Const vFName = 2

    Public cboCtrlList = GetAllCtrls(Me, GetType(ComboBox))
    Dim txtCtrlList = GetAllCtrls(Me, GetType(TextBox))
    Dim rdoCtrlList = GetAllCtrls(Me, GetType(RadioButton))
    Dim chkCtrlList = GetAllCtrls(Me, GetType(CheckBox))
    Dim dtCtrlList = GetAllCtrls(Me, GetType(DateTime))
    Dim dtC1CtrlList = GetAllCtrls(Me, GetType(C1.Win.C1Input.C1DateEdit))
    Dim vsfCtrlList = GetAllCtrls(Me, GetType(C1.Win.C1FlexGrid.C1FlexGrid))

    Dim blnTimer As Boolean = False
    Public blnRestore As Boolean = False
    Public strRestoreFileName As String = ""
    Dim strRestorePath As String
    Dim blnPicSave As Boolean

    Dim intNo As Integer
    Private _right As String
    Dim blnAutoGenCode As Boolean = False
    Dim blnAuthReq As Boolean = False

    Dim dvSearch As DataView
    Dim dtParty As DataTable

    'Dim intRowTaxType As Int32

    Private Sub AddProc()
        Call clearFlds()
        Mode = "A"
        txtPartyCode.Focus()
        pnlSearch.Visible = False
        PnlCopyFrom.Visible = True
        PnlCopyFrom.BringToFront()

        txtCustID.Text = clsSVT.getDataLongValue("Select max(" & strIDName & ") + 1 from " & strTblName, 1, False).ToString
        Call enableDisableFlds(True)
        If blnAutoGenCode Then
            Call bindCombo(cboSeries, strFldSeries, strFldSeries, "select Distinct " & strFldSeries & " from " & strTblName & " where " & strCodeName & " <> '-' and " & strFldSeries & "<>'' order by " & strFldSeries)
            cboSeries.Text = ""
        End If
        cboCountry.Text = "INDIA"
        cboState.Text = "KARNATAKA"
        cboGroupLed.SelectedIndex = -1
        Call fillVsfHFTDefValues(Me)
        Call setSearchGridHt("")
        Call StMsg()
        If UCase(strTblName) = "MSTCUST" Then
            cboGroupLed.Text = "Sundry Debtors"
            cboGroupLed.Enabled = False
        ElseIf UCase(strTblName) = "MSTSUPP" Then
            cboGroupLed.Enabled = False
            cboGroupLed.Text = "Sundry Creditors"
        End If
    End Sub

    Private Sub clearFlds()
        enableDisableFlds(False)
        cboSeries.Text = ""
        txtPartyCode.Text = ""
        txtSearch.Text = ""
        txtInActiveRem.Text = ""
        txtCustID.Text = ""
        txtPartyName.Text = ""
        txtBAdd1.Text = ""
        txtBAdd2.Text = ""
        txtBAdd3.Text = ""
        txtBadd4.Text = ""
        txtPincode.Text = ""
        cboCity.Text = ""
        cboCountry.Text = ""
        cboState.Text = ""
        txtTallyAlias.Text = ""
        txtAddress.Text = ""
        txtRmks.Text = ""
        cboPartyGrp.Text = ""
        Mode = "V"
        chkInActive.Checked = False
        pnlSearch.Visible = True
        lblApproved.Visible = False
        vsfCP.Rows.Count = 1
        ' chkEnforceItemLink.Checked = False
        StMsg("Status")
        '--7 Begin OthGRID
        Call ClearAddnlGrid(Me)
        txtSearch.Focus()
        btnConPer.Enabled = False
        PnlCopyFrom.Visible = False
        btnAlert.Enabled = False
        btnAddr.Text = "Multi Address(0)"
        btnConPer.Text = "Contact Person(0)"
        PicParty.Image = Nothing
        PicParty.Visible = False
        vsfFiles.Rows.Count = 0
        btnFileAttach.Text = "File Attachment(0)"
        btnActivities.Text = "Activities (0)"
        btnAlert.Text = "Alert Message (0)"
    End Sub

    Private Sub frmMstParty_FormClosing(sender As Object, e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        If Mode = "A" Or Mode = "M" Then
            If MsgBox("Are you Sure You Want to Exit the form with out Saving!!", MsgBoxStyle.Question + MsgBoxStyle.YesNo + vbDefaultButton2, "confirmation..") = MsgBoxResult.No Then
                e.Cancel = True
                Exit Sub
            End If
        End If
    End Sub

    Private Sub frmMstParty_KeyUp(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyUp
        Dim blnFnKey As Boolean = False
        If e.KeyCode = Keys.F9 And e.Control = True Then
            'Call initFormSize(Me, strFormCaption, True, 1200, 648, 0, 0)
            Call initFormVsfDet(Me, Me.Text, vsfCP, "CP")
            Call initFormVsfDet(Me, Me.Text, vsfHdr, "HDR")
            MsgBox("Restored Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
        If e.KeyCode = Keys.F8 And e.Control = True Then
            Call SaveActFormSettings(Me, "MstParty")
            Call SaveActFormVsfDet(Me, Me.Text, "CP", vsfCP)
            Call SaveActFormVsfDet(Me, Me.Text, "HDR", vsfHdr)
            Call SaveActFormVsfDet(Me, Me.Text, "SEARCH", vsfsearch)
            MsgBox("Saved Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
        If e.KeyCode = Keys.F5 Then
            Call bindCombo(cboCity, "City", "City", "select  distinct City from MStSCC where City <>'' order by city")
            Call bindCombo(cboState, "STATE", "STATE", "select  distinct STATE from MStSCC where state <>''  order by STATE")
            Call bindCombo(cboCountry, "COUNTRY", "COUNTRY", "select  distinct COUNTRY from MStSCC where country  <> '' order by COUNTRY")
            Call bindCombo(cboPartyGrp, strFrmPfx & "GroupCode", strFrmPfx & "GroupCode", "select  distinct " & strFrmPfx & "GroupCode from " & "MSt" & strFrmPfx & "Group where " & strFrmPfx & "GroupCode  <> '' order by " & strFrmPfx & "GroupCode")
            If cboCountry.Items.Count = 0 Then
                cboCountry.DataSource = Nothing
                cboCountry.Items.Add("INDIA")
            End If
        End If

        If Mode <> "A" Then
            If e.KeyCode = Keys.A And e.Control = True Then
                Call btnFileAttach_ClickButtonArea(sender, Nothing)
            End If
        End If

        If (e.Control) Or blnFnKey Then
            Call svtBtnCtrl.btnCtrlKeyUp(sender, e)
        End If

    End Sub

    Public Sub frmMstParty_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        strRights = getUserRights(GetMenuId(Me.Text))

        txtDocID.Text = intDocID
        lblFrm.Text = Me.Text
        intDocID = GetFrmId(strFormCaption)
        'Call initScreen()
        Call setSearchGridHt("")
        initGridStyles(Me)

        KeyPreview = True
        Call initFormVsfDet(Me, Me.Text, vsfHdr, "HDR")
        'Call initFormVsfDet(Me, Me.Text, vsfSearch, "SEARCH")
        Call initFormSize(Me, Me.Text, False, 1159, 788, 0, 0)
        Call initFormVsfDet(Me, Me.Text, vsfCP, "CP")
        UpdateTextColor(chkCtrlList)
        UpdateTextColor(rdoCtrlList)

        svtBtnCtrl.btnPrint.Visible = False
        svtBtnCtrl.FlowLayoutPanel1.BackColor = Color.Transparent
        svtBtnCtrl.BackColor = Color.Transparent
        vsfCP.Styles.EmptyArea.BackColor = Color.Transparent
        vsfCP.BackColor = Color.Transparent

        vsfHdr.Styles.EmptyArea.BackColor = Color.Transparent
        vsfHdr.BackColor = Color.Transparent
        tbAdnlFlds.TabPages(0).BackColor = gChildFormBackColor
        fillFormProperties(Me)
        lblFrm.ForeColor = Color.White
        lblSearch.ForeColor = Color.White



        If Not chkUserRights(strRights, chkRightsPos.AddRights, Me.Text, False) Then svtBtnCtrl.btnNew.Visible = False
        If Not chkUserRights(strRights, chkRightsPos.EditRights, Me.Text, False) Then svtBtnCtrl.btnEdit.Visible = False
        If Not chkUserRights(strRights, chkRightsPos.DelRights, Me.Text, False) Then svtBtnCtrl.btnDelete.Visible = False
        'If Not chkUserRights(strRights, chkRightsPos.opt1Rights, Me.Text, False) Then svtBtnCtrl.btnOptions.Visible = False

        Call ChangeFontSize(Me)
        lblFrm.Font = New Font("Microsoft Sans Serif", 14, FontStyle.Bold)
        lblSearch.Font = New Font("Segoe UI", 9.25, FontStyle.Bold)

        If gblnVoucherPresent = True Then
            Call bindCombo(cboGroupLed, "LedGroupCode", "LedGroupCode", "Select LedGroupCode from MstLEDGROUP order by LedGroupCode")
            lblGroupLed.Visible = True
            cboGroupLed.Visible = True
        End If
        Call FillDocDetails(Val(txtCustID.Text))
    End Sub
    Private Sub formProp()
        'svtBtnCtrl.btnOptions.Visible = False
        svtBtnCtrl.btnPrint.Visible = False
    End Sub
    Public Sub initScreen()

        Dim strSQL As String
        Call InitAddnlFldGrids(Me, vsfHdr, "Header", intDocID, tbAdnlFlds)
        Call InsertDataTbl()
        Call enableDisableFlds(False)
        With vsfCP
            .Rows.Count = 1
            .Cols.Count = intCOlsCP
            .SetData(0, vCPName, "Name")
            .SetData(0, vCPDesg, "Designation")
            .SetData(0, vCPMobNo, "Mobile No")
            .SetData(0, vCPeMail, "eMail")
            .AutoSizeCols()
        End With
        With vsfFiles
            .Cols.Count = 3
            .Rows.Count = 0
            .Cols(vFSlno).Visible = False
            .Cols(vFName).Visible = False
        End With
        Dim csf1 As C1.Win.C1FlexGrid.CellStyle = vsfCP.Styles.Add("F1")
        csf1.BackColor = Color.Beige
        'vsfCP.Rows(0).Style = csf1
        'vsfCP.Cols(0).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.LeftCenter
        Call fillContactPersons()
        Call SetColWidth(vsfHdr, 205)

        Call clearFlds()
        Call initGridStyles(Me)
        Call initToolTipColours(lblToolTip, pnlToolTip)
        strSQL = "Select Max(" & strIDName & ") from " & strTblName
        txtCustID.Text = clsSVT.getDataLongValue(strSQL, 0, False)
        Call FillCombo()

        If cboCountry.Items.Count = 0 Then
            cboCountry.DataSource = Nothing
            cboCountry.Items.Add("INDIA")
        End If

        Me.Text = strFormCaption
        Me.Tag = strFormCaption
        intDocID = GetFrmId(Me.Text)
        PnlCopyFrom.Visible = False
        txtAddress.Visible = False

        svtBtnCtrl.btnOptions.Visible = True
        'svtBtnCtrl.addOption("Email")
        If clsSVT.getDataStringValue("Select PartySMSReq from UtlMisc", "N", False) = "Y" Then svtBtnCtrl.addOption("SMS")

        If blnAuthReq Then
            svtBtnCtrl.addOption("Approve")
        End If
        If clsSVT.getDataStringValue("Select BlnEnvPrint from UtlTransactions where docid = " & intDocID, "N", False) = "Y" Then svtBtnCtrl.addOption("Envelope Printing")
        'dtParty = clsSVT.getDATATable("Select " & strCodeName & "," & strFrmPfx & "Name from " & strTblName & " where  " & strIDName & ">0  order by 1,2")

        'dvSearch = New DataView(dtParty)
        'vsfsearch.DataSource = dvSearch
        'vsfsearch.Refresh()
        'vsfsearch.ScrollBars = ScrollBars.Both
        'vsfsearch.AutoSizeCols()

        '  intMenuID = GetMenuId(Me.Text)
        svtBtnCtrl.addOption("Label Print")
    End Sub

    '# A - Insertion of Data to avoid link problem
    Private Sub InsertDataTbl()
        Dim recdummy As oledb.oledbDataReader
        Dim strSQL As String

        strSQL = " select * from " & strTblName & " where " & strIDName & " = 0 "
        recdummy = clsSVT.getDataReaderFromSQL(strSQL)

        If recdummy.HasRows = False Then
            strSQL = " Insert into " & strTblName & " ("
            strSQL = strSQL & strIDName & "," & strFldSeries & "," & strFldNo & "," & strCodeName & ", " & strFrmPfx & "Name" & ","
            ' strSQL = strSQL & strPartyType & ","
            strSQL = strSQL & " InActive,AddedBy,AddedOn,ChangedBy,ChangedOn)"
            strSQL = strSQL & " values("
            strSQL = strSQL & " 0,'',0,'-', '-',"
            strSQL = strSQL & " 'N','ADMIN',"
            strSQL = strSQL & "'" & getDateFormatInInsert(getSystemDateTime, True) & "',"
            strSQL = strSQL & "'',"
            strSQL = strSQL & "'" & getDateFormatInInsert(getSystemDateTime, True) & "')"
            Call clsSVT.execSQLStatement(strSQL)
        End If
        recdummy.Close()

    End Sub

    'Private Function getstr(argstring As String) As String
    '    argstring = LCase(argstring)
    '    argstring =
    '    Return argstring
    'End Function

    Private Function ValidFields() As Boolean
        If Mode = "V" Then Exit Function
        Dim blnFound As Boolean

        StMsg("Validating Data.....")
        If Not IsNothing(vsfHdr.Editor) Then
            If vsfHdr.Editor.Text <> "" Then
                vsfHdr.SetData(vsfHdr.Row, vOthFldDataValue, vsfHdr.Editor.Text)
            End If
        End If

        ValidFields = False
        'strFrmMsg = vbProperCase
        'strFrmPfx = getstr(strFrmPfx)
        svtBtnCtrl.btnSave.Focus()

        If blnAutoGenCode = True Then
            If Trim(cboSeries.Text) = "" Then
                MsgBox("Series should not be blank!", MsgBoxStyle.Information, Me.Text)
                cboSeries.Focus()
                Exit Function
            End If

            Call FillAutoGenItemNo()

        End If

        If Trim(txtPartyCode.Text) = "" Then
            MsgBox(strFrmMsg & " Code should not be blank!", MsgBoxStyle.Information, Me.Text)
            txtPartyCode.Focus()
            Exit Function
        End If


        If Trim(txtPartyName.Text) = "" Then
            MsgBox((strFrmMsg) & " Name should not be blank!", MsgBoxStyle.Information, Me.Text)
            txtPartyName.Focus()
            Exit Function
        End If

        If txtBAdd1.Text = "" Then
            MsgBox("Please enter atleast first line address!", MsgBoxStyle.Information, Me.Text)
            txtBAdd1.Focus()
            Exit Function
        End If

        If chkInActive.Checked And Trim(txtInActiveRem.Text) = "" Then
            MsgBox("Please enter Remarks for why the record is inactive!", MsgBoxStyle.Information, Me.Text)
            txtInActiveRem.Focus()
            Exit Function
        End If

        If Mode = "A" Then
            Call clsSVT.getDataStringValue("Select " & strCodeName & " from " & strTblName &
                    " where  " & strCodeName & "='" & Trim(txtPartyCode.Text) & "'", "", blnFound)
            If blnFound Then
                'MsgBox(strCodeName & " already exists..", MsgBoxStyle.Information, Me.Text)
                MsgBox(lblPartyCode.Text & " already exists..", MsgBoxStyle.Information, Me.Text)
                txtPartyCode.Text = ""
                txtPartyCode.Focus()
                Exit Function
            End If
        End If

        If Mode = "A" Then
            Call clsSVT.getDataStringValue("Select " & strFrmPfx & "Name   from " & strTblName &
                              " where  " & strFrmPfx & "Name ='" & Trim(txtPartyName.Text) & "'", "", blnFound)
            If blnFound Then
                MsgBox(lblPartyName.Text & " already exists..", MsgBoxStyle.Information, Me.Text)
                txtPartyName.Text = ""
                txtPartyName.Focus()
                Exit Function
            End If
        End If
        Dim IntGrpId As Integer

        IntGrpId = clsSVT.getDataLongValue("Select " & strFrmPfx & "GroupID   from " & strTblName & "Group" &
                             " where  InActive = 'N' and " & strFrmPfx & "GroupCode ='" & Trim(cboPartyGrp.Text) & "'", 0, blnFound)
        If IntGrpId = 0 And Trim(cboPartyGrp.Text) <> "-" Then
            MsgBox(StrConv(strFrmPfx, VbStrConv.ProperCase) & " Group Code  NOT found in the master !", MsgBoxStyle.Information, Me.Text)
            cboPartyGrp.Text = ""
            cboPartyGrp.Focus()
            Exit Function
        End If

        '================= Check only if Indian Customer 

        ' If vsfHdr.GetData(intRowTaxType, vOthFldDataValue) = "LOCAL" Or vsfHdr.GetData(intRowTaxType, vOthFldDataValue) = "INTERSTATE" Then

        If cboCity.Text = "" Then
                MsgBox("Please Enter The City!", vbInformation, Me.Text)
                cboCity.Focus()
                Exit Function
            End If

            If cboCountry.Text = "" Then
                MsgBox("Please Enter The Country!", vbInformation, Me.Text)
                cboCountry.Focus()
                Exit Function
            End If
            If cboState.Text = "" Then
                MsgBox("Please Enter The State!", vbInformation, Me.Text)
                cboState.Focus()
                Exit Function
            End If


            Dim intSCCID As Integer
            intSCCID = clsSVT.getDataLongValue("Select SCCID from MstSCC where City='" & cboCity.Text & "'", 0, blnFound)
            If Not blnFound Then
                MsgBox("City Not found in Master!", vbInformation, Me.Text)
                cboCity.Focus()
                Exit Function
            End If

            intSCCID = clsSVT.getDataLongValue("Select SCCID from MstSCC where Country='" & cboCountry.Text & "'", 0, blnFound)
            If Not blnFound Then
                MsgBox("Country Not found in Master!", vbInformation, Me.Text)
                cboCountry.Focus()
                Exit Function
            End If

            intSCCID = clsSVT.getDataLongValue("Select SCCID from MstSCC where State='" & cboState.Text & "'", 0, blnFound)
            If Not blnFound Then
                MsgBox("State Not found in Master!", vbInformation, Me.Text)
                cboState.Focus()
                Exit Function
            End If
        'End If
        If Not validOthFLds(Me, strTblName, strIDName, Val(txtCustID.Text)) Then
                Exit Function
            End If

        ValidFields = True

    End Function

    Private Function saveDetails() As Boolean
        Dim strSQL As String = ""
        Dim strValues As String = ""
        Dim strFlds As String = ""
        Dim intID As Long

        saveDetails = False
        If Mode = "A" Then
            intID = clsSVT.AutoIncrementedNum(strIDName, strTblName)
            txtCustID.Text = intID

            strSQL = " Insert into " & strTblName & "("
            strSQL = strSQL & strIDName & ",AddedBy,AddedOn, " & strFldSeries & "," & strFldNo & "," & strCodeName & ", " & strFrmPfx & "Name," & strFrmPfx & "AliasName,"
            strSQL = strSQL & " InActive,InActiveRmks,"
            strSQL = strSQL & " Badd1, Badd2, Badd3,Badd4,City,State,PinCode,Country,Remarks,"
            strSQL = strSQL & strFrmPfx & "GroupId,"
            strSQL = strSQL & " Authorised,AuthBy,AuthOn"

            If gblnVoucherPresent Then
                strSQL = strSQL & " ,LedGroupID "
            End If

            '--4 begin OthGRID
            Call GetInsertString(Me, strValues, strFlds)
            strSQL = strSQL & strFlds & ") values ("
            strSQL = strSQL & intID & ","
            strSQL = strSQL & "'" & gUserCode & "',"
            strSQL = strSQL & "'" & getDateFormatInInsert(getSystemDateTime(svtTrn), True) & "',"
            strSQL = strSQL & "'" & Trim(cboSeries.Text) & "',"
            strSQL = strSQL & Val(txtNo.Text) & ","
            strSQL = strSQL & "'" & Trim(txtPartyCode.Text) & "', "
            strSQL = strSQL & "'" & Trim(txtPartyName.Text) & "',"
            strSQL = strSQL & "'" & Trim(txtTallyAlias.Text) & "',"
            strSQL = strSQL & "'" & getCheckString(chkInActive) & "',"
            strSQL = strSQL & "'" & Trim(txtInActiveRem.Text) & "', "
            strSQL = strSQL & "'" & Trim(txtBAdd1.Text) & "', "
            strSQL = strSQL & "'" & Trim(txtBAdd2.Text) & "', "
            strSQL = strSQL & "'" & Trim(txtBAdd3.Text) & "', "
            strSQL = strSQL & "'" & Trim(txtBadd4.Text) & "', "
            strSQL = strSQL & "'" & Trim(cboCity.Text) & "', "
            strSQL = strSQL & "'" & Trim(cboState.Text) & "',"
            strSQL = strSQL & " '" & Trim(txtPincode.Text) & "',"
            strSQL = strSQL & "'" & Trim(cboCountry.Text) & "',"
            strSQL = strSQL & "'" & Trim(txtRmks.Text) & "',"
            strSQL = strSQL & "" & clsSVT.getDataLongValue("Select " & strFrmPfx & "GroupId from Mst" & strFrmPfx & "Group Where " & strFrmPfx & "GroupCode ='" & Trim(cboPartyGrp.Text) & "'", 0, True, svtTrn) & ""
            ' strSQL = strSQL & "'" & getCheckString(chkEnforceItemLink) & "' "
            If Not blnAuthReq Then
                strSQL = strSQL & ", 'Y','" & gUserCode & "'," & getDateFormatInCond(getSystemDate(svtTrn)) & ""
            Else
                strSQL = strSQL & ", 'N','',Null"
            End If

            If gblnVoucherPresent Then
                strSQL = strSQL & "," & clsSVT.getDataLongValue("Select LedGroupID from MstLEDGROUP where LEDGROUPCODE = '" & Trim(cboGroupLed.Text) & "'", 0, False, svtTrn)
            End If

            '--5 begin OthGRID
            strSQL = strSQL & strValues

        ElseIf Mode = "M" Then

            strSQL = " update " & strTblName & " set "
            strSQL = strSQL & strFldSeries & " = '" & Trim(cboSeries.Text) & "',"
            strSQL = strSQL & strFldNo & " = " & Val(txtNo.Text) & ","
            strSQL = strSQL & strCodeName & " = '" & Trim(txtPartyCode.Text) & "',"
            strSQL = strSQL & strFrmPfx & "Name" & " = '" & Trim(txtPartyName.Text) & "',"
            strSQL = strSQL & " ChangedBy = '" & gUserCode & "', "
            strSQL = strSQL & " ChangedOn = '" & getDateFormatInInsert(getSystemDateTime(svtTrn), True) & "',"
            strSQL = strSQL & " InActive = '" & getCheckString(chkInActive) & "',"
            strSQL = strSQL & " InActiveRmks = '" & Trim(txtInActiveRem.Text) & "',"
            strSQL = strSQL & " Badd1 = '" & Trim(txtBAdd1.Text) & "', "
            strSQL = strSQL & " Badd2 = '" & Trim(txtBAdd2.Text) & "', "
            strSQL = strSQL & " Badd3 = '" & Trim(txtBAdd3.Text) & "', "
            strSQL = strSQL & " Badd4 = '" & Trim(txtBadd4.Text) & "', "
            strSQL = strSQL & " City = '" & cboCity.Text & "',"
            strSQL = strSQL & " State = '" & cboState.Text & "',"
            strSQL = strSQL & " PinCode =   '" & (txtPincode.Text) & "',"
            strSQL = strSQL & " Country= '" & cboCountry.Text & "',"
            strSQL = strSQL & " Remarks= '" & txtRmks.Text & "',"
            '  strSQL = strSQL & " EnforceItemLink = '" & getCheckString(chkEnforceItemLink) & "', "
            strSQL = strSQL & strFrmPfx & "GroupId = " & clsSVT.getDataLongValue("Select " & strFrmPfx & "GroupId from Mst" & strFrmPfx & "Group Where " & strFrmPfx & "GroupCode ='" & Trim(cboPartyGrp.Text) & "'", 0, True, svtTrn) & ","
            strSQL = strSQL & strFrmPfx & "AliasName = '" & txtTallyAlias.Text & "'"
            If gblnVoucherPresent Then
                strSQL = strSQL & ",LedGroupID = " & clsSVT.getDataLongValue("Select LedGroupID from MstLEDGROUP where LEDGROUPCODE = '" & Trim(cboGroupLed.Text) & "'", 0, False, svtTrn)
            End If

            '--3 begin OthGRID
            Call GetUpdateString(Me, strFlds)
            strSQL = strSQL & strFlds

            strSQL = strSQL & " where " & strIDName & "=" & Val(txtCustID.Text)
        End If
        strSQL = Replace(Replace(strSQL, vbCrLf, ""), vbCr, "")
        'begin trans
        svtTrn = svtConn.BeginTransaction(IsolationLevel.ReadCommitted)
        Try
            If Not clsSVT.execSQLStatement(strSQL, svtTrn) Then
                svtTrn.Rollback()
                Exit Function
            End If

            If Not updateLedger(svtTrn) Then
                svtTrn.Rollback()
                Exit Function
            End If

            'for audit track
            If blnAuditTrack Then
                Call RecordAuditLog(strFormCaption, Val(txtCustID.Text), IIf(Mode = "A", UserActions.AddAction, UserActions.EditAction), strTblName, strAuditHdrSQL, strAuditDtlSQL, svtTrn)
            End If
            saveDetails = True
            If Mode = "A" Then
                MsgBox(strFldCaption & " '" & Trim(txtPartyName.Text) & "' added successfully.", vbInformation, Me.Text)
            Else
                MsgBox("Updated successfully.", vbInformation, Me.Text)
            End If
            svtTrn.Commit()
            'Call clearFlds()
            Mode = ""
            Call FillDocDetails(txtCustID.Text, False)
        Catch ex As Exception
            svtTrn.Rollback()
            MsgBox("Error during Saving." & vbCrLf & ex.Message, MsgBoxStyle.Critical, Me.Text)
            Exit Function
        End Try



    End Function

    Public Function updateLedger(argsvt As oledb.oledbTransaction) As Boolean
        If gblnVoucherPresent = False Then Return True
        updateLedger = False

        Dim intAcLedID As Int32
        Dim intLedGroupID As Int32
        Dim strSQL As String = ""
        Dim strType As String

        strType = strIDName.ToUpper().Replace("ID", "")
        strType = Mid(strType, 1, 1)


        intLedGroupID = clsSVT.getDataLongValue("select LedGroupId from " & strTblName & " where " & strIDName & "=" & Val(txtCustID.Text), 0, False, argsvt)
        If Mode = "A" Then
            intAcLedID = clsSVT.AutoIncrementedNum("AcLedID", "MstAcLed", "", argsvt)
            strSQL = " Insert into MstAcLed "
            strSQL &= " (AcLedId, AcLedName, LedType, LedMstID, LedGroupId, MstDocID) "
            strSQL &= " values ("
            strSQL &= intAcLedID & ","
            strSQL &= "'" & Trim(txtPartyCode.Text) & "', "
            strSQL &= "'" & strType & "',"
            strSQL &= Val(txtCustID.Text) & ", "
            strSQL &= intLedGroupID & ","
            strSQL &= intDocID & ")"
        Else
            intAcLedID = clsSVT.getDataLongValue("select AcLedID from MstAcLed where MstDocID = " & intDocID & " and LedMstID = " & Val(txtCustID.Text), 0, False, argsvt)
            If intAcLedID = 0 Then
                intAcLedID = clsSVT.AutoIncrementedNum("AcLedID", "MstAcLed", "", argsvt)
                strSQL = " Insert into MstAcLed "
                strSQL &= " (AcLedId, AcLedName, LedType, LedMstID, LedGroupId, MstDocID) "
                strSQL &= " values ("
                strSQL &= intAcLedID & ","
                strSQL &= "'" & Trim(txtPartyCode.Text) & "', "
                strSQL &= "'" & strType & "',"
                strSQL &= Val(txtCustID.Text) & ", "
                strSQL &= intLedGroupID & ","
                strSQL &= intDocID & ")"
            Else
                strSQL = " Update MstAcLed Set "
                strSQL &= " LedGroupId = " & intLedGroupID
                strSQL &= " Where AcLedID = " & intAcLedID
            End If
        End If

        If strSQL <> "" AndAlso Not clsSVT.execSQLStatement(strSQL, argsvt) Then
            Exit Function
        End If

        updateLedger = True
    End Function

    Public Sub vsfHdr_AfterEdit(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfHdr.AfterEdit

        Call vsfGenAfterEditBfrCalTot(Me, sender, e.Row, "M")
        Call CalTotal()
        Call vsfGenAfterEditAftrCalTot(Me, sender, intDocID, Mode, e.Row, False)

    End Sub

    Public Sub vsfHdr_BeforeEdit(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfHdr.BeforeEdit

        If vsfBeforeEditCancel(Me, sender, intDocID, strTblName, Mode, e.Row, e.Col, "M", "") Then
            e.Cancel = True
            Exit Sub
        End If
    End Sub
    Public Sub vsfCust_BeforeEdit(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.RowColEventArgs)

        vsfBeforeEditCancel(Me, sender, intDocID, strTblName, "M", e.Row, e.Col, "M", "")
        'e.Cancel = True
        '    Exit Sub
        'End If
    End Sub
    Public Sub vsfHdr_KeyPressEdit(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.KeyPressEditEventArgs) Handles vsfHdr.KeyPressEdit
        If e.KeyChar = "'" Then
            e.Handled = True
            Exit Sub
        End If
        If vsfKeyPressEdit(Me, sender, vsfHdr.Row, e.KeyChar) Then
            e.Handled = False
        Else
            e.Handled = True
        End If

    End Sub

    Public Sub vsfHdr_KeyUp(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles vsfHdr.KeyUp
        With sender
            If .Row = .Rows.Count - 1 Then
                svtBtnCtrl.Focus()
            End If
        End With
    End Sub

    Private Sub vsfHdr_LostFocus(sender As Object, e As System.EventArgs) Handles vsfHdr.LostFocus
        pnlToolTip.Visible = False
    End Sub

    Public Sub vsfHdr_RowColChange(ByVal sender As Object, ByVal e As System.EventArgs) Handles vsfHdr.RowColChange
        If Not (Mode = "A" Or Mode = "M") Then Exit Sub
        lblToolTip.Text = ""
        lblToolTip.Text = vsfRowColChange(Me, sender)
        If lblToolTip.Text <> "" Then
            pnlToolTip.Visible = True
        Else
            pnlToolTip.Visible = False
        End If
    End Sub

    Public Sub vsfHdr_SetupEditor(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfHdr.SetupEditor
        Call vsfSetupEditor(Me, sender, intDocID, strTblName, Mode, e.Row, e.Col, "M")
    End Sub

    Public Sub vsfHdr_ValidateEdit(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.ValidateEditEventArgs) Handles vsfHdr.ValidateEdit
        'If Not vsfValidateEdit(Me, vsfHdr, e.Row, e.Col, "M", strTblName, strIDName, Val(txtCustID.Text)) Then
        '    e.Cancel = True
        'End If
    End Sub

    Public Sub FillDocDetails(ByVal argCustID As Long, Optional ByVal argCopyItem As Boolean = False)
        Dim RsTemp As oledb.oledbDataReader
        Dim strSQL As String
        Dim i As Integer, X As Integer

        If argCustID = 0 Then Exit Sub

        '--8 begin OthGRID
        If argCopyItem = False Then
            Call ClearAddnlGrid(vsfHdr)
        End If

        strSQL = " select * from " & strTblName
        strSQL = strSQL & " where " & strIDName & " = " & argCustID
        RsTemp = clsSVT.getDataReaderFromSQL(strSQL)

        With RsTemp
            If .Read() Then
                txtCustID.Text = argCustID
                cboSeries.Text = GetVal(RsTemp(strFldSeries), "")
                txtPartyName.Text = GetVal(RsTemp(strFrmPfx & "Name"), "")
                txtInActiveRem.Text = GetVal(RsTemp("InActiveRmks"), "")
                txtTallyAlias.Text = GetVal(RsTemp(strFrmPfx & "AliasName"), "")

                If gblnVoucherPresent Then
                    cboGroupLed.Text = clsSVT.getDataStringValue("Select LEDGROUPCODE from MstLEDGROUP where LEDGROUPID = '" & GetVal(RsTemp("LEDGROUPID")) & "'", "", False)
                End If

                If GetVal(RsTemp("InActive"), "") = "N" Then
                        chkInActive.Checked = False
                        chkInActive.Text = "InActive"
                    Else
                        chkInActive.Checked = True
                        chkInActive.Text = "InActive Remarks"
                    End If
                    txtBAdd1.Text = GetVal(RsTemp("BAdd1"), "")
                    txtBAdd2.Text = GetVal(RsTemp("BAdd2"), "")
                    txtBAdd3.Text = GetVal(RsTemp("BAdd3"), "")
                    txtBadd4.Text = GetVal(RsTemp("BAdd4"), "")

                    'If GetVal(RsTemp("EnforceItemLink"), "") = "Y" Then
                    '    chkEnforceItemLink.Checked = True
                    'Else
                    '    chkEnforceItemLink.Checked = False
                    'End If

                    txtAddress.Text = Trim(GetVal(RsTemp.Item("BAdd1"), "")) & vbCrLf &
                                  Trim(GetVal(RsTemp.Item("BAdd2"), "")) & vbCrLf &
                                  Trim(GetVal(RsTemp.Item("BAdd3"), "")) & vbCrLf &
                                  Replace(Trim(GetVal(RsTemp.Item("BAdd4"), "")), vbCrLf, " ")
                    cboCity.Text = GetVal(RsTemp("City"), "")
                    cboState.Text = GetVal(RsTemp("State"), "")
                    cboCountry.Text = GetVal(RsTemp("Country"), "")
                    txtPincode.Text = (GetVal(RsTemp("PinCode"), ""))
                    txtRmks.Text = GetVal(RsTemp("Remarks"), "")
                    txtPartyCode.Text = GetVal(RsTemp(strCodeName), "")
                cboPartyGrp.Text = clsSVT.getDataStringValue("Select " & strFrmPfx & "GroupCode from Mst" & strFrmPfx & "Group Where " & strFrmPfx & "GroupId =" & CInt(GetVal(RsTemp(strFrmPfx & "GRouPID"), 0)), 0, True) & ""
                PicParty.Image = Nothing
                    'For Each foundFile As String In My.Computer.FileSystem.GetFiles(gstrRptPath & "Images\", Microsoft.VisualBasic.FileIO.SearchOption.SearchAllSubDirectories, strFrmPfx & "_" & txtPartyCode.Text & ".*")
                    '    picParty.Image = Image.FromFile(foundFile)
                    '    Exit For
                    'Next
                    LoadPartyImage()
                    '--9 begin OthGRID
                    For i = 0 To vsfHdr.Rows.Count - 1
                        For X = 0 To .FieldCount - 1
                            If UCase(.GetName(X)) = UCase(Trim(vsfHdr.GetData(i, vOthFldCode))) Then
                                If Trim(vsfHdr.GetData(i, vOthFldCmbTblName)) = "" Then
                                    vsfHdr.SetData(i, vOthFldDataValue, GetVal(.GetValue(X), ""))
                                Else
                                    vsfHdr.SetData(i, vOthFldDataValue,
                                    clsSVT.getDataStringValue("Select " & vsfHdr.GetData(i, vOthFldCmbDisplayCols) & " from " &
                                    vsfHdr.GetData(i, vOthFldCmbTblName) & " where " &
                                    vsfHdr.GetData(i, vOthFldCmbStorageFldName) & " = " & GetVal(.GetValue(X), 0), "", False))
                                End If
                            End If
                        Next X
                    Next

                    btnAddr.Text = "Multi Address (" & clsSVT.getDataLongValue("Select Count(*) from MstPartyAddress where MstDocID=" & intDocID & " and PartyID=" & argCustID, 0, False) & ")"
                    btnConPer.Text = "Contact Person (" & clsSVT.getDataLongValue("Select Count(*) from MstPartyContacts where MstDocID=" & intDocID & " and PartyID=" & argCustID, 0, False) & ")"
                    btnAlert.Text = "Alert Message (" & clsSVT.getDataLongValue("Select Count(*) from MstPartyAlertMessage where PartyDocID=" & intDocID & " and PartyID=" & argCustID, 0, False) & ")"
                    btnActivities.Text = "Activities (" & clsSVT.getDataLongValue("Select Count(*) from MstPartyActivity where PartyDocID=" & intDocID & " and PartyID=" & argCustID, 0, False) & ")"
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
                    .Close()
                    If vsfHdr.Rows.Count > 0 Then
                        Call vsfGenAfterEditBfrCalTot(Me, vsfHdr, vsfHdr.Rows.Count - 1, "M")
                    End If

                End If
        End With
        btnFileAttach.Text = "File Attachment (" & clsSVT.getDataLongValue("Select count(*) from UtlAttachFiles Where DocID = " & intDocID & " and DocValueID = '" & argCustID & "'", 0, False) & ")"
        Call fillAttachFiles(Val(txtCustID.Text))

        txtAddress.Visible = True
        txtAddress.BringToFront()
        Call fillContactPersons()
        txtSearch.Focus()
        If argCopyItem = False Then
            Mode = "V"
            enableDisableFlds(False)
            txtSearch.Focus()
        End If
    End Sub

    Private Sub LoadPartyImage()

        Dim strPartyName As String = ""
        strPartyName = Regex.Replace(txtPartyCode.Text, "[^A-Za-z0-9]", "")

        Try
            With PicParty
                If File.Exists(gstrPartyImagePath & GetVal(Trim(strPartyName), "") & ".jpg") Then
                    .ImageLocation = gstrPartyImagePath & GetVal(Trim(strPartyName), "") & ".jpg"
                    .SizeMode = PictureBoxSizeMode.StretchImage
                    .Visible = True
                Else
                    .ImageLocation = Nothing
                    .Image = vLQMS.My.Resources.user
                    .SizeMode = PictureBoxSizeMode.StretchImage
                    .Visible = False
                End If
            End With
        Catch ex As Exception
            MsgBox("Error on loading image!" & vbCrLf & ex.Message, MsgBoxStyle.Critical, Me.Text)
            Exit Sub
        End Try
    End Sub
    Private Sub CalTotal()
        Call AddnlMSTFldCalTotal(Me, vsfHdr, 0, "", "", txtCustID.Text)
    End Sub

    Private Sub setSearchGridHt(ByVal argFlag As String)
        If argFlag = "FULL" Then
            vsfsearch.Visible = True
            vsfsearch.BringToFront()
            'vsfSearch.Location = New Point(2, 45)
            'vsfSearch.Location = New Point(13, 51)
            pnlSearch.BringToFront()
        Else
            vsfsearch.Visible = False
            'vsfSearch.Height = 21
            'vsfSearch.Location = New Point(160, 6)
        End If

    End Sub
    Public Function NumberList(strSeparator As String, ParamArray arrVals() As String)

        ' Pass this function a character or characters '
        ' to be used as the separator followed by the values to be combined
        ' For example: strNumberedList =
        ' NumberList(" ", foo, bar)
        Dim X As Integer, strLine As String = ""

        For X = 0 To UBound(arrVals)
            If Not IsNothing(arrVals(X)) And Trim(arrVals(X)) <> "" Then
                strLine = strLine & strSeparator & arrVals(X)
            End If
        Next

        ' remove leading separator caharacter(s)
        strLine = Mid(strLine, Len(strSeparator) + 1)
        NumberList = strLine

    End Function
    Private Sub txtSearch_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles txtSearch.KeyPress
        If e.KeyChar = "'" Then
            e.Handled = True
        End If
    End Sub

    Private Sub txtSearch_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtSearch.KeyUp
        Dim StrQry As String, strCond As String = ""
        If e.KeyCode = Keys.Return Then
            Call vsfSearchSelectedItem()
            Exit Sub
        End If
        If e.KeyCode = Keys.Down Then
            vsfsearch.Focus()
            Exit Sub
        End If

        txtSearch.Text = Replace(txtSearch.Text, "'", "`")

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
        StrQry = "Select Top 30  " & strCodeName & "," & strFrmPfx & "Name"
        StrQry &= " from " & strTblName
        StrQry &= " where (" & strCodeName & " like '%" & Trim(txtSearch.Text) & "%'"
        StrQry &= " or " & strFrmPfx & "Name Like '%" & Trim(txtSearch.Text) & "%')"
        StrQry &= " order by 1"
        Call showWaitCursor()
        dtParty = clsSVT.getDATATable(StrQry)
        dvSearch = New DataView(dtParty)
        vsfsearch.DataSource = dvSearch
        Call showNormalCursor()
        vsfsearch.Refresh()
        vsfsearch.ScrollBars = ScrollBars.Both
        vsfsearch.AutoSizeCols()
    End Sub

    Private Sub vsfSearch_BeforeEdit(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfsearch.BeforeEdit
        e.Cancel = True
    End Sub

    Private Sub vsfSearch_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles vsfsearch.DoubleClick
        Call vsfSearchSelectedItem()
    End Sub

    Private Sub vsfSearchSelectedItem()
        Dim intCustID As Integer
        Dim strsQL As String
        If vsfsearch.Rows.Count > 1 Then
            strsQL = "Select " & strIDName & " from " & strTblName & " where " & strCodeName & "='" & vsfsearch.GetData(vsfsearch.Row, 0) & "'"

            intCustID = clsSVT.getDataLongValue(strsQL, 0, False)

            Mode = ""
            Call FillDocDetails(intCustID, False)
            Call setSearchGridHt("")
            txtSearch.Text = ""
            txtSearch.Focus()
        End If
    End Sub

    Private Sub vsfSearch_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles vsfsearch.KeyUp
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

    Private Sub enableDisableFlds(ByVal argFlag As Boolean)

        Lock(txtPartyCode, argFlag)
        Lock(txtPartyName, argFlag)
        Lock(txtBAdd1, argFlag)

        Lock(txtBAdd2, argFlag)
        Lock(txtBAdd3, argFlag)
        Lock(txtBadd4, argFlag)
        Lock(txtPincode, argFlag)
        Lock(cboCity, argFlag)
        Lock(cboState, argFlag)
        Lock(txtAddress, argFlag)
        Lock(txtRmks, argFlag)
        Lock(cboCountry, argFlag)
        Lock(txtTallyAlias, argFlag)
        Lock(cboGroupLed, argFlag)
        Lock(cboPartyGrp, argFlag)
        Lock(cboSeries, argFlag)
        chkInActive.Enabled = argFlag
        btnAddr.Enabled = argFlag
        btnAddr.Enabled = argFlag
        '  chkEnforceItemLink.Enabled = argFlag

        If chkInActive.Checked And Mode = "A" Then
            txtInActiveRem.Visible = True
            chkInActive.Visible = False
        Else
            txtInActiveRem.Visible = False
            chkInActive.Visible = False
        End If

        If chkInActive.Checked And Mode = "M" Then
            txtInActiveRem.Visible = True
            txtInActiveRem.Enabled = True
            chkInActive.Visible = True

        Else
            chkInActive.Visible = True
            chkInActive.Text = "InActive"

        End If

        If chkInActive.Checked And Mode = "V" Then
            txtInActiveRem.Enabled = False
            txtInActiveRem.Visible = True
            chkInActive.Visible = True

        End If
        If Mode = "A" Then
            btnAddr.Enabled = Not argFlag
            btnConPer.Enabled = Not argFlag
            btnAlert.Enabled = Not argFlag
            btnActivities.Enabled = Not argFlag
        ElseIf Mode = "M" Then
            btnAddr.Enabled = argFlag
            btnConPer.Enabled = argFlag
            btnAlert.Enabled = argFlag
            btnActivities.Enabled = argFlag
        End If
        If Mode = "V" Then
            btnAddr.Enabled = True
            btnConPer.Enabled = True
            btnAlert.Enabled = True
            btnActivities.Enabled = True
        End If
        If Mode = "A" Then
            PnlAttach.Enabled = False
        Else
            PnlAttach.Enabled = True
        End If
        pnlSearch.Visible = Not argFlag
    End Sub

    Public Sub initForm(ByVal argDocID As Integer)
        If argDocID > 0 Then intDocID = argDocID
        Dim rs As oledb.oledbDataReader
        rs = clsSVT.getDataReaderFromSQL(" Select * from UtlTransactions where DocID = " & argDocID)
        With rs
            .Read()
            If !MstPartyTally = "Y" Then
                lblTally.Visible = True
                txtTallyAlias.Visible = True
            Else
                lblTally.Visible = False
                txtTallyAlias.Visible = False
            End If
            'for audit track
            If GetVal(!AuditLogTrack, "") = "Y" Then
                blnAuditTrack = True
                strAuditHdrSQL = GetVal(!AuditSQLHdr, "")
                strAuditDtlSQL = GetVal(!AuditSQLDtl, "")
            End If
            strFrmMsg = Trim(Replace(!Docname, "Master", ""))
            strFormCaption = !DocName
            strTblName = !HdrTblName
            strCodeName = Trim(Replace(!HdrTblName, "Mst", "")) & "Code"
            strIDName = Trim(Replace(!HdrTblName, "Mst", "")) & "ID"
            strFrmPfx = Trim(Replace(!HdrTblName, "Mst", ""))
            strFldSeries = Replace(!HdrTblName, "Mst", "") & "Series"
            strFldNo = Replace(!HdrTblName, "Mst", "") & "No"
            lblGrp.Text = TITLECASE(strFrmPfx) & " Group"
            If GetVal(!AuthReq, "N") = "Y" Then
                blnAuthReq = True
            Else
                blnAuthReq = False
            End If

            If !FileAttachReq = "Y" Then
                blnShowFileAttach = True
            Else
                blnShowFileAttach = False
            End If

            strMstPartyAccounts = GetVal(!MstPartyAccounts, "N")
            If GetVal(!MstAutoGenCode, "N") = "Y" Then
                blnAutoGenCode = True
                txtPartyCode.TextAlign = HorizontalAlignment.Center
            Else
                blnAutoGenCode = False
                lblPartyCode.Left = lblPartyName.Left + 4
                txtPartyCode.Left = txtPartyName.Left
                txtPartyCode.Width = txtPartyName.Width
                txtPartyCode.TextAlign = HorizontalAlignment.Left
            End If

            Call setAutoGenCode()
            .Close()
        End With
        'Else
        '    strTblName = argTblName
        '    strIDName = argIDName
        '    strCodeName = argCodeName
        '    strPartyName = argPartyName
        '    strFormCaption = argFormCaption
        '    strFldCaption = argFldCaption
        '    Label3.Text = strFormCaption
        '    strsql = clsSVT.getDataStringValue("select Substring('" & strTblName & "',4, (Len('" & strTblName & "')))", "", False)
        '    strGroupID = strsql & "GroupID"
        '    strGroupName = strsql & "GroupName"
        '    strGroupTblName = strTblName & "Group"
        '    strPartyType = strsql & "Type"
        '    intDocID = clsSVT.getDataLongValue(" Select DocID from UtlTransactions where HdrTblName = '" & strTblName & "'", 0, False)

        '    If clsSVT.getDataStringValue(" Select FileAttachReq from UtlTransactions where HdrTblName = '" & strTblName & "'", "Y", False) = "N" Then
        '        blnShowFileAttach = True
        '    Else
        '        blnShowFileAttach = False
        '    End If
        'End If

        intMenuItemID = clsSVT.getDataLongValue("Select MenuItemID from MstMenu  where DocID = " & intDocID, 0, False)
        strRights = getUserRights(intMenuItemID)

        lblPartyCode.Text = TITLECASE(strFrmPfx) & " Code"
        lblPartyName.Text = TITLECASE(strFrmPfx) & " Name"

        Me.Text = strFormCaption
        Call initScreen()
        Call setSearchGridHt("")

        If Not blnShowFileAttach Then
            PnlAttach.Enabled = False
        Else
            PnlAttach.Enabled = True
        End If
        KeyPreview = True
        Call formProp()

        ' intRowTaxType = getRowNoFromAddnlGrid(vsfHdr, "TAXTYPE")
    End Sub

    Private Sub deleteDocument()
        Dim strSQL As String

        If Val(txtCustID.Text) <= 0 Then Exit Sub

        If Not blnDeleteOK(strTblName, Val(txtCustID.Text), True, True) Then
            Exit Sub
        End If

        If MsgBox("Are you sure you want to delete the " & strFrmMsg &
            " : '" & txtPartyName.Text & "'?", vbCritical + MsgBoxStyle.YesNo + vbDefaultButton2, " Delete Confirmation") = MsgBoxResult.Yes Then

            Try
                svtTrn = svtConn.BeginTransaction(IsolationLevel.ReadCommitted)

                'for audit track
                Call RecordAuditLog(strFormCaption, Val(txtCustID.Text), UserActions.DeleteAction, strTblName, strAuditHdrSQL, "", svtTrn)
                strSQL = " Delete from " & strTblName & " where " & strIDName & " = " & Val(txtCustID.Text)
                If Not clsSVT.execSQLStatement(strSQL, svtTrn) Then
                    svtTrn.Rollback()
                    Exit Sub
                End If

                strSQL = "Delete from MstPartyaddress where MstDocID = " & intDocID & " and PartyId = " & Val(txtCustID.Text)
                If Not clsSVT.execSQLStatement(strSQL, svtTrn) Then
                    svtTrn.Rollback()
                    Exit Sub
                End If

                strSQL = "Delete from MstPartyContacts where  MstDocID = " & intDocID & " and PartyId = " & Val(txtCustID.Text)
                If Not clsSVT.execSQLStatement(strSQL, svtTrn) Then
                    svtTrn.Rollback()
                    Exit Sub
                End If

                strSQL = "Delete from UtlAttachFiles where DocID = " & intDocID & " and DocValueID = " & Val(txtCustID.Text)
                If Not clsSVT.execSQLStatement(strSQL, svtTrn) Then
                    svtTrn.Rollback()
                    Exit Sub
                End If
                'Call UpdtAuditTrailTable(intDocID, "D", Val(txtCustID.Text))
                MsgBox("Data Deleted sucessfully", vbInformation, Me.Text)
                Call clearFlds()
                svtTrn.Commit()
            Catch ex As Exception
                MsgBox("Error during save" & vbCrLf & ex.Message, MsgBoxStyle.Critical, Me.Text)
                svtTrn.Rollback()
            End Try
        End If

    End Sub

    Private Sub svtBtnCtrl_btnClicked(argBtnClicked As Integer) Handles svtBtnCtrl.btnClicked
        Select Case argBtnClicked
            Case svtBtns.svtNew
                Mode = "A"
                If Not chkUserRights(strRights, chkRightsPos.AddRights, Me.Text) Then Exit Sub
                Call AddProc()
                svtBtnCtrl.PerformAction(argBtnClicked)
                btnAddr.Enabled = False
                btnConPer.Enabled = False
                lblFrm.Text = "Add:" & Me.Text
                txtAddress.Visible = False
                btnAlert.Enabled = False
                btnActivities.Enabled = False
                btnCopyFrom.Visible = True
                Call setAutoGenCode()
                Me.Parent.Tag = "A"
            Case svtBtns.svtClose
                Call frmMain.CloseSelectedTabPage(Me)

            Case svtBtns.svtEdit
                ' txtCustCode.Enabled = False
                If Trim(txtPartyCode.Text) = "" Then
                    Exit Sub
                End If
                If Not chkUserRights(strRights, chkRightsPos.EditRights, Me.Text) Then Exit Sub
                Mode = "M"
                Call enableDisableFlds(True)

                'txtPartyCode.ReadOnly = False
                'txtPartyName.Focus()
                Call chkSplRights_Std(Me, intDocID, strRights)
                Call chkSplRights_DOC(Me, intDocID, strRights, vsfHdr)
                Call setAutoGenCode()
                Call setSearchGridHt("")
                svtBtnCtrl.PerformAction(argBtnClicked)
                btnConPer.Enabled = True
                btnAlert.Enabled = True
                lblFrm.Text = "Edit:" & Me.Text
                txtAddress.Visible = False
                btnActivities.Enabled = True
                Me.Parent.Tag = "M"
            Case svtBtns.svtSave
                vsfHdr.Enabled = False
                txtSearch.Focus()
                vsfHdr.Enabled = True
                stringReplace(Me)
                If ValidFields() = True Then
                    Call saveDetails()
                    enableDisableFlds(False)
                    svtBtnCtrl.PerformAction(argBtnClicked)
                    btnCopyFrom.Visible = False
                    Mode = "V"
                Else

                    Exit Sub
                End If
                vsfHdr.Enabled = True

                'Call FillDocDetails(txtCustID.Text, True)
                lblFrm.Text = "View:" & Me.Text
                'pnlSearch.Visible = True
                Me.Parent.Tag = ""
            Case svtBtns.svtFind
                If Not chkUserRights(strRights, chkRightsPos.ViewRights, Me.Text) Then Exit Sub
                Call showViewForm(Me, strTblName, "Listing of " & Me.Text, "", "", " and " & strCodeName & " <> '-'", strFormCaption, strCodeName, intDocID)
                Mode = ""
                enableDisableFlds(False)
                svtBtnCtrl.PerformAction(argBtnClicked)
                lblFrm.Text = "View:" & Me.Text
                btnActivities.Enabled = True
                btnAddr.Enabled = True

            Case svtBtns.svtCancel
                Call clearFlds()
                txtSearch.Focus()
                svtBtnCtrl.PerformAction(argBtnClicked)
                lblFrm.Text = "View:" & Me.Text
                PnlCopyFrom.Visible = False
                Me.Parent.Tag = ""
            Case svtBtns.svtDelete
                If Not chkUserRights(strRights, chkRightsPos.DelRights, Me.Text) Then Exit Sub
                Call deleteDocument()
                svtBtnCtrl.PerformAction(svtBtns.svtCancel)
                lblFrm.Text = "View:" & Me.Text

            Case svtBtns.svtMoveFirst, svtBtns.svtMoveLast, svtBtns.svtMoveNext, svtBtns.svtMovePrev
                Call FillDocDetails(gMoveRecords(argBtnClicked, strTblName, strIDName, True, txtCustID.Text, ""))
                svtBtnCtrl.PerformAction(argBtnClicked)
                lblFrm.Text = "View:" & Me.Text
                txtAddress.Visible = True
                txtAddress.BringToFront()
        End Select
        If Not chkUserRights(strRights, chkRightsPos.AddRights, Me.Text, False) Then svtBtnCtrl.btnNew.Visible = False
        If Not chkUserRights(strRights, chkRightsPos.EditRights, Me.Text, False) Then svtBtnCtrl.btnEdit.Visible = False
        If Not chkUserRights(strRights, chkRightsPos.DelRights, Me.Text, False) Then svtBtnCtrl.btnDelete.Visible = False
        'If Not chkUserRights(strRights, chkRightsPos.opt1Rights, Me.Text, False) Then svtBtnCtrl.btnOptions.Visible = False
        svtBtnCtrl.btnPrint.Visible = False
    End Sub

    Private Sub frmMstParty_Resize(sender As Object, e As System.EventArgs) Handles Me.Resize
        'MsgBox(Me.Top)
        If Me.Width = 0 Then Exit Sub
        If vsfHdr.Rows.Count <= 0 Then Exit Sub
        Try
            Dim sngActGrdHt As Single = vsfHdr.Rows(0).HeightDisplay * (vsfHdr.Rows.Count) + 2
            If sngActGrdHt < Me.Height And sngActGrdHt > 0 Then
                vsfHdr.Height = sngActGrdHt
            End If
            If vsfHdr.Cols(vOthFldCaption).Width >= vsfHdr.Width Then
                vsfHdr.Cols(vOthFldCaption).Width = vsfHdr.Width - 20
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub btnConPer_Click(sender As System.Object, e As System.EventArgs) Handles btnConPer.Click
        If txtPartyCode.Text = "" Then Exit Sub
        With frmMstPartyContactPerson
            .strfrmCaption = strFrmMsg
            .txtPartyCode.Text = txtPartyCode.Text
            .lblPartyName.Text = txtPartyName.Text
            .strRights = strRights
            .strfrmCaption = strFrmMsg
            .Mode = Mode
            .initForm(intDocID, Val(txtCustID.Text))
            .ShowDialog()
            btnConPer.Text = "Contact Person (" & clsSVT.getDataLongValue("Select Count(*) from MstPartyContacts where MstDocID=" & intDocID & " and PartyID=" & Val(txtCustID.Text), 0, False).ToString & ")"
            Call fillContactPersons()
        End With
    End Sub

    Private Sub btnConPer_GotFocus(sender As Object, e As System.EventArgs) Handles btnConPer.GotFocus
        If txtPartyCode.Text = "" Then
            Exit Sub
        End If
        Call fillContactPersons()
    End Sub
    Private Sub fillContactPersons()
        Dim rs As oledb.oledbDataReader
        vsfCP.Rows.Count = 1
        rs = clsSVT.getDataReaderFromSQL("Select * from MstPartyContacts where MstDocID = " & intDocID & " and PartyID = " & Val(txtCustID.Text))
        Do While rs.Read
            vsfCP.Rows.Add()
            vsfCP.SetData(vsfCP.Rows.Count - 1, vCPName, rs("CName"))
            vsfCP.SetData(vsfCP.Rows.Count - 1, vCPDesg, rs("CDesig"))
            vsfCP.SetData(vsfCP.Rows.Count - 1, vCPMobNo, rs("CMobNo"))
            vsfCP.SetData(vsfCP.Rows.Count - 1, vCPeMail, rs("EMail"))
        Loop
        rs.Close()
    End Sub

    Private Sub btnAddr_Click(sender As System.Object, e As System.EventArgs) Handles btnAddr.Click
        If Trim(txtPartyCode.Text) = "" Then Exit Sub
        With frmMstPartyAddress
            .txtPartyCode.Text = txtPartyCode.Text
            .lblPartyName.Text = txtPartyName.Text
            .strfrmCaption = strFrmMsg
            .Mode = Mode
            .strRights = strRights
            .initForm(intDocID, Val(txtCustID.Text))
            .ShowDialog()
            btnAddr.Text = "Multi Address (" & clsSVT.getDataLongValue("Select Count(*) from MstPartyAddress where MstDocID=" & intDocID & " and PartyID=" & Val(txtCustID.Text), 0, False).ToString & ")"
        End With
    End Sub

    Private Sub chkInActive_CheckedChanged_1(sender As System.Object, e As System.EventArgs) Handles chkInActive.CheckedChanged
        If chkInActive.Checked Then
            chkInActive.Text = "InActive Remarks"
            txtInActiveRem.Visible = True
            txtInActiveRem.Enabled = True
            txtInActiveRem.ForeColor = Color.Black
            txtInActiveRem.Font = New Font("Segoe UI", 10, FontStyle.Regular)
            txtInActiveRem.Focus()
        Else
            txtInActiveRem.Visible = False
            txtInActiveRem.Enabled = False
            txtInActiveRem.Text = ""
            chkInActive.Text = "InActive"
        End If
    End Sub

    Private Sub chkInActive_GotFocus1(sender As Object, e As System.EventArgs) Handles chkInActive.GotFocus
        If txtPartyCode.Text = "" Then
            Exit Sub
        End If
        Call fillContactPersons()
    End Sub

    Private Sub txtCustName_TextChanged_1(sender As System.Object, e As System.EventArgs) Handles txtPartyName.TextChanged
        If Mode <> "A" Then Exit Sub
    End Sub

    Private Sub txtLimitAmt_KeyPress1(sender As Object, e As System.Windows.Forms.KeyPressEventArgs)
        If KeyValidate("Integer", e.KeyChar) Then
            e.Handled = False
        Else
            e.Handled = True
        End If
    End Sub

    Private Sub txtPincode_KeyPress1(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles txtPincode.KeyPress
        If KeyValidate("Integer", e.KeyChar) Then
            e.Handled = False
        Else
            e.Handled = True
        End If
        If e.KeyChar = "'" Then
            e.Handled = True
        End If
    End Sub

    Private Sub vsfCP_BeforeEdit1(sender As Object, e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfCP.BeforeEdit
        e.Cancel = True
    End Sub

    Private Sub vsfCP_DoubleClick(sender As Object, e As System.EventArgs) Handles vsfCP.DoubleClick
        If txtPartyCode.Text = "" Then Exit Sub
        Call btnConPer_Click(sender, e)
    End Sub
    Public Sub FillCtrlName(ByVal CtrlList As Object)
        With vsfAS
            For Each Ctrl As Control In CtrlList
                .Rows.Count = .Rows.Count + 1
                .SetData(.Rows.Count - 1, vASCtrlName, Ctrl.Name)
            Next
        End With
    End Sub
    Public Function GetAllCtrls(control As Control, type As Type) As IEnumerable(Of Control)
        Dim controls = control.Controls.Cast(Of Control)()
        Return controls.SelectMany(Function(ctrl) GetAllCtrls(ctrl, type)).Concat(controls).Where(Function(c) c.[GetType]() = type And CStr(c.Tag) = "")
    End Function
    Private Sub RestoreData()
        Dim strTimer As String = ""
        Dim strFileName As String = strRestoreFileName
        For Each strTimer In strFileName.Split("_")

        Next
        strRestoreFileName = Date.Now.ToString("yyyyMMddHHmmss")
        RestoreData_Grid(vsfAS, strFileName)
        RestoreData_Grid(vsfHdr, "AS_Mst_Cust_Vsf" & strTimer)

        UpdateC1DateCtrl(dtC1CtrlList)
        UpdateTextCtrl(cboCtrlList)
        UpdateTextCtrl(txtCtrlList)
        UpdateRadioCtrl(rdoCtrlList)
        UpdateChkBoxCtrl(chkCtrlList)
    End Sub
    Private Sub RestoreData_Grid(ByVal argVsf As C1FlexGrid, ByVal argPath As String)
        With argVsf
            Try
                .ReadXml(strRestorePath & argPath)
            Catch ex As Exception
                MsgBox(ex.Message, MsgBoxStyle.Exclamation, Me.Text)
            End Try
        End With
    End Sub

    Public Sub UpdateTextCtrl(ByVal CtrlList As Object)
        With vsfAS
            For Each Ctrl As Control In CtrlList
                For i As Int32 = 1 To .Rows.Count - 1
                    If .GetData(i, vASCtrlName) = Ctrl.Name Then
                        Ctrl.Text = GetVal(.GetData(i, vASCtrlValue), "")
                    End If
                Next
            Next
        End With
    End Sub

    Private Sub UpdateChkBoxCtrl(ByVal CtrlList As Object)
        With vsfAS
            For Each Ctrl As CheckBox In CtrlList
                For i As Int32 = 1 To .Rows.Count - 1
                    If .GetData(i, vASCtrlName) = Ctrl.Name Then
                        Ctrl.Checked = GetVal(.GetData(i, vASCtrlValue), False)
                    End If
                Next
            Next
        End With
    End Sub

    Private Sub UpdateRadioCtrl(ByVal CtrlList As Object)
        With vsfAS
            For Each Ctrl As RadioButton In CtrlList
                For i As Int32 = 1 To .Rows.Count - 1
                    If .GetData(i, vASCtrlName) = Ctrl.Name Then
                        Ctrl.Checked = GetVal(.GetData(i, vASCtrlValue), False)
                    End If
                Next
            Next
        End With
    End Sub

    Private Sub UpdateC1DateCtrl(ByVal CtrlList As Object)
        With vsfAS
            For Each Ctrl As C1.Win.C1Input.C1DateEdit In CtrlList
                For i As Int32 = 1 To .Rows.Count - 1
                    If .GetData(i, vASCtrlName) = Ctrl.Name Then
                        setC1Date(Ctrl, GetVal(.GetData(i, vASCtrlValue), ""))
                    End If
                Next
            Next
        End With
    End Sub
    Public Sub FillCombo()
        Call bindCombo(cboCity, "City", "City", "select  distinct City from MStSCC where City <>'' order by city")
        Call bindCombo(cboState, "STATE", "STATE", "select  distinct STATE from MStSCC where state <>''  order by STATE")
        Call bindCombo(cboCountry, "COUNTRY", "COUNTRY", "select  distinct COUNTRY from MStSCC where country  <> '' order by COUNTRY")
        Call bindCombo(cboPartyGrp, strFrmPfx & "GroupCode", strFrmPfx & "GroupCode", "select  distinct " & strFrmPfx & "GroupCode from " & "MSt" & strFrmPfx & "Group where " & strFrmPfx & "GroupCode  <> '' and InActive = 'N' and Authorised = 'Y' order by " & strFrmPfx & "GroupCode")
        Call bindCombo(cboSeries, strFldSeries, strFldSeries, "select Distinct " & strFldSeries & " from " & strTblName & " where " & strCodeName & " <> '-' order by " & strFldSeries)
        cboCity.SelectedIndex = -1
        cboState.SelectedIndex = -1
        cboCountry.SelectedIndex = -1
        cboPartyGrp.SelectedIndex = -1
    End Sub
    Private Sub setC1Date(ByVal ctrl As C1.Win.C1Input.C1DateEdit, ByVal strDt As String)
        If strDt = "" Then

            ctrl.ValueIsDbNull = True

        Else
            ctrl.Value = CDate(strDt)
        End If
    End Sub

    Private Sub btnCopyFrom_Click_1(sender As System.Object, e As System.EventArgs) Handles btnCopyFrom.Click
        Dim strSQL As String, strFMT As String
        strFMT = ""
        strSQL = " select  " & strIDName & ", " & strCodeName & " ," & strFrmPfx & "Name" & " "
        strSQL = strSQL & " from " & strTblName & " where " & strIDName & " > 0 "
        strSQL = strSQL & " order by " & strCodeName & ""

        Dim frmDisplay As New frmUtlSingleSel

        With frmDisplay
            .Text = "Select the " & strFrmMsg & " Name to Copy"
            Call .FillViewVsf(Me, strSQL, txtCopyItem, 0, 0)
            .ShowDialog()
            .vsfDet.Focus()
        End With

        Call FillDocDetails(Val(txtCopyItem.Text), True)
        PnlCopyFrom.Visible = True
        'enableDisableFlds(True)
    End Sub

    Public Sub UpdateTextColor(ByVal CtrlList As Object)
        For Each Ctrl As Control In CtrlList
            Ctrl.ForeColor = Color.Black
        Next
        btnCopyFrom.ForeColor = Color.Black
        btnConPer.ForeColor = Color.Black
        btnAddr.ForeColor = Color.Black
    End Sub

    Private Sub txtAddress_TextChanged(sender As Object, e As System.EventArgs) Handles txtAddress.TextChanged
        txtAddress.ScrollBars = ScrollBars.None
        TextBoxSizeChanged(txtAddress, e)
    End Sub

    Private Sub svtBtnCtrl_btnOptionSelected(argOptionName As String, argBlnCtrlClick As Boolean, argBlnAltClick As Boolean) Handles svtBtnCtrl.btnOptionSelected
        Select Case argOptionName.ToUpper
            Case "SMS"
                If txtPartyCode.Text = "" Then Exit Sub
                With New frmTrnPartySMS
                    .intDocId = intDocID
                    .initScreen(txtCustID.Text)
                    .ShowDialog()
                End With
            Case "APPROVE"
                If Trim(txtPartyCode.Text) = "" Then Exit Sub
                If Not chkUserRights(strRights, chkRightsPos.AuthRights, Me.Text) Then Exit Sub
                If UCase(lblApproved.Text) <> "APPROVED" Then
                    If MsgBox("Do you want to Approve this " & Replace(strFormCaption, "Master", "") & " : '" & txtPartyCode.Text & "' ?", vbQuestion + vbYesNo + vbDefaultButton1, Me.Text) = vbYes Then
                        clsSVT.execSQLStatement("Update " & strTblName & " set Authorised='Y', AuthBy='" & gUserCode & "', AuthOn=" & getDateFormatInCond(getSystemDate) & " where " & strIDName & "=" & Val(txtCustID.Text))
                        'for audit track
                        Call RecordAuditLog(strFormCaption, Val(txtCustID.Text), UserActions.EditAction, strTblName, strAuditHdrSQL, strAuditDtlSQL)
                        lblApproved.Text = "APPROVED"
                        'for audit track
                        Call RecordAuditLog(strFormCaption, Val(txtCustID.Text), UserActions.EditAction, strTblName, strAuditHdrSQL, strAuditDtlSQL)
                        lblApproved.ForeColor = Color.Green
                        Exit Sub
                    End If
                ElseIf UCase(lblApproved.Text) <> "UN-APPROVED" Then
                    If MsgBox(Replace(strFormCaption, "Master", "") & " : '" & txtPartyCode.Text & "' is already approved." & vbCrLf & "Do you want to revoke it?", vbQuestion + vbYesNo + vbDefaultButton1, Me.Text) = vbYes Then
                        clsSVT.execSQLStatement("Update " & strTblName & " set Authorised='N', AuthBy='', AuthOn=NULL where " & strIDName & "=" & Val(txtCustID.Text))
                        Call RecordAuditLog(strFormCaption, Val(txtCustID.Text), UserActions.EditAction)
                        lblApproved.Text = "UN-APPROVED"
                        lblApproved.ForeColor = Color.FromArgb(192, 0, 0)
                        Exit Sub
                    End If
                End If
            Case "ENVELOPE PRINTING"
                If txtPartyCode.Text = "" Then Exit Sub
                Dim objFrm As New frmUtlEnv
                objFrm.strDocName = Me.Text
                objFrm.intPartyId = txtCustID.Text
                objFrm.Text = objFrm.Text & " - " & Me.Text
                objFrm.ShowDialog()

            Case "LABEL PRINT"
                If txtPartyCode.Text = "" Then Exit Sub
                Dim objFrm As New frmRptLblPrint
                objFrm.intDocID = intDocID
                'objFrm.strDocName = Me.Text
                'objFrm.intPartyId = txtCustID.Text
                ' objFrm.Text = objFrm.Text & " - " & Me.Text
                objFrm.ShowDialog()
        End Select
    End Sub

    Private Sub txtPartyName_Validated(sender As Object, e As EventArgs) Handles txtPartyName.Validated
        txtTallyAlias.Text = txtPartyName.Text
    End Sub

    Private Sub frmMstParty_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        txtSearch.Focus()
    End Sub

    Private Sub cboCity_Validating(sender As Object, e As CancelEventArgs) Handles cboCity.Validating
        If cboCity.Text = "" Then
            Exit Sub
        End If
        If Not (Mode = "A" Or Mode = "M") Then Exit Sub
        Dim blnFound As Boolean = False
        Dim intSCCID As Integer
        intSCCID = clsSVT.getDataLongValue("Select SCCID from MstSCC where City='" & cboCity.Text & "'", 0, blnFound)
        If Not blnFound Then
            Dim response As MsgBoxResult
            response = MsgBox("City Not found! Do you want to add city ??", vbYesNo, Me.Text)
            If response = vbYes Then
                cboCity.Text = ""
                Dim intSCCDocID As Integer
                intSCCDocID = clsSVT.getDataLongValue("select DocID from UtlTransactions where DocName='State City Country'", 0, False)
                Dim argfrm As New frmMstSCC

                argfrm.initForm(intSCCDocID, "MstSCC", "SCCID", "City", "State City Country", "City")
                frmMain.OpenForms(argfrm, "State City Country")


            Else
                Exit Sub
            End If

            'e.Cancel = True
            Exit Sub
        End If
        cboCountry.Text = clsSVT.getDataStringValue("Select Country from MstSCC where SCCID=" & intSCCID, 0, False)
        cboState.Text = clsSVT.getDataStringValue("Select State from MstSCC where SCCID=" & intSCCID, 0, False)
    End Sub

    Private Sub cboCountry_Validating(sender As Object, e As CancelEventArgs) Handles cboCountry.Validating
        Dim blnFound As Boolean
        Dim intSCCID As Integer
        intSCCID = clsSVT.getDataLongValue("Select SCCID from MstSCC where Country='" & cboCountry.Text & "'", 0, blnFound)
        If Not blnFound Then
            ' MsgBox("Country Not found in Master!", vbInformation, Me.Text)
            'e.Cancel = True
            Exit Sub
        End If
    End Sub

    Private Sub cboState_Validating(sender As Object, e As CancelEventArgs) Handles cboState.Validating
        Dim blnFound As Boolean
        Dim intSCCID As Integer
        intSCCID = clsSVT.getDataLongValue("Select SCCID from MstSCC where State='" & cboState.Text & "'", 0, blnFound)
        If Not blnFound Then
            'MsgBox("State Not found in Master!", vbInformation, Me.Text)

            'e.Cancel = True
            Exit Sub
        End If
    End Sub

    Private Sub lblSearchImage_Click(sender As Object, e As EventArgs) Handles lblSearchImage.Click

        If Not chkUserRights(strRights, chkRightsPos.ViewRights, Me.Text) Then Exit Sub
        txtSearch.Text = ""

        lblFrm.Text = "View:" & Me.Text
        txtAddress.Visible = True

        Dim StrQry As String, strCond As String = ""

        If vsfsearch.Visible = True Then
            vsfsearch.Visible = False
        Else
            Call setSearchGridHt("FULL")

            StrQry = "Select Top 30  " & strCodeName & "," & strFrmPfx & "Name"
            StrQry &= " from " & strTblName
            StrQry &= " order by 1"
            Call showWaitCursor()
            dtParty = clsSVT.getDATATable(StrQry)
            dvSearch = New DataView(dtParty)
            vsfsearch.DataSource = dvSearch
            Call showNormalCursor()
            vsfsearch.Refresh()
            vsfsearch.ScrollBars = ScrollBars.Both
            vsfsearch.AutoSizeCols()
        End If


    End Sub
    Public Sub setFLdValues(ByVal argMobile As String)
        Dim strMobileNo() As String
        strMobileNo = Split(argMobile, ",")
        For Each strmob As String In strMobileNo
            'vsfContact.Rows.Count = vsfContact.Rows.Count + 1
            'vsfContact.SetCellImage(vsfContact.Rows.Count - 1, vDel, My.Resources.cancel)
            'vsfContact.SetData(vsfContact.Rows.Count - 1, vTo, strmob)
            'vsfContact.SetData(vsfContact.Rows.Count - 1, vName, clsSVT.getDataStringValue("Select CName from MstContact where Mob1 ='" & strmob & "'", "", False))
        Next
    End Sub

    Private Sub txtRmks_TextChanged(sender As Object, e As EventArgs) Handles txtRmks.TextChanged
        Call TextBoxSizeChanged(txtRmks, e)
    End Sub

    Private Sub btnMstGrp_Click(sender As Object, e As EventArgs) Handles btnMstGrp.Click
        'Call frmMain.OpenGeneralForm(Replace(strFormCaption, "Master", "Group Master"))
        Call getMasterFlds("Mst" & GetVal(strFrmPfx & "Group"), cboPartyGrp.Text)
    End Sub

    Private Sub setAutoGenCode()
        If blnAutoGenCode Then
            cboSeries.Visible = True
            lblSeries.Visible = True
            lblPartyCode.ForeColor = gChildReadOnlyFore
            txtPartyCode.ReadOnly = True
            If Mode = "A" Or Mode = "M" Then
                cboSeries.Focus()
            End If
        Else
            cboSeries.Visible = False
            lblSeries.Visible = False
            lblPartyCode.ForeColor = gChildMandatoryFore
            txtPartyCode.ReadOnly = False
            If Mode = "A" Or Mode = "M" Then
                txtPartyCode.Focus()
            End If
        End If
    End Sub

    Private Sub cboSeries_Validating(sender As Object, e As CancelEventArgs) Handles cboSeries.Validating
        If Mode = "A" Then
            If Trim(cboSeries.Text) <> "" Then
                Call FillAutoGenItemNo()
                txtPartyCode.Focus()
            End If
        End If
    End Sub

    Public Sub FillAutoGenItemNo()
        showWaitCursor()
        intNo = 0
        If Trim(cboSeries.Text) <> "" Then
            intNo = clsSVT.getDataLongValue("Select Max(" & getSyntax("ISNULL0", strFldNo) & ")+1 from " & strTblName & " where " & strFldSeries & " ='" & Trim(cboSeries.Text) & "' and " & strIDName & "<>" & Val(txtCustID.Text), 0, False)
            If intNo = 0 Then intNo = 1
        End If
        txtNo.Text = intNo
        txtPartyCode.Text = Trim(cboSeries.Text) & Microsoft.VisualBasic.Right("000000" & intNo, 6)
        showNormalCursor()
    End Sub

    Private Sub cboSeries_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cboSeries.KeyPress
        If Mode = "A" Or Mode = "M" Then
            If e.KeyChar = "'" Then e.Handled = True

            e.KeyChar = UCase(e.KeyChar)
            If Not KeyValidate("String", e.KeyChar) Then
                e.Handled = True
            End If
        Else
            e.Handled = True
        End If
    End Sub

    Private Sub txtPartyCode_Validating(sender As Object, e As CancelEventArgs) Handles txtPartyCode.Validating
        If Trim(txtPartyCode.Text) = "" Then Exit Sub
        If Not blnAutoGenCode Then
            Dim strSQL As String
            StMsg("Validating " & strCodeName & "..")
            Try
                If clsSVT.getDataLongValue("select count(" & strCodeName & ") " & " from " & strTblName & " where " & strCodeName & "Clean = dbo.RemoveNonAlphaCharacters('" & Trim(txtPartyCode.Text) & "') and " & strIDName & "<>" & Val(txtCustID.Text), 0, False) > 0 Then
                    If MsgBox("Similar " & strCodeName & " already exists." & vbCrLf & "Do you want to view them?", vbInformation + vbYesNo + vbDefaultButton1, Me.Text) = vbYes Then
                        Dim frmDisplay As New frmUtlMstView
                        strSQL = "Select " & strCodeName & "," & strFrmPfx & "Name" & ", BAdd1, BAdd2, BAdd3, BAdd4" &
                                                " from " & strTblName & " where " & strCodeName & "Clean = dbo.RemoveNonAlphaCharacters('" & Trim(txtPartyCode.Text) & "')"
                        With frmDisplay
                            .Text = Me.Text
                            Call .FillViewVsf(Me, strSQL, "", strFrmPfx & "CODE " & "'" & txtPartyCode.Text & "' is similar to below " & Replace(strFormCaption, "Master", "") & " :", intDocID, Me.Text, False)
                            .ShowDialog()
                        End With
                        Exit Sub
                    End If
                End If
                StMsg()
            Catch ex As Exception
            End Try
        End If
    End Sub

    Private Sub lblAddress_DoubleClick(sender As Object, e As EventArgs) Handles lblAddress.DoubleClick
        If txtPartyCode.Text = "" Then Exit Sub
        Dim txtCustDtl As New TextBox
        txtCustDtl.Multiline = True
        If Mode = "M" Or Mode = "V" Then
            txtCustDtl.Text = clsSVT.getDataStringValue("Select CopyFlds from UtlPartyCopy where DocID=" & intDocID, "", False)
            If Trim(txtCustDtl.Text) <> "" Then
                Dim schemaTable As DataTable
                Dim myField As DataRow
                Dim myProperty As DataColumn
                schemaTable = clsSVT.getDATATable("Select * from " & strTblName & " where " & strIDName & "=" & txtCustID.Text)
                For Each myField In schemaTable.Rows
                    For Each myProperty In schemaTable.Columns
                        txtCustDtl.Text = Replace(txtCustDtl.Text, UCase("<<" & myProperty.ColumnName & ">>"), GetVal(myField(myProperty).ToString(), ""))
                    Next
                Next
                Clipboard.SetText(Trim(txtCustDtl.Text))
                MsgBox("Address copied to clipboard.", MsgBoxStyle.Information, Me.Text)
                Exit Sub
            End If
        End If

        txtCustDtl.Text = txtPartyName.Text
        If txtBAdd1.Text <> "" Then txtCustDtl.Text = txtCustDtl.Text & vbCrLf & txtBAdd1.Text
        If txtBAdd2.Text <> "" Then txtCustDtl.Text = txtCustDtl.Text & vbCrLf & txtBAdd2.Text
        If txtBAdd3.Text <> "" Then txtCustDtl.Text = txtCustDtl.Text & vbCrLf & txtBAdd3.Text
        If txtBadd4.Text <> "" Then txtCustDtl.Text = txtCustDtl.Text & vbCrLf & txtBadd4.Text
        If cboCity.Text <> "" Then txtCustDtl.Text = txtCustDtl.Text & vbCrLf & cboCity.Text
        If cboState.Text <> "" Then txtCustDtl.Text = txtCustDtl.Text & vbCrLf & cboState.Text
        If txtPincode.Text <> "" Then txtCustDtl.Text = txtCustDtl.Text & vbCrLf & txtPincode.Text

        Clipboard.SetText(txtCustDtl.Text)
        MsgBox("Address copied to clipboard.", MsgBoxStyle.Information, Me.Text)
    End Sub

    Private Sub lblAddress_Click(sender As Object, e As EventArgs) Handles lblAddress.Click
        If Control.ModifierKeys = Keys.Control Then
            With frmUtlFldsTemplate
                .blnAddressTemplate = True
                .Text = "Copy Formatting -" & strFormCaption
                .strMstTblName = strTblName
                .intDocID = intDocID
                .ShowDialog()
            End With
        End If
    End Sub


    Private Sub btnAlert_Click(sender As Object, e As EventArgs) Handles btnAlert.Click
        If txtPartyCode.Text = "" Then Exit Sub
        With frmMstPartyAlert

            .strfrmCaption = strFrmMsg
            .txtPartyCode.Text = txtPartyCode.Text
            .lblPartyName.Text = txtPartyName.Text
            .initForm(intDocID, Val(txtCustID.Text))
            .strRights = strRights
            .strfrmCaption = strFrmMsg
            .ShowDialog()
            btnAlert.Text = "Alert Message (" & clsSVT.getDataLongValue("Select Count(*) from MstPartyAlertMessage where PartyDocID=" & intDocID & " and PartyID=" & Val(txtCustID.Text).ToString, 0, False) & ")"
        End With
    End Sub

    Private Sub btnActivities_Click(sender As Object, e As EventArgs) Handles btnActivities.Click
        If txtPartyCode.Text = "" Then Exit Sub
        With frmMstPartyActivities
            .strfrmCaption = strFrmMsg
            .strRights = strRights
            .txtPartyCode.Text = txtPartyCode.Text
            .lblPartyName.Text = txtPartyName.Text
            .strfrmCaption = strFrmMsg
            .initForm(intDocID, Val(txtCustID.Text))
            .ShowDialog()
            btnActivities.Text = "Activities (" & clsSVT.getDataLongValue("Select Count(*) from MstPartyActivity where PartyDocID=" & intDocID & " and PartyID=" & Val(txtCustID.Text), 0, False).ToString & ")"
            Call fillContactPersons()
        End With
    End Sub

    Private Sub fillAttachFiles(ByVal argID As Long)
        Dim Rs As oledb.oledbDataReader
        Dim strsql As String
        Dim introw As Integer
        Dim i As Integer

        strsql = "Select * from UtlAttachFiles"
        strsql = strsql & " where DocID = " & intDocID & " and DocValueID = " & argID
        Rs = clsSVT.getDataReaderFromSQL(strsql)

        With Rs
            vsfFiles.Rows.Count = 0

            Do While .Read
                vsfFiles.Rows.Count = vsfFiles.Rows.Count + 1
                introw = vsfFiles.Rows.Count - 1
                i = introw
                vsfFiles.SetData(i, vFSlno, GetVal(Rs("DocSlno"), ""))
                vsfFiles.SetData(i, vFName, GetVal(Rs("AFileName"), ""))
                vsfFiles.SetData(i, vFDesc, GetVal(Rs("FileDesc"), ""))
            Loop
            .Close()
        End With
    End Sub

    Private Sub vsfFiles_BeforeEdit(sender As Object, e As RowColEventArgs) Handles vsfFiles.BeforeEdit
        e.Cancel = True
    End Sub

    Private Sub vsfFiles_MouseHoverCell(sender As Object, e As EventArgs) Handles vsfFiles.MouseEnterCell
        If vsfFiles.Rows.Count > 1 Then
            With vsfFiles
                tt.SetToolTip(vsfFiles, .GetData(.Row, vFName) & "/" & .GetData(.Row, vFDesc))
            End With
        End If
    End Sub

    Private Sub vsfFiles_DoubleClick(sender As Object, e As EventArgs) Handles vsfFiles.DoubleClick
        Dim strFile As String
        If vsfFiles.Row < 0 Then Exit Sub
        strFile = vsfFiles.GetData(vsfFiles.Row, vFName)
        '  MsgBox(strFile.Substring(2, 1))
        If strFile.Substring(1, 1) = ":" Then
            MessageBox.Show("Please save and then Open the file...!", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If
        strFile = gstrRptPath & "TrnDocFiles\" & strFile
        Try
            Dim p As New System.Diagnostics.Process
            Dim s As New System.Diagnostics.ProcessStartInfo(strFile)
            s.UseShellExecute = True
            s.WindowStyle = ProcessWindowStyle.Normal
            p.StartInfo = s
            p.Start()
        Catch ex As Exception
            MessageBox.Show("File '" & strFile & "' could not be found!", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnFileAttach_ClickButtonArea(Sender As Object, e As MouseEventArgs) Handles btnFileAttach.ClickButtonArea
        If Mode = "A" Then Exit Sub
        If Trim(txtPartyCode.Text) = "" Then Exit Sub
        If Not chkUserRights(strRights, chkRightsPos.ViewRights, Me.Text) Then Exit Sub
        With frmUtlFileControl
            .strRights = strRights
            .argOwnerCaption = Me.Text
            .argDocID = intDocID
            .argDocValueID = Val(txtCustID.Text)
            .StartPosition = FormStartPosition.CenterScreen
            .ShowDialog()
            btnFileAttach.Text = "File Attachment (" & clsSVT.getDataLongValue("Select count(*) from UtlAttachFiles Where DocID = " & intDocID & " and DocValueID = '" & Val(txtCustID.Text) & "'", 0, False) & ")"
            fillAttachFiles(Val(txtCustID.Text))
        End With
    End Sub

    Private Sub vsfHdr_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles vsfHdr.MouseDoubleClick
        Dim ht = vsfHdr.HitTest
        With vsfHdr
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

    Public Sub vsfHdr_OwnerDrawCell(sender As System.Object, e As C1.Win.C1FlexGrid.OwnerDrawCellEventArgs) Handles vsfHdr.OwnerDrawCell
        If sender.Rows(e.Row).Style IsNot Nothing Then
            If sender.Rows(e.Row).Style.Name = "StyleClr" & e.Row Then
                e.Style = sender.Styles("StyleClr" & e.Row)
            End If
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

    Private Sub vsfHdr_AfterResizeColumn(sender As Object, e As RowColEventArgs) Handles vsfHdr.AfterResizeColumn
        If vsfHdr.Cols(vOthFldCaption).Width >= vsfHdr.Width Then
            vsfHdr.Cols(vOthFldCaption).Width = vsfHdr.Width - 20
        End If
    End Sub

    Private Sub txtSearch_TextChanged(sender As Object, e As EventArgs) Handles txtSearch.TextChanged

    End Sub

    Private Sub frmMstParty_Layout(sender As Object, e As LayoutEventArgs) Handles Me.Layout

    End Sub

    Private Sub txtPartyCode_LostFocus(sender As Object, e As EventArgs) Handles txtPartyCode.LostFocus
        If Mode = "A" Then txtPartyName.Text = txtPartyCode.Text
    End Sub



    'Private Sub getProperties()
    '    Dim str As String = ""
    '    For Each controlProperty As System.Reflection.PropertyInfo In Reflection.pro properties
    '        If controlProperty.Name = "Text" AndAlso controlProperty.PropertyType = GetType(String) Then
    '            Dim propertyValue As String = GetPropertyValue(controlProperty.Name, Controls)
    '            Response.Write(propertyValue)
    '            Response.Write("<br><br>")
    '        End If
    '    Next
    'End Sub
End Class