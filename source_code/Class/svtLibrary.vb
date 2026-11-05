Imports System.Data.oledb
Imports C1.Win.C1FlexGrid


Public Class svtLibrary
    Public Function getDATATable(ByVal strSQL As [String], Optional ByVal branchConn As OleDbConnection = Nothing, Optional ByVal sTrans As OleDbTransaction = Nothing) As DataTable
        Dim dt As New DataTable()
        'Dim cmd As New oledbCommand
        Dim sConn As OleDbConnection
        Try
            If branchConn Is Nothing Then
                sConn = svtConn
            Else
                sConn = branchConn
            End If
            'cmd.CommandText = strSQL
            'cmd.CommandType = CommandType.Text
            'cmd.Connection = sConn
            'If Not sTrans Is Nothing Then
            '    cmd.Transaction = sTrans
            'End If
            'cmd.ExecuteNonQuery()

            'Dim da As New oledb.oledbDataAdapter(cmd)

            Dim da As New OleDb.OleDbDataAdapter(strSQL, sConn)
            da.SelectCommand.Transaction = sTrans
            da.Fill(dt)

        Catch ex As Exception

            MsgBox(ex.Message & vbCrLf & "Error in SQL: " & strSQL)
            Call CopyPendQry(strSQL)
        End Try

        Return dt
    End Function

    Public Sub setGridDataBind(ByVal strSQL As [String], ByRef argVSF As C1FlexGrid, Optional ByVal branchConn As OleDbConnection = Nothing, Optional ByVal sTrans As OleDbTransaction = Nothing)
        Dim dt As New DataTable
        Dim sConn As OleDbConnection
        Try
            If branchConn Is Nothing Then
                sConn = svtConn
            Else
                sConn = branchConn
            End If

            Dim da As New OleDb.OleDbDataAdapter(strSQL, sConn)
            da.Fill(dt)
            argVSF.DataSource = dt.DefaultView

        Catch ex As Exception
            MsgBox(ex.Message & vbCrLf & "Error in SQL: " & strSQL)
        End Try
    End Sub

    Public Sub setGridDataBind_DT(ByVal strSQL As [String], ByRef argVSF As C1FlexGrid, Optional ByVal branchConn As OleDbConnection = Nothing, Optional ByVal sTrans As OleDbTransaction = Nothing)
        Dim cmd As New OleDbCommand
        Dim sConn As OleDbConnection
        Dim dt As New DataTable
        Dim rs As OleDbDataReader = Nothing
        Try
            If branchConn Is Nothing Then
                sConn = svtConn
            Else
                sConn = branchConn
            End If
            cmd.CommandText = strSQL
            cmd.CommandType = CommandType.Text
            cmd.Connection = sConn
            If Not sTrans Is Nothing Then
                cmd.Transaction = sTrans
            End If
            rs = cmd.ExecuteReader()
            dt.Load(rs)
            argVSF.DataSource = dt

        Catch ex As Exception
            MsgBox(ex.Message & vbCrLf & "Error in SQL: " & strSQL)
        End Try
    End Sub

    Public Function getDataType(ByVal argTblName As String, ByVal argFldName As String) As String
        getDataType = "TEXT"
        Dim strSQL As String
        Dim rsDummy As OleDbDataReader

        strSQL = "Select " & argFldName & " from " & argTblName & " where 1 <> 1"
        rsDummy = getDataReaderFromSQL(strSQL)

        With rsDummy
            If InStr(.GetDataTypeName(0), "CHAR") > 0 Then
                getDataType = "TEXT"
            ElseIf InStr(.GetDataTypeName(0), "DATE") > 0 Then
                getDataType = "DATE"
            Else
                getDataType = "NUMERIC"
            End If
            .Close()
        End With
    End Function

    Public Function getDataStringValue(ByVal strSQL As String, ByVal argEmptyValue As String, ByRef blnFound As Boolean,
                                        Optional ByVal sTrans As OleDbTransaction = Nothing,
                                        Optional ByVal argBranchConn As OleDbConnection = Nothing) As String
        Dim rs As OleDbDataReader = Nothing
        Dim strValue As String = ""
        getDataStringValue = argEmptyValue
