Imports System.Data.oledb
Imports System.IO
Public Class frmUtlMultiSelAudit
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
    Public blnCancel As Boolean = False
    Public strTempList As String = ""

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        ' MsgBox(Me.Left)
        blnCancel = True
        objForm.strList = strTempList
        Me.Close()
    End Sub

    Public Sub InitVsfSel(ByVal argStrSQL As String, _
                            ByVal argHidecol As Integer, _
                            ByVal argRetCol As String, _
                            ByVal argRetTypeNumeric As Boolean, _
                            ByVal argForm As Form, _
                            ByVal argRetGrid As Boolean, Optional ByVal argConn As oledbConnection = Nothing, Optional argStrList As String = "")
        Dim i As Integer
        'On Error GoTo errhandler
        Me.Tag = argForm.Text

        If argStrList <> "" Then
            strTempList = argStrList
        End If
        Try
            Dim cs = vsfDet.Styles.Add("Boolean")
            cs.DataType = GetType(Boolean)
            cs.ImageAlign = C1.Win.C1FlexGrid.ImageAlignEnum.CenterCenter
            vsfDet.SetCellStyle(0, 0, cs)

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

            If Trim(argStrList) <> "" Then
                If argStrList = "ALL" Then
                    For i = 0 To vsfDet.Rows.Count - 1
                        vsfDet.SetCellCheck(i, 0, C1.Win.C1FlexGrid.CheckEnum.Checked)
                    Next
                Else
                    For i = 1 To vsfDet.Rows.Count - 1
                        If argStrList.Contains(vsfDet.GetData(i, 1)) Then
                            vsfDet.SetCellCheck(i, 0, C1.Win.C1FlexGrid.CheckEnum.Checked)
                        Else
                            vsfDet.SetCellCheck(i, 0, C1.Win.C1FlexGrid.CheckEnum.Unchecked)
                        End If
                    Next
                End If
            End If

            Call vsfDet.AutoSizeCols()
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

    Private Sub vsfDet_MouseClick(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles vsfDet.MouseClick
        Dim ht = vsfDet.HitTest
        If ht.Row < 0 Then Exit Sub

        If ht.Row = 0 And ht.Column = 0 Then
            If vsfDet.GetCellCheck(0, 0) = C1.Win.C1FlexGrid.CheckEnum.Checked Then
                For i = 1 To vsfDet.Rows.Count - 1
                    vsfDet.SetCellCheck(i, 0, C1.Win.C1FlexGrid.CheckEnum.Checked)
                Next
            Else
                For i = 1 To vsfDet.Rows.Count - 1
                    vsfDet.SetCellCheck(i, 0, C1.Win.C1FlexGrid.CheckEnum.Unchecked)
                Next
            End If
        End If

        vsfDet.SetCellCheck(0, 0, C1.Win.C1FlexGrid.CheckEnum.Checked)

        For i = 1 To vsfDet.Rows.Count - 1
            If vsfDet.GetCellCheck(i, 0) = C1.Win.C1FlexGrid.CheckEnum.Unchecked Then
                vsfDet.SetCellCheck(0, 0, C1.Win.C1FlexGrid.CheckEnum.Unchecked)
                Exit For
            End If
        Next

    End Sub
End Class