Imports System.Data.oledb
Imports System.IO
Imports C1.Win.C1FlexGrid

Public Class frmUtlDSEShortClose
    Dim intTotRec As Integer

    Dim clsSVT As New svtLibrary
    Dim strSQL As String

    Dim strDocStdDtlWidth As String

    Const vDetSlNo = 0
    Const vDetItemID = 1
    Const vDetItemCode = 2
    Const vDetItemName = 3
    Const vDetTariff = 4
    Const vDetMake = 5
    Const vDetCustPart = 6
    Const vDetSPQQty = 7
    Const vDetCommDate = 8
    Const vDetDPOQty = 9
    Const vDetCumDShpQty = 10
    Const vDetPendQty = 11
    Const vDetShortClosed = 12
    Const vDetShortClosedBy = 13
    Const vDetShortClosedDate = 14
    Const vDetShortCloseQty = 15
    Const vDetDPODtlID = 16
    Const intDetCols = 17

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
            .SetData(0, vDetItemCode, "ItemCode")
            .SetData(0, vDetItemName, "ItemName")
            .SetData(0, vDetTariff, "Tariff")
            .SetData(0, vDetMake, "Make")
            .SetData(0, vDetCustPart, "CustPart")
            .SetData(0, vDetSPQQty, "SPQ Qty")
            .SetData(0, vDetCommDate, "CommDate")
            .SetData(0, vDetDPOQty, "DPOQty")
            .SetData(0, vDetCumDShpQty, "Cum Ship Qty")
            .SetData(0, vDetPendQty, "Pend Qty")
            .SetData(0, vDetShortClosed, "Shortclosed")
            .SetData(0, vDetShortClosedBy, "Shortclosed By")
            .SetData(0, vDetShortClosedDate, "Shortclosed Date")
            .SetData(0, vDetShortCloseQty, "Shortclosed Qty")


            .Cols(vDetSlNo).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.LeftCenter
            .Cols(vDetShortClosed).DataType = GetType(Boolean)
            Call SetColAlign(vsfDet, svtAlign.Left, vDetItemCode, vDetItemName)
            Call SetHideCol(vsfDet, True, vDetItemID, vDetDPODtlID)
            SetFlexColor(vsfDet, "", vDetSlNo, vDetItemCode, vDetItemName, vDetTariff, vDetCustPart, vDetCommDate, vDetDPOQty, vDetPendQty, vDetShortClosedDate, vDetShortClosedBy, vDetShortCloseQty, vDetShortClosed)
        End With
        Call initFormSize(Me, "DPOShortClose", False, 945, 480, 0, 0)
        initFormVsfDet(Me, "DPOShortClose", vsfDet, "DET")
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

    Private Sub frmUtlSEShortClose_KeyUp(sender As Object, e As KeyEventArgs) Handles Me.KeyUp
        If e.KeyCode = Keys.Escape Then
            Call btnCancel.PerformClick()
        End If
        If e.KeyCode = Keys.F9 And e.Control = True Then
            Call initFormSize(Me, "DPOShortClose", False, 945, 480, 0, 0)
            MsgBox("Restored Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
        If e.KeyCode = Keys.F8 And e.Control = True Then
            Call SaveActFormSettings(Me, "DPOShortClose")
            Call SaveActFormVsfDet(Me, "DPOShortClose", "DET", vsfDet)
            MsgBox("Saved Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
    End Sub

    Public Sub loadCPODtls(ByVal argobj As Object, argDPOID As Long, argDPONo As String, argCustPO As String, objVSF As C1FlexGrid)
        argForm = argobj
        Me.Text = "ShortClose of Diversion PO - DPO No." & argCustPO & " and DPO No." & argDPONo
        txtDPOID.Text = argDPOID
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

        strSQL = "Select d.*, i.ItemCode, i.ItemName, m.MakeCode, i.TariffNo "
        strSQL &= " from  TrnhdrDPO h, TrnDtlDPO d, MstItem i, MstMake m   "
        strSQL &= " where d.DPOID = h.DPOID"
        strSQL &= " and d.ItemID=i.ItemID"
        strSQL &= " and i.MakeID = m.MakeID"
        strSQL &= " And h.DPOID = " & Val(txtDPOID.Text)
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
                .SetData(i, vDetItemID, GetVal(Rs!ItemID, 0))
                .SetData(i, vDetItemCode, GetVal(Rs!ItemCode, ""))
                .SetData(i, vDetItemName, GetVal(Rs!ItemName, ""))
                .SetData(i, vDetTariff, GetVal(Rs!TariffNo, ""))
                .SetData(i, vDetMake, GetVal(Rs!MakeCode, ""))
                .SetData(i, vDetCustPart, GetVal(Rs!CUSTPARTNo, ""))
                .SetData(i, vDetCommDate, Rs!COMMDATE)
                .SetData(i, vDetDPOQty, Rs!DPOQty)
                .SetData(i, vDetDPODtlID, Rs!DPODtlID)

                strSQL = "select  sum(SEPOQty) as SEPOQty "
                strSQL &= " from TrnDtlSEPO d, TrnHdrSEPO h"
                strSQL &= " where h.SEPOID=d.SEPOID "
                strSQL &= " And Cancelled='N'"
                strSQL &= " And DPOID = " & Val(txtDPOID.Text)
                strSQL &= " and ItemID= " & Val(vsfDet.GetData(i, vDetItemID))
                strSQL &= " and CustPartNo= '" & GetVal(vsfDet.GetData(i, vDetCustPart), "") & "'"
                '    strSQL &= " and COMMDATE= " & getDateFormatInCond(vsfDet.GetData(i, vDetCommDate))
                vsfDet.SetData(i, vDetCumDShpQty, clsSVT.getDataLongValue(strSQL, 0, False))

                ' .SetData(i, vDetCumDShpQty, Rs!CUMSHPQTY)

                .SetData(i, vDetPendQty, Val(.GetData(i, vDetDPOQty)) - Val(.GetData(i, vDetCumDShpQty)))
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

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click

        Try
            With vsfDet
                For i = 1 To .Rows.Count - 1
                    strSQL = "Update TrnDtlDPO set ShortClosed = "
                    If .GetCellCheck(i, vDetShortClosed) = C1.Win.C1FlexGrid.CheckEnum.Checked Then
                        strSQL = strSQL & "'Y',"
                        strSQL &= " ShortClosedDt = '" & getDateFormatInInsert(.GetData(i, vDetShortClosedDate)) & "',"
                        strSQL &= " ShortClosedBy =' " & gUserCode & "',"
                        strSQL &= " CumDShpQty = " & Val(.GetData(i, vDetCumDShpQty)) & ","
                        strSQL &= " ShortClosedQty = " & Val(.GetData(i, vDetShortCloseQty))
                    Else
                        strSQL = strSQL & "'N',"
                        strSQL &= " ShortClosedDT = Null,"
                        strSQL &= " ShortClosedBy = '',"
                        strSQL &= " CumDShpQty = " & Val(.GetData(i, vDetCumDShpQty)) & ","
                        strSQL &= " ShortClosedQty = 0"
                    End If
                    strSQL &= " where DPODtlID = " & Val(.GetData(i, vDetDPODtlID))
                    Call clsSVT.execSQLStatement(strSQL)
                Next
            End With

            MsgBox("Updated Successfully!!", MsgBoxStyle.Information, Me.Text)

            With argForm
                .fillDtlFields(txtDPOID.Text)
            End With
            Me.Close()

        Catch ex As Exception
            MsgBox("Unable to save" & vbCrLf & ex.Message, MsgBoxStyle.Information, Me.Text)
        End Try
    End Sub

    Private Sub frmUtlSEShortClose_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub
End Class