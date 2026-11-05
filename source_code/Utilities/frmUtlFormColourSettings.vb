Imports System.IO
Imports C1.Win.C1FlexGrid
Public Class frmUtlFormColourSettings
    Dim converter As System.ComponentModel.TypeConverter =
        System.ComponentModel.TypeDescriptor.GetConverter(GetType(Font))
    Dim clsSVT As New svtLibrary
    Dim strSQL As String

    Const sampItemCode = 0
    Const sampItemName = 1
    Const sampBrand = 2
    Const sampSubGroup = 3

    Private Sub btnDefault_Click_1(sender As System.Object, e As System.EventArgs) Handles btnDefault.Click
        If MsgBox("Are you sure you want to reset the form colours to default? ", vbQuestion + MsgBoxStyle.YesNo, "Confirmation") = MsgBoxResult.Yes Then
            defaultFormClrs()
        End If
    End Sub
    Private Sub fillGridColours()
        Dim str As String = getGsettings("COMPANY", "MainFormClrs", "")
        If str = "" Then Exit Sub
        Dim strArr() As String = str.Split(New Char() {","c})
        Dim strChild() As String
        Dim strfontName As String

        'Normal
        lblMainFore.BackColor = Color.FromArgb(Convert.ToInt32(strArr(0)))
        lblMainBack.BackColor = Color.FromArgb(Convert.ToInt32(strArr(1)))
        strfontName = Trim(strArr(2))
        lblMainFont.Font = CType(converter.ConvertFromString(strfontName & "," & strArr(3) & "pt"), Font)
        tvMain.Font = lblMainFont.Font
        lblUsername.Font = lblMainFont.Font
        lblUsername.ForeColor = lblMainFore.BackColor
        pnlLogin.BackColor = lblMainBack.BackColor
        tvMain.BackColor = lblMainBack.BackColor
        tvMain.ForeColor = lblMainFore.BackColor


        str = getGsettings("COMPANY", "FormClrs", "")

        strChild = str.Split(New Char() {","c})
        'Alternate
        lblChildMandatoryFore.BackColor = Color.FromArgb(Convert.ToInt32(strChild(0)))
        lblChildReadFore.BackColor = Color.FromArgb(Convert.ToInt32(strChild(1)))
        lblChildOptionalFore.BackColor = Color.FromArgb(Convert.ToInt32(strChild(2)))
        lblChildBack.BackColor = Color.FromArgb(Convert.ToInt32(strChild(3)))

        lblMandtory.ForeColor = lblChildMandatoryFore.BackColor
        lblReadonly.ForeColor = lblChildReadFore.BackColor
        lblOptional.ForeColor = lblChildOptionalFore.BackColor
        pnlChild.BackColor = lblChildBack.BackColor


    End Sub

    Private Sub defaultFormClrs()
        'Main

        lblMainBack.BackColor = Color.FromArgb(43, 43, 43)
        lblMainFore.BackColor = Color.White
        lblMainFont.Font = New Font("Segoe UI", 10, FontStyle.Regular)
        lblUsername.ForeColor = Color.White
        tvMain.Font = New Font("Segoe UI", 10, FontStyle.Regular)
        tvMain.ForeColor = Color.White
        tvMain.BackColor = Color.FromArgb(43, 43, 43)

        'Child
        lblChildMandatoryFore.BackColor = Color.Red
        lblChildReadFore.BackColor = Color.Blue
        lblChildOptionalFore.BackColor = Color.Black
        lblChildBack.BackColor = Color.FromArgb(188, 205, 227)

        lblMandtory.ForeColor = Color.Red
        lblReadonly.ForeColor = Color.Blue
        lblOptional.ForeColor = Color.Black

        pnlChild.BackColor = Color.FromArgb(188, 205, 227)

    End Sub

    Private Sub btnFormClrSave_Click(sender As System.Object, e As System.EventArgs) Handles btnFormClrSave.Click
        'Dim str As String, strSQL As String
        'Try
        '    'Normal
        '    str = lblMainFore.BackColor.ToArgb & ", " & lblMainBack.BackColor.ToArgb & ", " & lblMainFont.Font.Name & "," & Math.Round(Val(lblMainFont.Font.Size)) & ", "
        '    str = str & lblMainFont.Font.Style
        '    strSQL = "Update UtlGSettings set SetValue = '" & str & "' where SetName= 'MainFormClrs' and SetGroup = 'COMPANY'"
        '    clsSVT.execSQLStatement(strSQL)

        '    str = lblChildMandatoryFore.BackColor.ToArgb & ", " & lblChildReadFore.BackColor.ToArgb & ", " & lblChildOptionalFore.BackColor.ToArgb & ", "
        '    str = str & lblChildBack.BackColor.ToArgb
        '    strSQL = "Update UtlGSettings set SetValue = '" & str & "' where SetName= 'FormClrs' and SetGroup = 'COMPANY'"
        '    clsSVT.execSQLStatement(strSQL)

        '    'str = lblMandtory.Font.Size & ", " & lblReadonly.Font.Size & ", " & lblOptional.Font.Size & ", " & TextBox1.Font.Size & ", " & TextBox2.Font.Size & ", " & TextBox3.Font.Size
        '    str = tbFont.Value
        '    strSQL = "Update UtlGSettings set SetValue = '" & str & "' where SetName= 'FontSize' and SetGroup = 'COMPANY'"
        '    clsSVT.execSQLStatement(strSQL)

        '    MsgBox("Updated Successfully!", vbInformation, Me.Text)
        '    Call loadFormColors()
        '    Call frmMain.SetMainFormColor()
        '    gintFontSize = CInt(str)
        'Catch ex As Exception
        '    MsgBox("Update failed! Please check for valid data and retry.", vbCritical, Me.Text)
        '    Exit Sub
        'End Try
    End Sub

    Private Sub btnClose_Click(sender As System.Object, e As System.EventArgs) Handles btnCloseq.Click
        Me.Close()
    End Sub
    Private Sub Initilization()
        lblUsername.Text = gUserCode
        lblComp.Text = gSettings.gCompName
        lblFin.Text = "(" & "FinYear : " & gSettings.gCompStDt & " - " & gSettings.gCompEndDt & ")"
        GetUserImage()
        Call CreateRoundObj(picLogin)
    End Sub
    Private Sub frmUtlGCSettings_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        Initilization()

        Call fillGridColours()
        'tbFont.Value.ToString = "Select SetValue from UtlGSettings where SetGroup='Company' and SetName='FontSize'"
        strSQL = "Select SetValue from UtlGSettings where SetGroup='Company' and SetName='FontSize'"
        tbFont.Value = Val(clsSVT.getDataStringValue(strSQL, "", False))
        lblMandtory.Font = New Font("Verdana", tbFont.Value)
        lblReadonly.Font = New Font("Verdana", tbFont.Value)
        lblOptional.Font = New Font("Verdana", tbFont.Value)
        TextBox1.Font = New Font("Verdana", tbFont.Value)
        TextBox2.Font = New Font("Verdana", tbFont.Value)
        TextBox3.Font = New Font("Verdana", tbFont.Value)
        frmMain.tvMain.Font = New Font("Verdana", tbFont.Value)
        frmMain.txtSearch.Font = New Font("Verdana", tbFont.Value)
    End Sub

    Private Sub showClrDlg(ByVal argLbl As Label)
        Dim cDialog As New ColorDialog()
        cDialog.Color = argLbl.BackColor ' initial selection is current color.

        If (cDialog.ShowDialog() = DialogResult.OK) Then
            argLbl.BackColor = cDialog.Color ' update with user selected color.
        End If
    End Sub
    Private Sub showClrDlgTltip(ByVal argLbl As Label, ByVal argtltip As Label, ByVal argPnltltip As Panel, ByVal argClrType As String)
        Dim cDialog As New ColorDialog()
        cDialog.Color = argLbl.BackColor ' initial selection is current color.

        If (cDialog.ShowDialog() = DialogResult.OK) Then
            argLbl.BackColor = cDialog.Color ' update with user selected color.
            Select Case argClrType

                Case "B"
                    argtltip.BackColor = argLbl.BackColor
                    argPnltltip.BackColor = argLbl.BackColor

                Case "F"
                    argtltip.ForeColor = argLbl.BackColor
            End Select
        End If
    End Sub

    Private Sub GetUserImage()
        Dim strSql As String = ""
        Dim cmd As oledb.oledbCommand = Nothing
        picLogin.Image = Nothing
        strSql = "select " & getImgSyntax("ISNULL", "UserImg") & " from MstUser where UserID = " & Val(gUserID)
        Try
            cmd = New oledb.oledbCommand (strSql, svtConn)
            Dim imageData As Byte() = DirectCast(cmd.ExecuteScalar(), Byte())
            If imageData.Length = 0 Then
                ' btnRmvImg.Visible = False
                Exit Sub
            End If
            If Not imageData Is Nothing Then
                If imageData.Length > 0 Then
                    Dim ImageMemoryStream As MemoryStream = New IO.MemoryStream(GetUserImageBytesFromOLEField(imageData))
                    picLogin.Image = Image.FromStream(ImageMemoryStream, True)
                    picLogin.SizeMode = PictureBoxSizeMode.StretchImage
                    ' btnRmvImg.Visible = True
                Else
                    picLogin.Image = Nothing
                    '  btnRmvImg.Visible = False
                End If
            End If
        Catch
        End Try
    End Sub

    Friend Shared Function GetUserImageBytesFromOLEField(ByVal oleFieldBytes() As Byte) As Byte()

        Dim BITMAP_ID_BLOCK As String = "BM"
        Dim JPG_ID_BLOCK As String = ChrW(&HFF).ToString() & ChrW(&HD8).ToString() & ChrW(&HFF).ToString()
        Dim PNG_ID_BLOCK As String = ChrW(&H89).ToString() & "PNG" & vbCrLf & ChrW(&H1A).ToString() & vbLf
        Dim GIF_ID_BLOCK As String = "GIF8"
        Dim TIFF_ID_BLOCK As String = "II*" & ChrW(&H0).ToString()

        Dim imageBytes() As Byte

        ' Get a UTF7 Encoded string version
        Dim u7 As System.Text.Encoding = System.Text.Encoding.UTF7
        Dim strTemp As String = u7.GetString(oleFieldBytes)
        Dim st2 As String = System.Text.Encoding.UTF8.GetString(oleFieldBytes)
        ' Get the first 300 characters from the string
        Dim strVTemp As String = strTemp.Substring(0, 300)

        ' Search for the block
        Dim iPos As Integer = -1
        If strVTemp.IndexOf(BITMAP_ID_BLOCK) <> -1 Then
            iPos = strVTemp.IndexOf(BITMAP_ID_BLOCK)
        ElseIf strVTemp.IndexOf(JPG_ID_BLOCK) <> -1 Then
            iPos = strVTemp.IndexOf(JPG_ID_BLOCK)
        ElseIf strVTemp.IndexOf(PNG_ID_BLOCK) <> -1 Then
            iPos = strVTemp.IndexOf(PNG_ID_BLOCK)
        ElseIf strVTemp.IndexOf(GIF_ID_BLOCK) <> -1 Then
            iPos = strVTemp.IndexOf(GIF_ID_BLOCK)
        ElseIf strVTemp.IndexOf(TIFF_ID_BLOCK) <> -1 Then
            iPos = strVTemp.IndexOf(TIFF_ID_BLOCK)
        Else
            Throw New Exception("Unable to determine header size for the OLE Object")
        End If


        ' From the position above get the new image
        If iPos = -1 Then
            Throw New Exception("Unable to determine header size for the OLE Object")
        End If
        imageBytes = New Byte(CInt(oleFieldBytes.LongLength - iPos - 1)) {}
        Array.ConstrainedCopy(oleFieldBytes, iPos, imageBytes, 0, oleFieldBytes.Length - iPos)

        Return imageBytes
    End Function

    Private Sub lblMainBack_Click(sender As Object, e As EventArgs) Handles lblMainBack.Click
        Call showClrDlg(lblMainBack)
        pnlLogin.BackColor = lblMainBack.BackColor
        tvMain.BackColor = lblMainBack.BackColor
    End Sub

    Private Sub lblMainFore_Click(sender As Object, e As EventArgs) Handles lblMainFore.Click
        Call showClrDlg(lblMainFore)
        lblUsername.ForeColor = lblMainFore.BackColor
        tvMain.ForeColor = lblMainFore.BackColor
    End Sub

    Private Sub lblChildBack_Click(sender As Object, e As EventArgs) Handles lblChildBack.Click
        Call showClrDlg(lblChildBack)
        pnlChild.BackColor = lblChildBack.BackColor
    End Sub

    Private Sub lblMainFont_Click(sender As Object, e As EventArgs) Handles lblMainFont.Click
        Dim FDialog As New FontDialog
        FDialog.ShowEffects = False
        FDialog.AllowScriptChange = False
        FDialog.Font = lblMainFont.Font
        If FDialog.ShowDialog = Windows.Forms.DialogResult.OK Then
            lblMainFont.Font = FDialog.Font
            tvMain.Font = FDialog.Font
            lblUsername.Font = FDialog.Font
        End If
    End Sub

    Private Sub lblChildMandatoryFore_Click(sender As Object, e As EventArgs) Handles lblChildMandatoryFore.Click
        Call showClrDlg(lblChildMandatoryFore)
        lblMandtory.ForeColor = lblChildMandatoryFore.BackColor
    End Sub

    Private Sub lblChildReadFore_Click(sender As Object, e As EventArgs) Handles lblChildReadFore.Click
        Call showClrDlg(lblChildReadFore)
        lblReadonly.ForeColor = lblChildReadFore.BackColor
    End Sub

    Private Sub lblChildOptionalFore_Click(sender As Object, e As EventArgs) Handles lblChildOptionalFore.Click
        Call showClrDlg(lblChildOptionalFore)
        lblOptional.ForeColor = lblChildOptionalFore.BackColor
    End Sub

    Private Sub tbFont_Scroll(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbFont.Scroll
        lblMandtory.Font = New Font("Verdana", tbFont.Value)
        lblReadonly.Font = New Font("Verdana", tbFont.Value)
        lblOptional.Font = New Font("Verdana", tbFont.Value)
        TextBox1.Font = New Font("Verdana", tbFont.Value)
        TextBox2.Font = New Font("Verdana", tbFont.Value)
        TextBox3.Font = New Font("Verdana", tbFont.Value)
        frmMain.tvMain.Font = New Font("Verdana", tbFont.Value)
        frmMain.txtSearch.Font = New Font("Verdana", tbFont.Value)
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Dim str As String, strSQL As String
        Try
            'Normal
            str = lblMainFore.BackColor.ToArgb & ", " & lblMainBack.BackColor.ToArgb & ", " & lblMainFont.Font.Name & "," & Math.Round(Val(lblMainFont.Font.Size)) & ", "
            str = str & lblMainFont.Font.Style
            strSQL = "Update UtlGSettings set SetValue = '" & str & "' where SetName= 'MainFormClrs' and SetGroup = 'COMPANY'"
            clsSVT.execSQLStatement(strSQL)

            str = lblChildMandatoryFore.BackColor.ToArgb & ", " & lblChildReadFore.BackColor.ToArgb & ", " & lblChildOptionalFore.BackColor.ToArgb & ", "
            str = str & lblChildBack.BackColor.ToArgb
            strSQL = "Update UtlGSettings set SetValue = '" & str & "' where SetName= 'FormClrs' and SetGroup = 'COMPANY'"
            clsSVT.execSQLStatement(strSQL)

            'str = lblMandtory.Font.Size & ", " & lblReadonly.Font.Size & ", " & lblOptional.Font.Size & ", " & TextBox1.Font.Size & ", " & TextBox2.Font.Size & ", " & TextBox3.Font.Size
            str = tbFont.Value
            strSQL = "Update UtlGSettings set SetValue = '" & str & "' where SetName= 'FontSize' and SetGroup = 'COMPANY'"
            clsSVT.execSQLStatement(strSQL)

            MsgBox("Updated Successfully!", vbInformation, Me.Text)
            Call loadFormColors()
            Call frmMain.SetMainFormColor()
            gintFontSize = CInt(str)
        Catch ex As Exception
            MsgBox("Update failed! Please check for valid data and retry.", vbCritical, Me.Text)
            Exit Sub
        End Try
    End Sub

    Private Sub btnClose_Click_1(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub
End Class