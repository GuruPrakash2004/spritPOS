Imports System.Data.oledb
Imports System.IO
Imports C1.Win.C1FlexGrid

Public Class frmUtlSEShortClose
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
    Const vDetSPQ = 7
    Const vDetCustReqDate = 8
    Const vDetCommDate = 9
    Const vDetCostPrice = 10
    Const vDetSellPrice = 11
    Const vDetCPOQty = 12
    Const vDetCumCShpQty = 13
    Const vDetPendQty = 14
    Const vDetShortClosed = 15
    Const vDetShortClosedBy = 16
    Const vDetShortClosedDate = 17
    Const vDetShortCloseQty = 18
    Const vDetCPODtlID = 19
    Const intDetCols = 20

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
            .SetData(0, vDetSPQ, "SPQ")
            .SetData(0, vDetCustReqDate, "CustReqDate")
            .SetData(0, vDetCommDate, "CommDate")
            .SetData(0, vDetCPOQty, "CPOQty")
            .SetData(0, vDetCostPrice, "CostPrice")
            .SetData(0, vDetSellPrice, "SellPrice")
            .SetData(0, vDetCumCShpQty, "Cum Ship Qty")
            .SetData(0, vDetPendQty, "Pend Qty")
            .SetData(0, vDetShortClosed, "Shortclosed")
            .SetData(0, vDetShortClosedBy, "Shortclosed By")
            .SetData(0, vDetShortClosedDate, "Shortclosed Date")
            .SetData(0, vDetShortCloseQty, "Shortclosed Qty")


            .Cols(vDetSlNo).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.LeftCenter
            .Cols(vDetShortClosed).DataType = GetType(Boolean)
            Call SetColAlign(vsfDet, svtAlign.Left, vDetItemCode, vDetItemName)
            Call SetHideCol(vsfDet, True, vDetItemID, vDetCPODtlID)
            SetFlexColor(vsfDet, "", vDetSlNo, vDetItemCode, vDetItemName, vDetTariff, vDetCustPart, vDetSPQ, vDetCustReqDate, vDetCommDate, vDetCPOQty, vDetCostPrice, vDetSellPrice, vDetPendQty, vDetShortClosedDate, vDetShortClosedBy, vDetShortCloseQty, vDetShortClosed)
        End With
        Call initFormSize(Me, "CPOShortClose", False, 945, 480, 0, 0)
        initFormVsfDet(Me, "CPOShortClose", vsfDet, "DET")
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
            Call initFormSize(Me, "CPOShortClose", False, 945, 480, 0, 0)
            MsgBox("Restored Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
        If e.KeyCode = Keys.F8 And e.Control = True Then
            Call SaveActFormSettings(Me, "CPOShortClose")
            Call SaveActFormVsfDet(Me, "CPOShortClose", "DET", vsfDet)
            MsgBox("Saved Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
    End Sub

    Public Sub loadCPODtls(ByVal argobj As Object, argCPOID As Long, argCPONo As String, argCustPO As String, objVSF As C1FlexGrid)
        argForm = argobj
        Me.Text = "ShortClose of Customer Order - PO No." & argCustPO & " and CPO No." & argCPONo
        txtCPOID.Text = argCPOID
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
        strSQL &= " from  TrnhdrCPO h, TrnDtlCPO d, MstItem i, MstMake m   "
        strSQL &= " where d.CPOID = h.CPOID"
        strSQL &= " and d.ItemID=i.ItemID"
        strSQL &= " and i.MakeID = m.MakeID"
        strSQL &= " And h.CPOID = " & Val(txtCPOID.Text)
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
                .SetData(i, vDetCustPart, GetVal(Rs!CUSTPART, ""))
                .SetData(i, vDetSPQ, GetVal(Rs!SPQ, ""))
                .SetData(i, vDetCustReqDate, Rs!CUSTREQDT)
                .SetData(i, vDetCommDate, Rs!COMMDT)
                .SetData(i, vDetCostPrice, Rs!COSTPRICE)
                .SetData(i, vDetSellPrice, Rs!SELLPRICE)
                .SetData(i, vDetCPOQty, Rs!CPOQTY)
                .SetData(i, vDetCPODtlID, Rs!CPODtlID)

                strSQL = "select  sum(CshpQty) as CshpQty "
                strSQL &= " from TrnDtlCshp d, TrnHdrCSHP h"
                strSQL &= " where h.CSHPID=d.CSHPID "
                strSQL &= " And Cancelled='N'"
                strSQL &= " And CPOID = " & Val(txtCPOID.Text)
                strSQL &= " and ItemID= " & Val(vsfDet.GetData(i, vDetItemID))
                strSQL &= " and CustPart= '" & GetVal(vsfDet.GetData(i, vDetCustPart), "") & "'"
                strSQL &= " and CustReqDt= " & getDateFormatInCond(vsfDet.GetData(i, vDetCustReqDate))
                strSQL &= " and COMMDT= " & getDateFormatInCond(vsfDet.GetData(i, vDetCommDate))
                strSQL &= " and CostPrice= " & Val(vsfDet.GetData(i, vDetCostPrice))
                strSQL &= " and SellPrice= " & Val(vsfDet.GetData(i, vDetSellPrice))
                vsfDet.SetData(i, vDetCumCShpQty, clsSVT.getDataLongValue(strSQL, 0, False))

                ' .SetData(i, vDetCumCShpQty, Rs!CUMSHPQTY)

                .SetData(i, vDetPendQty, Val(.GetData(i, vDetCPOQty)) - Val(.GetData(i, vDetCumCShpQty)))
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
                    strSQL = "Update TrnDtlCPO set ShortClosed = "
                    If .GetCellCheck(i, vDetShortClosed) = C1.Win.C1FlexGrid.CheckEnum.Checked Then
                        strSQL = strSQL & "'Y',"
                strSQL &= " ShortClosedDt = '" & getDateFormatInInsert(.GetData(i, vDetShortClosedDate)) & "',"
                        strSQL &= " ShortClosedBy =' " & gUserCode & "',"
                        strSQL &= " CumShpQty = " & Val(.GetData(i, vDetCumCShpQty)) & ","
                        strSQL &= " ShortClosedQty = " & Val(.GetData(i, vDetShortCloseQty))
                    Else
                        strSQL = strSQL & "'N',"
                        strSQL &= " ShortClosedDT = Null,"
                        strSQL &= " ShortClosedBy = '',"
                        strSQL &= " CumShpQty = " & Val(.GetData(i, vDetCumCShpQty)) & ","
                        strSQL &= " ShortClosedQty = 0"
                    End If
                    strSQL &= " where CPODtlID = " & Val(.GetData(i, vDetCPODtlID))
                    Call clsSVT.execSQLStatement(strSQL)
                Next
            End With

            MsgBox("Updated Successfully!!", MsgBoxStyle.Information, Me.Text)

            With argForm
                .fillDtlFields(txtCPOID.Text)
            End With
            Me.Close()

        Catch ex As Exception
            MsgBox("Unable to save" & vbCrLf & ex.Message, MsgBoxStyle.Information, Me.Text)
        End Try
    End Sub

    Private Sub frmUtlSEShortClose_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub
End Class