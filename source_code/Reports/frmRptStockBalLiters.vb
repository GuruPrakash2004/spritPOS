Public Class frmRptStockBalLiters
    Private Sub frmRptStockBalLiters_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Call fillFormProperties(Me)
        Call ChangeFontSize(Me)
        Call initFormVsfDet(Me, Me.Text, vsfView, "VIEW")
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        frmMain.CloseSelectedTabPage(Me)
    End Sub
End Class