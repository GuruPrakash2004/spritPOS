Public Class frmModule

    ' Load all of the saved settings.
    Public Shared Sub LoadAllSettings(ByVal app_name As String, ByVal frm As Form)
        Try
            frm.SetBounds( _
            CInt(INIGet("FORM_POSITION", frm.Name & "-" & "Form_Left", frm.Left)), _
            CInt(INIGet("FORM_POSITION", frm.Name & "-" & "Form_Top", frm.Top)), _
            CInt(INIGet("FORM_SIZE", frm.Name & "-" & "Form_Width", frm.Width)), _
            CInt(INIGet("FORM_SIZE", frm.Name & "-" & "Form_Height", frm.Height)))
            'frm.Left = GetSetting(app_name, "Settings", frm.Name & "." & "FormLeft", frm.Left)

            Dim state As Integer = INIGet("WINDOW_STATE", frm.Name & "-" & "Form_WindowState", CInt(frm.WindowState))
            frm.WindowState = DirectCast(state, FormWindowState)
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
        ' Load form settings.


        ' Load the controls' values.
        If Not frm Is Nothing Then
            LoadChildSettings(app_name, frm, frm.Name)
        End If
    End Sub

    ' Load all child control settings.
    Public Shared Sub LoadChildSettings(ByVal app_name As String, ByVal parent As Control, argFrmName As String)
        Try
            For Each child As Control In parent.Controls
                ' Restore the child's value.
                Select Case (child.GetType().Name)
                    Case "C1FlexGrid"
                        Dim strWidth As String = ""
                        Dim arrColwidths() As String

                        Dim vsf As C1.Win.C1FlexGrid.C1FlexGrid = DirectCast(child, C1.Win.C1FlexGrid.C1FlexGrid)

                        'strWidth = GetSetting(app_name, "Settings", parent.Name & "." & vsf.Name, strWidth)
                        strWidth = INIGet(argFrmName & "-VSF_COLWIDTH", vsf.Name, strWidth)

                        If Trim(strWidth) = "" Then Continue For

                        arrColwidths = Split(GetVal(strWidth, ""), ",")

                        For i = 0 To UBound(arrColwidths)
                            If i > vsf.Cols.Count - 1 Then Exit For
                            vsf.Cols(i).Width = GetVal(arrColwidths(i), -1)
                        Next i
                    Case "SplitContainer"
                        Dim splCon As SplitContainer = DirectCast(child, SplitContainer)
                        'INIStore(gUserCode, parent.Name & "." & splCon.Name & "." & "Height", splCon.Height)
                        splCon.Width = INIGet(argFrmName & "-SPLITTER", splCon.Name & "-" & "Width", splCon.Width)
                        splCon.Height = INIGet(argFrmName & "-SPLITTER", splCon.Name & "-" & "Height", splCon.Height)
                        splCon.SplitterDistance = INIGet(argFrmName & "-SPLITTER", splCon.Name & "-" & "Distance", splCon.SplitterDistance)
                        ' Add other control types here.
                End Select

                ' Recursively restore the child's children.
                LoadChildSettings(app_name, child, argFrmName)
            Next child
        Catch ex As Exception

        End Try


        'Case "TextBox", "ListBox", "ComboBox"
        '    child.Text = GetSetting(app_name, "Settings", child.Name, child.Text)

        'Case "CheckBox"
        '    Dim chk As CheckBox = DirectCast(child, CheckBox)
        '    chk.Checked = Boolean.Parse( _
        '        GetSetting(app_name, "Settings", child.Name, chk.Checked.ToString()))

        'Case "RadioButton"
        '    Dim rad As RadioButton = DirectCast(child, RadioButton)
        '    rad.Checked = Boolean.Parse( _
        '        GetSetting(app_name, "Settings", child.Name, rad.Checked.ToString()))

        'Case "VScrollBar"
        '    Dim vscr As VScrollBar = DirectCast(child, VScrollBar)
        '    vscr.Value = CInt(GetSetting(app_name, "Settings", child.Name, vscr.Value))

        'Case "HScrollBar"
        '    Dim hscr As HScrollBar = DirectCast(child, HScrollBar)
        '    hscr.Value = CInt(GetSetting(app_name, "Settings", child.Name, hscr.Value))

        'Case "NumericUpDown"
        '    Dim nud As NumericUpDown = DirectCast(child, NumericUpDown)
        '    nud.Value = Decimal.Parse(GetSetting(app_name, "Settings", _
        '        child.Name, nud.Value).ToString())


    End Sub

    ' Save all of the form's settings.
    Public Shared Sub SaveAllSettings(ByVal app_name As String, ByVal frm As Form)
        Try
            ' Save form settings.
            'Dim state As Integer = INIGet("WINDOW_STATE", frm.Name & "-" & "Form_WindowState", CInt(frm.WindowState))
            'frm.WindowState = DirectCast(state, FormWindowState)
            INIStore("WINDOW_STATE", frm.Name & "-" & "Form_WindowState", CInt(frm.WindowState))

            If (frm.WindowState = FormWindowState.Normal) Then
                ' Save current bounds.
                'SaveSetting(app_name, "Settings", frm.Name & "." & "FormLeft", frm.Left)
                INIStore("FORM_POSITION", frm.Name & "-" & "Form_Left", frm.Left)
                INIStore("FORM_POSITION", frm.Name & "-" & "Form_Top", frm.Top)
                INIStore("FORM_SIZE", frm.Name & "-" & "Form_Width", frm.Width)
                INIStore("FORM_SIZE", frm.Name & "-" & "Form_Height", frm.Height)
            Else
                ' Save bounds when we're restored.
                INIStore("FORM_POSITION", frm.Name & "-" & "Form_Left", frm.RestoreBounds.Left)
                INIStore("FORM_POSITION", frm.Name & "-" & "Form_Top", frm.RestoreBounds.Top)
                INIStore("FORM_SIZE", frm.Name & "-" & "Form_Width", frm.RestoreBounds.Width)
                INIStore("FORM_SIZE", frm.Name & "-" & "Form_Height", frm.RestoreBounds.Height)
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try


        ' Save the controls' values.
        SaveChildSettings(app_name, frm, frm.Name)
    End Sub

    ' Save all child control settings.
    Public Shared Sub SaveChildSettings(ByVal app_name As String, ByVal parent As Control, argFrmName As String)
        Try
            For Each child As Control In parent.Controls
                ' Save the child's value.
                Select Case (child.GetType().Name)
                    'Case "TextBox", "ListBox", "ComboBox"
                    '    SaveSetting(app_name, "Settings", child.Name, child.Text)

                    'Case "CheckBox"
                    '    Dim chk As CheckBox = DirectCast(child, CheckBox)
                    '    SaveSetting(app_name, "Settings", child.Name, chk.Checked.ToString())

                    'Case "RadioButton"
                    '    Dim rad As RadioButton = DirectCast(child, RadioButton)
                    '    SaveSetting(app_name, "Settings", child.Name, rad.Checked.ToString())

                    'Case "VScrollBar"
                    '    Dim vscr As VScrollBar = DirectCast(child, VScrollBar)
                    '    SaveSetting(app_name, "Settings", child.Name, vscr.Value)

                    'Case "HScrollBar"
                    '    Dim hscr As HScrollBar = DirectCast(child, HScrollBar)
                    '    SaveSetting(app_name, "Settings", child.Name, hscr.Value)

                    'Case "NumericUpDown"
                    '    Dim nud As NumericUpDown = DirectCast(child, NumericUpDown)
                    '    SaveSetting(app_name, "Settings", child.Name, nud.Value)

                    ' Add other control types here.
                    Case "C1FlexGrid"
                        Dim strWidth As String = ""
                        Dim vsf As C1.Win.C1FlexGrid.C1FlexGrid = DirectCast(child, C1.Win.C1FlexGrid.C1FlexGrid)
                      
                        For i = 0 To vsf.Cols.Count - 1
                            strWidth = strWidth & vsf.Cols(i).Width & ","
                        Next i
                        'SaveSetting(app_name, "Settings", parent.Name & "." & vsf.Name, strWidth)
                        INIStore(argFrmName & "-VSF_COLWIDTH", vsf.Name, strWidth)

                    Case "SplitContainer"
                        Dim splCon As SplitContainer = DirectCast(child, SplitContainer)
                        INIStore(argFrmName & "-SPLITTER", splCon.Name & "-" & "Width", splCon.Width)
                        INIStore(argFrmName & "-SPLITTER", splCon.Name & "-" & "Height", splCon.Height)
                        INIStore(argFrmName & "-SPLITTER", splCon.Name & "-" & "Distance", splCon.SplitterDistance)
                End Select

                ' Recursively save the child's children.
                SaveChildSettings(app_name, child, argFrmName)
            Next child
        Catch ex As Exception

        End Try

    End Sub
End Class
