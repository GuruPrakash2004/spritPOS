Imports System.ComponentModel
Imports System.IO
Imports C1.Win.C1FlexGrid

Public Class frmMstPriceMaintenance
    Dim clsSVT As New svtLibrary

    Dim strRights As String
    Dim intMenuID As Integer, intDocID As Integer
    Dim strItemGrp As String

    Dim svtTrn As oledb.oledbTransaction

    'Const for Item Grp
    Const vItmGrpSel As Byte = 0
    Const vItmGrpID As Byte = 1
    Const vItmGrpCode As Byte = 2
    Const vItmGrpName As Byte = 3
    Const vItmGrpSName As Byte = 4
    Const vItmGrpHotKey As Byte = 5
    'Const for Item Grp details
    Const vItmGrpDetID As Byte = 0
    Const vItmGrpDetGrpName As Byte = 1
    Const vItmGrpDetItmID As Byte = 2
    Const vItmGrpDetSel As Byte = 3
    Const vItmGrpDetItmCode As Byte = 4
    Const vItmGrpDetItmName As Byte = 5
    'Const for Item Col
    Const vItmColSel As Byte = 0
    Const vItmColCode As Byte = 1
    Const vItmColName As Byte = 2

    Const iStdCols As Byte = 6

    Dim argItmFldCode As String 'Stores the selected Item FldCode
    'Dim blnItmColSel As Boolean
    Dim lngRecCnt As Long
    Dim fso As File

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Call frmMain.CloseSelectedTabPage(Me)
    End Sub

    Private Sub btnBack_Click(sender As Object, e As EventArgs) Handles btnBack.Click
        lblFrm.Text = "Selection : " & Me.Text
        Call EnableSelFldScreen(True)
    End Sub

    Private Sub EnableSelFldScreen(argBln As Boolean)
        pnlSelFlds.Visible = argBln
        If argBln Then
            pnlSelFlds.BringToFront()
        Else
            pnlSelFlds.SendToBack()
        End If
        pnlMain.Visible = Not argBln
        If argBln Then
            pnlMain.BringToFront()
        Else
            pnlMain.SendToBack()
        End If
    End Sub

    Private Sub btnMainClose_Click(sender As Object, e As EventArgs)
        Call frmMain.CloseSelectedTabPage(Me)
    End Sub

    Private Sub btnApply_Click(sender As Object, e As EventArgs) Handles btnApply.Click
        Dim i As Integer, j As Integer
        StMsg("Applying......")

        'CHK rights
        If Not chkUserRights(strRights, chkRightsPos.AddRights, Me.Text) Then
            Exit Sub
        End If
        With vsfItemGroupDet
            If .Rows.Count = 1 Then
                MsgBox("No Items present in this Item Group.Cannot continue", vbInformation + vbOKOnly, Me.Text)
                vsfItemGroup.Focus()
                Exit Sub
            End If
            If .FindRow(True, 1, vItmGrpDetSel, False) < 0 Then
                MsgBox("Please select atleast one Item to update Rate to new values", vbInformation + vbOKOnly, Me.Text)
                .Focus()
                Exit Sub
            End If
        End With

        With vsfItemCol
            If .FindRow(True, 1, vItmColSel, False) < 0 Then
                MsgBox("Please select atleast one Rate columns to update to new values", vbInformation + vbOKOnly, Me.Text)
                .Focus()
                Exit Sub
            End If
        End With

        If Not CheckforNegative() Then
            MsgBox("Rates cannot go Negative.", vbExclamation, Me.Text)
            Exit Sub
        End If
        'continue the upd process
        'Call UpdateVal(False)

        'update the val
        With vsfItemGroupDet
            For i = 1 To .Rows.Count - 1
                If .GetCellCheck(i, vItmGrpDetSel) = CheckEnum.Checked Then
                    For j = iStdCols To .Cols.Count - 1 Step 2
                        If optInc.Checked Then
                            If optAmt.Checked = True Then 'Amt
                                .SetData(i, j + 1, svtModule.SVTROUND(Val(.GetData(i, j)) + Val(txtAmt.Text), 2))
                            Else 'Per
                                .SetData(i, j + 1, svtModule.SVTROUND(Val(.GetData(i, j)) + (Val(.GetData(i, j)) * Val(txtAmt.Text) / 100), 2))
                                If cboRound.Text = "50p RoundOff" Then
                                    .SetData(i, j + 1, ROUND50(Val(.GetData(i, j + 1))))
                                End If
                                If cboRound.Text = "1Rs RoundOff" Then
                                    .SetData(i, j + 1, ROUND100(Val(.GetData(i, j + 1))))
                                End If
                            End If
                        Else
                            If optAmt.Checked = True Then 'Amt
                                .SetData(i, j + 1, svtModule.SVTROUND(Val(.GetData(i, j)) - Val(txtAmt.Text), 2))
                            Else 'Per
                                .SetData(i, j + 1, svtModule.SVTROUND(Val(.GetData(i, j)) - (Val(.GetData(i, j)) * Val(txtAmt.Text) / 100), 2))
                                If cboRound.Text = "50p RoundOff" Then
                                    .SetData(i, j + 1, ROUND50(Val(.GetData(i, j + 1))))
                                End If
                                If cboRound.Text = "1Rs RoundOff" Then
                                    .SetData(i, j + 1, ROUND100(Val(.GetData(i, j + 1))))
                                End If

                            End If
                        End If
                    Next j
                End If
            Next i
        End With
        'txtAmt.Text = ""
        StMsg("Done...")
        Call EnableFlds(2, True)
        MsgBox("Price alterations applied successfully!!!" & vbLf + vbCrLf & "Please click on 'Update to DB' to update the altered prices to Database", vbInformation + vbOKOnly, Me.Text)
        btnUpdtToDB.Focus()
        Call EnableFlds(4, False)
    End Sub

    Private Sub EnableFlds(argCase As Integer, argBln As Boolean)
        Select Case argCase
            Case 1
                btnLoad.Enabled = argBln
            Case 2
                btnUpdtToDB.Enabled = argBln
            Case 3
                lblRecCnt.Visible = argBln
            Case 4
                btnApply.Enabled = argBln
            'cmdReplace.Enabled = argBln
            Case 5
                btnReplace.Visible = argBln
        End Select
    End Sub

    Private Sub btnUpdtToDB_Click(sender As Object, e As EventArgs) Handles btnUpdtToDB.Click

        Dim strsql As String, i As Integer, j As Integer
        Dim strFldCode As String

        'CHK rights
        If Not chkUserRights(strRights, chkRightsPos.AddRights, Me.Text) Then
            Exit Sub
        End If
        Try
            If Not fnValidateUpd() Then Exit Sub
            If MsgBox("You are about to update rate for " & lngRecCnt & " Item(s) ." &
            vbLf + vbCrLf & "If you click Yes all existing rates will be over-written and cannot be reverted back!!!" &
            vbLf + vbCrLf & "Are you sure you want to continue?", vbCritical + vbYesNo, Me.Text) = vbYes Then

                '            If MsgBox("Do you want to take a Database BACK-UP now?", vbInformation + vbYesNo, Me.Caption) = vbYes Then
                '                'Write code to take DB Back-up in SQL and ACCESS
                '                FraMain.Enabled = False
                '                fraBackup.Visible = True
                '                fraBackup.ZOrder (vbBringToFront)
                '                Exit Sub
                '            Else
                '                FraMain.Enabled = True
                '                fraBackup.Visible = False
                '                fraBackup.ZOrder (vbSendToBack)
                '            End If

