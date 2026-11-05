
Imports C1.Win.C1FlexGrid

Public Class frmutlEnvDesign

    Const vdetSlno = 0
    Const vDetFldSettings = 1

    Const vsfDataFld = 0

    Public strDocName As String
    Public Mode As String
    Dim clsSvt As New svtLibrary
    Dim _src As C1FlexGrid = Nothing

    Private Sub frmutlEnvDesign_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.KeyPreview = True
        Call initFormSize(Me, Me.Text, False, 915, 718, 0, 0)
        Call initFormVsfDet(Me, Me.Text, vsfDet, "vDet")
        Call Clearfields()
        Call initScreen()
        initGridStyles(Me)

        Call bindCombo(CboTemplate, "TemplateName", "TemplateName", "select TemplateName from UtlPartyEnv where TemplateName <>'' and DocName='" & strDocName & "' order by TemplateName ")
        GetDataFields()

    End Sub
    Private Sub initScreen()
        With vsfDet
            .Rows.Count = 21
            .Cols.Count = 2
            .SetData(0, vDetFldSettings, "Field Settings")
            .Cols(vDetFldSettings).TextAlign = TextAlignEnum.LeftCenter
            For i = 1 To vsfDet.Rows.Count - 1
                .SetData(i, vdetSlno, i)
                .Cols(vdetSlno).Visible = False
            Next
            .DragMode = DragModeEnum.Manual
            .DropMode = DropModeEnum.Manual
        End With
        With vsfColumn
            .Rows.Count = 1
            .Cols.Count = 1
            .SetData(0, vsfDataFld, "Fields")
        End With
        Mode = "V"
        Call EnalbeDisable(False)
    End Sub

    Private Sub btnCopyFrom_Click(sender As System.Object, e As System.EventArgs) Handles btnCopyFrom.Click
        Dim strSQL As String, strFMT As String
        strFMT = ""
        Try
            strSQL = "Select EnvId,TemplateName,ReportName"
            strSQL = strSQL & " from UtlPartyEnv"
            strSQL = strSQL & " where EnvId > 0"
            strSQL = strSQL & " and DocName='" & strDocName & "'"
            strSQL = strSQL & " order by TemplateName"

            Dim frmDisplay As New frmUtlSingleSel

            With frmDisplay
                .Text = "Select the Template to copy"
                Call .FillViewVsf(Me, strSQL, txtCopyItem, 0, 0)
                .ShowDialog()
                .vsfDet.Focus()
            End With

            Call fillGrid(Val(txtCopyItem.Text), True)
            PnlCopyFrom.Visible = True
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, Me.Text)
            Cursor.Current = Cursors.Default
        End Try
    End Sub
    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Dim svtTrn As oledb.oledbTransaction
        Dim strSQL As String
        Dim i As Integer

        If Not ValidFields() Then Exit Sub
        Cursor.Current = Cursors.WaitCursor
        svtTrn = svtConn.BeginTransaction(IsolationLevel.ReadCommitted)
        Try
            If Mode = "M" Then
                clsSvt.execSQLStatement("Delete from UtlPartyEnv where EnvId=" & txtEnvID.Text, svtTrn)
            End If

            'If vsfDet.GetData(i, vDetFldSettings) <> "" Then
            strSQL = "insert into UtlPartyEnv"
            strSQL = strSQL & "( EnvID,DocName,TemplateName,ReportName,AddnlTbls,AddnlCond,"
            strSQL = strSQL & " FLDContent1,FLDContent2,FLDContent3,FLDContent4,FLDContent5,FLDContent6,"
            strSQL = strSQL & " FLDContent7,FLDContent8,FLDContent9,FLDContent10,FLDContent11,FLDContent12,FLDContent13,FLDContent14,"
            strSQL = strSQL & " FLDContent15,FLDContent16,FLDContent17,FLDContent18,FLDContent19,FLDContent20)"
            strSQL = strSQL & "values ("
            strSQL = strSQL & " " & Val(txtEnvID.Text) & ","
            strSQL = strSQL & "'" & strDocName & "',"
            strSQL = strSQL & " '" & Trim(CboTemplate.Text) & "',"
            strSQL = strSQL & " '" & Trim(txtRptName.Text) & "',"
            strSQL = strSQL & "'" & (txtTable.Text) & "',"
            strSQL = strSQL & "'" & Trim(txtCondn.Text) & "',"
            For i = 1 To vsfDet.Rows.Count - 1
                If vsfDet.GetData(i, vDetFldSettings) <> "" Then
                    strSQL = strSQL & "'" & vsfDet.GetData(i, vDetFldSettings).Replace("'", "`") & "',"
                Else
                    strSQL = strSQL & "'" & "" & "',"
                End If
            Next i
            strSQL = Mid(strSQL, 1, Len(strSQL) - 1)
            strSQL = strSQL & ")"
            If Not clsSvt.execSQLStatement(strSQL, svtTrn) Then
                svtTrn.Rollback()
                Exit Sub
            End If
            'End If
            MsgBox("Template Saved Successfully!", MsgBoxStyle.Information, Me.Text)
            svtTrn.Commit()
            fillGrid(txtEnvID.Text, False)
            Call bindCombo(CboTemplate, "TemplateName", "TemplateName", "select TemplateName from UtlPartyEnv where TemplateName <>'' and DocName='" & strDocName & "' order by TemplateName ")

        Catch ex As Exception
            MsgBox("Error during Update" & vbCrLf & ex.Message, MsgBoxStyle.Critical, Me.Text)
            svtTrn.Rollback()
            Cursor.Current = Cursors.Default
        End Try
        Cursor.Current = Cursors.Default
        PnlCopyFrom.Visible = True
        Mode = "V"
        Call EnalbeDisable(False)
        Call Clearfields()
    End Sub

    Public Sub fillGrid(ByVal argEnvId As Integer, Optional argCopyItem As Boolean = False)
        Dim rstemp As oledb.oledbDataReader
        Dim strSql As String
        Dim i As Integer
        rstemp = Nothing
        Try
            strSql = "select * from UtlPartyEnv where Envid=" & argEnvId
            rstemp = clsSvt.getDataReaderFromSQL(strSql)
            With rstemp
                If .Read() Then
                    If argCopyItem = False Then
                        CboTemplate.Text = GetVal(rstemp("TemplateName"), "")
                        txtEnvID.Text = (GetVal(rstemp("EnvID"), 0))
                    End If
                    txtRptName.Text = GetVal(rstemp("ReportName"), "")
                    txtTable.Text = GetVal(rstemp("AddnlTbls"), "")
                    txtCondn.Text = GetVal(rstemp("AddnlCond"), "")
                    For i = 1 To vsfDet.Rows.Count - 1
                        vsfDet.SetData((i), vDetFldSettings, GetVal(rstemp("FLDContent" & i), ""))
                    Next
                Else
                    txtRptName.Text = ""
                    vsfDet.Rows.Count = 1
                    vsfDet.Rows.Count = 21
                End If
                .Close()
            End With
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, Me.Text)
            rstemp.Close()
            Cursor.Current = Cursors.Default
        End Try
    End Sub

    Private Function ValidFields() As Boolean
        Dim blnfound As Boolean = False
        ValidFields = False
        If Mode = "A" Then
            clsSvt.getDataLongValue("select EnvID from UtlPartyEnv where TemplateName='" & Trim(CboTemplate.Text) & "'", 0, blnfound)
            If blnfound Then
                MsgBox("Template Name already Exist!", MsgBoxStyle.Information, Me.Text)
                CboTemplate.Text = ""
                CboTemplate.Focus()
                Exit Function
            End If
        End If
        If CboTemplate.Text = "" Then
            MsgBox("Template cannot be blank!", MsgBoxStyle.Information, Me.Text)
            CboTemplate.Focus()
            Exit Function
        End If

        If Trim(txtRptName.Text) = "" Then
            MsgBox("Report cannot be blank!", MsgBoxStyle.Information, Me.Text)
            txtRptName.Focus()
            Exit Function
        End If

        ValidFields = True
    End Function

    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        txtEnvID.Text = clsSvt.AutoIncrementedNum("ENVID", "UtlPartyEnv")
        Mode = "A"
        Call Clearfields()
        Call EnalbeDisable(True)
        btnEdit.Visible = False
        btnDelete.Visible = False
    End Sub

    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        If CboTemplate.Text = "" Then Exit Sub

        Dim blnFound As Boolean
        Dim svtTrn As oledb.oledbTransaction
        Cursor.Current = Cursors.WaitCursor
        svtTrn = svtConn.BeginTransaction(IsolationLevel.ReadCommitted)
        txtEnvID.Text = clsSvt.getDataStringValue("Select EnvID from UtlPartyEnv where TemplateName='" & CboTemplate.Text & "'", "", blnFound, svtTrn)
        If Not blnFound Then
            MsgBox("Template is not found", MsgBoxStyle.Information, Me.Text)
            CboTemplate.Focus()
            Exit Sub
        End If
        Try
            If MsgBox("Are you sure you want to delete the Template  ? " &
            CboTemplate.Text, vbCritical + MsgBoxStyle.YesNo + vbDefaultButton2, "Delete Confirmation") = MsgBoxResult.Yes Then
                clsSvt.execSQLStatement("Delete from UtlPartyEnv where EnvId=" & txtEnvID.Text, svtTrn)
                MsgBox("Template " & CboTemplate.Text & " Deleted Successfully", MsgBoxStyle.Information, Me.Text)
                Call Clearfields()

            End If
            svtTrn.Commit()
            Cursor.Current = Cursors.Default
            Mode = "V"
            Call EnalbeDisable(False)
            Call bindCombo(CboTemplate, "TemplateName", "TemplateName", "select TemplateName from UtlPartyEnv where TemplateName <>'' and DocName='" & strDocName & "' order by TemplateName ",)
        Catch ex As Exception
            MsgBox("Erron on Deletion! " & vbCrLf & ex.Message, MsgBoxStyle.Critical, Me.Text)
            svtTrn.Rollback()
            Cursor.Current = Cursors.Default
        End Try
    End Sub
    Private Sub GetDataFields()
        Dim rs As oledb.oledbDataReader
        Dim StrTbleName As String
        Dim strsql As String
        Dim i As Integer

        Cursor.Current = Cursors.WaitCursor

        StrTbleName = clsSvt.getDataStringValue("select HdrTblName from UtlTransactions where DocName='" & strDocName & "'", "", False)
        strsql = ("select Column_Name from Information_Schema.columns where Table_Name='" & StrTbleName & "'")
        rs = clsSvt.getDataReaderFromSQL(strsql)
        Try
            vsfColumn.Rows.Count = 1
            With rs
                If Not .HasRows Then
                    .Close()
                    Exit Sub
                End If
                Do While .Read()
                    vsfColumn.Rows.Count = vsfColumn.Rows.Count + 1
                    i = vsfColumn.Rows.Count - 1
                    vsfColumn.SetData(i, vsfDataFld, GetVal(rs("Column_Name"), ""))
                Loop
                .Close()
            End With
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, Me.Text)
            rs.Close()
            Cursor.Current = Cursors.Default
        End Try
    End Sub
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles btnload.Click
        Dim blnFound As Boolean
        Cursor.Current = Cursors.WaitCursor
        txtEnvID.Text = clsSvt.getDataStringValue("Select EnvID from UtlPartyEnv where TemplateName='" & CboTemplate.Text & "' And DocName='" & strDocName & "'", "", blnFound)
        If Not blnFound Then
            MsgBox("Template is not found", MsgBoxStyle.Information, Me.Text)
            Call Clearfields()
            CboTemplate.Focus()
            Cursor.Current = Cursors.Default
            Exit Sub
        End If

        Call fillGrid(Val(txtEnvID.Text), False)
        Cursor.Current = Cursors.Default
        btnSave.Visible = False
        btnCancel.Visible = False
        btnEdit.Visible = True
        btnDelete.Visible = True
        txtRptName.Enabled = False
        CboTemplate.Enabled = True
        Mode = "V"

    End Sub
    Private Sub Clearfields()
        txtRptName.Text = ""
        CboTemplate.Text = ""
        txtTable.Text = ""
        txtCondn.Text = ""
        vsfDet.Rows.Count = 1
        vsfDet.Rows.Count = 21
    End Sub

    Private Sub frmutlEnvDesign_KeyUp(sender As Object, e As KeyEventArgs) Handles Me.KeyUp
        If e.KeyCode = Keys.F5 Then
            Call bindCombo(CboTemplate, "TemplateName", "TemplateName", "select TemplateName from UtlPartyEnv where TemplateName <>'' and DocName='" & strDocName & "' order by TemplateName ")
        End If
        If e.KeyCode = Keys.F9 And e.Control = True Then
            Call initFormSize(Me, Me.Text, True, 915, 718, 0, 0)
            MsgBox("Restored Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
        If e.KeyCode = Keys.F8 And e.Control = True Then
            Call SaveActFormSettings(Me, Me.Text)
            Call SaveActFormVsfDet(Me, Me.Text, "DET", vsfDet)
            MsgBox("Saved Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
    End Sub

    Private Sub vsfDet_KeyPress(sender As Object, e As KeyPressEventArgs) Handles vsfDet.KeyPress
        If e.KeyChar = "'" Then
            e.Handled = True
        End If
    End Sub
    Private Sub CboTemplate_KeyPress(sender As Object, e As KeyPressEventArgs) Handles CboTemplate.KeyPress
        If e.KeyChar = "'" Then
            e.Handled = True
        End If
    End Sub

    Private Sub txtRptName_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtRptName.KeyPress
        If e.KeyChar = "'" Then
            e.Handled = True
        End If
    End Sub

    Private Sub vsfColumn_DoubleClick(sender As Object, e As EventArgs) Handles vsfColumn.DoubleClick
        If Mode = "V" Then Exit Sub
        With vsfColumn
            If vsfDet.GetData(vsfDet.Row, vDetFldSettings) = "" Then
                vsfDet.SetData(vsfDet.Row, vDetFldSettings, "{" & .GetData(.Row, vsfDataFld) & "}")
            Else
                If InStr(vsfDet.GetData(vsfDet.Row, vDetFldSettings), "{}") > 0 Then
                    vsfDet.SetData(vsfDet.Row, vDetFldSettings, Replace(vsfDet.GetData(vsfDet.Row, vDetFldSettings), "{}", "{" & vsfColumn.GetData(.Row, vsfDataFld) & "}"))
                Else
                    vsfDet.SetData(vsfDet.Row, vDetFldSettings, vsfDet.GetData(vsfDet.Row, vDetFldSettings) & ",{" & .GetData(.Row, vsfDataFld) & "}")
                End If
            End If

        End With
    End Sub

    Private Sub vsfColumn_BeforeEdit(sender As Object, e As RowColEventArgs) Handles vsfColumn.BeforeEdit
        e.Cancel = True
    End Sub

    Private Sub btnRemoveQuery_Click(sender As Object, e As EventArgs) Handles btnRemoveQuery.Click
        If Mode = "V" Then Exit Sub
        If vsfDet.Rows.Count < 1 Then Exit Sub
        If vsfDet.GetData(vsfDet.Row, vDetFldSettings) <> "" Then
            If MsgBox("Do You want to Remove the Data --> " & vsfDet.GetData(vsfDet.Row, vDetFldSettings) & " ?", vbInformation + vbYesNo, Me.Text) = vbYes Then
                vsfDet.RemoveItem(vsfDet.Row)
                vsfDet.Rows.Count = vsfDet.Rows.Count + 1
            End If
        End If
        If vsfDet.Rows.Count = 1 Then vsfDet.Rows.Count = 2
    End Sub

    Private Sub btnEdit_Click(sender As Object, e As EventArgs) Handles btnEdit.Click
        Mode = "M"
        Call EnalbeDisable(True)
        PnlCopyFrom.Visible = False
        btnAdd.Visible = False
    End Sub

    Private Sub EnalbeDisable(ByVal argFlag As Boolean)

        btnClose.Visible = Not argFlag
        btnAdd.Visible = Not argFlag
        btnload.Visible = Not argFlag
        txtRptName.Enabled = argFlag
        btnSave.Visible = argFlag
        btnEdit.Visible = argFlag
        btnDelete.Visible = argFlag
        btnCancel.Visible = argFlag
        PnlCopyFrom.Visible = argFlag
        txtCondn.Enabled = argFlag
        txtTable.Enabled = argFlag
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Call Clearfields()
        Mode = "V"
        Call EnalbeDisable(False)
    End Sub
    Private Sub btnMoveUP_Click(sender As System.Object, e As System.EventArgs)
        If Mode = "V" Then Exit Sub
        If vsfDet.Row > 1 Then
            Call MoveOneRowUp(vsfDet, vdetSlno)
        End If

    End Sub

    Private Sub btnMoveDown_Click(sender As System.Object, e As System.EventArgs)
        If Mode = "V" Then Exit Sub
        If vsfDet.Row > 1 Then
            Call MoveOneRowDown(vsfDet, vdetSlno)
        End If
    End Sub

    Private Sub vsfDet_SetupEditor(sender As Object, e As RowColEventArgs) Handles vsfDet.SetupEditor
        If Mode = "V" Then Exit Sub
        With vsfDet
            Select Case e.Col
                Case vDetFldSettings
                    If TypeOf (vsfDet.Editor) Is TextBox Then
                        Dim tb As TextBox = vsfDet.Editor
                        tb.MaxLength = 250
                    End If
            End Select
        End With
    End Sub

    Private Sub txtTable_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtTable.KeyPress
        If e.KeyChar = "'" Then
            e.Handled = True
        End If
    End Sub

    Private Sub txtCondn_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtCondn.KeyPress
        If e.KeyChar = "'" Then
            e.Handled = True
        End If
    End Sub

    Private Sub vsfDet_BeforeEdit(sender As Object, e As RowColEventArgs) Handles vsfDet.BeforeEdit

        If Mode = "V" Then
            e.Cancel = True
        End If
        If (Mode = "A" Or Mode = "M") Then
            e.Cancel = False
        End If
    End Sub

    'Private Sub vsfDet_BeforeMouseDown(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.BeforeMouseDownEventArgs) Handles vsfDet.BeforeMouseDown
    '    'start dragging when the user clicks the cell
    '    Dim flex As C1FlexGrid = CType(sender, C1FlexGrid)
    '    Dim hti As HitTestInfo = flex.HitTest(e.X, e.Y)

    '    If hti.Type = HitTestTypeEnum.Cell Then
    '        'select the row 
    '        Dim index As Integer = hti.Row
    '        flex.Select(index, 0, index, flex.Cols.Count - 1, False)

    '        'save info for target
    '        _src = flex

    '        'do drag drop
    '        Dim dd As DragDropEffects = flex.DoDragDrop(flex.Clip, DragDropEffects.Move)

    '        'if it worked, delete row from source (it's a move) 
    '        If dd = DragDropEffects.Move Then
    '            flex.Rows.Remove(index)
    '        End If

    '        'done, reset info
    '        _src = Nothing
    '    End If
    'End Sub


    'Private Sub vsfDet_DragOver(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles vsfDet.DragOver
    '    'check that we have the type of data we want
    '    If e.Data.GetDataPresent(GetType(String)) Then
    '        e.Effect = DragDropEffects.Move
    '    End If
    'End Sub

    'Private Sub vsfdet_DragDrop(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles vsfDet.DragDrop
    '    'find the drop position
    '    Dim flex As C1FlexGrid = CType(sender, C1FlexGrid)
    '    Dim pt As Point = flex.PointToClient(New Point(e.X, e.Y))
    '    Dim hti As HitTestInfo = flex.HitTest(pt.X, pt.Y)

    '    'newindex - the index where the row is to be moved to
    '    'oldindex - the index from where the row was moved from
    '    Dim newindex As Integer = hti.Row
    '    Dim oldindex As Integer = flex.RowSel

    '    If newindex < 0 Then newindex = flex.Rows.Count
    '    If newindex < 1 Then newindex = 1

    '    Dim row As C1.Win.C1FlexGrid.Row

    '    If oldindex > 1 And newindex < oldindex Then    'going upwards
    '        row = flex.Rows(oldindex)
    '    End If

    '    If newindex > oldindex And (oldindex + 1) < flex.Rows.Count Then   'coming downwards
    '        row = flex.Rows(oldindex + 1)
    '    End If

    '    ' remove the row from the earlier position
    '    flex.Rows.Remove(oldindex)

    '    If newindex > flex.Rows.Count Then           ' if the row to be added is the last row
    '        'insert a new row at the drop position
    '        flex.Rows.Insert(newindex - 1)

    '        'copy data from source row
    '        flex.Select(newindex - 1, 0, newindex - 1, flex.Cols.Count - 1, False)
    '        flex.Clip = CType(e.Data.GetData(GetType(String)), String)
    '    Else
    '        'insert a new row at the drop position
    '        flex.Rows.Insert(newindex)

    '        'copy data from source row
    '        flex.Select(newindex, 0, newindex, flex.Cols.Count - 1, False)
    '        flex.Clip = CType(e.Data.GetData(GetType(String)), String)

    '    End If

    '    flex.Rows.Insert(oldindex)
    'End Sub
End Class