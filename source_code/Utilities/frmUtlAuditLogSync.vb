Imports C1.Win.C1FlexGrid
Imports System.Data.oledb
Public Class frmUtlAuditLogSync
    Dim strSQL As String
    Dim clsSVT As New svtLibrary

    Const vDetDocID = 0
    Const vDetShName = 1
    Const vDetHdrTblName = 2
    Const vDetDtlTblName = 3
    Const vDetDocType = 4
    Const vDetDocName = 5
    Const vDetLogTrack = 6
    Const vDetTotCol = 7

    Private Sub frmUtlAuditLogSync_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Call fillFormProperties(Me)
        Call initGridStyles(Me)
        Call initScreen()
        Call fillDtls()
        Call initFormVsfDet(Me, Me.Text, vsfDet, "DET")
        Me.KeyPreview = True
    End Sub

    Private Sub initScreen()
        With vsfDet
            .Rows.Count = 1
            .Cols.Count = vDetTotCol

            .SetData(0, vDetDocID, "DocID")
            .SetData(0, vDetShName, "ShName")
            .SetData(0, vDetHdrTblName, "Hdr Table Name")
            .SetData(0, vDetDtlTblName, "Dtl Table Name")
            .SetData(0, vDetDocType, "Document Type")
            .SetData(0, vDetDocName, "Document Name")
            .SetData(0, vDetLogTrack, "Log Track Required")
            .Cols(vDetDocType).AllowMerging = True
            SetHideCol(vsfDet, True, vDetDocID, vDetShName, vDetHdrTblName, vDetDtlTblName)
            SetFlexColorFullGrid(vsfDet, "")
            SetColAlign(vsfDet, svtAlign.Center, vDetLogTrack)
        End With

        txtLogDB.Text = gStrLogDBName
    End Sub

    Private Sub fillDtls()
        Dim rs As oledb.oledbDataReader

        strSQL = "Select distinct DocID, DocShForm, DocType, DocName, HdrTblName, DtlTblName, AuditLogTrack"
        strSQL &= " From utltransactions"
        strSQL &= " Where isnull(hdrtblname,'') <> ''"
        strSQL &= " Order by DocType, AuditLogTrack desc, DocName"

        rs = clsSVT.getDataReaderFromSQL(strSQL)

        vsfDet.Rows.Count = 1
        If Not rs.HasRows Then
            rs.Close()
            Exit Sub
        End If

        With rs
            Do While .Read
                vsfDet.Rows.Count += 1
                vsfDet.SetData(vsfDet.Rows.Count - 1, vDetDocID, Val(!DocID))
                vsfDet.SetData(vsfDet.Rows.Count - 1, vDetShName, GetVal(!DocShForm, ""))
                vsfDet.SetData(vsfDet.Rows.Count - 1, vDetHdrTblName, GetVal(!HdrTblName, ""))
                vsfDet.SetData(vsfDet.Rows.Count - 1, vDetDtlTblName, GetVal(!DtlTblName, ""))
                vsfDet.SetData(vsfDet.Rows.Count - 1, vDetDocType, GetVal(!DocType, ""))
                vsfDet.SetData(vsfDet.Rows.Count - 1, vDetDocName, GetVal(!DocName, ""))
                vsfDet.SetData(vsfDet.Rows.Count - 1, vDetLogTrack, GetVal(!AuditLogTrack, ""))
                If GetVal(!AuditLogTrack, "") = "Y" Then
                    Call setVsfCellColour(vsfDet, vsfDet.Rows.Count - 1, vDetDocName, Color.ForestGreen, Color.Black, True)
                Else
                    vsfDet.SetData(vsfDet.Rows.Count - 1, vDetLogTrack, "")
                End If
            Loop
            .Close()
        End With
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Call frmMain.CloseSelectedTabPage(Me)
    End Sub

    Private Sub frmUtlAuditLogSync_KeyUp(sender As Object, e As KeyEventArgs) Handles Me.KeyUp
        If e.KeyCode = Keys.F9 And e.Control Then
            Call initFormVsfDet(Me, Me.Text, vsfDet, "DET")
            MsgBox("Restored Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If

        If e.KeyCode = Keys.F8 And e.Control = True Then
            Call SaveActFormVsfDet(Me, Me.Text, "DET", vsfDet)
            MsgBox("Saved Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If

        If e.Control AndAlso e.KeyCode = Keys.F11 Then
            If vsfDet.Row > 0 AndAlso vsfDet.Col = vDetLogTrack Then
                If vsfDet.GetData(vsfDet.Row, vDetLogTrack) <> "Y" Then Call updateAuditLogTrack(vsfDet.Row)
            End If
        End If
    End Sub

    Private Sub vsfDet_BeforeEdit(sender As Object, e As RowColEventArgs) Handles vsfDet.BeforeEdit
        e.Cancel = True
    End Sub

    Private Sub txtLogDB_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtLogDB.KeyPress
        e.Handled = True
    End Sub

    Private Sub btnSync_Click(sender As Object, e As EventArgs) Handles btnSync.Click
        Dim i As Integer, strHdrTblName As String, strDtlTblname As String
        Call showWaitCursor()
        If clsSVT.getDataLongValue("Select database_id from sys.databases where upper(name) = '" & UCase(gStrLogDBName) & "'", 0, False) <= 0 Then
            If Not clsSVT.execSQLStatement("Create Database " & gStrLogDBName & " COLLATE  SQL_Latin1_General_CP1_CI_AS") Then
                Call showNormalCursor()
                MsgBox("Log-Database NOT Created!!", MsgBoxStyle.Critical, Me.Text)
                Exit Sub
            End If
        End If

        With vsfDet
            For i = 1 To .Rows.Count - 1
                If GetVal(.GetData(i, vDetLogTrack), "") = "Y" Then
                    Call clsSVT.execSQLStatement("Update utlTransactions Set AuditSQLhdr = '', AuditSQLDtl = '' Where DocID = " & .GetData(i, vDetDocID))

                    strHdrTblName = GetVal(.GetData(i, vDetHdrTblName), "")
                    If fnTablePresent(strHdrTblName) Then
                        If Not fnTablePresent(gStrLogDBName & ".." & strHdrTblName) Then
                            Call createTbl(strHdrTblName, "N")
                        Else
                            'check for Extra columns in LogDB
                            Call chkRemoveExtraLogTblColumns(strHdrTblName)

                            'check for Missing columns in LogDB
                            Call chkAddMissingLogTblColumns(strHdrTblName)
                        End If
                        Call updateAuditSQL(strHdrTblName, .GetData(i, vDetDocID), .GetData(i, vDetShName), False)
                    End If

                    strDtlTblname = GetVal(.GetData(i, vDetDtlTblName), "")
                    If strDtlTblname <> "" Then
                        If Not fnTablePresent(gStrLogDBName & ".." & strDtlTblname) Then
                            Call createTbl(strDtlTblname, "Y")
                        Else
                            'check for Extra columns in LogDB
                            Call chkRemoveExtraLogTblColumns(strDtlTblname)

                            'check for Missing columns in LogDB
                            Call chkAddMissingLogTblColumns(strDtlTblname)
                        End If
                        Call updateAuditSQL(strDtlTblname, .GetData(i, vDetDocID), .GetData(i, vDetShName), True)
                    End If
                End If
            Next i
        End With
        Call showNormalCursor()
        MsgBox("Sync Successfully Completed!!", vbInformation, Me.Text)
    End Sub

    Private Sub createTbl(argTblName As String, argDtl As String)
        Dim cmd As oledbCommand = New oledbCommand("mSp_CreateLogTable")
        Try
            With cmd
                cmd.Connection = svtConn
                .CommandType = CommandType.StoredProcedure
                .Parameters.Add("@table_name", OleDbType.VarChar, 50, ParameterDirection.Input)
                .Parameters(0).Value = "dbo." + argTblName
                .Parameters.Add("@logDBName", OleDbType.VarChar, 50, ParameterDirection.Input)
                .Parameters(1).Value = gStrLogDBName
                .Parameters.Add("@Dtl", OleDbType.Char, 1, ParameterDirection.Input)
                .Parameters(2).Value = argDtl
                .ExecuteNonQuery()
            End With

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub chkRemoveExtraLogTblColumns(argTblName As String)
        argTblName = UCase(argTblName)
        Dim rs As oledb.oledbDataReader

        strSQL = "select c.name"
        strSQL &= " from " & gStrLogDBName & ".sys.columns c, " & gStrLogDBName & ".sys.tables t"
        strSQL &= " where upper(t.name) = '" & argTblName & "'"
        strSQL &= " and c.object_id =  t.object_id"
        strSQL &= " and c.is_computed = 0"
        strSQL &= " and upper(c.name) not in ("
        strSQL &= " select c.name"
        strSQL &= " from " & gStrDBName & ".sys.columns c, " & gStrDBName & ".sys.tables t"
        strSQL &= " where upper(t.name) = '" & argTblName & "'"
        strSQL &= " and c.object_id =  t.object_id)"
        strSQL &= " and c.name not in ("
        strSQL &= " 'RecId','LogUserName','UserAction','ActionDateTime','LogSysName'"
        strSQL &= " ,'LogSysUserName','LogIPAddress')"

        rs = clsSVT.getDataReaderFromSQL(strSQL)

        If Not rs.HasRows Then
            rs.Close()
            Exit Sub
        End If

        Try
            With rs
                Do While .Read
                    clsSVT.execSQLStatement("Alter Table " & gStrLogDBName & ".." & argTblName & " Drop Column " & GetVal(!name, "") & "")
                Loop
                rs.Close()
            End With
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub chkAddMissingLogTblColumns(argTblName As String)
        argTblName = UCase(argTblName)
        Dim rs As oledb.oledbDataReader

        strSQL = "select c.name as colName, typ.name as dataTypeName, c.max_length as MaxLen, c.precision as intPlaces, c.scale as decPlaces"
        strSQL &= " from " & gStrDBName & ".sys.columns c, " & gStrDBName & ".sys.tables t, sys.types typ"
        strSQL &= " where upper(t.name) = '" & argTblName & "'"
        strSQL &= " and c.object_id = t.object_id"
        strSQL &= " and c.system_Type_ID = typ.system_Type_ID"
        strSQL &= " and c.is_computed = 0"
        strSQL &= " and upper(c.name) not in ("
        strSQL &= " select c.name"
        strSQL &= " from " & gStrLogDBName & ".sys.columns c, " & gStrLogDBName & ".sys.tables t"
        strSQL &= " where upper(t.name) = '" & argTblName & "'"
        strSQL &= " and c.object_id =  t.object_id)"

        rs = clsSVT.getDataReaderFromSQL(strSQL)

        If Not rs.HasRows Then
            rs.Close()
            Exit Sub
        End If

        Try
            With rs
                Do While rs.Read()
                    Select Case UCase(GetVal(!dataTypeName, ""))
                        Case "IMAGE", "DATE", "DATETIME", "SMALLDATETIME", "TIME", "TIMESTAMP", "BIGINT", "INT", "SMALLINT", "TINYINT"
                            strSQL = "Alter Table " & gStrLogDBName & ".." & argTblName & ""
                            strSQL &= " Add " & GetVal(!colName, "") & " " & GetVal(!dataTypeName, "") & " not null"
                            clsSVT.execSQLStatement(strSQL)
                        Case "DECIMAL", "FLOAT", "MONEY", "NUMERIC", "REAL", "SMALLMONEY"
                            strSQL = "Alter Table " & gStrLogDBName & ".." & argTblName & ""
                            strSQL &= " Add " & GetVal(!colName, "") & " " & GetVal(!dataTypeName, "")
                            strSQL &= "(" & GetVal(!intPlaces, "") & "," & GetVal(!decPlaces, "") & ") not null"
                            clsSVT.execSQLStatement(strSQL)
                        Case "CHAR", "NCHAR", "NTEXT", "NVARCHAR", "VARCHAR"
                            strSQL = "Alter Table " & gStrLogDBName & ".." & argTblName & ""
                            strSQL &= " Add " & GetVal(!colName, "") & " " & GetVal(!dataTypeName, "")
                            strSQL &= "(" & GetVal(!MaxLen, "") & ") not null"
                            clsSVT.execSQLStatement(strSQL)
                    End Select
                Loop
            End With
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub updateAuditSQL(argTblName As String, argDocID As Int32, argShName As String, blnDtl As Boolean)
        Dim rs As oledb.oledbDataReader

        strSQL = "Select  Stuff("
        strSQL &= " ( Select ', ' + C.COLUMN_NAME"
        strSQL &= " From INFORMATION_SCHEMA.COLUMNS As C"
        strSQL &= " Where C.TABLE_SCHEMA = T.TABLE_SCHEMA"
        strSQL &= " And C.TABLE_NAME = T.TABLE_NAME"
        strSQL &= " Order By C.ORDINAL_POSITION"
        strSQL &= " For Xml Path('')"
        strSQL &= " ), 1, 2, '') As Columns"
        strSQL &= " From INFORMATION_SCHEMA.TABLES As T"
        strSQL &= " where t.TABLE_NAME = '" & argTblName & "'"

        rs = clsSVT.getDataReaderFromSQL(strSQL)

        If Not rs.HasRows Then
            rs.Close()
            Exit Sub
        End If

        With rs
            .Read()
            strSQL = "Insert Into " & gStrLogDBName & ".." & argTblName & " ("
            strSQL &= GetVal(!Columns, "")
            If Not blnDtl Then
                strSQL &= ",LogUserName,UserAction,ActionDateTime,LogSysName,LogSysUserName,LogIPAddress)"
            Else
                strSQL &= ",RecID)"
            End If
            strSQL &= " Select "
            strSQL &= GetVal(!Columns, "")
            If Not blnDtl Then
                strSQL &= " ,<<LogUserName>>,<<UserAction>>,<<ActionDateTime>>,<<LogSysName>>"
                strSQL &= " ,<<LogSysUserName>>,<<LogIPAddress>>"
            Else
                strSQL &= " ,<<RECID>>"
            End If
            strSQL &= " From " & argTblName
            strSQL &= " Where " & argShName & "ID = <<DOCID>>"
            rs.Close()
        End With

        If blnDtl Then
            clsSVT.execSQLStatement("Update utlTransactions Set AuditSQLDtl = '" & strSQL & "' Where DocID = " & argDocID & "")
        Else
            clsSVT.execSQLStatement("Update utlTransactions Set AuditSQLHdr = '" & strSQL & "' Where DocID = " & argDocID & "")
        End If
    End Sub

    Private Sub updateAuditLogTrack(ByVal argRow As Int32)
        With vsfDet
            If clsSVT.execSQLStatement("Update utlTransactions Set AuditLogTrack = 'Y' Where DocID = " & .GetData(argRow, vDetDocID) & "") Then
                .SetData(argRow, vDetLogTrack, "Y")
                .Col = vDetLogTrack
                Call setVsfCellColour(vsfDet, argRow, vDetDocName, Color.ForestGreen, Color.Black, True)
            End If
        End With
    End Sub
End Class