Public Class frmTrnVchPend
    Const vDetSel = 0
    Const vDetAccID = 1
    Const vDetDocType = 2
    Const vDetTrnNo = 3
    Const vDetTrnDate = 4
    Const vDetTrnDueDate = 5
    Const vDetDocAmt = 6
    Const vDetCumAdjAmt = 7
    Const vDetBalAmt = 8
    Const vDetAmtType = 9
    Const vDetAdjAmt = 10

    Dim intAccLedID As Long
    Dim clsSVT As New svtLibrary
    Dim frmObj As Object


    Const vAdjAccLedID = 0
    Const vAdjAccID = 1
    Const vAdjAdjAmt = 2
    Const vAdjDrCr = 3
    Const vAdjType = 4
    Const vAdjNewRefNo = 5

    Dim lngAccID As Long
    Dim strshortform As String

    Private Sub frmTrnVchPend_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyUp
        If e.KeyCode = Keys.F8 And e.Control = True Then
            Call SaveActFormSettings(Me, "VCHPEND")
            Call SaveActFormVsfDet(Me, "VCHPEND", "DET", vsfDet)
            MsgBox("Saved Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
    End Sub

    Private Sub frmTrnVchPend_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Call initFormSize(Me, "VCHPEND", False, 958, 579, 0, 0)
        Call initFormVsfDet(Me, "VCHPEND", vsfDet, "DET")
    End Sub

    Public Sub LoadPendBills(ByVal argLedID As Long, ByVal argFRM As Form, ByVal argAccID As Long, ByVal argShForm As String, ByVal argVsfAdj As C1.Win.C1FlexGrid.C1FlexGrid)
        intAccLedID = argLedID
        frmObj = argFRM
        lngAccID = argAccID
        strshortform = argShForm
        Call initScreen()
        Call LoadPendData(argVsfAdj)
        Call calTotal()
    End Sub

    Private Sub initScreen()
        With vsfDet
            .Cols.Count = 11
            .Cols.Fixed = 0
            .Rows.Count = 1
            Call SetFlexColor(vsfDet, "", 1, 2, 3, 4, 5, 6, 7, 8, 9)
            .SetData(0, vDetAdjAmt, "Sel")
            .SetData(0, vDetDocType, "Doc Type")
            .Cols(vDetSel).DataType = GetType(Boolean)
            .SetData(0, vDetAdjAmt, "Adj. Amount")
            .SetData(0, vDetAmtType, "Dr/Cr")
            .SetData(0, vDetDocAmt, "Doc. Amount")
            .SetData(0, vDetBalAmt, "Bal. Amount")
            .SetData(0, vDetCumAdjAmt, "CumAdj. Amount")
            .SetData(0, vDetTrnDate, "Doc. Date")
            .SetData(0, vDetTrnDueDate, "Due Date")
            .SetData(0, vDetTrnNo, "Doc. No")
            Call SetColAlign(vsfDet, svtAlign.Right, vDetAdjAmt, vDetDocAmt, vDetBalAmt)

            Call SetHideCol(vsfDet, True, vDetAccID, vDetCumAdjAmt, vDetAmtType)

            .Cols(vDetAdjAmt).Format = "#.00"
            .Cols(vDetDocAmt).Format = "#.00"
        End With
        KeyPreview = True
    End Sub

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        Dim i As Integer, sngAdjAmt As Single
        If Not optNewRef.Checked Then
            If getNumberFromAccFormat(txtRemAmt.Text) <> 0 Then
                If MsgBox(txtRemAmt.Text & " is still un-Adjusted. " & vbCrLf & "Do you want to treat this as New-Reference Amt ?", MsgBoxStyle.Question + MsgBoxStyle.YesNo, Me.Text) = MsgBoxResult.No Then
                    optOnAccount.Checked = True
                Else
                    optNewRef.Checked = True
                    txtNewRef.Visible = True
                    txtNewRef.Focus()
                    If Trim(txtNewRef.Text) = "" Then
                        MsgBox("Please enter New reference number.", MsgBoxStyle.Information, Me.Text)
                        Exit Sub
                    End If
                End If
            End If
        End If

        Dim sngToAdj As Single
        sngToAdj = getNumberFromAccFormat(txtTOAdjust.Text)
lblstart:
        With frmObj.vsfadj
            For i = 1 To .rows.count - 1
                If .getdata(i, vAdjAccLedID) = intAccLedID Then
                    .removeitem(i)
                    GoTo lblStart
                End If
            Next
            If optNewRef.Checked Then
                If Math.Abs(getNumberFromAccFormat(txtRemAmt.Text)) > 0 Then
                    .setdata(0, 0, "N")
                    .setdata(0, 1, txtNewRef.Text)

                    .rows.count = .rows.count + 1
                    .setdata(.rows.count - 1, vAdjAccID, 0)
                    sngAdjAmt = getNumberFromAccFormat(txtRemAmt.Text)
                    If sngAdjAmt < 0 Then
                        .setdata(.rows.count - 1, vAdjDrCr, "Cr")
                    Else
                        .setdata(.rows.count - 1, vAdjDrCr, "Dr")
                    End If
                    .setdata(.rows.count - 1, vAdjType, "N")
                    .setdata(.rows.count - 1, vAdjNewRefNo, txtNewRef.Text)
                    .setdata(.rows.count - 1, vAdjAdjAmt, Math.Abs(sngAdjAmt))
                    .setdata(.rows.count - 1, vAdjAccLedID, intAccLedID)
                End If
            End If

            For i = 1 To vsfDet.Rows.Count - 1
                If Math.Abs(getNumberFromAccFormat(vsfDet.GetData(i, vDetAdjAmt))) > 0 Then
                    .rows.count = .rows.count + 1
                    .setdata(.rows.count - 1, vAdjAccID, vsfDet.GetData(i, vDetAccID))
                    sngAdjAmt = getNumberFromAccFormat(vsfDet.GetData(i, vDetAdjAmt))
                    If sngAdjAmt < 0 Then
                        .setdata(.rows.count - 1, vAdjDrCr, "Cr")
                    Else
                        .setdata(.rows.count - 1, vAdjDrCr, "Dr")
                    End If
                    .setdata(.rows.count - 1, vAdjType, "A")
                    .setdata(.rows.count - 1, vAdjAdjAmt, Math.Abs(sngAdjAmt))
                    .setdata(.rows.count - 1, vAdjAccLedID, intAccLedID)
                End If
            Next
        End With
        Me.Close()
    End Sub

    Private Sub LoadPendData(ByVal argVsfAdj As C1.Win.C1FlexGrid.C1FlexGrid)
        Dim cmd As New oledb.oledbCommand 
        Dim rs As oledb.oledbDataReader
        Try
            With cmd
                .Connection = svtConn
                .CommandText = "mSP_GetPendBills"
                .CommandType = CommandType.StoredProcedure
                .Parameters.Add("@AcLedID", OleDb.OleDbType.Integer).Value = intAccLedID
                .Parameters.Add("@AccID", OleDb.OleDbType.Integer).Value = lngAccID
                .CommandTimeout = 0
                rs = .ExecuteReader
            End With

        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, Me.Text)
            Exit Sub
        End Try

        vsfDet.Rows.Count = 1
        With rs
            If .HasRows Then
                Do While .Read
                    vsfDet.Rows.Count = vsfDet.Rows.Count + 1
                    vsfDet.SetData(vsfDet.Rows.Count - 1, vDetDocType, !DocType)
                    vsfDet.SetData(vsfDet.Rows.Count - 1, vDetTrnNo, !TrnNo)
                    vsfDet.SetData(vsfDet.Rows.Count - 1, vDetTrnDate, !TrnDate)
                    'If !BalAmt >= 0 Then
                    '    vsfDet.SetData(vsfDet.Rows.Count - 1, vDetAmtType, "Dr")
                    'Else
                    '    vsfDet.SetData(vsfDet.Rows.Count - 1, vDetAmtType, "Cr")
                    'End If
                    vsfDet.SetData(vsfDet.Rows.Count - 1, vDetAccID, !AccID)
                    vsfDet.SetData(vsfDet.Rows.Count - 1, vDetDocAmt, Format(!DocAmt, AccStringFmt))
                    vsfDet.SetData(vsfDet.Rows.Count - 1, vDetBalAmt, Format(!BalAmt, AccStringFmt))
                    vsfDet.SetData(vsfDet.Rows.Count - 1, vDetCumAdjAmt, !CumAdjAmt)
                Loop
            End If
            .Close()
        End With

        With argvsfadj
            For x = 1 To .Rows.Count - 1
                If intAccLedID = .GetData(x, vAdjAccLedID) Then
                    If .GetData(x, vAdjType) = "N" Then
                        optNewRef.Checked = True
                        txtNewRef.Text = .GetData(x, vAdjNewRefNo)
                        txtNewRef.Visible = True
                    End If
                    For i = 1 To vsfDet.Rows.Count - 1
                        If Val(vsfDet.GetData(i, vDetAccID)) = Val(.GetData(x, vAdjAccID)) Then
                            vsfDet.SetCellCheck(i, vDetSel, C1.Win.C1FlexGrid.CheckEnum.Checked)
                            If UCase(.GetData(x, vAdjDrCr)) = "DR" Then
                                vsfDet.SetData(i, vDetAdjAmt, Format(.GetData(x, vAdjAdjAmt), AccStringFmt))
                            Else
                                vsfDet.SetData(i, vDetAdjAmt, Format(-1 * Val(.GetData(x, vAdjAdjAmt)), AccStringFmt))
                            End If
                            Exit For
                        End If
                    Next
                End If
            Next x
        End With

    End Sub

    Private Sub btnClearAdj_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClearAdj.Click
        Dim i As Integer
        With vsfDet
            For i = 1 To .Rows.Count - 1
                .SetCellCheck(i, vDetSel, C1.Win.C1FlexGrid.CheckEnum.Unchecked)
                .SetData(i, vDetAdjAmt, "")
            Next
        End With
        Call calTotal()
    End Sub

    Private Sub vsfDet_AfterEdit(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfDet.AfterEdit
        Dim sngToAdj As Single
        Dim sngBalAmt As Single
        With vsfDet
            Select Case e.Col
                Case vDetSel
                    If .GetCellCheck(e.Row, vDetSel) = C1.Win.C1FlexGrid.CheckEnum.Checked Then
                        sngToAdj = getNumberFromAccFormat(txtRemAmt.Text)
                        sngBalAmt = getNumberFromAccFormat(.GetData(e.Row, vDetBalAmt))

                        If sngBalAmt / Math.Abs(sngBalAmt) <> sngToAdj / Math.Abs(sngToAdj) Then
                            If Math.Abs(sngBalAmt) < Math.Abs(sngToAdj) Then
                                .SetData(e.Row, vDetAdjAmt, Format(sngBalAmt, AccStringFmt))
                            Else
                                .SetData(e.Row, vDetAdjAmt, Format(Math.Abs(sngToAdj) * (sngBalAmt / Math.Abs(sngBalAmt)), AccStringFmt))
                            End If
                        Else
                            If Math.Abs(sngBalAmt) < Math.Abs(sngToAdj) Then
                                .SetData(e.Row, vDetAdjAmt, Format(sngBalAmt, AccStringFmt))
                            Else
                                .SetData(e.Row, vDetAdjAmt, Format(sngToAdj, AccStringFmt))
                            End If
                        End If
                    Else
                        .SetData(e.Row, vDetAdjAmt, 0.0)
                    End If
                    'vsfSummry.TextMatrix(1, vSumryAdjAmt) = Format(getVsfColTot(vsfPend, vPendAdjAmt), "0.00")

                Case vDetAdjAmt
                    .SetData(e.Row, vDetAdjAmt, .GetData(e.Row, vDetAdjAmt))
                    If Val(.GetData(e.Row, vDetAdjAmt)) > 0 Then
                        .SetCellCheck(e.Row, vDetSel, C1.Win.C1FlexGrid.CheckEnum.Checked)
                        'vsfSummry.TextMatrix(1, vSumryAdjAmt) = Format(getVsfColTot(vsfPend, vPendAdjAmt), "0.00")
                    Else
                        .SetCellCheck(e.Row, vDetSel, C1.Win.C1FlexGrid.CheckEnum.Unchecked)
                    End If
            End Select
        End With
        Call calTotal()
    End Sub

    Private Sub calTotal()
        Dim sngAdjTot As Single
        Dim i As Integer
        sngAdjTot = 0
        With vsfDet
            For i = 1 To .Rows.Count - 1
                sngAdjTot = sngAdjTot + getNumberFromAccFormat(.GetData(i, vDetAdjAmt))
            Next
        End With
        txtRemAmt.Text = Format(getNumberFromAccFormat(txtTOAdjust.Text) + sngAdjTot, AccStringFmt)
    End Sub

    Private Function getToAdj() As Single
        Dim i As Integer
        Dim sngDetGridAmt As Double, sngAdjAmt As Double
        sngDetGridAmt = getNumberFromAccFormat(txtTOAdjust.Text)
        getToAdj = 0
        sngAdjAmt = 0
        With vsfDet
            For i = 1 To .Rows.Count - 1
                sngAdjAmt = sngAdjAmt + getNumberFromAccFormat(.GetData(i, vDetAdjAmt))
            Next i
        End With
        getToAdj = sngDetGridAmt - sngAdjAmt
    End Function

    Private Sub optNewRef_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles optNewRef.CheckedChanged
        If optNewRef.Checked Then
            txtNewRef.Visible = True
            txtNewRef.Focus()
        End If
    End Sub

    Private Sub txtNewRef_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles txtNewRef.KeyPress, txtRemAmt.KeyPress, txtTOAdjust.KeyPress
        If e.KeyChar = "'" Then
            e.Handled = True
        End If
    End Sub

    Private Sub txtNewRef_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles txtNewRef.Validating
        Dim strVNo As String, strSQL As String
        strSQL = "select doctype + ' - ' + TrnNo  from trnhdracc h, trndtlaccadj a"
        strSQL = strSQL & " where(h.accid = a.accid)"
        strSQL = strSQL & " and acctype = 'V'"
        strSQL = strSQL & " and doctype = '" & strshortform & "'"
        strSQL = strSQL & " and newrefNo = '" & txtNewRef.Text & "'"
        strVNo = clsSVT.getDataStringValue(strSQL, "", False)
        If strVNo <> "" Then
            MsgBox("New reference number " & txtNewRef.Text & " is already used in " & strVNo, MsgBoxStyle.Critical, Me.Text)
            e.Cancel = True
        End If
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub
End Class