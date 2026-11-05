Imports SSPLLic.clsSSPLLic
Public Class frmUtlReg
    Dim intRowNo As Integer
    Dim clsR As New SSPLLic.clsSSPLLic

    Private Sub btnClose_Click(sender As System.Object, e As System.EventArgs) Handles btnClose.Click
        End
    End Sub

    Private Sub frmUtlReg_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        Do While True
            lblRegNo.Text = getForNoRandom()
            If lblRegNo.Text <> "" Then Exit Do
        Loop
    End Sub

    Public Function getForNoRandom() As String
        Dim intNo As Long
        getForNoRandom = ""
        intRowNo = 0
        Do While True
            intNo = Int(10 * Now().Ticks / 3000 Mod 2500)
            If intNo > 0 And intNo <= 2500 Then
                getForNoRandom = clsR.getRegNo(CInt(intNo))
                intRowNo = intNo
                Exit Do
            End If
        Loop
    End Function

    Private Sub btnRegister_Click(sender As System.Object, e As System.EventArgs) Handles btnRegister.Click
        If lblRegNo.Text = "" Then Exit Sub
        If txtKey.Text = "" Then Exit Sub
        If intRowNo = 0 Then Exit Sub

        If Trim(txtKey.Text) = "" Then Exit Sub
        If Not clsR.validRegKey(lblRegNo.Text, txtKey.Text) Then
            MsgBox("Invalid Key. Contact SoftVent Services Pvt Ltd!!", vbCritical, "REGISTRATION...")
            Exit Sub
        End If
        Call SaveSetting("SVTREG", "VIC", "VICkey", "YES")
        MsgBox("Successfully Registered!!", MsgBoxStyle.Exclamation, "REGISTRATION")
        Me.Close()
    End Sub

    Private Sub frmUtlReg_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        If chkSetupVersion() <> "OK" Then
            End
        End If
    End Sub
End Class