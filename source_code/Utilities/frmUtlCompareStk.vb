Imports Microsoft.Office.Interop
Imports C1.Win.C1FlexGrid
Public Class frmUtlCompareStk

    Const vH1Slno = 0
    Const vH1ItemName = 1
    Const vH1ClQty = 2
    Const vH1ClVal = 3
    Const vH1TotCols = 4

    Const vH2Slno = 0
    Const vH2ItemName = 1
    Const vH2ClQty = 2
    Const vH2ClVal = 3
    Const vH2Found = 4
    Const vH2TotCols = 5

    Const vCSlno = 0
    Const vCItemName1 = 1
    Const vCClQty1 = 2
    Const vCClVal1 = 3
    Const vCItemName2 = 4
    Const vCClQty2 = 5
    Const vCClVal2 = 6
    Const vCClQtyDiff = 7
    Const vCClValDiff = 8
    Const vCError = 9
    Const vCTotCols = 10

    Private Sub initScreen()
        With vsfHdr1
            .Rows.Count = 1
            .Cols.Count = vH1TotCols
            .SetData(0, vH1Slno, "SlNo")
            .SetData(0, vH1ItemName, "Item Name")
            .SetData(0, vH1ClQty, "Closing Qty")
            .SetData(0, vH1ClVal, "Closing Value")
        End With
        SetFlexColor(vsfHdr1, "", vH1Slno, vH1ItemName, vH1ClQty, vH1ClVal)
        Call initFormVsfDet(Me, Me.Text, vsfHdr1, "HDR1")

        With vsfHdr2
            .Rows.Count = 1
            .Cols.Count = vH2TotCols
            .SetData(0, vH2Slno, "SlNo")
            .SetData(0, vH2ItemName, "Item Name")
            .SetData(0, vH2ClQty, "Closing Qty")
            .SetData(0, vH2ClVal, "Closing Value")
        End With
        SetFlexColor(vsfHdr2, "", vH2Slno, vH2ItemName, vH2ClQty, vH2ClVal)
        Call initFormVsfDet(Me, Me.Text, vsfHdr2, "HDR2")

        With vsfCompare
            .Rows.Count = 1
            .Cols.Count = vCTotCols
            .SetData(0, vCSlno, "SlNo")
            .SetData(0, vCItemName1, "Item Name")
            .SetData(0, vCClQty1, "Closing Qty")
            .SetData(0, vCClVal1, "Closing Value")
            .SetData(0, vCItemName2, "Item Name")
            .SetData(0, vCClQty2, "Closing Qty")
            .SetData(0, vCClVal2, "Closing Value")
            .SetData(0, vCClQtyDiff, "Qty Diff")
            .SetData(0, vCClValDiff, "Value Diff")
            .SetData(0, vCError, "Error")
        End With
        SetFlexColor(vsfCompare, "", vCSlno, vCItemName1, vCClQty1, vCClVal1, vCItemName2, vCClQty2, vCClVal2, vCClQtyDiff, vCClValDiff, vCError)
        Call initFormVsfDet(Me, Me.Text, vsfCompare, "COMPARE")
    End Sub

    Private Sub btnImpXL1_Click(sender As Object, e As EventArgs) Handles btnImpXL1.Click
        Cdlg.DefaultExt = "xlsx"
        Cdlg.Filter = "Excel File (*.xlsx)|*.xlsx"
        Cdlg.FileName = "*.xlsx"

        If Cdlg.ShowDialog() <> DialogResult.OK Then
            Return
            Exit Sub
        End If

        showWaitCursor()
        importFromXL(Cdlg.FileName, vsfHdr1)
        showNormalCursor()
    End Sub

    Private Sub importFromXL(ByVal argFileName As String, ByVal argVSF As C1.Win.C1FlexGrid.C1FlexGrid)
        Dim xlapp As New Excel.Application
        Dim i As Integer, intClQty As Integer, intClVal As Double
        Dim strItemName As String

        xlapp.Workbooks.Open(argFileName, , True)
        argVSF.Rows.Count = 1
        With xlapp.Application

            i = 2
            Try

                Do While .Range("A" & i).Value <> ""
                    strItemName = .Range("A" & i).Value
                    intClQty = .Range("B" & i).Value
                    intClVal = .Range("C" & i).Value

                    With argVSF
                        .Rows.Count = .Rows.Count + 1
                        .SetData(.Rows.Count - 1, vH1Slno, .Rows.Count - 1)
                        .SetData(.Rows.Count - 1, vH1ItemName, strItemName)
                        .SetData(.Rows.Count - 1, vH1ClQty, intClQty)
                        .SetData(.Rows.Count - 1, vH1ClVal, intClVal)
                    End With
                    i = i + 1
                Loop

                .Workbooks(1).Close(SaveChanges:=False)
            Catch ex As Exception
                MsgBox("Cannot Import From Excel." & vbCrLf & ex.Message, MsgBoxStyle.Critical, Me.Text)
            End Try
        End With

        Exit Sub
    End Sub

    Private Sub btnImpXL2_Click(sender As Object, e As EventArgs) Handles btnImpXL2.Click
        Cdlg.DefaultExt = "xlsx"
        Cdlg.Filter = "Excel File (*.xlsx)|*.xlsx"
        Cdlg.FileName = "*.xlsx"

        If Cdlg.ShowDialog() <> DialogResult.OK Then
            Return
            Exit Sub
        End If

        showWaitCursor()
        importFromXL(Cdlg.FileName, vsfHdr2)
        showNormalCursor()
    End Sub

    Private Sub btnImpClose_Click(sender As Object, e As EventArgs) Handles btnImpClose.Click
        frmMain.CloseSelectedTabPage(Me)
    End Sub

    Private Sub frmUtlCompareStk_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.KeyPreview = True

        fillFormProperties(Me)
        initGridStyles(Me)
        initScreen()
        initFormVsfDet(Me, Me.Text, vsfHdr1, "HDR1")
        initFormVsfDet(Me, Me.Text, vsfHdr2, "HDR2")
        initFormVsfDet(Me, Me.Text, vsfCompare, "COMPARE")
    End Sub

    Private Sub vsfHdr1_BeforeEdit(sender As Object, e As RowColEventArgs) Handles vsfHdr1.BeforeEdit
        e.Cancel = True
    End Sub

    Private Sub vsfHdr2_BeforeEdit(sender As Object, e As RowColEventArgs) Handles vsfHdr2.BeforeEdit
        e.Cancel = True
    End Sub

    Private Sub vsfCompare_BeforeEdit(sender As Object, e As RowColEventArgs) Handles vsfCompare.BeforeEdit
        e.Cancel = True
    End Sub

    Private Sub btnCompare_Click(sender As Object, e As EventArgs) Handles btnCompare.Click
        Dim intClQty As Integer, intClVal As Double
        Dim strItemName1 As String
        Dim blnFound As Boolean

        vsfCompare.Rows.Count = 1

        showWaitCursor()

        With vsfHdr1
            For i As Int32 = 1 To .Rows.Count - 1
                strItemName1 = .GetData(i, vH1ItemName)
                intClQty = .GetData(i, vH1ClQty)
                intClVal = .GetData(i, vH1ClVal)
                blnfound = False
                For x As Int32 = 1 To vsfHdr2.Rows.Count - 1
                    If strItemName1 = vsfHdr2.GetData(x, vH2ItemName) Then
                        vsfHdr2.SetData(x, vH2Found, "Y")
                        vsfCompare.Rows.Count = vsfCompare.Rows.Count + 1
                        vsfCompare.SetData(vsfCompare.Rows.Count - 1, vCSlno, vsfCompare.Rows.Count - 1)
                        vsfCompare.SetData(vsfCompare.Rows.Count - 1, vCItemName1, strItemName1)
                        vsfCompare.SetData(vsfCompare.Rows.Count - 1, vCClQty1, intClQty)
                        vsfCompare.SetData(vsfCompare.Rows.Count - 1, vCClVal1, intClVal)
                        vsfCompare.SetData(vsfCompare.Rows.Count - 1, vCItemName2, vsfHdr2.GetData(x, vH2ItemName))
                        vsfCompare.SetData(vsfCompare.Rows.Count - 1, vCClQty2, vsfHdr2.GetData(x, vH2ClQty))
                        vsfCompare.SetData(vsfCompare.Rows.Count - 1, vCClVal2, vsfHdr2.GetData(x, vH2ClVal))
                        blnFound = True
                        Exit For
                    End If
                Next
                If Not blnFound Then
                    vsfCompare.Rows.Count = vsfCompare.Rows.Count + 1
                    vsfCompare.SetData(vsfCompare.Rows.Count - 1, vCSlno, vsfCompare.Rows.Count - 1)
                    vsfCompare.SetData(vsfCompare.Rows.Count - 1, vCItemName1, strItemName1)
                    vsfCompare.SetData(vsfCompare.Rows.Count - 1, vCClQty1, intClQty)
                    vsfCompare.SetData(vsfCompare.Rows.Count - 1, vCClVal1, intClVal)
                End If
            Next
            For i As Int32 = 1 To vsfHdr2.Rows.Count - 1
                If vsfHdr2.GetData(i, vH2Found) <> "Y" Then
                    vsfCompare.Rows.Count = vsfCompare.Rows.Count + 1
                    vsfCompare.SetData(vsfCompare.Rows.Count - 1, vCSlno, vsfCompare.Rows.Count - 1)
                    vsfCompare.SetData(vsfCompare.Rows.Count - 1, vCItemName1, "")
                    vsfCompare.SetData(vsfCompare.Rows.Count - 1, vCClQty1, 0)
                    vsfCompare.SetData(vsfCompare.Rows.Count - 1, vCClVal1, 0)
                    vsfCompare.SetData(vsfCompare.Rows.Count - 1, vCItemName2, vsfHdr2.GetData(i, vH2ItemName))
                    vsfCompare.SetData(vsfCompare.Rows.Count - 1, vCClQty2, vsfHdr2.GetData(i, vH2ClQty))
                    vsfCompare.SetData(vsfCompare.Rows.Count - 1, vCClVal2, vsfHdr2.GetData(i, vH2ClVal))
                    blnFound = True
                End If
            Next
        End With

        Dim strerr As String
        strerr = ""
        With vsfCompare
            For i As Int32 = 1 To vsfCompare.Rows.Count - 1
                strerr = ""
                If .GetData(i, vCItemName1) <> .GetData(i, vCItemName2) Then
                    strerr = strerr & " " & "Item Not found"
                    GoTo NXT
                End If
                If .GetData(i, vCClQty1) <> .GetData(i, vCClQty2) Then
                    If .GetData(i, vCClQty1) > .GetData(i, vCClQty2) Then
                        .SetData(i, vCClQtyDiff, .GetData(i, vCClQty1) - .GetData(i, vCClQty2))
                    Else
                        .SetData(i, vCClQtyDiff, .GetData(i, vCClQty2) - .GetData(i, vCClQty1))
                    End If
                    strerr = strerr & " " & "Qty Mismatch"
                End If
                If .GetData(i, vCClVal1) <> .GetData(i, vCClVal2) Then
                    If .GetData(i, vCClVal1) > .GetData(i, vCClVal2) Then
                        .SetData(i, vCClValDiff, .GetData(i, vCClVal1) - .GetData(i, vCClVal2))
                    Else
                        .SetData(i, vCClValDiff, .GetData(i, vCClVal2) - .GetData(i, vCClVal1))
                    End If
                    strerr = strerr & " " & "Value Mismatch"
                End If
