Public Class frmUtlAddParty

    Dim strSQL As String
    Dim clsSVT As New svtLibrary
    Dim svtTrn As oledb.oledbTransaction

    Dim strTblName As String
    Dim strIDName As String
    Dim strCodeName As String
    Dim strPartyName As String
    Dim strFormCaption As String
    Dim strFrmMsg As String
    Dim strFrmPfx As String
    Dim strFldSeries As String
    Dim strFldNo As String
    Dim objFrm As Object
    Dim blnAuthReq As Boolean = False

    Public Sub initForm(ByVal argDocID As Integer, ByVal argFrm As Object)
        objFrm = argFrm
        Dim rs As oledb.oledbDataReader
        rs = clsSVT.getDataReaderFromSQL(" Select * from UtlTransactions where DocID = " & argDocID)

        With rs
            .Read()

            strFrmMsg = Trim(Replace(!Docname, "Master", ""))
            strFormCaption = !DocName
            strTblName = !HdrTblName
            strCodeName = Trim(Replace(!HdrTblName, "Mst", "")) & "Code"
            strIDName = Trim(Replace(!HdrTblName, "Mst", "")) & "ID"
            strFrmPfx = Trim(Replace(!HdrTblName, "Mst", ""))
            strFldSeries = Replace(!HdrTblName, "Mst", "") & "Series"
            strFldNo = Replace(!HdrTblName, "Mst", "") & "No"

            If GetVal(!AuthReq, "N") = "Y" Then
                blnAuthReq = True
            Else
                blnAuthReq = False
            End If

            lblPartyName.Text = TITLECASE(strFrmPfx) & " Name"
        End With
    End Sub

    Private Sub frmUtlAddParty_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Call fillFormProperties(Me)
        Call ChangeFontSize(Me)
    End Sub

    Private Sub clrFlds()
        txtContactNo.Text = ""
        txtPartyName.Text = ""
        txtRmks.Text = ""
    End Sub

    Private Function validFlds() As Boolean
        validFlds = False
        Dim rs As oledb.oledbDataReader

        If Trim(txtPartyName.Text) = "" Then
            MsgBox("Please enter " & lblPartyName.Text, vbInformation, Me.Text)
            txtPartyName.Focus()
            Exit Function
        End If

        If Trim(txtContactNo.Text) = "" Then
            MsgBox("Please enter " & lblContactNo.Text, vbInformation, Me.Text)
            txtContactNo.Focus()
            Exit Function
        End If

        rs = clsSVT.getDataReaderFromSQL("Select * from " & strTblName & " where " & strCodeName & " = '" & Trim(txtPartyName.Text) & "' or PHNO = '" & Trim(txtContactNo.Text) & "'")

        If rs.HasRows Then
            MsgBox("Party already exist", vbInformation, Me.Text)
            txtPartyName.Text = ""
            txtPartyName.Focus()
            rs.Close()
            Exit Function
        End If
        rs.Close()
        validFlds = True
    End Function

    Private Sub btnUpdate_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click
        If Not validFlds() Then Exit Sub
        If saveDetails() Then
            MsgBox("Party Created Successfully...", MsgBoxStyle.Information, Me.Text)
            objFrm.txtParty.Text = Trim(txtPartyName.Text) & " - " & Trim(txtContactNo.Text)
            objFrm.txtSalesPerson.Focus()
            frmTrnInv.vsfCust.Visible = False
            Me.Close()
        End If
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub txtRmks_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtRmks.KeyPress
        If e.KeyChar = "'" Then
            e.Handled = True
        End If
    End Sub

    Private Sub txtPartyName_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtPartyName.KeyPress
        If e.KeyChar = "'" Then
            e.Handled = True
        End If
    End Sub

    Private Sub txtContactNo_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtContactNo.KeyPress
        If e.KeyChar = "'" Then
            e.Handled = True
        End If
    End Sub

    Private Function saveDetails() As Boolean
        Dim strSQL As String = ""
        Dim strValues As String = ""
        Dim strFlds As String = ""
        Dim intID As Long

        saveDetails = False

        intID = clsSVT.AutoIncrementedNum(strIDName, strTblName)
        svtTrn = svtConn.BeginTransaction(IsolationLevel.ReadCommitted)

        strSQL = " Insert into " & strTblName & "("
        strSQL = strSQL & strIDName & ",AddedBy,AddedOn," & strCodeName & ", " & strFrmPfx & "Name," & strFrmPfx & "AliasName,"
        strSQL = strSQL & " Remarks,"
        strSQL = strSQL & " Authorised,AuthBy,AuthOn"
        strSQL = strSQL & " ,PHNO "
        strSQL = strSQL & strFlds & ") values ("
        strSQL = strSQL & intID & ","
        strSQL = strSQL & "'" & gUserCode & "',"
        strSQL = strSQL & "'" & getDateFormatInInsert(getSystemDateTime(svtTrn), True) & "',"
        strSQL = strSQL & "'" & Trim(txtPartyName.Text) & "', "
        strSQL = strSQL & "'" & Trim(txtPartyName.Text) & "',"
        strSQL = strSQL & "'" & Trim(txtPartyName.Text) & "',"
        strSQL = strSQL & "'" & Trim(txtRmks.Text) & "'"

        If Not blnAuthReq Then
            strSQL = strSQL & ", 'Y','" & gUserCode & "'," & getDateFormatInCond(getSystemDate(svtTrn)) & ""
        Else
            strSQL = strSQL & ", 'N','',Null "
        End If
        strSQL = strSQL & ",'" & Trim(txtContactNo.Text) & "')"

        Try
            If Not clsSVT.execSQLStatement(strSQL, svtTrn) Then
                svtTrn.Rollback()
                Exit Function
            End If
            svtTrn.Commit()
            saveDetails = True
        Catch ex As Exception
            svtTrn.Rollback()
            MsgBox("Error during Saving." & vbCrLf & ex.Message, MsgBoxStyle.Critical, Me.Text)
            Exit Function
        End Try
    End Function
End Class