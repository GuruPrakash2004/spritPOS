Imports C1.Win.C1FlexGrid

Public Class frmUtlAssignRight

    Dim clsSVT As New svtLibrary
    Dim strSQL As String
    Const vDetCRptGrp = 0
    Const vDetCRptId = 1

    Const vDetCRptName = 2
    Const vDetCRptAllow = 3
    Const vDetTotcols = 4


    Private Sub initscreen()
        With vsfDet
            .Cols.Count = vDetTotcols
            .Cols.Fixed = 0
            .Rows.Count = 2

            .SetData(0, vDetCRptGrp, "Report Group")
            .SetData(0, vDetCRptId, "Report Group")
            .SetData(0, vDetCRptName, "Report Name")
            .SetData(0, vDetCRptAllow, "Allow")

            .Cols(vDetCRptAllow).DataType = GetType(Boolean)
            SetFlexColor(vsfDet, "", vDetCRptGrp, vDetCRptName)
            SetHideCol(vsfDet, True, vDetCRptId)
        End With
    End Sub
    Private Sub fillDocDetails()
        Dim strSql As String = ""
        Dim i As Integer
        Dim rs As oledb.oledbDataReader = Nothing

        strSql = " select ReportGroup,CRID,CRName"
        strSql &= " from UtlReportsCRHdr"
        strSql &= " Order By ReportGroup"
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

                .SetData(i, vDetCRptGrp, GetVal(rs("ReportGroup"), ""))
                .SetData(i, vDetCRptId, GetVal(rs("CRID"), 0))
                .SetData(i, vDetCRptName, GetVal(rs("CRName"), ""))

                If rs("CRID") = clsSVT.getDataLongValue("select Crid from  UtlReportsCRRights where Userid=" & txtDummy.Text & "  And CRid = " & rs("CRID") & "  ", 0, False) Then


                    .SetCellCheck(i, vDetCRptAllow, CheckEnum.Checked)

                End If
            Loop
            rs.Close()
        End With
        With vsfDet

            .AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.RestrictAll
            .Cols(vDetCRptGrp).AllowMerging = True

        End With

    End Sub


    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Dim strSql As String

        If txtUser.Text = "" Then
            MsgBox("Please select a user", vbInformation, Me.Text)
            Exit Sub
        End If

        Try
            clsSVT.execSQLStatement("Delete from UtlReportsCRRights where Userid = " & Val(txtDummy.Text))
            With vsfDet
                For i = 1 To .Rows.Count - 1
                    If .GetCellCheck(i, vDetCRptAllow) = C1.Win.C1FlexGrid.CheckEnum.Checked Then
                        strSql = "insert into UtlReportsCRRights (CRID, UserID) "
                        strSql = strSql & " values ("
                        strSql = strSql & GetVal(.GetData(i, vDetCRptId), 0) & ","
                        strSql = strSql & Val(txtDummy.Text)
                        strSql = strSql & ")"
                        Call clsSVT.execSQLStatement(strSql)
                    End If
                    Next i
            End With
            MsgBox("Report rights assigned successfully!", vbInformation, Me.Text)
        Catch ex As Exception
            MsgBox("Error While assigning rights!", vbCritical, Me.Text)
        End Try
    End Sub

    Private Function vDetUserID() As Integer
        Throw New NotImplementedException()
    End Function

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        'frmMain.CloseSelectedTabPage(Me)
        Me.Close()
    End Sub

    Private Sub chkAll_CheckedChanged(sender As Object, e As EventArgs) Handles chkAll.CheckedChanged
        With vsfDet
            If .Rows.Count <= 0 Then Exit Sub
            If chkAll.Checked = True Then
                For i As Int32 = 1 To .Rows.Count - 1
                    .SetData(i, vDetCRptAllow, True)
                Next
            Else
                For i As Int32 = 1 To .Rows.Count - 1
                    .SetData(i, vDetCRptAllow, False)
                Next
            End If
        End With
    End Sub

    Private Sub btnSelUser_Click(sender As Object, e As EventArgs) Handles btnSelUser.Click
        Dim StrSql As String
        StrSql = "Select Userid ,UserCode,Designation  "
        StrSql &= " from MstUser "
        StrSql &= " where Authorised ='Y'"

            Dim frmobj As New frmUtlSingleSel
        With frmobj
            .Text = "Select User"
            .FillViewVsf(Me, StrSql, txtDummy, 0, 0)
            .StartPosition = FormStartPosition.CenterScreen
            .ShowDialog()
        End With
        If Val(txtDummy.Text) <> 0 Then
            txtUser.Text = clsSVT.getDataStringValue("select Usercode from MstUser  where userid=" & txtDummy.Text & " ", "", False)
        End If

    End Sub

    Private Sub vsfDet_BeforeEdit(sender As Object, e As RowColEventArgs) Handles vsfDet.BeforeEdit
        Select Case e.Col
            Case vDetCRptGrp, vDetCRptName
                e.Cancel = True
        End Select
    End Sub

    Private Sub txtUser_TextChanged(sender As Object, e As EventArgs) Handles txtUser.TextChanged
        If Not Trim(txtUser.Text) = "" Then
            Call fillDocDetails()
        End If
    End Sub

    Private Sub frmUtlAssignRight_Load(sender As Object, e As EventArgs) Handles Me.Load
        Me.KeyPreview = True
        fillFormProperties(Me)
        initGridStyles(Me)
        Call initFormSize(Me, Me.Text, False, 642, 469, 0, 0)
        Call initFormVsfDet(Me, Me.Text, vsfDet, "DET")
        initscreen()
    End Sub

    Private Sub frmUtlAssignRight_KeyUp(sender As Object, e As KeyEventArgs) Handles Me.KeyUp

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
End Class