NXT:
                .SetData(i, vCError, strerr)
            Next
        End With

        Call RemoveEmptVSFRows(vsfCompare, 1, vCError)
        ReArrangeSlNo(vsfCompare, vCSlno)
        showNormalCursor()
    End Sub


    Private Sub vsfHdr1_KeyUp(sender As Object, e As KeyEventArgs) Handles vsfHdr1.KeyUp
        If e.KeyCode = Keys.F9 And e.Control = True Then
            Call initFormVsfDet(Me, Me.Text, vsfHdr1, "HDR1")
            MsgBox("Restored Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
        If e.KeyCode = Keys.F8 And e.Control = True Then
            Call SaveActFormVsfDet(Me, Me.Text, "HDR1", vsfHdr1)
            MsgBox("Saved Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
    End Sub

    Private Sub vsfHdr2_KeyUp(sender As Object, e As KeyEventArgs) Handles vsfHdr2.KeyUp
        If e.KeyCode = Keys.F9 And e.Control = True Then
            Call initFormVsfDet(Me, Me.Text, vsfHdr2, "HDR2")
            MsgBox("Restored Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
        If e.KeyCode = Keys.F8 And e.Control = True Then
            Call SaveActFormVsfDet(Me, Me.Text, "HDR2", vsfHdr2)
            MsgBox("Saved Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
    End Sub

    Private Sub vsfCompare_KeyUp(sender As Object, e As KeyEventArgs) Handles vsfCompare.KeyUp
        If e.KeyCode = Keys.F9 And e.Control = True Then
            Call initFormVsfDet(Me, Me.Text, vsfCompare, "COMPARE")
            MsgBox("Restored Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
        If e.KeyCode = Keys.F8 And e.Control = True Then
            Call SaveActFormVsfDet(Me, Me.Text, "COMPARE", vsfCompare)
            MsgBox("Saved Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
    End Sub

    Private Sub btnExportToXL_Click(sender As Object, e As EventArgs) Handles btnExportToXL.Click
        ExportToXL(vsfCompare)
    End Sub
End Class