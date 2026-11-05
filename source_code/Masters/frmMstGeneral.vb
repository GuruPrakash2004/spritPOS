Imports System.ComponentModel
Imports C1.Win.C1FlexGrid

Public Class frmMstGeneral
    Dim Mode As String
    Dim intDocID As Integer
    Dim clsSVT As New svtLibrary
    Dim strTblName As String
    Dim strSHForm As String
    Dim strDtlTblName As String
    Dim strIDName As String
    Dim strCodeName As String
    Dim strFldCodeName As String
    Dim strFormCaption As String
    Dim strFldCaption As String
    Dim strFldName As String
    Dim strFldSeries As String
    Dim strFldNo As String
    Dim blnShowFileAttach As Boolean
    Dim strRights As String
    Dim intBtnColor As Integer
    Dim strCode As String = ""
    Dim blnFromIM As Boolean = False
    Dim blnAutoGenCode As Boolean = False
    Dim intNo As Integer
    Dim dvSearch As DataView
    Dim dtGeneral As DataTable
    Dim strFrmPfx As String
    Dim blnAuthReq As Boolean = False

    'for audit track
    Dim blnAuditTrack As Boolean = False
    Dim strAuditHdrSQL As String
    Dim strAuditDtlSQL As String

    'Consts for vsfFiles
    Const vFSlno = 0
    Const vFDesc = 1
    Const vFName = 2
    Dim svtTrn As oledb.oledbTransaction

    Private Sub frmMstGeneral_FormClosing(sender As Object, e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        If Mode = "A" Or Mode = "M" Then
            If MsgBox("Are you sure you want to close this form without saving?", MsgBoxStyle.Question + MsgBoxStyle.YesNo + vbDefaultButton2, "Confirmation..") = MsgBoxResult.No Then
                e.Cancel = True
                Exit Sub
            End If
        End If
    End Sub

    Private Sub frmMstGeneral_KeyUp(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyUp
        Dim blnFnKey As Boolean = False
        If e.KeyCode = Keys.F9 And e.Control = True Then
            If vsfHdr.Visible Then
                Call initFormVsfDet(Me, Me.Text, vsfHdr, "Hdr")
            End If
            MsgBox("Restored Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
        If e.KeyCode = Keys.F8 And e.Control = True Then
            If vsfHdr.Visible Then
                Call SaveActFormVsfDet(Me, Me.Text, "Hdr", vsfHdr)
            End If
            MsgBox("Saved Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If

        If Mode <> "A" Then
            If e.KeyCode = Keys.A And e.Control = True Then
                Call btnFileAttach_ClickButtonArea(sender, Nothing)
            End If
        End If

        If (e.Control) Or blnFnKey Then
            Call svtBtnCtrl.btnCtrlKeyUp(sender, e)
        End If
        'Call bindCombo(cboSeries, strFrmPfx & "Series", strFrmPfx & "Series", "Select Distinct " & strFrmPfx & "Series from " & strTblName & " where " & strFrmPfx & "Series is not null and " & strFrmPfx & "Series <>'-' order by 1")
    End Sub
    Private Sub fillCombo()
        Call bindCombo(cboSeries, strFrmPfx & "Series", strFrmPfx & "Series", "Select Distinct " & strFrmPfx & "Series from " & strTblName & " where " & strFrmPfx & "Series is not null and " & strFrmPfx & "Series <>'-' order by 1")
        cboSeries.SelectedIndex = -1
    End Sub
    Private Sub frmMstGeneral_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        'txtSearch.Focus()
        strRights = getUserRights(GetMenuId(Me.Text))
        Call setSearchGridHt("")
        Me.Tag = strTblName

        Call formProp()
        fillFormProperties(Me, vsfHdr)
        lblFrm.ForeColor = Color.White
        lblSearch.ForeColor = Color.White
        svtBtnCtrl.FlowLayoutPanel1.BackColor = Color.Transparent
        svtBtnCtrl.BackColor = Color.Transparent

        vsfHdr.Styles.EmptyArea.BackColor = Color.Transparent
        vsfHdr.BackColor = Color.Transparent
        tbAdnlFlds.TabPages(0).BackColor = gChildFormBackColor

        If Not chkUserRights(strRights, chkRightsPos.AddRights, Me.Text, False) Then svtBtnCtrl.btnNew.Visible = False
        If Not chkUserRights(strRights, chkRightsPos.EditRights, Me.Text, False) Then svtBtnCtrl.btnEdit.Visible = False
        If Not chkUserRights(strRights, chkRightsPos.DelRights, Me.Text, False) Then svtBtnCtrl.btnDelete.Visible = False
        If Not chkUserRights(strRights, chkRightsPos.opt1Rights, Me.Text, False) Then svtBtnCtrl.btnOptions.Visible = False

        FillDocDetails(Val(txtID.Text))
        If vsfHdr.Visible Then
            Call initFormVsfDet(Me, Me.Text, vsfHdr, "HDR")
        End If

        'Call initFormVsfDet(Me, Me.Text, vsfFnlYr, "FnlYr")
        'Call bindCombo(cboSeries, strFrmPfx & "Series", strFrmPfx & "Series", "Select Distinct " & strFrmPfx & "Series from " & strTblName & " where " & strFrmPfx & "Series is not null and " & strFrmPfx & "Series <>'-' order by 1")
        'cboSeries.SelectedIndex = -1

        Call ChangeFontSize(Me)
        Me.WindowState = FormWindowState.Maximized
        lblFrm.Font = New Font("Microsoft Sans Serif", 14, FontStyle.Bold)
        lblSearch.Font = New Font("Segoe UI", 9.25, FontStyle.Bold)
    End Sub
    Private Sub formProp()
        svtBtnCtrl.btnOptions.Visible = True
        svtBtnCtrl.btnPrint.Visible = False
    End Sub
    Public Sub New(Optional ByVal argCode As String = "", Optional ByVal argRights As String = "")

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        If Trim(argCode) <> "" Then
            strCode = argCode
        End If

        If Trim(argRights) <> "" Then
            strRights = argRights
        Else
            strRights = getUserRights(GetMenuId(Me.Text))
        End If

    End Sub

    Private Sub initScreen()
        Dim intMenuID As Integer
        Dim strSQL As String

        intMenuID = clsSVT.getDataLongValue("Select MenuItemID from MstMenu where DocId = " & intDocID, 0, False)

        Call InitAddnlFldGrids(Me, vsfHdr, "Header", intDocID, tbAdnlFlds)
        txtInActiveRem.Visible = False
        Call clearFlds()
        Call InsertDataTbl()
        strSQL = "Select Max(" & strIDName & ") from " & strTblName
        txtID.Text = clsSVT.getDataLongValue(strSQL, 0, False)

        Me.Tag = strTblName

        Dim ctrl As Control = Nothing
        KeyPreview = True
        initGridStyles(Me)
        With vsfFiles
            .Cols.Count = 3
            .Rows.Count = 0
            .Cols(vFSlno).Visible = False
            .Cols(vFName).Visible = False
        End With

        initToolTipColours(lblToolTip, pnlToolTip)
        dtGeneral = clsSVT.getDATATable("Select " & strCodeName & "," & strFldCodeName & " from " & strTblName & " where  " & strIDName & ">0 and InActive<>'Y' order by 1,2")
        LoadSearchItems()
        'Call chkSplRights_DOC(Me, intDocID, strRights, vsfHdr)
        If intDocID = 1110 Then
            svtBtnCtrl.addOption("Arrange Group Slno")
        End If
        If intDocID = clsSVT.getDataLongValue("Select DocID from UtlTransactions where HdrTblName = 'MSTPKG'", 0, False) Then
            svtBtnCtrl.addOption("Arrange Pkg Slno")
        End If
        If blnAuthReq Then
            svtBtnCtrl.addOption("Approve")
        End If
    End Sub

    Private Sub LoadSearchItems()
        dvSearch = New DataView(dtGeneral)
        vsfSearch.DataSource = dvSearch
        vsfSearch.Refresh()
        vsfSearch.ScrollBars = ScrollBars.Both
        vsfSearch.AutoSizeCols()
    End Sub
    Private Sub clearFlds()
        txtCode.Text = ""
        txtSearch.Text = ""
        txtName.Text = ""
        txtID.Text = ""
        chkInactive.Enabled = False
        chkInactive.Checked = False
        txtInActiveRem.Text = ""
        cboSeries.Text = ""

        pnlSearch.Visible = True
        txtSearch.Visible = True
        lblApproved.Visible = False
        txtSearch.Focus()
        '--7 Begin OthGRID
        Call ClearAddnlGrid(Me)

        vsfFiles.Rows.Count = 0
        btnFileAttach.Text = "File Attachment"
    End Sub

    '# A - Insertion of Data to avoid link problem
    Private Sub InsertDataTbl()

        Dim recdummy As oledb.oledbDataReader
        Dim strSQL As String

        strSQL = "select * from " & strTblName & " where " & strIDName & " = 0"
        recdummy = clsSVT.getDataReaderFromSQL(strSQL)
        If recdummy.HasRows = False Then
            strSQL = " Insert into " & strTblName & " ("
            strSQL = strSQL & strIDName & "," & strCodeName & "," & strFldCodeName & "," & " InActive,InActiveRmks,"
            strSQL = strSQL & "AddedBy,AddedOn,ChangedBy,ChangedOn)"
            strSQL = strSQL & " values("
            strSQL = strSQL & "0,'-','-', "
            strSQL = strSQL & "'N','',"
            strSQL = strSQL & "'',"
            strSQL = strSQL & "'" & getDateFormatInInsert(getSystemDateTime, True) & "',"
            strSQL = strSQL & "'',"
            strSQL = strSQL & "'" & getDateFormatInInsert(getSystemDateTime, True) & "')"

            Call clsSVT.execSQLStatement(strSQL)
        End If
        recdummy.Close()

    End Sub
    Private Sub AddProc()

        Call clearFlds()
        Mode = "A"
        If blnAutoGenCode Then
            Call bindCombo(cboSeries, strFldSeries, strFldSeries, "select Distinct " & strFldSeries & " from " & strTblName & " where " & strCodeName & " <> '-' and " & strFldSeries & "<>'' order by " & strFldSeries)
            cboSeries.Text = ""
        End If
        vsfSearch.Visible = False
        vsfSearch.SendToBack()
        pnlSearch.Visible = False
        Call fillVsfHFTDefValues(Me)
        Call enableDisableFlds(True)
        StMsg()
    End Sub
    Private Sub enableDisableFlds(ByVal argFlag As Boolean)

        txtCode.Enabled = argFlag
        chkInactive.Enabled = argFlag
        txtName.Enabled = argFlag
        If chkInactive.Checked And Mode = "A" Then
            txtInActiveRem.Visible = True
        Else
            txtInActiveRem.Visible = False
        End If
        If chkInactive.Checked And Mode = "M" Then
            txtInActiveRem.Visible = True
            txtInActiveRem.Enabled = True
        End If

        If chkInactive.Checked And Mode = "V" Then
            txtInActiveRem.Enabled = False
            txtInActiveRem.Visible = True
        End If
        Lock(cboSeries, argFlag)

        If Mode = "A" Then
            PnlAttach.Enabled = False
        Else
            PnlAttach.Enabled = True
        End If
    End Sub

    Private Sub setSearchGridHt(ByVal argFlag As String)
        If argFlag = "FULL" Then
            vsfSearch.Visible = True
            vsfSearch.BringToFront()
            pnlSearch.BringToFront()
        Else
            vsfSearch.Visible = False
        End If
    End Sub

    Private Function ValidFields() As Boolean
        ValidFields = False
        If Mode = "V" Then Exit Function
        Dim blnFound As Boolean
        txtCode.Focus()
        StMsg("Validating Data.....")
        If Not IsNothing(vsfHdr.Editor) Then
            If vsfHdr.Editor.Text <> "" Then
                vsfHdr.SetData(vsfHdr.Row, vOthFldDataValue, vsfHdr.Editor.Text)
            End If
        End If
        ValidFields = False

        If blnAutoGenCode = True Then
            If Trim(cboSeries.Text) = "" Then
                MsgBox("Series should not be blank!", MsgBoxStyle.Information, Me.Text)
                cboSeries.Focus()
                Exit Function
            End If
            Call FillAutoGenItemNo()
        End If


        If Trim(txtCode.Text) = "" Then
            MsgBox(lblCode.Text & " should not be blank!", MsgBoxStyle.Exclamation, Me.Text)
            txtCode.Focus()
            Exit Function
        End If
        If Trim(txtName.Text) = "" Then
            MsgBox(lblName.Text & " should not be blank!", MsgBoxStyle.Information, Me.Text)
            txtName.Focus()
            Exit Function
        End If

        If Mode = "A" Then txtID.Text = clsSVT.getDataLongValue("Select max(" & strIDName & ") + 1 from " & strTblName, 1, False).ToString
        Call clsSVT.getDataStringValue("Select " & strCodeName & " from " & strTblName _
                & " where " & strCodeName & " ='" & Trim(txtCode.Text) & "' and " & strIDName & " <> " & Val(txtID.Text), "", blnFound)
        If blnFound Then
            MsgBox(strFldCaption & " already exists..", MsgBoxStyle.Information, Me.Text)
            txtCode.Text = ""
            txtCode.Focus()
            Exit Function
        End If

        If Not validOthFLds(Me, strTblName, strIDName, Val(txtID.Text)) Then
            Exit Function
        End If

        If chkInactive.Checked And Trim(txtInActiveRem.Text) = "" Then
            MsgBox("Please enter remarks for why the record is inactive!", MsgBoxStyle.Information, Me.Text)
            txtInActiveRem.Focus()
            Exit Function
        End If

        ValidFields = True
    End Function

    Private Function updateLed(argsvt As oledb.oledbTransaction) As Boolean
        updateLed = False
        Dim intAcLedID As Int32
        Dim intLedGroupId As Int32
        Dim strSQL As String = ""

        If gblnVoucherPresent = False Then Return True
        If strSHForm = "LED" Or strSHForm = "LEDGER" Then
            intLedGroupId = clsSVT.getDataLongValue("select LedGroupID from " & strTblName & " where " & strIDName & "=" & Val(txtID.Text) & "", 0, False, argsvt)
            If Mode = "A" Then
                intAcLedID = clsSVT.AutoIncrementedNum("AcLedID", "MstAcLed", "", argsvt)
                strSQL = "Insert into MstAcLed "
                strSQL &= " (AcLedID, AcLedName, LedType, LedMstID, LedGroupId, MstDocID) "
                strSQL &= " Values ("
                strSQL &= intAcLedID & ","
                strSQL &= "'" & Trim(txtCode.Text) & "',"
                strSQL &= "'L',"
                strSQL &= Val(txtID.Text) & ","
                strSQL &= intLedGroupId & ", " & intDocID & ")"
            Else
                intAcLedID = clsSVT.getDataLongValue("select AcLedID from MstAcLed where MstDocID = " & intDocID & " and LedMstID = " & Val(txtID.Text), 0, False, argsvt)
                strSQL = " Update MstAcLed Set "
                strSQL &= " LedGroupId = " & intLedGroupId
                strSQL &= " Where AcLedID = " & intAcLedID
            End If
        End If

        If strSQL <> "" AndAlso Not clsSVT.execSQLStatement(strSQL, argsvt) Then
            Exit Function
        End If
        updateLed = True
    End Function

    Private Function saveDetails() As Boolean
        Dim strSQL As String = ""
        Dim strValues As String = ""
        Dim strFlds As String = ""
        saveDetails = False
        Dim intID As Long
        If Not (Mode = "A" Or Mode = "M") Then Exit Function
        If Mode = "A" Then
            intID = clsSVT.AutoIncrementedNum(strIDName, strTblName)
            txtID.Text = intID
            strSQL = " Insert into " & strTblName & " ("
            strSQL = strSQL & strIDName & "," & strCodeName & "," & strFldCodeName & "," & " InActive,InActiveRmks,"
            strSQL = strSQL & "AddedBy,AddedOn, " & strFldSeries & "," & strFldNo
            strSQL = strSQL & ", Authorised,AuthBy,AuthOn"

            '--4 begin OthGRID
            Call GetInsertString(Me, strValues, strFlds)

            strSQL = strSQL & strFlds & ") values ("
            strSQL = strSQL & intID & ","
            strSQL = strSQL & "'" & txtCode.Text & "', "
            strSQL = strSQL & "'" & txtName.Text & "', "
            strSQL = strSQL & "'" & getCheckString(chkInactive) & "',"
            strSQL = strSQL & "'" & Trim(GetVal(txtInActiveRem.Text, "")) & "',"
            strSQL = strSQL & "'" & gUserCode & "',"
            strSQL = strSQL & "'" & getDateFormatInInsert(getSystemDateTime, True) & "',"
            strSQL = strSQL & "'" & Trim(cboSeries.Text) & "',"
            strSQL = strSQL & Val(txtNo.Text)
            If Not blnAuthReq Then
                strSQL = strSQL & ", 'Y','" & gUserCode & "'," & getDateFormatInCond(getSystemDate) & ""
            Else
                strSQL = strSQL & ", 'N','',Null"
            End If
            '--5 begin OthGRID
            strSQL = strSQL & strValues
        Else

            strSQL = "Update  " & strTblName & " set "
            strSQL = strSQL & strFldName & "  = '" & txtCode.Text & "',"
            strSQL = strSQL & strFldCodeName & "  = '" & txtName.Text & "',"
            strSQL = strSQL & " InActive = '" & getCheckString(chkInactive) & "',"
            strSQL = strSQL & " InActiveRmks = '" & Trim(GetVal(txtInActiveRem.Text, "")) & "',"
            strSQL = strSQL & " ChangedBy = '" & gUserCode & "',"
            strSQL = strSQL & " ChangedOn = '" & getDateFormatInInsert(getSystemDateTime, True) & "'"

            '--3 begin OthGRID
            Call GetUpdateString(Me, strFlds)
            strSQL = strSQL & strFlds

            strSQL = strSQL & " where " & strIDName & " =  " & Val(txtID.Text)
        End If
        strSQL = Replace(Replace(strSQL, vbCrLf, ""), vbCr, "")
        saveDetails = False

        svtTrn = svtConn.BeginTransaction(IsolationLevel.ReadCommitted)
        Try
            If clsSVT.execSQLStatement(strSQL, svtTrn) Then
                intID = Val(txtID.Text)
                'save attachments

                If Not updateLed(svtTrn) Then
                    svtTrn.Rollback()
                    Exit Function
                End If

                'for audit track
                If blnAuditTrack Then
                    Call RecordAuditLog(strFormCaption, Val(txtID.Text), IIf(Mode = "A", UserActions.AddAction, UserActions.EditAction), strTblName, strAuditHdrSQL, strAuditDtlSQL, svtTrn)
                End If

                If Mode = "A" Then
                    MsgBox(lblCode.Text & " '" & Trim(txtCode.Text) & "' added successfully.", vbInformation, Me.Text)
                Else
                    MsgBox("Updated successfully.", vbInformation, Me.Text)
                End If
                svtTrn.Commit()
                saveDetails = True
                StMsg("Status")
            End If
            Call FillDocDetails(txtID.Text)
        Catch ex As Exception
            MsgBox("Error during save" & vbCrLf & ex.Message, MsgBoxStyle.Critical, Me.Text)
            svtTrn.Rollback()
        End Try
        'svtTrn.Dispose()
    End Function

    Public Sub FillDocDetails(ByVal argID As Long)
        Dim RsTemp As oledb.oledbDataReader
        Dim strSQL As String
        Dim i As Integer, X As Integer

        If argID = 0 Then Exit Sub

        '--8 begin OthGRID
        Call ClearAddnlGrid(vsfHdr)

        strSQL = "Select * from " & strTblName
        strSQL = strSQL & " where " & strIDName & " = " & argID
        RsTemp = clsSVT.getDataReaderFromSQL(strSQL)

        With RsTemp
            If .Read() Then
                txtID.Text = argID
                txtCode.Text = GetVal(RsTemp(strCodeName), "")
                txtName.Text = GetVal(RsTemp(strFldCodeName), "")

                cboSeries.Text = GetVal(RsTemp(strFldSeries), "")
                Call ApplyChkToCheckBox(chkInactive, GetVal(!Inactive, "N"))
                txtInActiveRem.Text = GetVal(RsTemp("InActiveRmks"), "")


                '--9 begin OthGRID
                For i = 0 To vsfHdr.Rows.Count - 1
                    For X = 0 To .FieldCount - 1
                        If UCase(.GetName(X)) = UCase(Trim(vsfHdr.GetData(i, vOthFldCode))) Then
                            If Trim(vsfHdr.GetData(i, vOthFldCmbTblName)) = "" Then
                                vsfHdr.SetData(i, vOthFldDataValue, GetVal(.GetValue(X), ""))
                            Else
                                Select Case UCase(vsfHdr.GetData(i, vOthFldDataType))
                                    Case "NUMERIC", "LIST NUMERIC"
                                        vsfHdr.SetData(i, vOthFldDataValue,
                                        clsSVT.getDataStringValue("Select " & vsfHdr.GetData(i, vOthFldCmbDisplayCols) & " from " &
                                        vsfHdr.GetData(i, vOthFldCmbTblName) & " where " &
                                        vsfHdr.GetData(i, vOthFldCmbStorageFldName) & " = " & GetVal(.GetValue(X), 0), "", False))
                                    Case Else
                                        vsfHdr.SetData(i, vOthFldDataValue,
                                        clsSVT.getDataStringValue("Select " & vsfHdr.GetData(i, vOthFldCmbDisplayCols) & " from " &
                                        vsfHdr.GetData(i, vOthFldCmbTblName) & " where " &
                                        vsfHdr.GetData(i, vOthFldCmbStorageFldName) & " = '" & GetVal(.GetValue(X), "") & "'", "", False))
                                End Select
                                'vsfHdr.SetData(i, vOthFldDataValue,
                                '    clsSVT.getDataStringValue("Select " & vsfHdr.GetData(i, vOthFldCmbDisplayCols) & " from " &
                                '    vsfHdr.GetData(i, vOthFldCmbTblName) & " where " &
                                '    vsfHdr.GetData(i, vOthFldCmbStorageFldName) & " = " & GetVal(.GetValue(X), 0), "", False))
                            End If
                        End If
                    Next X
                Next
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

                If vsfHdr.Rows.Count > 0 Then
                    Call vsfGenAfterEditBfrCalTot(Me, vsfHdr, vsfHdr.Rows.Count - 1, "M")
                End If

            End If
            .Close()
        End With
        btnFileAttach.Text = "File Attachment (" & clsSVT.getDataLongValue("Select count(*) from UtlAttachFiles Where DocID = " & intDocID & " and DocValueID = '" & argID & "'", 0, False) & ")"
        Call fillAttachFiles(Val(txtID.Text))

        Call fillAttachFiles(Val(txtID.Text))
        Mode = "V"
        enableDisableFlds(False)
        pnlSearch.Visible = True
        txtSearch.Focus()
    End Sub

    Private Sub CalTotal()
        Call AddnlMSTFldCalTotal(Me, vsfHdr, 0, "", "", txtID.Text)
    End Sub

    Private Sub deleteDocument()

        If Val(txtID.Text) <= 0 Then Exit Sub
        If Not blnDeleteOK(strTblName, Val(txtID.Text), True, True) Then
            Exit Sub
        End If

        If MsgBox("Are you sure you want to delete the " & strFldCaption &
            " : '" & txtCode.Text & "'?", vbCritical + MsgBoxStyle.YesNo + vbDefaultButton2, " Delete Confirmation") = MsgBoxResult.Yes Then

            svtTrn = svtConn.BeginTransaction(IsolationLevel.ReadCommitted)
            Try
                'for audit track
                Call RecordAuditLog(strFormCaption, Val(txtID.Text), UserActions.DeleteAction, strTblName, strAuditHdrSQL, "", svtTrn)

                If Not clsSVT.execSQLStatement("Delete from " & strTblName & " where " & strIDName & " = " & Val(txtID.Text), svtTrn) Then
                    svtTrn.Rollback()
                    Exit Sub
                End If

                svtTrn.Commit()

                MsgBox("Deleted Successfully!", MsgBoxStyle.Information, Me.Text)
                Call clearFlds()


            Catch ex As Exception
                svtTrn.Rollback()
                Exit Sub
            End Try
            ' svtTrn.Dispose()
        Else
            Exit Sub

        End If
    End Sub

    Public Sub initForm(ByVal argDocID As Integer, ByVal argTblName As String, ByVal argIDName As String, ByVal argCodeName As String, ByVal argFldCodeName As String, ByVal argFormCaption As String, ByVal argFldCaption As String, Optional ByVal argFromIM As Boolean = False)

        txtSearch.Focus()
        If argFromIM Then
            blnFromIM = True
        End If

        If argDocID > 0 Then
            intDocID = argDocID

            Dim rs As oledb.oledbDataReader
            rs = clsSVT.getDataReaderFromSQL("Select * from UtlTransactions where DocID = " & argDocID)
            With rs
                If Not .HasRows Then
                    .Close()
                    Exit Sub
                End If

                .Read()
                strSHForm = GetVal(!DocShForm, "")
                strTblName = !HdrTblName
                strIDName = strSHForm & "ID"
                strCodeName = strSHForm & "Code"
                strFldCodeName = strSHForm & "Name"
                strFormCaption = !DocName
                lblFrm.Text = strFormCaption
                strFldName = argCodeName
                strFrmPfx = strSHForm
                strFldCaption = Trim(Replace(!Docname, "Master", "Name"))
                strFldSeries = strSHForm & "Series"
                strFldNo = strSHForm & "No"
                'for audit track
                If GetVal(!AuditLogTrack, "") = "Y" Then
                    blnAuditTrack = True
                    strAuditHdrSQL = GetVal(!AuditSQLHdr, "")
                    strAuditDtlSQL = GetVal(!AuditSQLDtl, "")
                End If
                If GetVal(!AuthReq, "N") = "Y" Then
                    blnAuthReq = True
                Else
                    blnAuthReq = False
                End If
                lblCode.Text = strSHForm & " Code"
                lblName.Text = strSHForm & " Name"

                If !FileAttachReq = "Y" Then
                    blnShowFileAttach = True
                Else
                    blnShowFileAttach = False
                End If

                If GetVal(!MstAutoGenCode, "N") = "Y" Then
                    blnAutoGenCode = True
                    txtCode.TextAlign = HorizontalAlignment.Center
                Else
                    blnAutoGenCode = False
                    lblCode.Left = lblName.Left
                    txtCode.Left = txtName.Left
                    txtCode.Width = txtName.Width
                    txtCode.TextAlign = HorizontalAlignment.Left
                End If
                Call setAutoGenCode()

                .Close()
            End With

        Else
            strTblName = argTblName
            strIDName = argIDName
            strCodeName = argCodeName
            strFldCodeName = argFldCodeName
            strFormCaption = argFormCaption
            strFldCaption = argFldCaption
            intDocID = clsSVT.getDataLongValue("Select DocID from UtlTransactions where HdrTblName = '" & strTblName & "'", 0, False)
            If clsSVT.getDataStringValue("Select FileAttachReq from UtlTransactions where HdrTblName = '" & strTblName & "'", "N", False) = "Y" Then
                blnShowFileAttach = True
            Else
                blnShowFileAttach = False
            End If
        End If
        Me.Text = strFormCaption

        Call initScreen()
        Call setSearchGridHt("")
        Call initFormVsfDet(Me, strFormCaption, vsfSearch, "SEARCH")

        If Not blnShowFileAttach Then
            PnlAttach.Enabled = False
        Else
            PnlAttach.Enabled = True
        End If
        KeyPreview = True
    End Sub

    Public Sub vsfHdr_AfterEdit(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfHdr.AfterEdit

        Call vsfGenAfterEditBfrCalTot(Me, vsfHdr, e.Row, "M")
        Call CalTotal()
        Call vsfGenAfterEditAftrCalTot(Me, vsfHdr, intDocID, Mode, e.Row, False)
    End Sub

    Public Sub vsfHdr_BeforeEdit(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfHdr.BeforeEdit

        If vsfBeforeEditCancel(Me, vsfHdr, intDocID, strTblName, Mode, e.Row, e.Col, "M", "") Then
            e.Cancel = True
            Exit Sub
        End If
    End Sub

    Public Sub vsfHdr_KeyPressEdit(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.KeyPressEditEventArgs) Handles vsfHdr.KeyPressEdit
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


    Public Sub vsfHdr_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles vsfHdr.KeyUp

        With vsfHdr
            If e.KeyCode = Keys.Tab And .Row = .Rows.Count - 1 And .Col = 4 Then
                If Trim(.GetData(.Row, 1) = "") Then
                    .Col = 1
                    .Row = 0
                    .LeftCol = 1
                    .Focus()
                Else
                    If Mode = "A" Or Mode = "M" Then
                        PnlAttach.Focus()
                    Else
                        svtBtnCtrl.Focus()
                    End If
                End If
            End If
        End With
        With vsfHdr
            If .Row = .Rows.Count - 1 Then
                svtBtnCtrl.Focus()
                ' vsfHdr.Focus()
            End If
        End With

    End Sub

    Public Sub vsfHdr_LostFocus(sender As Object, e As System.EventArgs) Handles vsfHdr.LostFocus
        pnlToolTip.Visible = False

    End Sub

    Public Sub vsfHdr_RowColChange(ByVal sender As Object, ByVal e As System.EventArgs) Handles vsfHdr.RowColChange
        If Not (Mode = "A" Or Mode = "M") Then Exit Sub
        lblToolTip.Text = ""
        lblToolTip.Text = vsfRowColChange(Me, vsfHdr)
        If lblToolTip.Text <> "" Then
            pnlToolTip.Visible = True
        Else
            pnlToolTip.Visible = False
        End If
    End Sub

    Public Sub vsfHdr_SetupEditor(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfHdr.SetupEditor
        Call vsfSetupEditor(Me, vsfHdr, intDocID, strTblName, Mode, e.Row, e.Col, "M")

    End Sub

    Private Sub txtSearch_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles txtSearch.KeyPress
        If e.KeyChar = "'" Then
            e.Handled = True
        End If
    End Sub

    Private Sub txtSearch_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtSearch.KeyUp
        Dim StrQry As String, strCond As String = ""
        If txtSearch.Text = " " Then Exit Sub
        If e.KeyCode = Keys.Return Then
            Call vsfSearchSelectedItem()
            Exit Sub
        End If

        If e.KeyCode = Keys.Down Then
            vsfSearch.Focus()
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
        vsfSearch.DataSource = Nothing

        Call setSearchGridHt("FULL")


        StrQry = "Select Top 30  " & strCodeName & "," & strFrmPfx & "Name"
        StrQry &= " from " & strTblName
        StrQry &= " where (" & strCodeName & " like '%" & Trim(txtSearch.Text) & "%'"
        StrQry &= " or " & strFrmPfx & "Name Like '%" & Trim(txtSearch.Text) & "%')"
        StrQry &= " order by 1"
        Call showWaitCursor()
        dtGeneral = clsSVT.getDATATable(StrQry)
        dvSearch = New DataView(dtGeneral)
        vsfSearch.DataSource = dvSearch
        Call showNormalCursor()
        vsfSearch.Refresh()
        vsfSearch.ScrollBars = ScrollBars.Both
        vsfSearch.AutoSizeCols()


        'StrQry = ""
        'With vsfSearch
        '    Select Case gDBTypeCurrent
        '        Case gDbTypeAccess
        '            'StrQry = StrQry & strFldItemCode & " like '*" & .Editor.Text & "*' "
        '            'StrQry = StrQry & strFldItemName & " like '*" & .Editor.Text & "*'"
        '        Case Else
        '            If txtSearch.Text = " " Then
        '                dvSearch.RowFilter = "1 = 1"
        '                Exit Sub
        '            Else
        '                StrQry = StrQry & strCodeName & " like '%" & txtSearch.Text & "%'" & " Or "
        '                StrQry = StrQry & strFldCodeName & " Like '%" & txtSearch.Text & "%'"
        '            End If
        '    End Select
        '    dvSearch.RowFilter = StrQry
        'End With
    End Sub

    Private Sub vsfSearch_BeforeEdit(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfSearch.BeforeEdit
        e.Cancel = True
    End Sub

    Private Sub vsfSearch_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles vsfSearch.DoubleClick
        Call vsfSearchSelectedItem()
    End Sub
    Private Sub vsfSearchSelectedItem()
        Dim intID As Integer

        If vsfSearch.Rows.Count <= 1 Then Exit Sub

        intID = clsSVT.getDataLongValue("Select " & strIDName & " from " & strTblName & " where " & strCodeName & " = '" & vsfSearch.GetData(vsfSearch.Row, 0) & "'", 0, False)
        txtSearch.Focus()
        Call FillDocDetails(intID)
        Call setSearchGridHt("")
        Call enableDisableFlds(False)
        txtSearch.Text = ""
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

    Private Sub lblSearch_Click(sender As System.Object, e As System.EventArgs) Handles lblSearch.Click
        With frmUtlSearchSettings
            .strTblName = strTblName
            .strDocName = strFormCaption
            .strMainFld = strCodeName
            .ShowDialog()
        End With
    End Sub

    Private Sub svtBtnCtrl_btnClicked(argBtnClicked As Integer) Handles svtBtnCtrl.btnClicked
        Select Case argBtnClicked
            Case svtBtns.svtNew
                If Not chkUserRights(strRights, chkRightsPos.AddRights, Me.Text) Then Exit Sub
                Call setSearchGridHt("")
                Call AddProc()
                lblFrm.Text = "Add:" & Me.Text
                svtBtnCtrl.PerformAction(argBtnClicked)
                cboSeries.Enabled = True
                lblSeries.ForeColor = Color.Red
                'Call chkSplRights_Std(Me, intDocID, strRights)
                Call setAutoGenCode()
                Call CalTotal()
                Me.Parent.Tag = "A"
            Case svtBtns.svtClose
                Call frmMain.CloseSelectedTabPage(Me)

            Case svtBtns.svtEdit
                If Trim(txtID.Text) = "" Then
                    Exit Sub
                End If
                cboSeries.Enabled = False
                lblSeries.ForeColor = Color.Blue
                Select Case Me.Text.ToLower
                    Case "department master", "category master", "brand master", "item sub group master"
                        If Editable() = False Then
                            MsgBox("Editing is not available for this masters", MsgBoxStyle.Information, Me.Text)
                            Exit Sub
                        End If
                End Select
                If Not chkUserRights(strRights, chkRightsPos.EditRights, Me.Text) Then Exit Sub
                Mode = "M"
                pnlSearch.Visible = False
                Call chkSplRights_Std(Me, intDocID, strRights)
                Call chkSplRights_DOC(Me, intDocID, strRights, vsfHdr)
                Call setSearchGridHt("")
                Call enableDisableFlds(True)
                Call setAutoGenCode()

                lblFrm.Text = "Edit:" & Me.Text
                svtBtnCtrl.PerformAction(argBtnClicked)
                Me.Parent.Tag = "M"
            Case svtBtns.svtSave
                vsfHdr.Enabled = False
                Call stringReplace(Me)
                If ValidFields() Then
                    If saveDetails() Then
                        svtBtnCtrl.PerformAction(argBtnClicked)
                        lblFrm.Text = "View:" & Me.Text
                    End If
                End If
                vsfHdr.Enabled = True
                Me.Parent.Tag = ""
            Case svtBtns.svtFind
                If Not chkUserRights(strRights, chkRightsPos.ViewRights, Me.Text) Then Exit Sub
                Call showViewForm(Me, strTblName, "Listing of " & Me.Text, "", "", " and " & strFldName & " <> '-'", strFormCaption, "", intDocID)
                Call enableDisableFlds(False)
                svtBtnCtrl.PerformAction(argBtnClicked)
                lblFrm.Text = "View:" & Me.Text

            Case svtBtns.svtCancel
                Call clearFlds()
                enableDisableFlds(False)
                pnlSearch.Visible = True
                txtSearch.Focus()
                txtCode.Text = ""
                Mode = ""
                svtBtnCtrl.PerformAction(argBtnClicked)
                lblFrm.Text = "View:" & Me.Text
                Call enableDisableFlds(False)
                Me.Parent.Tag = ""
            Case svtBtns.svtDelete
                If Not chkUserRights(strRights, chkRightsPos.DelRights, Me.Text) Then Exit Sub
                Call deleteDocument()
                svtBtnCtrl.PerformAction(svtBtns.svtCancel)
                lblFrm.Text = "View:" & Me.Text

            Case svtBtns.svtMoveFirst, svtBtns.svtMoveLast, svtBtns.svtMoveNext, svtBtns.svtMovePrev
                cboSeries.Focus()
                Call FillDocDetails(gMoveRecords(argBtnClicked, strTblName, strIDName, True, txtID.Text, ""))
                svtBtnCtrl.PerformAction(argBtnClicked)
                lblFrm.Text = "View:" & Me.Text
                svtBtnCtrl.btnMoveFirst.Focus()
                svtBtnCtrl.btnMoveLast.Focus()
                svtBtnCtrl.btnMoveNext.Focus()
                svtBtnCtrl.btnMovePrev.Focus()
        End Select
        If Not chkUserRights(strRights, chkRightsPos.AddRights, Me.Text, False) Then svtBtnCtrl.btnNew.Visible = False
        If Not chkUserRights(strRights, chkRightsPos.EditRights, Me.Text, False) Then svtBtnCtrl.btnEdit.Visible = False
        If Not chkUserRights(strRights, chkRightsPos.DelRights, Me.Text, False) Then svtBtnCtrl.btnDelete.Visible = False
        If Not chkUserRights(strRights, chkRightsPos.opt1Rights, Me.Text, False) Then svtBtnCtrl.btnOptions.Visible = False

        With svtBtnCtrl
            .btnPrint.Visible = False
            '.btnOptions.Visible = False
        End With
    End Sub

    Private Function Editable() As Boolean
        '  strIDName, strTblName,intID
        Editable = False
        Dim strTableName As New ArrayList()
        Dim intRow As Int32 = 0
        Dim rsdummy As oledb.oledbDataReader
        rsdummy = clsSVT.getDataReaderFromSQL("select t.name from sys.columns c,sys.tables t where t.object_id=c.object_id and c.name like '" & strIDName & "' and t.name <>'" & strTblName & "'")
        If rsdummy.HasRows Then
            Do While rsdummy.Read
                strTableName.Add(rsdummy.Item("name"))
            Loop
        End If
        rsdummy.Close()

        For i As Int32 = 0 To strTableName.Count - 1
            If clsSVT.getDataLongValue("Select count(*) from " & strTableName(i) & " where " & strIDName & " = " & GetVal(txtID.Text, 0), 0, False) > 0 Then
                Editable = False
                Exit Function
            End If
        Next
        Editable = True
    End Function

    Private Sub frmMstGeneral_Resize(sender As Object, e As System.EventArgs) Handles Me.Resize
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

    Private Sub chkInactive_CheckedChanged(sender As Object, e As System.EventArgs) Handles chkInactive.CheckedChanged
        If chkInactive.Checked Then
            chkInactive.Text = "Inactive Remarks"
            txtInActiveRem.Visible = True
            txtInActiveRem.Enabled = True
        Else
            chkInactive.Text = "Inactive"
            txtInActiveRem.Visible = False
            txtInActiveRem.Enabled = False
            txtInActiveRem.Text = ""
        End If
    End Sub

    Private Sub txtCode_LostFocus(sender As Object, e As EventArgs) Handles txtCode.LostFocus
        ' txtName.Text = txtCode.Text
    End Sub

    Private Sub frmMstGeneral_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        txtSearch.Focus()
        If blnAutoGenCode Then
            ' lblSeries.ForeColor = Color.Red
            txtCode.ReadOnly = True
            'cboSeries.Focus()
            txtSearch.Focus()
        Else
            txtCode.Focus()
            txtCode.ReadOnly = False
        End If
        txtSearch.Focus()
    End Sub

    Private Sub lblSearchImage_Click(sender As Object, e As EventArgs) Handles lblSearchImage.Click
        Dim StrQry As String, strCond As String = ""
        txtSearch.Text = ""

        If vsfSearch.Visible = True Then
            vsfSearch.Visible = False
        Else
            Call setSearchGridHt("FULL")

            StrQry = "Select Top 30  " & strCodeName & "," & strFrmPfx & "Name"
            StrQry &= " from " & strTblName
            StrQry &= " order by 1"
            Call showWaitCursor()
            dtGeneral = clsSVT.getDATATable(StrQry)
            dvSearch = New DataView(dtGeneral)
            vsfSearch.DataSource = dvSearch
            Call showNormalCursor()
            vsfSearch.Refresh()
            vsfSearch.ScrollBars = ScrollBars.Both
            vsfSearch.AutoSizeCols()
        End If
    End Sub

    Private Sub cboSeries_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles cboSeries.KeyPress
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

    Public Sub FillAutoGenItemNo()
        intNo = 0
        If Trim(cboSeries.Text) <> "" Then
            intNo = clsSVT.getDataLongValue("Select Max(" & getSyntax("ISNULL0", strFldNo) & ")+1 from " & strTblName & " where " & strFldSeries & " ='" & Trim(cboSeries.Text) & "' and " & strIDName & "<>" & Val(txtID.Text), 0, False)
            If intNo = 0 Then intNo = 1
        End If
        txtNo.Text = intNo
        txtCode.Text = Trim(cboSeries.Text) & Microsoft.VisualBasic.Right("000000" & intNo, 6)
        showNormalCursor()
    End Sub

    Private Sub cboSeries_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles cboSeries.Validating
        If Mode = "A" Then
            If Trim(cboSeries.Text) <> "" Then
                Call FillAutoGenItemNo()
                txtCode.Focus()
            End If
        End If
    End Sub

    Private Sub setAutoGenCode()
        If blnAutoGenCode Then
            ' pnlSeries.Visible = True
            cboSeries.Visible = True
            lblSeries.Visible = True
            lblCode.ForeColor = gChildReadOnlyFore
            txtCode.ReadOnly = True
            If Mode = "A" Or Mode = "M" Then
                cboSeries.Focus()
            End If
        Else
            'pnlSeries.Visible = False
            cboSeries.Visible = False
            lblSeries.Visible = False
            lblCode.ForeColor = gChildMandatoryFore
            txtCode.ReadOnly = False
            If Mode = "A" Or Mode = "M" Then
                txtCode.Focus()
            End If
        End If
    End Sub

    Private Sub svtBtnCtrl_btnOptionSelected(argOptionName As String, argBlnCtrlClick As Boolean, argBlnAltClick As Boolean) Handles svtBtnCtrl.btnOptionSelected
        Select Case argOptionName.ToUpper
            Case "APPROVE"
                If Trim(txtCode.Text) = "" Then Exit Sub
                If Not chkUserRights(strRights, chkRightsPos.AuthRights, Me.Text) Then Exit Sub
                If UCase(lblApproved.Text) <> "APPROVED" Then
                    If MsgBox("Do you want to Approve this " & Replace(strFormCaption, "Master", "") & " : '" & txtCode.Text & "' ?", vbQuestion + vbYesNo + vbDefaultButton1, Me.Text) = vbYes Then
                        clsSVT.execSQLStatement("Update " & strTblName & " set Authorised='Y', AuthBy='" & gUserCode & "', AuthOn=" & getDateFormatInCond(getSystemDate) & " where " & strIDName & "=" & Val(txtID.Text))
                        'for audit track
                        Call RecordAuditLog(strFormCaption, Val(txtID.Text), UserActions.EditAction, strTblName, strAuditHdrSQL, strAuditDtlSQL)
                        lblApproved.Text = "APPROVED"
                        lblApproved.ForeColor = Color.Green
                        Exit Sub
                    End If
                ElseIf UCase(lblApproved.Text) <> "UN-APPROVED" Then
                    If MsgBox(Replace(strFormCaption, "Master", "") & " : '" & txtCode.Text & "' is already approved." & vbCrLf & "Do you want to revoke it?", vbQuestion + vbYesNo + vbDefaultButton1, Me.Text) = vbYes Then
                        clsSVT.execSQLStatement("Update " & strTblName & " set Authorised='N', AuthBy='', AuthOn=NULL where " & strIDName & "=" & Val(txtID.Text))
                        'for audit track
                        Call RecordAuditLog(strFormCaption, Val(txtID.Text), UserActions.EditAction, strTblName, strAuditHdrSQL, strAuditDtlSQL)
                        lblApproved.Text = "UN-APPROVED"
                        lblApproved.ForeColor = Color.FromArgb(192, 0, 0)
                        Exit Sub
                    End If
                End If

            Case "ARRANGE GROUP SLNO"
                Dim obj As New frmUtlGrpOrderBy
                obj.StartPosition = FormStartPosition.CenterScreen
                obj.ShowDialog()

            Case "ARRANGE PKG SLNO"
                Dim obj As New frmUtlPkgOrderBy
                obj.StartPosition = FormStartPosition.CenterScreen
                obj.ShowDialog()

        End Select
    End Sub

    Private Sub txtCode_Validating(sender As Object, e As CancelEventArgs) Handles txtCode.Validating
        If Trim(txtCode.Text) = "" Then Exit Sub
        If Not blnAutoGenCode Then
            Dim strSQL As String
            Try
                If clsSVT.getDataLongValue("select count(" & strCodeName & ") " & " from " & strTblName & " where " & strCodeName & "Clean = dbo.RemoveNonAlphaCharacters('" & Trim(txtCode.Text) & "') and " & strIDName & "<>" & Val(txtID.Text), 0, False) > 0 Then
                    If MsgBox("Similar " & strCodeName & " already exists." & vbCrLf & "Do you want to view them?", vbInformation + vbYesNo + vbDefaultButton1, Me.Text) = vbYes Then
                        Dim frmDisplay As New frmUtlMstView
                        strSQL = "Select " & strCodeName & "," & strFrmPfx & "Name" &
                                                " from " & strTblName & " where " & strCodeName & "Clean = dbo.RemoveNonAlphaCharacters('" & Trim(txtCode.Text) & "')"
                        With frmDisplay
                            Call .FillViewVsf(Me, strSQL, "", strFrmPfx & "CODE " & "'" & txtCode.Text & "' is similar to below " & Replace(strFormCaption, "Master", "") & " :", intDocID, Me.Text, False)
                            Call .searchSettings(strFormCaption, strTblName, strCodeName)
                            .ShowDialog()
                        End With
                        Exit Sub
                    End If
                End If
            Catch ex As Exception
            End Try
        End If
    End Sub

    'Private Sub btnAttach_ClickButtonArea(Sender As Object, e As MouseEventArgs)
    '    If Mode = "A" Then Exit Sub
    '    If Trim(txtCode.Text) = "" Then Exit Sub
    '    If Not chkUserRights(strRights, chkRightsPos.ViewRights, Me.Text) Then Exit Sub
    '    With frmUtlFileControl
    '        .strRights = strRights
    '        .argOwnerCaption = Me.Text
    '        .argDocID = intDocID
    '        .argDocValueID = Val(txtID.Text)
    '        .StartPosition = FormStartPosition.CenterScreen
    '        .ShowDialog()
    '        btnFileAttach.Text = "File Attachment (" & clsSVT.getDataLongValue("Select count(*) from UtlAttachFiles Where DocID = " & intDocID & " and DocValueID = '" & Val(txtID.Text) & "'", 0, False) & ")"
    '        fillAttachFiles(Val(txtID.Text))
    '    End With
    'End Sub

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

    Private Sub vsfFiles_MouseHoverCell(sender As Object, e As EventArgs) Handles vsfFiles.MouseEnterCell
        If vsfFiles.Rows.Count > 1 Then
            With vsfFiles
                tt.SetToolTip(vsfFiles, .GetData(.Row, vFName) & "/" & .GetData(.Row, vFDesc))
            End With
        End If
    End Sub

    Private Sub btnFileAttach_ClickButtonArea(Sender As Object, e As MouseEventArgs) Handles btnFileAttach.ClickButtonArea
        If Mode = "A" Then Exit Sub
        If Trim(txtCode.Text) = "" Then Exit Sub
        If Not chkUserRights(strRights, chkRightsPos.ViewRights, Me.Text) Then Exit Sub
        With frmUtlFileControl
            .strRights = strRights
            .argOwnerCaption = Me.Text
            .argDocID = intDocID
            .argDocValueID = Val(txtID.Text)
            .StartPosition = FormStartPosition.CenterScreen
            .ShowDialog()
            btnFileAttach.Text = "File Attachment (" & clsSVT.getDataLongValue("Select count(*) from UtlAttachFiles Where DocID = " & intDocID & " and DocValueID = '" & Val(txtID.Text) & "'", 0, False) & ")"
            fillAttachFiles(Val(txtID.Text))
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

    Private Sub chkInactive_Validating(sender As Object, e As CancelEventArgs) Handles chkInactive.Validating
        If chkInactive.Checked Then
            txtInActiveRem.Focus()
        Else
            If vsfHdr.Visible = True Then
                vsfHdr.Row = 0
                vsfHdr.Col = vOthFldDataValue
                vsfHdr.Focus()
            Else
                Exit Sub
            End If
        End If

    End Sub

    Private Sub lblApproved_Click(sender As Object, e As EventArgs) Handles lblApproved.Click

    End Sub

    Private Sub txtSearch_TextChanged(sender As Object, e As EventArgs) Handles txtSearch.TextChanged
        If Trim(txtSearch.Text) = "" Then
            lblSearchImage.Image = My.Resources.D_FIND
        Else
            lblSearchImage.Image = My.Resources.Remove
        End If
    End Sub

    Private Sub txtSearch_LostFocus(sender As Object, e As EventArgs) Handles txtSearch.LostFocus
        If vsfSearch.Focused Then
            Exit Sub
        Else
            Call setSearchGridHt("")
        End If
    End Sub
End Class