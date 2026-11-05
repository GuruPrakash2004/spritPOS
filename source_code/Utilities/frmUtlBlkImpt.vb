Imports C1.Win.C1FlexGrid
Public Class frmUtlBulkImpFlds

    Dim clsSvt As New svtLibrary
    Private Sub frmUtlBulkImpFlds_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        vsfDet.Rows.Count = 1
        vsfDet.Rows.Count = 2
        loadFlds()
    End Sub
    Private Sub vsfDet_BeforeEdit(sender As Object, e As RowColEventArgs) Handles vsfDet.BeforeEdit
        With vsfDet
            .EditMask = ""
            .ComboList = ""
            If e.Col = 0 Then
                .Cols(0).ComboList = GenVSfComboStringFromQry("SELECT column_name FROM INFORMATION_SCHEMA.COLUMNS  WHERE TABLE_NAME = 'MstItem'")
            End If
            If e.Col = 1 Then
                .Cols(1).ComboList = "Text|Integer|Date|Numeric|"
            End If
        End With
    End Sub
    Private Sub vsfDet_KeyUp(sender As Object, e As KeyEventArgs) Handles vsfDet.KeyUp
        With vsfDet
            If .Col = 1 And e.KeyCode = Keys.Tab And .Row = .Rows.Count - 1 Then
                If Trim(.GetData(.Row, 0)) <> "" Then
                    .Rows.Count = .Rows.Count + 1
                End If
            End If
        End With

    End Sub
    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub
    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Dim strSQL As String = ""
        Dim i As Integer
        clsSvt.execSQLStatement("Delete from UtlItemBulkUpdate")
        With vsfDet
            For i = 1 To .Rows.Count - 1
                If .GetData(i, 0) <> "" Then
                    strSQL = " Insert into UtlItemBulkUpdate(FieldName,DataType)"
                    strSQL = strSQL & " values ("
                    strSQL = strSQL & "'" & .GetData(i, 0) & "',"
                    strSQL = strSQL & "'" & .GetData(i, 1) & "')"
                    clsSvt.execSQLStatement(strSQL)
                End If
            Next
            MsgBox("Saved successfully.", vbInformation, Me.Text)

        End With
    End Sub
    Private Sub loadFlds()
        Dim strSql As String
        Dim rs As oledb.oledbDataReader

        strSql = "select  FieldName, DataType  from UtlItemBulkupdate"
        rs = clsSvt.getDataReaderFromSQL(strSql)
        If rs.HasRows Then
            With vsfDet
                .Rows.Count = 1
                Do While rs.Read
                    .Rows.Count = .Rows.Count + 1
                    .SetData(.Rows.Count - 1, 0, rs!FieldName)
                    .SetData(.Rows.Count - 1, 1, rs!DataType)
                Loop
            End With
        End If
    End Sub
End Class