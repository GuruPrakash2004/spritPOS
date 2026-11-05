Imports System.ComponentModel
Imports System.Data.oledb
Imports C1.Win.C1FlexGrid

Public Class frmUtlBulkUpdate

    Dim clsSVT As New svtLibrary
    Dim mode As String
    Dim strTable As String
    Dim intTotNoOfRec As String
    Dim strDataType As Type

    Dim dt As DataTable
    Dim dv As DataView
    Dim tooltip As New ToolTip


    Private Sub frmUtlBulkUpdate_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim strSQL As String
        Call initScreen()
        strSQL = "Select Name from Sys.Tables where (upper(Name) like'MST%' or upper(Name) like'TRN%') "
        strSQL = strSQL & " and upper(Name) not in ('MSTMENU','MSTCOMP','MSTCOMPFINYEAR','MSTROLE','MSTROLERIGHTS','MSTSTATE',"
        strSQL = strSQL & " 'MSTUDRRIGHTS''MSTUSER','MSTUSERRIGHTS','TRNQOH''TRNSTKFLOW','TRNSTOCK') "
        strSQL = strSQL & " order by Name "
        Call bindCombo(cboTable, "Name", "Name", strSQL)
        cboTable.Text = ""
        cboTable.Focus()
        tooltip.SetToolTip(Me.btnFreeze, "Freeze : Alt + F")
        'Call SetHideCol(vsfDet, True, 3, 4, 5, 6)
    End Sub

    Private Sub initScreen()
        With vsfDet
            .Rows.Count = 1
        End With
        Me.KeyPreview = True
        Call initFormVsfDet(Me, Me.Text, vsfDet, "DET")
        cboTable.SelectedIndex = -1
        initGridStyles(Me)
        fillFormProperties(Me)
        txtNoofRec.Text = ""
        GrpUpdate.Visible = False
        pnlExp.Visible = False
        GrpDelete.Visible = False
    End Sub

    Private Sub btnShowRec_Click(sender As Object, e As EventArgs) Handles btnShowRec.Click
        If cboTable.Text = "" Then Exit Sub
        Dim i As Integer, blnFound As Boolean
        blnFound = False
        For i = 1 To cboTable.Items.Count - 1
            If UCase(cboTable.GetItemText(cboTable.Items(i))) = UCase(cboTable.Text) Then
                blnFound = True
                Exit For
            End If
        Next
        If Not blnFound Then
            MsgBox("Please select a table from the drop down!", MsgBoxStyle.Critical, "ERROR")
            Exit Sub
        End If
        Call fillData()
        txtReplaceValue.Text = ""
        txtCurValue.Text = ""
        txtFields.Text = ""

    End Sub
    Public Sub fillData()

        Dim strSQL As String


        strSQL = " select top 1000 * from " & cboTable.Text & " where 1=1"



        Call showWaitCursor()
        With vsfDet
            .BeginUpdate()

            clsSVT.setGridDataBind(strSQL, vsfDet)
            .Cols(0).Visible = False
            .Refresh()
            .Visible = True
            Call showNormalCursor()
            .EndUpdate()
        End With
        Call SetFlexRowColor(vsfDet, "", 0)
        ' Call SetHideCol(vsfDet, True, 3, 4, 5, 6)

        intTotNoOfRec = Val(vsfDet.Rows.Count - 1)
        txtNoofRec.Text = intTotNoOfRec
        If vsfDet.Rows.Count > 1 Then
            GrpUpdate.Visible = True
            pnlExp.Visible = True
            GrpDelete.Visible = True
            btnUpdate.Visible = False
        Else
            GrpUpdate.Visible = False
            pnlExp.Visible = False
            GrpDelete.Visible = False
            btnUpdate.Visible = False
        End If
        If UCase(Trim(cboTable.Text)) = "MSTCUST" Or UCase(Trim(cboTable.Text)) = "MSTSUPP" Then
            For i = 1 To vsfDet.Cols.Count - 1
                If UCase(vsfDet.GetData(0, i)) = "REMARKS" Then
                    vsfDet.SetData(0, i, "Party Notes")
                    Exit For
                End If
            Next
        End If
        If UCase(Trim(cboTable.Text)) = "TRNDTLBR" Then
            Call SetHideCol(vsfDet, True, 3, 4, 5, 6)
        End If
    End Sub

    Private Sub vsfDet_BeforeEdit(sender As Object, e As RowColEventArgs) Handles vsfDet.BeforeEdit
        e.Cancel = True
    End Sub

    Private Sub cboTable_Validating(sender As Object, e As CancelEventArgs) Handles cboTable.Validating
        If cboTable.Text = "" Then Exit Sub
        Dim blnFound As Boolean
        clsSVT.getDataStringValue("Select Name from Sys.Tables where Name='" & Trim(cboTable.Text) & "'", "", blnFound)

        If Not blnFound Then
            MsgBox("Table Not found in the list!!", MsgBoxStyle.Information, Me.Text)
            cboTable.Focus()
            Exit Sub
        End If
    End Sub

    Private Sub vsfDet_AfterFilter(sender As Object, e As EventArgs) Handles vsfDet.AfterFilter
        Dim i As Integer, intno As Integer = 0

        For i = 1 To vsfDet.Rows.Count - 1
            If vsfDet.Rows(i).Visible = True Then
                intno = intno + 1
            End If
        Next
        txtNoofRec.Text = intno & " / " & intTotNoOfRec
    End Sub


    Private Sub vsfDet_DoubleClick(sender As Object, e As EventArgs) Handles vsfDet.DoubleClick
        Dim intLength As Integer = 0

        With vsfDet
            If .Col = 0 Or .Col = 1 Then
                MsgBox(Trim(.GetData(0, .Col)) & " Coloumn cannot be editable", MsgBoxStyle.Information, Me.Text)
                Exit Sub
            End If


            txtFields.Text = vsfDet.GetData(0, vsfDet.Col)

            If RdoComplete.Checked = True Then
                txtCurValue.Text = vsfDet.GetData(vsfDet.Row, vsfDet.Col).ToString
            Else
                txtPartial.Text = vsfDet.GetData(vsfDet.Row, vsfDet.Col).ToString
            End If

            strDataType = vsfDet.Cols(vsfDet.Col).DataType
            Select Case strDataType
                Case GetType(Integer), GetType(Decimal), GetType(Double), GetType(String)
                    txtReplaceValue.Visible = True
                    dtpDate.Visible = False
                Case GetType(DateTime)
                    txtReplaceValue.Visible = False
                    dtpDate.Visible = True
            End Select

        End With

        txtReplaceValue.Text = ""
        If txtReplaceValue.Visible = True Then
            txtReplaceValue.Focus()
        Else
            dtpDate.Focus()
        End If
        If txtFields.Text <> "" Then
            btnUpdate.Visible = True
        Else
            btnUpdate.Visible = False
        End If
    End Sub

    Private Sub PartialUpdate()
        Dim svtTrn As oledb.oledbTransaction
        Dim strSql As String = ""
        Dim intlength As Integer
        Dim i As Integer, intCount As Integer
        Dim blnfound As Boolean

        intCount = 0
        If txtReplaceValue.Text = "" AndAlso txtFields.Text = "" Then Exit Sub
        If txtPartial.Text = "" Then Exit Sub

        svtTrn = svtConn.BeginTransaction(IsolationLevel.ReadCommitted)
        Try
            intlength = clsSVT.getDataLongValue("select character_Maximum_Length from INFORMATION_SCHEMA.COLUMNS where table_Name='" & cboTable.Text & "'And Column_Name = '" & Trim(txtFields.Text) & " '", 0, False, svtTrn)
            For i = 1 To vsfDet.Rows.Count - 1

                If vsfDet.Rows(i).Visible = True Then

                    Select Case strDataType
                        Case GetType(Integer), GetType(Decimal), GetType(Double)

                        Case GetType(DateTime)

                        Case Else

                            clsSVT.getDataLongValue("select " & vsfDet.GetData(0, 0) & " from " & cboTable.Text & " where " & txtFields.Text & " LIKE '%" & txtPartial.Text & "%' And " & vsfDet.GetData(0, 0) & "=" & vsfDet.GetData(i, 0), 0, blnfound, svtTrn)

                            If Not blnfound Then
                                GoTo nextloop
                            End If

                            If Len(txtReplaceValue.Text) > intlength Then
                                MsgBox("Lenght Of replace value should Not be more than " & intlength, MsgBoxStyle.Information, Me.Text)
                                txtReplaceValue.Focus()
                                svtTrn.Rollback()
                                Exit Sub
                            End If

                            If txtPartial.Text <> "" Then

                                strSql = " Update " & cboTable.Text
                                strSql &= " Set " & Trim(txtFields.Text) & " = Replace( " & Trim(txtFields.Text) & ",'" & txtPartial.Text & "','" & txtReplaceValue.Text & "') "
                                strSql &= " where  " & Trim(txtFields.Text) & " Like '%" & Trim(txtPartial.Text) & "%' and " & vsfDet.GetData(0, 0) & " = " & vsfDet.GetData(i, 0)

                            Else

                                strSql = "Update " & cboTable.Text
                                strSql &= " Set " & Trim(txtFields.Text) & " = Replace( " & Trim(txtFields.Text) & ",'" & txtPartial.Text & "','" & txtReplaceValue.Text & "') "
                                strSql &= " where " & vsfDet.GetData(0, 0) & " = " & vsfDet.GetData(i, 0) & " And  (" & txtFields.Text & " ='" & Trim(txtPartial.Text) & "') Or   (" & Trim(txtFields.Text) & " is Null)"
                            End If
                    End Select
                End If
                If strSql <> "" Then
                    intCount = intCount + 1
                    If Not clsSVT.execSQLStatement(strSql, svtTrn) Then
                        svtTrn.Rollback()
                        Exit Sub
                    End If
                End If
                strSql = ""
