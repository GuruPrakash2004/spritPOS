Public Class frmDBCreation

    Dim clsSvt As New svtLibrary

    Const vDetDocID = 0
    Const vDetDocName = 1
    Const vDetAfctStk = 2
    Const vDetQtyFld = 3
    Const vDetValReq = 4
    Const vDetRateFld = 5
    Const vDetLocFld = 6
    Const vDetLoc2Fld = 7
    Const vDetTotCols = 8

    Const dtDocID = 0
    Const dtDocName = 1
    Const dtDocShName = 2
    Const dtStkAffect = 3
    Const dtFldQty = 4
    Const dtFldValReq = 5
    Const dtFldRate = 6
    Const dtFldFLoc = 7
    Const dtFldTLoc = 8

    Dim dtStkDoc As DataTable

    Dim strSHForm As String
    Dim strDocName As String
    Dim strDocHdrTblName As String
    Dim strDocDtlTblName As String
    Dim strDocIDFldName As String
    Dim strDocNoFldName As String
    Dim strDocDateFldName As String
    Dim strQtyFldName As String
    Dim strRateFldName As String
    Dim blnValReq As Boolean
    Dim strLocFldName As String
    Dim strLoc2FldName As String
    Dim intStkMultiplier As Int32
    Dim intDocID As Int32
    Dim blnStkTransfer As Boolean

    Dim blnPartyPresent As Boolean

    Dim svtTrn As OleDb.OleDbTransaction


    Dim dtLFYFrom As Date, dtLFYTo As Date
    Dim dtCFYFrom As Date, dtCFYTo As Date
    Dim intCFYear As Int32

    Dim intCFY As Int32
    Dim intLFY As Int32

    Dim strValType As String
    Dim strRateFld As String

    Dim strMsg As String = ""
    Dim blnTimerStop As Boolean = False

    Private Sub frmDBCreation_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        dtpTyr.Value = getSystemDate()
        txtDBPath.Text = getGsettings("DATABASE", "SQLBACKUPPATH", "")
        txtNetPath.Text = getGsettings("DATABASE", "SQLNWBACKUPPATH", "")

    End Sub

    Private Sub btnChangeFinYr_Click(sender As Object, e As EventArgs) Handles btnChangeFinYr.Click

        Dim strSQL As String = "Select * from MstCompFinYear Order by yrFdate"

        Dim objfrm As New frmUtlSingleSel
        With objfrm
            .Text = "Select Finyear"
            .FillViewVsf(Me, strSQL, txtDummy, 0, -1, txtDummy2, 1)
            .ShowDialog()
        End With

        If Trim(CStr(txtDummy.Text)) <> "" Then
            'dtpFyr.Value = CDate(txtDummy.Text)
            dtpTyr.Value = CDate(txtDummy2.Text)
        End If
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        If blnTimerStop Then
            Timer1.Stop()
            Exit Sub
        End If
        lblMsg.Text = strMsg
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles btnUpdatePath.Click
        clsSvt.execSQLStatement("Update utlgsettings set setvalue='" & Trim(txtDBPath.Text) & "'" & " where setgroup='DATABASE' AND SetName='SQLBACKUPPATH'")
        clsSvt.execSQLStatement("Update utlgsettings set setvalue='" & Trim(txtNetPath.Text) & "'" & " where setgroup='DATABASE' AND SetName='SQLNWBACKUPPATH' ")
        MsgBox("Updated Path  Successfully...", MsgBoxStyle.Information, Me.Text)
    End Sub

    Private Function backupDB(ByVal argName As String) As Boolean
        backupDB = False
        Dim strBackupPath As String, strFile As String, strSQL As String, strFilePfx As String
        'backupDB = ""
        'if condition added by Surabhi for ACCESS DB
        If gDBTypeCurrent = gDbTypeAccess Then
            strFilePfx = "\db" & gStrDBName & getSystemDateTime.ToString("ddMMMyyyy_hhmmss") & ".mdb"
        Else
            strFilePfx = argName & ".Bak"
        End If

        strBackupPath = txtDBPath.Text
        If Trim(strBackupPath) = "" Then
            MsgBox("Backup path of SQL is not specified in Global Settings. (Under Database Group)", MsgBoxStyle.Critical, "Path not specified")
            Exit Function
        End If
        strFile = strBackupPath & strFilePfx

        'Try
        '    If Not IO.Directory.Exists(strBackupPath) Then
        '        MsgBox("Invalid Backup Path... Backup path of SQL is not specified in Global Settings. (Under Database Group)", MsgBoxStyle.Critical, "Path not specified")
        '        Exit Function
        '    End If
        'Catch ex As Exception
        '    MsgBox(ex.Message, MsgBoxStyle.Information, Me.Text)
        '    Exit Function
        'End Try

        Dim strNWBackupPath As String = "", strNWFile As String = ""

        strNWBackupPath = txtNetPath.Text
        If Trim(strNWBackupPath) = "" Then
            MsgBox("Network Backup path of SQL is not specified in Global Settings. (Under Database Group)", MsgBoxStyle.Critical, "Path not specified")
            Exit Function
        End If

        Try
            If Not IO.Directory.Exists(strNWBackupPath) Then
                MsgBox("Invalid Network Backup Path.... Network Backup path of SQL is not specified in Global Settings. (Under Database Group)", MsgBoxStyle.Critical, "Path not specified")
                Exit Function
            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information, Me.Text)
            Exit Function
        End Try

        strNWFile = strNWBackupPath & strFilePfx

        If gDBTypeCurrent = gDbTypeMSSQL Then
            Try
                If System.IO.File.Exists(strNWFile) Then
                    System.IO.File.Delete(strNWFile)
                End If

                strSQL = " BACKUP DATABASE [" & gStrDBName & "] TO   "
                strSQL &= " DISK = N'" & strFile & "'  "
                strSQL &= " WITH NOFORMAT, NOINIT,   "
                strSQL &= " NAME = N'" & gStrDBName & "-Full Database Backup',  "
                strSQL &= " SKIP, NOREWIND, NOUNLOAD,  STATS = 10 "
                clsSvt.execSQLStatement(strSQL)
            Catch ex As Exception
                MsgBox("Error during taking backup" & vbCrLf & Err.Description, MsgBoxStyle.Critical, "Backup error")
                Exit Function
            End Try

            strSQL = " SELECT Physical_Name AS Location "
            strSQL &= " FROM sys.master_files mf INNER JOIN  "
            strSQL &= " sys.databases db ON db.database_id = mf.database_id "
            strSQL &= " where db.name = '" & gStrDBName & "'  "
            strSQL &= " and Type_Desc = 'ROWS' "

            Dim strMDFLoc As String = clsSvt.getDataStringValue(strSQL, "", False)

            strSQL = " SELECT Physical_Name AS Location "
            strSQL &= " FROM sys.master_files mf INNER JOIN  "
            strSQL &= " sys.databases db ON db.database_id = mf.database_id "
            strSQL &= " where db.name = '" & gStrDBName & "'  "
            strSQL &= " and Type_Desc = 'LOG' "

            Dim strLDFLoc As String = clsSvt.getDataStringValue(strSQL, "", False)


            strSQL = " SELECT f.name "
            strSQL &= " FROM sys.master_files f "
            strSQL &= " INNER JOIN sys.databases d ON d.database_id = f.database_id "
            strSQL &= " where d.name = '" & gStrDBName & "'  "
            strSQL &= " and Type_Desc = 'ROWS' "

            Dim strMdfName As String = clsSvt.getDataStringValue(strSQL, "", False)


            strSQL = " SELECT f.name "
            strSQL &= " FROM sys.master_files f "
            strSQL &= " INNER JOIN sys.databases d ON d.database_id = f.database_id "
            strSQL &= " where d.name = '" & gStrDBName & "'  "
            strSQL &= " and Type_Desc = 'LOG' "

            Dim strLDFName As String = clsSvt.getDataStringValue(strSQL, "", False)
            Try


                strSQL = " USE [master] "
                strSQL &= " RESTORE DATABASE [" & argName & "] "
                strSQL &= " FROM  DISK = N'" & strFile & "'  "
                strSQL &= " WITH  FILE = 1,  "
                strSQL &= " MOVE N'" & strMdfName & "' TO N'" & strMDFLoc.Replace(gStrDBName, argName) & "',   "
                strSQL &= " MOVE N'" & strLDFName & "' TO N'" & strLDFLoc.Replace(gStrDBName, argName) & "',  "
                strSQL &= " NOUNLOAD,  REPLACE,  STATS = 5 "

                clsSvt.execSQLStatement(strSQL)

                clsSvt.execSQLStatement("Use " & gStrDBName)
            Catch ex As Exception
                MsgBox("Error during restore backup" & vbCrLf & Err.Description, MsgBoxStyle.Critical, "Backup error")
                Exit Function
            End Try

            Return True
        End If
    End Function

    Private Sub btnCreate_Click(sender As Object, e As EventArgs) Handles btnCreateDB.Click
        Dim strSQl As String = ""

        Call showWaitCursor()

        strMsg = ""
        lblMsg.Visible = True

        Timer1.Interval = 1
        Timer1.Start()

        blnTimerStop = False

        Dim strNewDBName As String = gStrDBName & "_Archive_" & Format(dtpTyr.Value, "yyyyMMdd")

        'strNewDBName = gStrDBName & "_Archive_20240401"

        strMsg = "Creating Database " & strNewDBName

        If clsSvt.getDataLongValue("select count(*) from master.sys.databases where name = '" & strNewDBName & "'", 0, False) > 0 Then
            Call showNormalCursor()
            MsgBox("Backup Database already available...", MsgBoxStyle.Exclamation, Me.Text)
            blnTimerStop = True
            Exit Sub
        End If

        If Not backupDB(strNewDBName) Then
            Exit Sub
        End If

        Dim dtTrans As DataTable = clsSvt.getDATATable("Select * From UtlTransactions Where DocType = 'Transaction' Order by DocName")

        strMsg = "Transfer Data "

        If dtTrans.Rows.Count <= 0 Then
            Call showNormalCursor()
            MsgBox("There is no transaction is database...", MsgBoxStyle.Exclamation, Me.Text)
            blnTimerStop = True
            Exit Sub
        End If

        For k As Int32 = 0 To dtTrans.Rows.Count - 1
            strDocHdrTblName = dtTrans.Rows(k).Item("HdrTblName")
            strDocDtlTblName = dtTrans.Rows(k).Item("DtlTblName")
            strSHForm = dtTrans.Rows(k).Item("DocShForm")

            strMsg = "Transfer Data " & dtTrans.Rows(k).Item("DocName")

            'strSQl = "Select * Into " & strNewDBName & ".." & strDocHdrTblName & " From " & strDocHdrTblName
            'strSQl &= " Where " & strSHForm & "Date < '" & getDateFormatInInsert(dtpTyr.Value) & "'"
            'clsSvt.execSQLStatement(strSQl)

            'strSQl = "Select d.* Into " & strNewDBName & ".." & strDocDtlTblName & " From " & strDocHdrTblName & " h, " & strDocDtlTblName & " d "
            'strSQl &= " Where h." & strSHForm & "ID = d." & strSHForm & "ID and h." & strSHForm & "Date < '" & getDateFormatInInsert(dtpTyr.Value) & "'"
            'clsSvt.execSQLStatement(strSQl)

            If strSHForm.ToUpper() = "PB" Then
                strSQl = " Insert Into TrnDtlPB_Archive (PBID, PBDate, ItemID, PBRate, PBQty, NetAmt) "
                strSQl &= " Select d.PBID, h.PBDate, d.ItemID, d.PBRate, d.PBQty, d.NetAmt "
                strSQl &= " From TrnHdrPB h, TrnDtlPB d "
                strSQl &= " Where h.PBID=d.PBID and h.PBDate < '" & getDateFormatInInsert(dtpTyr.Value) & "'"
                clsSvt.execSQLStatement(strSQl)
            End If

            If strDocDtlTblName <> "" Then
                strSQl = "Delete from " & strDocDtlTblName & " Where " & strSHForm & "ID in (Select " & strSHForm & "ID From " & strDocHdrTblName
                strSQl &= " Where " & strSHForm & "Date < '" & getDateFormatInInsert(dtpTyr.Value) & "')"
                clsSvt.execSQLStatement(strSQl)
            End If

            If strDocHdrTblName <> "" Then
                strSQl = "Delete from " & strDocHdrTblName
                strSQl &= " Where " & strSHForm & "Date < '" & getDateFormatInInsert(dtpTyr.Value) & "'"
                clsSvt.execSQLStatement(strSQl)
            End If
        Next

        strMsg = "Transfer Data Opening Balance"

        'If clsSvt.getDataLongValue("select count(*) from  sys.tables where name = 'mstopbalance'", 0, False) > 0 Then
        '    strSQl = "Select * Into " & strNewDBName & "..mstopbalance From mstopbalance "
        '    strSQl &= " Where FinYear = " & Val(CStr(Format(dtpFyr.Value, "yyyy")).Substring(2, 2))
        '    clsSvt.execSQLStatement(strSQl)
        'End If

        If clsSvt.getDataLongValue("select count(*) from  sys.tables where name = 'UtlStockList'", 0, False) > 0 Then

            strMsg = "Transfer Stock Balance"

            'strSQl = "Select * Into " & strNewDBName & "..UtlStockList From UtlStockList "
            'strSQl &= " Where DocDate < '" & getDateFormatInInsert(dtpTyr.Value) & "'"
            'clsSvt.execSQLStatement(strSQl)

            If clsSvt.getDataLongValue("select count(*) from sys.Triggers Where name = 'tr_InsertQOH'", 0, False) > 0 Then
                clsSvt.execSQLStatement("DISABLE TRIGGER tr_InsertQOH ON UtlStockList")
                clsSvt.execSQLStatement("DISABLE TRIGGER tr_DeleteQOH ON UtlStockList")
            End If

            strSQl = " Insert Into UtlStockList (ItemID, StkMultiplier, DocType, DocDate, DocID, DocNo, Particulars,  "
            strSQl &= " Qty, LtrQty, Rate, DelFlag, ReWrite, LocID, DocValue, FifoRate, FifoUsedQty) "
            strSQl &= " Select v.ItemID, case When Qty > 0 then 1 else -1 end, 'OP', '" & getDateFormatInInsert(dtpTyr.Value) & "', Slno, Slno, "
            strSQl &= " 'Stock Carry Forward to New DB " & strNewDBName & "', case when Qty > 0 then Qty Else -1 * Qty End, "
            strSQl &= " Case when Qty > 0 then Qty * LtrPerBot else -1 * Qty * LtrPerBot End, Rate, 'N', 'N', 0, 0, 0, 0"
            strSQl &= " From ("
            strSQl &= " Select Row_Number() Over(Order by ItemID) as Slno, ItemID, Sum(Qty * StkMultiplier) as Qty, Avg(Rate) as Rate "
            strSQl &= " From UtlStockList "
            strSQl &= " Where Docdate < '" & getDateFormatInInsert(dtpTyr.Value) & "' "
            strSQl &= " Group by ItemID ) v, MstItem i, MstPkg p "
            strSQl &= " Where v.ItemID = i.ItemID  "
            strSQl &= " and i.PKGID = p.PKGID "
            clsSvt.execSQLStatement(strSQl)



            strSQl = "Delete From UtlStockList "
            strSQl &= " Where DocDate < '" & getDateFormatInInsert(dtpTyr.Value) & "'"
            clsSvt.execSQLStatement(strSQl)

            If clsSvt.getDataLongValue("select count(*) from sys.Triggers Where name = 'tr_InsertQOH'", 0, False) > 0 Then
                clsSvt.execSQLStatement("ENABLE Trigger tr_InsertQOH ON UtlStockList")
                clsSvt.execSQLStatement("ENABLE Trigger tr_DeleteQOH ON UtlStockList")
            End If
        End If


        lblMsg.Visible = False
        blnTimerStop = True
        Call showNormalCursor()

        MsgBox("Executed Successfully...", MsgBoxStyle.Information, Me.Text)
        Me.Close()
    End Sub
End Class