Imports C1.Win.C1FlexGrid

Public Class frmUtlInsertfromDoc
    Dim clsSVT As New svtLibrary
    Public intDocID As Integer
    Public argFrm As Object

    Dim strPartyShForm As String
    Dim strPartyTblName As String
    Dim strFldPartyID As String
    Dim strFldPartyCode As String
    Dim strFldPartyName As String
    Dim strPartyFilterCond As String
    Dim strPartyFlds As String

    Dim strItemTblName As String
    Dim strFldItemID As String
    Dim strFldItemSlno As String
    Dim strFldItemCode As String
    Dim strFldItemName As String
    Dim strItemFilterCond As String
    Dim strItemFlds As String
    Dim strItemShForm As String

    Public strMstTblName As String
    Public strMstFldCode As String
    Public strMstFldName As String
    Public strMstFldId As String

    Public strDocHdrTblName As String

    Dim blnShowPendingPartyWise As Boolean
    Dim blnShowPendingLoadDocNos As Boolean

    Dim strLoadFromHdrTblName As String
    Dim strLoadFromDtlTblName As String
    Dim strLoadDocShForm As String
    Dim strLoadFromDocNo As String
    Dim strLoadFromTrnID As String
    Dim intLoadFromDocID As Integer

    Public blnLoadItemGrid As Boolean
    Dim blnLinkedToDoc As Boolean
    Dim blnRateColReq As Boolean
    Public intItemDocID As Int32
    Public intPartyDocID As Int32


    Dim strSQL As String

    'declarations for detail grid
    'declarations for detail grid
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
    Const vDetCumQtyLinkedDoc = 15
    Const vDetPendQty = 16
    Const vDetConvFact = 17
    Const vDetDocCumQty = 18
    Public intDetCols As Int32 = 19
    Public intDetNextEntryCol As Int32 = -1

    Public intColQty As Int32 = -1
    Public intColrate As Int32 = -1
    Public intColStkQty As Int32 = -1
    Public strPartyCode As String

    Private Sub frmUtlInsertfromDoc_KeyUp(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyUp
        If e.KeyCode = Keys.F9 And e.Control Then
            Call initFormSize(Me, Me.Text, True, 269, 142, 0, 0)
            MsgBox("Restored Form Layout Successfully!!", vbInformation, Me.Text)
        End If
        If e.KeyCode = Keys.F8 And e.Control = True Then
            Call SaveActFormSettings(Me, Me.Text)
            MsgBox("Saved Form Layout Successfully!!", vbInformation, Me.Text)
        End If
    End Sub

    Private Sub frmUtlInsertfromDoc_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        cboInsertFromDocName.Refresh()
        cboInsertFromDocName.Text = ""
        cboInsertFromDocNo.Refresh()
        cboInsertFromDocNo.Text = ""
        lblDocNo.Text = "Doc No"

        If clsSVT.getDataStringValue("Select LoadPendingDocNos From UtlTransactions where DocID = " & intDocID, "N", False) = "Y" Then
            blnShowPendingLoadDocNos = True
        End If

        If clsSVT.getDataStringValue("Select LoadPendingPartyWise From UtlTransactions where DocID = " & intDocID, "N", False) = "Y" Then
            blnShowPendingPartyWise = True
        End If

        strSQL = "Select docName from UtlInsertFromDocs d, UtlTransactions t "
        strSQL &= " where d.DocID = " & intDocID
        strSQL &= " and t.DocID = d.InsertFromDocID order by 1"
        Call bindCombo(cboInsertFromDocName, "DocName", "DocName", strSQL)
        Call cboInsertFromDocName_Validated(sender, e)
        Call initFormSize(Me, Me.Text, False, 269, 142, 0, 0)
    End Sub

    Private Sub cboInsertFromDocName_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles cboInsertFromDocName.KeyPress
        If e.KeyChar = "'" Then e.Handled = True
    End Sub

    Private Sub cboInsertFromDocNo_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles cboInsertFromDocNo.KeyPress
        If e.KeyChar = "'" Then e.Handled = True
    End Sub

    Private Sub cboInsertFromDocName_Validated(sender As Object, e As System.EventArgs) Handles cboInsertFromDocName.Validated
        If Trim(cboInsertFromDocName.Text) = "" Then lblDocNo.Text = "Doc No"
        Call fillInsertFromcboData()
        cboInsertFromDocNo.Text = ""
    End Sub

    Private Sub fillInsertFromcboData()
        Dim strInsertFromDocShForm As String
        Dim strSQL As String
        If cboInsertFromDocName.Text = "" Then Exit Sub
        intLoadFromDocID = clsSVT.getDataLongValue("Select DocID from UtlTransactions Where docName = '" & Trim(cboInsertFromDocName.Text) & "'", 0, False)
        If intLoadFromDocID = 0 Then
            lblDocNo.Text = "Doc No"
            cboInsertFromDocNo.Refresh()
            btnInsert.Enabled = False
            MsgBox("Please select the valid Document type!!", MsgBoxStyle.Information, Me.Text)
            cboInsertFromDocName.Focus()
            Exit Sub
        End If
        strInsertFromDocShForm = clsSVT.getDataStringValue("Select DocShForm from UtlTransactions Where docName = '" & Trim(cboInsertFromDocName.Text) & "'", "", False)
        If strInsertFromDocShForm <> "" Then
            lblDocNo.Text = strInsertFromDocShForm & "NO"
            cboInsertFromDocNo.Enabled = True
            btnInsert.Enabled = True
        End If

        If intPartyDocID > 0 Then
            strPartyShForm = clsSVT.getDataStringValue("Select DocShForm From UtlTransactions where DocID = " & Val(intPartyDocID), "", False)
            strPartyTblName = "Mst" & strPartyShForm
            strFldPartyID = strPartyShForm & "ID"
            strFldPartyCode = strPartyShForm & "Code"
            strFldPartyName = strPartyShForm & "Name"
        End If

        If intItemDocID > 0 Then
            strItemShForm = clsSVT.getDataStringValue("Select DocShForm From UtlTransactions where DocID = " & Val(intItemDocID), "", False)
            strItemTblName = "Mst" & strItemShForm
            strFldItemID = strItemShForm & "ID"
            strFldItemSlno = strItemShForm & "Slno"
            strFldItemCode = strItemShForm & "Code"
            strFldItemName = strItemShForm & "Name"
        End If

        'cboInsertFromDocNo.DisplayMember = "DOCNo"                  'satish

        strLoadFromHdrTblName = "TrnHdr" & strInsertFromDocShForm
        strLoadFromDtlTblName = "TrnDtl" & strInsertFromDocShForm
        strLoadFromDocNo = strInsertFromDocShForm & "NO"
        strLoadFromTrnID = strInsertFromDocShForm & "ID"

        strLoadDocShForm = strInsertFromDocShForm

        strSQL = "Select " & strLoadFromDocNo & " as docNo"
        strSQL = strSQL & " from " & strLoadFromHdrTblName
        strSQL = strSQL & " where 1 = 1 "
        If blnShowPendingLoadDocNos Then
            strSQL = strSQL & " and " & strLoadFromDocNo
            strSQL = strSQL & " not in (Select InsertDocNo from "
            strSQL = strSQL & strDocHdrTblName
            strSQL = strSQL & " where InsertDOCid = "
            strSQL = strSQL & intLoadFromDocID & " "
            strSQL = strSQL & " and Cancelled = 'N')"
        End If
        If blnShowPendingPartyWise Then
            strSQL = strSQL & " and " & strFldPartyID & " = " & clsSVT.getDataLongValue("Select " & strFldPartyID & " From " & strPartyTblName & " Where " & strFldPartyCode & " = '" & strPartyCode & "'", 0, False)
        End If
        strSQL = strSQL & " order by 1"


        Call bindCombo(cboInsertFromDocNo, "DocNo", "DocNo", strSQL)
    End Sub

    Private Sub btnInsert_Click(sender As System.Object, e As System.EventArgs) Handles btnInsert.Click
        Dim blnFound As Boolean
        Dim strsql As String, i As Integer, x As Integer, f As Integer

        'argFrm.arginsertedNo = cboInsertFromDocNo.Text

        If Trim(cboInsertFromDocNo.Text) = "" Then Exit Sub
        Call clsSVT.getDataStringValue("Select " & strLoadFromDocNo & " from " & strLoadFromHdrTblName & " where " & strLoadFromDocNo & " = '" & Trim(cboInsertFromDocNo.Text) & "'", "", blnFound)
        If Not blnFound Then
            MsgBox("Invalid " & strLoadFromDocNo & "!!", vbInformation, Me.Text)
            Exit Sub
        End If

        Dim rsDummy As oledb.oledbDataReader
        Call ClearAddnlGrid(argFrm)
        Call fillVsfHFTDefValues(argFrm)


        'insert header flds
        strsql = getLoadQry("H")
        rsDummy = clsSVT.getDataReaderFromSQL(strsql)

        blnLoadItemGrid = True

        With rsDummy
            If .Read Then
                For f = 0 To .FieldCount - 1
                    blnFound = False
                    With argFrm.vsfhdr
                        For i = 0 To .Rows.count - 1
                            If UCase(Trim(.getdata(i, vOthFldCode))) = UCase(Trim(rsDummy.GetName(f))) Then
                                'if the field is of combo datatype then load data accordingly
                                '# A - 11/Jan/2008
                                If .getdata(i, vOthFldBlnComboData) = "Y" Then
                                    .setdata(i, vOthFldDataValue, clsSVT.getDataStringValue("Select " & Trim(.getdata(i, vOthFldCmbDisplayCols)) & " from " & .getdata(i, vOthFldCmbTblName) & " where " & .getdata(i, vOthFldCmbStorageFldName) & " = " & GetVal(rsDummy.GetValue(f), ""), "", False))
                                Else
                                    .setdata(i, vOthFldDataValue, GetVal(rsDummy.GetValue(f), ""))
                                End If

                                blnFound = True
                                Exit For
                            End If
                        Next i
                    End With
                    With argFrm.vsfTot
                        For i = 0 To .Rows.count - 1
                            If UCase(Trim(.getdata(i, vOthFldCode))) = UCase(Trim(rsDummy.GetName(f))) Then
                                'if the field is of combo datatype then load data accordingly
                                '# A - 11/Jan/2008
                                If .getdata(i, vOthFldBlnComboData) = "Y" Then
                                    .setdata(i, vOthFldDataValue, clsSVT.getDataStringValue("Select " & Trim(.getdata(i, vOthFldCmbDisplayCols)) & " from " & .getdata(i, vOthFldCmbTblName) & " where " & .getdata(i, vOthFldCmbStorageFldName) & " = " & GetVal(rsDummy.GetValue(f), ""), "", False))
                                Else
                                    .setdata(i, vOthFldDataValue, GetVal(rsDummy.GetValue(f), ""))
                                End If

                                blnFound = True
                                Exit For
                            End If
                        Next i
                    End With
                    With argFrm.vsfFtr
                        For i = 0 To .Rows.count - 1
                            If UCase(Trim(.getdata(i, vOthFldCode))) = UCase(Trim(rsDummy.GetName(f))) Then
                                'if the field is of combo datatype then load data accordingly
                                '# A - 11/Jan/2008
                                If .getdata(i, vOthFldBlnComboData) = "Y" Then
                                    .setdata(i, vOthFldDataValue, clsSVT.getDataStringValue("Select " & Trim(.getdata(i, vOthFldCmbDisplayCols)) & " from " & .getdata(i, vOthFldCmbTblName) & " where " & .getdata(i, vOthFldCmbStorageFldName) & " = " & GetVal(rsDummy.GetValue(f), ""), "", False))
                                Else
                                    .setdata(i, vOthFldDataValue, GetVal(rsDummy.GetValue(f), ""))
                                End If

                                blnFound = True
                                Exit For
                            End If
                        Next i
                        'If blnFound Then Exit For
                    End With
                Next f
            Else
                'MsgBox "No Fields are matched with " & cboInsertFromDocName.Text, vbInformation, Me.Caption
            End If
        End With

        'insert detail flds
        strsql = getLoadQry("D") & " order by slno"
        rsDummy = clsSVT.getDataReaderFromSQL(strsql)

        argFrm.vsfDet.Rows.count = 1

        If rsDummy.HasRows Then
            Do While rsDummy.Read

                With argFrm
                    .vsfDet.Rows.count = .vsfDet.Rows.count + 1
                    .vsfDet.Row = .vsfDet.Rows.count - 1
                    i = .vsfDet.Rows.count - 1

                    If intItemDocID > 0 Then
                        Dim strUOM As String = ""
                        For k As Int32 = 0 To rsDummy.FieldCount - 1
                            Select Case rsDummy.GetName(k)
                                Case "UOM", "UOMCODE"
                                    strUOM = rsDummy.Item(k)
                                Case "UOMID"
                                    strUOM = clsSVT.getDataStringValue("Select UOMCode from MstUOM Where UOMID=" & Val(rsDummy.Item(k)), "", False)
                            End Select
                            If strUOM <> "" Then Exit For
                        Next
                        Call .FillItemDet(clsSVT.getDataStringValue("Select " & strFldItemCode & " From " & strItemTblName & " Where " & strFldItemID & " = " & rsDummy.Item(strFldItemID), 0, False), i, strUOM)
                    End If

                End With

                For x = 0 To rsDummy.FieldCount - 1
                    For y = intDetCols To argFrm.vsfOptions.Rows.Count - 1
                        If UCase(argFrm.vsfOptions.GetData(y, vOthFldCode)) = UCase(rsDummy.GetName(x)) Then
                            argFrm.vsfDet.SetData(i, y, GetVal(rsDummy.GetValue(x), ""))
                        End If
                    Next y
                Next
            Loop
        End If

        'argFrm.vsfDet.Rows.count = 1
        'With rsDummy
        '    If Not .HasRows Then Exit Sub
        '    Do While .Read
        '        argFrm.vsfDet.Rows.count = argFrm.vsfDet.Rows.count + 1
        '        argFrm.vsfDet.Row = argFrm.vsfDet.Rows.count - 1
        '        i = argFrm.vsfDet.Rows.count - 1

        '        argFrm.vsfdet.setdata(i, vDetSlNo, i)
        '        argFrm.vsfdet.setdata(i, vDetItemID, !ItemID)
        '        argFrm.vsfdet.setdata(i, vDetItemSlNo, !ItemSLNO)
        '        argFrm.vsfdet.setdata(i, vDetItemCode, !ItemCode)

        '        Call fillItemRelatedDataFromDocDsgnr(intDocID, !ItemID, Me)

        '        argFrm.vsfDet.setdata(i, vDetItemName, !ItemName)
        '        '''''  argFrm.vsfDet.setdata(i, vDetSUOM, !UOMCode)
        '        'argFrm.vsfDet.setdata(i, VdetItemDesc, GetVal(!ItemDesc, ""))
        '        If blnLinkedToDoc Then
        '            argFrm.vsfDet.setdata(i, vDetShortClosed, GetVal(!ShortClosed, "N"))
        '        End If
        '        ''''' argFrm.vsfDet.setdata(i, vDetQty, GetVal(.Item(strLoadDocShForm & "Qty"), 0))

        '        'If GetVal(!UomID, 0) = 0 Then
        '        '    argFrm.vsfDet.setdata(i, vDetSUOM, clsSVT.getDataStringValue("Select UOMCode from MstUom u, MstItem i " & _
        '        '                     " where i.UomID = u.Uomid and Itemid =" & GetVal(!ItemID, 0), "", False))
        '        'Else
        '        '    argFrm.vsfDet.setdata(i, vDetSUOM, clsSVT.getDataStringValue("Select UOMCode from MstUom " & _
        '        '                    " where Uomid =" & GetVal(!UomID, 0), "", False))
        '        'End If


        '        If argFrm.vsfDet.getdata(i, vDetShortClosed) = "Y" Then
        '            Dim csGray As CellStyle = argFrm.vsfdet.Styles.Add("GRAY")
        '            csGray.BackColor = Color.LightGray
        '            Dim rgGray As CellRange = argFrm.vsfdet.GetCellRange(i, 0, i, argFrm.vsfdet.cols.count - 1)
        '            rgGray.Style = argFrm.vsfdet.Styles("GRAY")
        '        Else
        '            'Dim csWhite As CellStyle = argFrm.vsfdet.Styles.Add("WHITE")
        '            'csWhite.BackColor = Color.White
        '            'Dim rgWhite As CellRange = argFrm.vsfdet.GetCellRange(i, vDetItemCode)
        '            'rgWhite.Style = argFrm.vsfdet.Styles("WHITE")

        '        End If

        '        'If blnRateColReq Then
        '        '    argFrm.vsfDet.setdata(i, vdetRate, GetVal(.Item(strLoadDocShForm & "Rate"), 0))
        '        '    argFrm.vsfDet.setdata(i, vDetAmt, GetVal(.Item(strLoadDocShForm & "Amt"), 0))
        '        'End If


        '        'for loading additional flds of detail section
        '        For f = 0 To .FieldCount - 1
        '            blnFound = False
        '            With argFrm.VsfOptions
        '                For x = intDetCols To .Rows.count - 1
        '                    argFldName = Replace(UCase(Trim(rsDummy.GetName(f))), "d.", "")
        '                    argFldName = Replace(UCase(Trim(rsDummy.GetName(f))), "D.", "")
        '                    argFldName = Replace(UCase(Trim(rsDummy.GetName(f))), "h.", "")
        '                    argFldName = Replace(UCase(Trim(rsDummy.GetName(f))), "H.", "")
        '                    If UCase(Trim(.getdata(x, vOthFldCode))) = argFldName Then
        '                        argFrm.vsfDet.setdata(i, x, GetVal(rsDummy.GetValue(f), ""))
        '                        'blnFound = True
        '                        Exit For
        '                    End If
        '                Next x
        '                'If blnFound Then Exit For
        '            End With
        '        Next f
        '    Loop
        'End With
        'With argFrm
        '    argFrm.InsertPrevItemData(.intDetPrevTran, Val(.svtPCtrl.getBillID()), .vsfDet.GetData(i, vDetItemID), i)
        '    argFrm.fillItemRelatedDataFromDocDsgnr(intDocID, Val(.vsfDet.GetData(i, vDetItemID)), Me)
        '    argFrm.vsfOptValidateEdit(Me, .vsfDet, .vsfOptions, .vsfDet.Row, vDetItemCode, "T", .strDocDtlTblName, "", "")
        '    argFrm.fillVsfDetDefValues(.vsfDet, .vsfOptions, i)

        '    If .blnCalc = False Then
        '        Call .CalTotal()
        '    End If

        'End With



        'blnLoadItemGrid = False
        rsDummy.Close()
        Call argFrm.CalTotal()
        Me.Close()
    End Sub

    Private Function getLoadQry(argType As String) As String
        Dim strsql As String, strRes As String
        strRes = ""
        strsql = "Select * from UtlInsertFromDocFlds Where DocID = " & intDocID
        strsql = strsql & " and FromDocID = " & intLoadFromDocID
        strsql = strsql & " and TblType = '" & argType & "'"
        strsql = strsql & " order by DocfldName"

        Dim rsDummy As oledb.oledbDataReader
        'Dim blnItemTbl As Boolean
        Dim intDocItemId As Integer
        intDocItemId = clsSVT.getDataLongValue("Select MstDocIdItem from utlTransactions where HdrTblName='" & strDocHdrTblName & "'", 0, False)
        rsDummy = clsSVT.getDataReaderFromSQL(strsql)

        If argType = "H" Then
            strRes = "Select " & strLoadFromDocNo & " as LoadDocNo "
        Else
            strRes = "Select " & argType & ".* "
            If intDocItemId > 0 Then
                strRes &= ", ItemCode, ItemName, UOMCode "
            End If
        End If

        With rsDummy

            Do While .Read
                If argType = "H" Then
                    If UCase(Trim(!FromDocFldName)) <> UCase(Trim(!DocFldName)) Then
                        strRes = strRes & ", " & argType & "." & !FromDocFldName & " as " & !DocFldName
                    Else
                        strRes = strRes & ", " & argType & "." & !FromDocFldName
                    End If
                Else
                    If UCase(Trim(!FromDocFldName)) <> UCase(Trim(!DocFldName)) Then
                        strRes = strRes & ", " & argType & "." & !FromDocFldName & " as " & !DocFldName
                    Else
                        strRes = strRes & ", " & argType & "." & !FromDocFldName
                    End If
                End If
            Loop
            strRes = strRes & " from "
            If argType = "H" Then
                'strRes = strRes & strLoadFromHdrTblName
                strRes = strRes & strLoadFromHdrTblName & " " & argType
            Else
                strRes = strRes & strLoadFromDtlTblName & " d "
                If intDocItemId > 0 Then
                    strRes &= ", MstItem i, MstUOM u"
                End If

            End If
            strRes = strRes & " where " & strLoadFromTrnID & " = " & clsSVT.getDataLongValue("select " & strLoadFromTrnID & " from " & strLoadFromHdrTblName & " where " & strLoadFromDocNo & " ='" & Trim(cboInsertFromDocNo.Text) & "'", 0, False)
            If blnShowPendingLoadDocNos Then
                strRes = strRes & " and  " & strLoadFromTrnID
                strRes = strRes & " not in (Select InsertDOCid from "
                strRes = strRes & strDocHdrTblName
                strRes = strRes & " where "
                strRes = strRes & " Cancelled = 'N')"
            End If
            If argType = "D" Then
                If intDocItemId > 0 Then
                    strRes = strRes & " and d.ItemID = i.ItemID "
                    strRes = strRes & " and i.UomID = u.UomID"
                End If
            End If
            .Close()
        End With
        getLoadQry = strRes
    End Function
End Class