Imports C1.Win.C1FlexGrid

Public Class frmRptMultiPrint
    Dim clsSVT As New svtLibrary
    Dim svtTrn As oledb.oledbTransaction

    Dim dt As DataTable
    Dim dv As DataView
    Public strOptionname As String
    Public intDocId As Integer

    Dim blnDateFilter As Boolean
    Dim blnDoc As Boolean
    Dim blnTrackPrinted As Boolean
    Dim strCond As String

    Dim strSHForm As String
    Dim strDocHdrTblName As String
    Dim strDocDtlTblName As String
    Dim strDocIDFldName As String
    Dim strDocNoFldName As String
    Dim strDocDateFldName As String

    Dim intPartyDocID As Int32
    Dim strPartyType As String
    Dim strPartyTblName As String
    Dim strFldPartyID As String
    Dim strFldPartyCode As String
    Dim strFldPartyName As String

    Dim intItemDocID As Int32
    Dim strItemTblName As String
    Dim strFldItemID As String
    Dim strFldItemCode As String
    Dim strFldItemName As String
    Dim strItemType As String

    Public Sub LoadPrintSettings()
        Dim strSQL As String = ""
        If vsfDet.Rows.Count < 1 Then Exit Sub
        Dim rs As oledb.oledbDataReader
        Try
            strSQL = "Select * from UtlOption where DocID = " & intDocId
            strSQL = strSQL & " And optionName ='" & strOptionname & "'"
            rs = clsSVT.getDataReaderFromSQL(strSQL)

            If rs.HasRows Then
                If rs.Read Then
                    strCond = rs.Item("MultiPrintQuery")
                    strCond = Trim(strCond)
                    If rs.Item("AllowDateRange") = "Y" Then
                        blnDateFilter = True
                        chkDateRange.Checked = True
                        'optNo.Checked = True
                    Else
                        blnDateFilter = False
                    End If
                    If rs.Item("AllowDocNoRange") = "Y" Then
                        blnDoc = True
                        chkDocNumberRange.Checked = False
                        'optNo.Checked = True
                    Else
                        blnDoc = False
                    End If
                    If rs.Item("TrackPrinted") = "Y" Then
                        blnTrackPrinted = True
                        'chkDateRange.Checked = True
                        optNo.Checked = True
                    Else
                        blnTrackPrinted = False
                    End If

                    'Panel Visible and invisible
                    pnlDateRange.Visible = blnDateFilter
                    pnlDocNumber.Visible = blnDoc
                    pnlPrint.Visible = blnTrackPrinted
                End If
            End If
            Call getUtlDocDtls()
            Call FillGrid()
            Call syncGridCols()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub FillGrid()
        Dim strSQL As String
        strSQL = strCond
        'strSQL = strSQL.Replace(Mid(strSQL, 1, 6), "Select 0 as Sel,")

        If blnDateFilter Then
            If chkDateRange.Checked Then
                strSQL &= " And " & strDocDateFldName & " between " & getDateFormatInCond(dtpFromDate.Value) & " And " & getDateFormatInCond(dtpToDate.Value) & ""
            End If
        End If

        If blnTrackPrinted Then
            If optNo.Checked Then
                strSQL &= " And IsPrinted = 'N'"
            End If
        End If

        If blnDoc Then
            If chkDocNumberRange.Checked Then
                strSQL &= " And " & strDocNoFldName & " >= " & Val(txtFrom.Text) & " And " & strDocNoFldName & " <= " & Val(txtTo.Text) & ""
            End If
        End If

        Dim dtTemp As New DataTable
        dtTemp = clsSVT.getDATATable(strSQL)

        dt = New DataTable

        Dim dc As New DataColumn
        dc.DataType = GetType(Boolean)
        dc.ColumnName = "Sel"
        dt.Columns.Add(dc)
        dt.Merge(dtTemp)

        dv = New DataView(dt)
        vsfDet.DataSource = dv

        For i = 0 To vsfDet.Cols.Count - 1
            If vsfDet.Rows(0).Item(i).ToString.ToUpper.Contains("ID") Then
                SetHideCol(vsfDet, True, i)
            End If
        Next

        For i = 1 To vsfDet.Rows.Count - 1
            vsfDet.SetData(i, 0, False)
        Next
    End Sub

    Private Sub getUtlDocDtls()

        Dim rsDummy As oledb.oledbDataReader
        rsDummy = clsSVT.getDataReaderFromSQL("Select * from UtlTransactions where DocID = " & intDocId)
        With rsDummy
            If .HasRows Then
                .Read()
                strSHForm = GetVal(!DocShForm, "")
                strDocNoFldName = strSHForm & "No"
                strDocIDFldName = strSHForm & "ID"
                strDocDateFldName = strSHForm & "Date"
                strDocHdrTblName = "TrnHdr" & strSHForm
                strDocDtlTblName = "TrnDtl" & strSHForm
                intPartyDocID = GetVal(!MstDocIDParty, 0)

            End If
        End With
        rsDummy.Close()

        strPartyType = clsSVT.getDataStringValue("Select DocShForm From UtlTransactions where DocID = " & Val(intPartyDocID), "", False)
        strPartyTblName = "Mst" & strPartyType
        strFldPartyID = strPartyType & "ID"
        strFldPartyCode = strPartyType & "Code"
        strFldPartyName = strPartyType & "Name"
    End Sub

    Private Sub frmRptMultiPrint_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Call syncGridCols()
    End Sub

    Private Sub vsfSearch_AfterScroll(sender As Object, e As RangeEventArgs) Handles vsfSearch.AfterScroll
        Dim intv As Integer = vsfSearch.ScrollPosition.X
        Dim inth As Integer = vsfDet.ScrollPosition.Y
        vsfDet.ScrollPosition = New System.Drawing.Point(intv, inth)
    End Sub

    Private Sub vsfSearch_ChangeEdit(sender As Object, e As EventArgs) Handles vsfSearch.ChangeEdit
        Call applyFilterFromGrid()
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
                                Else
                                    StrQry = StrQry & vsfDet.GetData(0, i) & " like '%" & strValue & "%'"
                                End If
                        End Select
                    End If
                Next i
            End With
            Dim k As Integer = vsfDet.Rows.Count
            dv.RowFilter = StrQry
            Call syncGridCols()
            applyFilterFromGrid = True
        Catch ex As Exception
            MsgBox(ex.Message)
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

    Private Sub vsfDet_AfterResizeColumn(sender As Object, e As RowColEventArgs) Handles vsfDet.AfterResizeColumn
        Call syncGridCols()
    End Sub

    Private Sub vsfDet_AfterScroll(sender As Object, e As RangeEventArgs) Handles vsfDet.AfterScroll
        vsfSearch.ScrollPosition = vsfDet.ScrollPosition
    End Sub

    Private Sub chkSelectAll_CheckedChanged(sender As Object, e As EventArgs) Handles chkSelectAll.CheckedChanged
        With vsfDet
            .Cols(0).AllowEditing = True
            If .Rows.Count <= 1 Then Exit Sub
            For i As Int32 = 1 To .Rows.Count - 1
                If chkSelectAll.Checked Then
                    .SetData(i, 0, True)
                Else
                    .SetData(i, 0, False)
                End If
            Next
        End With
    End Sub

    Private Sub vsfDet_BeforeEdit(sender As Object, e As RowColEventArgs) Handles vsfDet.BeforeEdit
        If e.Col <> 0 Then e.Cancel = True
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub btnLoad_Click(sender As Object, e As EventArgs) Handles btnLoad.Click
        Call FillGrid()
        vsfDet.Cols(0).AllowEditing = True
    End Sub

    Private Sub btnPrint_Click(sender As Object, e As EventArgs) Handles btnPrint.Click
        Dim strID As String = ""
        With vsfDet
            For i As Int32 = 1 To .Rows.Count - 1
                If .GetData(i, 0) = True Then
                    strID &= Val(.GetData(i, 1)) & ","
                End If
            Next
        End With

        strID = Mid(strID, 1, Len(strID) - 1)

        If strID = "" Then
            MsgBox("Please select Document from Grid....", MsgBoxStyle.Information, Me.Text)
            Exit Sub
        End If

        Try
            Dim frmv As New frmUtlReportViewer
            With frmv
                .StartPosition = FormStartPosition.CenterScreen
                .ShowRptFile_MultiPrint(intDocId, strOptionname, strID)
                .Show()
            End With

            If blnTrackPrinted Then
                Dim strSQL As String = ""
                strSQL = " Update " & strDocHdrTblName & " Set IsPrinted='Y' Where "
                strSQL = " Where " & strDocIDFldName & " in (" & strID & ") "

                Call clsSVT.execSQLStatement(strSQL)
            End If
        Catch ex As Exception

        End Try
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