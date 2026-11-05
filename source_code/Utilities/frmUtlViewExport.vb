Imports System.Data.oledb
Imports System.IO
Imports C1.Win.C1FlexGrid

Public Class frmUtlViewExport
    Dim clsSvt As New svtLibrary
    Dim blnADO As Boolean, strFormat As String, blnRecordset As Boolean
    Dim strSQL As String, blnSave As Boolean
    Dim strSavedGrid As String

    Public strRptTag As String
    Public strRptVchName As String

    Public strTag As String
    Public strTag1 As String
    Public strTag2 As String
    Public strOrgQry As String
    Public argQry As String
    Dim fldArr() As String

    'Declarations for vsfShowOrHide
    Const vHide As Byte = 0
    Const vColName As Byte = 1

    Const vLayColNo As Byte = 0
    Const vLayColName As Byte = 1
    Const vLayColPrint As Byte = 2
    Const vLayColDataType As Byte = 3
    Const vLayColAlignment As Byte = 4
    Const vLayColWidth As Byte = 5
    Const vLayColFormat As Byte = 6
    Const vLayColFont As Byte = 7
    Const vLayColFontBold As Byte = 8
    Const vLayPrintAsGroup As Byte = 9
    Const vLayColcaptionName As Byte = 10

    Const intPageLen As Integer = 60

    Const vFltGrp As Byte = 0
    Const vFltColumn As Byte = 1
    Const vFltFld As Byte = 2
    Const vFltDataType As Byte = 3
    Const vFltOperator As Byte = 4
    Const vFltFilter As Byte = 5
    Const vFltValue As Byte = 6

    Const vXlsSel As Byte = 1
    Const vXlsCaption As Byte = 0
    Const vXlsValue As Byte = 2
    Dim dtSQL As DataTable, dtView As DataView


    '    Private Function getViewCol(ByVal argColName As String) As Integer

    '        Dim intCol As Integer

    '        With vsfView
    '            For intCol = 0 To .Cols - 1
    '                If UCase$(argColName) = UCase$(.TextMatrix(0, intCol)) Then
    '                    getViewCol = intCol
    '                    Exit Function
    '                End If
    '            Next
    '        End With
    '        getViewCol = -1
    '    End Function

    '    Private Sub cmdCancel_LostFocus()
    '        cmdFreeze.SetFocus()
    '    End Sub

    '    Private Sub cmdClose_LostFocus()
    '        If fraDosPrint.Visible = True Then
    '            cboLayout.SetFocus()
    '        End If
    '    End Sub

    '    Private Sub cmdDelete_Click()
    '        If Trim(cboLayoutName.Text) = "" Then
    '            MsgBox("Layout Name is not entered!!", vbInformation, Me.Caption)
    '            Exit Sub
    '        End If
    '        svtConn.BeginTrans()
    '        svtConn.Execute("Delete from UtlDosRptLayout where " _
    '            & " rptName = '" & cboLayoutName.Text & "' and " _
    '            & " rptTag = '" & Trim(Me.Caption) & "'") 'strRptTag
    '        svtConn.CommitTrans()

    '        MsgBox("Report deleted", vbInformation + vbOKOnly, Me.Caption)

    '        fraLayoutInfo.Visible = False
    '        fraDosPrint.Enabled = True
    '        cboLayout.Text = ""
    '        cboLayoutName.Text = ""
    '        '# A
    '        Call fillRptLayoutName()
    '    End Sub

    '    Private Sub cmdFreeze_Click()
    '        If cmdFreeze.Caption = "Freeze" Then
    '            cmdFreeze.Caption = "UnFreeze"
    '            vsfView.FrozenCols = vsfView.Col
    '        Else
    '            cmdFreeze.Caption = "Freeze"
    '            vsfView.FrozenCols = 0
    '        End If
    '        'call
    '    End Sub

    '    Private Sub cmdHideClose_Click()
    '        With chkShowOrHide
    '            .value = vbUnchecked
    '            .Caption = "Select Cols"
    '            fraHide.Visible = False
    '            vsfView.Width = fraView.Width - 200
    '        End With
    '    End Sub

    '    Private Sub cmdHideClose_LostFocus()
    '        If fraHide.Visible = True Then
    '            chkShowAll.SetFocus()
    '        End If
    '    End Sub

    '    Private Sub cmdLayoutCancel_Click()
    '        fraLayoutInfo.Visible = False
    '        fraDosPrint.Enabled = True
    '    End Sub
    '    Private Sub cmdAdvSettings_Click()
    '        fraAdvsettings.Visible = True
    '    End Sub

    '    Private Function fnValidate() As Boolean
    '        Dim i As Integer
    '        fnValidate = False
    '        If Trim(cboLayoutName.Text) = "" Then
    '            MsgBox("Layout Name is not entered!!", vbInformation, Me.Caption)
    '            Exit Function
    '        End If
    '        With vsfLayoutInfo
    '            For i = 1 To .Rows - 1
    '                'If .Cell(flexcpChecked, i, vLayColPrint) <> flexChecked Then GoTo nexti
    '                If Trim(vsfLayoutInfo.TextMatrix(i, vLayColDataType)) = "" Then
    '                    MsgBox("Please enter Column Data type", vbInformation + vbOKOnly, Me.Caption)
    '                    .Col = vLayColDataType : .Row = i
    '                    .SetFocus()
    '                    Exit Function
    '                End If
    '                If Trim(vsfLayoutInfo.TextMatrix(i, vLayColAlignment)) = "" Then
    '                    MsgBox("Please enter Column Alignment", vbInformation + vbOKOnly, Me.Caption)
    '                    .Col = vLayColAlignment : .Row = i
    '                    .SetFocus()
    '                    Exit Function
    '                End If
    '                If Trim(vsfLayoutInfo.TextMatrix(i, vLayColWidth)) = "" Then
    '                    MsgBox("Please enter Column width", vbInformation + vbOKOnly, Me.Caption)
    '                    .Col = vLayColWidth : .Row = i
    '                    .SetFocus()
    '                    Exit Function
    '                End If
    '                If Trim(vsfLayoutInfo.TextMatrix(i, vLayColName)) = "" Then
    '                    MsgBox("Please enter Column name", vbInformation + vbOKOnly, Me.Caption)
    '                    .Col = vLayColName : .Row = i
    '                    .SetFocus()
    '                    Exit Function
    '                End If
    'nexti:
    '            Next i
    '        End With
    '        fnValidate = True
    '    End Function


    '    Private Sub cmdSave_Click()
    '        Dim i As Integer, strSQL As String
    '        On Error GoTo errHandler

    '        If Not fnValidate Then Exit Sub

    '        svtConn.BeginTrans()
    '        svtConn.Execute("Delete from UtlDosRptLayout where " _
    '         & " rptName = '" & cboLayoutName.Text & "'" _
    '         & " and rptTag = '" & strTag1 & "'")  'strRptTag 'Trim(Me.Caption)
    '        With vsfLayoutInfo
    '            For i = 1 To .Rows - 1
    '                'If .Cell(flexcpChecked, i, vLayColPrint) <> flexChecked Then GoTo nexti
    '                strSQL = "Insert into UtlDosRptLayout ("
    '                strSQL = strSQL & " RptTag, RptName, ColID, ColName, ColCaptionName,ColPrint, ColDataType, "
    '                strSQL = strSQL & " ColAlignment, ColWidth, ColFormat, ColFont, "
    '                strSQL = strSQL & " ColFontBold, "
    '                strSQL = strSQL & " ColPrintAsGroup) values ("
    '                'strSQL = strSQL & "'" & strRptTag & "',"
    '                'strSQL = strSQL & "'" & Trim(Me.Caption) & "'," 'Rpt tag
    '                strSQL = strSQL & "'" & strTag1 & "',"
    '                strSQL = strSQL & "'" & Trim(cboLayoutName.Text) & "'," 'Rpt Name
    '                strSQL = strSQL & Val(.TextMatrix(i, vLayColNo)) & ","
    '                strSQL = strSQL & "'" & .TextMatrix(i, vLayColName) & "',"
    '                strSQL = strSQL & "'" & .TextMatrix(i, vLayColcaptionName) & "',"
    '                If .Cell(flexcpChecked, i, vLayColPrint) = flexChecked Then
    '                    strSQL = strSQL & "'Y', "
    '                Else
    '                    strSQL = strSQL & "'N', "
    '                End If
    '                strSQL = strSQL & "'" & .TextMatrix(i, vLayColDataType) & "',"
    '                strSQL = strSQL & "'" & .TextMatrix(i, vLayColAlignment) & "',"
    '                strSQL = strSQL & "'" & .TextMatrix(i, vLayColWidth) & "',"
    '                strSQL = strSQL & "'" & .TextMatrix(i, vLayColFormat) & "',"
    '                strSQL = strSQL & "'" & .TextMatrix(i, vLayColFont) & "',"
    '                If .Cell(flexcpChecked, i, vLayColFontBold) = flexChecked Then
    '                    strSQL = strSQL & "'Y', "
    '                Else
    '                    strSQL = strSQL & "'N', "
    '                End If

    '                If .Cell(flexcpChecked, i, vLayPrintAsGroup) = flexChecked Then
    '                    strSQL = strSQL & "'Y') "
    '                Else
    '                    strSQL = strSQL & "'N') "
    '                End If
    '                svtConn.Execute(strSQL)
    'nexti:
    '            Next i
    '        End With
    '        svtConn.CommitTrans()
    '        MsgBox("Saved Successfully!!", vbInformation, Me.Caption)
    '        Exit Sub

    'errHandler:
    '        svtConn.RollbackTrans()
    '        MsgBox(err.Description, vbInformation + vbOKOnly, Me.Caption)
    '    End Sub

    '    Private Sub cmdSaveCols_Click()

    '        Dim strCols As String, strFld As String
    '        Dim i As Integer
    '        If strRptTag = "" Then
    '            strRptTag = Me.Caption
    '        End If
    '        svtConn.Execute("delete from UtlSelCols where UserCode='" & gUsrCode & "' and  FormTag ='" & strRptTag & "'")
    '        strFld = ""
    '        With vsfShowOrHide
    '            For i = 1 To .Rows - 1
    '                If .Cell(flexcpChecked, i, 0) = flexChecked Then
    '                    strFld = strFld & .TextMatrix(i, 1) & ","
    '                End If
    '            Next
    '        End With
    '        If Trim(strFld) <> "" Then
    '            strFld = Mid(strFld, 1, Len(strFld) - 1)
    '            strFld = "insert into UtlSelCols (USerCode,FormTag, SelCols) values ('" & _
    '                                        gUsrCode & "','" & strRptTag & "','" & strFld & "')"
    '            svtConn.Execute(strFld)
    '        End If

    '        With chkShowOrHide
    '            .value = vbUnchecked
    '            .Caption = "Select Cols"
    '            fraHide.Visible = False
    '            vsfView.Width = fraView.Width - 200
    '        End With
    '    End Sub

    '    Private Sub cmdSaveCols_KeyPress(ByVal KeyAscii As Integer)
    '        If KeyAscii = vbKeyTab Then cmdCancel.SetFocus()
    '    End Sub

    '    Private Sub Form_KeyUp(ByVal KeyCode As Integer, ByVal Shift As Integer)
    '        If KeyCode = vbKeyF9 And Shift = 2 Then
    '            Call initFormSize(Me, True, 6915, 11870, 15, 45)
    '            MsgBox("Restored Form & Grid Layout Successfully!!", vbInformation, Me.Caption)
    '        End If
    '        If KeyCode = vbKeyF8 And Shift = 2 Then
    '            Call SaveActFormSettings(Me)
    '            MsgBox("Saved Form & Grid Layout Successfully!!", vbInformation, Me.Caption)
    '        End If
    '    End Sub
    '    Private Sub Form_Resize()
    '        If frmMain.WindowState = vbMinimized Then Exit Sub
    '        If Me.WindowState = vbMinimized Then Exit Sub '# A
    '        If Me.Height < 6915 Then
    '            Me.Height = 6915
    '        End If
    '        If Me.Width < 11870 Then
    '            Me.Width = 11870
    '        End If
    '        'Me.Show
    '        fraView.Width = Me.Width - fraView.Left - 200
    '        fraView.Height = Me.Height - fraView.Top - 1200

    '        vsfView.Width = fraView.Width - vsfView.Left - 100
    '        fraControls.Top = fraView.Height - fraControls.Height - 100
    '        fraControls.Left = fraView.Left + (fraView.Width - fraControls.Width) / 2

    '        fraHide.Left = Me.Width - fraHide.Width - fraView.Left - 300
    '        fraHide.Height = vsfView.Height - 285
    '        vsfShowOrHide.Height = fraHide.Height - 800
    '        chkShowAll.Top = fraHide.Height - 400
    '        cmdHideClose.Top = chkShowAll.Top
    '        cmdSaveCols.Top = cmdHideClose.Top


    '        fraLayoutInfo.Width = vsfView.Width - 300
    '        vsfLayoutInfo.Width = fraLayoutInfo.Width - 500
    '        cmdLayoutClose.Left = fraLayoutInfo.Width - cmdLayoutClose.Width - 200
    '        cmdSave.Left = fraLayoutInfo.Width - cmdLayoutClose.Width - cmdSave.Width - 200
    '        cmdDelete.Left = cmdLayoutClose.Left
    '        'Me.Show
    '        vsfView.Height = fraView.Height - fraControls.Height - 400
    '    End Sub

    '    Private Sub Form_Load()
    '        Dim i As Integer
    '        With Me
    '            .Height = 6915
    '            .Top = 45
    '            .Left = 15
    '            .Width = 11870
    '        End With
    '        Me.Tag = Me.Caption
    '        Call CenteringChildFormInMDI(frmMain, Me)
    '        fraDosPrint.Visible = False
    '        fraLayoutInfo.Visible = False
    '        fraView.Visible = True
    '        With vsfLayoutInfo
    '            .Rows = 1
    '            .Cols = 11
    '            .FixedCols = 2
    '            .TextMatrix(0, vLayColName) = "Column Name"
    '            .TextMatrix(0, vLayColPrint) = "Include"
    '            .TextMatrix(0, vLayPrintAsGroup) = "Print As Group"
    '            .TextMatrix(0, vLayColAlignment) = "Alignment"
    '            .TextMatrix(0, vLayColDataType) = "Data Type"
    '            .TextMatrix(0, vLayColFont) = "Font"
    '            .TextMatrix(0, vLayColFontBold) = "Bold"
    '            .TextMatrix(0, vLayColFormat) = "Format"
    '            .TextMatrix(0, vLayColWidth) = "Width"
    '            .TextMatrix(0, vLayColcaptionName) = "Caption Name"
    '            .ColDataType(vLayColPrint) = flexDTBoolean
    '            .ColDataType(vLayPrintAsGroup) = flexDTBoolean
    '            .ColDataType(vLayColFontBold) = flexDTBoolean
    '            .ColComboList(vLayColDataType) = "String |Numeric |Date"
    '            .ColComboList(vLayColAlignment) = "Left|Center|Right"
    '            .ColComboList(vLayColFont) = "Normal|Compressed"
    '            .ColHidden(vLayColNo) = True
    '            Call clsSvt.SetColWidth(vsfLayoutInfo, 15, 3105, 825, 1215, 1005, 600, 1080, 975, 450, 1125, 1300)
    '        End With
    '        With vsfXlsFormat
    '            .FixedCols = 1
    '            .Cols = 3
    '            .Rows = 7
    '            .TextMatrix(0, vXlsSel) = "*"
    '            .TextMatrix(0, vXlsCaption) = "Heading"
    '            .TextMatrix(0, vXlsValue) = "Value/Information"
    '            .ColDataType(vXlsSel) = flexDTBoolean
    '            Call clsSvt.SetColWidth(vsfXlsFormat, 2000, 250, 3560)

    '            .TextMatrix(1, vXlsCaption) = "Company Name"
    '            .TextMatrix(1, vXlsValue) = gSettings.gCompName
    '            .TextMatrix(2, vXlsCaption) = "Company Address"
    '            .TextMatrix(2, vXlsValue) = gSettings.gCompAdd1
    '            .TextMatrix(3, vXlsCaption) = ""
    '            .TextMatrix(3, vXlsValue) = gSettings.gCompAdd2
    '            .TextMatrix(4, vXlsCaption) = ""
    '            .TextMatrix(4, vXlsValue) = gSettings.gCompAdd3
    '            .TextMatrix(5, vXlsCaption) = ""
    '            .TextMatrix(5, vXlsValue) = gSettings.gCompAdd4
    '            .TextMatrix(6, vXlsCaption) = "Report Heading 1 "
    '            .TextMatrix(6, vXlsValue) = Me.Caption
    '            For i = 1 To .Rows - 1
    '                .Cell(flexcpChecked, i, vXlsSel) = flexChecked
    '            Next i
    '        End With

    '        Me.KeyPreview = True
    '        Call initFormSize(Me, False, 6915, 11870, 15, 45)

    '    End Sub

    '    Public Sub initScreenSQL(ByVal argBlnADO As Boolean, _
    '                          ByVal argStrFormat As String, _
    '                          ByVal argStrSQL As String, _
    '                          ByVal arblnsave As Boolean, _
    '                          ByVal arStrSavedGrid As String, _
    '                          ByVal argStrTag As String, _
    '                          ByVal argHighlightFld As String, _
    '                          ByVal argHighlightChkValue As String, _
    '                          ByVal ParamArray intGrp())


    '        argQry = argStrSQL

    '        Dim i As Integer, j As Integer
    '        Dim cmdDummy As New ADODB.Command
    '        Dim rsDummy As New ADODB.Recordset
    '        On Error GoTo errHandler

    '        Dim intGrpCol As Object, strString As String
    '        Dim strColList As String
    '        Dim intStartPos As Integer
    '        Dim intEndPos As Integer

    '        Screen.MousePointer = vbHourglass
    '        stMsg("Loading Listing Details...")
    '        blnADO = argBlnADO
    '        'vsfView.Height = 5685

    '        strFormat = argStrFormat
    '        strSQL = argStrSQL
    '        blnSave = arblnsave
    '        strSavedGrid = arStrSavedGrid
    '        strTag = argStrTag

    '        With cmdDummy
    '            .ActiveConnection = svtConn
    '            .CommandText = strSQL
    '            rsDummy = .Execute
    '        End With

    '        vsfView.FormatString = strFormat

    '        If strSavedGrid = "" Then
    '            Call fillViewGrid(rsDummy, argHighlightFld, argHighlightChkValue)
    '        Else
    '            If Dir(strSavedGrid) <> "" Then
    '                Call vsfView.LoadGrid(strSavedGrid, flexFileAll, True)
    '            Else
    '                Call fillViewGrid(rsDummy, argHighlightFld, argHighlightChkValue)
    '            End If
    '        End If
    '        vsfView.FormatString = strFormat

    '        With vsfView
    '            For Each intGrpCol In intGrp
    '                If intGrpCol >= 0 Then .MergeCol(intGrpCol) = True
    '            Next intGrpCol
    '        End With

    '        With vsfShowOrHide
    '            .TextMatrix(0, vHide) = " Show/Hide Column "
    '            .TextMatrix(0, vColName) = " Show/Hide Column "
    '            ''.MergeCells = flexMergeRestrictAll
    '            .ColWidthMin = 550
    '            .ColWidth(0) = 1000
    '            .ColWidth(1) = 2500
    '            .ColDataType(0) = flexDTBoolean
    '            .Gridlines = flexGridFlatHorz
    '            .Rows = 1
    '            For i = 0 To vsfView.Cols - 1
    '                .Rows = .Rows + 1
    '                .TextMatrix(.Rows - 1, 1) = vsfView.TextMatrix(0, i)
    '                If vsfView.ColHidden(i) = True Then
    '                    .TextMatrix(.Rows - 1, 0) = flexUnchecked
    '                Else
    '                    .TextMatrix(.Rows - 1, 0) = flexChecked
    '                End If
    '            Next i
    '        End With

    '        With rsDummy
    '            strString = ""
    '            For i = 0 To vsfView.Cols - 1
    '                strString = strString & vsfView.TextMatrix(0, i) & "|"
    '                Select Case .Fields(i).Type
    '                    Case adDate, adDBDate, adDBTimeStamp
    '                        vsfView.ColDataType(i) = flexDTDate
    '                        vsfView.ColFormat(i) = "dd mmm yyyy"
    '                End Select
    '            Next i
    '        End With
    '        vsfView.Refresh()
    '        Call vsfView.AutoSize(0, vsfView.Cols - 1)
    '        vsfXlsFormat.TextMatrix(6, vXlsValue) = Me.Caption

    '        rsDummy = Nothing
    '        cmdDummy = Nothing
    '        stMsg("")
    '        Screen.MousePointer = vbArrow
    '        Exit Sub
    'errHandler:

    '        Clipboard.Clear()
    '        Clipboard.SetText(argStrSQL)
    '        MsgBox(err.Description)
    '    End Sub
    '    Private Sub cmdXlsOk_Click()
    '        Dim i As Integer
    '        '    For i = 0 To 5
    '        '        If chkXlsInfo(i).Value = vbChecked Then
    '        '            arrXlsInfo(0, i) = txtXlsCap(i).Text
    '        '            arrXlsInfo(1, i) = txtXlsInfo(i).Text
    '        '        End If
    '        '    Next i
    '        fraAdvsettings.Visible = False
    '    End Sub

    Public Sub initScreenSQL(ByVal argBlnADO As Boolean,
                          ByVal argStrFormat As String,
                          ByVal argStrSQL As String,
                          ByVal arblnsave As Boolean,
                          ByVal arStrSavedGrid As String,
                          ByVal argStrTag As String,
                          ByVal argStrTag1 As String,
                          ByVal argStrTag2 As String,
                          ByVal ParamArray intGrp() As Integer)

        'Dim cmdDummy As New ADODB.Command
        Dim rsDummy As oledb.oledbDataReader = Nothing
        'On Error GoTo errHandler

        'Dim intGrpCol As Object
        'Dim strColList As String
        'Dim intStartPos As Integer
        'Dim intEndPos As Integer

        Cursor = Cursors.WaitCursor
        StMsg("Loading Listing Details...")
        blnADO = argBlnADO
        'vsfView.Height = 5685
        'vsfView.Height = fraControls.Top - 300
        Try
            strFormat = argStrFormat
            strSQL = Trim(argStrSQL)
            blnSave = arblnsave
            strSavedGrid = arStrSavedGrid
            strTag = argStrTag
            strTag1 = argStrTag1
            strTag2 = argStrTag2



            Call fillViewGrid(strSQL)

            'With rsDummy
            '    strString = ""
            '    For i = 0 To vsfView.Cols.Count - 1
            '        strString = strString & vsfView.GetData(0, i) & "|"
            '        Select Case .Item(i).Type
            '            Case vbDate
            '                vsfView.Cols(i).DataType = GetType(Date)
            '                vsfView.Cols(i).Format = "dd mmm yyyy"
            '        End Select
            '    Next i
            'End With
            'rsDummy = Nothing

            StMsg("")
            Cursor = Cursors.Default
            Exit Sub
        Catch ex As Exception
        End Try
        'errHandler:
        '            Clipboard.Clear()
        '            Clipboard.SetText(argStrSQL)
        '            MsgBox(Err.Description)
    End Sub

    Private Sub frmUtlViewExport_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub fraControls_Paint(sender As Object, e As PaintEventArgs) Handles fraControls.Paint

    End Sub

    Private Sub fillViewGrid(ByVal strsql As String, Optional ByVal argHighlightFld As String = "", Optional ByVal argHighlightChkValue As String = "")


        dtSQL = clsSvt.getDATATable(strsql)
        dtView = New DataView(dtSQL)
        vsfView.Cols.Fixed = 0
        vsfView.DataSource = dtView


        '    vsfDet.Rows.Fixed = 4
        '  vsfDet.Rows.GlyphRow = 3
        vsfView.BeginUpdate()

        vsfView.DataSource = dtView
        vsfView.EndUpdate()
        vsfSearch.Cols.Count = vsfView.Cols.Count

        Dim intScrollBarWidth As Integer = SystemInformation.VerticalScrollBarWidth
        vsfSearch.Cols(vsfSearch.Cols.Count - 1).Width = vsfView.Cols(vsfView.Cols.Count - 1).Width + intScrollBarWidth
        Call syncGridCols()
        'Try
        '    With vsfView
        '        For j = 0 To dtSQL.Columns.Count - 1
        '            Select Case UCase(dtSQL.Columns(j).DataType.Name.ToString)
        '                Case "DATETIME"
        '                    .Cols(j).DataType = GetType(Date)
        '                    If Trim(.GetData(i, j)) <> "" Then
        '                        .SetData(i, j, Format(.GetData(i, j), "dd mmm yy"))
        '                    End If
        '                Case vbCurrency
        '                    .SetData(i, j, Format(.GetData(i, j), "0.00"))
        '            End Select
        '        Next j
        '        vsfView.AutoSizeCols()
        '        StMsg("")
        '    End With
        'Catch ex As Exception
        '    MsgBox("Error: " & ex.Message, vbCritical, Me.Text)
        'End Try
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

    '    Private Sub cboLayoutName_LostFocus()
    '        Call fillData()
    '    End Sub

    '    Private Sub chkShowAll_Click()
    '        Dim i As Integer
    '        If chkShowAll.Caption = "Show All" Then
    '            chkShowAll.Caption = "Hide All"
    '        Else
    '            chkShowAll.Caption = "Show All"
    '        End If
    '        With vsfShowOrHide
    '            For i = 1 To .Rows - 1
    '                If chkShowAll.Caption = "Hide All" Then
    '                    .TextMatrix(i, 0) = flexChecked
    '                Else
    '                    .Cell(flexcpChecked, i, 0) = flexUnchecked
    '                End If
    '                .Row = i
    '                Call ShowOrHideCols()
    '            Next i
    '        End With
    '    End Sub

    '    Private Sub chkShowOrHide_Click()
    '        Dim i As Integer
    '        With vsfShowOrHide
    '            .TextMatrix(0, vHide) = " Show/Hide"
    '            .TextMatrix(0, vColName) = "Column Name"
    '            ''.MergeCells = flexMergeRestrictAll
    '            .ColWidth(0) = 1000
    '            .ColWidth(1) = 2500
    '            .ColDataType(0) = flexDTBoolean
    '            .Gridlines = flexGridFlatHorz
    '            .Rows = 1
    '            For i = 0 To vsfView.Cols - 1
    '                .Rows = .Rows + 1
    '                .TextMatrix(.Rows - 1, 1) = vsfView.TextMatrix(0, i)
    '                If vsfView.ColHidden(i) = True Then
    '                    .Cell(flexcpChecked, .Rows - 1, 0) = flexUnchecked
    '                Else
    '                    .Cell(flexcpChecked, .Rows - 1, 0) = flexChecked
    '                End If
    '            Next i
    '        End With

    '        With chkShowOrHide
    '            If .value = vbChecked Then
    '                .Caption = "Selection OK"
    '                fraHide.Visible = True
    '                If vsfShowOrHide.Enabled = True Then vsfShowOrHide.SetFocus()
    '                fraHide.ZOrder(0)
    '                '           vsfView.Width = fraView.Width - (fraHide.Width + 500)
    '                If cmdHideClose.Enabled = True Then cmdHideClose.SetFocus()
    '            Else
    '                .Caption = "Select Cols"
    '                fraHide.Visible = False
    '                '           vsfView.Width = fraView.Width - 200

    '            End If
    '        End With
    '    End Sub

    '    Private Sub cmdCancel_Click()
    '        If blnSave Then
    '            vsfView.SaveGrid(strSavedGrid, flexFileAll, True)
    '        End If
    '        Unload(Me)
    '    End Sub

    '    Private Sub cmdClose_Click()
    '        fraDosPrint.Visible = False
    '        fraView.Enabled = True
    '    End Sub

    '    Private Sub cmdDosPrint_Click()
    '        Dim iposn As Integer
    '        strRptTag = ""
    '        iposn = InStr(1, Trim(Me.Caption), "for the Period")
    '        If iposn > 0 Then
    '            strRptTag = Trim(Mid(Trim(Me.Caption), 1, iposn - 1))
    '        Else
    '            strRptTag = Trim(Me.Caption)
    '        End If
    '        'strRptTag = "Purchase Order Report"
    '        Call fillRptLayoutName()

    '        '    Call clsSvt.FillCombo("UtlDosRptLayout", "Distinct rptname", "", cboLayout, _
    '        '            "where rptTag = '" & Trim(Me.Caption) & "'", "order by 1", True, False)
    '        '    Call clsSvt.FillCombo("UtlDosRptLayout", "Distinct rptname", "", cboLayoutName, _
    '        '            "where rptTag = '" & Trim(Me.Caption) & "'", "order by 1", True, False)
    '        fraDosPrint.Visible = True
    '        cboLayout.SetFocus()
    '        fraView.Enabled = False
    '        If cboLayout.ListCount > 1 Then
    '            cboLayout.Text = cboLayout.List(1)
    '        End If
    '    End Sub

    '    Private Sub cmdLayoutClose_Click()
    '        fraLayoutInfo.Visible = False
    '        fraDosPrint.Enabled = True
    '    End Sub

    '    Private Sub cmdNew_Click()
    '        fraLayoutInfo.Visible = True
    '        fraLayoutInfo.ZOrder(0)
    '        fraDosPrint.Enabled = False
    '        fraView.Enabled = False
    '        cboLayoutName.Text = cboLayout.Text
    '        Call cboLayoutName_LostFocus()
    '    End Sub

    '    Private Sub cmdRefresh_Click()
    '        Dim cmdDummy As New ADODB.Command
    '        Dim rsDummy As New ADODB.Recordset

    '        If strSQL <> "" Then
    '            With cmdDummy
    '                .ActiveConnection = svtConn
    '                .CommandText = strSQL
    '                rsDummy = .Execute
    '            End With
    '            Call fillViewGrid(rsDummy)
    '        End If

    '        rsDummy = Nothing
    '        cmdDummy = Nothing
    '    End Sub
    '    Private Sub Form_Unload(ByVal Cancel As Integer)
    '        clsSvt = Nothing
    '    End Sub


    '    Private Sub vsfShowOrHide_BeforeEdit(ByVal Row As Long, ByVal Col As Long, ByVal Cancel As Boolean)
    '        If Col <> 0 Then Cancel = True
    '    End Sub

    '    Private Sub vsfShowOrHide_Click()
    '        Dim i As Integer
    '        Dim blnFlag As Boolean
    '        With vsfShowOrHide
    '            If .MouseRow = 0 Then Exit Sub
    '            '   .Col = 0
    '            '   .EditCell
    '            Call ShowOrHideCols()
    '            blnFlag = False
    '            For i = 1 To .Rows - 1
    '                If .TextMatrix(i, 0) = True Then
    '                    blnFlag = True
    '                    Exit For
    '                End If
    '            Next i
    '        End With

    '        If blnFlag = True Then
    '            chkShowAll.Caption = "Show All"
    '        Else
    '            chkShowAll.Caption = "Hide All"
    '        End If
    '    End Sub

    '    Private Sub vsfShowOrHide_KeyUp(ByVal KeyCode As Integer, ByVal Shift As Integer)
    '        With vsfShowOrHide
    '            If .Row = 0 Then Exit Sub
    '            If KeyCode = 32 Then
    '                '            .Col = 0
    '                '            .EditCell
    '                Call ShowOrHideCols()
    '            End If
    '            If .Row = .Rows - 1 And .Col = 1 Then
    '                If KeyCode = vbKeyTab Then
    '                    chkShowAll.SetFocus()
    '                End If
    '            End If

    '        End With
    '    End Sub


    '    Private Sub ShowOrHideCols()
    '        With vsfShowOrHide
    '            .Col = 0
    '            .Row = .Row
    '            If .CellChecked = flexChecked Then
    '                .Col = 1
    '                vsfView.ColHidden(.Row - 1) = False
    '            Else
    '                .Col = 1
    '                vsfView.ColHidden(.Row - 1) = True
    '            End If
    '        End With
    '    End Sub



    '    Private Sub vsfView_DblClick()
    '        Call showDet()
    '        If blnSave Then
    '            Call vsfView.SaveGrid(strSavedGrid, flexFileAll, True)
    '            SaveSetting(App.Title, "RowPositions", strSavedGrid, vsfView.Row)
    '            SaveSetting(App.Title, "TopRows", strSavedGrid & "TopRow", vsfView.TopRow)
    '        End If

    '    End Sub

    '    Private Sub VsfView_KeyPress(ByVal KeyAscii As Integer)
    '        If KeyAscii = vbKeyReturn Then
    '            Call showDet()
    '            If blnSave Then
    '                Call vsfView.SaveGrid(strSavedGrid, flexFileAll, True)
    '                SaveSetting(App.Title, "RowPositions", strSavedGrid, vsfView.Row)
    '                SaveSetting(App.Title, "TopRows", strSavedGrid & "TopRow", vsfView.TopRow)
    '            End If

    '        End If
    '    End Sub

    '    Private Sub cmdExport_Click()
    '        If vsfView.Rows > 1 Then
    '            If chkFormat.value = vbUnchecked Then
    '                Call expExUnFormatVw(vsfView, strTag, strTag1, strTag2)
    '                'Call expExUnFormat(vsfView)
    '            Else
    '                Call expExFormatVw(vsfView, strTag, strTag1, strTag2)
    '                'Call expExFormatVw(vsfView, strTag, strTag1, strTag2)
    '                'Call excelExportFromGrid(vsfView, vsfXlsFormat)
    '                'Call expExFormat(vsfView, Me.Tag)
    '            End If
    '        Else
    '            MsgBox("No Information to export", vbInformation, Me.Caption)
    '            Exit Sub
    '        End If
    '    End Sub
    '    Public Sub ExportToXl()
    '        If chkFormat.value = vbUnchecked Then
    '            Call expExUnFormatVw(vsfView, strTag, strTag1, strTag2)
    '            'Call expExUnFormat(vsfView)
    '        Else
    '            Call expExFormatVw(vsfView, strTag, strTag1, strTag2)
    '            'Call expExFormat(vsfView, Me.Tag)
    '        End If
    '    End Sub
    '    Private Sub UpDn_DownClick()
    '        Call MoveOneRowDown(vsfLayoutInfo)
    '    End Sub

    '    Private Sub UpDn_UpClick()
    '        Call MoveOneRowUp(vsfLayoutInfo)
    '    End Sub

    '    Private Sub fillData()
    '        Dim cmdDummy As New ADODB.Command
    '        Dim rsDummy As New ADODB.Recordset
    '        With cmdDummy
    '            .ActiveConnection = svtConn
    '            .CommandText = "Select * from UtlDosRptLayout " _
    '            & " where rptName = '" & cboLayoutName.Text & "' " _
    '            & " and rptTag = '" & strRptTag & "'" 'Trim(Me.Caption)
    '            rsDummy = .Execute
    '        End With
    '        With rsDummy
    '            If .EOF Then
    '                Call fillNewData()
    '            Else
    '                vsfLayoutInfo.Rows = 1
    '                Do While Not .EOF
    '                    vsfLayoutInfo.Rows = vsfLayoutInfo.Rows + 1
    '                    vsfLayoutInfo.TextMatrix(vsfLayoutInfo.Rows - 1, vLayColAlignment) = !ColAlignment
    '                    vsfLayoutInfo.TextMatrix(vsfLayoutInfo.Rows - 1, vLayColDataType) = !ColDataType
    '                    vsfLayoutInfo.TextMatrix(vsfLayoutInfo.Rows - 1, vLayColFont) = !ColFont
    '                    If !colfontBold = "Y" Then
    '                        vsfLayoutInfo.TextMatrix(vsfLayoutInfo.Rows - 1, vLayColFontBold) = True
    '                    End If
    '                    vsfLayoutInfo.TextMatrix(vsfLayoutInfo.Rows - 1, vLayColFormat) = !ColFormat
    '                    vsfLayoutInfo.TextMatrix(vsfLayoutInfo.Rows - 1, vLayColName) = !ColName
    '                    'vsfLayoutInfo.TextMatrix(vsfLayoutInfo.Rows - 1, vLayColcaptionName) = clsSVT.GetVal(!CaptionColName, "")
    '                    vsfLayoutInfo.TextMatrix(vsfLayoutInfo.Rows - 1, vLayColcaptionName) = clsSvt.GetVal(!ColCaptionName, "")

    '                    vsfLayoutInfo.TextMatrix(vsfLayoutInfo.Rows - 1, vLayColNo) = !ColID
    '                    If !colPrint = "Y" Then
    '                        vsfLayoutInfo.TextMatrix(vsfLayoutInfo.Rows - 1, vLayColPrint) = True
    '                    End If
    '                    vsfLayoutInfo.TextMatrix(vsfLayoutInfo.Rows - 1, vLayColWidth) = !ColWidth
    '                    If !ColPrintAsGroup = "Y" Then
    '                        vsfLayoutInfo.TextMatrix(vsfLayoutInfo.Rows - 1, vLayPrintAsGroup) = True
    '                    End If
    '                    .MoveNext()
    '                Loop
    '            End If
    '        End With

    '    End Sub

    '    Private Sub fillNewData()
    '        Dim i As Integer
    '        With vsfLayoutInfo
    '            .Rows = 1
    '            For i = 0 To vsfView.Cols - 1
    '                .Rows = .Rows + 1
    '                .TextMatrix(.Rows - 1, vLayColNo) = i
    '                .TextMatrix(.Rows - 1, vLayColName) = vsfView.TextMatrix(0, i)
    '                .TextMatrix(.Rows - 1, vLayColcaptionName) = vsfView.TextMatrix(0, i)
    '                .TextMatrix(.Rows - 1, vLayColFont) = "Normal"
    '                If vsfView.ColHidden(i) = True Then
    '                    .Cell(flexcpChecked, .Rows - 1, vLayColPrint) = flexUnchecked
    '                Else
    '                    .Cell(flexcpChecked, .Rows - 1, vLayColPrint) = flexChecked
    '                End If
    '                If vsfView.Rows > 1 Then
    '                    If IsNumeric(vsfView.TextMatrix(1, i)) Then
    '                        .TextMatrix(.Rows - 1, vLayColDataType) = "Numeric"
    '                        .TextMatrix(.Rows - 1, vLayColAlignment) = "Right"
    '                        .TextMatrix(.Rows - 1, vLayColFormat) = "0.00"
    '                        .TextMatrix(.Rows - 1, vLayColWidth) = "12"
    '                    ElseIf clsSvt.dateValidate(vsfView.TextMatrix(1, i), False) Then
    '                        .TextMatrix(.Rows - 1, vLayColDataType) = "Date"
    '                        .TextMatrix(.Rows - 1, vLayColAlignment) = "Left"
    '                        .TextMatrix(.Rows - 1, vLayColFormat) = "dd/mm/yyyy"
    '                        .TextMatrix(.Rows - 1, vLayColWidth) = "10"
    '                    Else
    '                        .TextMatrix(.Rows - 1, vLayColDataType) = "String"
    '                        .TextMatrix(.Rows - 1, vLayColAlignment) = "Left"
    '                        .TextMatrix(.Rows - 1, vLayColWidth) = "50"
    '                    End If
    '                End If
    '            Next i
    '        End With
    '    End Sub

    '    Private Sub cmdPrint_Click()

    '        If Trim(cboLayout.Text) = "" Then
    '            MsgBox("Layout Name is not entered!!", vbInformation, Me.Caption)
    '            Exit Sub
    '        End If
    '        cboLayoutName.Text = cboLayout.Text
    '        Call cboLayoutName_LostFocus()
    '    Close #1
    '    Open App.Path & "\DosPrnt.txt" For Output As #1
    '        Call PrntData()
    '    Print #1, prntEject
    '    Close #1
    '        Dim frmPrnt As New frmUtlPrintPreview

    '        With frmPrnt
    '            .strFilepath = App.Path & "\dosPrnt.txt"
    '            .Show()
    '        End With
    '    End Sub

    '    Private Sub PrntData()
    '        Dim intR As Integer, intC As Integer, i As Integer
    '        Dim intLnCtr As Integer, intPgCnt As Integer
    '        Dim strVal As String, strFontChar As String
    '        Dim strLine As String
    '        Dim strPgHdr As String, strHypLine As String
    '        strPgHdr = getPgHdr
    '        Select Case Len(strPgHdr)
    '            Case Is <= 80
    '                strFontChar = prntNormal
    '            Case Else
    '                strFontChar = prntCondensed
    '        End Select
    '    strHypLine = strFontChar & String(Len(strPgHdr), "-")
    '    Print #1, prntNormal & prntSmallExpanded & gSettings.gCompName & prntNormal
    '    Print #1, prntNormal & prntBold & Me.Tag & prntundoBold
    '    Print #1, strHypLine
    '    Print #1, strFontChar & strPgHdr
    '    Print #1, strHypLine
    '        intLnCtr = 5
    '        intPgCnt = 1
    '        With vsfView
    '            For intR = 1 To .Rows - 1
    '                If intLnCtr > intPageLen Then
    '                Print #1, strHypLine
    '                Print #1, "Page : " & intPgCnt & "          (Report Dt: " & Format(Date, "mmm dd yyyy") & "  " & Format(Time, "h:m:s ampm") & ")"
    '                Print #1, prntEject
    '                Print #1, prntNormal & prntSmallExpanded & gSettings.gCompName & prntNormal
    '                Print #1, prntNormal & prntBold & Me.Tag & prntundoBold
    '                Print #1, strHypLine
    '                Print #1, strFontChar & strPgHdr
    '                Print #1, strHypLine
    '                    intPgCnt = intPgCnt + 1
    '                    intLnCtr = 5
    '                End If
    '                strLine = ""
    '                strVal = ""
    '                For intC = 0 To .Cols - 1
    '                    For i = 1 To vsfLayoutInfo.Rows - 1
    '                        If .TextMatrix(0, intC) = vsfLayoutInfo.TextMatrix(i, vLayColName) Then
    '                            If vsfLayoutInfo.Cell(flexcpChecked, i, vLayColPrint) = flexChecked Then
    '                                If vsfLayoutInfo.TextMatrix(i, vLayColFormat) <> "" Then
    '                                    strVal = Format(.TextMatrix(intR, intC), vsfLayoutInfo.TextMatrix(i, vLayColFormat))
    '                                Else
    '                                    strVal = .TextMatrix(intR, intC)
    '                                End If
    '                                strVal = fnJustify(Val(vsfLayoutInfo.TextMatrix(i, vLayColWidth)), strVal, vsfLayoutInfo.TextMatrix(i, vLayColAlignment))
    '                                'for finding wether it is grouped or not
    '                                Dim r1 As Long, c1 As Long, r2 As Long, c2 As Long
    '                                .GetMergedRange(intR, intC, r1, c1, r2, c2)
    '                                If r1 <> r2 Or c1 <> c2 Then
    '                                    If c1 = c2 Then
    '                                        If r1 <> intR Then
    '                                            strVal = Space(Val(vsfLayoutInfo.TextMatrix(i, vLayColWidth)))
    '                                        End If
    '                                    End If
    '                                    If r1 = r2 Then
    '                                        If c1 <> intC Then
    '                                            strVal = Space(Val(vsfLayoutInfo.TextMatrix(i, vLayColWidth)))
    '                                        End If
    '                                    End If
    '                                End If
    '                                If vsfLayoutInfo.Cell(flexcpChecked, i, vLayPrintAsGroup) = flexChecked And Not .IsSubtotal(intR) Then
    '                                    If Trim(strVal) <> "" Then
    '                                        '                                    If intR <> 1 And intC = 0 Then
    '                                        '                                        Print #1, strHypLine
    '                                        '                                        intLnCtr = intLnCtr + 1
    '                                        '                                    End If
    '                                    Print #1, strVal
    '                                    Print #1, String(Len(Trim(strVal)), "~")
    '                                        intLnCtr = intLnCtr + 2
    '                                    End If
    '                                Else
    '                                    strVal = strVal & Space(1)
    '                                    Select Case vsfLayoutInfo.TextMatrix(i, vLayColFont)
    '                                        Case "Normal"
    '                                            strVal = prntNormal & strVal
    '                                        Case "Condensed"
    '                                            strVal = prntCondensed & strVal
    '                                        Case "Small Expanded"
    '                                            strVal = prntSmallExpanded & strVal
    '                                        Case "Large Expanded"
    '                                            strVal = prntLargeExpanded & strVal
    '                                    End Select
    '                                    If vsfLayoutInfo.Cell(flexcpChecked, i, vLayColFontBold) = flexChecked Then
    '                                        strVal = prntBold & strVal & prntundoBold
    '                                    End If
    '                                    strLine = strLine & strVal
    '                                End If
    '                            End If
    '                            Exit For
    '                        End If
    '                    Next i
    '                Next intC
    '                If .IsSubtotal(intR) Then
    '                Print #1, strHypLine
    '                    intLnCtr = intLnCtr + 1
    '                End If
    '                If .IsSubtotal(intR) Then
    '                Print #1, strFontChar & prntBold & strLine & prntundoBold
    '                Else
    '                Print #1, strFontChar & strLine
    '                End If
    '                If .IsSubtotal(intR) Then
    '                Print #1, strHypLine
    '                    intLnCtr = intLnCtr + 1
    '                End If
    '                intLnCtr = intLnCtr + 1
    '            Next intR
    '        Print #1, strHypLine
    '        End With
    '    Print #1, "Page : " & intPgCnt & "          (Report Dt: " & Format(Date, "mmm dd yyyy") & "  " & Format(Time, "h:m:s ampm") & ")"
    '    End Sub

    '    Private Function fnJustify(ByVal argLen As Integer, ByVal argVal As String, ByVal argFldAlignment As String) As String
    '        fnJustify = argVal
    '        Select Case argFldAlignment
    '            Case "Left"
    '                fnJustify = clsSvt.PadStr(argVal, CLng(argLen), vbLeftJustify)
    '            Case "Right"
    '                fnJustify = clsSvt.PadStr(argVal, CLng(argLen), vbRightJustify)
    '            Case "Center"
    '                fnJustify = clsSvt.PadStr(argVal, CLng(argLen), vbCenter)
    '        End Select
    '    End Function

    '    Private Function getPgHdr() As String
    '        Dim intR As Integer, intC As Integer, i As Integer
    '        Dim strVal As String
    '        Dim strLine As String
    '        With vsfView
    '            For intR = 0 To .Rows - 1
    '                strLine = ""
    '                strVal = ""
    '                For intC = 0 To .Cols - 1
    '                    For i = 1 To vsfLayoutInfo.Rows - 1
    '                        If .TextMatrix(0, intC) = vsfLayoutInfo.TextMatrix(i, vLayColName) Then
    '                            If vsfLayoutInfo.Cell(flexcpChecked, i, vLayColPrint) = flexChecked Then
    '                                If vsfLayoutInfo.Cell(flexcpChecked, i, vLayPrintAsGroup) <> flexChecked Then
    '                                    strVal = vsfLayoutInfo.TextMatrix(i, vLayColcaptionName)
    '                                    'strVal = .TextMatrix(intR, intC)
    '                                    strVal = fnJustify(Val(vsfLayoutInfo.TextMatrix(i, vLayColWidth)), _
    '                                            strVal, vsfLayoutInfo.TextMatrix(i, vLayColAlignment))
    '                                    strVal = strVal & Space(1)
    '                                    strLine = strLine & strVal
    '                                End If
    '                            End If
    '                            Exit For
    '                        End If
    '                    Next i
    '                Next intC
    '                getPgHdr = prntBold & strLine & prntundoBold
    '                Exit For
    '            Next intR
    '        End With
    '    End Function
    '    ''''' for customisation

    '    Private Function getColList() As String
    '        Dim strSQL As String, i As Integer

    '        ReDim fldarrr(vsfView.Cols - 1)
    '        strSQL = argQry

    '        strSQL = Replace(UCase$(strSQL), UCase$("Format("), "")
    '        strSQL = Replace(UCase$(strSQL), UCase$(", 'dd mmm yyyy')"), "")

    '        strSQL = Mid(strSQL, 8, InStr(1, UCase$(strSQL), UCase$(" from")) - 7)

    '        fldArr = Split(strSQL, ",")

    '        getColList = Replace(strSQL, ",", "|")

    '    End Function
    '    Private Sub vsfXlsFormat_BeforeEdit(ByVal Row As Long, ByVal Col As Long, ByVal Cancel As Boolean)
    '        'if col=vXlsSel then Cancel
    '    End Sub

    '    Private Sub vsfXlsFormat_BeforeSort(ByVal Col As Long, ByVal Order As Integer)
    '        If Col = vXlsSel Then
    '            Dim i As Integer
    '            With vsfXlsFormat
    '                For i = 1 To .Rows - 1
    '                    .Col = 0
    '                    .Row = i
    '                    If .CellChecked = flexChecked Then
    '                        .CellChecked = flexUnchecked
    '                        .TextMatrix(i, 4) = "N"
    '                    Else
    '                        .CellChecked = flexChecked
    '                        .TextMatrix(i, 4) = "Y"
    '                    End If
    '                Next
    '            End With
    '        End If
    '    End Sub
    '    Private Sub vsfXlsFormat_EnterCell()
    '        vsfXlsFormat.EditCell()
    '    End Sub
    '    Private Sub vsfXlsFormat_KeyUp(ByVal KeyCode As Integer, ByVal Shift As Integer)
    '        With vsfXlsFormat
    '            If KeyCode = vbKeyTab And .Col = .Cols - 1 And .Row = .Rows - 1 Then
    '                .Col = vXlsValue
    '                .Rows = .Rows + 1
    '                .Row = .Rows - 1
    '                .Cell(flexcpChecked, .Row, vXlsSel) = flexChecked
    '                .EditCell()
    '                Exit Sub
    '            End If
    '            If KeyCode = vbKeyEscape Then
    '                Call RemoveEmptVSFRows(vsfXlsFormat, 1, vXlsValue)
    '                cmdXlsOk.SetFocus()
    '                Exit Sub
    '            End If
    '        End With
    '    End Sub
    '    Private Sub vsfXlsFormat_MouseDown(ByVal Button As Integer, ByVal Shift As Integer, ByVal x As Single, ByVal Y As Single)
    '        If vsfXlsFormat.MouseCol = vXlsSel Then
    '        End If
    '    End Sub

    '    Private Sub fillRptLayoutName()
    '        Call clsSvt.FillCombo("UtlDosRptLayout", "Distinct rptname", "", cboLayout, _
    '                "where rptTag = '" & strTag1 & "'", "order by 1", True, False)
    '        Call clsSvt.FillCombo("UtlDosRptLayout", "Distinct rptname", "", cboLayoutName, _
    '                "where rptTag = '" & strTag1 & "'", "order by 1", True, False)
    '    End Sub

    '    Private Sub showDet()
    '        Dim strPicFilePath As String
    '        Dim strFormatString As String, strSQL As String
    '        Dim strItemcode As String, strImage As String
    '        Dim lngTemp As Long
    '        With vsfView
    '            If .Rows <= 1 Then Exit Sub
    '            If .Row = 0 Then Exit Sub
    '            Select Case Trim(strRptTag)
    '                Case "General Ledger"
    '                    Call showScreen(.TextMatrix(.Row, 2), .TextMatrix(.Row, 3))
    '                Case "Receivable Report", "Payable Report", "Receivable Ageing", "Payable Ageing", "Day Book"
    '                    Call showScreen(.TextMatrix(.Row, 1), .TextMatrix(.Row, 2))
    '                Case "Register"
    '                    Call showScreen(.TextMatrix(.Row, 0), .TextMatrix(.Row, 1))
    '                Case "VchListing"
    '                    Call showScreen(strRptVchName, .TextMatrix(.Row, 2))
    '            End Select


    '        End With
    '    End Sub

    '    Private Sub showScreen(ByVal argName As String, ByVal argDocNo As String)
    '        If Trim(argName) = "" Then Exit Sub
    '        Dim intDocID As Integer, strType As String
    '        intDocID = clsSvt.GetDataValue("DocID", 0, "MstMenu", False, "Where MenuItemcaption = '" & argName & "'")
    '        If intDocID = 0 Then
    '            intDocID = clsSvt.GetDataValue("DocID", 0, "UtlTransactions", False, "Where DocShForm = '" & argName & "'")
    '        End If
    '        If intDocID = 0 Then Exit Sub
    '        strType = clsSvt.GetDataValue("DocType", 0, "UtlTransactions", False, "Where DocID = " & intDocID)
    '        If strType = "Transaction" Then
    '            Call showTrnScreen(intDocID, argDocNo)
    '        End If
    '        If strType = "Voucher" Then
    '            Call showVchScreen(strRptVchName, argDocNo)
    '        End If
    '    End Sub

    '    Private Sub showVchScreen(ByVal argName As String, ByVal argDocNo As String)
    '        Dim intDocID As Integer
    '        intDocID = clsSvt.GetDataValue("DocID", 0, "MstMenu", False, "Where MenuItemcaption = '" & argName & "'")
    '        If intDocID = 0 Then Exit Sub
    '        Dim frmVch As New FrmTrnVchEntry
    '        With frmVch
    '            .argDocID = intDocID
    '            .Show()
    '            .FillDocDetails(argDocNo)
    '        End With
    '    End Sub

    '    Private Sub showTrnScreen(ByVal argDocID As Integer, ByVal argDocNo As String)
    '        Dim rsDummy As New ADODB.Recordset
    '        Dim cmdDummy As New ADODB.Command
    '        Dim strTrnName As String
    '        With cmdDummy
    '            .ActiveConnection = svtConn
    '            .CommandText = "Select * from UtlTransactions where DocID = " & argDocID
    '            rsDummy = .Execute
    '        End With

    '        With rsDummy
    '            If Not .EOF Then
    '                strTrnName = !DocName
    '                If clsSvt.GetVal(!LinkedFROMDoc, "N") = "N" Then
    '                    Select Case clsSvt.GetVal(!NonItemDoc, "Y")
    '                        Case "N" 'if it contains item
    '                            If clsSvt.GetVal(!CustSuppReq, "N") = "C" Or clsSvt.GetVal(!CustSuppReq, "N") = "S" Then
    '                                Dim frmTrnPartyEntry As New FrmTrnPartyDocEntry
    '                                With frmTrnPartyEntry
    '                                    .argDocID = argDocID
    '                                    .Show()
    '                                    .FillDocDetails(argDocNo)
    '                                End With
    '                            Else
    '                                Dim frmDocEntry As New FrmTrnDocEntry
    '                                With frmDocEntry
    '                                    .argDocID = argDocID
    '                                    .Show()
    '                                    .FillDocDetails(argDocNo)
    '                                End With
    '                            End If

    '                        Case "Y" 'if it DOES NOT contains item
    '                            Select Case clsSvt.GetVal(!NonItemDetSecPresent, "Y")
    '                                Case "Y"    'if det section present
    '                                    If clsSvt.GetVal(!CustSuppReq, "N") = "C" Or clsSvt.GetVal(!CustSuppReq, "N") = "S" Then
    '                                        Dim frmTrnPartyEntryNonItem As New FrmTrnPartyDocNonItemEntry
    '                                        With frmTrnPartyEntryNonItem
    '                                            .argDocID = argDocID
    '                                            .Show()
    '                                            .FillDocDetails(argDocNo)
    '                                        End With
    '                                        Exit Sub
    '                                    Else
    '                                        '# A - 24032008
    '                                        Dim frmTrnEntryNonItem As New FrmTrnDocNonItemEntry
    '                                        With frmTrnEntryNonItem
    '                                            .argDocID = argDocID
    '                                            .Show()
    '                                            .FillDocDetails(argDocNo)
    '                                        End With
    '                                        Exit Sub
    '                                    End If
    '                                Case "N"    'if det section is not present
    '                                    'otherwise call frt entry type form

    '                                    Dim strFrmName As String
    '                                    strFrmName = clsSvt.GetDataValue("FrmName", "", "MstMenu", False, _
    '                                                " Where MenuItemCaption='" & strTrnName & "'")
    '                                    If strFrmName = "" Then
    '                                        Dim frmNonItemDocEntry As New FrmTrnNonItemDocEntry
    '                                        With frmNonItemDocEntry
    '                                            .argDocID = argDocID
    '                                            .Show()
    '                                            .FillDocDetails(argDocNo)
    '                                        End With
    '                                    Else
    '                                        '                                    Select Case strFrmName
    '                                        '                                        Case "FrmTrnBond"
    '                                        '                                            With FrmTrnBondEntry
    '                                        '                                                .argDocID = argDocID
    '                                        '                                                .Show
    '                                        '                                                .FillDocDetails (argDocNo)
    '                                        '                                            End With
    '                                        '                                        Case "FrmTrnDeBond"
    '                                        '                                            With FrmTrnDeBondEntry
    '                                        '                                                .argDocID = argDocID
    '                                        '                                                .Show
    '                                        '                                                .FillDocDetails (argDocNo)
    '                                        '                                            End With
    '                                        '                                        Case Else
    '                                        '                                            Exit Sub
    '                                        '                                    End Select
    '                                    End If
    '                            End Select
    '                            '                            Case "N"    'if det section is not present
    '                            '                                'otherwise call frt entry type form
    '                            '                                Dim frmNonItemDocEntry As New FrmTrnNonItemDocEntry
    '                            '                                With frmNonItemDocEntry
    '                            '                                    .argDocID = intDocID
    '                            '                                    .Show
    '                            '                                End With
    '                            '                        End Select
    '                    End Select
    '                End If

    '                If clsSvt.GetVal(!LinkedFROMDoc, "N") = "Y" Then
    '                    If clsSvt.GetVal(!CustSuppReq, "N") = "C" Or clsSvt.GetVal(!CustSuppReq, "N") = "S" Then
    '                        Dim frmTrnPartyPending As New FrmTrnPartyPendingDocEntry
    '                        With frmTrnPartyPending
    '                            .argDocID = argDocID
    '                            .Show()
    '                            .FillDocDetails(argDocNo)
    '                        End With
    '                    Else
    '                        Dim frmTrnPending As New FrmTrnPendingDocEntry
    '                        With frmTrnPending
    '                            .argDocID = argDocID
    '                            .Show()
    '                            .FillDocDetails(argDocNo)
    '                        End With
    '                    End If
    '                    Exit Sub

    '                End If
    '            End If
    '        End With
    '        rsDummy = Nothing
    '        cmdDummy = Nothing
    '    End Sub

    '    Private Sub cmdPrinterPrint_Click()
    '        vsfView.PrintGrid(strRptTag & "1.pdf", True, , 0.5, 0.5)
    '    End Sub


    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub btnExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        If vsfView.Rows.Count < 1 Then
            Exit Sub
        End If
        ExportToXL(vsfView)
    End Sub

    Private Sub vsfView_AfterScroll(sender As Object, e As RangeEventArgs) Handles vsfView.AfterScroll
        vsfSearch.ScrollPosition = vsfView.ScrollPosition
        Call syncGridCols()
    End Sub

    Private Sub vsfSearch_AfterScroll(sender As Object, e As RangeEventArgs) Handles vsfSearch.AfterScroll
        'Dim intv As Integer = vsfSearch.ScrollPosition.X
        'Dim inth As Integer = vsfView.ScrollPosition.Y
        'vsfView.ScrollPosition = New System.Drawing.Point(intv, inth)
        vsfSearch.ScrollPosition = vsfView.ScrollPosition
    End Sub

    Private Sub vsfSearch_ChangeEdit(sender As Object, e As EventArgs) Handles vsfSearch.ChangeEdit
        Call applyFilterFromGrid()
    End Sub

    Private Sub vsfView_AfterResizeColumn(sender As Object, e As RowColEventArgs) Handles vsfView.AfterResizeColumn
        Call syncGridCols()
    End Sub

    Private Sub vsfSearch_RowColChange(sender As System.Object, e As System.EventArgs) Handles vsfSearch.RowColChange
        vsfView.Col = vsfSearch.Col
        vsfView.Refresh()
    End Sub

    Private Function applyFilterFromGrid() As Boolean
        Dim StrQry As String, strValue As String
        Dim i As Integer

        applyFilterFromGrid = False
        StrQry = ""
        With vsfSearch
            For i = 0 To .Cols.Count - 1
                strValue = ""
                If i = vsfSearch.ColSel Then
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

                    If vsfView.Cols(i).DataType = GetType(Decimal) Or vsfView.Cols(i).DataType = GetType(Short) Or vsfView.Cols(i).DataType = GetType(Date) Or vsfView.Cols(i).DataType = GetType(Int32) Or vsfView.Cols(i).DataType = GetType(Int16) Or vsfView.Cols(i).DataType = GetType(Byte) Then
                        StrQry = StrQry & "Convert(" & vsfView.GetData(0, i) & ",'System.String')" & " like '" & strValue & "*'"
                    Else
                        StrQry = StrQry & vsfView.GetData(0, i) & " like '*" & strValue & "*'"
                    End If
                End If
            Next i
        End With
        dtView.RowFilter = StrQry
        Call syncGridCols()
        applyFilterFromGrid = True
    End Function
End Class