beginCode:
        ' Define the command 
        Using cmd As New OleDbCommand()

            If argBranchConn Is Nothing Then
                cmd.Connection = svtConn
            Else
                cmd.Connection = argBranchConn
            End If

            cmd.CommandType = CommandType.Text
            cmd.CommandText = strSQL
            If Not sTrans Is Nothing Then
                cmd.Transaction = sTrans
            End If

            strValue = argEmptyValue
            blnFound = False
            Try
                rs = cmd.ExecuteReader()
                While rs.Read
                    getDataStringValue = rs.GetValue(0).ToString
                    blnFound = True
                    Exit While
                End While
                rs.Close()
            Catch ex As Exception
                If InStr(ex.Message, "network error") > 0 Or InStr(ex.Message, "requires an open and available Connection") > 0 Then
                    Try
                        If argBranchConn Is Nothing Then
                            svtConn.Close()
                        Else
                            argBranchConn.Close()
                        End If

                        Call openMSSqlConn(gStrServerName, gStrDBName, gStrDBLoginName, gStrDBLoginPwd, False)
                        GoTo beginCode
                    Catch e As Exception
                        MsgBox(e.Message & vbCrLf & "Data connectivity lost, please check your network connection!")
                        End
                    End Try
                End If
                'MsgBox(ex.Message & vbCrLf & "Error in SQL: " & strSQL)
                WriteToErrorLog(ex.Message, ex.StackTrace, ex.Source)
            End Try
        End Using
    End Function


    Public Function getDataReaderFromSQL(ByVal strSQL As String, Optional ByVal sTrans As OleDbTransaction = Nothing,
                                         Optional ByVal argBranchConn As OleDbConnection = Nothing) As OleDbDataReader

        Dim rsRDR As OleDbDataReader = Nothing
beginCode:
        ' Define the command 
        Dim cmdRDR As New OleDbCommand()

        If argBranchConn Is Nothing Then
            cmdRDR.Connection = svtConn
        Else
            cmdRDR.Connection = argBranchConn
        End If

        cmdRDR.CommandType = CommandType.Text
        cmdRDR.CommandText = strSQL
        cmdRDR.CommandTimeout = 0
        If Not sTrans Is Nothing Then
            cmdRDR.Transaction = sTrans
        End If

        Try
            rsRDR = cmdRDR.ExecuteReader()

        Catch ex As Exception
            If InStr(ex.Message, "network error") > 0 Or InStr(ex.Message, "requires an open and available Connection") > 0 Then
                Try
                    If argBranchConn Is Nothing Then
                        svtConn.Close()
                    Else
                        argBranchConn.Close()
                    End If

                    Call openMSSqlConn(gStrServerName, gStrDBName, gStrDBLoginName, gStrDBLoginPwd, False)
                    GoTo beginCode
                Catch e As Exception
                    MsgBox(e.Message & vbCrLf & "Data connectivity lost, please check your network connection!")
                    End
                End Try
            End If

            MsgBox(ex.Message & vbCrLf & "Error in SQL: " & strSQL)
            WriteToErrorLog(ex.Message, ex.StackTrace, ex.Source)
        End Try

        Return rsRDR
    End Function


    Public Function execSQLStatement(ByVal strSQL As String, Optional ByVal sTrans As OleDbTransaction = Nothing,
                                     Optional ByVal argBranchConn As OleDbConnection = Nothing, Optional argBlnDoNotShowError As Boolean = True) As Boolean
        ' Define the command 
        execSQLStatement = False
        Dim cmd As New OleDbCommand()
