Imports System.IO
Public Class frmUtlBCDesigner
    Dim Mode As String
    Dim clsSvt As New svtLibrary
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Me.Close()
        Me.Dispose()
    End Sub
    Public Sub FillDocDetails(ByVal argLblName As String)
        Dim rs As oledb.oledbDataReader
        rs = clsSvt.getDataReaderFromSQL("Select * from UtlBCLayout where BCLblName = '" & argLblName & "'")
        With rs
            If .HasRows Then
                .Read()
                txtBCName.Text = argLblName
                txtBCName.Enabled = False
                txtQuery.Text = !BCQuery
                txtBCLayout.Text = !BCFmtFile
                btnSave.Visible = True
                btnClose.Enabled = False
                btnCancel.Visible = True
                btnDelete.Enabled = True
                btnNewBC.Enabled = False
                btnBCList.Enabled = True
                fraMain.Enabled = True
            End If
            .Close()
        End With
        rs = Nothing
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Call clearFlds()
        btnClose.Enabled = True
        fraMain.Enabled = False
        btnNewBC.Enabled = True
        btnSave.Visible = False
        btnCancel.Visible = False
        btnDelete.Enabled = False
        btnBCList.Enabled = True
    End Sub

    Private Sub clearFlds()
        txtBCName.Text = ""
        txtBCLayout.Text = ""
        txtQuery.Text = ""
    End Sub

    Private Sub frmUtlBCDesigner_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'fraMain.Enabled = False
        'btnSave.Visible = False
        'btnCancel.Visible = False
        'btnDelete.Enabled = False

        fraMain.Enabled = True
        btnSave.Visible = True
        btnCancel.Visible = True
        btnDelete.Enabled = True
        txtBCName.Enabled = False
        initGridStyles(Me)
    End Sub

    Private Sub btnNewBC_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNewBC.Click
        Call clearFlds()
        txtBCName.Enabled = True
        btnSave.Visible = True
        btnClose.Enabled = False
        btnCancel.Visible = True
        btnDelete.Enabled = False
        btnNewBC.Enabled = False
        btnBCList.Enabled = False
        fraMain.Enabled = True
        txtBCName.Focus()

    End Sub

    Private Sub btnBCList_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBCList.Click
        'Dim frmMstList As New frmRptView
        'With frmMstList
        '    .Text = " Listing of Barcode Labels"
        '    Call .FillViewVsf(Me, "Select * from UtlBCLayout", "", "")
        '    .lblTotRec.Text = "Total Records: " & .vsfDet.Rows.Count - 4
        '    .ShowDialog()
        'End With

        Dim strSQL As String = ""
        strSQL = "Select BCLblName,BCQuery,BCFmtFile from UtlBCLayout order by 1"

        Dim frmUtlSelection As New frmUtlSelection
        With frmUtlSelection
            .Text = " Listing of Barcode Labels"
            .FillViewVsf(Me, False, Nothing, strSQL, txtBCName, 0, txtQuery, 1, txtBCLayout, 2)
            .Text = " Please Select Item"
            .ShowDialog()
            .Close()
        End With
        fraMain.Enabled = True
        txtBCName.Enabled = False
        btnSave.Visible = True
        btnCancel.Visible = True
    End Sub

    Private Sub btnBrowse_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBrowse.Click
        fdb.ShowDialog()
        txtBCLayout.Text = Path.GetFileName(fdb.FileName)

    End Sub

    Private Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim strsql As String
        strsql = "Delete from UtlBCLayout where BCLblName = '" & txtBCName.Text & "'"
        clsSvt.execSQLStatement(strsql)

        strsql = "Insert into UtlBCLayout (BCLblName, BCQuery, BCFmtFile) values ("
        strsql = strsql & "'" & txtBCName.Text & "',"
        strsql = strsql & "'" & txtQuery.Text & "',"
        strsql = strsql & "'" & txtBCLayout.Text & "')"
        clsSvt.execSQLStatement(strsql)

        MsgBox("Saved Successfully!", vbInformation, Me.Text)
        Call clearFlds()
        fraMain.Enabled = False
        btnClose.Enabled = True
        btnSave.Visible = False
        btnCancel.Visible = False
        btnBCList.Enabled = True
        btnNewBC.Enabled = True
        btnDelete.Enabled = True
    End Sub

    Private Sub btnOpenFile_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOpenFile.Click
        Try
            System.Diagnostics.Process.Start(gstrRptPath & txtBCLayout.Text)
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information, Me.Text)
        End Try
    End Sub

    Private Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        Dim strsql As String
        If MsgBox("Do you want to delete this Label Design ?", MsgBoxStyle.Critical + vbYesNo, Me.Text) = MsgBoxResult.Yes Then
            strsql = "Delete from UtlBCLayout where BCLblName = '" & txtBCName.Text & "'"
            clsSvt.execSQLStatement(strsql)

            Call clearFlds()
            MsgBox("Deleted successfully!", MsgBoxStyle.Information, Me.Text)
        End If
    End Sub
End Class