Imports System.IO
Public Class FrmUtlGSettings

    Const vDetSetGrp = 0
    Const vDetSetName = 1
    Const vDetSetCaption = 2
    Const vDetSetVal = 3
    Const vDetRemarks = 4

    'vsfGrp for Filter
    Const vItemGroup = 0

    Dim strFlds As String
    Dim Mode As String
    Dim intDocID As Integer

    Dim clsSVT As New svtLibrary
    Dim strMstItemFlds As String
    Dim strMstPriceCatFlds As String

    Private Sub saveImage()
        Dim imagePath As String
        Dim strSql As String
        Dim img As Byte() = Nothing

        Dim idb As New OpenFileDialog

        idb.Title = "Select your image."
        idb.Filter = "Image Files|*.jpg;"
        idb.RestoreDirectory = False

        If idb.ShowDialog() = DialogResult.OK Then
            imagePath = idb.FileName
        Else
            Exit Sub
        End If

        Dim Extension As String = Path.GetExtension(imagePath)

        If StrComp(Extension, ".jpg", CompareMethod.Text) = 0 Or
           StrComp(Extension, ".jpeg", CompareMethod.Text) = 0 Then

            img = GetImage(imagePath)

            Dim cmd As oledb.oledbCommand
            strSql = "Update MstComp set JpgFile = ?"
            cmd = New oledb.oledbCommand(strSql, svtConn)
            cmd.Parameters.Add("@Image", OleDb.OleDbType.VarBinary).Value = img

            If cmd.ExecuteNonQuery() Then
                MsgBox("Image updated successfully! Please restart the application for changes to take effect.", vbInformation, Me.Text)
            End If
            Exit Sub
        Else
            MsgBox("Please enter only jpg/jpeg images!", vbInformation, Me.Text)
            Exit Sub

        End If
    End Sub

    Public Shared Function GetImage(ByVal imagePath As String) As Byte()
        Dim stream As FileStream = New FileStream(imagePath, FileMode.Open, FileAccess.Read)
        Dim reader As BinaryReader = New BinaryReader(stream)
        Dim img() As Byte = reader.ReadBytes(stream.Length)

        reader.Close()
        stream.Close()

        Return img
    End Function

    Private Sub FrmUtlGSettings_KeyUp(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyUp
        If e.KeyCode = Keys.F9 And e.Control = True Then
            Call initFormSize(Me, Me.Text, True, 888, 671, 100, 4)
            MsgBox("Restored Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
        If e.KeyCode = Keys.F8 And e.Control = True Then
            Call SaveActFormSettings(Me, Me.Text)
            Call SaveActFormVsfDet(Me, "G Settings", "DET", vsfDET)
            Call SaveActFormVsfDet(Me, "G Settings", "FILTER", vsfGrp)
            MsgBox("Saved Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If

        If e.Alt And e.Control And e.KeyCode.ToString = "I" Then
            saveImage()
        End If

    End Sub

    Private Sub FrmUtlGlobe_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'intDocID = 40
        With vsfDET
            .Rows.Count = 2
            .Cols.Count = 5
            .Cols(0).Width = 200

            .SetData(0, vDetSetGrp, "SetGroup")   'Group
            .SetData(0, vDetSetName, "SetName")   'Name
            .SetData(0, vDetSetVal, "SetValue")   'Value
            .SetData(0, vDetSetCaption, "SetCaption")  'Caption
            .SetData(0, vDetRemarks, "Remarks")   'Remarks
        End With
        Call SetColWidth(vsfDET, 200, 20, 200, 100)
        Call SetHideCol(vsfDET, True, vDetSetGrp, vDetSetName, vDetRemarks)
        Call SetFlexColor(vsfDET, "", vDetSetGrp, vDetSetName, vDetSetCaption)
        With vsfGrp
            .Rows.Count = 1
            .Cols.Count = 1
            .SetData(0, vItemGroup, "SetGroup")   'Group
        End With

        Call SetFlexColor(vsfGrp, "", vItemGroup)
        Call Clearflds()
        Call FillFlds()
        Call vsfGrp_Click(sender, e)
        Call initFormSize(Me, Me.Text, False, 888, 671, 100, 4)
        Call initFormVsfDet(Me, "G Settings", vsfDET, "DET")
        Call initFormVsfDet(Me, "G Settings", vsfGrp, "FILTER")
        Me.CenterToScreen()
        If Not blnSMSFeature Then btnAdnlSettings.Items(2).Visible = False
        If UCase(gUserCode) <> "ADMIN" Then ddLoginSettings.Visible = False

    End Sub

    Private Sub FillFlds()
        Dim intRow As Integer
        Dim rs As oledb.oledbDataReader
        Dim strSQL As String, intIGRow As Integer, blnIGFound As Boolean

        Call bindCombo(cboSetGrp, "SetGroup", "SetGroup", "Select distinct SetGroup from UtlGSettings")

        strSQL = "Select * from  UtlGSettings where HideSetting <> 'Y' order by SetGroup "
        rs = clsSVT.getDataReaderFromSQL(strSQL)

        vsfDET.Rows.Count = 1
        With rs
            Do While .Read
                With vsfDET
                    intRow = .Rows.Count
                    .Rows.Count = .Rows.Count + 1
                    .SetData(intRow, vDetSetGrp, GetVal(rs!SetGroup, ""))

                    If Not getItemGroup(rs("SetGroup")) Then
                        blnIGFound = True
                        intIGRow = vsfGrp.Rows.Count
                        vsfGrp.Rows.Count = vsfGrp.Rows.Count + 1
                        vsfGrp.SetData(intIGRow, vItemGroup, GetVal(rs("SetGroup"), ""))
                    End If
                    .SetData(intRow, vDetSetName, GetVal(rs!SetName, ""))
                    .SetData(intRow, vDetSetVal, GetVal(rs!SetValue, ""))
                    .SetData(intRow, vDetSetCaption, GetVal(rs!SetCaption, ""))
                    .SetData(intRow, vDetRemarks, GetVal(rs!Remarks, ""))
                End With
            Loop
            rs.Close()
        End With
        Mode = "V"
        Call showbtns("VIEW")
    End Sub

    Private Function getItemGroup(ByVal argItemGroup As String) As Boolean
        Dim i As Integer
        getItemGroup = False

        With vsfGrp
            For i = 1 To .Rows.Count - 1
                If UCase(.GetData(i, vItemGroup)) = UCase(argItemGroup) Then
                    getItemGroup = True
                    Exit Function
                End If
            Next
        End With

        getItemGroup = False
    End Function
    Private Sub Clearflds()
        txtCaption.Text = ""
        txtCode.Text = ""
        txtValue.Text = ""
        txtRemarks.Text = ""
        cboSetGrp.Text = ""
        lblToolTip.Text = ""
        chkSel.Checked = False
        chkHide.Checked = False
        Mode = ""
        pnlGrpSet.Visible = False
        Call showbtns("VIEW")
    End Sub

    Private Sub btnAdd_ClickButtonArea(Sender As System.Object, e As EventArgs) Handles btnAdd.Click
        Call Clearflds()
        Mode = "A"
        pnlGrpSet.Visible = True
        txtCode.Enabled = True
        txtCode.Focus()
        Call showbtns("ADD")
    End Sub

    Private Sub btnSave_ClickButtonArea(Sender As System.Object, e As EventArgs) Handles btnSave.Click
        Call stringReplace(Me)
        If ValidFields() Then
            Call saveDetails()
            Call showbtns("SAVE")
        End If
    End Sub

    Private Function ValidFields() As Boolean

        ValidFields = False
        Dim blnfound As Boolean

        If Mode = "V" Then Exit Function

        If txtCode.Text = "" Then
            MsgBox("FldCode should not be blank!!!", MsgBoxStyle.Information, Me.Text)
            txtCode.Focus()
            Exit Function
        End If

        If txtCaption.Text = "" Then
            MsgBox("FldCaption should not be blank!!!", MsgBoxStyle.Information, Me.Text)
            txtCaption.Focus()
            Exit Function
        End If

        If Mode = "A" Then
            clsSVT.getDataStringValue("Select SetName from UtlGSettings where SetName = '" & Trim(txtCode.Text) & "'", "", blnfound)

            If blnfound Then
                MsgBox("FldCode already exists...", MsgBoxStyle.Information, Me.Text)
                txtCode.Text = ""
                txtCode.Focus()
                Exit Function
            End If

        End If
        ValidFields = True

    End Function

    Private Function saveDetails() As Boolean
        Dim strSQL As String = ""

        saveDetails = False


        If Mode = "A" Then
            strSQL = " Insert into UtlGSettings("
            strSQL = strSQL & " SetGroup,SetName,SetValue,SetCaption,Remarks,HideSetting) values ("
            strSQL = strSQL & "'" & Trim(GetVal(cboSetGrp.Text, "")) & "',"
            strSQL = strSQL & "'" & Trim(GetVal(txtCode.Text, "")) & "',"
            strSQL = strSQL & "'" & Trim(GetVal(txtValue.Text, "")) & "',"
            strSQL = strSQL & "'" & Trim(GetVal(txtCaption.Text, "")) & "',"
            strSQL = strSQL & "'" & Trim(GetVal(txtRemarks.Text, "")) & "',"
            strSQL = strSQL & "'" & Trim(GetVal(getCheckString(chkHide), "N")) & "')"
        End If



        Try
            If clsSVT.execSQLStatement(strSQL) Then

                If Mode = "A" Then
                    MsgBox("'" & Trim(txtCode.Text) & "' added successfully.", vbInformation, Me.Text)
                End If

                Call Clearflds()
                saveDetails = True

                Call FillFlds()
            End If
        Catch ex As Exception
            MsgBox("Error during save" & vbCrLf & ex.Message, MsgBoxStyle.Critical, Me.Text)

            Exit Function
        End Try
    End Function

    Private Sub showbtns(ByVal argtype As String)
        Select Case argtype
            Case "CANCEL", "SAVE", "VIEW"
                btnCancel.Visible = False
                btnSave.Visible = False
                btnAdd.Visible = True
                chkSel.Visible = True
                If Mode = "M" Then
                    btnSaveVal.Visible = True
                    btnCancelDtl.Visible = True
                Else
                    btnSaveVal.Visible = False
                    btnCancelDtl.Visible = False
                End If
                btnClose.Visible = True
            Case "ADD", "EDIT"
                btnCancel.Visible = True
                btnSave.Visible = True
                btnAdd.Visible = False
                chkSel.Visible = False
                btnSaveVal.Visible = False
                btnCancelDtl.Visible = False
                btnClose.Visible = False
        End Select
    End Sub

    Private Sub vsfDET_AfterEdit(sender As Object, e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfDET.AfterEdit
        With vsfDET
            Mode = ""
            Select Case e.Col
                Case vDetSetVal, vDetSetCaption
                    Mode = "M"
                    e.Cancel = False
                    Call showbtns("SAVE")
                Case Else
                    e.Cancel = True
            End Select
        End With
    End Sub

    Private Sub vsfDET_BeforeEdit(sender As Object, e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfDET.BeforeEdit
        With vsfDET
            Select Case e.Col
                Case vDetSetVal, vDetSetCaption 'changed by SVP
                    e.Cancel = False
                    If vsfGrp.GetData(vsfGrp.Row, vItemGroup) = "ToolTip" Then  'CNUD
                        For i As Integer = 1 To .Rows.Count - 1
                            If .GetData(i, vDetSetCaption) = "ToolTipEntries" Then
                                .Cols(vDetSetVal).ComboList = "..."
                            End If
                        Next
                    Else
                        For i As Integer = 1 To .Rows.Count - 1
                            .Cols(vDetSetVal).ComboList = Nothing
                        Next
                    End If

                Case Else
                    e.Cancel = True
            End Select
        End With
    End Sub

    Private Sub vsfDET_Click(sender As Object, e As System.EventArgs) Handles vsfDET.Click
        pnlGrpSet.Visible = False
        Call showbtns("CANCEL")
    End Sub

    Private Sub vsfDET_DoubleClick(sender As Object, e As System.EventArgs) Handles vsfDET.DoubleClick

        'If vsfDET.Col = vDetSetCaption Then
        '    pnlGrpSet.Visible = True
        '    Call fillEditDtls()
        'End If
    End Sub

    Private Sub btnCancel_ClickButtonArea(Sender As System.Object, e As EventArgs) Handles btnCancel.Click
        pnlGrpSet.Visible = False
        Call Clearflds()
    End Sub

    Private Sub btnClose_ClickButtonArea(Sender As System.Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
        'frmMain.dockTabMain.SelectedTab.Dispose()
    End Sub

    Private Sub vsfGrp_BeforeEdit(sender As Object, e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfGrp.BeforeEdit
        e.Cancel = True
    End Sub

    Private Sub vsfGrp_Click(sender As Object, e As System.EventArgs) Handles vsfGrp.Click
        Dim j As Integer

        With vsfGrp
            Select Case .Col
                Case vItemGroup
                    For j = 1 To vsfDET.Rows.Count - 1
                        If UCase(.GetData(.Row, vItemGroup)) = UCase(vsfDET.GetData(j, vDetSetGrp)) Then
                            vsfDET.Rows(j).Visible = True
                        Else
                            vsfDET.Rows(j).Visible = False
                        End If
                    Next
            End Select
        End With
        chkSel.Checked = False
        With vsfDET
            For i = 1 To vsfDET.Rows.Count - 1
                If .Rows(i).Visible Then
                    .Row = i
                    lblToolTip.Text = Trim(GetVal(.GetData(.Row, vDetRemarks), ""))
                End If
            Next
        End With
        Mode = ""
        Call showbtns("CANCEL")
    End Sub

    Private Sub chkSel_CheckedChanged(sender As Object, e As System.EventArgs) Handles chkSel.CheckedChanged
        Dim i As Integer
        With vsfDET
            If chkSel.Checked = True Then
                .AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.RestrictAll
                For i = 0 To .Rows.Count - 1
                    vsfDET.Rows(i).Visible = True
                Next
                vsfDET.Cols(vDetSetGrp).Visible = True
                .Cols(vDetSetGrp).AllowMerging = True
            Else
                vsfGrp_Click(New EventArgs, e)
                vsfDET.Cols(vDetSetGrp).Visible = False
            End If

        End With
    End Sub

    Private Sub txtCode_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles txtCode.KeyPress
        Dim AllowedKeys As String = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789,"
        Select Case e.KeyChar

            Case Convert.ToChar(Keys.Enter) ' Enter is pressed
                ' Call method here...

            Case Convert.ToChar(Keys.Back) ' Backspace is pressed
                e.Handled = False ' Delete the character

                'Case Convert.ToChar(Keys.Capital Or Keys.RButton) ' CTRL+V is pressed
                '    ' Paste clipboard content only if contains allowed keys
                '    e.Handled = Not Clipboard.GetText().All(Function(c) AllowedKeys.Contains(c))

            Case Else ' Other key is pressed
                ' MsgBox("")
                e.Handled = Not AllowedKeys.Contains(e.KeyChar)


        End Select

    End Sub

    Private Sub btnSaveVal_ClickButtonArea(Sender As System.Object, e As EventArgs) Handles btnSaveVal.Click
        If UpdateValDtl() Then
            btnCancelDtl_ClickButtonArea(New EventArgs, e)
            gstrVsfItemCodeCols = getGsettings("Items Details", "VsfItemCodeCols", "")
            gCustQryFlds = getGsettings("Party", "QryCust", "")
            gSuppQryFlds = getGsettings("Party", "QrySupp", "")
            Call frmMain.InitCompData()
        End If
    End Sub

    Private Function UpdateValDtl() As Boolean
        Dim strSQL As String = ""
        Dim i As Integer

        UpdateValDtl = False
        With vsfDET
            If Mode = "M" Then

                For i = 1 To .Rows.Count - 1

                    Try
                        UpdateValDtl = False

                        If vsfDET.Rows(i).Visible = True Then
                            strSQL = "Update UtlGSettings set "
                            strSQL = strSQL & " SetCaption = '" & Trim(GetVal(vsfDET.GetData(i, vDetSetCaption), "")) & "',"
                            strSQL = strSQL & " SetValue = '" & Replace(Trim(GetVal(vsfDET.GetData(i, vDetSetVal), "")), "'", "''") & "'"
                            strSQL = strSQL & " where SetName = '" & Trim(vsfDET.GetData(i, vDetSetName)) & "'"

                            If Not clsSVT.execSQLStatement(strSQL) Then

                                UpdateValDtl = False
                                Exit Function
                            End If
                        End If

                    Catch ex As Exception
                        MsgBox("Error during save" & vbCrLf & ex.Message, MsgBoxStyle.Critical, Me.Text)

                    End Try
                Next

                If Mode = "M" Then
                    MsgBox("Updated successfully.", vbInformation, Me.Text)
                    UpdateValDtl = True
                End If

            End If

        End With

    End Function

    Private Sub btnCancelDtl_ClickButtonArea(Sender As System.Object, e As EventArgs) Handles btnCancelDtl.Click
        Call FillFlds()
        Call vsfGrp_Click(Sender, e)
        Call Clearflds()
    End Sub

    Private Sub btnAdnlSettings_DropDownItemClicked(sender As Object, e As C1.Win.C1Input.DropDownItemClickedEventArgs) Handles btnAdnlSettings.DropDownItemClicked
        Try
            Dim objFrm As Object
            Select Case e.ClickedItem.Text
                Case "Invoice Settings"
                    objFrm = New frmUtlInvNumberFmt
                Case "Grid Colour Settings"
                    objFrm = New frmUtlGColourSettings
                Case "SMTP Settings"
                    objFrm = New frmUtlSMTPSettings
                Case "SMS Settings"
                    objFrm = New frmUtlSendSMS
                Case "Post Doc List"
                    objFrm = New frmUtlPostDocList
                Case "Doc Prefix Settings"
                    objFrm = New frmUtlDocNumber
                Case "Form Colour Settings"
                    objFrm = New frmUtlFormColourSettings
                Case "Form Userwise Settings"
                    objFrm = New frmUtlFormSettings
                    'frmUtlFormSettings.ShowDialog()
                Case "Login Settings"
                    objFrm = New frmUtlLoginSettings
                Case "Field Captions"
                    objFrm = New frmUtlFldCaptions
                Case "Menu Sequence"
                    objFrm = New frmUtlMenuSequence
                Case "Delete Linkage Updation"
                    objFrm = New FrmUtlDeleteLinkageUpdation
                Case "Transaction Settings"
                    objFrm = New FrmUtlTrnSettings
                Case "Print Settings"
                    objFrm = New FrmUtlPrintSettings
                Case "Print Settings"
                    objFrm = New FrmUtlPrintSettings
                Case "View Search Display Fields"
                    objFrm = New frmUtlFindFlds
                Case "WhatsApp Settings"
                    objFrm = New frmUtlWAS
                Case "Bulk Item Import Fields"
                    objFrm = New frmUtlBulkImpFlds

                Case Else
                    objFrm = Nothing
            End Select

            If Not IsNothing(objFrm) Then
                objFrm.StartPosition = FormStartPosition.CenterScreen
                objFrm.ShowDialog()
                objFrm.Close()
            End If

            Call FillFlds()

            With vsfGrp
                Select Case .Col
                    Case .Col = 1
                        .Row = 2
                        .Focus()
                        vsfGrp_Click(New EventArgs, e)
                End Select
            End With
        Catch ex As Exception

        End Try
    End Sub
    Private Sub vsfDET_RowColChange(sender As Object, e As System.EventArgs) Handles vsfDET.RowColChange
        Dim strRmks As String

        With vsfDET
            If .Row < 1 Then Exit Sub
            strRmks = Trim(GetVal(.GetData(.Row, vDetRemarks), ""))
            lblToolTip.Text = strRmks
        End With
    End Sub

    Private Sub vsfDET_TextChanged(sender As Object, e As System.EventArgs) Handles vsfDET.TextChanged
        Call showbtns("EDIT")
    End Sub



    Private Sub FrmUtlGSettings_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        Call frmMain.CloseSelectedTabPage(Me)
    End Sub


End Class