nextloop:
            Next i

            MsgBox(intCount & " Rows updated Successfully", MsgBoxStyle.Information, Me.Text)
            svtTrn.Commit()
            Call fillData()

        Catch ex As Exception
            MsgBox("Error during Update" & vbCrLf & ex.Message, MsgBoxStyle.Critical, Me.Text)
            svtTrn.Rollback()
        End Try
    End Sub

    Private Sub CompleteUpdate()
        Dim svtTrn As oledb.oledbTransaction
        Dim strSql As String = ""
        Dim intlength As Integer
        Dim i As Integer, intCount As Integer
        Dim blnfound As Boolean

        intCount = 0
        If txtReplaceValue.Text = "" AndAlso txtFields.Text = "" Then Exit Sub
        svtTrn = svtConn.BeginTransaction(IsolationLevel.ReadCommitted)
        Try
            intlength = clsSVT.getDataLongValue("select character_Maximum_Length from INFORMATION_SCHEMA.COLUMNS where table_Name='" & cboTable.Text & "'And Column_Name = '" & Trim(txtFields.Text) & " '", 0, False, svtTrn)
            For i = 1 To vsfDet.Rows.Count - 1

                If vsfDet.Rows(i).Visible = True Then

                    Select Case strDataType
                        Case GetType(Integer), GetType(Decimal), GetType(Double)

                            clsSVT.getDataLongValue("select " & vsfDet.GetData(0, 0) & " from " & cboTable.Text & " where " & txtFields.Text & "=" & txtCurValue.Text & " And " & vsfDet.GetData(0, 0) & "=" & vsfDet.GetData(i, 0), 0, blnfound, svtTrn)
                            If Not blnfound Then
                                GoTo nextloop
                            End If

                            strSql = "Update " & cboTable.Text & " set " & txtFields.Text & " = " & Val(Trim(Replace(Replace(txtReplaceValue.Text, vbCrLf, ""), vbCr, ""))) & " where " & txtFields.Text & " =" & Trim(txtCurValue.Text) & " and " & vsfDet.GetData(0, 0) & " = " & vsfDet.GetData(i, 0)

                        Case GetType(DateTime)

                            If Trim(txtCurValue.Text) = "" Then
                                clsSVT.getDataLongValue("select " & vsfDet.GetData(0, 0) & " from " & cboTable.Text & " where " & txtFields.Text & "='" & (txtCurValue.Text) & "'Or   (" & Trim(txtFields.Text) & " is Null) And " & vsfDet.GetData(0, 0) & "=" & vsfDet.GetData(i, 0), 0, blnfound, svtTrn)
                                If Not blnfound Then
                                    GoTo nextloop
                                End If
                                strSql = "Update " & cboTable.Text & " Set " & txtFields.Text & " = " & getDateFormatInCond(dtpDate.Value) & "" & " where " & Trim(txtFields.Text) & " ='" & (txtCurValue.Text) & "'Or   (" & Trim(txtFields.Text) & " is Null)  and " & vsfDet.GetData(0, 0) & " = " & vsfDet.GetData(i, 0)

                            Else
                                clsSVT.getDataLongValue("select " & vsfDet.GetData(0, 0) & " from " & cboTable.Text & " where " & txtFields.Text & "=" & getDateFormatInCond(CDate(txtCurValue.Text)) & " And " & vsfDet.GetData(0, 0) & "=" & vsfDet.GetData(i, 0), 0, blnfound, svtTrn)
                                If Not blnfound Then
                                    GoTo nextloop
                                End If
                                strSql = "Update " & cboTable.Text & " Set " & txtFields.Text & " = " & getDateFormatInCond(dtpDate.Value) & "" & " where " & Trim(txtFields.Text) & " =" & getDateFormatInCond(CDate(txtCurValue.Text)) & " And " & vsfDet.GetData(0, 0) & "=" & vsfDet.GetData(i, 0)
                            End If

                        Case Else
                            clsSVT.getDataLongValue("select " & vsfDet.GetData(0, 0) & " from " & cboTable.Text & " where " & txtFields.Text & "='" & txtCurValue.Text & "' And " & vsfDet.GetData(0, 0) & "=" & vsfDet.GetData(i, 0), 0, blnfound, svtTrn)
                            If Not blnfound Then
                                GoTo nextloop
                            End If

                            If Len(txtReplaceValue.Text) > intlength Then
                                MsgBox("Lenght Of replace value should Not be more than " & intlength, MsgBoxStyle.Information, Me.Text)
                                txtReplaceValue.Focus()
                                svtTrn.Rollback()
                                Exit Sub
                            End If
                            If txtCurValue.Text = "" Then
                                strSql = "Update " & cboTable.Text & " Set " & Trim(txtFields.Text) & " = '" & Trim(Replace(Replace(txtReplaceValue.Text, vbCrLf, ""), vbCr, "")) & "' where " & vsfDet.GetData(0, 0) & " = " & vsfDet.GetData(i, 0) & " And  (" & txtFields.Text & " ='" & Trim(txtCurValue.Text) & "') Or   (" & Trim(txtFields.Text) & " is Null)"
                            Else
                                strSql = "Update " & cboTable.Text & " set " & Trim(txtFields.Text) & " = '" & Trim(Replace(Replace(txtReplaceValue.Text, vbCrLf, ""), vbCr, "")) & "' where " & Trim(txtFields.Text) & " ='" & Trim(txtCurValue.Text) & "' and " & vsfDet.GetData(0, 0) & " = " & vsfDet.GetData(i, 0)
                            End If
                    End Select
                End If
                If strSql <> "" Then
                    intCount = intCount + 1
                    If Not clsSVT.execSQLStatement(strSql, svtTrn) Then
                        svtTrn.Rollback()
                        Exit Sub
                    End If
                End If
                strSql = ""
