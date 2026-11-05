Public Class frmUtlSelBranch

    Dim clsSvt As New svtLibrary

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        frmMain.Opacity = 1
        frmMain.Enabled = True
        Me.Close()
    End Sub

    Private Sub frmUtlChBrch_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        For fadeout = 90 To -10 Step -20
            Me.Opacity = fadeout / 100
            Me.Refresh()
            Threading.Thread.Sleep(50)
        Next
    End Sub

    Private Sub frmUtlChBrch_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim screenWidth As Integer = Screen.PrimaryScreen.Bounds.Width
        Dim screenHeight As Integer = Screen.PrimaryScreen.Bounds.Height

        bindCombo(cboBranch, "BranchName", "BranchName", "Select BranchName from MstBranch where BranchID > 0 and BranchName <> '-'")

        cboBranch.SelectedIndex = -1

        Me.Left = (screenWidth - Me.Width) / 2
        Me.Top = (screenHeight - Me.Height) / 2

        For fadein = 0.0 To 1.1 Step 0.2
            Me.Opacity = fadein
            Me.Refresh()
            Threading.Thread.Sleep(100)
        Next
    End Sub

    Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click
        Dim blnFound As Boolean = False
        Dim lngBranchID As Integer = 0
        Dim strSql As String = ""
        Dim strBranchType As String = ""

        lngBranchID = clsSvt.getDataLongValue("Select BranchID from MstBranch where BranchName  = '" & Trim(cboBranch.Text) & "'", 0, blnFound)

        If blnFound And lngBranchID > 0 Then
            If lngBranchID = 1 Then
                strBranchType = "H"
            Else
                strBranchType = "B"
            End If
            strSql = "Update UtlGlobe set DBType = '" & strBranchType & "', BranchID = " & lngBranchID
            Try
                If clsSvt.execSQLStatement(strSql) Then
                    MsgBox("Connection set to branch " & cboBranch.Text & " successfully!" & vbCrLf & vbCrLf & "Application will restart now...", vbInformation, Me.Text)
                    Application.Restart()
                End If
            Catch ex As Exception
                MsgBox("Error while executing query: " & vbCrLf & ex.Message, vbCritical, Me.Text)
                Exit Sub
            End Try
        Else
            MsgBox("Branch not found in master, please select a valid branch!", vbCritical, Me.Text)
            cboBranch.Focus()
            Exit Sub
        End If

    End Sub
End Class