beginCode:

        If argBranchConn Is Nothing Then
            cmd.Connection = svtConn
        Else
            cmd.Connection = argBranchConn
        End If

        cmd.CommandType = CommandType.Text
        cmd.CommandTimeout = 0
        cmd.CommandText = strSQL
        If Not sTrans Is Nothing Then
            cmd.Transaction = sTrans
        End If

        Try
            cmd.ExecuteNonQuery()
            execSQLStatement = True
        Catch ex As Exception
            If InStr(ex.Message, "network error") > 0 Or InStr(ex.Message, "requires an open and available Connection") > 0 Then
                Try
                    If argBranchConn Is Nothing Then
                        svtConn.Close()
                    Else
                        argBranchConn.Close()
                    End If

                    Call openMSSqlConn(gStrServerName, gStrDBName, gStrDBLoginName, gStrDBLoginPwd, False)
                    GoTo beginCode
                Catch e As Exception
                    MsgBox(e.Message & vbCrLf & "Data connectivity lost, please check your network connection!")
                    End
                End Try
            End If

            If argBlnDoNotShowError Then
                MsgBox(ex.Message & vbCrLf & "Error in SQL: " & strSQL, MsgBoxStyle.Critical)
            End If
            WriteToErrorLog(ex.Message, ex.StackTrace, ex.Source)
        End Try
    End Function

    Public Function execSQLStmtMsg(ByVal strSQL As String, Optional ByVal sTrans As OleDbTransaction = Nothing,
                                     Optional ByVal argBranchConn As OleDbConnection = Nothing) As String
        ' Define the command 
        execSQLStmtMsg = ""
        Dim cmd As New OleDbCommand()

beginCode:
        If argBranchConn Is Nothing Then
            cmd.Connection = svtConn

        Else
            cmd.Connection = argBranchConn
        End If

        cmd.CommandType = CommandType.Text
        cmd.CommandTimeout = 0
        cmd.CommandText = strSQL
        If Not sTrans Is Nothing Then
            cmd.Transaction = sTrans
        End If

        Try
            cmd.ExecuteNonQuery()
            execSQLStmtMsg = ""
        Catch ex As Exception
            If InStr(ex.Message, "network error") > 0 Or InStr(ex.Message, "requires an open and available Connection") > 0 Then
                Try
                    If argBranchConn Is Nothing Then
                        svtConn.Close()
                    Else
                        argBranchConn.Close()
                    End If

                    Call openMSSqlConn(gStrServerName, gStrDBName, gStrDBLoginName, gStrDBLoginPwd, False)
                    GoTo beginCode
                Catch e As Exception
                    MsgBox(e.Message & vbCrLf & "Data connectivity lost, please check your network connection!")
                    End
                End Try
            End If
            execSQLStmtMsg = ex.Message
        End Try
    End Function

    Public Function execSQLStmtReturnMsg(ByVal strSQL As String, Optional ByVal sTrans As OleDbTransaction = Nothing,
                                     Optional ByVal argBranchConn As OleDbConnection = Nothing) As String
        ' Define the command 
        execSQLStmtReturnMsg = ""
        Dim cmd As New OleDbCommand()

beginCode:
        If argBranchConn Is Nothing Then
            cmd.Connection = svtConn

        Else
            cmd.Connection = argBranchConn
        End If

        cmd.CommandType = CommandType.Text
        cmd.CommandTimeout = 0
        cmd.CommandText = strSQL
        If Not sTrans Is Nothing Then
            cmd.Transaction = sTrans
        End If

        Try
            execSQLStmtReturnMsg = CStr(cmd.ExecuteNonQuery())
            execSQLStmtReturnMsg = ""
        Catch ex As Exception
            If InStr(ex.Message, "network error") > 0 Or InStr(ex.Message, "requires an open and available Connection") > 0 Then
                Try
                    If argBranchConn Is Nothing Then
                        svtConn.Close()
                    Else
                        argBranchConn.Close()
                    End If

                    Call openMSSqlConn(gStrServerName, gStrDBName, gStrDBLoginName, gStrDBLoginPwd, False)
                    GoTo beginCode
                Catch e As Exception
                    MsgBox(e.Message & vbCrLf & "Data connectivity lost, please check your network connection!")
                    End
                End Try
            End If
            execSQLStmtReturnMsg = ex.Message
        End Try
    End Function

    Public Function getDataLongValue(ByVal strSQL As String, ByVal argEmptyValue As Long, ByRef blnFound As Boolean,
                                      Optional ByVal sTrans As OleDbTransaction = Nothing,
                                      Optional ByVal argBranchConn As OleDbConnection = Nothing) As Long
        Dim rs As OleDbDataReader
        getDataLongValue = argEmptyValue
        Dim lngValue As Long = argEmptyValue
