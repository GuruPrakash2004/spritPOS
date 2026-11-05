Imports System.Data.OleDb
Public Class frmRptSMSHistory
    Dim clsSVT As New svtLibrary

    Public strList As String
    Dim strItemGroupList As String
    Dim strItemList As String
    Dim strValMethod As String
    Dim arrRctps(4, 1) As String
    Dim arrFlds() As String
    Dim intAdnlFldCount As String
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Me.Close()
        Me.Dispose()
    End Sub
    Private Sub btnView_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView.Click

    End Sub


    Private Sub RemoveVsfRows(argVSF As C1.Win.C1FlexGrid.C1FlexGrid)
        Dim i As Integer
        With argVSF
Repeat:
            For i = 1 To (.Rows.Count - 1)
                If chkZeroBal.Checked Then
                    If Val(.GetData(i, .Cols.Count - 2)) = 0 Then
                        .RemoveItem(i)
                        GoTo Repeat
                    End If
                End If
                If chkZeroValue.Checked Then
                    If Val(.GetData(i, .Cols.Count - 1)) <= Val(txtValBelow.Text) Then
                        .RemoveItem(i)
                        GoTo Repeat
                    End If
                End If
            Next i
        End With
    End Sub

    Private Sub frmRptStockBal_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.KeyPreview = True
        Call initScreen()
        Call initFormSize(Me, Me.Text, False, 958, 579, 0, 0)
        Call initFormVsfDet(Me, Me.Text, vsfView, "DET")
        Call fillFormProperties(Me)
        Call initGridStyles(Me)
    End Sub

    Private Sub initScreen()
        ' dtpFrom.Value = getFirstDateInMonth(getSystemDate)
        dtpFrom.Value = gSettings.gCompStDt
        dtpTo.Value = getSystemDate()
        'Call bindCombo(cboLocation, "LocID", "LocName", "Select locid, LocName from MstLoc where LocID > 0 order by 1")
    End Sub

    Private Sub btnExportToXL_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExportToXL.Click
        Call ExportToXL(vsfView)
    End Sub

    Private Sub vsfDet_BeforeEdit(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfView.BeforeEdit
        e.Cancel = True
    End Sub

    Private Sub frmRptStockBal_KeyUp(sender As Object, e As KeyEventArgs) Handles Me.KeyUp
        If e.KeyCode = Keys.F9 And e.Control = True Then
            Call initFormSize(Me, Me.Text, True, 958, 579, 0, 0)
            MsgBox("Restored Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
        If e.KeyCode = Keys.F8 And e.Control = True Then
            Call SaveActFormSettings(Me, Me.Text)
            Call SaveActFormVsfDet(Me, Me.Text, "DET", vsfView)
            MsgBox("Saved Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
        If e.Control = False And e.Alt = True And e.KeyCode = Keys.E Then
            Call ExportToXL(vsfView)
        End If

    End Sub
End Class