Imports System.ComponentModel
Imports System.Data.oledb
Imports System.Text.RegularExpressions
Imports System.IO
Imports C1.Win.C1FlexGrid

Public Class frmMstPartyContactPerson
    Dim clsSVT As New svtLibrary
    Public Mode As String
    Dim intPartyID As Integer
    Dim intdocID As Integer
    Public strPartyIDFld As String
    Public strRights As String
    Public strfrmCaption As String
    Dim imagePath As String

    Dim strPartyTblName As String
    Dim strFldPartyID As String
    Dim strFldPartyCode As String
    Dim strFldPartyName As String

    Const vDetCID = 0
    Const vDetPartyID = 1
    Const vDetDOcId = 2
    Const vDetCName = 3
    Const vdetCDepatment = 4
    Const vDetCDesig = 5
    Const vDetCMobNo = 6
    Const vDetCPhoneNo = 7
    Const vDetFax = 8
    Const vDetEmail = 9
    Const vDetCDispName = 10
    Const vDetCRemarks = 11
    Const vdetdefault = 12
    Const vdetImage = 13
    Const vdetDetails = 14


    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub frmMstContactPerson_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Call ChangeFontSize(Me)
        Call initFormVsfDet(Me, Me.Text, vsfDet, "DET")
    End Sub


    Public Sub initScreen()
        KeyPreview = True
        Call initFormSize(Me, Me.Text, False, 939, 483, 0, 0)

        With vsfDet
            .Rows.Count = 1
            .Cols.Count = 15

            .SetData(0, vDetCID, "CID")
            .SetData(0, vDetPartyID, "CustID")
            .SetData(0, vDetDOcId, "DocID")
            .SetData(0, vDetCName, "Contact Person Name")
            .SetData(0, vdetCDepatment, "Department")
            .SetData(0, vDetCDesig, "Designation")
            .SetData(0, vDetCMobNo, "Mobile Number")
            .SetData(0, vDetCPhoneNo, "Phone Number")
            .SetData(0, vDetFax, "Fax")
            .SetData(0, vDetEmail, "EmailID")
            .SetData(0, vDetCDispName, "Display Name")
            .SetData(0, vDetCRemarks, "Remarks")
            .SetData(0, vdetDetails, "Contact Details")
            .SetData(0, vdetdefault, "Default")
            .SetData(0, vdetImage, "Image")
            .Cols(vDetCDispName).Visible = False
            .Cols(vdetImage).ComboList = "..."
            .Cols(vdetImage).Width = 125
            'Call SetColWidth(vsfDet, 30, 30)
        End With

        '.SetData(0, vDetCDesig, "Designation")
        '.SetData(0, vDetFax, "Fax")
        Call SetHideCol(vsfDet, True, vDetCID, vDetCName, vDetPartyID, vDetCDispName, vdetdefault, vDetCRemarks, vDetCMobNo, vDetDOcId, vDetCPhoneNo, vDetFax, vDetEmail, vDetCDesig, vdetCDepatment)
        'Call SetColWidth(vsfDet, 15, 15, 250, 150, 100, 100, 100, 100, 250, 250)
        Call SetColAlign(vsfDet, svtAlign.Left, vDetCMobNo, vDetCPhoneNo)
        fraEditvsfDetVisible(True)
        txtPartyCode.Enabled = False
        '  txtCustName.Enabled = False
        fraEdit.Visible = False
        vsfDet.Visible = True
        txtName.Focus()
        Me.Text = strfrmCaption & " Contact Person(s)"
        lblPartyCode.Text = strfrmCaption & " Code "
        ' lblName.Text = strfrmCaption & " Name "
        initGridStyles(Me)
        Call GetPartyDtls()
        Call FillDocDetails(intPartyID)

        If Not chkUserRights(strRights, chkRightsPos.AddRights, Me.Text, False) Then btnAdd.Visible = False
        If Not chkUserRights(strRights, chkRightsPos.EditRights, Me.Text, False) Then btnEdit.Visible = False
        If Not chkUserRights(strRights, chkRightsPos.DelRights, Me.Text, False) Then btnDelete.Visible = False

        Call EnableBtn("M")

    End Sub

    Private Sub EnableBtn(ByVal argMode As String)
        If argMode <> "M" Then
            btnAdd.Enabled = False
            btnEdit.Enabled = False
            btnDelete.Enabled = False
        Else
            btnAdd.Enabled = True
            btnEdit.Enabled = True
            btnDelete.Enabled = True
        End If
    End Sub
    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        fraEdit.Visible = False
        vsfDet.Visible = True
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim svtTrn As oledb.oledbTransaction = Nothing
        Dim blnFound As Boolean, strSQL As String

        stringReplace(Me)
        txtCID.Text = clsSVT.AutoIncrementedNum("PartyCID", "MstPartyContacts")

        If Trim(txtName.Text) = "" Then
            MsgBox("Contact Person Name is Empty...", MsgBoxStyle.Information, Me.Text)
            txtName.Focus()
            Exit Sub
        End If

        If chkDefault.Checked = True Then
            strSQL = "Update MstPartyContacts set SetDefault='N'  "
            'strSQL = strSQL & " where PartyAddID  = " & Val(txtAddID.Text)
            strSQL = strSQL & " where MstDocID  = " & intdocID
            strSQL = strSQL & " and PartyID  = " & intPartyID
            clsSVT.execSQLStatement(strSQL, svtTrn)
        End If

        Try
            If Mode = "A" Then
                txtCID.Text = clsSVT.AutoIncrementedNum("PartyCId", "MstPartyContacts")
                Call clsSVT.getDataStringValue("Select CName from  MstPartyContacts " _
                   & " where PartyID =" & Val(txtPartyID.Text) & " and PartyCID <> " & Val(txtCID.Text) & " and CName= '" & Trim(txtName.Text) & "'  and MstDocID=  " & intdocID, "", blnFound)
                If blnFound Then
                    MsgBox("Contact person Name already exists..", vbInformation, Me.Text)
                    txtName.Text = ""
                    txtName.Focus()
                    Exit Sub
                End If


                strSQL = "Insert into MstPartyContacts (PartyID, MstDocId,CName,Cdepartment, CDesig, CMobNo,CPhone, "
                strSQL = strSQL & " Fax,ImgFileName, Email, CRemarks,SetDefault) values ("
                ' strSQL = strSQL & Val(txtCID.Text) & ","
                strSQL = strSQL & intPartyID & ","
                strSQL = strSQL & intdocID & ","
                strSQL = strSQL & "'" & Trim(txtName.Text) & "',"
                strSQL = strSQL & "'" & Trim(txtDept.Text) & "',"
                strSQL = strSQL & "'" & Trim(txtDesig.Text) & "',"
                strSQL = strSQL & "'" & Trim(txtMobNo.Text) & "',"
                strSQL = strSQL & "'" & Trim(txtPhone.Text) & "',"
                strSQL = strSQL & "'" & txtFax.Text & "',"
                If txtFileName.Text <> "" Then
                    strSQL = strSQL & "'" & intPartyID & "_" & txtFileName.Text & "',"
                Else
                    strSQL = strSQL & "'',"
                End If
                strSQL = strSQL & "'" & txtEmail.Text & "',"
                strSQL = strSQL & "'" & txtRemarks.Text & "',"
                If chkDefault.Checked = True Then
                    strSQL = strSQL & "'Y')"
                Else
                    strSQL = strSQL & "'N')"
                End If
                strSQL = Replace(Replace(strSQL, vbCrLf, ""), vbCr, "")
            Else
                strSQL = "Update MstPartyContacts"
                txtCID.Text = vsfDet.GetData(vsfDet.Row, vDetCID)
                strSQL = strSQL & " set CName = '" & txtName.Text & "',"
                strSQL = strSQL & " CDepartment = '" & txtDept.Text & "',"
                strSQL = strSQL & " CDesig = '" & txtDesig.Text & "',"
                strSQL = strSQL & " CMobNo = '" & txtMobNo.Text & "',"
                strSQL = strSQL & " CPhone = '" & txtPhone.Text & "',"
                strSQL = strSQL & " Fax = '" & txtFax.Text & "',"
                If Not (PicBox Is Nothing) Then
                    strSQL = strSQL & "ImgFileName = '" & intPartyID & "_" & txtFileName.Text & "',"
                Else
                    strSQL = strSQL & "ImgFileName ='',"
                End If
                'If txtFileName.Text <> "" Then
                '    strSQL = strSQL & "ImgFileName = '" & intPartyID & "_" & txtFileName.Text & "',"
                'Else
                '    strSQL = strSQL & "ImgFileName ='',"
                'End If
                strSQL = strSQL & " Email = '" & txtEmail.Text & "',"
                strSQL = strSQL & " CRemarks = '" & txtRemarks.Text & "',"
                If chkDefault.Checked = True Then
                    strSQL = strSQL & " SetDefault = 'Y'"
                Else
                    strSQL = strSQL & " SetDefault = 'N'"
                End If
                strSQL = strSQL & " where PartyCID  = " & Val(txtCID.Text)
                strSQL = strSQL & " And MstDocID  = " & Val(txtDocId.Text)
                strSQL = Replace(Replace(strSQL, vbCrLf, ""), vbCr, "")
            End If
            svtTrn = svtConn.BeginTransaction(IsolationLevel.ReadCommitted)
            If Not clsSVT.execSQLStatement(strSQL, svtTrn) Then
                svtTrn.Rollback()
                Exit Sub
            End If
            If Not (PicBox.Image Is Nothing) Then
                Call SaveContactImg()
            Else
                If System.IO.File.Exists(gstrRptPath & "custfiles\contactfiles\" & txtCID.Text & ".jpg") = True Then
                    System.IO.File.Delete(gstrRptPath & "custfiles\contactfiles\" & txtCID.Text & ".jpg")
                End If
            End If


            'If Mode = "A" Then
            '    Call RecordAuditLog("PARTY CONTACTS", Val(txtCID.Text), UserActions.AddAction, svtTrn)
            'Else
            '    Call RecordAuditLog("PARTY CONTACTS", Val(txtCID.Text), UserActions.EditAction, svtTrn)
            'End If
            MsgBox("Data Saved Successfully!", MsgBoxStyle.Information, Me.Text)
            svtTrn.Commit()

            Mode = ""
            fraEdit.Visible = False
            FillDocDetails(intPartyID)
            vsfDet.Visible = True
            'Call initScreen()
        Catch ex As Exception
            MsgBox("Error during Save" & vbCrLf & ex.Message, MsgBoxStyle.Critical, Me.Text & "[btnSave_Click]")
            svtTrn.Rollback()
        End Try
    End Sub

    Public Sub FillDocDetails(ByVal argPartyID As Integer)
        Dim rsTemp As oledb.oledbDataReader
        Dim strSql As String
        Dim strDetail As String
        fraEditvsfDetVisible(True)

        txtPartyCode.Text = clsSVT.getDataStringValue("Select " & strFldPartyCode & " from " & strPartyTblName & " where " & strFldPartyID & "=" & argPartyID, "", False)
        lblPartyName.Text = clsSVT.getDataStringValue("Select " & strFldPartyName & " from " & strPartyTblName & " where " & strFldPartyID & "=" & argPartyID, "", False)

        strSql = "Select *,  PartyID from MstPartyContacts where MstDocID = " & intdocID & " And PartyID = " & argPartyID & "order by CName"
        rsTemp = clsSVT.getDataReaderFromSQL(strSql)
        vsfDet.Rows.Count = 1
        With rsTemp
            Do While .Read()
                With vsfDet
                    vsfDet.Rows.Count = vsfDet.Rows.Count + 1
                    .SetData(vsfDet.Rows.Count - 1, vDetCID, GetVal(rsTemp("PartyCID"), 0))
                    .SetData(vsfDet.Rows.Count - 1, vDetDOcId, GetVal(rsTemp("MstDocID"), ""))
                    .SetData(vsfDet.Rows.Count - 1, vDetPartyID, GetVal(rsTemp("PartyID"), ""))
                    .SetData(vsfDet.Rows.Count - 1, vDetCName, GetVal(rsTemp("CName"), ""))
                    .SetData(vsfDet.Rows.Count - 1, vdetCDepatment, GetVal(rsTemp("CDepartment"), ""))
                    .SetData(vsfDet.Rows.Count - 1, vDetCDesig, GetVal(rsTemp("CDesig"), ""))
                    .SetData(vsfDet.Rows.Count - 1, vDetCMobNo, GetVal(rsTemp("CMobNo"), ""))
                    .SetData(vsfDet.Rows.Count - 1, vDetCPhoneNo, GetVal(rsTemp("CPhone"), ""))
                    .SetData(vsfDet.Rows.Count - 1, vDetFax, GetVal(rsTemp("Fax"), ""))
                    .SetData(vsfDet.Rows.Count - 1, vDetEmail, GetVal(rsTemp("Email"), ""))
                    .SetData(vsfDet.Rows.Count - 1, vDetCDispName, GetVal(rsTemp("CDispName"), ""))
                    .SetData(vsfDet.Rows.Count - 1, vDetCRemarks, GetVal(rsTemp("CRemarks"), ""))

                    If GetVal(rsTemp("SetDefault"), "N") = "Y" Then
                        .SetData(vsfDet.Rows.Count - 1, vdetdefault, "Y")
                    Else
                        .SetData(vsfDet.Rows.Count - 1, vdetdefault, "N")
                    End If

                    strDetail = NumberList(vbCrLf, Trim(GetVal(rsTemp("CName"), "")), Trim(GetVal(rsTemp("CDesig"), "")), Trim(GetVal(rsTemp("CDepartment"), "")), Trim(GetVal(rsTemp("CMobNo"), "")), Trim(GetVal(rsTemp("CPhone"), "")),
                     Trim(GetVal(rsTemp("Fax"), "")), Trim(GetVal(rsTemp("Email"), "")))


                    .SetData(vsfDet.Rows.Count - 1, vdetDetails, strDetail)

                    .SetData(vsfDet.Rows.Count - 1, vdetImage, "")
                    If File.Exists(gstrRptPath & "CustFiles\ContactFiles\" & GetVal(rsTemp("PartyCID"), 0) & ".jpg") Then
                        Dim imgStrm As New MemoryStream(GetImageByte(gstrRptPath & "CustFiles\ContactFiles\" & GetVal(rsTemp("PartyCID"), 0) & ".jpg"))
                        PicBox.Image = Image.FromStream(imgStrm)
                        '.SetCellImage(vsfDet.Rows.Count - 1, vdetImage, Image.FromFile(gstrCPImagePath & GetVal(rsTemp("PartyCID"), 0) & ".jpg"))
                        .SetCellImage(vsfDet.Rows.Count - 1, vdetImage, Image.FromStream(imgStrm))
                        .Styles.Normal.ImageAlign = C1.Win.C1FlexGrid.ImageAlignEnum.Stretch
                        ' .Styles.Normal.ImageAlign = ImageAlignEnum.Stretch
                        .Rows(.Rows.Count - 1).Height = 120
                    End If
                    '   Call SetCellFlexImage(vsfDet, vsfDet.Rows.Count - 1, vdetImage, gstrCPImagePath & GetVal(rsTemp("ImgFileName"), 0), , C1.Win.C1FlexGrid.ImageAlignEnum.CenterCenter)

                End With
            Loop
            .Close()

        End With
        'vsfDet.AutoSizeCols()
        ' Call SetColWidth(vsfDet, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 300)

        If vsfDet.Rows.Count = 1 Then
            lblNoofAddress.Text = "No Contacts present for this " & strfrmCaption
            btnEdit.Enabled = False
            btnDelete.Enabled = False
        Else
            lblNoofAddress.Text = "Total Contacts present for this " & strfrmCaption & " is " & vsfDet.Rows.Count - 1 & ""
            btnEdit.Enabled = True
            btnDelete.Enabled = True
        End If

        Dim cs As C1.Win.C1FlexGrid.CellStyle = vsfDet.Styles.Add(1)
        Dim blndefFound As Boolean = False
        For i = 1 To vsfDet.Rows.Count - 1
            If vsfDet.GetData(i, vdetdefault) = "Y" Then
                blndefFound = True
                cs.BackColor = Color.GreenYellow
                vsfDet.SetCellStyle(i, vdetDetails, cs)
                Exit For
            End If
        Next
        For i = 1 To vsfDet.Rows.Count - 1
            vsfDet.Rows(i).Height = 125
        Next
        'lblNoofAddress.Text = "Total records = " & vsfDet.Rows.Count - 1
    End Sub

    Private Sub fraEditvsfDetVisible(ByVal argFlag As Boolean)
        If argFlag Then
            fraEdit.Visible = False
            vsfDet.Visible = True
        Else
            fraEdit.Visible = True
            vsfDet.Visible = False
        End If
    End Sub

    Private Sub visibleEditbtn(ByVal argBLN As Boolean)
        If argBLN Then
            btnSave.Visible = True
            btnCancel.Visible = True
            btnCloseEdit.Visible = False
        Else
            Mode = ""
            btnCancel.Visible = False
            btnSave.Visible = False
            btnCloseEdit.Visible = True
        End If
    End Sub


    Private Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        If Not chkUserRights(strRights, chkRightsPos.AddRights, Me.Text) Then Exit Sub
        Mode = "A"
        Call ClearFlds()
        vsfDet.Visible = False
        fraEdit.Visible = True
        fraEdit.BringToFront()
        txtName.Focus()
        visibleEditbtn(True)
        txtName.Enabled = True
        txtDept.Enabled = True
        txtDesig.Enabled = True
        txtMobNo.Enabled = True
        txtPhone.Enabled = True
        txtEmail.Enabled = True
        txtFax.Enabled = True
        txtRemarks.Enabled = True
        btnRmvImg.Visible = False
    End Sub

    Private Sub ClearFlds()
        chkDefault.Checked = False
        txtName.Text = ""
        txtDesig.Text = ""
        txtDept.Text = ""
        txtMobNo.Text = ""
        txtPhone.Text = ""
        txtFax.Text = ""
        txtEmail.Text = ""
        txtFileName.Text = ""
        txtRemarks.Text = ""
        PicBox.Image = Nothing
        PicBox.Visible = True
    End Sub
    Private Sub LoadPartyImage()

        Dim strPartyName As String = ""
        ' gstrRptPath & "CustFiles\ContactFiles\" & txtCID.Text & ".jpg"
        PicBox.ImageLocation = Nothing
        PicBox.Image = Nothing
        Try
            With PicBox
                'madhu
                If File.Exists(gstrRptPath & "CustFiles\ContactFiles\" & txtCID.Text & ".jpg") Then
                    '.ImageLocation = gstrRptPath & "CustFiles\ContactFiles\" & txtCID.Text & ".jpg"
                    Call getImage()
                    .SizeMode = PictureBoxSizeMode.StretchImage
                    .Visible = True
                Else
                    .ImageLocation = Nothing
                    '.Image = vLQMS.My.Resources.user
                    .SizeMode = PictureBoxSizeMode.StretchImage
                    ' .Visible = False
                End If
            End With
        Catch ex As Exception
            MsgBox("Error on loading image!" & vbCrLf & ex.Message, MsgBoxStyle.Critical, Me.Text)
            Exit Sub
        End Try
    End Sub
    Private Sub vsfDet_BeforeEdit(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfDet.BeforeEdit
        e.Cancel = True
    End Sub

    Private Sub vsfDet_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles vsfDet.DoubleClick
        Call editProc("VIEW")
    End Sub

    Private Sub btnEdit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEdit.Click
        Call editProc("EDIT")
    End Sub

    Private Sub editProc(ByVal argType As String)
        If vsfDet.Rows.Count = 1 Then Exit Sub
        If vsfDet.Row <= 0 Then Exit Sub

        Dim rs As oledbDataReader
        rs = clsSVT.getDataReaderFromSQL("Select * from MstPArtyContacts where PartyCID = " & Val(vsfDet.GetData(vsfDet.Row, 0)) & "And MstDocid= " & Val(vsfDet.GetData(vsfDet.Row, 2)))
        If rs.Read Then
            txtCID.Text = GetVal(rs("PartyCID"), "")
            txtName.Text = GetVal(rs("CName"), "")
            txtDept.Text = GetVal(rs("CDepartment"), "")
            txtDesig.Text = GetVal(rs("CDesig"), "")
            txtMobNo.Text = GetVal(rs("CMobNo"), "")
            txtPhone.Text = GetVal(rs("CPhone"), "")
            txtFax.Text = GetVal(rs("Fax"), "")
            txtEmail.Text = GetVal(rs("Email"), "")
            txtRemarks.Text = GetVal(rs("CRemarks"), "")
            txtDocId.Text = GetVal(rs("MstDocId"), "")
            txtFileName.Text = GetVal(rs!ImgFileName, "")
            If txtFileName.Text <> "" Then
                txtFileName.Text.Replace(intPartyID & "_", "")
            End If
            If GetVal(rs("SetDefault"), "N") = "Y" Then
                chkDefault.Checked = True
            Else
                chkDefault.Checked = False
            End If
            rs.Close()
        Else
            Exit Sub
        End If

        fraEdit.Visible = True
        vsfDet.Visible = False
        fraEdit.BringToFront()

        If argType = "EDIT" Then
            Mode = "M"
            visibleEditbtn(True)
            txtName.Enabled = True
            txtDesig.Enabled = True
            txtDept.Enabled = True
            txtMobNo.Enabled = True
            txtPhone.Enabled = True
            txtEmail.Enabled = True
            txtFax.Enabled = True
            txtRemarks.Enabled = True
        Else
            visibleEditbtn(False)
            txtName.Enabled = False
            txtDept.Enabled = False
            txtDesig.Enabled = False
            txtMobNo.Enabled = False
            txtPhone.Enabled = False
            txtEmail.Enabled = False
            txtFax.Enabled = False
            txtRemarks.Enabled = False

        End If
        Call LoadPartyImage()
        If PicBox.Image Is Nothing Then
            btnRmvImg.Visible = False
        Else
            btnRmvImg.Visible = True
        End If
    End Sub

    Private Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        Dim strSQL As String
        If vsfDet.Rows.Count = 1 Then
            MsgBox("Contact Details Not Present for this " & strfrmCaption & "!!", vbInformation, Me.Text)
            Exit Sub
        End If
        If vsfDet.Row <= 0 Then Exit Sub
        'SetHideRow(vsfDet, True, vsfDet.Row)

        If MsgBox("Are you sure you want to delete : '" & vsfDet.GetData(vsfDet.Row, 3) _
                 & "'", vbCritical + MsgBoxStyle.YesNo + vbDefaultButton2, "Delete Confirmation") = MsgBoxResult.Yes Then
            Dim svtTrn As oledb.oledbTransaction = Nothing
            Try
                SetHideRow(vsfDet, True, vsfDet.Row)
                svtTrn = svtConn.BeginTransaction(IsolationLevel.ReadCommitted)

                'Call RecordAuditLog("PARTY CONTACTS", Val(vsfDet.GetData(vsfDet.Row, 0)), UserActions.DeleteAction, svtTrn)

                strSQL = "Delete from MstPartyContacts where PartyCID = " & Val(vsfDet.GetData(vsfDet.Row, 0)) & "And MstDocId=" & Val(vsfDet.GetData(vsfDet.Row, 2))
                Call clsSVT.execSQLStatement(strSQL, svtTrn)
                MsgBox("Deleted Successfully!", MsgBoxStyle.Information, Me.Text)

                svtTrn.Commit()
            Catch ex As Exception
                svtTrn.Rollback()
                Exit Sub
            End Try
        End If
        Call initScreen()
        Call FillDocDetails(intPartyID)

    End Sub

    Private Sub btnCloseEdit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCloseEdit.Click
        fraEdit.Visible = False
        vsfDet.Visible = True
    End Sub

    Public Sub initForm(ByVal argDocID As Integer, ByVal argPartyID As Integer)
        intPartyID = argPartyID
        intdocID = argDocID
        txtPartyID.Text = argPartyID
        Call initScreen()
    End Sub

    Private Sub txtMobNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtMobNo.KeyPress
        If e.KeyChar = "'" Then
            e.Handled = True
        End If
        'If Asc(e.KeyChar) <> 8 Then
        '    If Asc(e.KeyChar) < 48 Or Asc(e.KeyChar) > 57 Then
        '        e.Handled = True
        '    End If
        'End If
    End Sub

    Private Sub txtPhone_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtPhone.KeyPress
        If e.KeyChar = "'" Then
            e.Handled = True
        End If
        'If Asc(e.KeyChar) <> 8 Then
        '    If Asc(e.KeyChar) < 48 Or Asc(e.KeyChar) > 57 Then
        '        e.Handled = True
        '    End If
        'End If
    End Sub

    Public Function validateEmail(ByVal emailAddress) As Boolean
        validateEmail = False
        If txtEmail.Text <> "" Then
            Dim email As New Regex("([\w-+]+(?:\.[\w-+]+)*@(?:[\w-]+\.)+[a-zA-Z]{2,7})")
            If email.IsMatch(emailAddress) Then
                Return (True)
            Else
                Return (False)
            End If
        End If
    End Function

    'Private Sub txtEmail_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles txtEmail.Validating
    '    Dim email As New Regex("([\w-+]+(?:\.[\w-+]+)*@(?:[\w-]+\.)+[a-zA-Z]{2,7})")
    '    If email.IsMatch(emailAddress) Then
    '        Return (True)
    '    Else
    '        Return (False)
    '    End If
    '    'If Not validateEmail(txtEmail.Text) = False Then
    '    '    MsgBox("Please enter your email address correctly", "Incorrect Email Entry")
    '    'End If

    'End Sub

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

    Private Sub txtDept_TextChanged(sender As Object, e As EventArgs) Handles txtDept.TextChanged

    End Sub

    Private Sub txtDept_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtDept.KeyPress
        If e.KeyChar = "'" Then
            e.Handled = True
        End If

    End Sub

    Private Sub txtEmail_Validating(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles txtEmail.Validating
        If txtEmail.Text = "" Then
            Exit Sub
        End If

        Dim pattern As String = "^[a-z][a-z|0-9|]*([_][a-z|0-9]+)*([.][a-z|0-9]+([_][a-z|0-9]+)*)?@[a-z][a-z|0-9|]*\.([a-z][a-z|0-9]*(\.[a-z][a-z|0-9]*)?)$"

        Dim match As System.Text.RegularExpressions.Match = Regex.Match(txtEmail.Text.Trim(), pattern, RegexOptions.IgnoreCase)
        If (match.Success) Then
            'MessageBox.Show("Success", "Checking")
        Else
            MessageBox.Show("Please enter a valid email id", "Checking")
            txtEmail.Clear()
        End If

    End Sub

    Private Sub frmMstPartyContactPerson_KeyUp(sender As Object, e As KeyEventArgs) Handles Me.KeyUp
        If e.KeyCode = Keys.F9 And e.Control = True Then
            Call initFormSize(Me, Me.Text, True, 939, 483, 0, 0)
            Call initFormVsfDet(Me, Me.Text, vsfDet, "DET")
            MsgBox("Restored Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
        If e.KeyCode = Keys.F8 And e.Control = True Then
            Call SaveActFormSettings(Me, Me.Text)
            Call SaveActFormVsfDet(Me, Me.Text, "DET", vsfDet)
            MsgBox("Saved Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
        If e.Alt = True And e.KeyCode = Keys.X Then
            Call ExportToXL(vsfDet)
        End If
    End Sub

    Private Sub btnExport_Click(sender As Object, e As EventArgs) Handles btnExport.Click
        Call ExportToXL(vsfDet, argFileName:="Contact Persons for -" & txtPartyCode.Text)
    End Sub

    Private Sub PicBox_Click(sender As Object, e As EventArgs) Handles PicBox.Click
        Call SaveImage()
    End Sub

    Private Sub SaveImage()
        If Mode = "V" Or Mode = "" Then Exit Sub
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
                PicBox.ImageLocation = imagePath
                ' PicBox.Image = Image.FromFile(imagePath)
                txtFileName.Text = Path.GetFileName(idb.FileName)
                btnRmvImg.Visible = True
            Else
                txtFileName.Text = ""
                Exit Sub
            End If

        Catch ex As Exception
            MsgBox("Error in image selection!" & vbCrLf & ex.Message, MsgBoxStyle.Critical, Me.Text)
            Exit Sub
        End Try


    End Sub

    Private Sub SaveContactImg()
        Dim img As Byte() = Nothing
        Dim Extension As String = Path.GetExtension(PicBox.ImageLocation)

        Try
            If StrComp(Extension, ".jpg", CompareMethod.Text) = 0 Or
               StrComp(Extension, ".jpeg", CompareMethod.Text) = 0 Then


                img = GetImageByte(imagePath)


                Try
                    If System.IO.File.Exists(gstrRptPath & "CustFiles\ContactFiles\" & txtCID.Text & ".jpg") = True Then
                        System.IO.File.Delete(gstrRptPath & "CustFiles\ContactFiles\" & txtCID.Text & ".jpg")
                    End If
                Catch ex As Exception
                    MsgBox(ex.Message)
                End Try

                Me.PicBox.Image.Save(gstrRptPath & "CustFiles\ContactFiles\" & txtCID.Text & ".jpg", System.Drawing.Imaging.ImageFormat.Jpeg)
                MsgBox("Image updated successfully for the user: '" & (txtPartyCode.Text) & "'", vbInformation, Me.Text)

                Exit Sub
            Else
                'MsgBox("Please enter only jpg/jpeg images!", vbInformation, Me.Text)
                'Exit Sub
            End If
        Catch ex As Exception
            MsgBox("Error while update image!" & vbCrLf & ex.Message, vbCritical, Me.Text)
        End Try
    End Sub

    Private Sub btnRmvImg_Click(sender As Object, e As EventArgs) Handles btnRmvImg.Click
        If Mode = "V" Or Mode = "" Then Exit Sub

        'If PicBox.Image Is Nothing Then
        '    btnRmvImg.Visible = False
        '    Exit Sub
        'Else
        '    btnRmvImg.Visible = True
        'End If

        If MsgBox("Are you sure you want to remove the user's image?", vbQuestion + vbYesNo + vbDefaultButton2, Me.Text) = MsgBoxResult.Yes Then
            vsfDet.SetCellImage(vsfDet.Row, vdetImage, Nothing)
            PicBox.Image = Nothing
            btnRmvImg.Visible = False
            'If System.IO.File.Exists(gstrRptPath & "CustFiles\ContactFiles\" & txtCID.Text & ".jpg") = True Then
            '    System.IO.File.Delete(gstrRptPath & "CustFiles\ContactFiles\" & txtCID.Text & ".jpg")
            'End If
        End If


    End Sub

    Private Sub getImage()
        Dim imgStrm As New MemoryStream(GetImageByte(gstrRptPath & "CustFiles\ContactFiles\" & txtCID.Text & ".jpg"))
        PicBox.Image = Image.FromStream(imgStrm)
    End Sub

    Private Sub btnConList_Click(sender As Object, e As EventArgs) Handles btnConList.Click
        Dim strSQL As String
        Dim frmDisplay As New frmUtlMstView
        strSQL = " Select PartyID," & strFldPartyCode & "," & strFldPartyName & ",CName as ContactName, CDesig as Designation,CMobNo as MobileNo,CPhone as PhoneNo,p.Fax,p.Email,CRemarks as Remarks,Cdepartment as Department "
        strSQL = strSQL & " From MstPartyContacts p, " & strPartyTblName & " c "
        strSQL = strSQL & " Where c." & strFldPartyID & " = PartyID And MstDocID = " & intdocID & " Order By " & strFldPartyCode

        With frmDisplay
            .Text = Me.Text
            Call .FillViewVsf(Me, strSQL, "", "", intdocID, "Contact Master")
            .vsfDet.Cols(0).Visible = False
            .vsfDet.Cols(1).AllowMerging = True
            .vsfDet.Cols(2).AllowMerging = True
            .ShowDialog()
        End With


    End Sub

    Private Sub GetPartyDtls()
        Dim rsDummy As oledb.oledbDataReader
        rsDummy = clsSVT.getDataReaderFromSQL("Select * from UtlTransactions where DocID = " & intdocID)

        With rsDummy
            If .HasRows Then
                .Read()
                Dim strPartyShForm As String
                strPartyShForm = GetVal(!DocShForm, 0)
                strPartyTblName = "Mst" & strPartyShForm
                strFldPartyID = strPartyShForm & "ID"
                strFldPartyCode = strPartyShForm & "Code"
                strFldPartyName = strPartyShForm & "Name"
            End If
            .Close()
        End With
    End Sub

End Class

