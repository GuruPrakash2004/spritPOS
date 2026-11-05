Imports System.IO
Imports C1.Win.C1FlexGrid
Imports System.Data.OleDb
Imports System.Speech.Recognition
Imports Microsoft.VisualBasic
Imports Microsoft.Win32
Imports C1.Win.C1Command

Public Class frmMain
    Implements IMessageFilter
    Dim clsSvt As New svtLibrary
    Dim strMnuName As String
    Dim strCmd As String, strSQL As String
    Dim strSeparator As String = "______________________________"
    Dim SecondsCount As Integer 'Counts each second
    Public intNotifCount As Integer
    Dim strWordList() As String
    Dim blnSpeech As Boolean = False, blnImgForm As Boolean = False

    Private Sub frmMain_KeyUp(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyUp

        Select Case gStrSystemUserName
            Case "SSPL7", "MADHUSUDAN", "SSPL19", "JUPITER3", "USER", "KUMARDELL", "ADMIN", "EARTH5", "ADMIN", "SSPLMINISRVR", "JUPITER2", "SSPLUSER_18", "URANUS1", "MARS2", "MARS3", "EARTH1", "GANGADHAR", "SELVA", "ASHWINI", "PRADEEP", "LOKESH"
                If e.KeyCode = Keys.F12 And e.Control = True And e.Shift = True Then
                    Call updateToCloudDB()
                    Exit Sub
                End If
        End Select

        If e.Control = True AndAlso e.Alt = True AndAlso e.KeyCode = Keys.D Then
            frmUtlDeleteNonLiqItems.ShowDialog()
        End If

        If e.Control = True AndAlso e.Alt = True AndAlso e.KeyCode = Keys.X Then
            If pnlProject.Panels(0).Visible = True Then
                pnlProject.Panels(0).Visible = False
            Else
                pnlProject.Panels(0).Visible = True
            End If
        End If

        If e.Control = True AndAlso e.KeyCode = Keys.Q Then
            If pnlProject.Panels(0).Visible = False Then
                pnlProject.Panels(0).Visible = True
            End If
        End If


        If e.Control = True AndAlso e.KeyCode = Keys.I Then
            pnlProject.Panels(0).Visible = False
            TransactionsClick("Sales Invoice")
        End If



        If e.Control = True AndAlso e.KeyCode = Keys.F Then
            With New frmUtlQuickRef
                .StartPosition = FormStartPosition.CenterScreen
                .ShowDialog()
            End With
        End If

        If e.Alt = True AndAlso e.KeyCode = Keys.X Then
            Call frmMain_FormClosing(sender, Nothing)
        End If

        If e.Control = True AndAlso e.KeyCode = Keys.Oemtilde Then
            Dim objCal As New frmUtlCalci
            objCal.Show()
        End If
    End Sub

    Private Sub frmMain_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'MsgBox("1")
        Select Case Command()
            Case "DROPSEMAIL", "WEBDATASYNC"
                Me.WindowState = FormWindowState.Minimized
        End Select
        Call SetCultureInfo()
        'MsgBox("2")
        gIntInvCtr = 0
        Dim strProductID As String
        'initialise the grid by loading into iniGrid from .Ini File
        Call initINIGrid()
        If Not initDBSettingsMSSQL() Then
            frmUtlDBConnection.Show()
            Exit Sub
        End If
        'MsgBox("3")
        'If Strings.Right(Application.StartupPath, 5).ToUpper <> "DEBUG" Then
        '    With frmUpdateExe
        '        .Show()
        '        .Refresh()
        '        .chkExeVersion()
        '    End With
        'End If


        Call ChangeFontSize(Me)
        'gSettings.gCompFinYear
        'MsgBox("4")
        Dim strE As String = chkSetupVersion()
        ''MsgBox("5")
        Select Case strE
            Case "OK"
                Dim svtL As New SSPLLic.clsSSPLLic
                Dim strKey As String = svtL.getKey
                sbDemo.Text = "Licensed Copy:" & strKey
            Case "INVALIDREG"
                sbDemo.Text = "INVALID REG"
                frmUtlReg.ShowDialog()
            Case Else
                Dim svtL As New SSPLLic.clsSSPLLic
                Dim strKey As String = svtL.getKey
                Clipboard.SetText(strKey)
                sbDemo.Text = "INVALID LIC:" & strKey
                MsgBox("Key= " & strKey & vbCrLf & "This is copied in Clipboard also", MsgBoxStyle.Information, "Information")
                End
        End Select
        'MsgBox("6")
        txtSearch.Focus()
        Call getDefaultPrinter()
        gStrLogDBName = gStrDBName & "AuditLog"
        'TransactionsClick("Sales Invoice")
        'MsgBox("7")
        If Command() = "DROPSEMAIL" Then
            Dim objfrm As New frmRptDailySummary
            objfrm.dtpDay.Value = getSystemDate()
            objfrm.PDFSendMail()
            End
        End If
        'sgBox("8")
        If Command() = "WEBDATASYNC" Then
            Call syncDataToWeb(False)
            End
        End If

        'MsgBox("9")
        If Command().ToUpper = "AUTOEMAIL" Then
            Me.WindowState = FormWindowState.Minimized
            Call autoEmailTrigger()
            End
        End If

        'Call ExecReWriteStock()
        'MsgBox("10")
        If UCase(gUserCode) <> "ADMIN" Then
            If getGsettings("INVOICE", "SHOWSALESVALUE", "N") = "Y" Then
                Dim dblTotalSales As Double = clsSvt.getDataDoubleValue("Select Sum(InvValue) From TrnHdrSINV Where IsDelete = 'N' and SINVDate='" & getDateFormatInInsert(getSystemDate()) & "'", 0, False)
                Dim intTotalBills As Integer = clsSvt.getDataDoubleValue("Select Count(InvValue) From TrnHdrSINV Where IsDelete = 'N' and     SINVDate='" & getDateFormatInInsert(getSystemDate()) & "'", 0, False)
                lblSalesValue.Text = "Total Bills: " & intTotalBills & " & Sales: " & dblTotalSales
                lblSalesValue.Visible = True
            End If
        Else
            If gStrSystemName.ToUpper = "PLUTO" Or gStrSystemName.ToUpper = "EARTH5" Or gStrSystemName.ToUpper = "VENUS2" Then

            Else

                Dim dblTotalSales As Double = clsSvt.getDataDoubleValue("Select Sum(InvValue) From TrnHdrSINV Where IsDelete = 'N' and SINVDate='" & getDateFormatInInsert(getSystemDate()) & "'", 0, False)
                Dim intTotalBills As Integer = clsSvt.getDataDoubleValue("Select Count(InvValue) From TrnHdrSINV Where IsDelete = 'N' and     SINVDate='" & getDateFormatInInsert(getSystemDate()) & "'", 0, False)
                lblSalesValue.Text = "Total Bills: " & intTotalBills & " & Sales: " & dblTotalSales
                lblSalesValue.Visible = True
            End If
        End If

        'MsgBox("11")
        'If getGsettings("INVOICE", "SHOWSALESVALUE", "N") = "Y" Then
        '    Dim dblTotalSales As Double = clsSvt.getDataDoubleValue("Select Sum(InvValue) From TrnHdrSINV Where IsDelete = 'N' and SINVDate='" & getDateFormatInInsert(getSystemDate()) & "'", 0, False)
        '    Dim intTotalBills As Integer = clsSvt.getDataDoubleValue("Select Count(InvValue) From TrnHdrSINV Where IsDelete = 'N' and     SINVDate='" & getDateFormatInInsert(getSystemDate()) & "'", 0, False)
        '    lblSalesValue.Text = "Total Bills: " & intTotalBills & " & Sales: " & dblTotalSales
        '    lblSalesValue.Visible = True
        'End If

        gDblPurSellVarPer = Val(getGsettings("Company", "PurSellVarPer"))
        Call truncateLog()
        'MsgBox("12")
        Dim strShowstartUp As String
        strShowstartUp = clsSvt.getDataStringValue("SELECT ShowAtStartUp FROM UtlDashSettings", "N", False)
        If UCase(strShowstartUp) = "Y" Then
            Dim argform As New frmUtlDashboard
            Me.OpenForms(argform, "Dashboard")
        End If

        'Select Case gStrSystemUserName
        '    Case "MADHUWORK", "SSPLLAP9-PC"
        '    Case Else
        '        Dim argfrm As New frmKeyReg
        '        argfrm.StartPosition = FormStartPosition.CenterScreen
        '        argfrm.ShowDialog()
        'End Select

        'Call chkMinStock()
        'MsgBox("13")
        If clsSvt.getDataStringValue("Select EnableLoyalty from UtlMisc", "N", False) = "Y" Then
            blnLoyalty = True
            gdblBillValueGreaterThan = clsSvt.getDataDoubleValue("select convert(numeric(18,2),setvalue) from utlGSettings where SetName = 'BillValueGreaterThan'", 0, False)
            gIntAlertIfPointsGreaterThan = clsSvt.getDataLongValue("select convert(int,setvalue) from utlGSettings where SetName = 'AlertIfPointsGreaterThan'", 0, False)
            gDblPointsForOneRs = clsSvt.getDataDoubleValue("select convert(numeric(18,4),setvalue) from utlGSettings where SetName = 'PointsForOneRs'", 0, False)
            gDblRsForOnePoint = clsSvt.getDataDoubleValue("select convert(numeric(18,4),setvalue) from utlGSettings where SetName = 'RsForOnePoint'", 0, False)
        Else
            blnLoyalty = False
        End If
        Call RemovePreviousQrfiles()
        'MsgBox("14")
        If Not fnTablePresent("vwInvalidItems") Then
            Dim strsql As String = ""
            strsql = "create view [dbo].[vwInvalidItems] as "
            strsql = strsql & " Select 'UOM Error' as LinkProblem, itemid, itemname, uomid, itemgroupid, "
            strsql = strsql & " pkgid From mstitem Where uomid Not in (Select uomID from mstuom)"
            strsql = strsql & " union all select 'ItemGroup Error' as LinkProblem, itemid, itemname, uomid,"
            strsql = strsql & " itemgroupid, pkgid from mstitem where ItemGroupID "
            strsql = strsql & " Not in (Select itemGroupID from MstITEMGROUP) "
            strsql = strsql & " union all select 'Packaging Error' as LinkProblem, itemid, itemname, uomid, "
            strsql = strsql & " itemgroupid, pkgid from mstitem where PkgID not in (Select PkgID from MstPkg) "
            strsql = strsql & " union all select 'No Selling Rate' as LinkProblem, itemid, itemname, "
            strsql = strsql & " uomid, itemgroupid, pkgid from mstitem  where isnull(ItemSell,0) = 0 and itemid > 0"
            Call clsSvt.execSQLStatement(strsql)
        End If
        'MsgBox("15")
        If clsSvt.getDataLongValue("select count(itemid) from vwInvalidItems", 0, False) > 0 Then
            Dim objFrm As New frmRPtInvalidItems
            objFrm.StartPosition = FormStartPosition.CenterScreen
            objFrm.ShowDialog()
        End If
    End Sub

    Private Function IsLatestEXE() As Boolean
        IsLatestEXE = True
        Try
            Dim fso
            fso = CreateObject("Scripting.FileSystemObject")
            If Not fso.FileExists(Application.StartupPath & "\vLQMS.exe") Then Return True

            Dim strServerfile As String = gstrServerExePath
            If Not File.Exists(strServerfile) Then
                Return True
            End If

            'If fso.getFile(Application.StartupPath & "\" & fso.GetFileName(StrServerFile)).DateLastModified > fso.getFile(StrServerFile).DateLastModified Then
            '    Return True
            'End If

            If fso.getFile(Application.StartupPath & "\" & fso.GetFileName(strServerfile)).DateLastModified <> fso.getFile(strServerfile).DateLastModified Then
                Return False
            End If

        Catch ex As Exception
            Return True
        End Try
    End Function

    Public Sub InitCompData()
        'MsgBox("24")
        gstrServerExePath = getGsettings("Reports", "SrvrExeFilePath", "")
        gStrSystemName = UCase(My.Computer.Name.ToString())
        gStrSystemUserName = System.Environment.UserName.ToString().ToUpper()
        gstrRptPath = getGsettings("Reports", "RptFilePath", Application.StartupPath & "\")
        Select Case My.Computer.Name.ToUpper()
            Case "MADHUWORK"
                If Not Directory.Exists(gstrRptPath) Then
                    If Not Directory.Exists("E:\T\Rpts") Then Directory.CreateDirectory("E:\T\Rpts")
                    gstrRptPath = "e:\T\Rpts\"
                End If
            Case Else
                gstrRptPath = getGsettings("Reports", "RptFilePath", Application.StartupPath & "\")
        End Select
        ''MsgBox("25")
        If Not Directory.Exists(gstrRptPath) Then
            MsgBox("Invalid Rpt Path!", MsgBoxStyle.Critical, Me.Text)
        Else
            Select Case gStrSystemUserName
                Case "SSPL7", "SOFTVENT", "SSPL19", "SSPLMINISRVR"
                Case Else
                    If Not IsLatestEXE() Then
                        If MsgBox("You are Using old EXE... Pls click yes To update New version.... ", MsgBoxStyle.YesNo, Me.Text) = MsgBoxResult.Yes Then
                            If File.Exists(Application.StartupPath & "\BakvLQMS.exe") Then
                                File.Delete(Application.StartupPath & "\BakvLQMS.exe")
                            End If
                            File.Move(Application.StartupPath & "\vLQMS.exe", Application.StartupPath & "\BakvLQMS.exe")
                            File.Copy(gstrServerExePath, Application.StartupPath & "\vLQMS.exe")
                            MsgBox("Successfully updated latest EXE.... Pls restart the application", MsgBoxStyle.Information, Me.Text)
                            End
                        End If
                    End If
            End Select
            gstrQRImagePath = gstrRptPath & "QRImage\"
            Try
                If Not Directory.Exists(gstrRptPath & "QRImage") Then
                    Call Directory.CreateDirectory(gstrRptPath & "QRImage")
                End If
            Catch ex As Exception
            End Try

            If gstrItemImagePath = "" Then gstrItemImagePath = gstrRptPath & "ItemImage\"
            Try
                If Not Directory.Exists(gstrRptPath & "ItemImage") Then
                    Call Directory.CreateDirectory(gstrRptPath & "ItemImage")
                End If
            Catch ex As Exception
                MsgBox("Invalid Item Image Rpt Path!" & vbCrLf & ex.Message, MsgBoxStyle.Critical, Me.Text)
            End Try

            'Modified by Thangam
            If gstrPartyImagePath = "" Then gstrPartyImagePath = gstrRptPath & "PartyImage\"
            Try
                If Not Directory.Exists(gstrRptPath & "PartyImage") Then
                    Call Directory.CreateDirectory(gstrRptPath & "PartyImage")
                End If
            Catch ex As Exception
                MsgBox("Invalid Party Image Rpt Path!" & vbCrLf & ex.Message, MsgBoxStyle.Critical, Me.Text)
            End Try

            If gstrCPImagePath = "" Then gstrCPImagePath = gstrRptPath & "CustFiles\ContactFiles\"
            Try
                If Not Directory.Exists(gstrRptPath & "CustFiles") Then
                    Call Directory.CreateDirectory(gstrRptPath & "CustFiles")
                End If
                If Not Directory.Exists(gstrRptPath & "\Custfiles\" & "ContactFiles") Then
                    Call Directory.CreateDirectory(gstrRptPath & "\Custfiles\" & "ContactFiles")
                End If
            Catch ex As Exception
                MsgBox("Invalid Party Image Rpt Path!" & vbCrLf & ex.Message, MsgBoxStyle.Critical, Me.Text)
            End Try

            If gstrAutoSave = "" Then gstrAutoSave = Application.StartupPath & "\" & "AutoSave\"
            Try
                If Not Directory.Exists(gstrAutoSave) Then
                    Call Directory.CreateDirectory(gstrAutoSave)
                End If
            Catch ex As Exception
                MsgBox("Invalid Auto Save Path!" & vbCrLf & ex.Message, MsgBoxStyle.Critical, Me.Text)
            End Try


            If gstrDataPath = "" Then gstrDataPath = gstrRptPath
            Try

                If Not Directory.Exists(gstrDataPath & "TrnDocFiles") Then
                    Call Directory.CreateDirectory(gstrDataPath & "TrnDocFiles")
                End If
            Catch ex As Exception
            End Try

            'FilePath for TallyConfigurator in exe folder
            If gstrDataPath = "" Then gstrDataPath = gstrRptPath
            Try
                If Not Directory.Exists(gstrDataPath & "TallyConfigurator") Then
                    Call Directory.CreateDirectory(gstrDataPath & "TallyConfigurator")
                End If
            Catch ex As Exception
            End Try

            gstrAudioPath = gstrRptPath & "Audio\"
            Try
                If Not Directory.Exists(gstrAudioPath) Then
                    Call Directory.CreateDirectory(gstrAudioPath)
                End If
            Catch ex As Exception
                MsgBox("Invalid Audio Path!" & vbCrLf & ex.Message, MsgBoxStyle.Critical, Me.Text)
            End Try

            Try
                If Not Directory.Exists(gstrAudioPath & "RecordedVoice\") Then
                    Call Directory.CreateDirectory(gstrAudioPath & "RecordedVoice\")
                End If
            Catch ex As Exception
                MsgBox("Invalid Recorded Voice Path!" & vbCrLf & ex.Message, MsgBoxStyle.Critical, Me.Text)
            End Try

            Try
                If Not Directory.Exists(gstrAudioPath & "VoiceCommand\") Then
                    Call Directory.CreateDirectory(gstrAudioPath & "VoiceCommand\")
                End If
            Catch ex As Exception
                MsgBox("Invalid Voice Command Path!" & vbCrLf & ex.Message, MsgBoxStyle.Critical, Me.Text)
            End Try
        End If
        'MsgBox("26")
        Try
            If Not File.Exists(gstrRptPath & "\vLQMS Designer.exe") Then
                btnDesigner.Visible = False
            Else
                btnDesigner.Visible = True
            End If
        Catch ex As Exception

        End Try
        'MsgBox("27")
        Try
            If Not File.Exists(gstrRptPath & "\TallyConfigurator\svtTallyExport.exe") Then
                btnConfigurator.Visible = False
            Else
                btnConfigurator.Visible = True
            End If
        Catch ex As Exception

        End Try

        gStrHelpFilePath = gstrRptPath & "Help\"



        ' MsgBox("28")
        Call addNewFields()
        ' MsgBox("29")
        Call ChkFinYearCreation()

        'MsgBox("30")
        Call initGSettings()
        'MsgBox("31")
        'Reduce Graphics Flicker with Double Buffering for Forms and Controls
        SetStyle(ControlStyles.OptimizedDoubleBuffer, True)

        'get the values from UtlMisc to Variables
        Call GetMiscFlds()

        gblnUseCountry = True
        gblnUseState = True
        gblnPartyEditTrn = False
        gblnShowMultiAddTrn = True

        gStrSystemName = UCase(My.Computer.Name.ToString())
        gStrSystemUserName = System.Environment.UserName.ToString().ToUpper()

        'MsgBox("31")
        'load from ini to variables
        Call getValuesFromGSettings()
        'MsgBox("32")
        'load image
        Call loadMainPageImage()


        'MsgBox("33")
        Me.BackgroundImage = Nothing
        Select Case Command()
            Case "DROPSEMAIL", "WEBDATASYNC"
                gUserCode = "ADMIN"
                gUserID = 1
            Case Else
                Select Case gStrServerName.ToUpper
                    Case "MADHUWORK\SQL14", "IBMSSPLSRVR", "SSPLMINISRVR", "IBMSSPLSRVR\SPR14", "VENUS2\SQL22", "IBMSSPLSRVR\MSSQLSERVER01"
                        gUserCode = "ADMIN"
                        gUserID = 1
                        gBlnCheckForFraud = True
                        GoTo Nxt
                End Select

                Select Case gStrServerName.ToUpper
                    Case "MADHUWORK\SQL14", "IBMSSPLSRVR", "IBMSSPLSRVR\MSSQLSERVER01"
                        gUserCode = "ADMIN"
                        gUserID = 1
                        gBlnCheckForFraud = True
                    Case Else
                        If gblnADAuth Then
                            If gblnADAutolog Then
                                gUserID = clsSvt.getDataLongValue("Select UserID from MstUser where Authorised = 'Y' and UserCode <> 'ADMIN' and UserCode = '" & gStrSystemUserName & "'", 0, False)
                                If gUserID = 0 Then
                                    MsgBox("Invalid User Code : " & gStrSystemUserName & vbCrLf & " Please ask System Administrator to create user " & gStrSystemUserName & " in User master", MsgBoxStyle.Critical, "Invalid User")
                                    End
                                End If
                                gUserCode = gStrSystemUserName
                            Else
                                If gLoginReq = "N" Then
                                    gUserCode = "ADMIN"
                                    gUserID = 1
                                Else
                                    frmLogin.ShowDialog()
                                End If
                            End If
                        Else
                            If gLoginReq = "N" Then
                                gUserCode = "ADMIN"
                                gUserID = 1
                            Else
                                frmLogin.ShowDialog()
                            End If
                        End If
                End Select
        End Select
Nxt:
        'MsgBox("34")
        'Checking fld because Menu will be loaded
        If Not fnFldPresent("DashBoardReq", "UtlMisc") Then   'CNUD
            clsSvt.execSQLStatement("Alter table UtlMisc add DashBoardReq char(1) Default('N') Not Null")
        End If
        If Not fnFldPresent("RateMntc", "UtlMisc") Then   'CNUD
            clsSvt.execSQLStatement("Alter table UtlMisc add RateMntc varchar(1) Default('N') Not Null")
            Call clsSvt.execSQLStatement("Update UtlMisc set RateMntc = 'N'")
            Call clsSvt.execSQLStatement("Update MstMenu set MenuShow = 'N' where MenuItemCaption = 'Item Price Maintenance Master'")
        Else
            If clsSvt.getDataStringValue("Select RateMntc from UtlMisc", "N", False) = "Y" Then
                Call clsSvt.execSQLStatement("Update MstMenu set MenuShow = 'Y' where MenuItemCaption = 'Item Price Maintenance Master'")
            Else
                Call clsSvt.execSQLStatement("Update MstMenu set MenuShow = 'N' where MenuItemCaption = 'Item Price Maintenance Master'")
            End If
        End If
        'MsgBox("35")
        If Not fnFldPresent("ProfitMgnInvFld", "UtlMisc") Then   'CNUD
            clsSvt.execSQLStatement("Alter table UtlMisc add ProfitMgnInvFld varchar(50) Default('') Not Null")
            Call clsSvt.execSQLStatement("Update UtlMisc set ProfitMgnInvFld = 'SINVAmt'")
        End If
        'MsgBox("36")
        If Not fnFldPresent("AllowSplit", "UtlMisc") Then   'CNUD
            clsSvt.execSQLStatement("Alter table UtlMisc add AllowSplit char(1) Default('Y') Not Null")
        End If
        If Not fnFldPresent("InvPrintFromSP", "UtlMisc") Then   'CNUD
            clsSvt.execSQLStatement("Alter table UtlMisc add InvPrintFromSP char(1) Default('N') Not Null")
        End If
        If Not fnFldPresent("InvPrintFromXL", "UtlMisc") Then   'CNUD
            clsSvt.execSQLStatement("Alter table UtlMisc add InvPrintFromXL char(1) Default('N') Not Null")
        End If

        If Not fnFldPresent("ReWriteINPROGRESS", "UtlMisc") Then   'CNUD
            clsSvt.execSQLStatement("Alter table UtlMisc add ReWriteINPROGRESS char(1) Default('N') Not Null")
        End If
        If Not fnMenuPresent("Split Data") Then
            Call fnCreateMenu("Split Data", "", MnuNode.Tools)
        End If
        'MsgBox("37")
        lblUsername.Text = gUserCode
        Call GetUserImage()

        If fnMenuPresent("Standard Reports") Then
            Call clsSvt.execSQLStatement("Delete from MstMenu where MenuItemCaption = 'Standard Reports'")
            Call clsSvt.execSQLStatement("Delete from MstuserRights where MenuItemID not in (select MenuItemID from MstMenu)")
        End If

        If Not fnMenuPresent("Stock Balance Report") Then
            Call fnCreateMenu("Stock Balance Report", "", MnuNode.Reports)
        End If

        If Not fnMenuPresent("Stock Ledger Report") Then
            Call fnCreateMenu("Stock Ledger Report", "", MnuNode.Reports)
        End If

        If Not fnMenuPresent("Stock Ageing Report") Then
            Call fnCreateMenu("Stock Ageing Report", "", MnuNode.Reports)
        End If
        If Not fnMenuPresent("Stock Level Report") Then
            Call fnCreateMenu("Stock Level Report", "", MnuNode.Reports)
        End If

        If Not fnMenuPresent("Daily Summary Report") Then
            Call fnCreateMenu("Daily Summary Report", "", MnuNode.Reports)
        End If
        'If Not fnMenuPresent("Profit Margin Report") Then
        '    Call fnCreateMenu("Profit Margin Report", "", MnuNode.Reports)
        'End If

        If Not fnMenuPresent("Package Wise Sales Report") Then
            Call fnCreateMenu("Package Wise Sales Report", "", MnuNode.Reports)
        End If
        If Not fnMenuPresent("Package Wise Purchase Report") Then
            Call fnCreateMenu("Package Wise Purchase Report", "", MnuNode.Reports)
        End If
        If Not fnMenuPresent("Replace and Remove Duplicate items") Then
            Call fnCreateMenu("Replace and Remove Duplicate items", "", MnuNode.Utilities)
        End If
        'MsgBox("38")
        Call AddMenusFromDB()
        'MsgBox("39")
        gCurID = getGsettings("COMPANY", "DefCurrencyID", 1)
        gstrCurCode = clsSvt.getDataStringValue("Select CurCode from MstCur where CurID = " & gCurID, "Rs", False)
        If gstrCurCode = "" Then gstrCurCode = "Rs"
        gstrCurName = clsSvt.getDataStringValue("Select CurName from MstCur where CurID = " & gCurID, "Rupees", False)
        If gstrCurName = "" Then gstrCurName = "Rupees"
        gstrCurDecName = clsSvt.getDataStringValue("Select CurDecimal from MstCur where CurID = " & gCurID, "Rupees", False)
        If gstrCurDecName = "" Then gstrCurDecName = "Paise"

        gstrVsfItemCodeCols = getGsettings("Items Details", "VsfItemCodeCols", "")
        gCustQryFlds = getGsettings("Party", "QryCust", "")
        gSuppQryFlds = getGsettings("Party", "QrySupp", "")
        gStrChkValue = getGsettings("PurchaseBill", "CheckPBValue", "")
        Call GenerateUtlFldLst()

        Call loadMainPageImage()
        'MsgBox("40")
        If Not Directory.Exists(Application.StartupPath & "\Temp") Then
            Directory.CreateDirectory(Application.StartupPath & "\Temp")
        End If
        'MsgBox("41")
        If gUserCode = "" Then End
        Call EnableDisableMenus()
        'MsgBox("42")
        initGridStyles(Me)
        sbPnlDB.Text = "DB : " & gStrDBName
        sbPnlLogin.Text = "Login : " & gUserCode

        'MsgBox("43")
        gSysTempPath = System.IO.Path.GetTempPath
        Call chkDemoVersion()

        vsfINI.Cols(2).TextAlign = TextAlignEnum.LeftCenter
        vsfINI.AutoSizeCols()
        KeyPreview = True
        'MsgBox("44")
        Call CreateRoundObj(picLogin)
        'MsgBox("45")
        Call chkCounterWiseResetEnabled()

        'Me.Text = "Softvent vLQMS .Net Ver 7.0 - " & gSettings.gCompName & " 18-Jan-2023, 10:09 AM"
        'FIFO rate in grp summary
        'Me.Text = "Softvent vLQMS .Net Ver 7.0 - " & gSettings.gCompName & " 17-July-2023, 12:55 PM"

        'Rate Maintenance
        'Me.Text = "Softvent vLQMS .Net Ver 7.0 - " & gSettings.gCompName & " 17-July-2023, 7:26 PM"

        'Actual Sales Value in Invoice
        'Me.Text = "Softvent vLQMS .Net Ver 7.0 - " & gSettings.gCompName & " 25-July-2023, 5:10 PM"

        'Profit Margin Changed formula
        'Me.Text = "Softvent vLQMS .Net Ver 7.0 - " & gSettings.gCompName & " 14-Aug-2023, 4:15 PM"

        'Network discconnection resolved
        'Me.Text = "Softvent vLQMS .Net Ver 7.0 - " & gSettings.gCompName & " 04-Sep-2023, 3:41 PM"

        'Profit margin FIFO
        'Me.Text = "Softvent vLQMS .Net Ver 7.0 - " & gSettings.gCompName & " 15-Sep-2023, 6:41 PM"

        'Login issue
        'Me.Text = "Softvent vLQMS .Net Ver 7.0 - " & gSettings.gCompName & " 26-Sep-2023, 11:50 AM"

        'Barcode form Included
        'Me.Text = "Softvent vLQMS .Net Ver 7.0 - " & gSettings.gCompName & " 28-Sep-2023, 3:15 PM"

        'Me.Text = "Softvent vLQMS .Net Ver 7.0 - " & gSettings.gCompName & " 14-Dec-2023, 12:50 PM"


        'New License Info
        'Me.Text = "Softvent vLQMS .Net Ver 7.0 - " & gSettings.gCompName & " 18-Dec-2023, 9:41 AM"

        'Zero sales counter id message
        'Me.Text = "Softvent vLQMS .Net Ver 7.0 - " & gSettings.gCompName & " 20-Dec-2023, 7:30 PM"

        'Me.Text = "Softvent vLQMS .Net Ver 7.0 - " & gSettings.gCompName & " 30-May-2024, 8:35 PM"
        ' Me.Text = "Softvent vLQMS .Net Ver 8.0 - " & gSettings.gCompName & " 06-Aug-2024, 01:10 PM"

        'Me.Text = "Softvent vLQMS .Net Ver 8.0 - " & gSettings.gCompName & " 06-Aug-2025, 01:20 PM"
        'Changed for Alcoholic AROED calculation in SinglePB Entry

        Me.Text = "Softvent vLQMS .Net Ver 8.0 - " & gSettings.gCompName & " 06-Aug-2026, 03:55 PM"

        intAppIdleTime = getGsettings("", "APPIDLETIME", 0)
        If intAppIdleTime > 0 Then
            Application.AddMessageFilter(Me)
        End If

        Call loadFormColors()
        lblSoftvent.BackColor = Color.Transparent

        Dim intMenuID As Int32 = 2
        For intno As Integer = 0 To tvMain.Nodes(intMenuID - 1).GetNodeCount(False) - 1
            If tvMain.Nodes(intMenuID - 1).Nodes(intno).Text = strSeparator Then
                tvMain.Nodes(intMenuID - 1).Nodes(intno).ForeColor = Color.DimGray
            End If
        Next
        ' MsgBox("46")
        tvMain.ForeColor = gMainForeColor
        tvMain.BackColor = gMainBackColor
        tvMain.Font = gMainFontName
        pnlMain.BackColor = gMainBackColor
        lblUsername.ForeColor = gMainForeColor
        lblUsername.Font = gMainFontName
        txtSearch.Focus()

        Dim reader As String = ""
        Hotkey.registerHotkey(Me, "Q", Hotkey.KeyModifier.Control)
        'AddHandler System.Windows.Forms.Application.Idle, AddressOf Application_Idle
        'MsgBox("47")
        Call ChangeFontSize(Me)

        gblnVoucherPresent = False
        If clsSvt.getDataLongValue("Select top 1 DocID from UtlTransactions where DocTYpe = 'Voucher'", 0, False) > 0 Then
            gblnVoucherPresent = True
            Call syncCustSuppLedger()
        End If

        'used for testing TRANSACTION EXCEEDED CAPACITY
        'Dim svtTrn As OleDb.OleDbTransaction
        'svtTrn = svtConn.BeginTransaction(IsolationLevel.ReadCommitted)
        'Call clsSvt.execSQLStatement("Update MstItem set itemcode = '-' where itemid = 0", svtTrn)
        'svtTrn.Commit()

        Dim strINIFileName As String = RemoveSplChar(gUserCode)
        'MsgBox("48")
        Try
            If Directory.Exists(gstrRptPath) Then
                If Not Directory.Exists(gstrRptPath & "Notification") Then
                    Call Directory.CreateDirectory(gstrRptPath & "Notification")
                End If
                If Not File.Exists(gstrRptPath & "Notification\" & strINIFileName & ".ini") Then
                    File.WriteAllText(gstrRptPath & "Notification\" & strINIFileName & ".ini", "N")
                End If
            End If
        Catch ex As Exception

        End Try
        'MsgBox("49")
        Try
            Dim xlapp As New Microsoft.Office.Interop.Excel.Application
            gBlnMSOffice = True
            xlapp.Quit()
        Catch ex As Exception
            gBlnMSOffice = False
        End Try

        sbMic.Visible = False
        'MsgBox("50")
        Call welcomeMsg()

        gDblBeerLmt = getGsettings("Invoice", "AllowBeerInLtr", "1")
        gDblIMLlmt = getGsettings("Invoice", "AllowIMLinLtr", "1")
        gDblWineLmt = getGsettings("Invoice", "AllowWineInLtr", "1")
        gdblSalesValLmt = clsSvt.getDataDoubleValue("Select invSMSSalesvalue from UtlMisc", 0, False)
        gstrChkRateFld = clsSvt.getDataStringValue("Select RateFld from UtlMisc", "", False)
        gstrInvPrintFromSP = clsSvt.getDataStringValue("Select InvPrintFromSP from UtlMisc", "", False)
        gstrInvPrintFromXL = clsSvt.getDataStringValue("Select InvPrintFromXL from UtlMisc", "", False)
        gStrWatsAppFile = clsSvt.getDataStringValue("Select WAMsgFile From UtlMisc", "", False)
        gStrWatsAppMsg = clsSvt.getDataStringValue("Select WAMsg From UtlMisc", "", False)
        gStrWatsAppRcvrNo = clsSvt.getDataStringValue("Select WADefRcvr From UtlMisc", "", False)
        lblComp.Font = New Font(Me.Font.FontFamily, 9)
        gAppStarted = True
        lblFin.Font = New Font(Me.Font.FontFamily, 9)
        Try
            timerNotify.Stop()
        Catch ex As Exception

        End Try
        Call initSMSSettings()

        'MsgBox("51")
        timerNotify.Start()
        timerNotify.Interval = 10000
        'If clsSvt.getDataStringValue("Select SERVERPROPERTY('MachineName')", "", False).ToUpper = gStrSystemName.ToUpper Then
        '    Call updateNewLicInfo()
        'End If
    End Sub

    Public Sub frmMain_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Dim strLOGFile As String = ""

        '  If Not blnAppStarted Then
        If MsgBox("Exit from software? ", vbQuestion + vbYesNo + vbDefaultButton1, "Softvent..." & gSettings.gCompName) = vbNo Then
            If Not e Is Nothing Then e.Cancel = True
            Exit Sub
        End If
        ' End If

        If MdiChildren.Count > 0 Then
            For Each child As Form In MdiChildren
                child.Close()
            Next
        End If

        Call SyncDB()
        Call saveGridToINI()

        '01Sep
        Hotkey.unregisterHotkeys(Me)
        svtConn.Close()

        svtConn.Dispose()
        GC.Collect()
        End
    End Sub

    Public Sub saveGridToINI()
        Dim i As Integer, blnFound As Boolean
        Call INIStore("DataPath", "DataFolder", gstrDataPath)

        With vsfINI
            .ClipSeparators = ";" & vbCr
            Do While True
                For i = 0 To .Rows.Count - 1
                    If .GetData(i, 0) = "" Then
                        .RemoveItem(i)
                        Exit For
                    End If
                Next i
                blnFound = False
                For i = 0 To .Rows.Count - 1
                    If .GetData(i, 0) = "" Then
                        blnFound = True
                        Exit For
                    End If
                Next i

                If Not blnFound Then Exit Do
            Loop
            .SaveGrid(Application.StartupPath & "\" & gINIFileName, FileFormatEnum.TextCustom)
        End With
    End Sub

    Private Sub initINIGrid()
        Dim strIniFile As String = Application.StartupPath & "\" & gINIFileName
        Dim strInifileBak As String = strIniFile & "bak"
        Dim i As Integer, blnFound As Boolean = False
LblBegin:
        With vsfINI
            .Cols.Fixed = 0
            .Cols.Count = 3
            .Rows.Count = 0
        End With

        If Not File.Exists(strInifileBak) Then
            If File.Exists(strIniFile) Then
                'take backup file if not present
                File.Copy(strIniFile, strInifileBak)
            End If
        End If

        If Not File.Exists(strIniFile) Then
            If File.Exists(strInifileBak) Then
                'copy backup file if not ini not present
                File.Copy(strInifileBak, strIniFile)
            Else
                MsgBox("INI File corrupted! No backup INI. Please copy from server!", MsgBoxStyle.Critical, Me.Text)
                Exit Sub
            End If
        End If

        With vsfINI
            .Cols.Fixed = 0
            .Cols.Count = 3
            .Rows.Count = 0
            .ClipSeparators = ";" & vbCr
            If File.Exists(Application.StartupPath & "\" & gINIFileName) Then
                .LoadGrid(Application.StartupPath & "\" & gINIFileName, FileFormatEnum.TextCustom)
            End If

            For i = 0 To .Rows.Count - 1
                .SetData(i, 0, Replace(GetVal(.GetData(i, 0)).ToString, vbTab, ""))
                .SetData(i, 1, Replace(GetVal(.GetData(i, 1)).ToString, vbTab, ""))
                .SetData(i, 2, Replace(GetVal(.GetData(i, 2)).ToString, vbTab, ""))
                If .GetData(i, 0) = "CONNPATH" Then
                    gBlnConnectionPath = True
                    gstrConnectionPath = .GetData(i, 2)
                    If Strings.Right(gstrConnectionPath, 1) <> "\" Then gstrConnectionPath = gstrConnectionPath & "\"
                    If Not File.Exists(gstrConnectionPath & "SvtConn.ini") Then
                        gBlnConnectionPath = False
                    End If
                End If
            Next
        End With

        If gBlnConnectionPath Then
            Dim vsf As New C1FlexGrid
            With vsf
                .Cols.Fixed = 0
                .Cols.Count = 3
                .Rows.Count = 0
                .ClipSeparators = ";" & vbCr
                If File.Exists(gstrConnectionPath & "SvtConn.ini") Then
                    .LoadGrid(gstrConnectionPath & "SvtConn.ini", FileFormatEnum.TextCustom)
                End If

                For i = 0 To .Rows.Count - 1
                    .SetData(i, 0, Replace(GetVal(.GetData(i, 0)).ToString, vbTab, ""))
                    .SetData(i, 1, Replace(GetVal(.GetData(i, 1)).ToString, vbTab, ""))
                    .SetData(i, 2, Replace(GetVal(.GetData(i, 2)).ToString, vbTab, ""))
                Next
            End With

            gStrServerName = INIGetSerLoc(vsf, "SqlConn", "ServerName", "")
            gStrDBName = INIGetSerLoc(vsf, "SqlConn", "DBName", "")
            gStrDBLoginName = INIGetSerLoc(vsf, "SqlConn", "DBUserCode", "sa")
            gStrDBLoginPwd = svtCrypto.Decrypt(INIGetSerLoc(vsf, "SqlConn", "DBLoginPwd", ""))

            'write to system registry
            If gStrServerName <> "" And gStrDBLoginPwd <> "" Then
                Dim s As String
                SaveSetting("VLQMS", "INI", "SERVERNAME", gStrServerName)
                SaveSetting("VLQMS", "INI", "DBNAME", gStrDBName)
                SaveSetting("VLQMS", "INI", "LOGINNAME", gStrDBLoginName)
                SaveSetting("VLQMS", "INI", "LOGINPWD", gStrDBLoginPwd)
            End If
        End If
    End Sub

    Private Function initDBSettingsMSSQL() As Boolean
        'MsgBox("16")
        initDBSettingsMSSQL = False
        'IBMSSPLSrvrßsaßsvt@123$$ßPosVogueßADMINß1
        strCmd = Command()
        Select Case strCmd
            Case ""
                If gStrServerName = "" AndAlso INIGet("SqlConn", "ServerName", "") <> "" Then
                    gStrServerName = INIGet("SqlConn", "ServerName", "")
                    gStrDBName = INIGet("SqlConn", "DBName", "")
                    gStrDBLoginName = INIGet("SqlConn", "DBUserCode", "sa")
                    gStrDBLoginPwd = svtCrypto.Decrypt(INIGet("SqlConn", "DBLoginPwd", ""))
                End If
            Case "DROPSEMAIL", "WEBDATASYNC"
                'blnCustAge = True
                If gStrServerName = "" AndAlso INIGet("SqlConn", "ServerName", "") <> "" Then
                    gStrServerName = INIGet("SqlConn", "ServerName", "")
                    gStrGlobeDBName = INIGet("SqlConn", "GlobeDBName", "")
                    gStrDBName = INIGet("SqlConn", "DBName", "")
                    gStrDBLoginName = INIGet("SqlConn", "DBUserCode", "sa")
                    gStrDBLoginPwd = svtCrypto.Decrypt(INIGet("SqlConn", "DBLoginPwd", ""))
                End If

            Case Else
                Try
                    Dim arrCMD() = Split(strCmd, "ß")
                    gStrServerName = arrCMD(0)
                    gStrDBName = arrCMD(3)
                    gStrDBLoginName = arrCMD(1)
                    gStrDBLoginPwd = arrCMD(2)
                    gUserCode = arrCMD(4)
                    gUserID = Val(arrCMD(5))
                Catch ex As Exception
                    MsgBox("Invalid Command Line arguments!", MsgBoxStyle.Critical, "Cannot Execute Application")
                    End
                End Try
        End Select

        If gStrDBName = "" Or gStrServerName = "" Or gStrDBLoginName = "" Then
            frmUtlDBConnection.ShowDialog()
            Exit Function
        End If

        If Not openMSSqlConn(gStrServerName, gStrDBName, gStrDBLoginName, gStrDBLoginPwd) Then
            frmUtlDBConnection.ShowDialog()
            Exit Function
        End If

        If clsSvt.getDataStringValue("Select AdvancedADMLogin from UtlMisc", "N", False) = "Y" Then
            gBlnAdvAdminLogin = True
        Else
            gBlnAdvAdminLogin = False
        End If
        'MsgBox("17")
        'open the default company
        Call openDefCompany()

        If Not gBlnConnectionPath Then
            Dim vsf As New C1FlexGrid
            With vsf
                .Cols.Fixed = 0
                .Cols.Count = 3
                .Rows.Count = 0
                .Rows.Count = 5

                .ClipSeparators = ";" & vbCr

                .SetData(0, 0, "SqlConn")
                .SetData(0, 1, "ServerName")
                .SetData(0, 2, gStrServerName)

                .SetData(1, 0, "SqlConn")
                .SetData(1, 1, "DBName")
                .SetData(1, 2, gStrDBName)

                .SetData(2, 0, "SqlConn")
                .SetData(2, 1, "DBUserCode")
                .SetData(2, 2, gStrDBLoginName)

                .SetData(3, 0, "SqlConn")
                .SetData(3, 1, "DBLoginPwd")
                .SetData(3, 2, svtCrypto.Encrypt(gStrDBLoginPwd))

                .SetData(4, 0, "SqlConn")
                .SetData(4, 1, "GLOBEDBNAME")
                .SetData(4, 2, gStrGlobeDBName)
                .SaveGrid(gstrRptPath & "SvtConn.ini", FileFormatEnum.TextCustom)
                .SaveGrid(Application.StartupPath & "\ServerDetail.ini", FileFormatEnum.TextCustom)
                INIStore("CONNPATH", "PATH", gstrRptPath)
            End With
            With vsfINI
STAGAIN:
                For i = 0 To .Rows.Count - 1
                    If "SQLCONN" = UCase(Trim(.GetData(i, 0))) Then
                        .RemoveItem(i)
                        GoTo STAGAIN
                    End If
                Next i
            End With
        End If

        initDBSettingsMSSQL = True
    End Function

    Private Sub openDefCompany()
        'MsgBox("18")
        If Not clsSvt.execSQLStatement("Use " & gStrDBName) Then
            End
        End If
        ' MsgBox("19")
        Call initGSettings()
        ' MsgBox("20")
        Call InitCompData()
    End Sub

    Public Sub initGSettings()
        'MsgBox("21")
        If gDBTypeCurrent = gDbTypeMSSQL Then
            If clsSvt.getDataStringValue("select name from sys.objects where name = 'getsvtdate'", "", False) = "" Then
                gSqlGetDate = "GetDate()"
            Else
                gSqlGetDate = "dbo.GetSvtDate()"
            End If
        End If

        Dim rsDummy As OleDb.OleDbDataReader

        rsDummy = clsSvt.getDataReaderFromSQL("Select * from MstComp")

        With rsDummy
            If .Read Then
                gSettings.gCompName = rsDummy("CompName")
                'gSettings.gCompNo = rsDummy("CompNo")
                gSettings.gCompAdd1 = GetVal(rsDummy("Address1"), "")
                gSettings.gCompAdd2 = GetVal(rsDummy("Address2"), "")
                gSettings.gCompAdd3 = GetVal(rsDummy("Address3"), "")
                gSettings.gCompAdd4 = GetVal(rsDummy("Address4"), "")
                gSettings.gCompAdd5 = GetVal(rsDummy("Address5"), "")
                gSettings.gCompGSTIN = GetVal(rsDummy("GSTNO"), "")
                gSettings.gCompPHNO = GetVal(rsDummy("PHNO"), "")
                gCompMobNo = GetVal(rsDummy("MOBNO"), "")
                gSettings.gMDBName = gStrDBName
                .Close()
            End If
        End With
        rsDummy = clsSvt.getDataReaderFromSQL("Select * from  MstCompFinYear where Active = 'Y'")
        'MsgBox("22")
        With rsDummy
            .Read()
            If Not .HasRows Then
                MsgBox("No company is active !!", vbInformation, Me.Text)
                .Close()
                End
                Exit Sub
            End If
            gSettings.gCompEndDt = CDate(GetVal(!yrTDate, getSystemDate))
            gSettings.gCompStDt = CDate(GetVal(!yrfDate, getSystemDate))
            gSettings.gCompFinYear = Microsoft.VisualBasic.Strings.Right(Year(gSettings.gCompStDt).ToString, 2)
            .Close()
        End With
        Me.Text = "Softvent vLQMS .Net Ver 6.0" & " - " & gSettings.gCompName
        sbFinYear.Text = "FinYear : " & gSettings.gCompStDt & " - " & gSettings.gCompEndDt

        lblComp.Text = gSettings.gCompName
        lblFin.Text = "(" & "FinYear : " & gSettings.gCompStDt & " - " & gSettings.gCompEndDt & ")"
        gSmtpMailID = getGsettings("COMPANY", "SMTPMailID", "")
        gSmtpMailPwd = getGsettings("COMPANY", "SMTPMailPwd")
        gSmtpHostName = getGsettings("COMPANY", "SMTPHostName")
        gSmtpServerPort = getGsettings("COMPANY", "SMTPServerPort")
        gSmtpEnableSSL = getGsettings("COMPANY", "EnableSSL")
        gStrRptEmailID = getGsettings("REPORTS", "RPTEMAILIDS", "")

        gBtnCtrlBackClr = Val(getGsettings("BTNCTRL", "BACKCLR", 0))
        gBtnCtrlForeClr = Val(getGsettings("BTNCTRL", "FORECLR", 0))

        Select Case gStrDBName.ToUpper()
            Case "VLQMSVBS", "VLQMSHLB", "VLQMSRAJADHANI"
                gdtFreezeDate = clsSvt.getDataDateValue("select case when ISDATE(SetValue) = 1 then setvalue else '" & DateAdd(DateInterval.Day, -1, getSystemDate()) & "' end from UtlGSettings where SetName='FreezeDate'", gSettings.gCompStDt, False)
            Case Else
                gdtFreezeDate = clsSvt.getDataDateValue("select case when ISDATE(SetValue) = 1 then setvalue else '" & gSettings.gCompStDt & "' end from UtlGSettings where SetName='FreezeDate'", gSettings.gCompStDt, False)
        End Select



        If gStrDBName.ToUpper() = "" Then

        Else

        End If
        'MsgBox("23")
    End Sub

    Private Sub TransactionsClick(ByVal argMenuname As String)
        'MsgBox("Entered Trn Click")
        Dim intDocID As Integer, strFrmName As String

        strMnuName = argMenuname
        intDocID = clsSvt.getDataLongValue("Select DocID from mstMenu where MenuItemCaption = '" & strMnuName & "'", 0, False)
        strFrmName = clsSvt.getDataStringValue("Select FrmName from MstMenu where MenuItemCaption = '" & strMnuName & "'", "", False)

        Dim objfrm As Object = Nothing
        If strMnuName = "Redeem Points" Then
            objfrm = New frmTrnRDM
            objfrm.initForm(intDocID)
            OpenForms(objfrm)
            Exit Sub
        End If

        Select Case UCase(strFrmName)
            Case "FRMTRNSTKDED"
                If intDocID = 0 Then Exit Sub
                objfrm = New frmTrnSTKDED
                objfrm.initForm(intDocID)

            Case "FRMTRNSR"
                If intDocID = 0 Then Exit Sub
                objfrm = New frmTrnSR
                objfrm.initForm(intDocID)

            Case "FRMINV"
                If intDocID = 0 Then Exit Sub
                objfrm = New frmTrnInv
                objfrm.initForm(intDocID)
                If Not objfrm.blnSalesCounter Then
                    Exit Sub
                End If
                objfrm.txtScan.Focus()
                pnlProject.Panels(0).Visible = False

            Case "FRMTRNPB", "FRMTRNPI"
                If intDocID = 0 Then Exit Sub
                objfrm = New frmTrnPBM
                objfrm.initForm(intDocID)
                pnlProject.Panels(0).Visible = False

            Case "FRMTRPOINTINV"
                If intDocID = 0 Then Exit Sub
                objfrm = New frmTrnPosInv
                objfrm.initForm(intDocID)
                pnlProject.Panels(0).Visible = False

            Case "PSE"
                objfrm = New frmTrnPSE

            Case "CSE"
                objfrm = New frmTrnCloseStk
                objfrm.initForm(intDocID)

            Case Else
                If intDocID = 0 Then Exit Sub
                objfrm = New frmTrnPartyDoc
                objfrm.initForm(intDocID)





        End Select

        If Not IsNothing(objfrm) Then
            OpenForms(objfrm)
            If intDocID = 1216 Then
                objfrm.txtscan.focus()
                Call fillVsfHFTDefValues(objfrm)
            End If
        End If
    End Sub

    Private Sub VoucherClick(ByVal argMenuname As String)
        Dim intDocID As Integer, strFRMName As String, strDetSection As String = ""

        Dim tabshown, tabhidden As New TabControl
        Dim AllTabs As List(Of TabPage) = New List(Of TabPage)
        Dim tab As New C1.Win.C1Command.C1DockingTabPage

        strMnuName = argMenuname
        intDocID = clsSvt.getDataLongValue("Select DocID from mstMenu where MenuItemCaption = '" & strMnuName & "'", 0, False)

        strFRMName = clsSvt.getDataStringValue("Select FrmName from MstMenu where DocID = " & intDocID, "", False)

        Dim argfrm As New frmTrnVch
        Dim pnlForm As New Panel()
        Dim tb As New C1.Win.C1Command.C1DockingTabPage()
        tb.Text = strMnuName
        'Dim oldTAB As Padding = tb.Margin
        'tb.Margin = New Padding(5, oldTAB.Top, 0, oldTAB.Bottom)
        tb.DockTo(dockTabMain)
        tb.Parent = dockTabMain
        tb.Anchor = (AnchorStyles.Bottom Or AnchorStyles.Top Or AnchorStyles.Right Or AnchorStyles.Left)
        Me.dockTabMain.TabPages.Add(tb)
        Me.dockTabMain.SelectedTab = tb
        Me.dockTabMain.Visible = True
        argfrm.Anchor = (AnchorStyles.Bottom Or AnchorStyles.Top Or AnchorStyles.Right Or AnchorStyles.Left)
        argfrm.TopLevel = False
        argfrm.Dock = DockStyle.Fill
        argfrm.WindowState = FormWindowState.Maximized
        argfrm.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        'Dim old As Padding = argfrm.Margin
        'argfrm.Margin = New Padding(5, old.Top, 0, old.Bottom)
        tb.Controls.Add(argfrm)
        argfrm.initForm(intDocID)
        argfrm.Show()
        argfrm.srcTransaction.setFocus()

    End Sub

    Public Sub loadMainPageImage()
        Dim cmd As OleDb.OleDbCommand
        Dim strSQL As String
        strSQL = "Select isnull(JpgFile,'') from MstComp"
        Try
            cmd = New OleDb.OleDbCommand(strSQL, svtConn)
            Dim imageData As Byte() = DirectCast(cmd.ExecuteScalar(), Byte())
            If Not imageData Is Nothing Then
                Using ms As New MemoryStream(imageData, 0, imageData.Length)
                    ms.Write(imageData, 0, imageData.Length)
                    pnlForm.BackgroundImage = Image.FromStream(ms, True)
                End Using
            End If

        Catch ex As Exception
            MsgBox("Error during loading of MainPage image. (loadMainPageImage)", MsgBoxStyle.Critical, "ERROR")
        End Try
    End Sub

    Public Sub EnableDisableMenus()
        Dim strMenuKey As String = ""
        Call EnableMenuALL(tvMain)
        If gUserCode = "ADMIN" Then
            Exit Sub
        End If
        'Call hideMenus
        Dim rstUserRights As OleDb.OleDbDataReader
        Dim strQuery As String

        strQuery = "Select MstMenu.MenuItemID,MenuItemCaption"
        strQuery = strQuery & " From " & IIf(gblnUserBased, "MstUserRights", "MstRoleRights") & ", MstMenu"
        strQuery = strQuery & " Where LTRIM(RTRIM(" & IIf(gblnUserBased, "UserID", "RoleID") & ")) = " & IIf(gblnUserBased, gUserID, gRoleID) & " And "
        strQuery = strQuery & " " & IIf(gblnUserBased, "MstUserRights", "MstRoleRights") & ".MenuItemID=MstMenu.MenuItemID AND "
        strQuery = strQuery & " " & IIf(gblnUserBased, "UserRights", "RoleRights") & " = '000000000000'"
        strQuery = strQuery & " Union"
        strQuery = strQuery & " Select MstMenu.MenuItemID,MenuItemCaption"
        strQuery = strQuery & " From MstMenu"
        strQuery = strQuery & " Where 1 = 1"
        strQuery = strQuery & " and MstMenu.MenuItemID NOT in"
        strQuery = strQuery & " (select menuitemid from " & IIf(gblnUserBased, "MstUserRights", "MstRoleRights") & " "
        strQuery = strQuery & " where LTRIM(RTRIM(" & IIf(gblnUserBased, "UserID", "RoleID") & ")) = " & IIf(gblnUserBased, gUserID, gRoleID) & ")"

        rstUserRights = clsSvt.getDataReaderFromSQL(strQuery)
        With rstUserRights
            Do While .Read
                strMenuKey = "K" & !MenuItemID
                Dim nodes As TreeNode() = tvMain.Nodes.Find(strMenuKey, True)
                For Each node In nodes
                    node.ForeColor = Color.Gray
                    node.Tag = "NO"
                Next
            Loop
            .Close()
        End With
    End Sub

    Private Sub EnableMenuALL(ByVal aTreeView As TreeView)
        Dim n As TreeNode
        For Each n In aTreeView.Nodes
            Call chileNodeEnable(n)
        Next
    End Sub

    Private Sub chileNodeEnable(ByVal argTV As TreeNode)
        Try

            If My.Computer.Name.ToString.ToUpper() = "PLUTO" Then Exit Sub
            If argTV Is Nothing Then Exit Sub
            If argTV.GetNodeCount(True) > 0 Then
                Dim n As TreeNode
                For Each n In argTV.Nodes
                    Call chileNodeEnable(n)
                    If Not n Is Nothing Then
                        If n.Text.ToUpper = "ADMIN AUDIT LOG" Then
                            If Not gBlnCheckForFraud Then
                                n.Remove()
                            End If
                        End If
                    End If
                Next
            Else
                argTV.Tag = ""
                EnableNode(argTV)
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub EnableNode(ByVal n As TreeNode)
        Try
            n.ForeColor = Color.White
            Dim aNode As TreeNode
            For Each aNode In n.Nodes
                EnableNode(aNode)
            Next
        Catch ex As Exception

        End Try
    End Sub

    Private Sub sbPnlLogin_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles sbPnlLogin.Click
        If dockTabMain.TabPages.Count > 0 Then
            MsgBox("Please close all open pages in application..", MsgBoxStyle.Information, Me.Text)
            Exit Sub
        End If
        frmLogin.ShowDialog()
    End Sub

    Private Sub chkDemoVersion()
        Dim strNo As String
        Dim clsR As New clsReg
        strNo = GetSetting("SVTREG", "VIC", "VICkey", "")
        If strNo <> "YES" Then
            frmUtlReg.ShowDialog()
            End
        End If

        'Dim intTimes As Long
        'sbDemo.Text = "Demo"
        'intTimes = clsSvt.getDataLongValue("Select count(InvID) from trnhdrInv", 0, False)
        'If intTimes >= 100 Then
        '    Call chkKeys()
        '    sbDemo.Text = "Licensed Copy"
        'End If
    End Sub

    Private Sub lblSoftvent_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblSoftvent.Click
        Process.Start("www.softvent.com")
    End Sub

    Private Sub imgSoftvent_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles imgSoftvent.Click
        Process.Start("www.softvent.com")
    End Sub

    Public Sub OpenGeneralForm(argMnuName As String)
        'for fixed forms
        Dim intDocID As Integer, strDocType As String = "", strSQL As String
        Dim strCustSuppReq As String = "", strNonItemDoc As String = ""
        Dim argTBLName As String = "", argIDName As String = "", argCodeName As String = "", argFormCaption As String = "", argFLDCaption As String = ""
        Dim rs As OleDb.OleDbDataReader

        intDocID = clsSvt.getDataLongValue("select DOCID from MStMenu where upper(MenuItemCaption) = '" & UCase(argMnuName) & "'", 0, False)
        If intDocID = 0 Then Exit Sub

        strSQL = "Select * from UtlTransactions where DocID = " & intDocID

        rs = clsSvt.getDataReaderFromSQL(strSQL)

        With rs
            Do While .Read
                argTBLName = !HdrTblName
                argIDName = Replace(!HdrTblName, "Mst", "") & "ID"
                argCodeName = Replace(!HdrTblName, "Mst", "") & "Code"
                argFormCaption = !DocName
                argFLDCaption = !DocName
                strDocType = GetVal(!TrnDocType, "")
            Loop
            .Close()
        End With

        If strDocType = "General" Then
            Dim frmMstGeneral As New frmMstGeneral
            frmMstGeneral.initForm(intDocID, argTBLName, argIDName, argCodeName, argFormCaption, argFLDCaption, False)
            OpenForms(frmMstGeneral)

        ElseIf strDocType = "Party Variant" Then
            Dim frmParty As New frmMstParty
            frmParty.initForm(intDocID)
            OpenForms(frmParty, argFormCaption)

        ElseIf strDocType = "Item Variant" Then
            Dim argfrm As New frmMstItem
            argfrm.initForm(intDocID)
            OpenForms(argfrm)
        End If
    End Sub

    Public Sub setFinYear()
        sbFinYear.Text = "FinYear : " & gSettings.gCompStDt & " - " & gSettings.gCompEndDt
    End Sub

    Private Sub MstSubMenuClick(sender As System.Object, e As System.EventArgs)
        Call MasterClick(sender.text)
    End Sub

    Public Sub MasterClick(argMenu As String)
        Dim intDocID As Integer ', strDocType As String
        Dim strCustSuppReq As String = "", strNonItemDoc As String = ""
        strMnuName = argMenu


        Dim tabshown, tabhidden As New TabControl
        Dim AllTabs As List(Of TabPage) = New List(Of TabPage)
        Dim tab As New C1.Win.C1Command.C1DockingTabPage

        ''for fixed forms
        intDocID = clsSvt.getDataLongValue("select DOCID from MStMenu where MenuItemCaption = '" & strMnuName & "'", 0, False)
        Select Case UCase(strMnuName)
            Case "COMPANY MASTER"
                Dim argfrm As New FrmMstComp
                OpenForms(argfrm)

            Case "SELECT FINANCIAL YEAR"
                Dim argfrm As New FrmSelFinYear
                OpenForms(argfrm)

            Case "ITEM OPENING BALANCE"
                Dim argfrm As New frmMstOPBalEntry
                OpenForms(argfrm)

            Case "STATE CITY COUNTRY"
                Dim argfrm As New frmMstSCC
                argfrm.initForm(intDocID, "MstSCC", "SCCID", "City", "State City Country", "State City Country")
                OpenForms(argfrm)

            Case "UOM MASTER"
                Dim argfrm As New frmMstUOM
                argfrm.initForm(intDocID, "MstUOM", "", "UOMCODE", "UOMNAME", "UOM Master", "UOMCODE")
                OpenForms(argfrm)

            'Case "CURRENCY MASTER"
            '    Dim argfrm As New frmMstCurr
            '    argfrm.initForm(intDocID, "MstCUR", "CURID", "CURCODE", "CURNAME", "CURRENCY Master", "CURCODE")
            '    OpenForms(argfrm)

            Case "HOLIDAY MASTER"
                Dim argfrm As New frmMstHoliday
                OpenForms(argfrm)

            Case "BILL OF MATERIAL"
                Dim argfrm As New frmMstBOM
                argfrm.initForm(intDocID)
                OpenForms(argfrm)

            Case "SUPPLIER VS ITEM MASTER"
                Dim argfrm As New frmPartyItem
                argfrm.Text = "Supplier Vs Item Master"
                OpenForms(argfrm)

            Case "CUSTOMER VS ITEM MASTER"
                Dim argfrm As New frmPartyItem
                argfrm.Text = "Customer Vs Item Master"
                OpenForms(argfrm)

            Case "DOCTOR VS SERVICE MASTER"
                Dim argfrm As New frmPartyItem
                argfrm.Text = "Doctor vs SERVICE Master"
                OpenForms(argfrm)

            Case "ITEM PRICE MAINTENANCE MASTER"
                Dim argfrm As New frmMstPriceMaintenance
                OpenForms(argfrm)

            Case "EXIT"
                If MsgBox("Are you sure you want To Exit the application?", vbQuestion + vbYesNo + vbDefaultButton1, "Softvent..." & gSettings.gCompName) = vbYes Then
                    Me.Close()
                End If

            Case Else
                Call OpenGeneralForm(argMenu)
        End Select
    End Sub


    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick

        Static SecondsCount As Integer 'Counts each second
        SecondsCount += 1 'Increment
        If SecondsCount > intAppIdleTime Then 'minutes have passed since being active
            Try

                If Me.WindowState <> FormWindowState.Minimized Then
                    Timer1.Stop()
                    SecondsCount = 0
                    frmLogin.blnAppIdle = True
                    frmLogin.ShowDialog()
                Else
                    SecondsCount = intAppIdleTime + 100
                End If

            Catch
            End Try
        End If
    End Sub

    Public Function PreFilterMessage(ByRef m As System.Windows.Forms.Message) As Boolean Implements System.Windows.Forms.IMessageFilter.PreFilterMessage
        Dim mouse As Boolean = (m.Msg >= &H200 And m.Msg <= &H20D) Or (m.Msg >= &HA0 And m.Msg <= &HAD)
        Dim kbd As Boolean = (m.Msg >= &H100 And m.Msg <= &H109)
        If mouse Or kbd Then
            Timer1.Enabled = False
            Timer1.Enabled = True
            Timer1.Start()
        End If
        Return False
    End Function

    Private Sub ReportsClick(argMenu As String)
        strMnuName = argMenu

        Dim tabshown, tabhidden As New TabControl
        Dim AllTabs As List(Of TabPage) = New List(Of TabPage)
        Dim tab As New C1.Win.C1Command.C1DockingTabPage


        'for fixed forms
        Dim intDocID As Int32 = clsSvt.getDataLongValue("select DOCID from MStMenu where MenuItemCaption = '" & strMnuName & "'", 0, False)

        If dockTabMain.Visible <> True Then dockTabMain.Visible = True
        For i = 0 To dockTabMain.TabPages.Count - 1
            If dockTabMain.TabPages(i).Text = argMenu Then
                dockTabMain.TabPages(i).Show()
                Exit Sub
            End If
        Next

        Select Case UCase(argMenu)
            'Accounting Reports
            '-----------------------------------------------------------------------------
            Case "CUSTOMISED REPORTS..."
                Dim argFrm As New frmRptCustomiseRptView
                argFrm.fillReport()
                OpenForms(argFrm)
            Case "DROPS REPORTS", "QUENCH REPORTS"
                Dim argFrm As New frmRptsDrops
                OpenForms(argFrm)
            Case "LOYALTY REPORT"
                Dim argfrm As New frmRptLoyalty
                OpenForms(argfrm)
            Case "STOCK BALANCE REPORT"
                Dim argFrm As New frmRptStockBal
                OpenForms(argFrm)
            Case "STANDARD REPORTS"
                Dim argFrm As New frmRptReport
                OpenForms(argFrm)
            Case "BILL WISE AGEING"
                Dim argfrm As New FrmRptAccAgeBillWise
                OpenForms(argfrm)
            Case "GENERAL LEDGER"
                Dim argfrm As New FrmRptAccGL
                OpenForms(argfrm)
            Case "PARTY WISE OUTSTANDING"
                Dim argfrm As New frmRptAccOutstanding
                OpenForms(argfrm)
            Case "BILL WISE OUTSTANDING"
                Dim argfrm As New FrmRptAccOSBillWise
                OpenForms(argfrm)

            '-----------------------------------------------------------------------------
            Case "LABEL PRINTING"
                Dim argfrm As New frmRptLblPrint
                OpenForms(argfrm)
            Case "SQL QUERY EXECUTION"
                Dim argfrm As New frmUtlSQLQueryExec
                OpenForms(argfrm)

            Case "SQL SAVED QUERY"
                Dim argfrm As New frmRptQuery
                OpenForms(argfrm)

            Case "AUDIT LOG"
                Dim argfrm As New frmUtlRecAuditLog
                OpenForms(argfrm)

            Case "PDF GENERATOR"
                Dim argfrm As New frmUtlBulkPDFGenerator
                OpenForms(argfrm)

            Case "STOCK LEDGER REPORT"
                Dim argfrm As New frmRptStockLed
                OpenForms(argfrm)

            Case "STOCK AGEING REPORT"
                Dim argfrm As New frmRptStockAgeing
                OpenForms(argfrm)

            Case "STOCK LEVEL REPORT"
                Dim argfrm As New frmRptStkLevel
                OpenForms(argfrm)

            Case "Daily Summary Report".ToUpper
                Dim argfrm As New frmRptDailySummary
                OpenForms(argfrm, "Daily Summary Report", True)

            Case "Profit Margin Report".ToUpper
                Dim argfrm As New frmRptProMrg
                argfrm.strRptName = "Profit Margin Report"
                OpenForms(argfrm, "Profit Margin Report", True)

            Case "Package Wise Sales Report".ToUpper
                Dim argfrm As New frmRptSalesPkgwise
                OpenForms(argfrm, "Package Wise Sales Report", True)

            Case "Package Wise Purchase Report".ToUpper
                Dim argfrm As New frmRptSalesPkgwise
                OpenForms(argfrm, "Package Wise Purchase Report", True)

            Case "BRAND WISE REPORT"
                Dim argFrm As New frmRptBrandWise
                OpenForms(argFrm)

            Case Else
                Call OpenGeneralForm(argMenu)
        End Select

    End Sub

    Private Sub UtilityClick(argMenu As String)
        strMnuName = argMenu

        Dim tabshown, tabhidden As New TabControl
        Dim AllTabs As List(Of TabPage) = New List(Of TabPage)
        Dim tab As New C1.Win.C1Command.C1DockingTabPage


        'for fixed forms
        Dim intDocID As Int32 = clsSvt.getDataLongValue("select DOCID from MStMenu where MenuItemCaption = '" & strMnuName & "'", 0, False)

        If dockTabMain.Visible <> True Then dockTabMain.Visible = True
        For i = 0 To dockTabMain.TabPages.Count - 1
            If dockTabMain.TabPages(i).Text = argMenu Then
                dockTabMain.TabPages(i).Show()
                Exit Sub
            End If
        Next

        Select Case UCase(argMenu)
            'Accounting Reports
            '-----------------------------------------------------------------------------
            Case "Replace and Remove Duplicate items".ToUpper
                Dim argfrm As New frmTrnRemoveDuplicates
                OpenForms(argfrm)
                Exit Sub
            Case "MERGE ITEMS"
                Dim argfrm As New frmUtlMergeItems
                OpenForms(argfrm)
            Case "DATA PORT TO CLOUD (203)"
                Dim argfrm As New frmUtlExportStkWeb
                gblnAuto = False
                OpenForms(argfrm)
            Case "COMPARE STOCK"
                Dim argfrm As New frmUtlCompareStk
                OpenForms(argfrm)

            Case "ADMIN AUDIT LOG"
                Dim argfrm As New frmRptAdminAuditLog
                OpenForms(argfrm, "ADMIN Audit Log")

            Case "BILL WISE AGEING"
                Dim argfrm As New FrmRptAccAgeBillWise
                OpenForms(argfrm)
            Case "GENERAL LEDGER"
                Dim argfrm As New FrmRptAccGL
                OpenForms(argfrm)
            Case "PARTY WISE OUTSTANDING"
                Dim argfrm As New frmRptAccOutstanding
                OpenForms(argfrm)
            Case "BILL WISE OUTSTANDING"
                Dim argfrm As New FrmRptAccOSBillWise
                OpenForms(argfrm)

            Case "DATA EXPORT To Web"
                Dim argfrm As New frmUtlExportStkWeb
                OpenForms(argfrm)


            '-----------------------------------------------------------------------------

            Case "BULK MASTER UPDATE", "BULK UPDATE"
                Dim argfrm As New frmUtlMstTrnBulkUpdate
                OpenForms(argfrm)

            Case "LABEL PRINTING"
                Dim argfrm As New frmRptLblPrint
                OpenForms(argfrm)

            Case "SECURITY"
                Dim argfrm As New frmMstUSer
                argfrm.initForm(intDocID, "MstUSer", "UserID", "USerCode", "UserCode", "UserMaster", "User Code", False)
                OpenForms(argfrm)

            Case "CHANGE LOGIN"
                Dim argfrm As New frmLogin
                frmLogin.ShowDialog()

            Case "IMPORT FROM EXCEL"
                Dim argfrm As New frmUtlImportXL
                OpenForms(argfrm)

            Case "ABOUT"
                'Dim argfrm As New frmKeyReg
                'argfrm.StartPosition = FormStartPosition.CenterScreen
                'argfrm.ShowDialog()

                'frmUtlAbout.StartPosition = FormStartPosition.CenterScreen
                'frmUtlAbout.ShowDialog()

                Dim intDocIDNew As Int32 = clsSvt.getDataLongValue("Select DocID From UtlTransactions Where DocName = 'Sales Invoice'", 0, False)
                Dim objfrm As Object
                objfrm = New frmTrnPosInv
                objfrm.initForm(intDocIDNew)
                If Not objfrm.blnSalesCounter Then
                    Exit Sub
                End If
                OpenForms(objfrm, "Sales Invoice")
                objfrm.txtScan.Focus()
                pnlProject.Panels(0).Visible = False


            Case "KEY GENERATION"
                Dim argfrm As New frmKeyReg
                argfrm.StartPosition = FormStartPosition.CenterScreen
                argfrm.ShowDialog()
                Exit Sub

            Case "AUTO E-MAIL SCHEDULER"
                Dim argfrm As New frmUtlAutoEmailSch
                OpenForms(argfrm)


            Case "DATABASE ARCHIVE"
                Dim argfrm As New frmDBCreation
                OpenForms(argfrm)

            Case "DASHBOARD"
                'Try
                '    Dim strDashBrd As String = clsSvt.getDataStringValue("Select DashBoardLink from MstComp", "", False)
                '    If strDashBrd <> "" Then

                '        Process.Start(strDashBrd)
                '    End If
                'Catch ex As Exception
                '    MsgBox("Page Not Found.", vbInformation, Me.Text)
                'End Try
                Dim argfrm As New frmUtlDashboard
                OpenForms(argfrm)
                Exit Sub


            Case "HELP LINK"


                Try
                    Process.Start("http://47.23.106.203/vLQMStips/")
                Catch ex As Exception
                    MsgBox("Page not Found.", vbInformation, Me.Text)
                End Try
                Exit Sub

            Case "HELP DOC"
                Try
                    If File.Exists(gstrRptPath & "\HelpTips\ToolTipsShortcuts.xlsx") Then
                        Process.Start(gstrRptPath & "\HelpTips\ToolTipsShortcuts.xlsx")
                    Else
                        MsgBox("No help document....", MsgBoxStyle.Information, Me.Text)
                    End If
                Catch ex As Exception
                    MsgBox("ToolTipsShortcuts not Found.", vbInformation, Me.Text)
                End Try
                Exit Sub

            Case "GLOBAL SETTINGS..."
                Dim argfrm As New FrmUtlGSettings
                OpenForms(argfrm, "GLOBAL SETTINGS...", True)

            Case "AUTHORISE DOCUMENTS"
                Dim argfrm As New frmUtlAuth
                OpenForms(argfrm)

            Case "CALCULATOR"
                System.Diagnostics.Process.Start("calc.exe")

            'Case "Create &User Rights..."
            '    Dim argfrm As New frmMstUser_Old
            '    UtilityForms(argfrm)

            Case "DATABASE CONNECTIVITY"
                Dim argfrm As New frmUtlDBConnection
                argfrm.ShowDialog()

            Case "IMPORT DATA FROM EXCEL"
                Dim argfrm As New frmUtlImportXL
                OpenForms(argfrm)

            Case "SHORTCLOSE DOCUMENTS"
                Dim argfrm As New frmUtlShortclose
                OpenForms(argfrm)

            Case "REMOVE DUPLICATE INVOICE"
                Dim argfrm As New frmUtlDelDupInvoice
                OpenForms(argfrm)

            Case "IMPORT LEGACY DATA"
                Dim argfrm As New frmUtlImportLegacyData
                OpenForms(argfrm)

            Case "HELP"
                'OpenForms(argfrm)

            'Case "GENERAL ATTACHMENTS"
            '    Dim argfrm As New frmUtlGeneralAttach
            '    OpenForms(argfrm)

            Case "AUDIT LOG"
                Dim argfrm As New frmUtlRecAuditLog
                OpenForms(argfrm)

            'for audit track
            Case "AUDITLOG SYNC"
                Dim argfrm As New frmUtlAuditLogSync
                OpenForms(argfrm)
            Case "SPLIT DATA"
                If clsSvt.getDataStringValue("Select AllowSplit from UtlMisc", "", False) = "N" Then
                    MsgBox("Split Data is NOT allowed for this database.", MsgBoxStyle.Critical, Me.Text)
                    Exit Sub
                End If
                frmUtlSplitData.ShowDialog()
                Exit Sub

            Case "BULK UPDATE"
                Dim argfrm As New frmUtlBulkUpdate
                OpenForms(argfrm)

            Case "SQL QUERY EXECUTION"
                Dim argfrm As New frmUtlSQLQueryExec
                OpenForms(argfrm)

            Case "RE WRITE STOCK"
                frmStkRewrite.initScreen("FULL")
                frmStkRewrite.ShowDialog()
                Exit Sub

            Case "DOWNLOAD LATEST EXE"
                With New frmUpdateExe
                    .Show()
                    .Refresh()
                    .chkExeVersion()
                End With
                Exit Sub

            Case "SPLIT STOCK"
                frmUtlSplitData.ShowDialog()
                Exit Sub

            Case "SQL SAVED QUERY"
                Dim argfrm As New frmRptQuery
                OpenForms(argfrm)

            Case "STOCK TRAN MGNT"
                Dim argfrm As New frmUtlStk
                OpenForms(argfrm)

            Case "SHORTCLOSE DOCUMENTS"
                Dim argfrm As New frmUtlShortclose
                OpenForms(argfrm)

            Case "POST DOC ENTRY"
                Dim argfrm As New frmUtlPostDocEntry
                OpenForms(argfrm)

            Case "SHARED DOCUMENTS"
                Dim argfrm As New frmUtlSharedDocuments
                OpenForms(argfrm)

            Case "SCREEN DOCS"
                Dim argfrm As New frmUtlScreenDocs
                If dockTabMain.TabPages.Count > 0 Then
                    argfrm.argFormCaption = dockTabMain.SelectedTab.CaptionText
                Else
                    argfrm.argFormCaption = Me.Text
                End If
                argfrm.StartPosition = FormStartPosition.CenterScreen
                argfrm.ShowDialog()
            Case "EXIT"
                blnAppStarted = False
                Call frmMain_FormClosing(Nothing, Nothing)
                End

            Case "TEST"
                Me.Close()
            Case Else
                Call OpenGeneralForm(argMenu)
        End Select
    End Sub

    Private Sub txtSearch_KeyUp(sender As Object, e As KeyEventArgs) Handles txtSearch.KeyUp
        If e.Shift And e.Control And e.KeyCode = Keys.F11 Then
            frmUtlSQLQueryExec.ShowDialog()
            Exit Sub
        End If

        If txtSearch.Text = String.Empty Then
            If e.KeyCode = Keys.Escape Then
                lblSearchImage.Visible = True
                lblSearchClose.Visible = False
                vsfSearch.Visible = False
                txtSearch.Text = ""
                txtSearch.Focus()
            End If
            Exit Sub
        End If

        Dim strSQL As String, strCond As String = ""



        If e.KeyCode = Keys.Return Then
            If vsfSearch.Rows.Count <= 1 Then Exit Sub
            vsfSearch.Row = 1
            Call vsfSearchSelectedItem()
            Exit Sub
        End If

        txtSearch.Text = Replace(txtSearch.Text, "'", "`")

        If e.KeyCode = Keys.Escape Then
            txtSearch.Text = ""
            Call setSearchGridHt("")
            Exit Sub
        End If

        If Len(txtSearch.Text) < 1 And e.KeyCode <> Keys.Down Then
            Call setSearchGridHt("")
            Exit Sub
        End If

        If e.KeyCode = Keys.Down Then
            vsfSearch.Focus()
        End If

        vsfSearch.DataSource = Nothing

        Call setSearchGridHt("FULL")
        Dim dt As DataTable

        strSQL = "Select distinct MenuItemCaption  as MenuList, convert(varchar,MenuItemID),SCMenuName as ShortCut from mstmenu where menuitemcaption <> '-'"
        strSQL &= " and (MenuItemCaption like '%" & txtSearch.Text & "%'"
        strSQL &= " or SCMenuName = '" & txtSearch.Text & "')"
        strSQL &= " and MenuShow <> 'N'"
        strSQL &= " order by menuitemcaption"

        dt = clsSvt.getDATATable(strSQL)
        Dim dv As DataView = New DataView(dt)
        vsfSearch.DataSource = dv
        vsfSearch.Refresh()
        vsfSearch.Cols(1).Visible = False
        vsfSearch.Cols(0).Width = 250
        vsfSearch.ScrollBars = ScrollBars.Both
    End Sub

    Private Sub vsfSearchSelectedItem()
        Dim intItemtype As Integer
        Dim strsQL As String

        Dim nodes As TreeNode() = tvMain.Nodes.Find("K" & Trim(vsfSearch.GetData(vsfSearch.Row, 1)).ToString, True)
        For Each node In nodes
            If node.Tag = "NO" Then
                MsgBox("ACCESS DENIED for " & vsfSearch.GetData(vsfSearch.Row, 0), MsgBoxStyle.Critical, "Rights not alloted to Menu " & vsfSearch.GetData(vsfSearch.Row, 0))
                Exit Sub
            End If
        Next

        strsQL = "select menuitemtype from MstMenu where MenuItemCaption = '" & vsfSearch.GetData(vsfSearch.Row, 0) & "'"
        intItemtype = clsSvt.getDataLongValue(strsQL, 0, False)

        Call setSearchGridHt("")
        txtSearch.Text = ""
        txtSearch.Focus()

        If dockTabMain.Visible <> True And intItemtype > 0 Then dockTabMain.Visible = True
        For i = 0 To dockTabMain.TabPages.Count - 1
            If dockTabMain.TabPages(i).Text = vsfSearch.GetData(vsfSearch.Row, 0) Then
                dockTabMain.TabPages(i).Show()
                Exit Sub
            End If
        Next

        Select Case intItemtype
            Case MnuNode.DashBoard
                Call Dashboardclick(vsfSearch.GetData(vsfSearch.Row, 0))
            Case MnuNode.Masters
                Call MasterClick(vsfSearch.GetData(vsfSearch.Row, 0))
            Case MnuNode.Transaction
                Call TransactionsClick(vsfSearch.GetData(vsfSearch.Row, 0))
            Case MnuNode.Vouchers
                Call VoucherClick(vsfSearch.GetData(vsfSearch.Row, 0))
            Case MnuNode.Reports
                Call ReportsClick(vsfSearch.GetData(vsfSearch.Row, 0))
            Case MnuNode.AcReports

            Case MnuNode.Tools
                Call UtilityClick(vsfSearch.GetData(vsfSearch.Row, 0))
            Case MnuNode.Utilities
                Call UtilityClick(vsfSearch.GetData(vsfSearch.Row, 0))

        End Select
    End Sub

    Private Sub txtSearch_TextChanged(sender As Object, e As EventArgs) Handles txtSearch.TextChanged
        If Trim(txtSearch.Text) = "" Then
            lblSearchImage.Visible = True
            lblSearchClose.Visible = False
        Else
            lblSearchImage.Visible = False
            lblSearchClose.Visible = True
        End If
    End Sub

    Private Sub lblSearchImage_Click(sender As Object, e As EventArgs) Handles lblSearchImage.Click
        vsfSearch.Focus()
        vsfSearch.DataSource = Nothing

        Call setSearchGridHt("FULL")
        Dim dt As DataTable

        strSQL = "Select distinct menuitemcaption  as MenuList, SCMenuName as [Shortcut Key] from mstmenu where menuitemcaption <> '-'"
        strSQL &= " and (menuitemcaption like '%" & txtSearch.Text & "%'"
        strSQL &= " or SCMenuName = '" & txtSearch.Text & "')"
        strSQL &= " and MenuShow <> 'N'"
        strSQL &= " order by menuitemcaption"

        dt = clsSvt.getDATATable(strSQL)
        vsfSearch.DataSource = dt
        vsfSearch.Refresh()
        vsfSearch.ScrollBars = ScrollBars.Both
        lblSearchImage.Visible = False
        lblSearchClose.Visible = True
    End Sub
    Private Sub setSearchGridHt(ByVal argFlag As String)
        If argFlag = "FULL" Then
            vsfSearch.Visible = True
            vsfSearch.BringToFront()
        Else
            vsfSearch.Visible = False
        End If

    End Sub

    Private Sub txtSearch_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtSearch.KeyPress
        If e.KeyChar = "'" Then
            e.Handled = True
        End If
    End Sub

    Private Sub vsfsearch_BeforeEdit(sender As Object, e As RowColEventArgs) Handles vsfSearch.BeforeEdit
        e.Cancel = True
    End Sub

    Private Sub vsfsearch_KeyUp(sender As Object, e As KeyEventArgs) Handles vsfSearch.KeyUp
        If e.KeyCode = Keys.Return Then
            Call vsfSearchSelectedItem()
            Exit Sub
        End If
        If e.KeyCode = Keys.Escape Then
            txtSearch.Text = ""
            Call setSearchGridHt("")
            Exit Sub
        End If
        If e.KeyCode = Keys.Enter Then
            Call vsfSearchSelectedItem()
        End If
    End Sub

    Private Sub vsfsearch_DoubleClick(sender As Object, e As EventArgs) Handles vsfSearch.DoubleClick
        Call vsfSearchSelectedItem()
    End Sub

    Private Sub lblSearchClose_Click(sender As Object, e As EventArgs) Handles lblSearchClose.Click
        lblSearchImage.Visible = True
        lblSearchClose.Visible = False
        vsfSearch.Visible = False
        txtSearch.Text = ""
        txtSearch.Focus()
    End Sub

    Public Sub GetUserImage()
        Dim strSql As String = ""
        Dim cmd As OleDb.OleDbCommand
        picLogin.Image = Nothing
        strSql = "select " & getImgSyntax("ISNULL", "UserImg") & " from MstUser where UserID = " & Val(gUserID)
        Try
            cmd = New OleDb.OleDbCommand(strSql, svtConn)
            Dim imageData As Byte() = DirectCast(cmd.ExecuteScalar(), Byte())
            If imageData.Length = 0 Then
                ' btnRmvImg.Visible = False
                'Exit Sub
            End If
            If Not imageData Is Nothing Then
                Using ms As New MemoryStream(imageData, 0, imageData.Length)
                    If imageData.Length > 0 Then
                        ms.Write(imageData, 0, imageData.Length)
                        picLogin.BackgroundImage = Image.FromStream(ms, True)
                        picLogin.SizeMode = PictureBoxSizeMode.StretchImage
                    Else
                        picLogin.Image = Nothing
                        picLogin.Image = vLQMS.My.Resources.user
                        picLogin.SizeMode = PictureBoxSizeMode.StretchImage
                    End If
                End Using
            End If
        Catch
        End Try
    End Sub

    Private Sub picLogin_Click(sender As Object, e As EventArgs) Handles picLogin.Click
        If dockTabMain.TabPages.Count > 0 Then
            MsgBox("Please close all open pages in application..", MsgBoxStyle.Information, Me.Text)
            Exit Sub
        End If

        With frmLogin
            .StartPosition = FormStartPosition.CenterScreen
            .blnAppIdle = False
            .ShowDialog()
        End With
    End Sub

    Private Sub dockTabMain_SelectedTabChanged(sender As Object, e As System.EventArgs) Handles dockTabMain.SelectedTabChanged
        If dockTabMain.TabPages.Count = 0 Then
            dockTabMain.Visible = False
        End If
        If dockTabMain.SelectedTab Is Nothing Then Exit Sub
        For Each frm In dockTabMain.SelectedTab.Controls
            Try
                If TypeOf frm Is Form Then
                    frm.WindowState = FormWindowState.Normal
                    frm.WindowState = FormWindowState.Maximized
                    Exit For
                End If
            Catch ex As Exception

            End Try

        Next

    End Sub

    Public Sub CloseSelectedTabPage(ByVal argFrm As Object)
        argFrm.Dispose()
        Dim tb As New C1.Win.C1Command.C1DockingTabPage()
        Dim dockTabMain As New C1.Win.C1Command.C1DockingTab()
        argFrm.Dispose()
        tb.Controls.Clear()
        If Not Me.dockTabMain.SelectedTab Is Nothing Then Me.dockTabMain.SelectedTab.Dispose()
        txtSearch.Focus()
    End Sub

    Public Sub OpenForms(ByVal argfrm As Form, Optional ByVal argFrmCaption As String = "", Optional ByVal ChkFormOpenStatus As Boolean = False, Optional blnOpenInNewTab As Boolean = False)
        argfrm.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        If ChkFormOpenStatus Then
            Dim strDummyMenu As String
            If argFrmCaption <> "" Then
                strDummyMenu = argFrmCaption
            Else
                strDummyMenu = strMnuName
            End If
            If dockTabMain.Visible <> True Then dockTabMain.Visible = True
            If Not blnOpenInNewTab Then
                For i = 0 To dockTabMain.TabPages.Count - 1
                    If dockTabMain.TabPages(i).Text = strDummyMenu Then
                        dockTabMain.TabPages(i).Show()
                        Exit Sub
                    End If
                Next
            End If
        End If
        Dim tb As New C1.Win.C1Command.C1DockingTabPage()
        If argFrmCaption <> "" Then
            tb.Text = argFrmCaption
        Else
            tb.Text = strMnuName
        End If
        tb.DockTo(dockTabMain)
        tb.Parent = dockTabMain
        tb.Anchor = (AnchorStyles.Bottom Or AnchorStyles.Top Or AnchorStyles.Right Or AnchorStyles.Left)
        '--------Changed
        'If argFrmCaption = "Role Master" Then
        '    If dockTabMain.TabPages.Count <= 2 Then
        '        Me.dockTabMain.TabPages.Add(tb)
        '    Else
        '        Me.dockTabMain.TabPages.Remove(tb)
        '        Exit Sub
        '    End If
        'End If

        'If argFrmCaption = "GLOBAL SETTINGS..." Then
        '    If dockTabMain.TabPages.Count < 2 Then
        '        Me.dockTabMain.TabPages.Add(tb)
        '    Else
        '        Me.dockTabMain.TabPages.Remove(tb)
        '        Exit Sub
        '    End If
        'End If
        '---------
        Me.dockTabMain.TabPages.Add(tb)
        Me.dockTabMain.SelectedTab = tb
        Me.dockTabMain.Visible = True
        argfrm.Anchor = (AnchorStyles.Bottom Or AnchorStyles.Top Or AnchorStyles.Right Or AnchorStyles.Left)
        argfrm.TopLevel = False
        argfrm.Dock = DockStyle.Fill
        argfrm.WindowState = FormWindowState.Normal
        argfrm.WindowState = FormWindowState.Maximized
        argfrm.Top = 0
        argfrm.Left = 0
        tb.Controls.Add(argfrm)
        tb.Dock = DockStyle.Fill
        argfrm.Show()
    End Sub

    Private Sub tvMain_NodeMouseClick(sender As Object, e As System.Windows.Forms.TreeNodeMouseClickEventArgs) Handles tvMain.NodeMouseClick
        'MsgBox("Node Expand")
        If e.Button = MouseButtons.Right Then Exit Sub
        Dim strMenuKey As String = ""
        Dim rstUserRights As OleDb.OleDbDataReader
        Dim strQuery As String
        'MsgBox("Dock Checking")
        If dockTabMain.TabPages.Count = 20 Then
            MsgBox("Maximum Tabs Reached !!! Close a tab", vbInformation, Me.Text)
            Exit Sub
        End If
        'MsgBox("Exit Checking")
        If e.Node.Text = "Exit - [Alt + X]" Then
            blnAppStarted = False
            Call frmMain_FormClosing(sender, Nothing)
        End If
        'MsgBox("Node Checking")
        If e.Node.Text <> "" Then
            'MsgBox(e.Node.Text)
            If Not gUserCode = "ADMIN" Then
                strQuery = "Select MstMenu.MenuItemID,MenuItemCaption"
                strQuery = strQuery & " From " & IIf(gblnUserBased, "MstUserRights", "MstRoleRights") & ", MstMenu"
                strQuery = strQuery & " Where LTRIM(RTRIM(" & IIf(gblnUserBased, "UserID", "RoleID") & ")) = " & IIf(gblnUserBased, gUserID, gRoleID) & " And "
                strQuery = strQuery & " " & IIf(gblnUserBased, "MstUserRights", "MstRoleRights") & ".MenuItemID=MstMenu.MenuItemID AND "
                strQuery = strQuery & " " & IIf(gblnUserBased, "UserRights", "RoleRights") & " = '000000000000'"
                strQuery = strQuery & " Union"
                strQuery = strQuery & " Select MstMenu.MenuItemID,MenuItemCaption"
                strQuery = strQuery & " From MstMenu"
                strQuery = strQuery & " Where 1 = 1"
                strQuery = strQuery & " and MstMenu.MenuItemID NOT in"
                strQuery = strQuery & " (select menuitemid from " & IIf(gblnUserBased, "MstUserRights", "MstRoleRights") & " "
                strQuery = strQuery & " where LTRIM(RTRIM(" & IIf(gblnUserBased, "UserID", "RoleID") & ")) = " & IIf(gblnUserBased, gUserID, gRoleID) & ")"
                'MsgBox(strQuery)
                'Dim o As New DataObject
                'o.SetText(strQuery)
                'Clipboard.SetDataObject(o, True)

                rstUserRights = clsSvt.getDataReaderFromSQL(strQuery)
                With rstUserRights
                    Do While .Read
                        strMenuKey = "K" & !MenuItemID
                        Dim nodes As TreeNode() = tvMain.Nodes.Find(strMenuKey, True)
                        For Each node In nodes
                            node.ForeColor = Color.Gray
                            node.Tag = "NO"
                        Next
                    Loop
                    .Close()
                End With
                'MsgBox("query executed")
                If e.Node.Tag = "NO" Then
                    MsgBox("ACCESS DENIED for " & e.Node.Text, MsgBoxStyle.Critical, "Rights not alloted to Menu " & e.Node.Text)
                    Exit Sub
                Else
                    'tvMain.SelectedNode.ForeColor = Color.White
                    'tvMain.SelectedNode.Tag = "YES"
                    'MsgBox("Calling Node Function")
                    NodeSelection(e.Node.Text)
                End If
                If e.Node.Text = "Exit - [Alt + X]" Then
                    blnAppStarted = False
                    Call frmMain_FormClosing(sender, Nothing)
                End If
                'NodeSelection(e.Node.Text)
            End If
            'MsgBox("Calling Node Function From Admin")
            NodeSelection(e.Node.Text)
        End If
    End Sub

    Private Sub NodeSelection(ByVal strMenu As String)
        'MsgBox("Entered Node Selection")
        Dim nv As String, intItemtype As Long
        Dim i As Integer, strMenuHdr As String = ""
        Dim strDashBrd As String
        nv = strMenu

        'MsgBox("Menu Type")
        strSQL = "select menuitemtype from MstMenu where menuitemcaption = '" & nv & "'"
        intItemtype = clsSvt.getDataLongValue(strSQL, -1, False)

        'MsgBox("Checking Tab")
        If intItemtype < 0 Then
        Else
            If dockTabMain.Visible <> True And intItemtype > 0 Then dockTabMain.Visible = True
            For i = 0 To dockTabMain.TabPages.Count - 1
                If dockTabMain.TabPages(i).Text = nv Then
                    dockTabMain.TabPages(i).Show()
                    Exit Sub
                End If
            Next
        End If

        'MsgBox("Item Type Selection")
        Select Case intItemtype
            Case 0
                'Dim argfrm As New frmUtlDashboard
                'Dim pnlForm As New Panel()
                'Dim tb As New C1.Win.C1Command.C1DockingTabPage()
                'tb.Text = "Dashboard"   'strMnuName
                'tb.DockTo(dockTabMain)
                'tb.Parent = dockTabMain
                'tb.Anchor = (AnchorStyles.Bottom Or AnchorStyles.Top Or AnchorStyles.Right Or AnchorStyles.Left)
                'Me.dockTabMain.TabPages.Add(tb)
                'Me.dockTabMain.SelectedTab = tb
                'argfrm.Anchor = (AnchorStyles.Bottom Or AnchorStyles.Top Or AnchorStyles.Right Or AnchorStyles.Left)
                'argfrm.TopLevel = False
                'argfrm.Dock = DockStyle.Fill
                'argfrm.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
                'tb.Controls.Add(argfrm)
                'argfrm.Show()
                Call Dashboardclick(nv)
            Case 2
                'MsgBox("Trn Click")
                Call TransactionsClick(nv)
            Case 1
                Call MasterClick(nv)
            Case 3
                Call VoucherClick(nv)
            Case 4
                Call ReportsClick(nv)
            Case Else
                Call UtilityClick(nv)
        End Select
        If dockTabMain.Visible = True And dockTabMain.TabPages.Count <= 0 Then dockTabMain.Visible = False

    End Sub

    'Private Sub tvMain_KeyUp(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles tvMain.KeyUp
    'If e.KeyCode = Keys.Enter Then
    'If dockTabMain.TabPages.Count = 20 Then
    '    MsgBox("Maximum Tabs Reached !!! Close a tab", vbInformation, Me.Text)
    '    Exit Sub
    'End If
    'If Not tvMain.SelectedNode Is Nothing Then
    '    If Not gUserCode = "ADMIN" Then
    '        If tvMain.SelectedNode.Tag = "NO" Then
    '            MsgBox("ACCESS DENIED for " & tvMain.SelectedNode.Text, MsgBoxStyle.Critical, "Rights not alloted to Menu " & tvMain.SelectedNode.Text)
    '            Exit Sub
    '        End If
    '    Else
    '        NodeSelection(tvMain.SelectedNode.Text)
    '    End If
    '    'NodeSelection(tvMain.SelectedNode.Text)
    'End If
    'Call tvMain_NodeMouseClick(sender, e)
    'End If
    'End Sub

    Public Sub SetMainFormColor()
        tvMain.ForeColor = gMainForeColor
        tvMain.BackColor = gMainBackColor
        tvMain.Font = gMainFontName
        lblUsername.ForeColor = gMainForeColor
        lblUsername.Font = gMainFontName
        pnlMain.BackColor = gMainBackColor
    End Sub

    Private Sub btCalExit_Click(sender As Object, e As System.Windows.Forms.FormClosingEventArgs) Handles btCalExit.Click
        Dim strLOGFile As String = ""
        If Not blnAppStarted Then
            If MsgBox("Exit from software? ", vbQuestion + vbYesNo + vbDefaultButton1, "Softvent..." & gSettings.gCompName) = vbNo Then
                e.Cancel = True
                Exit Sub
            End If
        End If

        If MdiChildren.Count > 0 Then
            For Each child As Form In MdiChildren
                child.Close()
            Next
        End If

        Call saveGridToINI()
        'Hotkey.unregisterHotkeys(Me)
        End
    End Sub

    Private Sub getValuesFromGSettings()
        gIntDailyLimit = getGsettings("Company", "DailyLimit", 0)
        gIntLimit = getGsettings("Company", "TotLimit", 0)
        gIntMsgLen = getGsettings("Company", "SingleMsgLength", 0)
        gFormColor = getGsettings("Company", "FormBGColor", "")
        gCompCode = getGsettings("Company", "CompCode", "")
        'gSMSU = getGsettings("Company", "Username", "")
        'gSMSP = getGsettings("Company", "Password", "")
        'gSENDER = getGsettings("Company", "SENDER", "")
        gSMSURL = getGsettings("Company", "SMSURL", 0)
        gintFontSize = getGsettings("Company", "FontSize", 8)

        gSmtpMailID = getGsettings("SMTP", "SMTPMailID", "")
        gSmtpMailPwd = getGsettings("SMTP", "SMTPMailPwd")
        gSmtpHostName = getGsettings("SMTP", "SMTPHostName")
        gSmtpServerPort = getGsettings("SMTP", "SMTPServerPort")
        gSmtpEnableSSL = getGsettings("Company", "EnableSSL")
        If getGsettings("Inventory", "AllowNeg") = "Y" Then
            gBlnAllowNegativeStock = True
        End If
    End Sub

    Private Sub pnlForm_Resize(sender As Object, e As EventArgs) Handles pnlForm.Resize

        For Each dPG As Control In dockTabMain.Controls
            For Each frm In dPG.Controls
                Try
                    If TypeOf frm Is Form Then
                        frm.WindowState = FormWindowState.Normal
                        frm.WindowState = FormWindowState.Maximized
                        Exit For
                    End If
                Catch ex As Exception

                End Try
            Next
        Next
    End Sub

    'Private Sub RemoveOldXML()
    '    Try
    '        Dim DirList As New IO.DirectoryInfo(gstrAutoSave)
    '        For Each SubDirList() As ArrayList In Directory.GetDirectories

    '        Next
    '    Catch ex As Exception

    '    End Try
    'End Sub

    'Private Sub RemoveOldXMLFiles(ByVal argDirList As String)
    '    Dim dirList As New IO.DirectoryInfo(argDirList)
    '    For Each file As IO.FileInfo In Directory.GetFiles
    '        If (Now - file.CreationTime).Days > 7 Then file.Delete()
    '    Next
    'End Sub

#Region "Quick Launch shortcut"
    'Protected Overrides Sub WndProc(ByRef m As System.Windows.Forms.Message)
    '    If m.Msg = Hotkey.WM_HOTKEY Then
    '        Hotkey.handleHotKeyEvent(m.WParam)
    '    End If
    '    MyBase.WndProc(m)
    'End Sub
#End Region
    Private Sub pbNotify_Click(sender As Object, e As EventArgs) Handles pbNotify.Click
        Dim argFrm As New frmUtlNotification
        argFrm.ShowDialog()
    End Sub

    Private Sub lblComp_Click(sender As Object, e As EventArgs) Handles lblComp.Click
        If dockTabMain.TabPages.Count > 0 Then
            MsgBox("Please close all open pages in application..", MsgBoxStyle.Information, Me.Text)
            Exit Sub
        End If
        frmMstSelComp.ShowDialog()
    End Sub

#Region "check Flds present"
    Private Sub addNewFields()

        If Not fnFldPresent("PurBill4Deci", "UtlMisc") Then
            clsSvt.execSQLStatement("Alter table UtlMisc Add PurBill4Deci char(1) not null default('N')")
        End If

        If clsSvt.getDataLongValue("Select Count(*) From UtlMisc where PurBill4Deci = 'Y'", 0, False) > 0 Then
            gBlnPurBill4Deci = True
        End If

        If Not fnMenuPresent("Bulk Master Update") Then
            Call fnCreateMenu("Bulk Master Update", "Bulk Master Update", MnuNode.Utilities)
        End If
        If Not fnTablePresent("UtlMstTrnBulkUpdate") Then
            clsSvt.execSQLStatement("CREATE TABLE [dbo].[UtlMstTrnBulkUpdate](
	[RowID] [int] IDENTITY(1,1) NOT NULL,
	[DocName] [varchar](250) NOT NULL,
	[TblName] [varchar](250) NOT NULL,
	[KeyFields] [varchar](250) NOT NULL,
	[DispCols] [varchar](max) NOT NULL,
	[FilterCols] [varchar](max) NOT NULL,
	[StorageCols] [varchar](max) NOT NULL,
 CONSTRAINT [PK_UtlMstTrnBulkUpdate] PRIMARY KEY CLUSTERED 
(
	[DocName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]")
        End If

        If gBlnPurBill4Deci Then
            clsSvt.execSQLStatement("ALTER TABLE TrnDtlPB ALTER COLUMN PBRate Numeric(18,4)")
            clsSvt.execSQLStatement("ALTER TABLE UtlStockList ALTER COLUMN Rate Numeric(18,4)")
            clsSvt.execSQLStatement("ALTER TABLE MstItem ALTER COLUMN PurRate Numeric(18,4)")
            clsSvt.execSQLStatement("update u set FldNumDecimals = 4 From UtlDocDsgrFlds u, UtlTransactions t Where u.DocID = t.DocID and t.DocName = 'Item Master' and u.FldCode = 'PURRATE'")
            clsSvt.execSQLStatement("update u set FldNumDecimals = 4 From UtlDocDsgrFlds u, UtlTransactions t Where u.DocID = t.DocID and t.DocName = 'Purchase Bill' and u.FldCode = 'PBRATE'")
        End If
        Exit Sub
        Dim strSQL As String = ""
        Dim blnFound As Boolean = False


        If Not fnFldPresent("AROEDPER", "MstItem") Then
            clsSvt.execSQLStatement("alter table MstItem add AROEDPER  numeric(19, 2) not null default(0)")
        End If

        If Not fnFldPresent("AROEDPERBOT", "TrnDtlPB") Then

            clsSvt.execSQLStatement("alter table TrnDtlPB add AROEDPERBOT  numeric(19, 2) not null default(0)")
        End If


        If Not fnMenuPresent("Database Archive") Then
            Call fnCreateMenu("Database Archive", "", MnuNode.Utilities)
        End If

        If Not fnTablePresent("TrnDtlPB_Archive") Then
            clsSvt.execSQLStatement("CREATE TABLE [dbo].[TrnDtlPB_Archive](
	[PBID] [int] NOT NULL,
	[PBDate] [datetime] NOT NULL,
	[ItemID] [int] NOT NULL,
	[PBRate] [numeric](18, 2) NOT NULL,
[PBQty] [numeric](18, 2) NOT NULL,
[NetAmt] [numeric](18, 2) NOT NULL
) ON [PRIMARY]")
        End If

        If Not fnTablePresent("vwPBItemLastPrice") Then
            clsSvt.execSQLStatement("Create View vwPBItemLastPrice as
Select Row_Number() Over(Partition By ItemID Order By PBDate desc, PBID desc) as Slno, ItemID, PBRate , PBDate, NetAmt, PBQty
From (
Select hs.PBID, hs.PBDate, ItemID, PBRate, NetAmt, PBQty from TrnDtlPB s, trnHdrPB hs where s.pbid = hs.pbid 
Union All
Select PBID, PBDate, ItemID, PBRate, NetAmt, PBQty From TrnDtlPB_Archive
) v")
        End If

        Dim intSinvDocID As Integer = clsSvt.getDataLongValue("Select DocId from UtlTransactions where HdrTblName='TRNHDRSINV'", 0, False)
        Call clsSvt.getDataStringValue("Select BrandID From MstBrand Where BrandID = 0", "", blnFound)
        If Not blnFound Then
            strSQL = "insert into MstBrand(brandid, brandcode, BRANDNAME,addedon) values (0,'-','-',getdate())"
            clsSvt.execSQLStatement(strSQL)
        End If

        Call clsSvt.getDataStringValue("Select setName From utlgsettings Where setName = 'APPROVEURL'", "", blnFound)
        If Not blnFound Then
            strSQL = "insert into UtlGSettings(setGroup,SetName, SetValue,Setcaption,Remarks,HideSetting)"
            strSQL = strSQL & "values('COMPANY','APPROVEURL','http://192.168.1.26/vLQMSSSPLSecurity/index.aspx?invid=<<INVID>>','Bill Security Confirmation URL','Enter URL to save QR Image for security check','N')"
            clsSvt.execSQLStatement(strSQL)
        End If

        If Not fnFldPresent("QRImageCheck", "TrnHdrSINV") Then
            clsSvt.execSQLStatement("alter table TrnHdrSINV add QRImageCheck varchar(250) not null default(0)")
        End If

        If intSinvDocID > 0 Then
            If Not fnFldPresent("SECCHK", "TrnHdrSINV") Then
                clsSvt.execSQLStatement("alter table TrnHdrSINV add SECCHK char(1) not null default('N')")

                clsSvt.execSQLStatement("INSERT [dbo].[UtlDocDsgrFlds] ([DocID],  [FldSection], [FldCode], [FldCaption], [FldDataType], [FldDataWidth], [FldBlnReadOnly], [FLdBlnHide], [FldBlnMandatory], [FldUnique], [FldUseDefault], [FldRowClrFontBold], [FldRowClrForeColour], [FldBlnComboData], [FldBlnRefill], [FldOrderBy], [FldFormula], [FldBlnDispOnly], [FldPickUP], [FldNumBlnAnyNumber], [FldNumBlnPercentage], [FldNumDecimals], [FldNumBlnAllowNegative], [FldNumBlnRange], [FldNumBlnRangeFromChk], [FldNumRangeFromOperator], [FldNumRangeFromValue], [FldNumBlnRangeToChk], [FldNumRangeToOperator], [FldNumRangeToValue], [FldBlnNumOrStringList], [FldNumStringListContent], [FldDtBlnAnyDate], [FldDtBlnWithinFinYear], [FldDtBlnRange], [FldDtBlnRangeFrom], [FldDtRangeFromOperator], [FldDtRangeFromValue], [FldDtBlnRangeTo], [FldDtRangeToOperator], [FldDtRangeToValue], [FldDtBlnAsEntered], [FldDtBlnFirstDOM], [FldDtBlnLastDOM], [FldTxtStringType], [FldTxtCase], [FldTxtBlnDoNotAllowChk], [FldTxtDoNotAllowChars], [FldTxtBlnMask], [FldTxtMaskChar], [FldBlnRowClr], [FldBlnFontBold], [FldHideCompareVal], [FldHideOperator], [FldHideWithVal], [FldMndtryCompareVal], [FldMndtryOperator], [FldMndtryWithVal], [FldROCompareVal], [FldROOperator], [FldROWithVal], [FldCmbTblName], [FldCmbStorageFldName], [FldCmbDisplayCols], [FldCmbAddnlCond], [FldDefStdDefault], [FldDefBlnAgnstCond], [FldDefCondList], [FldTxtBlnMultiLine], [FldPickUPAfterFld], [FldToolTip], [FldUniqueBasedOn], [FldTxtAlignment], [FldBlnStandard], [FldBlnLoadFrom], [FldLoadFldName], [FldClrBefHide], [DocValue], [FldLoadFromType], [BlnApportinate], [FldApportinate], [BasedOn], [FldFontName], [FldFontSize], [BlnCopyPreviousRowData], [SubSectionName], [PerAmount], [Separator], [PickFldsFromSrcDocQry], [BlnLoadColFrom], [FldLoadColFrom], [BlnTogHide], [FldBlnNewTrnCalc], [FldBlnChkDuplicate], [BlnTotalReq], [LoadPrevTrnData], [LoadPrevDataParty], [LoadPrevDataItem], [CSVFieldName], [RoundType], [PrevTrnFldName], [FldChkDocDate], [MultiAmtFld]) VALUES (" & intSinvDocID & ", N'Total', N'SECCHK', N'Security Check', N'List String', 1, N'N', N'Y', N'N', N'N', N'Y', N'N', N'-2147483633', N'N', N'N', (Select Max(FldOrderBy) from UtlDocDsgrFlds where DOCID=" & intSinvDocID & " and FldSection='Total'), N'', N'N', N'', N'Y', N'N', 0, N'N', N'N', N'N', N'          ', N'', N'N', N'          ', N'', N'S', N'Y|N', N'Y', N'N', N'N', N'N', N'          ', N'', N'N', N'', N'', N'Y', N'N', N'N', N'ANY', N'ASENTERED', N'N', N'', N'N', N' ', N'N', N'N', N'', N'          ', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'N', N'N', N'', N'N', N'', N'', N'', N' ', N'N', N'N', N'', N'N', N'N', N' ', N'N', N'', N'', N'MS Sans Serif', CAST(8.00 AS Numeric(19, 2)), N'N', N'', N'N', N'', NULL, N'N', N'', N'N', N'N', N'N', N'N', N'N', N'N', N'N', N'', N'None', N'', N'N', N'N')")

                clsSvt.execSQLStatement("update UtlDocDsgrFlds set FldOrderBy=(Select max(fldorderby)+1 from UtlDocDsgrFlds where DocID=" & intSinvDocID & " and fldsection='Total') where DocID=" & intSinvDocID & " and fldsection='Total' and fldcode='INVVALUE'")

                'clsSvt.execSQLStatement("update UtlDocDsgrFlds set FldOrderBy=(Select fldorderby-1 from UtlDocDsgrFlds where DocID=" & intSinvDocID & " and fldsection='Total' and fldcode='INVVALUE') where DocID=1216 and fldsection='Total' and fldcode='SECCHK'")

            End If
        End If

        Call clsSvt.getDataStringValue("Select setName From utlgsettings Where setName = 'AUTOEMAILSRVR'", "", blnFound)
        If Not blnFound Then
            strSQL = "insert into UtlGSettings(setGroup,SetName, SetValue,Setcaption,Remarks,HideSetting)"
            strSQL = strSQL & " values('Auto Email Server','AUTOEMAILSRVR','OUTLOOK','Auto Email Server','Select the Auto email server OUTLOOK or SMTP ','N')"
            clsSvt.execSQLStatement(strSQL)
        End If

        Call clsSvt.getDataStringValue("Select setName From utlgsettings Where setName = 'BRANDREQ'", "", blnFound)
        If Not blnFound Then
            strSQL = "insert into UtlGSettings(setGroup,SetName, SetValue,Setcaption,Remarks,HideSetting)"
            strSQL = strSQL & " values('Inventory','BRANDREQ','N','BRAND REQUIRED','BRAND NAME REQUIRED OR NOT IN STOCK BALANCE','N')"
            clsSvt.execSQLStatement(strSQL)
        End If

        Call clsSvt.getDataStringValue("Select setName From utlgsettings Where setName = 'SPLITINV'", "", blnFound)
        If Not blnFound Then
            strSQL = "insert into UtlGSettings(setGroup,SetName, SetValue,Setcaption,Remarks,HideSetting)"
            strSQL = strSQL & "values('Invoice','SPLITINV','','Allow Split Invoice','Enter Y to Allow Split Invoice','N')"
            clsSvt.execSQLStatement(strSQL)
        End If

        If Not fnFldPresent("SplitInvRef", "TRNHDRSINV") Then
            strSQL = "Alter table trnHdrSinv add SplitInvRef varchar(50) not null default ''"
            clsSvt.execSQLStatement(strSQL)
            clsSvt.execSQLStatement("Update TrnHdrSINV set SplitInvRef  = ''")
        End If

        If Not fnFldPresent("QRImageCheck", "TRNHDRSINV") Then
            clsSvt.execSQLStatement("alter table TRNHDRSINV add QRImageCheck varchar(250) not null default(0)")
        End If

        If Not fnTablePresent("MstItemAudit") Then
            strSQL = "Create table MstItemAudit (ItemID int, ItemCode varchar(250), ItemName varchar(250),ISource varchar(50), OldMRP numeric(18,2), OldPurRate numeric(18,2), newMRP numeric(18,2), NewPurRate numeric(18,2), CSystemName varchar(250), CUserName varchar(50), cDateTime datetime not null default getdate())"
            clsSvt.execSQLStatement(strSQL)
        End If

        If Not fnFldPresent("FldBlnFinYrUpdate", "UtlMisc") Then
            Call clsSvt.execSQLStatement("Alter table UtlMisc add FldBlnFinYrUpdate char(1) not null default 'N'")
        End If

        'to update manually
        'update utlmisc set AdvancedADMLogin = 'Y'
        'update mstuser set admloginpwd = 'kZ1VKRSuZJw=' where userid = 1
        '
        '
        If Not fnFldPresent("AdvancedADMLogin", "UtlMisc") Then
            Call clsSvt.execSQLStatement("Alter table UtlMisc add AdvancedADMLogin char(1) not null default 'N'")
        End If

        If Not fnFldPresent("WebClientID", "UtlMisc") Then
            Call clsSvt.execSQLStatement("Alter table UtlMisc add WebClientID int not null default 0")
        End If

        If Not fnMenuPresent("ADMIN Audit Log") Then
            Call fnCreateMenu("ADMIN Audit Log", "", MnuNode.Utilities)
        End If

        If Not fnFldPresent("ADMLoginPwd", "MstUser") Then
            strSQL = "Alter table MstUser add ADMLoginPwd varchar(150) not null default ''"
            Call clsSvt.execSQLStatement(strSQL)
        End If
        If Not fnFldPresent("AdvancedADMLogin", "UtlMisc") Then
            strSQL = "Alter table UtlMisc add AdvancedADMLogin char(1) not null default 'N'"
            Call clsSvt.execSQLStatement(strSQL)
        End If
        If Not fnTablePresent("UtlAdmAudit") Then
            strSQL = "create table UtlAdmAudit( DocID int identity,ScreenName	varchar(150) not null default '', DAction varchar(10) not null default '', DocDate smalldatetime, DocDateTime	smalldatetime, DocSysName varchar(150) not null default '', SysUserName	varchar(150) not null default '', MainFldID int, MainFld	varchar(150) not null default '', EarlierData	varchar(max),ChangedData varchar(max))"
            Call clsSvt.execSQLStatement(strSQL)
        End If

        If Not fnTablePresent("UtlAdmAuditItem") Then
            strSQL = "create table UtlAdmAuditItem( DocID int , ItemID int, ItemCode varchar(200) , Itemname varchar(200) , ActionStatus varchar(20),  ItemDetails varchar(max))"
            Call clsSvt.execSQLStatement(strSQL)
        End If

        If Not fnTablePresent("UtlItemBulkUpdate") Then
            clsSvt.execSQLStatement("CREATE TABLE [dbo].[UtlItemBulkUpdate](
	[FieldName] [varchar](100) NOT NULL,
	[DataType] [varchar](50) NULL,
 CONSTRAINT [PK_UtlItemBulkUpdate] PRIMARY KEY CLUSTERED 
(
	[FieldName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]")

            clsSvt.execSQLStatement("Insert Into UtlItemBulkUpdate (FieldName, DataType)
Values ('BotPerCases','INTEGER'),
('HSNCode','TEXT'),
('ITEMSELL','NUMERIC'),
('LiqType','TEXT'),
('MRP','NUMERIC'),
('NOSELINVOICE','TEXT'),
('PKGITEM','TEXT'),
('PURRATE','NUMERIC'),
('ROL','NUMERIC'),
('ROQ','NUMERIC')")
        End If

        If Not fnTablePresent("TrnHdrPSE") Then
            clsSvt.execSQLStatement("CREATE TABLE [dbo].[TrnHdrPSE](
	[PSEID] [int] IDENTITY(1,1) NOT NULL,
	[PSEDATE] [datetime] NOT NULL,
	[SYSNAME] [varchar](50) NOT NULL,
	[USERNAME] [varchar](50) NOT NULL,
	[ENTRYTIME] [datetime] NOT NULL
) ON [PRIMARY]")

            clsSvt.execSQLStatement(" ALTER TABLE [dbo].[TrnHdrPSE] ADD  CONSTRAINT [DF_TrnHdrPSE_PSEDATE]  DEFAULT (CONVERT([date],getdate(),(102))) FOR [PSEDATE] ")

            clsSvt.execSQLStatement(" ALTER TABLE [dbo].[TrnHdrPSE] ADD  CONSTRAINT [DF_TrnHdrPSE_ENTRYTIME]  DEFAULT (getdate()) FOR [ENTRYTIME] ")

            clsSvt.execSQLStatement(" CREATE TABLE [dbo].[TrnDtlPSE](
	[PSEID] [int] NOT NULL,
	[Slno] [int] NOT NULL,
	[ItemID] [int] NOT NULL,
	[PRate] [numeric](18, 2) NOT NULL,
	[MRate] [numeric](18, 2) NOT NULL,
	[ClQty] [numeric](18, 2) NOT NULL,
	[QOH] [numeric](18, 2) NOT NULL,
	[DiffQty] [numeric](18, 2) NOT NULL,
	[PBNO] [varchar](50) NULL,
	[StkDedNO] [varchar](50) NULL,
 CONSTRAINT [PK_TrnDtlPSE] PRIMARY KEY CLUSTERED 
(
	[PSEID] ASC,
	[Slno] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] ")

            'clsSvt.execSQLStatement(" ")


            'clsSvt.execSQLStatement(" ")

            'clsSvt.execSQLStatement(" ")
        End If

        If Not fnFldPresent("GroupOrderBy", "MSTITEMGROUP") Then
            strSQL = "Alter table mstItemgroup add GroupOrderBy int not null default (1000)"
            Call clsSvt.execSQLStatement(strSQL)
        End If
        If Not fnFldPresent("PkgOrderBy", "MSTPKG") Then
            strSQL = "Alter table mstPKG add PKGOrderBy int not null default (1000)"
            Call clsSvt.execSQLStatement(strSQL)
        End If

        If Not fnMenuPresent("Physical Stock Entry") Then
            fnCreateMenu("Physical Stock Entry", "PSE", 2)
        End If

        If Not fnFldPresent("LiqItem", "MSTITEMGROUP") Then
            strSQL = "Alter table mstItemgroup add LiqItem char(1) not null default 'N'"
            Call clsSvt.execSQLStatement(strSQL)

            strSQL = "INSERT dbo.UtlDocDsgrFlds (DocID,  FldSection, FldCode, FldCaption, FldDataType, FldDataWidth, FldBlnReadOnly, FLdBlnHide, FldBlnMandatory, FldUnique, FldUseDefault, FldRowClrFontBold, FldRowClrForeColour, FldBlnComboData, FldBlnRefill, FldOrderBy, FldFormula, FldBlnDispOnly, FldPickUP, FldNumBlnAnyNumber, FldNumBlnPercentage, FldNumDecimals, FldNumBlnAllowNegative, FldNumBlnRange, FldNumBlnRangeFromChk, FldNumRangeFromOperator, FldNumRangeFromValue, FldNumBlnRangeToChk, FldNumRangeToOperator, FldNumRangeToValue, FldBlnNumOrStringList, FldNumStringListContent, FldDtBlnAnyDate, FldDtBlnWithinFinYear, FldDtBlnRange, FldDtBlnRangeFrom, FldDtRangeFromOperator, FldDtRangeFromValue, FldDtBlnRangeTo, FldDtRangeToOperator, FldDtRangeToValue, FldDtBlnAsEntered, FldDtBlnFirstDOM, FldDtBlnLastDOM, FldTxtStringType, FldTxtCase, FldTxtBlnDoNotAllowChk, FldTxtDoNotAllowChars, FldTxtBlnMask, FldTxtMaskChar, FldBlnRowClr, FldBlnFontBold, FldHideCompareVal, FldHideOperator, FldHideWithVal, FldMndtryCompareVal, FldMndtryOperator, FldMndtryWithVal, FldROCompareVal, FldROOperator, FldROWithVal, FldCmbTblName, FldCmbStorageFldName, FldCmbDisplayCols, FldCmbAddnlCond, FldDefStdDefault, FldDefBlnAgnstCond, FldDefCondList, FldTxtBlnMultiLine, FldPickUPAfterFld, FldToolTip, FldUniqueBasedOn, FldTxtAlignment, FldBlnStandard, FldBlnLoadFrom, FldLoadFldName, FldClrBefHide, DocValue, FldLoadFromType, BlnApportinate, FldApportinate, BasedOn, FldFontName, FldFontSize, BlnCopyPreviousRowData, SubSectionName, PerAmount, Separator, PickFldsFromSrcDocQry, BlnLoadColFrom, FldLoadColFrom, BlnTogHide, FldBlnNewTrnCalc, FldBlnChkDuplicate, BlnTotalReq, LoadPrevTrnData, LoadPrevDataParty, LoadPrevDataItem, CSVFieldName, RoundType, PrevTrnFldName, FldChkDocDate, MultiAmtFld) VALUES (1110,  N'Header', N'LiqItem', N'Liquor ItemGroup', N'List String', 1, N'N', N'N', N'Y', N'N', N'Y', N'N', N'-2147483633', N'N', N'N', 17, N'', N'N', N'', N'Y', N'N', 0, N'N', N'N', N'N', N'          ', N'', N'N', N'          ', N'', N'S', N'Y|N', N'Y', N'N', N'N', N'N', N'          ', N'', N'N', N'', N'', N'Y', N'N', N'N', N'ANY', N'ASENTERED', N'N', N'', N'N', N' ', N'N', N'N', N'', N'          ', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'N', N'N', N'', N'N', N'', N'', N'', N' ', N'N', N'N', N'', N'N', N'N', N' ', N'N', N'', N'', N'MS Sans Serif', CAST(8.00 AS Numeric(19, 2)), N'N', N'', N'N', N'', NULL, N'N', N'', N'N', N'N', N'N', N'N', N'N', N'N', N'N', N'', N'None', N'', N'N', N'N')"
            Call clsSvt.execSQLStatement(strSQL)
        End If

        If Not fnFldPresent("RAMT", "TRNDTLEXP") Then
            strSQL = "Alter table TrnDtlExp add RAMT Numeric(18,2) not null default 0"
            Call clsSvt.execSQLStatement(strSQL)
            Dim intExpID As Integer
            intExpID = clsSvt.getDataLongValue("Select docID from UtlTransactions where HdrTblName = 'TrnHdrEXP'", 0, False)
            strSQL = "INSERT [dbo].[UtlDocDsgrFlds] ([DocID], [FldSection], [FldCode], [FldCaption], [FldDataType], [FldDataWidth], [FldBlnReadOnly], [FLdBlnHide], [FldBlnMandatory], [FldUnique], [FldUseDefault], [FldRowClrFontBold], [FldRowClrForeColour], [FldBlnComboData], [FldBlnRefill], [FldOrderBy], [FldFormula], [FldBlnDispOnly], [FldPickUP], [FldNumBlnAnyNumber], [FldNumBlnPercentage], [FldNumDecimals], [FldNumBlnAllowNegative], [FldNumBlnRange], [FldNumBlnRangeFromChk], [FldNumRangeFromOperator], [FldNumRangeFromValue], [FldNumBlnRangeToChk], [FldNumRangeToOperator], [FldNumRangeToValue], [FldBlnNumOrStringList], [FldNumStringListContent], [FldDtBlnAnyDate], [FldDtBlnWithinFinYear], [FldDtBlnRange], [FldDtBlnRangeFrom], [FldDtRangeFromOperator], [FldDtRangeFromValue], [FldDtBlnRangeTo], [FldDtRangeToOperator], [FldDtRangeToValue], [FldDtBlnAsEntered], [FldDtBlnFirstDOM], [FldDtBlnLastDOM], [FldTxtStringType], [FldTxtCase], [FldTxtBlnDoNotAllowChk], [FldTxtDoNotAllowChars], [FldTxtBlnMask], [FldTxtMaskChar], [FldBlnRowClr], [FldBlnFontBold], [FldHideCompareVal], [FldHideOperator], [FldHideWithVal], [FldMndtryCompareVal], [FldMndtryOperator], [FldMndtryWithVal], [FldROCompareVal], [FldROOperator], [FldROWithVal], [FldCmbTblName], [FldCmbStorageFldName], [FldCmbDisplayCols], [FldCmbAddnlCond], [FldDefStdDefault], [FldDefBlnAgnstCond], [FldDefCondList], [FldTxtBlnMultiLine], [FldPickUPAfterFld], [FldToolTip], [FldUniqueBasedOn], [FldTxtAlignment], [FldBlnStandard], [FldBlnLoadFrom], [FldLoadFldName], [FldClrBefHide], [DocValue], [FldLoadFromType], [BlnApportinate], [FldApportinate], [BasedOn], [FldFontName], [FldFontSize], [BlnCopyPreviousRowData], [SubSectionName], [PerAmount], [Separator], [PickFldsFromSrcDocQry], [BlnLoadColFrom], [FldLoadColFrom], [BlnTogHide], [FldBlnNewTrnCalc], [FldBlnChkDuplicate], [BlnTotalReq], [LoadPrevTrnData], [LoadPrevDataParty], [LoadPrevDataItem], [CSVFieldName], [RoundType], [PrevTrnFldName], [FldChkDocDate], [MultiAmtFld]) VALUES (" & intExpID & ",  N'Detail', N'RAMT', N'Receipt Amt', N'Numeric', 12, N'N', N'N', N'N', N'N', N'N', N'N', N'-2147483633', N'N', N'N', 7, N'', N'N', N'', N'Y', N'N', 2, N'N', N'N', N'N', N'          ', N'', N'N', N'          ', N'', N'S', N'', N'Y', N'N', N'N', N'N', N'          ', N'', N'N', N'', N'', N'Y', N'N', N'N', N'ANY', N'ASENTERED', N'N', N'', N'N', N' ', N'N', N'N', N'', N'          ', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', N' ', N'', N'N', N'', N'', N'', N' ', N'N', N'N', N' ', N'N', N'N', N' ', N'N', N'', N'', N'MS Sans Serif', CAST(8.00 AS Numeric(19, 2)), N'N', N'', N'N', N'', NULL, N'N', N'', N'N', N'N', N'N', N'N', N'N', N'N', N'N', N'', N'None', N'', N'N', N'N')"
            Call clsSvt.execSQLStatement(strSQL)
        End If

        Call clsSvt.getDataStringValue("Select setName From utlgsettings Where setName = 'ALLOWNEG'", "", blnFound)
        If Not blnFound Then
            Call clsSvt.execSQLStatement("Delete from utlgsettings Where setName = 'ALLOWNEG'")
            strSQL = "insert into UtlGSettings(setGroup,SetName, SetValue,Setcaption,Remarks,HideSetting)"
            strSQL = strSQL & "values('Inventory','ALLOWNEG','','Allow Negative','Enter Y / N. If Y, then Negative Stock will be allowed.','N')"
            clsSvt.execSQLStatement(strSQL)
        End If

        Call clsSvt.getDataStringValue("Select setName From utlgsettings Where setName = 'UPIurl'", "", blnFound)
        If Not blnFound Then
            strSQL = "insert into UtlGSettings(setGroup,SetName, SetValue,Setcaption,Remarks,HideSetting)"
            strSQL = strSQL & "values('Company','UPIurl','','UPI URL','Enter UPI URL To make payment','N')"
            clsSvt.execSQLStatement(strSQL)
        End If
        If Not fnFldPresent("QRImage", "TrnHdrSInv") Then
            strSQL = "Alter table TrnHdrSInv add QRImage varchar(250) not null default 0"
            clsSvt.execSQLStatement(strSQL)
        End If

        If Not fnFldPresent("FifoRate", "utlstocklist") Then
            strSQL = "Alter table utlstocklist add FifoRate numeric(18,6) not null default 0"
            clsSvt.execSQLStatement(strSQL)
        End If
        If Not fnFldPresent("FifoUsedQty", "utlstocklist") Then
            strSQL = "Alter table utlstocklist add FifoUsedQty numeric(18,2) not null default 0"
            clsSvt.execSQLStatement(strSQL)
        End If

        'alter table utlstocklist add DocValue numeric(18,2) not null default 0
        If Not fnFldPresent("DocValue", "utlstocklist") Then
            strSQL = "Alter table utlstocklist add DocValue numeric(18,2) not null default 0"
            clsSvt.execSQLStatement(strSQL)
        End If

        If Not fnFldPresent("CANCELRMKS", "TRNHDRSINV") Then
            strSQL = "Alter table TrnHdrSInv add CancelRmks varchar(250) not null default ''"
            clsSvt.execSQLStatement(strSQL)
        End If

        If Not fnTablePresent("TrnExpOPBal") Then
            strSQL = "CREATE TABLE TrnExpOPBal (ExpDate smalldatetime NOT NULL, OpBalAmt numeric(18, 2) NOT NULL, CONSTRAINT [PK_TrnExpOPBal] PRIMARY KEY CLUSTERED (ExpDate ASC))"
            clsSvt.execSQLStatement(strSQL)
        End If
        If Not fnTablePresent("vwItemSelPurVar") Then
            strSQL = "create view vwItemSelPurVar as select itemcode, itemname, purrate, ITEMSELL, ROUND(itemsell/purrate*100 -100,0) as VarPer"
            strSQL = strSQL & " From mstitem where LiqType <> 'Others'and purrate > 0 "
            clsSvt.execSQLStatement(strSQL)
        End If
        If Not fnTablePresent("vwExpSmry") Then
            strSQL = "Create view vwExpSmry as "
            strSQL &= " select expDate, sum(opBalAmt) as OPAmt, sum(sExpAmt) as ExpAmt,"
            strSQL &= " sum(opBalAmt - sExpAmt) as BalAmt"
            strSQL &= " from ("
            strSQL &= " select expdate, opbalamt, 0 as sExpAmt from TrnExpOPBal"
            strSQL &= " union all"
            strSQL &= " Select ExpDate, 0,  sum(amt)"
            strSQL &= " from TrnDtlEXP d, trnhdrexp h"
            strSQL &= " where d.EXPid = h.EXPID"
            strSQL &= " group by EXPDate) a"
            strSQL &= " group by ExpDate"
            clsSvt.execSQLStatement(strSQL)
        End If
        If Not fnTablePresent("vwExpDet") Then
            strSQL = " Create view vwExpDet as "
            strSQL &= " Select expdate, 'Opening Balance' as expLed, opbalamt, 0 as LedAmt,'' as Remarks from TrnExpOPBal"
            strSQL &= " union all"
            strSQL &= " Select ExpDate, LEDCODE, 0, amt, d.remarks"
            strSQL &= " from TrnDtlEXP d, trnhdrexp h, MstLed l"
            strSQL &= " where d.EXPid = h.EXPID"
            strSQL &= " and d.LEDID = l.LEDID"
            clsSvt.execSQLStatement(strSQL)
        End If

        If Not fnFldPresent("NOSELINVOICE", "MSTITEM") Then
            strSQL = "Alter table mstItem add NOSELINVOICE char(1) not null default 'N'"
            Call clsSvt.execSQLStatement(strSQL)

            strSQL = "INSERT into [dbo].[UtlDocDsgrFlds] ([DocID], [FldSection], [FldCode], [FldCaption], [FldDataType], [FldDataWidth], [FldBlnReadOnly], [FLdBlnHide], [FldBlnMandatory], [FldUnique], [FldUseDefault], [FldRowClrFontBold], [FldRowClrForeColour], [FldBlnComboData], [FldBlnRefill], [FldOrderBy], [FldFormula], [FldBlnDispOnly], [FldPickUP], [FldNumBlnAnyNumber], [FldNumBlnPercentage], [FldNumDecimals], [FldNumBlnAllowNegative], [FldNumBlnRange], [FldNumBlnRangeFromChk], [FldNumRangeFromOperator], [FldNumRangeFromValue], [FldNumBlnRangeToChk], [FldNumRangeToOperator], [FldNumRangeToValue], [FldBlnNumOrStringList], [FldNumStringListContent], [FldDtBlnAnyDate], [FldDtBlnWithinFinYear], [FldDtBlnRange], [FldDtBlnRangeFrom], [FldDtRangeFromOperator], [FldDtRangeFromValue], [FldDtBlnRangeTo], [FldDtRangeToOperator], [FldDtRangeToValue], [FldDtBlnAsEntered], [FldDtBlnFirstDOM], [FldDtBlnLastDOM], [FldTxtStringType], [FldTxtCase], [FldTxtBlnDoNotAllowChk], [FldTxtDoNotAllowChars], [FldTxtBlnMask], [FldTxtMaskChar], [FldBlnRowClr], [FldBlnFontBold], [FldHideCompareVal], [FldHideOperator], [FldHideWithVal], [FldMndtryCompareVal], [FldMndtryOperator], [FldMndtryWithVal], [FldROCompareVal], [FldROOperator], [FldROWithVal], [FldCmbTblName], [FldCmbStorageFldName], [FldCmbDisplayCols], [FldCmbAddnlCond], [FldDefStdDefault], [FldDefBlnAgnstCond], [FldDefCondList], [FldTxtBlnMultiLine], [FldPickUPAfterFld], [FldToolTip], [FldUniqueBasedOn], [FldTxtAlignment], [FldBlnStandard], [FldBlnLoadFrom], [FldLoadFldName], [FldClrBefHide], [DocValue], [FldLoadFromType], [BlnApportinate], [FldApportinate], [BasedOn], [FldFontName], [FldFontSize], [BlnCopyPreviousRowData], [SubSectionName], [PerAmount], [Separator], [PickFldsFromSrcDocQry], [BlnLoadColFrom], [FldLoadColFrom], [BlnTogHide], [FldBlnNewTrnCalc], [FldBlnChkDuplicate], [BlnTotalReq], [LoadPrevTrnData], [LoadPrevDataParty], [LoadPrevDataItem], [CSVFieldName], [RoundType], [PrevTrnFldName], [FldChkDocDate], [MultiAmtFld]) VALUES (1114,  N'Header', N'NOSELINVOICE', N'No Selection in Invoice', N'List String', 1, N'N', N'N', N'N', N'N', N'Y', N'N', N'-2147483633', N'N', N'N', 15, N'', N'N', N'', N'Y', N'N', 0, N'N', N'N', N'N', N'          ', N'', N'N', N'          ', N'', N'S', N'Y|N', N'Y', N'N', N'N', N'N', N'          ', N'', N'N', N'', N'', N'Y', N'N', N'N', N'ANY', N'ASENTERED', N'N', N'', N'N', N' ', N'N', N'N', N'', N'          ', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'N', N'N', N'', N'N', N'', N'', N'', N' ', N'N', N'N', N' ', N'N', N'N', N' ', N'N', N'', N'', N'MS Sans Serif', CAST(8.00 AS Numeric(19, 2)), N'N', N'', N'N', N'', NULL, N'N', N'', N'N', N'N', N'N', N'N', N'N', N'N', N'N', N'', N'None', N'', N'N', N'N')"
            Call clsSvt.execSQLStatement(strSQL)
        End If

        If Not fnMenuPresent("Merge Items") Then
            Call fnCreateMenu("Merge Items", "", MnuNode.Utilities)
        End If

        If Not fnMenuPresent("Compare Stock") Then
            Call fnCreateMenu("Compare Stock", "", MnuNode.Utilities)
        End If

        Call clsSvt.getDataStringValue("Select setName From utlgsettings Where setName = 'PurSellVarPer'", "", blnFound)
        If Not blnFound Then
            strSQL = "insert into UtlGSettings(setGroup,SetName, SetValue,Setcaption,Remarks,HideSetting)"
            strSQL = strSQL & "values('Company','PurSellVarPer','0','Purchase-Selling Rate variation %','Enter % to which Sales Rate has to Exceed Purchase Rate','N')"
            clsSvt.execSQLStatement(strSQL)
        End If

        'gStrChkValue = getGsettings("PurchaseBill", "CheckPBValue", "")
        Call clsSvt.getDataStringValue("Select setName From utlgsettings Where setName = 'CheckPBValue'", "", blnFound)
        If Not blnFound Then
            strSQL = "insert into UtlGSettings(setGroup,SetName, SetValue,Setcaption,Remarks,HideSetting)"
            strSQL = strSQL & "values('PurchaseBill','CheckPBValue','N','Check Whether PBTotal And Invoice Total','','N')"
            clsSvt.execSQLStatement(strSQL)
        End If

        Call clsSvt.getDataStringValue("Select setName From utlgsettings Where setName = 'ShowDBatStartUp'", "", blnFound)
        If Not blnFound Then
            strSQL = "insert into UtlGSettings(setGroup,SetName, SetValue,Setcaption,Remarks,HideSetting)"
            strSQL = strSQL & "values('Company','ShowDBatStartUp','N','Show Dashboard at startup','','N')"
            clsSvt.execSQLStatement(strSQL)
        End If


        'clsSvt.execSQLStatement("Delete From TrnQOH Where LOCID = 0")
        'clsSvt.execSQLStatement("Delete From TrnQOH Where CLBal = 0")

        If Not fnTablePresent("TrnDtlSinvDenom") Then
            clsSvt.execSQLStatement(" CREATE TABLE [dbo].[TrnDtlSinvDenom](
	[SINVID] [int] NOT NULL,
	[CurName] [varchar](50) NOT NULL,
	[Nos] [int] NULL,
	[Value] [numeric](18, 2) NULL,
 CONSTRAINT [PK_TrnDtlSinvDenom_1] PRIMARY KEY CLUSTERED 
(
	[SINVID] ASC,
	[CurName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]")
        End If


        If Not fnFldPresent("MultiAmtFld", "UtlDocDsgrFlds") Then
            clsSvt.execSQLStatement("Alter table UtlDocDsgrFlds add MultiAmtFld char(1) Default('N') not null ")
        End If

        If Not fnTablePresent("TrnMultiAmt") Then
            clsSvt.execSQLStatement("CREATE TABLE TrnMultiAmt(DocID int, TrnID int, FldCode Varchar(50), Slno int, Particulars varchar(150), Amount Numeric(19,2))")
        End If

        If Not fnTablePresent("UtlMsgSettings") Then
            clsSvt.execSQLStatement("CREATE TABLE [dbo].[UtlMsgSettings]([RowID] [int] NOT NULL, 	[Provider] [varchar](50) NOT NULL,DailyLimit int not null default 0,	[SMSURL] [varchar](500) NOT NULL,	[Def] [char](1) NOT NULL Default('N'), CONSTRAINT [PK_UtlMsgSetting] PRIMARY KEY CLUSTERED (	[Provider] ASC )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY]")
        End If
        clsSvt.getDataStringValue("select name from sys.views where name = 'vwSR'", "", blnFound)
        If Not blnFound Then
            If Not fnTablePresent("vwItemNoStock") Then
                clsSvt.execSQLStatement("Create view [dbo].[vwItemNoStock] as 
Select i.ItemID, g.ITEMGROUPCODE, i.ITEMCODE, i.ITEMNAME, i.ShortName, i.MRP, i.PURRATE 
From Mstitem i, MstITEMGROUP g
where i.ITEMGroupID=g.ITEMGROUPID
and i.InActive = 'N'
and i.ItemID > 0
and IsNull((Select Sum(StkMultiplier * Qty) from UtlStockList u Where u.ItemID=i.ItemID),0) <= 0")
            End If
        End If
        If Not fnTablePresent("TrnSMS") Then
            clsSvt.execSQLStatement("CREATE TABLE TrnSMS (RowID int IDENTITY(1,1), ProviderID int,TrnDate smallDateTime,MobNo varchar(150),MsgText varchar(250),TrnDateTime DateTime)")
        End If

        If Not fnFldPresent("Def", "UtlSMSTemplate") Then
            clsSvt.execSQLStatement("Alter table UtlSMSTemplate Add Def Char(1) Not Null Default('N')")
        End If

        If fnFldPresent("InvMsgMobNO", "UtlMisc") Then
            clsSvt.execSQLStatement("Alter Table UtlMisc Alter column InvMsgMobNO varchar(500)")
        End If

        If Not fnFldPresent("WAMsg", "UtlMisc") Then
            clsSvt.execSQLStatement("Alter Table UtlMisc ADD  WAMSG varchar(1000)")
        End If
        If Not fnFldPresent("WAMsgFile", "UtlMisc") Then
            clsSvt.execSQLStatement("Alter Table UtlMisc ADD WAMSGFILE varchar(1000)")
        End If
        If Not fnFldPresent("WADefRcvr", "UtlMisc") Then
            clsSvt.execSQLStatement("Alter Table UtlMisc ADD WADEFRCVR varchar(20)")
        End If
        If Not fnFldPresent("WAEmoji", "UtlMisc") Then
            clsSvt.execSQLStatement("Alter Table UtlMisc ADD WAEmoji NVARCHAR(100)")
        End If



        Call clsSvt.getDataStringValue("Select setName From utlgsettings Where setName = 'blnBold'", "", blnFound)
        If Not blnFound Then
            strSQL = "insert into UtlGSettings(setGroup,SetName, SetValue,Setcaption,Remarks,HideSetting)"
            strSQL = strSQL & "values('WhatsApp','blnBold','N','Bold','','N')"
            clsSvt.execSQLStatement(strSQL)
        End If

        Call clsSvt.getDataStringValue("Select setName From utlgsettings Where setName = 'blnItalic'", "", blnFound)
        If Not blnFound Then
            strSQL = "insert into UtlGSettings(setGroup,SetName, SetValue,Setcaption,Remarks,HideSetting)"
            strSQL = strSQL & "values('WhatsApp','blnItalic','N','Italic','','N')"
            clsSvt.execSQLStatement(strSQL)
        End If

        Call clsSvt.getDataStringValue("Select setName From utlgsettings Where setName = 'blnUnderLine'", "", blnFound)
        If Not blnFound Then
            strSQL = "insert into UtlGSettings(setGroup,SetName, SetValue,Setcaption,Remarks,HideSetting)"
            strSQL = strSQL & "values('WhatsApp','blnUnderLine','N','Underline','','N')"
            clsSvt.execSQLStatement(strSQL)
        End If




        If fnFldPresent("InvMsgMobNO", "UtlMisc") Then
            clsSvt.execSQLStatement("Alter Table UtlMisc Alter column InvMsgMobNO varchar(500)")
        End If

        clsSvt.getDataStringValue("select name from sys.tables where name = 'TrnHdrSR'", "", blnFound)
        If blnFound Then
            clsSvt.getDataStringValue("select name from sys.views where name = 'vwSR'", "", blnFound)
            If Not blnFound Then
                clsSvt.execSQLStatement("CREATE view [dbo].[vwSR] as select h.*, SRQty      ,SRRate      ,SRAmt      ,LTRPERBOT
      ,SRQTYINLTRS ,i.itemname from  trnhdrsr h,trndtlsr d ,mstitem i where h.srid=d.srid and  i.itemid=d.itemid")
            End If
        End If
        If Not fnFldPresent("PDFPreview", "UtlTransactions") Then
            clsSvt.execSQLStatement("Alter table UtlTransactions add PDFPreview char(1) Default('N') not null")
        End If

        clsSvt.execSQLStatement("Update UtlTransactions Set PDFPreview='Y' Where DocID in (1216)")

        clsSvt.execSQLStatement("Update UtlMisc Set AccTranReq='N'")

        clsSvt.execSQLStatement("Update MstMenu Set MenuShow='N' where MenuItemCaption IN('ShortClose Documents','Authorise Documents','Post Doc Entry','Barcode Layout Format','Barcode Ammendment','Voice Command Alternatives')")

        'clsSvt.execSQLStatement("Update MstMenu Set MenuShow='N' where MenuItemCaption IN('Bulk Update','SQL Saved Query','AuditLog Sync')")
        clsSvt.execSQLStatement("Update MstMenu Set MenuShow='N' where MenuItemCaption IN('Bulk Update','SQL Saved Query')")

        Call clsSvt.getDataStringValue("Select setName From utlgsettings Where setName = 'QTYSELINV'", "", blnFound)
        If Not blnFound Then
            strSQL = "insert into UtlGSettings(setGroup,SetName, SetValue,Setcaption,Remarks,HideSetting)"
            strSQL = strSQL & "values('INVOICE','QTYSELINV','N','Qty: Manual Entry in Invoice','','N')"
            clsSvt.execSQLStatement(strSQL)
        End If

        'Added by Surabhi to set top rows for Invoice Reprint, Cancel
        Call clsSvt.getDataStringValue("Select setName From utlgsettings Where setName = 'InvreprintTopRows'", "", blnFound)
        If Not blnFound Then
            strSQL = "insert into UtlGSettings(setGroup,SetName, SetValue,Setcaption,Remarks,HideSetting)"
            strSQL = strSQL & "values('Invoice','InvreprintTopRows',200,'Invoice Reprint Top Rows','Top Rows to show in Invoice Reprint','N')"
            clsSvt.execSQLStatement(strSQL)
        End If




        Call clsSvt.getDataStringValue("Select setName From utlgsettings Where setName = 'APPRESTART'", "", blnFound)
        If Not blnFound Then
            strSQL = "insert into UtlGSettings(setGroup,SetName, SetValue,Setcaption,Remarks,HideSetting)"
            strSQL = strSQL & "values('INVOICE','APPRESTART','N','Restart Application (mem.leakage)','Restart the application @1pm and @6pm','N')"
            clsSvt.execSQLStatement(strSQL)
        End If

        Call clsSvt.getDataStringValue("Select setName From utlgsettings Where setName = 'SHOWSALESVALUE'", "", blnFound)
        If Not blnFound Then
            strSQL = "insert into UtlGSettings(setGroup,SetName, SetValue,Setcaption,Remarks,HideSetting)"
            strSQL = strSQL & "values('INVOICE','SHOWSALESVALUE','N','Show overall sales value in Main Screen','','N')"
            clsSvt.execSQLStatement(strSQL)
        End If

        Call clsSvt.getDataStringValue("Select MenuItemCaption From MstMenu Where MenuItemCaption = 'Re Write Stock'", "", blnFound)
        If Not blnFound Then
            fnCreateMenu("Re Write Stock", "FRMRPTREWRITESTOCK", 6)
        End If

        If Not fnTablePresent("UtlRunStockProcedure") Then
            strSQL = "Create Table UtlRunStockProcedure (RunDate DateTime)"
        End If

        clsSvt.execSQLStatement("Update u Set u.LtrQty= u.Qty * p.LtrPerBot From UtlStockList u, MstItem i, MstPKG p Where u.ItemID=i.ItemID and i.PKGID=p.PKGID and u.LtrQty=0")

        Call clsSvt.getDataStringValue("Select MenuItemCaption From MstMenu Where MenuItemCaption = 'Download Latest Exe'", "", blnFound)
        If Not blnFound Then
            fnCreateMenu("Download Latest Exe", "", 6)
        End If

        Call clsSvt.getDataStringValue("Select MenuItemCaption From MstMenu Where MenuItemCaption = 'Remove Duplicate Invoice'", "", blnFound)
        If Not blnFound Then
            fnCreateMenu("Remove Duplicate Invoice", "", 7)
        End If

        'Code Of Brand Master
        If Not fnTablePresent("MstBrand") Then
            Dim intDocIDb As Int32
            intDocIDb = clsSvt.getDataLongValue("Select Max(DOCID)+1 From UtlTransactions", 0, False)

            clsSvt.execSQLStatement("CREATE TABLE [dbo].[MstBrand](
        	[BRANDID] [int] NOT NULL,
        	[BRANDCODE] [varchar](50) NULL,
        	[BRANDNAME] [varchar](250) NULL,
        	[BRANDCodeClean]  AS ([dbo].[RemoveNonAlphaCharacters]([BRANDCode])),
        	[BRANDSeries] [varchar](10) NOT NULL DEFAULT (''),
        	[BRANDNO] [numeric](19, 2) NOT NULL DEFAULT ((0)),
        	[InActiveRmks] [varchar](250) NOT NULL DEFAULT (''),
        	[InActive] [char](1) NOT NULL DEFAULT ('N'),
        	[Authorised] [char](1) NOT NULL DEFAULT ('Y'),
        	[AuthBy] [varchar](50) NOT NULL DEFAULT (''),
        	[AuthOn] [datetime] NULL,
        	[AddedBy] [varchar](50) NOT NULL DEFAULT (''),
        	[AddedOn] [datetime] NOT NULL,
        	[ChangedBy] [varchar](50) NOT NULL DEFAULT (''),
        	[ChangedOn] [datetime] NULL,
        PRIMARY KEY CLUSTERED 
        (
        	[BRANDID] ASC
        )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
        ) ON [PRIMARY]")


            clsSvt.execSQLStatement("Insert Into UtlTransactions(DocID,DocType,DocName,HdrTblName,DtlTblName,AllowLoadFromBOM,AllowCopyFrom,DocShForm,LinkedTODoc,LinkedTODocShForm,AuthReq,AmmReq,LinkedFROMDoc,LinkedFROMDocShForm,TrnDocType,AffectsStock,NonItemDoc,ContDocNoGroup,Voucher,BalanceColReq,BillWiseFeatureReq,RepresentAmtAS,DefFirstRowAmtType,ChqPrintReq,PrintAfterSave,MultiDocGen,AllowLoadFromAnotherDoc,LoadPendingDocNos,LoadPendingPartyWise,blnFreezeDoc,AllowEditDeleteForDays,DetSecPresent,AccTrn,TrnDrCr,AllowTenderChange,TenderChangeFld,ItemFilterCond,PartyFilterCond,AllowImpExp,FileAttachReq,MstInventory,MstStockLevels,MstPartyAccounts,MstAutoGenCode,MstPartyTally,NoOfAuth,NoOfAuthQueryCond,OffLineCalc,MstDocIDItem,MstDocIDParty,PendingQryFlds,PendingQryTbls,PendingQryCond,PickFldsFromSrcDocQry,PickFldsFromSrc,AllowManualPartySel,SavePartyData,ShowShipAdd,ShowGraceQty,blnEnvPrint,FreezeQtyCol,FreezeRateCol,ApplyAvgRate,AllowEditDocDate,BlnAllowDuplicateItems,LinkedPendingParty,LinkedPendingDocNo,LinkedPendingSingleDoc,FldblnPartyItem,FldBlnFromMemorandum,FldBlnEnforcePartyItemLink,RateFldPartyItem,RateFldMstItem,AccMstType,AcLedFld,AccValueFld,DayWiseSmry,custsuppreq,FldSMS,GeneratePDF,FldEmail,AuditLogTrack,AuditSQLHdr,AuditSQLDtl,PWAShowQry,PDFPreview)
              Values(" & intDocIDb & ",'Master','Brand Master','MstBrand','','N','N','Brand','N','','N','N','N','','General','NA','N',Null,'N','N','N',Null,Null,'N','N',Null,'N','N','N','N',0,'N','N',Null,'N','','N','','N','N','N','N','N','N','N',0,'','N',0,0,'','','','','','N','Y','N','N','N','N','N','N','N','N','N','N','N','N','N','N','','','','','','N','N','N','N','N','N','','','','N')")


            fnCreateMenu("Brand Master", "", 1)
            clsSvt.execSQLStatement("Update MstMenu Set DocID=" & intDocIDb & ",MenuGroupName='OTHERS' Where  MenuItemCaption = 'Brand Master'")
            fnCreateMenu("Brand Wise Report", "", 4)

            clsSvt.execSQLStatement("Alter table MstItem add BrandID int Default(0)  Not Null")

            clsSvt.execSQLStatement("Insert Into UtlDocDsgrFlds (DocID,FldSection,FldCode,FldCaption,FldDataType,FldDataWidth,FldBlnReadOnly,FLdBlnHide,FldBlnMandatory,FldUnique,FldUseDefault,FldRowClrFontBold,FldRowClrForeColour,FldBlnComboData,FldBlnRefill,FldOrderBy,FldFormula,FldBlnDispOnly,FldPickUP,FldNumBlnAnyNumber,FldNumBlnPercentage,FldNumDecimals,FldNumBlnAllowNegative,FldNumBlnRange,FldNumBlnRangeFromChk,FldNumRangeFromOperator,FldNumRangeFromValue,FldNumBlnRangeToChk,FldNumRangeToOperator,FldNumRangeToValue,FldBlnNumOrStringList,FldNumStringListContent,FldDtBlnAnyDate,FldDtBlnWithinFinYear,FldDtBlnRange,FldDtBlnRangeFrom,FldDtRangeFromOperator,FldDtRangeFromValue,FldDtBlnRangeTo,FldDtRangeToOperator,FldDtRangeToValue,FldDtBlnAsEntered,FldDtBlnFirstDOM,FldDtBlnLastDOM,FldTxtStringType,FldTxtCase,FldTxtBlnDoNotAllowChk,FldTxtDoNotAllowChars,FldTxtBlnMask,FldTxtMaskChar,FldBlnRowClr,FldBlnFontBold,FldHideCompareVal,FldHideOperator,FldHideWithVal,FldMndtryCompareVal,FldMndtryOperator,FldMndtryWithVal,FldROCompareVal,FldROOperator,FldROWithVal,FldCmbTblName,FldCmbStorageFldName,FldCmbDisplayCols,FldCmbAddnlCond,FldDefStdDefault,FldDefBlnAgnstCond,FldDefCondList,FldTxtBlnMultiLine,FldPickUPAfterFld,FldToolTip,FldUniqueBasedOn,FldTxtAlignment,FldBlnStandard,FldBlnLoadFrom,FldLoadFldName,FldClrBefHide,DocValue,FldLoadFromType,BlnApportinate,FldApportinate,BasedOn,FldFontName,FldFontSize,BlnCopyPreviousRowData,SubSectionName,PerAmount,Separator,PickFldsFromSrcDocQry,BlnLoadColFrom,FldLoadColFrom,BlnTogHide,FldBlnNewTrnCalc,FldBlnChkDuplicate,BlnTotalReq,LoadPrevTrnData,LoadPrevDataParty,LoadPrevDataItem,CSVFieldName,RoundType,PrevTrnFldName,FldChkDocDate,MultiAmtFld)
                    Values ('1114','Header','BRANDID','Brand','Numeric','12','N','N','N','N','N','N','-2147483633','Y','N','14','','N','','Y','N','0','N','N','N','','',
                        'N','','','S','','Y','N','N','N','','','N','','','Y','N','N','ANY','ASENTERED','N','','N',' ','N','N','','','','','','','','','','MstBRAND','BRANDID','BRANDCODE','','',' ','','N','','','',' ','N','N',' ','N','N',' ','N','','','MS Sans Serif','8','N','','N','','NULL','N','','N','N','N','N','N','N','N','','None','','N','N')")

            clsSvt.execSQLStatement("CREATE view [dbo].[vwBrandReport] as 
Select PBDate as DocDate, 'Purchase' as DocType,b.BrandID, BrandCode, ig.ItemGroupID,ItemGroupCode, ItemCode, ItemName, UOMCODE, 
PBQty, PBRate, PBAmt, 0 as SInvQty, 0 as SInvRAte, 0 as SInvAmt  
From TrnHdrPB h, TrnDtlPB d, MstItem i, MstITEMGROUP ig, MstUOM u, MstBrand b  
Where h.PBID = d.PBID  and i.ItemID=d.ItemID  and i.ItemGroupID=ig.ITEMGROUPID 
and h.cancelled='N'
and i.UOMID=u.UOMID and i.BRANDID=b.BRANDID  
Union all
Select SINVDate as DocDate, 'Sales' as DocType,b.BrandID, BrandCode,ig.ItemGroupID, ItemGroupCode, ItemCode, ItemName, UOMCODE, 
0 as PBQty,0 as PBRate,0 as PBAmt, SInvQty, SInvRAte, SInvAmt  
From TrnHdrSINV h, TrnDtlSINV d, MstItem i, MstITEMGROUP ig, MstUOM u, MstBrand b  
Where h.SINVID = d.SINVID  and i.ItemID=d.ItemID  and i.ItemGroupID=ig.ITEMGROUPID 
and h.cancelled='N' and h.IsDelete = 'N'
and i.UOMID=u.UOMID and i.BRANDID=b.BRANDID")



        End If
        If fnTablePresent("vwBrandReport") Then
            clsSvt.execSQLStatement("Alter view [dbo].[vwBrandReport] as 
Select PBDate as DocDate, 'Purchase' as DocType,b.BrandID, BrandCode, ig.ItemGroupID,ItemGroupCode, ItemCode, ItemName, UOMCODE, 
PBQty, PBRate, PBAmt, 0 as SInvQty, 0 as SInvRAte, 0 as SInvAmt,i.BotPerCases  
From TrnHdrPB h, TrnDtlPB d, MstItem i, MstITEMGROUP ig, MstUOM u, MstBrand b  
Where h.PBID = d.PBID  and i.ItemID=d.ItemID  and i.ItemGroupID=ig.ITEMGROUPID 
and h.cancelled='N'
and i.UOMID=u.UOMID and i.BRANDID=b.BRANDID  
Union all
Select SINVDate as DocDate, 'Sales' as DocType,b.BrandID, BrandCode,ig.ItemGroupID, ItemGroupCode, ItemCode, ItemName, UOMCODE, 
0 as PBQty,0 as PBRate,0 as PBAmt, SInvQty, SInvRAte, SInvAmt  , i.BotPerCases
From TrnHdrSINV h, TrnDtlSINV d, MstItem i, MstITEMGROUP ig, MstUOM u, MstBrand b  
Where h.SINVID = d.SINVID  and i.ItemID=d.ItemID  and i.ItemGroupID=ig.ITEMGROUPID 
and h.cancelled='N' and h.IsDelete = 'N'
and i.UOMID=u.UOMID and i.BRANDID=b.BRANDID")
        End If

        '################Code For DashBoard#############

        If Not fnTablePresent("UtlDashboard") Then
            clsSvt.execSQLStatement("CREATE TABLE [dbo].[UtlDashboard](
	[DashID] [int] NOT NULL,
	[DashQry] [varchar](max) NULL,
	[RTitle] [varchar](50) NULL,
	[Remark] [varchar](50) NULL,
	[Visible] [char](1) NULL CONSTRAINT [DF_UtlDashboard_Visible]  DEFAULT ('Y')
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]")


            fnCreateMenu("Dashboard", "", 0)
            clsSvt.execSQLStatement("Update UtlMisc Set DashBoardReq = 'Y'")
            clsSvt.execSQLStatement("Alter Table MstComp Add DashImg image")
        End If
        Dim strTxt As String = ""
        Dim intK As Integer
        intK = clsSvt.getDataLongValue("Select dashid from utlDashBoard where dashID = 1", 0, False)
        If intK = 0 Then
            strTxt = "Select g.ItemGroupCode as ItemGroup, cast(Sum(convert(Int,d.PBQty))as decimal(15,2))as Qty, cast(Sum(i.MRP * d.PBQty)as decimal(15,2)) as Amount From TrnHdrPB h, TrnDtlPB d, MstItem i, MstItemGroup g  Where h.PBID=d.PBID and d.ITEMID=i.ITEMID and i.ITEMGroupID=g.ITEMGROUPID and Cancelled=`N` and PBDate =  convert(date,getdate()) Group by g.ItemGroupCode"
            clsSvt.execSQLStatement("Insert Into UtlDashBoard(DashID,DashQry,RTitle,Remark,Visible) Values(1,'" & strTxt & "','Purchase Information','','Y')")
        End If
        intK = clsSvt.getDataLongValue("Select dashid from utlDashBoard where dashID = 2", 0, False)
        If intK = 0 Then
            strTxt = "Select Count(SINVNo) As TotBills, cast(Sum(INVValue) As Decimal(15,2)) As TotSales, cast(sum(CashAmt) As Decimal(15,2)) As TotCashAmt,cast(Sum(CardAmt) As Decimal(15,2)) As TotCardAmt,cast( Sum(OthPayAmt) As Decimal(15,2)) As OtherAmt from TrnHdrSINV Where SINVDate =  convert(date,getdate()) and Cancelled = `N`"
            clsSvt.execSQLStatement("Insert Into UtlDashBoard(DashID,DashQry,RTitle,Remark,Visible) Values(2,'" & strTxt & "','Sales Information','','Y')")
        End If

        intK = clsSvt.getDataLongValue("Select dashid from utlDashBoard where dashID = 3", 0, False)
        If intK = 0 Then
            strTxt = "Select cast(Sum(Qty * StkMultiplier) as decimal(15,2)) as OpQty, cast(Sum(MRP * Qty * StkMultiplier) as decimal(15,2)) as OpBal from UtlStockList  u, MstItem i Where u.ItemID=i.ItemID and DocDate <  convert(date,getdate())"
            clsSvt.execSQLStatement("Insert Into UtlDashBoard(DashID,DashQry,RTitle,Remark,Visible) Values(3,'" & strTxt & "','Stock Information','','Y')")
        End If

        intK = clsSvt.getDataLongValue("Select dashid from utlDashBoard where dashID = 4", 0, False)
        If intK = 0 Then
            strTxt = "Select l.LedCode, cast(Sum(d.Amt) As Decimal(15, 2)) As Amount From TrnHdrEXP h, TrnDtlExp d, MstLed l  Where h.EXPID= d.EXPID And d.LEDID = l.LEDID And Cancelled =`N` and ExpDate =  convert(date,getdate()) Group by l.LedCode"
            clsSvt.execSQLStatement("Insert Into UtlDashBoard(DashID,DashQry,RTitle,Remark,Visible) Values(4,'" & strTxt & "','Others/Mis Information','','Y')")
        End If


        If Not fnTablePresent("UtlDashSettings") Then
            clsSvt.execSQLStatement("CREATE TABLE [dbo].[UtlDashSettings](
	[ShowDashBoard] [char](1) NULL,
	[SalesBGClr] [varchar](50) NULL,
	[PurchaseBGClr] [varchar](50) NULL,
	[StockBGClr] [varchar](50) NULL,
	[MiscBGClr] [varchar](50) NULL,
	[ShowAtStartUp] [char](1) NULL,
	[SalesFrClr] [varchar](50) NULL,
	[PurchaseFrClr] [varchar](50) NULL,
	[StockFrClr] [varchar](50) NULL,
	[MiscFrClr] [varchar](50) NULL,
	[HdrFont] [varchar](50) NULL,
	[HdrSize] [int] NULL,
	[HdrStyle] [varchar](50) NULL,
	[InfoFont] [varchar](50) NULL,
	[InfoSize] [int] NULL,
	[InfoStyle] [varchar](50) NOT NULL
) ON [PRIMARY]")

        End If

        If fnTablePresent("UtlDashQry") Then
            clsSvt.execSQLStatement("Update UtlDashQry Set InfoQry='SELECT sum(INVVALUE) as InvValue FROM TrnHdrSINV WHERE IsDelete<>''Y'' and SINVDate = CAST( GETDATE() AS Date )' Where OrderNo=1 and InfoHeader='Sales For The Day'")
            clsSvt.execSQLStatement("Update UtlDashQry Set InfoQry='SELECT sum(INVVALUE) as InvValue FROM TrnHdrSINV WHERE SINVDate >= DATEADD(wk, 0, DATEADD(DAY, 1-DATEPART(WEEKDAY, GETDATE()), DATEDIFF(dd, 0, GETDATE()))) and  IsDelete<>''Y'' and sinvdate <= DATEADD(wk, 1, DATEADD(DAY, 0-DATEPART(WEEKDAY, GETDATE()), DATEDIFF(dd, 0, GETDATE())))' Where OrderNo=2 and InfoHeader='Sales For The Week'")
            clsSvt.execSQLStatement("Update UtlDashQry Set InfoQry='SELECT sum(INVVALUE) as InvValue FROM TrnHdrSINV WHERE year(SINVDate) = year(getdate()) and IsDelete<>''Y'' and month(sinvdate) = month(getdate())' Where OrderNo=3 and InfoHeader='Sales For The Month'")
        End If

        If Not fnTablePresent("UtlDashQry") Then

            clsSvt.execSQLStatement("CREATE TABLE [dbo].[UtlDashQry](
	[Recid] [int] IDENTITY(1,1) NOT NULL,
	[DashShow] [char](1) NULL,
	[SectionHdr] [varchar](50) NULL,
	[OrderNo] [int] NULL,
	[GroupHeader] [varchar](150) NULL,
	[InfoHeader] [varchar](150) NULL,
	[InfoType] [varchar](50) NULL,
	[InfoQry] [varchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]")


            clsSvt.execSQLStatement("insert into UtlDashQry (DashShow,SectionHdr,OrderNo,GroupHeader,InfoHeader,InfoType,InfoQry) values ('Y','SALES',1,'Sales Info','Sales For The Day','SINGLEVALUE','SELECT sum(INVVALUE) as InvValue FROM TrnHdrSINV WHERE IsDelete<>''Y'' and SINVDate = CAST( GETDATE() AS Date )') ")

            clsSvt.execSQLStatement("insert into UtlDashQry (DashShow,SectionHdr,OrderNo,GroupHeader,InfoHeader,InfoType,InfoQry) values ('Y','SALES',2,'Sales Info','Sales For The Week','SINGLEVALUE','SELECT sum(INVVALUE) as InvValue FROM TrnHdrSINV WHERE SINVDate >= DATEADD(wk, 0, DATEADD(DAY, 1-DATEPART(WEEKDAY, GETDATE()), DATEDIFF(dd, 0, GETDATE()))) and  IsDelete<>''Y'' and sinvdate <= DATEADD(wk, 1, DATEADD(DAY, 0-DATEPART(WEEKDAY, GETDATE()), DATEDIFF(dd, 0, GETDATE())))')")

            clsSvt.execSQLStatement("insert into UtlDashQry (DashShow,SectionHdr,OrderNo,GroupHeader,InfoHeader,InfoType,InfoQry) values ('Y','SALES',3,'Sales Info','Sales For The Month','SINGLEVALUE','SELECT sum(INVVALUE) as InvValue FROM TrnHdrSINV WHERE year(SINVDate) = year(getdate()) and IsDelete<>''Y'' and month(sinvdate) = month(getdate())')")

            clsSvt.execSQLStatement("insert into UtlDashQry (DashShow,SectionHdr,OrderNo,GroupHeader,InfoHeader,InfoType,InfoQry) values ('Y','SALES',4,'Expenses Summary','Expenses For The Day','SINGLEVALUE','SELECT sum(TOTAMT) as Expvalue FROM TrnHdrEXP WHERE EXPDate = CAST( GETDATE() AS Date )')")

            clsSvt.execSQLStatement("insert into UtlDashQry (DashShow,SectionHdr,OrderNo,GroupHeader,InfoHeader,InfoType,InfoQry) values ('Y','SALES',5,'Expenses Summary','Expenses For The Week','SINGLEVALUE','SELECT sum(TOTAMT) as Expvalue FROM TrnHdrEXP WHERE EXPDate >= DATEADD(wk, 0, DATEADD(DAY, 1-DATEPART(WEEKDAY, GETDATE()), DATEDIFF(dd, 0, GETDATE()))) and EXPDate <= DATEADD(wk, 1, DATEADD(DAY, 0-DATEPART(WEEKDAY, GETDATE()), DATEDIFF(dd, 0, GETDATE())))')")

            clsSvt.execSQLStatement("insert into UtlDashQry (DashShow,SectionHdr,OrderNo,GroupHeader,InfoHeader,InfoType,InfoQry) values ('Y','SALES',6,'Expenses Summary','Expenses For The Month','SINGLEVALUE','SELECT sum(TOTAMT) as Expvalue FROM TrnHdrEXP WHERE year(EXPDate) = year(getdate()) and month(EXPDate) = month(getdate())')")

            clsSvt.execSQLStatement("insert into UtlDashQry (DashShow,SectionHdr,OrderNo,GroupHeader,InfoHeader,InfoType,InfoQry) values ('Y','SALES',7,'Fast Moving 25 Items','','GRIDVALUE','select top 25  itemname, sum(sinvqty) as Qty from trnhdrsinv h,trndtlsinv d,MstITEM i where h.SINVID=d.SINVID and d.ITEMID=i.ITEMID group by itemname ORDER BY sum(sinvqty) DESC')")

            clsSvt.execSQLStatement("insert into UtlDashQry (DashShow,SectionHdr,OrderNo,GroupHeader,InfoHeader,InfoType,InfoQry) values ('Y','PURCHASE',1,'Purchase Info','Purchase For The Day','SINGLEVALUE','SELECT sum(GRNDTOT) as PBValue FROM TrnHdrPB WHERE PBDate = CAST( GETDATE() AS Date )')")

            clsSvt.execSQLStatement("insert into UtlDashQry (DashShow,SectionHdr,OrderNo,GroupHeader,InfoHeader,InfoType,InfoQry) values ('Y','PURCHASE',2,'Purchase Info','Purchase For The Week','SINGLEVALUE','SELECT sum(GRNDTOT) as PBValue FROM TrnHdrPB WHERE PBDate >= DATEADD(wk, 0, DATEADD(DAY, 1-DATEPART(WEEKDAY, GETDATE()), DATEDIFF(dd, 0, GETDATE()))) and PBDate <= DATEADD(wk, 1, DATEADD(DAY, 0-DATEPART(WEEKDAY, GETDATE()), DATEDIFF(dd, 0, GETDATE())))')")

            clsSvt.execSQLStatement("insert into UtlDashQry (DashShow,SectionHdr,OrderNo,GroupHeader,InfoHeader,InfoType,InfoQry) values ('Y','PURCHASE',3,'Purchase Info','Purchase For The Month','SINGLEVALUE','SELECT sum(GRNDTOT) as PBValue FROM TrnHdrPB  WHERE year(PBDate) = year(getdate()) and month(PBDate) = month(getdate())')")

            clsSvt.execSQLStatement("insert into UtlDashQry (DashShow,SectionHdr,OrderNo,GroupHeader,InfoHeader,InfoType,InfoQry) values ('Y','PURCHASE',4,'Top 5 Purchase Invoice','','GRIDVALUE','SELECT top 5 SUPPCODE as Supplier ,PBNO,PBdate,GRNDTOT as PBValue FROM TrnHdrPB h ,mstSupp s where h.SUPPID=S.SUPPID order by pbno desc')")

            clsSvt.execSQLStatement("insert into UtlDashQry (DashShow,SectionHdr,OrderNo,GroupHeader,InfoHeader,InfoType,InfoQry) values ('Y','STOCK',1,'Stock Info','Current Stock Value','SINGLEVALUE','Select sum(clbal * i.PURRATE) as CurrentStock from trnqoh t, mstitem i where t.itemid = i.itemid')")

            clsSvt.execSQLStatement("insert into UtlDashQry (DashShow,SectionHdr,OrderNo,GroupHeader,InfoHeader,InfoType,InfoQry) values ('Y','STOCK',2,'Stock Info','Last (100) days Non Moving Items','GRIDVALUE','select itemname, CurrentStock, lastTrnDate, DATEDIFF(dd,lasttrndate,getdate()) as ageDays from (
select ItemName, clbal as CurrentStock, 
(select max(docdate) from UtlStockList u where u.itemid = t.itemid) as lastTrnDate
 from TrnQOH t, mstitem i
where t.itemid not in (select itemid from UtlStockList where DocDate >= dateadd(dd,-100,getdate()))
and clbal > 0 and t.itemid = i.itemid) a')")

            clsSvt.execSQLStatement("insert into UtlDashQry (DashShow,SectionHdr,OrderNo,GroupHeader,InfoHeader,InfoType,InfoQry) values ('Y','MISC',1,'MISC Info','Items Below Min Level','GRIDVALUE','select ItemName, clbal as CurrentStock from TrnQOH t, mstitem i where t.itemid = i.itemid and i.MINLEVEL > 0 and clbal < i.MINLEVEL order by 1')")

        End If

        Call clsSvt.getDataStringValue("Select setName From utlgsettings Where setName = 'BlockPrint'", "", blnFound)
        If Not blnFound Then
            strSQL = "Insert into UtlGSettings(setGroup,SetName, SetValue,Setcaption,Remarks,HideSetting)"
            strSQL = strSQL & "values('INVOICE','BlockPrint','','BlockPrintINV','To Block Printing On a Perticular PC','N')"
            clsSvt.execSQLStatement(strSQL)
        End If

        Call clsSvt.getDataStringValue("Select setName From utlgsettings Where setName = 'ShowInvVal'", "", blnFound)
        If Not blnFound Then
            strSQL = "Insert into UtlGSettings(setGroup,SetName, SetValue,Setcaption,Remarks,HideSetting)"
            strSQL = strSQL & "values('INVOICE','ShowInvVal','N','ShowInvVal','To Show Inv Total on Top-Left Corner','N')"
            clsSvt.execSQLStatement(strSQL)
        End If

        'for loyalty
        If Not fnFldPresent("EnableLoyalty", "UtlMisc") Then   'CNUD
            clsSvt.execSQLStatement("Alter table UtlMisc add EnableLoyalty char(1) Default('N') Not Null")
        End If

        Call clsSvt.getDataStringValue("Select setName From utlgsettings Where setName = 'BillValueGreaterThan'", "", blnFound)
        If Not blnFound Then
            strSQL = "insert into UtlGSettings(setGroup,SetName, SetValue,Setcaption,Remarks,HideSetting)"
            strSQL = strSQL & "values('Loyalty','BillValueGreaterThan','1500','Apply points for value > than ','','Y')"
            clsSvt.execSQLStatement(strSQL)
        End If

        Call clsSvt.getDataStringValue("Select setName From utlgsettings Where setName = 'PointsForOneRs'", "", blnFound)
        If Not blnFound Then
            strSQL = "insert into UtlGSettings(setGroup,SetName, SetValue,Setcaption,Remarks,HideSetting)"
            strSQL = strSQL & "values('Loyalty','PointsForOneRs','0.01','Number of points for 1 Rs','','Y')"
            clsSvt.execSQLStatement(strSQL)
        End If

        Call clsSvt.getDataStringValue("Select setName From utlgsettings Where setName = 'RsForOnePoint'", "", blnFound)
        If Not blnFound Then
            strSQL = "insert into UtlGSettings(setGroup,SetName, SetValue,Setcaption,Remarks,HideSetting)"
            strSQL = strSQL & "values('Loyalty','RsForOnePoint','1','Point Value in Rs','','N')"
            clsSvt.execSQLStatement(strSQL)
        End If


        Call clsSvt.getDataStringValue("Select setName From utlgsettings Where setName = 'AlertIfPointsGreaterThan'", "", blnFound)
        If Not blnFound Then
            strSQL = "insert into UtlGSettings(setGroup,SetName, SetValue,Setcaption,Remarks,HideSetting)"
            strSQL = strSQL & "values('Loyalty','AlertIfPointsGreaterThan','50000','Alert if Accrued points greater than','','Y')"
            clsSvt.execSQLStatement(strSQL)
        End If


        If Not fnFldPresent("Loyalty", "MSTITEMGROUP") Then
            strSQL = "Alter table mstItemgroup add LOYALTY char(1) not null default 'N'"
            Call clsSvt.execSQLStatement(strSQL)

            strSQL = "INSERT dbo.UtlDocDsgrFlds (DocID,  FldSection, FldCode, FldCaption, FldDataType, FldDataWidth, FldBlnReadOnly, FLdBlnHide, FldBlnMandatory, FldUnique, FldUseDefault, FldRowClrFontBold, FldRowClrForeColour, FldBlnComboData, FldBlnRefill, FldOrderBy, FldFormula, FldBlnDispOnly, FldPickUP, FldNumBlnAnyNumber, FldNumBlnPercentage, FldNumDecimals, FldNumBlnAllowNegative, FldNumBlnRange, FldNumBlnRangeFromChk, FldNumRangeFromOperator, FldNumRangeFromValue, FldNumBlnRangeToChk, FldNumRangeToOperator, FldNumRangeToValue, FldBlnNumOrStringList, FldNumStringListContent, FldDtBlnAnyDate, FldDtBlnWithinFinYear, FldDtBlnRange, FldDtBlnRangeFrom, FldDtRangeFromOperator, FldDtRangeFromValue, FldDtBlnRangeTo, FldDtRangeToOperator, FldDtRangeToValue, FldDtBlnAsEntered, FldDtBlnFirstDOM, FldDtBlnLastDOM, FldTxtStringType, FldTxtCase, FldTxtBlnDoNotAllowChk, FldTxtDoNotAllowChars, FldTxtBlnMask, FldTxtMaskChar, FldBlnRowClr, FldBlnFontBold, FldHideCompareVal, FldHideOperator, FldHideWithVal, FldMndtryCompareVal, FldMndtryOperator, FldMndtryWithVal, FldROCompareVal, FldROOperator, FldROWithVal, FldCmbTblName, FldCmbStorageFldName, FldCmbDisplayCols, FldCmbAddnlCond, FldDefStdDefault, FldDefBlnAgnstCond, FldDefCondList, FldTxtBlnMultiLine, FldPickUPAfterFld, FldToolTip, FldUniqueBasedOn, FldTxtAlignment, FldBlnStandard, FldBlnLoadFrom, FldLoadFldName, FldClrBefHide, DocValue, FldLoadFromType, BlnApportinate, FldApportinate, BasedOn, FldFontName, FldFontSize, BlnCopyPreviousRowData, SubSectionName, PerAmount, Separator, PickFldsFromSrcDocQry, BlnLoadColFrom, FldLoadColFrom, BlnTogHide, FldBlnNewTrnCalc, FldBlnChkDuplicate, BlnTotalReq, LoadPrevTrnData, LoadPrevDataParty, LoadPrevDataItem, CSVFieldName, RoundType, PrevTrnFldName, FldChkDocDate, MultiAmtFld) VALUES (1110,  N'Header', N'LOYALTY', N'Consider for Loyalty', N'List String', 1, N'N', N'N', N'Y', N'N', N'Y', N'N', N'-2147483633', N'N', N'N', 17, N'', N'N', N'', N'Y', N'N', 0, N'N', N'N', N'N', N'          ', N'', N'N', N'          ', N'', N'S', N'Y|N', N'Y', N'N', N'N', N'N', N'          ', N'', N'N', N'', N'', N'Y', N'N', N'N', N'ANY', N'ASENTERED', N'N', N'', N'N', N' ', N'N', N'N', N'', N'          ', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'N', N'N', N'', N'N', N'', N'', N'', N' ', N'N', N'N', N'', N'N', N'N', N' ', N'N', N'', N'', N'MS Sans Serif', CAST(8.00 AS Numeric(19, 2)), N'N', N'', N'N', N'', NULL, N'N', N'', N'N', N'N', N'N', N'N', N'N', N'N', N'N', N'', N'None', N'', N'N', N'N')"
            Call clsSvt.execSQLStatement(strSQL)
        End If

        'clsSvt.execSQLStatement("Update MstMenu set MenuShow = 'Y' where MenuItemCaption = 'Customer Master'")

        If Not fnFldPresent("CARDNO", "MSTCUST") Then
            strSQL = "Alter table MstCust add CARDNO varchar(50) not null default ''"
            Call clsSvt.execSQLStatement(strSQL)

            strSQL = "INSERT dbo.UtlDocDsgrFlds (DocID,  FldSection, FldCode, FldCaption, FldDataType, FldDataWidth, FldBlnReadOnly, FLdBlnHide, FldBlnMandatory, FldUnique, FldUseDefault, FldRowClrFontBold, FldRowClrForeColour, FldBlnComboData, FldBlnRefill, FldOrderBy, FldFormula, FldBlnDispOnly, FldPickUP, FldNumBlnAnyNumber, FldNumBlnPercentage, FldNumDecimals, FldNumBlnAllowNegative, FldNumBlnRange, FldNumBlnRangeFromChk, FldNumRangeFromOperator, FldNumRangeFromValue, FldNumBlnRangeToChk, FldNumRangeToOperator, FldNumRangeToValue, FldBlnNumOrStringList, FldNumStringListContent, FldDtBlnAnyDate, FldDtBlnWithinFinYear, FldDtBlnRange, FldDtBlnRangeFrom, FldDtRangeFromOperator, FldDtRangeFromValue, FldDtBlnRangeTo, FldDtRangeToOperator, FldDtRangeToValue, FldDtBlnAsEntered, FldDtBlnFirstDOM, FldDtBlnLastDOM, FldTxtStringType, FldTxtCase, FldTxtBlnDoNotAllowChk, FldTxtDoNotAllowChars, FldTxtBlnMask, FldTxtMaskChar, FldBlnRowClr, FldBlnFontBold, FldHideCompareVal, FldHideOperator, FldHideWithVal, FldMndtryCompareVal, FldMndtryOperator, FldMndtryWithVal, FldROCompareVal, FldROOperator, FldROWithVal, FldCmbTblName, FldCmbStorageFldName, FldCmbDisplayCols, FldCmbAddnlCond, FldDefStdDefault, FldDefBlnAgnstCond, FldDefCondList, FldTxtBlnMultiLine, FldPickUPAfterFld, FldToolTip, FldUniqueBasedOn, FldTxtAlignment, FldBlnStandard, FldBlnLoadFrom, FldLoadFldName, FldClrBefHide, DocValue, FldLoadFromType, BlnApportinate, FldApportinate, BasedOn, FldFontName, FldFontSize, BlnCopyPreviousRowData, SubSectionName, PerAmount, Separator, PickFldsFromSrcDocQry, BlnLoadColFrom, FldLoadColFrom, BlnTogHide, FldBlnNewTrnCalc, FldBlnChkDuplicate, BlnTotalReq, LoadPrevTrnData, LoadPrevDataParty, LoadPrevDataItem, CSVFieldName, RoundType, PrevTrnFldName, FldChkDocDate, MultiAmtFld) VALUES (1112,  N'Header', N'CARDNO', N'Loyalty Card Number', N'Text', 50, N'N', N'N', N'N', N'N', N'N', N'', N'-2147483633', N'N', N'N', 17, N'', N'N', N'', N'Y', N'N', 0, N'N', N'N', N'N', N'          ', N'', N'N', N'          ', N'', N'S', N'Y|N', N'Y', N'N', N'N', N'N', N'          ', N'', N'N', N'', N'', N'Y', N'N', N'N', N'ANY', N'ASENTERED', N'N', N'', N'N', N' ', N'N', N'N', N'', N'          ', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'N', N'N', N'', N'N', N'', N'', N'', N' ', N'N', N'N', N'', N'N', N'N', N' ', N'N', N'', N'', N'MS Sans Serif', CAST(8.00 AS Numeric(19, 2)), N'N', N'', N'N', N'', NULL, N'N', N'', N'N', N'N', N'N', N'N', N'N', N'N', N'N', N'', N'None', N'', N'N', N'N')"
            Call clsSvt.execSQLStatement(strSQL)
        End If
        If Not fnFldPresent("SENDSMS", "MSTCUST") Then
            strSQL = "Alter table MstCust add SENDSMS char(1) not null default 'N'"
            Call clsSvt.execSQLStatement(strSQL)

            strSQL = "INSERT dbo.UtlDocDsgrFlds (DocID,  FldSection, FldCode, FldCaption, FldDataType, FldDataWidth, FldBlnReadOnly, FLdBlnHide, FldBlnMandatory, FldUnique, FldUseDefault, FldRowClrFontBold, FldRowClrForeColour, FldBlnComboData, FldBlnRefill, FldOrderBy, FldFormula, FldBlnDispOnly, FldPickUP, FldNumBlnAnyNumber, FldNumBlnPercentage, FldNumDecimals, FldNumBlnAllowNegative, FldNumBlnRange, FldNumBlnRangeFromChk, FldNumRangeFromOperator, FldNumRangeFromValue, FldNumBlnRangeToChk, FldNumRangeToOperator, FldNumRangeToValue, FldBlnNumOrStringList, FldNumStringListContent, FldDtBlnAnyDate, FldDtBlnWithinFinYear, FldDtBlnRange, FldDtBlnRangeFrom, FldDtRangeFromOperator, FldDtRangeFromValue, FldDtBlnRangeTo, FldDtRangeToOperator, FldDtRangeToValue, FldDtBlnAsEntered, FldDtBlnFirstDOM, FldDtBlnLastDOM, FldTxtStringType, FldTxtCase, FldTxtBlnDoNotAllowChk, FldTxtDoNotAllowChars, FldTxtBlnMask, FldTxtMaskChar, FldBlnRowClr, FldBlnFontBold, FldHideCompareVal, FldHideOperator, FldHideWithVal, FldMndtryCompareVal, FldMndtryOperator, FldMndtryWithVal, FldROCompareVal, FldROOperator, FldROWithVal, FldCmbTblName, FldCmbStorageFldName, FldCmbDisplayCols, FldCmbAddnlCond, FldDefStdDefault, FldDefBlnAgnstCond, FldDefCondList, FldTxtBlnMultiLine, FldPickUPAfterFld, FldToolTip, FldUniqueBasedOn, FldTxtAlignment, FldBlnStandard, FldBlnLoadFrom, FldLoadFldName, FldClrBefHide, DocValue, FldLoadFromType, BlnApportinate, FldApportinate, BasedOn, FldFontName, FldFontSize, BlnCopyPreviousRowData, SubSectionName, PerAmount, Separator, PickFldsFromSrcDocQry, BlnLoadColFrom, FldLoadColFrom, BlnTogHide, FldBlnNewTrnCalc, FldBlnChkDuplicate, BlnTotalReq, LoadPrevTrnData, LoadPrevDataParty, LoadPrevDataItem, CSVFieldName, RoundType, PrevTrnFldName, FldChkDocDate, MultiAmtFld) VALUES (1112,  N'Header', N'SENDSMS', N'Send SMS to Customer', N'List String', 1, N'N', N'N', N'Y', N'N', N'Y', N'N', N'-2147483633', N'N', N'N', 17, N'', N'N', N'', N'Y', N'N', 0, N'N', N'N', N'N', N'          ', N'', N'N', N'          ', N'', N'S', N'Y|N', N'Y', N'N', N'N', N'N', N'          ', N'', N'N', N'', N'', N'Y', N'N', N'N', N'ANY', N'ASENTERED', N'N', N'', N'N', N' ', N'N', N'N', N'', N'          ', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'N', N'N', N'', N'N', N'', N'', N'', N' ', N'N', N'N', N'', N'N', N'N', N' ', N'N', N'', N'', N'MS Sans Serif', CAST(8.00 AS Numeric(19, 2)), N'N', N'', N'N', N'', NULL, N'N', N'', N'N', N'N', N'N', N'N', N'N', N'N', N'N', N'', N'None', N'', N'N', N'N')"
            Call clsSvt.execSQLStatement(strSQL)
        End If
        If Not fnTablePresent("TrnPoints") Then
            strSQL = "Create table TrnPoints (DocID int identity, DocDate smalldatetime, CustID int, 
                SInvID int, sInvNo varchar(50), sInvdate smalldatetime, InvAmt  numeric(18,2), Points int, 
                Remarks varchar(250))"
            Call clsSvt.execSQLStatement(strSQL)
        End If

        If clsSvt.getDataStringValue("select EnableLoyalty from UtlMisc", "N", False) = "Y" Then
            If Not fnMenuPresent("Loyalty Report") Then
                Call fnCreateMenu("Loyalty Report", "", MnuNode.Reports)
            End If
            clsSvt.execSQLStatement("Update UtlGSettings set HideSetting = 'N' where setName = 'BillValueGreaterThan'")
            clsSvt.execSQLStatement("Update UtlGSettings set HideSetting = 'N' where setName = 'PointsForOneRs'")
            clsSvt.execSQLStatement("Update UtlGSettings set HideSetting = 'N' where setName = 'AlertIfPointsGreaterThan'")
            clsSvt.execSQLStatement("Update UtlDocDsgrFlds set FLdBlnHide = 'N' where FldCode = 'LOYALTY'")
            clsSvt.execSQLStatement("Update UtlDocDsgrFlds set FLdBlnHide = 'N' where FldCode = 'CARDNO'")
            clsSvt.execSQLStatement("Update UtlDocDsgrFlds set FLdBlnHide = 'N' where FldCode = 'SENDSMS'")
        End If


        Call clsSvt.getDataStringValue("Select setName From utlgsettings Where setName = 'FreezeDate'", "", blnFound)
        If Not blnFound Then
            strSQL = "insert into UtlGSettings(setGroup,SetName, SetValue,Setcaption,Remarks,HideSetting)"
            strSQL = strSQL & "values('FreezeDate','FreezeDate','" & getDateFormatInInsert(DateAdd(DateInterval.Year, -1, getSystemDate())) & "','FreezeDate','Freeze the Document date to Edit/Delete','N')"
            clsSvt.execSQLStatement(strSQL)
        End If

        'added by Akshay as per kumar sir instruction 28/09/23
        If Not fnTablePresent("RptBC") Then
            clsSvt.execSQLStatement("CREATE TABLE [dbo].[RptBC](
	[SLNO] [int] NOT NULL,
	[USERNAME] [varchar](50) NULL,
	[BCNO] [varchar](50) NULL,
	[ItemCode] [varchar](50) NULL,
	[ItemName] [varchar](150) NULL,
	[Qty] [numeric](18, 2) NULL,
	[MRP] [numeric](18, 2) NULL,
	[UOM] [varchar](10) NULL,
	[ExpDate] [smalldatetime] NULL,
	[FLD1] [varchar](150) NULL,
	[FLD2] [varchar](150) NULL,
	[FLD3] [varchar](150) NULL,
	[FLD4] [varchar](150) NULL,
	[FLD5] [varchar](150) NULL,
	[FLD6] [varchar](150) NULL,
	[FLD7] [varchar](150) NULL,
	[FLD8] [varchar](150) NULL,
	[FLD9] [varchar](150) NULL,
	[FLD10] [varchar](150) NULL,
	[FLD11] [varchar](150) NULL,
	[FLD12] [varchar](150) NULL,
	[FLD13] [varchar](150) NULL,
	[FLD14] [varchar](150) NULL,
	[FLD15] [varchar](150) NULL,
 CONSTRAINT [PK_RptBC] PRIMARY KEY CLUSTERED 
(
	[SLNO] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]")

            clsSvt.execSQLStatement("CREATE TABLE [dbo].[RptBCAdnlflds](
	[FLDNO] [varchar](50) NULL,
	[FLDNAME] [varchar](50) NULL
) ON [PRIMARY]")
        End If

    End Sub

#End Region
    Private Sub btnDesigner_Click(sender As Object, e As EventArgs) Handles btnDesigner.Click
        Try
            Process.Start(gstrRptPath & "\vLQMS Designer.exe")
        Catch ex As Exception
            MsgBox("Designer application not Found." & vbCrLf & " [btnDesigner_Click]", vbInformation, Me.Text)
        End Try
    End Sub

    Private Sub btnConfigurator_Click(sender As Object, e As EventArgs) Handles btnConfigurator.Click
        Try
            Process.Start(gstrRptPath & "\TallyConfigurator\svtTallyExport.exe")
        Catch ex As Exception
            MsgBox("Tally Configurator application not Found." & vbCrLf & " [btnConfigurator_Click]", vbInformation, Me.Text)
        End Try
    End Sub

    Private Sub lblNotify_Click(sender As Object, e As EventArgs) Handles lblNotify.Click
        Dim argFrm As New frmUtlNotification
        argFrm.ShowDialog()
    End Sub

    Private Sub btnDownload_Click(sender As Object, e As EventArgs) Handles btnDownload.Click
        Dim objFrm As New frmUpdateExe
        With objFrm
            .StartPosition = FormStartPosition.CenterScreen
            .Show()
            .Refresh()
            .chkExeVersion()
        End With
    End Sub

    'Private Sub timerNotify_Tick(sender As Object, e As EventArgs) Handles timerNotify.Tick
    '    Exit Sub
    '    Try
    '        Dim svtTempConn As New OleDbConnection, strConn As String
    '        Dim rs As OleDb.OleDbDataReader
    '        strConn = "Provider=SQLOLEDB.1;Persist Security Info=False"
    '        strConn = strConn & ";User ID=" & gStrDBLoginName
    '        strConn = strConn & ";Pwd=" & gStrDBLoginPwd
    '        strConn = strConn & ";Initial Catalog=" & gStrDBName
    '        strConn = strConn & ";Data Source=" & gStrServerName

    '        Using svtTempConn
    '            svtTempConn.ConnectionString = strConn
    '            Try
    '                svtTempConn.Open()
    '            Catch ex As Exception
    '                Exit Sub
    '            End Try


    '            Using cmd As New OleDb.OleDbCommand()
    '                cmd.Connection = svtTempConn
    '                cmd.CommandText = "Select count(*) from UtlNotify where Status='N' and ToUserCode  = '" & gUserCode & "'"
    '                cmd.CommandType = CommandType.Text

    '                rs = cmd.ExecuteReader()

    '                If Not rs.HasRows Then
    '                    rs.Close()
    '                    Exit Sub
    '                End If

    '                rs.Read()
    '                intNotifCount = GetVal(rs.Item(0), 0)
    '                rs.Close()
    '            End Using
    '            lblNotify.Text = intNotifCount
    '        End Using
    '    Catch ex As Exception

    '    End Try
    'End Sub

    Private Sub btnUpdate_Click(sender As Object, e As EventArgs)
        If MsgBox("Application will be closed during updation time. Click yes to proceed.", MsgBoxStyle.YesNo, Me.Text) = MsgBoxResult.No Then Exit Sub
        Dim strFile As String = Application.StartupPath & "\PrjUpdateExe.exe"
        Process.Start(strFile, gstrServerExePath)
        End
    End Sub

    Private Sub lblFin_Click(sender As Object, e As EventArgs) Handles lblFin.Click
        With FrmSelFinYear
            .btnSave.Visible = False
            .ShowDialog()
        End With
    End Sub

    Private Sub btnSettings_Click(sender As Object, e As EventArgs) Handles btnSettings.Click
        Dim argfrm As New FrmUtlGSettings
        OpenForms(argfrm, "GLOBAL SETTINGS...", True)
    End Sub

    Private Sub getVoiceCmdList()
        Dim myList As New List(Of String)()
        strWordList = myList.ToArray()
        Dim rs As OleDb.OleDbDataReader
        rs = clsSvt.getDataReaderFromSQL("Select VoiceCmd from MstVoiceCmd")
        If Not rs.HasRows Then
            rs.Close()
            Exit Sub
        End If
        Dim i As Int32 = 0
        Do While rs.Read
            myList.Add(rs.Item(0))
        Loop
        rs.Close()
        strWordList = myList.ToArray()
    End Sub

    Private Sub sbMic_Click(sender As Object, e As EventArgs) Handles sbMic.Click
        Try
            picVoiceRecg.Visible = True
            picVoiceRecg.BringToFront()
            blnSpeech = True
            blnImgForm = True
            TimerVoiceRecg.Interval = 1
            TimerVoiceRecg.Start()
            Dim recognizer As SpeechRecognitionEngine = New SpeechRecognitionEngine()
            Dim strWordChoice As New Choices(strWordList)
            Dim gram = New Grammar(New GrammarBuilder(strWordChoice))
            recognizer.LoadGrammar(gram)
            Try
startAgain:
                recognizer.SetInputToDefaultAudioDevice()
                Dim result As RecognitionResult = recognizer.Recognize()
                If IsNothing(result) Then
                    If MsgBox("No match.... Do you want Try Again?", MsgBoxStyle.YesNo, Me.Text) = MsgBoxResult.No Then
                        blnSpeech = False
                        Exit Sub
                    End If
                    GoTo startAgain
                End If
                Call getVoiceCmd(result)
                blnSpeech = False
            Catch exception As InvalidOperationException
                blnSpeech = False
                Dim str As String = String.Format("Could not recognize input from default aduio device. Is a microphone or sound card available?" & vbCrLf & "{0} - {1}.", exception.Source, exception.Message)
                MsgBox(str, MsgBoxStyle.Exclamation, Me.Text)
            Finally
                recognizer.UnloadAllGrammars()
            End Try
        Catch ex As Exception
            blnSpeech = False
            MsgBox(ex.Message, MsgBoxStyle.Exclamation, Me.Text)
        End Try
    End Sub

    Private Sub TimeVoiceRecg_Tick(sender As Object, e As EventArgs) Handles TimerVoiceRecg.Tick
        If Not blnSpeech = True Then
            TimerVoiceRecg.Stop()
            picVoiceRecg.Visible = False
            picVoiceRecg.SendToBack()
        End If
    End Sub

    Private Sub getVoiceCmd(ByVal argCmd As RecognitionResult)
        Dim strType As String, intVoiceMenuID As Int32, intMenuType As Int32
        Dim strDocName As String = ""
        Dim rs As OleDb.OleDbDataReader
        rs = clsSvt.getDataReaderFromSQL("Select * from MstVoiceCmd Where VoiceCmd='" & argCmd.Text & "'")
        If Not rs.HasRows Then
            rs.Close()
            Exit Sub
        End If

        rs.Read()
        strType = rs.Item("CMDTYPE")
        intVoiceMenuID = rs.Item("MENUID")
        rs.Close()

        If intVoiceMenuID > 0 Then
            intMenuType = clsSvt.getDataLongValue("Select MenuItemType from MstMenu Where MenuItemID = " & intVoiceMenuID, 0, False)
            strDocName = clsSvt.getDataStringValue("Select MenuItemCaption from MstMenu Where MenuItemID=" & intVoiceMenuID, "", False)
        End If


        Select Case UCase(strType)
            Case "OPEN"
                For i = 0 To dockTabMain.TabPages.Count - 1
                    If dockTabMain.TabPages(i).Text = strDocName Then
                        dockTabMain.TabPages(i).Show()
                        Exit Sub
                    End If
                Next
                Select Case intMenuType
                    Case MnuNode.Masters
                        MasterClick(strDocName)
                    Case MnuNode.Transaction
                        TransactionsClick(strDocName)
                    Case MnuNode.Utilities
                        UtilityClick(strDocName)
                End Select
            Case "CLOSE"
                For i = 0 To dockTabMain.TabPages.Count - 1
                    If dockTabMain.TabPages(i).Text = strDocName Then
                        dockTabMain.TabPages(i).Dispose()
                        Exit Sub
                    End If
                Next
            Case "EXIT"
                End
        End Select
    End Sub

    Private Sub welcomeMsg()
        Try
            'If File.Exists(gstrAudioPath & "\RecordedVoice\Welcome.wma") Then
            '    Dim Player As WMPLib.WindowsMediaPlayer = New WMPLib.WindowsMediaPlayer
            '    Player.URL = gstrAudioPath & "\RecordedVoice\Welcome.wma"
            '    Player.controls.play()
            '    Player.close()
            'End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub btnConfigurator_MouseHover(sender As Object, e As EventArgs) Handles btnConfigurator.MouseHover, btnDesigner.MouseHover, btnDownload.MouseHover
        txtBtnHelp.Text = sender.Text
    End Sub

    Private Sub btnDesigner_MouseLeave(sender As Object, e As EventArgs) Handles btnDesigner.MouseLeave, btnConfigurator.MouseLeave, btnDownload.MouseLeave
        txtBtnHelp.Text = ""
    End Sub

    Private Sub imgDashboard_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles imgDashboard.MouseDoubleClick
        Try
            Dim strDashBrd As String = clsSvt.getDataStringValue("select DashBoardLink from MstComp", "", False)
            If strDashBrd <> "" Then
                Process.Start(strDashBrd)
            Else
                Process.Start("http://192.168.1.100/gateway/")
            End If

        Catch ex As Exception
            MsgBox("Page not Found.", vbInformation, Me.Text)
        End Try
    End Sub

    Private Sub sbFinYear_Click(sender As Object, e As EventArgs) Handles sbFinYear.Click
        With FrmSelFinYear
            .btnSave.Visible = False
            .ShowDialog()
        End With
    End Sub

    Private Sub chkCounterWiseResetEnabled()
        Dim strSql As String = ""
        Dim strcounterWise As String = ""
        Dim strDocFmt As String = ""

        strSql = "Select SetValue from UtlGSettings where SetName = 'CounterWsRst'"
        strcounterWise = clsSvt.getDataStringValue(strSql, "N", False)

        If Trim(UCase(strcounterWise)) = "Y" Then
            gCheckCounter = True
            strDocFmt = clsSvt.getDataStringValue("Select SetValue from UtlGsettings where SetName = 'DocNoFmt' and SetGroup = 'Invoice'", "", False)
            If Trim(strDocFmt) <> "" Then
                If strDocFmt.Contains("<<2>>") Then
                    gCounterWiseFmt = True
                Else
                    gCounterWiseFmt = False
                End If
            End If
        Else
            gCheckCounter = False
        End If
    End Sub

    Private Sub ToolStripStatusLabel2_Click(sender As Object, e As EventArgs) Handles ToolStripStatusLabel2.Click
        If pnlProject.Panels(0).Visible = True Then
            pnlProject.Panels(0).Visible = False
        Else
            pnlProject.Panels(0).Visible = True
        End If
    End Sub

    Public Sub getDefaultPrinter()
        Try
            Dim printDoc As Printing.PrintDocument = New Printing.PrintDocument()
            gStrDefPrinter = ""

            Dim i As Integer
            Dim pkInstalledPrinters As String

            For i = 0 To Printing.PrinterSettings.InstalledPrinters.Count - 1
                pkInstalledPrinters = Printing.PrinterSettings.InstalledPrinters(i)

                If printDoc.PrinterSettings.IsDefaultPrinter Then
                    gstrdefprinterPaperSize = printDoc.PrinterSettings.DefaultPageSettings.PaperSize
                    gStrDefPrinter = printDoc.PrinterSettings.PrinterName
                End If
            Next
        Catch ex As Exception

        End Try

    End Sub



    Private Sub SyncDB()
        Dim strLOGFile As String

        Try
            strLOGFile = clsSvt.getDataStringValue("Select Name from sys.master_files WHERE database_id = db_id() and type = 1", "", False)

            If Trim(strLOGFile) <> "" Then
                clsSvt.execSQLStatement(" USE [master]")
                clsSvt.execSQLStatement(" ALTER DATABASE [" & gStrDBName & "] SET RECOVERY SIMPLE WITH NO_WAIT")
                clsSvt.execSQLStatement(" USE " & gStrDBName)
                clsSvt.execSQLStatement(" DBCC SHRINKFILE(" & strLOGFile & ", 1)")
                clsSvt.execSQLStatement(" USE [master]")
                clsSvt.execSQLStatement(" ALTER DATABASE [" & gStrDBName & "] SET RECOVERY FULL WITH NO_WAIT")
            End If

        Catch ex As Exception
            Exit Sub
        End Try

    End Sub

    Private Sub AddMenusFromDB()
        Dim strShowMenu As String = clsSvt.getDataStringValue("Select ShowMenuIcons from UtlMisc", "N", False)
        Dim strQuery As String

        Dim imageListNo As Integer

        Dim strMenu As String = "", strMenuKey As String = ""
        Dim strMenuCaption As String, strGrpName As String
        Dim strSQL As String = ""

        Dim ndInv As TreeNode
        Dim ndMst As TreeNode
        Dim ndTrn As TreeNode
        Dim ndRpt As TreeNode
        Dim ndTools As TreeNode
        Dim ndUtl As TreeNode

        Dim ndAcc As TreeNode = Nothing
        Dim ndVch As TreeNode = Nothing
        Dim ndAcRpt As TreeNode = Nothing

        With tvMain
            .Nodes.Clear()

            If strShowMenu = "Y" Then
                imgTree.Images.Clear()
                imgTree.Images.Add(New Bitmap(1, 1))
                tvMain.ImageList = imgTree
            End If

            'imgTree.Images.Add(Image.FromFile("D:\svtprojects\vLQMS\bin\Debug\MstMenuImg.png"))
            'imageListNo = imgTree.Images.Count

            If clsSvt.getDataStringValue("select DashBoardReq from UtlMisc ", "N", False) = "Y" Then
                .Nodes.Add("DASHBOARD")  'CNUD
                imgDashboard.Visible = True
            Else
                imgDashboard.Visible = False
            End If

            ndInv = .Nodes.Add("INVENTORY")
            ndMst = ndInv.Nodes.Add("MASTERS", "MASTERS", imageListNo)
            ndTrn = ndInv.Nodes.Add("TRANSACTIONS")
            ndRpt = ndInv.Nodes.Add("REPORTS")

            If UCase(gStrDBName) <> "vFactIC" Then
                If clsSvt.getDataStringValue("select AccTranReq from UtlMisc ", "N", False) = "Y" Then
                    ndAcc = .Nodes.Add("ACCOUNTING")
                    ndVch = ndAcc.Nodes.Add("VOUCHERS")
                    ndAcRpt = ndAcc.Nodes.Add("REPORTS")
                End If
            End If

            ndTools = .Nodes.Add("TOOLS")
            ndUtl = .Nodes.Add("UTILITIES")
            Select Case clsSvt.getDataStringValue("select Custcode from MstComp Where Custcode in ('DMS')", "", False)
                Case ""
                    .Nodes.Add("Screen Docs")
            End Select
            'Select Case Environment.UserName.ToString().ToUpper()
            Select Case gStrSystemName.ToString().ToUpper()
                'Case "SSPL7", "SSPLLAP", "SSPL20", "KUMARDELL", "SSPL21", "MADHUSUDAN", "SSPL19", "EARTH3", "EARTH5", "MARS1", "EARTH2", "SSPLMINISRVR", "JUPITER2", "SSPLUSER_18", "URANUS1", "MARS2", "MARS3", "MARS4", "URANUS3", "VENUS1", "VENUS2", "JUPITER1", "EARTH1"

                Case "SSPL7", "SSPLLAP", "SSPL20", "KUMARDELL", "SSPL21", "MADHUSUDAN", "SSPL19", "EARTH3", "EARTH5", "SELVA", "EARTH2", "SSPLMINISRVR", "GANGADHAR", "SSPLUSER_18", "URANUS1", "MARS2", "MARS3", "MARS4", "URANUS3", "VENUS1", "VENUS2", "JUPITER1", "EARTH1", "ASHWINI", "PRADEEP", "LOKESH"
                    .Nodes.Add("Key Generation")
            End Select
            Select Case Environment.UserName.ToString().ToUpper()
                Case "SSPL7", "SSPLL28", "SSPL20", "KUMARDELL", "SSPL21", "SSPL19", "EARTH3", "EARTH5", "SSPLMINISRVR", "JUPITER2", "JUPITER3", "SSPLUSER_18", "URANUS1", "MARS2", "MARS3", "EARTH1", "GANGADHAR", "SELVA", "ASHWINI", "PRADEEP", "LOKESH"
                    .Nodes.Add("Delete/Remove/Empty Transaction Data..")
            End Select

            .Nodes.Add("About")
            .Nodes.Add("Exit - [Alt + X]")
        End With



        Try
            strSQL = "Select * From MstMenu Where 1 = 1"
            strSQL = strSQL & " And Menushow = 'Y'"
            strSQL = strSQL & " Order By MenuItemType, MenuIndex, MenuGroupName"

            Dim rs As OleDb.OleDbDataReader
            rs = clsSvt.getDataReaderFromSQL(strSQL)

            If Not rs.HasRows Then
                rs.Close()
                Exit Sub
            End If

            While rs.Read()
                strMenuCaption = GetVal(rs("MenuItemCaption"), "") 'Assign Menu Caption

                strMenuCaption = strMenuCaption.Replace("&", "") 'remove & sysmbol for safety purpose

                If strMenuCaption = "-" Then
                    strMenuCaption = strMenuCaption.Replace("-", strSeparator) 'remove & sysmbol for safety purpose
                End If


                strGrpName = GetVal(rs("MenuGroupName"), "") 'Assign group name

                strGrpName = strGrpName.Replace("&", "") 'remove & sysbol for safety porpose

                imageListNo = -1
                If strShowMenu = "Y" Then
                    Try
                        If Not IsDBNull(rs!MenuImage) Then
                            Dim lb() As Byte = rs!MenuImage
                            Dim lstr As New System.IO.MemoryStream(lb)
                            imgTree.Images.Add(Image.FromStream(lstr))
                            imageListNo = imgTree.Images.Count
                        End If
                    Catch ex As Exception
                        imageListNo = -1
                    End Try
                End If

                strMenuKey = "K" & rs!MenuItemID

                Select Case CInt(rs("MenuItemType"))
                    Case MnuNode.Masters
                        Call addSubNodes(ndMst, strGrpName, strMenuCaption, strMenuKey, imageListNo)
                    Case MnuNode.Transaction
                        Call addSubNodes(ndTrn, strGrpName, strMenuCaption, strMenuKey, imageListNo)
                    Case MnuNode.Vouchers
                        Call addSubNodes(ndVch, strGrpName, strMenuCaption, strMenuKey, imageListNo)
                    Case MnuNode.Reports
                        Call addSubNodes(ndRpt, strGrpName, strMenuCaption, strMenuKey, imageListNo)
                    Case MnuNode.AcReports
                        Call addSubNodes(ndAcRpt, strGrpName, strMenuCaption, strMenuKey, imageListNo)
                    Case MnuNode.Tools
                        Call addSubNodes(ndTools, strGrpName, strMenuCaption, strMenuKey, imageListNo)
                    Case MnuNode.Utilities
                        Call addSubNodes(ndUtl, strGrpName, strMenuCaption, strMenuKey, imageListNo)
                End Select
            End While
            rs.Close()

            ndInv.Expand()
            'ndAcc.Expand()
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

        'color grey nodes
        If Not gUserCode = "ADMIN" Then
            strQuery = "Select MstMenu.MenuItemID,MenuItemCaption"
            strQuery = strQuery & " From " & IIf(gblnUserBased, "MstUserRights", "MstRoleRights") & ", MstMenu"
            strQuery = strQuery & " Where LTRIM(RTRIM(" & IIf(gblnUserBased, "UserID", "RoleID") & ")) = " & IIf(gblnUserBased, gUserID, gRoleID) & " And "
            strQuery = strQuery & " " & IIf(gblnUserBased, "MstUserRights", "MstRoleRights") & ".MenuItemID=MstMenu.MenuItemID AND "
            strQuery = strQuery & " " & IIf(gblnUserBased, "UserRights", "RoleRights") & " = '000000000000'"
            strQuery = strQuery & " Union"
            strQuery = strQuery & " Select MstMenu.MenuItemID,MenuItemCaption"
            strQuery = strQuery & " From MstMenu"
            strQuery = strQuery & " Where 1 = 1"
            strQuery = strQuery & " and MstMenu.MenuItemID NOT in"
            strQuery = strQuery & " (select menuitemid from " & IIf(gblnUserBased, "MstUserRights", "MstRoleRights") & " "
            strQuery = strQuery & " where LTRIM(RTRIM(" & IIf(gblnUserBased, "UserID", "RoleID") & ")) = " & IIf(gblnUserBased, gUserID, gRoleID) & ")"
            Dim rstUserRights As Data.OleDb.OleDbDataReader
            rstUserRights = clsSvt.getDataReaderFromSQL(strQuery)
            With rstUserRights
                Do While .Read
                    strMenuKey = "K" & !MenuItemID
                    Dim nodes As TreeNode() = tvMain.Nodes.Find(strMenuKey, True)
                    For Each node In nodes
                        node.ForeColor = Color.Gray
                        node.Tag = "NO"
                    Next
                Loop
                .Close()
            End With
            'NodeSelection(e.Node.Text)
        End If

    End Sub

    Private Sub addSubNodes(ByVal nd As TreeNode, ByVal argGrp As String, ByVal argMenu As String, ByVal argKey As String, ByVal argImg As Int32)
        If nd Is Nothing Then Exit Sub
        Try
            Dim intSubTotMenu As Int32 = 0, ndGroup As TreeNode = Nothing, blnFound As Boolean
            With tvMain
                If argGrp <> "" Then
                    blnFound = False
                    For Each ndLoop As TreeNode In nd.Nodes
                        If ndLoop.Text = argGrp Then
                            ndGroup = ndLoop
                            blnFound = True
                            Exit For
                        End If
                    Next
                    If Not blnFound Then
                        nd.Nodes.Add(argGrp)
                        For Each ndLoop As TreeNode In nd.Nodes
                            If ndLoop.Text = argGrp Then
                                ndGroup = ndLoop
                                Exit For
                            End If
                        Next
                    End If
                End If

                If argGrp = "" Then
                    nd.Nodes.Add(argKey, argMenu, argImg)
                Else
                    ndGroup.Nodes.Add(argKey, argMenu, argImg)
                End If
            End With
        Catch ex As Exception

        End Try
    End Sub


    Private Sub tvMain_KeyUp(sender As Object, e As KeyEventArgs) Handles tvMain.KeyUp
        If e.KeyCode <> Keys.Enter Then
            Exit Sub
        End If

        Dim strMenuKey As String = ""
        Dim rstUserRights As OleDb.OleDbDataReader
        Dim strQuery As String

        If dockTabMain.TabPages.Count = 20 Then
            MsgBox("Maximum Tabs Reached !!! Close a tab", vbInformation, Me.Text)
            Exit Sub
        End If

        If tvMain.SelectedNode.Text = "Exit - [Alt + X]" Then
            blnAppStarted = False
            Call frmMain_FormClosing(sender, Nothing)
        End If

        If tvMain.SelectedNode.Text <> "" Then
            If Not gUserCode = "ADMIN" Then
                strQuery = "Select MstMenu.MenuItemID,MenuItemCaption"
                strQuery = strQuery & " From " & IIf(gblnUserBased, "MstUserRights", "MstRoleRights") & ", MstMenu"
                strQuery = strQuery & " Where LTRIM(RTRIM(" & IIf(gblnUserBased, "UserID", "RoleID") & ")) = " & IIf(gblnUserBased, gUserID, gRoleID) & " And "
                strQuery = strQuery & " " & IIf(gblnUserBased, "MstUserRights", "MstRoleRights") & ".MenuItemID=MstMenu.MenuItemID AND "
                strQuery = strQuery & " " & IIf(gblnUserBased, "UserRights", "RoleRights") & " = '000000000000'"
                strQuery = strQuery & " Union"
                strQuery = strQuery & " Select MstMenu.MenuItemID,MenuItemCaption"
                strQuery = strQuery & " From MstMenu"
                strQuery = strQuery & " Where 1 = 1"
                strQuery = strQuery & " and MstMenu.MenuItemID NOT in"
                strQuery = strQuery & " (select menuitemid from " & IIf(gblnUserBased, "MstUserRights", "MstRoleRights") & " "
                strQuery = strQuery & " where LTRIM(RTRIM(" & IIf(gblnUserBased, "UserID", "RoleID") & ")) = " & IIf(gblnUserBased, gUserID, gRoleID) & ")"

                rstUserRights = clsSvt.getDataReaderFromSQL(strQuery)
                With rstUserRights
                    Do While .Read
                        strMenuKey = "K" & !MenuItemID
                        Dim nodes As TreeNode() = tvMain.Nodes.Find(strMenuKey, True)
                        For Each node In nodes
                            node.ForeColor = Color.Gray
                            node.Tag = "NO"
                        Next
                    Loop
                    .Close()
                End With
                If tvMain.SelectedNode.Tag = "NO" Then
                    MsgBox("ACCESS DENIED for " & tvMain.SelectedNode.Text, MsgBoxStyle.Critical, "Rights not alloted to Menu " & tvMain.SelectedNode.Text)
                    Exit Sub
                Else
                    'tvMain.SelectedNode.ForeColor = Color.White
                    'tvMain.SelectedNode.Tag = "YES"
                    NodeSelection(tvMain.SelectedNode.Text)
                End If
                If tvMain.SelectedNode.Text = "Exit - [Alt + X]" Then
                    blnAppStarted = False
                    Call frmMain_FormClosing(sender, Nothing)
                End If
                'NodeSelection(e.Node.Text)
            End If
            NodeSelection(tvMain.SelectedNode.Text)
        End If
    End Sub

    Private Sub ExecReWriteStock()
        Dim strReWrite As String = "", strType As String = ""
        Dim strSQL As String

        strSQL = "if exists( select itemid, sum(pQty) - sum(uslpbqty) as diffQty from ("
        strSQL &= " Select itemid, sum(pbQty) pqty, 0 As USLPbQty from trndtlpb d group by itemid"
        strSQL &= " union all"
        strSQL &= " Select itemid, 0, sum(Qty) As USLPbQty from UtlStockList where upper(DocType) in('PB','SA','ST')"
        strSQL &= " group by itemid) a "
        strSQL &= " group by itemid"
        strSQL &= " having sum(pQty) <> sum(uslpbqty)) select 'Y' else select 'N'"
        strReWrite = clsSvt.getDataStringValue(strSQL, "", False)

        If strReWrite = "Y" Then
            With frmStkRewrite
                .initScreen("FULL")
                If gUserCode <> "ADMIN" Then .btnReWrite.Visible = False
                .ShowDialog()
            End With
            Exit Sub
        End If





        strSQL = "if exists( select itemid, sum(pQty) - sum(uslInvqty) as diffQty from ("
        strSQL &= " Select itemid, sum(SInvQty) pqty, 0 As USLInvQty from TrndtlSInv d, trnHdrSINV h"
        strSQL &= " where h.SinvID = d.SinvID"
        strSQL &= " AND h.Cancelled = 'N' "
        strSQL &= " And h.IsDelete = 'N'"
        strSQL &= " group by itemid"
        strSQL &= " union all"
        strSQL &= " Select itemid, 0, sum(Qty) As USLInvQty from UtlStockList where Upper(DocType) = 'INV'"
        strSQL &= " group by itemid) a "
        strSQL &= " group by itemid"
        strSQL &= " having sum(pQty) <> sum(uslInvqty)) select 'Y' else select 'N'"
        strReWrite = clsSvt.getDataStringValue(strSQL, "", False)
        If strReWrite = "Y" Then
            With frmStkRewrite
                .initScreen("FULL")
                If gUserCode <> "ADMIN" Then .btnReWrite.Visible = False
                .ShowDialog()
            End With
            Exit Sub
        End If


        If fnTablePresent("TRNHDRSR") Then
            strSQL = "if exists( select itemid, sum(pQty) - sum(uslSRqty) as diffQty from ("
            strSQL &= " Select itemid, sum(SRQty) pqty, 0 As USLSRQty from trndtlSR d group by itemid"
            strSQL &= " union all"
            strSQL &= " Select itemid, 0, sum(Qty) As USLSRQty from UtlStockList where Upper(DocType) = 'SR'"
            strSQL &= " group by itemid) a "
            strSQL &= " group by itemid"
            strSQL &= " having sum(pQty) <> sum(uslSRqty)) select 'Y' else select 'N'"
            strReWrite = clsSvt.getDataStringValue(strSQL, "", False)
            If strReWrite = "Y" Then
                With frmStkRewrite
                    .initScreen("FULL")
                    If gUserCode <> "ADMIN" Then .btnReWrite.Visible = False
                    .ShowDialog()
                End With
                Exit Sub
            End If

        End If

        strSQL = "if exists( select itemid, sum(pQty) - sum(uslStkDedqty) as diffQty from ("
        strSQL &= " Select itemid, sum(StkDedQty) pqty, 0 As USLStkDedQty from trndtlStkDed d group by itemid"
        strSQL &= " union all"
        strSQL &= " Select itemid, 0, sum(Qty) As USLStkDedQty from UtlStockList where upper(DocType) = 'STKDED'"
        strSQL &= " group by itemid) a "
        strSQL &= " group by itemid"
        strSQL &= " having sum(pQty) <> sum(uslStkDedqty)) select 'Y' else select 'N'"
        strReWrite = clsSvt.getDataStringValue(strSQL, "", False)
        If strReWrite = "Y" Then
            With frmStkRewrite
                .initScreen("FULL")
                If gUserCode <> "ADMIN" Then .btnReWrite.Visible = False
                .ShowDialog()
            End With
            Exit Sub
        End If

        strSQL = "if exists( select itemid, sum(pQty) - sum(uslqty) as diffQty from ("
        strSQL &= " Select itemid, sum(StkMultiplier * Qty) pqty, 0 as UslQty from UtlStockList d group by itemid"
        strSQL &= " union all"
        strSQL &= " Select itemid, 0, clbal as UslQty from TrnQOH) a "
        strSQL &= " group by itemid"
        strSQL &= " having sum(pQty) <> sum(uslqty)) select 'Y' else select 'N'"
        strReWrite = clsSvt.getDataStringValue(strSQL, "", False)
        If strReWrite = "Y" Then
            With frmStkRewrite
                .initScreen("QOH")
                If gUserCode <> "ADMIN" Then .btnReWrite.Visible = False
                .ShowDialog()
            End With
            Exit Sub
        End If
    End Sub

    Private Sub lblSalesValue_Click(sender As Object, e As EventArgs) Handles lblSalesValue.Click
        Dim dblTotalSales As Double = clsSvt.getDataDoubleValue("Select Sum(InvValue) From TrnHdrSINV Where IsDelete = 'N' and SINVDate='" & getDateFormatInInsert(getSystemDate()) & "'", 0, False)
        Dim intTotalBills As Integer = clsSvt.getDataDoubleValue("Select Count(InvValue) From TrnHdrSINV Where IsDelete = 'N' and  SINVDate='" & getDateFormatInInsert(getSystemDate()) & "'", 0, False)
        lblSalesValue.Text = "Total Bills: " & intTotalBills & " & Sales: " & dblTotalSales
    End Sub

    Public Sub SetCultureInfo()
        Dim myCulture As New Globalization.CultureInfo("EN-US")    '<-- set this to match the nearest default culture. (US-EN = US English etc.)

        'set here whatever custom formats you want for your applicaiton.
        With myCulture.DateTimeFormat
            .ShortDatePattern = "dd/MM/yyyy"
            .LongDatePattern = "dddd, MMMM dd, yyyy"
            .ShortTimePattern = "h:mm:ss tt"
        End With
        With myCulture.NumberFormat
            .CurrencySymbol = ""
            .CurrencyDecimalSeparator = "."
            .CurrencyDecimalDigits = 2
        End With

        ''finally apply this customized culture to your application.
        Application.CurrentCulture = myCulture
    End Sub

    Private Sub sbCalci_Click(sender As Object, e As EventArgs) Handles sbCalci.Click
        frmUtlCalci.ShowDialog()
    End Sub

    Private Sub updateToCloudDB()
        Dim strFileName As String
        Dim webConn As New OleDb.OleDbConnection
        'Dim webConnString As String = "Provider=SQLOLEDB.1;Persist Security Info=False;User ID=sa;Pwd=svtSQLadmn@2020;Initial Catalog=Softvent;Data Source=103.12.1.88\SQLEXPRESS,62350"
        ' Dim webConnString As String = "Provider=SQLOLEDB.1;Persist Security Info=False;User ID=sa;Pwd=svtSQLadmn@2020;Initial Catalog=Softvent;Data Source=103.38.50.169\SQLExpress,62781"
        Dim webConnString As String = "Provider=SQLOLEDB.1;Persist Security Info=False;User ID=SoftsqlAdmin;Pwd=Ydp$wGdu@963ScA#;Initial Catalog=Softvent;Data Source=103.38.50.154\SQLExpress,62789"
        webConn.ConnectionString = webConnString
        Try
            webConn.Open()
        Catch ex As Exception
            Exit Sub
        End Try

        strFileName = Application.StartupPath & "\vLQMS.exe"

        Try
            Dim fs As FileStream

            fs = New FileStream(strFileName, FileMode.Open, FileAccess.Read)
            Dim docByte As Byte() = New Byte(fs.Length - 1) {}
            fs.Read(docByte, 0, System.Convert.ToInt32(fs.Length))
            fs.Close()

            Dim cmd As OleDb.OleDbCommand
            strSQL = "Update MstVLQMS set vLQMSAppFile = ?"
            cmd = New OleDb.OleDbCommand(strSQL, webConn)
            cmd.CommandTimeout = 0
            cmd.Parameters.AddWithValue("@Image", docByte)
            If cmd.ExecuteNonQuery() Then
                Call clsSvt.execSQLStatement("Update MstVLQMS set vLQMSFileVer = '" & getSystemDateTime.ToString("ddMMyyhhmmss") & "'", Nothing, webConn)
                MsgBox("Exe updated to cloud successfully!", vbInformation, Me.Text)
            End If
            webConn.Close()
        Catch ex As Exception
            MsgBox("Error during cloud updation." & ex.Message, MsgBoxStyle.Critical, Me.Text)
        End Try
    End Sub

    Private Sub alterReWriteStkSP()
        clsSvt.execSQLStatement("
ALTER PROCEDURE [dbo].[mSp_RewriteStkList] 
	@argType varchar(50), @Retval int output
AS
--exec mSp_RewriteStkList 
BEGIN
		set @Retval = 0; --FAIL

		delete from trnDtlPB where PBID not in (Select PBID from trnHdrPB)
		delete from trnDtlsInv where sInvID not in (Select SinvID from TrnHdrSINV)
		delete from TrnDtlSTKDED where STKDEDID not in (Select stkdedID from TrnHdrSTKDED)

		if @argType = 'FULL' 
		BEGIN
			BEGIN TRY
				
				update UtlMisc set ReWriteINPROGRESS = 'Y'
				BEGIN TRAN TRANStkList
					select * into #UList from UtlStockList where 1 <> 1

					Insert Into #UList 
					(ItemID, StkMultiplier, DocType, DocDate, DocID, DocNo, Particulars, Qty, LtrQty, Rate, locid, delflag, rewrite, FifoUsedQty, DocValue, FifoRate) 
					Select ItemID, 1 as StkMultiplier, 
					case when Purchasetype = 'Purchase' then 'PB' 
						 when Purchasetype = 'Stock Addition' then 'SA' 
						 when Purchasetype = 'Stock Transfer' then 'ST' end 
					as docType, PBDate, h.PBID, PBNO, PartyName, PBQty, IsNull(PBQtyInLtrs,0), PBRate, 0 , 'N', 'N', 0, 0, 0
					From TrnDtlPB d, TrnHdrPB h 
					Where d.PBID=h.PBID 
					AND h.Cancelled = 'N' 
					select  *from trnhdrpb

					--Sales Invoice -  Reduce from Stock
					Insert Into #UList 
					(ItemID, StkMultiplier, DocType, DocDate, DocID, DocNo, Particulars, Qty, LtrQty, Rate, locid, delflag, rewrite, FifoUsedQty, DocValue, FifoRate) 
					Select ItemID, '-1', 'INV', SINVDate, h.SINVID, SINVNO, PartyName, SINVQty, IsNull(SINVQtyInLtrs,0), 0,0, 'N', 'N' , 0, 0, 0
					From TrnDtlSINV d, TrnHdrSINV h 
					Where d.SINVID=h.SINVID 
					AND h.Cancelled = 'N' 
					AND h.IsDelete = 'N'

					--Stock Deduction -  Reduce from Stock
					Insert Into #UList 
					(ItemID, StkMultiplier, DocType, DocDate, DocID, DocNo, Particulars, Qty, LtrQty, Rate, locid, delflag, rewrite, FifoUsedQty, DocValue, FifoRate) 
					Select ItemID, '-1', 'STKDED', STKDEDDate, h.STKDEDID, STKDEDNO, '', STKDEDQty, IsNull(STKDEDQtyInLtrs,0), 0,0, 'N', 'N' , 0, 0, 0
					From TrnDtlSTKDED d, TrnHdrSTKDED h 
					Where d.STKDEDID=h.STKDEDID 
					AND h.Cancelled = 'N' 

					--sales return
					if exists(select * from sys.tables t where upper(t.name) = 'TRNHDRSR')
					begin
						Insert Into #UList 
						(ItemID, StkMultiplier, DocType, DocDate, DocID, DocNo, Particulars, Qty, LtrQty, Rate, LOCID, delflag, rewrite, FifoUsedQty, DocValue, FifoRate)  
						Select ItemID, 1 as StkMultiplier, 
						'SR' as docType, SRDate, h.SRID, SRNO, '', SRQty, IsNull(SRQtyInLtrs,0), SRRate, 0, 'N', 'N' , 0, 0, 0
						From TrnDtlSR d, TrnHdrSR h 
						Where d.SRID=h.SRID 
						AND h.Cancelled = 'N' 
					end

					select itemid, sum(stkmultiplier *  qty) as negStk 
					into #Neg
					from #uList 
					group by itemid 
					having sum(stkmultiplier *  qty) < 0

					if exists(select top 1 * from #neg where ItemID > 0) 
						begin
							--do not insert data for negative items
							declare @i int;
						end
					else
						begin
							--write to utlStock
							truncate table trnqoh
							truncate table UtlStockList

							-- Disable all table constraints
							ALTER TABLE TrnQOH NOCHECK CONSTRAINT ALL

							Insert Into UtlStockList
							(ItemID, StkMultiplier, DocType, DocDate, DocID, DocNo, Particulars, Qty, LtrQty, Rate, locid, delflag, rewrite) 
							select ItemID, StkMultiplier, DocType, DocDate, DocID, DocNo, Particulars, Qty, LtrQty, Rate, locid, delflag, rewrite
							from #UList 
							order by docdate, itemid, StkMultiplier desc, doctype, docid

							truncate table TrnQoh
		
							insert into TrnQOH (itemid, LocId, clBal)
							select itemid, 1, sum(qty * stkmultiplier) from #UList group by itemid having sum(qty * stkmultiplier) >= 0

							-- Enable all table constraints
							ALTER TABLE TrnQOH CHECK CONSTRAINT ALL
						end
 
					/*SELECT rowid, docdate, itemid, doctype, StkMultiplier, qty,  SUM (qty * stkmultiplier) OVER  (PARTITION BY ItemID 
					ORDER BY docdate, itemid, StkMultiplier desc, doctype, docid)  AS RunningAgeTotal
					FROM #ulist
					order by docdate, itemid, StkMultiplier desc, doctype, docid
					*/
					
				COMMIT TRAN TRANStkList
				update UtlMisc set ReWriteINPROGRESS = 'N'
				set @Retval = 1; --SUCCESSS
			END TRY

			BEGIN CATCH
				update UtlMisc set ReWriteINPROGRESS = 'N'
				set @Retval = 0; --FAIL
				select ERROR_MESSAGE()
				ROLLBACK TRAN TRANStkList
			END CATCH
		END
		if @argType = 'QOH' 
		BEGIN
			truncate table TrnQoh
			
			insert into TrnQOH (itemid, LocID, clBal)
			select itemid, 1, sum(qty * stkmultiplier) from UtlStockList group by itemid having sum(qty * stkmultiplier) >= 0
			set @Retval = 1; --SUCCESSS
			update UtlMisc set ReWriteINPROGRESS = 'N'
		END
END")
    End Sub

    'Private Sub truncateLog()
    '    Dim strSQL As String, strLogName As String
    '    Dim dblSize As Double
    '    Try
    '        strLogName = clsSvt.getDataStringValue("SELECT name FROM sys. master_files where  physical_name like '%" & gStrDBName & "_log%'", "", False)
    '        dblSize = clsSvt.getDataDoubleValue("SELECT  size * 8 / 1024 / 1024 FROM sys.database_files where name = '" & strLogName & "'", 0, False)
    '        If dblSize >= 1 Then
    '            strSQL = "ALTER DATABASE " & gStrDBName & " SET RECOVERY SIMPLE WITH NO_WAIT;"
    '            strSQL = strSQL & " DBCC SHRINKFILE (N'" & strLogName & "' , 1); use master; ALTER DATABASE " & gStrDBName & " SET RECOVERY FULL WITH NO_WAIT use " & gStrDBName
    '            Call clsSvt.execSQLStatement(strSQL)
    '        End If
    '    Catch ex As Exception
    '        MsgBox("Error during Log truncation. " & ex.Message, MsgBoxStyle.Critical, Me.Text)
    '    End Try
    'End Sub

    Private Sub truncateLog()
        If gStrSystemName.ToUpper = "PLUTO" Or gStrSystemName.ToUpper = "EARTH5" Or gStrSystemName.ToUpper = "VENUS2" Then Exit Sub

        Select Case gStrServerName.ToUpper
            Case "IBMSSPLSRVR", "SSPLMINISRVR", "VENUS2\SQLEXPRESS", "IBMSSPLSRVR\SQL19", "IBMSSPLSRVR\SPR14", "IBMSSPLSRVR\MSSQLSERVER01"
                Exit Sub
            Case Else

        End Select
        Dim strSQL As String
        Dim dtDBInfo As DataTable
        strSQL = " SELECT 
      'USE [' + d.name + N']' + CHAR(13) + CHAR(10) 
    + ' ALTER DATABASE ['+ d.name +'] SET RECOVERY SIMPLE WITH NO_WAIT'
    + ' USE [' + d.name + N']' + CHAR(13) + CHAR(10) 
    + ' DBCC SHRINKFILE ([' + mf.name + '] , 1)' 
    + ' use master'
    + ' ALTER DATABASE ['+ d.name +'] SET RECOVERY FULL WITH NO_WAIT'
    + ' use [' + d.name + ']'
    + CHAR(13) + CHAR(10) + CHAR(13) + CHAR(10) 
        FROM 
         sys.master_files mf 
    JOIN sys.databases d 
        ON mf.database_id = d.database_id 
    WHERE d.database_id > 4
    and mf.type_desc = 'LOG' order by 1  "

        dtDBInfo = clsSvt.getDATATable(strSQL)
        For i As Int32 = 0 To dtDBInfo.Rows.Count - 1
            strSQL = dtDBInfo.Rows(i).Item(0)
            clsSvt.execSQLStatement(strSQL)
        Next
        clsSvt.execSQLStatement("use " & gStrDBName)

    End Sub
    Private Sub Dashboardclick(argmenu As String)
        Dim argform As New frmUtlDashboard
        Me.OpenForms(argform, "Dashboard", True)
        pnlProject.Panels(0).Visible = False
    End Sub

    Private Sub imgDashboard_Click(sender As Object, e As EventArgs) Handles imgDashboard.Click
        Dim objFrm As New frmUtlDashboard
        Me.OpenForms(objFrm, "Dashboard", True)
    End Sub

    Private Sub ExpandAllMenusToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExpandAllMenusToolStripMenuItem.Click
        tvMain.ExpandAll()
    End Sub

    Private Sub ToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles CollapseAllMenusToolStripMenuItem1.Click
        tvMain.CollapseAll()
    End Sub

    Private Sub sbCalci_MouseHover(sender As Object, e As EventArgs) Handles sbCalci.MouseHover
        sbCalci.ToolTipText = "Simple Calci"
        sbCalci.AutoToolTip = True
    End Sub

    Private Sub chkMinStock()
        Dim strSQL As String
        strSQL = "Select count(itemid) from ("
        strSQL = strSQL & " Select s.itemid, sum(Qty*StkMultiplier) as clQty, i.minLevel  "
        strSQL = strSQL & " from MstItem i, UtlStockList s"
        strSQL = strSQL & " where s.ItemID = i.ItemID"
        strSQL = strSQL & " And i.MinLevel > 0 "
        strSQL = strSQL & " And DocDate <= " & getDateFormatInCond(getSystemDate)
        strSQL = strSQL & " group by s.itemid, i.minlevel"
        strSQL = strSQL & " Having sum(Qty*StkMultiplier) < i.MinLevel) a "
        If clsSvt.getDataLongValue(strSQL, 0, False) > 0 Then
            Dim argFrm As New frmRptStkLevel
            Call OpenForms(argFrm, "Min Level Report")
            argFrm.btnView.PerformClick()
        End If
    End Sub

    Private Sub sbDemo_Click(sender As Object, e As EventArgs) Handles sbDemo.Click
        Clipboard.SetText(sbDemo.Text)
        MsgBox("Copied Key info to clipboard", MsgBoxStyle.Information, Me.Text)
    End Sub

    Private Sub RemovePreviousQrfiles()
        If Not Directory.Exists(gstrQRImagePath) Then Exit Sub

        Dim eachFileInMydirectory As New DirectoryInfo(gstrQRImagePath)
        Dim dt As Date = getSystemDate()
        Dim fileName As IO.FileInfo
        Try
            For Each fileName In eachFileInMydirectory.GetFiles
                If (dt - fileName.CreationTime).Days > 1 Then
                    fileName.Delete()
                End If
            Next
        Catch ex As Exception

        End Try
    End Sub

    Private Sub timerNotify_Tick(sender As Object, e As EventArgs) Handles timerNotify.Tick

    End Sub

    Private Sub ChkFinYearCreation()
        Dim dtCurDate As Date = getSystemDate()
        Dim dtLastFinYearDate As Date = clsSvt.getDataDateValue("Select Max(YrTDate) From MstCompFinYear", Nothing, False)

        If dtCurDate <= dtLastFinYearDate Then Exit Sub

        Dim obj As New frmMstCreateFinYear
        obj.StartPosition = FormStartPosition.CenterScreen
        obj.ShowDialog()

    End Sub

    Private Sub dockTabMain_TabPageClosing(sender As Object, e As TabPageCancelEventArgs) Handles dockTabMain.TabPageClosing
        If e.TabPage.Tag <> "" Then
            MsgBox("Cannot Close tab, as Action is pending!", MsgBoxStyle.Critical, "Cannot close Tab")
            e.Cancel = True
        End If
    End Sub
End Class