Imports C1.Win.C1FlexGrid
Imports System.Data.oledb

Public Class frmUtlPostDocEntry

    Const vPDEID = 0
    Const vPDEName = 1
    Const vPDERmks = 2
    Const vCondText = 3
    Const vPendCond = 4
    Const vTot = 5

    Dim clsSVT As New svtLibrary

    Private Sub frmUtlPostDocEntry_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.KeyPreview = True
        Call initFormVsfDet(Me, Me.Text, vsfPostDoc, "DET")
        Call fillFormProperties(Me)
        Call initGridStyles(Me)
        Call initScreen()
        Call fillDetails()
    End Sub

    Private Sub frmUtlPostDocEntry_KeyUp(sender As Object, e As KeyEventArgs) Handles Me.KeyUp
        If e.KeyCode = Keys.F9 And e.Control = True Then
            Call initFormVsfDet(Me, Me.Text, vsfPostDoc, "DET")
            MsgBox("Restored Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
        If e.KeyCode = Keys.F8 And e.Control = True Then
            Call SaveActFormVsfDet(Me, Me.Text, "DET", vsfPostDoc)
            MsgBox("Saved Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
    End Sub

    Private Sub initScreen()
        With vsfPostDoc
            .Rows.Count = 1
            .Cols.Count = vTot

            .SetData(0, vPDEID, "Post DocID")
            .SetData(0, vPDEName, "Post Document Name")
            .SetData(0, vPDERmks, "Remarks")
            .SetData(0, vCondText, "Condition Text")
            .SetData(0, vPendCond, "PendCond")
        End With
        Call SetHideCol(vsfPostDoc, True, vPDEID, vPendCond, vCondText)
    End Sub

    Private Sub fillDetails()
        Dim strSQL As String = ""
        Dim rs As oledb.oledbDataReader

        vsfPostDoc.Rows.Count = 1
        strSQL = " Select * from UtlPDE Order by PostDocID "
        Try
            rs = clsSVT.getDataReaderFromSQL(strSQL)
            If Not rs.HasRows Then
                rs.Close()
                Exit Sub
            End If

            With vsfPostDoc
                Do While rs.Read
                    .Rows.Count = .Rows.Count + 1
                    .SetData(.Rows.Count - 1, vPDEID, GetVal(rs!PostDocID, ""))
                    .SetData(.Rows.Count - 1, vPDEName, GetVal(rs!PostDocName, ""))
                    .SetData(.Rows.Count - 1, vPDERmks, GetVal(rs!PostDocRmks, ""))
                    .SetData(.Rows.Count - 1, vPendCond, GetVal(rs!PendCond, ""))
                    .SetData(.Rows.Count - 1, vCondText, GetVal(rs!CondText, ""))
                Loop
                rs.Close()
            End With

        Catch ex As Exception
            MsgBox(ex.Message, vbInformation, Me.Text)
        End Try

    End Sub

    Private Sub vsfPostDoc_DoubleClick(sender As Object, e As EventArgs) Handles vsfPostDoc.DoubleClick
        Call OpenPostDocRpt()
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Call frmMain.CloseSelectedTabPage(Me)
    End Sub

    Private Sub btnSelect_Click(sender As Object, e As EventArgs) Handles btnSelect.Click
        Call OpenPostDocRpt()
    End Sub

    Private Sub OpenPostDocRpt()
        If vsfPostDoc.Row >= 1 Then
            Dim obj As New frmUtlPostDocRpt
            With obj
                .intPostDocID = vsfPostDoc.GetData(vsfPostDoc.Row, vPDEID)
                .strCond = vsfPostDoc.GetData(vsfPostDoc.Row, vCondText)
                .strPendCond = vsfPostDoc.GetData(vsfPostDoc.Row, vPendCond)
                .strPDEName = vsfPostDoc.GetData(vsfPostDoc.Row, vPDEName)
            End With
            frmMain.OpenForms(obj, vsfPostDoc.GetData(vsfPostDoc.Row, vPDEName))
            frmMain.txtSearch.Focus()
        End If
    End Sub
End Class