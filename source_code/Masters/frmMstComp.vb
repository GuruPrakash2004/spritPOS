Imports C1.Win.C1FlexGrid
Imports C1.Win.C1Input
Imports System.IO
Imports System.Text.RegularExpressions
Imports System.IO.Compression

Public Class FrmMstComp
    Dim intDocID As Integer
    Dim strRights As String
    Dim clsSVT As New svtLibrary

    Dim strBackupMailID As String

    Const vCDetCompName = 0
    Const vCDetAdd1 = 1
    Const vCDetAdd2 = 2
    Const vCDetAdd3 = 3
    'Const vCDetTinNo = 4
    'Const vCDetCstNo = 5
    'Const vCDetSerTaxNo = 4
    'Const vCDetPhoneNo = 5
    Const vCDetEmail = 4
    'Const vCDetStore = 5
    'Const vCDetWeb = 7

    Const vCDetCompAcnNo = 0
    Const vCDetBankName = 1

    Const vsfFdate = 1
    Const vsfTdate = 2
    Const vsfActive = 3

    Const vsfDetGroup = 0
    Const vsfDetIcon = 1
    Const vsfDetFileType = 2
    Const vsfDetFileName = 3
    Const vsfDetRemarks = 4

    Public strFileName As String
    Public strFilepath As String
    Public strExtnName As String
    Public img As Image = Nothing

    Dim strMode As String
    Dim Mode As String
    Dim blnFound As Boolean
    Dim scriptExecute As New Process

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        pnlFnlYr.Visible = False
    End Sub

    Private Sub CalTotal()
        'Call AddnlMSTFldCalTotal(Me, vsfHdr, 0, "", "", txtCustID.Text)
    End Sub

    Private Sub initScreen()
        Me.KeyPreview = True

        Try
            If Not Directory.Exists(gstrRptPath & "SharedDocs") Then
                Directory.CreateDirectory(gstrRptPath & "SharedDocs")
            End If
        Catch ex As Exception

        End Try

        strBackupMailID = getGsettings("COMPANY", "BACKUPMAILID", "")
        If strBackupMailID <> "" Then
            Dim dbm As New DropDownItem
            dbm.Text = "Backup and Mail to " & strBackupMailID
            btnBackup.Items.Add(dbm)
        End If

        btnOpt.Visible = False
        Call InitAddnlFldGrids(Me, vsfHdr, "Header", intDocID)
        btnChangeImg.Enabled = False
        cboType.Items.Add("RestaurantImg")
        cboType.Items.Add("InvImg")
        cboType.Items.Add("PayImg")
        cboType.Items.Add("FAImg")

        With vsfCDET
            .Rows.Count = 5
            .Cols.Count = 2

            .SetData(vCDetCompName, 0, "Company Name")

            .SetData(vCDetAdd1, 0, "Address")
            .SetData(vCDetAdd2, 0, ".")
            .SetData(vCDetAdd3, 0, "..")
            .SetData(vCDetEmail, 0, "Email ID")
            '.SetData(vCDetStore, 0, "Store Name")
        End With

        With vsfFnlYr
            .Cols.Count = 4
            .Rows.Count = 2

            .SetData(0, vsfFdate, "From date")
            .SetData(0, vsfTdate, "To date")
            .SetData(0, vsfActive, "Active")

            .Cols(vsfTdate).DataType = GetType(Date)
            .Cols(vsfFdate).DataType = GetType(Date)
            .Cols(vsfActive).DataType = GetType(Char)
            .Cols(vsfActive).AllowEditing = False
            Call SetFlexColor(vsfFnlYr, "", vsfFdate, vsfTdate)
        End With

        With vsfDet
            .Cols.Count = 5
            .Rows.Count = 2

            .SetData(0, vsfDetGroup, "Group")
            .SetData(0, vsfDetIcon, "Icon")
            .SetData(0, vsfDetFileType, "File Type")
            .SetData(0, vsfDetFileName, "File Name")
            .SetData(0, vsfDetRemarks, "Remarks")
            .AllowEditing = False
            .Cols(vsfDetIcon).Visible = False
            .Cols(vsfDetIcon).DataType = GetType(PictureBox)
            .ExtendLastCol = True
        End With
        Call fillGrid()
        Call SetColWidth(vsfCDET, 150, 200)
        Call SetColWidth(vsfFnlYr, -1, 150, 150, 20)
        Call SetFlexColor(vsfCDET, "M", vCDetCompName)
        C1DockingTab1.SelectedIndex = 0
        vsfCDET.ExtendLastCol = True
        strMode = ""
        pnlFnlYr.Visible = False
        Mode = "M"
        Call initGridStyles(Me)
        GetImage()
        btnChangeImg.Enabled = True
        vsfHdr.Styles.EmptyArea.BackColor = Color.Transparent
        vsfHdr.BackColor = Color.Transparent

        vsfDet.AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.RestrictAll
        Dim j%
        For j = vsfDet.Cols.Fixed To vsfDet.Cols.Count - 1
            vsfDet.Cols(vsfDetGroup).AllowMerging = True
        Next

        Call SetColWidth(vsfDet, 80, 50, 150, 200, 250)
        fillVsfDet()

        If vsfDet.Rows.Count > 1 Then
            vsfDet.Row = 1
            'Call vsfDet_RowColChange(sender, e)
        End If
        'Mode = "V"
        'fillFormProperties(Me)
        'vsfHdr.Styles.EmptyArea.BackColor = Color.Transparent
        'vsfHdr.BackColor = Color.Transparent
        'vsfCDET.Styles.EmptyArea.BackColor = Color.Transparent
        'vsfCDET.BackColor = Color.Transparent
    End Sub


    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Call frmMain.CloseSelectedTabPage(Me)
    End Sub

    Private Sub FrmMstComp_KeyUp(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyUp
        If e.KeyCode = Keys.F5 Then
            Call fillGrid()
        End If

        If e.KeyCode = Keys.F9 And e.Control = True Then
            Call initFormSize(Me, "MstComp", True, 913, 481, 0, 0)
            MsgBox("Restored Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
        If e.KeyCode = Keys.F8 And e.Control = True Then
            Call SaveActFormSettings(Me, "MstComp")
            Call SaveActFormVsfDet(Me, Me.Text, "DET", vsfCDET)
            Call SaveActFormVsfDet(Me, Me.Text, "FnlYr", vsfFnlYr)
            MsgBox("Saved Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
    End Sub

    Private Sub FrmMstComp_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        lblFrm.Text = "Company Master"

        intDocID = 17
        strRights = getUserRights(GetMenuId(Me.Text))

        'If gStrGlobeDBName <> "" Then
        '    btnCompanies.Visible = False
        'Else
        '    btnCompanies.Visible = True
        'End If

        Call initScreen()
        initGridStyles(Me)

        Call initFormSize(Me, "MstComp", False, 913, 481, 0, 0)
        Call initFormVsfDet(Me, Me.Text, vsfCDET, "DET")
        Call initFormVsfDet(Me, Me.Text, vsfFnlYr, "FnlYr")
        ColorChange()
        Call ChangeFontSize(Me)
        lblFrm.Font = New Font("Microsoft Sans Serif", 14, FontStyle.Bold)
        ToolTip1.SetToolTip(btnBackup, "Backup of current database " & gStrDBName)

        Dim strDashUpdte As String = clsSVT.getDataStringValue("select DashBoardReq from UtlMisc", "", False) 'CNUD
        If strDashUpdte = "Y" Then
            llblDashBoard.Visible = True
        End If

    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim strSQL As String
        Dim strValues As String = ""
        Dim strFlds As String = ""

        If Not chkUserRights(strRights, chkRightsPos.ViewRights, Me.Text) Then Exit Sub

        StMsg("Validating Data.....")
        Dim intGridRow As Int32 = 0
        Dim intGridCol As Int32 = 0
        stringReplace(Me)
        With vsfCDET
            intGridCol = 1
            For intGridRow = 0 To .Rows.Count - 1
                .SetData(intGridRow, intGridCol, CStr(GetVal(.GetData(intGridRow, intGridCol), "")).Replace("'", "`"))
            Next
        End With

        With vsfHdr
            intGridCol = 4
            For intGridRow = 0 To .Rows.Count - 1
                .SetData(intGridRow, intGridCol, CStr(GetVal(.GetData(intGridRow, intGridCol), "")).Replace("'", "`"))
            Next
        End With

        With vsfFnlYr
            For intGridRow = 1 To .Rows.Count - 1
                For intGridCol = 0 To .Cols.Count - 1
                    .SetData(intGridRow, intGridCol, CStr(GetVal(.GetData(intGridRow, intGridCol), "")).Replace("'", "`"))
                Next
            Next
        End With
        If Not IsNothing(vsfHdr.Editor) Then
            If vsfHdr.Editor.Text <> "" Then
                vsfHdr.SetData(vsfHdr.Row, vOthFldDataValue, vsfHdr.Editor.Text)
            End If
        End If
        With vsfCDET
            If vsfCDET.GetData(vCDetCompName, 1) = "" Then
                MsgBox("Company Name Should not be blank!!!", vbInformation, Me.Text)
                vsfCDET.Focus()
                .Col = vCDetCompName
                .Row = 0
                Exit Sub
            End If
        End With
        If validateEmail(vsfCDET.GetData(vCDetEmail, 1)) = True Or vsfCDET.GetData(vCDetEmail, 1) = "" Then
        Else
            MsgBox("EmailID is not valid!!", vbInformation, Me.Text)
            vsfCDET.Focus()
            vsfCDET.Col = 1
            vsfCDET.Row = vCDetEmail
            Exit Sub
        End If


        With vsfCDET
            strSQL = "update MstComp set "
            strSQL = strSQL & " CompName = '" & .GetData(vCDetCompName, 1) & "',"
            strSQL = strSQL & " Address1 = '" & .GetData(vCDetAdd1, 1) & "',"
            strSQL = strSQL & " Address2 = '" & .GetData(vCDetAdd2, 1) & "',"
            strSQL = strSQL & " Address3 = '" & .GetData(vCDetAdd3, 1) & "',"
            'strSQL = strSQL & " StoreName = '" & .GetData(vCDetStore, 1) & "',"
            'strSQL = strSQL & " TINNO = '" & .GetData(vCDetTinNo, 1) & "',"
            'strSQL = strSQL & " CSTNO = '" & .GetData(vCDetCstNo, 1) & "',"
            'strSQL = strSQL & " SERTAXNO = '" & .GetData(vCDetSerTaxNo, 1) & "',"
            'strSQL = strSQL & " PHNO = '" & .GetData(vCDetPhoneNo, 1) & "',"
            strSQL = strSQL & " EMAIL = '" & .GetData(vCDetEmail, 1) & "'"
            Call GetUpdateString(Me, strFlds)
            strSQL = strSQL & strFlds
        End With

        If clsSVT.execSQLStatement(strSQL) Then
            MsgBox("Updated Successfully.", vbInformation, Me.Text)
        End If

    End Sub

    Private Sub fillGrid()
        Dim rsTemp As oledb.oledbDataReader
        Dim strSql As String

        strSql = "Select * from MstComp"

        rsTemp = clsSVT.getDataReaderFromSQL(strSql)
        With rsTemp
            Do While .Read()
                With vsfCDET
                    .SetData(vCDetCompName, 1, GetVal(rsTemp("CompName"), ""))
                    .SetData(vCDetAdd1, 1, GetVal(rsTemp("Address1"), ""))
                    .SetData(vCDetAdd2, 1, GetVal(rsTemp("Address2"), ""))
                    .SetData(vCDetAdd3, 1, GetVal(rsTemp("Address3"), ""))
                    '.SetData(vCDetTinNo, 1, GetVal(rsTemp("TINNO"), ""))
                    '.SetData(vCDetCstNo, 1, GetVal(rsTemp("CSTNO"), ""))
                    '.SetData(vCDetSerTaxNo, 1, GetVal(rsTemp("SERTAXNO"), ""))
                    '.SetData(vCDetPhoneNo, 1, GetVal(rsTemp("PHNO"), ""))
                    '.SetData(vCDetStore, 1, GetVal(rsTemp("StoreName"), ""))
                    .SetData(vCDetEmail, 1, GetVal(rsTemp("Email"), ""))
                    '.SetData(vCDetWeb, 1, GetVal(rsTemp("WEB"), ""))
                End With
                For i = 0 To vsfHdr.Rows.Count - 1
                    For X = 0 To .FieldCount - 1
                        If UCase(.GetName(X)) = UCase(Trim(vsfHdr.GetData(i, vOthFldCode))) Then
                            If Trim(vsfHdr.GetData(i, vOthFldCmbTblName)) = "" Then
                                vsfHdr.SetData(i, vOthFldDataValue, GetVal(.GetValue(X), ""))
                            Else
                                strSql = "Select " & vsfHdr.GetData(i, vOthFldCmbDisplayCols) & " from " &
                                    vsfHdr.GetData(i, vOthFldCmbTblName) & " where " &
                                    vsfHdr.GetData(i, vOthFldCmbStorageFldName) & " = " & GetVal(.GetValue(X), 0)
                                vsfHdr.SetData(i, vOthFldDataValue,
                                    clsSVT.getDataStringValue(strSql, "", False))
                            End If
                        End If
                    Next X
                Next
            Loop
            .Close()
        End With

        strSql = "Select * from MstCompFinYear Order by yrFdate"
        rsTemp = clsSVT.getDataReaderFromSQL(strSql)

        'vsfIDET.Rows.Count = 1
        'With rsTemp
        '    Do While .Read()
        '        vsfIDET.Rows.Count = vsfIDET.Rows.Count + 1
        '        vsfIDET.SetData(vsfIDET.Rows.Count - 1, VDetTemplateName, GetVal(rsTemp("TemplateName"), ""))'
        vsfFnlYr.Rows.Count = 1
        With rsTemp
            Do While .Read()
                With vsfFnlYr
                    .Rows.Count = .Rows.Count + 1
                    .SetData(.Rows.Count - 1, vsfFdate, GetVal(rsTemp("yrFdate"), ""))
                    .SetData(.Rows.Count - 1, vsfTdate, GetVal(rsTemp("yrTdate"), ""))
                    .SetData(.Rows.Count - 1, vsfActive, GetVal(rsTemp("Active"), ""))

                End With
            Loop
        End With
    End Sub

    Private Sub btnCreateFinYr_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCreateFinYr.Click
        pnlFnlYr.Visible = True

        Dim dtFinFYear As Date
        Dim dtFinTYear As Date

        If vsfFnlYr.Rows.Count >= 2 Then
            dtFinFYear = CDate(vsfFnlYr.GetData(vsfFnlYr.Rows.Count - 1, vsfFdate))
            dtFinTYear = CDate(vsfFnlYr.GetData(vsfFnlYr.Rows.Count - 1, vsfTdate))

            dtpFyr.Value = DateAdd("yyyy", 1, dtFinFYear)
            dtpTyr.Value = DateAdd("yyyy", 1, dtFinTYear)

            Exit Sub
        End If

        dtFinFYear = "01/04/" & getSystemDate.Year
    End Sub

    'Private Sub vsfFnlYr_MouseDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles vsfFnlYr.MouseDoubleClick
    '    Dim strSql As String

    '    With vsfFnlYr
    '        If .Row < 1 Then Exit Sub
    '        If .GetData(.Row, vsfActive) = "Y" Then
    '            Exit Sub
    '        Else
    '            If MsgBox("Do you want to set this as your financial year ?", vbInformation + vbYesNo, Me.Text) = vbYes Then
    '                strSql = "Update MstCompFinYear set Active = 'N'"
    '                clsSVT.execSQLStatement(strSql)

    '                strSql = "Update MstCompFinYear set Active = 'Y' where yrFdate = '" & Format(.GetData(.Row, vsfFdate), "dd MMM yyyy") & "'"
    '                clsSVT.execSQLStatement(strSql)
    '                Call fillGrid()

    '            End If
    '        End If
    '    End With
    'End Sub

    Private Sub btnCreate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCreate.Click
        Dim strSQL As String, blnYearFound As Boolean

        strSQL = "select * from MstCompFinYear where " & getDateFormatInCond(dtpFyr.Value) & " between yrfdate and yrtdate"

        Call clsSVT.getDataStringValue(strSQL, "", blnYearFound)

        If blnYearFound Then
            MsgBox("Financial Year already exists!", vbInformation, Me.Text)
            Exit Sub
        Else
            strSQL = "Insert into MstCompFinYear "
            strSQL = strSQL & "(yrFdate,yrTdate,Active) values ("
            strSQL = strSQL & "'" & Format(dtpFyr.Value, "dd MMM yyyy") & "',"
            strSQL = strSQL & "'" & Format(dtpTyr.Value, "dd MMM yyyy") & "',"

            strSQL = strSQL & "'N'" & ")"
            clsSVT.execSQLStatement(strSQL)
            Call fillGrid()
        End If

        pnlFnlYr.Visible = False
    End Sub

    Private Sub btnRemoveFinYr_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRemoveFinYr.Click

        'If vsfFnlYr.Row <> 1 Then
        '    MsgBox(vsfFnlYr.Row)
        '    Exit Sub
        'End If
        If vsfFnlYr.Rows.Count = 2 Then
            MsgBox("Atleast 1 active financial year must exist!", vbCritical + vbOKOnly, Me.Text)
            Exit Sub
        End If


        If vsfFnlYr.Row = 1 Or vsfFnlYr.Row = vsfFnlYr.Rows.Count - 1 Then
            If vsfFnlYr.GetData(vsfFnlYr.Row, vsfActive) = "Y" Then
                MsgBox("Unable to remove as this financial year is currently in use!", vbInformation, Me.Text)
                Exit Sub
            End If
            If MsgBox("Do you want to delete this financial year -- " & vsfFnlYr.GetData(vsfFnlYr.Row, vsfFdate) & " to " & vsfFnlYr.GetData(vsfFnlYr.Row, vsfTdate) & " ?", vbYesNo + vbCritical + vbDefaultButton2, Me.Text) = MsgBoxResult.No Then Exit Sub
            If clsSVT.execSQLStatement("Delete from MstCompFinYear where yrFdate = " & getDateFormatInCond(vsfFnlYr.GetData(vsfFnlYr.Row, vsfFdate)) & "and yrTdate = " & getDateFormatInCond(vsfFnlYr.GetData(vsfFnlYr.Row, vsfTdate)) & "") Then
                MsgBox("Financial Year -- " & Format(vsfFnlYr.GetData(vsfFnlYr.Row, vsfFdate), "yyyy") & " deleted successfully.", vbInformation, Me.Text)
                Call fillGrid()
            Else
                MsgBox("Deletion failed. Invalid year...", vbCritical, Me.Text)
            End If
        Else
            MsgBox("Only first and last row can be deleted!", vbCritical, Me.Text)
        End If


    End Sub

    Private Sub vsfHdr_AfterEdit(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfHdr.AfterEdit
        Call vsfGenAfterEditBfrCalTot(Me, vsfHdr, e.Row, "M")
        Call CalTotal()
        Call vsfGenAfterEditAftrCalTot(Me, vsfHdr, intDocID, Mode, e.Row, False)
        With vsfHdr
            If .Row = .Rows.Count - 1 Then
                btnBackup.Focus()
            End If
        End With

        'Call AutosizeMstfrmGrid(frmMstParty)
    End Sub

    Private Sub vsfHdr_BeforeEdit(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfHdr.BeforeEdit
        If vsfBeforeEditCancel(Me, vsfHdr, intDocID, "MstComp", Mode, e.Row, e.Col, "M", "") Then
            e.Cancel = True
            Exit Sub
        End If
    End Sub

    Private Sub vsfHdr_EnterCell(ByVal sender As Object, ByVal e As System.EventArgs) Handles vsfHdr.EnterCell
        'If Mode = "V" Then Exit Sub
        'With vsfHdr
        '    If .GetData(.Row, vOthFldTxtBlnMultiLine) = "Y" Then Exit Sub

        '    If .GetData(.Row, vOthFldBlnComboData) = "Y" Then Exit Sub
        '    If .GetData(.Row, vOthFldDataType) = "List String" Then Exit Sub
        '    If .GetData(.Row, vOthFldBlnReadOnly) = "Y" Then Exit Sub
        '    If .GetData(.Row, vOthFldBlnRefill) = "Y" Then Exit Sub

        '    .StartEditing()
        'End With
    End Sub

    Private Sub vsfHdr_KeyPressEdit(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.KeyPressEditEventArgs) Handles vsfHdr.KeyPressEdit
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

    Private Sub vsfHdr_KeyUp(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles vsfHdr.KeyUp
        With vsfHdr
            If .Row = .Rows.Count - 1 Then
                btnBackup.Focus()
            End If
        End With
    End Sub

    Private Sub vsfHdr_RowColChange(ByVal sender As Object, ByVal e As System.EventArgs) Handles vsfHdr.RowColChange
        If Not (Mode = "A" Or Mode = "M") Then Exit Sub
        'Call vsfRowColChange(Me, vsfHdr)
    End Sub

    Private Sub vsfHdr_SetupEditor(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfHdr.SetupEditor
        Call vsfSetupEditor(Me, vsfHdr, intDocID, "MstComp", Mode, e.Row, e.Col, "M")
    End Sub

    Private Sub vsfCDET_KeyPressEdit(sender As Object, e As C1.Win.C1FlexGrid.KeyPressEditEventArgs) Handles vsfCDET.KeyPressEdit
        If e.KeyChar = "'" Then
            e.Handled = True
        End If
    End Sub

    Private Sub vsfCDET_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles vsfCDET.LostFocus
        With vsfCDET
            If .GetData(vCDetCompName, 1) = "" Then
                MsgBox("Company name should not be blank!", vbInformation, Me.Text)
                .Focus()
                .Col = 1
                .Row = 0
                Exit Sub
            End If
        End With

    End Sub

    Private Sub btnChangeImg_Click(sender As System.Object, e As System.EventArgs) Handles btnChangeImg.Click
        saveImage()
        GetImage()

        If Not PicBox.Image Is Nothing Then
            Call frmMain.loadMainPageImage()
        End If
    End Sub

    Private Sub saveImage()
        Dim imagePath As String
        Dim strSql As String
        Dim img As Byte() = Nothing
        Dim idb As New OpenFileDialog
        If Not chkUserRights(strRights, chkRightsPos.AddRights, Me.Text) Then Exit Sub
        idb.Title = "Select your image."
        idb.Filter = "Image Files|*.jpg;"
        idb.RestoreDirectory = False

        If idb.ShowDialog() = DialogResult.OK Then
            imagePath = idb.FileName
            img = GetImageByte(imagePath)
            If img.Length > 5000000 Then
                MsgBox("Image size cannot be greater than 5MB.", vbInformation, Me.Text)
                Exit Sub
            End If
        Else
            Exit Sub
        End If

        Dim Extension As String = Path.GetExtension(imagePath)
        Try
            If StrComp(Extension, ".jpg", CompareMethod.Text) = 0 Or
               StrComp(Extension, ".jpeg", CompareMethod.Text) = 0 Then

                Dim cmd As oledb.oledbCommand 

                strSql = "Update MstComp set  jpgFile = ?"
                cmd = New oledb.oledbCommand (strSql, svtConn)
                cmd.Parameters.AddWithValue("@Image", img)


                If cmd.ExecuteNonQuery() Then
                    MsgBox("Image updated successfully!", vbInformation, Me.Text)
                End If
                Exit Sub
            Else
                MsgBox("Please enter only jpg/jpeg images!", vbInformation, Me.Text)
                Exit Sub
            End If
        Catch
            MsgBox("Error while update images!", vbCritical, Me.Text)
        End Try
    End Sub

    Public Shared Function GetImageByte(ByVal imagePath As String) As Byte()
        Dim stream As FileStream = New FileStream(imagePath, FileMode.Open, FileAccess.Read)
        Dim reader As BinaryReader = New BinaryReader(stream)
        Dim img() As Byte = reader.ReadBytes(stream.Length)

        reader.Close()
        stream.Close()

        Return img
    End Function


    Private Function UnicodeStringToBytes(ByVal str As String) As Byte()
        Return System.Text.Encoding.Unicode.GetBytes(str)
    End Function

    Private Function UnicodeBytesToString(
    ByVal bytes() As Byte) As String
        Return System.Text.Encoding.Unicode.GetString(bytes)
    End Function

    'Private Function BytesToUnicodeString(ByVal argbytes As Byte()) As String
    '    BytesToUnicodeString = ""
    '    'Return System.Text.Encoding.Unicode.GetString(argbytes)
    '    For t = 1 To argbytes.Length - 1
    '        BytesToUnicodeString += argbytes(t)
    '    Next
    '    BytesToUnicodeString += argbytes(t)
    'End Function

    Private Sub GetImage()
        Dim strSql As String = ""
        Dim cmd As oledb.oledbCommand = Nothing
        PicBox.Image = Nothing
        strSql = "Select " & getImgSyntax("ISNULL", "JpgFile") & " from MstComp"
        Try
            cmd = New oledb.oledbCommand (strSql, svtConn)
            Dim imageData As Byte() = DirectCast(cmd.ExecuteScalar(), Byte())
            If imageData.Length = 0 Then Exit Sub
            If Not imageData Is Nothing Then
                If imageData.Length > 0 Then
                    Dim ImageMemoryStream As MemoryStream = New IO.MemoryStream(GetImageBytesFromOLEField(imageData))
                    PicBox.Image = Image.FromStream(ImageMemoryStream, True)
                    PicBox.SizeMode = PictureBoxSizeMode.StretchImage
                Else
                    PicBox.Image = Nothing
                End If
            End If
        Catch
        End Try
    End Sub

    Friend Shared Function GetImageBytesFromOLEField(ByVal oleFieldBytes() As Byte) As Byte()

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

    'Private Sub btnLoad_Click(sender As System.Object, e As System.EventArgs) Handles btnLoad.Click
    '    Select Case cboType.Text
    '        Case "RestaurantImg", "InvImg", "PayImg", "FAImg"
    '        Case Else
    '            MsgBox("Please select a valid Main form Type!", MsgBoxStyle.Critical, Me.Text)
    '            Exit Sub
    '    End Select
    '    GetImage()
    '    btnChangeImg.Enabled = True
    'End Sub

    Private Sub btnSelFinYear_Click(sender As System.Object, e As System.EventArgs) Handles btnSelFinYear.Click
        With FrmSelFinYear
            .btnSelFinyear.Visible = False
            .lblDblClick.Visible = False
            .ShowDialog()

            fillGrid()
            fillVsfDet()
        End With
    End Sub

    'Private Sub TabPageImage_CaptionTextChanged(sender As Object, e As System.EventArgs) Handles TabPageImage.CaptionTextChanged
    '    GetImage()
    '    btnChangeImg.Enabled = True
    'End Sub

    'Private Sub TabPageImage_Click(sender As Object, e As System.EventArgs) Handles TabPageImage.Click
    '    GetImage()
    '    btnChangeImg.Enabled = True
    'End Sub

    'Private Sub TabPageImage_TabIndexChanged(sender As Object, e As System.EventArgs) Handles TabPageImage.TabIndexChanged
    '    GetImage()
    '    btnChangeImg.Enabled = True
    'End Sub

    Private Sub btnAlias_Click(sender As System.Object, e As System.EventArgs) Handles btnAlias.Click
        Dim frmAlias As New frmUtlAlias

        With frmAlias
            .MdiParent = frmMain
            .Show()
        End With
    End Sub

    Private Sub btnOpt_GotFocus(sender As Object, e As System.EventArgs) Handles btnOpt.GotFocus
        'With vsfCDET
        '    .Row = 0
        '    .Col = 1
        'End With

        'With vsfHdr
        '    .Row = 0
        '    .Col = 1
        'End With

    End Sub

    Private Sub btnOpt_KeyUp(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles btnOpt.KeyUp
        If e.KeyCode = Keys.Space Then
            If Not btnOpt.DroppedDown Then
                btnOpt.DroppedDown = True
            Else
                btnOpt.DroppedDown = False
            End If
        End If
    End Sub

    Private Sub vsfFnlYr_KeyPressEdit(sender As Object, e As C1.Win.C1FlexGrid.KeyPressEditEventArgs) Handles vsfFnlYr.KeyPressEdit
        If e.KeyChar = "'" Then
            e.Handled = True
        End If
    End Sub
    Public Function validateEmail(emailAddress) As Boolean
        Dim email As New Regex("([\w-+]+(?:\.[\w-+]+)*@(?:[\w-]+\.)+[a-zA-Z]{2,7})")
        If email.IsMatch(emailAddress) Then
            Return True
        Else
            Return False
        End If
    End Function

    Private Sub vsfCDET_LeaveCell(sender As Object, e As System.EventArgs) Handles vsfCDET.LeaveCell
        'With vsfCDET
        '    Dim strField As String = GetVal(.GetData(.Row, .Col), "")
        '    If strField.Length > 100 Then
        '        MsgBox("Allowed charaters are 100 only... Please reduce the lenght of the string", MsgBoxStyle.Information, Me.Text)
        '        .SetData(.Row, .Col, strField.Substring(1, 99))
        '        Exit Sub
        '    End If
        'End With
    End Sub

    Private Sub vsfCDET_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles vsfCDET.KeyDown
        With vsfCDET
            If .Row = .Rows.Count - 1 Then
                If pnlDashBrd.Visible = True Then
                    txtDashBrdUrl.Focus()
                Else
                    btnBackup.Focus()
                End If
            End If
        End With
    End Sub

    Private Sub vsfCDET_SetupEditor(sender As Object, e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfCDET.SetupEditor
        With vsfCDET
            Select Case e.Row
                Case vCDetAdd1, vCDetAdd2, vCDetAdd3, vCDetEmail
                    If TypeOf (vsfCDET.Editor) Is TextBox Then
                        Dim tb As TextBox = vsfCDET.Editor
                        tb.MaxLength = 100
                    End If
                Case vCDetCompName
                    If TypeOf (vsfCDET.Editor) Is TextBox Then
                        Dim tb As TextBox = vsfCDET.Editor
                        tb.MaxLength = 250
                    End If
            End Select
        End With
        Exit Sub
    End Sub

    'Private Sub frmMstComp_Resize(sender As Object, e As System.EventArgs) Handles Me.Resize
    '    If vsfCDET.Rows.Count <= 0 Then Exit Sub
    '    Try
    '        Dim sngActGrdHt As Single = vsfCDET.Rows(0).HeightDisplay * (vsfCDET.Rows.Count) + 2
    '        If sngActGrdHt < Me.Height And sngActGrdHt > 0 Then
    '            vsfCDET.Height = sngActGrdHt
    '        End If
    '    Catch ex As Exception

    '    End Try
    'End Sub
    Private Sub ColorChange()
        btnSave.ForeColor = Color.Black
        btnChangeImg.ForeColor = Color.Black
        btnCancel.ForeColor = Color.Black
        btnOpt.ForeColor = Color.Black
        btnCreate.ForeColor = Color.Black
        btnClose.ForeColor = Color.Black
        btnSelFinYear.ForeColor = Color.Black
        btnCreateFinYr.ForeColor = Color.Black
        btnRemoveFinYr.ForeColor = Color.Black
        btnBackup.ForeColor = Color.Black
        btnSelectCompany.ForeColor = Color.Black
        btnCompanies.ForeColor = Color.Black
    End Sub

    Private Sub btnCompanies_Click(sender As Object, e As EventArgs) Handles btnCompanies.Click
        frmMstGlobeComp.ShowDialog()
        frmMstGlobeComp.StartPosition = FormStartPosition.CenterParent

    End Sub

    Private Sub vsfHdr_AfterResizeColumn(sender As Object, e As RowColEventArgs) Handles vsfHdr.AfterResizeColumn
        If vsfHdr.Cols(vOthFldCaption).Width >= vsfHdr.Width Then
            vsfHdr.Cols(vOthFldCaption).Width = vsfHdr.Width - 20
        End If
    End Sub

    Private Sub FrmMstComp_Resize(sender As Object, e As EventArgs) Handles Me.Resize
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

    Private Function backupDB() As String
        Dim strBackupPath As String, strFile As String, strSQL As String, strNWBackupPath As String, strFilePfx As String, strNWFile As String
        backupDB = ""
        strFilePfx = "\db" & gStrDBName & getSystemDateTime.ToString("ddMMMyyyy_hhmmss") & ".Bak"

        strBackupPath = getGsettings("Database", "SQLBACKUPPATH")
        If Trim(strBackupPath) = "" Then
            MsgBox("Backup path of SQL is not specified in Global Settings. (Under Database Group)", MsgBoxStyle.Critical, "Path not specified")
            Exit Function
        End If
        strFile = strBackupPath & strFilePfx

        strNWBackupPath = getGsettings("Database", "SQLNWBACKUPPATH")
        If Trim(strNWBackupPath) = "" Then
            MsgBox("Network Backup path of SQL is not specified in Global Settings. (Under Database Group)", MsgBoxStyle.Critical, "Path not specified")
            Exit Function
        End If
        strNWFile = strNWBackupPath & strFilePfx
        Try
            showWaitCursor()
            strSQL = "BACKUP DATABASE " & gStrDBName & " TO DISK = '" & strFile & "'  WITH FORMAT;"
            If clsSVT.execSQLStatement(strSQL) Then
                'MsgBox(".....")
            End If
            backupDB = strNWFile
            showNormalCursor()
        Catch ex As Exception
            showNormalCursor()
            MsgBox("Error during taking backup" & vbCrLf & Err.Description, MsgBoxStyle.Critical, "Backup error")
        End Try
    End Function

    Private Sub btnBackup_DropDownItemClicked(sender As Object, e As DropDownItemClickedEventArgs) Handles btnBackup.DropDownItemClicked
        Dim strFile As String, strBody As String, strCompressedFile As String
        Dim strDirSource As String, strDirTgt As String
        strDirSource = Application.StartupPath & "\cSourceFiles"
        strDirTgt = Application.StartupPath & "\cTgtFiles"
        Select Case e.ClickedItem.Text
            Case "Truncate Log"
                Call TruncateDBLog()
            Case "Backup and Mail to " & strBackupMailID
                strFile = backupDB()
                If File.Exists(strFile) Then
                    Try
                        If Directory.Exists(strDirSource) Then Directory.Delete(strDirSource, True)
                        If Directory.Exists(strDirTgt) Then Directory.Delete(strDirTgt, True)
                        Directory.CreateDirectory(strDirSource)
                        Directory.CreateDirectory(strDirTgt)
                        strCompressedFile = strDirTgt & "\backup.zip"
                        File.Copy(strFile, strDirSource & "\" & Path.GetFileName(strFile))
                        Call ZipFile.CreateFromDirectory(strDirSource, strCompressedFile)
                    Catch ex As Exception
                        MsgBox("Unable to Copy and ZIp File" & vbCrLf & ex.Message, MsgBoxStyle.Critical, "Error during Copying file")
                        Exit Sub
                    End Try

                    strBody = "Backup of Database " & gStrDBName & vbCrLf
                    strBody = strBody & " Company :" & gSettings.gCompName & vbCrLf
                    strBody = strBody & " Company :" & gSettings.gCompAdd1 & vbCrLf & gSettings.gCompAdd2 & vbCrLf & gSettings.gCompAdd3
                    strBody = strBody & vbCrLf & vbCrLf
                    strBody = strBody & " Autoemail from software"

                    Call sendMailToSoftvent(strCompressedFile, "Backup of  Database " & Path.GetFileName(strCompressedFile), strBody, strBackupMailID)
                End If

            Case "Backup Database"
                strFile = backupDB()
                If strFile <> "" Then
                    MsgBox("Backup of current database " & gStrDBName & vbCrLf & "Successfully taken in File " & strFile, MsgBoxStyle.Information, "Backup Successful")
                End If
            Case "Backup DB and Mail to Softvent"
                strFile = backupDB()
                If File.Exists(strFile) Then
                    Try
                        If Directory.Exists(strDirSource) Then Directory.Delete(strDirSource, True)
                        If Directory.Exists(strDirTgt) Then Directory.Delete(strDirTgt, True)
                        Directory.CreateDirectory(strDirSource)
                        Directory.CreateDirectory(strDirTgt)
                        strCompressedFile = strDirTgt & "\backup.zip"
                        File.Copy(strFile, strDirSource & "\" & Path.GetFileName(strFile))
                        Call ZipFile.CreateFromDirectory(strDirSource, strCompressedFile)
                    Catch ex As Exception
                        MsgBox("Unable to Copy and ZIp File" & vbCrLf & ex.Message, MsgBoxStyle.Critical, "Error during Copying file")
                        Exit Sub
                    End Try

                    strBody = "Backup of Database " & gStrDBName & vbCrLf
                    strBody = strBody & " Company :" & gSettings.gCompName & vbCrLf
                    strBody = strBody & " Company :" & gSettings.gCompAdd1 & vbCrLf & gSettings.gCompAdd2 & vbCrLf & gSettings.gCompAdd3
                    strBody = strBody & vbCrLf & vbCrLf
                    strBody = strBody & " Autoemail from software"

                    Call sendMailToSoftvent(strCompressedFile, "Backup of  Database " & Path.GetFileName(strFile), strBody, "")
                End If
            Case "Backup RPTS"
                Try
                    If Directory.Exists(strDirSource) Then Directory.Delete(strDirSource, True)
                    If Directory.Exists(strDirTgt) Then Directory.Delete(strDirTgt, True)
                    Directory.CreateDirectory(strDirSource)
                    Directory.CreateDirectory(strDirTgt)
                    strCompressedFile = strDirTgt & "\backupRPTS.zip"
                    Dim fls() As String = Directory.GetFiles(gstrRptPath, "*.rpt")
                    For Each fl As String In fls
                        File.Copy(fl, strDirSource & "\" & Path.GetFileName(fl))
                    Next

                    Call ZipFile.CreateFromDirectory(strDirSource, strCompressedFile)
                    MsgBox("Backup of Rpts" & vbCrLf & "Successfully taken in File " & strCompressedFile, MsgBoxStyle.Information, "RPTS Backup Successful")

                Catch ex As Exception
                    MsgBox("Unable to Copy and ZIp File" & vbCrLf & ex.Message, MsgBoxStyle.Critical, "Error during Copying RPTS")
                    Exit Sub
                End Try
            Case "Backup RPTS and Mail to Softvent"
                Try
                    If Directory.Exists(strDirSource) Then Directory.Delete(strDirSource, True)
                    If Directory.Exists(strDirTgt) Then Directory.Delete(strDirTgt, True)
                    Directory.CreateDirectory(strDirSource)
                    Directory.CreateDirectory(strDirTgt)
                    strCompressedFile = strDirTgt & "\backupRPTS.zip"
                    Dim fls() As String = Directory.GetFiles(gstrRptPath, "*.rpt")
                    For Each fl As String In fls
                        File.Copy(fl, strDirSource & "\" & Path.GetFileName(fl))
                    Next

                    Call ZipFile.CreateFromDirectory(strDirSource, strCompressedFile)
                    'MsgBox("Backup of Rpts" & vbCrLf & "Successfully taken in File " & strCompressedFile, MsgBoxStyle.Information, "RPTS Backup Successful")
                    strBody = "Backup of RPTS of " & gStrDBName & vbCrLf
                    strBody = strBody & " Company :" & gSettings.gCompName & vbCrLf
                    strBody = strBody & " Company :" & gSettings.gCompAdd1 & vbCrLf & gSettings.gCompAdd2 & vbCrLf & gSettings.gCompAdd3
                    strBody = strBody & vbCrLf & vbCrLf
                    strBody = strBody & " Autoemail from software"

                    Call sendMailToSoftvent(strCompressedFile, "Backup of  Rpts " & gStrDBName, strBody, "")

                Catch ex As Exception
                    MsgBox("Unable to Copy and Zip File" & vbCrLf & ex.Message, MsgBoxStyle.Critical, "Error during Copying RPTS")
                    Exit Sub
                End Try
        End Select
    End Sub

    Private Sub sendMailToSoftvent(argFile As String, argSubject As String, argBody As String, argEMAIL As String)
        Dim Smtp_Server As New Net.Mail.SmtpClient
        Dim e_mail As New Net.Mail.MailMessage()
        Try
            Smtp_Server.UseDefaultCredentials = False
            Smtp_Server.Credentials = New Net.NetworkCredential(gSmtpMailID, gSmtpMailPwd)
            Smtp_Server.Port = gSmtpServerPort
            Smtp_Server.EnableSsl = True 'IIf(gSmtpEnableSSL = "Y", True, False)
            Smtp_Server.Host = gSmtpHostName
            e_mail.From = New Net.Mail.MailAddress(gSmtpMailID)
            If argEMAIL = "" Then
                e_mail.To.Add("softvent@gmail.com")
            Else
                e_mail.To.Add(argEMAIL)
            End If
            e_mail.Subject = argSubject
            e_mail.IsBodyHtml = False
            e_mail.Body = argBody
            If argFile <> "" Then
                Dim attachment As Net.Mail.Attachment
                attachment = New System.Net.Mail.Attachment(argFile)
                e_mail.Attachments.Add(attachment)
            End If
            Smtp_Server.Send(e_mail)
            MsgBox("Email Sent.", MsgBoxStyle.Information, Me.Text)

        Catch ex As Exception
            MsgBox("EMail NOT SENT" & vbCrLf & ex.Message, MsgBoxStyle.Critical, "EMAIL SENDING ERROR")
        End Try
    End Sub

    Private Sub btnCompCancel_Click(sender As Object, e As EventArgs) Handles btnCompCancel.Click
        pnlAddSharedDocs.Visible = False
    End Sub

    Private Sub btnOk_Click(sender As Object, e As EventArgs) Handles btnOk.Click
        Dim blnFound As Boolean
        Dim strSql As String

        If Not ValidData() Then Exit Sub
        'Call clsSVT.getDataStringValue("Select CustName from MstCust " _
        '           & " where CustName ='" & Trim(txtCustName.Text) & "'", "", blnFound)

        If Mode = "M" Then
            Call clsSVT.getDataLongValue("Select DocID from MstCompDocs " _
                        & " where DocFileName ='" & Trim(txtDoc.Text) & "'", 0, blnFound)
            If blnFound Then
                strSql = "Update MstCompDocs Set "
                strSql = strSql & "GroupName = '" & cboGroup.Text & "',"
                'strSql = strSql & "DocFileName = '" & Trim(txtDoc.Text) & "',"
                strSql = strSql & "Remarks = '" & Trim(txtRemarks.Text) & "'"
                strSql = strSql & " where DocID = " & Trim(txtDocID.Text)
                Call clsSVT.execSQLStatement(strSql)
                'Call FileObj.CopyFile(strFileName, gstrRptPath & "\SharedDocs\")
                fillVsfDet()
                Exit Sub
            End If


        End If

        strSql = "Insert into MstCompDocs (GroupName, DocFileName, Remarks) values ("
        strSql = strSql & "'" & Trim(cboGroup.Text) & "',"
        strSql = strSql & "'" & Trim(txtDoc.Text) & "',"
        strSql = strSql & "'" & Trim(txtRemarks.Text) & "')"

        Call clsSVT.execSQLStatement(strSql)
        Call File.Copy(strFileName, gstrRptPath & "SharedDocs\" & Path.GetFileName(strFileName))
        fillVsfDet()
        btnEdit.Visible = True
        btnRemove.Visible = True
    End Sub

    Private Function ValidData() As Boolean
        Dim blnfound As Boolean
        Dim strDocID As String

        ValidData = False

        If Trim(cboGroup.Text) = "" Then
            MsgBox("Group name cannot be empty!", vbInformation + vbOKOnly, Me.Text)
            cboGroup.Focus()
            Exit Function
        End If
        If Trim(txtDoc.Text) = "" Then
            MsgBox("Please select a document", vbInformation + vbOKOnly, Me.Text)
            btnBrowse.Focus()
            Exit Function
        End If
        If Mode = "A" Then
            strDocID = clsSVT.getDataLongValue("Select DocID from MstCompDocs " _
                        & " where DocFileName ='" & Trim(txtDoc.Text) & "'", 0, blnfound)
            If strDocID <> txtDocID.Text Then
                Call clsSVT.getDataStringValue("Select DocFileName from MstCompDocs " _
                        & " where DocFileName ='" & Trim(txtDoc.Text) & "'", "", blnfound)
                If blnfound Then
                    MsgBox("Document already exists!", vbInformation + vbOKOnly, Me.Text)
                    btnBrowse.Focus()
                    txtDoc.Text = ""
                    Exit Function
                End If
            End If
        End If

        ValidData = True

    End Function

    Private Sub fillVsfDet()
        Dim RsTemp As oledb.oledbDataReader
        Dim strSQL As String
        Dim fName As String
        Dim dfname As String
        Dim strFilePath As String
        Dim fext As String
        Dim i As Integer

        cboGroup.Text = ""
        txtDoc.Text = ""
        txtRemarks.Text = ""
        pnlAddSharedDocs.Visible = False

        strSQL = " select * from MstCompDocs "
        RsTemp = clsSVT.getDataReaderFromSQL(strSQL)

        vsfDet.Rows.Count = 1
        With RsTemp
            Do While .Read()
                dfname = GetVal(RsTemp("DocFileName"))
                fext = GetFileExtType(Path.GetExtension(gstrRptPath & "\SharedDocs\" & dfname))
                strFilePath = gstrRptPath & "\SharedDocs\" & dfname
                'PictureBox1.Image = Icon.ExtractAssociatedIcon(strFilepath).ToBitmap
                With vsfDet
                    .Rows.Count += 1
                    i = .Rows.Count - 1
                    .SetData(i, vsfDetGroup, GetVal(RsTemp("GroupName"), ""))
                    Try
                        .SetCellImage(i, vsfDetIcon, Icon.ExtractAssociatedIcon(strFilePath).ToBitmap)
                    Catch ex As Exception
                    End Try
                    .SetData(i, vsfDetFileType, fext)
                    .SetData(i, vsfDetFileName, GetVal(RsTemp("DocFileName"), ""))
                    .SetData(i, vsfDetRemarks, GetVal(RsTemp("Remarks"), ""))

                End With
            Loop
            .Close()
        End With
        With vsfDet
            If .Rows.Count > 1 Then
                .Row = 1
                lblfname.Visible = True
                lblfsize.Visible = True
                lblmod.Visible = True
                fName = gstrRptPath & "\SharedDocs\" & .GetData(.Row, vsfDetFileName)
                Dim infoReader As System.IO.FileInfo
                infoReader = My.Computer.FileSystem.GetFileInfo(fName)

                lblfname.Text = "File Name: " & Path.GetFileNameWithoutExtension(fName) & "" & Path.GetExtension(fName)
                Try
                    lblfsize.Text = "File Size: " & infoReader.Length
                Catch ex As Exception
                End Try

                lblmod.Text = "Last Modified: " & File.GetLastAccessTime(fName)
            Else
                .Row = 0
                lblfname.Visible = False
                lblfsize.Visible = False
                lblmod.Visible = False
            End If
        End With
        vsfDet.AutoSizeRows()

        vsfDet.AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.RestrictCols
        Dim j%
        For j = vsfDet.Cols.Fixed To vsfDet.Cols.Count - 1
            vsfDet.Cols(vsfDetGroup).AllowMerging = True
        Next

    End Sub

    Private Sub btnBrowse_Click(sender As Object, e As EventArgs) Handles btnBrowse.Click
        Dim n As Integer
        Dim blnFound As Boolean
        'On Error GoTo err
        Try

            With fsd
                .FileName = ""
                .ShowDialog()
                If fsd.FileName = "" Then Exit Sub
                strFileName = fsd.FileName
                strExtnName = System.IO.Path.GetExtension(fsd.FileName)
                strFilepath = System.IO.Path.GetFullPath(fsd.FileName)
                blnFound = True
                While blnFound
                    n = InStr(1, strFileName, "\")
                    If n <> 0 Then
                        strFileName = Microsoft.VisualBasic.Right(strFileName, Len(strFileName) - n)
                    Else
                        blnFound = False
                    End If
                End While

                txtDoc.Text = strFileName
                strFileName = strFilepath

            End With
        Catch ex As Exception
        End Try
        'err:
    End Sub

    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        pnlAddSharedDocs.Visible = True
        Call bindCombo(cboGroup, "GroupName", "GroupName", "select distinct GroupName from MstCompDocs order by GroupName")
        cboGroup.Focus()
        Mode = "A"
        btnBrowse.Visible = True
        btnEdit.Visible = False
        btnRemove.Visible = False
    End Sub

    Private Sub btnEdit_Click(sender As Object, e As EventArgs) Handles btnEdit.Click
        Dim blnfound As Boolean

        With vsfDet
            If .Row > 0 Then
                pnlAddSharedDocs.Visible = True
                btnBrowse.Visible = False
                Call bindCombo(cboGroup, "GroupName", "GroupName", "select distinct GroupName from MstCompDocs order by GroupName")
                cboGroup.Focus()
                Mode = "M"
                cboGroup.Text = .GetData(.Row, vsfDetGroup)
                txtDoc.Text = .GetData(.Row, vsfDetFileName)
                txtRemarks.Text = .GetData(.Row, vsfDetRemarks)
                txtDocID.Text = clsSVT.getDataLongValue("Select DocID from MstCompDocs " _
                        & " where DocFileName ='" & Trim(txtDoc.Text) & "'", 0, blnfound)
                '.Text = clsGen.GetDataValue("DocID", "", "utlSharedDocs", blnFlag, _
                '            " WHERE DocFileName = '" & Trim(txtDocument.Text) & "'")
                'imgFile.Picture = LoadPicture(App.Path & "\Images\Img" & Right(.TextMatrix(.Row, VFileName), 3) & ".jpg")
            Else
                MsgBox("You have not selected any File for Editing!!" & vbCrLf & vbCrLf,
                        vbInformation + vbOKOnly, Me.Text)
                Exit Sub
            End If
        End With
        'pnlAddSharedDocs.Enabled = False
        ''fraShrdAdd.Visible = True
        ''fraShrdAdd.Enabled = True
        'cboGroup.Focus()
    End Sub

    Private Sub btnRemove_Click(sender As Object, e As EventArgs) Handles btnRemove.Click
        With vsfDet
            If .Rows.Count > 1 Then
                If MsgBox("Are you sure you want to delete the document '" &
                          vsfDet.GetData(vsfDet.Row, vsfDetFileName),
                          vbYesNo + vbCritical, Me.Text) = vbYes Then
                    DeleteDoc()
                    fillVsfDet()
                End If
            Else
                MsgBox("No files found!", vbInformation + vbOKOnly, Me.Text)
                Exit Sub
            End If
        End With
    End Sub

    Private Sub btnList_Click(sender As Object, e As EventArgs) Handles btnList.Click
        ExportToXL(vsfDet, "")
    End Sub

    Private Sub btnCompClose_Click(sender As Object, e As EventArgs) Handles btnCompClose.Click
        Me.Close()
    End Sub

    Private Sub DeleteDoc()
        With vsfDet
            clsSVT.execSQLStatement("Delete  from MstCompDocs where DocFileName = '" &
                    .GetData(.Row, vsfDetFileName) & "'")
            File.Delete(gstrRptPath & "\SharedDocs\" & .GetData(.Row, vsfDetFileName))
        End With
    End Sub

    Private Sub vsfDet_DoubleClick(sender As Object, e As EventArgs) Handles vsfDet.DoubleClick
        Dim strFile As String

        If vsfDet.Col <> vsfDetFileName Then Exit Sub

        If vsfDet.Row > 0 Then
            strFile = gstrRptPath & "\SharedDocs\" & vsfDet.GetData(vsfDet.Row, vsfDetFileName)
            If Dir(strFile) = "" Then
                MsgBox("File not found!", vbCritical, Me.Text)
                Exit Sub
            End If
            Dim p As New System.Diagnostics.Process
            Dim s As New System.Diagnostics.ProcessStartInfo(strFile)
            s.UseShellExecute = True
            s.WindowStyle = ProcessWindowStyle.Normal
            p.StartInfo = s
            p.Start()
        End If
    End Sub

    Private Function BytesToString(ByVal byteCount As Long)
        Dim bytes As Long
        Dim place As Integer
        Dim num As Double
        Dim suf() As String = {" B", " KB", " MB", " GB", " TB", " PB", " EB"}

        If byteCount = 0 Then
            Return "0" & suf(0)
        End If

        bytes = Math.Abs(byteCount)
        place = Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)))
        num = Math.Round(bytes / Math.Pow(1024, place), 1)
        Return (Math.Sign(byteCount) * num).ToString & suf(place)
    End Function

    Private Sub TruncateDBLog()
        Dim strSQL As String
        Dim strEx As String
        strSQL = "SELECT ' DBCC SHRINKFILE (N''' + mf.name + N''' , 1)'"
        strSQL &= "FROM "
        strSQL &= "sys.master_files mf "
        strSQL &= "Join sys.databases d "
        strSQL &= "On mf.database_id = d.database_id "
        strSQL &= "WHERE d.database_id > 4"
        strSQL &= "And mf.type_desc = 'LOG'"
        strSQL &= "And upper(d.name) = '" & UCase(gStrDBName) & "'"
        strEx = clsSVT.getDataStringValue(strSQL, "", False)
        If strEx <> "" Then
            Try
                Call clsSVT.execSQLStatement("ALTER DATABASE " & gStrDBName & " SET RECOVERY SIMPLE WITH NO_WAIT ")
                Call clsSVT.execSQLStatement(strEx)
                Call clsSVT.execSQLStatement("use master")
                Call clsSVT.execSQLStatement("ALTER DATABASE " & gStrDBName & " SET RECOVERY FULL WITH NO_WAIT")
                Call clsSVT.execSQLStatement("use " & gStrDBName)

                MsgBox("Truncated log successfully!", MsgBoxStyle.Information, "Truncated Log")
            Catch ex As Exception
                MsgBox("Unable to Truncate log", MsgBoxStyle.Critical, "Log File Truncation")
                Call clsSVT.execSQLStatement("use " & gStrDBName)
            End Try
        End If
    End Sub

    Private Sub btnBackup_LostFocus(sender As Object, e As EventArgs) Handles btnBackup.LostFocus
        btnCompanies.Focus()
    End Sub

    Private Sub btnCancel_LostFocus(sender As Object, e As EventArgs) Handles btnCancel.LostFocus
        If C1DockingTabPage1.IsSelected Then
            With vsfCDET
                .Select()
                .Row = 0
                .Col = 0
            End With
        End If

        If C1DockingTabPage2.IsSelected Then
            With vsfHdr
                .Select()
                .Row = 0
                .Col = 0
            End With
        End If

        If C1DockingTabPage3.IsSelected Then
            With vsfFnlYr
                .Select()
                .Row = 0
                .Col = 0
            End With

        End If

        If TPDocuments.IsSelected Then
            With vsfDet
                .Select()
                .Row = 0
                .Col = 0
            End With
        End If

        If TabPageImage.IsSelected Then
            btnChangeImg.Focus()
        End If

    End Sub

    Private Sub vsfFnlYr_KeyDown(sender As Object, e As KeyEventArgs) Handles vsfFnlYr.KeyDown
        If vsfFnlYr.Row = vsfFnlYr.Rows.Count - 1 AndAlso vsfFnlYr.Col = vsfFnlYr.Cols.Count - 1 AndAlso e.KeyCode = Keys.Tab Then
            btnSelFinYear.Focus()
        End If
    End Sub

    Private Sub btnRemoveFinYr_LostFocus(sender As Object, e As EventArgs) Handles btnRemoveFinYr.LostFocus
        btnBackup.Focus()
    End Sub

    Private Sub btnChangeImg_LostFocus(sender As Object, e As EventArgs) Handles btnChangeImg.LostFocus
        btnBackup.Focus()
    End Sub

    Private Sub vsfDet_KeyDown(sender As Object, e As KeyEventArgs) Handles vsfDet.KeyDown
        If vsfDet.Row = vsfDet.Rows.Count - 1 AndAlso vsfDet.Col = vsfDet.Cols.Count - 1 AndAlso e.KeyCode = Keys.Tab Then
            btnAdd.Focus()
        End If
    End Sub

    Private Sub llblDashBoard_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles llblDashBoard.LinkClicked
        pnlDashBrd.Visible = True
        txtDashBrdUrl.Focus()
        Dim strDashLink As String = clsSVT.getDataStringValue("select DashBoardLink from MstComp", "", False)
        txtDashBrdUrl.Text = strDashLink
    End Sub

    Private Sub btnUpdateDash_Click(sender As Object, e As EventArgs) Handles btnUpdateDash.Click
        Dim strEDashLink As String
        strEDashLink = clsSVT.getDataStringValue("Select DashBoardLink from MstComp", "", False)
        If strEDashLink <> Trim(txtDashBrdUrl.Text) Then
            clsSVT.execSQLStatement("Update MstComp set DashBoardLink = '" & txtDashBrdUrl.Text & "'")
            MsgBox("DashBoard link updated successfully!", MsgBoxStyle.Information, Me.Text)
            pnlDashBrd.Visible = False
            Exit Sub
        End If
    End Sub

    Private Sub btnCancelDash_Click(sender As Object, e As EventArgs) Handles btnCancelDash.Click
        pnlDashBrd.Visible = False
    End Sub

    Private Sub btnBackup_Click(sender As Object, e As EventArgs) Handles btnBackup.Click

    End Sub
End Class