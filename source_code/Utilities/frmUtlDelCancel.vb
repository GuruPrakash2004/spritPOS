Public Class frmUtlDelCancel
    Dim clsSVt As New svtLibrary
    Public argDocID As Long
    Public argDocName As String
    Public argCaption As String
    Public argForm As Object
    Public argDocIDFldName As String
    Public argDocNoFldValue As String
    Public argHdrTblName As String
    Public argDtlTblName As String
    Public argTblList As String
    Public strDelCancelStatus As String

    Public argBlnLog As Boolean = False
    Public argMenu As String = ""

    Public argColItemID As Int32
    Public argColQty As Int32
    Public argVSF As C1.Win.C1FlexGrid.C1FlexGrid

    Private Sub frmUtlDelCancel_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.Text = argCaption
    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click

        'If Not blnDeleteOK(argHdrTblName, argDocNoFldValue, True, True) Then
        '    MsgBox("Transaction exists. Cannot Cancel!!", vbInformation, Me.Text)
        '    Exit Sub
        'End If 
        ''Top 4 lines commited by Anand
        If Not blnDeleteOK(argHdrTblName, argDocNoFldValue, True, True) Then
            Exit Sub
        End If

        If deleteDocEntry(argDocID, argDocIDFldName, argDocNoFldValue, argTblList, argBlnLog, argMenu) Then
            MsgBox("Successfully Deleted the entry!", MsgBoxStyle.Information, Me.Text)
            strDelCancelStatus = "DELETED"
        End If
        Me.Close()
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click

        If Not blnDeleteOK(argHdrTblName, argDocNoFldValue, True, True) Then
            'MsgBox "Transaction exists. Cannot Cancel!!", vbInformation, Me.Caption
            Exit Sub
        End If

        If MsgBox("Are you sure to Cancel this Entry ", _
                vbYesNo + vbDefaultButton2 + vbCritical, _
                Me.Text) = vbNo Then
            Exit Sub
        End If
        strDelCancelStatus = "CANCELLED"
        If CancelDocEntry(argDocID, argDocIDFldName, argDocNoFldValue, argHdrTblName, argBlnLog, argMenu) Then
            MsgBox("Successfully CANCELLED the entry!", MsgBoxStyle.Information, Me.Text)
            strDelCancelStatus = "CANCELLED"
        End If

        Me.Close()
    End Sub

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

End Class