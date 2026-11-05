Imports System.Data.oledb
Imports C1.Win.C1FlexGrid
Imports System.Reflection

Public Class frmUtlAddRptItems
    Dim clsSVT As New svtLibrary

    Public frm As Object
    Public argItemCode As Integer
    Public Mode As String
    Public strType As String

    Const vSlno = 0
    Const vItemID = 1
    Const vItemCode = 2
    Const vShortName = 3
    Const vValid = 4
    Const vUOM = 5
    Const vTotal = 6

    Private Sub btnLoadItemsToCPO_Click(sender As System.Object, e As System.EventArgs) Handles btnLoadItems.Click
        Dim i As Integer = 0

        With vsfItems
            For i = 1 To .Rows.Count - 1
                If Val(.GetData(i, vItemID)) > 0 Then
                    If strType = "I" Then
                        Call frm.fillItemDtls(.GetData(i, vItemCode))
                    Else
                        Call frm.fillPkgDtls(.GetData(i, vItemCode))
                    End If
                End If
            Next
        End With

        Me.Close()
    End Sub

    Private Sub btnLoadItems_Click(sender As System.Object, e As System.EventArgs) Handles btnItemValidation.Click
        Dim strTxt As String
        Dim i As Integer
        Dim strWrd As String
        Dim rs As oledbDataReader
        Dim strSQL As String

        i = 1
        vsfItems.Rows.Count = 1
        vsfItems.Cols.Count = vTotal
        vsfItems.Cols(vSlno).Width = 50
        vsfItems.Cols(vItemCode).Width = 150
        vsfItems.ExtendLastCol = True
        vsfItems.Cols(vItemID).Visible = False
        vsfItems.Cols(vUOM).Visible = False
        showWaitCursor()
        strWrd = ""
        strTxt = Replace(txtData.Text, "'", "`")
        Do While i <= Len(strTxt)
            Select Case Mid(strTxt, i, 1)
                Case Chr(10), Chr(13)
                    If strWrd <> "" Then
                        vsfItems.Rows.Count = vsfItems.Rows.Count + 1
                        vsfItems.SetData(vsfItems.Rows.Count - 1, vItemCode, strWrd)
                    End If
                    strWrd = ""
                Case Else
                    strWrd = strWrd & Mid(strTxt, i, 1)
            End Select
            i = i + 1
        Loop
        If strWrd <> "" Then
            vsfItems.Rows.Count = vsfItems.Rows.Count + 1
            vsfItems.SetData(vsfItems.Rows.Count - 1, vSlno, vsfItems.Rows.Count - 1)
            vsfItems.SetData(vsfItems.Rows.Count - 1, vItemCode, strWrd)
        End If
        btnItemValidation.BackColor = Color.Gray

        With vsfItems
            For i = 1 To .Rows.Count - 1

                Application.DoEvents()
                .TopRow = i

                If strType = "P" Then
                    strSQL = "Select PKGID, PKGNAME from MstPKG"
                    strSQL &= " Where PKGCode = '" & .GetData(i, vItemCode) & "'"
                    strSQL &= " Or PKGName = '" & .GetData(i, vItemCode) & "'"

                    rs = clsSVT.getDataReaderFromSQL(strSQL)

                    If Not rs.HasRows Then Exit Sub

                    rs.Read()

                    vsfItems.SetData(i, vItemID, Val(rs!PKGID))
                    vsfItems.SetData(i, vShortName, GetVal(rs!PKGNAME))

                    rs.Close()
                Else

                    strSQL = "Select ItemID, ShortName from MstItem "
                    strSQL &= " Where ItemName = '" & .GetData(i, vItemCode) & "'"
                    strSQL &= " Or ItemCode = '" & .GetData(i, vItemCode) & "'"
                    strSQL &= " Or ShortName = '" & .GetData(i, vItemCode) & "'"
                    strSQL &= " Or HotKey = '" & .GetData(i, vItemCode) & "'"

                    rs = clsSVT.getDataReaderFromSQL(strSQL)

                    If Not rs.HasRows Then Exit Sub

                    rs.Read()

                    vsfItems.SetData(i, vItemID, Val(rs!ITEMID))
                    vsfItems.SetData(i, vShortName, GetVal(rs!ShortName))
                    rs.Close()
                End If

                If vsfItems.GetData(i, vItemID) <= 0 Then
                    'vsfItems.SetData(i, vValid, "INVALID")
                    SetCellFlexImage(vsfItems, i, vValid, "cancel.png", False, C1.Win.C1FlexGrid.ImageAlignEnum.CenterCenter)
                    btnItemValidation.BackColor = Color.Red

                Else
                    'vsfItems.SetData(i, vValid, "Ok")
                    SetCellFlexImage(vsfItems, i, vValid, "small_tick.png", False, C1.Win.C1FlexGrid.ImageAlignEnum.CenterCenter)
                    .SetData(i, vUOM, clsSVT.getDataStringValue("Select UOMCode from MstUOM Where UOMID = (Select UOMID from MstItem Where ItemID=" & vsfItems.GetData(i, vItemID) & ")", "", False))
                End If
            Next
        End With
        ReArrangeSlNo(vsfItems, vSlno)
        showNormalCursor()
        txtData.Text = ""
    End Sub

    Private Sub frmUtlAddRptItems_KeyUp(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyUp
        If e.KeyCode = Keys.F9 And e.Control Then
            Call initFormSize(Me, Me.Text, True, 747, 455, 0, 0)
            MsgBox("Restored Form Layout Successfully!!", vbInformation, Me.Text)
        End If
        If e.KeyCode = Keys.F8 And e.Control = True Then
            Call SaveActFormSettings(Me, Me.Text)
            MsgBox("Saved Form Layout Successfully!!", vbInformation, Me.Text)
        End If
    End Sub

    Private Sub frmUtlAddRptItems_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        Call initFormSize(Me, Me.Text, False, 747, 455, 0, 0)
        With vsfItems
            .Cols.Count = vTotal
            .Rows.Count = 1
            .SetData(0, vSlno, "SLNo")
            If strType = "I" Then
                .SetData(0, vItemCode, "Item Code")
                .SetData(0, vShortName, "Short Name")
            Else
                .SetData(0, vItemCode, "PKG Code")
                .SetData(0, vShortName, "PKG Name")
            End If
            .SetData(0, vValid, "Valid/Invalid")
            SetHideCol(vsfItems, True, vItemCode, vItemID, vUOM)
        End With
    End Sub

    Private Sub txtData_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtData.TextChanged
        If Trim(txtData.Text) = "" Then
            btnItemValidation.Enabled = False
            btnItemValidation.ForeColor = Color.Black
        Else
            btnItemValidation.Enabled = True
            Timer1.Enabled = True
        End If
        txtData.ScrollBars = ScrollBars.None
        TextBoxSizeChanged(txtData, e)
    End Sub

    Private Sub vsfItems_BeforeEdit(sender As Object, e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfItems.BeforeEdit
        e.Cancel = True
    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        btnItemValidation.ForeColor = Color.Aqua
        Timer1.Enabled = False
        Timer2.Enabled = True
    End Sub

    Private Sub Timer2_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer2.Tick
        btnItemValidation.ForeColor = Color.Red
        Timer2.Enabled = False
        Timer1.Enabled = True
    End Sub

    Private Sub vsfItems_MouseClick(sender As Object, e As MouseEventArgs) Handles vsfItems.MouseClick
        Dim ht = vsfItems.HitTest

        With vsfItems
            If ht.Row < 0 Then Exit Sub
            If .GetData(ht.Row, vItemCode) = "" Then Exit Sub
            If ht.Column <> vValid Then Exit Sub
            ' If MsgBox("Do you want to delete this Contact -- " & .GetData(ht.Row, vTo) & " ?", vbYesNo + vbDefaultButton2 + vbCritical, "Remove Contact") = MsgBoxResult.No Then Exit Sub
            .RemoveItem(ht.Row)
            Call ReArrangeSlNo(vsfItems, vSlno)
        End With

    End Sub

    Private Sub Delete_Click(sender As Object, e As EventArgs) Handles Delete.Click
        Dim i As Integer, blnBlank As Boolean, strCompare As String
        Dim argMinRowsToKeep As Integer
        With vsfItems
Repeat:
            For i = 1 To (.Rows.Count - 1)
                blnBlank = True
                strCompare = ""

                If strCompare <> "" Then blnBlank = False
                If blnBlank Then
                    .RemoveItem(i)
                    GoTo Repeat
                End If
            Next i
            'If All rows will be removed if want to add some blank rows
            If vsfItems.Rows.Count = 1 And argMinRowsToKeep <> 0 Then vsfItems.Rows.Count = vsfItems.Rows.Count + argMinRowsToKeep
        End With
    End Sub
End Class