Imports C1.Win.C1FlexGrid

Public Class frmUtlBarCodePrint

    Public strSQLline As String
    Dim clssvt As New svtLibrary
    'Dim FileObj As New FileSystemObject
    Dim svtTrn As OleDb.OleDbTransaction

    Public strList As String
    Public strItemList As String
    Public strFlds As String
    Public blnFromTrn As Boolean = False

    Public strAdnlFlds As String = ""
    Public strAdnlFldNo As String = ""


    Const vItemFldName = 0
    Const vItemDBfldName = 1

    Const vDetSlno = 0
    Const vDetSel = 1
    Const vDetNoofPrint = 2
    Const vDetItemID = 3
    Const vDetItemCode = 4
    Const vDetItemName = 5
    Const vDetItemBarCode = 6
    Const vDetItemUOM = 7
    Const vDetItemQty = 8
    Const vDetItemRate = 9
    Const vDetItemTOtCols = 10
    Private Sub frmUtlBarCodePrint_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Me.KeyPreview = True
        fillFormProperties(Me)
        initScreen()
        initGridStyles(Me)
    End Sub

    Private Sub initScreen()

        Dim i As Integer, strSql As String
        Dim rs As OleDb.OleDbDataReader

        With vsfItems

            .Rows.Count = 1
            .Cols.Count = 2
            .Cols.Fixed = 1

            .SetData(0, vItemFldName, "Rpt Field Name")
            .SetData(0, vItemDBfldName, "DB Field Name")
            .Cols(vItemDBfldName).ComboList = getBCList()

            For c As Int32 = 1 To 50
                .Rows.Count = .Rows.Count + 1
                .SetData(.Rows.Count - 1, 0, "Fld - " & c)
            Next c
            .Cols(0).TextAlign = TextAlignEnum.CenterCenter
        End With

        With vsfDet
            .Cols.Count = vDetItemTOtCols
            .Rows.Count = 1
            .Cols.Fixed = 1
        End With

        SetHideCol(vsfDet, True, vDetItemID, vDetSlno)

        Call initFormVsfDet(Me, Me.Text, vsfDet, "DET")
        Call initFormVsfDet(Me, Me.Text, vsfItems, "ITEMS")

        Dim intColNo As Integer = 0

        With vsfDet
            .Cols.Count = vDetItemTOtCols
            .Rows.Count = 1
            .Cols.Fixed = 1
            .SetData(0, vDetSlno, "SlNo")
            .SetData(0, vDetSel, "Sel")
            .SetData(0, vDetNoofPrint, "No of Labels")
            .SetData(0, vDetItemID, "ItemID")
            .SetData(0, vDetItemCode, "Item Code")
            .SetData(0, vDetItemName, "Item Name")
            .SetData(0, vDetItemBarCode, "Barcode")
            .SetData(0, vDetItemQty, "Qty")
            .SetData(0, vDetItemUOM, "UOM")
            .SetData(0, vDetItemRate, "MRP")

            .Cols(vDetSel).DataType = GetType(Boolean)
            .Cols(vDetNoofPrint).TextAlign = TextAlignEnum.LeftCenter

        End With


        strSql = "Select FLDNO,FLDNAME from RptBCAdnlflds"
        rs = clssvt.getDataReaderFromSQL(strSql)
        If rs.HasRows Then
            Do While rs.Read
                strAdnlFlds = strAdnlFlds & "," & rs!FldName
                strAdnlFldNo = strAdnlFldNo & "," & rs!FldNo

            Loop
        Else
            rs.Close()
            Exit Sub
        End If
        rs.Close()

        Dim strCols As String
        strCols = Strings.Right(strAdnlFlds, Len(strAdnlFlds) - 1)
        intColNo = 1

        With vsfDet
            For Each Flds As String In strCols.Split(",")
                .Cols.Count = .Cols.Count + 1
                .SetData(0, .Cols.Count - 1, "Fld - " & intColNo & " (" & Flds & ")")

                vsfItems.SetData(intColNo, vItemDBfldName, Flds)

                intColNo = intColNo + 1
            Next
        End With

    End Sub


    Private Sub btnPrint_Click(sender As Object, e As EventArgs) Handles btnPrint.Click
        Dim i As Integer, strToReplace As String, strReplaceWith As String, j As Integer
        Dim strSql As String, strOtherFlds As String
        Dim intCtr As Integer

        svtTrn = svtConn.BeginTransaction(IsolationLevel.ReadCommitted)

        If gDBTypeCurrent = "ACCESS" Then
            If Not clssvt.execSQLStatement("Delete * from RptBC", svtTrn) Then
                svtTrn.Rollback()
                Exit Sub
            End If

        Else
            If Not clssvt.execSQLStatement("Truncate table RptBC", svtTrn) Then
                svtTrn.Rollback()
                Exit Sub
            End If

        End If
        intCtr = 0

        With vsfDet
            For i = 1 To .Rows.Count - 1
                If .GetCellCheck(i, vDetSel) = CheckEnum.Checked Then
                    For j = 1 To Val(.GetData(i, vDetNoofPrint))
                        intCtr = intCtr + 1

                        strSql = "Insert into RptBC (Slno,USERNAME, BCNO, ItemCode,ItemName, Qty, UOM,MRP,"


                        If strAdnlFldNo <> "" Then
                            strOtherFlds = Strings.Right(strAdnlFldNo, Len(strAdnlFldNo) - 1)
                            strOtherFlds = Replace(strOtherFlds, " ", "")
                            strOtherFlds = Replace(strOtherFlds, "-", "")

                            strSql &= "" & strOtherFlds & ""

                        Else
                            strSql = Strings.Left(strSql, Len(strSql) - 1)
                        End If

                        strSql &= ") values ("
                        strSql = strSql & intCtr & ","
                        strSql = strSql & "'" & gUserCode & "',"
                        strSql = strSql & "'" & .GetData(i, vDetItemBarCode) & "',"
                        strSql = strSql & "'" & .GetData(i, vDetItemCode) & "',"
                        strSql = strSql & "'" & .GetData(i, vDetItemName) & "',"
                        strSql = strSql & Val(.GetData(i, vDetItemQty)) & ","
                        strSql = strSql & "'" & .GetData(i, vDetItemUOM) & "',"
                        strSql = strSql & "" & Val(.GetData(i, vDetItemRate)) & ""

                        If strAdnlFldNo <> "" Then
                            For a As Int32 = vDetItemTOtCols To .Cols.Count - 1
                                strSql &= ",'" & .GetData(i, a) & "'"
                            Next

                        End If

                        'strSql = Strings.Left(strSql, Len(strSql) - 1)

                        strSql &= ")"

                        If Not clssvt.execSQLStatement(strSql, svtTrn) Then
                            svtTrn.Rollback()
                            Exit Sub
                        End If
                    Next j
                End If
            Next i
        End With

        svtTrn.Commit()


        Try
            'For i = 1 To vsfDet.Rows.Count - 1
            ' If vsfDet.GetCellCheck(i, 1) = CheckEnum.Checked Then

            Dim strFormulas(,) As String
            'Dim strSF As String = "{rptbc.bcno}= '" & vsfDet.GetData(i, vDetItemBarCode) & "'"
            Dim strSF As String = "{rptbc.UserName}= '" & gUserCode & "'"
            'ReDim strFormulas(0, 1)

            Dim frmv As New frmUtlReportViewer
            With frmv
                .StartPosition = FormStartPosition.CenterScreen
                .showRptFile("Barcode Print", "RPTBC.rpt", strSF, strFormulas, False)
                .Show()
            End With

            '     End If
            ' Next


        Catch ex As Exception
        End Try
    End Sub

    Private Sub vsfDet_BeforeEdit(sender As Object, e As RowColEventArgs) Handles vsfDet.BeforeEdit
        Dim i As Integer
        With vsfDet
            'If e.Col = 1 Then
            '    For i = 1 To .Rows.Count - 1
            '        If Order = 1 Then
            '            .SetCellCheck(i, 1, CheckEnum.Checked)
            '        Else
            '            .SetCellCheck(i, 1, CheckEnum.Unchecked)
            '        End If
            '    Next
            'End If
        End With
    End Sub

    Private Sub btnLoadItems_Click(sender As Object, e As EventArgs) Handles btnLoadItems.Click
        Dim intColNo As Integer = 0

        With vsfDet
            .Cols.Count = vDetItemTOtCols
            .Rows.Count = 1
            .Cols.Fixed = 1
            .SetData(0, vDetSlno, "SlNo")
            .SetData(0, vDetSel, "Sel")
            .SetData(0, vDetNoofPrint, "No of Times to Print")
            .SetData(0, vDetItemID, "ItemID")
            .SetData(0, vDetItemCode, "Item Code")
            .SetData(0, vDetItemName, "Item Name")
            .SetData(0, vDetItemBarCode, "Barcode")
            .SetData(0, vDetItemQty, "Qty")
            .SetData(0, vDetItemUOM, "UOM")
            .SetData(0, vDetItemRate, "MRP")

            With vsfItems
                intColNo = 0
                For c As Int32 = 1 To .Rows.Count - 1
                    If .GetData(c, vItemDBfldName) <> "" Then
                        intColNo = intColNo + 1
                        vsfDet.Cols.Count = vsfDet.Cols.Count + 1
                        vsfDet.SetData(0, vsfDet.Cols.Count - 1, "Fld - " & intColNo & " (" & .GetData(c, vItemDBfldName) & ")")
                    End If
                Next c
            End With

            '.Cols(c).TextAlign = TextAlignEnum.CenterCenter
            .Cols(vDetSel).DataType = GetType(Boolean)
            .Cols(vDetNoofPrint).TextAlign = TextAlignEnum.LeftCenter

            '.Cols(5).TextAlign = TextAlignEnum.LeftCenter
        End With

    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        frmMain.CloseSelectedTabPage(Me)
    End Sub

    Private Sub frmUtlBarCodePrint_KeyUp(sender As Object, e As KeyEventArgs) Handles Me.KeyUp
        If e.KeyCode = Keys.F9 And e.Control = True Then
            Call initFormVsfDet(Me, Me.Text, vsfDet, "DET")
            Call initFormVsfDet(Me, Me.Text, vsfItems, "ITEMS")
            MsgBox("Restored Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If

        If e.KeyCode = Keys.F8 And e.Control = True Then
            Call SaveActFormSettings(Me, Me.Text)
            SaveActFormVsfDet(Me, Me.Text, "DET", vsfDet)
            SaveActFormVsfDet(Me, Me.Text, "ITEMS", vsfItems)
            MsgBox("Saved Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
    End Sub

    Private Sub vsfItems_BeforeEdit(sender As Object, e As RowColEventArgs) Handles vsfItems.BeforeEdit

    End Sub

    Private Sub btnSelItem_Click(sender As Object, e As EventArgs) Handles btnSelItem.Click
        Dim strSQL As String

        strList = ""
        strItemList = ""

        Dim frmSel As New frmUtlMultiSelection

        strSQL = "Select distinct 0 as sel, ItemID, ItemCode, ItemName,u.UOMCODE"
        strSQL = strSQL & " From MstItem i, MstUom u"
        strSQL = strSQL & " Where i.InActive = 'N' and ItemID > 0"
        strSQL = strSQL & " and i.UOMID=u.UOMID"
        strSQL = strSQL & " order by Itemcode"
        Call frmSel.InitVsfSel(strSQL, 1, 1, True, Me, False)
        frmSel.Text = "Select [Multiple] Items"
        frmSel.vsfDet.SetData(0, 0, "Sel")
        frmSel.ShowDialog()
        strItemList = strList

        If strItemList <> "" Then
            loadItems(strItemList)
        End If
    End Sub

    Public Sub loadItems(argItemList As String, Optional ByVal argPBQty As Integer = 1, Optional ByVal argPBID As Integer = 0, Optional ByVal argDispFlds As String = "")
        If argItemList = "" Then Exit Sub

        Dim strSql As String, strAdnlFlds As String
        Dim rs As OleDb.OleDbDataReader

        strSql = "Select FLDNAME from RptBCAdnlflds"
        rs = clssvt.getDataReaderFromSQL(strSql)
        If rs.HasRows Then
            Do While rs.Read
                strAdnlFlds = strAdnlFlds & "," & rs!FldName
            Loop
        End If
        rs.Close()

        strSql = "select ItemId,ItemCode as ItemC,ItemName as ItemN,ItemCode as ItemBC,"
        strSql &= " 1 as BatchQty,(Select UOMCODE from MstUOM where UOMID=b.UOMID) as UOM,isnull(MRP,0) as MRP"
        strSql &= "" & strAdnlFlds
        strSql &= " from MstItem b"
        strSql &= " where ItemID in (" & argItemList & ")"
        rs = clssvt.getDataReaderFromSQL(strSql)
        If Not rs.HasRows Then
            MsgBox("No records found!", MsgBoxStyle.Information, Me.Text)
            Exit Sub
        End If

        Dim intcol As Integer = vDetItemQty + 1

        With vsfDet
            .Rows.Count = 1
            Do While rs.Read
                .Rows.Count = .Rows.Count + 1
                .SetData(.Rows.Count - 1, vDetSlno, .Rows.Count - 1)
                .SetCellCheck(.Rows.Count - 1, vDetSel, CheckEnum.Checked)

                .SetData(.Rows.Count - 1, vDetItemQty, "1")

                .SetData(.Rows.Count - 1, vDetNoofPrint, "1")

                .SetData(.Rows.Count - 1, vDetItemID, GetVal(rs!ItemID, 0))
                .SetData(.Rows.Count - 1, vDetItemCode, GetVal(rs!ItemC, ""))
                .SetData(.Rows.Count - 1, vDetItemName, GetVal(rs!ItemN, ""))
                .SetData(.Rows.Count - 1, vDetItemBarCode, GetVal(rs!ItemBC, ""))
                .SetData(.Rows.Count - 1, vDetItemUOM, GetVal(rs!UOM, ""))
                .SetData(.Rows.Count - 1, vDetItemRate, GetVal(rs!MRP, 0))

                intcol = vDetItemTOtCols
                If strFlds <> "" Then
                    strFlds = Strings.Right(strAdnlFlds, Len(strAdnlFlds) - 1)

                    If strFlds <> "" Then
                        For Each Flds As String In strFlds.Split(",")
                            .SetData(.Rows.Count - 1, intcol, rs(Flds))
                            intcol = intcol + 1
                        Next
                    End If
                End If
            Loop
        End With
    End Sub

    Private Sub vsfItems_KeyUp(sender As Object, e As KeyEventArgs) Handles vsfItems.KeyUp
        If e.KeyCode = Keys.F9 And e.Control = True Then
            Call initFormVsfDet(Me, Me.Text, vsfItems, "ITEMS")
            MsgBox("Restored Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If

        If e.KeyCode = Keys.F8 And e.Control = True Then
            SaveActFormVsfDet(Me, Me.Text, "ITEMS", vsfItems)
            MsgBox("Saved Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
    End Sub

    Private Sub vsfDet_KeyUp(sender As Object, e As KeyEventArgs) Handles vsfDet.KeyUp
        If e.KeyCode = Keys.F9 And e.Control = True Then
            Call initFormVsfDet(Me, Me.Text, vsfDet, "DET")
            MsgBox("Restored Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If

        If e.KeyCode = Keys.F8 And e.Control = True Then
            SaveActFormVsfDet(Me, Me.Text, "DET", vsfDet)
            MsgBox("Saved Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
    End Sub

    Public Sub initDetGrid()
        Dim intColNo As Integer

        With vsfDet
            .Cols.Count = vDetItemTOtCols
            .Rows.Count = 1
            .Cols.Fixed = 1
            .SetData(0, vDetSlno, "SlNo")
            .SetData(0, vDetSel, "Sel")
            .SetData(0, vDetNoofPrint, "No of Times to Print")
            .SetData(0, vDetItemID, "ItemID")
            .SetData(0, vDetItemCode, "Item Code")
            .SetData(0, vDetItemName, "Item Name")
            .SetData(0, vDetItemBarCode, "Barcode")
            .SetData(0, vDetItemUOM, "UOM")
            .SetData(0, vDetItemQty, "Qty")
            .SetData(0, vDetItemRate, "MRP")

            With vsfItems
                intColNo = 0
                For c As Int32 = 1 To .Rows.Count - 1
                    If .GetData(c, vItemDBfldName) <> "" Then
                        intColNo = intColNo + 1
                        vsfDet.Cols.Count = vsfDet.Cols.Count + 1
                        vsfDet.SetData(0, vsfDet.Cols.Count - 1, "Fld - " & intColNo & " (" & .GetData(c, vItemDBfldName) & ")")
                    End If
                Next c
            End With

            .Cols(vDetSel).DataType = GetType(Boolean)
            .Cols(vDetNoofPrint).TextAlign = TextAlignEnum.LeftCenter

        End With
    End Sub

    Private Function getBCList() As String
        Dim rsBC As OleDb.OleDbDataReader
        Dim strFld As String = "|"

        rsBC = clssvt.getDataReaderFromSQL("Select TOP 1 * from MstItem")

        For i As Int32 = 0 To rsBC.FieldCount - 1
            strFld = strFld & rsBC.GetName(i) & "|"
        Next
        rsBC.Close()

        getBCList = strFld
    End Function

    Private Sub btnSavefldMatrix_Click(sender As Object, e As EventArgs) Handles btnSavefldMatrix.Click
        Dim blnFnd As Boolean = False
        Dim strSql As String, strFldNo As String

        With vsfItems
            For i As Int32 = 1 To .Rows.Count - 1
                If .GetData(i, vItemDBfldName) <> "" Then
                    blnFnd = True
                End If
            Next
            If Not blnFnd Then Exit Sub

            If gDBTypeCurrent = "ACCESS" Then
                clssvt.execSQLStatement("Delete * from RptBCAdnlFlds")
            Else
                clssvt.execSQLStatement("Truncate table RptBCAdnlFlds")
            End If

            strFldNo = ""

            For i As Int32 = 1 To .Rows.Count - 1
                If .GetData(i, vItemDBfldName) <> "" Then
                    strSql = "insert into RptBCAdnlFlds(FLDNO,FLDNAME)"
                    strFldNo = Replace(.GetData(i, vItemFldName), "-", "")
                    strFldNo = Replace(strFldNo, " ", "")

                    strSql &= " Values('" & strFldNo & "','" & .GetData(i, vItemDBfldName) & "')"

                    clssvt.execSQLStatement(strSql)
                End If
            Next

        End With

        btnLoadItems_Click(Nothing, Nothing)
    End Sub
End Class