#Region "Satish BackUP To be taken Care"
                'Call BackUpDB()
#End Region

                svtTrn = svtConn.BeginTransaction(IsolationLevel.ReadCommitted)
                With vsfItemGroupDet
                    For i = 1 To .Rows.Count - 1
                        If .GetCellCheck(i, vItmGrpDetSel) = CheckEnum.Checked Then
                            For j = iStdCols To .Cols.Count - 1 Step 2
                                strFldCode = Trim(.GetData(0, j))
                                strsql = "update MstItem set " & strFldCode & " = " & Val(.GetData(i, j + 1))
                                strsql = strsql & " where ItemID = " & Val(.GetData(i, vItmGrpDetItmID))
                                clsSVT.execSQLStatement(strsql, svtTrn)
                            Next j
                        End If
                    Next i
                End With
                svtTrn.Commit()
                MsgBox("Altered Price values updated to database successfully", vbInformation + vbOKOnly, Me.Text)
                'End If
                Call UpdateVal(False)
            End If
        Catch ex As Exception
            MsgBox(ex.Message, vbInformation + vbOKOnly, Me.Text)
            svtTrn.Rollback()
        End Try
        Call EnableFlds(2, False)
    End Sub

    Private Function fnValidateUpd() As Boolean
        Dim i As Integer, j As Integer
        fnValidateUpd = False
        With vsfItemGroupDet
            If .Rows.Count = 1 Then
                MsgBox("No Items present in this Item Group.Cannot continue", vbInformation + vbOKOnly, Me.Text)
                vsfItemGroup.Focus()
                Exit Function
            End If
            If .FindRow(True, 1, vItmGrpDetSel, False) < 0 Then
                MsgBox("Please select atleast one Item to update Rate to new values", vbInformation + vbOKOnly, Me.Text)
                .Focus()
                Exit Function
            End If
            For i = 1 To .Rows.Count - 1
                If .GetCellCheck(i, vItmGrpDetSel) = CheckEnum.Checked Then
                    For j = iStdCols + 1 To .Cols.Count - 1 Step 2
                        If .GetData(0, j).ToString.ToUpper = "MRP-NEW" Then
                            If Val(.GetData(i, j)) <= 0 Then
                                MsgBox("Price cannot be negative or zero. Please enter a valid Price", vbInformation + vbOKOnly, Me.Text)
                                Call EnableFlds(2, False)
                                Exit Function
                            End If
                        End If
                    Next j
                End If
            Next i
        End With
        fnValidateUpd = True
    End Function

    Private Sub BackUpDB()
        Dim FileName As String, str As String
        Dim FolderFound As Boolean
        Dim strBkUPFile As String
        Try

            strBkUPFile = gstrDataPath
            If Trim(strBkUPFile) = "" Then Exit Sub
            FileName = strBkUPFile
            If gDBTypeCurrent = gDbTypeAccess Then
                If Path.GetExtension(FileName) <> "mdb" Then
                    str = Path.GetExtension(FileName)
                    FileName = Replace(FileName, str, "")
                End If
                If Path.GetExtension(FileName) = "" Then
                    FileName = FileName & ".mdb"
                End If
                strBkUPFile = FileName
                str = Path.GetDirectoryName(FileName)
#Region "Satish : to check for drive name"
                'FolderFound = fso.DriveExists("" & str & "")
                'If FolderFound = True Then
                '    str = fso.GetParentFolderName(FileName)
                '    FolderFound = fso.FolderExists(str)
                '    If FolderFound = False Then
                '        fso.CreateFolder(str)
                '    End If
                'Else
                '    MsgBox("Drive Name   " & File(FileName) & "\ does  not Exist", vbInformation, "vLQMS")
                '    Exit Sub
                'End If
