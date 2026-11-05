Imports C1.Win.C1FlexGrid

Public Class frmUtlSearchSettings

    Const vDetSel = 0
    Const vDetFldCode = 1
    Const vDetSLNO = 2
    Const vDetFldCaption = 3
    Const vDetDsgrFld = 4
    Const vDel = 5
    Const vTo = 6
    Const vName = 7

    Dim clsSVT As New svtLibrary
    Public strDocName As String
    Public strTblName As String
    Public strMainFld As String
    Public blnSRCHFrom As Boolean
    Public dblTotCol As Long
    Public argForm As Form
    Dim intchkClr As Integer
    Dim chkPrnt As Integer
    Dim strDispFldList As String
    Public argFrm As Object
    Public blnBulkSMS As Boolean
    Public intDocID As Integer

    Private Sub frmUtlSearchSettings_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim ToolTip As New ToolTip
        With vsfDet
            .Cols.Count = 5
            .Cols(0).DataType = GetType(Boolean)
            .Rows.Count = 1
            .SetData(0, vDetSLNO, "SLNO")
            .SetData(0, vDetSel, "Select")
            .SetData(0, vDetFldCode, "Field to Search")
            .SetData(0, vDetFldCaption, "Field Caption")
            Call SetHideCol(vsfDet, True, vDetSLNO, vDetDsgrFld)

        End With

        If blnSRCHFrom Then
            Call FillSrchFormFlds(strDocName, strTblName, strMainFld)
        ElseIf Trim(strDocName) <> "" And blnSRCHFrom = False Then
            Call FillFlds(strDocName, strTblName, strMainFld)
        End If

        Call chkSel()
        Call getDsgrFlds()
        initGridStyles(Me)
        ToolTip.SetToolTip(Me.btnSave, "Save:Alt+S")
        ToolTip.SetToolTip(Me.btnCancel, "Close:Alt+O")
        If vsfDet.Rows.Count > 1 Then
            vsfDet.SetData(1, vDetSel, True)
        End If
    End Sub


    Public Sub FillFlds(ByVal argDocName As String, ByVal argTblName As String, ByVal argMainFld As String)
        Dim i As Integer
        Dim rs As oledb.oledbDataReader
        Dim strSQL As String
        strDocName = argDocName
        strTblName = argTblName

        strSQL = "Select * from " & argTblName & " where 1 <> 1"
        rs = clsSVT.getDataReaderFromSQL(strSQL)
        intchkClr = rs.FieldCount
        For i = 0 To rs.FieldCount - 1
            If UCase(argMainFld) <> UCase(rs.GetName(i)) Then
                If DataTypeIsString(rs.GetFieldType(i)) Then
                    If UCase(rs.GetName(i)) <> "IMGPRINT" Then
                        vsfDet.Rows.Add()
                        vsfDet.SetData(vsfDet.Rows.Count - 1, vDetFldCode, rs.GetName(i))
                    End If
                End If
            End If
        Next

        strSQL = "Select * from UtlSearchSettings where docName = '" & strDocName & "'"
        rs = clsSVT.getDataReaderFromSQL(strSQL)
        Do While rs.Read
            For i = 0 To vsfDet.Rows.Count - 1
                If UCase(vsfDet.GetData(i, vDetFldCode)) = UCase(rs("SearchFld")) Then
                    vsfDet.SetCellCheck(i, vDetSel, C1.Win.C1FlexGrid.CheckEnum.Checked)
                    Exit For
                End If
            Next
        Loop

    End Sub

    Public Sub FillSrchFormFlds(ByVal argDocName As String, ByVal argTblName As String, ByVal argMainFld As String)
        Dim i As Integer
        Dim rs As oledb.oledbDataReader
        Dim strSQL As String, strColName As String
        strDocName = argDocName
        strTblName = argTblName

        Dim ctrlList As New List(Of Control)
        Dim ctrl As Control = Nothing

        Do
            ctrl = argForm.GetNextControl(ctrl, True)

            If ctrl Is Nothing Then
                Exit Do
            End If
            If ctrl.GetType = GetType(C1FlexGrid) Then
                If ctrl.Tag = "View" Then ctrlList.Add(ctrl)
            End If
        Loop Until ctrl Is Nothing

        For Each argVsf As C1FlexGrid In ctrlList
            With argVsf
                For i = 0 To dblTotCol - 1
                    vsfDet.Rows.Add()
                    strColName = .GetData(4, i)
                    vsfDet.SetData(vsfDet.Rows.Count - 1, vDetFldCode, .GetData(4, i))
                    vsfDet.SetData(vsfDet.Rows.Count - 1, vDetFldCaption, .GetData(3, i))
                Next

                strSQL = "Select * from UtlSearchSettings where docName = '" & strDocName & "' "
                rs = clsSVT.getDataReaderFromSQL(strSQL)
                Do While rs.Read
                    For i = 0 To vsfDet.Rows.Count - 1
                        If UCase(vsfDet.GetData(i, vDetFldCode)) = UCase(rs("SearchFld")) Then
                            vsfDet.SetCellCheck(i, vDetSel, C1.Win.C1FlexGrid.CheckEnum.Checked)
                            Exit For
                        End If
                    Next
                Loop
            End With
        Next
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        blnSRCHFrom = False
        chkSelClr.Checked = False
        Me.Close()

    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click

        'Dim i As Integer
        'Dim strMobile As String = ""
        'With vsfDet
        '    If .Rows.Count > 1 Then
        '        For i = 1 To .Rows.Count - 1
        '            If .GetData(i, 0) = True Then
        '                strMobile &= .GetData(i, 1) & ","
        '            End If
        '        Next
        '    End If
        'End With
        'strMobile = Mid(strMobile, 1, Len(strMobile) - 1)
        'If blnBulkSMS = False Then
        '    argFrm.setMobileNo(strMobile)

        'Else
        '    argFrm.AddContacts(strMobile)
        'End If
        'Me.Close()


        ReArrangeSlNo(vsfDet, vDetSLNO)
        Dim i As Integer, strSQL As String, strFld As String = ""
        strSQL = "Delete from UtlSearchSettings where DocName = '" & strDocName & "'"
        clsSVT.execSQLStatement(strSQL)
        With vsfDet
            For i = 1 To .Rows.Count - 1
                If .GetCellCheck(i, vDetSel) = C1.Win.C1FlexGrid.CheckEnum.Checked Then
                    strSQL = "Insert into UtlSearchSettings (DocName, SearchFld) values ("
                    strSQL = strSQL & "'" & strDocName & "',"
                    strSQL = strSQL & "'" & .GetData(i, vDetFldCode) & "')"
                    'strSQL = strSQL & .GetData(i, vDetSLNO) & ")"
                    clsSVT.execSQLStatement(strSQL)
                    strFld = strFld & .GetData(i, vDetFldCode) & ","
                End If
            Next
        End With

        If strFld <> "" Then strFld = Mid(strFld, 1, Len(strFld) - 1)
        strSQL = "Delete from UtlSelCols where UserCode = '" & gUserCode & "' and FormTag = '" & strDocName & "'"
        clsSVT.execSQLStatement(strSQL)

        strSQL = "Insert into UtlSelCols (UserCode, FormTag, SelCols,FldsOrderBy) values ("
        strSQL = strSQL & "'" & gUserCode & "',"
        strSQL = strSQL & "'" & strDocName & "',"
        strSQL = strSQL & "'" & strFld & "',"
        strSQL = strSQL & "'" & getUpdtQry() & "')"
        'strSQL = strSQL & "'" & getFldCaption() & "')"

        clsSVT.execSQLStatement(strSQL)

        Call SaveFldCaptions()
        MsgBox("Saved Settings Successfully!", MsgBoxStyle.Information, Me.Text)
        blnSRCHFrom = False
        'Me.Close()


    End Sub
    Public Sub setMobileNo(ByVal argMobile As String)
        'If Trim(txtTo.Text) = "" Then
        '    txtTo.Text = txtTo.Text & argMobile
        'Else
        '    If Mid(txtTo.Text, Len(txtTo.Text), 1) = "," Then
        '        txtTo.Text = txtTo.Text & argMobile
        '    Else
        '        txtTo.Text = txtTo.Text & "," & argMobile
        '    End If
        'End If
        'If Trim(txtTo.Text) = "" Then Exit Sub
        'txtTo.Text = Mid(txtTo.Text, 1, Len(txtTo.Text) - 1)
        Dim strMobileNo() As String
        strMobileNo = Split(argMobile, ",")
        For Each strmob As String In strMobileNo
            frmTrnSMS.vsfContact.Rows.Count = frmTrnSMS.vsfContact.Rows.Count + 1
            frmTrnSMS.vsfContact.SetCellImage(frmTrnSMS.vsfContact.Rows.Count - 1, vDel, My.Resources.cancel)
            frmTrnSMS.vsfContact.SetData(frmTrnSMS.vsfContact.Rows.Count - 1, vTo, strmob)
            frmTrnSMS.vsfContact.SetData(frmTrnSMS.vsfContact.Rows.Count - 1, vName, clsSVT.getDataStringValue("Select CName from MstContact where Mob1 ='" & strmob & "'", "", False))
        Next
    End Sub
    Private Sub chkSelClr_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkSelClr.CheckedChanged
        Dim i As Integer
        If chkSelClr.Checked = True Then
            For i = 2 To vsfDet.Rows.Count - 1
                vsfDet.SetCellCheck(i, vDetSel, CheckEnum.Checked)
            Next
        Else
            For i = 2 To vsfDet.Rows.Count - 1
                vsfDet.SetCellCheck(i, vDetSel, CheckEnum.Unchecked)
            Next
        End If
    End Sub

    Private Sub chkSel()
        For i = 0 To vsfDet.Rows.Count - 1
            If vsfDet.GetCellCheck(i, vDetSel) = CheckEnum.Checked Then
                chkPrnt += 1
                ' chkSelClr.Checked = True
            End If
        Next

        If chkPrnt = vsfDet.Rows.Count - 1 Then
            chkSelClr.Checked = True
        End If
    End Sub

    Private Sub vsfDet_AfterScroll(sender As Object, e As C1.Win.C1FlexGrid.RangeEventArgs) Handles vsfDet.AfterScroll
        '  vsfDet.TopRow = e.NewRange.r1
    End Sub
    Private Sub vsfDet_BeforeEdit(sender As Object, e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfDet.BeforeEdit
        Select Case e.Col
            Case vDetSel
                e.Cancel = False

            Case vDetFldCode
                e.Cancel = True
        End Select
        If e.Row = 1 Then
            e.Cancel = True
        End If
        If vsfDet.GetData(e.Row, vDetDsgrFld) = "Y" Then
            e.Cancel = True
        End If
    End Sub

    Private Sub btnMoveUP_Click(sender As System.Object, e As System.EventArgs) Handles btnMoveUP.Click
        If vsfDet.Row > 2 Then
            Call MoveOneRowUp(vsfDet, vDetSLNO)
            ' vsfDet.Top = vsfDet.Row
        End If

        'With vsfDet
        '    If .Rows.Count > 15 Then
        '        Dim intTopRow As Integer = vsfDet.Row
        '        .TopRow = intTopRow
        '    End If
        'End With
    End Sub

    Private Sub btnMoveDown_Click(sender As System.Object, e As System.EventArgs) Handles btnMoveDown.Click
        If vsfDet.Row > 1 Then
            Call MoveOneRowDown(vsfDet, vDetSLNO)
            ' vsfDet.Top = vsfDet.Row
        End If
        'With vsfDet
        '    If .Rows.Count > 15 Then
        '        Dim intLastRow As Integer = vsfDet.Row
        '        .TopRow = intLastRow
        '    End If
        'End With
    End Sub

    Private Function getUpdtQry() As String

        Dim blnFound As Boolean, strQry As String
        Dim strFldList As String = ""
        Dim strCondList As String = ""
        Dim strFld As String = ""
        Dim i As Integer
        getUpdtQry = ""

        Call GenerateFldLst()
        strFldList = ""
        With vsfDet
            For i = 1 To .Rows.Count - 1

                Dim strFldName As String
                Select Case UCase(Trim(.GetData(i, vDetFldCode)))
                    Case "IDNO", "[ID No]"
                        strFldName = "IdentityNo"
                    Case "AMMENDMENT", "IMGPRINT", "AMMDETAILS", "AMMNO", "AMMDATE", "AUTHORISED", "AUTHDATE", "AUTHORISEDBY"
                        GoTo NxtFld
                    Case Else
                        strFldName = UCase((.GetData(i, vDetFldCode)))
                End Select

                If UCase$(.GetData(i, vDetFldCode)) <> "RGUID" Then
                    blnFound = False
                    If InStr(1, strDispFldList, UCase$(.GetData(i, vDetFldCode))) > 0 And strDocName <> "Item MasterVIEW" Then
                        strQry = "Select StorageFld from UtlFindFlds where TableName <> '" & strTblName & "' AND  " &
                                            " (DisplayFld ='" & .GetData(i, vDetFldCode) & "' OR " &
                                            " AliasFld1 ='" & .GetData(i, vDetFldCode) & "' OR " &
                                            " AliasFld2 ='" & .GetData(i, vDetFldCode) & "' OR " &
                                            " AliasFld3 ='" & .GetData(i, vDetFldCode) & "' OR " &
                                            " AliasFld4 ='" & .GetData(i, vDetFldCode) & "' OR " &
                                            " AliasFld5 ='" & .GetData(i, vDetFldCode) & "')"
                        strFld = UCase(clsSVT.getDataStringValue(strQry, "", blnFound))
                    End If

                    If blnFound = True Then
                        strFldList = strFldList & strFld & ","
                    Else
                        strFldList = strFldList & strFldName & ","
                    End If

                End If
NxtFld:
            Next i
        End With
        getUpdtQry = Mid(strFldList, 1, Len(strFldList) - 1)

    End Function
    Public Sub GenerateFldLst()
        Dim rsDummy As oledb.oledbDataReader

        rsDummy = clsSVT.getDataReaderFromSQL("Select * from UtlFindFlds")

        strDispFldList = ""
        With rsDummy
            If .HasRows Then
                Do While .Read
                    strDispFldList = strDispFldList & "'" & GetVal(!DisplayFld, "") & "',"
                    If GetVal(!AliasFld1, "") <> "" Then _
                        strDispFldList = strDispFldList & "'" & GetVal(!AliasFld1, "") & "',"
                    If GetVal(!AliasFld2, "") <> "" Then _
                        strDispFldList = strDispFldList & "'" & GetVal(!AliasFld2, "") & "',"
                    If GetVal(!AliasFld3, "") <> "" Then _
                        strDispFldList = strDispFldList & "'" & GetVal(!AliasFld3, "") & "',"
                    If GetVal(!AliasFld4, "") <> "" Then _
                        strDispFldList = strDispFldList & "'" & GetVal(!AliasFld4, "") & "',"
                    If GetVal(!AliasFld5, "") <> "" Then _
                        strDispFldList = strDispFldList & "'" & GetVal(!AliasFld5, "") & "',"
                Loop
            End If
        End With
        If strDispFldList <> "" Then strDispFldList = Mid(strDispFldList, 1, Len(strDispFldList) - 1)
    End Sub

    Public Function getFldCaption() As String
        Dim i As Integer = 0
        getFldCaption = ""
        With vsfDet
            For i = 1 To .Rows.Count - 1
                getFldCaption = getFldCaption & .GetData(i, vDetFldCode) & ":"
                If Trim(.GetData(i, vDetFldCaption)) <> "" Then
                    getFldCaption = getFldCaption & .GetData(i, vDetFldCaption) & ";"
                Else
                    getFldCaption = getFldCaption & .GetData(i, vDetFldCode) & ";"
                End If
            Next
        End With
    End Function

    Private Sub getDsgrFlds()
        Dim i As Integer
        Dim strSQL As String
        Dim rs As oledb.oledbDataReader
        strSQL = "Select FldCode from UtlDocDsgrFlds where DocId = " & intDocID
        rs = clsSVT.getDataReaderFromSQL(strSQL)

        With rs
            If Not .HasRows Then
                rs.Close()
                Exit Sub
            End If
            Do While .Read
                For i = 1 To vsfDet.Rows.Count - 1
                    If UCase(Trim(vsfDet.GetData(i, vDetFldCode))) = UCase(rs("FldCode")) And UCase(Strings.Right(Trim(vsfDet.GetData(i, vDetFldCode)), 2)) = "ID" Then
                        vsfDet.SetData(i, vDetDsgrFld, "Y")
                        Exit For
                    Else
                        vsfDet.SetData(i, vDetDsgrFld, "N")
                        Exit For
                    End If
                Next
            Loop
        End With
        rs.Close()
    End Sub

    Private Sub SaveFldCaptions()
        Dim i As Integer, blnFnd As Boolean, strSQL As String

        For i = 1 To vsfDet.Rows.Count - 1
            If Trim(vsfDet.GetData(i, vDetDsgrFld)) <> "Y" Then
                clsSVT.getDataStringValue("Select Code from UtlFldCaptions where Code='" & Trim(vsfDet.GetData(i, vDetFldCode)) & "'", "", blnFnd)
                If blnFnd Then
                    clsSVT.execSQLStatement("update UtlFldCaptions set Caption='" & Trim(vsfDet.GetData(i, vDetFldCaption)) & "' where Code='" & Trim(vsfDet.GetData(i, vDetFldCode)) & "'")
                Else
                    strSQL = "Insert into UtlFldCaptions (Code, Caption) values ("
                    strSQL = strSQL & "'" & Trim(vsfDet.GetData(i, vDetFldCode)) & "',"
                    strSQL = strSQL & "'" & Trim(vsfDet.GetData(i, vDetFldCaption)) & "')"
                    clsSVT.execSQLStatement(strSQL)
                End If
            End If
        Next

    End Sub

End Class