Imports System.ComponentModel
Imports System.Data.oledb
Imports C1.Win.C1FlexGrid

Public Class FrmUtlDeleteLinkageUpdation

    Dim clsSVT As New svtLibrary
    Dim strSQL As String

    'For Grid vsfFlds
    Const vFldTblName = 0
    Const vFldColName = 1
    Const vFldDispName = 2
    Const vFldCaption = 3
    Const vFldTotal = 4

    'For Grid vsfDtl
    Const vDtlFldName = 0
    Const vDtlTotal = 1

    'For VsfMain Grid
    Const vMainDelTblName = 0
    Const vMainDocName = 1
    Const vMainTotal = 2

    Public Mode As String

    Private Sub FrmUtlDeleteLinkageUpdation_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'Initialize with Default columns in Grid
        Call initGridStyles(Me)

        'Initialize Form Properties with default settings
        Call fillFormProperties(Me)

        'Form initialize event
        Call initscreen()
        Call ChangeFontSize(Me)

        'For Fixed Columns getting size from tables
        Call initFormVsfDet(Me, Me.Text, vsfMain, "Main")
        Call initFormVsfDet(Me, Me.Text, vsfFlds, "Flds")
        Call initFormVsfDet(Me, Me.Text, vsfDtl, "DET")

        Call ClearFlds()
        Call FillPreview()
        Call ucBtnEnabledisable()
    End Sub

    Private Sub ucBtnEnabledisable()
        With ucBtnTools
            .btnVisible("", False, False)
            .tsbInsert.Visible = True
            .tsbExport.Visible = True
            .tsbFreeze.Visible = False
            .tsbDocExport.Visible = False
            .tsbCopyFrom.Visible = False
            .tsbCopy.Visible = True
            .tsbPaste.Visible = True
            .tsbCalTotal.Visible = False
            .tsbMultiDoc.Visible = False
            .tsbBOM.Visible = False
            .tsbInserDoc.Visible = False
            .tsbImport.Visible = False
            .tsbTemplate.Visible = False
            .tsbAmmend.Visible = False
            .tsbTender.Visible = False
            .tsbMoveUp.Visible = False
            .tsbMoveDown.Visible = False
            .tsbRemove.Visible = True
        End With
    End Sub

    Private Sub ClearFlds()
        vsfDtl.Rows.Count = 1
        vsfFlds.Rows.Count = 1
        vsfDtl.Rows.Count = 2

        cboDelTblName.SelectedIndex = -1
        txtDocName.Text = ""

        Mode = "V"
        EnableDisable(False)
        ChangeCtrlBtn()
    End Sub

    Private Sub EnableDisable(ByVal blnFlag As Boolean)
        cboDelTblName.Enabled = blnFlag
        txtDocName.Enabled = blnFlag
        vsfDtl.Enabled = blnFlag
        vsfFlds.Enabled = blnFlag
        btnSearch.Enabled = blnFlag
        btnRemove.Enabled = blnFlag
    End Sub

    Private Sub ChangeCtrlBtn()
        btnAdd.Visible = False
        btnEdit.Visible = False
        btnDel.Visible = False
        btnSave.Visible = False
        btnCancel.Visible = False

        Select Case Mode
            Case "V"
                btnAdd.Visible = True
                btnEdit.Visible = True
                btnDel.Visible = True
            Case "A", "M"
                btnSave.Visible = True
                btnCancel.Visible = True
            Case "M"
        End Select
    End Sub

    Private Sub initscreen()
        fraUtlDeletion.Visible = True
        cboDelTblName.Enabled = False

        With vsfMain
            .Rows.Count = 1
            .Cols.Count = vMainTotal
            .SetData(0, vMainDocName, "Document Name")
            Call SetFlexColor(vsfMain, "", vMainDelTblName, vMainDocName)
            Call SetHideCol(vsfMain, True, vMainDelTblName)
        End With

        With vsfFlds
            .Rows.Count = 1
            .Cols.Count = vFldTotal
            .SetData(0, vFldTblName, "Table")
            .SetData(0, vFldColName, "Column")
            .SetData(0, vFldDispName, "Display Column")
            .SetData(0, vFldCaption, "Doc Name")
            Call SetFlexColor(vsfFlds, "M", vFldTblName, vFldColName)
        End With

        With vsfDtl
            .Rows.Count = 1
            .Cols.Count = vDtlTotal
            .SetData(0, vDtlFldName, "Column")
            Call SetFlexColor(vsfDtl, "M", vDtlFldName)
        End With
    End Sub

    Private Sub FillPreview()
        Dim rsDummy As oledb.oledbDataReader
        Try
            strSQL = "select DelTblName,case DelDocName When '' then DelTblName else DelDocName end DocName from utldeletion Group by DelTblName, DelDocName"
            rsDummy = clsSVT.getDataReaderFromSQL(strSQL)
            vsfMain.Rows.Count = 1
            Using rsDummy
                With vsfMain
                    If Not rsDummy.HasRows Then
                        rsDummy.Close()
                        Exit Sub
                    End If
                    Do While rsDummy.Read
                        .Rows.Count = .Rows.Count + 1
                        .SetData(.Rows.Count - 1, vMainDelTblName, GetVal(rsDummy.Item("DelTblName"), ""))
                        .SetData(.Rows.Count - 1, vMainDocName, GetVal(rsDummy.Item("DocName"), ""))
                    Loop
                End With
            End Using
            Call bindCombo(cboDelTblName, "Object_id", "Name", "Select Object_Id,Name From sys.tables where LEFT(name,3) in ('Mst','Trn','Utl','Rpt') order by Name")
            cboDelTblName.SelectedIndex = -1
        Catch ex As Exception
            MsgBox("Data Not found !!! ", vbCritical, Me.Text & "[fillDelFldsMain]")
            Exit Sub
        End Try
    End Sub

    Private Sub LoadDelTable(ByVal argDelTblName As String)
        If argDelTblName = "" Then Exit Sub
        Dim rsDummy As oledb.oledbDataReader
        strSQL = "select * from UtlDeletion  where DelTblName = '" & argDelTblName & "' order by DelTblName "
        rsDummy = clsSVT.getDataReaderFromSQL(strSQL)
        If Not rsDummy.HasRows Then
            rsDummy.Close()
            Exit Sub
        End If

        Dim blnMain As Boolean = False
        vsfFlds.Rows.Count = 1
        vsfDtl.Rows.Count = 1
        vsfDtl.Rows.Count = 2
        Using rsDummy
            With vsfFlds
                Do While rsDummy.Read()
                    If GetVal(rsDummy.Item("DelDocName"), "") <> "" Then txtDocName.Text = GetVal(rsDummy.Item("DelDocName"), "")
                    If GetVal(rsDummy.Item("DelTblName"), "") <> "" Then cboDelTblName.Text = GetVal(rsDummy.Item("DelTblName"), "")
                    If GetVal(rsDummy.Item("DelDocFld"), "") <> "" Then vsfDtl.SetData(1, vDtlFldName, GetVal(rsDummy.Item("DelDocFld"), ""))
                    If Not blnMain Then
                        'txtDocName.Text = GetVal(rsDummy.Item("DelDocName"), "")
                        'cboDelTblName.Text = GetVal(rsDummy.Item("DelTblName"), "")
                        'vsfDtl.SetData(1, vDtlFldName, GetVal(rsDummy.Item("DelDocFld"), ""))
                        blnMain = True
                    End If

                    .Rows.Count = .Rows.Count + 1
                    .SetData(.Rows.Count - 1, vFldTblName, GetVal(rsDummy.Item("TblName"), ""))
                    .SetData(.Rows.Count - 1, vFldColName, GetVal(rsDummy.Item("FldName"), ""))
                    .SetData(.Rows.Count - 1, vFldDispName, GetVal(rsDummy.Item("DispFldName"), ""))
                    .SetData(.Rows.Count - 1, vFldCaption, GetVal(rsDummy.Item("TblCaption"), ""))
                Loop
            End With
        End Using
        vsfFlds.Rows.Count = vsfFlds.Rows.Count + 1

    End Sub

    Private Sub FrmUtlDeleteLinkageUpdation_KeyUp(sender As Object, e As KeyEventArgs) Handles Me.KeyUp
        If e.KeyCode = Keys.F9 And e.Control = True Then
            Call initFormSize(Me, Me.Text, True, 594, 505, 0, 0)
            Call initFormVsfDet(Me, Me.Text, vsfMain, "Main")
            Call initFormVsfDet(Me, Me.Text, vsfFlds, "Flds")
            Call initFormVsfDet(Me, Me.Text, vsfDtl, "DET")
            MsgBox("Restored Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
        If e.KeyCode = Keys.F8 And e.Control = True Then
            Call SaveActFormSettings(Me, Me.Text)
            Call SaveActFormVsfDet(Me, Me.Text, "Main", vsfMain)
            Call SaveActFormVsfDet(Me, Me.Text, "Flds", vsfFlds)
            Call SaveActFormVsfDet(Me, Me.Text, "DET", vsfDtl)
            MsgBox("Saved Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
        Call raiseShortCutDetEvent(e)
    End Sub

    Private Sub raiseShortCutDetEvent(ByVal e As System.Windows.Forms.KeyEventArgs)
        'Remove Row event
        If e.Control = True And e.Shift = True And e.KeyCode = Keys.R Then
            'If Not vsfFlds.Rows.Count > 1 Then
            '    MsgBox("No data to Remove", vbExclamation)
            'End If
            'vsfFlds.SetData(vsfFlds.Row, -1, "")
            With vsfFlds
                If .Rows.Count <= 1 Then Exit Sub
                If .Row < 1 Then Exit Sub
                If cboDelTblName.Text = "" Then Exit Sub
                If MsgBox("Are you sure you want to Remove ?", MsgBoxStyle.YesNo + vbDefaultButton2 + MsgBoxStyle.Critical, "Deletion Confirmation...") = vbNo Then Exit Sub
                vsfFlds.Rows.Remove(vsfFlds.Row)
                If vsfFlds.Rows.Count = 1 Then vsfFlds.Rows.Count = 2
            End With
        End If

        If e.Control = True And e.Shift = True And e.KeyCode = Keys.I Then
            Call ucBtnTools.vsfInsert(vsfFlds, vsfFlds.Row, vFldTblName)
            'ReArrangeSlNo(vsfFlds, vDetSlNo)
        End If

        'Copy  event
        If e.Control = True And e.Shift = True And e.KeyCode = Keys.C Then
            Call ucBtnTools.vsfCopy(vsfFlds, vsfFlds.Row, vFldTblName)
        End If

        'Paste Event
        If e.Control = True And e.Shift = True And e.KeyCode = Keys.P Then
            Call ucBtnTools.vsfPaste(vsfFlds, vsfFlds.Row, vFldTblName)
            'ReArrangeSlNo(vsfFlds, vDetSlNo)
        End If

        'Exprot to Excel
        If e.Control = True And e.Shift = True And e.KeyCode = Keys.E Then
            Call ucBtnTools.vsfExport(vsfFlds)
        End If

    End Sub

    Private Sub vsfDelTblMain_DoubleClick(sender As Object, e As EventArgs) Handles vsfMain.DoubleClick
        If vsfMain.Row <= 0 Then Exit Sub
        Call LoadDelTable(GetVal(vsfMain.GetData(vsfMain.Row, vMainDelTblName), ""))
    End Sub

    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        Mode = "A"
        Call ChangeCtrlBtn()
        Call EnableDisable(True)
    End Sub

    Private Sub cboDelTblName_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cboDelTblName.KeyPress
        If e.KeyChar = "'" Then
            e.Handled = True
        End If
    End Sub

    Private Sub vsfMain_BeforeEdit(sender As Object, e As RowColEventArgs) Handles vsfMain.BeforeEdit
        e.Cancel = True
    End Sub

    Private Sub vsfFlds_BeforeEdit(sender As Object, e As RowColEventArgs) Handles vsfFlds.BeforeEdit
        If Not (Mode = "A" Or Mode = "M") Then
            e.Cancel = True
            Exit Sub
        End If

        vsfFlds.ComboList = ""
        If cboDelTblName.Text = String.Empty Then
            e.Cancel = True
            Exit Sub
        End If

        If vsfFlds.Rows.Count <= 1 Then
            e.Cancel = True
            Exit Sub
        End If

        If e.Col = vFldTblName Then
            vsfFlds.ComboList = String.Empty
            vsfFlds.ComboList = GenVSfComboStringCond("Name", "Sys.tables", " Order By Name", " where LEFT(name,3) in ('Mst','Trn','Utl','Rpt')")
            Exit Sub
        End If

        If e.Col = vFldColName Or e.Col = vFldDispName Then

            vsfFlds.ComboList = String.Empty
            vsfFlds.ComboList = GenVSfComboStringCond("COLUMN_NAME", "Information_Schema.Columns", "",
                                                     " Where TABLE_NAME='" & GetVal(vsfFlds.GetData(vsfFlds.Row, vFldTblName), "") & "'")
            Exit Sub
        End If
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        If cboDelTblName.Text = String.Empty Then
            MsgBox("Select Table name", MsgBoxStyle.Critical, Me.Text)
            Exit Sub
        End If
        If GetVal(vsfDtl.GetData(vsfDtl.Row, vDtlFldName)) = String.Empty Then
            MsgBox("Select Field to be Checked for Deletion", MsgBoxStyle.Critical, Me.Text)
            Exit Sub
        End If
        Call LoadFormList()
    End Sub

    Private Function LoadFormList() As Boolean
        Dim strSQL As String
        Dim rsDummy As oledbDataReader
        Try
            LoadFormList = False
            strSQL = "Select TABLE_NAME,COLUMN_NAME From Information_Schema.Columns"
            strSQL = strSQL & "  Where COLUMN_NAME= '" & GetVal(vsfDtl.GetData(vsfDtl.Row, vDtlFldName), "") & "'"
            strSQL = strSQL & " AND TABLE_NAME <> '" & Trim(cboDelTblName.Text) & "'"
            strSQL = strSQL & " And Table_Name Not Like '%vw%' Order By TABLE_NAME"
            rsDummy = clsSVT.getDataReaderFromSQL(strSQL)
            Using rsDummy
                If Not rsDummy.HasRows Then
                    rsDummy.Close()
                    vsfFlds.Rows.Count = 1
                    Exit Function
                End If

                With vsfFlds
                    .Rows.Count = 1
                    Do While rsDummy.Read
                        .Rows.Count = .Rows.Count + 1
                        .SetData(.Rows.Count - 1, vFldTblName, GetVal(rsDummy.Item("TABLE_NAME"), ""))
                        .SetData(.Rows.Count - 1, vFldColName, GetVal(rsDummy.Item("COLUMN_NAME"), ""))
                    Loop
                End With
            End Using
            Call getDocNames()
            LoadFormList = True

            Exit Function
        Catch ex As Exception
            MsgBox("Error during Load" & vbCrLf & ex.Message, MsgBoxStyle.Critical, Me.Text & " [LoadFormList]")
            LoadFormList = False
        End Try
    End Function

    'Private Sub btnRemv_Click(sender As Object, e As EventArgs) Handles btnRemove.Click
    '    With vsfFlds
    '        If .Rows.Count <= 1 Then Exit Sub
    '        If .Row < 1 Then Exit Sub
    '        If cboDelTblName.Text = "" Then Exit Sub
    '        If MsgBox("Are you sure you want to Remove ?", MsgBoxStyle.YesNo + vbDefaultButton2 + MsgBoxStyle.Critical, "Deletion Confirmation...") = vbNo Then Exit Sub
    '        vsfFlds.Rows.Remove(vsfFlds.Row)
    '        If vsfFlds.Rows.Count = 1 Then vsfFlds.Rows.Count = 2
    '    End With
    'End Sub

    Private Sub btnCncl_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Call ClearFlds()
    End Sub

    Private Sub btnSav_Click(sender As Object, e As EventArgs) Handles btnSave.Click

        If Trim(cboDelTblName.Text) = String.Empty Then
            MsgBox("Table name is Empty...", MsgBoxStyle.Critical, Me.Text)
            cboDelTblName.Focus()
            Exit Sub
        End If

        If Trim(txtDocName.Text) = String.Empty Then
            MsgBox("Document name is Empty...", MsgBoxStyle.Critical, Me.Text)
            txtDocName.Focus()
            Exit Sub
        End If

        If vsfFlds.Rows.Count = 1 Then
            MsgBox("Data is not available", MsgBoxStyle.Critical, Me.Text)
            vsfFlds.Rows.Count = 2
            Exit Sub
        End If

        If GetVal(vsfDtl.GetData(vsfDtl.Row, vDtlFldName), "") = String.Empty Then
            MsgBox("Column is Empty....", MsgBoxStyle.Critical, Me.Text)
            vsfDtl.Col = vDtlFldName
            vsfDtl.Row = 1
            vsfDtl.Focus()
            Exit Sub
        End If

        Call RemoveEmptVSFRows(vsfFlds, 0, vFldTblName, vFldColName)

        Dim StrSQL As String
        Dim svtTrn As oledb.oledbTransaction = Nothing
        Try
            'For Delete  Columns from ultOptions
            svtTrn = svtConn.BeginTransaction(IsolationLevel.ReadCommitted)
            StrSQL = "Delete from utlDeletion where DelTblName='" & GetVal(cboDelTblName.Text, "") & "'"
            clsSVT.execSQLStatement(StrSQL, svtTrn)


            With vsfFlds
                StrSQL = ""
                Dim blnFound As Boolean = False
                StrSQL = StrSQL & "Insert into utlDeletion (DelTblName, TblName, FldName, DispFldName, TblCaption, DelDocName, DelDocFld) values ("
                For i = 1 To .Rows.Count - 1
                    If GetVal(.GetData(i, vFldTblName), "") <> "" AndAlso GetVal(.GetData(i, vFldColName), "") <> "" Then
                        StrSQL = StrSQL & "'" & GetVal(cboDelTblName.Text, "") & "'"
                        StrSQL = StrSQL & ",'" & GetVal(.GetData(i, vFldTblName), "") & "'"
                        StrSQL = StrSQL & ",'" & GetVal(.GetData(i, vFldColName), "") & "'"
                        StrSQL = StrSQL & ",'" & GetVal(.GetData(i, vFldDispName), "") & "'"
                        StrSQL = StrSQL & ",'" & GetVal(.GetData(i, vFldCaption), "") & "'"
                        StrSQL = StrSQL & ",'" & GetVal(Trim(txtDocName.Text), "") & "'"
                        StrSQL = StrSQL & ",'" & GetVal(vsfDtl.GetData(1, vDtlFldName), "") & "'"
                        StrSQL = StrSQL & "),("
                        blnFound = True
                    End If
                Next

                If blnFound Then
                    StrSQL = Mid(StrSQL, 1, Len(StrSQL) - 2)

                    If Not clsSVT.execSQLStatement(StrSQL, svtTrn) Then
                        svtTrn.Rollback()
                        Exit Sub
                    End If
                End If
            End With
            svtTrn.Commit()
            MsgBox("Saved Successfully!", vbInformation, Me.Text)

            Dim strDelTblName As String = cboDelTblName.Text
            Call ClearFlds()
            Call FillPreview()
            Call LoadDelTable(strDelTblName)

        Catch ex As Exception
            MsgBox("Error during save" & vbCrLf & ex.Message, MsgBoxStyle.Critical, Me.Text & " [BtnSave]")
            svtTrn.Rollback()
            Exit Sub
        End Try

    End Sub

    Private Sub btnDlt_Click(sender As Object, e As EventArgs) Handles btnDel.Click
        If Trim(cboDelTblName.Text) = "" Then Exit Sub
        If MsgBox("Are you sure you want to delete ?", MsgBoxStyle.YesNo + vbDefaultButton2 + MsgBoxStyle.Critical, "Deletion Confirmation...") = vbNo Then Exit Sub
        strSQL = "Delete from utlDeletion where DelTblName = '" & GetVal(Trim(cboDelTblName.Text), "") & "'"
        clsSVT.execSQLStatement(strSQL)
        Call ClearFlds()
        MsgBox("Deleted Successfully!", vbInformation, Me.Text)
    End Sub

    Private Sub btnEdt_Click(sender As Object, e As EventArgs) Handles btnEdit.Click
        If cboDelTblName.Text = "" Then Exit Sub
        EnableDisable(True)
        Mode = "M"
        Call ChangeCtrlBtn()
    End Sub

    Private Sub vsfDtl_BeforeEdit(sender As Object, e As RowColEventArgs) Handles vsfDtl.BeforeEdit
        vsfDtl.ComboList = ""
        If Trim(cboDelTblName.Text) = "" Then
            e.Cancel = True
        Else
            vsfDtl.ComboList = GenVSfComboStringCond("Column_Name", "Information_schema.Columns", " where table_name= '" & GetVal(Trim(cboDelTblName.Text)) & "'")
            Exit Sub
        End If
    End Sub

    Private Sub getDocNames()
        With vsfFlds
            Dim i As Integer
            For i = 1 To .Rows.Count - 1
                .SetData(i, vFldCaption, clsSVT.getDataStringValue("select DocName from utlTransactions where HdrTblName='" & vsfFlds.GetData(i, vFldTblName) & "' or DtlTblName='" & vsfFlds.GetData(i, vFldTblName) & "'", "", False))
                .SetData(i, vFldDispName, clsSVT.getDataStringValue("select DocShForm + 'Code' from utlTransactions where HdrTblName='" & Trim(cboDelTblName.Text) & "' or DtlTblName='" & Trim(cboDelTblName.Text) & "'", vsfDtl.GetData(vsfDtl.Row, vDtlFldName), False))
            Next
        End With
        For i = 1 To vsfDtl.Rows.Count - 1
            Dim blnFound As Boolean
            clsSVT.getDataStringValue("Select Column_Name from Information_schema.Columns where table_name='" & Trim(cboDelTblName.Text) & "' and column_Name ='" & vsfDtl.GetData(i, vDtlFldName) & "'", "", blnFound)
            If Not blnFound Then
                vsfDtl.SetData(i, vDtlFldName, "")
            End If
        Next
    End Sub

    Private Sub vsfFlds_KeyPress(sender As Object, e As KeyPressEventArgs) Handles vsfFlds.KeyPress
        If Mode = "M" Or Mode = "A" Then
            Select Case vsfFlds.Col
                Case vFldCaption
                    If e.KeyChar = ChrW(Keys.Enter) Then
                        e.Handled = False
                    Else
                        e.Handled = True
                    End If

                    Exit Sub
            End Select
        Else
            e.Handled = True
        End If

    End Sub

    Private Sub vsfFlds_KeyUp(sender As Object, e As KeyEventArgs) Handles vsfFlds.KeyUp
        If e.KeyCode = Keys.Tab AndAlso vsfFlds.Row = vsfFlds.Rows.Count - 1 AndAlso
                GetVal(vsfFlds.GetData(vsfFlds.Row, vFldTblName), "") <> "" AndAlso
                GetVal(vsfFlds.GetData(vsfFlds.Row, vFldColName), "") <> "" AndAlso
                vsfFlds.Col > vFldColName Then
            vsfFlds.Rows.Count = vsfFlds.Rows.Count + 1
        End If
    End Sub

    Private Sub ucBtnTools_btnClicked(btnValue As Integer) Handles ucBtnTools.btnClicked
        Try
            With ucBtnTools
                Select Case btnValue
                    Case svtDtlBtns.svtInsert
                        Call .vsfInsert(vsfFlds, vsfFlds.Row, vFldTblName)

                    Case svtDtlBtns.svtRemove
                        If Not vsfFlds.Rows.Count > 1 Then
                            MsgBox("No data to Remove", vbExclamation)
                        End If
                        Call .vsfRemove(vsfFlds, vsfFlds.Row, vFldTblName)

                    Case svtDtlBtns.svtCopy
                        Call .vsfCopy(vsfFlds, vsfFlds.Row, vFldTblName)

                    Case svtDtlBtns.svtPaste
                        Call .vsfPaste(vsfFlds, vsfFlds.Row, vFldTblName)
                        'vsfFlds.SetData(vsfFlds.Row, vFldTblName, "")

                    Case svtDtlBtns.svtExport
                        Call .vsfExport(vsfFlds)

                End Select
            End With
        Catch ex As Exception
        End Try
    End Sub

    Private Function getmaxNo() As Integer
        Dim i As Integer, intNo As Integer
        With vsfFlds
            intNo = 0
            For i = 1 To .Rows.Count - 1
                If Val(.GetData(i, vFldTblName)) > intNo Then
                    intNo = Val(.GetData(i, vFldTblName))
                End If
            Next i
        End With
        intNo = intNo + 1
        getmaxNo = intNo
    End Function
End Class