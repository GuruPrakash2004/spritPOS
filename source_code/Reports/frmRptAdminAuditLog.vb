Imports System.Data.OleDb
Public Class frmRptAdminAuditLog

    Dim clsSvt As New svtLibrary
    Dim strSql As String

    Dim strScrName

    Const vsfScrName = 0
    Const vsfDocNo = 1
    Const vsfDtChanged = 2
    Const vsfTimeChanged = 3
    Const vsfEarData = 4
    Const vsfChanData = 5
    Const vsfItemCode = 6
    Const vsfItemName = 7
    Const vsfItemdetails = 8
    Const vsfItemStatus = 9



    Const vsfTotCols = 15
    Private Sub frmRptAdminAuditLog_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.KeyPreview = True
        Call fillFormProperties(Me)
        Call ChangeFontSize(Me)
        Call initGridStyles(Me)
        Call initScreen()
        Call initFormVsfDet(Me, Me.Text, vsfView, "VIEW")
    End Sub
    Private Sub initScreen()
        Call bindCombo(cboScr, "ScreenName", "ScreenName", "select Distinct ScreenName from UtlAdmAudit")

        cboScr.Text = INIGet("ScreenName", "ScreenName", "")

        With vsfView
            .Rows.Count = 1
            .Cols.Count = vsfTotCols

            .SetData(0, vsfScrName, "ScreenName")
            .SetData(0, vsfDocNo, "Doc NO")
            .SetData(0, vsfDtChanged, "Date Changed")
            .SetData(0, vsfTimeChanged, "Time Changed")
            .SetData(0, vsfEarData, "Earlier Data")
            .SetData(0, vsfChanData, "Changed Data")
            .SetData(0, vsfItemCode, "Item Code")
            .SetData(0, vsfItemName, "Item Name")
            .SetData(0, vsfItemStatus, "Item Status")
            .SetData(0, vsfItemdetails, "Item Details")


        End With
    End Sub

    Private Sub btnView_Click(sender As Object, e As EventArgs) Handles btnView.Click
        Dim i As Integer
        Dim rs As OleDbDataReader
        Dim strSQL As String
        strSQL = "select h.ScreenName,h.MainFld,DocDate,DocDateTime,EarlierData,ChangedData,"
        strSQL &= " d.ItemCode, d.Itemname,d.ActionStatus,d.ItemDetails"
        strSQL &= " From UtlAdmAudit h left outer join  UtlAdmAuditItem d "
        strSQL &= " on h.docid = d.docid"
        strSQL &= " Where DocDate between " & getDateFormatInCond(dtpFromDate.Value)
        strSQL &= " And " & getDateFormatInCond(dtpToDate.Value)
        If cboScr.Text <> "" Then
            strSQL &= " And ScreenName = '" & cboScr.Text & "'"
        End If
        strSQL &= " order by 1,2,3"
        rs = clsSvt.getDataReaderFromSQL(strSQL)

        If Not rs.HasRows Then
            rs.Close()
            Exit Sub
        End If
        vsfView.Rows.Count = 1
        With rs
            Do While .Read
                vsfView.Rows.Count = vsfView.Rows.Count + 1
                vsfView.SetData(vsfView.Rows.Count - 1, vsfScrName, !ScreenName)
                vsfView.SetData(vsfView.Rows.Count - 1, vsfDocNo, !MainFld)
                vsfView.SetData(vsfView.Rows.Count - 1, vsfDtChanged, getDateFormatInInsert(!DocDate))
                vsfView.SetData(vsfView.Rows.Count - 1, vsfTimeChanged, Format(rs!DocDateTime, "hh:mm:sstt"))
                vsfView.SetData(vsfView.Rows.Count - 1, vsfEarData, !EarlierData)

                vsfView.SetData(vsfView.Rows.Count - 1, vsfChanData, !ChangedData)
                vsfView.SetData(vsfView.Rows.Count - 1, vsfItemCode, !ItemCode)
                vsfView.SetData(vsfView.Rows.Count - 1, vsfItemName, !ItemName)
                vsfView.SetData(vsfView.Rows.Count - 1, vsfItemStatus, !ActionStatus)
                vsfView.SetData(vsfView.Rows.Count - 1, vsfItemdetails, !ItemDetails)
            Loop
            .Close()
        End With
        With vsfView
            .AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.RestrictAll
            For i = 0 To vsfItemName
                .Cols(i).AllowMerging = True
            Next
            .ExtendLastCol = False
            .Styles.Normal.WordWrap = True
            .AutoSizeRows()
        End With
    End Sub

    Private Sub frmRptAdminAuditLog_KeyUp(sender As Object, e As KeyEventArgs) Handles Me.KeyUp
        If e.KeyCode = Keys.F9 And e.Control = True Then
            Call initFormVsfDet(Me, Me.Text, vsfView, "VIEW")
            MsgBox("Restored Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
        If e.KeyCode = Keys.F8 And e.Control = True Then
            Call SaveActFormVsfDet(Me, Me.Text, "VIEW", vsfView)
            MsgBox("Saved Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
    End Sub

    Private Sub btnExcel_Click(sender As Object, e As EventArgs) Handles btnExcel.Click
        ExportToXL(vsfView)
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Call frmMain.CloseSelectedTabPage(Me)
    End Sub
    Private Sub ClearFlds()
        vsfView.Rows.Count = 1
        cboScr.Text = ""
    End Sub



    'Private Sub cboScr_Validating(sender As Object, e As CancelEventArgs) Handles cboScr.Validating
    '    If cboScr.Text = "" Then Exit Sub

    '    intDocID = clsSvt.getDataStringValue("Select DocID From UtlTransactions Where DocName = '" & cboScr.Text & "'", 0, False)

    '    If intDocID <= 0 Then
    '        MsgBox("Invalid Screen Name!", vbInformation, Me.Text)
    '        cboScr.Text = ""
    '        cboScr.Focus()
    '        Exit Sub
    '    End If


    'End Sub
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

