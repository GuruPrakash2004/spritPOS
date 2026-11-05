Imports System.ComponentModel
Imports System.Data.oledb
Imports System.IO
Imports C1.Win.C1FlexGrid

Public Class frmUtlMstApprovals
    Dim intTotRec As Integer

    Dim clsSVT As New svtLibrary
    Dim strSQL As String
    Dim strTable As String
    Dim strAllFlds As String

    Const vDetDocID = 0
    Const vDetMaster = 1
    Const vDetApproval = 2
    Const vTot = 3

    Public argForm As Object
    Public argVSF As C1.Win.C1FlexGrid.C1FlexGrid

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub initScreen()

        Call initFormSize(Me, Me.Text, False, 474, 466, 0, 0)
        Call initFormVsfDet(Me, Me.Text, vsfDet, "DET")
        'stringReplace(Me)

        initGridStyles(Me)
        fillFormProperties(Me)

        With vsfDet
            .Rows.Count = 1
            .Cols.Count = vTot

            .SetData(0, vDetDocID, "DocID")
            .SetData(0, vDetMaster, "Master")
            .SetData(0, vDetApproval, "Master Approval")
            .Cols(vDetApproval).DataType = GetType(Boolean)
        End With
        SetHideCol(vsfDet, True, vDetDocID)
    End Sub

    Private Sub frmUtlMstApprovals_Load(sender As Object, e As EventArgs) Handles Me.Load
        Call initScreen()
        Call fillMaster()
        Call initFormVsfDet(Me, Me.Text, vsfDet, "DET")
        Me.StartPosition = FormStartPosition.CenterScreen
    End Sub

    Private Sub fillMaster()
        Dim StrSQL As String = ""
        Dim rs As oledb.oledbDataReader
        StrSQL = "Select * from UtlTransactions where DocType='Master'  and DocID not in (5,17) order by DocName "
        rs = clsSVT.getDataReaderFromSQL(StrSQL)

        If Not rs.HasRows Then
            rs.Close()
            MsgBox("Table is Empty...", MsgBoxStyle.Information, Me.Text)
            Exit Sub
        End If

        With vsfDet
            .Rows.Count = 1
            Do While rs.Read
                .Rows.Count = .Rows.Count + 1
                .SetData(.Rows.Count - 1, vDetDocID, rs!DocID)
                .SetData(.Rows.Count - 1, vDetMaster, rs!DocName)
                If GetVal(rs!AuthReq, "N") = "Y" Then
                    .SetCellCheck(.Rows.Count - 1, vDetApproval, C1.Win.C1FlexGrid.CheckEnum.Checked)
                Else
                    .SetCellCheck(.Rows.Count - 1, vDetApproval, C1.Win.C1FlexGrid.CheckEnum.Unchecked)
                End If

            Loop
        End With
        rs.Close()
    End Sub

    Private Sub vsfDet_BeforeEdit(sender As Object, e As RowColEventArgs) Handles vsfDet.BeforeEdit
        With vsfDet
            Select Case .Col
                Case vDetApproval
                Case Else
                    e.Cancel = True
            End Select
        End With
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click

        Dim svtTrn As oledb.oledbTransaction = Nothing
        Dim strSQL As String

        With vsfDet
            If .Rows.Count <= 1 Then
                MsgBox("Grid Empty...", MsgBoxStyle.Critical, Me.Text)
                vsfDet.Focus()
                Exit Sub
            End If

            Try
                svtTrn = svtConn.BeginTransaction(IsolationLevel.ReadCommitted)

                For i As Int32 = 1 To .Rows.Count - 1
                    strSQL = "Update UtlTransactions set AuthReq = "
                    If .GetCellCheck(i, vDetApproval) = C1.Win.C1FlexGrid.CheckEnum.Checked Then
                        strSQL &= "'Y'"
                    Else
                        strSQL &= "'N'"
                    End If
                    strSQL &= " where DocID=" & .GetData(i, vDetDocID)
                    If Not clsSVT.execSQLStatement(strSQL, svtTrn) Then
                        svtTrn.Rollback()
                        Exit Sub
                    End If
                Next
                svtTrn.Commit()
                MsgBox("Data saved successfully.", vbInformation, Me.Text)

            Catch ex As Exception
                MsgBox("Error during Save" & vbCrLf & ex.Message, MsgBoxStyle.Critical, Me.Text)
                svtTrn.Rollback()
            End Try
        End With

    End Sub

    Private Sub frmUtlMstApprovals_KeyUp(sender As Object, e As KeyEventArgs) Handles Me.KeyUp
        If e.KeyCode = Keys.F9 And e.Control Then
            Call initFormVsfDet(Me, Me.Text, vsfDet, "DET")
            MsgBox("Restored Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
        If e.KeyCode = Keys.F8 And e.Control = True Then
            Call SaveActFormVsfDet(Me, Me.Text, "DET", vsfDet)
            MsgBox("Saved Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
    End Sub
End Class