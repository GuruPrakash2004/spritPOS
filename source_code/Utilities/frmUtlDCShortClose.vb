Imports System.Data.oledb
Imports System.IO
Imports C1.Win.C1FlexGrid

Public Class frmUtlDCShortClose
    Dim intTotRec As Integer

    Dim clsSVT As New svtLibrary
    Dim strSQL As String

    Dim strDocStdDtlWidth As String

    Const vDetSlNo = 0
    Const vdetItemID = 1
    Const vDetDCDtlID = 2
    Const vDetItemCode = 3
    Const vDetItemName = 4
    Const vDetTariff = 5
    Const vDetMake = 6
    Const vDetDCQty = 7
    Const vDetCumDCRQty = 8
    Const vDetPendQty = 9
    Const vDetShortClosed = 10
    Const vDetShortClosedDate = 11
    Const vDetShortClosedBy = 12
    Const vDetShortCloseQty = 13
    Const intDetCols = 14

    Public argForm As Object
    Public argVSF As C1.Win.C1FlexGrid.C1FlexGrid

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub initScreen()
        Me.KeyPreview = True
        With vsfDet
            .Cols.Count = intDetCols
            .Rows.Count = 2
            .SetData(0, vDetSlNo, "Sl No")
            .SetData(0, vDetItemCode, "Item Code")
            .SetData(0, vDetItemName, "Item Name")
            .SetData(0, vDetTariff, "Tariff")
            .SetData(0, vDetDCQty, "DC Qty")
            .SetData(0, vDetMake, "MakeCode")
            .SetData(0, vDetCumDCRQty, "Cum DCR Qty")
            .SetData(0, vDetPendQty, "Pend Qty")
            .SetData(0, vDetShortClosed, "Shortclosed")
            .SetData(0, vDetShortClosedBy, "Shortclosed By")
            .SetData(0, vDetShortClosedDate, "Shortclosed Date")
            .SetData(0, vDetShortCloseQty, "Shortclosed Qty")

            .Cols(vDetSlNo).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.LeftCenter
            .Cols(vDetShortClosed).DataType = GetType(Boolean)
            Call SetColAlign(vsfDet, svtAlign.Left, vDetItemCode, vDetItemName)
            Call SetHideCol(vsfDet, True, vdetItemID, vDetDCDtlID)
            SetFlexColor(vsfDet, "", vDetSlNo, vDetItemCode, vDetItemName, vDetMake, vDetDCQty, vDetTariff, vDetCumDCRQty, vDetPendQty, vDetShortClosedDate, vDetShortClosedBy, vDetShortCloseQty, vDetShortClosed)
        End With

        initFormVsfDet(Me, "POShortClose", vsfDet, "DET")
    End Sub



    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Try
            With vsfDet
                For i = 1 To .Rows.Count - 1
                    strSQL = "Update TrnDtlDC set ShortClosed = "
                    If .GetCellCheck(i, vDetShortClosed) = C1.Win.C1FlexGrid.CheckEnum.Checked Then
                        strSQL = strSQL & "'Y',"
                        strSQL &= " ShortClosedDt = '" & getDateFormatInInsert(.GetData(i, vDetShortClosedDate)) & "',"
                        strSQL &= " ShortClosedBy =' " & gUserCode & "',"
                        '  strSQL &= " CumGRNQty = " & Val(.GetData(i, vDetCumDCRQty)) & ","
                        strSQL &= " ShortClosedQty = " & Val(.GetData(i, vDetShortCloseQty))
                    Else
                        strSQL = strSQL & "'N',"
                        strSQL &= " ShortClosedDT = Null,"
                        strSQL &= " ShortClosedBy ='',"
                        '  strSQL &= " CumGRNQty = " & Val(.GetData(i, vDetCumDCRQty)) & ","
                        strSQL &= " ShortClosedQty = 0"
                    End If
                    strSQL &= " where DCDtlID = " & Val(.GetData(i, vDetDCDtlID))
                    Call clsSVT.execSQLStatement(strSQL)
                Next

            End With

            MsgBox("Updated Successfully!!", MsgBoxStyle.Information, Me.Text)

            With argForm
                .fillDtlFields(txtDCID.Text)
            End With
            Me.Close()

        Catch ex As Exception
            MsgBox("Unable to save" & vbCrLf & ex.Message, MsgBoxStyle.Information, Me.Text)
        End Try
    End Sub

    Private Sub frmUtlPOShortClose_KeyUp(sender As Object, e As KeyEventArgs) Handles Me.KeyUp
        If e.KeyCode = Keys.Escape Then
            Call btnCancel.PerformClick()
        End If
        If e.KeyCode = Keys.F9 And e.Control = True Then
            Call initFormSize(Me, "DCShortClose", False, 962, 456, 0, 0)
            MsgBox("Restored Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
        If e.KeyCode = Keys.F8 And e.Control = True Then
            Call SaveActFormSettings(Me, "DCShortClose")
            Call SaveActFormVsfDet(Me, "DCShortClose", "DET", vsfDet)
            MsgBox("Saved Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
    End Sub

    Public Sub loadDCDtls(ByVal argobj As Object, argPOID As Long, argPONo As String, objVSF As C1FlexGrid)
        argForm = argobj
        Me.Text = "ShortClose of Delivery Challan " & argPONo & " against DCR"
        txtDCID.Text = argPOID
        argVSF = objVSF

        initGridStyles(Me)
        Call initScreen()
        Call FillDocDetails()
    End Sub

    Private Sub FillDocDetails()

        Dim i As Integer, strSQL As String
        Dim Rs As oledb.oledbDataReader
        i = 0
        vsfDet.Rows.Count = 1

        strSQL = "Select d.* "
        strSQL &= " from  TrnhdrDC h, vwDC d   "
        strSQL &= " where d.DCID = h.DCID"
        strSQL &= " And h.DCID = " & Val(txtDCID.Text)
        strSQL &= " order by d.SlNo"

        Rs = clsSVT.getDataReaderFromSQL(strSQL)

        With vsfDet
            If Not Rs.HasRows Then
                Rs.Close()
                Exit Sub
            End If

            Do While Rs.Read()
                .Rows.Count = .Rows.Count + 1
                i = i + 1
                .SetData(i, vDetSlNo, GetVal(Rs!Slno, 0))
                .SetData(i, vDetItemCode, GetVal(Rs!ItemCode, ""))
                .SetData(i, vDetItemName, GetVal(Rs!ItemName, ""))
                .SetData(i, vDetTariff, GetVal(Rs!Tariff, ""))
                .SetData(i, vDetMake, GetVal(Rs!MakeCode, ""))
                .SetData(i, vDetDCDtlID, GetVal(Rs!DCDtlID, ""))
                .SetData(i, vDetDCQty, Rs!DCQTY)
                .SetData(i, vDetCumDCRQty, GetVal(Rs!CumDCRQty, ""))
                .SetData(i, vDetPendQty, Val(.GetData(i, vDetDCQty)) - Val(.GetData(i, vDetCumDCRQty)))
                If Rs!ShortClosed = "Y" Then
                    .SetCellCheck(i, vDetShortClosed, C1.Win.C1FlexGrid.CheckEnum.Checked)
                Else
                    .SetCellCheck(i, vDetShortClosed, C1.Win.C1FlexGrid.CheckEnum.Unchecked)
                End If
                .SetData(i, vDetShortCloseQty, GetVal(Rs!ShortClosedqty, 0))
                .SetData(i, vDetShortClosedDate, GetVal(Rs!ShortClosedDT, ""))
                .SetData(i, vDetShortClosedBy, clsSVT.getDataStringValue("Select username from Mstuser where userid = " & GetVal(Rs!ShortClosedBy, 0), "", False))
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
    Private Sub vsfDet_AfterEdit(sender As Object, e As RowColEventArgs) Handles vsfDet.AfterEdit
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
    Private Sub vsfDet_BeforeEdit(sender As Object, e As RowColEventArgs) Handles vsfDet.BeforeEdit
        If e.Col <> vDetShortClosed Then
            e.Cancel = True
        Else
            If Val(vsfDet.GetData(e.Row, vDetPendQty)) <= 0 Then
                e.Cancel = True
            End If
        End If
    End Sub

    Private Sub frmUtlDCShortClose_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub
End Class