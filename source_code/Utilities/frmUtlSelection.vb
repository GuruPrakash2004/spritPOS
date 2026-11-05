Imports System.Data.oledb
Imports System.IO
Public Class frmUtlSelection
    Dim intTotRec As Integer

    Dim clsSVT As New svtLibrary
    Dim strSQL As String
    Dim dt As DataTable
    Dim dv As DataView
    Dim argRetFirstCol As Integer, argRetSecCol As Integer, argRetThCol As Integer, argRetFourthCol As Integer, argRetFiveCol As Integer
    Dim argRetSixCol As Integer, argRetSevenCol As Integer
    Dim objFrm As Object, objFirst As Object, objSecond As Object, objThird As Object, objFourthObject As Object, objFiveObject As Object
    Dim objSixObject As Object, objSevenObject As Object
    Dim blnSecOBJ As Boolean, blnThrOBJ As Boolean, blnFourthObj As Boolean, blnFiveObj As Boolean, blnSixObj As Boolean, blnSevenObj As Boolean
    Public blnMultiColSearch As Boolean

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        ' MsgBox(Me.Left)
        Me.Close()

    End Sub

    Private Function applyFilterFromGrid() As Boolean
        Try
            Dim StrQry As String, strValue As String

            applyFilterFromGrid = False
            StrQry = ""
            strValue = Trim(txtSearch.Text)
            'If strValue <> "" Then
            '    StrQry = StrQry & vsfDet.GetData(0, vsfDet.Col) & " like '%" & strValue & "%'"
            'End If

            If strValue <> "" Then

                If Not blnMultiColSearch Then
                    If StrQry <> "" Then StrQry = StrQry & " and "
                    If vsfDet.Cols(vsfDet.Col).DataType = GetType(Decimal) Or vsfDet.Cols(vsfDet.Col).DataType = GetType(Integer) Or vsfDet.Cols(vsfDet.Col).DataType = GetType(Date) Then
                        Try
                            Dim strTest = Convert.ToDecimal(strValue)
                            StrQry = StrQry & "Convert(" & vsfDet.GetData(0, vsfDet.Col) & ",'System.String')" & " like '" & strValue & "*'"
                        Catch ex As Exception
                            ' vsfSearch.SetData(0, i, "")
                            dv.RowFilter = "1 = 1"
                            Exit Function
                        End Try

                    Else
                        StrQry = StrQry & "[" & vsfDet.GetData(0, vsfDet.Col) & "]" & " like '%" & strValue & "%'"
                    End If
                Else
                    For i = 0 To vsfDet.Cols.Count - 1
                        If vsfDet.Cols(i).DataType = GetType(Decimal) Or vsfDet.Cols(i).DataType = GetType(Integer) Or vsfDet.Cols(i).DataType = GetType(Date) Then
                            If StrQry <> "" Then StrQry = StrQry & " or "
                            StrQry = StrQry & "Convert(" & vsfDet.GetData(0, i) & ",'System.String')" & " like '" & strValue & "*'"
                        Else
                            If StrQry <> "" Then StrQry = StrQry & " or "
                            StrQry &= "[" & vsfDet.GetData(0, i) & "]  Like '%" & strValue & "%'"
                        End If
                    Next
                End If
            End If
            dv.RowFilter = StrQry
            applyFilterFromGrid = True
            vsfDet.AutoSizeCols()
            lblTotRecs.Text = "Total Records : " & vsfDet.Rows.Count - 1 & " / " & intTotRec
        Catch ex As Exception
            applyFilterFromGrid = False
        End Try
    End Function

    Public Sub FillViewVsf(ByVal argFrm As Form, argFromDT As Boolean, argDT As DataTable, ByVal argSQL As String, ByRef argObj As Object, ByVal argCol As Integer, _
                           Optional ByVal argSObj As Object = 0, Optional ByVal argSColNo As Integer = 0, Optional ByVal argTObj As Object = Nothing, _
                           Optional ByVal argTColNo As Integer = 0, Optional ByVal argFObj As Object = Nothing, Optional ByVal argFColNo As Integer = 0, _
                           Optional ByVal argFiObj As Object = Nothing, Optional ByVal argFiColNo As Integer = 0, Optional ByVal argSiObj As Object = Nothing, _
                           Optional ByVal argSiColNo As Integer = 0, Optional ByVal argSeObj As Object = Nothing, Optional ByVal argSeColNo As Integer = 0)
        strSQL = argSQL
        Cursor.Current = Cursors.WaitCursor
        If argFromDT Then
            dt = argDT
        Else
            dt = clsSVT.getDATATable(strSQL)
        End If
        dv = New DataView(dt)

        objFrm = argFrm
        objFirst = argObj
        objSecond = argSObj
        objThird = argTObj
        objFourthObject = argFObj
        objFiveObject = argFiObj
        objSixObject = argSiObj
        objSevenObject = argSeObj

        argRetFirstCol = argCol
        argRetSecCol = argSColNo
        argRetThCol = argTColNo
        argRetFourthCol = argFColNo
        argRetFiveCol = argFiColNo
        argRetSixCol = argSiColNo
        argRetSevenCol = argSeColNo
        vsfDet.Rows.Fixed = 1
        vsfDet.Rows.GlyphRow = 1
        vsfDet.BeginUpdate()
        '2679 3693  4645

        vsfDet.DataSource = dv
        Cursor.Current = Cursors.Arrow
        'Call initVsfForHdrs(argHdrLine1, argHdrLine2)

        vsfDet.EndUpdate()
        vsfDet.AutoSizeCols() '(0, vsfDet.Cols.Count - 1, 0)
        txtSearch.Focus()
        intTotRec = vsfDet.Rows.Count - 1
        lblTotRecs.Text = "Total Records : " & vsfDet.Rows.Count - 1 & " / " & vsfDet.Rows.Count - 1
        txtSearch.Focus()
    End Sub

    Private Sub vsfDet_BeforeEdit(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfDet.BeforeEdit
        e.Cancel = True
    End Sub

    Private Sub vsfDet_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles vsfDet.DoubleClick
        If vsfDet.Rows.Count > 1 Then
            Call loadResultBack()
        Else
            MsgBox("No records found!", vbInformation, Me.Text)
        End If
    End Sub

    Private Sub loadResultBack()
        With vsfDet
            objFirst.Text = .GetData(.Row, argRetFirstCol)
            If argRetSecCol > 0 Then
                If Not objSecond Is Nothing Then
                    objSecond.text = .GetData(.Row, argRetSecCol)
                End If
            End If
            If argRetThCol > 0 Then
                If Not objThird Is Nothing Then
                    objThird.text = .GetData(.Row, argRetThCol)
                End If
            End If

            If argRetFourthCol > 0 Then
                If Not objFourthObject Is Nothing Then
                    objFourthObject.text = .GetData(.Row, argRetFourthCol)
                End If
            End If

            If argRetFiveCol > 0 Then
                If Not objFiveObject Is Nothing Then
                    objFiveObject.text = .GetData(.Row, argRetFiveCol)
                End If
            End If
            If argRetSixCol > 0 Then
                If Not objSixObject Is Nothing Then
                    objSixObject.text = .GetData(.Row, argRetSixCol)
                End If
            End If
            If argRetSevenCol > 0 Then
                If Not objSevenObject Is Nothing Then
                    objSevenObject.text = .GetData(.Row, argRetSevenCol)
                End If
            End If
        End With
        Me.Close()
    End Sub

    Private Sub vsfSearch_ChangeEdit(ByVal sender As Object, ByVal e As System.EventArgs)
        Call applyFilterFromGrid()
    End Sub

    Private Sub vsfDet_KeyPressEdit(sender As Object, e As C1.Win.C1FlexGrid.KeyPressEditEventArgs) Handles vsfDet.KeyPressEdit
        If e.KeyChar = "'" Then
            e.Handled = True
        End If
    End Sub

    Private Sub vsfDet_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles vsfDet.KeyUp
        If e.KeyCode = Keys.Enter Then
            If vsfDet.Rows.Count > 1 Then
                Call loadResultBack()
            Else
                MsgBox("No records found!", vbInformation, Me.Text)
            End If

        End If
    End Sub

    Private Sub txtSearch_GotFocus(sender As Object, e As System.EventArgs) Handles txtSearch.GotFocus
        txtSearch.SelectAll()
    End Sub

    Private Sub txtSearch_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles txtSearch.KeyPress
        If e.KeyChar = "'" Then
            e.Handled = True
        End If
    End Sub

    Private Sub txtSearch_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtSearch.KeyUp
        If e.KeyCode = Keys.Down Then
            vsfDet.Focus()
        End If
        If e.KeyCode = Keys.Enter Then
            If vsfDet.Rows.Count > 1 Then
                vsfDet.Row = 1
                Call loadResultBack()
            End If
        End If
        If e.KeyCode = Keys.Escape Then
            Call btnCancel.PerformClick()
        End If
    End Sub

    Private Sub txtSearch_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtSearch.TextChanged
        Call applyFilterFromGrid()
    End Sub

    Private Sub frmUtlSelection_FormClosing(sender As Object, e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        dt = Nothing
        dv = Nothing

    End Sub

    Private Sub frmUtlSelection_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyUp

        If e.KeyCode = Keys.F9 And e.Control Then
            Call initFormVsfDet(Me, Me.Text, vsfDet, Tag)
            MsgBox("Restored Form & Grid Layout Successfully!!", vbInformation, Me.Text)
            Exit Sub
        End If

        If e.KeyCode = Keys.F8 And e.Control = True Then
            Call SaveActFormVsfDet(Me, Me.Text, Tag, vsfDet)
            MsgBox("Saved Form & Grid Layout Successfully!!", vbInformation, Me.Text)
            Exit Sub
        End If

        If e.KeyCode = Keys.Escape Then
            Call btnCancel.PerformClick()
        End If
    End Sub

    Private Sub frmUtlSelection_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Call initFormVsfDet(Me, Me.Text, vsfDet, Tag)
        initGridStyles(Me)
        Me.KeyPreview = True
        txtSearch.Focus()
        Me.CenterToScreen()
    End Sub

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        If vsfDet.Rows.Count > 1 Then
            Call loadResultBack()
        Else
            MsgBox("No records to load!", vbInformation, Me.Text)
        End If
    End Sub
End Class