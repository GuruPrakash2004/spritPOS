Imports C1.Win.C1FlexGrid
Imports System.Data
Imports System.Data.OleDb
Imports System.Windows.Forms
Imports System.Text.RegularExpressions

Module DsgrModule

    '# A - Consts for Voucher Credit and Debit COlumn
    Const gConstDebitCol = 4
    Const gConstCreditCol = 5

    Public Const vOthFldCaption As Byte = 0
    Public Const vOthFldCode As Byte = 1
    Public Const vOthFldDataType As Byte = 2
    Public Const vOthFldDataWidth As Byte = 3
    Public Const vOthFldDataPer As Byte = 4
    Public Const vOthFldDataValue As Byte = 5
    Public Const vOthFldBlnReadOnly As Byte = 6
    Public Const vOthFLdBlnHide As Byte = 7
    Public Const vOthFldBlnMandatory As Byte = 8
    Public Const vOthFldUnique As Byte = 9
    Public Const vOthFldUseDefault As Byte = 10
    Public Const vOthFldRowClrFontBold As Byte = 11
    Public Const vOthFldRowClrForeColour As Byte = 12
    Public Const vOthFldBlnComboData As Byte = 13
    Public Const vOthFldBlnRefill As Byte = 14
    Public Const vOthFldOrderBy As Byte = 15
    Public Const vOthFldFormula As Byte = 16
    Public Const vOthFldBlnDispOnly As Byte = 17
    Public Const vOthFldNumBlnAnyNumber As Byte = 18
    Public Const vOthFldNumBlnPercentage As Byte = 19
    Public Const vOthFldNumDecimals As Byte = 20
    Public Const vOthFldNumBlnAllowNegative As Byte = 21
    Public Const vOthFldNumBlnRange As Byte = 22
    Public Const vOthFldNumBlnRangeFromChk As Byte = 23
    Public Const vOthFldNumRangeFromOperator As Byte = 24
    Public Const vOthFldNumRangeFromValue As Byte = 25
    Public Const vOthFldNumBlnRangeToChk As Byte = 26
    Public Const vOthFldNumRangeToOperator As Byte = 27
    Public Const vOthFldNumRangeToValue As Byte = 28
    Public Const vOthFldBlnNumOrStringList As Byte = 29
    Public Const vOthFldNumStringListContent As Byte = 30
    Public Const vOthFldDtBlnAnyDate As Byte = 31
    Public Const vOthFldDtBlnWithinFinYear As Byte = 32
    Public Const vOthFldDtBlnRange As Byte = 33
    Public Const vOthFldDtBlnRangeFrom As Byte = 34
    Public Const vOthFldDtRangeFromOperator As Byte = 35
    Public Const vOthFldDtRangeFromValue As Byte = 36
    Public Const vOthFldDtBlnRangeTo As Byte = 37
    Public Const vOthFldDtRangeToOperator As Byte = 38
    Public Const vOthFldDtRangeToValue As Byte = 39
    Public Const vOthFldDtBlnAsEntered As Byte = 40
    Public Const vOthFldDtBlnFirstDOM As Byte = 41
    Public Const vOthFldDtBlnLastDOM As Byte = 42
    Public Const vOthFldTxtStringType As Byte = 43
    Public Const vOthFldTxtCase As Byte = 44
    Public Const vOthFldTxtBlnDoNotAllowChk As Byte = 45
    Public Const vOthFldTxtDoNotAllowChars As Byte = 46
    Public Const vOthFldTxtBlnMask As Byte = 47
    Public Const vOthFldTxtMaskChar As Byte = 48
    Public Const vOthFldBlnRowClr As Byte = 49
    Public Const vOthFldBlnFontBold As Byte = 50
    Public Const vOthFldHideCompareVal As Byte = 51
    Public Const vOthFldHideOperator As Byte = 52
    Public Const vOthFldHideWithVal As Byte = 53
    Public Const vOthFldMndtryCompareVal As Byte = 54
    Public Const vOthFldMndtryOperator As Byte = 55
    Public Const vOthFldMndtryWithVal As Byte = 56
    Public Const vOthFldROCompareVal As Byte = 57
    Public Const vOthFldROOperator As Byte = 58
    Public Const vOthFldROWithVal As Byte = 59
    Public Const vOthFldCmbTblName As Byte = 60
    Public Const vOthFldCmbStorageFldName As Byte = 61
    Public Const vOthFldCmbDisplayCols As Byte = 62
    Public Const vOthFldCmbAddnlCond As Byte = 63
    Public Const vOthFldDefStdDefault As Byte = 64
    Public Const vOthFldDefBlnAgnstCond As Byte = 65
    Public Const vOthFldDefCondList As Byte = 66
    Public Const vOthFldTxtBlnMultiLine As Byte = 67
    Public Const vOthFldToolTip As Byte = 68
    Public Const vOthFldUniqueBasedOn As Byte = 69
    Public Const vOthFldTxtAlignment As Byte = 70
    Public Const vOthFldBlnLoadFrom As Byte = 71 '# A - Code for load from
    Public Const vOthFldLoadFldName As Byte = 72
    Public Const vOthFldLoadFromType As Byte = 73
    Public Const vOthFldClrBefHide As Byte = 74
    Public Const vOthFldPickUP As Byte = 75
    Public Const vOthFldPickUPAfterFld As Byte = 76
    Public Const vOthFldBlnApportinate As Byte = 77
    Public Const vOthFLdApportinate As Byte = 78
    Public Const vOthFldBasedOn As Byte = 79
    Public Const vOthFldFontName As Byte = 80
    Public Const vOthFldFontSize As Byte = 81
    Public Const vOthBlnCopyPreviousRowData As Byte = 82
    Public Const vOthDocumentValue As Byte = 83
    Public Const vOthSeparator As Byte = 84
    Public Const vOthPerAmount As Byte = 85
    Public Const vOthBlnLoadColFrom As Byte = 86
    Public Const vOthFldLoadColFrom As Byte = 87
    Public Const vOthBlnTogHide = 88
    Public Const vOthFldBlnNewTrnCalc = 89
    Public Const vOthFldBlnChkDuplicate = 90
    Public Const vOthBlnTotalReq = 91
    Public Const vOthBlnLoadPrevTrnData = 92
    Public Const vOthBlnLoadPrevTrnParty = 93
    Public Const vOthBlnLoadPrevTrnItem = 94
    Public Const vOthRoundType = 95
    Public Const vOthCSVFieldName = 96
    Public Const vOthFldPrevTranFldName = 97
    Public Const vOthFldMultiAmtFld = 98
    Public Const vOthFldMultiAmtVal = 99
    Private intTotCols As Integer = 100

    Dim clsSvtLib As New svtLibrary

    Public Function FillOptionBTNDet(ByVal argFrm As Object, ByVal argDOCID As Integer) As List(Of String)
        Dim strSQL As String
        Dim rs As OleDbDataReader
        Dim Ilist As New List(Of String)
        strSQL = "select * from UTLOption where DocID = " & argDOCID & " order by optionOrder"

        rs = clsSvtLib.getDataReaderFromSQL(strSQL)

        If Not rs.HasRows Then
            rs.Close()
            Exit Function
        End If
        '------------

        With rs
            Do While rs.Read
                If GetVal(!OptionName, "") <> "" Then Ilist.Add(!OptionName)
            Loop
        End With

        rs.Close()
        FillOptionBTNDet = Ilist
    End Function

    Public Sub InitAddnlFldGrids(ByVal argFrm As Object, ByVal argVsf As C1FlexGrid, ByVal argSection As String, ByVal intDocID As Integer,
                                        Optional ByVal argTabObj As TabControl = Nothing)
        Dim i As Integer, x As Integer
        Dim rs As OleDbDataReader
        Dim rsSubSection As OleDbDataReader
        Dim intRowsHidden As Integer

        Dim strSQL As String
        'Dim cs As CellStyle = argVsf.Styles.Add("bold")
        'cs.Font = New Font("Microsoft Sans Serif", 8.25, FontStyle.Bold)
        'Dim csWhite As CellStyle = argVsf.Styles.Add("WHITE")
        'csWhite.ForeColor = Color.White

        strSQL = "Select distinct ltrim(rtrim(SubSectionName)) as sName"
        strSQL &= " from UtlDocDsgrFlds where DocID = " & intDocID
        strSQL &= " and FldSection = '" & argSection & "'"
        rsSubSection = clsSvtLib.getDataReaderFromSQL(strSQL)
        x = 0

        If Not IsNothing(argTabObj) Then
            argTabObj.Visible = False
