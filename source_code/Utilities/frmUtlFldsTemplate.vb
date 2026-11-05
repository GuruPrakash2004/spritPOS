Imports System.ComponentModel
Imports System.Data.oledb
Imports C1.Win.C1FlexGrid

Public Class frmUtlFldsTemplate
    Dim clsSVT As New svtLibrary
    Public strMstTblName As String
    Public intDocID As Integer
    Public blnAddressTemplate As Boolean
    Dim strSQL As String

    Private Sub btnClear_Click(sender As Object, e As EventArgs) Handles btnClear.Click
        txtFlds.Text = ""
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Dispose()
    End Sub

    Private Sub frmUtlTemp_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Call fillFormProperties(Me)
        Call initGridStyles(Me)
        Call ChangeFontSize(Me)
        If blnAddressTemplate Then
            lblTemplate.Visible = False
            cboTemplate.Visible = False
            btnDelTemplate.Visible = False
            txtFlds.Top = cboTemplate.Top
            txtFlds.Height = txtFlds.Height + cboTemplate.Height + 5
            Call FillMstFlds()
            txtFlds.Text = clsSVT.getDataStringValue("Select CopyFlds from UtlPartyCopy where DocID=" & intDocID, "", False)
            Exit Sub
        End If
        'cboTemplate.Focus()

        Call bindCombo(cboTemplate, "Template", "Template", "Select Distinct Template from UtlSMSTemplate where DocID = " & intDocID & " order by 1")
        'Call cboTemplate_Validating(cboTemplate, Nothing)
        Call FillMstFlds()
    End Sub

    Private Sub FillMstFlds()
        Dim dt As DataTable

        If blnAddressTemplate Then
            strSQL = "Select Column_Name [Field Code] from Information_schema.Columns where Table_Name='" & strMstTblName & "'"
        Else
            strSQL = "Select Column_Name [Field Code] from Information_schema.Columns where Table_Name in (Select HdrTblName from UtlTransactions where DocID=" & intDocID & ")"
        End If
        dt = clsSVT.getDATATable(strSQL)
        vsfFlds.DataSource = dt
    End Sub

    Private Sub vsfFlds_DoubleClick(sender As Object, e As EventArgs) Handles vsfFlds.DoubleClick
        If vsfFlds.Rows.Count <= 1 Then Exit Sub
        txtFlds.Text = txtFlds.Text & "<<" & UCase(vsfFlds.GetData(vsfFlds.Row, vsfFlds.Col)) & ">>"
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click

        If Not validFlds() Then Exit Sub

        If blnAddressTemplate Then
            clsSVT.execSQLStatement("Delete from UtlPartyCopy where docID=" & intDocID)
            strSQL = "Insert into UtlPartyCopy (DocID, CopyFlds) values("
            strSQL &= intDocID & ","
            strSQL &= " '" & Replace(txtFlds.Text, vbLf, vbCrLf) & "')"
            clsSVT.execSQLStatement(strSQL)
            MsgBox("Data Saved Successfully!", vbInformation, Me.Text)
            Me.Dispose()
            Exit Sub
        End If

        clsSVT.execSQLStatement("Delete from UtlSMSTemplate where docID=" & intDocID & " and Template='" & Trim(cboTemplate.Text) & "'")
        strSQL = "Insert into UtlSMSTemplate (DocID, Template, TemplateText) values ("
        strSQL &= intDocID & ","
        strSQL &= "'" & Trim(cboTemplate.Text) & "',"
        strSQL &= " '" & Replace(txtFlds.Text, vbLf, vbCrLf) & "')"
        clsSVT.execSQLStatement(strSQL)
        MsgBox("Data Saved Successfully!", vbInformation, Me.Text)
        Me.Dispose()
    End Sub

    Private Sub vsfFlds_BeforeEdit(sender As Object, e As RowColEventArgs) Handles vsfFlds.BeforeEdit
        e.Cancel = True
    End Sub

    Private Sub cboTemp_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cboTemplate.KeyPress
        If e.KeyChar = "'" Then e.Handled = True
    End Sub

    Private Sub txtFlds_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtFlds.KeyPress
        If e.KeyChar = "'" Then e.KeyChar = "`"
    End Sub

    'Private Sub cboTemplate_Validating(sender As Object, e As CancelEventArgs) Handles cboTemplate.Validating
    '    If Trim(cboTemplate.Text) = "" Then Exit Sub
    '    txtFlds.Text = clsSVT.getDataStringValue("Select TemplateText from UtlSMSTemplate where DocID=" & intDocID & " and Template='" & cboTemplate.Text & "'", "", False)
    '    txtFlds.Focus()
    'End Sub

    Private Sub btnDelTemplate_Click(sender As Object, e As EventArgs) Handles btnDelTemplate.Click
        If Trim(cboTemplate.Text) = "" Then Exit Sub
        If MsgBox("Do You want to Delete this '" & cboTemplate.Text & "'", vbInformation + vbYesNo + vbDefaultButton2, Me.Text) = vbYes Then
            clsSVT.execSQLStatement("Delete from UtlSMSTemplate where docID=" & intDocID & " and Template='" & Trim(cboTemplate.Text) & "'")
            MsgBox("Template deleted successfully!", MsgBoxStyle.Information, Me.Text)
            Call bindCombo(cboTemplate, "Template", "Template", "Select Distinct Template from UtlSMSTemplate where DocID = " & intDocID & " order by 1")
            cboTemplate.Text = ""
            txtFlds.Text = ""
            Exit Sub
        End If
    End Sub

    Private Sub vsfFlds_KeyDown(sender As Object, e As KeyEventArgs) Handles vsfFlds.KeyDown
        If e.KeyCode = Keys.Tab Then
            If vsfFlds.Row = vsfFlds.Rows.Count - 1 Then
                txtFlds.Focus()
            End If
        End If
    End Sub

    Private Sub btnDelTemplate_LostFocus(sender As Object, e As EventArgs) Handles btnDelTemplate.LostFocus
        If Not e.Equals(Keys.Tab) Then
            With vsfFlds
                .Row = 0
                .Focus()
            End With
        End If
    End Sub

    Private Sub cboTemplate_LostFocus(sender As Object, e As EventArgs) Handles cboTemplate.LostFocus
        If Not e.Equals(Keys.Tab) Then
            btnNewTem.Focus()
        End If
    End Sub

    Public Function validFlds() As Boolean
        validFlds = False
        If Trim(cboTemplate.Text) = "" Then
            MsgBox("Template name should not be empty!", MsgBoxStyle.Information, Me.Text)
            Exit Function
        End If

        If txtFlds.Text = "" Then
            MsgBox("Add the template text!", MsgBoxStyle.Information, Me.Text)
            Exit Function
        End If
        validFlds = True
    End Function

    Private Sub btnNewTem_Click(sender As Object, e As EventArgs) Handles btnNewTem.Click
        cboTemplate.Text = ""
        txtFlds.Text = ""
    End Sub

    Private Sub cboTemplate_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboTemplate.SelectedIndexChanged
        txtFlds.Text = clsSVT.getDataStringValue("select TemplateText from UtlSMSTemplate where  docID=" & intDocID & " and Template='" & Trim(cboTemplate.Text) & "'", "", False)
    End Sub

End Class