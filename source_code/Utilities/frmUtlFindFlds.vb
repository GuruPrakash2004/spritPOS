Imports System.ComponentModel
Imports System.Data.oledb

Public Class frmUtlFindFlds
    Dim clsSVT As New svtLibrary
    Public intTablID As Integer

    Dim Mode As String
    'For Grid vDET
    Const vDetTblID = 0
    Const vDetTblName = 1
    Const vDetStrFld = 2
    Const vDetDplyFld = 3
    Const vdetAlias1 = 4
    Const vdetAlias2 = 5
    Const vdetAlias3 = 6
    Const vdetAlias4 = 7
    Const vdetAlias5 = 8
    Const intColsDet = 9

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub frmMstAddress_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        KeyPreview = True
        'For Initialize Form With Default Grid
        Call initScreen()
        'For Load Grid Data
        Call initLoadgrid()
        fraEdit.Visible = False
        If vsfDet.Rows.Count > 0 Then
            btnEdit.Enabled = True
            btnDelete.Enabled = True
        End If
    End Sub

    Private Sub initLoadgrid()
        Dim rs As oledbDataReader
        Try
            vsfDet.Rows.Count = 1
            Dim strsQL As String
            strsQL = "select * "
            strsQL &= "  From UtlFindFlds order by TableName"
            rs = clsSVT.getDataReaderFromSQL(strsQL)
            Using rs
                With vsfDet
                    If Not rs.HasRows Then
                        rs.Close()
                        Exit Sub
                    End If
                    Do While rs.Read
                        .Rows.Count = .Rows.Count + 1
                        .SetData(.Rows.Count - 1, vDetTblID, rs.Item("TableID"))
                        .SetData(.Rows.Count - 1, vDetTblName, rs.Item("TableName"))
                        .SetData(.Rows.Count - 1, vDetStrFld, rs.Item("StorageFld"))
                        .SetData(.Rows.Count - 1, vDetDplyFld, rs.Item("DisplayFld"))
                        .SetData(.Rows.Count - 1, vdetAlias1, rs.Item("AliasFld1"))
                        .SetData(.Rows.Count - 1, vdetAlias2, rs.Item("AliasFld2"))
                        .SetData(.Rows.Count - 1, vdetAlias3, rs.Item("AliasFld3"))
                        .SetData(.Rows.Count - 1, vdetAlias4, rs.Item("AliasFld4"))
                        .SetData(.Rows.Count - 1, vdetAlias5, rs.Item("AliasFld5"))
                    Loop
                End With
            End Using
            fraEdit.Visible = False
        Catch ex As Exception
            MsgBox("Error during Load" & vbCrLf & ex.Message, MsgBoxStyle.Critical, Me.Text & " [initLoadgrid]")
        End Try
    End Sub
    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        fraEdit.Visible = False
        vsfDet.Visible = True
        Call ClearFlds()
    End Sub
    Private Sub visibleEditbtn(ByVal argBLN As Boolean)
        If argBLN Then
            btnSave.Visible = True
            btnCancel.Visible = True
            btnClose.Visible = True
        Else
            btnCancel.Visible = True
            btnSave.Visible = False
            btnClose.Visible = True
        End If
    End Sub

    Private Sub initScreen()
        Try
            With vsfDet
                .Rows.Count = 1
                .Cols.Count = intColsDet

                .SetData(0, vDetTblID, "TableID")
                .SetData(0, vDetTblName, "Table Name")
                .SetData(0, vDetStrFld, "Storage Field")
                .SetData(0, vDetDplyFld, "Display Field")
                .SetData(0, vdetAlias1, "Alias Fld1")
                .SetData(0, vdetAlias2, "Alias Fld2")
                .SetData(0, vdetAlias3, "Alias Fls3")
                .SetData(0, vdetAlias4, "Alias Fld4")
                .SetData(0, vdetAlias5, "Alias Fld5")
            End With
            Call SetHideCol(vsfDet, True, vDetTblID)
            Call SetRowAlign(vsfDet, svtAlign.Center, 0)
            Call initFormSize(Me, Me.Text, False, 756, 483, 0, 0)
            Call initFormVsfDet(Me, Me.Text, vsfDet, "DET")
            Call ChangeFontSize(Me)
            Call SetFlexColor(vsfDet, "", vDetTblName, vDetStrFld, vDetDplyFld, vdetAlias1, vdetAlias2, vdetAlias3, vdetAlias4, vdetAlias5)
            fraEdit.Visible = False
            vsfDet.Visible = True
            initGridStyles(Me)
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub


    Private Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        Mode = "A"
        Call ClearFlds()
        vsfDet.Visible = True
        fraEdit.Visible = True
        fraEdit.BringToFront()
        cboTblNam.Focus()
        visibleEditbtn(True)
        txtDispFld.Enabled = True
        txtAlias1.Enabled = True
        txtAlias2.Enabled = True
        txtAlias3.Enabled = True
        txtAlias4.Enabled = True
        txtAlias5.Enabled = True
        cboTblNam.Enabled = True
        cboStrFld.Enabled = True
        fraEdit.BringToFront()
        Call bindCombo(cboTblNam, "Object_Id", "Name", "Select Object_Id,Name " _
                     & "From sys.tables where LEFT(name,3) in ('MST','TRN','UTL','RPT') order by Name")
        cboTblNam.Text = ""
        cboStrFld.Text = ""

    End Sub

    Private Sub ClearFlds()
        txtDispFld.Text = ""
        txtAlias1.Text = ""
        txtAlias2.Text = ""
        txtAlias3.Text = ""
        txtAlias4.Text = ""
        txtAlias5.Text = ""
        cboTblNam.Text = ""
        cboStrFld.Text = ""
    End Sub

    Private Sub vsfDet_BeforeEdit(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfDet.BeforeEdit
        e.Cancel = True
    End Sub

    Private Sub vsfDet_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles vsfDet.DoubleClick
        Mode = "M"
        Call LoadData()
    End Sub

    Private Sub btnEdit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEdit.Click
        Mode = "M"
        Call LoadData()
    End Sub

    Private Sub LoadData()
        If vsfDet.Rows.Count <= 1 Then Exit Sub
        Call bindCombo(cboTblNam, "Object_Id", "Name", "Select Object_Id,Name " _
                          & "From sys.tables where LEFT(name,3) in ('MST','TRN','UTL','RPT') order by Name")
        intTablID = vsfDet.GetData(vsfDet.Row, vDetTblID)
        cboTblNam.Text = vsfDet.GetData(vsfDet.Row, vDetTblName)
        cboStrFld.Text = vsfDet.GetData(vsfDet.Row, vDetStrFld)
        txtDispFld.Text = vsfDet.GetData(vsfDet.Row, vDetDplyFld)
        txtAlias1.Text = vsfDet.GetData(vsfDet.Row, vdetAlias1)
        txtAlias2.Text = vsfDet.GetData(vsfDet.Row, vdetAlias2)
        txtAlias3.Text = vsfDet.GetData(vsfDet.Row, vdetAlias3)
        txtAlias4.Text = vsfDet.GetData(vsfDet.Row, vdetAlias4)
        txtAlias5.Text = vsfDet.GetData(vsfDet.Row, vdetAlias5)
        fraEdit.Visible = True
        fraEdit.BringToFront()
        cboTblNam.Enabled = False
        vsfDet.Visible = True
        btnEdit.Enabled = True
        btnDelete.Enabled = True
    End Sub

    Private Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        Dim strSQL As String
        Dim svtTrn As oledb.oledbTransaction = Nothing
        Try
            If MsgBox("Are you sure you want to delete", MsgBoxStyle.YesNo + vbDefaultButton2 + MsgBoxStyle.Information, "Deletion Confirmation...") = vbNo Then Exit Sub
            svtTrn = svtConn.BeginTransaction(IsolationLevel.ReadCommitted)
            strSQL = "Delete from utlfindflds where TableID = " & GetVal(vsfDet.GetData(vsfDet.Row, vDetTblID), 0)
            clsSVT.execSQLStatement(strSQL, svtTrn)
            svtTrn.Commit()
            vsfDet.Rows.Remove(vsfDet.Row)
            MsgBox("Deleted Successfully!", vbInformation, Me.Text)
            Exit Sub
        Catch ex As Exception
            MsgBox("Error during Delete" & vbCrLf & ex.Message, MsgBoxStyle.Critical, Me.Text & " [btnDelete]")
            svtTrn.Rollback()
        End Try
    End Sub

    Private Sub frmMstPartyAddress_KeyUp(sender As Object, e As KeyEventArgs) Handles Me.KeyUp
        If e.KeyCode = Keys.F9 And e.Control = True Then
            Call initFormSize(Me, Me.Text, True, 756, 483, 0, 0)
            Call initFormVsfDet(Me, Me.Text, vsfDet, "DET")
            MsgBox("Restored Form & Grid Layout Successfully!!", MsgBoxStyle.Information, Me.Text)
        End If
        If e.KeyCode = Keys.F8 And e.Control = True Then
            Call SaveActFormSettings(Me, Me.Text)
            Call SaveActFormVsfDet(Me, Me.Text, "DET", vsfDet)
            MsgBox("Saved Form & Grid Layout Successfully!!", MsgBoxStyle.Information, Me.Text)
        End If

        If e.Alt = True And e.KeyCode = Keys.F12 Then
            Call ExportToXL(vsfDet)
        End If
    End Sub

    Private Sub btnExport_Click(sender As Object, e As EventArgs) Handles btnExport.Click
        Call ExportToXL(vsfDet, argFileName:="View Search Find Fields")
    End Sub

    Private Sub cboTblNam_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboTblNam.SelectedIndexChanged
        If Not String.IsNullOrEmpty(Trim(cboTblNam.Text)) Then
            Call bindCombo(cboStrFld, "COLUMN_NAME", "COLUMN_NAME", "select TABLE_NAME,COLUMN_NAME From Information_Schema.Columns 
                                    where TABLE_NAME= '" & GetVal(Trim(cboTblNam.Text), "") & "'")
        End If
    End Sub

    Private Function SaveMenuSettings() As Boolean
        Dim strSQL As String
        SaveMenuSettings = False
        Dim svtTrn As oledb.oledbTransaction
        svtTrn = Nothing
        Try

            svtTrn = svtConn.BeginTransaction(IsolationLevel.ReadCommitted)
            If Mode = "A" Then
                strSQL = "insert into UtlFindFlds (TableID, TableName, StorageFld,"
                strSQL = strSQL & " DisplayFld, AliasFld1, AliasFld2,"
                strSQL = strSQL & " AliasFld3, AliasFld4,AliasFld5) values ( "
                strSQL = strSQL & clsSVT.AutoIncrementedNum("TableID", "UtlFindFlds", "", svtTrn) & ""
                strSQL = strSQL & ",'" & GetVal(Trim(cboTblNam.Text), "") & "'"
                strSQL = strSQL & ",'" & GetVal(Trim(cboStrFld.Text), "") & "'"
                strSQL = strSQL & ",'" & GetVal(Trim(txtDispFld.Text), "") & "'"
                strSQL = strSQL & ",'" & GetVal(Trim(txtAlias1.Text), "") & "'"
                strSQL = strSQL & ",'" & GetVal(Trim(txtAlias2.Text), "") & "'"
                strSQL = strSQL & ",'" & GetVal(Trim(txtAlias3.Text), "") & "'"
                strSQL = strSQL & ",'" & GetVal(Trim(txtAlias4.Text), "") & "'"
                strSQL = strSQL & ",'" & GetVal(Trim(txtAlias5.Text), "") & "'"
                strSQL = strSQL & " )"
                clsSVT.execSQLStatement(strSQL, svtTrn)
            Else
                strSQL = "Update UtlFindFlds set "
                strSQL = strSQL & " TableName = '" & GetVal(Trim(cboTblNam.Text), "") & "',"
                strSQL = strSQL & " StorageFld = '" & GetVal(Trim(cboStrFld.Text), "") & "',"
                strSQL = strSQL & " DisplayFld = '" & GetVal(Trim(txtDispFld.Text), "") & "',"
                strSQL = strSQL & " AliasFld1 = '" & GetVal(Trim(txtAlias1.Text), "") & "',"
                strSQL = strSQL & " AliasFld2 = '" & GetVal(Trim(txtAlias2.Text), "") & "',"
                strSQL = strSQL & " AliasFld3 = '" & GetVal(Trim(txtAlias3.Text), "") & "',"
                strSQL = strSQL & " AliasFld4 = '" & GetVal(Trim(txtAlias4.Text), "") & "',"
                strSQL = strSQL & " AliasFld5 = '" & GetVal(Trim(txtAlias5.Text), "") & "'"
                strSQL = strSQL & " where TableID =" & intTablID
                clsSVT.execSQLStatement(strSQL, svtTrn)
            End If
            SaveMenuSettings = True
            svtTrn.Commit()
            If Mode = "A" Then
                MsgBox("Saved Display Fields successfully", MsgBoxStyle.Information, Me.Text)
            Else
                MsgBox("Display Fields Updated successfully", MsgBoxStyle.Information, Me.Text)
            End If
        Catch ex As Exception
            MsgBox("Error during save" & vbCrLf & ex.Message, MsgBoxStyle.Critical, Me.Text & "[SaveMenuSettings]")
            svtTrn.Rollback()
            SaveMenuSettings = False
        End Try
    End Function

    Private Function ValidateFlds() As Boolean
        ValidateFlds = False

        'mandatory Checking Validations
        If String.IsNullOrEmpty(Trim(cboTblNam.Text)) Then
            MsgBox("Select Table Name", vbInformation, Me.Text)
            cboTblNam.Focus()
            Exit Function
        End If

        If String.IsNullOrEmpty(Trim(cboStrFld.Text)) Then
            MsgBox("Select Field Name", vbInformation, Me.Text)
            cboStrFld.Focus()
            Exit Function
        End If

        If String.IsNullOrEmpty(Trim(txtDispFld.Text)) Then
            MsgBox("Enter Display Field Name", vbInformation, Me.Text)
            txtDispFld.Focus()
            Exit Function
        End If

        'Check For Existing Table Name
        If Mode = "A" Then
            If clsSVT.getDataLongValue("select TableID from  UtlFindFlds " _
                      & " where TableName= '" & GetVal(Trim((cboTblNam.Text)), "") & "'", 0, False) > 0 Then
                MsgBox("Table Name Already Exist!", MsgBoxStyle.Exclamation, Me.Text)
                Exit Function
            End If
        End If

        ValidateFlds = True
    End Function

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        If ValidateFlds() = False Then Exit Sub
        Call SaveMenuSettings()
        Call ClearFlds()
        Call initLoadgrid()
    End Sub

    Private Sub cboTblNam_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cboTblNam.KeyPress, txtDispFld.KeyPress, cboStrFld.KeyPress, txtAlias1.KeyPress, txtAlias2.KeyPress, txtAlias3.KeyPress, txtAlias4.KeyPress, txtAlias5.KeyPress
        If e.KeyChar = "'" Then
            e.Handled = True
        End If
    End Sub

    Private Sub cboTblNam_Validating(sender As Object, e As CancelEventArgs) Handles cboTblNam.Validating
        If Trim(cboTblNam.Text) = "" Then Exit Sub

        Dim blnFound As Boolean
        clsSVT.getDataLongValue("select Object_ID From Sys.Tables  Where Name = '" & Trim(cboTblNam.Text) & "'", 0, blnFound)

        If Not blnFound Then
            MsgBox("Selected Table Name Not Found in Transaction List! " & vbCrLf & "Check Entry...", MsgBoxStyle.Exclamation, Me.Text)
            cboTblNam.Text = ""
            cboTblNam.Focus()
            Exit Sub
        End If
    End Sub

    Private Sub cboStrFld_Validating(sender As Object, e As CancelEventArgs) Handles cboStrFld.Validating
        If Trim(cboStrFld.Text) = "" And Trim(cboTblNam.Text) = "" Then Exit Sub

        Dim blnFound As Boolean
        clsSVT.getDataStringValue("select COLUMN_NAME From Information_Schema.Columns" _
                & " Where TABLE_NAME= '" & Trim(cboTblNam.Text) & "' and COLUMN_NAME = '" & Trim(cboStrFld.Text) & "' ", "", blnFound)

        If Not blnFound Then
            MsgBox("Selected Column Name Not Found in Transaction List! " & vbCrLf & "Check Entry...", MsgBoxStyle.Exclamation, Me.Text)
            cboStrFld.Text = ""
            cboStrFld.Focus()
            Exit Sub
        End If
    End Sub
End Class