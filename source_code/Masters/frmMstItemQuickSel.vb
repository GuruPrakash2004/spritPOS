Imports System.ComponentModel
Imports System.Data.oledb
Imports System.Text.RegularExpressions
Imports System.IO
Imports C1.Win.C1FlexGrid

Public Class frmMstItemQuickSel
    Dim clsSVT As New svtLibrary
    Public Mode As String
    Dim dt As New DataTable
    Dim dv As New DataView

    Const vDetSlNo = 0
    Const vDetID = 1
    Const vDetItemName = 2
    Const vDetShortName = 3
    Const vDetTot = 4

    Public strTblName As String

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub frmMstContactPerson_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Initialize with Default columns in Grid
        Call initGridStyles(Me)

        'Initialize Form Properties with default settings
        Call fillFormProperties(Me)

        'Form initialize event
        Call initScreen()
        Call ChangeFontSize(Me)

        'For Fixed Columns getting size from tables
        Call initFormVsfDet(Me, Me.Text, vsfDet, "DET")

        Call bindCombo(cboSel, "QSetName", "QSetName", " Select distinct QSetName from " & strTblName & " order by QSetName")

        Call fillDtls()

        ReArrangeSlNo(vsfDet, vDetSlNo)
    End Sub

    Public Sub initScreen()
        Me.KeyPreview = True
        Call initFormSize(Me, Me.Text, False, 587, 591, 0, 0)

        With vsfDet
            .Rows.Count = 2
            .Cols.Count = vDetTot

            .SetData(0, vDetSlNo, "SlNo")
            .SetData(0, vDetID, "ID")
            .SetData(0, vDetItemName, "Item Name")
            .SetData(0, vDetShortName, "Short Name")
        End With

        Call SetFlexColor(vsfDet, "", vDetSlNo, vDetItemName, vDetShortName)
        Call SetHideCol(vsfDet, True, vDetID)
    End Sub

    Public Sub fillDtls()
        Dim strSQL As String
        Dim rs As oledb.oledbDataReader

        strSQL = " Select QSetName, q.ItemID, i.ITEMNAME, i.SHORTNAME from " & strTblName & " q, MstITEM i where QSetName = '" & Trim(cboSel.Text) & "' and q.itemid = i.ITEMID"
        strSQL = strSQL & " order by ITEMCODE"

        rs = clsSVT.getDataReaderFromSQL(strSQL)

        If Not rs.HasRows Then
            vsfDet.Rows.Count = 1
            vsfDet.Rows.Count = 2
            Exit Sub
        End If

        vsfDet.Rows.Count = 1

        With rs
            Do While .Read
                vsfDet.Rows.Count = vsfDet.Rows.Count + 1
                vsfDet.SetData(vsfDet.Rows.Count - 1, vDetItemName, !ITEMNAME)
                vsfDet.SetData(vsfDet.Rows.Count - 1, vDetShortName, !SHORTNAME)
                vsfDet.SetData(vsfDet.Rows.Count - 1, vDetID, !ItemID)
            Loop
            .Close()
        End With
    End Sub

    Private Sub vsfDet_BeforeEdit(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfDet.BeforeEdit

        Select Case e.Col
            Case vDetSlNo, vDetItemName, vDetShortName
                e.Cancel = True
        End Select
    End Sub

    Private Sub frmMstItemQuickSel_KeyUp(sender As Object, e As KeyEventArgs) Handles Me.KeyUp
        If e.KeyCode = Keys.F9 And e.Control = True Then
            Call initFormSize(Me, Me.Text, True, 939, 483, 0, 0)
            Call initFormVsfDet(Me, Me.Text, vsfDet, "DET")
            MsgBox("Restored Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
        If e.KeyCode = Keys.F8 And e.Control = True Then
            Call SaveActFormSettings(Me, Me.Text)
            Call SaveActFormVsfDet(Me, Me.Text, "DET", vsfDet)
            MsgBox("Saved Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If

        If e.Alt AndAlso e.KeyCode = Keys.A Then
            btnAddItem_Click(sender, e)
        End If

        If e.Alt AndAlso e.KeyCode = Keys.R Then
            btnRemove_Click(sender, e)
        End If

        With vsfDet
            If Trim(.GetData(.Row, vDetShortName)) <> "" And e.KeyCode = Keys.Tab And .Row = .Rows.Count - 1 Then
                .LeftCol = vDetShortName
                .Col = vDetShortName
                .Rows.Count = .Rows.Count + 1
                .Row = .Rows.Count - 1
                .TopRow = .Rows.Count - 1
                ReArrangeSlNo(vsfDet, vDetSlNo)
            End If
        End With

        If e.KeyCode = Keys.F4 Then
            Dim lngItemID As Integer
            Dim strSQL As String
            Dim rs As oledb.oledbDataReader

            lngItemID = showItemPopup()

            If lngItemID > 0 Then

                strSQL = " Select ITEMID, ITEMNAME, SHORTNAME from MstItem"
                strSQL &= " Where ItemID = " & lngItemID & ""

                rs = clsSVT.getDataReaderFromSQL(strSQL)

                If Not rs.HasRows Then
                    vsfDet.Rows.Count = 1
                    vsfDet.Rows.Count = 2
                    Exit Sub
                End If

                With rs
                    Do While .Read
                        vsfDet.Rows.Count = vsfDet.Rows.Count + 1
                        vsfDet.SetData(vsfDet.Rows.Count - 1, vDetItemName, !ITEMNAME)
                        vsfDet.SetData(vsfDet.Rows.Count - 1, vDetShortName, !SHORTNAME)
                        vsfDet.SetData(vsfDet.Rows.Count - 1, vDetID, !ItemID)
                    Loop
                    .Close()
                End With

                Call RemoveEmptVSFRows(vsfDet, 0, vDetShortName)
                ReArrangeSlNo(vsfDet, vDetSlNo)
            End If
        End If
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Dim strQry As String
        Dim i As Int32

        stringReplace(Me)

        With vsfDet
            If .Rows.Count <= 1 Then
                MsgBox("Grid Empty...", MsgBoxStyle.Critical, Me.Text)
                vsfDet.Focus()
                Exit Sub
            End If

            Call RemoveEmptVSFRows(vsfDet, 1, vDetShortName)

            Try
                strQry = " Delete from " & strTblName & " where QSetName = '" & Trim(cboSel.Text) & "'"
                clsSVT.execSQLStatement(strQry)
                strQry = ""

                For i = 1 To .Rows.Count - 1
                    strQry &= "Insert into " & strTblName & " (QSetName, ItemID) values("
                    strQry &= "'" & Trim(cboSel.Text) & "',"
                    strQry &= "" & Val(.GetData(i, vDetID)) & ")" & vbNewLine
                Next
                If Not clsSVT.execSQLStatement(strQry) Then
                    Exit Sub
                End If
                MsgBox("Data saved successfully.", vbInformation, Me.Text)
                Me.Close()

            Catch ex As Exception
                MsgBox("Error during Save" & vbCrLf & ex.Message, MsgBoxStyle.Critical, Me.Text)

            End Try
        End With
    End Sub

    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click

        If vsfDet.Row < 1 Then
            Exit Sub
        End If
        Dim strSQL As String

        'If vsfDet.Rows.Count = 2 And Trim(vsfDet.GetData(vsfDet.Row, vDetShortName)) = "" Then
        '    MsgBox("No item present to delete!", vbInformation + vbOKOnly, Me.Text)
        '    Exit Sub
        'End If

        If MsgBox("Are you sure you want to Remove Template: " & Trim(cboSel.Text) & " ?", MsgBoxStyle.YesNo + vbDefaultButton2 + MsgBoxStyle.Critical, "Deletion Confirmation...") = vbNo Then Exit Sub

        strSQL = " Delete from " & strTblName & " where QSetName= '" & Trim(cboSel.Text) & "'"

        If clsSVT.execSQLStatement(strSQL) Then
            MsgBox("Delete Successfully", vbInformation, Me.Text)
            vsfDet.Rows.Count = 1
            vsfDet.Rows.Count = 2
            cboSel.Text = ""
            Call bindCombo(cboSel, "QSetName", "QSetName", " Select distinct QSetName from " & strTblName & " order by QSetName")
            Exit Sub
            'Else
            '    MsgBox("Delete Failed", vbInformation, Me.Text)
            '    Exit Sub
        End If
        cboSel.Text = ""
    End Sub

    Private Sub btnLoad_Click(sender As Object, e As EventArgs) Handles btnLoad.Click
        Call fillDtls()
        ReArrangeSlNo(vsfDet, vDetSlNo)
    End Sub

    Private Sub vsfDet_AfterEdit(sender As Object, e As RowColEventArgs) Handles vsfDet.AfterEdit
        Dim intItemID As Long
        intItemID = clsSVT.getDataLongValue("Select ItemID from MstItem where ShortName = '" & vsfDet.GetData(vsfDet.Row, vDetShortName) & "'", 0, False)
        vsfDet.SetData(vsfDet.Row, vDetID, intItemID)
    End Sub

    Private Sub btnRemove_Click(sender As Object, e As EventArgs) Handles btnRemove.Click
        Dim strSQL As String
        If vsfDet.Rows.Count <= 1 Then Exit Sub
        If Trim(vsfDet.GetData(vsfDet.Row, vDetID)) = "" Then Exit Sub
        If vsfDet.Rows.Count > 1 Then
            If MsgBox("Are you sure you want to Remove Item: " & vsfDet.GetData(vsfDet.Row, vDetItemName) & " from  " & cboSel.Text & "  ?", MsgBoxStyle.YesNo + vbDefaultButton2 + MsgBoxStyle.Critical, "Remove Confirmation...") = vbNo Then Exit Sub

            strSQL = " Delete from " & strTblName & " where QSetName='" & Trim(cboSel.Text) & "' and ItemID= " & vsfDet.GetData(vsfDet.Row, vDetID) & ""

            If clsSVT.execSQLStatement(strSQL) Then
                MsgBox("Removed Successfully", vbInformation, Me.Text)
                Call fillDtls()
                Call ReArrangeSlNo(vsfDet, vDetSlNo)
                Exit Sub
            Else
                MsgBox("Delete Failed", vbInformation, Me.Text)
                Exit Sub
            End If
        End If
    End Sub

    Private Function showItemPopup() As Long
        Dim strSQL As String
        Dim i As Int32
        strSQL = "Select ShortName, IsNull(CLBal,0) QOH, MRP, ItemCode, ItemName, ItemGroupName, i.ItemID "
        strSQL &= " From MstItem i, MstItemGroup g, TrnQOH s"
        strSQL &= " where g.ItemGroupID=i.ItemGroupID "
        strSQL &= " And i.InActive = 'N' "
        strSQL &= " And i.ItemID = s.ItemID "
        strSQL &= " order by 1"
        txtDummy.Text = ""
        txtDummy2.Text = ""
        Dim objfrm As New frmUtlSelection
        With objfrm
            .blnMultiColSearch = True
            .FillViewVsf(Me, False, Nothing, strSQL, txtDummy, 0, txtDummy2, 6)
            .Text = " Please Select Item"
            .Tag = " frmMstItemQuickSel "
            .vsfDet.Cols(6).Visible = False
            .ShowDialog()
            .Close()
        End With
        showItemPopup = 0
        For i = 1 To vsfDet.Rows.Count - 1
            If Trim(txtDummy.Text) = Trim(vsfDet.GetData(i, vDetShortName)) Then
                MsgBox("Item Already Present..", vbInformation, Me.Text)
                Exit Function
            End If
        Next
        If Trim(txtDummy.Text) <> "" Then
            showItemPopup = Val(txtDummy2.Text)
        End If
    End Function

    Private Sub btnAddItem_Click(sender As Object, e As EventArgs) Handles btnAddItem.Click
        Dim lngItemID As Integer
        Dim strSQL As String
        Dim rs As oledb.oledbDataReader


        lngItemID = showItemPopup()

        If lngItemID > 0 Then

            strSQL = " Select ITEMID, ITEMNAME, SHORTNAME from MstItem"
            strSQL &= " Where ItemID = " & lngItemID & ""

            rs = clsSVT.getDataReaderFromSQL(strSQL)

            If Not rs.HasRows Then
                vsfDet.Rows.Count = 1
                vsfDet.Rows.Count = 2
                Exit Sub
            End If

            With rs
                Do While .Read
                    vsfDet.Rows.Count = vsfDet.Rows.Count + 1
                    vsfDet.SetData(vsfDet.Rows.Count - 1, vDetItemName, !ITEMNAME)
                    vsfDet.SetData(vsfDet.Rows.Count - 1, vDetShortName, !SHORTNAME)
                    vsfDet.SetData(vsfDet.Rows.Count - 1, vDetID, !ItemID)
                Loop
                .Close()
            End With

            Call RemoveEmptVSFRows(vsfDet, 0, vDetShortName)
            ReArrangeSlNo(vsfDet, vDetSlNo)
        End If
    End Sub
End Class

