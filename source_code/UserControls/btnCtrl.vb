Public Class btnCtrl
    Event btnClicked(ByVal argBtnClicked As Integer)
    Event btnOptionSelected(ByVal argOptionName As String, ByVal argBlnCtrlClick As Boolean, ByVal argBlnAltClick As Boolean)

    Private myLayoutStyle As typeLayoutStyles
    Dim clsSVT As New svtLibrary
    Dim strSender As String
    Dim btnCtrlList = GetAllCtrls(Me, GetType(Button))
    Dim lblCtrlList = GetAllCtrls(Me, GetType(LinkLabel))
    Dim txtCtrlList = GetAllCtrls(Me, GetType(TextBox))
    Dim rdoCtrlList = GetAllCtrls(Me, GetType(RadioButton))

    Public Property LayoutStyle() As typeLayoutStyles
        Get
            Return myLayoutStyle
        End Get
        Set(ByVal value As typeLayoutStyles)
            myLayoutStyle = value
            Select Case value
                Case typeLayoutStyles.Horizontal
                    'Me.Width = 643
                    'Me.Height = 29
                    'tsBtns.LayoutStyle = ToolStripLayoutStyle.HorizontalStackWithOverflow
                Case (typeLayoutStyles.Vertical)
                    'Me.Width = 50
                    'Me.Height = 400
                    'tsBtns.LayoutStyle = ToolStripLayoutStyle.VerticalStackWithOverflow
            End Select
        End Set
    End Property

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        RaiseEvent btnClicked(svtBtns.svtNew)
    End Sub

    Private Sub btnEdit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEdit.Click
        RaiseEvent btnClicked(svtBtns.svtEdit)
    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        RaiseEvent btnClicked(svtBtns.svtDelete)
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        RaiseEvent btnClicked(svtBtns.svtSave)
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        RaiseEvent btnClicked(svtBtns.svtCancel)
    End Sub

    Private Sub btnOptions_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOptions.Click
        RaiseEvent btnClicked(svtBtns.svtOptions)
    End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        RaiseEvent btnClicked(svtBtns.svtPrint)
    End Sub

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        RaiseEvent btnClicked(svtBtns.svtClose)
    End Sub

    Public Sub ClearOption()
        btnOptions.Items.Clear()
    End Sub

    Private Sub btnMoveFirst_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMoveFirst.Click
        RaiseEvent btnClicked(svtBtns.svtMoveFirst)
    End Sub

    Private Sub btnMovePrev_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMovePrev.Click
        RaiseEvent btnClicked(svtBtns.svtMovePrev)
    End Sub

    Private Sub btnFind_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFind.Click
        RaiseEvent btnClicked(svtBtns.svtFind)
    End Sub

    Private Sub btnMoveNext_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMoveNext.Click
        RaiseEvent btnClicked(svtBtns.svtMoveNext)
    End Sub

    Private Sub btnMoveLast_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMoveLast.Click
        RaiseEvent btnClicked(svtBtns.svtMoveLast)
    End Sub

    Public Sub ShowBtns(ByVal argDocID As Int32, ByVal strRights As String)
        Call EnableAll(True)
        If argDocID <= 0 Then
            btnEdit.Visible = False
            btnDelete.Visible = False
            btnPrint.Visible = False
        Else
            If chkUserRights(strRights, chkRightsPos.EditRights, Me.Text) Then

            End If
        End If

        If btnOptions.Items.Count = 0 Then
            btnOptions.Visible = False
        End If
    End Sub

    Private Sub EnableAll(ByVal blnFlg As Boolean)
        'btnNew.Enabled = blnFlg
        'btnEdit.Enabled = blnFlg
        'btnDelete.Enabled = blnFlg
        'btnCancel.Visible = False
        'btnSave.Visible = False
        'btnOptions.Enabled = blnFlg
        'btnPrint.Enabled = blnFlg
        'btnClose.Enabled = blnFlg
        ''btnhelp.enabled = blnFlg
        'btnMoveFirst.Enabled = blnFlg
        'btnMovePrev.Enabled = blnFlg
        'btnFind.Enabled = blnFlg
        'btnMoveNext.Enabled = blnFlg
        'btnMoveLast.Enabled = blnFlg

        btnNew.Visible = blnFlg
        btnDelete.Visible = blnFlg
        btnEdit.Visible = blnFlg
        btnCancel.Visible = False
        btnSave.Visible = False
        btnOptions.Visible = blnFlg
        btnPrint.Visible = blnFlg
        btnClose.Visible = blnFlg
        'btnhelp.Visible = blnFlg
        btnMoveFirst.Visible = blnFlg
        btnMovePrev.Visible = blnFlg
        btnFind.Visible = blnFlg
        btnMoveNext.Visible = blnFlg
        btnMoveLast.Visible = blnFlg
    End Sub

    Public Sub PerformAction(ByVal btnIndex As Integer, Optional ByVal argRights As String = "", Optional ByVal argDocID As Int32 = 0)
        Select Case btnIndex
            Case svtBtns.svtNew, svtBtns.svtEdit    ' add or modify
                Call EnableAll(False)
                btnCancel.Visible = True
                btnSave.Visible = True

            Case svtBtns.svtSave ' save
                Call EnableAll(False)
                Call EnableAll(True)
                                'btnNew.focus

            Case svtBtns.svtCancel  ' cancel
                Call EnableAll(False)
                Call EnableAll(True)
                'command1(svtBtns.svtNew).SetFocus()

            Case svtBtns.svtDelete ' delete
            Case svtBtns.svtPrint ' print
            Case svtBtns.svtClose  ' close
            Case svtBtns.svtOptions  ' options
            Case svtBtns.svtHelp  ' help
            Case svtBtns.svtMoveFirst  ' move first
            Case svtBtns.svtMovePrev ' move previous
            Case svtBtns.svtFind ' find
            Case svtBtns.svtMoveNext ' move next
            Case svtBtns.svtMoveLast ' move last
        End Select
        'If btnNew.visible = False Then

        '    btnNew.ForeColor = Color.White
        'Else
        '    btnNew.ForeColor = Color.Blue
        'End If
    End Sub

    Public Sub New()
        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Call EnableAll(True)
        'btnMoveFirst.BackColor = Me.BackColor
        'btnMovePrev.BackColor = Me.BackColor
        'btnFind.BackColor = Me.BackColor
        'btnMoveNext.BackColor = Me.BackColor
        'btnMoveLast.BackColor = Me.BackColor

        For Each c As Button In btnCtrlList
            AddHandler c.KeyDown, AddressOf dummy_click
        Next

    End Sub

    Public Sub dummy_click(sender As Object, e As KeyEventArgs)
        If e.KeyCode = Keys.F1 Then
            'MsgBox("zz")
        End If
    End Sub

    Public Sub addOption(ByVal argOptionName As String)
        Dim c1DD As New C1.Win.C1Input.DropDownItem
        c1DD.Text = argOptionName
        btnOptions.Items.Add(c1DD)
        btnOptions.Visible = True
    End Sub

    Private Sub btnOptions_DropDownItemClicked(ByVal sender As Object, ByVal e As C1.Win.C1Input.DropDownItemClickedEventArgs) Handles btnOptions.DropDownItemClicked
        If Control.ModifierKeys = (Keys.Control Or Keys.Alt) Then
            RaiseEvent btnOptionSelected(e.ClickedItem.Text, True, True)
        ElseIf Control.ModifierKeys = Keys.Control Then
            RaiseEvent btnOptionSelected(e.ClickedItem.Text, True, False)
        Else
            RaiseEvent btnOptionSelected(e.ClickedItem.Text, False, False)
        End If
    End Sub


    Private Sub btnCancel_MouseUp(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles btnClose.MouseUp, btnDelete.MouseUp, btnEdit.MouseUp, _
        btnFind.MouseUp, btnMoveFirst.MouseUp, btnMoveLast.MouseUp, btnMoveNext.MouseUp, btnMovePrev.MouseUp, btnNew.MouseUp, btnOptions.MouseUp, btnPrint.MouseUp
        If e.Button <> Windows.Forms.MouseButtons.Right Then Exit Sub
        strSender = sender.Name
        mnuSettings.Show(CType(sender, Control), e.Location)
    End Sub

    Private Sub mnuBtnBackClr_Click(sender As System.Object, e As System.EventArgs)
        If ColorDialog1.ShowDialog <> Windows.Forms.DialogResult.Cancel Then
            gBtnCtrlBackClr = ColorDialog1.Color.ToArgb

            btnNew.BackColor = ColorDialog1.Color
            btnEdit.BackColor = ColorDialog1.Color
            btnDelete.BackColor = ColorDialog1.Color
            btnOptions.BackColor = ColorDialog1.Color
            btnPrint.BackColor = ColorDialog1.Color
            btnClose.BackColor = ColorDialog1.Color

            Call clsSVT.execSQLStatement("Update UtlGSettings set SetValue = '" & ColorDialog1.Color.ToArgb & "' where SetName= 'BackClr' and SetGroup = 'BtnCtrl'")
        End If
    End Sub

    Private Sub mnuForeClr_Click(sender As Object, e As System.EventArgs)
        If ColorDialog1.ShowDialog <> Windows.Forms.DialogResult.Cancel Then
            gBtnCtrlForeClr = ColorDialog1.Color.ToArgb

            btnNew.ForeColor = ColorDialog1.Color
            btnEdit.ForeColor = ColorDialog1.Color
            btnDelete.ForeColor = ColorDialog1.Color
            btnOptions.ForeColor = ColorDialog1.Color
            btnPrint.ForeColor = ColorDialog1.Color
            btnClose.ForeColor = ColorDialog1.Color

            Call clsSVT.execSQLStatement("Update UtlGSettings set SetValue = '" & ColorDialog1.Color.ToArgb & "' where SetName= 'ForeClr' and SetGroup = 'BtnCtrl'")
        End If
    End Sub

    'Private Sub FlowLayoutPanel1_SizeChanged(sender As Object, e As System.EventArgs) Handles FlowLayoutPanel1.SizeChanged
    '    FlowLayoutPanel1.SuspendLayout()

    '    For Each ctrl As Control In FlowLayoutPanel1.Controls
    '        If TypeOf ctrl Is Button Then
    '            ctrl.Width = FlowLayoutPanel1.ClientSize.Width
    '        End If
    '    Next

    '    FlowLayoutPanel1.ResumeLayout()

    'End Sub


    Private Sub SettingsToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles SettingsToolStripMenuItem.Click
        Dim frmUtlSettings As New frmUtlSettings
        With frmUtlSettings
            '.strBtnCaption = strSender
            .StartPosition = FormStartPosition.CenterScreen
            .ShowDialog()
        End With
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

    Public Sub btnRearrange()
        Dim intLeft As Integer
        intLeft = 0
        If btnNew.Visible Then
            intLeft = btnNew.Width
        End If
        If btnEdit.Visible Then
            btnEdit.Left = intLeft + 6
            intLeft = intLeft + btnEdit.Width
        End If
        If btnDelete.Visible Then
            btnDelete.Left = intLeft + 6
            intLeft = intLeft + btnDelete.Width
        End If
        If btnSave.Visible Then
            btnSave.Left = intLeft + 6
            intLeft = intLeft + btnSave.Width
        End If
        If btnCancel.Visible Then
            btnCancel.Left = intLeft + 6
            intLeft = intLeft + btnCancel.Width
        End If
        If btnOptions.Visible Then
            btnOptions.Left = intLeft + 6
            intLeft = intLeft + btnOptions.Width
        End If
        If btnPrint.Visible Then
            btnPrint.Left = intLeft + 6
            intLeft = intLeft + btnPrint.Width
        End If
        If btnClose.Visible Then
            btnClose.Left = intLeft + 6
            intLeft = intLeft + btnClose.Width
        End If
        If btnMoveFirst.Visible Then
            btnMoveFirst.Left = intLeft + 6
            intLeft = intLeft + btnMoveFirst.Width
        End If
        If btnMovePrev.Visible Then
            btnMovePrev.Left = intLeft + 6
            intLeft = intLeft + btnMovePrev.Width
        End If
        If btnFind.Visible Then
            btnFind.Left = intLeft + 6
            intLeft = intLeft + btnFind.Width
        End If
        If btnMoveNext.Visible Then
            btnMoveNext.Left = intLeft + 6
            intLeft = intLeft + btnMoveNext.Width
        End If
        If btnMoveLast.Visible Then
            btnMoveLast.Left = intLeft + 6
            intLeft = intLeft + btnMoveLast.Width
        End If
    End Sub

    Public Sub btnCtrlKeyUp(sender As Object, e As System.Windows.Forms.KeyEventArgs)
        'for save if it is ctrl+S
        Dim strKeyPress As String = ""
        If e.Control And e.Shift Then
            strKeyPress = "Ctrl+Shift+" & e.KeyCode.ToString
        ElseIf e.Control Then
            strKeyPress = "Ctrl+" & e.KeyCode.ToString
        Else
            strKeyPress = e.KeyCode.ToString
        End If
        Select strKeyPress
            Case gBCShortCutKey.SaveKey
                RaiseEvent btnClicked(svtBtns.svtSave)
            Case gBCShortCutKey.AddKey
                RaiseEvent btnClicked(svtBtns.svtNew)
            Case gBCShortCutKey.EditKey
                RaiseEvent btnClicked(svtBtns.svtEdit)
            Case gBCShortCutKey.CloseKey
                RaiseEvent btnClicked(svtBtns.svtClose)
            Case gBCShortCutKey.FindKey
                RaiseEvent btnClicked(svtBtns.svtFind)
            Case gBCShortCutKey.CancelKey
                RaiseEvent btnClicked(svtBtns.svtCancel)
            Case gBCShortCutKey.DeleteKey
                RaiseEvent btnClicked(svtBtns.svtDelete)
            Case gBCShortCutKey.MoveFirstKey
                RaiseEvent btnClicked(svtBtns.svtMoveFirst)
            Case gBCShortCutKey.MoveLastKey
                RaiseEvent btnClicked(svtBtns.svtMoveLast)
            Case gBCShortCutKey.MoveNextKey
                RaiseEvent btnClicked(svtBtns.svtMoveNext)
            Case gBCShortCutKey.MovePrevKey
                RaiseEvent btnClicked(svtBtns.svtMovePrev)
        End Select
    End Sub

    Private Sub btnCtrl_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        'btnOptions.Visible = False
        Me.BackColor = Me.Parent.BackColor
        FlowLayoutPanel1.BackColor = Me.BackColor

        If Me.DesignMode = False Then
            Dim strClrOrTheme = getGsettings("BtnCtrl", "ClrOrTheme", "C")
            If UCase(Trim(strClrOrTheme)) = "C" Then
                If gBtnCtrlBackClr <> 0 Then
                    btnNew.BackColor = Color.FromArgb(gBtnCtrlBackClr)
                    btnEdit.BackColor = Color.FromArgb(gBtnCtrlBackClr)
                    btnDelete.BackColor = Color.FromArgb(gBtnCtrlBackClr)
                    btnClose.BackColor = Color.FromArgb(gBtnCtrlBackClr)
                    btnPrint.BackColor = Color.FromArgb(gBtnCtrlBackClr)
                    btnOptions.BackColor = Color.FromArgb(gBtnCtrlBackClr)
                    btnMovePrev.BackColor = Color.FromArgb(gBtnCtrlBackClr)
                    btnMoveFirst.BackColor = Color.FromArgb(gBtnCtrlBackClr)
                    btnFind.BackColor = Color.FromArgb(gBtnCtrlBackClr)
                    btnMoveNext.BackColor = Color.FromArgb(gBtnCtrlBackClr)
                    btnMoveLast.BackColor = Color.FromArgb(gBtnCtrlBackClr)
                    btnSave.BackColor = Color.FromArgb(gBtnCtrlBackClr)
                    btnCancel.BackColor = Color.FromArgb(gBtnCtrlBackClr)
                End If
                If gBtnCtrlForeClr <> 0 Then
                    btnNew.ForeColor = Color.FromArgb(gBtnCtrlForeClr)
                    btnEdit.ForeColor = Color.FromArgb(gBtnCtrlForeClr)
                    btnDelete.ForeColor = Color.FromArgb(gBtnCtrlForeClr)
                    btnClose.ForeColor = Color.FromArgb(gBtnCtrlForeClr)
                    btnPrint.ForeColor = Color.FromArgb(gBtnCtrlForeClr)
                    btnOptions.ForeColor = Color.FromArgb(gBtnCtrlForeClr)
                    btnMovePrev.ForeColor = Color.FromArgb(gBtnCtrlForeClr)
                    btnMoveFirst.ForeColor = Color.FromArgb(gBtnCtrlForeClr)
                    btnFind.ForeColor = Color.FromArgb(gBtnCtrlForeClr)
                    btnMoveNext.ForeColor = Color.FromArgb(gBtnCtrlForeClr)
                    btnMoveLast.ForeColor = Color.FromArgb(gBtnCtrlForeClr)
                    btnSave.ForeColor = Color.FromArgb(gBtnCtrlForeClr)
                    btnCancel.ForeColor = Color.FromArgb(gBtnCtrlForeClr)
                End If
            Else
                fillTheme()
            End If

            'Set tooltip to buttons
            Dim strTempHotkeys As String = ""

            strTempHotkeys = clsSVT.getDataStringValue("Select SetValue from UtlGSettings where SetGroup = 'BtnCtrl' and SetName = 'Hotkeys'", "", False)

            If Trim(strTempHotkeys) <> "" Then

                Dim dictionary As New Dictionary(Of String, String)
                Dim items() As String = strTempHotkeys.TrimEnd("ß").Split("ß")
                For Each item As String In items
                    Dim keyValue() As String = item.Split("||")
                    dictionary.Add(keyValue(0), keyValue(2))
                Next

                For Each cc As Control In btnCtrlList
                    Select Case cc.Tag
                        Case "New"
                            If dictionary.ContainsKey("New") Then
                                tt.SetToolTip(cc, "Shortcut: " & dictionary("New"))
                                gBCShortCutKey.AddKey = dictionary("New")
                            End If
                            Exit Select
                        Case "Edit"
                            If dictionary.ContainsKey("Edit") Then
                                tt.SetToolTip(cc, "Shortcut: " & dictionary("Edit"))
                                gBCShortCutKey.EditKey = dictionary("Edit")
                            End If
                            Exit Select
                        Case "Delete"
                            If dictionary.ContainsKey("Delete") Then
                                tt.SetToolTip(cc, "Shortcut: " & dictionary("Delete"))
                                gBCShortCutKey.DeleteKey = dictionary("Delete")
                            End If
                            Exit Select
                        Case "Close"
                            If dictionary.ContainsKey("Close") Then
                                tt.SetToolTip(cc, "Shortcut: " & dictionary("Close"))
                                gBCShortCutKey.CloseKey = dictionary("Close")
                            End If
                            Exit Select
                        Case "Print"
                            If dictionary.ContainsKey("Print") Then
                                tt.SetToolTip(cc, "Shortcut: " & dictionary("Print"))
                                gBCShortCutKey.PrintKey = dictionary("Print")
                            End If
                            Exit Select
                        Case "Options"
                            If dictionary.ContainsKey("Options") Then
                                tt.SetToolTip(cc, "Shortcut: " & dictionary("Options"))
                                gBCShortCutKey.OptionsKey = dictionary("Options")
                            End If
                            Exit Select
                        Case "MovePrev"
                            If dictionary.ContainsKey("MovePrev") Then
                                tt.SetToolTip(cc, "Shortcut: " & dictionary("MovePrev"))
                                gBCShortCutKey.MovePrevKey = dictionary("MovePrev")
                            End If
                            Exit Select
                        Case "MoveFirst"
                            If dictionary.ContainsKey("MoveFirst") Then
                                tt.SetToolTip(cc, "Shortcut: " & dictionary("MoveFirst"))
                                gBCShortCutKey.MoveFirstKey = dictionary("MoveFirst")
                            End If
                            Exit Select
                        Case "Find"
                            If dictionary.ContainsKey("Find") Then
                                tt.SetToolTip(cc, "Shortcut: " & dictionary("Find"))
                                gBCShortCutKey.FindKey = dictionary("Find")
                            End If
                            Exit Select
                        Case "MoveNext"
                            If dictionary.ContainsKey("MoveNext") Then
                                tt.SetToolTip(cc, "Shortcut: " & dictionary("MoveNext"))
                                gBCShortCutKey.MoveNextKey = dictionary("MoveNext")
                            End If
                            Exit Select
                        Case "MoveLast"
                            If dictionary.ContainsKey("MoveLast") Then
                                tt.SetToolTip(cc, "Shortcut: " & dictionary("MoveLast"))
                                gBCShortCutKey.MoveLastKey = dictionary("MoveLast")
                            End If
                            Exit Select
                        Case "Save"
                            If dictionary.ContainsKey("Save") Then
                                tt.SetToolTip(cc, "Shortcut: " & dictionary("Save"))
                                gBCShortCutKey.SaveKey = dictionary("Save")
                            End If
                            Exit Select
                        Case "Cancel"
                            If dictionary.ContainsKey("Cancel") Then
                                tt.SetToolTip(cc, "Shortcut: " & dictionary("Cancel"))
                                gBCShortCutKey.CancelKey = dictionary("Cancel")
                            End If
                            Exit Select
                    End Select
                Next
            End If

            'fill button captions
            fillCaptions()
        End If
    End Sub

    Private Sub fillDefaults()
        For Each c In btnCtrlList
            c.BackColor = Color.Transparent
            c.BackgroundImage = Nothing
            c.Image = Nothing
            btnOptions.BackColor = Color.Transparent
            btnOptions.BackgroundImage = Nothing
            btnOptions.Image = Nothing
        Next

        'If gBtnCtrlForeClr <> 0 Then
        '    btnNew.ForeColor = Color.FromArgb(gBtnCtrlForeClr)
        '    btnEdit.ForeColor = Color.FromArgb(gBtnCtrlForeClr)
        '    btnDelete.ForeColor = Color.FromArgb(gBtnCtrlForeClr)
        '    btnClose.ForeColor = Color.FromArgb(gBtnCtrlForeClr)
        '    btnPrint.ForeColor = Color.FromArgb(gBtnCtrlForeClr)
        '    btnOptions.ForeColor = Color.FromArgb(gBtnCtrlForeClr)
        '    btnMovePrev.ForeColor = Color.FromArgb(gBtnCtrlForeClr)
        '    btnMoveFirst.ForeColor = Color.FromArgb(gBtnCtrlForeClr)
        '    btnFind.ForeColor = Color.FromArgb(gBtnCtrlForeClr)
        '    btnMoveNext.ForeColor = Color.FromArgb(gBtnCtrlForeClr)
        '    btnMoveLast.ForeColor = Color.FromArgb(gBtnCtrlForeClr)
        '    btnSave.ForeColor = Color.FromArgb(gBtnCtrlForeClr)
        '    btnCancel.ForeColor = Color.FromArgb(gBtnCtrlForeClr)
        'End If


        btnNew.ForeColor = Color.Black
        btnEdit.ForeColor = Color.Black
        btnDelete.ForeColor = Color.Black
        btnClose.ForeColor = Color.Black
        btnPrint.ForeColor = Color.Black
        btnOptions.ForeColor = Color.Black
        btnMovePrev.ForeColor = Color.Black
        btnMoveFirst.ForeColor = Color.Black
        btnFind.ForeColor = Color.Black
        btnMoveNext.ForeColor = Color.Black
        btnMoveLast.ForeColor = Color.Black
        btnSave.ForeColor = Color.Black
        btnCancel.ForeColor = Color.Black

        btnNew.BackColor = Color.Transparent
        btnEdit.BackColor = Color.Transparent
        btnDelete.BackColor = Color.Transparent
        btnClose.BackColor = Color.Transparent
        btnPrint.BackColor = Color.Transparent
        btnOptions.BackColor = Color.Transparent
        btnMovePrev.BackColor = Color.Transparent
        btnMoveFirst.BackColor = Color.Transparent
        btnFind.BackColor = Color.Transparent
        btnMoveNext.BackColor = Color.Transparent
        btnMoveLast.BackColor = Color.Transparent
        btnSave.BackColor = Color.Transparent
        btnCancel.BackColor = Color.Transparent

        btnNew.Image = My.Resources.ResourceManager.GetObject(UCase("D_NEW"))
        btnEdit.Image = My.Resources.ResourceManager.GetObject(UCase("D_EDIT"))
        btnDelete.Image = My.Resources.ResourceManager.GetObject(UCase("D_DELETE"))
        btnClose.Image = My.Resources.ResourceManager.GetObject(UCase("D_CLOSE"))
        btnPrint.Image = My.Resources.ResourceManager.GetObject(UCase("D_PRINT"))
        btnOptions.Image = My.Resources.ResourceManager.GetObject(UCase("D_OPTIONS"))
        btnMovePrev.Image = My.Resources.ResourceManager.GetObject(UCase("D_PREVIOUS"))
        btnMoveFirst.Image = My.Resources.ResourceManager.GetObject(UCase("D_FIRST"))
        btnFind.Image = My.Resources.ResourceManager.GetObject(UCase("D_FIND"))
        btnMoveNext.Image = My.Resources.ResourceManager.GetObject(UCase("D_NEXT"))
        btnMoveLast.Image = My.Resources.ResourceManager.GetObject(UCase("D_LAST"))
        btnSave.Image = My.Resources.ResourceManager.GetObject(UCase("D_SAVE"))
        btnCancel.Image = My.Resources.ResourceManager.GetObject(UCase("D_CANCEL"))
    End Sub

    Private Sub fillTheme()

        Dim strTheme = getGsettings("BtnCtrl", "BtnTheme", "D")

        For Each c In btnCtrlList
            c.BackColor = Color.Transparent
            c.BackgroundImage = Nothing
            c.Image = Nothing
            btnOptions.BackColor = Color.Transparent
            btnOptions.BackgroundImage = Nothing
            btnOptions.Image = Nothing
        Next

        If Val(gBtnCtrlForeClr) <> 0 Then
            btnNew.ForeColor = Color.FromArgb(gBtnCtrlForeClr)
            btnEdit.ForeColor = Color.FromArgb(gBtnCtrlForeClr)
            btnDelete.ForeColor = Color.FromArgb(gBtnCtrlForeClr)
            btnClose.ForeColor = Color.FromArgb(gBtnCtrlForeClr)
            btnPrint.ForeColor = Color.FromArgb(gBtnCtrlForeClr)
            btnOptions.ForeColor = Color.FromArgb(gBtnCtrlForeClr)
            btnMovePrev.ForeColor = Color.FromArgb(gBtnCtrlForeClr)
            btnMoveFirst.ForeColor = Color.FromArgb(gBtnCtrlForeClr)
            btnFind.ForeColor = Color.FromArgb(gBtnCtrlForeClr)
            btnMoveNext.ForeColor = Color.FromArgb(gBtnCtrlForeClr)
            btnMoveLast.ForeColor = Color.FromArgb(gBtnCtrlForeClr)
            btnSave.ForeColor = Color.FromArgb(gBtnCtrlForeClr)
            btnCancel.ForeColor = Color.FromArgb(gBtnCtrlForeClr)
        End If

        If strTheme = "D" Then
            fillDefaults()
            Exit Sub
        End If

        btnNew.BackgroundImage = My.Resources.ResourceManager.GetObject(UCase(strTheme & "_NEW"))
        btnEdit.BackgroundImage = My.Resources.ResourceManager.GetObject(UCase(strTheme & "_EDIT"))
        btnDelete.BackgroundImage = My.Resources.ResourceManager.GetObject(UCase(strTheme & "_DELETE"))
        btnClose.BackgroundImage = My.Resources.ResourceManager.GetObject(UCase(strTheme & "_CLOSE"))
        btnPrint.BackgroundImage = My.Resources.ResourceManager.GetObject(UCase(strTheme & "_PRINT"))
        btnOptions.BackgroundImage = My.Resources.ResourceManager.GetObject(UCase(strTheme & "_OPTIONS"))
        btnMovePrev.BackgroundImage = My.Resources.ResourceManager.GetObject(UCase(strTheme & "_PREVIOUS"))
        btnMoveFirst.BackgroundImage = My.Resources.ResourceManager.GetObject(UCase(strTheme & "_FIRST"))
        btnFind.BackgroundImage = My.Resources.ResourceManager.GetObject(UCase(strTheme & "_FIND"))
        btnMoveNext.BackgroundImage = My.Resources.ResourceManager.GetObject(UCase(strTheme & "_NEXT"))
        btnMoveLast.BackgroundImage = My.Resources.ResourceManager.GetObject(UCase(strTheme & "_LAST"))
        btnSave.BackgroundImage = My.Resources.ResourceManager.GetObject(UCase(strTheme & "_SAVE"))
        btnCancel.BackgroundImage = My.Resources.ResourceManager.GetObject(UCase(strTheme & "_CANCEL"))
    End Sub

    Private Sub btnAlias_Click(sender As Object, e As System.EventArgs)
        Dim frmAlias As New frmUtlAlias

        With frmAlias
            .MdiParent = frmMain
            .Show()
        End With
    End Sub

    Private Sub btnOptions_KeyUp(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles btnOptions.KeyUp
        If e.KeyCode = Keys.Space Then
            If Not btnOptions.DroppedDown Then
                btnOptions.DroppedDown = True
            Else
                btnOptions.DroppedDown = False
            End If
        End If
    End Sub

    Public Sub reSizeBtnCtrl()
        btnMoveFirst.Size = New Point(btnMoveFirst.Size.Width, btnNew.Size.Height)
        btnMoveLast.Size = New Point(btnMoveLast.Size.Width, btnNew.Size.Height)
        btnMoveNext.Size = New Point(btnMoveNext.Size.Width, btnNew.Size.Height)
        btnMovePrev.Size = New Point(btnMovePrev.Size.Width, btnNew.Size.Height)
        btnFind.Size = New Point(btnFind.Size.Width, btnNew.Size.Height)
    End Sub

End Class
