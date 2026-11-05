Imports System.Data.oledb

Public Class frmUtlDispAuth
    Const vDetAuthSeqNo = 0
    Const vDetAuthorised = 1
    Const vDetAuthBy = 2
    Const vDetAuthOn = 3
    Const vDetAuthRemarks = 4
    Dim clsSVT As New svtLibrary

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Call getColWidth(vsfDet)
        Me.Close()
    End Sub
    Public Sub showAuthInfo(ByVal argTrnDocID As Integer, ByVal argDocID As Long)
        Dim rs As oledbDataReader, strSQL As String
        With vsfDet
            .Cols.Count = 5
            .Rows.Count = 1
            .SetData(0, vDetAuthSeqNo, "Auth Sequence")
            .SetData(0, vDetAuthorised, "Authorised")
            .SetData(0, vDetAuthBy, "Authorised By")
            .SetData(0, vDetAuthOn, "Authorised On")
            .SetData(0, vDetAuthRemarks, "Remarks")
            Call SetFlexColor(vsfDet, "", 0, 1, 2, 3)
            Call SetColWidth(vsfDet, 86, 65, 180, 82, -1)
            .Cols(vDetAuthSeqNo).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter
            .Cols(vDetAuthorised).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter
            .Cols(vDetAuthBy).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter
            .Cols(vDetAuthOn).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter

        End With
        strSQL = "select * from trnauthorise where TrnDocID  = " & argTrnDocID & " and DocID = " & argDocID & " order by AuthNumber"
        rs = clsSVT.getDataReaderFromSQL(strSQL)

        With rs
            If .HasRows Then
                Do While .Read
                    vsfDet.Rows.Count = vsfDet.Rows.Count + 1
                    vsfDet.SetData(vsfDet.Rows.Count - 1, vDetAuthSeqNo, !AuthNumber)
                    vsfDet.SetData(vsfDet.Rows.Count - 1, vDetAuthorised, !Auth)
                    vsfDet.SetData(vsfDet.Rows.Count - 1, vDetAuthBy, !AuthBy)
                    If Not IsDBNull(!AuthOn) Then
                        vsfDet.SetData(vsfDet.Rows.Count - 1, vDetAuthOn, !AuthOn)
                    End If
                    vsfDet.SetData(vsfDet.Rows.Count - 1, vDetAuthRemarks, !AuthRemarks)
                Loop
            End If
        End With
    End Sub

    Private Sub vsfDet_BeforeEdit(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfDet.BeforeEdit
        e.Cancel = True
    End Sub

    Private Sub frmUtlDispAuth_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        initGridStyles(Me)
    End Sub
End Class