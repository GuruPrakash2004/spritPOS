Imports System.ComponentModel
Imports System.IO

Public Class FrmUtlTrnLblPrint
    Dim Mode As String
    Dim clsSvt As New svtLibrary
    Dim argLblName As String

    Private Sub FrmUtlTrnLblPrint_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        fraMain.Enabled = False
        btnSave.Visible = False
        btnCancel.Visible = False
        btnDelete.Enabled = False

        initGridStyles(Me)
        'cboTable.SelectedIndex = -1
        Dim strSQL As String = "Select DocName from utltransactions where DocType = 'Transaction'"
        bindCombo(cboTable, "DocName", "DocName", strSQL)
        'cboTable.Text = ""
         cboTable.SelectedIndex = -1
    End Sub

    Private Sub FrmUtlTrnLblPrint_KeyUp(sender As Object, e As KeyEventArgs) Handles Me.KeyUp
        If e.KeyCode = Keys.F9 And e.Control = True Then
            Call initFormSize(Me, Me.Text, True, 514, 489, 0, 0)
            MsgBox("Restored Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
        If e.KeyCode = Keys.F8 And e.Control = True Then
            Call SaveActFormSettings(Me, Me.Text)
            'Call SaveActFormVsfDet(Me, Me.Text, "DET", vsfDet)
            MsgBox("Saved Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
        If e.KeyCode = Keys.O And e.Alt = True Then
            Me.Close()
        End If
    End Sub

    Public Sub FillDocDetails(ByVal argLblName As String)
        Dim rs As OleDb.OleDbDataReader
        Dim strSQL, strSQL1 As String
        strSQL = "Select DocName,PrintLblName,PLQuery,PLFmtFile,PLDocID from UtlTrnLblPrint l,UtlTransactions t where l.PLDocID = t.DocID and PrintLblName = '" & argLblName & "'"
        rs = clsSvt.getDataReaderFromSQL(strSQL)
        'strSQL1 = "Select DocName from UtlTransactions where DocID = '" & txtID.Text & "'" 
        'cboTable.Text = clsSvt.execSQL(strSQL1)
        'argLblName = txtPrintLblName.Text
        With rs
            If .HasRows Then
                .Read()
                txtPrintLblName.Text = argLblName
                txtPrintLblName.Enabled = False
                txtQuery.Text = !PLQuery
                txtPrintLayout.Text = !PLFmtFile
                txtID.Text = !PLDocID
                cboTable.Text = !DocName
                btnSave.Visible = True
                btnClose.Enabled = False
                btnCancel.Visible = True
                btnDelete.Enabled = True
                btnNewPrint.Enabled = False
                btnListPrint.Enabled = True
                fraMain.Enabled = True
            End If
            .Close()
        End With
        rs = Nothing
    End Sub

    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        Dim strsql As String
        If MsgBox("Do you want to delete this Label Design ?", MsgBoxStyle.Critical + vbYesNo, Me.Text) = MsgBoxResult.Yes Then
            strsql = "Delete from UtlTrnLblPrint where PrintLblName = '" & txtPrintLblName.Text & "'"
            clsSvt.execSQLStatement(strsql)

            Call clearFlds()
            MsgBox("Deleted successfully!", MsgBoxStyle.Information, Me.Text)
        End If
    End Sub

    Private Sub btnBrowse_Click(sender As Object, e As EventArgs) Handles btnBrowse.Click
        fdb.FileName = ""
        fdb.ShowDialog()
        txtPrintLayout.Text = Path.GetFileName(fdb.FileName)
    End Sub

    Private Sub clearFlds()
        txtPrintLblName.Text = ""
        txtPrintLayout.Text = ""
        txtQuery.Text = ""
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Call clearFlds()
        btnClose.Enabled = True
        fraMain.Enabled = False
        btnNewPrint.Enabled = True
        btnSave.Visible = False
        btnCancel.Visible = False
        btnDelete.Enabled = False
        btnListPrint.Enabled = True
        cboTable.SelectedIndex = -1
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
        Me.Dispose()
    End Sub

    Private Sub btnListPrint_Click(sender As Object, e As EventArgs) Handles btnListPrint.Click
        'Dim frmMstList As New frmRptView
        'With frmMstList
        '    .Text = " Listing of Print Labels"
        '    Call .FillViewVsf(Me, "Select * from UtlTrnLblPrint", "", "")
        '    .lblTotRec.Text = "Total Records: " & .vsfDet.Rows.Count - 4
        '    .ShowDialog()
        'End With
        txtPrintLblName.Text = ""
        Dim frmDisplay As New frmUtlSingleSel
        With frmDisplay
            .FillViewVsf(Me, "Select PrintLblName,PLQuery,PLFmtFile,PLDocID from UtlTrnLblPrint", txtPrintLblName, 0, -1)
            .ShowDialog()
        End With
        If txtPrintLblName.Text = "" Then
            Exit Sub
        End If
        Call FillDocDetails(txtPrintLblName.Text)

    End Sub

    Private Sub btnNewPrint_Click(sender As Object, e As EventArgs) Handles btnNewPrint.Click
        Call clearFlds()
        txtPrintLblName.Enabled = True
        btnSave.Visible = True
        btnClose.Enabled = False
        btnCancel.Visible = True
        btnDelete.Enabled = False
        btnNewPrint.Enabled = False
        btnListPrint.Enabled = False
        fraMain.Enabled = True
        txtPrintLblName.Focus()
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Dim strsql As String
        strsql = "Delete from UtlTrnLblPrint where PrintLblName = '" & txtPrintLblName.Text & "'"
        clsSvt.execSQLStatement(strsql)

        strsql = "Insert into UtlTrnLblPrint (PrintLblName, PLQuery, PLFmtFile,PLDocID) values ("
        strsql = strsql & "'" & txtPrintLblName.Text & "',"
        strsql = strsql & "'" & txtQuery.Text & "',"
        strsql = strsql & "'" & txtPrintLayout.Text & "',"
        strsql = strsql & Val(txtID.Text) & ")"
        clsSvt.execSQLStatement(strsql)

        MsgBox("Saved Successfully!", vbInformation, Me.Text)
        Call clearFlds()
        fraMain.Enabled = False
        btnClose.Enabled = True
        btnSave.Visible = False
        btnCancel.Visible = False
        btnListPrint.Enabled = True
        btnNewPrint.Enabled = True
        btnDelete.Enabled = True
        cboTable.SelectedIndex = -1
    End Sub

    Private Sub btnOpenFile_Click(sender As Object, e As EventArgs) Handles btnOpenFile.Click
        Try
            System.Diagnostics.Process.Start(gstrRptPath & txtPrintLayout.Text)
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information, Me.Text)
        End Try
    End Sub

    Private Sub cboTable_Validating(sender As Object, e As CancelEventArgs) Handles cboTable.Validating
        If cboTable.Text = "" Then Exit Sub
        Dim blnFound As Boolean
        Dim intDocID As Integer
        intDocID = clsSvt.getDataStringValue("Select DocID from UtlTransactions where Upper(DocName)='" & Trim(cboTable.Text) & "'", 0, blnFound)
        txtID.Text = intDocID

        If Not blnFound Then
            MsgBox("Table Not found in the list!!", MsgBoxStyle.Information, Me.Text)
            cboTable.Focus()
            Exit Sub
        End If
    End Sub
End Class