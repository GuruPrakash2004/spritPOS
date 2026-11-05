Imports C1.Win.C1FlexGrid

Public Class frmUtlPostDocRpt

    Const vDetID = 0
    Const vDetSlno = 1
    Const vDetTblName = 2
    Const vDetFldName = 3
    Const vDetCaption = 4
    Const vDetDtFilter = 5
    Const vDetColHide = 6
    Const vDetAllowEdit = 7

    Const vPKID = 0
    Const vPKSlno = 1
    Const vPKTblName = 2
    Const vPKFldName = 3

    Dim clsSVT As New svtLibrary
    Public intPostDocID As Integer
    Public strCond As String
    Public strPendCond As String
    Public strPDEName As String

    Dim dtDtl As New DataTable
    Dim dtPKDtl As New DataTable

    Dim svtTrn As oledb.oledbTransaction

    Private Sub frmUtlPostDocRpt_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.KeyPreview = True
        Me.Text = strPDEName
        Call fillFormProperties(Me)
        Call initGridStyles(Me)
        Call initGrid()
        Call LoadPDE()
        Call initFormVsfDet(Me, Me.Text, vsfDet, "DET")
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Call frmMain.CloseSelectedTabPage(Me)
    End Sub

    Private Sub initGrid()
        If intPostDocID <= 0 Then
            MsgBox("Invalid PDE document.....", MsgBoxStyle.Exclamation, Me.Text)
            Me.Close()
        End If

        dtDtl = clsSVT.getDATATable("select PostDocID, Slno, TblName, FldName, FldCaption, DateFilter, ColHide, AllowEdit from UtlPDEDtl where PostDocID=" & intPostDocID)
        dtPKDtl = clsSVT.getDATATable("select PostDocID, Slno,TblName,FldName from UtlPDEPK where PostDocID=" & intPostDocID)

        Dim blFound As Boolean
        clsSVT.getDataStringValue("Select Top 1 dateFilter from UtlPDEDtl where dateFilter='Y' and PostDocID= " & intPostDocID, "", blFound)
        If blFound Then
            pnlDtRng.Visible = True
            chkDtRng.Checked = True
            dtpFromDt.Value = getFirstDateInMonth(Date.Today)
        Else
            pnlDtRng.Visible = False
        End If

        Dim strCondqry As String
        Dim strPendQry As String
        strCondqry = clsSVT.getDataStringValue("Select CondText from UtlPDE where PostDocID=" & intPostDocID, "", False)
        strPendQry = clsSVT.getDataStringValue("Select PendCond from UtlPDE where PostDocID=" & intPostDocID, "", False)
        If strPendQry = "" Then
            pnlPending.Visible = False
        Else
            pnlPending.Visible = True
            rdoPending.Checked = True
        End If

        With vsfDet
            .Rows.Count = 1
            .Cols.Count = 0

            For i = 0 To dtDtl.Rows.Count - 1
                .Cols.Count = .Cols.Count + 1
                .SetData(0, .Cols.Count - 1, dtDtl.Rows(i).Item(vDetCaption))
                Dim strDataType As String = GetDataType(dtDtl.Rows(i).Item(vDetTblName), dtDtl.Rows(i).Item(vDetFldName))
                Select Case strDataType.ToUpper()
                    Case "DATETIME", "SMALLDATETIME", "DATE"
                        .Cols(.Cols.Count - 1).DataType = GetType(Date)
                    Case "INT", "DOUBLE", "NUMERIC", "DECIMAL", "FLOAT"
                        .Cols(.Cols.Count - 1).DataType = GetType(Double)
                    Case Else
                End Select
                If dtDtl.Rows(i).Item(vDetColHide) = "Y" Then
                    .Cols(i).Visible = False
                End If
                If dtDtl.Rows(i).Item(vDetAllowEdit) = "N" Then Call SetFlexColor(vsfDet, "", i)
                If dtDtl.Rows(i).Item(vDetAllowEdit) = "Y" Then Call SetFlexColor(vsfDet, "M", i)

            Next
        End With
    End Sub

    Private Sub LoadPDE()
        Dim strSQL As String = ""
        Dim strTblList As String = ""
        strSQL = " select distinct stuff((select ', ' + Tblname "
        strSQL &= " from  UtlPDEDtl v  "
        strSQL &= " Where v.PostDocID = u.postdocid  "
        strSQL &= " Group by TblName "
        strSQL &= " FOR XML PATH('')),1,2,'') as tbls "
        strSQL &= " From UtlPDEDtl u "
        strSQL &= " where PostDocID=" & intPostDocID
        strTblList = clsSVT.getDataStringValue(strSQL, "", False)

        With dtDtl
            strSQL = " Select "
            For i As Int32 = 0 To .Rows.Count - 1
                strSQL &= .Rows(i).Item(vDetTblName) & "." & .Rows(i).Item(vDetFldName) & ","
            Next
            strSQL = Mid(strSQL, 1, Len(strSQL) - 1)
            strSQL &= " From " & strTblList
            strSQL &= " Where 1=1 "
            If strCond <> "" Then strSQL &= strCond
            If pnlPending.Visible AndAlso strPendCond <> "" AndAlso rdoPending.Checked Then strSQL &= " " & strPendCond
            If pnlDtRng.Visible AndAlso chkDtRng.Checked Then
                For i As Int32 = 0 To .Rows.Count - 1
                    If GetVal(.Rows(i).Item(vDetDtFilter), "") = "Y" Then
                        strSQL &= " and " & .Rows(i).Item(vDetTblName) & "." & .Rows(i).Item(vDetFldName) & " Between "
                        strSQL &= getDateFormatInCond(dtpFromDt.Value) & " and "
                        strSQL &= getDateFormatInCond(dtpToDt.Value)
                    End If
                Next
            End If
        End With

        Dim rs As oledb.oledbDataReader
        rs = clsSVT.getDataReaderFromSQL(strSQL)
        vsfDet.Rows.Count = 1

        With vsfDet
            If Not rs.HasRows Then
                rs.Close()
                MsgBox("No records....", MsgBoxStyle.Information, Me.Text)
                Exit Sub
            End If

            Do While rs.Read
                .Rows.Count = .Rows.Count + 1
                For j As Int32 = 0 To .Cols.Count - 1
                    .SetData(.Rows.Count - 1, j, rs.Item(j))
                Next
            Loop
            rs.Close()
        End With
    End Sub

    Private Sub chkDtRng_CheckedChanged(sender As Object, e As EventArgs) Handles chkDtRng.CheckedChanged
        If chkDtRng.Checked Then
            pnlDtRng.Enabled = True
        Else
            pnlDtRng.Enabled = False
            'lblFrom.Visible = True
            'lblTo.Visible = True
            'dtpFromDt.Visible = True
            'dtpToDt.Visible = True

        End If
    End Sub

    Private Sub vsfDet_BeforeEdit(sender As Object, e As RowColEventArgs) Handles vsfDet.BeforeEdit
        With dtDtl
            If IsNothing(dtDtl) Then Exit Sub
            If dtDtl.Rows.Count <= 0 Then Exit Sub
            If vsfDet.Rows.Count <= 0 Then Exit Sub
            If vsfDet.Cols.Count <= 0 Then Exit Sub
            If dtDtl.Rows.Count <> vsfDet.Cols.Count Then Exit Sub
            If .Rows(e.Col).Item(vDetAllowEdit) = "N" Then
                e.Cancel = True
            End If
        End With
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Try
            With vsfDet
                If .Rows.Count <= 1 Then
                    MsgBox("Grid is empty....", MsgBoxStyle.Exclamation, Me.Text)
                    Exit Sub
                End If

                Dim strEditTbl As String = ""
                With dtDtl
                    For i As Int32 = 0 To .Rows.Count - 1
                        If GetVal(.Rows(i).Item(vDetAllowEdit), "") = "Y" Then
                            strEditTbl = .Rows(i).Item(vDetTblName)
                            Exit For
                        End If
                    Next
                End With

                If strEditTbl = "" Then
                    MsgBox("Update Table Name is Missing....", MsgBoxStyle.Exclamation, Me.Text)
                    Exit Sub
                End If

                svtTrn = svtConn.BeginTransaction(IsolationLevel.ReadCommitted)
                Dim blnSaved As Boolean = False
                For i As Int32 = 1 To .Rows.Count - 1
                    For k As Int32 = 0 To .Cols.Count - 1
                        If GetVal(dtDtl.Rows(k).Item(vDetAllowEdit), "") = "Y" Then
                            If CStr(.GetData(i, k) & "") = "" Then GoTo NextRecord
                        End If
                    Next
                    If Not UpdateReport(i, strEditTbl, svtTrn) Then
                        Exit Sub
                    End If
                    blnSaved = True
