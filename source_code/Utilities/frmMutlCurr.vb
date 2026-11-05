Imports C1.Win.C1FlexGrid

Public Class frmMutlCurr
    Dim clssvt As New svtLibrary
    Const vsfColCurr = 0
    Const vsfColNos = 1
    Const vsfColAmt = 2
    Const vsfColTot = 3

    Const vsfRTwoThousand = 1
    Const vsfRFiveHun = 2
    Const vsfRtwoHun = 3
    Const vsfROneHun = 4
    Const vsfRFifty = 5
    Const vsfRTwenty = 6
    Const vsfRTen = 7
    Const vsfRcoins = 8
    Const vsfRtotRow = 9

    Public objFrm As Object
    Public argfrm As Object
    Public strDenom As String = ""
    Dim intCurCount As Double

    Private Sub initscreen()

        Dim imgFileTwoT As Image = Nothing
        imgFileTwoT = My.Resources._2000

        Dim imgFileFiveHun As Image = Nothing
        imgFileFiveHun = My.Resources._500

        Dim imgFiletwoHun As Image = Nothing
        imgFiletwoHun = My.Resources._200

        Dim imgFileOneHun As Image = Nothing
        imgFileOneHun = My.Resources._100

        Dim imgFileFifty As Image = Nothing
        imgFileFifty = My.Resources._50

        Dim imgFileTwenty As Image = Nothing
        imgFileTwenty = My.Resources._20

        Dim imgFileten As Image = Nothing
        imgFileten = My.Resources._10
        Dim imgFilecoins As Image = Nothing
        imgFilecoins = My.Resources._1

        With vsfcur
            .Cols.Count = 0
            .Rows.Count = 0
            .Cols.Count = vsfColTot
            .Rows.Count = vsfRtotRow

            .SetData(0, vsfColCurr, "Currency")
            .SetData(0, vsfColNos, "Nos")
            .SetData(0, vsfColAmt, "Amount")


            .SetData(vsfRTwoThousand, 0, "2000")
            .SetData(vsfRFiveHun, 0, "500")
            .SetData(vsfRtwoHun, 0, "200")
            .SetData(vsfROneHun, 0, "100")
            .SetData(vsfRFifty, 0, "50")
            .SetData(vsfRTwenty, 0, "20")
            .SetData(vsfRTen, 0, "10")
            .SetData(vsfRcoins, 0, "1")

            SetFlexRowColor(vsfcur, "", vsfColCurr, vsfRTwoThousand, vsfRFiveHun, vsfRtwoHun, vsfROneHun, vsfRFifty, vsfRTwenty, vsfRTen, vsfRcoins)

            .SetCellImage(vsfRTwoThousand, 0, imgFileTwoT)
            .SetCellImage(vsfRFiveHun, 0, imgFileFiveHun)
            .SetCellImage(vsfRtwoHun, 0, imgFiletwoHun)
            .SetCellImage(vsfROneHun, 0, imgFileOneHun)
            .SetCellImage(vsfRFifty, 0, imgFileFifty)
            .SetCellImage(vsfRTwenty, 0, imgFileTwenty)
            .SetCellImage(vsfRTen, 0, imgFileten)
            .SetCellImage(vsfRcoins, 0, imgFilecoins)
            .AutoSizeRows()

            If strDenom <> "" Then
                For Each strLine As String In strDenom.Split("■")
                    Dim strCur As String = ""
                    Dim intCount As Int32 = 0
                    For Each strVal As String In strLine.Split("|")
                        If intCount = 0 Then
                            strCur = strVal
                        End If
                        Dim intRow As Int32 = vsfcur.FindRow(strCur, 0, vsfColCurr, True, True, False)
                        If intRow > -1 Then
                            Select Case intCount
                                Case 0
                                    .SetData(intRow, vsfColCurr, strVal)
                                Case 1
                                    .SetData(intRow, vsfColNos, strVal)
                                Case 2
                                    .SetData(intRow, vsfColAmt, strVal)
                            End Select
                        End If
                        intCount = intCount + 1
                    Next
                Next
            End If
        End With

    End Sub

    Private Sub frmutlCurr_Load(sender As Object, e As EventArgs) Handles Me.Load
        Me.KeyPreview = True
        initGridStyles(Me)
        fillFormProperties(Me)
        Call initFormVsfDet(Me, Me.Text, vsfcur, "Cur")
        initscreen()
    End Sub

    Private Sub vsfcur_BeforeEdit(sender As Object, e As RowColEventArgs) Handles vsfcur.BeforeEdit
        Select Case e.Col
            Case vsfColCurr
                e.Cancel = True
            Case vsfColAmt
                If Not e.Row = vsfRcoins AndAlso e.Col = vsfColAmt Then
                    e.Cancel = True
                End If
            Case vsfColNos
                If e.Row = vsfRcoins AndAlso e.Col = vsfColNos Then
                    e.Cancel = True
                End If

        End Select
        Select Case e.Row
            Case 0
                If e.Row = 0 AndAlso e.Col = vsfColNos Then
                    e.Cancel = True
                End If
        End Select
    End Sub

    Private Sub frmutlCurr_KeyUp(sender As Object, e As KeyEventArgs) Handles Me.KeyUp
        If e.KeyCode = Keys.F9 And e.Control Then
            Call initFormVsfDet(Me, Me.Text, vsfcur, "Cur")
            MsgBox("Restored Form & Grid Layout Successfully!!", MsgBoxStyle.Information, Me.Text)
        End If
        If e.KeyCode = Keys.F8 And e.Control = True Then
            Call SaveActFormVsfDet(Me, Me.Text, "Cur", vsfcur)
            MsgBox("Saved Form & Grid Layout Successfully!!", MsgBoxStyle.Information, Me.Text)
        End If
    End Sub

    Private Sub vsfcur_KeyUp(sender As Object, e As KeyEventArgs) Handles vsfcur.KeyUp
        If Keys.Tab Or Keys.Enter Then
            For i = 1 To vsfcur.Rows.Count - 2
                vsfcur.SetData(i, vsfColAmt, Val(vsfcur.GetData(i, 0)) * Val(vsfcur.GetData(i, vsfColNos)))
            Next i
            Call total()
        End If
    End Sub
    Private Sub total()

        Dim total As Double
        For i = 1 To vsfcur.Rows.Count - 1

            total += GetVal(vsfcur.GetData(i, vsfColAmt), 0)
        Next
        txtTotAMT.Text = total
    End Sub

    Private Sub btnOK_Click(sender As Object, e As EventArgs) Handles btnOK.Click
        Dim strRecords As String = ""

        With vsfcur
            If Val(txtTotAMT.Text) = 0 Then Exit Sub
            For i = 1 To .Rows.Count - 1
                If Val(.GetData(i, vsfColAmt)) > 0 Then
                    strRecords &= Val(.GetData(i, vsfColCurr)) & "|" & Val(.GetData(i, vsfColNos)) & "|" & Val(.GetData(i, vsfColAmt)) & "■"
                End If
            Next i

            If strRecords <> "" Then
                strRecords = Mid(strRecords, 1, Len(strRecords) - 1)
            End If
        End With
        argfrm.strNotesValue = strRecords
        argfrm.txtRcvdAmt.Text = Val(txtTotAMT.Text)
        Me.Close()
    End Sub

    Private Sub vsfcur_KeyPressEdit(sender As Object, e As KeyPressEditEventArgs) Handles vsfcur.KeyPressEdit
        If Not Char.IsNumber(e.KeyChar) AndAlso Not e.KeyChar = Chr(Keys.Back) Then
            e.Handled = True
        End If
        If e.KeyChar = " '" Then
        e.Handled = True
            Exit Sub
        End If

    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        vsfcur.Cols.Count = 1
        txtTotAMT.Text = ""
        intCurCount = 0
        Me.Close()
    End Sub

    Private Sub vsfcur_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles vsfcur.MouseDoubleClick

        With vsfcur
            If .ColSel = vsfColCurr Then
                If vsfcur.Row = vsfRTwoThousand Then
                    .SetData(.Row, vsfColNos, GetVal(.GetData(vsfRTwoThousand, vsfColNos), 0) + 1)
                End If
                If vsfcur.Row = vsfRFiveHun Then
                    .SetData(.Row, vsfColNos, GetVal(.GetData(vsfRFiveHun, vsfColNos), 0) + 1)
                End If
                If vsfcur.Row = vsfRtwoHun Then
                    .SetData(.Row, vsfColNos, GetVal(.GetData(vsfRtwoHun, vsfColNos), 0) + 1)
                End If
                If vsfcur.Row = vsfROneHun Then
                    .SetData(.Row, vsfColNos, GetVal(.GetData(vsfROneHun, vsfColNos), 0) + 1)
                End If
                If vsfcur.Row = vsfRFifty Then
                    .SetData(.Row, vsfColNos, GetVal(.GetData(vsfRFifty, vsfColNos), 0) + 1)
                End If
                If vsfcur.Row = vsfRTwenty Then
                    .SetData(.Row, vsfColNos, GetVal(.GetData(vsfRTwenty, vsfColNos), 0) + 1)
                End If
                If vsfcur.Row = vsfRTen Then
                    .SetData(.Row, vsfColNos, GetVal(.GetData(vsfRTen, vsfColNos), 0) + 1)
                End If
                If vsfcur.Row = vsfRcoins Then
                    .SetData(.Row, vsfColNos, GetVal(.GetData(vsfRcoins, vsfColNos), 1))
                End If
                For i = 1 To vsfcur.Rows.Count - 2
                    vsfcur.SetData(i, vsfColAmt, Val(vsfcur.GetData(i, 0)) * Val(vsfcur.GetData(i, vsfColNos)))
                Next i
            End If
            Call total()
        End With
    End Sub

    Public Sub btnClear_Click(sender As Object, e As EventArgs) Handles btnClear.Click

        vsfcur.SetData(vsfRTwoThousand, vsfColNos, 0)
        vsfcur.SetData(vsfRTwoThousand, vsfColAmt, 0)

        vsfcur.SetData(vsfRFiveHun, vsfColNos, 0)
        vsfcur.SetData(vsfRFiveHun, vsfColAmt, 0)

        vsfcur.SetData(vsfRtwoHun, vsfColNos, 0)
        vsfcur.SetData(vsfRtwoHun, vsfColAmt, 0)

        vsfcur.SetData(vsfROneHun, vsfColNos, 0)
        vsfcur.SetData(vsfROneHun, vsfColAmt, 0)

        vsfcur.SetData(vsfRFifty, vsfColNos, 0)
        vsfcur.SetData(vsfRFifty, vsfColAmt, 0)

        vsfcur.SetData(vsfRTwenty, vsfColNos, 0)
        vsfcur.SetData(vsfRTwenty, vsfColAmt, 0)

        vsfcur.SetData(vsfRTen, vsfColNos, 0)
        vsfcur.SetData(vsfRTen, vsfColAmt, 0)

        vsfcur.SetData(vsfRcoins, vsfColNos, 0)
        vsfcur.SetData(vsfRcoins, vsfColAmt, 0)

        Call total()
    End Sub
End Class