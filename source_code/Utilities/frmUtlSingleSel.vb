Imports System.Data.oledb
Imports System.IO

Public Class frmUtlSingleSel
    Dim clsSVT As New svtLibrary
    Dim strSQL As String
    Dim argCForm As Object
    Dim argSecObj As Object
    Dim argThirdObj As Object
    Dim argFourthObj As Object
    Dim argCFillObj As Object
    Dim dt As DataTable
    Dim dv As DataView
    Dim intRetCol As Integer
    Dim intHideCol As Integer
    Dim strScrollBarWidth As Integer
    Dim intSecRetCol As Integer
    Dim intThirdRetCol As Integer
    Dim intForthRetCol As Integer
    Public blnDefRow As Boolean

    Public Sub FillViewVsf(ByVal argFrm As Form, ByVal argSQL As String, ByVal argObject As Object, ByVal argRetCol As Integer, ByVal argHideCol As Integer, _
                                  Optional ByVal argSecObject As Object = Nothing, Optional ByVal argSecRetCol As Integer = 0, _
                                  Optional ByVal argThirdObject As Object = Nothing, Optional ByVal argTrirdRetCol As Integer = 0, _
                                  Optional ByVal argFourthObject As Object = Nothing, Optional ByVal argFourthRetCol As Integer = 0)
        strSQL = argSQL
        Cursor.Current = Cursors.WaitCursor
        dt = clsSVT.getDATATable(strSQL)
        dv = New DataView(dt)

        vsfDet.Rows.Fixed = 1
        vsfDet.BeginUpdate()

        vsfDet.DataSource = dv
        Cursor.Current = Cursors.Arrow
        argCForm = argFrm
        argCFillObj = argObject
        intRetCol = argRetCol

        argSecObj = argSecObject
        intSecRetCol = argSecRetCol

        argThirdObj = argThirdObject
        intThirdRetCol = argTrirdRetCol

        argFourthObj = argFourthObject
        intForthRetCol = argFourthRetCol

        intHideCol = argHideCol
        'Call initVsfForHdrs()

        vsfDet.EndUpdate()
        vsfSearch.Cols.Count = vsfDet.Cols.Count

        'Call SetHideCol(vsfDet, True, 0, 1, 3)
        'For i = 5 To 40
        '    Call SetHideCol(vsfDet, True, i)
        'Next

        If argHideCol >= 0 Then vsfDet.Cols(argHideCol).Visible = False
        Call syncGridCols()
        strScrollBarWidth = SystemInformation.VerticalScrollBarWidth

        vsfSearch.Cols(vsfSearch.Cols.Count - 1).Width = vsfDet.Cols(vsfDet.Cols.Count - 1).Width + strScrollBarWidth

        If blnDefRow Then Call UpdateGridColor()
    End Sub

    Public Sub syncGridCols()
        Dim i As Integer
        With vsfDet
            vsfSearch.Cols.Count = vsfDet.Cols.Count
            For i = 0 To .Cols.Count - 1
                vsfSearch.Cols(i).Width = vsfDet.Cols(i).Width
                vsfSearch.Cols(i).Visible = vsfDet.Cols(i).Visible
            Next i
        End With
    End Sub

    Private Sub vsfdet_OwnerDrawCell(sender As System.Object, e As C1.Win.C1FlexGrid.OwnerDrawCellEventArgs) Handles vsfDet.OwnerDrawCell
        If Not blnDefRow Then Exit Sub
        If sender.Rows(e.Row).Style IsNot Nothing Then
            If sender.Rows(e.Row).Style.Name = "StyleClr" & e.Row Then
                e.Style = sender.Styles("StyleClr" & e.Row)
            End If
        End If
    End Sub

    Private Sub UpdateGridColor()
        vsfDet.DrawMode = C1.Win.C1FlexGrid.DrawModeEnum.OwnerDraw
        Dim intCol As Int32 = -1
        Dim intRow As Int32 = -1
        With vsfDet
            For i As Int32 = 0 To .Cols.Count - 1
                If InStr(UCase(.GetData(0, i)), "DEFAULTCOL") > 0 Then
                    intCol = i
                    Exit For
                End If
            Next

            If intCol <= -1 Then Exit Sub
            If .Rows.Count <= 0 Then Exit Sub
            For j As Int32 = 0 To .Rows.Count - 1
                If .GetData(j, intCol) = "Y" Then
                    Dim csf1 As C1.Win.C1FlexGrid.CellStyle = .Styles.Add("StyleClr" & j)
                    Dim myColor As Color = Color.LightGreen
                    csf1.BackColor = myColor
                    .Rows(j).Style = csf1
                End If
            Next
        End With
    End Sub

    Private Sub initVsfForHdrs(ByVal argHdrLine1 As String, ByVal argHdrLine2 As String)
        Dim i As Integer
        With vsfDet
            For i = 0 To .Cols.Count - 1
                .SetData(0, i, .GetData(0, i))
            Next
        End With
    End Sub

    Private Sub vsfDet_AfterResizeColumn(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfDet.AfterResizeColumn
        Call syncGridCols()
    End Sub

    Private Sub vsfDet_BeforeEdit(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfDet.BeforeEdit
        e.Cancel = True
    End Sub
     
    Private Sub vsfDet_AfterScroll(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.RangeEventArgs) Handles vsfDet.AfterScroll
        vsfSearch.ScrollPosition = vsfDet.ScrollPosition
    End Sub

    Private Sub vsfSearch_AfterScroll(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.RangeEventArgs) Handles vsfSearch.AfterScroll
        vsfSearch.ScrollPosition = vsfDet.ScrollPosition
    End Sub

    Private Sub vsfSearch_ChangeEdit(ByVal sender As Object, ByVal e As System.EventArgs) Handles vsfSearch.ChangeEdit
        Call applyFilterFromGrid()
    End Sub

    Private Function applyFilterFromGrid() As Boolean
        Dim StrQry As String, strValue As String
        Dim i As Integer
        Dim blnDateTime As Boolean = False

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
                        If vsfDet.Cols(i).DataType = GetType(Decimal) Or vsfDet.Cols(i).DataType = GetType(Integer) Or vsfDet.Cols(i).DataType = GetType(Date) Then
                            ' StrQry = StrQry & "CONVERT (" & vsfDet.GetData(0, i) & ", System.String) LIKE '%" & strValue & "%'"
                            StrQry = StrQry & "Convert(" & vsfDet.GetData(0, i) & ",'System.String')" & " like '" & strValue & "*'"
                        Else
                            StrQry = StrQry & vsfDet.GetData(0, i) & " like '%" & strValue & "%'"
                        End If

                    End If
                Next i
            End With
            'If StrQry <> "" Then
            '    vsfDet.DataSource = dt.Select(StrQry)
            'Else
            '    vsfDet.DataSource = dt
            'End If
            dv.RowFilter = StrQry

            Call syncGridCols()
            applyFilterFromGrid = True
        Catch ex As Exception

        End Try
        
    End Function

    Private Sub frmUtlSingleSel_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyUp
        If e.KeyCode = Keys.Escape Then
            Me.Close()
        End If

        If e.KeyCode = Keys.F5 Then
            FillViewVsf(argCForm, strSQL, argCFillObj, intRetCol, intHideCol, argSecObj, intSecRetCol)
        End If
    End Sub

    Private Sub frmUtlSingleSel_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        KeyPreview = True
        initGridStyles(Me)
        Dim tooltip As New ToolTip
        tooltip.SetToolTip(Me.btnClose, "Close : Alt+O")
    End Sub

    Private Sub vsfSearch_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles vsfSearch.KeyDown
        If e.KeyCode = Keys.Down Then
            If vsfDet.Rows.Count > 1 Then
                vsfDet.Row = 1
                vsfDet.Col = 1
            End If
            vsfDet.Focus()
        End If
    End Sub

    Private Sub vsfSearch_KeyPressEdit(sender As Object, e As C1.Win.C1FlexGrid.KeyPressEditEventArgs) Handles vsfSearch.KeyPressEdit
        If e.KeyChar = "'" Then
            e.Handled = True
        End If
    End Sub

    Private Sub vsfSearch_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles vsfSearch.KeyUp
        If e.KeyCode = Keys.Down Then
            If vsfDet.Rows.Count > 1 Then
                vsfDet.Row = 1
                vsfDet.Col = 1
            End If
            vsfDet.Focus()
        End If
    End Sub

    Private Sub btnClose_Click(sender As System.Object, e As System.EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub vsfDet_KeyUp(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles vsfDet.KeyUp
        Try
            If vsfDet.Row > 0 And vsfDet.Rows.Count > 1 Then
                argCFillObj.text = (vsfDet.GetData(vsfDet.Row, intRetCol))

                If Not argSecObj Is Nothing Then
                    argSecObj.text = (vsfDet.GetData(vsfDet.Row, intSecRetCol))
                End If

                If Not argThirdObj Is Nothing Then
                    argThirdObj.text = (vsfDet.GetData(vsfDet.Row, intThirdRetCol))
                End If

                If Not argFourthObj Is Nothing Then
                    argFourthObj.text = (vsfDet.GetData(vsfDet.Row, intForthRetCol))
                End If

                With vsfDet
                    If (e.KeyCode = Keys.Escape) And .Row = .Rows.Count - 1 Then
                        btnClose.Focus()
                        Exit Sub
                    End If
                End With
                If e.KeyCode = Keys.Enter Or e.KeyCode = Keys.Space Then
                    Me.Close()
                End If
            End If

        Catch ex As Exception

        End Try
    End Sub

    Private Sub vsfDet_MouseDoubleClick(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles vsfDet.MouseDoubleClick
        Dim ht = vsfDet.HitTest(e.X, e.Y)

        If ht.Row <= 0 Then Exit Sub

        argCFillObj.text = (vsfDet.GetData(vsfDet.Row, intRetCol))

        If Not argSecObj Is Nothing Then
            argSecObj.text = vsfDet.GetData(vsfDet.Row, intSecRetCol)
        End If

        If Not argThirdObj Is Nothing Then
            argThirdObj.text = (vsfDet.GetData(vsfDet.Row, intThirdRetCol))
        End If

        If Not argFourthObj Is Nothing Then
            argFourthObj.text = (vsfDet.GetData(vsfDet.Row, intForthRetCol))
        End If

        Me.Close()
    End Sub

    Private Sub vsfSearch_KeyUpEdit(sender As Object, e As C1.Win.C1FlexGrid.KeyEditEventArgs) Handles vsfSearch.KeyUpEdit
        If e.KeyCode = Keys.Down Then
            vsfDet.Row = 1
            vsfDet.Col = vsfSearch.Col
            vsfDet.Focus()
        End If
    End Sub

    Private Sub frmUtlSingleSel_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        'With vsfSearch
        '    .Row = 0
        '    .Col = 1
        '    .Focus()
        'End With
    End Sub
End Class