NextRecord:
                Next

                svtTrn.Commit()
                If blnSaved Then
                    MsgBox("Updated Successfully....", MsgBoxStyle.Information, Me.Text)
                Else
                    MsgBox("Records Not Affected....", MsgBoxStyle.Exclamation, Me.Text)
                End If

                Call LoadPDE()
            End With
        Catch ex As Exception
            svtTrn.Rollback()
        End Try
    End Sub

    Private Function UpdateReport(ByVal argRow As Int32, ByVal argTblName As String, ByVal argTrn As oledb.oledbTransaction) As Boolean
        UpdateReport = False

        Dim strSQL As String = ""

        strSQL = "Update " & argTblName
        strSQL &= " Set "


        With dtDtl
            For i As Int32 = 0 To .Rows.Count - 1
                If GetVal(.Rows(i).Item(vDetAllowEdit), "") = "Y" Then
                    strSQL &= .Rows(i).Item(vDetTblName) & "." & .Rows(i).Item(vDetFldName)
                    If IsDate(vsfDet.GetData(argRow, i)) Then
                        strSQL &= " = " & getDateFormatInCond(vsfDet.GetData(argRow, i)) & ","
                    Else
                        strSQL &= " = '" & vsfDet.GetData(argRow, i) & "',"
                    End If
                End If
            Next
        End With

        strSQL = Mid(strSQL, 1, Len(strSQL) - 1)
        strSQL &= " Where "

        With dtPKDtl
            For i As Int32 = 0 To .Rows.Count - 1
                strSQL &= "" & .Rows(i).Item(vPKTblName) & "."
                strSQL &= "" & .Rows(i).Item(vPKFldName) & "="

                For j As Int32 = 0 To dtDtl.Rows.Count - 1
                    If UCase(dtDtl.Rows(j).Item(vDetTblName)) = UCase(dtPKDtl.Rows(i).Item(vPKTblName)) AndAlso UCase(dtDtl.Rows(j).Item(vDetFldName)) = dtPKDtl.Rows(i).Item(vPKFldName) Then
                        strSQL &= vsfDet.GetData(argRow, j)
                    End If
                Next
            Next
        End With

        If strSQL <> "" Then
            If Not clsSVT.execSQLStatement(strSQL, argTrn) Then
                UpdateReport = False
                argTrn.Rollback()
                Exit Function
            End If
        End If

        UpdateReport = True
    End Function

    Private Sub btnLoad_Click(sender As Object, e As EventArgs) Handles btnLoad.Click
        Call LoadPDE()
    End Sub

    Private Sub frmUtlPostDocRpt_KeyUp(sender As Object, e As KeyEventArgs) Handles Me.KeyUp
        If e.KeyCode = Keys.F9 And e.Control = True Then
            Call initFormVsfDet(Me, Me.Text, vsfDet, "DET")
            MsgBox("Restored Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
        If e.KeyCode = Keys.F8 And e.Control = True Then
            Call SaveActFormVsfDet(Me, Me.Text, "DET", vsfDet)
            MsgBox("Saved Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
    End Sub

    Private Sub vsfDet_DoubleClick(sender As Object, e As EventArgs) Handles vsfDet.DoubleClick
        'vsfDet.Focus()
        If vsfDet.Col = 4 Then
            Call showWaitCursor()
        End If
    End Sub

    'Private Sub vsfDet_KeyUp(sender As Object, e As KeyEventArgs) Handles vsfDet.KeyUp
    '    With vsfDet
    '        If e.KeyCode = Keys.Tab AndAlso .Row = .Rows.Count - 1 Then
    '            vsfDet.Focus()
    '            vsfDet.StartEditing()
    '        End If
    '    End With
    'End Sub

    Private Function GetDataType(ByVal argTblName As String, ByVal argFldName As String)
        Return clsSVT.getDataStringValue("Select distinct DATA_TYPE from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = '" & argTblName & "' and Column_Name = '" & argFldName & "'", "", False)
    End Function
End Class
