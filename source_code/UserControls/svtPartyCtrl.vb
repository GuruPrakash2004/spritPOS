Public Class svtPartyCtrl

    Dim clsSVT As New svtLibrary

    Event TxtChanged(ByVal argValue As String)
    Event TxtKeyUp(ByVal argValue As String)
    Event TxtValidation(ByVal argValue As String)
    Event vsfDoubleClick(ByVal argValue As Int32)
    Event vsfKeyUp(ByVal argValue As Int32)
    Event afterPartySelection(ByVal argPartyID As Long)
    Event LinkClicked(ByVal argType As String, ByVal argID As Int32)

    Dim blnValidate As Boolean = False
    Dim strParty As String = ""
    Dim blnManual As Boolean

    'Dim blnOnlycode As Boolean, blnOnlyBill As Boolean, blnBillShip As Boolean, intPartyDocID As Int32

    Const vPID = 0
    Const vPCode = 1
    Const vPName = 2
    Const vPTot = 3

    Const vDetBill = 0
    Const vDetShip = 1

    Const vDetID = 1
    Const vDetName = 2
    Const vDetAdd1 = 3
    Const vDetAdd2 = 4
    Const vDetAdd3 = 5
    Const vDetAdd4 = 6
    Const vDetCity = 7
    Const vDetPincode = 8
    Const vDetState = 9
    Const vDetCountry = 10
    Const vDetCName = 11
    Const vDetCMob = 12
    Const vDetCDesig = 13
    Const vDetCEmail = 14
    Const vDetTot = 15

    Public Sub ResizeCode()
        txtPartyCode.Size = New Point(200, 22)
    End Sub

    Public Sub frmSize_Code()
        Me.Size = New Point(590, 29)
    End Sub

    Public Sub frmSize_Billing()
        Me.Size = New Point(568, 142)
    End Sub

    Public Sub frmSize_BillShip()
        Me.Size = New Point(568, 142)
    End Sub

    Public Sub frmSize_Expand()
        Me.Size = New Point(590, 420)
    End Sub

    Public Sub TrnGSettings(ByVal argPartyType As String)
        pnlBilling.Visible = False
        pnlShipping.Visible = False
        Select Case argPartyType
            Case "B"
                Call frmSize_Billing()
                pnlBilling.Visible = True
            Case "BS"
                Call frmSize_BillShip()
                pnlBilling.Visible = True
                pnlShipping.Visible = True
            Case "C"
                Call frmSize_Code()
        End Select
        'linkBill.BackColor = Color.Transparent
        'linkShip.BackColor = Color.Transparent
        With vsfDet
            .Rows.Fixed = 1
            .Rows.Count = 1
            .Cols.Count = 2
            .Rows.Count = vDetTot
        End With
    End Sub

    Public Sub SetPartyType(ByVal argDocID As Int32)
        strParty = clsSVT.getDataStringValue("Select DocShForm From UtlTransactions where DocID = " & Val(argDocID), "", False)
        Select Case strParty
            Case "CUST"
                lblParty.Text = "Customer"
                ToolTip1.SetToolTip(btnMaster, "Open Customer Master")
            Case "SUPP"
                lblParty.Text = "Supplier"
                ToolTip1.SetToolTip(btnMaster, "Open Supplier Master")
            Case ""
                btnMaster.Visible = False
                lblParty.Text = ""
                strParty = ""
                blnManual = True
        End Select
        lblParty.ForeColor = Color.Maroon
    End Sub

    Public Sub EnableDisable(ByVal argBln As Boolean)
        Lock(txtPartyCode, argBln)
        Lock(lblBill, argBln)
        Lock(lblShp, argBln)
        lblShp.Visible = argBln
        lblBill.Visible = argBln
        lblBill.BackColor = Color.Transparent
        lblShp.BackColor = Color.Transparent
    End Sub

    Public Sub ClearFlds()
        blnValidate = False
        txtPartyCode.Text = ""
        lblBillAdd1.Text = ""
        lblBillAdd2.Text = ""
        lblBillAdd3.Text = ""
        lblBillCName.Text = ""
        lblBillMob.Text = ""
        lblBillPSCC.Text = ""
        lblShipAdd1.Text = ""
        lblShipAdd2.Text = ""
        lblShipAdd3.Text = ""
        lblShipCName.Text = ""
        lblShipMob.Text = ""
        lblShipPSCC.Text = ""
        txtBillID.Text = 0
        With vsfDet
            For i As Int32 = 1 To .Rows.Count - 1
                .SetData(i, vDetBill, "")
                .SetData(i, vDetShip, "")
            Next
        End With
        blnValidate = True
    End Sub

    Public Function getBillID() As String
        Return Trim(txtBillID.Text)
    End Function

    Public Function getCode() As String
        Return Trim(txtPartyCode.Text)
    End Function

    Public Sub vsfVisisble(ByVal argBln As Boolean)
        vsfView.Visible = argBln
    End Sub

    Public Sub setCode(ByVal argValue As String)
        blnValidate = False
        txtPartyCode.Text = argValue
        blnValidate = True
    End Sub

    Public Function getBillAddress() As String
        Return lblBillCName.Text
    End Function

    Public Function getShipAddress() As String
        Return lblShipCName.Text
    End Function

    Public Sub UpdateFromGrid()
        With vsfDet
            blnValidate = False

            Dim strBillAddress As String
            strBillAddress = .GetData(vDetName, vDetBill)
            strBillAddress &= IIf(.GetData(vDetAdd1, vDetBill) = "", "", vbNewLine & .GetData(vDetAdd1, vDetBill))
            strBillAddress &= IIf(.GetData(vDetAdd2, vDetBill) = "", "", vbNewLine & .GetData(vDetAdd2, vDetBill))
            strBillAddress &= IIf(.GetData(vDetAdd3, vDetBill) = "", "", vbNewLine & .GetData(vDetAdd3, vDetBill))
            strBillAddress &= IIf(.GetData(vDetAdd4, vDetBill) = "", "", " " & .GetData(vDetAdd4, vDetBill))
            strBillAddress &= IIf(.GetData(vDetCity, vDetBill) = "", "", vbNewLine & .GetData(vDetCity, vDetBill))
            strBillAddress &= IIf(.GetData(vDetPincode, vDetBill) = "", "", " " & .GetData(vDetPincode, vDetBill))
            strBillAddress &= IIf(.GetData(vDetState, vDetBill) = "", "", " " & .GetData(vDetState, vDetBill))
            strBillAddress &= IIf(.GetData(vDetCountry, vDetBill) = "", "", " " & .GetData(vDetCountry, vDetBill))
            strBillAddress &= IIf(.GetData(vDetCName, vDetBill) = "", "", vbNewLine & .GetData(vDetCName, vDetBill))
            strBillAddress &= IIf(.GetData(vDetCMob, vDetBill) = "", "", " " & .GetData(vDetCMob, vDetBill))
            lblBillCName.Text = strBillAddress

            Dim strShipAddress As String
            strShipAddress = .GetData(vDetName, vDetShip)
            strShipAddress &= IIf(.GetData(vDetAdd1, vDetShip) = "", "", vbNewLine & .GetData(vDetAdd1, vDetShip))
            strShipAddress &= IIf(.GetData(vDetAdd2, vDetShip) = "", "", vbNewLine & .GetData(vDetAdd2, vDetShip))
            strShipAddress &= IIf(.GetData(vDetAdd3, vDetShip) = "", "", vbNewLine & .GetData(vDetAdd3, vDetShip))
            strShipAddress &= IIf(.GetData(vDetAdd4, vDetShip) = "", "", " " & .GetData(vDetAdd4, vDetShip))
            strShipAddress &= IIf(.GetData(vDetCity, vDetShip) = "", "", vbNewLine & .GetData(vDetCity, vDetShip))
            strShipAddress &= IIf(.GetData(vDetPincode, vDetShip) = "", "", " " & .GetData(vDetPincode, vDetShip))
            strShipAddress &= IIf(.GetData(vDetState, vDetShip) = "", "", " " & .GetData(vDetState, vDetShip))
            strShipAddress &= IIf(.GetData(vDetCountry, vDetShip) = "", "", " " & .GetData(vDetCountry, vDetShip))
            strShipAddress &= IIf(.GetData(vDetCName, vDetShip) = "", "", vbNewLine & .GetData(vDetCName, vDetShip))
            strShipAddress &= IIf(.GetData(vDetCMob, vDetShip) = "", "", " " & .GetData(vDetCMob, vDetShip))
            lblShipCName.Text = strShipAddress
            txtBillID.Text = .GetData(vDetID, vDetBill)
            blnValidate = True
        End With
    End Sub

    Public Sub setData(ByVal argTrnID As Int32,
                       ByVal argPartyTypeID As String,
                       Optional ByVal argTrnHdrTbl As String = "",
                       Optional argTrnIdColName As String = "",
                       Optional argPartyColName As String = "",
                       Optional argShipID As Int32 = 0,
                       Optional argContID As Int32 = 0,
                       Optional argSavedData As Boolean = False)

        If argTrnID < 0 Then Exit Sub

        Dim rs As oledb.oledbDataReader, strSQL As String
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
                txtPartyCode.Text = GetVal(rs.Item(strFrm & "Code"), "")
                'txtName.Text = GetVal(rs.Item(strFrm & "Name"), "")
                'txtID.Text = GetVal(rs.Item(strFrm & "ID"), 0)
                'txtAdd1.Text = GetVal(rs!Badd1, "")
                'txtAdd2.Text = GetVal(rs!Badd2, "")
                'txtAdd3.Text = GetVal(rs!Badd3, "")
                'txtAdd4.Text = GetVal(rs!Badd4, "")
                'cboCity.Text = GetVal(rs!CITY, "")
                'txtPincode.Text = GetVal(rs!PINCODE, "")
                'cboState.Text = GetVal(rs!STATE, "")
                'cboCountry.Text = GetVal(rs!country, "")
                'txtAddr.Text = GetVal(rs.Item(strFrm & "Name"), "") & vbNewLine & GetVal(rs.Item(strFrm & "Address"), "")
            Else
                txtPartyCode.Text = GetVal(rs.Item(strFrm & "Code"), "")
                'txtName.Text = GetVal(rs.Item("PartyName"), "")
                'txtID.Text = GetVal(rs.Item(argPartyColName), 0)
                'txtAdd1.Text = GetVal(rs!PartyAdd1, "")
                'txtAdd2.Text = GetVal(rs!PartyAdd2, "")
                'txtAdd3.Text = GetVal(rs!PartyAdd3, "")
                'txtAdd4.Text = GetVal(rs!PartyAdd4, "")
                'cboCity.Text = GetVal(rs!PartyCITY, "")
                'txtPincode.Text = GetVal(rs!PartyPINCODE, "")
                'cboState.Text = GetVal(rs!PartySTATE, "")
                'cboCountry.Text = GetVal(rs!Partycountry, "")
                'txtAddr.Text = GetVal(rs.Item("PartyName"), "") & vbNewLine & GetVal(rs.Item(strFrm & "Address"), "")
            End If
        Loop
        rs.Close()

        blnValidate = True
    End Sub

    Public Sub ValidateText(ByVal argPartyType As String, ByVal argPartyCode As String, Optional ByVal argEdit As Boolean = False)
        With vsfView

            If blnValidate = False Then Exit Sub
            If vsfView.Visible = True And vsfView.Rows.Count > 1 Then Exit Sub
            Dim strSQL As String
            Dim blnFound As Boolean, intPartyID As Int32 = 0

            If argEdit = True Then Exit Sub

            strSQL = "Select " & argPartyType & "ID from Mst" & argPartyType & " where " & argPartyType & "Code ='" & argPartyCode & "'"

            intPartyID = clsSVT.getDataLongValue(strSQL, 0, blnFound)

            If blnFound = False Then
                MsgBox("Selected Party not found in master ", vbCritical, "Invalid Selection")
                txtPartyCode.Focus()
                Exit Sub
            End If

            txtBillID.Text = intPartyID

        End With
    End Sub

    Public Sub LoadAllParty(ByVal argPartyType As String, ByVal argCond As String, ByVal argFlds As String)
        Dim dt As DataTable
        Dim strSQL As String = ""


        Call frmSize_Expand()

        strSQL = "Select Top " & gIntTopNrows & " " & argPartyType & "ID," & argPartyType & "Code as Code," & argPartyType & "Name as Name "
        If argFlds <> "" Then
            strSQL = strSQL & "," & argFlds
        End If
        strSQL = strSQL & " from Mst" & argPartyType
        strSQL = strSQL & " Where InActive = 'N' "
        strSQL = strSQL & argCond
        strSQL = strSQL & " order by 2"

        With vsfView
            Call showWaitCursor()
            dt = clsSVT.getDATATable(strSQL)
            .DataSource = dt
            Call SetColWidth(vsfView, 100, 100, 100)
            .Cols(0).Visible = False
            .Refresh()
            .Visible = True
        End With
    End Sub

    Public Sub LoadFilterParty(ByVal argPartyType As String, ByVal argCode As String, ByVal argCond As String, ByVal argFlds As String)
        Dim dt As DataTable
        Dim strSQL As String = ""
        vsfView.Visible = True
        Call frmSize_Expand()

        strSQL = "Select Top " & gIntTopNrows & " " & argPartyType & "ID," & argPartyType & "Code as Code," & argPartyType & "Name as Name "
        If argFlds <> "" Then
            strSQL = strSQL & "," & argFlds
        End If
        strSQL = strSQL & " from Mst" & argPartyType
        strSQL = strSQL & " Where InActive = 'N' "
        strSQL = strSQL & argCond
        strSQL = strSQL & " and(" & argPartyType & "Code  like '%" & argCode & "%' or " & argPartyType & "Name like '%" & argCode & "%')"
        strSQL = strSQL & " order by 2"

        If strSQL = "" Then Exit Sub

        With vsfView
            Call showWaitCursor()
            dt = clsSVT.getDATATable(strSQL)
            .DataSource = dt
            'Call SetColWidth(vsfView, 100, 100, 100)
            .Cols(0).Visible = False
            .Refresh()
            Call showNormalCursor()
        End With
    End Sub

    Public Sub LoadFromGrid(ByVal argPartyType As String, ByVal argPartyDocID As Int32, ByVal argBlnCode As Boolean, ByVal argBlnShip As Boolean, ByVal argBlnBill As Boolean, Optional ByVal argRow As Integer = -1)
        With vsfView
            If .Rows.Count = 1 Then Exit Sub
            If .Row <= 0 Then Exit Sub

            If argRow = -1 Then
                argRow = .Row
            End If

            Dim rs As oledb.oledbDataReader, strSQL As String

            strSQL = "Select * from Mst" & argPartyType & " where " & argPartyType & "ID =" & Val(.GetData(argRow, vPID))

            rs = clsSVT.getDataReaderFromSQL(strSQL)

            If Not rs.HasRows Then
                MsgBox("Selected Party not found in master ", vbCritical, Me.Text)
                txtPartyCode.Focus()
                vsfVisisble(True)
                blnValidate = True
                rs.Close()
                GoTo exitRecord
            End If

            Call ClearFlds()
            blnValidate = False
            rs.Read()
            txtPartyCode.Text = GetVal(rs.Item(argPartyType & "Code"), "")
            With vsfDet
                .SetData(vDetID, vDetBill, GetVal(rs.Item(argPartyType & "ID"), 0))
                .SetData(vDetName, vDetBill, GetVal(rs.Item(argPartyType & "Name"), ""))
                .SetData(vDetAdd1, vDetBill, GetVal(rs!Badd1, ""))
                .SetData(vDetAdd2, vDetBill, GetVal(rs!Badd2, ""))
                .SetData(vDetAdd3, vDetBill, GetVal(rs!Badd3, ""))
                .SetData(vDetAdd4, vDetBill, GetVal(rs!Badd4, ""))
                .SetData(vDetCity, vDetBill, GetVal(rs!CITY, ""))
                .SetData(vDetPincode, vDetBill, GetVal(rs!PINCODE, ""))
                .SetData(vDetState, vDetBill, GetVal(rs!STATE, ""))
                .SetData(vDetCountry, vDetBill, GetVal(rs!country, ""))
                .SetData(vDetCName, vDetBill, "")
                .SetData(vDetCMob, vDetBill, "")
                .SetData(vDetCDesig, vDetBill, "")
                .SetData(vDetCEmail, vDetBill, "")

                .SetData(vDetID, vDetShip, 0)
                .SetData(vDetName, vDetShip, "")
                .SetData(vDetAdd1, vDetShip, "")
                .SetData(vDetAdd2, vDetShip, "")
                .SetData(vDetAdd3, vDetShip, "")
                .SetData(vDetAdd4, vDetShip, "")
                .SetData(vDetCity, vDetShip, "")
                .SetData(vDetPincode, vDetShip, "")
                .SetData(vDetState, vDetShip, "")
                .SetData(vDetCountry, vDetShip, "")
                .SetData(vDetCName, vDetShip, "")
                .SetData(vDetCMob, vDetShip, "")
                .SetData(vDetCDesig, vDetShip, "")
                .SetData(vDetCEmail, vDetShip, "")
            End With
            rs.Close()

            strSQL = "Select TOP 1 * from MstPartyContacts Where SetDefault = 'Y' and MstDocID= " & argPartyDocID & " and PartyID =" & Val(.GetData(argRow, vPID))
            rs = clsSVT.getDataReaderFromSQL(strSQL)

            If Not rs.HasRows Then
                With vsfDet
                    .SetData(vDetCName, vDetBill, "")
                    .SetData(vDetCMob, vDetBill, "")
                    .SetData(vDetCDesig, vDetBill, "")
                    .SetData(vDetCEmail, vDetBill, "")
                    rs.Close()
                    GoTo exitContactDtl
                End With
            End If

            rs.Read()
            With vsfDet
                .SetData(vDetCName, vDetBill, GetVal(rs.Item("CNAME"), ""))
                .SetData(vDetCMob, vDetBill, GetVal(rs.Item("CMOBNO"), ""))
                .SetData(vDetCDesig, vDetBill, GetVal(rs.Item("CDESIG"), ""))
                .SetData(vDetCEmail, vDetBill, GetVal(rs.Item("EMAIL"), ""))
            End With
            rs.Close()
