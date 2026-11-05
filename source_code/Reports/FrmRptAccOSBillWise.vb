Imports System.Data.oledb
Public Class FrmRptAccOSBillWise
    Dim clsSVT As New svtLibrary

    Public strRcvblPbl As String
    Public strList As String
    Const vDetGroup = 0
    Const vDetParty = 1
    Const vDetDocDate = 2
    Const vDetDocType = 3
    Const vDetDocNo = 4
    Const vDetDebit = 5
    Const vDetCredit = 6

    Const VDetTotCols = 7

    Dim strPartyShForm As String
    Dim strPartyType As String
    Dim strPartyName As String
    Dim sngSign As Single

    Dim strPartyGroupList As String
    Dim strPartyList As String = ""

    Private Sub frmRptAccOutstanding_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyUp
        If e.KeyCode = Keys.F9 And e.Control = True Then
            Call initFormSize(Me, Me.Text, True, 958, 579, 0, 0)
            'Call SetColWidthFromString(vsfDet, strDocStdDtlWidth)
            MsgBox("Restored Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
        If e.KeyCode = Keys.F8 And e.Control = True Then
            Call SaveActFormSettings(Me, Me.Text)
            Call SaveActFormVsfDet(Me, Me.Text, "", vsfView)
            MsgBox("Saved Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If

        If e.Alt = True And e.KeyCode = Keys.E Then
            If vsfView.Rows.Count > 1 Then
                Call ExportToXL(vsfView)
            End If
        End If

    End Sub

    Private Sub frmRptAccOutstanding_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        dtpFromDate.Value = getSystemDate()
        With vsfView
            .Cols.Count = VDetTotCols
            .SetData(0, vDetGroup, "Group")
            .SetData(0, vDetParty, "Party")
            .SetData(0, vDetDebit, "Debit")
            .SetData(0, vDetCredit, "Credit")
            .SetData(0, vDetDocNo, "DocNo")
            .SetData(0, vDetDocDate, "DocDate")
            .SetData(0, vDetDocType, "DocType")
            .Rows.Count = 1
        End With
        Call SetColWidth(vsfView, 75, 200, 200, 120)
        KeyPreview = True
        initGridStyles(Me)
        If strRcvblPbl = "R" Then
            strPartyShForm = "Cust"
            strPartyType = "C"
            sngSign = -1
            strPartyName = "Customer"
            Me.Text = strPartyName & " Receivable Report "
        Else
            sngSign = 1
            strPartyShForm = "Supp"
            strPartyType = "S"
            strPartyName = "Supplier"
            Me.Text = strPartyName & " Payable Report "
        End If
        Call initFormSize(Me, Me.Text, False, 958, 579, 0, 0)
        chkSelCustGroup.Text = "Select " & strPartyName & " Group"
        chkSelCust.Text = "Select " & strPartyName
        Call initGridStyles(Me)
        Call fillFormProperties(Me)
        Call ChangeFontSize(Me)
    End Sub

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Call frmMain.CloseSelectedTabPage(Me)
    End Sub

    Private Sub btnExportToXL_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExportToXL.Click
        Call ExportToXL(vsfView,,,, True, Me.Text, "For the period " & getDateFormatInInsert(dtpFromDate.Value))
    End Sub

    Private Sub vsfview_BeforeEdit(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfView.BeforeEdit
        e.Cancel = True
    End Sub

    Private Sub btnView_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView.Click
        Dim rs As oledbDataReader, cmd As New oledbCommand
        Dim blnAdd As Boolean

        'If strPartyGroupList <> "" Then
        '    strSQL = strSQL & " and g." & strPartyShForm & "GroupID" & " in (" & strPartyGroupList & ")"
        'End If
        'If strPartyList <> "" Then
        '    strSQL = strSQL & " and p." & strPartyShForm & "ID" & " in (" & strPartyList & ")"
        'End If
        With cmd
            .Connection = svtConn
            .CommandText = "mSP_AcBillWisePendReport"
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add("@AcType", OleDbType.VarChar, 1).Value = strRcvblPbl
            .Parameters.Add("@AcLedgers", OleDbType.VarChar, -1).Value = strPartyList
            .CommandTimeout = 0
            rs = .ExecuteReader
        End With
        vsfView.Rows.Count = 1
        Me.Cursor = Cursors.WaitCursor

        If Not rs.HasRows Then
            MsgBox("No Data to load", MsgBoxStyle.Information, Me.Text)
            btnExportToXL.Visible = False
            Exit Sub
        End If

        btnExportToXL.Visible = True
        With rs
            If .HasRows Then
                Do While .Read
                    blnAdd = True
                    If Val(txtCr.Text) > 0 Then
                        If Math.Abs(!CAmt) <= Val(txtCr.Text) Then
                            blnAdd = False
                        End If
                    End If
                    If Val(txtDr.Text) > 0 Then
                        If Math.Abs(!DAmt) <= Val(txtDr.Text) Then
                            blnAdd = False
                        End If
                    End If
                    If blnAdd Then
                        vsfView.Rows.Count = vsfView.Rows.Count + 1
                        vsfView.SetData(vsfView.Rows.Count - 1, vDetGroup, "-")
                        vsfView.SetData(vsfView.Rows.Count - 1, vDetParty, !AcLedName)
                        vsfView.SetData(vsfView.Rows.Count - 1, vDetDocNo, !TrnNo)
                        vsfView.SetData(vsfView.Rows.Count - 1, vDetDocType, !DocType)
                        vsfView.SetData(vsfView.Rows.Count - 1, vDetDocDate, !TrnDate)
                        If !BalAmt > 0 Then
                            vsfView.SetData(vsfView.Rows.Count - 1, vDetDebit, Math.Abs(!balamt))
                        Else
                            vsfView.SetData(vsfView.Rows.Count - 1, vDetCredit, Math.Abs(!balAmt))
                        End If
                    End If
                Loop
            End If
        End With

        With vsfView
            .AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.RestrictAll
            .Cols(vDetGroup).AllowMerging = True
            .Cols(vDetParty).AllowMerging = True

            .SubtotalPosition = C1.Win.C1FlexGrid.SubtotalPositionEnum.BelowData

            .Subtotal(C1.Win.C1FlexGrid.AggregateEnum.Sum, 0, 1, vDetDebit)
            .Subtotal(C1.Win.C1FlexGrid.AggregateEnum.Sum, 0, 1, vDetCredit)

            .Subtotal(C1.Win.C1FlexGrid.AggregateEnum.Sum, -1, -1, vDetDebit)
            .Subtotal(C1.Win.C1FlexGrid.AggregateEnum.Sum, -1, -1, vDetCredit)

            .Cols(vDetDebit).Format = "0.00"
            .Cols(vDetCredit).Format = "0.00"
            .Cols(vDetDebit).TextAlignFixed = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter
            .Cols(vDetCredit).TextAlignFixed = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter
        End With
        Me.Cursor = Cursors.Default
        MsgBox("Data loaded successfully", MsgBoxStyle.Information, Me.Text)
        btnExportToXL.Visible = True
    End Sub

    Private Sub chkSelCustGroup_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkSelCustGroup.CheckedChanged
        Dim strSQL As String

        If chkSelCustGroup.Checked = False Then
            strList = ""
            strPartyGroupList = ""
            Exit Sub
        End If

        Dim frmSel As New frmUtlMultiSelection

        strSQL = "Select distinct 0 as sel," & strPartyShForm & "GroupID," & strPartyShForm & "GroupName from Mst" & strPartyShForm & "Group "
        strSQL = strSQL & " where " & strPartyShForm & "GroupID > 0 and "
        strSQL = strSQL & strPartyShForm & "GroupName Not in ('-' , '') and InActive = 'N' order by " & strPartyShForm & "GroupName"
        Call frmSel.InitVsfSel(strSQL, 1, 1, True, Me, False)
        frmSel.Text = "Select [Multiple] " & strPartyName & " Group"
        frmSel.vsfDet.SetData(0, 0, "Sel")
        frmSel.ShowDialog()
        strPartyGroupList = strList

        If Trim(strPartyGroupList) = "" Then chkSelCustGroup.Checked = False
    End Sub

    Private Sub chkSelCust_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkSelCust.CheckedChanged
        Dim strSQL As String
        If chkSelCust.Checked = False Then
            strList = ""
            strPartyList = ""
            Exit Sub
        End If

        Dim frmSel As New frmUtlMultiSelection

        strSQL = "Select distinct 0 as sel,AcLedID,AcLedName from MstAcLed"
        strSQL = strSQL & " where AcLedID > 0 and "
        If strRcvblPbl = "R" Then
            strSQL = strSQL & "LedType = 'C'"
        Else
            strSQL = strSQL & "LedType = 'S'"
        End If
        strSQL = strSQL & " and InActive = 'N' order by AcLedName"
        Call frmSel.InitVsfSel(strSQL, 1, 1, True, Me, False)
        frmSel.Text = "Select [Multiple] " & strPartyName & ""
        frmSel.vsfDet.SetData(0, 0, "Sel")
        frmSel.ShowDialog()
        strPartyList = strList

        If Trim(strPartyList) = "" Then chkSelCustGroup.Checked = False
    End Sub

    Private Sub txtCr_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles txtCr.KeyPress, txtDr.KeyPress
        If e.KeyChar = "'" Then
            e.Handled = True
        End If
    End Sub

    Private Sub pnlbtnClose_Click(sender As Object, e As EventArgs) Handles pnlbtnClose.Click
        pnlCopyShortCut.Visible = False
    End Sub

    Private Sub btnCopy_Click(sender As Object, e As EventArgs) Handles btnCopy.Click

        If RbnCopyCell.Checked = True Then
            CopyGrid(vsfView, "Cell")
        ElseIf RbnCopyColumn.Checked = True Then

            CopyGrid(vsfView, "Column")

        ElseIf RbnCopyRow.Checked = True Then

            CopyGrid(vsfView, "Row")
        ElseIf RbnCopyGrid.Checked = True Then

            CopyGrid(vsfView, "Full Grid")

        End If
        pnlCopyShortCut.Visible = False
    End Sub

    Private Sub vsfDet_Click(sender As Object, e As EventArgs) Handles vsfView.Click

        If (Control.ModifierKeys And Keys.Control) = Keys.Control AndAlso (Control.ModifierKeys And Keys.Alt) = Keys.Alt Then
            pnlCopyShortCut.Show()
        End If

    End Sub
End Class