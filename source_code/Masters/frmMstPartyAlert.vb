Imports System.ComponentModel
Imports System.Data.oledb
Imports System.Text.RegularExpressions
Imports C1.Win.C1FlexGrid


Public Class frmMstPartyAlert
    Dim clsSVT As New svtLibrary

    Dim intPartyID As Integer
    Dim intdocID As Integer
    Public strRights As String
    Public strfrmCaption As String
    Public Mode As String
    Dim intNo As Integer

    Dim strPartyTblName As String
    Dim strFldPartyID As String
    Dim strFldPartyCode As String
    Dim strFldPartyName As String

    Const vDetDocId = 0
    Const vDetTrnName = 1
    Const vdetMessage = 2

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub frmMstContactPerson_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        KeyPreview = True

        Call initFormSize(Me, Me.Text, False, 854, 440, 0, 0)
        Call fillGrid()
        'Call FillDocDetails(intPartyID)
        Call initFormVsfDet(Me, Me.Text, vsfDet, "DET")
        pnlMessage.Visible = False
        Call ChangeFontSize(Me)
    End Sub

    Private Sub initScreen()
        With vsfDet
            .Rows.Count = 1
            .Cols.Count = 3

            .SetData(0, vDetDocId, "DocID")
            .SetData(0, vDetTrnName, "Transactions")
            .SetData(0, vdetMessage, "Alert Message")
            .Cols(vdetMessage).ComboList = "..."

        End With
        initGridStyles(Me)
        Call SetHideCol(vsfDet, True, vDetDocId)
        Call SetColWidth(vsfDet, 1, 150, 100)
        Call SetFlexColor(vsfDet, "", vDetTrnName)
        vsfDet.Visible = True
        Me.Text = strfrmCaption & " Alert Messages"

        If Not chkUserRights(strRights, chkRightsPos.AddRights, Me.Text, False) Then btnSave.Visible = False
        If Not chkUserRights(strRights, chkRightsPos.EditRights, Me.Text, False) Then
            btnSave.Visible = False
        Else
            btnSave.Visible = True
        End If

        Call GetPartyDtls()
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Call fillGrid()
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim svtTrn As oledb.oledbTransaction = Nothing
        Dim strSQL As String

        stringReplace(Me)

        Try
            svtTrn = svtConn.BeginTransaction(IsolationLevel.ReadCommitted)

            strSQL = "Delete from MstPartyAlertMessage where PartyId=" & txtPartyId.Text & "and PartyDocId=" & intdocID
            clsSVT.execSQLStatement(strSQL, svtTrn)

            For i = 1 To vsfDet.Rows.Count - 1
                If vsfDet.GetData(i, vdetMessage) <> "" Then
                    strSQL = "Insert into MstPartyAlertMessage (MstDocId,Message,PartyID,PartyDocID) values ("
                    strSQL = strSQL & "" & Val(vsfDet.GetData(i, vDetDocId)) & ","
                    strSQL = strSQL & "'" & Trim(vsfDet.GetData(i, vdetMessage)) & "',"
                    strSQL = strSQL & "" & txtPartyId.Text & ","
                    strSQL = strSQL & "" & intdocID & ")"
                    If Not clsSVT.execSQLStatement(strSQL, svtTrn) Then
                        svtTrn.Rollback()
                        Exit Sub
                    End If
                End If
            Next i

            'Call RecordAuditLog("PARTY ALERT", Val(txtPartyId.Text), UserActions.DeleteAction, svtTrn)

            MsgBox("Message Saved Successfully", MsgBoxStyle.Information, Me.Text)
            svtTrn.Commit()
            fillGrid()
            vsfDet.Visible = True

        Catch ex As Exception
            MsgBox("Error during Save" & vbCrLf & ex.Message, MsgBoxStyle.Critical, Me.Text)
            svtTrn.Rollback()
        End Try
    End Sub

    Private Sub fillGrid()
        Dim rsTemp As oledb.oledbDataReader
        Dim strSql As String

        strSql = "Select u.DocID,DocName,isnull(Message,'') as Message"
        strSql &= " From UtlTransactions u left Join MstPartyAlertMessage m on u.DocID=m.MstDocID And m.PartyID = " & txtPartyId.Text
        strSql &= " Where MstDocIDParty = " & intdocID


        rsTemp = clsSVT.getDataReaderFromSQL(strSql)

        vsfDet.Rows.Count = 1
        With rsTemp
            Do While .Read()
                With vsfDet
                    vsfDet.Rows.Count = vsfDet.Rows.Count + 1
                    .SetData(vsfDet.Rows.Count - 1, vDetDocId, GetVal(rsTemp("DocID"), 0))
                    .SetData(vsfDet.Rows.Count - 1, vDetTrnName, GetVal(rsTemp("DocName"), ""))
                    .SetData(vsfDet.Rows.Count - 1, vdetMessage, GetVal(rsTemp("Message"), ""))
                End With
            Loop
            .Close()

        End With
        intNo = 0
        For i = 1 To vsfDet.Rows.Count - 1
            If vsfDet.GetData(i, vdetMessage) <> "" Then
                intNo = intNo + 1
            End If
        Next
        If intNo = 0 Then
            lblNoofMessages.Text = "No Alert Messages Present For this " & strfrmCaption
        Else
            lblNoofMessages.Text = "Total Alert Messages Present For this " & strfrmCaption & " Is " & intNo
        End If
        vsfDet.AutoSizeRows()

    End Sub

    Private Sub vsfDet_BeforeEdit(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfDet.BeforeEdit
        With vsfDet
            Select Case e.Col
                Case vDetTrnName
                    e.Cancel = True
            End Select

        End With
    End Sub

    Public Sub initForm(ByVal argDocID As Integer, ByVal argPartyID As Integer)
        intPartyID = argPartyID
        intdocID = argDocID
        txtPartyId.Text = argPartyID
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

    Private Sub frmMstPartyContactPerson_KeyUp(sender As Object, e As KeyEventArgs) Handles Me.KeyUp
        If e.KeyCode = Keys.F9 And e.Control = True Then
            Call initFormSize(Me, Me.Text, True, 854, 440, 0, 0)
            Call initFormVsfDet(Me, Me.Text, vsfDet, "DET")
            MsgBox("Restored Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
        If e.KeyCode = Keys.F8 And e.Control = True Then
            Call SaveActFormSettings(Me, Me.Text)
            Call SaveActFormVsfDet(Me, Me.Text, "DET", vsfDet)
            MsgBox("Saved Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
    End Sub

    Private Sub btnMsgCancel_Click(sender As Object, e As EventArgs) Handles btnMsgCancel.Click
        pnlMessage.Visible = False
        pnlBtns.Visible = True
        vsfDet.Enabled = True
    End Sub

    Private Sub txtAlertMsg_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtAlertMsg.KeyPress
        If e.KeyChar = "'" Then
            e.Handled = True
        End If
    End Sub

    Private Sub btnOk_Click(sender As Object, e As EventArgs) Handles btnOk.Click
        vsfDet.SetData(vsfDet.Row, vdetMessage, txtAlertMsg.Text)
        pnlMessage.Visible = False
        pnlBtns.Visible = True
        vsfDet.Enabled = True
        vsfDet.AutoSizeRows()
        intNo = 0
        If intNo = 0 Then
            lblNoofMessages.Text = "No Alert Messages Present For this " & strfrmCaption
        Else
            lblNoofMessages.Text = "Total Alert Messages Present For this " & strfrmCaption & " Is " & intNo
        End If
    End Sub

    Private Sub vsfDet_CellButtonClick(sender As Object, e As RowColEventArgs) Handles vsfDet.CellButtonClick
        With vsfDet
            Select Case e.Col
                Case vdetMessage
                    pnlMessage.Visible = True
                    txtTrnName.Text = .GetData(e.Row, vDetTrnName)
                    txtAlertMsg.Text = .GetData(e.Row, vdetMessage)
                    txtAlertMsg.Focus()
                    pnlBtns.Visible = False
                    vsfDet.Enabled = False
            End Select
        End With
    End Sub

    Private Sub vsfDet_KeyUp(sender As Object, e As KeyEventArgs) Handles vsfDet.KeyUp
        With vsfDet
            If e.KeyCode = Keys.Escape Then
                If pnlMessage.Visible = True Then
                    pnlMessage.Visible = False
                End If
            End If
        End With
    End Sub

    Private Sub frmMstPartyAlert_ResizeEnd(sender As Object, e As EventArgs) Handles Me.ResizeEnd
        vsfDet.AutoSizeRows()
        vsfDet.Cols(vdetMessage).Width = vsfDet.Width - vsfDet.Cols(1).Width
    End Sub

    Private Sub vsfDet_AfterResizeColumn(sender As Object, e As RowColEventArgs) Handles vsfDet.AfterResizeColumn
        vsfDet.AutoSizeRows()
    End Sub

    Private Sub btnList_Click(sender As Object, e As EventArgs) Handles btnList.Click
        Dim strSQL As String
        Dim frmDisplay As New frmUtlMstView
        strSQL = " Select PartyID," & strFldPartyCode & "," & strFldPartyName & ",Message "
        strSQL = strSQL & " From MstPartyAlertMessage p, " & strPartyTblName & " c "
        strSQL = strSQL & " Where c." & strFldPartyID & " = PartyID And PartyDocID = " & intdocID & " Order By " & strFldPartyCode

        With frmDisplay
            .Text = Me.Text
            Call .FillViewVsf(Me, strSQL, "", "", intdocID, "Contact Master")
            .vsfDet.Cols(0).Visible = False
            .vsfSearch.Cols(0).Visible = False
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
        Dim dt As DataTable
        'Dim strDetail As String
        'fraEditvsfDetVisible(True)

        txtPartyCode.Text = clsSVT.getDataStringValue("Select " & strFldPartyCode & " from " & strPartyTblName & " where " & strFldPartyID & "=" & argPartyID, "", False)
        lblPartyName.Text = clsSVT.getDataStringValue("Select " & strFldPartyName & " from " & strPartyTblName & " where " & strFldPartyID & "=" & argPartyID, "", False)
        intPartyID = argPartyID
        strSql = "Select *,  PartyID from MstPartyAlertMessage where MstDocID = " & intdocID & " And PartyID = " & argPartyID & "order by Message"
        dt = clsSVT.getDATATable(strSql)
        vsfDet.DataSource = dt
        'fraEdit.Visible = False
        vsfDet.Visible = True
        txtPartyCode.Enabled = False
        'txtPartyName.Enabled = False
        SetHideCol(vsfDet, True, 0)
        vsfDet.AutoSizeCols()
        vsfDet.AutoSizeRows()
        'vsfDet.Cols(3).Visible = False
        Call SetColWidth(vsfDet, 300)
        Me.Text = strfrmCaption & " Addresses "
        lblPartyCode.Text = strfrmCaption & " Code "
        '  lblName.Text = strfrmCaption & " Name "
        initGridStyles(Me)
        Call GetPartyDtls()
        'If vsfDet.Rows.Count <= 1 Then
        '    lblNoofAddress.Text = "No Addresses Present for this " & strfrmCaption
        '    btnEdit.Enabled = False
        '    btnDelete.Enabled = False
        '    btnAdd.Enabled = True
        'Else
        '    lblNoofAddress.Text = "Total Addresses Present for this " & strfrmCaption & " is " & vsfDet.Rows.Count - 1 & ""
        '    btnEdit.Enabled = True
        '    btnDelete.Enabled = True
        '    btnAdd.Enabled = True
        'End If
        Call initFormVsfDet(Me, Me.Text, vsfDet, "DET")
        Dim cs As C1.Win.C1FlexGrid.CellStyle = vsfDet.Styles.Add(1)

        Dim blndefFound As Boolean = False
        For i = 1 To vsfDet.Rows.Count - 1
            If vsfDet.GetData(i, vsfDet.Cols.Count - 1) = "Y" Then
                blndefFound = True
                cs.BackColor = Color.GreenYellow
                vsfDet.SetCellStyle(i, 1, cs)
                Exit For
            End If
        Next
    End Sub
End Class
