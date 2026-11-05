Imports C1.Win.C1FlexGrid
Imports System.Reflection
Imports System.IO
Imports System.ComponentModel
Imports Microsoft.Office.Interop
Imports ClosedXML.Excel
Public Class frmUtlImportAnyFld
    Public strList As String
    Dim strItemGroupList As String
    Dim strItemPkg As String
    Dim strItemList As String
    Dim clsSVT As New svtLibrary
    Dim strFlds As String
    Dim dvXl As DataView
    Dim dtXl As DataTable

    Const vFldsSel = 0
    Const vFldsCode = 1
    Const vFldsDataType = 2

    'Const vXlSlNo = 0
    Const vXlItemID = 0
    Const vXlItemCode = 1
    'Const vXlItemName = 3
    Const vXlTotCols = 2

    Private Sub frmUtlImportAnyFld_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.KeyPreview = True

        Call initScreen()
        Call initFormVsfDet(Me, Me.Text, vsfFlds, "FLDS")
        Call initFormVsfDet(Me, Me.Text, vsfExcel, "EXCEL")
        Call fillFormProperties(Me)
        Call initGridStyles(Me)
    End Sub

    Private Sub chkItem_CheckedChanged(sender As Object, e As EventArgs) Handles chkItem.CheckedChanged
        Dim strSQL As String
        If chkItem.Checked = False Then
            strList = ""
            strItemList = ""
            Exit Sub
        End If

        Dim frmSel As New frmUtlMultiSelection

        strSQL = "Select 0 as sel, ItemID, ItemName from MstItem where ItemID > 0 and "
        strSQL = strSQL & " InActive = 'N' order by ItemName"
        Call frmSel.InitVsfSel(strSQL, 1, 1, True, Me, False)
        frmSel.Text = "Select [Multiple] Item"
        frmSel.vsfDet.SetData(0, 0, "Sel")
        frmSel.ShowDialog()
        strItemList = strList
        If Trim(strItemList) = "" Then chkItem.Checked = False
    End Sub

    Private Sub chkItemGroup_CheckedChanged(sender As Object, e As EventArgs) Handles chkItemGroup.CheckedChanged
        Dim strSQL As String
        If chkItemGroup.Checked = False Then
            strList = ""
            strItemGroupList = ""
            Exit Sub
        End If

        Dim frmSel As New frmUtlMultiSelection

        strSQL = "Select distinct 0 as sel, ItemGroupID, ItemGroupCode from MstItemGroup where ItemGroupID > 0 and "
        strSQL = strSQL & " InActive = 'N' order by ItemGroupCode"
        Call frmSel.InitVsfSel(strSQL, 1, 1, True, Me, False)
        frmSel.Text = "Select [Multiple] Item Groups"
        frmSel.vsfDet.SetData(0, 0, "Sel")
        frmSel.ShowDialog()
        strItemGroupList = strList

        If Trim(strItemGroupList) = "" Then chkItemGroup.Checked = False
    End Sub

    Private Sub chkPacking_CheckedChanged(sender As Object, e As EventArgs) Handles chkPacking.CheckedChanged
        Dim strSQL As String
        If chkPacking.Checked = False Then
            strList = ""
            strItemPkg = ""
            Exit Sub
        End If

        Dim frmSel As New frmUtlMultiSelection

        strSQL = "Select distinct 0 as sel, PKGID, PKGCode,LTRPERBOT from MstPKG where PKGID > 0 and "
        strSQL = strSQL & " InActive = 'N' order by LTRPERBOT"
        Call frmSel.InitVsfSel(strSQL, 1, 1, True, Me, False)
        frmSel.Text = "Select [Multiple] Item Packaging"
        frmSel.vsfDet.SetData(0, 0, "Sel")
        frmSel.ShowDialog()
        strItemPkg = strList

        If Trim(strItemPkg) = "" Then chkPacking.Checked = False
    End Sub

    Private Sub btnTemplate_Click(sender As Object, e As EventArgs) Handles btnTemplate.Click
        Dim strSql As String, strIDfldName As String, strIDfldQry As String
        Dim rsItem As oledb.oledbDataReader
        strFlds = ""
        With vsfFlds
            For i As Int32 = 1 To .Rows.Count - 1
                If .GetCellCheck(i, vFldsSel) = CheckEnum.Checked Then
                    If InStr(.GetData(i, vFldsCode), "ID") > 0 Then
                        strIDfldName = Replace(.GetData(i, vFldsCode), "ID", "")
                        strIDfldQry = "(Select " & strIDfldName & "Code from MST" & strIDfldName & " where " & .GetData(i, vFldsCode) & "=MstItem." & .GetData(i, vFldsCode) & ") as " & .GetData(i, vFldsCode)

                        strFlds = strIDfldQry & "," & strFlds
                    Else
                        strFlds = .GetData(i, vFldsCode) & "," & strFlds
                    End If
                End If
            Next
        End With

        If strFlds = "" Then
            MsgBox("Please Select atleast one field!", MsgBoxStyle.Information, Me.Text)
            Exit Sub
        End If
        strFlds = Strings.Left(strFlds, Len(strFlds) - 1)

        strSql = "Select ITEMID,ITEMCODE,ITEMNAME," & strFlds & " From MstItem where ITEMID<>0"
        If strItemGroupList <> "" Then
            strSql &= " and ItemGroupID in (" & strItemGroupList & ")"
        End If
        If strItemList <> "" Then
            strSql &= " and ItemID in (" & strItemList & ")"
        End If
        If strItemPkg <> "" Then
            strSql &= " and PKGID in (" & strItemPkg & ")"
        End If
        strSql &= " order by ItemCode"
        rsItem = clsSVT.getDataReaderFromSQL(strSql)

        If Not rsItem.HasRows Then
            rsItem.Close()
            MsgBox("No Records Found!", MsgBoxStyle.Information, Me.Text)
            Exit Sub
        End If

        Dim strPath As String, strFileName As String, intChar As Integer = 66, intRow As Integer = 1
        strPath = Application.StartupPath() & "\Temp\"
        strFileName = Date.Now.ToString("yyyyMMddHHmmss") & "_BulkImpFlds"


        Dim xlapp As New Excel.Application
            Dim xlWB As Excel.Workbook = xlapp.Workbooks.Add()
        Dim xlWS As Excel.Worksheet = xlWB.Worksheets.Add
        Dim intCol As Integer, intCtr As Integer
        showWaitCursor()

        Try
            With xlapp

                .Range("A1").Value = "ItemID"
                .Range("B1").Value = "Item Name"

                intCol = 1
                For i = 1 To vsfFlds.Rows.Count - 1
                    If vsfFlds.GetCellCheck(i, vFldsSel) = CheckEnum.Checked Then
                        intCol = intCol + 1
                        .Range(getcol(intCol) & "1").Value = vsfFlds.GetData(i, vFldsCode)
                    End If
                Next
                '.Range("C1").Value = "Item Name"
                Do While rsItem.Read
                    intRow = intRow + 1
                    .Range("A" & intRow).Value = rsItem!ItemID
                    .Range("B" & intRow).Value = rsItem!ITEMNAME
                    intCtr = 1
                    For i = 1 To vsfFlds.Rows.Count - 1
                        If vsfFlds.GetCellCheck(i, vFldsSel) = CheckEnum.Checked Then
                            intCtr = intCtr + 1
                            .Range(getcol(intCtr) & intRow).Value = rsItem(vsfFlds.GetData(i, vFldsCode))
                        End If
                    Next i
                Loop
            End With
            xlWB.SaveAs(strPath & strFileName & ".xlsx")
            xlWB.Close()
            xlapp.Quit()
            rsItem.Close()
            showNormalCursor()
            MsgBox("Template Created", MsgBoxStyle.Information, "Process Completed")
            System.Diagnostics.Process.Start(strPath & strFileName & ".xlsx")

            pnlExcel.Visible = True
        Catch ex As Exception
            showNormalCursor()
            xlWB.Close()
            'xlWS.Dispose()
            xlapp.Quit()
            MsgBox("Template Excel Generation Failed. Details" & vbNewLine & ex.Message, MsgBoxStyle.Information, "Failed")
        End Try
        'End If
    End Sub

    Private Sub initScreen()
        With vsfFlds
            .Cols.Count = 3
            .Rows.Count = 1
            .SetData(0, vFldsSel, "Select Fields to Import")
            .SetData(0, vFldsCode, "Field Names")

            .Cols(vFldsSel).DataType = GetType(Boolean)

            SetHideCol(vsfFlds, True, vFldsDataType)
        End With

        Call initFormVsfDet(Me, Me.Text, vsfFlds, "FLDS")
        Call initFormVsfDet(Me, Me.Text, vsfExcel, "EXCEL")

        loadFlds()
    End Sub

    Private Sub vsfFlds_BeforeEdit(sender As Object, e As RowColEventArgs) Handles vsfFlds.BeforeEdit

    End Sub

    Private Sub loadFlds()
        Dim strSql As String
        Dim rs As oledb.oledbDataReader

        strSql = "select Distinct FieldName,Datatype from UtlItemBulkupdate order by FieldName"
        rs = clsSVT.getDataReaderFromSQL(strSql)
        If rs.HasRows Then
            With vsfFlds
                .Rows.Count = 1
                Do While rs.Read
                    .Rows.Count = .Rows.Count + 1
                    .SetData(.Rows.Count - 1, vFldsCode, rs!FieldName)
                    .SetData(.Rows.Count - 1, vFldsDataType, rs!Datatype)
                Loop
            End With
        End If
        rs.Close()
    End Sub

    Private Sub vsfFlds_KeyUp(sender As Object, e As KeyEventArgs) Handles vsfFlds.KeyUp
        If e.KeyCode = Keys.F9 And e.Control = True Then
            Call initFormVsfDet(Me, Me.Text, vsfFlds, "FLDS")
            MsgBox("Restored Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
        If e.KeyCode = Keys.F8 And e.Control = True Then
            Call SaveActFormVsfDet(Me, Me.Text, "FLDS", vsfFlds)
            MsgBox("Saved Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
    End Sub

    Private Sub vsfExcel_KeyUp(sender As Object, e As KeyEventArgs) Handles vsfExcel.KeyUp
        If e.KeyCode = Keys.F9 And e.Control = True Then
            Call initFormVsfDet(Me, Me.Text, vsfExcel, "EXCEL")
            MsgBox("Restored Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
        If e.KeyCode = Keys.F8 And e.Control = True Then
            Call SaveActFormVsfDet(Me, Me.Text, "EXCEL", vsfExcel)
            MsgBox("Saved Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If

        If e.KeyCode = Keys.C And e.Control = True Then
            vsfCopyItemCode(vsfExcel, vsfExcel.Row, vsfExcel.Col)
        End If
    End Sub

    Private Sub btnClose_Click_1(sender As Object, e As EventArgs) Handles btnClose.Click
        frmMain.CloseSelectedTabPage(Me)
    End Sub

    Private Sub btnSelect_Click(sender As Object, e As EventArgs) Handles btnSelect.Click
        Dim dialog As New OpenFileDialog
        Dim strFilename As String

        If dialog.ShowDialog() = DialogResult.OK Then
            strFilename = dialog.FileName
        Else
            Exit Sub
        End If
        If Not File.Exists(strFilename) Or strFilename = "" Then
            MsgBox("Please select a valid file for importing", MsgBoxStyle.Critical, "Error")
            Exit Sub
        End If
        txtSelect.Text = strFilename

        showWaitCursor()
        Call importData(strFilename)
        showNormalCursor()
    End Sub

    Private Sub importData(argFilename As String)
        vsfExcel.DataSource = Nothing
        With vsfExcel
            .Cols.Count = vXlTotCols
            .Rows.Count = 1
            .Rows.Count = 2
            .SetData(0, vXlItemID, "Item ID")
            '.SetData(0, vXlSlNo, "SlNo")
            .SetData(0, vXlItemCode, "Item Name")

            SetHideCol(vsfExcel, True, vXlItemID)
        End With

        SetFlexColor(vsfExcel, "", vXlItemCode) 'vXlSlNo

        Dim xlapp As New Excel.Application
        Dim i As Integer, intRow As Integer, intChar As Integer = 67, intCol As Integer
        Dim strs As String
        Dim strTemplateStrings As String = ""

        strTemplateStrings = ""

        xlapp.Workbooks.Open(argFilename, , True)
        With xlapp.Application
            strs = ""

            strTemplateStrings = .Range("A1").Value & "," & .Range("B1").Value

            If strTemplateStrings.ToUpper <> "ITEMID,ITEM NAME" Then
                MsgBox("Please Select a valid Template!", MsgBoxStyle.Information, Me.Text)
                Exit Sub
            End If

            vsfExcel.Rows.Count = 1
            i = 2
            Dim blnRunLoop As Boolean = True
            Try
                Do Until GetVal(.Range("A" & i).Value, 0) = 0
                    vsfExcel.Rows.Count = vsfExcel.Rows.Count + 1
                    'vsfExcel.SetData(vsfExcel.Rows.Count - 1, vXlSlNo, vsfExcel.Rows.Count - 1)
                    vsfExcel.SetData(vsfExcel.Rows.Count - 1, vXlItemID, .Range("A" & i).Value)
                    vsfExcel.SetData(vsfExcel.Rows.Count - 1, vXlItemCode, .Range("B" & i).Value)
                    i = i + 1
                Loop

                Do Until GetVal(.Range(Chr(intChar) & "1").Value, "") = ""
                    intCol = -1
                    For a As Int32 = 1 To vsfExcel.Cols.Count - 1
                        If vsfExcel.GetData(0, a) = GetVal(.Range(Chr(intChar) & "1").Value, "") Then
                            intCol = a
                        End If
                    Next
                    If intCol = -1 Then
                        vsfExcel.Cols.Count = vsfExcel.Cols.Count + 1
                        intCol = vsfExcel.Cols.Count - 1
                        vsfExcel.SetData(0, intCol, GetVal(.Range(Chr(intChar) & "1").Value, ""))
                    End If
                    i = 2
                    intRow = 1
                    Do While Not IsDBNull(.Range(Chr(intChar) & i).Value)
                        If intRow = vsfExcel.Rows.Count - 1 Then
                            vsfExcel.SetData(vsfExcel.Rows.Count - 1, intCol, .Range(Chr(intChar) & i).Value)
                            Exit Do
                        End If
                        vsfExcel.SetData(intRow, intCol, .Range(Chr(intChar) & i).Value)
                        i = i + 1
                        intRow = intRow + 1
                    Loop
                    intChar = intChar + 1

                Loop

                .Workbooks(1).Close(SaveChanges:=False)
            Catch ex As Exception
                MsgBox("Cannot export to Excel." & vbCrLf & ex.Message, MsgBoxStyle.Critical, "Import Error")
            End Try
        End With
        pnlExcel.Visible = True
    End Sub

    Private Sub btnImportData_Click(sender As Object, e As EventArgs) Handles btnImportData.Click
        Dim strSql As String, strDATAtype As String, strUpdFlds As String, strColName As String
        Dim x As Integer, strIDfldName As String, intFldID As Integer = 0
        showWaitCursor()
        Dim svtTrn As oledb.oledbTransaction

        'svtTrn = svtConn.BeginTransaction(IsolationLevel.ReadCommitted)
        Try
            With vsfExcel
                For i As Int32 = 1 To .Rows.Count - 1
                    strSql = "Update MstItem set "
                    strUpdFlds = ""
                    For c As Int32 = vXlItemCode + 1 To .Cols.Count - 1
                        strColName = .GetData(0, c)
                        strDATAtype = ""
                        For x = 1 To vsfFlds.Rows.Count - 1
                            If .GetData(0, c) = vsfFlds.GetData(x, vFldsCode) Then
                                strDATAtype = vsfFlds.GetData(x, vFldsDataType)
                                Exit For
                            End If
                        Next x

                        If InStr(strColName, "ID") > 0 Then
                            strIDfldName = Replace(strColName, "ID", "")
                            intFldID = clsSVT.getDataLongValue("Select " & strColName & " from MST" & strIDfldName & " where " & strIDfldName & "Code='" & .GetData(i, c) & "'", 0, False)
                        End If

                        Select Case strDATAtype.ToUpper
                            Case "TEXT"
                                strUpdFlds &= strColName & "='" & .GetData(i, c) & "',"
                            Case "INTEGER", "NUMERIC"
                                If InStr(strColName, "ID") > 0 Then
                                    strUpdFlds &= strColName & "=" & intFldID & ","
                                Else
                                    strUpdFlds &= strColName & "=" & Val(.GetData(i, c)) & ","
                                End If

                            Case "DATE"
                                strUpdFlds &= strColName & "='" & getDateFormatInInsert(.GetData(i, c)) & "',"
                        End Select
                    Next
                    strUpdFlds = Strings.Left(strUpdFlds, Len(strUpdFlds) - 1)

                    strSql &= strUpdFlds & " where ITEMID=" & Val(.GetData(i, vXlItemID))
                    clsSVT.execSQLStatement(strSql, svtTrn)
                Next
            End With

            'svtTrn.Commit()

            showNormalCursor()
            MsgBox("Data Imported Successfully!", MsgBoxStyle.Information, Me.Text)

            btnClose.PerformClick()

        Catch ex As Exception
            ' svtTrn.Rollback()
            MsgBox("Error During Import!" & ex.Message, MsgBoxStyle.Information, Me.Text)
            showNormalCursor()
        End Try

    End Sub

    Private Sub vsfExcel_BeforeEdit(sender As Object, e As RowColEventArgs) Handles vsfExcel.BeforeEdit
        e.Cancel = True
        If e.Col > vXlItemCode Then
            e.Cancel = False
        End If
    End Sub

    Private Sub btnExportToXL_Click(sender As Object, e As EventArgs) Handles btnExportToXL.Click
        ExportToXL(vsfExcel)

    End Sub

    Private Sub btnLoadtoGrd_Click(sender As Object, e As EventArgs) Handles btnLoadtoGrd.Click
        Dim strSql As String, strIDfldName As String, strIDfldQry As String
        Dim rsItem As oledb.oledbDataReader
        strFlds = ""
        strIDfldName = ""
        strIDfldQry = ""

        With vsfFlds
            For i As Int32 = 1 To .Rows.Count - 1
                If .GetCellCheck(i, vFldsSel) = CheckEnum.Checked Then
                    If InStr(.GetData(i, vFldsCode), "ID") > 0 Then
                        strIDfldName = Replace(.GetData(i, vFldsCode), "ID", "")
                        strIDfldQry = "(Select " & strIDfldName & "Code from MST" & strIDfldName & " where " & .GetData(i, vFldsCode) & "=MstItem." & .GetData(i, vFldsCode) & ") as " & .GetData(i, vFldsCode)

                        strFlds = strIDfldQry & "," & strFlds
                    Else
                        strFlds = .GetData(i, vFldsCode) & "," & strFlds
                    End If
                End If
            Next
        End With

        If strFlds = "" Then
            MsgBox("Please Select atleast one field!", MsgBoxStyle.Information, Me.Text)
            Exit Sub
        End If
        strFlds = Strings.Left(strFlds, Len(strFlds) - 1)

        strSql = "Select ITEMID,ITEMNAME," & strFlds & " From MstItem where ITEMID<>0"
        If strItemGroupList <> "" Then
            strSql &= " and ItemGroupID in (" & strItemGroupList & ")"
        End If
        If strItemList <> "" Then
            strSql &= " and ItemID in (" & strItemList & ")"
        End If
        If strItemPkg <> "" Then
            strSql &= " and PKGID in (" & strItemPkg & ")"
        End If
        strSql &= " order by ItemCode"
        rsItem = clsSVT.getDataReaderFromSQL(strSql)

        If Not rsItem.HasRows Then
            rsItem.Close()
            MsgBox("No Records Found!", MsgBoxStyle.Information, Me.Text)
            Exit Sub
        End If

        dtXl = clsSVT.getDATATable(strSql)
        dvXl = New DataView(dtXl)
        vsfExcel.DataSource = dvXl

        pnlExcel.Visible = True

    End Sub
End Class