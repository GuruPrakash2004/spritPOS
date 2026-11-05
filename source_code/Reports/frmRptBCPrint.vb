Imports System
Imports System.IO
Imports System.Text
Imports System.Collections.Generic
Imports C1.Win.C1FlexGrid
Public Class frmRptBCPrint
    Dim clsSVT As New svtLibrary
    Dim strSQL As String
    Dim sb As New StringBuilder()
    Dim dtSearch As DataTable
    Dim dvSearch As DataView
    Dim strBCFormatFile As String
    Dim strAdnlCond As String
    Dim argvsf As C1FlexGrid
    Private Sub frmRptBCPrint_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        txtPrinter.Text = GetSetting("BC", "PrntName", "PrntKey", "PRN")
        txtNoOfCopies.Text = 1
        Call bindCombo(cboTemplate, "", "BCLblName", "Select BCLblname from UtlBCLayout order by 1")

        initGridStyles(Me)
    End Sub
    Public Sub LoadBCDetails(ByVal argBCLbl As String, ByVal argSQL As String)
        cboTemplate.Text = argBCLbl
        strSQL = argSQL
    End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        Dim strBCDataFileName As String
        If vsfDet.Rows.Count <= 1 Then Exit Sub
        strBCFormatFile = gstrRptPath & "\" & clsSVT.getDataStringValue("Select BCFmtFile from UtlBCLayout Where BCLblName = '" _
                                                            & cboTemplate.Text & "'", "", False)


        strBCDataFileName = Application.StartupPath & "\BCData.txt"
        sb.Clear()
        With vsfDet
            For i = 1 To .Rows.Count - 1
                Call bcPrint(i)
            Next
        End With

        Using outfile As New StreamWriter(strBCDataFileName)
            outfile.Write(sb.ToString())
        End Using

        If chkEditor.Checked Then
            System.Diagnostics.Process.Start(strBCDataFileName)
        Else
            sb.Clear()
            sb.AppendLine("@echo off")
            sb.AppendLine(" Type " & strBCDataFileName & " > " & Trim(txtPrinter.Text))

            Using outfile As New StreamWriter(Application.StartupPath & "\PrntFile.bat")
                outfile.Write(sb.ToString())
            End Using
            System.Diagnostics.Process.Start(Application.StartupPath & "\PrntFile.bat")
        End If
        SaveSetting("BC", "PrntName", "PrntKey", txtPrinter.Text)
    End Sub

    Private Sub bcPrint(ByVal argRow As Integer)
        Dim i As Integer
        Dim strText As String = File.ReadAllText(strBCFormatFile)
        With vsfDet
            For i = 1 To .Cols.Count - 1
                strText = Replace(strText, "<<" & UCase(.GetData(0, i)) & ">>", .GetData(argRow, i))
            Next i
        End With
        If Val(txtNoOfCopies.Text) <= 0 Then txtNoOfCopies.Text = 1

        For i = 1 To Val(txtNoOfCopies.Text)
            sb.AppendLine(strText)
        Next i
    End Sub

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Me.Close()
        Me.Dispose()
    End Sub

    Private Sub btnDesign_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDesign.Click
        frmUtlBCDesigner.Show()
        frmUtlBCDesigner.MdiParent = frmMain
    End Sub

    Private Sub btnLoad_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLoad.Click
        Call fillViewGrid()
    End Sub
    Private Sub fillViewGrid()
        Dim strSQL As String
        Dim intCol As Integer

        strSQL = clsSVT.getDataStringValue("Select BCQuery from UtlBCLayout Where BCLblName = '" & cboTemplate.Text & "'", "", False)
        If strAdnlCond <> "" Then strSQL = strSQL & " " & strAdnlCond

        Call showWaitCursor()
        dtSearch = clsSVT.getDATATable(strSQL)
        dvSearch = New DataView(dtSearch)

        vsfView.DataSource = dvSearch
        Call showNormalCursor()
        vsfSearch.Cols.Count = vsfDet.Cols.Count
        vsfDet.Cols.Count = vsfView.Cols.Count
        vsfDet.Rows.Count = 1

        For intCol = 0 To vsfView.Cols.Count - 1
            vsfDet.SetData(0, intCol, vsfView.GetData(0, intCol))
            vsfDet.AutoSizeCols(intCol)
        Next intCol
        Call loadBCImg()
        If strAdnlCond <> "" Then
            vsfDet.Rows.Count = vsfView.Rows.Count
            vsfDet.Cols.Count = vsfView.Cols.Count
            For i = 1 To vsfView.Rows.Count - 1
                For j = 0 To vsfView.Cols.Count - 1
                    vsfDet.SetData(i, j, vsfView.GetData(i, j))
                Next
            Next
        End If
    End Sub
    Private Sub vsfView_AfterResizeColumn(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfView.AfterResizeColumn
        Call syncGridCols()
    End Sub

    Private Sub vsfView_BeforeEdit(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfView.BeforeEdit
        e.Cancel = True
    End Sub
    Private Sub syncGridCols()
        Dim i As Integer
        With vsfView
            vsfSearch.Cols.Count = vsfView.Cols.Count
            For i = 0 To .Cols.Count - 1
                vsfSearch.Cols(i).Width = vsfView.Cols(i).Width
                vsfSearch.Cols(i).Visible = vsfView.Cols(i).Visible
            Next i
        End With
    End Sub
    Private Sub vsfView_AfterScroll(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.RangeEventArgs) Handles vsfView.AfterScroll
        vsfSearch.LeftCol = vsfView.LeftCol
    End Sub

    Private Sub vsfSearch_ChangeEdit(ByVal sender As Object, ByVal e As System.EventArgs) Handles vsfSearch.ChangeEdit
        Call applyFilterFromGrid()
    End Sub

    Private Function applyFilterFromGrid() As Boolean
        Dim StrQry As String, strValue As String
        Dim i As Integer

        applyFilterFromGrid = False
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
                    StrQry = StrQry & vsfView.GetData(0, i) & " like '*" & strValue & "*'"
                End If
            Next i
        End With
        'If StrQry <> "" Then
        '    vsfView.DataSource = dt.Select(StrQry)
        'Else
        '    vsfView.DataSource = dt
        'End If
        dvSearch.RowFilter = StrQry
        Call syncGridCols()
        applyFilterFromGrid = True
    End Function

    Private Sub vsfSearch_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles vsfSearch.KeyUp
        If e.KeyCode = Keys.Down Then
            vsfView.Focus()
            If vsfView.Rows.Count > 1 Then
                vsfView.Row = 1
            End If
        End If

        If e.KeyCode = Keys.Return Then
            If vsfView.Rows.Count > 1 Then
                vsfView.Row = 1
                Exit Sub
            End If
        End If

    End Sub

    Private Sub vsfSearch_KeyUpEdit(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.KeyEditEventArgs) Handles vsfSearch.KeyUpEdit
        If e.KeyCode = Keys.Down Then
            vsfView.Focus()
            If vsfView.Rows.Count > 1 Then
                vsfView.Row = 1
            End If
        End If
        If e.KeyCode = Keys.Return Then
            If vsfView.Rows.Count > 1 Then
                vsfView.Row = 1
                Exit Sub
            End If
        End If

    End Sub

    Private Sub vsfSearch_RowColChange(ByVal sender As Object, ByVal e As System.EventArgs) Handles vsfSearch.RowColChange
        vsfView.Col = vsfSearch.Col
        vsfView.Refresh()
    End Sub


    Private Sub btnSelAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSelAll.Click
        Dim i As Integer, j As Integer
        vsfDet.Rows.Count = 1
        With vsfView
            For i = 1 To .Rows.Count - 1
                vsfDet.Rows.Count = vsfDet.Rows.Count + 1
                For j = 0 To .Cols.Count - 1
                    vsfDet.SetData(i, j, .GetData(i, j))
                Next j
            Next i
        End With
    End Sub

    Private Sub loadBCImg()
        lblImage.Visible = False
        If File.Exists(gstrRptPath & cboTemplate.Text & ".JPG") Then
            Try
                imgBC.BackgroundImage = Image.FromFile(gstrRptPath & cboTemplate.Text & ".JPG")
            Catch ex As Exception
            End Try

        Else
            lblImage.Visible = True
        End If
    End Sub

    Private Sub txtNoOfCopies_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtNoOfCopies.TextChanged

    End Sub


    Private Sub vsfView_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles vsfView.Click

    End Sub

    Private Sub vsfView_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles vsfView.DoubleClick
        If vsfView.Row <= 0 Then Exit Sub
        Dim i As Integer, intCol As Integer
        Dim strPendGrid As String, strSelectedGrid As String

        With vsfDet
            strPendGrid = ""
            For intCol = 0 To .Cols.Count - 1
                strPendGrid = strPendGrid & "-" & vsfView.GetData(vsfView.Row, intCol)
            Next intCol

            For i = 1 To .Rows.Count - 1
                strSelectedGrid = ""
                For intCol = 0 To vsfView.Cols.Count - 1
                    strSelectedGrid = strSelectedGrid & "-" & .GetData(i, intCol)
                Next intCol

                If strSelectedGrid = strPendGrid Then
                    MsgBox("Item Already present!!", vbInformation, Me.Text)
                    Exit Sub
                End If
            Next i
        End With

        With vsfView
            vsfDet.Rows.Count = vsfDet.Rows.Count + 1
            i = vsfDet.Rows.Count - 1

            For intCol = 0 To .Cols.Count - 1
                vsfDet.SetData(i, intCol, .GetData(.Row, intCol))
            Next intCol
        End With
    End Sub

    Public Sub loadCustom(ByVal argCond As String)
        strAdnlCond = argCond
        Call fillViewGrid()
    End Sub


    Private Sub pnlbtnClose_Click(sender As Object, e As EventArgs) Handles pnlbtnClose.Click
        pnlCopyShortCut.Visible = False
    End Sub

    Private Sub btnCopy_Click(sender As Object, e As EventArgs) Handles btnCopy.Click

        If RbnCopyCell.Checked = True Then
            CopyGrid(argvsf, "Cell")
        ElseIf RbnCopyColumn.Checked = True Then

            CopyGrid(argvsf, "Column")

        ElseIf RbnCopyRow.Checked = True Then

            CopyGrid(argvsf, "Row")
        ElseIf RbnCopyGrid.Checked = True Then

            CopyGrid(argvsf, "Full Grid")

        End If

        pnlCopyShortCut.Visible = False
    End Sub

    Private Sub vsfDet_Click(sender As Object, e As EventArgs) Handles vsfDet.Click

        If (Control.ModifierKeys And Keys.Control) = Keys.Control AndAlso (Control.ModifierKeys And Keys.Alt) = Keys.Alt Then
            pnlCopyShortCut.Show()
        End If
        argvsf = vsfDet

    End Sub

    Private Sub vsfSearch_Click(sender As Object, e As EventArgs) Handles vsfSearch.Click
        If (Control.ModifierKeys And Keys.Control) = Keys.Control AndAlso (Control.ModifierKeys And Keys.Alt) = Keys.Alt Then
            pnlCopyShortCut.Show()
        End If
        argvsf = vsfSearch
    End Sub
End Class