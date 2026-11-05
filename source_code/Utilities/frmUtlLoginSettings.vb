Imports System.IO
Imports System.Data.oledb

Public Class frmUtlLoginSettings
    Dim clsSVT As New svtLibrary

    Private Sub txtDomain_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtADDomain.KeyPress
        If e.KeyChar = "'" Then
            e.Handled = True
        End If
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Call stringReplace(Me)
        If ValidFields() Then
            Call saveDetails()
            Call GetMiscFlds()
            Me.Close()
        End If
    End Sub

    Private Function ValidFields() As Boolean
        ValidFields = False

        If opAD.Checked Then
            If opADReg.Checked Then
                If txtADDomain.Text = "" Then
                    MsgBox("Please enter Domain Name!!!", MsgBoxStyle.Information, Me.Text)
                    txtADDomain.Focus()
                    Exit Function
                End If
            End If
        End If

        If chkPwd.Checked Then
            If opPwdRules.Checked = True Then
                If opExpiryDays.Checked Then
                    If Val(txtExpDays.Text) <= 0 Then
                        MsgBox("Please enter the Password Expiry Days!!", MsgBoxStyle.Information, Me.Text)
                        txtExpDays.Focus()
                        Exit Function
                    End If
                End If
                If Val(txtSplChrs.Text) + Val(txtNos.Text) + Val(txtAlpha.Text) > Val(txtPwdLen.Text) Then
                    MsgBox("Min. no of " & vbCrLf & "Special Character + Alphabets + Numeric" & vbCrLf & "should not be less than the Password length!!", MsgBoxStyle.Information, Me.Text)
                    Exit Function
                End If

                'If Val(txtAlpha.Text) <> 0 Then
                If Val(txtAlpha.Text) <> Val(txtLCase.Text) + Val(txtUCase.Text) Then
                    MsgBox("Mni. No of " & vbCrLf & "LowerCase + UpperCase" & vbCrLf & "should be equal to Alphabets Length!!", MsgBoxStyle.Information, Me.Text)
                    Exit Function
                End If
                'End If
                'If Val(txtPwdLen.Text) = 0 Then
                '    MsgBox("Please Enter the Min.Length of Password!", MsgBoxStyle.Information, Me.Text)
                '    Exit Function
                'End If
            End If
        End If
        ValidFields = True

    End Function

    Private Function saveDetails() As Boolean
        Dim strSQL As String = ""
        Dim svtTrn As oledb.oledbTransaction
        saveDetails = False

        strSQL = " Update UtlMisc set "

        If chkLoginReq.Checked Then
            strSQL &= " LoginReq='Y',"
        Else
            strSQL &= " LoginReq='N',"
        End If

        If opUserBased.Checked Then
            strSQL &= " UserBased='Y',"
        Else
            strSQL &= " UserBased='N',"
        End If

        If opAD.Checked Then
            strSQL &= " ADAuth = 'Y',"
            If opADAuto.Checked Then
                strSQL &= " ADAutoLogin = 'Y',"
                strSQL &= " ADDomain = '',"
                strSQL = strSQL & " PwdRules = 'N',NeverExpires = 'N',PwdExpiryDays = 0,PwdMinSplChrs = 0,PwdMinNumeric = 0,PwdMinAlpha = 0,PwdMinLen = 0"
            Else
                strSQL &= " ADAutoLogin = 'N',"
                strSQL &= " ADDomain = '" & Trim(txtADDomain.Text) & "',"
                strSQL = strSQL & " PwdRules = 'N',NeverExpires = 'N',PwdExpiryDays = 0,PwdMinSplChrs = 0,PwdMinNumeric = 0,PwdMinAlpha = 0,PwdMinLen = 0"
            End If
        Else
            strSQL &= " ADAuth = 'N',ADAutoLogin = 'N',ADDomain = '',"
            'strSQL = strSQL & " ShowRememberMe = '" & Trim(GetVal(getCheckString(chkRemember), "N")) & "',"
            If chkPwd.Checked Then
                strSQL = strSQL & " PwdPolicy = 'Y',"
                If opNeverExpires.Checked Then
                    strSQL = strSQL & " NeverExpires = 'Y',"
                    strSQL = strSQL & " PwdExpiryDays = 0,"
                Else
                    strSQL = strSQL & " NeverExpires = 'N',"
                    strSQL = strSQL & " PwdExpiryDays = " & Val(txtExpDays.Text) & ","
                End If
                If opPwdRules.Checked Then
                    strSQL = strSQL & " PwdRules = 'Y',"

                    strSQL = strSQL & " PwdMinSplChrs = " & Val(txtSplChrs.Text) & ","
                    strSQL = strSQL & " PwdMinNumeric = " & Val(txtNos.Text) & ","
                    strSQL = strSQL & " PwdMinAlpha = " & Val(txtAlpha.Text) & ","
                    strSQL = strSQL & " PwdMinLen = " & Val(txtPwdLen.Text) & ","
                    strSQL = strSQL & " PwdMinLCase = " & Val(txtLCase.Text) & ","
                    strSQL = strSQL & " PwdMinUCase = " & Val(txtUCase.Text)
                Else
                    strSQL = strSQL & " PwdRules = 'N',PwdMinSplChrs = 0,PwdMinNumeric = 0,PwdMinAlpha = 0,PwdMinLen = 0"
                End If
            Else
                strSQL = strSQL & " PwdPolicy = 'N',PwdRules = 'N',NeverExpires = 'N',PwdExpiryDays = 0,PwdMinSplChrs = 0,PwdMinNumeric = 0,PwdMinAlpha = 0,PwdMinLen = 0"
            End If
        End If

        Try
            svtTrn = svtConn.BeginTransaction(IsolationLevel.ReadCommitted)
            If clsSVT.execSQLStatement(strSQL, svtTrn) Then
                MsgBox("Updated  successfully.", vbInformation, Me.Text)
                saveDetails = True
                svtTrn.Commit()
            End If
        Catch ex As Exception
            MsgBox("Error during save" & vbCrLf & ex.Message, MsgBoxStyle.Critical, Me.Text)
            svtTrn.Rollback()
            Exit Function
        End Try
    End Function

    Private Sub frmUtlLoginSettings_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Call fillFormProperties(Me)
        Call fillLoginSettings()
        Call initFormSize(Me, Me.Text, False, 650, 335, 342, 263)
    End Sub

    Private Sub opbtnAD_CheckedChanged(sender As Object, e As EventArgs) Handles opAD.CheckedChanged
        If opAD.Checked Then
            gbADType.Visible = True
            gbPwd.Visible = False
        Else
            gbADType.Visible = False
            gbPwd.Visible = False
        End If
    End Sub

    Private Sub chkPwd_CheckedChanged(sender As Object, e As EventArgs) Handles chkPwd.CheckedChanged
        If chkPwd.Checked Then
            gbPwd.Visible = True
            gbADType.Visible = False
        Else
            gbPwd.Visible = False
            gbADType.Visible = False
        End If
    End Sub

    Private Sub opbtnReg_CheckedChanged(sender As Object, e As EventArgs) Handles opReg.CheckedChanged
        If opReg.Checked Then
            '    chkRemember.Visible = True
            '    chkPwd.Visible = True
        Else
            '    chkRemember.Checked = False
            '    chkRemember.Visible = False
            chkPwd.Visible = False
            chkPwd.Checked = False
            gbPwd.Visible = False
        End If
    End Sub

    Private Sub fillLoginSettings()
        Dim rs As oledb.oledbDataReader
        Dim strSQL As String = ""
        strSQL = "Select * from UtlMisc"
        rs = clsSVT.getDataReaderFromSQL(strSQL)
        opUserBased.Checked = True
        opNoRules.Checked = True
        With rs
            .Read()
            If !ADAuth = "Y" Then
                opAD.Checked = True
                If !ADAutoLogin = "Y" Then
                    opADAuto.Checked = True
                    pnlDomain.Visible = False
                Else
                    opADReg.Checked = True
                    pnlDomain.Visible = True
                    txtADDomain.Text = GetVal(!ADDomain, "")
                End If
                gbPwd.Visible = False
            Else
                opReg.Checked = True
                'If !ShowRememberMe = "Y" Then
                '    chkRemember.Checked = True
                'Else
                '    chkRemember.Checked = False
                'End If
                If !PwdPolicy = "Y" Then
                    chkPwd.Checked = True
                    If !NeverExpires = "Y" Then
                        opNeverExpires.Checked = True
                    Else
                        opExpiryDays.Checked = True
                        txtExpDays.Text = GetVal(!PwdExpiryDays, "")
                    End If

                    If !PwdRules = "Y" Then
                        opPwdRules.Checked = True
                        opNoRules.Checked = False
                        txtSplChrs.Text = GetVal(!PwdMinSplChrs, "")
                        txtNos.Text = GetVal(!PwdMinNumeric, "")
                        txtAlpha.Text = GetVal(!PwdMinAlpha, "")
                        txtPwdLen.Text = GetVal(!PwdMinLen, "")
                        txtLCase.Text = GetVal(!PwdMinLCase, "")
                        txtUCase.Text = GetVal(!PwdMinUcase, "")
                    Else
                        opNoRules.Checked = True
                    End If
                Else
                    chkPwd.Checked = False
                End If
            End If
            If !UserBased = "Y" Then
                opRoleBased.Checked = False
                opUserBased.Checked = True
            Else
                opUserBased.Checked = False
                opRoleBased.Checked = True
            End If
            If !LoginReq = "Y" Then
                chkLoginReq.Checked = True
            Else
                chkLoginReq.Checked = False
            End If
            .Close()
        End With
    End Sub

    Private Sub opPwdRules_CheckedChanged(sender As Object, e As EventArgs) Handles opPwdRules.CheckedChanged
        If opPwdRules.Checked Then
            pnlPwdRules.Visible = True
        Else
            pnlPwdRules.Visible = False
        End If
    End Sub

    Private Sub txtExpDays_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtExpDays.KeyPress, txtSplChrs.KeyPress, txtNos.KeyPress, txtAlpha.KeyPress, txtPwdLen.KeyPress
        If Not IsNumeric(e.KeyChar) And e.KeyChar <> vbBack Then
            e.Handled = True
        End If
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub opADAuto_CheckedChanged(sender As Object, e As EventArgs) Handles opADAuto.CheckedChanged
        If opADAuto.Checked = True Then
            pnlDomain.Visible = False
        Else
            pnlDomain.Visible = True
        End If
    End Sub

    Private Sub frmUtlLoginSettings_KeyUp(sender As Object, e As KeyEventArgs) Handles Me.KeyUp
        If e.KeyCode = Keys.F9 And e.Control = True Then
            Call initFormSize(Me, Me.Text, False, 650, 335, 342, 263)
            MsgBox("Restored Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
        If e.KeyCode = Keys.F8 And e.Control = True Then
            Call SaveActFormSettings(Me, Me.Text)
            MsgBox("Saved Form Layout Successfully!!", vbInformation, Me.Text)
        End If
    End Sub

    Private Sub txtLCase_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtLCase.KeyPress
        If Not IsNumeric(e.KeyChar) And e.KeyChar <> vbBack Then
            e.Handled = True
        End If
    End Sub

    Private Sub txtUCase_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtUCase.KeyPress
        If Not IsNumeric(e.KeyChar) And e.KeyChar <> vbBack Then
            e.Handled = True
        End If
    End Sub

    Private Sub opADReg_CheckedChanged(sender As Object, e As EventArgs) Handles opADReg.CheckedChanged
        If opADReg.Checked = True Then
            pnlDomain.Visible = True
        Else
            pnlDomain.Visible = False
        End If
    End Sub

    Private Sub chkLoginReq_CheckedChanged(sender As Object, e As EventArgs) Handles chkLoginReq.CheckedChanged
        If chkLoginReq.Checked = True Then
            PnlAuth.Visible = True
            If opAD.Checked Then
                gbADType.Visible = True
                gbPwd.Visible = False
            ElseIf chkPwd.Checked Then
                gbADType.Visible = False
                gbPwd.Visible = True
            Else
                gbADType.Visible = False
                gbPwd.Visible = False
            End If
        Else
                PnlAuth.Visible = False
            gbADType.Visible = False
            gbPwd.Visible = False
        End If
    End Sub

    Private Sub frmUtlLoginSettings_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        If opAD.Checked Then
            gbPwd.Visible = False
        End If
    End Sub

    Private Sub opNeverExpires_CheckedChanged(sender As Object, e As EventArgs) Handles opNeverExpires.CheckedChanged
        If opNeverExpires.Checked Then
            txtExpDays.Text = ""
            txtExpDays.Enabled = False
        Else
            txtExpDays.Enabled = True
        End If
    End Sub

    Private Sub opExpiryDays_CheckedChanged(sender As Object, e As EventArgs) Handles opExpiryDays.CheckedChanged
        If opExpiryDays.Checked Then
            txtExpDays.Enabled = True
        End If
    End Sub

End Class