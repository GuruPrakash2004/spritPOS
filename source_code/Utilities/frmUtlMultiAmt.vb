Imports C1.Win.C1FlexGrid

Public Class frmUtlMultiAmt
    Dim clsSVT As New svtLibrary

    Public strList As String
    Public intDocID As Int32
    Public intTrnID As Int32
    Public strFldCode As String

    Const vDetSlNo = 0
    Const vDetParticular = 1
    Const vDetAmt = 2
    Const vDetTotCol = 3

    Public argObj As Object

    Private Sub frmUtlMultiAmt_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Call initFormSize(Me, Me.Text, True, 421, 356, 0, 0)
        Call fillFormProperties(Me)
        Call initGridStyles(Me)
        Call initScreen()
        Call initFormVsfDet(Me, Me.Text, vsfDet, "DET")
        KeyPreview = True
    End Sub

    Private Sub initScreen()
        With vsfDet
            .Rows.Count = 1
            .Rows.Fixed = 1
            .Cols.Fixed = 1
            .Cols.Count = vDetTotCol

            .SetData(0, vDetSlNo, "SlNo")
            .SetData(0, vDetParticular, "Particulars")
            .SetData(0, vDetAmt, "Amount")
            .Cols(vDetAmt).DataType = GetType(Double)
            Call SetColAlign(vsfDet, svtAlign.Left, vDetAmt)
            Call SetFlexColor(vsfDet, "", vDetSlNo)
            Call SetFlexColor(vsfDet, "M", vDetParticular, vDetAmt)


            If strList <> "" Then
                For Each strRecord As String In strList.Split("■")
                    If strRecord <> "" Then
                        .Rows.Count = .Rows.Count + 1
                        Dim intCol As Int32 = 1
                        For Each strVal As String In strRecord.Split("|")
                            Select Case intCol
                                Case 1
                                    .SetData(.Rows.Count - 1, vDetSlNo, strVal)
                                Case 2
                                    .SetData(.Rows.Count - 1, vDetParticular, strVal)
                                Case 3
                                    .SetData(.Rows.Count - 1, vDetAmt, strVal)
                            End Select
                            intCol = intCol + 1
                        Next
                    End If
                Next
            End If
            .Rows.Count = .Rows.Count + 1
            Call ReArrangeSlNo(vsfDet, vDetSlNo)
        End With
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub frmUtlMultiAmt_KeyUp(sender As Object, e As KeyEventArgs) Handles Me.KeyUp
        If e.KeyCode = Keys.F9 And e.Control Then
            Call initFormSize(Me, Me.Text, True, 421, 356, 0, 0)
            Call initFormVsfDet(Me, Me.Text, vsfDet, "DET")
            MsgBox("Restored Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If

        If e.KeyCode = Keys.F8 And e.Control = True Then
            Call SaveActFormSettings(Me, Me.Text)
            Call SaveActFormVsfDet(Me, Me.Text, "DET", vsfDet)
            MsgBox("Saved Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If

        If e.KeyCode = Keys.R And e.Alt Then
            btnRemove_Click(sender, e)
        End If

        If e.KeyCode = Keys.E AndAlso e.Alt Then
            Call ExportToXL(vsfDet)
        End If
    End Sub

    Private Sub vsfDet_KeyPressEdit(sender As Object, e As KeyPressEditEventArgs) Handles vsfDet.KeyPressEdit
        Select Case e.Col
            Case vDetAmt
                If Not KeyValidate("Decimal", e.KeyChar) Then
                    e.Handled = True
                End If
            Case vDetParticular
                If Not KeyValidate("String", e.KeyChar) Then
                    e.Handled = True
                End If
            Case Else
                e.Handled = True
        End Select
    End Sub

    Private Sub vsfDet_KeyUp(sender As Object, e As KeyEventArgs) Handles vsfDet.KeyUp
        If e.KeyCode = Keys.Tab AndAlso vsfDet.Col = vDetAmt AndAlso GetVal(vsfDet.GetData(vsfDet.Row, vDetParticular), "") <> "" AndAlso vsfDet.Row = vsfDet.Rows.Count - 1 Then
            vsfDet.Rows.Count = vsfDet.Rows.Count + 1
            Call ReArrangeSlNo(vsfDet, vDetSlNo)
        End If
    End Sub

    Private Sub btnOK_Click(sender As Object, e As EventArgs) Handles btnOK.Click
        With vsfDet

            Call ReArrangeSlNo(vsfDet, vDetSlNo)
            Call RemoveEmptVSFRows(vsfDet, 0, vDetParticular)

            Dim dblTotAmt As Double = 0
            Dim strRecords As String = ""
            For i As Int32 = 1 To vsfDet.Rows.Count - 1
                dblTotAmt = dblTotAmt + Val(vsfDet.GetData(i, vDetAmt))
                strRecords &= Val(.GetData(i, vDetSlNo)) & "|"
                strRecords &= Trim(.GetData(i, vDetParticular)) & "|"
                strRecords &= Val(.GetData(i, vDetAmt)) & "■"
            Next
            argObj.strMultiAmtVal = strRecords
            argObj.dblMultiAmtVal = dblTotAmt
            argObj.blnMultiAmtVal = True
        End With

        Me.Dispose()
    End Sub

    Private Sub btnRemove_Click(sender As Object, e As EventArgs) Handles btnRemove.Click
        If vsfDet.Rows.Count <= 1 Then Exit Sub
        vsfDet.Rows.Remove(vsfDet.Row)
        If vsfDet.Rows.Count = 1 Then vsfDet.Rows.Count = 2
        Call ReArrangeSlNo(vsfDet, vDetSlNo)
    End Sub

    Private Sub btnExportToXL_Click(sender As Object, e As EventArgs) Handles btnExportToXL.Click
        Call ExportToXL(vsfDet)
    End Sub

    Private Sub vsfDet_BeforeEdit(sender As Object, e As RowColEventArgs) Handles vsfDet.BeforeEdit

        Select Case e.Col
            Case vDetParticular
                vsfDet.Cols(vDetParticular).ComboList = ""
                vsfDet.Cols(vDetParticular).ComboList = GenVSfComboStringCond("Particulars", "TrnMultiAmt", "Order By 1")
        End Select

    End Sub

End Class