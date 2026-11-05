Imports System.Data.oledb
Imports C1.Win.C1FlexGrid
Imports System.IO
Public Class frmUtlFormSettings

    Dim clsSVT As New svtLibrary


    Const vDetUsrID = 0
    Const vDetUsrCode = 1
    Const vDetDocID = 2
    Const vDetSlno = 3
    Const vDetDocName = 4
    Const vDetHdr = 5
    Const vDetDtl = 6
    Const vDetFtr = 7
    Const vDetTot = 8
    Const vDetNewMode = 9
    Const vDetLoadLastTrn = 10
    Const vDetAfterSave = 11
    Const intTotCols = 12

    Private Sub frmUtlFormSettings_KeyUp(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyUp
        If e.KeyCode = Keys.F9 And e.Control Then
            '958, 579
            Call initFormSize(Me, "FormSettings", True, 662, 528, 0, 0)
            Call initFormVsfDet(Me, "FormSettings", vsfDet, "DET")
            MsgBox("Restored Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
        If e.KeyCode = Keys.F8 And e.Control = True Then
            Call SaveActFormSettings(Me, "FormSettings")
            Call SaveActFormVsfDet(Me, "FormSettings", "DET", vsfDet)
            MsgBox("Saved Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
    End Sub

    Private Sub frmUtlFormSettings_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        Call initFormSize(Me, "FormSettings", False, 662, 528, 0, 0)
        Call initFormVsfDet(Me, "FormSettings", vsfDet, "DET")
        With vsfDet
            .Cols.Count = intTotCols
            .Rows.Count = 1
            .SetData(0, vDetUsrCode, "User")
            .SetData(0, vDetDocName, "Document")
            .SetData(0, vDetHdr, "Header Grid")
            .SetData(0, vDetDtl, "Detail Grid")
            .SetData(0, vDetFtr, "Footer Grid")
            .SetData(0, vDetTot, "Total Grid")
            .SetData(0, vDetNewMode, "New Mode")
            .SetData(0, vDetLoadLastTrn, "Load Last Transaction")
            .SetData(0, vDetSlno, "SlNo")
            .SetData(0, vDetAfterSave, "After Save")
            For i = vDetHdr To intTotCols - 2
                .Cols(i).DataType = GetType(Boolean)
            Next

            .Cols(vDetAfterSave).ComboList = "-|New Doc|Save Doc|Clear Doc"

            Call SetHideCol(vsfDet, True, vDetUsrID, vDetUsrCode, vDetDocID)
            ' Call SetFlexColor(vsfDet, "", vDetUsrCode, vDetDocName)
            .AutoSizeCols()
        End With
        Me.Show()
        Call LoadDocuments()
        Call LoadFormSettings()

    End Sub

    Private Sub LoadDocuments()
        Dim strSQL As String = ""
        Dim rsDummy As oledb.oledbDataReader
        strSQL = "select DocID,DocName from utlTransactions where DocType='Transaction' order by DocName"
        Try
            rsDummy = clsSVT.getDataReaderFromSQL(strSQL)
            vsfDet.Rows.Count = 1
            With rsDummy
                Do While .Read
                    vsfDet.Rows.Count = vsfDet.Rows.Count + 1
                    vsfDet.SetData(vsfDet.Rows.Count - 1, vDetSlno, vsfDet.Rows.Count - 1)
                    vsfDet.SetData(vsfDet.Rows.Count - 1, vDetDocID, !DocID)
                    vsfDet.SetData(vsfDet.Rows.Count - 1, vDetDocName, !DocName)
                Loop
                .Close()
            End With
        Catch ex As Exception
            rsDummy = Nothing
            MsgBox(ex.Message, MsgBoxStyle.Critical, Me.Text)
        End Try
    End Sub

    Private Sub LoadFormSettings()
        Dim strSQL As String = ""
        Dim i As Integer = 0
        Dim rsDummy As oledb.oledbDataReader
        strSQL = "select f.UserID,f.DocID,UserCode,Header,Detail,Footer,Total,NewMode,LoadLastTrnNo, "
        strSQL &= " Case  AfterSave when '-' then '-' When 'N' then 'New Doc' when 'S' then 'Save Doc' when 'C' then 'Clear Doc' else '' end as AfterSave"
        strSQL &= " from UtlFormSettings f, MstUser  u"
        strSQL &= " where f.UserID = u.UserID"
        strSQL &= " And f.UserID = " & gUserID
        Try
            rsDummy = clsSVT.getDataReaderFromSQL(strSQL)
            With rsDummy
                Do While .Read
                    For i = 1 To vsfDet.Rows.Count - 1
                        If Val(vsfDet.GetData(i, vDetDocID)) = !DocID Then
                            vsfDet.SetData(vsfDet.Rows.Count - 1, vDetUsrID, !UserID)
                            vsfDet.SetData(vsfDet.Rows.Count - 1, vDetUsrCode, !UserCode)
                            If !Header = "Y" Then
                                vsfDet.SetCellCheck(i, vDetHdr, C1.Win.C1FlexGrid.CheckEnum.Checked)
                            Else
                                vsfDet.SetCellCheck(i, vDetHdr, C1.Win.C1FlexGrid.CheckEnum.Unchecked)
                            End If
                            If !Detail = "Y" Then
                                vsfDet.SetCellCheck(i, vDetDtl, C1.Win.C1FlexGrid.CheckEnum.Checked)
                            Else
                                vsfDet.SetCellCheck(i, vDetDtl, C1.Win.C1FlexGrid.CheckEnum.Unchecked)
                            End If
                            If !Footer = "Y" Then
                                vsfDet.SetCellCheck(i, vDetFtr, C1.Win.C1FlexGrid.CheckEnum.Checked)
                            Else
                                vsfDet.SetCellCheck(i, vDetFtr, C1.Win.C1FlexGrid.CheckEnum.Unchecked)
                            End If
                            If !Total = "Y" Then
                                vsfDet.SetCellCheck(i, vDetTot, C1.Win.C1FlexGrid.CheckEnum.Checked)
                            Else
                                vsfDet.SetCellCheck(i, vDetTot, C1.Win.C1FlexGrid.CheckEnum.Unchecked)
                            End If
                            If !NewMode = "Y" Then
                                vsfDet.SetCellCheck(i, vDetNewMode, C1.Win.C1FlexGrid.CheckEnum.Checked)
                            Else
                                vsfDet.SetCellCheck(i, vDetNewMode, C1.Win.C1FlexGrid.CheckEnum.Unchecked)
                            End If
                            If !LoadLastTrnNo = "Y" Then
                                vsfDet.SetCellCheck(i, vDetLoadLastTrn, C1.Win.C1FlexGrid.CheckEnum.Checked)
                            Else
                                vsfDet.SetCellCheck(i, vDetLoadLastTrn, C1.Win.C1FlexGrid.CheckEnum.Unchecked)
                            End If
                            vsfDet.SetData(i, vDetAfterSave, rsDummy!AfterSave)
                            Exit For
                        End If
                    Next i
                Loop
                .Close()
            End With
        Catch ex As Exception
            rsDummy = Nothing
            MsgBox(ex.Message, MsgBoxStyle.Critical, Me.Text)
        End Try
        vsfDet.AutoSizeCols()
    End Sub

    Private Sub btnCancel_Click(sender As System.Object, e As System.EventArgs) Handles btnCancel.Click
        LoadDocuments()
        LoadFormSettings()
        'Me.Close()
    End Sub

    Private Sub btnSave_Click(sender As System.Object, e As System.EventArgs) Handles btnSave.Click
        Dim strSQL As String = ""
        Dim svtTrn As oledb.oledbTransaction
        svtTrn = svtConn.BeginTransaction(IsolationLevel.ReadCommitted)
        Try
            clsSVT.execSQLStatement("Delete from UtlFormSettings where UserID=" & gUserID, svtTrn)
            For i = 1 To vsfDet.Rows.Count - 1
                If vsfDet.GetCellCheck(i, vDetHdr) = C1.Win.C1FlexGrid.CheckEnum.Checked Or
                    vsfDet.GetCellCheck(i, vDetDtl) = C1.Win.C1FlexGrid.CheckEnum.Checked Or
                    vsfDet.GetCellCheck(i, vDetFtr) = C1.Win.C1FlexGrid.CheckEnum.Checked Or
                    vsfDet.GetCellCheck(i, vDetTot) = C1.Win.C1FlexGrid.CheckEnum.Checked Or
                    vsfDet.GetCellCheck(i, vDetNewMode) = C1.Win.C1FlexGrid.CheckEnum.Checked Or
                    vsfDet.GetCellCheck(i, vDetLoadLastTrn) = C1.Win.C1FlexGrid.CheckEnum.Checked Or
                    vsfDet.GetCellCheck(i, vDetAfterSave) = C1.Win.C1FlexGrid.CheckEnum.Checked Then

                    strSQL = "Insert into UtlFormSettings(UserID,DocID,Header,Detail,Footer,Total,NewMode,LoadLastTrnNo,AfterSave"
                    strSQL &= ") values("
                    strSQL &= gUserID & ", "
                    strSQL &= Val(vsfDet.GetData(i, vDetDocID)) & ", "
                    If vsfDet.GetCellCheck(i, vDetHdr) = C1.Win.C1FlexGrid.CheckEnum.Checked Then
                        strSQL &= "'Y', "
                    Else
                        strSQL &= "'N', "
                    End If
                    If vsfDet.GetCellCheck(i, vDetDtl) = C1.Win.C1FlexGrid.CheckEnum.Checked Then
                        strSQL &= "'Y', "
                    Else
                        strSQL &= "'N', "
                    End If
                    If vsfDet.GetCellCheck(i, vDetFtr) = C1.Win.C1FlexGrid.CheckEnum.Checked Then
                        strSQL &= "'Y', "
                    Else
                        strSQL &= "'N', "
                    End If
                    If vsfDet.GetCellCheck(i, vDetTot) = C1.Win.C1FlexGrid.CheckEnum.Checked Then
                        strSQL &= "'Y', "
                    Else
                        strSQL &= "'N', "
                    End If
                    If vsfDet.GetCellCheck(i, vDetNewMode) = C1.Win.C1FlexGrid.CheckEnum.Checked Then
                        strSQL &= "'Y', "
                    Else
                        strSQL &= "'N', "
                    End If
                    If vsfDet.GetCellCheck(i, vDetLoadLastTrn) = C1.Win.C1FlexGrid.CheckEnum.Checked Then
                        strSQL &= "'Y', "
                    Else
                        strSQL &= "'N', "
                    End If

                    If vsfDet.GetData(i, vDetAfterSave) <> "" Then
                        Select Case vsfDet.GetData(i, vDetAfterSave)
                            Case "-"
                                strSQL = strSQL & "'-'"
                            Case "New Doc"
                                strSQL = strSQL & "'N'"
                            Case "Save Doc"
                                strSQL = strSQL & "'S'"
                            Case "Clear Doc"
                                strSQL = strSQL & "'C'"
                        End Select
                    Else
                        strSQL = strSQL & "'-'"
                    End If

                    'If GetVal(vsfDet.GetData(i, vDetAfterSave), "") <> "" Then
                    '    strSQL = strSQL & CStr(vsfDet.GetData(i, vDetAfterSave)).Substring(0, 1) & "'"
                    'End If
                    'strSQL &= Trim(vsfDet.GetData(i, vDetAfterSave)) & "'"
                    strSQL &= ")"
                    clsSVT.execSQLStatement(strSQL, svtTrn)
                End If
            Next
            svtTrn.Commit()
            MsgBox("User wise Form settings saved!!", MsgBoxStyle.Information, Me.Text)
        Catch ex As Exception
            svtTrn.Rollback()
            MsgBox(ex.Message, MsgBoxStyle.Information, Me.Text)
        End Try
    End Sub

    Private Sub vsfDet_AfterEdit(sender As Object, e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfDet.AfterEdit
        With vsfDet
            Select Case e.Col
                Case vDetHdr
                    If .GetCellCheck(e.Row, vDetHdr) = C1.Win.C1FlexGrid.CheckEnum.Checked Then
                        SetVsfChecked("N", vsfDet, e.Row, vDetDtl)
                        SetVsfChecked("N", vsfDet, e.Row, vDetFtr)
                        SetVsfChecked("N", vsfDet, e.Row, vDetTot)
                    End If
                Case vDetDtl
                    Dim intPartyPresent As String = ""
                    intPartyPresent = clsSVT.getDataLongValue("Select MstDocIDParty from UtlTransactions where DocID = " & Val(vsfDet.GetData(vsfDet.Row, vDetDocID)), 0, False)
                    If intPartyPresent > 0 Then
                        vsfDet.SetCellCheck(e.Row, vDetDtl, C1.Win.C1FlexGrid.CheckEnum.Unchecked)
                    End If
                    If .GetCellCheck(e.Row, vDetDtl) = C1.Win.C1FlexGrid.CheckEnum.Checked Then
                        SetVsfChecked("N", vsfDet, e.Row, vDetHdr)
                        SetVsfChecked("N", vsfDet, e.Row, vDetFtr)
                        SetVsfChecked("N", vsfDet, e.Row, vDetTot)
                    End If
                Case vDetFtr
                    If .GetCellCheck(e.Row, vDetFtr) = C1.Win.C1FlexGrid.CheckEnum.Checked Then
                        SetVsfChecked("N", vsfDet, e.Row, vDetDtl)
                        SetVsfChecked("N", vsfDet, e.Row, vDetHdr)
                        SetVsfChecked("N", vsfDet, e.Row, vDetTot)
                    End If
                Case vDetTot
                    If .GetCellCheck(e.Row, vDetTot) = C1.Win.C1FlexGrid.CheckEnum.Checked Then
                        SetVsfChecked("N", vsfDet, e.Row, vDetDtl)
                        SetVsfChecked("N", vsfDet, e.Row, vDetFtr)
                        SetVsfChecked("N", vsfDet, e.Row, vDetHdr)
                    End If
                Case vDetNewMode
                    If .GetCellCheck(e.Row, vDetNewMode) = C1.Win.C1FlexGrid.CheckEnum.Checked Then
                        SetVsfChecked("N", vsfDet, e.Row, vDetLoadLastTrn)
                    End If
                Case vDetLoadLastTrn
                    If .GetCellCheck(e.Row, vDetLoadLastTrn) = C1.Win.C1FlexGrid.CheckEnum.Checked Then
                        SetVsfChecked("N", vsfDet, e.Row, vDetNewMode)
                    End If
            End Select
        End With
    End Sub

    Private Sub vsfDet_BeforeEdit(sender As Object, e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfDet.BeforeEdit
        With vsfDet
            Select Case e.Col
                Case vDetUsrCode, vDetDocName, vDetSlno
                    e.Cancel = True
                Case Else
            End Select
        End With
    End Sub

    Private Sub btnClose_Click(sender As System.Object, e As System.EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub
End Class