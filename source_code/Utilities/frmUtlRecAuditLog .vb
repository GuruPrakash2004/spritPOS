Imports System.ComponentModel
Imports System.Data.oledb
Imports C1.Win.C1FlexGrid

Public Class frmUtlRecAuditLog

    Dim clsSVT As New svtLibrary
    Dim mode As String
    Dim strTable As String

    Const vsfMnthyear = 0
    Const vsfMnth = 1
    Const vsfRecNo = 2

    Dim strSHForm As String

    Dim dt As DataTable
    Dim dv As DataView

    Dim strMstTblName As String = ""
    Dim strTrnHdrTblName As String = ""
    Dim strTrnDtlTblName As String = ""
    '
    Private Sub initScreen()
        Call initFormSize(Me, Me.Text, False, 855, 515, 0, 0)
        chkTrans.Checked = True
        chkTrans.Checked = False
        With vsfDet
            .Rows.Count = 1
            .Cols(3).DataType = GetType(DateTime)
        End With
        With vsfRec
            .Cols.Count = 3
            .Rows.Count = 2

            .SetData(0, vsfMnthyear, "Month")
            .SetData(0, vsfMnth, "Month-Year")
            .SetData(0, vsfRecNo, "NoOf Records")
            Call SetHideCol(vsfRec, True, vsfMnthyear)
            Call SetFlexColor(vsfRec, "", vsfMnth, vsfRecNo)
            vsfRec.AutoSizeCols()
        End With
        cboMaster.SelectedIndex = -1
        initGridStyles(Me)
        fillFormProperties(Me)
    End Sub

    Public Sub fillTRNDtls()
        Dim RsTemp As oledb.oledbDataReader
        Dim strSQL As String
        Dim i As Integer
        Dim j As Integer

        strSQL = " select * from " & gStrLogDBName & ".." & strMstTblName & " where 1=1"
        If chkDate.Checked Then
            strSQL = strSQL & " and ActionDateTime between '" & Format(dtpFromDate.Value, "yyyy-MM-dd") & " 00:00:00' and '" & Format(dtpToDate.Value, "yyyy-MM-dd") & " 23:59:00'"
        End If
        RsTemp = clsSVT.getDataReaderFromSQL(strSQL)

        With vsfDet
            .Rows.Count = 1

            Do While RsTemp.Read()
                i = .Rows.Count
                .Rows.Count = .Rows.Count + 1
                For j = 0 To .Cols.Count - 1
                    .SetData(i, j, RsTemp(.GetData(0, j)))
                Next j
                i = i + 1
            Loop

            Call vsfDet.AutoSizeCols()
            RsTemp.Close()
        End With
        txtNoOfInv.Text = vsfDet.Rows.Count - 1
    End Sub

    Private Sub frmUtlAuditLog_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        chkTrans.Focus()
        Call initScreen()
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Dim tb As New C1.Win.C1Command.C1DockingTabPage()
        Dim dockTabMain As New C1.Win.C1Command.C1DockingTab()
        Me.Dispose()
        tb.Controls.Clear()
        frmMain.dockTabMain.SelectedTab.Dispose()
    End Sub

    Private Sub vsfDet_BeforeEdit(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfDet.BeforeEdit, vsfChild.BeforeEdit, vsfChild2.BeforeEdit
        e.Cancel = True
    End Sub

    Private Sub btnView_Click(sender As Object, e As EventArgs) Handles btnView.Click
        If chkDate.Checked Then
            If dtpFromDate.Value > dtpToDate.Value Then
                MsgBox("From date Should Be less than To Date!", vbInformation, Me.Text)
                Exit Sub
            End If
        End If

        Dim rs As oledbDataReader
        Dim rsTemp As oledbDataReader
        Dim i As Integer
        Dim strSql As String
        Dim strStartFinYr As String

        If cboMaster.Text = "" Then
            MsgBox("Master field Should Not be empty!!", MsgBoxStyle.Information, Me.Text)
            cboMaster.Focus()
            Exit Sub
        End If

        vsfRec.Rows.Count = 1
        strStartFinYr = CStr(clsSVT.getDataDateValue("select YrFdate from MstCompFinYear where Active='Y'", getSystemDate, False))

        i = 13
        vsfRec.Rows.Count = 13
        For j = 0 To 12 - 1
            vsfRec.SetData(i - 1, vsfMnthyear, (Format(DateAdd("M", j, CDate(strStartFinYr)), "MMM")) & "-" & (Format(DateAdd("M", j, CDate(strStartFinYr)), "yyyy")))
            i = i - 1
        Next j

        strSql = " Select substring(datename(Month,ActionDateTime),1,3) + '-' + DATENAME(year,actiondatetime) as Month, Count(*) as NoOfRec "
        strSql = strSql & " From " & gStrLogDBName & ".." & strMstTblName & " Where 1=1 "
        If chkDate.Checked Then
            strSql = strSql & " and ActionDateTime between '" & Format(dtpFromDate.Value, "yyyy-MM-dd") & " 00:00:00' and '" & Format(dtpToDate.Value, "yyyy-MM-dd") & " 23:59:00'"
        End If
        strSql = strSql & " group by substring(datename(Month,ActionDateTime),1,3) + '-' + DATENAME(year,actiondatetime)  "
        strSql = strSql & " order by 1 "


        rsTemp = clsSVT.getDataReaderFromSQL(strSql)

        'vsfRec.Rows.Count = 1
        With rsTemp
            If Not .HasRows Then
                MsgBox("No Records found", MsgBoxStyle.Information, Me.Text)
                Exit Sub
            End If
            i = 1
            Do While .Read()

                For i = 1 To vsfRec.Rows.Count - 1
                    If vsfRec.GetData(i, vsfMnthyear) = GetVal(rsTemp("Month"), "") Then
                        vsfRec.Rows.Count = vsfRec.Rows.Count + 1
                        vsfRec.SetData(i, vsfMnth, rsTemp("Month"))
                        vsfRec.SetData(i, vsfRecNo, rsTemp("NoOfRec"))
                        i = i + 1
                    End If
                Next i
            Loop
        End With
        For i = 1 To vsfRec.Rows.Count - 1
            If Val(vsfRec.GetData(i, vsfRecNo)) = 0 Then
                vsfRec.Rows(i).Visible = False
            End If
        Next
        strSql = "Select column_name from " & gStrLogDBName & ".information_schema.columns where table_name ='" & strMstTblName & "'"
        rs = clsSVT.getDataReaderFromSQL(strSql)

        With rs
            vsfDet.Cols.Count = 0
            i = 0
            Do While .Read()
                vsfDet.Cols.Count = vsfDet.Cols.Count + 1
                vsfDet.SetData(0, i, rs(0))
                i = i + 1
            Loop
            .Close()
        End With
        Call fillTRNDtls()

        Call SetHideCol(vsfDet, True, 0)
        vsfDet.Cols.Frozen = 0       '------Changed
    End Sub

    Private Sub cboMaster_Validating(sender As Object, e As CancelEventArgs) Handles cboMaster.Validating
        If cboMaster.Text = "" Then Exit Sub
        Dim blnFound As Boolean
        strMstTblName = clsSVT.getDataStringValue("select HdrTblName from UtlTransactions where DocName='" & Trim(cboMaster.Text) & "'", "", blnFound)

        If Not blnFound Then
            MsgBox("Master Not found in the list!!", MsgBoxStyle.Information, Me.Text)
            cboMaster.Focus()
            Exit Sub
        End If
        strSHForm = clsSVT.getDataStringValue("select IsNull(DocShForm,'') from UtlTransactions where DocName='" & Trim(cboMaster.Text) & "'", "", blnFound)
    End Sub

    Private Sub vsfRec_BeforeEdit(sender As Object, e As RowColEventArgs) Handles vsfRec.BeforeEdit
        e.Cancel = True
    End Sub

    Private Sub btnExcel_Click(sender As Object, e As EventArgs) Handles btnExcel.Click
        Call ExportToXL(vsfDet)
    End Sub

    Private Sub frmUtlRecAuditLog_KeyUp(sender As Object, e As KeyEventArgs) Handles Me.KeyUp
        If e.KeyCode = Keys.F9 And e.Control = True Then
            Call initFormSize(Me, Me.Text, False, 855, 515, 0, 0)
            MsgBox("Restored Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
        If e.KeyCode = Keys.F8 And e.Control = True Then
            Call SaveActFormSettings(Me, Me.Text)
            Call SaveActFormVsfDet(Me, Me.Text, "Excel", vsfDet)
            MsgBox("Saved Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
        'If e.KeyCode = Keys.Tab Then
        'chkTrans.Focus()
        'End If
    End Sub

    Private Sub btnFreeze_Click(sender As Object, e As EventArgs) Handles btnFreeze.Click
        Dim tooltip As New ToolTip
        If btnFreeze.Text = "&Freeze" Then
            tooltip.SetToolTip(Me.btnFreeze, "UnFreeze : Alt+U")
            btnFreeze.Text = "&UnFreeze"
            vsfDet.Cols.Frozen = vsfDet.Col
        Else
            btnFreeze.Text = "&Freeze"
            tooltip.Active = False
            tooltip.SetToolTip(Me.btnFreeze, "Freeze : Alt+F")
            tooltip.Active = True
            vsfDet.Cols.Frozen = 0
        End If
    End Sub

    Private Sub vsfDet_DoubleClick(sender As Object, e As EventArgs) Handles vsfDet.DoubleClick
        If vsfDet.Rows.Count <= 1 Then Exit Sub
        If vsfDet.Row < 0 Then Exit Sub

        Dim intCol As Int32 = -1
        Dim strCol As String = ""

        Select Case strTable.ToUpper()
            Case "MSTCUST", "MSTSUPP"
                strCol = "RECID"
            Case "TRNHDRCPO"
                strCol = "CPOID"
            Case "TRNHDRCSHP"
                strCol = "CSHPID"
            Case "TRNHDRDC"
                strCol = "DCID"
            Case "TRNHDRDCR"
                strCol = "DCRID"
            Case "TRNHDRDPO"
                strCol = "DPOID"
            Case "TRNHDRGRN"
                strCol = "GRNID"

            Case "TRNHDRIGV"
                strCol = "IGVID"
            Case "TRNHDRINV"
                strCol = "INVID"
            Case "TRNHDRIQTN"
                strCol = "IQTNID"
            Case "TRNHDRLQTN"
                strCol = "LQTNID"
            Case "TRNHDRPINV"
                strCol = "PINVID"
            Case "TRNHDRPMT"
                strCol = "PMTID"
            Case "TRNHDRPO"
                strCol = "POID"
            Case "TRNHDRSA"
                strCol = "SAID"
            Case "TRNHDRSEPO"
                strCol = "SEPOID"
            Case "TrnHDRSO"
                strCol = "SOID"
            Case "TRNHDRSR"
                strCol = "SRID"
            Case "TRNHDRSSUB"
                strCol = "SSUBID"
            Case "TRNHDRSO"
                strCol = "SOID"
            Case "TrnHDRGRN"
                strCol = "GRNID"
            Case "TRNHDRPO"
                strCol = "POID"
            Case "TRNHDRINV"
                strCol = "INVID"

        End Select

        If strCol = "" Then Exit Sub

        With vsfDet
            For i As Int32 = 0 To .Cols.Count - 1
                If strCol = UCase(GetVal(.GetData(0, i), "")) Then
                    intCol = i
                    Exit For
                End If
            Next
        End With
        If intCol = -1 Then Exit Sub

        vsfChild2.Visible = True
        ' vsfChild.Size = New Point(vsfChild.Size.Height + vsfChild2.Size.Height, vsfChild.Size.Width)
        Select Case strTable.ToUpper()
            Case "MSTCUST"
                loadChild("MstPartyAddress", strCol, Val(vsfDet.GetData(vsfDet.Row, intCol)), " and MstDocID=7 ")
                loadChild2("MstPartyContacts", strCol, Val(vsfDet.GetData(vsfDet.Row, intCol)), " and MstDocID=7 ")
            Case "MSTSUPP"
                loadChild("MstPartyAddress", strCol, Val(vsfDet.GetData(vsfDet.Row, intCol)), " and MstDocID=10 ")
                loadChild2("MstPartyContacts", strCol, Val(vsfDet.GetData(vsfDet.Row, intCol)), " and MstDocID=10 ")
                loadChild2("MstPartyContacts", strCol, Val(vsfDet.GetData(vsfDet.Row, intCol)), " and MstDocID=10 ")
            Case "TRNHDRCPO", "TRNHDRCSHP", "TRNHDRDC", "TRNHDRDCR", "TRNHDRDPO", "TRNHDRGRN", "TRNHDRIGV", "TRNHDRINV", "TRNHDRIQTN",
                 "TRNHDRLQTN", "TRNHDRPINV", "TRNHDRPMT", "TRNHDRPO", "TRNHDRSA", "TRNHDRSEPO", "TrnHDRSO", "TRNHDRSR", "TRNHDRSSUB",
                 "TRNHDRSO", "TRNHDRPO", "TRNHDRGRN", "TrnHdrINV"
                loadChild("TrnHdr" & strSHForm, strCol, Val(vsfDet.GetData(vsfDet.Row, intCol)), "")
                loadChild2("TrnDtl" & strSHForm, strCol, Val(vsfDet.GetData(vsfDet.Row, intCol)), "")
        End Select

        pnlDet.Visible = True
        pnlDet.BringToFront()
    End Sub

    Private Sub loadChild(ByVal argTbl As String, ByVal argIDCol As String, ByVal argID As Int32, Optional ByVal argCond As String = "")
        Dim strSQL As String
        strSQL = "Select * from " & gStrLogDBName & ".." & argTbl & " where " & argIDCol & " = " & argID & argCond & " Order by ActionDateTime"


        Cursor.Current = Cursors.WaitCursor
        dt = clsSVT.getDATATable(strSQL)
        dv = New DataView(dt)

        vsfChild.Rows.Fixed = 1
        vsfChild.Rows.GlyphRow = 1
        vsfChild.BeginUpdate()
        vsfChild.DataSource = dv
        vsfChild.EndUpdate()
        Cursor.Current = Cursors.Arrow
    End Sub

    Private Sub loadChild2(ByVal argTbl As String, ByVal argIDCol As String, ByVal argID As Int32, Optional ByVal argCond As String = "")
        Dim strSQL As String
        strSQL = "Select * from " & gStrLogDBName & ".." & argTbl & " where " & argIDCol & " = " & argID & argCond & " Order by ActionDateTime"


        Cursor.Current = Cursors.WaitCursor
        dt = clsSVT.getDATATable(strSQL)
        dv = New DataView(dt)

        vsfChild2.Rows.Fixed = 1
        vsfChild2.Rows.GlyphRow = 1
        vsfChild2.BeginUpdate()
        vsfChild2.DataSource = dv
        vsfChild2.EndUpdate()
        Cursor.Current = Cursors.Arrow
    End Sub

    Private Sub btnCloseAddItems_Click(sender As Object, e As EventArgs) Handles btnCloseAddItems.Click
        pnlDet.Visible = False
    End Sub

    Private Sub chkTrans_CheckedChanged(sender As Object, e As EventArgs) Handles chkTrans.CheckedChanged
        If chkTrans.Checked Then
            'cboMaster.Items.Clear()
            'cboMaster.Items.Add("Customer Order")
            'cboMaster.Items.Add("Shipment Entry CPO")
            'cboMaster.Items.Add("Payment Entry CPO")
            'cboMaster.Items.Add("Claimed Entry")
            'cboMaster.Items.Add("Diversion PO")
            'cboMaster.Items.Add("Div PO Shipent Entry")
            'cboMaster.Items.Add("Local Quotation")
            'cboMaster.Items.Add("Import Quotation")
            'cboMaster.Items.Add("Proforma Invoice")
            'cboMaster.Items.Add("Sample Submission")
            'cboMaster.Items.Add("Sales Return")
            'cboMaster.Items.Add("Sales Order")
            'cboMaster.Items.Add("Sales Invoice")
            'cboMaster.Items.Add("Purchase Order")
            'cboMaster.Items.Add("Goods Receipt Note")

            lblCaption.Text = "Transaction"
            Call bindCombo(cboMaster, "DocName", "DocName", "Select DocName from UtlTransactions where DocType = 'Transaction' And AuditLogTrack = 'Y' order by DocName")

        Else

            lblCaption.Text = "Master"
            Call bindCombo(cboMaster, "DocName", "DocName", "Select DocName from UtlTransactions where DocType = 'Master' And AuditLogTrack = 'Y' order by DocName")

            'cboMaster.Items.Clear()
            'cboMaster.Items.Add("Item Master")
            'cboMaster.Items.Add("Customer Master")
            'cboMaster.Items.Add("Supplier Master")
            'cboMaster.Items.Add("Currency Master")
            'cboMaster.Items.Add("Customer Group Master")
            'cboMaster.Items.Add("Supplier Group Master")
            'cboMaster.Items.Add("Sales Person")
            'cboMaster.Items.Add("Sales Incharge")
            'cboMaster.Items.Add("Item Make")
            'cboMaster.Items.Add("Zone Master")
            'cboMaster.Items.Add("Region Master")
            'cboMaster.Items.Add("State City Country")
            'cboMaster.Items.Add("Freight Forward")
            'cboMaster.Items.Add("Segment")
        End If
    End Sub

    Private Sub chkDate_CheckedChanged(sender As Object, e As EventArgs) Handles chkDate.CheckedChanged

    End Sub

    Private Sub cboMaster_TextChanged(sender As Object, e As EventArgs) Handles cboMaster.TextChanged
        If chkTrans.Checked = True Then
            strTrnHdrTblName = clsSVT.getDataStringValue("Select HdrTblName from Utltransactions where DocName = '" & Trim(cboMaster.Text) & "'", "", False)
            strTrnDtlTblName = clsSVT.getDataStringValue("Select DtlTblName from Utltransactions where DocName = '" & Trim(cboMaster.Text) & "'", "", False)
        Else
            strMstTblName = clsSVT.getDataStringValue("Select HdrTblName from Utltransactions where DocName = '" & Trim(cboMaster.Text) & "'", "", False)
        End If
    End Sub

    Private Sub chkTrans_Leave(sender As Object, e As EventArgs) Handles chkTrans.Leave
        cboMaster.Focus()
    End Sub

    Private Sub cboMaster_Leave(sender As Object, e As EventArgs) Handles cboMaster.Leave
        chkDate.Focus()
    End Sub

    Private Sub chkDate_Leave(sender As Object, e As EventArgs) Handles chkDate.Leave
        dtpFromDate.Focus()
    End Sub

    Private Sub dtpToDate_Leave(sender As Object, e As EventArgs) Handles dtpToDate.Leave
        btnView.Focus()
    End Sub

    Private Sub btnView_Leave(sender As Object, e As EventArgs) Handles btnView.Leave
        With vsfDet
            .Row = 1
            .Col = 1
            Call showWaitCursor()
        End With
    End Sub

    'Private Sub vsfDet_LostFocus(sender As Object, e As EventArgs) Handles vsfDet.LostFocus
    '    btnExcel.Focus()
    'End Sub

    Private Sub vsfDet_KeyUp(sender As Object, e As KeyEventArgs) Handles vsfDet.KeyUp
        If vsfDet.Row = vsfDet.Rows.Count - 1 AndAlso vsfDet.Col = vsfDet.Cols.Count - 1 AndAlso e.KeyCode = Keys.Tab Then
            btnExcel.Focus()
        End If
    End Sub

    Private Sub btnCancel_Leave(sender As Object, e As EventArgs) Handles btnCancel.Leave
        chkTrans.Focus()
    End Sub


    Private Sub pnlbtnClose_Click(sender As Object, e As EventArgs) Handles pnlbtnClose.Click
        pnlCopyShortCut.Visible = False
    End Sub

    Private Sub btnCopy_Click(sender As Object, e As EventArgs) Handles btnCopy.Click

        If RbnCopyCell.Checked = True Then
            CopyGrid(vsfDet, "Cell")
        ElseIf RbnCopyColumn.Checked = True Then

            CopyGrid(vsfDet, "Column")

        ElseIf RbnCopyRow.Checked = True Then

            CopyGrid(vsfDet, "Row")
        ElseIf RbnCopyGrid.Checked = True Then

            CopyGrid(vsfDet, "Full Grid")

        End If
        pnlCopyShortCut.Visible = False
    End Sub

    Private Sub vsfDet_Click(sender As Object, e As EventArgs) Handles vsfDet.Click

        If (Control.ModifierKeys And Keys.Control) = Keys.Control AndAlso (Control.ModifierKeys And Keys.Alt) = Keys.Alt Then
            pnlCopyShortCut.Show()
        End If

    End Sub
End Class