nextloop:
            Next i

            MsgBox(intCount & " Rows updated Successfully", MsgBoxStyle.Information, Me.Text)
            svtTrn.Commit()
            Call fillData()




        Catch ex As Exception
            MsgBox("Error during Update" & vbCrLf & ex.Message, MsgBoxStyle.Critical, Me.Text)
            svtTrn.Rollback()
        End Try
    End Sub

    Private Sub btnUpdate_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click
        If RdoComplete.Checked = True Then
            Call CompleteUpdate()
        Else
            Call PartialUpdate()

        End If
    End Sub

    Private Sub frmUtlBulkUpdate_KeyUp(sender As Object, e As KeyEventArgs) Handles Me.KeyUp
        If e.KeyCode = Keys.F9 And e.Control = True Then
            Call initFormSize(Me, Me.Text, True, 1100, 682, 0, 0)
            MsgBox("Restored Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
        If e.KeyCode = Keys.F8 And e.Control = True Then
            Call SaveActFormSettings(Me, Me.Text)
            Call SaveActFormVsfDet(Me, Me.Text, "DET", vsfDet)
            MsgBox("Saved Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If

        If e.Alt = True And e.KeyCode = Keys.E Then
            Call ExportToXL(vsfDet)
        End If

    End Sub

    Private Sub txtReplaceValue_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtReplaceValue.KeyPress
        If strDataType = GetType(Integer) Or strDataType = GetType(Decimal) Or strDataType = GetType(Double) Then
            If KeyValidate("Decimal", e.KeyChar) Then
                e.Handled = False
            Else
                e.Handled = True
            End If
        End If
        If e.KeyChar = "'" Then
            e.Handled = True
        End If

    End Sub

    Private Sub txtPartial_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtPartial.KeyPress

        If strDataType = GetType(Integer) Or strDataType = GetType(Decimal) Or strDataType = GetType(Double) Then
            If KeyValidate("Decimal", e.KeyChar) Then
                e.Handled = False
            Else
                e.Handled = True
            End If
        End If
        If e.KeyChar = "'" Then
            e.Handled = True
        End If

    End Sub


    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click

        If Trim(cboTable.Text) = "" Then Exit Sub
        For i = 1 To vsfDet.Rows.Count - 1
            If vsfDet.Rows(i).Visible = True Then
                If Not blnDeleteOK(cboTable.Text, Val(vsfDet.GetData(i, 0)), True, True,) Then
                    Exit Sub
                End If
            End If
        Next i
        If MsgBox("Are you sure you want to delete the Record from ? " &
            cboTable.Text, vbCritical + MsgBoxStyle.YesNo + vbDefaultButton2, "Delete Confirmation") = MsgBoxResult.Yes Then
            If MsgBox("You can't undo this Delete operation ! " &
            "Continue?", vbCritical + MsgBoxStyle.YesNo + vbDefaultButton2, "Delete Confirmation") = MsgBoxResult.Yes Then
                Dim svtTrn As oledb.oledbTransaction = Nothing
                Try
                    svtTrn = svtConn.BeginTransaction(IsolationLevel.ReadCommitted)
                    For i = 1 To vsfDet.Rows.Count - 1
                        If vsfDet.Rows(i).Visible = True Then
                            If Not clsSVT.execSQLStatement("Delete from " & cboTable.Text & " where " & vsfDet.GetData(0, 0) & " = " & vsfDet.GetData(i, 0), svtTrn) Then
                                svtTrn.Rollback()
                                Exit Sub
                            End If
                        End If
                    Next i
                    MsgBox("Deleted Successfully!", MsgBoxStyle.Information, Me.Text)

                    svtTrn.Commit()
                    Call fillData()
                Catch ex As Exception
                    svtTrn.Rollback()
                    Exit Sub
                End Try
            Else
                Exit Sub

            End If
        End If


    End Sub

    Private Sub btnExcel_Click(sender As Object, e As EventArgs) Handles btnExcel.Click
        Call ExportToXL(vsfDet)
    End Sub

    Private Sub btnFreeze_Click(sender As Object, e As EventArgs) Handles btnFreeze.Click
        If btnFreeze.Text = "&Freeze" Then
            btnFreeze.Text = "&UnFreeze"
            ToolTip.SetToolTip(Me.btnFreeze, "UnFreeze : Alt + U")
            vsfDet.Cols.Frozen = vsfDet.Col + 1
            vsfDet.Cols.Frozen = vsfDet.Col + 1
        Else
            btnFreeze.Text = "&Freeze"
            tooltip.Active = False
            tooltip.SetToolTip(Me.btnFreeze, "Freeze : Alt + F")
            tooltip.Active = True
            vsfDet.Cols.Frozen = 0
            vsfDet.Cols.Frozen = 0
        End If
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub frmUtlBulkUpdate_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        Call frmMain.CloseSelectedTabPage(Me)
    End Sub

    Private Sub RdoComplete_CheckedChanged(sender As Object, e As EventArgs) Handles RdoComplete.CheckedChanged
        txtPartial.Visible = False
    End Sub

    Private Sub RdoPartial_CheckedChanged(sender As Object, e As EventArgs) Handles RdoPartial.CheckedChanged
        txtPartial.Visible = True
    End Sub

    Private Sub vsfDet_KeyUp(sender As Object, e As KeyEventArgs) Handles vsfDet.KeyUp
        With vsfDet
            If e.KeyCode = Keys.Tab Then
                If vsfDet.Row = vsfDet.Rows.Count - 1 AndAlso vsfDet.Col = vsfDet.Cols.Count - 1 Then
                    btnFreeze.Focus()
                End If
            End If
        End With
    End Sub

    'Private Sub vsfDet_KeyDown(sender As Object, e As KeyEventArgs) Handles vsfDet.KeyDown
    '    If vsfDet.Row = vsfDet.Rows.Count - 1 AndAlso vsfDet.Col = vsfDet.Cols.Count - 1 Then
    '        btnFreeze.Focus()
    '    End If
    'End Sub

    Private Sub btnDelete_Leave(sender As Object, e As EventArgs) Handles btnDelete.Leave
        btnClose.Focus()
    End Sub

    Private Sub btnShowRec_Leave(sender As Object, e As EventArgs) Handles btnShowRec.Leave
        With vsfDet
            If Not (String.IsNullOrEmpty(cboTable.Text)) Then
                .Row = 1
                .Col = 1
                Call showWaitCursor()
                'Else
                'Row = 0
                '.Col = 1
                'l showWaitCursor()
            End If
        End With
    End Sub

    Private Sub btnClose_Leave(sender As Object, e As EventArgs) Handles btnClose.Leave
        cboTable.Focus()
    End Sub

    Private Sub txtNoofRec_Leave(sender As Object, e As EventArgs) Handles txtNoofRec.Leave
        RdoComplete.Focus()
    End Sub

    Private Sub btnExcel_Leave(sender As Object, e As EventArgs) Handles btnExcel.Leave
        RdoComplete.Focus()
    End Sub

    Private Sub RdoComplete_Leave(sender As Object, e As EventArgs) Handles RdoComplete.Leave
        txtPartial.Focus()
    End Sub

    Private Sub txtPartial_Leave(sender As Object, e As EventArgs) Handles txtPartial.Leave
        txtReplaceValue.Focus()
    End Sub

    Private Sub txtReplaceValue_Leave(sender As Object, e As EventArgs) Handles txtReplaceValue.Leave
        btnUpdate.Focus()
    End Sub

    Private Sub btnUpdate_Leave(sender As Object, e As EventArgs) Handles btnUpdate.Leave
        btnDelete.Focus()
    End Sub

    Private Sub frmUtlBulkUpdate_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        cboTable.Focus()
    End Sub
End Class