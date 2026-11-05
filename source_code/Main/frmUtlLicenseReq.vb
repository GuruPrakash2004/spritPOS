Imports System.IO
Public Class frmUtlLicenseReq
    Dim clsSvt As New svtLibrary
    Private Sub frmUtlLicenseReq_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Call fillFormProperties(Me)
        Call fillDtls()
    End Sub

    Private Sub fillDtls()
        Dim strSQL As String
        Dim cmdDummy As New OleDb.OleDbCommand
        Dim rs As OleDb.OleDbDataReader
        Dim strMacID As String = UCase(Trim(getUUID()))
        Dim blnRecPresent As Boolean = False

        strSQL = "Select RecID, MacID, CompName, CompAdd1, CompAdd2, CompAdd3, CompAdd4, ClientSystemName,Remarks"
        strSQL &= " From MstLIC"
        strSQL &= " Where MacID = '" & strMacID & "'"

        txtMacID.Text = strMacID

        txtCompName.Text = gSettings.gCompName
        txtAdd1.Text = gSettings.gCompAdd1
        txtAdd2.Text = gSettings.gCompAdd2
        txtAdd3.Text = gSettings.gCompAdd3
        txtAdd4.Text = gSettings.gCompAdd4

        webConn.ConnectionString = webConnString
        Try

            webConn.Open()
        Catch ex As Exception
            txtMacID.PasswordChar = ""
            lblInfo.Text = "Cannot connect. Please check internet connection and try again!"
            btnSend.Visible = False
            webConn.Close()
            Exit Sub
        End Try

        Try
            With cmdDummy
                .Connection = webConn
                .CommandText = strSQL
                rs = .ExecuteReader()
                If rs.HasRows Then
                    rs.Read()
                    blnRecPresent = True
                    txtMacID.Text = GetVal(rs!MacID, "")
                    txtCompName.Text = GetVal(rs!CompName, "")
                    txtAdd1.Text = GetVal(rs!CompAdd1, "")
                    txtAdd2.Text = GetVal(rs!CompAdd2, "")
                    txtAdd3.Text = GetVal(rs!CompAdd3, "")
                    txtAdd4.Text = GetVal(rs!CompAdd4, "")
                    txtRmks.Text = GetVal(rs!Remarks, "")
                    txtServerName.Text = GetVal(rs!ClientSystemname, "")
                    lblInfo.Text = "Request already present. Call up Softvent for further support! (RNumber: " & rs!RecID & ")"
                    btnSend.Visible = False
                    rs.Close()
                Else
                    txtMacID.Text = getUUID()
                    txtCompName.Text = gSettings.gCompName
                    txtAdd1.Text = gSettings.gCompAdd1
                    txtAdd2.Text = gSettings.gCompAdd2
                    txtAdd3.Text = gSettings.gCompAdd3
                    txtAdd4.Text = gSettings.gCompAdd4
                    txtServerName.Text = gStrSystemName
                    btnSend.Visible = True
                End If
            End With
            webConn.Close()
        Catch ex As Exception
            MsgBox("Error" & vbCrLf & ex.Message, MsgBoxStyle.Critical, "Error")
            btnSend.Visible = False
            webConn.Close()
        End Try
    End Sub

    Private Sub btnSend_Click(sender As Object, e As EventArgs)
        Dim strSQL As String
        Dim cmdDummy As New OleDb.OleDbCommand

        strSQL = "Insert Into MstLIC(MacID, CompName, CompAdd1, CompAdd2, CompAdd3, CompAdd4,ClientSysDateTime, ClientSystemName, ClientSystemUserName,Remarks)"
        strSQL &= " Values ("
        strSQL &= " '" & GetVal(txtMacID.Text, "") & "',"
        strSQL &= " '" & GetVal(txtCompName.Text, "") & "',"
        strSQL &= " '" & GetVal(txtAdd1.Text, "") & "',"
        strSQL &= " '" & GetVal(txtAdd2.Text, "") & "',"
        strSQL &= " '" & GetVal(txtAdd3.Text, "") & "',"
        strSQL &= " '" & GetVal(txtAdd4.Text, "") & "',"
        strSQL &= " '" & getDateFormatInInsert(Date.Now, True) & "',"
        strSQL &= " '" & txtServerName.Text & "',"
        strSQL &= " '" & gStrSystemUserName.Replace("'", "") & "',"
        strSQL &= " '" & GetVal(txtRmks.Text, "") & "')"

        webConn.ConnectionString = webConnString
        Try
            webConn.Open()
            With cmdDummy
                .Connection = webConn
                .CommandText = strSQL
                If .ExecuteNonQuery > 0 Then
                    MsgBox("Request Sent Sucessfully! " & vbCrLf & "Please call Softvent for granting Licence.", vbInformation, Me.Text)
                    frmMain.CloseSelectedTabPage(Me)
                End If
                webConn.Close()
            End With

        Catch ex As Exception
            MsgBox("Cannot connect. Please check internet connection and try again!" & vbCrLf & ex.Message, vbInformation, "Connectivity Error")
            btnSend.Visible = False
            Exit Sub
        End Try
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Call frmMain.CloseSelectedTabPage(Me)
    End Sub

    Private Sub txtMacID_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtMacID.KeyPress, txtCompName.KeyPress, txtAdd1.KeyPress, txtAdd2.KeyPress, txtAdd3.KeyPress, txtAdd4.KeyPress
        e.Handled = True
    End Sub

    Private Sub txtRmks_KeyPress(sender As Object, e As KeyPressEventArgs)
        If e.KeyChar = "'" Then
            e.Handled = True
        End If
    End Sub

    Private Sub btnRecUpd_Click(sender As Object, e As EventArgs)
        'Dim strLicFile As String, strStatus As String
        'strLicFile = Application.StartupPath & "\svt.Lic"

        'webConn.ConnectionString = webConnString
        'Try

        '    webConn.Open()
        'Catch ex As Exception
        '    lblInfo.Text = "Cannot connect. Please check internet connection and try again!"
        '    btnSend.Visible = False
        '    btnRecUpd.Visible = False
        '    webConn.Close()
        '    Exit Sub
        'End Try

        'strStatus = clsSvt.getDataStringValue("Select CStatus from MstLIC where MacID = '" & UCase(Trim(getMacAddress())) & "'", "", False, Nothing, webConn)
        'webConn.Close()

        'If strStatus = "" Then strStatus = "N"

        'Dim strKey As String
        'strKey = DateAdd("d", 90, getSystemDate()) & "|"
        'strKey = strKey & UCase(Trim(getMacAddress())) & "|"
        'strKey = strKey & strstatus
        'strKey = svtCrypto.Encrypt(strKey, "L@#$_)ic(2019%^")

        'Using outfile As New StreamWriter(strLicFile)
        '    outfile.Write(strKey)
        'End Using
        'If strStatus = "Y" Then
        '    MsgBox("Successfully Updated Licence. Application needs to be Restarted.", MsgBoxStyle.Information, "Licence Updated")
        '    End
        'Else
        '    MsgBox("Please call Softvent for granting Licence.", MsgBoxStyle.Critical, "Licence to be granted")
        '    End
        'End If
    End Sub
End Class