beginRemove:
            For i = 1 To argTabObj.TabPages.Count - 1
                argTabObj.TabPages.RemoveAt(i)
                GoTo beginRemove
            Next i
        End If
        argVsf.Rows.Count = 0
        argVsf.Visible = False

        If rsSubSection.HasRows Then

            Do While rsSubSection.Read()

                If Not IsNothing(argTabObj) Then
                    argTabObj.Visible = True
                End If

                x = x + 1
                Dim argDVsf As New C1FlexGrid()
                argDVsf.Tag = "DSGR"
                argDVsf.ExtendLastCol = True
                ' argDVsf.
                If x = 1 Then
                    argDVsf = argVsf

                    argDVsf.Cols(0).Width = argVsf.Cols(0).Width
                    If Not IsNothing(argTabObj) Then
                        argTabObj.TabPages(0).Text = GetVal(rsSubSection!SName, "")
                        If argTabObj.TabPages(0).Text = "" Then
                            argTabObj.TabPages(0).Text = "Header Info"
                        End If
                    End If
                Else
                    If Not IsNothing(argTabObj) Then
                        Dim mTabPage As New TabPage
                        mTabPage.Text = GetVal(rsSubSection!SName, "")
                        argTabObj.TabPages.Add(mTabPage)
                        mTabPage.Controls.Add(argDVsf)
                        argDVsf.Dock = DockStyle.Fill
                        Select Case UCase(argSection)
                            Case "HEADER"
                                argDVsf.Name = "VSFHDR" & x
                            Case "FOOTER"
                                argDVsf.Name = "VSFFTR" & x
                            Case "TOTAL"
                                argDVsf.Name = "VSFTOT" & x
                        End Select


                        If UCase(argVsf.Name) = "VSFHDR" Then
                            AddHandler argDVsf.BeforeEdit, AddressOf argFrm.vsfHdr_BeforeEdit
                            AddHandler argDVsf.ValidateEdit, AddressOf argFrm.vsfHdr_ValidateEdit
                            AddHandler argDVsf.KeyPressEdit, AddressOf argFrm.vsfHdr_KeyPressEdit
                            AddHandler argDVsf.AfterEdit, AddressOf argFrm.vsfHdr_AfterEdit
                            AddHandler argDVsf.SetupEditor, AddressOf argFrm.vsfHdr_SetupEditor
                            AddHandler argDVsf.OwnerDrawCell, AddressOf argFrm.vsfHdr_OwnerDrawCell
                            AddHandler argDVsf.CellButtonClick, AddressOf argFrm.vsfHdr_cellButtonClick
                            AddHandler argDVsf.MouseClick, AddressOf argFrm.vsfHdr_MouseClick
                            AddHandler argDVsf.MouseDoubleClick, AddressOf argFrm.vsfHdr_MouseDoubleClick
                        ElseIf UCase(argVsf.Name) = "VSFFTR" Then
                            AddHandler argDVsf.BeforeEdit, AddressOf argFrm.vsfFtr_BeforeEdit
                            AddHandler argDVsf.ValidateEdit, AddressOf argFrm.vsfFtr_ValidateEdit
                            AddHandler argDVsf.KeyPressEdit, AddressOf argFrm.vsfFtr_KeyPressEdit
                            AddHandler argDVsf.AfterEdit, AddressOf argFrm.vsfFtr_AfterEdit
                            AddHandler argDVsf.SetupEditor, AddressOf argFrm.vsfFtr_SetupEditor
                            AddHandler argDVsf.OwnerDrawCell, AddressOf argFrm.vsfFtr_OwnerDrawCell
                            AddHandler argDVsf.CellButtonClick, AddressOf argFrm.vsfFtr_cellButtonClick
                            AddHandler argDVsf.MouseClick, AddressOf argFrm.vsfFtr_MouseClick
                            AddHandler argDVsf.MouseDoubleClick, AddressOf argFrm.vsfFtr_MouseDoubleClick
                        ElseIf UCase(argVsf.Name) = "VSFTOT" Then
                            AddHandler argDVsf.BeforeEdit, AddressOf argFrm.vsftot_BeforeEdit
                            AddHandler argDVsf.ValidateEdit, AddressOf argFrm.vsftot_ValidateEdit
                            AddHandler argDVsf.KeyPressEdit, AddressOf argFrm.vsftot_KeyPressEdit
                            AddHandler argDVsf.AfterEdit, AddressOf argFrm.vsftot_AfterEdit
                            AddHandler argDVsf.SetupEditor, AddressOf argFrm.vsftot_SetupEditor
                            AddHandler argDVsf.OwnerDrawCell, AddressOf argFrm.vsfTot_OwnerDrawCell
                            AddHandler argDVsf.MouseClick, AddressOf argFrm.vsfTot_MouseClick
                        End If

                    Else
                        argDVsf = argVsf
                    End If
                End If
                argDVsf.BeginUpdate()
                With argDVsf
                    .Cols.Count = intTotCols

                    .Cols(0).Width = 200
                    '.AutoSizeMode = flexAutoSizeRowHeight
                    '.WordWrap = True
                    .AutoResize = True
                    .Cols(vOthFldDataValue).TextAlign = TextAlignEnum.LeftCenter
                    .Cols(vOthFldDataPer).TextAlign = TextAlignEnum.LeftCenter
                    .Rows.Count = 0

                    For i = 0 To .Cols.Count - 1
                        .Cols(i).Visible = False
                    Next i

                    .Cols(vOthFldDataValue).Visible = True
                    .Cols(vOthFldDataPer).Visible = False
                    .Cols(vOthFldCaption).Visible = True
                End With

                strSQL = "Select d.* from vwAddnlFldsHFTSec d, UtlTransactions t "
                strSQL = strSQL & " where d.DocId = t.DocID "
                strSQL = strSQL & " and t.DocId=" & intDocID
                strSQL = strSQL & " and ltrim(rtrim(subsectionname)) = '" & rsSubSection!SName & "'"
                strSQL = strSQL & " and FldSection = '" & argSection & "'"
                strSQL = strSQL & " order by FldOrderBy"
                rs = clsSvtLib.getDataReaderFromSQL(strSQL)

                If Not rs.HasRows Then
                    rs.Close()
                    Exit Sub
                End If
                '------------

                With rs
                    Do While .Read()
                        argDVsf.Rows.Add()
                        i = argDVsf.Rows.Count - 1
                        argDVsf.SetData(i, vOthFldCode, GetVal(rs("FldCode"), ""))
                        If GetVal(rs("FldCmbTblName"), "") <> "" Then
                            argDVsf.SetData(i, vOthFldCaption, GetVal(rs("FldCaption"), "") & " [Mst]")
                        Else
                            argDVsf.SetData(i, vOthFldCaption, GetVal(rs("FldCaption"), ""))
                        End If
                        argDVsf.SetData(i, vOthFldDataType, GetVal(rs("FldDataType"), ""))
                        argDVsf.SetData(i, vOthFldDataWidth, GetVal(rs("FldDataWidth"), ""))
                        argDVsf.SetData(i, vOthFldBlnReadOnly, GetVal(rs("FldBlnReadOnly"), ""))
                        argDVsf.SetData(i, vOthFLdBlnHide, GetVal(rs("FldBlnHide"), ""))
                        argDVsf.SetData(i, vOthFldClrBefHide, GetVal(rs("FldClrBefHide"), ""))
                        argDVsf.SetData(i, vOthFldBlnMandatory, GetVal(rs("FldBlnMandatory"), ""))
                        argDVsf.SetData(i, vOthFldUnique, GetVal(rs("FldUnique"), ""))
                        argDVsf.SetData(i, vOthFldUseDefault, GetVal(rs("FldUseDefault"), ""))
                        argDVsf.SetData(i, vOthFldRowClrFontBold, GetVal(rs("FldRowClrFontBold"), ""))
                        argDVsf.SetData(i, vOthFldRowClrForeColour, GetVal(rs("FldRowClrForeColour"), ""))
                        argDVsf.SetData(i, vOthFldBlnComboData, GetVal(rs("FldBlnComboData"), ""))
                        argDVsf.SetData(i, vOthFldBlnRefill, GetVal(rs("FldBlnRefill"), ""))
                        argDVsf.SetData(i, vOthFldOrderBy, GetVal(rs("FldOrderBy"), ""))
                        argDVsf.SetData(i, vOthFldFormula, GetVal(rs("FldFormula"), ""))
                        argDVsf.SetData(i, vOthFldBlnDispOnly, GetVal(rs("FldBlnDispOnly"), ""))
                        argDVsf.SetData(i, vOthFldNumBlnAnyNumber, GetVal(rs("FldNumBlnAnyNumber"), ""))
                        argDVsf.SetData(i, vOthFldNumBlnPercentage, GetVal(rs("FldNumBlnPercentage"), ""))
                        argDVsf.SetData(i, vOthFldNumDecimals, GetVal(rs("FldNumDecimals"), ""))
                        argDVsf.SetData(i, vOthFldNumBlnAllowNegative, GetVal(rs("FldNumBlnAllowNegative"), ""))
                        argDVsf.SetData(i, vOthFldNumBlnRange, GetVal(rs("FldNumBlnRange"), ""))
                        argDVsf.SetData(i, vOthFldNumBlnRangeFromChk, GetVal(rs("FldNumBlnRangeFromChk"), ""))
                        argDVsf.SetData(i, vOthFldNumRangeFromOperator, GetVal(rs("FldNumRangeFromOperator"), ""))
                        argDVsf.SetData(i, vOthFldNumRangeFromValue, GetVal(rs("FldNumRangeFromValue"), ""))
                        argDVsf.SetData(i, vOthFldNumBlnRangeToChk, GetVal(rs("FldNumBlnRangeToChk"), ""))
                        argDVsf.SetData(i, vOthFldNumRangeToOperator, GetVal(rs("FldNumRangeToOperator"), ""))
                        argDVsf.SetData(i, vOthFldNumRangeToValue, GetVal(rs("FldNumRangeToValue"), ""))
                        argDVsf.SetData(i, vOthFldBlnNumOrStringList, GetVal(rs("FldBlnNumOrStringList"), ""))
                        argDVsf.SetData(i, vOthFldNumStringListContent, GetVal(rs("FldNumStringListContent"), ""))
                        argDVsf.SetData(i, vOthFldDtBlnAnyDate, GetVal(rs("FldDtBlnAnyDate"), ""))
                        argDVsf.SetData(i, vOthFldDtBlnWithinFinYear, GetVal(rs("FldDtBlnWithinFinYear"), ""))
                        argDVsf.SetData(i, vOthFldDtBlnRange, GetVal(rs("FldDtBlnRange"), ""))
                        argDVsf.SetData(i, vOthFldDtBlnRangeFrom, GetVal(rs("FldDtBlnRangeFrom"), ""))
                        argDVsf.SetData(i, vOthFldDtRangeFromOperator, GetVal(rs("FldDtRangeFromOperator"), ""))
                        argDVsf.SetData(i, vOthFldDtRangeFromValue, GetVal(rs("FldDtRangeFromValue"), ""))
                        argDVsf.SetData(i, vOthFldDtBlnRangeTo, GetVal(rs("FldDtBlnRangeTo"), ""))
                        argDVsf.SetData(i, vOthFldDtRangeToOperator, GetVal(rs("FldDtRangeToOperator"), ""))
                        argDVsf.SetData(i, vOthFldDtRangeToValue, GetVal(rs("FldDtRangeToValue"), ""))
                        argDVsf.SetData(i, vOthFldDtBlnAsEntered, GetVal(rs("FldDtBlnAsEntered"), ""))
                        argDVsf.SetData(i, vOthFldDtBlnFirstDOM, GetVal(rs("FldDtBlnFirstDOM"), ""))
                        argDVsf.SetData(i, vOthFldDtBlnLastDOM, GetVal(rs("FldDtBlnLastDOM"), ""))
                        argDVsf.SetData(i, vOthFldTxtStringType, GetVal(rs("FldTxtStringType"), ""))
                        argDVsf.SetData(i, vOthFldTxtCase, GetVal(rs("FldTxtCase"), ""))
                        argDVsf.SetData(i, vOthFldTxtBlnDoNotAllowChk, GetVal(rs("FldTxtBlnDoNotAllowChk"), ""))
                        argDVsf.SetData(i, vOthFldTxtDoNotAllowChars, GetVal(rs("FldTxtDoNotAllowChars"), ""))
                        argDVsf.SetData(i, vOthFldTxtBlnMask, GetVal(rs("FldTxtBlnMask"), ""))
                        argDVsf.SetData(i, vOthFldTxtMaskChar, GetVal(rs("FldTxtMaskChar"), ""))
                        argDVsf.SetData(i, vOthFldBlnRowClr, GetVal(rs("FldBlnRowClr"), ""))
                        argDVsf.SetData(i, vOthFldBlnFontBold, GetVal(rs("FldBlnFontBold"), ""))
                        argDVsf.SetData(i, vOthFldHideCompareVal, GetVal(rs("FldHideCompareVal"), ""))
                        argDVsf.SetData(i, vOthFldHideOperator, GetVal(rs("FldHideOperator"), ""))
                        argDVsf.SetData(i, vOthFldHideWithVal, GetVal(rs("FldHideWithVal"), ""))
                        argDVsf.SetData(i, vOthFldMndtryCompareVal, GetVal(rs("FldMndtryCompareVal"), ""))
                        argDVsf.SetData(i, vOthFldMndtryOperator, GetVal(rs("FldMndtryOperator"), ""))
                        argDVsf.SetData(i, vOthFldMndtryWithVal, GetVal(rs("FldMndtryWithVal"), ""))
                        argDVsf.SetData(i, vOthFldROCompareVal, GetVal(rs("FldROCompareVal"), ""))
                        argDVsf.SetData(i, vOthFldROOperator, GetVal(rs("FldROOperator"), ""))
                        argDVsf.SetData(i, vOthFldROWithVal, GetVal(rs("FldROWithVal"), ""))
                        argDVsf.SetData(i, vOthFldCmbTblName, GetVal(rs("FldCmbTblName"), ""))
                        argDVsf.SetData(i, vOthFldCmbStorageFldName, GetVal(rs("FldCmbStorageFldName"), ""))
                        argDVsf.SetData(i, vOthFldCmbDisplayCols, GetVal(rs("FldCmbDisplayCols"), ""))
                        argDVsf.SetData(i, vOthFldCmbAddnlCond, GetVal(rs("FldCmbAddnlCond"), ""))
                        argDVsf.SetData(i, vOthFldDefStdDefault, GetVal(rs("FldDefStdDefault"), ""))
                        argDVsf.SetData(i, vOthFldDefBlnAgnstCond, GetVal(rs("FldDefBlnAgnstCond"), ""))
                        argDVsf.SetData(i, vOthFldDefCondList, GetVal(rs("FldDefCondList"), ""))
                        argDVsf.SetData(i, vOthFldTxtBlnMultiLine, GetVal(rs("FldTxtBlnMultiLine"), ""))
                        argDVsf.SetData(i, vOthFldToolTip, GetVal(rs("FldToolTip"), ""))
                        argDVsf.SetData(i, vOthFldUniqueBasedOn, GetVal(rs("FldUniqueBasedOn"), ""))
                        argDVsf.SetData(i, vOthFldTxtAlignment, GetVal(rs("FldTxtAlignment"), ""))
                        argDVsf.SetData(i, vOthFldBlnLoadFrom, GetVal(rs("FldBlnLoadFrom"), ""))
                        argDVsf.SetData(i, vOthFldLoadFldName, GetVal(rs("FldLoadFldName"), ""))
                        argDVsf.SetData(i, vOthFldLoadFromType, GetVal(rs("FldLoadFromType"), ""))
                        argDVsf.SetData(i, vOthFldPickUP, GetVal(rs("FldPickUP"), ""))
                        argDVsf.SetData(i, vOthFldPickUPAfterFld, GetVal(rs("FldPickUPAfterFld"), ""))
                        argDVsf.SetData(i, vOthFldFontName, GetVal(rs("FldFontName"), ""))
                        argDVsf.SetData(i, vOthFldFontSize, GetVal(rs("FldFontSize"), ""))
                        argDVsf.SetData(i, vOthBlnCopyPreviousRowData, GetVal(rs("BlnCopyPreviousRowData"), ""))
                        argDVsf.SetData(i, vOthDocumentValue, GetVal(rs("DocValue"), ""))
                        argDVsf.SetData(i, vOthSeparator, GetVal(rs("Separator"), ""))
                        argDVsf.SetData(i, vOthPerAmount, GetVal(rs("PerAmount"), ""))
                        argDVsf.SetData(i, vOthFldBlnNewTrnCalc, GetVal(rs("FldBlnNewTrnCalc"), ""))
                        argDVsf.SetData(i, vOthFldBlnChkDuplicate, GetVal(rs("FldBlnChkDuplicate"), ""))
                        argDVsf.SetData(i, vOthBlnTotalReq, GetVal(rs("BlnTotalReq"), "N"))
                        argDVsf.SetData(i, vOthBlnLoadPrevTrnData, GetVal(rs("LoadPrevTrnData"), "N"))
                        argDVsf.SetData(i, vOthBlnLoadPrevTrnParty, GetVal(rs("LoadPrevDataParty"), "N"))
                        argDVsf.SetData(i, vOthBlnLoadPrevTrnItem, GetVal(rs("LoadPrevDataItem"), "N"))
                        argDVsf.SetData(i, vOthRoundType, GetVal(rs("RoundType"), "N"))
                        argDVsf.SetData(i, vOthCSVFieldName, GetVal(rs("CSVFieldName"), ""))
                        argDVsf.SetData(i, vOthFldPrevTranFldName, GetVal(rs("PrevTrnFldName"), ""))
                        argDVsf.SetData(i, vOthFldMultiAmtFld, GetVal(rs("MultiAmtFld"), "N"))
                        argDVsf.SetData(i, vOthFldMultiAmtVal, "")

                        If argDVsf.GetData(i, vOthPerAmount) = "Y" Then
                            argDVsf.Cols(vOthFldDataPer).Visible = True
                            argDVsf.Cols(vOthFldDataPer).Width = 50
                        End If

                        If GetVal(rs("FldRowClrFontBold"), "") = "Y" Then
                            Dim rg As CellRange = argDVsf.GetCellRange(i, vOthFldDataValue)
                            rg.Style = argDVsf.Styles("bold")
                        End If

                        'If !FldBlnRowClr = "Y" Then
                        '    If !FldRowClrForeColour <> "" Then
                        '        'argdvsf.Cell(flexcpBackColor, i, vOthFldDataValue, i, vOthFldDataValue, GetVal(rs("FldRowClrForeColour
                        '    End If
                        'End If

                        If GetVal(rs!FldTxtAlignment, "") <> "" Then
                            Dim csAln As CellStyle = argDVsf.Styles.Add("ALN")
                            Select Case !FldTxtAlignment
                                Case "Left"
                                    csAln.TextAlign = TextAlignEnum.LeftCenter
                                    'argdvsf.Cell(flexcpAlignment, i, vOthFldDataValue, i, vOthFldDataValue) = flexAlignLeftCenter
                                Case "Right"
                                    csAln.TextAlign = TextAlignEnum.RightCenter
                                    'argdvsf.Cell(flexcpAlignment, i, vOthFldDataValue, i, vOthFldDataValue) = flexAlignRightCenter
                                Case "Centre"
                                    csAln.TextAlign = TextAlignEnum.CenterCenter
                                    'argdvsf.Cell(flexcpAlignment, i, vOthFldDataValue, i, vOthFldDataValue) = flexAlignCenterCenter
                            End Select
                            Dim rg As CellRange = argDVsf.GetCellRange(i, vOthFldDataValue, i, vOthFldDataValue)
                            rg.Style = argDVsf.Styles("ALN")
                        End If

                        If argDVsf.GetData(i, vOthSeparator) = "Y" Then

                            Dim csSep As CellStyle = argDVsf.Styles.Add("SEP")
                            csSep.Border.Color = Color.Black
                            csSep.Border.Width = 4
                            csSep.Border.Direction = BorderDirEnum.Horizontal

                            Dim rg As CellRange = argDVsf.GetCellRange(i, 0, i, argDVsf.Cols.Count - 1)
                            rg.Style = argDVsf.Styles("SEP")
                        End If
                        'argdvsf.Cell(flexcpFontName, i, vOthFldDataValue, i, vOthFldDataValue) = clsSVT.GetVal(!FldFontName, "Times New Roman")
                        'argdvsf.Cell(flexcpFontSize, i, vOthFldDataValue, i, vOthFldDataValue) = clsSVT.GetVal(!FldFontSize, 10)
                    Loop
                End With

                rs.Close()
                '---------------

                intRowsHidden = 0

                With argDVsf
                    .DrawMode = C1.Win.C1FlexGrid.DrawModeEnum.OwnerDraw
                    If .Rows.Count = 0 Then
                        .Visible = False
                    Else
                        .Visible = True
                        For i = 0 To .Rows.Count - 1
                            If .Rows(i).Visible = False Then intRowsHidden = intRowsHidden + 1
                        Next i
                        'If all rows are hidden shoud not show the grid
                        If intRowsHidden = .Rows.Count Then .Visible = False
                    End If
                End With
                Call vsfGenHideMndt(argFrm, argDVsf)
                argDVsf.EndUpdate()
            Loop
        End If
        rsSubSection.Close()
        ' -------------
    End Sub

    Public Sub ClearAddnlGrid(argFrm As Object)
        Dim i As Integer
        Dim ctrlList = GetAllDsgrVsf(argFrm, GetType(C1.Win.C1FlexGrid.C1FlexGrid))

        For Each argVsf As C1FlexGrid In ctrlList
            With argVsf
                For i = .Rows.Count - 1 To 0 Step -1
                    If .Rows(i).Visible Then .TopRow = i
                    .SetData(i, vOthFldDataPer, "")
                    .SetData(i, vOthFldDataValue, "")
                Next i
                Call vsfGenHideMndt(argFrm, argVsf)
            End With
        Next

    End Sub

    Public Sub vsfGenHideMndt(ByVal argFrm As Form, ByVal argVsf As C1FlexGrid)
        Dim j As Integer
        Dim rg As CellRange
        Dim csBlack As CellStyle = argVsf.Styles.Add("BLACK")

        Select Case UCase(argFrm.Name)
            Case "FRMTRNINV", "FRMTRNPB"
                csBlack.ForeColor = Color.White
            Case Else
                csBlack.ForeColor = Color.Black
        End Select

        Dim csRed As CellStyle = argVsf.Styles.Add("RED")
        Select Case UCase(argFrm.Name)
            Case "FRMTRNINV", "FRMTRNPB"
                csRed.ForeColor = Color.White
            Case Else
                csRed.ForeColor = Color.DarkRed
        End Select

        Dim csBlue As CellStyle = argVsf.Styles.Add("BLUE")
        Select Case UCase(argFrm.Name)
            Case "FRMTRNINV", "FRMTRNPB"
                csBlue.ForeColor = Color.White
            Case Else
                csBlue.ForeColor = Color.Blue
        End Select

        With argVsf
            .Styles("Normal").WordWrap = True
            .AllowResizing = AllowResizingEnum.Both
            .AutoSizeRows()

            '#  Code to hide and make mandatory clr as red
            For j = 0 To .Rows.Count - 1
                'hide row if to be hidden
                If .GetData(j, vOthFLdBlnHide) = "Y" Then
                    If .GetData(j, vOthFldHideOperator) = "" Then
                        .Rows(j).Visible = False
                    Else
                        .Rows(j).Visible = True
                        If fnExpEvaluate(.GetData(j, vOthFldHideCompareVal),
                                                     .GetData(j, vOthFldHideWithVal),
                                                     .GetData(j, vOthFldHideOperator),
                                                      argFrm, "") Then
                            .Rows(j).Visible = False
                        End If
                    End If
                    If .Rows(j).Visible = False And .GetData(j, vOthFldClrBefHide) = "Y" Then
                        .SetData(j, vOthFldDataValue, "")
                        .SetData(j, vOthFldDataPer, "")
                    End If
                End If

                rg = .GetCellRange(j, vOthFldCaption)
                rg.Style = argVsf.Styles("BLACK")

                'mark red if mandatory
                If .GetData(j, vOthFldBlnMandatory) = "Y" Then
                    If .GetData(j, vOthFldMndtryOperator) = "" Then
                        rg.Style = argVsf.Styles("RED")
                    Else
                        If fnExpEvaluate(.GetData(j, vOthFldMndtryCompareVal),
                                                    .GetData(j, vOthFldMndtryWithVal),
                                                    .GetData(j, vOthFldMndtryOperator),
                                                    argFrm, "") Then
                            rg.Style = argVsf.Styles("RED")
                        End If
                    End If
                End If

                'mark blue if read only
                If .GetData(j, vOthFldBlnReadOnly) = "Y" Then
                    If .GetData(j, vOthFldROOperator) = "" Then
                        rg.Style = argVsf.Styles("BLUE")
                    Else
                        If fnExpEvaluate(.GetData(j, vOthFldROCompareVal),
                                                    .GetData(j, vOthFldROWithVal),
                                                    .GetData(j, vOthFldROOperator),
                                                     argFrm, "") Then
                            rg.Style = argVsf.Styles("BLUE")
                        End If
                    End If
                End If
            Next j
        End With
    End Sub

    Public Function fnExpEvaluate(ByVal strCompVal As String, ByVal strWithVal As String,
        ByVal strOperator As String, ByVal argFrm As Object,
        ByVal argDataType As String, Optional ByVal argcurRow As Integer = -1) As Boolean

        Dim strSQL As String = "", strDataTypeOLD As String

        Dim strDataType As String
        strCompVal = Trim(strCompVal)
        strWithVal = Trim(strWithVal)
        strOperator = Trim(strOperator)

        fnExpEvaluate = False
        strDataType = ""
        If strCompVal <> "" Then strCompVal = ReplSVTExpWithConstantOrValues(strCompVal, strDataType, argFrm, argcurRow)
        strDataTypeOLD = strDataType
        If strWithVal <> "" Then strWithVal = ReplSVTExpWithConstantOrValues(strWithVal, strDataType, argFrm, argcurRow)
        If strDataType = "Text" And strDataTypeOLD <> "" Then strDataType = strDataTypeOLD
        If argDataType <> "" Then strDataType = argDataType
        If Trim(strOperator) <> "" Then
            Select Case gDBTypeCurrent
                Case gDbTypeAccess
                    Select Case strDataType
                        Case "Numeric"
                            strSQL = "Select " & Val(strCompVal) & " " & strOperator & " " & Val(strWithVal) & " as mResult"
                        Case "Date"
                            If Trim(strCompVal) <> "" Then
                                If Not dateValidate(strCompVal, True) Then
                                    MsgBox("Invalid Date!!", vbInformation, argFrm.Text)
                                    Exit Function
                                End If
                            End If
                            If Trim(strWithVal) <> "" Then
                                If Not dateValidate(strWithVal, True) Then
                                    MsgBox("Invalid Date!!", vbInformation, argFrm.Text)
                                    Exit Function
                                End If
                            End If
                            strSQL = "Select "
                            If Trim(strCompVal) = "" Then
                                strSQL = strSQL & " Null"
                            Else
                                strSQL = strSQL & " " & getDateFormatInInsert(strCompVal) & ""
                            End If
                            strSQL = strSQL & strOperator
                            If Trim(strWithVal) = "" Then
                                strSQL = strSQL & " Null"
                            Else
                                strSQL = strSQL & " " & getDateFormatInInsert(strWithVal) & " as mResult"
                            End If
                        Case Else
                            strSQL = "Select '" & strCompVal & "' " & strOperator & " '" & strWithVal & "' as mResult"
                    End Select
                Case gDbTypeMSSQL
                    '# A - Hide for SQL
                    Select Case strDataType
                        Case "Numeric"
                            strSQL = "Select  mResult = case when " & Val(strCompVal) & " "
                            strSQL = strSQL & strOperator & " " & Val(strWithVal) & " then -1 else 0 end "
                        Case "Date"
                            If Trim(strCompVal) <> "" And Trim(strCompVal) <> "/  /" And Trim(strCompVal) <> "__/__/____" Then
                                If Not dateValidate(strCompVal, True) Then
                                    MsgBox("Invalid Date!!", vbInformation, argFrm.Text)
                                    Exit Function
                                End If
                            End If
                            If Trim(strWithVal) <> "" Then
                                If Not dateValidate(strWithVal, True) Then
                                    MsgBox("Invalid Date!!", vbInformation, argFrm.Text)
                                    Exit Function
                                End If
                            End If
                            If strOperator = "" AndAlso strWithVal = "" Then
                                strSQL = "select mResult = case "
                                If Trim(strCompVal) = "" Then
                                    strSQL = strSQL & " Null"
                                Else
                                    strSQL = strSQL & " " & getDateFormatInInsert(strCompVal) & ""
                                End If
                                strSQL = strSQL & " then 1 else 0 end "
                            Else
                                strSQL = "select 'mResult' ="
                                strSQL = strSQL & " case "
                                strSQL = strSQL & " when '" & Format(CDate(strCompVal), "yyyy-MM-dd") & "' " & Trim(strOperator) & " '" & Format(CDate(strWithVal), "yyyy-MM-dd") & "' "
                                strSQL = strSQL & " then 1"
                                strSQL = strSQL & " Else 0"
                                strSQL = strSQL & " End"
                            End If
                        Case Else
                            strSQL = "select 'mResult' ="
                            strSQL = strSQL & " case "
                            strSQL = strSQL & " when '" & Trim(strCompVal) & "' " & Trim(strOperator) & " '" & Trim(strWithVal) & "' "
                            strSQL = strSQL & " then 1"
                            strSQL = strSQL & " Else 0"
                            strSQL = strSQL & " End"
                    End Select
            End Select
            Dim rs As OleDbDataReader = Nothing

            ' Define the command 

            Dim cmd As New OleDbCommand()
            cmd.Connection = svtConn
            cmd.CommandType = CommandType.Text
            cmd.CommandText = strSQL
            Try
                'rs = cmd.ExecuteReader()
                rs = clsSvtLib.getDataReaderFromSQL(strSQL)
                If rs.HasRows Then
                    rs.Read()
                    If rs("mResult") = 0 Then '0 = fails the condition, -1 = Satisfies the condition
                        fnExpEvaluate = False
                    Else
                        fnExpEvaluate = True
                    End If
                End If
                rs.Close()


            Catch ex As Exception
                fnExpEvaluate = False
                MsgBox(ex.Message & vbCrLf & "Please enter proper expression in " & strSQL)
            End Try
        Else
            fnExpEvaluate = True
        End If

    End Function
    '# A - Function to evaluate for hide,readonly,mandatory depending on the conditions

    Public Function ReplSVTExpWithConstantOrValues(ByVal strSvtExp As String,
            ByRef strDataType As String,
             ByVal argFrm As Object, ByVal argcurRow As Integer, Optional ByVal argDetRow As Integer = -1) As String

        Dim ctrlList = GetAllDsgrVsf(argFrm, GetType(C1.Win.C1FlexGrid.C1FlexGrid))

        Dim strResult As String, i As Integer
        strResult = strSvtExp
        strDataType = "Text"
        If InStr(strResult, "#PFX#") > 0 Then
            strResult = Replace(strResult, "#PFX#", Trim(argFrm.cboPfx.Text))
        End If

        If InStr(strResult, "SVTCURDATE") > 0 Then
            strResult = Replace(strResult, "SVTCURDATE", Format(getSystemDate, "dd/MM/yyyy"))
            strDataType = "Date"
        End If

        If InStr(strResult, "SVTCURTIME") > 0 Then
            strResult = Replace(strResult, "SVTCURTIME", Format(getSystemDateTime, "hh:mm:ss"))
            strDataType = "Date"
        End If

        If InStr(strResult, "#D#") > 0 Then
            strResult = Replace(strResult, "#D#", Format(argFrm.dtpDocDate.value, "dd/MM/yyyy"))
            strDataType = "Date"
        End If

        If InStr(strResult, "#FYF#") > 0 Then
            'strResult = Replace(strResult, "#FYF#", Format(gSettings.gCompStDt, "dd/MM/yyyy"))
            strDataType = "Date"
        End If

        If InStr(strResult, "#FYT#") > 0 Then
            'strResult = Replace(strResult, "#FYT#", Format(gSettings.gCompEndDt, "dd/MM/yyyy"))
            strDataType = "Date"
        End If

        If InStr(strResult, "#P#") > 0 Then
            strResult = Replace(strResult, "#P#", Val(argFrm.svtPartyDtl.txtID.text))
        End If

        If InStr(strResult, "SVTUSERCODE") > 0 Then
            strResult = Replace(strResult, "SVTUSERCODE", gUserCode)
        End If

        For Each argVsf As C1FlexGrid In ctrlList
            With argVsf
                For i = 0 To .Rows.Count - 1

                    If InStr(strResult, "#" & .GetData(i, vOthFldCode) & "#") > 0 Then
                        strDataType = .GetData(i, vOthFldDataType)
                        strResult = Replace(strResult, "#" & .GetData(i, vOthFldCode) & "#", Trim(.GetData(i, vOthFldDataValue)))
                    End If
                    If .GetData(i, vOthPerAmount) = "Y" Then
                        If InStr(strResult, "#" & .GetData(i, vOthFldCode) & "PER#") > 0 Then
                            strDataType = .GetData(i, vOthFldDataType)
                            strResult = Replace(strResult, "#" & .GetData(i, vOthFldCode) & "#", Trim(.GetData(i, vOthFldDataPer)))
                        End If
                        If InStr(strResult, "#" & .GetData(i, vOthFldCode) & "AMT#") > 0 Then
                            strDataType = .GetData(i, vOthFldDataType)
                            strResult = Replace(strResult, "#" & .GetData(i, vOthFldCode) & "#", Trim(.GetData(i, vOthFldDataValue)))
                        End If
                    End If

                Next i
            End With
        Next

        If argcurRow <> -1 Then
            With argFrm.VsfOptions
                For i = 0 To .Rows.count - 1
                    'MsgBox(.getdata(i, vOthFldCode))
                    If InStr(strResult, "#" & .getdata(i, vOthFldCode) & "#") > 0 Then
                        strDataType = .getdata(i, vOthFldDataType)
                        '                    strResult = Replace(strResult, "#" & .TextMatrix(i, vOthFldCode) & "#", Trim(argFrm.vsfDet.TextMatrix(i, argcurRow)))
                        'Very Very Important
                        strResult = Replace(strResult, "#" & .getdata(i, vOthFldCode) & "#", Trim(argFrm.vsfDet.getdata(argDetRow, i)))
                    End If
                    If InStr(strResult, "#" & i & "#") > 0 Then
                        '                    strResult = Replace(strResult, "#" & i & "#", Trim(argFrm.vsfDet.TextMatrix(argcurRow, i)))
                        strResult = Replace(strResult, "#" & i & "#", Trim(argFrm.vsfDet.getdata(argDetRow, i)))
                        strDataType = .getdata(i, vOthFldDataType)
                    End If

                Next i
            End With
        End If
        If Trim(strResult) <> "" Then
            If strDataType = "Numeric" Then
                strResult = clsSvtLib.getDataLongValue(strResult, 0, False)
            End If
        End If
        ReplSVTExpWithConstantOrValues = strResult

        'errhandler:
        'MsgBox(Err.Description & vbCrLf & "Please enter valid expression for " & Trim(argFrm.vsfGen(0).TextMatrix(argFrm.vsfGen(0).Row, vOthFldCaption)), vbInformation + vbOKOnly, argFrm.Caption)
    End Function

    Public Function validOthFLds(ByVal argFrm As Object,
                    ByVal argTblName As String, ByVal argPK As String, ByVal argPKValue As String, Optional ByVal argDocShForm As String = "") As Boolean
        Dim i As Integer, blnFlag As Boolean
        Dim strFirstDispCol As String

        validOthFLds = True

        Dim ctrlList = GetAllDsgrVsf(argFrm, GetType(C1.Win.C1FlexGrid.C1FlexGrid))
        For Each argVsf As C1FlexGrid In ctrlList
            With argVsf
                For i = 0 To .Rows.Count - 1
                    If .GetData(i, vOthFldDataType) = "Date" Then
                        If Trim(.GetData(i, vOthFldDataValue)) = "/  /" Then
                            .SetData(i, vOthFldDataValue, "")
                        End If
                    End If

                    '# Code for mandatory field
                    If .GetData(i, vOthFldBlnMandatory) = "Y" Then
                        If fnExpEvaluate(.GetData(i, vOthFldMndtryCompareVal),
                                                    .GetData(i, vOthFldMndtryWithVal),
                                                    .GetData(i, vOthFldMndtryOperator),
                                                       argFrm, "") Then
                            If Trim(.GetData(i, vOthFldDataValue)) = "" Then
                                MsgBox("Field " & .GetData(i, vOthFldCaption) & " cannot be blank ", MsgBoxStyle.Information, argFrm.text)
                                .Row = i
                                '.TopRow = i
                                .Focus()
                                GoTo EndFunc
                            End If

                            If .GetData(i, vOthFldDataType) = "Numeric" And .GetData(i, vOthFldBlnComboData) = "N" Then
                                If .GetData(i, vOthFldNumBlnAnyNumber) = "Y" AndAlso .GetData(i, vOthFldNumBlnAllowNegative) = "N" Then
                                    If Val(.GetData(i, vOthFldDataValue)) <= 0 Then
                                        MsgBox("Field " & .GetData(i, vOthFldCaption) & " cannot be blank or less than zero ", MsgBoxStyle.Information, argFrm.text)
                                        .Row = i
                                        .TopRow = i
                                        .Focus()
                                        GoTo EndFunc
                                    End If
                                End If
                            End If
                        End If
                    End If
                    '# Code for mandatory field

                    '# Code for unique
                    If .GetData(i, vOthFldUnique) = "Y" Then

                        'If fnExpUniqueEvaluate(argFrm, argTblName, _
                        '    Trim(.GetData(i, vOthFldUniqueBasedOn)), _
                        '    Trim(.GetData(i, vOthFldCode)), _
                        '    Trim(.GetData(i, vOthFldDataValue)), argPK, CStr(argPKValue), _
                        '    Trim(.GetData(i, vOthFldDataType)), i, argDocShForm) Then
                        '    'If intRec <> 0 Then
                        '    strMsg = Trim(.TextMatrix(i, vOthFldCaption)) & " is to be unique"

                        '    '# A - Common function which returns the based on parameter
                        '    If Trim(.TextMatrix(i, vOthFldUniqueBasedOn)) <> "" Then
                        '        strMsg = strMsg & " based on " & fnBasedonMsg(Trim(UCase(.TextMatrix(i, vOthFldUniqueBasedOn))))
                        '    End If
                        '    MsgBox(strMsg, vbInformation + vbOKOnly, argFrm.Caption)
                        '    GoTo EndFunc

                        'End If
                    End If
                    '# Code for unique

                    If .GetData(i, vOthFldCmbTblName) <> "" Then
                        If Trim(.GetData(i, vOthFldDataValue)) <> "" Then
                            If InStr(1, .GetData(i, vOthFldCmbDisplayCols), ",") > 0 Then
                                strFirstDispCol = Left(.GetData(i, vOthFldCmbDisplayCols) _
                                                    , InStr(1, .GetData(i, vOthFldCmbDisplayCols), ",") - 1)
                            Else
                                strFirstDispCol = .GetData(i, vOthFldCmbDisplayCols)
                            End If
                            Select Case UCase(.GetData(i, vOthFldDataType))
                                Case "TEXT", "LIST STRING"
                                    Call clsSvtLib.getDataStringValue("Select " & .GetData(i, vOthFldCmbStorageFldName) & " from " & .GetData(i, vOthFldCmbTblName) _
                                & " where " & strFirstDispCol & " = '" & .GetData(i, vOthFldDataValue) & "'", "", blnFlag)
                                Case "NUMERIC", "LIST NUMERIC"
                                    Call clsSvtLib.getDataLongValue("Select " & .GetData(i, vOthFldCmbStorageFldName) & " from " & .GetData(i, vOthFldCmbTblName) _
                                & " where " & strFirstDispCol & " = '" & .GetData(i, vOthFldDataValue) & "'", 0, blnFlag)
                                Case Else
                                    Call clsSvtLib.getDataDateValue("Select " & .GetData(i, vOthFldCmbStorageFldName) & " from " & .GetData(i, vOthFldCmbTblName) _
                                & " where " & strFirstDispCol & " = '" & .GetData(i, vOthFldDataValue) & "'", Nothing, blnFlag)
                            End Select

                            If Not blnFlag Then
                                MsgBox(.GetData(i, vOthFldCaption) & "[" & .GetData(i, vOthFldDataValue) &
                                "] ...  Not found in " & .GetData(i, vOthFldCmbTblName) & " !!",
                                MsgBoxStyle.Information, argFrm.text)
                                .Row = i
                                .Focus()
                                GoTo EndFunc
                            End If
                        End If
                    End If
                Next i
            End With
        Next

        Exit Function
EndFunc:
        validOthFLds = False
    End Function

    Public Function vsfBeforeEditCancel(ByVal argFrm As Form, ByVal argVsf As C1FlexGrid, ByVal argDocID As Integer, ByVal argHdrTblName As String,
        ByVal argMode As String, ByVal argRow As Integer, ByVal argCol As Long, ByVal argMstTrns As String, ByVal argParty As String) As Boolean
        Dim strCondition As String = "", strFlds As String
        Dim strNumFormat As String = ""
        vsfBeforeEditCancel = False
        If argMode = "V" Or argMode = "" Then
            vsfBeforeEditCancel = True
            Exit Function
        End If

        If argVsf.GetData(argRow, vOthPerAmount) <> "Y" Then
            If argCol <> vOthFldDataValue Then
                vsfBeforeEditCancel = True
                argVsf.Col = vOthFldDataValue
                Exit Function
            End If
        End If

        If argCol <> vOthFldDataValue Then
            If argVsf.GetData(argRow, vOthPerAmount) <> "Y" Then
                vsfBeforeEditCancel = True
                argVsf.Col = vOthFldDataValue
                Exit Function
            End If
        End If
        If argVsf.GetData(argRow, vOthPerAmount) = "Y" Then
            If argCol = vOthFldDataValue Then
                vsfBeforeEditCancel = True
                Exit Function
            End If
        End If

        'If argCol = vOthFldDataPer Then
        '    If argVsf.GetData(argRow, vOthPerAmount) <> "Y" Then
        '        vsfBeforeEditCancel = True
        '        argVsf.Col = vOthFldDataValue
        '        Exit Function
        '    Else
        '        argVsf.Col = vOthFldDataPer
        '    End If
        'Else
        '    If argCol <> vOthFldDataValue Then
        '        vsfBeforeEditCancel = True
        '        argVsf.Col = vOthFldDataValue
        '        Exit Function
        '    End If
        '    If argVsf.GetData(argRow, vOthPerAmount) = "Y" Then
        '        vsfBeforeEditCancel = True
        '        argVsf.Col = vOthFldDataValue
        '        Exit Function
        '    End If
        'End If

        If argMode = "M" And argMstTrns = "T" Then
            Dim rs As OleDb.OleDbDataReader
            Dim i As Integer

            With argVsf
                rs = clsSvtLib.getDataReaderFromSQL("Select CndFldCode from UtlDocNoPrefix where DocID = " & argDocID)

                If Not rs.HasRows Then
                    rs.Close()
                    GoTo SKIPDOCNOPFX
                End If
                '------------------
                Do While rs.Read()
                    For i = 0 To argVsf.Rows.Count - 1
                        If Trim(UCase(rs("CndFldCode"))) = Trim(UCase(argVsf.GetData(i, vOthFldCode))) Then
                            vsfBeforeEditCancel = True
                            Exit Function
                        End If
                    Next i
                Loop
                rs.Close()
            End With
        End If
SKIPDOCNOPFX:
        With argVsf
            '# A - Code to make readonly
            If .GetData(argRow, vOthFldBlnReadOnly) = "Y" Then
                If .GetData(argRow, vOthFldROOperator) = "" Then
                    vsfBeforeEditCancel = True
                    Exit Function
                Else
                    If fnExpEvaluate(.GetData(argRow, vOthFldROCompareVal),
                                                .GetData(argRow, vOthFldROWithVal),
                                                .GetData(argRow, vOthFldROOperator),
                                                  argFrm, "") Then
                        vsfBeforeEditCancel = True
                        Exit Function
                    Else
                        vsfBeforeEditCancel = False
                        Exit Function
                    End If
                End If
            End If

            .ComboList = ""
            .EditMask = ""

            'fill from list of options
            If Mid(.GetData(argRow, vOthFldDataType), 1, 4) = "List" Then
                .ComboList = .GetData(argRow, vOthFldNumStringListContent)
                Exit Function
            End If

            'fill from list of options
            If .GetData(argRow, vOthFldTxtBlnMultiLine) = "Y" Or .GetData(argRow, vOthFldMultiAmtFld) = "Y" Then
                .ComboList = "..."
                Exit Function
            End If

            'fill for multi combo cols
            If Trim(.GetData(argRow, vOthFldCmbTblName)) <> "" Then
                strFlds = .GetData(argRow, vOthFldCmbDisplayCols)
                If .GetData(argRow, vOthFldCmbAddnlCond) <> "" Then
                    strCondition = " Where " & Replace(.GetData(argRow, vOthFldCmbAddnlCond), "`", "'", 1)
                    If InStr(strCondition, "#C#") > 0 Then
                        strCondition = Replace(strCondition, "#C#", clsSvtLib.getDataLongValue("select CustID from MstCust Where CustCode = '" & argParty & "'", 0, False).ToString)
                    End If
                    If InStr(strCondition, "#S#") > 0 Then
                        strCondition = Replace(strCondition, "#C#", clsSvtLib.getDataLongValue("select SuppID from MstSupp Where SuppCode = '" & argParty & "'", 0, False).ToString)
                    End If
                End If


                '# A - 22/Jan/2008 - To Restrict Sundry Drs and Crs in Ledger master
                If argCol = vOthFldDataValue Then
                    'If ledger mst screen + MstLedGroup (tbl)
                    '+ strFlds is LedGroupName + Form name is frmMstGeneral
                    If InStr(1, Trim(argFrm.Text), "Ledger") > 0 _
                    And Trim(.GetData(argRow, vOthFldCmbTblName)) = "MstLedGroup" _
                    And Trim(strFlds) = "LedGroupName" And argFrm.Name = "frmMstGeneral" Then
                        strCondition = " where LedGroupName not in ('Sundry Creditors', 'Sundry Debtors')"
                    End If
                    If InStr(strCondition, "#") > 0 Then
                        strCondition = ReplSVTExpWithConstantOrValues(strCondition, "", argFrm, -1)
                    End If
                    .ComboList = genMultiColVsfComboString(.GetData(argRow,
                    vOthFldCmbTblName), strCondition & " order by 1", strFlds)
                End If

            End If

            If .GetData(argRow, vOthFldBlnRefill) = "Y" Then
                .ComboList = GenVSfComboStringCond(.GetData(argRow, vOthFldCode), argHdrTblName, " order by 1")
            End If
            '.EditMask = ""
            Select Case .GetData(argRow, vOthFldDataType)
                Case "Date"
                    .EditMask = "##/##/####"
                    'Case "Numeric"
                    '    strNumFormat = "##############"
                    '    If Val(getdata(argRow, vOthFldNumDecimals)) > 0 Then
                    '        .EditMask = "##############"
                    '    End If
            End Select
        End With

        vsfBeforeEditCancel = False
    End Function

    Public Sub vsfSetupEditor(ByVal argFrm As Form, ByVal argVsf As C1FlexGrid, ByVal argDocID As Integer, ByVal argHdrTblName As String,
        ByVal argMode As String, ByVal argRow As Integer, ByVal argCol As Long, ByVal argMstTrns As String)

        If argMode = "V" Or argMode = "" Then
            Exit Sub
        End If
        With argVsf
            'if it is a combo then exit sub
            If Trim(.GetData(argRow, vOthFldCmbTblName)) <> "" Then Exit Sub
            If Mid(Trim(.GetData(argRow, vOthFldDataType)), 1, 4) = "List" Then Exit Sub
            'set max length
            If TypeOf (argVsf.Editor) Is TextBox Then
                Dim tb As TextBox = argVsf.Editor
                tb.MaxLength = Val(.GetData(argRow, vOthFldDataWidth))
                If .GetData(argRow, vOthFldTxtBlnMultiLine) = "Y" Then
                    tb.Multiline = True

                    tb.CharacterCasing = CharacterCasing.Upper
                End If
            End If
        End With
    End Sub

    Public Function vsfKeyPressEdit(ByVal argFrm As Form, ByVal argVsf As C1FlexGrid, ByVal argRow As Integer,
                                     ByVal KeyChar As String) As Boolean
        With argVsf
            vsfKeyPressEdit = True
            If Trim(.GetData(argRow, vOthFldBlnComboData)) = "Y" Then
                vsfKeyPressEdit = True
                Exit Function
            End If

            Dim keyAscii As Integer
            keyAscii = Asc(KeyChar)

            If UCase(.GetData(argRow, vOthFldDataType)) = "NUMERIC" Or UCase(.GetData(argRow, vOthFldDataType)) = "LIST NUMERIC" Then
                If Trim(.GetData(argRow, vOthFldBlnComboData)) = "Y" Then
                    vsfKeyPressEdit = KeyValidate("String", KeyChar)
                    Exit Function
                Else
                    vsfKeyPressEdit = KeyValidate("Decimal", KeyChar,
                                Val(.GetData(argRow, vOthFldDataWidth)))
                    Exit Function
                End If
            End If

            Select Case .GetData(.Row, vOthFldDataType)
                Case "Text"
                    Select Case .GetData(.Row, vOthFldTxtStringType)
                        Case "ALPHANUMBER"
                            If Not ((keyAscii >= Asc("a") And keyAscii <= Asc("z")) Or
                                (keyAscii >= Asc("A") And keyAscii <= Asc("Z")) Or
                                (keyAscii >= Asc("0") And keyAscii <= Asc("9"))) And
                                keyAscii <> 8 And keyAscii <> 32 Then

                                vsfKeyPressEdit = False
                                Exit Function
                            End If
                            If InStr(1, .GetData(.Row, vOthFldTxtDoNotAllowChars), Chr(keyAscii)) > 0 Then
                                vsfKeyPressEdit = False
                                Exit Function
                            End If
                            vsfKeyPressEdit = True
                            Exit Function

                        Case "ALPHABET"
                            If Not ((keyAscii >= Asc("a") And keyAscii <= Asc("z")) Or
                                    (keyAscii >= Asc("A") And keyAscii <= Asc("Z"))) And
                                    keyAscii <> 8 And keyAscii <> 32 Then
                                vsfKeyPressEdit = False
                                Exit Function
                            End If
                            If InStr(1, .GetData(.Row, vOthFldTxtDoNotAllowChars), Chr(keyAscii)) > 0 Then
                                vsfKeyPressEdit = False
                                Exit Function
                            End If
                            vsfKeyPressEdit = True
                            Exit Function

                        Case "NUMBER"
                            If Not (keyAscii >= Asc("0") And keyAscii <= Asc("9")) And
                                    keyAscii <> 8 And keyAscii <> 46 Then
                                vsfKeyPressEdit = False
                                Exit Function
                            End If
                            If InStr(1, .GetData(.Row, vOthFldTxtDoNotAllowChars), Chr(keyAscii)) > 0 Then
                                vsfKeyPressEdit = False
                                Exit Function
                            End If
                            vsfKeyPressEdit = True
                            Exit Function

                        Case Else
                            If InStr(1, .GetData(.Row, vOthFldTxtDoNotAllowChars), Chr(keyAscii)) > 0 Then
                                vsfKeyPressEdit = False
                                Exit Function
                            End If
                            vsfKeyPressEdit = True
                            Exit Function
                    End Select

                Case "Numeric"
                    If .GetData(argRow, vOthFldNumBlnAnyNumber) = "Y" Then
                        If .GetData(argRow, vOthFldNumBlnAllowNegative) = "Y" Then
                            If Not (keyAscii >= Asc("0") And keyAscii <= Asc("9")) And
                                keyAscii <> 8 And keyAscii <> 45 And keyAscii <> 46 Then
                                vsfKeyPressEdit = False
                                Exit Function
                            End If
                        Else
                            If Not (keyAscii >= Asc("0") And keyAscii <= Asc("9")) And
                                keyAscii <> 8 And keyAscii <> 46 Then
                                vsfKeyPressEdit = False
                                Exit Function
                            End If
                        End If
                    Else
                        If Not (keyAscii >= Asc("0") And keyAscii <= Asc("9")) And
                            keyAscii <> 8 And keyAscii <> 46 Then
                            vsfKeyPressEdit = False
                            Exit Function
                        End If
                    End If
            End Select
        End With
        vsfKeyPressEdit = True
    End Function

    Public Function vsfRowColChange(ByVal argFrm As Form, ByVal argVSF As C1FlexGrid) As String
        vsfRowColChange = ""
        With argVSF
            If .Row < 0 Then Exit Function
            If Trim(.GetData(.Row, vOthFldToolTip)) = "" Then
                vsfRowColChange = ""
            Else
                vsfRowColChange = .GetData(.Row, vOthFldToolTip)
            End If
        End With
    End Function

    Public Function vsfValidateEdit(ByVal argFrm As Form, ByVal argVsf As C1FlexGrid, ByVal argRow As Integer, ByVal argCol As Integer _
                                    , ByVal strMstOrTrans As String, ByVal strTab As String _
                                    , ByVal strPrimKey As String, ByVal strValPrim As String, Optional ByVal argShCode As String = "") As Boolean
        Dim strMsg As String, blnFlag As Boolean, strAdnlCond As String = ""
        Dim blnValid As Boolean, strFirstDispCol As String, strSQL As String
        With argVsf
            vsfValidateEdit = False

            '# Code for mandatory validation
            If .GetData(argRow, vOthFldBlnMandatory) = "Y" Then
                If fnExpEvaluate(.GetData(argRow, vOthFldMndtryCompareVal),
                                            .GetData(argRow, vOthFldMndtryWithVal),
                                            .GetData(argRow, vOthFldMndtryOperator),
                                              argFrm, "") Then

                    If Trim(.Editor.Text) = "" Or Trim(.Editor.Text) = "0" Then
                        MsgBox("Mandatory field: '" & .GetData(argRow, vOthFldCaption) & "' cannot be " & IIf(Trim(.Editor.Text) = "", "blank!!", "zero!!"), vbInformation, argFrm.Text)
                        Exit Function
                    End If
                End If
            End If


            'for data pickup feature
            For x As Int32 = 0 To .Rows.Count - 1
                If Trim(.GetData(x, vOthFldPickUPAfterFld)) <> "" Then
                    If Trim(Replace(.GetData(x, vOthFldPickUPAfterFld), "#", "")) = Trim(.GetData(argRow, vOthFldCode)) Then
                        strSQL = .GetData(x, vOthFldPickUP)
                        'strSQL = ReplSVTExpWithConstantOrValues(strSQL, .GetData(x, vOthFldDataType), argFrm, -1)
                        If strSQL <> "" Then
                            strSQL = Replace(Replace(Replace(strSQL, "#", ""), .GetData(argRow, vOthFldCode), Trim(.Editor.Text)), "`", "'")
                            Select Case UCase(.GetData(x, vOthFldDataType))
                                Case "TEXT", "LIST STRING"
                                    .SetData(x, vOthFldDataValue, clsSvtLib.getDataStringValue(strSQL, "", False))
                                Case "NUMERIC", "LIST NUMERIC"
                                    .SetData(x, vOthFldDataValue, clsSvtLib.getDataDoubleValue(strSQL, 0, False))
                                Case "DATE"
                                    .SetData(x, vOthFldDataValue, clsSvtLib.getDataStringValue(strSQL, Nothing, False))
                            End Select
                        End If
                    End If
                End If
            Next


            '#  Code for unique
            If .GetData(argRow, vOthFldUnique) = "Y" Then
                'Dim intRec As Integer
                If fnExpUniqueEvaluate(argFrm, strMstOrTrans, strTab,
                    Trim(.GetData(argRow, vOthFldUniqueBasedOn)),
                    Trim(.GetData(argRow, vOthFldCode)),
                    .Editor.Text,
                    strPrimKey, strValPrim,
                    Trim(.GetData(argRow, vOthFldDataType)), CInt(argRow), argShCode) Then
                    'If intRec <> 0 Then
                    strMsg = Trim(.GetData(argRow, vOthFldCaption)) & " is to be unique"

                    '# A - Common function which returns the based on parameter
                    If Trim(.GetData(argRow, vOthFldUniqueBasedOn)) <> "" Then
                        strMsg = strMsg & " based on " & fnBasedonMsg(Trim(UCase(.GetData(argRow, vOthFldUniqueBasedOn))))
                    End If

                    MsgBox(strMsg, vbInformation + vbOKOnly, argFrm.Text)
                    Exit Function
                End If
            End If
            '# A - Code for unique

            '# A - Code for First day and last day in month
            If .GetData(argRow, vOthFldDtBlnFirstDOM) = "Y" And IsDate(.Editor.Text) Then
                .Editor.Text = getFirstDateInMonth(.Editor.Text)
            End If
            If .GetData(argRow, vOthFldDtBlnLastDOM) = "Y" And IsDate(.Editor.Text) Then
                .Editor.Text = getLastDateInMonth(.Editor.Text)
            End If
            '# A - Code for First day and last day in month

            'code to validate combo
            If .GetData(argRow, vOthFldCmbTblName) <> "" Then
                If Trim(.Editor.Text) <> "" Then
                    If InStr(1, .GetData(argRow, vOthFldCmbDisplayCols), ",") > 0 Then
                        strFirstDispCol = Left(.GetData(argRow, vOthFldCmbDisplayCols) _
                                            , InStr(1, .GetData(argRow, vOthFldCmbDisplayCols), ",") - 1)
                    Else
                        strFirstDispCol = .GetData(argRow, vOthFldCmbDisplayCols)
                    End If
                    strAdnlCond = Trim(argVsf.GetData(argRow, vOthFldCmbAddnlCond))
                    If strAdnlCond <> "" Then
                        strAdnlCond = strAdnlCond.Replace("`", "'")
                        strAdnlCond = " and " & strAdnlCond
                    End If
                    strSQL = "Select " & .GetData(argRow, vOthFldCmbStorageFldName) & " from " &
                        .GetData(argRow, vOthFldCmbTblName) & " where " & strFirstDispCol & " = '" & .Editor.Text & "' " & strAdnlCond
                    Select Case UCase(.GetData(argRow, vOthFldDataType))
                        Case "TEXT", "LIST STRING"
                            Call clsSvtLib.getDataStringValue(strSQL, "", blnFlag)
                        Case "NUMERIC", "LIST NUMERIC"
                            Call clsSvtLib.getDataLongValue(strSQL, 0, blnFlag)
                        Case Else
                            Call clsSvtLib.getDataDateValue(strSQL, Nothing, blnFlag)
                    End Select
                    ' Call clsSvtLib.getDataLongValue(strSQL, 0, blnFlag)
                    If Not blnFlag Then
                        MsgBox(.GetData(argRow, vOthFldCaption) & "[" & .Editor.Text & "] ...  Not found in " _
                               & .GetData(argRow, vOthFldCmbTblName) & " !!", MsgBoxStyle.Information, argFrm.Text)
                        .Focus()
                        Exit Function
                    End If
                End If
                vsfValidateEdit = True
                Exit Function
            End If

            '# R Begin
            If .GetData(argRow, vOthFldDataType) = "Numeric" Or .GetData(argRow, vOthFldDataType) = "List Numeric" Then
                If .GetData(argRow, vOthFldNumBlnPercentage) = "Y" Then
                    If .GetData(argRow, vOthFldBlnMandatory) = "Y" Then
                        If Val(.Editor.Text) <= 0 Or Val(.Editor.Text) > 100 Then
                            MsgBox("Invalid percentage should range 1-100 !!!", vbInformation, argFrm.Text)
                            Exit Function
                        End If
                    Else
                        If Val(.Editor.Text) < 0 Or Val(.Editor.Text) > 100 Then
                            MsgBox("Invalid percentage should range 0-100 !!!", MsgBoxStyle.Information, argFrm.Text)
                            Exit Function
                        End If
                    End If
                End If

                If .GetData(argRow, vOthFldNumBlnRangeFromChk) = "Y" And .GetData(argRow, vOthFldNumBlnRangeToChk) = "N" Then
                    If Not fnExpEvaluate(.Editor.Text,
                                    .GetData(argRow, vOthFldNumRangeFromValue),
                                    .GetData(argRow, vOthFldNumRangeFromOperator),
                                     argFrm, "Numeric") Then
                        MsgBox(.GetData(argRow, vOthFldCaption) & " --- Entered number not " & .GetData(argRow, vOthFldNumRangeFromOperator) &
                               " " & .GetData(argRow, vOthFldNumRangeFromValue), MsgBoxStyle.Information, argFrm.Text)
                        Exit Function
                    End If
                ElseIf .GetData(argRow, vOthFldNumBlnRangeFromChk) = "N" And .GetData(argRow, vOthFldNumBlnRangeToChk) = "Y" Then
                    If Not fnExpEvaluate(.Editor.Text,
                                    .GetData(argRow, vOthFldNumRangeToValue),
                                    .GetData(argRow, vOthFldNumRangeToOperator),
                                     argFrm, "Numeric") Then
                        MsgBox(.GetData(argRow, vOthFldCaption) & " --- Entered number not " & .GetData(argRow, vOthFldNumRangeToOperator) &
                               " " & .GetData(argRow, vOthFldNumRangeToValue), MsgBoxStyle.Information, argFrm.Text)
                        Exit Function
                    End If
                ElseIf .GetData(argRow, vOthFldNumBlnRangeFromChk) = "Y" And .GetData(argRow, vOthFldNumBlnRangeToChk) = "Y" Then
                    blnValid = True
                    If Not fnExpEvaluate(.Editor.Text,
                                    .GetData(argRow, vOthFldNumRangeFromValue),
                                    .GetData(argRow, vOthFldNumRangeFromOperator),
                                     argFrm, "Numeric") Then
                        blnValid = False
                    End If
                    If Not fnExpEvaluate(.Editor.Text,
                                    .GetData(argRow, vOthFldNumRangeToValue),
                                    .GetData(argRow, vOthFldNumRangeToOperator),
                                     argFrm, "Numeric") Then
                        blnValid = False
                    End If
                    If Not blnValid Then
                        MsgBox(.GetData(argRow, vOthFldCaption) & " -- Entered number should be in range " &
                                .GetData(argRow, vOthFldNumRangeFromOperator) & " " & .GetData(argRow, vOthFldNumRangeFromValue) & " and " &
                                .GetData(argRow, vOthFldNumRangeToOperator) & " " & .GetData(argRow, vOthFldNumRangeToValue), vbInformation, argFrm.Text)
                        Exit Function
                    End If
                End If
            End If

            'check for date range validation
            If .GetData(argRow, vOthFldDataType) = "Date" Then
                'check for financial year range
                If Trim(.Editor.Text) = "" Or Trim(.Editor.Text) = "/  /" Or Trim(.Editor.Text) = "__/__/____" Then
                    Return True
                End If

                If .GetData(argRow, vOthFldDtBlnWithinFinYear) = "Y" Then
                    If .GetData(argRow, vOthFldBlnMandatory) = "Y" Then
                        ' Date Error Reportted By Raghu 11July2008
                        If Not dateValidate(.Editor.Text, True) Then
                            MsgBox("Invalid date", vbInformation, argFrm.Text)
                            Exit Function
                        End If
                        ' Date Error Reportted By Raghu 11July2008

                        If Trim(.Editor.Text) = "" Or Trim(.Editor.Text) = "/  /" Or Trim(.Editor.Text) = "__/__/____" Then
                            MsgBox(.GetData(argRow, vOthFldCaption) & " --- Date should be within Financial Year " &
                                   Format(gSettings.gCompStDt, "dd MMM yyyy" & " to " & Format(gSettings.gCompEndDt, "dd MMM yyyy")), MsgBoxStyle.Information, argFrm.Text)
                            Exit Function
                        End If
                        If CDate(.Editor.Text) <= CDate(gSettings.gCompStDt) Or CDate(.Editor.Text) >= CDate(gSettings.gCompEndDt) Then
                            MsgBox(.GetData(argRow, vOthFldCaption) & " --- Date should be within Financial Year " _
                                    & Format(gSettings.gCompStDt, "dd MMM yyyy" & " to " & Format(gSettings.gCompEndDt, "dd MMM yyyy")), MsgBoxStyle.Information, argFrm.Text)
                            Exit Function
                        End If

                    Else

                        If Trim(.Editor.Text) <> "" And Trim(.Editor.Text) <> "/  /" Or Trim(.Editor.Text) <> "__/__/____" Then
                            ' Date Error Reportted By Raghu 11July2008
                            If Not dateValidate(.Editor.Text, True) Then
                                MsgBox("Invalid date", vbInformation, argFrm.Text)
                                Exit Function
                            End If
                            ' Date Error Reportted By Raghu 11July2008

                            If CDate(.Editor.Text) <= CDate(gSettings.gCompStDt) Or CDate(.Editor.Text) >= CDate(gSettings.gCompEndDt) Then
                                MsgBox(.GetData(argRow, vOthFldCaption) & " --- Date should be within Financial Year " &
                                       Format(gSettings.gCompStDt, "dd MMM yyyy") & " to " & Format(gSettings.gCompEndDt, "dd MMM yyyy"), vbInformation, argFrm.Text)
                                Exit Function
                            End If
                        End If
                    End If
                End If

                'check for range from, to, and both
                If .GetData(argRow, vOthFldDtBlnRangeFrom) = "Y" And .GetData(argRow, vOthFldDtBlnRangeTo) = "N" Then
                    If Not fnExpEvaluate(.Editor.Text,
                                    .GetData(argRow, vOthFldDtRangeFromValue),
                                    .GetData(argRow, vOthFldDtRangeFromOperator),
                                     argFrm, "Date") Then
                        MsgBox(.GetData(argRow, vOthFldCaption) & " --- Entered Date not " & .GetData(argRow, vOthFldDtRangeFromOperator) &
                               " " & .GetData(argRow, vOthFldDtRangeFromValue), MsgBoxStyle.Information, argFrm.Text)
                        Exit Function
                    End If
                ElseIf .GetData(argRow, vOthFldDtBlnRangeFrom) = "N" And .GetData(argRow, vOthFldDtBlnRangeTo) = "Y" Then
                    If Not fnExpEvaluate(.Editor.Text,
                                    .GetData(argRow, vOthFldDtRangeToValue),
                                    .GetData(argRow, vOthFldDtRangeToOperator),
                                     argFrm, "Date") Then
                        MsgBox(.GetData(argRow, vOthFldCaption) & " --- Entered Date not " & .GetData(argRow, vOthFldDtRangeToOperator) &
                               " " & .GetData(argRow, vOthFldDtRangeToValue), MsgBoxStyle.Information, argFrm.Text)
                        Exit Function
                    End If
                ElseIf .GetData(argRow, vOthFldDtBlnRangeFrom) = "Y" And .GetData(argRow, vOthFldDtBlnRangeTo) = "Y" Then
                    blnValid = True
                    If Not fnExpEvaluate(.Editor.Text,
                                    .GetData(argRow, vOthFldDtRangeFromValue),
                                    .GetData(argRow, vOthFldDtRangeFromOperator),
                                     argFrm, "Date", argRow) Then
                        blnValid = False
                    End If
                    If Not fnExpEvaluate(.Editor.Text,
                                    .GetData(argRow, vOthFldDtRangeToValue),
                                    .GetData(argRow, vOthFldDtRangeToOperator),
                                     argFrm, "Date") Then
                        blnValid = False
                    End If
                    If Not blnValid Then
                        MsgBox(.GetData(argRow, vOthFldCaption) & " --- Entered Date should be in range " &
                                .GetData(argRow, vOthFldDtRangeFromValue) & " and " &
                                .GetData(argRow, vOthFldDtRangeToValue), MsgBoxStyle.Information, argFrm.Text)
                        Exit Function
                    End If
                End If

            End If
            vsfValidateEdit = True
        End With
    End Function

    Public Function fnExpUniqueEvaluate(ByVal argFrm As Object, ByVal strMstOrTrans As String, ByVal strHTabName As String _
                        , ByVal strUbasedOn As String, ByVal strDataCode As String, ByVal strDataVal As String _
                        , ByVal strPrimaryKey As String, ByVal strValPrimaryKey As String, ByVal strDataType As String, ByVal argcurRow As Integer, Optional ByVal argDOCShCode As String = "") As Boolean

        fnExpUniqueEvaluate = False

        Dim rsDummy As OleDb.OleDbDataReader
        Dim strSQL As String, strCondition As String = "", intNoOfGrids As Integer, X As Integer
        Dim strFldVal As String, i As Integer

        strSQL = "select count(*)  from " & Trim(strHTabName) & " t "
        Select Case UCase(Trim(strUbasedOn))
            Case "#C#"
                strSQL = strSQL & ", MstCust m"
                strCondition = " and m.CustID = t.CustID"
                strCondition = strCondition & " and CustCode = '" & argFrm.cboCust.Text & "'"
            Case "#S#"
                strSQL = strSQL & ", MstSupp m"
                strCondition = " and m.SuppID = t.SuppID"
                strCondition = strCondition & " and SuppCode = '" & argFrm.cboCust.Text & "'"
            Case "#PFX#"
                If strMstOrTrans = "T" Then _
                    strCondition = " and Prefix = '" & argFrm.cboPfx.Text & "'"
                '"#D#" - get the frm dtp val
            Case "#D#"
                If strMstOrTrans = "T" Then _
                    strCondition = " and " & argDOCShCode & "Date = " & getDateFormatInCond(argFrm.dtDocDate.value)
                '"#FY#" - get the frm date btwn fy
            Case "#FY#"
                If strMstOrTrans = "T" Then _
                    strCondition = strCondition & " and " & strDataCode & " = " & fnGetUniqueCondFld(strDataType, strDataVal)
                strCondition = strCondition & " and " & argDOCShCode & "Date >= " & getDateFormatInCond(clsSvtLib.getDataStringValue("Select yrFdate from MstCompFinYear where Active = 'Y'", "", False))
                strCondition = strCondition & " and " & argDOCShCode & "Date <= " & getDateFormatInCond(clsSvtLib.getDataStringValue("Select yrTdate from MstCompFinYear where Active = 'Y'", "", False))
            Case Else 'Chk for Fld codes
                If InStr(1, strUbasedOn, "#") < 0 Then Exit Function
                intNoOfGrids = 0
                If Trim(strMstOrTrans) <> "M" Then intNoOfGrids = 2
                For X = 0 To intNoOfGrids
                    With argFrm.vsfGen(X)
                        For i = 0 To .Rows - 1
                            If InStr(strUbasedOn, "#" & .TextMatrix(i, vOthFldCode) & "#") > 0 Then
                                strDataType = .TextMatrix(i, vOthFldDataType)
                                strUbasedOn = Replace(strUbasedOn, "#", "")
                                strFldVal = Trim(.TextMatrix(i, vOthFldDataValue))
                                strCondition = strCondition & " and " & strUbasedOn &
                                               " = " & fnGetUniqueCondFld(strDataType, strFldVal)
                                GoTo ExitForLoop
                            End If
                        Next i
                    End With
                Next X


ExitForLoop:

        End Select
        If Trim(strSQL) = "" Then Exit Function
        Select Case strDataType
            Case "Numeric"
                strSQL = strSQL & " where " & strDataCode & " = " & Val(strDataVal)
            Case "Date"
                strSQL = strSQL & " where " & strDataCode & " = " & getDateFormatInCond(strDataVal)
            Case Else
                strSQL = strSQL & " where " & strDataCode & " = '" & strDataVal & "'"
        End Select

        strSQL = strSQL & strCondition
        If InStr(strValPrimaryKey, "/") > 0 Then
            strSQL = strSQL & " and " & strPrimaryKey & " <> '" & strValPrimaryKey & "'"
        Else
            strSQL = strSQL & " and " & strPrimaryKey & " <> " & strValPrimaryKey
        End If
        Try
            rsDummy = clsSvtLib.getDataReaderFromSQL(strSQL)
            fnExpUniqueEvaluate = IIf(rsDummy(0) > 0, True, False)
            rsDummy.Close()
        Catch ex As Exception
            MsgBox(ex.Message & vbCrLf & "Please enter valid unique expression for" & strDataCode & vbCrLf & "SQL:" & strSQL, MsgBoxStyle.Critical, argFrm.text)
            fnExpUniqueEvaluate = False
        End Try
    End Function

    Private Function fnGetUniqueCondFld(ByVal argDataType As String, ByVal argFldVal As String) As String
        fnGetUniqueCondFld = ""
        Select Case argDataType
            Case "Numeric"
                fnGetUniqueCondFld = argFldVal
            Case "Date"
                If Trim(argFldVal) <> "" Then
                    If Not dateValidate(argFldVal, True) Then
                        MsgBox("Invalid Date!!", vbInformation) ', argFrm.Caption
                        Exit Function
                    End If
                End If
                If gDBTypeCurrent = gDbTypeAccess Then
                    fnGetUniqueCondFld = IIf(Trim(argFldVal) = "", "Null", getDateFormatInCond(argFldVal))
                Else
                End If
            Case Else
                fnGetUniqueCondFld = " '" & argFldVal & "'"
        End Select
    End Function

    Private Function fnBasedonMsg(ByVal argUniqBased As String) As String
        fnBasedonMsg = ""
        If Trim(argUniqBased) = "" Then Exit Function
        Select Case UCase(Trim(argUniqBased))
            Case "#S#"
                fnBasedonMsg = "Supplier"
            Case "#C#"
                fnBasedonMsg = "Customer"
            Case "#D#"
                fnBasedonMsg = "Doc Date"
            Case "#PFX#"
                fnBasedonMsg = "Prefix"
            Case "#FY#"
                fnBasedonMsg = "Financial year"
            Case Else
                fnBasedonMsg = Replace(Trim(argUniqBased), "#", "")
        End Select
    End Function

    Public Sub vsfGenAfterEditBfrCalTot(ByVal argFrm As Object, ByVal argVsf As C1FlexGrid, ByVal argRow As Integer, ByVal strMstOrTrans As String, Optional blnSplClr As Boolean = False)

        If argRow < 0 Then Exit Sub
        If argVsf.Visible = False Then Exit Sub
        With argVsf
            If .GetData(argRow, vOthFldDataType) = "Date" Then
                If Trim(.GetData(argRow, vOthFldDataValue)) = "##/##/####" Or Trim(.GetData(argRow, vOthFldDataValue)) = "/  /" Then
                    .SetData(argRow, vOthFldDataValue, "")
                    Exit Sub
                End If
                If Not dateValidate(Trim(.GetData(argRow, vOthFldDataValue)), False) Then
                    If Trim(.GetData(argRow, vOthFldDataValue)) <> "" Then
                        MsgBox("Invalid Date!", vbInformation)
                    End If
                    .SetData(argRow, vOthFldDataValue, "")
                    .Focus()
                End If

                If Trim(argVsf.GetData(argRow, vOthFldDataValue)) <> "" Then
                    Dim blnErr As Boolean = True

                    Try
                        .SetData(argRow, vOthFldDataValue, CDate(argVsf.GetData(argRow, vOthFldDataValue)).ToString("dd/MM/yyyy"))
                        blnErr = False

                    Catch ex As Exception
                    End Try
                    If blnErr = True Then
                        MsgBox("Invalid Date!", vbInformation)
                    End If
                End If
            End If
        End With

        If Not blnSplClr Then
            Call vsfGenHideMndt(argFrm, argVsf)
        End If

        With argVsf
            '# Code for def afteredit
            'Call fillVsfHFTDefValues(argFrm)

            '#  Bigin
            Dim strDecPlaces As String
            Select Case .GetData(.Row, vOthFldDataType)
                Case "Text", "List String"
                    Select Case .GetData(.Row, vOthFldTxtCase)
                        Case "LOWERCASE"
                            .SetData(.Row, vOthFldDataValue, Trim(LCase(.GetData(.Row, vOthFldDataValue))))
                        Case "SENTENCECASE"
                            .SetData(.Row, vOthFldDataValue, Trim(ConVrtToSentence(LCase(.GetData(.Row, vOthFldDataValue)))))
                        Case "TITLECASE"
                            .SetData(.Row, vOthFldDataValue, Trim(StrConv(.GetData(.Row, vOthFldDataValue) _
                                                                        , vbProperCase)))
                        Case "UPPERCASE"
                            .SetData(.Row, vOthFldDataValue, Trim(UCase(.GetData(.Row, vOthFldDataValue))))
                        Case "ASENTERED"
                            .SetData(.Row, vOthFldDataValue, Trim(.GetData(.Row, vOthFldDataValue)))
                    End Select

                Case "Numeric", "List Numeric"
                    If Trim(.GetData(.Row, vOthFldBlnComboData)) = "N" And Trim(.GetData(.Row, vOthFldBlnLoadFrom)) = "N" Then
                        If Val(.GetData(.Row, vOthFldNumDecimals)) > 0 Then
                            strDecPlaces = "#." & New String("0", (Val(.GetData(.Row, vOthFldNumDecimals))))
                        Else
                            strDecPlaces = "#"
                        End If
                        If Len(.GetData(.Row, vOthFldDataValue)) <= 14 Then .SetData(.Row, vOthFldDataValue,
                                SVTROUND(Val(.GetData(.Row, vOthFldDataValue)), Val(.GetData(.Row, vOthFldNumDecimals))))

                        If Len(.GetData(.Row, vOthFldDataValue)) <= 14 Then .SetData(.Row, vOthFldDataValue, Format(CDbl(.GetData(.Row, vOthFldDataValue)), strDecPlaces))
                    End If
            End Select
            '# R End
        End With
        Call vsfGenHideMndtAllGrids(argFrm)
    End Sub

    Public Sub fillVsfHFTDefValues(ByVal argFrm As Object)
        Dim i As Integer, j As Integer
        Dim arrDefCondElements
        Dim arrDefConditionLine
        Dim ctrlList = GetAllDsgrVsf(argFrm, GetType(C1.Win.C1FlexGrid.C1FlexGrid))

        For Each argVsf As C1FlexGrid In ctrlList
            With argVsf
                'ExportToXL(argVsf)
                If .Visible Then
                    For i = 0 To .Rows.Count - 1
                        If Trim(.GetData(i, vOthPerAmount)) = "Y" Then
                            .SetData(i, vOthFldDataPer, .GetData(i, vOthFldDefStdDefault))
                        End If

                        If Trim(.GetData(i, vOthPerAmount)) <> "Y" Then
                            '# A - Code for default
                            If Trim(.GetData(i, vOthFldUseDefault)) = "Y" Then
                                If Trim(.GetData(i, vOthFldDefCondList)) = "" Then
                                    If Trim(.GetData(i, vOthFldDefCondList)) = "" Then
                                        If Trim(.GetData(i, vOthFldDataValue)) = "" Or Trim(.GetData(i, vOthFldDataValue)) = "0" And
                                        Trim(.GetData(i, vOthFldBlnReadOnly)) = "N" Then
                                            .SetData(i, vOthFldDataValue, ReplSVTExpWithConstantOrValues(.GetData(i, vOthFldDefStdDefault), .GetData(i, vOthFldDataType), argFrm, -1))
                                        ElseIf Trim(.GetData(i, vOthFldBlnReadOnly)) = "Y" Then 'For R/o - fill every time
                                            .SetData(i, vOthFldDataValue, ReplSVTExpWithConstantOrValues(.GetData(i, vOthFldDefStdDefault), .GetData(i, vOthFldDataType), argFrm, -1))
                                        End If
                                    End If
                                Else
                                    arrDefConditionLine = Split(.GetData(i, vOthFldDefCondList), "°")
                                    For j = 0 To UBound(arrDefConditionLine)
                                        If .GetData(i, vOthFldDefBlnAgnstCond) = "Y" And arrDefConditionLine(j) <> "" Then
                                            arrDefCondElements = Split(arrDefConditionLine(j), "ß")
                                            If fnExpEvaluate(CStr(arrDefCondElements(0)), CStr(arrDefCondElements(2)), CStr(arrDefCondElements(1)),
                                                         argFrm, .GetData(i, vOthFldDataType), i) Then
                                                'Replace at after edit if no data entried, if entry present then it is retained
                                                'MMM comment if [GIVE AN OPTION FOR CLEARING AND LOADING ]
                                                If Trim(.GetData(i, vOthFldDataValue)) = "" And Trim(.GetData(i, vOthFldBlnReadOnly)) = "N" Then
                                                    .SetData(i, vOthFldDataValue, arrDefCondElements(3))
                                                ElseIf Trim(.GetData(i, vOthFldBlnReadOnly)) = "Y" Then
                                                    .SetData(i, vOthFldDataValue, arrDefCondElements(3))
                                                ElseIf Trim(.GetData(i, vOthFldDataValue)) <> "" AndAlso Trim(.GetData(i, vOthFldDataValue)) <> arrDefCondElements(3) Then
                                                    .SetData(i, vOthFldDataValue, arrDefCondElements(3))
                                                End If
                                                Exit For
                                            End If
                                        End If
                                    Next j
                                End If
                            End If
                        End If
                    Next i

                End If
            End With
        Next
    End Sub

    Public Sub fillVsfDetDefValues(ByVal argVSF As C1FlexGrid, ByVal argVSFOpt As C1FlexGrid, ByVal argRow As Int32)
        Dim i As Integer ', j As Integer
        'Dim arrDefCondElements
        'Dim arrDefConditionLine
        With argVSFOpt
            For i = 0 To .Rows.Count - 1
                '# A - Code for default
                If Trim(.GetData(i, vOthFldUseDefault)) = "Y" Then
                    If Trim(.GetData(i, vOthFldDefCondList)) = "" Then
                        If GetVal(argVSF.GetData(argRow, i), "") = "" OrElse GetVal(.GetData(argRow, i), 0) = 0 Then
                            argVSF.SetData(argRow, i, .GetData(i, vOthFldDefStdDefault))
                        End If
                    Else
                        'arrDefConditionLine = Split(.GetData(i, vOthFldDefCondList), "°")
                        'For j = 0 To UBound(arrDefConditionLine)
                        '    If .GetData(i, vOthFldDefBlnAgnstCond) = "Y" And arrDefConditionLine(j) <> "" Then
                        '        arrDefCondElements = Split(arrDefConditionLine(j), "ß")
                        '        If fnExpEvaluate(CStr(arrDefCondElements(0)), CStr(arrDefCondElements(2)), CStr(arrDefCondElements(1)),
                        '                    "M", argFrm, "") Then
                        '            'Replace at after edit if no data entried, if entry present then it is retained
                        '            'MMM comment if [GIVE AN OPTION FOR CLEARING AND LOADING ]
                        '            If Trim(.GetData(i, vOthFldDataValue)) = "" And Trim(.GetData(i, vOthFldBlnReadOnly)) = "N" Then
                        '                .SetData(i, vOthFldDataValue, arrDefCondElements(3))
                        '            ElseIf Trim(.GetData(i, vOthFldBlnReadOnly)) = "Y" Then
                        '                .SetData(i, vOthFldDataValue, arrDefCondElements(3))
                        '            End If
                        '            Exit For
                        '        End If
                        '    End If
                        'Next j
                    End If
                End If
            Next i
        End With
    End Sub

    Public Sub vsfGenAfterEditAftrCalTot(ByVal argFrm As Object, ByVal argVSF As C1FlexGrid, ByVal argTrnID As Integer,
                                              ByVal argMode As String, ByVal argRow As Integer, ByVal argAllowToChangePfx As Boolean)
        Dim strSQL As String
        If argRow < 0 Then Exit Sub
        If argVSF.Visible = False Then Exit Sub
        'argVSF.AutoSizeRow(argRow)
        argVSF.Styles("Normal").WordWrap = True
        argVSF.AllowResizing = AllowResizingEnum.RowsUniform
        argVSF.AutoSizeRows()
        If argMode = "A" Then
            Dim rsDummy As OleDbDataReader
            With argVSF
                strSQL = "Select * from UtlDocNoPrefix where DocID = " & argTrnID
                rsDummy = clsSvtLib.getDataReaderFromSQL(strSQL)
                If Not rsDummy.HasRows Then
                    rsDummy.Close()
                    Exit Sub
                End If
                '-----------------------------
                With rsDummy
                    Do While .Read()
                        If Trim(UCase(rsDummy("CndFldCode"))) = Trim(UCase(argVSF.GetData(argRow, vOthFldCode))) Then
                            Call assignPfx(argFrm, argVSF, argTrnID, argMode, argAllowToChangePfx, argRow, GetVal(rsDummy("Prefix"), ""),
                                                    GetVal(rsDummy("CndOperator"), ""),
                                                    GetVal(rsDummy("CndFldvalue"), ""))
                        End If
                    Loop
                End With
            End With
            rsDummy.Close()
            '-------------------------
        End If

    End Sub

    Private Sub assignPfx(ByVal argFrm As Object, ByVal argVsf As C1FlexGrid, ByVal argTrnID As Integer, ByVal argMode As String,
                          ByVal argAllowToChangePfx As Boolean, ByVal argRow As Integer, ByVal argPrefix As String,
                            ByVal argCndOperator As String, ByVal argCndFldvalue As String)

        Dim blnEnable As Boolean
        blnEnable = False
        With argVsf
            Select Case argCndOperator
                Case "="
                    If UCase(Trim(.GetData(argRow, vOthFldDataValue))) = UCase(Trim(argCndFldvalue)) Then
                        argFrm.cboPfx.Text = argPrefix
                        blnEnable = True
                    End If
                Case ">"
                    If Val(.GetData(argRow, vOthFldDataValue)) > Val(argCndFldvalue) Then
                        argFrm.cboPfx.Text = argPrefix
                        blnEnable = True
                    End If
                Case "<"
                    If Val(.GetData(argRow, vOthFldDataValue)) < Val(argCndFldvalue) Then
                        argFrm.cboPfx.Text = argPrefix
                        blnEnable = True
                    End If
                Case "<>"
                    If UCase(Trim(.GetData(argRow, vOthFldDataValue))) <> UCase(Trim(argCndFldvalue)) Then
                        argFrm.cboPfx.Text = argPrefix
                        blnEnable = True
                    End If
                Case ">="
                    If Val(.GetData(argRow, vOthFldDataValue)) >= Val(argCndFldvalue) Then
                        argFrm.cboPfx.Text = argPrefix
                        blnEnable = True
                    End If
                Case "<="
                    If Val(.GetData(argRow, vOthFldDataValue)) <= Val(argCndFldvalue) Then
                        argFrm.cboPfx.Text = argPrefix
                        blnEnable = True
                    End If
            End Select
        End With
        Call cboPfxValidate(argFrm, argAllowToChangePfx, argTrnID, argMode, False)

        '# A - To lock the Pfx combo
        If argFrm.cboPfx.Text = argPrefix Then
            argFrm.cboPfx.Enabled = Not blnEnable
        End If
    End Sub

    Public Sub cboPfxValidate(ByVal argFrm As Object, ByVal argAllowToChangePfx As Boolean, ByVal argTrnID As Integer,
                              ByVal argMode As String, ByRef Cancel As Boolean)

        If argMode = "M" Then Exit Sub

        If argAllowToChangePfx Then
            Dim blnFound As Boolean
            Call clsSvtLib.getDataStringValue("Select Prefix from UtlDocNoPrefix where DocID = " & argTrnID &
                " and Prefix = '" & Trim(UCase(argFrm.cboPfx.Text)) & "'", "", blnFound)
            If Not blnFound Then
                MsgBox("Please enter a valid Prefix!!", vbInformation, argFrm.Text)
                Cancel = True
                Exit Sub
            End If
        End If

        Call argFrm.FillAutoGenTrnNo()

    End Sub

    Public Sub AddnlMSTFldCalTotal(ByRef argFrm As Object, ByVal argVSF As C1FlexGrid, ByVal argDetCols As Integer, ByVal argCboTxt As String, ByVal argCboTag As String, ByVal argIdtext As String)
        Dim i As Integer, strF As String, blnCalculate As Boolean

        With argVSF
            For i = 0 To .Rows.Count - 1
                strF = Trim(.GetData(i, vOthFldFormula))
                If strF <> "" Then
                    blnCalculate = True
                    If Trim(.GetData(i, vOthFldBlnReadOnly)) = "N" Then
                        If Trim(.GetData(i, vOthFldDataType)) <> "Numeric" Then
                            If Trim(.GetData(i, vOthFldDataValue)) = "" Then
                                blnCalculate = True
                            Else
                                blnCalculate = False
                            End If
                        Else
                            If Val(.GetData(i, vOthFldDataValue)) = 0 Then
                                blnCalculate = True
                            Else
                                blnCalculate = False
                            End If
                        End If
                    End If
                    If blnCalculate Then

                        .SetData(i, vOthFldDataValue, fnMstParse(argVSF, strF, .GetData(i, vOthFldCaption), argIdtext))
                    End If
                End If
            Next i
        End With

        Call vsfGenHideMndt(argFrm, argVSF)
    End Sub

    Public Function fnMstParse(ByVal vsfHdr As C1FlexGrid,
            ByVal strFormula As String, ByVal strFldName As String,
             ByVal strDocNo As String,
            Optional ByVal argCustSuppCode As String = "") As String

        fnMstParse = ""
        Dim i As Integer, blnFormula As Boolean
        'Dim strColNo As String, intColNo As Integer
        Try


            'in this place put all the rectifications that may
            'me done wrongly in Designer
            'one ex is given below

            '1
            strFormula = Replace(strFormula, "SVTCOLSUM (", "SVTCOLSUM(")
            strFormula = Replace(strFormula, "SVTCOLAVG (", "SVTCOLAVG(")
            '2.
            strFormula = Replace(strFormula, "svtAmtInwords (", "svtAmtInwords( ")


            With vsfHdr
                For i = 0 To .Rows.Count - 1
                    Select Case Trim(.GetData(i, vOthFldDataType))
                        Case "Numeric", "List Numeric"
                            If .GetData(i, vOthFldBlnComboData) = "Y" Then
                                strFormula = Replace(strFormula, "#" & .GetData(i, vOthFldCode) & "#", .GetData(i, vOthFldDataValue))
                            Else
                                strFormula = Replace(strFormula, "#" & .GetData(i, vOthFldCode) & "#", Format(Val(.GetData(i, vOthFldDataValue)), "0.00"))
                            End If
                        Case Else
                            strFormula = Replace(strFormula, "#" & .GetData(i, vOthFldCode) & "#", Trim(.GetData(i, vOthFldDataValue)))
                    End Select
                Next i
            End With

            blnFormula = True


            If InStr(strFormula, "SVTDATEDIFF") > 0 Then
                fnMstParse = GETDateDiff(strFormula)
                Exit Function
            End If

            If InStr(strFormula, "SYSTEMNAME") > 0 Then
                strFormula = gStrSystemName
                blnFormula = False
            End If


            'SVTCALCTAXAMT(#BASEAMTFLD#, #TAXPERFLDNAME#, 10)
            If InStr(strFormula, "SVTCALCTAXAMT") > 0 Then
                fnMstParse = Gettaxamt(strFormula)
                Exit Function
            End If

            If InStr(strFormula, "SVTDOCNO") > 0 Then
                strFormula = Replace(strFormula, "SVTDOCNO", strDocNo)
                blnFormula = False
            End If

            If InStr(strFormula, "SVTFYF") > 0 Then
                strFormula = Replace(strFormula, "SVTFYF", Right(gSettings.gCompStDt, 2))
                blnFormula = False
            End If

            If InStr(strFormula, "SVTFYT") > 0 Then
                strFormula = Replace(strFormula, "SVTFYT", Right(gSettings.gCompEndDt, 2))
                blnFormula = False
            End If


            If InStr(strFormula, "SVTCURDATE") > 0 Then
                strFormula = Replace(strFormula, "SVTCURDATE", Format(getSystemDate, "dd/MM/yyyy"))
                blnFormula = False
            End If

            If InStr(strFormula, "SVTCURTIME") > 0 Then
                strFormula = Replace(strFormula, "SVTCURTIME", Format(getSystemDateTime, "hh:mm:ss"))
                blnFormula = False
            End If


            If InStr(strFormula, "#SVTCUSTCODE#") > 0 Then
                strFormula = Replace(strFormula, "#SVTCUSTCODE#", argCustSuppCode)
                blnFormula = False
            End If

            '# A - #SVTSUPPCODE#
            If InStr(strFormula, "#SVTSUPPCODE#") > 0 Then
                strFormula = Replace(strFormula, "#SVTSUPPCODE#", argCustSuppCode)
                blnFormula = False
            End If


            '# A - #SVTCREDIT# - gConstCreditCol
            If InStr(strFormula, "#SVTCREDITCOL#") > 0 Then
                strFormula = Replace(strFormula, "#SVTCREDITCOL#", gConstCreditCol)
                blnFormula = False
            End If

            '# A - #SVTDEBIT# - gConstDebitCol
            If InStr(strFormula, "#SVTDEBITCOL#") > 0 Then
                strFormula = Replace(strFormula, "#SVTDEBITCOL#", gConstDebitCol)
                blnFormula = False
            End If

            'SVTCONCATENATE(#FLD1#, #FLD2#,…)
            If InStr(strFormula, "SVTCONCATENATE") > 0 Then
                strFormula = Replace(strFormula, "SVTCONCATENATE", "")
                fnMstParse = strFormula
                Exit Function
            End If

            'svtRoundOff
            If InStr(strFormula, "SVTROUND50") > 0 Then
                strFormula = Replace(strFormula, "SVTROUND50", "")
                fnMstParse = Format(ROUND50(fnEvaluate(strFormula)), "0.00")
                Exit Function
            End If

            If InStr(strFormula, "SVTROUND2") > 0 Then
                strFormula = Replace(strFormula, "SVTROUND2", "")
                fnMstParse = Format(ROUND2(fnEvaluate(strFormula)), "0.00")
                Exit Function
            End If

            If InStr(strFormula, "SVTROUND100") > 0 Then
                strFormula = Replace(strFormula, "SVTROUND100", "")
                fnMstParse = Format(ROUND100(fnEvaluate(strFormula)), "0.00")
                Exit Function
            End If

            '# A - Round off amt
            If InStr(strFormula, "SVTROUNDOFFAMT") > 0 Then
                strFormula = Replace(strFormula, "SVTROUNDOFFAMT(", "")
                strFormula = Replace(strFormula, ")", "")
                fnMstParse = Format(SVTROUNDOFFAMT(Val(strFormula)), "0.00")
                Exit Function
            End If

            'SVTAMTINWORDS(#NumFld#, #CurNameFld#)
            If InStr(strFormula, "SVTAMTINWORDS") > 0 Then
                Dim strCurName As String = "", strDecCurName As String = ""
                Dim iPos As Integer
                Dim strInMillion As String
                Dim strCaptionStyle As String
                Dim strAmt As String
                strFormula = Replace(strFormula, "SVTAMTINWORDS(", "")
                strFormula = Replace(strFormula, ")", "")

                iPos = InStr(1, strFormula, ",")
                If iPos <> 0 Then
                    strCurName = Mid(strFormula, iPos + 1)

                    If InStr(1, strCurName, ",") > 0 Then
                        strCurName = Mid(strCurName, 1, InStr(1, strCurName, ",") - 1)
                    End If

                    strDecCurName = Trim(clsSvtLib.getDataStringValue("Select Curdecimal from MstCur where Curname ='" & Trim(strCurName) & "'", "", False))
                End If

                strInMillion = Trim(clsSvtLib.getDataStringValue("Select WordsInMillions from MstCur where Curname ='" & Trim(strCurName) & "'", "N", False))
                strCaptionStyle = Mid(Right(strFormula, 2), 2, 2)
                strFormula = Replace(strFormula, strCurName, "")

                If iPos = 0 Or (Trim(strCurName) = "" Or Trim(strDecCurName) = "") Or (Trim(strCurName) = "-" Or Trim(strDecCurName) = "-") Then
                    '# A - Get Def Currency name
                    strCurName = gstrCurName  '"Rupee"
                    ' strDecCurName = gstrCurDecName '"Paise"
                End If

                strAmt = fnAmtInWords(CDbl(Val(strFormula)), strCurName, strDecCurName, strInMillion)
                Select Case Trim(strCaptionStyle)
                    Case "U"
                        strAmt = UPPERCASE(strAmt)
                    Case "L"
                        strAmt = LOWERCASE(strAmt)
                    Case Else
                        strAmt = TITLECASE(strAmt) 'Default is T - Title Case
                End Select

                fnMstParse = fnAmtInWords(CDbl(Val(strFormula)), strCurName, strDecCurName, strInMillion)
                fnMstParse = strAmt

                Exit Function
            End If
            If blnFormula Then
                fnMstParse = Format(clsSvtLib.getDataDoubleValue("Select " & strFormula & " as FldVal ", 0, False), "0.00")
            Else
                fnMstParse = strFormula
            End If
            Exit Function

        Catch ex As Exception
        End Try
    End Function

    Private Function GETDateDiff(ByVal argStr As String) As String
        'example
        Dim arr() As String, strInterval As String
        Dim strFirstDate As String, strSecondDate As String
        arr = Split(argStr, ",")
        strFirstDate = Replace(arr(0), "SVTDATEDIFF(", "")
        strFirstDate = Replace(strFirstDate, "#", "")
        strSecondDate = Replace(arr(1), "#", "")
        strInterval = Replace(arr(2), ")", "")
        strInterval = Replace(strInterval, "`", "")
        strInterval = Replace(strInterval, "'", "")
        strInterval = Replace(strInterval, " ", "")
        If IsDate(strFirstDate) And IsDate(strSecondDate) And (strInterval = "D" Or strInterval = "M") Then
            GETDateDiff = DateDiff(strInterval, strFirstDate, strSecondDate) + 1
        Else
            GETDateDiff = ""
        End If

    End Function

    Private Function Gettaxamt(ByVal argStr As String) As String
        'example
        'SVTCALCTAXAMT(#BASEAMTFLD#, #TAXPERFLDNAME#, 10)
        Dim arr() As String
        Dim strBaseAmt As String, strTaxper As String
        Dim intDecimals As Integer, dblTaxAmt As Double
        arr = Split(argStr, ",")
        Gettaxamt = ""
        Try

            strBaseAmt = Replace(arr(0), "SVTCALCTAXAMT(", "")
            strBaseAmt = Replace(strBaseAmt, "#", "")
            strTaxper = Replace(arr(1), "#", "")
            intDecimals = Val(Replace(arr(2), ")", ""))

            dblTaxAmt = SVTROUND(Val(strBaseAmt) * Val(strTaxper) / 100, 2)
            Select Case intDecimals
                Case 0
                    dblTaxAmt = SVTROUND(dblTaxAmt, 0)
                Case 10
                    'sngTaxAmt = ROUND10(sngTaxAmt, 0)
                Case 50
                    dblTaxAmt = ROUND50(dblTaxAmt)
                Case 100
                    dblTaxAmt = ROUND100(dblTaxAmt)
            End Select
            Gettaxamt = CStr(dblTaxAmt)
        Catch ex As Exception
            MsgBox(ex.Message & vbCrLf & "Error in " & argStr, MsgBoxStyle.Information, "Formula Error!")
        End Try

    End Function

    Public Function ROUND50(ByVal nRound50 As Double) As Double
        Dim strNumber As String
        Dim strDecimal As String
        ROUND50 = nRound50
        Try
            If nRound50 <> 0 Then
                strNumber = ParameterValue(".", nRound50, 1)
                strDecimal = ParameterValue(".", nRound50, 2)
                If strDecimal = "" Then
                    strDecimal = "00"
                End If
                If IsNumeric(nRound50) = True Then
                    If CLng(Left(strDecimal, 1)) >= 5 Then
                        strNumber = CLng(strNumber) + 1
                    End If

                    ROUND50 = strNumber
                Else
                    'MsgBox(LoadResString(117), vbCritical, LoadResString(109))
                End If
            End If

        Catch ex As Exception
            MsgBox(ex.Message & vbCrLf & "Error during Rounding", MsgBoxStyle.Information, "Rounding Error")
        End Try


    End Function

    Public Function ROUND100(ByVal nRound100 As Double) As Double
        Dim strNumber As String
        Dim strDecimal As String

        ROUND100 = nRound100
        Try

            If nRound100 <> 0 Then
                If IsNumeric(nRound100) = True Then
                    strNumber = ParameterValue(".", nRound100, 1)
                    strDecimal = ParameterValue(".", nRound100, 2)

                    If strDecimal <> "" Then
                        strNumber = CLng(strNumber) + 1
                        ROUND100 = strNumber
                    Else
                        ROUND100 = strNumber
                    End If
                Else
                    'MsgBox LoadResString(117), vbCritical, LoadResString(109)
                End If
            End If

        Catch ex As Exception
            MsgBox(ex.Message & vbCrLf & "Error during Rounding", MsgBoxStyle.Information, "Rounding Error")
        End Try

    End Function

    Public Function ROUND2(ByVal nRound2 As Double) As Double
        ROUND2 = Math.Round(nRound2, 2)
    End Function

    Public Function SVTROUND(ByVal aNumberToRound As Double,
      Optional ByVal aDecimalPlaces As Double = 0) As Double

        SVTROUND = aNumberToRound

        Dim nFactor As Double
        Dim nTemp As Double
        Try

            nFactor = 10 ^ aDecimalPlaces
            nTemp = (aNumberToRound * nFactor) + 0.5
            SVTROUND = Int(CDec(nTemp)) / nFactor
        Catch ex As Exception
            MsgBox(ex.Message & vbCrLf & "Error during Rounding", MsgBoxStyle.Information, "Rounding Error")
        End Try

    End Function

    Public Function SVTROUNDOFFAMT(ByVal nRound As Double) As Double


        SVTROUNDOFFAMT = nRound
        Try
            If nRound <> 0 Then
                If IsNumeric(nRound) = True Then
                    SVTROUNDOFFAMT = Math.Round(SVTROUND(nRound) - nRound, 2)
                End If
            End If

        Catch ex As Exception
            MsgBox(ex.Message & vbCrLf & "Error during Rounding", MsgBoxStyle.Information, "Rounding Error")
        End Try

    End Function

    ' This function is used to get the value from a string like a tokenizer
    Public Function ParameterValue(ByVal ParseCharacter As String,
                                   ByVal tString As Object,
                                   ByVal Index As Integer) As String

        Dim CurrentPosition As Integer                'used for the current position
        Dim ParseToPosition As Integer                'used for the postion from where the the parsing starts
        Dim CurrentToken As Integer                   'used for the Current Token position
        Dim TempString As String                      'used as a Temporary string
        ParameterValue = ""
        TempString = Trim(tString) + ParseCharacter   'set the Delimiter
        If Len(TempString) = 1 Then Exit Function 'check for the length
        CurrentPosition = 1                           'set the current position
        CurrentToken = 1                              'set the current Token
        Do                                            'Start the Do loop
            ParseToPosition = InStr(CurrentPosition, TempString,
            ParseCharacter)             'Use the Instr function for the Delimiter
            If Index = CurrentToken Then
                ParameterValue = Mid$(TempString, CurrentPosition,
                ParseToPosition - CurrentPosition)      'Use the Mid function to get the Output string after the Delimiter
                Exit Function
            End If
            CurrentToken = CurrentToken + 1
            CurrentPosition = ParseToPosition + 1
        Loop Until (CurrentPosition >= Len(TempString))

    End Function

    Private Function UPPERCASE(ByVal strUpperCase As String) As String
        UPPERCASE = UCase(strUpperCase)
    End Function

    Public Function LOWERCASE(ByVal strLowerCase As String) As String
        LOWERCASE = LCase(strLowerCase)
    End Function

    Public Function TITLECASE(ByVal strTitleCase As String) As String
        TITLECASE = StrConv(strTitleCase, vbProperCase)
    End Function

    Public Sub GetInsertString(ByVal argFrm As Object, ByRef strValues As String, ByRef strFlds As String,
                                    Optional ByVal argSVTRN As OleDb.OleDbTransaction = Nothing)
        Dim i As Integer, strSQL As String
        Dim strFldValue As String, strFirstDispCol As String
        Dim strMstTbl As String, strMstCode As String
        Dim strFldBlnComboData As String, strFldCmbTblName As String
        Dim strFldCmbStorageFldName As String, strFldCmbDisplayCols As String, strFldCmbAddnlCond As String

        Dim intLoadDocID As Integer
        strFlds = ""
        strValues = ""
        Dim ctrlList = GetAllDsgrVsf(argFrm, GetType(C1.Win.C1FlexGrid.C1FlexGrid))

        For Each argVsf As C1FlexGrid In ctrlList
            With argVsf
                'MsgBox(argVsf.Name)
                For i = 0 To .Rows.Count - 1
                    If Trim(.GetData(i, vOthFldBlnDispOnly)) = "Y" Then GoTo nexti
                    If Trim(.GetData(i, vOthPerAmount)) = "Y" Then
                        strFlds = strFlds & "," & Trim(.GetData(i, vOthFldCode)) & "PER"
                        strFldValue = Trim(.GetData(i, vOthFldDataPer))
                        strValues = strValues & ","
                        strValues = strValues & Val(strFldValue) & ""

                        strFlds = strFlds & "," & Trim(.GetData(i, vOthFldCode)) & "AMT"
                        strFldValue = Trim(.GetData(i, vOthFldDataValue))
                        strValues = strValues & ","
                        strValues = strValues & Val(strFldValue) & ""
                    Else
                        strFlds = strFlds & "," & Trim(.GetData(i, vOthFldCode))
                        strFldValue = Trim(.GetData(i, vOthFldDataValue))
                        If Trim(.GetData(i, vOthFldCmbTblName)) <> "" Then
                            If InStr(1, .GetData(i, vOthFldCmbDisplayCols), ",") > 0 Then
                                strFirstDispCol = Left(.GetData(i, vOthFldCmbDisplayCols) _
                                                    , InStr(1, .GetData(i, vOthFldCmbDisplayCols), ",") - 1)
                            Else
                                strFirstDispCol = .GetData(i, vOthFldCmbDisplayCols)
                            End If
                            strSQL = "Select " & .GetData(i, vOthFldCmbStorageFldName) & " from "
                            strSQL = strSQL & .GetData(i, vOthFldCmbTblName)
                            strSQL = strSQL & " where " & strFirstDispCol & " = '" & strFldValue & "'"

                            Select Case UCase(.GetData(i, vOthFldDataType))
                                Case "TEXT", "LIST STRING"
                                    strFldValue = clsSvtLib.getDataStringValue(strSQL, "", False, argSVTRN)
                                Case "NUMERIC", "LIST NUMERIC"
                                    strFldValue = clsSvtLib.getDataLongValue(strSQL, 0, False, argSVTRN)
                                Case Else
                                    strFldValue = clsSvtLib.getDataDateValue(strSQL, Nothing, False, argSVTRN)
                            End Select

                            'strFldValue = clsSvtLib.getDataLongValue(strSQL, 0, False, argSVTRN)
                        End If

                        If Trim(.GetData(i, vOthFldBlnLoadFrom)) = "Y" Then
                            If .GetData(i, vOthFldLoadFromType) = "S" Then
                                strMstTbl = "MstSupp"
                                strMstCode = "SuppCode"
                                strSQL = "Select DocID from UtlTransactions where DocName = 'Supplier Master'"
                                intLoadDocID = clsSvtLib.getDataLongValue(strSQL, 0, False, argSVTRN)
                            Else
                                strMstTbl = "MstCust"
                                strMstCode = "CustCode"
                                strSQL = "Select DocID from UtlTransactions where DocName = 'Customer Master'"
                                intLoadDocID = clsSvtLib.getDataLongValue(strSQL, 0, False, argSVTRN)
                            End If

                            strSQL = "Select FldBlnComboData from UtlDocDsgrFlds where DocID = " & intLoadDocID & " and FldCode = '"
                            strSQL = strSQL & UCase(Trim(.GetData(i, vOthFldLoadFldName))) & "'"
                            strFldBlnComboData = clsSvtLib.getDataStringValue(strSQL, "", False, argSVTRN)
                            If strFldBlnComboData = "Y" Then
                                strSQL = "Select FldCmbTblName from UtlDocDsgrFlds where DocID = " & intLoadDocID & " and FldCode = '"
                                strSQL = strSQL & UCase(Trim(.GetData(i, vOthFldLoadFldName))) & "'"
                                strFldCmbTblName = clsSvtLib.getDataStringValue(strSQL, "", False, argSVTRN)

                                strSQL = "Select FldCmbStorageFldName from UtlDocDsgrFlds where DocID = " & intLoadDocID & " and FldCode = '"
                                strSQL = strSQL & UCase(Trim(.GetData(i, vOthFldLoadFldName))) & "'"
                                strFldCmbStorageFldName = clsSvtLib.getDataStringValue(strSQL, "", False, argSVTRN)

                                strSQL = "Select FldCmbDisplayCols from UtlDocDsgrFlds where DocID = " & intLoadDocID & " and FldCode = '"
                                strSQL = strSQL & UCase(Trim(.GetData(i, vOthFldLoadFldName))) & "'"
                                strFldCmbDisplayCols = clsSvtLib.getDataStringValue(strSQL, "", False, argSVTRN)

                                strSQL = "Select FldCmbDisplayCols from FldCmbAddnlCond where DocID = " & intLoadDocID & " and FldCode = '"
                                strSQL = strSQL & UCase(Trim(.GetData(i, vOthFldLoadFldName))) & "'"
                                strFldCmbAddnlCond = clsSvtLib.getDataStringValue(strSQL, "", False, argSVTRN)

                                If strFldCmbTblName <> "" Then
                                    If InStr(1, strFldCmbDisplayCols, ",") > 0 Then
                                        strFirstDispCol = Left(strFldCmbDisplayCols,
                                        InStr(1, strFldCmbDisplayCols, ",") - 1)
                                    Else
                                        strFirstDispCol = strFldCmbDisplayCols
                                    End If
                                    strSQL = "Select " & strFldCmbStorageFldName & " from " & strFldCmbTblName
                                    strSQL = strSQL & " where " & strFirstDispCol & " = '" & strFldValue & "'"
                                    strFldValue = clsSvtLib.getDataLongValue(strSQL, 0, False, argSVTRN).ToString
                                End If
                            End If
                        End If

                        strValues = strValues & ","
                        Select Case .GetData(i, vOthFldDataType)
                            Case "Text", "List String"
                                If strFldValue = "" Then strFldValue = " "
                                strValues = strValues & "'" & strFldValue & "'"
                            Case "Date"
                                If strFldValue <> "" Then
                                    strValues = strValues & "'" & getDateFormatInInsert(strFldValue) & "'"
                                Else
                                    strValues = strValues & "Null"
                                End If
                            Case Else
                                strValues = strValues & Val(strFldValue) & ""
                        End Select
                    End If
nexti:
                Next i
            End With
        Next
        strValues = Mid(strValues, 1, Len(strValues)) & ")"
    End Sub

    Public Sub GetUpdateString(ByVal argFrm As Form, ByRef strFlds As String, Optional ByVal argSvtTrn As OleDb.OleDbTransaction = Nothing)
        Dim i As Integer, strSQL As String
        Dim strFldValue As String, strFirstDispCol As String
        strFlds = ""
        Dim ctrlList = GetAllDsgrVsf(argFrm, GetType(C1.Win.C1FlexGrid.C1FlexGrid))

        For Each argVsf As C1FlexGrid In ctrlList
            With argVsf
                For i = 0 To .Rows.Count - 1
                    If Trim(.GetData(i, vOthFldBlnDispOnly)) = "Y" Then GoTo nexti
                    If Trim(.GetData(i, vOthPerAmount)) = "Y" Then
                        strFldValue = Trim(.GetData(i, vOthFldDataPer))
                        strFlds = strFlds & ", " & Trim(.GetData(i, vOthFldCode)) & "PER = "
                        strFlds = strFlds & Val(strFldValue) & ""

                        strFldValue = Trim(.GetData(i, vOthFldDataValue))
                        strFlds = strFlds & ", " & Trim(.GetData(i, vOthFldCode)) & "AMT = "
                        strFlds = strFlds & Val(strFldValue) & ""
                    Else
                        strFldValue = Trim(.GetData(i, vOthFldDataValue))
                        If Trim(.GetData(i, vOthFldCmbTblName)) <> "" Then
                            ' # R Begin
                            If InStr(1, .GetData(i, vOthFldCmbDisplayCols), ",") > 0 Then
                                strFirstDispCol = Left(.GetData(i, vOthFldCmbDisplayCols) _
                                                    , InStr(1, .GetData(i, vOthFldCmbDisplayCols), ",") - 1)
                            Else
                                strFirstDispCol = .GetData(i, vOthFldCmbDisplayCols)
                            End If
                            strSQL = "Select " & .GetData(i, vOthFldCmbStorageFldName) & " from "
                            strSQL = strSQL & .GetData(i, vOthFldCmbTblName)
                            strSQL = strSQL & " where " & strFirstDispCol & " = '" & strFldValue & "'"
                            Select Case .GetData(i, vOthFldDataType)
                                Case "Text", "List String"
                                    strFldValue = clsSvtLib.getDataStringValue(strSQL, "", False, argSvtTrn)
                                Case "Date"
                                    strFldValue = clsSvtLib.getDataDateValue(strSQL, Nothing, False, argSvtTrn)
                                Case Else
                                    strFldValue = clsSvtLib.getDataLongValue(strSQL, 0, False, argSvtTrn)
                            End Select

                            'strFldValue = clsSvtLib.getDataLongValue(strSQL, 0, False, argSvtTrn)
                        End If

                        strFlds = strFlds & ", " & Trim(.GetData(i, vOthFldCode)) & " = "

                        Select Case .GetData(i, vOthFldDataType)
                            Case "Text", "List String"
                                If strFldValue = "" Then strFldValue = " "
                                strFlds = strFlds & "'" & strFldValue & "'"
                            Case "Date"
                                If strFldValue <> "" Then
                                    strFlds = strFlds & "'" & getDateFormatInInsert(strFldValue) & "'"
                                Else
                                    strFlds = strFlds & "Null"
                                End If
                            Case Else
                                strFlds = strFlds & Val(strFldValue) & ""
                        End Select
                    End If
nexti:
                Next i
            End With
        Next
    End Sub

    Public Function getValueFromAddnlGrid(ByVal argFldCode As String, ByVal argVSF As C1FlexGrid) As String
        Dim i As Integer
        getValueFromAddnlGrid = ""
        With argVSF
            For i = 0 To .Rows.Count - 1
                If Trim(UCase(argFldCode)) = Trim(UCase(.GetData(i, vOthFldCode))) Then
                    getValueFromAddnlGrid = .GetData(i, vOthFldDataValue)
                    Exit Function
                End If
            Next i
        End With
    End Function

    Public Sub InitDetAddnlGrid(ByVal argFrm As Object, ByVal argVsfDet As C1.Win.C1FlexGrid.C1FlexGrid, ByVal argVsfOptions As C1.Win.C1FlexGrid.C1FlexGrid,
                                ByVal argDetCols As Integer, ByVal argDtlTbl As String, ByVal argDocID As Integer, Optional argBlnFromMstTrn As Boolean = False)
        Dim i As Integer
        Dim rsDummy As OleDbDataReader
        Dim clsSvt As New svtLibrary


        'argVsfDet.Styles.Highlight.BackColor = Color.FromArgb(gIntGridHighlightClr)

        Dim rgHL As CellRange = argVsfDet.GetCellRange(0, argVsfDet.Cols.Count - 1)
        rgHL.Style = argVsfDet.Styles("bold")


        Dim csBlack As CellStyle = argVsfDet.Styles.Add("BLACK")
        Select Case UCase(argFrm.Name)
            Case "FRMTRNINV", "FRMTRNPB"
                csBlack.ForeColor = Color.White
            Case Else
                csBlack.ForeColor = Color.Black
        End Select


        Dim csRed As CellStyle = argVsfDet.Styles.Add("RED")
        Select Case UCase(argFrm.Name)
            Case "FRMTRNINV", "FRMTRNPB"
                csRed.ForeColor = Color.White
            Case Else
                csRed.ForeColor = Color.DarkRed
        End Select


        Dim csBlue As CellStyle = argVsfDet.Styles.Add("BLUE")
        Select Case UCase(argFrm.Name)
            Case "FRMTRNINV", "FRMTRNPB"
                csBlue.ForeColor = Color.White
            Case Else
                csBlue.ForeColor = Color.Blue
        End Select

        If argBlnFromMstTrn Then
            rsDummy = clsSvt.getDataReaderFromSQL(" Select d.* from vwAddnlFldsHFTSec d, UtlTransactions t " _
                    & " where d.DocId = t.DocID and t.DocID = " & argDocID _
                    & " order by FldOrderBy")
        Else
            rsDummy = clsSvt.getDataReaderFromSQL(" Select d.* from vwAddnlFldsDetailSec d, UtlTransactions t " _
                    & " where d.DocId = t.DocID and t.DocID = " & argDocID _
                    & " order by FldOrderBy")
        End If

        Dim cs As CellStyle = argVsfDet.Styles.Add("bold")
        cs.Font = New Font("Microsoft Sans Serif", 8.25, FontStyle.Bold)

        argVsfOptions.Clear(ClearFlags.All)
        argVsfDet.Cols.Count = argDetCols

        argVsfDet.DrawMode = C1.Win.C1FlexGrid.DrawModeEnum.OwnerDraw


        With argVsfOptions
            .Cols(0).Width = 2000
            .Cols.Count = intTotCols

            .Rows.Count = argDetCols
            .Cols(vOthFldDataValue).TextAlign = TextAlignEnum.LeftCenter

            If Not rsDummy.HasRows Then
                rsDummy.Close()
                Exit Sub
            End If
            '----------------------------------

            Do While rsDummy.Read
                argVsfDet.Cols.Count = argVsfDet.Cols.Count + 1
                i = argVsfDet.Cols.Count - 1
                argVsfDet.SetData(0, i, rsDummy!Fldcaption)
                .Rows.Count = .Rows.Count + 1
                i = .Rows.Count - 1
                'MsgBox(rsDummy!Fldcaption)
                .SetData(i, vOthFldCode, rsDummy!FldCode)
                .SetData(i, vOthFldCaption, rsDummy!Fldcaption)
                .SetData(i, vOthFldDataType, rsDummy!FldDataType)
                'If (rsDummy!FldDataType) = "Date" Then
                '    argVsfDet.Cols(i).DataType = GetType(Date)
                'End If
                .SetData(i, vOthFldDataWidth, rsDummy!FldDataWidth)
                .SetData(i, vOthFldBlnReadOnly, rsDummy!FldBlnReadOnly)
                .SetData(i, vOthFLdBlnHide, rsDummy!FldBlnHide)
                .SetData(i, vOthFldBlnMandatory, rsDummy!FldBlnMandatory)
                .SetData(i, vOthFldUnique, rsDummy!FldUnique)
                .SetData(i, vOthFldUseDefault, rsDummy!FldUseDefault)
                .SetData(i, vOthFldRowClrFontBold, rsDummy!FldRowClrFontBold)
                .SetData(i, vOthFldRowClrForeColour, rsDummy!FldRowClrForeColour)
                .SetData(i, vOthFldBlnComboData, rsDummy!FldBlnComboData)
                .SetData(i, vOthFldBlnRefill, rsDummy!FldBlnRefill)
                .SetData(i, vOthFldOrderBy, rsDummy!FldOrderBy)
                .SetData(i, vOthFldFormula, Replace(GetVal(rsDummy!FldFormula, ""), "`", "'"))
                .SetData(i, vOthFldBlnDispOnly, rsDummy!FldBlnDispOnly)
                .SetData(i, vOthFldPickUP, GetVal(rsDummy!FldPickUP, ""))
                .SetData(i, vOthFldNumBlnAnyNumber, rsDummy!FldNumBlnAnyNumber)
                .SetData(i, vOthFldNumBlnPercentage, rsDummy!FldNumBlnPercentage)
                .SetData(i, vOthFldNumDecimals, rsDummy!FldNumDecimals)

                If rsDummy!FldNumDecimals > 0 Then
                    argVsfDet.Cols(i).Format = "0." & Strings.StrDup(rsDummy!FldNumDecimals, "0")
                End If

                .SetData(i, vOthFldNumBlnAllowNegative, rsDummy!FldNumBlnAllowNegative)
                .SetData(i, vOthFldNumBlnRange, rsDummy!FldNumBlnRange)
                .SetData(i, vOthFldNumBlnRangeFromChk, rsDummy!FldNumBlnRangeFromChk)
                .SetData(i, vOthFldNumRangeFromOperator, rsDummy!FldNumRangeFromOperator)
                .SetData(i, vOthFldNumRangeFromValue, rsDummy!FldNumRangeFromValue)
                .SetData(i, vOthFldNumBlnRangeToChk, rsDummy!FldNumBlnRangeToChk)
                .SetData(i, vOthFldNumRangeToOperator, GetVal(rsDummy!FldNumRangeToOperator, ""))
                .SetData(i, vOthFldNumRangeToValue, GetVal(rsDummy!FldNumRangeToValue, ""))
                .SetData(i, vOthFldBlnNumOrStringList, rsDummy!FldBlnNumOrStringList)
                .SetData(i, vOthFldNumStringListContent, rsDummy!FldNumStringListContent)
                .SetData(i, vOthFldDtBlnAnyDate, rsDummy!FldDtBlnAnyDate)
                .SetData(i, vOthFldDtBlnWithinFinYear, rsDummy!FldDtBlnWithinFinYear)
                .SetData(i, vOthFldDtBlnRange, rsDummy!FldDtBlnRange)
                .SetData(i, vOthFldDtBlnRangeFrom, rsDummy!FldDtBlnRangeFrom)
                .SetData(i, vOthFldDtRangeFromOperator, rsDummy!FldDtRangeFromOperator)
                .SetData(i, vOthFldDtRangeFromValue, rsDummy!FldDtRangeFromValue)
                .SetData(i, vOthFldDtBlnRangeTo, rsDummy!FldDtBlnRangeTo)
                .SetData(i, vOthFldDtRangeToOperator, rsDummy!FldDtRangeToOperator)
                .SetData(i, vOthFldDtRangeToValue, rsDummy!FldDtRangeToValue)
                .SetData(i, vOthFldDtBlnAsEntered, rsDummy!FldDtBlnAsEntered)
                .SetData(i, vOthFldDtBlnFirstDOM, rsDummy!FldDtBlnFirstDOM)
                .SetData(i, vOthFldDtBlnLastDOM, rsDummy!FldDtBlnLastDOM)
                .SetData(i, vOthFldTxtStringType, rsDummy!FldTxtStringType)
                .SetData(i, vOthFldTxtCase, rsDummy!FldTxtCase)
                .SetData(i, vOthFldTxtBlnDoNotAllowChk, rsDummy!FldTxtBlnDoNotAllowChk)
                .SetData(i, vOthFldTxtDoNotAllowChars, rsDummy!FldTxtDoNotAllowChars)
                .SetData(i, vOthFldTxtBlnMask, rsDummy!FldTxtBlnMask)
                .SetData(i, vOthFldTxtMaskChar, rsDummy!FldTxtMaskChar)
                .SetData(i, vOthFldBlnRowClr, rsDummy!FldBlnRowClr)
                .SetData(i, vOthFldBlnFontBold, rsDummy!FldBlnFontBold)
                .SetData(i, vOthFldHideCompareVal, rsDummy!FldHideCompareVal)
                .SetData(i, vOthFldHideOperator, rsDummy!FldHideOperator)
                .SetData(i, vOthFldHideWithVal, rsDummy!FldHideWithVal)
                .SetData(i, vOthFldMndtryCompareVal, rsDummy!FldMndtryCompareVal)
                .SetData(i, vOthFldMndtryOperator, rsDummy!FldMndtryOperator)
                .SetData(i, vOthFldMndtryWithVal, rsDummy!FldMndtryWithVal)
                .SetData(i, vOthFldROCompareVal, rsDummy!FldROCompareVal)
                .SetData(i, vOthFldROOperator, rsDummy!FldROOperator)
                .SetData(i, vOthFldROWithVal, rsDummy!FldROWithVal)
                .SetData(i, vOthFldCmbTblName, rsDummy!FldCmbTblName)
                .SetData(i, vOthFldCmbStorageFldName, rsDummy!FldCmbStorageFldName)
                .SetData(i, vOthFldCmbDisplayCols, rsDummy!FldCmbDisplayCols)
                .SetData(i, vOthFldCmbAddnlCond, rsDummy!FldCmbAddnlCond)
                .SetData(i, vOthFldDefStdDefault, rsDummy!FldDefStdDefault)
                .SetData(i, vOthFldDefBlnAgnstCond, rsDummy!FldDefBlnAgnstCond)
                .SetData(i, vOthFldDefCondList, rsDummy!FldDefCondList)
                .SetData(i, vOthFldTxtBlnMultiLine, rsDummy!FldTxtBlnMultiLine)
                .SetData(i, vOthFldToolTip, rsDummy!FldToolTip)
                .SetData(i, vOthFldUniqueBasedOn, rsDummy!FldUniqueBasedOn)
                .SetData(i, vOthFldTxtAlignment, rsDummy!FldTxtAlignment)
                .SetData(i, vOthFldBlnLoadFrom, rsDummy!FldBlnLoadFrom)
                .SetData(i, vOthFldLoadFldName, GetVal(rsDummy!FldLoadFldName, ""))
                .SetData(i, vOthFldLoadFromType, GetVal(rsDummy!FldLoadFromType, ""))
                .SetData(i, vOthFldClrBefHide, GetVal(rsDummy!FldClrBefHide, ""))
                .SetData(i, vOthFldPickUP, GetVal(rsDummy!FldPickUP, ""))
                .SetData(i, vOthFldPickUPAfterFld, GetVal(rsDummy!FldPickUPAfterFld, ""))

                .SetData(i, vOthFldBlnApportinate, GetVal(rsDummy!BlnApportinate, ""))
                .SetData(i, vOthFLdApportinate, GetVal(rsDummy!FldApportinate, ""))
                .SetData(i, vOthFldBasedOn, GetVal(rsDummy!BasedOn, ""))
                '.SetData(i, vOthBlnHideForPend, GetVal(rsDummy!HideForPendInfo, ""))

                .SetData(i, vOthFldFontName, GetVal(rsDummy!FldFontName, "Times New Roman"))
                .SetData(i, vOthFldFontSize, GetVal(rsDummy!FldFontSize, "10"))
                .SetData(i, vOthBlnCopyPreviousRowData, GetVal(rsDummy!BlnCopyPreviousRowData, ""))
                .SetData(i, vOthBlnLoadColFrom, GetVal(rsDummy!BlnLoadColFrom, "N"))
                '.SetData(i, vOthFldLoadColFrom, GetVal(rsDummy!FldLoadColFrom, ""))
                .SetData(i, vOthBlnTogHide, GetVal(rsDummy!BlnTogHide, "N"))
                .SetData(i, vOthFldBlnNewTrnCalc, GetVal(rsDummy!FldBlnNewTrnCalc, "N"))
                .SetData(i, vOthFldBlnChkDuplicate, GetVal(rsDummy!FldBlnChkDuplicate, "N"))
                .SetData(i, vOthBlnTotalReq, GetVal(rsDummy!BlnTotalReq, "N"))
                .SetData(i, vOthBlnLoadPrevTrnData, GetVal(rsDummy("LoadPrevTrnData"), "N"))
                .SetData(i, vOthBlnLoadPrevTrnParty, GetVal(rsDummy("LoadPrevDataParty"), "N"))
                .SetData(i, vOthBlnLoadPrevTrnItem, GetVal(rsDummy("LoadPrevDataItem"), "N"))
                .SetData(i, vOthRoundType, GetVal(rsDummy("RoundType"), "None"))
                .SetData(i, vOthCSVFieldName, GetVal(rsDummy("CSVFieldName"), ""))
                .SetData(i, vOthFldPrevTranFldName, GetVal(rsDummy("PrevTrnFldName"), ""))
                'Microsoft Sans Serif, 8.25pt

                If argVsfOptions.GetData(i, vOthFldBlnFontBold) = "Y" Then
                    Dim rg As CellRange = argVsfDet.GetCellRange(0, argVsfDet.Cols.Count - 1)
                    rg.Style = argVsfDet.Styles("bold")
                End If

                'If argVsfOptions.setdata(i, vOthFldBlnRowClr) = "Y" Then
                '    If argVsfOptions.setdata(i, vOthFldRowClrForeColour) <> "" Then
                '        argVsfDet.Cell(flexcpBackColor, 0, argVsfDet.Cols.Count - 1) = _
                '           argVsfOptions.setdata(i, vOthFldRowClrForeColour)
                '    End If
                'End If

                If argVsfOptions.GetData(i, vOthFldTxtAlignment) <> "" Then
                    Select Case argVsfOptions.GetData(i, vOthFldTxtAlignment)
                        Case "Left"
                            argVsfDet.Cols(argVsfDet.Cols.Count - 1).TextAlign = TextAlignEnum.LeftCenter
                        Case "Right"
                            argVsfDet.Cols(argVsfDet.Cols.Count - 1).TextAlign = TextAlignEnum.RightCenter
                        Case "Centre"
                            argVsfDet.Cols(argVsfDet.Cols.Count - 1).TextAlign = TextAlignEnum.CenterCenter
                    End Select
                End If
                'argVsfDet.Cell(flexcpFontName, 0, argVsfDet.Cols.Count - 1) = clsSvt.GetVal(rsDummy!FldFontName, "Times New Roman")
                'argVsfDet.Cell(flexcpFontSize, 0, argVsfDet.Cols.Count - 1) = clsSvt.GetVal(rsDummy!FldFontSize, 10)
            Loop
            rsDummy.Close()
        End With


        With argVsfOptions
            For i = argDetCols To .Rows.Count - 1

                'for hiding
                If .GetData(i, vOthFLdBlnHide) = "Y" And
                    Trim(.GetData(i, vOthFldHideOperator)) = "" Then
                    argVsfDet.Cols(i).Visible = False
                Else
                    argVsfDet.Cols(i).Visible = True
                End If

                'reset to black fore colour
                Dim rgBlack As CellRange = argVsfDet.GetCellRange(0, i)
                rgBlack.Style = argVsfDet.Styles("BLACK")

                'make it red in case of mandatory without condition
                If .GetData(i, vOthFldBlnMandatory) = "Y" And Trim(.GetData(i, vOthFldMndtryOperator)) = "" Then
                    Dim rgRed As CellRange = argVsfDet.GetCellRange(0, i)
                    rgRed.Style = argVsfDet.Styles("RED")
                End If

                'make it blue in case of readonly without condition
                If .GetData(i, vOthFldBlnReadOnly) = "Y" And Trim(.GetData(i, vOthFldROOperator)) = "" Then
                    Dim rgBlue As CellRange = argVsfDet.GetCellRange(0, i)
                    rgBlue.Style = argVsfDet.Styles("BLUE")
                End If

                'Toggle Hide 
                If .GetData(i, vOthBlnTogHide) = "Y" Then
                    argVsfDet.Cols(i).Visible = True
                End If
            Next i
        End With
    End Sub

    Public Function vsfOptBeforeEditCancel(ByVal argFrm As Object, ByVal argVsfDet As C1FlexGrid,
                                           ByVal argVsfOptions As C1FlexGrid, ByVal argDtlTblName As String, ByVal argMode As String,
        ByVal argRow As Integer, ByVal argCol As Integer, ByVal argDetCols As Integer) As Boolean

        Dim strCondition As String = "", strFlds As String
        vsfOptBeforeEditCancel = False
        If argMode = "V" Or argMode = "" Then
            vsfOptBeforeEditCancel = True
            Exit Function
        End If

        With argVsfOptions
            'restrict entry into read only columns
            If .GetData(argCol, vOthFldBlnReadOnly) = "Y" Then
                If .GetData(argCol, vOthFldTxtBlnMultiLine) = "Y" Then
                    argVsfDet.ComboList = "..."
                    Exit Function
                End If
                If .GetData(argCol, vOthFldROOperator) = "" Then
                    vsfOptBeforeEditCancel = True
                    Exit Function
                Else
                    If fnExpEvaluate(.GetData(argCol, vOthFldROCompareVal),
                                                .GetData(argCol, vOthFldROWithVal),
                                                .GetData(argCol, vOthFldROOperator),
                                                  argFrm, "", argRow) Then
                        vsfOptBeforeEditCancel = True
                        Exit Function
                    End If
                End If
            End If

            argVsfDet.ComboList = ""
            argVsfDet.EditMask = ""

            'fill from list of options
            If Mid(.GetData(argCol, vOthFldDataType), 1, 4) = "List" Then
                argVsfDet.ComboList = .GetData(argCol, vOthFldNumStringListContent)
                Exit Function
            End If

            'fill from list of options
            If .GetData(argCol, vOthFldTxtBlnMultiLine) = "Y" Then
                argVsfDet.ComboList = "..."
                Exit Function
            End If

            'fill for multi combo cols
            If Trim(.GetData(argCol, vOthFldCmbTblName)) <> "" Then
                strFlds = .GetData(argCol, vOthFldCmbDisplayCols)
                If .GetData(argCol, vOthFldCmbAddnlCond) <> "" Then
                    strCondition = " Where " & Replace(.GetData(argCol, vOthFldCmbAddnlCond), "`", "'", 1)
                End If
                argVsfDet.ComboList = genMultiColVsfComboString(.GetData(argCol,
                    vOthFldCmbTblName), strCondition & " order by 1", strFlds)
            End If

            If .GetData(argCol, vOthFldBlnRefill) = "Y" Then
                argVsfDet.ComboList = GenVSfComboStringCond(.GetData(argCol, vOthFldCode),
                        argDtlTblName, " order by 1")
            End If

            If .GetData(argCol, vOthFldDataType) = "Date" Then
                argVsfDet.EditMask = "##/##/####"
            End If
        End With
        vsfOptBeforeEditCancel = False
    End Function

    Public Function vsfOptKeyPressEdit(ByVal argFrm As Object, ByVal argVsfDet As C1FlexGrid, ByVal argVsfOptions As C1FlexGrid,
                                       ByVal argDetCol As Integer, ByVal argDetRow As Integer, ByVal argKeyChar As String) As Boolean
        With argVsfOptions
            If Trim(.GetData(argDetCol, vOthFldBlnComboData)) = "Y" Then
                vsfOptKeyPressEdit = True
                Exit Function
            End If
            Dim keyAscii As Integer
            keyAscii = Asc(argKeyChar)

            If UCase$(.GetData(argDetCol, vOthFldDataType)) = "NUMERIC" And
                    Trim(.GetData(argDetCol, vOthFldCmbDisplayCols)) <> "" Then
                vsfOptKeyPressEdit = KeyValidate("String", argKeyChar)
                If keyAscii = Asc("'") Then keyAscii = Asc("`")
            ElseIf UCase$(.GetData(argDetCol, vOthFldDataType)) = "NUMERIC" Then
                vsfOptKeyPressEdit = KeyValidate("Decimal", argKeyChar,
                                Val(.GetData(argDetCol, vOthFldDataWidth)))
            End If

            Select Case .GetData(argDetCol, vOthFldDataType)
                Case "Text"
                    Select Case .GetData(argDetCol, vOthFldTxtStringType)
                        Case "ALPHANUMBER"
                            If Not ((keyAscii >= Asc("a") And keyAscii <= Asc("z")) Or
                                (keyAscii >= Asc("A") And keyAscii <= Asc("Z")) Or
                                (keyAscii >= Asc("0") And keyAscii <= Asc("9"))) And
                                keyAscii <> 8 And keyAscii <> 32 Then
                                vsfOptKeyPressEdit = False
                                Exit Function
                            End If
                            If InStr(1, .GetData(argDetCol _
                                 , vOthFldTxtDoNotAllowChars), Chr(keyAscii)) > 0 Then
                                vsfOptKeyPressEdit = False
                                Exit Function
                            End If
                            vsfOptKeyPressEdit = True
                        Case "ALPHABET"
                            If Not ((keyAscii >= Asc("a") And keyAscii <= Asc("z")) Or
                                (keyAscii >= Asc("A") And keyAscii <= Asc("Z"))) And
                                keyAscii <> 8 And keyAscii <> 32 Then
                                vsfOptKeyPressEdit = False
                                Exit Function
                            End If
                            If InStr(1, .GetData(argDetCol, vOthFldTxtDoNotAllowChars), Chr(keyAscii)) > 0 Then
                                vsfOptKeyPressEdit = False
                                Exit Function
                            End If
                            vsfOptKeyPressEdit = True
                        Case "NUMBER"
                            If Not (keyAscii >= Asc("0") And keyAscii <= Asc("9")) And
                                keyAscii <> 8 And keyAscii <> 46 Then
                                vsfOptKeyPressEdit = False
                                Exit Function
                            End If
                            If InStr(1, .GetData(argDetCol, vOthFldTxtDoNotAllowChars), Chr(keyAscii)) > 0 Then
                                vsfOptKeyPressEdit = False
                                Exit Function
                            End If
                            vsfOptKeyPressEdit = True
                        Case Else
                            If InStr(1, .GetData(argDetCol, vOthFldTxtDoNotAllowChars), Chr(keyAscii)) > 0 Then
                                vsfOptKeyPressEdit = False
                                Exit Function
                            End If
                            vsfOptKeyPressEdit = True
                    End Select

                Case "Numeric"
                    'MsgBox keyAscii
                    If .GetData(argDetCol, vOthFldNumBlnAnyNumber) = "Y" Then
                        If .GetData(argDetCol, vOthFldNumBlnAllowNegative) = "Y" Then
                            If Not (keyAscii >= Asc("0") And keyAscii <= Asc("9")) And
                                keyAscii <> 8 And keyAscii <> 45 And keyAscii <> 46 Then
                                vsfOptKeyPressEdit = False
                                Exit Function
                            End If
                        Else
                            If Not (keyAscii >= Asc("0") And keyAscii <= Asc("9")) And
                                keyAscii <> 8 And keyAscii <> 46 Then
                                vsfOptKeyPressEdit = False
                                Exit Function
                            End If
                        End If
                    Else
                        If Not (keyAscii >= Asc("0") And keyAscii <= Asc("9")) And
                            keyAscii <> 8 And keyAscii <> 46 Then
                            vsfOptKeyPressEdit = False
                            Exit Function
                        End If
                    End If
            End Select
            '# R End

        End With
        vsfOptKeyPressEdit = True
    End Function

    'Public Sub vsfOptKeyUp(ByVal argForm As Object, ByVal argIntDetCols As Integer)
    '    Dim i As Integer
    '    ' # R Begin 1607
    '    For i = argIntDetCols To argForm.vsfDet.Cols - 1
    '        If argForm.VsfOptions.TextMatrix(i, vOthFldBlnFontBold) = "Y" Then
    '            argForm.vsfDet.Cell(flexcpFontBold, argForm.vsfDet.Row, i) = True
    '        End If
    '        If argForm.VsfOptions.TextMatrix(i, vOthFldBlnRowClr) = "Y" Then
    '            If argForm.VsfOptions.TextMatrix(i, vOthFldRowClrForeColour) <> "" Then
    '                argForm.vsfDet.Cell(flexcpBackColor, argForm.vsfDet.Row, i) = _
    '                    argForm.VsfOptions.TextMatrix(i, vOthFldRowClrForeColour)
    '            End If
    '        End If

    '        If argForm.VsfOptions.TextMatrix(i, vOthFldTxtAlignment) <> "" Then
    '            Select Case argForm.VsfOptions.TextMatrix(i, vOthFldTxtAlignment)
    '                'argForm.vsfGen(intType).Col = vOthFldDataValue
    '                Case "Left"
    '                    argForm.vsfDet.Cell(flexcpAlignment, argForm.vsfDet.Row, i) = flexAlignLeftCenter
    '                    'argForm.vsfGen(intType).CellAlignment = flexAlignLeftCenter
    '                Case "Right"

    '                    argForm.vsfDet.Cell(flexcpAlignment, argForm.vsfDet.Row, i) = flexAlignRightCenter
    '                    '                        argForm.vsfGen(intType).CellAlignment = flexAlignRightCenter
    '                Case "Centre"
    '                    argForm.vsfDet.Cell(flexcpAlignment, argForm.vsfDet.Row, i) = flexAlignCenterCenter
    '                    '                        argForm.vsfGen(intType).CellAlignment = flexAlignCenterTop
    '            End Select

    '        End If
    '        If argForm.VsfOptions.TextMatrix(i, vOthFldUseDefault) = "Y" Then
    '            'If Trim(argForm.VSFdet.TextMatrix(i, vOthFldDataValue)) = "" Then
    '            argForm.vsfDet.TextMatrix(argForm.vsfDet.Row, i) = argForm.VsfOptions.TextMatrix(i, vOthFldDefStdDefault)
    '            'End If
    '        End If
    '    Next i

    '    ' # R End


    'End Sub

    Private Function CheckAlphaNumeric(ByVal str As String) As Boolean
        If InStr(str, "/") > 0 Then Return True
        Return Regex.Match(str.Trim(), "^[a-zA-Z0-9\s_@.-]*$").Success
    End Function

    Public Function vsfOptValidateEdit(ByVal argFrm As Object, ByVal argVsfDet As C1FlexGrid, ByVal argVsfOptions As C1FlexGrid,
                                       ByVal argRow As Integer, ByVal argCol As Integer _
                                    , ByVal strMstOrTrans As String, ByVal strTab As String _
                                    , ByVal strPrimKey As String, ByVal strValPrim As String, Optional ByVal strParty As String = "") As Boolean
        Dim x As Integer, strsql As String, i As Integer
        Dim strVal As String, strGridValue As String

        Dim blnValid As Boolean, strFirstDispCol As String, blnFlag As Boolean
        With argVsfOptions
            vsfOptValidateEdit = False

            'for data pickup feature
            For x = 0 To argVsfOptions.Rows.Count - 1
                If Trim(argVsfOptions.GetData(x, vOthFldPickUPAfterFld)) <> "" Then
                    If Trim(Replace(argVsfOptions.GetData(x, vOthFldPickUPAfterFld), "#", "")) = Trim(argVsfOptions.GetData(argCol, vOthFldCode)) Then
                        strsql = Replace(Trim(argVsfOptions.GetData(x, vOthFldPickUP)), "`", "'")
                        strsql = Replace(strsql, "#", "")
                        strsql = Replace(strsql, "SVTSUPPCODE", strParty)
                        strsql = Replace(strsql, "SVTCUSTCODE", strParty)
                        strsql = Replace(strsql, "SVTUSERCODE", gUserCode)
                        strsql = Replace(strsql, "SVTUSERID", gUserID)
                        If strsql <> "" Then
                            For i = 0 To argVsfOptions.Rows.Count - 1
                                strVal = GetVal(argVsfDet.GetData(argRow, i), "")
                                If i + 1 = argCol Then
                                    If Not IsNothing(argVsfDet.Editor.Text) Then
                                        strVal = Trim(argVsfDet.Editor.Text)
                                        'faced this issue, in while creating JobWork Production--JWName has a pickupqry after jwbarcode, its considering .editor.text  --- Satish 15feb2018
                                        strsql = Replace(strsql, Trim(argVsfOptions.GetData(argCol, vOthFldCode)), strVal)  'Satish15feb2018
                                    Else
                                        strVal = ""
                                    End If
                                End If
                                'Debug.Print " Replace <<" & i + 1 & ">> with ", strVal
                                strsql = Replace(strsql, "<<" & i + 1 & ">>", strVal)
                            Next i
                            strsql = ReplSVTExpWithConstantOrValues(strsql, "TEXT", argFrm, argCol, argRow)
                            argVsfDet.SetData(argRow, x, getRsVal(strsql))
                        End If
                    End If
                End If
            Next x

            '# Code for mandatory
            If .GetData(argCol, vOthFldBlnMandatory) = "Y" Then
                If fnExpEvaluate(.GetData(argCol, vOthFldMndtryCompareVal),
                                            .GetData(argCol, vOthFldMndtryWithVal),
                                            .GetData(argCol, vOthFldMndtryOperator),
                                                argFrm, argVsfOptions.GetData(argCol, vOthFldDataType), argRow) Then
                    'If Trim(.Editor.text) = "" And Val(.Editor.text) = 0 Then
                    'If argVsfDet.Editor Is Nothing Then Exit Function

                    strGridValue = Trim(argVsfDet.Editor.Text)
                    'strGridValue = Val(argVsfDet.Editor.Text)
                    'Or Val(argVsfDet.Editor.Text) = 0
                    'Trim(Int(Convert.ToDecimal(argVsfDet.Editor.Text))) = "0"
                    If Trim(argVsfDet.Editor.Text) = "" Or Trim(argVsfDet.Editor.Text) = "__/__/____" Then
                        MsgBox("Mandatory field: '" & .GetData(argCol, vOthFldCaption) & "' cannot be blank!", vbInformation, argFrm.text)
                        Exit Function
                    End If
                    If Trim(argVsfOptions.GetData(argCol, vOthFldCmbTblName)) = "" Then
                        If Not CheckAlphaNumeric(argVsfDet.Editor.Text) Then
                            If Val((Convert.ToDecimal(argVsfDet.Editor.Text.ToString().Replace("/", "").ToString().Replace("_", "")))) = 0 Then
                                MsgBox("Mandatory field: '" & .GetData(argCol, vOthFldCaption) & "' cannot be 0!", vbInformation, argFrm.text)
                                Exit Function
                            End If
                        End If
                    End If
                End If
            End If

            'code to validate combo
            If .GetData(argCol, vOthFldCmbTblName) <> "" Then
                If Trim(argVsfDet.Editor.Text) <> "" Then
                    If InStr(1, .GetData(argCol, vOthFldCmbDisplayCols), ",") > 0 Then
                        strFirstDispCol = Left(.GetData(argCol, vOthFldCmbDisplayCols) _
                                            , InStr(1, .GetData(argCol, vOthFldCmbDisplayCols), ",") - 1)
                    Else
                        strFirstDispCol = .GetData(argCol, vOthFldCmbDisplayCols)
                    End If
                    Call clsSvtLib.getDataStringValue("Select " & .GetData(argCol, vOthFldCmbStorageFldName) & " from " & .GetData(argCol, vOthFldCmbTblName) _
                                                      & " where " & strFirstDispCol & " = '" & argVsfDet.Editor.Text & "'", "", blnFlag)
                    If Not blnFlag Then
                        'MsgBox .getData(argCol, vOthFldCaption) & "[" & argVsfDet.Editor.text & "] ...  Not found in " & .getData(argRow, vOthFldCmbTblName) & " !!", vbInformation, argFrm.Caption
                        '# A
                        MsgBox(.GetData(argCol, vOthFldCaption) & "[" & argVsfDet.Editor.Text & "] ...  Not found in " &
                               .GetData(argCol, vOthFldCmbTblName) & " !!", MsgBoxStyle.Information, argFrm.text)
                        argVsfDet.Focus()
                        Exit Function
                    End If
                End If
                vsfOptValidateEdit = True
                Exit Function
            End If

            '# R Begin
            If .GetData(argCol, vOthFldDataType) = "Numeric" Or .GetData(argCol, vOthFldDataType) = "List Numeric" Then
                If .GetData(argCol, vOthFldNumBlnPercentage) = "Y" Then
                    If .GetData(argCol, vOthFldBlnMandatory) = "Y" Then
                        If Val(argVsfDet.Editor.Text) <= 0 Or Val(argVsfDet.Editor.Text) > 100 Then
                            MsgBox("Invalid percentage should range 1-100 !!!", vbInformation, argFrm.text)
                            Exit Function
                        End If
                    Else
                        If Val(argVsfDet.Editor.Text) < 0 Or Val(argVsfDet.Editor.Text) > 100 Then
                            MsgBox("Invalid percentage should range 0-100 !!!", vbInformation, argFrm.text)
                            Exit Function
                        End If
                    End If
                End If

                If .GetData(argCol, vOthFldNumBlnRangeFromChk) = "Y" And .GetData(argCol, vOthFldNumBlnRangeToChk) = "N" Then
                    If Not fnExpEvaluate(argVsfDet.Editor.Text,
                                    .GetData(argCol, vOthFldNumRangeFromValue),
                                    .GetData(argCol, vOthFldNumRangeFromOperator),
                                     argFrm, "Numeric", argRow) Then
                        If InStr(1, .GetData(argCol, vOthFldNumRangeFromValue), "#SVTPROD") >= 1 Then
                            MsgBox("Entered number not " & Trim$(.GetData(argCol, vOthFldNumRangeFromOperator)) _
                                & " " & Trim$(Replace(Replace(.GetData(argCol, vOthFldNumRangeFromValue), "#", ""), "SVTPROD", "")), vbInformation, argFrm.text)
                        Else
                            MsgBox("Entered number not " & .GetData(argCol, vOthFldNumRangeFromOperator) _
                                & " " & .GetData(argCol, vOthFldNumRangeFromValue), vbInformation, argFrm.text)
                        End If
                        Exit Function
                    End If
                ElseIf .GetData(argCol, vOthFldNumBlnRangeFromChk) = "N" And .GetData(argCol, vOthFldNumBlnRangeToChk) = "Y" Then
                    If Not fnExpEvaluate(argVsfDet.Editor.Text,
                                    .GetData(argCol, vOthFldNumRangeToValue),
                                    .GetData(argCol, vOthFldNumRangeToOperator),
                                     argFrm, "Numeric", argRow) Then
                        If InStr(1, .GetData(argCol, vOthFldNumRangeFromValue), "#SVTPROD") >= 1 Then
                            MsgBox("Entered number not " & Trim$(.GetData(argCol, vOthFldNumRangeFromOperator)) _
                                & Trim$(Replace(Replace(.GetData(argCol, vOthFldNumRangeToValue), "#", ""), "SVTPROD", "")), vbInformation, argFrm.text)
                        Else
                            MsgBox("Entered number not " & .GetData(argCol, vOthFldNumRangeFromOperator) _
                                & " " & .GetData(argCol, vOthFldNumRangeToValue), vbInformation, argFrm.text)
                        End If

                        Exit Function
                    End If
                ElseIf .GetData(argCol, vOthFldNumBlnRangeFromChk) = "Y" And .GetData(argCol, vOthFldNumBlnRangeToChk) = "Y" Then
                    blnValid = True
                    If Not fnExpEvaluate(argVsfDet.Editor.Text,
                                    .GetData(argCol, vOthFldNumRangeFromValue),
                                    .GetData(argCol, vOthFldNumRangeFromOperator),
                                     argFrm, "Numeric", argRow) Then
                        blnValid = False
                    End If
                    If Not fnExpEvaluate(argVsfDet.Editor.Text,
                                    .GetData(argCol, vOthFldNumRangeToValue),
                                    .GetData(argCol, vOthFldNumRangeToOperator),
                                     argFrm, "Numeric", argRow) Then
                        blnValid = False
                    End If
                    If Not blnValid Then
                        If InStr(1, .GetData(argCol, vOthFldNumRangeFromValue), "#SVTPROD") >= 1 Then
                            MsgBox("Entered number should be in range " &
                               Trim$(Replace(Replace(.GetData(argCol, vOthFldNumRangeFromValue), "#", ""), "SVTPROD", "")) & " and " &
                                Trim$(Replace(Replace(.GetData(argCol, vOthFldNumRangeToValue), "#", ""), "SVTPROD", "")), vbInformation, argFrm.text)
                        Else
                            MsgBox("Entered number should be in range " &
                                 .GetData(argCol, vOthFldNumRangeFromValue) & " and " &
                                 Replace(.GetData(argCol, vOthFldNumRangeToValue), "#", ""), vbInformation, argFrm.text)
                        End If
                        Exit Function
                    End If
                End If
                '# A - Commented for now - 12/Feb/2008
                '                If Not ValidateNumber(argFrm.Name, argVsfDet.getData(argRow, argCol), strDetNum, _
                '                    .getData(argCol, vOthFldBlnMandatory), _
                '                    .getData(argCol, vOthFldNumDecimals), _
                '                    .getData(argCol, vOthFldNumBlnAllowNegative), _
                '                    .getData(argCol, vOthFldNumBlnPercentage)) Then
                '                    argVsfDet.getData(argRow, argCol) = strDetNum
                '                End If
            End If

            'check for date range validation
            If .GetData(argCol, vOthFldDataType) = "Date" Then
                If Not dateValidate(argVsfDet.Editor.Text, True) Then
                    MsgBox("Invalid date", MsgBoxStyle.Critical, argFrm.Text)
                    Exit Function
                End If

                'check for financial year range
                If .GetData(argCol, vOthFldDtBlnWithinFinYear) = "Y" Then
                    If .GetData(argCol, vOthFldBlnMandatory) = "Y" Then
                        If Trim(argVsfDet.Editor.Text) = "" Or Trim(argVsfDet.Editor.Text) = "/  /" Then
                            MsgBox("Date should be within Financial Year " & Format(gSettings.gCompStDt, "dd MMM yyyy" & " to " & Format(gSettings.gCompEndDt, "dd MMM yyyy")), vbInformation, argFrm.text)
                            Exit Function
                        End If
                        If CDate(argVsfDet.Editor.Text) <= CDate(gSettings.gCompStDt) Or CDate(argVsfDet.Editor.Text) >= CDate(gSettings.gCompEndDt) Then
                            MsgBox("Date should be within Financial Year " & Format(gSettings.gCompStDt, "dd MMM yyyy" & " to " & Format(gSettings.gCompEndDt, "dd MMM yyyy")), vbInformation, argFrm.text)
                            Exit Function
                        End If
                    Else
                        If Trim(argVsfDet.Editor.Text) <> "" And Trim(argVsfDet.Editor.Text) <> "/  /" Then
                            If CDate(argVsfDet.Editor.Text) <= CDate(gSettings.gCompStDt) Or CDate(argVsfDet.Editor.Text) >= CDate(gSettings.gCompEndDt) Then
                                MsgBox("Date should be within Financial Year " & Format(gSettings.gCompStDt, "dd MMM yyyy") & " to " & Format(gSettings.gCompEndDt, "dd MMM yyyy"), vbInformation, argFrm.text)
                                Exit Function
                            End If
                        End If
                    End If
                End If

                'check for range from, to, and both
                If .GetData(argCol, vOthFldDtBlnRangeFrom) = "Y" And .GetData(argCol, vOthFldDtBlnRangeTo) = "N" Then
                    If Not fnExpEvaluate(argVsfDet.Editor.Text,
                                    .GetData(argCol, vOthFldDtRangeFromValue),
                                    .GetData(argCol, vOthFldDtRangeFromOperator),
                                    strMstOrTrans, argFrm, "Date") Then
                        MsgBox("Entered Date not " & .GetData(argCol, vOthFldDtRangeFromOperator) & " " & .GetData(argCol, vOthFldDtRangeFromValue), vbInformation, argFrm.text)
                        Exit Function
                    End If
                ElseIf .GetData(argCol, vOthFldDtBlnRangeFrom) = "N" And .GetData(argCol, vOthFldDtBlnRangeTo) = "Y" Then
                    If Not fnExpEvaluate(argVsfDet.Editor.Text,
                                    .GetData(argCol, vOthFldDtRangeToValue),
                                    .GetData(argCol, vOthFldDtRangeToOperator),
                                    strMstOrTrans, argFrm, "Date") Then
                        MsgBox("Entered Date not " & .GetData(argCol, vOthFldDtRangeToOperator) & " " & .GetData(argCol, vOthFldDtRangeToValue), vbInformation, argFrm.text)
                        Exit Function
                    End If
                ElseIf .GetData(argCol, vOthFldDtBlnRangeFrom) = "Y" And .GetData(argCol, vOthFldDtBlnRangeTo) = "Y" Then
                    blnValid = True
                    If Not fnExpEvaluate(argVsfDet.Editor.Text,
                                    .GetData(argCol, vOthFldDtRangeFromValue),
                                    .GetData(argCol, vOthFldDtRangeFromOperator),
                                    strMstOrTrans, argFrm, "Date") Then
                        blnValid = False
                    End If
                    If Not fnExpEvaluate(argVsfDet.Editor.Text,
                                    .GetData(argCol, vOthFldDtRangeToValue),
                                    .GetData(argCol, vOthFldDtRangeToOperator),
                                    strMstOrTrans, argFrm, "Date") Then
                        blnValid = False
                    End If
                    If Not blnValid Then
                        MsgBox("Entered Date should be in range " &
                                .GetData(argCol, vOthFldDtRangeFromValue) & " and " &
                                .GetData(argCol, vOthFldDtRangeToValue), vbInformation, argFrm.text)
                        Exit Function
                    End If
                End If

            End If


            'end data pickup feature
            vsfOptValidateEdit = True
        End With
    End Function

    Public Sub vsfOptAfterEditBfrCalTot(ByVal argFrm As Object, ByVal argVsfDet As C1FlexGrid,
                                             ByVal argVsfOptions As C1FlexGrid, ByVal argDetRow As Integer, ByVal argDetCol As Integer)
        Dim clsSvt As New svtLibrary

        Dim intDecPlace As Integer
        With argVsfOptions
            If .GetData(argDetCol, vOthFldDataType) = "Date" Then
                If Trim(argVsfDet.GetData(argDetRow, argDetCol)) = "##/##/####" Or Trim(argVsfDet.GetData(argDetRow, argDetCol)) = "/  /" Then
                    argVsfDet.SetData(argDetRow, argDetCol, "")
                    Exit Sub
                End If
                Try
                    argVsfDet.SetData(argDetRow, argDetCol, CDate(argVsfDet.GetData(argDetRow, argDetCol)).ToString("dd/MM/yyyy"))
                Catch ex As Exception
                    MsgBox("Invalid Date", MsgBoxStyle.Critical, "Error")
                End Try
            End If

            'Code to hide
            Dim j As Integer
            For j = 0 To .Rows.Count - 1
                If .GetData(j, vOthFLdBlnHide) = "Y" Then
                    argVsfDet.Cols(j).Visible = False
                End If
            Next j

            'Call fillDetDefValues(argFrm, strMstOrTrans)

            '# R Bigin
            Dim strDecPlaces As String
            Select Case .GetData(argDetCol, vOthFldDataType)
                Case "Text", "List String"
                    Select Case .GetData(argDetCol, vOthFldTxtCase)
                        Case "LOWERCASE"
                            argVsfDet.SetData(argDetRow, argDetCol, Trim(LCase(argVsfDet.GetData(argDetRow, argDetCol))))
                        Case "SENTENCECASE"
                            argVsfDet.SetData(argDetRow, argDetCol, Trim(ConVrtToSentence(LCase(argVsfDet.GetData(argDetRow, argDetCol)))))
                        Case "TITLECASE"
                            argVsfDet.SetData(argDetRow, argDetCol, Trim(StrConv(argVsfDet.GetData(argDetRow, argDetCol), vbProperCase)))
                        Case "UPPERCASE"
                            argVsfDet.SetData(argDetRow, argDetCol, Trim(UCase(argVsfDet.GetData(argDetRow, argDetCol))))
                        Case "ASENTERED"
                            argVsfDet.SetData(argDetRow, argDetCol, Trim(argVsfDet.GetData(argDetRow, argDetCol)))
                    End Select

                Case "Numeric", "List Numeric"

                    If Trim(.GetData(argDetCol, vOthFldBlnComboData)) = "N" And Trim(.GetData(argDetCol, vOthFldBlnLoadFrom)) = "N" Then
                        If Val(.GetData(argDetCol, vOthFldNumDecimals)) > 0 Then
                            intDecPlace = Val(.GetData(argDetCol, vOthFldNumDecimals))
                            strDecPlaces = "#." & Strings.StrDup(intDecPlace, "0")
                        Else
                            strDecPlaces = "#"
                        End If
                        If Len(argVsfDet.GetData(argDetRow, argDetCol)) <= 14 Then argVsfDet.SetData(argDetRow, argDetCol, SVTROUND(Val(argVsfDet.GetData(argDetRow, argDetCol)), Val(.GetData(argDetCol, vOthFldNumDecimals))))
                        If Len(argVsfDet.GetData(argDetRow, argDetCol)) <= 14 Then argVsfDet.SetData(argDetRow, argDetCol, Format(Val(argVsfDet.GetData(argDetRow, argDetCol)), strDecPlaces))
                    End If
                    'argVsfDet.Cell(flexcpFontName, argDetRow, argDetCol) = argFrm.VsfOptions.getdata(argDetCol, vOthFldFontName)
                    'argVsfDet.Cell(flexcpFontSize, argDetRow, argDetCol) = argFrm.VsfOptions.getdata(argDetCol, vOthFldFontSize)
            End Select
        End With

    End Sub

    Public Sub AddnlFldCalTotal(ByRef argFrm As Object, ByVal argVsfOptions As C1FlexGrid, ByVal argVsfDet As C1FlexGrid,
                                ByVal argDetCols As Integer, ByVal argCboTxt As String, ByVal argCboTag As String, Optional ByVal argMode As String = "", Optional argDetRowNo As Long = 0)
        Dim i As Integer, strF As String, blnCalculate As Boolean
        Dim x As Integer, z As Integer, clsSvt As New svtLibrary, intDeciNos As Integer
        If argMode = "V" Then Exit Sub
        If Not argVsfOptions Is Nothing Then
            With argVsfOptions
                For i = 1 To .Rows.Count - 1
                    If Trim(.GetData(i, vOthBlnTotalReq)) = "Y" Then
                        Dim dblColSum As Double = GETCOLTOT(argVsfDet, i)
                        argFrm.vsfTop.setdata(0, i, IIf(dblColSum = 0, "", dblColSum))
                    End If
                Next i
                If argDetRowNo = 0 Then
                    For x = 1 To argVsfDet.Rows.Count - 1
                        For i = 0 To .Rows.Count - 1
                            strF = UCase(Trim(.GetData(i, vOthFldFormula)))
                            'LoadFormula:
                            If strF <> "" Then
                                blnCalculate = True
                                If Trim(.GetData(i, vOthFldBlnReadOnly)) = "N" Then
                                    Select Case .GetData(i, vOthFldDataType)
                                        Case "Numeric"
                                            If Trim(.GetData(i, vOthFldBlnMandatory)) = "Y" Then
                                                If Val(argVsfDet.GetData(x, i)) = 0 Then
                                                    blnCalculate = True
                                                Else
                                                    blnCalculate = False
                                                End If
                                            Else
                                                If Val(argVsfDet.GetData(x, i)) > 0 Then
                                                    blnCalculate = True
                                                    'ElseIf Trim(.GetData(i, vOthBlnLoadColFrom)) = "Y" Then  'satish 14feb2018
                                                    '    blnCalculate = True
                                                Else
                                                    blnCalculate = False
                                                End If
                                            End If

                                        Case Else
                                            If Trim(argVsfDet.GetData(x, i)) = "" Then
                                                blnCalculate = True
                                                'ElseIf Trim(.GetData(i, vOthBlnLoadColFrom)) = "Y" Then  'satish 14feb2018
                                                '    blnCalculate = True
                                            Else
                                                blnCalculate = False
                                            End If

                                    End Select
                                End If
                                If Mid(strF, 1, 9) = "SVTGETQOH" Then
                                    If Val(argVsfDet.GetData(x, i)) = 0 Then
                                        blnCalculate = True
                                    Else
                                        blnCalculate = False
                                    End If
                                End If
                                If blnCalculate Then
                                    For z = 0 To .Rows.Count - 1
                                        'MsgBox(.GetData(z, vOthFldCode))
                                        Try
                                            If Trim(GetVal(argVsfDet.GetData(x, z), "")).All(AddressOf Char.IsDigit) Then
                                                strF = Replace(strF, "<<" & z + 1 & ">>", Val(Trim(GetVal(argVsfDet.GetData(x, z), ""))))
                                            Else
                                                strF = Replace(strF, "<<" & z + 1 & ">>", Trim(GetVal(argVsfDet.GetData(x, z), "")))
                                            End If
                                        Catch ex As Exception

                                        End Try

                                        'strF = Replace(strF, "<<" & z + 1 & ">>", Val(Trim(GetVal(argVsfDet.GetData(x, z)))))

                                        If InStr(strF, "#SVTMNTHDAYS#") > 0 Then
                                            strF = Replace(strF, "#SVTMNTHDAYS#", argFrm.vsfMain.getdata(6, 1))
                                        End If

                                        If InStr(strF, "#SVTWRKDAYS#") > 0 Then
                                            strF = Replace(strF, "#SVTWRKDAYS#", argFrm.vsfMain.getdata(5, 1))
                                        End If

                                        If InStr(strF, "SVTCOLCUM") > 0 Then
                                            strF = Replace(strF, "@" & .GetData(z, vOthFldCode) & "@", z)
                                            strF = Replace(strF, "!" & .GetData(z, vOthFldCode) & "!", x)
                                        End If

                                        If UCase(.GetData(z, vOthFldDataType)) = "DATE" Then
                                            If IsDate(argVsfDet.GetData(x, z)) Then
                                                strF = Replace(strF, "#" & .GetData(z, vOthFldCode) & "#", Format(CDate(argVsfDet.GetData(x, z)), "dd/MM/yyyy"))
                                            Else
                                                strF = Replace(strF, "#" & .GetData(z, vOthFldCode) & "#", "01/01/2010")
                                            End If
                                        ElseIf UCase(.GetData(z, vOthFldDataType)) = "NUMERIC" Then
                                            strF = Replace(strF, "#" & UCase(.GetData(z, vOthFldCode)) & "#", Format(Val(argVsfDet.GetData(x, z)), "0." + Strings.StrDup(.GetData(z, vOthFldNumDecimals), "0")))
                                        Else
                                            strF = Replace(strF, "#" & UCase(.GetData(z, vOthFldCode)) & "#", GetVal(argVsfDet.GetData(x, z)))
                                        End If
                                    Next z

                                    If Trim(argCboTxt) <> "" Then
                                        argVsfDet.SetData(x, i, fnParse(argFrm, argVsfDet, argVsfOptions, strF, .GetData(i, vOthFldCaption), Trim(argCboTxt), argFrm.txtNo.Text,
                                                                        argFrm.txtdocno.Text, Trim(argCboTxt), argFrm.dtDocDate.value))
                                    Else
                                        argVsfDet.SetData(x, i, fnParse(argFrm, argVsfDet, argVsfOptions, strF, .GetData(i, vOthFldCaption), Trim(argCboTxt),
                                                                        argFrm.txtNo.Text, argFrm.txtdocno.Text, "", argFrm.dtDocDate.value))
                                    End If
                                End If
                                'satish 14.0Feb2018
                                'ElseIf strF = "" Then
                                '    If Trim(.GetData(i, vOthBlnLoadColFrom)) = "Y" Then
                                '        strF = Trim(.GetData(i, vOthFldLoadColFrom))
                                '        GoTo LoadFormula
                                '    End If

                            End If
                        Next i
                    Next x
                Else
                    x = argDetRowNo
                    For i = 0 To .Rows.Count - 1
                        strF = UCase(Trim(.GetData(i, vOthFldFormula)))
                        'LoadFormula:
                        If strF <> "" Then
                            blnCalculate = True
                            If Trim(.GetData(i, vOthFldBlnReadOnly)) = "N" Then
                                Select Case .GetData(i, vOthFldDataType)
                                    Case "Numeric"
                                        If Trim(.GetData(i, vOthFldBlnMandatory)) = "Y" Then
                                            If Val(argVsfDet.GetData(x, i)) = 0 Then
                                                blnCalculate = True
                                            Else
                                                blnCalculate = False
                                            End If
                                        Else
                                            If Val(argVsfDet.GetData(x, i)) > 0 Then
                                                blnCalculate = True
                                                'ElseIf Trim(.GetData(i, vOthBlnLoadColFrom)) = "Y" Then  'satish 14feb2018
                                                '    blnCalculate = True
                                            Else
                                                blnCalculate = False
                                            End If
                                        End If

                                    Case Else
                                        If Trim(argVsfDet.GetData(x, i)) = "" Then
                                            blnCalculate = True
                                            'ElseIf Trim(.GetData(i, vOthBlnLoadColFrom)) = "Y" Then  'satish 14feb2018
                                            '    blnCalculate = True
                                        Else
                                            blnCalculate = False
                                        End If

                                End Select
                            End If
                            If Mid(strF, 1, 9) = "SVTGETQOH" Then
                                If Val(argVsfDet.GetData(x, i)) = 0 Then
                                    blnCalculate = True
                                Else
                                    blnCalculate = False
                                End If
                            End If
                            If blnCalculate Then
                                For z = 0 To .Rows.Count - 1
                                    'MsgBox(.GetData(z, vOthFldCode))
                                    Try
                                        If Trim(GetVal(argVsfDet.GetData(x, z), "")).All(AddressOf Char.IsDigit) Then
                                            strF = Replace(strF, "<<" & z + 1 & ">>", Val(Trim(GetVal(argVsfDet.GetData(x, z), ""))))
                                        Else
                                            strF = Replace(strF, "<<" & z + 1 & ">>", Trim(GetVal(argVsfDet.GetData(x, z), "")))
                                        End If
                                    Catch ex As Exception

                                    End Try

                                    'strF = Replace(strF, "<<" & z + 1 & ">>", Val(Trim(GetVal(argVsfDet.GetData(x, z)))))

                                    If InStr(strF, "#SVTMNTHDAYS#") > 0 Then
                                        strF = Replace(strF, "#SVTMNTHDAYS#", argFrm.vsfMain.getdata(6, 1))
                                    End If

                                    If InStr(strF, "#SVTWRKDAYS#") > 0 Then
                                        strF = Replace(strF, "#SVTWRKDAYS#", argFrm.vsfMain.getdata(5, 1))
                                    End If

                                    If InStr(strF, "SVTCOLCUM") > 0 Then
                                        strF = Replace(strF, "@" & .GetData(z, vOthFldCode) & "@", z)
                                        strF = Replace(strF, "!" & .GetData(z, vOthFldCode) & "!", x)
                                    End If

                                    If UCase(.GetData(z, vOthFldDataType)) = "DATE" Then
                                        If IsDate(argVsfDet.GetData(x, z)) Then
                                            strF = Replace(strF, "#" & .GetData(z, vOthFldCode) & "#", Format(CDate(argVsfDet.GetData(x, z)), "dd/MM/yyyy"))
                                        Else
                                            strF = Replace(strF, "#" & .GetData(z, vOthFldCode) & "#", "01/01/2010")
                                        End If
                                    ElseIf UCase(.GetData(z, vOthFldDataType)) = "NUMERIC" Then
                                        strF = Replace(strF, "#" & UCase(.GetData(z, vOthFldCode)) & "#", Format(Val(argVsfDet.GetData(x, z)), "0." + Strings.StrDup(.GetData(z, vOthFldNumDecimals), "0")))
                                    Else
                                        strF = Replace(strF, "#" & UCase(.GetData(z, vOthFldCode)) & "#", GetVal(argVsfDet.GetData(x, z)))
                                    End If
                                Next z

                                If Trim(argCboTxt) <> "" Then
                                    argVsfDet.SetData(x, i, fnParse(argFrm, argVsfDet, argVsfOptions, strF, .GetData(i, vOthFldCaption), Trim(argCboTxt), argFrm.txtNo.Text,
                                                                    argFrm.txtdocno.Text, Trim(argCboTxt), argFrm.dtDocDate.value))
                                Else
                                    argVsfDet.SetData(x, i, fnParse(argFrm, argVsfDet, argVsfOptions, strF, .GetData(i, vOthFldCaption), Trim(argCboTxt),
                                                                    argFrm.txtNo.Text, argFrm.txtdocno.Text, "", argFrm.dtDocDate.value))
                                End If
                            End If
                            'satish 14.0Feb2018
                            'ElseIf strF = "" Then
                            '    If Trim(.GetData(i, vOthBlnLoadColFrom)) = "Y" Then
                            '        strF = Trim(.GetData(i, vOthFldLoadColFrom))
                            '        GoTo LoadFormula
                            '    End If

                        End If
                    Next i
                End If
            End With
        End If

        Dim ctrlList = GetAllDsgrVsf(argFrm, GetType(C1.Win.C1FlexGrid.C1FlexGrid))

        For Each argVsf As C1FlexGrid In ctrlList
            With argVsf
                For i = 0 To .Rows.Count - 1
                    strF = Trim(.GetData(i, vOthFldFormula))
                    If Trim(.GetData(i, vOthFldBlnNewTrnCalc)) = "Y" Then
                        If argMode <> "A" Then GoTo FldBlnNewTrnCalc
                    End If
                    If strF <> "" Then
                        blnCalculate = True
                        If Trim(.GetData(i, vOthFldBlnReadOnly)) = "N" Then
                            If Trim(.GetData(i, vOthFldDataType)) <> "Numeric" Then
                                If Trim(.GetData(i, vOthFldDataValue)) = "" Then
                                    blnCalculate = True
                                Else
                                    blnCalculate = False
                                End If
                            Else
                                If Val(.GetData(i, vOthFldDataValue)) = 0 Then
                                    blnCalculate = True
                                Else
                                    blnCalculate = False
                                End If
                            End If
                        End If
                        If .GetData(i, vOthPerAmount) = "Y" Then
                            blnCalculate = True
                        End If

                        If blnCalculate Then
                            If Trim(argCboTxt) <> "" Then
                                .SetData(i, vOthFldDataValue, fnParse(argFrm, argVsfDet,
                                                                      argVsfOptions, strF, .GetData(i, vOthFldCaption),
                                                                      argFrm.cboPfx.Text, argFrm.txtNo.Text, argFrm.txtdocno.Text, Trim(argCboTxt),
                                                                      argFrm.dtDocDate.value))

                                If Not InStr(strF, "SVTPARTYADDNLDATA") > 0 Then
                                    If Trim(.GetData(i, vOthFldDataType)) = "Numeric" Then
                                        Select Case Trim(.GetData(i, vOthRoundType))
                                            Case "None"
                                            Case "2 Decimals"
                                                .SetData(i, vOthFldDataValue, SVTROUND(Val(.GetData(i, vOthFldDataValue)), 2))
                                            Case "Nearest 50"
                                                .SetData(i, vOthFldDataValue, SVTROUND(Val(.GetData(i, vOthFldDataValue)), 0))
                                            Case "Next Integer"
                                                If Math.Floor(Val(.GetData(i, vOthFldDataValue))) = Math.Ceiling(Val(.GetData(i, vOthFldDataValue))) Then
                                                    .SetData(i, vOthFldDataValue, CInt(Val(.GetData(i, vOthFldDataValue)) + 1))
                                                Else
                                                    .SetData(i, vOthFldDataValue, CInt(Val(.GetData(i, vOthFldDataValue))))
                                                End If
                                            Case "Ignore Decimal"
                                                .SetData(i, vOthFldDataValue, CInt(Val(.GetData(i, vOthFldDataValue))))
                                        End Select

                                        intDeciNos = Val(.GetData(i, vOthFldNumDecimals))
                                        .SetData(i, vOthFldDataValue, Format(Val(.GetData(i, vOthFldDataValue)), "0." & Microsoft.VisualBasic.Strings.StrDup(intDeciNos, "0")))
                                    End If
                                End If
                            Else
                                .SetData(i, vOthFldDataValue, fnParse(argFrm, argVsfDet, argVsfOptions, strF, .GetData(i, vOthFldCaption), argFrm.cboPfx.Text, argFrm.txtNo.Text, argFrm.txtdocno.Text, "", argFrm.dtDocDate.value))
                                If Trim(.GetData(i, vOthFldDataType)) = "Numeric" Then
                                    Select Case Trim(.GetData(i, vOthRoundType))
                                        Case "None"
                                        Case "2 Decimals"
                                            .SetData(i, vOthFldDataValue, SVTROUND(Val(.GetData(i, vOthFldDataValue)), 2))
                                        Case "Nearest 50"
                                            .SetData(i, vOthFldDataValue, SVTROUND(Val(.GetData(i, vOthFldDataValue)), 0))
                                        Case "Next Integer"
                                            If Math.Floor(Val(.GetData(i, vOthFldDataValue))) = Math.Ceiling(Val(.GetData(i, vOthFldDataValue))) Then
                                                .SetData(i, vOthFldDataValue, CInt(Val(.GetData(i, vOthFldDataValue)) + 1))
                                            Else
                                                .SetData(i, vOthFldDataValue, CInt(Val(.GetData(i, vOthFldDataValue))))
                                            End If
                                        Case "Ignore Decimal"
                                            .SetData(i, vOthFldDataValue, CInt(Val(.GetData(i, vOthFldDataValue))))
                                    End Select

                                    intDeciNos = Val(.GetData(i, vOthFldNumDecimals))
                                    .SetData(i, vOthFldDataValue, Format(Val(.GetData(i, vOthFldDataValue)), "0." & Microsoft.VisualBasic.Strings.StrDup(intDeciNos, "0")))
                                End If
                            End If
                        End If
                    End If
FldBlnNewTrnCalc:
                Next i
            End With
        Next

        'Call vsfGenHideMndt(argFrm, "T")
        clsSvt = Nothing
    End Sub

    Public Sub AddnlFldCalTotal(ByRef argFrm As Object, ByVal argCboTxt As String, ByVal argCboTag As String, Optional ByVal argMode As String = "")
        Dim i As Integer, strF As String, blnCalculate As Boolean
        Dim clsSvt As New svtLibrary, intDeciNos As Integer
        If argMode = "V" Then Exit Sub

        Dim ctrlList = GetAllDsgrVsf(argFrm, GetType(C1.Win.C1FlexGrid.C1FlexGrid))

        For Each argVsf As C1FlexGrid In ctrlList
            With argVsf
                For i = 0 To .Rows.Count - 1
                    strF = Trim(.GetData(i, vOthFldFormula))
                    If Trim(.GetData(i, vOthFldBlnNewTrnCalc)) = "Y" Then
                        If argMode <> "A" Then GoTo FldBlnNewTrnCalc
                    End If
                    If strF <> "" Then
                        blnCalculate = True
                        If Trim(.GetData(i, vOthFldBlnReadOnly)) = "N" Then
                            If Trim(.GetData(i, vOthFldDataType)) <> "Numeric" Then
                                If Trim(.GetData(i, vOthFldDataValue)) = "" Then
                                    blnCalculate = True
                                Else
                                    blnCalculate = False
                                End If
                            Else
                                If Val(.GetData(i, vOthFldDataValue)) = 0 Then
                                    blnCalculate = True
                                Else
                                    blnCalculate = False
                                End If
                            End If
                        End If
                        If .GetData(i, vOthPerAmount) = "Y" Then
                            blnCalculate = True
                        End If

                        Dim objVSF As Object = Nothing
                        Dim objVSFOpt As Object = Nothing
                        If blnCalculate Then
                            If Trim(argCboTxt) <> "" Then
                                .SetData(i, vOthFldDataValue, fnParse(argFrm, objVSF,
                                                                      objVSFOpt, strF, .GetData(i, vOthFldCaption),
                                                                      argFrm.cboPfx.Text, argFrm.txtNo.Text, argFrm.txtdocno.Text, Trim(argCboTxt),
                                                                      argFrm.dtDocDate.value))

                                If Not InStr(strF, "SVTPARTYADDNLDATA") > 0 Then
                                    If Trim(.GetData(i, vOthFldDataType)) = "Numeric" Then
                                        intDeciNos = Val(.GetData(i, vOthFldNumDecimals))
                                        .SetData(i, vOthFldDataValue, Format(Val(.GetData(i, vOthFldDataValue)), "0." & Microsoft.VisualBasic.Strings.StrDup(intDeciNos, "0")))
                                    End If
                                End If
                            Else
                                .SetData(i, vOthFldDataValue, fnParse(argFrm, objVSF, objVSF, strF, .GetData(i, vOthFldCaption), argFrm.cboPfx.Text, argFrm.txtNo.Text, argFrm.txtdocno.Text, "", argFrm.dtDocDate.value))
                                If Trim(.GetData(i, vOthFldDataType)) = "Numeric" Then
                                    intDeciNos = Val(.GetData(i, vOthFldNumDecimals))
                                    .SetData(i, vOthFldDataValue, Format(Val(.GetData(i, vOthFldDataValue)), "0." & Microsoft.VisualBasic.Strings.StrDup(intDeciNos, "0")))
                                End If
                            End If
                        End If
                    End If
FldBlnNewTrnCalc:
                Next i
            End With
        Next

        'Call vsfGenHideMndt(argFrm, "T")
        clsSvt = Nothing
    End Sub

    Public Function getColNoFromAddnlDetGrid(ByVal argVsfOpt As C1FlexGrid, ByVal argVSF As C1FlexGrid, ByVal argFldCode As String) As Integer
        Dim i As Integer
        getColNoFromAddnlDetGrid = -1
        With argVsfOpt
            For i = 1 To .Rows.Count - 1
                Dim strMsg As String = Trim(UCase(.GetData(i, 1)))
                If Trim(UCase(argFldCode)) = Trim(UCase(.GetData(i, 1))) Then
                    getColNoFromAddnlDetGrid = i
                    Exit Function
                End If
            Next i
        End With
    End Function

    Private Function SVTGETGRIDDATA(ByVal argStr As String, ByVal argvsfdet As C1FlexGrid, ByVal argVsfOpt As C1FlexGrid) As String
        'On Error GoTo errHandler
        'example
        'SVTGETGRIDDATA(L, #VATPER#)
        Dim arr() As String
        Dim strSumCol As String, strRowType As String
        Dim intSumCol As Integer, introw As Integer
        arr = Split(argStr, ",")
        SVTGETGRIDDATA = ""
        Try
            strRowType = UCase(Replace(arr(0), "SVTGETGRIDDATA(", ""))
            strSumCol = Replace(arr(1), ")", "")
            strSumCol = Replace(strSumCol, "#", "")

            intSumCol = getColNoFromAddnlDetGrid(argVsfOpt, argvsfdet, strSumCol)
            If strRowType = "F" Then
                introw = 1
            Else
                introw = argvsfdet.Rows.Count - 1
            End If
            If Not (introw > 0 And introw <= argvsfdet.Rows.Count - 1) Then
                SVTGETGRIDDATA = ""
                Exit Function
            End If

            SVTGETGRIDDATA = argvsfdet.GetData(introw, intSumCol)

            Exit Function
        Catch ex As Exception
        End Try
        'errHandler:
        '        MsgBox(Err.Description, vbInformation, "Formula Error")
    End Function

    Private Function GETCOLAVG(ByVal vsf As C1FlexGrid, ByVal strColNo As String) As Double
        Dim i As Integer, sngTot As Double, sngCount As Integer, sngColValue As Double, intColNo As Integer
        GETCOLAVG = 0
        intColNo = Val(strColNo)
        If Not IsNumeric(strColNo) Then Exit Function
        If strColNo = "" Then Exit Function
        If intColNo - 1 < 0 Then Exit Function
        With vsf
            sngTot = 0
            sngCount = 0
            For i = 1 To .Rows.Count - 1
                sngColValue = Val(.GetData(i, intColNo))
                sngCount = sngCount + 1
                sngTot = sngTot + Val(.GetData(i, intColNo - 1))
            Next i
        End With
        GETCOLAVG = SVTROUND(sngTot / sngCount, 4)
    End Function

    Public Function GETCOLTOT(ByVal vsf As C1FlexGrid, ByVal strColNo As String) As Double
        Dim i As Integer, sngTot As Double, intColNo As Integer
        GETCOLTOT = 0
        intColNo = Val(strColNo)
        If Not IsNumeric(strColNo) Then Exit Function
        If strColNo = "" Then Exit Function
        If Val(strColNo) - 1 < 0 Then Exit Function
        With vsf
            sngTot = 0
            For i = 1 To .Rows.Count - 1
                sngTot = sngTot + Val(.GetData(i, intColNo))
            Next i
        End With
        GETCOLTOT = sngTot
    End Function

    Public Function GETCOLCUMULATIVE(ByVal vsfDet As C1FlexGrid, ByVal argVal As String) As String
        Dim i As Integer, intColNo As Integer
        Dim strCol As String = "", strVal As String = "", strRowNo As String = 0
        GETCOLCUMULATIVE = 0
        i = 0
        For Each strCol In argVal.Split(",")
            If i = 0 Then
                strVal = strCol
                i = i + 1
                GoTo NextLoop
            End If
            If i = 1 Then
                strRowNo = strCol
                i = i + 1
                GoTo NextLoop
            End If
NextLoop:
        Next
        i = 0
        intColNo = Val(strCol)
        If Not IsNumeric(intColNo) Then Exit Function
        If intColNo < 0 Then Exit Function
        If strVal = "" Then Exit Function
        With vsfDet
            If Val(strRowNo) > 1 Then
                strVal = strVal & " + " & .GetData(CInt(strRowNo) - 1, intColNo)
            Else
                For Each strVal In argVal.Split("+")
                    Exit For
                Next
            End If
        End With
        GETCOLCUMULATIVE = strVal
    End Function

    Public Function GETCOLMAX(ByVal vsf As C1FlexGrid, ByVal strColNo As String) As Double
        Dim i As Integer, sngMax As Double, intColNo As Integer
        GETCOLMAX = 0
        intColNo = Val(strColNo)
        If Not IsNumeric(strColNo) Then Exit Function
        If strColNo = "" Then Exit Function
        If Val(strColNo) - 1 < 0 Then Exit Function
        With vsf
            sngMax = 0
            For i = 1 To .Rows.Count - 1
                If Val(.GetData(i, intColNo - 1)) > sngMax Then
                    sngMax = Val(.GetData(i, intColNo - 1))
                End If
            Next i
        End With
        GETCOLMAX = sngMax
    End Function

    Public Function validOthDetFLds(ByVal argFrm As Object, ByVal argVsfDet As C1FlexGrid, ByVal argVsfOptions As C1FlexGrid,
                                    ByVal argDetCols As Integer) As Boolean
        Dim i As Integer, blnFlag As Boolean, x As Integer
        Dim strFirstDispCol As String

        validOthDetFLds = True
        With argVsfOptions
            For x = 1 To argVsfDet.Rows.Count - 1
                For i = argDetCols To .Rows.Count - 1
                    If .GetData(i, vOthFldDataType) = "Date" Then
                        If Trim(argVsfDet.GetData(x, vOthFldDataValue)) = "/  /" Then
                            .SetData(x, i, "")
                        End If
                    End If

                    '# Code for mandatory field
                    If .GetData(i, vOthFldBlnMandatory) = "Y" Then
                        If fnExpEvaluate(.GetData(i, vOthFldMndtryCompareVal),
                                                    .GetData(i, vOthFldMndtryWithVal),
                                                    .GetData(i, vOthFldMndtryOperator),
                                                     argFrm, "", x) Then
                            If Trim(argVsfDet.GetData(x, i)) = "" Then
                                MsgBox("Field " & .GetData(i, vOthFldCaption) & " cannot be blank ", vbInformation, argFrm.text)
                                argVsfDet.Row = x
                                argVsfDet.Col = i
                                argVsfDet.TopRow = x
                                argVsfDet.Focus()
                                GoTo EndFunc
                            End If

                            If .GetData(i, vOthFldDataType) = "Numeric" And .GetData(i, vOthFldBlnComboData) = "N" Then
                                If Val(argVsfDet.GetData(x, i)) = 0 Then

                                    MsgBox("Field " & .GetData(i, vOthFldCaption) & " cannot be blank ", vbInformation, argFrm.text)
                                    argVsfDet.Row = x
                                    argVsfDet.Col = i
                                    argVsfDet.TopRow = x
                                    argVsfDet.Focus()
                                    GoTo EndFunc
                                End If
                            End If

                        End If
                    End If

                    '# Code for mandatory field

                    If .GetData(i, vOthFldBlnComboData) = "Y" Then
                        If Trim(argVsfDet.GetData(x, i)) <> "" Then
                            If InStr(1, .GetData(i, vOthFldCmbDisplayCols), ",") > 0 Then
                                strFirstDispCol = Left(.GetData(i, vOthFldCmbDisplayCols) _
                                                    , InStr(1, .GetData(i, vOthFldCmbDisplayCols), ",") - 1)
                            Else
                                strFirstDispCol = .GetData(i, vOthFldCmbDisplayCols)
                            End If
                            Call clsSvtLib.getDataLongValue("Select " & .GetData(i, vOthFldCmbStorageFldName) _
                                & " From " & .GetData(i, vOthFldCmbTblName) & " where " & strFirstDispCol &
                                " = '" & argVsfDet.GetData(x, i) & "'", 0, blnFlag)
                            If Not blnFlag Then
                                MsgBox(.GetData(i, vOthFldCaption) & "[" & argVsfDet.GetData(x, i) & "] ...  Not found in " & .GetData(i, vOthFldCmbTblName) & " !!", MsgBoxStyle.Information, argFrm.text)
                                argVsfDet.Row = x
                                argVsfDet.TopRow = x
                                argVsfDet.Col = i
                                argVsfDet.Focus()
                                GoTo EndFunc
                            End If
                        End If
                    End If
                Next i
            Next x
        End With
        Exit Function
EndFunc:
        validOthDetFLds = False
    End Function

    Public Sub stringReplace(ByVal argFrm As Object)

        Dim ctrlList As New List(Of Control)
        Dim ctrl As New Control

        Do
            ctrl = argFrm.GetNextControl(ctrl, True)

            If ctrl Is Nothing Then
                Exit Do
            End If
            If ctrl.GetType = GetType(TextBox) Or ctrl.GetType = GetType(ComboBox) Then
                ctrl.Text = Replace(ctrl.Text, "'", "`")
            End If
        Loop Until ctrl Is Nothing
    End Sub

    Public Sub GetvsfOptDetInsertString(ByVal argFrm As Object, ByVal argVsfDet As C1FlexGrid,
                                        ByVal argVsfOptions As C1FlexGrid, ByVal argDetCols As Integer, ByVal argRow As Integer,
                                        ByRef strFlds As String, ByRef strValues As String, Optional ByVal argSVTRN As OleDb.OleDbTransaction = Nothing)
        Dim iColNo As Integer, strFirstDispCol As String
        Dim strFldValue As String, clsSvt As New svtLibrary
        strFldValue = ""

        With argVsfDet
            For iColNo = argDetCols To .Cols.Count - 1
                If (argVsfOptions.GetData(iColNo, vOthFldBlnDispOnly)) <> "Y" Then
                    strFlds = strFlds & Trim(argVsfOptions.GetData(iColNo, vOthFldCode)) & ","
                    strFldValue = Trim(GetVal(.GetData(argRow, iColNo), ""))

                    If Trim(argVsfOptions.GetData(iColNo, vOthFldCmbTblName)) <> "" Then
                        If InStr(1, argVsfOptions.GetData(iColNo, vOthFldCmbDisplayCols), ",") > 0 Then
                            strFirstDispCol = Left(argVsfOptions.GetData(iColNo, vOthFldCmbDisplayCols) _
                                                , InStr(1, argVsfOptions.GetData(iColNo, vOthFldCmbDisplayCols), ",") - 1)
                        Else
                            strFirstDispCol = argVsfOptions.GetData(iColNo, vOthFldCmbDisplayCols)
                        End If
                        strFldValue = clsSvt.getDataStringValue("Select " & argVsfOptions.GetData(iColNo, vOthFldCmbStorageFldName) _
                                                                & " from " & argVsfOptions.GetData(iColNo, vOthFldCmbTblName) _
                                                                & " where " & strFirstDispCol & " = '" & strFldValue & "'", "", False, argSVTRN)
                    End If
                    'If Trim(argVsfOptions.GetData(iColNo, vOthFldBlnLoadFrom)) = "Y" Then
                    '    'if ledname is shown and ledname is to be stored [using load from option]
                    '    ' till here
                    '    If argVsfOptions.GetData(iColNo, vOthFldLoadFromType) = "I" Then
                    '        If UCase(Mid(Trim(argVsfOptions.GetData(iColNo, vOthFldLoadFldName)), 1, 1)) = "I" Then
                    '            'find if it is addnlfld, and if it is loaded from combo
                    '            'example i.LedID
                    '            strFldBlnComboData = clsSvt.GetDataValue("FldBlnComboData", "N", _
                    '            "UtlDocDsgrFlds", False, "where DocID = 2 and FldCode = '" & _
                    '            UCase(Mid(Trim(argVsfOptions.GetData(iColNo, vOthFldLoadFldName)), 3)) & "'")
                    '            If strFldBlnComboData = "Y" Then
                    '                strFldCmbTblName = clsSvt.GetDataValue("FldCmbTblName", "N", _
                    '                "UtlDocDsgrFlds", False, "where DocID = 2 and FldCode = '" & _
                    '                UCase(Mid(Trim(argVsfOptions.GetData(iColNo, vOthFldLoadFldName)), 3)) & "'")
                    '                strFldCmbStorageFldName = clsSvt.GetDataValue("FldCmbStorageFldName", "N", _
                    '                "UtlDocDsgrFlds", False, "where DocID = 2 and FldCode = '" & _
                    '                UCase(Mid(Trim(argVsfOptions.GetData(iColNo, vOthFldLoadFldName)), 3)) & "'")

                    '                strFldCmbDisplayCols = clsSvt.GetDataValue("FldCmbDisplayCols", "N", _
                    '                "UtlDocDsgrFlds", False, "where DocID = 2 and FldCode = '" & _
                    '                UCase(Mid(Trim(argVsfOptions.GetData(iColNo, vOthFldLoadFldName)), 3)) & "'")

                    '                strFldCmbAddnlCond = clsSvt.GetDataValue("FldCmbAddnlCond", "N", _
                    '                "UtlDocDsgrFlds", False, "where DocID = 2 and FldCode = '" & _
                    '                UCase(Mid(Trim(argVsfOptions.GetData(iColNo, vOthFldLoadFldName)), 3)) & "'")

                    '                If strFldCmbTblName <> "" Then
                    '                    If InStr(1, strFldCmbDisplayCols, ",") > 0 Then
                    '                        strFirstDispCol = Left(strFldCmbDisplayCols, InStr(1, strFldCmbDisplayCols, ",") - 1)
                    '                    Else
                    '                        strFirstDispCol = strFldCmbDisplayCols
                    '                    End If
                    '                    strFldValue = clsSvt.GetDataValue(strFldCmbStorageFldName, _
                    '                        "", strFldCmbTblName, False, " where " & strFirstDispCol & " = '" & _
                    '                        strFldValue & "'")
                    '                End If
                    '            End If
                    '        End If
                    '    End If
                    'End If

                    Select Case argVsfOptions.GetData(iColNo, vOthFldDataType)
                        Case "Text", "List String"
                            If strFldValue = "" Then strFldValue = " "
                            strValues = strValues & "'" & strFldValue & "'"
                        Case "Date"
                            If strFldValue <> "" Then
                                strValues = strValues & "'" & getDateFormatInInsert(strFldValue) & "'"
                            Else
                                strValues = strValues & "Null"
                            End If
                        Case Else
                            strValues = strValues & Val(strFldValue) & ""
                    End Select
                    strValues = strValues & ","
                End If
            Next iColNo
        End With

        strFlds = Mid(strFlds, 1, Len(strFlds) - 1)
        strFlds = strFlds & ") values ("
        strValues = Mid(strValues, 1, Len(strValues) - 1) & ")"

        clsSvt = Nothing
    End Sub

    Public Sub GetvsfOptDetInsertStringFldCode(ByVal argFrm As Object, ByVal argVsfDet As C1FlexGrid,
                                        ByVal argVsfOptions As C1FlexGrid, ByVal argDetCols As Integer, ByVal argRow As Integer,
                                        ByRef strFlds As String)
        Dim iColNo As Integer
        Dim strFldValue As String, clsSvt As New svtLibrary
        strFldValue = ""

        With argVsfDet
            For iColNo = argDetCols To .Cols.Count - 1
                If (argVsfOptions.GetData(iColNo, vOthFldBlnDispOnly)) <> "Y" Then
                    strFlds = strFlds & Trim(argVsfOptions.GetData(iColNo, vOthFldCode)) & ","
                End If
            Next
        End With

        strFlds = Mid(strFlds, 1, Len(strFlds) - 1)
        strFlds = strFlds & ")"

        clsSvt = Nothing
    End Sub

    Public Sub GetvsfOptHdrDetInsertString(ByVal argFrm As Object, ByVal argVsfDet As C1FlexGrid,
                                            ByVal argVsfOptions As C1FlexGrid, ByVal argDetCols As Integer, ByVal argRow As Integer,
                                            ByRef strFlds As String, ByRef strValues As String, Optional ByVal argSVTRN As OleDb.OleDbTransaction = Nothing)
        Dim iColNo As Integer, strFirstDispCol As String
        Dim strFldValue As String, clsSvt As New svtLibrary
        strFldValue = ""

        With argVsfDet
            For iColNo = argDetCols To .Cols.Count - 1

                If (argVsfOptions.GetData(iColNo, vOthFldBlnDispOnly)) <> "Y" Then
                    strFlds = strFlds & Trim(argVsfOptions.GetData(iColNo, vOthFldCode)) & ","
                    strFldValue = Trim(GetVal(.GetData(argRow, iColNo), ""))

                    If Trim(argVsfOptions.GetData(iColNo, vOthFldCmbTblName)) <> "" Then
                        If InStr(1, argVsfOptions.GetData(iColNo, vOthFldCmbDisplayCols), ",") > 0 Then
                            strFirstDispCol = Left(argVsfOptions.GetData(iColNo, vOthFldCmbDisplayCols) _
                                                , InStr(1, argVsfOptions.GetData(iColNo, vOthFldCmbDisplayCols), ",") - 1)
                        Else
                            strFirstDispCol = argVsfOptions.GetData(iColNo, vOthFldCmbDisplayCols)
                        End If
                        strFldValue = clsSvt.getDataStringValue("Select " & argVsfOptions.GetData(iColNo, vOthFldCmbStorageFldName) _
                                                                & " from " & argVsfOptions.GetData(iColNo, vOthFldCmbTblName) _
                                                                & " where " & strFirstDispCol & " = '" & strFldValue & "'", "", False, argSVTRN)
                    End If
                    'If Trim(argVsfOptions.GetData(iColNo, vOthFldBlnLoadFrom)) = "Y" Then
                    '    'if ledname is shown and ledname is to be stored [using load from option]
                    '    ' till here
                    '    If argVsfOptions.GetData(iColNo, vOthFldLoadFromType) = "I" Then
                    '        If UCase(Mid(Trim(argVsfOptions.GetData(iColNo, vOthFldLoadFldName)), 1, 1)) = "I" Then
                    '            'find if it is addnlfld, and if it is loaded from combo
                    '            'example i.LedID
                    '            strFldBlnComboData = clsSvt.GetDataValue("FldBlnComboData", "N", _
                    '            "UtlDocDsgrFlds", False, "where DocID = 2 and FldCode = '" & _
                    '            UCase(Mid(Trim(argVsfOptions.GetData(iColNo, vOthFldLoadFldName)), 3)) & "'")
                    '            If strFldBlnComboData = "Y" Then
                    '                strFldCmbTblName = clsSvt.GetDataValue("FldCmbTblName", "N", _
                    '                "UtlDocDsgrFlds", False, "where DocID = 2 and FldCode = '" & _
                    '                UCase(Mid(Trim(argVsfOptions.GetData(iColNo, vOthFldLoadFldName)), 3)) & "'")
                    '                strFldCmbStorageFldName = clsSvt.GetDataValue("FldCmbStorageFldName", "N", _
                    '                "UtlDocDsgrFlds", False, "where DocID = 2 and FldCode = '" & _
                    '                UCase(Mid(Trim(argVsfOptions.GetData(iColNo, vOthFldLoadFldName)), 3)) & "'")

                    '                strFldCmbDisplayCols = clsSvt.GetDataValue("FldCmbDisplayCols", "N", _
                    '                "UtlDocDsgrFlds", False, "where DocID = 2 and FldCode = '" & _
                    '                UCase(Mid(Trim(argVsfOptions.GetData(iColNo, vOthFldLoadFldName)), 3)) & "'")

                    '                strFldCmbAddnlCond = clsSvt.GetDataValue("FldCmbAddnlCond", "N", _
                    '                "UtlDocDsgrFlds", False, "where DocID = 2 and FldCode = '" & _
                    '                UCase(Mid(Trim(argVsfOptions.GetData(iColNo, vOthFldLoadFldName)), 3)) & "'")

                    '                If strFldCmbTblName <> "" Then
                    '                    If InStr(1, strFldCmbDisplayCols, ",") > 0 Then
                    '                        strFirstDispCol = Left(strFldCmbDisplayCols, InStr(1, strFldCmbDisplayCols, ",") - 1)
                    '                    Else
                    '                        strFirstDispCol = strFldCmbDisplayCols
                    '                    End If
                    '                    strFldValue = clsSvt.GetDataValue(strFldCmbStorageFldName, _
                    '                        "", strFldCmbTblName, False, " where " & strFirstDispCol & " = '" & _
                    '                        strFldValue & "'")
                    '                End If
                    '            End If
                    '        End If
                    '    End If
                    'End If

                    Select Case argVsfOptions.GetData(iColNo, vOthFldDataType)
                        Case "Text", "List String"
                            If strFldValue = "" Then strFldValue = " "
                            strValues = strValues & "'" & strFldValue & "'"
                        Case "Date"
                            If strFldValue <> "" Then
                                strValues = strValues & "'" & getDateFormatInInsert(strFldValue) & "'"
                            Else
                                strValues = strValues & "Null"
                            End If
                        Case Else
                            strValues = strValues & Val(strFldValue) & ""
                    End Select
                    strValues = strValues & ","
                End If
            Next iColNo
        End With
        If strFlds <> "" Then strFlds = Mid(strFlds, 1, Len(strFlds) - 1)
        strFlds = strFlds & ")"
        'strFlds = strFlds & ") values ("
        If strValues <> "" Then strValues = Mid(strValues, 1, Len(strValues) - 1) & ")"
        clsSvt = Nothing
    End Sub

    Public Sub GetvsfOptDetInsertStringFldValue(ByVal argFrm As Object, ByVal argVsfDet As C1FlexGrid,
                                        ByVal argVsfOptions As C1FlexGrid, ByVal argDetCols As Integer, ByVal argRow As Integer,
                                        ByRef strValues As String)
        Dim iColNo As Integer, strFirstDispCol As String
        Dim strFldValue As String, clsSvt As New svtLibrary
        strFldValue = ""

        With argVsfDet

            For iColNo = argDetCols To .Cols.Count - 1
                If (argVsfOptions.GetData(iColNo, vOthFldBlnDispOnly)) <> "Y" Then
                    'strFlds = strFlds & Trim(argVsfOptions.GetData(iColNo, vOthFldCode)) & ","
                    strFldValue = Trim(GetVal(.GetData(argRow, iColNo), ""))

                    If Trim(argVsfOptions.GetData(iColNo, vOthFldCmbTblName)) <> "" Then
                        If InStr(1, argVsfOptions.GetData(iColNo, vOthFldCmbDisplayCols), ",") > 0 Then
                            strFirstDispCol = Left(argVsfOptions.GetData(iColNo, vOthFldCmbDisplayCols) _
                                                , InStr(1, argVsfOptions.GetData(iColNo, vOthFldCmbDisplayCols), ",") - 1)
                        Else
                            strFirstDispCol = argVsfOptions.GetData(iColNo, vOthFldCmbDisplayCols)
                        End If
                        strFldValue = clsSvt.getDataStringValue("Select " & argVsfOptions.GetData(iColNo, vOthFldCmbStorageFldName) _
                                                                & " from " & argVsfOptions.GetData(iColNo, vOthFldCmbTblName) _
                                                                & " where " & strFirstDispCol & " = '" & strFldValue & "'", "", False)
                    End If

                    Select Case argVsfOptions.GetData(iColNo, vOthFldDataType)
                        Case "Text", "List String"
                            If strFldValue = "" Then strFldValue = " "
                            strValues = strValues & "'" & strFldValue & "'"
                        Case "Date"
                            If strFldValue <> "" Then
                                strValues = strValues & "'" & getDateFormatInInsert(strFldValue) & "'"
                            Else
                                strValues = strValues & "Null"
                            End If
                        Case Else
                            strValues = strValues & Val(strFldValue) & ""
                    End Select
                    strValues = strValues & ","
                End If
            Next iColNo
        End With

        strValues = Mid(strValues, 1, Len(strValues) - 1)

        clsSvt = Nothing
    End Sub

    Public Sub FillvsfOptDtlFlds(ByVal argFrm As Object, ByVal argVsfDet As C1FlexGrid, ByVal argVsfOptions As C1FlexGrid,
                                 ByVal argRS As OleDbDataReader, ByVal argRow As Integer, ByVal argDetCols As Integer)
        Dim intAddCol As Integer, x As Integer
        Dim clsSvt As New svtLibrary, strFirstDispCol As String, strColName As String, strGridColName As String

        With argRS
            For intAddCol = argDetCols To argVsfOptions.Rows.Count - 1
                For x = 0 To .FieldCount - 1

                    strColName = UCase$(.GetName(x))
                    strGridColName = argVsfOptions.GetData(intAddCol, vOthFldCode)

                    If UCase$(.GetName(x)) = UCase$(argVsfOptions.GetData(intAddCol, vOthFldCode)) Then
                        If Trim(argVsfOptions.GetData(intAddCol, vOthFldBlnComboData)) = "N" Then
                            'argVsfDet.SetData(argRow, intAddCol, GetVal(.GetValue(x), "", argVsfOptions.GetData(intAddCol, vOthFldDataType)))
                            argVsfDet.SetData(argRow, intAddCol, GetVal(.GetValue(x), ""))
                        Else
                            If InStr(1, argVsfOptions.GetData(intAddCol, vOthFldCmbDisplayCols), ",") > 0 Then
                                strFirstDispCol = Left(argVsfOptions.GetData(intAddCol, vOthFldCmbDisplayCols) _
                                                    , InStr(1, argVsfOptions.GetData(intAddCol, vOthFldCmbDisplayCols), ",") - 1)
                            Else
                                strFirstDispCol = argVsfOptions.GetData(intAddCol, vOthFldCmbDisplayCols)
                            End If
                            argVsfDet.SetData(argRow, intAddCol,
                                clsSvt.getDataStringValue("Select " & strFirstDispCol & " from " & argVsfOptions.GetData(intAddCol, vOthFldCmbTblName) _
                                & " where " &
                                argVsfOptions.GetData(intAddCol, vOthFldCmbStorageFldName) &
                                " = " & GetVal(.GetValue(x), 0), "", False))
                        End If
                        Exit For
                    End If
                Next x
            Next
        End With
    End Sub

    Public Function getRsVal(ByVal strsql As String) As String
        Dim rsDummy As OleDb.OleDbDataReader
        getRsVal = ""
        If strsql = "" Then Exit Function
        Try
            rsDummy = clsSvtLib.getDataReaderFromSQL(strsql)

            If Not rsDummy.HasRows Then
                rsDummy.Close()
                Exit Function
            End If
            '--------------------
            With rsDummy
                If .HasRows Then
                    .Read()
                    getRsVal = GetVal(.GetValue(0), "")
                End If
                .Close()
            End With
        Catch ex As Exception
            MsgBox(ex.Message & " : " & vbCrLf & strsql, MsgBoxStyle.Critical)
        End Try
    End Function

    Public Function fnParse(ByVal argFrm As Object,
        ByVal vsfDet As Object, ByVal vsfOpt As Object,
        ByVal strFormula As String, ByVal strFldName As String,
        ByVal strDocPfx As String, ByVal strNo As String, ByVal strDocNo As String,
        Optional ByVal argCustSuppCode As String = "", Optional ByVal argDocDate As String = "",
        Optional argVsfHdr As Object = Nothing, Optional argVsfTot As Object = Nothing, Optional argVsfFtr As Object = Nothing,
        Optional argEmpID As Integer = 0) As String

        'On Error GoTo errHandler
        Dim i As Integer, blnFormula As Boolean, intColNo As Integer
        Dim strColNo As String

        fnParse = ""
        Try
            'in this place put all the rectifications that may
            'me done wrongly in Designer
            'one ex is given below

            '1
            '    MsgBox strFormula
            strFormula = Replace(strFormula, "SVTCOLSUM (", "SVTCOLSUM(")
            strFormula = Replace(strFormula, "SVTCOLAVG (", "SVTCOLAVG(")
            strFormula = Replace(strFormula, "SVTCOLSUMGROUP (", "SVTCOLSUMGROUP(")

            '2.
            strFormula = Replace(strFormula, "SVTAMTINWORDS (", "SVTAMTINWORDS( ")

            Dim ctrlList = GetAllDsgrVsf(argFrm, GetType(C1.Win.C1FlexGrid.C1FlexGrid))

            'J		
            'SVTCTC(#TDS#)		

            If InStr(strFormula, "SVTCTC") > 0 Then
                fnParse = GETCTC(strFormula, argVsfHdr, argVsfFtr, argVsfTot, argDocDate, argEmpID)
                Exit Function
            End If

            '# A - #SVTCUSTCODE#
            If InStr(strFormula, "#SVTCUSTCODE#") > 0 Then
                fnParse = Replace(strFormula, "#SVTCUSTCODE#", argCustSuppCode)
                Exit Function
            End If

            '# A - #SVTSUPPCODE#
            If InStr(strFormula, "#SVTSUPPCODE#") > 0 Then
                fnParse = Replace(strFormula, "#SVTSUPPCODE#", argCustSuppCode)
                Exit Function
            End If

            '# A - #SVTDOCDATE#
            If InStr(strFormula, "#SVTDOCDATE#") > 0 Then
                fnParse = Replace(strFormula, "#SVTDOCDATE#", argDocDate)
                Exit Function
            End If

            If InStr(strFormula, "#SVTMNTHDAYS#") > 0 Then
                strFormula = Replace(strFormula, "#SVTMNTHDAYS#", argFrm.vsfMain.getdata(6, 1))
            End If

            If InStr(strFormula, "#SVTWRKDAYS#") > 0 Then
                strFormula = Replace(strFormula, "#SVTWRKDAYS#", argFrm.vsfMain.getdata(5, 1))
            End If


            For Each argVsf As C1FlexGrid In ctrlList
                If Not argVsf Is Nothing Then
                    With argVsf
                        For i = 0 To .Rows.Count - 1
                            Select Case Trim(.GetData(i, vOthFldDataType))
                                Case "Numeric", "List Numeric"
                                    If .GetData(i, vOthFldBlnComboData) = "Y" Then
                                        strFormula = Replace(strFormula, "#" & .GetData(i, vOthFldCode) & "#", GetVal(.GetData(i, vOthFldDataValue), "0"))
                                    Else
                                        strFormula = Replace(strFormula, "#" & .GetData(i, vOthFldCode) & "#", Format(Val(.GetData(i, vOthFldDataValue)), "0." & Strings.StrDup(.GetData(i, vOthFldNumDecimals), "0")))
                                    End If
                                Case Else
                                    strFormula = Replace(strFormula, "#" & .GetData(i, vOthFldCode) & "#", GetVal(.GetData(i, vOthFldDataValue), ""))
                            End Select
                        Next i
                    End With
                End If
            Next

            'If Not argVsfFtr Is Nothing Then
            '    With argVsfFtr
            '        For i = 0 To .Rows.Count - 1
            '            Select Case Trim(.getdata(i, vOthFldDataType))
            '                Case "Numeric", "List Numeric"
            '                    If .getdata(i, vOthFldBlnComboData) = "Y" Then
            '                        strFormula = Replace(strFormula, "#" & .getdata(i, vOthFldCode) & "#", GetVal(.getdata(i, vOthFldDataValue), "0"))
            '                    Else
            '                        strFormula = Replace(strFormula, "#" & .getdata(i, vOthFldCode) & "#", Format(Val(.getdata(i, vOthFldDataValue)), "0.0000"))
            '                    End If
            '                Case Else
            '                    strFormula = Replace(strFormula, "#" & .getdata(i, vOthFldCode) & "#", GetVal(.getdata(i, vOthFldDataValue), "0"))
            '            End Select
            '        Next i
            '    End With
            'End If

            'If Not argVsfHdr Is Nothing Then
            '    With argVsfHdr
            '        For i = 0 To .Rows.Count - 1
            '            Select Case Trim(.getdata(i, vOthFldDataType))
            '                Case "Numeric", "List Numeric"
            '                    If .getdata(i, vOthFldBlnComboData) = "Y" Then
            '                        strFormula = Replace(strFormula, "#" & .getdata(i, vOthFldCode) & "#", .getdata(i, vOthFldDataValue))
            '                    Else
            '                        strFormula = Replace(strFormula, "#" & .getdata(i, vOthFldCode) & "#", Format(Val(.getdata(i, vOthFldDataValue)), "0.0000"))
            '                    End If
            '                Case Else
            '                    strFormula = Replace(strFormula, "#" & .getdata(i, vOthFldCode) & "#", Trim(.getdata(i, vOthFldDataValue)))
            '            End Select
            '        Next i
            '    End With
            'End If


            blnFormula = True
            '-----		


            For Each argVsf As C1FlexGrid In ctrlList
                With argVsf
                    If .Visible Then
                        For i = 0 To .Rows.Count - 1
                            Select Case Trim(.GetData(i, vOthFldDataType))
                                Case "Numeric", "List Numeric"
                                    If .GetData(i, vOthFldBlnComboData) = "Y" Then
                                        strFormula = Replace(strFormula, "#" & .GetData(i, vOthFldCode) & "#", .GetData(i, vOthFldDataValue))
                                    Else
                                        strFormula = Replace(strFormula, "#" & .GetData(i, vOthFldCode) & "#", Format(Val(.GetData(i, vOthFldDataValue)), "0." & Strings.StrDup(.GetData(i, vOthFldNumDecimals), "0")))
                                    End If
                                Case Else
                                    strFormula = Replace(strFormula, "#" & .GetData(i, vOthFldCode) & "#", Trim(.GetData(i, vOthFldDataValue)))
                            End Select
                            If .GetData(i, vOthPerAmount) = "Y" Then
                                strFormula = Replace(strFormula, "#" & .GetData(i, vOthFldCode) & "PER#", Format(Val(.GetData(i, vOthFldDataPer)), "0.00"))
                                strFormula = Replace(strFormula, "#" & .GetData(i, vOthFldCode) & "AMT#", Format(Val(.GetData(i, vOthFldDataValue)), "0." & Strings.StrDup(.GetData(i, vOthFldNumDecimals), "0")))
                            End If
                        Next i
                    End If
                End With
            Next

            blnFormula = True

            'SVTCOLSUMGROUP(#AVATMNT#, #VATPER#, "=",12)
            If InStr(strFormula, "SVTCOLSUMGROUP") > 0 Then
                fnParse = GETGRPCOLTOT(strFormula, vsfDet, vsfOpt)
                Exit Function
            End If


            If InStr(strFormula, "SVTDATEDIFF") > 0 Then
                fnParse = GETDateDiff(strFormula)
                Exit Function
            End If
            If InStr(strFormula, "SVTDATEADD") > 0 Then
                fnParse = GETDateAdded(strFormula)
                Exit Function
            End If

            If InStr(strFormula, "SVTGETGRIDDATA") > 0 Then
                fnParse = SVTGETGRIDDATA(strFormula, vsfDet, vsfOpt)
                Exit Function
            End If

            If InStr(strFormula, "SVTPARTYADDNLDATA") > 0 Then
                Dim strTable As String = "", strLinkCol As String = "", strCol As String = "", strParty As String = "", strCode As String = ""
                Dim intCol As Int32 = 0
                strFormula = strFormula.ToUpper().Replace("SVTPARTYADDNLDATA(", "")
                strFormula = strFormula.Replace(")", "")
                For Each strVal As String In strFormula.Split(",")
                    Select Case intCol
                        Case 0
                            strParty = strVal
                        Case 1
                            strTable = strVal
                        Case 2
                            strLinkCol = strVal
                        Case 3
                            strCol = strVal
                    End Select
                    intCol = intCol + 1
                Next
                strCode = argCustSuppCode
                fnParse = SVTPartyAddnlData(strTable, strLinkCol, strCol, strParty, strCode)
                Exit Function
            End If

            If InStr(strFormula, "SVTPARTYDATA") > 0 Then
                Dim strCol As String = "", strParty As String = "", strCode As String = ""
                Dim intCol As Int32 = 0
                strFormula = strFormula.ToUpper().Replace("SVTPARTYDATA(", "")
                strFormula = strFormula.Replace(")", "")
                For Each strVal As String In strFormula.Split(",")
                    Select Case intCol
                        Case 0
                            strParty = strVal
                        Case 1
                            strCol = strVal
                    End Select
                    intCol = intCol + 1
                Next
                strCode = argCustSuppCode
                fnParse = SVTPartyData(strCol, strParty, strCode)
                Exit Function
            End If

            If InStr(strFormula, "SVTUSERCODE") > 0 Then
                fnParse = gUserCode
                Exit Function
            End If

            'SVTCALCTAXAMT(#BASEAMTFLD#, #TAXPERFLDNAME#, 10)
            If InStr(strFormula, "SVTCALCTAXAMT") > 0 Then
                fnParse = Gettaxamt(strFormula)
                Exit Function
            End If

            If InStr(strFormula, "SVTNUMBER") > 0 Then
                strFormula = Replace(strFormula, "SVTNUMBER", strNo)
                blnFormula = False
            End If

            If InStr(strFormula, "SVTUSERID") > 0 Then
                fnParse = gUserID
                Exit Function
            End If

            If InStr(strFormula, "SVTSYSNAME") > 0 Then
                fnParse = gStrSystemName
                Exit Function
            End If

            If InStr(strFormula, "SVTSYSUSER") > 0 Then
                fnParse = gStrSystemUserName
                Exit Function
            End If

            If InStr(strFormula, "SVTDOCNO") > 0 Then
                strFormula = Replace(strFormula, "SVTDOCNO", strDocNo)
                blnFormula = False
            End If

            If InStr(strFormula, "SVTFYF") > 0 Then
                strFormula = Replace(strFormula, "SVTFYF", Right(gSettings.gCompStDt, 2))
                blnFormula = False
            End If

            If InStr(strFormula, "SVTFYT") > 0 Then
                strFormula = Replace(strFormula, "SVTFYT", Right(gSettings.gCompEndDt, 2))
                blnFormula = False
            End If

            If InStr(strFormula, "#SVTPRODAMOUNT#") > 0 Then
                intColNo = getRowNoFromAddnlGrid(vsfOpt, "SVTPRODAMOUNT") + 1
                strFormula = Replace(strFormula, "#SVTPRODAMOUNT#", intColNo)
                blnFormula = True
            End If

            If InStr(strFormula, "#SVTPRODQTY#") > 0 Then
                intColNo = getRowNoFromAddnlGrid(vsfOpt, "SVTPRODQTY") + 1
                strFormula = Replace(strFormula, "#SVTPRODQTY#", intColNo)
                blnFormula = True
            End If

            If InStr(strFormula, "#SVTPRODRATE#") > 0 Then
                intColNo = getRowNoFromAddnlGrid(vsfOpt, "SVTPRODRATE") + 1
                strFormula = Replace(strFormula, "#SVTPRODRATE#", intColNo)
                blnFormula = True
            End If
            If InStr(strFormula, "SVTSRCDOCQTY") > 0 Then
                intColNo = getRowNoFromAddnlGrid(vsfOpt, "SVTSRCDOCQTY") + 1
                strFormula = Replace(strFormula, "#SVTSRCDOCQTY#", intColNo)
                blnFormula = True
            End If

            If InStr(strFormula, "SVTCURDATE") > 0 Then
                strFormula = Replace(strFormula, "SVTCURDATE", Format(getSystemDate, "dd/MM/yyyy"))
                blnFormula = False
            End If

            If InStr(strFormula, "SVTCURTIME") > 0 Then
                strFormula = Replace(strFormula, "SVTCURTIME", Format(getSystemDateTime, "hh:mm:ss"))
                blnFormula = False
            End If

            If InStr(strFormula, "SVTPENDQTY") > 0 Then
                intColNo = getRowNoFromAddnlGrid(vsfOpt, "SVTPENDQTY") + 1
                strFormula = Replace(strFormula, "#SVTPENDQTY#", intColNo)
                blnFormula = True
            End If

            If InStr(strFormula, "SVTPREFIX") > 0 Then
                strFormula = Replace(strFormula, "SVTPREFIX", strDocPfx)
                blnFormula = False
            End If

            If InStr(strFormula, "SVTUSERCODE") > 0 Then
                strFormula = Replace(strFormula, "SVTUSERCODE", gUserCode)
                blnFormula = False
            End If

            '# A - #SVTCUSTCODE#
            If InStr(strFormula, "#SVTCUSTCODE#") > 0 Then
                strFormula = Replace(strFormula, "#SVTCUSTCODE#", argCustSuppCode)
                blnFormula = False
            End If

            '# A - #SVTSUPPCODE#
            If InStr(strFormula, "#SVTSUPPCODE#") > 0 Then
                strFormula = Replace(strFormula, "#SVTSUPPCODE#", argCustSuppCode)
                blnFormula = False
            End If

            '# A - #SVTDOCDATE#
            If InStr(strFormula, "#SVTDOCDATE#") > 0 Then
                strFormula = Replace(strFormula, "#SVTDOCDATE#", argDocDate)
                blnFormula = False
            End If

            If InStr(strFormula, "#SVTSTARTMNTHDOCDATE#") > 0 Then
                strFormula = Replace(strFormula, "#SVTSTARTMNTHDOCDATE#", getFirstDateInMonth(CDate(argDocDate)))
                blnFormula = False
            End If

            If InStr(strFormula, "#SVTENDMNTHDOCDATE#") > 0 Then
                strFormula = Replace(strFormula, "#SVTENDMNTHDOCDATE#", getLastDateInMonth(CDate(argDocDate)))
                blnFormula = False
            End If

            '# A - #SVTCREDIT# - gConstCreditCol
            If InStr(strFormula, "#SVTCREDITCOL#") > 0 Then
                strFormula = Replace(strFormula, "#SVTCREDITCOL#", gConstCreditCol)
                blnFormula = False
            End If

            '# A - #SVTDEBIT# - gConstDebitCol
            If InStr(strFormula, "#SVTDEBITCOL#") > 0 Then
                strFormula = Replace(strFormula, "#SVTDEBITCOL#", gConstDebitCol)
                blnFormula = False
            End If

            'SVTCONCATENATE(#FLD1#, #FLD2#,…)
            If InStr(strFormula, "SVTCONCATENATE") > 0 Then
                strFormula = Replace(strFormula, "SVTCONCATENATE", "")
                fnParse = strFormula
                Exit Function
            End If

            If InStr(strFormula, "SVTCOLSUM") > 0 Then
                strColNo = Replace(strFormula, "SVTCOLSUM(", "")
                strColNo = Replace(strColNo, ")", "")
                If InStr(strColNo, "#") > 0 Then
                    strColNo = getColNoFromAddnlDetGrid(vsfOpt, vsfDet, Replace(strColNo, "#", ""))
                End If
                strFormula = Replace(strFormula, ")", "")
                strFormula = GETCOLTOT(vsfDet, strColNo)
                blnFormula = False
            End If

            If InStr(strFormula, "SVTCOLCUM") > 0 Then
                strColNo = Replace(strFormula, "SVTCOLCUM(", "")
                strColNo = Replace(strColNo, ")", "")
                strFormula = Replace(strFormula, ")", "")
                strFormula = GETCOLCUMULATIVE(vsfDet, strColNo)
            End If

            If InStr(strFormula, "SVTCOLMAX") > 0 Then
                strColNo = Replace(strFormula, "SVTCOLMAX(", "")
                strColNo = Replace(strColNo, ")", "")
                If InStr(strColNo, "#") > 0 Then
                    strColNo = getColNoFromAddnlDetGrid(vsfOpt, vsfDet, Replace(strColNo, "#", ""))
                End If
                strFormula = Replace(strFormula, ")", "")
                strFormula = GETCOLMAX(vsfDet, strColNo)
                blnFormula = False
            End If

            If InStr(strFormula, "SVTCOLAVG") > 0 Then
                strColNo = Replace(strFormula, "SVTCOLAVG(", "")
                strColNo = Replace(strColNo, ")", "")
                If InStr(strColNo, "#") > 0 Then
                    strColNo = getColNoFromAddnlDetGrid(vsfOpt, vsfDet, Replace(strColNo, "#", ""))
                End If
                strFormula = Replace(strFormula, ")", "")
                strFormula = GETCOLAVG(vsfDet, strColNo)
                blnFormula = False
            End If

            'svtRoundOff
            If InStr(strFormula, "SVTROUND50") > 0 Then
                strFormula = Replace(strFormula, "SVTROUND50", "")
                strFormula = Regex.Replace(strFormula, "-{2,}", "-")
                fnParse = Format(ROUND50(fnEvaluate(strFormula)), "0.00")
                Exit Function
            End If

            If InStr(strFormula, "SVTROUND2") > 0 Then
                strFormula = Replace(strFormula, "SVTROUND2", "")
                fnParse = Format(ROUND2(fnEvaluate(strFormula)), "0.00")
                Exit Function
            End If

            If InStr(strFormula, "SVTROUND100") > 0 Then
                strFormula = Replace(strFormula, "SVTROUND100", "")
                fnParse = Format(ROUND100(fnEvaluate(strFormula)), "0.00")
                Exit Function
            End If

            '# A - Round off amt
            If InStr(strFormula, "SVTROUNDOFFAMT") > 0 Then
                strFormula = Replace(strFormula, "SVTROUNDOFFAMT(", "")
                strFormula = Replace(strFormula, ")", "")
                fnParse = Format(SVTROUNDOFFAMT(Val(strFormula)), "0.00")
                Exit Function
            End If

            'SVTAMTINWORDS(#NumFld#, #CurNameFld#)
            If InStr(strFormula, "SVTAMTINWORDS") > 0 Then
                Dim strCurCode As String = "", strDecCurName As String = "", strCurName As String = ""
                Dim iPos As Integer
                Dim strInMillion As String
                Dim strCaptionStyle As String
                Dim strAmt As String
                strFormula = Replace(strFormula, "SVTAMTINWORDS(", "")
                strFormula = Replace(strFormula, ")", "")

                iPos = InStr(1, strFormula, ",")
                If iPos <> 0 Then
                    strCurCode = Mid(strFormula, iPos + 1)

                    If InStr(1, strCurCode, ",") > 0 Then
                        strCurCode = Mid(strCurCode, 1, InStr(1, strCurCode, ",") - 1)
                    End If

                    strDecCurName = Trim(clsSvtLib.getDataStringValue("Select IsNull(Curdecimal, '') from MstCur where CurCode ='" & Trim(strCurCode) & "'", "", False))
                    strCurName = Trim(clsSvtLib.getDataStringValue("Select CurName from MstCur where CurCode ='" & Trim(strCurCode) & "'", "", False))
                End If
                strInMillion = Trim(clsSvtLib.getDataStringValue("Select IsNull(WordsInMillions,'N') from MstCur where CurCode ='" & Trim(strCurCode) & "'", "N", False))
                If Trim(strInMillion) = "" Then strInMillion = "N"
                strCaptionStyle = Mid(Right(strFormula, 2), 2, 2)
                strFormula = Replace(strFormula, strCurCode, "")

                If iPos = 0 Or (Trim(strCurCode) = "") Or (Trim(strCurCode) = "-") Then
                    '# A - Get Def Currency name
                    strCurCode = "Rupees"
                    strDecCurName = "Paise"
                End If
                If InStr(1, strFormula, ",") > 0 Then
                    strFormula = Replace(strFormula, ",", "")
                End If

                strAmt = fnAmtInWords(CDbl(Val(strFormula)), strCurCode, strDecCurName, strInMillion)
                Select Case Trim(strCaptionStyle)
                    Case "U"
                        strAmt = UPPERCASE(strAmt)
                    Case "L"
                        strAmt = LOWERCASE(strAmt)
                    Case Else
                        strAmt = TITLECASE(strAmt) 'Default is T - Title Case
                End Select

                fnParse = strAmt

                Exit Function
            End If


            If InStr(strFormula, "SVTGETQOH") > 0 Then
                '    fnParse = GETQOHFORMULA(strFormula, vsfDet, vsfOpt)
                Exit Function
            End If

            If blnFormula Then
                Dim intRowNo As Int32 = 0
                Dim blnRowFound As Boolean
                With vsfOpt
                    For intRowNo = 0 To .Rows.Count - 1
                        If GetVal(.getData(intRowNo, vOthFldCaption), "") = Trim(strFldName) Then
                            blnRowFound = True
                            Exit For
                        End If
                    Next
                End With
                If blnRowFound Then
                    fnParse = Format(clsSvtLib.getDataDoubleValue("Select " & strFormula & " as FldVal ", 0, False), "0." & Strings.StrDup(vsfOpt(intRowNo, vOthFldNumDecimals), "0"))
                Else
                    fnParse = Format(clsSvtLib.getDataDoubleValue("Select " & strFormula & " as FldVal ", 0, False), "0.00")
                End If
            Else
                fnParse = strFormula
            End If
            Exit Function
        Catch ex As Exception
            MsgBox("error")
        End Try
        'errHandler:
        '        Select Case Err.Number
        '            Case 13
        '                fnParse = 0
        '            Case Else
        '                MsgBox(Err.Description & vbCrLf & " Error in Field " & strFldName & vbCrLf & " Formula : " & strFormula, vbInformation, "Parsing Function")
        '        End Select
    End Function

    Private Function fnEvaluate(ByVal argString As String) As String
        fnEvaluate = clsSvtLib.getDataStringValue("Select " & argString, "", False)
    End Function

    Public Function CalColSum(ByVal argfrm As Object, ByVal argvsfdet As C1FlexGrid, ByVal argCol As Integer) As String
        CalColSum = GETCOLTOT(argfrm.argvsfdet, argCol)
    End Function

    Private Function GETGRPCOLTOT(ByVal argStr As String, ByVal argvsfdet As C1FlexGrid, ByVal argVsfOpt As C1FlexGrid) As String
        'On Error GoTo errHandler
        'example
        'SVTCOLSUMGROUP(#VATAMT#, #VATPER#, 12) 
        Dim arr() As String
        Dim strSumCol As String, strBasedOn As String
        Dim intSumCol As Integer, sngTot As Double
        Dim strDataType As String = ""
        Dim strCompareVal As String
        Dim i As Integer, x As Integer
        arr = Split(argStr, ",")
        GETGRPCOLTOT = ""
        Try
            strSumCol = Replace(arr(0), "SVTCOLSUMGROUP(", "")
            strSumCol = Replace(strSumCol, "#", "")
            strBasedOn = Replace(arr(1), "#", "")
            strCompareVal = Replace(arr(2), ")", "")
            intSumCol = getColNoFromAddnlDetGrid(argVsfOpt, argvsfdet, strSumCol)
            sngTot = 0
            With argVsfOpt
                For i = 1 To .Rows.Count - 1
                    If UCase(Trim(.GetData(i, vOthFldCode))) = UCase(Trim(strBasedOn)) Then
                        strDataType = .GetData(i, vOthFldDataType)
                        For x = 1 To argvsfdet.Rows.Count - 1
                            If strDataType = "Numeric" Or strDataType = "List Numeric" Then
                                If Val(strCompareVal) = Val(argvsfdet.GetData(x, i)) Then
                                    sngTot = sngTot + Val(argvsfdet.GetData(x, intSumCol))
                                End If
                            Else
                                If Trim(UCase((strCompareVal))) = Trim(UCase(argvsfdet.GetData(x, i))) Then
                                    sngTot = sngTot + Val(argvsfdet.GetData(x, intSumCol))
                                End If

                            End If
                        Next x
                    End If
                Next i
            End With

            Try
                If arr.Count > 3 Then
                    Dim strAddVal As String
                    If strDataType = "Numeric" Or strDataType = "List Numeric" Then
                        strAddVal = Replace(arr(3), "+", "")
                        sngTot = sngTot + Val(strAddVal)
                    End If
                End If
            Catch ex As Exception
                MsgBox(ex.Message)
            End Try
            GETGRPCOLTOT = CStr(sngTot)
        Catch ex As Exception
        End Try
        '        Exit Function
        'errHandler:
        '        MsgBox(Err.Description, vbInformation, "Formula Error")
    End Function

    Private Function GETDateAdded(ByVal argStr As String) As String
        'On Error GoTo errHandler
        'example
        'SVTCOLSUMGROUP(#VATAMT#, #VATPER#, 12)
        Dim arr() As String
        Dim intInterval As Integer
        Dim strToDate As String
        GETDateAdded = ""
        Try
            arr = Split(argStr, ",")
            strToDate = Replace(arr(0), "SVTDATEADD(", "")
            strToDate = Replace(strToDate, "#", "")
            intInterval = Replace(arr(1), ")", "")
            intInterval = Replace(intInterval, "`", "")
            If IsDate(strToDate) Then
                GETDateAdded = DateAdd("d", intInterval, Format(strToDate, "dd/MM/yyyy"))
            Else
                GETDateAdded = ""
            End If
        Catch ex As Exception
        End Try
        'errHandler:

    End Function

    Public Function getRowNoFromAddnlGrid(ByVal argVSF As C1FlexGrid, ByVal argFldCode As String) As Integer
        Dim i As Integer
        getRowNoFromAddnlGrid = -1
        With argVSF
            For i = 0 To .Rows.Count - 1
                If Trim(UCase(argFldCode)) = Trim(UCase(.GetData(i, 1))) Then
                    getRowNoFromAddnlGrid = i
                    Exit Function
                End If
            Next i
        End With
    End Function

    Public Sub vsfOptionsSetupEditor(ByVal argFrm As Form, ByVal argVsfDet As C1FlexGrid,
                                 ByVal argVsfOptions As C1FlexGrid, ByVal argDocID As Integer, ByVal argHdrTblName As String,
    ByVal argMode As String, ByVal argRow As Integer, ByVal argCol As Integer, ByVal argMstTrns As String)

        Try

            If argMode = "V" Or argMode = "" Then
                Exit Sub
            End If
            With argVsfOptions
                'if it is a combo then exit sub
                If Trim(.GetData(argCol, vOthFldCmbTblName)) <> "" Then Exit Sub
                If Mid(Trim(.GetData(argCol, vOthFldDataType)), 1, 4) = "List" Then Exit Sub
                'set max length

                'Exit Sub

                Dim tb As TextBox = argVsfDet.Editor
                tb.MaxLength = Val(.GetData(argCol, vOthFldDataWidth))
                If .GetData(argCol, vOthFldTxtBlnMultiLine) = "Y" Then
                    tb.Multiline = True

                    tb.CharacterCasing = CharacterCasing.Upper
                End If
            End With

        Catch ex As Exception
        End Try
    End Sub

    Public Sub initGridStyles(ByVal argForm As Form)
        Dim ctrl As Control = Nothing
        Dim argVsfGrid As C1FlexGrid
        Dim str As String = getGsettings("COMPANY", "GridClrs", "")
        If str = "" Then Exit Sub
        Dim strArr() As String = str.Split(New Char() {","c})

        Do
            ctrl = argForm.GetNextControl(ctrl, True)

            If ctrl Is Nothing Then
                Exit Do
            End If
            If ctrl.GetType = GetType(C1.Win.C1FlexGrid.C1FlexGrid) Then
                argVsfGrid = ctrl
                If UCase(argVsfGrid.Name) <> "VSFTOP" And argVsfGrid.Tag <> "NOSTYLE" Then
                    With argVsfGrid
                        ' .Rows.Count = .Rows.Count + 1
                        .AllowDragging = AllowDraggingEnum.None
                        .KeyActionTab = KeyActionEnum.MoveAcross
                        .VisualStyle = VisualStyle.Custom
                        .SelectionMode = SelectionModeEnum.RowRange
                        .Font = New Font("Microsoft Sans Serif", 9)
                        .AutoResize = True

                        'align center, fixed col header
                        If .Cols.Fixed > 1 Then
                            .Styles.Fixed.TextAlign = TextAlignEnum.CenterCenter
                        End If

                        'set Normal Style
                        Dim cs As CellStyle = .Styles.Normal
                        cs.Border.Color = Color.Black
                        cs.ForeColor = Color.FromArgb(Convert.ToInt32(strArr(0)))
                        cs.BackColor = Color.FromArgb(Convert.ToInt32(strArr(1)))

                        'set Alternate style
                        cs = .Styles.Alternate
                        cs.ForeColor = Color.FromArgb(Convert.ToInt32(strArr(2)))
                        cs.BackColor = Color.FromArgb(Convert.ToInt32(strArr(3)))
                        cs.Border.Color = Color.Black

                        'set Fixed style
                        cs = .Styles.Fixed
                        cs.Border.Color = Color.Black
                        cs.Font = New Font("Microsoft Sans Serif", 9)
                        cs.ForeColor = Color.FromArgb(Convert.ToInt32(strArr(4)))
                        cs.BackColor = Color.FromArgb(Convert.ToInt32(strArr(5)))

                        'set highlight style
                        cs = .Styles.Highlight
                        cs.ForeColor = Color.FromArgb(Convert.ToInt32(strArr(6)))
                        cs.BackColor = Color.FromArgb(Convert.ToInt32(strArr(7)))
                        cs.Border.Color = Color.Black

                        'set focus style
                        cs = .Styles.Focus
                        cs.Border.Color = Color.Black
                        cs.ForeColor = Color.FromArgb(Convert.ToInt32(strArr(8)))
                        cs.BackColor = Color.FromArgb(Convert.ToInt32(strArr(9)))

                        'set Empty Area Style
                        'cs = .Styles.Focus
                        '.Styles.EmptyArea.BackColor = Color.FromArgb(Convert.ToInt32(strArr(10)))
                    End With
                End If
            End If
        Loop Until ctrl Is Nothing
    End Sub

    Public Sub initToolTipColours(arglbl, argPnl)
        Dim strlbl As String = getGsettings("ToolTip", "ToolTipClr", "")
        If strlbl = "" Then Exit Sub
        Dim strArr() As String = strlbl.Split(New Char() {","c})

        'btnGroup
        arglbl.Forecolor = Color.FromArgb(Convert.ToInt32(strArr(0)))
        arglbl.backColor = Color.FromArgb(Convert.ToInt32(strArr(1)))
        argPnl.backcolor = Color.FromArgb(Convert.ToInt32(strArr(1)))

    End Sub

    Public Sub vsfGenAfterEditAftrCalTot(ByVal argFrm As Object, ByVal argTrnID As Integer, ByVal argMode As String, ByVal argRow As Long, ByVal argIndex As Integer, ByVal argAllowToChangePfx As Boolean)
        Dim clsSVT As New svtLibrary
        Dim strSQL As String
        If argRow < 0 Then Exit Sub

        If argFrm.vsfGen(argIndex).Visible = False Then Exit Sub
        If argMode = "A" Then
            Dim rsDummy As OleDb.OleDbDataReader

            With argFrm.vsfGen(argIndex)
                strSQL = "Select * from UtlDocNoPrefix where DocID = " & argTrnID
                rsDummy = clsSVT.getDataReaderFromSQL(strSQL)

                If Not rsDummy.HasRows Then
                    rsDummy.Close()
                    Exit Sub
                End If
                '------------------------------------
                With rsDummy
                    Do While .Read
                        If Trim(UCase(!CndFldCode)) = Trim(UCase(argFrm.vsfGen(argIndex).TextMatrix(argRow, vOthFldCode))) Then
                            Call assignPfx(argFrm, Nothing, argMode, argAllowToChangePfx, argIndex, argRow, GetVal(!Prefix, ""),
                                                    GetVal(!CndOperator, ""),
                                                    GetVal(!CndFldvalue, ""))
                        End If
                    Loop
                End With
            End With
            rsDummy.Close()
            '------------------------
        End If
        Call vsfGenHideMndtAllGrids(argFrm)
        clsSVT = Nothing
    End Sub

    Public Function GetAllDsgrVsf(control As Control, type As Type) As IEnumerable(Of Control)
        Dim controls = control.Controls.Cast(Of Control)()
        Return controls.SelectMany(Function(ctrl) GetAllDsgrVsf(ctrl, type)).Concat(controls).Where(Function(c) c.[GetType]() = type And CStr(c.Tag) = "DSGR")
    End Function

    Public Sub AddnlFldCalTotalDetOnly(ByRef argFrm As Object, ByVal argVsfOptions As C1FlexGrid, ByVal argVsfDet As C1FlexGrid,
                            ByVal argDetCols As Integer, ByVal argCboTxt As String, ByVal argCboTag As String)
        Dim i As Integer, strF As String, blnCalculate As Boolean
        Dim x As Integer, z As Integer, clsSvt As New svtLibrary

        With argVsfOptions
            For x = 1 To argVsfDet.Rows.Count - 1
                For i = 0 To .Rows.Count - 1
                    strF = Trim(.GetData(i, vOthFldFormula))
                    If strF <> "" Then
                        blnCalculate = True
                        If Trim(.GetData(i, vOthFldBlnReadOnly)) = "N" Then
                            Select Case .GetData(i, vOthFldDataType)
                                Case "Numeric"
                                    If Trim(.GetData(i, vOthFldBlnMandatory)) = "Y" Then
                                        If Val(argVsfDet.GetData(x, i)) = 0 Then
                                            blnCalculate = True
                                        Else
                                            blnCalculate = False
                                        End If
                                    Else
                                        If Val(argVsfDet.GetData(x, i)) > 0 Then
                                            blnCalculate = True
                                        Else
                                            blnCalculate = False
                                        End If
                                    End If

                                Case Else
                                    If Trim(argVsfDet.GetData(x, i)) = "" Then
                                        blnCalculate = True
                                    Else
                                        blnCalculate = False
                                    End If

                            End Select
                        End If
                        If Mid(strF, 1, 9) = "SVTGETQOH" Then
                            If Val(argVsfDet.GetData(x, i)) = 0 Then
                                blnCalculate = True
                            Else
                                blnCalculate = False
                            End If
                        End If
                        If blnCalculate Then
                            For z = 0 To .Rows.Count - 1
                                strF = Replace(strF, "<<" & z + 1 & ">>", Val(Trim(GetVal(argVsfDet.GetData(x, z)))))
                                If UCase(.GetData(z, vOthFldDataType)) = "DATE" Then
                                    strF = Replace(strF, "#" & .GetData(z, vOthFldCode) & "#", Format(argVsfDet.GetData(x, z), "dd/MM/yyyy"))
                                ElseIf UCase(.GetData(z, vOthFldDataType)) = "NUMERIC" Then
                                    strF = Replace(strF, "#" & .GetData(z, vOthFldCode) & "#", Format(Val(argVsfDet.GetData(x, z)), "0.00"))
                                Else
                                    strF = Replace(strF, "#" & .GetData(z, vOthFldCode) & "#", GetVal(argVsfDet.GetData(x, z)))
                                End If
                            Next z
                            If Trim(argCboTxt) <> "" Then
                                argVsfDet.SetData(x, i, fnParse(argFrm, argVsfDet, argVsfOptions, strF, .GetData(i, vOthFldCaption), argFrm.cboPfx.Text, argFrm.txtNo.Text,
                                                                argFrm.txtdocno.Text, argFrm.txtcustcode.Text, argFrm.dtDocDate.value))
                            Else
                                argVsfDet.SetData(x, i, fnParse(argFrm, argVsfDet, argVsfOptions, strF, .GetData(i, vOthFldCaption), argFrm.cboPfx.Text,
                                                                argFrm.txtNo.Text, argFrm.txtdocno.Text, "", argFrm.dtDocDate.value))
                            End If
                        End If
                    End If
                Next i
            Next x
        End With

        clsSvt = Nothing
    End Sub

    Private Function GETCTC(argFormula As String, argVsfHdr As C1FlexGrid, argVsfFtr As C1FlexGrid, argVsfTot As C1FlexGrid, argDocDate As String, argEmpID As String) As String
        'example
        'SVTCTC(#CTCTDS#)
        Dim strSumCol As String
        Dim intSumCol As Integer, sngTot As Double
        Dim strCond As String

        Try
            strSumCol = Replace(argFormula, "SVTCTC", "")
            strSumCol = Replace(strSumCol, "#", "")
            strSumCol = Replace(strSumCol, ")", "")
            strSumCol = Trim(Replace(strSumCol, "(", ""))


            intSumCol = getRowNoFromAddnlGrid(argVsfTot, strSumCol)
            If intSumCol > 0 Then
                sngTot = Val(argVsfTot.GetData(intSumCol, vOthFldDataValue))
            Else
                intSumCol = getRowNoFromAddnlGrid(argVsfFtr, strSumCol)
                If intSumCol > 0 Then
                    sngTot = Val(argVsfFtr.GetData(intSumCol, vOthFldDataValue))
                Else
                    intSumCol = getRowNoFromAddnlGrid(argVsfHdr, strSumCol)
                    If intSumCol > 0 Then
                        sngTot = Val(argVsfHdr.GetData(intSumCol, vOthFldDataValue))
                    End If
                End If
            End If

            strCond = " where MnthDate >= datevalue('" & Format(gSettings.gCompStDt, "dd/MM/yyyy") & "')"
            strCond = strCond & " and MnthDate < datevalue('" & Format(argDocDate, "dd/MM/yyyy") & "')"
            strCond = strCond & " and EmpID = " & Val(argEmpID)
            sngTot = sngTot + clsSvtLib.getDataDoubleValue("Select Sum(" & strSumCol & ") from TrnHdrPaySlip " & strCond, 0, False)

            GETCTC = CStr(sngTot)
        Catch ex As Exception
            GETCTC = 0
            MsgBox(ex.Message, vbInformation, "Formula Error: " & argFormula)
        End Try
    End Function

    Public Sub AddnlFldAppotionateCatTotal(ByRef argFrm As Object, argDetCols As Integer, argCboTxt As String, argCboTag As String, argDocID As Integer, Optional ByVal argChkCol As Int32 = 0)
        Dim i As Integer
        ', strF As String, strFQry As String, blnCalculate As Boolean
        Dim x As Integer, clsSvt As New svtLibrary
        ', z As Integer, strComp As String,
        Dim dblResult As Double, dblFldAppVal As Double, dblFldBasedOnValCol As Integer
        Dim blnFound As Boolean, strFormula As String
        Dim dblFldBasedOnVal As Double
        clsSvt.getDataStringValue("select BlnApportinate from UtlDocDsgrFlds where BlnApportinate = 'Y' and DocID = " & argDocID, "N", blnFound)
        If Not blnFound Then Exit Sub
        With argFrm.VsfOptions
            For x = 1 To argFrm.vsfDet.Rows.count - 1 'Detial Grid
                If Trim(argFrm.vsfDet.getdata(x, argChkCol)) <> "" Then
                    For i = argDetCols To .Rows.count - 1
                        If Trim(.getdata(i, vOthFldBlnApportinate)) = "Y" Then
                            If x <> argFrm.vsfDet.Rows.count - 1 Then
                                strFormula = .getdata(i, vOthFLdApportinate)
                                dblFldAppVal = fnParse(argFrm, argFrm.vsfDet, argFrm.VsfOptions, .getdata(i, vOthFLdApportinate), .getdata(i, vOthFldCaption), argFrm.cboPfx.Text, argFrm.txtNo.Text, argFrm.txtDocNo.Text, argFrm.svtPCtrl.getCode(), argFrm.dtDocDate.Value, argFrm.vsfHdr, argFrm.vsfTot, argFrm.vsfFtr)

                                'dblFldBasedOnValCol = fnParse(argFrm, argFrm.vsfDet, argFrm.VsfOptions, .Getdata(i, vOthFldBasedOn), .Getdata(i, vOthFldCaption), argFrm.cboPfx.Text, argFrm.txtNo.Text, argFrm.txtDocNo.Text, argFrm.svtPCtrl.getCode(), argFrm.dtDocDate.Value, argFrm.vsfhdr, argFrm.vsftot, argFrm.vsfftr)
                                dblFldBasedOnValCol = getColNoFromAddnlDetGrid(argFrm.vsfoptions, argFrm.vsfdet, Replace(.getdata(i, vOthFldBasedOn), "#", ""))

                                If GETCOLTOT(argFrm.vsfDet, CStr(dblFldBasedOnValCol)) <> 0 Then
                                    dblResult = dblFldAppVal * Val(argFrm.vsfDet.getdata(x, dblFldBasedOnValCol)) / GETCOLTOT(argFrm.vsfDet, CStr(dblFldBasedOnValCol))
                                End If
                                '& " * " & .Getdata(i, vOthFldBasedOn) & " / SVTCOLSUM(" & .Getdata(i, vOthFldBasedOn) & ")"
                            Else
                                dblFldAppVal = fnParse(argFrm, argFrm.vsfDet, argFrm.VsfOptions, .Getdata(i, vOthFLdApportinate), .Getdata(i, vOthFldCaption), argFrm.cboPfx.Text, argFrm.txtNo.Text, argFrm.txtDocNo.Text, argFrm.svtPCtrl.getCode(), argFrm.dtDocDate.Value)
                                argFrm.vsfDet.setdata(x, i, 0)
                                If argFrm.vsfDet.Rows.count > 1 Then
                                    dblFldBasedOnVal = fnParse(argFrm, argFrm.vsfDet, argFrm.VsfOptions, "SVTCOLSUM(#" & .Getdata(i, vOthFldCode) & "#", .Getdata(i, vOthFldCaption), argFrm.cboPfx.Text, argFrm.txtNo.Text, argFrm.txtDocNo.Text, argFrm.svtPCtrl.getCode(), argFrm.dtDocDate.Value)
                                    dblResult = dblFldAppVal - dblFldBasedOnVal
                                Else
                                    dblResult = dblFldAppVal
                                End If
                            End If
                            argFrm.vsfDet.setdata(x, i, SVTROUND(dblResult, Val(.getdata(i, vOthFldNumDecimals))))
                        End If
                    Next i
                End If
            Next x
        End With
        Call AddnlFldCalTotal(argFrm, argFrm.vsfOptions, argFrm.vsfDet, argDetCols, argCboTxt, "CUSTOMER")
        clsSvt = Nothing

    End Sub

    Public Sub DateFreezing(ByVal argForm As Object, ByVal argMode As String)
        If argMode = "" Or argMode = "V" Then
            argForm.dtDocDate.Enabled = False
            Exit Sub
        End If

        If getGsettings("Reports", "DateFreeze", "N") = "N" Then
            argForm.dtDocDate.Enabled = True
        Else
            argForm.dtDocDate.Enabled = False
        End If
    End Sub

    Public Function SVTPartyAddnlData(ByVal argTable As String, ByVal argLinkCol As String, ByVal argCol As String, ByVal argParty As String, ByVal argCode As String) As String
        If argTable = "" Or argLinkCol = "" Or argCol = "" Or argParty = "" Or argCode = "" Then Return ""
        If argParty = "CUST" Then
            Return clsSvtLib.getDataStringValue("Select " & argCol & " From MstCust c," & argTable & " t Where c." & argLinkCol & " = t." & argLinkCol & " and Custcode = '" & argCode & "'", "", False)
        Else
            Return clsSvtLib.getDataStringValue("Select " & argCol & " From MstSupp c," & argTable & " t Where c." & argLinkCol & " = t." & argLinkCol & " and SuppCode = '" & argCode & "'", "", False)
        End If
    End Function

    Public Function SVTPartyData(ByVal argCol As String, ByVal argParty As String, ByVal argCode As String) As String
        If argCol = "" Or argParty = "" Or argCode = "" Then Return ""
        If argParty = "CUST" Then
            Return clsSvtLib.getDataStringValue("Select " & argCol & " From MstCust  Where Custcode = '" & argCode & "'", "", False)
        Else
            Return clsSvtLib.getDataStringValue("Select " & argCol & " From MstSupp  Where SuppCode = '" & argCode & "'", "", False)
        End If
    End Function

    Public Function ChkDuplicateData(ByVal argOpt As C1FlexGrid, ByVal argVsf As C1FlexGrid, ByRef argDupRow As Integer,
                                     ByRef argDupDataCaption As String, ByRef argDupDataValue As String) As Boolean
        Dim intCols As Object
        Dim strUniqChk As String, strUniqKey As String
        ChkDuplicateData = False

        Dim intChkCols As New ArrayList
        Dim strHdr As String, strVal As String
        Dim i As Integer, j As Integer, k As Integer
        strHdr = ""
        For k = 0 To argOpt.Rows.Count - 1
            'MsgBox(Trim(argOpt.GetData(k, vOthFldCode)))
            If Trim(argOpt.GetData(k, vOthFldBlnChkDuplicate)) = "Y" Then
                intChkCols.Add(k)
                strHdr = strHdr & "   [" & argOpt.GetData(k, vOthFldCaption) & "]   "
            End If
        Next
        argDupRow = 0

        With argVsf
            For i = 1 To .Rows.Count - 1
                strUniqKey = ""
                strVal = ""
                For Each intCols In intChkCols
                    strUniqKey = strUniqKey & "|" & Trim(.GetData(i, intCols))
                    strVal = strVal & "  [" & .GetData(i, intCols) & "]   "
                Next intCols
                For j = 1 To .Rows.Count - 1
                    strUniqChk = ""
                    For Each intCols In intChkCols
                        strUniqChk = strUniqChk & "|" & Trim(.GetData(j, intCols))
                    Next intCols
                    If i <> j Then
                        If strUniqChk <> "" And strUniqChk <> "" Then
                            If strUniqKey = strUniqChk And strUniqChk <> "|" Then
                                argDupRow = i
                                argDupDataCaption = strHdr
                                argDupDataValue = strVal
                                ChkDuplicateData = True
                                Exit For
                            End If
                        End If
                    End If
                Next j
            Next i
        End With
    End Function

    Public Sub vsfOptCellButtonClick(argText As String, argMaxLen As Integer, argCaption As String, argForm As Object, Optional ByVal blnReadOnly As Boolean = False)
        Dim frmNewUtlNotesPopUP As New frmUtlMultilineNote
        With frmNewUtlNotesPopUP
            .objfrm = argForm
            .txtNote.Text = argText
            .txtNote.MaxLength = argMaxLen
            .Text = argCaption
            If blnReadOnly Then
                .txtNote.ReadOnly = True
                .btnSave.Visible = False
            End If
            .StartPosition = FormStartPosition.CenterScreen
            .ShowDialog()
        End With
    End Sub

    Public Sub vsfGenHideMndtAllGrids(ByVal argFrm As Form)
        Dim j As Integer
        Dim rg As CellRange
        Dim obj As Object = argFrm
        Dim ctrlList = GetAllDsgrVsf(argFrm, GetType(C1.Win.C1FlexGrid.C1FlexGrid))

        For Each argVsf As C1FlexGrid In ctrlList
            With argVsf
                Dim csBlack As CellStyle = argVsf.Styles.Add("BLACK")
                Select Case UCase(argFrm.Name)
                    Case "FRMTRNINV", "FRMTRNPB"
                        csBlack.ForeColor = Color.White
                    Case Else
                        csBlack.ForeColor = Color.Black
                End Select


                Dim csRed As CellStyle = argVsf.Styles.Add("RED")
                Select Case UCase(argFrm.Name)
                    Case "FRMTRNINV", "FRMTRNPB"
                        csRed.ForeColor = Color.White
                    Case Else
                        csRed.ForeColor = Color.DarkRed
                End Select


                Dim csBlue As CellStyle = argVsf.Styles.Add("BLUE")
                Select Case UCase(argFrm.Name)
                    Case "FRMTRNINV", "FRMTRNPB"
                        csBlue.ForeColor = Color.White
                    Case Else
                        csBlue.ForeColor = Color.Blue
                End Select

                '#  Code to hide and make mandatory clr as red
                For j = 0 To .Rows.Count - 1
                    'hide row if to be hidden
                    If .GetData(j, vOthFLdBlnHide) = "Y" Then
                        If .GetData(j, vOthFldHideOperator) = "" Then
                            .Rows(j).Visible = False
                        Else
                            .Rows(j).Visible = True
                            If fnExpEvaluate(.GetData(j, vOthFldHideCompareVal),
                                                     .GetData(j, vOthFldHideWithVal),
                                                     .GetData(j, vOthFldHideOperator),
                                                      argFrm, "") Then
                                .Rows(j).Visible = False
                            End If
                        End If
                        If .Rows(j).Visible = False And .GetData(j, vOthFldClrBefHide) = "Y" Then
                            .SetData(j, vOthFldDataValue, "")
                            .SetData(j, vOthFldDataPer, "")
                        End If
                    End If

                    rg = .GetCellRange(j, vOthFldCaption)
                    rg.Style = argVsf.Styles("BLACK")

                    'mark red if mandatory
                    If .GetData(j, vOthFldBlnMandatory) = "Y" Then
                        If .GetData(j, vOthFldMndtryOperator) = "" Then
                            rg.Style = argVsf.Styles("RED")
                        Else
                            If fnExpEvaluate(.GetData(j, vOthFldMndtryCompareVal),
                                                    .GetData(j, vOthFldMndtryWithVal),
                                                    .GetData(j, vOthFldMndtryOperator),
                                                    argFrm, "") Then
                                rg.Style = argVsf.Styles("RED")
                            End If
                        End If
                    End If

                    'mark blue if read only
                    If .GetData(j, vOthFldBlnReadOnly) = "Y" Then
                        If .GetData(j, vOthFldROOperator) = "" Then
                            rg.Style = argVsf.Styles("BLUE")
                        Else
                            If fnExpEvaluate(.GetData(j, vOthFldROCompareVal),
                                                    .GetData(j, vOthFldROWithVal),
                                                    .GetData(j, vOthFldROOperator),
                                                     argFrm, "") Then
                                rg.Style = argVsf.Styles("BLUE")
                            End If
                        End If
                    End If
                Next j
            End With
        Next
    End Sub

    Public Sub vsfDetHideCol(ByVal argFrm As Object)
        With argFrm
            For intOptRow As Int32 = 0 To .vsfOptions.Rows.Count - 1

            Next
        End With
    End Sub
    Public Function ChkDuplicateGridData(ByVal argVsf As C1FlexGrid, blnDispMsg As Boolean, ParamArray argDupCols() As Integer) As Integer

        Dim intCols As Object
        Dim strUniqChk As String, strUniqKey As String
        ChkDuplicateGridData = -1
        Dim i As Integer, j As Integer, strVal As String
        Dim strHdr As String

        With argVsf
            For i = 1 To .Rows.Count - 1
                strUniqKey = ""
                strVal = ""
                strHdr = ""
                For Each intCols In argDupCols
                    If strHdr <> "" Then strHdr = strHdr & " -- "
                    strHdr = strHdr & .GetData(0, intCols)
                    strUniqKey = strUniqKey & "|" & Trim(.GetData(i, intCols))
                    strVal = strVal & "  [" & .GetData(i, intCols) & "]   "
                Next intCols
                For j = 1 To .Rows.Count - 1
                    strUniqChk = ""
                    For Each intCols In argDupCols
                        strUniqChk = strUniqChk & "|" & Trim(.GetData(j, intCols))
                    Next intCols
                    If i <> j Then
                        If strUniqChk <> "" And strUniqChk <> "" Then
                            If strUniqKey = strUniqChk And strUniqChk <> "|" Then
                                ChkDuplicateGridData = i
                                If blnDispMsg Then
                                    MsgBox("Duplicate value at row " & i & vbCrLf & "Columns : " & strHdr & vbCrLf & "Values:" & strVal, MsgBoxStyle.Critical, "Duplicate data")
                                    Exit Function
                                End If
                                Exit For
                            End If
                        End If
                    End If
                Next j
            Next i
        End With
    End Function
End Module