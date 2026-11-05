Imports System.IO
Imports C1.Win.C1FlexGrid
Public Class frmUtlGColourSettings

    Dim clsSVT As New svtLibrary
    Const sampItemCode = 0
    Const sampItemName = 1
    Const sampBrand = 2
    Const sampSubGroup = 3

    Private Sub btnDefault_Click_1(sender As System.Object, e As System.EventArgs) Handles btnDefault.Click
        If MsgBox("Are you sure you want to reset the grid colours to default? ", vbCritical + MsgBoxStyle.YesNo, "Confirmation") = MsgBoxResult.Yes Then
            defaultGridStyle()
        End If
    End Sub
    Private Sub fillGridColours()
        Dim str As String = getGsettings("COMPANY", "GridClrs", "")
        If str = "" Then Exit Sub
        Dim strArr() As String = str.Split(New Char() {","c})

        'Normal
        lblNormalFore.BackColor = Color.FromArgb(Convert.ToInt32(strArr(0)))
        lblNormalBack.BackColor = Color.FromArgb(Convert.ToInt32(strArr(1)))

        'Alternate
        lblAlternateFore.BackColor = Color.FromArgb(Convert.ToInt32(strArr(2)))
        lblAlternateBack.BackColor = Color.FromArgb(Convert.ToInt32(strArr(3)))

        'Fixed
        lblFixedFore.BackColor = Color.FromArgb(Convert.ToInt32(strArr(4)))
        lblFixedBack.BackColor = Color.FromArgb(Convert.ToInt32(strArr(5)))

        'Highlight
        lblHighlightFore.BackColor = Color.FromArgb(Convert.ToInt32(strArr(6)))
        lblHighlightBack.BackColor = Color.FromArgb(Convert.ToInt32(strArr(7)))

        'Focus
        lblFocusFore.BackColor = Color.FromArgb(Convert.ToInt32(strArr(8)))
        lblFocusBack.BackColor = Color.FromArgb(Convert.ToInt32(strArr(9)))


        'Empty Area
        lblEmptyAreaBack.BackColor = Color.FromArgb(Convert.ToInt32(strArr(10)))

        initSampGridColours()

        initTlTipColours()
    End Sub

    Private Sub initTlTipColours()
        ' Call resetBtnClrs()

        Dim strgrp As String = getGsettings("ToolTip", "ToolTipClr", "")
        If strgrp <> "" Then
            Dim strArr() As String = strgrp.Split(New Char() {","c})
            'btnGroup
          

            lblTlTipFore.BackColor = Color.FromArgb(Convert.ToInt32(strArr(0)))
            lblToolTip.ForeColor = lblTlTipFore.BackColor

            lblTlTipBack.BackColor = Color.FromArgb(Convert.ToInt32(strArr(1)))
            lblToolTip.BackColor = lblTlTipBack.BackColor
            pnlTlTip.BackColor = lblTlTipBack.BackColor
        End If

    End Sub


    Private Sub initSampGridColours()
        'Normal
        Dim cs As CellStyle = sampleGrid.Styles.Normal
        cs.ForeColor = lblNormalFore.ForeColor
        cs.BackColor = lblNormalBack.BackColor

        'Alternate
        cs = sampleGrid.Styles.Alternate
        cs.ForeColor = lblAlternateFore.ForeColor
        cs.BackColor = lblAlternateBack.BackColor

        'Fixed
        cs = sampleGrid.Styles.Fixed
        cs.ForeColor = lblFixedFore.ForeColor
        cs.BackColor = lblFixedBack.BackColor

        'Highlight
        cs = sampleGrid.Styles.Highlight
        cs.ForeColor = lblHighlightFore.ForeColor
        cs.BackColor = lblHighlightBack.BackColor

        'Focus
        cs = sampleGrid.Styles.Focus
        cs.ForeColor = lblFocusFore.ForeColor
        cs.BackColor = lblFocusBack.BackColor

        'Empty Area
        cs = sampleGrid.Styles.EmptyArea
        cs.BackColor = lblEmptyAreaBack.BackColor

        'Tool Tip
        cs = sampleGrid.Styles.Focus
        cs.ForeColor = lblTlTipFore.ForeColor
        cs.BackColor = lblTlTipBack.BackColor
    End Sub
    Private Sub defaultGridStyle()
        'Normal
        Dim cs As CellStyle = sampleGrid.Styles.Normal
        cs.ForeColor = Color.Black
        lblNormalFore.BackColor = Color.Black
        cs.BackColor = Color.White
        lblNormalBack.BackColor = Color.White

        'Alternate
        cs = sampleGrid.Styles.Alternate
        cs.ForeColor = Color.Black
        lblAlternateFore.BackColor = Color.Black
        cs.BackColor = Color.FromArgb(221, 235, 247)
        lblAlternateBack.BackColor = Color.FromArgb(221, 235, 247)

        'Fixed
        cs = sampleGrid.Styles.Fixed
        cs.ForeColor = Color.Black
        lblFixedFore.BackColor = Color.Black
        cs.BackColor = Color.FromKnownColor(KnownColor.Control)
        lblFixedBack.BackColor = Color.FromKnownColor(KnownColor.Control)

        'Highlight
        cs = sampleGrid.Styles.Highlight
        cs.ForeColor = Color.White
        lblHighlightFore.BackColor = Color.White
        cs.BackColor = Color.FromArgb(91, 155, 213)
        lblHighlightBack.BackColor = Color.FromArgb(91, 155, 213)

        'Focus
        cs = sampleGrid.Styles.Focus
        cs.ForeColor = Color.Black
        lblFocusFore.BackColor = Color.Black
        cs.BackColor = Color.FromArgb(91, 155, 213)
        lblFocusBack.BackColor = Color.FromArgb(91, 155, 213)

        'Empty Area
        cs = sampleGrid.Styles.EmptyArea
        cs.BackColor = Color.Gray
        lblEmptyAreaBack.BackColor = Color.Gray

        'Tool Tip
        lblToolTip.ForeColor = Color.White
        lblTlTipFore.BackColor = Color.White
        lblToolTip.BackColor = Color.FromArgb(52, 68, 88)
        lblTlTipBack.BackColor = Color.FromArgb(52, 68, 88)
        pnlTlTip.BackColor = lblToolTip.BackColor

    End Sub

    'Private Sub btnGridClrSave_Click(sender As System.Object, e As System.EventArgs) Handles btnGridClrSave.Click
    '    Dim str As String, strSQL As String
    '    Try
    '        'Normal
    '        str = lblNormalFore.BackColor.ToArgb & ", " & lblNormalBack.BackColor.ToArgb & ", "
    '        'Alternate
    '        str = str & lblAlternateFore.BackColor.ToArgb & ", " & lblAlternateBack.BackColor.ToArgb & ", "
    '        'Fixed
    '        str = str & lblFixedFore.BackColor.ToArgb & ", " & lblFixedBack.BackColor.ToArgb & ", "
    '        'Highlight
    '        str = str & lblHighlightFore.BackColor.ToArgb & ", " & lblHighlightBack.BackColor.ToArgb & ", "
    '        'Focus
    '        str = str & lblFocusFore.BackColor.ToArgb & ", " & lblFocusBack.BackColor.ToArgb & ","
    '        'Empty Area
    '        str = str & lblEmptyAreaBack.BackColor.ToArgb

    '        strSQL = "Update UtlGSettings set SetValue = '" & str & "' where SetName= 'GridClrs' and SetGroup = 'COMPANY'"

    '        clsSVT.execSQLStatement(strSQL)

    '        'Tool Tip
    '        str = lblTlTipFore.BackColor.ToArgb & ", " & lblTlTipBack.BackColor.ToArgb
    '        strSQL = "Update UtlGSettings set SetValue = '" & str & "' where SetName= 'ToolTipClr' and SetGroup = 'ToolTip'"

    '        clsSVT.execSQLStatement(strSQL)

    '        MsgBox("Updated Successfully!", vbInformation, Me.Text)
    '    Catch ex As Exception
    '        MsgBox("Update failed! Please check for valid data and retry.", vbCritical, Me.Text)
    '        Exit Sub
    '    End Try
    'End Sub

    'Private Sub btnClose_Click(sender As System.Object, e As System.EventArgs) Handles btnClose.Click
    '    Me.Close()
    'End Sub
    Private Sub Initilization()
        With sampleGrid
            .Rows.Count = 1
            .Cols.Count = 5
            .ExtendLastCol = True

            .SetData(0, sampItemCode, "Item Code")
            .SetData(0, sampItemName, "Item Name")
            .SetData(0, sampBrand, "Brand")
            .SetData(0, sampSubGroup, "Sub Group")
        End With
        sampleGrid.AutoSizeCols()
    End Sub
    Private Sub frmUtlGCSettings_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        Initilization()
        Dim strSQL As String
        strSQL = "SELECT TOP 10 DocType as ItemCode, '' as ItemName, HdrTblName as Brand, docid as subgroupID FROM Utltransactions "
        Dim rsSamp As oledb.oledbDataReader
        rsSamp = clsSVT.getDataReaderFromSQL(strSQL)

        With rsSamp
            Do While .Read()
                sampleGrid.Rows.Count += 1
                With sampleGrid
                    .SetData(.Rows.Count - 1, sampItemName, rsSamp!ItemName)
                    .SetData(.Rows.Count - 1, sampItemCode, rsSamp!ItemCode)
                    .SetData(.Rows.Count - 1, sampBrand, rsSamp!Brand)
                    .SetData(.Rows.Count - 1, sampSubGroup, rsSamp!subgroupID)
                End With
            Loop
            .Close()
        End With
        Call fillGridColours()
        sampleGrid.AutoSizeCols()
    End Sub
    Private Sub lblFocusBack_Click(sender As System.Object, e As System.EventArgs) Handles lblFocusBack.Click, lblNormalFore.Click, lblNormalBack.Click, lblHighlightFore.Click, lblHighlightBack.Click, lblFocusFore.Click, lblFixedFore.Click, lblFixedBack.Click, lblAlternateFore.Click, lblAlternateBack.Click, lblEmptyAreaBack.Click, lblTlTipFore.Click, lblTlTipBack.Click
        With DirectCast(sender, Label)
            Select Case CInt(.Tag)
                Case 0
                    Call showClrDlg(sender, sampleGrid.Styles.Normal, "F")
                Case 1
                    Call showClrDlg(sender, sampleGrid.Styles.Normal, "B")
                Case 2
                    Call showClrDlg(sender, sampleGrid.Styles.Alternate, "F")
                Case 3
                    Call showClrDlg(sender, sampleGrid.Styles.Alternate, "B")
                Case 4
                    Call showClrDlg(sender, sampleGrid.Styles.Fixed, "F")
                Case 5
                    Call showClrDlg(sender, sampleGrid.Styles.Fixed, "B")
                Case 6
                    Call showClrDlg(sender, sampleGrid.Styles.Highlight, "F")
                Case 7
                    Call showClrDlg(sender, sampleGrid.Styles.Highlight, "B")
                Case 8
                    Call showClrDlg(sender, sampleGrid.Styles.Focus, "F")
                Case 9
                    Call showClrDlg(sender, sampleGrid.Styles.Focus, "B")
                Case 10
                    Call showClrDlg(sender, sampleGrid.Styles.EmptyArea, "B")


            End Select
        End With
    End Sub
    Private Sub lblTlTipBack_click(sender As System.Object, e As System.EventArgs) Handles lblTlTipFore.Click, lblTlTipBack.Click
        With DirectCast(sender, Label)
            Select Case CInt(.Tag)
                Case 11
                    Call showClrDlgTltip(sender, lblToolTip, pnlTlTip, "F")
                Case 12
                    Call showClrDlgTltip(sender, lblToolTip, pnlTlTip, "B")
            End Select
        End With
    End Sub
    Private Sub showClrDlg(ByVal argLbl As Label, ByVal argType As CellStyle, ByVal argClrType As String)
        Dim cDialog As New ColorDialog()
        cDialog.Color = argLbl.BackColor ' initial selection is current color.

        If (cDialog.ShowDialog() = DialogResult.OK) Then
            argLbl.BackColor = cDialog.Color ' update with user selected color.
            Dim cs As CellStyle = argType

            If argClrType = "B" Then
                cs.BackColor = argLbl.BackColor

            Else
                cs.ForeColor = argLbl.BackColor
            End If

        End If
    End Sub
    Private Sub showClrDlgTltip(ByVal argLbl As Label, ByVal argtltip As Label, ByVal argPnltltip As Panel, ByVal argClrType As String)
        Dim cDialog As New ColorDialog()
        cDialog.Color = argLbl.BackColor ' initial selection is current color.

        If (cDialog.ShowDialog() = DialogResult.OK) Then
            argLbl.BackColor = cDialog.Color ' update with user selected color.
            Select Case argClrType

                Case "B"
                    argtltip.BackColor = argLbl.BackColor
                    argPnltltip.BackColor = argLbl.BackColor

                Case "F"
                    argtltip.ForeColor = argLbl.BackColor
            End Select
        End If
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Dim str As String, strSQL As String
        Try
            'Normal
            str = lblNormalFore.BackColor.ToArgb & ", " & lblNormalBack.BackColor.ToArgb & ", "
            'Alternate
            str = str & lblAlternateFore.BackColor.ToArgb & ", " & lblAlternateBack.BackColor.ToArgb & ", "
            'Fixed
            str = str & lblFixedFore.BackColor.ToArgb & ", " & lblFixedBack.BackColor.ToArgb & ", "
            'Highlight
            str = str & lblHighlightFore.BackColor.ToArgb & ", " & lblHighlightBack.BackColor.ToArgb & ", "
            'Focus
            str = str & lblFocusFore.BackColor.ToArgb & ", " & lblFocusBack.BackColor.ToArgb & ","
            'Empty Area
            str = str & lblEmptyAreaBack.BackColor.ToArgb

            strSQL = "Update UtlGSettings set SetValue = '" & str & "' where SetName= 'GridClrs' and SetGroup = 'COMPANY'"

            clsSVT.execSQLStatement(strSQL)

            'Tool Tip
            str = lblTlTipFore.BackColor.ToArgb & ", " & lblTlTipBack.BackColor.ToArgb
            strSQL = "Update UtlGSettings set SetValue = '" & str & "' where SetName= 'ToolTipClr' and SetGroup = 'ToolTip'"

            clsSVT.execSQLStatement(strSQL)

            MsgBox("Updated Successfully!", vbInformation, Me.Text)
        Catch ex As Exception
            MsgBox("Update failed! Please check for valid data and retry.", vbCritical, Me.Text)
            Exit Sub
        End Try
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub
End Class