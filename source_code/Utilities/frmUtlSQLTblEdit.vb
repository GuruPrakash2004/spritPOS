Imports C1.Win.C1FlexGrid

Public Class frmUtlSQLTblEdit
    Private Dadapter As oledb.oledbDataAdapter
    Private DSet As DataSet
    Dim dview As DataView
    Dim dtbl As DataTable
    Dim clsSVT As New svtLibrary
    Public strTblName As String
    Public blnEdit As Boolean
    Dim dtQuery As DataTable

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub btnExport_Click(sender As Object, e As EventArgs) Handles btnExport.Click
        If dv.Rows.Count < 1 Then
            Exit Sub
        End If
    End Sub
    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Dim cb As New oledb.oledbCommandBuilder(Dadapter)
        Dadapter.Update(DSet, "Table1")

        Dadapter = New oledb.oledbDataAdapter("select * from " & strTblName, svtConn)
        DSet = New DataSet
        Dadapter.Fill(DSet, "Table1")
        dv.DataSource = DSet.Tables("Table1")
        Exit Sub
    End Sub

    Private Sub frmUtlSQL_KeyUp(sender As Object, e As KeyEventArgs) Handles Me.KeyUp
        If e.KeyCode = Keys.F9 And e.Control = True Then
            Call initFormSize(Me, Me.Text, True, 756, 483, 0, 0)
            MsgBox("Restored Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
        If e.KeyCode = Keys.F8 And e.Control = True Then
            Call SaveActFormSettings(Me, Me.Text)
            'Call SaveActFormVsfDet(Me, Me.Text, "DET", vsfDet)
            MsgBox("Saved Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If

        If e.KeyCode = Keys.C And e.Alt = True Then
            Me.Close()
        End If

        If e.KeyCode = Keys.F12 And e.Alt = True Then
            ' ExportToXL(vsfDet)
        End If
    End Sub

    Private Sub frmUtlSQLTblEdit_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Text = "TableData for " & strTblName
        lblTblName.Text = " No of Records in " & strTblName & " : "

        Dadapter = New oledb.oledbDataAdapter("select top 100 * from " & strTblName, svtConn)
        DSet = New DataSet
        Dadapter.Fill(DSet, "Table1")
        dv.DataSource = DSet.Tables("Table1")
        lblRecords.Text = Val(dv.RowCount - 1)

        ''for c1
        'Dim dt As DataTable
        'Dim datVw As DataView
        'dt = clsSVT.getDATATable("select top 100 * from " & strTblName)
        'datVw = New DataView(dt)
        'vsfDet.DataSource = datVw

    End Sub

    Public Sub TblClick()
        Dim strSQL As String = "", strSQL1 As String = ""
        Dim rs As oledb.oledbDataReader
        txtEdit.Text = ""
        'SplitContainer1.Panel2Collapsed = False
        txtEdit.Text = "Select top 100 "
        'strSQL = "Select top 1000 from" &
        strSQL = "SELECT Column_Name From INFORMATION_SCHEMA.COLUMNS Where TABLE_NAME ='" & strTblName & "'"
        rs = clsSVT.getDataReaderFromSQL(strSQL)
        With rs
            If Not rs.HasRows Then
                rs.Close()
                MsgBox("Table is Empty...", MsgBoxStyle.Information, Me.Text)
                'cboTables.Focus()
                Exit Sub
            End If
            Do While .Read
                'txtEdit.Text = txtEdit.Text & GetVal(!Column_Name, "") & "," & vbCrLf
                strSQL1 = strSQL1 & GetVal(!Column_Name, "") & ","
            Loop
            txtEdit.Text = txtEdit.Text & vbCrLf & strSQL1
        End With
        txtEdit.Text = Mid(txtEdit.Text, 1, Len(txtEdit.Text) - 1)
        txtEdit.Text = txtEdit.Text & vbCrLf & " From " & strTblName
    End Sub

    Private Sub btnReload_Click(sender As Object, e As EventArgs) Handles btnReload.Click
        dtQuery = clsSVT.getDATATable(txtEdit.Text)
        dv.DataSource = dtQuery
        dv.Refresh()
        dv.ScrollBars = ScrollBars.Both
        'dv.AutoSizeCols()
        lblTblName.Visible = True
        'Label4.Visible = True
        lblRecords.Text = Val(dv.RowCount - 1)
    End Sub

    Private Sub dv_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dv.CellClick
    End Sub

    Private Sub dv_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dv.CellContentClick

    End Sub

    Private Sub dv_CellEnter(sender As Object, e As DataGridViewCellEventArgs) Handles dv.CellEnter
        If blnEdit Then
            dv.ReadOnly = False
        Else
            dv.ReadOnly = True
        End If
    End Sub
End Class