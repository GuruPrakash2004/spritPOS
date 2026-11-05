Imports C1.Win.C1FlexGrid

Public Class frmUtlValueMatrixEmails

    Dim clsSvt As New svtLibrary

    'vsfdet
    Const vDetSlno = 0
    Const vDetCrdtFrm = 1
    Const vDetCrdtTo = 2
    Const intDetCols = 3

    Private Sub frmUtlValueMatrixEmails_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        With vsfDet
            .Rows.Count = 2
            .Cols.Count = intDetCols
            .SetData(1, vDetSlno, 1)
            .SetData(0, vDetSlno, "SLNo")
            .SetData(0, vDetCrdtFrm, "Credit From")
            .SetData(0, vDetCrdtTo, "Credit To")
            SetFlexColor(vsfDet, "", vDetSlno)
            .AutoResize = True
        End With
        Call fillZones()
        Call fillCreditLimit()
        Call FillZoneEmails()

        Call initFormSize(Me, Me.Text, False, 921, 540, 0, 0)
        Call initFormVsfDet(Me, Me.Text, vsfDet, "Detail")
        Cursor.Current = Cursors.Arrow
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        If validFlds() Then
            If saveGrids() Then
                MsgBox("Saved successfully!", vbInformation, Me.Text)
                Call fillCreditLimit()
                Call FillZoneEmails()
                Cursor.Current = Cursors.Arrow
            End If
        End If
    End Sub

    Private Function validFlds() As Boolean
        Dim i As Integer
        validFlds = False
