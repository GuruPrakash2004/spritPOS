Imports System.IO
Imports C1.Win.C1FlexGrid
Public Class frmUtlAlias

    Dim clsSVT As New svtLibrary
    Dim Mode As String
    Dim dlg As New OpenFileDialog()
    Dim strformcaption As String = ""
    Dim intSLNo As Integer
    Public strFileName As String
    Public strFilepath As String

    Const vDetAliasName = 0

    Private Sub frmUtlAlias_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        strformcaption = ""

        Try
            If frmMain.ActiveMdiChild.Text = "" Then
                lblformcaption.Text = frmMain.Text
                strformcaption = frmMain.Name
            Else
                lblformcaption.Text = frmMain.ActiveMdiChild.Text
                strformcaption = frmMain.ActiveMdiChild.Name
            End If

        Catch ex As Exception
            lblformcaption.Text = frmMain.Text
            strformcaption = frmMain.Name
            Exit Sub
        End Try

        initScreen()


    End Sub

    Private Sub initScreen()
        Call initGridStyles(Me)
        With vsfDet
            .Cols.Count = 1
            .Rows.Count = 2
            .SetData(0, vDetAliasName, "Alias Name")
            .ExtendLastCol = True
            '.Styles("Fixed").Font = New Font("Consolas", 14, FontStyle.Bold)
            '.Styles.Fixed.TextAlign = TextAlignEnum.CenterCenter
            .Styles.Normal.TextAlign = TextAlignEnum.LeftCenter
            .Styles("Fixed").TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter
            .Cols(0).TextAlignFixed = TextAlignEnum.CenterCenter
        End With
        'SetColAlign(vsfDet, svtAlign.Center, vsfDetSLno, vsfDetGroup, vsfDetFileName, vsfDetRemarks)
        Mode = "V"
        fillDocDetails(Trim(gStrTempFormName))
        vsfDetHeight()
        vsfDet.Focus()
        'vsfDet.Cols(vDetGroup).AllowMerging = True
        'vsfDet.AllowMerging = AllowMergingEnum.RestrictRows
        vsfDet.AutoSizeCols()
    End Sub

    Private Sub vsfDetHeight()
        Dim FixedRowHeight As Int32 = 40
        Dim NormalRowHeight As Int32 = 30
        vsfDet.Font = New Font(vsfDet.Font.FontFamily, 12)
        For Each row As C1.Win.C1FlexGrid.Row In vsfDet.Rows
            If row.Index < vsfDet.Rows.Fixed Then
                'For Fixed rows
                row.HeightDisplay = FixedRowHeight
            Else
                'For other rows
                row.HeightDisplay = NormalRowHeight
            End If
        Next
    End Sub

    Private Function ValidFields() As Boolean
        Dim rs As oledb.oledbDataReader = Nothing
        Dim strSql As String = ""
        Dim intDupRow As Integer = 0

        ValidFields = False
        RemoveEmptVSFRows(vsfDet, 1, vDetAliasName)

        If clsSVT.ChkDuplicateData(vsfDet, intDupRow, vDetAliasName) Then
            'MsgBox("Duplicate  " & .GetData(intDupRow, vDetItemCode) & "!!", vbInformation, Me.Text)
            MsgBox("Duplicate alias name: '" & vsfDet.GetData(intDupRow, vDetAliasName) & "'!", MsgBoxStyle.Information, Me.Text)
            vsfDet.Row = intDupRow
            vsfDet.TopRow = intDupRow
            vsfDet.Col = vDetAliasName
            vsfDet.Focus()
            Exit Function
        End If

        'If gridEmpty() Then
        '    MsgBox("Please enter atleast 1 alias name before saving!", vbInformation, Me.Text)
        '    Exit Function
        'End If

        strSql = "Select * from UtlMnuAlias where MnuFormName <> '" & Trim(gStrTempFormName) & "'"
        rs = clsSVT.getDataReaderFromSQL(strSql)

        With vsfDet
            Do While rs.Read()
                For i = 1 To .Rows.Count - 1
                    If Trim(UCase(.GetData(i, vDetAliasName))) = Trim(UCase(rs!MnuAliasName)) Then
                        MsgBox("Alias name: '" & Trim(.GetData(i, vDetAliasName)) & "' already used for '" & rs!MnuFormCaption _
                               & "'!" & vbCrLf & "Try a different name!", MsgBoxStyle.Information, Me.Text)
                        .Row = i
                        .Col = vDetAliasName
                        .Focus()
                        Exit Function
                    End If
                Next
            Loop
            rs.Close()
        End With

        ValidFields = True
    End Function

    Private Sub btnClose_Click_1(sender As System.Object, e As System.EventArgs) Handles btnClose.Click
        strformcaption = ""
        Me.Close()
    End Sub

    Private Sub ClearFlds()
        vsfDet.Enabled = True
    End Sub

    'Private Sub btnSave_Click(sender As System.Object, e As System.EventArgs) Handles btnSave.Click
    '    stringReplace(Me)
    '    If ValidFields() Then
    '        If saveDetails() Then
    '            fillDocDetails(Trim(gStrTempFormName))
    '            frmMain.bindCboDropDown()
    '        End If
    '    End If
    'End Sub

    Private Sub fillDocDetails(ByVal argFrmCapt As String)
        Dim RsTemp As oledb.oledbDataReader
        Dim strSQL As String
        Dim i As Integer
        'Dim i As Integer, X As Integer
        If argFrmCapt = "" Then Exit Sub
        'txtfrmCaption.Text = argFrmCapt
        strSQL = "Select MnuAliasName from UtlMnuAlias where MnuFormName = '" & Trim(gStrTempFormName) & "' order by MnuAliasName"
        RsTemp = clsSVT.getDataReaderFromSQL(strSQL)
        With vsfDet
            .Rows.Count = 1

            Do While RsTemp.Read()
                i = .Rows.Count
                .Rows.Count = .Rows.Count + 1
                .SetData(i, vDetAliasName, RsTemp!MnuAliasName)
            Loop
            RsTemp.Close()
        End With

        Call gridEmpty()
    End Sub
    Private Sub ClearFieldvsfVwdet()
        With vsfDet
            .Rows.Count = 1
            .Rows.Count = 2
        End With
    End Sub

    Private Function saveDetails() As Boolean
        Dim strSQL As String = ""
        Dim svtTrn As oledb.oledbTransaction = Nothing

        saveDetails = False

        With vsfDet
            svtTrn = svtConn.BeginTransaction(IsolationLevel.ReadCommitted)

            Try
                strSQL = "Delete from UtlMnuAlias where MnuFormName = '" & Trim(gStrTempFormName) & "'"
                If Not clsSVT.execSQLStatement(strSQL, svtTrn) Then
                    svtTrn.Rollback()
                    Exit Function
                End If

                For i = 1 To .Rows.Count - 1
                    strSQL = " Insert into UtlMnuAlias ("
                    strSQL = strSQL & "MnuFormName,MnuAliasName,MnuFormCaption) values ("
                    strSQL = strSQL & "'" & Trim(gstrTempFormName) & "',"
                    strSQL = strSQL & "'" & Trim(.GetData(i, vDetAliasName)) & "',"
                    strSQL = strSQL & "'" & Trim(lblformcaption.Text) & "')"

                    If Not clsSVT.execSQLStatement(strSQL, svtTrn) Then
                        svtTrn.Rollback()
                        Exit Function
                    End If
                Next i

                saveDetails = True
                svtTrn.Commit()
                MsgBox("Alias details saved successfully!", vbInformation, Me.Text)

            Catch ex As Exception
                MsgBox("Error while saving alias details!" & vbCrLf & ex.Message, MsgBoxStyle.Critical, Me.Text)
                svtTrn.Rollback()
                Exit Function
            End Try
        End With
    End Function

    Private Sub vsfDet_EnterCell(sender As Object, e As System.EventArgs) Handles vsfDet.EnterCell
        If vsfDet.Row > 1 Then
            vsfDet.StartEditing()
        End If
    End Sub

    Private Sub vsfDet_GotFocus(sender As Object, e As System.EventArgs) Handles vsfDet.GotFocus
        With vsfDet
            If .Rows.Count = 2 AndAlso Trim(.GetData(1, vDetAliasName)) = "" Then
                .Row = .Rows.Count - 1
                .StartEditing()
            End If
        End With
    End Sub

    Private Sub vsfDet_KeyUp(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles vsfDet.KeyUp
        Dim intRow As Integer

        With vsfDet
            If e.KeyCode = Keys.Tab And .Row = .Rows.Count - 1 And .ColSel = vDetAliasName Then
                If Trim(.GetData(.Row, vDetAliasName) <> "") Then
                    intRow = .Rows.Count
                    .Rows.Count = .Rows.Count + 1
                    .Col = vDetAliasName
                    .Row = intRow
                    .LeftCol = vDetAliasName
                    .Focus()
                Else
                    RemoveEmptVSFRows(vsfDet, 1, vDetAliasName)
                    .Row = 1
                    .Col = vDetAliasName
                    btnRemoveRow.Focus()
                End If
            End If
        End With
    End Sub

    Private Sub btnRemoveRow_Click(sender As System.Object, e As System.EventArgs) Handles btnRemoveRow.Click
        If vsfDet.Row <= 0 Then Exit Sub

        If gridEmpty() Then
            MsgBox("No rows to remove!", vbInformation, Me.Text)
            Exit Sub
        End If

        If MsgBox("Are you sure you want to remove this row: " & vsfDet.GetData(vsfDet.Row, vDetAliasName) & " ?", vbYesNo + MsgBoxStyle.DefaultButton2, "Confirmation...") = vbNo Then Exit Sub

        vsfDet.RemoveItem(vsfDet.Row)

        If vsfDet.Rows.Count = 1 Then
            vsfDet.Rows.Count = 2
        End If
    End Sub

    Public Function gridEmpty() As Boolean
        gridEmpty = False

        If vsfDet.Rows.Count = 1 Then
            vsfDet.Rows.Count = 2
        End If
        '''Changed by sandhya
        If vsfDet.Rows.Count = 2 AndAlso Trim(GetVal(vsfDet.GetData(1, vDetAliasName), "")) = "" Then
            gridEmpty = True
        End If
    End Function
End Class