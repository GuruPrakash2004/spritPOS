Imports System.ComponentModel
Imports C1.Win.C1FlexGrid

Public Class frmRptPMDesign

    Public intDocID As Int32 = 1
    Dim clsSVT As New svtLibrary
    Dim svtTrn As oledb.oledbTransaction
    Dim intSlno As Int32

    Const vDetSlno = 0
    Const vDetType = 1
    Const vDetDocNo = 2
    Const vDetDocDate = 3
    Const vDetParty = 4
    Const vDetItem = 5
    Const vDetItemName = 6
    Const vDetItemGroup = 7
    Const vDetUOM = 8
    Const vDetAdd1 = 9
    Const vDetAdd2 = 10
    Const vDetAdd3 = 11
    Const vDetAdd4 = 12
    Const vDetAdd5 = 13
    Const vDetSQty = 14
    Const vDetSRate = 15
    Const vDetSVal = 16
    Const vDetPRate = 17
    Const vDetPVal = 18
    Const vDetDiff = 19
    Const vDetTotal = 20


    Const vID = 0
    Const vDocNo = 1
    Const vDocDate = 2
    Const vParty = 3
    Const vItem = 4
    Const vItemName = 5
    Const vItemGroup = 6
    Const vUOM = 7
    Const vAdd1 = 8
    Const vAdd2 = 9
    Const vAdd3 = 10
    Const vAdd4 = 11
    Const vAdd5 = 12
    Const vSQty = 13
    Const vSRate = 14
    Const vSVal = 15
    Const vPRate = 16
    Const vPVal = 17
    Const vDiff = 18
    Const vTotal = 19

    Const vName = 0
    Const vCaption = 1
    Const vTbl = 2
    Const vFld = 3
    Const vColTotal = 4

    Private Sub frmRptQRDesign_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If intDocID = 0 Then
            ucBtnTools.Enabled = False
        Else
            ucBtnTools.Enabled = True
        End If


        initGridStyles(Me)
        Me.KeyPreview = True
        Call initScreen()
        Call fillFormProperties(Me)
        Call initFormVsfDet(Me, Me.Text, vsfDet, "DET")
        Call initFormVsfDet(Me, Me.Text, vsfDoc, "INFO")
        Call ChangeFontSize(Me)
        Call DisableAdnlbtn()


        cboDocType.SelectedIndex = -1
        cboRateType.SelectedIndex = -1

        If intDocID > 0 Then FillPMDtls()
        'ReArrangeSlNo(vsfDet, vDetSlno)

        vsfDoc.Rows(vDiff).Visible = False

        If cboDocType.Text = "" Then
            vsfDoc.Visible = False
        End If
    End Sub

    Private Sub DisableAdnlbtn()
        With ucBtnTools
            .tsbInsert.Visible = True
            .tsbCopy.Visible = True
            .tsbPaste.Visible = True
            .tsbMoveUp.Visible = True
            .tsbMoveDown.Visible = True
            .tsbRemove.Visible = True
            .tsbExport.Visible = True


            .tsbAddItem.Visible = False
            .tsbReplace.Visible = False
            .tsbAttachment.Visible = False
            .tsbRestore.Visible = False
            .tsbFreeze.Visible = False
            .tsbAddnl.Visible = False
            .tsbDocExport.Visible = False
            .tsbImport.Visible = False
            .tsbTemplate.Visible = False
            .tsbCopyFrom.Visible = False
            .tsbAmmend.Visible = False
            .tsbTender.Visible = False
            .tsbInserDoc.Visible = False
            .tsbMultiDoc.Visible = False
            .tsbBOM.Visible = False
            .tsbCalTotal.Visible = False
            .GSeparator6.Visible = False
            .GSeparator7.Visible = False
        End With
    End Sub

    Private Sub FillPMDtls()
        Call fillHdrDtls()
        Call fillDetDtls()
        'ReArrangeSlNo(vsfDet, vDetSlno)
    End Sub

    Private Sub fillHdrDtls()
        Dim strSQL As String = ""
        Dim rs As oledb.oledbDataReader

        strSQL = "Select * "
        strSQL &= " From UtlProfitMargin "
        strSQL &= " where DocID = " & intDocID
        Try
            rs = clsSVT.getDataReaderFromSQL(strSQL)

            If Not rs.HasRows Then
                rs.Close()
                Exit Sub
            End If

            Do While rs.Read
                txtDocName.Text = GetVal(rs("DocName"), "")
                cboRateType.Text = GetVal(rs("RateType"), "")
                txtDateFld.Text = ""
                txtCondText.Text = GetVal(rs("CondText"), "")
            Loop
            rs.Close()
        Catch ex As Exception
            MsgBox(ex.Message & " [fillHdrDtls]", vbInformation, Me.Text)
        End Try
    End Sub

    Private Sub fillDetDtls()
        Dim strSQL As String = ""
        Dim rs As oledb.oledbDataReader

        strSQL = "Select * "
        strSQL &= " From UtlProfitDtlFlds "
        strSQL &= " where DocID = " & intDocID
        strSQL &= " Order by Slno "

        Try
            rs = clsSVT.getDataReaderFromSQL(strSQL)

            vsfDet.Rows.Count = 1

            If Not rs.HasRows Then
                rs.Close()
                Exit Sub
            End If

            With vsfDet
                Do While rs.Read
                    .Rows.Count = .Rows.Count + 1
                    .SetData(.Rows.Count - 1, vDetSlno, GetVal(rs!Slno, ""))
                    .SetData(.Rows.Count - 1, vDetType, GetVal(rs!DocType, ""))
                    .SetData(.Rows.Count - 1, vDetDocNo, GetVal(rs!FldNoTbl, "") & "." & GetVal(rs!FldNo, ""))
                    .SetData(.Rows.Count - 1, vDetDocDate, GetVal(rs!FldDateTbl, "") & "." & GetVal(rs!FldDate, ""))
                    .SetData(.Rows.Count - 1, vDetParty, GetVal(rs!FldPartyTbl, "") & "." & GetVal(rs!FldParty, ""))
                    .SetData(.Rows.Count - 1, vDetItem, GetVal(rs!FldItemTbl, "") & "." & GetVal(rs!FldItem, ""))
                    .SetData(.Rows.Count - 1, vDetItemName, GetVal(rs!FldItemDescTbl, "") & "." & GetVal(rs!FldItemDesc, ""))
                    .SetData(.Rows.Count - 1, vDetItemGroup, GetVal(rs!FldItemGroupTbl, "") & "." & GetVal(rs!FldItemGroup, ""))
                    .SetData(.Rows.Count - 1, vDetUOM, GetVal(rs!FldUOMTbl, "") & "." & GetVal(rs!FldUOM, ""))
                    .SetData(.Rows.Count - 1, vDetAdd1, GetVal(rs!FldAD1Tbl, "") & "." & GetVal(rs!FldAD1, ""))
                    .SetData(.Rows.Count - 1, vDetAdd2, GetVal(rs!FldAD2Tbl, "") & "." & GetVal(rs!FldAD2, ""))
                    .SetData(.Rows.Count - 1, vDetAdd3, GetVal(rs!FldAD3Tbl, "") & "." & GetVal(rs!FldAD3, ""))
                    .SetData(.Rows.Count - 1, vDetAdd4, GetVal(rs!FldAD4Tbl, "") & "." & GetVal(rs!FldAD4, ""))
                    .SetData(.Rows.Count - 1, vDetAdd5, GetVal(rs!FldAD5Tbl, "") & "." & GetVal(rs!FldAD5, ""))
                    .SetData(.Rows.Count - 1, vDetSQty, GetVal(rs!FldSQtyTbl, "") & "." & GetVal(rs!FldSQty, ""))
                    .SetData(.Rows.Count - 1, vDetSRate, GetVal(rs!FldSRateTbl, "") & "." & GetVal(rs!FldSRate, ""))
                    .SetData(.Rows.Count - 1, vDetSVal, GetVal(rs!FldSValueTbl, "") & "." & GetVal(rs!FldSValue, ""))
                    .SetData(.Rows.Count - 1, vDetPRate, GetVal(rs!FldPRTbl, "") & "." & GetVal(rs!FldPR, ""))
                    .SetData(.Rows.Count - 1, vDetPVal, GetVal(rs!FldPRVTbl, "") & "." & GetVal(rs!FldPRV, ""))
                    .SetData(.Rows.Count - 1, vDetDiff, GetVal(rs!FldDiffTbl, "") & "." & GetVal(rs!FldDiff, ""))
                Loop
                rs.Close()
            End With

        Catch ex As Exception
            MsgBox(ex.Message & " [fillDetDtls]", vbInformation, Me.Text)
        End Try
    End Sub
    Private Sub initScreen()
        With vsfDet
            .Rows.Count = 1
            .Cols.Count = vDetTotal

            .SetData(0, vDetSlno, "Slno")
            .SetData(0, vDetType, "Type")
            .SetData(0, vDetDocNo, "Doc No")
            .SetData(0, vDetDocDate, "Doc Date")
            .SetData(0, vDetParty, "Party")
            .SetData(0, vDetItem, "Item Code")
            .SetData(0, vDetItemName, "Item Name")
            .SetData(0, vDetItemGroup, "Item Group")
            .SetData(0, vDetUOM, "UOM")
            .SetData(0, vDetAdd1, "Addnl Fld-1")
            .SetData(0, vDetAdd2, "Addnl Fld-2")
            .SetData(0, vDetAdd3, "Addnl Fld-3")
            .SetData(0, vDetAdd4, "Addnl Fld-4")
            .SetData(0, vDetAdd5, "Addnl Fld-5")
            .SetData(0, vDetSQty, "Sales Qty")
            .SetData(0, vDetSRate, "Sales Rate")
            .SetData(0, vDetSVal, "Sales Value")
            .SetData(0, vDetPRate, "Purchase Rate")
            .SetData(0, vDetPVal, "Purchase Value")
            .SetData(0, vDetDiff, "Difference")

            Call SetFlexColor(vsfDet, "", vDetSlno, vDetType, vDetDocNo, vDetDocDate, vDetParty, vDetItem, vDetItemName, vDetItemGroup, vDetUOM, vDetAdd1, vDetAdd2, vDetAdd3, vDetAdd4, vDetAdd5, vDetSQty, vDetSRate, vDetSVal, vDetPRate, vDetPVal, vDetDiff)
        End With


        With vsfDoc
            .Cols.Fixed = 1
            .Rows.Fixed = 1
            .Rows.Count = vTotal
            .Cols.Count = vColTotal

            .SetData(vID, vName, "")
            .SetData(vID, vCaption, "Caption")
            .SetData(vID, vTbl, "Table")
            .SetData(vID, vFld, "Field")

            .SetData(vDocNo, vName, "Doc No")
            .SetData(vDocDate, vName, "Doc Date")
            .SetData(vParty, vName, "Party")
            .SetData(vItem, vName, "Item Code")
            .SetData(vItemName, vName, "Item Name")
            .SetData(vItemGroup, vName, "Item Group")
            .SetData(vUOM, vName, "UOM")
            .SetData(vAdd1, vName, "Addnl Fld-1")
            .SetData(vAdd2, vName, "Addnl Fld-2")
            .SetData(vAdd3, vName, "Addnl Fld-3")
            .SetData(vAdd4, vName, "Addnl Fld-4")
            .SetData(vAdd5, vName, "Addnl Fld-5")
            .SetData(vSQty, vName, "Sales Qty")
            .SetData(vSRate, vName, "Sales Rate")
            .SetData(vSVal, vName, "Sales Value")
            .SetData(vPRate, vName, "Purchase Rate")
            .SetData(vPVal, vName, "Purchase Value")
            .SetData(vDiff, vName, "Difference")
        End With
        'ReArrangeSlNo(vsfDet, vDetSlno)
    End Sub

    Private Sub vsfDet_BeforeEdit(sender As Object, e As RowColEventArgs) Handles vsfDet.BeforeEdit
        e.Cancel = True
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        txtDocName.Text = ""
        txtCondText.Text = ""
        vsfDet.Rows.Count = 1
        vsfDet.Rows.Count = 2
        cboDocType.SelectedIndex = -1
        cboRateType.SelectedIndex = -1
    End Sub

    Private Function ValidFlds()
        ValidFlds = False

        If Trim(txtDocName.Text) = String.Empty Then
            MsgBox("Name cannot be Empty..", vbInformation, Me.Text)
            txtDocName.Focus()
            Exit Function
        End If

        If Trim(cboRateType.Text) = String.Empty Then
            MsgBox("Rate Type cannot be Empty..", vbInformation, Me.Text)
            cboRateType.Focus()
            Exit Function
        End If

        'If Trim(txtDateFld.Text) = String.Empty Then
        '    MsgBox("Date Field cannot be Empty..", vbInformation, Me.Text)
        '    txtDateFld.Focus()
        '    Exit Function
        'End If

        ValidFlds = True
    End Function

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        If ValidFlds() Then
            Dim Mode As String = "M"
            If intDocID <= 0 Then Mode = "A"

            Dim strSQL As String = ""

            If intDocID = 0 Then
                strSQL = "Insert into UtlProfitMargin (DocName, RateType, CondText) Values ("
                strSQL &= "'" & Trim(txtDocName.Text) & "',"
                strSQL &= "'" & Trim(cboRateType.Text) & "',"
                strSQL &= "'" & Trim(txtCondText.Text) & "')"

            Else
                strSQL = "Update UtlProfitMargin Set "
                strSQL &= " DocName='" & Trim(txtDocName.Text) & "',"
                strSQL &= " RateType='" & Trim(cboRateType.Text) & "',"
                strSQL &= " CondText='" & Trim(txtCondText.Text) & "'"
                strSQL &= " Where DocID=" & intDocID

            End If
            svtTrn = svtConn.BeginTransaction(IsolationLevel.ReadCommitted)

            Try
                If Not clsSVT.execSQLStatement(strSQL, svtTrn) Then
                    svtTrn.Rollback()
                    Exit Sub
                End If

                If intDocID = 0 Then intDocID = clsSVT.getDataLongValue("Select Max(DocID) From UtlProfitMargin", 0, False, svtTrn)
                svtTrn.Commit()

                If Mode = "A" Then
                    MsgBox("Document added Successfully...", MsgBoxStyle.Information, Me.Text)
                Else
                    MsgBox("Document Updated Successfully...", MsgBoxStyle.Information, Me.Text)
                End If
                'ReArrangeSlNo(vsfDet, vDetSlno)

                ucBtnTools.Enabled = True
                txtDocName.Enabled = False
                cboRateType.Enabled = False
                txtDateFld.Enabled = False
                txtCondText.Enabled = False
                Call clearFlds()
            Catch ex As Exception
                svtTrn.Rollback()
            End Try
        End If
    End Sub

    Public Sub clearFlds()
        txtDocName.Text = ""
        cboRateType.Text = ""
        txtDateFld.Text = ""
        txtCondText.Text = ""
    End Sub
    Private Sub ucBtnTools_btnClicked(btnValue As Integer) Handles ucBtnTools.btnClicked
        Try
            vsfDet.FinishEditing()
            vsfDet.Enabled = False
            With ucBtnTools
                Select Case btnValue
                    Case svtDtlBtns.svtInsert
                        If intDocID <= 0 Then Exit Sub
                        Call AddDocument()

                    Case svtDtlBtns.svtRemove
                        Call RemoveRecord()
                        If Not vsfDet.Rows.Count > 1 Then
                            MsgBox("No data to Remove", vbExclamation)
                        End If
                        Call .vsfRemove(vsfDet, vsfDet.Row, vDetItem)
                        'ReArrangeSlNo(vsfDet, vDetSlno)
                    Case svtDtlBtns.svtCopy
                        Call .vsfCopy(vsfDet, vsfDet.Row, vDetDocNo)

                    Case svtDtlBtns.svtPaste
                        Call .vsfPaste(vsfDet, vsfDet.Row, vDetDocNo + 1)

                    Case svtDtlBtns.svtUp
                        Call MoveOneRowUp(vsfDet, vDetDocNo)
                        vsfDet.Enabled = True

                    Case svtDtlBtns.svtDown
                        Call MoveOneRowDown(vsfDet, vDetDocNo)

                    Case svtDtlBtns.svtExport
                        Call .vsfExport(vsfDet)

                    Case Else
                        Exit Sub
                End Select
                vsfDet.Enabled = True
            End With
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information, Me.Text)
        End Try
    End Sub

    Private Sub frmRptQRDesign_KeyUp(sender As Object, e As KeyEventArgs) Handles Me.KeyUp
        If e.KeyCode = Keys.F9 And e.Control = True Then
            Call initFormVsfDet(Me, Me.Text, vsfDet, "DET")
            Call initFormVsfDet(Me, Me.Text, vsfDoc, "INFO")
            MsgBox("Restored Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
        If e.KeyCode = Keys.F8 And e.Control = True Then
            Call SaveActFormVsfDet(Me, Me.Text, "DET", vsfDet)
            Call SaveActFormVsfDet(Me, Me.Text, "INFO", vsfDoc)
            MsgBox("Saved Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
    End Sub

    Private Sub AddDocument()
        intSlno = 0
        If intDocID <= 0 Then Exit Sub
        With vsfDoc
            For i As Int32 = 1 To .Rows.Count - 1
                For j As Int32 = 1 To .Cols.Count - 1
                    .SetData(i, j, "")
                Next
            Next
        End With
        'For i = 1 To 6
        vsfDoc.SetData(vDocNo, 1, vsfDoc.GetData(vDocNo, 0))
        vsfDoc.SetData(vDocDate, 1, vsfDoc.GetData(vDocDate, 0))
        vsfDoc.SetData(vItem, 1, vsfDoc.GetData(vItem, 0))
        vsfDoc.SetData(vItemName, 1, vsfDoc.GetData(vItemName, 0))
        vsfDoc.SetData(vUOM, 1, vsfDoc.GetData(vUOM, 0))
        ' vsfDoc.SetData(vsfDoc.Row, vUOM, vsfDoc.GetData(vsfDoc.Row, 0))
        vsfDoc.SetData(vSQty, 1, vsfDoc.GetData(vSQty, 0))
        vsfDoc.SetData(vSRate, 1, vsfDoc.GetData(vSRate, 0))
        vsfDoc.SetData(vPRate, 1, vsfDoc.GetData(vPRate, 0))
        vsfDoc.SetData(vPVal, 1, vsfDoc.GetData(vPVal, 0))
        ' Next
        vsfDoc.Rows(3).ComboList = "Customer|Supplier"
        vsfDoc.AutoSizeCols()

        gbDocuments.Visible = True
        gbDocuments.Dock = DockStyle.Fill


    End Sub
    'Private Sub TableId()
    '    Dim strSQL As String = ""
    '    Dim strTable As String
    '    Try
    '        'For i = 1 To 6
    '        strTable = vsfDoc.GetData(vsfDoc.Row, vTbl)
    '        'Next
    '        strSQL = "select COLUMN_NAME "
    '        strSQL &= "From INFORMATION_SCHEMA.COLUMNS "
    '        strSQL &= "Where table_name ='" & strTable & "' "

    '        'vsfDet.GetData(vsfDet.Row, vDetSlno)
    '        'strSQL = "Select * "
    '        'strSQL &= " From UtlProfitDtlFlds "
    '        'strSQL &= " where DocID = " & intDocID
    '        'strSQL &= " And Slno = " & intSlno
    '        'strSQL &= " Order by Slno "

    '        clsSVT.execSQLStatement(strSQL)
    '    Catch ex As Exception
    '        MsgBox(ex.Message, vbInformation)
    '    End Try
    'End Sub
    Private Sub vsfDoc_BeforeEdit(sender As Object, e As RowColEventArgs) Handles vsfDoc.BeforeEdit
        vsfDoc.ComboList = String.Empty
        If e.Col = vTbl Then
            ' vsfDoc.ComboList = GenVSfComboStringCond("TABLE_NAME", "Information_Schema.Columns", "Order by TABLE_NAME", "")
            vsfDoc.Cols(e.Col).ComboList = GenVSfComboStringCond("name", "sys.tables", "name")
            Exit Sub
        End If

        If e.Col = vFld Then
            'vsfDoc.ComboList = GenVSfComboStringCond("COLUMN_NAME", " Information_Schema.Columns", " Order by COLUMN_NAME", " where TABLE_NAME='" & Trim(vsfDoc.GetData(e.Row, vTbl)) & "'")
            vsfDoc.Cols(e.Col).ComboList = GenVSfComboStringCond("column_name", "Information_schema.Columns", " order by Column_name", " where table_name='" & vsfDoc.GetData(vsfDoc.Row, vTbl) & "'")
            Exit Sub
        End If

    End Sub

    Private Sub btDocClose_Click(sender As Object, e As EventArgs) Handles btDocClose.Click
        gbDocuments.Visible = False
        gbDocuments.Dock = DockStyle.None
    End Sub

    Private Sub SaveDocument()
        Dim strSQL As String = ""
        svtTrn = svtConn.BeginTransaction(IsolationLevel.ReadCommitted)
        Try
            If intSlno > 0 Then
                strSQL = "Delete from UtlProfitDtlFlds Where DocID=" & intDocID & " and Slno =" & intSlno

                If Not clsSVT.execSQLStatement(strSQL, svtTrn) Then
                    svtTrn.Rollback()
                    Exit Sub
                End If
            Else
                intSlno = clsSVT.getDataLongValue("Select Max(Slno) + 1 From UtlProfitDtlFlds Where DocID=" & intDocID, 1, False, svtTrn)
            End If



            With vsfDoc
                strSQL = "Insert Into UtlProfitDtlFlds ("
                strSQL &= "DocID,Slno,DocType, "
                strSQL &= "FldNoText,FldNoTbl,FldNo,"
                strSQL &= "FldDateText,FldDateTbl,FldDate,"
                strSQL &= "FldPartyText,FldPartyTbl,FldParty,"
                strSQL &= "FldItemText,FldItemTbl,FldItem,"
                strSQL &= "FldItemDescText,FldItemDescTbl,FldItemDesc,"
                strSQL &= "FldItemGroupText,FldItemGroupTbl,FldItemGroup,"
                strSQL &= "FldUOMText,FldUOMTbl,FldUOM,"
                strSQL &= "FldAD1Text,FldAD1Tbl,FldAD1,"
                strSQL &= "FldAD2Text,FldAD2Tbl,FldAD2,"
                strSQL &= "FldAD3Text,FldAD3Tbl,FldAD3,"
                strSQL &= "FldAD4Text,FldAD4Tbl,FldAD4,"
                strSQL &= "FldAD5Text,FldAD5Tbl,FldAD5,"
                strSQL &= "FldSQtyText,FldSQtyTbl,FldSQty,"
                strSQL &= "FldSRateText,FldSRateTbl,FldSRate,"
                strSQL &= "FldSValueText,FldSValueTbl,FldSValue,"
                strSQL &= "FldPRText,FldPRTbl,FldPR,"
                strSQL &= "FldPRVText,FldPRVTbl,FldPRV,"
                strSQL &= "FldDiffText,FldDiffTbl,FldDiff,"
                strSQL &= "FldCond) "
                strSQL &= " Values ("
                strSQL &= intDocID & ","
                strSQL &= intSlno & ","
                strSQL &= "'" & Trim(cboDocType.Text) & "',"
                For i As Int32 = 1 To .Rows.Count - 1
                    If .GetData(i, 1) <> "" AndAlso .GetData(i, 2) <> "" AndAlso .GetData(i, 3) <> "" Then
                        For j As Int32 = 1 To .Cols.Count - 1
                            strSQL &= "'" & GetVal(.GetData(i, j), "") & "',"
                        Next
                    Else
                        strSQL &= "'','','',"
                    End If
                Next
                strSQL &= "'" & CStr(txtDocCond.Text).Replace("'", "''") & "')"
            End With

            If Not clsSVT.execSQLStatement(strSQL, svtTrn) Then
                svtTrn.Rollback()
                Exit Sub
            End If

            svtTrn.Commit()
            MsgBox("Updated Successfully...", MsgBoxStyle.Information, Me.Text)
            Call fillDetDtls()
            gbDocuments.Visible = False
            gbDocuments.Dock = DockStyle.None
            'ReArrangeSlNo(vsfDet, vDetSlno)
        Catch ex As Exception
            MsgBox("Error..." & ex.Message, MsgBoxStyle.Critical, Me.Text)
            svtTrn.Rollback()
        End Try
    End Sub
    Private Sub RemoveRecord()
        If vsfDet.Rows.Count <= 1 Then Exit Sub
        Dim strSQL As String = ""
        Try
            strSQL = "Delete from UtlProfitDtlFlds Where DocID=" & intDocID & " and Slno = " & vsfDet.GetData(vsfDet.Row, vDetSlno)
            clsSVT.execSQLStatement(strSQL)
            'MsgBox("Deleted Successfully...", MsgBoxStyle.Information, Me.Text)
            Call fillDetDtls()
        Catch ex As Exception
            MsgBox(ex.Message, vbInformation)
        End Try
    End Sub

    Private Sub btnDocSave_Click(sender As Object, e As EventArgs) Handles btnDocSave.Click
        If Trim(cboDocType.Text) = String.Empty Then
            MsgBox("Doc Type cannot be Empty..", vbInformation, Me.Text)
            cboDocType.Focus()
            Exit Sub
        End If
        Call SaveDocument()
    End Sub

    Private Sub vsfDet_DoubleClick(sender As Object, e As EventArgs) Handles vsfDet.DoubleClick
        With vsfDet

            If .Row <= 0 Then Exit Sub
            If GetVal(.GetData(.Row, vDetSlno), 0) <= 0 Then Exit Sub
            If intDocID <= 0 Then Exit Sub
            intSlno = GetVal(.GetData(.Row, vDetSlno), 0)

            Call FillDocuments()
            vsfDoc.AutoSizeCols()
            gbDocuments.Visible = True
            gbDocuments.Dock = DockStyle.Fill
        End With
    End Sub

    Private Sub FillDocuments()
        Dim strSQL As String = ""
        Dim rs As oledb.oledbDataReader

        strSQL = "Select * "
        strSQL &= " From UtlProfitDtlFlds "
        strSQL &= " where DocID = " & intDocID
        strSQL &= " and Slno = " & intSlno
        strSQL &= " Order by Slno "

        Try
            With vsfDoc
                For i As Int32 = 1 To .Rows.Count - 1
                    For j As Int32 = 1 To .Cols.Count - 1
                        .SetData(i, j, "")
                    Next
                Next
            End With

            rs = clsSVT.getDataReaderFromSQL(strSQL)

            If Not rs.HasRows Then
                rs.Close()
                Exit Sub
            End If

            With vsfDoc
                rs.Read()
                cboDocType.Text = rs.Item("DocType")
                .SetData(vDocNo, vCaption, GetVal(rs!FldNoText, ""))
                .SetData(vDocDate, vCaption, GetVal(rs!FldDateText, ""))
                .SetData(vParty, vCaption, GetVal(rs!FldPartyText, ""))
                .SetData(vItem, vCaption, GetVal(rs!FldItemText, ""))
                .SetData(vItemName, vCaption, GetVal(rs!FldItemDescText, ""))
                .SetData(vItemGroup, vCaption, GetVal(rs!FldItemGroupText, ""))
                .SetData(vUOM, vCaption, GetVal(rs!FldUOMText, ""))
                .SetData(vAdd1, vCaption, GetVal(rs!FldAD1Text, ""))
                .SetData(vAdd2, vCaption, GetVal(rs!FldAD2Text, ""))
                .SetData(vAdd3, vCaption, GetVal(rs!FldAD3Text, ""))
                .SetData(vAdd4, vCaption, GetVal(rs!FldAD4Text, ""))
                .SetData(vAdd5, vCaption, GetVal(rs!FldAD5Text, ""))
                .SetData(vSQty, vCaption, GetVal(rs!FldSQtyText, ""))
                .SetData(vSRate, vCaption, GetVal(rs!FldSRateText, ""))
                .SetData(vSVal, vCaption, GetVal(rs!FldSValueText, ""))
                .SetData(vPRate, vCaption, GetVal(rs!FldPRText, ""))
                .SetData(vPVal, vCaption, GetVal(rs!FldPRVText, ""))
                .SetData(vDiff, vCaption, GetVal(rs!FldDiffText, ""))

                .SetData(vDocNo, vTbl, GetVal(rs!FldNoTbl, ""))
                .SetData(vDocDate, vTbl, GetVal(rs!FldDateTbl, ""))
                .SetData(vParty, vTbl, GetVal(rs!FldPartyTbl, ""))
                .SetData(vItem, vTbl, GetVal(rs!FldItemTbl, ""))
                .SetData(vItemName, vTbl, GetVal(rs!FldItemDescTbl, ""))
                .SetData(vItemGroup, vTbl, GetVal(rs!FldItemGroupTbl, ""))
                .SetData(vUOM, vTbl, GetVal(rs!FldUOMTbl, ""))
                .SetData(vAdd1, vTbl, GetVal(rs!FldAD1Tbl, ""))
                .SetData(vAdd2, vTbl, GetVal(rs!FldAD2Tbl, ""))
                .SetData(vAdd3, vTbl, GetVal(rs!FldAD3Tbl, ""))
                .SetData(vAdd4, vTbl, GetVal(rs!FldAD4Tbl, ""))
                .SetData(vAdd5, vTbl, GetVal(rs!FldAD5Tbl, ""))
                .SetData(vSQty, vTbl, GetVal(rs!FldSQtyTbl, ""))
                .SetData(vSRate, vTbl, GetVal(rs!FldSRateTbl, ""))
                .SetData(vSVal, vTbl, GetVal(rs!FldSValueTbl, ""))
                .SetData(vPRate, vTbl, GetVal(rs!FldPRTbl, ""))
                .SetData(vPVal, vTbl, GetVal(rs!FldPRVTbl, ""))
                .SetData(vDiff, vTbl, GetVal(rs!FldDiffTbl, ""))

                .SetData(vDocNo, vFld, GetVal(rs!FldNo, ""))
                .SetData(vDocDate, vFld, GetVal(rs!FldDate, ""))
                .SetData(vParty, vFld, GetVal(rs!FldParty, ""))
                .SetData(vItem, vFld, GetVal(rs!FldItem, ""))
                .SetData(vItemName, vFld, GetVal(rs!FldItemDesc, ""))
                .SetData(vItemGroup, vFld, GetVal(rs!FldItemGroup, ""))
                .SetData(vUOM, vFld, GetVal(rs!FldUOM, ""))
                .SetData(vAdd1, vFld, GetVal(rs!FldAD1, ""))
                .SetData(vAdd2, vFld, GetVal(rs!FldAD2, ""))
                .SetData(vAdd3, vFld, GetVal(rs!FldAD3, ""))
                .SetData(vAdd4, vFld, GetVal(rs!FldAD4, ""))
                .SetData(vAdd5, vFld, GetVal(rs!FldAD5, ""))
                .SetData(vSQty, vFld, GetVal(rs!FldSQty, ""))
                .SetData(vSRate, vFld, GetVal(rs!FldSRate, ""))
                .SetData(vSVal, vFld, GetVal(rs!FldSValue, ""))
                .SetData(vPRate, vFld, GetVal(rs!FldPR, ""))
                .SetData(vPVal, vFld, GetVal(rs!FldPRV, ""))
                .SetData(vDiff, vFld, GetVal(rs!FldDiff, ""))

                'txtDocCond.Text = GetVal(rs!FldCond, "")

                rs.Close()
            End With

        Catch ex As Exception
            MsgBox(ex.Message & " [fillDetDtls]", vbInformation, Me.Text)
        End Try
    End Sub
    Private Sub cboDocType_TextChanged(sender As Object, e As EventArgs) Handles cboDocType.TextChanged
        If cboDocType.Text = "" Then
            vsfDoc.Visible = False
        End If
        If cboDocType.Text = "Sales" Then
            vsfDoc.Visible = True
            vsfDoc.Rows(vPRate).Visible = False
            vsfDoc.Rows(vPVal).Visible = False

            vsfDoc.Rows(vSQty).Visible = True
            vsfDoc.Rows(vSRate).Visible = True
            vsfDoc.Rows(vSVal).Visible = True
        End If
        If cboDocType.Text = "Purchase" Then
            vsfDoc.Visible = True
            vsfDoc.Rows(vSQty).Visible = False
            vsfDoc.Rows(vSRate).Visible = False
            vsfDoc.Rows(vSVal).Visible = False

            vsfDoc.Rows(vPRate).Visible = True
            vsfDoc.Rows(vPVal).Visible = True
        End If
    End Sub
    Private Sub vsfDoc_DoubleClick(sender As Object, e As EventArgs) Handles vsfDoc.DoubleClick

        Select Case vsfDoc.Row
            Case vDocDate

                If txtDocCond.Text <> "" Then
                    txtDocCond.Text &= vbCrLf & "and" & " " & vsfDoc.GetData(vDocDate, vTbl) & "." & vsfDoc.GetData(vDocDate, vFld) &
                            " between @FROMDATE AND @TODATE"
                Else
                    txtDocCond.Text = "and" & " " & vsfDoc.GetData(vDocDate, vTbl) & "." & vsfDoc.GetData(vDocDate, vFld) &
                            " between @FROMDATE AND @TODATE"
                End If
            Case vSQty
                If txtDocCond.Text <> "" Then
                    txtDocCond.Text &= vbCrLf & "and" & " " & vsfDoc.GetData(vSQty, vTbl) & "." & vsfDoc.GetData(vSQty, vFld) &
                            " in (@ID)"
                Else
                    txtDocCond.Text = "and" & " " & vsfDoc.GetData(vSQty, vTbl) & "." & vsfDoc.GetData(vSQty, vFld) &
                            "in (@ID) "
                End If
            Case Else
                If txtDocCond.Text <> "" Then
                    Dim str As String = txtDocCond.Text
                    str = Mid(str, 1, txtDocCond.SelectionStart - 1)

                    txtDocCond.Text &= vbCrLf & "and" & " " & vsfDoc.GetData(vsfDoc.Row, vTbl) & "." & vsfDoc.GetData(vsfDoc.Row, vFld) &
                            " ="
                Else
                    txtDocCond.Text = "and" & " " & vsfDoc.GetData(vsfDoc.Row, vTbl) & "." & vsfDoc.GetData(vsfDoc.Row, vFld) &
                            "= "
                End If
        End Select
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