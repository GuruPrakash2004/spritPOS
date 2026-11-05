Imports C1.Win.C1FlexGrid
Imports System.Data.oledb
Imports System.IO

Public Class frmUtlPostDocDesign
    Dim strSQL As String
    Dim clsSVT As New svtLibrary
    Dim svtTrn As oledb.oledbTransaction
    Dim blnPKSection As Boolean = True
    Dim blnDetSection As Boolean = True
    Public Mode As String

    Public intPostDocId As Integer

    Const vDetSlNo = 0
    Const vDetTableName = 1
    Const vDetFieldName = 2
    Const vDetCaption = 3
    Const vDetDateFilter = 4
    Const vDetHidden = 5
    Const vDetAllowingEditing = 6
    Const vDetTotal = 7

    Const vEdtSlNo = 0
    Const vEdtTableName = 1
    Const vEdtPk = 2
    Const vEdtTotal = 3

    Public Sub InitScreen()
        With vsfDetail
            .Rows.Count = 1
            .Cols.Count = vDetTotal
            .SetData(0, vDetSlNo, "SL No")
            .SetData(0, vDetTableName, "Table Name")
            .SetData(0, vDetFieldName, "Field Name")
            .SetData(0, vDetCaption, "Caption")
            .SetData(0, vDetDateFilter, "Date Filter")
            .SetData(0, vDetHidden, "Hidden")
            .SetData(0, vDetAllowingEditing, "Allow Edit")
            SetFlexColor(vsfDetail, "M", vDetTableName, vDetFieldName, vDetCaption)
            SetFlexColor(vsfDetail, "", vDetSlNo)
        End With

        With vsfPK
            .Rows.Count = 1
            .Cols.Count = vEdtTotal
            .SetData(0, vEdtSlNo, "SL No")
            .SetData(0, vEdtTableName, "Table Name")
            .SetData(0, vEdtPk, "Primary Col")
            SetFlexColor(vsfPK, "", vEdtSlNo)
            SetFlexColor(vsfPK, "M", vEdtTableName, vEdtPk)
        End With
    End Sub

    Private Sub clearflds()
        txtName.Text = ""
        txtRmks.Text = ""
        txtCond.Text = ""
        txtPendCond.Text = ""
        vsfDetail.Rows.Count = 1
        vsfPK.Rows.Count = 1

    End Sub

    Private Sub frmUtlPostDocDesign_Load(sender As Object, e As EventArgs) Handles Me.Load
        Call initFormVsfDet(Me, Me.Text, vsfDetail, "DET")
        Call initFormVsfDet(Me, Me.Text, vsfPK, "PRIMARY")
        Call fillFormProperties(Me)
        Call initGridStyles(Me)
        Call InitScreen()
        Call clearflds()

        If intPostDocId > 0 Then
            Call fillDetails()
            txtName.ReadOnly = True
        Else
            txtName.ReadOnly = False
            vsfDetail.Rows.Count = vsfDetail.Rows.Count + 1
            vsfPK.Rows.Count = vsfPK.Rows.Count + 1
        End If

        Call ReArrangeSlNo(vsfDetail, vDetSlNo)
        Call ReArrangeSlNo(vsfPK, vEdtSlNo)
        Call VisibilityUcBtns()
    End Sub

    Private Sub frmUtlPostDocDesign_KeyUp(sender As Object, e As KeyEventArgs) Handles Me.KeyUp
        If e.KeyCode = Keys.F9 And e.Control Then
            Call initFormVsfDet(Me, Me.Text, vsfDetail, "DET")
            Call initFormVsfDet(Me, Me.Text, vsfPK, "PRIMARY")
            MsgBox("Restored Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If

        If e.KeyCode = Keys.F8 And e.Control = True Then
            Call SaveActFormVsfDet(Me, Me.Text, "DET", vsfDetail)
            Call SaveActFormVsfDet(Me, Me.Text, "PRIMARY", vsfPK)
            MsgBox("Saved Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If

        'If vsfDetail.Rows.Count > 1 AndAlso e.KeyCode = Keys.D AndAlso e.Control = True Then
        '    For i = vsfDetail.Rows.Count - 1 To vsfDetail.Rows.Count - 1 Step -1
        '        vsfDetail.Rows.Add()
        '        For j = 1 To vsfDetail.Cols.Count - 1
        '            vsfDetail.SetData(i + 1, j, vsfDetail.GetData(i, j))
        '        Next
        '    Next
        '    Call ReArrangeSlNo(vsfDetail, vDetSlNo)
        'End If
    End Sub

    Private Sub vsfDetail_BeforeEdit(sender As Object, e As RowColEventArgs) Handles vsfDetail.BeforeEdit
        If vsfDetail.Enabled = False Then Exit Sub
        vsfDetail.ComboList = ""
        If e.Col = vDetDateFilter Or e.Col = vDetHidden Or e.Col = vDetAllowingEditing Then
            vsfDetail.ComboList = "Y|N"
            Exit Sub
        End If

        If e.Col = vDetTableName Then
            vsfDetail.ComboList = GenVSfComboStringCond("Name", "Sys.Tables", "Name")
            Exit Sub
        End If

        If e.Col = vDetFieldName Then
            vsfDetail.ComboList = GenVSfComboStringCond("column_name", "Information_schema.Columns", " order by Column_name", " where table_name='" & vsfDetail.GetData(vsfDetail.Row, vDetTableName) & "'")
            Exit Sub
        End If

    End Sub

    Private Sub vsfEditingColumnsPK_BeforeEdit(sender As Object, e As RowColEventArgs) Handles vsfPK.BeforeEdit
        If vsfPK.Enabled = False Then Exit Sub
        vsfPK.ComboList = ""

        If e.Col = vEdtTableName Then
            vsfPK.ComboList = GenVSfComboStringCond("name", "sys.tables", "name")
            Exit Sub
        End If

        If e.Col = vEdtPk Then
            vsfPK.ComboList = GenVSfComboStringCond("column_name", "Information_schema.Columns", " order by Column_name", " where table_name='" & vsfPK.GetData(vsfPK.Row, vEdtTableName) & "'")
            Exit Sub
        End If
    End Sub

        Private Sub vsfDetail_KeyUp(sender As Object, e As KeyEventArgs) Handles vsfDetail.KeyUp
        If e.KeyCode = Keys.Tab AndAlso vsfDetail.Rows.Count > 1 AndAlso vsfDetail.Row >= 1 AndAlso vsfDetail.Row = vsfDetail.Rows.Count - 1 AndAlso vsfDetail.Col = vsfDetail.Cols.Count - 1 Then
            If vsfDetail.GetData(vsfDetail.Rows.Count - 1, vDetTableName) <> "" AndAlso vsfDetail.GetData(vsfDetail.Rows.Count - 1, vDetFieldName) <> "" Then
                vsfDetail.Rows.Count = vsfDetail.Rows.Count + 1
                Call ReArrangeSlNo(vsfDetail, vDetSlNo)
            End If
        End If

        Call raiseShortCutDetEvent(e)
    End Sub

    Private Sub vsfDetail_AfterEdit(sender As Object, e As RowColEventArgs) Handles vsfDetail.AfterEdit
        If e.Col = vDetFieldName Then
            vsfDetail.SetData(e.Row, e.Col + 1, vsfDetail.GetData(e.Row, e.Col))
        End If

    End Sub

    Private Sub vsfEditingColumnsPK_KeyUp(sender As Object, e As KeyEventArgs) Handles vsfPK.KeyUp
        If e.KeyCode = Keys.Tab AndAlso vsfPK.Rows.Count > 1 AndAlso vsfPK.Row >= 1 AndAlso vsfPK.Row = vsfPK.Rows.Count - 1 AndAlso vsfPK.Col = vsfPK.Cols.Count - 1 AndAlso vsfPK.Col = vsfPK.Cols.Count - 1 Then
            If vsfPK.GetData(vsfPK.Rows.Count - 1, vEdtTableName) <> "" AndAlso vsfPK.GetData(vsfPK.Rows.Count - 1, vEdtPk) <> "" Then
                vsfPK.Rows.Count = vsfPK.Rows.Count + 1
                Call ReArrangeSlNo(vsfPK, vEdtSlNo)
            End If
        End If

        Call raiseShortCutPKEvent(e)
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        vsfDetail.Enabled = False
        vsfPK.Enabled = False
        If ValidFields() Then
            Call SaveDoc()
        End If
        vsfDetail.Enabled = True
        vsfPK.Enabled = True
    End Sub

    Private Function SaveDoc() As Boolean
        SaveDoc = False
        svtTrn = svtConn.BeginTransaction(IsolationLevel.ReadCommitted)
        Dim Mode As String = ""
        Try
            If intPostDocId = 0 Then
                Mode = "A"

                strSQL = "Insert into UtlPDE ("
                strSQL &= "PostDocName,PostDocRmks,CondText,PendCond) Values ("
                strSQL &= "'" & Trim(txtName.Text) & "',"
                strSQL &= "'" & CStr(txtRmks.Text).Replace("'", "''") & "',"
                strSQL &= "'" & CStr(txtCond.Text).Replace("'", "''") & "',"
                strSQL &= "'" & CStr(txtPendCond.Text).Replace("'", "''") & "')"

                If Not clsSVT.execSQLStatement(strSQL, svtTrn) Then
                    svtTrn.Rollback()
                    Exit Function
                End If
                intPostDocId = clsSVT.getDataLongValue("Select Max(PostDocID)  From UtlPDE", 0, False, svtTrn)
            Else
                strSQL = " Update UtlPDE set "
                strSQL &= " PostDocRmks = '" & CStr(txtRmks.Text).Replace("'", "''") & "',"
                strSQL &= " CondText = '" & CStr(txtCond.Text).Replace("'", "''") & "',"
                strSQL &= " PendCond = '" & CStr(txtPendCond.Text).Replace("'", "''") & "'"
                strSQL &= " Where PostDocID = " & intPostDocId & ""

                If Not clsSVT.execSQLStatement(strSQL, svtTrn) Then
                    svtTrn.Rollback()
                    Exit Function
                End If
            End If

            If Mode <> "A" Then
                clsSVT.execSQLStatement("Delete from UtlPDEDtl where POstDocId=" & intPostDocId, svtTrn)
                clsSVT.execSQLStatement("Delete from UtlPDEPK where POstDocId=" & intPostDocId, svtTrn)
            End If

            With vsfDetail
                strSQL = ""
                For i = 1 To vsfDetail.Rows.Count - 1
                    If GetVal(.GetData(i, vDetTableName), "") <> "" AndAlso
                        GetVal(.GetData(i, vDetFieldName), "") <> "" AndAlso
                        GetVal(.GetData(i, vDetCaption), "") <> "" Then

                        strSQL &= "Insert into UtlPDEDtl ("
                        strSQL &= "PostDocID,Slno,TblName,FldName,FldCaption,DateFilter,ColHide,AllowEdit) Values ("
                        strSQL &= "" & intPostDocId & ","
                        strSQL &= "" & .GetData(i, vDetSlNo) & ","
                        strSQL &= "'" & .GetData(i, vDetTableName) & "',"
                        strSQL &= "'" & .GetData(i, vDetFieldName) & "',"
                        strSQL &= "'" & .GetData(i, vDetCaption) & "',"
                        strSQL &= "'" & GetVal(.GetData(i, vDetDateFilter), "N") & "',"
                        strSQL &= "'" & GetVal(.GetData(i, vDetHidden), "N") & "',"
                        strSQL &= "'" & GetVal(.GetData(i, vDetAllowingEditing), "N") & "')" & vbNewLine
                    End If
                Next
                If strSQL <> "" AndAlso Not clsSVT.execSQLStatement(strSQL, svtTrn) Then
                    svtTrn.Rollback()
                    Exit Function
                End If
            End With

            With vsfPK
                strSQL = ""
                Call ReArrangeSlNo(vsfPK, vEdtSlNo)
                Call RemoveEmptVSFRows(vsfPK, 0, vEdtTableName)

                For i = 1 To .Rows.Count - 1
                    If GetVal(.GetData(i, vEdtTableName), "") <> "" AndAlso
                        GetVal(.GetData(i, vEdtPk), "") <> "" Then
                        strSQL &= "Insert into UtlPDEPK	("
                        strSQL &= "PostDocID,Slno,TblName,FldName) Values ("
                        strSQL &= "" & intPostDocId & ","
                        strSQL &= "" & .GetData(i, vEdtSlNo) & ","
                        strSQL &= "'" & .GetData(i, vEdtTableName) & "',"
                        strSQL &= "'" & .GetData(i, vEdtPk) & "')" & vbNewLine
                    End If
                Next
                If strSQL <> "" AndAlso Not clsSVT.execSQLStatement(strSQL, svtTrn) Then
                    svtTrn.Rollback()
                    Exit Function
                End If
            End With

            If Mode = "A" Then
                MsgBox("PDE Created Successfully....", MsgBoxStyle.Information, Me.Text)
            Else
                MsgBox("PDE Updated Successfully....", MsgBoxStyle.Information, Me.Text)
            End If
            svtTrn.Commit()
        Catch ex As Exception
            svtTrn.Rollback()
            MsgBox(ex.Message, MsgBoxStyle.Information, Me.Text)
        End Try
    End Function

    Private Function ValidFields() As Boolean

        ValidFields = False

        vsfDetail.EndUpdate()
        vsfDetail.FinishEditing()

        vsfPK.EndUpdate()
        vsfPK.FinishEditing()

        Call ReArrangeSlNo(vsfDetail, vDetSlNo)
        Call RemoveEmptVSFRows(vsfDetail, 0, vDetTableName)

        Call ReArrangeSlNo(vsfPK, vEdtSlNo)
        Call RemoveEmptVSFRows(vsfPK, 0, vEdtTableName)

        If Trim(txtName.Text) = "" Then
            MsgBox(lblName.Text & " should not be blank!!", MsgBoxStyle.Information, Me.Text)
            txtName.Focus()
            Exit Function
        End If

        If Trim(txtCond.Text) = "" Then
            MsgBox("Condition should not be blank!!", MsgBoxStyle.Information, Me.Text)
            txtCond.Focus()
            Exit Function
        End If

        If Trim(txtPendCond.Text) = "" Then
            MsgBox("Pending Condition should not be blank!!", MsgBoxStyle.Information, Me.Text)
            txtPendCond.Focus()
            Exit Function
        End If

        Dim i, j As Int32
        Dim blnFound As Boolean = False
        For i = 1 To vsfPK.Rows.Count - 1
            For j = 1 To vsfDetail.Rows.Count - 1
                If (Trim(vsfPK.GetData(i, vEdtTableName)) = (Trim(vsfDetail.GetData(j, vDetTableName)))) AndAlso (Trim(vsfPK.GetData(i, vEdtPk)) = (Trim(vsfDetail.GetData(j, vDetFieldName)))) Then
                    blnFound = True
                    'Else
                    ' blnFound = False
                End If
            Next j
        Next i

        If blnFound = False Then
            MsgBox("Please enter Report Columns..", vbInformation, Me.Text)
            Exit Function
        End If

        ValidFields = True
    End Function

    Private Sub fillDetails()
        Dim strSQL As String = ""
        Dim rs As oledb.oledbDataReader

        Try
            strSQL = "Select * from UtlPDE Where PostDocID = " & intPostDocId
            rs = clsSVT.getDataReaderFromSQL(strSQL)

            If Not rs.HasRows Then
                rs.Close()
                Exit Sub
            End If

            rs.Read()

            txtName.Text = GetVal(rs!PostDocName, "")
            txtRmks.Text = GetVal(rs!PostDocRmks, "")
            txtCond.Text = GetVal(rs!CondText, "")
            txtPendCond.Text = GetVal(rs!PendCond, "")

            rs.Close()

            strSQL = "Select * from UtlPDEDtl Where PostDocID = " & intPostDocId & " Order by Slno "
            rs = clsSVT.getDataReaderFromSQL(strSQL)
            If Not rs.HasRows Then
                rs.Close()
                Exit Sub
            End If

            With vsfDetail
                .Rows.Count = 1
                Do While rs.Read
                    .Rows.Count = .Rows.Count + 1
                    .SetData(.Rows.Count - 1, vDetSlNo, GetVal(rs!Slno, ""))
                    .SetData(.Rows.Count - 1, vDetTableName, GetVal(rs!TblName, ""))
                    .SetData(.Rows.Count - 1, vDetFieldName, GetVal(rs!FldName, ""))
                    .SetData(.Rows.Count - 1, vDetCaption, GetVal(rs!FldCaption, ""))
                    .SetData(.Rows.Count - 1, vDetDateFilter, GetVal(rs!DateFilter, ""))
                    .SetData(.Rows.Count - 1, vDetHidden, GetVal(rs!ColHide, ""))
                    .SetData(.Rows.Count - 1, vDetAllowingEditing, GetVal(rs!AllowEdit, ""))
                Loop
                rs.Close()
                '.Rows.Count = .Rows.Count + 1
            End With


            strSQL = "Select * from UtlPDEPK Where PostDocID = " & intPostDocId & " Order by PostDocID Desc"
            rs = clsSVT.getDataReaderFromSQL(strSQL)
            If Not rs.HasRows Then
                rs.Close()
                Exit Sub
            End If
            With vsfPK
                .Rows.Count = 1
                Do While rs.Read
                    .Rows.Count = .Rows.Count + 1
                    .SetData(.Rows.Count - 1, vEdtSlNo, GetVal(rs!Slno, ""))
                    .SetData(.Rows.Count - 1, vEdtTableName, GetVal(rs!TblName, ""))
                    .SetData(.Rows.Count - 1, vEdtPk, GetVal(rs!FldName, ""))
                Loop
                '.Rows.Count = .Rows.Count + 1

                Call ReArrangeSlNo(vsfDetail, vDetSlNo)
                Call ReArrangeSlNo(vsfPK, vEdtSlNo)

                rs.Close()
            End With
        Catch ex As Exception
            MsgBox(ex.Message, vbInformation, Me.Text)
        End Try
    End Sub


    Private Sub txtName_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtName.KeyPress
        If e.KeyChar = "'" Then
            e.Handled = True
        End If
    End Sub

    Private Sub VisibilityUcBtns()
        UcBtnDet.tsbInsert.Visible = True
        UcBtnDet.tsbRemove.Visible = True
        UcBtnDet.tsbCopy.Visible = True
        UcBtnDet.tsbPaste.Visible = True
        UcBtnDet.tsbMoveDown.Visible = True
        UcBtnDet.tsbMoveUp.Visible = True
        UcBtnDet.tsbExport.Visible = True
        UcBtnDet.GSeparator1.Visible = False
        UcBtnDet.GSeparator2.Visible = False
        UcBtnDet.GSeparator3.Visible = False
        UcBtnDet.GSeparator4.Visible = False

        UcBtnDet.tsbAddItem.Visible = False
        UcBtnDet.tsbAttachment.Visible = False
        UcBtnDet.tsbAmmend.Visible = False
        UcBtnDet.tsbCalTotal.Visible = False
        UcBtnDet.tsbDocExport.Visible = False
        UcBtnDet.tsbFreeze.Visible = False
        UcBtnDet.tsbMultiDoc.Visible = False
        UcBtnDet.tsbRestore.Visible = False
        UcBtnDet.tsbTemplate.Visible = False
        UcBtnDet.tsbTender.Visible = False
        UcBtnDet.tsbBOM.Visible = False
        UcBtnDet.tsbImport.Visible = False
        UcBtnDet.tsbCopyFrom.Visible = False
        UcBtnDet.tsbReplace.Visible = True
        UcBtnDet.tsbInserDoc.Visible = False
        UcBtnDet.tsbAddnl.Visible = False
        UcBtnDet.GSeparator5.Visible = False
        UcBtnDet.GSeparator6.Visible = False
        UcBtnDet.GSeparator7.Visible = False

        UcBtnPk.tsbInsert.Visible = True
        UcBtnPk.tsbRemove.Visible = True
        UcBtnPk.tsbCopy.Visible = True
        UcBtnPk.tsbPaste.Visible = True
        UcBtnPk.tsbMoveDown.Visible = True
        UcBtnPk.tsbMoveUp.Visible = True
        UcBtnPk.tsbExport.Visible = False
        UcBtnPk.GSeparator1.Visible = False
        UcBtnPk.GSeparator2.Visible = False
        UcBtnPk.GSeparator3.Visible = False
        UcBtnPk.GSeparator4.Visible = False

        UcBtnPk.tsbAddItem.Visible = False
        UcBtnPk.tsbAttachment.Visible = False
        UcBtnPk.tsbAmmend.Visible = False
        UcBtnPk.tsbCalTotal.Visible = False
        UcBtnPk.tsbDocExport.Visible = False
        UcBtnPk.tsbFreeze.Visible = False
        UcBtnPk.tsbMultiDoc.Visible = False
        UcBtnPk.tsbRestore.Visible = False
        UcBtnPk.tsbTemplate.Visible = False
        UcBtnPk.tsbTender.Visible = False
        UcBtnPk.tsbBOM.Visible = False
        UcBtnPk.tsbImport.Visible = False
        UcBtnPk.tsbCopyFrom.Visible = False
        UcBtnPk.tsbReplace.Visible = True
        UcBtnPk.tsbInserDoc.Visible = False
        UcBtnPk.tsbAddnl.Visible = False
        UcBtnPk.GSeparator5.Visible = False
        UcBtnPk.GSeparator6.Visible = False
        UcBtnPk.GSeparator7.Visible = False
    End Sub

    Private Sub UcBtnDet_btnClicked(btnValue As Integer) Handles UcBtnDet.btnClicked
        With UcBtnDet
            Select Case btnValue
                Case svtDtlBtns.svtInsert
                    Call .vsfInsert(vsfDetail, vsfDetail.Rows.Count - 1, vDetTableName)
                    Call ReArrangeSlNo(vsfDetail, vDetSlNo)
                Case svtDtlBtns.svtRemove
                    Call .vsfRemove(vsfDetail, vsfDetail.Row, vDetTableName)
                    Call ReArrangeSlNo(vsfDetail, vDetSlNo)
                Case svtDtlBtns.svtCopy
                    Call .vsfCopy(vsfDetail, vsfDetail.Row, vDetTableName)
                Case svtDtlBtns.svtPaste
                    Call .vsfPaste(vsfDetail, vsfDetail.Row, vDetTableName)
                    Call ReArrangeSlNo(vsfDetail, vDetSlNo)
                Case svtDtlBtns.svtUp
                    Call MoveOneRowUp(vsfDetail, vDetSlNo)
                Case svtDtlBtns.svtDown
                    Call MoveOneRowDown(vsfDetail, vDetSlNo)
                'Case svtDtlBtns.svtExport
                '    Call .vsfExport(vsfDetail)
                Case svtDtlBtns.svtReplace
                    Call .vsfReplace(vsfDetail, vsfDetail.Row, vsfDetail.Col, vDetTableName)
            End Select
        End With
    End Sub

    Private Sub UcBtnPk_btnClicked(btnValue As Integer) Handles UcBtnPk.btnClicked
        With UcBtnPk
            Select Case btnValue
                Case svtDtlBtns.svtInsert
                    Call .vsfInsert(vsfPK, vsfPK.Rows.Count - 1, vEdtTableName)
                    Call ReArrangeSlNo(vsfPK, vEdtSlNo)
                Case svtDtlBtns.svtRemove
                    Call .vsfRemove(vsfPK, vsfPK.Row, vEdtTableName)
                    Call ReArrangeSlNo(vsfPK, vEdtSlNo)
                Case svtDtlBtns.svtCopy
                    Call .vsfCopy(vsfPK, vsfPK.Row, vEdtTableName)
                Case svtDtlBtns.svtPaste
                    Call .vsfPaste(vsfPK, vsfPK.Row, vEdtTableName)
                    Call ReArrangeSlNo(vsfPK, vEdtSlNo)
                Case svtDtlBtns.svtUp
                    Call MoveOneRowUp(vsfPK, vEdtSlNo)
                Case svtDtlBtns.svtDown
                    Call MoveOneRowDown(vsfPK, vEdtSlNo)
                Case svtDtlBtns.svtExport
                    Call .vsfExport(vsfPK)
                Case svtDtlBtns.svtReplace
                    Call .vsfReplace(vsfPK, vsfPK.Row, vsfPK.Col, vEdtTableName)
            End Select
        End With

    End Sub

    Private Sub raiseShortCutPKEvent(ByVal e As System.Windows.Forms.KeyEventArgs)

        If blnPKSection = False Then Exit Sub
        'If Mode = "A" Or Mode = "M" Then
        'Insert Row event
        If e.Control = True And e.Shift = True And e.KeyCode = Keys.I Then
            Call UcBtnPk.vsfInsert(vsfPK, vsfPK.Row, vEdtTableName)
            ReArrangeSlNo(vsfPK, vEdtSlNo)
        End If

        'Remove Row event
        If e.Control = True And e.Shift = True And e.KeyCode = Keys.R Then
            Call UcBtnPk.vsfRemove(vsfPK, vsfPK.Row, vEdtTableName)
            ReArrangeSlNo(vsfPK, vEdtSlNo)
        End If

        'Copy  event
        If e.Control = True And e.Shift = True And e.KeyCode = Keys.C Then
            Call UcBtnPk.vsfCopy(vsfPK, vsfPK.Row, vEdtTableName)
        End If
        'Paste Event
        If e.Control = True And e.Shift = True And e.KeyCode = Keys.P Then
            Call UcBtnPk.vsfPaste(vsfPK, vsfPK.Row, vEdtTableName)
            ReArrangeSlNo(vsfPK, vEdtSlNo)
        End If

        'MoveUp Event
        If e.Control = True And e.Shift = True And e.KeyCode = Keys.U Then
            Call MoveOneRowUp(vsfPK, vEdtSlNo)
        End If

        'MoveDown Event
        If e.Control = True And e.Shift = True And e.KeyCode = Keys.V Then
            Call MoveOneRowDown(vsfPK, vEdtSlNo)
        End If

        'RePlace Rest Event
        If e.Control = True And e.Shift = True And e.KeyCode = Keys.N Then

            Call UcBtnPk.vsfReplace(vsfPK, vsfPK.Row, vsfPK.Col, vEdtTableName)
            ''------changed 
            'svtBtnCtrl.btnEdit.Visible = False
            'svtBtnCtrl.btnDelete.Visible = False
            ''-------
        End If

    End Sub

    Private Sub raiseShortCutDetEvent(ByVal e As System.Windows.Forms.KeyEventArgs)


        If blnDetSection = False Then Exit Sub
            'If Mode = "A" Or Mode = "M" Then
            'Insert Row event
            If e.Control = True And e.Shift = True And e.KeyCode = Keys.I Then
                Call UcBtnDet.vsfInsert(vsfDetail, vsfDetail.Row, vDetTableName)
                ReArrangeSlNo(vsfDetail, vDetSlNo)
            End If

            'Remove Row event
            If e.Control = True And e.Shift = True And e.KeyCode = Keys.R Then
                Call UcBtnDet.vsfRemove(vsfDetail, vsfDetail.Row, vDetTableName)
                ReArrangeSlNo(vsfDetail, vDetSlNo)
            End If

            'Copy  event
            If e.Control = True And e.Shift = True And e.KeyCode = Keys.C Then
                Call UcBtnDet.vsfCopy(vsfDetail, vsfDetail.Row, vDetTableName)
            End If
            'Paste Event
            If e.Control = True And e.Shift = True And e.KeyCode = Keys.P Then
                Call UcBtnDet.vsfPaste(vsfDetail, vsfDetail.Row, vDetTableName)
                ReArrangeSlNo(vsfDetail, vDetSlNo)
            End If

            'MoveUp Event
            If e.Control = True And e.Shift = True And e.KeyCode = Keys.U Then
                Call MoveOneRowUp(vsfDetail, vDetSlNo)
            End If

            'MoveDown Event
            If e.Control = True And e.Shift = True And e.KeyCode = Keys.V Then
                Call MoveOneRowDown(vsfDetail, vDetSlNo)
            End If

            'Exprot to Excel
            If e.Control = True And e.Shift = True And e.KeyCode = Keys.E Then
                Call UcBtnDet.vsfExport(vsfDetail)
            End If

            'RePlace Rest Event
            If e.Control = True And e.Shift = True And e.KeyCode = Keys.N Then

                Call UcBtnDet.vsfReplace(vsfDetail, vsfDetail.Row, vsfDetail.Col, vDetTableName)
                ''------changed 
                'svtBtnCtrl.btnEdit.Visible = False
                'svtBtnCtrl.btnDelete.Visible = False
                ''-------
            End If

    End Sub

End Class