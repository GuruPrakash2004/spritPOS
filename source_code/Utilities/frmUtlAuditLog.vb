Imports System.Data.oledb
Public Class frmUtlAuditLog

    Dim clsSVT As New svtLibrary
    Dim mode As String

    Const vDetType = 0
    Const vDetDoc = 1
    Const vDetDocID = 2
    Const vDetAuditLog = 3
    Const vDetFldwWiseTrac = 4
    Const vDetTotCols = 5

    Private Sub initScreen()
        With vsfDet
            .Cols.Count = vDetTotCols
            .Rows.Count = 1

            .SetData(0, vDetType, "Type")
            .SetData(0, vDetDocID, "DOC ID")
            .SetData(0, vDetDoc, "Document")
            .SetData(0, vDetAuditLog, "AuditLogReq")
            .SetData(0, vDetFldwWiseTrac, "FldwiseTrackReq")

            .Cols(vDetAuditLog).DataType = GetType(Boolean)
            .Cols(vDetFldwWiseTrac).DataType = GetType(Boolean)
            .Cols(vDetDocID).Visible = False
            Call SetFlexColor(vsfDet, "", vDetType)
            Call SetFlexColor(vsfDet, "", vDetDoc)

        End With

        Call fillTRNDtls()
    End Sub

    Private Function saveDetails() As Boolean
        Dim strSQL As String = ""
        Dim strValues As String = ""
        Dim strFlds As String = ""
        saveDetails = False

        With vsfDet
            For i = 1 To .Rows.Count - 1
                strSQL = "update UtlTransactions set"

                If .GetCellCheck(i, vDetAuditLog) = C1.Win.C1FlexGrid.CheckEnum.Checked Then
                    strSQL = strSQL & " AuditLogReq = 'Y',"
                Else
                    strSQL = strSQL & " AuditLogReq = 'N',"
                End If

                If .GetCellCheck(i, vDetFldwWiseTrac) = C1.Win.C1FlexGrid.CheckEnum.Checked Then
                    strSQL = strSQL & " FldWiseTrackReq = 'Y'"
                Else
                    strSQL = strSQL & " FldWiseTrackReq = 'N'"
                End If

                strSQL = strSQL & " where DocType = '" & .GetData(i, vDetType) & "'"
                strSQL = strSQL & " and DocID = " & Val(.GetData(i, vDetDocID))
                strSQL = strSQL & " and DocName = '" & .GetData(i, vDetDoc) & "'"

                Call clsSVT.execSQLStatement(strSQL)
            Next
        End With

        Me.Close()
    End Function

    Public Sub fillTRNDtls()
        Dim RsTemp As oledb.oledbDataReader
        Dim strSQL As String
        Dim i As Integer

        strSQL = " select * from UtlTransactions order by Doctype, DocName"
        RsTemp = clsSVT.getDataReaderFromSQL(strSQL)

        With vsfDet
            .Rows.Count = 1
            Do While RsTemp.Read()
                i = .Rows.Count
                .Rows.Count = .Rows.Count + 1

                .SetData(i, vDetType, RsTemp!DocType)
                .SetData(i, vDetDoc, RsTemp!DocName)
                .SetData(i, vDetDocID, RsTemp!DocID)

                If GetVal(RsTemp!AuditLogReq, "N") = "Y" Then
                    .SetCellCheck(i, vDetAuditLog, C1.Win.C1FlexGrid.CheckEnum.Checked)
                Else
                    .SetCellCheck(i, vDetAuditLog, C1.Win.C1FlexGrid.CheckEnum.Unchecked)
                End If

                If GetVal(RsTemp!FldWiseTrackReq, "N") = "Y" Then
                    .SetCellCheck(i, vDetFldwWiseTrac, C1.Win.C1FlexGrid.CheckEnum.Checked)
                Else
                    .SetCellCheck(i, vDetFldwWiseTrac, C1.Win.C1FlexGrid.CheckEnum.Unchecked)
                End If

            Loop

            Call vsfDet.AutoSizeCols()
            RsTemp.Close()
        End With
    End Sub

    Private Sub frmUtlAuditLog_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Call initScreen()
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If saveDetails() Then
        End If
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
        '  Call frmMain.CloseSelectedTabPage(Me)
    End Sub

   
    Private Sub vsfDet_BeforeEdit(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfDet.BeforeEdit
        Select Case e.Col
            Case vDetType
                e.Cancel = True
            Case vDetDoc
                e.Cancel = True
               End Select
    End Sub

    Private Sub frmUtlAuditLog_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        Call frmMain.CloseSelectedTabPage(Me)
    End Sub


End Class