exitContactDtl:
            strSQL = "Select TOP 1 * from MstPartyAddress Where DefShipAdd = 'Y' and MstDocID= " & argPartyDocID & " and PartyID =" & Val(.GetData(argRow, vPID))

            rs = clsSVT.getDataReaderFromSQL(strSQL)

            If Not rs.HasRows Then
                With vsfDet
                    .SetData(vDetName, vDetShip, GetVal(.GetData(vDetName, vDetBill), ""))
                    .SetData(vDetAdd1, vDetShip, GetVal(.GetData(vDetAdd1, vDetBill), ""))
                    .SetData(vDetAdd2, vDetShip, GetVal(.GetData(vDetAdd2, vDetBill), ""))
                    .SetData(vDetAdd3, vDetShip, GetVal(.GetData(vDetAdd3, vDetBill), ""))
                    .SetData(vDetAdd4, vDetShip, GetVal(.GetData(vDetAdd4, vDetBill), ""))
                    .SetData(vDetCity, vDetShip, GetVal(.GetData(vDetCity, vDetBill), ""))
                    .SetData(vDetPincode, vDetShip, GetVal(.GetData(vDetPincode, vDetBill), ""))
                    .SetData(vDetState, vDetShip, GetVal(.GetData(vDetState, vDetBill), ""))
                    .SetData(vDetCountry, vDetShip, GetVal(.GetData(vDetCountry, vDetBill), ""))
                    rs.Close()
                    GoTo exitShipAdd
                End With
            End If

            rs.Read()
            With vsfDet
                .SetData(vDetName, vDetShip, GetVal(rs.Item("AddCat"), ""))
                .SetData(vDetAdd1, vDetShip, GetVal(rs.Item("Add1"), ""))
                .SetData(vDetAdd2, vDetShip, GetVal(rs.Item("Add2"), ""))
                .SetData(vDetAdd3, vDetShip, GetVal(rs.Item("Add3"), ""))
                .SetData(vDetAdd4, vDetShip, GetVal(rs.Item("Add4"), ""))
                .SetData(vDetCity, vDetShip, GetVal(rs.Item("City"), ""))
                .SetData(vDetPincode, vDetShip, GetVal(rs!PINCODE, ""))
                .SetData(vDetState, vDetShip, GetVal(rs!STATE, ""))
                .SetData(vDetCountry, vDetShip, GetVal(rs!country, ""))
            End With
            rs.Close()

