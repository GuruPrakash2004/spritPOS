Imports Microsoft.Office.Interop
Imports C1.Win.C1FlexGrid
Imports System.IO

Public Class frmUpdateItemStock
    Dim clsSvt As New svtLibrary

    Const viSlno = 0
    Const viItemCode = 1
    Const viItemName = 2
    Const viUOM = 3
    Const viStock = 4
    Const viStockRate = 5
    Const viStockAmt = 6
    Const viError = 7
    Const viErrorDesc = 8

    Private Sub frmImportItemXLData_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Text = "Import Item Stock"
        With vsfExcel
            .Rows.Count = 1
            .Rows.Fixed = 1
            .Cols.Count = viErrorDesc + 1
            .Cols.Fixed = 1
        End With
        With vsfExcel
            .SetData(0, viSlno, "Slno")
            .SetData(0, viItemCode, "ItemCode")
            .SetData(0, viItemName, "ItemName")
            .SetData(0, viUOM, "UOM")
            .SetData(0, viStock, "Stock")
            .SetData(0, viStockRate, "Rate")
            .SetData(0, viStockAmt, "Amount")
            .SetData(0, viError, "Error")
            .SetData(0, viErrorDesc, "ErrorDesc")
        End With

        Call enablebtn(False)
        Call EnableFields(False, 1)
        Call initGridStyles(Me)
        btnRemErr.Enabled = False
        btnExportToXL.Enabled = False
    End Sub

    Private Sub btnExport_Click(sender As Object, e As EventArgs) Handles btnExport.Click
        Dim intRow As Integer
        If Not File.Exists(gstrRptPath & "StockImport_Template.xlsx") Then
            MsgBox("File " & gstrRptPath & "StockImport_Template.xlsx does not exist!", MsgBoxStyle.Critical, Me.Text)
            Exit Sub
        End If
        Dim rsItem As oledb.oledbDataReader
        Dim xlApp As New Excel.Application
        xlApp.Workbooks.Open(gstrRptPath & "StockImport_Template.xlsx",, True)
        rsItem = clsSvt.getDataReaderFromSQL("Select ItemCode, Itemname, UOMCode from MstItem i, MstUOM u where i.UOMID = u.UOMID and i.itemid > 0 order by 1 ")
        With rsItem
            intRow = 2
            Do While .Read
                Application.DoEvents()
                StMsg("Loading item " & !ItemCode)
                intRow = intRow + 1
                xlApp.Range("A" & intRow).Value = !ItemCode
                xlApp.Range("B" & intRow).Value = !Itemname
                xlApp.Range("C" & intRow).Value = !UOMcode
            Loop
            .Close()
        End With
        StMsg("")
        xlApp.Visible = True
    End Sub

    Private Sub btnImportFromXL_Click(sender As Object, e As EventArgs) Handles btnImportFromXL.Click

        If Trim(txtXLFilePath.Text) = "" Then
            MsgBox("Please select excel file path..", MsgBoxStyle.Information + vbOKOnly, Me.Text)
            Exit Sub
        End If

        If Dir(Trim(txtXLFilePath.Text)) = "" Then
            MsgBox("File not found", MsgBoxStyle.Information + vbOKOnly, Me.Text)
            Exit Sub
        End If

        btnSave.Enabled = False
        Call ImportItem()

        With vsfExcel
            Dim blnError As Boolean = False
            For i As Int32 = 1 To .Rows.Count - 1
                If Trim(.GetData(i, viError)) = "Y" Then
                    blnError = True
                    Exit For
                End If
            Next
            btnSave.Enabled = Not blnError
        End With
    End Sub

    Private Sub ImportItem()
        With vsfExcel
            .Rows.Count = 1
            .Cols.Count = viErrorDesc + 1
        End With

        Dim xlapp As New Excel.Application
        Dim i As Integer, str As String = ""

        Dim introw As Integer, intcol As Integer
        xlapp.Workbooks.Open(Trim(txtXLFilePath.Text), , True)

        Dim strXLColName As String = ""
        Dim strXLTempColName As String = "ITEMCODE,ITEMNAME,UOMCode"

        With xlapp.Application
            intcol = 3
            For i = 1 To intcol
                strXLColName &= UCase(Trim(xlapp.ActiveSheet.Range(Chr(64 + i) & 2).Value)) & ","
            Next i

            strXLColName = Mid(strXLColName, 1, Len(strXLColName) - 1)

            If UCase(Trim(strXLColName)) <> UCase(Trim(strXLTempColName)) Then
                MsgBox("Invalid Template...", MsgBoxStyle.Critical, Me.Text)
                Exit Sub
            End If

            i = 3 'ASSIGNING THE FIRST ROW
            Try
                Do While Trim(.Range("A" & i).Value) <> ""

                    vsfExcel.Rows.Count = vsfExcel.Rows.Count + 1
                    introw = vsfExcel.Rows.Count - 1

                    Application.DoEvents()
                    vsfExcel.TopRow = introw
                    StMsg("Importing row " & introw)

                    vsfExcel.SetData(introw, viSlno, introw)
                    vsfExcel.SetData(introw, viItemCode, Trim(xlapp.ActiveSheet.Range("A" & i).Value))
                    vsfExcel.SetData(introw, viItemName, Trim(xlapp.ActiveSheet.Range("B" & i).Value))
                    vsfExcel.SetData(introw, viUOM, Trim(xlapp.ActiveSheet.Range("C" & i).Value))
                    vsfExcel.SetData(introw, viStock, Val(xlapp.ActiveSheet.Range("D" & i).Value))
                    vsfExcel.SetData(introw, viStockRate, Val(xlapp.ActiveSheet.Range("E" & i).Value))
                    txtNoOfInv.Text = i - 1
                    i = i + 1
                Loop
                StMsg("Data Loaded, now Validating the data")
                .Workbooks(1).Close(SaveChanges:=False)
            Catch ex As Exception
                MsgBox("Cannot export to Excel." & vbCrLf & ex.Message, MsgBoxStyle.Critical, "Import Error")
            End Try
        End With

        With vsfExcel
            .SetData(0, viErrorDesc, "Errors")
            .SetData(0, viError, "BlnFound")
            Call SetHideCol(vsfExcel, True, viError, viErrorDesc)
        End With

        'Validating the data here - begins
        Call ValidateItemEntry()
        vsfExcel.AutoSizeCols(0, vsfExcel.Cols.Count - 1, 1)
        StMsg("")
        vsfExcel.Redraw = True

        For i = 1 To vsfExcel.Rows.Count - 1
            If GetVal(vsfExcel.GetData(i, viError), "") = "Y" Then
                vsfExcel.Cols(viErrorDesc).Visible = True
            End If
        Next

        'Validating whether the row count is > O if yes the buttons are enabled 
        If vsfExcel.Rows.Count > 0 Then
            btnRemErr.Enabled = True
            btnExportToXL.Enabled = True
        End If
    End Sub

    Private Sub ValidateItemEntry()
        Dim i As Integer, strerr As String = "", blnfound As Boolean, blnErr As Boolean
        With vsfExcel
            If .Rows.Count = 1 Then
                MsgBox("There is no data for importing", vbInformation + vbOKOnly, Me.Text)
                Exit Sub
            End If

            For i = 1 To .Rows.Count - 1
                Application.DoEvents()
                .TopRow = i
                StMsg("Validating row " & i)
                blnErr = False
                strerr = ""
                .SetData(i, viErrorDesc, "")
                .SetData(i, viError, "")
                .SetData(i, viItemCode, Replace(.GetData(i, viItemCode), "'", "`"))

                lblStatus.Text = "Validating " & vsfExcel.GetData(i, viSlno) & " -- " & i & " / " & txtNoOfInv.Text

                clsSvt.getDataLongValue("Select ItemID from MstItem Where Itemcode='" & Trim(.GetData(i, viItemCode)) & "'", 0, blnfound)

                If Not blnfound Then
                    blnErr = True
                    strerr = "Item not found"
                    Dim cs As C1.Win.C1FlexGrid.CellStyle = vsfExcel.Styles.Add("ItemCode")
                    cs.BackColor = Color.Red
                    vsfExcel.SetCellStyle(i, viItemCode, cs)
                End If

                If Val(.GetData(i, viStock)) < 0 Then
                    blnErr = True
                    strerr = IIf(strerr <> "", strerr & vbCrLf, "") & "Invalid Stock"

                    Dim cs As C1.Win.C1FlexGrid.CellStyle = vsfExcel.Styles.Add("ItemCode")
                    cs.BackColor = Color.Red
                    vsfExcel.SetCellStyle(i, viStock, cs)
                End If

                Try
                    vsfExcel.SetData(i, viStockAmt, Val(vsfExcel.GetData(i, viStock)) * GetVal(vsfExcel.GetData(i, viStockRate), 0))
                Catch ex As Exception

                End Try

                If blnErr Then
                    .SetData(i, viErrorDesc, strerr)
                    .SetData(i, viError, "Y")
                End If
            Next i
            .AllowResizing = AllowResizingEnum.Both
            .AutoSizeCol(viErrorDesc)
        End With
    End Sub

    Private Sub btnBrowse_Click(sender As Object, e As EventArgs) Handles btnBrowse.Click
        cDLG.DefaultExt = "xlsx"
        cDLG.Filter = "Excel File (*.xlsx)|*.xlsx"
        cDLG.FileName = "*.xlsx"

        If cDLG.ShowDialog() <> DialogResult.OK Then
            Return
            Exit Sub
        End If
        txtXLFilePath.Text = cDLG.FileName
        Call EnableFields(False, 1)
        btnImportFromXL.Enabled = True

    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        If vsfExcel.Rows.Count <= 1 Then
            MsgBox("No records found in the selected excel file!", vbOKOnly + vbInformation, Me.Text)
            Exit Sub
        End If
        Call SAveStock()
    End Sub

    Private Sub SAveStock()
        Dim strSQL As String = "", intPBID As Long, intSuppID As Integer
        Dim strTNo As String, sngCases As Single
        'Dim sngPurPrice As Single
        Dim dblPurPrice As Double
        intSuppID = clsSvt.getDataLongValue("Select SuppID from MstSupp where suppCode = 'KSBCL'", 0, False)
        strTNo = TrnNoAutoGen("TrnHdrPB", "PBNO", "OP", getSystemDate, 1276)
        strTNo = getFmtDocNo("OP", strTNo, getSystemDate)
        strSQL = "insert into trnhdrpb "
        strSQL &= " (PBNO,Prefix,PrintPBNO,PBDate,SUPPID, "
        strSQL &= " PartyName,PartyAdd1,PartyAdd2,PartyAdd3,PartyAdd4,PartyCity,PartyState,PartyPinCode, "
        strSQL &= " PartyCountry,PartyCName,PartyCMob,PartyCEmail,PartyCDesig,Authorised,AuthBy,AuthOn, "
        strSQL &= " LOCID,ChangedBy,ChangedOn,Cancelled,AddedBy,AddedOn,AMTINWRDS,RMKS,SUPPINVNO, "
        strSQL &= " SUPPINVDATE,SUPPINVVALUE,TOTAMT,OTHAMT,GRNDTOT,TOTCGST,TOTSGST,TOTIGST,PURCHASETYPE) values ("
        strSQL &= "'" & strTNo & "',"
        strSQL &= "'OP',"
        strSQL &= "'" & strTNo & "',"
        strSQL &= "'" & getDateFormatInInsert(getSystemDate) & "',"
        strSQL &= intSuppID & ",'-','-','-','-','-','-','-','-','-','-','-','-','-','Y','ADMIN',"
        strSQL &= "'" & getDateFormatInInsert(getSystemDate) & "',1,'',Null,'N','ADMIN',"
        strSQL &= "'" & getDateFormatInInsert(getSystemDate) & "',"
        strSQL &= "'','','',Null,0,0,0,0,0,0,0,'')"
        Call clsSvt.execSQLStatement(strSQL)

        intPBID = clsSvt.getDataLongValue("Select max(PBID) from TrnHdrPB", 0, False)

        With vsfExcel
            For i As Int32 = 1 To .Rows.Count - 1
                If GetVal(.GetData(i, viError)) <> "Y" And Val(GetVal(.GetData(i, viStock), 0)) > 0 Then
                    strSQL = "Insert into TrnDtlPB (PBID, SlNo, ITEMID,UOMID,ItemSlNO,StkQty,PBQty,PBRate,PBAmt,NOOFCASES,NOOFBOTTLES,"
                    strSQL &= "CGSTPER,CGSTAMT,SGSTPER,SGSTAMT,IGSTPER,IGSTAMT,NETAMT,MRP,PBQtyInLtrs) values ("
                    strSQL &= intPBID & ","
                    strSQL &= i & ","
                    strSQL &= clsSvt.getDataLongValue("Select ItemID From MstItem Where ItemCode = '" & GetVal(.GetData(i, viItemCode), "") & "'", 0, False) & ","
                    strSQL &= "" & clsSvt.getDataLongValue("Select UOMID From MstUOM Where UOMCode = '" & GetVal(.GetData(i, viUOM), "") & "'", 0, False) & ","
                    strSQL &= i & ","
                    strSQL &= Val(GetVal(.GetData(i, viStock), 0)) & ","
                    strSQL &= Val(GetVal(.GetData(i, viStock), 0)) & ","
                    If chkRate.Checked AndAlso GetVal(.GetData(i, viStockRate), 0) > 0 Then
                        dblPurPrice = GetVal(.GetData(i, viStockRate), 0)
                    Else
                        dblPurPrice = clsSvt.getDataDoubleValue("Select PurRate From MstItem Where ItemCode = '" & GetVal(.GetData(i, viItemCode), "") & "'", 0, False)
                    End If

                    strSQL &= dblPurPrice & ","
                    strSQL &= dblPurPrice * Val(GetVal(.GetData(i, viStock), 0)) & ","
                    sngCases = clsSvt.getDataLongValue("Select BotPerCases From MstItem Where ItemCode = '" & GetVal(.GetData(i, viItemCode), "") & "'", 0, False)
                    If sngCases > 0 Then
                        strSQL &= Math.Round(Val(GetVal(.GetData(i, viStock), 0)) / sngCases, 2) & ","
                    Else
                        strSQL &= "0,"
                    End If
                    strSQL &= dblPurPrice * Val(GetVal(.GetData(i, viStock), 0)) & ","
                    strSQL &= "0,0,0,0,0,0,"
                    strSQL &= dblPurPrice * Val(GetVal(.GetData(i, viStock), 0)) & ","
                    strSQL &= clsSvt.getDataDoubleValue("Select MRP From MstItem Where ItemCode = '" & GetVal(.GetData(i, viItemCode), "") & "'", 0, False) & ","
                    sngCases = clsSvt.getDataLongValue("Select LtrPerBot From MstPkg p, MstItem i Where i.PkgID = i.PkgID and ItemCode = '" & GetVal(.GetData(i, viItemCode), "") & "'", 0, False)
                    strSQL &= Math.Round(Val(GetVal(.GetData(i, viStock), 0)) * sngCases, 2) & ")"
                    Call clsSvt.execSQLStatement(strSQL)
                End If
            Next

            MsgBox("Imported Data Successfully!", vbInformation, Me.Text)
            Call closefrom()
        End With
    End Sub

    Private Sub btnRemErr_Click(sender As Object, e As EventArgs) Handles btnRemErr.Click
        Dim i As Integer, j As Integer

        If MsgBox("Do you want to Remove ALL Erroneous Records ?", vbYesNo + vbQuestion, Me.Text) = vbNo Then
            Exit Sub
        End If
        With vsfExcel
Repeat:
            For i = 1 To (.Rows.Count - 1)
                If .GetData(i, viError) = "Y" Then
                    .RemoveItem(i)
                    GoTo Repeat
                End If
            Next i
        End With

        btnSave.Enabled = True
    End Sub

    Private Sub btnExportToXL_Click(sender As Object, e As EventArgs) Handles btnExportToXL.Click
        Call ExportToXL(vsfExcel)
    End Sub

    Private Sub btnImpClose_Click(sender As Object, e As EventArgs) Handles btnImpClose.Click
        txtNoOfInv.Text = ""
        txtXLFilePath.Text = ""
        Call frmMain.CloseSelectedTabPage(Me)
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
    Private Sub enablebtn(ByVal argBLN As Boolean)
        ' lblError.Visible = argBLN
        btnRemErr.Visible = True
        'optAppnd.Visible = argBLN
        'optDel.Visible = argBLN
    End Sub
    'Once Data is imported the from is closed
    Private Sub closefrom()
        txtNoOfInv.Text = ""
        txtXLFilePath.Text = ""
        Call frmMain.CloseSelectedTabPage(Me)
    End Sub


End Class