firstRow:

        With vsfDet
            For i = 1 To .Rows.Count - 1
                If Val(.GetData(i, vDetCrdtFrm)) = 0 Or Val(.GetData(i, vDetCrdtTo)) = 0 Then
                    MsgBox("Please fill the required details!!", vbInformation, Me.Text)
                    .Row = i
                    .Focus()
                    Exit Function
                End If
                If Val(.GetData(i, vDetCrdtFrm)) = 0 Or Val(.GetData(i, vDetCrdtTo)) = 0 Then
                    Call RemoveRow(vsfDet, vDetSlno)
                    GoTo firstRow
                End If
            Next i
        End With
        validFlds = True
    End Function

    Public Sub RemoveRow(argVsfGrid As Object, Optional argSLNoCol As Integer = -1)
        With argVsfGrid
            If .Rows > 1 Then
                If .Row <> 0 Then
                    .RemoveItem(.Row)
                    If .Rows = 1 Then
                        If argSLNoCol <> -1 Then
                            .AddItem(1)
                        Else
                            .AddItem(1)
                        End If
                    End If
                End If
            End If
        End With
        If argSLNoCol <> -1 Then
            Call ReArrangeSlNo(argVsfGrid, argSLNoCol)
        End If
    End Sub

    Private Function saveGrids() As Boolean
        Dim i As Integer, j As Integer
        Dim strsql As String
        Dim svtTrn As oledb.oledbTransaction
        saveGrids = False

        Cursor.Current = Cursors.WaitCursor
        svtTrn = svtConn.BeginTransaction(IsolationLevel.ReadCommitted)
        Try
            clsSvt.execSQLStatement("Delete from UtlCrLimit", svtTrn)

            With vsfDet
                For i = 1 To .Rows.Count - 1
                    If Val(.GetData(i, vDetCrdtFrm)) > 0 And Val(.GetData(i, vDetCrdtTo)) > 0 Then
                        For j = intDetCols To vsfDet.Cols.Count - 1
                            If .GetData(i, j) <> "" Then
                                strsql = "Insert into UtlCrLimit( CRFrom, CRTo, EmailID,ZoneID )values("
                                strsql = strsql & .GetData(i, vDetCrdtFrm) & ","
                                strsql = strsql & .GetData(i, vDetCrdtTo) & ","
                                strsql = strsql & "'" & Trim(.GetData(i, j)) & "',"
                                strsql = strsql & clsSvt.getDataLongValue("Select ZoneID from MstZone where ZoneCode = '" & Trim(.GetData(0, j)) & "'", 0, False, svtTrn) & ")"
                                strsql = Replace(Replace(strsql, vbCrLf, ""), vbCr, "")
                                clsSvt.execSQLStatement(strsql, svtTrn)
                            End If
                        Next j
                    End If
                Next i
            End With
            saveGrids = True
            svtTrn.Commit()
            Exit Function
        Catch ex As Exception
            saveGrids = False
            svtTrn.Rollback()
            MsgBox(ex.Message, MsgBoxStyle.Information, Me.Text)
        End Try
    End Function

    Private Sub fillCreditLimit()
        Dim rstDummy As oledb.oledbDataReader = Nothing
        Dim i As Integer
        Cursor.Current = Cursors.WaitCursor
        Try
            rstDummy = clsSvt.getDataReaderFromSQL("Select Distinct CRFrom, CRTo from UtlCrLimit")

            vsfDet.Rows.Count = 1
            i = 0
            With rstDummy
                If Not .HasRows Then
                    vsfDet.Rows.Count = 1
                    vsfDet.Rows.Count = 2
                    vsfDet.SetData(1, vDetSlno, 1)
                    Exit Sub
                End If
                Do While .Read
                    vsfDet.Rows.Count = vsfDet.Rows.Count + 1
                    i = i + 1
                    vsfDet.SetData(i, vDetSlno, i)
                    vsfDet.SetData(i, vDetCrdtFrm, GetVal(!CRFrom, ""))
                    vsfDet.SetData(i, vDetCrdtTo, GetVal(!CRTo, ""))
                Loop
                .Close()
            End With
            Cursor.Current = Cursors.Arrow
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information, Me.Text)
            Cursor.Current = Cursors.Arrow
        End Try

    End Sub

    Private Sub vsfDet_KeyPressEdit(sender As Object, e As KeyPressEditEventArgs) Handles vsfDet.KeyPressEdit
        If e.KeyChar = "'" Then
            e.Handled = True
        End If
        With vsfDet
            Select Case e.Col
                Case vDetCrdtFrm, vDetCrdtTo
                    If Not KeyValidate("Integer", e.KeyChar) Then
                        e.Handled = True
                    End If
            End Select
        End With
    End Sub

    Private Sub vsfDet_BeforeEdit(sender As Object, e As RowColEventArgs) Handles vsfDet.BeforeEdit
        With vsfDet
            Select Case e.Col
                Case vDetSlno
                    e.Cancel = True
            End Select
        End With
    End Sub

    Private Sub vsfDet_KeyUp(sender As Object, e As KeyEventArgs) Handles vsfDet.KeyUp
        With vsfDet

            'If (e.KeyCode = Keys.Escape) And .Row = .Rows.Count - 1 Then
            '    If Trim(.GetData(.Row, vDetEmail)) = "" And Trim(.GetData(.Row, vDetCrdtFrm)) = "" _
            '        And Trim(.GetData(.Row, vDetEmail)) = "" Then
            '        btnSave.Focus()
            '        Exit Sub
            '    End If
            'End If

            If .Col = .Cols.Count - 1 And e.KeyCode = Keys.Tab And .Row = .Rows.Count - 1 Then
                If Trim(.GetData(.Row, vDetCrdtFrm)) <> "" And Trim(.GetData(.Row, vDetCrdtTo)) <> "" Then
                    .LeftCol = vDetCrdtFrm
                    .Col = vDetCrdtFrm
                    .Rows.Count = .Rows.Count + 1
                    .TopRow = .Rows.Count - 1
                    .Row = .Rows.Count - 1
                    .Focus()
                End If
            End If
            If e.KeyCode = Keys.D And e.Shift = 2 Then
                If MsgBox("Do You want to Remove the Credit Limit ?", vbInformation + vbYesNo, Me.Text) = vbYes Then
                    vsfDet.RemoveItem(vsfDet.Row)
                End If
            End If
            ReArrangeSlNo(vsfDet, vDetSlno)
        End With
    End Sub

    Private Sub btnRemove_Click(sender As Object, e As EventArgs) Handles btnRemove.Click
        If vsfDet.Rows.Count < 2 Then Exit Sub
        If MsgBox("Do You want to Remove the Row ?", vbInformation + vbYesNo, Me.Text) = vbYes Then
            vsfDet.RemoveItem(vsfDet.Row)
        End If
        If vsfDet.Rows.Count = 1 Then vsfDet.Rows.Count = vsfDet.Rows.Count + 1
        ReArrangeSlNo(vsfDet, vDetSlno)
    End Sub

    Private Sub fillZones()
        Dim strSQL As String
        Dim rsZ As oledb.oledbDataReader = Nothing
        strSQL = "Select ZoneCode from MstZone where ZoneID>0 order by ZoneID"
        rsZ = clsSvt.getDataReaderFromSQL(strSQL)
        vsfDet.Cols.Count = intDetCols
        With rsZ
            If Not .HasRows Then
                vsfDet.Cols.Count = intDetCols
                Exit Sub
            End If
            Do While .Read
                vsfDet.Cols.Count = vsfDet.Cols.Count + 1
                vsfDet.SetData(0, vsfDet.Cols.Count - 1, !ZoneCode)
            Loop
            .Close()
        End With

    End Sub

    Private Sub frmUtlValueMatrixEmails_KeyUp(sender As Object, e As KeyEventArgs) Handles Me.KeyUp
        If e.KeyCode = Keys.F9 And e.Control Then
            Call initFormSize(Me, Me.Text, True, 921, 540, 0, 0)
            Call initFormVsfDet(Me, Me.Text, vsfDet, "Detail")
            MsgBox("Restored Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
        If e.KeyCode = Keys.F8 And e.Control = True Then
            Call SaveActFormSettings(Me, Me.Text)
            Call SaveActFormVsfDet(Me, Me.Text, "Detail", vsfDet)
            MsgBox("Saved Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
    End Sub

    Private Sub FillZoneEmails()
        Dim strSQL As String
        Dim rsZ As oledb.oledbDataReader = Nothing
        strSQL = "select u.*,ZoneCode from UtlCRLimit u, MstZone z where u.ZoneID=z.ZoneID"
        rsZ = clsSvt.getDataReaderFromSQL(strSQL)
        With rsZ
            If Not .HasRows Then
                vsfDet.Cols.Count = intDetCols
                Exit Sub
            End If
            Do While .Read
                For i = 1 To vsfDet.Rows.Count - 1
                    If vsfDet.GetData(i, vDetCrdtFrm) = GetVal(!CRFrom, 0) And vsfDet.GetData(i, vDetCrdtTo) = GetVal(!CRTo, 0) Then
                        For j = intDetCols To vsfDet.Cols.Count - 1
                            If vsfDet.GetData(0, j) = GetVal(!Zonecode, "") Then
                                vsfDet.SetData(i, j, GetVal(!EmailID, ""))
                                Exit For
                            End If
                        Next
                    End If
                Next
            Loop
            .Close()
        End With
    End Sub

End Class