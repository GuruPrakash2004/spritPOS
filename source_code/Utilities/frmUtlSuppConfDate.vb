Imports System.ComponentModel
Imports C1.Win.C1FlexGrid

Public Class frmUtlSuppConfDate
    Dim clsSVT As New svtLibrary

    Public intDocID As Int32

    Const vDetSlNo = 0
    Const vDetItemID = 1
    Const vDetItemCode = 2
    Const vDetItemName = 3
    Const vDetUOMCODE = 4
    Const vDetQty = 5
    Const vDetSchDate = 6
    Const vDetSuppConfDate = 7
    Const vDetTot = 8

    Dim strSHForm As String
    Dim strDocHdrTblName As String
    Dim strDocDtlTblName As String
    Dim strDocIDFldName As String
    Dim strDocNoFldName As String
    Dim intItemDocID As Int32

    Private Sub frmUtlSuppConfDate_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.KeyPreview = True
        Call initFormSize(Me, Me.Text, False, 816, 489, 0, 0)
        Call initFormVsfDet(Me, Me.Text, vsfDet, "DET")
        Me.StartPosition = FormStartPosition.CenterScreen
        initGridStyles(Me)
        Call getUtlTransactions()
        Call initScreen()
        Call FillDtls()
    End Sub

    Private Sub getUtlTransactions()
        If intDocID <= 0 Then Me.Close()
        Dim rsDummy As oledb.oledbDataReader
        rsDummy = clsSVT.getDataReaderFromSQL("Select * from UtlTransactions where DocID = " & intDocID)

        With rsDummy
            If .HasRows Then
                .Read()
                Me.Text = GetVal(!DocName, "") & " - Update Supplier Confirmation Date..."
                strSHForm = GetVal(!DocShForm, "")
                strDocNoFldName = strSHForm & "No"
                strDocIDFldName = strSHForm & "ID"
                strDocHdrTblName = "TrnHdr" & strSHForm
                strDocDtlTblName = "TrnDtl" & strSHForm
                intItemDocID = GetVal(!MstDocIDItem, 0)
            End If
        End With
    End Sub

    Public Sub FillDtls()
        Dim strSQL As String = ""
        Dim rs As oledb.oledbDataReader

        strSQL = "SELECT  ITEMSLNO,"
        If intItemDocID > 0 Then
            strSQL &= " ItemCode, ItemName, "
        Else
            strSQL &= " Item as ItemCode,ItemDesc as ItemName, "
        End If
        strSQL &= " UOMCODE ," & strSHForm & "Qty, schDate, supconfdt "
        strSQL &= " FROM " & strDocDtlTblName & " D, "
        If intItemDocID > 0 Then strSQL &= " MSTITEM I, "
        strSQL &= " MSTUOM U "
        strSQL &= " WHERE D.UOMID=U.UOMID "
        If intItemDocID > 0 Then strSQL &= " And D.ItemID=I.ItemID "
        strSQL &= " And " & strDocIDFldName & " = " & Val(txtPOID.Text) & ""
        strSQL &= " Order by ItemSlno"


        rs = clsSVT.getDataReaderFromSQL(strSQL)

        If Not rs.HasRows Then
            rs.Close()
            Exit Sub
        End If

        With vsfDet
            .Rows.Count = 1
            Do While rs.Read
                .Rows.Count = .Rows.Count + 1
                .SetData(.Rows.Count - 1, vDetSlNo, rs!ITEMSLNO)
                .SetData(.Rows.Count - 1, vDetItemCode, rs!ITEMCODE)
                .SetData(.Rows.Count - 1, vDetItemName, rs!ITEMNAME)
                .SetData(.Rows.Count - 1, vDetUOMCODE, rs!UOMCODE)
                .SetData(.Rows.Count - 1, vDetQty, rs.Item(strSHForm & "Qty"))
                .SetData(.Rows.Count - 1, vDetSchDate, rs!SCHDATE)
                .SetData(.Rows.Count - 1, vDetSuppConfDate, rs!SUPCONFDT)
            Loop
        End With
    End Sub

    Private Sub initScreen()
        With vsfDet
            .Cols.Count = vDetTot
            .Rows.Count = 1
            .SetData(0, vDetSlNo, "SLNO")
            .SetData(0, vDetItemID, "ItemID")
            .SetData(0, vDetItemCode, "Item Code")
            .SetData(0, vDetItemName, "Item Name")
            .SetData(0, vDetUOMCODE, "UOMCODE")
            .SetData(0, vDetQty, "POQty")
            .SetData(0, vDetSchDate, "SchDate")
            .SetData(0, vDetSuppConfDate, "Supplier Confirmation Date")
            .Cols(vDetSuppConfDate).DataType = GetType(Date)
            SetHideCol(vsfDet, True, vDetItemID)
            Call SetFlexColor(vsfDet, "", vDetItemCode, vDetItemName, vDetUOMCODE, vDetQty, vDetSchDate)
            Call SetFlexColor(vsfDet, "M", vDetSuppConfDate)
        End With
    End Sub

    Private Sub frmUtlSuppConfDate_KeyUp(sender As Object, e As KeyEventArgs) Handles Me.KeyUp
        If e.KeyCode = Keys.F9 And e.Control = True Then
            Call initFormSize(Me, Me.Text, True, 756, 483, 0, 0)
            MsgBox("Restored Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
        If e.KeyCode = Keys.F8 And e.Control = True Then
            Call SaveActFormSettings(Me, Me.Text)
            Call SaveActFormVsfDet(Me, Me.Text, "DET", vsfDet)
            MsgBox("Saved Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub btnUpdate_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click
        If validFlds() Then
            Call SaveDoc()
        End If
    End Sub

    Public Function SaveDoc() As Boolean
        SaveDoc = False
        Dim strSQL As String
        Dim svtTrn As oledb.oledbTransaction = Nothing

        stringReplace(Me)

        If validFlds() Then

            Try
                svtTrn = svtConn.BeginTransaction(IsolationLevel.ReadCommitted)
                For i = 1 To vsfDet.Rows.Count - 1
                    strSQL = "Update " & strDocDtlTblName & " set "
                    If GetVal(vsfDet.GetData(i, vDetSuppConfDate), "") = "" Then
                        strSQL = strSQL & " SUPCONFDT = Null "
                    Else
                        strSQL = strSQL & " SUPCONFDT = '" & getDateFormatInInsert(vsfDet.GetData(i, vDetSuppConfDate)) & "'"
                    End If
                    strSQL = strSQL & " where " & strDocIDFldName & " = " & Val(txtPOID.Text) & " and ItemSlNO = " & Val(vsfDet.GetData(i, vDetSlNo))
                    If Not clsSVT.execSQLStatement(strSQL, svtTrn) Then
                        SaveDoc = False
                        svtTrn.Rollback()
                        Exit Function
                    End If
                Next
                svtTrn.Commit()
                MsgBox("Updated successfully.", vbInformation, Me.Text)
                Me.Close()

            Catch ex As Exception
                MsgBox("Error during Update" & vbCrLf & ex.Message, MsgBoxStyle.Critical, Me.Text & "[btnUpdate_Click]")
                svtTrn.Rollback()
            End Try
        End If
        SaveDoc = True
    End Function

    Private Function validFlds() As Boolean
        validFlds = False

        With vsfDet
            For i = 1 To .Rows.Count - 1
                If GetVal(.GetData(i, vDetSuppConfDate), "") = "" Then
                    MsgBox("Please Enter Supplier Confirmation Date..", vbInformation, Me.Text)
                    Try
                        .StartEditing()
                    Catch ex As Exception
                    End Try
                    Exit Function
                End If
            Next
        End With

        validFlds = True
    End Function

    Private Sub vsfDet_BeforeEdit(sender As Object, e As RowColEventArgs) Handles vsfDet.BeforeEdit
        vsfDet.ComboList = ""
        vsfDet.EditMask = ""
        If e.Col = vDetSuppConfDate Then
            e.Cancel = False
            '  vsfDet.EditMask = "__/__/____"
            Exit Sub
        Else
            e.Cancel = True
            Exit Sub
        End If
    End Sub
End Class