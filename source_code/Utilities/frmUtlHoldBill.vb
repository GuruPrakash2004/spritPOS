Imports C1.Win.C1FlexGrid
Imports System.IO

Public Class frmUtlHoldBill
    Dim clsSVT As New svtLibrary

    Dim objFrm As Object
    Dim intSCID As Integer
    Dim strRestorePath As String

    Const vDetSCID = 0
    Const vDetDateTime = 1
    Const vDetHoldName = 2
    Const vDetTol = 3

    Public Sub InitForm(ByVal argDocID As Integer, ByVal argFrm As Object, ByVal argRestorePath As String)
        objFrm = argFrm
        intSCID = argDocID
        strRestorePath = argRestorePath
    End Sub

    Public Sub InitScreen()
        With vsfDet
            .Rows.GlyphRow = -1
            .Rows.Fixed = 1
            .Cols.Count = vDetTol
            .Rows.Count = 1
            .SetData(0, vDetSCID, "SCID")
            .SetData(0, vDetDateTime, "Bill DateTime")
            .SetData(0, vDetHoldName, "Hold Name")
            .Cols(vDetHoldName).TextAlignFixed = TextAlignEnum.CenterCenter
            Call SetHideCol(vsfDet, True, vDetSCID, vDetDateTime)
            Call SetFlexColor(vsfDet, "", vDetSCID, vDetDateTime, vDetHoldName)
            Call SetColAlign(vsfDet, svtAlign.Center, vDetHoldName)
        End With
    End Sub

    Private Sub clearflds()
        txtHoldName.Text = ""
    End Sub

    Private Sub frmUtlHoldBill_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Call initFormVsfDet(Me, Me.Text, vsfDet, "Detail")
        Call fillFormProperties(Me)
        Call initGridStyles(Me)
        Call initScreen
        Call fillHoldDtls()
        Call clearflds()
    End Sub

    Private Sub btnHold_Click(sender As Object, e As EventArgs) Handles btnHold.Click
        If Trim(txtHoldName.Text) = "" Then
            MsgBox("Please enter hold name", vbInformation, Me.Text)
            txtHoldName.Focus()
            Exit Sub
        End If

        If File.Exists(strRestorePath & Trim(txtHoldName.Text) & ".xml") Then
            If MsgBox(Trim(txtHoldName.Text) & " file already exist. Do you like to replace?", vbCritical + vbYesNo, Me.Text) = vbNo Then Exit Sub
            File.Delete(strRestorePath & Trim(txtHoldName.Text) & ".xml")
        End If

        Call SaveHoldData()
    End Sub

    Private Sub SaveHoldData()
        Dim strSQL As String
        strSQL = "Insert Into UtlHoldBill (SCID, BillDateTime, HoldName) values"
        strSQL = strSQL & " (" & intSCID & ""
        strSQL = strSQL & "," & getDateFormatInCond(getSystemDateTime()) & ""
        strSQL = strSQL & ",'" & Trim(txtHoldName.Text) & "')"
        Try
            clsSVT.execSQLStatement(strSQL)

            'If File.Exists(strRestorePath & Trim(txtHoldName.Text) & ".xml") Then
            '    File.Delete(strRestorePath & Trim(txtHoldName.Text) & ".xml")
            'End If

            objFrm.vsfDet.WriteXml(strRestorePath & Trim(txtHoldName.Text) & ".xml")
            objFrm.clearFlds()
            Me.Close()
        Catch ex As Exception
            MsgBox("Hold Data Capture Failed...." & vbNewLine & ex.Message, MsgBoxStyle.Exclamation, Me.Text)
        End Try
    End Sub

    Private Sub txtHoldName_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtHoldName.KeyPress
        If e.KeyChar = "'" Then
            e.Handled = True
        End If
    End Sub

    Private Sub fillHoldDtls()
        Dim strSQL As String = ""
        Dim intRow As Int32
        Dim rs As oledb.oledbDataReader

        strSQL = "Select * from utlholdbill Where SCID = " & intSCID & ""

        rs = clsSVT.getDataReaderFromSQL(strSQL)

        If Not rs.HasRows Then
            Exit Sub
        End If

        With rs
            Do While rs.Read
                vsfDet.Rows.Count += 1
                intRow = vsfDet.Rows.Count - 1
                vsfDet.SetData(intRow, vDetSCID, !SCID)
                vsfDet.SetData(intRow, vDetDateTime, !BillDateTime)
                vsfDet.SetData(intRow, vDetHoldName, !HoldName)
            Loop
        End With
        rs.Close()
    End Sub

    Private Sub vsfDet_BeforeEdit(sender As Object, e As RowColEventArgs) Handles vsfDet.BeforeEdit
        Select Case e.Col
            Case vDetSCID, vDetDateTime, vDetHoldName
                e.Cancel = True
        End Select
    End Sub

    Private Sub vsfDet_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles vsfDet.MouseDoubleClick
        Dim ht = vsfDet.HitTest

        If ht.Row <> -1 Then
            If ht.Row <= 0 Then
                Exit Sub
            End If

            Dim strHoldName As String
            strHoldName = vsfDet.GetData(ht.Row, vDetHoldName)

            If File.Exists(strRestorePath & strHoldName & ".xml") Then
                objFrm.vsfDet.ReadXml(strRestorePath & strHoldName & ".xml")
                Me.Close()
            Else
                MsgBox(strHoldName & " not exist", vbInformation, Me.Text)
            End If
        End If
    End Sub
End Class