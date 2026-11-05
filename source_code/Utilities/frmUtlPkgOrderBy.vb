Imports System.ComponentModel
Imports System.Data.oledb
Imports System.IO
Imports C1.Win.C1FlexGrid

Public Class frmUtlPkgOrderBy
    Const vDetID = 0
    Const vDetSlno = 1
    Const vDetPkg = 2
    Const vDetLtr = 3
    Const vDetTot = 4

    Dim clsSVT As New svtLibrary

    Private Sub frmUtlQCFldSel_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.KeyPreview = True
        Call initFormVsfDet(Me, Me.Text, vsfDet, "DET")
        Call fillFormProperties(Me)
        Call initGridStyles(Me)
        Call ChangeFontSize(Me)
        Call initScreen()
    End Sub

    Private Sub initScreen()
        With vsfDet
            .Rows.Count = 1
            .Rows.Fixed = 1
            .Cols.Count = vDetTot

            .SetData(0, vDetSlno, "Slno")
            .SetData(0, vDetPkg, "Package")
            .SetData(0, vDetLtr, "Ltr Per Bot")

            Call SetFlexColorFullGrid(vsfDet, "")
            Call SetHideCol(vsfDet, True, vDetID)
        End With
        Call LoadDetails("")
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub btnExportToXL_Click(sender As Object, e As EventArgs) Handles btnExportToXL.Click
        If vsfDet.Rows.Count > 1 Then
            Call ExportToXL(vsfDet)
        End If
    End Sub

    Private Sub frmUtlQCFldSel_KeyUp(sender As Object, e As KeyEventArgs) Handles Me.KeyUp
        If e.KeyCode = Keys.F9 And e.Control = True Then
            Call initFormVsfDet(Me, Me.Text, vsfDet, "DET")
            MsgBox("Restored Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
        If e.KeyCode = Keys.F8 And e.Control = True Then
            Call SaveActFormVsfDet(Me, Me.Text, "DET", vsfDet)
            MsgBox("Saved Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If

        If e.Alt = True AndAlso e.KeyCode = Keys.E Then
            If vsfDet.Rows.Count > 1 Then
                Call ExportToXL(vsfDet)
            End If
        End If
    End Sub

    Private Sub vsfDet_BeforeEdit(sender As Object, e As RowColEventArgs) Handles vsfDet.BeforeEdit
        e.Cancel = True
    End Sub

    Private Sub btnView_Click(sender As System.Object, e As System.EventArgs) Handles btnSave.Click
        Dim strSQL As String = ""

        With vsfDet
            For i As Int32 = 1 To .Rows.Count - 1
                strSQL = " Update MstPkg Set "
                strSQL &= " PkgOrderBy = " & Val(.GetData(i, vDetSlno))
                strSQL &= " Where PkgID = " & Val(.GetData(i, vDetID))

                If Not clsSVT.execSQLStatement(strSQL) Then
                    Exit Sub
                End If
            Next
            MsgBox("Updated Successfully...", MsgBoxStyle.Information, Me.Text)
        End With
    End Sub

    Private Sub btnMvUP_Click(sender As Object, e As EventArgs) Handles btnMvUP.Click
        If vsfDet.Row > 1 Then
            Call MoveOneRowUp(vsfDet)
            Call ReArrangeSlNo(vsfDet, vDetSlno)
        End If
    End Sub

    Private Sub btnMvDwn_Click(sender As Object, e As EventArgs) Handles btnMvDwn.Click
        If vsfDet.Row < vsfDet.Rows.Count - 1 Then
            Call MoveOneRowDown(vsfDet)
            Call ReArrangeSlNo(vsfDet, vDetSlno)
        End If
    End Sub

    Private Sub btnAsc_Click(sender As Object, e As EventArgs) Handles btnAsc.Click
        Call LoadDetails("ASC")
    End Sub
    Private Sub LoadDetails(argType As String)
        Dim strSQL As String = ""
        strSQL = "Select PkgID, PkgOrderBy, PkgCode, LtrPerBot "
        strSQL &= " From MstPkg "
        Select Case argType
            Case ""
                strSQL &= " Order by PkgOrderBy"
            Case "ASC"
                strSQL &= " Order by LtrPerBot "
            Case "DSC"
                strSQL &= " Order by LtrPerBot desc"
        End Select

        Dim rs As OleDbDataReader
        rs = clsSVT.getDataReaderFromSQL(strSQL)
        vsfDet.Rows.Count = 1
        If rs.HasRows Then
            Do While rs.Read
                With vsfDet
                    .Rows.Count = .Rows.Count + 1
                    .SetData(.Rows.Count - 1, vDetID, rs.Item("PkgID"))
                    .SetData(.Rows.Count - 1, vDetSlno, .Rows.Count - 1)
                    .SetData(.Rows.Count - 1, vDetPkg, rs.Item("PkgCode"))
                    .SetData(.Rows.Count - 1, vDetLtr, rs.Item("LtrPerBot"))
                End With
            Loop
        End If
        rs.Close()
        ReArrangeSlNo(vsfDet, vDetSlno)
    End Sub

    Private Sub btnDesc_Click(sender As Object, e As EventArgs) Handles btnDesc.Click
        Call LoadDetails("DSC")
    End Sub
End Class