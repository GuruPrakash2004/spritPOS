Imports C1.Win.C1FlexGrid

Public Class frmUtlFldCaptions
    Const vDetCode = 0
    Const vDetCaption = 1
    Const vTot = 2

    Dim dv As DataView
    Dim dt As DataTable
    Dim blnDup As Boolean = False
    Dim clsSvt As New svtLibrary
    Dim svtTrn As oledb.oledbTransaction
    Public Mode As String

    Private Sub frmUtlFldCaptions_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Call initFormSize(Me, Me.Text, False, 514, 489, 0, 0)
        Me.StartPosition = FormStartPosition.CenterScreen
        Me.KeyPreview = True
        Call fillDtls()
        Call initFormVsfDet(Me, Me.Text, vsfDet, "DET")
        SetFlexColor(vsfDet, "", 0, 1)
        Call syncGridCols()
        pnlAdd.Visible = False
        pnlAdd.SendToBack()
        Call initGridStyles(Me)
        Call fillFormProperties(Me)
        Call ChangeFontSize(Me)
    End Sub

    Private Sub fillDtls()
        Dim strsql As String

        strsql = "Select Code, Caption from UtlFldCaptions"
        Cursor.Current = Cursors.WaitCursor
        dt = clsSvt.getDATATable(strsql)
        dv = New DataView(dt)

        vsfDet.BeginUpdate()
        vsfDet.DataSource = dv
        vsfDet.EndUpdate()
        vsfSearch.Cols.Count = vsfDet.Cols.Count
        Call syncGridCols()
        Cursor.Current = Cursors.Arrow
        SetFlexColor(vsfDet, "", 0, 1)
    End Sub



    Private Sub frmUtlFldCaptions_KeyUp(sender As Object, e As KeyEventArgs) Handles Me.KeyUp

        If e.KeyCode = Keys.F9 And e.Control = True Then
            Call initFormSize(Me, Me.Text, True, 514, 489, 0, 0)
            MsgBox("Restored Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
        If e.KeyCode = Keys.F8 And e.Control = True Then
            Call SaveActFormSettings(Me, Me.Text)
            Call SaveActFormVsfDet(Me, Me.Text, "DET", vsfDet)
            MsgBox("Saved Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If

        If e.KeyCode = Keys.A And e.Alt = True Then
            Call btnAdd_Click(btnAdd, e)
        End If

        If e.KeyCode = Keys.R And e.Alt = True Then
            Call btnRemove_Click(btnRemove, e)
        End If

    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        'Me.Size = New Size(492, 470)
        Call fillDtls()

    End Sub

    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        pnlAdd.Visible = True
        pnlAdd.BringToFront()
        vsfDet.Enabled = False
        vsfDet.Visible = False
        vsfSearch.Visible = False
        vsfSearch.Enabled = False
        pnlbtns.Enabled = False
        btnAdd.Enabled = False
        btnRemove.Enabled = False
        pnlbtns.Visible = False
        btnAdd.Visible = False
        btnRemove.Visible = False
        txtFldCode.Focus()
        'Me.Size = New Size(453, 180)
    End Sub

    Private Function applyFilterFromGrid() As Boolean
        Dim StrQry As String, strValue As String
        Dim i As Integer
        applyFilterFromGrid = False
        Try
            StrQry = ""
            With vsfSearch
                For i = 0 To .Cols.Count - 1
                    strValue = ""
                    If i = .ColSel Then
                        If .Editor.Text <> "" Then
                            strValue = Trim(.Editor.Text)
                        End If
                    Else
                        If .GetData(0, i) <> "" Then
                            strValue = Trim(.GetData(0, i))
                        End If
                    End If
                    If strValue <> "" Then
                        If StrQry <> "" Then StrQry = StrQry & " and "
                        Select Case gDBTypeCurrent
                            Case gDbTypeAccess
                                StrQry = StrQry & vsfDet.GetData(0, i) & " like '*" & strValue & "*'"
                            Case Else
                                'If vsfDet.Cols(i).DataType = GetType(Decimal) Or vsfDet.Cols(i).DataType = GetType(Integer) Then
                                '    Try
                                '        Dim strTest = Convert.ToDecimal(strValue)
                                '        StrQry = StrQry & vsfDet.GetData(3, i) & " >= " & strValue
                                '    Catch ex As Exception
                                '        ' vsfSearch.SetData(0, i, "")
                                '        dv.RowFilter = "1 <> 1"
                                '        Exit Function
                                '    End Try
                                If vsfDet.Cols(i).DataType = GetType(Decimal) Or vsfDet.Cols(i).DataType = GetType(Integer) Or vsfDet.Cols(i).DataType = GetType(Date) Then
                                    Try
                                        StrQry = StrQry & "Convert(" & vsfDet.GetData(0, i) & ",'System.String')" & " like '" & strValue & "*'"
                                    Catch ex As Exception
                                        dv.RowFilter = "1 <> 1"
                                        Exit Function
                                    End Try
                                Else
                                    StrQry = StrQry & vsfDet.GetData(0, i) & " like '%" & strValue & "%'"
                                End If
                        End Select
                    End If
                Next i
            End With
            'If StrQry <> "" Then
            '    vsfDet.DataSource = dt.Select(StrQry)
            'Else
            '    vsfDet.DataSource = dt
            'End If
            Dim k As Integer = vsfDet.Rows.Count
            dv.RowFilter = StrQry
            'lblTotRec.Text = "Total Records: " & Val(Val(vsfDet.Rows.Count) - 5) & "/" & intTotRec
            Call syncGridCols()
            applyFilterFromGrid = True
        Catch ex As Exception

        End Try

    End Function

    Private Sub syncGridCols()
        Dim i As Integer
        With vsfDet
            vsfSearch.Cols.Count = vsfDet.Cols.Count
            For i = 0 To .Cols.Count - 1
                vsfSearch.Cols(i).Width = vsfDet.Cols(i).Width
                vsfSearch.Cols(i).Visible = vsfDet.Cols(i).Visible
            Next i
        End With
    End Sub

    Private Sub vsfSearch_KeyUpEdit(sender As Object, e As C1.Win.C1FlexGrid.KeyEditEventArgs) Handles vsfSearch.KeyUpEdit
        If e.KeyCode = Keys.Down Then
            vsfDet.Row = 0
            vsfDet.Col = vsfSearch.Col
            vsfDet.Focus()
        End If
    End Sub

    Private Sub vsfSearch_AfterScroll(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.RangeEventArgs) Handles vsfSearch.AfterScroll
        Dim intv As Integer = vsfSearch.ScrollPosition.X
        Dim inth As Integer = vsfDet.ScrollPosition.Y
        vsfDet.ScrollPosition = New System.Drawing.Point(intv, inth)
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        'Dim svtTrn As oledb.oledbTransaction = Nothing
        Dim strSQL As String
        Dim i As Int32

        stringReplace(Me)

        Call Duplicates()

        If blnDup = True Then
            MsgBox("Can not insert duplicate data.. ")
            vsfDet.Rows.Count = 1
            Exit Sub
        End If

        With vsfDet
            If .Rows.Count <= 1 Then
                MsgBox("Grid Empty...", MsgBoxStyle.Critical, Me.Text)
                vsfDet.Focus()
                Exit Sub
            End If

            Try
                svtTrn = svtConn.BeginTransaction(IsolationLevel.ReadCommitted)
                strSQL = " Delete from UtlFldCaptions "
                clsSvt.execSQLStatement(strSQL, svtTrn)

                For i = 1 To .Rows.Count - 1
                    strSQL = "Insert into UtlFldCaptions (Code, Caption) values("
                    strSQL &= "'" & .GetData(i, vDetCode) & "',"
                    strSQL &= "'" & .GetData(i, vDetCaption) & "')"
                    If Not clsSvt.execSQLStatement(strSQL, svtTrn) Then
                        svtTrn.Rollback()
                        Exit Sub
                    End If
                Next
                svtTrn.Commit()
                MsgBox("Data saved successfully.", vbInformation, Me.Text)

            Catch ex As Exception
                MsgBox("Error during Save" & vbCrLf & ex.Message, MsgBoxStyle.Critical, Me.Text)
                svtTrn.Rollback()
            End Try
        End With
    End Sub

    Private Sub vsfDet_AfterResizeColumn(sender As Object, e As RowColEventArgs) Handles vsfDet.AfterResizeColumn
        Call syncGridCols()
    End Sub

    Private Sub btnRemove_Click(sender As Object, e As EventArgs) Handles btnRemove.Click
        Dim strSQL As String
        If vsfDet.Row < 1 Then
            Exit Sub
        End If

        If vsfDet.Rows.Count = 2 And Trim(vsfDet.GetData(vsfDet.Row, vDetCode)) = "" Then
            MsgBox("No item present to delete!", vbInformation + vbOKOnly, Me.Text)
            Exit Sub
        End If

        If MsgBox("Are you sure you want to Remove ?", MsgBoxStyle.YesNo + vbDefaultButton2 + MsgBoxStyle.Critical, "Deletion Confirmation...") = vbNo Then Exit Sub

        strSQL = " Delete from UtlFldCaptions where Code= '" & vsfDet.GetData(vsfDet.Row, vDetCode) & "'"
        If clsSvt.execSQLStatement(strSQL) Then
            MsgBox("Field Caption Removed Successfully", vbInformation, Me.Text)
            vsfDet.RemoveItem(vsfDet.Row)
            Exit Sub
        Else
            MsgBox("Field Caption Removal Failed", vbInformation, Me.Text)
            Exit Sub
        End If
    End Sub

    Public Sub Duplicates()
        Dim i, j As Int32

        'Dim codeval, rownumval As String

        With vsfDet
            'codeval = Trim(.GetData(i, vDetCode))
            'rownumval = GetVal(.GetData(.Row, vDetCode))
            'For i = 1 To .Rows.Count - 1
            '    codeval = Trim(.GetData(i, vDetCode))
            '    rownumval = GetVal(.GetData(.Row, vDetCode))
            '    For j = 1 To .Rows.Count - 1
            '        If Trim(.GetData(j, vDetCode)) = codeval And GetVal(.GetData(.Row, vDetCode)) <> rownumval Then
            '            MsgBox("Duplicate data..")
            '        End If
            '    Next
            'Next
            For i = 1 To .Rows.Count - 1
                For j = i + 1 To .Rows.Count - 1
                    If Trim(.GetData(i, vDetCode)) = Trim(.GetData(j, vDetCode)) Then
                        MsgBox("Duplicate data..")
                        blnDup = True
                    Else
                        blnDup = False
                    End If
                Next
            Next
            'blnDup = True
            '    For i = 0 To .Rows.Count - 1
            '    For j = i + 1 To .Rows.Count - 1
            '        If .Rows(i).c(0) = .Rows(j).Index(0).ToString() Then

            '        End If
            '    Next
            'Next
        End With
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub vsfSearch_ChangeEdit(sender As Object, e As EventArgs) Handles vsfSearch.ChangeEdit
        Call applyFilterFromGrid()
    End Sub

    Private Sub vsfDet_BeforeEdit(sender As Object, e As RowColEventArgs) Handles vsfDet.BeforeEdit
        e.Cancel = True
    End Sub

    Private Sub btnPnlSave_Click(sender As Object, e As EventArgs) Handles btnPnlSave.Click
        'Dim svtTrn As oledb.oledbTransaction = Nothing
        Dim blnFound As Boolean, strSQL As String

        stringReplace(Me)


        If Trim(txtFldCode.Text) = "" Then
            MsgBox("Field Code is Empty...", MsgBoxStyle.Information, Me.Text)
            txtFldCode.Focus()
            Exit Sub
        End If

        If Trim(txtFldCaption.Text) = "" Then
            MsgBox("Field Caption is Empty...", MsgBoxStyle.Information, Me.Text)
            txtFldCaption.Focus()
            Exit Sub
        End If

        Try
            Call clsSvt.getDataStringValue("Select Code, Caption from  UtlFldCaptions " _
                           & " where Code= '" & Trim(txtFldCode.Text) & "' ", "", blnFound)
            If blnFound Then
                MsgBox("Field Code already exists..", vbInformation, Me.Text)
                txtFldCode.Text = ""
                txtFldCaption.Text = ""
                txtFldCode.Focus()
                Exit Sub
            End If

            strSQL = "Insert into UtlFldCaptions (Code, Caption) values("
            strSQL &= "'" & Trim(txtFldCode.Text) & "',"
            strSQL &= "'" & Trim(txtFldCaption.Text) & "')"

            svtTrn = svtConn.BeginTransaction(IsolationLevel.ReadCommitted)
            If Not clsSvt.execSQLStatement(strSQL, svtTrn) Then
                svtTrn.Rollback()
                Exit Sub
            End If
            MsgBox("Data Saved Successfully!", MsgBoxStyle.Information, Me.Text)
            svtTrn.Commit()

            '    Mode = ""
            txtFldCode.Text = ""
            txtFldCaption.Text = ""
            pnlAdd.Visible = False
            vsfDet.Visible = True
            vsfSearch.Visible = True
            btnAdd.Visible = True
            btnRemove.Visible = True
            pnlbtns.Visible = True

            vsfDet.Enabled = True
            vsfSearch.Enabled = True
            btnAdd.Enabled = True
            btnRemove.Enabled = True
            pnlbtns.Enabled = True
            fillDtls()
        Catch ex As Exception
            MsgBox("Error during Save" & vbCrLf & ex.Message, MsgBoxStyle.Critical, Me.Text & "[btnSave_Click]")
            svtTrn.Rollback()
        End Try
    End Sub

    Private Sub btnPnlCancel_Click(sender As Object, e As EventArgs) Handles btnPnlClose.Click
        pnlbtns.Enabled = True
        pnlbtns.Visible = True
        pnlAdd.Visible = False
        pnlAdd.SendToBack()
        vsfDet.Enabled = True
        vsfSearch.Enabled = True
        vsfDet.Visible = True
        vsfSearch.Visible = True
        btnAdd.Visible = True
        btnRemove.Visible = True
        btnAdd.Enabled = True
        btnRemove.Enabled = True
    End Sub

    Private Sub vsfDet_DoubleClick(sender As Object, e As EventArgs) Handles vsfDet.DoubleClick

    End Sub
End Class