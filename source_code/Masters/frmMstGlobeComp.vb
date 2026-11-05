Imports C1.Win.C1FlexGrid
Imports System.IO

Public Class frmMstGlobeComp
    Dim clsSVT As New svtLibrary
    Public strRights As String
    Public strfrmCaption As String
    Dim Mode As String
    Dim intNo As Integer
    Const vDetGcId = 0
    Const vDetCompName = 1
    Const vDetDBName = 2
    Const vDetCompDefault = 3
    Dim blnFound As Boolean

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub frmMstGlobeComp_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        pnlAdd.Visible = False
        KeyPreview = True
        'gStrGlobeDBName = "svtGlobe"
        initGridStyles(Me)
        Call initFormSize(Me, Me.Text, True, 640, 440, 0, 0)
        Call initScreen()
        Call fillGrid()
        Call initFormVsfDet(Me, Me.Text, vsfDet, "DET")
        Call fillFormProperties(Me)
        pnlBtns.Visible = True
        Call ChangeFontSize(Me)

    End Sub

    Private Sub initScreen()
        With vsfDet
            .Rows.Count = 2
            .Cols.Count = 4
            .SetData(0, vDetGcId, "GcId")
            .SetData(0, vDetCompName, "Company")
            .SetData(0, vDetDBName, "Database")
            .SetData(0, vDetCompDefault, "Default")
            .AllowResizing = AllowResizingEnum.Columns
        End With
        initGridStyles(Me)
        Call SetHideCol(vsfDet, True, vDetGcId)
        'Call SetColWidth(vsfDet, 0, 300, 100)
        Call SetFlexColor(vsfDet, "", vDetCompName, vDetDBName, vDetCompDefault)
    End Sub

    Private Sub fillGrid()
        Dim rsTemp As oledb.oledbDataReader
        Dim strSql As String
        strSql = "Select * from " & gStrGlobeDBName & "..MstGlobeComp Order by CompOrder"
        rsTemp = clsSVT.getDataReaderFromSQL(strSql)
        vsfDet.Rows.Count = 1

        Do While rsTemp.Read()
            With vsfDet
                vsfDet.Rows.Count = vsfDet.Rows.Count + 1
                .SetData(vsfDet.Rows.Count - 1, vDetGcId, GetVal(rsTemp("GCID"), 0))
                .SetData(vsfDet.Rows.Count - 1, vDetCompName, GetVal(rsTemp("GCName"), ""))
                .SetData(vsfDet.Rows.Count - 1, vDetDBName, GetVal(rsTemp("GCDBName"), ""))
                .SetData(vsfDet.Rows.Count - 1, vDetCompDefault, GetVal(rsTemp("CompDefault"), "N"))
            End With
        Loop
        rsTemp.Close()
        vsfDet.AutoSizeCols()
    End Sub

    Private Sub vsfDet_BeforeEdit(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfDet.BeforeEdit
        e.Cancel = True
    End Sub

    Public Sub initForm(ByVal argDocID As Integer, ByVal argPartyID As Integer)
        Call initScreen()
    End Sub

    Private Sub frmMstGlobeComp_KeyUp(sender As Object, e As KeyEventArgs) Handles Me.KeyUp
        If e.KeyCode = Keys.F9 And e.Control = True Then
            Call initFormSize(Me, Me.Text, True, 854, 440, 0, 0)
            Call initFormVsfDet(Me, Me.Text, vsfDet, "DET")
            MsgBox("Restored Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
        If e.KeyCode = Keys.F8 And e.Control = True Then
            Call SaveActFormSettings(Me, Me.Text)
            Call SaveActFormVsfDet(Me, Me.Text, "DET", vsfDet)
            MsgBox("Saved Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
    End Sub

    Private Sub txtAddCompName_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtAddCompName.KeyPress
        If e.KeyChar = "'" Then
            e.Handled = True
        End If
        If Not KeyValidate("NonSpclChar", e.KeyChar) Then
            e.Handled = True
        End If
        If e.KeyChar = " " Then
            e.Handled = False
        End If
    End Sub

    Private Sub txtDBName_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtDBName.KeyPress
        If e.KeyChar = "'" Then
            e.Handled = True
        End If
        If Not KeyValidate("NonSpclChar", e.KeyChar) Then
            e.Handled = True
        End If
    End Sub

    Private Sub frmMstGlobeComp_ResizeEnd(sender As Object, e As EventArgs) Handles Me.ResizeEnd
        vsfDet.AutoSizeRows()
        vsfDet.Cols(vDetDBName).Width = vsfDet.Width - vsfDet.Cols(1).Width
    End Sub

    Private Sub vsfDet_AfterResizeColumn(sender As Object, e As RowColEventArgs) Handles vsfDet.AfterResizeColumn
        vsfDet.AutoSizeRows()
    End Sub

    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        pnlAdd.BringToFront()
        pnlAdd.Visible = True
        pnlBtns.Visible = False
        vsfDet.Visible = False
        lblCompTitle.Text = "Creating new Company"

        txtBackup.Visible = True
        lblCreateFrmBackup.Visible = True
        btnbackup.Visible = True
        txtId.Text = ""
        txtAddCompName.Text = ""
        txtDBName.Text = ""
        chkDef.Checked = False
        txtBackup.Text = ""
        txtAddCompName.Focus()
        Mode = "A"
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        pnlBtns.Visible = True
        pnlAdd.Visible = False
        vsfDet.Visible = True
        txtAddCompName.Text = ""
        txtDBName.Text = ""
        chkDef.Checked = False
        Mode = "V"
    End Sub

    Private Sub txtBackup_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtBackup.KeyPress
        If e.KeyChar = "'" Then
            e.Handled = True
        End If
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click

        Dim strSQL As String = ""
        stringReplace(Me)

        If txtAddCompName.Text = "" Then
            MsgBox("Enter Company Name", vbInformation, Me.Text)
            Exit Sub
        End If
        If txtDBName.Text = "" Then
            MsgBox("Enter Database Name", vbInformation, Me.Text)
            Exit Sub
        End If
        If Mode = "A" Then
            'If Trim(txtBackup.Text) = "" Then
            '    MsgBox("Select File Name", vbInformation, Me.Text)
            '    Exit Sub
            'End If
        End If

        Dim ValidChars As String = "ABCDEFGHIJKLMNOPQRSTUVWXYZ" _
                                   & "abcdefghijklmnopqrstuvwxyz" _
                                   & "01234567890"
        Dim ValidChar As String = "ABCDEFGHIJKLMNOPQRSTUVWXYZ" _
                                   & "abcdefghijklmnopqrstuvwxyz" _
                                   & "01234567890" _
                                   & " "
        For Each chr As Char In txtDBName.Text
            If InStr(ValidChars, chr) <= 0 Then
                MsgBox("Not a valid Database Name. Special Charaters and Spaces not allow", vbInformation, Me.Text)
                Exit Sub
            End If
        Next
        For Each chr As Char In txtAddCompName.Text
            If InStr(ValidChar, chr) <= 0 Then
                MsgBox("Not a valid Company Name. Special Charaters are not allow", vbInformation, Me.Text)
                Exit Sub
            End If
        Next

        Call clsSVT.getDataStringValue("Select GCDBName from  " & gStrGlobeDBName & "..MstGlobeComp where GCDBName = '" & Trim(txtDBName.Text) & "'  and GCId <> " & Val(txtId.Text), 0, blnFound)
        If blnFound Then
            MsgBox("DataBase already exists..", vbInformation, Me.Text)
            txtDBName.Focus()
            Exit Sub
        End If
        Call clsSVT.getDataStringValue("Select GCName from  " & gStrGlobeDBName & "..MstGlobeComp where GCName = '" & Trim(txtAddCompName.Text) & "'  and GCId <> " & Val(txtId.Text), 0, blnFound)
        If blnFound Then
            MsgBox("Company already exists..", vbInformation, Me.Text)
            txtAddCompName.Focus()
            Exit Sub
        End If
        Dim svtTrn As oledb.oledbTransaction = Nothing
        Try
            Dim strFilePath As String = gstrRptPath & "Temp\"
            If File.Exists(strFilePath) Then
                File.Delete(strFilePath)
            End If


            File.Move(txtBackup.Text, strFilePath & "test")

            If Not createDatabase(txtDBName.Text, strFilePath) Then
                Exit Sub
            End If

            svtTrn = svtConn.BeginTransaction(IsolationLevel.ReadCommitted)
            If chkDef.Checked Then
                clsSVT.execSQLStatement("Update " & gStrGlobeDBName & "..MstGlobeComp set CompDefault = 'N'", svtTrn)
            End If
            If Mode = "A" Then
                strSQL = "Insert into " & gStrGlobeDBName & "..MstGlobeComp (GCId, GCName, GCDBName, CompDefault,CompOrder) values ("
                strSQL = strSQL & clsSVT.AutoIncrementedNum("GCID", gStrGlobeDBName & "..MstGlobeComp", "", svtTrn) & ","
                strSQL = strSQL & "'" & Trim(txtAddCompName.Text) & "',"
                strSQL = strSQL & "'" & Trim(txtDBName.Text) & "',"
                strSQL = strSQL & getCheckString(chkDef, True) & ","
                strSQL = strSQL & clsSVT.AutoIncrementedNum("CompOrder", gStrGlobeDBName & "..MstGlobeComp", "", svtTrn) & ")"
            Else
                strSQL = "Update " & gStrGlobeDBName & "..MstGlobeComp"
                strSQL = strSQL & " Set  GCName" & " = '" & Trim(txtAddCompName.Text) & "',"
                strSQL = strSQL & " CompDefault = " & getCheckString(chkDef, True) & ","
                strSQL = strSQL & " GCDBName " & " = '" & Trim(txtDBName.Text) & "'"
                strSQL = strSQL & " where  GCId" & " = " & Val(txtId.Text) & ""
            End If
            If Not clsSVT.execSQLStatement(strSQL, svtTrn) Then
                svtTrn.Rollback()
                Exit Sub
            End If
            svtTrn.Commit()
            MsgBox("Data Saved Successfully", MsgBoxStyle.Information, Me.Text)
            fillGrid()
            vsfDet.Visible = True
            pnlAdd.Visible = False
            pnlBtns.Visible = True
        Catch ex As Exception
            MsgBox("Error during Save" & vbCrLf & ex.Message, MsgBoxStyle.Critical, Me.Text)
            svtTrn.Rollback()
        End Try
    End Sub

    Private Sub btnEdit_Click(sender As Object, e As EventArgs) Handles btnEdit.Click
        txtAddCompName.Text = ""
        txtDBName.Text = ""
        txtBackup.Text = ""
        chkDef.Checked = False
        If vsfDet.Rows.Count <= 1 Then Exit Sub
        pnlAdd.BringToFront()
        pnlAdd.Visible = True
        txtBackup.Visible = False
        lblCompTitle.Text = "Editing Company"
        vsfDet.Visible = False
        pnlBtns.Visible = False
        lblCreateFrmBackup.Visible = False
        btnbackup.Visible = False
        txtId.Text = vsfDet.GetData(vsfDet.Row, vDetGcId)
        txtAddCompName.Text = vsfDet.GetData(vsfDet.Row, vDetCompName)
        txtDBName.Text = vsfDet.GetData(vsfDet.Row, vDetDBName)
        Call setCheckBox(chkDef, vsfDet.GetData(vsfDet.Row, vDetCompDefault))
        Mode = "M"
    End Sub

    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        Dim svtTrn As oledb.oledbTransaction = Nothing
        pnlAdd.Visible = False
        txtId.Text = vsfDet.GetData(vsfDet.Row, vDetGcId)
        If MsgBox("Are you sure you want to delete the '" & vsfDet.GetData(vsfDet.Row, vDetCompName) _
                 & "'", vbCritical + MsgBoxStyle.YesNo + vbDefaultButton3, "Delete Confirmation") = MsgBoxResult.Yes Then
            Try
                svtTrn = svtConn.BeginTransaction(IsolationLevel.ReadCommitted)
                clsSVT.execSQLStatement("delete from " & gStrGlobeDBName & "..MstGlobeComp where GCID=" & txtId.Text, svtTrn)
                svtTrn.Commit()
                Call DeleteDB(vsfDet.GetData(vsfDet.Row, vDetDBName))

                fillGrid()
                MsgBox("Deleted Successfully", MsgBoxStyle.Information, Me.Text)

            Catch ex As Exception
                svtTrn.Rollback()
                Exit Sub
            End Try
        End If
    End Sub

    Private Sub DeleteDB(argDBName As String)
        Dim strBackupPath As String, strFile As String, strSQL As String
        strBackupPath = getGsettings("Database", "SQLBACKUPPATH")

        strFile = strBackupPath & "\db" & argDBName & getSystemDateTime.ToString("ddMMMyyyy_hhmmss") & ".Bak"

        strSQL = "BACKUP DATABASE " & argDBName & " TO DISK = '" & strFile & "'  WITH FORMAT;"
        If clsSVT.execSQLStatement(strSQL) Then
            clsSVT.execSQLStatement("drop database " & argDBName)
        End If
    End Sub

    Private Sub btnbackup_Click(sender As Object, e As EventArgs) Handles btnbackup.Click
        Dim idb As New OpenFileDialog
        If Not chkUserRights(strRights, chkRightsPos.AddRights, Me.Text) Then Exit Sub
        idb.Title = "Select your Backup file."
        idb.Filter = "Backup Files|*.bak;"
        idb.RestoreDirectory = False
        If idb.ShowDialog() = DialogResult.OK Then
            txtBackup.Text = idb.FileName
        End If
    End Sub
    Private Function createDatabase(argDBName As String, argBakFile As String) As Boolean
        Dim strSQL As String
        Dim strLogFileName As String, strDBFileName As String
        Dim strLogPathName As String, strDataPathName As String
        createDatabase = False
        Try
            strLogFileName = clsSVT.getDataStringValue("select name as logFile from sys.master_files " _
                     & " where type_desc = 'LOG' and database_id in (select database_id  from sys.databases where upper(name) = '" & UCase(gStrDBName) & "')", "", False)
            strLogPathName = clsSVT.getDataStringValue("select Physical_name as logFile from sys.master_files " _
                     & " where type_desc = 'LOG' and database_id in (select database_id  from sys.databases where upper(name) = '" & UCase(gStrDBName) & "')", "", False)
            strLogPathName = strLogPathName.Replace(Path.GetFileName(strLogPathName).Replace("_log.ldf", ""), argDBName)

            strDBFileName = clsSVT.getDataStringValue("select name as logFile from sys.master_files " _
                     & " where type_desc = 'ROWS' and database_id in (select database_id  from sys.databases where upper(name) = '" & UCase(gStrDBName) & "')", "", False)
            strDataPathName = clsSVT.getDataStringValue("select Physical_name as logFile from sys.master_files " _
                     & " where type_desc = 'ROWS' and database_id in (select database_id  from sys.databases where upper(name) = '" & UCase(gStrDBName) & "')", "", False)
            strDataPathName = strDataPathName.Replace(Path.GetFileName(strDataPathName).Replace(".mdf", ""), argDBName)

            Call clsSVT.execSQLStatement("Create database " & argDBName)

            strSQL = "RESTORE DATABASE " & argDBName
            strSQL = strSQL & " From DISK = '" & argBakFile & "' With File=1,"
            strSQL = strSQL & " Move '" & strDBFileName & "' TO '" & strDataPathName & "' ,"
            strSQL = strSQL & " Move '" & strLogFileName & "'  TO '" & strLogPathName & "', REPLACE"
            Call clsSVT.execSQLStatement(strSQL)
            createDatabase = True
        Catch ex As Exception
            MsgBox("Error during database creation" & vbCrLf & ex.Message, MsgBoxStyle.Critical, "Error [createDatabase]")
        End Try
    End Function

    Private Sub vsfDet_KeyUp(sender As Object, e As KeyEventArgs) Handles vsfDet.KeyUp
        If vsfDet.Row = vsfDet.Rows.Count - 1 AndAlso vsfDet.Col = vsfDet.Cols.Count - 1 Then
            btnAdd.Focus()
        End If
    End Sub

    Private Sub btnClose_Leave(sender As Object, e As EventArgs) Handles btnClose.Leave
        With vsfDet
            .Row = 1
            .Col = 1
            Call showWaitCursor()
        End With
    End Sub

End Class
