Public Class frmUtlInvNumberFmt
    Const vMnthName = 0
    Const vMnthPrefix = 1

    Dim optsel As String
    Dim clsSVT As New svtLibrary
    Dim svtTrn As oledb.oledbTransaction

    Private Sub frmUtlInvNumberFmt_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Call clearflds()
        Call initScreen()
    End Sub
    Private Sub initScreen()
        fraMonth.Visible = False
        With vsfDocNoSet
            .Cols.Count = 15
            For i = 0 To .Cols.Count - 1
                .Cols(i).Width = 32
                .Cols(i).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter
            Next

        End With
        'Call SetColWidth(vsfDocNoSet, 42, 48, 72, 55, 70, 72, 58, 70)
        With vsfEx
            .Cols.Count = 8
            .Rows.Count = 3
            For i = 0 To .Cols.Count - 1
                .SetData(0, i, i + 1)
                .Cols(i).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter
                .Cols(i).TextAlignFixed = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter
                .AutoSizeCols()
            Next i
            For i = 1 To 1
                .SetData(i, 0, "Prefix")
                .SetData(i, 1, "Counter")
                .SetData(i, 2, "Year From")
                .SetData(i, 3, "Year To")
                .SetData(i, 4, "Sh YearFrom")
                .SetData(i, 5, "Sh YearTo")
                .SetData(i, 6, "DocNo")
                .SetData(i, 7, "ShDocNo")
            Next i
            .SetData(2, 0, "JAN")
            .SetData(2, 1, "C1")
            .SetData(2, 2, Year(gSettings.gCompStDt))
            .SetData(2, 3, Year(gSettings.gCompEndDt))
            .SetData(2, 4, Microsoft.VisualBasic.Strings.Right(.GetData(2, 2).ToString, 2))
            .SetData(2, 5, Microsoft.VisualBasic.Strings.Right(.GetData(2, 3).ToString, 2))
            .SetData(2, 6, "00001")
            .SetData(2, 7, "1")
            Call SetColWidth(vsfEx, 42, 48, 72, 55, 70, 72, 58, 70)
        End With

        With vsfMnth
            .Rows.Count = 13
            .Cols.Count = 2
            .ExtendLastCol = True
            .SetData(0, vMnthName, "Month")
            .SetData(0, vMnthPrefix, "Prefix")

            .SetData(1, vMnthName, "Jan")
            .SetData(2, vMnthName, "Feb")
            .SetData(3, vMnthName, "Mar")
            .SetData(4, vMnthName, "Apr")
            .SetData(5, vMnthName, "May")
            .SetData(6, vMnthName, "Jun")
            .SetData(7, vMnthName, "Jul")
            .SetData(8, vMnthName, "Aug")
            .SetData(9, vMnthName, "Sep")
            .SetData(10, vMnthName, "Oct")
            .SetData(11, vMnthName, "Nov")
            .SetData(12, vMnthName, "Dec")
            .Cols(vMnthPrefix).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter
        End With
        Call SetColWidth(vsfMnth, 62, -1)
        Call fillInvNoFmt()
        Call SetDocInvFmt()
        initGridStyles(Me)
        cboInvF8.Text = loadCboValue("INVF12")
        cboInvCtrlF8.Text = loadCboValue("INVCTRLF12")
        cboInvAltF8.Text = loadCboValue("INVALTF12")
        loadContent()
    End Sub

    Private Sub fillInvNoFmt()
        Dim strSQL As String
        Dim rs As oledb.oledbDataReader
        Dim chkCount As String, intRow As Integer
        Dim strMnth As String = ""
        Dim MnthWise() As String, j As Integer


        strSQL = "Select SetValue from UtlGSettings where SetGroup = 'Invoice' and SetName = 'InvNoReset'"
        rs = clsSVT.getDataReaderFromSQL(strSQL)

        With rs
            Do While .Read
                If rs!SetValue = "M" Then
                    optMonthWise.Checked = True
                ElseIf rs!SetValue = "D" Then
                    optDaywise.Checked = True
                Else
                    optNone.Checked = False
                End If
            Loop
            .Close()
        End With

        strSQL = "Select SetValue from UtlGSettings where SetGroup = 'Invoice' and SetName = 'CounterWsRst'"

        chkCount = clsSVT.getDataStringValue(strSQL, "", False)
        Call ApplyChkToCheckBox(chkCounterWsRst, chkCount)

        strSQL = "Select SetValue from UtlGSettings where SetGroup = 'Invoice' and SetName = 'PrefixSettings'"

        strMnth = clsSVT.getDataStringValue(strSQL, "", False)
        MnthWise = strMnth.Split(",")
        intRow = 1
        With vsfMnth
            For j = 0 To UBound(MnthWise)
                strMnth = MnthWise(j)
                If Not intRow >= 13 Then
                    .SetData(intRow, vMnthPrefix, strMnth)
                    intRow = intRow + 1
                End If
            Next
        End With
        Call fillDocNoPfx()
    End Sub

    Private Function loadCboValue(ByVal argSetName) As String
        Dim strValue As String
        strValue = clsSVT.getDataStringValue("Select SetValue from utlGSettings where  setName='" & argSetName & "'", "", False)
        Select Case strValue
            Case "S"
                Return "Save"
            Case "SP"
                Return "Save and Print"
            Case "SPP"
                Return "Save and Preview"
            Case Else
                Return ""
        End Select
    End Function

    Private Function retriveCboValue(ByVal argValue) As String
        retriveCboValue = ""
        Select Case argValue
            Case "Save"
                Return "S"
            Case "Save and Print"
                Return "SP"
            Case "Save and Preview"
                Return "SPP"
        End Select
    End Function



    Private Sub loadContent()
        Dim strSql As String
        Dim rsDummy As oledb.oledbDataReader
        strSql = "Select * from UtlMisc"
        rsDummy = clsSVT.getDataReaderFromSQL(strSql)

        If Not rsDummy.HasRows Then
            rsDummy.Close()
            Exit Sub
        End If

        rsDummy.Read()
        txtValue.Text = rsDummy!InvSMSSalesvalue
        txtSalesMsg.Text = rsDummy!InvSMSSalesMSg
        txtCancelMsg.Text = rsDummy!InvSMSCancelMsg
        txtDelMsg.Text = rsDummy!InvSMSDeleteMsg
        ApplyChkToCheckBox(chkForSalesValue, rsDummy!InvSMSblnSales)
        ApplyChkToCheckBox(chkForCancellation, rsDummy!InvSMSblnCancel)
        ApplyChkToCheckBox(chkForDeletion, rsDummy!InvSMSblnDelete)
        ApplyChkToCheckBox(chkUpdateRate, rsDummy!RateFld)
        rsDummy.Close()

    End Sub


    Private Sub fillDocNoPfx()
        Dim strSQL As String, strDocNo As String
        Dim strDocPfx() As String, i As Integer

        strSQL = "Select SetValue from UtlGSettings where SetGroup = 'Invoice' and SetName = 'DocNoFmt'"
        strDocNo = clsSVT.getDataStringValue(strSQL, "", False)

        Dim intCol As Integer = 0
        'For i = 0 To strDocNo.Length - 1
        '    If strDocNo(i) = "<" Then Continue For
        '    If strDocNo(i) = ">" Then
        '        If strDocNo(i + 1) = ">" Then
        '            vsfDocNoSet.SetData(0, intCol, strDocNo(i - 1))
        '            intCol = intCol + 1
        '            Continue For
        '        End If
        '    End If
        '    vsfDocNoSet.SetData(0, intCol, strDocNo(i))
        'Next i
        strDocNo = Replace(strDocNo, "<<", ",")
        strDocNo = Replace(strDocNo, ">>", ",")
        strDocNo = Replace(strDocNo, ",", ",")
        'strDocNo = Regex.Replace(strDocNo, "[^A-Za-z0-9\-/]", ",")

        strDocNo = Mid(strDocNo, 2, Len(strDocNo) - 2)

        strDocPfx = strDocNo.Split(",")

        intCol = 0
        With vsfDocNoSet
            For i = 0 To UBound(strDocPfx)
                strDocNo = strDocPfx(i)
                If strDocNo <> "" Then
                    If Not intCol >= 15 Then
                        .SetData(0, intCol, strDocNo)
                        intCol = intCol + 1
                    End If
                End If
            Next
        End With
        'strSQL = "Select SetValue from UtlGSettings where SetGroup = 'Invoice' and SetName = 'DocNoFmt'"
        'strDocNo = clsSVT.getDataStringValue(strSQL, "", False)

        'strDocNo = Replace(strDocNo, "<<", ",")
        'strDocNo = Replace(strDocNo, ">>", ",")
        'strDocNo = Regex.Replace(strDocNo, "[^A-Za-z0-9\-/]", ",")

        'strDocNo = Mid(strDocNo, 2, Len(strDocNo) - 2)

        'strDocPfx = strDocNo.Split(",")

        'intcol = 0
        'With vsfDocNoSet
        '    For i = 0 To UBound(strDocPfx)
        '        strDocNo = strDocPfx(i)
        '        If Not intcol >= 8 Then
        '            .SetData(0, intcol, strDocNo)
        '            intcol = intcol + 1
        '        End If
        '    Next
        'End With

        'For i = 1 To UBound(strDocPfx)
        '    strDocNo = strDocPfx(i)
        '    If Not intcol >= 8 Then
        '        .SetData(0, intcol, strDocPfx)
        '        intcol = intcol + 1
        '    End If
        'Next
        'With vsfDocNoSet
        '    For i = 0 To .Cols.Count - 1
        '        strDocPfx = strDocNo.Substring(0, 1)
        '        If strDocPfx <= 8 Then
        '            .SetData(0, i, strDocPfx)
        '        ElseIf strDocPfx = Regex.IsMatch(strDocPfx, "[. $ ^ { [ ( | ) * + ? \]") Then
        '            .SetData(0, i, strDocPfx)
        '        Else
        '            strDocPfx = Regex.IsMatch(strDocPfx, "[A-Za-z]")
        '            .SetData(0, i, strDocPfx)
        '        End If
        '        strDocNo = strDocNo.Remove(strDocPfx)
        '    Next
        'End With
        'For i = 0 To strDocNo.Length - 1
        '    strDocPfx = strDocNo.Substring(i, i)
        '    If Not intcol >= 8 Then
        '        .SetData(0, intcol, strDocPfx)
        '        intcol = intcol + 1
        '    End If
        'Next
    End Sub
    Private Sub vsfMnth_BeforeEdit(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfMnth.BeforeEdit
        If e.Col <> vMnthPrefix Then
            e.Cancel = True
            Exit Sub
        End If
    End Sub

    Private Sub optMonthWise_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles optMonthWise.CheckedChanged
        If optMonthWise.Checked Then
            optsel = "M"
            fraMonth.Visible = True
        End If
    End Sub

    Private Sub optFixed_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles optNone.CheckedChanged
        If optNone.Checked Then
            optsel = ""
            fraMonth.Visible = False
        End If

    End Sub

    Private Sub btnMnthDefault_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnMnthDefault.Click
        Call fillDefMnthStyle("Style-1")
    End Sub

    Private Sub btnMnthDefault_DropDownItemClicked(ByVal sender As Object, ByVal e As C1.Win.C1Input.DropDownItemClickedEventArgs) Handles btnMnthDefault.DropDownItemClicked
        Call fillDefMnthStyle(e.ClickedItem.Text)
    End Sub

    Private Sub fillDefMnthStyle(ByVal argStyle As String)
        With vsfMnth
            Select Case argStyle
                Case "Style-1"
                    .SetData(1, vMnthPrefix, "A")
                    .SetData(2, vMnthPrefix, "B")
                    .SetData(3, vMnthPrefix, "C")
                    .SetData(4, vMnthPrefix, "D")
                    .SetData(5, vMnthPrefix, "E")
                    .SetData(6, vMnthPrefix, "F")
                    .SetData(7, vMnthPrefix, "G")
                    .SetData(8, vMnthPrefix, "H")
                    .SetData(9, vMnthPrefix, "I")
                    .SetData(10, vMnthPrefix, "J")
                    .SetData(11, vMnthPrefix, "K")
                    .SetData(12, vMnthPrefix, "L")

                Case "Style-2"
                    .SetData(1, vMnthPrefix, "J")
                    .SetData(2, vMnthPrefix, "F")
                    .SetData(3, vMnthPrefix, "M")
                    .SetData(4, vMnthPrefix, "A")
                    .SetData(5, vMnthPrefix, "Y")
                    .SetData(6, vMnthPrefix, "U")
                    .SetData(7, vMnthPrefix, "L")
                    .SetData(8, vMnthPrefix, "G")
                    .SetData(9, vMnthPrefix, "S")
                    .SetData(10, vMnthPrefix, "O")
                    .SetData(11, vMnthPrefix, "N")
                    .SetData(12, vMnthPrefix, "D")

                Case "Style-3"
                    .SetData(1, vMnthPrefix, "1")
                    .SetData(2, vMnthPrefix, "2")
                    .SetData(3, vMnthPrefix, "3")
                    .SetData(4, vMnthPrefix, "4")
                    .SetData(5, vMnthPrefix, "5")
                    .SetData(6, vMnthPrefix, "6")
                    .SetData(7, vMnthPrefix, "7")
                    .SetData(8, vMnthPrefix, "8")
                    .SetData(9, vMnthPrefix, "9")
                    .SetData(10, vMnthPrefix, "O")
                    .SetData(11, vMnthPrefix, "N")
                    .SetData(12, vMnthPrefix, "D")
            End Select
        End With
    End Sub

    Private Sub vsfMnth_SetupEditor(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfMnth.SetupEditor
        If vsfMnth.Cols(e.Col).Index = vMnthPrefix Then
            Dim tb As TextBox = vsfMnth.Editor
            tb.MaxLength = 1
            tb.CharacterCasing = CharacterCasing.Upper
        End If
    End Sub


    Private Sub vsfDocNoSet_AfterEdit(sender As Object, e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfDocNoSet.AfterEdit
        Call SetDocInvFmt()
    End Sub

    Private Sub SetDocInvFmt()

        Dim i As Integer
        Dim j As Integer
        Dim strDocNo As String
        strDocNo = ""

        With vsfDocNoSet
            For i = 0 To .Cols.Count - 1
                Select Case Val(.GetData(0, i))
                    Case 1, 2, 3, 4, 5, 6, 7, 8
                        j = Val(.GetData(0, i)) - 1
                        strDocNo = strDocNo & vsfEx.GetData(2, j)
                    Case Else
                        strDocNo = strDocNo & .GetData(0, i)
                End Select
            Next i
            txtDocNo.Text = strDocNo
        End With
    End Sub
    Private Sub clearflds()
        Dim i As Integer
        With vsfDocNoSet
            For i = 0 To .Cols.Count - 1
                .SetData(0, i, Nothing)
            Next
        End With
        chkCounterWsRst.Checked = False
        optDaywise.Checked = False
        optMonthWise.Checked = False
        optNone.Checked = True
        txtDocNo.Text = ""
        txtSalesMsg.Text = ""
        txtCancelMsg.Text = ""
        txtDelMsg.Text = ""
        txtValue.Text = ""
    End Sub
    Private Function ValidFields() As Boolean
        Dim i As Integer = 0
        ValidFields = False
        Dim blnFalse As Boolean = False
        If optMonthWise.Checked = True Or optDaywise.Checked = True Then

            If clsSVT.ChkDuplicateData(vsfMnth, i, vMnthPrefix) Then
                MsgBox("Duplicate Prefix '" & vsfMnth.GetData(i, vMnthPrefix) & "'!!", MsgBoxStyle.Information, Me.Text)
                vsfMnth.Row = i
                vsfMnth.Col = vMnthPrefix
                vsfMnth.Focus()
                Exit Function
            End If

            For i = 0 To vsfMnth.Rows.Count - 1
                If vsfMnth.GetData(i, vMnthPrefix) = "" Then
                    MsgBox("Please fill the default Styles ", MsgBoxStyle.Information, Me.Text)
                    btnMnthDefault.Focus()
                    Exit Function
                End If
            Next
            For i = 0 To vsfDocNoSet.Cols.Count - 1
                If vsfDocNoSet.GetData(0, i) = "1" Then
                    blnFalse = True
                End If
            Next

            If Not blnFalse Then
                MsgBox("Please select 1 as prefix if month or daywise is selected.", vbInformation, Me.Text)
                Exit Function
            End If
        End If

        If cboInvF8.Text = cboInvCtrlF8.Text OrElse cboInvF8.Text = cboInvAltF8.Text OrElse cboInvCtrlF8.Text = cboInvAltF8.Text Then
            MsgBox("Please select  distinct Invoice value", MsgBoxStyle.Information, Me.Text)
            Exit Function
        End If

        ValidFields = True
    End Function
    Private Function saveDetails() As Boolean
        Dim strSQL As String, strDocNo As String = ""
        Dim strMnth As String = "", strDoc As String = ""

        saveDetails = False
        svtTrn = svtConn.BeginTransaction(IsolationLevel.ReadCommitted)
        Try
            strSQL = UpdateInv(optsel, "InvNoReset")
            If Not clsSVT.execSQLStatement(strSQL, svtTrn) Then
                svtTrn.Rollback()
                saveDetails = False
                Exit Function
            End If

            strSQL = UpdateInv(getCheckString(chkCounterWsRst), "CounterWsRst")
            If Not clsSVT.execSQLStatement(strSQL, svtTrn) Then
                svtTrn.Rollback()
                saveDetails = False
                Exit Function
            End If

            strDocNo = ""
            For i = 0 To vsfDocNoSet.Cols.Count - 1
                strDocNo = vsfDocNoSet.GetData(0, i)

                If Val(strDocNo) <> 0 And Val(strDocNo) < 8 Then
                    strDoc = strDoc & "<<" & Val(vsfDocNoSet.GetData(0, i)) & ">>"
                Else
                    strDoc = strDoc & vsfDocNoSet.GetData(0, i) & ","
                End If
            Next i

            ' If condition for CheckCounter Added by surabhi
            If getCheckString(chkCounterWsRst) = "Y" Then
                If strDoc.Contains("<<2>>") Then
                    gCounterWiseFmt = True
                Else
                    gCounterWiseFmt = False
                End If
            End If

            strSQL = UpdateInv(strDoc, "DocNofmt")
            If Not clsSVT.execSQLStatement(strSQL, svtTrn) Then
                svtTrn.Rollback()
                saveDetails = False
                Exit Function
            End If


            With vsfMnth
                For i = 1 To .Rows.Count - 1
                    strMnth = strMnth & "" & .GetData(i, vMnthPrefix) & ","
                Next i
                strMnth = Mid(strMnth, 1, Len(strMnth) - 1)
            End With

            If optNone.Checked = True Then
                strMnth = ""
            End If

            strSQL = UpdateInv(strMnth, "PrefixSettings")
            If Not clsSVT.execSQLStatement(strSQL, svtTrn) Then
                svtTrn.Rollback()
                saveDetails = False
                Exit Function
            End If
            '....................................................................
            Dim strValue As String
            If cboInvF8.Text <> "" Then
                strValue = retriveCboValue(cboInvF8.Text)
                strSQL = UpdateInv(strValue, "INVF12")
                If Not clsSVT.execSQLStatement(strSQL, svtTrn) Then
                    svtTrn.Rollback()
                    saveDetails = False
                    Exit Function
                End If

            End If
            If cboInvCtrlF8.Text <> "" Then
                strValue = retriveCboValue(cboInvCtrlF8.Text)
                strSQL = UpdateInv(strValue, "INVCTRLF12")
                If Not clsSVT.execSQLStatement(strSQL, svtTrn) Then
                    svtTrn.Rollback()
                    saveDetails = False
                    Exit Function
                End If
            End If
            If cboInvAltF8.Text <> "" Then
                strValue = retriveCboValue(cboInvAltF8.Text)
                strSQL = UpdateInv(strValue, "INVAltF12")
                If Not clsSVT.execSQLStatement(strSQL, svtTrn) Then
                    svtTrn.Rollback()
                    saveDetails = False
                    Exit Function
                End If
            End If

            '............................................................................
            strSQL = "update UtlMisc set"
            strSQL = strSQL & " InvSMSblnSales= '" & getCheckString(chkForSalesValue) & "',"
            strSQL = strSQL & "InvSMSblnCancel= '" & getCheckString(chkForCancellation) & "',"
            strSQL = strSQL & "InvSMSblnDelete='" & getCheckString(chkForDeletion) & "',"

            strSQL = strSQL & "InvSMSSalesvalue=" & GetVal(txtValue.Text, 0) & ", "
            strSQL = strSQL & "InvSMSSalesMSg= '" & GetVal(txtSalesMsg.Text, "") & "',"

            strSQL = strSQL & "InvSMSCancelMsg= '" & GetVal(txtCancelMsg.Text, "") & "',"


            strSQL = strSQL & "InvSMSDeleteMsg= '" & GetVal(txtDelMsg.Text, "") & "',"

            strSQL = strSQL & "RateFld='" & getCheckString(chkUpdateRate) & "' "
            If Not clsSVT.execSQLStatement(strSQL, svtTrn) Then
                svtTrn.Rollback()
                saveDetails = False
                Exit Function
            End If



            MsgBox("Saved Successfully.", vbInformation, Me.Text)
            saveDetails = True
            svtTrn.Commit()
        Catch ex As Exception
            MsgBox("Error during save" & vbCrLf & ex.Message, MsgBoxStyle.Critical, Me.Text)
            saveDetails = False
            svtTrn.Rollback()
        End Try
    End Function

    Private Function UpdateInv(ByVal SetVal As String, ByVal SetName As String) As String
        Dim strSQL As String

        strSQL = " Update UtlGSettings set "
        strSQL = strSQL & "SetValue = '" & SetVal & "'"
        strSQL = strSQL & " where SetGroup = 'Invoice' and SetName = '" & SetName & "'"

        UpdateInv = strSQL
    End Function
    Private Sub vsfEx_BeforeEdit(sender As Object, e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfEx.BeforeEdit
        e.Cancel = True
    End Sub

    Private Sub optDaywise_CheckedChanged(sender As Object, e As System.EventArgs) Handles optDaywise.CheckedChanged
        If optDaywise.Checked Then
            optsel = "D"
            fraMonth.Visible = True
        End If
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        If ValidFields() Then
            If saveDetails() Then
                If chkUpdateRate.Checked Then
                    gstrChkRateFld = "Y"
                Else
                    gstrChkRateFld = "N"
                End If
                Call clearflds()
                Me.Close()
            End If
        End If
    End Sub

    Private Sub chkForCancellation_CheckedChanged(sender As Object, e As EventArgs) Handles chkForCancellation.CheckedChanged
        If chkForCancellation.Checked Then
            txtCancelMsg.Enabled = True
        Else
            txtCancelMsg.Enabled = False
            txtCancelMsg.Text = ""
            txtCancelMsg.Focus()
        End If
    End Sub

    Private Sub chkForDeletion_CheckedChanged(sender As Object, e As EventArgs) Handles chkForDeletion.CheckedChanged
        If chkForDeletion.Checked Then
            txtDelMsg.Enabled = True
        Else
            txtDelMsg.Enabled = False
            txtDelMsg.Text = ""
            txtDelMsg.Focus()
        End If
    End Sub

    Private Sub chkForSalesValue_CheckedChanged(sender As Object, e As EventArgs) Handles chkForSalesValue.CheckedChanged
        If chkForSalesValue.Checked Then
            txtSalesMsg.Enabled = True
            txtValue.Enabled = True
        Else
            txtSalesMsg.Enabled = False
            txtValue.Enabled = False
            txtSalesMsg.Text = ""
            txtValue.Text = ""
            txtValue.Focus()
        End If
    End Sub
    Private Sub txtValue_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtValue.KeyPress
        If Not Char.IsNumber(e.KeyChar) And Not e.KeyChar = Chr(Keys.Delete) And Not e.KeyChar = Chr(Keys.Back) Then
            e.Handled = True
        End If
    End Sub

    Private Sub txtSalesMsg_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtSalesMsg.KeyPress, txtDelMsg.KeyPress, txtCancelMsg.KeyPress
        If e.KeyChar = "'" Then
            e.Handled = True
        End If
    End Sub

End Class