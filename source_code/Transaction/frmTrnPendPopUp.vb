Imports C1.Win.C1FlexGrid

Public Class frmTrnPendPopUp
    Dim clsSVT As New svtLibrary
    Dim dtSearch As DataTable
    Dim dvSearch As DataView

    Const intColPartyCtrlBill = 0
    Const intColPartyCtrlShip = 1

    Const intRowPartyCtrlID = 1
    Const intRowPartyCtrlName = 2
    Const intRowPartyCtrlAdd1 = 3
    Const intRowPartyCtrlAdd2 = 4
    Const intRowPartyCtrlAdd3 = 5
    Const intRowPartyCtrlAdd4 = 6
    Const intRowPartyCtrlCity = 7
    Const intRowPartyCtrlPincode = 8
    Const intRowPartyCtrlState = 9
    Const intRowPartyCtrlCountry = 10
    Const intRowPartyCtrlCName = 11
    Const intRowPartyCtrlCMob = 12
    Const intRowPartyCtrlCDesig = 13
    Const intRowPartyCtrlCEmail = 14

    Const vDetSlNo = 0
    Const vDetItemID = 1
    Const vDetItemSlNo = 2
    Const vDetItemCode = 3
    Const vDetItemName = 4
    Const vDetUOM = 5
    Const vDetShortClosed = 6
    Const vDetShortClosedDT = 7
    Const vDetShortClosedBy = 8
    Const vDetShortClosedQty = 9
    Const vDetQOH = 10
    Const vDetSrcDocID = 11
    Const vDetSrcDocNo = 12
    Const vDetSrcItemSlNo = 13
    Const vDetSrcDocQty = 14
    Const vDetCumQty = 15
    Const vDetPendQty = 16
    Const vDetConvFact = 17
    Public vDetDocQty As Int32 = 0
    Dim intDetCols As Int32 = 19

    Const vVwPartyCode = 0
    Const vVwPartyName = 1
    Const vVwSRCDocNo = 2
    Const vVWSRCDocId = 3
    Const vVwSRCDocDate = 4
    Const vVWSRCItemSlNo = 5
    Const vVWItemID = 6
    Const vVWItemCode = 7
    Const vVwItemName = 8
    Const vVwItemGroupName = 9
    Const vVwUOMCode = 10
    Const vVwSRCDocQty = 11
    Const vVWCumDocQty = 12
    Const vVwPendQty = 13
    Const vVwConvFact = 14
    Const vVwQOH = 15
    Const vVwDocQty = 16
    Const vVwRowID = 17

    Dim vsfDetCalled As C1FlexGrid
    Dim intPartyId As Integer
    Dim intTrnDocID As Integer
    Dim intTrnID As Integer
    Dim objForm As Form
    Dim argObj As Object

    Dim strCurDocShForm As String
    Dim strSrcDocShForm As String

    Dim strDocQtyFldName As String
    Dim strDocRateFldName As String
    Dim strDocAmtFldName As String
    Dim blnRateColReq As Boolean
    Dim strDestSHForm As String

    Dim strLinkedFromHdrTblName As String
    Dim strLinkedFromDtlTblName As String
    Dim strLinkedFromDocNoFldName As String
    Dim strLinkedFromDocQtyFldName As String
    Dim strLinkedFromDocRateFldName As String
    Dim strLinkedFromDocDateFldName As String

    Dim strPartyShForm As String
    Dim strPMstTblName As String = ""
    Dim strPMstFldCode As String = ""
    Dim strPMstFldName As String = ""
    Dim strPMstFldID As String = ""

    Dim strItemShForm As String
    Dim strIMstTblName As String = ""
    Dim strIMstFldCode As String = ""
    Dim strIMstFldName As String = ""
    Dim strIMstFldID As String = ""

    Dim blnPendParty As Boolean
    Dim blnPendDocNo As Boolean
    Dim blnPendSingleDoc As Boolean
    Dim blnPartyPresent As Boolean
    Dim blnItemPresent As Boolean
    Dim blnAuthReq As Boolean
    Dim blnQOH As Boolean

    Dim strPendingQryFlds As String = ""
    Dim strPendingQryTbls As String = ""
    Dim strPendingQryCond As String = ""
    Dim strSQL As String
    Dim blnMultiPendingDoc As Boolean
    Dim strLinkDocAuthReq As String

    Public Sub fillPendQry(argTRNDOCID As Integer, argDocID As Long, argPartyID As Long, argVsfDet As C1FlexGrid, ByVal argIntColQty As Int32, Optional argForm As Form = Nothing)
        vsfPend.Rows.Count = 1

        Dim i As Integer = 0, j As Integer, intCol As Integer
        vDetDocQty = argIntColQty
        vsfDetCalled = argVsfDet
        intPartyId = argPartyID
        intTrnDocID = argTRNDOCID
        intTrnID = argDocID
        objForm = argForm
        argObj = argForm
        Call getUtlTrnData(argTRNDOCID)
        strSQL = getPendQry(argDocID, argPartyID)

        strSQL = getItemSearchGridQuery(strSQL)
        Call showWaitCursor()
        dtSearch = clsSVT.getDATATable(strSQL)
        dvSearch = New DataView(dtSearch)

        vsfView.DataSource = dvSearch
        Call showNormalCursor()
        vsfSearch.Cols.Count = vsfView.Cols.Count
        'Call SetHideCol(vsfView, True, vViewCustID, vViewCustBOMID)

        btnSelFull.Text = "Select All Items in " & strSrcDocShForm
        btnRemoveAll.Text = "Remove All Items of selected " & strSrcDocShForm
        lblSelItemsDoc.Text = "No of Items in selected " & strSrcDocShForm
        chkPendQty.Checked = True
        With vsfPend
            .Cols.Count = vsfView.Cols.Count
            .Rows.Count = 1
            For i = 0 To .Cols.Count - 1
                .SetData(0, i, vsfView.GetData(0, i))
            Next
            For i = 1 To argVsfDet.Rows.Count - 1
                .Rows.Count = .Rows.Count + 1
                For j = 1 To vsfView.Rows.Count - 1
                    If Val(vsfView.GetData(j, vVWSRCDocId)) = Val(argVsfDet.GetData(i, vDetSrcDocID)) _
                                And Val(vsfView.GetData(j, vVWItemID)) = Val(argVsfDet.GetData(i, vDetItemID)) Then
                        For intCol = 0 To vsfView.Cols.Count - 1
                            .SetData(.Rows.Count - 1, intCol, vsfView.GetData(j, intCol))
                        Next

                        Exit For
                    End If
                Next
                .SetData(.Rows.Count - 1, vVwDocQty, argVsfDet.GetData(i, vDetDocQty))
            Next
        End With
        Call HideCols()

        txtNoofRows.Text = vsfPend.Rows.Count - 1 & "/" & vsfView.Rows.Count - 1
    End Sub

    Private Sub HideCols()
        With vsfView
            vsfView.Cols(vVWSRCDocId).Visible = False
            vsfView.Cols(vVWItemID).Visible = False
            vsfView.Cols(vVWSRCItemSlNo).Visible = False
            vsfView.Cols(vVwRowID).Visible = False

            If Not blnItemPresent Then
                vsfView.Cols(vVWItemCode).Visible = False
                vsfView.Cols(vVwItemGroupName).Visible = False
                vsfView.Cols(vVwItemName).Visible = False
                vsfView.Cols(vVwUOMCode).Visible = False
            End If

            If Not blnPendDocNo Then
                vsfView.Cols(vVwPartyCode).Visible = False
                vsfView.Cols(vVwPartyName).Visible = False
            End If

            vsfPend.Cols(vVWSRCDocId).Visible = False
            vsfPend.Cols(vVWItemID).Visible = False
            vsfPend.Cols(vVWSRCItemSlNo).Visible = False
            vsfPend.Cols(vVwRowID).Visible = False


            If Not blnItemPresent Then
                vsfPend.Cols(vVWItemCode).Visible = False
                vsfPend.Cols(vVwItemGroupName).Visible = False
                vsfPend.Cols(vVwItemName).Visible = False
                vsfPend.Cols(vVwUOMCode).Visible = False
            End If

            If Not blnPendDocNo Then
                vsfPend.Cols(vVwPartyCode).Visible = False
                vsfPend.Cols(vVwPartyName).Visible = False
            End If
        End With
        Call syncGridCols()
    End Sub

    Private Sub getUtlTrnData(ByVal argDocID As Integer)
        Dim rsDummy As oledb.oledbDataReader

        strSQL = "Select * from UtlTransactions where DocID = " & argDocID
        rsDummy = clsSVT.getDataReaderFromSQL(strSQL)

        With rsDummy
            If .HasRows Then
                .Read()
                strCurDocShForm = GetVal(!DocShForm, "")
                strSrcDocShForm = GetVal(!LinkedFROMDocShForm, "")


                strLinkedFromHdrTblName = "TrnHdr" & GetVal(!LinkedFROMDocShForm, "")
                strLinkedFromDtlTblName = "TrnDtl" & GetVal(!LinkedFROMDocShForm, "")
                strLinkedFromDocNoFldName = GetVal(!LinkedFROMDocShForm, "") & "No"
                strLinkedFromDocQtyFldName = GetVal(!LinkedFROMDocShForm, "") & "Qty"
                strLinkedFromDocRateFldName = GetVal(!LinkedFROMDocShForm, "") & "Rate"
                strLinkedFromDocDateFldName = GetVal(!LinkedFROMDocShForm, "") & "Date"
                strLinkDocAuthReq = clsSVT.getDataStringValue("Select AuthReq From UtlTransactions where DocShForm = '" & strSrcDocShForm & "'", "N", False)
                Me.Text = "Select Pending " & clsSVT.getDataStringValue("Select DocName From UtlTransactions where DocShForm = '" & strSrcDocShForm & "'", "", False)

                strPendingQryFlds = GetVal(!PendingQryFlds, "")
                strPendingQryTbls = GetVal(!PendingQryTbls, "")
                strPendingQryCond = GetVal(!PendingQryCond, "")

                strPartyShForm = clsSVT.getDataStringValue("Select DocShForm From UtlTransactions where DocID = " & GetVal(!MstDocIDParty, 0), "", False)
                strPMstTblName = "Mst" & strPartyShForm
                strPMstFldID = strPartyShForm & "ID"
                strPMstFldCode = strPartyShForm & "Code"
                strPMstFldName = strPartyShForm & "Name"

                strItemShForm = clsSVT.getDataStringValue("Select DocShForm From UtlTransactions where DocID = " & GetVal(!MstDocIDItem, 0), "", False)
                strIMstTblName = "Mst" & strItemShForm
                strIMstFldID = strItemShForm & "ID"
                strIMstFldCode = strItemShForm & "Code"
                strIMstFldName = strItemShForm & "Name"

                If strItemShForm <> "" Then blnItemPresent = True
                If strPartyShForm <> "" Then blnPartyPresent = True

                blnPendParty = IIf(GetVal(!LinkedPendingParty, "N") = "Y", True, False)
                blnPendDocNo = IIf(GetVal(!LinkedPendingDocNo, "N") = "Y", True, False)
                blnPendSingleDoc = IIf(GetVal(!LinkedPendingSingleDoc, "N") = "Y", True, False)

                Dim strAuth As String
                strAuth = clsSVT.getDataStringValue("Select AuthReq from UtlTransactions where DocShForm = '" & strSrcDocShForm & "'", "", False)
                If strAuth = "Y" Then
                    blnAuthReq = True
                End If

                If UCase(GetVal(!AffectsStock, "")) = "ADD" Or UCase(GetVal(!AffectsStock, "")) = "REDUCE" Then
                    blnQOH = True
                End If

            End If
            .Close()
        End With

        If clsSVT.getDataLongValue("Select Count(*) From UtlTransCum Where DocID=" & argDocID, 0, False) > 1 Then
            blnMultiPendingDoc = True
        End If
    End Sub

    Private Function getPendQry(argDocID As Long, argPartyID As Long) As String
        Dim strSQL As String
        strSQL = "select "
        'If Pending Doc No wise then
        If blnPendParty Then
            If blnPartyPresent Then
                strSQL &= strPMstFldCode & "," & strPMstFldName & ","
            Else
                strSQL &= " PartyName as " & strPMstFldCode & ",PartyName as " & strPMstFldName & ","
            End If
        Else
            strSQL &= " '' as PartyCode,'' as PartyName,"
        End If

        'Fixed Fields DocNo, DocID, DocDate, ItemSlno
        strSQL &= strSrcDocShForm & "No, d." & strSrcDocShForm & "ID, " & strSrcDocShForm & "Date, d.ItemSlno,"


        'If Item Present then ItemID, ItemCode, ItemName, ItemGroupCode, UOMCode
        'Else All column created with Empty Value
        If blnItemPresent Then
            strSQL &= " d." & strIMstFldID & ", " & strIMstFldCode & ", " & strIMstFldName & ", ItemGroupCode, UOMCode, "
        Else
            strSQL &= " 0 as " & strIMstFldID & ", '' as " & strIMstFldCode & ", '' as " & strIMstFldName & ", '' as ItemGroupCode, '' as UOMCode, "
        End If

        'Fixed Fields LinkDocQty, CumQty, PendQty
        strSQL &= " d." & strSrcDocShForm & "Qty as " & strSrcDocShForm & "Qty, " & strCurDocShForm & "CumQty, d." & strSrcDocShForm & "Qty - IsNull(ShortClosedQty,0) - ISNULL(" & strCurDocShForm & "CumQty,0) + "
        If argDocID > 0 Then
            strSQL &= "isnull((select " & strCurDocShForm & "QTY from TrnDtl" & strCurDocShForm & " id where id." & strSrcDocShForm & "ID = d." & strSrcDocShForm & "ID and id.ItemSlno=d.ItemSlno and id." & strCurDocShForm & "ID = " & argDocID
            If blnItemPresent Then
                strSQL &= " and id." & strIMstFldID & " = d." & strIMstFldID
            End If
            strSQL &= "),0) "
        Else
            strSQL &= "0"
        End If
        strSQL &= " as PendQty, "

        'If Item Present then take Conversion Factor
        If blnItemPresent Then
            strSQL &= " ConvFact, "
        Else
            strSQL &= " 1 as ConvFact, "
        End If

        'If Stock Affects then reterive QOH values
        If blnQOH Then
            strSQL &= " IsNUll((Select sum(CLBal) from TrnQOH where TrnQOH.ItemID=d.ItemID),0) + "
            If argDocID > 0 Then
                strSQL &= "isnull((select " & strCurDocShForm & "QTY from TrnDtl" & strCurDocShForm & " id where id." & strSrcDocShForm & "ID = d." & strSrcDocShForm & "ID and id.ItemSlno=d.ItemSlno and id." & strCurDocShForm & "ID = " & argDocID
                If blnItemPresent Then
                    strSQL &= " and id." & strIMstFldID & " = d." & strIMstFldID
                End If
                strSQL &= "),0) "
            Else
                strSQL &= " 0 "
            End If
        Else
            strSQL &= " 0 "
        End If
        strSQL &= " as QOH, "

        'Fixed Columns DocQty
        strSQL &= " 0 as DocQty,"

        'Key Combination
        'If blnPendParty OrElse blnPendSingleDoc Then
        strSQL &= " Convert(varchar,d." & strSrcDocShForm & "ID) + '|' + convert(varchar,d.ItemSlno) "
        'End If

        'If blnPendDocNo Then
        '    If blnPartyPresent Then
        '        strSQL &= strPMstFldCode & " + '|' + Convert(varchar, d." & strSrcDocShForm & "ID) + '|' convert(varchar,d.ItemSlno) "
        '    Else
        '        strSQL &= "PartyName + '|' + Convert(varchar, d." & strSrcDocShForm & "ID) + '|' convert(varchar,d.ItemSlno) "
        '    End If
        'End If

        If blnItemPresent Then
            strSQL &= " + '|' + Convert(varchar,d." & strIMstFldID & ") "
        End If

        strSQL &= " as ItemRowID"

        'If Additional fields available in utltransactions table
        If strPendingQryFlds <> "" Then
            strSQL &= ", " & strPendingQryFlds
        End If

        'Fixed Tables 
        strSQL &= " from "
        strSQL &= strLinkedFromHdrTblName & " h," & strLinkedFromDtlTblName & " d "

        'If Item Present take Item Master, UOM Master, Item wise UOM Master table
        If blnItemPresent Then
            strSQL &= ", " & strIMstTblName & " i, MstItemGroup g, MstUOM u, MstItemUOM iu"
        End If

        'If party Present
        If blnPendParty AndAlso blnPartyPresent Then
            strSQL &= "," & strPMstTblName & " p"
        End If

        'If Additional Tables available in utltransactions table
        If strPendingQryTbls <> "" Then
            strSQL &= "," & strPendingQryTbls
        End If

        'Fixed Where Condition
        strSQL &= " where h." & strSrcDocShForm & "ID = d." & strSrcDocShForm & "ID"

        'If additional condition available in utltransactions table
        If strPendingQryCond <> "" Then
            strSQL &= " and " & strPendingQryCond
        End If

        If strLinkDocAuthReq = "Y" Then
            strSQL &= " and h.Authorised='Y' "
        End If

        If blnMultiPendingDoc Then
            strSQL &= " And d.ShortClosed = 'N'"
        End If

        'If item Present
        If blnItemPresent Then
            strSQL &= " and i.ItemGroupID = g.ItemGroupID"
            strSQL &= " and d.UOMId = u.UOMId"
            strSQL &= " and d." & strIMstFldID & " = i." & strIMstFldID
            strSQL &= " and d." & strIMstFldID & " = iu." & strIMstFldID
            strSQL &= " and u.UOMID = iu.UOMID"
        End If

        'If Party Present
        If blnPendParty AndAlso blnPartyPresent Then
            strSQL &= " and h." & strPMstFldID & " = p." & strPMstFldID
            If argPartyID > 0 Then strSQL &= " and p." & strPMstFldID & " = " & argPartyID
        End If

        'Fixed condition
        strSQL &= " and d." & strSrcDocShForm & "Qty - IsNull(ShortClosedQty,0) - ISNULL(" & strCurDocShForm & "CumQty,0) + "
        If argDocID > 0 Then
            strSQL &= "isnull((select " & strCurDocShForm & "QTY from TrnDtl" & strCurDocShForm & " id where id." & strSrcDocShForm & "ID = d." & strSrcDocShForm & "ID and id.ItemSlno=d.ItemSlno and id." & strCurDocShForm & "ID = " & argDocID
            If blnItemPresent Then
                strSQL &= " and id." & strIMstFldID & " = d." & strIMstFldID
            End If
            strSQL &= "),0) "
        Else
            strSQL &= "0"
        End If
        strSQL &= " > 0 "

        strSQL &= " order by " & strSrcDocShForm & "Date, " & strSrcDocShForm & "No, ItemSlno"

        If blnItemPresent Then
            strSQL &= ", ItemCode, ItemName"
        End If
        strSQL = Replace(strSQL, "False", "")

        getPendQry = strSQL

    End Function

    Private Sub vsfView_BeforeEdit(sender As System.Object, e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfView.BeforeEdit
        e.Cancel = True
    End Sub

    Private Function applyFilterFromGrid() As Boolean
        Dim StrQry As String, strValue As String
        Dim i As Integer

        applyFilterFromGrid = False
        StrQry = ""
        With vsfSearch
            For i = 0 To .Cols.Count - 1
                strValue = ""
                If i = .ColSel Then
                    If .Editor.Text <> "" Then
                        strValue = Trim(.Editor.Text)
                    End If
                Else
                    If .GetData(0, i) <> "" Then
                        strValue = Trim(.GetData(0, i))
                    End If
                End If
                If strValue <> "" Then
                    If StrQry <> "" Then StrQry = StrQry & " and "
                    Select Case gDBTypeCurrent
                        Case gDbTypeAccess
                            StrQry = StrQry & vsfView.GetData(0, i) & " like '*" & strValue & "*'"
                        Case Else
                            If vsfView.Cols(i).DataType = GetType(Decimal) Or vsfView.Cols(i).DataType = GetType(Integer) Or vsfView.Cols(i).DataType = GetType(Date) Then
                                Try
                                    StrQry = StrQry & "Convert(" & vsfView.GetData(0, i) & ",'System.String')" & " like '" & strValue & "*'"
                                Catch ex As Exception
                                    dvSearch.RowFilter = "1 <> 1"
                                    Exit Function
                                End Try
                            Else
                                StrQry = StrQry & vsfView.GetData(0, i) & " like '%" & strValue & "%'"
                            End If
                    End Select
                End If
            Next i
        End With

        dvSearch.RowFilter = StrQry
        Call syncGridCols()
        applyFilterFromGrid = True
    End Function

    Private Sub syncGridCols()
        Dim i As Integer
        With vsfView

            vsfSearch.Cols.Count = vsfView.Cols.Count
            For i = 0 To .Cols.Count - 1
                vsfSearch.Cols(i).Width = vsfView.Cols(i).Width
                vsfSearch.Cols(i).Visible = vsfView.Cols(i).Visible
            Next i
        End With
    End Sub

    Private Sub vsfSearch_RowColChange(sender As System.Object, e As System.EventArgs) Handles vsfSearch.RowColChange
        vsfView.Col = vsfSearch.Col
        vsfView.Refresh()
    End Sub

    Private Sub vsfSearch_AfterScroll(sender As System.Object, e As C1.Win.C1FlexGrid.RangeEventArgs) Handles vsfSearch.AfterScroll
        vsfSearch.ScrollPosition = vsfView.ScrollPosition
    End Sub

    Private Sub vsfSearch_ChangeEdit(sender As System.Object, e As System.EventArgs) Handles vsfSearch.ChangeEdit
        Call applyFilterFromGrid()
    End Sub

    Private Sub vsfView_AfterScroll(sender As System.Object, e As C1.Win.C1FlexGrid.RangeEventArgs) Handles vsfView.AfterScroll
        vsfSearch.ScrollPosition = vsfView.ScrollPosition
    End Sub

    Private Sub vsfView_AfterResizeColumn(sender As System.Object, e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfView.AfterResizeColumn
        Call syncGridCols()
    End Sub

    Private Sub btnCancel_Click(sender As System.Object, e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub btnOK_Click(sender As System.Object, e As System.EventArgs) Handles btnOK.Click

        If (vsfPend.Rows.Count <= 1) Then
            MsgBox("Pending List is Empty", MsgBoxStyle.Information, Me.Text)
            Exit Sub
        End If
        If GetVal(vsfPend.GetData(1, vVwSRCDocNo)) = "" Then
            MsgBox("Pending List is Empty", MsgBoxStyle.Information, Me.Text)
            Exit Sub
        End If

        Dim i, j, lngDOCID As Integer
        ' Dim lngGraceQty As Long
        Dim bFound As Boolean

        With vsfDetCalled
            For i = 1 To vsfPend.Rows.Count - 1
                lngDOCID = Val(vsfPend.GetData(i, vVWSRCDocId))
                If lngDOCID = 0 Then Exit For
                For j = 1 To .Rows.Count - 1
                    bFound = False
                    If blnItemPresent Then
                        If lngDOCID = .GetData(j, vDetSrcDocID) _
                            And vsfPend.GetData(i, vVWItemID) = .GetData(j, vDetItemID) And vsfPend.GetData(i, vVWSRCItemSlNo) = .GetData(j, vDetSrcItemSlNo) Then
                            bFound = True
                            Exit For
                        End If
                    Else
                        If lngDOCID = .GetData(j, vDetSrcDocID) _
                            And vsfPend.GetData(i, vVWSRCItemSlNo) = .GetData(j, vDetSrcItemSlNo) Then
                            bFound = True
                            Exit For
                        End If
                    End If

                Next

                If Not bFound Then
                    .Rows.Count += 1
                    .SetData(.Rows.Count - 1, vDetSlNo, .Rows.Count - 1)
                    .SetData(.Rows.Count - 1, vDetSrcDocID, vsfPend.GetData(i, vVWSRCDocId))
                    .SetData(.Rows.Count - 1, vDetItemSlNo, getmaxNo())
                    .SetData(.Rows.Count - 1, vDetSrcItemSlNo, vsfPend.GetData(i, vVWSRCItemSlNo))
                    .SetData(.Rows.Count - 1, vDetItemID, vsfPend.GetData(i, vVWItemID))
                    .SetData(.Rows.Count - 1, vDetItemCode, vsfPend.GetData(i, vVWItemCode))
                    .SetData(.Rows.Count - 1, vDetItemName, vsfPend.GetData(i, vVwItemName))
                    .SetData(.Rows.Count - 1, vDetSrcDocNo, vsfPend.GetData(i, vVwSRCDocNo))
                    .SetData(.Rows.Count - 1, vDetUOM, vsfPend.GetData(i, vVwUOMCode))
                    .SetData(.Rows.Count - 1, vDetSrcDocQty, vsfPend.GetData(i, vVwSRCDocQty))
                    .SetData(.Rows.Count - 1, vDetCumQty, vsfPend.GetData(i, vVWCumDocQty))

                    ' lngGraceQty = clsSVT.getDataLongValue("Select GracePer From MstCUST Where CUSTID = " & iSuppId, 0, False)
                    .SetData(.Rows.Count - 1, vDetPendQty, vsfPend.GetData(i, vVwPendQty))

                    If chkPendQty.Checked Then
                        .SetData(.Rows.Count - 1, vDetDocQty, vsfPend.GetData(i, vVwPendQty))
                    Else
                        .SetData(.Rows.Count - 1, vDetDocQty, 0)
                    End If
                    .SetData(.Rows.Count - 1, vDetConvFact, vsfPend.GetData(i, vVwConvFact))
                    .SetData(.Rows.Count - 1, vDetQOH, vsfPend.GetData(i, vVwQOH))
                End If

            Next



            Call RemoveEmptVSFRows(vsfDetCalled, 1, vDetItemSlNo)
            Call UpdateCSVField()
            'BeginStm:
            '            For i = 1 To .Rows.Count - 2
            '                lngDOCID = Val(.GetData(i, vDetSrcDocID))
            '                For j = 1 To vsfPend.Rows.Count - 1
            '                    bFound = False
            '                    If lngDOCID = vsfPend.GetData(j, vVWSRCDocId) And vsfPend.GetData(j, vVWItemID) = .GetData(i, vDetItemID) Then
            '                        bFound = True
            '                        Exit For
            '                    End If
            '                Next
            '                If Not bFound Then
            '                    .RemoveItem(i)
            '                    GoTo BeginStm
            '                End If
            '            Next
        End With
        'Call UpdateParty()
        Me.Close()
    End Sub

    Private Function getmaxNo() As Integer
        Dim i As Integer, intNo As Integer
        With vsfDetCalled
            intNo = 0
            For i = 1 To .Rows.Count - 1
                If Val(.GetData(i, vDetItemSlNo)) > intNo Then
                    intNo = Val(.GetData(i, vDetItemSlNo))
                End If
            Next i
        End With
        intNo = intNo + 1
        getmaxNo = intNo
    End Function

    Private Sub UpdateParty()
        If blnPendDocNo Then
            Dim intTrnID As Int32
            With vsfPend
                If .Rows.Count > 1 Then
                    intTrnID = Val(.GetData(1, vVWSRCDocId))
                End If
            End With

            If intTrnID > 0 Then
                Dim strSQL As String = "Select * from TrnHdr" & strSrcDocShForm & " Where " & strSrcDocShForm & "ID = " & intTrnID
                Dim rs As oledb.oledbDataReader
                rs = clsSVT.getDataReaderFromSQL(strSQL)
                If Not rs.HasRows Then
                    rs.Close()
                    Exit Sub
                End If
                rs.Read()

                With argObj.svtPCtrl
                    If blnPartyPresent Then
                        .setCode(clsSVT.getDataStringValue("Select " & strPMstFldCode & " from " & strPMstTblName & " where " & strPMstFldID & " = " & intTrnID, "", False))
                        .setGridData(intRowPartyCtrlID, intColPartyCtrlBill, Val(intTrnID))
                    Else
                        .setGridData(intRowPartyCtrlID, intColPartyCtrlBill, 0)
                        .setCode("")
                    End If


                    .setGridData(intRowPartyCtrlName, intColPartyCtrlBill, GetVal(rs.Item("PartyName"), ""))
                    .setGridData(intRowPartyCtrlAdd1, intColPartyCtrlBill, GetVal(rs.Item("PartyAdd1"), ""))
                    .setGridData(intRowPartyCtrlAdd2, intColPartyCtrlBill, GetVal(rs.Item("PartyAdd2"), ""))
                    .setGridData(intRowPartyCtrlAdd3, intColPartyCtrlBill, GetVal(rs.Item("PartyAdd3"), ""))
                    .setGridData(intRowPartyCtrlAdd4, intColPartyCtrlBill, GetVal(rs.Item("PartyAdd4"), ""))
                    .setGridData(intRowPartyCtrlCity, intColPartyCtrlBill, GetVal(rs.Item("PartyCity"), ""))
                    .setGridData(intRowPartyCtrlState, intColPartyCtrlBill, GetVal(rs.Item("PartyState"), ""))
                    .setGridData(intRowPartyCtrlPincode, intColPartyCtrlBill, GetVal(rs.Item("PartyPincode"), ""))
                    .setGridData(intRowPartyCtrlCountry, intColPartyCtrlBill, GetVal(rs.Item("PartyCountry"), ""))
                    .setGridData(intRowPartyCtrlCName, intColPartyCtrlBill, GetVal(rs.Item("PartyCName"), ""))
                    .setGridData(intRowPartyCtrlCMob, intColPartyCtrlBill, GetVal(rs.Item("PartyCMob"), ""))
                    .setGridData(intRowPartyCtrlCEmail, intColPartyCtrlBill, GetVal(rs.Item("PartyCEmail"), ""))
                    .setGridData(intRowPartyCtrlCDesig, intColPartyCtrlBill, GetVal(rs.Item("PartyCDesig"), ""))
                    .UpdateFromGrid()
                End With
            End If
        End If
    End Sub

    Private Sub UpdateCSVField()

        Dim argDsgrGrids = GetAllDsgrVsf(argObj, GetType(C1FlexGrid))

        For Each GridList As C1FlexGrid In argDsgrGrids
            With GridList
                If .Visible Then
                    For i As Int32 = 0 To .Rows.Count - 1
                        If GetVal(.GetData(i, vOthCSVFieldName), "") <> "" Then
                            Dim strCSField As String = UCase(.GetData(i, vOthCSVFieldName))
                            Dim strValue As String = ""
                            Dim strChkString As String = ""
                            Dim intCol As Int32 = -1
                            With vsfPend

                                For k As Int32 = 0 To .Cols.Count - 1
                                    If strCSField = UCase(GetVal(.GetData(0, k), "")) Then
                                        intCol = k
                                        Exit For
                                    End If
                                Next

                                If intCol = -1 Then Exit Sub

                                For k As Int32 = 1 To .Rows.Count - 1
                                    If strChkString = "" Then
                                        strChkString = .GetData(k, intCol)
                                        strValue = strChkString
                                    Else
                                        If strChkString <> .GetData(k, intCol) Then
                                            strChkString = .GetData(k, intCol)
                                            strValue &= "," & strChkString
                                        End If
                                    End If
                                Next

                                GridList.SetData(i, vOthFldDataValue, strValue)

                                '.AllowFiltering = True
                                'Dim FilterCol As C1.Win.C1FlexGrid.Column = .Cols(intCol)
                                'Dim cf As = New ColumnFilter(FilterCol)
                                'cf.ConditionFi
                                'For j As Int32 = .

                                'FilterCol.Filter = Filter.Getlo
                                'Dim cf = _filter.GetColumnFilter(c)
                                'cf.ValueFilter.Clear()
                                'Dim c1 = cf.ConditionFilter.Condition1
                                'c1.[Operator] = ConditionOperator.IsGreaterThanOrEqualTo
                                'c1.Parameter = 900.5
                                '_filter.Apply()

                                ' Dim vsfFilter As IC1ColumnFilter = New IC1ColumnFil

                                'For intRow As Int32 = 1 To .Rows.Count - 1

                                'Next
                            End With
                        End If
                    Next
                End If
            End With
        Next
    End Sub

    Private Sub vsfView_DoubleClick(sender As Object, e As System.EventArgs) Handles vsfView.DoubleClick
        If vsfView.Row <= 0 Then Exit Sub
        Dim i As Integer, intCol As Integer
        With vsfPend
            For i = 1 To .Rows.Count - 1
                If blnItemPresent Then
                    If .GetData(i, vVWSRCDocId) = vsfView.GetData(vsfView.Row, vVWSRCDocId) And
                        .GetData(i, vVWItemID) = vsfView.GetData(vsfView.Row, vVWItemID) And
                        .GetData(i, vVWSRCItemSlNo) = vsfView.GetData(vsfView.Row, vVWSRCItemSlNo) Then
                        MsgBox("Item Already selected!!", vbInformation, Me.Text)
                        Exit Sub
                    End If
                Else
                    If .GetData(i, vVWSRCDocId) = vsfView.GetData(vsfView.Row, vVWSRCDocId) And
                        .GetData(i, vVWSRCItemSlNo) = vsfView.GetData(vsfView.Row, vVWSRCItemSlNo) Then
                        MsgBox("Item Already selected!!", vbInformation, Me.Text)
                        Exit Sub
                    End If
                End If
            Next i
        End With

        With vsfView
            i = vsfPend.Rows.Count - 1
            If i = 0 Then
                vsfPend.Rows.Count = vsfPend.Rows.Count + 1
                i = vsfPend.Rows.Count - 1
            End If
            If Val(vsfPend.GetData(i, vVWSRCDocId)) > 0 Then
                vsfPend.Rows.Count = vsfPend.Rows.Count + 1
            End If
            i = vsfPend.Rows.Count - 1
            For intCol = 0 To .Cols.Count - 1
                vsfPend.SetData(i, intCol, .GetData(.Row, intCol))
            Next
        End With

        If vsfPend.Rows.Count > 1 Then
            vsfPend.TopRow = vsfPend.Rows.Count
            vsfPend.Row = vsfPend.Rows.Count - 1
            vsfPend.Focus()
        End If
        Call calQtyTot()
        txtNoofRows.Text = vsfPend.Rows.Count - 1 & "/" & vsfView.Rows.Count - 1
    End Sub

    Private Sub btnSelFull_Click(sender As System.Object, e As System.EventArgs) Handles btnSelFull.Click
        Dim lngSRCDocID As Long

        If vsfView.Row <= 0 Then Exit Sub
        Dim i As Integer, x As Integer, strItemRowID As String = "", introwfound As Int32
        lngSRCDocID = Val(vsfView.GetData(vsfView.Row, vVWSRCDocId))

        With vsfView
            For i = 1 To .Rows.Count - 1
                If Val(.GetData(i, vVWSRCDocId)) = lngSRCDocID Then
                    If blnItemPresent Then
                        strItemRowID = Val(vsfView.GetData(i, vVWSRCDocId)) & "|" & Val(vsfView.GetData(i, vVWSRCItemSlNo)) & "|" & Val(vsfView.GetData(i, vVWItemID))
                    Else
                        strItemRowID = Val(vsfView.GetData(i, vVWSRCDocId)) & "|" & Val(vsfView.GetData(i, vVWSRCItemSlNo))
                    End If

                    introwfound = vsfPend.FindRow(strItemRowID, 1, vVwRowID, False, True, True)

                    If introwfound <= 0 Then
                        x = vsfPend.Rows.Count - 1
                        If x = 0 Then
                            vsfPend.Rows.Count = vsfPend.Rows.Count + 1
                        End If
                        x = vsfPend.Rows.Count - 1
                        If Val(vsfPend.GetData(x, vVWSRCDocId)) > 0 Then
                            vsfPend.Rows.Count = vsfPend.Rows.Count + 1
                        End If
                        x = vsfPend.Rows.Count - 1
                        For intCol = 0 To .Cols.Count - 1
                            vsfPend.SetData(x, intCol, .GetData(i, intCol))
                        Next
                    End If
                End If
            Next i
        End With
        txtNoofRows.Text = vsfPend.Rows.Count - 1 & "/" & vsfView.Rows.Count - 1
        vsfPend.Row = vsfPend.Rows.Count - 1
        Call calQtyTot()
    End Sub

    Private Sub btnRemove_Click(sender As System.Object, e As System.EventArgs) Handles btnRemove.Click
        With vsfPend
            If .Row > 0 Then
                .RemoveItem(.Row)
                txtNoofRows.Text = vsfPend.Rows.Count - 1 & "/" & vsfView.Rows.Count - 1
            End If
        End With
    End Sub

    Private Sub btnRemoveAll_Click(sender As System.Object, e As System.EventArgs) Handles btnRemoveAll.Click
        Dim iRemPOID '', iRemItemID As Int32
        With vsfPend
            .Rows.Count = 1

            If .Row > 0 Then
                iRemPOID = vsfView.GetData(vsfView.Row, vVWSRCDocId)
                'iRemItemID = .GetData(.Row, vVwItemId)
                For iPendRow = 1 To .Rows.Count - 1
                    If iRemPOID = .GetData(iPendRow, vVWSRCDocId) Then  ''And iRemItemID = .GetData(iPendRow, vVwItemId)
                        vsfPend.RemoveItem(iPendRow)
                        iPendRow -= 1
                    End If
                    If iPendRow >= .Rows.Count - 1 Then
                        Exit For
                    End If
                Next
            End If
        End With
        txtNoofRows.Text = vsfPend.Rows.Count - 1 & "/" & vsfView.Rows.Count - 1
    End Sub

    Private Sub vsfPend_BeforeEdit(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfPend.BeforeEdit
        'If chkPendQty.Checked = True Then
        '    Select Case vsfPend.Col
        '        Case vVwDocQty
        '            e.Cancel = True
        '    End Select
        'Else
        '    Select Case vsfPend.Col
        '        Case vVwDocQty
        '            e.Cancel = False
        '    End Select
        'End If
        'commented by Madhu. Will not allow to edit Doc qty here as the validations cannot be done here. 
        e.Cancel = True
    End Sub

    Private Sub frmTrnPendPopUp_KeyUp(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyUp
        If e.KeyCode = Keys.F9 And e.Control Then
            '958, 579
            Call initFormSize(Me, Me.Tag, True, 922, 507, 0, 0)
            Call initFormVsfDet(Me, Me.Tag, vsfPend, "Pending")
            Call initFormVsfDet(Me, Me.Tag, vsfView, "View")
            'Call SetColWidthFromString(vsfDet, strDocStdDtlWidth)
            MsgBox("Restored Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
        If e.KeyCode = Keys.F8 And e.Control = True Then
            Call SaveActFormSettings(Me, Me.Tag)
            Call SaveActFormVsfDet(Me, Me.Tag, "Pending", vsfPend)
            Call SaveActFormVsfDet(Me, Me.Tag, "View", vsfView)
            'Call SaveItemComboFrameSettings(fraSearch, vsfView,  intDocID)
            MsgBox("Saved Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If

        If e.Control = False And e.Alt = True And e.KeyCode = Keys.E Then
            Call ExportToXL(vsfView)
        End If
        If e.Control = False And e.Alt = True And e.KeyCode = Keys.X Then
            Call ExportToXL(vsfPend)
        End If
    End Sub

    Private Sub frmTrnPending_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        Call initFormSize(Me, Me.Tag, False, 922, 507, 0, 0)
        Call initFormVsfDet(Me, Me.Text, vsfPend, "Pending")
        Call initFormVsfDet(Me, Me.Text, vsfView, "view")

        With vsfSearch
            If .Cols.Count >= 0 Then
                For i As Int32 = 0 To .Cols.Count - 1
                    If .Cols(i).Visible Then
                        .Col = i
                        .Focus()
                        Exit Sub
                    End If
                Next
            End If
        End With
    End Sub

    Private Sub calQtyTot()
        txtQtyTot.Text = ""
        lblPart.Text = ""
        Dim i As Integer, strItem As String
        Dim sngTot As Double
        With vsfPend
            If .Row <= 0 Then Exit Sub
            If .Rows.Count = 2 Then .Row = 1
            strItem = .GetData(.Row, vVWItemCode)
            lblPart.Text = "Qty Total for " & strItem
            For i = 1 To .Rows.Count - 1
                If strItem = .GetData(i, vVWItemCode) Then
                    sngTot = sngTot + GetVal(.GetData(i, vVwPendQty), 0)
                End If
            Next i
        End With
        txtQtyTot.Text = sngTot
    End Sub

    Private Sub vsfPend_Click(sender As Object, e As EventArgs) Handles vsfPend.Click
        If vsfPend.Rows.Count <= 1 Then Exit Sub
        Call calQtyTot()
    End Sub

    Private Sub btnExportXLVw_Click(sender As Object, e As EventArgs) Handles btnExportXLVw.Click
        Call ExportToXL(vsfView)
    End Sub

    Private Sub btnExportXLPend_Click(sender As Object, e As EventArgs) Handles btnExportXLPend.Click
        Call ExportToXL(vsfPend)
    End Sub

    Private Sub vsfView_Click(sender As Object, e As EventArgs) Handles vsfView.Click
        If vsfView.Rows.Count <= 1 Then Exit Sub
        With vsfView
            If .Row >= 1 Then
                Dim strDocNo As String = ""
                txtNoRowsDoc.Text = ""
                strDocNo = .GetData(.Row, vVwSRCDocNo)
                For i = 1 To .Rows.Count - 1
                    If strDocNo = .GetData(i, vVwSRCDocNo) Then
                        txtNoRowsDoc.Text = Val(txtNoRowsDoc.Text) + 1
                    End If
                Next i

            End If
        End With
    End Sub

    Private Sub frmTrnPendPopUp_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        vsfSearch.Focus()
        'With vsfSearch
        '    .Row = 0
        '    .Col = 0
        '    Call showWaitCursor()
        'End With
    End Sub

End Class