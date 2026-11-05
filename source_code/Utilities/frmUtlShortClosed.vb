Imports System.Data.OleDb
Imports System.IO
Public Class frmUtlShortClosed

    Dim intTotRec As Integer

    Dim clsSVT As New svtLibrary
    Dim strSQL As String

    Dim strDocStdDtlWidth As String

    Const vDetSlNo = 0
    Const vDetDtlID = 1
    Const vDetItemID = 2
    Const vDetItemCode = 3
    Const vDetItemName = 4
    Const vDetFld1 = 5
    Const vDetFld2 = 6
    Const vDetFld3 = 7
    Const vDetFld4 = 8
    Const vDetFld5 = 9
    Const vDetFld6 = 10
    Const vDetFld7 = 11
    Const vDetFld8 = 12
    Const vDetFld9 = 13
    Const vDetQty = 14
    Const vDetCumQty = 15
    Const vDetPendQty = 16
    Const vDetShortClosed = 17
    Const vDetShortClosedDate = 18
    Const vDetShortClosedBy = 19
    Const vDetShortCloseQty = 20

    Const intDetCols = 21

    Public argForm As Object
    Dim strShForm As String
    Dim strTblName As String
    Dim strFldList As String

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub vsfDet_AfterEdit(sender As Object, e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfDet.AfterEdit
        If e.Col = vDetShortClosed Then
            If vsfDet.GetCellCheck(e.Row, vDetShortClosed) = C1.Win.C1FlexGrid.CheckEnum.Checked Then
                vsfDet.SetData(e.Row, vDetShortClosedDate, getSystemDate)
                vsfDet.SetData(e.Row, vDetShortClosedBy, gUserCode)
                vsfDet.SetData(e.Row, vDetShortCloseQty, vsfDet.GetData(e.Row, vDetPendQty))
            Else
                vsfDet.SetData(e.Row, vDetShortClosedDate, "")
                vsfDet.SetData(e.Row, vDetShortClosedBy, "")
                vsfDet.SetData(e.Row, vDetShortCloseQty, "0")
            End If
        End If
    End Sub

    Private Sub vsfDet_BeforeEdit(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfDet.BeforeEdit
        If e.Col <> vDetShortClosed Then
            e.Cancel = True
        Else
            If Val(vsfDet.GetData(e.Row, vDetPendQty)) <= 0 Then
                e.Cancel = True
            End If
        End If

    End Sub

    Private Sub frmUtlPOShortClose_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyUp
        If e.KeyCode = Keys.Escape Then
            Call btnCancel.PerformClick()
        End If
        If e.KeyCode = Keys.F9 And e.Control = True Then
            Call initFormSize(Me, "ShortClose", False, 980, 479, 0, 0)
            MsgBox("Restored Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
        If e.KeyCode = Keys.F8 And e.Control = True Then
            Call SaveActFormSettings(Me, "ShortClose")
            Call SaveActFormVsfDet(Me, "ShortClose", "DET", vsfDet)
            MsgBox("Saved Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
    End Sub

    Public Sub loadShortCloseDtls(ByVal argobj As Object, argID As Long, argNo As String, argCaption As String, argShForm As String, argVsf As C1.Win.C1FlexGrid.C1FlexGrid)
        argForm = argobj
        strShForm = argShForm
        Me.Text = "ShortClose of " & argCaption & ": " & argNo & " against Invoice"
        txtID.Text = argID
        strTblName = "TrnDtl" & strShForm



        initGridStyles(Me)
        Call initScreen()
        Call FillDocDetails()
    End Sub

    Private Sub initScreen()
        Me.KeyPreview = True

        With vsfDet
            .Cols.Count = intDetCols
            .Rows.Count = 2
            .SetData(0, vDetSlNo, "Sl No")
            .SetData(0, vDetItemCode, "ItemCode")
            .SetData(0, vDetItemName, "ItemName")
            .SetData(0, vDetShortClosed, "ShortClose")
            .SetData(0, vDetShortClosedDate, "ShortClose Date")
            .SetData(0, vDetShortClosedBy, "ShortClose By")
            .SetData(0, vDetQty, strShForm & " Qty")
            .SetData(0, vDetCumQty, "Cum Qty")
            .SetData(0, vDetShortCloseQty, "ShortClosed Qty")
            .SetData(0, vDetPendQty, "Pend Qty")

            .Cols(vDetSlNo).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.LeftCenter
            .Cols(vDetShortClosed).DataType = GetType(Boolean)
            Call SetColAlign(vsfDet, svtAlign.Left, vDetItemCode, vDetItemName)
            Call SetHideCol(vsfDet, True, vDetItemID, vDetDtlID, vDetFld1, vDetFld2, vDetFld3, vDetFld4, vDetFld5, vDetFld6, vDetFld7, vDetFld8, vDetFld9)
            SetFlexColor(vsfDet, "", vDetSlNo, vDetItemCode, vDetItemName, vDetShortClosed, vDetQty, vDetPendQty, vDetCumQty, vDetShortClosedBy, vDetShortClosedDate, vDetShortCloseQty)
        End With
        Call initFormSize(Me, "ShortClose", False, 980, 479, 0, 0)
        initFormVsfDet(Me, "ShortClose", vsfDet, "DET")
    End Sub

    Private Sub FillDocDetails()

        Dim i As Integer, strSQL As String
        Dim Rs As OleDb.OleDbDataReader

        i = 0
        vsfDet.Rows.Count = 1

StartFunctionAgain:
        strFldList = clsSVT.getDataStringValue("Select Flds from UtlShortClosedFlds where TblName = '" & strShForm & "'", "", False)

        If strFldList = "" Then
            If MsgBox("Short closed column is missing.... Click yes to proceed or form will be closed.", MsgBoxStyle.YesNo, Me.Text) = MsgBoxResult.No Then Me.Close()

            Dim obj As Form = frmUtlShortClosedFlds
            obj.ShowDialog()
            GoTo StartFunctionAgain
        End If

        strFldList = strFldList.Replace("|", ",")

        strSQL = " Select "
        strSQL &= strFldList & ", ItemCode, ItemName, (" & strShForm & "Qty - CumQty) As PendQty "
        strSQL &= " From MstItem i," & strTblName & " d "
        strSQL &= " Where i.ItemID=d.ItemID "
        strSQL &= " And d." & strShForm & "Id = " & Val(txtID.Text)
        strSQL &= " order by SlNo"

        Rs = clsSVT.getDataReaderFromSQL(strSQL)
        Me.Cursor = Cursors.WaitCursor


        With vsfDet
            Do While Rs.Read()
                .Rows.Count = .Rows.Count + 1
                i = i + 1
                .SetData(i, vDetSlNo, i)
                .SetData(i, vDetItemID, GetVal(Rs!ItemID, 0))
                .SetData(i, vDetItemCode, GetVal(Rs!ItemCode, ""))
                .SetData(i, vDetItemName, GetVal(Rs!ItemName, ""))
                .SetData(i, vDetDtlID, GetVal(Rs.Item(strShForm & "DtlID"), 0))
                .SetData(i, vDetQty, GetVal(Rs.Item(strShForm & "Qty"), 0))
                .SetData(i, vDetCumQty, GetVal(Rs!CumQty, 0))
                .SetData(i, vDetPendQty, GetVal(Rs!PendQty, 0))
                If Rs!ShortClosed = "Y" Then
                    .SetCellCheck(i, vDetShortClosed, C1.Win.C1FlexGrid.CheckEnum.Checked)
                End If
                .SetData(i, vDetShortCloseQty, GetVal(Rs!ShortClosedqty, 0))
                .SetData(i, vDetShortClosedDate, GetVal(Rs!ShortClosedDT, ""))
                .SetData(i, vDetShortClosedBy, clsSVT.getDataStringValue("Select Username from MstUser where UserID = " & GetVal(Rs!ShortClosedBy, 0), "", False))
                Dim intCol As Int32 = vDetFld1
                For k As Int32 = 0 To Rs.FieldCount - 1
                    Select Case UCase(Rs.GetName(k))
                        Case UCase(strShForm) & "ID", UCase(strShForm) & "DTLID", UCase(strShForm) & "QTY"
                        Case "ITEMID", "ITEMCODE", "ITEMNAME", "SLNO", "SHORTCLOSED", "SHORTCLOSEDBY", "SHORTCLOSEDDT", "SHORTCLOSEDQTY", "CUMQTY", "PENDQTY"
                        Case Else
                            .SetData(0, intCol, Rs.GetName(k))
                            .Cols(intCol).Visible = True
                            Call SetFlexColor(vsfDet, "", intCol)
                            .SetData(i, intCol, GetVal(Rs.Item(Rs.GetName(k)), ""))
                            intCol = intCol + 1
                    End Select
                Next
            Loop
            Rs.Close()
            btnSave.Enabled = False
            For i = 1 To .Rows.Count - 1
                If Val(.GetData(i, vDetPendQty)) > 0 Then
                    btnSave.Enabled = True
                    Exit For
                End If
            Next
        End With
        Me.Cursor = Cursors.Arrow

    End Sub


    Private Sub btnSave_Click(sender As System.Object, e As System.EventArgs) Handles btnSave.Click
        Dim blnFound As Boolean
        Dim svtTrn As OleDb.OleDbTransaction
        svtTrn = svtConn.BeginTransaction(IsolationLevel.ReadCommitted)
        Try
            With vsfDet
                blnFound = False

                For i = 1 To .Rows.Count - 1
                    strSQL = "Update " & strTblName & " set ShortClosed = "
                    If .GetCellCheck(i, vDetShortClosed) = C1.Win.C1FlexGrid.CheckEnum.Checked Then
                        strSQL = strSQL & "'Y',"
                strSQL &= " ShortClosedDt = '" & getDateFormatInInsert(.GetData(i, vDetShortClosedDate)) & "',"
                        strSQL &= " ShortClosedBy =' " & gUserID & "',"
                        strSQL &= " CumQty = " & Val(.GetData(i, vDetCumQty)) & ","
                        strSQL &= " ShortClosedQty = " & Val(.GetData(i, vDetShortCloseQty))
                    Else
                        strSQL = strSQL & "'N',"
                        strSQL &= " ShortClosedDT = Null,"
                        strSQL &= " ShortClosedBy = '',"
                        strSQL &= " CumQty = " & Val(.GetData(i, vDetCumQty)) & ","
                        strSQL &= " ShortClosedQty = 0"
                    End If
                    strSQL &= " where " & strShForm & "DtlID = " & Val(.GetData(i, vDetDtlID))

                    Call clsSVT.execSQLStatement(strSQL, svtTrn)
                Next
            End With
            svtTrn.Commit()
            svtTrn.Dispose()
            MsgBox("Updated Successfully!!", MsgBoxStyle.Information, Me.Text)

            With argForm
                .fillDtlFields(txtID.Text)
            End With
            Me.Close()

        Catch ex As Exception
            'svtTrn.Rollback()
            MsgBox("Unable to save" & vbCrLf & ex.Message, MsgBoxStyle.Information, Me.Text)
            Exit Sub
        End Try
    End Sub


End Class
