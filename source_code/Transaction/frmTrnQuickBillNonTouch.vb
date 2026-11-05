Imports C1.Win.C1FlexGrid
Imports System.Data.oledb

Public Class frmTrnQuickBillNonTouch
    Dim clsSvt As New svtLibrary
    Dim xPos As Integer, xMin As Integer, xMax As Integer
    Const vDetDel = 0
    Const vDetSlno = 1
    Const vDetBcatID = 2
    Const vDetBCatName = 3
    Const vDetItemId = 4
    Const vDetItemCode = 5
    Const vDetItemName = 6
    Const vDetQTY = 7
    Const vDetOTHList = 8
    Const vDetRate = 9
    Const vDetAmt = 10
    Const vDetParclRate = 11
    Const vDetParclAmt = 12

    Dim BlnParcel As Boolean

    Const intTotDetCols = 13

    Const vQDetItemid = 0
    Const vQQDetItem = 1

    Dim blnQtySel As Boolean = False

    'variables used for saving
    Dim strValues As String
    Dim strFlds As String

    Const vBCDetBCatID = 0
    Const vBCDetBCatName = 2
    Const vBCDetTotItemAmt = 3
    Const intTotBCDetCols = 4

    Dim intPriceCatID As Integer

    Dim intTblID As Integer = 0
    Dim intDocID As Integer
    Dim intCurMnuGrpID As Integer
    Dim strCurMnuGrp As String
    Dim strDefMenuGroup As String = ""
    Dim strCounter As String
    Dim intSalesCounter As Integer
    Dim blnConsiderBCat As Boolean = False
    Dim blnItemDsgrFldsPresentInDtl As Boolean = False

    Const vCFType = 0
    Const vCFColNo = 1
    Const vCFFldName = 2

    Dim intRowInvValue As Integer


    'invoice printing variables
    Dim strInvNoReset As String
    Dim strCounterWsRst As String
    Dim strDocNoFmt As String
    Dim strPrefixSettings As String
    Dim lngInvCNO As Long
    Dim strCounterPfx As String
    Dim intSalesCounterID As Integer
    Dim strRights As String
    Dim strKOTRights As String
    Dim intDtlColBCatID As Integer
    Dim strBCatID As String

    Public strContactNo As String
    Public strDelAddress As String

    Private Sub initScreen()
        Dim intFldID As Long
        intDocID = 203
        strRights = getUserRights(GetMenuId(Me.Text))
        intTblID = clsSvt.getDataLongValue("Select TblID from MstTbl where QuickServiceTable = 'Y'", 0, False)
        intPriceCatID = clsSvt.getDataLongValue("Select PriceCatID from MstTbl Where TblID = " & intTblID, 0, False)
        dtDocDate.Value = getSystemDate()
        strInvNoReset = getGsettings("Invoice", "InvNoReset", "")
        strCounterWsRst = getGsettings("Invoice", "CounterWsRst", "N")
        strDocNoFmt = getGsettings("Invoice", "DocNoFmt", "<<5>>/<<6>>")
        strPrefixSettings = getGsettings("Invoice", "PrefixSettings", "A,B,C,D,E,F,G,H,I,J,K,L")
        strCounter = clsSvt.getDataStringValue("Select SalesCounterCode from MstSalesCounter where SysName = '" & gStrSystemName & "'", "", False)
        strCounterPfx = clsSvt.getDataStringValue("Select SalesCounterPfx from MstSalesCounter where SysName = '" & gStrSystemName & "'", "", False)
        intSalesCounterID = clsSvt.getDataStringValue("Select SalesCounterID from MstSalesCounter where SysName = '" & gStrSystemName & "'", 0, False)

        If getGsettings("KOT", "QtySelKOT", "Y") = "Y" Then
            blnQtySel = True
        Else
            blnQtySel = False
        End If

        'get the global settings as to whether Billing category is applicable or not
        If getGsettings("Invoice", "BCatWise", "N") = "Y" Then blnConsiderBCat = True
        vsfColFlds.Visible = False
        If Not blnConsiderBCat Then
            vsfBC.Visible = False
        End If

        Call bindCombo(cboMenuGroup, "itemmenugroupcode", "temmenugroupcode", "Select itemmenugroupID, itemmenugroupcode from mstitemmenugroup order by 2")
        With vsfMenu
            .Rows.Count = 1
            .Cols.Count = 2
            .Cols(vQDetItemid).Visible = False
            .SetData(0, vQQDetItem, "Quick Selection")
        End With

        lblCounter.Text = "Counter:" & strCounter
        Timer1.Start()

        Call InitAddnlFldGrids(Me, vsfHdr, "Header", intDocID)
        vsfHdr.Cols(vOthFldDataValue).TextAlign = TextAlignEnum.RightCenter
        With vsfColFlds
            .Rows.Count = 1
            .Cols.Count = 3
        End With
        vsfHdr.ExtendLastCol = True

        With vsfDet
            .Rows.Count = 1
            .Cols.Count = intTotDetCols

            .SetData(0, vDetSlno, "Slno")
            .SetData(0, vDetBcatID, "BCat ID")
            .SetData(0, vDetBCatName, "BCat Name")
            .SetData(0, vDetItemId, "Item ID")
            .SetData(0, vDetItemCode, "Item Code")
            .SetData(0, vDetItemName, "Item Name")
            .SetData(0, vDetQTY, "Qty")
            .SetData(0, vDetRate, "Rate")
            .SetData(0, vDetAmt, "Amount")
            .SetData(0, vDetParclRate, "Parcel Rate")
            .SetData(0, vDetParclAmt, "Parcel Amount")
            .SetData(0, vDetDel, "Del")
            .Cols(vDetDel).TextAlignFixed = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter
            .Cols(vDetDel).ImageAlign = C1.Win.C1FlexGrid.ImageAlignEnum.CenterCenter
            .Cols(vDetSlno).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter
            .Cols(vDetSlno).TextAlignFixed = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter
            Call SetHideCol(vsfDet, True, vDetOTHList, vDetBcatID, vDetBCatName, vDetParclRate, vDetParclAmt)

            Call SetHideCol(vsfDet, True, vDetItemId, vDetItemCode)
            Call SetColWidth(vsfDet, 35, -1, -1, 190, 36, -1, -1, -1)
            .ExtendLastCol = False
        End With

        'Load designer fields of Invoice detail
        Call InitDetAddnlGrid(Me, vsfDet, vsfOptions, intTotDetCols, "TRNDTLDINV", intDocID)
        With vsfOptions
            .SetData(vDetSlno, vOthFldDataType, "Numeric")
            .SetData(vDetItemId, vOthFldDataType, "Numeric")
            .SetData(vDetItemCode, vOthFldDataType, "Text")
            .SetData(vDetItemName, vOthFldDataType, "Text")
            .SetData(vDetQTY, vOthFldDataType, "Numeric")
            .SetData(vDetItemCode, vOthFldCode, "ITEMCODE")
            .SetData(vDetQTY, vOthFldCode, "SVTPRODQTY")
            .SetData(vDetRate, vOthFldCode, "SVTPRODRATE")
            .SetData(vDetAmt, vOthFldCode, "SVTPRODAMOUNT")
            .SetData(vDetParclAmt, vOthFldCode, "PCKAMT")
        End With

        'initialise the bln field to TRUE only if there is at least ONE field having Item pickup field in Detail section
        intFldID = clsSvt.getDataLongValue("Select FldID from UtlDocDsgrFlds where DocID = " & intDocID & " and FldPickUpAfterFld = '#ITEM#'", 0, False)
        If intFldID > 0 Then blnItemDsgrFldsPresentInDtl = True

        'do this only if billing category is required
        If blnConsiderBCat Then
            'execute this statment to change Fldsection to detail from Header --- FOR Billing category flds
            clsSvt.execSQLStatement("update UtlDocDsgrFlds set FldSection = 'Detail' where docid = 204 and FldSection <> 'Detail'")

            With vsfBC
                .Rows.Count = 1
                .Cols.Count = intTotBCDetCols

                .SetData(0, vBCDetBCatID, "BCAT ID")
                .SetData(0, vBCDetBCatName, "BCat Name")
                .SetData(0, vBCDetTotItemAmt, "Total Item Amt")
                Call SetHideCol(vsfBC, True, vBCDetBCatID)
            End With

            'load designer fields of Billing Category
            Call InitDetAddnlGrid(Me, vsfBC, vsfOptionsBC, intTotBCDetCols, "TRNHDRDINVBC", 204)
            With vsfOptionsBC
                .SetData(vBCDetTotItemAmt, vOthFldCode, "TOTITEMAMT")
                .SetData(vBCDetTotItemAmt, vOthFldDataType, "Numeric")
            End With

            'Load all the Billing category fields to a grid with matching column number and field name
            Call fillVsfColFlds()
        End If

        intDtlColBCatID = getColNoFromAddnlDetGrid(vsfOptions, vsfDet, "BCatId")

        strBCatID = getGsettings("Invoice", "BCATID", "N")

        intRowInvValue = getRowNoFromAddnlGrid(vsfHdr, "INVVALUE")
        txtTblCode.Focus()
    End Sub

    Private Sub frmTrnQuickBillNonTouch_FormClosing(sender As Object, e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Call INIStore("QBILLNT", "DEFMENUGRP", cboMenuGroup.Text)
    End Sub

    Private Sub frmTrnQuickBillNonTouch_KeyUp(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyUp
        If e.KeyCode = Keys.F9 And e.Control = True Then
            Call initFormSize(Me, Me.Text, False, 850, 323, 0, 0)
            Call initFormVsfDet(Me, Me.Text, vsfDet, "Detail", "", SplitContainer1) 'Satish
            MsgBox("Restored Form & Grid Layout Successfully!!", vbInformation, Me.Text)
            Exit Sub
        End If
        If e.KeyCode = Keys.F8 And e.Control = True Then
            Call SaveActFormSettings(Me, Me.Text)
            Call SaveActFormVsfDet(Me, Me.Text, "Detail", vsfDet, "", SplitContainer1)  'Satish
            MsgBox("Saved Form & Grid Layout Successfully!!", vbInformation, Me.Text)

            Exit Sub
        End If
        If e.KeyCode = Keys.F8 Then
            Call btnSave_ClickButtonArea(Nothing, Nothing)
        End If
        If e.KeyCode = Keys.F3 Then
            Call btnClearKOT_ClickButtonArea(Nothing, Nothing)
        End If
        If e.KeyCode = Keys.F12 Then
            Call BtnParcel_ClickButtonArea(Nothing, Nothing)
        End If
    End Sub

    Private Sub frmTrnKOT_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        Dim ToolTip As New ToolTip()
        Call initScreen()
        'stringReplace(Me)
        Call clearFlds()
        fillVsfHFTDefValues(Me)
        cboMenuGroup.Text = INIGet("QBILLNT", "DEFMENUGRP", "")

        Call Timer1_Tick(New EventArgs, e)
        Me.KeyPreview = True
        txtTblCode.Focus()
        ToolTip.SetToolTip(Me.BtnParcel, "Parcel:Alt+R")
        ToolTip.SetToolTip(Me.btnOptions, "Options")
        ToolTip.SetToolTip(Me.btnClearItems, "ClearItems:Alt+C")
        ToolTip.SetToolTip(Me.btnSave, "PrintBill:Alt+B")
        ToolTip.SetToolTip(Me.btnClose, "Close:Alt+L")
        ToolTip.SetToolTip(Me.SplitContainer1, "Drag the slider to adjust the window")
        Call initFormSize(Me, Me.Text, False, 850, 323, 0, 0)
        Call initFormVsfDet(Me, Me.Text, vsfDet, "Detail", "", SplitContainer1)  'Satish
        txtTblCode.Focus()
        vsfHdr.Cols.Fixed = vOthFldCaption + 1
        'Call initInvGridStyles(vsfHdr)
        'Call initInvGridStyles(vsfDet)
        'Call initInvGridStyles(vsfMenu)
        pnlAmt.BackColor = vsfDet.Styles.Fixed.BackColor
        lblTotalCaption.ForeColor = vsfDet.Styles.Fixed.ForeColor
        lblTotal.ForeColor = vsfDet.Styles.Fixed.ForeColor

        vsfMenu.Styles.EmptyArea.BackColor = vsfDet.Styles.EmptyArea.BackColor
    End Sub

    Private Sub showbtns(ByVal argBLN As Boolean)
        If argBLN Then
            txtTblCode.Enabled = False
        Else
            txtTblCode.Enabled = True
        End If
        btnSave.Visible = argBLN
        'btnClearKOT.Visible = argBLN
    End Sub
    Public Sub clearFlds()
        vsfDet.Rows.Count = 1
        lblItemName.Text = ""
        lblParcel.Visible = False
        txtItemCode.Text = ""
        txtQty.Text = ""
        txtTblCode.Text = ""
        cboMenuGroup.SelectedIndex = -1
        SetHideCol(vsfDet, True, vDetParclAmt, vDetParclRate)
        lblTotal.Text = ""
        Call showbtns(False)
        Call ClearAddnlGrid(vsfHdr)
        vsfBC.Rows.Count = 1
        'Dim i As Integer
        'Dim rg As CellRange
        'Dim csWhite As CellStyle = vsfHdr.Styles.Add("WHITE")
        'csWhite.ForeColor = Color.White
        'Dim csRO As CellStyle = vsfHdr.Styles.Add("RO")
        'csRO.ForeColor = Color.DarkGray
        'For i = 0 To vsfHdr.Rows.Count - 1
        '    rg = vsfHdr.GetCellRange(i, vOthFldCaption)
        '    rg.Style = vsfHdr.Styles("WHITE")
        '    If vsfHdr.GetData(i, vOthFldBlnReadOnly) = "Y" Then
        '        rg.Style = vsfHdr.Styles("RO")
        '    End If
        'Next
        fillVsfHFTDefValues(Me)
    End Sub

    Private Sub btnClose_ClickButtonArea(Sender As System.Object, e As System.Windows.Forms.MouseEventArgs) Handles btnClose.ClickButtonArea
        Me.Close()
    End Sub

    Private Sub AddItemToGrid(argItemID As Long)
        Dim intBCatId As Integer, strSQL As String
        Dim i As Integer
        Dim introw As Integer

        If Val(txtQty.Text) = 0 Then
            txtQty.Text = 1
        End If

        With vsfDet
            introw = 0
            For i = 1 To .Rows.Count - 1
                If argItemID = Val(.GetData(i, vDetItemId)) Then
                    introw = i
                    Exit For
                End If
            Next i
            If introw = 0 Then
                .Rows.Count = .Rows.Count + 1
                introw = .Rows.Count - 1
            End If
            .TopRow = introw
            .Row = introw
            .SetData(introw, vDetItemId, argItemID)
            .SetData(introw, vDetItemCode, clsSvt.getDataStringValue("Select ItemCode from MstItem where ItemID = " & argItemID, "", False))
            .SetData(introw, vDetItemName, clsSvt.getDataStringValue("Select ItemName from MstItem where ItemID = " & argItemID, "", False))
            .SetCellImage(introw, vDetDel, My.Resources.cancel)
            .SetData(introw, vDetQTY, Val(.GetData(introw, vDetQTY)) + Val(txtQty.Text))
            .SetData(introw, vDetParclRate, clsSvt.getDataStringValue("Select PckAmt from MstItem where ItemID = " & argItemID, "", False))
            .SetData(introw, vDetRate, clsSvt.getDataDoubleValue("Select ItemRate from  MstItemPriceList Where PriceCatID = " & intPriceCatID & " and ItemID  = " & Val(.GetData(introw, vDetItemId)), 0, False))

            If intDtlColBCatID > 0 Then
                strSQL = "select " & strBCatID & " from MStItem where ItemId = " & argItemID
                intBCatId = clsSvt.getDataDoubleValue(strSQL, 0, False)
                .SetData(introw, intDtlColBCatID, intBCatId)
            End If

            Call calTotal()
            If blnItemDsgrFldsPresentInDtl Then
                Call updateItemFlds(argItemID, introw)
            End If
            Call calTotal()

            Call ReArrangeSlNo(vsfDet, vDetSlno)
            Call showbtns(True)
        End With

        Call RemoveEmptVSFRows(vsfDet, 1, vDetItemCode)
        ReArrangeSlNo(vsfDet, vDetSlno)
    End Sub

    Private Sub calTotal(Optional ByVal argint As Integer = 0)
        Dim i As Integer
        Dim sngTot As Double = 0

        With vsfDet
            For i = 1 To .Rows.Count - 1
                If BlnParcel = True Then
                    .SetData(i, vDetParclAmt, Math.Round(Val(.GetData(i, vDetParclRate)) * Val(.GetData(i, vDetQTY)), 2))
                Else
                    .SetData(i, vDetParclAmt, 0)
                End If
            Next
            For i = 1 To .Rows.Count - 1
                .SetData(i, vDetAmt, Math.Round(Val(.GetData(i, vDetRate)) * Val(.GetData(i, vDetQTY)), 2))
                ' .SetData(i, vDetAmt, Math.Round(Val(.GetData(i, vDetRate)) * Val(.GetData(i, vDetQTY)) + Val(.GetData(i, vDetParclAmt)), 2))
                sngTot = sngTot + Val(.GetData(i, vDetAmt))
            Next
            Call AddnlFldCalTotal(Me, vsfOptions, vsfDet, intTotDetCols, "", "CUSTOMER")

        End With
        ' End If

        'do this only if billing category is applicable
        If blnConsiderBCat Then
            With vsfBC
                For i = 1 To .Rows.Count - 1
                    sngTot = 0
                    For j = 1 To vsfDet.Rows.Count - 1
                        If Val(.GetData(i, vBCDetBCatID)) = Val(vsfDet.GetData(j, vDetBcatID)) Then
                            sngTot = sngTot + Val(vsfDet.GetData(j, vDetAmt))
                        End If
                    Next
                    .SetData(i, vBCDetTotItemAmt, sngTot)
                    For x = intTotBCDetCols To vsfOptionsBC.Rows.Count - 1
                        For j = 0 To vsfOptions.Rows.Count - 1
                            If vsfOptionsBC.GetData(x, vOthFldCode) = vsfOptions.GetData(j, vOthFldCode) Then
                                sngTot = GETCOLTOTBcat(vsfDet, x, j, Val(.GetData(i, vBCDetBCatID)))
                                .SetData(i, x, sngTot)
                                Exit For
                            End If
                        Next
                    Next
                Next
            End With

            'call dsgr calucation for Billing cat grid
            Call AddnlFldCalTotalDetOnly(Me, vsfOptionsBC, vsfBC, intTotBCDetCols, "", "CUSTOMER")
        End If

        lblTotal.Text = gstrCurCode & " " & Format(Val(vsfHdr.GetData(intRowInvValue, vOthFldDataValue)), "0.00")
    End Sub

    Private Sub btnSave_ClickButtonArea(Sender As System.Object, e As System.Windows.Forms.MouseEventArgs) Handles btnSave.ClickButtonArea
        'Added by SUrabhi
        If gCheckCounter = True Then
            If Not gCounterWiseFmt Then
                MsgBox("Counter wise reset is enabled in global settings!" & vbCrLf & "Please ensure counter number is a part of doc no. format!", vbInformation, Me.Text)
                frmUtlInvNumberFmt.ShowDialog()
                Exit Sub
            End If
        End If
        stringReplace(Me)
        If Not validFlds() Then Exit Sub
        Dim lngDInvID As Long
        lngDInvID = saveDInv()
        If lngDInvID > 0 Then
            Call PrintInvCrpt(lngDInvID)
            clearFlds()
        End If
        txtItemCode.Focus()
    End Sub

    Private Function saveDInv() As Long
        Dim strSQL As String, intMaxDocID As Long
        Dim AddressList() As String
        saveDInv = 0
        strSQL = "Insert into TrnHdrDInv (FinYear, DInvNo, invCNO, SalesCounterID, DInvDate,InvTime, TblID,ParcelBill,"
        strSQL = strSQL & "  AddedBy, AddedOn, PSysName, Cancelled,"
        strSQL = strSQL & "  ContactNo, DelAdd1, DelAdd2, DelAdd3, DelAdd4, DelAdd5"
        Call GetInsertString(Me, strValues, strFlds)
        strSQL = strSQL & strFlds & ") output inserted.DInvID into @testTable values ("
        strSQL = strSQL & gSettings.gCompFinYear & ","
        strSQL = strSQL & "'" & getInvNo() & "',"
        strSQL = strSQL & lngInvCNO & ","
        strSQL = strSQL & intSalesCounterID & ","
        strSQL = strSQL & "'" & getDateFormatInInsert(getSystemDate) & "', "
        strSQL = strSQL & "'" & getSystemDateTime.ToString("hh:mm:ss") & "',"
        strSQL = strSQL & intTblID & ","
        If BlnParcel = True Then
            strSQL = strSQL & "'" & "Y" & "',"
        Else
            strSQL = strSQL & "'" & "N" & "',"
        End If
        strSQL = strSQL & "'" & gUserCode & "', "
        strSQL = strSQL & "'" & getDateFormatInInsert(getSystemDateTime, True) & "', "
        strSQL = strSQL & "'" & strCounter & "', "
        strSQL = strSQL & "'N',"
        strSQL = strSQL & " '" & strContactNo & "',"
        If strDelAddress <> "" Then
            AddressList = strDelAddress.Split("▐")
            For i = 0 To UBound(AddressList)
                strSQL = strSQL & " '" & Trim(AddressList(i)) & "',"
            Next
            strSQL = Mid(strSQL, 1, Len(strSQL) - 1)
        Else
            strSQL = strSQL & " '','','','',''"
        End If
        strSQL = strSQL & strValues
        Dim svtTrn As oledb.oledbTransaction
        svtTrn = svtConn.BeginTransaction(IsolationLevel.ReadCommitted)
        Try
            intMaxDocID = getMaxInsertID(strSQL, "TrnHdrDInv", svtTrn)
            If intMaxDocID = -1 Then
                MsgBox("Error during save" & vbCrLf & "Sql:" & strSQL, MsgBoxStyle.Critical, Me.Text)
                svtTrn.Rollback()
                Exit Function
            End If
            saveDInv = intMaxDocID
            'Insert detail
            With vsfDet
                For i = 1 To .Rows.Count - 1
                    strFlds = ""
                    strValues = ""

                    strFlds = "Insert into TrnDtlDInv (DInvID, SlNo, ItemID, Remarks, DInvQty,DInvRate, DInvAmt,PCKrate,PCkAmt,ItemExtraInfo,"
                    strValues = intMaxDocID & ","
                    strValues = strValues & i & ","
                    strValues = strValues & Val(.GetData(i, vDetItemId)) & ","
                    strValues = strValues & "'',"
                    strValues = strValues & Val(.GetData(i, vDetQTY)) & ","
                    strValues = strValues & Val(.GetData(i, vDetRate)) & ","
                    strValues = strValues & Val(.GetData(i, vDetAmt)) & ","
                    strValues = strValues & Val(.GetData(i, vDetParclRate)) & ","
                    strValues = strValues & Val(.GetData(i, vDetParclAmt)) & ","
                    strValues = strValues & "'" & .GetData(i, vDetOTHList) & "',"
                    Call GetvsfOptDetInsertString(Me, vsfDet, vsfOptions, intTotDetCols, i, strFlds, strValues, svtTrn)
                    strSQL = strFlds & strValues
                    If Not clsSvt.execSQLStatement(strSQL, svtTrn) Then
                        MsgBox("Error during save -- " & strSQL, MsgBoxStyle.Critical, Me.Text)
                        saveDInv = 0
                        svtTrn.Rollback()
                        Exit Function
                    End If
                Next
            End With

            'insert bill category
            If blnConsiderBCat Then
                For i = 1 To vsfBC.Rows.Count - 1
                    strFlds = ""
                    strValues = ""

                    strFlds = "Insert into TrnHdrDInvBC (DInvID, BCatID, TotItemAmt,"
                    strValues = intMaxDocID & ", "
                    strValues = strValues & Val(vsfBC.GetData(i, vBCDetBCatID)) & ","
                    strValues = strValues & Val(vsfBC.GetData(i, vBCDetTotItemAmt)) & ","
                    Call GetvsfOptDetInsertString(Me, vsfBC, vsfOptionsBC, intTotBCDetCols, i, strFlds, strValues, svtTrn)

                    strSQL = strFlds & strValues

                    If Not clsSvt.execSQLStatement(strSQL, svtTrn) Then
                        MsgBox("Error during save -- " & strSQL, MsgBoxStyle.Critical, Me.Text)
                        svtTrn.Rollback()
                        Exit Function
                    End If

                Next
            End If

            svtTrn.Commit()

        Catch ex As Exception
            saveDInv = 0
            MsgBox("Error during save" & vbCrLf & ex.Message, MsgBoxStyle.Critical, Me.Text)
            svtTrn.Rollback()
            Exit Function
        End Try
    End Function

    Private Function getAutoTrnNo() As String
        Dim intTrnNo As Long
        intTrnNo = Val(TrnNoAutoGen("TrnHdrDInv", "DInvNo", "", getSystemDate, intDocID))
        getAutoTrnNo = getFmtDocNo("", intTrnNo, getSystemDate)
    End Function

    Private Function validFlds() As Boolean
        validFlds = False
        If intTblID = 0 Then
            MsgBox("Invalid Table!!", MsgBoxStyle.Critical, Me.Text)
            Exit Function
        End If
        If vsfDet.Rows.Count = 1 Then
            MsgBox("Nothing to Save. Select at least one item.", MsgBoxStyle.Critical, Me.Text)
            Exit Function
        End If

        For i = 1 To vsfDet.Rows.Count - 1
            If vsfDet.GetData(i, vDetRate) = 0 Then
                'MsgBox("Rate is not present for item " & vsfDet.GetData(i, vDetItemName), MsgBoxStyle.Critical, Me.Text)
                'Exit Function
            End If
        Next i

        validFlds = True
    End Function


    Private Sub btnClearKOT_ClickButtonArea(Sender As System.Object, e As System.Windows.Forms.MouseEventArgs) Handles btnClearItems.ClickButtonArea
        Call clearFlds()
        txtTblCode.Focus()
    End Sub


    Private Sub Timer1_Tick(sender As Object, e As System.EventArgs) Handles Timer1.Tick
        lblDatetime.Text = "Date Time :" & Format(Date.Now, "dd MMM yyyy hh:mm tt")
    End Sub

    Private Sub btnOptions_DropDownItemClicked(sender As Object, e As C1.Win.C1Input.DropDownItemClickedEventArgs) Handles btnOptions.DropDownItemClicked
        Select Case e.ClickedItem.Text
            Case "Re-Print Bill"
                'If Not hasKOTBtnRights("Re-Print Bill") Then Exit Sub
                'frmUtlDInvRP.ShowDialog()
            Case "Customer / Member Dtls"
                'If Not hasKOTBtnRights("Customer / Member Dtls") Then Exit Sub
                'Dim frmMstCust As New frmMstPOSCust(True)
                'With frmMstCust
                '    .ShowDialog()
                'End With
        End Select
    End Sub

    Private Sub fillVsfColFlds()
        vsfColFlds.Rows.Count = 1
        Dim rsBC As oledbDataReader, intRow As Integer, intFld As Integer

        'fill Billing category fields that are to be filled by default (example: STPer, SBCPer...)
        rsBC = clsSvt.getDataReaderFromSQL("Select top 1 * from MstBCat")
        With rsBC
            If .Read Then
                For intRow = intTotBCDetCols To vsfOptionsBC.Rows.Count - 1
                    For intFld = 0 To rsBC.FieldCount - 1
                        If UCase(vsfOptionsBC.GetData(intRow, vOthFldCode)) = UCase(rsBC.GetName(intFld)) Then
                            vsfColFlds.Rows.Count = vsfColFlds.Rows.Count + 1
                            vsfColFlds.SetData(vsfColFlds.Rows.Count - 1, vCFType, "BCAT")
                            vsfColFlds.SetData(vsfColFlds.Rows.Count - 1, vCFFldName, UCase(vsfOptionsBC.GetData(intRow, vOthFldCode)))
                            vsfColFlds.SetData(vsfColFlds.Rows.Count - 1, vCFColNo, intRow)
                            Exit For
                        End If
                    Next
                Next
            End If
            .Close()
        End With

        ''fill Item fields that are to be filled by default (example: VATPER...)
        'rsItem = clsSvt.getDataReaderFromSQL("Select top 1 * from MstItem")
        'With rsItem
        '    If .Read Then
        '        For intRow = intTotDetCols To vsfOptions.Rows.Count - 1
        '            For intFld = 0 To rsItem.FieldCount - 1
        '                If UCase(vsfOptions.GetData(intRow, vOthFldCode)) = UCase(rsItem.GetName(intFld)) Then
        '                    vsfColFlds.Rows.Count = vsfColFlds.Rows.Count + 1
        '                    vsfColFlds.SetData(vsfColFlds.Rows.Count - 1, vCFType, "ITEM")
        '                    vsfColFlds.SetData(vsfColFlds.Rows.Count - 1, vCFFldName, UCase(vsfOptions.GetData(intRow, vOthFldCode)))
        '                    vsfColFlds.SetData(vsfColFlds.Rows.Count - 1, vCFColNo, intRow)
        '                    Exit For
        '                End If
        '            Next
        '        Next
        '    End If
        '    .Close()
        'End With

    End Sub

    Private Sub fillBCatDefaultFlds()
        Dim i As Integer, j As Integer, x As Integer
        Dim rsBC As oledbDataReader

        'fill Billing category fields that are to be filled by default (example: STPer, SBCPer...)
        rsBC = clsSvt.getDataReaderFromSQL("Select * from MstBCat")
        With rsBC
            Do While .Read
                For i = 1 To vsfBC.Rows.Count - 1
                    If Val(vsfBC.GetData(i, vBCDetBCatID)) = !BcatID Then
                        For j = 1 To vsfColFlds.Rows.Count - 1
                            If vsfColFlds.GetData(j, vCFType) = "BCAT" Then
                                x = Val(vsfColFlds.GetData(j, vCFColNo))
                                vsfBC.SetData(i, x, GetVal(rsBC(vsfColFlds.GetData(j, vCFFldName)), ""))
                            End If
                        Next
                    End If
                Next i
            Loop
            .Close()
        End With
    End Sub

    Private Function GETCOLTOTBcat(argVsf As C1FlexGrid, argBCRow As Integer, argDetCol As Integer, argBcatID As Long)
        GETCOLTOTBcat = 0
        Dim z As Integer, sngBTot As Double = 0
        With argVsf
            For z = 0 To .Rows.Count - 1
                If argBcatID = Val(.GetData(z, vDetBcatID)) Then
                    sngBTot = sngBTot + Val(.GetData(z, argDetCol))
                End If
            Next
        End With
        GETCOLTOTBcat = sngBTot
    End Function

    Private Sub updateItemFlds(argItemID As Long, argRow As Integer)
        Dim strSQL As String, i As Integer
        Dim rsItem As oledb.oledbDataReader
        strSQL = "Select fldcode,FldPickUpAfterFld,fldpickup  "
        strSQL = strSQL & " from utldocdsgrflds "
        strSQL = strSQL & " where fldpickupafterfld ='#ITEM#'"
        strSQL = strSQL & " and DocID = " & intDocID

        rsItem = clsSvt.getDataReaderFromSQL(strSQL)
        With rsItem
            Do While .Read
                For i = 0 To vsfOptions.Rows.Count - 1
                    If UCase(vsfOptions.GetData(i, vOthFldCode)) = UCase(!FldCode) Then
                        strSQL = GetVal(rsItem!FldPickUp, "")
                        If strSQL <> "" Then
                            '"SELECT VATPER FROM MSTITEM WHERE ITEMID=#ITEMID#"
                            strSQL = Replace(strSQL, "#ITEMID#", argItemID)

                            Select Case vsfOptions.GetData(i, vOthFldDataType)
                                Case "Numeric"
                                    If Val(vsfOptions.GetData(i, vOthFldNumDecimals)) = 0 Then
                                        vsfDet.SetData(argRow, i, clsSvt.getDataLongValue(strSQL, 0, False))
                                    Else
                                        vsfDet.SetData(argRow, i, clsSvt.getDataDoubleValue(strSQL, 0, False))
                                    End If
                                Case Else
                                    vsfDet.SetData(argRow, i, clsSvt.getDataStringValue(strSQL, "", False))
                            End Select
                        End If
                        Exit For
                    End If
                Next
            Loop
            .Close()
        End With
    End Sub

    Private Sub vsfHdr_AfterEdit(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfHdr.AfterEdit

        Call vsfGenAfterEditBfrCalTot(Me, vsfHdr, e.Row, "M")
        Call calTotal()
        Call vsfGenAfterEditAftrCalTot(Me, vsfHdr, intDocID, "A", e.Row, False)
        setGridColorWhite()
        'Call AutosizeMstfrmGrid(frmMstGen)
    End Sub

    Private Sub vsfDet_BeforeEdit(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfDet.BeforeEdit
        e.Cancel = True
    End Sub

    Private Sub vsfHdr_BeforeEdit(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfHdr.BeforeEdit
        If vsfBeforeEditCancel(Me, vsfHdr, intDocID, "TRNHDRDINV", "A", e.Row, e.Col, "M", "") Then
            e.Cancel = True
            Exit Sub
        End If
    End Sub

    Private Sub vsfHdr_EnterCell(ByVal sender As Object, ByVal e As System.EventArgs) Handles vsfHdr.EnterCell
        'If Mode = "V" Then Exit Sub
        'With vsfHdr
        '    If .GetData(.Row, vOthFldTxtBlnMultiLine) = "Y" Then Exit Sub

        '    If .GetData(.Row, vOthFldBlnComboData) = "Y" Then Exit Sub
        '    If .GetData(.Row, vOthFldDataType) = "List String" Then Exit Sub
        '    If .GetData(.Row, vOthFldBlnReadOnly) = "Y" Then Exit Sub
        '    If .GetData(.Row, vOthFldBlnRefill) = "Y" Then Exit Sub

        '    .StartEditing()
        'End With
    End Sub

    Private Sub vsfHdr_KeyPressEdit(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.KeyPressEditEventArgs) Handles vsfHdr.KeyPressEdit
        If e.KeyChar = "'" Then
            e.Handled = True
        End If
        If vsfKeyPressEdit(Me, vsfHdr, vsfHdr.Row, e.KeyChar) Then
            e.Handled = False
        Else
            e.Handled = True
        End If
    End Sub

    Private Sub vsfHdr_RowColChange(ByVal sender As Object, ByVal e As System.EventArgs) Handles vsfHdr.RowColChange
        'If Not (Mode = "A" Or Mode = "M") Then Exit Sub
        'Call vsfRowColChange(Me, vsfHdr, Nothing)
    End Sub

    Private Sub vsfHdr_SetupEditor(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfHdr.SetupEditor
        Call vsfSetupEditor(Me, vsfHdr, intDocID, "TRNHDRDINV", "A", e.Row, e.Col, "M")
    End Sub

    Private Sub vsfHdr_ValidateEdit(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.ValidateEditEventArgs) Handles vsfHdr.ValidateEdit
        If Not vsfValidateEdit(Me, vsfHdr, e.Row, e.Col, "M", "TRNHDRDINV", "DINVNO", "") Then
            e.Cancel = True
        End If
    End Sub

    Private Sub vsfBC_AfterEdit(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfBC.AfterEdit
        If e.Col <> vDetSlno Then Call vsfOptAfterEditBfrCalTot(Me, vsfBC, vsfOptionsBC, vsfBC.Row, vsfBC.Col)
        Call calTotal()
    End Sub

    Private Sub vsfBC_ValidateEdit(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.ValidateEditEventArgs) Handles vsfBC.ValidateEdit

        If vsfBC.Col < intTotBCDetCols Then Exit Sub

        If vsfOptValidateEdit(Me, vsfDet, vsfOptionsBC, vsfBC.Row, vsfBC.Col, "T", "TRNDTLINVBC", "", "") = False Then
            e.Cancel = True
        End If
    End Sub

    Private Function getInvNo() As String
        Dim strSQL As String, strPfx As String = "", strFldValue As String
        'Invoice()
        'utlgsettings()
        'InvNoReset -- D M blank
        'counterWsRst(--Y / N)
        'DocNoFmt -- pattern with double angle brackets -- <<2>>-<<6>>/<<7>>
        getInvNo = ""
        strSQL = "Select max(invCNO) + 1 from TrnHdrDInv"
        strSQL = strSQL & " where FinYear = " & gSettings.gCompFinYear
        Select Case strInvNoReset
            Case "D"
                strSQL = strSQL & " and month(Dinvdate) = " & Month(dtDocDate.Value)
                strSQL = strSQL & " and day(Dinvdate) = " & DateAndTime.Day(dtDocDate.Value)
                strPfx = getMnthDayPfx(Month(dtDocDate.Value), strPrefixSettings)
                strPfx = strPfx & DateAndTime.Day(dtDocDate.Value)
            Case "M"
                strSQL = strSQL & " and month(Dinvdate) = " & Month(dtDocDate.Value)
                strPfx = getMnthDayPfx(Month(dtDocDate.Value), strPrefixSettings)
        End Select
        If strCounterWsRst = "Y" Then
            strSQL = strSQL & " and SalesCounterID = " & intsalescounterID
        End If
        lngInvCNO = clsSvt.getDataLongValue(strSQL, 1, False)
        strFldValue = strDocNoFmt
        If strFldValue <> "" Then
            strFldValue = Replace(strFldValue, "<<1>>", strPfx)
            strFldValue = Replace(strFldValue, "<<2>>", strCounterPfx)
            strFldValue = Replace(strFldValue, "<<3>>", Strings.Right(gSettings.gCompStDt, 4))
            strFldValue = Replace(strFldValue, "<<4>>", Strings.Right(gSettings.gCompEndDt, 4))
            strFldValue = Replace(strFldValue, "<<5>>", Strings.Right(gSettings.gCompStDt, 2))
            strFldValue = Replace(strFldValue, "<<6>>", Strings.Right(gSettings.gCompEndDt, 2))
            strFldValue = Replace(strFldValue, "<<7>>", Format(lngInvCNO, "0000000"))
            strFldValue = Replace(strFldValue, "<<8>>", lngInvCNO)
        End If
        strFldValue = Replace(strFldValue, "//", "/")
        strFldValue = Replace(strFldValue, "--", "-")
        strFldValue = Replace(strFldValue, ",", "")
        getInvNo = strFldValue
    End Function
    Private Function getMnthDayPfx(argMonth As Integer, argSet As String) As String
        getMnthDayPfx = ""
        Dim arr() As String = Split(argSet, ",")
        If arr.Length <= 0 Then Exit Function
        Try
            getMnthDayPfx = arr(argMonth - 1)
        Catch ex As Exception

        End Try

    End Function

    Private Sub txtQty_GotFocus(sender As Object, e As System.EventArgs)
        txtQty.SelectAll()
    End Sub
    Private Sub txtItemCode_GotFocus(sender As Object, e As System.EventArgs) Handles txtItemCode.GotFocus
        txtItemCode.SelectAll()
    End Sub
    Private Sub cboMenuGroup_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cboMenuGroup.SelectedIndexChanged
        Dim rs As oledb.oledbDataReader
        Dim strSQL As String
        strSQL = "Select i.* "
        strSQL = strSQL & " from MstItem i, MstMenuGroupItems s, MstItemMenuGroup g"
        strSQL = strSQL & "  where g.ItemMenuGroupCode = '" & cboMenuGroup.Text & "'"
        strSQL = strSQL & "  and i.itemid = s.itemid"
        strSQL = strSQL & "  and s.itemMenugroupid = g.itemMenugroupid"
        strSQL = strSQL & " order by Itemcode"
        rs = clsSvt.getDataReaderFromSQL(strSQL)
        vsfMenu.Rows.Count = 1
        With rs
            Do While .Read
                vsfMenu.Rows.Count = vsfMenu.Rows.Count + 1
                vsfMenu.SetData(vsfMenu.Rows.Count - 1, vQDetItemid, !ItemID)
                vsfMenu.SetData(vsfMenu.Rows.Count - 1, vQQDetItem, !ItemCode & "-" & !ItemName)
            Loop
            .Close()
        End With
    End Sub
    Private Sub vsfMenu_BeforeEdit(sender As Object, e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfMenu.BeforeEdit
        e.Cancel = True
    End Sub

    Private Sub txtItemCode_KeyUp(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles txtItemCode.KeyUp
        Dim lngItemID As Integer
        Dim blnFound As Boolean
        If intTblID = 0 Then
            MsgBox("Please select Table!", MsgBoxStyle.Critical, Me.Text)
            Exit Sub
        End If

        If e.KeyCode = Keys.Down Then
            Dim frmKB As New frmUtlKeyBoard
            Dim strSQL As String
            txtItemCode.Text = ""
            With frmKB
                strSQL = "Select ItemID, Itemcode, Itemname from MstItem "
                strSQL = strSQL & " where Inactive  = 'N' and itemid > 0 "
                strSQL = strSQL & " and itemID in (Select ItemID from MstItemPriceList where ItemRate > 0 and PriceCatID = " & intPriceCatID & ")"
                strSQL = strSQL & " order by ItemCode"
                .enableDisableOptBtns("S")
                .ShowUtlKeyboard(KeyboardType.AlphaNumeric, True, strSQL, txtItemCode, 1, 0, 2)
                .enableDisableOptBtns("S")
                .ShowDialog()
                If Trim(txtItemCode.Text) <> "" Then
                    GoTo fromKB
                    'AddItemToGrid(Val(txtItemCode.Text))
                End If
            End With
        End If

        If Trim(txtItemCode.Text) = "+" Then
            txtItemCode.Text = ""
            For i = vsfDet.Rows.Count - 1 To 1 Step -1
                If vsfDet.GetData(i, vDetItemCode) <> "" Then
                    vsfDet.Row = i
                    vsfDet.SetData(i, vDetQTY, Val(vsfDet.GetData(i, vDetQTY) + 1))
                    Call calTotal()
                    txtItemCode.Text = ""
                    Exit For
                End If
            Next i
        End If

        If Trim(txtItemCode.Text) = "-" Then
            txtItemCode.Text = ""
            For i = vsfDet.Rows.Count - 1 To 1 Step -1
                If vsfDet.GetData(i, vDetItemCode) <> "" Then
                    vsfDet.Row = i
                    If vsfDet.GetData(i, vDetQTY) > 1 Then
                        vsfDet.SetData(i, vDetQTY, Val(vsfDet.GetData(i, vDetQTY) - 1))
                    End If
                    Call calTotal()
                    txtItemCode.Text = ""

                    Exit For
                End If
            Next i
        End If


        If e.KeyCode = Keys.Return Or e.KeyCode = Keys.Tab Then
fromKB:
            txtItemCode.Text = Trim(UCase(txtItemCode.Text))
            If txtItemCode.Text = "" Then
                If vsfDet.Rows.Count > 1 Then
                    'btnSave.Focus()
                    Exit Sub
                End If
            End If

            If Trim(txtItemCode.Text) <> "" Then
                'sngQOH = getQOH(txtItemCode.Text)
                lngItemID = clsSvt.getDataLongValue("Select ItemID from MstItem where ItemCode = '" & txtItemCode.Text & "'", 0, blnFound)
                If Not blnFound Then
                    lngItemID = clsSvt.getDataLongValue("Select ItemID from MstItem where HotKey = '" & txtItemCode.Text & "'", 0, blnFound)
                    txtItemCode.Text = clsSvt.getDataStringValue("Select ItemCode from MstItem Where HotKey = '" & txtItemCode.Text & "'", "", blnFound)
                End If
                If Not blnFound Then
                    MsgBox("Item not found!!", vbInformation, Me.Text)
                    txtItemCode.Text = ""
                    lblItemName.Text = ""
                    Exit Sub
                Else
                    If isPriceZero(lngItemID) Then
                        Exit Sub
                    End If
                End If

                lblItemName.Text = clsSvt.getDataStringValue("Select ItemName from MstItem Where ItemCode = '" & txtItemCode.Text & "'", "", blnFound)

                If blnQtySel Then
                    txtQty.Text = 1
                    txtQty.Focus()
                Else
                    'write code to update item to grid with qty as 1
                    txtQty.Text = 1
                    AddItemToGrid(lngItemID)
                    'Call txtQty_KeyUp(vbKeyReturn, 0)
                    txtItemCode.Text = ""
                    lblItemName.Text = ""
                End If
            End If
        End If

        If e.KeyCode = Keys.Escape Then
            btnSave.Focus()
        End If
    End Sub

    Private Sub txtItemCode_Validated(sender As Object, e As System.EventArgs) Handles txtItemCode.Validated
        If Trim(txtItemCode.Text) <> "" Then
            lblItemName.Text = clsSvt.getDataStringValue("Select ItemName From MstItem Where ItemCode = '" & txtItemCode.Text & "'", "", False)

            If Trim(lblItemName.Text) = "" Then
                lblItemName.Text = clsSvt.getDataStringValue("Select ItemName From MstItem Where HotKey = '" & txtItemCode.Text & "'", "", False)
            End If
        End If
    End Sub

    Private Sub LblItemCaption_Click(sender As Object, e As System.EventArgs) Handles lblItemCaption.Click
        Dim frmKB As New frmUtlKeyBoard
        Dim lngItemID As Integer
        Dim blnFound As Boolean
        Dim strSQL As String
        txtItemCode.Text = ""
        With frmKB
            strSQL = "Select ItemID, Itemcode, Itemname from MstItem "
            strSQL = strSQL & " where Inactive  = 'N' and itemid > 0 "
            strSQL = strSQL & " and itemID in (Select ItemID from MstItemPriceList where ItemRate > 0 and PriceCatID = " & intPriceCatID & ")"
            strSQL = strSQL & " order by ItemCode"
            .ShowUtlKeyboard(KeyboardType.AlphaNumeric, True, strSQL, txtItemCode, 1, 0, 2)
            .enableDisableOptBtns("S")
            .ShowDialog()
            If Trim(txtItemCode.Text) <> "" Then
                'sngQOH = getQOH(txtItemCode.Text)
                lngItemID = clsSvt.getDataLongValue("Select ItemID from MstItem where ItemCode = '" & txtItemCode.Text & "'", 0, blnFound)
                If Not blnFound Then
                    lngItemID = clsSvt.getDataLongValue("Select ItemID from MstItem where HotKey = '" & txtItemCode.Text & "'", 0, blnFound)
                    txtItemCode.Text = clsSvt.getDataStringValue("Select ItemCode from MstItem Where HotKey = '" & txtItemCode.Text & "'", "", blnFound)
                End If
                If Not blnFound Then
                    MsgBox("Item not found!!", vbInformation, Me.Text)
                    txtItemCode.Text = ""
                    lblItemName.Text = ""
                    Exit Sub
                End If

                lblItemName.Text = clsSvt.getDataStringValue("Select ItemName from MstItem Where ItemCode = '" & txtItemCode.Text & "'", "", blnFound)

                If blnQtySel Then
                    txtQty.Text = 1
                    txtQty.Focus()
                Else
                    'write code to update item to grid with qty as 1
                    txtQty.Text = 1
                    AddItemToGrid(lngItemID)
                    'Call txtQty_KeyUp(vbKeyReturn, 0)
                    txtItemCode.Text = ""
                    lblItemName.Text = ""
                End If
            End If

            'If Val(txtItemCode.Text) > 0 Then
            '    AddItemToGrid(Val(txtItemCode.Text))
            'End If
        End With
    End Sub

    Private Sub vsfMenu_MouseDoubleClick(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles vsfMenu.MouseDoubleClick
        Dim ht = vsfMenu.HitTest
        If intTblID = 0 Then
            MsgBox("Please select Table!", MsgBoxStyle.Critical, Me.Text)
            Exit Sub
        End If
      
        If vsfMenu.Rows.Count > 0 Then
            If ht.Row > 0 Then
                If isPriceZero(Val(vsfMenu.GetData(ht.Row, vQDetItemid))) Then
                    Exit Sub
                End If

                AddItemToGrid(Val(vsfMenu.GetData(ht.Row, vQDetItemid)))
            End If
        End If
    End Sub

    Private Sub txtQty_KeyUp(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles txtQty.KeyUp
        Dim lngItemId As Long, blnFound As Boolean
        'Dim sngQOH As Single, strNonStk As String
        Dim introw As Integer, i As Integer

        If e.KeyCode = Keys.Escape Then
            txtQty.Text = ""
            txtItemCode.Text = ""
            lblItemName.Text = ""
            txtItemCode.Focus()
            Exit Sub
        End If

        If e.KeyCode = Keys.Return Then
            If txtItemCode.Text = "" Then
                MsgBox("Please select an item first!", vbInformation, Me.Text)
                txtItemCode.Focus()
                Exit Sub
            End If

            If txtItemCode.Text = "" Then Exit Sub

            If Trim(txtQty.Text) = "" Then txtQty.Text = "1"

            If Val(txtQty.Text) = 0 Then
                MsgBox("Invalid Qty", vbInformation, Me.Text)
                txtQty.Focus()
                Exit Sub
            End If

            lngItemId = clsSvt.getDataLongValue("Select ItemID from MstItem Where ItemCode = '" & txtItemCode.Text & "'" & _
                                                " OR HotKey = '" & txtItemCode.Text & "'", 0, False)

            If lngItemId <= 0 Then
                MsgBox("Invalid Item!!", vbInformation, Me.Text)
                txtItemCode.Focus()
                Exit Sub
            Else
                If isPriceZero(lngItemId) Then
                    Exit Sub
                End If
            End If

            '        If Not gBlnAllowNegativeStock Then
            '            If strNonStk <> "Y" Then
            '                If blnHideQOH = False Then
            '                    sngQOH = getQOH(CInt(lngItemId), 1, dtDocDate.value, dtDocDate.value, gSystemUser, "Y")
            '                    If Val(txtQty.Text) > sngQOH Then
            '                        MsgBox "Cannot Exceed QOH " & Format(sngQOH, "0"), vbInformation, Me.Caption
            '                        txtQty.SetFocus
            '                        Exit Sub
            '                    End If
            '                End If
            '            End If
            '        End If

            blnFound = False
            With vsfDet
                For i = 1 To .Rows.Count - 1
                    If .GetData(i, vDetItemCode) = txtItemCode.Text Then
                        blnFound = True
                        introw = i
                        Exit For
                    End If
                Next i
            End With

            If Not blnFound Then
                vsfDet.Rows.Count = vsfDet.Rows.Count + 1
                vsfDet.Row = vsfDet.Rows.Count - 1
                vsfDet.TopRow = vsfDet.Rows.Count - 1

                AddItemToGrid(lngItemId)
            Else
                vsfDet.SetData(introw, vDetQTY, Val(vsfDet.GetData(introw, vDetQTY) + Val(txtQty.Text)))
            End If
            Call calTotal()

            txtItemCode.Text = ""
            lblItemName.Text = ""
            txtQty.Text = "1"

            txtItemCode.Focus()
        End If
        If vsfDet.Rows.Count > 1 Then
            Call RemoveEmptVSFRows(vsfDet, 1, vDetItemCode)
            ReArrangeSlNo(vsfDet, vDetSlno)
        End If
        vsfDet.AutoSizeCols()
        vsfDet.AutoSizeRows()
    End Sub

 
    Private Sub BtnParcel_ClickButtonArea(Sender As System.Object, e As System.Windows.Forms.MouseEventArgs) Handles BtnParcel.ClickButtonArea
        'If Not hasKOTBtnRights("Parcel") Then Exit Sub

        If BlnParcel = False Then
            BlnParcel = True
            lblParcel.Visible = True
            SetHideCol(vsfDet, False, vDetParclAmt, vDetParclRate)
            calTotal()
        Else
            lblParcel.Visible = False
            BlnParcel = False
            SetHideCol(vsfDet, True, vDetParclAmt, vDetParclRate)
            calTotal()
        End If
    End Sub

    Private Sub FlowLayoutPanel1_Paint(sender As System.Object, e As System.Windows.Forms.PaintEventArgs) Handles FlowLayoutPanel1.Paint

    End Sub

    Private Sub btnAddItem_Click(sender As System.Object, e As System.EventArgs) Handles btnAddItem.Click
        txtQty_KeyUp(Me, New KeyEventArgs(Keys.Enter))
    End Sub

    Private Sub vsfDet_MouseClick(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles vsfDet.MouseClick
        Dim ht = vsfDet.HitTest

        With vsfDet
            If ht.Row <= 0 Then Exit Sub
            If ht.Column <> vDetDel Then Exit Sub
            If MsgBox("Are you sure you want to delete the item '" & .GetData(ht.Row, vDetItemName) & "'?", vbYesNo + vbQuestion + vbDefaultButton2, "Item deletion...") = MsgBoxResult.No Then Exit Sub
            .RemoveItem(ht.Row)
            Call ReArrangeSlNo(vsfDet, vDetSlno)
            Call calTotal()
            If ht.Row <= 0 Then
                btnSave.Visible = False
            End If
        End With
    End Sub

    Private Sub vsfDet_MouseDoubleClick(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles vsfDet.MouseDoubleClick
        Dim ht = vsfDet.HitTest(e.X, e.Y)
        If ht.Row <= 0 Then Exit Sub
        'With frmQBillItemDtl
        '    .intKOTItemRow = ht.Row
        '    .intcolOthInsl = vDetOTHList
        '    .intColQty = vDetQTY
        '    .showDetails(Me, vsfDet.GetData(ht.Row, vDetItemCode), vsfDet.GetData(ht.Row, vDetItemName), Val(vsfDet.GetData(ht.Row, vDetQTY)), vsfDet.GetData(ht.Row, vDetOTHList))
        '    .ShowDialog()
        'End With
        calTotal()
    End Sub

    Private Sub setGridColorWhite()
        'Dim i As Integer
        'Dim rg As CellRange
        'Dim csWhite As CellStyle = vsfHdr.Styles.Add("WHITE")
        'csWhite.ForeColor = Color.White
        'Dim csRO As CellStyle = vsfHdr.Styles.Add("RO")
        'csRO.ForeColor = Color.DarkGray
        'For i = 0 To vsfHdr.Rows.Count - 1
        '    rg = vsfHdr.GetCellRange(i, vOthFldCaption)
        '    rg.Style = vsfHdr.Styles("WHITE")
        '    If vsfHdr.GetData(i, vOthFldBlnReadOnly) = "Y" Then
        '        rg.Style = vsfHdr.Styles("RO")
        '    End If
        'Next
    End Sub

    Private Sub txtQty_KeyPress(sender As System.Object, e As System.Windows.Forms.KeyPressEventArgs) Handles txtQty.KeyPress
        If e.KeyChar <> ControlChars.Back Then
            e.Handled = ("0123456789".IndexOf(e.KeyChar) = -1)
        End If
    End Sub

    Private Sub txtDocNo_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles txtDocNo.KeyPress, txtItemCode.KeyPress, txtNo.KeyPress
        If e.KeyChar = "'" Then
            e.Handled = True
        End If
    End Sub

    Private Function isPriceZero(ByVal argItemID As Long) As Boolean
        isPriceZero = True
        Dim intPrice% = 0, strItemName$ = "", strPriceCatName$ = ""

        intPrice = clsSvt.getDataLongValue("Select ItemRate from MstItemPriceList where ItemID = " & argItemID & _
                                                       " and PriceCatID = " & intPriceCatID, 0, False)
        strItemName = clsSvt.getDataStringValue("SELECT ItemName FROM MstItem WHERE ItemID = " & argItemID, "", False)
        strPriceCatName = clsSvt.getDataStringValue("SELECT PriceCatCode FROM MstPriceCat WHERE PriceCatID = " & intPriceCatID, "", False)

        'If intPrice = 0 Then
        '    txtItemCode.Text = ""
        '    lblItemName.Text = ""

        '    MsgBox("Price not found for item: '" & strItemName & "' in price category: '" & strPriceCatName & "'", vbInformation, Me.Text)
        '    If MsgBox("Do you want to enter the price now?", vbQuestion + vbYesNo + vbDefaultButton2, Me.Text) = MsgBoxResult.Yes Then
        '        Dim frmPriceList As New frmMstPLItems(strPriceCatName, argItemID.ToString())
        '        With frmPriceList
        '            .ShowDialog()
        '        End With
        '        intPrice = clsSvt.getDataLongValue("Select ItemRate from MstItemPriceList where ItemID = " & argItemID & _
        '                                             " and PriceCatID = " & intPriceCatID, 0, False)

        '        If intPrice > 0 Then
        '            isPriceZero = False
        '        End If
        '    Else
        '        Exit Function
        '    End If
        'Else
        '    txtItemCode.Focus()
        '    isPriceZero = False
        'End If
    End Function

    Private Sub txtItemCode_TextChanged(sender As Object, e As System.EventArgs) Handles txtItemCode.TextChanged
        If Trim(txtItemCode.Text) <> "" Then
            txtQty.Enabled = True
            btnAddItem.Enabled = True
        Else
            txtQty.Enabled = False
            btnAddItem.Enabled = False
        End If
    End Sub

    Private Sub cboMenuGroup_TextChanged(sender As Object, e As System.EventArgs) Handles cboMenuGroup.TextChanged
        If Trim(cboMenuGroup.Text) = "" Then
            vsfMenu.Rows.Count = 1
        End If
    End Sub

    Private Sub btnAlias_Click(sender As Object, e As System.EventArgs) Handles btnAlias.Click
        Dim frmAlias As New frmUtlAlias

        With frmAlias
            .MdiParent = frmMain
            .Show()
        End With
    End Sub

    Private Sub SplitContainer1_Panel1_Paint(sender As System.Object, e As System.Windows.Forms.PaintEventArgs) Handles SplitContainer1.Panel1.Paint

    End Sub

    Private Sub SplitContainer1_SplitterMoved(sender As Object, e As System.Windows.Forms.SplitterEventArgs) Handles SplitContainer1.SplitterMoved
        If SplitContainer1.SplitterDistance < 250 Then
            SplitContainer1.SplitterDistance = 250
        End If
        If SplitContainer1.SplitterDistance > 950 Then
            SplitContainer1.SplitterDistance = 950
        End If
    End Sub

    Private Sub txtTblCode_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtTblCode.GotFocus
        With txtTblCode
            .SelectAll()
        End With
    End Sub

    Private Sub txtTblCode_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles txtTblCode.KeyPress, txtItemCode.KeyPress, txtQty.KeyPress
        If e.KeyChar = "'" Then
            e.Handled = True
        End If
    End Sub

    Private Sub btnDelvryAdd_ClickButtonArea(Sender As Object, e As MouseEventArgs) Handles btnDelvryAdd.ClickButtonArea

    End Sub

    Private Sub txtTblCode_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtTblCode.KeyUp
        Dim blnFound As Boolean = False
        If e.KeyCode = Keys.Enter Then
            If txtTblCode.Text = "" Then Exit Sub
            If txtTblCode.Text <> "" Then txtTblCode.Focus()
        End If

        If e.KeyCode = Keys.Down Then
            Dim frmKB As New frmUtlKeyBoard
            Dim strSQL As String = ""

            With frmKB
                strSQL = "Select TblID, Tblcode, TblGroup, Status from MstTbl "
                strSQL = strSQL & " where Inactive  = 'N' and TblId > 0"
                strSQL = strSQL & " and QuickServiceTable = 'Y'"
                strSQL = strSQL & " and Status <> 'P'"
                'If BLNCOUNTERWISETABLES Then
                '    strSQL = strSQL & " AND TBLGROUP IN (SELECT TBLGROUP FROM MSTSALESCOUNTERDTL WHERE SALESCOUNTERID = " & intSalesCounter & ")"
                'End If
                strSQL = strSQL & " order by TblCode"
                .ShowUtlKeyboard(IIf(gBlnTableAlphaNumeric, KeyboardType.AlphaNumeric, KeyboardType.NumericOnly), True, strSQL, txtTblCode, 1, 0, 1)
                .ShowDialog()
                If Trim(txtTblCode.Text) <> "" Then
                    GoTo fromKB
                End If
            End With
        End If

        If txtTblCode.Text = "" Then Exit Sub

        If e.KeyCode = Keys.Enter Then
fromKB:
            If Trim(txtTblCode.Text) <> "" Then
                intTblID = clsSvt.getDataLongValue("Select TblID from MstTbl where Tblcode = '" & Trim(txtTblCode.Text) & "'", 0, blnFound)
                If Not blnFound Then
                    MsgBox("Table not found!!", vbInformation, Me.Text)
                    txtTblCode.Text = ""
                    Exit Sub
                End If
                Call showbtns(True)
                intPriceCatID = clsSvt.getDataLongValue("Select PriceCatID from MstTbl Where TblID = " & intTblID, 0, False)
                txtItemCode.Focus()
            End If
        End If
    End Sub

    Private Sub frmTrnQuickBillNonTouch_Shown(sender As Object, e As System.EventArgs) Handles Me.Shown
        Call initFormVsfDet(Me, Me.Text, vsfDet, "Detail", "", SplitContainer1) 'Satish
    End Sub

    'Private Sub btnDelvryAdd_ClickButtonArea(Sender As System.Object, e As System.Windows.Forms.MouseEventArgs) Handles btnDelvryAdd.ClickButtonArea
    '    If Trim(txtTblCode.Text) = "" Then Exit Sub
    '    Dim frmUtlDelvryAddress As New frmUtlDelvryAddress
    '    With frmUtlDelvryAddress
    '        .argFrm = Me
    '        .blnTouch = False
    '        .ShowDialog()
    '    End With
    'End Sub

End Class