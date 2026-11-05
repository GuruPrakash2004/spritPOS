Imports System.Data.oledb
Imports System.IO
Imports C1.Win.C1FlexGrid

Public Class frmUtlUpdatefromTally
    Dim clsSvt As New svtLibrary

    Const vDetLedName = 0
    Const vDetBal = 1
    Private Sub btnUpdateFromTally_Click(sender As Object, e As EventArgs) Handles btnUpdateFromTally.Click
        Dim TallyCn As New Odbc.OdbcConnection("DSN=TallyODBC_9000")
        Dim rst As Odbc.OdbcDataReader
        Dim strLedName As String, sngAmt As Double, strSQL As String

        Try
            TallyCn.Open()
            'SUNDRY DEBTORS
            Dim cmd As New Odbc.OdbcCommand
            cmd.Connection = TallyCn
            cmd.CommandType = CommandType.Text
            cmd.CommandText = "Select $Parent, $Name as ledName, $Closingbalance as ledAmt From Ledger where $Parent LIKE '%SUNDRY DEBTORS%'"
            rst = cmd.ExecuteReader()
            'rst.Open(, TallyCn, CursorTypeEnum.adOpenDynamic, LockTypeEnum.adLockOptimistic)

            vsfDet.Rows.Count = 1
            With rst
                Do While .Read()
                    vsfDet.Rows.Count = vsfDet.Rows.Count + 1
                    vsfDet.TopRow = vsfDet.Rows.Count - 1
                    strLedName = Replace(GetVal(rst.GetValue(1), ""), "'", "`")
                    sngAmt = GetVal(rst.GetValue(2), 0)

                    vsfDet.SetData(vsfDet.Rows.Count - 1, vDetLedName, strLedName)
                    vsfDet.SetData(vsfDet.Rows.Count - 1, vDetBal, Format(sngAmt, "0.00"))

                    StMsg("Updating Outstanding for " & strLedName)
                    strSQL = "Update MstCust set CLBalance  = " & sngAmt
                    strsql = strsql & " where TallyAllias = '" & strLedName & "'"
                    clsSvt.execSQLStatement(strsql)
                Loop
                .Close()
            End With

            TallyCn.Close()
            If gTallyUpdate = "Y" Then
                End
            Else
                MsgBox("Updated Closing balances from Tally!", vbInformation, Me.Text)
                'Unload Me
            End If
            Exit Sub
        Catch
            MsgBox(Err.Description, vbCritical, Me.Text)
        End Try
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub frmUtlUpdatefromTally_Load(sender As Object, e As EventArgs) Handles Me.Load
        vsfDet.Rows.Count = 1
        vsfDet.Cols.Count = 2
        vsfDet.SetData(0, vDetLedName, "Ledger")
        vsfDet.SetData(0, vDetBal, "Amount")
        vsfDet.Cols(0).Visible = True
        vsfDet.Cols(1).Visible = True

        If gTallyUpdate = "Y" Then Call btnUpdateFromTally_Click(sender, e)
        Call initFormSize(Me, Me.Text, False, 425, 500, 0, 0)
    End Sub

    Private Sub btnExport_Click(sender As Object, e As EventArgs) Handles btnExport.Click
        If vsfDet.Rows.Count < 1 Then
            Exit Sub
        End If
        ExportToXL(vsfDet)
    End Sub

    Private Sub frmUtlUpdatefromTally_KeyUp(sender As Object, e As KeyEventArgs) Handles Me.KeyUp
        If e.KeyCode = Keys.F9 And e.Control Then
            Call initFormSize(Me, Me.Text, True, 425, 500, 0, 0)
            MsgBox("Restored Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
        If e.KeyCode = Keys.F8 And e.Control = True Then
            Call SaveActFormSettings(Me, Me.Text)
            MsgBox("Saved Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
    End Sub
End Class