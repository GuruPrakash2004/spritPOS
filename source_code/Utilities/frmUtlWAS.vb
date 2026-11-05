Public Class frmUtlWAS
    Dim Clssvt As New svtLibrary
    Private Sub frmUtlWAS_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.KeyPreview = True
        fillFormProperties(Me)
        FillAPI()
    End Sub
    Private Sub FillAPI()
        Dim rs As oledb.oledbDataReader
        Dim strSql As String

        strSql = " Select WAMSG,WAMSGFILE,WADEFRCVR ,WAEmoji from UtlMisc"

        rs = Clssvt.getDataReaderFromSQL(strSql)
        If rs.HasRows Then
            rs.Read()
            txtMSGAPI.Text = GetVal(rs("WAMSG"), "")
            txtMsgFileAPI.Text = GetVal(rs("WAMSGFILE"), "")
            txtDMOBNO.Text = GetVal(rs("WADEFRCVR"), "")
            txtEmoji.Text = GetVal(rs("WAEmoji"), "")
        End If
        rs.Close()

    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub frmUtlWAS_KeyUp(sender As Object, e As KeyEventArgs) Handles Me.KeyUp
        If e.KeyCode = Keys.F9 And e.Control = True Then
            Call initFormSize(Me, Me.Text, True, 652, 289, 0, 0)
            MsgBox("Restored Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If

        If e.KeyCode = Keys.F8 And e.Control = True Then
            Call SaveActFormSettings(Me, Me.Text)
            MsgBox("Saved Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Dim StrSql As String

        Try
            StrSql = "Update UtlMisc Set "
            StrSql &= " WAMSG='" & Trim(txtMSGAPI.Text) & "',"
            StrSql &= " WAMSGFILE='" & Trim(txtMsgFileAPI.Text) & "',"
            StrSql &= " WADEFRCVR='" & Trim(txtDMOBNO.Text) & "',"
            StrSql &= " WAEmoji=" & "N" & "'" & Trim(txtEmoji.Text) & "'"

            Clssvt.execSQLStatement(StrSql)

            MsgBox("Data Saved Successfully!!", vbInformation, Me.Text)

        Catch ex As Exception
            MsgBox("Error While Saving Settings", vbInformation, Me.Text)
        End Try



    End Sub

    Private Sub btnGetAPI_Click(sender As Object, e As EventArgs) Handles btnGetAPI.Click
        Dim StrMsgAPI As String
        Dim StrMsgFileAPI As String

        StrMsgAPI = "https://messageapi.in/MessagingAPI/sendMessage.php?LoginId=9205389999&password=123456&mobile_number=<<MOB>>&message=<<MSG>>"

        StrMsgFileAPI = "https://messageapi.in/MessagingAPI/sendMessageDouble.php?LoginId=9205389999&password=123456&mobile_number=<<MOB>>&message=<<MSG>>&fileUrl=<<URL>>"

        txtMSGAPI.Text = StrMsgAPI
        txtMsgFileAPI.Text = StrMsgFileAPI
    End Sub
End Class