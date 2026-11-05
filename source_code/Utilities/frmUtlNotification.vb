Imports System.ComponentModel
Imports C1.Win.C1FlexGrid

Public Class frmUtlNotification
    Dim clsSVT As New svtLibrary
    Dim strGenText As String = ""
    Dim dvSearch As DataView
    Dim dtGeneral As DataTable

    Const vDel = 0
    Const vID = 1
    Const vName = 2

    Const vDetID = 0
    Const vDetFromUserCode = 1
    Const vDetTitle = 2
    Const vDetNotifText = 3
    Const vDetStatus = 4
    Const vDetToUserCode = 5
    Const vDetTot = 6

    Dim blnSent As Boolean

    Private Sub frmUtlNotification_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Call initGridStyles(Me)
        Call fillFormProperties(Me)
        Call ChangeFontSize(Me)
        Call initFormVsfDet(Me, Me.Text, vsfDet, "DET")

        Me.Text = "Notification For Current User : " & gUserCode
        Me.KeyPreview = True
        With vsfContact
            .Cols.Count = 3
            .Cols(vDel).Width = 28
            .Rows.Count = 0
            .Height = 150
            .Cols(vID).Visible = False
        End With
        With vsfDet
            .Cols.Count = vDetTot
            .Rows.Count = 1
            .SetData(0, vDetID, "ID")
            .SetData(0, vDetTitle, "Title")
            .SetData(0, vDetNotifText, "Notification")
            .SetData(0, vDetStatus, "Status")
            .SetData(0, vDetFromUserCode, "User")
            .SetData(0, vDetToUserCode, "User")
            SetHideCol(vsfDet, True, vDetID)
            Call SetFlexColor(vsfDet, "", vDetTitle, vDetNotifText, vDetStatus, vDetToUserCode)
        End With
        Call initFormSize(Me, Me.Text, False, 550, 429, 0, 0)
        Call initFormVsfDet(Me, Me.Text, vsfDet, "DET")
        pnlNotify.Visible = False
        pnlNotify.SendToBack()
        pnlList.Visible = True
        pnlList.BringToFront()
        Call fillNotifications(False)
        Me.StartPosition = FormStartPosition.CenterScreen
        initGridStyles(Me)
        Call syncGridCols()
    End Sub

    Private Sub frmUtlMenuSequence_KeyUp(sender As Object, e As KeyEventArgs) Handles Me.KeyUp
        If e.KeyCode = Keys.F9 And e.Control = True Then
            Call initFormVsfDet(Me, Me.Text, vsfDet, "DET")
            MsgBox("Restored Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
        If e.KeyCode = Keys.F8 And e.Control = True Then
            Call SaveActFormVsfDet(Me, Me.Text, "DET", vsfDet)
            MsgBox("Saved Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
    End Sub


    Private Sub ClearFlds()
        txtTitle.Text = ""
        txtNotes.Text = ""
        txtUsers.Text = ""
        txtTitle.Focus()
        'txtMsg.MaxLength = gIntMsgLen
        vsfContact.Rows.Count = 0
        vsfContact.Height = (vsfContact.Rows.MinSize * 3) + 10
        pnlContact.Height = vsfContact.Height + 50
    End Sub

    Private Sub vsfSearchSelectedItem()
        Dim intID As Integer
        If vsfSearch.Rows.Count <= 1 Then Exit Sub

        intID = clsSVT.getDataLongValue("SELECT UserID FROM MstUser where UserCode =  '" & vsfSearch.GetData(vsfSearch.Row, 0) & "'", 0, False)
        Call FillUsers(intID)
        Call setSearchGridHt("")
        txtUsers.Text = ""
    End Sub

    Private Sub FillUsers(ByVal argID As Integer)
        Dim lngRowID As Integer
        Dim rsDummy As oledb.oledbDataReader
        rsDummy = clsSVT.getDataReaderFromSQL("SELECT * FROM MstUser where UserID =  " & argID)
        With rsDummy
            Do While .Read
                vsfContact.Rows.Count = vsfContact.Rows.Count + 1
                lngRowID = vsfContact.Rows.Count - 1
                vsfContact.SetData(lngRowID, vID, !UserID)
                vsfContact.SetData(lngRowID, vName, !UserCode)
                vsfContact.SetCellImage(vsfContact.Rows.Count - 1, vDel, My.Resources.cancel)
            Loop
            .Close()
        End With
    End Sub

    Private Sub setSearchGridHt(ByVal argFlag As String)
        If argFlag = "FULL" Then
            vsfSearch.Visible = True
            vsfSearch.BringToFront()
            'pnlSearch.BringToFront()
        Else
            vsfSearch.Visible = False
        End If
    End Sub

    Private Sub btnExport_Click(sender As Object, e As EventArgs) Handles btnExport.Click
        If vsfDet.Rows.Count < 1 Then
            Exit Sub
        End If
        ExportToXL(vsfDet)
    End Sub

    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        Dim strSql As String
        Dim svtTrn As oledb.oledbTransaction = Nothing
        If vsfDet.Row < 1 Then
            Exit Sub
        End If

        If vsfDet.Rows.Count = 2 And Trim(vsfDet.GetData(vsfDet.Row, vDetToUserCode)) = "" Then
            MsgBox("No data present to delete!", vbInformation + vbOKOnly, Me.Text)
            Exit Sub
        End If

        If MsgBox("Are you sure you want to delete the data:'" & vsfDet.GetData(vsfDet.Row, vDetToUserCode) & "'" & "?",
                vbYesNo + vbQuestion + vbDefaultButton1, Me.Text) = vbYes Then


            svtTrn = svtConn.BeginTransaction(IsolationLevel.ReadCommitted)
            Try
                With vsfDet
                    strSql = "Delete from UtlNotify where ID = " & .GetData(.Row, vDetID) & ""
                    clsSVT.execSQLStatement(strSql, svtTrn)
                End With
                svtTrn.Commit()
                MsgBox("Data deleted successfully.", vbInformation, Me.Text)
                Call fillNotifications(blnSent)
            Catch ex As Exception
                MsgBox("Error during Deletion" & vbCrLf & ex.Message, MsgBoxStyle.Critical, Me.Text & "[btnDelete_Click]")
                svtTrn.Rollback()
                Exit Sub
            End Try
        End If
    End Sub

    Private Sub vsfDet_BeforeEdit(sender As Object, e As RowColEventArgs) Handles vsfDet.BeforeEdit
        e.Cancel = True
    End Sub

    Private Sub fillNotifications(ByVal argFrmbln As Boolean)

        Dim strQry As String
        strQry = "Select ID, FromUserCode, Title, NotifText As Notification, Status,ToUserCode from UtlNotify "
        If argFrmbln Then
            strQry &= " where FromUserCode  = '" & gUserCode & "'"
        Else
            strQry &= " where ToUserCode  = '" & gUserCode & "'"
        End If
        strQry &= " order by title, ID Desc"
        Call showWaitCursor()
        dtGeneral = clsSVT.getDATATable(strQry)
        dvSearch = New DataView(dtGeneral)
        vsfDet.DataSource = dvSearch
        syncGridCols()
        Call showNormalCursor()
        vsfDet.Refresh()
        vsfDet.Cols(vDetID).Visible = False
        vsfDet.Cols(vDetFromUserCode).Visible = True
        vsfDet.Cols(vDetToUserCode).Visible = True
        If argFrmbln Then
            vsfDet.Cols(vDetFromUserCode).Visible = False
            Me.Text = "Notification sent from Current User : " & gUserCode
        Else
            vsfDet.Cols(vDetToUserCode).Visible = False
            Me.Text = "Notification For Current User : " & gUserCode
        End If
        vsfDet.ScrollBars = ScrollBars.Both
        Call initFormVsfDet(Me, Me.Text, vsfDet, "DET")
        Call syncGridCols()
        With vsfDet
            .AutoSizeRows()
        End With
    End Sub
    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Call ClearFlds()
        pnlList.Visible = True
        pnlList.Enabled = True
        pnlList.BringToFront()
        With vsfContact
            .Cols.Count = 3
            .Cols(vDel).Width = 28
            .Rows.Count = 0
            .Height = 150
            .Cols(vID).Visible = False
        End With
    End Sub

    Private Sub vsfContact_BeforeEdit(sender As Object, e As RowColEventArgs) Handles vsfContact.BeforeEdit
        e.Cancel = True
    End Sub

    Private Sub vsfContact_MouseClick(sender As Object, e As MouseEventArgs) Handles vsfContact.MouseClick
        Dim ht = vsfContact.HitTest

        With vsfContact
            If ht.Row < 0 Then Exit Sub
            If .GetData(ht.Row, vName) = "" Then Exit Sub
            If ht.Column <> vDel Then Exit Sub
            .RemoveItem(ht.Row)
            If vsfContact.Rows.Count > 3 Then
                vsfContact.Height = (vsfContact.Rows.MinSize * vsfContact.Rows.Count) + 10
                pnlContact.Height = vsfContact.Height
            End If
        End With
    End Sub

    Private Sub txtUsers_KeyUp(sender As Object, e As KeyEventArgs) Handles txtUsers.KeyUp
        Dim StrQry As String, strCond As String = ""

        If e.KeyCode = Keys.Return Then
            Call vsfSearchSelectedItem()
            Exit Sub
        End If

        If e.KeyCode = Keys.Down Then
            vsfSearch.Focus()
            Exit Sub
        End If

        txtUsers.Text = Replace(txtUsers.Text, "'", "`")

        If e.KeyCode = Keys.Escape Then
            txtUsers.Text = ""
            Call setSearchGridHt("")
            Exit Sub
        End If

        If Len(txtUsers.Text) < 1 Then
            Call setSearchGridHt("")
            Exit Sub
        End If
        vsfSearch.DataSource = Nothing

        Call setSearchGridHt("FULL")
        StrQry = "Select Top 30  UserCode, UserName"
        StrQry &= " from MstUser "
        StrQry &= " where UserCode  Like '%" & Trim(txtUsers.Text) & "%'"
        StrQry &= " and UserID<>" & gUserID
        StrQry &= " order by 1"
        Call showWaitCursor()
        dtGeneral = clsSVT.getDATATable(StrQry)
        dvSearch = New DataView(dtGeneral)
        vsfSearch.DataSource = dvSearch
        Call showNormalCursor()
        vsfSearch.Refresh()
        vsfSearch.ScrollBars = ScrollBars.Both
        vsfSearch.AutoSizeCols()
        strGenText = Trim(txtUsers.Text)
    End Sub

    Private Sub vsfSearch_BeforeEdit(sender As Object, e As RowColEventArgs) Handles vsfSearch.BeforeEdit
        e.Cancel = True
    End Sub

    Private Sub vsfSearch_DoubleClick(sender As Object, e As EventArgs) Handles vsfSearch.DoubleClick
        Call vsfSearchSelectedItem()
    End Sub

    Private Sub btnSend_Click(sender As Object, e As EventArgs) Handles btnSend.Click
        Dim svtTrn As oledb.oledbTransaction = Nothing
        Dim strSQL As String = ""
        stringReplace(Me)
        If Trim(txtTitle.Text) = "" Then
            MsgBox("Please enter title.", vbInformation, Me.Text)
            Exit Sub
        End If
        If Trim(txtNotes.Text) = "" Then
            MsgBox("Please enter Note.", vbInformation, Me.Text)
            Exit Sub
        End If

        If vsfContact.Rows.Count <= 0 Then
            MsgBox("Atleast one user has to be selected.", vbInformation, Me.Text)
            Exit Sub
        End If

        With vsfContact
            Try
                svtTrn = svtConn.BeginTransaction(IsolationLevel.ReadCommitted)
                For i = 0 To .Rows.Count - 1
                    strSQL = "Insert into utlNotify(ToUserCode, Title, NotifText, Status, FromUserCode) values("
                    strSQL &= "'" & .GetData(i, vName) & "',"
                    strSQL &= "'" & txtTitle.Text & "',"
                    strSQL &= "'" & Replace(txtNotes.Text, vbCrLf, " ") & "',"
                    strSQL &= "'N',"
                    strSQL &= "'" & gUserCode & "')"
                    If Not clsSVT.execSQLStatement(strSQL, svtTrn) Then
                        svtTrn.Rollback()
                        Exit Sub
                    End If
                Next
                svtTrn.Commit()
                MsgBox("Data Sent Successfully.", vbInformation, Me.Text)
                Call UpdateINIFile()
                Call ClearFlds()
            Catch ex As Exception
                MsgBox("Error during Save" & vbCrLf & ex.Message, MsgBoxStyle.Critical, Me.Text & "[btnSend_Click]")
                svtTrn.Rollback()
            End Try
        End With
    End Sub

    Private Sub txtNotes_TextChanged(sender As Object, e As EventArgs) Handles txtNotes.TextChanged
        TextBoxSizeChanged(txtNotes, e)
    End Sub

    Private Sub btnDelAll_Click(sender As Object, e As EventArgs) Handles btnDelAll.Click
        Dim strSql As String
        Dim svtTrn As oledb.oledbTransaction = Nothing
        If vsfDet.Row < 1 Then
            Exit Sub
        End If

        If vsfDet.Rows.Count = 2 Then
            MsgBox("No data present to delete!", vbInformation + vbOKOnly, Me.Text)
            Exit Sub
        End If

        If MsgBox("Are you sure you want to delete all the data ?", vbYesNo + vbQuestion + vbDefaultButton1, Me.Text) = vbYes Then


            svtTrn = svtConn.BeginTransaction(IsolationLevel.ReadCommitted)
            Try
                With vsfDet
                    strSql = "Delete from UtlNotify where FromUserCode  = '" & gUserCode & "'"
                    clsSVT.execSQLStatement(strSql, svtTrn)
                End With
                svtTrn.Commit()
                MsgBox("All Data deleted successfully.", vbInformation, Me.Text)
                frmMain.intNotifCount = clsSVT.getDataLongValue("Select count(*) from UtlNotify where FromUserCode  = '" & gUserCode & "'", 0, False)
                frmMain.lblNotify.Text = frmMain.intNotifCount
            Catch ex As Exception
                MsgBox("Error during Deletion" & vbCrLf & ex.Message, MsgBoxStyle.Critical, Me.Text & "[btnDelAll_Click]")
                svtTrn.Rollback()
                Exit Sub
            End Try

        End If
    End Sub

    Private Sub btnpnlClose_Click(sender As Object, e As EventArgs) Handles btnpnlClose.Click
        'pnlList.Visible = False
        'pnlList.SendToBack()
        'pnlNotify.Visible = True
        Me.Close()
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs)
        Me.Close()
    End Sub

    Private Sub frmUtlNotification_KeyUp(sender As Object, e As KeyEventArgs) Handles Me.KeyUp
        If e.KeyCode = Keys.F9 And e.Control = True Then
            Call initFormSize(Me, Me.Text, True, 756, 483, 0, 0)
            MsgBox("Restored Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
        If e.KeyCode = Keys.F8 And e.Control = True Then
            Call SaveActFormSettings(Me, Me.Text)
            Call SaveActFormVsfDet(Me, Me.Text, "DET", vsfDet)
            MsgBox("Saved Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
        If e.KeyCode = Keys.F12 And e.Alt = True Then
            ExportToXL(vsfDet)
        End If
    End Sub

    Private Sub btnRecvList_Click(sender As Object, e As EventArgs) Handles btnRecvList.Click
        blnSent = False
        pnlList.Visible = True
        pnlList.BringToFront()
        Call fillNotifications(False)
    End Sub

    Private Sub btnSentList_Click(sender As Object, e As EventArgs) Handles btnSentList.Click
        blnSent = True
        pnlList.Visible = True
        pnlList.BringToFront()
        Call fillNotifications(True)

    End Sub

    Private Sub btnNew_Click(sender As Object, e As EventArgs) Handles btnNew.Click
        txtTitle.Focus()
        pnlNotify.Visible = True
        pnlList.Enabled = False
        pnlNotify.BringToFront()
        Call fillNotifications(True)
        Me.Text = "Notification from Current User : " & gUserCode
    End Sub

    Private Sub vsfSearch_KeyUp(sender As Object, e As KeyEventArgs) Handles vsfSearch.KeyUp
        If e.KeyCode = Keys.Return Then
            Call vsfSearchSelectedItem()
            Exit Sub
        End If
    End Sub

    Private Sub vsfSearchNotif_AfterScroll(sender As Object, e As RangeEventArgs) Handles vsfSearchNotif.AfterScroll
        Dim intv As Integer = vsfSearchNotif.ScrollPosition.X
        Dim inth As Integer = vsfDet.ScrollPosition.Y
        vsfDet.ScrollPosition = New System.Drawing.Point(intv, inth)
    End Sub

    Private Sub vsfSearchNotif_ChangeEdit(sender As Object, e As EventArgs) Handles vsfSearchNotif.ChangeEdit
        Call applyFilterFromGrid()
    End Sub

    Private Function applyFilterFromGrid() As Boolean
        Dim StrQry As String, strValue As String
        Dim i As Integer
        applyFilterFromGrid = False
        Try
            StrQry = ""
            With vsfSearchNotif
                For i = 0 To .Cols.Count - 1
                    strValue = ""
                    If i = .ColSel Then
                        If .Editor.Text <> "" Then
                            strValue = Trim(.Editor.Text)
                        End If
                    Else
                        If .GetData(0, i) <> "" Then
                            strValue = Trim(.GetData(0, i))
                        End If
                    End If
                    If strValue <> "" Then
                        If StrQry <> "" Then StrQry = StrQry & " and "
                        Select Case gDBTypeCurrent
                            Case gDbTypeAccess
                                StrQry = StrQry & vsfDet.GetData(0, i) & " like '*" & strValue & "*'"
                            Case Else
                                'If vsfDet.Cols(i).DataType = GetType(Decimal) Or vsfDet.Cols(i).DataType = GetType(Integer) Then
                                '    Try
                                '        Dim strTest = Convert.ToDecimal(strValue)
                                '        StrQry = StrQry & vsfDet.GetData(3, i) & " >= " & strValue
                                '    Catch ex As Exception
                                '        ' vsfSearch.SetData(0, i, "")
                                '        dv.RowFilter = "1 <> 1"
                                '        Exit Function
                                '    End Try
                                If vsfDet.Cols(i).DataType = GetType(Decimal) Or vsfDet.Cols(i).DataType = GetType(Integer) Or vsfDet.Cols(i).DataType = GetType(Date) Then
                                    Try
                                        StrQry = StrQry & "Convert(" & vsfDet.GetData(0, i) & ",'System.String')" & " like '" & strValue & "*'"
                                    Catch ex As Exception
                                        dvSearch.RowFilter = "1 <> 1"
                                        Exit Function
                                    End Try
                                Else
                                    StrQry = StrQry & vsfDet.GetData(0, i) & " like '%" & strValue & "%'"
                                End If
                        End Select
                    End If
                Next i
            End With
            'If StrQry <> "" Then
            '    vsfDet.DataSource = dt.Select(StrQry)
            'Else
            '    vsfDet.DataSource = dt
            'End If
            Dim k As Integer = vsfDet.Rows.Count
            dvSearch.RowFilter = StrQry
            'lblTotRec.Text = "Total Records: " & Val(Val(vsfDet.Rows.Count) - 5) & "/" & intTotRec
            Call syncGridCols()
            applyFilterFromGrid = True
        Catch ex As Exception

        End Try

    End Function

    Private Sub syncGridCols()
        Dim i As Integer
        With vsfDet
            vsfSearchNotif.Cols.Count = vsfDet.Cols.Count
            For i = 0 To .Cols.Count - 1
                vsfSearchNotif.Cols(i).Width = vsfDet.Cols(i).Width
                vsfSearchNotif.Cols(i).Visible = vsfDet.Cols(i).Visible
            Next i
        End With
    End Sub

    Private Sub vsfSearchNotif_KeyUpEdit(sender As Object, e As KeyEditEventArgs) Handles vsfSearchNotif.KeyUpEdit
        If e.KeyCode = Keys.Down Then
            vsfDet.Row = 0
            vsfDet.Col = vsfSearchNotif.Col
            vsfDet.Focus()
        End If
    End Sub

    Private Sub vsfDet_AfterResizeColumn(sender As Object, e As RowColEventArgs) Handles vsfDet.AfterResizeColumn
        Call syncGridCols()
        vsfDet.AutoSizeRows()
    End Sub

    Private Sub vsfDet_AfterScroll(sender As Object, e As RangeEventArgs) Handles vsfDet.AfterScroll
        vsfSearchNotif.ScrollPosition = vsfDet.ScrollPosition
    End Sub

    Private Sub UpdateINIFile()
        Dim strSQL As String = ""

        strSQL = " select * from utlNotify Where FromUserCode='" & gUserCode & "' and Title = '" & Trim(txtTitle.Text) & "' "
        Dim rs As oledb.oledbDataReader

        rs = clsSVT.getDataReaderFromSQL(strSQL)
        If Not rs.HasRows Then
            rs.Close()
            Exit Sub
        End If

        Do While rs.Read
            Try
                Dim strINIFileName As String = RemoveSplChar(UCase(rs.Item("ToUserCode")))

                If Not IO.File.Exists(gstrRptPath & "Notification\" & strINIFileName & ".ini") Then
                    IO.File.WriteAllText(gstrRptPath & "Notification\" & strINIFileName & ".ini", "N")
                End If

                IO.File.WriteAllText(gstrRptPath & "Notification\" & strINIFileName & ".ini", "Y")
            Catch ex As Exception

            End Try
        Loop
    End Sub

    Private Sub frmUtlNotification_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing

    End Sub
End Class