Imports System.Data.oledb
Imports System.IO
Imports C1.Win.C1FlexGrid

Public Class frmUtlMultiSelection
    Dim intTotRec As Integer

    Dim clsSVT As New svtLibrary
    Dim strSQL As String
    Dim dt As DataTable
    Dim dv As DataView
    Public strList As String
    Dim objForm As Object
    Dim intRetCol As Integer
    Dim blnRetTypeNumeric As Boolean
    Public strOrgQry As String
    Public argQry As String
    Public argSingle As Boolean
    Dim argFrm As Object
    Public argTag As String
    Dim blnRetGrid As Boolean

    Public intColNo As Integer

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        ' MsgBox(Me.Left)
        Me.Close()

    End Sub

    Public Sub InitVsfSel(ByVal argStrSQL As String,
                            ByVal argHidecol As Integer,
                            ByVal argRetCol As String,
                            ByVal argRetTypeNumeric As Boolean,
                            ByVal argForm As Form,
                            ByVal argRetGrid As Boolean, Optional ByVal argConn As oledbConnection = Nothing)
        Me.Tag = argForm.Text
        If argTag = "" Then
            Me.Text = "Selection [Multiple] -" & argForm.Text
        Else
            Me.Text = "Selection [Multiple] -" & argTag
        End If
        Try
            argFrm = argForm
            vsfDet.Rows.Count = 1

            dt = clsSVT.getDATATable(argStrSQL, argConn)
            dv = New DataView(dt)
            vsfDet.DataSource = dv

            intRetCol = argRetCol
            blnRetTypeNumeric = argRetTypeNumeric
            objForm = argForm
            blnRetGrid = argRetGrid

            argQry = argStrSQL
            vsfDet.Cols(0).DataType = GetType(Boolean)

            If argHidecol > 0 Then
                vsfDet.Cols(argHidecol).Visible = False
            End If

            Call vsfDet.AutoSizeCols()
            Me.Tag = Me.Text
            'Call initFormSize(Me, False, 6615, 6450, 975, 705)

            Call syncGridCols()

            btnPaste.Visible = True
            If vsfDet.Rows.Count = 1 Then btnPaste.Visible = False

            StMsg("")
            Exit Sub
        Catch ex As Exception
        End Try
        'errhandler:
        '        MsgBox(Err.Description)
    End Sub

    Private Sub vsfDet_BeforeEdit(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfDet.BeforeEdit
        If e.Col <> 0 Then e.Cancel = True
    End Sub

    Private Sub vsfDet_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles vsfDet.DoubleClick
        Call loadResultBack()
    End Sub

    Private Sub loadResultBack()
        Dim i As Integer
        If Not blnRetGrid Then
            With vsfDet
                strList = ""
                For i = 1 To .Rows.Count - 1
                    If .GetCellCheck(i, 0) = C1.Win.C1FlexGrid.CheckEnum.Checked Then
                        If blnRetTypeNumeric Then
                            strList = strList & .GetData(i, intRetCol) & ","
                        Else
                            strList = strList & "'" & .GetData(i, intRetCol) & "',"
                        End If
                    End If
                Next i
                If strList <> "" Then
                    strList = Mid(strList, 1, Len(strList) - 1)
                End If
            End With
            objForm.strList = strList
        End If
        If blnRetGrid Then
            Dim stream As New System.IO.MemoryStream()
            vsfDet.WriteXml(stream)

            stream.Position = 0
            objForm.vsfretgrid.ReadXml(stream)
        End If
        Me.Close()
    End Sub

    Private Sub vsfDet_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles vsfDet.KeyUp
        If e.KeyCode = Keys.Enter Then
            Call loadResultBack()
        End If
        If vsfDet.Row = vsfDet.Rows.Count - 1 AndAlso vsfDet.Col = vsfDet.Cols.Count - 1 Then
            chkAll.Focus()
        End If
    End Sub

    Private Sub frmUtlSelection_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyUp
        If e.KeyCode = Keys.Escape Then
            Call btnCancel.PerformClick()
        End If
    End Sub

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        Call loadResultBack()
    End Sub

    Private Sub frmUtlMultiSelection_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        initGridStyles(Me)
    End Sub
    Private Sub vsfSearch_AfterScroll(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.RangeEventArgs) Handles vsfSearch.AfterScroll
        'vsfSearch.ScrollPosition = vsfDet.ScrollPosition

        Dim intv As Integer = vsfSearch.ScrollPosition.X
        Dim inth As Integer = vsfDet.ScrollPosition.Y
        vsfDet.ScrollPosition = New System.Drawing.Point(intv, inth)
    End Sub

    Private Sub vsfSearch_KeyUpEdit(sender As Object, e As C1.Win.C1FlexGrid.KeyEditEventArgs) Handles vsfSearch.KeyUpEdit
        If e.KeyCode = Keys.Down Then
            vsfDet.Row = 1
            vsfDet.Col = vsfSearch.Col
            vsfDet.Focus()
        End If
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
                                If vsfDet.Cols(i).DataType = GetType(Decimal) Or vsfDet.Cols(i).DataType = GetType(Integer) Or vsfDet.Cols(i).DataType = GetType(Date) Then
                                    Try
                                        StrQry = StrQry & "Convert(" & vsfDet.GetData(0, i) & ",'System.String')" & " like '" & strValue & "*'"
                                    Catch ex As Exception
                                        dv.RowFilter = "1 <> 1"
                                        Exit Function
                                    End Try
                                ElseIf vsfDet.Cols(i).DataType = GetType(Date) Then
                                    Try
                                        StrQry = StrQry & "Convert(" & vsfDet.GetData(0, i) & ",'System.String')" & " like '" & strValue & "*'"
                                    Catch ex As Exception
                                        dv.RowFilter = "1 <> 1"
                                        Exit Function
                                    End Try
                                Else
                                    StrQry = StrQry & vsfDet.GetData(0, i) & " like '%" & strValue & "%'"
                                End If
                                ' If vsfDet.Cols(i).DataType = GetType(Decimal) Or vsfDet.Cols(i).DataType = GetType(Integer) Then
                                '    Try

                                '        Dim strTest = Convert.ToDecimal(strValue)
                                '        StrQry = StrQry & vsfDet.GetData(0, i) & " >= " & strValue
                                '    Catch ex As Exception
                                '        ' vsfSearch.SetData(0, i, "")
                                '        dv.RowFilter = "1 <> 1"
                                '        Exit Function
                                '    End Try

                                'Else
                                '    StrQry = StrQry & vsfDet.GetData(0, i) & " like '%" & strValue & "%'"
                                'End If
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
            ' lblTotRec.Text = "Total Records: " & Val(Val(vsfDet.Rows.Count) - 4) & "/" & intTotRec
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

    Private Sub vsfDet_AfterResizeColumn(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfDet.AfterResizeColumn
        Call syncGridCols()
    End Sub

    Private Sub vsfSearch_ChangeEdit(ByVal sender As Object, ByVal e As System.EventArgs) Handles vsfSearch.ChangeEdit
        Call applyFilterFromGrid()
    End Sub

    Private Sub chkAll_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkAll.CheckedChanged
        With vsfDet
            If .Rows.Count <= 0 Then Exit Sub
            If chkAll.Checked = True Then
                For i As Int32 = 1 To .Rows.Count - 1
                    .SetData(i, 0, True)
                Next
            Else
                For i As Int32 = 1 To .Rows.Count - 1
                    .SetData(i, 0, False)
                Next
            End If
        End With
    End Sub

    Private Sub frmUtlMultiSelection_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        vsfSearch.Focus()
    End Sub

    Private Sub vsfSearch_AfterEdit(sender As Object, e As RowColEventArgs) Handles vsfSearch.AfterEdit
        With vsfSearch
            If .Row = .Rows.Count - 1 AndAlso .Col = .Cols.Count - 1 Then
                vsfDet.Focus()
            End If
        End With
    End Sub


    Private Sub btnPaste_Click(sender As Object, e As EventArgs) Handles btnPaste.Click
        If vsfDet.Rows.Count = 1 Then Exit Sub

        cboColName.Text = ""
        txtPasteData.Text = ""

        cboColName.Items.Clear()
        For x As Int32 = 0 To vsfDet.Cols.Count - 1

            If InStr(UCase(vsfDet.GetData(0, x)), "SEL") = 0 And InStr(UCase(vsfDet.GetData(0, x)), "ID") = 0 Then
                cboColName.Items.Add(vsfDet.GetData(0, x))
            End If
        Next

        If vsfDet.Col = 0 Then
            For x As Int32 = 0 To vsfDet.Cols.Count - 1
                If vsfDet.Cols(x).Visible = True Then
                    If UCase(vsfDet.GetData(0, x)) <> "SEL" Then
                        cboColName.Text = vsfDet.GetData(0, x)
                        Exit For
                    End If
                End If
            Next
        Else
            cboColName.Text = vsfDet.GetData(0, vsfDet.Col)
        End If

        intColNo = Val(vsfDet.Col)
        pnlPaste.Visible = True
        txtPasteData.Focus()
    End Sub

    Private Sub btnPasteOK_Click(sender As Object, e As EventArgs) Handles btnPasteOK.Click
        If txtPasteData.Text = "" Then
            MsgBox("No Data Present!", MsgBoxStyle.Information, Me.Text)
            Exit Sub
        End If

        intColNo = 0
        For b As Int32 = 1 To vsfDet.Cols.Count - 1
            If cboColName.Text = vsfDet.GetData(0, b) Then
                intColNo = b
                Exit For
            End If
        Next

        If intColNo = 0 Then Exit Sub

        Dim strTxt As String
        Dim i As Integer
        Dim strWrd As String, strArr() As String
        strWrd = ""
        strTxt = Replace(txtPasteData.Text, "'", "`")

        strArr = strTxt.Split(Chr(10))

        Try
            For i = 0 To strArr.Length - 1
                For a As Int32 = 1 To vsfDet.Rows.Count - 1
                    strWrd = strArr(i).ToString.Replace(Chr(13), "")
                    strWrd = Trim(strWrd.Replace(Chr(10), ""))
                    If Trim(strWrd) = vsfDet.GetData(a, intColNo) Then
                        vsfDet.SetCellCheck(a, 0, CheckEnum.Checked)
                        Exit For
                    End If
                Next
            Next

            btnPasteCncl_Click(Nothing, Nothing)
        Catch ex As Exception
            MsgBox("Invalid column selected!", MsgBoxStyle.Information, Me.Text)
        End Try
    End Sub

    Private Sub btnPasteCncl_Click(sender As Object, e As EventArgs) Handles btnPasteCncl.Click
        pnlPaste.Visible = False
    End Sub
End Class