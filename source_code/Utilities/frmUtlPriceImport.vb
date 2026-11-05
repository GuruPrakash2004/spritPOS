Imports Microsoft.Office.Interop
Public Class frmUtlPriceImport
    Dim intRow%
    Dim strErrorMsg As String
    Dim clsSvt As New svtLibrary
    Dim intDocID As Integer

    Const vDetSlno = 0
    Const vDetItemID = 1
    Const vDetItemCode = 2
    Const vDetItemName = 3
    Const vDetPriceRate = 4
    Const vDetRemarks = 5
    Const vDetErr = 6
    Const vDetBlnErr = 7

    Const vDetTotCols = 8

    Public strPCatName As String
    Dim intPCatID As String

    Private Sub frmUtlPriceImport_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Call initscreen()
    End Sub

    Private Sub initscreen()
        Dim red As C1.Win.C1FlexGrid.CellStyle
        Dim black As C1.Win.C1FlexGrid.CellStyle

        With vsfDet
            .Cols.Count = vDetTotCols
            .Rows.Count = 1

            .SetData(0, vDetSlno, "Slno")
            .SetData(0, vDetItemID, "Item ID")
            .SetData(0, vDetItemCode, "Item Code")
            .SetData(0, vDetItemName, "Item Name")
            .SetData(0, vDetPriceRate, "Rate")
            .SetData(0, vDetRemarks, "Remarks")
            .SetData(0, vDetErr, "Error Data")
            .SetData(0, vDetBlnErr, "Error Found")

            .Cols(vDetItemName).TextAlignFixed = C1.Win.C1FlexGrid.TextAlignEnum.LeftCenter
            '.Cols(vDetSlno).TextAlignFixed = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter
            '.Cols(vDetPriceRate).TextAlignFixed = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter
            '.Cols(vDetErr).TextAlignFixed = C1.Win.C1FlexGrid.TextAlignEnum.LeftCenter

            .Cols(vDetPriceRate).DataType = GetType(Decimal)
            .ExtendLastCol = True

            Call SetColAlign(vsfDet, svtAlign.Left, vDetItemCode, vDetItemName, vDetRemarks)
            Call SetColAlign(vsfDet, svtAlign.Right, vDetSlno, vDetPriceRate)
        End With
        Call SetFlexColor(vsfDet, "", vDetSlno, vDetItemID, vDetItemCode, vDetItemName, vDetPriceRate, vDetRemarks, vDetErr, vDetBlnErr)
        red = vsfDet.Styles.Add("Red")
        red.ForeColor = Color.Red

        black = vsfDet.Styles.Add("Black")
        black.ForeColor = Color.Black

        Call SetHideCol(vsfDet, True, vDetItemID, vDetBlnErr)
        btnSave.Enabled = False

        txtPCat.Text = strPCatName
        intPCatID = clsSvt.getDataLongValue("select PriceCatID from MstPriceCat where PriceCatCode = '" & txtPCat.Text & "'", 0, False)

        Call initGridStyles(Me)
    End Sub

    Private Sub btnBrowse_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowse.Click
        ' choose file
        Dim dlg As New OpenFileDialog()
        dlg.DefaultExt = "xlsx"
        dlg.Filter = "Excel File (*.xlsx)|*.xlsx"
        dlg.FileName = "*.xlsx"
        If dlg.ShowDialog() <> DialogResult.OK Then
            Return
            Exit Sub
        End If

        txtXLFilePath.Text = dlg.FileName
    End Sub

    Private Sub btnImportFromXL_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImportFromXL.Click
        'On Error GoTo Err

        Dim i As Integer, blnErrFound As Boolean
        Dim introw As Integer, intItemId As Long, strs As String
        Dim xlApp As New Excel.Application

        If Trim(txtXLFilePath.Text) = "" Then
            MsgBox("Please Select file for import", MsgBoxStyle.Information, Me.Text)
            Exit Sub
        End If

        btnSave.Enabled = False
        vsfDet.Rows.Count = 1
        xlApp.Workbooks.Open(txtXLFilePath.Text, , True)

        blnErrFound = False
        Try
            With xlApp.Application
                'vsfDet.ScrollBars = ScrollBars.Vertical
                strs = Trim(UCase(.Range("B1").Value.ToString)) & ","
                strs = strs & Trim(UCase(CType(.Range("C1").Value, String))) & ","
                strs = strs & Trim(UCase(CType(.Range("D1").Value, String))) & ","
                strs = strs & Trim(UCase(CType(.Range("E1").Value, String)))

                If Trim(UCase(strs)) <> UCase(Trim("Item Code,Item Name,Price Rate,Remarks")) Then
                    MsgBox("Invalid file selected, please select the right file/refer template for correct column format!", vbInformation + vbOKOnly, Me.Text)
                    chkInvalidData.Checked = True

                End If

                i = 2

                vsfDet.Redraw = False

                Do While Trim(.Range("B" & i).Value) <> ""
                    introw = vsfDet.Rows.Count
                    vsfDet.Rows.Count = vsfDet.Rows.Count + 1

                    vsfDet.SetData(introw, vDetSlno, introw)
                    vsfDet.SetData(introw, vDetItemCode, Trim(UCase(CType(.Range("B" & i).Value, String))))
                    vsfDet.SetData(introw, vDetItemName, Trim(UCase(CType(.Range("C" & i).Value, String))))
                    vsfDet.SetData(introw, vDetPriceRate, If(CType((.Range("D" & i).Value), String) = "" Or IsNumeric(CType((.Range("D" & i).Value), String)) = False, "0.00", CType((.Range("D" & i).Value), Integer)))
                    vsfDet.SetData(introw, vDetRemarks, Trim(UCase(CType(.Range("E" & i).Value, String))))

                    intItemId = clsSvt.getDataLongValue("select ItemId from MstItem where ItemCode  = '" & Trim(UCase(CType(.Range("B" & i).Value, String))) & "'", 0, False)

                    vsfDet.SetData(introw, vDetItemID, intItemId)

                    i += 1
                Loop

                vsfDet.AutoSizeCols(0, 1, 1)
            End With
            vsfDet.Redraw = True

            Call ValidateEntry()
            vsfDet.AutoSizeCols(0, 1, 1)
            Call vsfDet.AutoSizeCols()
            xlApp.Workbooks(1).Close(False)
            xlApp = Nothing
            Exit Sub
        Catch ex As Exception
        End Try
        'Err:
        '            MsgBox(Err.Description, MsgBoxStyle.Information, Me.Text)

    End Sub

    Private Sub ValidateEntry()
        Dim i As Integer, j As Integer, strerr As String
        Dim strItemName As String, strCItemName As String
        Dim blnErrFound As Boolean, blnItemFound As Boolean

        chkInvalidData.Checked = False
        blnErrFound = False

        With vsfDet
            If .Rows.Count = 1 Then
                MsgBox("Please enter valid data in the excel sheet (without leaving empty lines) before importing", vbInformation + vbOKOnly, Me.Text)
                Exit Sub
            End If

            For i = 1 To .Rows.Count - 1
                .SetData(i, vDetErr, "")
                .SetData(i, vDetBlnErr, "N")
                .SetCellStyle(i, vDetItemName, "Black")
            Next

            For i = 1 To .Rows.Count - 1
                .TopRow = i
                strerr = ""
                strItemName = .GetData(i, vDetItemName)

                For j = i + 1 To .Rows.Count - 1
                    strCItemName = .GetData(j, vDetItemName)
                    If strItemName = strCItemName Then
                        strerr = "Duplicate Item!"
                        .SetData(i, vDetErr, strerr)
                        .SetData(j, vDetErr, strerr)
                        .SetData(i, vDetBlnErr, "Y")
                        chkInvalidData.Checked = True
                        .SetData(i, vDetBlnErr, "Y")

                        blnErrFound = True
                        .SetCellStyle(i, vDetItemName, "Red")
                        .SetCellStyle(j, vDetItemName, "Red")
                        Exit For
                    End If
                Next j

                blnItemFound = False
                Call clsSvt.getDataLongValue("Select ItemID from MstItem where ItemName = '" & strItemName & "'", 0, blnItemFound)

                If Not blnItemFound Then
                    strerr = "Item " & strItemName & " not found!"
                    .SetData(i, vDetErr, strerr)
                    .SetData(i, vDetBlnErr, "Y")
                    chkInvalidData.Checked = True
                    blnErrFound = True
                    .SetCellStyle(i, vDetItemName, "Red")
                End If

                blnErrFound = False
                If Val(.GetData(i, vDetPriceRate)) <= 0 Then
                    strerr = "Price rate cannot be empty!"
                    .SetData(i, vDetErr, strerr)
                    .SetData(i, vDetBlnErr, "Y")
                    chkInvalidData.Checked = True
                    blnErrFound = True
                    .SetCellStyle(i, vDetItemName, "Red")
                End If

                chkInvalidData.Checked = False

                If Trim(.GetData(i, vDetBlnErr)) = "" Then
                    .SetData(i, vDetBlnErr, "N")
                End If
            Next i

            If blnErrFound Then
                btnSave.Enabled = True
            End If

            txtNoOfRec.Text = vsfDet.Rows.Count - 1
        End With
    End Sub

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub btnTemplate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTemplate.Click
        Dim xl As New Excel.Application
        With xl
            .Workbooks.Add()
            .Range("A1").Value = "Sl No."
            .Range("B1").Value = "Item Code"
            .Range("C1").Value = "Item Name"
            .Range("D1").Value = "Price Rate"
            .Range("E1").Value = "Remarks"
            .Visible = True
        End With
    End Sub

    Private Sub vsfDet_BeforeEdit(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfDet.BeforeEdit
        e.Cancel = True
    End Sub

    Private Sub vsfDet_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles vsfDet.Click

    End Sub

    Private Sub btnRemErr_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRemErr.Click
        Dim i As Integer

GoToBigen:
        With vsfDet
            For i = 1 To .Rows.Count - 1
                If .GetData(i, vDetBlnErr) = "Y" Then
                    .RemoveItem(i)
                    GoTo GoToBigen
                End If
            Next i
        End With

        Call ReArrangeSlNo(vsfDet, vDetSlno)
        Call ValidateEntry()

        If chkInvalidData.Checked = False Then
            btnSave.Enabled = True
        End If
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim strSQL As String, i As Integer, blnFound As Boolean

        intPCatID = clsSvt.getDataLongValue("select PriceCatID from MstPriceCat where PriceCatCode = '" & txtPCat.Text & "'", 0, blnFound)

        If Not blnFound Then
            MsgBox("Price Category not found", MsgBoxStyle.Information, Me.Text)
            Exit Sub
        End If

        'strSQL = "delete from MstPriceCatDtl where PriceCatID = " & intPCatID
        'clsSvt.execSQLStatement(strSQL)

        With vsfDet
            For i = 1 To .Rows.Count - 1
                strSQL = "delete from MstPriceCatDtl where PriceCatID = " & intPCatID
                strSQL = strSQL & " and ItemID = " & Val(.GetData(i, vDetItemID))
                Call clsSvt.execSQLStatement(strSQL)

                strSQL = "insert into MstPriceCatDtl (PriceCatID, ItemId, PriceCatRate, Remarks)"
                strSQL = strSQL & " values (" & intPCatID & ","
                strSQL = strSQL & Val(.GetData(i, vDetItemID)) & ","
                strSQL = strSQL & Val(.GetData(i, vDetPriceRate)) & ","
                strSQL = strSQL & "'" & .GetData(i, vDetRemarks) & "'"
                strSQL = strSQL & ")"
                Call clsSvt.execSQLStatement(strSQL)
            Next
        End With

        MsgBox("Data Imported", MsgBoxStyle.Information, Me.Text)
        Me.Close()
    End Sub

    Private Sub btnExportToXL_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExportToXL.Click
        Call ExportToXL(vsfDet)
    End Sub
End Class