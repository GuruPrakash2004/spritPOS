Imports C1.Win.C1FlexGrid
Imports System.IO
Imports System.Text.RegularExpressions
Imports System.Data.SqlClient
Imports System.Data.OleDb

Public Class frmMstNewComp

    Const vCDetCompName = 0
    Const vCDetDispCompName = 1
    Const vCDetAdd1 = 2
    Const vCDetAdd2 = 3
    Const vCDetAdd3 = 4
    Const vCDetEmail = 5

    Const vCDetCompAcnNo = 0
    Const vCDetBankName = 1

    Const cFrameMain = 0
    Const cFrameSel = 1
    Const cFrameEDB = 2
    Const cFrameFinish = 3

    Dim clsSVT As New svtLibrary
    Dim strMode As String
    Dim Mode As String

    Private Sub frmMstNewComp_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        Call initScreen()
    End Sub

    Private Sub initScreen()
        Me.KeyPreview = True

        With vsfCDET
            .Rows.Count = 6
            .Cols.Count = 2

            .SetData(vCDetCompName, 0, "Company Name")
            .SetData(vCDetDispCompName, 0, "Disp.Company Name")
            .SetData(vCDetAdd1, 0, "Address")
            .SetData(vCDetAdd2, 0, ".")
            .SetData(vCDetAdd3, 0, "..")
            .SetData(vCDetEmail, 0, "Email ID")
            .Cols(0).Width = 150
        End With
        Call SetFlexColor(vsfCDET, "M", vCDetCompName)
        vsfCDET.ExtendLastCol = True
        Call initDates()
        Call FillCompCombo()
    End Sub

    Private Sub initDates()
        Dim dtDate As Date
        dtDate = CDate(getSystemDate())
        If Month(dtDate) < 4 Then
            dtDate = CDate("01/04/" & Year(dtDate) - 1)
        Else
            dtDate = CDate("01/04/" & Year(dtDate))
        End If
        dtpFyr.Value = dtDate
        dtpTyr.Value = DateAdd("yyyy", 1, dtpFyr.Value).AddDays(-1)

    End Sub

    Private Sub FillCompCombo()
        Dim i As Integer
        Dim rsDummy As OleDb.OleDbDataReader

        rsDummy = clsSVT.getDataReaderFromSQL("Select * from MstComp order by CompID", , svtGlobeConn)

        cboDBTemplate.Items.Clear()

        With rsDummy
            Do While .Read
                cboDBTemplate.Items.Add(clsSVT.PadStr(Trim(!CompID), 5, TextAlignEnum.LeftCenter) & " | " & !CompName)
            Loop
            rsDummy.Close()
        End With

        For i = 0 To cboDBTemplate.Items.Count - 1
            cboDBTemplate.Text = cboDBTemplate.Items(i)
        Next i
        If cboDBTemplate.Text = "" Then
            cboDBTemplate.SelectedIndex = -1
        End If
    End Sub

    Private Sub fillGrid()
        Dim rsTemp As OleDb.OleDbDataReader
        Dim strSql As String

        strSql = "Select * from MstComp"

        rsTemp = clsSVT.getDataReaderFromSQL(strSql)
        With rsTemp
            Do While .Read()
                With vsfCDET
                    .SetData(vCDetCompName, 1, GetVal(rsTemp("CompName"), ""))
                    .SetData(vCDetAdd1, 1, GetVal(rsTemp("Address1"), ""))
                    .SetData(vCDetAdd2, 1, GetVal(rsTemp("Address2"), ""))
                    .SetData(vCDetAdd3, 1, GetVal(rsTemp("Address3"), ""))
                    .SetData(vCDetEmail, 1, GetVal(rsTemp("Email"), ""))
                End With
            Loop
            .Close()
        End With
    End Sub
    Private Sub btnCancel_Click(sender As System.Object, e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
        blnAppStarted = True
        frmMain.Close()
    End Sub

    Private Sub btnSave_Click(sender As System.Object, e As System.EventArgs) Handles btnNextMain.Click
        If validCompInfo() Then
            Call ShowFrame(cFrameSel)
        End If
    End Sub

    Private Function validCompInfo() As Boolean
        validCompInfo = False
        With vsfCDET
            If Trim(.GetData(vCDetCompName, 1)) = "" Then
                MsgBox("Company Name not entered!!", vbInformation, Me.Text)
                Exit Function
            End If
            If Trim(.GetData(vCDetAdd1, 1)) = "" Then
                MsgBox("At least ONE address Line should be entered!!", vbInformation, Me.Text)
                Exit Function
            End If
        End With
        If dtpFyr.Value > dtpTyr.Value Then
            MsgBox("Invalid Financial Year!!", vbInformation, Me.Text)
            Exit Function
        End If
        validCompInfo = True
    End Function

    Private Sub rdBtnTrnandMst_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles rdBtnTrnandMst.CheckedChanged
        If Not validExistDB() Then Exit Sub
        Call ShowFrame(cFrameFinish)
    End Sub

    Private Function validExistDB() As Boolean
        validExistDB = False
        Dim intDBTemplate As Integer
        Dim strDBTemplate As String

        intDBTemplate = Val(Mid(cboDBTemplate.Text, 1, 5))
        strDBTemplate = "C" & intDBTemplate & ".MDB"

        If Not File.Exists(gstrDataPath & strDBTemplate) Then
            MsgBox("Database " & gstrDataPath & strDBTemplate & " mentioned to be taken as TEMPLATE is not found!!", vbInformation, Me.Text)
            Exit Function
        End If

        validExistDB = True
    End Function

    Private Sub btnNextExisting_Click(sender As System.Object, e As System.EventArgs) Handles btnNextExisting.Click
        If rdbtnExistingDB.Checked Then
            Call ShowFrame(cFrameEDB)
            Exit Sub
        End If
        Call ShowFrame(cFrameFinish)
    End Sub

    Private Sub btnNextSel_Click(sender As System.Object, e As System.EventArgs) Handles btnNextSel.Click
        If rdbtnExistingDB.Checked Then
            Call ShowFrame(cFrameEDB)
            Exit Sub
        End If
        Call ShowFrame(cFrameFinish)
    End Sub

    Private Sub btnFinish_Click(sender As System.Object, e As System.EventArgs) Handles btnFinish.Click
        If rdbtnFromTemplate.Checked Then
            If Not File.Exists(gstrDataPath & "SVTFactIC.Mdb") Then
                MsgBox("Template File " & gstrDataPath & "SVTFactIC.Mdb Does not Exist!!" & vbCrLf & _
                " Please copy this file to the folder and then Create Company!!", vbInformation, Me.Text)
                Exit Sub
            End If
        End If

        If rdbtnDemoComp.Checked Then
            If Not File.Exists(gstrDataPath & "SvtDemo.Mdb") Then
                MsgBox("Template File " & gstrDataPath & "SvtDemo.MDB Does not Exist!!" & vbCrLf & _
                " Please copy this file to the folder and then Create Company!!", vbInformation, Me.Text)
                Exit Sub
            End If
        End If

        Call createComp()
        Me.Close()
    End Sub

    Private Sub createComp()
        Dim strSQL As String
        Dim StrQry As String = "", i As Integer = 0
        Dim intMaxID As Integer, strMDBName As String
        Dim intDBTemplate As Integer, strDBTemplate As String = ""

        intMaxID = clsSVT.getDataLongValue("select Max(CompID) + 1 from MstComp", 0, False, , svtGlobeConn)
        If intMaxID = 0 Then intMaxID = 1

        strMDBName = "C" & intMaxID & ".MDB"

        If rdbtnExistingDB.Checked Then
            intDBTemplate = Val(Mid(cboDBTemplate.Text, 1, 5))
            strDBTemplate = "C" & intDBTemplate & ".MDB"
        End If
        If rdbtnFromTemplate.Checked Then
            strDBTemplate = "SVTFactIC.Mdb"
        End If
        If rdbtnDemoComp.Checked Then
            strDBTemplate = "SvtDemo.MDB"
        End If

        File.Copy(gstrDataPath & strDBTemplate, gstrDataPath & strMDBName, False)

        'Insert into GLOBE MstComp
        strSQL = "Insert into MstComp (CompNO, CompDefault, CompName, DispCompName) "
        strSQL = strSQL & " values ("
        strSQL = strSQL & intMaxID & ","
        With vsfCDET
            If intMaxID = 1 Then
                strSQL = strSQL & "'Y', "
            Else
                strSQL = strSQL & "'N', "
            End If
            strSQL = strSQL & "'" & Trim(.GetData(vCDetCompName, 1)) & "', "
            strSQL = strSQL & "'" & Trim(.GetData(vCDetDispCompName, 1)) & "') "
        End With
        Dim cmd As SqlCommand = New SqlCommand(strSQL)
        svtGlobeConn.Open()
        cmd.ExecuteNonQuery()
        svtGlobeConn.Close()


        Call openAccessConn(gstrDataPath & strMDBName)

        'insert into C1, C2... MstComp

        strSQL = "Update MstComp Set "
        strSQL = strSQL & "CompNO = " & intMaxID & ","
        With vsfCDET
            strSQL = strSQL & "CompName = '" & (.GetData(vCDetCompName, 1)) & "', "
            strSQL = strSQL & "Address1 = '" & (.GetData(vCDetAdd1, 1)) & "', "
            strSQL = strSQL & "Address2 = '" & (.GetData(vCDetAdd2, 1)) & "', "
            strSQL = strSQL & "Address3 = '" & (.GetData(vCDetAdd3, 1)) & "', "
            strSQL = strSQL & "Email = '" & (.GetData(vCDetEmail, 1)) & "'"
        End With
        clsSVT.execSQLStatement(strSQL)

        If Not rdbtnDemoComp.Checked Then
            clsSVT.execSQLStatement("Delete from MstCompFinYear ")

            strSQL = "Insert into MstCompFinYear (yrFDate, yrTDate, "
            strSQL = strSQL & " Active"
            strSQL = strSQL & " ) values ("
            strSQL = strSQL & "'" & Format(dtpFyr.Value, "dd/MM/yyyy") & "',"
            strSQL = strSQL & "'" & Format(dtpTyr.Value, "dd/MM/yyyy") & "',"
            strSQL = strSQL & "'Y')"
            clsSVT.execSQLStatement(strSQL)
        End If

        clsSVT.execSQLStatement("Update UtlGlobe Set LoginReq = '" & IIf(chkLoginReq.Checked = True, "Y", "N") & "'")

        If rdBtnOnlyMasters.Checked Then Call removeTrnData()

        MsgBox("New Company Created Successfully!", vbInformation, Me.Text)
        MsgBox("Application will close. " _
                & " Please run the Application again [To Open with the above Connection].", vbInformation, Me.Text)
        blnAppStarted = True
        Call frmMain.Close()
        End
    End Sub

    Private Sub removeTrnData()
        Dim strTblName As String, i As Integer
        Dim restrictions() As String = New String(4) {}
        restrictions(3) = "Table"

        Dim dtAccessTbls As DataTable = Nothing
        dtAccessTbls = svtConn.GetSchema("Tables", restrictions)
        For i = 0 To dtAccessTbls.Rows.Count - 1 Step i + 1
            strTblName = dtAccessTbls.Rows(i)(2).ToString()
            If UCase(Mid(strTblName, 1, 3)) = "TRN" Then
                clsSVT.execSQLStatement("Delete from " & strTblName)
            End If
        Next
    End Sub

    Private Sub ShowFrame(argFrame As Integer)
        gbCompInfo.Visible = False
        gbExistingDB.Visible = False
        gbFinish.Visible = False
        gbSel.Visible = False
        Select Case argFrame
            Case cFrameEDB
                gbExistingDB.Visible = True
                gbExistingDB.BringToFront()
            Case cFrameFinish
                Call fillFinishDetails()
                gbFinish.Visible = True
                gbFinish.BringToFront()
            Case cFrameMain
                gbCompInfo.Visible = True
                gbCompInfo.BringToFront()
            Case cFrameSel
                If cboDBTemplate.SelectedIndex = 0 Then
                    rdbtnExistingDB.Enabled = False
                Else
                    rdbtnExistingDB.Enabled = True
                End If
                gbSel.Visible = True
                gbSel.BringToFront()
        End Select
    End Sub

    Private Sub fillFinishDetails()
        Dim strDet As String

        strDet = "Create New Company"
        strDet = strDet & vbCrLf
        strDet = strDet & vbCrLf
        strDet = strDet & "Name   : " & Trim(vsfCDET.GetData(vCDetCompName, 1)) & vbCrLf
        If Trim(vsfCDET.GetData(vCDetDispCompName, 1)) <> "" Then
            strDet = strDet & "Display Name: " & Trim(vsfCDET.GetData(vCDetDispCompName, 1)) & vbCrLf
        End If
        strDet = strDet & "Address: " & Trim(vsfCDET.GetData(vCDetAdd1, 1)) & vbCrLf
        If Trim(vsfCDET.GetData(vCDetAdd2, 1)) <> "" Then
            strDet = strDet & "       : " & Trim(vsfCDET.GetData(vCDetAdd2, 1)) & vbCrLf
        End If
        If Trim(vsfCDET.GetData(vCDetAdd3, 1)) <> "" Then
            strDet = strDet & "       : " & Trim(vsfCDET.GetData(vCDetAdd3, 1)) & vbCrLf
        End If
        strDet = strDet & vbCrLf
        strDet = strDet & "FinYear: From " & Format(dtpFyr.Value, "MMM yyyy") & " to " & Format(dtpTyr.Value, "MMM yyyy") & vbCrLf
        strDet = strDet & vbCrLf
        If rdbtnFromTemplate.Checked Then
            strDet = strDet & "Using Softvent Template"
        End If
        If rdbtnDemoComp.Checked Then
            strDet = strDet & "Using Demo Company"
        End If
        If rdbtnExistingDB.Checked Then
            strDet = strDet & "Using Company [ " & cboDBTemplate.Text & "]" & vbCrLf
            If rdBtnTrnandMst.Checked Then
                strDet = strDet & "               Transaction & Master "
            End If

            If rdBtnOnlyMasters.Checked Then
                strDet = strDet & "               Masters Only "
            End If
        End If
        strDet = strDet & vbCrLf
        strDet = strDet & vbCrLf
        If chkLoginReq.Checked = False Then
            strDet = strDet & "LOGIN (Security) NOT Required" & vbCrLf
        Else
            strDet = strDet & "LOGIN (Security) Required" & vbCrLf
        End If
        txtFinishDet.Text = strDet
    End Sub

    Private Sub vsfCDET_KeyPressEdit(sender As Object, e As C1.Win.C1FlexGrid.KeyPressEditEventArgs) Handles vsfCDET.KeyPressEdit
        If e.KeyChar = "'" Then
            e.Handled = True
        End If
    End Sub

    Private Sub vsfCDET_KeyUp(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles vsfCDET.KeyUp
        With vsfCDET
            If .Row = .Rows.Count - 1 Then
                btnNextMain.Focus()
            End If
        End With
    End Sub

    Private Sub vsfCDET_LeaveCell(sender As Object, e As System.EventArgs) Handles vsfCDET.LeaveCell
        With vsfCDET
            Dim strField As String = GetVal(.GetData(.Row, .Col), "")
            If strField.Length > 100 Then
                MsgBox("Allowed charaters are 100 only... Please reduce the lenght of the string", MsgBoxStyle.Information, Me.Text)
                .SetData(.Row, .Col, strField.Substring(1, 99))
                Exit Sub
            End If
        End With
    End Sub

    Private Sub vsfCDET_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles vsfCDET.LostFocus
        With vsfCDET
            If .GetData(vCDetCompName, 1) = "" Then
                MsgBox("Company name should not be blank!", vbInformation, Me.Text)
                .Focus()
                .Col = 1
                .Row = 0
                Exit Sub
            ElseIf .GetData(vCDetCompName, 1) = "" Then
                MsgBox("Display Company name should not be blank!", vbInformation, Me.Text)
                .Focus()
                .Col = 1
                .Row = vCDetDispCompName
            End If
        End With
    End Sub

    Private Sub vsfCDET_SetupEditor(sender As Object, e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfCDET.SetupEditor
        With vsfCDET
            Select Case e.Row
                Case vCDetAdd1, vCDetAdd2, vCDetAdd3
                    If TypeOf (vsfCDET.Editor) Is TextBox Then
                        Dim tb As TextBox = vsfCDET.Editor
                        tb.MaxLength = 100
                    End If
                Case vCDetCompName, vCDetDispCompName
                    If TypeOf (vsfCDET.Editor) Is TextBox Then
                        Dim tb As TextBox = vsfCDET.Editor
                        tb.MaxLength = 250
                    End If
            End Select
        End With
        Exit Sub
    End Sub

    Private Sub btnBackSel_Click(sender As Object, e As System.EventArgs) Handles btnBackSel.Click
        Call ShowFrame(cFrameMain)
    End Sub

    Private Sub btnBackExisting_Click(sender As Object, e As System.EventArgs) Handles btnBackExisting.Click
        Call ShowFrame(cFrameSel)
    End Sub

    Private Sub btnFinishBack_Click(sender As Object, e As System.EventArgs) Handles btnFinishBack.Click
        If rdbtnExistingDB.Checked Then
            Call ShowFrame(cFrameEDB)
            Exit Sub
        End If
        ShowFrame(cFrameSel)
    End Sub
End Class
