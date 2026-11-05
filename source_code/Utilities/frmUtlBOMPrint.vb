Imports System.Data.oledb

Public Class frmUtlBOMPrint
    Dim clsSVT As New svtLibrary
    Const vDetBOMID = 0
    Const vDetTItemCode = 1
    Const vDetItemCode = 2
    Const vDetItemName = 3
    Const vDetUOM = 4
    Const vDetQty = 5

    Public Sub InitScreen()
        With vsfDet
            .Rows.Count = 1
            .Cols.Count = 6
            .SetData(0, vDetTItemCode, "Tree Item Code")
            .SetData(0, vDetItemCode, "Item Code")
            .SetData(0, vDetItemName, "Item Name")
            .SetData(0, vDetUOM, "UOM")
            .SetData(0, vDetQty, "Qty")
            SetFlexColor(vsfDet, "", vDetTItemCode, vDetItemName, vDetItemCode, vDetUOM, vDetQty)
            SetColAlign(vsfDet, svtAlign.Right, vDetItemName)
            .Cols(vDetBOMID).Visible = False
        End With

    End Sub

    Private Sub btnClose_Click(sender As System.Object, e As System.EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub vsfDet_BeforeEdit(sender As Object, e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfDet.BeforeEdit
        e.Cancel = True
    End Sub

    Private Sub frmUtlBOMPrint_KeyUp(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyUp
        If e.KeyCode = Keys.F9 And e.Control Then
            Call initFormSize(Me, Me.Text, True, 737, 507, 0, 0)
            Call initFormVsfDet(Me, Me.Text, vsfDet, "DET")
            MsgBox("Restored Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
        If e.KeyCode = Keys.F8 And e.Control = True Then
            Call SaveActFormSettings(Me, Me.Text)
            Call SaveActFormVsfDet(Me, Me.Text, "DET", vsfDet)
            MsgBox("Saved Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
    End Sub

    Public Sub LoadBOM(argBOMID As Integer)
        Dim Strsql As String
        Dim i As Integer

        Call showWaitCursor()

        Strsql = "Select ItemCode,ItemName,h.* from MstHdrBOM h, MstItem i where i.ItemID=h.BOMItemID and BomID = " & argBOMID
        Dim groupDS As DataSet = addDS(Strsql)

        i = 0
        vsfDet.Rows.Count = 1
        For Each dr As DataRow In groupDS.Tables(0).Rows
            i = i + 1
            vsfDet.Rows.Count = vsfDet.Rows.Count + 1
            vsfDet.SetData(vsfDet.Rows.Count - 1, vDetBOMID, dr("BOMID"))
            vsfDet.SetData(vsfDet.Rows.Count - 1, vDetTItemCode, Strings.StrDup(i, "»") & dr("ItemCode").ToString)
            Call fillBOMItems(dr("BOMID"), i)
        Next
        vsfDet.AutoSizeCols()
        Call showNormalCursor()
    End Sub

    Private Function fillBOMItems(ByVal argBOMID As Integer, ByRef argIntRow As Integer) As Integer
        Dim Strsql As String
        Dim intItemBOMId As Integer, i As Integer = 0

        Strsql = "Select BOMID,i.ItemID, ItemCode, ItemName, UOMCode,Qty "
        Strsql &= " from MstDtlBOM d, MstItem i, MstUOM u "
        Strsql &= " where d.ItemID=i.ItemID"
        Strsql &= " and u.UOMID=i.UOMID"
        Strsql &= " and BOMID = " & argBOMID

        Dim ledgerDS As DataSet = addDS(Strsql)
        If ledgerDS.Tables(0).Rows.Count > 0 Then
            If argIntRow = 1 Then
                argIntRow = 5
            End If

            For Each dr As DataRow In ledgerDS.Tables(0).Rows
                vsfDet.Rows.Count = vsfDet.Rows.Count + 1
                vsfDet.SetData(vsfDet.Rows.Count - 1, vDetTItemCode, Strings.StrDup(argIntRow, " ") & " ∙" & dr("ItemCode"))
                vsfDet.SetData(vsfDet.Rows.Count - 1, vDetItemCode, dr("ItemCode").ToString)
                vsfDet.SetData(vsfDet.Rows.Count - 1, vDetItemName, dr("ItemName").ToString)
                vsfDet.SetData(vsfDet.Rows.Count - 1, vDetUOM, dr("UOMCode"))
                vsfDet.SetData(vsfDet.Rows.Count - 1, vDetQty, dr("Qty"))

                Strsql = "select BOMID from MstHdrBOM where BOMItemId = " & dr("ItemID")
                intItemBOMId = clsSVT.getDataLongValue(Strsql, 0, False)
                If intItemBOMId > 0 Then
                    fillBOMItems(intItemBOMId, argIntRow + 5)
                Else
                    argIntRow = argIntRow
                End If

            Next
            Return 1
        Else
            Return 0
        End If
        vsfDet.Rows.Count = vsfDet.Rows.Count - 1
    End Function

    Private Function addDS(ByVal strQry As String) As DataSet
        Dim strConn As String
        strConn = "Persist Security Info=False"
        strConn = strConn & ";User ID=" & gStrDBLoginName
        strConn = strConn & ";Pwd=" & gStrDBLoginPwd
        strConn = strConn & ";Initial Catalog=" & gStrDBName
        strConn = strConn & ";Data Source=" & gStrServerName
        Dim SqlConn As New oledbConnection(strConn)
        Dim da As New oledbDataAdapter(strQry, SqlConn)
        Dim ds As New DataSet()
        da.Fill(ds)
        Return ds
    End Function

    Private Sub btnExport_Click(sender As Object, e As EventArgs) Handles btnExport.Click
        Call ExportToXL(vsfDet)
    End Sub
End Class
