Imports System.Globalization
Imports System.IO
Imports System.Data.oledb
Imports C1.Win.C1FlexGrid
Imports System.ComponentModel

Public Class frmUtlMenuSequence

    Const vFLMenuItemID = 0
    Const vFLMenuItemCaption = 1
    Const vFLMenuGrpName = 2
    Const vFLMenuItemName = 3
    Const vFLMenuItemType = 4
    Const vFLOptDet1 = 5
    Const vFLOptDet2 = 6
    Const vFLOptDet3 = 7
    Const vFLOptDet4 = 8
    Const vFLOptDet5 = 9
    Const vFLMenuindex = 10
    Const vFLDocID = 11
    Const vFLFrmName = 12
    Const vFLMenuShow = 13
    Const vFLSCKey = 14
    Const vFLSCName = 15
    Public Mode As String
    Const vTotCols = 16

    Public strNewHotKey As String = ""
    Dim blnDup As Boolean = False

    Dim clsSVT As New svtLibrary
    Dim intMenuType As Integer, strVoucherType As String, intMenuIndex As Integer

    Private Sub UploadBtn_Click(sender As Object, e As EventArgs) Handles BtnUpload.Click
        Dim imagePath As String
        Dim strSql As String
        Dim img As Byte() = Nothing
        Dim idb As New OpenFileDialog
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
                PicBox.Image = Image.FromFile(idb.FileName)
                PicBox.SizeMode = PictureBoxSizeMode.StretchImage
                Dim cmd As oledb.oledbCommand 
                strSql = "Update MstMenu set  MenuImage = ? where MenuItemID='" & vsfDET.GetData(vsfDET.Row, vFLMenuItemID) & "' "
                cmd = New oledb.oledbCommand (strSql, svtConn)
                cmd.Parameters.AddWithValue("@Image", img)
                If cmd.ExecuteNonQuery() Then
                    MsgBox("Image updated successfully!", vbInformation, Me.Text)
                End If
                BtnRemove.Visible = True
                Exit Sub
            Else
                MsgBox("Please enter only jpg/jpeg images!", vbInformation, Me.Text)
                Exit Sub
            End If
        Catch ex As Exception
            MsgBox("Error while update images!", vbCritical, Me.Text)
        End Try
    End Sub

    Const conChunkSize = 8192

    Private Sub frmUtlMenuSequence_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        FraGrpName.Visible = False
        Call initGridStyles(Me)
        Call initGrid()
        Call fillFormProperties(Me)
        Call initFormSize(Me, Me.Text, False, 594, 505, 0, 0)
        Call initFormVsfDet(Me, Me.Text, vsfDET, "DET")
        BtnUpload.Enabled = False
        BtnRemove.Enabled = False
        btnSave.Enabled = False
        BtnAddSeprator.Enabled = False
        chkSelKey.Checked = False
        chkSelKey.Enabled = False
        pnlSCKeys.Visible = False
        btnApply.Visible = False
        btnClear.Visible = False
        PnlArrow.Visible = False
        fillDocDetails()
        Call bindCombo(cboGrpName, "MenuGroupName", "MenuGroupName", "Select distinct MenuGroupName from MstMenu")
    End Sub

    Public Sub fillDocDetails()
        Dim strKey As String = ""

        If Trim(strNewHotKey) <> "" Then
            strNewHotKey = Replace(strNewHotKey, " ", "")
            If strNewHotKey.Contains("Alt") Then
                chkAlt.Checked = True
            Else
                chkAlt.Checked = False
            End If
            If strNewHotKey.Contains("Ctrl") Then
                chkCtrl.Checked = True
            Else
                chkCtrl.Checked = False
            End If
            If strNewHotKey.Contains("Shift") Then
                chkShift.Checked = True
            Else
                chkShift.Checked = False
            End If
            strKey = strNewHotKey.Substring(strNewHotKey.LastIndexOf("+") + 1, strNewHotKey.Length - strNewHotKey.LastIndexOf("+") - 1)
            strKey = strKey.Replace(" ", "")
            cboKey.Text = strKey
        End If
    End Sub

    Private Sub initGrid()
        cboType.Text = "-- Select --"
        With vsfDET
            .Rows.Count = 1
            .Cols.Count = vTotCols
            .SetData(0, vFLMenuItemID, "Id")
            .SetData(0, vFLMenuItemCaption, "Menu")
            .SetData(0, vFLMenuItemName, "Item Name")
            .SetData(0, vFLMenuItemType, "Type")
            .SetData(0, vFLOptDet1, "opt1")
            .SetData(0, vFLOptDet2, "opt2")
            .SetData(0, vFLOptDet3, "opt3")
            .SetData(0, vFLOptDet4, "opt4")
            .SetData(0, vFLOptDet5, "opt5")
            .SetData(0, vFLMenuindex, "Index")
            .SetData(0, vFLDocID, "DocId")
            .SetData(0, vFLMenuShow, "Show")
            .SetData(0, vFLMenuGrpName, "Group Name")
            .SetData(0, vFLFrmName, "FormName")
            .SetData(0, vFLSCKey, "ShortCut Keys")
            .SetData(0, vFLSCName, "ShortCut Name")
            .Cols(vFLMenuShow).ComboList = "Y|N"
            Call SetHideCol(vsfDET, True, vFLMenuItemID, vFLMenuItemName, vFLMenuItemType,
                    vFLOptDet1, vFLOptDet2, vFLOptDet3, vFLOptDet4, vFLOptDet5, vFLMenuindex, vFLDocID,
                  vFLFrmName)
            Call SetFlexColor(vsfDET, "", vFLSCKey)
            Call SetFlexColor(vsfDET, "", vFLMenuItemCaption, vFLMenuGrpName)
        End With
        BtnUpload.Enabled = False
        BtnRemove.Enabled = False
    End Sub


    Private Sub LoadBtn_Click(sender As Object, e As EventArgs) Handles BtnLoad.Click
        If cboType.Text = "" Then
            chkSelKey.Enabled = False
            pnlSCKeys.Visible = False
            Exit Sub
        End If
        chkSelKey.Enabled = True
        pnlSCKeys.Visible = True
        pnlSCKeys.Enabled = False
        cboKey.Text = ""
        chkCtrl.Checked = False
        chkShift.Checked = False
        chkAlt.Checked = False
        vsfDET.Rows.Count = 1
        Call LoadFormList()
        Call RemoveEmptVSFRows(vsfDET, 0, vFLMenuItemCaption)
        BtnUpload.Enabled = True
        BtnRemove.Enabled = True
        btnSave.Enabled = True
        BtnAddSeprator.Enabled = True
        If vsfDET.Rows.Count > 2 Then
            PnlArrow.Visible = True
        Else
            PnlArrow.Visible = False
        End If
    End Sub

    Private Sub ClearForm()
        vsfDET.Rows.Count = 1
        cboType.Focus()
        FraGrpName.Visible = False
    End Sub

    Private Function validSeperator() As Boolean
        Dim i As Integer
        validSeperator = False
        With vsfDET
            If .GetData(1, vFLMenuItemCaption) = "-" Then
                MsgBox("Separator at 1st Line is not allowed,cannot Continue saving!!", vbInformation, Me.Text)
                Exit Function
            ElseIf .GetData(.Rows.Count - 1, vFLMenuItemCaption) = "-" Then
                MsgBox("Separator at Last Line is not allowed,cannot Continue saving!!", vbInformation, Me.Text)
                Exit Function
            Else
                For i = 1 To .Rows.Count - 2
                    If .GetData(i, vFLMenuItemCaption) = "-" And
                   .GetData(i, vFLMenuItemCaption) = .GetData(i + 1, vFLMenuItemCaption) Then
                        MsgBox("Two CONTINUOUS Separators are not allowed,cannot Continue saving!!", vbInformation, Me.Text)
                        Exit Function
                    End If
                Next i
            End If
        End With
        validSeperator = True
    End Function

    Private Function SaveMenuSettings() As Boolean
        Dim strsql As String, i As Integer, strCond As String
        SaveMenuSettings = False
        'Dim svtTrn As oledb.oledbTransaction
        'svtTrn = Nothing
        Try
            'svtTrn = svtConn.BeginTransaction(IsolationLevel.ReadCommitted)
            If intMenuType = 2 Then
                strCond = " and (DocID in (Select DocID from UtlTransactions where Voucher = '" & strVoucherType & "') or MenuItemCaption='-')"
            Else
                strCond = ""
            End If
            strsql = "Delete from MstMenu Where MenuItemType = " & intMenuType & " and MenuItemCaption = '-'" & strCond
            clsSVT.execSQLStatement(strsql)

            With vsfDET
                For i = 1 To .Rows.Count - 1
                    strsql = ""
                    If .GetData(i, vFLMenuItemCaption) <> "-" Then
                        ' Non separator Items
                        strsql = "Update MstMenu set "
                        strsql = strsql & " MenuIndex = " & intMenuIndex + i & ","
                        strsql = strsql & " MenuGroupName = '" & .GetData(i, vFLMenuGrpName) & "',"
                        strsql = strsql & " MenuShow = '" & .GetData(i, vFLMenuShow) & "',"
                        strsql = strsql & " SCMenuName = '" & .GetData(i, vFLSCName) & "',"
                        strsql = strsql & " SCKeys = '" & .GetData(i, vFLSCKey) & "'"
                        strsql = strsql & " where MenuItemID =" & .GetData(i, vFLMenuItemID) & ""
                        clsSVT.execSQLStatement(strsql)
                    Else
                        strsql = "insert into MstMenu (MenuItemID, MenuItemCaption, MenuItemName,"
                        strsql = strsql & " MenuItemType, OptDet1, OptDet2, OptDet3, Menuindex,"
                        strsql = strsql & " MenuShow, MenuGroupName,SCMenuName,SCKeys) values ( "
                        strsql = strsql & clsSVT.getDataLongValue("Select Max(MenuItemID) + 1  From MstMenu ", 0, False)
                        strsql = strsql & ", '-',"
                        strsql = strsql & " '" & .GetData(i, vFLMenuItemName) & "',"
                        strsql = strsql & intMenuType & ","
                        strsql = strsql & " '',"
                        strsql = strsql & " '',"
                        strsql = strsql & " '',"
                        strsql = strsql & intMenuIndex + i & ","
                        strsql = strsql & " '" & .GetData(i, vFLMenuShow) & "',"
                        strsql = strsql & " '" & .GetData(i, vFLMenuGrpName) & "',"
                        strsql = strsql & " '" & .GetData(i, vFLSCName) & "',"
                        strsql = strsql & " '" & .GetData(i, vFLSCKey) & "'"
                        strsql = strsql & " )"
                        clsSVT.execSQLStatement(strsql)
                    End If
                Next i

            End With
            SaveMenuSettings = True
            Exit Function
        Catch ex As Exception
            MsgBox("Error during save" & vbCrLf & ex.Message, MsgBoxStyle.Critical, Me.Text)
            SaveMenuSettings = False
        End Try
    End Function

    Private Sub ReArrengeMenuIndex()
        Dim j As Integer
        j = 0
        With vsfDET
            For j = 1 To .Rows.Count - 1
                .SetData(j, vFLMenuindex, j)
            Next j
        End With
    End Sub

    Private Sub txtGrpName_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtGrpName.KeyPress
        If e.KeyChar = "'" Then
            e.Handled = True
        End If
    End Sub

    Private Sub frame1_MouseMove(sender As Object, e As MouseEventArgs) Handles frame1.MouseMove
        lblHelp.Text = ""
    End Sub


    Private Function LoadFormList() As Boolean
        Dim strSQL As String, strCond As String
        Dim cmdDummy As New oledbCommand
        Dim rsDummy As oledbDataReader
        Try
            LoadFormList = False
            If cboType.Text <> "Vouchers" Then
                intMenuType = Choose(cboType.SelectedIndex + 1, 1, 2, 4, 3, 5, 6, 7)
                strVoucherType = "N"
            Else
                intMenuType = 3
                strVoucherType = "Y"
            End If
            intMenuIndex = Choose(intMenuType, 0, 1000, 2000, 3000, 4000, 5000, 6000)
            If intMenuType <> 4 Then
                strCond = " And (DocID in (Select DocID from UtlTransactions where Voucher = '" _
                        & strVoucherType & "') or  MenuItemCaption='-') "
            Else
                strCond = ""
            End If
            strSQL = "Select * from MstMenu"
            strSQL = strSQL & " Where MenuItemType = " & intMenuType & " order By MenuIndex"
            rsDummy = clsSVT.getDataReaderFromSQL(strSQL)
            With vsfDET
                Call initGrid()
                If Not rsDummy.HasRows Then
                    rsDummy.Close()
                    Exit Function
                End If
                Do While rsDummy.Read
                    .Rows.Count = .Rows.Count + 1
                    .SetData(.Rows.Count - 1, vFLMenuItemID, rsDummy.Item("MenuItemID"))
                    .SetData(.Rows.Count - 1, vFLMenuItemCaption, rsDummy.Item("MenuItemCaption"))
                    .SetData(.Rows.Count - 1, vFLMenuGrpName, rsDummy("MenuGroupName"))
                    .SetData(.Rows.Count - 1, vFLMenuShow, rsDummy.Item("MenuShow"))
                    .SetData(.Rows.Count - 1, vFLOptDet1, rsDummy.Item("OptDet1"))
                    .SetData(.Rows.Count - 1, vFLOptDet2, rsDummy.Item("OptDet2"))
                    .SetData(.Rows.Count - 1, vFLOptDet3, rsDummy.Item("OptDet3"))
                    .SetData(.Rows.Count - 1, vFLOptDet4, rsDummy.Item("OptDet4"))
                    .SetData(.Rows.Count - 1, vFLOptDet5, rsDummy.Item("OptDet5"))
                    .SetData(.Rows.Count - 1, vFLMenuItemType, rsDummy.Item("MenuItemType"))
                    .SetData(.Rows.Count - 1, vFLMenuindex, vsfDET.Rows.Count - 1)
                    .SetData(.Rows.Count - 1, vFLDocID, rsDummy.Item("DOCID"))
                    .SetData(.Rows.Count - 1, vFLFrmName, rsDummy.Item("FRMName"))
                    .SetData(.Rows.Count - 1, vFLSCName, rsDummy.Item("SCMenuName"))
                    .SetData(.Rows.Count - 1, vFLSCKey, rsDummy.Item("SCKeys"))
                Loop
                rsDummy.Close()
            End With
            LoadFormList = True
        Catch ex As Exception
            MsgBox("Error during save" & vbCrLf & ex.Message, MsgBoxStyle.Critical, Me.Text)
            LoadFormList = False
        End Try
    End Function

    Private Sub vsfFormList_BeforeEdit(sender As Object, e As RowColEventArgs) Handles vsfDET.BeforeEdit
        If e.Col < 0 Then Exit Sub
        If e.Col <> vFLMenuShow And e.Col <> vFLSCName Then
            e.Cancel = True
        End If
        'Select Case vsfDET.Col
        '    Case vFLMenuItemName, vFLMenuGrpName
        '        If (vFLMenuItemName <> String.Empty Or vFLMenuGrpName <> String.Empty) Then
        '            btnDelete.Visible = True
        '        Else
        '            btnDelete.Visible = False
        '        End If
        '    Case Else
        'End Select
    End Sub

    Private Sub LoadBtn_MouseMove(sender As Object, e As MouseEventArgs) Handles BtnLoad.MouseMove
        lblHelp.Text = ""
    End Sub

    Private Sub ExtBtn_MouseMove(sender As Object, e As MouseEventArgs)
        lblHelp.Text = ""
    End Sub

    Private Sub vsfFormList_Click(sender As Object, e As EventArgs) Handles vsfDET.Click
        Dim strSQL As String = ""
        If vsfDET.Row < 0 Then Exit Sub

        If GetVal(Trim(vsfDET.GetData(vsfDET.Row, vFLMenuItemCaption)), "") = "-" Or GetVal(Trim(vsfDET.GetData(vsfDET.Row, vFLMenuItemCaption)), "") = "-" Then
            chkSelKey.Enabled = False
        Else
            chkSelKey.Enabled = True
        End If

        If vsfDET.GetData(vsfDET.Row, vFLMenuItemCaption) = "-" Then
            BtnUpload.Visible = False
            Exit Sub
        Else
            BtnUpload.Visible = True

        End If

        'For Loading Shortcuts
        'strNewHotKey = ""
        'strNewHotKey = Trim(vsfDET.GetData(vsfDET.Row, vFLSCKey))
        'intSplit = InStr(1, strNewHotKey, "+")
        'If Not String.IsNullOrEmpty(strNewHotKey) Then
        '    pnlSCKeys.Enabled = True
        '    cboKey.Text = Strings.Right(strNewHotKey, (Len(strNewHotKey) - intSplit))
        '    chkCtrl.Checked = False
        '    chkShift.Checked = False
        '    chkAlt.Checked = False
        '    Select Case Strings.Left(strNewHotKey, intSplit - 1)
        '        Case "Alt"
        '            chkAlt.Checked = True
        '        Case "Shift"
        '            chkShift.Checked = True
        '        Case "Ctrl"
        '            chkCtrl.Checked = True
        '        Case "Alt+Shift"
        '            chkAlt.Checked = True
        '            chkShift.Checked = True
        '        Case "ctrl+Alt"
        '            chkCtrl.Checked = True
        '            chkAlt.Checked = True
        '        Case "ctrl+Shift"
        '            chkCtrl.Checked = True
        '            chkShift.Checked = True
        '        Case "ctrl+Alt+shift"
        '            chkCtrl.Checked = True
        '            chkAlt.Checked = True
        '            chkShift.Checked = True

        '    End Select
        'Else
        '    pnlSCKeys.Enabled = False
        '    cboKey.Text = ""
        '    chkCtrl.Checked = False
        '    chkShift.Checked = False
        '    chkAlt.Checked = False
        'End If


        Dim strValue As String
        strValue = vsfDET.GetData(vsfDET.Row, vFLSCKey)
        'MessageBox.Show(strValue)
        If strValue <> "" Then
            chkSelKey.Checked = True
            If strValue.Contains("Ctrl") = True Then
                chkCtrl.Checked = True
                strValue = strValue.Replace("Ctrl+", "")
            End If
            If strValue.Contains("Alt") = True Then
                chkAlt.Checked = True
                strValue = strValue.Replace("Alt+", "")
            End If
            If strValue.Contains("Shift") = True Then
                chkAlt.Checked = True
                strValue = strValue.Replace("Shift+", "")
            End If
            cboKey.Text = strValue


        Else
            chkSelKey.Checked = False
            pnlSCKeys.Enabled = False
            cboKey.Text = ""
            chkCtrl.Checked = False
            chkShift.Checked = False
            chkAlt.Checked = False
        End If



        PicBox.Image = Nothing
        Dim cmd As oledb.oledbCommand 
        strSQL = "select " & getImgSyntax("", "MenuImage") & " from MstMenu where MenuItemID =" & vsfDET.GetData(vsfDET.Row, vFLMenuItemID)
        Try
            cmd = New oledb.oledbCommand (strSQL, svtConn)
            Dim imageData As Byte() = DirectCast(cmd.ExecuteScalar(), Byte())
            If imageData IsNot Nothing AndAlso imageData.Length > 0 Then
                Using ms As New MemoryStream(imageData, 0, imageData.Length)
                    ms.Write(imageData, 0, imageData.Length)
                    PicBox.Image = Image.FromStream(ms, True)
                    PicBox.SizeMode = PictureBoxSizeMode.StretchImage
                End Using
                BtnRemove.Visible = True
            Else
                BtnRemove.Visible = False
            End If

        Catch ex As Exception
            MsgBox("Error during loading of image." & vbNewLine & ex.Message, MsgBoxStyle.Critical, "ERROR")
        End Try
    End Sub

    Private Sub vsfFormList_MouseMove(sender As Object, e As MouseEventArgs) Handles vsfDET.MouseMove
        If cboType.Text = "" Then Exit Sub
        lblHelp.Text = "List Of " & cboType.Text & " Menus"
    End Sub

    Private Sub vsfDET_RowColChange(sender As Object, e As EventArgs) Handles vsfDET.RowColChange
        If vsfDET.Row < 0 Then Exit Sub
        If vsfDET.Rows.Count > 1 Then
            If vsfDET.GetData(vsfDET.Row, vFLMenuItemCaption) = "-" Then
                BtnAddSeprator.Text = "&Remove separator"
            Else
                BtnAddSeprator.Text = "&Add separator"
            End If
        End If
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        If cboType.Text = "" Then Exit Sub
        Call ReArrengeMenuIndex()
        If chkDuplicates() Then Exit Sub
        If validSeperator() Then
            If SaveMenuSettings() Then
                MsgBox("Saved menu sequence successfully!!", vbInformation, Me.Text)
                BtnUpload.Enabled = False
                BtnRemove.Enabled = False
                btnSave.Enabled = False
                BtnAddSeprator.Enabled = False
                Call btnClear_Click(btnClear, e)
                chkSelKey.Checked = False
                chkSelKey.Enabled = False
                pnlSCKeys.Enabled = False
                PnlArrow.Visible = False
            Else
                MsgBox("Menu sequence saving FAILED!!", vbInformation, Me.Text)
                btnSave.Enabled = True
                BtnAddSeprator.Enabled = True
            End If
            Call ClearForm()
            FraGrpName.Visible = False
        End If
    End Sub

    Private Function chkDuplicates() As Boolean
        Dim i, j As Int32
        chkDuplicates = False
        With vsfDET
            For i = 1 To .Rows.Count - 1
                If Trim(GetVal(.GetData(i, vFLSCName))) <> "" Then
                    For j = 1 To .Rows.Count - 1
                        If i <> j Then
                            If Trim(GetVal(.GetData(i, vFLSCName))) = Trim(GetVal(.GetData(j, vFLSCName))) Then
                                chkDuplicates = True
                                MsgBox("Duplicate data for Shortcut Menu Name " & Trim(GetVal(.GetData(i, vFLSCName))), MsgBoxStyle.Critical, "Duplicate Data")
                                Exit Function
                            End If
                        End If
                    Next j
                End If
            Next i
        End With
    End Function

    Private Sub btnDesign_Click(sender As Object, e As EventArgs) Handles btnAddGroup.Click
        If vsfDET.Rows.Count = 1 Then
            Exit Sub
        End If
        txtGrpName.Text = ""
        FraGrpName.Visible = True
        cboGrpName.Text = Trim(vsfDET.GetData(vsfDET.Row, vFLMenuGrpName))
    End Sub

    Private Sub frmUtlMenuSequence_KeyUp(sender As Object, e As KeyEventArgs) Handles Me.KeyUp
        If e.KeyCode = Keys.F9 And e.Control = True Then
            Call initFormVsfDet(Me, Me.Text, vsfDET, "DET")
            MsgBox("Restored Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
        If e.KeyCode = Keys.F8 And e.Control = True Then
            Call SaveActFormSettings(Me, Me.Text)
            Call SaveActFormVsfDet(Me, Me.Text, "DET", vsfDET)
            MsgBox("Saved Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
    End Sub

    Private Sub BtnAddSeprator_Click(sender As Object, e As EventArgs) Handles BtnAddSeprator.Click
        If vsfDET.Rows.Count > 2 Then
            If BtnAddSeprator.Text = "&Add separator" Then
                Dim intCurRow As Integer
                With vsfDET
                    If .Rows.Count = .Rows.Count - 1 Then Exit Sub
                    If .GetData(.Row, vFLMenuItemCaption) = "-" Then Exit Sub
                    If .GetData(.Row - 1, vFLMenuItemCaption) = "-" Then Exit Sub
                    If vsfDET.Rows.Count - 1 <> vsfDET.Row Then
                        If .GetData(.Row + 1, vFLMenuItemCaption) = "-" Then Exit Sub
                    End If
                    intCurRow = vsfDET.Row
                    .Rows.Insert(intCurRow)
                    .Row = intCurRow
                    .SetData(vsfDET.Row, vFLMenuItemCaption, "-")
                    '  .SetData(vsfDET.Row, vFLMenuShow, "Y")    'Edited 
                    .GetCellStyle(intCurRow, vFLMenuItemCaption)
                End With
            Else
                If vsfDET.GetData(vsfDET.Row, vFLMenuItemCaption) <> "-" Then Exit Sub
                vsfDET.RemoveItem(vsfDET.Row)
                BtnAddSeprator.Text = "&Add separator"
                Call vsfDET_RowColChange(sender, e)
            End If
            Call ReArrangeSlNo(vsfDET, vFLMenuindex)
        Else
            MsgBox("Yoc cannot add separator for single menu", vbInformation, Me.Text)
        End If
    End Sub

    Private Sub BtnCancel_Click(sender As Object, e As EventArgs) Handles BtnCancel.Click
        FraGrpName.Visible = False
    End Sub

    Private Sub BtnOk_Click(sender As Object, e As EventArgs) Handles BtnOk.Click
        stringReplace(Me)
        If Trim(cboGrpName.Text) = "" Then
            If MsgBox("Please Enter Group Name" & vbCrLf & "Do you want to continue without Group?", vbInformation + vbYesNo, Me.Text) = vbNo Then
                cboGrpName.Focus()
                Exit Sub
            End If
        End If
        vsfDET.SetData(vsfDET.Row, vFLMenuGrpName, cboGrpName.Text)
        FraGrpName.Visible = False
    End Sub
    ' Edited code
    Private Sub btnApply_Click(sender As Object, e As EventArgs) Handles btnApply.Click
        If Trim(cboKey.Text) = "" Then
            MsgBox("Please select a key!", vbInformation, Me.Text)
            Exit Sub
        End If

        If chkAlt.Checked = False AndAlso chkCtrl.Checked = False AndAlso chkShift.Checked = False AndAlso cboKey.Text = "" Then
            MsgBox("Please select atleast one option!", vbInformation, Me.Text)
        End If
        strNewHotKey = ""

        If chkCtrl.Checked And chkAlt.Checked And chkShift.Checked Then
            strNewHotKey &= "Ctrl+Alt+shift+"
        ElseIf chkCtrl.Checked And chkAlt.Checked Then
            strNewHotKey &= "Ctrl+Alt+"
        ElseIf chkCtrl.Checked And chkShift.Checked Then
            strNewHotKey &= "Ctrl+Shift+"
        ElseIf chkAlt.Checked And chkShift.Checked Then
            strNewHotKey &= "Alt+Shift+"
        ElseIf chkCtrl.Checked Then
            strNewHotKey &= "Ctrl+"
        ElseIf chkAlt.Checked Then
            strNewHotKey &= "Alt+"
        ElseIf chkShift.Checked Then
            strNewHotKey &= "Shift+"
        End If
        strNewHotKey &= cboKey.Text

        'For Remove Duplicate Shortcuts
        Dim iCurrentRow As Integer = vsfDET.Row
        For i = 1 To vsfDET.Rows.Count - 1
            If i <> iCurrentRow Then
                If Trim(vsfDET.GetData(i, vFLSCKey)) = strNewHotKey Then
                    MsgBox("Already exist for " & Trim(vsfDET.GetData(i, vFLMenuItemCaption)), vbInformation, Me.Text)
                    Exit Sub
                End If
            End If
        Next
        vsfDET.SetData(iCurrentRow, vFLSCKey, strNewHotKey)
        chkSelKey.Checked = False
        pnlSCKeys.Enabled = False
        Call btnClear_Click(btnClear, e)
        btnApply.Visible = False
        btnClear.Visible = False
    End Sub

    Private Sub BtnAddSeprator_MouseHover(sender As Object, e As EventArgs) Handles BtnAddSeprator.MouseHover
        If BtnAddSeprator.Text = "&Add separator" Then
            lblHelp.Text = "Adds separator BELOW the selected menu"
        Else
            lblHelp.Text = "Removes selected separator"
        End If
    End Sub

    Private Sub btnClear_Click(sender As Object, e As EventArgs) Handles btnClear.Click
        chkCtrl.Checked = False
        chkAlt.Checked = False
        chkShift.Checked = False
        cboKey.SelectedIndex = -1
    End Sub

    Private Sub btnSave_MouseHover(sender As Object, e As EventArgs) Handles btnSave.MouseHover
        lblHelp.Text = "Save the menu sequence"
    End Sub

    Private Sub chkSelKey_CheckedChanged(sender As Object, e As EventArgs) Handles chkSelKey.CheckedChanged
        If chkSelKey.Checked = True Then
            pnlSCKeys.Enabled = True
        Else
            pnlSCKeys.Enabled = False
            chkCtrl.Checked = False
            chkAlt.Checked = False
            chkShift.Checked = False
            btnClear.Visible = False
            btnApply.Visible = False
            cboKey.Text = ""


        End If
    End Sub

    Private Sub chkCtrl_CheckedChanged(sender As Object, e As EventArgs) Handles chkCtrl.CheckedChanged
        If chkCtrl.Checked = True Then
            btnClear.Visible = True
            btnApply.Visible = True
            '  chkShift.Checked = False
            '  chkAlt.Checked = False
        ElseIf chkAlt.Checked = False AndAlso chkShift.Checked = False Then
            ' If Not cboKey.Text = ("F1|F2|F3|F4|F5|F6|F7|F8|F9|F10|F11|F12") Then
            btnClear.Visible = False
            btnApply.Visible = False
        End If
    End Sub

    Private Sub btnMvUP_Click(sender As Object, e As EventArgs) Handles btnMvUP.Click
        'Grid Rows move to Up
        If vsfDET.Row > 1 Then
            Call MoveOneRowUp(vsfDET)

            Call ReArrangeSlNo(vsfDET, vFLMenuindex)
        End If
    End Sub

    Private Sub btnMvDwn_Click(sender As Object, e As EventArgs) Handles btnMvDwn.Click
        'Grid Rows move to Down
        If vsfDET.Row < vsfDET.Rows.Count - 1 Then
            Call MoveOneRowDown(vsfDET)
            Call ReArrangeSlNo(vsfDET, vFLMenuindex)
        End If
    End Sub

    Private Sub btnRmv_Click(sender As Object, e As EventArgs) Handles BtnRemove.Click
        Dim strSQL As String
        Try
            If MsgBox("Are you sure you want to remove the user's image?", vbQuestion + vbYesNo + vbDefaultButton2, Me.Text) = MsgBoxResult.Yes Then
                PicBox.Image = Nothing
                strSQL = "Update MstMenu set  MenuImage = Null where MenuItemID='" & vsfDET.GetData(vsfDET.Row, vFLMenuItemID) & "' "
                clsSVT.execSQLStatement(strSQL)
                Exit Sub
            End If
        Catch ex As Exception
            MsgBox("Error while update images!", vbCritical, Me.Text)
        End Try
    End Sub

    Private Sub chkAlt_CheckedChanged(sender As Object, e As EventArgs) Handles chkAlt.CheckedChanged
        'If chkSelKey.Checked = False Then
        '    btnClear.Visible = True
        '    btnApply.Visible = True

        'End If

        If chkAlt.Checked = True Then
            btnClear.Visible = True
            btnApply.Visible = True
            ' chkShift.Checked = False
            ' chkCtrl.Checked = False
        ElseIf chkShift.Checked = False AndAlso chkCtrl.Checked = False Then
            btnClear.Visible = False
            btnApply.Visible = False
        End If
    End Sub



    'Private Sub btnRemoveRow_Click(sender As Object, e As EventArgs)
    '    If vsfDET.Rows.Count <= 1 Then
    '        Exit Sub
    '    End If

    '    If MsgBox("Are you sure you want to Remove ?", MsgBoxStyle.YesNo + vbDefaultButton2 + MsgBoxStyle.Critical, "Deletion Confirmation...") = vbNo Then Exit Sub
    '    If Trim(vsfDET.Rows.Count > 1) And Trim(vsfDET.GetData(vsfDET.Row, "")) <> "" Then
    '        vsfDET.Rows.Remove(vsfDET.Row)
    '        If vsfDET.Rows.Count = 1 Then vsfDET.Rows.Count = 2
    '    End If
    'End Sub

    Private Sub chkShift_CheckedChanged(sender As Object, e As EventArgs) Handles chkShift.CheckedChanged
        If chkShift.Checked = True Then
            btnClear.Visible = True
            btnApply.Visible = True
            'chkAlt.Checked = False
            'chkCtrl.Checked = False
        ElseIf chkCtrl.Checked = False AndAlso chkAlt.Checked = False Then
            btnClear.Visible = False
            btnApply.Visible = False
        End If
    End Sub

    'Extra code
    Private Sub vsfDET_KeyUp(sender As Object, e As KeyEventArgs) Handles vsfDET.KeyUp
        If vsfDET.Col = vFLMenuGrpName AndAlso vsfDET.GetData(vsfDET.Row, vFLMenuGrpName) <> "" AndAlso e.KeyCode = Keys.Delete Then
            ' strSql = "Delete from MstMenu where MenuItemCaption='" & vsfDET.GetData(vsfDET.Row, vFLMenuItemCaption) & "'"
            'clsSVT.execSQLStatement(strSql)
            vsfDET.SetData(vsfDET.Row, vFLMenuGrpName, "")
        End If
    End Sub


    Private Sub cboKey_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboKey.SelectedIndexChanged
        If chkAlt.Checked = False AndAlso chkCtrl.Checked = False AndAlso chkShift.Checked = False Then
            If cboKey.SelectedIndex > 36 Then
                btnApply.Visible = True
                btnClear.Visible = True
            Else
                btnApply.Visible = False
                btnClear.Visible = False
            End If
        End If
    End Sub

    Private Sub vsfDET_DoubleClick(sender As Object, e As EventArgs) Handles vsfDET.DoubleClick
        If Trim(vsfDET.GetData(vsfDET.Row, vFLMenuGrpName)) = String.Empty Then
            btnAddGroup.Text = "&Add Group"
            lblAddGroup.Text = "&Add Group"
        Else
            btnAddGroup.Text = "&Edit Group"
            lblAddGroup.Text = "&Edit Group"
        End If

        With vsfDET
            Select Case .Col
                Case vFLMenuGrpName
                    FraGrpName.Visible = True
                Case Else
                    FraGrpName.Visible = False
            End Select
        End With

        'FraGrpName.Visible = True
        cboGrpName.Text = GetVal(vsfDET.GetData(vsfDET.Row, vFLMenuGrpName), "")

        'Call bindCombo(cboGrpName, "MenuGroupName", "MenuGroupName", "Select distinct MenuGroupName from MstMenu where MenuItemType = " & clsSVT.getDataLongValue("Select MenuItemType from MstMenu where MenuGroupName = '" & Trim(vsfDET.GetData(vsfDET.Row, vFLMenuGrpName)) & "'", 0, False))
        ''''Changed
        If Trim(vsfDET.GetData(vsfDET.Row, vFLMenuItemCaption)) = "-" Then
            Call bindCombo(cboGrpName, "MenuGroupName", "MenuGroupName", "Select distinct MenuGroupName from MstMenu where MenuItemType='" & intMenuType & "'")
            Exit sub
        End If
        ''''Finished on 4FEB2019

        Call bindCombo(cboGrpName, "MenuGroupName", "MenuGroupName", "Select distinct MenuGroupName from MstMenu where MenuItemType = " & clsSVT.getDataLongValue("Select MenuItemType from MstMenu where MenuItemCaption = '" & Trim(vsfDET.GetData(vsfDET.Row, vFLMenuItemCaption)) & "'", 0, False))
    End Sub

    Private Sub btnRemoveRow_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        Dim intDocID As Int32
        Dim strSQL As String = ""

        If vsfDET.Rows.Count <= 1 Then Exit Sub
        If vsfDET.Row < 1 Then Exit Sub
        If cboType.Text = "" Then Exit Sub

        intDocID = clsSVT.getDataLongValue("Select DocID from MstMenu where MenuItemCaption = '" & Trim(vsfDET.GetData(vsfDET.Row, vFLMenuItemCaption)) & "'", 0, False)
        If intDocID > 0 Then
            MsgBox("Document Cannot be Deleted because it is Created from the Doc-Designer..", vbInformation, Me.Text)
            Exit Sub
        Else
            If MsgBox("Are you sure you want to Delete ?", MsgBoxStyle.YesNo + vbDefaultButton2 + MsgBoxStyle.Critical, "Deletion Confirmation...") = vbNo Then Exit Sub
            Try
                strSQL = "Delete from MstMenu Where MenuItemCaption = '" & Trim(vsfDET.GetData(vsfDET.Row, vFLMenuItemCaption)) & "' and DocID = " & intDocID
                clsSVT.execSQLStatement(strSQL)
                MsgBox("Deleted Successfully..", vbInformation, Me.Text)

            Catch ex As Exception
                MsgBox("Error during Deletion" & vbCrLf & ex.Message, MsgBoxStyle.Critical, Me.Text)
            End Try
            'vsfDET.Rows.Remove(vsfDET.Row)
            'If vsfDET.Rows.Count = 1 Then vsfDET.Rows.Count = 2
        End If
    End Sub


    Private Sub cboGrpName_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cboGrpName.KeyPress
        If Asc(e.KeyChar) <> 32 Then
            If Not KeyValidate("NonSpclChar", e.KeyChar) Then
                e.Handled = True
            End If
        End If
    End Sub

    Private Sub frmUtlMenuSequence_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing

    End Sub
End Class