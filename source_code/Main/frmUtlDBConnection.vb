Imports System.IO
Imports C1.Win.C1Command
Imports C1.Win
Imports C1.WinImports
Imports System.Data.OleDb
Imports C1.Win.C1FlexGrid
Public Class frmUtlDBConnection
    Dim clsSVT As New svtLibrary

    Dim intFontSize = 9
    Dim svtTrn As OleDb.OleDbTransaction

    Const vDetSlNo = 0
    Const vDetDBType = 1
    Const vDetClient = 2
    Const vDetLocation = 3
    Const vDetServerName = 4
    Const vDetDBName = 5
    Const vDetPassWord = 6
    Const vDetLoginName = 7
    Const vDetRemarks = 8
    Const vDetTotCols = 9
    Private Sub btnBrowse_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowse.Click
        fbd.Description = "Select the directory where your " &
        "Data resides."
        fbd.ShowDialog()
        txtDataPath.Text = fbd.SelectedPath & "\"

    End Sub
    Private Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click

        'Dim tb As New C1.Win.C1Command.C1DockingTabPage()
        'Dim dockTabMain As New C1.Win.C1Command.C1DockingTab()
        'Me.Dispose()
        'tb.Controls.Clear()
        'frmMainTabInterface.dockTabMain.SelectedTab.Dispose()

        Me.Close()
        Me.Dispose()
        'frmMain.tvMain.SelectedNode = Nothing
    End Sub

    Private Sub initscreen()
        With vsfdet
            .Rows.Count = 1
            .Cols.Count = vDetTotCols

            .SetData(0, vDetSlNo, "Slno")
            .SetData(0, vDetDBType, "DB Type")
            .SetData(0, vDetClient, "Client")
            .SetData(0, vDetLocation, "Location")
            .SetData(0, vDetServerName, "ServerName")
            .SetData(0, vDetDBName, "DB Name")
            .SetData(0, vDetPassWord, "Password")
            .SetData(0, vDetLoginName, "Login Name")
            .SetData(0, vDetRemarks, "Remarks")
            SetFlexColor(vsfdet, "")
        End With
        Select Case gDBTypeCurrent
            Case gDbTypeAccess
                If txtDataPath.Text.StartsWith("R:\") Or txtDataPath.Text.StartsWith("\\ssplminisrvr") Then
                    vsfdet.Visible = True
                    btnLoad.Visible = True
                    btnLoad.PerformClick()
                End If
            Case gDbTypeMSSQL
                If (txtServerName.Text.ToUpper() = "SSPLMINISRVR") Or (txtServerName.Text.ToUpper() = "SSPLMINISRVR\SQL19") Or (txtServerName.Text.ToUpper() = "SSPLMINISRVR\SQL2022") Then
                    vsfdet.Visible = True
                    btnLoad.Visible = True
                    btnLoad.PerformClick()
                End If
        End Select
    End Sub
    Private Sub btnConnect_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnConnect.Click
        Select Case gDBTypeCurrent
            Case gDbTypeAccess
                If Not validAccess() Then Exit Sub
                Call INIStore("DBType", "", gDBTypeCurrent)
                Call INIStore("DataPath", "DataFolder", Trim(txtDataPath.Text))
                If txtDataPath.Text.StartsWith("R:\") Or txtDataPath.Text.StartsWith("\\ssplminisrvr") Then
                    SaveNewCon()
                End If
                MsgBox("Data path successfully saved!", MsgBoxStyle.Information, Me.Text)
                gstrDataPath = txtDataPath.Text
                MsgBox("Application will close. " _
                    & " Please run the Application again [To Open with the above Connection].", vbInformation, Me.Text)
                blnAppStarted = True
                Call frmMain.Close()
                End
            Case gDbTypeMSSQL
                If Not validMSSql() Then Exit Sub
                If Not Directory.Exists(txtRptPath.Text) Then txtRptPath.Text = ""
                If Trim(txtRptPath.Text) = "" Then
                    Dim strRptPath As String = clsSVT.getDataStringValue("select SetValue from UtlGSettings Where SetName='RptFilePath'", "", False)
                    If strRptPath = "" Then
                        If MsgBox("Report Path is Empty." & vbNewLine & "Continue....?", MsgBoxStyle.YesNo, Me.Text) = MsgBoxResult.No Then
                            Exit Sub
                        End If
                    Else
                        txtRptPath.Text = strRptPath
                    End If
                Else
                    Call clsSVT.execSQLStatement("Update UtlGSettings set SetValue = '" & txtRptPath.Text & "' where SetName = 'RptFilePath'")
                End If
                If Strings.Right(txtRptPath.Text, 1) <> "\" Then txtRptPath.Text = txtRptPath.Text & "\"
                Dim vsf As New C1.Win.C1FlexGrid.C1FlexGrid
                With vsf
                    .Cols.Fixed = 0
                    .Cols.Count = 3
                    .Rows.Count = 0
                    .Rows.Count = 5

                    .ClipSeparators = ";" & vbCr

                    .SetData(0, 0, "SqlConn")
                    .SetData(0, 1, "ServerName")
                    .SetData(0, 2, txtServerName.Text)

                    .SetData(1, 0, "SqlConn")
                    .SetData(1, 1, "DBName")
                    .SetData(1, 2, txtDBName.Text)

                    .SetData(2, 0, "SqlConn")
                    .SetData(2, 1, "DBUserCode")
                    .SetData(2, 2, txtLoginName.Text)

                    .SetData(3, 0, "SqlConn")
                    .SetData(3, 1, "DBLoginPwd")
                    .SetData(3, 2, svtCrypto.Encrypt(txtPwd.Text))

                    .SetData(4, 0, "SqlConn")
                    .SetData(4, 1, "GLOBEDBNAME")
                    .SetData(4, 2, "")
                    .SaveGrid(txtRptPath.Text & "SvtConn.ini", C1.Win.C1FlexGrid.FileFormatEnum.TextCustom)
                    .SaveGrid(Application.StartupPath & "\ServerDetail.ini", C1.Win.C1FlexGrid.FileFormatEnum.TextCustom)
                    INIStore("CONNPATH", "PATH", txtRptPath.Text)
                End With

                'gBlnINIFileCreation = True
                'Call INIStore("SqlConn", "ServerName", txtServerName.Text)
                'Call INIStore("SqlConn", "DBName", txtDBName.Text)
                'Call INIStore("SqlConn", "DBUserCode", txtLoginName.Text)
                'Call INIStore("SqlConn", "DBLoginPwd", svtCrypto.Encrypt(txtPwd.Text))
                'Call INIStore("DFTMenuGroup", "DefaultMenuGroupID", "")

                Call INIStore("RPTPATH", "RPTPATH", txtRptPath.Text)

                gStrServerName = Trim(txtServerName.Text)
                gStrDBName = Trim(txtDBName.Text)
                gStrDBLoginName = Trim(txtLoginName.Text)
                gStrDBLoginPwd = Trim(txtPwd.Text)
                gstrRptPath = Trim(txtRptPath.Text)
                If (txtServerName.Text.ToUpper() = "SSPLMINISRVR") Or (txtServerName.Text.ToUpper() = "SSPLMINISRVR\SQL19") Or (txtServerName.Text.ToUpper() = "SSPLMINISRVR\SQL2022") Then
                    SaveNewCon()
                End If
                MsgBox("Connection Successfull!", MsgBoxStyle.Information, Me.Text)
                vsf.SaveGrid(Application.StartupPath & "\ServerDetail.ini", C1.Win.C1FlexGrid.FileFormatEnum.TextCustom)
                MsgBox("Application will close. " _
                  & " Please run the Application again [To Open with the above Connection].", vbInformation, Me.Text)
                blnAppStarted = True
                Call frmMain.Close()
                End
        End Select
    End Sub

    Private Sub vsfdet_MouseClick(sender As Object, e As MouseEventArgs) Handles vsfdet.MouseClick
        If tabSQL.Visible Then
            txtServerName.Text = vsfdet.GetData(vsfdet.Row, vDetServerName)
            txtDBName.Text = vsfdet.GetData(vsfdet.Row, vDetDBName)
            txtLoginName.Text = vsfdet.GetData(vsfdet.Row, vDetLoginName)
            txtPwd.Text = vsfdet.GetData(vsfdet.Row, vDetPassWord)
            'txtClient.Text = vsfdet.GetData(vsfdet.Row, vDetClient)
            'txtDesc.Text = vsfdet.GetData(vsfdet.Row, vDetRemarks)
        Else
            txtDataPath.Text = vsfdet.GetData(vsfdet.Row, vDetServerName)
            'txtClientName.Text = vsfdet.GetData(vsfdet.Row, vDetClient)
            'txtDescName.Text = vsfdet.GetData(vsfdet.Row, vDetRemarks)
        End If

    End Sub

    Private Sub SaveNewCon()
        Dim strCONN As String, strsql As String
        svtConn = New OleDbConnection
        Dim rs As OleDb.OleDbDataReader
        strCONN = "Provider = Microsoft.Jet.Oledb.4.0;Data source = R:\Softvent Utl\SVTDBConn.mdb"
        svtConn.ConnectionString = strCONN
        svtConn.Open()

        'Dim intSLNO As Int32 = clsSVT.AutoIncrementedNum("SLNO", "SConn", "", svtTrn, svtConn)
        Dim intSLNO As Int32 = clsSVT.getDataLongValue("Select max(slno+1) as Slno from Sconn", 0, False, svtTrn)

        If clsSVT.getDataStringValue("Select client from SConn where servername='" & Trim(txtServerName.Text) & "' And dbName = '" & Trim(txtDBName.Text) & "'", "", False) = "" Then
            strsql = "Insert into SConn ("
            strsql = strsql & "[SlNo],[DBType],[Client],[Location],[ServerName],[DBName],[PassWord],[LoginName],[Remarks],[ChangedOn]"
            strsql = strsql & ") values ("
            strsql = strsql & intSLNO & ","
            If Me.tabAccess.Visible = True Then
                strsql = strsql & "'ACCESS',"
            Else
                strsql = strsql & "'MSSQL',"
            End If
            strsql = strsql & "'" & Trim(txtClientName.Text) & "',"
            strsql = strsql & "'Softvent',"
            strsql = strsql & "'" & Trim(txtDataPath.Text) & "',"
            strsql = strsql & "'" & Trim(txtDataPath.Text) & "',"
            strsql = strsql & "'',"
            strsql = strsql & "'',"
            strsql = strsql & "'',"
            strsql = strsql & "'" & clsSVT.getDataDateValue("Select now()", Date.Now, False) & "')"
            If Not clsSVT.execSQLStatement(strsql) Then
                'svtTrn.Rollback()
                Exit Sub
            End If
        End If
        If Me.tabAccess.Visible = True Then
            clsSVT.execSQLStatement("Update SConn set ChangedOn ='" & Format(clsSVT.getDataDateValue("Select Now()", Date.Now, False), "dd/MM/yyyy") & "' Where servername = '" & Trim(txtDataPath.Text) & "' And dbName = '" & Trim(txtDataPath.Text) & "' and DBType='ACCESS'")
        Else
            clsSVT.execSQLStatement("Update SConn set ChangedOn ='" & Format(clsSVT.getDataDateValue("Select Now()", Date.Now, False), "dd/MM/yyyy") & "' Where servername= '" & Trim(txtServerName.Text) & "' And dbName = '" & Trim(txtDBName.Text) & "' and DBType='MSSQL'")
        End If
        svtConn.Close()

        'If Me.tabAccess.Visible = True Then
        '    If clsSVT.getDataStringValue("Select client from SConn where servername='" & Trim(txtDataPath.Text) & "' And dbName = '" & Trim(txtDataPath.Text) & "' and DBType='ACCESS'", "", False) = "" Then
        '        strsql = "Insert into SConn ("
        '        strsql = strsql & "[SlNo],[DBType],[Client],[Location],[ServerName],[DBName],[PassWord],[LoginName],[Remarks],[ChangedOn]"
        '        strsql = strsql & ") values ("
        '        strsql = strsql & intSLNO & ","
        '        strsql = strsql & "'ACCESS',"
        '        strsql = strsql & "'" & Trim(txtClientName.Text) & "',"
        '        strsql = strsql & "'Softvent',"
        '        strsql = strsql & "'" & Trim(txtDataPath.Text) & "',"
        '        strsql = strsql & "'" & Trim(txtDataPath.Text) & "',"
        '        strsql = strsql & "'',"
        '        strsql = strsql & "'',"
        '        strsql = strsql & "'',"
        '        strsql = strsql & "'" & getSystemDateTime() & "')"
        '        If Not clsSVT.execSQLStatement(strsql) Then
        '            'svtTrn.Rollback()
        '            Exit Sub
        '        End If
        '    End If
        '    clsSVT.execSQLStatement("Update SConn set ChangedOn ='" & getSystemDateTime() & "' Where servername = '" & Trim(txtDataPath.Text) & "' And dbName = '" & Trim(txtDataPath.Text) & "' and DBType='ACCESS'")
        '    'clsSVT.execSQLStatement("Update SConn set Remarks ='" & Trim(txtDescName.Text) & "' Where servername = '" & Trim(txtDataPath.Text) & "' And dbName = '" & Trim(txtDataPath.Text) & "' and DBType='ACCESS'")
        'ElseIf Me.tabSQL.Visible = True Then
        '    If clsSVT.getDataStringValue("Select client from SConn where servername='" & Trim(txtServerName.Text) & "' And dbName = '" & Trim(txtDBName.Text) & "'", "", False) = "" Then
        '        strsql = "Insert into SConn ("
        '        strsql = strsql & "SlNo,DBType,Client,Location,ServerName,DBName,PassWord,LoginName,Remarks,ChangedOn"
        '        strsql = strsql & ") values ("
        '        strsql = strsql & intSLNO & ","
        '        strsql = strsql & "'MSSQL',"
        '        strsql = strsql & "'" & Trim(txtClientName.Text) & "',"
        '        strsql = strsql & "'Softvent',"
        '        strsql = strsql & "'" & Trim(txtServerName.Text) & "',"
        '        strsql = strsql & "'" & Trim(txtDBName.Text) & "',"
        '        strsql = strsql & "'" & Trim(txtPwd.Text) & "',"
        '        strsql = strsql & "'" & Trim(txtLoginName.Text) & "',"
        '        strsql = strsql & "'',"
        '        strsql = strsql & "'" & getSystemDateTime() & "')"
        '        If Not clsSVT.execSQLStatement(strsql) Then
        '            'svtTrn.Rollback()
        '            Exit Sub
        '        End If
        '    End If
        'svtTrn.Commit()
        'clsSVT.execSQLStatement("Update SConn set ChangedOn ='" & Format(clsSVT.getDataDateValue("Select Now()", Date.Now, False), "dd/MM/yyyy") & "' Where servername= '" & Trim(txtServerName.Text) & "' And dbName = '" & Trim(txtDBName.Text) & "' and DBType='MSSQL'")
        ''clsSVT.execSQLStatement("Update SConn set Client ='" & Trim(txtClient.Text) & "' Where servername= '" & Trim(txtServerName.Text) & "' And dbName = '" & Trim(txtDBName.Text) & "' and DBType='MSSQL'")
        'clsSVT.execSQLStatement("Update SConn set Remarks ='" & Trim(txtDesc.Text) & "' Where servername= '" & Trim(txtServerName.Text) & "' And dbName = '" & Trim(txtDBName.Text) & "' and DBType='MSSQL'")

    End Sub

    Private Function validAccess() As Boolean
        validAccess = False
        If Not Directory.Exists(txtDataPath.Text) Then
            MsgBox("Directory " & txtDataPath.Text & " does not exist!", MsgBoxStyle.Critical, Me.Text)
            Exit Function
        End If
        validAccess = True
    End Function

    Private Function validMSSql() As Boolean
        validMSSql = False
        If Trim(txtServerName.Text) = "" Then
            MsgBox("Server Name has to be entered! ", MsgBoxStyle.Critical, Me.Text)
            txtServerName.Focus()
            Exit Function
        End If

        If Trim(txtDBName.Text) = "" Then
            MsgBox("Database Name has to be entered! ", MsgBoxStyle.Critical, Me.Text)
            txtDBName.Focus()
            Exit Function
        End If
        If Trim(txtLoginName.Text) = "" Then
            MsgBox("Database Login Name has to be entered! ", MsgBoxStyle.Critical, Me.Text)
            txtLoginName.Focus()
            Exit Function
        End If

        If Trim(txtPwd.Text) = "" Then
            MsgBox("Database Login Password has to be entered! ", MsgBoxStyle.Critical, Me.Text)
            txtPwd.Focus()
            Exit Function
        End If

        If Not openMSSqlConn(txtServerName.Text, txtDBName.Text, txtLoginName.Text, txtPwd.Text, False, Nothing) Then
            Exit Function
        End If
        'If Not fnTablePresent(txtGlobeDB.Text & "..MstGlobeComp") Then
        '    MsgBox("Invalid Globe Database!", MsgBoxStyle.Critical, "Connection error")
        '    Exit Function
        'End If
        validMSSql = True
    End Function

    Private Sub frmUtlDBConnection_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        btnInitDBs.Visible = False
        If Not fnTablePresent("SVTGLOBE..MstGlobeComp") Then
            btnInitDBs.Visible = True
        End If
        Call fillCurSettings()
        Select Case gDBTypeCurrent
            Case gDbTypeAccess
                tabDB.TabPages("tabSQL").TabVisible = False
                'tabDB.SelectedTab = tabDB.TabPages("tabAccess")
                PicBoxAccess.Visible = True
                PicBoxSQL.Visible = False
            Case Else
                tabDB.TabPages("tabAccess").TabVisible = False
                'tabDB.SelectedTab = tabDB.TabPages("tabSQL")
                PicBoxAccess.Visible = False
                PicBoxSQL.Visible = True
        End Select
        Call ChangeFontSize(Me)
        Call initscreen()
    End Sub
    Private Sub fillCurSettings()
        Select Case gDBTypeCurrent
            Case gDbTypeAccess
                txtDataPath.Text = INIGet("DataPath", "DataFolder", Application.StartupPath)
            Case gDbTypeMySQL, gDbTypeMSSQL
                Dim vsf As New C1.Win.C1FlexGrid.C1FlexGrid
                With vsf
                    .Cols.Fixed = 0
                    .Cols.Count = 3
                    .Rows.Count = 0
                    .ClipSeparators = ";" & vbCr
                    'If File.Exists(gstrConnectionPath & "SvtConn.ini") Then
                    If File.Exists(Application.StartupPath & "\ServerDetail.ini") Then
                        .LoadGrid(Application.StartupPath & "\ServerDetail.ini", C1.Win.C1FlexGrid.FileFormatEnum.TextCustom)
                    End If

                    For i = 0 To .Rows.Count - 1
                        .SetData(i, 0, Replace(GetVal(.GetData(i, 0)).ToString, vbTab, ""))
                        .SetData(i, 1, Replace(GetVal(.GetData(i, 1)).ToString, vbTab, ""))
                        .SetData(i, 2, Replace(GetVal(.GetData(i, 2)).ToString, vbTab, ""))
                    Next
                End With

                txtServerName.Text = INIGetSerLoc(vsf, "SqlConn", "ServerName", "")
                txtDBName.Text = INIGetSerLoc(vsf, "SqlConn", "DBName", "")
                txtLoginName.Text = INIGetSerLoc(vsf, "SqlConn", "DBUSERCODE", "")
                txtPwd.Text = svtCrypto.Decrypt(INIGetSerLoc(vsf, "SqlConn", "DBLoginPwd", ""))
                txtRptPath.Text = INIGet("RPTPATH", "RPTPATH", "")
                'txtRptPath.Text = clsSVT.getDataStringValue("Select SetValue from UtlGSettings where SetName = 'RptFilePath'", "", False)
        End Select
    End Sub

    Private Sub txtServerName_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtServerName.KeyPress
        If e.KeyChar = "'" Then
            e.Handled = True
            Exit Sub
        End If
    End Sub

    Private Sub txtDBName_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtDBName.KeyPress
        If e.KeyChar = "'" Then
            e.Handled = True
            Exit Sub
        End If
    End Sub

    Private Sub txtGlobeDB_KeyPress(sender As Object, e As KeyPressEventArgs)
        If e.KeyChar = "'" Then
            e.Handled = True
            Exit Sub
        End If
    End Sub

    Private Sub txtLoginName_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtLoginName.KeyPress
        If e.KeyChar = "'" Then
            e.Handled = True
            Exit Sub
        End If
    End Sub

    Private Sub txtPwd_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtPwd.KeyPress
        If e.KeyChar = "'" Then
            e.Handled = True
            Exit Sub
        End If
    End Sub

    Private Sub txtDataPath_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtDataPath.KeyPress
        If e.KeyChar = "'" Then
            e.Handled = True
            Exit Sub
        End If
    End Sub

    Private Sub btnInitDBs_Click(sender As Object, e As EventArgs) Handles btnInitDBs.Click
        frmCreateFactICDbs.ShowDialog()
    End Sub

    Private Sub btnRptPath_Click(sender As Object, e As EventArgs) Handles btnRptPath.Click
        Dim strSQL As String
        fbd.Description = "Select Report Path" &
        fbd.ShowDialog()
        txtRptPath.Text = fbd.SelectedPath & "\"
        If Trim(txtRptPath.Text) <> "" Then
            strSQL = "Update UtlgSettings Set SetValue='" & GetVal(txtRptPath.Text, "") & "' Where SetGroup='Reports' and SetName='RptFilePath'"
            clsSVT.execSQLStatement(strSQL)
            gstrRptPath = txtRptPath.Text
        End If
    End Sub

    Private Sub btnHelpConn_Click(sender As Object, e As EventArgs) Handles btnHelpConn.Click
        'Dim strFile As String = gstrRptPath & "DATA BASE CONNECTIVITY.docx"
        'If File.Exists(strFile) Then
        '    Process.Start(strFile, gstrRptPath)
        'End If

        Dim filepath As String = My.Computer.FileSystem.SpecialDirectories.Temp + "\ErrorsAndSolutionsvLQMSSoftware.pdf"

        IO.File.WriteAllBytes(filepath, My.Resources.ErrorsAndSolutionsvLQMSSoftware)

        Process.Start(filepath)

    End Sub

    Private Sub btnLoad_Click(sender As Object, e As EventArgs) Handles btnLoad.Click
        Dim blnLoad As Boolean
        Dim strCONN As String
        svtConn = New OleDbConnection
        Dim rs As OleDb.OleDbDataReader
        strCONN = "Provider = Microsoft.Jet.Oledb.4.0;Data source = R:\Softvent Utl\SVTDBConn.mdb"
        Try
            svtConn.ConnectionString = strCONN
            svtConn.Open()
            If Me.tabAccess.Visible = True Then
                Dim Strsql As String = "SELECT * FROM SConn WHERE DBType='ACCESS' order by 1 "
                With vsfdet
                    vsfdet.Rows.Count = 1
                    rs = clsSVT.getDataReaderFromSQL(Strsql)
                    Do While rs.Read
                        .Rows.Count = .Rows.Count + 1
                        .SetData(.Rows.Count - 1, vDetSlNo, .Rows.Count - 1)
                        .SetData(.Rows.Count - 1, vDetDBType, rs!DBType)
                        .SetData(.Rows.Count - 1, vDetClient, rs!Client)
                        .SetData(.Rows.Count - 1, vDetLocation, rs!Location)
                        .SetData(.Rows.Count - 1, vDetServerName, rs!ServerName)
                        .SetData(.Rows.Count - 1, vDetDBName, rs!DBName)
                        .SetData(.Rows.Count - 1, vDetPassWord, rs!PassWord)
                        .SetData(.Rows.Count - 1, vDetLoginName, rs!LoginName)
                        .SetData(.Rows.Count - 1, vDetRemarks, rs!Remarks)
                    Loop
                    rs.Close()
                End With
            End If

            If Me.tabSQL.Visible = True Then
                Dim Strsql As String = "SELECT * FROM SConn WHERE DBType='MSSQL' order by 1"
                With vsfdet
                    .Rows.Count = 1
                    rs = clsSVT.getDataReaderFromSQL(Strsql)
                    Do While rs.Read
                        .Rows.Count = .Rows.Count + 1
                        .SetData(.Rows.Count - 1, vDetSlNo, .Rows.Count - 1)
                        .SetData(.Rows.Count - 1, vDetDBType, rs!DBType)
                        .SetData(.Rows.Count - 1, vDetClient, rs!Client)
                        .SetData(.Rows.Count - 1, vDetLocation, rs!Location)
                        .SetData(.Rows.Count - 1, vDetServerName, rs!ServerName)
                        .SetData(.Rows.Count - 1, vDetDBName, rs!DBName)
                        .SetData(.Rows.Count - 1, vDetPassWord, rs!PassWord)
                        .SetData(.Rows.Count - 1, vDetLoginName, rs!LoginName)
                        .SetData(.Rows.Count - 1, vDetRemarks, rs!Remarks)
                    Loop
                    rs.Close()
                End With

            End If
            svtConn.Close()
            blnLoad = True
            'Call initFormVsfDet(Me, Me.Text, vsfdet, "DET")
        Catch ex As Exception
            blnLoad = False
            MsgBox("Error in Connecting to Database." & vbCrLf & ex.Message, MsgBoxStyle.Critical, "Error in Connecting to DB")
            Exit Sub
        End Try
    End Sub
End Class