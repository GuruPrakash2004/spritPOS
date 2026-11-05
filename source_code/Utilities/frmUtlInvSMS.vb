Public Class frmUtlInvSMS
    Dim clsSVT As New svtLibrary

    Private Sub frmUtlInvSMS_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim strSql As String
        Dim rsDummy As oledb.oledbDataReader
        strSql = "Select * from UtlMisc"
        rsDummy = clsSVT.getDataReaderFromSQL(strSql)

        If Not rsDummy.HasRows Then
            rsDummy.Close()
            Exit Sub
        End If

        rsDummy.Read()
        txtValue.Text = rsDummy!InvSMSSalesvalue
        txtSalesMsg.Text = rsDummy!InvSMSSalesMSg
        txtCancelMsg.Text = rsDummy!InvSMSCancelMsg
        txtDelMsg.Text = rsDummy!InvSMSDeleteMsg
        txtMobNo.Text = GetVal(rsDummy!InvMsgMobNo, "")
        ApplyChkToCheckBox(chkForSalesValue, rsDummy!InvSMSblnSales)
        ApplyChkToCheckBox(chkForCancellation, rsDummy!InvSMSblnCancel)
        ApplyChkToCheckBox(chkForDeletion, rsDummy!InvSMSblnDelete)

        rsDummy.Close()

    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        stringReplace(Me)

        Dim strSQL As String
        strSQL = "update UtlMisc set"
        strSQL = strSQL & " InvSMSblnSales= '" & getCheckString(chkForSalesValue) & "',"
        strSQL = strSQL & "InvSMSblnCancel= '" & getCheckString(chkForCancellation) & "',"
        strSQL = strSQL & "InvSMSblnDelete='" & getCheckString(chkForDeletion) & "',"
        strSQL = strSQL & "InvMsgMobNo='" & GetVal(txtMobNo.Text, "") & "', "
        strSQL = strSQL & "InvSMSSalesvalue=" & GetVal(txtValue.Text, 0) & ", "
        strSQL = strSQL & "InvSMSSalesMSg= '" & GetVal(txtSalesMsg.Text, "") & "',"
        strSQL = strSQL & "InvSMSCancelMsg= '" & GetVal(txtCancelMsg.Text, "") & "',"
        strSQL = strSQL & "InvSMSDeleteMsg= '" & GetVal(txtDelMsg.Text, "") & "'"

        Call clsSVT.execSQLStatement(strSQL)

        Dim rsDummy As oledb.oledbDataReader

        rsDummy = clsSVT.getDataReaderFromSQL("Select * from UtlMisc")
        If Not rsDummy.HasRows Then
            rsDummy.Close()
            Exit Sub
        End If
        With rsDummy
            If .HasRows Then
                .Read()
                gdblSalesValLmt = rsDummy.Item("InvSMSSalesvalue")
                gstrSalesSmsMsg = rsDummy.Item("InvSMSSalesMSg")
                gstrCancelledSMSMsg = rsDummy.Item("InvSMSCancelMsg")
                gstrDeleteSMSMsg = rsDummy.Item("InvSMSDeleteMsg")

                blnSMSFeature = IIf(rsDummy.Item("SMSFeature") = "Y", True, False)
                gBlnSMSInvCancelled = IIf(rsDummy.Item("InvSMSblnCancel") = "Y", True, False)
                gBlnSMSInvDeleted = IIf(rsDummy.Item("InvSMSblnDelete") = "Y", True, False)
                gBlnSMSInvSales = IIf(rsDummy.Item("InvSMSblnSales") = "Y", True, False)
                gStrSalesMobNo = rsDummy.Item("InvMsgMobNo")
            End If
            .Close()
        End With
        MsgBox("Saved Successfully.", vbInformation, Me.Text)

        Me.Close()
    End Sub
End Class