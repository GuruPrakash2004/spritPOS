Public Class frmUtlToolTipText
    Dim clsSvt As New svtLibrary
    Private Sub frmUtlToolTipText_Load(sender As Object, e As EventArgs)
        fillFormProperties(Me)
        ChangeFontSize(Me)
        txtSearch.Focus()
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Dim strSql As String
        If Not validate() Then
            Exit Sub
        End If
        strSql = "Insert into UtlToolTips (frmName,CtrlName,TooltipText)  Values("
        strSql &= "'" & Trim(txtFrmName.Text) & "', "
        strSql &= "'" & Trim(txtCtrlName.Text) & "', "
        strSql &= "'" & Trim(txtToolTipText.Text) & "')"

        If clsSvt.execSQLStatement(strSql) Then
            MsgBox("Tooltip is successfully inserted", MsgBoxStyle.Information, Me.Text)
        End If
    End Sub

    Private Function validate() As Boolean
        validate = False
        If txtFrmName.Text = "" Then
            MsgBox("Please enter the form Name", MsgBoxStyle.Information, Me.Text)
            Exit Function
        End If
        If txtCtrlName.Text = "" Then
            MsgBox("Please enter the Control Name", MsgBoxStyle.Information, Me.Text)
            Exit Function
        End If
        If txtToolTipText.Text = "" Then
            MsgBox("Please enter the Tool tiptext", MsgBoxStyle.Information, Me.Text)
            Exit Function
        End If
        validate = True
    End Function

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        txtCtrlName.Text = ""
        txtFrmName.Text = ""
        txtSearch.Text = ""
        txtToolTipText.Text = ""
        txtFrmName.Focus()
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()

    End Sub

    Private Sub txtSearch_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtSearch.KeyPress, txtCtrlName.KeyPress, txtFrmName.KeyPress, txtToolTipText.KeyPress
        If Not Char.IsLetter(e.KeyChar) AndAlso Not e.KeyChar = Chr(Keys.Delete) AndAlso Not e.KeyChar = Chr(Keys.Back) AndAlso Not e.KeyChar = Chr(Keys.Space) Then

            e.Handled = True

        End If

    End Sub

    Private Sub btnLoad_Click(sender As Object, e As EventArgs) Handles btnLoad.Click
        Dim intTooltipid As String
        Dim strSql As String
        Dim rsDummy As oledb.oledbDataReader
        intTooltipid = clsSvt.getDataLongValue("Select Tooltipid from utlTooltips where frmName='" & txtSearch.Text & "'", 0, False)
        If intTooltipid = 0 Then
            MsgBox("Please enter valid from name", MsgBoxStyle.Information, Me.Text)
            txtSearch.Text = ""
            txtSearch.Focus()
            Exit Sub

        End If

        strSql = "Select frmName,ctrlName,TooltipText from utlTooltips where TooltipId='" & intTooltipid & "'"
        rsDummy = clsSvt.getDataReaderFromSQL(strSql)
        If Not rsDummy.HasRows Then
            rsDummy.Close()
            Exit Sub
        End If
        rsDummy.Read()
        txtFrmName.Text = rsDummy!Frmname
        txtCtrlName.Text = rsDummy!Ctrlname
        txtToolTipText.Text = rsDummy!TooltipText

        rsDummy.Close()
    End Sub
End Class