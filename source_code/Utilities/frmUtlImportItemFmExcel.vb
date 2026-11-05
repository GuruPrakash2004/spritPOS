Imports Microsoft.Office.Interop
Imports C1.Win.C1FlexGrid

Public Class frmUtlImportItemFmExcel
    Dim intSelRow As Integer '# A - Starting row
    Dim strErrorMsg As String
    Dim blnValidData As Boolean
    Dim strRights As String
    Dim intDocID As Integer

    'Item Related Information
    Dim intItemDocID As Int32 = 0
    Dim strItemTblName As String
    Dim strFldItemID As String
    Dim strFldItemCode As String
    Dim strFldItemName As String
    Dim strItemShForm As String

    'Party Related Information
    Dim intPartyDocID As Int32 = 0
    Dim strPartyTblName As String
    Dim strFldPartyID As String
    Dim strFldPartyCode As String
    Dim strFldPartyName As String
    Dim strPartyShForm As String

    Dim strFldr As String
    Dim strTemp As String
    Dim clssvt As New svtLibrary

    Public objFrm As Object
    Public argVSF As C1FlexGrid

    Const vsfSlNo = 0
    Const vsfItemID = 1
    Const vsfItemCode = 2
    Const vsfItemName = 3
    Const vsfUom = 4
    Const vDetMemno = 5
    Const vsfRate = 6

    Const vExErr = 7
    Const vExBlnErr = 8

    Public Sub New()
        ' This call is required by the designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
    End Sub

    Public Sub getUtlTrnData(ByVal argDocID As Integer)
        Dim rsDummy As oledb.oledbDataReader
        rsDummy = clssvt.getDataReaderFromSQL("Select * from UtlTransactions where DocID = " & argDocID)

        With rsDummy
            If .HasRows Then
                intDocID = argDocID
                .Read()

                intPartyDocID = GetVal(!MstDocIDParty, 0)

                If intPartyDocID <= 0 Then
                    MsgBox("No Party Table Present.. Application Closed...", MsgBoxStyle.Information, Me.Text)
                    Me.Close()
                Else
                    strPartyShForm = clssvt.getDataStringValue("Select DocShForm From UtlTransactions where DocID = " & Val(intPartyDocID), "", False)
                    strPartyTblName = "Mst" & strPartyShForm
                    strFldPartyID = strPartyShForm & "ID"
                    strFldPartyCode = strPartyShForm & "Code"
                    strFldPartyName = strPartyShForm & "Name"
                End If

                intItemDocID = GetVal(!MstDocIDItem, 0)

                If intItemDocID = 0 Then
                    MsgBox("No Item Table Present.. Application Closed...", MsgBoxStyle.Information, Me.Text)
                    Me.Close()
                Else
                    strItemShForm = clssvt.getDataStringValue("Select DocShForm From UtlTransactions where DocID = " & Val(intItemDocID), "", False)
                    strItemTblName = "Mst" & strItemShForm
                    strFldItemID = strItemShForm & "ID"
                    strFldItemCode = strItemShForm & "Code"
                    strFldItemName = strItemShForm & "Name"
                End If
            End If
            .Close()
        End With
    End Sub
    Private Sub frmUtlImportItemFmExcel_KeyUp(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyUp
        If e.KeyCode = Keys.F9 And e.Control = True Then
            Call initFormSize(Me, Me.Text, False, 577, 349, 0, 0)
            MsgBox("Restored Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
        If e.KeyCode = Keys.F8 And e.Control = True Then
            Call SaveActFormSettings(Me, Me.Text)
            Call SaveActFormVsfDet(Me, Me.Text, "Excel", vsfExcel)
            MsgBox("Saved Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
    End Sub

    Private Sub frmUtlItemImport_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.KeyPreview = True
        Call initFormSize(Me, Me.Text, True, 577, 349, 0, 0)
        Call getUtlTrnData(intDocID)
        intSelRow = 2
        With vsfExcel
            .Rows.Count = 1
            .Cols.Count = 9
            .Rows.Fixed = 1
            Call SetColWidth(vsfExcel, 50, 200, 200, 200, 200, 200, 200)

            .SetData(0, vsfSlNo, "Sl No")
            .SetData(0, vsfItemID, "Item ID")
            .SetData(0, vsfItemCode, gItemCode)
            .SetData(0, vsfItemName, gItemName)
            .SetData(0, vsfUom, "UOM")
            .SetData(0, vsfRate, "Rate")
            .SetData(0, vExErr, "Error Record")
            .SetData(0, vExBlnErr, "blnerr")

            vsfExcel.Styles.Fixed.Font = New Font("Lucida Sans", vsfExcel.Font.Size, FontStyle.Bold)
            vsfExcel.Styles.Fixed.TextAlign = TextAlignEnum.CenterCenter

            Call SetFlexColor(vsfExcel, "", vsfSlNo, vsfUom, vExErr, vExBlnErr, vsfItemCode, vsfItemName, vsfRate)

            SetHideCol(vsfExcel, True, vsfItemID, vExBlnErr, vExErr, vDetMemno)
            .Styles(CellStyleEnum.Normal).WordWrap = True
            .AllowResizing = AllowResizingEnum.Both
            .AutoSizeRows()
            Call wrapGrid()
        End With

        Call enablebtn(False)
        Call EnableFields(False, 1)
        Call initGridStyles(Me)
    End Sub

    Private Sub wrapGrid()
        With vsfExcel
            .Styles(CellStyleEnum.Normal).WordWrap = True
            .AllowResizing = AllowResizingEnum.Both
            .AutoSizeRows()
        End With
    End Sub

    Private Sub enablebtn(ByVal argBLN As Boolean)
        lblError.Visible = argBLN
        btnRemErr.Visible = True
        optAppnd.Visible = argBLN
        optDel.Visible = argBLN
    End Sub

    Private Sub btnBrowse_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowse.Click
        Cdlg.DefaultExt = "xlsx"
        Cdlg.Filter = "Excel File (*.xlsx)|*.xlsx"
        Cdlg.FileName = "*.xlsx"

        If Cdlg.ShowDialog() <> DialogResult.OK Then
            Return
            Exit Sub
        End If
        txtXLFilePath.Text = Cdlg.FileName
        Call EnableFields(False, 1)
        btnImportFromXL.Enabled = True
    End Sub

    Private Sub EnableFields(ByVal blnEnable As Boolean, ByVal argType As Integer)
        Select Case argType
            Case 1
                btnSave.Enabled = blnEnable
            Case 2
                btnSave.Enabled = Not (blnEnable)
            Case 3
                btnSave.Enabled = blnEnable
                btnImportFromXL.Enabled = blnEnable
            Case 4

        End Select
    End Sub

    Private Sub btnImportFromXL_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImportFromXL.Click
        If Trim(txtXLFilePath.Text) = "" Then
            MsgBox("Please select excel file path..", MsgBoxStyle.Information + vbOKOnly, Me.Text)
            Exit Sub
        End If

        If Dir(Trim(txtXLFilePath.Text)) = "" Then
            MsgBox("File not found", MsgBoxStyle.Information + vbOKOnly, Me.Text)
            Exit Sub
        End If

        Call importFromXL(Trim(txtXLFilePath.Text))

        optAppnd.Checked = True

        If Not blnValidData Then

            Call EnableFields(False, 1)
            lblError.Visible = True
            enablebtn(True)

            Call EnableFields(True, 1)
        Else
            Call EnableFields(True, 1)
            lblError.Visible = False
            btnRemErr.Visible = False
            Call enablebtn(True)
        End If

        txtNoOfInv.Text = vsfExcel.Rows.Count - 1

    End Sub

    Private Sub importFromXL(ByVal argFileName As String)
        Dim xlapp As New Excel.Application
        Dim i As Integer
        Dim strs As String
        Dim introw As Integer
        xlapp.Workbooks.Open(argFileName, , True)
        'Call InitScreenINV


        With xlapp.Application

            strs = ""
            For i = 1 To 2
                strs = strs & UCase(Trim(xlapp.ActiveSheet.Range(Chr(64 + i) & 1).Value)) & ","
            Next i

            If Trim(strs) <> "" Then strs = LSet(strs, Len(strs) - 1)


            'Invalid XLS file selected validation
            If Trim(UCase(strs)) <> UCase(Trim("ItemCode,Rate")) Then
                MsgBox("Please select the valid template", vbInformation + vbOKOnly, Me.Text)
                vsfExcel.Clear(C1.Win.C1FlexGrid.ScrollFlags.None)
                vsfExcel.Rows.Count = 1
                Exit Sub
            End If
            vsfExcel.Rows.Count = 1

            i = 2 'ASSIGNING THE FIRST ROW
            Try
                Do While Trim(.Range("A" & i).Value) & Trim(.Range("B" & i).Value) <> ""
                    introw = vsfExcel.Rows.Count
                    vsfExcel.Rows.Count = vsfExcel.Rows.Count + 1

                    vsfExcel.SetData(introw, vsfSlNo, introw)
                    vsfExcel.SetData(introw, vsfItemCode, Replace(Replace(Replace(Trim(UCase(CType(.Range("A" & i).Value, String))), "'", "`"), vbCrLf, ""), vbCr, ""))
                    vsfExcel.SetData(introw, vsfRate, Replace(Replace(Replace(Trim(UCase(CType(.Range("B" & i).Value, Integer))), "'", "`"), vbCrLf, ""), vbCr, ""))
                    txtNoOfInv.Text = i
                    i = i + 1
                Loop
                StMsg("Data Loaded, now Validating the data")
                .Workbooks(1).Close(SaveChanges:=False)
            Catch ex As Exception
                MsgBox("Cannot export to Excel." & vbCrLf & ex.Message, MsgBoxStyle.Critical, "Import Error")
            End Try
        End With

        'Validating the data here - begins
        Call ValidateEntry()
        Call FillItemDtls()
        vsfExcel.AutoSizeCols(0, vsfExcel.Cols.Count - 1, 1)
        StMsg("")

        vsfExcel.Redraw = True

        Call wrapGrid()
        Exit Sub
    End Sub
    Private Sub FillItemDtls()
        Dim rs As oledb.oledbDataReader
        For i = 0 To vsfExcel.Rows.Count - 1
            If Val(vsfExcel.GetData(i, vsfItemID)) <> 0 Then
                rs = clssvt.getDataReaderFromSQL("Select ItemId ,ItemName,UOMCode from MstItem i,MstuOM u where i.UOMID=u.UOMID and ItemID='" & vsfExcel.GetData(i, vsfItemID) & "'")
                With rs
                    .Read()
                    vsfExcel.SetData(i, vsfItemName, GetVal(rs("ItemName"), ""))
                    vsfExcel.SetData(i, vsfUom, GetVal(rs("UOMCode"), ""))
                End With
                rs.Close()
            End If
        Next

    End Sub
    Private Function ValidateEntry() As Boolean
        Dim i As Integer, strerr As String = ""
        blnValidData = True
        lblError.Visible = False
        btnRemErr.Visible = False

        ValidateEntry = False
        blnValidData = True

        With vsfExcel
            If .Rows.Count = 1 Then
                MsgBox("There is no data exists for importing", vbInformation + vbOKOnly, Me.Text)
                Exit Function
            End If

            For i = 1 To .Rows.Count - 1
                .TopRow = i
                My.Application.DoEvents()
                strerr = "" 'Refreshing the err messg - each time
                vsfExcel.SetData(i, vExErr, strerr)
                vsfExcel.SetData(i, vExBlnErr, "")
                vsfExcel.SetData(i, vsfItemCode, Replace(vsfExcel.GetData(i, vsfItemCode), "'", "`"))
                lblStatus.Text = "Validating " & vsfExcel.GetData(i, vsfSlNo) & " -- " & i & " / " & txtNoOfInv.Text


                If Trim(vsfExcel.GetData(i, vsfItemCode)) = "" Then
                    strerr = strerr & IIf(i <> 1, vbLf, "") & " " & .GetData(0, vsfItemCode) & " is Empty "
                    .SetData(i, vExErr, strerr)
                    .SetData(i, vExBlnErr, "Y")
                    blnValidData = False
                    Dim cs As C1.Win.C1FlexGrid.CellStyle = vsfExcel.Styles.Add("ItemCode")
                    cs.BackColor = Color.Red

                    .SetCellStyle(i, vsfItemCode, cs)
                End If

                Dim intItemId As Integer
                If Trim(vsfExcel.GetData(i, vsfItemCode)) <> "" Then
                    intItemId = clssvt.getDataLongValue("Select ItemID from MstItem Where Itemcode='" & .GetData(i, vsfItemCode) & "'", 0, False)
                    If intItemId = 0 Then
                        strerr = strerr & IIf(i <> 1, vbLf, "") & " " & .GetData(0, vsfItemCode) & " Does not Exst "
                        .SetData(i, vExErr, strerr)
                        .SetData(i, vExBlnErr, "Y")
                        blnValidData = False
                        Dim cs As C1.Win.C1FlexGrid.CellStyle = vsfExcel.Styles.Add("ItemCode")
                        cs.BackColor = Color.Red
                        .SetCellStyle(i, vsfItemCode, cs)
                    Else
                        vsfExcel.SetData(i, vsfItemID, intItemId)
                    End If
                End If

            Next
            If clssvt.ChkDuplicateData(vsfExcel, i, vsfItemCode) Then
                Dim cs As C1.Win.C1FlexGrid.CellStyle = vsfExcel.Styles.Add("ItemCode")
                strerr = strerr & IIf(i <> 1, vbLf, "") & " " & .GetData(0, vsfItemCode) & " already exists "
                .SetData(i, vExErr, strerr)
                .SetData(i, vExBlnErr, "Y")
                cs.BackColor = Color.Red
                blnValidData = False
                vsfExcel.SetCellStyle(i, vsfItemCode, cs)
            End If

        End With
        lblStatus.Visible = False
        vsfExcel.Cols(vExErr).Visible = False
        vsfExcel.Cols(vExBlnErr).Visible = False

        If Not blnValidData Then
            vsfExcel.Cols(vExErr).Visible = True
            vsfExcel.Cols(vExBlnErr).Visible = False
            Call enablebtn(True)
            Exit Function
        End If
    End Function

    Private Function getItemPresent() As Boolean
        getItemPresent = False
        'Dim strSQL As String
        'Dim rs As oledb.oledbDataReader
        Dim strItemCode As String

        Dim strerr As String = ""
        Dim cs As C1.Win.C1FlexGrid.CellStyle = vsfExcel.Styles.Add("ItemCode")
        'strSQL = "select * from MstItem"

        'rs = clssvt.getDataReaderFromSQL(strSQL)

        'With rs
        '    Do While .Read
        For i = 1 To vsfExcel.Rows.Count - 1
            strItemCode = vsfExcel.GetData(i, vsfItemCode)
            'If !ItemCode = strItemCode Then
            If clssvt.getDataStringValue("Select ItemCode from MstItem where ItemCode='" & strItemCode & "'", "", False) <> "" Then
                cs.BackColor = Color.Red
                blnValidData = False
                strerr = strerr & IIf(i <> 1, vbLf, "") & " " & vsfExcel.GetData(0, vsfItemCode) & " already exists "
                vsfExcel.SetData(i, vExErr, strerr)
                vsfExcel.SetData(i, vExBlnErr, "Y")
                vsfExcel.SetCellStyle(i, vsfItemCode, cs)
                strerr = ""
            End If
        Next
        '    Loop
        'End With

        getItemPresent = True
    End Function

    Private Sub btnExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        Dim xl As New Excel.Application
        With xl
            .Workbooks.Add()
            .Range("A1").Value = "ItemCode"
            .Range("B1").Value = "Rate"

            .Visible = True
        End With
    End Sub

    Private Sub btnRemErr_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRemErr.Click
        Dim i As Integer, j As Integer
        If MsgBox("Do you want to Remove ALL Erroneous Records ?", vbYesNo + vbQuestion, Me.Text) = vbNo Then
            Exit Sub
        End If
        vsfTmp.Rows.Count = 1
        vsfTmp.Cols.Count = vsfExcel.Cols.Count
        For i = 0 To vsfExcel.Cols.Count - 1
            vsfTmp.SetData(0, i, vsfExcel.GetData(0, i))
        Next i
        With vsfExcel
Repeat:
            For i = 1 To (.Rows.Count - 1)
                If .GetData(i, vExBlnErr) = "Y" Then
                    vsfTmp.Rows.Count = vsfTmp.Rows.Count + 1
                    For j = 0 To vsfExcel.Cols.Count - 1
                        Dim cs As C1.Win.C1FlexGrid.CellStyle = vsfExcel.Styles.Add("AllColumns")
                        vsfTmp.SetData(vsfTmp.Rows.Count - 1, j, vsfExcel.GetData(i, j))
                        cs.BackColor = Color.White
                        vsfExcel.SetCellStyle(i, j, cs)
                    Next j
                    .RemoveItem(i)
                    GoTo Repeat
                End If
            Next i
            If vsfTmp.Rows.Count > 1 Then
                If MsgBox("Do you want to Export ALL Erroneous Records to XL?", vbYesNo + vbQuestion, Me.Text) = vbYes Then
                    Call ExportToXL(vsfTmp, "Errorneous Records")
                End If
            End If
        End With
        lblError.Visible = False
        Call EnableFields(True, 1)
    End Sub

    Private Sub btnImpClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImpClose.Click
        txtNoOfInv.Text = ""
        txtXLFilePath.Text = ""
        Dim tb As New C1.Win.C1Command.C1DockingTabPage()
        Dim dockTabMain As New C1.Win.C1Command.C1DockingTab()
        Me.Dispose()
        tb.Controls.Clear()
        'frmMain.dockTabMain.SelectedTab.Dispose()
        'frmMain.txtSearch.Focus()
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If vsfExcel.Rows.Count <= 1 Then
            MsgBox("No records found in the selected excel file!", vbOKOnly + vbInformation, Me.Text)
            Exit Sub
        End If

        For i = 1 To vsfExcel.Rows.Count - 1
            If vsfExcel.GetData(i, vExErr) <> "" Then
                MsgBox("Please Remove Errorneous Data!!", vbInformation, Me.Text)
                btnSave.Enabled = False
                Exit Sub
            End If
        Next i

        Call ReArrangeSlNo(vsfExcel, vsfSlNo)

        If ImportData() = True Then
            Me.Close()
        End If
    End Sub

    Private Function ImportData() As Boolean
        ImportData = False

        Dim strSQL As String = ""
        Try
            Dim intRow As Int32 = 1

            If optAppnd.Checked = False Then
                objFrm.vsfDet.Rows.Count = intRow
            Else
                intRow = objFrm.vsfDet.Rows.Count
            End If

            For i = 1 To vsfExcel.Rows.Count - 1
                objFrm.vsfDet.Rows.Count = intRow + 1
                objFrm.vsfDet.SetData(intRow, vsfSlNo, intRow - 1)
                For j = 1 To vsfExcel.Cols.Count - 2
                    If vsfExcel.GetData(0, j) = objFrm.vsfDet.GetData(0, j) Then
                        objFrm.vsfDet.SetData(intRow, j, vsfExcel.GetData(i, j))
                        'If objFrm.vsfDet.GetData(intRow, j).ToString = "" Then
                        '    objFrm.vsfDet.SetData(intRow, j, 0)
                        'End If
                    End If
                Next
                intRow = intRow + 1
            Next

            MsgBox(" Item Details Imported Successfully!", vbInformation, Me.Text)
            ImportData = True
        Catch ex As Exception
            ImportData = False
            MsgBox(ex.Message & vbCrLf & "Error in SQL: " & strSQL)
        End Try

    End Function

    Private Sub vsfExcel_BeforeEdit(sender As Object, e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfExcel.BeforeEdit
        e.Cancel = True
    End Sub

   
    Private Sub btnExportToXL_Click(sender As System.Object, e As System.EventArgs) Handles btnExportToXL.Click
        If vsfExcel.Row < 1 Then Exit Sub
        Call ExportToXL(vsfExcel)
    End Sub
End Class