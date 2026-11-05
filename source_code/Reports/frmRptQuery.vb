Imports System.ComponentModel
Imports C1.Win.C1FlexGrid

Public Class frmRptQuery
    Dim clsSVT As New svtLibrary
    Dim strRights As String
    Const vdetTblName = 0

    Const vsfQName = 0

    Private Sub frmRptQuery_Load(sender As Object, e As EventArgs) Handles Me.Load
        strRights = getUserRights(GetMenuId(Me.Text))

        With vsfDet
            .Rows.Count = 1
            .Cols.Count = 1
            .SetData(0, vdetTblName, "Table Name")
            Call SetFlexRowColor(vsfDet, "", vdetTblName)
            .Rows(0).TextAlign = TextAlignEnum.CenterCenter
        End With

        With vsfQuery
            .Rows.Count = 1
            .Cols.Count = 1
            .SetData(0, vsfQName, "Query")
            Call SetFlexRowColor(vsfQuery, "", vsfQName)
            .Rows(0).TextAlign = TextAlignEnum.CenterCenter

        End With

        Call fillFormProperties(Me)
        Call initGridStyles(Me)
        Call bindCombo(cboGrp, "QGroup", "QGroup", "Select distinct QGroup from RptQuery")
        Call loadLstTbls()
        'Call loadQuerylist()
        txtQuery.Text = "Select * from "
        txtTableName.Focus()
        btnNew.Visible = True
    End Sub
    Private Sub loadLstTbls()
        Dim i As Integer
        Dim rsF As oledb.oledbDataReader

        i = vsfDet.Rows.Count
        rsF = clsSVT.getDataReaderFromSQL("Select Name from SysObjects where xType = 'U' and Name like'Mst%' or Name like'Trn%'  Order by Name")
        With rsF
            If Not .HasRows Then
                MsgBox("No Records found!", MsgBoxStyle.Information, Me.Text)
                Exit Sub
            End If
            Do While .Read

                vsfDet.Rows.Count = vsfDet.Rows.Count + 1
                vsfDet.SetData(i, vdetTblName, !Name)
                i = i + 1
            Loop
            .Close()
        End With
    End Sub
    Private Sub loadQuerylist()
        Dim i As Integer
        Dim rsF As oledb.oledbDataReader

        If cboGrp.Text = "" Then
            MsgBox("Please Select Group Name from the list", MsgBoxStyle.Information, Me.Text)
            cboGrp.Focus()
            Exit Sub
        End If

        vsfQuery.Rows.Count = 1
        i = vsfQuery.Rows.Count
        rsF = clsSVT.getDataReaderFromSQL("Select QName from RptQuery where QGroup='" & cboGrp.Text & "' Order by QName")
        With rsF
            If Not .HasRows Then
                MsgBox("No Records found!", MsgBoxStyle.Information, Me.Text)
                Exit Sub
            End If
            Do While .Read
                vsfQuery.Rows.Count = vsfQuery.Rows.Count + 1
                vsfQuery.SetData(i, vsfQName, !QName)
                i = i + 1
            Loop
            .Close()
        End With

        With vsfQuery
            .Row = 1
            .Col = 0
            .Focus()
        End With
    End Sub
    Private Sub vsfDet_BeforeEdit(sender As Object, e As RowColEventArgs) Handles vsfDet.BeforeEdit
        e.Cancel = True
    End Sub

    Private Sub vsfQuery_BeforeEdit(sender As Object, e As RowColEventArgs) Handles vsfQuery.BeforeEdit
        e.Cancel = True
    End Sub

    Private Sub vsfQuery_DoubleClick(sender As Object, e As EventArgs) Handles vsfQuery.DoubleClick
        If vsfQuery.Rows.Count < 2 Then Exit Sub
        With vsfQuery
            Dim i As Integer = .Row
            txtTableName.Text = .GetData(.Row, vsfQName)
            txtTableName.ReadOnly = True
            txtQuery.ReadOnly = True
            txtQuery.Text = clsSVT.getDataStringValue("Select Query from RptQuery where QName='" & .GetData(.Row, vsfQName) & "'", "", False)
        End With
    End Sub

    Private Sub executeQuery()

        Dim i As Integer
        Dim strsql As String
        Dim rsDummy As oledb.oledbDataReader

        Dim strFMT As String
        Try
            strsql = txtQuery.Text
            rsDummy = clsSVT.getDataReaderFromSQL(strsql)

            With rsDummy
                strFMT = ""
                For i = 0 To .FieldCount - 1
                    strFMT = strFMT & .GetName(i) & " |"
                Next i
                If strFMT <> "" Then
                    strFMT = Mid(strFMT, 1, Len(strFMT) - 1)
                End If
            End With
            rsDummy.Close()
            rsDummy = Nothing

            Dim frmDisplay As New frmUtlViewExport
            With frmDisplay
                Call .initScreenSQL(True, strFMT, txtQuery.Text, False, "", "", "", "", 0)
                .Show()
            End With
            Exit Sub

        Catch ex As Exception
            MsgBox("Error on Executing query" & Err.Description, MsgBoxStyle.Information, Me.Text)
            Exit Sub
        End Try
    End Sub

    Private Sub vsfDet_DoubleClick(sender As Object, e As EventArgs) Handles vsfDet.DoubleClick
        Dim i As Integer
        Dim rsDummy As oledb.oledbDataReader
        Dim strsql As String
        Dim strFMT As String

        strsql = "Sp_Columns " & vsfDet.GetData(vsfDet.Row, vdetTblName)
        rsDummy = clsSVT.getDataReaderFromSQL(strsql)

        With rsDummy
            strFMT = ""
            For i = 0 To 7
                strFMT = strFMT & .GetName(i) & " |"
            Next i
            If strFMT <> "" Then
                strFMT = Mid(strFMT, 1, Len(strFMT) - 1)
            End If
        End With
        rsDummy.Close()
        rsDummy = Nothing


        Dim frmDisplayP As New frmUtlViewExport
        With frmDisplayP
            Call .initScreenSQL(False, strFMT, "Sp_columns " & vsfDet.GetData(vsfDet.Row, vdetTblName), False, "", "", 0, 1, 2)
            .StartPosition = FormStartPosition.CenterScreen
            .Show()
            .vsfView.Cols(0).Visible = False
            .vsfView.Cols(1).Visible = False
            .vsfView.Cols(4).Visible = False
            .vsfView.Cols(6).Visible = False
            .Text = "Design of " & vsfDet.GetData(vsfDet.Row, vdetTblName)
        End With

    End Sub

    Private Sub vsfDet_MouseClick(sender As Object, e As MouseEventArgs) Handles vsfDet.MouseClick
        If e.Button = MouseButtons.Right Then
            txtQuery.Text = txtQuery.Text & vsfDet.GetData(vsfDet.Row, vdetTblName)
        End If
    End Sub

    Private Sub txtTableName_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtTableName.KeyPress
        If e.KeyChar = "'" Then
            e.Handled = True
        End If

        If Not Char.IsLetter(e.KeyChar) AndAlso Not e.KeyChar = Chr(Keys.Back) And Not e.KeyChar = Chr(Keys.Space) AndAlso Not e.KeyChar = Chr(Keys.Delete) Then
            e.Handled = True
        End If
    End Sub

    Private Sub btnRemoveQuery_Click(sender As Object, e As EventArgs) Handles btnRemoveQuery.Click
        If vsfQuery.Rows.Count < 2 Then Exit Sub
        Dim strsql As String
        If Not chkUserRights(strRights, chkRightsPos.DelRights, Me.Text) Then Exit Sub
        If MsgBox("Do You want to Remove the Query --> " & vsfQuery.GetData(vsfQuery.Row, vsfQName) & " ?", vbInformation + vbYesNo, Me.Text) = vbYes Then
            strsql = "delete from RptQuery where QName='" & Trim(vsfQuery.GetData(vsfQuery.Row, vsfQName)) & "'"
            clsSVT.execSQLStatement(strsql)
            vsfQuery.RemoveItem(vsfQuery.Row)

        End If
        If vsfQuery.Rows.Count = 1 Then vsfQuery.Rows.Count = 2
    End Sub

    Private Sub cboGrp_Validating(sender As Object, e As CancelEventArgs) Handles cboGrp.Validating
        Call loadQuerylist()
    End Sub

    Private Sub btnResult_Click_1(sender As Object, e As EventArgs) Handles btnResult.Click
        If Trim(txtQuery.Text) = "" Then Exit Sub
        If gUserCode <> "ADMIN" Then Exit Sub
        If InStr(UCase(txtQuery.Text), "DELETE") > 0 Then
            MsgBox("Delete action is not allowed!!", MsgBoxStyle.Information, Me.Text)
            txtQuery.Text = ""
            Exit Sub
        End If
        If InStr(UCase(txtQuery.Text), "UPDATE") > 0 Then
            MsgBox("Update action is not allowed!!", MsgBoxStyle.Information, Me.Text)
            txtQuery.Text = ""
            Exit Sub
        End If
        Call executeQuery()
    End Sub

    Private Sub btnNew_Click(sender As Object, e As EventArgs) Handles btnNew.Click
        If Not chkUserRights(strRights, chkRightsPos.AddRights, Me.Text) Then Exit Sub
        Call ClearFlds()
        txtTableName.ReadOnly = False
        txtQuery.ReadOnly = False
        txtTableName.Focus()
        btnNew.Visible = False
        btnEdit.Visible = False
    End Sub


    Private Sub ClearFlds()
        cboGrp.Text = ""
        txtTableName.Text = ""
        txtQuery.Text = ""
    End Sub
    Private Sub btnSave_Click_1(sender As Object, e As EventArgs) Handles btnSave.Click
        Dim svtTrn As oledb.oledbTransaction
        Dim strSql As String
        Dim blnFound As Boolean

        If cboGrp.Text = "" Then
            MsgBox("Please enter Group name", MsgBoxStyle.Information, Me.Text)
            cboGrp.Focus()
            Exit Sub
        End If

        If txtTableName.Text = "" Then
            MsgBox("Please enter table name", MsgBoxStyle.Information, Me.Text)
            txtTableName.Focus()
            Exit Sub
        End If

        clsSVT.getDataLongValue("select QID from RptQuery where QName='" & txtTableName.Text & "'", 0, blnFound)
        If blnFound Then
            MsgBox("Table name already Exist!", vbInformation, Me.Text)
            txtTableName.Text = ""
            txtTableName.Focus()
            txtTableName.ReadOnly = False
            Exit Sub
        End If

        svtTrn = svtConn.BeginTransaction(IsolationLevel.ReadCommitted)
        Try
            strSql = "insert into RptQuery (Query,QName,QGroup)"
            strSql = strSql & "values ('" & txtQuery.Text & "','" & txtTableName.Text & "', '" & cboGrp.Text & "')"
            clsSVT.execSQLStatement(strSql, svtTrn)
            MsgBox("Query Saved successfully.", vbInformation, Me.Text)
            svtTrn.Commit()
            Call loadQuerylist()
        Catch ex As Exception
            MsgBox("Error during save" & vbCrLf & ex.Message, MsgBoxStyle.Critical, Me.Text)
            svtTrn.Rollback()
        End Try
        btnNew.Visible = True
        Cancel_Click(Nothing, Nothing)
    End Sub

    Private Sub btnClose_Click_1(sender As Object, e As EventArgs) Handles btnClose.Click
        Dim tb As New C1.Win.C1Command.C1DockingTabPage()
        Dim dockTabMain As New C1.Win.C1Command.C1DockingTab()
        Me.Dispose()
        tb.Controls.Clear()
        frmMain.dockTabMain.SelectedTab.Dispose()
    End Sub

    Private Sub btnLoad_Click(sender As Object, e As EventArgs) Handles btnLoad.Click
        loadQuerylist()
    End Sub

    Private Sub frmRptQuery_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        txtTableName.Focus()
    End Sub

    Private Sub Cancel_Click(sender As Object, e As EventArgs) Handles Cancel.Click
        Call bindCombo(cboGrp, "QGroup", "QGroup", "Select distinct QGroup from RptQuery")
        ' Call loadLstTbls()
        'Call loadQuerylist()
        txtQuery.Text = "Select * from "
        txtTableName.Text = ""
        txtTableName.Focus()
        btnNew.Visible = True
        btnEdit.Visible = True
        txtTableName.ReadOnly = False
        txtQuery.ReadOnly = False
    End Sub

    Private Sub btnEdit_Click(sender As Object, e As EventArgs) Handles btnEdit.Click
        txtTableName.ReadOnly = False
        txtQuery.ReadOnly = False
        txtTableName.Focus()
    End Sub
End Class