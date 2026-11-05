Imports C1.Win.C1FlexGrid
Imports System.IO

Public Class frmUtlDashSettings
    Dim svttrn As oledb.oledbTransaction
    Dim clsSVT As New svtLibrary
    Dim strRights As String
    Dim strSQL As String
    'Public objFrmSettings As Object
    Public argFrm As Object
    Dim Mode As String

    Const vDetDashID = 0
    Const vDetDasHShow = 1
    Const vDetSectionHdr = 2
    Const vDetOrderNo = 3
    Const vDetGroupHdr = 4
    Const vDetInfoHdr = 5
    Const vDetInfoType = 6
    Const vDetDashQuery = 7
    Const vDetTotcols = 8



    Private Sub frmUtlDashBoardRight_Load(sender As Object, e As EventArgs) Handles Me.Load

        Me.KeyPreview = True
        fillFormProperties(Me)
        initGridStyles(Me)
        initFormSize(Me, Me.Text, False, 642, 469, 0, 0)
        initFormVsfDet(Me, Me.Text, vsfDet, "DET")
        Mode = "V"
        PnlD1.Visible = False
        initscreen()
        pnlImage.Visible = False
        'btnAdd.Visible = False
        'btnDelete.Visible = False
        initFormSize(Me, Me.Text, False, 642, 469, 0, 0)
        initFormVsfDet(Me, Me.Text, vsfDet, "DET")
        fillDocDetails()
        If clsSVT.getDataStringValue("Select ShowAtStartUp From UtlDashSettings", "", False) = "Y" Then
            chkShow.Checked = True
        Else
            chkShow.Checked = False
        End If
    End Sub

    Private Sub initscreen()
        With vsfDet
            .Cols.Count = vDetTotcols
            .Cols.Fixed = 0
            .Rows.Count = 2

            .SetData(0, vDetDashID, "Dash Id")
            .SetData(0, vDetDasHShow, "Dash Show")
            .SetData(0, vDetSectionHdr, "Section Header")
            .SetData(0, vDetOrderNo, "OrderNo")
            .SetData(0, vDetGroupHdr, "Group Header")
            .SetData(0, vDetInfoHdr, "Info Header")
            .SetData(0, vDetInfoType, "Info Type")
            .SetData(0, vDetDashQuery, "DashQuery")


            .Cols(vDetDasHShow).DataType = GetType(Boolean)
            SetFlexColorFullGrid(vsfDet, "")

            SetHideCol(vsfDet, True, vDetDashID)
        End With
    End Sub

    'Private Sub btnSelUser_Click(sender As Object, e As EventArgs)
    '    Dim StrSql As String

    '    StrSql = "Select Userid ,UserCode,Designation "
    '    StrSql &= " from MstUser "
    '    StrSql &= " where Authorised ='Y'"

    '    Dim frmobj As New frmUtlSingleSel
    '    With frmobj
    '        .Text = "Select User"
    '        .FillViewVsf(Me, StrSql, txtDummy, 0, 0)
    '        .StartPosition = FormStartPosition.CenterScreen
    '        .ShowDialog()
    '    End With

    '    If Val(txtDummy.Text) > 0 Then
    '        fillDocDetails(Val(txtDummy.Text))
    '    End If
    'End Sub

    Private Sub fillDocDetails()
        Dim i As Int32
        Dim rs As oledb.oledbDataReader = Nothing

        strSQL = " select * from UtldashQry"
        strSQL &= " order by SectionHdr ,OrderNo"
        rs = clsSVT.getDataReaderFromSQL(strSQL)

        With vsfDet
            If rs.HasRows Then
                .Rows.Count = 1
                Do While rs.Read()
                    .Rows.Count = .Rows.Count + 1
                    i = .Rows.Count - 1
                    .SetData(i, vDetDashID, GetVal(rs("RecID"), 0))
                    .SetData(i, vDetSectionHdr, GetVal(rs("SectionHdr"), ""))
                    .SetData(i, vDetOrderNo, GetVal(rs("Orderno"), 0))
                    .SetData(i, vDetGroupHdr, GetVal(rs("GroupHeader"), ""))
                    .SetData(i, vDetInfoHdr, GetVal(rs("InfoHeader"), ""))
                    .SetData(i, vDetInfoType, GetVal(rs("InfoType"), ""))
                    .SetData(i, vDetDashQuery, GetVal(rs("InfoQry"), ""))

                    If GetVal(rs("dashShow"), "") = "Y" Then
                        .SetCellCheck(i, vDetDasHShow, C1.Win.C1FlexGrid.CheckEnum.Checked)
                    Else
                        .SetCellCheck(i, vDetDasHShow, C1.Win.C1FlexGrid.CheckEnum.Unchecked)
                    End If
                    'If clsSVT.getDataLongValue("Select DashID from  UtlDashBoardRights Where UserID=" & argUID & "  And DashID = " & GetVal(rs("DashID"), 0), 0, False) Then
                    'End If
                Loop
            Else
                .Rows.Count = 1
            End If
            rs.Close()

            '.AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.RestrictAll
            '.Cols(vDetDashQuery).AllowMerging = True
        End With
    End Sub


    Private Function vDetUserID() As Integer
        Throw New NotImplementedException()
    End Function

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    'Private Sub chkAll_CheckedChanged(sender As Object, e As EventArgs)
    '    With vsfDet
    '        If .Rows.Count <= 0 Then Exit Sub
    '        If chkAll.Checked = True Then
    '            For i As Int32 = 1 To .Rows.Count - 1
    '                .SetData(i, vDetVisible, True)
    '            Next
    '        Else
    '            For i As Int32 = 1 To .Rows.Count - 1
    '                .SetData(i, vDetVisible, False)
    '            Next
    '        End If
    '    End With
    'End Sub



    'Private Sub txtUser_TextChanged(sender As Object, e As EventArgs) Handles txtUser.TextChanged
    '    ' Call fillDocDetails()
    'End Sub



    Private Sub btnChangeImg_Click(sender As Object, e As EventArgs) Handles btnBkg.Click
        pnlImage.Visible = True
        pnlImage.BringToFront()
        FillColrSettings()
    End Sub

    Public Sub FillColrSettings()
        Dim rs As oledb.oledbDataReader
        Dim strSQL As String
        Dim HdrFont As String
        Dim infoFont As String
        Dim HdrStyle As String
        Dim infoStyle As String
        Dim HdrSize As Int32
        Dim infosize As Int32

        strSQL = "Select * from UtlDashSettings"
        Try
            rs = clsSVT.getDataReaderFromSQL(strSQL)
            If rs.HasRows Then
                rs.Read()
                If rs("ShowDashBoard") = "Y" Then
                    chkSettingsShow.Checked = True
                Else
                    chkSettingsShow.Checked = False
                End If
                pnlDet1.BackColor = System.Drawing.Color.FromArgb(rs("SalesBGClr"))
                pnlDet2.BackColor = System.Drawing.Color.FromArgb(rs("PurchaseBGClr"))
                pnlDet3.BackColor = System.Drawing.Color.FromArgb(rs("StockBGClr"))
                pnlDet4.BackColor = System.Drawing.Color.FromArgb(rs("MiscBGClr"))


                lblSales.ForeColor = System.Drawing.Color.FromArgb(rs("SalesFrClr"))
                lblPB.ForeColor = System.Drawing.Color.FromArgb(rs("PurchaseFrClr"))
                lblStk.ForeColor = System.Drawing.Color.FromArgb(rs("StockFrClr"))
                lblMisc.ForeColor = System.Drawing.Color.FromArgb(rs("MiscFrClr"))


                HdrFont = rs("HdrFont")
                infoFont = rs("InfoFont")
                infosize = rs("InfoSize")
                HdrSize = rs("HdrSize")
                ''header
                lblHdr.Font = New Font(HdrFont, HdrSize,
                      FontStyle.Bold)
                ''values
                lblSales.Font = New Font(infoFont, infosize,
                     FontStyle.Regular)
                lblPB.Font = New Font(infoFont, infosize,
                    FontStyle.Regular)
                lblStk.Font = New Font(infoFont, infosize,
                    FontStyle.Regular)
                lblMisc.Font = New Font(infoFont, infosize,
                    FontStyle.Regular)
            End If

        Catch ex As Exception

            MsgBox("Error While Applying Colors", MsgBoxStyle.Critical, Me.Text)
        End Try
    End Sub

    Private Sub btnEdit_Click(sender As Object, e As EventArgs) Handles btnEdit.Click
        If vsfDet.Row = -1 Then Exit Sub
        If vsfDet.Rows.Count > 1 Then
            Mode = "M"
            TxtDashid.Enabled = False
            Call FillEditDtlGrid(vsfDet.GetData(vsfDet.Row, vDetDashID))
        End If
        btnAdd.Visible = False
        btnDelete.Visible = False
    End Sub
    Private Sub FillEditDtlGrid(ByVal argDashID As String)
        PnlD1.Visible = True
        TxtDashid.Text = argDashID
        With vsfDet

            TxtdashQuery.Text = .GetData(.Row, vDetDashQuery)
            TxtdashQuery.Text = TxtdashQuery.Text.Replace("`", "'")
            cboSecHdr.Text = .GetData(.Row, vDetSectionHdr)
            txtOrderNo.Text = .GetData(.Row, vDetOrderNo)
            txtGrpHdr.Text = .GetData(.Row, vDetGroupHdr)
            txtInfoHdr.Text = .GetData(.Row, vDetInfoHdr)
            cboInfoType.Text = .GetData(.Row, vDetInfoType)
            If vsfDet.GetCellCheck(.Row, vDetDasHShow) = CheckEnum.Checked Then
                chkVisible.Checked = True
            Else
                chkVisible.Checked = False
            End If

        End With

    End Sub


    Private Function ValidFields() As Boolean
        ValidFields = False
        Dim blnFound As Boolean = False
        If Mode = "V" Then Exit Function
        If Trim(TxtDashid.Text) = "" Then
            MsgBox("Dashid should not be empty!", MsgBoxStyle.Information, Me.Text)
            TxtDashid.Focus()
            Exit Function
        End If

        If Trim(TxtdashQuery.Text) = "" Then
            MsgBox("DashQry should not be empty!", MsgBoxStyle.Information, Me.Text)
            TxtdashQuery.Focus()
            Exit Function
        End If
        If Trim(txtGrpHdr.Text) = "" Then
            MsgBox("Group Header should not be empty!", MsgBoxStyle.Information, Me.Text)
            txtGrpHdr.Focus()
            Exit Function
        End If
        If Trim(cboInfoType.Text) = "" Then
            MsgBox("InfoType should not be empty!", MsgBoxStyle.Information, Me.Text)

            Exit Function
        End If
        If Trim(cboSecHdr.Text) = "" Then
            MsgBox("Section Header should not be empty!", MsgBoxStyle.Information, Me.Text)
            Exit Function
        End If
        ValidFields = True
    End Function

    Private Function saveDetails() As Boolean
        Dim strSQL As String = ""
        Dim strValues As String = ""
        Dim strFlds As String = ""
        saveDetails = False
        TxtdashQuery.Text = TxtdashQuery.Text.Replace("'", "`")
        Try
            With vsfDet
                If Mode = "A" Then

                    strSQL = "Insert into UtldashQry ("
                    strSQL = strSQL & "InfoQry,SectionHdr,Orderno,GroupHeader,InfoHeader,InfoType,DashShow"
                    strSQL = strSQL & ") values ("

                    strSQL = strSQL & "'" & Trim(TxtdashQuery.Text) & "',"
                    strSQL = strSQL & "'" & Trim(cboSecHdr.Text) & "',"
                    strSQL = strSQL & "'" & Trim(txtOrderNo.Text) & "',"
                    strSQL = strSQL & "'" & Trim(txtGrpHdr.Text) & "',"
                    strSQL = strSQL & "'" & Trim(txtInfoHdr.Text) & "',"
                    strSQL = strSQL & "'" & Trim(cboInfoType.Text) & "',"
                    If chkVisible.Checked = True Then
                        strSQL = strSQL & "'Y')"
                    Else
                        strSQL = strSQL & "'N')"
                    End If

                Else
                    strSQL = "update UtldashQry Set "
                    strSQL = strSQL & "InfoQry='" & TxtdashQuery.Text & "',"
                    strSQL = strSQL & "SectionHdr='" & cboSecHdr.Text & "',"
                    strSQL = strSQL & "Orderno='" & txtOrderNo.Text & "',"
                    strSQL = strSQL & "GroupHeader='" & txtGrpHdr.Text & "',"
                    strSQL = strSQL & "InfoHeader='" & txtInfoHdr.Text & "',"
                    strSQL = strSQL & "InfoType='" & cboInfoType.Text & "',"
                    If chkVisible.Checked = True Then
                        strSQL = strSQL & "DashShow= 'Y' "
                    Else
                        strSQL = strSQL & "DashShow= 'N'"
                    End If
                    strSQL = strSQL & " where RECID=" & Val(vsfDet.GetData(.Row, vDetDashID)) & ""

                End If
            End With

            If Not clsSVT.execSQLStatement(strSQL) Then
                Exit Function
            End If

            saveDetails = True

            If Mode = "A" Then
                MsgBox(" DashBoard ID '" & TxtDashid.Text & "' is added successfully.", vbInformation, Me.Text)
            Else
                MsgBox("Updated successfully.", vbInformation, Me.Text)
            End If
        Catch ex As Exception

            MsgBox("Error during save" & vbCrLf & ex.Message, MsgBoxStyle.Critical, Me.Text)
        End Try
    End Function
    Private Sub Clearflds()
        chkVisible.Checked = False
        TxtDashid.Text = ""
        TxtdashQuery.Text = ""
        txtInfoHdr.Text = ""
        txtGrpHdr.Text = ""
        txtOrderNo.Text = ""
        PnlD1.Visible = False

    End Sub
    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Mode = ""
        Clearflds()
        btnEdit.Visible = True
        btnDelete.Visible = True
        btnAdd.Visible = True
        TxtDashid.Enabled = True
    End Sub
    Private Sub TxtDashid_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TxtDashid.KeyPress
        If Not IsNumeric(e.KeyChar) And Not e.KeyChar = Chr(Keys.Delete) And Not e.KeyChar = Chr(Keys.Back) Then
            e.Handled = True
        End If
    End Sub

    Private Sub VsfDet_DoubleClick(sender As Object, e As EventArgs) Handles vsfDet.DoubleClick
        If vsfDet.Rows.Count > 1 Then
            Mode = "M"
            Call FillEditDtlGrid(vsfDet.GetData(vsfDet.Row, vDetDashID))
        End If
        btnAdd.Visible = False
        btnDelete.Visible = False
    End Sub
    Private Sub VsfDet_BeforeEdit(sender As Object, e As RowColEventArgs) Handles vsfDet.BeforeEdit
        e.Cancel = True
    End Sub

    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click

        'If Not chkUserRights(strRights, chkRightsPos.AddRights, Me.Text) Then Exit Sub
        'vsfDet.Rows.Count = vsfDet.Rows.Count + 1
        Mode = "A"
        Clearflds()
        Dim Intmaxid As Integer = 0
        TxtDashid.Enabled = True
        Intmaxid = clsSVT.getDataLongValue("Select max (Recid)  from UtldashQry", 0, False)
        TxtDashid.Text = Intmaxid + 1

        PnlD1.Visible = True
        btnEdit.Visible = False
        btnDelete.Visible = False
    End Sub

    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click

        'If Not chkUserRights(strRights, chkRightsPos.DelRights, Me.Text) Then Exit Sub
        'If MsgBox("Do You want to remove selected Row?", MsgBoxStyle.Information + MsgBoxStyle.YesNo, Me.Text) = MsgBoxResult.Yes Then
        '    vsfDet.Rows.Remove(vsfDet.Row)
        '    If vsfDet.Rows.Count = 1 Then vsfDet.Rows.Count = 2
        'End If
        'Call fillDocDetails()

        If vsfDet.Row = -1 Then Exit Sub
        If vsfDet.Rows.Count > 1 Then
            If MsgBox("Are you sure you want to delete the DashQry: '" & vsfDet.GetData(vsfDet.Row, vDetDashID) & "'?", MsgBoxStyle.YesNo + vbDefaultButton2 + MsgBoxStyle.Critical, "Deletion...") = vbNo Then Exit Sub


            If Not clsSVT.execSQLStatement("Delete from UtldashQry where RecidID = " & vsfDet.GetData(vsfDet.Row, vDetDashID)) Then
                Exit Sub
            End If

            MsgBox("Deleted Successfully!", vbInformation, Me.Text)

        End If
        Call fillDocDetails()
    End Sub

    Private Sub BtnSave_Click(sender As Object, e As EventArgs) Handles BtnSave.Click
        If Not ValidFields() Then Exit Sub
        If Not saveDetails() Then Exit Sub
        fillDocDetails()
        btnAdd.Visible = True
        btnEdit.Visible = True
        btnDelete.Visible = True
        PnlD1.Visible = False


    End Sub

    Private Sub chkShow_CheckedChanged(sender As Object, e As EventArgs) Handles chkShow.CheckedChanged

        If chkShow.Checked Then
            clsSVT.execSQLStatement("Update UtlDashSettings set ShowAtStartUp  = 'Y'")
        Else
            clsSVT.execSQLStatement("Update UtlDashSettings set ShowAtStartUp  = 'N'")
        End If
    End Sub

    Private Sub frmUtlDashSettings_KeyUp(sender As Object, e As KeyEventArgs) Handles Me.KeyUp
        If e.KeyCode = Keys.F9 And e.Control = True Then
            Call initFormSize(Me, Me.Text, True, 642, 469, 0, 0)
            Call initFormVsfDet(Me, Me.Text, vsfDet, "DET")
            MsgBox("Restored Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
        If e.KeyCode = Keys.F8 And e.Control = True Then
            Call SaveActFormSettings(Me, Me.Text)
            Call SaveActFormVsfDet(Me, Me.Text, "DET", vsfDet)
            MsgBox("Saved Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
    End Sub
    Private Sub pnlDet2_Click(sender As Object, e As EventArgs) Handles pnlDet2.Click
        If CDColor.ShowDialog <> Windows.Forms.DialogResult.Cancel Then
            pnlDet2.BackColor = CDColor.Color
        End If
    End Sub

    Private Sub pnlDet3_Click(sender As Object, e As EventArgs) Handles pnlDet3.Click
        If CDColor.ShowDialog <> Windows.Forms.DialogResult.Cancel Then
            pnlDet3.BackColor = CDColor.Color
        End If
    End Sub

    Private Sub pnlDet4_Click(sender As Object, e As EventArgs) Handles pnlDet4.Click
        If CDColor.ShowDialog <> Windows.Forms.DialogResult.Cancel Then
            pnlDet4.BackColor = CDColor.Color
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles btnClrCancel.Click
        pnlImage.Visible = False
    End Sub

    Private Sub btnClrSave_Click(sender As Object, e As EventArgs) Handles btnClrSave.Click
        Dim StrSql As String = ""

        lblinfoFont.Text = lblSales.Font.Name
        lblinfoSize.Text = lblSales.Font.Size
        lblInfoStyle.Text = lblSales.Font.Style
        lblHdrFont.Text = lblHdr.Font.Name
        lblHdrSize.Text = lblHdr.Font.Size
        lblHdrStyle.Text = lblHdr.Font.Style

        clsSVT.execSQLStatement("delete From UtlDashSettings ")

        StrSql = "Insert Into UtlDashSettings"
        StrSql &= "(ShowDashBoard,SalesBGClr,PurchaseBGClr,StockBGClr,MiscBGClr "
        StrSql &= " ,SalesFrClr,PurchaseFrClr,StockFrClr,MiscFrClr"
        StrSql &= " ,hdrFont,HdrSize,HdrStyle,InfoFont,InfoSize,InfoStyle"
        StrSql &= " )"
        StrSql &= " values("
        If chkSettingsShow.Checked Then
            StrSql &= "'Y',"
        Else
            StrSql &= "'N',"
        End If

        StrSql &= "'" & pnlDet1.BackColor.ToArgb & "',"
        StrSql &= "'" & pnlDet2.BackColor.ToArgb & "',"
        StrSql &= "'" & pnlDet3.BackColor.ToArgb & "',"
        StrSql &= "'" & pnlDet4.BackColor.ToArgb & "',"

        StrSql &= "'" & lblSales.ForeColor.ToArgb & "',"
        StrSql &= "'" & lblPB.ForeColor.ToArgb & "',"
        StrSql &= "'" & lblStk.ForeColor.ToArgb & "',"
        StrSql &= "'" & lblMisc.ForeColor.ToArgb & "',"

        StrSql &= "'" & lblHdrFont.Text & "',"
        StrSql &= "" & lblHdrSize.Text & ","
        StrSql &= "'" & lblHdrStyle.Text & "',"

        StrSql &= "'" & lblinfoFont.Text & "',"
        StrSql &= "" & lblinfoSize.Text & ","
        StrSql &= "'" & lblInfoStyle.Text & "'"



        StrSql &= ")"
        clsSVT.execSQLStatement(StrSql)

        MsgBox("Saved SuccessFully", MsgBoxStyle.Information, Me.Text)
        pnlImage.Visible = False
        Call argFrm.FillSettingClr()
    End Sub

    Private Sub pnlDet1_Click(sender As Object, e As EventArgs) Handles pnlDet1.Click
        If CDColor.ShowDialog <> Windows.Forms.DialogResult.Cancel Then
            pnlDet1.BackColor = CDColor.Color
        End If
    End Sub

    Private Sub btnMoveUp_Click(sender As Object, e As EventArgs) Handles btnMoveUp.Click
        With vsfDet
            If .Row > 1 Then
                If .GetData(.Row - 1, vDetSectionHdr) = .GetData(.Row, vDetSectionHdr) Then
                    MoveOneRowUp(vsfDet, vDetOrderNo)
                End If
            End If
        End With
        assignSlNos()
    End Sub

    Private Sub btnMoveDown_Click(sender As Object, e As EventArgs) Handles btnMoveDown.Click
        With vsfDet
            If .Row > 1 Then
                If .GetData(.Row + 1, vDetSectionHdr) = .GetData(.Row, vDetSectionHdr) Then
                    MoveOneRowDown(vsfDet, vDetOrderNo)
                End If
            End If
        End With
        assignSlNos()
    End Sub

    Private Sub assignSlNos()

        Dim i As Integer
        Dim intCtr As Integer
        With vsfDet
            .Col = vDetSectionHdr
            intCtr = 0
            For i = 1 To .Rows.Count - 1
                If .GetData(i, vDetSectionHdr) = "SALES" Then
                    intCtr = intCtr + 1
                    .SetData(i, vDetOrderNo, intCtr)
                End If
            Next i
            intCtr = 0
            For i = 1 To .Rows.Count - 1
                If .GetData(i, vDetSectionHdr) = "PURCHASE" Then
                    intCtr = intCtr + 1
                    .SetData(i, vDetOrderNo, intCtr)
                End If
            Next i
            intCtr = 0
            For i = 1 To .Rows.Count - 1
                If .GetData(i, vDetSectionHdr) = "STOCK" Then
                    intCtr = intCtr + 1
                    .SetData(i, vDetOrderNo, intCtr)
                End If
            Next i
            intCtr = 0
            For i = 1 To .Rows.Count - 1
                If .GetData(i, vDetSectionHdr) = "MISC" Then
                    intCtr = intCtr + 1
                    .SetData(i, vDetOrderNo, intCtr)
                End If
            Next i
        End With
    End Sub
    Private Sub TxtdashQuery_LostFocus(sender As Object, e As EventArgs) Handles TxtdashQuery.LostFocus
        Dim rs As oledb.oledbDataReader

        Dim intRecordcount As Int32 = 0
        Dim intfldCnt As Int32 = 0
        If Trim(TxtdashQuery.Text) = "" Then Exit Sub
        Try
            rs = clsSVT.getDataReaderFromSQL(Trim(TxtdashQuery.Text))
            If rs.HasRows Then
                Do While rs.Read
                    intfldCnt = rs.FieldCount
                    intRecordcount = intRecordcount + 1
                Loop
                If intRecordcount > 1 Or intfldCnt > 1 Then
                    cboInfoType.Text = "GRIDVALUE"
                Else
                    cboInfoType.Text = "SINGLEVALUE"
                End If
            End If
        Catch ex As Exception
            MsgBox("Error in DashQuery", MsgBoxStyle.Critical, Me.Text)
            TxtdashQuery.Focus()
        End Try
    End Sub

    Private Sub lblSales_Click(sender As Object, e As EventArgs) Handles lblSales.Click

        If CDColor.ShowDialog <> Windows.Forms.DialogResult.Cancel Then
            lblSales.ForeColor = CDColor.Color
        End If

    End Sub
    Private Sub lblPB_Click(sender As Object, e As EventArgs) Handles lblPB.Click
        If CDColor.ShowDialog <> Windows.Forms.DialogResult.Cancel Then
            lblPB.ForeColor = CDColor.Color
        End If

    End Sub
    Private Sub lblStk_Click(sender As Object, e As EventArgs) Handles lblStk.Click
        If CDColor.ShowDialog <> Windows.Forms.DialogResult.Cancel Then
            lblStk.ForeColor = CDColor.Color
        End If

    End Sub
    Private Sub lblMisc_Click(sender As Object, e As EventArgs) Handles lblMisc.Click
        If CDColor.ShowDialog <> Windows.Forms.DialogResult.Cancel Then
            lblMisc.ForeColor = CDColor.Color
        End If

    End Sub
    Private Sub fillfont()
        If cdlg.ShowDialog <> Windows.Forms.DialogResult.Cancel Then

            If Mode = "FV" Then
                lblMisc.Font = cdlg.Font
                lblSales.Font = cdlg.Font
                lblPB.Font = cdlg.Font
                lblStk.Font = cdlg.Font
                lblinfoFont.Text = lblSales.Font.Name
                lblinfoSize.Text = lblSales.Font.Size
                lblInfoStyle.Text = lblSales.Font.Style

            End If

            If Mode = "FH" Then
                lblHdr.Font = cdlg.Font
                lblHdrFont.Text = lblHdr.Font.Name
                lblHdrSize.Text = lblHdr.Font.Size
                lblHdrStyle.Text = lblHdr.Font.Style
            End If

        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles btnFValue.Click
        Mode = "FV"
        fillfont()
    End Sub

    Private Sub Button1_Click_1(sender As Object, e As EventArgs) Handles btnFHdr.Click
        Mode = "FH"
        fillfont()
    End Sub

    Private Sub btnDefaultColrs_Click(sender As Object, e As EventArgs) Handles btnDefaultColrs.Click

        If MsgBox("Are you sure to you want to change to Defualt Color settings", MsgBoxStyle.YesNo, Me.Text) = MsgBoxResult.Yes Then
            DefaultSettings()
        End If

    End Sub

    Private Sub DefaultSettings()
        pnlDet1.BackColor = System.Drawing.Color.FromArgb(-1459415)
        pnlDet2.BackColor = System.Drawing.Color.FromArgb(-1092691)
        pnlDet3.BackColor = System.Drawing.Color.FromArgb(-12220075)
        pnlDet4.BackColor = System.Drawing.Color.FromArgb(-10834525)

        lblSales.ForeColor = Color.Black
        lblPB.ForeColor = Color.Black
        lblStk.ForeColor = Color.Black
        lblMisc.ForeColor = Color.Black

        lblHdr.Font = New Font("Segoe UI", 14,
                      FontStyle.Bold)
        lblSales.Font = New Font("Segoe UI", 12,
             FontStyle.Regular)
        lblPB.Font = New Font("Segoe UI", 12,
            FontStyle.Regular)
        lblStk.Font = New Font("Segoe UI", 12,
            FontStyle.Regular)
        lblMisc.Font = New Font("Segoe UI", 12,
            FontStyle.Regular)
    End Sub

End Class