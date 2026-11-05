Imports System.Data.oledb
Imports C1.Win.C1FlexGrid

Public Class frmUtlDocDeleteData
    Const vSel = 0
    Const vDocID = 1
    Const vDocName = 2
    Const vHdrTblName = 3
    Const vRecordsPrsnt = 4
    Const vDtlTblName = 5
    Const vtotalCols = 6

    Dim chk As Boolean
    Dim clsSVT As New svtLibrary

    Private Sub UtlDocDeleteData_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Call initGridStyles(Me)
        Call initGrid()
        Call fillFormProperties(Me)
        Call initFormVsfDet(Me, Me.Text, vsfDET, "Detail")
    End Sub

    Private Sub initGrid()
        With vsfDET
            .Rows.Count = 1
            .Cols.Count = VtotalCols
            .SetData(0, vSel, "Sel")
            .SetData(0, vDocID, "Doc ID")
            .SetData(0, vDocName, "Document Name")
            .SetData(0, vHdrTblName, "HdrTblName")
            .SetData(0, vDtlTblName, "DtlTblName")
            .SetData(0, vRecordsPrsnt, "Records Present")

        End With

        Call SetHideCol(vsfDET, True, vDocID, vHdrTblName, vDtlTblName)
    End Sub
    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub BtnDlt_Click(sender As Object, e As EventArgs) Handles BtnDlt.Click

        If vsfDET.FindRow(True, 1, 0, False) < 0 Then
            MsgBox("Atleast one table has to be selected!", vbInformation, Me.Text)
            Exit Sub
        End If

        Dim svtTrn As oledb.oledbTransaction
        svtTrn = Nothing
        With vsfDET
            For i = 0 To .Rows.Count - 1
                If vsfDET.GetCellCheck(i, 0) = CheckEnum.Checked Then
                    Dim strsql1 As String
                    If Not IsDBNull(vsfDET.GetData(i, vDtlTblName)) Then
                        strsql1 = "Delete from  " & vsfDET.GetData(i, vHdrTblName) & "; Delete from " & vsfDET.GetData(i, vDtlTblName)

                        clsSVT.execSQLStatement(strsql1, svtTrn)
                    Else
                        strsql1 = "Delete from " & vsfDET.GetData(i, vHdrTblName)
                        clsSVT.execSQLStatement(strsql1, svtTrn)
                    End If
                End If

            Next
            MsgBox("Data Successfully Deleted", MsgBoxStyle.Critical)
        End With

        FormCheckState()

    End Sub

    Public Sub FormCheckState()

        'If RboMasters.Checked = False Then
        '    Exit Sub
        'End If


        Dim strSQL As String = ""
        Dim rsDummy As oledbDataReader

        If RboMasters.Checked = True Then
            strSQL = "Select * from UtlTransactions where DocType='Master' order by DocName"

            ' strSql2 = "Select count(*) from UtlTransactions where DocName =" & vDocName & ""
        ElseIf RboTransactions.Checked = True Then
            strSQL = "Select * from UtlTransactions where DocType='Transaction' order by DocName"
        ElseIf RboVouchers.Checked = True Then
            strSQL = "Select * from UtlTransactions where DocType='Voucher' order by DocName"
        End If

        rsDummy = clsSVT.getDataReaderFromSQL(strSQL)



        With vsfDET
                Call initGrid()

                If Not rsDummy.HasRows Then
                    rsDummy.Close()
                    Exit Sub
                End If
                Do While rsDummy.Read
                    .Cols.Count = vtotalCols
                    .Rows.Count = .Rows.Count + 1
                    .SetData(.Rows.Count - 1, vSel, vSel)
                    .SetData(.Rows.Count - 1, vDocID, rsDummy.Item("DocID"))
                    .SetData(.Rows.Count - 1, vDocName, rsDummy.Item("DocName"))
                    .SetData(.Rows.Count - 1, vHdrTblName, rsDummy.Item("HdrTblName"))
                .SetData(.Rows.Count - 1, vDtlTblName, rsDummy.Item("DtlTblName"))
                .SetData(.Rows.Count - 1, vRecordsPrsnt, clsSVT.getDataLongValue("Select Count(*) from " & rsDummy.Item("HdrTblName"), 0, False))


            Loop


            .Cols(0).DataType = GetType(Boolean)
            End With



        vsfDET.AutoSizeCols()
    End Sub




    Private Sub BtnLoad_Click(sender As Object, e As EventArgs)
        FormCheckState()
    End Sub

    Private Sub RboMasters_CheckedChanged(sender As Object, e As EventArgs) Handles RboMasters.CheckedChanged
        FormCheckState()
    End Sub

    Private Sub RboTransactions_CheckedChanged(sender As Object, e As EventArgs) Handles RboTransactions.CheckedChanged
        FormCheckState()
    End Sub

    Private Sub vsfDET_BeforeEdit(sender As Object, e As RowColEventArgs) Handles vsfDET.BeforeEdit
        Select Case vsfDET.Col
            Case 2, 4
                e.Cancel = True

        End Select
    End Sub






    Private Sub RboVouchers_CheckedChanged(sender As Object, e As EventArgs) Handles RboVouchers.CheckedChanged
        FormCheckState()
    End Sub

    Private Sub chkSelClr_CheckedChanged(sender As Object, e As EventArgs) Handles chkSelClr.CheckedChanged
        Dim i As Integer
        If chkSelClr.Checked = True Then
            For i = 1 To vsfDET.Rows.Count - 1
                vsfDET.SetCellCheck(i, vSel, CheckEnum.Checked)
            Next
        Else
            For i = 1 To vsfDET.Rows.Count - 1
                vsfDET.SetCellCheck(i, vSel, CheckEnum.Unchecked)
            Next
        End If
    End Sub
End Class