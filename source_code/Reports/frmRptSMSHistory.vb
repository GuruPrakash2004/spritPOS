Imports System.Data.oledb
Public Class frmRptSMSHistory
    Dim clsSVT As New svtLibrary

    Public strList As String
    Dim strProviderList As String

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Me.Close()
        Me.Dispose()
    End Sub
    Private Sub btnView_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView.Click

        Dim strSQL As String = ""
        'check the date range
        If Format(dtpTo.Value, "yyyy/MM/dd") < Format(dtpFrom.Value, "yyyy/MM/dd") Then
            MsgBox("Invalid Date Range!", MsgBoxStyle.Critical, Me.Text)
            Exit Sub
        End If

        strSQL = "  Select s.RowID, TrnDate, Provider, MobNo, MsgText   "
        strSQL &= "  from TrnSMS s, UtlMsgSettings m  "
        strSQL &= "  Where 1=1 "
        strSQL &= "  and s.ProviderID=m.RowID  "
        If chkDate.Checked Then
            strSQL &= "  and TrnDate Between '" & Format(dtpFrom.Value, "yyyy-MM-dd") & "' and '" & Format(dtpTo.Value, "yyyy-MM-dd") & "' "
        End If
        If strProviderList <> "" AndAlso chkProvider.Checked Then
            strSQL &= "  and m.RowID in (" & strProviderList & ") "
        End If
        strSQL &= "  Order by TrnDate, Provider, MobNo "

        Dim strHeading As String = ""
        If chkDate.Checked Then
            strHeading = "From " & dtpFrom.Value.ToString("dd-MMM-yyyy") & " To " & dtpTo.Value.ToString("dd-MMM-yyyy")
        End If

        Dim dt As DataTable
        Dim dv As DataView

        dt = clsSVT.getDATATable(strSQL)
        dv = New DataView(dt)
        vsfDet.DataSource = dv

        vsfDet.Rows.Fixed = 4
        vsfDet.Rows.GlyphRow = 3
        vsfDet.BeginUpdate()
        vsfDet.DataSource = dv
        vsfDet.Cols(0).AllowMerging = True
        vsfDet.EndUpdate()
        vsfDet.Cols(0).Visible = False
        Call initVsfForHdrs(Me.Text, strHeading)
    End Sub

    Private Sub initVsfForHdrs(ByVal argHdrLine1 As String, ByVal argHdrLine2 As String)
        Dim i As Integer

        With vsfDet
            For i = 0 To .Cols.Count - 1
                .SetData(3, i, .GetData(0, i))
                .SetData(0, i, gSettings.gCompName & ",  " & gSettings.gCompAdd1 & gSettings.gCompAdd2 & gSettings.gCompAdd3 &
                          gSettings.gCompAdd4 & gSettings.gCompAdd5)
                .SetData(1, i, argHdrLine1)
                .SetData(2, i, argHdrLine2)
            Next
            Dim csf1 As C1.Win.C1FlexGrid.CellStyle = vsfDet.Styles.Add("F1")
            csf1.BackColor = Color.Beige

            .Rows(0).Style = csf1
            .Rows(1).Style = csf1
            .Rows(2).Style = csf1

            If argHdrLine1 = "" Then
                .Rows(1).Visible = False
            Else
                .Rows(1).Visible = True
            End If

            If argHdrLine2 = "" Then
                .Rows(2).Visible = False
            Else
                .Rows(2).Visible = True
            End If

            .Rows(0).AllowMerging = True
            .Rows(1).AllowMerging = True
            .Rows(2).AllowMerging = True
        End With
    End Sub

    Private Sub frmRptStockBal_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.KeyPreview = True
        Call initScreen()
        Call initFormSize(Me, Me.Text, False, 958, 579, 0, 0)
        Call initFormVsfDet(Me, Me.Text, vsfDet, "DET")
        Call fillFormProperties(Me)
        Call initGridStyles(Me)
        vsfDet.Rows.Fixed = 4
        vsfDet.Rows.Count = 4
    End Sub

    Private Sub initScreen()
        dtpFrom.Value = getFirstDateInMonth(getSystemDate)
        dtpTo.Value = getSystemDate()
    End Sub

    Private Sub btnExportToXL_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExportToXL.Click
        Call ExportToXL(vsfDet,,,, True, Me.Text, "For the period " & getDateFormatInInsert(dtpFrom.Value) & " to " & getDateFormatInInsert(dtpTo.Value))
    End Sub

    Private Sub vsfDet_BeforeEdit(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfDet.BeforeEdit
        e.Cancel = True
    End Sub

    Private Sub frmRptStockBal_KeyUp(sender As Object, e As KeyEventArgs) Handles Me.KeyUp
        If e.KeyCode = Keys.F9 And e.Control = True Then
            Call initFormSize(Me, Me.Text, True, 958, 579, 0, 0)
            Call initFormVsfDet(Me, Me.Text, vsfDet, "DET")
            MsgBox("Restored Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
        If e.KeyCode = Keys.F8 And e.Control = True Then
            Call SaveActFormSettings(Me, Me.Text)
            Call SaveActFormVsfDet(Me, Me.Text, "DET", vsfDet)
            MsgBox("Saved Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
    End Sub

    Private Sub chkProvider_CheckedChanged(sender As Object, e As EventArgs) Handles chkProvider.CheckedChanged
        Dim strSQL As String
        If chkProvider.Checked = False Then
            strList = ""
            strProviderList = ""
            Exit Sub
        End If

        Dim frmSel As New frmUtlMultiSelection

        strSQL = "Select distinct 0 as sel, RowID, Provider from UtlMsgSettings where  "
        strSQL = strSQL & " 1=1 order by 1"
        Call frmSel.InitVsfSel(strSQL, 1, 1, True, Me, False)
        frmSel.Text = "Select [Multiple] Providers"
        frmSel.vsfDet.SetData(0, 0, "Sel")
        frmSel.ShowDialog()
        strProviderList = strList

        If Trim(strProviderList) = "" Then chkProvider.Checked = False
    End Sub

    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        If vsfDet.Rows.Count <= 4 Then Exit Sub
        If MsgBox("Do you want to delete listed messages....?" & vbNewLine & "Click yes to delete all...", MsgBoxStyle.YesNo, Me.Text) = MsgBoxResult.No Then Exit Sub


        Dim strRowID As String = ""
        With vsfDet
            For i As Int32 = 4 To .Rows.Count - 1
                strRowID &= Trim(.GetData(i, 0)) & ","
            Next
        End With

        If strRowID <> "" Then
            strRowID = Mid(strRowID, 1, Len(strRowID) - 1)
            If clsSVT.execSQLStatement("Delete From TrnSMS Where RowID in (" & strRowID & ")") Then
                MsgBox("Deleted Successfully...", MsgBoxStyle.Information, Me.Text)
                Call btnView_Click(Nothing, Nothing)
            End If
        End If

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