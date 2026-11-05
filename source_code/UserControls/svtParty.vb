Public Class svtParty

    Dim clsSVT As New svtLibrary

    Event TxtChanged(ByVal argValue As String)
    Event TxtKeyUp(ByVal argValue As String)
    Event TxtValidation(ByVal argValue As String)
    Event btnAddrClicked(ByVal argSearch As String)
    Event vsfDoubleClick(ByVal argValue As Int32)
    Event vsfKeyUp(ByVal argValue As Int32)
    Event afterPartySelection(ByVal argPartyID As Long)

    Event cboSelectedValueChange(ByVal argValue As Int32)

    Dim blnAutoGenCode As Boolean = False
    Dim blnValidate As Boolean = False
    Dim blnTrnEdit As Boolean = False
    Dim blnTrn As Boolean = False
    Dim blnPNP As Boolean = False
    Dim blnShip As Boolean = False
    Dim blnContact As Boolean = False
    Dim blnState As Boolean = False
    Dim strPartyType As String = ""

    Const vPID = 0
    Const vPCode = 1
    Const vPName = 2
    Const vPTot = 3

    Public Sub getPNP(ByVal argBln As Boolean)
        blnPNP = argBln
    End Sub

    Public Sub GSettings()
        Dim strSetValue As String = ""
        strSetValue = clsSVT.getDataStringValue("Select SetValue from UtlGSettings where SetGroup = 'Party' and SetName ='Country'", "", False)
        If strSetValue = "Y" Then
            PnlState.Visible = True
            blnState = True
        Else
            PnlState.Visible = False
        End If
        lblCountry.ForeColor = Color.Black
        lblState.ForeColor = Color.Black
        lblPartyName.ForeColor = Color.Black
        lblAddr.ForeColor = Color.Black
        lblCity.ForeColor = Color.Black
        lblPincode.ForeColor = Color.Black
    End Sub

    Public Sub TrnGSettings(ByVal argEdit As Boolean, Optional ByVal argShip As Boolean = False, Optional ByVal argContact As Boolean = False)
        blnTrnEdit = argEdit
        If Not argEdit Then
            PnlAddrComb.Visible = True
            pnlAddr.Visible = False
        Else
            PnlAddrComb.Visible = False
            pnlAddr.Visible = True
            Call GSettings()
        End If

        pnlShip.Visible = argShip
        blnShip = argShip

        pnlContact.Visible = argContact
        blnContact = argContact
    End Sub

    Public Sub SetPartyType(ByVal argDocID As Int32)
        Dim strPartyShForm As String
        strPartyShForm = clsSVT.getDataStringValue("Select DocShForm From UtlTransactions where DocID = " & Val(argDocID), "", False)
        lblParty.Text = strPartyShForm & " Code"
        lblPartyName.Text = strPartyShForm & " Name"
        strPartyType = strPartyShForm
    End Sub

    Public Sub SetTrnType(ByVal argType As String)
        If argType = "M" Then
            blnTrn = False
        Else
            blnTrn = True
        End If
    End Sub

    Public Sub TrnEdit(ByVal argBln As Boolean)
        If blnTrnEdit Then
            Lock(txtCode, argBln)
            Lock(txtName, argBln)
            Lock(txtAdd1, argBln)
            Lock(txtAdd2, argBln)
            Lock(txtAdd3, argBln)
            Lock(txtAdd4, argBln)
            Lock(txtPincode, argBln)
            Lock(cboCity, argBln)
            Lock(cboState, argBln)
            Lock(cboCountry, argBln)
        Else
            Lock(txtCode, argBln)
            Lock(txtName, False)
            Lock(txtAdd1, False)
            Lock(txtAdd2, False)
            Lock(txtAdd3, False)
            Lock(txtAdd4, False)
            Lock(txtPincode, False)
            Lock(cboCity, False)
            Lock(cboState, False)
            Lock(cboCountry, False)
        End If
        Lock(txtAddr, False)
        Lock(txtShipAddress, False)
        If blnShip Then
            Lock(cboShipAddr, argBln)
            Lock(btnShip, argBln)
        End If
        If blnContact Then
            Lock(cboContact, argBln)
        End If
    End Sub

    Public Sub MstEdit(ByVal argBln As Boolean)
        Lock(txtCode, argBln)
        Lock(txtName, argBln)
        Lock(txtAdd1, argBln)
        Lock(txtAdd2, argBln)
        Lock(txtAdd3, argBln)
        Lock(txtPincode, argBln)
        Lock(cboCity, argBln)
        Lock(cboState, argBln)
        Lock(cboCountry, argBln)
        Lock(txtAddr, False)
    End Sub

    Public Sub ClearFlds()
        blnValidate = False
        txtCode.Text = ""
        txtName.Text = ""
        txtAdd1.Text = ""
        txtAdd2.Text = ""
        txtAdd3.Text = ""
        txtAdd4.Text = ""
        txtPincode.Text = ""
        cboCity.Text = ""
        cboCountry.Text = ""
        cboState.Text = ""
        txtID.Text = ""
        txtShipAddID.Text = ""
        blnValidate = True
        txtAddr.Text = ""
        txtShipAddress.Text = ""
        cboContact.Text = ""
        cboShipAddr.Text = ""
    End Sub

    Public Function getContID() As String
        Return Trim(txtContID.Text)
    End Function

    Public Function getID() As String
        Return Trim(txtID.Text)
    End Function

    Public Function getCode() As String
        Return Trim(txtCode.Text)
    End Function

    Public Function getParty() As String
        If blnPNP Then
            Return Trim(txtCode.Text)
        Else
            Return Trim(txtName.Text)
        End If

    End Function

    Public Function getFullAddress() As String
        Return Trim(txtShipAddress.Text)
    End Function

    Public Function getShipID() As String
        Return Trim(txtShipAddID.Text)
    End Function

    Public Function getAdd1() As String
        Return Trim(txtAdd1.Text)
    End Function

    Public Function getAdd2() As String
        Return Trim(txtAdd2.Text)
    End Function

    Public Function getAdd3() As String
        Return Trim(txtAdd3.Text)
    End Function

    Public Function getAdd4() As String
        Return Trim(txtAdd4.Text)
    End Function

    Public Function getCity() As String
        Return Trim(cboCity.Text)
    End Function

    Public Function getPincode() As String
        Return Trim(txtPincode.Text)
    End Function

    Public Function getState() As String
        Return Trim(cboState.Text)
    End Function

    Public Function getCountry() As String
        Return Trim(cboCountry.Text)
    End Function

    Public Sub setData(ByVal argTrnID As Int32,
                       ByVal argPartyTypeID As String,
                       Optional ByVal argTrnHdrTbl As String = "",
                       Optional argTrnIdColName As String = "",
                       Optional argPartyColName As String = "",
                       Optional argShipID As Int32 = 0,
                       Optional argContID As Int32 = 0,
                       Optional argSavedData As Boolean = False)

        If argTrnID < 0 Then Exit Sub

        Dim rs As OleDb.OleDbDataReader, strSQL As String
        Dim blnFound As Boolean = False
        Dim strFrm As String = ""

        strFrm = clsSVT.getDataStringValue("Select DocShForm From UtlTransactions where DocID = " & Val(argPartyTypeID), "", False)

        If strFrm = "" Then Exit Sub

        If argPartyColName = "" Then
            argPartyColName = strFrm & "ID"
        End If

        If argSavedData = False Then
            strSQL = "Select * from Mst" & strFrm & " where " & strFrm & "ID =" & Val(argTrnID)
        Else
            strSQL = "Select t.*, " & strFrm & "Code," & strFrm & "Address from Mst" & strFrm & " m, " & argTrnHdrTbl & " t where m." & strFrm & "ID = t." & argPartyColName & " and t." & argTrnIdColName & "=" & Val(argTrnID)
        End If

        rs = clsSVT.getDataReaderFromSQL(strSQL)

        If Not rs.HasRows Then
            rs.Close()
            Exit Sub
        End If

        blnValidate = False
        Call ClearFlds()

        Do While rs.Read
            If argSavedData = False Then
                txtCode.Text = GetVal(rs.Item(strFrm & "Code"), "")
                txtName.Text = GetVal(rs.Item(strFrm & "Name"), "")
                txtID.Text = GetVal(rs.Item(strFrm & "ID"), 0)
                txtAdd1.Text = GetVal(rs!Badd1, "")
                txtAdd2.Text = GetVal(rs!Badd2, "")
                txtAdd3.Text = GetVal(rs!Badd3, "")
                txtAdd4.Text = GetVal(rs!Badd4, "")
                cboCity.Text = GetVal(rs!CITY, "")
                txtPincode.Text = GetVal(rs!PINCODE, "")
                cboState.Text = GetVal(rs!STATE, "")
                cboCountry.Text = GetVal(rs!country, "")
                txtAddr.Text = GetVal(rs.Item(strFrm & "Name"), "") & vbNewLine & GetVal(rs.Item(strFrm & "Address"), "")
            Else
                txtCode.Text = GetVal(rs.Item(strFrm & "Code"), "")
                txtName.Text = GetVal(rs.Item("PartyName"), "")
                txtID.Text = GetVal(rs.Item(argPartyColName), 0)
                txtAdd1.Text = GetVal(rs!PartyAdd1, "")
                txtAdd2.Text = GetVal(rs!PartyAdd2, "")
                txtAdd3.Text = GetVal(rs!PartyAdd3, "")
                txtAdd4.Text = GetVal(rs!PartyAdd4, "")
                cboCity.Text = GetVal(rs!PartyCITY, "")
                txtPincode.Text = GetVal(rs!PartyPINCODE, "")
                cboState.Text = GetVal(rs!PartySTATE, "")
                cboCountry.Text = GetVal(rs!Partycountry, "")
                txtAddr.Text = GetVal(rs.Item("PartyName"), "") & vbNewLine & GetVal(rs.Item(strFrm & "Address"), "")
            End If
        Loop
        rs.Close()

        If blnShip Then
            txtShipAddID.Text = argShipID
            Call bindCombo(cboShipAddr, "PartyAddID", "AddCat", "Select PartyAddID, AddCat from MstPartyAddress where MstDocId=" & argPartyTypeID & " and partyID = " & Val(txtID.Text) & " order by 1")
            cboShipAddr.SelectedValue = argShipID
        End If

        If blnContact Then
            txtContID.Text = argContID
            Call bindCombo(cboContact, "PartyCID", "CName", "Select PartyCID, CName from MstPartyContacts where MstDocId=" & argPartyTypeID & " and partyID = " & Val(txtID.Text) & " order by 1")
            cboContact.SelectedValue = argContID
        End If

        blnValidate = True
    End Sub

    Public Sub ValidateText(ByVal argType As String, ByVal argSrc As String)
        With vsfView
            If blnValidate = False Then Exit Sub
            If vsfView.Visible = True And vsfView.Rows.Count > 1 Then Exit Sub
            Dim strSQL As String
            Dim blnFound As Boolean, intPartyID As Int32 = 0, strFrm As String

            strFrm = clsSVT.getDataStringValue("Select DocShForm From UtlTransactions where DocID = " & Val(argType), "", False)

            strSQL = "Select " & strFrm & "ID from Mst" & strFrm & " where " & strFrm & "Code ='" & Trim(argSrc) & "'"

            intPartyID = clsSVT.getDataLongValue(strSQL, 0, blnFound)

            If blnFound = False Then
                MsgBox("Selected Party not found in master ", vbCritical, "Invalid Selection")
                txtCode.Focus()
                Exit Sub
            End If

        End With
    End Sub

    Public Sub frmNormalSize()
        Me.Size = New Point(326, 114)
    End Sub

    Public Sub frmReSize()
        Me.Size = New Point(425, 395)
    End Sub

    Public Sub FullSize()
        Me.Size = New Point(425, 395)
    End Sub

    Public Sub LoadAllParty(ByVal argType As String, ByVal argCond As String)
        Dim dt As DataTable
        Dim strSQL As String = ""

        Call frmReSize()

        Dim strFrm As String = strPartyType

        strSQL = "Select Top " & gIntTopNrows & " " & strFrm & "ID," & strFrm & "Code," & strFrm & "Name from Mst" & strFrm
        strSQL = strSQL & " Where InActive = 'N' "
        strSQL = strSQL & argCond
        strSQL = strSQL & " order by 1,2,3"


        If strSQL = "" Then Exit Sub

        With vsfView
            Call showWaitCursor()
            dt = clsSVT.getDATATable(strSQL)
            .DataSource = dt
            Call SetColWidth(vsfView, 100, 100, 100)
            .Cols(0).Visible = False
            .Refresh()
            .Visible = True
            Call showNormalCursor()
        End With
    End Sub

    Public Sub LoadFilterParty(ByVal argSrc As String, ByVal argType As String, ByVal argCond As String)
        Dim dt As DataTable
        Dim strSQL As String = ""

        If argSrc = "" Then
            ClearFlds()
            Call frmNormalSize()
            vsfView.Visible = False
            Exit Sub
        End If
        vsfView.Visible = True
        Call frmReSize()

        Dim strFrm As String = strPartyType

        strSQL = "Select Top " & gIntTopNrows & " " & strFrm & "ID," & strFrm & "Code," & strFrm & "Name from Mst" & strFrm
        strSQL = strSQL & " Where InActive = 'N' "
        strSQL = strSQL & argCond
        strSQL = strSQL & " and(" & strFrm & "Code  like '%" & argSrc & "%' or " & strFrm & "Name like '%" & argSrc & "%')"
        strSQL = strSQL & " order by 1,2,3"

        If strSQL = "" Then Exit Sub

        With vsfView
            Call showWaitCursor()
            dt = clsSVT.getDATATable(strSQL)
            .DataSource = dt
            Call SetColWidth(vsfView, 100, 100, 100)
            .Cols(0).Visible = False
            .Refresh()
            If blnPNP = True AndAlso .Rows.Count <= 1 Then
                .Visible = False
                Call frmNormalSize()
            Else
                .Visible = True
            End If
            Call showNormalCursor()
        End With
    End Sub

    Public Sub LoadFromGrid(ByVal argType As String, Optional ByVal argRow As Integer = -1)

        With vsfView
            If .Rows.Count = 1 Then Exit Sub
            If .Row <= 0 Then Exit Sub

            If argRow = -1 Then
                argRow = .Row
            End If

            Dim rs As OleDb.OleDbDataReader, strSQL As String
            Dim strFrm As String = ""

            strFrm = strPartyType


            strSQL = "Select * from Mst" & strFrm & " where " & strFrm & "ID =" & Val(.GetData(argRow, vPID))

            rs = clsSVT.getDataReaderFromSQL(strSQL)

            If Not rs.HasRows Then
                MsgBox("Selected Party not found in master ", vbCritical, Me.Text)
                txtCode.Focus()
                Exit Sub
            End If

            Call ClearFlds()
            blnValidate = False
            Do While rs.Read
                txtCode.Text = GetVal(rs.Item(strFrm & "Code"), "")
                txtName.Text = GetVal(rs.Item(strFrm & "Name"), "")
                txtID.Text = GetVal(rs.Item(strFrm & "ID"), 0)
                txtAdd1.Text = GetVal(rs!Badd1, "")
                txtAdd2.Text = GetVal(rs!Badd2, "")
                txtAdd3.Text = GetVal(rs!Badd3, "")
                txtAdd4.Text = GetVal(rs!Badd4, "")
                cboCity.Text = GetVal(rs!CITY, "")
                txtPincode.Text = GetVal(rs!PINCODE, "")
                cboState.Text = GetVal(rs!STATE, "")
                cboCountry.Text = GetVal(rs!country, "")
                txtAddr.Text = GetVal(rs.Item(strFrm & "Name"), "") & vbNewLine & GetVal(rs.Item(strFrm & "Address"), "")
            Loop
            rs.Close()

            Dim intDocID As Int32 = 0
            If blnShip Or blnContact Then
                intDocID = clsSVT.getDataLongValue("Select DocID from UtlTransactions where DocShForm ='" & strFrm & "'", 0, False)
            End If

            If blnShip Then
                txtShipAddID.Text = 0
                Call bindCombo(cboShipAddr, "PartyAddID", "AddCat", "Select PartyAddID, AddCat from MstPartyAddress where MstDocId=" & intDocID & " and partyID = " & Val(txtID.Text) & " order by 1")
            End If

            If blnContact Then
                txtContID.Text = 0
                Call bindCombo(cboContact, "PartyCID", "CName", "Select PartyCID, CName from MstPartyContacts where MstDocId=" & intDocID & " and partyID = " & Val(txtID.Text) & " order by 1")
            End If

            vsfView.Visible = True
            blnValidate = False

            Call frmNormalSize()
        End With
    End Sub

    Public Sub fillGrid(ByVal argRow As Integer, ByVal argType As String)
        With vsfView
            If argRow < 0 Then Exit Sub
            If .Rows.Count - 1 < argRow Then Exit Sub

            Dim rs As OleDb.OleDbDataReader, strSQL As String
            Dim strFrm As String

            strFrm = strPartyType

            strSQL = "Select * from Mst" & strFrm & " where " & strFrm & "ID =" & Val(.GetData(argRow, vPID))

            rs = clsSVT.getDataReaderFromSQL(strSQL)

            If Not rs.HasRows Then
                MsgBox("Selected Party not found in master ", vbCritical, Me.Text)
                txtCode.Focus()
                Exit Sub
            End If

            blnValidate = False
            Call ClearFlds()

            Do While rs.Read
                With vsfView
                    txtName.Text = GetVal(rs.Item(strFrm & "Name"), "")
                    txtCode.Text = GetVal(rs.Item(strFrm & "Code"), "")
                    txtID.Text = GetVal(rs.Item(strFrm & "ID"), 0)
                    txtAdd1.Text = GetVal(rs!Badd1, "")
                    txtAdd2.Text = GetVal(rs!Badd2, "")
                    txtAdd3.Text = GetVal(rs!Badd3, "")
                    cboCity.Text = GetVal(rs!CITY, "")
                    txtPincode.Text = GetVal(rs!PINCODE, "")
                    cboState.Text = GetVal(rs!STATE, "")
                    cboCountry.Text = GetVal(rs!COUNTRY, "")
                    txtAddr.Text = GetVal(rs.Item(strFrm & "Name"), "") & vbNewLine & GetVal(rs.Item(strFrm & "Address"), "")
                End With
            Loop
            rs.Close()

            Dim intDocID As Int32 = 0
            If blnShip Or blnContact Then
                intDocID = clsSVT.getDataLongValue("Select DocID from UtlTransactions where DocShForm ='" & strFrm & "'", 0, False)
            End If

            If blnShip Then
                txtShipAddID.Text = 0
                Call bindCombo(cboShipAddr, "PartyAddID", "AddCat", "Select PartyAddID, AddCat from MstPartyAddress where MstDocId=" & intDocID & " and partyID = " & Val(txtID.Text) & " order by 1")
            End If

            If blnContact Then
                txtContID.Text = 0
                Call bindCombo(cboContact, "PartyCID", "CName", "Select PartyCID, CName from MstPartyContacts where MstDocId=" & intDocID & " and partyID = " & Val(txtID.Text) & " order by 1")
            End If

            vsfView.Visible = False
            blnValidate = True

            Call frmNormalSize()
        End With
    End Sub

    Public Sub fillParty(ByVal argPartyID As Integer, ByVal argType As String)

        Dim rs As OleDb.OleDbDataReader, strSQL As String
        Dim strFrm As String

        strFrm = strPartyType
        If argPartyID < 0 Then Exit Sub

        blnValidate = False
        txtID.Text = argPartyID

        strSQL = "Select * from Mst" & strFrm & " where " & strFrm & "ID =" & argPartyID
        Try
            rs = clsSVT.getDataReaderFromSQL(strSQL)
            Call ClearFlds()
            Do While rs.Read
                With vsfView
                    txtName.Text = GetVal(rs.Item(strFrm & "Name"), "")
                    txtCode.Text = GetVal(rs.Item(strFrm & "Code"), "")
                    txtID.Text = GetVal(rs.Item(strFrm & "ID"), 0)
                    txtAdd1.Text = GetVal(rs!Badd1, "")
                    txtAdd2.Text = GetVal(rs!Badd2, "")
                    txtAdd3.Text = GetVal(rs!Badd3, "")
                    cboCity.Text = GetVal(rs!CITY, "")
                    txtPincode.Text = GetVal(rs!PINCODE, "")
                    cboState.Text = GetVal(rs!STATE, "")
                    cboCountry.Text = GetVal(rs!COUNTRY, "")
                    txtAddr.Text = GetVal(rs.Item(strFrm & "Name"), "") & vbNewLine & GetVal(rs.Item(strFrm & "Address"), "")
                End With
            Loop
            rs.Close()

            Dim intDocID As Int32 = 0
            If blnShip Or blnContact Then
                intDocID = clsSVT.getDataLongValue("Select DocID from UtlTransactions where DocShForm ='" & strFrm & "'", 0, False)
            End If

            If blnShip Then
                txtShipAddID.Text = 0
                Call bindCombo(cboShipAddr, "PartyAddID", "AddCat", "Select PartyAddID, AddCat from MstPartyAddress where MstDocId=" & intDocID & " and partyID = " & Val(txtID.Text) & " order by 1")
            End If

            If blnContact Then
                txtContID.Text = 0
                Call bindCombo(cboContact, "PartyCID", "CName", "Select PartyCID, CName from MstPartyContacts where MstDocId=" & intDocID & " and partyID = " & Val(txtID.Text) & " order by 1")
            End If

            vsfView.Visible = False
            blnValidate = True
            Call frmNormalSize()
        Catch
        End Try
    End Sub

    Private Sub txtCode_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles txtCode.KeyPress
        If e.KeyChar = "'" Then
            e.Handled = True
        End If
    End Sub

    Private Sub txtCode_KeyUp(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles txtCode.KeyUp
        If blnTrn = False Then Exit Sub
        Select Case e.KeyCode
            Case Keys.Enter
                RaiseEvent TxtKeyUp("ENTER")
            Case Keys.Down
                blnValidate = False
                If vsfView.Rows.Count > 1 Then
                    vsfView.Row = 1
                    vsfView.Col = 1
                    vsfView.Focus()
                End If
                If Trim(txtCode.Text) <> "" Then
                    RaiseEvent TxtKeyUp(Trim(txtCode.Text))
                Else
                    RaiseEvent TxtKeyUp("DOWN_LIST")
                End If
            Case Keys.Escape
                txtCode.Focus()
                vsfView.Visible = False
                Call frmNormalSize()
            Case Keys.Back
                If Trim(txtCode.Text) = "" Then
                    RaiseEvent TxtKeyUp("DOWN_LIST")
                End If
        End Select
    End Sub

    Private Sub txtCode_TextChanged(sender As Object, e As System.EventArgs) Handles txtCode.TextChanged
        If blnTrn = False Then Exit Sub
        RaiseEvent TxtChanged(Trim(txtCode.Text))
    End Sub

    Private Sub txtCode_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles txtCode.Validating
        If blnTrn = False Then Exit Sub
        If blnPNP = True Then
            txtName.Focus()
            Exit Sub
        End If
        If Trim(txtCode.Text) = "" Then Exit Sub
        RaiseEvent TxtValidation(Trim(txtCode.Text))
    End Sub

    Private Sub vsfView_DoubleClick(sender As Object, e As System.EventArgs) Handles vsfView.DoubleClick
        RaiseEvent vsfDoubleClick(vsfView.Row)
    End Sub

    Private Sub vsfView_KeyUp(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles vsfView.KeyUp
        If e.KeyCode = Keys.Enter And vsfView.Row > 0 Then
            RaiseEvent vsfKeyUp(vsfView.Row)
            vsfView.Visible = False
            Call frmNormalSize()
            Exit Sub
        End If
        If e.KeyCode = Keys.Escape Then
            txtCode.Focus()
            vsfView.Visible = False
            Call frmNormalSize()
        End If
    End Sub

    Private Sub vsfView_BeforeEdit(sender As Object, e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfView.BeforeEdit
        e.Cancel = True
    End Sub

    Public Sub setFocus()
        txtCode.Focus()
    End Sub

    Private Sub vsfView_Leave(sender As Object, e As System.EventArgs) Handles vsfView.Leave
        vsfView.Visible = False
        Call frmNormalSize()
    End Sub

    Private Sub txtPincode_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles txtPincode.KeyPress
        If e.KeyChar = "'" Then
            e.Handled = True
        End If
    End Sub

    Private Sub txtAdd1_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles txtAdd1.KeyPress
        If e.KeyChar = "'" Then
            e.Handled = True
        End If
    End Sub

    Private Sub txtAdd2_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles txtAdd2.KeyPress
        If e.KeyChar = "'" Then
            e.Handled = True
        End If
    End Sub

    Private Sub txtAdd3_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles txtAdd3.KeyPress
        If e.KeyChar = "'" Then
            e.Handled = True
        End If
    End Sub

    Private Sub txtName_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles txtName.KeyPress
        If e.KeyChar = "'" Then
            e.Handled = True
        End If
    End Sub

    Private Sub txtID_TextChanged(sender As Object, e As EventArgs) Handles txtID.TextChanged, txtContID.TextChanged
        If Val(txtID.Text) > 0 Then
            Call frmNormalSize()
            RaiseEvent afterPartySelection(Val(txtID.Text))
        End If
    End Sub

    Private Sub lblParty_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles lblParty.MouseDoubleClick
        Dim intDocID As Int32
        Dim PartyID As Int32 = 0
        Dim strMnuName As String

        If Val(txtID.Text) > 0 Then
            PartyID = Val(txtID.Text)
        End If


        If lblParty.Text.ToUpper() = "CUST CODE" Then
            intDocID = clsSVT.getDataLongValue("Select DocID from UtlTransactions where DocShForm = 'CUST'", 0, False)
            strMnuName = "Customer Master"
        Else
            intDocID = clsSVT.getDataLongValue("Select DocID from UtlTransactions where DocShForm = 'SUPP'", 0, False)
            strMnuName = "Supplier Master"
        End If

        Dim frmParty As New frmMstParty
        With frmParty
            .MdiParent = frmMain
            frmParty.initForm(intDocID)
            frmParty.Show()
            If PartyID > 0 Then frmParty.FillDocDetails(PartyID)
            .txtSearch.Focus()
            Exit Sub
        End With
    End Sub

    Private Sub cboShipAddr_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboShipAddr.SelectedIndexChanged
        If Not blnShip OrElse Trim(cboShipAddr.Text) = "" OrElse Not IsNumeric(cboShipAddr.SelectedValue) Then
            txtShipAddID.Text = 0
            GoTo SetID
        End If
        txtShipAddID.Text = cboShipAddr.SelectedValue
        If Val(txtShipAddID.Text) = 0 Then
            MsgBox("Address not found in the master!!", MsgBoxStyle.Information, Me.Text)
            GoTo SetID
        Else
            'txtShipAddress.Text = ""
            'Dim strPartyAddress As String = clsSVT.getDataStringValue("Select Add1 + '|' + Add2 + '|' + Add3 + '|' + Add4 + '|' + City + ' ' + Pincode + '|' + State + ' ' + Country  from MstPartyAddress Where PartyAddID = " & Val(txtShipAddID.Text), "", False)
            'For Each str As String In strPartyAddress.Split("|")
            '    txtShipAddress.Text = txtShipAddress.Text & IIf(str <> "", str & vbNewLine, "")
            'Next
        End If
SetID:
        RaiseEvent cboSelectedValueChange(Val(txtShipAddID.Text))
    End Sub

    Private Sub cboContact_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboContact.SelectedIndexChanged
        If Not blnContact OrElse Trim(cboContact.Text) = "" OrElse Not IsNumeric(cboContact.SelectedValue) Then
            txtContID.Text = 0
            GoTo SetID
        End If
        txtContID.Text = cboContact.SelectedValue
        If Val(txtContID.Text) = 0 Then
            MsgBox("Contact not found in the master!!", MsgBoxStyle.Information, Me.Text)
            GoTo SetID
        End If
SetID:
        RaiseEvent cboSelectedValueChange(Val(txtContID.Text))
    End Sub

    Private Sub btnShip_Click(sender As Object, e As EventArgs) Handles btnShip.Click
        If Val(txtShipAddID.Text) <= 0 Then
            MsgBox("Shipping Address Empty.....", MsgBoxStyle.Information, "")
            Exit Sub
        End If
        Dim strShipAddress As String = ""
        Dim strPartyAddress As String = clsSVT.getDataStringValue("Select Add1 + '|' + Add2 + '|' + Add3 + '|' + Add4 + '|' + City + ' ' + Pincode + '|' + State + ' ' + Country  from MstPartyAddress Where PartyAddID = " & Val(txtShipAddID.Text), "", False)
        For Each str As String In strPartyAddress.Split("|")
            strShipAddress = strShipAddress & IIf(str <> "", str & vbNewLine, "")
        Next
        TransferPlainText = strShipAddress
        'frmUtlShowText.ShowDialog()
    End Sub

    Private Sub tsbCopy_Click(sender As Object, e As EventArgs) Handles tsbCopy.Click
        Clipboard.SetText(Trim(txtAddr.Text))
    End Sub
End Class