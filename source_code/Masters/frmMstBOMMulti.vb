Imports System.ComponentModel
Imports C1.Win.C1FlexGrid

Public Class frmMstBOMMulti

    Dim clsSVT As New svtLibrary

    Const vDetItemName As Byte = 0
    Const vDetOldQty As Byte = 1
    Const vDetQty As Byte = 2
    Const vdetitemid As Byte = 3
    Const vDetBomID As Byte = 4

    Const intDetcols = 5
    Dim strVMode As String
    Dim Mode As String
    Dim svtTrn As oledb.oledbTransaction
    Dim lngBOMID As Long
    Dim lngCode As Long

    Private Sub InitScreen()
        strVMode = ""
        With vsfDet
            .Rows.Count = 2
            .Cols.Count = intDetcols

            .SetData(0, vDetItemName, "Parent (BOM) Item Description")
            .SetData(0, vDetOldQty, "Old Qty")
            .SetData(0, vDetQty, "Changed Qty")
            .SetData(0, vdetitemid, "ItemID")
            .SetData(0, vDetBomID, "BOMID")
            SetHideCol(vsfDet, True, vdetitemid, vDetBomID)
            .Cols(vDetQty).Format = "#.000"
            .Cols(vDetOldQty).Format = "#.000"
            Call SetFlexColor(vsfDet, "", vDetItemName, vDetOldQty)
            Call SetFlexColor(vsfDet, "M", vDetQty)
            Call SetColWidth(vsfDet, 250, 200, 150, 150)
        End With

        Call bindCombo(cboItemGrp, "ItemGroupName", "ItemGroupName", "select distinct ItemGroupName from  MstItemgroup Where ItemGroupId > 0   Order by 1")
        'Call bindCombo(cboItem, "ItemName", "ItemName", "select distinct ItemName from MstItem Where ItemtypeId = 2 and  Itemtype='Finished goods' Order by ItemName")
        Call bindCombo(cboItem, "ItemName", "ItemName", "Select distinct ItemName from MstItem    where BomItem='Y' Order by ItemName")
        ' Call bindCombo(cboTo, "ItemName", "ItemName", "select distinct ItemName from MstItem Where ItemtypeId = 2 and Itemtype='Finished goods'  Order by ItemName")
        Call bindCombo(cboTo, "ItemName", "ItemName", "Select distinct ItemName from MstItem Where Bomitem='Y'  Order by ItemName")

        cboItem.Text = ""
        cboItemGrp.Text = ""
        cboTo.Text = ""
        btnSave.Enabled = True
        grpChangeItem.Visible = False
        strVMode = "V"
        ' ColorChange()
    End Sub

    Private Sub frmMstItemVsBoard_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load

        Call InitScreen()
        lblFrm.Text = "Item-BOM Update"
        Me.KeyPreview = True
        Call initFormSize(Me, Me.Text, False, 958, 579, 0, 0)
        Call SaveActFormVsfDet(Me, Me.Text, "DET", vsfDet)

        ucBtnTools.tsbCopyFrom.Visible = False
        ucBtnTools.tsbPaste.Visible = False
        ucBtnTools.tsbImport.Visible = False
        ucBtnTools.tsbInserDoc.Visible = False
        ucBtnTools.tsbAddItem.Visible = False
        ucBtnTools.tsbInsert.Visible = False
        ucBtnTools.tsbAttachment.Visible = False
        ucBtnTools.tsbPaste.Visible = False
        ucBtnTools.tsbAmmend.Visible = False

        ucBtnTools.tsbCalTotal.Visible = False
        ucBtnTools.tsbCopy.Visible = False
        ucBtnTools.tsbDocExport.Visible = False
        ucBtnTools.tsbFreeze.Visible = False
        ucBtnTools.tsbMoveDown.Visible = False
        ucBtnTools.tsbMoveUp.Visible = False
        ucBtnTools.tsbMultiDoc.Visible = False
        ucBtnTools.tsbRestore.Visible = False
        ucBtnTools.tsbTemplate.Visible = False
        ucBtnTools.tsbTender.Visible = False
        ucBtnTools.tsbBOM.Visible = False
        ucBtnTools.tsbExport.Visible = False
        ucBtnTools.btnVisible("", False, False)
        ucBtnTools.tsbRemove.Visible = True
        ucBtnTools.tsbReplace.Visible = True
        Call ChangeFontSize(Me)
        lblFrm.Font = New Font("Microsoft Sans Serif", 14, FontStyle.Bold)

    End Sub

    Private Sub btnsave_Click(sender As System.Object, e As System.EventArgs) Handles btnSave.Click
        If vsfDet.GetData(1, vDetItemName) = "" Then Exit Sub
        Cursor = Cursors.WaitCursor

        Call SaveDetails()
        If Not Validations() Then Exit Sub
        MsgBox("Saved Successfully!", vbInformation, Me.Text)
        Me.Close()
        Exit Sub
        Mode = ""
        strVMode = "V"

        Cursor = Cursors.Default

    End Sub

    Private Function SaveDetails() As Boolean
        Dim sngItemQty As Single
        Dim strSQL As String
        Dim i As Integer
        Dim intMakeItemID As Integer
        Dim intChangeToItemID As Integer
        Dim intItemId As Long

        strSQL = ""
        intMakeItemID = clsSVT.getDataLongValue("select ItemID from MstItem  Where ItemName = '" & Trim(cboItem.Text) & "'", 0, False)

        intChangeToItemID = clsSVT.getDataLongValue("select ItemID from MstItem Where ItemName = '" & Trim(cboTo.Text) & "'", 0, False)

        svtTrn = svtConn.BeginTransaction(IsolationLevel.ReadCommitted)
        SaveDetails = False
        Try
            For i = 1 To (vsfDet.Rows.Count - 1)
                With vsfDet
                    If Trim(.GetData(i, vDetItemName)) <> "" Then
                        intItemId = Val(.GetData(i, vdetitemid))

                        sngItemQty = Val(.GetData(i, vDetQty))

                        strSQL = "Update MSTDtlBOM set Qty = " & sngItemQty
                        If chkChangeItem.Checked Then
                            strSQL = strSQL & ", ItemID = " & intChangeToItemID
                        End If
                        strSQL = strSQL & " Where BOmID = " & Val(.GetData(i, vDetBomID))
                        strSQL = strSQL & " and ItemId = " & intMakeItemID
                    End If
                    clsSVT.execSQLStatement(strSQL, svtTrn)
                End With
            Next i

            SaveDetails = True
            svtTrn.Commit()
            Cursor = Cursors.Default
            Exit Function

        Catch EX As Exception
            SaveDetails = False
            MsgBox("Error during save" & vbCrLf & EX.Message, MsgBoxStyle.Critical, Me.Text)
            svtTrn.Rollback()
        End Try
    End Function

    Private Function Validations() As Boolean
        Dim i As Integer
        Validations = False
        Try
            If chkChangeItem.Checked Then
                If clsSVT.getDataLongValue("select ItemID from MstItem Where ItemName = '" & Trim(cboTo.Text) & "'", 0, False) = 0 Then
                    MsgBox("Change to Item should not be Blank!!", vbInformation, Me.Text)
                    cboTo.Focus()
                    Exit Function
                End If
            End If

            With vsfDet
                For i = 1 To .Rows.Count - 1
                    If Trim(.GetData(i, vDetItemName)) <> "" And (Trim(.GetData(i, vDetQty)) = "" Or Val(.GetData(i, vDetQty)) = 0) Then
                        MsgBox("Quantity not entered for the Item --> " & .GetData(i, vDetItemName), vbInformation, Me.Text)
                        .Col = vDetQty
                        .Row = i
                        .StartEditing()
                        Exit Function
                    End If
                Next i
            End With
            Validations = True
        Catch ex As Exception
            MsgBox(Err.Description, MsgBoxStyle.Critical, Me.Text)
            Exit Function
        End Try
    End Function

    Private Sub clearFlds()
        LBLUom.Text = ""
        vsfDet.Rows.Count = 1
        vsfDet.Rows.Count = 2
    End Sub

    Private Sub btnItemRemove_Click(sender As System.Object, e As System.EventArgs)
        If Trim(cboItemGrp.Text) = "" Then Exit Sub
        If vsfDet.Rows.Count = 1 Then Exit Sub
        If MsgBox("Do you want to remove this item " & vsfDet.GetData(vsfDet.Row, vDetItemName, vbInformation + vbYesNo) = vbNo) Then Exit Sub
        vsfDet.RemoveItem(vsfDet.Row)
    End Sub


    Private Sub vsfDet_BeforeEdit(sender As Object, e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfDet.BeforeEdit
        Select Case e.Col
            Case vDetOldQty, vdetitemid, vDetItemName, vDetBomID
                e.Cancel = True
            Case vDetQty

            Case Else
        End Select
    End Sub

    Private Sub vsfDet_KeyUp(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles vsfDet.KeyUp
        If e.KeyCode = Keys.Tab And vsfDet.Row = vsfDet.Rows.Count - 1 Then
            If vsfDet.GetData(vsfDet.Rows.Count - 1, vDetItemName) <> "" Then vsfDet.Rows.Count = vsfDet.Rows.Count + 1
            vsfDet.Row = vsfDet.Rows.Count - 1
            vsfDet.Col = vDetItemName
            vsfDet.Focus()
        End If
    End Sub

    Private Sub btncancel_Click(sender As System.Object, e As System.EventArgs) Handles btnCancel.Click
        Call clearFlds()
    End Sub
    Private Sub ColorChange()
        btnCancel.ForeColor = Color.Black
        ' btnItemRemove.ForeColor = Color.Black
        btnSave.ForeColor = Color.Black
        btnClose.ForeColor = Color.Black
    End Sub

    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Public Sub FillBOMItems()
        Dim rs As oledb.oledbDataReader
        Dim strSQL As String, IntItemGrpID As Integer
        Dim lngItem As Long
        If cboItem.Text = "" Then
            MsgBox("Select BOM for Child Item", vbInformation, Me.Text)
            Exit Sub
        End If
        If cboItemGrp.Text = "" Then
            MsgBox("Select BOM Belonging to Group", vbInformation, Me.Text)
            Exit Sub
        End If

        lngItem = clsSVT.getDataLongValue("select ItemID from MstItem Where ItemName = '" & Trim(cboItem.Text) & "'", 0, False)
        'BOM CHANGE
        IntItemGrpID = clsSVT.getDataLongValue("select ItemGroupID from MstItemGroup Where ItemGroupName  = '" & cboItemGrp.Text & "'", 0, False)

        'strSQL = "select h.bomid, h.bomitemid, itemname,i.Itemid, qty "
        strSQL = "select h.bomid,h.bomitemid,  itemname,i.Itemid, qty "
        strSQL = strSQL & " from MstHdrBOM h, MstDtlbom b, mstitem i "
        strSQL = strSQL & " Where h.bomitemid = i.ItemID "
        strSQL = strSQL & " and h.bomid = b.bomid"
        If IntItemGrpID > 0 Then
            strSQL = strSQL & " and i.itemGroupID = " & IntItemGrpID
        End If
        strSQL = strSQL & " and year(h.effto) =9998"
        If lngItem > 0 Then
            strSQL = strSQL & " and b.itemid = " & lngItem
        End If
        strSQL = strSQL & " order by 2"

        rs = clsSVT.getDataReaderFromSQL(strSQL)
        If Not rs.HasRows Then
            rs.Close()
            Exit Sub
        End If
        vsfDet.Rows.Count = 1
        With rs
            Do While .Read
                vsfDet.Rows.Count = vsfDet.Rows.Count + 1

                vsfDet.SetData(vsfDet.Rows.Count - 1, vDetItemName, !itemName)
                vsfDet.SetData(vsfDet.Rows.Count - 1, vDetOldQty, !Qty)
                vsfDet.SetData(vsfDet.Rows.Count - 1, vDetQty, !Qty)
                vsfDet.SetData(vsfDet.Rows.Count - 1, vdetitemid, !ItemID)
                vsfDet.SetData(vsfDet.Rows.Count - 1, vDetBomID, !BomID)
            Loop
        End With
        rs.Close()

    End Sub

    Private Sub btnLoad_Click(sender As Object, e As EventArgs) Handles btnLoad.Click
        vsfDet.Rows.Count = 1
        vsfDet.Rows.Count = 2
        Call FillBOMItems()
    End Sub

    Private Sub frmMstBOMMultiPerf_KeyUp(sender As Object, e As KeyEventArgs) Handles Me.KeyUp
        Dim blnFnKey As Boolean = False

        If e.KeyCode = Keys.F9 And e.Control = True Then
            Call initFormSize(Me, Me.Text, True, 913, 481, 0, 0)
            Call initFormVsfDet(Me, Me.Text, vsfDet, "Detail", Nothing)
            MsgBox("Restored Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
        If e.KeyCode = Keys.F8 And e.Control = True Then
            Call SaveActFormSettings(Me, Me.Text)
            Call SaveActFormVsfDet(Me, Me.Text, "Detail", vsfDet, Nothing)
            MsgBox("Saved Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
        'If Mode <> "V" Then
        If e.Control = True And e.Shift = True And e.KeyCode = Keys.R Then
            Call ucBtnTools.vsfRemove(vsfDet, vsfDet.Row, vDetItemName)
        End If


        If e.Control = True And e.Shift = True And e.KeyCode = Keys.S Then
            Call ucBtnTools.vsfReplace(vsfDet, vsfDet.Row, vsfDet.Col, vDetItemName)
        End If
        ' End If
    End Sub

    Private Sub vsfDet_Click(sender As Object, e As EventArgs) Handles vsfDet.Click
        If strVMode = "V" Then Exit Sub

        With vsfDet
            Select Case .Col
                Case vDetQty
                    .StartEditing()
                Case Else
            End Select
        End With
    End Sub

    Private Sub vsfDet_KeyPressEdit(sender As Object, e As KeyPressEditEventArgs) Handles vsfDet.KeyPressEdit
        If strVMode = "" Then Exit Sub
        If e.KeyChar = "'" Then
            e.Handled = True
        End If
        Select Case e.Col
            Case vDetOldQty, vDetQty
                If KeyValidate("Decimal", e.KeyChar) Then
                    e.Handled = False
                Else
                    e.Handled = True
                End If

        End Select
    End Sub

    Private Sub ucBtnTools_btnClicked(btnValue As Integer) Handles ucBtnTools.btnClicked
        With ucBtnTools
            Select Case btnValue
                Case svtDtlBtns.svtRemove
                    Call .vsfRemove(vsfDet, vsfDet.Row, vDetItemName)
                Case svtDtlBtns.svtReplace
                    Call .vsfReplace(vsfDet, vsfDet.Row, vsfDet.Col, vDetItemName)
            End Select
        End With
    End Sub


    Private Sub chkChangeItem_CheckedChanged(sender As Object, e As EventArgs) Handles chkChangeItem.CheckedChanged
        If chkChangeItem.Checked Then
            grpChangeItem.Visible = True
        Else
            grpChangeItem.Visible = False
        End If

    End Sub

    Private Sub cboTo_Validating(sender As Object, e As CancelEventArgs) Handles cboTo.Validating
        Dim lngTmpItemId As Long
        Dim blnFound As Boolean
        If Trim(cboTo.Text) = "" Then
            grpChangeItem.Visible = False
            Exit Sub
        End If

        'lngTmpItemId = clsSVT.getDataLongValue("select ItemID from MstItem Where ItemName = '" & Trim(cboTo.Text) & "' and ItemgroupId = 2  ", 0, False)
        lngTmpItemId = clsSVT.getDataLongValue("select ItemID from MstItem Where ItemName = '" & Trim(cboTo.Text) & "' and ItemType='Finished Goods'  ", 0, blnFound)
        If Not blnFound Then
            MsgBox("Selected Item NOT Found in Finished Goods List! " & vbCrLf & "Check Entry...", vbInformation, Me.Text)
            e.Cancel = True
            Exit Sub
        End If
        If strVMode = "V" Then
            strVMode = ""
            Exit Sub
        End If
        strVMode = "V"

        grpChangeItem.Visible = False
    End Sub

    Private Sub cboItem_Validating(sender As Object, e As CancelEventArgs) Handles cboItem.Validating
        Dim blnFound As Boolean
        Dim lngItemId As Long
        If chkChangeItem.Checked = False Then Exit Sub

        If Trim(cboItem.Text) = "" Then
            MsgBox("Item NOT Selected! " & vbCrLf & vbCrLf, vbInformation, Me.Text)
            e.Cancel = True
            Exit Sub
        End If
        lngItemId = clsSVT.getDataLongValue("select ItemID from MstItem Where ItemName = '" & Trim(cboItem.Text) & "'and Itemgroupid = 2 ", 0, blnFound)
        If Not blnFound Then
            MsgBox("Item NOT Found in Finished Goods List! " & vbCrLf & "Check Entry...", vbInformation, Me.Text)
            'e.Cancel = True
            Exit Sub
        End If
        lngBOMID = lngItemId
        vsfDet.Refresh()
        Call FillHeader(lngItemId)
        Call fillDetails(lngItemId)
        If vsfDet.Rows.Count = 1 Then
            btnSave.Enabled = True
            btnClose.Enabled = True
            Mode = "A"
        End If

        If Mode = "A" Then
            With vsfDet
                .Rows.Count = .Rows.Count + 1
                strVMode = ""
                '''Changed by sandhya
                If .Rows.Count = 3 AndAlso Trim(.GetData(1, vDetItemName)) = "" Then
                    .RemoveItem(1)
                End If
                .Col = vDetItemName
                .Row = .Rows.Count - 1
                .StartEditing()
            End With
        End If
        If Mode = "M" Then
            With vsfDet
                If .Rows.Count <= 1 Then
                    .Rows.Count= .Rows.Count + 1
                End If
                .Row = 1
                .Col = vDetItemName
                .StartEditing()
            End With
        End If


    End Sub

    Public Sub fillDetails(argID As Long)

        Dim rsTemp As oledb.oledbDataReader

        Dim strItemName As String
        Dim strItemId As String
        Dim strUOM As String
        Dim strItemQty As String
        Dim strItemType As String
        Dim i As Integer
        Dim strsql As String

        Cursor = Cursors.WaitCursor
        lngCode = argID

        If Mode <> "COPY" Then
            Call FillHeader(lngCode)
        End If

        strsql = " Select I.ItemId,ItemName, UOMCode, Qty,t.Itemgroupcode  "
        strsql = strsql & " from MstItem i, MstHdrBOM h, MstDtlBOM b, MstUOM u,Mstitemgroup t"
        strsql = strsql & " Where i.ItemID = b.ItemId and h.bomid = b.bomid "
        strsql = strsql & " and year(effto) = 9998 and  h.BOMItemId = " & argID
        strsql = strsql & " and i.UOMid = u.UOMId "
        strsql = strsql & " and I.itemgroupid=t.itemgroupid"
        rsTemp = clsSVT.getDataReaderFromSQL(strsql)


        With rsTemp
            If Mode <> "COPY" Then
                vsfDet.Rows.Count = 1
            End If
            Do While .Read
                If Mode = "COPY" Then
                    For i = 1 To vsfDet.Rows.Count - 1
                        If UCase(Trim(!itemName)) = UCase(Trim(vsfDet.GetData(i, vDetItemName))) Then GoTo NextItem
                    Next i
                End If

                strItemName = GetVal(!itemName, "")
                strUOM = GetVal(!UOM, "")
                strItemType = GetVal(!ItemType, "")
                strItemQty = GetVal(!MakeQty, "0.00")
                strItemId = GetVal(!ItemID, "0")

                'vsfDet.AddItem strItemName & vbTab & strItemQty _
                '       & vbTab & strUOM & vbTab & strItemId & vbTab & strItemType
Nextitem:
            Loop
            .Close

        End With
        Cursor = Cursors.Default

    End Sub
    Private Sub FillHeader(argID As Long)
        Dim strsql As String
        Dim rsTemp As oledb.oledbDataReader


        Cursor = Cursors.WaitCursor

        lngCode = argID
        strsql = " select ItemName, UOMcode from "
        strsql = strsql & " MstItem i, MStHdrBOM B, MstUOM U "
        strsql = strsql & " Where Year(effTo) =9998 and i.ItemId =  " & argID
        strsql = strsql & " and  i.UOMId = U.UOMId "

        rsTemp = clsSVT.getDataReaderFromSQL(strsql)
        If Not rsTemp.HasRows Then
            rsTemp.Close()
            Exit Sub
        End If
        With rsTemp
            If .HasRows Then
                cboItem.Text = GetVal(!ItemName, "")
                LBLUom.Text = "To Make 1 " & GetVal(!UOMCode, " ") & " Of  " & Trim(cboItem.Text)
            End If
        End With

        Cursor = Cursors.Default

    End Sub


End Class