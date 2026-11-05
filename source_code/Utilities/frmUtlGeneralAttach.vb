Imports System.IO
Imports C1.Win.C1FlexGrid
Imports System.Windows.Forms

Public Class frmUtlGeneralAttach
    Dim clsSvt As New svtLibrary
    Dim strFileName As String = ""
    Dim tds As New TreeNode
    Dim dt As New DataTable
    Dim dv As DataView

    Const vDetFileName = 0
    Const vDetRmks = 1
    Const vDetKey1 = 2
    Const vDetKey2 = 3
    Const vDetKey3 = 4
    Const vTot = 5

    Dim svtTrn As OleDb.OleDbTransaction

    Private Sub GeneralAttachments_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'tvAttachments.Nodes.Add(GoDownALevel("C:\Test")) '-- mapping for this folder.
        Me.Text = "General Attachments"
        Me.KeyPreview = True
        Call initGridStyles(Me)
        Call fillFormProperties(Me)
        Call ChangeFontSize(Me)
        Call initScreen()
        Call initFormVsfDet(Me, Me.Text, vsfDet, "DET")
        Call syncGridCols()
        lblfrm.Text = Me.Text
        lblfrm.ForeColor = Color.White
        lblSearch.ForeColor = Color.White
        txtSearch.Focus()
        lblfrm.Font = New Font("Microsoft Sans Serif", 14, FontStyle.Bold)
        lblSearch.Font = New Font("Segoe UI", 9, FontStyle.Bold)

        'tvAttachments.Nodes.Clear()
        'tvAttachments.BeginUpdate()
        'tvAttachments.Nodes.Add(GoDownALevel("E:\Arpita"))
        'tvAttachments.EndUpdate()
        'tvAttachments.Refresh()

        Processtree("E:\Arpita")
    End Sub

    'Sub Processtree(ByVal dirPath As String)

    '    tvAttachments.BeginUpdate()
    '    tvAttachments.Nodes.Add(New TreeNode(dirPath))
    '    tvAttachments.EndUpdate()
    '    tvAttachments.Refresh()
    '    Dim objDirInfo As New DirectoryInfo(dirPath)
    '    Dim objSubDirInfo As DirectoryInfo() = objDirInfo.GetDirectories("*.*")
    '    Dim Directoryname As DirectoryInfo
    '    For Each Directoryname In objSubDirInfo
    '        Try
    '            Processtree(Directoryname.FullName)
    '        Catch ex As Exception

    '        End Try
    '    Next
    'End Sub

    Private Sub Processtree(ByVal Dir As String)
        tvAttachments.BeginUpdate()
        tvAttachments.Nodes.Add(New TreeNode(Dir))
        tvAttachments.EndUpdate()
        tvAttachments.Refresh()
        Dim di As New DirectoryInfo(Dir)
        tds = tvAttachments.Nodes.Add(di.Name)
        tds.Tag = di.FullName
        LoadFiles(Dir, tds)
        LoadSubDirectories(Dir, tds)
    End Sub

    Private Sub LoadSubDirectories(ByVal dir As String, ByVal td As TreeNode)
        Dim subdirectoryEntries As String() = Directory.GetDirectories(dir)

        For Each subdirectory As String In subdirectoryEntries
            Dim di As DirectoryInfo = New DirectoryInfo(subdirectory)
            Dim tds As TreeNode = td.Nodes.Add(di.Name)
            tds.Tag = di.FullName
            LoadFiles(subdirectory, tds)
            LoadSubDirectories(subdirectory, tds)
        Next
    End Sub

    Private Sub LoadFiles(ByVal dir As String, ByVal td As TreeNode)

        Dim Files As String() = Directory.GetFiles(dir, "*.*")

        For Each file As String In Files
            Dim fi As FileInfo = New FileInfo(file)
            Dim tds As TreeNode = td.Nodes.Add(fi.Name)

            strFileName = strFileName & "," & fi.Name
            tds.Tag = fi.FullName
            tds.StateImageIndex = 1
        Next
    End Sub

    Private Sub initScreen()
        With vsfDet
            .Rows.Count = 1
            .Cols.Count = vTot

            .SetData(0, vDetFileName, "FileName")
            .SetData(0, vDetRmks, "Remarks")
            .SetData(0, vDetKey1, "Key1")
            .SetData(0, vDetKey2, "Key2")
            .SetData(0, vDetKey3, "Key3")
        End With
    End Sub

    Private Sub vsfDet_BeforeEdit(sender As Object, e As RowColEventArgs) Handles vsfDet.BeforeEdit
        e.Cancel = True
    End Sub

    Private Sub vsfDet_AfterResizeColumn(sender As Object, e As RowColEventArgs) Handles vsfDet.AfterResizeColumn
        Call syncGridCols()
    End Sub

    Private Sub syncGridCols()
        Dim i As Integer
        With vsfDet
            vsfDet.Cols.Count = vTot
            vsfSearch.Cols.Count = vsfDet.Cols.Count
            For i = 0 To .Cols.Count - 1
                vsfSearch.Cols(i).Width = vsfDet.Cols(i).Width
                vsfSearch.Cols(i).Visible = vsfDet.Cols(i).Visible
            Next i
        End With
    End Sub

    Private Sub vsfSearch_AfterScroll(sender As Object, e As C1.Win.C1FlexGrid.RangeEventArgs) Handles vsfSearch.AfterScroll
        Dim intv As Integer = vsfSearch.ScrollPosition.X
        Dim inth As Integer = vsfDet.ScrollPosition.Y
        vsfDet.ScrollPosition = New System.Drawing.Point(intv, inth)
    End Sub

    Private Sub txtSearch_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtSearch.KeyPress
        If e.KeyChar = "'" Then
            e.Handled = True
        End If
    End Sub

    'Private Function GoDownALevel(ByVal location As String) As TreeNode
    '    Dim myNodeCount As Int32

    '    Dim directories() As String = Nothing '-- see if there are any sub directories.
    '    Dim returnNode As TreeNode = New TreeNode(location) '-- set our currenet level.

    '    directories = IO.Directory.GetDirectories(location) '-- get any sub directories.

    '    'myNodeCount = tvAttachments.Nodes(0).GetNodeCount(True)

    '    For Each temp As String In directories '-- for each sub directory
    '        returnNode.Nodes.Add(GoDownALevel(temp)) '-- add any node(s) from examining that sub directory. 
    '        Dim eDirInf As New IO.DirectoryInfo(temp)
    '        Dim iDir As IO.FileInfo() = eDirInf.GetFiles()
    '        Dim aFInfo As IO.FileInfo
    '        'The below loop should only be adding files from the current directory.
    '        For Each aFInfo In iDir 'While we are looping the same directory, we will get the files for this directory. 
    '            returnNode.Nodes.Add(aFInfo.Name.ToString)
    '        Next
    '    Next

    '    Return returnNode
    'End Function

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Call frmMain.CloseSelectedTabPage(Me)
    End Sub

    Private Sub tvAttachments_NodeMouseClick(sender As Object, e As TreeNodeMouseClickEventArgs) Handles tvAttachments.NodeMouseClick
        'With vsfDet
        '    For i As Int32 = 0 To .Rows.Count - 1
        '        '.SetData(i, vDetFileName, "")
        '    Next
        'End With

        'For i As Integer = 1 To tvAttachments.Nodes.Count - 1
        '    Dim node As TreeNode = tvAttachments.Nodes(i)
        '    Dim row As Object() = New Object(node.Nodes.Count - 1) {}

        '    For j As Integer = 0 To node.Nodes.Count - 1
        '        row(j) = node.Nodes(j).Text
        '    Next

        '    'vsfDet.Rows.Add(strFileName)
        '    'vsfDet.SetData(i, vDetFileName, strFileName)
        'Next

        'Dim s As String = strFileName
        ''Dim strWordList() As String
        ''Dim myList As New List(Of String)()

        ''strWordList = myList.ToArray()

        '' Split string based on comma
        'Dim words As String() = s.Split(New Char() {","c})
        '' Dim words As String() = s.Split(New String() {vbCr & vbLf, vbLf}, StringSplitOptions.None)

        '' Use For Each loop over words and display them
        'vsfDet.Rows.Count = words.Length
        'Dim word As String
        ''For Each word In words
        ''    vsfDet.Rows.Count = vsfDet.Rows.Count + 1
        'For i = 1 To vsfDet.Rows.Count - 1
        '    vsfDet.SetData(i, vDetFileName, words(i))
        'Next
        '' Console.WriteLine(word)
        '' Next
    End Sub

    Private Sub tvAttachments_AfterSelect(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles tvAttachments.AfterSelect
        Dim SelNode As String = ""
        Dim strfile As String = ""
        Dim i As Int32
        Dim words As String()

        SelNode = tvAttachments.SelectedNode.Text

        Dim lastNode As TreeNode = tvAttachments.Nodes(tvAttachments.Nodes.Count - 1)
        Dim snd As TreeNode = tvAttachments.SelectedNode
        'tvAttachments.CollapseAll()
        snd.Expand()
        tvAttachments.SelectedNode = snd

        Dim CurrentNode As TreeNode = e.Node
        Dim fullpath As String = CurrentNode.FullPath

        fullpath = "E:\" & fullpath

        Dim dir As String = fullpath
        If Not Directory.Exists(dir) Then Exit Sub
        Dim Files As String() = Directory.GetFiles(dir, "*.*")

        If Files.Length = 0 Then Exit Sub

        For Each file As String In Files
            Dim fi As FileInfo = New FileInfo(file)
            'Dim tds As TreeNode = CurrentNode.Nodes.Add(fi.Name)

            strfile = strfile & "," & fi.Name
            words = strfile.Split(New Char() {","c})
            tds.Tag = fi.FullName
        Next

        vsfDet.Rows.Count = 1
        vsfDet.Rows.Count = words.Length

        For i = 1 To vsfDet.Rows.Count - 1
            vsfDet.SetData(i, vDetFileName, words(i))
        Next i

        Call fillTable()

        'fillGV()

        'Dim fullpath As String = CurrentNode.FullPath

        'fullpath = "E:\" & fullpath
        'Dim dir As String = fullpath
        'MessageBox.Show(fullpath)

        'If LoadFiles(dir, snd) = True Then
        '    'MessageBox.Show(strFileName)
        'End If
    End Sub

    Private Sub fillTable()
        Dim i As Int32
        Dim strSQL As String
        With vsfDet
            Try
                svtTrn = svtConn.BeginTransaction(IsolationLevel.ReadCommitted)
                strSQL = " Delete from UtlGenAttach "
                clsSvt.execSQLStatement(strSQL, svtTrn)

                For i = 1 To .Rows.Count - 1
                    strSQL = "Insert into UtlGenAttach (FileName, Remarks, Key1, Key2, Key3) values("
                    strSQL &= "'" & .GetData(i, vDetFileName) & "',"
                    strSQL &= "'" & .GetData(i, vDetRmks) & "',"
                    strSQL &= "'" & .GetData(i, vDetKey1) & "',"
                    strSQL &= "'" & .GetData(i, vDetKey2) & "',"
                    strSQL &= "'" & .GetData(i, vDetKey3) & "')"
                    If Not clsSvt.execSQLStatement(strSQL, svtTrn) Then
                        svtTrn.Rollback()
                        Exit Sub
                    End If
                Next
                svtTrn.Commit()

            Catch ex As Exception
                svtTrn.Rollback()
            End Try
        End With

        strSQL = "Select * from UtlGenAttach"
        dt = clsSvt.getDATATable(strSQL)
        dv = New DataView(dt)
    End Sub

    'Private Sub fillGV()
    '    DGV.Rows.Clear()
    '    DGV.RowCount = vsfDet.Rows.Count
    '    DGV.ColumnCount = vsfDet.Cols.Count
    '    'Dim intRow As Int32
    '    'Dim intcol As Int32
    '    'For intRow = 0 To vsfDet.Rows.Count - 1
    '    '    DGV.RowCount = DGV.RowCount + 1
    '    '    For intcol = 0 To vsfDet.Cols.Count - 1
    '    '        DGV.Rows(intRow).Cells(intcol).Value = vsfDet.GetData(intRow, intcol)
    '    '        'DGV.Rows(intRow).Cells(0).Value = vsfDet.GetData(intRow, 0)
    '    '        '    DGV.Rows(intRow).Cells(1).Value = vsfDet.GetData(intRow, 1)
    '    '        '    DGV.Rows(intRow).Cells(2).Value = vsfDet.GetData(intRow, 2)
    '    '        '    DGV.Rows(intRow).Cells(3).Value = vsfDet.GetData(intRow, 3)
    '    '        '    DGV.Rows(intRow).Cells(4).Value = vsfDet.GetData(intRow, 4)
    '    '        DGV.ColumnCount = DGV.ColumnCount + 1
    '    '    Next
    '    '    'DGV.ColumnCount = DGV.ColumnCount + 1
    '    'Next

    '    For intcol As Int32 = 0 To vsfDet.Cols.Count - 1
    '        DGV.ColumnCount = DGV.ColumnCount + 1
    '        DGV.Rows(0).Cells(intcol).Value = vsfDet.GetData(0, intcol)

    '    Next

    '    For intRow As Int32 = 1 To vsfDet.Rows.Count - 1

    '        'DGV.Rows(intRow).Cells(0).Value = vsfDet.GetData(intRow, 0)
    '        'DGV.Rows(intRow).Cells(1).Value = vsfDet.GetData(intRow, 1)
    '        'DGV.Rows(intRow).Cells(2).Value = vsfDet.GetData(intRow, 2)
    '        'DGV.Rows(intRow).Cells(3).Value = vsfDet.GetData(intRow, 3)
    '        'DGV.Rows(intRow).Cells(4).Value = vsfDet.GetData(intRow, 4)
    '        DGV.RowCount = DGV.RowCount + 1

    '        For intcol As Int32 = 0 To DGV.ColumnCount - 1
    '            DGV.Rows(intRow).Cells(intcol).Value = vsfDet.GetData(intRow, intcol)
    '        Next

    '    Next

    '    'If DGV.RowCount <> vsfDet.Rows.Count - 1 Then
    '    '    If DGV.Rows(intRow).Cells(intcol).Value = "" Then
    '    '        DGV.RowCount = DGV.RowCount - 1
    '    '        DGV.ColumnCount = DGV.ColumnCount - 1
    '    '    End If
    '    'End If
    'End Sub

    Private Sub fillGV()
        DGV.Rows.Clear()
        DGV.RowCount = vsfDet.Rows.Count
        DGV.ColumnCount = vsfDet.Cols.Count

        For intcol As Int32 = 0 To vsfDet.Cols.Count - 1
            For intRow As Int32 = 0 To vsfDet.Rows.Count - 1
                DGV.Rows(intRow).Cells(intcol).Value = vsfDet.GetData(intRow, intcol)
            Next
        Next
    End Sub

    Private Sub vsfSearch_ChangeEdit(sender As Object, e As EventArgs) Handles vsfSearch.ChangeEdit
        'ToDataTable(DGV)

        If vsfDet.Rows.Count > 2 Then
            Call applyFilterFromGrid()
        Else
            Exit Sub
        End If
    End Sub

    Private Function ToDataTable(ByVal dataGridView As DataGridView) As DataTable
        Dim dt As New DataTable


        For Each dataGridViewColumn As DataGridViewColumn In dataGridView.Columns

            If dataGridViewColumn.Visible Then
                dt.Columns.Add()
            End If
        Next

        Dim cell = New Object(dataGridView.Columns.Count - 1) {}

        For Each dataGridViewRow As DataGridViewRow In dataGridView.Rows

            For i As Integer = 0 To dataGridViewRow.Cells.Count - 1
                cell(i) = dataGridViewRow.Cells(i).Value
            Next

            dt.Rows.Add(cell)

        Next
        dv = dt.DefaultView
        dv = dt.AsDataView
        Return dt
        'DGV.DataSource = dv
        'DGV.DataBindings
    End Function

    Private Function applyFilterFromGrid() As Boolean
        Dim StrQry As String, strValue As String
        Dim i As Integer
        applyFilterFromGrid = False
        Try
            StrQry = ""
            With vsfSearch
                For i = 0 To .Cols.Count - 1
                    strValue = ""
                    If i = .ColSel Then
                        If .Editor.Text <> "" Then
                            strValue = Trim(.Editor.Text)
                        End If
                    Else
                        If .GetData(0, i) <> "" Then
                            strValue = Trim(.GetData(0, i))
                        End If
                    End If
                    If strValue <> "" Then
                        If StrQry <> "" Then StrQry = StrQry & " and "
                        Select Case gDBTypeCurrent
                            Case gDbTypeAccess
                                StrQry = StrQry & vsfDet.GetData(0, i) & " like '*" & strValue & "*'"
                            Case Else
                                If vsfDet.Cols(i).DataType = GetType(Decimal) Or vsfDet.Cols(i).DataType = GetType(Integer) Or vsfDet.Cols(i).DataType = GetType(Date) Then
                                    Try
                                        StrQry = StrQry & "Convert(" & vsfDet.GetData(0, i) & ",'System.String')" & " like '" & strValue & "*'"
                                    Catch ex As Exception
                                        dv.RowFilter = "1 <> 1"
                                        Exit Function
                                    End Try
                                Else
                                    StrQry = StrQry & vsfDet.GetData(0, i) & " like '%" & strValue & "%'"
                                End If
                        End Select
                    End If
                Next i
            End With
            Dim k As Integer = vsfDet.Rows.Count
            dv.RowFilter = StrQry
            Call syncGridCols()
            vsfDet.BeginUpdate()
            vsfDet.DataSource = dv
            vsfDet.EndUpdate()
            Call syncGridCols()
            applyFilterFromGrid = True
        Catch ex As Exception
        End Try
    End Function

    Private Sub frmUtlGeneralAttach_KeyUp(sender As Object, e As KeyEventArgs) Handles Me.KeyUp
        If e.KeyCode = Keys.F9 And e.Control = True Then
            Call initFormVsfDet(Me, Me.Text, vsfDet, "DET")
            MsgBox("Restored Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
        If e.KeyCode = Keys.F8 And e.Control = True Then
            Call SaveActFormSettings(Me, Me.Text)
            Call SaveActFormVsfDet(Me, Me.Text, "DET", vsfDet)
            MsgBox("Saved Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
    End Sub
End Class