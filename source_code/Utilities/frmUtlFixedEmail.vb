Public Class frmUtlFixedEmail

    Private Sub rdoFixed_CheckedChanged(sender As Object, e As EventArgs) Handles rdoFixed.CheckedChanged
        If rdoFixed.Checked Then
            pnlFixed.Visible = True
        Else
            pnlFixed.Visible = False
        End If
    End Sub

    Private Sub rdoVariable_CheckedChanged(sender As Object, e As EventArgs) Handles rdoVariable.CheckedChanged
        If rdoVariable.Checked Then
            pnlVariable.Visible = True
            pnlVariable.BringToFront()
        Else
            pnlVariable.Visible = False
        End If
    End Sub

    Public Sub ClearFixedEmailDtls()
        cboSendingType.SelectedIndex = -1
        txtFixedEMailID.Text = ""
        txtFldCond.Text = ""
        txtFieldName.Text = ""
    End Sub

    Private Sub btnPnlClose_Click(sender As Object, e As EventArgs) Handles btnPnlClose.Click
        Me.Close()
    End Sub

    Public Sub LoadDataFromGrid(Optional ByVal argType As String = "", Optional ByVal argBlnFixed As String = "Y",
                                Optional ByVal argFixedEmail As String = "",
                                Optional ByVal argFldName As String = "", Optional ByVal argFldCond As String = "")
        Call ClearFixedEmailDtls()
        Select Case argType
            Case "T"
                cboSendingType.Text = "To"
            Case "C"
                cboSendingType.Text = "Cc"
            Case "B"
                cboSendingType.Text = "Bcc"
            Case Else
                cboSendingType.Text = ""
        End Select

        If argBlnFixed = "Y" Then
            rdoFixed.Checked = True
            pnlFixed.Visible = True
            pnlVariable.Visible = False
            txtFixedEMailID.Text = argFixedEmail
        Else
            rdoFixed.Checked = False
            pnlFixed.Visible = False
            pnlVariable.Visible = True
            txtFieldName.Text = argFldName
            txtFldCond.Text = argFldCond
        End If
    End Sub

    Private Sub btnOK_Click(sender As Object, e As EventArgs) Handles btnOK.Click
        Dim strSendtype As String = "", strFixed As String = ""
        Select Case cboSendingType.Text
            Case "To"
                strSendtype = "T"
            Case "Cc"
                strSendtype = "C"
            Case "Bcc"
                strSendtype = "B"
            Case Else
                MsgBox("Type is empty....", MsgBoxStyle.Exclamation, Me.Text)
                cboSendingType.Focus()
                Exit Sub
        End Select

        If Trim(rdoFixed.Checked) Then
            strFixed = "Y"
            If Trim(txtFixedEMailID.Text) = "" Then
                MsgBox("email ID is empty....", MsgBoxStyle.Exclamation, Me.Text)
                txtFixedEMailID.Focus()
                Exit Sub
            End If
        Else
            strFixed = "N"
            If Trim(txtFieldName.Text) = "" Then
                MsgBox("Fld Name is empty....", MsgBoxStyle.Exclamation, Me.Text)
                txtFieldName.Focus()
                Exit Sub
            End If
            If Trim(txtFldCond.Text) = "" Then
                MsgBox("Fld Condition is empty....", MsgBoxStyle.Exclamation, Me.Text)
                txtFldCond.Focus()
                Exit Sub
            End If
        End If

        Dim strDtl As String = ""
        strDtl &= strFixed & "■"
        strDtl &= strSendtype & "■"
        If strFixed = "Y" Then
            strDtl &= Trim(txtFixedEMailID.Text) & "■"
            strDtl &= "" & "■"
            strDtl &= ""
        Else
            strDtl &= "" & "■"
            strDtl &= Trim(txtFieldName.Text) & "■"
            strDtl &= Trim(txtFldCond.Text)
        End If

        gStrDummy = strDtl
        Me.Close()
    End Sub

    Private Sub frmUtlFixedEmail_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Call initGridStyles(Me)
        Call fillFormProperties(Me)
    End Sub
End Class