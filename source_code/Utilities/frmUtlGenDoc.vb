Imports C1.Win.C1FlexGrid
Imports Microsoft.Office.Interop
Imports ClosedXML.Excel

Public Class frmUtlGenDoc
    Dim clsSVT As New svtLibrary
    Public strMstTblName As String
    Public strMstFldCode As String
    Public strMstFldName As String
    Public strMstFldId As String
    Dim strRights As String
    Dim ModeIMP As String
    Dim intInvGen As Integer
    Public strSHForm As String
    Public strDocHdrTblName As String
    Public strDocDtlTblName As String
    Public strDocNoFldName As String
    Public strDocNo As String
    Public strPartyType As String
    Public dtDocDate As Date
    Public intDocID As Integer
    Public argFrm As Object
    Dim intPartyDocID As Int32
    Dim strPartyFilterCond As String
    Dim strPartyFlds As String

    Dim intItemId As Long
    Dim strItemQueryPresent As String
    Dim strItemQuery As String

    Dim dtSearch As DataTable
    Dim dvSearch As DataView

    Dim sngDefQty As Single

    'declarations for detail grid
   Const vDetSlNo = 0
    Const vDetItemID = 1
    Const vDetItemSlNo = 2
    Const vDetItemCode = 3
    Const vDetItemName = 4
    Const vDetSUOM = 5
    Const vDetShortClosed = 6
    Const vDetQOH = 7
    Const vDetQty = 8

    'Consts fro vsfCustItemList
    Const vCISel = 0
    Const vCIPartyNm = 1
    Const vCIItemCode = 2
    Const vCISUOM = 3
    Const vCIQty = 4
    Const vCIRate = 5

    'declarations for MultiGridDUMMY

    Const vCIDCategory = 0
    Const vCIDPartyNm = 1
    Const vCIDItemCode = 2
    Const vCIDSUOM = 3
    Const vCIDQty = 4
    Const vCIDRate = 5
    Private Sub cboCategory_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles cboCategory.KeyPress
        If e.KeyChar = "'" Then e.Handled = True
    End Sub

    Private Sub frmUtlGenDoc_KeyUp(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyUp
        If e.KeyCode = Keys.F9 And e.Control Then
            Call initFormSize(Me, Me.Text, True, 812, 610, 0, 0)
            Call initFormVsfDet(Me, Me.Text, vsfCustItemList, "DET")
            MsgBox("Restored Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
        If e.KeyCode = Keys.F8 And e.Control = True Then
            Call SaveActFormSettings(Me, Me.Text)
            Call SaveActFormVsfDet(Me, Me.Text, "DET", vsfCustItemList)
            MsgBox("Saved Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
    End Sub

    Private Sub frmUtlGenDoc_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        With vsfCustItemList
            .Cols.Count = 6
            .Rows.Count = 2

            Call SetFlexColor(vsfCustItemList, "M", vCIItemCode, vCIQty, vCISUOM, vCIPartyNm, vCIRate)

            .SetData(0, vCISel, "Sel")
            .SetData(0, vCIPartyNm, "Party Name")
            .SetData(0, vCIItemCode, "Item Code")
            .SetData(0, vCISUOM, "UOM")
            .SetData(0, vCIQty, "Qty")
            .SetData(0, vCIRate, "Rate")
            .Cols(vCIItemCode).ComboList = "..."
            '.ColComboList(vCIDPartyNm) = clsSVT.GenerateVSfComboString(strMstFldCode, _
            '                            strMstTblName, " Order by " & strMstFldCode)
            .Cols(vCIDPartyNm).ComboList = "..."
            .Cols(vCIDSUOM).ComboList = "..."
            .Cols(vCISel).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter
            .Cols(vCISel).DataType = GetType(Boolean)
            .Cols(vCIQty).DataType = GetType(Decimal)
            .Cols(vCIRate).DataType = GetType(Decimal)
            Call SetColWidth(vsfCustItemList, 405, 1875, 2310, 960, 960, 860)
            .SetCellCheck(1, vCISel, C1.Win.C1FlexGrid.CheckEnum.Checked)
            .AutoSizeCols()
            ' Call SetHideCol(vsfCustItemList, True, vCISel)
            Call SetFlexColor(vsfCustItemList, "", vCIDSUOM)
        End With
        fraSearch.Visible = False
        Call initItemVsfCombo()
        Call bindCombo(cboCategory, "Category", "Category", "Select Distinct Category from TrnMultiDocGen where PartyType = '" & Trim(Replace(strMstTblName, "Mst", "")) & "'" _
                       & " Order By Category")

        Call EnableControls(False)
        vsfCustItemList.Rows.Count = 1
        vsfCustItemList.Rows.Count = 2
        chkSelectAll.Visible = False
        Call initFormVsfDet(Me, Me.Text, vsfCustItemList, "DET")
        Call initFormSize(Me, Me.Text, False, 812, 610, 0, 0)
    End Sub

    Private Sub EnableControls(argBln As Boolean)
        btnGenerate.Enabled = argBln
        btnSave.Enabled = argBln
        btnCancel.Enabled = argBln
        vsfCustItemList.Enabled = argBln

        btnLoad.Enabled = Not argBln
        btnClose.Enabled = Not argBln
        cboCategory.Enabled = Not argBln
        btnImport.Enabled = Not argBln
        btnExport.Enabled = Not argBln
        chkSelectAll.Visible = argBln
    End Sub

    Private Sub btnLoad_Click(sender As System.Object, e As System.EventArgs) Handles btnLoad.Click
        Call EnableControls(True)
        Call fillInfo()
    End Sub

    Private Sub fillInfo()
        Dim strsql As String
        Dim rsDummy As oledb.oledbDataReader
        Dim strParty As String, strUOM As String, strItem As String
        Dim blnParty As Boolean, blnUom As Boolean, blnItem As Boolean

        strsql = "Select * from TrnMultiDocGen where Category='"
        strsql = strsql & cboCategory.Text & "' and Partytype='" & Trim(Replace(strMstTblName, "Mst", "")) & "'"

        rsDummy = clsSVT.getDataReaderFromSQL(strsql)
        Call RemoveEmptVSFRows(vsfCustItemList, 1, vCIPartyNm)
        vsfCustItemList.Rows.Count = 1
        With rsDummy
            Do While .Read
                strParty = clsSVT.getDataStringValue("Select " & strMstFldCode & " from " & strMstTblName & " where " & strMstFldId & "=" & GetVal(!PartyId), "", blnParty)
                strUOM = clsSVT.getDataStringValue("Select UOMCode from MStUom Where UomId=" & GetVal(!UomID), "", blnUom)
                strItem = clsSVT.getDataStringValue("Select ItemCode from MstItem where ItemId=" & GetVal(!ItemID), "", blnItem)
                If Not blnParty Or Not blnUom Or Not blnItem Then GoTo nextItem
                vsfCustItemList.Rows.Count = vsfCustItemList.Rows.Count + 1
                vsfCustItemList.SetData(vsfCustItemList.Rows.Count - 1, vCIPartyNm, strParty)
                vsfCustItemList.SetData(vsfCustItemList.Rows.Count - 1, vCIItemCode, strItem)
                vsfCustItemList.SetData(vsfCustItemList.Rows.Count - 1, vCIQty, GetVal(!Qty, 0))
                vsfCustItemList.SetData(vsfCustItemList.Rows.Count - 1, vCIRate, GetVal(!Rate, 0))
                vsfCustItemList.SetData(vsfCustItemList.Rows.Count - 1, vCIDSUOM, strUOM)

nextItem:
            Loop
            Call RemoveEmptVSFRows(vsfCustItemList, 1, vCIPartyNm)
            .Close()
            vsfCustItemList.AutoSizeCols()
        End With


    End Sub

    Private Sub btnImport_Click(sender As System.Object, e As System.EventArgs) Handles btnImport.Click
        If (Not chkUserRights(strRights, chkRightsPos.AddRights, Me.Text)) _
      And (Not chkUserRights(strRights, chkRightsPos.EditRights, Me.Text)) Then
            Exit Sub
        End If
        Try
            cdlg.DefaultExt = "xlsx"
            cdlg.Filter = "Excel File (*.xlsx)|*.xlsx"
            cdlg.FileName = "*.xlsx"
            If cdlg.ShowDialog() <> DialogResult.OK Then
                Return
                Exit Sub
            End If
            Call importFromXL(cdlg.FileName)
            Dim intRow As Int32 = 0
            intRow = PartyValidation()
            If intRow > 0 Then
                MsgBox("Invalid Party(" & vsfCustItemList.GetData(intRow, vCIDPartyNm) & ")...", MsgBoxStyle.Exclamation, Me.Text)
                Exit Sub
            End If
            intRow = ItemValidation()
            If intRow > 0 Then
                MsgBox("Invalid Item(" & vsfCustItemList.GetData(intRow, vCIDItemCode) & ")...", MsgBoxStyle.Exclamation, Me.Text)
                Exit Sub
            End If

            Call EnableControls(True)
            Exit Sub
        Catch ex As Exception
            MsgBox(ex.Message, vbInformation + vbOKOnly, Me.Text)
        End Try
    End Sub

    Private Sub importFromXL(argFileName As String)
        If gBlnMSOffice Then
            Try

                Dim xlWS As New Excel.Worksheet
                Dim xlapp As New Excel.Application
                xlapp.Workbooks.Open(argFileName, , True)
                xlWS = xlapp.Sheets(1)


                'Dim xlWB As New XLWorkbook(argFileName)
                'Dim xlWS As IXLWorksheet = xlWB.Worksheet(1)

                With vsfCustItemList
                    .Rows.Count = 1
                    Dim intRow As Int32 = 0
                    Dim intCol As Int32 = 0
                    For intCol = 0 To .Cols.Count - 1
                        If UCase(GetVal(.GetData(intRow, intCol), "")) <> UCase(GetVal(xlWS.Cells(intRow + 1, intCol + 1).Value, "")) Then
                            MsgBox("Column '" & GetVal(xlWS.Cells(intRow + 1, intCol + 1).Value, "") & "' does not match with grid...", MsgBoxStyle.Exclamation, "XL Import Failed")
                            Exit Sub
                        End If
                    Next

                    For intRow = 1 To 500
                        If GetVal(xlWS.Cells(intRow + 1, vCIItemCode + 1).Value, "") = "" Or GetVal(xlWS.Cells(intRow + 1, vCIPartyNm + 1).Value, "") = "" Then Exit For
                        For intCol = 0 To .Cols.Count - 1
                            If .Cols(intCol).Visible = True Then
                                Dim rgBlue As CellRange = .GetCellRange(0, intCol)
                                If rgBlue.Style.ForeColor.Name = "Red" Then
                                    Select Case .Cols(intCol).DataType
                                        Case GetType(Boolean), GetType(Date)
                                        Case GetType(Double), GetType(Integer), GetType(Decimal)
                                            If GetVal(xlWS.Cells(intRow + 1, intCol + 1).Value, 0) = 0 Then
                                                MsgBox("Mandatory Column '" & .GetData(0, intCol) & "' Value should be greater than 0 in excel...", MsgBoxStyle.Exclamation, "XL Import Failed")
                                                Exit Sub
                                            End If
                                        Case Else
                                            If GetVal(xlWS.Cells(intRow + 1, intCol + 1).Value, "") = "" Then
                                                MsgBox("Mandatory Column '" & .GetData(0, intCol) & "' does not contain value in excel...", MsgBoxStyle.Exclamation, "XL Import Failed")
                                                Exit Sub
                                            End If
                                    End Select

                                End If
                            End If
                        Next
                    Next

                    For intRow = 1 To 500
                        If GetVal(xlWS.Cells(intRow + 1, vCIItemCode + 1).Value, "") = "" Or GetVal(xlWS.Cells(intRow + 1, vCIPartyNm + 1).Value, "") = "" Then Exit For
                        .Rows.Count = .Rows.Count + 1
                        For intCol = 0 To .Cols.Count - 1
                            Select Case .Cols(intCol).DataType
                                Case GetType(Boolean)
                                    .SetData(intRow, intCol, GetVal(xlWS.Cells(intRow + 1, intCol + 1).Value, False))
                                Case GetType(Date)
                                    .SetData(intRow, intCol, GetVal(xlWS.Cells(intRow + 1, intCol + 1).Value, ""))
                                Case GetType(Double), GetType(Integer), GetType(Decimal)
                                    .SetData(intRow, intCol, GetVal(xlWS.Cells(intRow + 1, intCol + 1).Value, ""))
                                Case Else
                                    .SetData(intRow, intCol, GetVal(xlWS.Cells(intRow + 1, intCol + 1).Value, ""))
                            End Select
                        Next
                    Next
                    ' vsfImport = True
                End With
            Catch ex As Exception
                '  vsfImport = False
            End Try
        Else
            Try
                Dim xlWB As New XLWorkbook(argFileName)
                Dim xlWS As IXLWorksheet = xlWB.Worksheet(1)

                With vsfCustItemList
                    .Rows.Count = 1
                    Dim intRow As Int32 = 0
                    Dim intCol As Int32 = 0
                    For intCol = 0 To .Cols.Count - 1
                        If UCase(GetVal(.GetData(intRow, intCol), "")) <> UCase(GetVal(xlWS.Cell(intRow + 1, intCol + 1).Value, "")) Then
                            MsgBox("Column '" & GetVal(xlWS.Cell(intRow + 1, intCol + 1).Value, "") & "' does not match with grid...", MsgBoxStyle.Exclamation, "")
                            Exit Sub
                        End If
                    Next

                    For intRow = 1 To 500
                        If GetVal(xlWS.Cell(intRow + 1, vCIItemCode + 1).Value, "") = "" Or GetVal(xlWS.Cell(intRow + 1, vCIPartyNm + 1).Value, "") = "" Then Exit For
                        For intCol = 0 To .Cols.Count - 1
                            If .Cols(intCol).Visible = True Then
                                Dim rgBlue As CellRange = .GetCellRange(0, intCol)
                                If rgBlue.Style.ForeColor.Name = "Red" Then
                                    Select Case .Cols(intCol).DataType
                                        Case GetType(Boolean), GetType(Date)
                                        Case GetType(Double), GetType(Integer), GetType(Decimal)
                                            If GetVal(xlWS.Cell(intRow + 1, intCol + 1).Value, 0) = 0 Then
                                                MsgBox("Mandatory Column '" & .GetData(0, intCol) & "' Value should be greater than 0 in excel...", MsgBoxStyle.Exclamation, "")
                                                Exit Sub
                                            End If
                                        Case Else
                                            If GetVal(xlWS.Cell(intRow + 1, intCol + 1).Value, "") = "" Then
                                                MsgBox("Mandatory Column '" & .GetData(0, intCol) & "' does not contain value in excel...", MsgBoxStyle.Exclamation, "")
                                                Exit Sub
                                            End If
                                    End Select

                                End If
                            End If
                        Next
                    Next

                    For intRow = 1 To 500
                        If GetVal(xlWS.Cell(intRow + 1, vCIItemCode + 1).Value, "") = "" Or GetVal(xlWS.Cell(intRow + 1, vCIPartyNm + 1).Value, "") = "" Then Exit For
                        .Rows.Count = .Rows.Count + 1
                        For intCol = 0 To .Cols.Count - 1
                            Select Case .Cols(intCol).DataType
                                Case GetType(Boolean)
                                    .SetData(intRow, intCol, GetVal(xlWS.Cell(intRow + 1, intCol + 1).Value, False))
                                Case GetType(Date)
                                    .SetData(intRow, intCol, GetVal(xlWS.Cell(intRow + 1, intCol + 1).Value, ""))
                                Case GetType(Double), GetType(Integer), GetType(Decimal)
                                    .SetData(intRow, intCol, GetVal(xlWS.Cell(intRow + 1, intCol + 1).Value, ""))
                                Case Else
                                    .SetData(intRow, intCol, GetVal(xlWS.Cell(intRow + 1, intCol + 1).Value, ""))
                            End Select
                        Next
                    Next
                    ' vsfImport = True
                End With
            Catch ex As Exception
                '  vsfImport = False
            End Try
        End If
    End Sub

    '        Dim xlapp As New Excel.Application
    '        Dim i As Integer, lngItemId As Long, blnFound As Boolean, lngUOMID As Long

    '        showWaitCursor()

    '        xlapp.Workbooks.Open(argFileName, , True)
    '        With xlapp.ActiveSheet
    '            '8888888888888888 Check for excel format
    '            If Trim$(.Range("A1") & .Range("B1") & .Range("C1") & .Range("D1") & .Range("E1")) <> _
    '            "Party NameItem NameUOMQtyRate" Then '# A - Change
    '                MsgBox("Excel file is not according to the TEMPLATE..." & vbCrLf & _
    '                       "Please Select the correct Excel Sheet!!", vbInformation, Me.Text)
    '                GoTo ExitSub
    '            End If
    '            '8888888888888888

    '            i = 2
    '            If Trim(vsfCustItemList.GetData(vsfCustItemList.Rows.Count - 1, vCIPartyNm)) = "" Then
    '                vsfCustItemList.Rows.Count = vsfCustItemList.Rows.Count - 1
    '                vsfCustItemList.Row = vsfCustItemList.Rows.Count - 1
    '            End If
    '            'Validation
    '            Do While Trim(.Range("A" & i)) <> ""
    '                'Chk for UOM - Validation for that
    '                blnFound = False
    '                lngItemId = clsSVT.getDataLongValue("select " & strMstFldId & " from " & strMstTblName & "Where " & strMstFldCode & " = '" & .Range("A" & i) & "'", 0, blnFound)

    '                If Not blnFound Then
    '                    MsgBox("Party Name (" & .Range("A" & i) & ")not found in the " & Replace(strMstFldCode, "Mst", "") & _
    '                    "master !!", vbInformation, Me.Text)
    '                    Exit Sub
    '                End If
    '                blnFound = False
    '                lngItemId = clsSVT.getDataLongValue("Select ItemId from MstItem Where ItemCode = '" & .Range("B" & i) & "'", 0, blnFound)
    '                If Not blnFound Then
    '                    MsgBox("Item Name (" & .Range("B" & i) & ")not found in the item master!!", vbInformation, Me.Text)
    '                    Exit Sub
    '                End If
    '                'Chk for UOM - Validation for that
    '                blnFound = False
    '                lngUOMID = clsSVT.getDataLongValue("Select UOMId from MstUOM Where UOM = '" & .Range("C" & i) & "'", 0, blnFound)
    '                If Not blnFound Then _
    '                MsgBox("UOM (" & .Range("C" & i) & ")not found in the UOM master!!", vbInformation, Me.Text)
    '                Dim strUOM As String
    '                strUOM = Trim$(UCase$(genVsfMultiUomList(CInt(lngItemId))))
    '                If InStr(1, strUOM, Trim$(UCase$(.Range("C" & i)))) = 0 Then
    '                    MsgBox("Please enter valid UOM for the Item " & .Range("A" & i), vbInformation + vbOKOnly, Me.Text)
    '                    Exit Sub
    '                End If

    '                i = i + 1
    '            Loop
    '            ModeIMP = "V"
    '            i = 2
    '            Do While Trim(.Range("A" & i)) <> ""
    '                StMsg("Reading " & .Range("A" & i))
    '                vsfCustItemList.Rows.Count = vsfCustItemList.Rows.Count + 1
    '                vsfCustItemList.Row = vsfCustItemList.Rows.Count - 1
    '                vsfCustItemList.SetData(vsfCustItemList.Rows.Count - 1, vCIPartyNm, .Range("A" & i))
    '                vsfCustItemList.SetData(vsfCustItemList.Rows.Count - 1, vCIItemCode, .Range("B" & i))
    '                vsfCustItemList.SetData(vsfCustItemList.Rows.Count - 1, vCISUOM, .Range("C" & i))  '# A - Change
    '                vsfCustItemList.SetData(vsfCustItemList.Rows.Count - 1, vCIQty, Val(.Range("D" & i)))
    '                vsfCustItemList.SetData(vsfCustItemList.Rows.Count - 1, vCIRate, Val(.Range("E" & i)))
    '                i = i + 1
    '            Loop
    '            ModeIMP = ""
    '        End With
    'ExitSub:
    '        StMsg("")
    '        xlapp.Workbooks.Close()
    '        xlapp = Nothing
    '        showNormalCursor()
    '    End Sub

    Private Sub chkSelectAll_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkSelectAll.CheckedChanged
        Dim i As Integer
        For i = 1 To vsfCustItemList.Rows.Count - 1
            If chkSelectAll.Checked = True Then
                vsfCustItemList.SetCellCheck(i, vCISel, C1.Win.C1FlexGrid.CheckEnum.Checked)
            Else
                vsfCustItemList.SetCellCheck(i, vCISel, C1.Win.C1FlexGrid.CheckEnum.Unchecked)
            End If
        Next i
    End Sub

    Private Function PartyValidation() As Int32
        PartyValidation = 0
        Dim blnfound As Boolean = False
        With vsfCustItemList
            For i As Int32 = 1 To .Rows.Count - 1
                If GetVal(.GetData(i, vCIDPartyNm), "") <> "" Then
                    blnfound = False
                    Call clsSVT.getDataStringValue("Select * from " & strMstTblName & " where inactive='N' and " & strMstFldCode & "='" & .GetData(i, vCIDPartyNm) & "'", "", blnfound)
                    If blnfound = False Then Return i
                End If
            Next
        End With
    End Function

    Private Function ItemValidation() As Int32
        ItemValidation = 0
        Dim blnfound As Boolean = False
        With vsfCustItemList
            For i As Int32 = 1 To .Rows.Count - 1
                If GetVal(.GetData(i, vCIDItemCode), "") <> "" Then
                    blnfound = False
                    Call clsSVT.getDataStringValue("Select * from MstItem where inactive='N' and ItemCode='" & .GetData(i, vCIDItemCode) & "'", "", blnfound)
                    If blnfound = False Then Return i
                End If
            Next
        End With
    End Function

    Private Sub btnRemove_Click(sender As System.Object, e As System.EventArgs) Handles btnRemove.Click
        Try
            If MsgBox("Do You want to Remove the row --> " & vsfCustItemList.GetData(vsfCustItemList.Row, 2) & " ?", vbInformation + vbYesNo, Me.Text) = vbYes Then
                vsfCustItemList.Rows.Remove(vsfCustItemList.Row)
            End If
            If vsfCustItemList.Rows.Count = 1 Then vsfCustItemList.Rows.Count = 2

            Exit Sub
        Catch ex As Exception
            MsgBox("Please select the row in the Grid", vbOKOnly, Me.Text)
        End Try
    End Sub

    Private Sub btnGenerate_Click(sender As System.Object, e As System.EventArgs) Handles btnGenerate.Click
        Dim strParty As String
        Dim i As Integer
        Dim dtInvDate As Date
        intInvGen = 0
        vsfGenCopy.Visible = True
        If Trim(vsfCustItemList.GetData(0, vCIPartyNm)) = "" Then Exit Sub
        Call RemoveEmptVSFRows(vsfCustItemList, 0, vCIDPartyNm)
        If Not validEntry() Then Exit Sub

        If Not validOthFLds(Me, strDocHdrTblName, strDocNoFldName, strDocNo, Trim(strSHForm)) Then
            MsgBox("Please ensure all the errors reported are taken care, before Multi Generation!!", vbInformation, Me.Text)
            Exit Sub
        End If
        dtInvDate = argFrm.dtDocDate.value
        With vsfCustItemList
            'this will save hdr ftr det values [used for restoration during consecutive additions
            Call saveHdrFtrDetGridFlds()               'Satish

            Do While True       'infinite loop
                strParty = ""
                'loop for selecting a party which is checked
                For i = 1 To .Rows.Count - 1
                    If .GetCellCheck(i, vCISel) = C1.Win.C1FlexGrid.CheckEnum.Checked Then
                        strParty = Trim(.GetData(i, vCIPartyNm))
                        Exit For
                    End If
                Next i

                If strParty = "" Then Exit Do
                Dim intPartyID As Integer
                intPartyID = clsSVT.getDataLongValue("Select " & strMstFldId & " from " & strMstTblName & " Where " & strMstFldCode & "='" & strParty & "'", 0, False)
                'new click should be done here
                Call argFrm.svtBtnCtrl_btnClicked(svtBtns.svtNew)
                argFrm.dtDocDate.value = dtInvDate
                argFrm.svtPartyDtl.fillparty(intPartyID, strPartyType)
                For i = 1 To .Rows.Count - 1
                    If .GetCellCheck(i, vCISel) = C1.Win.C1FlexGrid.CheckEnum.Checked And strParty = Trim(.GetData(i, vCIPartyNm)) Then
                        'add rows to detail
                        Call FillDataFromMulti(i)
                        .SetCellCheck(i, vCISel, C1.Win.C1FlexGrid.CheckEnum.Unchecked)
                    End If
                Next i
                Call insertMultiData()   'satish

            Loop
        End With
        MsgBox(intInvGen & " " & strSHForm & "'s are generated!!", vbInformation, Me.Text)
        Call btnCancel_Click(sender, e)
        Call btnClose_Click(sender, e)
    End Sub

    Private Sub saveHdrFtrDetGridFlds()
        Dim i As Integer, j As Integer
        With vsfGenCopy
            .Rows.Count = 0
            For i = 0 To 2
                For j = 0 To argFrm.vsfHdr.Rows.count - 1
                    .Rows.Count = .Rows.Count + 1
                    .SetData(.Rows.Count - 1, 0, argFrm.vsfHdr.getdata(j, vOthFldCode))
                    .SetData(.Rows.Count - 1, 1, argFrm.vsfHdr.getdata(j, vOthFldDataValue))
                Next j
                For j = 0 To argFrm.vsfFtr.Rows.count - 1
                    .Rows.Count = .Rows.Count + 1
                    .SetData(.Rows.Count - 1, 0, argFrm.vsfFtr.getdata(j, vOthFldCode))
                    .SetData(.Rows.Count - 1, 1, argFrm.vsfFtr.getdata(j, vOthFldDataValue))
                Next j
            Next i
            For i = argFrm.intDetCols To argFrm.vsfDet.Cols.count - 1
                .Rows.Count = .Rows.Count + 1
                .SetData(.Rows.Count - 1, 0, argFrm.VsfOptions.getdata(i, vOthFldCode))
                .SetData(.Rows.Count - 1, 1, argFrm.vsfDet.getdata(1, i))
            Next i
        End With
    End Sub

    Private Sub insertMultiData()
        Call fillHdrFldsFromvsfGenCopy()
        'Call cboCust_Validate(False)
        Call argFrm.CalTotal()
        If argFrm.validFlds Then
            If argFrm.SaveDoc Then
                Call argFrm.enableDisableFlds(False)
                argFrm.svtbtnCtrl.PerformAction(svtBtns.svtSave)
                argFrm.Mode = ""
                ' argFrm.btnCopyFrom.Visible = False
                intInvGen = intInvGen + 1
            End If
        End If
    End Sub

    Private Sub fillHdrFldsFromvsfGenCopy()
        Dim i As Integer, j As Integer
        With vsfGenCopy
            For i = 0 To .Rows.Count - 1
                For j = 0 To argFrm.vsfhdr.Rows.count - 1
                    If .GetData(i, 0) = argFrm.vsfhdr.getdata(j, vOthFldCode) Then
                        argFrm.vsfhdr.setdata(j, vOthFldDataValue, .GetData(i, 1))
                        Exit For
                    End If
                Next j
                For j = 0 To argFrm.vsfFtr.Rows.count - 1
                    If .GetData(i, 0) = argFrm.vsfFtr.getdata(j, vOthFldCode) Then
                        argFrm.vsfFtr.setdata(j, vOthFldDataValue, .GetData(i, 1))
                        Exit For
                    End If
                Next j
            Next i
        End With
    End Sub

    Private Function validEntry() As Boolean
        Dim i As Integer
        Dim strInactive As String
        validEntry = False
        Dim blnParty As Boolean, blnUom As Boolean, blnItem As Boolean
        If Trim$(cboCategory.Text) = "" Then
            MsgBox("Category Cannot Be Null!!", vbInformation, Me.Text)
            cboCategory.Enabled = True
            cboCategory.Focus()
            Exit Function
        End If
        If Not validDate(argFrm.dtDocDate.value) Then
            MsgBox("Doc Date Should be with the financial year", vbExclamation, Me.Text)
            Exit Function
        End If

        With vsfCustItemList
            For i = 1 To .Rows.Count - 1
                '                intPartyCode = clsSvt.GetDataValue(strMstFldCode, 0, strMstTblName, _
                '                blnParty, " Where " & strMstFldName & "='" & .TextMatrix(i, vCIPartyNm) & "'")


                Call clsSVT.getDataLongValue("Select " & strMstFldId & " from " & strMstTblName & " Where " & strMstFldCode & "='" & Trim(.GetData(i, vCIPartyNm)) & "'", 0, blnParty)
                If Not blnParty Then
                    MsgBox(Trim(.GetData(i, vCIPartyNm)) & "not present in " & Replace(strMstTblName, "Mst", "") & " Master  !!", vbInformation, Me.Text)
                    vsfCustItemList.Focus()
                    Exit Function
                End If

                strInactive = clsSVT.getDataStringValue("Select Inactive from " & strMstTblName & " Where " & strMstFldCode & "='" & Trim(.GetData(i, vCIPartyNm)) & "'", "", blnParty)
                If strInactive = "Y" Then
                    MsgBox(Trim(.GetData(i, vCIPartyNm)) & " is marked as INACTIVE!! Please mark it as Active and resume!", vbInformation, Me.Text)
                    vsfCustItemList.Focus()
                    Exit Function
                End If

                Call clsSVT.getDataLongValue("Select ItemId from MstItem Where ItemCode='" & Trim(.GetData(i, vCIItemCode)) & "'", 0, blnItem)
                If Not blnItem Then
                    MsgBox("Item Not Present in Item Master!!", vbInformation, Me.Text)
                    vsfCustItemList.Focus()
                    Exit Function
                End If
                Call clsSVT.getDataLongValue("Select UOMid from MstUOM Where UOMCode='" & Trim(.GetData(i, vCISUOM)) & "'", 0, blnUom)
                If Not blnParty Then
                    MsgBox("Uom Not Prsent in UOM Master!!", vbInformation, Me.Text)
                    vsfCustItemList.Focus()
                    Exit Function
                End If
                If Val(.GetData(i, vCIQty)) <= 0 Then
                    MsgBox("Quantity must be greater than Zero!!", vbInformation, Me.Text)
                    vsfCustItemList.Focus()
                    Exit Function
                End If
                If Val(.GetData(i, vCIRate)) <= 0 Then
                    MsgBox("Rate must be greater than Zero!!", vbInformation, Me.Text)
                    vsfCustItemList.Focus()
                    Exit Function
                End If

            Next i
        End With
        validEntry = True
    End Function

    Private Sub btnCancel_Click(sender As System.Object, e As System.EventArgs) Handles btnCancel.Click
        Call EnableControls(False)
        vsfCustItemList.Rows.Count = 1
        vsfCustItemList.ScrollBars = ScrollBars.None
    End Sub

    Private Sub btnSave_Click(sender As System.Object, e As System.EventArgs) Handles btnSave.Click
        If validEntry() Then
            If SaveCategory() Then
                MsgBox("Category " & Trim(cboCategory.Text) & " saved successfully!!", vbInformation, Me.Text)
                Call EnableControls(False)
            End If
        End If
    End Sub

    Private Function SaveCategory() As Boolean
        Dim intParty As Integer, intItem As Integer, intUOM As Integer
        Dim strsql As String, i As Integer
        SaveCategory = False
        Call CopyGrid()
        With vsfCustItemList
            clsSVT.execSQLStatement("Delete from TrnMultiDocGen where Category='" & (cboCategory.Text) & "'")
            For i = 1 To .Rows.Count - 1
                If Trim(.GetData(i, vCIPartyNm)) = "" Then GoTo nexti
                intParty = clsSVT.getDataLongValue("Select " & strMstFldId & " from " & strMstTblName & " Where " & strMstFldCode & "='" & Trim(.GetData(i, vCIPartyNm)) & "'", 0, False)
                intItem = clsSVT.getDataLongValue("Select ItemId from MstItem Where ItemCode='" & Trim(.GetData(i, vCIItemCode)) & "'", 0, False)
                intUOM = clsSVT.getDataLongValue("Select UOMid from MstUOM Where UOMCode='" & Trim(.GetData(i, vCISUOM)) & "'", 0, False)

                strsql = "Insert into TrnMultiDocGen"
                strsql = strsql & "(Category,Partyid,ItemId,UomId,Qty,Rate,PartyType)"
                strsql = strsql & " values("
                strsql = strsql & "'" & (cboCategory.Text) & "',"
                strsql = strsql & intParty & ","
                strsql = strsql & intItem & ","
                strsql = strsql & intUOM & ","
                strsql = strsql & Val(.GetData(i, vCIQty)) & ","
                strsql = strsql & Val(.GetData(i, vCIRate)) & ","
                strsql = strsql & "'" & Trim(Replace(strMstTblName, "Mst", "")) & "'"
                strsql = strsql & ")"
                clsSVT.execSQLStatement(strsql)
nexti:
            Next i
        End With
        SaveCategory = True
    End Function

    Private Sub CopyGrid()
        Dim intParty As Integer, intItem As Integer, intUOM As Integer
        Dim i As Integer
        vsfCustItemListDummy.Rows.Count = 0
        vsfCustItemListDummy.Cols.Count = vsfCustItemList.Cols.Count
        With vsfCustItemList
            For i = 1 To .Rows.Count - 1
                If .GetCellCheck(i, vCISel) = C1.Win.C1FlexGrid.CheckEnum.Unchecked Then GoTo nexti
                intParty = clsSVT.getDataLongValue("Select " & strMstFldId & " from " & strMstTblName & " Where " & strMstFldCode & "='" & Trim(.GetData(i, vCIPartyNm)) & "'", 0, False)
                intItem = clsSVT.getDataLongValue("Select ItemId from MstItem  Where ItemCode='" & Trim(.GetData(i, vCIItemCode)) & "'", 0, False)
                intUOM = clsSVT.getDataLongValue("Select UOMid from MstUOM  Where UOMCode='" & Trim(.GetData(i, vCISUOM)) & "'", 0, False)
                vsfCustItemListDummy.Rows.Count = vsfCustItemListDummy.Rows.Count + 1
                vsfCustItemListDummy.SetData(vsfCustItemListDummy.Row, vCIDCategory, Trim(cboCategory.Text))
                vsfCustItemListDummy.SetData(vsfCustItemListDummy.Row, vCIDPartyNm, Trim(.GetData(i, vCIPartyNm)))
                vsfCustItemListDummy.SetData(vsfCustItemListDummy.Row, vCIDItemCode, Trim(.GetData(i, vCIItemCode)))
                vsfCustItemListDummy.SetData(vsfCustItemListDummy.Row, vCIDSUOM, Trim(.GetData(i, vCISUOM)))
                vsfCustItemListDummy.SetData(vsfCustItemListDummy.Row, vCIDQty, Trim(.GetData(i, vCIQty)))
                vsfCustItemListDummy.SetData(vsfCustItemListDummy.Row, vCIDRate, Trim(.GetData(i, vCIRate)))
nexti:
            Next i
        End With
    End Sub

    Private Sub btnExport_Click(sender As System.Object, e As System.EventArgs) Handles btnExport.Click
        If gBlnMSOffice Then
            Dim xlapp As New Excel.Application
            Dim xlWB As Excel.Worksheet = xlapp.Workbooks.Add()
            Dim xlWS As Excel.Worksheets = xlWB.Worksheet.Add()

            'Dim xlWB As New XLWorkbook()
            'xlWB.Worksheets.Add("Sheet1")
            'Dim xlWS As IXLWorksheet = xlWB.Worksheet("Sheet1")
            Try
                Dim i As Int32 = 0
                With vsfCustItemList
                    For j = 0 To .Cols.Count - 1
                        If .Cols(j).Visible = False Then
                            xlWS.Column(j + 1).Hide()
                            If GetVal(.GetData(i, j), "") <> "" Then xlWS.Cell(i + 1, j + 1).Value = .GetData(i, j)
                        Else
                            If GetVal(.GetData(i, j), "") = "" Then
                                xlWS.Column(j + 1).Hide()
                            Else
                                xlWS.Cells(i + 1, j + 1).Value = .GetData(i, j)
                                Dim rgBlue As CellRange = .GetCellRange(i, j)
                                If rgBlue.Style.ForeColor.Name = "Blue" Then
                                    xlWS.Column(j + 1).Hide()
                                    xlWS.Cells(i + 1, j + 1).Style.Font.FontColor = Color.Blue
                                ElseIf rgBlue.Style.ForeColor.Name = "Red" Then
                                    xlWS.Cells(i + 1, j + 1).Style.Font.FontColor = Color.Red
                                Else
                                    xlWS.Cells(i + 1, j + 1).Style.Font.FontColor = Color.Black
                                End If
                            End If
                        End If
                    Next
                End With
                Dim strTemp As String = "MultiDocGen_Template_" & Date.Now.Ticks.ToString() & ".xlsx"
                xlWB.SaveAs(strTemp)
                xlWB.Dispose()
                xlWS.Dispose()
                MsgBox("Templated Created", MsgBoxStyle.Information, "Process Completed")
                System.Diagnostics.Process.Start(strTemp)
            Catch ex As Exception
                xlWB.Dispose()
                xlWS.Dispose()
                MsgBox("Template Excel Generation Failed. Details" & vbNewLine & ex.Message, MsgBoxStyle.Information, "Failed")
                '  vsfTemplate = False
            End Try
        Else
            Dim xlWB As New XLWorkbook()
        xlWB.Worksheets.Add("Sheet1")
        Dim xlWS As IXLWorksheet = xlWB.Worksheet("Sheet1")
        Try
            Dim i As Int32 = 0
            With vsfCustItemList
                For j = 0 To .Cols.Count - 1
                    If .Cols(j).Visible = False Then
                        xlWS.Column(j + 1).Hide()
                        If GetVal(.GetData(i, j), "") <> "" Then xlWS.Cell(i + 1, j + 1).Value = .GetData(i, j)
                    Else
                        If GetVal(.GetData(i, j), "") = "" Then
                            xlWS.Column(j + 1).Hide()
                        Else
                            xlWS.Cell(i + 1, j + 1).Value = .GetData(i, j)
                            Dim rgBlue As CellRange = .GetCellRange(i, j)
                            If rgBlue.Style.ForeColor.Name = "Blue" Then
                                xlWS.Column(j + 1).Hide()
                                xlWS.Cell(i + 1, j + 1).Style.Font.FontColor = XLColor.Blue
                            ElseIf rgBlue.Style.ForeColor.Name = "Red" Then
                                xlWS.Cell(i + 1, j + 1).Style.Font.FontColor = XLColor.Red
                            Else
                                xlWS.Cell(i + 1, j + 1).Style.Font.FontColor = XLColor.Black
                            End If
                        End If
                    End If
                Next
            End With
            Dim strTemp As String = "MultiDocGen_Template_" & Date.Now.Ticks.ToString() & ".xlsx"
            xlWB.SaveAs(strTemp)
            xlWB.Dispose()
            xlWS.Dispose()
                MsgBox("Templated Created", MsgBoxStyle.Information, "")
                System.Diagnostics.Process.Start(strTemp)
        Catch ex As Exception
            xlWB.Dispose()
            xlWS.Dispose()
                MsgBox("Template Excel Generation Failed. Details" & vbNewLine & ex.Message, MsgBoxStyle.Information, "")
                '  vsfTemplate = False
            End Try
        End If
    End Sub

    Private Sub btnClose_Click(sender As System.Object, e As System.EventArgs) Handles btnClose.Click
        Me.Close()
        'chkMultiDoc.value = vbUnchecked
    End Sub

    Private Sub vsfCustItemList_BeforeEdit(sender As Object, e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfCustItemList.BeforeEdit
        With vsfCustItemList
            Select Case e.Col
                Case vCIPartyNm
                    .Cols(vCIPartyNm).ComboList = GenVSfComboStringCond(strMstFldCode, strMstTblName, "order by " & strMstFldCode, " Where InActive='N'")
                Case vCISUOM
                    Dim intItemId As Integer
                    intItemId = clsSVT.getDataLongValue("Select ItemID from MstItem where ItemCode = '" & Trim(vsfCustItemList.GetData(vsfCustItemList.Row, vCIItemCode)) & "'", 0, False)
                    .Cols(vCISUOM).ComboList = genVsfMultiUomList(Val(intItemId))
            End Select
        End With
    End Sub

    Private Sub vsfCustItemList_KeyPressEdit(sender As Object, e As C1.Win.C1FlexGrid.KeyPressEditEventArgs) Handles vsfCustItemList.KeyPressEdit
        If e.KeyChar = "'" Then e.Handled = True
    End Sub

    Private Sub vsfCustItemList_KeyUp(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles vsfCustItemList.KeyUp
        If ModeIMP = "V" Then Exit Sub
        With vsfCustItemList
            If .Col = .Cols.Count - 1 And e.KeyCode = 9 And .Row = .Rows.Count - 1 Then
                If Trim(.GetData(.Row, vCIItemCode)) <> "" And Trim(.GetData(.Row, vCIPartyNm)) <> "" And Trim(.GetData(.Row, vCIQty)) <> "" And Trim(.GetData(.Row, vCIRate)) <> "" Then
                    .LeftCol = vCISel
                    .Col = vCISel
                    .Rows.Count = .Rows.Count + 1
                    .Row = .Rows.Count - 1
                    .TopRow = .Rows.Count - 1
                    .SetCellCheck(.Row, vCISel, C1.Win.C1FlexGrid.CheckEnum.Checked)
                Else
                    'If .Rows.Count > 2 Then .Rows.Count = .Rows.Count - 1
                    Exit Sub
                End If
            End If
        End With
    End Sub

    Private Sub vsfCustItemList_SetupEditor(sender As Object, e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfCustItemList.SetupEditor
        Select Case e.Col
            Case vCIRate, vCIQty
                Dim tb As TextBox = vsfCustItemList.Editor
                tb.MaxLength = 10
        End Select
    End Sub

    Private Sub vsfCustItemList_ValidateEdit(sender As Object, e As C1.Win.C1FlexGrid.ValidateEditEventArgs) Handles vsfCustItemList.ValidateEdit
        If ModeIMP = "V" Then Exit Sub
        Dim i As Integer

        Dim blnParty As Boolean, blnUom As Boolean, blnItem As Boolean
        With vsfCustItemList
            Select Case e.Col
                Case vCIPartyNm
                    Call clsSVT.getDataLongValue("Select " & strMstFldId & " from " & strMstTblName & " Where " & strMstFldCode & "='" & .Editor.Text & "'", 0, blnParty)
                    If Not blnParty Then
                        MsgBox(Trim(.GetData(i, vCIPartyNm)) & "not present in " & Replace(strMstTblName, "Mst", "") & " Master  !!", vbInformation, Me.Text)
                        e.Cancel = True
                        Exit Sub
                    End If
                Case vCIItemCode
                    Call clsSVT.getDataLongValue("Select ItemId from MstItem Where ItemCode='" & .Editor.Text & "'", 0, blnItem)
                    If Not blnItem Then
                        MsgBox("Item Not Present in Item Master!!", vbInformation, Me.Text)
                        e.Cancel = True
                        Exit Sub
                    End If
                Case vCISUOM
                    Call clsSVT.getDataLongValue("Select UOMid from MstUOM Where UOMCode ='" & .Editor.Text & "'", 0, blnUom)
                    If Not blnUom Then
                        MsgBox("Uom Not Prsent in UOM Master!!", vbInformation, Me.Text)
                        e.Cancel = True
                        Exit Sub
                    End If
                Case vCIQty
                    If Val(.Editor.Text) <= 0 Then
                        MsgBox("Quantity must be greater than Zero!!", vbInformation, Me.Text)
                        e.Cancel = True
                        Exit Sub
                    End If
                Case vCIRate
                    If Val(.Editor.Text) <= 0 Then
                        MsgBox("Rate must be greater than Zero!!", vbInformation, Me.Text)
                        e.Cancel = True
                        Exit Sub
                    End If

            End Select
        End With
    End Sub

    Private Sub FillDataFromMulti(argRow As Integer)
        Dim i As Integer
        Dim strUOM As String, strItemCode As String, strItemName As String, strItemId As String

        With vsfCustItemList
            'vsfDet.Rows = 1
            argFrm.vsfDet.Rows.count = argFrm.vsfDet.Rows.count + 1
            i = argFrm.vsfDet.Rows.count - 1
            argFrm.vsfDet.setdata(i, vDetSlNo, i)
            argFrm.vsfDet.setdata(i, vDetItemSlNo, i)
            '        strParty = clsSvt.GetDataValue(strMstFldName, "", strMstTblName, blnParty, _
            '                " where " & strMstFldId & "=" & clsSvt.GetVal(!PartyId))
            strUOM = .GetData(argRow, vCIDSUOM)
            strItemCode = .GetData(argRow, vCIDItemCode).ToString
            strItemName = clsSVT.getDataStringValue("Select ItemName from MstItem where ItemCode='" & Trim(.GetData(argRow, vCIDItemCode)) & "'", "", False)
            strItemId = clsSVT.getDataLongValue("Select ItemId from MstItem where ItemCode='" & Trim(.GetData(argRow, vCIDItemCode)) & "'", 0, False)
            argFrm.vsfDet.setdata(i, vDetItemCode, strItemCode)
            argFrm.vsfDet.setdata(i, vDetItemName, strItemName)
            argFrm.vsfDet.setdata(i, vDetItemID, strItemId)

            argFrm.vsfDet.Row = i
            'cboItemCode.Text = Trim(.TextMatrix(argRow, vMultiItem))
            'Call cboItemCode_Validate(False) 

            'Call argFrm.vsfDet_ValidateEdit(argRow, vDetItemCode, False) 'Satish
            If argFrm.vsfdetValidateEdit(argRow, vDetItemCode) = True Then
                Exit Sub
            End If

            'Call fillOthFlds(Me)
            'Call vsfDet_ValidateEdit(i, vDetItemName, False)
            'argFrm.vsfDet.setdata(i, vdetRate, Val(.GetData(argRow, vCIDRate)))
            argFrm.vsfDet.setdata(i, vDetQty, Val(.GetData(argRow, vCIDQty)))
            argFrm.vsfDet.setdata(i, vDetSUOM, strUOM)
            With vsfCustItemList
                For x = 0 To .Cols.Count - 1
                    For j = 0 To argFrm.vsfdet.cols.count - 1
                        If .GetData(0, x) = argFrm.vsfdet.getdata(0, j) Then
                            argFrm.vsfDet.setdata(i, j, .GetData(argRow, x))
                            Exit For
                        End If
                    Next j
                Next x
            End With


            'Satish
            'With vsfGenCopy
            '    For x = 0 To .Rows.Count - 1
            '        For j = 0 To argFrm.VsfOptions.Rows.count - 1
            '            If .GetData(x, 0) = argFrm.VsfOptions.getdata(j, vOthFldCode) Then
            '                argFrm.vsfDet.setdata(i, j, .GetData(x, 1))
            '                Exit For
            '            End If
            '        Next j
            '    Next x
            'End With

          
            Call fillItemRelatedDataFromDocDsgnr(intDocID, Val(strItemId), argFrm)
        End With
        Call argFrm.CalTotal()
    End Sub

    Public Sub fillItemRelatedDataFromDocDsgnr(argDocID As Integer, argItemID As Long, argFrm As Object)

        Dim i As Integer, strResult As String
        Dim strLoadFrom As String
        Dim strFldBlnComboData As String, strFldCmbTblName As String
        Dim strFldCmbStorageFldName As String, strFldCmbDisplayCols As String, strFldCmbAddnlCond As String
        Dim strFirstDispCol As String
        '# A - To load based on the load data settings
        Dim strCustSupp As String, strTblName As String
        Try
            strCustSupp = clsSVT.getDataStringValue("Select CustSuppReq from UTLTransactions where Docid =" & argDocID, "", False)

            strTblName = Microsoft.VisualBasic.Switch(strCustSupp = "C", "MstCustItem", strCustSupp = "S", "MstSuppItem", strCustSupp = "", "")

            strLoadFrom = clsSVT.getDataStringValue("Select FldBlnLoadFrom from UtlDocDsgrFlds where FldLoadFromType = 'I' and FldBlnLoadFrom = 'Y' and DocID = " & argDocID, "N", False)

            '    'for loading pickup field values
            '    For x = 0 To argFrm.VsfOptions.Rows - 1
            '        If Trim(argFrm.VsfOptions.GetData(x, vOthFldPickUPAfterFld)) <> "" Then
            '            If Trim(Replace(argFrm.VsfOptions.GetData(x, vOthFldPickUPAfterFld), "#", "")) = "ITEMCODE" Then
            '                strSQL = Replace(Trim(argFrm.VsfOptions.GetData(x, vOthFldPickUP)), "`", "'")
            '                If strSQL <> "" Then
            '                    For i = 0 To argFrm.VsfOptions.Rows - 1
            '                        strVal = Trim(argFrm.vsfDet.GetData(argFrm.vsfDet.Row, i))
            '                        If i + 1 = argFrm.vsfDet.Cols Then
            '                            strVal = Trim(argFrm.vsfDet.EditText)
            '                        End If
            '                        strSQL = Replace(strSQL, "<<" & i + 1 & ">>", strVal)
            '                    Next i
            '                        strSQL = ReplSVTExpWithConstantOrValues(strSQL, "TEXT", "T", argFrm, CInt(argFrm.vsfDet.Col), CInt(argFrm.vsfDet.Row))
            '                        argFrm.vsfDet.GetData(argFrm.vsfDet.Row, x) = getRsVal(strSQL)
            '                End If
            '            End If
            '        End If
            '    Next x

            If strLoadFrom = "N" Then Exit Sub

            With argFrm.VsfOptions
                For i = 1 To .Rows.Count - 1
                    If .getdata(i, vOthFldBlnLoadFrom) = "Y" Then
                        If .getdata(i, vOthFldLoadFromType) = "I" Then
                            If UCase(Mid(.getdata(i, vOthFldLoadFldName), 1, 2)) = "C." Or UCase(Mid(.getdata(i, vOthFldLoadFldName), 1, 2)) = "S." Then
                                strResult = clsSVT.getDataStringValue("Select " & .getdata(i, vOthFldLoadFldName) & _
                                   " from MstItem i, MstItemGroup g, MstUOM u" & _
                                   IIf(strTblName <> "", "," & strTblName & " " & strCustSupp, "") & _
                                   " Where i.ItemGroupID = g.ItemGroupID and " & _
                                   " i.UOMID = u.UOMID and " & _
                                   " i.ItemID = " & argItemID & _
                                   IIf(strTblName <> "", " and i.ItemID = " & strCustSupp & ".ItemID", ""), "", False)
                            Else
                                strResult = clsSVT.getDataStringValue( _
                                    "Select " & .getdata(i, vOthFldLoadFldName) & _
                                    " from MstItem i, MstItemGroup g, MstUOM u " & _
                                    " Where i.ItemGroupID = g.ItemGroupID and " _
                                    & " i.UOMID = u.UOMID and " _
                                    & " i.ItemID = " & argItemID, "", False)
                            End If
                            If UCase(Mid(Trim(.GetData(i, vOthFldLoadFldName)), 1, 1)) = "I" Then
                                'find if it is addnlfld, and if it is loaded from combo
                                'example i.LedID
                                '88888888888888888888888888888888888888
                                strFldBlnComboData = clsSVT.getDataStringValue("Select FldBlnComboData from UtlDocDsgrFlds " & _
                                        "where DocID = 2 and FldCode = '" & UCase(Mid(Trim(.getdata(i, vOthFldLoadFldName)), 3)) & "'", "N", False)
                                If strFldBlnComboData = "Y" Then
                                    strFldCmbTblName = clsSVT.getDataStringValue("Select FldCmbTblName from UtlDocDsgrFlds" & _
                                        " where DocID = 2 and FldCode = '" & UCase(Mid(Trim(.getdata(i, vOthFldLoadFldName)), 3)) & "'", "N", False)
                                    strFldCmbStorageFldName = clsSVT.getDataStringValue("Select FldCmbStorageFldName from UtlDocDsgrFlds" & _
                                        " where DocID = 2 and FldCode = '" & UCase(Mid(Trim(.getdata(i, vOthFldLoadFldName)), 3)) & "'", "N", False)

                                    strFldCmbDisplayCols = clsSVT.getDataStringValue("Select FldCmbDisplayCols from UtlDocDsgrFlds" & _
                                        " where DocID = 2 and FldCode = '" & UCase(Mid(Trim(.getdata(i, vOthFldLoadFldName)), 3)) & "'", "N", False)

                                    strFldCmbAddnlCond = clsSVT.getDataStringValue("Select FldCmbAddnlCond from UtlDocDsgrFlds" & _
                                        " where DocID = 2 and FldCode = '" & UCase(Mid(Trim(.getdata(i, vOthFldLoadFldName)), 3)) & "'", "N", False)

                                    If strFldCmbTblName <> "" Then
                                        If InStr(1, strFldCmbDisplayCols, ",") > 0 Then
                                            strFirstDispCol = Microsoft.VisualBasic.Left(strFldCmbDisplayCols, InStr(1, strFldCmbDisplayCols, ",") - 1)
                                        Else
                                            strFirstDispCol = strFldCmbDisplayCols
                                        End If
                                        strResult = clsSVT.getDataStringValue("Select " & strFirstDispCol & _
                                            " from " & strFldCmbTblName & " where " & strFldCmbStorageFldName & " = " & Val(strResult), "", False)
                                        If .getdata(i, vOthFldDataType) = "Text" Then
                                            strResult = Mid(strResult, 1, Val(.getdata(i, vOthFldDataWidth)))
                                        End If
                                    End If
                                End If
                            End If
                            argFrm.vsfDet.setdata(argFrm.vsfDet.Row, i, strResult)
                        End If
                    End If
                Next i
            End With
            Exit Sub
        Catch ex As Exception
            MsgBox(ex.Message, vbInformation, argFrm.Text)
        End Try
    End Sub

    Public Sub getUtlDetails(ByVal argDocID As Int32)
        strPartyType = ""
        Dim rsDummy As oledb.oledbDataReader
        rsDummy = clsSVT.getDataReaderFromSQL("Select * from UtlTransactions where DocID = " & argDocID)

        With rsDummy
            If Not .HasRows Then Exit Sub
            .Read()

            intPartyDocID = GetVal(!MstDocIDParty, 0)

            If intPartyDocID > 0 Then
                Dim strPartyShForm As String
                strPartyShForm = clsSVT.getDataStringValue("Select DocShForm From UtlTransactions where DocID = " & Val(intPartyDocID), "", False)
                strPartyType = strPartyShForm
                strMstTblName = "Mst" & strPartyShForm
                strMstFldId = strPartyShForm & "ID"
                strMstFldCode = strPartyShForm & "Code"
                strMstFldName = strPartyShForm & "Name"
                strPartyFilterCond = clsSVT.getDataStringValue("Select PartyFilterCond From UtlTransactions where DocID = " & Val(intPartyDocID), "", False)
                Select Case strPartyShForm
                    Case "CUST"
                        strPartyFlds = gCustQryFlds
                    Case "SUPP"
                        strPartyFlds = gSuppQryFlds
                End Select
            End If

            strPartyType = !CustSuppReq
            If strPartyType = "" Then Exit Sub
            Select Case !CustSuppReq
                Case "C"
                    strMstTblName = "MstCust"
                    strMstFldId = "CustID"
                    strMstFldCode = "CustCode"
                Case "S"
                    strMstTblName = "MstSupp"
                    strMstFldId = "SuppID"
                    strMstFldCode = "SuppCode"
            End Select
            strSHForm = GetVal(!DocShForm, "")
            strDocNoFldName = strSHForm & "No"
            strDocHdrTblName = "TrnHdr" & strSHForm
            strDocDtlTblName = "TrnDtl" & strSHForm
            sngDefQty = GetVal(!DefaultQty, 0)
            strItemQueryPresent = !BlnItemQuery
            strItemQuery = ""
            If strItemQueryPresent = "Y" Then strItemQuery = !ItemQuery
        End With
    End Sub

    Private Sub vsfCustItemList_CellButtonClick(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfCustItemList.CellButtonClick
        With vsfCustItemList
            Select Case e.Col
                Case vCIDItemCode
                    e.Cancel = True
                    fraSearch.Top = vsfCustItemList.Top + vsfCustItemList.Rows(e.Row).Top + (vsfCustItemList.Rows(e.Row).Height) + 20
                    fraSearch.Left = vsfCustItemList.Left + vsfCustItemList.Cols(e.Col).Left
                    fraSearch.BringToFront()
                    fraSearch.Visible = True


                    vsfSearch.Col = 0
                    vsfSearch.LeftCol = 0
                    vsfSearch.Text = ""
                    vsfSearch.Focus()
                    vsfSearch.Cols.Count = vsfView.Cols.Count
                Case Else
                    Exit Sub
            End Select
        End With
    End Sub

    Private Sub vsfView_AfterResizeColumn(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfView.AfterResizeColumn
        Call syncGridCols()
    End Sub

    Private Sub vsfView_BeforeEdit(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfView.BeforeEdit
        e.Cancel = True
    End Sub

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

    Private Sub vsfView_AfterScroll(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.RangeEventArgs) Handles vsfView.AfterScroll
        vsfSearch.ScrollPosition = vsfView.ScrollPosition
    End Sub

    Private Sub vsfSearch_AfterScroll(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.RangeEventArgs) Handles vsfSearch.AfterScroll
        vsfView.ScrollPosition = vsfSearch.ScrollPosition
    End Sub

    Private Sub vsfSearch_ChangeEdit(ByVal sender As Object, ByVal e As System.EventArgs) Handles vsfSearch.ChangeEdit
        Call applyFilterFromGrid()
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
                    StrQry = StrQry & vsfView.GetData(0, i) & " like '*" & strValue & "*'"
                End If
            Next i
        End With
        dvSearch.RowFilter = StrQry
        Call syncGridCols()
        applyFilterFromGrid = True
    End Function

    Private Sub vsfView_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles vsfView.DoubleClick
        If vsfView.Row > 0 Then
            Call loadItemToDtl()
        End If
    End Sub

    Private Sub vsfView_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles vsfView.KeyUp
        If e.KeyCode = Keys.Escape Then
            Call closefraComboItem(False)
        End If
        If vsfView.Row > 0 Then
            If e.KeyCode = Keys.Return Or e.KeyCode = Keys.Tab Then
                Call loadItemToDtl()
            End If
        End If

    End Sub

    Private Sub loadItemToDtl()
        If Not FillItemDet(vsfView.GetData(vsfView.Row, 0), vsfCustItemList.Row) Then
            MsgBox("Invalid Item!!", vbInformation, Me.Text)
            Exit Sub
        End If
        Call closefraComboItem(True)
    End Sub

    Private Sub closefraComboItem(ByVal argSetFocus As Boolean)
        vsfView.Clear()
        vsfSearch.Clear()
        dvSearch.RowFilter = ""
        vsfCustItemList.Enabled = True
        fraSearch.Visible = False
        vsfCustItemList.Focus()
        If argSetFocus Then
            vsfCustItemList.Col = vCIDSUOM
        End If
    End Sub

    Private Sub vsfSearch_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles vsfSearch.KeyUp
        If e.KeyCode = Keys.Down Then
            vsfView.Focus()
            If vsfView.Rows.Count > 1 Then
                vsfView.Row = 1
            End If
        End If
        If e.KeyCode = Keys.Escape Then
            Call closefraComboItem(False)
        End If

    End Sub

    Private Sub vsfSearch_KeyUpEdit(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.KeyEditEventArgs) Handles vsfSearch.KeyUpEdit
        If e.KeyCode = Keys.Down Then
            vsfView.Focus()
            If vsfView.Rows.Count > 1 Then
                vsfView.Row = 1
            End If
        End If
        If e.KeyCode = Keys.Escape Then
            Call closefraComboItem(False)
        End If

    End Sub

    Private Sub vsfSearch_RowColChange(ByVal sender As Object, ByVal e As System.EventArgs) Handles vsfSearch.RowColChange
        vsfView.Col = vsfSearch.Col
        vsfView.Refresh()
    End Sub

    Public Function FillItemDet(ByVal argItem As String, ByVal argRow As Long, Optional ByVal rowIndex As Integer = -1) As Boolean
        Dim rsDummy As oledb.oledbDataReader
        Dim strSQL As String
        Dim blnInActive As Boolean
        FillItemDet = False

        If rowIndex = -1 Then
            rowIndex = vsfCustItemList.Row
        End If

        With vsfCustItemList
            If argItem = "" Then
                Exit Function
            End If

            intItemId = clsSVT.getDataLongValue("Select ItemID from MstItem where ItemCode = '" & Trim(argItem) & "'", 0, blnInActive)
            If intItemId = 0 Then
                MsgBox("Item code '" & argItem & "' does not exist!", vbInformation, Me.Text)
                Exit Function
            End If

            Call clsSVT.getDataLongValue("Select ItemID from MstItem where InActive = 'Y' and ItemCode = '" & Trim(argItem) & "'", 0, blnInActive)

            If blnInActive Then
                MsgBox("Item code '" & argItem & "' is inactive! Please select an active item from the list.", vbInformation, Me.Text)
                Exit Function
            End If

            strSQL = "Select ItemCode, UOMCode from "
            strSQL = strSQL & "MstItem i, MstUom u"
            strSQL = strSQL & "  where i.UomID = u.UomID and "
            strSQL = strSQL & " ItemCode = '" & Trim(argItem) & "'"

            rsDummy = clsSVT.getDataReaderFromSQL(strSQL)

            With rsDummy
                If Not .HasRows Then
                    MsgBox("Item does not exist!!", vbInformation, Me.Text)
                    Exit Function
                End If
                .Read()
                vsfCustItemList.SetData(rowIndex, vCIItemCode, rsDummy("ItemCode"))
                If GetVal(vsfCustItemList.GetData(rowIndex, vCIQty), 0) = 0 Then
                    vsfCustItemList.SetData(rowIndex, vCIQty, sngDefQty)
                End If
                vsfCustItemList.SetData(rowIndex, vCISUOM, !UOMCode)
                .Close()
            End With
        End With
        FillItemDet = True
    End Function

    Private Sub initItemVsfCombo()
        Dim strSQL As String
        strSQL = getItemSearchGridQuery(strItemQuery)

        Call showWaitCursor()
        dtSearch = clsSVT.getDATATable(strSQL)
        dvSearch = New DataView(dtSearch)

        vsfView.DataSource = dvSearch
        Call showNormalCursor()

        vsfSearch.Cols.Count = vsfCustItemList.Cols.Count

    End Sub

    Private Sub btnCloseSearch_Click(sender As System.Object, e As System.EventArgs) Handles btnCloseSearch.Click
        fraSearch.Visible = False
    End Sub

    Private Sub lblHelp_Click(sender As Object, e As EventArgs) Handles lblHelp.Click

    End Sub
End Class
