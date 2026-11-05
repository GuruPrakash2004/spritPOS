Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports System.IO
Imports System.Drawing.Printing
Imports Microsoft.Office

Public Class frmUtlReportViewer
    Dim clsSVT As New svtLibrary
    Dim strRptFileName As String
    Dim strDocNo As String

    Dim strSHForm As String
    Dim strDocHdrTblName As String
    Dim strDocDtlTblName As String
    Dim strDocIDFldName As String
    Dim strDocNoFldName As String
    Dim strDocDateFldName As String

    Dim intPartyDocID As Int32
    Dim strPartyType As String
    Dim strPartyTblName As String
    Dim strFldPartyID As String
    Dim strFldPartyCode As String
    Dim strFldPartyName As String

    Public Function ShowRptFile(ByVal argRptName As String, argRptFileName As String,
                                argFormula As String, argFormulaVal As String,
                                 Optional ByVal argSelection As String = "", Optional ByVal argPrinterName As String = "",
                            Optional ByVal argPrintCopies As Integer = 1, Optional ByVal argBlnPDF As Boolean = False, ByVal Optional argPDFFileName As String = "") As Boolean
        ShowRptFile = True
        Dim iPos As Integer, i As Integer
        Dim strFormula As String, strTable As String, strFld As String
        Dim arrFormulaFld() As String, arrFormulaVal() As String
        Dim connectionLoginInfo As New TableLogOnInfo
        Dim cryRpt As New ReportDocument

        strRptFileName = gstrRptPath & argRptFileName

        With cryRpt
            Try
                .Load(strRptFileName)
            Catch ex As Exception
                MsgBox("Please ensure " & strRptFileName & " Exists!" & vbCrLf & ex.Message, MsgBoxStyle.Critical, Me.Text)
                Exit Function
            End Try

            connectionLoginInfo.ConnectionInfo.UserID = gStrDBLoginName
            connectionLoginInfo.ConnectionInfo.Password = gStrDBLoginPwd
            connectionLoginInfo.ConnectionInfo.ServerName = gStrServerName
            connectionLoginInfo.ConnectionInfo.DatabaseName = gStrDBName

            For Each reportTable In .Database.Tables
                reportTable.ApplyLogOnInfo(connectionLoginInfo)
            Next

            If argSelection <> "" Then
                .RecordSelectionFormula = argSelection
            End If
            ' strDocNo = strSelFormula
            'Try
            If argFormula <> "" Then
                arrFormulaFld = Split(argFormula, ";")
                arrFormulaVal = Split(argFormulaVal, ";")
                For i = 0 To arrFormulaFld.Length - 1
                    strFormula = Replace(Trim(arrFormulaFld(i)), "`", "'")
                    If InStr(1, strFormula, "=") = 0 Then
                        strFormula = strFormula & " ="
                    End If
                    iPos = InStr(1, arrFormulaVal(i), ".")
                    If iPos > 0 Then
                        strTable = Strings.Left(Trim(arrFormulaVal(i)), iPos - 1)
                        strFld = Mid(Trim(arrFormulaVal(i)), iPos + 1, Len(arrFormulaVal(i)))

                        strFormula = clsSVT.getDataStringValue("Select " & strFld & " from " & strTable, "", False)
                    Else
                        strFormula = arrFormulaVal(i)
                    End If
                    .SetParameterValue(arrFormulaFld(i), strFormula)
                Next i
            End If


            With crptViewr
                .ToolPanelView = CrystalDecisions.Windows.Forms.ToolPanelViewType.None
                .RefreshReport()
                .ReportSource = cryRpt
                If argPrinterName = "" Then
                    cryRpt.PrintOptions.PrinterName = gStrDefPrinter
                    cryRpt.PrintOptions.PaperSize = CType(gstrdefprinterPaperSize.RawKind, CrystalDecisions.Shared.PaperSize)
                End If
                .Refresh()

                If argPrinterName <> "" Then
                    Dim ps As New System.Drawing.Printing.PrinterSettings
                    Dim pg As New System.Drawing.Printing.PageSettings
                    Dim pgSize As New System.Drawing.Printing.PaperSize("Letter", 210, 297)

                    pgSize.PaperName = PaperKind.Letter
                    pg.PaperSize = pgSize
                    ps.PrinterName = argPrinterName
                    ps.Copies = argPrintCopies
                    If argRptFileName.ToUpper = "FMTSINV.RPT" Then
                        Dim pd As New System.Drawing.Printing.PaperSize()
                        Dim pds As New System.Drawing.Printing.PageSettings
                        pd.PaperName = PaperKind.Custom
                        pds.PaperSize = pd
                        cryRpt.PrintToPrinter(ps, pds, 0)
                        cryRpt.Close()
                        cryRpt.Dispose()
                        GC.Collect()
                    Else
                        cryRpt.PrintToPrinter(ps, pg, 0)
                        cryRpt.Close()
                        cryRpt.Dispose()
                        GC.Collect()
                    End If

                End If

                If argBlnPDF AndAlso argPDFFileName <> "" Then
                    Try
                        Dim strAttachFileName As String = argPDFFileName
                        strAttachFileName = Application.StartupPath & "\Temp\" & strAttachFileName & ".pdf"
                        If File.Exists(strAttachFileName) Then
                            File.Delete(strAttachFileName)
                        End If
                        Dim pdfCryRpt As New ReportDocument
                        pdfCryRpt = cryRpt
                        Try
                            pdfCryRpt.ExportToDisk(ExportFormatType.PortableDocFormat, strAttachFileName)
                        Catch ex As Exception
                            MsgBox(ex.Message & vbCrLf & "Error during Exporting to PDF", MsgBoxStyle.Critical, "ERROR")
                        End Try
                        pdfCryRpt.Close()
                        pdfCryRpt.Dispose()
                        GC.Collect()
                    Catch ex As Exception
                    End Try
                End If
            End With
        End With

        Return True
    End Function


    Public Sub showRptFile(ByVal argCaption As String,
                            ByVal argRptName As String,
                            ByVal argSelFormula As String,
                            ByVal argFormulas(,) As String,
                            ByVal blnSendMail As Boolean,
                            Optional ByVal blnShowPrintButton As Boolean = True,
                            Optional ByVal blnShowExportButton As Boolean = False,
                            Optional ByVal argPrinterName As String = "",
                            Optional ByVal argPrintCopies As Integer = 1,
                            Optional ByVal blnExportToPDF As Boolean = False,
                            Optional ByVal strPdfPath As String = "")

        Dim connectionLoginInfo As New TableLogOnInfo
        Dim index As Integer = 0
        Dim intCounter As Integer = 0
        Me.Text = argCaption & "    [File:" & Path.GetFileName(argRptName) & "]"
        strRptFileName = argRptName
        strDocNo = argSelFormula

        Dim cryRpt As New ReportDocument, i As Integer
        'Sub report object of crystal report.
        Dim SubRptO As CrystalDecisions.CrystalReports.Engine.SubreportObject

        'Sub report document of crystal report.
        Dim SubRptD As New CrystalDecisions.CrystalReports.Engine.ReportDocument

        If Not File.Exists(gstrRptPath & argRptName) Then
            MsgBox(argRptName & " is not present!", MsgBoxStyle.Critical, argCaption)
            Exit Sub
        End If

        Try
            cryRpt.Load(gstrRptPath & argRptName)
        Catch ex As Exception
            MsgBox("Error in Loading Report " & argRptName, MsgBoxStyle.Critical, Me.Text)
            End Try

            connectionLoginInfo.ConnectionInfo.UserID = gStrDBLoginName
            connectionLoginInfo.ConnectionInfo.Password = gStrDBLoginPwd
            connectionLoginInfo.ConnectionInfo.ServerName = gStrServerName
            connectionLoginInfo.ConnectionInfo.DatabaseName = gStrDBName
            Try
                For Each reportTable In cryRpt.Database.Tables
                    reportTable.ApplyLogOnInfo(connectionLoginInfo)
                Next
            Catch ex As Exception
                MsgBox("Error during applying Login settings." & vbCrLf & ex.Message, MsgBoxStyle.Critical, "Printing")
                Exit Sub
            End Try

            'Loop through each section on the report then look through each object
            ' in the section if the object is a subreport, then apply logon info 
            ' on each table of that sub report
            For index = 0 To cryRpt.ReportDefinition.Sections.Count - 1
                For intCounter = 0 To cryRpt.ReportDefinition.Sections(index).ReportObjects.Count - 1
                    With cryRpt.ReportDefinition.Sections(index)
                        If .ReportObjects(intCounter).Kind = CrystalDecisions.Shared.ReportObjectKind.SubreportObject Then
                            SubRptO = CType(.ReportObjects(intCounter), CrystalDecisions.CrystalReports.Engine.SubreportObject)
                            SubRptD = SubRptO.OpenSubreport(SubRptO.SubreportName)
                            For intCounter1 = 0 To SubRptD.Database.Tables.Count - 1
                                SubRptD.Database.Tables(intCounter1).ApplyLogOnInfo(connectionLoginInfo)
                            Next
                        End If
                    End With
                Next
            Next

            'cryRpt.SetDatabaseLogon(gStrDBLoginName, gStrDBLoginPwd, gStrServerName, gStrDBName, True)
            cryRpt.RecordSelectionFormula = argSelFormula

        Try
            With crptViewr
                '.ToolPanelView = CrystalDecisions.Windows.Forms.ToolPanelViewType.None
                .RefreshReport()
                .ReportSource = cryRpt
                If Not (argFormulas Is Nothing) Then
                    For i = 0 To argFormulas.GetLength(0) - 1
                        cryRpt.SetParameterValue(argFormulas(i, 0), argFormulas(i, 1))
                    Next i
                End If

                If blnExportToPDF Then
                    cryRpt.ExportToDisk(ExportFormatType.PortableDocFormat, strPdfPath)
                End If

                If argPrinterName <> "" Then
                    Dim ps As New System.Drawing.Printing.PrinterSettings
                    Dim pd As New System.Drawing.Printing.PaperSize()
                    Dim pds As New System.Drawing.Printing.PageSettings
                    pd.PaperName = PaperKind.Custom
                    pds.PaperSize = pd
                    ps.PrinterName = argPrinterName
                    ps.Copies = argPrintCopies
                    cryRpt.PrintToPrinter(ps, pds, 0)
                    cryRpt.Close()
                    cryRpt.Dispose()
                    GC.Collect()
                    'For ix As Integer = 0 To System.Drawing.Printing.PrinterSettings.PaperSizes.Count - 1
                    '    If PrintDocument1.PrinterSettings.PaperSizes(ix).Kind = PaperKind.A4 Then
                    '        ps = PrintDocument1.PrinterSettings.PaperSizes(ix)
                    '        PrintDocument1.DefaultPageSettings.PaperSize = ps
                    '        PageSetupDialog1.PageSettings.PaperSize = ps
                    '    End If
                    'Next
                Else
                    cryRpt.PrintOptions.PrinterName = gStrDefPrinter
                    cryRpt.PrintOptions.PaperSize = CType(gstrdefprinterPaperSize.RawKind, CrystalDecisions.Shared.PaperSize)
                    '.Show()
                    'cryRpt.PrintOptions.PrinterName = "Invoice"
                End If
            End With

        Catch ex As Exception
            MsgBox("Error during Printing." & vbCrLf & ex.Message, MsgBoxStyle.Critical, "Printing")
            Exit Sub
        End Try
    End Sub

    Public Function ShowRptFileFromUtlOptions(ByVal argTrnDocID As Integer, ByVal argDocIdValue As Long, ByVal argReportOptionName As String,
                                              ByVal blnPreview As Boolean, Optional blnUsedForPDF As Boolean = False, Optional argPrintPDF As Boolean = False, Optional argPrintPDFName As String = "") As Boolean
        Dim iPos As Integer, i As Integer
        Dim strSelFormula As String, strAdnlFormula As String
        Dim Strfld As String, StrTable As String
        Dim strFormula As String
        Dim arrFormulaFld() As String, arrFormulaVal() As String, strCrystalReportFileName As String
        Dim strMultiRpt As String
        Dim strSQL As String
        Dim connectionLoginInfo As New TableLogOnInfo
        ShowRptFileFromUtlOptions = False
        Dim recDummy As oledb.oledbDataReader
        Dim intNoOfCopies As Integer
        Dim cryRpt As New ReportDocument

        strSQL = " select top 1 * from UTLOption where DocID = " & argTrnDocID
        If argReportOptionName <> "" Then strSQL = strSQL & " and OptionName = '" & argReportOptionName & "'"
        strSQL = strSQL & " order by optionorder"

        recDummy = clsSVT.getDataReaderFromSQL(strSQL)

        If Not recDummy.HasRows Then Exit Function
        recDummy.Read()
        intNoOfCopies = recDummy("NoOfCopies")

        If GetVal(recDummy!MultiRpt, "N") = "N" Then
            strCrystalReportFileName = gstrRptPath & GetVal(recDummy!CrptFileName, "")
            strRptFileName = gstrRptPath & GetVal(recDummy!CrptFileName, "")
        Else
            strMultiRpt = GetVal(recDummy!MultiRptString1, "") _
                        & GetVal(recDummy!MultiRptString2, "") _
                        & GetVal(recDummy!MultiRptString3, "")
            strCrystalReportFileName = gstrRptPath & getRptName(strMultiRpt, argTrnDocID, argDocIdValue)
            strRptFileName = gstrRptPath & GetVal(recDummy!CrptFileName, "")
        End If

        If InStr(1, UCase(Trim(strCrystalReportFileName)), ".RPT") = 0 Then
            strCrystalReportFileName = strCrystalReportFileName & ".Rpt"
        End If

        'Get the selection formula
        strSelFormula = Trim(Replace(Trim(GetVal(recDummy!SelFormula, "")), "`", "'"))
        If strSelFormula <> "" Then
            strSelFormula = "" & strSelFormula & " " & argDocIdValue
        End If

        'Get the addnl selection formula
        strAdnlFormula = Replace(Trim(GetVal(recDummy!AddnlSelFormula, "")), "`", "'")
        'strSelFormula = Trim(strSelFormula & " " & strAdnlFormula)
        'strSelFormula = strSelFormula.Replace("<<COMPID>>", gIntCompID)

        With cryRpt
            Try
                .Load(strCrystalReportFileName)
            Catch ex As Exception
                MsgBox("Please ensure " & strCrystalReportFileName & " Exists!" & vbCrLf & ex.Message, MsgBoxStyle.Critical, Me.Text)
                Exit Function
            End Try

            connectionLoginInfo.ConnectionInfo.UserID = gStrDBLoginName
            connectionLoginInfo.ConnectionInfo.Password = gStrDBLoginPwd
            connectionLoginInfo.ConnectionInfo.ServerName = gStrServerName
            connectionLoginInfo.ConnectionInfo.DatabaseName = gStrDBName

            For Each reportTable In .Database.Tables
                reportTable.ApplyLogOnInfo(connectionLoginInfo)
            Next

            '.SetDatabaseLogon(gStrDBLoginName, gStrDBLoginPwd, gStrServerName, gStrDBName, True)
            .RecordSelectionFormula = strSelFormula
            strDocNo = strSelFormula
            'Try
            If recDummy!Formulasfld <> "" Then
                arrFormulaFld = Split(GetVal(recDummy!FormulasFld, ""), ";")
                arrFormulaVal = Split(GetVal(recDummy!FormulasVal, ""), ";")

                For i = 0 To arrFormulaFld.Length - 1
                    strFormula = Replace(Trim(arrFormulaFld(i)), "`", "'")
                    If InStr(1, strFormula, "=") = 0 Then
                        strFormula = strFormula & " ="
                    End If
                    iPos = InStr(1, arrFormulaVal(i), ".")
                    StrTable = Strings.Left(Trim(arrFormulaVal(i)), iPos - 1)
                    Strfld = Mid(Trim(arrFormulaVal(i)), iPos + 1, Len(arrFormulaVal(i)))

                    strFormula = clsSVT.getDataStringValue("Select " & Strfld & " from " & StrTable, "", False)
                    .SetParameterValue(arrFormulaFld(i), strFormula)
                Next i
            End If

            'Catch ex As Exception
            'MsgBox("Please check Formula in Designer!" & vbCrLf & ex.Message, MsgBoxStyle.Critical, Me.Text)
            'End Try

            If Not blnPreview Then .PrintToPrinter(1, False, 0, 0)
        End With
        If blnUsedForPDF Then
            cryRpt.SetParameterValue("mPDF", "")
        End If

        If argPrintPDF Then
            Dim pdfCryRpt As New ReportDocument
            pdfCryRpt = cryRpt
            pdfCryRpt.SetParameterValue("mPDF", "Y")
            pdfCryRpt.RecordSelectionFormula = pdfCryRpt.RecordSelectionFormula & " " & strAdnlFormula
            Try
                pdfCryRpt.ExportToDisk(ExportFormatType.PortableDocFormat, argPrintPDFName)
            Catch ex As Exception
                MsgBox(ex.Message & vbCrLf & "Error during Exporting to PDF", MsgBoxStyle.Critical, "ERROR")
            End Try
            ShowRptFileFromUtlOptions = True
            Me.Close()
        Else
            With crptViewr
                .ToolPanelView = CrystalDecisions.Windows.Forms.ToolPanelViewType.None
                .RefreshReport()
                .ReportSource = cryRpt
                .Refresh()
            End With
        End If
        ShowRptFileFromUtlOptions = True
        Exit Function
    End Function

    Private Function getRptName(ByVal strMultiRpt As String, ByVal argTrnDocID As Integer, ByVal argDocIdValue As Long) As String
        Dim arrMRRow() As String, arrMRCols() As String, i As Integer, j As Integer
        Dim strFldName As String, strFldValue As String, strTblName As String
        Dim blnFound As Boolean
        Dim strshform As String

        Const vMRRptName = 1
        Const vMRFldName = 2
        Const vMROperator = 3
        Const vMRValue = 4

        getRptName = ""
        If strMultiRpt = "" Then Exit Function
        arrMRRow = Split(strMultiRpt, "ç")
        Dim arrTemp(UBound(arrMRRow) - 1, 5)
        For i = 0 To UBound(arrMRRow) - 1
            arrMRCols = Split(arrMRRow(i), "å")
            For j = 0 To UBound(arrMRCols) - 1
                arrTemp(i, j) = arrMRCols(j)
            Next j
        Next i
        For i = 0 To UBound(arrMRRow) - 1
            strFldName = Replace(arrTemp(i, vMRFldName), "#", "")
            strFldName = Replace(strFldName, "h.", "")
            strFldName = Replace(strFldName, "d.", "")
            strshform = clsSVT.getDataStringValue("Select DocShForm from UtlTransactions where DocId = " & argTrnDocID, "", False)
            If InStr(1, arrTemp(i, vMRFldName), "#h.") > 0 Then
                strTblName = "TrnHdr" & strshform
            Else
                strTblName = "TrnDtl" & strshform
            End If
            strFldValue = ""
            Select Case getDataType(strTblName, strFldName)
                Case "TEXT"
                    strFldValue = "'" & arrTemp(i, vMRValue) & "'"
                Case "DATE"
                    strFldValue = "'" & arrTemp(i, vMRValue) & "'"
                Case "NUMERIC"
                    strFldValue = arrTemp(i, vMRValue)
            End Select
            Call clsSVT.getDataStringValue("Select * from " & strTblName & "  Where " & strshform & "ID = " & argDocIdValue & " and " & strFldName _
                    & arrTemp(i, vMROperator) & strFldValue, "", blnFound)
            If blnFound Then Exit For
        Next i
        If i < UBound(arrMRRow) Then
            getRptName = arrTemp(i, vMRRptName)
        Else
            MsgBox("Combination not found, cannot open report", vbOKOnly, "Printing Error")
        End If
    End Function

    Private Sub frmUtlReportViewer_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Public Function ShowRptFileFromUtlOptions_New(ByVal argTrnDocID As Integer,
                                                ByVal argDocIdValue As Long,
                                                ByVal argReportOptionName As String,
                                                ByVal argBlnCtrlClick As Boolean, ByVal Optional argFileName As String = "") As Boolean
        Dim iPos As Integer, i As Integer
        Dim strSelFormula As String, strAdnlFormula As String
        Dim Strfld As String, StrTable As String
        Dim strFormula As String
        Dim arrFormulaFld() As String, arrFormulaVal() As String, strCrystalReportFileName As String
        Dim strSQL As String
        Dim connectionLoginInfo As New TableLogOnInfo
        ShowRptFileFromUtlOptions_New = False
        Dim recDummy As oledb.oledbDataReader
        Dim intNoOfCopies As Integer
        Dim cryRpt As New ReportDocument
        Dim strClickEvent As String

        strSQL = " select top 1 * from UTLOption where DocID = " & argTrnDocID
        If argReportOptionName <> "" Then strSQL = strSQL & " and OptionName = '" & argReportOptionName & "'"
        strSQL = strSQL & " order by optionorder"

        recDummy = clsSVT.getDataReaderFromSQL(strSQL)

        If Not recDummy.HasRows Then Exit Function
        recDummy.Read()
        intNoOfCopies = recDummy("NoOfCopies")

        strCrystalReportFileName = gstrRptPath & GetVal(recDummy!CrptFileName, "")
        strRptFileName = gstrRptPath & GetVal(recDummy!CrptFileName, "")

        If argBlnCtrlClick Then
            strClickEvent = GetVal(recDummy.Item("CtrlClickOperation"), "")
        Else
            strClickEvent = GetVal(recDummy.Item("ClickOperation"), "")
        End If

        If strClickEvent = "" Then strClickEvent = "P"

        If InStr(1, UCase(Trim(strCrystalReportFileName)), ".RPT") = 0 Then
            strCrystalReportFileName = strCrystalReportFileName & ".Rpt"
        End If

        'Get the selection formula
        strSelFormula = Trim(Replace(Trim(GetVal(recDummy!SelFormula, "")), "`", "'"))
        If strSelFormula <> "" Then
            strSelFormula = "" & strSelFormula & " " & argDocIdValue
        End If

        'Get the addnl selection formula
        strAdnlFormula = Replace(Trim(GetVal(recDummy!AddnlSelFormula, "")), "`", "'")

        With cryRpt
            Try
                .Load(strCrystalReportFileName)
            Catch ex As Exception
                MsgBox("Please ensure " & strCrystalReportFileName & " Exists!" & vbCrLf & ex.Message, MsgBoxStyle.Critical, Me.Text)
                Exit Function
            End Try

            connectionLoginInfo.ConnectionInfo.UserID = gStrDBLoginName
            connectionLoginInfo.ConnectionInfo.Password = gStrDBLoginPwd
            connectionLoginInfo.ConnectionInfo.ServerName = gStrServerName
            connectionLoginInfo.ConnectionInfo.DatabaseName = gStrDBName

            For Each reportTable In .Database.Tables
                reportTable.ApplyLogOnInfo(connectionLoginInfo)
            Next

            .RecordSelectionFormula = strSelFormula
            strDocNo = strSelFormula
            'Try
            If recDummy!Formulasfld <> "" Then
                arrFormulaFld = Split(GetVal(recDummy!FormulasFld, ""), ";")
                arrFormulaVal = Split(GetVal(recDummy!FormulasVal, ""), ";")

                For i = 0 To arrFormulaFld.Length - 1
                    strFormula = Replace(Trim(arrFormulaFld(i)), "`", "'")
                    If InStr(1, strFormula, "=") = 0 Then
                        strFormula = strFormula & " ="
                    End If
                    iPos = InStr(1, arrFormulaVal(i), ".")
                    StrTable = Strings.Left(Trim(arrFormulaVal(i)), iPos - 1)
                    Strfld = Mid(Trim(arrFormulaVal(i)), iPos + 1, Len(arrFormulaVal(i)))

                    strFormula = clsSVT.getDataStringValue("Select " & Strfld & " from " & StrTable, "", False)
                    .SetParameterValue(arrFormulaFld(i), strFormula)
                Next i
            End If

            'Catch ex As Exception
            'MsgBox("Please check Formula in Designer!" & vbCrLf & ex.Message, MsgBoxStyle.Critical, Me.Text)
            'End Try

            'If Not blnPreview Then .PrintToPrinter(1, False, 0, 0)
            Select Case strClickEvent
                Case "P"
                    With crptViewr
                        .ToolPanelView = CrystalDecisions.Windows.Forms.ToolPanelViewType.None
                        .RefreshReport()
                        .ReportSource = cryRpt
                        .Refresh()
                    End With
                    Exit Function
                Case "D"
                    Dim ps As New System.Drawing.Printing.PrinterSettings
                    Dim pg As New System.Drawing.Printing.PageSettings
                    Dim pgSize As New System.Drawing.Printing.PaperSize("Letter", 210, 297)
                    ps.PrinterName = Trim(GetVal(recDummy.Item("DirectPrinterName"), ""))
                    ps.Copies = intNoOfCopies
                    .PrintToPrinter(ps, pg, 0)
                    .Close()
                    .Dispose()
                    GC.Collect()
                    Exit Function
                Case "E"
                    Call getUtlDocDtls(argTrnDocID)
                    Dim strSubject As String, strBody As String, strFooter As String, strToEmail As String, strCcEmail As String, strBccEmail As String
                    Dim intOptionOrder As Int32 = clsSVT.getDataLongValue("Select OptionOrder From UtlOption Where DocID = " & argTrnDocID & " and OptionName = '" & argReportOptionName & "'", 0, False)

                    strSubject = ReplaceEmailStrings(GetVal(recDummy!EmailSubject, ""), argDocIdValue, GetVal(recDummy!EmailCond, ""))
                    strBody = ReplaceEmailStrings(GetVal(recDummy!EmailBody, ""), argDocIdValue, GetVal(recDummy!EmailCond, ""))
                    strFooter = ReplaceEmailStrings(GetVal(recDummy!EmailFooter, ""), argDocIdValue, GetVal(recDummy!EmailCond, ""))

                    strToEmail = getEmailDtls(argTrnDocID, intOptionOrder, "T", argDocIdValue)
                    strCcEmail = getEmailDtls(argTrnDocID, intOptionOrder, "C", argDocIdValue)
                    strBccEmail = getEmailDtls(argTrnDocID, intOptionOrder, "B", argDocIdValue)

                    Try
                        Dim strAttachFileName As String = ""
                        If argFileName <> "" Then
                            strAttachFileName = argFileName
                        Else
                            strAttachFileName = Date.Now.ToString("yyyyMMddHHmmssfff")
                        End If
                        strAttachFileName = Application.StartupPath & "\Temp\" & strAttachFileName & ".pdf"
                        If GetVal(recDummy!DoNotAttach, "") = "N" Then
                            If File.Exists(strAttachFileName) Then
                                File.Delete(strAttachFileName)
                            End If
                            Dim pdfCryRpt As New ReportDocument
                            pdfCryRpt = cryRpt
                            Try
                                pdfCryRpt.ExportToDisk(ExportFormatType.PortableDocFormat, strAttachFileName)
                            Catch ex As Exception
                                MsgBox(ex.Message & vbCrLf & "Error during Exporting to PDF", MsgBoxStyle.Critical, "ERROR")
                            End Try
                        End If

                        If GetVal(recDummy!EmailRouting, "") = "O" Then
                            Dim oApp As Interop.Outlook._Application
                            oApp = New Interop.Outlook.Application

                            Dim oMsg As Interop.Outlook._MailItem
                            oMsg = oApp.CreateItem(Interop.Outlook.OlItemType.olMailItem)

                            oMsg.Subject = strSubject
                            oMsg.Body = strBody & vbNewLine & strFooter
                            If strToEmail <> "" Then
                                oMsg.To = strToEmail
                            End If
                            If strCcEmail <> "" Then
                                oMsg.CC = strCcEmail
                            End If
                            If strBccEmail <> "" Then
                                oMsg.BCC = strBccEmail
                            End If
                            If GetVal(recDummy!DoNotAttach, "") = "N" Then
                                oMsg.Attachments.Add(strAttachFileName)
                            End If
                            Dim strMailName As String = Application.StartupPath & "\Temp\" & Date.Now.ToString("yyyyMMddHHmmssfff") & ".msg"
                            If File.Exists(strMailName) Then
                                File.Delete(strMailName)
                            End If
                            oMsg.SaveAs(strMailName)
                            oMsg.Close(Microsoft.Office.Interop.Outlook.OlInspectorClose.olSave)
                            Process.Start(strMailName)
                        Else
                            Try
                                If gSmtpMailID = "" Then
                                    MsgBox("SMTP Configuration Empty....", MsgBoxStyle.Exclamation, Me.Text)
                                    Exit Function
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
                                If strToEmail <> "" Then e_mail.To.Add(strToEmail)
                                If strCcEmail <> "" Then e_mail.CC.Add(strCcEmail)
                                If strBccEmail <> "" Then e_mail.Bcc.Add(strBccEmail)
                                e_mail.Subject = strSubject
                                e_mail.IsBodyHtml = False
                                e_mail.Body = strBody & vbNewLine & strFooter
                                If GetVal(recDummy!DoNotAttach, "") = "N" Then
                                    Dim attachment As Net.Mail.Attachment
                                    attachment = New System.Net.Mail.Attachment(strAttachFileName)
                                    e_mail.Attachments.Add(attachment)
                                End If
                                Smtp_Server.Send(e_mail)
                                MsgBox("Email Sent....", MsgBoxStyle.Information, Me.Text)
                            Catch ex As Exception
                                MsgBox("Error in SMTP eMail!" & vbCrLf & ex.Message, MsgBoxStyle.Critical, "ERROR")
                            End Try
                        End If
                    Catch ex As Exception
                        MsgBox("Error in Sending Mail!" & vbCrLf & ex.Message, MsgBoxStyle.Critical, "ERROR")
                    End Try
                    Exit Function
                Case Else
                    Exit Function
            End Select


        End With
        ShowRptFileFromUtlOptions_New = True
        Exit Function
    End Function

    Private Sub getUtlDocDtls(ByVal argDOCID As Int32)
        Dim rsDummy As oledb.oledbDataReader = clsSVT.getDataReaderFromSQL("Select * from UtlTransactions where DocID = " & argDOCID)

        With rsDummy
            If .HasRows Then
                .Read()
                strSHForm = GetVal(!DocShForm, "")
                strDocNoFldName = strSHForm & "No"
                strDocIDFldName = strSHForm & "ID"
                strDocDateFldName = strSHForm & "Date"
                strDocHdrTblName = "TrnHdr" & strSHForm
                strDocDtlTblName = "TrnDtl" & strSHForm
                intPartyDocID = GetVal(!MstDocIDParty, 0)
            End If
        End With
        rsDummy.Close()

        strPartyType = clsSVT.getDataStringValue("Select DocShForm From UtlTransactions where DocID = " & Val(intPartyDocID), "", False)
        strPartyTblName = "Mst" & strPartyType
        strFldPartyID = strPartyType & "ID"
        strFldPartyCode = strPartyType & "Code"
        strFldPartyName = strPartyType & "Name"
    End Sub

    Private Function ReplaceEmailStrings(ByVal argStr As String, ByVal argTrnID As Int32, ByVal argCond As String) As String
        ReplaceEmailStrings = ""
        If InStr(argStr, "<<") < 0 Then
            Return argStr
        End If

        Dim strQry As String, strReturn As String = argStr
        strQry = "Select " & strDocNoFldName & ", " & strDocDateFldName
        If strPartyType <> "" Then
            strQry &= "," & strFldPartyCode & ", " & strFldPartyName & " "
        End If
        strQry &= " From " & strDocHdrTblName
        If strPartyType <> "" Then
            strQry &= "," & strPartyTblName
        End If

        strQry &= " Where 1=1 "
        If strPartyType <> "" Then
            strQry &= " and " & strDocHdrTblName & "." & strFldPartyID & "=" & strPartyTblName & "." & strFldPartyID
        End If
        strQry &= " and " & strDocHdrTblName & "." & strDocIDFldName & " = " & argTrnID
        strQry &= argCond

        Dim rs As oledb.oledbDataReader = clsSVT.getDataReaderFromSQL(strQry)
        If rs.HasRows Then
            rs.Read()
            strReturn = strReturn.Replace("<<" & UCase(strDocIDFldName) & ">>", argTrnID)
            strReturn = strReturn.Replace("<<" & UCase(strDocNoFldName) & ">>", GetVal(rs.Item(strDocNoFldName), ""))
            strReturn = strReturn.Replace("<<" & UCase(strDocDateFldName) & ">>", GetVal(rs.Item(strDocDateFldName), ""))
            strReturn = strReturn.Replace("<<" & UCase(strFldPartyCode) & ">>", GetVal(rs.Item(strFldPartyCode), ""))
            strReturn = strReturn.Replace("<<" & UCase(strFldPartyName) & ">>", GetVal(rs.Item(strFldPartyName), ""))
        End If
        rs.Close()
        Return strReturn
    End Function

    Private Function getEmailDtls(ByVal argDocID As Int32, ByVal argOrder As Int32, ByVal argEmailType As String, ByVal argTrnID As Int32) As String
        getEmailDtls = ""

        Dim rs As oledb.oledbDataReader
        rs = clsSVT.getDataReaderFromSQL("Select * from UtlOptionEmailDtls Where DocID=" & argDocID & " and OptionOrder=" & argOrder & " and emailType='" & argEmailType & "' Order by Slno")

        Dim strEmailID As String = ""
        If rs.HasRows Then
            Do While rs.Read
                If GetVal(rs.Item("FixedEmail"), "") = "Y" Then
                    strEmailID &= GetVal(rs.Item("EmailID"), "") & ";"
                Else

                    Dim strQry As String
                    strQry = "Select "
                    strQry &= GetVal(rs.Item("TxnFldName"), "")
                    strQry &= " From " & strDocHdrTblName
                    If strDocDtlTblName <> "" Then
                        strQry &= "," & strDocDtlTblName
                    End If
                    If strPartyType <> "" Then
                        strQry &= "," & strPartyTblName
                    End If

                    strQry &= " Where 1=1 "
                    If strDocDtlTblName <> "" Then
                        'strQry &= " and " & strDocHdrTblName & "." & strDocIDFldName & "=" & strDocHdrTblName & "." & strDocIDFldName
                        strQry &= " and " & strDocHdrTblName & "." & strDocIDFldName & "=" & strDocDtlTblName & "." & strDocIDFldName
                    End If
                    strQry &= " and " & " " & GetVal(rs.Item("TxnCondition"), "")

                    strQry = strQry.Replace("<<" & UCase(strDocIDFldName) & ">>", argTrnID)
                    strQry = strQry.Replace("<<" & UCase(strDocHdrTblName) & "." & UCase(strDocIDFldName) & ">>", argTrnID)
                    strQry = strQry.Replace("`", "'")
                    strEmailID = clsSVT.getDataStringValue(strQry, "", False) & ";"
                End If
            Loop
        End If
        rs.Close()
        If strEmailID <> "" Then
            strEmailID = Mid(strEmailID, 1, Len(strEmailID) - 1)
        End If
        Return strEmailID
    End Function

    Public Function ShowRptFile_MultiPrint(ByVal argTrnDocID As Integer,
                                               ByVal argReportOptionName As String, ByVal argDocIdValue As String) As Boolean
        Dim iPos As Integer, i As Integer
        Dim strSelFormula As String, strAdnlFormula As String
        Dim Strfld As String, StrTable As String
        Dim strFormula As String
        Dim arrFormulaFld() As String, arrFormulaVal() As String, strCrystalReportFileName As String
        Dim strSQL As String
        Dim connectionLoginInfo As New TableLogOnInfo
        ShowRptFile_MultiPrint = False
        Dim recDummy As oledb.oledbDataReader
        Dim intNoOfCopies As Integer
        Dim cryRpt As New ReportDocument


        strSQL = " select top 1 * from UTLOption where DocID = " & argTrnDocID
        If argReportOptionName <> "" Then strSQL = strSQL & " and OptionName = '" & argReportOptionName & "'"
        strSQL = strSQL & " order by optionorder"

        recDummy = clsSVT.getDataReaderFromSQL(strSQL)

        If Not recDummy.HasRows Then Exit Function
        recDummy.Read()
        intNoOfCopies = recDummy("NoOfCopies")

        strCrystalReportFileName = gstrRptPath & GetVal(recDummy!CrptFileName, "")
        strRptFileName = gstrRptPath & GetVal(recDummy!CrptFileName, "")

        If InStr(1, UCase(Trim(strCrystalReportFileName)), ".RPT") = 0 Then
            strCrystalReportFileName = strCrystalReportFileName & ".Rpt"
        End If

        'Get the selection formula
        strSelFormula = Trim(Replace(Trim(GetVal(recDummy!SelFormula, "")), "`", "'"))
        If strSelFormula <> "" Then
            strSelFormula = "" & strSelFormula.Replace("=", " in [") & argDocIdValue & "]"
        End If

        'Get the addnl selection formula
        strAdnlFormula = Replace(Trim(GetVal(recDummy!AddnlSelFormula, "")), "`", "'")

        With cryRpt
            Try
                .Load(strCrystalReportFileName)
            Catch ex As Exception
                MsgBox("Please ensure " & strCrystalReportFileName & " Exists!" & vbCrLf & ex.Message, MsgBoxStyle.Critical, Me.Text)
                Exit Function
            End Try

            connectionLoginInfo.ConnectionInfo.UserID = gStrDBLoginName
            connectionLoginInfo.ConnectionInfo.Password = gStrDBLoginPwd
            connectionLoginInfo.ConnectionInfo.ServerName = gStrServerName
            connectionLoginInfo.ConnectionInfo.DatabaseName = gStrDBName

            For Each reportTable In .Database.Tables
                reportTable.ApplyLogOnInfo(connectionLoginInfo)
            Next

            .RecordSelectionFormula = strSelFormula
            strDocNo = strSelFormula
            'Try
            If recDummy!Formulasfld <> "" Then
                arrFormulaFld = Split(GetVal(recDummy!FormulasFld, ""), ";")
                arrFormulaVal = Split(GetVal(recDummy!FormulasVal, ""), ";")

                For i = 0 To arrFormulaFld.Length - 1
                    strFormula = Replace(Trim(arrFormulaFld(i)), "`", "'")
                    If InStr(1, strFormula, "=") = 0 Then
                        strFormula = strFormula & " ="
                    End If
                    iPos = InStr(1, arrFormulaVal(i), ".")
                    StrTable = Strings.Left(Trim(arrFormulaVal(i)), iPos - 1)
                    Strfld = Mid(Trim(arrFormulaVal(i)), iPos + 1, Len(arrFormulaVal(i)))

                    strFormula = clsSVT.getDataStringValue("Select " & Strfld & " from " & StrTable, "", False)
                    .SetParameterValue(arrFormulaFld(i), strFormula)
                Next i
            End If

            With crptViewr
                .ToolPanelView = CrystalDecisions.Windows.Forms.ToolPanelViewType.None
                .RefreshReport()
                .ReportSource = cryRpt
                .Refresh()
            End With
        End With
        ShowRptFile_MultiPrint = True
        Exit Function
    End Function

    Public Function GenRptFile(ByVal argRptName As String, argRptFileName As String,
                               argFormula As String, argFormulaVal As String,
                                Optional ByVal argSelection As String = "") As Boolean
        GenRptFile = True
        Dim iPos As Integer, i As Integer
        Dim strFormula As String, strTable As String, strFld As String
        Dim arrFormulaFld() As String, arrFormulaVal() As String
        Dim connectionLoginInfo As New TableLogOnInfo
        Dim cryRpt As New ReportDocument

        strRptFileName = gstrRptPath & argRptFileName

        With cryRpt
            Try
                .Load(strRptFileName)
            Catch ex As Exception
                MsgBox("Please ensure " & strRptFileName & " Exists!" & vbCrLf & ex.Message, MsgBoxStyle.Critical, Me.Text)
                Exit Function
            End Try

            connectionLoginInfo.ConnectionInfo.UserID = gStrDBLoginName
            connectionLoginInfo.ConnectionInfo.Password = gStrDBLoginPwd
            connectionLoginInfo.ConnectionInfo.ServerName = gStrServerName
            connectionLoginInfo.ConnectionInfo.DatabaseName = gStrDBName

            For Each reportTable In .Database.Tables
                reportTable.ApplyLogOnInfo(connectionLoginInfo)
            Next

            If argSelection <> "" Then
                .RecordSelectionFormula = argSelection
            End If
            ' strDocNo = strSelFormula
            'Try
            If argFormula <> "" Then
                arrFormulaFld = Split(argFormula, ";")
                arrFormulaVal = Split(argFormulaVal, ";")
                For i = 0 To arrFormulaFld.Length - 1
                    strFormula = Replace(Trim(arrFormulaFld(i)), "`", "'")
                    If InStr(1, strFormula, "=") = 0 Then
                        strFormula = strFormula & " ="
                    End If
                    iPos = InStr(1, arrFormulaVal(i), ".")
                    If iPos > 0 Then
                        strTable = Strings.Left(Trim(arrFormulaVal(i)), iPos - 1)
                        strFld = Mid(Trim(arrFormulaVal(i)), iPos + 1, Len(arrFormulaVal(i)))

                        strFormula = clsSVT.getDataStringValue("Select " & strFld & " from " & strTable, "", False)
                    Else
                        strFormula = arrFormulaVal(i)
                    End If
                    .SetParameterValue(arrFormulaFld(i), strFormula)
                Next i
            End If


            With crptViewr
                .ToolPanelView = CrystalDecisions.Windows.Forms.ToolPanelViewType.None
                .RefreshReport()
                .ReportSource = cryRpt
                .Refresh()
            End With
        End With

        Return True
    End Function

    Public Function ExportPDF(ByVal argTrnDocID As Integer,
                                            ByVal argDocIdValue As Long,
                                            ByVal argReportOptionName As String,
                                            ByVal Optional argFileName As String = "") As Boolean
        Dim iPos As Integer, i As Integer
        Dim strSelFormula As String, strAdnlFormula As String
        Dim Strfld As String, StrTable As String
        Dim strFormula As String
        Dim arrFormulaFld() As String, arrFormulaVal() As String, strCrystalReportFileName As String
        Dim strSQL As String
        Dim connectionLoginInfo As New TableLogOnInfo
        ExportPDF = False
        Dim recDummy As oledb.oledbDataReader
        Dim intNoOfCopies As Integer
        Dim cryRpt As New ReportDocument

        strSQL = " select top 1 * from UTLOption where DocID = " & argTrnDocID
        If argReportOptionName <> "" Then strSQL = strSQL & " and OptionName = '" & argReportOptionName & "'"
        strSQL = strSQL & " order by optionorder"

        recDummy = clsSVT.getDataReaderFromSQL(strSQL)

        If Not recDummy.HasRows Then Exit Function
        recDummy.Read()
        intNoOfCopies = recDummy("NoOfCopies")

        strCrystalReportFileName = gstrRptPath & GetVal(recDummy!CrptFileName, "")
        strRptFileName = gstrRptPath & GetVal(recDummy!CrptFileName, "")

        If InStr(1, UCase(Trim(strCrystalReportFileName)), ".RPT") = 0 Then
            strCrystalReportFileName = strCrystalReportFileName & ".Rpt"
        End If

        'Get the selection formula
        strSelFormula = Trim(Replace(Trim(GetVal(recDummy!SelFormula, "")), "`", "'"))
        If strSelFormula <> "" Then
            strSelFormula = "" & strSelFormula & " " & argDocIdValue
        End If

        'Get the addnl selection formula
        strAdnlFormula = Replace(Trim(GetVal(recDummy!AddnlSelFormula, "")), "`", "'")

        With cryRpt
            Try
                .Load(strCrystalReportFileName)
            Catch ex As Exception
                MsgBox("Please ensure " & strCrystalReportFileName & " Exists!" & vbCrLf & ex.Message, MsgBoxStyle.Critical, Me.Text)
                Exit Function
            End Try

            connectionLoginInfo.ConnectionInfo.UserID = gStrDBLoginName
            connectionLoginInfo.ConnectionInfo.Password = gStrDBLoginPwd
            connectionLoginInfo.ConnectionInfo.ServerName = gStrServerName
            connectionLoginInfo.ConnectionInfo.DatabaseName = gStrDBName

            For Each reportTable In .Database.Tables
                reportTable.ApplyLogOnInfo(connectionLoginInfo)
            Next

            .RecordSelectionFormula = strSelFormula
            strDocNo = strSelFormula
            'Try
            If recDummy!Formulasfld <> "" Then
                arrFormulaFld = Split(GetVal(recDummy!FormulasFld, ""), ";")
                arrFormulaVal = Split(GetVal(recDummy!FormulasVal, ""), ";")

                For i = 0 To arrFormulaFld.Length - 1
                    strFormula = Replace(Trim(arrFormulaFld(i)), "`", "'")
                    If InStr(1, strFormula, "=") = 0 Then
                        strFormula = strFormula & " ="
                    End If
                    iPos = InStr(1, arrFormulaVal(i), ".")
                    StrTable = Strings.Left(Trim(arrFormulaVal(i)), iPos - 1)
                    Strfld = Mid(Trim(arrFormulaVal(i)), iPos + 1, Len(arrFormulaVal(i)))

                    strFormula = clsSVT.getDataStringValue("Select " & Strfld & " from " & StrTable, "", False)
                    .SetParameterValue(arrFormulaFld(i), strFormula)
                Next i
            End If

            'Catch ex As Exception
            'MsgBox("Please check Formula in Designer!" & vbCrLf & ex.Message, MsgBoxStyle.Critical, Me.Text)
            'End Try

            'If Not blnPreview Then .PrintToPrinter(1, False, 0, 0)
            Try
                Dim strAttachFileName As String = ""
                If argFileName <> "" Then
                    strAttachFileName = argFileName
                Else
                    strAttachFileName = Application.StartupPath & "\Temp\" & Date.Now.ToString("yyyyMMddHHmmssfff") & ".pdf"
                End If

                If File.Exists(strAttachFileName) Then
                    File.Delete(strAttachFileName)
                End If
                Dim pdfCryRpt As New ReportDocument
                pdfCryRpt = cryRpt
                Try
                    pdfCryRpt.ExportToDisk(ExportFormatType.PortableDocFormat, strAttachFileName)
                Catch ex As Exception
                    MsgBox(ex.Message & vbCrLf & "Error during Exporting to PDF", MsgBoxStyle.Critical, "ERROR")
                End Try

            Catch ex As Exception
                MsgBox("Error in PDF File!" & vbCrLf & ex.Message, MsgBoxStyle.Critical, "ERROR")
            End Try
        End With
        ExportPDF = True
    End Function
    Public Sub showSPRpt(argFilename As String, argInvID As Long, argDefPrinter As String, Optional argPrintCopies As Integer = 1)
        Dim connectionLoginInfo As New TableLogOnInfo
        Dim cryRpt As New ReportDocument
        With cryRpt
            Try
                .Load(gstrRptPath & "fmtSPInvPrint.rpt")
            Catch ex As Exception
                MsgBox("Rpt file " & gstrRptPath & "fmtSPInvPrint.rpt does not Exists!" & vbCrLf & ex.Message, MsgBoxStyle.Critical, Me.Text)
                Exit Sub
            End Try

            connectionLoginInfo.ConnectionInfo.UserID = gStrDBLoginName
            connectionLoginInfo.ConnectionInfo.Password = gStrDBLoginPwd
            connectionLoginInfo.ConnectionInfo.ServerName = gStrServerName
            connectionLoginInfo.ConnectionInfo.DatabaseName = gStrDBName

            For Each reportTable In .Database.Tables
                reportTable.ApplyLogOnInfo(connectionLoginInfo)
                reportTable.location = gStrDBName & "..mSP_PrintInv;1"
            Next
            .SetParameterValue("@mInvID", argInvID)
            If argDefPrinter <> "" Then
                Dim ps As New System.Drawing.Printing.PrinterSettings
                ps.PrinterName = argDefPrinter
                'ps.Copies = argPrintCopies
                Dim pd As New System.Drawing.Printing.PaperSize()
                Dim pds As New System.Drawing.Printing.PageSettings
                pd.PaperName = PaperKind.Custom
                pds.PaperSize = pd
                For i = 1 To argPrintCopies
                    cryRpt.PrintToPrinter(ps, pds, 0)
                Next i
                cryRpt.Close()
                cryRpt.Dispose()
                GC.Collect()
            Else
                With crptViewr
                    .RefreshReport()
                    .ReportSource = cryRpt
                End With
            End If
        End With
    End Sub
End Class