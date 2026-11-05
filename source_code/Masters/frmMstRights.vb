Imports System.Text.RegularExpressions
Imports System.Globalization
Imports System.IO

Public Class frmMstRights

    Dim intDocID As Integer
    Dim clsSVT As New svtLibrary
    Dim strRights As String

    Dim lngUserID As Long
    Dim strLoginName As String
    Dim strChkValue As String

    Dim blnR As Boolean = False
    Dim blnPay As Boolean = False
    Dim blnInv As Boolean = False
    Dim blnFA As Boolean = False

    Dim intRowR As Integer
    Dim intRowPay As Integer
    Dim intRowInv As Integer
    Dim intRowFA As Integer

    Public strFrmPrfx As String
    Public IntID As Integer
    Dim blnFormLoaded As Boolean = False

    Const vUserId = 0
    Const vUserLogin = 1
    Const vUserName = 2

    Const vDetMenuID = 0
    Const vDetMenuTypeID = 1
    Const vDetMenuType = 2
    Const vDetMenuName = 3
    Const vDetAdd = 4
    Const vDetEdit = 5
    Const vDetDel = 6
    Const vDetPrint = 7
    Const vDetView = 8
    Const vDetAuth = 9
    Const vDetAuth1 = 10
    Const vDetOpt1 = 11
    Const vDetOpt2 = 12
    Const vDetOpt3 = 13
    Const vDetOpt4 = 14
    Const vDetOpt5 = 15
    Const vDetOpt6 = 16

    Const vMenuID = 0
    Const vMenuTypeID = 1
    Const vMenuType = 2
    Const vMenuName = 3

    Const vDetUserID = 0
    Const vDetUserLogin = 1
    Const vDetUserName = 2
    Const vDetUAdd = 3
    Const vDetUEdit = 4
    Const vDetUDel = 5
    Const vDetUPrint = 6
    Const vDetUView = 7
    Const vDetUAuth = 8
    Const vDetUAuth1 = 9
    Const vDetUOpt1 = 10
    Const vDetUOpt2 = 11
    Const vDetUOpt3 = 12
    Const vDetUOpt4 = 13
    Const vDetUOpt5 = 14

    Private Sub frmMstRights_Activated(sender As Object, e As System.EventArgs) Handles MyBase.Activated
        blnFormLoaded = True
    End Sub

    Private Sub frmMstRights_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        txtUserID.Text = IntID
        intDocID = 18
        'strRights = getUserRights(201)
        initGridStyles(Me)
        Call initScreen()
        Call initFormSize(Me, Me.Text, False, 1048, 639, 0, 0)
        Call initFormVsfDet(Me, Me.Text, vsfDet, "Rights")
        KeyPreview = True
        Call ChangeFontSize(Me)
        cboFilterOn.Text = "All"

    End Sub

    Private Sub initScreen()
        lblCode.Text = strFrmPrfx & "Code"
        lblName.Text = strFrmPrfx & "Name"
        txtCode.Text = clsSVT.getDataStringValue("Select " & strFrmPrfx & "Code" & " from Mst" & strFrmPrfx & " where " & strFrmPrfx & "ID=" & IntID, "", False)
        txtName.Text = clsSVT.getDataStringValue("Select " & strFrmPrfx & "Name from Mst" & strFrmPrfx & " where " & strFrmPrfx & "ID=" & IntID, "", False)
        Call bindCombo(cboUserCopy, strFrmPrfx & "Code", strFrmPrfx & "Code", "Select " & strFrmPrfx & "Code" & " from Mst" & strFrmPrfx & " where InActive<>'Y' order by 1")
        Call fillRights()
        Call fillUserRights(IntID)
        initGridStyles(Me)
    End Sub

    Private Function saveDetails() As Boolean
        Dim strSQL As String = ""
        Dim strSelRights As String
        Dim strValues As String = ""
        Dim strFlds As String = ""
        Dim svtTrn As oledb.oledbTransaction
        saveDetails = False

        svtTrn = svtConn.BeginTransaction(IsolationLevel.ReadCommitted)
        Try
            lngUserID = Val(txtUserID.Text)
            Call clsSVT.execSQLStatement("Delete from Mst" & strFrmPrfx & "Rights where " & strFrmPrfx & "Id = " & lngUserID, svtTrn)
            With vsfDet
                For i = 1 To .Rows.Count - 1
                    strSelRights = ""
                    For j = vDetAdd To vDetOpt5
                        If .GetCellCheck(i, j) = C1.Win.C1FlexGrid.CheckEnum.Checked Then
                            strSelRights = strSelRights & "1"
                        Else
                            strSelRights = strSelRights & "0"
                        End If
                    Next
                    strSQL = "Insert into Mst" & strFrmPrfx & "Rights(" & strFrmPrfx & "Id, MenuItemID, " & strFrmPrfx & "Rights) values ("
                    strSQL = strSQL & lngUserID & ","
                    strSQL = strSQL & .GetData(i, vDetMenuID) & ","
                    strSQL = strSQL & "'" & strSelRights & "')"
                    If Not clsSVT.execSQLStatement(strSQL, svtTrn) Then
                        saveDetails = False
                        svtTrn.Rollback()
                        Exit Function
                    End If
                Next
            End With
            saveDetails = True
            MsgBox("Updated successfully.", vbInformation, Me.Text)
            svtTrn.Commit()
            fillUserRights(lngUserID)
        Catch ex As Exception
            svtTrn.Rollback()
            saveDetails = False
        End Try
    End Function

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub fillRights()
        Dim rs As oledb.oledbDataReader
        Dim strSQL As String, intRow As Integer
        With vsfDet
            .Cols.Count = 17

            SetHideCol(vsfDet, True, vDetMenuID, vDetMenuTypeID, vDetOpt6)
            .SetData(0, vDetAdd, "Add")
            .SetData(0, vDetDel, "Delete")
            .SetData(0, vDetEdit, "Edit")
            .SetData(0, vDetPrint, "Print")
            .SetData(0, vDetView, "View")
            .SetData(0, vDetAuth, "Auth")
            .SetData(0, vDetAuth1, "Auth1")
            .SetData(0, vDetMenuName, "Menu")
            .SetData(0, vDetMenuType, "Type")
            .SetData(0, vDetOpt1, "Opt-1")
            .SetData(0, vDetOpt2, "Opt-2")
            .SetData(0, vDetOpt3, "Opt-3")
            .SetData(0, vDetOpt4, "Opt-4")
            .SetData(0, vDetOpt5, "Opt-5")
            .SetData(0, vDetOpt6, "Opt-6")

            SetFlexColor(vsfDet, "", vDetMenuName, vDetMenuType)

            .Cols(vDetAdd).DataType = GetType(Boolean)
            .Cols(vDetEdit).DataType = GetType(Boolean)
            .Cols(vDetDel).DataType = GetType(Boolean)
            .Cols(vDetPrint).DataType = GetType(Boolean)
            .Cols(vDetView).DataType = GetType(Boolean)
            .Cols(vDetAuth).DataType = GetType(Boolean)
            .Cols(vDetAuth1).DataType = GetType(Boolean)
            .Cols(vDetOpt1).DataType = GetType(Boolean)
            .Cols(vDetOpt2).DataType = GetType(Boolean)
            .Cols(vDetOpt3).DataType = GetType(Boolean)
            .Cols(vDetOpt4).DataType = GetType(Boolean)
            .Cols(vDetOpt5).DataType = GetType(Boolean)
            .Cols(vDetOpt6).DataType = GetType(Boolean)
            .Cols(vDetMenuName).AllowEditing = False
            .Cols(vDetMenuType).AllowEditing = False

            .Cols.Frozen = vDetMenuName
        End With
        SetColWidth(vsfDet, -1, -1, 104, 269, 31, 31, 44, 35, 36, 39, 39, 39, 39)

        strSQL = "Select * from MstMenu  where MenuItemCaption <> '-' "
        strSQL = strSQL & " and menushow='Y' order by "
        strSQL = strSQL & " MenuItemType, MenuItemCaption"
        rs = clsSVT.getDataReaderFromSQL(strSQL)
        vsfDet.Rows.Count = 1

        With rs
            Do While .Read
                intRow = vsfDet.Rows.Count
                vsfDet.Rows.Count = vsfDet.Rows.Count + 1
                vsfDet.SetData(intRow, vDetMenuID, !MenuItemID)
                vsfDet.SetData(intRow, vDetMenuName, !MenuItemCaption)
                vsfDet.SetData(intRow, vDetMenuTypeID, !MenuItemType)
                Select Case !MenuItemType
                    Case 1
                        vsfDet.SetData(intRow, vDetMenuType, "Masters")
                    Case 2
                        vsfDet.SetData(intRow, vDetMenuType, "Transactions")
                    Case 3
                        vsfDet.SetData(intRow, vDetMenuType, "Vouchers")
                    Case 4
                        vsfDet.SetData(intRow, vDetMenuType, "Reports")
                    'Case 5
                    '    vsfDet.SetData(intRow, vDetMenuType, "")
                    Case 6
                        vsfDet.SetData(intRow, vDetMenuType, "Tools")
                    Case 7
                        vsfDet.SetData(intRow, vDetMenuType, "Utilities")
                End Select

            Loop
            .Close()
        End With
        With vsfDet

            .AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.RestrictRows
            .Cols(vDetMenuType).AllowMerging = True
        End With
    End Sub

    Private Sub fillUserRights(ByVal argpartyid As Long)
        Dim strSQL As String, i As Integer, strMenuRights As String
        Dim rs As oledb.oledbDataReader
        strSQL = "Select * from Mst" & strFrmPrfx & "Rights  where " & strFrmPrfx & "Id = " & argpartyid
        rs = clsSVT.getDataReaderFromSQL(strSQL)
        With rs
            Do While .Read
                For i = 1 To vsfDet.Rows.Count - 1
                    If vsfDet.GetData(i, vDetMenuID) = !MenuItemId Then
                        strMenuRights = rs(strFrmPrfx & "Rights")
                        vsfDet.SetCellCheck(i, vDetAdd, chkVsfRights(strMenuRights, chkRightsPos.AddRights))
                        vsfDet.SetCellCheck(i, vDetEdit, chkVsfRights(strMenuRights, chkRightsPos.EditRights))
                        vsfDet.SetCellCheck(i, vDetDel, chkVsfRights(strMenuRights, chkRightsPos.DelRights))
                        vsfDet.SetCellCheck(i, vDetPrint, chkVsfRights(strMenuRights, chkRightsPos.PrintRights))
                        vsfDet.SetCellCheck(i, vDetView, chkVsfRights(strMenuRights, chkRightsPos.ViewRights))
                        vsfDet.SetCellCheck(i, vDetAuth, chkVsfRights(strMenuRights, chkRightsPos.AuthRights))
                        vsfDet.SetCellCheck(i, vDetAuth1, chkVsfRights(strMenuRights, chkRightsPos.Auth1Rights))
                        vsfDet.SetCellCheck(i, vDetOpt1, chkVsfRights(strMenuRights, chkRightsPos.opt1Rights))
                        vsfDet.SetCellCheck(i, vDetOpt2, chkVsfRights(strMenuRights, chkRightsPos.opt2Rights))
                        vsfDet.SetCellCheck(i, vDetOpt3, chkVsfRights(strMenuRights, chkRightsPos.opt3Rights))
                        vsfDet.SetCellCheck(i, vDetOpt4, chkVsfRights(strMenuRights, chkRightsPos.opt4Rights))
                        vsfDet.SetCellCheck(i, vDetOpt5, chkVsfRights(strMenuRights, chkRightsPos.opt5Rights))
                        Exit For
                    End If
                Next
            Loop
            .Close()
        End With
    End Sub

    Public Function chkVsfRights(ByVal mRights As String, ByVal mPos As chkRightsPos, Optional ByVal argfrmCaption As String = "") As C1.Win.C1FlexGrid.CheckEnum
        If Mid(mRights, mPos, 1) = "1" Then
            'if 0 then return true
            chkVsfRights = C1.Win.C1FlexGrid.CheckEnum.Checked
        Else
            'if 1 then return false
            chkVsfRights = C1.Win.C1FlexGrid.CheckEnum.Unchecked
        End If
    End Function

    Private Sub HidePnlrows(ByVal argtype As String)
        With vsfDet
            If argtype = "ALL" Then
                For intRow = 1 To .Rows.Count - 1
                    .Rows(intRow).Visible = True
                Next
                '.Cols(vMenuType).Visible = True
                Exit Sub
            End If

            For intRow = 1 To .Rows.Count - 1
                ' MsgBox(UCase(.GetData(intRow, vMenuType)))
                If UCase(.GetData(intRow, vMenuType)) = argtype Then
                    .Rows(intRow).Visible = True
                Else
                    .Rows(intRow).Visible = False
                End If
            Next
        End With
    End Sub

    Private Sub HidePnlrows_Transpose(ByVal argtype As String)
        With vsfMenu
            If argtype = "ALL" Then
                For intRow = 1 To .Rows.Count - 1
                    .Rows(intRow).Visible = True
                Next
                '.Cols(vMenuType).Visible = True
                Exit Sub
            End If

            For intRow = 1 To .Rows.Count - 1
                ' MsgBox(UCase(.GetData(intRow, vMenuType)))
                If UCase(.GetData(intRow, vMenuType)) = argtype Then
                    .Rows(intRow).Visible = True
                Else
                    .Rows(intRow).Visible = False
                End If
            Next
        End With
    End Sub

    Private Sub Hiderows()
        Dim intRow As Integer
        Dim strMenuName As String = ""
        With vsfMenu
            For intRow = 1 To .Rows.Count - 1
                If cboFilterOn.Text = "Masters" Then
                    If .GetData(intRow, vDetMenuType) <> "Masters" Then
                        .Rows(intRow).Visible = False
                    End If
                End If
                If cboFilterOn.Text = "Transactions" Then
                    If .GetData(intRow, vDetMenuType) <> "Transactions" Then
                        .Rows(intRow).Visible = False
                    End If
                End If
                If cboFilterOn.Text = "Vouchers" Then
                    If .GetData(intRow, vDetMenuType) <> "Vouchers" Then
                        .Rows(intRow).Visible = False
                    End If
                End If
                If cboFilterOn.Text = "Reports" Then
                    If .GetData(intRow, vDetMenuType) <> "Reports" Then
                        .Rows(intRow).Visible = False
                    End If
                End If
                If cboFilterOn.Text = "Utilities" Then
                    If .GetData(intRow, vDetMenuType) <> "Utilities" Then
                        .Rows(intRow).Visible = False
                    End If
                End If
            Next
        End With
        vsfDet.AutoSizeCols()
    End Sub

    Private Sub vsfDet_BeforeEdit(sender As Object, e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfDet.BeforeEdit
        'If Not (Mode = "A" Or Mode = "M") Then
        '    e.Cancel = True
        'End If
    End Sub

    Private Sub vsfDet_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles vsfDet.DoubleClick, btnTranspose.Click
        If vsfDet.Col = 0 Then
            With vsfDet
                .Col = 2
            End With
        End If

        If Not allowEdit() Then
            Exit Sub
        End If

        'Modified on 27-08-2016 by Thangam
        If vsfDet.Col = vDetMenuName Or vsfDet.Col = vDetMenuType Then
            Dim intMenuid As Integer, intMenuType As Integer
            intMenuid = GetVal(vsfDet.GetData(vsfDet.Row, vDetMenuID), 0)
            intMenuType = GetVal(vsfDet.GetData(vsfDet.Row, vDetMenuTypeID), 0)
            pnlTranspose.Visible = True
            pnlTranspose.BringToFront()

            fillMenuDet()
            fillUserDet()
            FillUserRightsDetails(vsfMenu.GetData(vsfMenu.Row, vMenuID))
            With vsfMenu
                For i As Int32 = 1 To .Rows.Count - 1
                    If GetVal(.GetData(i, vMenuID), 0) = intMenuid AndAlso GetVal(.GetData(i, vMenuTypeID), 0) = intMenuType Then
                        .Row = i
                        .Col = vMenuName
                        Exit For
                    End If
                Next
            End With
        End If
        'pnlTranspose.Visible = True
        'pnlTranspose.BringToFront()

        'fillMenuDet()
        'fillUserDet()
        'FillUserRightsDetails(vsfMenu.GetData(vsfMenu.Row, vMenuID))
        'vsfUserDet.Row = vsfUser.Row
    End Sub

    Private Sub fillUserDet()
        Dim rs As oledb.oledbDataReader
        Dim strSQL As String

        With vsfUserDet
            .Cols.Count = 15
            SetHideCol(vsfUserDet, True, vDetUserID)
            .SetData(0, vDetUAdd, "Add")
            .SetData(0, vDetUDel, "Delete")
            .SetData(0, vDetUEdit, "Edit")
            .SetData(0, vDetUPrint, "Print")
            .SetData(0, vDetUView, "View")
            .SetData(0, vDetUAuth, "Auth")
            .SetData(0, vDetUAuth1, "Auth1")
            .SetData(0, vDetUserLogin, "Login Name")
            .SetData(0, vDetUserName, "User Name")
            .SetData(0, vDetUOpt1, "Opt-1")
            .SetData(0, vDetUOpt2, "Opt-2")
            .SetData(0, vDetUOpt3, "Opt-3")
            .SetData(0, vDetUOpt4, "Opt-4")
            .SetData(0, vDetUOpt5, "Opt-5")

            .Cols(vDetUAdd).DataType = GetType(Boolean)
            .Cols(vDetUEdit).DataType = GetType(Boolean)
            .Cols(vDetUDel).DataType = GetType(Boolean)
            .Cols(vDetUPrint).DataType = GetType(Boolean)
            .Cols(vDetUView).DataType = GetType(Boolean)
            .Cols(vDetUAuth).DataType = GetType(Boolean)
            .Cols(vDetUAuth1).DataType = GetType(Boolean)
            .Cols(vDetUOpt1).DataType = GetType(Boolean)
            .Cols(vDetUOpt2).DataType = GetType(Boolean)
            .Cols(vDetUOpt3).DataType = GetType(Boolean)
            .Cols(vDetUOpt4).DataType = GetType(Boolean)
            .Cols(vDetUOpt5).DataType = GetType(Boolean)
            .Cols(vDetUserLogin).AllowEditing = False
            .Cols(vDetUserName).AllowEditing = False
        End With
        SetColWidth(vsfUserDet, -1, 104, 269, 31, 31, 44, 35, 36, 39, 39, 39, 39, 39)
        strSQL = "Select * from Mst" & strFrmPrfx & " order by " & strFrmPrfx & "Code"
        rs = clsSVT.getDataReaderFromSQL(strSQL)
        vsfUserDet.Rows.Count = 1
        With rs
            Do While .Read
                vsfUserDet.Rows.Count = vsfUserDet.Rows.Count + 1
                vsfUserDet.SetData(vsfUserDet.Rows.Count - 1, vDetUserID, rs(strFrmPrfx & "ID"))
                vsfUserDet.SetData(vsfUserDet.Rows.Count - 1, vDetUserLogin, rs(strFrmPrfx & "Code"))
                vsfUserDet.SetData(vsfUserDet.Rows.Count - 1, vDetUserName, rs(strFrmPrfx & "Name"))
            Loop
            .Close()
        End With

        vsfUserDet.AutoSizeCols()
    End Sub

    Private Function allowEdit() As Boolean '
        Dim blnfound As Boolean
        allowEdit = False

        If Trim(UCase(gUserCode)) = "ADMIN" Then
            allowEdit = True
            Exit Function
        End If

        Dim str As String
        str = clsSVT.getDataStringValue("select UserRights from mstUserRights where userID = " & gUserID & " and menuItemID = 201", "0", blnfound)

        If Not blnfound Then
            Exit Function
        End If

        If str(1) = "0" Then
            Exit Function
        End If

        allowEdit = True
    End Function


    Private Sub fillMenuDet()
        Dim rs As oledb.oledbDataReader
        Dim strSQL As String
        Dim strTemp As String = ""
        With vsfMenu
            .Cols.Count = 4
            .Cols(vMenuID).Visible = False
            .Cols(vMenuTypeID).Visible = False
            .SetData(0, vMenuType, "Type")
            .SetData(0, vMenuName, "Menu")

            SetFlexColor(vsfMenu, "", vMenuType, vMenuName)

            .Cols(vMenuType).AllowEditing = False
            .Cols(vMenuName).AllowEditing = False

        End With

        '  strSQL = "Select * from MstMenu  where menushow='Y' and MenuItemCaption <> '-' order by MenuItemType, ModuleName, MenuIndex"

        strSQL = "Select * from MstMenu  "
        strSQL = strSQL & " where MenuItemCaption <> '-' "
        strSQL = strSQL & " and menushow='Y' "
        strSQL = strSQL & " order by "
        strSQL = strSQL & " MenuItemType, MenuIndex"

        ' strSQL = "Select * from MstMenu  where MenuItemCaption <> '-' order by MenuItemType, MenuIndex"
        rs = clsSVT.getDataReaderFromSQL(strSQL)
        vsfMenu.Rows.Count = 1
        With rs
            Do While .Read
                vsfMenu.Rows.Count = vsfMenu.Rows.Count + 1
                vsfMenu.SetData(vsfMenu.Rows.Count - 1, vMenuID, !MenuItemID)
                vsfMenu.SetData(vsfMenu.Rows.Count - 1, vMenuTypeID, !MenuItemType)
                vsfMenu.SetData(vsfMenu.Rows.Count - 1, vMenuName, !MenuItemCaption)
                vsfMenu.SetData(vsfMenu.Rows.Count - 1, vMenuType, !MenuItemType)

                Select Case !MenuItemType
                    Case 1
                        vsfMenu.SetData(vsfMenu.Rows.Count - 1, vMenuType, "Masters")
                    Case 2
                        vsfMenu.SetData(vsfMenu.Rows.Count - 1, vMenuType, "Transactions")
                    Case 3
                        vsfMenu.SetData(vsfMenu.Rows.Count - 1, vMenuType, "Vouchers")
                    Case 4
                        vsfMenu.SetData(vsfMenu.Rows.Count - 1, vMenuType, "Reports")
                    'Case 5
                    '    vsfMenu.SetData(vsfMenu.Rows.Count - 1, vMenuType, "")
                    Case 6
                        vsfMenu.SetData(vsfMenu.Rows.Count - 1, vMenuType, "Tools")
                    Case 7
                        vsfMenu.SetData(vsfMenu.Rows.Count - 1, vMenuType, "Utilities")
                End Select
            Loop
            .Close()
        End With

        With vsfMenu
            .AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.RestrictRows
            .Cols(vMenuType).AllowMerging = True
            .Row = vsfDet.Row
            optTAll.Checked = True
        End With

        vsfMenu.AutoSizeCols()
    End Sub

    Private Sub btnPnlClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPnlClose.Click
        pnlTranspose.Visible = False
    End Sub

    Private Sub vsfMenu_GridChanged(sender As Object, e As C1.Win.C1FlexGrid.GridChangedEventArgs) Handles vsfMenu.GridChanged
        If vsfMenu.Rows.Count = 1 Then
            lblMnuCaption.Text = ""
        End If
    End Sub

    Private Sub vsfMenu_RowColChange(ByVal sender As Object, ByVal e As System.EventArgs) Handles vsfMenu.RowColChange
        If vsfMenu.Rows.Count <= 1 Then
            lblMnuCaption.Text = ""
            Exit Sub
        End If

        If Val(vsfMenu.GetData(vsfMenu.Row, vMenuID)) > 0 Then
            Call FillUserRightsDetails(Val(vsfMenu.GetData(vsfMenu.Row, vMenuID)))
            lblMnuCaption.Text = "Assign " & strFrmPrfx & " rights for menu: " & vsfMenu.GetData(vsfMenu.Row, vMenuName)
        End If
    End Sub

    Private Sub FillUserRightsDetails(ByVal argPartyID As Long)
        Dim strSQL As String, i As Integer, strUserRights As String
        Dim rs As oledb.oledbDataReader
        strSQL = "Select * from Mst" & strFrmPrfx & "Rights  where MenuItemID = " & argPartyID
        rs = clsSVT.getDataReaderFromSQL(strSQL)
        Dim introw As Integer

        For introw = 1 To vsfUserDet.Rows.Count - 1
            If UCase(vsfUserDet.GetData(introw, vDetUserLogin)) = "ADMIN" Then
                vsfUserDet.Rows(introw).Visible = False
            End If
        Next

        With rs
            Do While .Read
                For i = 1 To vsfUserDet.Rows.Count - 1
                    If vsfUserDet.GetData(i, vDetUserID) = rs(strFrmPrfx & "ID") Then
                        strUserRights = rs(strFrmPrfx & "Rights")
                        vsfUserDet.SetCellCheck(i, vDetUAdd, chkVsfRights(strUserRights, chkRightsPos.AddRights))
                        vsfUserDet.SetCellCheck(i, vDetUEdit, chkVsfRights(strUserRights, chkRightsPos.EditRights))
                        vsfUserDet.SetCellCheck(i, vDetUDel, chkVsfRights(strUserRights, chkRightsPos.DelRights))
                        vsfUserDet.SetCellCheck(i, vDetUPrint, chkVsfRights(strUserRights, chkRightsPos.PrintRights))
                        vsfUserDet.SetCellCheck(i, vDetUView, chkVsfRights(strUserRights, chkRightsPos.ViewRights))
                        vsfUserDet.SetCellCheck(i, vDetUAuth, chkVsfRights(strUserRights, chkRightsPos.AuthRights))
                        vsfUserDet.SetCellCheck(i, vDetUAuth1, chkVsfRights(strUserRights, chkRightsPos.Auth1Rights))
                        vsfUserDet.SetCellCheck(i, vDetUOpt1, chkVsfRights(strUserRights, chkRightsPos.opt1Rights))
                        vsfUserDet.SetCellCheck(i, vDetUOpt2, chkVsfRights(strUserRights, chkRightsPos.opt2Rights))
                        vsfUserDet.SetCellCheck(i, vDetUOpt3, chkVsfRights(strUserRights, chkRightsPos.opt3Rights))
                        vsfUserDet.SetCellCheck(i, vDetUOpt4, chkVsfRights(strUserRights, chkRightsPos.opt4Rights))
                        vsfUserDet.SetCellCheck(i, vDetUOpt5, chkVsfRights(strUserRights, chkRightsPos.opt5Rights))
                        Exit For
                    End If
                Next
            Loop
            .Close()
        End With


    End Sub

    Private Sub btnPnlSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPnlSave.Click
        Dim strSQL As String = ""
        Dim strSelRights As String
        Dim strValues As String = ""
        Dim strFlds As String = ""
        Dim lngMenuID As Long

        lngMenuID = Val(vsfMenu.GetData(vsfMenu.Row, vMenuID))
        Call clsSVT.execSQLStatement("Delete from Mst" & strFrmPrfx & "Rights where MenuItemID = " & lngMenuID)

        With vsfUserDet
            For i = 1 To .Rows.Count - 1
                strSelRights = ""
                For j = vDetUAdd To vDetUOpt5
                    If .GetCellCheck(i, j) = C1.Win.C1FlexGrid.CheckEnum.Checked Then
                        strSelRights = strSelRights & "1"
                    Else
                        strSelRights = strSelRights & "0"
                    End If
                Next
                strSQL = "Insert into Mst" & strFrmPrfx & "Rights(" & strFrmPrfx & "ID, MenuItemID,  " & strFrmPrfx & "Rights) values ("
                strSQL = strSQL & .GetData(i, vUserId) & ","
                strSQL = strSQL & lngMenuID & ","
                strSQL = strSQL & "'" & strSelRights & "')"
                Call clsSVT.execSQLStatement(strSQL)
            Next
        End With

        MsgBox("Updated successfully!", vbInformation, Me.Text)
        fillMenuDet()
        'fillUserRights(Val
    End Sub

    Private Sub vsfMenu_LostFocus(sender As Object, e As System.EventArgs) Handles vsfMenu.LostFocus
        If Not vsfUserDet.Focused Then
            lblMnuCaption.Text = ""
        End If
    End Sub

    Private Sub txtChgPwd_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs)
        e.Handled = Not (Char.IsDigit(e.KeyChar) Or e.KeyChar = vbBack)
    End Sub

    Private Sub optMasters_CheckedChanged(sender As System.Object, e As System.EventArgs)
        With DirectCast(sender, RadioButton)
            If .Checked Then
                Call HidePnlrows(CStr(.Tag))
            End If
        End With
    End Sub

    Private Sub optTransactions_CheckedChanged(sender As System.Object, e As System.EventArgs)
        With DirectCast(sender, RadioButton)
            If .Checked Then
                Call HidePnlrows(CStr(.Tag))
            End If
        End With
    End Sub

    Private Sub optVouchers_CheckedChanged(sender As System.Object, e As System.EventArgs)
        With DirectCast(sender, RadioButton)
            If .Checked Then
                Call HidePnlrows(CStr(.Tag))
            End If
        End With
    End Sub

    Private Sub optReports_CheckedChanged(sender As System.Object, e As System.EventArgs)
        With DirectCast(sender, RadioButton)
            If .Checked Then
                Call HidePnlrows(CStr(.Tag))
            End If
        End With
    End Sub

    Private Sub optUtilities_CheckedChanged(sender As System.Object, e As System.EventArgs)
        With DirectCast(sender, RadioButton)
            If .Checked Then
                Call HidePnlrows(CStr(.Tag))
            End If
        End With
    End Sub

    Private Sub optAll_CheckedChanged(sender As System.Object, e As System.EventArgs)
        With DirectCast(sender, RadioButton)
            If .Checked Then
                Call HidePnlrows(CStr(.Tag))
            End If
        End With
    End Sub

    Private Sub optTMasters_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles optTMasters.CheckedChanged
        With DirectCast(sender, RadioButton)
            If .Checked Then
                Call HidePnlrows_Transpose(CStr(.Tag))
            End If
        End With
    End Sub

    Private Sub optTTransactions_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles optTTransactions.CheckedChanged
        With DirectCast(sender, RadioButton)
            If .Checked Then
                Call HidePnlrows_Transpose(CStr(.Tag))
            End If
        End With
    End Sub

    Private Sub optTVouchers_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles optTVouchers.CheckedChanged
        With DirectCast(sender, RadioButton)
            If .Checked Then
                Call HidePnlrows_Transpose(CStr(.Tag))
            End If
        End With
    End Sub

    Private Sub optTReports_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles optTReports.CheckedChanged
        With DirectCast(sender, RadioButton)
            If .Checked Then
                Call HidePnlrows_Transpose(CStr(.Tag))
            End If
        End With
    End Sub

    Private Sub optTUtilities_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles optTUtilities.CheckedChanged
        With DirectCast(sender, RadioButton)
            If .Checked Then
                Call HidePnlrows_Transpose(CStr(.Tag))
            End If
        End With
    End Sub

    Private Sub optTAll_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles optTAll.CheckedChanged
        With DirectCast(sender, RadioButton)
            If .Checked Then
                Call HidePnlrows_Transpose(CStr(.Tag))
            End If
        End With
    End Sub


    Private Sub btnCopyFrom_Click(sender As System.Object, e As System.EventArgs) Handles btnCopyFrom.Click
        fraCopyFrom.Visible = True
    End Sub

    Private Sub cboUserCopy_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles cboUserCopy.KeyPress
        e.Handled = True
    End Sub

    Private Sub btnOK_Click(sender As System.Object, e As System.EventArgs) Handles btnOK.Click
        Dim i As Integer, j As Integer
        If Trim(cboUserCopy.Text) = "ADMIN" Then
            For i = 1 To vsfDet.Rows.Count - 1
                For j = vDetAdd To vDetOpt6
                    vsfDet.SetCellCheck(i, j, C1.Win.C1FlexGrid.CheckEnum.Checked)
                Next j
            Next i
        Else
            Call fillUserRights(clsSVT.getDataLongValue("Select " & strFrmPrfx & "ID from Mst" & strFrmPrfx & " where " & strFrmPrfx & "Code" & " ='" & Trim(cboUserCopy.Text) & "'", 0, False))
        End If

        fraCopyFrom.Visible = False
    End Sub

    Private Sub btnCls_Click(sender As System.Object, e As System.EventArgs) Handles btnCls.Click
        fraCopyFrom.Visible = False
    End Sub

    Private Sub vsfDet_MouseClick(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles vsfDet.MouseClick
        Dim blnChkd As Boolean, intCol As Integer, i As Integer
        blnChkd = False

        Dim ht = vsfDet.HitTest

        If ht.Row = 0 Then
            intCol = ht.Column

            If ht.Column >= vDetAdd Then
                For i = 1 To vsfDet.Rows.Count - 1
                    If vsfDet.Rows(i).Visible Then
                        If (vsfDet.GetCellCheck(i, intCol) = C1.Win.C1FlexGrid.CheckEnum.Checked) Then
                            vsfDet.SetCellCheck(i, intCol, C1.Win.C1FlexGrid.CheckEnum.Unchecked)
                        Else
                            vsfDet.SetCellCheck(i, intCol, C1.Win.C1FlexGrid.CheckEnum.Checked)
                        End If
                    End If
                Next i
            End If
        End If
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        If saveDetails() Then
        End If
    End Sub

    Private Sub frmMstRights_KeyUp(sender As Object, e As KeyEventArgs) Handles Me.KeyUp
        If e.KeyCode = Keys.F9 And e.Control = True Then
            Call initFormSize(Me, Me.Text, True, 1048, 639, 0, 0)
            Call initFormVsfDet(Me, Me.Text, vsfDet, "Rights")
            MsgBox("Restored Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
        If e.KeyCode = Keys.F8 And e.Control = True Then
            Call SaveActFormSettings(Me, Me.Text)
            Call SaveActFormVsfDet(Me, Me.Text, "Rights", vsfDet)
            MsgBox("Saved Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
    End Sub

    Private Sub cboFilterOn_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboFilterOn.SelectedIndexChanged
        If cboFilterOn.SelectedIndex = -1 Then
            Exit Sub
        End If
        Call HidePnlrows(UCase(cboFilterOn.Text))
    End Sub

    Private Sub vsfDet_LostFocus(sender As Object, e As EventArgs) Handles vsfDet.LostFocus
        With vsfDet
            If .Rows.Count = .Rows.Count - 1 Then
                btnTranspose.Focus()
            End If
        End With
    End Sub

    Private Sub vsfDet_KeyUp(sender As Object, e As KeyEventArgs) Handles vsfDet.KeyUp
        With vsfDet
            If e.KeyCode = Keys.Tab Then
                If vsfDet.Row = vsfDet.Rows.Count - 1 Then
                    btnTranspose.Focus()
                End If
            End If
        End With
    End Sub
End Class