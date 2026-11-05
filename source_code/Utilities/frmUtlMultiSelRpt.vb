Imports System.Data.oledb
Imports System.IO
Public Class frmUtlMultiSelRpt
    Dim intTotRec As Integer

    Dim clsSVT As New svtLibrary
    Dim strSQL As String
    Dim dt As DataTable
    Dim dv As DataView
    Dim strList As String
    Dim objForm As Object
    Dim intRetCol As Integer
    Dim blnRetTypeNumeric As Boolean
    Public strOrgQry As String
    Public argQry As String
    Public argSingle As Boolean
    Dim argFrm As Object
    Public argTag As String
    Dim blnRetGrid As Boolean
    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        ' MsgBox(Me.Left)
        Me.Close()

    End Sub

    Public Sub InitVsfSel(ByVal argStrSQL As String, _
                            ByVal argHidecol As Integer, _
                            ByVal argRetCol As String, _
                            ByVal argRetTypeNumeric As Boolean, _
                            ByVal argForm As Form, _
                            ByVal argRetGrid As Boolean, Optional ByVal argConn As oledbConnection = Nothing)
        'Dim i As Integer
        Try
            Me.Tag = argForm.Text
            If argTag = "" Then
                Me.Text = "Selection [Multiple] -" & argForm.Text
            Else
                Me.Text = "Selection [Multiple] -" & argTag
            End If

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
            vsfDet.Cols(0).Width = 50
            Me.Tag = Me.Text
            'Call initFormSize(Me, False, 6615, 6450, 975, 705)

            StMsg("")
            Exit Sub
        Catch ex As Exception
        End Try
        'errhandler:
        '            MsgBox(Err.Description)
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

    Private Sub txtCustName_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtCustName.TextChanged
        Call applyFilterFromGrid()
    End Sub
    Private Function applyFilterFromGrid() As Boolean
        Dim StrQry As String, strValue As String
        'Dim i As Integer
        Dim blnDateTime As Boolean = False

        applyFilterFromGrid = False
        StrQry = ""
        strValue = ""
        If txtCustName.Text <> "" Then
            strValue = Trim(txtCustName.Text)
        End If
        If strValue <> "" Then
            If StrQry <> "" Then StrQry = StrQry & " and "
            'If vsfDet.Cols(2).DataType = GetType(Decimal) Or vsfDet.Cols(2).DataType = GetType(Integer) Then
            '    Try

            '        Dim strTest = Convert.ToDecimal(strValue)
            '        StrQry = StrQry & vsfDet.GetData(0, 2) & " >= " & strValue
            '    Catch ex As Exception
            '        ' vsfSearch.SetData(0, i, "")
            '        dv.RowFilter = "1 <> 1"
            '    End Try
            'ElseIf vsfDet.Cols(2).DataType = GetType(DateTime) Then
            '    StrQry = StrQry & "CONVERT (" & vsfDet.GetData(0, 2) & ", System.String) LIKE '%" & strValue & "%'"
            'Else
            StrQry = StrQry & vsfDet.GetData(0, 2) & " like '%" & strValue & "%'"
            'End If

        End If
        'If StrQry <> "" Then
        '    vsfDet.DataSource = dt.Select(StrQry)
        'Else
        '    vsfDet.DataSource = dt
        'End If
        dv.RowFilter = StrQry
        Call vsfDet.AutoSizeCols()
        applyFilterFromGrid = True
    End Function

    Private Sub txtCustName_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtCustName.KeyPress
        If e.KeyChar = "'" Then
            e.Handled = True
        End If
    End Sub

    Private Sub chkAll_CheckedChanged(sender As Object, e As EventArgs) Handles chkAll.CheckedChanged
        If chkAll.Checked Then
            chkAll.Text = "Un-Select All"
        Else
            chkAll.Text = "Select All"
        End If

        With vsfDet
            For i As Int32 = 1 To .Rows.Count - 1
                .SetData(i, 0, chkAll.Checked)
            Next
        End With
    End Sub

    'Private Function applyFilterFromGrid() As Boolean
    '    Dim StrQry As String, strValue As String
    '    Dim i As Integer

    '    applyFilterFromGrid = False
    '    StrQry = ""
    '    With vsfDet
    '        For i = 0 To .Cols.Count - 1
    '            strValue = ""
    '            If i = .ColSel Then
    '                If .Editor.Text <> "" Then
    '                    strValue = Trim(.Editor.Text)
    '                End If
    '            Else
    '                If .GetData(0, i) <> "" Then
    '                    strValue = Trim(.GetData(0, i))
    '                End If
    '            End If
    '            If strValue <> "" Then
    '                If StrQry <> "" Then StrQry = StrQry & " and "
    '                Select Case gDBTypeCurrent
    '                    Case gDbTypeAccess
    '                        StrQry = StrQry & vsfDet.GetData(3, i) & " like '*" & strValue & "*'"
    '                    Case Else
    '                        If vsfDet.Cols(i).DataType = GetType(Decimal) Or vsfDet.Cols(i).DataType = GetType(Integer) Then
    '                            Try

    '                                Dim strTest = Convert.ToDecimal(strValue)
    '                                StrQry = StrQry & vsfDet.GetData(3, i) & " >= " & strValue
    '                            Catch ex As Exception
    '                                ' vsfSearch.SetData(0, i, "")
    '                                dv.RowFilter = "1 <> 1"
    '                                Exit Function
    '                            End Try

    '                        Else
    '                            StrQry = StrQry & vsfDet.GetData(3, i) & " like '%" & strValue & "%'"
    '                        End If
    '                End Select
    '            End If
    '        Next i
    '    End With
    '    'If StrQry <> "" Then
    '    '    vsfDet.DataSource = dt.Select(StrQry)
    '    'Else
    '    '    vsfDet.DataSource = dt
    '    'End If
    '    Dim k As Integer = vsfDet.Rows.Count
    '    dv.RowFilter = StrQry

    '    applyFilterFromGrid = True
    'End Function
End Class