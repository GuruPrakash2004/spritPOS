Public Class frmUtlAddItem
    Dim clsSVT As New svtLibrary
    Public strItemList As String = ""
    Private Sub btnLoadItems_Click(sender As System.Object, e As System.EventArgs) Handles btnLoadItems.Click
        Dim strTxt As String
        Dim i As Integer
        Dim strWrd As String

        i = 1
        vsfItems.Rows.Count = 1
        vsfItems.Cols.Count = 3
        vsfItems.Cols(0).Width = 100
        vsfItems.ExtendLastCol = True
        vsfItems.Cols(2).Visible = False
        showWaitCursor()
        strWrd = ""
        strTxt = txtData.Text
        Do While i <= Len(strTxt)
            Select Case Mid(strTxt, i, 1)
                Case ",", Chr(10), Chr(13)
                    If strWrd <> "" Then
                        vsfItems.Rows.Count = vsfItems.Rows.Count + 1
                        vsfItems.SetData(vsfItems.Rows.Count - 1, 1, strWrd)
                    End If
                    strWrd = ""
                Case Else
                    strWrd = strWrd & Mid(strTxt, i, 1)
            End Select
            i = i + 1
        Loop
        If strWrd <> "" Then
            vsfItems.Rows.Count = vsfItems.Rows.Count + 1
            vsfItems.SetData(vsfItems.Rows.Count - 1, 1, strWrd)
        End If
        btnLoadItems.BackColor = Color.Gray

        With vsfItems
            For i = 1 To .Rows.Count - 1

                Application.DoEvents()
                .TopRow = i
                vsfItems.SetData(i, 2, clsSVT.getDataLongValue("Select itemID from MstItem where ItemCode = '" & .GetData(i, 1) & "'", 0, False))
                If vsfItems.GetData(i, 2) <= 0 Then
                    vsfItems.SetData(i, 0, "INVALID")
                    btnLoadItems.BackColor = Color.Red
                Else
                    vsfItems.SetData(i, 0, "Ok")
                End If
            Next
        End With
        showNormalCursor()
        txtData.Text = ""
    End Sub

    Private Sub btnLoadItemsToCPO_Click(sender As System.Object, e As System.EventArgs) Handles btnLoadItemsToCPO.Click
        With vsfItems
            For i As Int32 = 1 To .Rows.Count - 1
                If Val(.GetData(i, 2)) > 0 Then
                    strItemList = strItemList & .GetData(i, 1) & ","
                End If
            Next
        End With

    End Sub

    Private Sub frmUtlAddItem_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        strItemList = ""
    End Sub
End Class