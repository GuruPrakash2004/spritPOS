Public Class frmUtlChange
    Dim intPresent As Integer
    Dim arr(3) As Double
    Dim clsSVT As New svtLibrary
    Dim gComputerName As String

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        Me.Close()
    End Sub

    Private Sub frmUtlChange_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyUp
        If e.KeyCode = Keys.Escape Then
            Me.Close()
        End If
        If e.KeyCode = Keys.PageUp Then
            btnAutoRcv.PerformClick()
        End If
    End Sub

    Private Sub frmUtlChange_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.KeyPreview = True
        txtRcvdAmt.MaxLength = 10
        gComputerName = System.Net.Dns.GetHostName
    End Sub

    Private Sub btnAutoRcv_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAutoRcv.Click
        intPresent = intPresent + 1
        If intPresent > 2 Then intPresent = 0
        If txtRcvdAmt.Text = arr(intPresent) Then
            intPresent = intPresent + 1
            If intPresent > 2 Then intPresent = 0
        End If
        txtRcvdAmt.Text = arr(intPresent)
        txtRcvdAmt.SelectAll()
    End Sub

    Private Sub btnNextBill_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNextBill.Click
        Dim strinvID As Long
        strinvID = clsSVT.getDataLongValue("Select isnull(Min(Invid),0) from TrnHdrInv Where pSysName = '" & gComputerName & "' and InvID > " & Val(txtBillID.Text), 0, False)
        If strinvID <= 0 Then Exit Sub
        txtBillID.Text = strinvID
        txtBillNo.Text = clsSVT.getDataStringValue("Select InvNo from TrnHdrInv Where InvID = " & strinvID, "", False)
        txtBillAmt.Text = Format(clsSVT.getDataDoubleValue("Select InvValue from TrnHdrInv Where InvID = " & strinvID, 0, False), "0.00")
        Call fillNearestAmt()
        txtRcvdAmt.Focus()

    End Sub


    Private Sub btnPrevBill_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrevBill.Click
        Dim strinvID As Long
        strinvID = clsSVT.getDataLongValue("Select isnull(Max(Invid),0) from TrnHdrInv Where pSysName = '" & gComputerName & "' and InvID < " & Val(txtBillID.Text), 0, False)
        If strinvID <= 0 Then Exit Sub
        txtBillID.Text = strinvID
        txtBillNo.Text = clsSVT.getDataStringValue("Select InvNo from TrnHdrInv Where InvID = " & strinvID, "", False)
        txtBillAmt.Text = Format(clsSVT.getDataDoubleValue("Select invValue from TrnHdrInv Where InvID = " & strinvID, 0, False), "0.00")
        Call fillNearestAmt()
        txtRcvdAmt.Focus()
    End Sub

    Public Sub fillNearestAmt()
        Dim sngAmt As Double, strSQL As String
        Dim rsDummy As oledb.oledbDataReader
        sngAmt = Val(txtBillAmt.Text)

        strsql = "select mResult = CEILING(" & sngAmt & " * 1.0 / 100) * 100"
        rsDummy = clsSVT.getDataReaderFromSQL(strSQL)
        If rsDummy.HasRows Then
            If rsDummy.Read Then
                arr(0) = rsDummy!mResult
                txtRcvdAmt.Text = arr(0)
            End If
        End If

        strSQL = "select  mResult = CEILING(" & sngAmt & "  * 1.0 / 500) * 500"
        rsDummy = clsSVT.getDataReaderFromSQL(strSQL)
        If rsDummy.HasRows Then
            If rsDummy.Read Then
                arr(1) = rsDummy!mResult
            End If
        End If


        strSQL = "select  mResult = CEILING(" & sngAmt & "  * 1.0 / 1000) * 1000"
        rsDummy = clsSVT.getDataReaderFromSQL(strSQL)
        If rsDummy.HasRows Then
            If rsDummy.Read Then
                arr(2) = rsDummy!mResult
            End If
        End If
    End Sub

    Private Sub txtRcvdAmt_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles txtRcvdAmt.KeyPress
        If KeyValidate("Decimal", e.KeyChar) Then
            e.Handled = False
        Else
            e.Handled = True
        End If
    End Sub

    Private Sub txtRcvdAmt_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtRcvdAmt.TextChanged
        txtPayBack.Text = Format(Val(txtRcvdAmt.Text) - Val(txtBillAmt.Text), "0.00")
    End Sub
End Class