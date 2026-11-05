Imports Microsoft.Office.Interop
Imports C1.Win.C1FlexGrid
Imports System.IO

Public Class frmImportItemXLData
    Dim clsSvt As New svtLibrary

    Const viSlno = 0
    Const viItemCode = 1
    Const viItemName = 2
    Const viShortName = 3
    Const viGroup = 4
    Const viBrand = 5
    Const viLiqType = 6
    Const viUOM = 7
    Const viPkgItemName = 8
    Const viPkgCode = 9
    Const viBotPerCase = 10
    Const viMRP = 11
    Const viPurRate = 12
    Const viCGSTPer = 13
    Const viSGSTPer = 14
    Const viROL = 15
    Const viROQ = 16
    Const viMaxLevel = 17
    Const viMinLevel = 18
    Const viHSNCode = 19
    Const viNonStkable = 20
    'Added by SUrabhi
    Const viCessper = 21
    Const viCompCess = 22
    Const viError = 23
    Const viErrorDesc = 24

    ' Const viError = 20
    'Const viErrorDesc = 21
    Private Sub frmImportItemXLData_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Text = "Import Item Master"
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
            .SetData(0, viShortName, "Short Name")
            .SetData(0, viGroup, "ItemGroup")
            .SetData(0, viLiqType, "Liq Category")
            .SetData(0, viUOM, "UOM")
            .SetData(0, viPkgItemName, "Pkg.ItemName")
            .SetData(0, viPkgCode, "Pkg.Type")
            .SetData(0, viBotPerCase, "Bots.PerCase")
            .SetData(0, viMRP, "MRP")
            .SetData(0, viPurRate, "Purchase Rate")
            .SetData(0, viBrand, "Brand")
            .SetData(0, viCGSTPer, "Cgst%")
            .SetData(0, viSGSTPer, "Sgst%")
            .SetData(0, viROL, "ROL")
            .SetData(0, viROQ, "ROQ")
            .SetData(0, viMaxLevel, "MaxLevel")
            .SetData(0, viMinLevel, "MinLevel")
            .SetData(0, viHSNCode, "HSNcode")
            .SetData(0, viNonStkable, "Non-Stockable")
            'ADded by Surabhi  CessPer and CompCess
            .SetData(0, viCessper, "CessPer")
            .SetData(0, viCompCess, "CompCess")
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
        If Not File.Exists(gstrRptPath & "ItemImport_Template.xlsx") Then
            MsgBox("File " & gstrRptPath & "ItemImport_Template.xlsx does not exist!", MsgBoxStyle.Critical, Me.Text)
            Exit Sub
        End If

        Dim p As New System.Diagnostics.Process
        Dim s As New System.Diagnostics.ProcessStartInfo(gstrRptPath & "ItemImport_Template.xlsx")
        s.UseShellExecute = True
        s.WindowStyle = ProcessWindowStyle.Maximized
        p.StartInfo = s
        p.Start()
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
        Dim strXLTempColName As String = "ITEMCODE,ITEMNAME,SHORTNAME,ITEMGROUPCode,LIQTYPE,UOMCode"

        With xlapp.Application
            intcol = 6
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
                    vsfExcel.SetData(introw, viShortName, Trim(xlapp.ActiveSheet.Range("C" & i).Value))
                    vsfExcel.SetData(introw, viGroup, Trim(xlapp.ActiveSheet.Range("D" & i).Value))
                    vsfExcel.SetData(introw, viLiqType, Trim(xlapp.ActiveSheet.Range("E" & i).Value))
                    vsfExcel.SetData(introw, viUOM, Trim(xlapp.ActiveSheet.Range("F" & i).Value))
                    vsfExcel.SetData(introw, viPkgItemName, Trim(xlapp.ActiveSheet.Range("G" & i).Value))
                    vsfExcel.SetData(introw, viPkgCode, Trim(xlapp.ActiveSheet.Range("H" & i).Value))
                    vsfExcel.SetData(introw, viBotPerCase, Val(xlapp.ActiveSheet.Range("I" & i).Value))
                    vsfExcel.SetData(introw, viMRP, Val(xlapp.ActiveSheet.Range("J" & i).Value))
                    vsfExcel.SetData(introw, viPurRate, Val(xlapp.ActiveSheet.Range("K" & i).Value))
                    vsfExcel.SetData(introw, viCGSTPer, Val(xlapp.ActiveSheet.Range("L" & i).Value))
                    vsfExcel.SetData(introw, viSGSTPer, Val(xlapp.ActiveSheet.Range("M" & i).Value))
                    vsfExcel.SetData(introw, viROL, Val(xlapp.ActiveSheet.Range("N" & i).Value))
                    vsfExcel.SetData(introw, viROQ, Val(xlapp.ActiveSheet.Range("O" & i).Value))
                    vsfExcel.SetData(introw, viMaxLevel, Val(xlapp.ActiveSheet.Range("P" & i).Value))
                    vsfExcel.SetData(introw, viMinLevel, Val(xlapp.ActiveSheet.Range("Q" & i).Value))
                    vsfExcel.SetData(introw, viHSNCode, Trim(xlapp.ActiveSheet.Range("R" & i).Value))
                    vsfExcel.SetData(introw, viNonStkable, Trim(xlapp.ActiveSheet.Range("S" & i).Value))
                    'ADded by Surabhi
                    vsfExcel.SetData(introw, viCessper, Trim(xlapp.ActiveSheet.Range("T" & i).Value))
                    vsfExcel.SetData(introw, viCompCess, Trim(xlapp.ActiveSheet.Range("U" & i).Value))
                    vsfExcel.SetData(introw, viBrand, Trim(xlapp.ActiveSheet.Range("V" & i).Value))
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

                If blnfound Then
                    blnErr = True
                    strerr = "Item Already Present"
                    Dim cs As C1.Win.C1FlexGrid.CellStyle = vsfExcel.Styles.Add("ItemCode")
                    cs.BackColor = Color.Red
                    vsfExcel.SetCellStyle(i, viItemCode, cs)
                End If

                If Trim(.GetData(i, viGroup)) = "" Then .SetData(i, viGroup, "-")
                clsSvt.getDataLongValue("Select ItemGroupID from MstItemGroup Where ItemGroupCode='" & Trim(.GetData(i, viGroup)) & "'", 0, blnfound)
                If Not blnfound Then
                    blnErr = True
                    strerr = IIf(strerr <> "", strerr & vbCrLf, "") & "Invalid Group"

                    Dim cs As C1.Win.C1FlexGrid.CellStyle = vsfExcel.Styles.Add("ItemCode")
                    cs.BackColor = Color.Red
                    vsfExcel.SetCellStyle(i, viGroup, cs)
                End If

                clsSvt.getDataLongValue("Select UOMID from MstUOM Where UOMCode='" & Trim(.GetData(i, viUOM)) & "'", 0, blnfound)

                If Not blnfound Then
                    blnErr = True
                    strerr = IIf(strerr <> "", strerr & vbCrLf, "") & "Invalid Group"
                    Dim cs As C1.Win.C1FlexGrid.CellStyle = vsfExcel.Styles.Add("ItemCode")
                    cs.BackColor = Color.Red
                    vsfExcel.SetCellStyle(i, viUOM, cs)
                End If

                If Trim(.GetData(i, viNonStkable)) = "" Then .SetData(i, viNonStkable, "N")

                If Trim(.GetData(i, viPkgCode)) = "" Then .SetData(i, viPkgCode, "-")
                clsSvt.getDataLongValue("Select PkgID from MstPkg Where PkgCode='" & Trim(.GetData(i, viPkgCode)) & "'", 0, blnfound)

                If Not blnfound Then
                    blnErr = True
                    strerr = IIf(strerr <> "", strerr & vbCrLf, "") & "Invalid Package Type"

                    Dim cs As C1.Win.C1FlexGrid.CellStyle = vsfExcel.Styles.Add("ItemCode")
                    cs.BackColor = Color.Red
                    vsfExcel.SetCellStyle(i, viPkgCode, cs)
                End If

                If Trim(.GetData(i, viBrand)) = "" Then .SetData(i, viBrand, "-")
                clsSvt.getDataLongValue("Select BrandID from MstBrand Where BrandCode='" & Trim(.GetData(i, viBrand)) & "'", 0, blnfound)

                If Not blnfound Then
                    blnErr = True
                    strerr = IIf(strerr <> "", strerr & vbCrLf, "") & "Invalid Brand"

                    Dim cs As C1.Win.C1FlexGrid.CellStyle = vsfExcel.Styles.Add("ItemCode")
                    cs.BackColor = Color.Red
                    vsfExcel.SetCellStyle(i, viBrand, cs)
                End If

                Select Case UCase(Trim(.GetData(i, viLiqType)))
                    Case "", "BEER", "WINE", "IML", "OTHERS"
                    Case Else
                        blnErr = True
                        strerr = IIf(strerr <> "", strerr & vbCrLf, "") & "Invalid Liq.Type"

                        Dim cs As C1.Win.C1FlexGrid.CellStyle = vsfExcel.Styles.Add("ItemCode")
                        cs.BackColor = Color.Red
                        vsfExcel.SetCellStyle(i, viLiqType, cs)
                End Select

                If Val(.GetData(i, viMRP)) <= 0 Then
                    blnErr = True
                    strerr = IIf(strerr <> "", strerr & vbCrLf, "") & "Invalid MRP"

                    Dim cs As C1.Win.C1FlexGrid.CellStyle = vsfExcel.Styles.Add("ItemCode")
                    cs.BackColor = Color.Red
                    vsfExcel.SetCellStyle(i, viMRP, cs)
                End If
                If blnErr Then
                    .SetData(i, viErrorDesc, strerr)
                    .SetData(i, viError, "Y")
                End If
            Next i

            If clsSvt.ChkDuplicateData(vsfExcel, i, viItemCode) Then
                Dim cs As C1.Win.C1FlexGrid.CellStyle = vsfExcel.Styles.Add("ItemCode")
                .SetData(i, viErrorDesc, IIf(.GetData(i, viErrorDesc) <> "", .GetData(i, viErrorDesc) & vbCrLf, "") & "Duplicate Data")
                .SetData(i, viError, "Y")
                cs.BackColor = Color.Red
                vsfExcel.SetCellStyle(i, viItemCode, cs)
            End If
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
        Call SAveItemMaster()
    End Sub

    Private Sub SAveItemMaster()
        Dim strSQL As String = ""
        Dim sngSellPrice As Single
        With vsfExcel

            Dim blnCessPer As Boolean = False
            Dim blnCompCess As Boolean = False

            If fnFldPresent("CESSPER", "MstItem") Then
                blnCessPer = True
            End If
            If fnFldPresent("COMPCESS", "MstItem") Then
                blnCompCess = True
            End If

            For i As Int32 = 1 To .Rows.Count - 1
                If GetVal(.GetData(i, viError)) <> "Y" Then

                    sngSellPrice = Math.Round(Val(.GetData(i, viMRP)) / (Val(.GetData(i, viCGSTPer)) + Val(.GetData(i, viSGSTPer)) + 100) * 100, 2)

                    strSQL = "Insert into MstItem"
                    strSQL &= " (ITEMID,ITEMCODE,ITEMNAME,ShortName,HotKey,HSNCode,"
                    strSQL &= " ITEMGroupID, LiqType, NonStkable, InActive, InActiveRmks, UOMID,"
                    strSQL &= " AddedBy, AddedOn, ChangedBy, ChangedOn, MINLEVEL, CGSTPER,"
                    strSQL &= " SGSTPER, BotPerCases, PKGITEM, PKGID, ITEMSELL, BrandID, MRP,  "
                    If blnCessPer Then strSQL &= "Cessper,"
                    If blnCompCess Then strSQL &= "COMPCESS,"
                    strSQL &= " PURRATE, ROL, ROQ, MAXLEVEL) values "
                    strSQL &= " ((Select Max(ItemID) + 1 from MstItem), "
                    strSQL &= "'" & GetVal(.GetData(i, viItemCode), "") & "',"
                    strSQL &= "'" & Replace(GetVal(.GetData(i, viItemName), ""), "'", "`") & "',"
                    strSQL &= "'" & Replace(GetVal(.GetData(i, viShortName), ""), "'", "`") & "',"
                    strSQL &= "''," 'HotKey
                    strSQL &= "'" & Replace(GetVal(.GetData(i, viHSNCode), ""), "'", "`") & "',"
                    strSQL &= "" & clsSvt.getDataLongValue("Select ItemGroupID From MstItemGroup Where ItemGroupCode = '" & GetVal(.GetData(i, viGroup), "") & "'", 0, False) & ","
                    strSQL &= "'" & GetVal(.GetData(i, viLiqType), "") & "',"
                    strSQL &= "'" & GetVal(.GetData(i, viNonStkable), "") & "',"
                    strSQL &= "'N','',"
                    strSQL &= "" & clsSvt.getDataLongValue("Select UOMID From MstUOM Where UOMCode = '" & GetVal(.GetData(i, viUOM), "") & "'", 0, False) & ","
                    strSQL &= "'Admin',getdate(),'',Null,"
                    strSQL &= Val(GetVal(.GetData(i, viMinLevel), 0)) & ","
                    strSQL &= Val(GetVal(.GetData(i, viCGSTPer), 0)) & ","
                    strSQL &= Val(GetVal(.GetData(i, viSGSTPer), 0)) & ","
                    strSQL &= Val(GetVal(.GetData(i, viBotPerCase), 0)) & ","
                    strSQL &= "'" & GetVal(.GetData(i, viPkgItemName), "") & "',"
                    strSQL &= "" & clsSvt.getDataLongValue("Select PkgID From MstPkg Where PkgCode = '" & GetVal(.GetData(i, viPkgCode), "") & "'", 0, False) & ","
                    strSQL &= sngSellPrice & ","
                    strSQL &= "" & clsSvt.getDataLongValue("Select BrandID From MstBrand Where BrandCode = '" & GetVal(.GetData(i, viBrand), "") & "'", 0, False) & ","
                    strSQL &= Val(GetVal(.GetData(i, viMRP), 0)) & ","
                    If blnCessPer Then strSQL &= Val(GetVal(.GetData(i, viCessper), 0)) & ","
                    If blnCompCess Then strSQL &= Val(GetVal(.GetData(i, viCompCess), 0)) & ","
                    strSQL &= Val(GetVal(.GetData(i, viPurRate), 0)) & ","
                    strSQL &= Val(GetVal(.GetData(i, viROL), 0)) & ","
                    strSQL &= Val(GetVal(.GetData(i, viROQ), 0)) & ","
                    strSQL &= Val(GetVal(.GetData(i, viMaxLevel), 0)) & ")"

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