Imports System.IO
Imports System.Data.oledb
Public Class frmUtlSplitData
    Dim clsSVT As New svtLibrary
    Dim strNWFile As String
    Private Sub frmUtlSplitData_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        txtCDB.Text = gStrDBName
        txtLastInvDate.Text = clsSVT.getDataStringValue("Select top 1 convert(varchar,sinvDate,106) from TrnHdrSInv order by SinvDate desc", "", False)

        txtLastPBDate.Text = clsSVT.getDataStringValue("Select top 1 convert(varchar,PBDate,106) from TrnHdrPB order by PBDate desc", "", False)
        If txtLastInvDate.Text <> "" And txtLastPBDate.Text <> "" Then
            txtDBPfx.Text = Replace(Strings.Right(txtLastPBDate.Text, 8), " ", "")
            If CDate(txtLastInvDate.Text) > CDate(txtLastPBDate.Text) Then
                txtDBPfx.Text = Replace(Strings.Right(txtLastInvDate.Text, 8), " ", "")
            End If
        End If
        txtADB.Text = gStrDBName & "_" & txtDBPfx.Text
        Dim strT As String
        strT = "Current Database " & txtCDB.Text
        strT &= vbCrLf
        strT &= "Archived to Database " & txtADB.Text
        strT &= vbCrLf
        strT &= "Database " & txtCDB.Text & " will be cleared of all transactions"
        strT &= vbCrLf
        strT &= "Current Closing balance will be taken as OP Balance in " & txtCDB.Text
        strT &= vbCrLf
        strT &= "Database " & txtADB.Text & " can be used for viewing older data"
        lblTips.Text = strT
    End Sub

    Private Function CreateAndRestoreDB(argDBName As String, argBakFile As String) As Boolean
        CreateAndRestoreDB = False
        Dim strSQL As String
        Dim strLogFileName As String, strDBFileName As String
        Dim strLogPathName As String, strDataPathName As String
        Try
            strLogFileName = clsSVT.getDataStringValue("select name as logFile from sys.master_files " _
                     & " where type_desc = 'LOG' and database_id in (select database_id  from sys.databases where upper(name) = 'master')", "", False)
            strLogPathName = clsSVT.getDataStringValue("select Physical_name as logFile from sys.master_files " _
                     & " where type_desc = 'LOG' and database_id in (select database_id  from sys.databases where upper(name) = 'master')", "", False)
            strLogPathName = strLogPathName.Replace(Path.GetFileName(strLogPathName).Replace("_log.ldf", ""), argDBName)

            strDBFileName = clsSVT.getDataStringValue("select name as logFile from sys.master_files " _
                     & " where type_desc = 'ROWS' and database_id in (select database_id  from sys.databases where upper(name) = 'master')", "", False)
            strDataPathName = clsSVT.getDataStringValue("select Physical_name as logFile from sys.master_files " _
                     & " where type_desc = 'ROWS' and database_id in (select database_id  from sys.databases where upper(name) = 'master')", "", False)
            strDataPathName = strDataPathName.Replace(Path.GetFileName(strDataPathName).Replace(".mdf", ""), argDBName)

            StMsg("Creating New database backup....")
            Call clsSVT.execSQLStatement("Create database " & argDBName)

            StMsg("Restoring backup....")
            strSQL = "RESTORE DATABASE " & argDBName
            strSQL = strSQL & " From DISK = '" & argBakFile & "' With File=1,"
            strSQL = strSQL & " Move '" & argDBName & "' TO '" & strDataPathName & "' ,"
            strSQL = strSQL & " Move '" & argDBName & "_LOG'  TO '" & strLogPathName & ".ldf', REPLACE"
            Call clsSVT.execSQLStatement(strSQL)
            CreateAndRestoreDB = True
        Catch ex As Exception
            MsgBox("Error during database creation" & vbCrLf & ex.Message, MsgBoxStyle.Critical, "Error [createDatabase]")
        End Try

    End Function

    Private Function backupDB() As Boolean
        Dim strBackupPath As String, strFile As String, strSQL As String, strNWBackupPath As String, strFilePfx As String
        backupDB = False
        strFilePfx = "db" & gStrDBName & getSystemDateTime.ToString("ddMMMyyyy_hhmmss") & ".Bak"

        strBackupPath = getGsettings("Database", "SQLBACKUPPATH")
        If Trim(strBackupPath) = "" Then
            MsgBox("Backup path of SQL is not specified in Global Settings. (Under Database Group)", MsgBoxStyle.Critical, "Path not specified")
            Exit Function
        End If
        If Strings.Right(strBackupPath, 1) <> "\" Then strBackupPath = strBackupPath & "\"
        strFile = strBackupPath & strFilePfx

        strNWBackupPath = getGsettings("Database", "SQLNWBACKUPPATH")
        If Trim(strNWBackupPath) = "" Then
            MsgBox("Network Backup path of SQL is not specified in Global Settings. (Under Database Group)", MsgBoxStyle.Critical, "Path not specified")
            Exit Function
        End If
        If Strings.Right(strNWBackupPath, 1) <> "\" Then strNWBackupPath = strNWBackupPath & "\"
        strNWFile = strNWBackupPath & strFilePfx
        Try
            showWaitCursor()
            strSQL = "BACKUP DATABASE " & gStrDBName & " TO DISK = '" & strFile & "'  WITH FORMAT;"
            If clsSVT.execSQLStatement(strSQL) Then
                'MsgBox(".....")
            End If
            backupDB = True
            showNormalCursor()
        Catch ex As Exception
            showNormalCursor()
            MsgBox("Error during taking backup" & vbCrLf & Err.Description, MsgBoxStyle.Critical, "Backup error")
        End Try
    End Function

    Private Sub btnUpdate_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click
        showWaitCursor()
        StMsg("Creating current database backup....")
        If Not backupDB() Then Exit Sub
        showWaitCursor()
        Application.DoEvents()

        If Not CreateAndRestoreDB(txtADB.Text, strNWFile) Then
            Exit Sub
        End If
        Application.DoEvents()
        showWaitCursor()
        'clear all transactions and update PB with Op Balance
        StMsg("Clearing Transactions and Updating Opening balance")
        Application.DoEvents()
        Call clsSVT.execSQLStatement("Use " & gStrDBName)
        Call updateOPBalance()

        showWaitCursor()
        Call clsSVT.execSQLStatement("Update " & txtADB.Text & "..utlMisc set AllowSplit = 'N'")
        Application.DoEvents()
        showNormalCursor()
        MsgBox("Successfully Split data", MsgBoxStyle.Information, Me.Text)
        btnUpdate.Visible = False
    End Sub

    Private Sub updateOPBalance()
        svtConn.Close()
        If Not openMSSqlConn(gStrServerName, gStrDBName, gStrDBLoginName, gStrDBLoginPwd) Then
            frmUtlDBConnection.ShowDialog()
            Exit Sub
        End If

        Try
            Dim cmd As oledbCommand = New oledbCommand("ClearAndUpdateOP")
            With cmd
                cmd.Connection = svtConn
                .CommandType = CommandType.StoredProcedure
                .CommandTimeout = 0
                .ExecuteNonQuery()
            End With
        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
        End Try
        Exit Sub
        'Dim strSQL As String
        'Call clsSVT.execSQLStatement("truncate table trnHdrPB")
        'Call clsSVT.execSQLStatement("truncate table trndtlPB")
        'Call clsSVT.execSQLStatement("truncate table TrnHdrSInv")
        'Call clsSVT.execSQLStatement("truncate table TrndtlSinv")
        'Call clsSVT.execSQLStatement("truncate table TrnHdrSR")
        'Call clsSVT.execSQLStatement("truncate table TrnDtlSR")
        'Call clsSVT.execSQLStatement("truncate table TrnHdrStkDed")
        'Call clsSVT.execSQLStatement("truncate table TrnDtlStkDed")
        'Call clsSVT.execSQLStatement("truncate table utlStockList")

        'strSQL = "insert into trnhdrpb "
        'strSQL &= "(PBNO,Prefix,PrintPBNO,PBDate,SUPPID, "
        'strSQL &= "PartyName, PartyAdd1, PartyAdd2, PartyAdd3, PartyAdd4, PartyCity, PartyState, PartyPinCode,"
        'strSQL &= "PartyCountry, PartyCName, PartyCMob, PartyCEmail, PartyCDesig, Authorised, AuthBy, AuthOn,"
        'strSQL &= "LOCID, ChangedBy, ChangedOn, Cancelled, AddedBy, AddedOn, AMTINWRDS, RMKS, SUPPINVNO,"
        'strSQL &= "SUPPINVDATE, SUPPINVVALUE, TOTAMT, OTHAMT, GRNDTOT, TOTCGST, TOTSGST, TOTIGST, PURCHASETYPE) values ("
        'strSQL &= "'OP00001',"
        'strSQL &= "'OP',"
        'strSQL &= "'OP00001',"
        'strSQL &= "Convert(Date, getdate()),"
        'strSQL &= "0,'-','-','-','-','-','-','-','-','-','-','-','-','-','Y','ADMIN',"
        'strSQL &= "getdate(),1,'',Null,'N','ADMIN',"
        'strSQL &= "getdate(),'','','',Null,0,0,0,0,0,0,0,'SA')"
        'Call clsSVT.execSQLStatement(strSQL)

        'strSQL = "Insert into TrnDtlPB (PBID, SlNo, ITEMID, UOMID, ItemSlNO, StkQty, PBQty, PBRate, PBAmt, NOOFCASES, NOOFBOTTLES,"
        'strSQL &= "CGSTPER, CGSTAMT, SGSTPER, SGSTAMT, IGSTPER, IGSTAMT, NETAMT, MRP, PBQtyInLtrs) "
        'strSQL &= "Select 1, 1, q.itemid, i.uomid, 1, clbal, clbal, PURRATE, clbal * PURRATE, "
        'strSQL &= "Case when i.BotPerCases > 0 then round(clbal / i.BotPerCases,2) else clbal end,clbal,"
        'strSQL &= "0,0,0,0,0,0,clbal * PURRATE, i.mrp, clbal * p.LTRPERBOT"
        'strSQL &= "From TrnQOH q, mstitem i, MstPKG p"
        'strSQL &= "Where q.itemid = i.itemid"
        'strSQL &= "And i.PKGID = p.PKGID"
        'Call clsSVT.execSQLStatement(strSQL)

        'Call clsSVT.execSQLStatement("truncate table trnqoh")

        'strSQL = "Insert Into UtlStockList"
        'strSQL &= "(ItemID, StkMultiplier, DocType, DocDate, DocID, DocNo, Particulars, Qty, LtrQty, Rate, locid, delflag, rewrite) "
        'strSQL = "Select ItemID, 1, 'SA' as DocType, PBDate, h.PBID, h.pbno, '', pbQty, PBQtyInLtrs, pbRate, 1, 'N', 'N'"
        'strSQL = "From trndtlpb d, trnhdrpb h"
        'strSQL = "Where h.pbid = d.pbid"
        'strSQL = "Order By pbdate, itemid"
        'Call clsSVT.execSQLStatement(strSQL)

    End Sub
End Class