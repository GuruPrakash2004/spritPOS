Imports System.ComponentModel
Imports C1.Win.C1FlexGrid
Imports System.Text.RegularExpressions

Public Class frmMstItem
    Public intDocID As Integer
    Dim intMenuItemID As Integer
    Dim clsSVT As New svtLibrary
    Dim strRights As String
    Dim activeChild As Object = Nothing
    Dim lngItemID As Long
    Dim Mode As String
    Dim svtTrn As OleDb.OleDbTransaction

    'for audit track
    Dim blnAuditTrack As Boolean = False
    Dim strAuditHdrSQL As String
    Dim strAuditDtlSQL As String

    Dim strDocType As String
    Dim strTblName As String
    Dim strIDName As String
    Dim strCodeName As String
    Dim strPartyName As String
    Dim strFormCaption As String
    Dim strFldCaption As String
    Dim blnShowFileAttach As Boolean
    Dim strFrmCRFrmCaption As String
    Dim strFrmMsg As String
    Dim strFrmPfx As String
    Dim strInventory As String
    Dim strStockLevels As String
    Dim blnItemUsedInTrn As Boolean
    Dim dtSearch As DataTable
    Dim dvSearch As DataView
    Dim strFldSeries As String
    Dim strFldNo As String
    Dim intNo As Integer
    Dim blnAutoGenCode As Boolean = False
    Dim blnAuthReq As Boolean = False
    'Dim intRowInspect As Int32

    Const vItemAlias = 0


    'Consts for vsfFiles
    Const vFSlno = 0
    Const vFDesc = 1
    Const vFName = 2

    'Consts for vsfAUOM
    Const vAUOMID = 0
    Const vAUOM = 1
    Const vAUOMFactor = 2

    Dim width2 As Integer

    Dim blnItemAlternateUOM As Boolean, blnItemStockRelated As Boolean, blnItemPurRelated As Boolean, blnItemAlternateItem As Boolean, blnItemSuppRef As Boolean

    Private Sub AddProc()
        Call clearFlds()
        Mode = "A"

        enableDisableFlds(True)

        vsfSearch.Visible = False
        vsfSearch.SendToBack()
        Call fillVsfHFTDefValues(Me)
        PnlCopyFrom.Visible = True
        PnlCopyFrom.BringToFront()
        txtItemID.Text = clsSVT.getDataLongValue("Select max(" & strIDName & ") + 1 from " & strTblName, 1, False).ToString
        If blnAutoGenCode Then
            '  Call bindCombo(cboSeries, strFldSeries, strFldSeries, "select Distinct " & strFldSeries & " from " & strTblName & " where " & strCodeName & " <> '-' and " & strFldSeries & "<>'' order by " & strFldSeries)
            ' cboSeries.Text = ""
        End If
        StMsg()
    End Sub

    Private Sub clearFlds()
        PnlCopyFrom.Visible = False
        enableDisableFlds(False)
        ' cboSeries.Text = ""
        txtItemCode.Text = ""
        txtItemName.Text = ""
        txtHotKey.Text = ""
        txtShortName.Text = ""
        txtInActiveRem.Text = ""
        txtHSNCode.Text = ""
        txtItemID.Text = ""
        txtSearch.Text = ""

        txtInActiveRem.Text = ""
        Mode = "V"

        chkInactive.Checked = False
        cboIG.Text = Nothing
        cboIG.Text = ""
        cboUOM.Text = Nothing
        cboUOM.Text = ""
        cboType.Text = ""
        txtBotPerCases.Text = ""
        'chkReOrderItem.Checked = False
        'txtROL.Text = ""
        'txtROQ.Text = ""


        txtItemNO.Text = ""
        pnlSearch.Visible = True
        '  lblApproved.Visible = False
        txtSearch.Focus()
        Call ClearAddnlGrid(Me)
        chkNonStkable.Checked = False
        '  chkBOM.Checked = False
        ' vsfView.Visible = False
        '  vsfItem.Rows.Count = 1
        '  vsfItem.Rows.Count = 2
        vsfAlias.Rows.Count = 1
        vsfAlias.Rows.Count = 2
        vsfFiles.Rows.Count = 0
        ' vsfAUOM.Rows.Count = 1
        ' vsfAUOM.Rows.Count = 2
        btnFileAttach.Text = "File Attachment"
        btnCopyFrom.Visible = False
    End Sub

    Private Sub frmMstItem_FormClosing(sender As Object, e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        If Mode = "A" Or Mode = "M" Then
            If MsgBox("Are you sure you want to exit the form without saving?!", MsgBoxStyle.Question _
                      + MsgBoxStyle.YesNo + vbDefaultButton2, "Confirmation...") = MsgBoxResult.No Then
                e.Cancel = True
                Exit Sub
            End If
        End If
    End Sub

    Private Sub frmMstItem_KeyUp(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyUp
        Dim blnFnKey As Boolean = False
        Select Case e.KeyCode
            Case Keys.F1, Keys.F2, Keys.F3, Keys.F4, Keys.F5, Keys.F6, Keys.F7, Keys.F8, Keys.F9, Keys.F10, Keys.F11, Keys.F12
                blnFnKey = True
        End Select

        If (e.Control) Or blnFnKey Then
            Call svtBtnCtrl.btnCtrlKeyUp(sender, e)
        End If

        If e.KeyCode = Keys.F5 Then
            Call fillCombo()
        End If

        If e.KeyCode = Keys.F9 And e.Control = True Then
            Call initFormSize(Me, Me.Text, True, 1159, 788, 0, 0)
            Call initFormVsfDet(Me, Me.Text, vsfHdr, "HDR")
            MsgBox("Restored Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
        If e.KeyCode = Keys.F8 And e.Control = True Then
            Call SaveActFormSettings(Me, Me.Text)
            Call SaveActFormVsfDet(Me, Me.Text, "HDR", vsfHdr)
            Call SaveActFormVsfDet(Me, Me.Text, "SEARCH", vsfSearch)
            'Call SaveActFormVsfDet(Me, Me.Text, "VIEW", vsfView)
            MsgBox("Saved Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
        If Mode <> "A" Then
            If e.KeyCode = Keys.A And e.Control = True Then
                Call btnFileAttach_ClickButtonArea(sender, Nothing)
            End If
        End If
    End Sub

    Public Sub initForm(ByVal argDocID As Integer)
        If argDocID > 0 Then intDocID = argDocID
        Dim rs As OleDb.OleDbDataReader
        rs = clsSVT.getDataReaderFromSQL(" Select * from UtlTransactions where DocID = " & argDocID)
        With rs
            .Read()
            strFrmMsg = Trim(Replace(!Docname, "Master", ""))
            strFormCaption = !DocName
            strTblName = !HdrTblName
            strCodeName = Trim(Replace(!HdrTblName, "Mst", "")) & "Code"
            strIDName = Trim(Replace(!HdrTblName, "Mst", "")) & "ID"
            strFrmPfx = Trim(Replace(!HdrTblName, "Mst", ""))
            strFldSeries = Replace(!HdrTblName, "Mst", "") & "Series"
            strFldNo = Replace(!HdrTblName, "Mst", "") & "No"
            strInventory = GetVal(!MstInventory, "")
            strStockLevels = GetVal(!MstStockLevels, "")
            lblDispItemCode.Text = strFormCaption
            'for audit track
            If GetVal(!AuditLogTrack, "") = "Y" Then
                blnAuditTrack = True
                strAuditHdrSQL = GetVal(!AuditSQLHdr, "")
                strAuditDtlSQL = GetVal(!AuditSQLDtl, "")
            End If
            If GetVal(!AuthReq, "N") = "Y" Then
                blnAuthReq = True
            Else
                blnAuthReq = False
            End If
            If !MstAutoGenCode = "Y" Then
                blnAutoGenCode = True
                txtItemCode.TextAlign = HorizontalAlignment.Center
            Else
                blnAutoGenCode = False
                lblItemCode.Left = lblItemName.Left
                txtItemCode.Left = txtItemName.Left
                txtItemCode.Width = txtItemName.Width
                txtItemCode.TextAlign = HorizontalAlignment.Left
            End If
            If !FileAttachReq = "Y" Then
                blnShowFileAttach = True
            Else
                blnShowFileAttach = False
            End If
            Call setAutoGenCode()
            .Close()
        End With
        intMenuItemID = clsSVT.getDataLongValue("Select MenuItemID from MstMenu  where DocID = " & intDocID, 0, False)
        strRights = getUserRights(intMenuItemID)

        lblItemCode.Text = TITLECASE(strFrmPfx) & " Code"
        lblItemName.Text = TITLECASE(strFrmPfx) & " Name"

        Me.Text = strFormCaption
        Call initScreen()
        Call setSearchGridHt("")

        If Not blnShowFileAttach Then
            PnlAttach.Visible = False
        Else
            PnlAttach.Visible = True
        End If

        KeyPreview = True
        GetMiscFlds()
        Call formProp()
    End Sub

    Public Sub GetItemMiscFlds()
        Dim rsDummy As OleDb.OleDbDataReader

        rsDummy = clsSVT.getDataReaderFromSQL("Select * from UtlMisc")
        With rsDummy
            If .HasRows Then
                .Read()

                If GetVal(!ItemAlternateUOM, "N") = "Y" Then
                    blnItemAlternateUOM = True
                Else
                    blnItemAlternateUOM = False
                End If

                If GetVal(!ItemStockRelated, "N") = "Y" Then
                    blnItemStockRelated = True
                Else
                    blnItemStockRelated = False
                End If

                If GetVal(!ItemPurRelated, "N") = "Y" Then
                    blnItemPurRelated = True
                Else
                    blnItemPurRelated = False
                End If

                If GetVal(!ItemAlternateItem, "N") = "Y" Then
                    blnItemAlternateItem = True
                Else
                    blnItemAlternateItem = False
                End If

                If GetVal(!ItemSuppRef, "N") = "Y" Then
                    blnItemSuppRef = True
                Else
                    blnItemSuppRef = False
                End If

            End If
            .Close()
        End With
    End Sub

    Private Sub frmMstParty_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        strRights = getUserRights(GetMenuId(Me.Text))

        'Call initScreen()
        Call setSearchGridHt("")
        KeyPreview = True
        svtBtnCtrl.btnNew.Focus()
        Call initFormVsfDet(Me, Me.Text, vsfHdr, "HDR")
        Call initFormVsfDet(Me, Me.Text, vsfSearch, "SEARCH")
        ' Call initFormVsfDet(Me, Me.Text, vsfView, "VIEW")
        Call formProp()
        txtSearch.Focus()
        fillFormProperties(Me)
        Call FillDocDetails(Val(txtItemID.Text))
        svtBtnCtrl.FlowLayoutPanel1.BackColor = Color.Transparent
        svtBtnCtrl.BackColor = Color.Transparent

        vsfHdr.Styles.EmptyArea.BackColor = Color.Transparent
        vsfHdr.BackColor = Color.Transparent
        tbAdnlFlds.TabPages(0).BackColor = gChildFormBackColor

        lblDispItemCode.ForeColor = Color.White
        lblSearch.ForeColor = Color.White
        Call initFormSize(Me, Me.Text, False, 1159, 788, 0, 0)

        ' pnlGroups.Visible = True : Panel5.Visible = True
        ' If blnItemAlternateUOM Then gbAUOM.Visible = True
        ' If blnItemStockRelated Then gbStk.Visible = True
        ' If blnItemPurRelated Then gbPur.Visible = True
        ' If blnItemAlternateItem Then pnlItem.Visible = True
        ' If blnItemSuppRef Then pnlSupp.Visible = True
        ' If pnlItem.Visible Or pnlSupp.Visible Then
        'Panel5.Visible = True
        'Else
        'Panel5.Visible = False
        'End If
        '  If gbPur.Visible = False And gbStk.Visible = False Then pnlGroups.BorderStyle = BorderStyle.None
        'pnlGroups.BorderStyle = BorderStyle.None
        ' Call setflwpnlhght()
        'pnlGroups.Visible = True
        If Not chkUserRights(strRights, chkRightsPos.AddRights, Me.Text, False) Then svtBtnCtrl.btnNew.Visible = False
        If Not chkUserRights(strRights, chkRightsPos.EditRights, Me.Text, False) Then svtBtnCtrl.btnEdit.Visible = False
        If Not chkUserRights(strRights, chkRightsPos.DelRights, Me.Text, False) Then svtBtnCtrl.btnDelete.Visible = False
        If Not chkUserRights(strRights, chkRightsPos.opt1Rights, Me.Text, False) Then svtBtnCtrl.btnOptions.Visible = False

        ' vsfSearch.Width = width2
        'vsfSearch.Show()
        Call ChangeFontSize(Me)
        lblDispItemCode.Font = New Font("Microsoft Sans Serif", 14, FontStyle.Bold)
        lblSearch.Font = New Font("Segoe UI", 9.25, FontStyle.Bold)
        pnlReOrder.Visible = False
        vsfHdr.Styles("Normal").WordWrap = False
        vsfHdr.AllowResizing = AllowResizingEnum.Both
        vsfHdr.AutoSizeRows()
    End Sub
    Private Sub formProp()
        svtBtnCtrl.btnOptions.Visible = True
        svtBtnCtrl.btnPrint.Visible = False
    End Sub
    Private Sub initScreen()
        Dim strSQL As String

        Call initGridStyles(Me)

        Call InitAddnlFldGrids(Me, vsfHdr, "Header", intDocID, tbAdnlFlds)
        txtInActiveRem.Visible = False
        Call InsertDataTbl()
        Call fillCombo()

        Call clearFlds()

        With vsfAlias
            .Rows.Count = 2
            .Cols.Count = 1
            .SetData(0, 0, "Alias Name")
            .Cols(vItemAlias).TextAlign = TextAlignEnum.LeftCenter
            SetRowAlign(vsfAlias, svtAlign.Center, 0)
        End With

        'With vsfItem
        '    .Rows.Count = 2
        '    .Cols.Count = 1
        '    .SetData(0, 0, "Alternate Items")
        '    '   .Cols(vItemName).ComboList = "..."
        'End With

        'With vsfAUOM
        '    .Rows.Count = 2
        '    .Cols.Count = 3
        '    .SetData(0, vAUOM, "Alternate UOM")
        '    .SetData(0, vAUOMFactor, "Conv Factor")
        '    .Cols(vAUOMID).Visible = False
        '    '   .Cols(vItemName).ComboList = "..."
        'End With

        With vsfFiles
            .Cols.Count = 3
            .Rows.Count = 0
            .Cols(vFSlno).Visible = False
            .Cols(vFName).Visible = False
        End With

        strSQL = "Select Max(" & strIDName & ") from " & strTblName
        txtItemID.Text = clsSVT.getDataLongValue(strSQL, 0, False)
        PnlCopyFrom.Visible = False

        If blnAuthReq Then
            svtBtnCtrl.addOption("Approve")
        End If
        svtBtnCtrl.addOption("Items-DataLink Problem")
        'svtBtnCtrl.addOption("Print BarCode")
        svtBtnCtrl.addOption("Import Item from Excel")
        svtBtnCtrl.addOption("Update Stock from Excel")
        svtBtnCtrl.addOption("Item Vs Brand")
        svtBtnCtrl.addOption("Update Item Master Fields")
        svtBtnCtrl.addOption("Update Item Rates")

        'added by Akshay as per kumar sir instruction 28/09/23
        svtBtnCtrl.addOption("Print Barcode")

        Call GetItemMiscFlds()

        ' intRowInspect = getRowNoFromAddnlGrid(vsfHdr, "INSPNOTREQ")
    End Sub

    'Private Sub setflwpnlhght()
    '    flwpnl.Height = 0
    '    For Each gb As Control In flwpnl.Controls
    '        If gb.Visible = True Then
    '            flwpnl.Height = flwpnl.Height + gb.Height
    '        End If
    '    Next
    '    flwpnl.Height = flwpnl.Height + 20
    'End Sub
    Private Sub InsertDataTbl()
        Dim recdummy As OleDb.OleDbDataReader
        Dim strSQL As String

        strSQL = " select * from " & strTblName & " where " & strIDName & " = 0 "
        recdummy = clsSVT.getDataReaderFromSQL(strSQL)

        If recdummy.HasRows = False Then
            strSQL = " Insert into " & strTblName & " ("
            strSQL = strSQL & strIDName & "," & strFldSeries & "," & strFldNo & "," & strCodeName & ", " & strFrmPfx & "Name" & ","
            ' strSQL = strSQL & strPartyType & ","
            strSQL = strSQL & " InActive,AddedBy,AddedOn,ChangedBy,ChangedOn)"
            strSQL = strSQL & " values("
            strSQL = strSQL & " 0,'',0,'-', '-',"
            strSQL = strSQL & " 'N','ADMIN',"
            strSQL = strSQL & "'" & getDateFormatInInsert(getSystemDateTime, True) & "',"
            strSQL = strSQL & "'',"
            strSQL = strSQL & "'" & getDateFormatInInsert(getSystemDateTime, True) & "')"
            Call clsSVT.execSQLStatement(strSQL)
        End If
        recdummy.Close()

    End Sub

    Private Sub fillCombo()
        Call bindCombo(cboIG, "ItemGroupCode", "ItemGroupCode", "select  distinct ItemGroupCode from MstItemGroup where  InActive = 'N' and Authorised = 'Y' order by ItemGroupCode")
        Call bindCombo(cboUOM, "UOMCode", "UOMCode", "Select Distinct UOMcode from MstUOM where  InActive = 'N' and UOMCode <>'-' order by UOMCode")
        '  Call bindCombo(cboSeries, strFldSeries, strFldSeries, "select Distinct " & strFldSeries & " from " & strTblName & " where " & strCodeName & " <> '-' order by " & strFldSeries)
    End Sub


    Private Function ValidFields() As Boolean
        Dim blnFound As Boolean
        Dim strItemAlias As String
        Dim strList As String
        Dim strSQL As String = ""
        Dim intItemID As Long

        If Trim(txtItemID.Text) = "" Then
            txtItemID.Text = intItemID
        End If
        intItemID = Val(txtItemID.Text)


        If vsfAlias.Rows.Count > 1 Then
            If vsfAlias.GetData(1, vItemAlias) <> "" Then
                vsfAlias.EndUpdate()
            End If
            'vsfSupp.Editor.Dispose()
        End If

        Call RemoveEmptVSFRows(vsfAlias, 1, vItemAlias)

        StMsg("Validating Data.....")
        If Not IsNothing(vsfHdr.Editor) Then
            If vsfHdr.Editor.Text <> "" Then
                vsfHdr.SetData(vsfHdr.Row, vOthFldDataValue, vsfHdr.Editor.Text)
            End If
        End If

        ValidFields = False

        If Mode = "A" Then
            txtItemID.Text = clsSVT.getDataLongValue("Select max(" & strIDName & ") + 1 from " & strTblName, 1, False).ToString
        End If

        If txtItemCode.Text = "" Then
            MsgBox("Please enter the " & (StrConv(strCodeName, VbStrConv.ProperCase)) & "!", MsgBoxStyle.Information, Me.Text)
            txtItemCode.Focus()
            Exit Function
        End If

        ' if ItemName is blank
        If txtItemName.Text = "" Then
            MsgBox("Please enter the " & (StrConv(strFrmPfx, VbStrConv.ProperCase)) & " name!", MsgBoxStyle.Information, Me.Text)
            txtItemName.Focus()
            Exit Function
        End If

        If Trim(cboIG.Text) = "" And cboIG.Enabled = True Then
            MsgBox("Please select an " & (StrConv(strFrmPfx, VbStrConv.ProperCase)) & "Group from the drop-down list!", MsgBoxStyle.Information, Me.Text)
            cboIG.Focus()
            Exit Function
        End If

        If Trim(cboType.Text) = "" Then
            MsgBox("Please select Itemtype from the drop-down list!", MsgBoxStyle.Information, Me.Text)
            cboType.Focus()
            Exit Function
        End If

        If Trim(cboType.Text) <> "" Then
            If cboType.Items.Contains(cboType.Text) = False Then
                MsgBox("Not a valid ItemType!", vbInformation, Me.Text)
                cboType.Focus()
                Exit Function
            End If
        End If

        Call clsSVT.getDataStringValue("Select " & strCodeName & " from " & strTblName _
               & " where " & strCodeName & " ='" & Trim(txtItemCode.Text) & "' and " & strIDName & " <> " & Val(txtItemID.Text), "", blnFound)
        If blnFound Then
            MsgBox(strCodeName & " already exists..", vbInformation, Me.Text)
            ' txtItemName.Text = ""
            txtItemCode.Focus()
            Exit Function
        End If
        Call clsSVT.getDataStringValue("Select " & strFrmPfx & "Name from " & strTblName _
               & " where " & strFrmPfx & "Name ='" & Trim(txtItemName.Text) & "' and " & strIDName & " <> " & Val(txtItemID.Text), "", blnFound)
        If blnFound Then
            ''''''''''''''''Changed code
            'MsgBox(StrConv(strFrmPfx, VbStrConv.ProperCase) & " Name already exists..", vbInformation, Me.Text)
            '' txtItemName.Text = ""
            'txtItemName.Focus()
            'Exit Function
            ''''''''''''''''Changed code
        End If

        Call clsSVT.getDataStringValue("Select " & strFrmPfx & "GroupID from " & strTblName & "Group where " & strFrmPfx & "GroupCode = '" & Trim(cboIG.Text) & "'", "", blnFound)

        If blnFound = False Then
            MsgBox("Selected " & (StrConv(strFrmPfx, VbStrConv.ProperCase)) & "Group not found in master!", MsgBoxStyle.Critical, Me.Text)
            cboIG.Text = ""
            cboIG.Focus()
            Exit Function
        End If

        If Trim(cboUOM.Text) = "" And cboUOM.Enabled = True Then
            MsgBox("Please select an UOMCode from the drop-down list!", MsgBoxStyle.Information, Me.Text)
            cboUOM.Focus()
            Exit Function
        End If

        Dim intUOMId As Integer
        intUOMId = clsSVT.getDataLongValue("Select UOMID from MstUOM where UOMCode = '" & Trim(cboUOM.Text) & "'", 0, False)

        If intUOMId = 0 Then
            MsgBox("Selected UOMCode not found in the master !", MsgBoxStyle.Critical, Me.Text)
            cboUOM.Text = ""
            cboUOM.Focus()
            Exit Function
        End If

        If Not validOthFLds(Me, strTblName, strIDName, Val(txtItemID.Text)) Then
            Exit Function
        End If

        If chkInactive.Checked And Trim(txtInActiveRem.Text) = "" Then
            MsgBox("Please enter remarks for why the item is inactive!", MsgBoxStyle.Critical, Me.Text)
            txtInActiveRem.Focus()
            Exit Function
        End If
        If ChkDuplicateGridData(vsfAlias, True, vItemAlias) <> -1 Then
            Exit Function
        End If

        For i = 1 To vsfAlias.Rows.Count - 1
            If UCase(Trim(txtItemCode.Text)) = UCase(Trim(vsfAlias.GetData(i, vItemAlias))) Then
                MsgBox("ItemCode and Alias cannot be same ", MsgBoxStyle.Critical, Me.Text)
                Exit Function
            End If
        Next

        strItemAlias = ""
        With vsfAlias
            For i As Int32 = 1 To .Rows.Count - 1
                If Trim(.GetData(i, vItemAlias)) <> "" Then
                    strItemAlias &= "'" & .GetData(i, vItemAlias) & "',"
                End If
            Next
        End With
        strList = "'" & Trim(txtItemCode.Text) & "'"
        If strItemAlias <> "" Then strList = strList & "," & strItemAlias
        If Trim(txtShortName.Text) <> "" Then strList = strList & "'" & Trim(txtShortName.Text) & "'"
        If Trim(txtHotKey.Text) <> "" Then strList = strList & "'" & Trim(txtHotKey.Text) & "',"
        'strList = strList.Replace("''", "")
        'strList = strList.Replace(",,", ",")
        If Strings.Right(strList, 1) = "," Then strList = Strings.Mid(strList, 1, strList.Length - 1)

        strSQL = "Select ItemID from MstItem where ItemCode in (" & strList & ") And ItemID <> " & Val(txtItemID.Text)
        If clsSVT.getDataLongValue(strSQL, 0, False) > 0 Then
            MsgBox("Duplicate Item Code", MsgBoxStyle.Information, Me.Text)
            Exit Function
        End If

        strSQL = "Select ItemID from mstitemAlias  where ItemAlias in (" & strList & ") And ItemID <> " & Val(txtItemID.Text) & "  And ItemAlias <> ''"
        If clsSVT.getDataLongValue(strSQL, 0, False) > 0 Then
            MsgBox("Duplicate Item Aliase ", MsgBoxStyle.Information, Me.Text)
            Exit Function
        End If

        strSQL = "Select ItemID from MstItem where ShortName in (" & strList & ") And ItemID <> " & Val(txtItemID.Text)
        If clsSVT.getDataLongValue(strSQL, 0, False) > 0 Then
            MsgBox("Duplicate ShortName ", MsgBoxStyle.Information, Me.Text)
            Exit Function
        End If

        strSQL = "Select ItemID from MstItem where HotKey in (" & strList & ") And ItemID <> " & Val(txtItemID.Text) & " And HotKey<>''"
        If clsSVT.getDataLongValue(strSQL, 0, False) > 0 Then
            MsgBox("Duplicate HotKey ", MsgBoxStyle.Information, Me.Text)
            Exit Function
        End If

        'sell rate
        If gDblPurSellVarPer > 0 And Trim(cboType.Text.ToUpper) <> "OTHERS" Then
            Dim dblPurPrice As Double = Val(getValueFromAddnlGrid("PURRATE", vsfHdr))
            Dim dblSellPrice As Double = Val(getValueFromAddnlGrid("ItemSell", vsfHdr))
            Dim dblPer As Double = Math.Round(((dblSellPrice / dblPurPrice * 100) - 100), 2)
            If dblPer < gDblPurSellVarPer Then
                MsgBox("Purchase and Sales Variation (" & dblPer & "%) to be greater than " & gDblPurSellVarPer & "%", MsgBoxStyle.Critical, Me.Text)
                Exit Function
            End If
        End If
        ValidFields = True
    End Function

    Private Function saveDetails() As Boolean
        Dim strSQL As String = ""
        Dim StrSqlA As String = ""
        Dim strValues As String = ""
        Dim strFlds As String = ""
        saveDetails = False
        Dim intID As Long, strItemCode As String
        Dim rsItem As OleDb.OleDbDataReader
        Dim dtPrevID As DataTable, dtCurID As DataTable
        strItemCode = ""
        Dim strEarlierFlds As String = ""
        If Mode = "M" Then
            dtPrevID = clsSVT.getDATATable("Select * from mstitem where itemID = " & Val(txtItemID.Text))
        End If
        If Mode = "A" Then
            intID = clsSVT.AutoIncrementedNum(strIDName, strTblName)
            txtItemID.Text = intID

            strSQL = "Insert into " & strTblName & "("
            strSQL = strSQL & strIDName & "," & strCodeName & ", " & strFrmPfx & "Name, ShortName, HotKey, HSNCode, " & strFrmPfx & "GroupID, LiqType,"
            strSQL = strSQL & " NonStkable, InActive, InActiveRmks,"
            strSQL = strSQL & " UOMID,"
            strSQL = strSQL & " AddedBy, AddedOn,ChangedBy,ChangedOn, "
            strSQL = strSQL & " BotPerCases"
            Call GetInsertString(Me, strValues, strFlds)
            strSQL = strSQL & strFlds
            strSQL = strSQL & ") values ("

            strSQL = strSQL & intID & ","
            strSQL = strSQL & "'" & Trim(txtItemCode.Text) & "',"
            strSQL = strSQL & "'" & Trim(txtItemName.Text) & "',"
            strSQL = strSQL & "'" & Trim(txtShortName.Text) & "',"
            strSQL = strSQL & "'" & Trim(txtHotKey.Text) & "',"
            strSQL = strSQL & "'" & Trim(txtHSNCode.Text) & "',"
            strSQL = strSQL & clsSVT.getDataLongValue("select ItemGroupID from MStItemGroup where ItemGroupCode = '" & Trim(cboIG.Text) & "'", 0, False) & ","
            strSQL = strSQL & "'" & Trim(cboType.Text) & "',"
            strSQL = strSQL & "'" & getCheckString(chkNonStkable) & "',"
            strSQL = strSQL & "'" & getCheckString(chkInactive) & "',"
            strSQL = strSQL & "'" & Trim(txtInActiveRem.Text) & "',"
            strSQL = strSQL & clsSVT.getDataLongValue("select UOMID from MstUOM where  UOMCode = '" & cboUOM.Text & "'", 0, False) & ","
            strSQL = strSQL & "'" & gUserCode & "', "
            strSQL = strSQL & "'" & getDateFormatInInsert(getSystemDateTime(), True) & "','',Null"

            strSQL = strSQL & ", " & Val(txtBotPerCases.Text) & ""
            '--5 begin OthGRID
            strSQL = strSQL & strValues
        Else
            strSQL = "update " & strTblName & " set "
            strSQL = strSQL & strCodeName & " = '" & Trim(txtItemCode.Text) & "',"
            strSQL = strSQL & " " & strFrmPfx & "Name = '" & Trim(txtItemName.Text) & "',"
            strSQL = strSQL & "  ShortName = '" & Trim(txtShortName.Text) & "',"
            strSQL = strSQL & " HotKey = '" & Trim(txtHotKey.Text) & "',"

            strSQL = strSQL & " HSNCode = '" & Trim(txtHSNCode.Text) & "',"
            strSQL = strSQL & " ItemGroupID = " & clsSVT.getDataLongValue("select ItemGroupID from MStItemGroup where ItemGroupCode = '" & Trim(cboIG.Text) & "'", 0, False) & ","
            strSQL = strSQL & " LiqType = '" & Trim(cboType.Text) & "',"
            strSQL = strSQL & " NonStkable = '" & getCheckString(chkNonStkable) & "',"
            strSQL = strSQL & " Inactive = '" & getCheckString(chkInactive) & "',"
            strSQL = strSQL & " InActiveRmks = '" & Trim(GetVal(txtInActiveRem.Text, "")) & "',"
            strSQL = strSQL & " UOMID = " & clsSVT.getDataLongValue("select UOMID from MstUOM where  UOMCode = '" & cboUOM.Text & "'", 0, False) & ","

            strSQL = strSQL & " ChangedBy= '" & gUserCode & "',"
            strSQL = strSQL & " ChangedOn= '" & getDateFormatInInsert(getSystemDateTime(), True) & "' "
            strSQL = strSQL & ", BotPerCases= " & Val(txtBotPerCases.Text) & ""


            '--3 begin OthGRID
            Call GetUpdateString(Me, strFlds)
            strSQL = strSQL & strFlds

            strSQL = strSQL & " where " & strIDName & " =  " & Val(txtItemID.Text)
        End If
        strSQL = Replace(Replace(strSQL, vbCrLf, ""), vbCr, "")
        saveDetails = False

        svtTrn = svtConn.BeginTransaction(IsolationLevel.ReadCommitted)
        Try
            If clsSVT.execSQLStatement(strSQL, svtTrn) Then
                If Mode = "M" Then
                    clsSVT.execSQLStatement("Delete from MstItemAlias where ItemId=" & txtItemID.Text, svtTrn)
                End If
                For i = 1 To vsfAlias.Rows.Count - 1
                    strSQL = "Insert into MstItemAlias(ItemId,ItemAlias) values("
                    strSQL &= "" & Val(txtItemID.Text) & ","
                    strSQL &= "'" & vsfAlias.GetData(i, 0) & "')"
                    clsSVT.execSQLStatement(strSQL, svtTrn)
                Next
                lngItemID = Val(txtItemID.Text)
                'If Mode = "A" Then
                '    strSQL = "Insert Into TrnQOH (ItemID) Values (" & Val(txtItemID.Text) & ")"
                '    If Not clsSVT.execSQLStatement(strSQL, svtTrn) Then
                '        saveDetails = False
                '        svtTrn.Rollback()
                '        Exit Function
                '    End If
                'End If
                'for audit track
                If blnAuditTrack Then
                    Call RecordAuditLog("ITEM MASTER", Val(txtItemID.Text), IIf(Mode = "A", UserActions.AddAction, UserActions.EditAction), strTblName, strAuditHdrSQL, strAuditDtlSQL, svtTrn)
                End If

                If gBlnCheckForFraud Then
                    If Mode = "A" Then
                        strEarlierFlds = ""
                        rsItem = clsSVT.getDataReaderFromSQL("Select * from mstitem where itemID = " & Val(txtItemID.Text), svtTrn)
                        Do While rsItem.Read
                            For i = 0 To rsItem.FieldCount - 1
                                strEarlierFlds &= rsItem.GetName(i) & " : " & rsItem.GetValue(i).ToString & ", "
                            Next
                        Loop
                        rsItem.Close()
                    Else
                        dtCurID = clsSVT.getDATATable("Select * from mstitem where itemID = " & Val(txtItemID.Text), Nothing, svtTrn)
                        Dim Prow As DataRow = dtPrevID.Rows(0)
                        Dim Crow As DataRow = dtCurID.Rows(0)
                        For i = 0 To dtPrevID.Columns.Count - 1
                            If Prow(i).ToString() <> Crow(i).ToString And dtPrevID.Columns(i).ColumnName.ToString.ToUpper <> "CHANGEDON" Then
                                strEarlierFlds &= "{ " & dtPrevID.Columns(i).ColumnName.ToString & " : changed from [" & Prow(i).ToString & "] to [" & Crow(i).ToString & "] }  "
                            End If
                        Next
                    End If

                    strSQL = "Insert into UtlAdmAudit (ScreenName,DAction,DocDate,DocDateTime,DocSysName,SysUserName,MainFldID,MainFld, EarlierData,ChangedData) values ("
                    strSQL &= "'Item Master',"
                    If Mode = "A" Then
                        strSQL &= "'Added',"
                    Else
                        strSQL &= "'Edited',"
                    End If
                    strSQL &= "'" & getDateFormatInInsert(getSystemDate(svtTrn)) & "',"
                    strSQL &= "'" & getDateFormatInInsert(getSystemDateTime(svtTrn), True) & "',"
                    strSQL &= "'" & gStrSystemName & "',"
                    strSQL &= "'" & gStrSystemUserName & "',"
                    strSQL &= "'" & txtItemID.Text & "',"
                    strSQL &= "'" & txtItemCode.Text & "',"
                    strSQL &= "'',"
                    strSQL &= "'" & strEarlierFlds & "')"
                    Call clsSVT.execSQLStatement(strSQL, svtTrn)
                End If

                If Mode = "M" Then
                    'item rate audit log
                    Dim Prow As DataRow = dtPrevID.Rows(0)
                    Dim dblOldMRP As Double = 0, dblOldPurRate As Double = 0
                    For i = 0 To dtPrevID.Columns.Count - 1
                        Select Case dtPrevID.Columns(i).ColumnName.ToString.ToUpper
                            Case "MRP"
                                dblOldMRP = Val(Prow(i).ToString)
                            Case "PURRATE"
                                dblOldPurRate = Val(Prow(i).ToString)
                        End Select
                    Next
                    If dblOldMRP <> Val(getValueFromAddnlGrid("MRP", vsfHdr)) Or dblOldPurRate <> Val(getValueFromAddnlGrid("PURRATE", vsfHdr)) Then
                        strSQL = "Insert into MstItemAudit (ItemID, Itemcode, Itemname, ISource, OldMRP, OldPurRate, NewMRP, NewPurRate,  CSystemName, CUserName) values ("
                        strSQL &= Val(txtItemID.Text) & ","
                        strSQL &= "'" & txtItemCode.Text & "',"
                        strSQL &= "'" & txtItemName.Text & "','From Item Master',"
                        strSQL &= dblOldMRP & ","
                        strSQL &= dblOldPurRate & ","
                        strSQL &= Val(getValueFromAddnlGrid("MRP", vsfHdr)) & ","
                        strSQL &= Val(getValueFromAddnlGrid("PURRATE", vsfHdr)) & ","
                        strSQL &= "'" & gStrSystemName & "',"
                        strSQL &= "'" & gUserCode & "')"
                        Call clsSVT.execSQLStatement(strSQL, svtTrn)
                    End If
                End If

                saveDetails = True
                svtTrn.Commit()
                If Mode = "A" Then
                    MsgBox(StrConv(strFrmPfx, VbStrConv.ProperCase) & " '" & Trim(txtItemCode.Text) & "' added successfully.", vbInformation, Me.Text)
                Else
                    MsgBox("Updated successfully.", vbInformation, Me.Text)
                End If

                'Call clearFlds()
                Call FillDocDetails(txtItemID.Text, True)
            Else
                svtTrn.Rollback()
                Exit Function
            End If
        Catch ex As Exception
            MsgBox("Error during save" & vbCrLf & ex.Message, MsgBoxStyle.Critical, Me.Text)
            svtTrn.Rollback()
            Exit Function
        End Try
    End Function

    Public Function getOptionString(ByVal argOption As RadioButton) As String
        If argOption.Checked Then
            getOptionString = "Y"
        Else
            getOptionString = "N"
        End If
    End Function

    Public Sub vsfHdr_AfterEdit(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfHdr.AfterEdit

        Call vsfGenAfterEditBfrCalTot(Me, vsfHdr, e.Row, "M")
        Call CalTotal()
        Call vsfGenAfterEditAftrCalTot(Me, vsfHdr, intDocID, Mode, e.Row, False)

        vsfHdr.Styles("Normal").WordWrap = False
        vsfHdr.AllowResizing = AllowResizingEnum.Both
        vsfHdr.AutoSizeRows()

        'Call AutosizeMstfrmGrid(frmMstParty)
    End Sub

    Public Sub vsfHdr_BeforeEdit(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfHdr.BeforeEdit
        If Mode = "V" Or Mode = "" Then Exit Sub

        If vsfBeforeEditCancel(Me, vsfHdr, intDocID, strTblName, Mode, e.Row, e.Col, "M", "") Then
            e.Cancel = True
            Exit Sub
        End If
    End Sub

    Public Sub vsfHdr_KeyPressEdit(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.KeyPressEditEventArgs) Handles vsfHdr.KeyPressEdit
        If Mode = "V" Then
            e.Handled = True
            Exit Sub
        End If
        If e.KeyChar = "'" Then
            e.Handled = True
            Exit Sub
        End If
        If vsfKeyPressEdit(Me, vsfHdr, vsfHdr.Row, e.KeyChar) Then
            e.Handled = False
        Else
            e.Handled = True
        End If
    End Sub

    Public Sub vsfHdr_LostFocus(sender As Object, e As System.EventArgs) Handles vsfHdr.LostFocus
        pnlToolTip.Visible = False
    End Sub

    Public Sub vsfHdr_RowColChange(ByVal sender As Object, ByVal e As System.EventArgs) Handles vsfHdr.RowColChange
        If Not (Mode = "A" Or Mode = "M") Then Exit Sub
        lblToolTip.Text = ""
        lblToolTip.Text = vsfRowColChange(Me, vsfHdr)
        If lblToolTip.Text <> "" Then
            pnlToolTip.Visible = True
        Else
            pnlToolTip.Visible = False
        End If

    End Sub

    Private Sub vsfHdr_SetupEditor(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfHdr.SetupEditor
        Call vsfSetupEditor(Me, vsfHdr, intDocID, strTblName, Mode, e.Row, e.Col, "M")
    End Sub

    Private Sub vsfHdr_ValidateEdit(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.ValidateEditEventArgs) Handles vsfHdr.ValidateEdit
        If Not vsfValidateEdit(Me, vsfHdr, e.Row, e.Col, "M", strTblName, strIDName, Val(txtItemID.Text)) Then
            e.Cancel = True
        End If
    End Sub

    Public Sub FillDocDetails(ByVal argID As Long, Optional ByVal argCopyItem As Boolean = False)
        Dim RsTemp As OleDb.OleDbDataReader
        Dim strSQL As String
        Dim i As Integer, X As Integer
        Dim rsDummy As OleDb.OleDbDataReader

        If argID = 0 Then Exit Sub

        '--8 begin OthGRID

        If argCopyItem = False Then
            Call ClearAddnlGrid(Me)
        End If

        strSQL = "select ItemGroupCode, i.*"
        strSQL &= " from " & strTblName & " i, MstItemGroup ig"
        strSQL &= " where i." & strIDName & " = " & argID
        strSQL &= " AND i.ItemGroupID = ig.ItemGroupID "


        RsTemp = clsSVT.getDataReaderFromSQL(strSQL)

        With RsTemp
            If .Read() Then
                txtItemID.Text = argID
                ' cboSeries.Text = GetVal(RsTemp(strFldSeries), "")
                txtItemName.Text = GetVal(RsTemp(strFrmPfx & "Name"), "")
                txtHotKey.Text = GetVal(RsTemp("HotKey"), "")
                txtShortName.Text = GetVal(RsTemp("ShortName"), "")
                txtHSNCode.Text = GetVal(RsTemp("HSNCode"), "")
                cboIG.Text = GetVal(RsTemp("ItemGroupCode"), "")
                cboType.Text = GetVal(RsTemp("LiqType"), "")
                txtBotPerCases.Text = GetVal(RsTemp("BotPerCases"), "")
                cboUOM.Text = clsSVT.getDataStringValue("Select UOMCode from MstUOM where UOMId = " & GetVal(RsTemp("UOMID"), 0), "", False)

                If blnItemStockRelated Then
                    'If GetVal(RsTemp("ReOrderItem"), "N") = "Y" Then
                    'chkReOrderItem.Checked = True
                    'txtROL.Text = GetVal(RsTemp("ROL"), "")
                    'txtROQ.Text = GetVal(RsTemp("ROQ"), "")
                    'Else
                    'chkReOrderItem.Checked = False
                    'txtROL.Text = ""
                    'txtROQ.Text = ""
                End If
                'txtCriticalLevel.Text = GetVal(RsTemp("CriticalLevel"), "")
                'txtMaxLevel.Text = GetVal(RsTemp("MaxLevel"), "")
                ' txtBinNo.Text = GetVal(RsTemp("BinNo"), "")
                'If GetVal(RsTemp("LotWiseItem"), "N") = "Y" Then
                '    ChkLotWiseItem.Checked = True
                '    If GetVal(RsTemp("LotType"), "") = "A" Then
                '        opLotWiseAuto.Checked = True
                '    ElseIf GetVal(RsTemp("LotType"), "") = "M" Then
                '        opLotWiseManual.Checked = True
                '    End If
                '    If GetVal(RsTemp("ShelfLifeItem"), "N") = "Y" Then
                '        chkShelfItem.Checked = True
                '    Else
                '        chkShelfItem.Checked = False
                '    End If
                'Else
                '    ChkLotWiseItem.Checked = False
                '    opLotWiseAuto.Checked = False
                '    opLotWiseManual.Checked = False
                '    chkShelfItem.Checked = False
                'End If
                'Else
                '    chkReOrderItem.Checked = False
                '    txtROL.Text = ""
                '    txtROQ.Text = ""
                '    txtCriticalLevel.Text = ""
                '    txtMaxLevel.Text = ""

                'End If

                'If blnItemPurRelated Then
                '    txtMOQ.Text = GetVal(RsTemp("MOQ"), "")
                '    txtSPQ.Text = GetVal(RsTemp("SPQ"), "")
                'Else
                '    txtMOQ.Text = ""
                '    txtSPQ.Text = ""
                'End If

                txtInActiveRem.Text = GetVal(RsTemp("InActiveRmks"), "")
                If GetVal(RsTemp("InActive"), "") = "N" Then
                    chkInactive.Checked = False
                    chkInactive.Text = "InActive"
                Else
                    chkInactive.Checked = True
                    chkInactive.Text = "InActive Remarks"
                End If

                If GetVal(RsTemp("NonStkable"), "") = "N" Then
                    chkNonStkable.Checked = False
                Else
                    chkNonStkable.Checked = True
                End If

                'If GetVal(RsTemp("BOMITEM"), "") = "N" Then
                '    chkBOM.Checked = False
                'Else
                '    chkBOM.Checked = True
                'End If

                If argCopyItem = True Then

                Else
                    txtItemCode.Text = GetVal(RsTemp(strCodeName), "")
                End If

                For i = 0 To vsfHdr.Rows.Count - 1
                    For X = 0 To .FieldCount - 1
                        If UCase(.GetName(X)) = UCase(Trim(vsfHdr.GetData(i, vOthFldCode))) Then
                            If Trim(vsfHdr.GetData(i, vOthFldCmbTblName)) = "" Then
                                vsfHdr.SetData(i, vOthFldDataValue, GetVal(.GetValue(X), ""))
                            Else
                                vsfHdr.SetData(i, vOthFldDataValue,
                                    clsSVT.getDataStringValue("Select " & vsfHdr.GetData(i, vOthFldCmbDisplayCols) & " from " &
                                    vsfHdr.GetData(i, vOthFldCmbTblName) & " where " &
                                    vsfHdr.GetData(i, vOthFldCmbStorageFldName) & " = " & GetVal(.GetValue(X), 0), "", False))
                            End If
                        End If
                    Next X
                Next

                'If blnAuthReq Then
                '    If GetVal(!Authorised, "N") = "Y" Then
                '        lblApproved.Text = "APPROVED"
                '        lblApproved.ForeColor = Color.Green
                '    Else
                '        lblApproved.Text = "UN-APPROVED"
                '        lblApproved.ForeColor = Color.FromArgb(192, 0, 0)
                '    End If
                '    lblApproved.Visible = True
                'Else
                '    lblApproved.Visible = False
                'End If

                If vsfHdr.Rows.Count > 0 Then
                    Call vsfGenAfterEditBfrCalTot(Me, vsfHdr, vsfHdr.Rows.Count - 1, "M")
                End If


                If vsfHdr.Rows.Count > 0 Then
                    Call vsfGenAfterEditBfrCalTot(Me, vsfHdr, vsfHdr.Rows.Count - 1, "M")
                End If
            End If

            '    Dim strITemName As String = ""
            '    Dim folder As String = gstrRptPath & "ItemImage"
            '    strITemName = Regex.Replace(txtItemName.Text, "[^A-Za-z0-9]", "")
            '    Dim filename As String = System.IO.Path.Combine(folder, strITemName & ".jpg")
            '    If filename = Nothing Then Exit Sub
            '    ItemImage.Image = Image.FromFile(filename)
            .Close()
        End With
        RsTemp.Close()
        btnFileAttach.Text = "File Attachment (" & clsSVT.getDataLongValue("Select count(*) from UtlAttachFiles Where DocID = " & intDocID & " and DocValueID = '" & argID & "'", 0, False) & ")"
        If blnShowFileAttach Then Call fillAttachFiles(txtItemID.Text)

        Call LoadItemImage()
        vsfAlias.Rows.Count = 1
        rsDummy = clsSVT.getDataReaderFromSQL("Select ItemAlias from MstItemAlias where ItemId=" & argID & "")
        Do While rsDummy.Read
            vsfAlias.Rows.Count = vsfAlias.Rows.Count + 1
            vsfAlias.SetData(vsfAlias.Rows.Count - 1, 0, rsDummy!ItemAlias)
        Loop
        RemoveEmptVSFRows(vsfAlias, 1, 0)
        rsDummy.Close()

        If argCopyItem = False Then
            Mode = "V"
            enableDisableFlds(False)
            txtSearch.Focus()
        End If
        vsfHdr.Styles("Normal").WordWrap = False
        vsfHdr.AllowResizing = AllowResizingEnum.Both
        vsfHdr.AutoSizeRows()
    End Sub
    Private Sub LoadItemImage()

        Dim strItemName As String = ""
        strItemName = Regex.Replace(txtItemCode.Text, "[^A-Za-z0-9]", "")

        Try
            'With ItemImage
            '    If File.Exists(gstrItemImagePath & GetVal(Trim(strItemName), "") & ".jpg") Then
            '        ItemImage.Visible = True
            '        .ImageLocation = gstrItemImagePath & GetVal(Trim(strItemName), "") & ".jpg"
            '        .SizeMode = PictureBoxSizeMode.StretchImage
            '    Else
            '        ItemImage.Visible = True
            '        Try
            '            .ImageLocation = gstrItemImagePath & "ItemNoImage.jpg"
            '            .SizeMode = PictureBoxSizeMode.StretchImage
            '        Catch
            '        End Try
            '    End If
            'End With
        Catch ex As Exception
            MsgBox("Error on loading image!" & vbCrLf & ex.Message, MsgBoxStyle.Critical, Me.Text)
            Exit Sub
        End Try
    End Sub
    'Private Sub fillAltItemDtls(ByVal argID As Long)
    '    Dim Rs As oledb.oledbDataReader
    '    Dim strsql As String
    '    Dim introw As Integer, intAltItemID As Integer
    '    Dim i As Integer

    '    strsql = "Select * from MstAltITem"
    '    strsql = strsql & " where " & strIDName & " = " & argID
    '    Rs = clsSVT.getDataReaderFromSQL(strsql)

    '    With Rs
    '        vsfItem.Rows.Count = 1

    '        Do While .Read
    '            vsfItem.Rows.Count = vsfItem.Rows.Count + 1
    '            introw = vsfItem.Rows.Count - 1
    '            i = introw
    '            intAltItemID = GetVal(Rs("AltItemID"), 0)
    '            vsfItem.SetData(i, vItemCode, clsSVT.getDataStringValue("Select ItemCode from MstItem Where ItemId=" & intAltItemID, "", False))
    '        Loop
    '        .Close()
    '    End With
    '    If vsfItem.Rows.Count = 1 Then
    '        vsfItem.Rows.Count = 2
    '    End If
    'End Sub

    Private Sub fillSuppRefDtls(ByVal argID As Long)
        Dim Rs As OleDb.OleDbDataReader
        Dim strsql As String
        Dim introw As Integer
        Dim i As Integer

        strsql = "Select * from MstSuppRef"
        strsql = strsql & " where " & strIDName & " = " & argID
        Rs = clsSVT.getDataReaderFromSQL(strsql)

        With Rs
            vsfAlias.Rows.Count = 1

            Do While .Read
                vsfAlias.Rows.Count = vsfAlias.Rows.Count + 1
                introw = vsfAlias.Rows.Count - 1
                i = introw
                vsfAlias.SetData(i, vItemAlias, GetVal(Rs("SuppRef"), ""))
            Loop
            .Close()
        End With
        If vsfAlias.Rows.Count = 1 Then
            vsfAlias.Rows.Count = 2
        End If
    End Sub

    'Private Sub fillAltUOMDtls(ByVal argID As Long)
    '    Dim Rs As oledb.oledbDataReader
    '    Dim strsql As String
    '    Dim introw As Integer, intAltUOMID As Integer
    '    Dim i As Integer

    '    strsql = "Select * from MstItemUOM"
    '    strsql = strsql & " where " & strIDName & " = " & argID
    '    Rs = clsSVT.getDataReaderFromSQL(strsql)

    '    With Rs
    '        vsfAUOM.Rows.Count = 1

    '        Do While .Read
    '            intAltUOMID = GetVal(Rs("UOMID"), 0)
    '            If intAltUOMID <> clsSVT.getDataLongValue("Select UOMID from MstUOM where UOMCode='" & cboUOM.Text & "'", 0, False) Then
    '                vsfAUOM.Rows.Count = vsfAUOM.Rows.Count + 1
    '                introw = vsfAUOM.Rows.Count - 1
    '                i = introw
    '                vsfAUOM.SetData(i, vAUOM, clsSVT.getDataStringValue("Select UOMCode from MstUOM Where UOMID =" & intAltUOMID, "", False))
    '                vsfAUOM.SetData(i, vAUOMFactor, GetVal(Rs("ConvFact"), 1))
    '            End If
    '        Loop
    '        .Close()
    '    End With
    '    If vsfAUOM.Rows.Count = 1 Then
    '        vsfAUOM.Rows.Count = 2
    '    End If
    'End Sub


    Private Sub fillAttachFiles(ByVal argID As Long)
        Dim Rs As OleDb.OleDbDataReader
        Dim strsql As String
        Dim introw As Integer
        Dim i As Integer

        strsql = "Select * from UtlAttachFiles"
        strsql = strsql & " where DocID = " & intDocID & " and DocValueID = " & argID
        Rs = clsSVT.getDataReaderFromSQL(strsql)

        With Rs
            vsfFiles.Rows.Count = 0

            Do While .Read
                vsfFiles.Rows.Count = vsfFiles.Rows.Count + 1
                introw = vsfFiles.Rows.Count - 1
                i = introw
                vsfFiles.SetData(i, vFSlno, GetVal(Rs("DocSlno"), ""))
                vsfFiles.SetData(i, vFName, GetVal(Rs("AFileName"), ""))
                vsfFiles.SetData(i, vFDesc, GetVal(Rs("FileDesc"), ""))
            Loop
            .Close()
        End With
    End Sub

    Private Sub CalTotal()
        Call AddnlMSTFldCalTotal(Me, vsfHdr, 0, "", "", txtItemID.Text)
    End Sub

    Private Sub setSearchGridHt(ByVal argFlag As String)
        vsfSearch.Width = width2
        If argFlag = "FULL" Then
            vsfSearch.Visible = True
            vsfSearch.BringToFront()
            vsfSearch.Width = width2
            pnlSearch.BringToFront()
            'vsfSearch.Cols(0).Visible = False
        Else
            vsfSearch.Visible = False
        End If
    End Sub

    Private Sub txtSearch_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles txtSearch.KeyPress
        If e.KeyChar = "'" Then
            e.Handled = True
        End If
    End Sub

    Private Sub txtSearch_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtSearch.KeyUp
        Dim StrQry As String, strCond As String = ""
        Dim width As Integer = Me.Width
        'MsgBox(width)
        ' Dim frmsize As Integer = width - 20
        Dim width1 As Integer = Panel4.Left
        ' MsgBox(width1)
        'Dim width2 As Integer
        width2 = width - width1 - 30
        vsfSearch.Width = width2

        If e.Alt = Keys.Tab Then
            Exit Sub
        End If

        txtSearch.Text = Replace(txtSearch.Text, "'", "`")

        If e.KeyCode = Keys.Enter Then
            If Trim(txtSearch.Text) <> "" And vsfSearch.Rows.Count > 1 Then
                Call vsfSearchSelectedItem()
                Exit Sub
            End If
        End If

        If e.KeyCode = Keys.Down Then
            vsfSearch.Focus()
            Exit Sub
        End If

        If e.KeyCode = Keys.Escape Then
            txtSearch.Text = ""
            Call setSearchGridHt("")
            Exit Sub
        End If

        If Len(txtSearch.Text) < 1 Then
            Call setSearchGridHt("")
            Exit Sub
        End If

        Call setSearchGridHt("FULL")
        StrQry = "Select Top " & gIntTopNrows & " ItemCode, ItemName, ShortName, HotKey"
        StrQry &= " from MstItem"
        StrQry &= " where (ItemCode like '%" & Trim(txtSearch.Text) & "%'"
        StrQry &= " or ItemName like '%" & Trim(txtSearch.Text) & "%'"
        StrQry &= " or ShortName like '%" & Trim(txtSearch.Text) & "%'"
        StrQry &= " or HotKey like '%" & Trim(txtSearch.Text) & "%')"
        StrQry &= " order by 1"
        Call showWaitCursor()
        dtSearch = clsSVT.getDATATable(StrQry)
        dvSearch = New DataView(dtSearch)
        vsfSearch.DataSource = dvSearch
        Call showNormalCursor()
        vsfSearch.Refresh()
        vsfSearch.ScrollBars = ScrollBars.Both
        vsfSearch.AutoSizeCols()
    End Sub



    Private Sub vsfSearch_BeforeEdit(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfSearch.BeforeEdit
        e.Cancel = True
    End Sub

    Private Sub vsfSearch_ChangeEdit(ByVal sender As Object, ByVal e As System.EventArgs) Handles vsfSearch.ChangeEdit
        If vsfSearch.Rows.Count > 1 Then
            lblTotalRecords.Text = "Total Records: " & vsfSearch.Rows.Count - 1
        Else
            lblTotalRecords.Text = ""
        End If
    End Sub

    Private Sub vsfSearch_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles vsfSearch.DoubleClick
        Call vsfSearchSelectedItem()
    End Sub

    Private Sub vsfSearchSelectedItem()
        Dim intPartyID As Integer
        If vsfSearch.Rows.Count > 1 Then
            intPartyID = clsSVT.getDataLongValue("Select " & strIDName & " from " & strTblName & " where " & strCodeName & " = '" & vsfSearch.GetData(vsfSearch.Row, 0) & "'", 0, False)
            'Call FillDocDetails(intPartyID)
            'txtSearch.Text = ""
            Call setSearchGridHt("")
            txtSearch.Focus()
            txtSearch.Text = ""
            Mode = ""
            enableDisableFlds(False)
            Call FillDocDetails(intPartyID)
        End If
    End Sub

    Private Sub vsfSearch_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles vsfSearch.KeyUp
        If e.KeyCode = Keys.Return Then
            Call vsfSearchSelectedItem()
            Exit Sub
        End If
        If e.KeyCode = Keys.Escape Then
            txtSearch.Text = ""
            Call setSearchGridHt("")
            Exit Sub
        End If
    End Sub

    Private Sub enableDisableFlds(ByVal argFlag As Boolean)
        txtItemName.Enabled = argFlag
        txtShortName.Enabled = argFlag
        cboIG.Enabled = argFlag
        cboUOM.Enabled = argFlag
        chkInactive.Enabled = argFlag
        vsfHdr.AllowEditing = argFlag
        ' chkBOM.Enabled = argFlag
        chkNonStkable.Enabled = argFlag
        'Lock(cboSeries, argFlag)
        If chkInactive.Checked And Mode = "A" Then
            txtInActiveRem.Visible = True
            chkInactive.Visible = False
        Else
            txtInActiveRem.Visible = False
            chkInactive.Visible = False
        End If

        If chkInactive.Checked And Mode = "M" Then
            txtInActiveRem.Visible = True
            txtInActiveRem.Enabled = True
            chkInactive.Visible = True

        Else
            chkInactive.Visible = True
            chkInactive.Text = "InActive"

        End If

        If chkInactive.Checked And Mode = "V" Then
            txtInActiveRem.Enabled = False
            txtInActiveRem.Visible = True
            chkInactive.Visible = True

        End If
        txtHotKey.Enabled = argFlag
        txtHSNCode.Enabled = argFlag
        cboType.Enabled = argFlag
        txtBotPerCases.Enabled = argFlag
        'gbStk.Enabled = argFlag
        ' gbPur.Enabled = argFlag
        vsfAlias.AllowEditing = argFlag
        ' vsfItem.AllowEditing = argFlag
        'vsfAUOM.AllowEditing = argFlag
        If Mode = "A" Then
            PnlAttach.Enabled = False
        Else
            PnlAttach.Enabled = True
        End If
    End Sub

    Private Sub chkInactive_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkInactive.CheckedChanged
        If chkInactive.Checked Then
            chkInactive.Text = "InActive Remarks"
            txtInActiveRem.Visible = True
            txtInActiveRem.Enabled = True
            txtInActiveRem.Focus()
        Else
            txtInActiveRem.Visible = False
            txtInActiveRem.Enabled = False
            txtInActiveRem.Text = ""
            chkInactive.Text = "InActive"
        End If
    End Sub

    Private Sub deleteDocument()
        Dim strItemCode As String = ""
        strItemCode = Trim(txtItemCode.Text)
        If strItemCode <> "" Then
            If Not blnDeleteOK(strTblName, Val(txtItemID.Text), True, True) Then
                Exit Sub
            End If

            If MsgBox("Are you sure you want to delete the " & strFrmPfx & " : '" & Trim(strItemCode) & "'?", MsgBoxStyle.YesNo + vbDefaultButton2 + MsgBoxStyle.Critical, "Deletion...") = vbNo Then Exit Sub
            svtTrn = svtConn.BeginTransaction(IsolationLevel.ReadCommitted)
            Try
                'for audit track
                Call RecordAuditLog("ITEM MASTER", Val(txtItemID.Text), UserActions.DeleteAction, strTblName, strAuditHdrSQL, "", svtTrn)
                clsSVT.execSQLStatement("Delete from " & strTblName & " where " & strIDName & " = " & txtItemID.Text, svtTrn)
                clsSVT.execSQLStatement("Delete from TrnQOH where ItemId=" & txtItemID.Text, svtTrn)
            Catch ex As Exception
                MsgBox("Error while Deleting data!" & vbCrLf & ex.Message, vbCritical, Me.Text)
                svtTrn.Rollback()
                Exit Sub
            End Try


            MsgBox("Deleted Successfully!", vbInformation, Me.Text)
            svtTrn.Commit()
            Call clearFlds()
            vsfSearch.Visible = False
            vsfSearch.SendToBack()
            txtSearch.Focus()
        End If
    End Sub

    Private Sub cboIG_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles cboIG.KeyPress
        If e.KeyChar = "'" Then
            e.Handled = True
        End If
    End Sub

    Private Sub cboIG_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles cboIG.Validating
        Dim blnFound As Boolean = False, strItemGrp As String
        Dim intItemGrpID As Integer

        strItemGrp = cboIG.Text
        If cboIG.Text <> "" Then
            clsSVT.getDataStringValue("select  distinct ItemGroupCode from MstItemGroup where InActive='N' and ItemGroupCode='" & cboIG.Text & "'", "", blnFound)
            If blnFound = False Then
                MsgBox("Selected Item Group not found in master! ", MsgBoxStyle.Critical, Me.Text)
                cboIG.SelectAll()
                e.Cancel = True
                cboIG.Focus()
                Exit Sub
            End If
            intItemGrpID = clsSVT.getDataLongValue("select ItemGroupId from MstItemGroup where ItemGroupCode='" & cboIG.Text & "'", 0, False)
        End If
    End Sub

    Private Sub txtSearch_LostFocus(sender As Object, e As System.EventArgs) Handles txtSearch.LostFocus
        If vsfSearch.Focused Then
            Exit Sub
        Else
            Call setSearchGridHt("")
        End If
    End Sub

    Private Sub vsfSearch_LostFocus(sender As Object, e As System.EventArgs) Handles vsfSearch.LostFocus
        Call setSearchGridHt("")
    End Sub

    Private Sub txtConv_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs)
        Call numericKeyPress(e)
    End Sub

    Private Sub numericKeyPress(ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If Asc(e.KeyChar) <> 8 Then
            If Asc(e.KeyChar) < 48 Or Asc(e.KeyChar) > 57 Then
                e.Handled = Not (Char.IsDigit(e.KeyChar) Or e.KeyChar = ".")
            End If
        End If
    End Sub

    Private Sub svtBtnCtrl_btnClicked(argBtnClicked As Integer) Handles svtBtnCtrl.btnClicked

        Select Case argBtnClicked
            Case svtBtns.svtNew
                If Not chkUserRights(strRights, chkRightsPos.AddRights, Me.Text) Then Exit Sub
                Call AddProc()
                Mode = "A"
                lblDispItemCode.Text = "Add:" & Me.Text
                enableDisableFlds(True)
                pnlSearch.Visible = False
                txtItemCode.Focus()
                lblStkUOM.ForeColor = Color.DarkRed
                cboUOM.Enabled = True
                txtItemCode.ReadOnly = False
                btnCopyFrom.Visible = True
                svtBtnCtrl.PerformAction(argBtnClicked)
                'Call chkSplRights_Std(Me, intDocID, strRights)
                Call setAutoGenCode()
                Me.Parent.Tag = "A"
            Case svtBtns.svtClose
                Call frmMain.CloseSelectedTabPage(Me)

            Case svtBtns.svtEdit
                lblDispItemCode.Text = "Edit:" & Me.Text
                If Not chkUserRights(strRights, chkRightsPos.EditRights, Me.Text) Then Exit Sub
                If Trim(txtItemID.Text) = "" Then
                    Exit Sub
                End If
                Mode = "M"
                enableDisableFlds(True)
                pnlSearch.Visible = False
                'txtItemCode.ReadOnly = True
                txtItemName.Focus()
                If CheckforItemUsedInTrn(Val(txtItemID.Text)) Then
                    blnItemUsedInTrn = True
                    lblStkUOM.ForeColor = Color.Blue
                    cboUOM.Enabled = False
                Else
                    blnItemUsedInTrn = False
                    lblStkUOM.ForeColor = Color.DarkRed
                    cboUOM.Enabled = True
                End If

                svtBtnCtrl.PerformAction(argBtnClicked)
                Call chkSplRights_Std(Me, intDocID, strRights)
                Call chkSplRights_DOC(Me, intDocID, strRights, vsfHdr)
                Call setAutoGenCode()
                Me.Parent.Tag = "M"
            Case svtBtns.svtSave
                vsfHdr.Enabled = False
                stringReplace(Me)
                If ValidFields() Then
                    If saveDetails() Then
                        lblDispItemCode.Text = "View:" & Me.Text
                        svtBtnCtrl.PerformAction(svtBtns.svtCancel)
                        enableDisableFlds(False)
                        pnlSearch.Visible = True
                        btnCopyFrom.Visible = False
                        Mode = "V"
                    End If
                End If
                vsfHdr.Enabled = True
                Me.Parent.Tag = ""
            Case svtBtns.svtPrint
                If Not chkUserRights(strRights, chkRightsPos.AddRights, Me.Text) Then Exit Sub

            Case svtBtns.svtCancel
                lblDispItemCode.Text = "View:" & Me.Text
                Call clearFlds()
                enableDisableFlds(False)
                pnlSearch.Visible = True
                txtSearch.Focus()
                txtItemCode.Text = ""
                svtBtnCtrl.PerformAction(argBtnClicked)
                Me.Parent.Tag = ""
            Case svtBtns.svtMoveFirst, svtBtns.svtMoveLast, svtBtns.svtMoveNext, svtBtns.svtMovePrev
                Call FillDocDetails(gMoveRecords(argBtnClicked, strTblName, strIDName, True, txtItemID.Text, ""))
                svtBtnCtrl.PerformAction(argBtnClicked)
                lblDispItemCode.Text = "View:" & Me.Text

            Case svtBtns.svtDelete
                If Not chkUserRights(strRights, chkRightsPos.DelRights, Me.Text) Then Exit Sub
                Call deleteDocument()
                txtSearch.Focus()
                svtBtnCtrl.PerformAction(svtBtns.svtCancel)

            Case svtBtns.svtFind
                Mode = ""
                pnlSearch.Visible = True
                txtSearch.Focus()
                enableDisableFlds(False)
                svtBtnCtrl.PerformAction(argBtnClicked)
                If Not chkUserRights(strRights, chkRightsPos.ViewRights, Me.Text) Then Exit Sub
                Call showViewForm(Me, strTblName, "Listing of " & strFrmMsg, "", , , strFrmMsg & " Master", strCodeName, intDocID)

        End Select
        If Not chkUserRights(strRights, chkRightsPos.AddRights, Me.Text, False) Then svtBtnCtrl.btnNew.Visible = False
        If Not chkUserRights(strRights, chkRightsPos.EditRights, Me.Text, False) Then svtBtnCtrl.btnEdit.Visible = False
        If Not chkUserRights(strRights, chkRightsPos.DelRights, Me.Text, False) Then svtBtnCtrl.btnDelete.Visible = False
        If Not chkUserRights(strRights, chkRightsPos.opt1Rights, Me.Text, False) Then svtBtnCtrl.btnOptions.Visible = False
        svtBtnCtrl.btnPrint.Visible = False
    End Sub

    Private Function CheckforItemUsedInTrn(ByVal argItemID As Integer) As Boolean
        CheckforItemUsedInTrn = False
        Dim rs As OleDb.OleDbDataReader
        Dim blnFound As Boolean

        Try
            rs = clsSVT.getDataReaderFromSQL("Select 'Select ItemID from ' + Table_Name + ' where ItemID =' as SQLQry from Information_Schema.columns where Column_Name='ItemID' and Table_Name like 'TrnDtl%'")

            With rs
                If Not .HasRows Then
                    CheckforItemUsedInTrn = False
                    rs.Close()
                    Exit Function
                End If
                Do While .Read
                    Call clsSVT.getDataLongValue(rs("SQLQry") & argItemID, 0, blnFound)
                    Debug.Print(rs("sqlqry") & argItemID)
                    If blnFound Then
                        CheckforItemUsedInTrn = True
                        rs.Close()
                        Exit Function
                    End If
                Loop
            End With
        Catch
        End Try
    End Function

    Private Sub btnFind_ClickButtonArea(Sender As System.Object, e As System.Windows.Forms.MouseEventArgs) 'Handles btnFind.ClickButtonArea
        Call showViewForm(Me, strTblName, "Listing of " & strFrmMsg, "", , , strFrmMsg, strCodeName)
        Mode = ""
        pnlSearch.Visible = True
        txtSearch.Focus()
        enableDisableFlds(False)
    End Sub

    Private Sub frmMstItem_MdiChildActivate(sender As Object, e As System.EventArgs) Handles Me.MdiChildActivate
        If Me.MdiChildren.Count > 0 Then
            If Me.MdiChildren.Count > gFormCount Then
                gFormCount = gFormCount + 1
                activeChild = ActiveMdiChild
                frmModule.LoadAllSettings(Application.ProductName, ActiveMdiChild)
            Else
                gFormCount = gFormCount - 1
                frmModule.SaveAllSettings(Application.ProductName, activeChild)
            End If
        End If
    End Sub

    Private Sub btnCopyFrom_Click(sender As System.Object, e As System.EventArgs) Handles btnCopyFrom.Click
        Dim strSQL As String, strFMT As String
        strFMT = ""

        strSQL = "Select " & strIDName & ", " & strCodeName & ", " & strFrmPfx & "Name"
        strSQL = strSQL & " from " & strTblName
        strSQL = strSQL & " where " & strIDName & " > 0"
        strSQL = strSQL & " order by " & strCodeName & ", " & strFrmPfx & "Name"

        Dim frmDisplay As New frmUtlSingleSel

        With frmDisplay
            .Text = "Select the " & strFrmPfx & " to copy"
            Call .FillViewVsf(Me, strSQL, txtCopyItem, 0, 0)
            .ShowDialog()
            .vsfDet.Focus()
        End With

        Call FillDocDetails(Val(txtCopyItem.Text), True)
        PnlCopyFrom.Visible = True
    End Sub

    Private Sub txtItemName_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles txtItemName.Validating
        If Mode <> "A" Then Exit Sub
    End Sub

    Private Sub frmMstItem_Shown(sender As Object, e As System.EventArgs) Handles Me.Shown
        txtSearch.Focus()
    End Sub

    Private Function applyFilterFromGrid(ByVal argType As String, ByVal argVsf As C1FlexGrid) As Boolean
        Dim StrQry As String

        applyFilterFromGrid = False
        StrQry = ""
        With argVsf
            'If .Editor.Text = "" Then
            '    vsfView.Visible = False
            '    Exit Function
            'End If
            StrQry = "Select Top 20 " & argType & "Code, " & argType & "Name"
            StrQry &= " from Mst" & argType
            StrQry &= " where (" & argType & "Code like '%" & Trim(.Editor.Text) & "%'"
            StrQry &= " or " & argType & "Name like '%" & Trim(.Editor.Text) & "%')"
            StrQry &= " order by 1"
            Call showWaitCursor()
            dtSearch = clsSVT.getDATATable(StrQry)
            dvSearch = New DataView(dtSearch)
            'vsfView.DataSource = dvSearch
            Call showNormalCursor()
            'vsfView.Refresh()
            'vsfView.ScrollBars = ScrollBars.Both
            ' vsfView.AutoSizeCols()
        End With
        applyFilterFromGrid = True
    End Function


    'Private Sub loadDtl(ByVal argType As String, ByVal argVsf As C1FlexGrid, ByVal argColFocus As Integer)
    '    Dim intId As Integer
    '    With argVsf
    '        'If vsfView.GetData(vsfView.Row, 1) = "" Then
    '        '    Exit Sub
    '        'End If

    '        intId = clsSVT.getDataLongValue("Select " & argType & "ID from Mst" & argType & " where " & argType & "Code  = '" & vsfView.GetData(vsfView.Row, 0) & "'", 0, False)
    '        If intId = 0 Then
    '            MsgBox(argType & " code  does not exist!", MsgBoxStyle.Information, Me.Text)
    '            vsfView.Visible = True
    '            'vsfView.Cols(0).Visible = False
    '            Exit Sub
    '        End If
    '    End With
    '    argVsf.SetData(argVsf.Row, argColFocus, vsfView.GetData(vsfView.Row, 0))
    '    vsfView.Visible = False
    '    argVsf.Focus()
    '    argVsf.Col = argColFocus
    '    argVsf.StartEditing()
    'End Sub

    'Private Sub vsfItem_ChangeEdit(sender As Object, e As EventArgs)
    '    If vsfItem.Col = vItemCode Then
    '        Select Case vsfItem.Editor.Text
    '            Case ""
    '                vsfView.Visible = False
    '            Case Else
    '                ' vsfView.Top = vsfItem.Top
    '                'vsfView.Top = pnlItem.Top + vsfItem.Rows(vsfItem.Row).Top + (vsfItem.Rows(vsfItem.Row).Height) + 25
    '                vsfView.Location = Cursor.Position
    '                vsfView.Left = pnlItem.Left + vsfItem.Cols(vsfItem.Col).Left
    '                vsfView.BringToFront()
    '                vsfView.Visible = True
    '                'vsfView.Cols(0).Visible = False
    '                Call applyFilterFromGrid("Item", vsfItem)
    '        End Select
    '    End If
    'End Sub

    'Private Sub vsfItem_KeyUp(sender As Object, e As KeyEventArgs)
    '    With vsfItem
    '        If Trim(.GetData(.Row, vItemCode)) <> "" And e.KeyCode = Keys.Tab And .Row = .Rows.Count - 1 Then
    '            .LeftCol = vItemCode
    '            .Col = vItemCode
    '            .Col = vItemCode
    '            .Rows.Count = .Rows.Count + 1
    '            .Row = .Rows.Count - 1
    '            .TopRow = .Rows.Count - 1

    '        End If
    '        If .Col = vItemCode Then
    '            If e.KeyCode = Keys.Escape Then
    '                vsfView.Visible = False
    '            End If
    '            If e.KeyCode = Keys.Down AndAlso vsfView.Visible AndAlso vsfView.Rows.Count > 1 Then
    '                vsfView.Focus()
    '                vsfView.Row = 1
    '                vsfView.Col = 1
    '            End If
    '            Exit Sub
    '        End If
    '    End With
    'End Sub

    Private Sub vsfSupp_KeyUp(sender As Object, e As KeyEventArgs) Handles vsfAlias.KeyUp
        If Not (Mode = "A" Or Mode = "M") Then Exit Sub
        With vsfAlias
            If Trim(.GetData(.Row, vItemAlias)) <> "" And e.KeyCode = Keys.Tab And .Row = .Rows.Count - 1 Then
                .Col = vItemAlias
                .Rows.Count = .Rows.Count + 1
                .Row = .Rows.Count - 1
                .TopRow = .Rows.Count - 1
            End If
            If .Col = vItemAlias Then
                If e.KeyCode = Keys.Escape Then
                    ' vsfView.Visible = False
                End If
                'If e.KeyCode = Keys.Down AndAlso vsfView.Visible AndAlso vsfView.Rows.Count > 1 Then
                '    vsfView.Focus()
                '    vsfView.Row = 1
                '    vsfView.Col = 1
                'End If
                Exit Sub
            End If
        End With
    End Sub

    Private Sub btnRemoveItem_Click(sender As Object, e As EventArgs)
        If Not (Mode = "A" Or Mode = "M") Then Exit Sub
        'If MsgBox("Do You want to Remove the Item Code --> " & vsfItem.GetData(vsfItem.Row, vItemCode) & " ?", vbInformation + vbYesNo, Me.Text) = vbYes Then
        '    vsfItem.RemoveItem(vsfItem.Row)
        'End If
        'If vsfItem.Rows.Count = 1 Then vsfItem.Rows.Count = 2
    End Sub

    Private Sub btnRemoveSupp_Click(sender As Object, e As EventArgs) Handles btnRemoveSupp.Click
        If Not (Mode = "A" Or Mode = "M") Then Exit Sub
        If MsgBox("Do You want to Remove Alias Name --> " & vsfAlias.GetData(vsfAlias.Row, 0) & " ?", vbInformation + vbYesNo, Me.Text) = vbYes Then
            vsfAlias.RemoveItem(vsfAlias.Row)
        End If
        If vsfAlias.Rows.Count = 1 Then vsfAlias.Rows.Count = 2
    End Sub

    Private Sub vsfSupp_SetupEditor(sender As Object, e As RowColEventArgs) Handles vsfAlias.SetupEditor
        If Not (Mode = "A" Or Mode = "M") Then Exit Sub
        With vsfAlias
            Select Case e.Col
                Case vItemAlias
                    If TypeOf (.Editor) Is TextBox Then
                        Dim tb As TextBox = .Editor
                        tb.MaxLength = 30
                    End If
            End Select
        End With
    End Sub

    Private Sub vsfSupp_KeyPressEdit(sender As Object, e As KeyPressEditEventArgs) Handles vsfAlias.KeyPressEdit
        If Not (Mode = "A" Or Mode = "M") Then Exit Sub
        If e.KeyChar = "'" Then
            e.Handled = True
        End If
    End Sub

    Private Sub vsfItem_KeyPressEdit(sender As Object, e As KeyPressEditEventArgs)
        If e.KeyChar = "'" Then
            e.Handled = True
        End If
    End Sub


    Private Sub txtShortName_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtShortName.KeyPress
        If e.KeyChar = "'" Then
            e.Handled = True
        End If
    End Sub

    Private Sub vsfView_BeforeEdit(sender As Object, e As RowColEventArgs)
        e.Cancel = True
    End Sub

    Private Sub lblSearchImage_Click(sender As Object, e As EventArgs) Handles lblSearchImage.Click
        txtSearch.Text = ""
        Dim StrQry As String, strCond As String = ""
        Dim width As Integer = Me.Width
        ' MsgBox(width)
        ' Dim frmsize As Integer = width - 20
        Dim width1 As Integer = Panel4.Left
        'MsgBox(width1)
        'Dim width2 As Integer
        width2 = width - width1 - 30
        vsfSearch.Width = width2

        If vsfSearch.Visible = True Then
            vsfSearch.Visible = False
        Else
            Call setSearchGridHt("FULL")

            StrQry = "Select Top 20 ItemCode, ItemName, ShortName, HotKey"
            StrQry &= " from MstItem"
            StrQry &= " order by 1"
            Call showWaitCursor()
            dtSearch = clsSVT.getDATATable(StrQry)
            dvSearch = New DataView(dtSearch)
            vsfSearch.DataSource = dvSearch
            Call showNormalCursor()
            vsfSearch.Refresh()
            vsfSearch.ScrollBars = ScrollBars.Both
            vsfSearch.AutoSizeCols()
        End If
    End Sub

    Private Sub txtItemCode_LostFocus(sender As Object, e As EventArgs) Handles txtItemCode.LostFocus
        If Mode = "A" Then txtItemName.Text = txtItemCode.Text
    End Sub

    Private Sub setAutoGenCode()
        If blnAutoGenCode Then
            ' pnlSeries.Visible = True
            ' cboSeries.Visible = True
            'lblSeries.Visible = True
            lblItemCode.ForeColor = gChildReadOnlyFore
            txtItemCode.ReadOnly = True
            If Mode = "A" Or Mode = "M" Then
                'cboSeries.Focus()
            End If
        Else
            ' pnlSeries.Visible = False
            ' cboSeries.Visible = False
            '  lblSeries.Visible = False
            lblItemCode.ForeColor = gChildMandatoryFore
            txtItemCode.ReadOnly = False
            If Mode = "A" Or Mode = "M" Then
                txtItemCode.Focus()
            End If
        End If
    End Sub

    'Private Sub cboSeries_Validating(sender As Object, e As CancelEventArgs)
    '    If Mode = "A" Then
    '        'If Trim(cboSeries.Text) <> "" Then
    '        '    Call FillAutoGenItemNo()
    '        '    txtItemName.Focus()
    '        'End If
    '    End If
    'End Sub

    Public Sub FillAutoGenItemNo()
        intNo = 0
        'If Trim(cboSeries.Text) <> "" Then
        '    intNo = clsSVT.getDataLongValue("Select Max(" & getSyntax("ISNULL0", strFldNo) & ")+1 from " & strTblName & " where " & strFldSeries & " ='" & Trim(cboSeries.Text) & "' and " & strIDName & "<>" & Val(txtItemID.Text), 0, False)
        '    If intNo = 0 Then intNo = 1
        'End If
        'txtNo.Text = intNo
        'txtItemCode.Text = Trim(cboSeries.Text) & Microsoft.VisualBasic.Right("000000" & intNo, 6)
    End Sub

    Private Sub cboSeries_KeyPress(sender As Object, e As KeyPressEventArgs)
        If Mode = "A" Or Mode = "M" Then
            If e.KeyChar = "'" Then e.Handled = True

            e.KeyChar = UCase(e.KeyChar)
            If Not KeyValidate("String", e.KeyChar) Then
                e.Handled = True
            End If
        Else
            e.Handled = True
        End If
    End Sub



    Private Sub svtBtnCtrl_btnOptionSelected(argOptionName As String, argBlnCtrlClick As Boolean, argBlnAltClick As Boolean) Handles svtBtnCtrl.btnOptionSelected
        Select Case argOptionName.ToUpper
            Case "APPROVE"
                If Trim(txtItemCode.Text) = "" Then Exit Sub
                '' If UCase(lblApproved.Text) <> "APPROVED" Then
                'If MsgBox("Do you want to Approve this " & Replace(strFormCaption, "Master", "") & " : '" & txtItemCode.Text & "' ?", vbQuestion + vbYesNo + vbDefaultButton1, Me.Text) = vbYes Then
                '        clsSVT.execSQLStatement("Update " & strTblName & " set Authorised='Y', AuthBy='" & gUserCode & "', AuthOn=" & getDateFormatInCond(getSystemDate) & " where " & strIDName & "=" & Val(txtItemID.Text))
                ''        'for audit track
                'Call RecordAuditLog("ITEM MASTER", Val(txtItemID.Text), UserActions.EditAction, strTblName, strAuditHdrSQL, strAuditDtlSQL)
                '    ' lblApproved.Text = "APPROVED"
                '    lblApproved.ForeColor = Color.Green
                '    'Exit Sub
                'End If
                'ElseIf UCase(lblApproved.Text) <> "UN-APPROVED" Then
                '    If MsgBox(Replace(strFormCaption, "Master", "") & " : '" & txtItemCode.Text & "' is already approved." & vbCrLf & "Do you want to revoke it?", vbQuestion + vbYesNo + vbDefaultButton1, Me.Text) = vbYes Then
                '        clsSVT.execSQLStatement("Update " & strTblName & " set Authorised='N', AuthBy='', AuthOn=NULL where " & strIDName & "=" & Val(txtItemID.Text))
                ''        'for audit track
                'Call RecordAuditLog("ITEM MASTER", Val(txtItemID.Text), UserActions.EditAction, strTblName, strAuditHdrSQL, strAuditDtlSQL)
                '        lblApproved.Text = "UN-APPROVED"
                '        lblApproved.ForeColor = Color.FromArgb(192, 0, 0)
                '        Exit Sub
                '    End If
                'End If

            'Case "PRINT BARCODE"
            '    'Call testreport
            '    'Exit Sub
            '    Dim objFrm As New frmUtlBCLblGen
            '    objFrm.StartPosition = FormStartPosition.CenterScreen
            '    objFrm.ShowDialog()
            Case "Items-DataLink Problem".ToUpper
                Dim objFrm As New frmRPtInvalidItems
                objFrm.StartPosition = FormStartPosition.CenterScreen
                objFrm.ShowDialog()
            Case "Import Item from Excel".ToUpper
                Dim objfrm As New frmImportItemXLData
                frmMain.OpenForms(objfrm, "Import Item Data From Excel")
            Case "Update Stock from Excel".ToUpper
                Dim objfrm As New frmUpdateItemStock
                frmMain.OpenForms(objfrm, "Update Item Stock From Excel")
            Case "Item Vs Brand".ToUpper
                Dim objfrm As New frmUtlItemVsBrand
                frmMain.OpenForms(objfrm, "Item Vs Brand")

            Case "Update Item Master Fields".ToUpper
                Dim objfrm As New frmUtlImportAnyFld
                frmMain.OpenForms(objfrm, "Import Item Master Fields")

            Case "PRINT BARCODE"
                Dim obj As New frmUtlBarCodePrint
                frmMain.OpenForms(obj, "Barcode Printing")

            Case "UPDATE ITEM RATES"
                Call updateItemRates()
        End Select
    End Sub

    Private Sub txtItemCode_Validating(sender As Object, e As CancelEventArgs) Handles txtItemCode.Validating
        If Trim(txtItemCode.Text) = "" Then Exit Sub

        If Not blnAutoGenCode Then
            Dim strSQL As String
            StMsg("Validating " & strCodeName & "...")
            Try
                If clsSVT.getDataLongValue("select count(" & strCodeName & ") " & " from " & strTblName & " where " & strCodeName & "Clean = dbo.RemoveNonAlphaCharacters('" & Trim(txtItemCode.Text) & "') and " & strIDName & "<>" & Val(txtItemID.Text), 0, False) > 0 Then
                    If MsgBox("Similar " & strCodeName & " already exists." & vbCrLf & "Do you want to view them?", vbInformation + vbYesNo + vbDefaultButton1, Me.Text) = vbYes Then
                        Dim frmDisplay As New frmUtlMstView
                        ' strSQL = clsSVT.getDataReaderFromSQL("Select " & strCodeName & "," & strFrmPfx & "Name" & " from " & strTblName & " where " & strCodeName & "Clean = dbo.RemoveNonAlphaCharacters('" & Trim(txtItemCode.Text) & "')")
                        strSQL = "Select " & strCodeName & "," & strFrmPfx & "Name" &
                                           " from " & strTblName & " where " & strCodeName & "Clean = dbo.RemoveNonAlphaCharacters('" & Trim(txtItemCode.Text) & "') and " & strIDName & " <> " & Val(txtItemID.Text)
                        With frmDisplay

                            .Text = Me.Text
                            Call .FillViewVsf(Me, strSQL, "", strFrmPfx & "CODE " & "'" & txtItemCode.Text & "' is similar to below " & Replace(strFormCaption, "Master", "") & " :", intDocID, Me.Text, False)
                            .ShowDialog()
                        End With
                        Exit Sub
                    End If
                End If
                StMsg("")
            Catch ex As Exception
            End Try
        End If
    End Sub

    Private Sub txtItemName_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtItemName.KeyPress
        If e.KeyChar = "'" Then e.Handled = True
    End Sub

    Private Sub txtHotKey_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtHotKey.KeyPress
        If e.KeyChar = "'" Then e.Handled = True
    End Sub

    Private Sub txtHSNCode_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtHSNCode.KeyPress
        If e.KeyChar = "'" Then e.Handled = True
        If Not KeyValidate("Integer", e.KeyChar) Then
            e.Handled = True
        End If
    End Sub

    Private Sub cboType_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cboType.KeyPress
        If e.KeyChar = "'" Then e.Handled = True
    End Sub

    Private Sub txtInActiveRem_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtInActiveRem.KeyPress
        If e.KeyChar = "'" Then e.Handled = True
    End Sub

    Private Sub cboUOM_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cboUOM.KeyPress
        If e.KeyChar = "'" Then e.Handled = True
    End Sub

    Private Sub txtROL_KeyPress(sender As Object, e As KeyPressEventArgs)
        If Not KeyValidate("Decimal", e.KeyChar) Then
            e.Handled = True
        End If
        If InStr(txtROL.Text, ".") > 0 And e.KeyChar = "." Then
            e.Handled = True
        End If
    End Sub

    Private Sub txtROQ_KeyPress(sender As Object, e As KeyPressEventArgs)
        If Not KeyValidate("Decimal", e.KeyChar) Then
            e.Handled = True
        End If
        If InStr(txtROQ.Text, ".") > 0 And e.KeyChar = "." Then
            e.Handled = True
        End If
    End Sub

    'Private Sub ChkLotWiseItem_CheckedChanged(sender As Object, e As EventArgs)
    '    If ChkLotWiseItem.Checked Then
    '        pnlLotWise.Visible = True
    '        opLotWiseAuto.Checked = True
    '    Else
    '        pnlLotWise.Visible = False
    '    End If
    'End Sub

    'Private Sub txtCriticalLevel_KeyPress(sender As Object, e As KeyPressEventArgs)
    '    If Not KeyValidate("Decimal", e.KeyChar) Then
    '        e.Handled = True
    '    End If
    '    If InStr(txtCriticalLevel.Text, ".") > 0 And e.KeyChar = "." Then
    '        e.Handled = True
    '    End If
    'End Sub

    'Private Sub txtMaxLevel_KeyPress(sender As Object, e As KeyPressEventArgs)
    '    If Not KeyValidate("Decimal", e.KeyChar) Then
    '        e.Handled = True
    '    End If
    '    If InStr(txtMaxLevel.Text, ".") > 0 And e.KeyChar = "." Then
    '        e.Handled = True
    '    End If
    'End Sub

    Private Sub txtBinNo_KeyPress(sender As Object, e As KeyPressEventArgs)
        If e.KeyChar = "'" Then e.Handled = True
    End Sub

    'Private Sub txtMOQ_KeyPress(sender As Object, e As KeyPressEventArgs)
    '    If Not KeyValidate("Decimal", e.KeyChar) Then
    '        e.Handled = True
    '    End If
    '    '  If InStr(txtMOQ.Text, ".") > 0 And e.KeyChar = "." Then
    '    e.Handled = True
    '    End If
    'End Sub

    'Private Sub txtSPQ_KeyPress(sender As Object, e As KeyPressEventArgs)
    '    If Not KeyValidate("Decimal", e.KeyChar) Then
    '        e.Handled = True
    '    End If
    '    If InStr(txtSPQ.Text, ".") > 0 And e.KeyChar = "." Then
    '        e.Handled = True
    '    End If
    'End Sub

    Private Sub cboType_Validating(sender As Object, e As CancelEventArgs) Handles cboType.Validating
        If Trim(cboType.Text) = String.Empty Then Exit Sub
        If cboType.Items.Contains(cboType.Text) = False Then
            MsgBox("Not a valid ItemType! ", MsgBoxStyle.Critical, Me.Text)
            cboType.SelectAll()
            cboType.Focus()
            Exit Sub
        End If
    End Sub

    Private Sub cboUOM_Validating(sender As Object, e As CancelEventArgs) Handles cboUOM.Validating
        Dim blnFound As Boolean = False, strUOM As String
        Dim intUOMID As Integer

        strUOM = cboUOM.Text
        If cboUOM.Text <> "" Then
            clsSVT.getDataStringValue("select  distinct UOMCode from MstUOM where InActive='N' and UOMCode='" & cboUOM.Text & "'", "", blnFound)
            If blnFound = False Then
                MsgBox("Selected UOM not found in master! ", MsgBoxStyle.Critical, Me.Text)
                cboUOM.SelectAll()
                e.Cancel = True
                cboUOM.Focus()
                Exit Sub
            End If
            intUOMID = clsSVT.getDataLongValue("select UOMId from MstUOM where UOMCode='" & cboUOM.Text & "'", 0, False)
        End If
    End Sub

    Private Sub txtItemCode_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtItemCode.KeyPress
        If e.KeyChar = "'" Then e.Handled = True
    End Sub

    Private Sub btnMstGrp_Click(sender As Object, e As EventArgs) Handles btnMstItemGrp.Click
        Call getMasterFlds("Mst" & GetVal(strFrmPfx & "Group"), cboIG.Text)
    End Sub



    Private Sub txtROL_Validating(sender As Object, e As CancelEventArgs)
        Call allowNumDecimals(txtROL, 3)
    End Sub

    Private Sub txtROQ_Validating(sender As Object, e As CancelEventArgs)
        Call allowNumDecimals(txtROQ, 3)
    End Sub

    Private Sub btnMstUOM_Click(sender As Object, e As EventArgs) Handles btnMstUOM.Click
        Call getMasterFlds("MstUOM", cboUOM.Text)
    End Sub

    'Private Sub txtMaxLevel_Validating(sender As Object, e As CancelEventArgs)
    '    Call allowNumDecimals(txtMaxLevel, 3)
    'End Sub

    'Private Sub txtCriticalLevel_Validating(sender As Object, e As CancelEventArgs)
    '    Call allowNumDecimals(txtCriticalLevel, 3)
    'End Sub

    'Private Sub txtSPQ_Validating(sender As Object, e As CancelEventArgs)
    '    Call allowNumDecimals(txtMOQ, 3)
    'End Sub

    'Private Sub txtMOQ_Validating(sender As Object, e As CancelEventArgs)
    '    Call allowNumDecimals(txtMOQ, 3)
    'End Sub

    Private Sub btnFileAttach_ClickButtonArea(Sender As Object, e As MouseEventArgs) Handles btnFileAttach.ClickButtonArea
        If Mode = "A" Then Exit Sub
        If Trim(txtItemCode.Text) = "" Then Exit Sub
        If Not chkUserRights(strRights, chkRightsPos.ViewRights, Me.Text) Then Exit Sub
        With frmUtlFileControl
            .strRights = strRights
            .argOwnerCaption = Me.Text
            .argDocID = intDocID
            .argDocValueID = Val(txtItemID.Text)
            .StartPosition = FormStartPosition.CenterScreen
            .ShowDialog()
            btnFileAttach.Text = "File Attachment (" & clsSVT.getDataLongValue("Select count(*) from UtlAttachFiles Where DocID = " & intDocID & " and DocValueID = '" & Val(txtItemID.Text) & "'", 0, False) & ")"
            fillAttachFiles(Val(txtItemID.Text))
        End With
    End Sub

    'Private Sub chkReOrderItem_CheckedChanged(sender As Object, e As EventArgs)
    '    'If chkReOrderItem.Checked Then
    '    pnlReOrder.Visible = True
    '    Else
    '        pnlReOrder.Visible = False
    '    End If
    'End Sub

    Private Sub txtItemName_LostFocus(sender As Object, e As EventArgs) Handles txtItemName.LostFocus
        If Mode = "A" Then txtHotKey.Text = txtItemName.Text
    End Sub

    Private Sub vsfFiles_BeforeEdit(sender As Object, e As RowColEventArgs) Handles vsfFiles.BeforeEdit
        e.Cancel = True
    End Sub

    Private Sub vsfFiles_MouseHoverCell(sender As Object, e As EventArgs) Handles vsfFiles.MouseEnterCell
        If vsfFiles.Rows.Count > 1 Then
            With vsfFiles
                tt.SetToolTip(vsfFiles, .GetData(.Row, vFName) & "/" & .GetData(.Row, vFDesc))
            End With
        End If
    End Sub

    Private Sub vsfFiles_DoubleClick(sender As Object, e As EventArgs) Handles vsfFiles.DoubleClick
        Dim strFile As String
        If vsfFiles.Row < 0 Then Exit Sub
        strFile = vsfFiles.GetData(vsfFiles.Row, vFName)
        '  MsgBox(strFile.Substring(2, 1))
        If strFile.Substring(1, 1) = ":" Then
            MessageBox.Show("Please save and then Open the file...!", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If
        strFile = gstrRptPath & "TrnDocFiles\" & strFile
        Try
            Dim p As New System.Diagnostics.Process
            Dim s As New System.Diagnostics.ProcessStartInfo(strFile)
            s.UseShellExecute = True
            s.WindowStyle = ProcessWindowStyle.Normal
            p.StartInfo = s
            p.Start()
        Catch ex As Exception
            MessageBox.Show("File '" & strFile & "' could not be found!", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub ItemImage_DoubleClick(sender As Object, e As EventArgs)
        If Mode = "A" Then Exit Sub
        Dim strFile As String = ""
        strFile = Regex.Replace(txtItemCode.Text, "[^A-Za-z0-9]", "")
        strFile = gstrItemImagePath & strFile & ".jpg"
        Try
            Dim p As New System.Diagnostics.Process
            Dim s As New System.Diagnostics.ProcessStartInfo(strFile)
            s.UseShellExecute = True
            s.WindowStyle = ProcessWindowStyle.Normal
            p.StartInfo = s
            p.Start()
        Catch ex As Exception
            MessageBox.Show("File '" & strFile & "' could not be found!", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub vsfHdr_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles vsfHdr.MouseDoubleClick
        Dim ht = vsfHdr.HitTest
        With vsfHdr
            Try
                If ht.Column <> vOthFldDataValue Then
                    If InStr(UCase(GetVal(.GetData(.Row, vOthFldCaption))), "[MST]") > 0 Then
                        Call getMasterFlds(GetVal(.GetData(.Row, vOthFldCmbTblName)), GetVal(.GetData(.Row, vOthFldDataValue)))
                    End If
                End If
            Catch ex As Exception
            End Try
        End With
    End Sub

    'Private Sub vsfAUOM_KeyPressEdit(sender As Object, e As KeyPressEditEventArgs)
    '    If Not (Mode = "A" Or Mode = "M") Then Exit Sub
    '    If e.KeyChar = "'" Then
    '        e.Handled = True
    '    End If
    '    With vsfAUOM
    '        If .Col = vAUOMFactor Then
    '            If Not KeyValidate("Decimal", e.KeyChar) Then
    '                e.Handled = True
    '                Exit Sub
    '            End If

    '            If InStr(Trim(.Editor.Text).ToString, ".") > 0 And e.KeyChar = "." Then
    '                e.Handled = True
    '            End If
    '            If InStr(Trim(.Editor.Text).ToString, ".") > 0 Then
    '                If Len(Mid(Trim(vsfAUOM.Editor.Text).ToString, InStr(Trim(vsfAUOM.Editor.Text).ToString, ".") + 1, Len(Trim(vsfAUOM.Editor.Text).ToString) - InStr(Trim(vsfAUOM.Editor.Text).ToString, "."))) > 6 Then
    '                    e.Handled = True
    '                End If
    '            End If
    '        End If
    '    End With
    'End Sub

    Public Function allowNumDecimals(argtxtbox As TextBox, argNumofDecimals As Integer) As Boolean
        allowNumDecimals = False
        If InStr(argtxtbox.Text, ".") > 0 Then
            Dim TwoDec As New System.Text.RegularExpressions.Regex("^(\d)*\.\d{" & argNumofDecimals & "}$")
            If TwoDec.IsMatch(argtxtbox.Text) Then
            Else
                argtxtbox.Text = Mid(argtxtbox.Text, 1, InStr(argtxtbox.Text, ".")) & Mid(argtxtbox.Text, InStr(argtxtbox.Text, ".") + 1, argNumofDecimals)
            End If
        End If
    End Function

    Private Sub btnRemoveRowAUOM_Click(sender As Object, e As EventArgs)
        If Not (Mode = "A" Or Mode = "M") Then Exit Sub
        'If MsgBox("Do You want to Remove the UOM Code --> " & vsfAUOM.GetData(vsfAUOM.Row, vAUOM) & " ?", vbInformation + vbYesNo, Me.Text) = vbYes Then
        '    vsfAUOM.RemoveItem(vsfAUOM.Row)
        'End If
        'If vsfAUOM.Rows.Count = 1 Then vsfAUOM.Rows.Count = 2
    End Sub

    Private Sub pnlGroups_Paint(sender As Object, e As PaintEventArgs)

    End Sub

    Private Sub vsfAUOM_KeyUp(sender As Object, e As KeyEventArgs)
        If Not (Mode = "A" Or Mode = "M") Then Exit Sub
        'With vsfAUOM
        '    If Trim(.GetData(.Row, vAUOM)) <> "" And Trim(.GetData(.Row, vAUOMFactor)) <> "" And e.KeyCode = Keys.Tab And .Row = .Rows.Count - 1 Then
        '        .Col = vAUOM
        '        .Rows.Count = .Rows.Count + 1
        '        .Row = .Rows.Count - 1
        '        .TopRow = .Rows.Count - 1
        '    End If
        '    If .Col = vAUOM Then
        '        If e.KeyCode = Keys.Enter Then MsgBox("")
        '        If e.KeyCode = Keys.Escape Then
        '            vsfView.Visible = False
        '        End If
        '        If e.KeyCode = Keys.Down AndAlso vsfView.Visible AndAlso vsfView.Rows.Count > 1 Then
        '            vsfView.Focus()
        '            vsfView.Row = 1
        '            vsfView.Col = 1
        '        End If
        '        Exit Sub
        '    End If
        'End With
    End Sub

    'Private Sub vsfAUOM_ValidateEdit(sender As Object, e As EventArgs) Handles vsfAUOM.ValidateEdit
    '    If Not (Mode = "A" Or Mode = "M") Then Exit Sub
    '    With vsfAUOM
    '        If .Col = vAUOMFactor Then
    '            If Val(.Editor.Text) <= 0 Then
    '                .SetData(.Row, vAUOMFactor, 1)
    '            End If
    '        End If
    '    End With
    'End Sub

    'Private Sub vsfSupp_ChangeEdit(sender As Object, e As EventArgs) Handles vsfAlias.ChangeEdit
    '    If vsfAlias.Col = vItemAlias Then
    '        Select Case vsfAlias.Editor.Text
    '            Case ""
    '                vsfView.Visible = False
    '            Case Else
    '                ' vsfView.Top = vsfItem.Top
    '                'vsfView.Top = pnlItem.Top + vsfItem.Rows(vsfItem.Row).Top + (vsfItem.Rows(vsfItem.Row).Height) + 25
    '                vsfView.Location = Cursor.Position
    '                vsfView.Left = pnlSupp.Left + vsfAlias.Cols(vsfAlias.Col).Left
    '                vsfView.BringToFront()
    '                vsfView.Visible = True
    '                'vsfView.Cols(0).Visible = False
    '                Call applyFilterFromGrid("Supp", vsfAlias)
    '        End Select
    '    End If
    'End Sub

    'Private Sub vsfAUOM_ChangeEdit(sender As Object, e As EventArgs)
    '    If vsfAUOM.Col = vAUOM Then
    '        Select Case vsfAUOM.Editor.Text
    '            Case ""
    '                vsfView.Visible = False
    '            Case Else
    '                ' vsfView.Top = vsfItem.Top
    '                'vsfView.Top = pnlItem.Top + vsfItem.Rows(vsfItem.Row).Top + (vsfItem.Rows(vsfItem.Row).Height) + 25
    '                vsfView.Location = Cursor.Position
    '                vsfView.Left = gbAUOM.Left + vsfAUOM.Cols(vsfAUOM.Col).Left
    '                vsfView.BringToFront()
    '                vsfView.Visible = True
    '                'vsfView.Cols(0).Visible = False
    '                Call applyFilterFromGrid("UOM", vsfAUOM)
    '        End Select
    '    End If
    'End Sub

    Public Sub vsfHdr_MouseClick(sender As Object, e As MouseEventArgs) Handles vsfHdr.MouseClick
        If e.Button = MouseButtons.Right Then
            If vsfHdr.Rows.Fixed = 1 Then
                vsfHdr.Rows.Fixed = 0
                Call SaveActFormVsfDet(Me, Me.Text, "Hdr", vsfHdr)
            Else
                vsfHdr.Rows.Fixed = 1
            End If
        End If

    End Sub

    Public Sub vsfHdr_AfterResizeColumn(sender As Object, e As RowColEventArgs) Handles vsfHdr.AfterResizeColumn
        If vsfHdr.Cols(vOthFldCaption).Width >= vsfHdr.Width Then
            vsfHdr.Cols(vOthFldCaption).Width = vsfHdr.Width - 20
        End If
    End Sub

    Private Sub frmMstItem_Resize(sender As Object, e As EventArgs) Handles Me.Resize
        If Me.Width = 0 Then Exit Sub
        If vsfHdr.Cols(vOthFldCaption).Width >= vsfHdr.Width Then
            vsfHdr.Cols(vOthFldCaption).Width = vsfHdr.Width - 20
        End If
    End Sub

    Private Sub svtBtnCtrl_Load(sender As Object, e As EventArgs) Handles svtBtnCtrl.Load

    End Sub

    Public Sub vsfHdr_KeyUp(sender As Object, e As KeyEventArgs) Handles vsfHdr.KeyUp
        With vsfHdr
            If e.KeyCode = Keys.Tab Then
                If vsfHdr.Row = vsfHdr.Rows.Count - 1 Then
                    svtBtnCtrl.btnSave.Focus()
                End If
            End If
        End With
    End Sub

    Private Sub chkNonStkable_LostFocus(sender As Object, e As EventArgs) Handles chkNonStkable.LostFocus
        With vsfHdr
            .Row = 0
            .Col = 1
            Call showWaitCursor()
        End With
    End Sub
    Private Sub frmMstItem_SizeChanged(sender As Object, e As EventArgs) Handles Me.SizeChanged
    End Sub

    Private Sub txtBotPerCases_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtBotPerCases.KeyPress
        If Char.IsLetter(e.KeyChar) AndAlso Not e.KeyChar = Chr(Keys.Back) AndAlso Not e.KeyChar = Chr(Keys.Delete) Then
            e.Handled = True
        End If
    End Sub
    Private Sub testreport()
        Dim strFormulaFld As String = "", strFormulaVal As String = ""
        Dim frmv As New frmUtlReportViewer
        With frmv
            .StartPosition = FormStartPosition.CenterScreen
            strFormulaFld = "mItemID"
            strFormulaVal = "54"
            .ShowRptFile("", "TestReport.Rpt", strFormulaFld, strFormulaVal, "")
            .Text = "TestReport"
            .Show()
        End With

    End Sub

    Private Sub updateItemRates()
        If MsgBox("Are you sure you want to Update/Recalulate Rates?", MsgBoxStyle.Question + vbYesNo, Me.Text) = vbNo Then Exit Sub

        Dim strSql As String, intItemMstDocID As Long
        Dim strFldCode As String, strFormula As String
        Dim rsTemp As OleDb.OleDbDataReader

        'if these fields are null then set default value to ensure proper calculation of rates
        If fnFldPresent("UITCCGST", "MstItem") Then
            strSql = "select top 1 ItemID from MstItem where UITCCGST is null"
            If clsSVT.getDataLongValue(strSql, 0, False) > 0 Then
                clsSVT.execSQLStatement("Update MSTITEM SET UITCCGST= 0 WHERE UITCCGST Is NULL")
                clsSVT.execSQLStatement("ALTER TABLE MSTITEM ALTER COLUMN UITCCGST NUMERIC(18, 2) Not NULL")
                clsSVT.execSQLStatement("ALTER TABLE MSTITEM ADD CONSTRAINT DF_MSTITEM_UITCCGST DEFAULT (0) FOR UITCCGST")
            End If
        End If

        If fnFldPresent("UITCSGST", "MstItem") Then
            strSql = "select top 1 ItemID from MstItem where UITCSGST is null"
            If clsSVT.getDataLongValue(strSql, 0, False) > 0 Then
                clsSVT.execSQLStatement("Update MSTITEM SET UITCSGST= 0 WHERE UITCSGST Is NULL")
                clsSVT.execSQLStatement("ALTER TABLE MSTITEM ALTER COLUMN UITCSGST NUMERIC(18, 2) Not NULL")
                clsSVT.execSQLStatement("ALTER TABLE MSTITEM ADD CONSTRAINT DF_MSTITEM_UITCSGST DEFAULT (0) FOR UITCSGST")
            End If
        End If
        '----------------------------------------------

        strSql = "select docid from utltransactions where upper(docname)='ITEM MASTER'"
        intItemMstDocID = clsSVT.getDataLongValue(strSql, 0, False)
        If intItemMstDocID = 0 Then
            MsgBox("Invalid Item Master DocID!", MsgBoxStyle.Information, Me.Text)
            Exit Sub
        End If

        strSql = "Select FldCode,FldFormula from utldocdsgrflds"
        strSql &= " where docid=" & intItemMstDocID
        strSql &= " And isnull(FldFormula,'')<>''"
        rsTemp = clsSVT.getDataReaderFromSQL(strSql)
        If Not rsTemp.HasRows Then
            rsTemp.Close()
            Exit Sub
        End If

        Do While rsTemp.Read
            strFldCode = rsTemp!FldCode
            strFormula = Trim(rsTemp!FldFormula)

            strFormula = strFormula.Replace("#", "")

            Select Case Strings.Left(strFormula, 9)
                Case "SVTROUND2"
                    strFormula = strFormula.Replace("SVTROUND2", "ROUND")
                    strFormula = Strings.Left(strFormula, Len(strFormula) - 1) & ",2)"
                Case "SVTROUND4"
                    strFormula = strFormula.Replace("SVTROUND4", "ROUND")
                    strFormula = Strings.Left(strFormula, Len(strFormula) - 1) & ",4)"
            End Select

            strSql = "Update MstItem set " & strFldCode & "=" & strFormula
            strSql &= " where  and ItemID>0"
            clsSVT.execSQLStatement(strSql)
        Loop
        rsTemp.Close()

        MsgBox("Item Rates Updated/Recalculated!", MsgBoxStyle.Information, Me.Text)

    End Sub

    Private Sub frmMstItem_RightToLeftChanged(sender As Object, e As EventArgs) Handles Me.RightToLeftChanged

    End Sub
End Class