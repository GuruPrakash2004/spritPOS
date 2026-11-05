Public Class frmMstSelComp
    Dim clsSVT As New svtLibrary
    Const vCCompID = 0
    Const vCCompName = 1
    Const vCMDB = 2

    Public IntCompID As Integer

    Private Sub LoadCompFromGlobe()
        'Dim lngRowID As Integer
        'Dim rsDummy As oledb.oledbDataReader
        'rsDummy = clsSVT.getDataReaderFromSQL("SELECT * FROM " & gStrGlobeDBName & "..MstGlobecomp ORDER BY CompOrder ASC ")
        'With rsDummy
        '    vsfComp.Rows.Count = 1
        '    Do While .Read
        '        vsfComp.Rows.Count = vsfComp.Rows.Count + 1
        '        lngRowID = vsfComp.Rows.Count - 1
        '        vsfComp.SetData(lngRowID, vCCompID, !GCID)
        '        vsfComp.SetData(lngRowID, vCCompName, !GCName)
        '        vsfComp.SetData(lngRowID, vCMDB, !GCDBName)
        '    Loop
        '    .Close()
        'End With
    End Sub

    Private Sub SelectComp()

        With vsfComp
            If .Rows.Count = 0 Then
                MsgBox("No Company to Select...", vbInformation, Me.Text)
                End
            End If
            If .Row = -1 Then .Row = .Rows.Count - 1
            gStrDBName = Trim(.GetData(.Row, vCMDB))
            If Not clsSVT.execSQLStatement("Use " & gStrDBName) Then
                Exit Sub
            End If
        End With
        frmMain.initGSettings()
        frmMain.InitCompData()
        Call INIStore("SqlConn", "DBName", gStrDBName)
        Me.Dispose()
    End Sub

    Private Sub frmMstSelComp_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        With vsfComp
            .Rows.Count = 1
            .Cols.Count = 3
            '.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
            .SetData(0, vCCompName, "COMPANY")

            .Cols(vCCompID).Visible = False
            .Cols(vCMDB).Visible = False
            '.Rows(0).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter
            SetColAlign(vsfComp, svtAlign.Center, vCCompName)
        End With
        Call LoadCompFromGlobe()
        If vsfComp.Rows.Count = 0 Then
            If MsgBox("No Company Exists!! Do you want to Create a New COMPANY??", vbYesNo + vbInformation, Me.Text) = vbYes Then
                'frmMstNewComp.ShowDialog()
            Else
                End
            End If
        End If
    End Sub

    Private Sub vsfComp_BeforeEdit(sender As Object, e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfComp.BeforeEdit
        e.Cancel = True
    End Sub

    Private Sub vsfComp_DoubleClick(sender As Object, e As System.EventArgs) Handles vsfComp.DoubleClick
        Call SelectComp()
    End Sub

    Private Sub btnok_Click(sender As System.Object, e As System.EventArgs) Handles btnok.Click
        Call SelectComp()
    End Sub

    Private Sub btnClose_Click(sender As System.Object, e As System.EventArgs) Handles btnClose.Click
        If gAppStarted = False Then
            End
        Else
            Me.Close()
        End If
    End Sub

    Private Sub btnSwitchDB_Click(sender As System.Object, e As System.EventArgs) Handles btnSwitchDB.Click
        frmUtlDBConnection.ShowDialog()
        If frmMain.tvMain.Nodes.Count > 0 Then
            Me.Close()
        End If
    End Sub

    'Private Sub vsfComp_KeyPress(sender As Object, e As KeyPressEventArgs) Handles vsfComp.KeyPress
    '    Call SelectComp()

    'End Sub

    Private Sub vsfComp_KeyDown(sender As Object, e As KeyEventArgs) Handles vsfComp.KeyDown
        If vsfComp.Row = vsfComp.Rows.Count - 1 AndAlso e.KeyCode = Keys.Tab Then
            btnSwitchDB.Focus()
        End If
    End Sub

    Private Sub btnClose_Leave(sender As Object, e As EventArgs) Handles btnClose.Leave
        With vsfComp
            .Row = 1
            .Col = 0
            Call showWaitCursor()
        End With
    End Sub

    Private Sub vsfComp_KeyUp(sender As Object, e As KeyEventArgs) Handles vsfComp.KeyUp
        If e.KeyCode = Keys.Enter Then
            Call SelectComp()
        End If
    End Sub

    Private Sub frmMstSelComp_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        If gAppStarted = False Then
            End
        End If
    End Sub
End Class