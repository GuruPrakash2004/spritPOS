Public Class frmRptsDrops
    Dim clsSVT As New svtLibrary

    Const vRptSlNo = 0
    Const vRptName = 1

    Private Sub btnClose_Click(sender As Object, e As System.EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub frmRptDayLtr_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        With vsfRpt
            .Rows.Count = 5
            .Cols.Count = 2
            .SetData(0, vRptSlNo, "SlNo")
            .SetData(0, vRptName, "Report Name")

            .SetData(1, vRptSlNo, "1")
            .SetData(1, vRptName, "Sales Summary Report")

            .SetData(2, vRptSlNo, "2")
            .SetData(2, vRptName, "Petty Cash Report")

            .SetData(3, vRptSlNo, "3")
            .SetData(3, vRptName, "Purchase Report")

            .SetData(4, vRptSlNo, "4")
            .SetData(4, vRptName, "Consolidated Report")

            .Cols(vRptName).Width = 200
        End With
        SetColAlign(vsfRpt, svtAlign.Right, vRptSlNo)
        SetFlexColor(vsfRpt, "", vRptSlNo, vRptName)

    End Sub

    Private Sub vsfRpt_BeforeEdit(sender As Object, e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfRpt.BeforeEdit
        e.Cancel = True
    End Sub

    Private Sub vsfRpt_DoubleClick(sender As Object, e As System.EventArgs) Handles vsfRpt.DoubleClick
        Dim strFrmName As String
        strFrmName = ""

        With vsfRpt
            If .Row <= 0 Then Exit Sub
            Select Case .GetData(.Row, vRptName).ToString.ToUpper
                Case "SALES SUMMARY REPORT"
                    Dim argfrm As New frmRptDropsDSR
                    frmMain.OpenForms(argfrm, "Sales Summary Report")
                Case "PETTY CASH REPORT"
                    Dim argfrm As New frmRptDropsPettyCash
                    frmMain.OpenForms(argfrm, "Petty Cash Report")
                Case "PURCHASE REPORT"
                    Dim argfrm As New frmRptDropsPur
                    frmMain.OpenForms(argfrm, "Purchase Report")
                Case "CONSOLIDATED REPORT"
                    Dim argfrm As New frmRptDropsCons
                    frmMain.OpenForms(argfrm, "Consolidated Report")

            End Select
        End With
    End Sub


    Private Sub vsfRpt_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles vsfRpt.KeyDown
        If e.KeyCode = Keys.Enter Then
            Call vsfRpt_DoubleClick(sender, Nothing)
        End If
    End Sub

    Private Sub btnClose_ClickButtonArea(Sender As Object, e As MouseEventArgs) Handles btnClose.ClickButtonArea
        frmMain.CloseSelectedTabPage(Me)
    End Sub
End Class