exitShipAdd:
            With vsfDet
                .SetData(vDetCName, vDetShip, .GetData(vDetCName, vDetBill))
                .SetData(vDetCMob, vDetShip, .GetData(vDetCMob, vDetBill))
                .SetData(vDetCDesig, vDetShip, .GetData(vDetCDesig, vDetBill))
                .SetData(vDetCEmail, vDetShip, .GetData(vDetCEmail, vDetBill))
            End With
            Call UpdateFromGrid()

            If argBlnCode Then
                Call frmSize_Code()
            End If
            If argBlnBill Then
                Call frmSize_Billing()
            End If
            If argBlnShip Then
                Call frmSize_BillShip()
            End If
            blnValidate = True
exitRecord:
        End With
    End Sub

    Public Sub fillGrid(ByVal argRow As Integer, ByVal argPartyType As String, ByVal argBlnBill As Boolean, ByVal argBlnShip As Boolean, ByVal argBlnCode As Boolean, ByVal argPartyDocID As Int32)
        With vsfView
            If argRow < 0 Then Exit Sub
            If .Rows.Count - 1 < argRow Then Exit Sub

            Dim rs As oledb.oledbDataReader, strSQL As String

            strSQL = "Select * from Mst" & argPartyType & " where " & argPartyType & "ID =" & Val(.GetData(argRow, vPID))

            rs = clsSVT.getDataReaderFromSQL(strSQL)

            If Not rs.HasRows Then
                MsgBox("Selected Party not found in master ", vbCritical, Me.Text)
                txtPartyCode.Focus()
                vsfVisisble(True)
                blnValidate = True
                GoTo exitRecord
            End If

            Call ClearFlds()
            blnValidate = False
            rs.Read()
            txtPartyCode.Text = GetVal(rs.Item(argPartyType & "Code"), "")
            With vsfDet
                .SetData(vDetID, vDetBill, GetVal(rs.Item(argPartyType & "ID"), 0))
                .SetData(vDetName, vDetBill, GetVal(rs.Item(argPartyType & "Name"), ""))
                .SetData(vDetAdd1, vDetBill, GetVal(rs!Badd1, ""))
                .SetData(vDetAdd2, vDetBill, GetVal(rs!Badd2, ""))
                .SetData(vDetAdd3, vDetBill, GetVal(rs!Badd3, ""))
                .SetData(vDetAdd4, vDetBill, GetVal(rs!Badd4, ""))
                .SetData(vDetCity, vDetBill, GetVal(rs!CITY, ""))
                .SetData(vDetPincode, vDetBill, GetVal(rs!PINCODE, ""))
                .SetData(vDetState, vDetBill, GetVal(rs!STATE, ""))
                .SetData(vDetCountry, vDetBill, GetVal(rs!country, ""))
                .SetData(vDetCName, vDetBill, "")
                .SetData(vDetCMob, vDetBill, "")
                .SetData(vDetCDesig, vDetBill, "")
                .SetData(vDetCEmail, vDetBill, "")

                .SetData(vDetID, vDetShip, 0)
                .SetData(vDetName, vDetShip, "")
                .SetData(vDetAdd1, vDetShip, "")
                .SetData(vDetAdd2, vDetShip, "")
                .SetData(vDetAdd3, vDetShip, "")
                .SetData(vDetAdd4, vDetShip, "")
                .SetData(vDetCity, vDetShip, "")
                .SetData(vDetPincode, vDetShip, "")
                .SetData(vDetState, vDetShip, "")
                .SetData(vDetCountry, vDetShip, "")
                .SetData(vDetCName, vDetShip, "")
                .SetData(vDetCMob, vDetShip, "")
                .SetData(vDetCDesig, vDetShip, "")
                .SetData(vDetCEmail, vDetShip, "")
            End With
            rs.Close()

            strSQL = "Select TOP 1 * from MstPartyContacts Where SetDefault = 'Y' and MstDocID= " & argPartyDocID & " and PartyID =" & Val(.GetData(argRow, vPID))
            rs = clsSVT.getDataReaderFromSQL(strSQL)

            If Not rs.HasRows Then
                With vsfDet
                    .SetData(vDetCName, vDetBill, "")
                    .SetData(vDetCMob, vDetBill, "")
                    .SetData(vDetCDesig, vDetBill, "")
                    .SetData(vDetCEmail, vDetBill, "")
                    rs.Close()
                    GoTo exitContactDtl
                End With
            End If

            rs.Read()
            With vsfDet
                .SetData(vDetCName, vDetBill, GetVal(rs.Item("CNAME"), ""))
                .SetData(vDetCMob, vDetBill, GetVal(rs.Item("CMOBNO"), ""))
                .SetData(vDetCDesig, vDetBill, GetVal(rs.Item("CDESIG"), ""))
                .SetData(vDetCEmail, vDetBill, GetVal(rs.Item("EMAIL"), ""))
            End With
            rs.Close()
