Imports Microsoft.Office.Interop
Imports C1.Win.C1FlexGrid

Public Class frmRptsRegister
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
    Dim lngUserID As Long

    Private Sub btnClose_Click(sender As Object, e As System.EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub frmRptDayLtr_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        lngUserID = 0
        Call initScreen()
        Call FillReportGrid()
        If gUserCode = "ADMIN" Then
            btnRights.Enabled = True
        Else
            btnRights.Enabled = False
        End If
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
    End Sub

    Private Sub FillReportGrid()
        Dim rs As oledb.oledbDataReader
        Dim strSQL As String
        'Dim blnFound As Boolean, intRow As Integer
        'Dim cs As CellStyle = vsfHdr.Styles.Add("flexcpBackColor")
        'cs.BackColor = Color.Lime
        'cs.Font = New Font(Font.Name, Font.Size, FontStyle.Bold)

        strSQL = "select h.RptID,RptName,RptGroupName,RptFrmName"
        strSQL = strSQL & " from UtlReports h"
        If gUserCode <> "ADMIN" Then
            strSQL = strSQL & " ,utlreportsrights d"
            strSQL = strSQL & " where d.rptid = h.rptid "
            strSQL = strSQL & " and d.userid=" & gUserID
        End If
        strSQL = strSQL & " group by RptGroupName,RptName,h.RptID,RptFrmName"
        strSQL = strSQL & " Order by RptGroupName,RptName"
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
        Dim strFrmName As String
        strFrmName = ""
        'With vsfRpt
        '    strFrmName = GetVal(.GetData(.Row, vRptFrmName), "")
        '    Select Case UCase(strFrmName)
        '        Case "FRMRPTIGRNVAL"
        '            With frmRptIGRNVal
        '                .MdiParent = frmMain
        '                .Text = vsfRpt.GetData(vsfRpt.Row, vRptName)
        '                .Show()
        '            End With
        '        Case "FRMRPTPINV"
        '            With frmRptPInv
        '                .MdiParent = frmMain
        '                .Text = vsfRpt.GetData(vsfRpt.Row, vRptName)
        '                .Show()
        '            End With
        '        Case "FRMRPTSINV"
        '            With frmRptSInv
        '                .MdiParent = frmMain
        '                .Text = vsfRpt.GetData(vsfRpt.Row, vRptName)
        '                .Show()
        '            End With
        '        Case "FRMRPTGRN"
        '            With frmRptGRN
        '                .MdiParent = frmMain
        '                .Text = vsfRpt.GetData(vsfRpt.Row, vRptName)
        '                .Show()
        '            End With
        '        Case "FRMRPTJGRN"
        '            With frmRptJGRN
        '                .MdiParent = frmMain
        '                .Text = vsfRpt.GetData(vsfRpt.Row, vRptName)
        '                .Show()
        '            End With
        '        Case "FRMRPTLGRNVAL"
        '            With frmRptLGRNVal
        '                .MdiParent = frmMain
        '                .Text = vsfRpt.GetData(vsfRpt.Row, vRptName)
        '                .Show()
        '            End With
        '        Case "FRMRPTJDC"
        '            With frmRptJDC
        '                .MdiParent = frmMain
        '                .Text = vsfRpt.GetData(vsfRpt.Row, vRptName)
        '                .Show()
        '            End With
        '        Case "FRMRPTDCRETURN"
        '            With frmRptDCReturn
        '                .MdiParent = frmMain
        '                .Text = vsfRpt.GetData(vsfRpt.Row, vRptName)
        '                .Show()
        '            End With

        '        Case Else
        '            MsgBox("Form : " & .GetData(.Row, vRptFrmName) & " is not found.", vbOKOnly)
        '    End Select
        'End With
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
        'Dim blnFound As Boolean, intRow As Integer
        'Dim cs As CellStyle = vsfHdr.Styles.Add("flexcpBackColor")
        'cs.BackColor = Color.Lime
        'cs.Font = New Font(Font.Name, Font.Size, FontStyle.Bold)

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
        'Dim blnFound As Boolean, intRow As Integer
        'Dim cs As CellStyle = vsfHdr.Styles.Add("flexcpBackColor")
        'cs.BackColor = Color.Lime
        'cs.Font = New Font(Font.Name, Font.Size, FontStyle.Bold)

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

    Private Sub btnSave_Click(sender As Object, e As System.EventArgs) Handles btnSave.Click
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
End Class