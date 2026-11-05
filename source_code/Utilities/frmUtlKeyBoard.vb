Imports VB = Microsoft.VisualBasic

Public Class frmUtlKeyBoard
    Public argTxtBox As Object
    Private dragging As Boolean = False
    Private dragCursorPoint As Point
    Private dragFormPoint As Point
    Dim clsSVT As New svtLibrary
    Dim strSQL As String
    Dim argCForm As Object
    Dim argCFillObj As Object
    Dim argCSearch As Boolean = False
    Dim dt As DataTable
    Dim dv As DataView
    Dim intRetCol As Integer
    Dim intSearchCol As Integer
    Dim intHideCol As Integer
    Dim blnModeSingle As Boolean = False
    Dim blnModeCode As Boolean = False
    Dim blnModeMulti As Boolean = False
    Dim blnModeDefault As Boolean = False
    Dim kbType As KeyboardType
    Private Delegate Sub UpdateProgressDelegate(ByVal Message As String)

    Private Sub btnClear_ClickButtonArea(Sender As System.Object, e As System.Windows.Forms.MouseEventArgs) Handles btnClear.Click
        txtResult.Text = ""
    End Sub

    Private Sub btn0_ClickButtonArea(Sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles btn0.Click, btn1.Click, btn2.Click, _
        btn3.Click, btn4.Click, btn5.Click, btn6.Click, btn7.Click, btn8.Click, btn9.Click
        txtResult.Text = txtResult.Text & Sender.text
    End Sub

    Private Sub btnCancel_ClickButtonArea(Sender As System.Object, e As System.Windows.Forms.MouseEventArgs) Handles btnCancel.ClickButtonArea
        Me.Close()
    End Sub

    Private Sub btnSave_ClickButtonArea(Sender As System.Object, e As System.Windows.Forms.MouseEventArgs) Handles btnSave.ClickButtonArea
        If Not argCSearch Then
            argCFillObj.text = txtResult.Text
        Else
            With vsfDet
                If .Row < 0 Then
                    If .Rows.Count > 1 Then
                        argCFillObj.Text = .GetData(1, intRetCol)
                        Exit Sub
                    Else
                        argCFillObj.Text = ""
                    End If
                Else
                    argCFillObj.Text = .GetData(.Row, intRetCol)
                End If
            End With
        End If
        Me.Close()
    End Sub

    Private Sub btnToggle_ClickButtonArea(Sender As System.Object, e As System.Windows.Forms.MouseEventArgs) Handles btnToggle.Click
        If pnlAlph.Visible Then
            btnToggle.Text = "2/2"
            pnlSymb.Visible = True
            pnlAlph.Visible = False
        Else
            btnToggle.Text = "1/2"
            pnlAlph.Visible = True
            pnlSymb.Visible = False
        End If
    End Sub

    Private Sub btnQ_Click(ByVal Sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles btnQ.Click, btnW.Click, btnE.Click, btnR.Click, btnT.Click, btnY.Click, btnU.Click, _
        btnI.Click, btnO.Click, btnP.Click, btnA.Click, btnS.Click, btnD.Click, btnF.Click, btnG.Click, btnH.Click, btnJ.Click, btnK.Click, btnL.Click, btnZ.Click, btnX.Click, btnC.Click, btnV.Click, btnB.Click, btnN.Click, btnM.Click, btnMinus.Click, btnEx.Click, btnAt.Click, btnHash.Click, _
        btnDollar.Click, btnPercent.Click, btnCap.Click, btnAmper.Click, btnAst.Click, btnBkt.Click, btnLBkt.Click, btnRBkt.Click, btnLSqb.Click, btnRSqb.Click, btnUndSc.Click, btnPlus.Click, btnSlash.Click, btnPipe.Click, btnColon.Click, btnSemiColon.Click, _
        btnQuotes.Click, btnSQuote.Click, btnTilde.Click, btnLAngle.Click, btnRAngle.Click, btnComma.Click, btnFullStop.Click, btnFSlash.Click
        Dim strKey As String
        strKey = Sender.text
        txtResult.Text = txtResult.Text & Sender.text
    End Sub

    Private Sub frmUtlKeyBoard_FormClosing(sender As Object, e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        txtResult.Focus()
    End Sub

    Private Sub frmUtlKeyBoard_KeyUp(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyUp
        If e.KeyCode = Keys.Escape Then
                Me.Close()
        End If
    End Sub

    Private Sub frmUtlKeyBoard_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        KeyPreview = True
    End Sub

    Private Sub frmUtlKeyBoard_Paint(sender As Object, e As System.Windows.Forms.PaintEventArgs) Handles Me.Paint
        ControlPaint.DrawBorder(e.Graphics, e.ClipRectangle, Color.Black, ButtonBorderStyle.Solid)
    End Sub

    Public Sub ShowUtlKeyboard(argType As KeyboardType, argSearch As Boolean, argSql As String, ByVal argObject As Object, argRetCol As Integer, argHideCol As Integer, Optional argSearchCol As Integer = -1, Optional argFormCaption As String = "")
        'Dim strMode As String

        kbType = argType
        If Trim(argFormCaption) <> "" Then
            Me.Text = argFormCaption
        End If

        intRetCol = argRetCol
        intHideCol = argHideCol

        If argSearchCol >= 0 Then
            intSearchCol = argSearchCol
        End If

        'strMode = clsSVT.getDataStringValue("Select SearchMode from UtlGlobe", "", False)

        'enableDisableOptBtns(strMode)
        enableDisableOptBtns("")
        txtResult.Text = ""
        If InStr(UCase(argObject.name), "PWD") > 0 Then
            txtResult.PasswordChar = "*"
        End If
        argCFillObj = argObject
        If argSearch Then
            argCSearch = argSearch
            vsfDet.Visible = True

            strSQL = argSql
            Cursor.Current = Cursors.WaitCursor
            dt = clsSVT.getDATATable(strSQL)
            dv = New DataView(dt)

            vsfDet.Rows.Fixed = 1
            vsfDet.BeginUpdate()

            vsfDet.DataSource = dv
            Cursor.Current = Cursors.Arrow

            'Call initVsfForHdrs()

            vsfDet.EndUpdate()
            vsfDet.Cols(argHideCol).Visible = False
        Else
            pnlHdr.Visible = False
            vsfDet.Visible = False
            Me.Width = Me.Width - 200
            Me.Height = Me.Height - vsfDet.Height
        End If

        If argType = KeyboardType.AlphaNumeric Then
            pnlAlphaSym.Visible = True
            pnlSymb.Visible = True
            pnlNum.Visible = True
            If argSearch Then
                vsfDet.Width = pnlAlphaSym.Width + pnlCtrlBtns.Width + pnlNum.Width
                Me.Width = pnlAlphaSym.Width + pnlCtrlBtns.Width + pnlNum.Width + 50
                Me.Height = 680
                txtResult.Width = pnlAlphaSym.Width + pnlCtrlBtns.Width + pnlNum.Width
            Else
                txtResult.Width = pnlAlphaSym.Width + pnlCtrlBtns.Width + pnlNum.Width
                Me.Height = pnlAlphaSym.Height + txtResult.Height + 50
                Me.Width = pnlAlphaSym.Width + pnlCtrlBtns.Width + pnlNum.Width + 50
            End If
        End If

        If argType = KeyboardType.AlphaOnly Then
            pnlAlphaSym.Visible = True
            pnlSymb.Visible = True
            pnlNum.Visible = False
            If argSearch Then
                vsfDet.Width = pnlAlphaSym.Width + pnlCtrlBtns.Width
                Me.Width = pnlAlphaSym.Width + pnlCtrlBtns.Width + 50
                Me.Height = 680
                txtResult.Width = pnlAlphaSym.Width + pnlCtrlBtns.Width
            Else
                txtResult.Width = pnlAlphaSym.Width + pnlCtrlBtns.Width
                Me.Height = pnlAlphaSym.Height + txtResult.Height + 50
                Me.Width = pnlAlphaSym.Width + pnlCtrlBtns.Width + 50
            End If
        End If

        If argType = KeyboardType.NumericOnly Then
            pnlAlphaSym.Visible = False
            pnlSymb.Visible = False
            pnlNum.Visible = True
            If argSearch Then
                vsfDet.Width = pnlNum.Width + pnlCtrlBtns.Width
                Me.Width = pnlNum.Width + pnlCtrlBtns.Width + 50
                Me.Height = 680
                txtResult.Width = pnlNum.Width + pnlCtrlBtns.Width
            Else
                txtResult.Width = pnlNum.Width + pnlCtrlBtns.Width
                Me.Height = pnlAlphaSym.Height + txtResult.Height + 50
                Me.Width = txtResult.Width + 50
            End If
        End If

        txtResult.Focus()
        setGridHdrStyles(intSearchCol)
        vsfDet.AutoSizeCols()
    End Sub

    Private Sub setGridHdrStyles(Optional argCol As Integer = 0)
        resetGridHdrStyles()

        If blnModeSingle Then
            MakeColumnBold(argCol, vsfDet, "fixed")
            SetCellFlexImage(vsfDet, 0, argCol, , "magnifier.png")
        ElseIf blnModeMulti Then
            For i = 0 To vsfDet.Cols.Count - 1
                MakeColumnBold(i, vsfDet, "fixed")
                SetCellFlexImage(vsfDet, 0, i, , "magnifier.png")
            Next
        Else
            MakeColumnBold(argCol, vsfDet, "fixed")
            SetCellFlexImage(vsfDet, 0, argCol, , "magnifier.png")
        End If
    End Sub

    Private Function applyFilterFromGrid() As Boolean
        Dim StrQry As String, strValue As String
        Dim j%
        applyFilterFromGrid = False
        StrQry = ""
        strValue = Trim(txtResult.Text)
        'If strValue <> "" Then
        '    StrQry = StrQry & vsfDet.GetData(0, vsfDet.Col) & " like '%" & strValue & "%'"
        'End If

        If strValue <> "" Then
            If blnModeDefault Then
                Select Case gDBTypeCurrent
                    Case gDbTypeAccess
                        StrQry = StrQry & vsfDet.GetData(0, intSearchCol) & " like '*" & strValue & "*'"
                    Case Else
                        If vsfDet.Cols(intSearchCol).DataType = GetType(Date) Then
                            StrQry = StrQry & " 1 <> 1 "
                            GoTo ex
                        End If
                        If vsfDet.Cols(intSearchCol).DataType = GetType(Decimal) Or vsfDet.Cols(intSearchCol).DataType = GetType(Integer) Or vsfDet.Cols(intSearchCol).DataType = GetType(Date) Then
                            Try
                                If Not IsNumeric(strValue) Then Exit Try
                                Dim strTest = Convert.ToDecimal(strValue)
                                StrQry = StrQry & "Convert(" & vsfDet.GetData(0, intSearchCol) & ",'System.String')" & " = " & strValue
                            Catch ex As Exception
                                StrQry = StrQry & " 1 <> 1 "
                            End Try
                        Else
                            StrQry = StrQry & vsfDet.GetData(0, intSearchCol) & " like '" & strValue & "'"
                        End If
                End Select
            ElseIf blnModeSingle Then
                Select Case gDBTypeCurrent
                    Case gDbTypeAccess
                        StrQry = StrQry & vsfDet.GetData(0, intSearchCol) & " like '*" & strValue & "*'"
                    Case Else
                        If vsfDet.Cols(intSearchCol).DataType = GetType(Date) Then
                            StrQry = StrQry & " 1 <> 1 "
                            GoTo ex
                        End If
                        If vsfDet.Cols(intSearchCol).DataType = GetType(Decimal) Or vsfDet.Cols(intSearchCol).DataType = GetType(Integer) Or vsfDet.Cols(intSearchCol).DataType = GetType(Date) Then
                            Try
                                If Not IsNumeric(strValue) Then Exit Try
                                Dim strTest = Convert.ToDecimal(strValue)
                                StrQry = StrQry & "Convert(" & vsfDet.GetData(0, intSearchCol) & ",'System.String')" & " >= " & strValue
                            Catch ex As Exception
                                StrQry = StrQry & " 1 <> 1 "
                            End Try
                        Else
                            StrQry = StrQry & vsfDet.GetData(0, intSearchCol) & " like '%" & strValue & "%'"
                        End If
                End Select
            Else
                For j = 1 To vsfDet.Cols.Count - 1
                    If vsfDet.Cols(j).Visible Then
                        'If StrQry <> "" Then StrQry = StrQry & " OR "
                        Select Case gDBTypeCurrent
                            Case gDbTypeAccess
                                StrQry = StrQry & vsfDet.GetData(0, j) & " like '*" & strValue & "*'"
                                If j <> vsfDet.Cols.Count - 1 Then
                                    StrQry = StrQry & " OR "
                                End If
                            Case Else
                                If vsfDet.Cols(j).DataType = GetType(Date) Then
                                    StrQry = StrQry & " 1 <> 1 "
                                    Exit For
                                End If

                                If vsfDet.Cols(j).DataType = GetType(Decimal) Or vsfDet.Cols(j).DataType = GetType(Integer) Or vsfDet.Cols(j).DataType = GetType(Date) Then
                                    Try
                                        If Not IsNumeric(strValue) Then Exit Try
                                        Dim strTest = Convert.ToDecimal(strValue)
                                        If blnModeDefault Then
                                            StrQry = StrQry & "Convert(" & vsfDet.GetData(0, j) & ",'System.String')" & " = " & strValue
                                        Else
                                            StrQry = StrQry & "Convert(" & vsfDet.GetData(0, j) & ",'System.String')" & " >= " & strValue
                                        End If

                                        If j <> vsfDet.Cols.Count - 1 Then
                                            StrQry = StrQry & " OR "
                                        End If
                                    Catch ex As Exception
                                        ' vsfSearch.SetData(0, i, "")
                                        'dv.RowFilter = "1 <> 1"
                                        'Exit Function
                                        StrQry = StrQry & " 1 <> 1 "
                                        If j <> vsfDet.Cols.Count - 1 Then
                                            StrQry = StrQry & " OR "
                                        End If
                                    End Try

                                Else
                                    If blnModeDefault Then
                                        StrQry = StrQry & vsfDet.GetData(0, j) & " like '" & strValue & "'"
                                    Else
                                        StrQry = StrQry & vsfDet.GetData(0, j) & " like '%" & strValue & "%'"
                                    End If

                                    If j <> vsfDet.Cols.Count - 1 Then
                                        StrQry = StrQry & " OR "
                                    End If
                                End If
                        End Select
                    End If
                Next
            End If
        End If
ex:
        Try
            dv.RowFilter = StrQry
        Catch ex As Exception
            dv.RowFilter = " 1 <> 1 "
        End Try

        applyFilterFromGrid = True
        vsfDet.AutoSizeCols()
    End Function

    Private Sub txtResult_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles txtResult.KeyPress
        If kbType = KeyboardType.NumericOnly Then
            If e.KeyChar = "."c Then
                e.Handled = (CType(sender, TextBox).Text.IndexOf("."c) <> -1)
            ElseIf e.KeyChar <> ControlChars.Back Then
                e.Handled = ("0123456789".IndexOf(e.KeyChar) = -1)
            End If
        End If
    End Sub

    Private Sub txtResult_KeyUp(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles txtResult.KeyUp
        If e.KeyCode = Keys.Enter Then btnSave.PerformClick()
        If e.KeyCode = Keys.Up Then
            If vsfDet.Rows.Count > 1 And vsfDet.Row > 0 Then
                vsfDet.Row = vsfDet.Row - 1S
            End If
        End If
        If e.KeyCode = Keys.Down Then
            If vsfDet.Rows.Count > 1 And vsfDet.Row > 0 Then
                If vsfDet.Row < vsfDet.Rows.Count - 1 Then
                    vsfDet.Row = vsfDet.Row + 1
                End If
            End If
        End If
    End Sub

    Private Sub txtResult_TextChanged(sender As Object, e As System.EventArgs) Handles txtResult.TextChanged
        If Not argCSearch Then Exit Sub
        Call applyFilterFromGrid()
    End Sub

    Private Sub vsfDet_BeforeEdit(sender As Object, e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfDet.BeforeEdit
        e.Cancel = True
    End Sub

    Private Sub vsfDet_Click(sender As Object, e As System.EventArgs) Handles vsfDet.Click
        If blnModeMulti Then Exit Sub
        If blnModeDefault Then Exit Sub
        With vsfDet
            If .Row <> -1 Then
                resetGridHdrStyles()
                setGridHdrStyles(.Col)
                intSearchCol = .Col
            End If
        End With
    End Sub


    Private Sub vsfDet_DoubleClick(sender As Object, e As System.EventArgs) Handles vsfDet.DoubleClick
        If vsfDet.Rows.Count > 1 Then
            Call loadResultBack()
        Else
            MsgBox("No records found!", vbInformation, Me.Text)
        End If
    End Sub

    Private Sub loadResultBack()
        With vsfDet
            argCFillObj.Text = .GetData(.Row, intRetCol)
        End With
        Me.Close()
    End Sub

    Private Sub btnDel_ClickButtonArea(Sender As System.Object, e As System.Windows.Forms.MouseEventArgs) Handles btnDel.Click
        txtResult.Focus() : txtResult.Select(0, 0) : SendKeys.Send("{DELETE}")
    End Sub

    Private Sub btnBkSpace_ClickButtonArea(Sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles btnBkSpace.Click
        txtResult.Focus() : txtResult.SelectionStart = txtResult.Text.Length + 1 : SendKeys.Send("{BACKSPACE}")
    End Sub

    Private Sub btnSpace_ClickButtonArea(ByVal Sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles btnSpace.Click
        txtResult.Focus() : txtResult.SelectionStart = txtResult.Text.Length + 1 : SendKeys.Send(" ")
    End Sub

    Private Sub resetGridHdrStyles()
        With vsfDet
            For i = 0 To .Cols.Count - 1
                MakeColumnBold(i, vsfDet, "fixed", FontStyle.Regular, True)
                SetCellFlexImage(vsfDet, 0, i, , , True)
            Next
        End With
    End Sub

    Public Sub enableDisableOptBtns(argSearchMode As String)
        Select Case argSearchMode
            Case "C"
                blnModeCode = True
                blnModeDefault = False
                blnModeMulti = False
                blnModeSingle = False
                lblMode.Text = "Searching Mode: Code-Based"
                btnSingle.Enabled = True
                btnMulti.Enabled = True
            Case "S"
                blnModeSingle = True
                blnModeCode = False
                blnModeDefault = False
                blnModeMulti = False
                lblMode.Text = "Searching Mode: Single Column"
                btnSingle.Enabled = False
                btnMulti.Enabled = True
                setGridHdrStyles(intSearchCol)
            Case "M"
                blnModeMulti = True
                blnModeCode = False
                blnModeDefault = False
                blnModeSingle = False
                lblMode.Text = "Searching Mode: Multi Column"
                btnMulti.Enabled = False
                btnSingle.Enabled = True
                setGridHdrStyles()
            Case Else
                blnModeDefault = True
                blnModeCode = False
                blnModeMulti = False
                blnModeSingle = False
                lblMode.Text = "Searching Mode: Default"
                btnSingle.Enabled = True
                btnMulti.Enabled = True
                setGridHdrStyles(intSearchCol)
        End Select
    End Sub

    Private Sub btnSingle_Click(sender As Object, e As System.EventArgs) Handles btnSingle.Click
        enableDisableOptBtns("S")
    End Sub

    Private Sub btnMulti_Click(sender As Object, e As System.EventArgs) Handles btnMulti.Click
        enableDisableOptBtns("M")
    End Sub

    Private Sub vsfDet_KeyUp(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles vsfDet.KeyUp
        If e.KeyCode = Keys.Enter Then
            If vsfDet.Rows.Count > 1 Then
                Call loadResultBack()
            Else
                MsgBox("No records found!", vbInformation, Me.Text)
            End If
        End If
    End Sub
End Class