exitContactDtl:
            strSQL = "Select TOP 1 * from MstPartyAddress Where DefShipAdd = 'Y' and MstDocID= " & argPartyDocID & " and PartyID =" & Val(.GetData(argRow, vPID))

            rs = clsSVT.getDataReaderFromSQL(strSQL)

            If Not rs.HasRows Then
                With vsfDet
                    .SetData(vDetName, vDetShip, GetVal(.GetData(vDetName, vDetBill), ""))
                    .SetData(vDetAdd1, vDetShip, GetVal(.GetData(vDetAdd1, vDetBill), ""))
                    .SetData(vDetAdd2, vDetShip, GetVal(.GetData(vDetAdd2, vDetBill), ""))
                    .SetData(vDetAdd3, vDetShip, GetVal(.GetData(vDetAdd3, vDetBill), ""))
                    .SetData(vDetAdd4, vDetShip, GetVal(.GetData(vDetAdd4, vDetBill), ""))
                    .SetData(vDetCity, vDetShip, GetVal(.GetData(vDetCity, vDetBill), ""))
                    .SetData(vDetPincode, vDetShip, GetVal(.GetData(vDetPincode, vDetBill), ""))
                    .SetData(vDetState, vDetShip, GetVal(.GetData(vDetState, vDetBill), ""))
                    .SetData(vDetCountry, vDetShip, GetVal(.GetData(vDetCountry, vDetBill), ""))
                    rs.Close()
                    GoTo exitShipAdd
                End With
            End If

            rs.Read()
            With vsfDet
                .SetData(vDetName, vDetShip, GetVal(rs.Item("AddCat"), ""))
                .SetData(vDetAdd1, vDetShip, GetVal(rs.Item("Add1"), ""))
                .SetData(vDetAdd2, vDetShip, GetVal(rs.Item("Add2"), ""))
                .SetData(vDetAdd3, vDetShip, GetVal(rs.Item("Add3"), ""))
                .SetData(vDetAdd4, vDetShip, GetVal(rs.Item("Add4"), ""))
                .SetData(vDetCity, vDetShip, GetVal(rs.Item("City"), ""))
                .SetData(vDetPincode, vDetShip, GetVal(rs!PINCODE, ""))
                .SetData(vDetState, vDetShip, GetVal(rs!STATE, ""))
                .SetData(vDetCountry, vDetShip, GetVal(rs!country, ""))
            End With
            rs.Close()

