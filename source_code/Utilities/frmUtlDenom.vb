Imports System.Data.oledb
Imports C1.Win.C1FlexGrid

Public Class frmUtlDenom
    Dim clsSvt As New svtLibrary

    Dim strSQL As String

    Const vDSlno = 0
    Const vDDCur = 1
    Const vDSel = 2
    Const vDNo = 3
    Const vDAmt = 4
    Const vDSaved = 5

    Private Sub btnConfig_Click(sender As System.Object, e As System.EventArgs) Handles btnConfig.Click
        If btnConfig.Text = "&Config ->" Then
            btnConfig.Text = "&Config <-"
            VisibelBtns(True)
            txtAmt.Text = ""
            'SetFlexColor(vsfDet, "", vDNo)
            'SetFlexColor(vsfDet, "B", vDDCur)
        Else
            btnConfig.Text = "&Config ->"
            VisibelBtns(False)
            'SetFlexColor(vsfDet, "B", vDNo)
            'SetFlexColor(vsfDet, "", vDDCur)
            fillDenoms()
        End If
    End Sub

    Private Sub frmUtlDenom_KeyUp(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyUp
        If e.KeyCode = Keys.F9 And e.Control Then
            Call initFormSize(Me, "UtlDenom", True, 493, 442, 0, 0)
            Call initFormVsfDet(Me, Me.Text, vsfDet, "DET")
            MsgBox("Restored Form Size Successfully!!", vbInformation, Me.Text)
        End If
        If e.KeyCode = Keys.F8 And e.Control = True Then
            Call SaveActFormSettings(Me, "UtlDenom")
            Call SaveActFormVsfDet(Me, "UtlDenom", "DET", vsfDet)
            MsgBox("Saved Form Size Successfully!!", vbInformation, Me.Text)
        End If
    End Sub

    Private Sub UtlDenom_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        With vsfDet
            .Rows.Count = 1
            .Cols.Count = 6
            .SetData(0, vDSlno, "Slno")
            .SetData(0, vDDCur, "Denomination")
            .SetData(0, vDSel, "De-Active")
            .SetData(0, vDNo, "No")
            .SetData(0, vDAmt, "Amount")
            .Cols(vDNo).DataType = GetType(Integer)
            .Cols(vDSlno).DataType = GetType(Integer)
            .Cols(vDSel).DataType = GetType(Boolean)
            SetFlexColor(vsfDet, "", vDDCur, vDAmt)
            .Cols(vDSaved).Visible = False
            .Cols(vDSaved).DataType = GetType(Boolean)
            .Rows(0).TextAlign = TextAlignEnum.CenterCenter
        End With
        If UCase(gUserCode) = "ADMIN" Then btnConfig.Visible = True
        Call fillDenoms()
        vsfDet.AutoSizeCols()
        SetColWidth(vsfDet, 50, 100, 100, 80, 100)
        ' Call initFormVsfDet(Me, "UtlDenom", vsfDet, "DET")
        Call initFormSize(Me, "UtlDenom", False, 493, 442, 0, 0)
    End Sub

    Private Sub VisibelBtns(ByVal argBln As Boolean)
        btnAdd.Visible = argBln
        btnRemoveRow.Visible = argBln
        btnSave.Visible = argBln
        btnMoveDown.Visible = argBln
        btnMoveUp.Visible = argBln
    End Sub

    Private Sub btnMoveUp_Click(sender As System.Object, e As System.EventArgs) Handles btnMoveUp.Click
        If vsfDet.Row <> vsfDet.Rows.Count - 1 Then
            Call MoveOneRowUp(vsfDet, -1)
            ReArrangeSlNo(vsfDet, vDSlno)
        End If
    End Sub

    Private Sub btnMoveDown_Click(sender As System.Object, e As System.EventArgs) Handles btnMoveDown.Click
        If vsfDet.Row <> vsfDet.Rows.Count - 1 Then
            Call MoveOneRowDown(vsfDet, -1)
            ReArrangeSlNo(vsfDet, vDSlno)
        End If
    End Sub

    Private Sub btnAdd_Click(sender As System.Object, e As System.EventArgs) Handles btnAdd.Click
        If vsfDet.Row <> vsfDet.Rows.Count - 1 Then
            vsfDet.Rows.Count = vsfDet.Rows.Count + 1
            vsfDet.SetData(vsfDet.Rows.Count - 1, vDSlno, Val(vsfDet.GetData(vsfDet.Rows.Count - 2, vDSlno)) + 1)
            vsfDet.SetData(vsfDet.Rows.Count - 1, vDSaved, True)
        End If
    End Sub

    Private Sub fillDenoms()
        Dim rs As oledb.oledbDataReader

        rs = clsSvt.getDataReaderFromSQL("Select * from UtlDenom order by Slno")

        vsfDet.Rows.Count = 1
        With rs
            If Not .HasRows Then
                'vsfDet.Rows.Count = vsfDet.Rows.Count + 1
                'vsfDet.SetData(vsfDet.Rows.Count - 1, vDDCur, "Coins")
                'Exit Sub
            End If
            Do While .Read
                vsfDet.Rows.Count = vsfDet.Rows.Count + 1
                vsfDet.SetData(vsfDet.Rows.Count - 1, vDSlno, !Slno)
                vsfDet.SetData(vsfDet.Rows.Count - 1, vDDCur, !DCur)
                vsfDet.SetData(vsfDet.Rows.Count - 1, vDSel, IIf(!InActive = "N", False, True))
                vsfDet.SetData(vsfDet.Rows.Count - 1, vDSaved, False)
            Loop
            'vsfDet.Rows.Count = vsfDet.Rows.Count + 1
            'vsfDet.SetData(vsfDet.Rows.Count - 1, vDDCur, "Coins")
        End With

    End Sub

    Private Sub calTotal()
        With vsfDet
            For i = 1 To .Rows.Count - 2
                If i <> vsfDet.Rows.Count - 1 Then
                    .SetData(i, vDAmt, Val(.GetData(i, vDDCur)) * Val(.GetData(i, vDNo)))
                Else
                    .SetData(i, vDAmt, Val(.GetData(i, vDNo)))
                End If
            Next
            txtAmt.Text = 0.0
            For i = 1 To .Rows.Count - 1
                txtAmt.Text = Val(txtAmt.Text) + Val(.GetData(i, vDAmt))

            Next
        End With
    End Sub

    Private Sub vsfdet_AfterEdit(sender As Object, e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfDet.AfterEdit
        Call calTotal()
    End Sub

    Private Sub vsfdet_BeforeEdit(sender As Object, e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfDet.BeforeEdit
        With vsfDet
            If e.Col = vDSlno Or e.Col = vDAmt Then
                e.Cancel = True
            End If

            If e.Col = vDDCur And vsfDet.GetData(e.Row, vDSaved) = False Then
                e.Cancel = True
            End If

            'If btnAdd.Visible = False Then
            '    If e.Col = vDDCur Or e.Col = vDAmt Then
            '        e.Cancel = True
            '    End If
            'Else
            '    If e.Col = vDNo Or e.Col = vDAmt Then
            '        e.Cancel = True
            '    End If
            '    If e.Col = vDDCur And vsfDet.Row = vsfDet.Rows.Count - 1 Then
            '        e.Cancel = True
            '    End If
            'End If
        End With

    End Sub

    Private Sub vsfDet_KeyPressEdit(sender As Object, e As C1.Win.C1FlexGrid.KeyPressEditEventArgs) Handles vsfDet.KeyPressEdit
        If e.Col = vDNo Or e.Col = vDDCur Then
            If Not Char.IsDigit(e.KeyChar) Then
                e.Handled = True
            End If
        End If
    End Sub

    Private Sub btnSave_Click(sender As Object, e As System.EventArgs) Handles btnSave.Click
        If validFlds Then
            If saveData Then
                btnConfig_Click(sender, e)
            End If
        End If
    End Sub
    Private Function validFlds() As Boolean
        validFlds = False
        With vsfDet
            For i = 1 To .Rows.Count - 2
                If Val(.GetData(i, vDDCur)) <= 0 Then
                    MsgBox("Denomination should be greater than zero!!", vbInformation, Me.Text)
                    Exit Function
                End If
            Next
        End With
        validFlds = True
    End Function

    Private Function saveData() As Boolean
        Dim svtTrn As oledb.oledbTransaction

        saveData = False
        svtTrn = svtConn.BeginTransaction(IsolationLevel.ReadCommitted)
        Try

            clsSvt.execSQLStatement("Delete from UtlDenom", svtTrn)
            With vsfDet
                For i = 1 To .Rows.Count - 1
                    strSQL = "Insert into UtlDenom(Slno,DCur,InActive) values(" & Val(.GetData(i, vDSlno)) & "," & Val(.GetData(i, vDDCur)) & "," & IIf(.GetData(i, vDSel) = False, "'N'", "'Y'") & ")"
                    clsSvt.execSQLStatement(strSQL, svtTrn)
                Next
            End With
            saveData = True
            svtTrn.Commit()
        Catch ex As Exception
            MsgBox(ex.Message, vbCritical, Me.Text)
            svtTrn.Rollback()
        End Try
        saveData = True
    End Function
   
    Private Sub btnRemoveRow_Click(sender As System.Object, e As System.EventArgs) Handles btnRemoveRow.Click
        With vsfDet
            If Val(.GetData(.Row, vDDCur)) > 0 Then

                Dim blnResult As Boolean = False
                'clsSvt.getDataLongValue("Select h.trnid from trnsbdepdenom d,trnhdrsbdep h where d.trnid=h.trnid and h.cancelled='N' and denom=" & Val(.GetData(.Row, vDDCur)), 0, blnResult)
                'If blnResult = True Then
                '    MsgBox("The selected denomination is available in deposit entry.", MsgBoxStyle.Information, "Deletion not possible")
                '    Exit Sub
                'End If
                'clsSvt.getDataLongValue("Select h.trnid from trnsbwddenom d,trnhdrsbwd h where d.trnid=h.trnid and h.cancelled='N' and denom=" & Val(.GetData(.Row, vDDCur)), 0, blnResult)
                'If blnResult = True Then
                '    MsgBox("The selected denomination is available in withdrawal entry.", MsgBoxStyle.Information, "Deletion not possible")
                '    Exit Sub
                'End If
                .Rows.Remove(.Row)
            End If
        End With
    End Sub

End Class


