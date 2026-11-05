Imports C1.Win.C1FlexGrid
Imports System.Reflection
Imports System.IO
Imports System.ComponentModel

Public Class frmRptCustomiseRptView
    Const vDetRptID = 0
    Const vDetRptName = 1
    Const vDetRptDesc = 2
    Const vDetFilterPresent = 3
    Const vDetRptFileName = 4

    Dim clsSVT As New svtLibrary
    Dim svtTrn As oledb.oledbTransaction
    Dim intCrID As Integer
    Public strList As String
    Dim strRights As String

    Const vFilFldCode = 0
    Const vFilFldCaption = 1
    Const vFilFldDataType = 2
    Const vFilTblName = 3
    Const vFilBtn = 4
    Const vFilCol1 = 5
    Const vFilCol2 = 6

    Const vSelFilFldCode = 0
    Const vSelFldCaption = 1
    Const vSelFldFilterValue = 2
    Const vSelFilProcessed = 3
    Const vSelFilTblName = 4

    Const vExpColQry = 0
    Const vExpColTol = 1

    Const vExpRowCaption = 0
    Const vExpRowCRName = 1
    Const vExpRowHdr = 2
    Const vExpRowDtl = 3
    Const vExpRowTbl = 4
    Const vExpRowTol = 5

    Dim strRptPrintHdr As String, strRptPrintFilter As String
    Dim blnSlnoReq As Boolean


    Dim dtSearch As New DataTable
    Dim dvSearch As New DataView

    Private Sub btnDesign_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDesign.Click
        If Not chkUserRights(strRights, chkRightsPos.AddRights, Me.Text) Then Exit Sub
        Dim frm As New frmRptCRDesign
        frm.StartPosition = FormStartPosition.CenterParent
        If vsfDet.Row >= 1 Then
            frm.StrValue = vsfDet.GetData(vsfDet.Row, 1)
        End If
        frm.ShowDialog()
        frm.Dispose()
        Call fillReport()
    End Sub

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Dim tb As New C1.Win.C1Command.C1DockingTabPage()
        Dim dockTabMain As New C1.Win.C1Command.C1DockingTab()
        Me.Dispose()
        tb.Controls.Clear()
        frmMain.dockTabMain.SelectedTab.Dispose()

        'Me.Close()
    End Sub

    Public Sub fillReport()
        Dim strRptGroup As String
        fraFilters.Visible = False
        Dim cs As CellStyle
        lblReportName.Text = ""
        cs = vsfDet.Styles.Add("ImgCell")
        cs.ImageAlign = ImageAlignEnum.TileStretch
        cs.TextAlign = TextAlignEnum.LeftCenter
        cs.Font = New Font("Tahoma", 12, FontStyle.Bold)

        With vsfFilter
            .Cols.Count = 7
            .SetData(0, vFilFldCaption, "Filter on")
            .SetData(0, 4, "Select Filter")
            .SetData(0, 5, "From")
            .SetData(0, 6, "To")
            .Cols(vFilFldDataType).Visible = False
            .Cols(vFilTblName).Visible = False
            .Cols(vFilFldCode).Visible = False
            .Cols(vFilBtn).ComboList = "..."
            .Cols(vFilBtn).Width = 30
            .Cols(vFilFldCaption).Width = 190
            .Cols(vFilCol1).Width = 100
            .Cols(vFilCol2).Width = 100
        End With

        With vsfSelFilters
            .Cols.Count = 6
            .SetData(0, vSelFldCaption, "Filtered On")
            .SetData(0, vSelFldFilterValue, "Filtered Value")
            .Cols(vSelFilFldCode).Visible = False
            .Cols(vSelFilProcessed).Visible = False
            .Cols(vSelFilTblName).Visible = False
        End With

        With vsfDet
            .Cols.Count = 5
            .Rows.Count = 0
            .Cols(vDetRptName).TextAlign = TextAlignEnum.LeftCenter
            .Cols(vDetRptID).Visible = False
            .Cols(vDetRptDesc).Visible = False
            .Cols(vDetFilterPresent).Visible = False
            .Cols(vDetRptFileName).Visible = False
        End With

        'With vsfSearch
        '    If .Rows.Count <= 0 Then
        '        .Rows.Count = 1
        '    End If
        '    .Cols.Count = 2
        '    .SetData(0, 1, "Report Name")
        'End With


        Dim strSQL As String
        Dim rs As oledb.oledbDataReader
        strSQL = "Select ReportGroup, CRID, CRName, CRDesc, RptFileName,"
        strSQL = strSQL & " fq = (select count(Filterreq) from UtlReportsCRDtl where Utlreportscrhdr.CRID = Utlreportscrdtl.crid and Filterreq = 'Y')"
        strSQL = strSQL & " from UtlReportsCRHdr "
        If My.Application.Info.AssemblyName = "PosFA" Then
            strSQL = strSQL & " where ReportGroup = 'FixedAsset'"
        End If
        If gUserCode <> "ADMIN" Then
            strSQL = strSQL & " WHERE CRID IN (SELECT CRID FROM UtlReportsCRRights WHERE USERID =  " & Val(gUserID) & ")"
        End If
        strSQL = strSQL & " order by ReportGroup, CRName"
        rs = clsSVT.getDataReaderFromSQL(strSQL)
        strRptGroup = ""
        With rs
            If .HasRows Then
                Do While .Read
                    If strRptGroup <> !ReportGroup Then
                        vsfDet.Rows.Count = vsfDet.Rows.Count + 1
                        vsfDet.SetData(vsfDet.Rows.Count - 1, vDetRptName, !ReportGroup)
                        'vsfDet.SetCellImage(vsfDet.Rows.Count - 1, vDetRptName, My.Resources.VsfCR)
                        vsfDet.SetCellStyle(vsfDet.Rows.Count - 1, vDetRptName, cs)
                    End If

                    vsfDet.Rows.Count = vsfDet.Rows.Count + 1
                    vsfDet.SetData(vsfDet.Rows.Count - 1, vDetRptName, !CRName)
                    vsfDet.SetData(vsfDet.Rows.Count - 1, vDetRptID, !CRID)
                    vsfDet.SetData(vsfDet.Rows.Count - 1, vDetRptDesc, !CRDesc)
                    vsfDet.SetData(vsfDet.Rows.Count - 1, vDetRptFileName, GetVal(!RptFileName, ""))

                    If !fq > 0 Then
                        vsfDet.SetData(vsfDet.Rows.Count - 1, vDetFilterPresent, "Y")
                    End If
                    strRptGroup = !ReportGroup
                Loop
            End If
            .Close()
        End With
        If vsfDet.Rows.Count > 1 Then
            vsfDet.Row = 1
            txtDesc.Text = vsfDet.GetData(vsfDet.Row, vDetRptDesc)
        End If
        vsfFilter.AutoSizeCols(0, 4, 1)
        Call vsfSearchSelectedItem()
    End Sub

    Private Sub btnViewReport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnViewReport.Click
        If Not chkUserRights(strRights, chkRightsPos.ViewRights, Me.Text) Then Exit Sub
        Call ViewReport(vsfDet.Row)
    End Sub

    Private Sub vsfDet_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles vsfDet.DoubleClick
        intCrID = vsfDet.GetData(vsfDet.Row, vDetRptID)
        If intCrID <= 0 Then
            Exit Sub
        Else
            If txtDesc.Text = "" Then
                lblDescription.Visible = True
                lblDescription.Text = "Report Description"
            Else
                lblDescription.Visible = False
            End If
            Call ViewReport(vsfDet.Row)
        End If
    End Sub
    Private Sub ViewReport(ByVal argRow As Integer)
        txtRptName.Text = vsfDet.GetData(argRow, vDetRptName)
        intCrID = vsfDet.GetData(argRow, vDetRptID)
        Call retreiveData()
    End Sub

    Private Function ValidateFlds() As Boolean
        Dim blnFound As Boolean
        ValidateFlds = False
        'check the date range
        Call clsSVT.getDataStringValue("Select CRName from UtlReportsCRHdr Where CRID = " & intCrID, "", blnFound)
        If Not blnFound Then
            MsgBox("Invalid report.. click on Design to create new report!!", MsgBoxStyle.Critical, Me.Text)
            Exit Function
        End If
        'If CDate(dtTDt.value) < CDate(dtFdt.value) Then
        '    MsgBox("Invalid Date Range!", vbInformation, Me.Caption)
        '    Exit Function
        'End If

        'With vsfFltOn
        '    For i = 1 To vsfFltOn.Rows - 1
        '        If .Cell(flexcpChecked, i, vFltSel) = _
        '            flexChecked And .TextMatrix(i, vFltSelString) = "" Then
        '        MsgBox .TextMatrix(i, vFltFldDisp) & " List not Selected!!", vbInformation + vbOKOnly, Me.Caption
        '            Exit Function
        '        End If
        '    Next i
        'End With
        ValidateFlds = True
    End Function

    Private Sub retreiveData()
        Dim strSQL As String, intCtr As Integer
        strSQL = getSTRSQL()
        Dim frmCRpt As New frmRptView
        Dim strTag As String
        Dim rsDummy As oledb.oledbDataReader

        rsDummy = clsSVT.getDataReaderFromSQL("Select * from UtlreportsCRDtl where CRID = " & intCrID & " order by FldOrderBy")

        With frmCRpt
            '.MdiParent = frmMain
            strTag = (txtRptName.Text)
            'If chkDateRange.value = vbChecked Then
            'strTag = strTag & "For the Period of {" & dtFdt.value & "} To {" & dtTDt.value & "}"
            'End If
            .Text = strTag

            .FillViewVsf(Me, strSQL, strTag, "", argblnSlno:=blnSlnoReq)
            .Show()
            'intHdrFlds = clsSVT.GetDataValue("Count(fldName)", 0, "UtlCustomisedRpts", _
            '            False, " where  RptName='" & Trim(cboDocName.Text) & "' and FldArea='H'")
            ''MsgBox .vsfView.Cols
            'For i = 0 To intHdrFlds - 1
            '    .vsfView.MergeCol(i) = True
            'Next i

            With frmCRpt.vsfDet
                .AllowDragging = AllowDraggingEnum.None
                .AllowEditing = False
                '.Cols(0).WidthDisplay = 3
                .Tree.Column = 0
            End With

            frmCRpt.chkRemoveSubTot.Visible = False
            'put subtotals
            .vsfDet.SubtotalPosition = C1.Win.C1FlexGrid.SubtotalPositionEnum.BelowData

            Me.Cursor = Cursors.WaitCursor
            With rsDummy
                intCtr = -1
                If .HasRows Then
                    Do While .Read
                        intCtr = intCtr + 1
                        For i = 0 To frmCRpt.vsfDet.Cols.Count - 1
                            If frmCRpt.vsfDet.GetData(0, i) = !Fldcode Then
                                frmCRpt.vsfDet.SetData(3, i, !Fldcaption)
                                Exit For
                            End If

                            If frmCRpt.vsfDet.GetData(0, i) = "CountOf" & !Fldcode Then
                                frmCRpt.vsfDet.SetData(3, i, !Fldcaption)
                                Exit For
                            End If

                            If frmCRpt.vsfDet.GetData(0, i) = "SumOf" & !Fldcode Then
                                frmCRpt.vsfDet.SetData(3, i, !Fldcaption)
                                Exit For
                            End If
                            If frmCRpt.vsfDet.GetData(0, i) = "AvgOf" & !Fldcode Then
                                frmCRpt.vsfDet.SetData(3, i, !Fldcaption)
                                Exit For
                            End If
                        Next i

                        For i = 0 To frmCRpt.vsfDet.Cols.Count - 1
                            If GetVal(!AddnlCol, "") <> "" Then
                                Dim strWordArr() As String = CStr(!AddnlCol).Split("|")
                                If frmCRpt.vsfDet.GetData(0, i) = UCase(strWordArr(1)) Then
                                    frmCRpt.vsfDet.SetData(3, i, UCase(strWordArr(1)))
                                    Exit For
                                End If
                            End If
                        Next

                        If !TotReq = "Y" Then
                            For i = 0 To frmCRpt.vsfDet.Cols.Count - 1
                                If blnSlnoReq Then
                                    If intCtr + 1 = i Then
                                        frmCRpt.chkRemoveSubTot.Visible = True
                                        frmCRpt.vsfDet.Subtotal(C1.Win.C1FlexGrid.AggregateEnum.Sum, 0, -1, intCtr + 1, "Grand Total")
                                        Call putSubTot(frmCRpt.vsfDet, intCtr + 1)
                                    End If
                                Else
                                    If intCtr = i Then
                                        frmCRpt.chkRemoveSubTot.Visible = True
                                        frmCRpt.vsfDet.Subtotal(C1.Win.C1FlexGrid.AggregateEnum.Sum, 0, -1, intCtr, "Grand Total")
                                        Call putSubTot(frmCRpt.vsfDet, intCtr)
                                    End If

                                End If
                            Next i
                        End If
                    Loop
                End If
                .Close()
            End With
        End With

        With frmCRpt.vsfDet
            .AllowMerging = AllowMergingEnum.RestrictAll
            For i = 0 To .Cols.Count - 2
                .Cols(i).AllowMerging = True
            Next
            '.AutoSizeCols()
            frmCRpt.syncGridCols()
        End With
        Me.Cursor = Cursors.Default
    End Sub

    Private Sub putSubTot(ByVal argVSF As C1.Win.C1FlexGrid.C1FlexGrid, ByVal argCol As Integer)
        Dim intC As Integer
        Dim frmCRpt As New frmRptView
        Dim Rs As oledb.oledbDataReader
        Rs = clsSVT.getDataReaderFromSQL("Select * from UtlReportsCRDtl where CRID = " & intCrID & " order by FldOrderBy")

        'argVSF.Subtotal
        With Rs
            intC = -1
            If .HasRows Then
                Do While .Read
                    intC = intC + 1
                    If !SubTotOn = "Y" Then
                        For i = 0 To frmCRpt.vsfDet.Cols.Count - 1
                            If intC = i Then
                                argVSF.Subtotal(C1.Win.C1FlexGrid.AggregateEnum.Sum, 1, intC, argCol, "{0} SubTotal")
                            End If
                        Next i
                    End If
                Loop
            End If
            .Close()
        End With
    End Sub


    Private Function getSTRSQL() As String
        Dim intCtr As String, strFMT As String = "", blnFound As Boolean
        Dim strCodeName As String, strFldList As String, strTblName As String = ""
        Dim strSQL As String, strOrderBy As String, strTbls As String, strCond As String
        Dim rsDummy As oledb.oledbDataReader, intGroupByCount As Integer
        Dim strGroupBy As String = "", i As Integer
        Dim strCondText As String = clsSVT.getDataStringValue("Select CondText from UtlreportsCRHdr where CRID = " & intCrID, "", False)
        blnSlnoReq = False
        clsSVT.getDataStringValue("Select IsSlnoReq from UtlreportsCRHdr where IsSlnoReq='Y' and  CRID = " & intCrID, "", blnSlnoReq)
        strRptPrintHdr = ""
        strRptPrintFilter = ""
        'get the Fields, group by and order clauses
        intGroupByCount = clsSVT.getDataLongValue("Select Count(FldAggregate) from UtlReportsCRDtl  where CRID = " & intCrID & " and FldAggregate <> ''", 0, False)
        rsDummy = clsSVT.getDataReaderFromSQL("Select * from UtlReportsCRDtl where CRID = " & intCrID & " order by FldOrderBy ")
        strOrderBy = " order by "
        strSQL = "Select "
        If blnSlnoReq Then
            strSQL &= "'' as AutoSlno,"
            strFMT = "AUTOSLNO|"
        Else
            strFMT = ""
        End If


        strGroupBy = ""
        With rsDummy
            intCtr = 0
            If .HasRows Then
                Do While .Read
                    If !FldHideCol <> "Y" Then
                        strFMT = strFMT & !Fldcaption & "|"
                        If !FldAggregate <> "" Then
                            strSQL = strSQL & !FldAggregate & "(" & !TblName & "." & !FldCode & ") as " & !FldAggregate & "Of" & !FldCode & ","
                        Else
                            strSQL = strSQL & !TblName & "." & !FldCode & ","
                            If intGroupByCount > 0 Then
                                strGroupBy = strGroupBy & !TblName & "." & !FldCode & ","
                            End If
                        End If
                        intCtr = intCtr + 1
                        strOrderBy = strOrderBy & intCtr & ","
                    End If
                    If GetVal(!AddnlCol, "") <> "" Then
                        Dim strWordArr() As String = CStr(!AddnlCol).Split("|")
                        strFMT = strFMT & UCase(strWordArr(1)) & "|"
                        strSQL &= strWordArr(0) & " "
                        strSQL &= UCase(strWordArr(1)) & ","
                        intCtr = intCtr + 1
                        strOrderBy = strOrderBy & intCtr & ","
                    End If
                Loop
                .Close()
            End If
        End With
        If Trim(strSQL) <> "" Then strSQL = Mid(strSQL, 1, Len(strSQL) - 1)
        If Trim(strFMT) <> "" Then strFMT = Mid(strFMT, 1, Len(strFMT) - 1)
        If Trim(strGroupBy) <> "" Then strGroupBy = Mid(strGroupBy, 1, Len(strGroupBy) - 1)
        If Trim(strOrderBy) <> "" Then strOrderBy = Mid(strOrderBy, 1, Len(strOrderBy) - 1)

        'get the Table names and conditions
        rsDummy = clsSVT.getDataReaderFromSQL("Select * from UtlReportsCRTbl where CRID = " & intCrID & " and tblname in (select tblname from UtlReportsCRDtl where crid = " & intCrID & ")")
        strTbls = ""
        strCond = " Where 1 = 1 "
        With rsDummy
            If .HasRows Then
                Do While .Read
                    strTbls = strTbls & !TblName & ","
                    If !TblLinkCond <> "" Then
                        strCond = strCond & " and " & !TblLinkCond
                    End If
                Loop
            End If
            .Close()
        End With

        strCond &= " " & strCondText & " "

        If Trim(strTbls) <> "" Then strTbls = Mid(strTbls, 1, Len(strTbls) - 1)

        strSQL = strSQL & " from " & strTbls

        If fraFilters.Visible Then
            With vsfFilter
                For i = 1 To .Rows.Count - 1
                    If .GetData(i, vFilFldDataType) = "DATE" Then
                        strCond = strCond & " and " & .GetData(i, vFilTblName) & "." & .GetData(i, vFilFldCode) &
                            " between " & getDateFormatInCond(.GetData(i, vFilCol1)) &
                            " and " & getDateFormatInCond(.GetData(i, vFilCol2))
                        strRptPrintHdr = " Report for the period: from " & getDateFormatInInsert(.GetData(i, vFilCol1)) & " to " & getDateFormatInInsert(.GetData(i, vFilCol2))
                    End If
                Next
            End With

            With vsfSelFilters
                Do While True
                    blnFound = False
                    strCodeName = ""
                    For i = 1 To .Rows.Count - 1
                        If .GetData(i, vSelFilProcessed) = "" Then
                            blnFound = True
                            strCodeName = .GetData(i, vSelFilFldCode)
                            strTblName = .GetData(i, vSelFilTblName)
                            strRptPrintFilter &= " Report Filtered by " & .GetData(i, vSelFilFldCode) & " and the values are ("
                            Exit For
                        End If
                    Next
                    If strCodeName = "" Then Exit Do
                    strFldList = ""
                    For i = 1 To .Rows.Count - 1
                        If strCodeName = .GetData(i, vSelFilFldCode) Then

                            strFldList = strFldList & .GetData(i, vSelFldFilterValue) & ","
                            .SetData(i, vSelFilProcessed, "Y")
                            'strFldList = Replace(strFldList, "'", "")
                        End If
                    Next
                    If strFldList <> "" Then
                        strFldList = Mid(strFldList, 1, Len(strFldList) - 1)
                        strCond = strCond & " and " & strTblName & "." & strCodeName & " in (" & strFldList & ")"
                        strRptPrintFilter &= strFldList & ") "
                    End If
                Loop

                'reset processed for next run
                For i = 1 To .Rows.Count - 1
                    .SetData(i, vSelFilProcessed, "")
                Next
            End With


        End If


        'If chkDateRange.value = vbChecked And Trim(lblDateFld.Caption) <> "" Then
        '    If strCond = "" Then
        '        strCond = strCond & " where " & Trim(lblDateFld.Caption) & _
        '            " between " & getDateFormatInCond(dtFdt.value) & _
        '            " and " & getDateFormatInCond(dtTDt.value)
        '    Else
        '        strCond = strCond & " and " & Trim(lblDateFld.Caption) & _
        '            " between " & getDateFormatInCond(dtFdt.value) & _
        '            " and " & getDateFormatInCond(dtTDt.value)
        '    End If
        'End If
        'If strCond = " Where " Then strCond = ""

        strSQL = strSQL & strCond
        If strGroupBy <> "" Then
            strSQL = strSQL & " group by " & strGroupBy
        End If
        strSQL = strSQL & strOrderBy
        getSTRSQL = strSQL


    End Function

    Private Sub vsfDet_RowColChange(ByVal sender As Object, ByVal e As System.EventArgs) Handles vsfDet.RowColChange
        fraFilters.Visible = False
        lblReportName.Text = ""
        Try
            txtDesc.Text = vsfDet.GetData(vsfDet.Row, vDetRptDesc)
            intCrID = vsfDet.GetData(vsfDet.Row, vDetRptID)
            If intCrID <= 0 Then
                btnViewReport.Enabled = False
            Else
                lblReportName.Text = vsfDet.GetData(vsfDet.Row, vDetRptName)
                btnViewReport.Enabled = True
                If Trim(vsfDet.GetData(vsfDet.Row, vDetRptFileName)) <> "" Then
                    btnPrintReport.Enabled = True
                Else
                    btnPrintReport.Enabled = False
                End If
            End If
            If vsfDet.GetData(vsfDet.Row, vDetFilterPresent) = "Y" Then
                fraFilters.Visible = True
                Call fillFilterFields()
            Else
                fraFilters.Visible = False
            End If
        Catch ex As Exception

        End Try

    End Sub

    Private Sub fillFilterFields()
        Dim rg As CellRange
        Dim cs As CellStyle
        'cs = vsfFilter.Styles.Add("bool")
        'cs.DataType = GetType(Boolean)
        'cs.ImageAlign = ImageAlignEnum.CenterCenter
        Dim blnDateFldPresent As Boolean
        Dim blnFilterPresent As Boolean
        cs = vsfFilter.Styles.Add("date")
        cs.DataType = GetType(DateTime)
        cs.Format = "dd-MMM-yy"
        cs.ForeColor = Color.DarkGoldenrod

        Dim rsDummy As oledb.oledbDataReader
        rsDummy = clsSVT.getDataReaderFromSQL("Select * from UtlReportsCRDtl where FilterReq = 'Y' and CRID = " & intCrID & " order by FldOrderBy ")
        vsfFilter.Rows.Count = 1
        vsfSelFilters.Rows.Count = 1
        blnDateFldPresent = False
        blnFilterPresent = False
        With rsDummy
            If .HasRows Then
                Do While .Read
                    vsfFilter.Rows.Count = vsfFilter.Rows.Count + 1
                    vsfFilter.SetData(vsfFilter.Rows.Count - 1, vFilTblName, !TblName)
                    vsfFilter.SetData(vsfFilter.Rows.Count - 1, vFilFldCode, !Fldcode)
                    vsfFilter.SetData(vsfFilter.Rows.Count - 1, vFilFldCaption, !Fldcaption)
                    vsfFilter.SetData(vsfFilter.Rows.Count - 1, vFilFldDataType, !FldDataType)

                    If !fldDataType = "DATE" Then
                        vsfFilter.SetData(vsfFilter.Rows.Count - 1, vFilCol1, Format(getSystemDate, "dd/MM/yyyy"))
                        vsfFilter.SetData(vsfFilter.Rows.Count - 1, vFilCol2, Format(getSystemDate, "dd/MM/yyyy"))
                        rg = vsfFilter.GetCellRange(vsfFilter.Rows.Count - 1, vFilCol1, vsfFilter.Rows.Count - 1, vFilCol2)
                        rg.Style = vsfFilter.Styles("date")
                        blnDateFldPresent = True
                    Else
                        blnFilterPresent = True
                    End If
                Loop
            End If
            .Close()
        End With
        If Not blnDateFldPresent Then
            vsfFilter.Cols(vFilCol1).Visible = False
            vsfFilter.Cols(vFilCol2).Visible = False
        Else
            vsfFilter.Cols(vFilCol1).Visible = True
            vsfFilter.Cols(vFilCol2).Visible = True
        End If
        If Not blnFilterPresent Then
            vsfFilter.Cols(vFilBtn).Visible = False
            lblSelFilters.Visible = False
            vsfSelFilters.Visible = False
            btnRemFilter.Visible = False
            btnClearFilters.Visible = False
        Else
            vsfFilter.Cols(vFilBtn).Visible = True
            lblSelFilters.Visible = True
            vsfSelFilters.Visible = True
            btnRemFilter.Visible = True
            btnClearFilters.Visible = True
            btnPrintReport.Visible = True
            btnRemFilter.Enabled = False
            btnClearFilters.Enabled = False
        End If
    End Sub

    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        fraFilters.Visible = False
    End Sub

    Private Sub vsfFilter_AfterEdit(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfFilter.AfterEdit
        'Dim strSQL As String = ""
        'With vsfFilter
        '    If .Col = vFilCol1 And .GetData(.Row, vFilFldDataType) <> "DATE" Then
        '        If .GetCellCheck(.Row, vFilCol1) = CheckEnum.Checked Then
        '            'show the selection form From respective master
        '            Dim frmSel As New frmUtlMultiSelection
        '            strSQL = "Select distinct 0 as Sel, " & .GetData(.Row, vFilFldCode) & " from " & .GetData(.Row, vFilTblName) & " order by 1"
        '            strList = ""
        '            vsfRetGrid.Rows.Count = 1
        '            If .GetData(.Row, vFilFldDataType) = "NUMERIC" Then
        '                Call frmSel.InitVsfSel(strSQL, -1, 1, True, Me, True)
        '            Else
        '                Call frmSel.InitVsfSel(strSQL, -1, 1, False, Me, True)
        '            End If

        '            frmSel.ShowDialog()
        '        End If
        '        Call FillListInSelectedGrid(.GetData(.Row, vFilFldCode), .GetData(.Row, vFilFldCaption), .GetData(.Row, vFilFldDataType))
        '    End If
        'End With
    End Sub

    Private Sub vsfFilter_BeforeEdit(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfFilter.BeforeEdit
        If e.Col = vFilBtn And vsfFilter.GetData(e.Row, vFilFldDataType) = "DATE" Then
            e.Cancel = True
            Exit Sub
        End If
        If e.Col = vFilCol1 And vsfFilter.GetData(e.Row, vFilFldDataType) <> "DATE" Then
            e.Cancel = True
            Exit Sub
        End If
        If e.Col = vFilCol2 And vsfFilter.GetData(e.Row, vFilFldDataType) <> "DATE" Then
            e.Cancel = True
            Exit Sub
        End If
        If e.Col <> vFilCol1 And e.Col <> vFilCol2 And e.Col <> vFilBtn Then
            e.Cancel = True
            Exit Sub
        End If
    End Sub

    Private Sub FillListInSelectedGrid(ByVal argFldCode As String, ByVal argFldCaption As String, ByVal argDataType As String, ByVal argFldTblName As String)
        Dim i As Integer, blnAdded As Boolean, blnFound As Boolean
        With vsfSelFilters
            'add from the grid which are selected
            blnAdded = False
            blnFound = False
            For i = 1 To vsfRetGrid.Rows.Count - 1
                If vsfRetGrid.GetCellCheck(i, 0) = C1.Win.C1FlexGrid.CheckEnum.Checked Then
                    For x = 1 To .Rows.Count - 1
                        If .GetData(x, vSelFilFldCode) = argFldCode And Replace(.GetData(x, vSelFldFilterValue), "'", "") = vsfRetGrid.GetData(i, 1) Then
                            blnFound = True
                            blnAdded = True
                            Exit For
                        End If
                    Next

                    If Not blnFound Then
                        blnAdded = True
                        .Rows.Count = .Rows.Count + 1
                        .SetData(.Rows.Count - 1, vSelFilFldCode, argFldCode)
                        .SetData(.Rows.Count - 1, vSelFilTblName, argFldTblName)
                        .SetData(.Rows.Count - 1, vSelFldCaption, argFldCaption)
                        If argDataType = "NUMERIC" Then
                            .SetData(.Rows.Count - 1, vSelFldFilterValue, vsfRetGrid.GetData(i, 1))
                        Else
                            .SetData(.Rows.Count - 1, vSelFldFilterValue, "'" & vsfRetGrid.GetData(i, 1) & "'")
                        End If
                    End If
                End If
            Next i

            .AutoSizeRows()
            Dim order As C1.Win.C1FlexGrid.SortFlags = C1.Win.C1FlexGrid.SortFlags.Ascending
            .Sort(order, vSelFilFldCode)
            If .Rows.Count > 1 Then
                btnRemFilter.Enabled = True
                btnClearFilters.Enabled = True
                btnPrintReport.Enabled = True
            Else
                btnRemFilter.Enabled = False
                btnClearFilters.Enabled = False
                btnPrintReport.Enabled = False
            End If
        End With
    End Sub

    Private Sub vsfFilter_CellButtonClick(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfFilter.CellButtonClick
        Dim strSQL As String
        With vsfFilter
            'show the selection form From respective master
            Dim frmSel As New frmUtlMultiSelection
            strSQL = "Select distinct 0 as Sel, " & .GetData(e.Row, vFilFldCode) & " from " & .GetData(e.Row, vFilTblName) & " order by 1"
            strList = ""
            vsfRetGrid.Rows.Count = 1
            If .GetData(.Row, vFilFldDataType) = "NUMERIC" Then
                Call frmSel.InitVsfSel(strSQL, -1, 1, True, Me, True)
            Else
                Call frmSel.InitVsfSel(strSQL, -1, 1, False, Me, True)
            End If
            frmSel.Text = "Select [Multiple]" & .GetData(e.Row, vFilFldCaption)
            frmSel.StartPosition = FormStartPosition.CenterScreen
            frmSel.ShowDialog()
            Call FillListInSelectedGrid(.GetData(.Row, vFilFldCode), .GetData(.Row, vFilFldCaption), .GetData(.Row, vFilFldDataType), .GetData(.Row, vFilTblName))
        End With
    End Sub

    Private Sub btnRemFilter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRemFilter.Click
        If Not chkUserRights(strRights, chkRightsPos.EditRights, Me.Text) Then Exit Sub
        With vsfSelFilters
            If .Row <= 0 Then Exit Sub
            .RemoveItem(.Row)
            If .Rows.Count = 1 Then
                btnRemFilter.Enabled = False
                btnClearFilters.Enabled = False
                btnPrintReport.Enabled = True
            End If
        End With

    End Sub

    Private Sub btnClearFilters_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClearFilters.Click
        If Not chkUserRights(strRights, chkRightsPos.EditRights, Me.Text) Then Exit Sub
        'Dim strCodeName As String, i As Integer
        With vsfSelFilters
            .Rows.Count = 1
            btnRemFilter.Enabled = False
            btnClearFilters.Enabled = False
            btnPrintReport.Enabled = True
        End With
    End Sub

    Private Sub vsfSelFilters_AfterEdit(sender As Object, e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfSelFilters.AfterEdit
        If vsfSelFilters.Rows.Count > 1 Then
            btnRemFilter.Enabled = True
            btnClearFilters.Enabled = True
            btnPrintReport.Enabled = True
        Else
            btnRemFilter.Enabled = False
            btnClearFilters.Enabled = False
            btnPrintReport.Enabled = False
        End If

    End Sub

    Private Sub frmRptCustomiseRptView_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.KeyPreview = True
        initGridStyles(Me)
        Call initFormVsfDet(Me, Me.Text, vsfFilter, "FIL")
        Call initFormVsfDet(Me, Me.Text, vsfSelFilters, "SELFIL")
        strRights = getUserRights(GetMenuId(Me.Text))
        If Not chkUserRights(strRights, chkRightsPos.opt1Rights, Me.Text, False) Then
            btnRights.Visible = False
        Else
            btnRights.Visible = True
        End If
        txtSearch.Focus()
        txtDesc.Enabled = False
        'Call vsfSearchSelectedItem()

        With vsfExp
            .Rows.Count = vExpRowTol
            .Cols.Count = vExpColTol
        End With
    End Sub

    Private Sub btnRights_Click(sender As System.Object, e As System.EventArgs) Handles btnRights.Click
        'chnaged by Surabhi
        'Dim frmAssignRights As New frmUtlAssignRights
        Dim frmAssignRights As New frmUtlAssignRight
        Dim intCRID As Integer = 0

        ' intCRID = vsfDet.GetData(vsfDet.Row, vDetRptID)

        ' If intCRID > 0 Then
        With frmAssignRights
                ' .strRptName = vsfDet.GetData(vsfDet.Row, vDetRptName)
                ' .intCRID = intCRID
                .ShowDialog()
            End With
        ' Else
        'MsgBox("Please select a report from the grid!", vbInformation, Me.Text)
        '    vsfDet.Focus()
        '    Exit Sub
        'End If
    End Sub

    Private Sub btnPrintReport_Click(sender As Object, e As EventArgs) Handles btnPrintReport.Click
        If Not chkUserRights(strRights, chkRightsPos.PrintRights, Me.Text) Then Exit Sub
        Dim strSQL As String
        Dim strfilename As String
        Dim i As Integer, j As Integer, x As Integer

        'txtRptName.Text = vsfDet.GetData(argRow, vDetRptName)
        'intCrID = vsfDet.GetData(argRow, vDetRptID)

        txtRptName.Text = vsfDet.GetData(vsfDet.Row, vDetRptName)
        intCrID = vsfDet.GetData(vsfDet.Row, vDetRptID)

        strSQL = getSTRSQL()

        'Dim vsfDynamicGrid As New C1FlexGrid

        Dim dt As DataTable = clsSVT.getDATATable(strSQL)
        'Dim rsDummy As oledb.oledbDataReader
        'rsDummy = clsSVT.getDataReaderFromSQL("Select * from UtlreportsCRDtl where CRID = " & intCrID & " order by FldOrderBy")
        'For i = 0 To frmCRpt.vsfDet.Cols.Count - 1
        '    If frmCRpt.vsfDet.GetData(0, i) = !Fldcode Then
        '        frmCRpt.vsfDet.SetData(3, i, !Fldcaption)
        '        Exit For
        '    End If
        'Next
        'Dim dv As DataView = New DataView(dt)
        'vsfDynamicGrid.DataSource = dv

        With dt
            strSQL = "delete from RptUDR where UserName = '" & gUserCode & "' and RptName = '" & Trim(txtRptName.Text) & "'"
            clsSVT.execSQLStatement(strSQL)

            strSQL = "delete from RptUDRHeading where UserName = '" & gUserCode & "' and RptName = '" & Trim(txtRptName.Text) & "'"
            clsSVT.execSQLStatement(strSQL)

            strSQL = "Insert into RptUDRHeading (RptName,UserName,Fld1,Fld2,Fld3,Fld4,Fld5,Fld6,Fld7,Fld8,Fld9"
            strSQL = strSQL & " ,Fld10,Fld11,Fld12,Fld13,Fld14,Fld15,Fld16,Fld17,Fld18,Fld19,Fld20,HdrText,FilterText) values ("
            strSQL = strSQL & "'" & Trim(txtRptName.Text) & "',"
            strSQL = strSQL & "'" & gUserCode & "'"
            For j = 0 To .Columns.Count - 1
                If j <= 20 Then
                    strSQL = strSQL & ",'" & .Columns(j).Caption & "'"
                End If
            Next j
            For x = j + 1 To 20
                strSQL = strSQL & ",''"
            Next x
            strSQL = strSQL & ",'" & strRptPrintHdr & "'"
            strSQL = strSQL & ",'" & Replace(strRptPrintFilter, "'", "") & "'"

            strSQL = strSQL & ")"
            clsSVT.execSQLStatement(strSQL)

            For i = 0 To .Rows.Count - 1
                ' If Not .IsSubtotal(i) Then
                strSQL = "Insert into RptUDR (RptName,UserName,Fld1,Fld2,Fld3,Fld4,Fld5,Fld6,Fld7,Fld8,Fld9"
                strSQL = strSQL & " ,Fld10,Fld11,Fld12,Fld13,Fld14,Fld15,Fld16,Fld17,Fld18,Fld19,Fld20) values ("
                strSQL = strSQL & "'" & Trim(txtRptName.Text) & "',"
                strSQL = strSQL & "'" & gUserCode & "'"
                For j = 0 To .Columns.Count - 1
                    If j <= 20 Then
                        strSQL = strSQL & ",'" & .Rows(i).Item(j) & "'"
                    End If
                Next j
                For x = j + 1 To 20
                    strSQL = strSQL & ",''"
                Next x
                strSQL = strSQL & ")"
                clsSVT.execSQLStatement(strSQL)
                ' End If
            Next i
        End With
        strfilename = clsSVT.getDataStringValue("Select RptFileName from UtlReportsCRHdr where CRName = '" & Trim(txtRptName.Text) & "'", "", False)
        If strfilename = "" Then
            MsgBox("Please specify the Report File name in Report Designer!", vbInformation, Me.Text)
            Exit Sub
        End If

        If Not File.Exists(gstrRptPath & strfilename) Then
            MsgBox("File " & gstrRptPath & strfilename & " does not exist! Cannot Print!", vbCritical, Me.Text)
            Exit Sub
        End If

        Dim strSelFormula As String
        Dim strArrFrmla(,) As String = Nothing
        ReDim strArrFrmla(4, 1)

        strArrFrmla(0, 0) = "compname"
        strArrFrmla(0, 1) = gSettings.gCompName
        strArrFrmla(1, 0) = "add1"
        strArrFrmla(1, 1) = gSettings.gCompAdd1
        strArrFrmla(2, 0) = "add2"
        strArrFrmla(2, 1) = gSettings.gCompAdd2
        strArrFrmla(3, 0) = "add3"
        strArrFrmla(3, 1) = gSettings.gCompAdd3
        strArrFrmla(4, 0) = "add4"
        strArrFrmla(4, 1) = gSettings.gCompAdd4

        Dim strFormulaFld As String = "", strFormulaVal As String = ""
        strSelFormula = "{RptUDR.UserName} ='" & gUserCode & "' and {RptUDR.RptName} = '" & Trim(txtRptName.Text) & "'"
        Dim frmv As New frmUtlReportViewer
        With frmv
            .StartPosition = FormStartPosition.CenterScreen
            .showRptFile("Printing", strfilename, strSelFormula, strArrFrmla, False, False)
            .Show()
        End With

    End Sub

    Private Sub txtSearch_TextChanged(sender As Object, e As EventArgs) Handles txtSearch.TextChanged
        If Trim(txtSearch.Text) = String.Empty Then
            vsfSearch.SendToBack()
            vsfSearch.Visible = False
        Else
            vsfSearch.BringToFront()
            vsfSearch.Visible = True
        End If

        applyFilterFromGrid()
    End Sub

    Private Function applyFilterFromGrid() As Boolean
        Dim StrQry As String

        applyFilterFromGrid = False
        StrQry = ""
        With vsfSearch
            'If txtSearch.Text = "" Then
            '    Exit Function
            'End If
            Try
                If txtSearch.Text = " " Then
                    dvSearch.RowFilter = "1 = 1"
                    Exit Function
                Else
                    StrQry = StrQry & " ReportName like '%" & txtSearch.Text & "%'" ' Or "
                End If
                dvSearch.RowFilter = StrQry
            Catch ex As Exception

            End Try
        End With
        applyFilterFromGrid = True
    End Function

    Private Sub txtSearch_KeyUp(sender As Object, e As KeyEventArgs) Handles txtSearch.KeyUp
        If e.KeyCode = Keys.Return Then
            Call vsfSearchSelectedItem()
            Exit Sub
        End If

        If e.KeyCode = Keys.Down Then
            vsfSearch.Focus()
            Exit Sub
        End If

        If e.KeyCode = Keys.Enter Then
            If txtSearch.Text = String.Empty Then Exit Sub
            Call OpenRpt()
        End If
    End Sub

    Private Sub vsfSearchSelectedItem()
        Dim strSQL As String

        strSQL = "Select CRID, CRName as ReportName, CRDesc, RptFileName, CondText,"
        strSQL = strSQL & " fq = (select count(Filterreq) from UtlReportsCRDtl where Utlreportscrhdr.CRID = Utlreportscrdtl.crid and Filterreq = 'Y')"
        strSQL = strSQL & " from UtlReportsCRHdr "
        If My.Application.Info.AssemblyName = "PosFA" Then
            strSQL = strSQL & " where ReportGroup = 'FixedAsset'"
        End If
        If gUserCode <> "ADMIN" Then
            strSQL = strSQL & " WHERE CRID IN (SELECT CRID FROM UtlReportsCRRights WHERE USERID =  " & Val(gUserID) & ")"
        End If
        strSQL = strSQL & " order by ReportGroup, CRName"

        dtSearch = clsSVT.getDATATable(strSQL)
        dvSearch = New DataView(dtSearch)
        vsfSearch.DataSource = dvSearch
        Call SetHideCol(vsfSearch, True, 0, 2, 3, 4, 5)
    End Sub

    Private Sub vsfSearch_BeforeEdit(sender As Object, e As RowColEventArgs) Handles vsfSearch.BeforeEdit
        e.Cancel = True
    End Sub
    Private Sub frmRptCustomiseRptView_KeyUp(sender As Object, e As KeyEventArgs) Handles Me.KeyUp
        If e.KeyCode = Keys.F9 And e.Control = True Then
            Call initFormVsfDet(Me, Me.Text, vsfFilter, "FIL")
            Call initFormVsfDet(Me, Me.Text, vsfSelFilters, "SELFIL")
            MsgBox("Restored Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
        If e.KeyCode = Keys.F8 And e.Control = True Then
            Call SaveActFormVsfDet(Me, Me.Text, "FIL", vsfFilter)
            Call SaveActFormVsfDet(Me, Me.Text, "SELFIL", vsfSelFilters)
            MsgBox("Saved Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
    End Sub

    Private Sub vsfSearch_DoubleClick(sender As Object, e As EventArgs) Handles vsfSearch.DoubleClick
        'txtSearch.Text = ""
        Call OpenRpt()
    End Sub

    Private Sub vsfSearch_KeyUp(sender As Object, e As KeyEventArgs) Handles vsfSearch.KeyUp
        If e.KeyCode = Keys.Enter Then
            If Trim(txtSearch.Text) = "" Then Exit Sub
            If vsfSearch.Row < 0 Then Exit Sub
            Call OpenRpt()
            txtSearch.Text = ""
        End If
    End Sub

    Private Sub OpenRpt()
        If Not chkUserRights(strRights, chkRightsPos.ViewRights, Me.Text) Then Exit Sub
        If vsfSearch.Rows.Count < 1 Then Exit Sub
        If vsfSearch.Row < 0 Then Exit Sub
        txtRptName.Text = vsfSearch.GetData(vsfSearch.Row, 1)
        intCrID = vsfSearch.GetData(vsfSearch.Row, 0)
        vsfSearch.Visible = False
        With vsfDet
            For i As Int32 = 0 To .Rows.Count - 1
                If GetVal(.GetData(i, vDetRptID), 0) = intCrID Then
                    .Row = i
                    .Focus()
                    Exit Sub
                End If
            Next
        End With
        txtSearch.Text = ""
        btnViewReport.Focus()
        'Call retreiveData()
    End Sub

    Private Sub txtSearch_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtSearch.KeyPress
        If e.KeyChar = "'" Then
            e.Handled = True
        End If
    End Sub

    Private Sub vsfDet_Click(sender As Object, e As EventArgs) Handles vsfDet.Click
        If vsfDet.Row < 0 Then Exit Sub
        intCrID = vsfDet.GetData(vsfDet.Row, vDetRptID)
        If intCrID <= 0 Then
            Exit Sub
        Else
            If txtDesc.Text = "" Then
                lblDescription.Visible = True
                lblDescription.Text = "Report Description"

            Else
                lblDescription.Visible = False
            End If
        End If
    End Sub

    Private Sub frmRptCustomiseRptView_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        txtSearch.Focus()
    End Sub

    Private Sub vsfDet_KeyUp(sender As Object, e As KeyEventArgs) Handles vsfDet.KeyUp
        If e.KeyCode = Keys.Up Or e.KeyCode = Keys.Down Then
            If vsfDet.Row < 0 Then Exit Sub
            intCrID = vsfDet.GetData(vsfDet.Row, vDetRptID)
            If intCrID <= 0 Then
                Exit Sub
            Else
                If txtDesc.Text = "" Then
                    lblDescription.Visible = True
                    lblDescription.Text = "Report Description"

                Else
                    lblDescription.Visible = False
                End If
            End If
        End If
    End Sub

    Private Sub btnImport_Click(sender As Object, e As EventArgs) Handles btnImport.Click
        Dim dialog As New OpenFileDialog
        Dim strFilename As String = ""
        Dim intCrID As Int32

        If Directory.Exists(Application.StartupPath & "\" & "Temp") Then
            dialog.InitialDirectory = Application.StartupPath & "\" & "Temp"
        End If

        If dialog.ShowDialog() = DialogResult.OK Then
            strFilename = dialog.FileName
        End If
        If Not File.Exists(strFilename) Or strFilename = "" Then
            MsgBox("Please select a valid file for importing", MsgBoxStyle.Critical, "Error")
            Exit Sub
        End If
        With vsfExp
            .Rows.Count = 0
            .Cols.Count = 0
            .Rows.Count = vExpRowTol
            .Cols.Count = vExpColTol
            Try
                .LoadGrid(strFilename, FileFormatEnum.TextTab)
            Catch ex As Exception
                MsgBox("Unable to Import. Please select a valid file", vbCritical, Me.Text)
                Exit Sub
            End Try
        End With

        If UCase(vsfExp.GetData(vExpRowCaption, vExpColQry)) <> UCase("Export UDR report") Then
            MsgBox("File is not suitable for importing. Please select a valid file", vbCritical, Me.Text)
            vsfExp.Rows.Count = 0
            Exit Sub
        End If

        intCrID = clsSVT.getDataLongValue("Select crID from UtlReportsCRHdr where crName = '" & Trim(vsfExp.GetData(vExpRowCRName, vExpColQry)) & "'", 0, False)
        If intCrID > 0 Then
            MsgBox("Report already exists. Cannot import. (Please delete and re-import if required.)", vbInformation, Me.Text)
            Exit Sub
        End If

        Call executeExpQry()
        Call fillReport()
    End Sub

    Private Sub btnEmail_Click(sender As Object, e As EventArgs) Handles btnEmail.Click
        showWaitCursor()
        If Not chkUserRights(strRights, chkRightsPos.ViewRights, Me.Text) Then Exit Sub
        txtRptName.Text = vsfDet.GetData(vsfDet.Row, vDetRptName)
        intCrID = vsfDet.GetData(vsfDet.Row, vDetRptID)
        Dim strXlFileName As String = ""
        Dim strPDFFileName As String = ""
        Dim strSQL As String, intCtr As Integer
        strSQL = getSTRSQL()
        Dim frmCRpt As New frmRptView
        Dim rsDummy As oledb.oledbDataReader
        rsDummy = clsSVT.getDataReaderFromSQL("Select * from UtlreportsCRDtl where CRID = " & intCrID & " order by FldOrderBy")
        With frmCRpt
            .blnDirectExport = True
            .Text = Trim(txtRptName.Text)
            .FillViewVsf(Me, strSQL, Trim(txtRptName.Text), "", argblnSlno:=blnSlnoReq)
            .Show()
            With frmCRpt.vsfDet
                .AllowDragging = AllowDraggingEnum.None
                .AllowEditing = False
                .Tree.Column = 0
            End With
            frmCRpt.chkRemoveSubTot.Visible = False
            .vsfDet.SubtotalPosition = C1.Win.C1FlexGrid.SubtotalPositionEnum.BelowData
            With rsDummy
                intCtr = -1
                If .HasRows Then
                    Do While .Read
                        intCtr = intCtr + 1
                        For i = 0 To frmCRpt.vsfDet.Cols.Count - 1
                            If frmCRpt.vsfDet.GetData(0, i) = !Fldcode Then
                                frmCRpt.vsfDet.SetData(3, i, !Fldcaption)
                                Exit For
                            End If

                            If frmCRpt.vsfDet.GetData(0, i) = "CountOf" & !Fldcode Then
                                frmCRpt.vsfDet.SetData(3, i, !Fldcaption)
                                Exit For
                            End If

                            If frmCRpt.vsfDet.GetData(0, i) = "SumOf" & !Fldcode Then
                                frmCRpt.vsfDet.SetData(3, i, !Fldcaption)
                                Exit For
                            End If
                            If frmCRpt.vsfDet.GetData(0, i) = "AvgOf" & !Fldcode Then
                                frmCRpt.vsfDet.SetData(3, i, !Fldcaption)
                                Exit For
                            End If
                        Next i

                        For i = 0 To frmCRpt.vsfDet.Cols.Count - 1
                            If GetVal(!AddnlCol, "") <> "" Then
                                Dim strWordArr() As String = CStr(!AddnlCol).Split("|")
                                If frmCRpt.vsfDet.GetData(0, i) = UCase(strWordArr(1)) Then
                                    frmCRpt.vsfDet.SetData(3, i, UCase(strWordArr(1)))
                                    Exit For
                                End If
                            End If
                        Next

                        If !TotReq = "Y" Then
                            For i = 0 To frmCRpt.vsfDet.Cols.Count - 1
                                If blnSlnoReq Then
                                    If intCtr + 1 = i Then
                                        frmCRpt.chkRemoveSubTot.Visible = True
                                        frmCRpt.vsfDet.Subtotal(C1.Win.C1FlexGrid.AggregateEnum.Sum, 0, -1, intCtr + 1, "Grand Total")
                                        Call putSubTot(frmCRpt.vsfDet, intCtr + 1)
                                    End If
                                Else
                                    If intCtr = i Then
                                        frmCRpt.chkRemoveSubTot.Visible = True
                                        frmCRpt.vsfDet.Subtotal(C1.Win.C1FlexGrid.AggregateEnum.Sum, 0, -1, intCtr, "Grand Total")
                                        Call putSubTot(frmCRpt.vsfDet, intCtr)
                                    End If

                                End If
                            Next i
                        End If
                    Loop
                End If
                .Close()
            End With
        End With

        With frmCRpt.vsfDet
            .AllowMerging = AllowMergingEnum.RestrictAll
            For i = 0 To .Cols.Count - 2
                .Cols(i).AllowMerging = True
            Next
            '.AutoSizeCols()

            strXlFileName = Application.StartupPath & "\Temp\" & Trim(txtRptName.Text) & "_" & CStr(Format(Date.Now, "yyyyMMddHHmmss")) & ".xlsx"
            strPDFFileName = Application.StartupPath & "\Temp\" & Trim(txtRptName.Text) & "_" & CStr(Format(Date.Now, "yyyyMMddHHmmss")) & ".pdf"
            frmCRpt.ExportToXlPDF(strXlFileName, strPDFFileName)
        End With
        frmCRpt.Close()
        frmCRpt.Dispose()
        Call showWaitCursor()

        Try
            If gSmtpMailID = "" Then
                MsgBox("SMTP Configuration Empty....", MsgBoxStyle.Exclamation, Me.Text)
                Exit Sub
            End If

            If gStrRptEmailID = "" Then
                MsgBox("Please configure email 'TO' Address..", MsgBoxStyle.Exclamation, Me.Text)
                Exit Sub
            End If


            Dim Smtp_Server As New Net.Mail.SmtpClient
            Dim e_mail As New Net.Mail.MailMessage()
            Smtp_Server.UseDefaultCredentials = False
            Smtp_Server.Credentials = New Net.NetworkCredential(CStr(gSmtpMailID & ""), CStr(gSmtpMailPwd & ""))
            Smtp_Server.Port = CStr(gSmtpServerPort & "")
            If CStr(gSmtpEnableSSL & "") = "Y" Then
                Smtp_Server.EnableSsl = True
            Else
                Smtp_Server.EnableSsl = False
            End If
            Smtp_Server.Host = CStr(gSmtpHostName & "")
            e_mail.From = New Net.Mail.MailAddress(CStr(gSmtpMailID & ""))
            For Each str As String In gStrRptEmailID.Split(";")
                e_mail.To.Add(str)
            Next
            e_mail.Subject = Trim(txtRptName.Text)
            e_mail.IsBodyHtml = False
            e_mail.Body = "Automatic Email Pls don't reply....."
            Dim attachment As Net.Mail.Attachment
            attachment = New System.Net.Mail.Attachment(strXlFileName)
            e_mail.Attachments.Add(attachment)
            Smtp_Server.Send(e_mail)
            MsgBox("Email Sent....", MsgBoxStyle.Information, Me.Text)
            showNormalCursor()
        Catch ex As Exception
            showNormalCursor()
        End Try
    End Sub

    Private Sub executeExpQry()
        Dim strQry As String = ""
        Dim intCRID As Int32

        intCRID = clsSVT.getDataLongValue("Select max(crid)+1 from UtlReportsCRHdr", 0, False)

        svtTrn = svtConn.BeginTransaction(IsolationLevel.ReadCommitted)
        Try
            With vsfExp
                strQry = .GetData(vExpRowHdr, vExpColQry)
                strQry = strQry.Replace("<<CRID>>", intCRID)
                If Not clsSVT.execSQLStatement(strQry, svtTrn) Then
                    svtTrn.Rollback()
                    Exit Sub
                End If

                strQry = .GetData(vExpRowDtl, vExpColQry)
                strQry = strQry.Replace("<<CRID>>", intCRID)
                If Not clsSVT.execSQLStatement(strQry, svtTrn) Then
                    svtTrn.Rollback()
                    Exit Sub
                End If

                strQry = .GetData(vExpRowTbl, vExpColQry)
                strQry = strQry.Replace("<<CRID>>", intCRID)
                If Not clsSVT.execSQLStatement(strQry, svtTrn) Then
                    svtTrn.Rollback()
                    Exit Sub
                End If
            End With
            svtTrn.Commit()
            MsgBox("Import successful", vbInformation, Me.Text)

        Catch ex As Exception
            svtTrn.Rollback()
            MsgBox(ex.Message, MsgBoxStyle.Information, Me.Text)
        End Try
    End Sub
End Class