Imports System.ComponentModel
Imports System.Data.oledb
Imports System.Text.RegularExpressions

Public Class frmMstPartyActivities
    Dim clsSVT As New svtLibrary
    Public Mode As String
    Dim intPartyID As Integer
    Dim intdocID As Integer
    Public strPartyIDFld As String
    Public strRights As String
    Public strfrmCaption As String
    Dim intNo As Integer

    Dim strPartyTblName As String
    Dim strFldPartyID As String
    Dim strFldPartyCode As String
    Dim strFldPartyName As String

    Const vDetActID = 0
    Const vDetPartyID = 1
    Const vDetPartyDocId = 2
    Const vDetDate = 3
    Const vdetNotes = 4
    Const vdetUser = 5

    Private Sub frmMstPartyActivities_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        KeyPreview = True
        Call initFormSize(Me, Me.Text, False, 910, 427, 0, 0)
        Call fillGrid()
        Call FillDocDetails(intPartyID)
        btnAdd.Enabled = True
        btnRemove.Enabled = True
        Call initFormVsfDet(Me, Me.Text, vsfDet, "DET")
        Call ChangeFontSize(Me)

    End Sub

    Private Sub initScreen()
        With vsfDet
            .Rows.Count = 1
            .Cols.Count = 6

            .SetData(0, vDetActID, "ActID")
            .SetData(0, vDetPartyID, "CustID")
            .SetData(0, vDetPartyDocId, "PartyDocID")
            .SetData(0, vDetDate, "Date")
            .SetData(0, vdetNotes, "Notes")
            .SetData(0, vdetUser, "User")
        End With

        Call SetHideCol(vsfDet, True, vDetActID, vDetPartyID, vDetPartyDocId)
        Call SetColWidth(vsfDet, 15, 15, 250)
        Call SetFlexColor(vsfDet, "", vdetNotes, vDetDate, vdetUser)

        fraEditvsfDetVisible(True)
        txtPartyCode.Enabled = False

        fraEdit.Visible = False
        vsfDet.Visible = True

        Me.Text = strfrmCaption & " Activities "
        lblPartyCode.Text = strfrmCaption & " Code "
        initGridStyles(Me)
        Call GetPartyDtls()
        If Not chkUserRights(strRights, chkRightsPos.AddRights, Me.Text, False) Then btnAdd.Visible = False
        If Not chkUserRights(strRights, chkRightsPos.AddRights, Me.Text, False) Then btnSave.Visible = False
        If Not chkUserRights(strRights, chkRightsPos.EditRights, Me.Text, False) Then
            btnAdd.Visible = False
            btnSave.Visible = False
        Else
            btnAdd.Visible = True
            btnSave.Visible = True
        End If
        If Not chkUserRights(strRights, chkRightsPos.DelRights, Me.Text, False) Then btnRemove.Visible = False

    End Sub

    Private Sub EnableBtn(ByVal argMode As String)
        If argMode <> "M" Then
            btnAdd.Enabled = False
            btnCancel.Enabled = False
        Else

            btnAdd.Enabled = True
            btnCancel.Enabled = True
        End If
    End Sub
    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        fraEdit.Visible = False
        vsfDet.Visible = True
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim svtTrn As oledb.oledbTransaction = Nothing
        Dim strSQL As String

        stringReplace(Me)

        If txtNotes.Text = "" Then
            MsgBox("Note should not be blank", MsgBoxStyle.Information, Me.Text)
            txtNotes.Focus()
            Exit Sub
        End If

        txtActID.Text = clsSVT.AutoIncrementedNum("ActId", "MstPartyActivity")
        Try
            strSQL = "Delete from MstPartyActivity where ActID = " & Val(txtActID.Text) & " And PartyDocId=" & Val(txtDocId.Text) & " And PartyId=" & Val(txtPartyID.Text)
            Call clsSVT.execSQLStatement(strSQL, svtTrn)

            strSQL = "Insert into MstPartyActivity (PartyID, PartyDocId,UserID,Notes, ActDate) values ("
            strSQL = strSQL & intPartyID & ","
            strSQL = strSQL & intdocID & ","
            strSQL = strSQL & "" & gUserID & ","
            strSQL = strSQL & "'" & Trim(txtNotes.Text) & "',"
            strSQL = strSQL & "'" & getDateFormatInInsert(getSystemDateTime, True) & "')"

            svtTrn = svtConn.BeginTransaction(IsolationLevel.ReadCommitted)
            If Not clsSVT.execSQLStatement(strSQL, svtTrn) Then
                svtTrn.Rollback()
                Exit Sub
            End If
            'Call RecordAuditLog("PARTY NOTES", Val(txtActID.Text), UserActions.DeleteAction, svtTrn)

            svtTrn.Commit()
            MsgBox("Note Saved Successfully", MsgBoxStyle.Information, Me.Text)
            Mode = ""
            fraEdit.Visible = False
            fillGrid()
            Call FillDocDetails(intPartyID)
            vsfDet.Visible = True

        Catch ex As Exception
            MsgBox("Error during Save" & vbCrLf & ex.Message, MsgBoxStyle.Critical, Me.Text)
            svtTrn.Rollback()
        End Try
    End Sub

    Private Sub fillGrid()
        Dim rsTemp As oledb.oledbDataReader
        Dim strSql As String
        Dim StrUser As String
        fraEditvsfDetVisible(True)

        strSql = "Select *  from MstPartyActivity where PartydocID = " & intdocID & " and PartyID = " & intPartyID & "order by ActDate Desc"

        rsTemp = clsSVT.getDataReaderFromSQL(strSql)
        vsfDet.Rows.Count = 1
        With rsTemp
            Do While .Read()
                With vsfDet
                    vsfDet.Rows.Count = vsfDet.Rows.Count + 1
                    .SetData(vsfDet.Rows.Count - 1, vDetActID, GetVal(rsTemp("ActID"), ""))
                    .SetData(vsfDet.Rows.Count - 1, vDetPartyDocId, GetVal(rsTemp("PartyDocID"), ""))
                    .SetData(vsfDet.Rows.Count - 1, vDetPartyID, GetVal(rsTemp("PartyID"), ""))
                    .SetData(vsfDet.Rows.Count - 1, vDetDate, GetVal(rsTemp("ActDate"), ""))
                    .SetData(vsfDet.Rows.Count - 1, vdetNotes, GetVal(rsTemp("Notes"), ""))
                    StrUser = clsSVT.getDataStringValue("Select Usercode from MstUSer where USerId=" & GetVal(rsTemp("UserId")), "", False)
                    .SetData(vsfDet.Rows.Count - 1, vdetUser, StrUser)
                End With
            Loop
            .Close()

        End With
        intNo = 0
        For i = 1 To vsfDet.Rows.Count - 1
            If vsfDet.GetData(i, vdetNotes) <> "" Then
                intNo = intNo + 1
            End If
        Next
        If intNo = 0 Then
            lblNoofAddress.Text = "No Notes Present For this " & strfrmCaption
        Else
            ' vsfDet.AutoSizeCols()
            vsfDet.AutoSizeRows()
            'If vsfDet.Rows.Count = 1 Then
            'Else
            lblNoofAddress.Text = "Total Notes Present for this " & strfrmCaption & " is " & intNo
        End If

    End Sub

    Private Sub fraEditvsfDetVisible(ByVal argFlag As Boolean)
        If argFlag Then
            fraEdit.Visible = False
            vsfDet.Visible = True
        Else
            fraEdit.Visible = True
            vsfDet.Visible = False
        End If
    End Sub

    Private Sub visibleEditbtn(ByVal argBLN As Boolean)
        If argBLN Then
            btnSave.Visible = True
            btnCancel.Visible = True
            btnRemove.Enabled = True
        Else
            Mode = ""
            btnCancel.Visible = False
            btnSave.Visible = False
            btnRemove.Enabled = True
        End If
    End Sub


    Private Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdd.Click

        If Not chkUserRights(strRights, chkRightsPos.AddRights, Me.Text) Then Exit Sub
        Mode = "A"
        Call ClearFlds()
        vsfDet.Visible = False
        fraEdit.Visible = True
        fraEdit.BringToFront()
        DtpDate.Value = getSystemDate()
        txtNotes.Enabled = True
        visibleEditbtn(True)
        txtNotes.Focus()
    End Sub

    Private Sub ClearFlds()
        DtpDate.Text = ""
        txtNotes.Text = ""
    End Sub

    Private Sub vsfDet_BeforeEdit(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfDet.BeforeEdit
        e.Cancel = True
    End Sub

    Private Sub vsfDet_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles vsfDet.DoubleClick
        If vsfDet.Rows.Count <= 1 Then Exit Sub
        fraEdit.Visible = True
        txtActID.Text = vsfDet.GetData(vsfDet.Row, vDetActID)
        DtpDate.Value = vsfDet.GetData(vsfDet.Row, vDetDate)
        txtNotes.Text = vsfDet.GetData(vsfDet.Row, vdetNotes)
        txtDocId.Text = vsfDet.GetData(vsfDet.Row, vDetPartyDocId)
        txtPartyID.Text = vsfDet.GetData(vsfDet.Row, vDetPartyID)
        txtNotes.Focus()
    End Sub

    Private Sub btnRemove_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRemove.Click
        Dim strSQL As String
        If vsfDet.Rows.Count = 1 Then
            MsgBox("Notes Not Present for this " & strfrmCaption & "!!", vbInformation, Me.Text)
            Exit Sub
        End If
        If vsfDet.Row <= 0 Then Exit Sub


        If MsgBox("Are you sure you want to remove : '" & vsfDet.GetData(vsfDet.Row, vdetNotes) _
                 & "'", vbCritical + MsgBoxStyle.YesNo + vbDefaultButton2, "Remove Confirmation") = MsgBoxResult.Yes Then
            Dim svtTrn As oledb.oledbTransaction = Nothing
            Try
                SetHideRow(vsfDet, True, vsfDet.Row)
                svtTrn = svtConn.BeginTransaction(IsolationLevel.ReadCommitted)
                strSQL = "Delete from MstPartyActivity where ActID = " & Val(vsfDet.GetData(vsfDet.Row, vDetActID)) & "And PartyDocId=" & Val(vsfDet.GetData(vsfDet.Row, vDetPartyDocId))
                Call clsSVT.execSQLStatement(strSQL, svtTrn)

                'Call RecordAuditLog("PARTY NOTES", Val(vsfDet.GetData(vsfDet.Row, vDetActID)), UserActions.DeleteAction, svtTrn)

                MsgBox("Deleted Successfully!", MsgBoxStyle.Information, Me.Text)
                svtTrn.Commit()
            Catch ex As Exception
                svtTrn.Rollback()
                Exit Sub
            End Try
        End If
        Call initScreen()
        Call fillGrid()

    End Sub

    Private Sub btnCloseEdit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        fraEdit.Visible = False
        vsfDet.Visible = True
    End Sub

    Public Sub initForm(ByVal argDocID As Integer, ByVal argPartyID As Integer)
        intPartyID = argPartyID
        intdocID = argDocID
        txtPartyID.Text = argPartyID
        Call initScreen()
    End Sub


    Public Function NumberList(strSeparator As String, ParamArray arrVals() As String)

        ' Pass this function a character or characters '
        ' to be used as the separator followed by the values to be combined
        ' For example: strNumberedList =
        ' NumberList(" ", foo, bar)
        Dim X As Integer, strLine As String = ""

        For X = 0 To UBound(arrVals)
            If Not IsNothing(arrVals(X)) And Trim(arrVals(X)) <> "" Then
                strLine = strLine & strSeparator & arrVals(X)
            End If
        Next

        ' remove leading separator caharacter(s)
        strLine = Mid(strLine, Len(strSeparator) + 1)
        NumberList = strLine

    End Function


    Private Sub frmMstPartyActivities_KeyUp(sender As Object, e As KeyEventArgs) Handles Me.KeyUp
        If e.KeyCode = Keys.F9 And e.Control = True Then
            Call initFormSize(Me, Me.Text, True, 910, 427, 0, 0)
            Call initFormVsfDet(Me, Me.Text, vsfDet, "DET")
            MsgBox("Restored Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
        If e.KeyCode = Keys.F8 And e.Control = True Then
            Call SaveActFormSettings(Me, Me.Text)
            Call SaveActFormVsfDet(Me, Me.Text, "DET", vsfDet)
            MsgBox("Saved Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
    End Sub

    Private Sub txtNotes_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtNotes.KeyPress
        If e.KeyChar = "'" Then
            e.Handled = True
        End If
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub btnList_Click(sender As Object, e As EventArgs) Handles btnList.Click
        Dim strSQL As String
        Dim frmDisplay As New frmUtlMstView
        strSQL = " Select PartyID," & strFldPartyCode & "," & strFldPartyName & ",Notes,ActDate "
        strSQL = strSQL & " From MstPartyActivity p, " & strPartyTblName & " c "
        strSQL = strSQL & " Where c." & strFldPartyID & " = PartyID And PartyDocID = " & intdocID & " Order By " & strFldPartyCode

        With frmDisplay
            .Text = Me.Text
            Call .FillViewVsf(Me, strSQL, "", "", intdocID, "Address Master")
            .vsfDet.Cols(0).Visible = False
            .vsfDet.Cols(1).AllowMerging = True
            .vsfDet.Cols(2).AllowMerging = True
            .ShowDialog()
        End With
    End Sub

    Private Sub GetPartyDtls()
        Dim rsDummy As oledb.oledbDataReader
        rsDummy = clsSVT.getDataReaderFromSQL("Select * from UtlTransactions where DocID = " & intdocID)

        With rsDummy
            If .HasRows Then
                .Read()
                Dim strPartyShForm As String
                strPartyShForm = GetVal(!DocShForm, 0)
                strPartyTblName = "Mst" & strPartyShForm
                strFldPartyID = strPartyShForm & "ID"
                strFldPartyCode = strPartyShForm & "Code"
                strFldPartyName = strPartyShForm & "Name"
            End If
            .Close()
        End With
    End Sub

    Public Sub FillDocDetails(ByVal argPartyID As Integer)
        Dim strSql As String
        Dim StrUser As String
        Dim rsTemp As oledb.oledbDataReader
        txtPartyCode.Text = clsSVT.getDataStringValue("Select " & strFldPartyCode & " from " & strPartyTblName & " where " & strFldPartyID & "=" & argPartyID, "", False)
        lblPartyName.Text = clsSVT.getDataStringValue("Select " & strFldPartyName & " from " & strPartyTblName & " where " & strFldPartyID & "=" & argPartyID, "", False)
        intPartyID = argPartyID
        strSql = "Select *  from MstPartyActivity where PartydocID = " & intdocID & " and PartyID = " & argPartyID & "order by ActDate Desc"

        rsTemp = clsSVT.getDataReaderFromSQL(strSql)
        vsfDet.Rows.Count = 1
        With rsTemp
            Do While .Read()
                With vsfDet
                    vsfDet.Rows.Count = vsfDet.Rows.Count + 1
                    .SetData(vsfDet.Rows.Count - 1, vDetActID, GetVal(rsTemp("ActID"), ""))
                    .SetData(vsfDet.Rows.Count - 1, vDetPartyDocId, GetVal(rsTemp("PartyDocID"), ""))
                    .SetData(vsfDet.Rows.Count - 1, vDetPartyID, GetVal(rsTemp("PartyID"), ""))
                    .SetData(vsfDet.Rows.Count - 1, vDetDate, GetVal(rsTemp("ActDate"), ""))
                    .SetData(vsfDet.Rows.Count - 1, vdetNotes, GetVal(rsTemp("Notes"), ""))
                    StrUser = clsSVT.getDataStringValue("Select Usercode from MstUSer where USerId=" & GetVal(rsTemp("UserId")), "", False)
                    .SetData(vsfDet.Rows.Count - 1, vdetUser, StrUser)
                End With
            Loop
            .Close()

        End With

        Dim cs As C1.Win.C1FlexGrid.CellStyle = vsfDet.Styles.Add(1)
        Dim blndefFound As Boolean = False
        'For i = 1 To vsfDet.Rows.Count - 1
        '    If vsfDet.GetData(i, vdetdefault) = "Y" Then
        '        blndefFound = True
        '        cs.BackColor = Color.GreenYellow
        '        vsfDet.SetCellStyle(i, vdetDetails, cs)
        '        Exit For
        '    End If
        'Next
        ' vsfDet.AutoSizeCols()
        vsfDet.AutoSizeRows()
        If vsfDet.Rows.Count = 1 Then
        Else
            lblNoofAddress.Text = "Total Notes Present for this " & strfrmCaption & " is " & vsfDet.Rows.Count - 1 & ""
        End If
    End Sub
End Class
