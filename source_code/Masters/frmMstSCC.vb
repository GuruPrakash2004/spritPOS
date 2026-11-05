Imports System.ComponentModel
Imports C1.Win.C1FlexGrid

Public Class frmMstSCC
    Dim Mode As String
    Dim intDocID As Integer
    Dim clsSVT As New svtLibrary
    Dim strTblName As String
    Dim strDtlTblName As String
    Dim strIDName As String
    Dim strCodeName As String
    Dim strFormCaption As String
    Dim strFldCaption As String
    Dim strFldName As String
    Dim blnShowFileAttach As Boolean
    Dim strRights As String
    Dim intBtnColor As Integer
    Dim strCode As String = ""
    Dim blnFromIM As Boolean = False
    Dim blnAuthReq As Boolean = False
    Dim strFrmPfx As String

    'for audit track
    Dim blnAuditTrack As Boolean = False
    Dim strAuditHdrSQL As String
    Dim strAuditDtlSQL As String

    Private Sub frmMstGeneral_FormClosing(sender As Object, e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        If Mode = "A" Or Mode = "M" Then
            If MsgBox("Are you Sure You Want to Exit the form with out Saving!!", MsgBoxStyle.Question + MsgBoxStyle.YesNo + vbDefaultButton2, "confirmation..") = MsgBoxResult.No Then
                e.Cancel = True
                Exit Sub
            End If
        End If
    End Sub

    Private Sub frmMstGeneral_KeyUp(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyUp
        If e.KeyCode = Keys.F9 And e.Control = True Then
            Call SaveActFormSettings(Me, Me.Text)
            Call initFormVsfDet(Me, Me.Text, vsfHdr, "Hdr")
            MsgBox("Restored Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
        If e.KeyCode = Keys.F8 And e.Control = True Then
            Call SaveActFormSettings(Me, Me.Text)
            Call SaveActFormVsfDet(Me, Me.Text, "Hdr", vsfHdr)
            Call SaveActFormVsfDet(Me, Me.Text, "Search", vsfSearch)
            MsgBox("Saved Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
        If (e.Control) Then
            Call svtBtnCtrl.btnCtrlKeyUp(sender, e)
        End If
    End Sub

    Private Sub frmMstGeneral_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load

        strRights = getUserRights(GetMenuId(Me.Text))
        Call initScreen()
        Call FillDocDetails(Val(txtID.Text))
        Call setSearchGridHt("")
        Me.Tag = strTblName
        Call initFormVsfDet(Me, Me.Text, vsfHdr, "Hdr")

        initGridStyles(Me)

        With svtBtnCtrl
            .btnPrint.Visible = False
            .btnOptions.Visible = True
        End With
        fillFormProperties(Me)

        svtBtnCtrl.FlowLayoutPanel1.BackColor = Color.Transparent
        svtBtnCtrl.BackColor = Color.Transparent
        lblFrm.ForeColor = Color.White
        lblSearch.ForeColor = Color.White
        vsfHdr.Styles.EmptyArea.BackColor = Color.Transparent
        vsfHdr.BackColor = Color.Transparent
        If Not chkUserRights(strRights, chkRightsPos.AddRights, Me.Text, False) Then svtBtnCtrl.btnNew.Visible = False
        If Not chkUserRights(strRights, chkRightsPos.EditRights, Me.Text, False) Then svtBtnCtrl.btnEdit.Visible = False
        If Not chkUserRights(strRights, chkRightsPos.DelRights, Me.Text, False) Then svtBtnCtrl.btnDelete.Visible = False
        If Not chkUserRights(strRights, chkRightsPos.opt1Rights, Me.Text, False) Then svtBtnCtrl.btnOptions.Visible = False

        Call initFormSize(Me, Me.Text, False, 851, 553, 0, 0)
        Call initFormVsfDet(Me, Me.Text, vsfSearch, "SEARCH")
        Call ChangeFontSize(Me)
        Call clearFlds()
        lblFrm.Font = New Font("Microsoft Sans Serif", 14, FontStyle.Bold)
        lblSearch.Font = New Font("Segoe UI", 9.25, FontStyle.Bold)
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

        Call InitAddnlFldGrids(Me, vsfHdr, "Header", intDocID)

        Call clearFlds()
        Call InsertDataTbl()

        strSQL = "Select Max(" & strIDName & ") from " & strTblName
        txtID.Text = clsSVT.getDataLongValue(strSQL, 0, False)
        Call bindCombo(cboState, "STATENAME", "STATENAME", "Select Distinct STATENAME from MstSTATE where STATENAME<>'' order by STATENAME")
        ' Call bindCombo(cboStateCode, "StateCode", "StateCode", "Select Distinct StateCode from MstSCC where StateCode<>0 order by StateCode")
        Call bindCombo(cboCountry, "Country", "Country", "Select Distinct Country from MstSCC where Country<>'' order by Country")

        If cboCountry.Items.Count > 0 Then
            cboCountry.SelectedIndex = 0    ' The first item has index 0 '
        End If

        Me.Tag = strTblName
        txtInActiveRem.Visible = False

        Dim ctrl As Control = Nothing
        KeyPreview = True
        initGridStyles(Me)
        'Call chkSplRights_DOC(Me, intDocID, strRights, vsfHdr)
        If blnAuthReq Then
            svtBtnCtrl.addOption("Approve")
        End If
    End Sub

    Private Sub clearFlds()
        txtCode.Text = ""
        txtSearch.Text = ""
        txtID.Text = ""
        cboState.SelectedIndex = -1
        cboCountry.Text = ""
        'cbostateCode.text = ""
        txtInActiveRem.Text = ""
        chkInactive.Enabled = False
        chkInactive.Checked = False
        If blnShowFileAttach Then svtAttach.ClearFiles()
        pnlSearch.Visible = True
        txtSearch.Visible = True
        lblApproved.Visible = False
        txtSearch.Focus()
        '--7 Begin OthGRID
        Call ClearAddnlGrid(vsfHdr)
        Call enableDisableFlds(False)
        svtAttach.ViewMode = True
        Mode = "V"
    End Sub
    '# A - Insertion of Data to avoid link problem
    Private Sub InsertDataTbl()
        Dim recdummy As oledb.oledbDataReader
        Dim strSQL As String

        strSQL = "select * from " & strTblName & " where " & strIDName & " = 0"
        recdummy = clsSVT.getDataReaderFromSQL(strSQL)
        If recdummy.HasRows = False Then
            strSQL = " Insert into " & strTblName & " ("
            strSQL = strSQL & strIDName & "," & strCodeName & ",InActive,"
            strSQL = strSQL & "AddedBy,AddedOn,ChangedBy,ChangedOn)"
            strSQL = strSQL & " values("
            strSQL = strSQL & "0,'-', "
            strSQL = strSQL & "'N',"
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
        Call bindCombo(cboState, "STATENAME", "STATENAME", "Select Distinct STATENAME from MstSTATE where STATENAME<>'' order by STATENAME")
        '  Call bindCombo(cboStateCode, "StateCode", "StateCode", "Select Distinct StateCode from MstSCC where Statecode<>0 order by StateCode")
        Call bindCombo(cboCountry, "Country", "Country", "Select Distinct Country from MstSCC where Country<>'' order by Country")
        cboCountry.SelectedIndex = -1
        cboState.SelectedIndex = -1
        enableDisableFlds(True)

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
        svtAttach.Enabled = argFlag
        cboState.Enabled = argFlag
        'cboStateCode.Enabled = argFlag
        cboCountry.Enabled = argFlag
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
        If Mode = "V" Then Exit Function
        Dim blnFound As Boolean

        StMsg("Validating Data.....")
        If Not IsNothing(vsfHdr.Editor) Then
            If vsfHdr.Editor.Text <> "" Then
                vsfHdr.SetData(vsfHdr.Row, vOthFldDataValue, vsfHdr.Editor.Text)
            End If
        End If
        ValidFields = False
        txtCode.Text = Replace(txtCode.Text, "'", "`")
        If Trim(txtCode.Text) = "" Then
            MsgBox("City should not be blank!!", MsgBoxStyle.Information, Me.Text)
            txtCode.Focus()
            Exit Function
        End If

        If Trim(cboState.Text) = "" Then
            MsgBox("State should not be blank!!", MsgBoxStyle.Information, Me.Text)
            cboState.Focus()
            Exit Function
        End If

        clsSVT.getDataLongValue("Select StateID  from MstSTATE where STATENAME='" & cboState.Text & "'", 0, blnFound)
        If Not blnFound Then
            MsgBox("State Not found in Master!", vbInformation, Me.Text)
            cboState.Focus()
            Exit Function
        End If

        If Not (Mode = "A" Or Mode = "M") Then Exit Function
        ' Dim blnFound As Boolean
        Call clsSVT.getDataLongValue("Select StateID  from MstSTATE where STATENAME='" & cboState.Text & "'", 0, blnFound)
        If Not blnFound Then
            MsgBox("State Not found in Master!", vbInformation, Me.Text)
            cboState.Focus()
            Exit Function

        End If

        If Mode = "A" Then txtID.Text = clsSVT.getDataLongValue("Select max(" & strIDName & ") + 1 from " & strTblName, 1, False)
        Call clsSVT.getDataStringValue("Select " & strCodeName & " from " & strTblName _
                & " where " & strCodeName & " ='" & Trim(txtCode.Text) & "' and State ='" & Trim(cboState.Text) & "' and Country='" & Trim(cboCountry.Text) & "' and " & strIDName & " <> " & Val(txtID.Text), "", blnFound)
        If blnFound Then
            MsgBox(strFldCaption & " already exists..", MsgBoxStyle.Information, Me.Text)
            txtCode.Text = ""
            txtCode.Focus()
            Exit Function
        End If

        If Not validOthFLds(Me, strTblName, strIDName, Val(txtID.Text)) Then
            Exit Function
        End If

        ValidFields = True

    End Function

    Private Function saveDetails() As Boolean
        Dim strSQL As String = ""
        Dim strValues As String = ""
        Dim strFlds As String = ""
        Dim svtTrn As oledb.oledbTransaction
        saveDetails = False
        Dim intID As Long

        If Mode = "A" Then
            intID = clsSVT.AutoIncrementedNum(strIDName, strTblName)
            txtID.Text = intID

            strSQL = " Insert into " & strTblName & " ("
            strSQL = strSQL & strIDName & "," & strCodeName & ",Inactive,InactiveRem,"
            strSQL = strSQL & "AddedBy,AddedOn,State,Country"

            '--4 begin OthGRID
            Call GetInsertString(Me, strValues, strFlds)

            strSQL = strSQL & strFlds & ") values ("
            strSQL = strSQL & intID & ","
            strSQL = strSQL & "'" & txtCode.Text & "', "
            strSQL = strSQL & "'" & getCheckString(chkInactive) & "',"
            strSQL = strSQL & "'" & txtInActiveRem.Text & "',"
            strSQL = strSQL & "'" & gUserCode & "',"
            strSQL = strSQL & "'" & getDateFormatInInsert(getSystemDateTime, True) & "',"
            strSQL = strSQL & "'" & Replace(Trim(cboState.Text), "'", "`") & "',"
            strSQL = strSQL & "'" & Replace(Trim(cboCountry.Text), "'", "`") & "'"


            '--5 begin OthGRID
            strSQL = strSQL & strValues
        Else

            strSQL = "Update  " & strTblName & " set "
            strSQL = strSQL & strCodeName & "  = '" & txtCode.Text & "',"
            strSQL = strSQL & " InActive = '" & getCheckString(chkInactive) & "',"
            strSQL = strSQL & "InactiveRem= '" & txtInActiveRem.Text & "',"
            strSQL = strSQL & " ChangedBy = '" & gUserCode & "',"
            strSQL = strSQL & " ChangedOn = '" & getDateFormatInInsert(getSystemDateTime, True) & "',"
            strSQL = strSQL & " State = '" & Replace(Trim(cboState.Text), "'", "`") & "',"
            strSQL = strSQL & " Country = '" & Replace(Trim(cboCountry.Text), "'", "`") & "'"
            '    strSQL = strSQL & " Statecode = " & Val(Replace(Trim(cboStateCode.Text), "'", "`")) & ""

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
                Call svtAttach.saveAttachments(intDocID, Val(txtID.Text), True, svtTrn)
                'for audit track
                If blnAuditTrack Then
                    Call RecordAuditLog(strFormCaption, Val(txtID.Text), IIf(Mode = "A", UserActions.AddAction, UserActions.EditAction), strTblName, strAuditHdrSQL, strAuditDtlSQL, svtTrn)
                End If
                If Mode = "A" Then
                    MsgBox(strFldCaption & " '" & Trim(txtCode.Text) & "' added successfully.", vbInformation, Me.Text)
                Else
                    MsgBox("Updated successfully.", vbInformation, Me.Text)
                End If
                saveDetails = True
                svtTrn.Commit()
                StMsg("Status")
            End If
        Catch ex As Exception
            MsgBox("Error during save" & vbCrLf & ex.Message, MsgBoxStyle.Critical, Me.Text)
            svtTrn.Rollback()
        End Try
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
                cboCountry.Text = GetVal(RsTemp("Country"), "")
                cboState.Text = GetVal(RsTemp("State"), "")
                txtInActiveRem.Text = GetVal(RsTemp("InactiveRem"), "")
                'cboStateCode.Text = GetVal(RsTemp("Statecode"), 0)
                Call ApplyChkToCheckBox(chkInactive, GetVal(!Inactive, "N"))

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
        If blnShowFileAttach Then
            With svtAttach
                .ClearFiles()
                .LoadFilesFromData("Select AFilename from UtlAttachFiles Where DocID = " & intDocID & " and DocValueID = '" & argID & "'")
            End With
        End If

        Mode = "V"
        enableDisableFlds(False)
        pnlSearch.Visible = True
        svtAttach.ViewMode = True
        txtSearch.Focus()
    End Sub

    Private Sub CalTotal()
        Call AddnlMSTFldCalTotal(Me, vsfHdr, 0, "", "", txtID.Text)
    End Sub

    Private Sub deleteDocument()
        If Val(txtID.Text) <= 0 Then Exit Sub
        If Not blnDelOK(Val(txtID.Text)) Then
            Exit Sub
        End If

        If MsgBox("Are you sure you want to delete the " & strFldCaption &
            " : '" & txtCode.Text & "'?", vbCritical + MsgBoxStyle.YesNo + vbDefaultButton2, "Delete Confirmation") = MsgBoxResult.Yes Then
            Dim svtTrn As oledb.oledbTransaction = Nothing
            Try
                svtTrn = svtConn.BeginTransaction(IsolationLevel.ReadCommitted)
                Call RecordAuditLog(strFormCaption, Val(txtID.Text), UserActions.DeleteAction, strTblName, strAuditHdrSQL, "", svtTrn)
                clsSVT.execSQLStatement("Delete from " & strTblName & " where " & strIDName & " = " & Val(txtID.Text), svtTrn)
                If intDocID = 172 Then
                    clsSVT.execSQLStatement("Delete from MstFinishDtl where " & strIDName & " = " & Val(txtID.Text), svtTrn)
                End If
                MsgBox("Deleted Successfully!", MsgBoxStyle.Information, Me.Text)
                Call clearFlds()
                svtTrn.Commit()
            Catch ex As Exception
                svtTrn.Rollback()
            End Try
        Else
            Exit Sub
        End If
    End Sub

    Public Sub initForm(ByVal argDocID As Integer, ByVal argTblName As String, ByVal argIDName As String, ByVal argCodeName As String, ByVal argFormCaption As String, ByVal argFldCaption As String, Optional ByVal argFromIM As Boolean = False)

        If argFromIM Then
            blnFromIM = True
        End If

        If argDocID > 0 Then
            intDocID = argDocID

            Dim rs As oledb.oledbDataReader
            rs = clsSVT.getDataReaderFromSQL("Select * from UtlTransactions where DocID = " & argDocID)
            With rs
                .Read()
                strTblName = !HdrTblName
                strIDName = Replace(!HdrTblName, "Mst", "") & "ID"
                strCodeName = "City" 'Replace(!HdrTblName, "Mst", "") & "Code"
                strFormCaption = !DocName
                strFldName = argCodeName
                strFldCaption = Trim(Replace(!Docname, "Master", "Name"))
                lblFrm.Text = strFormCaption
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
                If !FileAttachReq = "Y" Then
                    blnShowFileAttach = True
                Else
                    blnShowFileAttach = False
                End If
            End With
        Else
            strTblName = argTblName
            strIDName = argIDName
            strCodeName = argCodeName
            strFldName = argCodeName
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
        lblCode.Text = "City" 'strFldCaption
        'Call initScreen()
        Call setSearchGridHt("")

        If Not blnShowFileAttach Then
            svtAttach.Visible = False
        Else
            With svtAttach
                .argOwnerCaption = Me.Text
                .argFilePath = gstrDataPath & "TrnDocFiles\"
                .argDocID = intDocID
                .ViewMode = True
            End With
        End If
        KeyPreview = True
    End Sub

    Private Sub vsfHdr_AfterEdit(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfHdr.AfterEdit

        Call vsfGenAfterEditBfrCalTot(Me, vsfHdr, e.Row, "M")
        Call CalTotal()
        Call vsfGenAfterEditAftrCalTot(Me, vsfHdr, intDocID, Mode, e.Row, False)
        ' Call SetFocusBtn(vsfHdr, True)
    End Sub

    Private Sub vsfHdr_BeforeEdit(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfHdr.BeforeEdit

        If vsfBeforeEditCancel(Me, vsfHdr, intDocID, strTblName, Mode, e.Row, e.Col, "M", "") Then
            e.Cancel = True
            Exit Sub
        End If
    End Sub

    Private Sub vsfHdr_KeyPressEdit(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.KeyPressEditEventArgs) Handles vsfHdr.KeyPressEdit
        If e.KeyChar = "'" Then
            e.Handled = True
        End If
        If vsfKeyPressEdit(Me, vsfHdr, vsfHdr.Row, e.KeyChar) Then
            e.Handled = False
        Else
            e.Handled = True
        End If
    End Sub

    Private Sub vsfHdr_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles vsfHdr.KeyUp

        With vsfHdr
            If e.KeyCode = Keys.Tab And .Row = .Rows.Count - 1 And .Col = 4 Then
                If Trim(.GetData(.Row, 1) = "") Then
                    .Col = 1
                    .Row = 0
                    .LeftCol = 1
                    .Focus()
                Else
                    If Mode = "A" Or Mode = "M" Then
                        svtAttach.Focus()
                    Else
                        svtBtnCtrl.Focus()
                    End If
                End If
            End If
        End With
        With vsfHdr
            If .Row = .Rows.Count - 1 Then
                svtBtnCtrl.Focus()
            End If
        End With

    End Sub

    Private Sub vsfHdr_RowColChange(ByVal sender As Object, ByVal e As System.EventArgs) Handles vsfHdr.RowColChange
        If Not (Mode = "A" Or Mode = "M") Then Exit Sub
    End Sub

    Private Sub vsfHdr_SetupEditor(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfHdr.SetupEditor
        Call vsfSetupEditor(Me, vsfHdr, intDocID, strTblName, Mode, e.Row, e.Col, "M")
    End Sub

    Private Sub txtSearch_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles txtSearch.KeyPress
        If e.KeyChar = "'" Then
            e.Handled = True
        End If
    End Sub

    Private Sub txtSearch_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtSearch.KeyUp
        Dim strSQL As String, strCond As String = ""

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
        Dim rs As oledb.oledbDataReader, dt As DataTable

        strSQL = " Select  " & strCodeName
        strCond = " where " & strCodeName & " like '%" & txtSearch.Text & "%'"
        rs = clsSVT.getDataReaderFromSQL("Select * from UtlSearchSettings where DocName = '" & strFormCaption & "'")

        Do While rs.Read
            strSQL = strSQL & "," & rs("SearchFld")
            strCond = strCond & " or " & rs("SearchFld") & " like '%" & txtSearch.Text & "%'"
        Loop
        rs.Close()
        strSQL = strSQL & " From  " & strTblName & " " & strCond
        dt = clsSVT.getDATATable(strSQL)
        vsfSearch.DataSource = dt
        vsfSearch.Refresh()
    End Sub

    Private Sub btnSearchSettings_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        With frmUtlSearchSettings
            .strTblName = strTblName
            .strDocName = strFormCaption
            .strMainFld = strCodeName
            .ShowDialog()
        End With
    End Sub

    Private Sub vsfSearch_BeforeEdit(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfSearch.BeforeEdit
        e.Cancel = True
    End Sub

    Private Sub vsfSearch_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles vsfSearch.DoubleClick
        'If vsfSearch.Rows.Count <= 1 Then Exit Sub
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
                svtAttach.ViewMode = False
                'Call chkSplRights_Std(Me, intDocID, strRights)
                txtCode.Focus()
                lblFrm.Text = "Add:" & Me.Text
                svtBtnCtrl.PerformAction(argBtnClicked)

            Case svtBtns.svtClose
                Call frmMain.CloseSelectedTabPage(Me)

            Case svtBtns.svtEdit
                If Trim(txtID.Text) = "" Then
                    Exit Sub
                End If
                If Not chkUserRights(strRights, chkRightsPos.EditRights, Me.Text) Then Exit Sub
                Mode = "M"
                pnlSearch.Visible = False
                svtAttach.ViewMode = False
                Call chkSplRights_Std(Me, intDocID, strRights)
                Call chkSplRights_DOC(Me, intDocID, strRights, vsfHdr)
                Call setSearchGridHt("")
                Call enableDisableFlds(True)
                txtCode.Focus()
                lblFrm.Text = "Edit:" & Me.Text
                svtBtnCtrl.PerformAction(argBtnClicked)
            Case svtBtns.svtSave
                vsfHdr.Enabled = False
                Call stringReplace(Me)
                If ValidFields() Then
                    Call saveDetails()
                    svtBtnCtrl.PerformAction(argBtnClicked)
                    lblFrm.Text = "View:" & Me.Text
                    Call clearFlds()
                End If
                vsfHdr.Enabled = True

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

            Case svtBtns.svtDelete
                If Not chkUserRights(strRights, chkRightsPos.DelRights, Me.Text) Then Exit Sub
                Call deleteDocument()
                svtBtnCtrl.PerformAction(svtBtns.svtCancel)
                lblFrm.Text = "View:" & Me.Text

            Case svtBtns.svtMoveFirst, svtBtns.svtMoveLast, svtBtns.svtMoveNext, svtBtns.svtMovePrev
                Call FillDocDetails(gMoveRecords(argBtnClicked, strTblName, strIDName, True, txtID.Text, ""))
                svtBtnCtrl.PerformAction(argBtnClicked)
                lblFrm.Text = "View:" & Me.Text
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

    Private Sub btnFind_ClickButtonArea(Sender As System.Object, e As System.Windows.Forms.MouseEventArgs) 'Handles btnFind.Click
        Call showViewForm(Me, strTblName, "Listing of " & Me.Text, "", "", " and " & strFldName & " <> '-'", strFormCaption, "", intDocID)
        Call enableDisableFlds(False)
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
        For i As Int32 = 0 To strTableName.Count - 1
            If clsSVT.getDataLongValue("Select count(*) from " & strTableName(i) & " where " & strIDName & " = " & GetVal(txtID.Text, 0), 0, False) > 0 Then
                Editable = False
                Exit Function
            End If
        Next
        Editable = True
    End Function

    Private Sub cboState_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles cboState.KeyPress
        If e.KeyChar = "'" Then e.KeyChar = "`"
        If Not KeyValidate("String", e.KeyChar, 100) Then
            e.Handled = True
        End If
    End Sub

    Private Sub cboCountry_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles cboCountry.KeyPress
        If e.KeyChar = "'" Then e.KeyChar = "`"
        If Not KeyValidate("String", e.KeyChar, 100) Then
            e.Handled = True
        End If
    End Sub

    Private Sub chkInactive_CheckedChanged(sender As Object, e As EventArgs) Handles chkInactive.CheckedChanged
        If chkInactive.Checked Then
            chkInactive.Text = "Inactive Remarks"
            txtInActiveRem.Visible = True
            txtInActiveRem.Enabled = True
            txtInActiveRem.Focus()
        Else
            chkInactive.Text = "Inactive"
            txtInActiveRem.Visible = False
            txtInActiveRem.Enabled = False
            txtInActiveRem.Text = ""
        End If
    End Sub

    Private Sub cboStateCode_KeyPress(sender As Object, e As KeyPressEventArgs)
        If e.KeyChar = "'" Then e.KeyChar = "`"
        If Not KeyValidate("Integer", e.KeyChar) Then
            e.Handled = True
        End If
    End Sub

    Private Sub lblSearchImage_Click(sender As Object, e As EventArgs) Handles lblSearchImage.Click
        Dim strSQL As String
        If Not chkUserRights(strRights, chkRightsPos.ViewRights, Me.Text) Then Exit Sub
        Dim dv As DataTable
        lblFrm.Text = "View:" & Me.Text
        txtSearch.Text = ""

        If vsfSearch.Visible = True Then
            vsfSearch.Visible = False
        Else
            strSQL = " Select  " & strCodeName
            strSQL = strSQL & " From  " & strTblName & " where 1=1"


            Call setSearchGridHt("FULL")
            dv = clsSVT.getDATATable(strSQL)
            vsfSearch.DataSource = dv
            vsfSearch.Refresh()
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
                        lblApproved.Text = "APPROVED"
                        'for audit track
                        Call RecordAuditLog(strFormCaption, Val(txtID.Text), UserActions.EditAction, strTblName, strAuditHdrSQL, strAuditDtlSQL)
                        lblApproved.ForeColor = Color.Green
                        Exit Sub
                    End If
                ElseIf UCase(lblApproved.Text) <> "UN-APPROVED" Then
                    If MsgBox(Replace(strFormCaption, "Master", "") & " : '" & txtCode.Text & "' is already approved." & vbCrLf & "Do you want to revoke it?", vbQuestion + vbYesNo + vbDefaultButton1, Me.Text) = vbYes Then
                        clsSVT.execSQLStatement("Update " & strTblName & " set Authorised='N', AuthBy='', AuthOn=NULL where " & strIDName & "=" & Val(txtID.Text))
                        lblApproved.Text = "UN-APPROVED"
                        'for audit track
                        Call RecordAuditLog(strFormCaption, Val(txtID.Text), UserActions.EditAction, strTblName, strAuditHdrSQL, strAuditDtlSQL)
                        lblApproved.ForeColor = Color.FromArgb(192, 0, 0)
                        Exit Sub
                    End If
                End If
        End Select
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

    Private Sub btnMstState_Click(sender As Object, e As EventArgs) Handles btnMstState.Click
        Dim argfrm As New frmMstGeneral
        Dim intSDocID As Integer
        intSDocID = clsSVT.getDataLongValue("select DocID from UtlTransactions where DocName='State Master'", 0, False)
        argfrm.initForm(intSDocID, "MstState", "STATEID", "STATECODE", "STATENAME", "State Master", "STATECODE")
        frmMain.OpenForms(argfrm, "State Master")
    End Sub

    Private Sub cboState_Validating(sender As Object, e As CancelEventArgs) Handles cboState.Validating

    End Sub

    Private Sub vsfHdr_AfterResizeColumn(sender As Object, e As RowColEventArgs) Handles vsfHdr.AfterResizeColumn
        If vsfHdr.Cols(vOthFldCaption).Width >= vsfHdr.Width Then
            vsfHdr.Cols(vOthFldCaption).Width = vsfHdr.Width - 20
        End If
    End Sub


    Private Sub frmMstSCC_Resize(sender As Object, e As EventArgs) Handles Me.Resize
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

    Private Function blnDelOK(ByVal argState As String) As Boolean
        blnDelOK = False

        'Dim blnFound As Boolean, strFldName As String, strTblName As String, strTablCaption As String, strDispFldName As String, strDocValue As String = "", dblDocValue As Long
        'Dim clsSvt As New svtLibrary
        'Dim rs As oledb.oledbDataReader
        'Dim strSQL As String, strDelTblname As String

        ''strSQL = "Select * from UtlDeletion where Upper(DelTblName) = '" & UCase(Trim(argTblName)) & "'"

        'strSQL = " select * from Utldeletion where " & getSyntax("UPPER") & "(DelTblName) ='" & UCase(Trim(argTblName)) & "'"
        'rs = clsSvt.getDataReaderFromSQL(strSQL)
        'With rs
        '    If .HasRows Then
        '        Do While rs.Read
        '            strFldName = GetVal(!Fldname, "")
        '            strTblName = GetVal(!TblName, "")
        '            strDispFldName = GetVal(!DispFldName, "")
        '            strTablCaption = GetVal(!TBLCaption, "")
        '            strDelTblname = GetVal(!DelTblNAme, "")

        '            blnFound = False
        '            If UCase(Trim(strTblName)) <> UCase(Trim(argTblName)) Then
        '                If blnNumeric Then
        '                    strSQL = "Select " & strFldName & " from " & strTblName & " Where " & strFldName & " = " & argFldValue
        '                    'strSQL = "Select " & strFldName & " from " & strTblName & " Where " & strFldName & " = '" & (clsSvt.getDataStringValue("Select State from MstSCC where SCCID = " & Val(argFldValue), "", False)) & "'"

        '                    strDocValue = clsSvt.getDataStringValue(strSQL, "", blnFound)

        '                    'Call clsSvt.getDataLongValue("Select " & strFldName & " from " & strTblName & " Where " & strFldName & " = " & Val(argFldValue), 0, blnFound)
        '                Else
        '                    strSQL = "Select " & strDispFldName & " from " & strTblName & " Where " & strFldName & " = '" & Trim(argFldValue) & "'"
        '                    dblDocValue = clsSvt.getDataStringValue(strSQL, "", blnFound)
        '                    strDocValue = dblDocValue
        '                    'strSQL = "Select " & strFldName & " from " & strTblName & " Where " & strFldName & " = '" & Trim(argFldValue) & "'"
        '                    ' Call clsSvt.getDataLongValue(strSQL, "", blnFound)
        '                End If
        '                If strDispFldName <> "" Then
        '                    strDispFldName = clsSvt.getDataStringValue("Select  " & strDispFldName & " from " & strDelTblname & " Where " & strFldName & " = " & Val(argFldValue), "", False)
        '                End If
        '            End If
        '            If blnFound Then
        '                blnDeleteOK = False
        '                If blnDispMsg Then
        '                    If argFldCode <> "" Then
        '                        MsgBox("Cannot Delete!!! " & vbCrLf & "Data Is exists In '" & strDispFldName & "' of '" & (strTablCaption) & "'", vbInformation, "Deletion Check")
        '                        'MsgBox("Cannot Delete!!! " & vbCrLf & "Data : " & argFldCode & " of " & argTblName & " exists in " & strTblName, vbInformation, "Deletion Check")
        '                    Else
        '                        'MsgBox("Cannot Delete!!! " & vbCrLf & "Data is exists in '" & strDispFldName & "' of '" & (strTablCaption) & "'", vbInformation, "Deletion Check")
        '                        MsgBox("Cannot Delete!!! " & vbCrLf & "Data : " & strDispFldName & " of " & argTblName & " exists in " & (strTablCaption), vbInformation, "Deletion Check")
        '                    End If
        '                End If
        '                Exit Function
        '            End If
        '        Loop
        '    End If
        '    .Close()
        'End With

        'blnDeleteOK = True
    End Function
End Class