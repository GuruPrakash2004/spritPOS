Public Class frmUtlTender

    Dim intPresent As Integer
    Dim arr(3) As Double
    Dim strDocHdrTblName As String
    Public intDocID As Int32
    Public intTrnID As Int32
    Public strBillNo As String
    Public dblBillAmt As Double


    Dim clsSVt As New svtLibrary

    Dim blnFromNT As Boolean = False
    Dim strDocno As String

    Public Sub New(Optional ByVal argBln As Boolean = False)
        ' This call is required by the designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
        blnFromNT = argBln
    End Sub

    Private Sub frmUtlTender_KeyUp(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyUp
        If e.KeyCode = Keys.F9 And e.Control Then
            Call initFormSize(Me, "TENDER", True, 958, 579, 0, 0)
            MsgBox("Restored Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
        If e.KeyCode = Keys.F8 And e.Control = True Then
            Call SaveActFormSettings(Me, "TENDER")
            MsgBox("Saved Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
        If e.KeyCode = Keys.Escape Then
            If fraRcpt.Visible Then
                fraRcpt.Visible = False
            Else
                Me.Close()
            End If
            Exit Sub
        End If
        If e.KeyCode = Keys.PageUp Then
            btnAutoRcv.PerformClick()
        End If
    End Sub

    Private Sub frmUtlTender_Load(sender As Object, e As System.EventArgs) Handles Me.Load

        Dim toolTip1 As New ToolTip()
        Dim strSql As String
        Dim rsDtl As OleDb.OleDbDataReader
        Me.KeyPreview = True
        Call initFormSize(Me, "TENDER", False, 504, 600, 0, 0)
        Me.KeyPreview = True
        Dim strSHForm As String = clsSVt.getDataStringValue("Select DocShForm from UtlTransactions where DocID=" & Val(intDocID), "", False)
        Dim strTblName As String = clsSVt.getDataStringValue("Select HdrTblName from UtlTransactions where DocID=" & Val(intDocID), "", False)
        Dim blnFound As Boolean
        clsSVt.getDataLongValue("Select TrnID from TrnTender where TrnID=" & Val(intTrnID) & " and DocID=" & Val(intDocID), 0, blnFound)
        If blnFound Then
            strSql = "Select * from TrnTender where TrnID=" & Val(intTrnID) & " and DocID=" & Val(intDocID)
            rsDtl = clsSVt.getDataReaderFromSQL(strSql)
            With rsDtl
                If .HasRows Then
                    .Read()
                    txtBillAmt.Text = GetVal(!BillAMt, 0)
                    txtCardAmt.Text = GetVal(!CardAmt, 0)
                    txtCash.Text = GetVal(!CashAmt, 0)
                    lblBillNo.Text = strSHForm & "No. # : " & clsSVt.getDataStringValue("Select " & strSHForm & "No from " & strTblName & " where " & strSHForm & "ID=" & Val(intTrnID), "", False)
                End If
                .Close()
            End With

        Else
            txtBillAmt.Text = dblBillAmt
            lblBillNo.Text = strSHForm & "No. # : " & strBillNo
        End If
        Call fillNearestAmt()
        btnOK.Enabled = False
        If intTrnID > 0 And blnFound = False Then
            btnOK.Enabled = True
        End If
    End Sub

    Private Sub btnClose_Click(sender As System.Object, e As System.EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub btnAutoRcv_Click(sender As Object, e As System.EventArgs) Handles btnAutoRcv.Click
        intPresent = intPresent + 1
        If intPresent > 2 Then intPresent = 0
        If txtRcvdAmt.Text = arr(intPresent) Then
            intPresent = intPresent + 1
            If intPresent > 2 Then intPresent = 0
        End If
        txtRcvdAmt.Text = arr(intPresent)
        txtRcvdAmt.SelectAll()
    End Sub

    Private Sub txtBillAmt_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles txtBillAmt.KeyPress
        e.Handled = True
    End Sub

    Private Sub txtRcvdAmt_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles txtRcvdAmt.KeyPress, txtCash.KeyPress, txtCardAmt.KeyPress, txtPayBack.KeyPress
        If e.KeyChar = "."c Then
            e.Handled = (CType(sender, TextBox).Text.IndexOf("."c) <> -1)
        ElseIf e.KeyChar <> ControlChars.Back Then
            e.Handled = ("0123456789".IndexOf(e.KeyChar) = -1)
        End If
    End Sub

    Private Sub txtRcvdAmt_TextChanged(sender As Object, e As System.EventArgs) Handles txtRcvdAmt.TextChanged
        Call calPayBack()
    End Sub

    Private Sub txtCardAmt_Click(sender As Object, e As System.EventArgs) Handles txtCardAmt.Click, txtCardAmt.Click, txtCash.Click, txtPayBack.Click
        Dim strCaption As String = ""
        If blnFromNT Then
            sender.SelectAll()
        Else
            Select Case sender.Name
                Case txtCash.Name, txtPayBack.Name
                    Exit Sub
                Case txtRcvdAmt.Name, txtCardAmt.Name
                    If sender.Name = "txtRcvdAmt" Then
                        strCaption = "Please enter the amount received..."
                    ElseIf sender.Name = "txtCardAmt" Then
                        strCaption = "Please enter card amount..."
                    End If
            End Select
        End If
        Call calPayBack()
    End Sub

    Private Sub txtRcvdAmt_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles txtRcvdAmt.Validating, txtCardAmt.Validating, txtCash.Validating
        Dim control As TextBox = CType(sender, TextBox)
        Dim result As Decimal = 0
        Decimal.TryParse(control.Text, result)
        control.Text = result.ToString()
    End Sub

    Private Sub calPayBack()
        txtCash.Text = Format(Val(txtBillAmt.Text) - Val(txtCardAmt.Text), "0.00")
        txtPayBack.Text = Format(Val(txtRcvdAmt.Text) + Val(txtCardAmt.Text) - Val(txtBillAmt.Text), "0.00")
    End Sub

    Private Sub txtCardAmt_TextChanged(sender As Object, e As System.EventArgs) Handles txtCardAmt.TextChanged
        calPayBack()
    End Sub

    Private Sub txtCash_TextChanged(sender As Object, e As System.EventArgs) Handles txtCash.TextChanged
        calPayBack()
    End Sub
    Public Sub fillNearestAmt()
        Dim sngAmt As Double, strSQL As String
        Dim rsDummy As OleDb.OleDbDataReader
        sngAmt = Val(txtBillAmt.Text)

        strSQL = "select mResult = CEILING(" & sngAmt & " * 1.0 / 100) * 100"
        rsDummy = clsSVt.getDataReaderFromSQL(strSQL)
        If rsDummy.HasRows Then
            If rsDummy.Read Then
                arr(0) = rsDummy!mResult
                txtRcvdAmt.Text = arr(0)
            End If
        End If

        strSQL = "select  mResult = CEILING(" & sngAmt & "  * 1.0 / 500) * 500"
        rsDummy = clsSVt.getDataReaderFromSQL(strSQL)
        If rsDummy.HasRows Then
            If rsDummy.Read Then
                arr(1) = rsDummy!mResult
            End If
        End If


        strSQL = "select  mResult = CEILING(" & sngAmt & "  * 1.0 / 1000) * 1000"
        rsDummy = clsSVt.getDataReaderFromSQL(strSQL)
        If rsDummy.HasRows Then
            If rsDummy.Read Then
                arr(2) = rsDummy!mResult
            End If
        End If
    End Sub

    Private Sub btnOK_Click(sender As Object, e As System.EventArgs) Handles btnOK.Click
        Dim strSQL As String
        Try
            strSQL = "Delete from TrnTender where TrnID=" & Val(intTrnID) & " and DocID=" & Val(intDocID)
            If clsSVt.execSQLStatement(strSQL) Then
                Try
                    strSQL = "Insert Into TrnTender"
                    strSQL = strSQL & "(DocID,TrnID,BillAmt,CashAmt,CardAmt)"
                    strSQL = strSQL & "Values("
                    strSQL = strSQL & Val(intDocID) & ", "
                    strSQL = strSQL & Val(intTrnID) & ", "
                    strSQL = strSQL & Val(txtBillAmt.Text) & ", "
                    strSQL = strSQL & Val(txtCash.Text) & ", "
                    strSQL = strSQL & Val(txtCardAmt.Text) & ")"
                    clsSVt.execSQLStatement(strSQL)
                Catch ex As Exception
                End Try
            End If
            MsgBox("Tender Change Updated SuccessFully!!", vbCritical, Me.Text)
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, Me.Text)
        End Try
    End Sub
End Class