exitShipAdd:
            With vsfDet
                .SetData(vDetCName, vDetShip, .GetData(vDetCName, vDetBill))
                .SetData(vDetCMob, vDetShip, .GetData(vDetCMob, vDetBill))
                .SetData(vDetCDesig, vDetShip, .GetData(vDetCDesig, vDetBill))
                .SetData(vDetCEmail, vDetShip, .GetData(vDetCEmail, vDetBill))
            End With

            Call UpdateFromGrid()

            If argBlnCode Then
                Call frmSize_Code()
            End If
            If argBlnBill Then
                Call frmSize_Billing()
            End If
            If argBlnShip Then
                Call frmSize_BillShip()
            End If
            blnValidate = True
exitRecord:
        End With
    End Sub

    Private Sub txtCode_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles txtPartyCode.KeyPress
        If e.KeyChar = "'" Then
            e.Handled = True
        End If
    End Sub

    Private Sub txtCode_KeyUp(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles txtPartyCode.KeyUp
        Select Case e.KeyCode
            Case Keys.Enter
                RaiseEvent TxtKeyUp("ENTER")
            Case Keys.F4
                RaiseEvent TxtKeyUp("F4")
            Case Keys.Down
                blnValidate = False
                If vsfView.Rows.Count > 1 Then
                    vsfView.Row = 1
                    vsfView.Col = 1
                    vsfView.Focus()
                End If
                If Trim(txtPartyCode.Text) <> "" Then
                    RaiseEvent TxtKeyUp(txtPartyCode.Text)
                Else
                    RaiseEvent TxtKeyUp("DOWN_LIST")
                End If
            Case Keys.Escape
                txtPartyCode.Focus()
                RaiseEvent TxtKeyUp("")
            Case Keys.Back
                If Trim(txtPartyCode.Text) = "" Then
                    RaiseEvent TxtKeyUp("DOWN_LIST")
                End If
        End Select
    End Sub

    Private Sub txtCode_TextChanged(sender As Object, e As System.EventArgs) Handles txtPartyCode.TextChanged
        If blnValidate = False Then Exit Sub
        RaiseEvent TxtChanged(txtPartyCode.Text)
    End Sub

    Private Sub txtCode_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles txtPartyCode.Validating
        If Trim(txtPartyCode.Text) = "" Then Exit Sub
        If blnValidate = False Then Exit Sub
        RaiseEvent TxtValidation(Trim(txtPartyCode.Text))
    End Sub

    Private Sub vsfView_DoubleClick(sender As Object, e As System.EventArgs) Handles vsfView.DoubleClick
        RaiseEvent vsfDoubleClick(vsfView.Row)
    End Sub

    Private Sub vsfView_KeyUp(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles vsfView.KeyUp
        If e.KeyCode = Keys.Enter And vsfView.Row > 0 Then
            RaiseEvent vsfKeyUp(vsfView.Row)
        End If
        If e.KeyCode = Keys.Escape Then
            RaiseEvent vsfKeyUp(-1)
            txtPartyCode.Focus()
            vsfView.Visible = False
        End If
    End Sub

    Public Sub ChangeFormSize(ByVal argBlnBill As Boolean, ByVal argBlnShip As Boolean, ByVal argBlnCode As Boolean)
        If argBlnCode Then
            Call frmSize_Code()
        End If
        If argBlnBill Then
            Call frmSize_Billing()
        End If
        If argBlnShip Then
            Call frmSize_BillShip()
        End If
    End Sub

    Private Sub vsfView_BeforeEdit(sender As Object, e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfView.BeforeEdit
        e.Cancel = True
    End Sub

    Public Sub setFocus()
        txtPartyCode.Focus()
    End Sub

    'Private Sub vsfView_Leave(sender As Object, e As System.EventArgs) Handles vsfView.Leave
    '    vsfView.Visible = False
    '    RaiseEvent vsfKeyUp(-1)
    'End Sub

    Private Sub txtID_TextChanged(sender As Object, e As EventArgs) Handles txtBillID.TextChanged
        If Val(txtBillID.Text) > 0 Then
            RaiseEvent vsfKeyUp(-1)
            RaiseEvent afterPartySelection(Val(txtBillID.Text))
        Else
            RaiseEvent afterPartySelection(-1)
        End If
    End Sub

    Private Sub lblParty_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles lblParty.MouseDoubleClick

    End Sub

    Public Sub SendPartyData(ByVal argPartyType As String, ByVal argPartyID As Int32, ByVal argDocID As Int32, ByVal argBillOrShip As String, ByVal argFrm As Form)
        Dim objFrm As New frmPartyInfo
        With objFrm
            .intDocID = argDocID
            .intPartyID = argPartyID
            .strPartyType = argPartyType
            .objFrm = argFrm
            .strBillOrShip = argBillOrShip
            .lblPartyName.Text = UCase(Mid(argPartyType, 1, 1)) & (Mid(argPartyType, 2)).ToLower & "-Name"
            If argBillOrShip = "B" Then
                .txtPartyID.Text = vsfDet.GetData(vDetID, vDetBill)
                .txtCustName.Text = vsfDet.GetData(vDetName, vDetBill)
                .txtAdd1.Text = vsfDet.GetData(vDetAdd1, vDetBill)
                .txtAdd2.Text = vsfDet.GetData(vDetAdd2, vDetBill)
                .txtAdd3.Text = vsfDet.GetData(vDetAdd3, vDetBill)
                .txtAdd4.Text = vsfDet.GetData(vDetAdd4, vDetBill)
                .cboCity.Text = vsfDet.GetData(vDetCity, vDetBill)
                .cboState.Text = vsfDet.GetData(vDetState, vDetBill)
                .cboCountry.Text = vsfDet.GetData(vDetCountry, vDetBill)
                .txtPinCode.Text = vsfDet.GetData(vDetPincode, vDetBill)
                .txtName.Text = vsfDet.GetData(vDetCName, vDetBill)
                .txtEmail.Text = vsfDet.GetData(vDetCEmail, vDetBill)
                .txtMobile.Text = vsfDet.GetData(vDetCMob, vDetBill)
                .txtDesig.Text = vsfDet.GetData(vDetCDesig, vDetBill)
                .lblBillAddress.Text = "Billing Address"
            Else
                .txtPartyID.Text = vsfDet.GetData(vDetID, vDetBill)
                .txtCustName.Text = vsfDet.GetData(vDetName, vDetShip)
                .txtAdd1.Text = vsfDet.GetData(vDetAdd1, vDetShip)
                .txtAdd2.Text = vsfDet.GetData(vDetAdd2, vDetShip)
                .txtAdd3.Text = vsfDet.GetData(vDetAdd3, vDetShip)
                .txtAdd4.Text = vsfDet.GetData(vDetAdd4, vDetShip)
                .cboCity.Text = vsfDet.GetData(vDetCity, vDetShip)
                .cboState.Text = vsfDet.GetData(vDetState, vDetShip)
                .cboCountry.Text = vsfDet.GetData(vDetCountry, vDetShip)
                .txtPinCode.Text = vsfDet.GetData(vDetPincode, vDetShip)
                .txtName.Text = vsfDet.GetData(vDetCName, vDetShip)
                .txtEmail.Text = vsfDet.GetData(vDetCEmail, vDetShip)
                .txtMobile.Text = vsfDet.GetData(vDetCMob, vDetShip)
                .txtDesig.Text = vsfDet.GetData(vDetCDesig, vDetShip)
                .lblBillAddress.Text = "Shipping Address"
            End If
            If argPartyType = "CUST" Then
                .Text = "Edit Customer Address"
            Else
                .Text = "Edit Supplier Address"
            End If
            .ShowDialog()
        End With
    End Sub

    Public Sub setGridData(ByVal argRow As Int32, ByVal argCol As Int32, ByVal argValue As String)
        vsfDet.SetData(argRow, argCol, argValue)
    End Sub

    Public Function getGridData(ByVal argRow As Int32, ByVal argCol As Int32) As String
        Return vsfDet.GetData(argRow, argCol)
    End Function

    Private Sub btnMaster_Click(sender As Object, e As EventArgs) Handles btnMaster.Click
        Dim intDocID As Int32
        Dim PartyID As Int32 = 0
        Dim strMnuName As String

        If Val(vsfDet.GetData(vDetID, vDetBill)) > 0 Then
            PartyID = Val(vsfDet.GetData(vDetID, vDetBill))
        End If


        If lblParty.Text.ToUpper() = "CUSTOMER" Then
            intDocID = clsSVT.getDataLongValue("Select DocID from UtlTransactions where DocShForm = 'CUST'", 0, False)
            strMnuName = "Customer Master"
        Else
            intDocID = clsSVT.getDataLongValue("Select DocID from UtlTransactions where DocShForm = 'SUPP'", 0, False)
            strMnuName = "Supplier Master"
        End If


        Dim frmParty As New frmMstParty
        frmParty.initForm(intDocID)
        frmMain.OpenForms(frmParty, strMnuName)
        If PartyID > 0 Then frmParty.FillDocDetails(PartyID)
        frmMain.txtSearch.Focus()

        'Dim frmParty As New frmMstParty
        'With frmParty
        '    .MdiParent = frmMain
        '    frmParty.initForm(intDocID)
        '    frmParty.Show()
        '    If PartyID > 0 Then frmParty.FillDocDetails(PartyID)
        '    .txtSearch.Focus()
        '    Exit Sub
        'End With
    End Sub

    Private Sub lblShp_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles lblShp.LinkClicked
        RaiseEvent LinkClicked("S", Val(txtBillID.Text))
    End Sub

    Private Sub lblBill_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles lblBill.LinkClicked
        RaiseEvent LinkClicked("B", Val(txtBillID.Text))
    End Sub

    Private Sub vsfView_LostFocus(sender As Object, e As EventArgs) Handles vsfView.LostFocus
        'If blnValidate = False Then
        '    blnValidate = True
        '    txtPartyCode.Focus()
        '    Dim obj As Object = Nothing
        '    Call txtCode_Validating(obj, obj)
        'End If
    End Sub

    Private Sub txtPartyCode_PreviewKeyDown(sender As Object, e As PreviewKeyDownEventArgs) Handles txtPartyCode.PreviewKeyDown
        If e.KeyCode = Keys.Tab Then
            RaiseEvent TxtKeyUp("ENTER")
        End If
    End Sub
End Class