#End Region

                '        If Chkzip.value = vbChecked Then
                '            FileName = App.Path & "\WINZIP32 "
                '            Shell FileName & " " & gstrDataPath & gSettings.gMDBName, vbMaximizedFocus
                '            strBkUpFile = ""
                '            Exit Sub
                '        End If

                FolderFound = File.Exists(FileName)
                '        If FolderFound = True Then
                '            If MsgBox("Want to Overwrite  ?", vbYesNo + vbDefaultButton2 _
                '                        + vbInformation, "Facct") = vbNo Then
                '                txtBackupFile.SetFocus
                '                Exit Sub
                '            End If
                '        End If
                File.Copy(gstrDataPath & gSettings.gMDBName, FileName, True)
            Else 'SQL back - up
                strBkUPFile = Trim(Replace(UCase(Trim(strBkUPFile)), ".MDB", ".DAT"))
                FileName = Trim(strBkUPFile)

                'Create a logical backup device for the full MyNwind backup.
                clsSVT.execSQLStatement("USE master")
                clsSVT.execSQLStatement("EXEC sp_addumpdevice 'disk', '" & gStrDBName & "_1'," &
            "'" & Trim(strBkUPFile) & gStrDBName & ".dat'")

                'Back up the full MyNwind database.
                clsSVT.execSQLStatement("BACKUP DATABASE " & gStrDBName & " TO " & gStrDBName & "_" & getSystemDateTime().ToString.Replace("/", "").Replace(":", "").Replace(" ", ""))
            End If
            'stMsg " Data Backedup to... " & FileName, ""

            Exit Sub
        Catch ex As Exception
            MsgBox(ex.Message, vbInformation, Me.Text)
        End Try
    End Sub

    Private Sub UpdateVal(argCalRate As Boolean)
        Dim strsql As String, iColCnt As Integer, j As Integer, BlnFldCode As Boolean, i As Integer
        Dim recdummy As oledb.oledbDataReader
        Dim arrItmCol() As String

        Try
            argItmFldCode = ""
            iColCnt = 0
            'To see if any Col is selected and Initialise the sel Itm Grp
            With vsfItemCol
                If .FindRow(True, 1, vItmColSel, False) > 0 Then
                    'blnItmColSel = True
                    For i = 1 To .Rows.Count - 1
                        If .GetCellCheck(i, vItmColSel) = CheckEnum.Checked Then
                            'iColCnt = iColCnt + 1
                            argItmFldCode = argItmFldCode & Trim(.GetData(i, vItmColCode)) & ","
                        End If
                    Next i
                    If Trim(argItmFldCode) <> "" Then _
                    argItmFldCode = Strings.Left(argItmFldCode, Len(argItmFldCode) - 1)
                End If
            End With

            '        'Show all the hidden cols
            '        If Not argHideRate Then
            '            With vsfItmGrpDet
            '                For j = iStdCols + 1 To .Cols - 1 Step 2
            '                    If .ColHidden(j) Then .ColHidden(j) = False
            '                Next j
            '                Exit Sub
            '            End With
            '        End If

            strsql = "Select ig.ItemGroupID, ig.ItemGroupName"
            strsql = strsql & ",i.ItemName,i.ItemID,i.ItemCode"
            If Trim(argItmFldCode) <> "" Then _
            strsql = strsql & "," & argItmFldCode
            strsql = strsql & " from MstITemGroup ig"
            strsql = strsql & " ,MstItem i"
            strsql = strsql & " where 1=1 "
            strsql = strsql & " AND ig.ItemGroupID =i.ItemGroupID"
            If strItemGrp <> "" Then
                If optItem.Checked = True Then
                    strsql = strsql & " and i.ItemID in (" & strItemGrp & ")"
                Else
                    strsql = strsql & " and iG.ItemGroupID in (" & strItemGrp & ")"
                End If
            End If
            strsql = strsql & " order by IG.ItemGroupName,i.ItemName"
            recdummy = clsSVT.getDataReaderFromSQL(strsql)

            arrItmCol = Split(argItmFldCode, ",")
            iColCnt = UBound(arrItmCol) + 1
            With vsfItemGroupDet
                .Rows.Count = 1
                '.Clear flexClearScrollable
                .Cols.Count = iStdCols + IIf(iColCnt > 0, iColCnt * 2, 0)
                BlnFldCode = False
                If Not recdummy.HasRows Then
                    recdummy.Close()
                    Exit Sub
                End If
                Do While recdummy.Read
                    .Rows.Count = .Rows.Count + 1 : .Row = .Rows.Count - 1
                    .SetData(.Row, vItmGrpDetID, GetVal(recdummy!ItemGroupID, 0))
                    .SetData(.Row, vItmGrpDetGrpName, GetVal(recdummy!ItemGroupName, ""))
                    .SetData(.Row, vItmGrpDetItmID, GetVal(recdummy!ItemID, 0))
                    .SetData(.Row, vItmGrpDetItmCode, GetVal(recdummy!ItemCode, ""))
                    .SetData(.Row, vItmGrpDetItmName, GetVal(recdummy!ItemName, ""))
                    i = 0
                    For j = 0 To (iColCnt - 1) * 2 Step 2
                        If Not BlnFldCode Then 'Flag set for action to be done only once
                            .SetData(0, iStdCols + j, arrItmCol(i).ToString)
                            Call SetFlexColor(vsfItemGroupDet, "", iStdCols + j)
                            .SetData(0, iStdCols + j + 1, (arrItmCol(i)) & "-New")
                            '.ColHidden(iStdCols + j + 1) = argCalRate
                            .Cols(iStdCols + j + 1).Visible = Not argCalRate
                            'Call clsSvt.SetFlexColor(vsfItmGrpDet, "M", iStdCols + j + 1)
                        End If
                        .SetData(.Row, iStdCols + j, GetVal(recdummy.Item(arrItmCol(i)), 0))
                        'Get the Rate present as it is by default initially or acts like refresh
                        .SetData(.Row, iStdCols + j + 1, "") 'clsSvt.GetVal(recdummy.Fields(arrItmCol(i)), 0)
                        i = i + 1
                    Next j
                    BlnFldCode = True
                Loop
                For i = iStdCols To vsfItemGroupDet.Cols.Count - 1
                    .Cols(i).Format = "0.00"
                Next
                .AutoSizeCols()
                .Cols(vItmGrpDetID).AllowMerging = True
                .Cols(vItmGrpDetGrpName).AllowMerging = True
                .Row = 1
                .Focus()
            End With
            'Call enableFlds(1, False)
            Call CountSelRows()
            txtAmt.Text = ""
            Call EnableFlds(4, False)
            Call EnableFlds(2, False)
            Call EnableFlds(3, True)

            Call EnableSelFldScreen(False)
            recdummy.Close()
        Catch ex As Exception
            If ex.Message <> "" Then MsgBox(ex.Message, vbInformation + vbOKOnly, Me.Text)
        End Try
    End Sub

    Private Sub btnReplace_Click(sender As Object, e As EventArgs) Handles btnReplace.Click
        Dim i As Integer, iRow As Integer, iCol As Integer
        Dim iPrice As Double, blnReplace As Boolean, strMsg As String
        blnReplace = False
        'If chkRepList.value = vbChecked Then
        'update the val
        iPrice = 0
        With vsfItemGroupDet
            iRow = .Row : iCol = .Col
            If .Col < iStdCols + 1 Then
                Call btnApply_Click(sender, e)
            End If
            If .GetCellCheck(iRow, vItmGrpDetSel) = CheckEnum.Unchecked Then
                MsgBox("Please select " & Trim(.GetData(iRow, vItmGrpDetItmName)) &
                "(i.e. current row) " & vbLf + vbCrLf & "to update current selected price- " &
                Val(.GetData(iRow, iCol)) & " to rest of the selected " &
                "Item present in the list", vbInformation + vbOKOnly, Me.Text)
                GoTo ExitSub
            End If
            iPrice = Val(.GetData(iRow, iCol))
            For i = iRow To .Rows.Count - 1
                If .GetCellCheck(i, vItmGrpDetSel) = CheckEnum.Checked And i <> iRow Then
                    .SetData(i, iCol, iPrice)
                    blnReplace = True
                End If
            Next i

            strMsg = IIf(blnReplace, "Price replaced to " & iPrice & vbLf + vbCrLf & "For rest of the selected Items present in the list " _
            & "successfully!!!", "No Items is selected below '" & Trim(.GetData(iRow, vItmGrpDetItmName)) &
            "' to replace price as " & iPrice)

            MsgBox(strMsg, vbInformation + vbOKOnly, Me.Text)
            Call EnableFlds(2, True)
            Call EnableFlds(4, False)
        End With
