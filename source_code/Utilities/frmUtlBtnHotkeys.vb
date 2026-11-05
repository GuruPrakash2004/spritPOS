Imports C1.Win.C1FlexGrid

Public Class frmUtlBtnHotkeys
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
    Public strHotKey As String = ""
    Dim btnCtrlList = GetAllCtrls(Me, GetType(Button))
    Dim lblCtrlList = GetAllCtrls(Me, GetType(LinkLabel))
    Dim txtCtrlList = GetAllCtrls(Me, GetType(TextBox))

    Private Sub frmUtlBtnHotkeys_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        initGridStyles(Me)
        fillDoc()
        For Each cc As Control In lblCtrlList
            AddHandler cc.Click, AddressOf dummy_click
        Next
    End Sub

    Public Sub fillDoc()
        fillLinkLabels()
        fillCaptions()
    End Sub

    Private Sub fillLinkLabels()
        Dim strTempHotkeys As String = ""
        Dim strTempKeys() As String = Nothing

        strTempHotkeys = clsSVT.getDataStringValue("Select SetValue from UtlGSettings where SetGroup = 'BtnCtrl' and SetName = 'Hotkeys'", "", False)

        If Trim(strTempHotkeys) <> "" Then
            strTempKeys = Split(strTempHotkeys, "ß")

            Dim dictionary As New Dictionary(Of String, String)
            Dim items() As String = strTempHotkeys.TrimEnd("ß").Split("ß")
            For Each item As String In items
                Dim keyValue() As String = item.Split("||")
                dictionary.Add(keyValue(0), keyValue(2))
            Next

            For Each cc As Control In lblCtrlList
                Select Case cc.Tag
                    Case "New"
                        If dictionary.ContainsKey("New") Then
                            cc.Text = dictionary("New")
                        End If
                        Exit Select
                    Case "Edit"
                        If dictionary.ContainsKey("Edit") Then
                            cc.Text = dictionary("Edit")
                        End If
                        Exit Select
                    Case "Delete"
                        If dictionary.ContainsKey("Delete") Then
                            cc.Text = dictionary("Delete")
                        End If
                        Exit Select
                    Case "Close"
                        If dictionary.ContainsKey("Close") Then
                            cc.Text = dictionary("Close")
                        End If
                        Exit Select
                    Case "Print"
                        If dictionary.ContainsKey("Print") Then
                            cc.Text = dictionary("Print")
                        End If
                        Exit Select
                    Case "Options"
                        If dictionary.ContainsKey("Options") Then
                            cc.Text = dictionary("Options")
                        End If
                        Exit Select
                    Case "MovePrev"
                        If dictionary.ContainsKey("MovePrev") Then
                            cc.Text = dictionary("MovePrev")
                        End If
                        Exit Select
                    Case "MoveFirst"
                        If dictionary.ContainsKey("MoveFirst") Then
                            cc.Text = dictionary("MoveFirst")
                        End If
                        Exit Select
                    Case "Find"
                        If dictionary.ContainsKey("Find") Then
                            cc.Text = dictionary("Find")
                        End If
                        Exit Select
                    Case "MoveNext"
                        If dictionary.ContainsKey("MoveNext") Then
                            cc.Text = dictionary("MoveNext")
                        End If
                        Exit Select
                    Case "MoveLast"
                        If dictionary.ContainsKey("MoveLast") Then
                            cc.Text = dictionary("MoveLast")
                        End If
                        Exit Select
                    Case "Save"
                        If dictionary.ContainsKey("Save") Then
                            cc.Text = dictionary("Save")
                        End If
                        Exit Select
                    Case "Cancel"
                        If dictionary.ContainsKey("Cancel") Then
                            cc.Text = dictionary("Cancel")
                        End If
                        Exit Select
                End Select
            Next
        End If
    End Sub

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

        For Each cc As Control In txtCtrlList
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

    Private Sub dummy_click(sender As Object, e As EventArgs)
        Dim frmNewShortcut As New frmUtlNewShortcutKey
        With frmNewShortcut
            .strNewHotKey = sender.text
            .currCombo = sender.text
            .StartPosition = FormStartPosition.CenterScreen
            .ShowDialog()
            If Trim(.strNewHotKey) <> "" Then
                sender.text = .strNewHotKey
            End If
        End With
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
                    End If
                End If
            End If
        Next

        strSQL = "Select * from UtlSearchSettings where docName = '" & strDocName & "'"
        rs = clsSVT.getDataReaderFromSQL(strSQL)
        Do While rs.Read
        Loop

    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSettClose.Click
        blnSRCHFrom = False
        Me.Close()

    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSettSave.Click
        saveHotKeys()
        saveBtnCaptions()

        MsgBox("Saved Settings Successfully!", MsgBoxStyle.Information, Me.Text)
        Me.Close()
    End Sub

    Private Sub saveHotKeys()
        Dim strHotkeys As String = ""
        Dim blnFound As Boolean = False

        For Each cc As Control In lblCtrlList
            If TypeOf (cc) Is LinkLabel Then
                strHotkeys &= cc.Tag & "||" & cc.Text & "ß"
            End If
        Next
            
        strHotkeys = strHotkeys.Replace(" ", "")
        strHotkeys = strHotkeys.TrimEnd("ß")

        clsSVT.getDataStringValue("Select SetValue from UtlGSettings where SetGroup = 'BtnCtrl' and SetName = 'Hotkeys'", "", blnFound)

        If blnFound Then
            clsSVT.execSQLStatement("Update UtlGSettings set SetValue = '" & strHotkeys & "' where SetGroup = 'BtnCtrl' and SetName = 'Hotkeys'")
        Else
            clsSVT.execSQLStatement("Insert into UtlGSettings (SetGroup, SetName, SetValue) values ('BtnCtrl', 'Hotkeys', '" & strHotkeys & "')")
        End If
    End Sub

    Private Sub saveBtnCaptions()
        Dim strBtnCaptions As String = ""
        Dim blnFound As Boolean = False


        For Each cc As Control In txtCtrlList
            strBtnCaptions &= cc.Tag & "||" & cc.Text & "ß"
        Next

        strBtnCaptions = strBtnCaptions.Replace(" ", "")
        strBtnCaptions = strBtnCaptions.TrimEnd("ß")

        clsSVT.getDataStringValue("Select SetValue from UtlGSettings where SetGroup = 'BtnCtrl' and SetName = 'BtnCaptions'", "", blnFound)

        If blnFound Then
            clsSVT.execSQLStatement("Update UtlGSettings set SetValue = '" & strBtnCaptions & "' where SetGroup = 'BtnCtrl' and SetName = 'BtnCaptions'")
        Else
            clsSVT.execSQLStatement("Insert into UtlGSettings (SetGroup, SetName, SetValue) values ('BtnCtrl', 'BtnCaptions', '" & strBtnCaptions & "')")
        End If
    End Sub

    Private Sub vsfDet_BeforeEdit(sender As Object, e As C1.Win.C1FlexGrid.RowColEventArgs)
        Select Case e.Col
            Case vDetSel
                e.Cancel = False

            Case vDetFldCode
                e.Cancel = True
        End Select
    End Sub

    Private Sub lblChangeForeClr_LinkClicked(sender As System.Object, e As System.Windows.Forms.LinkLabelLinkClickedEventArgs)
        If cdg.ShowDialog <> Windows.Forms.DialogResult.Cancel Then
            gBtnCtrlForeClr = cdg.Color.ToArgb

            'btnNew.ForeColor = cdg.Color
            'btnEdit.ForeColor = cdg.Color
            'btnDelete.ForeColor = cdg.Color
            'btnClose.ForeColor = cdg.Color
            'btnPrint.ForeColor = cdg.Color
            'btnOptions.ForeColor = cdg.Color
            'btnMovePrev.ForeColor = cdg.Color
            'btnMoveFirst.ForeColor = cdg.Color
            'btnFind.ForeColor = cdg.Color
            'btnMoveNext.ForeColor = cdg.Color
            'btnMoveLast.ForeColor = cdg.Color
            'btnSave.ForeColor = cdg.Color
            'btnCancel.ForeColor = cdg.Color

            'Call clsSVT.execSQLStatement("Update UtlGSettings set SetValue = '" & cdg.Color.ToArgb & "' where SetName= 'ForeClr' and SetGroup = 'BtnCtrl'")
        End If
    End Sub

    Private Sub lblChangeBackClr_LinkClicked(sender As System.Object, e As System.Windows.Forms.LinkLabelLinkClickedEventArgs)
        If cdg.ShowDialog <> Windows.Forms.DialogResult.Cancel Then
            gBtnCtrlBackClr = cdg.Color.ToArgb

            'btnNew.BackColor = cdg.Color
            'btnEdit.BackColor = cdg.Color
            'btnDelete.BackColor = cdg.Color
            'btnClose.BackColor = cdg.Color
            'btnPrint.BackColor = cdg.Color
            'btnOptions.BackColor = cdg.Color
            'btnMovePrev.BackColor = cdg.Color
            'btnMoveFirst.BackColor = cdg.Color
            'btnFind.BackColor = cdg.Color
            'btnMoveNext.BackColor = cdg.Color
            'btnMoveLast.BackColor = cdg.Color
            'btnSave.BackColor = cdg.Color
            'btnCancel.BackColor = cdg.Color

            'Call clsSVT.execSQLStatement("Update UtlGSettings set SetValue = '" & cdg.Color.ToArgb & "' where SetName= 'ForeClr' and SetGroup = 'BtnCtrl'")
        End If
    End Sub

    'Private Sub lblNew_LinkClicked(sender As System.Object, e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles lblNew.LinkClicked
    '    Dim frmUtlNewShortcutKey As New frmUtlNewShortcutKey
    '    With frmUtlNewShortcutKey
    '        'strNewHotKey = lblNew.Text
    '        .ShowDialog()
    '    End With
    'End Sub

    Public Function GetAllCtrls(control As Control, type As Type) As IEnumerable(Of Control)
        Dim controls = control.Controls.Cast(Of Control)()

        Return controls.SelectMany(Function(ctrl) GetAllCtrls(ctrl, type)).Concat(controls).Where(Function(c) c.[GetType]() = type And CStr(c.Tag) <> "")
    End Function

    Private Sub txtNew_LostFocus(sender As Object, e As System.EventArgs) Handles txtNew.LostFocus, txtEdit.LostFocus, txtDelete.LostFocus, _
        txtClose.LostFocus, txtPrint.LostFocus, txtOptions.LostFocus, txtMoveFirst.LostFocus, txtMovePrev.LostFocus, txtMoveNext.LostFocus, _
        txtMoveLast.LostFocus, txtSave.LostFocus, txtCancel.LostFocus

        If Trim(sender.text) = "" Then
            MsgBox("Caption cannot be blank! Please enter a caption for the button!", vbCritical, Me.Text)
            sender.focus()
            Exit Sub
        Else
            Dim strCaptionList As String = ""

            For Each c As Control In txtCtrlList
                If UCase(Trim(c.Text)) = UCase(Trim(sender.text)) AndAlso c.Tag <> sender.tag Then
                    MsgBox("Caption '" & sender.text & "' is already in use! Please try a different caption!!", vbCritical, Me.Text)
                    sender.focus()
                    sender.selectall()
                    Exit Sub
                End If
            Next
        End If

    End Sub

    Private Sub txtNew_TextChanged(sender As Object, e As System.EventArgs) Handles txtNew.TextChanged, txtEdit.TextChanged, txtDelete.TextChanged, _
        txtClose.TextChanged, txtPrint.TextChanged, txtOptions.TextChanged, txtMoveFirst.TextChanged, txtMovePrev.TextChanged, txtMoveNext.TextChanged, _
        txtMoveLast.TextChanged, txtSave.TextChanged, txtCancel.TextChanged

        For Each c As Control In btnCtrlList
            If sender.tag = c.Tag Then
                c.Text = sender.text
            End If
        Next
    End Sub
End Class