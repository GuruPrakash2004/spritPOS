Imports C1.Win.C1FlexGrid

Public Class frmUtlDelDupInvoice
    Dim clsSVT As New svtLibrary

    Const vDetInvID = 0
    Const vDetSel = 1
    Const vDetInvNo = 2
    Const vDetInvDate = 3
    Const vDetAddBy = 4
    Const vDetSalType = 5
    Const vDetBillSys = 6
    'Added by SUrabhi
    Const vDetCounter = 7
    Const vDetInvQty = 8
    Const vDetInvValue = 9
    Const vTotCols = 10

    Dim svtTrn As oledb.oledbTransaction

    Private Sub frmUtlDelCancel_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Call initFormVsfDet(Me, Me.Text, vsfView, "Invoice")
        Me.KeyPreview = True
        Call fillFormProperties(Me)
        Call initGridStyles(Me)
        Call initScreen()
    End Sub

    Private Sub initScreen()
        With vsfView
            .Rows.Count = 1
            .Cols.Count = vTotCols

            .SetData(0, vDetInvID, "Inv ID")
            .SetData(0, vDetSel, "Sel")
            .SetData(0, vDetInvNo, "Inv No")
            .SetData(0, vDetInvDate, "Inv Date")
            'Changed by Surabhi

            .SetData(0, vDetCounter, "Counter")
            .SetData(0, vDetAddBy, "Added By")
            .SetData(0, vDetSalType, "Sales Type")
            .SetData(0, vDetBillSys, "Bill System")
            .SetData(0, vDetInvQty, "Inv Qty")
            .SetData(0, vDetInvValue, "Inv Value")

            'SetFlexColorFullGrid(vsfView, "")
            'Changed by Surabhi to show invvalue and added by
            'SetFlexColor(vsfView, "", vDetInvNo)
            'SetHideCol(vsfView, True, vDetInvID, vDetInvDate, vDetAddBy, vDetSalType, vDetBillSys, vDetInvQty, vDetInvValue)
            SetFlexColor(vsfView, "", vDetInvNo, vDetInvDate, vDetInvQty, vDetInvValue, vDetCounter)
            SetHideCol(vsfView, True, vDetInvID, vDetSalType, vDetBillSys, vDetAddBy)


            .Cols(vDetSel).DataType = GetType(Boolean)
        End With
    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If vsfView.Rows.Count <= 1 Then
            MsgBox("No Records Found...", MsgBoxStyle.Exclamation, Me.Text)
            Exit Sub
        End If

        With vsfView
            Dim blnChecked As Boolean = False
            For x As Int32 = 1 To .Rows.Count - 1
                If .GetCellCheck(x, vDetSel) = C1.Win.C1FlexGrid.CheckEnum.Checked Then
                    blnChecked = True
                    Exit For
                End If
            Next

            If Not blnChecked Then
                MsgBox("Please select atleast one invoice...", MsgBoxStyle.Exclamation, Me.Text)
                Exit Sub
            End If

            ''the below code will not allow to delete if the doc date is less than or equal to freeze date in Gsettings
            'For i As Int32 = 1 To .Rows.Count - 1
            '    If .GetCellCheck(i, vDetSel) = C1.Win.C1FlexGrid.CheckEnum.Checked Then
            '        If CDate(.GetData(i, vDetInvDate)) <= gdtFreezeDate Then
            '            MsgBox("Documents cannot be Deleted. Some of the Document dates are before Freezed Date (Global Settings)!!", MsgBoxStyle.Critical, Me.Text)
            '            Exit Sub
            '        End If
            '    End If
            'Next

        End With

        If MsgBox("Selected Invoice Will Delete.... Do you want to Continue...?", MsgBoxStyle.YesNo, Me.Text) = MsgBoxResult.No Then Exit Sub

        Call RemoveInv()
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        If vsfView.Rows.Count <= 1 Then
            MsgBox("No Records Found...", MsgBoxStyle.Exclamation, Me.Text)
            Exit Sub
        End If

        With vsfView
            Dim blnChecked As Boolean = False
            For x As Int32 = 1 To .Rows.Count - 1
                If .GetCellCheck(x, vDetSel) = C1.Win.C1FlexGrid.CheckEnum.Checked Then
                    blnChecked = True
                    Exit For
                End If
            Next

            If Not blnChecked Then
                MsgBox("Please select atleast one invoice...", MsgBoxStyle.Exclamation, Me.Text)
                Exit Sub
            End If
        End With

        Call RemoveInv(False)
    End Sub

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        frmMain.CloseSelectedTabPage(Me)
    End Sub

    Private Sub frmUtlAuthDoc_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyUp
        If e.KeyCode = Keys.F9 And e.Control = True Then
            Call initFormVsfDet(Me, Me.Text, vsfView, "Invoice")
            MsgBox("Restored Form & Grid Layout Successfully!!", MsgBoxStyle.Information, Me.Text)
        End If
        If e.KeyCode = Keys.F8 And e.Control = True Then
            Call SaveActFormVsfDet(Me, Me.Text, "Invoice", vsfView)
            MsgBox("Saved Form & Grid Layout Successfully!!", MsgBoxStyle.Information, Me.Text)
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles btnLoad.Click
        txtInvNo.Text = ""
        Dim strSQL As String = ""

        'SCCounter added by Surabhi
        strSQL = "Select SINVID, SINVNO, SINVDate, H.AddedBy, BillSystem, TotQty, InvValue,SCCode as SalesCounter "
        strSQL = strSQL & " from TrnHdrSINV H, MSTSC S "
        strSQL = strSQL & " Where H.SCId =S.SCid and  Cancelled = 'N' and IsDelete = 'N'"
        strSQL = strSQL & " order by 1 desc"

        Dim objfrm As New frmUtlSelection
        With objfrm
            .blnMultiColSearch = True
            .FillViewVsf(Me, False, Nothing, strSQL, txtInvNo, 1)
            .Text = " Please Select Invoice"
            .ShowDialog()
            .Close()
        End With

        If Trim(txtInvNo.Text) <> "" Then
            Call btnView_Click(Nothing, Nothing)
        End If
    End Sub

    Private Sub btnView_Click(sender As Object, e As EventArgs) Handles btnView.Click

        If Trim(txtInvNo.Text) <> "" Then
            Dim intInvID As Int32 = clsSVT.getDataLongValue("Select SINVID From TrnHdrSINV Where SINVNO = '" & Trim(txtInvNo.Text) & "'", 0, False)
            If intInvID <= 0 Then
                MsgBox("Invalid Invoice...", MsgBoxStyle.Exclamation, Me.Text)
                Exit Sub
            End If
            Call FillInvoice(intInvID)
            Exit Sub
        End If

        Call FillInvoice()
    End Sub

    Private Sub FillInvoice(Optional ByVal argID As Int32 = 0)
        Dim strSQL As String = ""
        'Counter added by Surabhi
        strSQL = " Select H.*,S.SCCode From TrnHdrSINV H,MstSc S "
        strSQL &= " Where  H.SCID =S.SCId and Cancelled = 'N' "
        strSQL &= " and IsDelete = 'N' "
        If argID > 0 Then
            strSQL &= " and SINVID = " & argID
        Else
            strSQL &= " and SINVDate Between '" & getDateFormatInInsert(dtpFrom.Value) & "' "
            strSQL &= " and '" & getDateFormatInInsert(dtpTo.Value) & "' "
        End If
        strSQL &= " Order by SINVID "

        With vsfView
            .Rows.Count = 1

            Dim rs As oledb.oledbDataReader
            rs = clsSVT.getDataReaderFromSQL(strSQL)

            If Not rs.HasRows Then
                rs.Close()
                Exit Sub
            End If

            Do While rs.Read
                .Rows.Count = .Rows.Count + 1
                .SetData(.Rows.Count - 1, vDetInvID, rs("SINVID"))
                .SetData(.Rows.Count - 1, vDetSel, False)
                .SetData(.Rows.Count - 1, vDetInvNo, rs("SINVNO"))
                .SetData(.Rows.Count - 1, vDetInvDate, rs("SINVDate"))
                .SetData(.Rows.Count - 1, vDetAddBy, rs("AddedBy"))
                .SetData(.Rows.Count - 1, vDetSalType, rs("SalTyp"))
                .SetData(.Rows.Count - 1, vDetBillSys, rs("BillSystem"))
                'Added by Surabhi
                .SetData(.Rows.Count - 1, vDetCounter, rs("SCCode"))
                .SetData(.Rows.Count - 1, vDetInvQty, rs("TotQty"))
                .SetData(.Rows.Count - 1, vDetInvValue, rs("InvValue"))
            Loop
            rs.Close()
        End With
    End Sub

    Private Function RemoveInv(Optional ByVal blnDelete As Boolean = True) As Boolean
        RemoveInv = False

        Dim strSQL As String = ""

        Dim strErrMsg As String = ""
        svtTrn = svtConn.BeginTransaction(IsolationLevel.ReadCommitted)
        Try
            With vsfView
                For i As Int32 = 1 To .Rows.Count - 1
                    If .GetCellCheck(i, vDetSel) = C1.Win.C1FlexGrid.CheckEnum.Checked Then
                        If blnDelete Then
                            strSQL = "Delete from TrnHdrSINV where SINVID = " & Val(.GetData(i, vDetInvID))
                            If Not clsSVT.execSQLStatement(strSQL, svtTrn) Then
                                svtTrn.Rollback()
                                Exit Function
                            End If

                            strSQL = "Delete from TrnDtlSINV where SINVID = " & Val(.GetData(i, vDetInvID))
                            If Not clsSVT.execSQLStatement(strSQL, svtTrn) Then
                                svtTrn.Rollback()
                                Exit Function
                            End If

                            strSQL = "Delete from TrnDtlSinvDenom where SINVID = " & Val(.GetData(i, vDetInvID))
                            If Not clsSVT.execSQLStatement(strSQL, svtTrn) Then
                                svtTrn.Rollback()
                                Exit Function
                            End If
                        Else
                            strSQL = "Update TrnHdrSINV set IsDelete = 'Y', Cancelled = 'Y' where SINVID = " & Val(.GetData(i, vDetInvID))
                            If Not clsSVT.execSQLStatement(strSQL, svtTrn) Then
                                svtTrn.Rollback()
                                Exit Function
                            End If
                        End If

                        strSQL = "Delete from UtlStockList where DocType = 'INV' and DocID = " & Val(.GetData(i, vDetInvID))
                        strErrMsg = clsSVT.execSQLStmtMsg(strSQL, svtTrn)
                        If strErrMsg <> "" Then
                            If InStr(UCase(strErrMsg), "CHECK CONSTRAINT") > 0 Then
                                MsgBox("Stock Is going negative. Cannot Save.", MsgBoxStyle.Critical, "No Stock")
                            Else
                                MsgBox(strErrMsg & " Error during updating stock", MsgBoxStyle.Critical, "No Stock")
                            End If
                            svtTrn.Rollback()
                            Exit Function
                        End If
                    End If
                Next
            End With

            svtTrn.Commit()
            RemoveInv = True
            MsgBox("Data Removed successfully!", vbInformation, Me.Text)

            If Trim(txtInvNo.Text) = "" Then Call FillInvoice()
        Catch ex As Exception
            svtTrn.Rollback()
            MsgBox(ex.Message, MsgBoxStyle.Information, Me.Text)
        End Try
    End Function

    Private Sub vsfView_BeforeEdit(sender As Object, e As RowColEventArgs) Handles vsfView.BeforeEdit
        Select Case e.Col
            Case vDetSel
            Case Else
                e.Cancel = True
        End Select
    End Sub
End Class