Imports C1.Win.C1FlexGrid

Public Class frmUtlReceivedNotes

    Const vDetDate As Byte = 0
    Const vDetSel As Byte = 1
    Const vdetFrom As Byte = 2
    Const vdetNoteID As Byte = 3
    Const vDetNotes As Byte = 4

    Dim clsSVT As New svtLibrary
    Private Sub initScreen()
        With vsfdet
            .Rows.Count = 1
            .Cols.Count = 5

            .SetData(0, vDetSel, "Sel")
            .SetData(0, vDetDate, "Date")
            .SetData(0, vdetFrom, "From")
            .SetData(0, vDetNotes, "Message")
            .Rows.Fixed = 0
            Call SetHideCol(vsfdet, True, vdetNoteID, vDetSel)
            .Cols(vDetSel).DataType = GetType(Boolean)
            Call SetColWidth(vsfdet, 200, 200, 200, 200, 200)
        End With
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub btnExport_Click(sender As Object, e As EventArgs) Handles btnExport.Click
        If vsfdet.Rows.Count = 1 Then
            MsgBox("No Data to export", vbInformation + vbOKOnly, Me.Text)
            Exit Sub
        End If
        Call ExportToXL(vsfdet)
    End Sub
    Public Function fillData() As Boolean
        Dim rsDtl As oledb.oledbDataReader
        Dim iRow As Integer, strsql As String

        '    strsql = "Select RemindOnDate,  h.noteid,Fromuser, Notes from UtlRemNotes h, UtlRemNotesDtl d"
        '    strsql = strsql & " where h.NoteId = d.NoteID "
        '    strsql = strsql & " and '" & gUsrCode & "' = d.UserCode "
        '    strsql = strsql & " and RemindOnDate <= " & getDateFormatInCond(getSystemDate) & ""
        '    'strsql = strsql & " order by RemindOnDate desc, h.noteID desc"
        '    strsql = strsql & " union all "
        strsql = " Select getdate() as RemindONDate, 0 as NoteID, 'SO' as Fromuser,"
        strsql = strsql & "SONO + ' [' + custName + ']'"
        strsql = strsql & " + char(10) + ItemCode + ' [' + convert(varchar,SchDate,106) + '] [Qty:' + convert(varchar,SOQty) + ']' as Notes"
        'h.SONO, DisplayName as custName, ItemCode, SchDate, SOQty, d.RMKRS as Dremarks"
        strsql = strsql & " from trndtlso d, trnhdrso h, mstcust c, mstitem i"
        strsql = strsql & " Where h.soid = d.soid"
        strsql = strsql & " and d.itemid = i.itemid"
        strsql = strsql & " and h.CustID =c.custid"
        'strsql = strsql & " and '" & gUserCode & "' in (select usercode from utlSoAlert)"
        strsql = strsql & " and alertdate = '" & Format(getSystemDate, "dd MMM yyyy") & "'"
        strsql = strsql & " order by 1,2"
        rsDtl = clsSVT.getDataReaderFromSQL(strsql)

        With rsDtl
            If Not .HasRows Then
                fillData = False
                .Close()
            End If

            vsfdet.Rows.Count = 1
            Do While .Read
                vsfdet.Rows.Count = vsfdet.Rows.Count + 1
                iRow = vsfdet.Rows.Count - 1
                vsfdet.SetData(iRow, vDetDate, rsDtl("RemindOnDate"))
                vsfdet.SetData(iRow, vdetFrom, rsDtl("Fromuser"))
                vsfdet.SetData(iRow, vdetNoteID, rsDtl("NoteId"))
                vsfdet.SetData(iRow, vDetNotes, rsDtl("Notes"))

            Loop
            .Close()
        End With

        fillData = True

        With vsfdet
            .AllowMerging = AllowMergingEnum.RestrictAll
            .Cols(vDetDate).AllowMerging = True
            vsfdet.AutoSizeRows()
            .Styles(C1.Win.C1FlexGrid.CellStyleEnum.Normal).WordWrap = True
            .AutoResize = True
            .AutoSizeCols()
        End With
    End Function

    Private Sub vsfdet_DoubleClick(sender As Object, e As EventArgs) Handles vsfdet.DoubleClick
        Dim strSQL As String
        If vsfdet.Row <= 0 Then Exit Sub
        If vsfdet.GetData(vsfdet.Row, vdetNoteID) <= 0 Then Exit Sub

        If vsfdet.GetData(vsfdet.Row, vdetNoteID) <> "" Then
            If MsgBox("Do You want to Remove the highlighted Message ?", vbQuestion + vbYesNo, Me.Text) = vbYes Then
                strSQL = "Delete from UtlRemNotesDtl where NoteId =" & Val(vsfdet.GetData(vsfdet.Row, vdetNoteID))
                strSQL = "Delete from UtlRemNotes where NoteId not in (Select NoteID from UtlRemNotesDtl)"
                vsfdet.RemoveItem(vsfdet.Row)
                MsgBox("Removed Note!", vbInformation, Me.Text)
            End If
        End If
    End Sub

    Private Sub vsfdet_BeforeEdit(sender As Object, e As RowColEventArgs) Handles vsfdet.BeforeEdit
        If vsfdet.Col <> vDetSel Then
            e.Cancel = True
            Exit Sub
        End If
        If vsfdet.GetData(vsfdet.Row, vdetNoteID) <= 0 Then
            e.Cancel = True
            Exit Sub
        End If
    End Sub

    Private Sub frmTrnRemainder_KeyUp(sender As Object, e As KeyEventArgs) Handles Me.KeyUp
        If e.KeyCode = Keys.Escape Then
            Exit Sub
        End If
    End Sub

    Private Sub frmTrnRemainder_Load(sender As Object, e As EventArgs) Handles Me.Load
        Me.KeyPreview = True
        Call initScreen()
        fillData()
    End Sub

    'Private Sub ChkRead_Click()
    '    Dim i As Integer, blnFound As Boolean
    '    With vsfdet
    '        For i = 1 To .Rows - 1
    '            .RowHidden(i) = False
    '            If CDate(.TextMatrix(i, vDetDate)) <> CDate(getSystemDate()) Then
    '                If ChkRead.Value = vbChecked Then
    '                    .RowHidden(i) = True
    '                End If
    '            End If
    '        Next i
    '    End With
    'End Sub

    '    Private Sub cmdDismiss_Click()
    '        Dim i As Integer, blnFound As Boolean
    '        blnFound = False
    '        With vsfdet
    'begin:
    '            For i = 1 To .Rows - 1
    '                If .Cell(flexcpChecked, i, vDetSel) = flexChecked Then
    '                    svtConn.Execute "Delete from UtlRemNotesDtl where UserCode= '" & gUsrCode & "' and  NoteId =" & Val(vsfdet.TextMatrix(vsfdet.Row, vdetNoteID))
    '                svtConn.Execute "Delete from UtlRemNotes where NoteId not in (Select NoteID from UtlRemNotesDtl)"
    '                vsfdet.RemoveItem(vsfdet.Row)
    '                    blnFound = True
    '                    GoTo begin
    '                End If
    '            Next i
    '        End With
    '        If Not blnFound Then
    '            MsgBox "Please select a message to Remove!", vbCritical, Me.Caption
    '        Exit Sub
    '        Else
    '            MsgBox "Marked messages are Removed!", vbCritical, Me.Caption
    '        Exit Sub
    '        End If
    '    End Sub

    '    Private Sub cmdDueNotes_Click()
    '        Dim frmUS As New frmUtlSelection
    '        Dim strsql As String

    '        txtNoteID.Text = ""

    '        strsql = "SELECT  NoteID, RemindOndate, Notes"
    '        strsql = strsql & " FROM UtlRemNotes"
    '        strsql = strsql & " where Fromuser = '" & gUsrCode & "'"
    '        strsql = strsql & " and RemindOnDate > '" & Format(getSystemDate, "dd mmm yyyy") & "'"
    '        strsql = strsql & " order by 1"
    '        With frmUS
    '            Call .InitVsfSel(strsql, Me, txtNoteID)
    '            Call clsSVT.SetColWidth(.vsfView, 75, 960, 1455)
    '            Call .syncGridCols
    '        End With
    '        frmUS.Show vbModal
    '    If Val(txtNoteID.Text) > 0 Then
    '            With frmUtlRemNotes
    '                .txtNoteID = txtNoteID.Text
    '                .initFOrm
    '                Call .FillDocData(Val(txtNoteID.Text))
    '                .Show vbModal
    '        End With
    '        End If
    '    End Sub

    '    Private Sub cmdNew_Click()
    '        frmUtlRemNotes.initFOrm
    '        frmUtlRemNotes.Show vbModal
    'End Sub

End Class