Imports C1.Win.C1FlexGrid

Public Class FrmSelFinYear
    Const vsfFdate = 1
    Const vsfTdate = 2
    Const vsfActive = 3

    Dim clsSVT As New svtLibrary
    Dim strMode As String
    Dim Mode As String
    Dim intLastCol As Integer

    Private Sub initScreen()
        With vsfFnlYr
            .Cols.Count = 4
            .Rows.Count = 2

            .SetData(0, vsfFdate, "From Date")
            .SetData(0, vsfTdate, "To Date")
            .SetData(0, vsfActive, "Active")

            .Cols(0).TextAlign = TextAlignEnum.CenterCenter
            .Cols(1).TextAlign = TextAlignEnum.CenterCenter
            .Cols(2).TextAlign = TextAlignEnum.CenterCenter

            .Cols(vsfTdate).DataType = GetType(Date)
            .Cols(vsfFdate).DataType = GetType(Date)
            .Cols(vsfActive).DataType = GetType(Char)
            .Cols(vsfActive).AllowEditing = False

        End With

        Call fillGrid()
        Call SetColWidth(vsfFnlYr, -1, 150, 150, 20)
        Call initGridStyles(Me)
        Call SetFlexColor(vsfFnlYr, "", vsfTdate, vsfFdate, vsfActive)

        If gUserCode <> "ADMIN" Then btnSave.Visible = False

    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
        ' Call frmMain.CloseSelectedTabPage(Me)
    End Sub

    Private Sub FrmMstComp_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        lblFrm.Text = "Select Financial Year"
        Call initScreen()
        Call GetFinYrActiveFocus(vsfFnlYr)
        initGridStyles(Me)
        'btnSelFinyear.Visible = False
        ChangeColor()
        Call ChangeFontSize(Me)
        lblFrm.Font = New Font("Microsoft Sans Serif", 14, FontStyle.Bold)

    End Sub

    Public Sub fillGrid()
        Dim rsTemp As oledb.oledbDataReader
        Dim strSql As String

        strSql = "Select * from MstCompFinYear Order by yrFdate"
        rsTemp = clsSVT.getDataReaderFromSQL(strSql)

        vsfFnlYr.Rows.Count = 1
        With rsTemp
            Do While .Read()
                With vsfFnlYr
                    .Rows.Count = .Rows.Count + 1
                    .SetData(.Rows.Count - 1, vsfFdate, GetVal(rsTemp("yrFdate"), ""))
                    .SetData(.Rows.Count - 1, vsfTdate, GetVal(rsTemp("yrTdate"), ""))
                    .SetData(.Rows.Count - 1, vsfActive, GetVal(rsTemp("Active"), ""))
                End With
            Loop
            .Close()
        End With
    End Sub

    Private Sub vsfFnlYr_BeforeEdit(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfFnlYr.BeforeEdit
        e.Cancel = True
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim strSql As String

        With vsfFnlYr
            If .Row < 1 Then Exit Sub
            If .GetData(.Row, vsfActive) = "Y" Then
                Exit Sub
            Else
                If MsgBox("Do you want to set this as your financial year ?", vbInformation + vbYesNo, Me.Text) = vbYes Then
                    strSql = "Update MstCompFinYear set Active = 'N'"
                    clsSVT.execSQLStatement(strSql)

                    strSql = "Update MstCompFinYear set Active = 'Y' where yrFdate = " & getDateFormatInCond(.GetData(.Row, vsfFdate)) & ""
                    clsSVT.execSQLStatement(strSql)
                    Call fillGrid()
                    MsgBox("Successfully set as active!", MsgBoxStyle.Information, Me.Text)
                End If
            End If
        End With

        Call frmMain.initGSettings()

        'Me.Close()
    End Sub

    Private Sub vsfFnlYr_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles vsfFnlYr.DoubleClick
        Call selectFinYear()
        Me.Close()
    End Sub

    Private Sub GetFinYrActiveFocus(ByVal argvsf As C1FlexGrid)
        With argvsf
            For i = 0 To .Rows.Count - 1
                If .GetData(i, vsfActive) = "Y" Then
                    .Col = vsfActive
                    .Row = i
                    .Focus()
                End If
            Next i
        End With
    End Sub

    Private Sub btnSelFinyear_Click(sender As System.Object, e As System.EventArgs) Handles btnSelFinyear.Click
        Call selectFinYear()
        'vsfFnlYr.Focus()
        Dim tb As New C1.Win.C1Command.C1DockingTabPage()
        Dim dockTabMain As New C1.Win.C1Command.C1DockingTab()
        Me.Dispose()
        tb.Controls.Clear()
        'frmMain.dockTabMain.SelectedTab.Dispose()
    End Sub

    Private Sub selectFinYear()
        Dim rsDummy As oledb.oledbDataReader
        With vsfFnlYr
            If .Row < 1 Then Exit Sub

        End With

        rsDummy = clsSVT.getDataReaderFromSQL("Select * from MstCompFinYear where YrFDate = " & getDateFormatInCond(vsfFnlYr.GetData(vsfFnlYr.Row, vsfFdate)))
        With rsDummy
            If .HasRows Then
                .Read()
                gSettings.gCompEndDt = CDate(GetVal(!yrTDate, getSystemDate))
                gSettings.gCompStDt = CDate(GetVal(!yrfDate, getSystemDate))
                gSettings.gCompFinYear = Microsoft.VisualBasic.Strings.Right(Year(gSettings.gCompStDt).ToString, 2)
            End If
            .Close()
        End With
        frmMain.lblFin.Text = "(" & "FinYear : " & gSettings.gCompStDt & " - " & gSettings.gCompEndDt & ")"
        frmMain.sbFinYear.Text = "FinYear : " & gSettings.gCompStDt & " - " & gSettings.gCompEndDt
        'Me.Close()
        Dim tb As New C1.Win.C1Command.C1DockingTabPage()
        Dim dockTabMain As New C1.Win.C1Command.C1DockingTab()
        Me.Dispose()
        tb.Controls.Clear()
        'frmMain.dockTabMain.SelectedTab.Dispose()
    End Sub

    Private Sub vsfFnlYr_KeyUp(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles vsfFnlYr.KeyUp
        With vsfFnlYr
            If .Row = .Rows.Count - 1 Then
                btnSave.Focus()
            End If
        End With
    End Sub
    Private Sub ChangeColor()
        btnSave.ForeColor = Color.Black()
        btnCancel.ForeColor = Color.Black

    End Sub

    Private Sub vsfFnlYr_Click(sender As Object, e As EventArgs) Handles vsfFnlYr.Click

    End Sub
End Class