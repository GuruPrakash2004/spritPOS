Public Class frmUtlAssignRights
    Const vDetSel = 0
    Const vDetUserName = 1
    Const vDetUserID = 2

    Dim clsSVT As New svtLibrary
    Public intCRID As Integer = 0
    Public strRptName As String = ""

    Private Sub frmUtlAssignRights_FormClosing(sender As Object, e As System.Windows.Forms.FormClosingEventArgs)
        frmModule.SaveAllSettings(Application.ProductName, Me)
    End Sub

    Private Sub frmUtlInvNumberFmt_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Call initScreen()
        'UtlReportsCRRights
        frmModule.LoadAllSettings(Application.ProductName, Me)
    End Sub

    Private Sub initScreen()
        initGridStyles(Me)

        With vsfDet
            .Cols.Count = 3
            .Rows.Count = 2

            .SetData(0, vDetSel, "Sel")
            .SetData(0, vDetUserName, "User")
            .SetData(0, vDetUserID, "UserID")

            .Cols(vDetSel).DataType = GetType(Boolean)

            Dim cs = .Styles.Add("Boolean")
            cs.DataType = GetType(Boolean)
            cs.ImageAlign = C1.Win.C1FlexGrid.ImageAlignEnum.CenterCenter
            vsfDet.SetCellStyle(0, vDetSel, cs)
            MakeRowBold(0, vsfDet)
            .Rows(0).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter
            .Cols(vDetUserName).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter
            SetHideCol(vsfDet, True, vDetUserID)
        End With

        fillDocDetails(intCRID)
        Me.Text &= " - " & strRptName
    End Sub

    Private Sub fillDocDetails(argCRID As Integer)
        Dim strSql As String = ""
        Dim rs As oledb.oledbDataReader = Nothing
        Dim i%

        'strSql = "Select UserCode, UserID from MstUser where UserID > 1"
        'rs = clsSVT.getDataReaderFromSQL(strSql)

        strSql = " select u.* from MstUser u,MstUserRights ur, MstMenu m"
        strSql &= " where m.menuitemID = 492"
        strSql &= " and ur.userid = u.userid"
        strSql &= " and m.MenuItemID = ur.MenuItemID "
        '  strSql &= " order by u.UserCode"

        rs = clsSVT.getDataReaderFromSQL(strSql)

        With vsfDet
            .Rows.Count = 1

            Do While rs.Read()
                If Not rs.HasRows Then
                    vsfDet.Rows.Count = 1
                    Exit Sub
                End If
                i = vsfDet.Rows.Count
                vsfDet.Rows.Count += 1
                .SetData(i, vDetUserName, rs("UserCode"))
                .SetData(i, vDetUserID, rs("UserID"))
            Loop
            rs.Close()
        End With

        strSql = "Select UserCode, r.UserID, CRID from MstUser u, UtlReportsCRRights r"
        strSql &= " where(r.UserID > 1)"
        strSql &= " and r.UserID = u.UserID"
        strSql &= " and CRID = " & intCRID

        rs = clsSVT.getDataReaderFromSQL(strSql)

        With vsfDet
            Do While rs.Read()
                For i = 1 To .Rows.Count - 1
                    If Val(.GetData(i, vDetUserID)) = Val(rs("UserID")) Then
                        .SetCellCheck(i, vDetSel, C1.Win.C1FlexGrid.CheckEnum.Checked)
                    End If
                Next
            Loop
            rs.Close()
        End With

    End Sub

    Private Sub btnClose_ClickButtonArea(Sender As System.Object, e As System.Windows.Forms.MouseEventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub vsfDet_BeforeEdit(sender As Object, e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfDet.BeforeEdit
        'If e.Col <> vDetSel Then
        '    e.Cancel = True
        '    Exit Sub
        'End If
        'If e.Row <> 0 Then Exit Sub
        'If vsfDet.Rows.Count = 1 Then
        '    e.Cancel = True
        '    Exit Sub
        'End If



        'With vsfDet
        '    If .GetCellCheck(0, vDetSel) = C1.Win.C1FlexGrid.CheckEnum.Checked Then
        '        For i = 1 To .Rows.Count - 1
        '            .SetCellCheck(i, vDetSel, C1.Win.C1FlexGrid.CheckEnum.Checked)
        '        Next
        '    Else
        '        For i = 1 To .Rows.Count - 1
        '            .SetCellCheck(i, vDetSel, C1.Win.C1FlexGrid.CheckEnum.Unchecked)
        '        Next
        '    End If

        'If e.Row > 0 Then
        '    If .GetCellCheck(e.Row, vDetSel) = C1.Win.C1FlexGrid.CheckEnum.Checked Then
        '        MakeRowBold(e.Row, vsfDet)
        '    Else
        '        MakeRowBold(e.Row, vsfDet, , "Regular")
        '    End If
        'End If
        'End With


    End Sub

    Private Sub btnSave_Click(sender As Object, e As System.EventArgs) Handles btnSave.Click
        Dim strSql As String

        Try
            clsSVT.execSQLStatement("Delete from UtlReportsCRRights where CRID = " & intCRID)
            With vsfDet
                For i = 1 To .Rows.Count - 1
                    If .GetCellCheck(i, vDetSel) = C1.Win.C1FlexGrid.CheckEnum.Checked Then
                        strSql = "insert into UtlReportsCRRights (CRID, UserID)"
                        strSql = strSql & " values ("
                        strSql = strSql & intCRID & ","
                        strSql = strSql & .GetData(i, vDetUserID) & ")"

                        Call clsSVT.execSQLStatement(strSql)
                    End If
                Next i
            End With
            MsgBox("Report rights assigned successfully!", vbInformation, Me.Text)
        Catch ex As Exception
            MsgBox("Error while assigning rights!", vbCritical, Me.Text)
        End Try

    End Sub

    Private Sub vsfDet_MouseClick(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles vsfDet.MouseClick
        Dim ht = vsfDet.HitTest
        If ht.Row < 0 Then Exit Sub

        If ht.Row = 0 And ht.Column = vDetSel Then
            If vsfDet.GetCellCheck(0, vDetSel) = C1.Win.C1FlexGrid.CheckEnum.Checked Then
                For i = 1 To vsfDet.Rows.Count - 1
                    vsfDet.SetCellCheck(i, vDetSel, C1.Win.C1FlexGrid.CheckEnum.Checked)
                Next
            Else
                For i = 1 To vsfDet.Rows.Count - 1
                    vsfDet.SetCellCheck(i, vDetSel, C1.Win.C1FlexGrid.CheckEnum.Unchecked)
                Next
            End If

        End If

    End Sub
End Class