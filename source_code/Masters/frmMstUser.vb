Imports C1.Win.C1FlexGrid
Imports System.ComponentModel
Imports System.IO

Public Class frmMstUSer
    Dim Mode As String
    Dim intDocID As Integer
    Dim clsSVT As New svtLibrary
    Dim svtTrn As oledb.oledbTransaction
    Dim strTblName As String
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
    Dim strFrmPfx As String
    Dim imagePath As String
    Dim blnAuthReq As Boolean = False

    'for audit track
    Dim blnAuditTrack As Boolean = False
    Dim strAuditHdrSQL As String
    Dim strAuditDtlSQL As String

    Dim dvUser As DataView
    Dim dtUser As DataTable
    Dim strUserText As String = ""
    Dim intMinSplChrs As Integer, intMinNumeric As Integer, intMinAlpha As Integer, intMinLen As Integer, intMinLCase As Integer, intMinUCase As Integer


    Private Sub frmMstGeneral_FormClosing(sender As Object, e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        If Mode = "A" Or Mode = "M" Then
            If MsgBox("Are you sure you Want to close this form without saving?", MsgBoxStyle.Question + MsgBoxStyle.YesNo + vbDefaultButton2, "Confirmation..") = MsgBoxResult.No Then
                e.Cancel = True
                Exit Sub
            End If
        End If
    End Sub

    Private Sub frmMstGeneral_KeyUp(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyUp
        Dim blnFnKey As Boolean = False
        If e.KeyCode = Keys.F9 And e.Control = True Then
            Call initFormSize(Me, Me.Text, True, 1142, 648, 0, 0)
            'Call initFormVsfDet(Me, Me.Text, vsfHdr, "Hdr")
            MsgBox("Restored Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
        If e.KeyCode = Keys.F8 And e.Control = True Then
            Call SaveActFormSettings(Me, Me.Text)
            Call SaveActFormVsfDet(Me, Me.Text, "Hdr", vsfHdr)
            Call SaveActFormVsfDet(Me, Me.Text, "Search", vsfSearch)
            MsgBox("Saved Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
        If (e.Control) Or blnFnKey Then
            Call svtBtnCtrl.btnCtrlKeyUp(sender, e)
        End If

        If e.Shift = True And e.KeyCode = Keys.Right Then
            Call FillDocDetails(gMoveRecords(svtBtns.svtMoveNext, strTblName, strIDName, True, txtID.Text, ""))
        End If

        If e.Shift = True And e.KeyCode = Keys.Left Then
            Call FillDocDetails(gMoveRecords(svtBtns.svtMovePrev, strTblName, strIDName, True, txtID.Text, ""))
        End If
    End Sub

    Private Sub frmMstGeneral_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load

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

        Call initFormSize(Me, Me.Text, False, 1142, 648, 0, 0)
        Call initFormVsfDet(Me, Me.Text, vsfSearch, "SEARCH")
        Call initFormVsfDet(Me, Me.Text, vsfHdr, "HDR")
        If Mode = "V" Then
            imgEye.Visible = False
        End If
        Try
            imgEye.Image = Image.FromFile(gstrRptPath & "imgDontShow.png")
        Catch ex As Exception
        End Try
        Call ChangeFontSize(Me)
        btnRights.Visible = True

        If gblnUserBased Then
            btnRights.Visible = True
        Else
            btnRights.Visible = False
        End If
        lblFrm.Font = New Font("Microsoft Sans Serif", 14, FontStyle.Bold)
        lblSearch.Font = New Font("Segoe UI", 9.25, FontStyle.Bold)
        If gBlnAdvAdminLogin And gUserCode = "ADMIN" Then
            svtBtnCtrl.addOption("Change Adnl.ADMIN Password")
        End If
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
        'Call InsertDataTbl()
        strSQL = "Select Max(" & strIDName & ") from " & strTblName
        txtID.Text = clsSVT.getDataLongValue(strSQL, 0, False)

        Me.Tag = strTblName

        Dim ctrl As Control = Nothing
        KeyPreview = True
        initGridStyles(Me)
        initToolTipColours(lblToolTip, pnlToolTip)
        Call getLoginSettings()
        Call getPwdRules()
        svtBtnCtrl.addOption("Field Level Rights")
        svtBtnCtrl.addOption("Master Approvals")
        If blnAuthReq Then
            svtBtnCtrl.addOption("Approve")
        End If
    End Sub

    Private Sub getLoginSettings()
        If Not gblnUserBased Then
            pnlRole.Visible = True
            Call bindCombo(cboRole, "RoleCode", "RoleCode", "Select distinct RoleCode from MstRole where InActive <>'Y' order by 1")
        Else
            pnlRole.Visible = False
        End If

        If gblnPwdNeveExp = True Then
            pnlPwd.Visible = False
        Else
            pnlPwd.Visible = True
        End If

        If gblnADAuth Then
            pnlPwd.Visible = False
            lblPwdReset.Visible = False
            dtPwdReset.Visible = False
        Else
            pnlPwd.Visible = True
            lblPwdReset.Visible = True
            dtPwdReset.Visible = True
        End If
    End Sub

    Private Sub clearFlds()
        txtCode.Text = ""
        txtSearch.Text = ""
        txtName.Text = ""
        txtID.Text = ""
        chkInactive.Enabled = False
        chkInactive.Checked = False
        txtInActiveRem.Text = ""
        If blnShowFileAttach Then svtAttach.ClearFiles()

        pnlSearch.Visible = True
        txtSearch.Visible = True
        lblApproved.Visible = False
        txtSearch.Focus()
        '--7 Begin OthGRID
        Call ClearAddnlGrid(Me)
        Call enableDisableFlds(False)
        svtAttach.ViewMode = True
        txtCnfPassword.Text = ""
        txtPassword.Text = ""
        txtPhoneNo.Text = ""
        txtDept.Text = ""
        txtEmail.Text = ""
        chkDOB.Checked = False
        DtpDOB.Visible = False
        DtpDOB.MaxDate = getSystemDate()
        cboRole.Text = ""
        PicBox.Image = Nothing
        btnRights.Visible = False
        PnlPwdRules.Visible = False
        btnRmvImg.Visible = False
    End Sub

    '# A - Insertion of Data to avoid link problem
    Private Sub InsertDataTbl()
        Dim recdummy As oledb.oledbDataReader
        Dim strSQL As String

        strSQL = "select * from " & strTblName & " where " & strIDName & " = 0"
        recdummy = clsSVT.getDataReaderFromSQL(strSQL)
        If recdummy.HasRows = False Then
            strSQL = " Insert into " & strTblName & " ("
            strSQL = strSQL & strIDName & "," & strCodeName & "," & strFldCodeName & "," & " Inactive"
            strSQL = strSQL & ")"
            strSQL = strSQL & " values("
            strSQL = strSQL & "0,'-','-', "
            strSQL = strSQL & "'N'"
            strSQL = strSQL & ")"

            Call clsSVT.execSQLStatement(strSQL)
        End If
        recdummy.Close()

    End Sub
    Private Sub AddProc()

        Call clearFlds()
        Mode = "A"

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

        txtDept.Enabled = argFlag
        txtEmail.Enabled = argFlag
        txtPhoneNo.Enabled = argFlag
        txtPassword.Enabled = argFlag
        txtCnfPassword.Enabled = argFlag
        DtpDOB.Enabled = argFlag
        chkDOB.Enabled = argFlag
        cboRole.Enabled = argFlag
        PicBox.Enabled = argFlag
        btnRmvImg.Enabled = argFlag
        dtPwdReset.Enabled = argFlag
        'btnRole.Enabled = argFlag
        'btnRights.Enabled = argFlag
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
        stringReplace(Me)
        StMsg("Validating Data.....")
        If Not IsNothing(vsfHdr.Editor) Then
            If vsfHdr.Editor.Text <> "" Then
                vsfHdr.SetData(vsfHdr.Row, vOthFldDataValue, vsfHdr.Editor.Text)
            End If
        End If
        ValidFields = False

        If Trim(txtCode.Text) = "" Then
            MsgBox(lblCode.Text & " should not be blank!!", MsgBoxStyle.Information, Me.Text)
            txtCode.Focus()
            Exit Function
        End If
        If Trim(txtName.Text) = "" Then
            MsgBox(lblName.Text & " should not be blank!!", MsgBoxStyle.Information, Me.Text)
            txtName.Focus()
            Exit Function
        End If


        If Mode = "A" Then
            txtID.Text = clsSVT.getDataLongValue("Select max(" & strIDName & ") + 1 from " & strTblName, 1, False).ToString

            Call clsSVT.getDataStringValue("Select " & strCodeName & " from " & strTblName _
                & " where " & strCodeName & " ='" & Trim(txtCode.Text) & "' and " & strIDName & " <> " & Val(txtID.Text), "", blnFound)
            If blnFound Then
                MsgBox(strFldCaption & " already exists..", MsgBoxStyle.Information, Me.Text)
                txtCode.Text = ""
                txtCode.Focus()
                Exit Function
            End If
        End If
        If gblnADAuth = False Then
            If Trim(txtPassword.Text) = "" Then
                MsgBox("Password should not be blank!!", vbInformation + vbOKOnly, Me.Text)
                txtPassword.Focus()
                Exit Function
            End If

            txtCnfPassword.Text = Replace(txtCnfPassword.Text, "'", "`")
            If Trim(txtCnfPassword.Text) = "" Then
                MsgBox("Confirm Password should not be blank!!", vbInformation + vbOKOnly, Me.Text)
                txtCnfPassword.Focus()
                Exit Function
            End If

            If Trim(UCase(txtPassword.Text)) <> Trim(UCase(txtCnfPassword.Text)) Then
                MsgBox("Confirm Password should be same as password!!", vbInformation + vbOKOnly, Me.Text)
                txtCnfPassword.Focus()
                Exit Function
            End If

            If gblnPwdPolicy AndAlso gblnPwdRule Then
                If Not ValidPwdRules() Then
                    MsgBox("Password doesn't match the Rules!", vbInformation, Me.Text)
                    txtPassword.Focus()
                    Exit Function
                End If
                PnlPwdRules.Visible = False
            End If
        End If

        If chkInactive.Checked And Trim(txtInActiveRem.Text) = "" Then
            MsgBox("Reason for InActive!", MsgBoxStyle.Information, Me.Text)
            txtInActiveRem.Focus()
            Exit Function
        End If
        If validateEmail(Trim(txtEmail.Text)) = True Or Trim(txtEmail.Text) = "" Then
        Else
            MsgBox("EmailID is not valid!!", vbInformation, Me.Text)
            txtEmail.Focus()
            Exit Function
        End If

        If Not gblnUserBased Then
            If Trim(cboRole.Text) = "" Then
                MsgBox("Role Name should not be blank!!", MsgBoxStyle.Information, Me.Text)
                cboRole.Focus()
                Exit Function
            End If
            clsSVT.getDataLongValue("Select RoleId from MstRole where RoleCode='" & cboRole.Text & "' And inactive = 'N'", 0, blnFound)
            If blnFound = False Then
                MsgBox("Selected RoleCode Not Found in the Master", MsgBoxStyle.Information, Me.Text)
                cboRole.Focus()
                Exit Function
            End If
        End If

        If Not validOthFLds(Me, strTblName, strIDName, Val(txtID.Text)) Then
            Exit Function
        End If


        ValidFields = True
    End Function
    Private Function ValidPwdRules() As Boolean
        PnlPwdRules.Visible = False
        ValidPwdRules = False
        If txtPassword.Text = "" Then
            lblMinSplChars.ForeColor = Color.Black
            lblMinNum.ForeColor = Color.Black
            lblMinAlpha.ForeColor = Color.Black
            lblMinLCase.ForeColor = Color.Black
            lblMinUCase.ForeColor = Color.Black
            lblMinLen.ForeColor = Color.Black
            Exit Function
        End If
        If gblnPwdRule Then
            PnlPwdRules.Visible = True
        Else
            PnlPwdRules.Visible = False
        End If
        Dim intNumbers As Integer, intChars As Integer, intUcase As Integer, intLCase As Integer
        Dim intSplChars As Integer
        Dim input As String = Trim(txtPassword.Text)

        For Each c As Char In input
            If Asc(c) >= Asc(0) And Asc(c) <= Asc(9) Then
                intNumbers = intNumbers + 1
            ElseIf Asc(c) >= Asc("a") And Asc(c) <= Asc("z") Then
                intLCase = intLCase + 1
            ElseIf Asc(c) >= Asc("A") And Asc(c) <= Asc("Z") Then
                intUcase = intUcase + 1
            Else
                intSplChars = intSplChars + 1
            End If
        Next

        intChars = intLCase + intUcase

        If Len(txtPassword.Text) < intMinLen Then
            lblMinLen.ForeColor = Color.Red
        Else
            lblMinLen.ForeColor = Color.Green
        End If

        If intSplChars < intMinSplChrs Then
            lblMinSplChars.ForeColor = Color.Red
        Else
            lblMinSplChars.ForeColor = Color.Green
        End If
        If intNumbers < intMinNumeric Then
            lblMinNum.ForeColor = Color.Red
        Else
            lblMinNum.ForeColor = Color.Green
        End If

        If intChars < intMinAlpha Then
            lblMinAlpha.ForeColor = Color.Red
        Else
            lblMinAlpha.ForeColor = Color.Green
        End If
        If intUcase < intMinUCase Then
            lblMinUCase.ForeColor = Color.Red
        Else
            lblMinUCase.ForeColor = Color.Green
        End If
        If intLCase < intMinLCase Then
            lblMinLCase.ForeColor = Color.Red
        Else
            lblMinLCase.ForeColor = Color.Green
        End If

        If lblMinSplChars.ForeColor = Color.Red Or lblMinNum.ForeColor = Color.Red Or lblMinUCase.ForeColor = Color.Red Or lblMinLCase.ForeColor = Color.Red Or
                lblMinLen.ForeColor = Color.Red Then
            ValidPwdRules = False
            Exit Function
        End If

        ValidPwdRules = True
    End Function

    Private Sub getPwdRules()
        Dim RsTemp As oledb.oledbDataReader
        Dim strsql As String

        strsql = "Select PwdMinSplChrs,PwdMinNumeric,PwdMinAlpha,PwdMinLen,PwdMinLCase,PwdMinUCase from UtlMisc"
        RsTemp = clsSVT.getDataReaderFromSQL(strsql)

        With RsTemp
            If .Read() Then
                intMinSplChrs = GetVal(RsTemp("PwdMinSplChrs"), 0)
                intMinNumeric = GetVal(RsTemp("PwdMinNumeric"), 0)
                intMinAlpha = GetVal(RsTemp("PwdMinAlpha"), 0)
                intMinLen = GetVal(RsTemp("PwdMinLen"), 0)
                intMinLCase = GetVal(RsTemp("PwdMinLCase"), 0)
                intMinUCase = GetVal(RsTemp("PwdMinUCase"), 0)
            End If
            .Close()
            lblMinSplChars.Text = "Min.no of Special Characters: " & intMinSplChrs
            lblMinNum.Text = "Min.no of Numeric: " & intMinNumeric
            lblMinAlpha.Text = "Min.no of Alphabets: " & intMinAlpha
            lblMinUCase.Text = "Min.no of Upper case: " & intMinUCase
            lblMinLCase.Text = "Min.no of Lower case: " & intMinLCase
            lblMinLen.Text = "Min. Length of Password: " & intMinLen
        End With
    End Sub

    Private Function saveDetails() As Boolean
        Dim strSQL As String = ""
        Dim strValues As String = ""
        Dim strFlds As String = ""
        saveDetails = False
        Dim intID As Long

        If Mode = "A" Then
            intID = clsSVT.AutoIncrementedNum(strIDName, strTblName)
            txtID.Text = intID

            strSQL = " Insert into " & strTblName & " ("
            strSQL = strSQL & strIDName & "," & strCodeName & ",UserName, InActive, InActiveRmks,Authorised,Authby,AuthOn,"
            strSQL = strSQL & "LoginPwd, EmailID,"
            strSQL = strSQL & " MobileNo, Department, chkDOB, DOB,PwdResetDt, RoleId"

            '--4 begin OthGRID
            Call GetInsertString(Me, strValues, strFlds)

            strSQL = strSQL & strFlds & ") values ("
            strSQL = strSQL & intID & ","
            strSQL = strSQL & "'" & txtCode.Text & "', "
            strSQL = strSQL & "'" & txtName.Text & "', "
            strSQL = strSQL & "'" & getCheckString(chkInactive) & "',"
            strSQL = strSQL & "'" & Trim(GetVal(txtInActiveRem.Text, "")) & "',"
            If Not blnAuthReq Then
                strSQL = strSQL & "'Y',"
                strSQL = strSQL & "'" & gUserCode & "',"
                strSQL = strSQL & "'" & getDateFormatInInsert(getSystemDateTime, True) & "',"
            Else
                strSQL = strSQL & "'N',"
                strSQL = strSQL & "'',"
                strSQL = strSQL & "Null,"
            End If
            strSQL = strSQL & "'" & svtCrypto.Encrypt(Trim(txtPassword.Text)) & "',"
            strSQL = strSQL & "'" & Trim(txtEmail.Text) & "',"
            strSQL = strSQL & "'" & txtPhoneNo.Text & "',"

                strSQL = strSQL & "'" & Trim(txtDept.Text) & "',"

                If chkDOB.Checked = True Then
                    strSQL = strSQL & "'Y',"
                    strSQL = strSQL & "'" & Format(DtpDOB.Value, "dd MMM yyyy") & "',"
                Else
                    strSQL = strSQL & "'N',"
                    strSQL = strSQL & "Null,"
                End If
                strSQL = strSQL & "'" & getDateFormatInInsert(dtPwdReset.Value, True) & "',"
                strSQL = strSQL & "" & clsSVT.getDataLongValue("Select RoleId from MstRole where RoleCode='" & cboRole.Text & "'", 0, False)
                '--5 begin OthGRID
                strSQL = strSQL & strValues
            Else

            strSQL = "Update  " & strTblName & " set "
            strSQL = strSQL & strFldCodeName & "  = '" & txtName.Text & "',"
            strSQL = strSQL & " InActive = '" & getCheckString(chkInactive) & "',"
            strSQL = strSQL & " InActiveRmks = '" & Trim(GetVal(txtInActiveRem.Text, "")) & "',"
            'strSQL = strSQL & " ChangedBy = '" & gUserCode & "',"
            'strSQL = strSQL & " ChangedOn = '" & getDateFormatInInsert(getSystemDateTime, True) & "',"
            strSQL = strSQL & " LoginPwd = '" & svtCrypto.Encrypt(txtPassword.Text) & "',"
            strSQL = strSQL & " EmailID = '" & txtEmail.Text & "',"
            strSQL = strSQL & " MobileNo = '" & txtPhoneNo.Text & "',"
            strSQL = strSQL & " Department = '" & txtDept.Text & "',"
            strSQL = strSQL & " PwdResetdt = '" & getDateFormatInInsert(dtPwdReset.Value, True) & "',"
            If chkDOB.Checked = True Then
                strSQL = strSQL & "DOB = '" & Format(DtpDOB.Value, "dd MMM yyyy") & "',"
                strSQL = strSQL & "ChkDOB = " & "'Y',"
            Else
                strSQL = strSQL & "DOB = " & "Null" & ","
                strSQL = strSQL & "ChkDOB = " & "'N',"
            End If
            strSQL = strSQL & "RoleID=" & clsSVT.getDataLongValue("Select RoleId from MstRole where RoleCode='" & cboRole.Text & "'", 0, False)
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
                    MsgBox(strCodeName & " '" & Trim(txtCode.Text) & "' added successfully.", vbInformation, Me.Text)
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
                txtName.Text = GetVal(RsTemp("UserNAme"), "")
                Call ApplyChkToCheckBox(chkInactive, GetVal(!Inactive, "N"))
                txtInActiveRem.Text = GetVal(RsTemp("InActiveRmks"), "")
                txtPassword.Text = svtCrypto.Decrypt(GetVal(RsTemp("LoginPwd")))
                txtCnfPassword.Text = txtPassword.Text
                txtEmail.Text = GetVal(RsTemp("EmailID"))
                txtPhoneNo.Text = GetVal(RsTemp("MobileNo"))
                txtDept.Text = GetVal(RsTemp("Department"))
                dtPwdReset.Text = GetVal(RsTemp("PwdResetdt"), getSystemDate)
                If GetVal(RsTemp("ChkDOB"), "") = "Y" Then
                    chkDOB.Checked = True
                    DtpDOB.Text = GetVal(RsTemp("DOB"))
                    DtpDOB.Visible = True
                Else
                    chkDOB.Checked = False
                    DtpDOB.Visible = False

                End If
                cboRole.Text = clsSVT.getDataStringValue("Select RoleCode from MstRole where RoleId=" & GetVal(RsTemp("RoleID")), "", False)
                Call GetUserImage()
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
        If gblnUserBased AndAlso gUserCode = "ADMIN" Then btnRights.Visible = True
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
        If Not blnDeleteOK(strTblName, Val(txtID.Text), True, True) Then
            Exit Sub
        End If

        If MsgBox("Are you sure you want to delete the " & strFldCaption &
            " : '" & txtCode.Text & "'?", vbCritical + MsgBoxStyle.YesNo + vbDefaultButton2, "Confirmation") = MsgBoxResult.Yes Then
            Try
                svtTrn = svtConn.BeginTransaction(IsolationLevel.ReadCommitted)
                Call RecordAuditLog(strFormCaption, Val(txtID.Text), UserActions.DeleteAction, strTblName, strAuditHdrSQL, "", svtTrn)

                If Not clsSVT.execSQLStatement("Delete from " & strTblName & " where " & strIDName & " = " & Val(txtID.Text), svtTrn) Then
                    svtTrn.Rollback()
                    Exit Sub
                End If
                MsgBox("Deleted Successfully!", MsgBoxStyle.Information, Me.Text)
                svtTrn.Commit()
                Call clearFlds()
            Catch ex As Exception
                svtTrn.Rollback()
                Exit Sub
            End Try
        Else
            Exit Sub

        End If
    End Sub

    Public Sub initForm(ByVal argDocID As Integer, ByVal argTblName As String, ByVal argIDName As String, ByVal argCodeName As String, ByVal argFldCodeName As String, ByVal argFormCaption As String, ByVal argFldCaption As String, Optional ByVal argFromIM As Boolean = False)

        If argFromIM Then
            blnFromIM = True
        End If
        argDocID = 18
        If argDocID > 0 Then
            intDocID = argDocID

            Dim rs As oledb.oledbDataReader
            rs = clsSVT.getDataReaderFromSQL("Select * from UtlTransactions where DocID = 18")
            With rs
                If Not .HasRows Then
                    Exit Sub
                End If

                .Read()
                strTblName = !HdrTblName
                strIDName = Replace(!HdrTblName, "Mst", "") & "ID"
                strCodeName = Replace(!HdrTblName, "Mst", "") & "Code"
                strFldCodeName = Replace(!HdrTblName, "Mst", "") & "Name"
                strFrmPfx = Trim(Replace(!HdrTblName, "Mst", ""))
                strFormCaption = !DocName
                lblFrm.Text = strFormCaption
                strFldName = argCodeName
                strFldCaption = Trim(Replace(!Docname, "Master", "Name"))
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
                If InStr(UCase(!DocName), "MASTER") > 0 Then
                    lblCode.Text = TITLECASE(Trim(Replace(UCase(!Docname), "MASTER", "Code")))
                    lblName.Text = TITLECASE(Trim(Replace(UCase(!Docname), "MASTER", "Name")))
                Else
                    lblCode.Text = !Docname & " Code"
                    lblName.Text = !Docname & " Name"
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
            strFldCodeName = "UserName"
            strFormCaption = argFormCaption
            strFldCaption = argFldCaption
            strFrmPfx = "User"
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

        Call setSearchGridHt("FULL")
        If strUserText = Trim(txtSearch.Text) Then Exit Sub
        StrQry = "Select Top 30  " & strCodeName & "," & strFrmPfx & "Name"
        StrQry &= " from " & strTblName
        StrQry &= " where (" & strCodeName & " like '%" & Trim(txtSearch.Text) & "%'"
        StrQry &= " or " & strFrmPfx & "Name Like '%" & Trim(txtSearch.Text) & "%')"
        StrQry &= " order by 1"
        Call showWaitCursor()
        dtUser = clsSVT.getDATATable(StrQry)
        dvUser = New DataView(dtUser)
        vsfSearch.DataSource = dvUser
        Call showNormalCursor()
        vsfSearch.Refresh()
        vsfSearch.ScrollBars = ScrollBars.Both
        vsfSearch.AutoSizeCols()
        strUserText = Trim(txtSearch.Text)

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
                svtAttach.ViewMode = False

                lblFrm.Text = "Add:" & Me.Text
                svtBtnCtrl.PerformAction(argBtnClicked)
                txtCode.Focus()
                txtCode.ReadOnly = False
                'txtCode.Enabled = True
                Me.Parent.Tag = "A"
            Case svtBtns.svtClose
                Call frmMain.CloseSelectedTabPage(Me)

            Case svtBtns.svtEdit
                If Trim(txtID.Text) = "" Then
                    Exit Sub
                End If
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
                svtAttach.ViewMode = False
                Call chkSplRights_Std(Me, intDocID, strRights)
                Call chkSplRights_DOC(Me, intDocID, strRights, vsfHdr)
                Call setSearchGridHt("")
                Call enableDisableFlds(True)
                'txtCode.ReadOnly = True
                txtName.Focus()

                lblFrm.Text = "Edit:" & Me.Text
                svtBtnCtrl.PerformAction(argBtnClicked)
                Me.Parent.Tag = "M"
            Case svtBtns.svtSave
                vsfHdr.Enabled = False
                Call stringReplace(Me)
                If ValidFields() Then
                    Call saveDetails()
                    svtBtnCtrl.PerformAction(argBtnClicked)
                    lblFrm.Text = "View:" & Me.Text
                End If
                vsfHdr.Enabled = True
                Me.Parent.Tag = ""
            Case svtBtns.svtFind
                If Not chkUserRights(strRights, chkRightsPos.ViewRights, Me.Text) Then Exit Sub
                Call showViewForm(Me, strTblName, "Listing of " & Me.Text, "", "", " and " & strCodeName & " <> '-'", strFormCaption, "", intDocID)
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
                Me.Parent.Tag = ""
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
            txtInActiveRem.Focus()
        Else
            chkInactive.Text = "Inactive"
            txtInActiveRem.Visible = False
            txtInActiveRem.Enabled = False
            txtInActiveRem.Text = ""
        End If
    End Sub

    Private Sub txtCode_LostFocus(sender As Object, e As EventArgs) Handles txtCode.LostFocus
        txtName.Text = txtCode.Text
    End Sub

    Private Sub frmMstGeneral_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        txtSearch.Focus()
    End Sub

    Private Sub chkInactive_LostFocus(sender As Object, e As EventArgs) Handles chkInactive.LostFocus
        'If vsfHdr.Visible = True Then
        '    vsfHdr.Row = 0
        '    vsfHdr.Col = vOthFldDataValue
        '    vsfHdr.Focus()
        'Else
        '    Exit Sub
        'End If

    End Sub

    Private Sub chkDOB_CheckedChanged(sender As Object, e As EventArgs) Handles chkDOB.CheckedChanged
        If chkDOB.Checked Then
            DtpDOB.Visible = True
        Else
            DtpDOB.Visible = False
        End If
    End Sub

    Private Sub btnLoad_Click(sender As Object, e As EventArgs) Handles btnRole.Click
        Dim intRoleDocID As Int32 = clsSVT.getDataLongValue("Select DocID from UtlTransactions Where DocName = 'Role Master'", 0, False)
        If intRoleDocID = 0 Then
            MsgBox("Role master not available in UtlTransactions")
            Exit Sub
        End If
        Dim intRoleID As Int32 = clsSVT.getDataLongValue("Select RoleID from MstRole where RoleCode = '" & Trim(cboRole.Text) & "'", 0, False)
        Dim objfrm As New frmMstRole
        objfrm.initForm(intRoleDocID, "MstRole", "RoleID", "RoleCODE", "RoleNAME", "Role Master", "RoleCODE")
        objfrm.FillDocDetails(intRoleID)
        frmMain.OpenForms(objfrm, "Role Master")
    End Sub
    Private Sub btnRmvImg_Click(sender As System.Object, e As System.EventArgs) Handles btnRmvImg.Click
        If MsgBox("Are you sure you want to remove the user's image?", vbQuestion + vbYesNo + vbDefaultButton2, Me.Text) = MsgBoxResult.Yes Then
            If clsSVT.execSQLStatement("Update MstUser set UserImg = NULL where UserID = " & Val(txtID.Text)) Then
                PicBox.Image = Nothing
            End If
        End If
        If PicBox.Image Is Nothing Then
            btnRmvImg.Visible = False
        Else
            btnRmvImg.Visible = True
        End If
    End Sub

    Private Sub PicBox_Click(sender As Object, e As EventArgs) Handles PicBox.Click
        If Mode = "A" Then
            MsgBox("Please save the data & choose the User Image in Edit Mode!", vbInformation, Me.Text)
            Exit Sub
        End If

        Call SaveImage()
        Call GetUserImage()

    End Sub
    Public Shared Function GetImageByte(ByVal imagePath As String) As Byte()
        Dim stream As FileStream = New FileStream(imagePath, FileMode.Open, FileAccess.Read)
        Dim reader As BinaryReader = New BinaryReader(stream)
        Dim img() As Byte = reader.ReadBytes(stream.Length)

        reader.Close()
        stream.Close()

        Return img
    End Function

    Private Sub SaveImage()
        Dim strSql As String = "", dblLen As Double
        Dim idb As New OpenFileDialog
        If Not chkUserRights(strRights, chkRightsPos.AddRights, Me.Text) Then Exit Sub
        idb.Title = "Select your image."
        idb.Filter = "Image Files|*.jpg;"
        idb.RestoreDirectory = False
        Dim img As Byte() = Nothing
        Try
            If idb.ShowDialog() = DialogResult.OK Then
                Dim ImgFileInfo As New FileInfo(idb.FileName)
                dblLen = Math.Round(ImgFileInfo.Length / 1024 / 1024, 2)
                If dblLen > 3 Then
                    MsgBox("Please select an image less than THREE MB. Currently it is " & dblLen & " MB", MsgBoxStyle.Critical, Me.Text)
                    Exit Sub
                End If
                imagePath = idb.FileName
            Else
                Exit Sub
            End If

        Catch ex As Exception
            MsgBox("Error in image selection!" & vbCrLf & ex.Message, MsgBoxStyle.Critical, Me.Text)
            Exit Sub
        End Try

        Dim Extension As String = Path.GetExtension(imagePath)

        Try
            If StrComp(Extension, ".jpg", CompareMethod.Text) = 0 Or
               StrComp(Extension, ".jpeg", CompareMethod.Text) = 0 Then


                img = GetImageByte(imagePath)


                Dim cmd As oledb.oledbCommand 

                strSql = "Update MstUser set UserImg = ? where UserID = " & Val(txtID.Text)
                cmd = New oledb.oledbCommand (strSql, svtConn)
                cmd.Parameters.AddWithValue("@Image", img)


                If cmd.ExecuteNonQuery() Then
                    MsgBox("Image updated successfully for the user: '" & (txtCode.Text) & "'", vbInformation, Me.Text)
                End If
                Exit Sub
            Else
                MsgBox("Please enter only jpg/jpeg images!", vbInformation, Me.Text)
                Exit Sub
            End If
        Catch
            MsgBox("Error while update image!", vbCritical, Me.Text)
        End Try
    End Sub

    Private Sub PicBox_MouseEnter(sender As Object, e As System.EventArgs) Handles PicBox.MouseEnter
        Me.Cursor = Cursors.Hand
    End Sub

    Private Sub PicBox_MouseLeave(sender As Object, e As System.EventArgs) Handles PicBox.MouseLeave
        Me.Cursor = Cursors.Default
    End Sub


    Private Sub PicBox_Paint(sender As Object, e As System.Windows.Forms.PaintEventArgs) Handles PicBox.Paint
        If PicBox.Image Is Nothing Then
            Dim canvas As Graphics = e.Graphics
            Dim mainFont As Font
            Dim textStyle As New FontStyle
            mainFont = New Font("Consolas", 10, textStyle)
            Dim textArea As Rectangle = New Rectangle(0, 0, PicBox.Width, PicBox.Height)
            Dim textFormat As StringFormat = New StringFormat
            textFormat.LineAlignment = StringAlignment.Center
            'added this line
            textFormat.Alignment = StringAlignment.Center
            e.Graphics.DrawRectangle(Pens.Black, textArea)
            e.Graphics.DrawString("Click here to add user image.", mainFont, Brushes.Black, textArea, textFormat)
            canvas = Nothing
        End If
    End Sub

    Private Sub GetUserImage()
        Dim strSql As String = ""
        Dim cmd As oledb.oledbCommand = Nothing
        PicBox.Image = Nothing
        strSql = "select " & getImgSyntax("ISNULL", "UserImg") & " as UserImg from MstUser where UserID = " & Val(txtID.Text)
        cmd = New oledb.oledbCommand (strSql, svtConn)
        Dim imageData As Byte() = DirectCast(cmd.ExecuteScalar(), Byte())
        If imageData.Length = 0 Then
            btnRmvImg.Visible = False
            ' Exit Sub
        End If
        If Not imageData Is Nothing Then
            If imageData.Length > 0 Then
                Dim ImageMemoryStream As MemoryStream = New IO.MemoryStream(GetUserImageBytesFromOLEField(imageData))
                PicBox.Image = Image.FromStream(ImageMemoryStream, True)
                PicBox.SizeMode = PictureBoxSizeMode.StretchImage
                btnRmvImg.Visible = True
            Else
                PicBox.Image = Nothing
                btnRmvImg.Visible = False
            End If
        End If
    End Sub

    Friend Shared Function GetUserImageBytesFromOLEField(ByVal oleFieldBytes() As Byte) As Byte()

        Dim BITMAP_ID_BLOCK As String = "BM"
        Dim JPG_ID_BLOCK As String = ChrW(&HFF).ToString() & ChrW(&HD8).ToString() & ChrW(&HFF).ToString()
        Dim PNG_ID_BLOCK As String = ChrW(&H89).ToString() & "PNG" & vbCrLf & ChrW(&H1A).ToString() & vbLf
        Dim GIF_ID_BLOCK As String = "GIF8"
        Dim TIFF_ID_BLOCK As String = "II*" & ChrW(&H0).ToString()

        Dim imageBytes() As Byte

        ' Get a UTF7 Encoded string version
        Dim u7 As System.Text.Encoding = System.Text.Encoding.UTF7
        Dim strTemp As String = u7.GetString(oleFieldBytes)
        Dim st2 As String = System.Text.Encoding.UTF8.GetString(oleFieldBytes)
        ' Get the first 300 characters from the string
        Dim strVTemp As String = strTemp.Substring(0, 300)

        ' Search for the block
        Dim iPos As Integer = -1
        If strVTemp.IndexOf(BITMAP_ID_BLOCK) <> -1 Then
            iPos = strVTemp.IndexOf(BITMAP_ID_BLOCK)
        ElseIf strVTemp.IndexOf(JPG_ID_BLOCK) <> -1 Then
            iPos = strVTemp.IndexOf(JPG_ID_BLOCK)
        ElseIf strVTemp.IndexOf(PNG_ID_BLOCK) <> -1 Then
            iPos = strVTemp.IndexOf(PNG_ID_BLOCK)
        ElseIf strVTemp.IndexOf(GIF_ID_BLOCK) <> -1 Then
            iPos = strVTemp.IndexOf(GIF_ID_BLOCK)
        ElseIf strVTemp.IndexOf(TIFF_ID_BLOCK) <> -1 Then
            iPos = strVTemp.IndexOf(TIFF_ID_BLOCK)
        Else
            Throw New Exception("Unable to determine header size for the OLE Object")
        End If


        ' From the position above get the new image
        If iPos = -1 Then
            Throw New Exception("Unable to determine header size for the OLE Object")
        End If

        imageBytes = New Byte(CInt(oleFieldBytes.LongLength - iPos - 1)) {}
        Array.ConstrainedCopy(oleFieldBytes, iPos, imageBytes, 0, oleFieldBytes.Length - iPos)

        Return imageBytes
    End Function

    Private Sub btnRights_Click(sender As Object, e As EventArgs) Handles btnRights.Click
        If txtCode.Text = "" Then Exit Sub
        With frmMstRights
            .strFrmPrfx = strFrmPfx
            .IntID = txtID.Text
            .ShowDialog()
        End With
    End Sub

   Private Sub txtPassword_KeyUp(sender As Object, e As KeyEventArgs) Handles txtPassword.KeyUp
        If Mode = "V" Then Exit Sub
        If txtPassword.Text <> "" Then
            Call ValidPwdRules()
        End If
    End Sub

    Private Sub txtPassword_GotFocus(sender As Object, e As EventArgs) Handles txtPassword.GotFocus
        If txtPassword.Text <> "" Then
            'PnlPwdRules.Visible = True
            Call ValidPwdRules()
        Else
            PnlPwdRules.Visible = False
        End If
    End Sub

    Private Sub txtPassword_LostFocus(sender As Object, e As EventArgs) Handles txtPassword.LostFocus
        PnlPwdRules.Visible = False
    End Sub

    Private Sub svtBtnCtrl_btnOptionSelected(argOptionName As String, argBlnCtrlClick As Boolean, argBlnAltClick As Boolean) Handles svtBtnCtrl.btnOptionSelected
        Select Case argOptionName.ToUpper
            Case "CHANGE ADNL.ADMIN PASSWORD"
                Dim frmAU As New frmMstUserAdmPwdChange
                With frmAU
                    .ShowDialog()
                End With

            Case "APPROVE"

                If Trim(txtCode.Text) = "" Then Exit Sub
                If Not chkUserRights(strRights, chkRightsPos.AuthRights, Me.Text) Then Exit Sub
                If UCase(lblApproved.Text) <> "APPROVED" Then
                    If MsgBox("Do you want to Approve this " & Replace(strFormCaption, "Master", "") & " : '" & txtCode.Text & "' ?", vbQuestion + vbYesNo + vbDefaultButton1, Me.Text) = vbYes Then
                        clsSVT.execSQLStatement("Update " & strTblName & " set Authorised='Y', AuthBy='" & gUserCode & "', AuthOn=" & getDateFormatInCond(getSystemDate) & " where " & strIDName & "=" & Val(txtID.Text))
                        lblApproved.Text = "APPROVED"
                        lblApproved.ForeColor = Color.Green

                        'for audit track
                        Call RecordAuditLog(strFormCaption, Val(txtID.Text), UserActions.EditAction, strTblName, strAuditHdrSQL, strAuditDtlSQL)

                        Exit Sub
                    End If
                ElseIf UCase(lblApproved.Text) <> "UN-APPROVED" Then
                    If MsgBox(Replace(strFormCaption, "Master", "") & " : '" & txtCode.Text & "' is already approved." & vbCrLf & "Do you want to revoke it?", vbQuestion + vbYesNo + vbDefaultButton1, Me.Text) = vbYes Then
                        clsSVT.execSQLStatement("Update " & strTblName & " set Authorised='N', AuthBy='', AuthOn=NULL where " & strIDName & "=" & Val(txtID.Text))
                        lblApproved.Text = "UN-APPROVED"
                        lblApproved.ForeColor = Color.FromArgb(192, 0, 0)

                        'for audit track
                        Call RecordAuditLog(strFormCaption, Val(txtID.Text), UserActions.EditAction, strTblName, strAuditHdrSQL, strAuditDtlSQL)

                        Exit Sub
                    End If
                End If
            Case "FIELD LEVEL RIGHTS"
                Dim argFrm As New frmMstFldRights
                argFrm.ShowDialog()

            Case "MASTER APPROVALS"
                Dim argFrm As New frmUtlMstApprovals
                argFrm.ShowDialog()
        End Select
    End Sub

    Private Sub svtAttach_Load(sender As Object, e As EventArgs) Handles svtAttach.Load

    End Sub

    Private Sub pnlCodeName_Paint(sender As Object, e As PaintEventArgs) Handles pnlCodeName.Paint

    End Sub

    Private Sub txtCode_TextChanged(sender As Object, e As EventArgs) Handles txtCode.TextChanged

    End Sub

    Private Sub txtName_TextChanged(sender As Object, e As EventArgs) Handles txtName.TextChanged

    End Sub

    Private Sub txtDept_TextChanged(sender As Object, e As EventArgs) Handles txtDept.TextChanged

    End Sub

    Private Sub DtpDOB_ValueChanged(sender As Object, e As EventArgs) Handles DtpDOB.ValueChanged

    End Sub

    Private Sub txtEmail_TextChanged(sender As Object, e As EventArgs) Handles txtEmail.TextChanged

    End Sub

    Private Sub txtPhoneNo_TextChanged(sender As Object, e As EventArgs) Handles txtPhoneNo.TextChanged

    End Sub

    Private Sub cboRole_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboRole.SelectedIndexChanged

    End Sub

    Private Sub txtInActiveRem_TextChanged(sender As Object, e As EventArgs) Handles txtInActiveRem.TextChanged

    End Sub

    Private Sub dtPwdReset_ValueChanged(sender As Object, e As EventArgs) Handles dtPwdReset.ValueChanged

    End Sub

    Private Sub DtpDOB_Validating(sender As Object, e As CancelEventArgs) Handles DtpDOB.Validating
        If DtpDOB.Value > getSystemDate() Then
            MsgBox("Not a valid DOB ", MsgBoxStyle.Information, Me.Text)
            DtpDOB.Focus()
            Exit Sub
        End If
    End Sub

    Private Sub dtPwdReset_LostFocus(sender As Object, e As EventArgs) Handles dtPwdReset.LostFocus
        If vsfHdr.Visible = True Then
            vsfHdr.Row = 0
            vsfHdr.Col = vOthFldDataValue
            vsfHdr.Focus()
            vsfHdr.StartEditing()
        Else
            Exit Sub
        End If
    End Sub

    Private Sub imgEye_MouseLeave(sender As Object, e As EventArgs) Handles imgEye.MouseLeave
        If Mode = "V" Then Exit Sub
        txtPassword.PasswordChar = "*"
        Try
            imgEye.Image = Image.FromFile(gstrRptPath & "imgDontShow.png")
        Catch ex As Exception
        End Try
    End Sub

    Private Sub imgEye_MouseEnter(sender As Object, e As EventArgs) Handles imgEye.MouseEnter
        If Mode = "V" Then Exit Sub
        txtPassword.PasswordChar = ""
        Try
            imgEye.Image = Image.FromFile(gstrRptPath & "imgShow.png")
        Catch
        End Try
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

    Private Sub vsfHdr_AfterResizeColumn(sender As Object, e As RowColEventArgs) Handles vsfHdr.AfterResizeColumn
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
        Dim StrQry As String, strCond As String = ""
        txtSearch.Text = ""
        If vsfSearch.Visible = True Then
            vsfSearch.Visible = False
        Else
            Call setSearchGridHt("FULL")
            StrQry = "select top 7 " & strCodeName & "," & "UserName" & " from " & strTblName & " order by 1"
            Call showWaitCursor()
            dtUser = clsSVT.getDATATable(StrQry)
            dvUser = New DataView(dtUser)
            vsfSearch.DataSource = dvUser
            Call showNormalCursor()
            vsfSearch.Refresh()
            vsfSearch.ScrollBars = ScrollBars.Both
            vsfSearch.AutoSizeCols()
        End If
    End Sub
End Class