beginCode:
        blnFound = False
        ' Define the command 
        Try
            Using cmd As New OleDbCommand()

                If argBranchConn Is Nothing Then
                    cmd.Connection = svtConn
                Else
                    cmd.Connection = argBranchConn
                End If

                cmd.CommandType = CommandType.Text
                cmd.CommandText = strSQL
                cmd.CommandTimeout = 0
                If Not sTrans Is Nothing Then
                    cmd.Transaction = sTrans
                End If

                rs = cmd.ExecuteReader()
            End Using
            While rs.Read
                blnFound = True
                If IsDBNull(rs.GetValue(0)) Then
                    getDataLongValue = argEmptyValue
                Else
                    getDataLongValue = rs.GetValue(0)
                End If
                Exit While
            End While
            rs.Close()

        Catch ex As Exception
            If InStr(ex.Message, "network error") > 0 Or InStr(ex.Message, "requires an open and available Connection") > 0 Then
                Try
                    If argBranchConn Is Nothing Then
                        svtConn.Close()
                    Else
                        argBranchConn.Close()
                    End If

                    Call openMSSqlConn(gStrServerName, gStrDBName, gStrDBLoginName, gStrDBLoginPwd, False)
                    GoTo beginCode
                Catch e As Exception
                    MsgBox(e.Message & vbCrLf & "Data connectivity lost, please check your network connection!")
                    End
                End Try
            End If
            MsgBox(ex.Message & vbCrLf & "Error in SQL: " & strSQL)
            WriteToErrorLog(ex.Message, ex.StackTrace, ex.Source)
        End Try
    End Function

    Public Function getDataDoubleValue(ByVal strSQL As String, ByVal argEmptyValue As Double, ByRef blnFound As Boolean, Optional ByVal sTrans As OleDbTransaction = Nothing, Optional showMsg As Boolean = True) As Double
        Dim rs As OleDbDataReader
        getDataDoubleValue = argEmptyValue

beginCode:
        blnFound = False
        ' Define the command 
        Try
            Using cmd As New OleDbCommand()
                cmd.Connection = svtConn
                cmd.CommandType = CommandType.Text
                cmd.CommandText = strSQL
                cmd.CommandTimeout = 0
                If Not sTrans Is Nothing Then
                    cmd.Transaction = sTrans
                End If

                rs = cmd.ExecuteReader()
            End Using
            While rs.Read
                blnFound = True
                getDataDoubleValue = GetVal(rs.GetValue(0), 0)
                Exit While
            End While
            rs.Close()

        Catch ex As Exception
            If InStr(ex.Message, "network error") > 0 Or InStr(ex.Message, "requires an open and available Connection") > 0 Then
                Try
                    svtConn.Close()
                    Call openMSSqlConn(gStrServerName, gStrDBName, gStrDBLoginName, gStrDBLoginPwd, False)
                    GoTo beginCode
                Catch e As Exception
                    MsgBox(e.Message & vbCrLf & "Data connectivity lost, please check your network connection!")
                    End
                End Try
            End If
            Select Case UCase(ex.Message)
                Case UCase("Divide by zero error encountered.")
                    getDataDoubleValue = 0
                Case Else
                    If showMsg Then
                        MsgBox(ex.Message & vbCrLf & "Error in SQL: " & strSQL)
                        WriteToErrorLog(ex.Message, ex.StackTrace, ex.Source)
                    End If
            End Select
        End Try
    End Function
    Public Function getDataDateValue(ByVal strSQL As String, ByVal argEmptyValue As Date, ByRef blnFound As Boolean,
                                     Optional ByVal sTrans As OleDbTransaction = Nothing,
                                     Optional ByVal argBranchConn As OleDbConnection = Nothing) As Date
        Dim rsD As OleDbDataReader
        getDataDateValue = argEmptyValue
        Dim dtValue As Date = argEmptyValue
