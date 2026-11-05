Imports C1.Win.C1FlexGrid

Public Class frmUtlSettings
    Const vDetSel = 0
    Const vDetFldCode = 1
    Dim clsSVT As New svtLibrary
    Public strDocName As String
    Public strTblName As String
    Public strMainFld As String
    Public blnSRCHFrom As Boolean
    Public dblTotCol As Long
    Public argForm As Form
    Dim intchkClr As Integer
    Dim chkPrnt As Integer
    Dim btnCtrlList = GetAllCtrls(Me, GetType(Button))
    Dim lblCtrlList = GetAllCtrls(Me, GetType(LinkLabel))
    Dim txtCtrlList = GetAllCtrls(Me, GetType(TextBox))
    Dim rdoCtrlList = GetAllCtrls(Me, GetType(RadioButton))

    Private Sub frmUtlSettings_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        svtBtnCtrl.btnNew.Visible = True
        svtBtnCtrl.btnCancel.Visible = True
        svtBtnCtrl.btnSave.Visible = True
        rdoBtnColor.Checked = True
        rdoDefaultTheme.Checked = True
        Call fillDocDetails()
        initGridStyles(Me)
    End Sub

    Public Sub fillDocDetails()
        Dim strSql As String = ""
        Dim rs As oledb.oledbDataReader = Nothing

        Dim strClrOrTheme As String = getGsettings("BtnCtrl", "ClrOrTheme", "C")

        If UCase(Trim(strClrOrTheme)) = "C" Then
            rdoBtnColor.Checked = True

            If Val(gBtnCtrlBackClr) <> 0 Then
                svtBtnCtrl.btnNew.BackColor = Color.FromArgb(gBtnCtrlBackClr)
                svtBtnCtrl.btnEdit.BackColor = Color.FromArgb(gBtnCtrlBackClr)
                svtBtnCtrl.btnDelete.BackColor = Color.FromArgb(gBtnCtrlBackClr)
                svtBtnCtrl.btnClose.BackColor = Color.FromArgb(gBtnCtrlBackClr)
                svtBtnCtrl.btnPrint.BackColor = Color.FromArgb(gBtnCtrlBackClr)
                svtBtnCtrl.btnOptions.BackColor = Color.FromArgb(gBtnCtrlBackClr)
                svtBtnCtrl.btnMovePrev.BackColor = Color.FromArgb(gBtnCtrlBackClr)
                svtBtnCtrl.btnMoveFirst.BackColor = Color.FromArgb(gBtnCtrlBackClr)
                svtBtnCtrl.btnFind.BackColor = Color.FromArgb(gBtnCtrlBackClr)
                svtBtnCtrl.btnMoveNext.BackColor = Color.FromArgb(gBtnCtrlBackClr)
                svtBtnCtrl.btnMoveLast.BackColor = Color.FromArgb(gBtnCtrlBackClr)
                svtBtnCtrl.btnSave.BackColor = Color.FromArgb(gBtnCtrlBackClr)
                svtBtnCtrl.btnCancel.BackColor = Color.FromArgb(gBtnCtrlBackClr)
            End If

            If Val(gBtnCtrlForeClr) <> 0 Then
                svtBtnCtrl.btnNew.ForeColor = Color.FromArgb(gBtnCtrlForeClr)
                svtBtnCtrl.btnEdit.ForeColor = Color.FromArgb(gBtnCtrlForeClr)
                svtBtnCtrl.btnDelete.ForeColor = Color.FromArgb(gBtnCtrlForeClr)
                svtBtnCtrl.btnClose.ForeColor = Color.FromArgb(gBtnCtrlForeClr)
                svtBtnCtrl.btnPrint.ForeColor = Color.FromArgb(gBtnCtrlForeClr)
                svtBtnCtrl.btnOptions.ForeColor = Color.FromArgb(gBtnCtrlForeClr)
                svtBtnCtrl.btnMovePrev.ForeColor = Color.FromArgb(gBtnCtrlForeClr)
                svtBtnCtrl.btnMoveFirst.ForeColor = Color.FromArgb(gBtnCtrlForeClr)
                svtBtnCtrl.btnFind.ForeColor = Color.FromArgb(gBtnCtrlForeClr)
                svtBtnCtrl.btnMoveNext.ForeColor = Color.FromArgb(gBtnCtrlForeClr)
                svtBtnCtrl.btnMoveLast.ForeColor = Color.FromArgb(gBtnCtrlForeClr)
                svtBtnCtrl.btnSave.ForeColor = Color.FromArgb(gBtnCtrlForeClr)
                svtBtnCtrl.btnCancel.ForeColor = Color.FromArgb(gBtnCtrlForeClr)
            End If
        Else
            rdoBtnTheme.Checked = True
        End If

        

        Dim strTempHotkeys As String = ""

        strTempHotkeys = clsSVT.getDataStringValue("Select SetValue from UtlGSettings where SetGroup = 'BtnCtrl' and SetName = 'Hotkeys'", "", False)

        If Trim(strTempHotkeys) <> "" Then

            Dim dictionary As New Dictionary(Of String, String)
            Dim items() As String = strTempHotkeys.TrimEnd("ß").Split("ß")
            For Each item As String In items
                Dim keyValue() As String = item.Split("||")
                dictionary.Add(keyValue(0), keyValue(2))
            Next

            Dim ctrlList = GetAllBtnCtrls(Me, GetType(Button))

            For Each cc As Control In ctrlList
                Select Case cc.Tag
                    Case "New"
                        If dictionary.ContainsKey("New") Then
                            tt.SetToolTip(cc, "Shortcut: " & dictionary("New"))
                        End If
                        Exit Select
                    Case "Edit"
                        If dictionary.ContainsKey("Edit") Then
                            tt.SetToolTip(cc, "Shortcut: " & dictionary("Edit"))
                        End If
                        Exit Select
                    Case "Delete"
                        If dictionary.ContainsKey("Delete") Then
                            tt.SetToolTip(cc, "Shortcut: " & dictionary("Delete"))
                        End If
                        Exit Select
                    Case "Close"
                        If dictionary.ContainsKey("Close") Then
                            tt.SetToolTip(cc, "Shortcut: " & dictionary("Close"))
                        End If
                        Exit Select
                    Case "Print"
                        If dictionary.ContainsKey("Print") Then
                            tt.SetToolTip(cc, "Shortcut: " & dictionary("Print"))
                        End If
                        Exit Select
                    Case "Options"
                        If dictionary.ContainsKey("Options") Then
                            tt.SetToolTip(cc, "Shortcut: " & dictionary("Options"))
                        End If
                        Exit Select
                    Case "MovePrev"
                        If dictionary.ContainsKey("MovePrev") Then
                            tt.SetToolTip(cc, "Shortcut: " & dictionary("MovePrev"))
                        End If
                        Exit Select
                    Case "MoveFirst"
                        If dictionary.ContainsKey("MoveFirst") Then
                            tt.SetToolTip(cc, "Shortcut: " & dictionary("MoveFirst"))
                        End If
                        Exit Select
                    Case "Find"
                        If dictionary.ContainsKey("Find") Then
                            tt.SetToolTip(cc, "Shortcut: " & dictionary("Find"))
                        End If
                        Exit Select
                    Case "MoveNext"
                        If dictionary.ContainsKey("MoveNext") Then
                            tt.SetToolTip(cc, "Shortcut: " & dictionary("MoveNext"))
                        End If
                        Exit Select
                    Case "MoveLast"
                        If dictionary.ContainsKey("MoveLast") Then
                            tt.SetToolTip(cc, "Shortcut: " & dictionary("MoveLast"))
                        End If
                        Exit Select
                    Case "Save"
                        If dictionary.ContainsKey("Save") Then
                            '   tt.SetToolTip(cc, "Shortcut: " & dictionary("Save"))
                        End If
                        Exit Select
                    Case "Cancel"
                        If dictionary.ContainsKey("Cancel") Then
                            '  tt.SetToolTip(cc, "Shortcut: " & dictionary("Cancel"))
                        End If
                        Exit Select
                End Select
            Next
        End If

        fillCaptions()
    End Sub

    Public Sub FillFlds(ByVal argDocName As String, ByVal argTblName As String, ByVal argMainFld As String)
        Dim i As Integer
        Dim rs As oledb.oledbDataReader
        Dim strSQL As String
        strDocName = argDocName
        strTblName = argTblName

        strSQL = "Select * from " & argTblName & " where 1 <> 1"
        rs = clsSVT.getDataReaderFromSQL(strSQL)
        intchkClr = rs.FieldCount
        For i = 0 To rs.FieldCount - 1
            If UCase(argMainFld) <> UCase(rs.GetName(i)) Then
                If DataTypeIsString(rs.GetFieldType(i)) Then
                    If UCase(rs.GetName(i)) <> "IMGPRINT" Then
                        vsfDet.Rows.Add()
                        vsfDet.SetData(vsfDet.Rows.Count - 1, vDetFldCode, rs.GetName(i))
                    End If
                End If
            End If
        Next

        strSQL = "Select * from UtlSearchSettings where docName = '" & strDocName & "'"
        rs = clsSVT.getDataReaderFromSQL(strSQL)
        Do While rs.Read
            For i = 0 To vsfDet.Rows.Count - 1
                If UCase(vsfDet.GetData(i, vDetFldCode)) = UCase(rs("SearchFld")) Then
                    vsfDet.SetCellCheck(i, vDetSel, C1.Win.C1FlexGrid.CheckEnum.Checked)
                    Exit For
                End If
            Next
        Loop

    End Sub

    Public Sub FillSrchFormFlds(ByVal argDocName As String, ByVal argTblName As String, ByVal argMainFld As String)
        Dim i As Integer
        Dim rs As oledb.oledbDataReader
        Dim strSQL As String, strColName As String
        strDocName = argDocName
        strTblName = argTblName

        Dim ctrlList As New List(Of Control)
        Dim ctrl As Control = Nothing

        Do
            ctrl = argForm.GetNextControl(ctrl, True)

            If ctrl Is Nothing Then
                Exit Do
            End If
            If ctrl.GetType = GetType(C1FlexGrid) Then
                If ctrl.Tag = "View" Then ctrlList.Add(ctrl)
            End If
        Loop Until ctrl Is Nothing

        For Each argVsf As C1FlexGrid In ctrlList
            With argVsf
                For i = 0 To dblTotCol - 1
                    vsfDet.Rows.Add()
                    strColName = .GetData(3, i)
                    vsfDet.SetData(vsfDet.Rows.Count - 1, vDetFldCode, .GetData(3, i))
                Next

                strSQL = "Select * from UtlSearchSettings where docName = '" & strDocName & "'"
                rs = clsSVT.getDataReaderFromSQL(strSQL)
                Do While rs.Read
                    For i = 0 To vsfDet.Rows.Count - 1
                        If UCase(vsfDet.GetData(i, vDetFldCode)) = UCase(rs("SearchFld")) Then
                            vsfDet.SetCellCheck(i, vDetSel, C1.Win.C1FlexGrid.CheckEnum.Checked)
                            Exit For
                        End If
                    Next
                Loop
            End With
        Next
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSettClose.Click
        blnSRCHFrom = False
        Me.Close()

    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSettSave.Click
        Dim strTempForeClr As Long = 0
        Dim strTempBackClr As Long = 0
        Dim strTempTheme As String = ""
        Dim strTempClrTheme As String = ""

        If rdoBtnColor.Checked Then
            'Save ForeColor to DB
            strTempForeClr = clsSVT.getDataLongValue("Select SetValue from UtlGSettings where SetName= 'ForeClr' and SetGroup = 'BtnCtrl'", 0, False)
            If Val(strTempForeClr) = 0 Then
                clsSVT.execSQLStatement("Delete from UtlGSettings where SetName= 'ForeClr' and SetGroup = 'BtnCtrl'")
                clsSVT.execSQLStatement("Insert into UtlGSettings (SetGroup, SetName, SetValue) values ('BtnCtrl', 'ForeClr', '" & svtBtnCtrl.btnNew.ForeColor.ToArgb & "')")
            Else
                Call clsSVT.execSQLStatement("Update UtlGSettings set SetValue = '" & svtBtnCtrl.btnNew.ForeColor.ToArgb & "' where SetName= 'ForeClr' and SetGroup = 'BtnCtrl'")
            End If

            'Save BackColor to DB
            strTempBackClr = clsSVT.getDataLongValue("Select SetValue from UtlGSettings where SetName= 'BackClr' and SetGroup = 'BtnCtrl'", 0, False)
            If Val(strTempBackClr) = 0 Then
                clsSVT.execSQLStatement("Delete from UtlGSettings where SetName= 'BackClr' and SetGroup = 'BtnCtrl'")
                clsSVT.execSQLStatement("Insert into UtlGSettings (SetGroup, SetName, SetValue) values ('BtnCtrl', 'BackClr', '" & svtBtnCtrl.btnNew.BackColor.ToArgb & "')")
            Else
                Call clsSVT.execSQLStatement("Update UtlGSettings set SetValue = '" & svtBtnCtrl.btnNew.BackColor.ToArgb & "' where SetName= 'BackClr' and SetGroup = 'BtnCtrl'")
            End If
        End If

        If rdoBtnTheme.Checked Then
            'Save ForeColor to DB
            strTempForeClr = clsSVT.getDataLongValue("Select SetValue from UtlGSettings where SetName= 'ForeClr' and SetGroup = 'BtnCtrl'", 0, False)
            If Val(strTempForeClr) = 0 Then
                clsSVT.execSQLStatement("Delete from UtlGSettings where SetName= 'ForeClr' and SetGroup = 'BtnCtrl'")
                clsSVT.execSQLStatement("Insert into UtlGSettings (SetGroup, SetName, SetValue) values ('BtnCtrl', 'ForeClr', '" & svtBtnCtrl.btnNew.ForeColor.ToArgb & "')")
            Else
                Call clsSVT.execSQLStatement("Update UtlGSettings set SetValue = '" & svtBtnCtrl.btnNew.ForeColor.ToArgb & "' where SetName= 'ForeClr' and SetGroup = 'BtnCtrl'")
            End If

            'Save Button Theme to DB
            strTempTheme = clsSVT.getDataStringValue("Select SetValue from UtlGSettings where SetName= 'BtnTheme' and SetGroup = 'BtnCtrl'", "", False)
            If Trim(strTempTheme) = "" Then
                clsSVT.execSQLStatement("Delete from UtlGSettings where SetName= 'BtnTheme' and SetGroup = 'BtnCtrl'")
                clsSVT.execSQLStatement("Insert into UtlGSettings (SetGroup, SetName, SetValue) values ('BtnCtrl', 'BtnTheme', '" & Trim(txtTheme.Text) & "')")
            Else
                Call clsSVT.execSQLStatement("Update UtlGSettings set SetValue = '" & Trim(txtTheme.Text) & "' where SetName= 'BtnTheme' and SetGroup = 'BtnCtrl'")
            End If
        End If

        strTempClrTheme = clsSVT.getDataStringValue("Select SetValue from UtlGSettings where SetName= 'ClrOrTheme' and SetGroup = 'BtnCtrl'", "", False)
        If Trim(strTempClrTheme) = "" Then
            If rdoBtnTheme.Checked Then
                strTempClrTheme = "T"
            Else
                strTempClrTheme = "C"
            End If
            clsSVT.execSQLStatement("Delete from UtlGSettings where SetName= 'ClrOrTheme' and SetGroup = 'BtnCtrl'")
            clsSVT.execSQLStatement("Insert into UtlGSettings (SetGroup, SetName, SetValue) values ('BtnCtrl', 'ClrOrTheme', '" & strTempClrTheme & "')")
        Else
            If rdoBtnTheme.Checked Then
                strTempClrTheme = "T"
            Else
                strTempClrTheme = "C"
            End If
            Call clsSVT.execSQLStatement("Update UtlGSettings set SetValue = '" & strTempClrTheme & "' where SetName= 'ClrOrTheme' and SetGroup = 'BtnCtrl'")
        End If

        MsgBox("Saved settings successfully! Please restart application for changes to take place!", MsgBoxStyle.Information, Me.Text)
        Me.Close()
    End Sub

    Private Sub chkSel()
        For i = 0 To vsfDet.Rows.Count - 1
            If vsfDet.GetCellCheck(i, vDetSel) = CheckEnum.Checked Then
                chkPrnt += 1
                'chkSelClr.Checked = True
            End If
        Next
    End Sub
    Private Sub vsfDet_BeforeEdit(sender As Object, e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfDet.BeforeEdit
        Select Case e.Col
            Case vDetSel
                e.Cancel = False

            Case vDetFldCode
                e.Cancel = True
        End Select
    End Sub

    Private Sub lblChangeForeClr_LinkClicked(sender As System.Object, e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles lblChangeForeClr.LinkClicked, lblForeColor2.LinkClicked
        If cdg.ShowDialog <> Windows.Forms.DialogResult.Cancel Then
            gBtnCtrlForeClr = cdg.Color.ToArgb

            svtBtnCtrl.btnNew.ForeColor = cdg.Color
            svtBtnCtrl.btnEdit.ForeColor = cdg.Color
            svtBtnCtrl.btnDelete.ForeColor = cdg.Color
            svtBtnCtrl.btnClose.ForeColor = cdg.Color
            svtBtnCtrl.btnPrint.ForeColor = cdg.Color
            svtBtnCtrl.btnOptions.ForeColor = cdg.Color
            svtBtnCtrl.btnMovePrev.ForeColor = cdg.Color
            svtBtnCtrl.btnMoveFirst.ForeColor = cdg.Color
            svtBtnCtrl.btnFind.ForeColor = cdg.Color
            svtBtnCtrl.btnMoveNext.ForeColor = cdg.Color
            svtBtnCtrl.btnMoveLast.ForeColor = cdg.Color
            svtBtnCtrl.btnSave.ForeColor = cdg.Color
            svtBtnCtrl.btnCancel.ForeColor = cdg.Color

            'Call clsSVT.execSQLStatement("Update UtlGSettings set SetValue = '" & cdg.Color.ToArgb & "' where SetName= 'ForeClr' and SetGroup = 'BtnCtrl'")
        End If
    End Sub

    Private Sub lblChangeBackClr_LinkClicked(sender As System.Object, e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles lblChangeBackClr.LinkClicked
        If cdg.ShowDialog <> Windows.Forms.DialogResult.Cancel Then
            gBtnCtrlBackClr = cdg.Color.ToArgb

            svtBtnCtrl.btnNew.BackColor = cdg.Color
            svtBtnCtrl.btnEdit.BackColor = cdg.Color
            svtBtnCtrl.btnDelete.BackColor = cdg.Color
            svtBtnCtrl.btnClose.BackColor = cdg.Color
            svtBtnCtrl.btnPrint.BackColor = cdg.Color
            svtBtnCtrl.btnOptions.BackColor = cdg.Color
            svtBtnCtrl.btnMovePrev.BackColor = cdg.Color
            svtBtnCtrl.btnMoveFirst.BackColor = cdg.Color
            svtBtnCtrl.btnFind.BackColor = cdg.Color
            svtBtnCtrl.btnMoveNext.BackColor = cdg.Color
            svtBtnCtrl.btnMoveLast.BackColor = cdg.Color
            svtBtnCtrl.btnSave.BackColor = cdg.Color
            svtBtnCtrl.btnCancel.BackColor = cdg.Color

            'Call clsSVT.execSQLStatement("Update UtlGSettings set SetValue = '" & cdg.Color.ToArgb & "' where SetName= 'ForeClr' and SetGroup = 'BtnCtrl'")
        End If
    End Sub

    Private Sub lblChangeHotKeys_LinkClicked(sender As System.Object, e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles lblChangeHotKeys.LinkClicked, lblHotKeys2.LinkClicked
        Dim frmUtlHotkeys As New frmUtlBtnHotkeys
        With frmUtlHotkeys
            .StartPosition = FormStartPosition.CenterScreen
            .ShowDialog()
            fillDocDetails()
        End With
    End Sub

    Public Function GetAllBtnCtrls(control As Control, type As Type) As IEnumerable(Of Control)
        Dim controls = control.Controls.Cast(Of Control)()

        Return controls.SelectMany(Function(ctrl) GetAllBtnCtrls(ctrl, type)).Concat(controls).Where(Function(c) c.[GetType]() = type And CStr(c.Tag) <> "")
    End Function

    Private Sub rdoBtnColor_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles rdoBtnColor.CheckedChanged
        If rdoBtnColor.Checked Then
            pnlLinks.Visible = True
            pnlLinksThemes.Visible = False

            fillDefaults()

            If gBtnCtrlBackClr <> 0 Then
                svtBtnCtrl.btnNew.BackColor = Color.FromArgb(gBtnCtrlBackClr)
                svtBtnCtrl.btnEdit.BackColor = Color.FromArgb(gBtnCtrlBackClr)
                svtBtnCtrl.btnDelete.BackColor = Color.FromArgb(gBtnCtrlBackClr)
                svtBtnCtrl.btnClose.BackColor = Color.FromArgb(gBtnCtrlBackClr)
                svtBtnCtrl.btnPrint.BackColor = Color.FromArgb(gBtnCtrlBackClr)
                svtBtnCtrl.btnOptions.BackColor = Color.FromArgb(gBtnCtrlBackClr)
                svtBtnCtrl.btnMovePrev.BackColor = Color.FromArgb(gBtnCtrlBackClr)
                svtBtnCtrl.btnMoveFirst.BackColor = Color.FromArgb(gBtnCtrlBackClr)
                svtBtnCtrl.btnFind.BackColor = Color.FromArgb(gBtnCtrlBackClr)
                svtBtnCtrl.btnMoveNext.BackColor = Color.FromArgb(gBtnCtrlBackClr)
                svtBtnCtrl.btnMoveLast.BackColor = Color.FromArgb(gBtnCtrlBackClr)
                svtBtnCtrl.btnSave.BackColor = Color.FromArgb(gBtnCtrlBackClr)
                svtBtnCtrl.btnCancel.BackColor = Color.FromArgb(gBtnCtrlBackClr)
            End If
            If gBtnCtrlForeClr <> 0 Then
                svtBtnCtrl.btnNew.ForeColor = Color.FromArgb(gBtnCtrlForeClr)
                svtBtnCtrl.btnEdit.ForeColor = Color.FromArgb(gBtnCtrlForeClr)
                svtBtnCtrl.btnDelete.ForeColor = Color.FromArgb(gBtnCtrlForeClr)
                svtBtnCtrl.btnClose.ForeColor = Color.FromArgb(gBtnCtrlForeClr)
                svtBtnCtrl.btnPrint.ForeColor = Color.FromArgb(gBtnCtrlForeClr)
                svtBtnCtrl.btnOptions.ForeColor = Color.FromArgb(gBtnCtrlForeClr)
                svtBtnCtrl.btnMovePrev.ForeColor = Color.FromArgb(gBtnCtrlForeClr)
                svtBtnCtrl.btnMoveFirst.ForeColor = Color.FromArgb(gBtnCtrlForeClr)
                svtBtnCtrl.btnFind.ForeColor = Color.FromArgb(gBtnCtrlForeClr)
                svtBtnCtrl.btnMoveNext.ForeColor = Color.FromArgb(gBtnCtrlForeClr)
                svtBtnCtrl.btnMoveLast.ForeColor = Color.FromArgb(gBtnCtrlForeClr)
                svtBtnCtrl.btnSave.ForeColor = Color.FromArgb(gBtnCtrlForeClr)
                svtBtnCtrl.btnCancel.ForeColor = Color.FromArgb(gBtnCtrlForeClr)
            End If

        End If
    End Sub

    Private Sub rdoBtnTheme_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles rdoBtnTheme.CheckedChanged
        Dim strTempTheme As String = ""
        If rdoBtnTheme.Checked Then
            pnlLinks.Visible = False
            pnlLinksThemes.Visible = True
            strTempTheme = getGsettings("BtnCtrl", "BtnTheme", "D")
            Select Case strTempTheme
                Case "D"
                    rdoDefaultTheme.Checked = True
                    fillDefaults()
                Case "T1"
                    rdoTheme1.Checked = True
                Case "T2"
                    rdoTheme2.Checked = True
                Case "T3"
                    rdoTheme3.Checked = True
                Case "T4"
                    rdoTheme4.Checked = True
                Case "T5"
                    rdoTheme5.Checked = True
            End Select
        End If
    End Sub

    Private Sub rdoTheme1_CheckedChanged(sender As Object, e As System.EventArgs) Handles rdoDefaultTheme.CheckedChanged, rdoTheme1.CheckedChanged, rdoTheme2.CheckedChanged, rdoTheme3.CheckedChanged, rdoTheme4.CheckedChanged, rdoTheme5.CheckedChanged

        For Each c In btnCtrlList
            c.BackColor = Color.Transparent
            c.BackgroundImage = Nothing
            c.Image = Nothing
            svtBtnCtrl.btnOptions.BackColor = Color.Transparent
            svtBtnCtrl.btnOptions.BackgroundImage = Nothing
            svtBtnCtrl.btnOptions.Image = Nothing
        Next

        If sender.Checked = True Then
            txtTheme.Text = sender.tag

            If Val(gBtnCtrlForeClr) <> 0 Then
                svtBtnCtrl.btnNew.ForeColor = Color.FromArgb(gBtnCtrlForeClr)
                svtBtnCtrl.btnEdit.ForeColor = Color.FromArgb(gBtnCtrlForeClr)
                svtBtnCtrl.btnDelete.ForeColor = Color.FromArgb(gBtnCtrlForeClr)
                svtBtnCtrl.btnClose.ForeColor = Color.FromArgb(gBtnCtrlForeClr)
                svtBtnCtrl.btnPrint.ForeColor = Color.FromArgb(gBtnCtrlForeClr)
                svtBtnCtrl.btnOptions.ForeColor = Color.FromArgb(gBtnCtrlForeClr)
                svtBtnCtrl.btnMovePrev.ForeColor = Color.FromArgb(gBtnCtrlForeClr)
                svtBtnCtrl.btnMoveFirst.ForeColor = Color.FromArgb(gBtnCtrlForeClr)
                svtBtnCtrl.btnFind.ForeColor = Color.FromArgb(gBtnCtrlForeClr)
                svtBtnCtrl.btnMoveNext.ForeColor = Color.FromArgb(gBtnCtrlForeClr)
                svtBtnCtrl.btnMoveLast.ForeColor = Color.FromArgb(gBtnCtrlForeClr)
                svtBtnCtrl.btnSave.ForeColor = Color.FromArgb(gBtnCtrlForeClr)
                svtBtnCtrl.btnCancel.ForeColor = Color.FromArgb(gBtnCtrlForeClr)
            End If

            If sender.tag = "D" Then
                fillDefaults()
                Exit Sub
            End If

            svtBtnCtrl.btnNew.BackgroundImage = My.Resources.ResourceManager.GetObject(UCase(sender.tag & "_NEW"))
            svtBtnCtrl.btnEdit.BackgroundImage = My.Resources.ResourceManager.GetObject(UCase(sender.tag & "_EDIT"))
            svtBtnCtrl.btnDelete.BackgroundImage = My.Resources.ResourceManager.GetObject(UCase(sender.tag & "_DELETE"))
            svtBtnCtrl.btnClose.BackgroundImage = My.Resources.ResourceManager.GetObject(UCase(sender.tag & "_CLOSE"))
            svtBtnCtrl.btnPrint.BackgroundImage = My.Resources.ResourceManager.GetObject(UCase(sender.tag & "_PRINT"))
            svtBtnCtrl.btnOptions.BackgroundImage = My.Resources.ResourceManager.GetObject(UCase(sender.tag & "_OPTIONS"))
            svtBtnCtrl.btnMovePrev.BackgroundImage = My.Resources.ResourceManager.GetObject(UCase(sender.tag & "_PREVIOUS"))
            svtBtnCtrl.btnMoveFirst.BackgroundImage = My.Resources.ResourceManager.GetObject(UCase(sender.tag & "_FIRST"))
            svtBtnCtrl.btnFind.BackgroundImage = My.Resources.ResourceManager.GetObject(UCase(sender.tag & "_FIND"))
            svtBtnCtrl.btnMoveNext.BackgroundImage = My.Resources.ResourceManager.GetObject(UCase(sender.tag & "_NEXT"))
            svtBtnCtrl.btnMoveLast.BackgroundImage = My.Resources.ResourceManager.GetObject(UCase(sender.tag & "_LAST"))
            svtBtnCtrl.btnSave.BackgroundImage = My.Resources.ResourceManager.GetObject(UCase(sender.tag & "_SAVE"))
            svtBtnCtrl.btnCancel.BackgroundImage = My.Resources.ResourceManager.GetObject(UCase(sender.tag & "_CANCEL"))
        End If
    End Sub

    Public Function GetAllCtrls(control As Control, type As Type) As IEnumerable(Of Control)
        Dim controls = control.Controls.Cast(Of Control)()

        Return controls.SelectMany(Function(ctrl) GetAllCtrls(ctrl, type)).Concat(controls).Where(Function(c) c.[GetType]() = type And CStr(c.Tag) <> "")
    End Function

    Private Sub fillCaptions()
        Dim dictionary As New Dictionary(Of String, String)
        Dim strTempCaptions As String = clsSVT.getDataStringValue("Select SetValue from UtlGSettings where SetGroup = 'BtnCtrl' and SetName = 'BtnCaptions'", "", False)

        If Trim(strTempCaptions) <> "" Then
            Dim items() As String = strTempCaptions.TrimEnd("ß").Split("ß")
            For Each item As String In items
                Dim keyValue() As String = item.Split("||")
                dictionary.Add(keyValue(0), keyValue(2))
            Next
        End If

        For Each cc As Control In btnCtrlList
            Select Case cc.Tag
                Case "New"
                    If dictionary.ContainsKey("New") Then
                        cc.Text = dictionary("New")
                    Else
                        cc.Text = btnNew.Tag
                    End If
                    Exit Select
                Case "Edit"
                    If dictionary.ContainsKey("Edit") Then
                        cc.Text = dictionary("Edit")
                    Else
                        cc.Text = btnEdit.Tag
                    End If
                    Exit Select
                Case "Delete"
                    If dictionary.ContainsKey("Delete") Then
                        cc.Text = dictionary("Delete")
                    Else
                        cc.Text = btnDelete.Tag
                    End If
                    Exit Select
                Case "Close"
                    If dictionary.ContainsKey("Close") Then
                        cc.Text = dictionary("Close")
                    Else
                        cc.Text = btnClose.Tag
                    End If
                    Exit Select
                Case "Print"
                    If dictionary.ContainsKey("Print") Then
                        cc.Text = dictionary("Print")
                    Else
                        cc.Text = btnPrint.Tag
                    End If
                    Exit Select
                Case "Options"
                    If dictionary.ContainsKey("Options") Then
                        cc.Text = dictionary("Options")
                    Else
                        cc.Text = btnOptions.Tag
                    End If
                    Exit Select
                    'Case "MovePrev"
                    '    If Dictionary.ContainsKey("MovePrev") Then
                    '        cc.Text = Dictionary("MovePrev")
                    '    Else
                    '        cc.Text = btnMovePrev.Tag
                    '    End If
                    '    Exit Select
                    'Case "MoveFirst"
                    '    If Dictionary.ContainsKey("MoveFirst") Then
                    '        cc.Text = Dictionary("MoveFirst")
                    '    Else
                    '        cc.Text = btnMoveFirst.Tag
                    '    End If
                    '    Exit Select
                    'Case "Find"
                    '    If Dictionary.ContainsKey("Find") Then
                    '        cc.Text = Dictionary("Find")
                    '    Else
                    '        cc.Text = btnFind.Tag
                    '    End If
                    '    Exit Select
                    'Case "MoveNext"
                    '    If Dictionary.ContainsKey("MoveNext") Then
                    '        cc.Text = Dictionary("MoveNext")
                    '    Else
                    '        cc.Text = btnMoveNext.Tag
                    '    End If
                    '    Exit Select
                    'Case "MoveLast"
                    '    If Dictionary.ContainsKey("MoveLast") Then
                    '        cc.Text = Dictionary("MoveLast")
                    '    Else
                    '        cc.Text = btnMoveLast.Tag
                    '    End If
                    '    Exit Select
                Case "Save"
                    If dictionary.ContainsKey("Save") Then
                        cc.Text = dictionary("Save")
                    Else
                        cc.Text = btnSave.Tag
                    End If
                    Exit Select
                Case "Cancel"
                    If dictionary.ContainsKey("Cancel") Then
                        cc.Text = dictionary("Cancel")
                    Else
                        cc.Text = btnCancel.Tag
                    End If
                    Exit Select
            End Select
        Next
    End Sub

    Private Sub fillDefaults()
        For Each c In btnCtrlList
            c.BackColor = Color.Transparent
            c.BackgroundImage = Nothing
            c.Image = Nothing
            svtBtnCtrl.btnOptions.BackColor = Color.Transparent
            svtBtnCtrl.btnOptions.BackgroundImage = Nothing
            svtBtnCtrl.btnOptions.Image = Nothing
        Next

        svtBtnCtrl.btnNew.ForeColor = Color.Black
        svtBtnCtrl.btnEdit.ForeColor = Color.Black
        svtBtnCtrl.btnDelete.ForeColor = Color.Black
        svtBtnCtrl.btnClose.ForeColor = Color.Black
        svtBtnCtrl.btnPrint.ForeColor = Color.Black
        svtBtnCtrl.btnOptions.ForeColor = Color.Black
        svtBtnCtrl.btnMovePrev.ForeColor = Color.Black
        svtBtnCtrl.btnMoveFirst.ForeColor = Color.Black
        svtBtnCtrl.btnFind.ForeColor = Color.Black
        svtBtnCtrl.btnMoveNext.ForeColor = Color.Black
        svtBtnCtrl.btnMoveLast.ForeColor = Color.Black
        svtBtnCtrl.btnSave.ForeColor = Color.Black

        svtBtnCtrl.btnCancel.ForeColor = Color.Black
        svtbtnctrl.btnNew.BackColor = Color.Transparent
        svtBtnCtrl.btnEdit.BackColor = Color.Transparent
        svtBtnCtrl.btnDelete.BackColor = Color.Transparent
        svtBtnCtrl.btnClose.BackColor = Color.Transparent
        svtBtnCtrl.btnPrint.BackColor = Color.Transparent
        svtBtnCtrl.btnOptions.BackColor = Color.Transparent
        svtBtnCtrl.btnMovePrev.BackColor = Color.Transparent
        svtBtnCtrl.btnMoveFirst.BackColor = Color.Transparent
        svtBtnCtrl.btnFind.BackColor = Color.Transparent
        svtBtnCtrl.btnMoveNext.BackColor = Color.Transparent
        svtBtnCtrl.btnMoveLast.BackColor = Color.Transparent
        svtBtnCtrl.btnSave.BackColor = Color.Transparent
        svtBtnCtrl.btnCancel.BackColor = Color.Transparent

        svtBtnCtrl.btnNew.Image = My.Resources.ResourceManager.GetObject(UCase("D_NEW"))
        svtBtnCtrl.btnEdit.Image = My.Resources.ResourceManager.GetObject(UCase("D_EDIT"))
        svtBtnCtrl.btnDelete.Image = My.Resources.ResourceManager.GetObject(UCase("D_DELETE"))
        svtBtnCtrl.btnClose.Image = My.Resources.ResourceManager.GetObject(UCase("D_CLOSE"))
        svtBtnCtrl.btnPrint.Image = My.Resources.ResourceManager.GetObject(UCase("D_PRINT"))
        svtBtnCtrl.btnOptions.Image = My.Resources.ResourceManager.GetObject(UCase("D_OPTIONS"))
        svtBtnCtrl.btnMovePrev.Image = My.Resources.ResourceManager.GetObject(UCase("D_PREVIOUS"))
        svtBtnCtrl.btnMoveFirst.Image = My.Resources.ResourceManager.GetObject(UCase("D_FIRST"))
        svtBtnCtrl.btnFind.Image = My.Resources.ResourceManager.GetObject(UCase("D_FIND"))
        svtBtnCtrl.btnMoveNext.Image = My.Resources.ResourceManager.GetObject(UCase("D_NEXT"))
        svtBtnCtrl.btnMoveLast.Image = My.Resources.ResourceManager.GetObject(UCase("D_LAST"))
        svtBtnCtrl.btnSave.Image = My.Resources.ResourceManager.GetObject(UCase("D_SAVE"))
        svtBtnCtrl.btnCancel.Image = My.Resources.ResourceManager.GetObject(UCase("D_CANCEL"))
    End Sub

End Class