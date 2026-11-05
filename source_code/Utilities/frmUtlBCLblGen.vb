Imports System.ComponentModel
Imports System.Data.oledb
Imports System.Text.RegularExpressions
Imports System.IO
Imports C1.Win.C1FlexGrid

Public Class frmUtlBCLblGen
    Dim clsSVT As New svtLibrary
    Public Mode As String
    Dim dt As New DataTable
    Dim dv As New DataView
    Dim strFile As String = ""
    Dim strFileText As String = ""

    Private Sub frmMstContactPerson_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.KeyPreview = True

        'Initialize with Default columns in Grid
        Call initGridStyles(Me)

        'Initialize Form Properties with default settings
        Call fillFormProperties(Me)

        'Form initialize event
        Call initScreen()
        Call ChangeFontSize(Me)

        'For Fixed Columns getting size from tables
        Call initFormVsfDet(Me, Me.Text, vsfDet, "DET")
        Call initFormVsfDet(Me, Me.Text, vsfView, "VIEW")
        Call initFormVsfDet(Me, Me.Text, vsfSearch, "SEARCH")

        Call bindCombo(cboBCLbl, "BCLblName", "BCLblName", "Select BCLblName from UtlBCLayout order by 1")

    End Sub

    Public Sub initScreen()

        Call initFormSize(Me, Me.Text, False, 978, 642, 0, 0)

        With vsfDet
            .Rows.Count = 1
            .Cols.Count = 1
        End With

        With vsfView
            .Rows.Count = 1
            .Cols.Count = 1
        End With

        With vsfSearch
            .Rows.Count = 1
            .Cols.Count = 1
        End With

        cboBCLbl.Focus()
        txtPrinter.Text = "PRN"
        txtPrinter.ReadOnly = True
        txtNoOfCopies.Text = 1
        chkEditor.Checked = True
    End Sub

    Private Sub frmUtlBCLblGen_KeyUp(sender As Object, e As KeyEventArgs) Handles Me.KeyUp
        If e.KeyCode = Keys.F9 And e.Control = True Then
            Call initFormSize(Me, Me.Text, True, 939, 483, 0, 0)
            Call initFormVsfDet(Me, Me.Text, vsfDet, "DET")
            Call initFormVsfDet(Me, Me.Text, vsfView, "VIEW")
            Call initFormVsfDet(Me, Me.Text, vsfSearch, "SEARCH")
            MsgBox("Restored Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
        If e.KeyCode = Keys.F8 And e.Control = True Then
            Call SaveActFormSettings(Me, Me.Text)
            Call SaveActFormVsfDet(Me, Me.Text, "DET", vsfDet)
            Call SaveActFormVsfDet(Me, Me.Text, "VIEW", vsfView)
            Call SaveActFormVsfDet(Me, Me.Text, "SEARCH", vsfSearch)
            MsgBox("Saved Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If

    End Sub

    Private Sub btnDesign_Click(sender As Object, e As EventArgs) Handles btnDesign.Click
        If cboBCLbl.Text = "" Then
            MsgBox("Please Select the Barcode Label Name.", vbInformation, Me.Text)
            cboBCLbl.Focus()
            Exit Sub
        End If

        Dim objFrm As New frmUtlBCDesigner
        objFrm.StartPosition = FormStartPosition.CenterScreen
        objFrm.txtBCName.Text = clsSVT.getDataStringValue("Select BCLblName from UtlBCLayout where BCLblName = '" & Trim(cboBCLbl.Text) & "'", "", False)
        objFrm.txtQuery.Text = clsSVT.getDataStringValue("Select BCQuery from UtlBCLayout where BCLblName = '" & Trim(cboBCLbl.Text) & "'", "", False)
        objFrm.txtBCLayout.Text = clsSVT.getDataStringValue("Select BCFmtFile from UtlBCLayout where BCLblName = '" & Trim(cboBCLbl.Text) & "'", "", False)
        objFrm.ShowDialog()
        objFrm.Dispose()
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub btnLoad_Click(sender As Object, e As EventArgs) Handles btnLoad.Click
        Dim strDetails As String = ""
        Dim strSQL As String = ""
        Dim blnFound As Boolean = False

        If Trim(cboBCLbl.Text) = String.Empty Then
            MsgBox("Please Select the Barcode Label Name.", vbInformation, Me.Text)
            cboBCLbl.Focus()
            Exit Sub
        End If

        strDetails = clsSVT.getDataStringValue("Select BCQuery from UtlBCLayout where BCLblName = '" & Trim(cboBCLbl.Text) & "'", "", blnFound)

        strFile = clsSVT.getDataStringValue("Select BCFmtFile from UtlBCLayout where BCLblName = '" & Trim(cboBCLbl.Text) & "'", "", False)

        If blnFound = False Then
            MsgBox("Please Select the valid Barcode Label Name.", vbInformation, Me.Text)
            cboBCLbl.Focus()
            vsfView.DataSource = Nothing
            vsfView.Rows.Count = 1
            vsfDet.Rows.Count = 0
            Exit Sub
        Else
            dt = clsSVT.getDATATable(strDetails)
            dv = New DataView(dt)
            vsfView.DataSource = dv
            Call syncGridCols()
            Call showNormalCursor()
            vsfView.Refresh()
        End If

        With vsfDet
            .Rows.Fixed = 0
            .Rows.Count = 0
            .Rows.Count = 1
            .Cols.Count = 0
            .Cols.Fixed = 1

            .Cols.Count = vsfView.Cols.Count

            For i As Int32 = 0 To vsfView.Cols.Count - 1
                .SetData(0, i, vsfView.GetData(0, i))
            Next

        End With
    End Sub

    Private Sub vsfView_BeforeEdit(sender As Object, e As RowColEventArgs) Handles vsfView.BeforeEdit
        e.Cancel = True
    End Sub

    Private Sub vsfView_AfterResizeColumn(sender As Object, e As RowColEventArgs) Handles vsfView.AfterResizeColumn
        Call syncGridCols()
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

    Private Sub vsfView_AfterScroll(sender As Object, e As RangeEventArgs) Handles vsfView.AfterScroll
        vsfSearch.LeftCol = vsfView.LeftCol
    End Sub

    Private Sub vsfSearch_ChangeEdit(sender As Object, e As EventArgs) Handles vsfSearch.ChangeEdit
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
        dv.RowFilter = StrQry
        Call syncGridCols()
        applyFilterFromGrid = True
    End Function

    Private Sub vsfDet_BeforeEdit(sender As Object, e As RowColEventArgs) Handles vsfDet.BeforeEdit
        e.Cancel = True
    End Sub

    Private Sub vsfDet_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles vsfDet.MouseDoubleClick
        If vsfDet.Rows.Count <= 1 Then Exit Sub
        With vsfDet
            .RemoveItem(vsfDet.Row)
        End With
    End Sub

    Private Sub txtNoOfCopies_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtNoOfCopies.KeyPress
        If Not KeyValidate("Integer", e.KeyChar) Then
            e.Handled = True
        End If
    End Sub

    Private Sub cboBCLbl_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cboBCLbl.KeyPress
        If e.KeyChar = "'" Then e.Handled = True
    End Sub

    Private Sub txtPrinter_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtPrinter.KeyPress
        If e.KeyChar = "'" Then e.Handled = True
    End Sub

    Private Sub vsfView_DoubleClick(sender As Object, e As EventArgs) Handles vsfView.DoubleClick
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
            vsfDet.AutoSizeRows()
            vsfDet.AutoSizeCols()
        End With
    End Sub

    Private Sub btnSelAll_Click(sender As Object, e As EventArgs) Handles btnSelAll.Click
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
        vsfDet.AutoSizeCols()
        vsfDet.AutoSizeRows()
    End Sub

    Private Sub btnPrint_Click(sender As Object, e As EventArgs) Handles btnPrint.Click
        Dim blnFound As Boolean = False
        strFile = clsSVT.getDataStringValue("Select BCFmtFile from UtlBCLayout where BCLblName = '" & Trim(cboBCLbl.Text) & "'", "", blnFound)
        If blnFound = True Then
            strFileText = File.ReadAllText(gstrRptPath & "\" & strFile)
        Else
            Exit Sub
        End If

        If vsfDet.Rows.Count <= 1 Then
            Exit Sub
        Else
            Try
                If chkEditor.Checked Then
                    If System.IO.File.Exists(gstrRptPath & "\" & "Sample.text") = True Then
                        File.Delete(gstrRptPath & "\" & "Sample.text")
                    End If
                    createTxtFile()
                    Process.Start(gstrRptPath & "\" & "Sample.text")
                End If
            Catch ex As Exception
                MsgBox(ex.Message, MsgBoxStyle.Exclamation, Me.Text)
            End Try
        End If
    End Sub

    Private Sub createTxtFile()
        Dim strAllMsg As String = ""
        With vsfDet
            For i As Int32 = 1 To .Rows.Count - 1
                Dim strMsg As String = ""
                strMsg = strFileText
                For k As Int32 = 0 To .Cols.Count - 1
                    strMsg = strMsg.Replace("<<" & .GetData(0, k) & ">>", .GetData(i, k))
                Next
                strAllMsg &= strMsg
            Next
            File.WriteAllText(gstrRptPath & "\" & "Sample.text", strAllMsg)
        End With
    End Sub

End Class

