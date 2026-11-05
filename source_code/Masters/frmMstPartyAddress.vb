Imports System.ComponentModel
Imports System.Data.oledb

Public Class frmMstPartyAddress
    Dim clsSVT As New svtLibrary
    Public Mode As String
    Public strPartyIDFld As String
    Public strfrmCaption As String
    Public strRights As String
    Public intPartyID As Integer
    Dim intDocID As Integer

    Dim strPartyTblName As String
    Dim strFldPartyID As String
    Dim strFldPartyCode As String
    Dim strFldPartyName As String

    Const vdetPartyID = 0
    Dim Strsql As String

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub frmMstAddress_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        KeyPreview = True
        'Call EnableBtn(Mode)
        Call initFormSize(Me, Me.Text, False, 756, 483, 0, 0)
        Call initFormVsfDet(Me, Me.Text, vsfDet, "DET")
        Call ChangeFontSize(Me)
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        fraEdit.Visible = False
        vsfDet.Visible = True

    End Sub
    Private Sub visibleEditbtn(ByVal argBLN As Boolean)
        If argBLN Then
            btnSave.Visible = True
            btnCancel.Visible = True
            btnClose.Visible = True

        Else
            Mode = ""
            btnCancel.Visible = True
            btnSave.Visible = False
            btnClose.Visible = True
        End If
    End Sub

    Private Sub initScreen()
        Dim dt As DataTable
        Dim strSQL As String
        Call bindCombo(cboCity, "City", "City", "select  distinct City from MStSCC where City <>'' order by city")
        Call bindCombo(cboState, "STATE", "STATE", "select  distinct STATE from MStSCC where state <>''  order by STATE")
        Call bindCombo(cboCountry, "COUNTRY", "COUNTRY", "select  distinct COUNTRY from MStSCC where country  <> '' order by COUNTRY")

        If gDBTypeCurrent = "ACCESS" Then
            strSQL = "select partyAddID,AddCat as Category,("
            strSQL &= " iif(len(add1)>0,add1&chr(13)&chr(10),Null) & "
            strSQL &= " iif(len(add2)>0,add2&chr(13)&chr(10),null) & "
            strSQL &= " iif(len(add3)>0,add3&chr(13)&chr(10),null) & "
            strSQL &= " iif(len(add4)>0,add4&chr(13)&chr(10),null) & "
            strSQL &= " iif(len(City)>0,City&chr(13)&chr(10),null) & "
            strSQL &= " iif(len(State)>0,State&chr(13)&chr(10),null) & "
            strSQL &= " iif(len(Pincode)>0,Pincode&chr(13)&chr(10),null) & "
            strSQL &= " iif(len(Country)>0, Country,null)) as address ,DefShipAdd as DefaultShippingAddress"
            strSQL &= " from MstPartyAddress "
            strSQL &= " where PartyID = " & intPartyID
            strSQL &= " and MstDocID = " & intDocID
        Else
            strSQL = " Select partyAddID, AddCat as Category, "
            strSQL &= " (Select Case when IsNull(Add1,'')<>'' then Add1+char(10) else '' end )"
            strSQL &= " + (Select Case when IsNull(Add2,'')<>'' then Add2+char(10) else '' end )"
            strSQL &= " + (Select Case when IsNull(Add3,'')<>'' then Add3+char(10) else '' end)"
            strSQL &= " + (Select Case when IsNull(Add4,'')<>'' then Add4+char(10) else '' end)"
            strSQL &= "+ (Select Case when IsNull(City,'')<>'' then city+char(10) else '' end)"
            strSQL &= " + (Select Case when IsNull(convert(varchar(10),Pincode),'')<>'' then convert(varchar(10),Pincode)+char(10) else '' end)"
            strSQL &= " + (Select Case when IsNull(State,'')<>'' then State+char(10) else '' end )"
            strSQL &= "+ (Select Case when IsNull(Country,'')<>'' then Country+char(10) else '' end )"
            strSQL &= " as Address,DefShipAdd as DefaultShippingAddress "
            strSQL &= " from MstPartyAddress "
            strSQL &= " where PartyID= " & intPartyID
            strSQL &= " and MstDocID = " & intDocID
            strSQL &= " order by AddCat"
        End If


        dt = clsSVT.getDATATable(strSQL)
        vsfDet.DataSource = dt
        fraEdit.Visible = False
        vsfDet.Visible = True
        txtPartyCode.Enabled = False
        txtPartyName.Enabled = False
        SetHideCol(vsfDet, True, 0)
        vsfDet.AutoSizeCols()
        vsfDet.AutoSizeRows()
        'vsfDet.Cols(3).Visible = False
        Call SetColWidth(vsfDet, 300)
        ' Me.Text = strfrmCaption & " Addresses "
        Me.Text = "Multi Addresses "
        lblPartyCode.Text = strfrmCaption & " Code "
        '  lblName.Text = strfrmCaption & " Name "
        initGridStyles(Me)
        Call GetPartyDtls()
        If vsfDet.Rows.Count <= 1 Then
            lblNoofAddress.Text = "No Addresses Present for this " & strfrmCaption
            btnEdit.Enabled = False
            btnDelete.Enabled = False
            btnAdd.Enabled = True
        Else
            lblNoofAddress.Text = "Total Addresses Present for this " & strfrmCaption & " is " & vsfDet.Rows.Count - 1 & ""
            btnEdit.Enabled = True
            btnDelete.Enabled = True
            btnAdd.Enabled = True
        End If
        Call initFormVsfDet(Me, Me.Text, vsfDet, "DET")
        Dim cs As C1.Win.C1FlexGrid.CellStyle = vsfDet.Styles.Add(1)

        Dim blndefFound As Boolean = False
        For i = 1 To vsfDet.Rows.Count - 1
            If GetVal(vsfDet.GetData(i, vsfDet.Cols.Count - 1), "") = "Y" Then
                blndefFound = True
                cs.BackColor = Color.GreenYellow
                vsfDet.SetCellStyle(i, 1, cs)
                Exit For
            End If
        Next
        If Not chkUserRights(strRights, chkRightsPos.AddRights, Me.Text, False) Then btnAdd.Visible = False
        If Not chkUserRights(strRights, chkRightsPos.EditRights, Me.Text, False) Then btnEdit.Visible = False
        If Not chkUserRights(strRights, chkRightsPos.DelRights, Me.Text, False) Then btnDelete.Visible = False
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim strSQL As String

        Dim svtTrn As oledb.oledbTransaction = Nothing

        stringReplace(Me)

        If Not Validfields() Then
            Exit Sub
        End If


        'If chkDefault.Checked = True Then
        '    strSQL = "Update MstPartyAddress set SetDefault='N'  "
        '    'strSQL = strSQL & " where PartyAddID  = " & Val(txtAddID.Text)
        '    strSQL = strSQL & " where MstDocID  = " & intDocID
        '    strSQL = strSQL & " and PartyID  = " & intPartyID
        '    clsSVT.execSQLStatement(strSQL, svtTrn)
        'End If

        If ChkDefShipAd.Checked = True Then
            strSQL = "Update MstPartyAddress set DefShipAdd='N'  "
            'strSQL = strSQL & " where PartyAddID  = " & Val(txtAddID.Text)
            strSQL = strSQL & " where MstDocID  = " & intDocID
            strSQL = strSQL & " and PartyID  = " & intPartyID
            clsSVT.execSQLStatement(strSQL, svtTrn)
        End If

        If Mode = "A" Then
            txtAddID.Text = clsSVT.AutoIncrementedNum("PartyAddId", "MstPartyAddress")
            strSQL = "Insert into MstPartyAddress (MstDocId,PartyID ,AddCat, Add1, Add2, Add3,Add4, City,State,Pincode,Country,DefShipAdd) values ("
            strSQL = strSQL & " " & intDocID & ","
            strSQL = strSQL & intPartyID & ","
            strSQL = strSQL & "'" & Trim(txtAddCat.Text) & "',"
            strSQL = strSQL & "'" & Trim(txtAdd1.Text) & "',"
            strSQL = strSQL & "'" & Trim(txtAdd2.Text) & "',"
            strSQL = strSQL & "'" & Trim(txtAdd3.Text) & "',"
            strSQL = strSQL & "'" & Trim(txtAdd4.Text) & "',"
            strSQL = strSQL & "'" & Trim(cboCity.Text) & "',"
            strSQL = strSQL & "'" & Trim(cboState.Text) & "',"
            strSQL = strSQL & " '" & (txtPincode.Text) & "',"
            strSQL = strSQL & "'" & cboCountry.Text & "',"
            'If chkDefault.Checked = True Then
            '    strSQL = strSQL & "'Y',"
            'Else
            '    strSQL = strSQL & "'N',"
            'End If

            If ChkDefShipAd.Checked = True Then
                strSQL = strSQL & "'Y')"
            Else
                strSQL = strSQL & "'N')"
            End If
            strSQL = Replace(Replace(strSQL, vbCrLf, ""), vbCr, "")
        Else
            strSQL = "Update MstPartyAddress "
            strSQL = strSQL & " set AddCat = '" & txtAddCat.Text & "',"
            strSQL = strSQL & " Add1 = '" & txtAdd1.Text & "',"
            strSQL = strSQL & " Add2 = '" & txtAdd2.Text & "',"
            strSQL = strSQL & " Add3 = '" & txtAdd3.Text & "',"
            strSQL = strSQL & " Add4 = '" & txtAdd4.Text & "',"
            strSQL = strSQL & " City = '" & cboCity.Text & "',"
            strSQL = strSQL & " State = '" & cboState.Text & "',"
            strSQL = strSQL & " PinCode =  '" & (txtPincode.Text) & "',"
            strSQL = strSQL & " Country = '" & cboCountry.Text & "'"
            'If chkDefault.Checked = True Then
            '    strSQL = strSQL & ", SetDefault='Y'"
            'Else
            '    strSQL = strSQL & ", SetDefault='N'"
            'End If

            If ChkDefShipAd.Checked = True Then
                strSQL = strSQL & ", DefShipAdd = 'Y'"
            Else
                strSQL = strSQL & ", DefShipAdd = 'N'"
            End If
            strSQL = strSQL & " where PartyAddID  = " & Val(txtAddID.Text)
            strSQL = strSQL & " and MstDocID  = " & intDocID
            strSQL = strSQL & " and PartyID  = " & intPartyID
            strSQL = Replace(Replace(strSQL, vbCrLf, ""), vbCr, "")
        End If
        Try
            svtTrn = svtConn.BeginTransaction(IsolationLevel.ReadCommitted)
            If Not clsSVT.execSQLStatement(strSQL, svtTrn) Then
                svtTrn.Rollback()
                Exit Sub
            End If
            'If Mode = "A" Then
            '    Call RecordAuditLog("PARTY ADDRESS", Val(txtAddID.Text), UserActions.AddAction, svtTrn)
            'Else
            '    Call RecordAuditLog("PARTY ADDRESS", Val(txtAddID.Text), UserActions.EditAction, svtTrn)
            'End If
            MsgBox("Data Saved Successfully!", MsgBoxStyle.Information, Me.Text)
            svtTrn.Commit()
            Mode = ""
            fraEdit.Visible = False
            vsfDet.Visible = True
            Call initScreen()

            'For i = 0 To vsfDet.Rows.Count - 1
            '    If vsfDet.GetData(i, vsfDet.Cols.Count - 1) = "Y" Then
            '        blndefFound = True
            '        Exit For
            '    End If
            'Next
            'If blndefFound = False Then
            '    strSQL = "Update MstPartyAddress set SetDefault='Y' "
            '    strSQL = strSQL & " Where MstDocID  = " & intDocID
            '    strSQL = strSQL & " and PartyID  = " & intPartyID
            '    strSQL = strSQL & " and PartyAddId=" & vsfDet.GetData(1, 0)
            '    If Not clsSVT.execSQLStatement(strSQL, svtTrn) Then
            '        svtTrn.Rollback()
            '        Exit Sub
            '    End If
            'End If

        Catch ex As Exception
            MsgBox("Error during save" & vbCrLf & ex.Message, MsgBoxStyle.Critical, Me.Text & "[btnSave_Click]")
            svtTrn.Rollback()
        End Try

    End Sub

    Private Function Validfields() As Boolean
        Dim blnFound As Boolean = False
        Dim intSCCID As Integer
        Validfields = False
        If cboCountry.Text = UCase(Trim("India")) Then
            'If cboCountry.Text = UCase(Trim("India")) Or cboCountry.Text = "" Then
            intSCCID = clsSVT.getDataLongValue("Select SCCID from MstSCC where City='" & cboCity.Text & "'", 0, blnFound)
            If Not blnFound Then
                MsgBox("City Not found in Master!", vbInformation, Me.Text)
                cboCity.Focus()
                Exit Function
            End If
            blnFound = False

            intSCCID = clsSVT.getDataLongValue("Select SCCID from MstSCC where Country='" & cboCountry.Text & "'", 0, blnFound)
            If Not blnFound Then
                MsgBox("Country Not found in Master!", vbInformation, Me.Text)
                cboCountry.Focus()
                Exit Function
            End If
            blnFound = False

            intSCCID = clsSVT.getDataLongValue("Select SCCID from MstSCC where State='" & cboState.Text & "'", 0, blnFound)
            If Not blnFound Then
                MsgBox("State Not found in Master!", vbInformation, Me.Text)
                cboState.Focus()
                Exit Function
            End If
        Else
            cboState.Text = "-"
        End If
        blnFound = False
        'txtCID.Text = clsSVT.AutoIncrementedNum("PartyCID", "MstPartyContacts")

        If Trim(txtAddCat.Text) = "" Then
            MsgBox("Please enter Address Category...", MsgBoxStyle.Information, Me.Text)
            txtAddCat.Focus()
            Exit Function
        End If
        If Trim(txtAdd1.Text) = "" Then
            MsgBox("Please enter Address...", MsgBoxStyle.Information, Me.Text)
            txtAddCat.Focus()
            Exit Function
        End If
        If Trim(cboCity.Text) = "" Then
            MsgBox("Please enter City...", MsgBoxStyle.Information, Me.Text)
            txtAddCat.Focus()
            Exit Function
        End If


        If Mode = "M" Then

            Strsql = "Select PartyAddID from  MstPartyAddress " _
                    & " where AddCat ='" & Trim(txtAddCat.Text) & "' and PartyAddID <> " & Val(txtAddID.Text)

        ElseIf Mode = "A" Then

            Strsql = "Select PartyAddID from  MstPartyAddress " _
                    & " where AddCat ='" & Trim(txtAddCat.Text) & "' and MstDocID = " & intDocID & " and PartyID = " & intPartyID
        End If



        ''If Mode = "A" Then
        'Call clsSVT.getDataStringValue("Select PartyAddID from  MstPartyAddress " _
        '                & " where AddCat ='" & Trim(txtAddCat.Text) & "' and MstDocID = " & intDocID & " and PartyID <> " & intPartyID, "", blnFound)
        ''End If

        Call clsSVT.getDataStringValue(Strsql, "", blnFound)

        If blnFound Then
            MsgBox("Category already exists..", vbInformation, Me.Text)
            txtAddCat.Text = ""
            txtAddCat.Focus()
            Exit Function
        End If

        If Trim(txtAdd1.Text) = "" Then
            MsgBox("Please enter first line of Address...", MsgBoxStyle.Information, Me.Text)
            txtAdd1.Focus()
            Exit Function
        End If

        Validfields = True
    End Function

    Private Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        Mode = "A"
        Call ClearFlds()
        vsfDet.Visible = True
        fraEdit.Visible = True
        fraEdit.BringToFront()
        txtAddCat.Focus()
        visibleEditbtn(True)
        txtAddCat.Enabled = True
        txtAdd1.Enabled = True
        txtAdd2.Enabled = True
        txtAdd3.Enabled = True
        txtAdd4.Enabled = True
        txtPincode.Enabled = True
        txtPartyName.Enabled = True
        cboCity.Enabled = True
        cboState.Enabled = True
        cboCountry.Enabled = True
        ChkDefShipAd.Checked = False
        cboCountry.SelectedIndex = -1
        cboState.SelectedIndex = -1
    End Sub

    Private Sub ClearFlds()
        txtAddCat.Text = ""
        txtAdd1.Text = ""
        txtAdd2.Text = ""
        txtAdd3.Text = ""
        txtAdd4.Text = ""
        cboCity.Text = ""
        cboState.Text = ""
        txtPincode.Text = ""
        cboCountry.Text = ""
        '  chkDefault.Checked = False
    End Sub

    Private Sub vsfDet_BeforeEdit(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfDet.BeforeEdit
        e.Cancel = True
    End Sub

    Private Sub vsfDet_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles vsfDet.DoubleClick
        Call editProc("VIEW")
    End Sub

    Private Sub btnEdit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEdit.Click
        Call editProc("EDIT")
    End Sub

    Private Sub editProc(ByVal argType As String)
        'Dim intstring As String
        If vsfDet.Rows.Count = 1 Then Exit Sub
        If vsfDet.Row <= 0 Then Exit Sub

        'intstring = vsfDet.GetData(vsfDet.Row, 2)

        If vsfDet.GetData(vsfDet.Row, 4) = "DEFAULT" Then
            MsgBox("DEFAULT Address cannot be edited!!", vbInformation, Me.Text)
            Exit Sub
        End If

        txtAddID.Text = vsfDet.GetData(vsfDet.Row, 0)
        Dim rs As oledbDataReader
        rs = clsSVT.getDataReaderFromSQL("Select * from MstPartyAddress where PartyID = " & intPartyID & "  and MstDocid= " & intDocID & " and PartyAddID=" & vsfDet.GetData(vsfDet.Row, 0))
        If rs.Read Then
            txtAddID.Text = GetVal(rs("PartyAddID"), "")
            txtAddCat.Text = GetVal(rs("AddCat"), "")
            txtAdd1.Text = GetVal(rs("Add1"), "")
            txtAdd2.Text = GetVal(rs("Add2"), "")
            txtAdd3.Text = GetVal(rs("Add3"), "")
            txtAdd4.Text = GetVal(rs("Add4"), "")
            cboCity.Text = GetVal(rs("City"), "")
            cboState.Text = GetVal(rs("State"), "")
            cboCountry.Text = GetVal(rs("Country"), "")
            txtPincode.Text = GetVal(rs("Pincode"), "")
            txtDocId.Text = GetVal(rs("MstDocId"), 0)
            'If GetVal(rs("SetDefault"), "N") = "Y" Then
            '    chkDefault.Checked = True
            'Else
            '    chkDefault.Checked = False
            ' End If
            If GetVal(rs("DefShipAdd"), "N") = "Y" Then
                ChkDefShipAd.Checked = True
            Else
                ChkDefShipAd.Checked = False
            End If
            rs.Close()
        Else
            Exit Sub
        End If
        fraEdit.Visible = True
        vsfDet.Visible = True
        fraEdit.BringToFront()

        If argType = "EDIT" Then
            Mode = "M"
            visibleEditbtn(True)
            txtAddCat.Enabled = True
            txtAdd1.Enabled = True
            txtAdd2.Enabled = True
            txtAdd3.Enabled = True
            txtAdd4.Enabled = True
            txtPincode.Enabled = True
            txtPartyName.Enabled = True
            cboCity.Enabled = True
            cboState.Enabled = True
            cboCountry.Enabled = True
            'chkDefault.Enabled = True
            ChkDefShipAd.Enabled = True
        Else
            visibleEditbtn(False)
            txtAddCat.Enabled = False
            txtAdd1.Enabled = False
            txtAdd2.Enabled = False
            txtAdd3.Enabled = False
            txtAdd4.Enabled = False
            txtPincode.Enabled = False
            txtPartyName.Enabled = False
            cboCity.Enabled = False
            cboState.Enabled = False
            cboCountry.Enabled = False
            '  chkDefault.Enabled = False
            ChkDefShipAd.Enabled = False
        End If
        txtAddCat.Focus()
    End Sub
    Private Sub EnableBtn(ByVal argMode As String)
        If argMode <> "M" Then
            btnAdd.Enabled = False
            btnEdit.Enabled = False
            btnDelete.Enabled = False
        End If
    End Sub
    Private Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click

        If fraEdit.Visible = True Then Exit Sub
        Dim strSQL As String
        If vsfDet.Rows.Count = 1 Then
            MsgBox("Address Not Present for this " & strfrmCaption & "!!", vbInformation, Me.Text)
            Exit Sub
        End If
        If vsfDet.Row <= 0 Then
            MsgBox("Address Not Present for this " & strfrmCaption & "!!", vbInformation, Me.Text)
            Exit Sub
        End If
        If vsfDet.GetData(vsfDet.Row, 4) = "DEFAULT" Then
            MsgBox("DEFAULT Address cannot be deleted!!", vbInformation, Me.Text)
            Exit Sub
        End If
        ' 
        If MsgBox("Are you sure you want to delete : '" & vsfDet.GetData(vsfDet.Row, 1) _
                  & "'", vbCritical + MsgBoxStyle.YesNo + vbDefaultButton2, "Delete Confirmation") = MsgBoxResult.Yes Then
            Dim svtTrn As oledb.oledbTransaction = Nothing
            Try
                SetHideRow(vsfDet, True, vsfDet.Row)
                svtTrn = svtConn.BeginTransaction(IsolationLevel.ReadCommitted)
                'Call RecordAuditLog("PARTY ADDRESS", Val(vsfDet.GetData(vsfDet.Row, 0)), UserActions.DeleteAction, svtTrn)

                strSQL = "Delete from MstPartyAddress where PArtyAddID = " & Val(vsfDet.GetData(vsfDet.Row, 0)) & " and MstDocid=" & Val(txtDocId.Text)
                Call clsSVT.execSQLStatement(strSQL, svtTrn)
                MsgBox("Deleted Successfully!", MsgBoxStyle.Information, Me.Text)


                svtTrn.Commit()
            Catch ex As Exception
                svtTrn.Rollback()
                Exit Sub
            End Try
        End If

        Call initScreen()
    End Sub

    Public Sub initForm(ByVal argDocID As Integer, ByVal argPartyID As Integer)
        intPartyID = argPartyID
        intDocID = argDocID
        txtDocId.Text = argDocID
        txtPartyID.Text = argPartyID
        Call initScreen()
    End Sub

    Private Sub txtPincode_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles txtPincode.KeyPress
        If Asc(e.KeyChar) <> 8 Then
            If Asc(e.KeyChar) < 48 Or Asc(e.KeyChar) > 57 Then
                e.Handled = True
            End If
        End If
        If KeyValidate("Integer", e.KeyChar) Then
            e.Handled = False
        Else
            e.Handled = True
        End If
        If e.KeyChar = "'" Then
            e.Handled = True
        End If
    End Sub

    Private Sub frmMstPartyAddress_KeyUp(sender As Object, e As KeyEventArgs) Handles Me.KeyUp
        If e.KeyCode = Keys.F9 And e.Control = True Then
            Call initFormSize(Me, Me.Text, True, 756, 483, 0, 0)
            Call initFormVsfDet(Me, Me.Text, vsfDet, "DET")
            MsgBox("Restored Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
        If e.KeyCode = Keys.F8 And e.Control = True Then
            Call SaveActFormSettings(Me, Me.Text)
            Call SaveActFormVsfDet(Me, Me.Text, "DET", vsfDet)
            MsgBox("Saved Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If

        If e.Alt = True And e.KeyCode = Keys.X Then
            Call ExportToXL(vsfDet)
        End If
    End Sub

    Private Sub btnExport_Click(sender As Object, e As EventArgs) Handles btnExport.Click
        Call ExportToXL(vsfDet, argFileName:="Contact Address for -" & txtPartyCode.Text)
    End Sub

    Private Sub cboCity_Validating(sender As Object, e As CancelEventArgs) Handles cboCity.Validating

        Dim blnFound As Boolean = False
        Dim intSCCID As Integer
        intSCCID = clsSVT.getDataLongValue("Select SCCID from MstSCC where City='" & cboCity.Text & "'", 0, blnFound)
        cboCountry.Text = clsSVT.getDataStringValue("Select Country from MstSCC where SCCID=" & intSCCID, 0, False)
        cboState.Text = clsSVT.getDataStringValue("Select State from MstSCC where SCCID=" & intSCCID, 0, False)
    End Sub

    Private Sub btnAddList_Click(sender As Object, e As EventArgs) Handles btnAddList.Click
        Dim strSQL As String
        Dim frmDisplay As New frmUtlMstView
        strSQL = " Select PartyID," & strFldPartyCode & "," & strFldPartyName & ", AddCat as Category, "
        strSQL &= " (Select Case when IsNull(Add1,'')<>'' then Add1+char(10) else '' end )"
        strSQL &= " + (Select Case when IsNull(Add2,'')<>'' then Add2+char(10) else '' end )"
        strSQL &= " + (Select Case when IsNull(Add3,'')<>'' then Add3+char(10) else '' end)"
        strSQL &= " + (Select Case when IsNull(Add4,'')<>'' then Add4+char(10) else '' end)"
        strSQL &= "+ (Select Case when IsNull(p.City,'')<>'' then p.city+char(10) else '' end)"
        strSQL &= " + (Select Case when IsNull(convert(varchar(10),p.Pincode),'')<>'' then convert(varchar(10),p.Pincode)+char(10) else '' end)"
        strSQL &= " + (Select Case when IsNull(p.State,'')<>'' then p.State+char(10) else '' end )"
        strSQL &= "+ (Select Case when IsNull(p.Country,'')<>'' then p.Country+char(10) else '' end )"
        strSQL &= " as Address,DefShipAdd As DefaultShipAddress "
        strSQL = strSQL & " From MstPartyAddress p, " & strPartyTblName & " c "
        strSQL = strSQL & " Where c." & strFldPartyID & " = PartyID And MstDocID = " & intDocID & " Order By " & strFldPartyCode

        With frmDisplay
            .Text = Me.Text
            Call .FillViewVsf(Me, strSQL, "", "", intDocID, "Address Master")
            .vsfDet.Cols(0).Visible = False
            .vsfDet.Cols(1).AllowMerging = True
            .vsfDet.Cols(2).AllowMerging = True
            .vsfDet.AutoSizeRows()
            .ShowDialog()
        End With
    End Sub

    Private Sub GetPartyDtls()
        Dim rsDummy As oledb.oledbDataReader
        rsDummy = clsSVT.getDataReaderFromSQL("Select * from UtlTransactions where DocID = " & intDocID)

        With rsDummy
            If .HasRows Then
                .Read()
                Dim strPartyShForm As String
                strPartyShForm = GetVal(!DocShForm, 0)
                strPartyTblName = "Mst" & strPartyShForm
                strFldPartyID = strPartyShForm & "ID"
                strFldPartyCode = strPartyShForm & "Code"
                strFldPartyName = strPartyShForm & "Name"
            End If
            .Close()
        End With
    End Sub

    Public Sub FillDocDetails(ByVal argPartyID As Integer)
        Dim strSql As String
        Dim dt As DataTable
        txtPartyCode.Text = clsSVT.getDataStringValue("Select " & strFldPartyCode & " from " & strPartyTblName & " where " & strFldPartyID & "=" & argPartyID, "", False)
        lblPartyName.Text = clsSVT.getDataStringValue("Select " & strFldPartyName & " from " & strPartyTblName & " where " & strFldPartyID & "=" & argPartyID, "", False)
        intPartyID = argPartyID

        strSql = " Select partyAddID, AddCat as Category, "
        strSql &= " (Select Case when IsNull(Add1,'')<>'' then Add1+char(10) else '' end )"
        strSql &= " + (Select Case when IsNull(Add2,'')<>'' then Add2+char(10) else '' end )"
        strSql &= " + (Select Case when IsNull(Add3,'')<>'' then Add3+char(10) else '' end)"
        strSql &= " + (Select Case when IsNull(Add4,'')<>'' then Add4+char(10) else '' end)"
        strSql &= "+ (Select Case when IsNull(City,'')<>'' then city+char(10) else '' end)"
        strSql &= " + (Select Case when IsNull(convert(varchar(10),Pincode),'')<>'' then convert(varchar(10),Pincode)+char(10) else '' end)"
        strSql &= " + (Select Case when IsNull(State,'')<>'' then State+char(10) else '' end )"
        strSql &= "+ (Select Case when IsNull(Country,'')<>'' then Country+char(10) else '' end )"
        strSql &= " as Address,DefShipAdd as DefaultShipAddress "
        strSql &= " from MstPartyAddress "
        strSql &= " where PartyID= " & argPartyID
        strSql &= " and MstDocID = " & intDocID
        strSql &= " order by AddCat"

        'strDetail = NumberList(vbCrLf, Trim(GetVal(rsTemp("CName"), "")), Trim(GetVal(rsTemp("CDesig"), "")), Trim(GetVal(rsTemp("CDepartment"), "")), Trim(GetVal(rsTemp("CMobNo"), "")), Trim(GetVal(rsTemp("CPhone"), "")),
        '             Trim(GetVal(rsTemp("Fax"), "")), Trim(GetVal(rsTemp("Email"), "")))

        dt = clsSVT.getDATATable(strSql)
        vsfDet.DataSource = dt
        fraEdit.Visible = False
        vsfDet.Visible = True
        txtPartyCode.Enabled = False
        txtPartyName.Enabled = False
        SetHideCol(vsfDet, True, 0)
        vsfDet.AutoSizeCols()
        vsfDet.AutoSizeRows()
        'vsfDet.Cols(3).Visible = False
        Call SetColWidth(vsfDet, 300)
        Me.Text = strfrmCaption & " Addresses "
        lblPartyCode.Text = strfrmCaption & " Code "
        '  lblName.Text = strfrmCaption & " Name "
        initGridStyles(Me)
        Call GetPartyDtls()
        If vsfDet.Rows.Count <= 1 Then
            lblNoofAddress.Text = "No Addresses Present for this " & strfrmCaption
            btnEdit.Enabled = False
            btnDelete.Enabled = False
            btnAdd.Enabled = True
        Else
            lblNoofAddress.Text = "Total Addresses Present for this " & strfrmCaption & " is " & vsfDet.Rows.Count - 1 & ""
            btnEdit.Enabled = True
            btnDelete.Enabled = True
            btnAdd.Enabled = True
        End If
        Call initFormVsfDet(Me, Me.Text, vsfDet, "DET")
        Dim cs As C1.Win.C1FlexGrid.CellStyle = vsfDet.Styles.Add(1)

        Dim blndefFound As Boolean = False
        For i = 1 To vsfDet.Rows.Count - 1
            If vsfDet.GetData(i, vsfDet.Cols.Count - 1) = "Y" Then
                blndefFound = True
                cs.BackColor = Color.GreenYellow
                vsfDet.SetCellStyle(i, 1, cs)
                Exit For
            End If
        Next
    End Sub

    Public Function NumberList(strSeparator As String, ParamArray arrVals() As String)

        ' Pass this function a character or characters '
        ' to be used as the separator followed by the values to be combined
        ' For example: strNumberedList =
        ' NumberList(" ", foo, bar)
        Dim X As Integer, strLine As String = ""

        For X = 0 To UBound(arrVals)
            If Not IsNothing(arrVals(X)) And Trim(arrVals(X)) <> "" Then
                strLine = strLine & strSeparator & arrVals(X)
            End If
        Next

        ' remove leading separator caharacter(s)
        strLine = Mid(strLine, Len(strSeparator) + 1)
        NumberList = strLine

    End Function

    Private Sub lblPartyName_Click(sender As Object, e As EventArgs) Handles lblPartyName.Click

    End Sub
End Class