ExitSub:
        'chkRepList.value = vbUnchecked
        Call EnableFlds(2, True)
        'End If
    End Sub

    Private Sub CountSelRows()
        Dim i As Integer
        Call EnableFlds(3, True)
        lblRecCnt.Text = 0
        With vsfItemGroupDet
            For i = 1 To .Rows.Count - 1
                If .GetCellCheck(i, vItmGrpDetSel) = CheckEnum.Checked Then
                    lblRecCnt.Text = Val(lblRecCnt.Text) + 1
                    lngRecCnt = Val(lblRecCnt.Text) + 1
                End If
            Next i
        End With
        If lngRecCnt > 0 Then lngRecCnt = lngRecCnt - 1
        lblRecCnt.Text = lblRecCnt.Text & " Item(s) Selected out of " & i - 1
    End Sub

    Private Sub frmMstPriceMaintenance_Load(sender As Object, e As EventArgs) Handles Me.Load
        initGridStyles(Me)
        cboRound.SelectedIndex = 0
        intDocID = GetFrmId(Me.Text)
        intMenuID = GetMenuId(Me.Text)
        If Not chkDocId(intDocID, gUserCode, Me.Text) Then Exit Sub
        strRights = getUserRights(intMenuID)
        Me.KeyPreview = True
        strItemGrp = ""

        Call initScreen()
        Call EnableFlds(1, False)
        Call EnableFlds(2, False)
        Call EnableFlds(4, False)
        Call EnableFlds(5, False)
        Call EnableFlds(3, False)
        Call EnableSelFldScreen(True)
        Call initFormVsfDet(Me, Me.Text, vsfItemGroup, "GROUPSEL")
        Call initFormVsfDet(Me, Me.Text, vsfItemCol, "ITEMCOLS")
        Call initFormVsfDet(Me, Me.Text, vsfItemGroupDet, "ITEMDET")
        fillFormProperties(Me, Nothing, False)
        lblFrm.ForeColor = Color.White
        lblFrm.Text = "Selection : " & Me.Text
        Call ChangeFontSize(Me)
        lblFrm.Font = New Font("Microsoft Sans Serif", 14, FontStyle.Bold)

    End Sub

    Private Sub initScreen()
        Call initItmGrp() 'Init Item grp col
        optItemGroup.Checked = True
        Call LoadGroups()

        Call initItmGrpDet() 'Init Item grp det col
        Call initItmCol() 'Init Item col grid

        lblAmt.Text = "By %age"

        If Not LoadSelItemCol() Then 'Load the Rate related item cols
            MsgBox("No Rate Fields present in Item Master.Cannot Continue!!!" & vbLf + vbCrLf & "Please add atleast one Rate field from Doc Designer.", vbCritical + vbOKOnly, Me.Text)
        End If
    End Sub

    Private Sub initItmGrp()
        With vsfItemGroup
            .Rows.Count = 0 : .Rows.Count = 1 : .Cols.Count = 6 : .Rows.Fixed = 1
            .SetData(0, vItmGrpSel, "Sel")
            .SetData(0, vItmGrpID, "Item Group ID")
            .SetData(0, vItmGrpName, "Item Group Name")
            .SetData(0, vItmGrpCode, "Item Group Code")
            .SetData(0, vItmGrpSName, "Short Name")
            .SetData(0, vItmGrpHotKey, "Hot Key")
            Call SetColWidth(vsfItemGroup, 405, 1170, 2205, 1425)
            .Cols(vItmGrpID).Visible = False
            .Cols(vItmGrpSel).DataType = GetType(Boolean)
            Call SetFlexColor(vsfItemGroup, "", vItmGrpName, vItmGrpCode, vItmGrpSName, vItmGrpHotKey)
            '.ExtendLastCol = True
            '.ExplorerBar = flexExSortShow
            .AutoSearch = AutoSearchEnum.FromCursor
            .AllowResizing = AllowResizingEnum.Columns
        End With
    End Sub

    Private Sub LoadGroups()
        'Dim recdummy As oledb.oledbDataReader
        Dim strsql As String
        strsql = "Select 0 as sel, ItemGroupID, ItemGroupName, ItemGroupCode "
        strsql = strsql & " from MstItemGroup"
        strsql = strsql & " where MstItemGroup.ItemGroupID in (select distinct ItemGroupID from MstItem)"
        strsql = strsql & " and Authorised = 'Y'"
        strsql = strsql & " order by ItemGroupName"
        Try
            'recdummy = clsSVT.getDataReaderFromSQL(strsql)

            'With vsfItemGroup
            '    .Rows.Count = 1
            '    .SetData(0, vItmGrpCode, "Item Group Code")
            '    .SetData(0, vItmGrpName, "Item Group Name")
            '    If recdummy.HasRows Then
            '        Do While recdummy.Read
            '            .Rows.Count = .Rows.Count + 1
            '            .Row = .Rows.Count - 1
            '            .SetData(.Row, vItmGrpID, GetVal(recdummy!ItemGroupID, 0))
            '            .SetData(.Row, vItmGrpName, GetVal(recdummy!ItemGroupName, ""))
            '            .SetData(.Row, vItmGrpCode, GetVal(recdummy!ItemGroupCode, ""))
            '        Loop
            '    End If
            '    .AutoSizeCols()
            '    recdummy.Close()
            'End With

            Dim dtItemGrp As New DataTable
            dtItemGrp = clsSVT.getDATATable(strsql)
            With vsfItemGroup
                .Rows.Count = dtItemGrp.Rows.Count + 1
                .SetData(0, vItmGrpCode, "Item Group")
                .SetData(0, vItmGrpName, "Item Group Name")
                Call SetColAlign(vsfItemGroup, svtAlign.Left, vItmGrpCode)
                Call SetHideCol(vsfItemGroup, True, vItmGrpName)
                Call SetHideCol(vsfItemGroup, True, vItmGrpSName, vItmGrpHotKey)
            End With

            Dim dr As DataRow, dc As DataColumn
            Dim rowIndex%, colIndex%
            rowIndex = 0
            For Each dr In dtItemGrp.Rows
                rowIndex = rowIndex + 1
                colIndex = 0
                For Each dc In dtItemGrp.Columns
                    vsfItemGroup(rowIndex, colIndex) = dr(dc)
                    colIndex = colIndex + 1
                Next
            Next
            If vsfItemGroup.Rows.Count >= 2 Then vsfItemGroup.TopRow = 1
            dtItemGrp = Nothing


        Catch ex As Exception
            MsgBox(ex.Message, vbInformation + vbOKOnly, Me.Text)
        End Try
    End Sub

    Private Sub initItmGrpDet()
        With vsfItemGroupDet
            .Rows.Count = 0 : .Rows.Count = 1
            .Cols.Count = iStdCols
            .Rows.Fixed = 1 : .Cols.Fixed = 0
            .SetData(0, vItmGrpDetGrpName, "Group Name")
            .SetData(0, vItmGrpDetItmName, "Item Name")
            .SetData(0, vItmGrpDetSel, "Sel")
            .SetData(0, vItmGrpDetItmCode, "Item Code")
            .Cols(vItmGrpDetSel).DataType = GetType(Boolean)
            Call SetHideCol(vsfItemGroupDet, True, vItmGrpDetID, vItmGrpDetItmID)
            Call SetFlexColor(vsfItemGroupDet, "", vItmGrpDetGrpName, vItmGrpDetItmName, vItmGrpDetItmCode)
            Call SetColWidth(vsfItemGroupDet, 960, 720, 960, 495, 960, 960)
            .ExtendLastCol = False
            '.ExplorerBar = flexExSortShow
            .AutoSearch = AutoSearchEnum.FromCursor
            .AllowResizing = AllowResizingEnum.Columns
            .AllowMerging = AllowMergingEnum.RestrictCols
            .Cols(vItmGrpDetGrpName).AllowMerging = True
        End With
    End Sub

    Private Sub initItmCol()
        With vsfItemCol
            .Rows.Count = 0 : .Rows.Count = 1 : .Rows.Fixed = 1 : .Cols.Fixed = 0
            .Cols.Count = 3
            .SetData(0, vItmColSel, "Sel")
            .SetData(0, vItmColCode, "ColCode")
            .SetData(0, vItmColName, "Item Rate Columns")
            .Cols(vItmColSel).DataType = GetType(Boolean)
            Call SetHideCol(vsfItemCol, True, vItmColCode)
            Call SetFlexColor(vsfItemCol, "", vItmColCode, vItmColName)
            .ExtendLastCol = True
            .AllowResizing = AllowResizingEnum.Columns
            .AutoSearch = AutoSearchEnum.FromCursor
            '.ExplorerBar = flexExSortShow
        End With
    End Sub

    Private Function LoadSelItemCol() As Boolean
        Dim recdummy As oledb.oledbDataReader
        Dim strSQL As String
        LoadSelItemCol = False

        strSQL = "select f.FldCode,F.FldCaption from UtlTransactions u,UTLDocDsgrFlds f"
        strSQL &= " Where 1 = 1 "
        strSQL &= " and u.HdrTblName = 'MstItem'"
        strSQL &= " and f.DocID = u.DocID"
        strSQL &= " and f.FLDDataType ='Numeric'"
        strSQL &= " and Right(FldCode,2)<>'ID'"
        Try
            recdummy = clsSVT.getDataReaderFromSQL(strSQL)

            vsfItemCol.Rows.Count = 1
            With vsfItemCol
                '#A - Taking Fld containing Rate only
                If recdummy.HasRows Then
                    Do While recdummy.Read
                        'If InStr(1, UCase(Trim(recdummy!FldCode)), "RATE") > 0 Or InStr(1, UCase(Trim(recdummy!FldCode)), "VAT") > 0 Or InStr(1, UCase(Trim(recdummy!FldCode)), "TAX") > 0 Then
                        If GetVal(recdummy!FldCode, "") <> clsSVT.getDataStringValue("Select FldCode 
                                    from UtlDocDsgrFlds f, UtlTransactions u 
                                    where f.DocID = u.DocID and u.HdrTblName='MstItemGroup' and FldCode='" & GetVal(recdummy!FldCode, "") & "'", "", False) Then
                            .Rows.Count = .Rows.Count + 1
                            .Row = .Rows.Count - 1
                            .SetData(.Row, vItmColCode, GetVal(recdummy!FldCode, ""))
                            .SetData(.Row, vItmColName, GetVal(recdummy!Fldcaption, ""))
                        End If
                        'End If
                    Loop
                    vsfItemCol.Row = 1
                    vsfItemCol.Focus()
                End If
                recdummy.Close()

                If .Rows.Count = 2 Then
                    .SetCellCheck(.Rows.Count - 1, vItmColSel, CheckEnum.Checked)
                    .Visible = False
                ElseIf .Rows.Count = 1 Then
                    Exit Function
                Else
                    .Visible = True
                End If

            End With
            vsfItemCol.AutoSizeCols()
            LoadSelItemCol = True
        Catch ex As Exception
            MsgBox(ex.Message, vbInformation + vbOKOnly, Me.Text)
        End Try
    End Function

    Private Sub optItemGroup_CheckedChanged(sender As Object, e As EventArgs) Handles optItemGroup.CheckedChanged
        If optItemGroup.Checked = True Then
            Call LoadGroups() 'Load Item grp
            Call initFormVsfDet(Me, Me.Text, vsfItemGroup, "GROUPSEL")
            lblvsfItemGroup.Text = "Records selected : 0/" & vsfItemGroup.Rows.Count - 1
            lblItemSel.Text = "** Please select atleast one " & IIf(optItemGroup.Checked, "Item Group", "Item") & " for altering Rate Columns"
        End If

    End Sub

    Private Sub optItem_CheckedChanged(sender As Object, e As EventArgs) Handles optItem.CheckedChanged
        If optItem.Checked = True Then
            Call showWaitCursor()
            Call initFormVsfDet(Me, Me.Text, vsfItemGroup, "ITEMSEL")
            Call LoadItems()
            lblvsfItemGroup.Text = "Records selected : 0/" & vsfItemGroup.Rows.Count - 1
            lblItemSel.Text = "** Please select atleast one " & IIf(optItemGroup.Checked, "Item Group", "Item") & " for altering Rate Columns"
            showNormalCursor()
        End If
    End Sub

    Private Sub LoadItems()

        'Dim recdummy As oledb.oledbDataReader
        Dim strsql As String
        strsql = "Select 0 as sel,i.ItemID,i.ItemCode,i.ItemName,i.ShortName,i.HotKey"
        'strsql = strsql & ",ig.ItemGroupID, ig.ItemGroupName, ig.ItemGroupCode "
        strsql = strsql & " from MstItemGroup ig,MstItem i"
        strsql = strsql & " where 1= 1 "
        '    If strItemGrp <> "" Then
        '        strSQL = strSQL & " and i.ItemID in (" & strItemGrp & ")"
        '    End If
        strsql = strsql & " and ig.Authorised = 'Y'"
        strsql = strsql & " and i.ItemGroupID = ig.ItemGroupID"
        strsql = strsql & " order by ItemGroupName"
        Try
            'recdummy = clsSVT.getDataReaderFromSQL(strsql)


            'With vsfItemGroup
            '    .Rows.Count = 1
            '    .SetData(0, vItmGrpCode, "Item Code")
            '    .SetData(0, vItmGrpName, "Item Name")
            '    If recdummy.HasRows Then
            '        Do While recdummy.Read
            '            .Rows.Count = .Rows.Count + 1
            '            .Row = .Rows.Count - 1
            '            .SetData(.Row, vItmGrpID, GetVal(recdummy!ItemID, 0))
            '            .SetData(.Row, vItmGrpName, GetVal(recdummy!ItemName, ""))
            '            .SetData(.Row, vItmGrpCode, GetVal(recdummy!ItemCode, ""))
            '        Loop
            '    End If
            '    recdummy.Close()
            '    '.AutoSize 0, .Cols - 1
            'End With
            Dim dtItems As New DataTable
            dtItems = clsSVT.getDATATable(strsql)
            With vsfItemGroup
                .Rows.Count = dtItems.Rows.Count + 1
                .SetData(0, vItmGrpCode, "Item Code")
                .SetData(0, vItmGrpName, "Item Name")
                .SetData(0, vItmGrpSName, "Short Name")
                .SetData(0, vItmGrpHotKey, "Hot Key")
                Call SetHideCol(vsfItemGroup, False, vItmGrpName)
                Call SetHideCol(vsfItemGroup, False, vItmGrpSName, vItmGrpHotKey)
            End With

            Dim dr As DataRow, dc As DataColumn
            Dim rowIndex%, colIndex%
            rowIndex = 0
            For Each dr In dtItems.Rows
                rowIndex = rowIndex + 1
                colIndex = 0
                For Each dc In dtItems.Columns
                    vsfItemGroup(rowIndex, colIndex) = dr(dc)
                    colIndex = colIndex + 1
                Next
            Next
            If vsfItemGroup.Rows.Count >= 2 Then vsfItemGroup.TopRow = 1
            dtItems = Nothing
        Catch ex As Exception
            MsgBox(ex.Message, vbInformation + vbOKOnly, Me.Text)
        End Try
    End Sub

    Private Sub vsfItemGroup_AfterEdit(sender As Object, e As RowColEventArgs) Handles vsfItemGroup.AfterEdit
        Dim i As Int32, intRecs As Int32
        With vsfItemGroup
            Select Case e.Col
                Case vItmGrpSel
                    vsfItemGroupDet.Rows.Count = 1
                    'vsfItmGrpDet.Clear flexClearScrollable
                    Call EnableFlds(3, False)

                    strItemGrp = ""
                    For i = 1 To .Rows.Count - 1
                        If .GetCellCheck(i, e.Col) = CheckEnum.Checked Then
                            strItemGrp = strItemGrp & Val(.GetData(i, vItmGrpID)) & ","
                        End If
                    Next i
                    If Trim(strItemGrp) <> "" Then
                        strItemGrp = Strings.Left(strItemGrp, Len(strItemGrp) - 1)
                        'Enable only if the item col is also selected
                        If vsfItemCol.FindRow(True, 1, vItmColSel, False) > 0 Then
                            Call EnableFlds(1, True)
                            'Call cmdLoad_Click
                        End If
                    Else
                        Call EnableFlds(1, False)
                    End If
                    If strItemGrp = "" Then
                        lblItemSel.Visible = True
                        lblItemSel.Text = "** Please select atleast one " & IIf(optItemGroup.Checked, "Item Group", "Item") & " for altering Rate Columns"
                    Else
                        lblItemSel.Visible = False
                    End If
            End Select

            For i = 1 To .Rows.Count - 1
                If .GetCellCheck(i, vItmGrpSel) = CheckEnum.Checked Then
                    intRecs = intRecs + 1
                End If
            Next i
            lblvsfItemGroup.Text = "Records selected : " & intRecs & "/" & vsfItemGroup.Rows.Count - 1
        End With

    End Sub

    Private Sub vsfItemGroup_BeforeEdit(sender As Object, e As RowColEventArgs) Handles vsfItemGroup.BeforeEdit
        If vsfItemGroup.Col <> vItmGrpSel Then
            e.Cancel = True
        End If
    End Sub

    Private Sub vsfItemGroup_BeforeSort(sender As Object, e As SortColEventArgs) Handles vsfItemGroup.BeforeSort
        Dim i As Integer, blnchkItm As Boolean
        blnchkItm = False
        'Call vsfItemGrp_AfterEdit(vsfItemGrp.Row, Col)
        With vsfItemGroup
            If e.Col <> vItmGrpSel Then Exit Sub
            vsfItemGroupDet.Rows.Count = 1
            'vsfItmGrpDet.Clear flexClearScrollable
            Call EnableFlds(3, False)
            For i = 1 To .Rows.Count - 1
                If .GetCellCheck(i, 0) = CheckEnum.Checked Then
                    .SetCellCheck(i, 0, CheckEnum.Unchecked)
                Else
                    .SetCellCheck(i, 0, CheckEnum.Checked)
                    blnchkItm = True
                End If
            Next
            If blnchkItm And vsfItemCol.FindRow(True, 1, vItmGrpSel, False) > 0 Then
                Call EnableFlds(1, True)
            Else
                Call EnableFlds(1, False)
            End If
        End With

    End Sub

    Private Sub vsfItemGroup_KeyUp(sender As Object, e As KeyEventArgs) Handles vsfItemGroup.KeyUp
        With vsfItemGroup
            If .Row = .Rows.Count - 1 And .Col = .Cols.Count - 1 And e.KeyCode = Keys.Tab Then
                vsfItemCol.Focus()
            End If
        End With
    End Sub

    Private Sub vsfItemCol_AfterEdit(sender As Object, e As RowColEventArgs) Handles vsfItemCol.AfterEdit
        vsfItemGroupDet.Rows.Count = 1
        'vsfItmGrpDet.Clear flexClearScrollable
        Call EnableFlds(3, False)

        If vsfItemCol.FindRow(True, 1, vItmColSel, False) > 0 And vsfItemGroup.FindRow(True, 1, vItmGrpSel, False) > 0 Then
            Call EnableFlds(1, True)
            'Call cmdLoad_Click
        Else
            Call EnableFlds(1, False)
        End If
        If vsfItemCol.FindRow(True, 1, vItmColSel, False) > 0 Then
            lblRateSel.Visible = False
        Else
            lblRateSel.Visible = True
        End If
    End Sub

    Private Sub vsfItemCol_BeforeEdit(sender As Object, e As RowColEventArgs) Handles vsfItemCol.BeforeEdit
        If e.Col <> vItmColSel Then e.Cancel = True
    End Sub

    Private Sub vsfItemCol_BeforeSort(sender As Object, e As SortColEventArgs) Handles vsfItemCol.BeforeSort
        Dim i As Integer, blnchkItm As Boolean
        blnchkItm = False
        With vsfItemCol
            If e.Col <> vItmColSel Then Exit Sub
            vsfItemGroupDet.Rows.Count = 1
            'vsfItmGrpDet.Clear flexClearScrollable
            Call EnableFlds(3, False)

            For i = 1 To .Rows.Count - 1
                .Col = vItmColSel : .Row = i

                If .GetCellCheck(i, vItmColSel) = CheckEnum.Checked Then
                    .SetCellCheck(i, vItmColSel, CheckEnum.Unchecked)
                Else
                    .SetCellCheck(i, vItmColSel, CheckEnum.Checked)
                    blnchkItm = True
                End If
            Next
        End With
        If blnchkItm And vsfItemGroup.FindRow(True, 1, vItmGrpSel, False) > 0 Then
            Call EnableFlds(1, True)
        Else
            Call EnableFlds(1, False)
        End If
    End Sub

    Private Sub vsfItemCol_KeyUp(sender As Object, e As KeyEventArgs) Handles vsfItemCol.KeyUp
        With vsfItemGroup
            If .Row = .Rows.Count - 1 And .Col = .Cols.Count - 1 And e.KeyCode = Keys.Tab Then
                vsfItemGroupDet.Focus()
            End If
        End With

    End Sub

    Private Sub vsfItemGroupDet_AfterEdit(sender As Object, e As RowColEventArgs) Handles vsfItemGroupDet.AfterEdit
        Dim cs As C1.Win.C1FlexGrid.CellStyle
        cs = vsfItemGroupDet.Styles.Add("BoldnColor")
        cs.Font = New Font(vsfItemGroupDet.Font.FontFamily, vsfItemGroupDet.Font.Size, FontStyle.Bold)
        cs.ForeColor = Color.FromArgb(128, 64, 0)

        Dim csn As C1.Win.C1FlexGrid.CellStyle
        csn = vsfItemGroupDet.Styles.Add("Normal")
        csn.Font = New Font(vsfItemGroupDet.Font.FontFamily, vsfItemGroupDet.Font.Size, FontStyle.Regular)
        csn.ForeColor = Color.Black

        With vsfItemGroupDet
            Call EnableFlds(4, False)
            If .FindRow(True, 1, vItmGrpDetSel, False) > 0 Then
                If Val(txtAmt.Text) > 0 Then Call EnableFlds(4, True)
                Call EnableFlds(2, True)
                Call EnableFlds(3, True)
            Else
                Call EnableFlds(2, False)
                Call EnableFlds(3, False)
            End If

            Call CountSelRows()
        End With
    End Sub

    Private Sub vsfItemGroupDet_BeforeEdit(sender As Object, e As RowColEventArgs) Handles vsfItemGroupDet.BeforeEdit
        With vsfItemGroupDet
            If .Row = 0 Then Exit Sub
            Select Case .Col
                Case vItmGrpDetSel
                Case Else
                    If .GetCellCheck(.Row, vItmGrpDetSel) = CheckEnum.Checked And
                        InStr(1, .GetData(0, .Col), "-New") > 0 Then
                    Else
                        e.Cancel = True
                    End If
            End Select
        End With
    End Sub

    Private Sub vsfItemGroupDet_BeforeSort(sender As Object, e As SortColEventArgs) Handles vsfItemGroupDet.BeforeSort
        Dim cs As C1.Win.C1FlexGrid.CellStyle
        cs = vsfItemGroupDet.Styles.Add("BoldnColor")
        cs.Font = New Font(vsfItemGroupDet.Font.FontFamily, vsfItemGroupDet.Font.Size, FontStyle.Bold)
        cs.ForeColor = Color.FromArgb(128, 64, 0)

        Dim csn As C1.Win.C1FlexGrid.CellStyle
        csn = vsfItemGroupDet.Styles.Add("Normal")
        csn.Font = New Font(vsfItemGroupDet.Font.FontFamily, vsfItemGroupDet.Font.Size, FontStyle.Regular)
        csn.ForeColor = Color.Black

        Dim i As Integer, blnItmChk As Boolean
        blnItmChk = False
        With vsfItemGroupDet
            If .Col <> vItmGrpDetSel Then Exit Sub
            For i = 1 To .Rows.Count - 1
                .Col = vItmGrpDetSel : .Row = i
                If .GetCellCheck(i, vItmGrpDetSel) = CheckEnum.Checked Then
                    .SetCellCheck(i, vItmGrpDetSel, CheckEnum.Unchecked)
                    '.Cell(flexcpFontBold, i, vItmGrpCode, i, .Cols - 1) = False
                    '.Cell(flexcpForeColor, i, vItmGrpCode, i, .Cols - 1) = vbBlack
                    For j = vItmGrpCode To .Cols.Count - 1
                        .SetCellStyle(.Row, j, "Normal")
                    Next
                    Call FillRate(False, CLng(i))
                Else
                    .SetCellCheck(i, vItmGrpDetSel, CheckEnum.Checked)
                    blnItmChk = True
                    '.Cell(flexcpFontBold, i, vItmGrpCode, i, .Cols - 1) = True
                    '.Cell(flexcpForeColor, i, vItmGrpCode, i, .Cols - 1) = &H4080&
                    For j = vItmGrpCode To .Cols.Count - 1
                        .SetCellStyle(.Row, j, "BoldnColor")
                    Next
                    Call FillRate(True, CLng(i))
                End If
            Next
        End With

        Call CountSelRows()
        Call EnableFlds(4, IIf(blnItmChk, True, False))
        Call EnableFlds(2, False)
        'Call enableFlds(2, IIf(blnItmChk, True, False))
        Call EnableFlds(3, IIf(blnItmChk, True, False))
    End Sub

    Private Sub FillRate(argFillRate As Boolean, argRow As Integer)
        Dim i As Integer
        With vsfItemGroupDet
            'argFillRate = IIf(.Cell(flexcpChecked, .Row, vItmGrpDetSel) = flexChecked, True, False)
            For i = iStdCols + 1 To .Cols.Count - 1 Step 2
                If argFillRate Then
                    .SetData(argRow, i, Val(.GetData(argRow, i - 1)))
                Else
                    .SetData(argRow, i, "")
                End If
            Next i
        End With
    End Sub

    Private Sub vsfItemGroupDet_EnterCell(sender As Object, e As EventArgs) Handles vsfItemGroupDet.EnterCell
        With vsfItemGroupDet
            If .Rows.Count <= 1 Or .Cols.Count < iStdCols Then Exit Sub
            If .Col = vItmGrpDetSel Then Exit Sub
            If InStr(1, .GetData(0, .Col), "-New") > 0 Then
                Try
                    .StartEditing()
                Catch
                End Try
            End If
        End With
    End Sub

    Private Sub vsfItemGroupDet_KeyUp(sender As Object, e As KeyEventArgs) Handles vsfItemGroupDet.KeyUp
        With vsfItemGroupDet
            If e.KeyCode = Keys.Escape Then Exit Sub
            If .Row = .Rows.Count - 1 And .Col = .Cols.Count - 1 And e.KeyCode = Keys.Tab Then
                optInc.Focus()
            End If
        End With
    End Sub

    Private Sub vsfItemGroupDet_RowColChange(sender As Object, e As EventArgs) Handles vsfItemGroupDet.RowColChange
        With vsfItemGroupDet
            If .Rows.Count <= 1 Or .Cols.Count < iStdCols Then Exit Sub
            If InStr(1, UCase(Trim(.GetData(0, .Col))), "-NEW") > 0 And
                    .GetCellCheck(.Row, vItmGrpDetSel) = CheckEnum.Checked And Val(txtAmt.Text) = 0 Then
                Call EnableFlds(5, True)
                'cmdReplace.Caption = "Replace Price: " & .ValueMatrix(.Row, .Col) & " for rest of the List"
                '.Col = iStdCols + 1
                '.SetFocus
                'GoTo ExitSub
            Else
                Call EnableFlds(5, False)
                'cmdReplace.Caption = "Replace for rest of the List"
            End If

        End With
        tt.SetToolTip(btnReplace, btnReplace.Text)
    End Sub

    Private Sub vsfItemGroupDet_ValidateEdit(sender As Object, e As ValidateEditEventArgs) Handles vsfItemGroupDet.ValidateEdit
        With vsfItemGroupDet
            If .Col <> vItmGrpDetSel Then
                If Val(.Editor.Text) < 0 Then
                    MsgBox("Rate cannot be negative.Please enter a valid Rate", vbInformation + vbOKOnly, Me.Text)
                    e.Cancel = True
                End If

                If .Col = vItmGrpDetSel Then _
                Call FillRate(IIf(.Editor.Text = 1, True, False), .Row)
            End If
        End With
    End Sub

    Private Sub optPer_CheckedChanged(sender As Object, e As EventArgs) Handles optPer.CheckedChanged
        If optPer.Checked Then lblAmt.Text = "By % age"
        txtAmt_TextChanged(sender, e)
    End Sub

    Private Sub optAmt_CheckedChanged(sender As Object, e As EventArgs) Handles optAmt.CheckedChanged
        If optAmt.Checked Then lblAmt.Text = "By Amount"
        txtAmt_TextChanged(sender, e)
    End Sub

    Private Sub txtAmt_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtAmt.KeyPress
        If e.KeyChar = Chr(Keys.Escape) Or e.KeyChar = Chr(Keys.Tab) Or e.KeyChar = Chr(Keys.Back) Then Exit Sub
        If Not KeyValidate("Decimal", e.KeyChar) Then
            e.Handled = True
        End If

        If e.KeyChar = "." And InStr(1, Trim(txtAmt.Text), ".") > 0 Then
            e.Handled = True
        End If

    End Sub

    Private Sub txtAmt_Validating(sender As Object, e As CancelEventArgs) Handles txtAmt.Validating
        If Trim(txtAmt.Text) = "" Or Val(txtAmt.Text) = 0 Then Exit Sub
        If Not ValidateNumber(Me.Name, txtAmt.Text, "", True, 2, False, False) _
            And Trim(txtAmt.Text) <> "" Then _
            e.Cancel = True
    End Sub

    Private Sub btnLoad_Click(sender As Object, e As EventArgs) Handles btnLoad.Click
        btnUpdtToDB.Enabled = True
        lblFrm.Text = "Update Rate : " & Me.Text
        Call UpdateVal(False)
    End Sub

    Private Sub txtAmt_TextChanged(sender As Object, e As EventArgs) Handles txtAmt.TextChanged
        Dim blnEnable As Boolean
        blnEnable = IIf(Val(txtAmt.Text) > 0, True, False)
        Call EnableFlds(4, blnEnable)
        Call EnableFlds(5, Not blnEnable)
    End Sub

    Private Sub frmMstPriceMaintenance_KeyUp(sender As Object, e As KeyEventArgs) Handles Me.KeyUp
        If e.KeyCode = Keys.F9 And e.Control = True Then
            Call initFormSize(Me, Me.Text, True, 1200, 648, 0, 0)
            If optItemGroup.Checked Then
                Call initFormVsfDet(Me, Me.Text, vsfItemGroup, "GROUPSEL")
            Else
                Call initFormVsfDet(Me, Me.Text, vsfItemGroup, "ITEMSEL")
            End If
            Call initFormVsfDet(Me, Me.Text, vsfItemCol, "ITEMCOLS")
            Call initFormVsfDet(Me, Me.Text, vsfItemGroupDet, "ITEMDET")
            MsgBox("Restored Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
        If e.KeyCode = Keys.F8 And e.Control = True Then
            Call SaveActFormSettings(Me, "MstParty")
            If optItemGroup.Checked Then
                Call SaveActFormVsfDet(Me, Me.Text, "GROUPSEL", vsfItemGroup)
            Else
                Call SaveActFormVsfDet(Me, Me.Text, "ITEMSEL", vsfItemGroup)
            End If
            Call SaveActFormVsfDet(Me, Me.Text, "ITEMCOLS", vsfItemCol)
            Call SaveActFormVsfDet(Me, Me.Text, "ITEMDET", vsfItemGroupDet)
            MsgBox("Saved Form & Grid Layout Successfully!!", vbInformation, Me.Text)
            End If
            If e.Alt And e.KeyCode = Keys.E Then
            If pnlMain.Visible Then
                If vsfItemGroupDet.Rows.Count > 1 Then
                    Call ExportToXL(vsfItemGroupDet)
                End If
            Else
                If vsfItemGroup.Rows.Count > 1 Then
                    Call ExportToXL(vsfItemGroup)
                End If
            End If
        End If
    End Sub

    Private Sub vsfItemGroupDet_CellChecked(sender As Object, e As RowColEventArgs) Handles vsfItemGroupDet.CellChecked
        If vsfItemGroupDet.Rows.Count <= 1 Then Exit Sub

        With vsfItemGroupDet
            If .GetCellCheck(.Row, vItmGrpDetSel) = CheckEnum.Checked Then  'If chked
                '.Cell(flexcpFontBold, Row, vItmGrpCode, Row, .Cols - 1) = True
                '.Cell(flexcpForeColor, Row, vItmGrpCode, Row, .Cols - 1) = &H4080&
                For i = vItmGrpDetItmCode To .Cols.Count - 1
                    .SetCellStyle(.Row, i, "BoldnColor")
                Next
                'Call FillRate(True)
            Else
                '.Cell(flexcpFontBold, Row, vItmGrpCode, Row, .Cols - 1) = False
                '.Cell(flexcpForeColor, Row, vItmGrpCode, Row, .Cols - 1) = vbBlack
                For i = vItmGrpDetItmCode To .Cols.Count - 1
                    .SetCellStyle(.Row, i, "Normal")
                Next
                'Call FillRate(False)
            End If
        End With
    End Sub

    Private Sub cboRound_Validating(sender As Object, e As CancelEventArgs) Handles cboRound.Validating
        If cboRound.Items.Contains(cboRound.Text) = False Then
            e.Cancel = True
        End If
        txtAmt_TextChanged(sender, e)
    End Sub

    Private Sub optDec_CheckedChanged(sender As Object, e As EventArgs) Handles optDec.CheckedChanged
        txtAmt_TextChanged(sender, e)
    End Sub

    Private Sub optInc_CheckedChanged(sender As Object, e As EventArgs) Handles optInc.CheckedChanged
        txtAmt_TextChanged(sender, e)
    End Sub

    Private Sub btnExport_Click(sender As Object, e As EventArgs) Handles btnExport.Click
        If vsfItemGroupDet.Rows.Count <= 1 Then Exit Sub
        Call ExportToXL(vsfItemGroupDet)
    End Sub

    Private Sub btnExpXLvsfItemGroup_Click(sender As Object, e As EventArgs) Handles btnExpXLvsfItemGroup.Click
        If vsfItemGroup.Rows.Count <= 1 Then Exit Sub
        Call ExportToXL(vsfItemGroup)
    End Sub

    Private Sub chkAll_CheckedChanged(sender As Object, e As EventArgs) Handles chkAll.CheckedChanged
        With vsfItemGroupDet
            If .Rows.Count <= 0 Then Exit Sub
            If chkAll.Checked = True Then
                For i As Int32 = 1 To .Rows.Count - 1
                    .SetData(i, vItmGrpDetSel, True)
                Next
            Else
                For i As Int32 = 1 To .Rows.Count - 1
                    .SetData(i, vItmGrpDetSel, False)
                Next
            End If
        End With
        Call CountSelRows()
    End Sub

    Private Sub vsfItemGroupDet_KeyPressEdit(sender As Object, e As KeyPressEditEventArgs) Handles vsfItemGroupDet.KeyPressEdit
        If e.KeyChar = "'" Then e.Handled = True
        If vsfItemGroupDet.Col > iStdCols Then
            If Not KeyValidate("Decimal_NonNegative", e.KeyChar) Then
                e.Handled = True
            End If
        End If
    End Sub

    Private Sub frmMstPriceMaintenance_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        lblRateSel.Visible = True
        lblItemSel.Visible = True
        lblItemSel.Text = "** Please select atleast one " & IIf(optItemGroup.Checked, "Item Group", "Item") & " for altering Rate Columns"
        lblRateSel.Text = "** Please select atleast one Rate to alter"
    End Sub

    Private Function CheckforNegative() As Boolean
        CheckforNegative = False
        'update the val
        With vsfItemGroupDet
            For i = 1 To .Rows.Count - 1
                If .GetCellCheck(i, vItmGrpDetSel) = CheckEnum.Checked Then
                    For j = iStdCols To .Cols.Count - 1 Step 2
                        If optInc.Checked Then
                            If optAmt.Checked = True Then 'Amt
                                If svtModule.SVTROUND(Val(.GetData(i, j)) + Val(txtAmt.Text), 2) < 0 Then
                                    CheckforNegative = False
                                    Exit Function
                                End If
                            Else 'Per
                                If svtModule.SVTROUND(Val(.GetData(i, j)) + (Val(.GetData(i, j)) * Val(txtAmt.Text) / 100), 2) < 0 Then
                                    CheckforNegative = False
                                    Exit Function
                                End If
                                If cboRound.Text = "50p RoundOff" Then
                                    If ROUND50(Val(.GetData(i, j + 1))) < 0 Then
                                        CheckforNegative = False
                                        Exit Function
                                    End If
                                End If
                                If cboRound.Text = "1Rs RoundOff" Then
                                    If ROUND100(Val(.GetData(i, j + 1))) < 0 Then
                                        CheckforNegative = False
                                        Exit Function
                                    End If
                                End If
                            End If
                        Else
                            If optAmt.Checked = True Then 'Amt
                                If svtModule.SVTROUND(Val(.GetData(i, j)) - Val(txtAmt.Text), 2) < 0 Then
                                    CheckforNegative = False
                                    Exit Function
                                End If
                            Else 'Per
                                If svtModule.SVTROUND(Val(.GetData(i, j)) - (Val(.GetData(i, j)) * Val(txtAmt.Text) / 100), 2) < 0 Then
                                    CheckforNegative = False
                                    Exit Function
                                End If
                                If cboRound.Text = "50p RoundOff" Then
                                    If ROUND50(Val(.GetData(i, j + 1))) < 0 Then
                                        CheckforNegative = False
                                        Exit Function
                                    End If
                                End If
                                If cboRound.Text = "1Rs RoundOff" Then
                                    If ROUND100(Val(.GetData(i, j + 1))) < 0 Then
                                        CheckforNegative = False
                                        Exit Function
                                    End If
                                End If

                            End If
                        End If
                    Next j
                End If
            Next i
        End With
        Return True
    End Function

    Private Sub vsfItemGroup_AfterSort(sender As Object, e As SortColEventArgs) Handles vsfItemGroup.AfterSort
        If vsfItemGroup.FindRow(True, 1, vItmGrpSel, False) > 0 Then
            lblItemSel.Visible = False
        Else
            lblItemSel.Text = "** Please select atleast one " & IIf(optItemGroup.Checked, "Item Group", "Item") & " for altering Rate Columns"
            lblItemSel.Visible = True
        End If
    End Sub

    Private Sub vsfItemCol_AfterSort(sender As Object, e As SortColEventArgs) Handles vsfItemCol.AfterSort
        If vsfItemCol.FindRow(True, 1, vItmColSel, False) > 0 Then
            lblRateSel.Visible = False
        Else
            lblRateSel.Visible = True
        End If
    End Sub

End Class