Public Class ucSearch
    Dim clsSVT As New svtLibrary
    Event TxtChanged(ByVal argValue As String)
    Event TxtKeyUp(ByVal argValue As Int32)
    Event TxtValidation(ByVal argValue As String)
    Event vsfDoubleClick(ByVal argValue As Int32)
    Event vsfKeyUp(ByVal argValue As Int32)
    Event btnClicked(ByVal argSearch As String)
    Event TxtIDChanged(ByVal argSearch As String, ByVal argID As Int32)

    Const vPID = 0
    Const vPCode = 1
    Const vPName = 2
    Const vPTot = 3

    Const vAName = 0
    Const vAAdd1 = 1
    Const vAAdd2 = 2
    Const vAAdd3 = 3
    Const vACity = 4
    Const vAState = 5
    Const vABtn = 6
    Const vATot = 7

    Dim blnGridLoading As Boolean = False
    Dim blnParty As Boolean = True
    Dim blnValidation As Boolean = True
    Dim blnExpand As Boolean

    Public Sub SetName(ByVal argType As String)
        Select Case argType
            Case "C"
                lblParty.Text = "Customer"
                vsfAddress.Visible = True
                vsfView.Visible = False
                btnLoad.Visible = True
            Case "S"
                lblParty.Text = "Supplier"
                vsfAddress.Visible = True
                vsfView.Visible = False
                btnLoad.Visible = True
            Case Else
                lblParty.Text = "Search"
                lblParty.ForeColor = Color.White
                '  lblParty.Font.Style = FontStyle.Bold
                vsfAddress.Visible = False
                vsfView.Visible = False
                btnLoad.Visible = False
                Label1.Visible = False
        End Select
    End Sub

    Public Sub ClearSearch(ByVal argType As String)
        Select Case argType
            Case "C", "S"
                vsfAddress.Visible = True
                vsfView.Visible = False
                btnLoad.Visible = True
            Case Else
                vsfAddress.Visible = False
                vsfView.Visible = False
                btnLoad.Visible = False
        End Select
        txtParty.Text = ""
        txtPartyID.Text = ""
        vsfAddress.Rows.Count = 0
        vsfAddress.Rows.Count = vATot
        vsfAddress.Cols.Count = 1
        Call frmNormalSize()
    End Sub

    Public Function getId() As Int32
        Return Val(txtPartyID.Text)
    End Function

    Public Sub setID(ByVal argID As Int32)
        txtPartyID.Text = argID
    End Sub

    Public Sub ClearFlds()
        txtParty.Text = ""
        txtPartyID.Text = ""
        vsfView.Visible = False
        vsfAddress.Rows.Count = 0
        vsfAddress.Rows.Count = vATot
        vsfAddress.Cols.Count = 1
    End Sub

    Public Sub vsfAnchor()
        vsfView.Anchor = (AnchorStyles.Bottom Or AnchorStyles.Top Or AnchorStyles.Left)
        lblParty.Anchor = (AnchorStyles.Top Or AnchorStyles.Left)
        txtParty.Anchor = (AnchorStyles.Top Or AnchorStyles.Left)
    End Sub

    Public Sub vsfAnchorSize(ByVal argX As Int32, argY As Int32)
        Me.Size = New Point(argX, argY)
    End Sub

    Public Sub vsfAnchorReSize(ByVal argX As Int32, argY As Int32)
        Me.Size = New Point(argX, argY)
    End Sub

    Public Sub frmNormalSize()
        Me.Size = New Point(408, 28)
    End Sub

    Public Sub frmReSize(ByVal argX As Int32, argY As Int32)
        Me.Size = New Point(argX, argY)
    End Sub

    Public Sub vsfColor()
        ' initGridStyles(Me)
    End Sub

    Public Sub enabledisable(ByVal argType As String, ByVal argFlag As Boolean)
        Select Case argType
            Case "C", "S"
                vsfAddress.AllowEditing = argFlag
        End Select
        Lock(txtParty, argFlag)
    End Sub

    Private Sub btnLoad_Click(sender As System.Object, e As System.EventArgs) Handles btnLoad.Click
        Select Case lblParty.Text
            Case "Customer"
                RaiseEvent btnClicked("C")
            Case "Supplier"
                RaiseEvent btnClicked("S")
            Case Else
                RaiseEvent btnClicked("T")
        End Select
    End Sub

    Private Sub txtPartyID_TextChanged(sender As Object, e As System.EventArgs) Handles txtPartyID.TextChanged
        If blnGridLoading = True Then Exit Sub
        If Trim(txtPartyID.Text) = "" Then Exit Sub
        Select Case lblParty.Text
            Case "Customer"
                RaiseEvent TxtIDChanged("C", Val(txtPartyID.Text))
            Case "Supplier"
                RaiseEvent TxtIDChanged("S", Val(txtPartyID.Text))
            Case Else
                RaiseEvent TxtIDChanged("T", Val(txtPartyID.Text))
        End Select
    End Sub

    Private Sub txtParty_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles txtParty.KeyPress
        If e.KeyChar = "'" Then
            e.Handled = True
            Exit Sub
        End If
    End Sub

    Private Sub txtParty_KeyUp(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles txtParty.KeyUp
        Select Case e.KeyCode
            Case Keys.Enter
                If vsfView.Visible = True And Trim(txtParty.Text) <> "" And vsfView.Row > 0 Then
                    RaiseEvent TxtKeyUp(vsfView.Row)
                End If
                If Trim(txtParty.Text) = "" Then
                    RaiseEvent TxtKeyUp(-100)
                End If
                txtParty.Text = ""

            Case Keys.Down
                If vsfView.Visible = True And Trim(txtParty.Text) <> "" AndAlso vsfView.Rows.Count > 1 Then
                    blnValidation = False
                    vsfView.Row = 1
                    vsfView.Col = 2
                    vsfView.Focus()
                ElseIf vsfView.Visible = False And Trim(txtParty.Text) = "" AndAlso vsfView.Rows.Count > 1 Then
                    blnValidation = False
                    RaiseEvent TxtKeyUp(0)
                    vsfView.Row = 1
                    vsfView.Col = 2
                    vsfView.Focus()
                ElseIf vsfView.Visible = True And Trim(txtParty.Text) = "" AndAlso vsfView.Rows.Count > 1 Then
                    blnValidation = False
                    RaiseEvent TxtKeyUp(0)
                    vsfView.Row = 1
                    vsfView.Col = 2
                    vsfView.Focus()

                End If
            Case Keys.Escape
                vsfView.Visible = False
                txtParty.Focus()
                Call frmNormalSize()
        End Select
    End Sub

    Private Sub txtParty_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtParty.TextChanged
        If blnGridLoading = True Then Exit Sub
        If Trim(txtParty.Text) = "" Then
            blnExpand = False
            lblSearchImage.Image = My.Resources.D_FIND
        Else
            blnExpand = True
            lblSearchImage.Image = My.Resources.Remove
        End If
        RaiseEvent TxtChanged(Trim(txtParty.Text))
    End Sub

    Private Sub vsfPartySrc_DoubleClick(sender As Object, e As System.EventArgs) Handles vsfView.DoubleClick
        blnValidation = True
        If vsfView.Row > 0 Then
            RaiseEvent vsfDoubleClick(vsfView.Row)
        End If
        txtParty.Text = ""
    End Sub

    Private Sub vsfPartySrc_KeyUp(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles vsfView.KeyUp
        blnValidation = True
        If e.KeyCode = Keys.Enter And vsfView.Row > 0 Then
            RaiseEvent vsfKeyUp(vsfView.Row)
            txtParty.Text = ""
            Exit Sub
        End If
        If e.KeyCode = Keys.Escape Then
            txtParty.Focus()
            vsfView.Visible = False
            txtParty.Text = ""
            Call frmNormalSize()
        End If
    End Sub

    Public Sub txtKey(ByVal argSrc As String, ByVal argIntRow As Int32, ByVal argKey As Int32, ByVal argBlnCust As Boolean)
        If argKey = 3 Then
            vsfView.Visible = False
            Exit Sub
        End If
        If argSrc <> "" And argKey = 2 Then
            vsfView.Focus()
            Exit Sub
        End If
        If argKey = 1 And argSrc <> "" And argIntRow > 0 Then
            fillGrid(argIntRow, argBlnCust)
            Exit Sub
        End If
    End Sub

    Public Sub LoadGrid_All(ByVal argType As String, Optional ByVal argQry As String = "")
        Dim dt As DataTable
        Dim strSQL As String

        Call frmReSize(702, 429)

        Select Case argType
            'Case "C"
            '    strSQL = "Select " & gStrQryCust & " from MstCust "
            '    strSQL = strSQL & " Where InActive = 'N' "
            '    strSQL = strSQL & " order by 1,2,3"
            'Case "S"
            '    strSQL = "Select " & gStrQrySupp & " from MstSupp "
            '    strSQL = strSQL & " Where InActive = 'N' "
            '    strSQL = strSQL & " order by 1,2,3"
            Case Else
                strSQL = argQry
        End Select

        With vsfView
            Call showWaitCursor()
            dt = clsSVT.getDATATable(strSQL)
            .DataSource = dt
            .AutoSizeCols()
            .Cols(0).Visible = False
            .Refresh()
            .Visible = True
            Call showNormalCursor()
        End With
        Me.BringToFront()
    End Sub

    Public Sub LoadGrid(ByVal argSrc As String, ByVal argType As String, Optional ByVal argQry As String = "")
        Dim dt As DataTable
        Dim strSQL As String

        If argSrc = "" And argQry = "" Then
            ClearFlds()
            Call frmNormalSize()
            vsfView.Visible = False
            Exit Sub
        End If

        Call frmReSize(702, 429)

        Select Case argType
            'Case "C"
            '    strSQL = "Select " & gStrQryCust & " from MstCust "
            '    strSQL = strSQL & " Where InActive = 'N' "
            '    'For Each strCond As String In gStrQryCust.Split(",")
            '    '    If InStr(strCond, "▄") = 0 Then

            '    '    End If
            '    'Next
            '    strSQL = strSQL & " and(CustCode  like '%" & argSrc & "%' or Custname like '%" & argSrc & "%') order by 1,2,3"
            'Case "S"
            '    strSQL = "Select " & gStrQrySupp & " from MstSupp "
            '    strSQL = strSQL & " Where InActive = 'N' "
            '    strSQL = strSQL & " and(SuppCode  like '%" & argSrc & "%' or SuppName like '%" & argSrc & "%') order by 1,2,3"
            Case Else
                strSQL = argQry
        End Select

        With vsfView
            Call showWaitCursor()
            dt = clsSVT.getDATATable(strSQL)
            .DataSource = dt
            .AutoSizeCols()
            .Cols(0).Visible = False
            .Visible = True
            '.BringToFront()
            Call showNormalCursor()
        End With
        'SetFormSize() Satish15feb2018
        Me.BringToFront()
    End Sub

    Public Sub fillGrid(ByVal argRow As Integer, ByVal argType As String, Optional ByVal argQry As String = "")
        With vsfView
            If argRow < 0 Then Exit Sub
            If .Rows.Count - 1 < argRow Then Exit Sub

            Dim rs As oledb.oledbDataReader, strSQL As String
            Dim blnFound As Boolean, intPartyID As Int32 = 0, strFrm As String

            Select Case argType
                Case "C"
                    strFrm = "Cust"
                Case "S"
                    strFrm = "Supp"
                Case Else
                    strFrm = ""
            End Select

            strSQL = "Select " & strFrm & "ID from Mst" & strFrm & " where " & strFrm & "ID =" & Val(.GetData(argRow, vPID))

            If strFrm = "" Then
                txtPartyID.Text = Val(.GetData(argRow, vPID))
                Exit Sub
            End If

            intPartyID = clsSVT.getDataLongValue(strSQL, 0, blnFound)

            If blnFound = False Then
                MsgBox("Selected Party not found in master ", vbCritical, Me.Text)
                txtParty.Focus()
                Exit Sub
            End If

            strSQL = "Select * from Mst" & strFrm & " where " & strFrm & "ID =" & Val(.GetData(argRow, vPID))

            rs = clsSVT.getDataReaderFromSQL(strSQL)

            blnGridLoading = True
            Call ClearFlds()

            Do While rs.Read
                With vsfAddress
                    .SetData(vAName, 0, GetVal(rs.Item(strFrm & "Name"), ""))
                    txtParty.Text = GetVal(rs.Item(strFrm & "Code"), "")
                    txtPartyID.Text = GetVal(rs.Item(strFrm & "ID"), 0)
                    .SetData(vAAdd1, 0, GetVal(rs!Badd1, ""))
                    .SetData(vAAdd2, 0, GetVal(rs!Badd2, ""))
                    .SetData(vAAdd3, 0, GetVal(rs!Badd3, ""))
                    .SetData(vACity, 0, GetVal(rs!CITY, "") & " - " & GetVal(rs!PINCODE, ""))
                    .SetData(vAState, 0, GetVal(rs!STATE, ""))
                    .SetData(vABtn, 0, "")
                End With
            Loop
            rs.Close()
            blnGridLoading = False

            Call frmNormalSize()
        End With
    End Sub

    Public Sub SavedData(ByVal argType As String, ByVal argID As Int32, Optional ByVal argQry As String = "")
        With vsfView
            '  If argRow < 0 Then Exit Sub
            '  If .Rows.Count - 1 < argRow Then Exit Sub

            Dim rs As oledb.oledbDataReader, strSQL As String
            Dim intPartyID As Int32 = 0, strFrm As String

            Select Case argType
                Case "C"
                    strFrm = "Cust"
                Case "S"
                    strFrm = "Supp"
                Case Else
                    strFrm = ""
            End Select

            strSQL = "Select * from Mst" & strFrm & " where " & strFrm & "ID =" & Val(argID)
            If strFrm = "" Then
                strSQL = argQry
            End If

            '           strSQL = "Select * from Mst" & strFrm & " where " & strFrm & "ID =" & Val(.GetData(argRow, vPID))

            rs = clsSVT.getDataReaderFromSQL(strSQL)

            blnGridLoading = True
            Call ClearFlds()

            Do While rs.Read
                With vsfAddress
                    If strFrm = "" Then
                        txtParty.Text = ""
                        txtPartyID.Text = GetVal(rs.Item(0), 0)
                    Else
                        .SetData(vAName, 0, GetVal(rs.Item(strFrm & "Name"), ""))
                        txtParty.Text = GetVal(rs.Item(strFrm & "Code"), "")
                        txtPartyID.Text = GetVal(rs.Item(strFrm & "ID"), 0)
                        .SetData(vAAdd1, 0, GetVal(rs!Badd1, ""))
                        .SetData(vAAdd2, 0, GetVal(rs!Badd2, ""))
                        .SetData(vAAdd3, 0, GetVal(rs!Badd3, ""))
                        .SetData(vACity, 0, GetVal(rs!CITY, "") & " - " & GetVal(rs!PINCODE, ""))
                        .SetData(vAState, 0, GetVal(rs!STATE, ""))
                        .SetData(vABtn, 0, "")
                    End If
                End With
            Loop
            rs.Close()
            blnGridLoading = False

            Call frmNormalSize()
        End With
    End Sub

    Private Sub vsfPartyAddr_BeforeEdit(sender As Object, e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfAddress.BeforeEdit
        e.Cancel = True
    End Sub

    Private Sub vsfPartySrc_BeforeEdit(sender As Object, e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfView.BeforeEdit
        e.Cancel = True
    End Sub

    Private Sub txtParty_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles txtParty.Validating
        If Trim(txtParty.Text) = "" Then Exit Sub
        If Len(txtParty.Text) < 3 Then Exit Sub
        RaiseEvent TxtValidation(Trim(txtParty.Text))
    End Sub

    Public Sub ValidateText(ByVal argType As String, ByVal argSrc As String)
        With vsfView
            If blnValidation = False Then Exit Sub
            Dim strSQL As String
            Dim blnFound As Boolean, intPartyID As Int32 = 0, strFrm As String

            Select Case argType
                Case "C"
                    strFrm = "Cust"
                Case "S"
                    strFrm = "Supp"
                Case Else
                    strFrm = ""
            End Select
            strSQL = "Select " & strFrm & "ID from Mst" & strFrm & " where " & strFrm & "Code ='" & Trim(argSrc) & "'"

            intPartyID = clsSVT.getDataLongValue(strSQL, 0, blnFound)

            If blnFound = False Then
                MsgBox("Selected Party not found in master ", vbCritical, "Invalid Selection")
                txtParty.Focus()
                Exit Sub
            End If
        End With
    End Sub

    Public Sub setFocus()
        txtParty.Focus()
    End Sub

    Private Sub SetFormSize()
        Dim CellHeight As Integer = 0
        Dim GridHght As Integer = 0
        CellHeight = vsfView.Rows(0).HeightDisplay
        GridHght = CellHeight * vsfView.Rows.Count + (CellHeight * 2)
        If GridHght >= My.Computer.Screen.Bounds.Size.Width - 100 Then
            GridHght = My.Computer.Screen.Bounds.Size.Width - 500
        End If

        Me.Size = New Point(702, GridHght)
        vsfView.Anchor = (AnchorStyles.Bottom Or AnchorStyles.Top Or AnchorStyles.Right Or AnchorStyles.Left)
    End Sub

    Private Sub lblSearchImage_Click(sender As Object, e As EventArgs) Handles lblSearchImage.Click
        If blnExpand Then
            vsfView.Visible = False
            txtParty.Text = ""
            Call frmNormalSize()
            lblSearchImage.Image = My.Resources.D_FIND
            blnExpand = False
            Exit Sub
        Else
            RaiseEvent TxtChanged(" ")
            lblSearchImage.Image = My.Resources.Remove
            blnExpand = True
        End If
    End Sub
End Class