beginCode:
        blnFound = False
        ' Define the command 
        Try
            Using cmd As New OleDbCommand()
                If argBranchConn Is Nothing Then
                    cmd.Connection = svtConn
                Else
                    cmd.Connection = argBranchConn
                End If

                cmd.CommandType = CommandType.Text
                cmd.CommandText = strSQL

                If Not sTrans Is Nothing Then
                    cmd.Transaction = sTrans
                End If

                rsD = cmd.ExecuteReader()
            End Using
            While rsD.Read
                blnFound = True
                If IsDBNull(rsD.GetValue(0)) Then
                    getDataDateValue = argEmptyValue
                Else
                    getDataDateValue = rsD.GetValue(0)
                End If
                Exit While
            End While
            rsD.Close()

        Catch ex As Exception
            If InStr(ex.Message, "network error") > 0 Or InStr(ex.Message, "requires an open and available Connection") > 0 Then
                Try
                    If argBranchConn Is Nothing Then
                        svtConn.Close()
                    Else
                        argBranchConn.Close()
                    End If

                    Call openMSSqlConn(gStrServerName, gStrDBName, gStrDBLoginName, gStrDBLoginPwd, False)
                    GoTo beginCode
                Catch e As Exception
                    MsgBox(e.Message & vbCrLf & "Data connectivity lost, please check your network connection!")
                    End
                End Try
            End If
            MsgBox(ex.Message & vbCrLf & "Error in SQL: " & strSQL)
            WriteToErrorLog(ex.Message, ex.StackTrace, ex.Source)
        End Try
    End Function

    Public Sub WriteToErrorLog(ByVal msg As String,
       ByVal stkTrace As String, ByVal source As String)

        'If Not System.IO.Directory.Exists(System.Windows.Forms.Application.StartupPath &
        '"\Errors\") Then
        '    System.IO.Directory.CreateDirectory(System.Windows.Forms.Application.StartupPath &
        '    "\Errors\")
        'End If


        ''check the file
        'Dim fs As FileStream = New FileStream(System.Windows.Forms.Application.StartupPath &
        '"\Errors\errorlog.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite)
        'Dim s As StreamWriter = New StreamWriter(fs)
        's.Close()
        'fs.Close()

        ''log it
        'Dim fs1 As FileStream = New FileStream(System.Windows.Forms.Application.StartupPath &
        '"\Errors\errorlog.txt", FileMode.Append, FileAccess.Write)
        'Dim s1 As StreamWriter = New StreamWriter(fs1)
        's1.Write("Date/Time: " & DateTime.Now.ToString() & vbCrLf)
        's1.Write("Title: " & source & vbCrLf)
        's1.Write("Message: " & msg & vbCrLf)
        's1.Write("StackTrace: " & stkTrace & vbCrLf)
        's1.Write("================================================" & vbCrLf & vbNewLine)
        's1.Close()
        'fs1.Close()

        ''Dim myolApp As New Outlook.Application
        ''Dim myitem As Outlook.MailItem
        ''myitem = myolApp.CreateItem(Outlook.OlItemType.olMailItem)
        'Dim strTOMailId As String
        'Dim SafeItem
        'SafeItem = CreateObject("Redemption.SafeMailItem") 'Create an instance of Redemption.SafeMailItem

        ''SafeItem.Item = myitem 'set Item property
        ''strTOMailId = "praveen@softvent.com;ravikaran@softvent.com;meghana@softvent.com;softvent@gmail.com"
        'strTOMailId = "jaikanth@softvent.com"

        'Dim HostName As String
        'Dim hostIP As String
        'HostName = System.Net.Dns.GetHostName()
        'hostIP = System.Net.Dns.GetHostEntry(HostName).AddressList(0).ToString()

        'With SafeItem
        '    .Subject = "IkyaHO DB Synchronization Issue"
        '    .Attachments.Add(System.Windows.Forms.Application.StartupPath & "\Errors\errorlog.txt")
        '    .To = strTOMailId
        '    .Body = "Error log from host: " & HostName & vbCrLf & "Error log from IP Address: " & hostIP
        '    .Send()
        'End With

        ''uncomment this (below)to send error log to my email

        ''Dim schema
        ''Dim cdoConf
        ''Dim cdoMsg
        ''schema = "http://schemas.microsoft.com/cdo/configuration/"
        ''cdoConf = CreateObject("CDO.Configuration")
        ''With cdoConf.Fields
        ''    .Item(schema & "sendusing") = 2 'The CDO Sendusing Port
        ''    .Item(schema & "smtpserver") = "server"
        ''    .update()
        ''End With

        ''Dim HostName As String
        ''Dim hostIP As String
        ''HostName = System.Net.Dns.GetHostName()
        ''hostIP = System.Net.Dns.GetHostEntry(HostName).AddressList(0).ToString()

        ''cdoMsg = CreateObject("CDO.Message")

        ''With cdoMsg
        ''    .Configuration = cdoConf
        ''    .From = HostName & "@lalalalal"
        ''    .To = "me@lalalalala"
        ''    .Subject = "Error Log"
        ''    .TextBody = "Error log from IP Address: " & hostIP
        ''    .addattachment(System.Windows.Forms.Application.StartupPath & "\Errors\errorlog.txt")
        ''    .Send()
        ''End With

        ''MessageBox.Show("An Error has occured.  The Program will now quit now.  Please Restart, and try again.  If the problem persists, email details of what you were doing that caused the error to 'support@softvent.com'")
        ''cdoMsg = Nothing
    End Sub

    Public Function ChkDuplicateData(ByVal argVsfGrid As C1.Win.C1FlexGrid.C1FlexGrid, ByRef argDupRow As Integer, ByVal ParamArray intChkCols() As Integer) As Boolean
        Dim intCols As Object
        Dim strUniqChk As String, strUniqKey As String
        ChkDuplicateData = False
        Dim i As Integer, j As Integer
        argDupRow = 0
        With argVsfGrid
            For i = 1 To .Rows.Count - 1
                strUniqKey = ""
                For Each intCols In intChkCols
                    strUniqKey = strUniqKey & "|" & Trim(.GetData(i, intCols))
                Next intCols
                For j = 1 To .Rows.Count - 1
                    strUniqChk = ""
                    For Each intCols In intChkCols
                        strUniqChk = strUniqChk & "|" & Trim(.GetData(j, intCols))
                    Next intCols
                    If i <> j Then
                        If strUniqKey = strUniqChk And strUniqChk <> "|" Then
                            argDupRow = i
                            ChkDuplicateData = True
                            Exit For
                        End If
                    End If
                Next j
            Next i
        End With
    End Function

    Public Function GetBranchConnObject(ByVal argBranch As String, Optional ByVal argForReport As String = "") As OleDbConnection
        Dim intBranchID As Integer
        Dim rsBranch As OleDbDataReader, strConn As String
        Dim strServer As String = "", strDatabase As String = "", strUserName As String = "", strPassword As String = ""
        GetBranchConnObject = Nothing
        intBranchID = getDataLongValue("Select BranchID from MstBranch where BranchName = '" & argBranch & "'", 0, False, , svtConn)
        Dim gbrConn As New OleDbConnection
        If intBranchID = 0 Then
            Exit Function
        End If
        rsBranch = getDataReaderFromSQL("Select * from MstBranch where BranchID = " & intBranchID, , svtConn)
        With rsBranch
            If .HasRows Then
                .Read()
                If argForReport <> "REPORT" Then
                    strServer = GetVal(!ServerName, "")
                    strDatabase = GetVal(!DBName, "")
                    strUserName = GetVal(!UserCode, "")
                    strPassword = GetVal(!LoginPwd, "")
                Else
                    strServer = gStrServerName
                    strDatabase = GetVal(!BrDBInHo, "")
                    strUserName = gStrDBLoginName
                    strPassword = gStrDBLoginPwd
                End If
            End If
            .Close()
        End With

        strConn = "Provider=SQLOLEDB.1;Persist Security Info=False"
        strConn = strConn & ";User ID=" & strUserName
        strConn = strConn & ";Pwd=" & strPassword
        strConn = strConn & ";Initial Catalog=" & strDatabase
        strConn = strConn & ";Data Source=" & strServer
        gbrConn.ConnectionString = strConn
        Try
            gbrConn.Open()
            GetBranchConnObject = gbrConn
        Catch ex As Exception
            MsgBox("Error in Connecting to Database " & strDatabase & ", on server " & strServer & vbCrLf & ex.Message, MsgBoxStyle.Critical, "Error in Connecting to DB")
            Exit Function
        End Try
    End Function
    Public Function AutoIncrementedNum(ByVal argFieldName As String, ByVal argTableName As String, Optional argCond As String = "", Optional ByVal sTrans As OleDbTransaction = Nothing, Optional ByVal brConn As OleDbConnection = Nothing) As Long
        Dim rsAUTO As OleDbDataReader
        Dim strSQL As String
        ' strSQL = "select  " & "max(isnull(" & argFieldName & ",0)) + 1 as MaxNo from " & argTableName & argCond
        strSQL = "Select max(" & getSyntax("ISNULL0", argFieldName) & ")  + 1 as MaxNo from " & argTableName & argCond
        If Not sTrans Is Nothing Then
            If brConn Is Nothing Then
                rsAUTO = getDataReaderFromSQL(strSQL, sTrans, svtConn)
            Else
                rsAUTO = getDataReaderFromSQL(strSQL, sTrans, brConn)
            End If
        Else
            If brConn Is Nothing Then
                rsAUTO = getDataReaderFromSQL(strSQL)
            Else
                rsAUTO = getDataReaderFromSQL(strSQL, , brConn)
            End If

        End If

        AutoIncrementedNum = 1
        With rsAUTO
            Do While .Read()
                AutoIncrementedNum = GetVal(rsAUTO("MaxNo"), 1)
            Loop
            .Close()
        End With
    End Function

    Public Function LoadGridData(ByVal StrQry As String, ByVal argVsfdet As C1FlexGrid, Optional ByVal sTrans As OleDbTransaction = Nothing, Optional ByVal brConn As OleDbConnection = Nothing) As Boolean
        LoadGridData = False
        Dim dt As New DataTable
        Dim dv As New DataView
        Call showWaitCursor()
        dt = getDATATable(StrQry, brConn)

        dv = New DataView(dt)
        With argVsfdet
            .DataSource = dv
            Call showNormalCursor()
        End With
        Return True
    End Function

    Public Function PadStr(argString As String, _
            Optional argLen As Integer = 0, _
            Optional argAlign As TextAlignEnum = TextAlignEnum.LeftCenter, _
            Optional argFormatString As String = "", _
            Optional argPadChar As String = " ") As String
        Dim strPadchars As String
        PadStr = ""
        If Len(argPadChar) = 0 Then
            argPadChar = " "
        Else
            argPadChar = Mid(argPadChar, 1, 1)
        End If
        If Trim(argFormatString) <> "" Then
            argString = Format(argString, argFormatString)
        End If
        If argLen = 0 Then argLen = Len(argString)
        Select Case argAlign
            Case TextAlignEnum.LeftCenter
                PadStr = Left(argString & StrDup(argLen, argPadChar), argLen)
            Case TextAlignEnum.RightCenter
                PadStr = Right(StrDup(argLen, argPadChar) & argString, argLen)
            Case TextAlignEnum.CenterCenter
                If argLen > Len(Trim(argString)) Then
                    argLen = ((argLen - Len(Trim(argString))) / 2)
                    strPadchars = StrDup(argLen, argPadChar)
                    PadStr = strPadchars & Trim(argString) & strPadchars
                Else
                    PadStr = Mid(argString, 1, argLen)
                End If
        End Select
    End Function
End Class

