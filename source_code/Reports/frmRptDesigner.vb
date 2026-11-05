
Imports System.Data.oledb
Imports System.IO
Imports C1.Win.C1FlexGrid
Public Class frmRptDesigner
    Dim clsSVT As New svtLibrary
    Dim strSQL As String
    Dim argCForm As Object
    Dim dt As DataTable
    Dim dv As DataView
    Dim _src As C1FlexGrid = Nothing

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        strSQL = "Select * from mstitem"
        Cursor.Current = Cursors.WaitCursor
        dt = clsSVT.getDATATable(strSQL)
        dv = New DataView(dt)

        vsfDet.DataSource = dv
    End Sub

    Private Sub vsfDet_BeforeMouseDown(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.BeforeMouseDownEventArgs) Handles vsfDet.BeforeMouseDown
        ' start dragging when the user clicks the column headers
        Dim flex As C1FlexGrid = TryCast(sender, C1FlexGrid)
        Dim hti As HitTestInfo = flex.HitTest(e.X, e.Y)
        If hti.Type = HitTestTypeEnum.ColumnHeader Then
            ' select the col
            Dim indexCol As Integer = hti.Column
            flex.[Select](0, indexCol, flex.Rows.Count - 1, indexCol, False)

            ' save info for target
            _src = flex

            ' do drag drop
            Dim dd As DragDropEffects = flex.DoDragDrop(flex.Clip, DragDropEffects.Move)
            ' if it worked, delete column from source (it's a move)
            'If dd = DragDropEffects.Move Then
            '    flex.Cols.Remove(indexCol)
            'End If

            ' done, reset info
            _src = Nothing
        End If
    End Sub

    Private Sub vsfGroupBy_DragDrop(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles vsfGroupBy.DragDrop
        Dim flex As C1FlexGrid = TryCast(sender, C1FlexGrid)
        Dim pt As Point = flex.PointToClient(New Point(e.X, e.Y))
        Dim hti As HitTestInfo = flex.HitTest(pt.X, pt.Y)
        Dim indexCol As Integer = hti.Column

        If indexCol >= 0 Then
            vsfGroupBy.Rows.Count = vsfGroupBy.Rows.Count + 1
            vsfGroupBy.SetData(vsfGroupBy.Rows.Count - 1, 0, vsfDet.GetData(0, vsfDet.Col))
        End If
    End Sub

    Private Sub vsfDet_DragOver(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles vsfDet.DragOver, vsfGroupBy.DragOver
        ' check whether we can drop here:
        ' we must have a source object, and it should be the other grid
        ' (this sample allows dragging from one to the other grid only)
        If _src IsNot Nothing AndAlso Not _src.Equals(sender) Then
            ' check that we have the type of data we want
            If e.Data.GetDataPresent(GetType(String)) Then
                e.Effect = DragDropEffects.Move
            End If
        End If
    End Sub

    Private Sub frmRptDesigner_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        initGridStyles(Me)
    End Sub


    Private Sub pnlbtnClose_Click(sender As Object, e As EventArgs) Handles pnlbtnClose.Click
        pnlCopyShortCut.Visible = False
    End Sub

    Private Sub btnCopy_Click(sender As Object, e As EventArgs) Handles btnCopy.Click
        If RbnCopyCell.Checked = True Then
            CopyGrid(vsfDet, "Cell")
        ElseIf RbnCopyColumn.Checked = True Then

            CopyGrid(vsfDet, "Column")

        ElseIf RbnCopyRow.Checked = True Then

            CopyGrid(vsfDet, "Row")
        ElseIf RbnCopyGrid.Checked = True Then
            CopyGrid(vsfDet, "Full Grid")
        End If
        pnlCopyShortCut.Visible = False
    End Sub

    Private Sub vsfDet_Click(sender As Object, e As EventArgs) Handles vsfDet.Click

        If (Control.ModifierKeys And Keys.Control) = Keys.Control AndAlso (Control.ModifierKeys And Keys.Alt) = Keys.Alt Then
            pnlCopyShortCut.Show()
        End If

    End Sub
End Class