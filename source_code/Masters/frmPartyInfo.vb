
Imports System.IO
Imports System.Data.oledb
Imports C1.Win.C1FlexGrid
Imports System.ComponentModel


Public Class frmPartyInfo
    Dim strTblName As String
    Dim strCodeName As String
    Dim strFormCaption As String
    Public intDocID As Integer
    Dim clsSVT As New svtLibrary
    Public intPartyID As Integer
    Public blnNonParty As Boolean
    Public strPartyType As String
    Public strExitingData As String
    Public objFrm As Object
    Public strBillOrShip As String

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

    Private Sub LoadForm()

    End Sub

    Private Sub btnBillSel_Click(sender As Object, e As EventArgs) Handles btnBillSel.Click
        Dim strSQL As String = ""

        txtDummy.Text = ""
        strSQL = "Select PartyAddID, AddCat, Add1, Add2, Add3, Add4, City, Pincode, State, Country from MstPartyAddress "
        strSQL &= " Where MstDocID = " & Val(intDocID)
        If intPartyID > 0 Then
            strSQL &= " and PartyID=" & Val(intPartyID)
        End If

        If intPartyID > 0 Then
            strSQL &= " Union All "
            strSQL &= " Select -1 as PartyAddID, " & strPartyType & "Code as AddCat, BAdd1 as Add1,BAdd2 as Add2, BAdd3 as Add3, BAdd4 as Add4, City, Pincode, State, Country from Mst" & strPartyType
            strSQL &= " Where " & strPartyType & "ID = " & intPartyID
        End If

        Dim frmDisplay As New frmUtlSingleSel
        With frmDisplay
            .Text = "Select Address... "
            Call .FillViewVsf(Me, strSQL, txtDummy, 0, 0)
            .Show()
            .vsfDet.Focus()
        End With
    End Sub

    Private Sub txtDummy_TextChanged(sender As Object, e As EventArgs) Handles txtDummy.TextChanged
        If Trim(txtDummy.Text) = "" Then Exit Sub

        Dim rs As oledb.oledbDataReader
        Dim strSQL As String = ""

        If Val(txtDummy.Text) = -1 Then
            strSQL = "Select  " & strPartyType & "Code as AddCat, BAdd1 as Add1,BAdd2 as Add2, BAdd3 as Add3, BAdd4 as Add4, City, Pincode, State, Country from Mst" & strPartyType
            strSQL &= " Where " & strPartyType & "ID = " & intPartyID
        Else
            strSQL = "Select PartyAddID, AddCat, Add1, Add2, Add3, Add4, City, Pincode, State, Country from MstPartyAddress "
            strSQL &= " Where PartyAddID = " & Val(txtDummy.Text)
        End If


        rs = clsSVT.getDataReaderFromSQL(strSQL)

        If Not rs.HasRows Then
            MsgBox("Invalid Address ID...", MsgBoxStyle.Critical, Me.Text)
            rs.Close()
            Exit Sub
        End If

        rs.Read()

        txtCustName.Text = GetVal(rs.Item("AddCat"), "")
        txtAdd1.Text = GetVal(rs.Item("Add1"), "")
        txtAdd2.Text = GetVal(rs.Item("Add2"), "")
        txtAdd3.Text = GetVal(rs.Item("Add3"), "")
        txtAdd4.Text = GetVal(rs.Item("Add4"), "")
        cboCity.Text = GetVal(rs.Item("City"), "")
        cboState.Text = GetVal(rs.Item("State"), "")
        cboCountry.Text = GetVal(rs.Item("Country"), "")
        txtPinCode.Text = GetVal(rs.Item("Pincode"), "")
        rs.Close()
    End Sub

    Private Sub frmPartyInfo_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyUp
        If e.KeyCode = Keys.F9 And e.Control Then
            Call initFormSize(Me, "Edit Party Address", False, 577, 533, 0, 0)
            MsgBox("Restored Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
        If e.KeyCode = Keys.F8 And e.Control = True Then
            Call SaveActFormSettings(Me, "Edit Party Address")
            MsgBox("Saved Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
    End Sub

    Private Sub frmPartyInfo_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Call initFormSize(Me, "Edit Party Address", False, 577, 533, 0, 0)
        fillFormProperties(Me)
        intPartyID = Val(txtPartyID.Text)
        Dim strCity, strState, strCountry As String
        strCity = cboCity.Text
        strState = cboState.Text
        strCountry = cboCountry.Text
        Call bindCombo(cboCity, "City", "City", "select  distinct City from MStSCC where City <>'' order by city")
        Call bindCombo(cboState, "STATE", "STATE", "select  distinct STATE from MStSCC where state <>''  order by STATE")
        Call bindCombo(cboCountry, "COUNTRY", "COUNTRY", "select  distinct COUNTRY from MStSCC where country  <> '' order by COUNTRY")
        cboCity.Text = strCity
        cboState.Text = strState
        cboCountry.Text = strCountry
        Call ChangeFontSize(Me)

    End Sub

    Private Sub btnContSel_Click(sender As Object, e As EventArgs) Handles btnContSel.Click
        Dim strSQL As String = ""

        txtDummy.Text = ""
        strSQL = "Select PartyCID as ID, CName as ContactPerson, CDesig as Designation, CMobNo as MobileNo, Email, setDefault as DefaultCol from MstPartycontacts "
        strSQL &= " Where MstDocID = " & Val(intDocID)
        If intPartyID > 0 Then
            strSQL &= " and PartyID=" & Val(intPartyID)
        End If

        Dim frmDisplay As New frmUtlSingleSel

        With frmDisplay
            .Text = "Select Contact... "
            .blnDefRow = True
            Call .FillViewVsf(Me, strSQL, txtDummy2, 0, 0)
            .Show()
            .vsfSearch.Focus()
        End With
    End Sub

    Private Sub txtDummy2_TextChanged(sender As Object, e As EventArgs) Handles txtDummy2.TextChanged
        If Trim(txtDummy2.Text) = "" Then Exit Sub
        If Val(txtDummy2.Text) <= 0 Then Exit Sub
        Dim rs As oledb.oledbDataReader
        Dim strSQL As String = ""
        strSQL = "Select PartyCID, CName, CDesig, CMobNo, Email from MstPartycontacts "
        strSQL &= " Where PartyCID = " & Val(txtDummy2.Text)
        rs = clsSVT.getDataReaderFromSQL(strSQL)

        If Not rs.HasRows Then
            MsgBox("Invalid Address ID...", MsgBoxStyle.Critical, Me.Text)
            rs.Close()
            Exit Sub
        End If

        rs.Read()

        txtName.Text = GetVal(rs.Item("CName"), "")
        txtEmail.Text = GetVal(rs.Item("Email"), "")
        txtMobile.Text = GetVal(rs.Item("CMobNo"), "")
        txtDesig.Text = GetVal(rs.Item("CDesig"), "")

        rs.Close()
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        stringReplace(Me)
        With objFrm
            With .svtPCtrl
                If strBillOrShip = "B" Then
                    .setGridData(vDetName, vDetBill, txtCustName.Text)
                    .setGridData(vDetAdd1, vDetBill, txtAdd1.Text)
                    .setGridData(vDetAdd2, vDetBill, txtAdd2.Text)
                    .setGridData(vDetAdd3, vDetBill, txtAdd3.Text)
                    .setGridData(vDetAdd4, vDetBill, txtAdd4.Text)
                    .setGridData(vDetCity, vDetBill, cboCity.Text)
                    .setGridData(vDetState, vDetBill, cboState.Text)
                    .setGridData(vDetPincode, vDetBill, txtPinCode.Text)
                    .setGridData(vDetCountry, vDetBill, cboCountry.Text)
                    .setGridData(vDetCName, vDetBill, txtName.Text)
                    .setGridData(vDetCMob, vDetBill, txtMobile.Text)
                    .setGridData(vDetCEmail, vDetBill, txtEmail.Text)
                    .setGridData(vDetCDesig, vDetBill, txtDesig.Text)
                Else
                    .setGridData(vDetName, vDetShip, txtCustName.Text)
                    .setGridData(vDetAdd1, vDetShip, txtAdd1.Text)
                    .setGridData(vDetAdd2, vDetShip, txtAdd2.Text)
                    .setGridData(vDetAdd3, vDetShip, txtAdd3.Text)
                    .setGridData(vDetAdd4, vDetShip, txtAdd4.Text)
                    .setGridData(vDetCity, vDetShip, cboCity.Text)
                    .setGridData(vDetState, vDetShip, cboState.Text)
                    .setGridData(vDetPincode, vDetShip, txtPinCode.Text)
                    .setGridData(vDetCountry, vDetShip, cboCountry.Text)
                    .setGridData(vDetCName, vDetShip, txtName.Text)
                    .setGridData(vDetCMob, vDetShip, txtMobile.Text)
                    .setGridData(vDetCEmail, vDetShip, txtEmail.Text)
                    .setGridData(vDetCDesig, vDetShip, txtDesig.Text)
                End If
                .UpdateFromGrid()
            End With
        End With
        Me.Close()
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub cboCity_Validating(sender As Object, e As CancelEventArgs) Handles cboCity.Validating
        If cboCity.Text = "" Then
            Exit Sub
        End If
        Dim blnFound As Boolean = False
        Dim intSCCID As Integer
        intSCCID = clsSVT.getDataLongValue("Select SCCID from MstSCC where City='" & cboCity.Text & "'", 0, blnFound)
        If Not blnFound Then
            Dim response As MsgBoxResult
            response = MsgBox("City Not found! Do you want to add city ??", vbYesNo, Me.Text)
            If response = vbYes Then
                cboCity.Text = ""
                Dim intSCCDocID As Integer
                intSCCDocID = clsSVT.getDataLongValue("select DocID from UtlTransactions where DocName='State City Country'", 0, False)
                Dim argfrm As New frmMstSCC
                argfrm.initForm(intSCCDocID, "MstSCC", "SCCID", "City", "State City Country", "City")
                frmMain.OpenForms(argfrm, "State City Country")
            Else
                Exit Sub
            End If
            Exit Sub
        End If
        cboCountry.Text = clsSVT.getDataStringValue("Select Country from MstSCC where SCCID=" & intSCCID, 0, False)
        cboState.Text = clsSVT.getDataStringValue("Select State from MstSCC where SCCID=" & intSCCID, 0, False)
    End Sub
End Class