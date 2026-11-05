Imports System.Data.OleDb
Imports System.IO
Imports C1.Win.C1FlexGrid
Imports Microsoft.Office.Interop
Public Class frmUtlAutoEmailSch
    Dim clsSVT As New svtLibrary
    Public Mode As String
    Dim dt As New DataTable
    Dim dv As New DataView

    Const intRowFuncID = 0
    Const intRowFuncName = 1

    Const vSchUserID = 0
    Const vSchDept = 1
    Const vSchUserName = 2
    Const vSchUserEmailID = 3
    Const vSchTotCols = 4

    Private Sub initScreen()
        With vsfSch
            .Rows.Count = 2
            .Cols.Count = vSchTotCols
            .SetData(intRowFuncName, vSchUserID, "User ID")
            .SetData(intRowFuncName, vSchDept, "Department")
            .SetData(intRowFuncName, vSchUserName, "User Name")
            .SetData(intRowFuncName, vSchUserEmailID, "User eMail ID")


            SetFlexColor(vsfSch, "", vSchUserID, vSchUserName, vSchUserEmailID)
            SetHideCol(vsfSch, True, vSchUserID)
            SetHideRow(vsfSch, True, 0)
        End With

    End Sub

    Private Sub frmUtlAutoEmailSch_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        'Call initGridStyles(Me)
        Call fillFormProperties(Me)
        Call ChangeFontSize(Me)

        Call initScreen()
        Call loadSch()
        Call initFormVsfDet(Me, Me.Text, vsfSch, "SCH")
    End Sub

    Private Sub loadSch()
        Dim strSql As String
        Dim rsEmp As OleDb.OleDbDataReader

        strSql = "Select UserID,UserCode,EmailID,Department From MstUser where InActive='N' and Reporting='Y' order by UserCode"
        rsEmp = clsSVT.getDataReaderFromSQL(strSql)

        If rsEmp.HasRows Then
            With vsfSch
                .Rows.Count = 2
                .Cols.Count = vSchTotCols
                Do While rsEmp.Read
                    .Rows.Count = .Rows.Count + 1
                    .SetData(.Rows.Count - 1, vSchUserID, UCase(rsEmp!UserID))
                    .SetData(.Rows.Count - 1, vSchDept, UCase(rsEmp!Department))
                    .SetData(.Rows.Count - 1, vSchUserName, UCase(rsEmp!UserCode))
                    'setVsfCellColour(vsfSch, .Rows.Count - 1, vSchUserName, Color.LightGreen)
                    .SetData(.Rows.Count - 1, vSchUserEmailID, rsEmp!EMAILID)

                Loop
                .AutoResize = True
            End With
        End If
        rsEmp.Close()

        strSql = "Select Rptid, RptName FROM UtlAutoEmailSchHdr where Rptid>0 order by RptName"

        rsEmp = clsSVT.getDataReaderFromSQL(strSql)
        If rsEmp.HasRows Then
            With vsfSch
                Do While rsEmp.Read
                    .Cols.Count = .Cols.Count + 1
                    .SetData(intRowFuncID, .Cols.Count - 1, rsEmp!RptId)
                    .SetData(intRowFuncName, .Cols.Count - 1, rsEmp!RptName)
                    For r As Int32 = 2 To .Rows.Count - 1
                        .SetData(r, .Cols.Count - 1, clsSVT.getDataStringValue("Select SchType from UtlAutoEmailSchDtl where RPTID=" & rsEmp!RptId & " and USERID=" & Val(.GetData(r, vSchUserID)), "", False))
                    Next
                Loop
            End With
        End If
        rsEmp.Close()
    End Sub

    'Private Sub vsfSch_BeforeEdit(sender As Object, e As RowColEventArgs) Handles vsfSch.BeforeEdit
    '    e.Cancel = True
    '    If e.Col > vSchUserEmailID Then
    '        e.Cancel = False
    '        vsfSch.ComboList = " |Day|Week|Month"
    '    End If

    'End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        frmMain.CloseSelectedTabPage(Me)
    End Sub
    Private Sub vsfSch_KeyUp(sender As Object, e As KeyEventArgs) Handles vsfSch.KeyUp
        If e.KeyCode = Keys.F9 And e.Control = True Then
            Call initFormVsfDet(Me, Me.Text, vsfSch, "SCH")
            MsgBox("Restored Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
        If e.KeyCode = Keys.F8 And e.Control = True Then
            Call SaveActFormVsfDet(Me, Me.Text, "SCH", vsfSch)
            MsgBox("Saved Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
    End Sub


    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        'If Not validFlds() Then Exit Sub

        'Dim strSql As String = "", strSchInfo As String = "", intUserID As Integer

        'With vsfSch
        '    For i As Int32 = 2 To .Rows.Count - 1
        '        intUserID = Val(.GetData(i, vSchUserID))
        '        clsSVT.execSQLStatement("Delete from UtlAutoEmailSch where USERID=" & intUserID)

        '        For c As Int32 = 4 To .Cols.Count - 1
        '            If .GetData(i, c) <> "" Then
        '                strSql = "Insert into UtlAutoEmailSch(UserID,DOCNAME,SCHTYPE) Values("
        '                strSql &= "" & intUserID & ","
        '                strSql &= "'" & .GetData(0, c) & "',"
        '                Select Case UCase(.GetData(i, c))
        '                    Case "DAY"
        '                        strSql &= "'D')"
        '                    Case "WEEK"
        '                        strSql &= "'W')"
        '                    Case "MONTH"
        '                        strSql &= "'M')"
        '                    Case Else
        '                        strSql &= "'')"

        '                End Select

        '                clsSVT.execSQLStatement(strSql)

        '            End If
        '        Next
        '    Next

        '    MsgBox("Email Scheduling Updated!", MsgBoxStyle.Information, Me.Text)

        'End With

        'Call clrFlds()

        'Call loadSch()
    End Sub


    Private Function validFlds() As Boolean
        validFlds = False
        With vsfSch
            For i As Int32 = 2 To .Rows.Count - 1

                For c As Int32 = 4 To .Cols.Count - 1
                    If Trim(.GetData(i, c)) <> "" Then
                        If Trim(.GetData(i, vSchUserEmailID)) = "" Then
                            MsgBox("The User " & .GetData(i, vSchUserName) & " does not have Email ID, Task cannot be scheduled!", MsgBoxStyle.Information, Me.Text)
                            Exit Function
                        End If
                    End If
                Next
            Next
        End With
        If optMonthWise.Checked Then
            If optMonthStartDt.Checked = False And optMonthEndDt.Checked = False Then
                validFlds = False
                MsgBox("Please select an option in Month wise", MsgBoxStyle.Information, Me.Text)
                Exit Function
            End If
        End If

        validFlds = True
    End Function

    Private Sub btnExport_Click(sender As Object, e As EventArgs) Handles btnExport.Click
        ExportToXL(vsfSch)
    End Sub

    Private Sub vsfSch_Click(sender As Object, e As EventArgs) Handles vsfSch.Click
        If vsfSch.Rows.Count < 3 Then Exit Sub
        btnRemSch.Text = "Remove All Schedules for " & vsfSch.GetData(vsfSch.Row, vSchUserName)

    End Sub

    Private Sub btnRemSch_Click(sender As Object, e As EventArgs) Handles btnRemSch.Click
        If MsgBox("Are you sure you want to delete all the schedules for the user " & vsfSch.GetData(vsfSch.Row, vsfSch.Col), MsgBoxStyle.Question + vbYesNo, Me.Text) = vbNo Then Exit Sub

        Dim intUserID As Integer

        With vsfSch
            intUserID = Val(.GetData(.Row, vSchUserID))
            clsSVT.execSQLStatement("Delete from UtlAutoEmailSchdtl where USERID=" & intUserID)

            For c As Int32 = 4 To .Cols.Count - 1
                .SetData(.Row, c, "")
            Next
        End With
        MsgBox("Email Schedules Deleted for the user " & vsfSch.GetData(vsfSch.Row, vsfSch.Col), MsgBoxStyle.Information, Me.Text)

    End Sub

    Private Sub addCols(strDocName As String, strDocAlias As String)
        With vsfSch
            .Cols.Count = .Cols.Count + 1
            .SetData(intRowFuncID, .Cols.Count - 1, strDocName)
            .SetData(intRowFuncName, .Cols.Count - 1, strDocAlias)
        End With
    End Sub

    Private Sub btnVewRpt_Click(sender As Object, e As EventArgs) Handles btnVewRpt.Click
        'gBlnViewRtnRpt
        Dim strFuncName As String
        strFuncName = vsfSch.GetData(intRowFuncID, vsfSch.Col)
        showWaitCursor()

        'gBlnViewRtnRpt = True
        'dtFirstDayofYr = "01/01/" & Year(getSystemDate())

        'frmMain.openRtnRpt(strFuncName)

        'gBlnViewRtnRpt = False

        showNormalCursor()
    End Sub

    Private Sub btnAddRpt_Click(sender As Object, e As EventArgs) Handles btnAddRpt.Click
        clrFlds()
        PnlRpt.Visible = True

    End Sub

    Private Sub vsfSch_CellButtonClick(sender As Object, e As RowColEventArgs) Handles vsfSch.CellButtonClick
        Dim strSchType As String, strSchDs As String

        With vsfSch
            pnlSchSave.Visible = True
            lblRptID.Text = Val(.GetData(0, .Col))
            lblRptName.Text = .GetData(1, .Col)
            lblUserId.Text = Val(.GetData(.Row, vSchUserID))
            strSchType = .GetData(.Row, .Col)
            strSchDs = clsSVT.getDataStringValue("Select SchDays from UtlAutoEmailSchDtl where RPTID=" & Val(.GetData(0, .Col)) & " and USERID=" & Val(.GetData(.Row, vSchUserID)), "", False)
        End With

        If strSchType = "" Then Exit Sub

        disableAllOpts()

        Select Case UCase(strSchType)
            Case "D"
                optDayWise.Checked = True

            Case "ME"
                optMonthWise.Checked = True
                optMonthEndDt.Checked = True
                pnlMonthWise.Visible = True

            Case "MS"
                optMonthWise.Checked = True
                optMonthStartDt.Checked = True
                pnlMonthWise.Visible = True

            Case "DS"
                optSelDays.Checked = True
                pnlShowDays.Visible = True
                If strSchDs = "" Then Exit Sub
                For Each str As String In strSchDs.Split(",")
                    If str = "1" Then chkMonday.Checked = True
                    If str = "2" Then chkTuesday.Checked = True
                    If str = "3" Then chkWednesday.Checked = True
                    If str = "4" Then chkThursday.Checked = True
                    If str = "5" Then chkFriday.Checked = True
                    If str = "6" Then chkSaturday.Checked = True
                    If str = "7" Then chkSunday.Checked = True

                Next
        End Select
    End Sub

    Private Sub disableAllOpts()
        optDayWise.Checked = False
        optSelDays.Checked = False
        optMonthWise.Checked = False
        optMonthStartDt.Checked = False
        optMonthEndDt.Checked = False
        chkMonday.Checked = False
        chkTuesday.Checked = False
        chkWednesday.Checked = False
        chkThursday.Checked = False
        chkFriday.Checked = False
        chkSaturday.Checked = False
        chkSunday.Checked = False
        pnlMonthWise.Visible = False
        pnlShowDays.Visible = False
    End Sub

    Private Sub btnRptSave_Click(sender As Object, e As EventArgs) Handles btnRptSave.Click
        Dim strSql As String

        If clsSVT.getDataLongValue("Select RPTID from UtlAutoEmailSchHdr where RPTNAME='" & Trim(txtRptName.Text) & "'", 0, False) > 0 Then

            strSql = "Update UtlAutoEmailSchHdr set "
            strSql &= " RPTNAME='" & Trim(txtRptName.Text) & "',"
            strSql &= " AdnlEmailID='" & Trim(txtAddEmail.Text) & "',"
            strSql &= "InActive='" & IIf(ChkInAct.Checked, "Y", "N") & "',"
            strSql &= "Remarks='" & Trim(txtRmk.Text) & "'"
            strSql &= " where Rptid=" & Val(vsfSch.GetData(intRowFuncID, vsfSch.Col))

            clsSVT.execSQLStatement(strSql)

            MsgBox("Updated Sucessfully!", MsgBoxStyle.Information, Me.Text)

        Else
            strSql = "Insert into UtlAutoEmailSchHdr(RPTNAME,AdnlEmailID,InActive,Remarks)"
            strSql &= " values("
            strSql &= "'" & Trim(txtRptName.Text) & "',"
            strSql &= "'" & Trim(txtAddEmail.Text) & "',"
            strSql &= "'" & IIf(ChkInAct.Checked, "Y", "N") & "',"
            strSql &= "'" & Trim(txtRmk.Text) & "')"
            clsSVT.execSQLStatement(strSql)
            MsgBox("Report Saved!", MsgBoxStyle.Information, Me.Text)
        End If


        PnlRpt.Visible = False
    End Sub

    Private Sub btnSchSave_Click(sender As Object, e As EventArgs) Handles btnSchSave.Click

        If Not validFlds() Then Exit Sub

        Dim strSql As String = "", strSchInfo As String = "", strSelDays As String = ""
        Dim strSchType As String

        strSchType = ""
        strSchInfo = ""
        strSelDays = ""

        If optDayWise.Checked Then
            strSchType = "D"
        End If

        If optSelDays.Checked Then
            strSchType = "DS"
            strSchInfo = Trim(getDayNumber())
            If Strings.Right(strSchInfo, 1) = "," Then
                strSchInfo = Strings.Left(strSchInfo, Len(strSchInfo) - 1)
            End If
        End If

        If optMonthWise.Checked Then
            If optMonthStartDt.Checked Then
                strSchType = "MS"
            Else
                strSchType = "ME"
            End If
        End If

        If clsSVT.getDataLongValue("Select RPTID from UtlAutoEmailSchDtl where  RPTID=" & Val(lblRptID.Text) & " and USERID= " & Val(lblUserId.Text), 0, False) > 0 Then

            strSql = "Update UtlAutoEmailSchDtl set "
            strSql &= " RPTID=" & Val(lblRptID.Text) & ","
            strSql &= " USERID=" & Val(lblUserId.Text) & ","
            strSql &= "SchType='" & Trim(strSchType) & "',"
            strSql &= "SchDays='" & Trim(strSchInfo) & "'"
            strSql &= " where RPTID=" & Val(lblRptID.Text) & " and USERID= " & Val(lblUserId.Text)

            clsSVT.execSQLStatement(strSql)

            MsgBox("Updated Sucessfully!", MsgBoxStyle.Information, Me.Text)
        Else
            strSql = "Insert into UtlAutoEmailSchDtl(RPTID,USERID,SchType,SchDays)"
            strSql &= " values("
            strSql &= " " & Val(lblRptID.Text) & ","
            strSql &= "" & Val(lblUserId.Text) & ","
            strSql &= "'" & Trim(strSchType) & "',"
            strSql &= "'" & Trim(strSchInfo) & "')"

            clsSVT.execSQLStatement(strSql)

            MsgBox("Saved Sucessfully!", MsgBoxStyle.Information, Me.Text)

        End If
        Call clrFlds()

        Call loadSch()
    End Sub

    Private Sub btnRptCls_Click(sender As Object, e As EventArgs) Handles btnRptCls.Click
        clrFlds()
        PnlRpt.Visible = False
    End Sub
    Private Sub clrFlds()

        txtRptName.Text = ""
        txtAddEmail.Text = ""
        txtRmk.Text = ""
        ChkInAct.Checked = False

        optDayWise.Checked = False
        optMonthWise.Checked = False
        'optAllDays.Checked = False
        optSelDays.Checked = False
        optMonthStartDt.Checked = False
        optMonthEndDt.Checked = False
        chkMonday.Checked = False
        chkTuesday.Checked = False
        chkWednesday.Checked = False
        chkThursday.Checked = False
        chkFriday.Checked = False
        chkSaturday.Checked = False
        chkSunday.Checked = False

        pnlSchSave.Visible = False

        PnlRpt.Visible = False
    End Sub

    Private Sub vsfSch_BeforeEdit(sender As Object, e As RowColEventArgs) Handles vsfSch.BeforeEdit

        With vsfSch
            .ComboList = ""
            .EditMask = ""

            If .Cols.Count - 1 > 3 Then
                .ComboList = "..."
            End If
        End With
    End Sub

    Private Sub optDayWise_Click(sender As Object, e As EventArgs) Handles optDayWise.Click
        If optDayWise.Checked = True Then
            pnlShowDays.Visible = False
            pnlMonthWise.Visible = False
        Else
            pnlShowDays.Visible = True
            pnlMonthWise.Visible = True
        End If

    End Sub

    Private Sub optSelDays_Click(sender As Object, e As EventArgs) Handles optSelDays.Click
        If optSelDays.Checked Then
            pnlShowDays.Visible = True
            pnlMonthWise.Visible = False
        Else
            pnlShowDays.Visible = False
            pnlMonthWise.Visible = False
        End If
    End Sub

    Private Sub optMonthWise_Click(sender As Object, e As EventArgs) Handles optMonthWise.Click
        If optMonthWise.Checked Then
            pnlShowDays.Visible = False
            pnlMonthWise.Visible = True
        Else
            pnlShowDays.Visible = False
            pnlMonthWise.Visible = False
        End If
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        clrFlds()
        pnlSchSave.Visible = False
    End Sub

    Private Function getDayNumber() As String
        getDayNumber = ""

        If chkMonday.Checked Then getDayNumber = getDayNumber & "1,"
        If chkTuesday.Checked Then getDayNumber = getDayNumber & "2,"
        If chkWednesday.Checked Then getDayNumber = getDayNumber & "3,"
        If chkThursday.Checked Then getDayNumber = getDayNumber & "4,"
        If chkFriday.Checked Then getDayNumber = getDayNumber & "5,"
        If chkSaturday.Checked Then getDayNumber = getDayNumber & "6,"
        If chkSunday.Checked Then getDayNumber = getDayNumber & "7,"

    End Function

    Private Sub optOutLook_CheckedChanged(sender As Object, e As EventArgs) Handles optOutLook.CheckedChanged
        If optOutLook.Checked Then
            clsSVT.execSQLStatement("Update utlgsettings set SetValue='OUTLOOK' where setName = 'AUTOEMAILSRVR'")
            ' MsgBox("Auto Email Server updated to OutLook!", MsgBoxStyle.Information, Me.Text)
        End If
    End Sub

    Private Sub optSmtp_CheckedChanged(sender As Object, e As EventArgs) Handles optSmtp.CheckedChanged
        If optSmtp.Checked Then
            clsSVT.execSQLStatement("Update utlgsettings set SetValue='SMTP' where setName = 'AUTOEMAILSRVR'")
            'MsgBox("Auto Email Server updated to SMTP!", MsgBoxStyle.Information, Me.Text)
        End If
    End Sub

    Private Sub chkSelRpt_CheckedChanged(sender As Object, e As EventArgs) Handles chkSelRpt.CheckedChanged
        'Dim strSql As String
        'strSql = "Select Rptid, RptName FROM UtlAutoEmailSchHdr where Rptid>0 order by RptName"
        'Dim objFrm As New frmUtlMultiSelection
        'Call objFrm.InitVsfSel(strSql, 1, 1, True, Me, False, 2)
        'objFrm.Text = "Select columns to filter"
        'objFrm.vsfDet.SetData(0, 0, "Sel")
        'objFrm.ShowDialog()
        ''strRptList = strList

        ''If strRptList <> "" Then

        ''End If
    End Sub
    'Private Sub filterCols(argRptIDs As String)

    '    For Each intrpt As Integer In argRptIDs.Split(",")

    '    Next
    'End Sub

    'Private Sub cancelColFilter()

    'End Sub

    Private Sub vsfSch_DoubleClick(sender As Object, e As EventArgs) Handles vsfSch.DoubleClick
        clrFlds()
        PnlRpt.Visible = True

        With vsfSch
            txtRptName.Text = .GetData(intRowFuncName, .Col)
            txtAddEmail.Text = clsSVT.getDataStringValue("Select AdnlEmailID from UtlAutoEmailSchHdr where RPTID=" & Val(.GetData(intRowFuncID, .Col)), "", False)
            txtRmk.Text = clsSVT.getDataStringValue("Select Remarks from UtlAutoEmailSchHdr where RPTID=" & Val(.GetData(intRowFuncID, .Col)), "", False)
            If clsSVT.getDataStringValue("Select Inactive from UtlAutoEmailSchHdr where RPTID=" & Val(.GetData(intRowFuncID, .Col)), "N", False) = "Y" Then
                ChkInAct.Checked = True
            Else
                ChkInAct.Checked = False
            End If
        End With
    End Sub
End Class