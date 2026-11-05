Imports System.ComponentModel
Imports System.Data.OleDb
Imports System.IO
Imports C1.Win.C1FlexGrid

Public Class frmUtlShortClosedFlds
    Dim intTotRec As Integer

    Dim clsSVT As New svtLibrary
    Dim strSQL As String
    Dim strTable As String
    Dim strAllFlds As String

    Const vDetSlno = 0
    Const vDetFldName = 1
    Const vDetReq = 2
    Const vTot = 3

    Public argForm As Object
    Public argVSF As C1.Win.C1FlexGrid.C1FlexGrid

    Private Sub initScreen()
        Call initFormSize(Me, Me.Text, False, 816, 519, 0, 0)

        initGridStyles(Me)
        fillFormProperties(Me)

        With vsfDet
            .Rows.Count = 1
            .Cols.Count = vTot

            .SetData(0, vDetSlno, "Slno")
            .SetData(0, vDetFldName, "Field Name")
            .SetData(0, vDetReq, "Req")

            .Cols(vDetReq).DataType = GetType(Boolean)
        End With

    End Sub

    Private Sub cboTable_Validating(sender As Object, e As CancelEventArgs) Handles cboTable.Validating
        If Trim(cboTable.Text) = "" Then Exit Sub
        Dim blnFound As Boolean
        strTable = clsSVT.getDataStringValue("select DtlTblName from UtlTransactions where DocShForm = '" & Trim(cboTable.Text) & "'", "", blnFound)

        If Not blnFound Then
            MsgBox("Transaction Not found in the list!!", MsgBoxStyle.Information, Me.Text)
            cboTable.Text = ""
            cboTable.Focus()
            Exit Sub
        End If

        strSQL = "Select c.Name from Sys.Tables t, Sys.Columns c where t.Object_id=c.Object_id and t.Name ='" & strTable & "'"
        Dim rs As OleDb.OleDbDataReader
        rs = clsSVT.getDataReaderFromSQL(strSQL)

        If Not rs.HasRows Then
            rs.Close()
            MsgBox("Table is Empty...", MsgBoxStyle.Information, Me.Text)
            cboTable.Focus()
            Exit Sub
        End If

        With vsfDet
            .Rows.Count = 1
            Do While rs.Read
                .Rows.Count = .Rows.Count + 1
                .SetData(.Rows.Count - 1, vDetFldName, rs.Item("Name"))
                .SetData(.Rows.Count - 1, vDetReq, False)
            Loop
        End With
        rs.Close()

        Call ReArrangeSlNo(vsfDet, vDetSlno)

        vsfDet.AutoSizeCols()

        Call DefaultSelection()

        strAllFlds = clsSVT.getDataStringValue("Select Flds from UtlShortClosedFlds where TblName = '" & Trim(cboTable.Text) & "'", "", False)
        If strAllFlds = "" Then
            vsfDet.Row = 1
            vsfDet.Col = vDetFldName
            vsfDet.Focus()
            Exit Sub
        End If

        For Each strFlds As String In strAllFlds.Split("|")
            For i As Int32 = 1 To vsfDet.Rows.Count - 1
                If strFlds = Trim(vsfDet.GetData(i, vDetFldName)) Then
                    vsfDet.SetData(i, vDetReq, True)
                    Exit For
                End If
            Next
        Next

    End Sub

    Private Sub DefaultSelection()
        With vsfDet
            For i As Int32 = 1 To .Rows.Count - 1
                Select Case UCase(.GetData(i, vDetFldName))
                    Case Trim(UCase(cboTable.Text)) & "ID", Trim(UCase(cboTable.Text)) & "DTLID",
                         Trim(UCase(cboTable.Text)) & "QTY", "ITEMID", "SHORTCLOSED", "SHORTCLOSEDBY", "SHORTCLOSEDDT",
                        "SHORTCLOSEDQTY", "CUMQTY"

                        vsfDet.SetData(i, vDetReq, True)
                End Select
            Next
        End With
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub frmUtlShortClosedFlds_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Call initScreen()
        bindCombo(cboTable, "DocShForm", "DocShForm", "Select Distinct DocShForm From UtlTransactions Where DocType = 'Transaction' And DocShForm is Not Null")
        cboTable.Text = ""
        cboTable.Focus()
    End Sub

    Private Sub vsfDet_BeforeEdit(sender As Object, e As RowColEventArgs) Handles vsfDet.BeforeEdit
        If Not e.Col = vDetReq Then
            e.Cancel = True
        End If
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click

        If Trim(cboTable.Text) = "" Then
            MsgBox("Select table...", MsgBoxStyle.Exclamation, Me.Text)
            cboTable.Focus()
            Exit Sub
        End If

        Dim blnFound As Boolean

        clsSVT.getDataStringValue("select DtlTblName from UtlTransactions where DocShForm = '" & Trim(cboTable.Text) & "'", "", blnFound)
        If Not blnFound Then
            MsgBox("Transaction Not found in the list!!", MsgBoxStyle.Information, Me.Text)
            cboTable.Focus()
            Exit Sub
        End If

        With vsfDet
            If .Rows.Count <= 1 Then
                MsgBox("Grid Empty...", MsgBoxStyle.Critical, Me.Text)
                vsfDet.Focus()
                Exit Sub
            End If

            Call DefaultSelection()

            blnFound = False
            For i As Int32 = 1 To .Rows.Count - 1
                If .GetCellCheck(i, vDetReq) = CheckEnum.Checked Then
                    blnFound = True
                End If
            Next

            If Not blnFound Then
                MsgBox("Atleast one field should be selected in the grid...", MsgBoxStyle.Exclamation, Me.Text)
                vsfDet.Focus()
                Exit Sub
            End If

            strSQL = "Delete from UtlShortclosedFlds where TblName = '" & cboTable.Text & "'"
            clsSVT.execSQLStatement(strSQL)

            strSQL = "Insert into UtlShortClosedFlds (TblName, Flds) values("
            strSQL &= "'" & Trim(cboTable.Text) & "','"
            For i As Int32 = 1 To .Rows.Count - 1
                If .GetCellCheck(i, vDetReq) = CheckEnum.Checked Then
                    strSQL &= "d." & .GetData(i, vDetFldName) & "|"
                End If
            Next
            strSQL = Mid(strSQL, 1, Len(strSQL) - 1)
            strSQL &= "')"
            If clsSVT.execSQLStatement(strSQL) Then
                MsgBox("Data saved Successfully", MsgBoxStyle.Exclamation, Me.Text)
            End If
        End With

    End Sub

    Private Sub btnUP_Click(sender As Object, e As EventArgs) Handles btnUP.Click
        Call MoveOneRowUp(vsfDet, vDetSlno)
    End Sub

    Private Sub btnDown_Click(sender As Object, e As EventArgs) Handles btnDown.Click
        Call MoveOneRowDown(vsfDet, vDetSlno)
    End Sub


End Class