Imports Microsoft.Office.Interop
Imports System.IO
Public Class frmUtlInvXLPrintQuery
    Dim clsSVT As New svtLibrary
    Const vDetXLFld = 0
    Const vDetTbl = 1
    Const vDetFldName = 2
    Private Sub frmUtlInvXLPrintQuery_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        vsfDet.Cols.Count = 3
        txtQry.Text = clsSVT.getDataStringValue("Select InvXLPrintQuery from utlProjectSettings", "", False)
        txtExcelFileName.Text = clsSVT.getDataStringValue("Select InvXLPrintFileName from utlProjectSettings", "", False)
    End Sub

    Private Sub btnRefreshQuery_Click(sender As System.Object, e As System.EventArgs) Handles btnRefreshQuery.Click
        If Not File.Exists(gstrRptPath & txtExcelFileName.Text) Then
            MsgBox("File " & gstrRptPath & txtExcelFileName.Text & " is not found!", MsgBoxStyle.Critical, Me.Text)
            Exit Sub
        End If
        Dim xlApp As New Excel.Application
        Dim xlWorkBook As Excel.Workbook
        Dim xlWorkSheet As Excel.Worksheet
        vsfDet.Rows.Count = 1
        xlWorkBook = xlApp.Workbooks.Open(gstrRptPath & txtExcelFileName.Text, , True)
        xlWorkSheet = xlWorkBook.Sheets("BILL")
        With xlWorkSheet
            For intCol As Integer = 1 To 26
                For intRow As Integer = 1 To 50
                    If .Cells(intRow, intCol).value <> "" Then
                        If InStr(.Cells(intRow, intCol).value, "<<") > 0 Then
                            Call addToFldList(.Cells(intRow, intCol).value)
                        End If
                    End If
                Next
            Next
        End With
        ReleaseObjectsFromMemory(xlWorkSheet)
        xlWorkBook.Close(False)
        ReleaseObjectsFromMemory(xlWorkBook)
        xlApp.Quit()
        ReleaseObjectsFromMemory(xlApp)
        GC.Collect()
        GC.WaitForPendingFinalizers()
    End Sub

    Private Sub addToFldList(argString As String)
        If argString = "" Then Exit Sub
        argString = Replace(argString, "<<", "▄")
        argString = Replace(argString, ">>", "▌")
        Dim strFld As String, strFldName As String, blnFound As Boolean
        Dim arrFldList As New Collection
        Dim blnStart As Boolean = False
        strFld = ""
        For i = 1 To Len(argString)
            If Mid(argString, i, 1) = "▄" Then
                strFld = ""
                blnStart = True
            ElseIf Mid(argString, i, 1) = "▌" Then
                arrFldList.Add(strFld)
                blnStart = False
                strFld = ""
            Else
                If blnStart Then strFld = strFld & Mid(argString, i, 1)
            End If
        Next
        For Each strFldName In arrFldList
            For i = 1 To vsfDet.Rows.Count - 1
                blnFound = False
                If strFldName = vsfDet.GetData(i, vDetXLFld) Then
                    blnFound = True
                    Exit For
                End If
            Next i
            If Not blnFound Then
                vsfDet.Rows.Count = vsfDet.Rows.Count + 1
                vsfDet.SetData(vsfDet.Rows.Count - 1, vDetXLFld, strFldName)
                vsfDet.SetData(vsfDet.Rows.Count - 1, vDetTbl, Mid(strFldName, 1, InStr(strFldName, ".") - 1))
                vsfDet.SetData(vsfDet.Rows.Count - 1, vDetFldName, Mid(strFldName, InStr(strFldName, ".") + 1))
            End If
        Next
    End Sub

    Private Sub btnVerifyQuery_Click(sender As System.Object, e As System.EventArgs) Handles btnVerifyQuery.Click
        If txtQry.Text = "" Then
            MsgBox("Query is not present", MsgBoxStyle.Exclamation, Me.Text)
            Exit Sub
        End If
        Dim rs As oledb.oledbDataReader
        Try
            rs = Nothing
            rs = clsSVT.getDataReaderFromSQL(txtQry.Text)
            If Not rs Is Nothing Then
                MsgBox("Query is ok", MsgBoxStyle.Exclamation, Me.Text)
            End If
        Catch ex As Exception
            MsgBox("Invalid Query" & ex.Message, MsgBoxStyle.Critical, Me.Text)
        End Try
    End Sub

    Private Sub btnClose_Click(sender As System.Object, e As System.EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub btnUpdate_Click(sender As System.Object, e As System.EventArgs) Handles btnUpdate.Click
        Dim strSQL As String
        strSQL = "Update utlProjectSettings set InvXLPrintQuery  = '" & txtQry.Text & "',"
        strSQL = strSQL & " InvXLPrintFileName = '" & txtExcelFileName.Text & "'"
        If clsSVT.execSQLStatement(strSQL) Then
            MsgBox("Updated successfully!", MsgBoxStyle.Exclamation, Me.Text)
            Me.Close()
        End If
    End Sub
End Class