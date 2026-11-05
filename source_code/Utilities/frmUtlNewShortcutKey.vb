Public Class frmUtlNewShortcutKey
    Dim clsSVt As New svtLibrary
    Public argDocID As Long
    Public argDocName As String
    Public argCaption As String
    Public argForm As Object
    Public argDocIDFldName As String
    Public argDocNoFldValue As String
    Public argHdrTblName As String
    Public argDtlTblName As String
    Public argTblList As String
    Public strDelCancelStatus As String
    Public strBtnCaption As String
    Public strShortcutKeys As String
    Public strNewHotKey As String = ""
    Dim strTempHotkeys As String = ""
    Dim dictionary As New Dictionary(Of String, String)
    Dim currValue As String = ""
    Public currCombo As String = ""

    Private Sub frmUtlNewShortcutKey_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        fillDocDetails()

        strTempHotkeys = clsSVt.getDataStringValue("Select SetValue from UtlGSettings where SetGroup = 'BtnCtrl' and SetName = 'Hotkeys'", "", False)
        If Trim(strTempHotkeys) <> "" Then
            Dim items() As String = strTempHotkeys.TrimEnd("ß").Split("ß")
            For Each item As String In items
                Dim keyValue() As String = item.Split("||")
                dictionary.Add(keyValue(0), keyValue(2))
            Next
        End If
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

    Private Sub btnCancel_Click(sender As System.Object, e As System.EventArgs) Handles btnCancel.Click
        strNewHotKey = ""
        Me.Close()
    End Sub

    Private Sub btnOk_Click(sender As Object, e As System.EventArgs) Handles btnOk.Click
        If Trim(cboKey.Text) = "" Then
            MsgBox("Please select a key!", vbInformation, Me.Text)
            Exit Sub
        End If

        strNewHotKey = ""

        If chkCtrl.Checked Then
            strNewHotKey &= "Ctrl+"
        End If
        If chkAlt.Checked Then
            strNewHotKey &= "Alt+"
        End If
        If chkShift.Checked Then
            strNewHotKey &= "Shift+"
        End If

        strNewHotKey &= cboKey.Text

        Me.Close()
    End Sub

    Private Function combinationExists() As Boolean
        combinationExists = True

        Dim strCurrCombo As String = ""

        If chkCtrl.Checked Then
            strCurrCombo &= "Ctrl+"
        End If
        If chkAlt.Checked Then
            strCurrCombo &= "Alt+"
        End If
        If chkShift.Checked Then
            strCurrCombo &= "Shift+"
        End If

        strCurrCombo &= cboKey.Text

        If dictionary.ContainsValue(strCurrCombo) AndAlso UCase(Trim(currCombo)) <> UCase(Trim(strCurrCombo)) Then
            Dim key = (From k In dictionary Select k Where k.Value = strCurrCombo).First.Key

            MsgBox("Combination: '" & strCurrCombo & "' already in use by the button '" & key & "'" & _
                   vbCrLf & vbCrLf & "Please try a different combination!!", vbCritical, Me.Text)
            Exit Function
        End If


        combinationExists = False
    End Function

    Private Sub cboKey_DropDown(sender As Object, e As System.EventArgs) Handles cboKey.DropDown
        currValue = cboKey.Text
    End Sub

    Private Sub cboKey_SelectedValueChanged(sender As Object, e As System.EventArgs) Handles cboKey.SelectedValueChanged
        If combinationExists() Then
            cboKey.Text = currValue
        End If
    End Sub

    Private Sub chkAlt_CheckStateChanged(sender As Object, e As System.EventArgs) Handles chkAlt.CheckStateChanged, chkCtrl.CheckStateChanged, chkShift.CheckStateChanged
        Dim checkState As CheckState = sender.checkState

        If combinationExists() Then
            If checkState = Windows.Forms.CheckState.Unchecked Then
                sender.checked = True
            Else
                sender.checked = False
            End If
        End If
    End Sub
End Class