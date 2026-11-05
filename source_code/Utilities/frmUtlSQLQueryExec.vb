Imports C1.Win.C1FlexGrid
Imports System.IO
Imports System.ComponentModel
Imports C1.Win.C1Input
Imports System.Data.oledb
Imports System.Collections.Specialized
Imports System.Text

Public Class frmUtlSQLQueryExec

    Dim dtQuery As DataTable
    Dim clsSVT As New svtLibrary

    Dim dtSearch As New DataTable
    Dim dvSearch As New DataView
    Private Dadapter As oledb.oledbDataAdapter
    Private DSet As DataSet

    Public cmd As oledb.oledbCommand 

    Const vDetTables = 1
    Const vDetType = 0

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        'Dim cb As New oledb.oledbCommand Builder(Dadapter)
        'Dadapter.Update(DSet, "Table1")

        'Dadapter = New oledb.oledbDataAdapter("select * from " & vDet.GetData(vDet.Row, vDetTables), svtConn)
        'DSet = New DataSet
        'Dadapter.Fill(DSet, "Table1")
        'dv.DataSource = DSet.Tables("Table1")
        'Exit Sub

        'Dim tb As New C1.Win.C1Command.C1DockingTabPage()
        'Dim dockTabMain As New C1.Win.C1Command.C1DockingTab()
        'Me.Dispose()
        'tb.Controls.Clear()
        Call frmMain.CloseSelectedTabPage(Me)
        'frmMain.dockTabMain.SelectedTab.Dispose()
    End Sub

    Private Sub btnView_Click(sender As Object, e As EventArgs) Handles btnExecute.Click
        If InStr(txtQuery.Text, "Delete") > 0 Then
            If MsgBox("Are you sure you want to delete the Record? ", vbCritical + MsgBoxStyle.YesNo + vbDefaultButton2, "Delete Confirmation") = MsgBoxResult.No Then
                Exit Sub
            End If

            If MsgBox("You can't undo this Delete operation ! " &
            "Continue?", vbCritical + MsgBoxStyle.YesNo + vbDefaultButton2, "Delete Confirmation") = MsgBoxResult.No Then
                Exit Sub
            End If
        End If

        dtQuery = clsSVT.getDATATable(txtQuery.Text)
        vsfDet.DataSource = dtQuery
        vsfDet.Refresh()
        vsfDet.ScrollBars = ScrollBars.Both
        vsfDet.AutoSizeCols()
        Label3.Visible = True
        Label4.Visible = True
        Label4.Text = Val(vsfDet.Rows.Count - 1)
    End Sub

    Private Sub vsfDet_BeforeEdit(sender As Object, e As RowColEventArgs) Handles vsfDet.BeforeEdit
        e.Cancel = True
    End Sub

    Private Sub frmUtlSQLQueryExec_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        dv.Visible = False
        GBErr.Enabled = False
        GBErr.Visible = False
        SplitContainer1.Visible = True
        SplitContainer2.Visible = True
        btnExecute.Enabled = False
        btnExport.Enabled = False
        KeyPreview = True
        lblServer.Text = INIGet("SqlConn", "ServerName", "")
        'lblLogin.Text = INIGet("SqlConn", "DBUserCode", "sa")
        lblDBName.Text = INIGet("SqlConn", "DBName", "")

        Call NameType()
    End Sub

    Private Sub txtQuery_KeyUp(sender As Object, e As KeyEventArgs) Handles txtQuery.KeyUp
        GBErr.Visible = False
        txtErrMsg.Visible = False
        txtErrMsg.Text = ""
        Dim dt As New DataTable()

        If txtQuery.Text = "" Then
            btnExecute.Enabled = False
            'vsfDet.Rows.Count = 1
        Else
            btnExecute.Enabled = True
        End If

        If e.KeyCode = Keys.F5 Then
            Try
                Select Case Mid(UCase(Trim(txtQuery.Text)), 1, 6)
                    Case "SELECT"
                        dt = clsSVT.getDATATable(Trim(txtQuery.Text))
                        vsfDet.DataSource = dt
                        vsfDet.Refresh()
                        vsfDet.ScrollBars = ScrollBars.Both
                        vsfDet.AutoSizeCols()
                        Label3.Visible = True
                        Label4.Visible = True
                        Label4.Text = Val(vsfDet.Rows.Count - 1)
                    Case "INSERT", "DELETE", "UPDATE"
                        GBErr.Visible = True
                        txtErrMsg.Visible = True
                        txtErrMsg.Text = clsSVT.execSQLStmtReturnMsg(Trim(txtQuery.Text))
                        txtErrMsg.Text = txtErrMsg.Text & " row(s) affected"
                    Case Else
                        GBErr.Visible = True
                        txtErrMsg.Visible = True
                        txtErrMsg.Text = clsSVT.execSQLStmtReturnMsg(Trim(txtQuery.Text))
                End Select
            Catch ex As Exception
                GBErr.Visible = True
                txtErrMsg.Visible = True
                txtErrMsg.Text = ex.Message
            End Try
        End If
        If e.Control = True And e.KeyCode = Keys.R Then
            If SplitContainer1.Panel2Collapsed Then
                SplitContainer1.Panel2Collapsed = False
            Else
                SplitContainer1.Panel2Collapsed = True
            End If
        End If
    End Sub

    Private Sub vDet_BeforeEdit(sender As Object, e As RowColEventArgs) Handles vDet.BeforeEdit
        e.Cancel = True
    End Sub

    Public Sub clear()
        txtErrMsg.Text = ""
        '  cboTables.Text = ""
    End Sub

    Private Sub btnExport_Click(sender As Object, e As EventArgs) Handles btnExport.Click
        If vsfDet.Rows.Count < 1 Then
            Exit Sub
        End If
        ExportToXL(vsfDet)
    End Sub

    Private Sub vDet_DoubleClick(sender As Object, e As EventArgs) Handles vDet.DoubleClick
        'GBErr.Height = txtErrMsg.Height / 2 - 10
        'GBErr.Width = txtErrMsg.Width
        'GBErr.Top = txtErrMsg.Top / 2
        Dim strSQL As String
        Dim strSQL1 As String = ""
        Dim rs As oledb.oledbDataReader
        Dim dt As New DataTable()
        Dim sConn As oledb.oledbConnection
        'Dim strTblName As String
        'Dim i As Integer
        'strTblName = GetVal(vDet.Rows.Item(i))
        'txtQuery.Text = "Select top 1000 " & vDet.GetData(vDet.Row, vDetTables) & "','" & vbCrLf
        Label4.Text = ""

        Select Case Trim(UCase(vDet.GetData(vDet.Row, vDetType)))
            Case "TABLES"
                txtQuery.Text = ""
                SplitContainer1.Panel2Collapsed = False
                txtQuery.Text = "Select top 1000 "
                'strSQL = "Select top 1000 from" &
                strSQL = "SELECT Column_Name From INFORMATION_SCHEMA.COLUMNS Where TABLE_NAME ='" & vDet.GetData(vDet.Row, vDetTables) & "'"
                rs = clsSVT.getDataReaderFromSQL(strSQL)
                With rs
                    If Not rs.HasRows Then
                        rs.Close()
                        MsgBox("Table is Empty...", MsgBoxStyle.Information, Me.Text)
                        'cboTables.Focus()
                        Exit Sub
                    End If
                    Do While .Read
                        'Dim strSQL1 As String
                        strSQL1 = strSQL1 & GetVal(!Column_Name, "") & ","
                        'txtQuery.Text = txtQuery.Text & GetVal(!Column_Name, "") & ","

                    Loop
                    txtQuery.Text = txtQuery.Text & vbCrLf & strSQL1
                End With
                txtQuery.Text = Mid(txtQuery.Text, 1, Len(txtQuery.Text) - 1)
                txtQuery.Text = txtQuery.Text & vbCrLf & " From " & vDet.GetData(vDet.Row, vDetTables)
                btnExecute.Enabled = True
                With vsfDet
                    Dim da As oledb.oledbDataAdapter
                    'If Keys.KeyCode = Keys.F5 Then
                    Try
                        sConn = svtConn
                        GBErr.Enabled = False
                        GBErr.Visible = False
                        txtErrMsg.Visible = False
                        'strSQL = "SELECT * From " & vDet.GetData(vDet.Row, vDetTables) &
                        da = New oledb.oledbDataAdapter(txtQuery.Text, sConn)
                        da.Fill(dt)
                        vsfDet.DataSource = dt
                        vsfDet.Refresh()
                        vsfDet.ScrollBars = ScrollBars.Both
                        vsfDet.AutoSizeCols()
                        Label4.Text = Val(vsfDet.Rows.Count - 1)
                    Catch ex As Exception
                        GBErr.Enabled = True
                        GBErr.Visible = True
                        txtErrMsg.Text = ex.Message
                        txtErrMsg.Visible = True
                        'GBErr.Height = txtErrMsg.Height
                        'GBErr.Width = txtErrMsg.Width
                        'GBErr.Top = txtErrMsg.Top
                        'GBErr.BringToFront()
                        'txtErrMsg.Clear()
                    End Try
                    'End If

                End With

            Case "VIEWS"
                txtQuery.Text = ""
                strSQL = "SP_helptext '" & vDet.GetData(vDet.Row, vDetTables) & "'"
                rs = clsSVT.getDataReaderFromSQL(strSQL)
                With rs
                    If Not rs.HasRows Then
                        rs.Close()
                        Exit Sub
                    End If
                    Do While .Read
                        SplitContainer1.Panel2Collapsed = True
                        txtQuery.Text = txtQuery.Text & GetVal(!Text, "")
                    Loop
                    txtQuery.Text = Replace(UCase(txtQuery.Text), "CREATE", "ALTER")
                End With

            Case "PROCEDURES"
                txtQuery.Text = ""
                strSQL = "SP_helptext '" & vDet.GetData(vDet.Row, vDetTables) & "'"
                rs = clsSVT.getDataReaderFromSQL(strSQL)
                With rs
                    If Not rs.HasRows Then
                        rs.Close()
                        Exit Sub
                    End If
                    Do While .Read
                        SplitContainer1.Panel2Collapsed = True
                        txtQuery.Text = txtQuery.Text & GetVal(!Text, "")
                    Loop
                    txtQuery.Text = Replace(UCase(txtQuery.Text), "CREATE", "ALTER")
                End With
        End Select
    End Sub

    Private Sub ButtonPin_Click(sender As Object, e As EventArgs) Handles ButtonPin.Click
        SplitContainer2.SplitterDistance = SplitContainer2.SplitterDistance * 3.5
        ButtonPinHide1.Visible = True
        ButtonPin.Visible = False
    End Sub

    Private Sub ButtonPinHide1_Click(sender As Object, e As EventArgs) Handles ButtonPinHide1.Click
        SplitContainer2.SplitterDistance = 42
        ButtonPinHide1.Visible = False
        ButtonPin.Visible = True
    End Sub

    Private Sub frmUtlSQLQueryExec_KeyUp(sender As Object, e As KeyEventArgs) Handles Me.KeyUp
        If e.KeyCode = Keys.F9 And e.Control = True Then
            Call initFormSize(Me, Me.Text, True, 756, 483, 0, 0)
            MsgBox("Restored Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
        If e.KeyCode = Keys.F8 And e.Control = True Then
            Call SaveActFormSettings(Me, Me.Text)
            Call SaveActFormVsfDet(Me, Me.Text, "DET", vsfDet)
            MsgBox("Saved Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If

        If e.KeyCode = Keys.C And e.Alt = True Then
            Call frmMain.CloseSelectedTabPage(Me)
        End If

        If e.KeyCode = Keys.E And e.Alt = True Then
            ExportToXL(vsfDet)
        End If
    End Sub

    Public Sub NameType()
        Dim strSQL As String

        strSQL = "Select 'Tables' as SType,Name from sys.Tables Union  Select 'Views' as SType, Name from Sys.Views Union Select 'Procedures' as SType, Name from Sys.Procedures"

        dtSearch = clsSVT.getDATATable(strSQL)
        dvSearch = New DataView(dtSearch)
        vDet.DataSource = dvSearch

        'vDet.Rows.Count = 1
        'vDet.Cols.Count = 2
        'vDet.SetData(0, vDetTables, "Name")
        'vDet.SetData(0, vDetType, "Type")

        'Dim rs As oledb.oledbDataReader





        'strSQL = "select * from Sys.Tables order by name"
        'rs = clsSVT.getDataReaderFromSQL(strSQL)
        'If Not rs.HasRows Then
        '    rs.Close()
        '    MsgBox("Table is Empty...", MsgBoxStyle.Information, Me.Text)
        '    ' cboTables.Focus()

        'End If

        '    With vDet
        '        Do While rs.Read
        '            'For i As Int32 = 1 To .Rows.Count - 1
        '            .Rows.Count = .Rows.Count + 1
        '            .SetData(.Rows.Count - 1, 0, rs!name)
        '            .SetData(.Rows.Count - 1, 1, "Tables")
        '            '.SetData(.Rows.Count - 1, vTblTable, rs!name)
        '            '.SetData(.Rows.Count - 1, vDetRights, rs!UserRights)
        '            'Next
        '        Loop
        '    End With
        '    '  rs.Close()
        '    strSQL = "select * from Sys.Views order by name"
        'rs = clsSVT.getDataReaderFromSQL(strSQL)
        'If Not rs.HasRows Then
        '    rs.Close()
        '    MsgBox("Table is Empty...", MsgBoxStyle.Information, Me.Text)
        '    '   cboTables.Focus()

        'End If

        '    With vDet
        '        Do While rs.Read
        '            'For i As Int32 = 1 To .Rows.Count - 1
        '            .Rows.Count = .Rows.Count + 1
        '            .SetData(.Rows.Count - 1, 0, rs!name)
        '            .SetData(.Rows.Count - 1, 1, "Views")
        '            '.SetData(.Rows.Count - 1, vTblTable, rs!name)
        '            '.SetData(.Rows.Count - 1, vDetRights, rs!UserRights)
        '            'Next
        '        Loop
        '    End With
        '    ' rs.Close()

        '    strSQL = "select * from Sys.Procedures where name like 'mSp_%' or name like 'USP_%' order by name"
        'rs = clsSVT.getDataReaderFromSQL(strSQL)
        'If Not rs.HasRows Then
        '    rs.Close()
        '    MsgBox("Table is Empty...", MsgBoxStyle.Information, Me.Text)
        '    ' cboTables.Focus()

        'End If

        'With vDet

        '        Do While rs.Read
        '        'For i As Int32 = 1 To .Rows.Count - 1
        '        .Rows.Count = .Rows.Count + 1
        '        .SetData(.Rows.Count - 1, 0, rs!name)
        '        .SetData(.Rows.Count - 1, 1, "Procedures")
        '        '.SetData(.Rows.Count - 1, vTblTable, rs!name)
        '        '.SetData(.Rows.Count - 1, vDetRights, rs!UserRights)
        '        'Next
        '    Loop
        'End With
        'rs.Close()

        vDet.AutoSizeCols()
    End Sub

    Private Function applyFilterFromGrid() As Boolean
        Dim StrQry As String

        applyFilterFromGrid = False
        StrQry = ""
        With vDet
            If txtSearch.Text = "" Then
                Exit Function
            End If

            If txtSearch.Text = " " Then
                dvSearch.RowFilter = "1 = 1"
                Exit Function
            Else
                StrQry = StrQry & "name like '%" & txtSearch.Text & "%'" ' Or "


            End If
            dvSearch.RowFilter = StrQry
        End With
        applyFilterFromGrid = True
    End Function


    Private Sub vsfDet_RowColChange(sender As Object, e As EventArgs) Handles vsfDet.RowColChange
        If vsfDet.Rows.Count > 1 Then
            btnExport.Enabled = True
        Else
            btnExport.Enabled = False
        End If
    End Sub

    Private Sub vDet_MouseClick(sender As Object, e As MouseEventArgs) Handles vDet.MouseClick
        If e.Button = MouseButtons.Right Then
            dv.Visible = True
            Dadapter = New oledb.oledbDataAdapter("select * from " & vDet.GetData(vDet.Row, vDetTables), svtConn)
            DSet = New DataSet
            Dadapter.Fill(DSet, "Table1")
            dv.DataSource = DSet.Tables("Table1")
        End If
    End Sub

    Private Sub tsmEdit_Click(sender As Object, e As EventArgs) Handles tsmEdit.Click
        If vDet.GetData(vDet.Row, vDetType) = "Tables" Then
            With frmUtlSQLTblEdit
                .blnEdit = True
                .strTblName = vDet.GetData(vDet.Row, vDetTables)


                .ShowDialog()
            End With
        End If

    End Sub

    Private Sub tsmSelect_Click(sender As Object, e As EventArgs) Handles tsmSelect.Click
        If vDet.GetData(vDet.Row, vDetType) = "Tables" Or vDet.GetData(vDet.Row, vDetType) = "Views" Then
            With frmUtlSQLTblEdit
                .blnEdit = False
                .strTblName = vDet.GetData(vDet.Row, vDetTables)
                .TblClick()
                .ShowDialog()
            End With
        End If


    End Sub

    Private Sub tsmCreate_Click(sender As Object, e As EventArgs) Handles tsmCreate.Click
        'txtQuery.Text = CreateTable()
        'With frmUtlSQL
        '    .strTblName = vDet.GetData(vDet.Row, vDetTables)
        '    .Procedure()
        '    .ShowDialog()
        'End With


        '        ' Define your database And table you want to script out
        '        Dim dbName, tableName As String
        '        dbName = "vICSVT"
        '        With frmUtlSQL
        '            tableName = .strTblName
        '        End With
        '        'set up the SMO server objects - I'm using "integrated security" here for simplicity
        '        Server srv = New Server();
        'srv.ConnectionContext.LoginSecure = True;
        'srv.ConnectionContext.ServerInstance = "YourSQLServerInstance";

        '' get the database in question
        'Database db = New Database();
        ' db = srv.Databases[dbName]

        'StringBuilder sb = New StringBuilder();

        '' define the scripting options - what options to include Or Not
        '        ScriptingOptions options = New ScriptingOptions();
        'options.ClusteredIndexes = True;
        'options.Default = True;
        'options.DriAll = True;
        'options.Indexes = True;
        'options.IncludeHeaders = True;

        '// script out the table's creation 
        'Table tbl = db.Tables[tableName];

        'StringCollection coll = tbl.Script(options);

        'foreach(String str In coll)
        '{
        '    sb.Append(Str);
        '    sb.Append(Environment.NewLine);
        '}

        '' you can get the string that makes up the CREATE script here
        '' do with this CREATE script whatever you like!
        'String createScript = sb.ToString();



        'Dim srv As New Server
        'Dim db As Database
        'db = New Database
        'srv = New Server(New ServerConnection(gStrServerName, gStrDBLoginName, gStrDBLoginPwd))
        '' srv.SetDefaultInitFields(TypeOf (Table), True)

        'db = srv.Databases("vICSVT")
        'Dim so As New ScriptingOptions
        'so = New ScriptingOptions()
        'so.AllowSystemObjects = False
        'Dim dt = db.EnumObjects(DatabaseObjectTypes.Table)
        'Dim urns As String
        'urns = New Microsoft.SqlServer.Management.Sdk.Sfc.Urn[dt.Rows.Count]
        'get urns of tables to script
        '       For (Int() rowIndex = 0; rowIndex < dt.Rows.Count; ++rowIndex)
        '{
        ' urns[rowIndex] = dt.Rows[rowIndex]["urn"].ToString();
        '}
        ''script tables
        'Dim Scripter As New Scripter
        'Dim Scripts As New Scripter.scripts
        'Scripter = New Scripter(srv)
        'Scripts = Scripter.Script(urns)
        'foreach(script In scripts)
        ''{
        'System.Diagnostics.Trace.WriteLine(script);
        '}
    End Sub



    Private Sub vsfSearchSelectedItem()
        Dim StrQry As String
        If vDet.Rows.Count <= 1 Then Exit Sub

        StrQry = "Select Top 30  " & vDetTables & "," & vDetType & "Name"
        StrQry &= " from sys.tables"
        StrQry &= " where (" & vDetTables & " like '%" & Trim(txtSearch.Text) & "%'"
        StrQry &= " or " & vDetType & "Name Like '%" & Trim(txtSearch.Text) & "%')"
        StrQry &= " order by 1"
        ' Call FillUsers(intID)

        txtSearch.Text = ""
    End Sub


    Private Sub txtSearch_KeyUp(sender As Object, e As KeyEventArgs) Handles txtSearch.KeyUp
        'vsfSearchSelectedItem()

        'Dim StrQry As String

        'StrQry = "Select Top 30  " & vDetTables & "," & vDetType & "Name"
        'StrQry &= " from " &
        'StrQry &= " where (" & vDetTables & " like '%" & Trim(txtSearch.Text) & "%'"
        'StrQry &= " or " & vDetType & "Name Like '%" & Trim(txtSearch.Text) & "%')"
        'StrQry &= " order by 1"
        If e.KeyCode = Keys.Return Then
            Call vsfSearchSelectedItem()
            Exit Sub
        End If

        If e.KeyCode = Keys.Down Then
            vDet.Focus()
            Exit Sub
        End If
    End Sub

    Private Sub txtSearch_TextChanged(sender As Object, e As EventArgs) Handles txtSearch.TextChanged
        applyFilterFromGrid()
    End Sub

    Private Sub vDet_KeyUp(sender As Object, e As KeyEventArgs) Handles vDet.KeyUp
        If e.KeyCode = Keys.Return Then
            'Call vsfSearchSelectedItem()
            Call vDet_DoubleClick(vDet, e)
            Exit Sub
        End If
        If e.KeyCode = Keys.Escape Then
            txtSearch.Text = ""
            'Call setSearchGridHt("")
            Exit Sub
        End If
    End Sub

    Private Sub frmUtlSQLQueryExec_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        txtSearch.Focus()
    End Sub
End Class