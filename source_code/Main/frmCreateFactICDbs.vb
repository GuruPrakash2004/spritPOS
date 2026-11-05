Imports System.IO

Public Class frmCreateFactICDbs
    Dim clsSVT As New svtLibrary
    Private Sub btnFactICDBs_Click(sender As Object, e As EventArgs) Handles btnFactICDBs.Click
        If Trim(txtServerName.Text) = "" Then
            MsgBox("Server Name has to be entered! ", MsgBoxStyle.Critical, Me.Text)
            txtServerName.Focus()
            Exit Sub
        End If

        If Trim(txtLoginName.Text) = "" Then
            MsgBox("Database Login Name has to be entered! ", MsgBoxStyle.Critical, Me.Text)
            txtLoginName.Focus()
            Exit Sub
        End If

        If Trim(txtPwd.Text) = "" Then
            MsgBox("Database Login Password has to be entered! ", MsgBoxStyle.Critical, Me.Text)
            txtPwd.Focus()
            Exit Sub
        End If

        If Trim(txtDBName.Text) = "" Then
            MsgBox("Main Database Backup path has to be provided! ", MsgBoxStyle.Critical, Me.Text)
            txtDBName.Focus()
            Exit Sub
        End If

        If Trim(txtGlobeDB.Text) = "" Then
            MsgBox("Globe Database Backup path has to be provided! ", MsgBoxStyle.Critical, Me.Text)
            txtGlobeDB.Focus()
            Exit Sub
        End If

        If Not openMSSqlConn(txtServerName.Text, "master", txtLoginName.Text, txtPwd.Text, False, Nothing) Then
            Exit Sub
        End If
        If Not CreateAndRestoreDB("svtGlobe", txtGlobeDB.Text) Then
            Exit Sub
        End If
        If Not CreateAndRestoreDB("vLQMS", txtDBName.Text) Then
            Exit Sub
        End If
        Call INIStore("SqlConn", "ServerName", txtServerName.Text)
        Call INIStore("SqlConn", "DBName", "vLQMS")
        Call INIStore("SqlConn", "GlobeDBName", "svtGlobe")
        Call INIStore("SqlConn", "DBUserCode", txtLoginName.Text)
        Call INIStore("SqlConn", "DBLoginPwd", svtCrypto.Encrypt(txtPwd.Text))

        gStrServerName = Trim(txtServerName.Text)
        gStrDBName = "vLQMS"
        gStrDBLoginName = Trim(txtLoginName.Text)
        gStrDBLoginPwd = Trim(txtPwd.Text)
    End Sub

    Private Function CreateAndRestoreDB(argDBName As String, argBakFile As String) As Boolean
        CreateAndRestoreDB = False
        Dim strSQL As String
        Dim strLogFileName As String, strDBFileName As String
        Dim strLogPathName As String, strDataPathName As String
        Try
            strLogFileName = clsSVT.getDataStringValue("select name as logFile from sys.master_files " _
                     & " where type_desc = 'LOG' and database_id in (select database_id  from sys.databases where upper(name) = 'master')", "", False)
            strLogPathName = clsSVT.getDataStringValue("select Physical_name as logFile from sys.master_files " _
                     & " where type_desc = 'LOG' and database_id in (select database_id  from sys.databases where upper(name) = 'master')", "", False)
            strLogPathName = strLogPathName.Replace(Path.GetFileName(strLogPathName).Replace("_log.ldf", ""), argDBName)

            strDBFileName = clsSVT.getDataStringValue("select name as logFile from sys.master_files " _
                     & " where type_desc = 'ROWS' and database_id in (select database_id  from sys.databases where upper(name) = 'master')", "", False)
            strDataPathName = clsSVT.getDataStringValue("select Physical_name as logFile from sys.master_files " _
                     & " where type_desc = 'ROWS' and database_id in (select database_id  from sys.databases where upper(name) = 'master')", "", False)
            strDataPathName = strDataPathName.Replace(Path.GetFileName(strDataPathName).Replace(".mdf", ""), argDBName)

            Call clsSVT.execSQLStatement("Create database " & argDBName)

            strSQL = "RESTORE DATABASE " & argDBName
            strSQL = strSQL & " From DISK = '" & argBakFile & "' With File=1,"
            strSQL = strSQL & " Move '" & argDBName & "' TO '" & strDataPathName & "' ,"
            strSQL = strSQL & " Move '" & argDBName & "LOG'  TO '" & strLogPathName & "', REPLACE"
            Call clsSVT.execSQLStatement(strSQL)
            CreateAndRestoreDB = True
        Catch ex As Exception
            MsgBox("Error during database creation" & vbCrLf & ex.Message, MsgBoxStyle.Critical, "Error [createDatabase]")
        End Try

    End Function

    Private Sub btnBrowse_Click(sender As Object, e As EventArgs) Handles btnBrowse.Click
        Cdlg.DefaultExt = "bak"
        Cdlg.Filter = "Excel File (*.bak)|*.bak"
        Cdlg.FileName = "*.bak"

        If Cdlg.ShowDialog() <> DialogResult.OK Then
            Return
            Exit Sub
        End If
        txtDBName.Text = Cdlg.FileName
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub btnGlobeBrowse_Click(sender As Object, e As EventArgs) Handles btnGlobeBrowse.Click
        Cdlg.DefaultExt = "bak"
        Cdlg.Filter = "Excel File (*.bak)|*.bak"
        Cdlg.FileName = "*.bak"

        If Cdlg.ShowDialog() <> DialogResult.OK Then
            Return
            Exit Sub
        End If
        txtGlobeDB.Text = Cdlg.FileName
    End Sub

    Private Sub frmCreateFactICDbs_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Call fillFormProperties(Me)
    End Sub
End Class