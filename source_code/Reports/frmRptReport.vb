Imports Microsoft.Office.Interop
Imports C1.Win.C1FlexGrid
Public Class frmRptReport
    Dim clsSVT As New svtLibrary

    Const vRptSlNo = 0
    Const vRptGrpName = 1
    Const vRptID = 2
    Const vRptName = 3
    Const vRptFrmName = 4

    Const vUSRSlNo = 0
    Const vUSRID = 1
    Const vUSRCode = 2
    Const vUSRName = 3

    Const vRGTSlNo = 0
    Const vRGTID = 1
    Const vRGTRptGrpName = 2
    Const vRGTRptName = 3
    Const vRGTRights = 4

    Dim strFileName As String
    Dim strFrmName As String
    Dim lngUserID As Long
    Private Sub btnClose_Click(sender As Object, e As System.EventArgs) Handles btnClose.Click
        Dim tb As New C1.Win.C1Command.C1DockingTabPage()
        Dim dockTabMain As New C1.Win.C1Command.C1DockingTab()
        Me.Dispose()
        tb.Controls.Clear()
        frmMain.dockTabMain.SelectedTab.Dispose()

    End Sub

    Private Sub frmRptDayLtr_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        Me.KeyPreview = True
        lngUserID = 0
        Call initScreen()
        Call FillReportGrid()
        txtid.Focus()
        If gUserCode = "ADMIN" Then
            btnRights.Enabled = True
            btnRights.Visible = True
        Else
            btnRights.Enabled = False
            btnRights.Visible = False
        End If
        initFormVsfDet(Me, Me.Text, vsfRpt, "Report")
    End Sub

    Private Sub initScreen()
        With vsfRpt
            .Rows.Count = 1
            .Cols.Count = 5
            .SetData(0, vRptSlNo, "SlNo")
            .SetData(0, vRptID, "Report No.")
            .SetData(0, vRptGrpName, "Group Name")
            .SetData(0, vRptName, "Report Name")
            .SetData(0, vRptFrmName, "Form Name")
        End With
        SetColAlign(vsfRpt, svtAlign.Right, vRptSlNo)
        SetColAlign(vsfRpt, svtAlign.Left, vRptGrpName, vRptFrmName, vRptName)
        SetColAlign(vsfRpt, svtAlign.Center, vRptID)
        SetFlexColor(vsfRpt, "", vRptSlNo, vRptID, vRptGrpName, vRptName, vRptFrmName)
        SetHideCol(vsfRpt, True, vRptFrmName, vRptSlNo)

        With vsfUser
            .Rows.Count = 1
            .Cols.Count = 4
            .SetData(0, vUSRSlNo, "SlNo")
            .SetData(0, vUSRID, "UserID")
            .SetData(0, vUSRCode, "UserCode")
            .SetData(0, vUSRName, "User Name")

        End With
        SetColAlign(vsfUser, svtAlign.Right, vRptSlNo)
        SetColAlign(vsfUser, svtAlign.Left, vUSRID, vUSRCode, vUSRName)

        SetFlexColor(vsfUser, "", vUSRSlNo, vUSRID, vUSRCode, vUSRName)
        SetHideCol(vsfUser, True, vUSRID)

        With vsfRights
            .Rows.Count = 1
            .Cols.Count = 5
            .SetData(0, vRGTSlNo, "SlNo")
            .SetData(0, vRGTID, "ReportID")
            .SetData(0, vRGTRptGrpName, "Group Name")
            .SetData(0, vRGTRptName, "Report Name")

            .SetData(0, vRGTRights, "Rights")
        End With
        vsfRights.Cols(vRGTRights).DataType = GetType(Boolean)
        SetColAlign(vsfRights, svtAlign.Right, vRGTSlNo)
        SetColAlign(vsfRights, svtAlign.Left, vRGTID, vRGTRptName, vRGTRights)

        SetFlexColor(vsfRights, "", vRGTSlNo, vRGTID, vRGTRptName)
        SetFlexColor(vsfRights, "M", vRGTRights)
        SetHideCol(vsfRights, True, vRGTID)
        txtid.Focus()

    End Sub

    Private Sub FillReportGrid()
        Dim rs As oledb.oledbDataReader
        Dim strSQL As String

        strSQL = "select h.RptID,RptName,RptGroupName,RptFrmName"
        strSQL = strSQL & " from UtlReports h"
        If gUserCode <> "ADMIN" Then
            strSQL = strSQL & " ,utlreportsrights d"
            strSQL = strSQL & " where d.rptid = h.rptid "
            strSQL = strSQL & " and d.userid=" & gUserID
        End If
        strSQL = strSQL & " group by RptGroupName,RptName,h.RptID,RptFrmName"
        strSQL = strSQL & " Order by RptGroupName,Rptname"
        rs = clsSVT.getDataReaderFromSQL(strSQL)

        vsfRpt.Rows.Count = 1
        With rs
            Do While .Read()
                vsfRpt.Rows.Count += 1
                vsfRpt.SetData(vsfRpt.Rows.Count - 1, vRptSlNo, vsfRpt.Rows.Count - 1)
                vsfRpt.SetData(vsfRpt.Rows.Count - 1, vRptID, !rptid)
                vsfRpt.SetData(vsfRpt.Rows.Count - 1, vRptGrpName, !rptgroupname)
                vsfRpt.SetData(vsfRpt.Rows.Count - 1, vRptName, !rptname)
                vsfRpt.SetData(vsfRpt.Rows.Count - 1, vRptFrmName, !rptFrmname)
            Loop
            .Close()
        End With

        With vsfRpt
            .AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.RestrictRows
            .Cols(vRptGrpName).AllowMerging = True
            MakeColumnBold(vRptID, vsfRpt)
        End With

    End Sub

    Private Sub vsfRpt_BeforeEdit(sender As Object, e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfRpt.BeforeEdit
        e.Cancel = True
    End Sub

    Private Sub vsfRpt_DoubleClick(sender As Object, e As System.EventArgs) Handles vsfRpt.DoubleClick

        With vsfRpt
            If strFrmName = "" Then
                strFrmName = GetVal(.GetData(.Row, vRptFrmName), "")
            End If
            Select Case UCase(strFrmName)

                Case "RPTITEMQR"
                    Dim argfrm As New frmRptItemQR
                    'argfrm.disableall()
                    'argfrm.enableall()
                    argfrm.strRptName = "Item Quick Reference"
                    argfrm.StartPosition = FormStartPosition.CenterScreen
                    frmMain.OpenForms(argfrm, "Item Quick Reference", True)

                Case "RPTESTK"
                    Dim argfrm As New frmRptExciseStk
                    'argfrm.disableall()
                    'argfrm.enableall()
                    argfrm.strRptName = "Excise Stock Report"
                    argfrm.StartPosition = FormStartPosition.CenterScreen
                    frmMain.OpenForms(argfrm, "Excise Stock Report", True)

                Case "RPTPFTMGN"
                    Dim argfrm As New frmRptProMrg
                    'argfrm.disableall()
                    'argfrm.enableall()
                    argfrm.strRptName = "Profit Margin Report"
                    argfrm.StartPosition = FormStartPosition.CenterScreen
                    frmMain.OpenForms(argfrm, "Profit Margin Report", True)

                Case "RPTPKGSR"
                    Dim argfrm As New frmRptSalesPkgwise
                    argfrm.StartPosition = FormStartPosition.CenterScreen
                    argfrm.Text = "Package Wise Sales Report"
                    argfrm.ShowDialog()

                Case "RPTPKGPR"
                    Dim argfrm As New frmRptSPurPkgwise
                    argfrm.Text = "Package Wise Purchase Report"
                    argfrm.StartPosition = FormStartPosition.CenterScreen
                    argfrm.ShowDialog()

                Case "RPTHSR"
                    Dim argfrm As New frmRptHourlySales
                    'argfrm.disableall()
                    'argfrm.enableall()
                    argfrm.strRptName = "Hourly Sales Report"
                    argfrm.StartPosition = FormStartPosition.CenterScreen
                    frmMain.OpenForms(argfrm, "Hourly Sales Report", True)

                Case Else
                    MsgBox("Form : " & .GetData(.Row, vRptFrmName) & " is not found.", vbOKOnly)
            End Select
        End With
        txtid.Visible = True
        strFrmName = ""
    End Sub

    Private Sub btnRights_ClickButtonArea(Sender As System.Object, e As System.Windows.Forms.MouseEventArgs) Handles btnRights.ClickButtonArea
        Call FillUsers()
        If vsfUser.Rows.Count > 1 Then
            vsfUser.Row = 1
            vsfUser.RowSel = True
            lngUserID = Val(vsfUser.GetData(vsfUser.Row, vUSRID))
            Call FillRights(lngUserID)
        End If

        grpRights.Visible = True
        vsfRpt.Enabled = False
        btnRights.Enabled = False
        btnClose.Enabled = False
    End Sub

    Private Sub btnCancel_ClickButtonArea(Sender As System.Object, e As System.Windows.Forms.MouseEventArgs) Handles btnCancel.ClickButtonArea
        vsfRpt.Enabled = True
        btnRights.Enabled = True
        btnClose.Enabled = True
        grpRights.Visible = False
    End Sub

    Private Sub FillUsers()
        Dim rs As oledb.oledbDataReader
        Dim strSQL As String

        strSQL = "select UserID, UserCode, UserName"
        strSQL = strSQL & " from MstUser where UserName <> 'ADMIN'"
        strSQL = strSQL & " Order by UserCode, UserName"
        rs = clsSVT.getDataReaderFromSQL(strSQL)

        vsfUser.Rows.Count = 1
        With rs
            Do While .Read()
                vsfUser.Rows.Count += 1
                vsfUser.SetData(vsfUser.Rows.Count - 1, vUSRSlNo, vsfUser.Rows.Count - 1)
                vsfUser.SetData(vsfUser.Rows.Count - 1, vUSRID, !Userid)
                vsfUser.SetData(vsfUser.Rows.Count - 1, vUSRCode, !UserCode)
                vsfUser.SetData(vsfUser.Rows.Count - 1, vUSRName, !UserName)

            Loop
            .Close()
        End With
    End Sub

    Private Sub FillRights(argUserID As Long)
        Dim rs As oledb.oledbDataReader
        Dim strSQL As String

        strSQL = "select h.rptid,rptgroupname,h.rptname,u.userid  "
        strSQL = strSQL & " from utlreports h"
        strSQL = strSQL & " left join utlreportsrights d on h.rptid=d.rptid and d.userid = " & argUserID
        strSQL = strSQL & " left join mstuser u on d.userid=u.userid "
        strSQL = strSQL & " order by rptgroupname, rptname"

        rs = clsSVT.getDataReaderFromSQL(strSQL)

        vsfRights.Rows.Count = 1
        With rs
            Do While .Read()
                vsfRights.Rows.Count += 1
                vsfRights.SetData(vsfRights.Rows.Count - 1, vRGTSlNo, vsfRights.Rows.Count - 1)
                vsfRights.SetData(vsfRights.Rows.Count - 1, vRGTID, !rptid)
                vsfRights.SetData(vsfRights.Rows.Count - 1, vRGTRptGrpName, !rptgroupname)
                vsfRights.SetData(vsfRights.Rows.Count - 1, vRGTRptName, !rptname)
                If IsDBNull(!userid) Then
                    vsfRights.SetData(vsfRights.Rows.Count - 1, vRGTRights, False)
                Else
                    vsfRights.SetData(vsfRights.Rows.Count - 1, vRGTRights, True)
                End If
            Loop
            .Close()
        End With
    End Sub

    Private Sub vsfUser_AfterSelChange(sender As Object, e As C1.Win.C1FlexGrid.RangeEventArgs) Handles vsfUser.AfterSelChange
        With vsfUser
            If .Rows.Count > 1 Then
                '.Row = 1
                lngUserID = Val(.GetData(.Row, vUSRID))
                Call FillRights(lngUserID)
            End If
        End With
    End Sub

    'Private Sub btnSave_Click(sender As Object, e As System.EventArgs) Handles btnSave.Click
    '    Dim strSQL As String, iRow As Integer
    '    With vsfRights
    '        clsSVT.execSQLStatement("Delete from UtlReportsRights where userid =" & lngUserID)
    '        For iRow = 1 To .Rows.Count - 1
    '            If .GetData(iRow, vRGTRights) = True Then
    '                strSQL = "Insert Into UtlReportsRights (UserID,RptID) Values("
    '                strSQL = strSQL & lngUserID & ","
    '                strSQL = strSQL & Val(.GetData(iRow, vRGTID)) & ")"
    '                clsSVT.execSQLStatement(strSQL)
    '            End If
    '        Next iRow
    '    End With
    '    MsgBox("Data saved successfully!", vbOKOnly)
    'End Sub

    Private Sub vsfRights_BeforeEdit(sender As Object, e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfRights.BeforeEdit
        If e.Col <> vRGTRights Then
            e.Cancel = True
        End If
    End Sub

    Private Sub vsfUser_BeforeEdit(sender As Object, e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfUser.BeforeEdit
        e.Cancel = True
    End Sub

    Private Sub vsfRpt_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles vsfRpt.KeyDown
        If e.KeyCode = Keys.Enter Then
            Call vsfRpt_DoubleClick(sender, Nothing)
        End If
    End Sub

    Private Sub btnEnter_Click(sender As Object, e As EventArgs) Handles btnEnter.Click
        Dim strSQL As String

        strFrmName = ""
        strSQL = "select RptFrmName"
        strSQL = strSQL & " from UtlReports h"

        strSQL = strSQL & " where h.rptid =  '" & Trim(txtid.Text) & "'"
        Dim blnFound As Boolean
        strFrmName = clsSVT.getDataStringValue("select RptFrmName from UtlReports h where h.rptid =  '" & Trim(txtid.Text) & "'", "", blnFound)

        If ValidFlds() Then

            If txtid.Text <> "" Then
                'Dim strSQL As String
                Dim rs As oledb.oledbDataReader

                strSQL = "select h.RptID"
                strSQL = strSQL & " from UtlReports h"
                If gUserCode <> "ADMIN" Then
                    strSQL = strSQL & " ,utlreportsrights d"
                    strSQL = strSQL & " where d.rptid = h.rptid "
                    strSQL = strSQL & " and d.userid=" & gUserID
                End If
                strSQL = strSQL & " group by RptGroupName,RptName,h.RptID,RptFrmName"
                strSQL = strSQL & " Order by RptGroupName,Rptname"
                rs = clsSVT.getDataReaderFromSQL(strSQL)
                Do While rs.Read()
                    If rs!RptID = Val(txtid.Text) Then
                        vsfRpt_DoubleClick(sender, e)
                    End If
                Loop
                rs.Close()
            End If

            'With vsfRpt
            '    ' strFrmName = GetVal(.GetData(.Row, vRptFrmName), "")
            '    vsfRpt_DoubleClick(sender, e)
            '    'Select Case UCase(strno)
            '    '    Case "1"
            '    '        MsgBox("Your report name is " & strFrmName & " ")
            '    '    Case "2"
            '    '        MsgBox("Your report name is " & strFrmName & " ")
            '    '    Case "3"
            '    '        MsgBox("Your report name is " & strFrmName & " ")
            '    '    Case "4"
            '    '        MsgBox("Your report name is " & strFrmName & " ")
            '    '    Case "5"
            '    '        MsgBox("Your report name is " & strFrmName & " ")
            '    '    Case "6"
            '    '        MsgBox("Your report name is " & strFrmName & " ")
            '    '    Case "7"
            '    '        MsgBox("Your report name is " & strFrmName & " ")
            '    '    Case "8"
            '    '        MsgBox("Your report name is " & strFrmName & " ")


            '    '    Case Else
            '    '        MsgBox("Your report no is not found ")
            '    'End Select
            'End With
        End If
    End Sub

    Private Sub txtid_KeyUp(sender As Object, e As KeyEventArgs) Handles txtid.KeyUp
        If e.KeyCode = Keys.Enter Then
            If txtid.Text <> "" Then
                Dim strSQL As String
                Dim rs As oledb.oledbDataReader

                strSQL = "select h.RptID"
                strSQL = strSQL & " from UtlReports h"
                If gUserCode <> "ADMIN" Then
                    strSQL = strSQL & " ,utlreportsrights d"
                    strSQL = strSQL & " where d.rptid = h.rptid "
                    strSQL = strSQL & " and d.userid=" & gUserID
                End If
                strSQL = strSQL & " group by RptGroupName,RptName,h.RptID,RptFrmName"
                strSQL = strSQL & " Order by RptGroupName,Rptname"
                rs = clsSVT.getDataReaderFromSQL(strSQL)
                Do While rs.Read()
                    If rs!RptID = Val(txtid.Text) Then
                        btnEnter_Click(sender, e)
                    End If
                Loop
                rs.Close()
            End If
        End If
    End Sub

    Private Function ValidFlds() As Boolean
        Dim strRptno As Int16
        ValidFlds = False

        If Trim(txtid.Text) = "" Then
            MsgBox("report no  is Empty Please Enter the Report No.", MsgBoxStyle.Exclamation, Me.Text)
            txtid.Focus()
            Exit Function
        End If

        strRptno = clsSVT.getDataLongValue("select rptid from UtlReports h where h.rptid =  '" & Trim(txtid.Text) & "'", 0, False)
        If strRptno = 0 Then
            MsgBox("report no is not valid", MsgBoxStyle.Exclamation, Me.Text)
            txtid.Focus()
            Exit Function
        End If

        ValidFlds = True
    End Function

    Private Sub txtid_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtid.KeyPress
        If Not KeyValidate("Integer", e.KeyChar) Then
            e.Handled = True
        End If
    End Sub

    Private Sub frmRptReport_KeyUp(sender As Object, e As KeyEventArgs) Handles Me.KeyUp
        If e.KeyCode = Keys.F9 And e.Control = True Then
            Call initFormSize(Me, Me.Text, False, 577, 349, 0, 0)
            MsgBox("Restored Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
        If e.KeyCode = Keys.F8 And e.Control = True Then
            Call SaveActFormSettings(Me, Me.Text)
            Call SaveActFormVsfDet(Me, Me.Text, "Report", vsfRpt)
            MsgBox("Saved Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If

    End Sub

    Private Sub frmRptReport_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        txtid.Focus()
    End Sub

    'Private Sub btnClose_ClickButtonArea(Sender As Object, e As MouseEventArgs) Handles btnClose.Click
    '    frmMain.CloseSelectedTabPage(Me)
    'End Sub

    Private Sub btnClose_ClickButtonArea(Sender As System.Object, e As System.Windows.Forms.MouseEventArgs) Handles btnClose.ClickButtonArea
        frmMain.CloseSelectedTabPage(Me)
    End Sub

    Private Sub btnSave_ClickButtonArea(Sender As Object, e As MouseEventArgs) Handles btnSave.ClickButtonArea
        Dim strSQL As String, iRow As Integer
        With vsfRights
            clsSVT.execSQLStatement("Delete from UtlReportsRights where userid =" & lngUserID)
            For iRow = 1 To .Rows.Count - 1
                If .GetData(iRow, vRGTRights) = True Then
                    strSQL = "Insert Into UtlReportsRights (UserID,RptID) Values("
                    strSQL = strSQL & lngUserID & ","
                    strSQL = strSQL & Val(.GetData(iRow, vRGTID)) & ")"
                    clsSVT.execSQLStatement(strSQL)
                End If
            Next iRow
        End With
        MsgBox("Data saved successfully!", vbOKOnly)
    End Sub

    Private Sub vsfRpt_Click(sender As Object, e As EventArgs) Handles vsfRpt.Click

    End Sub
End Class

