Imports System.ComponentModel
Imports C1.Win.C1FlexGrid

Public Class frmUtlBulkPDFGenerator
    Dim strSQL As String
    Dim clsSVT As New svtLibrary

    Public strList As String
    Dim strPartyList As String

    Dim intDocID As Int32
    Dim strPrintOption As String
    Dim strDocName As String
    Dim strShForm As String

    Dim intMstPartyDocID As Int32
    Dim strPartyShForm As String

    Const vDetPath = 0
    Const vDetSel = 1
    Const vDetDocID = 2
    Const vDetDocNo = 3
    Const vDetDocDate = 4
    Const vDetParty = 5
    Const vDetStatus = 6
    Const vDetTot = 7

    Dim blnChkAll As Boolean = False

    Private Sub frmUtlBulkPDFGenerator_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Call initFormVsfDet(Me, Me.Text, vsfDet, "DET")
        Call fillFormProperties(Me)
        Call initGridStyles(Me)
        Call initScreen()
        txtPath.Text = INIGet("BulkPdfPath", "FolderPath", "")
    End Sub

    Private Sub initScreen()
        Call bindCombo(cboTrn, "DocName", "DocName", "Select DocName From UtlTransactions Where DocID in (Select DocID From UtlOption) Order by 1 ")
        cboTrn.Text = ""

        With vsfDet
            .Rows.Count = 1
            .Cols.Count = vDetTot

            .SetData(0, vDetPath, "Path")
            .SetData(0, vDetSel, "Sel")
            .SetData(0, vDetDocNo, "Doc No")
            .SetData(0, vDetDocDate, "Doc Date")
            .SetData(0, vDetParty, "Party")
            .SetData(0, vDetStatus, "Status")
            .Cols(vDetSel).DataType = GetType(Boolean)

            Call SetColAlign(vsfDet, svtAlign.Left, vDetDocNo)
            Call SetFlexColorFullGrid(vsfDet, "")
            Call SetHideCol(vsfDet, True, vDetPath, vDetDocID)
        End With
    End Sub

    Private Sub ClearFlds()
        vsfDet.Rows.Count = 1
        cboTrn.Text = ""
        cboPrintType.Text = ""
        chkParty.Checked = ""
        strList = ""
        strPartyList = ""
        txtPath.Text = ""
        intDocID = 0
        strPrintOption = ""
    End Sub

    Private Sub cboTrn_Validating(sender As Object, e As CancelEventArgs) Handles cboTrn.Validating
        If cboTrn.Text = "" Then Exit Sub

        intDocID = clsSVT.getDataStringValue("Select DocID From UtlTransactions Where DocName = '" & cboTrn.Text & "'", 0, False)

        If intDocID <= 0 Then
            MsgBox("Invalid Document Name!", vbInformation, Me.Text)
            cboTrn.Text = ""
            cboTrn.Focus()
            Exit Sub
        End If

        Dim blnFound As Boolean
        clsSVT.getDataStringValue(" Select Top 1 OptionName From UtlOption Where DocID=" & intDocID, "", blnFound)

        If Not blnFound Then
            MsgBox("No Print For this Document....", MsgBoxStyle.Exclamation, Me.Text)
            cboTrn.Text = ""
            cboTrn.Focus()
            Exit Sub
        End If

        Call bindCombo(cboPrintType, "OptionName", "OptionName", "Select OptionName From UtlOption Where DocID = (Select DocID From UtlTransactions Where DocName = '" & cboTrn.Text & "') Order by 1")
        cboPrintType.SelectedItem = 0


        intMstPartyDocID = clsSVT.getDataLongValue("Select MstDocIDParty from UtlTransactions Where DocID=" & intDocID, 0, False)

        strPartyShForm = ""
        If intMstPartyDocID > 0 Then
            strPartyShForm = clsSVT.getDataStringValue("Select DocShForm from UtlTransactions Where DocID = " & intMstPartyDocID & "", "", False)
        End If
        If strPartyShForm <> "" Then
            chkParty.Enabled = True
        Else
            chkParty.Checked = False
            chkParty.Enabled = False
            strPartyShForm = ""
        End If
    End Sub

    Private Sub cboPrintType_Validating(sender As Object, e As CancelEventArgs) Handles cboPrintType.Validating
        strPrintOption = ""
        If cboPrintType.Text = "" Then Exit Sub

        Dim blnFound As Boolean

        Call clsSVT.getDataStringValue("Select OptionName From UtlOption Where DocID =  " & intDocID & " And OptionName = '" & cboPrintType.Text & "'", "", blnFound)

        If Not blnFound Then
            MsgBox("Invalid Print Type!", vbInformation, Me.Text)
            cboPrintType.Text = ""
            cboPrintType.Focus()
            Exit Sub
        End If
        strPrintOption = Trim(cboPrintType.Text)
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        INIStore("BulkPdfPath", "FolderPath", txtPath.Text)
        frmMain.CloseSelectedTabPage(Me)
    End Sub

    Private Sub btnLoadPath_Click(sender As Object, e As EventArgs) Handles btnLoadPath.Click
        Dim idb As New FolderBrowserDialog
        idb.Description = "Select your folder"
        If idb.ShowDialog() = DialogResult.OK Then
            txtPath.Text = idb.SelectedPath
        End If
    End Sub

    Private Sub chkParty_CheckedChanged(sender As Object, e As EventArgs) Handles chkParty.CheckedChanged
        Dim strSQL As String

        If chkParty.Checked = False Then
            strList = ""
            strPartyList = ""
            Exit Sub
        End If

        Dim frmSel As New frmUtlMultiSelRpt

        strSQL = "Select distinct 0 as sel, " & strPartyShForm & "Id, " & strPartyShForm & "Code from Mst" & strPartyShForm & " where " & strPartyShForm & "ID > 0 and "
        strSQL = strSQL & " " & strPartyShForm & "Code Not in ('-' , '') and InActive = 'N' order by " & strPartyShForm & "Code"
        Call frmSel.InitVsfSel(strSQL, 1, 1, True, Me, False)
        frmSel.Text = "Select [Multiple] " & strPartyShForm & ""
        frmSel.vsfDet.SetData(0, 0, "Sel")
        frmSel.StartPosition = FormStartPosition.CenterScreen
        frmSel.ShowDialog()
        strPartyList = strList
        If strPartyList = "" Then chkParty.Checked = False
    End Sub

    Private Sub vsfDet_BeforeEdit(sender As Object, e As RowColEventArgs) Handles vsfDet.BeforeEdit
        Select Case e.Col
            Case vDetPath, vDetDocID, vDetDocNo, vDetDocDate, vDetParty, vDetStatus
                e.Cancel = True
        End Select
    End Sub

    Private Sub frmUtlBulkPDFGenerator_KeyUp(sender As Object, e As KeyEventArgs) Handles Me.KeyUp
        If e.KeyCode = Keys.F9 And e.Control Then
            Call initFormVsfDet(Me, Me.Text, vsfDet, "DET")
            MsgBox("Restored Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If

        If e.KeyCode = Keys.F8 And e.Control = True Then
            Call SaveActFormVsfDet(Me, Me.Text, "DET", vsfDet)
            MsgBox("Saved Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
    End Sub

    Private Sub btnGENPDF_Click(sender As Object, e As EventArgs) Handles btnGENPDF.Click
        If Not validFlds() Then Exit Sub
        If vsfDet.Rows.Count <= 1 Then
            MsgBox("No data present!!", vbInformation, Me.Text)
            Exit Sub
        End If
        If Trim(txtPath.Text) = "" Then
            MsgBox("Please select the path", vbInformation, Me.Text)
            txtPath.Focus()
            Exit Sub
        End If
        If Not System.IO.Directory.Exists(Trim(txtPath.Text)) Then
            MsgBox("Invalid path", vbInformation, Me.Text)
            txtPath.Focus()
            Exit Sub
        End If
        Call showWaitCursor()
        Call StartPDFGen()
        Call showNormalCursor()
    End Sub

    Private Function StartPDFGen() As Boolean
        StartPDFGen = False
        Try
            With vsfDet
                For i As Int32 = 1 To .Rows.Count - 1
                    If .GetData(i, vDetSel) = True Then
                        .TopRow = i
                        Dim frmv As New frmUtlReportViewer
                        With frmv
                            Application.DoEvents()
                            .StartPosition = FormStartPosition.CenterScreen
                            If .ExportPDF(intDocID, Val(vsfDet.GetData(i, vDetDocID)), strPrintOption, Trim(txtPath.Text) & "\" & CStr(intDocID) & "-" & CStr(vsfDet.GetData(i, vDetDocNo)) & ".pdf") Then

                                If System.IO.File.Exists(Trim(txtPath.Text) & "\" & CStr(intDocID) & "-" & CStr(vsfDet.GetData(i, vDetDocNo)) & ".pdf") Then
                                    vsfDet.SetData(i, vDetStatus, "Exported")
                                    vsfDet.SetData(i, vDetPath, Trim(txtPath.Text) & "\" & CStr(intDocID) & "-" & CStr(vsfDet.GetData(i, vDetDocNo)) & ".pdf")
                                End If
                            End If
                        End With
                        frmv.Dispose()
                    End If
                Next
            End With
            Call calExpRec()
            MsgBox("Export Successfully Completed", vbInformation, Me.Text)
            StartPDFGen = True
        Catch ex As Exception
            StartPDFGen = False
        End Try
    End Function

    Private Sub vsfDet_DoubleClick(sender As Object, e As EventArgs) Handles vsfDet.DoubleClick
        If vsfDet.Rows.Count <= 1 Then Exit Sub
        If vsfDet.Row <= 0 Then Exit Sub
        If vsfDet.GetData(vsfDet.Row, vDetStatus) = "Exported" Then
            If System.IO.File.Exists(CStr(vsfDet.GetData(vsfDet.Row, vDetPath))) Then
                Process.Start(CStr(vsfDet.GetData(vsfDet.Row, vDetPath)))
            End If
        End If

    End Sub

    Private Function validFlds() As Boolean
        validFlds = False
        If Trim(cboTrn.Text) = "" Then
            MsgBox("Please select transaction", vbInformation, Me.Text)
            cboTrn.Focus()
            Exit Function
        End If
        If intDocID <= 0 Then
            MsgBox("Invalid Transaction!!", vbInformation, Me.Text)
            cboTrn.Focus()
            Exit Function
        End If
        If Trim(cboPrintType.Text) = "" Then
            MsgBox("Please select Print Type", vbInformation, Me.Text)
            cboPrintType.Focus()
            Exit Function
        End If
        If strPrintOption = "" Then
            MsgBox("Invalid Print Type!!", vbInformation, Me.Text)
            cboPrintType.Focus()
            Exit Function
        End If
        validFlds = True
    End Function

    Private Sub btnLoad_Click(sender As Object, e As EventArgs) Handles btnLoad.Click
        If Not validFlds() Then Exit Sub
        strShForm = clsSVT.getDataStringValue("Select DocShForm From UtlTransactions Where DocID=" & intDocID, "", False)

        Dim strSQL As String = "Select "
        strSQL &= strShForm & "ID,"
        strSQL &= strShForm & "No,"
        strSQL &= strShForm & "Date,"
        If intMstPartyDocID > 0 Then
            strSQL &= "PartyName,"
        Else
            strSQL &= " '', "
        End If
        strSQL &= "'' as ST From TrnHdr" & strShForm
        strSQL &= " Where 1=1 "
        strSQL &= " and " & strShForm & "Date Between '" & Format(dtpFromDate.Value, "yyyy-MM-dd") & "' and '" & Format(dtpToDate.Value, "yyyy-MM-dd") & "'"
        If intMstPartyDocID > 0 Then
            If chkParty.Checked And strPartyList <> "" Then
                strSQL &= " and " & strPartyShForm & "ID in (" & strPartyList & ")"
            End If
        End If
        strSQL &= " Order by 2 "

        Dim rs As oledb.oledbDataReader
        rs = clsSVT.getDataReaderFromSQL(strSQL)

        vsfDet.Rows.Count = 1
        If Not rs.HasRows Then
            lblTolRec.Text = "Total records : " & vsfDet.Rows.Count - 1
            Call calExpRec()
            Exit Sub
        End If

        Do While rs.Read
            With vsfDet
                .Rows.Count = .Rows.Count + 1
                .SetData(.Rows.Count - 1, vDetDocID, rs.Item(strShForm & "ID"))
                .SetData(.Rows.Count - 1, vDetDocDate, rs.Item(strShForm & "Date"))
                .SetData(.Rows.Count - 1, vDetDocNo, rs.Item(strShForm & "No"))
                .SetData(.Rows.Count - 1, vDetStatus, rs.Item("ST"))
                If intMstPartyDocID > 0 Then
                    .SetData(.Rows.Count - 1, vDetParty, rs.Item("PartyName"))
                End If
            End With
        Loop
        rs.Close()
        lblTolRec.Text = "Total records : " & vsfDet.Rows.Count - 1
        Call calExpRec()
    End Sub

    Private Sub lblPath_Click(sender As Object, e As EventArgs) Handles lblPath.Click
        If Trim(txtPath.Text) = "" Then Exit Sub
        If System.IO.Directory.Exists(txtPath.Text) Then
            Process.Start(txtPath.Text)
        End If
    End Sub

    Private Sub vsfDet_AfterEdit(sender As Object, e As RowColEventArgs) Handles vsfDet.AfterEdit
        If e.Col = vDetSel Then
            Call calExpRec()
        End If
    End Sub

    Private Sub calExpRec()
        Dim intExp As Int32 = 0
        Dim intChkRec As Int32 = 0
        For i As Int32 = 1 To vsfDet.Rows.Count - 1
            If vsfDet.GetData(i, vDetSel) = True Then
                intChkRec = intChkRec + 1
            End If
            If vsfDet.GetData(i, vDetStatus) = "Exported" Then
                intExp = intExp + 1
            End If
        Next
        lblExpVsRec.Text = "Exported/Record Selected : " & intExp & "/" & intChkRec
    End Sub

    Private Sub vsfDet_MouseClick(sender As Object, e As MouseEventArgs) Handles vsfDet.MouseClick
        Dim ht = vsfDet.HitTest

        If ht.Row = 0 AndAlso ht.Column = vDetSel Then
            If blnChkAll Then
                For i As Int32 = 1 To vsfDet.Rows.Count - 1
                    vsfDet.SetData(i, vDetSel, False)
                Next
                blnChkAll = False
            Else
                For i As Int32 = 1 To vsfDet.Rows.Count - 1
                    vsfDet.SetData(i, vDetSel, True)
                Next
                blnChkAll = True
            End If
        End If
    End Sub

    Private Sub frmUtlBulkPDFGenerator_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        INIStore("BulkPdfPath", "FolderPath", txtPath.Text)
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