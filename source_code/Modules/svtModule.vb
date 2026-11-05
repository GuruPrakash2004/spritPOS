Imports System.Management ' requires adding a reference to System.Management
Imports System.Data.oledb
Imports System.DirectoryServices
Imports System.Drawing.Drawing2D
Imports System.IO
Imports System.Net
Imports System.Net.Mail
Imports System.Text
Imports System.Text.RegularExpressions
Imports C1.Win
Imports C1.Win.C1FlexGrid
Imports Microsoft.Office
Imports Microsoft.Office.Interop
Imports Microsoft.Win32
Imports System.Globalization
Imports SSPLLic
Imports svtLicence
Public Class Crc32
    Shared table As UInteger()

    Shared Sub New()
        Dim poly As UInteger = &HEDB88320UI
        table = New UInteger(255) {}
        Dim temp As UInteger = 0
        For i As UInteger = 0 To table.Length - 1
            temp = i
            For j As Integer = 8 To 1 Step -1
                If (temp And 1) = 1 Then
                    temp = CUInt((temp >> 1) Xor poly)
                Else
                    temp >>= 1
                End If
            Next
            table(i) = temp
        Next
    End Sub

    Public Function getCurrencyRate(ByVal currFrom As String, ByVal currTo As String) As Decimal
        Dim result As Decimal = 0.0
        Using c As New WebClient
            Try
                Dim data As String = c.DownloadString(String.Format("http://download.finance.yahoo.com/d/quotes.csv?s={0}{1}=X&f=sl1d1t1ba&e=.csv", currFrom, currTo))
                Dim rate As String = data.Split(","c)(1)
                Dim style = NumberStyles.Number
                Dim culture = CultureInfo.CreateSpecificCulture("en-US")
                Decimal.TryParse(rate, style, culture, result)
            Catch ex As Exception
                'MsgBox("Conversion rate not applied! Please check your internet connection!", vbInformation, "Softvent...")
                result = 1
            End Try
        End Using
        Return result
    End Function

    Public Shared Function ComputeChecksum(ByVal bytes As Byte()) As UInteger
        Dim crc As UInteger = &HFFFFFFFFUI
        For i As Integer = 0 To bytes.Length - 1
            Dim index As Byte = CByte(((crc) And &HFF) Xor bytes(i))
            crc = CUInt((crc >> 8) Xor table(index))
        Next
        Return Not crc
    End Function

End Class


Public Module svtModule

    Public webConnString As String = "Provider=SQLOLEDB.1;Persist Security Info=False;User ID=svtadmin;Pwd=svt@123$$;Initial Catalog=WebVLQMS;Data Source=103.231.43.48,16052"

    Public blnAllowToWork As Boolean = False
    Public gIntInvCtr As Integer

    'for connection
    Public gStrSystemName As String = ""
    Public gStrSystemUserName As String = ""
    Public gStrSystemIPAddress As String = ""
    Public strWebCamFile As String = ""
    Public svtConn As oledbConnection
    Public gstrConnString As String

    Public gDblPurSellVarPer As Double
    Public gStrWatsAppMsg As String = ""
    Public gStrWatsAppFile As String = ""
    Public gStrWatsAppRcvrNo As String = ""

    Public gstrBold As String
    Public gstrItalic As String
    Public gstrUnderline As String
    Public gStrEmoji As String
    Public gBlnAdvAdminLogin As Boolean
    Public gBlnCheckForFraud As Boolean
    Public strConnString As String = ""
    Public gstrDataPath As String = ""
    Public gstrRptPath As String = ""
    Public gstrServerExePath As String = ""
    Public gstrAutoSave As String = ""
    Public gstrItemImagePath As String = ""
    Public gstrQRImagePath As String = ""
    Public gstrCPImagePath As String = ""
    Public gstrPartyImagePath As String = ""
    Public gStrHelpFilePath As String = ""
    Public gStrUtlFldLst As String = ""
    Public gBranchType As String = ""
    Public gBlnMnuFilled As Boolean
    Public gstrVsfItemCodeCols As String = ""
    Public blnAppStarted As Boolean
    Public gAppStarted As Boolean = False
    Public gstrAudioPath As String
    Public gStrDefPrinter As String = ""
    Public gstrdefprinterPaperSize As New System.Drawing.Printing.PaperSize
    Public gCompMobNo As String = ""
    Public gdblBillValueGreaterThan As Double
    Public gIntAlertIfPointsGreaterThan As Integer
    Public gDblPointsForOneRs As Double
    Public gDblRsForOnePoint As Double
    Public blnLoyalty As Boolean

    Public gBlnAllowNegativeStock As Boolean
    Public gBtnCtrlBackClr As Long = 0
    Public gBtnCtrlForeClr As Long = 0
    Public Const AccStringFmt As String = "##.00 Dr;##.00 Cr;0.00"
    Public gblnADAuth As Boolean
    Public gblnADAutolog As Boolean
    Public gblnShowRememberMe As Boolean
    Public gblnPwdRule As Boolean
    Public gblnPwdPolicy As Boolean
    Public gblnUserBased As Boolean
    Public gblnPwdNeveExp As Boolean
    'for sql and mysql connection
    Public gBtnFlatStyle As String
    Public gBtnBackColor As Color
    Public gBtnForeColor As Color
    Public gstrConnectionPath As String = ""
    Public gBlnConnectionPath As Boolean = False
    'Public gBlnINIFileCreation As Boolean = False

    'Public gSMSU As String
    ' Public gSMSP As String
    'Public gSMSURL As String
    'Public gSENDER As String
    Public gCompCode As String
    Public gLoginReq As String
    Public gStrDummy As String
    Public gBlnMSOffice As Boolean

    Public gDefProviderID As Integer
    Public gDefProvider As String
    Public gSMSURL As String
    Public gIntDailyLimit As Int64

    'Public gIntDailyLimit As Int64
    Public gIntLimit As Int64
    Public gFormColor As String
    Public gPnlBackColor As Color

    Public gIntMsgLen As Int32

    Public gStrServerName As String = ""
    Public gStrGlobeDBName As String = ""
    Public gStrDBName As String = ""
    Public gStrDBLoginName As String = ""
    Public gStrDBLoginPwd As String = ""
    'for audit track
    Public gStrLogDBName As String

    Public gblnUseState As Boolean
    Public gblnUseCountry As Boolean
    Public gblnPartyEditTrn As Boolean
    Public gblnShowMultiAddTrn As Boolean
    Public gBlnAuditLog As Boolean
    Public gTallyUpdate As String

    Public gDblBeerLmt As Double = 0
    Public gDblWineLmt As Double = 0
    Public gDblIMLlmt As Double = 0

    'Public gStrQryCust As String
    'Public gStrQrySupp As String
    Public gCustQryFlds As String
    Public gSuppQryFlds As String
    Public gStrChkValue As String
    'Public gCustQryCond As String
    'Public gSuppQryCond As String

    Public gBlnPurBill4Deci As Boolean = False

    Public gstrInvPrintFromSP As String
    Public gstrInvPrintFromXL As String

    Public gblnItemTable As Boolean = False

    Public Const gStrMainFormCaption = ""
    'Public Const gCnstAccess = "ACCESS"
    'Public Const gCnstSQL = "SQL"
    'Public Const gStrConnectionType = "SQL"

    Public intAppIdleTime As Integer

    Public Const vAccFldLedName = 0
    Public Const vAccFldValue = 1
    Public Const vAccDrCr = 2
    Public Const vAccLedName = 3
    Public Const vAccValue = 4
    Public Const vAccLedID = 5
    Public Const vAccDrValue = 6
    Public Const vAccCrValue = 7
    Public DocID As String
    Public DocNo As String

    'Mainform & child form colours
    Public gMainForeColor As Color
    Public gMainBackColor As Color
    Public gMainFontName As Font

    Public gChildMandatoryFore As Color
    Public gChildReadOnlyFore As Color
    Public gChildOptionalFore As Color
    Public gChildFormBackColor As Color
    Public gStrServerExeFileName As String
    Public gStrLocalExeFileName As String

    'From Utl Misc flds Caption
    Public gItemCode As String
    Public gItemName As String
    Public gItemDesc As String
    Public gIntTopNrows As Int32

    Public gintFontSize As Int32
    'Public dtItems As DataTable
    'Public dtCustomer As DataTable
    Public blnSMSFeature As Boolean

    Public gBlnSMSInvSales As Boolean
    Public gBlnSMSInvCancelled As Boolean
    Public gBlnSMSInvDeleted As Boolean

    Public gdblSalesValLmt As Double
    Public gstrSalesSmsMsg As String
    Public gStrRptEmailID As String
    Private strGetPlainText As String
    Public gstrCancelledSMSMsg As String
    Public gstrDeleteSMSMsg As String
    Public gstrChkRateFld As String
    Public gStrSalesMobNo As String

    Public gblnAuto As Boolean

    Public gdtFreezeDate As Date

    Public gSqlGetDate As String

    Dim converter As System.ComponentModel.TypeConverter =
        System.ComponentModel.TypeDescriptor.GetConverter(GetType(Font))

    'for storing company settings
    Structure tSettings
        Dim gCompNo As Integer
        Dim gCompName As String
        Dim gCompAdd1 As String
        Dim gCompAdd2 As String
        Dim gCompAdd3 As String
        Dim gCompAdd4 As String
        Dim gCompAdd5 As String
        Dim gCompStDt As Date
        Dim gCompEndDt As Date
        Dim gCompPHNO As String
        Dim gCompGSTIN As String
        Dim gCompChqDesg As String
        Dim gCompFinYear As String
        Dim gMDBName As String

    End Structure

    Dim _src As C1FlexGrid

    Structure bcShortcutKeyas
        Dim SaveKey As String
        Dim AddKey As String
        Dim EditKey As String
        Dim PrintKey As String
        Dim CloseKey As String
        Dim FindKey As String
        Dim CancelKey As String
        Dim DeleteKey As String
        Dim OptionsKey As String
        Dim MoveFirstKey As String
        Dim MoveLastKey As String
        Dim MoveNextKey As String
        Dim MovePrevKey As String
    End Structure

    'Declaration for specifying the position of a particular right
    Public Enum chkRightsPos
        AddRights = 1
        EditRights = 2
        DelRights = 3
        PrintRights = 4
        ViewRights = 5
        AuthRights = 6
        Auth1Rights = 7
        opt1Rights = 8
        opt2Rights = 9
        opt3Rights = 10
        opt4Rights = 11
        opt5Rights = 12
    End Enum

    Public Enum MnuNode
        DashBoard = 0
        Masters = 1
        Transaction = 2
        Vouchers = 3
        Reports = 4
        AcReports = 5
        Tools = 6
        Utilities = 7
        Help = 8
        About = 9
        Ext = 10

    End Enum

    Public Enum UserActions
        AddAction = 0
        EditAction = 1
        DeleteAction = 2
    End Enum

    Public Enum KeyboardType
        AlphaNumeric = 0
        AlphaOnly = 1
        NumericOnly = 2
    End Enum

    Public Enum chartStyles
        XYPlot
        Pie
        Bar
        Area
        Polar
        Radar
        Bubble
        HiLo
        HiLiOpenClose
        Candle
        Gantt
        [Step]
        Histogram
    End Enum

    Public gstrCurName As String, gstrCurDecName As String, gCurID As Integer, gstrCurCode As String
    Public gLngHoldID As Integer

    Public Enum svtAlign
        Left
        Center
        Right
    End Enum
    Public Enum typeLayoutStyles
        Horizontal
        Vertical
    End Enum


    Public gSysTempPath As String

    Public gSettings As tSettings
    Public gBCShortCutKey As bcShortcutKeyas

    'for user tracking
    Public gUserID As Integer
    Public gUserCode As String
    Public gRoleID As Integer

    Public gCounterWiseFmt As Boolean
    Public gCheckCounter As Boolean
    Public gStrTempFormName As String
    Public gDispDashboard As String

    Public argAuditReq As String
    Public gblnVoucherPresent As Boolean


    'for ini file
    Public Const gINIFileName = "Factic.ini"

    Public Enum DataTypeKeyValidation
        svtInt
        svtDate
        svtString
        svtDecimal
    End Enum
    'for handling different database types
    Public Const gDbTypeAccess = "ACCESS"
    Public Const gDbTypeMySQL = "MYSQL"
    Public Const gDbTypeMSSQL = "MS-SQL"

    ' Public Const gDBTypeCurrent = "ACCESS"
    Public Const gDBTypeCurrent = "MS-SQL"

    Dim strNeg As String = "Minus "

    'Mail Details
    Public gSmtpMailID As String
    Public gSmtpMailPwd As String
    Public gSmtpHostName As String
    Public gSmtpServerPort As String
    Public gSmtpEnableSSL As String
    Public gSmtpEnableTLS As String
    Public gRptEmailIds As String

    Public gBlnTableAlphaNumeric As Boolean

    Public Enum svtBtns
        svtSave = 0
        svtNew = 1
        svtEdit = 2
        svtDelete = 3
        svtCancel = 4
        svtOptions = 5
        svtPrint = 6
        svtClose = 7
        svtHelp = 8
        svtMoveFirst = 9
        svtMovePrev = 10
        svtFind = 11
        svtMoveNext = 12
        svtMoveLast = 13
    End Enum

    Public Enum svtDtlBtns
        svtInsert = 1
        svtRemove = 2
        svtCopy = 3
        svtPaste = 4
        svtUp = 5
        svtDown = 6
        svtReplace = 7
        svtExport = 8
        svtImport = 9
        svtTemplate = 10
        svtAttach = 11
        svtRestore = 12
        svtCopyFrom = 13
        svtAmmend = 14
        svtFreeze = 15
        svtAddItems = 16
        svtTender = 17
        svtInsertDoc = 18
        svtMultiDoc = 19
        svtTrnExport = 20
        svtBOM = 21
        svtCalTotal = 22
        svt23 = 23
        svt24 = 24
        svtPending = 25
        svtResize = 26
        svtAddnl = 27
        svtSMS = 28
    End Enum

    Dim iLoop As Integer
    Public gFormCount As Integer = 0
    Dim clsSVT As New svtLibrary

    'For Special Rights
    Dim intOpt1 As Int32 = chkRightsPos.opt1Rights
    Dim intOpt2 As Int32 = chkRightsPos.opt2Rights
    Dim intOpt3 As Int32 = chkRightsPos.opt3Rights
    Dim intOpt4 As Int32 = chkRightsPos.opt4Rights
    Dim intOpt5 As Int32 = chkRightsPos.opt5Rights


    Public Function AuthenticateUser(ByVal path As String, ByVal user As String, ByVal pass As String) As Boolean
        Return True
        Exit Function
        Dim de As New DirectoryEntry(path, user, pass, AuthenticationTypes.Secure)
        Try
            'run a search using those credentials.  
            'If it returns anything, then you're authenticated
            Dim ds As DirectorySearcher = New DirectorySearcher(de)
            ds.FindOne()
            Return True
        Catch
            'otherwise, it will crash out so return false
            Return False
        End Try
    End Function

    Public Function SetDtCondn(ByVal argDtFld As String, ByVal argFromDt As String, Optional ByVal argToDt As String = "") As String
        'creates a date range condition for given argument date field
        Dim strCondn As String
        strCondn = ""
        strCondn = argDtFld & " >= " & getDateFormatInCond(argFromDt) & " "
        If argToDt <> "" Then
            strCondn = strCondn & " and " & argDtFld & " <= " & getDateFormatInCond(argToDt)
        End If
        SetDtCondn = strCondn
    End Function

    Public Function GetStock(ByVal argGetQOH As String, ByVal argLOCID As Integer, ByVal argItemID As String,
                                ByVal argFDate As Date, ByVal argTDate As Date, Optional ByVal argUsreName As String = "",
                                Optional ByVal sTrans As oledbTransaction = Nothing) As Double

        'argGetQOH = "Y"
        'argItemID = "410"

        If Trim(argUsreName) = "" Then argUsreName = gUserCode

        Dim cmd As oledbCommand = New oledbCommand("mSP_Stkbal")
        Using cmd
            With cmd
                If Not sTrans Is Nothing Then
                    cmd.Transaction = sTrans
                End If

                cmd.Connection = svtConn
                .CommandType = CommandType.StoredProcedure

                .Parameters.Add("@GetItemStock ", OleDbType.Char)
                .Parameters(0).Value = argGetQOH
                .Parameters.Add("@mLocID", OleDbType.Integer)
                .Parameters(1).Value = argLOCID
                .Parameters.Add("@mItemID", OleDbType.VarChar, 20, ParameterDirection.Input)
                .Parameters(2).Value = argItemID
                .Parameters.Add("@mUserName", OleDbType.VarChar, 50, ParameterDirection.Input)
                .Parameters(3).Value = argUsreName
                .Parameters.Add("@mFDt", OleDbType.DBDate)
                .Parameters(4).Value = gSettings.gCompStDt
                .Parameters.Add("@mTDt", OleDbType.DBDate)
                .Parameters(5).Value = argTDate ' getSystemDate()
                .Parameters.Add("@mFinYear", OleDbType.Integer)
                .Parameters(6).Value = 15 ' gSettings.gCompFinYear
                '.Parameters.Add("@mQOH", OleDbType.Numeric)
                .Parameters.Add("@mQOH", OleDbType.Decimal)
                .Parameters("@mQOH").Direction = ParameterDirection.Output
                .ExecuteNonQuery()

                If argGetQOH = "Y" Then
                    GetStock = GetVal(.Parameters("@mQOH").Value, 0)
                Else
                    GetStock = 0
                End If
                'GetStock = 100
            End With
        End Using


        'GetStock = 100
    End Function

    Public Function STKBal(ByVal argTDate As Date, Optional ByVal argUsreName As String = "") As Double
        STKBal = 0
        If Trim(argUsreName) = "" Then argUsreName = gUserCode
        Try
            Dim cmd As OleDbCommand = New OleDbCommand("mSP_Stkbal")
            With cmd
                cmd.Connection = svtConn
                .CommandType = CommandType.StoredProcedure

                .Parameters.Add("@mOPDate", OleDbType.DBDate)
                .Parameters(1).Value = gSettings.gCompStDt
                .Parameters.Add("@mFdt", OleDbType.DBDate)
                .Parameters(2).Value = gSettings.gCompStDt
                .Parameters.Add("@mTDt", OleDbType.DBDate)
                .Parameters(3).Value = argTDate ' getSystemDate()
                .Parameters.Add("@mUserName", OleDbType.VarChar, 50, ParameterDirection.Input)
                .Parameters(4).Value = gUserCode
                .CommandTimeout = 0
                .ExecuteNonQuery()
            End With
            cmd.Dispose() 'CNUD
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Exclamation, "Stock Execution Error")
        End Try
    End Function

    Public Sub INIStore(ByVal argCat As String, ByVal argSubCat As String, ByVal argValue As String)
        Dim i As Integer, introw As Integer
        argCat = Trim(UCase(argCat))
        argSubCat = Trim(UCase(argSubCat))

        With frmMain.vsfINI
            introw = -1
            For i = 0 To .Rows.Count - 1
                If argCat = UCase(Trim(.GetData(i, 0))) And
                        argSubCat = UCase(Trim(.GetData(i, 1))) Then
                    introw = i
                    Exit For
                End If
            Next i
            If introw = -1 Then
                .Rows.Add()
                introw = .Rows.Count - 1
            End If
            .SetData(introw, 0, argCat)
            .SetData(introw, 1, argSubCat)
            .SetData(introw, 2, argValue)
        End With
    End Sub

    Public Function getBatchDataTable(ByVal argItemID As Long) As DataTable
        Dim dt As New DataTable()
        Dim cmd As OleDbCommand = New OleDbCommand("spItemBatchStock")
        Try
            With cmd
                .Connection = svtConn
                .CommandType = CommandType.StoredProcedure
                .Parameters.Add("@mItemID", OleDbType.Integer)
                .Parameters(0).Value = argItemID
                '.Parameters.Add("@mLOTNO", OleDbType.VarChar, 50)
                '.Parameters(1).Value = ""
                '.Parameters.Add("@mQOH", OleDbType.integer)
                '.Parameters("@mQOH").Direction = ParameterDirection.Output
                '.ExecuteNonQuery()
                'getBatchDataTable = GetVal(.Parameters("@mQOH").Value, 0)
                '.ExecuteNonQuery()
            End With

            Dim da As New OleDb.OleDbDataAdapter(cmd)
            da.Fill(dt)

        Catch ex As Exception
            MsgBox(ex.Message & vbCrLf & "Error")
        End Try
        Return dt
    End Function

    Public Function INIGetSerLoc(ByVal argvsf As C1FlexGrid, ByVal argCat As String, ByVal argSubCat As String, ByVal argDefValue As String) As String
        Dim i As Integer, blnFound As Boolean
        argCat = Trim(UCase(argCat))
        argSubCat = Trim(UCase(argSubCat))
        INIGetSerLoc = ""
        blnFound = False
        With argvsf
            For i = 0 To .Rows.Count - 1
                If argCat = UCase(Trim(.GetData(i, 0))) And
                        argSubCat = UCase(Trim(.GetData(i, 1))) Then
                    INIGetSerLoc = .GetData(i, 2)
                    blnFound = True
                    Exit For
                End If
            Next i
        End With
        If Not blnFound Then
            INIGetSerLoc = argDefValue
        End If
    End Function

    Public Function INIGet(ByVal argCat As String, ByVal argSubCat As String, ByVal argDefValue As String) As String
        Dim i As Integer, blnFound As Boolean
        argCat = Trim(UCase(argCat))
        argSubCat = Trim(UCase(argSubCat))
        INIGet = ""
        blnFound = False
        With frmMain.vsfINI
            For i = 0 To .Rows.Count - 1
                If argCat = UCase(Trim(.GetData(i, 0))) And
                        argSubCat = UCase(Trim(.GetData(i, 1))) Then
                    INIGet = .GetData(i, 2)
                    blnFound = True
                    Exit For
                End If
            Next i
        End With
        If Not blnFound Then
            INIGet = argDefValue
        End If
    End Function

    Public Function openMSSqlConn(ByVal argServerName As String,
                                   ByVal argDBName As String,
                                   ByVal argUserCode As String,
                                   ByVal argLoginPwd As String, Optional ByVal argBranch As Boolean = False,
                                   Optional ByRef argBrchConn As OleDbConnection = Nothing) As Boolean

        openMSSqlConn = False
        Dim strConn As String

        If argBranch = False Then

            svtConn = New OleDbConnection
        Else
            argBrchConn = New OleDbConnection
        End If

        openMSSqlConn = False
        If argServerName = "" Or argDBName = "" Or argUserCode = "" Or argLoginPwd = "" Then Exit Function

        strConn = "Provider=SQLOLEDB.1;Persist Security Info=False"
        strConn = strConn & ";User ID=" & argUserCode
        strConn = strConn & ";Pwd=" & argLoginPwd
        strConn = strConn & ";Initial Catalog=" & argDBName
        strConn = strConn & ";Data Source=" & argServerName
        strConn = strConn & ";MultipleActiveResultSets = True"
        'strConn = "Server=" & argServerName & ";Database=" & argDBName & ";User Id=" & argUserCode & ";Password=" & argLoginPwd & ";MultipleActiveResultSets=True;"
        gstrConnString = strConn
        If argBrchConn Is Nothing Then
            svtConn.ConnectionString = strConn
        Else
            argBrchConn.ConnectionString = strConn
        End If

        Try
            If argBrchConn Is Nothing Then
                svtConn.Open()
            Else
                Try
                    argBrchConn.Open()

                Catch ex As Exception
                    MsgBox("Cannot connect. Please check Server/DatabaseName/LoginName/Password and try again!", vbInformation, "Connectivity Error")
                    Exit Function
                End Try

                'svtBrchConn.Open()
            End If
            openMSSqlConn = True
        Catch ex As Exception

            MsgBox("Error in Connecting to Database." & vbCrLf & ex.Message, MsgBoxStyle.Critical, "Error in Connecting to DB")
            'clsSVT.sendMail(ex.Message)
            'clsSVT.WriteToErrorLog(ex.Message, ex.StackTrace, ex.Source)
            'End
            Exit Function
        End Try

        openMSSqlConn = True
    End Function

    Public Function openAccessConn(ByVal argMdbFileName As String) As Boolean
        Dim strCONN As String
        svtConn = New OleDbConnection
        strCONN = "Provider = Microsoft.Jet.Oledb.4.0;Data source = " & argMdbFileName
        Try
            svtConn.ConnectionString = strCONN
            svtConn.Open()
            openAccessConn = True
        Catch ex As Exception
            openAccessConn = False
            MsgBox("Error in Connecting to Database." & vbCrLf & ex.Message, MsgBoxStyle.Critical, "Error in Connecting to DB")
            Exit Function
        End Try
    End Function

    Public Sub bindCombo(ByVal argComboBox As ComboBox, ByVal argValueMember As String, ByVal argDisplayMember As String, ByVal argSQL As String, Optional ByVal BranchConn As OleDbConnection = Nothing)
        Dim dt As DataTable
        Try
            dt = clsSVT.getDATATable(argSQL, BranchConn)
            argComboBox.DataSource = dt
            argComboBox.DisplayMember = argDisplayMember
            argComboBox.ValueMember = argValueMember
        Catch ex As Exception
            MsgBox(ex.Message, vbInformation)
        End Try
    End Sub

    'Public Sub bindC1Combo(ByVal argComboBox As C1.Win.C1List, ByVal argValueMember As String, ByVal argDisplayMember As String, ByVal argSQL As String)
    '    Dim dt As DataTable
    '    dt = clsSVT.getDATATable(argSQL)
    '    argComboBox.DataSource = dt
    '    argComboBox.DisplayMember = argDisplayMember
    '    argComboBox.ValueMember = argValueMember

    'End Sub

    Public Function GetVal(ByVal argValue As Object,
                    Optional ByVal argEmptyValue As Object = "", Optional argDataType As String = "") _
                    As Object
        If IsDBNull(argValue) Then
            GetVal = argEmptyValue
            If UCase(argDataType) = "DATE" Then GetVal = vbNull
        ElseIf Trim(argValue) = "" Then
            GetVal = argEmptyValue
        Else
            If VarType(argValue) = vbDate Then
                If argValue.ToString = "  /  /    " Then
                    GetVal = argEmptyValue
                Else
                    GetVal = Format(argValue, "dd/MM/yyyy")
                End If
            Else
                GetVal = argValue
            End If
        End If
    End Function

    Public Function dateValidate(ByVal dtDate As String, ByVal argAllowEmpty As Boolean) As Boolean
        dateValidate = False
        Dim strDate As Date
        dtDate = Replace(dtDate, "_", "")
        If argAllowEmpty Then
            If Trim(dtDate) = "/  /" Or Trim(dtDate) = "" Or dtDate = "__/__/____" Or dtDate = "//" Then
                dateValidate = True
                Exit Function
            End If
        End If

        If Len(dtDate) > 8 Then
            If Val(Right(dtDate, 4)) < 1900 Then Exit Function
            If Val(Right(dtDate, 4)) > 4012 Then Exit Function
        End If

        If Len(RTrim(dtDate)) < 8 Then
            Exit Function
        End If

        If InStr(dtDate, "/") > 0 Then
            If Val(Mid(dtDate, 4, 2)) > 12 Then
                Exit Function
            End If
        End If
        Try
            strDate = CDate(dtDate)
            dtDate = strDate.ToString("dd/MM/yyyy")

            If Not IsDate(dtDate) Then
                Exit Function
            End If

        Catch ex As Exception
            dateValidate = False
        End Try

        dateValidate = True
    End Function

    Public Function getDateFormatInInsert(ByVal argDate As Date, Optional ByVal blnWithTime As Boolean = False, Optional ByVal bln24HrFormat As Boolean = False) As String
        If blnWithTime Then
            If bln24HrFormat Then
                getDateFormatInInsert = argDate.ToString("dd MMM yyyy HH:mm:ss")
            Else
                getDateFormatInInsert = argDate.ToString("dd MMM yyyy hh:mm:ss")
            End If
        Else
            getDateFormatInInsert = argDate.ToString("dd MMM yyyy")
        End If
    End Function

    Public Function getDateFormatInCond(ByVal argDate As Date) As String
        If gDBTypeCurrent = gDbTypeAccess Then
            getDateFormatInCond = " Datevalue('" & Format(argDate, "dd/MM/yyyy") & "')"
        Else
            getDateFormatInCond = " '" & argDate.ToString("dd MMM yyyy") & "'"
        End If
    End Function

    Public Sub StMsg(Optional ByVal argMessage As String = "Ready")
        frmMain.tslblStatus.Text = argMessage
    End Sub

    Public Function getCheckString(ByVal argChkBox As CheckBox, Optional argBlnWithQuotes As Boolean = False) As String
        If argBlnWithQuotes Then
            getCheckString = IIf(argChkBox.Checked, "'Y'", "'N'")
        Else
            getCheckString = IIf(argChkBox.Checked, "Y", "N")
        End If
    End Function

    Public Sub setCheckBox(argChkBox As CheckBox, ByVal argValue As String)
        If UCase(Trim(GetVal(argValue, ""))) = "Y" Then
            argChkBox.Checked = True
        Else
            argChkBox.Checked = False
        End If
    End Sub

    Public Function genMultiColVsfComboString(ByVal argTblName As String,
            ByVal argOrdCond As String, ByVal strColNms As String) 'ParamArray strFldNames())

        Dim strComboString As String, strSQL As String
        Dim rsDummy As OleDb.OleDbDataReader
        Dim Colnm As Object
        Dim strFldNames() As String
        genMultiColVsfComboString = ""
        'modified by thangam 
        strSQL = "select distinct " & strColNms & " from " & argTblName & ""
        strSQL = strSQL & " " & argOrdCond

        rsDummy = clsSVT.getDataReaderFromSQL(strSQL)
        strFldNames = Split(strColNms, ",")

        Dim i As Integer
        strColNms = "|"
        If Not rsDummy.HasRows Then
            rsDummy.Close()
            Return ""
        End If
        With rsDummy
            strComboString = "|"
            Do While .Read()
                strColNms = String.Empty
                i = 0
                For Each Colnm In strFldNames
                    If i <> UBound(strFldNames) Then
                        strColNms = strColNms & rsDummy(Colnm) & vbTab
                    Else
                        strColNms = strColNms & rsDummy(Colnm)
                    End If
                    i = i + 1
                Next Colnm
                strComboString = strComboString & "|" & strColNms
            Loop
            .Close()
        End With
        genMultiColVsfComboString = strComboString
    End Function

    Public Function GenVSfComboStringCond(ByVal FeildName As String,
                           ByVal TableName As String,
                           Optional ByVal argOrderFlds As String = "",
                           Optional ByVal argCond As String = "") As String

        Dim rs As OleDbDataReader, strSQL As String
        strSQL = "Select distinct " & FeildName
        strSQL = strSQL & " From " & TableName
        strSQL = strSQL & argCond & " " & argOrderFlds

        rs = clsSVT.getDataReaderFromSQL(strSQL)

        Dim CboString As String
        CboString = "|"

        If Not rs.HasRows Then
            rs.Close()
            Return ""
        End If
        With rs
            Do While .Read()
                CboString = CboString & "|" & rs(0)
            Loop
            .Close()
        End With
        GenVSfComboStringCond = CboString
    End Function

    Public Function GenVSfComboStringCond_DT(ByVal FeildName As String,
                           ByVal TableName As DataTable,
                           Optional ByVal argOrderFlds As String = "",
                           Optional ByVal argCond As String = "") As String

        Dim CboString As String
        CboString = "|"
        With TableName
            For i As Int32 = 0 To .Rows.Count - 1
                CboString = CboString & "|" & .Rows(i).Item(FeildName)
            Next
        End With
        GenVSfComboStringCond_DT = CboString
    End Function

    Public Sub initFormSize(ByRef argFrm As Form, ByVal argTag As String,
                                ByVal argInitialise As Boolean, ByVal argWidth As Long, ByVal argHeight As Long, ByVal argTop As Long, ByVal argLeft As Long)
        Dim strSQL As String
        Dim rsDummy As OleDb.OleDbDataReader

        If argInitialise Then
            clsSVT.execSQLStatement("Delete from UtlFormLayoutSettings where UserID = " & gUserID & " and DocName = '" & argTag & "'")
        End If

        strSQL = "select * from UtlFormLayoutSettings where UserID = " & gUserID & " and DocName = '" & argTag & "'"
        rsDummy = clsSVT.getDataReaderFromSQL(strSQL)

        With rsDummy
            If .HasRows Then
                Do While .Read()
                    argFrm.Height = rsDummy("DocHeight")
                    argFrm.Width = rsDummy("DocWidth")
                    If Val(rsDummy("docTop")) = 0 AndAlso Val(rsDummy("docLeft")) = 0 Then
                        argFrm.StartPosition = FormStartPosition.CenterScreen
                    Else
                        argFrm.Top = rsDummy("docTop")
                        argFrm.Left = rsDummy("docLeft")
                    End If
                Loop
            Else
                argFrm.Height = argHeight
                argFrm.Width = argWidth
                If argTop = 0 AndAlso argLeft = 0 Then
                    argFrm.StartPosition = FormStartPosition.CenterScreen
                Else
                    argFrm.Top = argTop
                    argFrm.Left = argLeft
                End If
            End If
            .Close()
        End With
    End Sub
    Public Function chkSetupVersion() As String
        Select Case gStrSystemName.ToUpper
            Case "PLUTO", "EARTH5", "JUPITER2", "VENUS2", "SSPL17", "MADHUWORK", "EARTH1"
                Return "OK"
        End Select
        Select Case gStrServerName.ToUpper
            Case "SSPLMINISRVR\SQL2022", "SSPLMINISRVR", "SSPLMINISRVR\SQL19", "IBMSSPLSRVR\MSSQLSERVER01", "KUMARMACPRO\SQLEXPRESS14"
                Return "OK"
        End Select
        Dim svtL As New SSPLLic.clsSSPLLic
        Dim strChk As String = svtL.ValidLIC(svtConn, gStrServerName)
        chkSetupVersion = ""
        Select Case strChk
            Case "OK"
            Case "30DAYS"
                MsgBox("Please run the application in server with internet once.", MsgBoxStyle.Critical, "Security")
                Exit Function
            Case "INVALID_CLIENT_LIC"
                MsgBox("Please run the application in server once.", MsgBoxStyle.Critical, "Security")
                Exit Function
            Case Else
                MsgBox(strChk, MsgBoxStyle.Critical, "ERROR $$$")
                Exit Function
        End Select
        chkSetupVersion = "OK"
    End Function
    Public Sub SaveActFormSettings(ByVal argForm As Form, ByVal argDocTag As String)
        Dim strSQL As String
        strSQL = "Delete from UtlFormLayoutSettings where " & " UserID = " & gUserID & " and DocName = '" & argDocTag & "'"
        clsSVT.execSQLStatement(strSQL)

        strSQL = "Insert into UtlFormLayoutSettings "
        strSQL = strSQL & "(UserID,DocName,DocHeight,DocWidth,doctop,docleft) values ("
        strSQL = strSQL & gUserID & ","
        strSQL = strSQL & "'" & Mid(argDocTag, 1, 250) & "',"
        strSQL = strSQL & argForm.Height & ","
        strSQL = strSQL & argForm.Width & ","
        strSQL = strSQL & argForm.Top & ","
        strSQL = strSQL & argForm.Left & ")"
        clsSVT.execSQLStatement(strSQL)
    End Sub


    Public Function KeyValidate(ByVal DataType As String, ByVal Keychar As String,
        Optional ByVal MaxLength As Integer = 0, Optional ByVal FieldValue As Object = Nothing) As Boolean
        Select Case Keychar
            Case ChrW(22), ChrW(3)
                KeyValidate = True
                Exit Function
        End Select
        Select Case Asc(Keychar)
            Case Keys.Back, Keys.Tab
                KeyValidate = True
            Case Else
                Dim ValidChars As String = ""
                Select Case DataType
                    Case "String"
                        ValidChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ" _
                                   & "abcdefghijklmnopqrstuvwxyz" _
                                   & "01234567890!@#$%^&*(){}[] " _
                                   & ":" & "" & "|;'\<>?,./+- "
                    Case "Integer"
                        ValidChars = "01234567890,"
                    Case "Decimal"
                        ValidChars = "01234567890.,-"
                    Case "Decimal_NonNegative"
                        ValidChars = "01234567890."
                    Case "Date"
                        ValidChars = "01234567890-/"
                    Case "NonSpclChar"
                        ValidChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ" _
                                   & "abcdefghijklmnopqrstuvwxyz" _
                                   & "01234567890"
                End Select
                If InStr(ValidChars, Keychar) > 0 Then
                    KeyValidate = True
                Else
                    KeyValidate = False
                End If
        End Select
    End Function

    '# Get last day in month
    '(Month + 1) - subtract one day to get last day of the month
    Public Function getLastDateInMonth(ByVal argDate As Date) As Date
        Dim dt As Date
        dt = DateAdd(DateInterval.Month, 1, CDate("01/" & Month(argDate) & "/" & Year(argDate)))
        dt = DateAdd(DateInterval.Day, -1, dt)

        getLastDateInMonth = dt.ToString("dd/MM/yyyy")
    End Function

    '# Get First day in month
    Public Function getFirstDateInMonth(ByVal argDate As Date) As Date
        getFirstDateInMonth = CDate("01/" & Month(argDate) & "/" & Year(argDate)).ToString("dd/MM/yyyy")
    End Function

    Public Function getFirstDateInYear(ByVal argDate As Date) As Date
        getFirstDateInYear = CDate("01/01/" & Year(argDate)).ToString("dd/MM/yyyy")
    End Function

    Public Sub ApplyChkToCheckBox(ByVal argChkBox As CheckBox, ByVal argValue As String)
        If argValue = "Y" Then
            argChkBox.Checked = True
        Else
            argChkBox.Checked = False
        End If
    End Sub

    Public Function ConVrtToSentence(ByVal argText As String) As String

        Dim Txt As String, txtChr As String
        Dim x As Integer, Y As Integer
        Txt = argText
        ConVrtToSentence = ""
        'Check the first 10 characters to get
        'the first - incase the selection includes
        'some spaces at the beginning.
        For x = 1 To 10
            txtChr = Mid(Txt, x, 1)
            If txtChr Like "[a-zA-Z]" Then
                Mid(Txt, x, 1) = UCase(txtChr)
                Exit For
            End If
        Next x

        'Check each character to find the criteria (.!?)
        'for the end of a sentence, then Capitalize the next letter.
        For x = x To Len(Txt)
            txtChr = Mid(Txt, x, 1)
            If txtChr Like "[.!?]" Then
                For Y = x + 1 To x + 10
                    txtChr = Mid(Txt, Y, 1)
                    If txtChr Like "[a-zA-Z]" Then
                        Mid(Txt, Y, 1) = UCase(txtChr)
                        x = Y + 1
                        Exit For
                    End If
                Next Y
            End If
        Next x
        If Txt <> "" And Right(Txt, 1) <> "." Then Txt = Txt & "."
        ConVrtToSentence = Trim(Txt)
    End Function

    Public Function SVTROUND(ByVal aNumberToRound As Double,
      Optional ByVal aDecimalPlaces As Double = 0) As Double

        On Error GoTo errhandler

        '    SVTROUND = CStr(clsSvt.GetDataValue("round(" & Val(aSNumberToRound) & "," & aDecimalPlaces & ")", Val(aSNumberToRound), "", False, ""))
        '    Exit Function

        Dim nFactor As Double
        Dim nTemp As Double

        nFactor = 10 ^ aDecimalPlaces
        nTemp = (aNumberToRound * nFactor) + 0.5
        SVTROUND = Int(CDec(nTemp)) / nFactor

        '-----------EXIT POINT------------------
ExitPoint:

        Exit Function

        '-----------ERROR HANDLER---------------
errhandler:

        Select Case Err.Number
            Case Else
                ' Your error handling here
                SVTROUND = 0
                Resume ExitPoint
        End Select
    End Function

    Public Function getSystemDate(Optional ByVal sTrans As OleDbTransaction = Nothing) As Date
        'If Not sTrans Is Nothing Then
        '    getSystemDate = Format(clsSVT.getDataDateValue("Select getdate()", Date.Now, False, sTrans), "dd/MM/yyyy")
        'Else
        '    getSystemDate = Format(clsSVT.getDataDateValue("Select getdate()", Date.Now, False), "dd/MM/yyyy")
        'End If
        If gDBTypeCurrent = gDbTypeMSSQL Then
            If Not sTrans Is Nothing Then
                getSystemDate = Format(clsSVT.getDataDateValue("Select " & gSqlGetDate, Date.Now, False, sTrans), "dd/MM/yyyy")
            Else
                getSystemDate = Format(clsSVT.getDataDateValue("Select " & gSqlGetDate, Date.Now, False), "dd/MM/yyyy")
            End If
        Else
            If Not sTrans Is Nothing Then
                getSystemDate = Format(clsSVT.getDataDateValue("Select Now()", Date.Now, False, sTrans), "dd/MM/yyyy")
            Else
                getSystemDate = Format(clsSVT.getDataDateValue("Select now()", Date.Now, False), "dd/MM/yyyy")
            End If
        End If
    End Function

    Public Function getSystemDateTime(Optional ByVal sTrans As OleDbTransaction = Nothing) As Date
        'If Not sTrans Is Nothing Then
        '    getSystemDateTime = clsSVT.getDataDateValue("Select getdate()", Date.Now, False, sTrans)
        'Else
        '    getSystemDateTime = clsSVT.getDataDateValue("Select getdate()", Date.Now, False)
        'End If
        If gDBTypeCurrent = gDbTypeMSSQL Then
            If Not sTrans Is Nothing Then
                getSystemDateTime = clsSVT.getDataDateValue("Select " & gSqlGetDate, Date.Now, False, sTrans)
            Else
                getSystemDateTime = clsSVT.getDataDateValue("Select " & gSqlGetDate, Date.Now, False)
            End If
        Else
            If Not sTrans Is Nothing Then
                getSystemDateTime = clsSVT.getDataDateValue("Select now()", Date.Now, False, sTrans)
            Else
                getSystemDateTime = clsSVT.getDataDateValue("Select now()", Date.Now, False)
            End If
        End If
    End Function

    Public Function fnEvaluate(ByVal argString As String) As String
        fnEvaluate = clsSVT.getDataStringValue("Select " & argString, "", False)
    End Function

    Public Function fnAmtInWords(ByVal argAmt As Double, ByVal argCurName As String, ByVal argDecCurName As String, ByVal argInMillion As String) As String
        fnAmtInWords = ""
        Dim strDecimalName As String = ""

        If argDecCurName = "" Then
            Select Case UCase(argDecCurName)
                Case "PAISE"
                Case "PAISE"
            End Select
        End If

        Select Case UCase(argInMillion)
            Case "N"
                fnAmtInWords = getAmtInWordsLakhs(argAmt, argCurName, argDecCurName)

            Case "Y"
                fnAmtInWords = getAmtInWordsMillion(argAmt, argCurName, argDecCurName)
        End Select
        fnAmtInWords = removeSpaces(fnAmtInWords)
    End Function


    Private Function removeSpaces(ByVal argWords As String) As String
        removeSpaces = Replace(argWords, "  ", " ")
        removeSpaces = Replace(argWords, "  ", " ")
        removeSpaces = Replace(argWords, "  ", " ")
        removeSpaces = Replace(argWords, "  ", " ")
    End Function

    Public Function getAmtInWordsLakhs(ByVal argAmountVal As Double, ByVal argCur As String, ByVal argCurDeci As String)
        Dim argAmt
        Dim strWholePart As String = "", strDeciPart As String = "", temp As String = ""
        Dim DecimalPlace, Count
        Dim Place(9) As String

        If argCur = "Rupees" Then argCur = "Rupees"
        'If Trim(UCase(argCur)) = "INR" Then argCur = "Rupees"
        If argCur = "Paises" Then argCur = "Paise"

        argAmt = Math.Abs(argAmountVal)

        'If argCur = "" Then argCur = "Rupee"
        'If argCurDeci = "" Then argCurDeci = "Paise"
        Place(2) = " Thousand "
        Place(3) = " Lakhs " '
        Place(4) = " Crores "
        Place(5) = " Trillion "
        argAmt = Trim(Str(argAmt))     ' Position of decimal place 0 if none
        ' Expand the logic to 999 crores from 9 Lakhs
        If (argAmt > 999999999.99) Then
            'If (argAmt > 999999.99) Then
            getAmtInWordsLakhs = "Digit excced Maximum limit"
            Exit Function
        End If
        DecimalPlace = InStr(argAmt, ".")
        'Convert strDeciPart and set argAmt to strWholePart amount
        If DecimalPlace > 0 Then
            strDeciPart = GetTens(Left(Mid(argAmt, DecimalPlace + 1) & "00", 2))
            argAmt = Trim(Left(argAmt, DecimalPlace - 1))
        End If
        Count = 1
        Dim iTemp As Integer
        Do While argAmt <> ""
            'Do While Val(argAmt) <> 0
            If (Count >= 2) Then
                iTemp = Right(argAmt, 2)
            Else
                If (Len(argAmt) = 2) Then
                    iTemp = Right(argAmt, 2)
                ElseIf (Len(argAmt) = 1) Then
                    iTemp = Right(argAmt, 1)
                Else
                    iTemp = Right(argAmt, 3)
                End If
            End If
            If iTemp > 99 Then
                iTemp = Right(argAmt, 3)
                temp = GetHundreds(iTemp)
                'æÜ
            ElseIf iTemp <= 99 And iTemp > 9 Then
                iTemp = Right(argAmt, 2)
                temp = GetTens(iTemp)
            ElseIf iTemp < 10 Then
                iTemp = Right(argAmt, 2)
                temp = GetDigit(iTemp)
            End If
            'end if
            If temp <> "" Then
                strWholePart = temp & Place(Count) & strWholePart
            End If
            'If Len(argAmt) > 3 Then
            If Count = 2 Then
                If Len(argAmt) = 1 Then
                    argAmt = ""
                Else
                    argAmt = Left(argAmt, Len(argAmt) - 2)
                End If
            ElseIf Count = 3 Then
                If Len(argAmt) >= 3 Then
                    argAmt = Left(argAmt, Len(argAmt) - 2)
                Else
                    argAmt = ""
                End If
            ElseIf Count = 4 Then
                argAmt = ""
            Else
                If Len(argAmt) <= 2 Then
                    argAmt = ""
                Else
                    argAmt = Left(argAmt, Len(argAmt) - 3)
                End If
            End If
            Count = Count + 1
        Loop
        Select Case strWholePart
            Case ""
                strWholePart = "-"
            Case "One"
                strWholePart = "One " & argCur
            Case Else
                strWholePart = argCur & " " & strWholePart
                'strWholePart = strWholePart & " " & argCur & "s "
        End Select
        Select Case strDeciPart
            Case ""
                strDeciPart = ""
            Case "One"
                strDeciPart = " and One " & argCurDeci
            Case Else
                strDeciPart = " and " & strDeciPart & " " & argCurDeci
        End Select
        If argAmountVal < 0 Then
            getAmtInWordsLakhs = strNeg & strWholePart & strDeciPart & " Only"
        Else
            getAmtInWordsLakhs = strWholePart & strDeciPart & " Only"
        End If
        iLoop = 0
    End Function

    ' Converts a number from 10 to 99 into text
    Function GetTens(ByVal TensText)
        Dim Result As String
        Result = ""           'null out the temporary function value
        If Val(Left(TensText, 1)) = 1 Then   ' If value between 10-19
            Select Case Val(TensText)
                Case 10 : Result = "Ten"
                Case 11 : Result = "Eleven"
                Case 12 : Result = "Twelve"
                Case 13 : Result = "Thirteen"
                Case 14 : Result = "Fourteen"
                Case 15 : Result = "Fifteen"
                Case 16 : Result = "Sixteen"
                Case 17 : Result = "Seventeen"
                Case 18 : Result = "Eighteen"
                Case 19 : Result = "Nineteen"
                Case Else
            End Select
        Else                                 ' If value between 20-99
            Select Case Val(Left(TensText, 1))
                Case 2 : Result = "Twenty "
                Case 3 : Result = "Thirty "
                Case 4 : Result = "Forty "
                Case 5 : Result = "Fifty "
                Case 6 : Result = "Sixty "
                Case 7 : Result = "Seventy "
                Case 8 : Result = "Eighty "
                Case 9 : Result = "Ninety "
                Case Else
            End Select
            Result = Result & GetDigit _
               (Right(TensText, 1))  'Retrieve ones place
        End If
        GetTens = Result
    End Function

    'Converts a number from 100-999 into text
    Function GetHundreds(ByVal MyNumber)
        Dim Result As String = ""
        GetHundreds = ""
        If Val(MyNumber) = 0 Then Exit Function
        MyNumber = Right("000" & MyNumber, 3)     'Convert the hundreds place
        If Mid(MyNumber, 1, 1) <> "0" Then
            If (iLoop > 0) Then
                Result = GetDigit(Mid(MyNumber, 1, 1)) & " Lac "
                iLoop = 0
            Else
                Result = GetDigit(Mid(MyNumber, 1, 1)) & " Hundred "
                iLoop = iLoop + 1
            End If
        End If
        'Convert the tens and ones place
        If Mid(MyNumber, 2, 1) <> "0" Then
            Result = Result & GetTens(Mid(MyNumber, 2))
        Else
            Result = Result & GetDigit(Mid(MyNumber, 3))
        End If
        GetHundreds = Result
    End Function

    'Converts a number from 1 to 9 into text
    Function GetDigit(ByVal Digit)
        Select Case Val(Digit)
            Case 1 : GetDigit = "One"
            Case 2 : GetDigit = "Two"
            Case 3 : GetDigit = "Three"
            Case 4 : GetDigit = "Four"
            Case 5 : GetDigit = "Five"
            Case 6 : GetDigit = "Six"
            Case 7 : GetDigit = "Seven"
            Case 8 : GetDigit = "Eight"
            Case 9 : GetDigit = "Nine"
            Case Else : GetDigit = ""
        End Select
    End Function

    '# A - Function for AmtInWords - per currency
    Public Function getAmtInWordsMillion(ByVal nAmount As Double, ByVal argCur As String, ByVal argCurDeci As String) As String
        Dim nDollar As Double
        Dim nCent As Double
        '    If argCur = "" Then argCur = "Rupees"
        '    If argCurDeci = "" Then argCur = "Paise"
        nDollar = Int(nAmount)
        nCent = (Math.Abs(nAmount) * 100) Mod 100
        'getAmtInWordsMillion = NumToStringMillions(nDollar) & " " & argCur
        'getAmtInWordsMillion = argCur & "s " & NumToStringMillions(nDollar)
        getAmtInWordsMillion = argCur & NumToStringMillions(nDollar)
        If Math.Abs(nDollar) <> 1 Then
            getAmtInWordsMillion = getAmtInWordsMillion '& "s"
        End If

        getAmtInWordsMillion = getAmtInWordsMillion & " and" &
                         NumToStringMillions(nCent) & " " & argCurDeci

        If Math.Abs(nCent) <> 1 Then
            getAmtInWordsMillion = getAmtInWordsMillion '& "s"
        End If

    End Function

    Public Function NumToStringMillions(ByVal nNumber As Double) As String
        Dim bNegative As Boolean
        Dim bHundred As Boolean
        NumToStringMillions = ""
        If nNumber < 0 Then
            bNegative = True
        End If

        nNumber = Math.Abs(Int(nNumber))

        If nNumber < 1000 Then
            If nNumber \ 100 > 0 Then
                NumToStringMillions = NumToStringMillions &
                     NumToStringMillions(nNumber \ 100) & " hundred"
                bHundred = True
            End If
            nNumber = nNumber - ((nNumber \ 100) * 100)
            Dim bNoFirstDigit As Boolean
            bNoFirstDigit = False
            Select Case nNumber \ 10
                Case 0
                    Select Case nNumber Mod 10
                        Case 0
                            If Not bHundred Then
                                NumToStringMillions = NumToStringMillions & " Zero"
                            End If
                        Case 1 : NumToStringMillions = NumToStringMillions & " One"
                        Case 2 : NumToStringMillions = NumToStringMillions & " Two"
                        Case 3 : NumToStringMillions = NumToStringMillions & " Three"
                        Case 4 : NumToStringMillions = NumToStringMillions & " Four"
                        Case 5 : NumToStringMillions = NumToStringMillions & " Five"
                        Case 6 : NumToStringMillions = NumToStringMillions & " Six"
                        Case 7 : NumToStringMillions = NumToStringMillions & " Seven"
                        Case 8 : NumToStringMillions = NumToStringMillions & " Eight"
                        Case 9 : NumToStringMillions = NumToStringMillions & " Nine"
                    End Select
                    bNoFirstDigit = True
                Case 1
                    Select Case nNumber Mod 10
                        Case 0 : NumToStringMillions = NumToStringMillions & " Ten"
                        Case 1 : NumToStringMillions = NumToStringMillions & " Eleven"
                        Case 2 : NumToStringMillions = NumToStringMillions & " Twelve"
                        Case 3 : NumToStringMillions = NumToStringMillions & " Thirteen"
                        Case 4 : NumToStringMillions = NumToStringMillions & " Fourteen"
                        Case 5 : NumToStringMillions = NumToStringMillions & " Fifteen"
                        Case 6 : NumToStringMillions = NumToStringMillions & " Sixteen"
                        Case 7 : NumToStringMillions = NumToStringMillions & " Seventeen"
                        Case 8 : NumToStringMillions = NumToStringMillions & " Eighteen"
                        Case 9 : NumToStringMillions = NumToStringMillions & " Nineteen"
                    End Select
                    bNoFirstDigit = True
                Case 2 : NumToStringMillions = NumToStringMillions & " Twenty"
                Case 3 : NumToStringMillions = NumToStringMillions & " Thirty"
                Case 4 : NumToStringMillions = NumToStringMillions & " Forty"
                Case 5 : NumToStringMillions = NumToStringMillions & " Fifty"
                Case 6 : NumToStringMillions = NumToStringMillions & " Sixty"
                Case 7 : NumToStringMillions = NumToStringMillions & " Seventy"
                Case 8 : NumToStringMillions = NumToStringMillions & " Eighty"
                Case 9 : NumToStringMillions = NumToStringMillions & " Ninety"
            End Select
            If Not bNoFirstDigit Then
                If nNumber Mod 10 <> 0 Then
                    NumToStringMillions = NumToStringMillions & "-" &
                                  Mid(NumToStringMillions(nNumber Mod 10), 2)
                End If
            End If
        Else
            Dim nTemp As Double
            nTemp = 10 ^ 12 'trillion
            Do While nTemp >= 1
                If nNumber >= nTemp Then
                    NumToStringMillions = NumToStringMillions &
                                  NumToStringMillions(Int(nNumber / nTemp))
                    Select Case Int(Math.Log(nTemp) / Math.Log(10) + 0.5)
                        Case 12 : NumToStringMillions = NumToStringMillions & " Trillion"
                        Case 9 : NumToStringMillions = NumToStringMillions & " Billion"
                        Case 6 : NumToStringMillions = NumToStringMillions & " Million"
                        Case 3 : NumToStringMillions = NumToStringMillions & " Thousand"
                    End Select

                    nNumber = nNumber - (Int(nNumber / nTemp) * nTemp)
                End If
                nTemp = nTemp / 1000
            Loop
        End If

        If bNegative Then
            NumToStringMillions = " negative" & NumToStringMillions
        End If

    End Function

    Public Function fnTablePresent(ByVal argTbl As String) As Boolean
        Dim cmdDummy As New OleDbCommand
        fnTablePresent = False
        Try
            With cmdDummy
                .Connection = svtConn
                .CommandText = "Select top 1 * from " & argTbl
                .ExecuteNonQuery()
                fnTablePresent = True
            End With
        Catch ex As Exception
            fnTablePresent = False
        End Try
    End Function

    Public Function fnFldPresent(ByVal argFldName As String, ByVal argTblName As String, Optional ByVal argTrn As OleDb.OleDbTransaction = Nothing) As Boolean
        Dim cmdDummy As New OleDbCommand
        fnFldPresent = False
        Try
            With cmdDummy
                .Connection = svtConn

                .Transaction = argTrn

                .CommandText = "Select top 1 " & argFldName & " from " & argTblName
                .ExecuteNonQuery()
                fnFldPresent = True
            End With
        Catch ex As Exception
            fnFldPresent = False
        End Try
    End Function

    Public Sub SaveActFormVsfDet(ByVal argForm As Form, ByVal argDocName As String, ByVal argVsfname As String, ByVal argVSF As C1.Win.C1FlexGrid.C1FlexGrid, Optional ByVal argCtrlTyp As String = Nothing, Optional ByVal argSplit As System.Windows.Forms.SplitContainer = Nothing)
        Dim strSQL As String
        Dim strWidth As String, i As Integer
        strWidth = ""
        'Satish '20Jan2018
        If argSplit IsNot Nothing Then
            strWidth = argSplit.SplitterDistance & "$"
        End If

        With argVSF
            If UCase(argVSF.Tag) = "DSGR" Then
                For i = 0 To 5
                    strWidth = strWidth & .Cols(i).Width & ","
                Next i
            Else
                If argCtrlTyp = Nothing Then
                    For i = 0 To .Cols.Count - 1
                        strWidth = strWidth & .Cols(i).Width & ","
                    Next i
                Else
                    strWidth = argVSF.Width & ","
                    strWidth = strWidth & argVSF.Height & ","
                End If
            End If
        End With

        If strWidth <> "" Then strWidth = Mid(strWidth, 1, Len(strWidth) - 1)

        strSQL = "Delete from UtlVsfWidthSettngs "
        strSQL = strSQL & " where UserID = " & gUserID & " and DocName = '" & argDocName & "' and vsfName = '" & argVsfname & "'"
        Call clsSVT.execSQLStatement(strSQL)

        strSQL = "insert into UtlVsfWidthSettngs (DocName, UserID, vsfName, VsfColWidths) values ("
        strSQL = strSQL & "'" & argDocName & "',"
        strSQL = strSQL & gUserID & ","
        strSQL = strSQL & "'" & argVsfname & "',"
        strSQL = strSQL & "'" & strWidth & "')"
        Call clsSVT.execSQLStatement(strSQL)
    End Sub

    Public Sub initFormVsfDet(ByRef argFrm As Form, ByVal argDocName As String, ByRef argVSF As C1.Win.C1FlexGrid.C1FlexGrid, ByVal argVsfname As String, Optional ByVal argCtrlTyp As String = Nothing, Optional ByVal argSplit As System.Windows.Forms.SplitContainer = Nothing)
        Dim strSQL As String
        Dim rsDummy As OleDbDataReader
        Dim arrColwidths() As String

        strSQL = "select * from UtlVsfWidthSettngs where UserID = " & gUserID & " and DocName = '" & argDocName & "' and vsfName = '" & argVsfname & "'"
        rsDummy = clsSVT.getDataReaderFromSQL(strSQL)

        If Not rsDummy.HasRows Then
            rsDummy.Close()
            Exit Sub
        End If


        With rsDummy
            If .Read Then
                Dim intSplitDist As Integer = 0
                intSplitDist = InStr(GetVal(!VsfColWidths, ""), "$")
                arrColwidths = Split(Mid(GetVal(!VsfColWidths, ""), intSplitDist + 1, Len(GetVal(!VsfColWidths, ""))), ",")
                Dim i As Integer
                For i = 0 To UBound(arrColwidths)
                    If i > argVSF.Cols.Count - 1 Then Exit For

                    If argCtrlTyp = Nothing Then
                        argVSF.Cols(i).Width = arrColwidths(i)
                    Else
                        argVSF.Width = arrColwidths(0)
                        argVSF.Height = arrColwidths(1)
                    End If
                Next i
                If intSplitDist > 0 Then
                    argSplit.SplitterDistance = Mid(GetVal(!VsfColWidths, ""), 1, intSplitDist - 1)
                End If
            End If
            .Close()
        End With
    End Sub

    Public Sub SaveActFormSC(ByVal argForm As Form, ByVal argDocName As String, ByRef argSplit As SplitContainer, ByVal argScname As String, Optional ByVal argCtrlType As String = Nothing)
        Dim strSQL As String
        Dim strWidth As String = ""
        strWidth = ""
        Dim strHeight As String = ""
        'Udhaya '25Fed2019


        strSQL = "Delete from UtlScWidthSettings "
        strSQL = strSQL & " where UserID = " & gUserID & " and DocName = '" & argDocName & "' and ScName = '" & argScname & "'"
        Call clsSVT.execSQLStatement(strSQL)

        strWidth = argSplit.Size.Width
        strHeight = argSplit.Size.Height

        strSQL = "insert into UtlScWidthSettings (DocName, UserID, ScName, ScSize) values ("
        strSQL = strSQL & "'" & argDocName & "',"
        strSQL = strSQL & gUserID & ","
        strSQL = strSQL & "'" & argScname & "',"
        strSQL = strSQL & "'" & argSplit.SplitterDistance & "$" & strWidth & "," & strHeight & "')"
        Call clsSVT.execSQLStatement(strSQL)

    End Sub

    Public Sub initFormSC(ByRef argFrm As Form, ByVal argDocName As String, ByRef argSplit As SplitContainer, ByVal argScname As String, Optional ByVal argCtrlType As String = Nothing)
        Dim strSQL As String
        Dim strWidth As String = ""
        Dim strHeight As String = ""
        Dim rsDummy As OleDbDataReader
        Dim strDistance As String = ""

        strSQL = "select * from UtlScWidthSettings where UserID = " & gUserID & " and DocName = '" & argDocName & "' and ScName = '" & argScname & "'"
        rsDummy = clsSVT.getDataReaderFromSQL(strSQL)

        If Not rsDummy.HasRows Then
            rsDummy.Close()
            Exit Sub
        End If


        With rsDummy
            If .Read Then

                Dim blnDist As Boolean = False
                For Each strDist As String In CStr(!ScSize & "").Split("$")
                    If strDistance = "" Then
                        strDistance = strDist
                    End If

                    If blnDist Then
                        For Each strSize As String In strDist.Split(",")
                            If strWidth = "" Then
                                strWidth = strSize
                                GoTo nextrecord
                            End If
                            strHeight = strSize
nextrecord:
                        Next
                    End If
                    blnDist = True
                Next
                If strWidth = "" Then strWidth = argSplit.Size.Width
                If strHeight = "" Then strHeight = argSplit.Size.Height
                If strDistance = "" Then strDistance = argSplit.SplitterDistance
                argSplit.Size = New Size(CInt(strWidth), CInt(strHeight))
                argSplit.SplitterDistance = CInt(strDistance)
            End If
            .Close()
        End With
    End Sub

    Public Function gMoveRecords(ByVal btnClicked As Integer,
                    ByVal argTblName As String, ByVal argFldName As String,
                    ByVal blnInteger As Boolean,
                    ByVal argStrValue As String, ByVal argPfx As String, Optional ByVal argDtFldForFinYearChk As String = "", Optional ByVal strAdnlCond As String = "") As Long
        Dim clsSVT As New svtLibrary
        Dim strSQL As String = ""
        Dim strValue As Long = 0, strCond As String

        strCond = " Where 1 = 1 "
        If argDtFldForFinYearChk <> "" Then
            strCond = strCond & " and " & getDateCond(argDtFldForFinYearChk) & " "
        End If
        strCond = strCond & strAdnlCond
        strCond = strCond & IIf(argPfx = "", "", " and prefix = '" & argPfx & "' ")
        If blnInteger Then
            Select Case btnClicked
                Case svtBtns.svtMoveFirst
                    strSQL = "Select Min(" & getSyntax("ISNULL0", argFldName) & ") from " & argTblName & strCond & " and " & argFldName & "<>0"
                Case svtBtns.svtMoveNext
                    strSQL = "Select TOP 1 " & argFldName & " from " & argTblName & strCond & " and " & argFldName & " > " & Val(argStrValue) & " order by " & argFldName
                Case svtBtns.svtMovePrev
                    strSQL = "Select TOP 1 " & argFldName & " From " & argTblName & strCond & " and " & argFldName & " < " & Val(argStrValue) & " order by " & argFldName & " Desc"
                Case svtBtns.svtMoveLast
                    'strSQL = "Select isnull(max(isnull(" & argFldName & ",0)),0) from " & argTblName & strCond
                    strSQL = "Select Max(" & getSyntax("ISNULL0", argFldName) & ") from " & argTblName & strCond
            End Select
            If strSQL <> "" Then
                strValue = CStr(clsSVT.getDataLongValue(strSQL, 0, False))
            End If
        Else
            Select Case btnClicked
                Case svtBtns.svtMoveFirst
                    strSQL = "Select min(" & argFldName & ") from " & argTblName & strCond
                Case svtBtns.svtMoveNext
                    strSQL = "Select TOP 1 " & argFldName & " from " & argTblName & strCond & " and " & argFldName & " > '" & Trim(argStrValue) & "'"
                    strSQL = strSQL & " order by " & argFldName
                Case svtBtns.svtMovePrev
                    strSQL = "Select TOP 1 " & argFldName & " from " & argTblName & strCond & " and " & argFldName & " < '" & Trim(argStrValue) & "'"
                    strSQL = strSQL & " order by " & argFldName & " Desc"
                Case svtBtns.svtMoveLast
                    strSQL = "Select max(" & argFldName & ") from " & argTblName & strCond
            End Select
            If strSQL <> "" Then
                strValue = clsSVT.getDataStringValue(strSQL, "", False)
            End If

        End If

        gMoveRecords = strValue
    End Function

    Public Function getDateCond(ByVal argFldName As String) As String
        getDateCond = " " & argFldName & " >= " & getDateFormatInCond(gSettings.gCompStDt) & " and "
        getDateCond = getDateCond & " " & argFldName & " <= " & getDateFormatInCond(gSettings.gCompEndDt) & " "
    End Function

    Public Function DataTypeIsString(ByVal argDataType As Type) As Boolean
        DataTypeIsString = True
        'Select Case argDataType

        'End Select
    End Function

    Public Sub ExportToXL(ByVal argVSF As C1.Win.C1FlexGrid.C1FlexGrid,
                          Optional ByVal argExpType As String = "XL",
                          Optional ByRef FilePath As String = "",
                          Optional ByVal argFileName As String = "",
                          Optional blnShowCompName As Boolean = False,
                          Optional argHdr1 As String = "",
                          Optional argHdr2 As String = "")
        Dim strFile As String
        Try
            If argExpType = "CSV" Then
                If argFileName <> "" Then
                    strFile = Application.StartupPath & "\Temp\" & argFileName & Date.Now.Ticks & ".txt"
                Else
                    strFile = Application.StartupPath & "\Temp\CsvExport" & Date.Now.Ticks & ".txt"
                End If
                argVSF.SaveGrid(strFile, C1.Win.C1FlexGrid.FileFormatEnum.TextComma)

                If FilePath = "Y" Then
                    FilePath = strFile
                    Exit Sub
                End If
            Else
                If argFileName <> "" Then
                    strFile = Application.StartupPath & "\Temp\" & argFileName & ".xlsx"
                Else
                    strFile = Application.StartupPath & "\Temp\XLExport" & Date.Now.Ticks & ".xlsx"
                End If

                'If argVSF.Rows.Count > 65536 Then
                If argVSF.Rows.Count > 90000 Then
                    MsgBox("Limit Exceed!!!", vbInformation, argVSF.Text)
                    Exit Sub
                End If

                argVSF.SaveExcel(strFile, C1.Win.C1FlexGrid.FileFlags.AsDisplayed Or C1.Win.C1FlexGrid.FileFlags.IncludeMergedRanges Or C1.Win.C1FlexGrid.FileFlags.IncludeFixedCells)

                'argVSF.SaveExcel(strFile, C1.Win.C1FlexGrid.FileFlags.AsDisplayed Or C1.Win.C1FlexGrid.FileFlags.IncludeFixedCells)

                If FilePath = "Y" Then
                    FilePath = strFile
                    Exit Sub
                End If
                Dim xlApp As New Excel.Application
                xlApp.Workbooks.Open(strFile, False, True)
                'insert header if required
                If blnShowCompName Or (Trim(argHdr1) <> "") Or (Trim(argHdr2) <> "") Then
                    xlApp.Range("A1").Select()
                    xlApp.Rows("1:1").Select
                    xlApp.Selection.Insert(Shift:=Microsoft.Office.Interop.Excel.XlDirection.xlDown, CopyOrigin:=Microsoft.Office.Interop.Excel.XlInsertFormatOrigin.xlFormatFromLeftOrAbove)
                    xlApp.Rows("1:1").Select
                    xlApp.Selection.Insert(Shift:=Microsoft.Office.Interop.Excel.XlDirection.xlDown, CopyOrigin:=Microsoft.Office.Interop.Excel.XlInsertFormatOrigin.xlFormatFromLeftOrAbove)
                    xlApp.Rows("1:1").Select
                    xlApp.Selection.Insert(Shift:=Microsoft.Office.Interop.Excel.XlDirection.xlDown, CopyOrigin:=Microsoft.Office.Interop.Excel.XlInsertFormatOrigin.xlFormatFromLeftOrAbove)

                    If blnShowCompName Then xlApp.Range("A1").Value = gSettings.gCompName
                    xlApp.Range("A2").Value = argHdr1
                    xlApp.Range("A3").Value = argHdr2
                End If
                xlApp.Visible = True
            End If
            '    Dim p As New System.Diagnostics.Process
            'Dim s As New System.Diagnostics.ProcessStartInfo(strFile)
            's.UseShellExecute = True
            's.WindowStyle = ProcessWindowStyle.Normal
            'p.StartInfo = s
            'p.Start()
        Catch ex As Exception
            MsgBox("Cannot export to Excel." & vbCrLf & ex.Message, MsgBoxStyle.Critical, "Export Error")
        End Try
    End Sub


    Public Class GridToPdf
        ' Fields
        Private images As ArrayList
        Private zoom As Single

        ' ctor
        Public Sub New(ByVal flex As C1.Win.C1FlexGrid.C1FlexGrid)

            ' initialize zoom factor
            Me.zoom = 1.0!

            ' create page images
            Me.images = New ArrayList()

            ' initialize
            Dim r1%, c1%, r2%, c2%

            ' loop through columns looking for breaks
            c1 = flex.Cols.Fixed
            c2 = flex.Cols.Fixed

            Dim c%
            For c = flex.Cols.Fixed To flex.Cols.Count - 1

                ' check if this is a column break
                If ((c = flex.Cols.Count - 1) OrElse (Not (flex.Cols(c).UserData Is Nothing)) AndAlso (flex.Cols(c).UserData.ToString = "*")) Then

                    ' found break, column range is c1-c
                    c2 = c

                    ' loop through rows looking for breaks
                    r1 = flex.Rows.Fixed
                    r2 = flex.Rows.Fixed
                    Dim r%
                    For r = flex.Rows.Fixed To flex.Rows.Count - 1

                        ' look for next row break
                        If ((r = flex.Rows.Count - 1) OrElse (Not (flex.Rows.Item(r).UserData Is Nothing) AndAlso (flex.Rows(r).UserData.ToString = "*"))) Then
                            r2 = r
                            Me.images.Add(flex.CreateImage(r1, c1, r2, c2))
                            r1 = (r + 1)
                        End If
                    Next
                End If
            Next
        End Sub

        ' Methods
        'Public Sub Save(ByVal fileName As String)

        '    ' create new pdf document
        '    Dim doc As New C1.C1Pdf.C1PdfDocument()

        '    ' add pages to document
        '    Dim page%
        '    For page = 0 To Me.images.Count - 1
        '        Me.DrawPageImage(doc, page)
        '    Next

        '    ' done
        '    doc.Save(fileName)
        'End Sub

        '    Private Sub DrawPageImage(ByVal pdf As C1.C1Pdf.C1PdfDocument, ByVal index As Integer)

        '        ' get image bounds
        '        Dim rcBounds As RectangleF = pdf.PageRectangle
        '        rcBounds.Inflate(-72.0!, -72.0!)

        '        ' calculate zoom factor
        '        Dim img As Image
        '        If (index = 0) Then

        '            ' get size of largest image
        '            Dim sizeMax As SizeF
        '            For Each img In Me.images
        '                sizeMax.Height = Math.Max(sizeMax.Height, CType(img.Height, Single))
        '                sizeMax.Width = Math.Max(sizeMax.Width, CType(img.Width, Single))
        '            Next

        '            ' get size of page bounds
        '            Dim szPage As SizeF = rcBounds.Size

        '            ' calculate zoom so largest image doesn't overflow the page
        '            Me.zoom = 1.0!
        '            Dim zh As Single = (szPage.Width / sizeMax.Width)
        '            Dim zv As Single = (szPage.Height / sizeMax.Height)
        '            If ((zh < 1.0!) OrElse (zv < 1.0!)) Then
        '                Me.zoom = Math.Min(zh, zv)
        '            End If
        '        End If

        '        ' draw grid image
        '        Dim rc As RectangleF = rcBounds
        '        img = images(index)
        '        rc.Width = img.Width * zoom
        '        rc.Height = img.Height * zoom
        '        If (index > 0) Then
        '            pdf.NewPage()
        '        End If
        '        pdf.DrawImage(img, rc)

        '    End Sub
    End Class

    Public Function getUserRights(ByVal argMenuID As Integer) As String
        'to get the user rights from the database
        'parameter -> argMenuID is the code of the menu
        '             for which the rights has to be checked

        'init the return value to false
        '    Dim cmdDummy As New ADODB.Command
        '    Dim rsDummy As New ADODB.Recordset
        Dim strCond As String, strDefRights
        getUserRights = "111111111111"
        strDefRights = "000000000000"
        If gblnUserBased Then
            strCond = " where UserID = " & gUserID & ""
        Else
            strCond = " where RoleID = " & gRoleID & ""
        End If

        strCond = strCond & " and MenuItemId = " & argMenuID
        If gblnUserBased Then
            getUserRights = clsSVT.getDataStringValue("Select UserRights from MstUserRights " & strCond, strDefRights, False)
        Else
            getUserRights = clsSVT.getDataStringValue("Select RoleRights from MstRoleRights " & strCond, strDefRights, False)
        End If

    End Function


    Public Function blnDeleteOK(ByVal argTblName As String,
                    ByVal argFldValue As String, ByVal blnNumeric As Boolean, ByVal blnDispMsg As Boolean, Optional ByVal argFldCode As String = "") As Boolean
        blnDeleteOK = False
        Dim blnFound As Boolean, strFldName As String, strTblName As String, strTablCaption As String, strDispFldName As String, strDocValue As String = "", dblDocValue As Long
        Dim clsSvt As New svtLibrary
        Dim rs As OleDb.OleDbDataReader
        Dim strSQL As String, strDelTblname As String

        'strSQL = "Select * from UtlDeletion where Upper(DelTblName) = '" & UCase(Trim(argTblName)) & "'"

        strSQL = " select * from Utldeletion where " & getSyntax("UPPER") & "(DelTblName) ='" & UCase(Trim(argTblName)) & "'"
        rs = clsSvt.getDataReaderFromSQL(strSQL)
        With rs
            If .HasRows Then
                Do While rs.Read
                    strFldName = GetVal(!Fldname, "")
                    strTblName = GetVal(!TblName, "")
                    strDispFldName = GetVal(!DispFldName, "")
                    If strDispFldName = "" Then strDispFldName = strFldName
                    strTablCaption = GetVal(!TBLCaption, "")
                    If strTablCaption = "" Then strTablCaption = strTblName
                    strDelTblname = GetVal(!DelTblNAme, "")

                    blnFound = False
                    If UCase(Trim(strTblName)) <> UCase(Trim(argTblName)) Then
                        If blnNumeric Then
                            strSQL = "Select " & strFldName & " from " & strTblName & " Where " & strFldName & " = " & argFldValue
                            'strSQL = "Select " & strFldName & " from " & strTblName & " Where " & strFldName & " = '" & (clsSvt.getDataStringValue("Select State from MstSCC where SCCID = " & Val(argFldValue), "", False)) & "'"

                            strDocValue = clsSvt.getDataStringValue(strSQL, "", blnFound)

                            'Call clsSvt.getDataLongValue("Select " & strFldName & " from " & strTblName & " Where " & strFldName & " = " & Val(argFldValue), 0, blnFound)
                        Else
                            strSQL = "Select " & strDispFldName & " from " & strTblName & " Where " & strFldName & " = '" & Trim(argFldValue) & "'"
                            dblDocValue = clsSvt.getDataStringValue(strSQL, "", blnFound)
                            strDocValue = dblDocValue
                            'strSQL = "Select " & strFldName & " from " & strTblName & " Where " & strFldName & " = '" & Trim(argFldValue) & "'"
                            ' Call clsSvt.getDataLongValue(strSQL, "", blnFound)
                        End If
                        If strDispFldName <> "" Then
                            strDispFldName = clsSvt.getDataStringValue("Select  " & strFldName & " as " & strFldName & " from " & strDelTblname & " Where " & strFldName & " = " & Val(argFldValue), "", False)
                        End If
                    End If
                    If blnFound Then
                        blnDeleteOK = False
                        If blnDispMsg Then
                            If argFldCode <> "" Then
                                MsgBox("Cannot Delete!!! " & vbCrLf & "Data Is exists In '" & strDispFldName & "' of '" & (strTablCaption) & "'", vbInformation, "Deletion Check")
                                'MsgBox("Cannot Delete!!! " & vbCrLf & "Data : " & argFldCode & " of " & argTblName & " exists in " & strTblName, vbInformation, "Deletion Check")
                            Else
                                'MsgBox("Cannot Delete!!! " & vbCrLf & "Data is exists in '" & strDispFldName & "' of '" & (strTablCaption) & "'", vbInformation, "Deletion Check")
                                MsgBox("Cannot Delete!!! " & vbCrLf & "Data : " & strDispFldName & " of " & argTblName & " exists in " & (strTablCaption), vbInformation, "Deletion Check")
                            End If
                        End If
                        .Close()
                        Exit Function
                    End If
                Loop
            End If
            .Close()
        End With

        blnDeleteOK = True
    End Function

    Public Function getDataType(ByVal argTblName As String, ByVal argFldName As String) As String
        getDataType = "TEXT"
        Dim strSQL As String
        Dim rsDummy As OleDbDataReader

        strSQL = "Select " & argFldName & " from " & argTblName & " where 1 <> 1"
        rsDummy = clsSVT.getDataReaderFromSQL(strSQL)

        'Pavithra
        If Not rsDummy.HasRows Then
            rsDummy.Close()
            Exit Function
        End If
        '----------------------

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
    Public Sub insertBlankPfx(ByVal argDocID As Integer)
        Dim strsql As String
        strsql = "Insert into UtlDocNoPrefix (DocID, Prefix, "
        strsql = strsql & " cndOperator, CndFldCode, CndFldValue) Values ("
        strsql = strsql & argDocID & ", '', '', '', '') "
        clsSVT.execSQLStatement(strsql)
    End Sub

    Public Sub SetFlexColorFullGrid(ByVal argVsfGrid As C1.Win.C1FlexGrid.C1FlexGrid, ByVal argType As String, Optional ByVal argRow As Int32 = 0)
        Dim csBlack As CellStyle = argVsfGrid.Styles.Add("BLACK")
        csBlack.ForeColor = Color.Black

        Dim csRed As CellStyle = argVsfGrid.Styles.Add("RED")
        csRed.ForeColor = Color.Brown

        Dim csBlue As CellStyle = argVsfGrid.Styles.Add("BLUE")
        csBlue.ForeColor = Color.Blue

        With argVsfGrid
            For i As Int32 = 0 To .Cols.Count - 1
                If argType = "M" Then
                    Dim rgRed As CellRange = .GetCellRange(argRow, i)
                    rgRed.Style = argVsfGrid.Styles("RED")
                Else
                    Dim rgBlue As CellRange = .GetCellRange(argRow, i)
                    rgBlue.Style = argVsfGrid.Styles("BLUE")
                End If
            Next
        End With
    End Sub

    Public Sub SetPBFlexColor(ByVal argVsfGrid As C1FlexGrid)
        With argVsfGrid
            .Styles.EmptyArea.BackColor = Color.Black
            .Styles.Normal.BackColor = Color.Black
            .Styles.Alternate.BackColor = Color.Black
            .Styles.Normal.ForeColor = Color.White
            .Styles.Alternate.ForeColor = Color.White
            .Styles.Fixed.ForeColor = Color.White
            .Styles.Fixed.BackColor = Color.Black
            .Styles.Highlight.BackColor = Color.Yellow
            .Styles.Highlight.ForeColor = Color.Black
            .Styles.Focus.BackColor = Color.Yellow
            .Styles.Focus.ForeColor = Color.Black

            .Font = New Drawing.Font("Segoe UI", 9.75, FontStyle.Regular)

            For i = 0 To .Cols.Count - 1
                Dim rgRed As CellRange = .GetCellRange(0, i)
                rgRed.Style = argVsfGrid.Styles("WHITE")
            Next
        End With
    End Sub

    Public Sub SetPBFlexRowColor(ByVal argVsfGrid As C1FlexGrid)
        With argVsfGrid
            .Styles.EmptyArea.BackColor = Color.Black
            .Styles.Normal.BackColor = Color.Black
            .Styles.Alternate.BackColor = Color.Black
            .Styles.Normal.ForeColor = Color.White
            .Styles.Alternate.ForeColor = Color.White
            .Styles.Fixed.ForeColor = Color.White
        End With
    End Sub


    Public Sub SetFlexColor(ByVal argVsfGrid As C1.Win.C1FlexGrid.C1FlexGrid, ByVal argType As String, ByVal ParamArray IntColColor() As Integer)
        Dim intColClr As Object
        Dim csBlack As CellStyle = argVsfGrid.Styles.Add("BLACK")
        csBlack.ForeColor = Color.Black

        Dim csRed As CellStyle = argVsfGrid.Styles.Add("RED")
        csRed.ForeColor = Color.Brown

        Dim csBlue As CellStyle = argVsfGrid.Styles.Add("BLUE")
        csBlue.ForeColor = Color.Blue

        With argVsfGrid
            For Each intColClr In IntColColor
                If argType = "M" Then
                    Dim rgRed As CellRange = .GetCellRange(0, intColClr)
                    rgRed.Style = argVsfGrid.Styles("RED")
                Else
                    Dim rgBlue As CellRange = .GetCellRange(0, intColClr)
                    rgBlue.Style = argVsfGrid.Styles("BLUE")
                End If
            Next intColClr
        End With
    End Sub

    Public Sub SetRowColFlexColor(ByVal argVsfGrid As C1.Win.C1FlexGrid.C1FlexGrid, ByVal argType As String, ByVal argRow As Integer, ByVal ParamArray IntColColor() As Integer)
        Dim intColClr As Object
        Dim csBlack As CellStyle = argVsfGrid.Styles.Add("BLACK")
        csBlack.ForeColor = Color.Black

        Dim csRed As CellStyle = argVsfGrid.Styles.Add("RED")
        csRed.ForeColor = Color.Brown

        Dim csBlue As CellStyle = argVsfGrid.Styles.Add("BLUE")
        csBlue.ForeColor = Color.Blue

        With argVsfGrid
            For Each intColClr In IntColColor
                If argType = "M" Then
                    Dim rgRed As CellRange = .GetCellRange(argRow, intColClr)
                    rgRed.Style = argVsfGrid.Styles("RED")
                Else
                    Dim rgBlue As CellRange = .GetCellRange(argRow, intColClr)
                    rgBlue.Style = argVsfGrid.Styles("BLUE")
                End If
            Next intColClr
        End With
    End Sub

    Public Sub SetColAlign(ByVal argVsfGrid As C1.Win.C1FlexGrid.C1FlexGrid, ByVal argAlign As svtAlign, ByVal ParamArray intColAlign() As Integer)
        Dim intCol As Integer

        If intColAlign(0) = 999 Then
            For i = 0 To argVsfGrid.Cols.Count - 1
                argVsfGrid.Cols(i).TextAlign = TextAlignEnum.LeftCenter
            Next
            Exit Sub
        End If

        With argVsfGrid
            For Each intCol In intColAlign
                Select Case argAlign
                    Case svtAlign.Left
                        .Cols(intCol).TextAlign = TextAlignEnum.LeftCenter
                    Case svtAlign.Right
                        .Cols(intCol).TextAlign = TextAlignEnum.RightCenter
                    Case svtAlign.Center
                        .Cols(intCol).TextAlign = TextAlignEnum.CenterCenter
                    Case Else
                        .Cols(intCol).TextAlign = TextAlignEnum.LeftCenter
                End Select
            Next intCol
        End With
    End Sub

    Public Sub SetRowAlign(ByVal argVsfGrid As C1.Win.C1FlexGrid.C1FlexGrid, ByVal argAlign As svtAlign, ByVal ParamArray intRowAlign() As Integer)
        Dim intRow As Integer

        If intRowAlign(0) = 999 Then
            For i = 0 To argVsfGrid.Rows.Count - 1
                argVsfGrid.Rows(i).TextAlign = TextAlignEnum.LeftCenter
            Next
            Exit Sub
        End If

        With argVsfGrid
            For Each intRow In intRowAlign
                Select Case argAlign
                    Case svtAlign.Left
                        .Rows(intRow).TextAlign = TextAlignEnum.LeftCenter
                    Case svtAlign.Right
                        .Rows(intRow).TextAlign = TextAlignEnum.RightCenter
                    Case svtAlign.Center
                        .Rows(intRow).TextAlign = TextAlignEnum.CenterCenter
                    Case Else
                        .Rows(intRow).TextAlign = TextAlignEnum.LeftCenter
                End Select
            Next intRow
        End With
    End Sub

    Public Sub SetHideCol(ByVal argVsfGrid As C1.Win.C1FlexGrid.C1FlexGrid, ByVal argType As Boolean, ByVal ParamArray IntColHide() As Integer)
        Dim intHideCol As Integer
        With argVsfGrid
            For Each intHideCol In IntColHide
                If argType = True Then
                    .Cols(intHideCol).Visible = False
                Else
                    .Cols(intHideCol).Visible = True
                End If
            Next intHideCol
        End With
    End Sub

    Public Sub ReArrangeSlNo(ByVal argVsfGrid As C1.Win.C1FlexGrid.C1FlexGrid, ByVal argCol As Integer, Optional argItemSlCol As Integer = -1)
        Dim j As Integer, x As Integer, intMaxNo As Integer
        j = 0
        With argVsfGrid
            For j = 1 To .Rows.Count - 1
                .SetData(j, argCol, j)
            Next j
            'to update maxSlNo
            If argItemSlCol <> -1 Then
                For j = 1 To .Rows.Count - 1
                    If GetVal(.GetData(j, argItemSlCol), "").ToString = "" Then
                        intMaxNo = 0
                        For x = 1 To .Rows.Count - 1
                            If Val(.GetData(x, argItemSlCol)) > intMaxNo Then
                                intMaxNo = Val(.GetData(x, argItemSlCol))
                            End If
                        Next x
                        intMaxNo = intMaxNo + 1
                        .SetData(j, argItemSlCol, intMaxNo)
                    End If
                Next j
            End If
        End With
    End Sub

    Public Sub RemoveEmptyZeroVSFRows(ByVal argVSF As C1FlexGrid, ByVal argMinRowsToKeep As Integer, ByVal ParamArray intCols() As Integer)
        Dim i As Integer, blnBlank As Boolean, ColNo As Object, dblCompare As Double = 0
        With argVSF
Repeat:
            For i = 1 To (.Rows.Count - 1)
                blnBlank = True
                dblCompare = 0
                For Each ColNo In intCols
                    dblCompare = dblCompare + Val(.GetData(i, ColNo))
                Next ColNo
                If dblCompare <> 0 Then blnBlank = False
                If blnBlank Then
                    .RemoveItem(i)
                    GoTo Repeat
                End If
            Next i
            'If All rows will be removed if want to add some blank rows
            If argVSF.Rows.Count = 1 And argMinRowsToKeep <> 0 Then argVSF.Rows.Count = argVSF.Rows.Count + argMinRowsToKeep
        End With

    End Sub

    Public Sub RemoveEmptVSFRows(ByVal argVSF As C1FlexGrid, ByVal argMinRowsToKeep As Integer, ByVal ParamArray intCols() As Integer)
        Dim i As Integer, blnBlank As Boolean, ColNo As Object, strCompare As String
        With argVSF
Repeat:
            For i = 1 To (.Rows.Count - 1)
                blnBlank = True
                strCompare = ""
                For Each ColNo In intCols
                    strCompare = strCompare & Trim$(.GetData(i, ColNo))
                Next ColNo
                If strCompare <> "" Then blnBlank = False
                If blnBlank Then
                    .RemoveItem(i)
                    GoTo Repeat
                End If
            Next i
            'If All rows will be removed if want to add some blank rows
            If argVSF.Rows.Count = 1 And argMinRowsToKeep <> 0 Then argVSF.Rows.Count = argVSF.Rows.Count + argMinRowsToKeep
        End With

    End Sub

    Public Function getColWidth(ByVal argVSF As C1.Win.C1FlexGrid.C1FlexGrid) As String
        Dim strList As String, i As Integer
        strList = ""
        With argVSF
            For i = 0 To .Cols.Count - 1
                strList = strList & .Cols(i).Width & ", "
            Next i
            If strList <> "" Then
                strList = Mid(strList, 1, Len(strList) - 2)
            End If
        End With
        getColWidth = strList
    End Function
    Public Sub SetColWidth(ByVal argVsfGrid As C1FlexGrid, ByVal ParamArray IntColWidth() As Integer)
        Dim i As Integer
        Dim IntColWth As Object
        i = -1
        With argVsfGrid
            For Each IntColWth In IntColWidth
                i = i + 1
                .Cols(i).Width = IntColWth
            Next IntColWth
        End With
    End Sub

    Public Function getMaxInsertID(ByVal argSQL As String, ByVal argTblName As String,
                            Optional ByVal sTrans As OleDbTransaction = Nothing, Optional ByVal argBranchConn As OleDbConnection = Nothing) As Long
        getMaxInsertID = 0
        Try
            Dim cmd As OleDbCommand = New OleDbCommand("mSp_GetMaxID")
            With cmd
                If argBranchConn Is Nothing Then
                    cmd.Connection = svtConn
                Else
                    cmd.Connection = argBranchConn
                End If

                If Not sTrans Is Nothing Then
                    .Transaction = sTrans
                End If
                .CommandType = CommandType.StoredProcedure
                .Parameters.Add("@argSQL", OleDbType.VarChar, 8000, ParameterDirection.Input)
                .Parameters(0).Value = argSQL
                .Parameters.Add("@argTbl", OleDbType.VarChar, 250, ParameterDirection.Input)
                .Parameters(1).Value = argTblName
                .Parameters.Add("@MaxID", OleDbType.Integer)
                .Parameters("@MaxID").Direction = ParameterDirection.Output
                .ExecuteNonQuery()
                getMaxInsertID = GetVal(.Parameters("@MaxID").Value, 0)

            End With

        Catch ex As Exception
            MsgBox(ex.Message)
            getMaxInsertID = -1
        Finally

        End Try

    End Function

    Public Function getAddEditDelID(ByVal argHdrSQL As String, ByVal argDtlQty As String, ByVal argTBLName As String, ByVal argDocID As Long,
                           Optional ByVal sTrans As OleDbTransaction = Nothing) As Long
        getAddEditDelID = 0

        Try
            'Dim cmd As oledbCommand = New oledbCommand("USP_ADD_EDIT_DEL_TRNS")
            Dim cmd As OleDbCommand = New OleDbCommand("mSp_InsertTRN")
            With cmd
                cmd.Connection = svtConn
                If Not sTrans Is Nothing Then
                    .Transaction = sTrans
                End If
                .CommandType = CommandType.StoredProcedure
                .Parameters.Add("@HdrQry", OleDbType.VarChar, -1, ParameterDirection.Input)
                .Parameters(0).Value = argHdrSQL

                .Parameters.Add("@DtlQry", OleDbType.VarChar, -1, ParameterDirection.Input)
                .Parameters(1).Value = argDtlQty

                '.Parameters.Add("@TBLName", OleDbType.VarChar, 100, ParameterDirection.Input)
                '.Parameters(2).Value = argTBLName

                '.Parameters.Add("@Type", OleDbType.integer)
                '.Parameters(3).Value = argDocID

                .Parameters.Add("@DocID", OleDbType.Integer)
                .Parameters("@DocID").Direction = ParameterDirection.Output

                .ExecuteNonQuery()
                getAddEditDelID = GetVal(.Parameters("@DocID").Value, 0)

            End With

        Catch ex As Exception
            MsgBox(ex.Message)
            getAddEditDelID = -1
        Finally

        End Try

    End Function

    Public Function getMaxHdrDtlInsertID(ByVal argSQL As String, ByVal argDtlSql As String, ByVal argTblName As String,
                            Optional ByVal sTrans As OleDbTransaction = Nothing, Optional ByVal argBranchConn As OleDbConnection = Nothing) As Long
        getMaxHdrDtlInsertID = 0

        Try
            Dim cmd As OleDbCommand = New OleDbCommand("mSp_GetMaxHdrID")
            With cmd
                If argBranchConn Is Nothing Then
                    cmd.Connection = svtConn
                Else
                    cmd.Connection = argBranchConn
                End If

                If Not sTrans Is Nothing Then
                    .Transaction = sTrans
                End If

                .CommandType = CommandType.StoredProcedure
                .Parameters.Add("@argSQL", OleDbType.VarChar, -1, ParameterDirection.Input)
                .Parameters(0).Value = argSQL
                .Parameters.Add("@argDtlSql", OleDbType.VarChar, -1, ParameterDirection.Input)
                .Parameters(1).Value = argDtlSql
                .Parameters.Add("@argTbl", OleDbType.VarChar, 250, ParameterDirection.Input)
                .Parameters(2).Value = argTblName
                .Parameters.Add("@MaxID", OleDbType.Integer)
                .Parameters("@MaxID").Direction = ParameterDirection.Output
                .ExecuteNonQuery()
                getMaxHdrDtlInsertID = GetVal(.Parameters("@MaxID").Value, 0)

            End With

        Catch ex As Exception
            MsgBox(ex.Message)
            getMaxHdrDtlInsertID = -1
        Finally

        End Try

    End Function

    Public Function GetRandomString(ByRef iLength As Integer) As String
        Dim rdm As New Random()
        Dim allowChrs() As Char = "abc0123456789".ToCharArray()
        'Dim allowChrs() As Char = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLOMNOPQRSTUVWXYZ0123456789".ToCharArray()
        Dim sResult As String = ""

        For i As Integer = 0 To iLength - 1
            sResult += allowChrs(rdm.Next(0, allowChrs.Length))
        Next

        Return sResult
    End Function

    Public Sub CHKAllowNegativeStock(ByVal argVsfGrid As C1.Win.C1FlexGrid.C1FlexGrid, ByVal argType As Boolean, argQOHCol As Integer)
        If clsSVT.getDataStringValue("select SetValue from UtlGSettings where SetName = 'AllowNegative'", "N", False) = "N" Then
            gBlnAllowNegativeStock = False
            argVsfGrid.Cols(argQOHCol).Visible = True
        Else
            gBlnAllowNegativeStock = True
            argVsfGrid.Cols(argQOHCol).Visible = False
        End If
    End Sub

    Public Function GetRoundNum(ByVal number As Double, ByVal multiple As Integer) As Double
        Dim nRnd As Double
        nRnd = CInt(number / multiple) * multiple
        If nRnd < number Then
            nRnd = nRnd + multiple
        End If
        GetRoundNum = nRnd
    End Function

    Public Function getFmtDocNo(ByVal argPfx As String, ByVal intMaxNo As Long, ByVal argDT As Date)
        Dim strFinYear As String

        strFinYear = Right(Format(gSettings.gCompStDt, "dd/MM/yyyy"), 2)
        If Trim(argPfx) <> "" Then
            getFmtDocNo = Trim(strFinYear)
            getFmtDocNo = getFmtDocNo & Trim(argPfx)
            getFmtDocNo = getFmtDocNo & Format(intMaxNo, "00000")
        Else
            getFmtDocNo = Trim(strFinYear)
            getFmtDocNo = getFmtDocNo & Format(intMaxNo, "00000")
        End If
    End Function

    Public Function getPfxNo(ByVal argDocNo As String, ByRef argPfx As String, ByRef argNo As String)
        'getPfxNo = ""
        'If Trim(argDocNo) = "" Then Exit Function
        'argDocNo = Trim(argDocNo)
        'If Trim(argPfx) <> "" Then
        '    argPfx = Mid(Mid(argDocNo, 3), 1, Len(Mid(argDocNo, 3)) - 5)
        '    argNo = Format(Val(Right(argDocNo, 5)), "00000")
        'Else
        '    argPfx = ""
        '    argNo = Format(Val(Right(argDocNo, 5)), "00000")
        'End If

        Dim i As Integer, intcount As Integer
        getPfxNo = ""
        If Trim(argDocNo) = "" Then Exit Function
        argDocNo = Trim(argDocNo)
        intcount = 0

        For i = 1 To Len(argDocNo)
            If Mid(argDocNo, i, 1) = "/" Then
                intcount = intcount + 1
            End If
        Next i

        'If intcount = 2 Then
        '    argPfx = Mid(Mid(argDocNo, 4), 1, InStr(Mid(argDocNo, 4), "/") - 1)
        '    argNo = Format(Val(Mid(argDocNo, InStr(4, argDocNo, "/") + 1)), "00000")
        'Else
        '    argPfx = ""
        '    argNo = Format(Val(Right(argDocNo, 5)), "00000")
        'End If

        If Len(argDocNo) > 7 Then
            argPfx = Mid(argDocNo, 3, Len(argDocNo) - 7)
            argNo = Format(Val(Right(argDocNo, 5)), "00000")
        Else
            argPfx = ""
            argNo = Format(Val(Right(argDocNo, 5)), "00000")
        End If

    End Function

    Public Sub showWaitCursor()
        Cursor.Current = Cursors.WaitCursor
    End Sub

    Public Sub showNormalCursor()
        Cursor.Current = Cursors.Arrow
    End Sub

    Public Function validDate(ByVal argDocDate As Date) As Boolean
        validDate = False
        Try
            If Format(argDocDate, "yyyy-MM-dd") < Format(gSettings.gCompStDt, "yyyy-MM-dd") Then Exit Function
            If Format(argDocDate, "yyyy-MM-dd") > Format(gSettings.gCompEndDt, "yyyy-MM-dd") Then Exit Function
            validDate = True
        Catch ex As Exception
            validDate = False
        End Try
    End Function

    Public Function saveInAuthTable(ByVal argTrnDocID As Integer, ByVal argDocID As Long,
                                    ByVal blnCalculateNoOfAuth As Boolean, ByVal argNoOfAuth As Integer,
                                    ByVal argAuthQuery As String, ByVal argHdrTblName As String, ByVal argFldName As String,
                                    Optional ByVal sTrans As OleDbTransaction = Nothing) As Boolean
        Dim strSQL As String, i As Integer, intNoOfAuth As Integer

        saveInAuthTable = False
        If Not blnCalculateNoOfAuth Then
            intNoOfAuth = argNoOfAuth
        Else
            intNoOfAuth = clsSVT.getDataLongValue("Select " & argAuthQuery & " from " & argHdrTblName & " Where " & argFldName & " = " & argDocID, 0, False)
        End If
        If intNoOfAuth <= 0 Then intNoOfAuth = 1

        For i = 1 To intNoOfAuth
            strSQL = "insert into TrnAuthorise (TrnDocID, DocID, AuthNumber, Auth, AuthBy, AuthOn, AuthRemarks, Pending, NextAuthNo)"
            strSQL = strSQL & " values ("
            strSQL = strSQL & argTrnDocID & ","
            strSQL = strSQL & argDocID & ","
            strSQL = strSQL & i & ",'N','',Null,'',"
            If i = 1 Then
                strSQL = strSQL & " 'Y',"
            Else
                strSQL = strSQL & " 'N',"
            End If
            If i + 1 <= argNoOfAuth Then
                strSQL = strSQL & i + 1 & ")"
            Else
                strSQL = strSQL & "0)"
            End If
            Call clsSVT.execSQLStatement(strSQL)
        Next i
        saveInAuthTable = True
    End Function

    Public Function getPrintDocFmtNo(ByVal argDocID As Integer,
                            ByVal argDocNo As String,
                            ByVal argPfx As String,
                            ByVal argNo As Long, Optional ByVal sTrans As OleDbTransaction = Nothing)
        Dim strPrntNo As String, strFld As String, strFldValue As String
        Dim i As Integer
        'strNumberResetType As String, strMntPfxList As String
        Dim rsDummy As OleDbDataReader

        rsDummy = clsSVT.getDataReaderFromSQL("Select * from UtlDocNoFmt Where DocID = " & argDocID, sTrans)

        strPrntNo = ""

        'Pavithra
        If Not rsDummy.HasRows Then
            rsDummy.Close()
            Exit Function
        End If
        '----------------------

        With rsDummy
            If .Read Then
                For i = 1 To 14
                    strFld = "F" & i
                    strFldValue = GetVal(rsDummy(strFld), "")
                    If strFldValue <> "" Then
                        Select Case strFldValue
                            Case "1"
                                strPrntNo = strPrntNo & argPfx
                            Case "2"
                                strPrntNo = strPrntNo & Right(gSettings.gCompStDt, 4)
                            Case "3"
                                strPrntNo = strPrntNo & Right(gSettings.gCompEndDt, 4)
                            Case "4"
                                strPrntNo = strPrntNo & Right(gSettings.gCompStDt, 2)
                            Case "5"
                                strPrntNo = strPrntNo & Right(gSettings.gCompEndDt, 2)
                            Case "6"
                                strPrntNo = strPrntNo & Right(argDocNo, 5)
                            Case "7"
                                strPrntNo = strPrntNo & Val(argNo)
                            Case "8"
                                strPrntNo = strPrntNo & GetVal(!Misc1, "")
                            Case "9"
                                strPrntNo = strPrntNo & GetVal(!Misc2, "")
                            Case "10"
                                strPrntNo = strPrntNo & GetVal(!Misc3, "")
                            Case Else
                                If strFldValue = "/" Or strFldValue = "-" Then
                                    If strPrntNo <> "" Then
                                        strPrntNo = strPrntNo & strFldValue
                                    End If
                                Else
                                    strPrntNo = strPrntNo & strFldValue
                                End If

                        End Select
                    End If
                Next i
            End If
            If strPrntNo = "" Then strPrntNo = argDocNo
            .Close()
        End With

        getPrintDocFmtNo = strPrntNo
    End Function

    Public Sub MoveOneRowUp(ByVal argVsfGrid As C1FlexGrid, Optional ByVal argSLNoCol As Integer = -1)
        '€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€
        '€ Function MoveOneRowUp
        '€ Parameters 1. argVsfGrid As Object . eg : vsfDet
        '€            2. argSLNoCol as string . eg :0
        '€ Eg  :  MoveOneRowUp(vsfdet,0)
        '€        this will Move the Position of selected row to One Row Upward
        '€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€
        With argVsfGrid
            If .Row > 1 And .Row < .Rows.Count Then
                .Rows(.Row).Move(.Row - 1)
                '.Row = .Row - 1
                '.RowSel = .Row
                '.Focus()
            End If
        End With
        If argSLNoCol <> -1 Then
            Call ReArrangeSlNo(argVsfGrid, argSLNoCol)
        End If
    End Sub

    Public Sub MoveOneRowDown(ByVal argVsfGrid As C1FlexGrid, Optional ByVal argSLNoCol As Integer = -1)
        '€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€
        '€ Function MoveOneRowDown
        '€ Parameters 1. argVsfGrid As Object . eg : vsfDet
        '€            2. argSLNoCol as string . eg :0
        '€ Eg  :  MoveOneRowDown(vsfdet,0)
        '€        this will Move the Position of selected row to One Row Downward
        '€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€
        With argVsfGrid
            If .Row > 0 And .Row < .Rows.Count - 1 Then
                .Rows(.Row).Move(.Row + 1)
                '.Row = .Row + 1
                '.RowSel = .Row
                '.Focus()
            End If
        End With
        If argSLNoCol <> -1 Then
            Call ReArrangeSlNo(argVsfGrid, argSLNoCol)
        End If
    End Sub

    Public Function TrnNoAutoGen(ByVal argTableName As String,
            ByVal argFieldName As String, ByVal argPfx As String, ByVal argDate As Date,
            ByVal argDocID As Integer, Optional ByVal argCondn As String = "",
            Optional ByVal argPfxWiseNoReset As Boolean = True, Optional argtrn As OleDb.OleDbTransaction = Nothing) As String

        Dim rsDummy As OleDbDataReader
        Dim rsDummymax As OleDbDataReader

        Dim intLenArgPfx As Integer
        Dim strCondn As String
        Dim strFinYear As String, strFinPfx As String
        Dim strDocNoGrp As String, strDocShFrm As String
        Dim MaxNo As Integer

        strFinYear = Right(Format(gSettings.gCompStDt, "dd/MM/yyyy"), 2)

        argPfx = Trim(argPfx)
        intLenArgPfx = Len(argPfx)

        If argCondn <> "" Then
            strCondn = " and " & argCondn
        End If

        If Trim(argPfx) = "" Then
            strFinPfx = strFinYear
        Else
            strFinPfx = strFinYear
            strFinPfx = strFinPfx & Trim(argPfx)
        End If
        '---------------
        strDocNoGrp = clsSVT.getDataStringValue("Select ContDocNoGroup from UtlTransactions Where ContDocNoGroup <> DocShForm and DocId = " & argDocID, "", False, argtrn)

        If argPfxWiseNoReset Then
            'Code for Generate autonumber with prefix 07/Loc/00001

            If strDocNoGrp = "" Then
                'For Non Continuous number 07/Loc/00001,07/Exp/00001
                rsDummy = clsSVT.getDataReaderFromSQL("select max(right(" & argFieldName & ",5)) + 1 as LNO from " _
                                   & argTableName & " where " & getSyntax("MID") & "(" & argFieldName & " ,1," &
                                   Len(strFinPfx) & ") = '" & strFinPfx & "'" &
                                   " and Prefix = '" & argPfx & "'", argtrn)
            Else
                'For Continuous number in similar groups 07/Loc/00001,07/Exp/00002
                rsDummy = clsSVT.getDataReaderFromSQL(" select HdrTblName from UtlTransactions where " & " ContDocNoGroup='" & strDocNoGrp & "'", argtrn)
                MaxNo = 0
                Do While rsDummy.Read()
                    strDocShFrm = clsSVT.getDataStringValue("Select DocShForm from UtlTransactions Where HdrTblName='" & rsDummy!HdrTblName & "'", "", False, argtrn)
                    rsDummymax = clsSVT.getDataReaderFromSQL(" select max(right(" & strDocShFrm & "No,5)) + 1 as lNo from " _
                                       & rsDummy!HdrTblName & " where " & getSyntax("MID") & "(" & strDocShFrm & "No ,1," &
                                       Len(strFinPfx) & ") = '" & strFinPfx & "'" &
                                   " and Prefix = '" & argPfx & "'", argtrn)
                    rsDummymax.Read()
                    If MaxNo < GetVal(rsDummymax!LNo, 0) Then
                        MaxNo = GetVal(rsDummymax!LNo, 0)
                    End If
                    'Pavithra
                    rsDummymax.Close()
                Loop
                'Pavithra
                rsDummy.Close()
            End If
        Else
            'Code for Generate autonumber with out prefix 07/00001
            If strDocNoGrp = "" Then
                'For Non Continuous number 07/00001,07/00001
                rsDummy = clsSVT.getDataReaderFromSQL(" select max(right(" & argFieldName & ",5)) + 1  as LNO" _
                        & " from " & argTableName _
                        & " where  " & getSyntax("MID") & "(" & argFieldName & ",1," & Len(strFinPfx) & ") = '" & strFinPfx & "'", argtrn)
            Else
                'For Continuous number in similar groups 07/00001,07/00002
                rsDummy = clsSVT.getDataReaderFromSQL("Select HdrTblName from UtlTransactions where " &
                               " ContDocNoGroup='" & strDocNoGrp & "'", argtrn)
                MaxNo = 0
                Do While rsDummy.Read()
                    strDocShFrm = clsSVT.getDataStringValue("Select DocShForm from UtlTransactions Where HdrTblName='" & rsDummy!HdrTblName & "'", "", False, argtrn)
                    rsDummymax = clsSVT.getDataReaderFromSQL("Select max(right(" & strDocShFrm & "No,5)) + 1 as LNO from " _
                                       & rsDummy!HdrTblName & " where " & getSyntax("MID") & "(" & strDocShFrm & "No ,1," & "2) = '" & strFinYear & "'", argtrn)
                    If rsDummymax.HasRows Then
                        rsDummymax.Read()
                        If MaxNo < Val(GetVal(rsDummymax!LNO, 1)) Then
                            MaxNo = Val(GetVal(rsDummymax!LNO, 1))
                        End If
                    End If
                    rsDummymax.Close()
                Loop
                rsDummy.Close()
            End If
        End If

        If strDocNoGrp = "" Then
            With rsDummy
                .Read()
                If .HasRows Then
                    TrnNoAutoGen = Val(GetVal(!LNo, 1))
                Else
                    TrnNoAutoGen = 1
                End If
                .Close()
            End With
        Else
            TrnNoAutoGen = MaxNo
        End If

    End Function

    'Public Function BranchTrnNoAutoGen(ByVal argBrchCon As oledbConnection, ByVal argTran As oledbTransaction, ByVal argTableName As String, _
    '       ByVal argFieldName As String, ByVal argPfx As String, ByVal argDate As Date, _
    '       ByVal argDocID As Integer, Optional ByVal argCondn As String = "", _
    '       Optional ByVal argPfxWiseNoReset As Boolean = True) As String

    '    Dim rsDummy As oledbDataReader
    '    Dim rsDummymax As oledbDataReader

    '    Dim intLenArgPfx As Integer
    '    Dim strCondn As String
    '    Dim strFinYear As String, strFinPfx As String
    '    Dim strDocNoGrp As String, strDocShFrm As String
    '    Dim MaxNo As Integer

    '    strFinYear = Right(Format(gSettings.gCompStDt, "dd/mm/yyyy"), 2)

    '    argPfx = Trim(argPfx)
    '    intLenArgPfx = Len(argPfx)

    '    If argCondn <> "" Then
    '        strCondn = " and " & argCondn
    '    End If
    '    If Trim(argPfx) = "" Then
    '        strFinPfx = strFinYear & "/"
    '    Else
    '        strFinPfx = strFinYear & "/" & Trim(argPfx) & "/"
    '    End If
    '    '---------------
    '    strDocNoGrp = clsSVT.getDataStringValue("Select ContDocNoGroup from UtlTransactions Where ContDocNoGroup<>DocShForm and DocId=" & argDocID, "", False)

    '    If argPfxWiseNoReset Then
    '        'Code for Generate autonumber with prefix 07/Loc/00001

    '        If strDocNoGrp = "" Then
    '            'For Non Continuous number 07/Loc/00001,07/Exp/00001
    '            rsDummy = clsSVT.getDataReaderFromSQL("select max(right(" & argFieldName & ",5)) + 1 as LNO from " _
    '                               & argTableName & " where substring(" & argFieldName & " ,1," & _
    '                               Len(strFinPfx) & ") = '" & strFinPfx & "'" & _
    '                               " and Prefix = '" & argPfx & "'", argTran, True)
    '        Else
    '            'For Continuous number in similar groups 07/Loc/00001,07/Exp/00002
    '            rsDummy = clsSVT.getDataReaderFromSQL(" select HdrTblName from UtlTransactions where " & " ContDocNoGroup='" & strDocNoGrp & "'", argTran, True)
    '            MaxNo = 0
    '            Do While rsDummy.Read()
    '                strDocShFrm = clsSVT.getDataStringValue("Select DocShForm from UtlTransactions Where HdrTblName='" & rsDummy!HdrTblName & "'", "", False)
    '                rsDummymax = clsSVT.getDataReaderFromSQL(" select max(right(" & strDocShFrm & "No,5)) + 1 as lNo from " _
    '                                   & rsDummy!HdrTblName & " where substring(" & strDocShFrm & "No ,1," & _
    '                                   Len(strFinPfx) & ") = '" & strFinPfx & "'" & _
    '                               " and Prefix = '" & argPfx & "'", argTran, True)
    '                rsDummymax.Read()
    '                If MaxNo < GetVal(rsDummymax!LNo, 0) Then
    '                    MaxNo = GetVal(rsDummymax!LNo, 0)
    '                End If
    '            Loop
    '        End If
    '    Else
    '        'Code for Generate autonumber with out prefix 07/00001
    '        If strDocNoGrp = "" Then
    '            'For Non Continuous number 07/00001,07/00001
    '            rsDummy = clsSVT.getDataReaderFromSQL(" select max(right(" & argFieldName & ",5)) + 1  as LNO" _
    '                    & " from " & argTableName _
    '                    & " where Substring(" & argFieldName & ",1,2) = '" & strFinYear & "'", argTran, True)
    '        Else
    '            'For Continuous number in similar groups 07/00001,07/00002
    '            rsDummy = clsSVT.getDataReaderFromSQL("Select HdrTblName from UtlTransactions where " & _
    '                           " ContDocNoGroup='" & strDocNoGrp & "'", argTran, True)
    '            MaxNo = 0
    '            Do While rsDummy.Read()
    '                strDocShFrm = clsSVT.getDataStringValue("Select DocShForm from UtlTransactions Where HdrTblName='" & rsDummy!HdrTblName & "'", "", False)
    '                rsDummymax = clsSVT.getDataReaderFromSQL("Select max(right(" & strDocShFrm & "No,5)) + 1 as LNO from " _
    '                                   & rsDummy!HdrTblName & " where Substring(" & strDocShFrm & "No ,1," & "2) = '" & strFinYear & "'", argTran, True)
    '                If rsDummymax.HasRows Then
    '                    rsDummymax.Read()
    '                    If MaxNo < Val(GetVal(rsDummymax!LNO, 1)) Then
    '                        MaxNo = Val(GetVal(rsDummymax!LNO, 1))
    '                    End If
    '                End If
    '            Loop
    '        End If
    '    End If

    '    If strDocNoGrp = "" Then
    '        With rsDummy
    '            .Read()
    '            If .HasRows Then
    '                BranchTrnNoAutoGen = Val(GetVal(!LNo, 1))
    '            Else
    '                BranchTrnNoAutoGen = 1
    '            End If
    '            .Close()
    '        End With
    '    Else
    '        BranchTrnNoAutoGen = MaxNo
    '    End If
    'End Function

    Public Function BranchTrnNoAutoGen(ByVal argBrchCon As OleDbConnection, ByVal argTran As OleDbTransaction, ByVal argTableName As String,
           ByVal argFieldName As String, ByVal argPfx As String, ByVal argDate As Date,
           ByVal argDocID As Integer, Optional ByVal argCondn As String = "",
           Optional ByVal argPfxWiseNoReset As Boolean = True) As String

        Dim rsDummy As OleDbDataReader
        Dim rsDummymax As OleDbDataReader

        Dim intLenArgPfx As Integer
        Dim strCondn As String
        Dim strFinYear As String, strFinPfx As String
        Dim strDocNoGrp As String, strDocShFrm As String
        Dim MaxNo As Integer

        strFinYear = Right(Format(gSettings.gCompStDt, "dd/MM/yyyy"), 2)

        argPfx = Trim(argPfx)
        intLenArgPfx = Len(argPfx)

        If argCondn <> "" Then
            strCondn = " and " & argCondn
        End If
        If Trim(argPfx) = "" Then
            strFinPfx = strFinYear & "/"
        Else
            strFinPfx = strFinYear & "/" & Trim(argPfx) & "/"
        End If
        '---------------
        strDocNoGrp = clsSVT.getDataStringValue("Select ContDocNoGroup from UtlTransactions Where ContDocNoGroup<>DocShForm and DocId=" & argDocID, "", False)

        If argPfxWiseNoReset Then
            'Code for Generate autonumber with prefix 07/Loc/00001

            If strDocNoGrp = "" Then
                'For Non Continuous number 07/Loc/00001,07/Exp/00001
                rsDummy = clsSVT.getDataReaderFromSQL("select max(right(" & argFieldName & ",5)) + 1 as LNO from " _
                                   & argTableName & " where " & getSyntax("MID") & "(" & argFieldName & " ,1," &
                                   Len(strFinPfx) & ") = '" & strFinPfx & "'" &
                                   " and Prefix = '" & argPfx & "'", argTran, argBrchCon)
            Else
                'For Continuous number in similar groups 07/Loc/00001,07/Exp/00002
                rsDummy = clsSVT.getDataReaderFromSQL(" select HdrTblName from UtlTransactions where " & " ContDocNoGroup='" & strDocNoGrp & "'", argTran, argBrchCon)
                MaxNo = 0
                Do While rsDummy.Read()
                    strDocShFrm = clsSVT.getDataStringValue("Select DocShForm from UtlTransactions Where HdrTblName='" & rsDummy!HdrTblName & "'", "", False)
                    rsDummymax = clsSVT.getDataReaderFromSQL(" select max(right(" & strDocShFrm & "No,5)) + 1 as lNo from " _
                                       & rsDummy!HdrTblName & " where " & getSyntax("MID") & "(" & strDocShFrm & "No ,1," &
                                       Len(strFinPfx) & ") = '" & strFinPfx & "'" &
                                   " and Prefix = '" & argPfx & "'", argTran, argBrchCon)
                    rsDummymax.Read()
                    If MaxNo < GetVal(rsDummymax!LNo, 0) Then
                        MaxNo = GetVal(rsDummymax!LNo, 0)
                    End If
                    rsDummymax.Close()
                Loop
                'Pavithra
                rsDummy.Close()
            End If
        Else
            'Code for Generate autonumber with out prefix 07/00001
            If strDocNoGrp = "" Then
                'For Non Continuous number 07/00001,07/00001
                rsDummy = clsSVT.getDataReaderFromSQL(" select max(right(" & argFieldName & ",5)) + 1  as LNO" _
                        & " from " & argTableName _
                        & " where " & getSyntax("MID") & "(" & argFieldName & ",1,2) = '" & strFinYear & "'", argTran, argBrchCon)
            Else
                'For Continuous number in similar groups 07/00001,07/00002
                rsDummy = clsSVT.getDataReaderFromSQL("Select HdrTblName from UtlTransactions where " &
                               " ContDocNoGroup='" & strDocNoGrp & "'", argTran, argBrchCon)
                MaxNo = 0
                Do While rsDummy.Read()
                    strDocShFrm = clsSVT.getDataStringValue("Select DocShForm from UtlTransactions Where HdrTblName='" & rsDummy!HdrTblName & "'", "", False)
                    rsDummymax = clsSVT.getDataReaderFromSQL("Select max(right(" & strDocShFrm & "No,5)) + 1 as LNO from " _
                                       & rsDummy!HdrTblName & " where " & getSyntax("MID") & "(" & strDocShFrm & "No ,1," & "2) = '" & strFinYear & "'", argTran, argBrchCon)
                    If rsDummymax.HasRows Then
                        rsDummymax.Read()
                        If MaxNo < Val(GetVal(rsDummymax!LNO, 1)) Then
                            MaxNo = Val(GetVal(rsDummymax!LNO, 1))
                        End If
                        rsDummymax.Close()
                    End If
                Loop
                'Pavithra
                rsDummy.Close()
                '------------------
            End If
        End If

        If strDocNoGrp = "" Then
            With rsDummy
                .Read()
                If .HasRows Then
                    BranchTrnNoAutoGen = Val(GetVal(!LNo, 1))
                Else
                    BranchTrnNoAutoGen = 1
                End If
                .Close()
            End With
        Else
            BranchTrnNoAutoGen = MaxNo
        End If

    End Function

    Public Sub GenerateUtlFldLst()
        Dim rsDummy As OleDb.OleDbDataReader

        rsDummy = clsSVT.getDataReaderFromSQL("Select * from UtlFindFlds")

        gStrUtlFldLst = ""
        With rsDummy
            If .HasRows Then
                Do While .Read
                    gStrUtlFldLst = gStrUtlFldLst & "'" & GetVal(!StorageFld, "") & "',"
                    If GetVal(!AliasFld1, "") <> "" Then _
                        gStrUtlFldLst = gStrUtlFldLst & "'" & GetVal(!AliasFld1, "") & "',"
                    If GetVal(!AliasFld2, "") <> "" Then _
                        gStrUtlFldLst = gStrUtlFldLst & "'" & GetVal(!AliasFld2, "") & "',"
                    If GetVal(!AliasFld3, "") <> "" Then _
                        gStrUtlFldLst = gStrUtlFldLst & "'" & GetVal(!AliasFld3, "") & "',"
                    If GetVal(!AliasFld4, "") <> "" Then _
                        gStrUtlFldLst = gStrUtlFldLst & "'" & GetVal(!AliasFld4, "") & "',"
                    If GetVal(!AliasFld5, "") <> "" Then _
                        gStrUtlFldLst = gStrUtlFldLst & "'" & GetVal(!AliasFld5, "") & "',"
                Loop
            End If
            .Close()
        End With
        If gStrUtlFldLst <> "" Then gStrUtlFldLst = Mid(gStrUtlFldLst, 1, Len(gStrUtlFldLst) - 1)
    End Sub

    Public Sub SetVsfChecked(ByVal argValue As String, ByVal argVsf As C1.Win.C1FlexGrid.C1FlexGrid, ByVal argRow As Integer, ByVal argCol As Integer)
        If argValue = "Y" Then
            argVsf.SetCellCheck(argRow, argCol, C1.Win.C1FlexGrid.CheckEnum.Checked)
        Else
            argVsf.SetCellCheck(argRow, argCol, C1.Win.C1FlexGrid.CheckEnum.Unchecked)
        End If
    End Sub

    Public Function getVsfCheckedYN(ByVal argVsf As C1.Win.C1FlexGrid.C1FlexGrid, ByVal argRow As Integer, ByVal argCol As Integer) As String
        If argVsf.GetCellCheck(argRow, argCol) = C1.Win.C1FlexGrid.CheckEnum.Checked Then
            getVsfCheckedYN = "'Y'"
        Else
            getVsfCheckedYN = "'N'"
        End If
    End Function
    Public Function getFldDataType(ByVal argColDataType As String) As String
        argColDataType = UCase(argColDataType)
        getFldDataType = "TEXT"
        Select Case argColDataType
            Case "INT32", "DECIMAL", "DOUBLE", "INT16", "INT64", "SINGLE"
                getFldDataType = "NUMERIC"
            Case "DATETIME"
                getFldDataType = "DATE"
        End Select
    End Function
    Public Function chkUserRights(ByVal mRights As String, ByVal mPos As chkRightsPos, Optional ByVal argfrmCaption As String = "", Optional ByVal argblnShowMessage As Boolean = True, Optional ByVal argDocDate As Date = Nothing) As Boolean

        Select Case mPos
            Case chkRightsPos.DelRights, chkRightsPos.EditRights
                If argDocDate = Nothing Then Exit Select
                If CDate(argDocDate) <= gdtFreezeDate Then
                    MsgBox("Document cannot be edited / deleted. Date is before Freezed Date (Global Settings)!!", MsgBoxStyle.Critical, argfrmCaption)
                    chkUserRights = False
                    Exit Function
                End If
        End Select

        Dim clsInfo As New svtLibrary
        If UCase(Trim(gUserCode)) = "ADMIN" Then
            chkUserRights = True
            Exit Function
        End If
        If Mid(mRights, mPos, 1) = "1" Then
            'if 0 then return true
            chkUserRights = True
        Else
            'if 1 then return false
            chkUserRights = False
        End If

        If argblnShowMessage Then
            If Not chkUserRights Then
                If argfrmCaption <> "" Then
                    MsgBox("Access Denied!!", vbInformation, argfrmCaption)
                End If
            End If
        End If
    End Function

    Public Function chkUserRightsOnly(ByVal mRights As String, ByVal mPos As chkRightsPos) As Boolean
        Dim clsInfo As New svtLibrary
        If UCase(Trim(gUserCode)) = "ADMIN" Then
            chkUserRightsOnly = True
            Exit Function
        End If
        If Mid(mRights, mPos, 1) = "1" Then
            'if 0 then return true
            chkUserRightsOnly = True
        Else
            'if 1 then return false
            chkUserRightsOnly = False
        End If
    End Function

    Public Function deleteDocEntry(ByVal argDocID As Long, ByVal argDocNoFldName As String, ByVal argDocNoFldValue As String, ByVal argTblList As String, Optional ByVal argBlnLog As Boolean = False, Optional ByVal argMenu As String = "") As Boolean
        Dim blnExec As Boolean = True
        Dim arrStr() As String, rsADel As OleDbDataReader
        deleteDocEntry = False
        Dim svtTrn As OleDb.OleDbTransaction
        If MsgBox("Are you sure to Delete this Entry ", MsgBoxStyle.YesNo + vbDefaultButton2 + MsgBoxStyle.Critical, "Deletion...") = vbNo Then Exit Function

        arrStr = Split(argTblList, ",")
        svtTrn = svtConn.BeginTransaction(IsolationLevel.ReadCommitted)
        For i = 0 To UBound(arrStr)
            If argBlnLog AndAlso blnExec Then
                'Call RecordAuditLog(argMenu, argDocNoFldValue, UserActions.DeleteAction, svtTrn)
                blnExec = False
            End If
            clsSVT.execSQLStatement("Delete from " & arrStr(i) & " where " & argDocNoFldName & " = " & Val(argDocNoFldValue), svtTrn)
        Next i
        rsADel = clsSVT.getDataReaderFromSQL("Select * from UtlAttachFiles where DocID = " & argDocID & " and DocValueID = '" & Val(argDocNoFldValue) & "'", svtTrn)

        'Pavithra
        If Not rsADel.HasRows Then
            rsADel.Close()
            Return True
        End If
        '----------------------

        With rsADel
            Do While .Read
                If File.Exists(gstrRptPath & "TrndocFiles" & !AFileName) Then
                    Try
                        File.Delete(gstrRptPath & "TrndocFiles" & !AFileName)
                    Catch ex As Exception
                        MsgBox("Cannot delete " & gstrRptPath & "TrndocFiles" & !AFileName & vbCrLf & ex.Message, MsgBoxStyle.Critical, "Error while deleting attachment.")
                    End Try
                End If
            Loop
            .Close()
        End With

        clsSVT.execSQLStatement("Delete from UtlAttachFiles where DocID = " & argDocID & " and DocValueID = '" & Val(argDocNoFldValue) & "'", svtTrn)
        svtTrn.Commit()
        deleteDocEntry = True
    End Function

    Public Function CancelDocEntry(ByVal argDocID As Long, ByVal argDocNoFldName As String, ByVal argDocNoFldValue As String, ByVal argTblList As String, Optional ByVal argBlnLog As Boolean = False, Optional argMenu As String = "") As Boolean
        Dim arrStr() As String, strSQL As String
        CancelDocEntry = False
        Dim svtTrn As OleDb.OleDbTransaction
        'If MsgBox("Are you sure to Cancel this Entry ", MsgBoxStyle.YesNo + vbDefaultButton2 + MsgBoxStyle.Critical, "Cancellation...") = vbNo Then Exit Function

        arrStr = Split(argTblList, ",")

        svtTrn = svtConn.BeginTransaction(IsolationLevel.ReadCommitted)


        strSQL = "update " & argTblList & " set Cancelled = 'Y' where " & argDocNoFldName & " = " & Val(argDocNoFldValue)

        clsSVT.execSQLStatement(strSQL, svtTrn)

        'If argBlnLog Then
        '    Call RecordAuditLog(argMenu, argDocNoFldValue, UserActions.EditAction, svtTrn)
        'End If

        'For i = 0 To UBound(arrStr)
        '    clsSVT.execSQLStatement("update " & arrStr(i) & " set Cancelled = 'Y' where " & argDocNoFldName & " = " & Val(argDocNoFldValue), svtTrn)
        'Next i

        svtTrn.Commit()
        CancelDocEntry = True
    End Function

    Private Function getAuthPresent(ByVal argDocId As Long) As Boolean
        Dim strSQL As String, strDocName As String

        strSQL = "select DocName from UtlTransactions where DOCID = " & argDocId
        strDocName = clsSVT.getDataStringValue(strSQL, "", False)

        Select Case strDocName
            Case "Purchase Order", "Proforma Invoice", "Work Order", "Import GRN Valuation", "Local GRN Valuation",
                            "Jobwork Order", "JobWork GRN", "Stock Adjustment", "Sales Invoice Commercial"
                getAuthPresent = True
            Case Else
                getAuthPresent = False
        End Select
    End Function

    Private Function getUserMailIDList(ByVal argDOCID As Long, Optional ByVal arg2ndLeveAuth As Boolean = False) As String
        Dim strSQL As String
        Dim rs As OleDb.OleDbDataReader

        strSQL = "select EmailID"
        strSQL &= " from MstUserRights d, MstUser h, MstMenu m"
        strSQL &= " where h.UserID = d.UserID"
        strSQL &= " and d.MenuItemID = m.MenuItemID"
        If arg2ndLeveAuth = True Then
            strSQL &= " and " & getSyntax("MID") & "(UserRights, 8, 1) = 1"
        Else
            strSQL &= " and " & getSyntax("MID") & "(UserRights, 6, 1) = 1"
        End If
        strSQL &= " and m.DOCID = " & argDOCID

        rs = clsSVT.getDataReaderFromSQL(strSQL)

        strSQL = ""

        'Pavithra
        If Not rs.HasRows Then
            rs.Close()
            Return ""
        End If
        '----------------------

        With rs
            Do While .Read
                If Trim(GetVal(!EmailID, "")) <> "" Then
                    strSQL = strSQL & !EmailID & ";"
                End If
            Loop
        End With

        rs.Close()

        getUserMailIDList = strSQL
    End Function

    Public Function sendMail(ByVal argDocID As Integer, ByVal argDocNO As String, ByVal argDocDate As Date,
                                    ByVal argPartyCode As String, ByVal argDocFldName As String,
                                    Optional ByVal arg2ndLevel As Boolean = False) As Boolean

        Dim strTOMailID As String = "", strBody As String = "", strSubJect As String, strDocName As String

        sendMail = False

        If getAuthPresent(argDocID) = False Then
            Exit Function
        End If

        strTOMailID = getUserMailIDList(argDocID, arg2ndLevel)

        If Trim(strTOMailID) = "" Then
            Exit Function
        End If

        strDocName = "select DocName from UtlTransactions where DOCID = " & argDocID
        strDocName = clsSVT.getDataStringValue(strDocName, "", False)

        strSubJect = strDocName & " " & argDocNO & " is Pending for Authorization"

        strBody = "Dear Sir/Madam,"
        strBody &= vbCrLf & vbCrLf & strDocName & " No - " & argDocNO & ","
        If Trim(argPartyCode) <> "" Then strBody &= " Party Name - " & argPartyCode & ","
        strBody &= " is pending for authorization."
        strBody &= vbCrLf & vbCrLf & "Please do the needful."

        strBody &= vbCrLf & vbCrLf & "Regards,"
        strBody &= vbCrLf & vbCrLf & gUserCode


        'strTOMailID = "meghana@softvent.com;praveen@softvent.com"

        sendMail = False

        'Try
        '    gSmtpHostName = "smtp.gmail.com"
        '    gSmtpServerPort = 587
        '    gSmtpMailID = "softvent@gmail.com"
        '    gSmtpMailPwd = "sspl@123$$"

        '    Dim mail As New MailMessage()
        '    Dim SmtpServer As New SmtpClient(gSmtpHostName)
        '    mail.From = New MailAddress(gSmtpMailID)

        '    mail.[To].Add(Trim(strTOMailID))

        '    strSubJect = ""

        '    mail.Subject = strSubJect
        '    mail.Body = strBody
        '    mail.IsBodyHtml = True

        '    SmtpServer.Port = gSmtpServerPort
        '    SmtpServer.Credentials = New System.Net.NetworkCredential(gSmtpMailID, gSmtpMailPwd)
        '    SmtpServer.EnableSsl = True

        '    SmtpServer.Send(mail)

        'Catch ex As Exception
        '    MsgBox("Error in Sending Mail!" & vbCrLf & ex.Message, MsgBoxStyle.Critical)
        'End Try

        Try
            Dim oApp As Outlook._Application
            oApp = New Interop.Outlook.Application

            Dim oMsg As Interop.Outlook._MailItem
            oMsg = oApp.CreateItem(Interop.Outlook.OlItemType.olMailItem)

            oMsg.Subject = strSubJect
            oMsg.Body = strBody

            oMsg.To = strTOMailID
            'oMsg.CC = sCC


            'Dim strS As String = sFilename
            'Dim strN As String = sDisplayname

            'If sFilename <> "" Then
            '    Dim sBodyLen As Integer = Int(sBody.Length)
            '    Dim oAttachs As Interop.Outlook.Attachments = oMsg.Attachments
            '    Dim oAttach As Interop.Outlook.Attachment

            '    oAttach = oAttachs.Add(strS, , sBodyLen, strN)
            'End If

            oMsg.Send()

        Catch ex As Exception
            MsgBox("Error in Sending Mail!" & vbCrLf & ex.Message, MsgBoxStyle.Critical)
        End Try

        sendMail = True
    End Function

    'Public Sub sendMail(ByVal argFrm As Form, ByVal argBody As String, ByVal argFilePath As String, Optional ByVal blnFilePresent As Boolean = True)
    '    Dim arrToMailIDs() As String, i As Integer

    '    Try
    '        Dim Smtp_Server As New SmtpClient
    '        Dim e_mail As New MailMessage()

    '        Smtp_Server.UseDefaultCredentials = False
    '        Smtp_Server.Credentials = New Net.NetworkCredential(gSmtpMailID, gSmtpMailPwd)
    '        Smtp_Server.Port = gSmtpServerPort
    '        If gSmtpEnableSSL = "Y" Then
    '            Smtp_Server.EnableSsl = True
    '        Else
    '            Smtp_Server.EnableSsl = False
    '        End If
    '        Smtp_Server.Host = gSmtpHostName

    '        e_mail = New MailMessage()
    '        e_mail.From = New MailAddress(gSmtpMailID)
    '        arrToMailIDs = Split(gRptEmailIds, ";")
    '        For i = 0 To UBound(arrToMailIDs)
    '            e_mail.To.Add(arrToMailIDs(i))
    '        Next

    '        e_mail.Subject = argFrm.Text
    '        e_mail.IsBodyHtml = False
    '        e_mail.Body = argBody


    '        If blnFilePresent Then
    '            If System.IO.File.Exists(argFilePath) Then
    '                e_mail.Attachments.Add(New Attachment(argFilePath))
    '            Else
    '                MsgBox("File not found!", vbInformation, argFrm.Text)
    '                Exit Sub
    '            End If
    '        End If

    '        'If System.IO.File.Exists(txtAttach1.Text) Then e_mail.Attachments.Add(New Attachment(txtAttach1.Text))
    '        'If System.IO.File.Exists(txtAttach2.Text) Then e_mail.Attachments.Add(New Attachment(txtAttach2.Text))
    '        'If System.IO.File.Exists(txtAttach3.Text) Then e_mail.Attachments.Add(New Attachment(txtAttach3.Text))
    '        'If System.IO.File.Exists(txtAttach4.Text) Then e_mail.Attachments.Add(New Attachment(txtAttach4.Text))
    '        'If System.IO.File.Exists(txtAttach5.Text) Then e_mail.Attachments.Add(New Attachment(txtAttach5.Text))

    '        'Cursor.Current = Cursors.WaitCursor
    '        'Smtp_Server.Send(e_mail)
    '        'Cursor.Current = Cursors.Arrow

    '        Cursor.Current = Cursors.WaitCursor
    '        Smtp_Server.Send(e_mail)
    '        Cursor.Current = Cursors.Arrow
    '        MsgBox("Mail sent successfully!", vbInformation, argFrm.Text)

    '        'If Not chkManual.Checked Then
    '        '    Call clsSVT.execSQLStatement("Update UtlMail set MailSent = 'Y' where MailID = " & txtMailID.Text, svtConn)
    '        'Else
    '        '    MsgBox("Mail Sent Successfully!", vbInformation, Me.Text)
    '        'End If

    '    Catch error_t As Exception
    '        MsgBox("Mail not sent!" & vbCrLf & error_t.Message, vbCritical)
    '    End Try
    'End Sub                                                                                                                                                                                      

    'Public Function isFrozen(ByVal argDocID As Integer, ByVal argDocDate As Date) As Boolean
    '    Dim dtFreezeDate As Date
    '    Dim blnFound As Boolean
    '    dtFreezeDate = clsSVT.getDataDateValue("Select FreezeDocDate from UtlInvSettings where DocID = " & argDocID, getSystemDate, blnFound)

    '    If blnFound Then
    '        dtFreezeDate = Format(dtFreezeDate, "dd MMM yyyy")

    '        If argDocDate < dtFreezeDate Then
    '            isFrozen = True
    '            Exit Function
    '        End If
    '    End If

    '    isFrozen = False
    'End Function

    Public Sub ShowMstFindGrid(ByVal argFrm As Object, ByVal argTblName As String, ByVal argDocID As Integer, Optional ByVal argDT As String = "", Optional ByVal argOrderByfld As String = "")

        Dim strSQL As String, strFmt As String
        strFmt = ""

        Select Case argFrm.Name
            Case "frmMstMultUom"
                strSQL = "SELECT m.IDNO AS IdentityNo,u.UOM as UOMFrom ,uu.UOM as UOMTo"
                strSQL = strSQL & " ,m.CONVFACTOR AS ConvFactor"
                strSQL = strSQL & " FROM MSTMULTUOM m,MstUOM u,MstUOM uu"
                strSQL = strSQL & " Where 1 = 1"
                strSQL = strSQL & " and m.UOMFrom = u.UOMID and m.UOMTo = uu.UOMID"
            Case Else
                strSQL = getShowFindQry(argTblName, strFmt, argDT, argOrderByfld)   'If argOrderByFld is given,
                'take care,OrderBy field of UtlSelCols is empty.
        End Select
        Dim frmDisplay As New frmUtlMstView
        'Dim frmDisplay As New frmUtlMstView

        'strSelCols = clsSVT.GetDataValue("SelCols", "", " UtlSelCols", blnFlag, _
        '                " where UserCode='" & gUsrCode & "' and formTag='" & argTblName & "'")
        'strRights = getUserRights(argDocID)
        'With frmDisplay
        '    .Caption = argFrm.Caption & " ... Listing"
        '    Call .initScreenSQL(argFrm, strSQL, argTblName, 0)
        '    .Show()
        '    .vsfSearch.SetFocus()
        '    For i = 1 To .vsfSearch.Cols - 1
        '        If Not .vsfSearch.ColHidden(i) Then
        '            .vsfSearch.Col = i
        '            Exit For
        '        End If
        '    Next i
        '    .lblTotRec.Caption = .vsfView.Rows - 1
        '    If Not chkOnlyUserRights(strRights, printRights) Then
        '        .cmdExport.Enabled = False
        '    Else
        '        .cmdExport.Enabled = True
        '    End If
        'End With
    End Sub

    Public Function chkDocId(ByVal argDocID As Integer, ByVal argUserCode As String, ByVal argFormCaption As String) As Boolean
        chkDocId = False
        If argDocID = 0 And UCase(argUserCode) <> "ADMIN" Then
            MsgBox("Cannot continue, entry in security not made !!", vbInformation + vbOKOnly, argFormCaption)
            Exit Function
        End If
        chkDocId = True
    End Function

    '# A - Get the syntax based on the connection type
    'argData and argSize needed only for Cstr and IsNUll
    Public Function getSyntax(ByVal argString As String, Optional ByVal argData As String = "", Optional ByVal argSize As Integer = 0) As String
        getSyntax = argString
        Select Case UCase(Trim(argString))
            Case "UCASE", "UPPER"
                If gDBTypeCurrent = gDbTypeMSSQL Then
                    getSyntax = "UPPER"
                Else
                    getSyntax = "UCASE"
                End If
            Case "MID"
                If gDBTypeCurrent = gDbTypeMSSQL Then
                    getSyntax = "SUBSTRING"
                End If
            Case "CSTR"
                'If argSize = "" Or argData = "" Then Exit Function
                If gDBTypeCurrent = gDbTypeMSSQL Then
                    getSyntax = "Convert(varchar(" & argSize & ")," & argData & ")"
                Else
                    getSyntax = "CStr(" & argData & ")"
                End If
            Case "ISNULL"
                'If argSize = 0 Or argData = "" Then Exit Function
                If gDBTypeCurrent = gDbTypeMSSQL Then
                    getSyntax = "Isnull(" & argData & ",'')"
                Else
                    getSyntax = "iif(isnull(" & argData & "),''," & argData & ")"
                End If
            Case "ISNULL0"
                'If argSize = 0 Or argData = "" Then Exit Function
                If gDBTypeCurrent = gDbTypeMSSQL Then
                    getSyntax = "Isnull(" & argData & ",0)"
                Else
                    getSyntax = "iif(isnull(" & argData & "),0," & argData & ")"
                End If

        End Select

    End Function
    Public Function getImgSyntax(ByVal argString As String, Optional ByVal argData As String = "") As String
        getImgSyntax = 0
        If gDBTypeCurrent = gDbTypeMSSQL Then
            getImgSyntax = "IsNull(" & argData & ",'')"
        Else
            getImgSyntax = argData
        End If
    End Function

    Public Sub setColTypeDouble(ByVal argVsf As C1FlexGrid, ByVal ParamArray intCols() As Integer)
        With argVsf
            For Each col In intCols
                .Cols(col).DataType = GetType(Double)
                .Cols(col).Format = "0.00"
            Next
        End With
    End Sub

    Public Sub setColTypeInteger(ByVal argVsf As C1FlexGrid, ByVal ParamArray intCols() As Integer)
        With argVsf
            For Each col In intCols
                .Cols(col).DataType = GetType(Integer)
            Next
        End With
    End Sub

    Public Function GetFrmId(ByVal argfrmCaption As String) As Integer
        Dim strCond As String
        strCond = "where " & getSyntax("UCASE") & "(DocName)='" & UCase(Trim$(argfrmCaption)) & "'"
        GetFrmId = clsSVT.getDataLongValue("Select DocID from UtlTransactions " & strCond, 0, False)
    End Function

    Public Function GetMenuId(ByVal argfrmCaption As String) As Integer
        Dim strCond As String
        strCond = "where " & getSyntax("UCASE") & "(MenuItemCaption)='" & UCase(Trim$(argfrmCaption)) & "'"
        Select Case My.Application.Info.AssemblyName
            Case "PosRestaurant"
                strCond &= " and ModuleName = 'R'"
            Case "PosInv"
                strCond &= " and ModuleName = 'I'"
            Case "PosPay"
                strCond &= " and ModuleName = 'P'"
            Case "PosFA"
                strCond &= " and ModuleName = 'A'"
        End Select
        GetMenuId = clsSVT.getDataLongValue("Select MenuItemID from MstMenu " & strCond, 0, False)
    End Function

    Public Function genVsfMultiUomList(ByVal argItemID As Long) As String
        Dim rsDummy As OleDb.OleDbDataReader, strSQL As String

        strSQL = " Select u.UOMCode as UOM from MstItem i ,MstUOM u "
        strSQL = strSQL & " where i.UOMId=U.UOMId"
        strSQL = strSQL & " and i.ItemId=" & argItemID
        strSQL = strSQL & " Union "
        strSQL = strSQL & " Select u.UOMCode as UOM "
        strSQL = strSQL & " from MstMultUOM m,MstItem i,MstUOM u"
        strSQL = strSQL & " where m.UOMFrom  = i.UOMID "
        strSQL = strSQL & " and i.ItemID = " & argItemID
        strSQL = strSQL & " and u.UOMID = m.UOMto"

        rsDummy = clsSVT.getDataReaderFromSQL(strSQL)

        Dim CboString As String
        CboString = "|"

        'Pavithra
        If Not rsDummy.HasRows Then
            rsDummy.Close()
            Return ""
        End If
        '----------------------

        With rsDummy
            Do While .Read()
                CboString = CboString & "|" & .GetValue(0)
            Loop
            .Close()
        End With
        genVsfMultiUomList = CboString
    End Function

    Public Function getBatchQOH(ByVal argItemID As Long, ByVal argBatch As String, Optional ByVal sTrans As OleDbTransaction = Nothing) As Double
        getBatchQOH = 0
        Try
            Dim cmd As OleDbCommand = New OleDbCommand("getStock")
            With cmd
                cmd.Connection = svtConn
                If Not sTrans Is Nothing Then
                    .Transaction = sTrans
                End If
                .CommandType = CommandType.StoredProcedure
                .Parameters.Add("@mItemID", OleDbType.Integer)
                .Parameters(0).Value = argItemID
                .Parameters.Add("@mLOTNO", OleDbType.VarChar, 50, ParameterDirection.Input)
                .Parameters(1).Value = argBatch
                .Parameters.Add("@mQOH", OleDbType.Decimal)
                .Parameters("@mQOH").Direction = ParameterDirection.Output
                .ExecuteNonQuery()
                getBatchQOH = GetVal(.Parameters("@mQOH").Value, 0)
            End With
        Catch ex As Exception
            MsgBox(ex.Message)
            getBatchQOH = 0
        Finally
        End Try
    End Function

    Public Function getBatchCount(ByVal argItemID As Long, Optional ByVal sTrans As OleDbTransaction = Nothing) As Double
        getBatchCount = 0
        Try
            Dim cmd As OleDbCommand = New OleDbCommand("getBatchCount")
            With cmd
                cmd.Connection = svtConn
                If Not sTrans Is Nothing Then
                    .Transaction = sTrans
                End If
                .CommandType = CommandType.StoredProcedure
                .Parameters.Add("@mItemID", OleDbType.Integer)
                .Parameters(0).Value = argItemID
                .Parameters.Add("@mBatchCount", OleDbType.VarChar, 50, ParameterDirection.Input)
                .Parameters("@mBatchCount").Direction = ParameterDirection.Output
                .ExecuteNonQuery()
                getBatchCount = GetVal(.Parameters("@mBatchCount").Value, 0)
            End With
        Catch ex As Exception
            MsgBox(ex.Message)
            getBatchCount = 0
        Finally
        End Try
    End Function

    Public Function getSingleBatch(ByVal argItemID As Long, Optional ByVal sTrans As OleDbTransaction = Nothing) As String
        getSingleBatch = ""
        Try
            Dim cmd As OleDbCommand = New OleDbCommand("getSingleBatch")
            With cmd
                cmd.Connection = svtConn
                If Not sTrans Is Nothing Then
                    .Transaction = sTrans
                End If
                .CommandType = CommandType.StoredProcedure
                .Parameters.Add("@mItemID", OleDbType.Integer)
                .Parameters(0).Value = argItemID
                .Parameters.Add("@mLOTNO", OleDbType.VarChar, 50)
                .Parameters("@mLOTNO").Direction = ParameterDirection.Output
                .ExecuteNonQuery()
                getSingleBatch = GetVal(.Parameters("@mLOTNO").Value, "")
            End With
        Catch ex As Exception
            MsgBox(ex.Message)
            getSingleBatch = ""
        Finally
        End Try
    End Function

    Public Sub SetFlexRowColor(ByVal argVsfGrid As C1.Win.C1FlexGrid.C1FlexGrid, ByVal argType As String, ByVal ParamArray IntRowColor() As Integer)
        Dim intRowClr As Object
        Dim csBlack As CellStyle = argVsfGrid.Styles.Add("BLACK")
        csBlack.ForeColor = Color.Black

        Dim csRed As CellStyle = argVsfGrid.Styles.Add("RED")
        csRed.ForeColor = Color.Brown

        Dim csBlue As CellStyle = argVsfGrid.Styles.Add("BLUE")
        csBlue.ForeColor = Color.Blue
        If argVsfGrid.Rows.Count < 1 Then Exit Sub
        With argVsfGrid
            For Each intRowClr In IntRowColor
                If argType = "M" Then
                    Dim rgRed As CellRange = .GetCellRange(intRowClr, 0)
                    rgRed.Style = argVsfGrid.Styles("RED")
                ElseIf argType = "B" Then
                    Dim rgBlack As CellRange = .GetCellRange(intRowClr, 0)
                    rgBlack.Style = argVsfGrid.Styles("BLACK")
                Else
                    Dim rgBlue As CellRange = .GetCellRange(intRowClr, 0)
                    rgBlue.Style = argVsfGrid.Styles("BLUE")
                End If
            Next intRowClr
        End With
    End Sub

    Public Sub SetHideRow(ByVal argVsfGrid As C1.Win.C1FlexGrid.C1FlexGrid, ByVal argType As Boolean, ByVal ParamArray IntRowHide() As Integer)
        Dim intHideRow As Integer
        With argVsfGrid
            For Each intHideRow In IntRowHide
                If argType = True Then
                    .Rows(intHideRow).Visible = False
                Else
                    .Rows(intHideRow).Visible = True
                End If
            Next intHideRow
        End With
    End Sub

    Public Function getItemSearchGridQuery(ByVal argItemQuery As String) As String
        Dim strSQL As String
        If Trim(argItemQuery) <> "" Then
            strSQL = argItemQuery
        Else
            strSQL = "Select " & gstrVsfItemCodeCols
            strSQL = strSQL & " from MstItem "
            strSQL = strSQL & " where InActive = 'N'"
            strSQL = strSQL & " and Itemid > 0"
            strSQL = strSQL & " order by 1"
        End If

        getItemSearchGridQuery = strSQL
    End Function

    Public Sub DocXlExport(ByVal vsfHdr As C1FlexGrid, ByVal vsfDetail As C1FlexGrid, ByVal vsfFooter As C1FlexGrid, ByVal vsfTotal As C1FlexGrid,
                             ByVal DocNo As String, ByVal DocName As String, ByVal SODate As String,
                             ByVal strNameLabel As String, ByVal strName As String,
                             ByVal strAddress As String
                             )

        Dim i As Integer, j As Integer, h As Integer
        Dim RowNo As Integer

        Dim XLAppl As Object, XlBook As Object, XLSheet As Object
        XLAppl = CreateObject("Excel.Application")
        XlBook = XLAppl.Workbooks.Add
        XLSheet = XlBook.Worksheets.Add
        XLAppl.Visible = True

        RowNo = 3
        With XLSheet
            .Range("A1:I1").Merge()
            .Range("A1:I1").value = DocName
            .Range("A1:I1").Font.Size = 14
            .Range("A1:I1").Font.Bold = True
            '.Range("A1:I1").HorizontalAlignment = xlCenter
            '.Range("A1:I1").VerticalAlignment = xlCenter

            .Range("A3:A4").Font.Bold = True
            .Cells(RowNo, 1) = "DocNo" : .Cells(RowNo, 2) = DocNo

            .Cells(RowNo, 4) = "Date" : .Cells(RowNo, 5) = SODate
            .Cells(RowNo, 4).Font.Bold = True

            .Cells(RowNo + 3, 1) = strNameLabel
            .Cells(RowNo + 3, 2) = strName
            .Cells(RowNo + 3, 1).Font.Bold = True

            .Cells(RowNo + 4, 1) = "Address"
            '.Cells(RowNo + 4, 1).VerticalAlignment = xlTop

            .Cells(RowNo + 4, 2) = Trim(Replace(strAddress, Chr(13), "", 1))
            .Cells(RowNo + 4, 1).Font.Bold = True
        End With

        With vsfHdr
            For i = 0 To .Rows.Count - 1
                XLSheet.Cells(RowNo + i, 7) = .GetData(i, 0)
                XLSheet.Cells(RowNo + i, 8) = .GetData(i, 1)
            Next i
            RowNo = RowNo + (.Rows.Count - 1)
            XLSheet.Range("G3:H" & RowNo).Borders.Weight = 2
        End With
        If RowNo < 10 Then RowNo = 10

        h = 0
        With vsfDetail
            For i = 0 To .Rows.Count - 1
                For j = 0 To .Cols.Count - 1
                    If vsfDetail.Cols(j).Visible Then
                        XLSheet.Cells(RowNo + i, j + 1 - h) = .GetData(i, j)
                    Else
                        If vsfDetail.Cols(j).Visible Then h = h + 1
                    End If
                Next j
                h = 0
            Next i
            XLSheet.Range("A" & RowNo & ":I" & RowNo & "").Font.Bold = True
            XLSheet.Range("A" & RowNo & ":I" & (RowNo + vsfDetail.Rows.Count - 1) & "").Borders.Weight = 2
        End With

        RowNo = (RowNo + vsfDetail.Rows.Count - 1) + 3

        With vsfFooter
            For i = 0 To .Rows.Count - 1
                XLSheet.Cells(RowNo + (i + 1), 1) = .GetData(i, 0)
                XLSheet.Cells(RowNo + (i + 1), 1).Borders.Weight = 2
                XLSheet.Cells(RowNo + (i + 1), 2) = .GetData(i, 4)
                XLSheet.Cells(RowNo + (i + 1), 2).Borders.Weight = 2
            Next i
        End With

        With vsfTotal
            For i = 0 To .Rows.Count - 1
                XLSheet.Cells(RowNo + (i + 1), 8) = .GetData(i, 0)
                XLSheet.Cells(RowNo + (i + 1), 8).Borders.Weight = 2
                XLSheet.Cells(RowNo + (i + 1), 9) = .GetData(i, 4)
                XLSheet.Cells(RowNo + (i + 1), 9).Borders.Weight = 2
            Next i
        End With

    End Sub

    Public Function GetFileExtType(ByVal fileExtension As String) As String
        Dim FileExtensionName As String = ""
        'fileExtension = "." & fileExtension
        For Each subKey As String In Registry.ClassesRoot.GetSubKeyNames()
            If String.IsNullOrEmpty(subKey) Then
                Continue For
            End If

            If subKey.CompareTo(fileExtension) = 0 Then
                Dim defaultValue As String = Registry.ClassesRoot.OpenSubKey(subKey).GetValue("").ToString()

                If defaultValue.Length = 0 Then
                    Exit For
                End If

                If FileExtensionName.Length = 0 Then
                    FileExtensionName = defaultValue
                    fileExtension = FileExtensionName
                Else
                    If defaultValue.Length > 0 Then
                        FileExtensionName = defaultValue
                    End If
                    Exit For
                End If
            End If
        Next
        Return FileExtensionName
    End Function

    Public Sub SetFixedHeaderCenter(ByVal argForm As Form)
        Dim ctrl As Control = Nothing
        Dim argVsfGrid As C1FlexGrid
        Do
            ctrl = argForm.GetNextControl(ctrl, True)

            If ctrl Is Nothing Then
                Exit Do
            End If
            If ctrl.GetType = GetType(C1.Win.C1FlexGrid.C1FlexGrid) Then
                argVsfGrid = ctrl
                With argVsfGrid
                    For i = 0 To .Cols.Count - 1
                        .Cols(i).TextAlignFixed = TextAlignEnum.CenterCenter
                    Next
                    For i = 0 To .Rows.Count - 1
                        .Rows(i).TextAlignFixed = TextAlignEnum.CenterCenter
                    Next
                End With

            End If
        Loop Until ctrl Is Nothing
    End Sub

    Public Sub setRowTypeInteger(ByVal argVsf As C1FlexGrid, ByVal ParamArray intRows() As Integer)
        With argVsf
            For Each row In intRows
                .Rows(row).DataType = GetType(Integer)
            Next
        End With
    End Sub

    Public Sub setRowTypeDouble(ByVal argVsf As C1FlexGrid, ByVal ParamArray intRows() As Integer)
        With argVsf
            For Each row In intRows
                .Rows(row).DataType = GetType(Double)
                .Rows(row).Format = "0.00"
            Next
        End With
    End Sub

    Public Sub SetRowFlexColor(ByVal argVsfGrid As C1.Win.C1FlexGrid.C1FlexGrid, ByVal argType As String, ByVal ParamArray IntRowColor() As Integer)
        Dim intRowClr As Object
        Dim csBlack As CellStyle = argVsfGrid.Styles.Add("BLACK")
        csBlack.ForeColor = Color.Black

        Dim csRed As CellStyle = argVsfGrid.Styles.Add("RED")
        csRed.ForeColor = Color.Red

        Dim csBlue As CellStyle = argVsfGrid.Styles.Add("BLUE")
        csBlue.ForeColor = Color.Blue

        With argVsfGrid
            For Each intRowClr In IntRowColor
                If argType = "M" Then
                    Dim rgRed As CellRange = .GetCellRange(intRowClr, 0)
                    rgRed.Style = argVsfGrid.Styles("RED")
                Else
                    Dim rgBlue As CellRange = .GetCellRange(intRowClr, 0)
                    rgBlue.Style = argVsfGrid.Styles("BLUE")
                End If
            Next intRowClr
        End With
    End Sub

    'Public Sub SetCellFlexImage(ByVal argVsf As C1FlexGrid, ByVal argRow As Integer, ByVal argCol As Integer, Optional ByVal argPath As String = "")

    '    Dim imgPath As String = AppDomain.CurrentDomain.BaseDirectory
    '    Try
    '        imgPath = imgPath.Replace("\bin\Debug", "\Resources\red.png").TrimEnd("\\", "/")

    '        If argPath = "" Then
    '            argVsf.SetCellImage(argRow, argCol, Image.FromFile(imgPath))
    '        Else
    '            argVsf.SetCellImage(argRow, argCol, Image.FromFile(argPath))
    '        End If

    '        argVsf.Cols(argCol).ImageAlign = ImageAlignEnum.RightCenter
    '    Catch ex As Exception
    '        MsgBox("Incorrect file path: '" & ex.Message & "'", vbCritical)
    '        Exit Sub
    '    End Try

    'End Sub

    Public Sub SetCellFlexImage(ByVal argVsf As C1FlexGrid, ByVal argRow As Integer, ByVal argCol As Integer, Optional ByVal argImgName As String = "", Optional ByVal argRemove As Boolean = False, Optional ByVal argImgAlign As C1.Win.C1FlexGrid.ImageAlignEnum = ImageAlignEnum.LeftCenter)

        Dim strFileName As String = ""
        If argRemove Then
            argVsf.SetCellImage(argRow, argCol, Nothing)
            Exit Sub
        End If

        If argImgName <> "" Then
            strFileName = argImgName
        Else
            strFileName = "red.png"
        End If
        Dim imgFile As Image = Nothing
        Try
            strFileName = Left(strFileName, InStr(strFileName, ".") - 1)
            imgFile = My.Resources.ResourceManager.GetObject(strFileName)
        Catch
        Finally
            strFileName = argImgName

            If File.Exists(strFileName) Then
                imgFile = Image.FromFile(strFileName)
            End If

        End Try

        Try
            argVsf.SetCellImage(argRow, argCol, imgFile)

            argVsf.Cols(argCol).ImageAlignFixed = argImgAlign
            argVsf.Cols(argCol).ImageAlign = argImgAlign
        Catch ex As Exception
            MsgBox("Incorrect Image assignment -- " & ex.Message, vbCritical)
            Exit Sub
        End Try
    End Sub

    Public Function updateStock(ByVal argTRNShform As String, ByVal argTrnDocId As Integer, ByVal argDocDate As DateTime, ByVal argDocId As Long,
                            ByVal argDocNo As String, ByVal argItemID As Long, ByVal argDocQty As Long, ByVal argLOCID As Long,
                            Optional ByVal argFactor As Integer = 1, Optional ByVal argParty As String = "", Optional ByVal argDocOrder As Integer = 1,
                            Optional ByVal argFinYear As Integer = 0,
                            Optional ByVal sTrans As OleDbTransaction = Nothing) As Boolean
        Dim strSQL As String
        updateStock = True

        strSQL = "Insert into TrnStock (TRNSHForm, TrnDocID, DocDate, DocID, DocNo, Particulars,"
        strSQL = strSQL & " ItemID, DocQty, MFactor, LOCID, DocOrder, FinYear, WAR, FIFORate) values ("
        strSQL = strSQL & "'" & argTRNShform & "',"
        strSQL = strSQL & argTrnDocId & ","
        strSQL = strSQL & "'" & getDateFormatInInsert(argDocDate) & "',"
        strSQL = strSQL & argDocId & ","
        strSQL = strSQL & "'" & argDocNo & "',"
        strSQL = strSQL & "'" & argParty & "',"
        strSQL = strSQL & Val(argItemID) & ","
        strSQL = strSQL & Val(argDocQty) & ","
        strSQL = strSQL & argFactor & ","
        strSQL = strSQL & argLOCID & ","
        strSQL = strSQL & argDocOrder & ","
        strSQL = strSQL & argFinYear & ","
        strSQL = strSQL & " 0, 0)"

        If Not clsSVT.execSQLStatement(strSQL, sTrans) Then
            updateStock = False
            Exit Function
        End If
    End Function

    Public Function getTempQOH() As Double
        getTempQOH = 100
    End Function

    Public Function getCustQOH(argItemID As Long, argCustID As Integer, Optional sTrans As OleDb.OleDbTransaction = Nothing) As Single
        getCustQOH = 100

        Dim cmd As OleDbCommand = New OleDbCommand("mSP_GetQOH")
        With cmd

            If Not sTrans Is Nothing Then
                cmd.Transaction = sTrans
            End If

            cmd.Connection = svtConn
            .CommandType = CommandType.StoredProcedure

            .Parameters.Add("@mItemID", OleDbType.Integer)
            .Parameters(0).Value = argItemID
            .Parameters.Add("@mCustID", OleDbType.Integer)
            .Parameters(1).Value = argCustID
            .Parameters.Add("@mQOH", OleDbType.Decimal)
            .Parameters("@mQOH").Direction = ParameterDirection.Output
            .ExecuteNonQuery()
            getCustQOH = GetVal(.Parameters("@mQOH").Value, 0)
        End With

    End Function


    Public Function getQOH(ByVal argFromDate As Date, ByVal argToDate As Date, ByVal argLocation As Long, ByVal argItemID As Long) As Double
        'Dim strSQL As String

        'strSQL = "select sum(docQty) as Qty from vwStock where ItemID = " & argItemID
        'strSQL = strSQL & " and DocDate <= " & getDateFormatInCond(argToDate)

        'strSQL = "select sum(MFactor * DocQty) as Qty from TRNStock where ItemId = " & argItemID & " and LOCID = " & argLocation & " and DocDate <= " & getDateFormatInCond(argToDate)

        getQOH = 100 'clsSVT.getDataDoubleValue(strSQL, 0, False)

    End Function

    'Public Function getQOH(ByVal argFromDate As Date, ByVal argToDate As Date, Optional ByVal argLocId As Integer = 0, Optional ByVal argItemID As Integer = 0) As Single
    '    Dim strSQL As String
    '    Dim clsInfo As New svtLibrary
    '    Dim intFinYear As Integer
    '    Dim sngQOH As Single
    '    Dim rsDummy As oledbDataReader, rsQOH As oledbDataReader

    '    intFinYear = Val(gSettings.gCompFinYear)

    '    strSQL = "Select * from UtlStock order by DocOrderBy"

    '    sngQOH = 0

    '    rsDummy = clsSVT.getDataReaderFromSQL(strSQL)

    '    With rsDummy
    '        Do While .Read
    '            StMsg("Getting QOH from " & !DocName)
    '            strSQL = "Select isnull(" & !StkUpdate & " * sum(" & !qtyFld & "), 0) as fldQty "
    '            strSQL = strSQL & Replace(!Condition, "`", "'")
    '            If !FinYear = "Y" Then
    '                strSQL = strSQL & " and FinYear = " & intFinYear
    '            End If

    '            strSQL = strSQL & " and " & !ItemFld & " = " & argItemID
    '            If Trim(GetVal(!LocFld, "")) <> "" Then
    '                If argLocId > 0 Then
    '                    strSQL = strSQL & " and " & !LocFld & " = " & argLocId
    '                End If
    '            End If
    '            If Trim(GetVal(!DateFld, "")) <> "" Then
    '                strSQL = strSQL & " and " & getDateCond(!DateFld)
    '            End If

    '            rsQOH = clsSVT.getDataReaderFromSQL(strSQL)

    '            With rsQOH
    '                Do While .Read
    '                    sngQOH = sngQOH + GetVal(!FldQty, 0)
    '                Loop
    '            End With
    '        Loop
    '    End With

    '    getQOH = sngQOH
    '    sngConvFactor = clsInfo.GetDataValue("Convfactor", 1, "vwMultiUOM ", False, "where ItemID = " & argItemID & " and uomID = " & argUOMID)
    '    If sngConvFactor <= 0 Then sngConvFactor = 1
    '    getQOH = Round(sngQOH / sngConvFactor, 2)
    '    StMsg()

    '    rsDummy = Nothing
    '    rsQOH = Nothing
    'End Function

    Public Function WriteToTempFile(ByVal Data As String) As String
        ' Writes text to a temporary file and returns path
        Dim strFilename As String = System.IO.Path.GetTempFileName()
        Dim objFS As New System.IO.FileStream(strFilename,
        System.IO.FileMode.Append,
        System.IO.FileAccess.Write)
        ' Opens stream and begins writing
        Dim Writer As New System.IO.StreamWriter(objFS)
        Writer.BaseStream.Seek(0, System.IO.SeekOrigin.End)
        Writer.WriteLine(Data)
        Writer.Flush()
        ' Closes and returns temp path
        Writer.Close()
        Return strFilename
    End Function

    Public Function MousePoint(ByVal argFrm As Form, Optional ByVal argCursorType As String = "")
        MousePoint = False
        If Trim(argCursorType) = "" Then
            argFrm.Cursor = System.Windows.Forms.Cursors.Default
        Else
            argFrm.Cursor = System.Windows.Forms.Cursors.WaitCursor
        End If
        MousePoint = True
    End Function

    Public Function getGsettings(ByVal argGroup As String, ByVal argSettings As String, Optional ByVal argEmptyValue As String = "") As String
        Dim strSQL As String
        getGsettings = ""
        Try
            strSQL = "Select setValue from UtlGSettings where upper(SetGroup) = '" & UCase(argGroup) & "' and upper(SetName) = '" & UCase(argSettings) & "'"
            getGsettings = clsSVT.getDataStringValue(strSQL, argEmptyValue, False)
        Catch ex As Exception
            MsgBox("Error in Global Settings.." & argGroup & "-" & argSettings & vbCrLf & ex.Message, vbCritical, "Error")
        End Try
    End Function

    Public Sub ReleaseObjectsFromMemory(ByVal o As Object)
        Try
            While (System.Runtime.InteropServices.Marshal.ReleaseComObject(o) > 0)
            End While
        Catch
        Finally
            o = Nothing
        End Try
    End Sub

    Public Sub MakeColumnBold(ByVal argCol As Integer, ByVal argVsf As C1FlexGrid, Optional ByVal argColType As String = "", Optional ByVal argColStyleType As FontStyle = Nothing, Optional ByVal argBlnColStyle As Boolean = False)
        Dim cs As CellStyle

        If argColType = "" Then
            cs = argVsf.Cols(argCol).StyleNew
        Else
            cs = argVsf.Cols(argCol).StyleFixedNew
        End If

        If argBlnColStyle Then
            cs.Font = New Font(argVsf.Font.Name, argVsf.Font.Size, argColStyleType)
        Else
            cs.Font = New Font(argVsf.Font.Name, argVsf.Font.Size, FontStyle.Bold)
        End If
    End Sub

    Public Sub MakeRowBold(ByVal argRow As Integer, ByVal argVsf As C1FlexGrid, Optional ByVal argRowType As String = "", Optional ByVal argRowStyle As String = "")
        Dim cs As CellStyle

        If argRowType = "" Then
            cs = argVsf.Rows(argRow).StyleNew
        Else
            cs = argVsf.Rows(argRow).StyleFixedNew
        End If

        If Trim(argRowStyle) = "" Then
            cs.Font = New Font(argVsf.Font.Name, argVsf.Font.Size, FontStyle.Bold)
        ElseIf Trim(argRowStyle) = "Regular" Then
            cs.Font = New Font(argVsf.Font.Name, argVsf.Font.Size, FontStyle.Regular)
        End If

    End Sub
    Public Function getCheckSum(ByVal argContent As String) As String
        Dim strContent As String = "", i As Integer, j As Integer, k As Integer
        j = 1
        k = 3
        For i = Len(argContent) To 1 Step -1
            strContent = strContent & Mid(argContent, i, 1)
            If j < Len(argContent) Then strContent = strContent & Mid(argContent, j, 1)
            If k < Len(argContent) Then strContent = strContent & Mid(argContent, k, 1)
            j = j + 2
            If j > Len(argContent) Then j = 3
            k = k + 3
            If k > Len(argContent) Then k = 1
        Next
        getCheckSum = Crc32.ComputeChecksum(Encoding.UTF8.GetBytes(strContent)).ToString
    End Function
    Public Function getCleanString(ByVal argString As String) As String
        Dim iCount As Integer, strRS As String
        argString = UCase(argString)
        strRS = ""
        For iCount = 1 To Len(argString)
            Select Case Asc(Mid(argString, iCount, 1))
                Case 48 To 57, 65 To 90
                    strRS = strRS & Mid(argString, iCount, 1)
            End Select
        Next
        getCleanString = strRS
    End Function

    Public Sub PrintBillCrpt(ByVal argInvID As Long)
        Dim strArrFrmla(,) As String = Nothing
        Dim strRptFileName As String, strSelFormula As String
        Dim strFPName As String, intFCopies As Integer
        Dim strSPName As String, intSCopies As Integer
        Dim rs As OleDb.OleDbDataReader
        Dim strSQL As String
        Dim strFilName As String
        strFilName = getGsettings("Invoice", "InvRptFileName", "FmtInv.Rpt")
        If Not File.Exists(gstrRptPath & strFilName) Then
            MsgBox("Rpt File " & gstrRptPath & strFilName & " does not exist. Cannot print!", MsgBoxStyle.Critical, "Bill Printing")
            Exit Sub
        End If
        strArrFrmla = Nothing

        ReDim strArrFrmla(5, 1)

        strArrFrmla(0, 0) = "compname"
        strArrFrmla(0, 1) = gSettings.gCompName
        strArrFrmla(1, 0) = "add1"
        strArrFrmla(1, 1) = gSettings.gCompAdd1
        strArrFrmla(2, 0) = "add2"
        strArrFrmla(2, 1) = gSettings.gCompAdd2
        strArrFrmla(3, 0) = "add3"
        strArrFrmla(3, 1) = gSettings.gCompAdd3
        strArrFrmla(4, 0) = "add4"
        strArrFrmla(4, 1) = gSettings.gCompAdd4
        strArrFrmla(5, 0) = "gstin"
        strArrFrmla(5, 1) = gSettings.gCompGSTIN

        strSQL = "select FirstPrinterName, FirstPrinterCopies,"
        strSQL = strSQL & " SecPrinterName, SecPrinterCopies"
        strSQL = strSQL & " from MstBCatPrn "
        strSQL = strSQL & " where CSysName = '" & gStrSystemName & "'"
        strRptFileName = gstrRptPath & "FmtInv.Rpt"

        rs = clsSVT.getDataReaderFromSQL(strSQL)
        With rs
            If Not .HasRows Then
                MsgBox("Unable to Print." & vbCrLf & "Billing Printer NOT assigned for this system " & gStrSystemName, MsgBoxStyle.Critical, "Printing of Invoice")
                'Dim frmKOTPrint As New PosRestaurant.frmMstKOTBillPrinterSettings(gStrSystemName)
                'With frmKOTPrint
                '    .ShowDialog()
                'End With
                .Close()
                Exit Sub
            End If
            If .Read Then
                strFPName = GetVal(rs!FirstPrinterName, "")
                intFCopies = GetVal(rs!FirstPrinterCopies, 1)

                strSPName = GetVal(rs!SecPrinterName, "")
                intSCopies = GetVal(rs!SecPrinterCopies, 1)

                If strFPName <> "" Then
                    Dim frmv As New frmUtlReportViewer
                    With frmv
                        .MdiParent = frmMain
                        .StartPosition = FormStartPosition.CenterScreen
                        strSelFormula = "{TRNHDRInv.InvID} =" & argInvID
                        .showRptFile("Printing of Inv", strRptFileName, strSelFormula, strArrFrmla, False, False, True, strFPName, intFCopies)
                        .Close()
                    End With
                End If

                If strSPName <> "" Then
                    Dim frmvSec As New frmUtlReportViewer
                    With frmvSec
                        .MdiParent = frmMain
                        .StartPosition = FormStartPosition.CenterScreen
                        strSelFormula = "{TRNHDRInv.InvID} =" & argInvID
                        .showRptFile("Printing of Inv", strRptFileName, strSelFormula, strArrFrmla, False, False, True, strSPName, intSCopies)
                        .Close()
                    End With
                End If
            End If
            'Pavithra
            .Close()
        End With
    End Sub

    Public Sub PrintKOT(ByVal argKotID As Long, ByVal strKotPrint As String)
        Dim strArrFrmla(,) As String
        Dim strRptFileName As String, strSelFormula As String
        Dim strFPName As String, intFCopies As Integer
        Dim strSPName As String, intSCopies As Integer
        Dim rs As OleDb.OleDbDataReader
        Dim strSQL As String
        ReDim strArrFrmla(5, 1)


        strArrFrmla(0, 0) = "compname"
        strArrFrmla(0, 1) = gSettings.gCompName
        strArrFrmla(1, 0) = "add1"
        strArrFrmla(1, 1) = gSettings.gCompAdd1
        strArrFrmla(2, 0) = "add2"
        strArrFrmla(2, 1) = gSettings.gCompAdd2
        strArrFrmla(3, 0) = "add3"
        strArrFrmla(3, 1) = gSettings.gCompAdd3
        strArrFrmla(4, 0) = "add4"
        strArrFrmla(4, 1) = gSettings.gCompAdd4
        strArrFrmla(5, 0) = "gstin"
        strArrFrmla(5, 1) = gSettings.gCompGSTIN


        If strKotPrint = "N" Then Exit Sub
        If strKotPrint = "YP" Then
            If MsgBox("Do You want To Print KOT ?", MsgBoxStyle.YesNo + MsgBoxStyle.Information + vbDefaultButton2, "KOT Printing") = MsgBoxResult.No Then Exit Sub
        End If

        strSQL = "select distinct k.kcatid, FirstPrinterName, FirstPrinterCopies,"
        strSQL = strSQL & " SecPrinterName, SecPrinterCopies"
        strSQL = strSQL & " from mstitem i, trndtlkot d, MstKCatPrn k"
        strSQL = strSQL & " where i.itemid = d.itemid"
        strSQL = strSQL & " and d.kotid = " & argKotID
        strSQL = strSQL & " and k.KCatID = i.KCatID"
        'strSQL = strSQL & " and k.CatType = 'KOT'"
        strSQL = strSQL & " and k.CSysName = '" & gStrSystemName & "'"

        strRptFileName = getGsettings("Invoice", "KOTRptFileName", "FmtKOT.Rpt")
        If Not File.Exists(gstrRptPath & strRptFileName) Then
            MsgBox("Rpt File " & gstrRptPath & strRptFileName & " does not exist. Cannot print!", MsgBoxStyle.Critical, "KOT Printing")
            Exit Sub
        End If

        rs = clsSVT.getDataReaderFromSQL(strSQL)
        With rs
            If Not .HasRows Then
                'MsgBox("Unable to Print." & vbCrLf & "KOT category of the items may not be assigned Printers for this system " & gStrSystemName, MsgBoxStyle.Critical, "Printing of KOT")
                'MsgBox("Unable to Print." & vbCrLf & "KOT Printer NOT assigned for this system " & gStrSystemName, MsgBoxStyle.Critical, "Printing of KOT")
                'Dim frmKOTPrint As New PosRestaurant.frmMstKOTBillPrinterSettings(gStrSystemName)
                'With frmKOTPrint
                '    .ShowDialog()
                'End With
                .Close()
                Exit Sub
            End If

            Do While .Read
                strFPName = GetVal(rs!FirstPrinterName, "")
                intFCopies = GetVal(rs!FirstPrinterCopies, 1)

                strSPName = GetVal(rs!SecPrinterName, "")
                intSCopies = GetVal(rs!SecPrinterCopies, 1)

                If strFPName <> "" Then
                    Dim frmv As New frmUtlReportViewer
                    With frmv
                        .MdiParent = frmMain
                        .StartPosition = FormStartPosition.CenterScreen
                        strSelFormula = "{MstItem.KCatID} = " & rs!KCatID & " and {TRNHDRKOT.KOTID} =" & argKotID
                        .showRptFile("Printing of KOT", gstrRptPath & strRptFileName, strSelFormula, strArrFrmla, False, False, True, strFPName, intFCopies)
                        .Show()
                    End With
                End If

                If strSPName <> "" Then
                    Dim frmvSec As New frmUtlReportViewer
                    With frmvSec
                        .MdiParent = frmMain
                        .StartPosition = FormStartPosition.CenterScreen
                        strSelFormula = "{MstItem.KCatID} = " & rs!KCatID & " and {TRNHDRKOT.KOTID} =" & argKotID
                        .showRptFile("Printing of KOT", gstrRptPath & strRptFileName, strSelFormula, strArrFrmla, False, False, True, strSPName, intSCopies)
                        .Show()
                    End With
                End If
            Loop
            'Pavithra
            .Close()
        End With
    End Sub

    Public Sub PrintKOTCancel(ByVal argKotID As Long, ByVal strKotPrint As String)
        Dim strArrFrmla(,) As String
        Dim strRptFileName As String, strSelFormula As String
        Dim strFPName As String, intFCopies As Integer
        Dim strSPName As String, intSCopies As Integer
        Dim rs As OleDb.OleDbDataReader
        Dim strSQL As String
        ReDim strArrFrmla(5, 1)


        strArrFrmla(0, 0) = "compname"
        strArrFrmla(0, 1) = gSettings.gCompName
        strArrFrmla(1, 0) = "add1"
        strArrFrmla(1, 1) = gSettings.gCompAdd1
        strArrFrmla(2, 0) = "add2"
        strArrFrmla(2, 1) = gSettings.gCompAdd2
        strArrFrmla(3, 0) = "add3"
        strArrFrmla(3, 1) = gSettings.gCompAdd3
        strArrFrmla(4, 0) = "add4"
        strArrFrmla(4, 1) = gSettings.gCompAdd4
        strArrFrmla(5, 0) = "gstin"
        strArrFrmla(5, 1) = gSettings.gCompGSTIN


        If strKotPrint = "N" Then Exit Sub
        If strKotPrint = "Y" Then
            If MsgBox("Do you want to print KOT cancel?", MsgBoxStyle.YesNo + MsgBoxStyle.Information + vbDefaultButton2, "KOT Cancel Printing") = MsgBoxResult.No Then Exit Sub
        End If

        strSQL = "select distinct k.kcatid, FirstPrinterName, FirstPrinterCopies,"
        strSQL = strSQL & " SecPrinterName, SecPrinterCopies"
        strSQL = strSQL & " from mstitem i, trndtlkotCancel d, MstKCatPrn k"
        strSQL = strSQL & " where i.itemid = d.itemid"
        strSQL = strSQL & " and d.kotid = " & argKotID
        strSQL = strSQL & " and k.KCatID = i.KCatID"
        'strSQL = strSQL & " and k.CatType = 'KOT'"
        strSQL = strSQL & " and k.CSysName = '" & gStrSystemName & "'"

        strRptFileName = getGsettings("KOT", "KOTCancelRptFileName", "FmtCancelKOT.Rpt")
        If Not File.Exists(gstrRptPath & strRptFileName) Then
            MsgBox("Rpt File " & gstrRptPath & strRptFileName & " does not exist. Cannot print!", MsgBoxStyle.Critical, "KOT Printing")
            Exit Sub
        End If

        rs = clsSVT.getDataReaderFromSQL(strSQL)
        With rs
            If Not .HasRows Then
                'MsgBox("Unable to Print." & vbCrLf & "KOT category of the items may not be assigned Printers for this system " & gStrSystemName, MsgBoxStyle.Critical, "Printing of KOT")
                'MsgBox("Unable to Print." & vbCrLf & "KOT Printer NOT assigned for this system " & gStrSystemName, MsgBoxStyle.Critical, "Printing of KOT Cancel")
                'Dim frmKOTPrint As New PosRestaurant.frmMstKOTBillPrinterSettings(gStrSystemName)
                'With frmKOTPrint
                '    .ShowDialog()
                'End With
                Exit Sub
            End If

            Do While .Read
                strFPName = GetVal(rs!FirstPrinterName, "")
                intFCopies = GetVal(rs!FirstPrinterCopies, 1)

                strSPName = GetVal(rs!SecPrinterName, "")
                intSCopies = GetVal(rs!SecPrinterCopies, 1)

                If strFPName <> "" Then
                    Dim frmv As New frmUtlReportViewer
                    With frmv
                        .MdiParent = frmMain
                        .StartPosition = FormStartPosition.CenterScreen
                        strSelFormula = "{MstItem.KCatID} = " & rs!KCatID & " and {TRNHDRKOTCancel.KOTID} =" & argKotID
                        .showRptFile("Printing of KOT", gstrRptPath & strRptFileName, strSelFormula, strArrFrmla, False, False, True, strFPName, intFCopies)
                        .Show()
                    End With
                End If

                If strSPName <> "" Then
                    Dim frmvSec As New frmUtlReportViewer
                    With frmvSec
                        .MdiParent = frmMain
                        .StartPosition = FormStartPosition.CenterScreen
                        strSelFormula = "{MstItem.KCatID} = " & rs!KCatID & " and {TRNHDRKOTCancel.KOTID} =" & argKotID
                        .showRptFile("Printing of KOT", gstrRptPath & strRptFileName, strSelFormula, strArrFrmla, False, False, True, strSPName, intSCopies)
                        .Show()
                    End With
                End If
            Loop
            'Pavithra
            .Close()
        End With
    End Sub

    Public Sub PrintInvCrpt(ByVal argInvID As Long, Optional argFileName As String = "", Optional argPreview As Boolean = True, Optional argPDFPreview As Boolean = False, Optional argNoofCopies As Integer = 1)
        If gstrInvPrintFromSP = "Y" Then
            Call PrintInvCrptStoreProc(argInvID, argFileName, argPreview, argPDFPreview, argNoofCopies)
            Exit Sub
        End If
        If gstrInvPrintFromXL = "Y" Then
            Call PrintInvXL(argInvID, argPreview)
            Exit Sub
        End If

        If argFileName = "" Then
            argFileName = "FmtSInv.Rpt"
        End If
        Dim strFormulaFld As String = "", strFormulaVal As String = ""
        Dim strSelFormula As String
        strSelFormula = "{TRNHDRSInv.SInvID} =" & argInvID

        Dim frmv As New frmUtlReportViewer
        With frmv
            .StartPosition = FormStartPosition.CenterScreen
            strFormulaFld = "compname;add1;add2;add3;add4;gstin;phno"
            strFormulaVal = "MstComp.CompName;MstComp.Address1;MstComp.Address2;MstComp.Address3;MstComp.Address4;MstComp.GSTNO;MstComp.PHNO"
            If argPreview = True Then
                Dim strFileName As String = ""
                If argPDFPreview Then
                    strFileName = "Invoice_" & Date.Now.ToString("yyyyMMddHHmmssfff")
                End If
                .ShowRptFile("", argFileName, strFormulaFld, strFormulaVal, strSelFormula, argBlnPDF:=argPDFPreview, argPDFFileName:=strFileName)
                If argPDFPreview Then
                    Process.Start(Application.StartupPath & "\Temp\" & strFileName & ".pdf")
                Else
                    .Text = "Sales Invoice"
                    .Show()
                End If
            Else
                .ShowRptFile("", argFileName, strFormulaFld, strFormulaVal, strSelFormula, gStrDefPrinter, 1)
            End If
        End With
    End Sub

    Public Sub PrintInvCrptStoreProc(ByVal argInvID As Long, Optional argFileName As String = "", Optional argPreview As Boolean = True, Optional argPDFPreview As Boolean = False, Optional argPrintCopies As Integer = 1)

        If argFileName = "" Then
            argFileName = "FmtSInv.Rpt"
        End If
        Dim frmv As New frmUtlReportViewer
        With frmv
            .StartPosition = FormStartPosition.CenterScreen
            If argPreview = True Then
                .showSPRpt(argFileName, argInvID, "", argPrintCopies)
                .Text = "Sales Invoice"
                .Show()
                .BringToFront()
            Else
                .showSPRpt(argFileName, argInvID, gStrDefPrinter, argPrintCopies)
            End If
        End With
    End Sub

    Public Function getCrptDateFmt(ByVal argDate As Date) As String
        'changed
        getCrptDateFmt = "Date("
        getCrptDateFmt = getCrptDateFmt & Year(argDate) & ","
        getCrptDateFmt = getCrptDateFmt & Month(argDate) & ","
        'getCrptDateFmt = getCrptDateFmt & Day(argDate)
        getCrptDateFmt = getCrptDateFmt & Microsoft.VisualBasic.DateAndTime.Day(argDate)
        getCrptDateFmt = getCrptDateFmt & ")"
    End Function
    Private Function CalculateChecksum(ByVal dataToCalculate As String) As String
        Dim byteToCalculate As Byte() = Encoding.ASCII.GetBytes(dataToCalculate)

        Dim checksum As Integer = 0

        For Each chData As Byte In byteToCalculate
            checksum += chData
        Next

        checksum = checksum And &HFF

        Return checksum.ToString("X2")
    End Function

    Public Sub syncCustSuppLedger()
        Try
            Dim blnFound As Boolean, strSQL As String
            Dim strAccType As String, strMstTblName As String, strIDFld As String, strNameFld As String, strCodeFld As String, intMstDocID As Integer
            Call clsSVT.getDataLongValue("Select AcLedID from MstAcLed where AcLedID = 0", 0, blnFound)
            If Not blnFound Then
                strSQL = "Insert into MstAcLed (AcLedID, AcLedName, LedType, LedMstID, InActive, LedGroupID) values ("
                strSQL = strSQL & "0,'-','-',0,'N',0)"
                clsSVT.execSQLStatement(strSQL)
            End If

            'ledgers, customers/suppliers not in MstAcLed
            Dim rsMst As OleDb.OleDbDataReader
            rsMst = clsSVT.getDataReaderFromSQL("Select DocID, AccMstType, HdrTblName, DocShForm from UtlTransactions where AccMstType <> '' and doctype = 'Master'")

            'Pavithra
            If Not rsMst.HasRows Then
                rsMst.Close()
                Exit Sub
            End If
            '----------------------

            With rsMst
                Do While .Read
                    intMstDocID = GetVal(!DocID, 0)
                    strAccType = GetVal(!AccMstType, "")
                    strMstTblName = GetVal(!HdrTblName, "")
                    strIDFld = GetVal(!DocShForm, "") & "ID"
                    strNameFld = GetVal(!DocShForm, "") & "Name"
                    strCodeFld = GetVal(!DocShForm, "") & "Code"

                    'for every record in UtlTransactions where AccMstType <> '' insert the values from the table into MstAccLed
                    Dim rs As OleDb.OleDbDataReader
                    rs = clsSVT.getDataReaderFromSQL("Select * from " & strMstTblName & " where " _
                            & strIDFld & " > 0 and " & strIDFld & " not in (select LedMstID from MstAcLed where MstDocID = " & intMstDocID & ")")

                    'Pavithra
                    If Not rs.HasRows Then
                        rs.Close()
                        GoTo skiprsreader
                    End If
                    '----------------------

                    With rs
                        Do While .Read
                            strSQL = "Insert into MstAcLed (AcLedID, AcLedName, LedType, LedMstID, InActive, LedGroupID, MstDocID) values ("
                            strSQL = strSQL & clsSVT.AutoIncrementedNum("AcLedID", "MstAcLed") & ","
                            If GetVal(rs(strCodeFld), "") = GetVal(rs(strNameFld), "") Then
                                strSQL = strSQL & "'" & GetVal(rs(strCodeFld), "") & "', "
                            Else
                                strSQL = strSQL & "'" & GetVal(rs(strNameFld), "") & ")" & GetVal(rs(strCodeFld), "") & ")', "
                            End If
                            strSQL = strSQL & " '" & strAccType & "', "
                            strSQL = strSQL & GetVal(rs(strIDFld), "") & ", 'N',"
                            strSQL = strSQL & GetVal(rs!LedGroupID) & ","
                            strSQL = strSQL & intMstDocID & ")"
                            clsSVT.execSQLStatement(strSQL)
                        Loop
                        .Close()
                    End With
                    clsSVT.execSQLStatement("Delete from MstAcLed where LedMstID not in (Select " & strIDFld & " from " & strMstTblName & ") and MstDocID = " & intMstDocID)
skiprsreader:
                Loop
                .Close()
            End With
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical)
        End Try

    End Sub

    Public Function UpdtTrnToAccVch(ByVal argHdrID As Long, ByVal argStrSHForm As String, ByVal argHdrTblName As String,
                                    ByVal argDocNo As String, ByVal argDocDate As Date, ByVal argCreditDays As Integer,
                                    ByVal strTrnDrCr As String, intAcLedID As Integer, dblAccValue As Double, ByVal argSvtTrn As OleDbTransaction) As Boolean
        Dim strsql As String, intFactor As Integer, lngAccID As Long
        Dim cmd As New OleDbCommand(), insMode As String
        Dim intAccID As Long
        intAccID = clsSVT.getDataLongValue("Select AccID from TrnhdrACC where TrnID = " & argHdrID & " and DocType = '" & argStrSHForm & "'", 0, False, argSvtTrn)
        insMode = "A"
        If intAccID > 0 Then
            insMode = "M"
            Call clsSVT.execSQLStatement("Delete from TrnDtlAcc where AccID = " & intAccID, argSvtTrn)
            Call clsSVT.execSQLStatement("Delete from TrnDtlAccAdj where AccID = " & intAccID, argSvtTrn)
        End If

        cmd.Connection = svtConn
        cmd.CommandType = CommandType.Text
        If Not argSvtTrn Is Nothing Then
            cmd.Transaction = argSvtTrn
        End If
        UpdtTrnToAccVch = False
        intFactor = 0

        'add to header file
        If insMode = "A" Then
            strsql = "insert into trnHdrAcc (DocType,AccType, "
            strsql = strsql & " TrnID, TrnNo, TrnDate, DueDate,Narrn"
            strsql = strsql & ") output inserted.AccID into @testTable values ("
            strsql = strsql & "'" & argStrSHForm & "','B',"
            strsql = strsql & argHdrID & ","
            strsql = strsql & "'" & argDocNo & "',"
            strsql = strsql & "'" & getDateFormatInInsert(argDocDate) & "', "
            strsql = strsql & "'" & getDateFormatInInsert(DateAdd(DateInterval.Day, argCreditDays, argDocDate)) & "', "
            strsql = strsql & "'')"
            lngAccID = getMaxInsertID(strsql, argHdrTblName, argSvtTrn)
            If lngAccID = -1 Then
                UpdtTrnToAccVch = False
                Exit Function
            End If
        End If
        If insMode = "M" Then
            lngAccID = intAccID
        End If

        strsql = "insert into trnDtlAcc (AccID,DrAmt, CrAmt,AcLedID) values ("
        strsql = strsql & lngAccID & ","
        If strTrnDrCr = "D" Then
            strsql = strsql & dblAccValue & ","
            strsql = strsql & "0,"
        Else
            strsql = strsql & "0,"
            strsql = strsql & dblAccValue & ","
        End If
        strsql = strsql & intAcLedID & ")"

        If Not clsSVT.execSQLStatement(strsql, argSvtTrn) Then
            UpdtTrnToAccVch = False
            Exit Function
        End If
        UpdtTrnToAccVch = True
    End Function


    Public Function getNumberFromAccFormat(ByVal argString As String) As Double
        getNumberFromAccFormat = 0
        argString = UCase(argString)
        If InStr(argString, "CR") > 0 Then
            getNumberFromAccFormat = -1 * Val(Trim(Replace(argString, "CR", "")))
        ElseIf InStr(argString, "DR") > 0 Then
            getNumberFromAccFormat = Val(Trim(Replace(argString, "DR", "")))
        Else
            getNumberFromAccFormat = Val(argString)
        End If
    End Function

    Public Function validateCombo(txt As ComboBox) As Boolean
        If Trim(txt.Text) = "" Then Return True
        If Not txt.Items.Contains(txt.Text.ToString) Then
            Return False
        Else
            Return True
        End If
    End Function

    Public Function getAcBalance(ByVal argAcLedID As Long) As Double
        getAcBalance = 0
        Dim strSQL As String
        Dim rs As OleDbDataReader
        strSQL = "Select  sum(dramt - cramt)  as balAmt from trndtlacc "
        strSQL = strSQL & " where ACLedID = " & argAcLedID
        Try
            rs = clsSVT.getDataReaderFromSQL(strSQL)
            Do While rs.Read
                getAcBalance = GetVal(rs!BalAmt, 0)
            Loop
            rs.Close()
        Catch ex As Exception
            MsgBox(ex.Message & vbCrLf & "During getting balance (getAcBalance)", MsgBoxStyle.Critical, "Error")
            getAcBalance = 0
        End Try
    End Function

    Public Function UpdateAuditLog(ByVal argEntity As String, ByVal argAction As String, ByVal argParticulars As String,
                           Optional ByVal sTrans As OleDbTransaction = Nothing) As Boolean
        Dim strSQL As String
        UpdateAuditLog = True

        strSQL = "Insert into UtlAuditLog (LogEntity, LogAction, LogParticulars, LogUserName, LogDateTime) values ("
        strSQL = strSQL & "'" & argEntity & "',"
        strSQL = strSQL & "'" & argAction & "',"
        strSQL = strSQL & "'" & argParticulars & "',"
        strSQL = strSQL & "'" & gUserCode & "',"
        strSQL = strSQL & "'" & getDateFormatInInsert(getSystemDateTime(), True) & "')"

        If Not clsSVT.execSQLStatement(strSQL, sTrans) Then
            UpdateAuditLog = False
            Exit Function
        End If
    End Function

    Public Function getcol(ByVal lngCol As Long) As String
        Dim strPfx As String
        strPfx = ""
        If lngCol >= 0 And lngCol <= 25 Then
            strPfx = ""
        ElseIf lngCol >= 26 And lngCol <= 51 Then
            strPfx = "A"
        ElseIf lngCol >= 52 And lngCol <= 77 Then
            strPfx = "B"
        ElseIf lngCol >= 78 And lngCol <= 103 Then
            strPfx = "C"
        ElseIf lngCol >= 104 And lngCol <= 129 Then
            strPfx = "D"
        ElseIf lngCol >= 130 And lngCol <= 155 Then
            strPfx = "E"
        ElseIf lngCol >= 156 And lngCol <= 181 Then
            strPfx = "F"
        ElseIf lngCol >= 182 And lngCol <= 207 Then
            strPfx = "G"
        ElseIf lngCol >= 208 And lngCol <= 233 Then
            strPfx = "H"
        ElseIf lngCol >= 234 Then
            strPfx = "I"
        End If
        getcol = strPfx & Chr(65 + lngCol Mod 26)
        '' till here old code commented if the col is more than 26 then
        'type mismatch error above which takes care more than 26 cols in XL
        '    If lngCol >= 0 And lngCol <= 26 Then
        '        getcol = Chr(65 + lngCol)
        '    ElseIf lngCol >= 27 And lngCol <= 52 Then
        '        getcol = "A" & Chr(65 + lngCol)
        '    ElseIf lngCol >= 53 And lngCol <= 78 Then
        '        getcol = "B" & Chr(65 + lngCol)
        '    ElseIf lngCol >= 79 And lngCol <= 104 Then
        '        getcol = "C" & Chr(65 + lngCol)
        '    ElseIf lngCol >= 105 And lngCol <= 130 Then
        '        getcol = "D" & Chr(65 + lngCol)
        '    ElseIf lngCol >= 131 And lngCol <= 156 Then
        '        getcol = "E" & Chr(65 + lngCol)
        '    ElseIf lngCol >= 157 And lngCol <= 182 Then
        '        getcol = "F" & Chr(65 + lngCol)
        '    ElseIf lngCol >= 183 And lngCol <= 208 Then
        '        getcol = "G" & Chr(65 + lngCol)
        '    ElseIf lngCol >= 209 And lngCol <= 234 Then
        '        getcol = "H" & Chr(65 + lngCol)
        '    ElseIf lngCol >= 235 Then
        '        getcol = "I" & Chr(65 + lngCol)
        '    End If
    End Function

    Public Function GetExcelColumnName(ByVal columnNumber As Integer) As String
        Dim columnName As String = String.Empty
        Dim dividend As Integer = columnNumber
        Dim modulo As Integer

        While dividend > 0
            modulo = (dividend - 1) Mod 26
            columnName = Chr(65 + modulo) & columnName
            dividend = (dividend - modulo) \ 26
        End While

        Return columnName
    End Function

    Public Sub TextBoxSizeChanged(sender As Object, e As EventArgs)
        Dim textBoxRect As Size = TextRenderer.MeasureText(sender.Text, sender.Font, New Size(sender.Width, Integer.MaxValue), TextFormatFlags.WordBreak Or TextFormatFlags.TextBoxControl)
        Try
            sender.ScrollBars = If(textBoxRect.Height > sender.Height, ScrollBars.Vertical, ScrollBars.None)
        Catch ex As Exception
            'handle error
        End Try
    End Sub
    Public Function getPORmks(argPOID As Long) As String
        getPORmks = ""

        Dim clsSVT As New svtLibrary
        Dim strSQL As String

        strSQL = "select RMKs from trnhdrPO where POID = " & argPOID

        getPORmks = clsSVT.getDataStringValue(strSQL, "", False)
    End Function

    Public Function upDtQTNQty(ByVal argQTNID As Long, ByVal argDocDate As Date, Optional ByVal sTrans As OleDbTransaction = Nothing) As Boolean
        upDtQTNQty = False

        Dim clsSVT As New svtLibrary
        Dim strSQL As String, rs As OleDb.OleDbDataReader
        Dim dblQOHQty, dblBlockqty, dblDIGQty, dblQTNQty, dblSTQty As Double

        strSQL = "update TrnDtlQTN set SCWRTPO = 'N' where SCWRTPO is null or SCWRTPO = ''"

        If Not sTrans Is Nothing Then
            Call clsSVT.execSQLStatement(strSQL, sTrans)
        Else
            Call clsSVT.execSQLStatement(strSQL)
        End If

        strSQL = "select * from TrnDtlQTN where QTNID = " & argQTNID

        If Not sTrans Is Nothing Then
            rs = clsSVT.getDataReaderFromSQL(strSQL, sTrans)
        Else
            rs = clsSVT.getDataReaderFromSQL(strSQL)
        End If

        dblQTNQty = 0
        dblQOHQty = 0
        dblBlockqty = 0
        dblDIGQty = 0

        With rs
            Do While .Read
                dblQTNQty = !QTNQty

                dblQOHQty = GetStock("Y", 1, !ItemID, argDocDate, argDocDate, , sTrans)

                strSQL = "select sum(BlockedQty) from TrnDtlQTN d, TrnHdrQTN h"
                strSQL &= " where h.QTNID = d.QTNID"
                strSQL &= " and SCWRTPO = 'N'"
                strSQL &= " and Cancelled = 'N'"
                strSQL &= " and Authorised = 'Y'"
                strSQL &= " and ItemID = " & !ItemID
                'strSQL &= " and SizeL = " & !SizeL
                'strSQL &= " and SizeW = " & !SizeW
                'strSQL &= " and AddnlDesc = '" & !AddnlDesc & "'"
                dblBlockqty = clsSVT.getDataDoubleValue(strSQL, 0, False, sTrans)

                strSQL = "select sum(DIQty) from TrnHdrDI h, TrnDtlDI d, TrnDtlQTN q "
                strSQL &= " where h.DIID = d.DIID"
                strSQL &= " and h.QTNId = d.QTNID"
                strSQL &= " and h.Cancelled = 'N' "
                strSQL &= " and d.QTNID = q.QTNID "
                strSQL &= " and d.SizeL = q.SizeL "
                strSQL &= " and d.SizeW = q.SizeW "
                strSQL &= " and d.ItemId = q.ItemId"
                strSQL &= " and d.AddnlDesc = q.AddnlDesc "
                strSQL &= " and d.ItemID = " & !ItemID
                'strSQL &= " and d.SizeL = " & !SizeL
                'strSQL &= " and d.SizeW = " & !SizeW
                'strSQL &= " and d.AddnlDesc = '" & !AddnlDesc & "'"
                dblDIGQty = clsSVT.getDataDoubleValue(strSQL, 0, False, sTrans)

                dblQOHQty = dblQOHQty - (dblBlockqty - dblDIGQty)

                If dblQOHQty >= dblQTNQty Then
                    dblBlockqty = dblQTNQty
                    dblSTQty = 0
                Else
                    dblBlockqty = dblQOHQty
                    dblSTQty = dblQTNQty - dblQOHQty
                End If

                strSQL = "update TrnDtlQTN set BlockedQty = " & dblBlockqty & ","
                strSQL &= " SHORTAGEQTY = " & dblSTQty
                strSQL &= " where QTNID = " & !QTNID
                strSQL &= " and ItemID = " & !ItemID
                strSQL &= " and SizeL = " & !SizeL
                strSQL &= " and SizeW = " & !SizeW
                strSQL &= " and AddnlDesc = '" & !AddnlDesc & "'"

                If Not clsSVT.execSQLStatement(strSQL, sTrans) Then
                    Exit Function
                End If
            Loop

            .Close()
        End With

        upDtQTNQty = True
    End Function

    'Public Sub UpdtAuditTrailTable(argDOCID As Integer, argAction As String, argDocRef As String, Optional ByVal sTrans As oledbTransaction = Nothing)
    '    If argAuditReq <> "Y" Then Exit Sub

    '    Dim strSQL As String
    '    strSQL = "Insert into UtlAudit ( DocID, UserID, UserAction, ActionDateTime, ActionDate, docNo )"
    '    strSQL = strSQL & " values ("
    '    strSQL = strSQL & argDOCID & ", "
    '    strSQL = strSQL & gUserID & ", "
    '    strSQL = strSQL & "'" & argAction & "', "

    '    If gDBTypeCurrent = gDbTypeAccess Then
    '        strSQL = strSQL & "'" & Format(getSystemDateTime, "dd/MM/yyyy hh:mm:ss") & "',"
    '        strSQL = strSQL & "'" & Format(getSystemDateTime, "dd/MM/yyyy") & "',"
    '    Else
    '        strSQL = strSQL & "'" & Format(getSystemDateTime, "dd MMM yyyy hh:mm:ss") & "',"
    '        strSQL = strSQL & "'" & Format(getSystemDateTime, "dd MMM yyyy") & "',"
    '    End If

    '    strSQL = strSQL & "'" & argDocRef & "') "

    '    Call clsSVT.execSQLStatement(strSQL, sTrans)
    'End Sub

    Public Sub Lock(ByVal argCtrl As Control, ByVal argBln As Boolean)
        If TypeOf argCtrl Is TextBox Then
            Dim ctrl As TextBox = argCtrl
            ctrl.ReadOnly = Not argBln
            ctrl.BackColor = Color.White
            'argCtrl.Enabled = argBln
            ctrl.ForeColor = Color.Black
        Else
            argCtrl.Enabled = argBln
            argCtrl.BackColor = Color.White
            argCtrl.ForeColor = Color.Black
        End If
    End Sub
    Public Function vsfCopyItemCode(ByVal argVSF As C1FlexGrid, ByVal argRow As Int32, ByVal argcol As Int32) As Boolean
        Dim strItemCode As String
        vsfCopyItemCode = False
        strItemCode = ""
        Try
            If GetVal(argVSF.GetData(argRow, argcol), "") = "" Then
                MsgBox("No Items Prsent to Copy!!", vbCritical)
                Exit Function
            End If
            strItemCode = GetVal(argVSF.GetData(argRow, argcol), "")

            Dim o As New DataObject
            o.SetText(strItemCode)
            Clipboard.SetDataObject(o, True)


        Catch ex As Exception
            vsfCopyItemCode = False
        End Try

    End Function
    Public Function CopyPendQry(argsql As String) As Boolean
        Dim Strsql As String
        CopyPendQry = False
        Strsql = ""
        Try
            'If GetVal(argVSF.GetData(argRow, argcol), "") = "" Then
            '    MsgBox("No Items Prsent to Copy!!", vbCritical)
            '    Exit Function
            'End If
            Strsql = argsql

            Dim o As New DataObject
            o.SetText(Strsql)
            Clipboard.SetDataObject(o, True)


        Catch ex As Exception
            CopyPendQry = False
        End Try

    End Function

    Public Sub CopyGrid(ByVal argvsf As C1FlexGrid, ByVal argCopytype As String)
        With argvsf
            Select Case argCopytype
                Case "Cell"
                    If argvsf.RowSel >= 0 AndAlso argvsf.ColSel >= 0 Then
                        Dim cellValue As String = argvsf(argvsf.RowSel, argvsf.ColSel).ToString()
                        Clipboard.SetText(cellValue)
                    End If

                Case "Column"
                    If argvsf.ColSel >= 0 Then
                        Dim colIndex As Integer = argvsf.ColSel
                        Dim result As String = ""
                        For rowIndex As Integer = 0 To argvsf.Rows.Count - 1
                            If argvsf(rowIndex, colIndex) IsNot Nothing Then
                                result &= argvsf(rowIndex, colIndex).ToString() & vbCrLf
                            Else
                                result &= vbCrLf
                            End If
                        Next
                        Clipboard.SetText(result)
                    End If
                Case "Row"
                    If argvsf.RowSel >= 0 Then
                        Dim rowIndex As Integer = argvsf.RowSel
                        Dim result As String = ""
                        For colIndex As Integer = 0 To argvsf.Cols.Count - 1
                            If argvsf(rowIndex, colIndex) IsNot Nothing Then
                                result &= argvsf(rowIndex, colIndex).ToString() & vbTab
                            Else
                                result &= vbTab
                            End If
                        Next
                        result = result.TrimEnd(vbTab)
                        Clipboard.SetText(result)
                    End If
                Case "Full Grid"
                    Dim result As String = ""
                    For rowIndex As Integer = 0 To argvsf.Rows.Count - 1
                        Dim rowResult As String = ""
                        For colIndex As Integer = 0 To argvsf.Cols.Count - 1
                            If argvsf.Cols(colIndex).Visible Then
                                If argvsf(rowIndex, colIndex) IsNot Nothing Then
                                    Dim cellValue As Object = argvsf(rowIndex, colIndex)
                                    rowResult &= argvsf(rowIndex, colIndex).ToString() & vbTab
                                Else
                                    rowResult &= vbTab
                                End If
                            End If
                        Next
                        If rowResult.Length > 0 Then
                            rowResult = rowResult.TrimEnd(vbTab.ToCharArray())
                        End If
                        result &= rowResult & vbCrLf
                    Next
                    result = result.TrimEnd(vbCrLf.ToCharArray())
                    Clipboard.SetText(result)
            End Select
        End With
    End Sub

    Public Sub fillItemRelatedDataFromDocDsgnr(argDocID As Integer, argItemID As Long, argFrm As Object, Optional ByVal argRow As Int32 = -1)

        Dim i As Integer, strResult As String
        Dim strLoadFrom As String
        Dim strFldBlnComboData As String, strFldCmbTblName As String
        Dim strFldCmbStorageFldName As String, strFldCmbDisplayCols As String, strFldCmbAddnlCond As String
        Dim strFirstDispCol As String
        '# A - To load based on the load data settings
        Dim strTblName As String
        strTblName = ""
        Try
            'Satish 08Feb2018
            'strCustSupp = clsSVT.getDataStringValue("Select MstDocIDParty from UTLTransactions where Docid =" & argDocID, "", False)
            'strTblName = Microsoft.VisualBasic.Switch(strCustSupp = "C", "MstCustItem", strCustSupp = "S", "MstSuppItem", strCustSupp = "", "")

            strTblName = clsSVT.getDataStringValue("Select HdrTblName from UTLTransactions where Docid =" &
            clsSVT.getDataStringValue("Select MstDocIDParty from UTLTransactions where Docid =" & argDocID, 0, False), "", False)

            strTblName = strTblName & "Item"

            strLoadFrom = clsSVT.getDataStringValue("Select FldBlnLoadFrom from UtlDocDsgrFlds where FldLoadFromType = 'I' and FldBlnLoadFrom = 'Y' and DocID = " & argDocID, "N", False)

            If strLoadFrom = "N" Then Exit Sub

            With argFrm.VsfOptions
                For i = 1 To .Rows.count - 1
                    'MsgBox(.getdata(i, vOthFldCode))
                    If .getdata(i, vOthFldBlnLoadFrom) = "Y" Then
                        If .getdata(i, vOthFldLoadFromType) = "I" Then
                            If UCase(Mid(.getdata(i, vOthFldLoadFldName), 1, 2)) = "C." Or UCase(Mid(.getdata(i, vOthFldLoadFldName), 1, 2)) = "S." Then
                                strResult = clsSVT.getDataStringValue("Select " & .getdata(i, vOthFldLoadFldName) &
                                   " from MstItem i, MstItemGroup g, MstUOM u" &
                                   IIf(strTblName <> "", "," & strTblName & " ip", "") &
                                   " Where i.ItemGroupID = g.ItemGroupID and " &
                                   " i.UOMID = u.UOMID and " &
                                   " i.ItemID = " & argItemID &
                                   IIf(strTblName <> "", " and i.ItemID = ip.ItemID", ""), "", False)
                            Else
                                strResult = clsSVT.getDataStringValue(
                                    "Select " & .getdata(i, vOthFldLoadFldName) &
                                    " from MstItem i, MstItemGroup g, MstUOM u " &
                                    " Where i.ItemGroupID = g.ItemGroupID and " _
                                    & " i.UOMID = u.UOMID and " _
                                    & " i.ItemID = " & argItemID, "", False)
                            End If
                            If UCase(Mid(Trim(.getdata(i, vOthFldLoadFldName)), 1, 1)) = "I" Then
                                'find if it is addnlfld, and if it is loaded from combo
                                'example i.LedID
                                '88888888888888888888888888888888888888
                                strFldBlnComboData = clsSVT.getDataStringValue("Select FldBlnComboData from UtlDocDsgrFlds " &
                                        "where DocID = 2 and FldCode = '" & UCase(Mid(Trim(.getdata(i, vOthFldLoadFldName)), 3)) & "'", "N", False)
                                If strFldBlnComboData = "Y" Then
                                    strFldCmbTblName = clsSVT.getDataStringValue("Select FldCmbTblName from UtlDocDsgrFlds" &
                                        " where DocID = 2 and FldCode = '" & UCase(Mid(Trim(.getdata(i, vOthFldLoadFldName)), 3)) & "'", "N", False)
                                    strFldCmbStorageFldName = clsSVT.getDataStringValue("Select FldCmbStorageFldName from UtlDocDsgrFlds" &
                                        " where DocID = 2 and FldCode = '" & UCase(Mid(Trim(.getdata(i, vOthFldLoadFldName)), 3)) & "'", "N", False)

                                    strFldCmbDisplayCols = clsSVT.getDataStringValue("Select FldCmbDisplayCols from UtlDocDsgrFlds" &
                                        " where DocID = 2 and FldCode = '" & UCase(Mid(Trim(.getdata(i, vOthFldLoadFldName)), 3)) & "'", "N", False)

                                    strFldCmbAddnlCond = clsSVT.getDataStringValue("Select FldCmbAddnlCond from UtlDocDsgrFlds" &
                                        " where DocID = 2 and FldCode = '" & UCase(Mid(Trim(.getdata(i, vOthFldLoadFldName)), 3)) & "'", "N", False)

                                    If strFldCmbTblName <> "" Then
                                        If InStr(1, strFldCmbDisplayCols, ",") > 0 Then
                                            strFirstDispCol = Microsoft.VisualBasic.Left(strFldCmbDisplayCols, InStr(1, strFldCmbDisplayCols, ",") - 1)
                                        Else
                                            strFirstDispCol = strFldCmbDisplayCols
                                        End If
                                        strResult = clsSVT.getDataStringValue("Select " & strFirstDispCol &
                                            " from " & strFldCmbTblName & " where " & strFldCmbStorageFldName & " = " & Val(strResult), "", False)
                                        If .getdata(i, vOthFldDataType) = "Text" Then
                                            strResult = Mid(strResult, 1, Val(.getdata(i, vOthFldDataWidth)))
                                        End If
                                    End If
                                End If
                            End If
                            If argRow > -1 Then
                                argFrm.vsfDet.setdata(argRow, i, strResult)
                            Else
                                argFrm.vsfDet.setdata(argFrm.vsfDet.Row, i, strResult)
                            End If
                        End If
                    End If
                Next i
            End With
            Exit Sub
        Catch ex As Exception
            MsgBox(ex.Message, vbInformation, argFrm.text)
        End Try
    End Sub
    Public Function validateEmail(emailAddress) As Boolean
        Dim email As New Regex("([\w-+]+(?:\.[\w-+]+)*@(?:[\w-]+\.)+[a-zA-Z]{2,7})")
        If email.IsMatch(emailAddress) Then
            Return True
        Else
            Return False
        End If
    End Function
    Public Sub SaveFormSettings(ByVal argDocID As Integer, ByVal argblnHdr As Boolean, ByVal argblnDtl As Boolean, ByVal argblnFtr As Boolean, ByVal argblnTot As Boolean)
        Dim strSQL As String
        Try
            If clsSVT.getDataLongValue("Select DocID from UtlFormSettings where DocID=" & argDocID & " and UserID=" & gUserID, 0, False) = 0 Then
                strSQL = "Insert into UtlFormSettings(UserID, DocID) values("
                strSQL &= gUserID & ","
                strSQL &= argDocID & ")"
                clsSVT.execSQLStatement(strSQL)
            End If

            strSQL = "Update UtlFormSettings set "
            strSQL &= " Header = '" & IIf(argblnHdr = True, "Y", "N") & "',"
            strSQL &= " Detail = '" & IIf(argblnDtl = True, "Y", "N") & "',"
            strSQL &= " Footer = '" & IIf(argblnFtr = True, "Y", "N") & "',"
            strSQL &= " Total = '" & IIf(argblnTot = True, "Y", "N") & "'"
            strSQL &= " where DocID =" & argDocID
            strSQL &= " and UserID =" & gUserID
            clsSVT.execSQLStatement(strSQL)
        Catch ex As Exception
        End Try
    End Sub

    Public Function LoadFormSettings(ByVal argDocID As Integer, ByVal argActivity As String) As Boolean

        'LoadFormSettings = True
        'Exit Function

        Dim strSQL As String
        LoadFormSettings = False
        strSQL = "Select " & argActivity
        strSQL &= " from UtlFormSettings "
        strSQL &= " where DocID = " & argDocID
        strSQL &= " and UserID = " & gUserID

        Try
            If clsSVT.getDataStringValue(strSQL, "N", False) = "Y" Then
                LoadFormSettings = True
                Exit Function
            Else
                LoadFormSettings = False
                Exit Function
            End If
        Catch ex As Exception
            MsgBox(ex.Message & vbCrLf & "LoadFormSettings", MsgBoxStyle.Information)
        End Try
    End Function

    Public Sub LoadLastOrNewTrn(ByVal argDocID As Integer, ByRef argBlnNewTrn As Boolean, ByRef argBlnLastTrn As Boolean)
        Dim strSQL As String
        Dim rs As OleDb.OleDbDataReader
        strSQL = "Select NewMode,LoadLastTrnNo"
        strSQL &= " from UtlFormSettings "
        strSQL &= " where DocID = " & argDocID
        strSQL &= " and UserID = " & gUserID

        Try
            rs = clsSVT.getDataReaderFromSQL(strSQL)

            'Pavithra
            If Not rs.HasRows Then
                argBlnNewTrn = False
                argBlnLastTrn = False
                rs.Close()
                Exit Sub
            End If
            '----------------------

            If rs.Read Then
                If rs("NewMode") = "Y" Then
                    argBlnNewTrn = True
                Else
                    argBlnNewTrn = False
                End If
                If rs("LoadLastTrnNo") = "Y" Then
                    argBlnLastTrn = True
                Else
                    argBlnLastTrn = False
                End If
            End If
            'Pavithra
            rs.Close()
        Catch ex As Exception
            MsgBox(ex.Message & vbCrLf & "LoadLastOrNewTrn", MsgBoxStyle.Information)
        End Try
    End Sub

    Public Sub RecordLog(ByVal ex As Exception)
        Dim strFile As String = Application.StartupPath & "\ErrorLog.txt"
        Using writer As New StreamWriter(strFile, True)
            If File.Exists(strFile) Then
                writer.WriteLine("♣Error Message : " & ex.ToString & " Occured at--" & DateTime.Now)
            Else
                writer.WriteLine("Start Error Log for today")
            End If
            writer.Close()
        End Using
    End Sub

    Public Function chkItemPresentInDownliner(argParentID As Long, argChildId As Long, Optional ByVal sTrans As OleDbTransaction = Nothing) As Boolean
        chkItemPresentInDownliner = False

        Dim cmd As OleDbCommand = New OleDbCommand("mSp_CheckForChildItems")
        With cmd
            If Not sTrans Is Nothing Then
                cmd.Transaction = sTrans
            End If

            cmd.Connection = svtConn
            .CommandType = CommandType.StoredProcedure

            .Parameters.Add("@ParentItemID ", OleDbType.Integer)
            .Parameters(0).Value = argParentID
            .Parameters.Add("@ChildItemId", OleDbType.Integer)
            .Parameters(1).Value = argChildId
            .Parameters.Add("@mCount", OleDbType.Integer)
            .Parameters("@mCount").Direction = ParameterDirection.Output
            .ExecuteNonQuery()
            If GetVal(.Parameters("@mCount").Value, 0) > 0 Then
                chkItemPresentInDownliner = True
            End If
        End With
    End Function

    Public Function CreateRoundObj(ByVal argObj As Object)
        Dim gp As New GraphicsPath
        gp.AddEllipse(argObj.DisplayRectangle)
        argObj.Region = New Region(gp)
        Return 0
    End Function

    Public Sub vsfBeforeMouseDown(ByVal argFrm As Object, ByVal argVsfDet As C1FlexGrid,
                                             ByVal argE As C1.Win.C1FlexGrid.BeforeMouseDownEventArgs)
        'start dragging when the user clicks the cell
        Dim flex As C1FlexGrid = argVsfDet
        Dim hti As HitTestInfo = flex.HitTest(argE.X, argE.Y)

        If hti.Type = HitTestTypeEnum.Cell Then
            'select the row 
            Dim index As Integer = hti.Row
            flex.Select(index, 1, index, flex.Cols.Count - 1, False)

            'save info for target
            _src = flex

            'do drag drop
            Dim dd As DragDropEffects = flex.DoDragDrop(flex.Clip, DragDropEffects.Move)

            'if it worked, delete row from source (it's a move) 
            If dd = DragDropEffects.Move Then
                flex.Rows.Remove(index)
            End If

            'done, reset info
            _src = Nothing
        End If
    End Sub

    Public Sub vsfDragDrop(ByVal argFrm As Object, ByVal argVsfDet As C1FlexGrid,
                                             ByVal argE As System.Windows.Forms.DragEventArgs)
        'find the drop position
        Dim flex As C1FlexGrid = argVsfDet
        Dim pt As Point = flex.PointToClient(New Point(argE.X, argE.Y))
        Dim hti As HitTestInfo = flex.HitTest(pt.X, pt.Y)

        'newindex - the index where the row is to be moved to
        'oldindex - the index from where the row was moved from
        Dim newindex As Integer = hti.Row
        Dim oldindex As Integer = flex.RowSel

        If newindex < 0 Then newindex = flex.Rows.Count
        If newindex < 1 Then newindex = 1

        Dim row As C1.Win.C1FlexGrid.Row

        If oldindex > 1 And newindex < oldindex Then    'going upwards
            row = flex.Rows(oldindex)
        End If

        If newindex > oldindex And (oldindex + 1) < flex.Rows.Count Then   'coming downwards
            row = flex.Rows(oldindex + 1)
        End If

        ' remove the row from the earlier position
        flex.Rows.Remove(oldindex)

        If newindex > flex.Rows.Count Then           ' if the row to be added is the last row
            'insert a new row at the drop position
            flex.Rows.Insert(newindex - 1)

            'copy data from source row
            flex.Select(newindex - 1, 0, newindex - 1, flex.Cols.Count - 1, False)
            flex.Clip = CType(argE.Data.GetData(GetType(String)), String)
        Else
            'insert a new row at the drop position
            flex.Rows.Insert(newindex)

            'copy data from source row
            flex.Select(newindex, 0, newindex, flex.Cols.Count - 1, False)
            flex.Clip = CType(argE.Data.GetData(GetType(String)), String)

        End If

        flex.Rows.Insert(oldindex)
    End Sub

    Public Sub loadFormColors()
        Dim str As String = getGsettings("COMPANY", "MainFormClrs", "")
        If str = "" Then Exit Sub
        Dim strArr() As String = str.Split(New Char() {","c})

        'Normal
        gMainForeColor = Color.FromArgb(Convert.ToInt32(Trim(strArr(0))))
        gMainBackColor = Color.FromArgb(Convert.ToInt32(Trim(strArr(1))))
        'gMainFontName = CType(converter.ConvertFromString(strArr(2) & "," & strArr(3) & "pt"), Font)
        gMainFontName = CType(converter.ConvertFromString(Trim(strArr(2)) & "," & Trim(strArr(3)) & "pt"), Font)

        str = getGsettings("COMPANY", "FormClrs", "")
        Dim strArr1() As String = str.Split(New Char() {","c})
        'Alternate
        gChildMandatoryFore = Color.FromArgb(Convert.ToInt32(strArr1(0)))
        gChildReadOnlyFore = Color.FromArgb(Convert.ToInt32(strArr1(1)))
        gChildOptionalFore = Color.FromArgb(Convert.ToInt32(strArr1(2)))
        gChildFormBackColor = Color.FromArgb(Convert.ToInt32(strArr1(3)))
        gintFontSize = getGsettings("COMPANY", "FontSize", 8)
    End Sub

    Public Function GetAllLabels(control As Control, type As Type) As IEnumerable(Of Control)
        Dim controls = control.Controls.Cast(Of Control)()

        Return controls.SelectMany(Function(ctrl) GetAllLabels(ctrl, type)).Concat(controls).Where(Function(c) c.[GetType]() = type)
    End Function

    Public Sub fillFormProperties(ByVal argForm As Object, Optional ByVal argvsfHdr As C1FlexGrid = Nothing, Optional ByVal argchangeLblColor As Boolean = True)
        Dim ctrlList = GetAllLabels(argForm, GetType(Label))
        If argchangeLblColor Then
            For Each tB As Label In ctrlList
                If tB.Tag <> "NOCHANGE" Then
                    Select Case tB.Tag
                        Case "M"
                            tB.ForeColor = gChildMandatoryFore
                        Case "R"
                            tB.ForeColor = gChildReadOnlyFore
                        Case Else
                            tB.ForeColor = gChildOptionalFore
                    End Select
                End If
            Next
        End If
        argForm.BackColor = gChildFormBackColor

        Try
            argForm.svtBtnCtrl.FlowLayoutPanel1.BackColor = Color.Transparent
            argForm.svtBtnCtrl.BackColor = Color.Transparent
        Catch ex As Exception

        End Try
        'If Not IsNothing(argvsfHdr) Then
        '    argvsfHdr.BackColor = gChildFormBackColor
        'End If
    End Sub

    'for audit track
    Public Sub RecordAuditLog(ByVal argType As String, ByVal argID As Integer, ByVal argUserAction As UserActions,
                               Optional argHdrTbl As String = "", Optional argHdrSQL As String = "", Optional argDtlSQL As String = "", Optional ByVal sTrans As OleDbTransaction = Nothing)
        Dim strUserAction As String = "", strErrMsg As String, lngRecID As Long
        Select Case argUserAction
            Case 0
                strUserAction = "A"
            Case 1
                strUserAction = "M"
            Case 2
                strUserAction = "D"
        End Select

        If argHdrSQL <> "" Then
            argHdrSQL = argHdrSQL.ToUpper
            argHdrSQL = argHdrSQL.Replace("<<DOCID>>", argID)
            argHdrSQL = argHdrSQL.Replace("<<USERACTION>>", "'" & strUserAction & "'")
            argHdrSQL = argHdrSQL.Replace("<<ACTIONDATETIME>>", "'" & getDateFormatInInsert(getSystemDateTime(sTrans), True) & "'")
            argHdrSQL = argHdrSQL.Replace("<<LOGUSERNAME>>", "'" & gUserCode & "'")
            argHdrSQL = argHdrSQL.Replace("<<LOGSYSNAME>>", "'" & gStrSystemName & "'")
            argHdrSQL = argHdrSQL.Replace("<<LOGSYSUSERNAME>>", "'" & gStrSystemUserName & "'")
            argHdrSQL = argHdrSQL.Replace("<<LOGIPADDRESS>>", "''")
            strErrMsg = clsSVT.execSQLStmtReturnMsg(argHdrSQL, sTrans)
            If strErrMsg <> "" Then
                MsgBox("Error during saving of Log File" & vbCrLf & strErrMsg, MsgBoxStyle.Critical, "Error")
            End If
        End If

        If argDtlSQL <> "" Then
            lngRecID = clsSVT.getDataLongValue("Select Max(recID) from " & gStrLogDBName & ".." & argHdrTbl & " where LogUserName = '" & gUserCode & "'", 0, False, sTrans)
            If lngRecID > 0 Then
                argDtlSQL = argDtlSQL.Replace("<<DOCID>>", argID)
                argDtlSQL = argDtlSQL.Replace("<<RECID>>", lngRecID)
                strErrMsg = clsSVT.execSQLStmtReturnMsg(argDtlSQL, sTrans)
                If strErrMsg <> "" Then
                    MsgBox("Error during saving of Detail Log File" & vbCrLf & strErrMsg, MsgBoxStyle.Critical, "Error")
                End If
            End If
        End If

    End Sub
    Public Function sendGenMail(argToMailIDs As String, argCCMailIDs As String, ByVal strSubject As String, strBody As String, Optional argAttFileName As String = "", Optional argShowErrMsg As Boolean = True) As Boolean
        sendGenMail = False
        Try

            Dim mail As New MailMessage()
            Dim SmtpServer As New SmtpClient(gSmtpHostName)
            mail.From = New MailAddress(gSmtpMailID)

            mail.[To].Add(argToMailIDs)
            If Trim(argCCMailIDs) <> "" Then
                mail.[CC].Add(argCCMailIDs)
            End If

            mail.Subject = strSubject
            mail.IsBodyHtml = False
            mail.Body = strBody
            If argAttFileName <> "" Then
                Dim attachment As System.Net.Mail.Attachment
                attachment = New System.Net.Mail.Attachment(argAttFileName)
                mail.Attachments.Add(attachment)
            End If

            SmtpServer.Port = gSmtpServerPort
            SmtpServer.Credentials = New System.Net.NetworkCredential(gSmtpMailID, gSmtpMailPwd)
            'If gSmtpEnableSSL = "Y" Then
            'SmtpServer.EnableSsl = True
            'Else
            SmtpServer.EnableSsl = True
            'End If

            SmtpServer.Send(mail)
            sendGenMail = True

        Catch ex As Exception
            If argShowErrMsg Then MsgBox("Error in Sending SMTP Mail!" & vbCrLf & ex.Message, MsgBoxStyle.Critical, "ERROR")
            sendGenMail = False
        End Try
    End Function

    Public Sub initVsfForHdrs(ByVal argVSF As C1FlexGrid, ByVal argHdrLine1 As String, ByVal argHdrLine2 As String)
        Dim i As Integer

        With argVSF
            For i = 0 To .Cols.Count - 1
                .SetData(3, i, .GetData(0, i))

                .SetData(0, i, gSettings.gCompName & ",  " & gSettings.gCompAdd1 & gSettings.gCompAdd2 & gSettings.gCompAdd3 &
                          gSettings.gCompAdd4 & gSettings.gCompAdd5)
                .SetData(1, i, argHdrLine1)
                .SetData(2, i, argHdrLine2)
            Next
            Dim csf1 As C1.Win.C1FlexGrid.CellStyle = argVSF.Styles.Add("F1")
            csf1.BackColor = Color.Beige

            .Rows(0).Style = csf1
            .Rows(1).Style = csf1
            .Rows(2).Style = csf1

            If argHdrLine1 = "" Then
                .Rows(1).Visible = False
            Else
                .Rows(1).Visible = True
            End If

            If argHdrLine2 = "" Then
                .Rows(2).Visible = False
            Else
                .Rows(2).Visible = True
            End If

            .Rows(0).AllowMerging = True
            .Rows(1).AllowMerging = True
            .Rows(2).AllowMerging = True
            .Rows(3).AllowMerging = True

        End With
    End Sub

    'Public Function chkCustItemQOH(ByVal argCust As Int32, ByVal argItem As Int32, Optional ByVal argBlnAdd As Boolean = False,
    '                               Optional ByVal sTrans As oledbTransaction = Nothing)
    '    chkCustItemQOH = False

    '    Dim strSQL As String, blnFound As Boolean

    '    If Not (argCust > 0 AndAlso argItem > 0) Then
    '        MsgBox("No Customer & Item", MsgBoxStyle.Critical, "QOH Insert Error...")
    '        Return False
    '    End If

    '    If Not sTrans Is Nothing Then
    '        Call clsSVT.getDataLongValue("Select CustID from TrnQOH where custID = " & argCust & " and ItemID = " & argItem, 0, blnFound, sTrans)
    '        If Not blnFound Then
    '            If argBlnAdd Then
    '                strSQL = "Insert into TrnQOH (ItemID, CustID, BQOH, DQOH, PQOH) values ("
    '                strSQL = strSQL & argItem & ","
    '                strSQL = strSQL & argCust & ","
    '                strSQL = strSQL & "0,0,0)"
    '                Call clsSVT.execSQLStatement(strSQL, sTrans)
    '                Return True
    '            End If
    '        Else
    '            chkCustItemQOH = True
    '        End If
    '    Else
    '        Call clsSVT.getDataLongValue("Select CustID from TrnQOH where custID = " & argCust & " and ItemID = " & argItem, 0, blnFound)
    '        If Not blnFound Then
    '            If argBlnAdd Then
    '                strSQL = "Insert into TrnQOH (ItemID, CustID, BQOH, DQOH, PQOH) values ("
    '                strSQL = strSQL & argItem & ","
    '                strSQL = strSQL & argCust & ","
    '                strSQL = strSQL & "0,0,0)"
    '                Call clsSVT.execSQLStatement(strSQL)
    '                Return True
    '            End If
    '        Else
    '            chkCustItemQOH = True
    '        End If
    '    End If
    'End Function

    Public Function AddItemCustQOH(ByVal argCust As Int32, ByVal argItem As String, Optional ByVal sTrans As OleDbTransaction = Nothing) As Boolean
        AddItemCustQOH = False

        Dim strSQL As String

        If Not (argCust > 0 AndAlso argItem <> "") Then
            MsgBox("No Customer & Item", MsgBoxStyle.Critical, "QOH Insert Error...")
            Return False
        End If

        strSQL = "Insert into TrnQOH (ItemID, CustID, BQOH, DQOH, PQOH) "
        strSQL = strSQL & " Select i.ItemID, " & argCust & ", 0, 0, 0 "
        strSQL = strSQL & " from MstItem i "
        strSQL = strSQL & " Left Join TrnQOH q on i.ItemID=q.ItemID and CustID=" & argCust
        strSQL = strSQL & " Where i.ItemID in (" & argItem & ")"
        strSQL = strSQL & " and q.ItemID is NUll "

        If Not sTrans Is Nothing Then
            If Not clsSVT.execSQLStatement(strSQL, sTrans) Then
                Exit Function
            End If
        Else
            If Not clsSVT.execSQLStatement(strSQL) Then
                Exit Function
            End If
        End If

        Return True
    End Function
    Public Sub sendOutlookmail(ByVal argFileName As String)
        Try
            Dim oApp As Outlook._Application
            oApp = New Interop.Outlook.Application

            Dim oMsg As Interop.Outlook._MailItem
            oMsg = oApp.CreateItem(Interop.Outlook.OlItemType.olMailItem)

            'oMsg.Subject = ""
            'oMsg.Body = ""

            'oMsg.To = ""
            'oMsg.CC = ""

            'Dim strS As String = sFilename
            'Dim strN As String = sDisplayname

            If argFileName <> "" Then
                'Dim sBodyLen As Integer = Int(sBody.Length)
                Dim oAttachs As Interop.Outlook.Attachments = oMsg.Attachments
                Dim oAttach As Interop.Outlook.Attachment
                oAttach = oAttachs.Add(argFileName, , , )
            End If

            oMsg.Display()

        Catch ex As Exception
            MsgBox("Error in Sending Mail!" & vbCrLf & ex.Message, MsgBoxStyle.Critical)
        End Try

    End Sub

    Public Sub GetMiscFlds()
        Dim rsDummy As OleDb.OleDbDataReader

        rsDummy = clsSVT.getDataReaderFromSQL("Select * from UtlMisc")

        'Pavithra
        If Not rsDummy.HasRows Then
            rsDummy.Close()
            Exit Sub
        End If
        '----------------------
        With rsDummy
            If .HasRows Then
                .Read()
                gItemCode = rsDummy.Item("ItemCaption")
                gItemName = rsDummy.Item("ItemNameCaption")
                gItemDesc = rsDummy.Item("ItemDescCaption")
                gIntTopNrows = rsDummy.Item("FldSearchTopN")
                gdblSalesValLmt = rsDummy.Item("InvSMSSalesvalue")
                gstrSalesSmsMsg = rsDummy.Item("InvSMSSalesMSg")
                gstrCancelledSMSMsg = rsDummy.Item("InvSMSCancelMsg")
                gstrDeleteSMSMsg = rsDummy.Item("InvSMSDeleteMsg")

                gBlnSMSInvCancelled = IIf(rsDummy.Item("InvSMSblnCancel") = "Y", True, False)
                gBlnSMSInvDeleted = IIf(rsDummy.Item("InvSMSblnDelete") = "Y", True, False)
                gBlnSMSInvSales = IIf(rsDummy.Item("InvSMSblnSales") = "Y", True, False)
                blnSMSFeature = IIf(rsDummy.Item("SMSFeature") = "Y", True, False)
                gStrSalesMobNo = rsDummy.Item("InvMsgMobNo")

                'gCustQryCond = GetVal(rsDummy.Item("SuppQryCond"), "")
                'gSuppQryCond = GetVal(rsDummy.Item("SuppQryCond"), "")
                'gCustQryFlds = GetVal(rsDummy.Item("CustQryFlds"), "")
                'gSuppQryFlds = GetVal(rsDummy.Item("SuppQryFlds"), "")

                gLoginReq = "Y" 'GetVal(!LoginReq, "Y")

                'If GetVal(!BlnAuditLog, "N") = "Y" Then
                '    gBlnAuditLog = True
                'Else
                '    gBlnAuditLog = False
                'End If

                If GetVal(!ADAuth, "N") = "Y" Then
                    gblnADAuth = True
                Else
                    gblnADAuth = False
                End If

                If GetVal(!ADAuth, "N") = "Y" Then
                    gblnADAuth = True
                Else
                    gblnADAuth = False
                End If
                If GetVal(!ADAutoLogin, "N") = "Y" Then
                    gblnADAutolog = True
                Else
                    gblnADAutolog = False
                End If
                If GetVal(!PwdPolicy, "N") = "Y" Then
                    gblnPwdPolicy = True
                Else
                    gblnPwdPolicy = False
                End If
                If GetVal(!PwdRules, "N") = "Y" Then
                    gblnPwdRule = True
                Else
                    gblnPwdRule = False
                End If
                If GetVal(!UserBased, "N") = "Y" Then
                    gblnUserBased = True
                Else
                    gblnUserBased = False
                End If
                If GetVal(!ShowRememberMe, "N") = "Y" Then
                    gblnShowRememberMe = True
                Else
                    gblnShowRememberMe = False
                End If
                If GetVal(!NeverExpires, "N") = "Y" Then
                    gblnPwdNeveExp = True
                Else
                    gblnPwdNeveExp = False
                End If
            End If
            .Close()
        End With
    End Sub

    Public Sub fillButtonColor(ParamArray argBtns() As Control)
        Dim Btns As Object
        For Each Btns In argBtns
            Btns.ForeColor = gBtnForeColor
            Btns.BackColor = gBtnBackColor
            Select Case UCase(gBtnFlatStyle)
                Case "FLAT"
                    Btns.FlatStyle = FlatStyle.Flat
                Case "STANDARD"
                    Btns.FlatStyle = FlatStyle.Standard
                Case "POPUP"
                    Btns.FlatStyle = FlatStyle.Popup
            End Select
            Btns.Font = New System.Drawing.Font("Bookman Old Style", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))

        Next
    End Sub

    Public Function SendSMS(ByVal argMsg As String, ByVal argMobile As String, argProvider As String, Optional svtTrans As OleDb.OleDbTransaction = Nothing) As Boolean
        SendSMS = False
        If Not blnSMSFeature Then Exit Function
        Dim strURL As String = "", intLimit As Integer, kount As Integer, intProviderID As Integer
        'argMsg = "ನೀವು ಹೇಗಿದ್ದೀರಿ. ನಿನ್ನ ಮನೆಯವರು ಹೇಗಿದ್ದಾರೆ"
        Dim rs As OleDb.OleDbDataReader, strSQL As String
        If argProvider <> "" Then
            strSQL = "Select * from UtlMsgSettings where Provider = '" & argProvider & "'"
            rs = clsSVT.getDataReaderFromSQL(strSQL, svtTrans)
            If rs.HasRows Then
                rs.Read()
                strURL = rs!SMSURL
                intProviderID = rs!RowID
                intLimit = rs!DailyLimit
            End If
            rs.Close()
        Else
            intProviderID = gDefProviderID
            argProvider = gDefProvider
            strURL = gSMSURL
            intLimit = gIntDailyLimit
        End If
        If intProviderID = 0 Then Exit Function

        strURL = strURL.Replace("<<MOBNO>>", argMobile)
        strURL = strURL.Replace("<<MSG>>", argMsg)
        kount = clsSVT.getDataLongValue("Select count(rowID) from TrnSMS where ProviderID = " & intProviderID & " and Trndate = " & getDateFormatInCond(getSystemDate), 0, False, svtTrans)
        If kount > intLimit Then
            MsgBox("Daily SMS limit (" & intLimit & ") has exceeded for Provider " & argProvider & ". Cannot Send SMS.", MsgBoxStyle.Critical, "SMS")
            SendSMS = False
            Exit Function
        End If
        Try
            Dim myReq As HttpWebRequest = DirectCast(WebRequest.Create(strURL), HttpWebRequest)
            Dim myResp As HttpWebResponse = DirectCast(myReq.GetResponse(), HttpWebResponse)
            Dim respStreamReader As New System.IO.StreamReader(myResp.GetResponseStream())
            Dim responseString As String = respStreamReader.ReadToEnd()
            respStreamReader.Close()

            If Not responseString.ToUpper().Contains("DONE") Then
                SendSMS = False
                myResp.Close()
                Exit Function
            End If

            strSQL = "Insert into TrnSMS (TrnDate,ProviderID,MobNo,MsgText,TrnDateTime)"
            strSQL &= " values ('" & getDateFormatInInsert(getSystemDate) & "',"
            strSQL &= intProviderID & ","
            strSQL &= "'" & argMobile & "',"
            strSQL &= "'" & argMsg & "',"
            strSQL &= " '" & getDateFormatInInsert(getSystemDateTime, True) & "')"
            clsSVT.execSQLStatement(strSQL, svtTrans)

            SendSMS = True
            myResp.Close()

        Catch ex As Exception
            'Throw ex
        Finally
        End Try
    End Function

    Public Function getSMSSender() As String
        getSMSSender = ""
        Dim strSender As String = ""
        strSender = clsSVT.getDataStringValue("Select SetValue from UtlGSettings where SetName= 'Sender' and SetGroup = 'COMPANY'", "", False)
        If strSender <> "" Then
            Dim strArr() As String = strSender.Split("|")
            For i = 0 To UBound(strArr)
                getSMSSender = strArr(i)
                Exit Function
            Next
        End If
    End Function

    Public Function SendWhatsApp(ByVal argMsg As String, ByVal argMobile As String, Optional ByVal argFilePath As String = "", Optional ByVal argEmoji As String = "") As Boolean
        SendWhatsApp = False

        If gStrWatsAppMsg = "" Then Exit Function

        If gStrWatsAppFile = "" Then Exit Function

        If argFilePath <> "" AndAlso gStrWatsAppFile = "" Then Exit Function

        Dim strURL As String = ""

        If argFilePath <> "" Then
            strURL = gStrWatsAppFile
        Else
            strURL = gStrWatsAppMsg
        End If

        strURL = strURL.Replace("<<MOB>>", "91" & argMobile)
        strURL = strURL.Replace("<<MSG>>", argMsg & argEmoji)
        If argFilePath <> "" Then strURL = strURL.Replace("<<URL>>", argFilePath)

        Try

            Dim myReq As HttpWebRequest = DirectCast(WebRequest.Create(strURL), HttpWebRequest)
            Dim myResp As HttpWebResponse = DirectCast(myReq.GetResponse(), HttpWebResponse)
            Dim respStreamReader As New System.IO.StreamReader(myResp.GetResponseStream())
            Dim responseString As String = respStreamReader.ReadToEnd()
            respStreamReader.Close()

            If responseString.ToUpper() <> "SUCCESS" Then
                SendWhatsApp = False
                Exit Function
            End If


            SendWhatsApp = True
            myResp.Close()
        Catch ex As Exception
            'Throw ex
        Finally
        End Try
    End Function

    Public Function GenerateOTP(Optional ByVal argblnAlphaNumeric As Boolean = False, Optional ByVal argOTPLen As Integer = 4) As String
        Dim alphabets As String = "ABCDEFGHIJKLMNOPQRSTUVWXYZ"
        Dim small_alphabets As String = "abcdefghijklmnopqrstuvwxyz"
        Dim numbers As String = "1234567890"

        Dim characters As String = numbers
        If argblnAlphaNumeric Then
            characters += Convert.ToString(alphabets & small_alphabets) & numbers
        End If
        Dim length As Integer = Integer.Parse(argOTPLen)
        Dim otp As String = String.Empty
        For i As Integer = 0 To length - 1
            Dim character As String = String.Empty
            Do
                Dim index As Integer = New Random().Next(0, characters.Length)
                character = characters.ToCharArray()(index).ToString()
            Loop While otp.IndexOf(character) <> -1
            otp += character
        Next
        GenerateOTP = otp
    End Function

    Public Sub AddMsgCount(ByVal argBln As Boolean, ByVal argCount As Int32)
        'Dim blnFound As Boolean
        ''Dim strSQL As String = "Select Sent from TrnSMSSummary Where TrnDate=#" & Format(Date.Today, "MM/dd/yyyy") & "#"
        'clsSVT.getDataLongValue(strSQL, 0, blnFound)
        'If Not blnFound Then
        '    strSQL = "Insert into TrnSMSSummary (TrnDate,Sent,UnSent,LastTrn,TotCreated)"
        '    strSQL &= " values ('" & getDateFormatInInsert(Date.Today) & "',"
        '    strSQL &= " 0,0,'',0)"
        '    clsSVT.execSQLStatement(strSQL)

        'End If

        'If argBln Then
        '    strSQL = "Update TrnSMSSummary set Sent = Sent + " & argCount & ", LastTrn='" & CStr(Format(Date.Today, "yyyy-MM-dd")) & "' Where TrnDate=#" & Format(Date.Today, "MM/dd/yyyy") & "#"
        'Else
        '    strSQL = "Update TrnSMSSummary set UnSent = UnSent + " & argCount & " Where TrnDate=#" & Format(Date.Today, "MM/dd/yyyy") & "#"
        'End If
        'clsSVT.execSQLStatement(strSQL)
    End Sub

    'Public Sub AddMsg(ByVal argFrom As String,
    '                  ByVal argTo As String,
    '                  ByVal argMsg As String,
    '                  ByVal argStatus As String)
    '    Dim strSQL As String = "Insert into TrnSMS (UserID,TrnDate,MsgFrom,MsgTo,Msg,MsgStatus) values "

    '    strSQL &= "(" & gUserID & ","
    '    strSQL &= "'" & CStr(Format(Date.Today, "yyyy-MM-dd HH:mm:ss")) & "',"
    '    strSQL &= "'" & argFrom & "',"
    '    strSQL &= "'" & argTo & "',"
    '    strSQL &= "'" & argMsg & "',"
    '    strSQL &= "'" & argStatus & "')"
    '    clsSVT.execSQLStatement(strSQL)
    'End Sub

    Public Property TransferPlainText() As String
        Get
            Return strGetPlainText
        End Get
        Set(ByVal strPlainText As String)
            strGetPlainText = strPlainText
        End Set
    End Property

    Public Function sendOutLookMail(argToMailIDs As String, argCCMailIDs As String, ByVal strSubject As String, strBody As String, Optional argAttFileName As String = "", Optional argShowErrMsg As Boolean = True) As Boolean
        sendOutLookMail = False

        Try

            Dim oApp As Interop.Outlook._Application
            oApp = New Interop.Outlook.Application
            Dim oMsg As Interop.Outlook._MailItem
            oMsg = oApp.CreateItem(Interop.Outlook.OlItemType.olMailItem)
            oMsg.Subject = strSubject
            oMsg.Body = strBody
            oMsg.To = argToMailIDs
            If argCCMailIDs <> "" Then oMsg.CC = argCCMailIDs


            Dim strS As String = argAttFileName
            Dim strN As String = ""
            If argAttFileName <> "" Then
                Dim sBodyLen As Integer = Int(strBody.Length)
                Dim oAttachs As Interop.Outlook.Attachments = oMsg.Attachments
                Dim oAttach As Interop.Outlook.Attachment

                oAttach = oAttachs.Add(strS, , sBodyLen, strN)

            End If

            oMsg.Send()


            sendOutLookMail = True

        Catch ex As Exception
            If argShowErrMsg Then MsgBox("Error in Sending Outlook Mail!" & vbCrLf & ex.Message, MsgBoxStyle.Critical, "ERROR")
            sendOutLookMail = False
        End Try
    End Function

    Public Sub OpenMasters(argMenu As String, argID As Integer)
        Dim intDocID As Integer ', strDocType As String
        Dim strCustSuppReq As String = "", strNonItemDoc As String = ""
        Dim strMnuName As String
        strMnuName = argMenu
        '''''search for duplicate form
        For Each frm As Form In frmMain.MdiChildren  ' Scan all childform
            If frm.Text = argMenu Then 'if Same Name then exit sub function
                Exit Sub
            End If
        Next

        ''for fixed forms
        intDocID = clsSVT.getDataLongValue("select DOCID from MStMenu where MenuItemCaption = '" & strMnuName & "'", 0, False)
        Select Case strMnuName
            Case "State City Country"
                Dim argfrm As New frmMstSCC
                argfrm.initForm(intDocID, "MstSCC", "SCCID", "City", "State City Country", "State City Country")
                argfrm.FillDocDetails(argID)
                frmMain.OpenForms(argfrm)

            Case "Currency Master"
                Dim argfrm As New frmMstCurr
                argfrm.initForm(intDocID, "MstCur", "CURID", "CURCODE", "CURNAME", "Currency Master", "CURCODE")
                argfrm.FillDocDetails(argID)
                argfrm.txtSearch.Focus()
                frmMain.OpenForms(argfrm)

            Case "User Master"
                Dim argfrm As New frmMstUSer
                argfrm.initForm(intDocID, "MstUser", "UserID", "UserCODE", "UserNAME", "USer Master", "USERCODE")
                argfrm.FillDocDetails(argID)
                frmMain.OpenForms(argfrm)

            Case Else
                Call OpenGeneralMasterForm(argMenu, argID)
        End Select
    End Sub

    Public Sub OpenGeneralMasterForm(argMnuName As String, argID As Integer)
        'for fixed forms
        Dim intDocID As Integer, strDocType As String = "", strSQL As String
        Dim strCustSuppReq As String = "", strNonItemDoc As String = ""
        Dim argTBLName As String = "", argIDName As String = "", argCodeName As String = "", argFormCaption As String = "", argFLDCaption As String = ""
        Dim rs As OleDb.OleDbDataReader

        intDocID = clsSVT.getDataLongValue("select DOCID from MStMenu where upper(MenuItemCaption) = '" & UCase(argMnuName) & "'", 0, False)
        If intDocID = 0 Then Exit Sub

        strSQL = "Select * from UtlTransactions where DocID = " & intDocID

        rs = clsSVT.getDataReaderFromSQL(strSQL)

        'Pavithra
        If Not rs.HasRows Then
            rs.Close()
            Exit Sub
        End If
        '----------------------

        With rs
            Do While .Read
                argTBLName = !HdrTblName
                argIDName = Replace(!HdrTblName, "Mst", "") & "ID"
                argCodeName = Replace(!HdrTblName, "Mst", "") & "Code"
                argFormCaption = !DocName
                argFLDCaption = !DocName
                strDocType = GetVal(!TrnDocType, "")
            Loop
            'Pavithra
            .Close()
        End With

        If strDocType = "General" Then
            Dim frmMstGeneral As New frmMstGeneral
            frmMstGeneral.initForm(intDocID, argTBLName, argIDName, argCodeName, argFormCaption, argFLDCaption, False)
            frmMstGeneral.FillDocDetails(argID)
            frmMstGeneral.txtSearch.Focus()
            frmMain.OpenForms(frmMstGeneral, argFormCaption, True)

        ElseIf strDocType = "Party Variant" Then
            Dim frmParty As New frmMstParty
            frmParty.initForm(intDocID)
            frmParty.FillDocDetails(argID)
            frmParty.txtSearch.Focus()
            frmMain.OpenForms(frmParty, argFormCaption, True)

        ElseIf strDocType = "Item Variant" Then
            Dim argfrm As New frmMstItem
            argfrm.initForm(intDocID)
            argfrm.FillDocDetails(argID)
            argfrm.txtSearch.Focus()
            frmMain.OpenForms(argfrm, argFormCaption, True)

        End If
    End Sub

    Public Sub getMasterFlds(argTblName As String, argFldText As String)
        Dim rs As OleDb.OleDbDataReader
        Dim strMstDocName As String = "", strMstCodeName As String, strMstIDName As String, strMstFrmPfx As String
        Dim intMstID As Integer
        If argFldText = Nothing Then argFldText = ""
        rs = clsSVT.getDataReaderFromSQL(" Select * from UtlTransactions where HdrTblName = '" & argTblName & "'")

        'Pavithra
        If Not rs.HasRows Then
            rs.Close()
            Exit Sub
        End If
        '----------------------

        With rs
            .Read()
            strMstDocName = !DocName
            strMstCodeName = Trim(Replace(!HdrTblName, "Mst", "")) & "Code"
            strMstIDName = Trim(Replace(!HdrTblName, "Mst", "")) & "ID"
            strMstFrmPfx = Trim(Replace(!HdrTblName, "Mst", ""))
            intMstID = clsSVT.getDataLongValue("Select " & strMstIDName & " from " & argTblName & " where " & strMstCodeName & " = '" & Trim(argFldText) & "'", 0, False)
            Call OpenMasters(strMstDocName, intMstID)
            .Close()
        End With
    End Sub

    Public Function getCtrlList(control As Control, type As Type) As IEnumerable(Of Control)
        Dim controls = control.Controls.Cast(Of Control)()
        Return controls.SelectMany(Function(ctrl) getCtrlList(ctrl, type)).Concat(controls).Where(Function(c) c.[GetType]() = type And CStr(c.Tag) <> "")
    End Function

    Public Function chkCtrlTag(ByVal argObj As Object, ByVal argCode As String, ByVal argRights As String, ByVal argPos As Int32, ByVal argHdr As String) As Boolean
        chkCtrlTag = False
        For Each ctrl As Control In argObj
            If ctrl.Tag.ToString() = argCode Then
                If Not chkUserRights(argRights, argPos, argHdr, False) Then
                    ctrl.Enabled = False
                    Return True
                End If
            End If
        Next
    End Function

    Public Sub chkSplRights_Std(ByRef argFrm As Form, ByVal argDocID As Int32, ByVal argRights As String)
        'For Special rights we required control list
        Dim txtCtrlList = getCtrlList(argFrm, GetType(TextBox))
        Dim cboCtrlList = getCtrlList(argFrm, GetType(ComboBox))
        Dim chkCtrlList = getCtrlList(argFrm, GetType(CheckBox))
        Dim rdoCtrlList = getCtrlList(argFrm, GetType(RadioButton))
        Dim dtpCtrlList = getCtrlList(argFrm, GetType(DateTimePicker))


        Dim rs As OleDb.OleDbDataReader
        rs = clsSVT.getDataReaderFromSQL("Select * from UtlMstSpecialRights Where std='S' and DocID=" & argDocID)
        If Not rs.HasRows Then
            rs.Close()
            Exit Sub
        End If

        Do While rs.Read()
            Dim intPosition As Int32 = 0
            Select Case rs.Item("RightsNumber")
                Case 1
                    intPosition = intOpt1
                Case 2
                    intPosition = intOpt2
                Case 3
                    intPosition = intOpt3
                Case 4
                    intPosition = intOpt4
                Case 5
                    intPosition = intOpt5
            End Select

            If chkCtrlTag(txtCtrlList, rs.Item("FldCode"), argRights, intPosition, argFrm.Text) Then
                GoTo NextRecord
            End If
            If chkCtrlTag(cboCtrlList, rs.Item("FldCode"), argRights, intPosition, argFrm.Text) Then
                GoTo NextRecord
            End If
            If chkCtrlTag(rdoCtrlList, rs.Item("FldCode"), argRights, intPosition, argFrm.Text) Then
                GoTo NextRecord
            End If
            If chkCtrlTag(chkCtrlList, rs.Item("FldCode"), argRights, intPosition, argFrm.Text) Then
                GoTo NextRecord
            End If
            If chkCtrlTag(dtpCtrlList, rs.Item("FldCode"), argRights, intPosition, argFrm.Text) Then
                GoTo NextRecord
            End If
NextRecord:
        Loop
        rs.Close()
    End Sub
    Public Sub StatusMsg(ByVal argFldCode As String, ByVal argDocId As Integer)
        Dim strMsg As String
        strMsg = ""
        strMsg = clsSVT.getDataStringValue("select FldToolTip from UtlDocDsgrflds where DocID=" & argDocId & " and FldCode='" & argFldCode & "'", "", False)

        frmMain.tslblStatus.Text = strMsg
    End Sub

    Public Sub chkSplRights_DOC(ByRef argFrm As Form, ByVal argDocID As Int32, ByVal argRights As String, ByRef argVSF As C1FlexGrid)
        'For Special rights we required control list

        Dim rs As OleDb.OleDbDataReader
        rs = clsSVT.getDataReaderFromSQL("Select * from UtlMstSpecialRights Where std='D' and DocID=" & argDocID)
        If Not rs.HasRows Then
            rs.Close()
            Exit Sub
        End If

        Do While rs.Read()
            Dim intPosition As Int32 = 0
            Select Case rs.Item("RightsNumber")
                Case 1
                    intPosition = intOpt1
                Case 2
                    intPosition = intOpt2
                Case 3
                    intPosition = intOpt3
                Case 4
                    intPosition = intOpt4
                Case 5
                    intPosition = intOpt5
            End Select

            With argVSF
                For i As Int32 = 0 To .Rows.Count - 1
                    If rs.Item("FldCode") = GetVal(.GetData(i, vOthFldCode)) Then
                        If Not chkUserRights(argRights, intPosition, argFrm.Text, False) Then
                            .SetData(i, vOthFldBlnReadOnly, "Y")
                            GoTo NextRecord
                        End If
                    End If
                Next
            End With
NextRecord:
        Loop
        rs.Close()
    End Sub

    Public Function allowNumDecimals(argtxtbox As TextBox, argNumofDecimals As Integer) As Boolean
        allowNumDecimals = False
        If InStr(argtxtbox.Text, ".") > 0 Then
            Dim TwoDec As New System.Text.RegularExpressions.Regex("^(\d)*\.\d{" & argNumofDecimals & "}$")
            If TwoDec.IsMatch(argtxtbox.Text) Then
            Else
                argtxtbox.Text = Mid(argtxtbox.Text, 1, InStr(argtxtbox.Text, ".")) & Mid(argtxtbox.Text, InStr(argtxtbox.Text, ".") + 1, argNumofDecimals)
            End If
        End If
    End Function

    Public Function GetAllCtrlList(control As Control, type As Type) As IEnumerable(Of Control)
        Dim controls = control.Controls.Cast(Of Control)()
        Return controls.SelectMany(Function(ctrl) GetAllCtrlList(ctrl, type)).Concat(controls).Where(Function(c) c.[GetType]() = type)
    End Function

    Public Sub ReSizeCtrl(ByVal argTLP As Object, ByVal argBln As Boolean, ParamArray ByVal argCtrlList() As Object)
        With argTLP
            For Each ctrlList As Object In argCtrlList
                For Each ctrl As Control In ctrlList
                    If argBln Then
                        Dim size As Double = ctrl.Font.Size + 1
                        Dim Bold3 As Font = New Font(ctrl.Font.FontFamily, size)
                        ctrl.Font = Bold3
                    Else
                        If ctrl.Font.Size <= 9 Then Exit Sub
                        Dim size As Double = ctrl.Font.Size - 1
                        Dim Bold3 As Font = New Font(ctrl.Font.FontFamily, size)
                        ctrl.Font = Bold3
                    End If
                Next
            Next
        End With
    End Sub

    Public Function GetImageByte(ByVal imagePath As String) As Byte()
        Dim stream As FileStream = New FileStream(imagePath, FileMode.Open, FileAccess.Read)
        Dim reader As BinaryReader = New BinaryReader(stream)
        Dim img() As Byte = reader.ReadBytes(stream.Length)

        reader.Close()
        stream.Close()

        Return img
    End Function

    Public Function GetAllCtrls(control As Control, type As Type) As IEnumerable(Of Control)
        Dim controls = control.Controls.Cast(Of Control)()
        Return controls.SelectMany(Function(ctrl) GetAllCtrls(ctrl, type)).Concat(controls).Where(Function(c) c.[GetType]() = type)
    End Function

    Public Sub FormatCtrl(ByVal argObj As Object, ByVal argVsfName As String)
        Select Case argVsfName
            Case "B"
                For Each ctrl As C1.Win.C1Input.C1Button In argObj
                    ctrl.Font = New Font(ctrl.Font.FontFamily, gintFontSize, ctrl.Font.Style)
                Next
            Case "S"
                For Each ctrl As C1.Win.C1Input.C1SplitButton In argObj
                    ctrl.Font = New Font(ctrl.Font.FontFamily, gintFontSize, ctrl.Font.Style)
                Next
            Case "C"
                For Each ctrl As ComboBox In argObj
                    Dim str As String = GetVal(ctrl.Text, "")
                    ctrl.SelectedIndex = -1
                    ctrl.Font = New Font(ctrl.Font.FontFamily, gintFontSize, ctrl.Font.Style)
                    ctrl.Text = str
                Next
            Case Else
                For Each ctrl As Control In argObj
                    ctrl.Font = New Font(ctrl.Font.FontFamily, gintFontSize, ctrl.Font.Style)
                Next
        End Select
    End Sub



    Public Sub ChangeFontSize(ByVal argFrm As Form)
        If gintFontSize = 0 Then
            gintFontSize = 8
        End If
        Dim btnCtrlList = GetAllCtrls(argFrm, GetType(Button))
        Dim lblCtrlList = GetAllCtrls(argFrm, GetType(Label))
        Dim txtCtrlList = GetAllCtrls(argFrm, GetType(TextBox))
        Dim PBCtrlList = GetAllCtrls(argFrm, GetType(PictureBox))
        Dim vsfCtrlList = GetAllCtrls(argFrm, GetType(C1FlexGrid))
        Dim chkCtrlList = GetAllCtrls(argFrm, GetType(CheckBox))
        Dim cboCtrlList = GetAllCtrls(argFrm, GetType(ComboBox))
        Dim PnlCtrlList = GetAllCtrls(argFrm, GetType(Panel))
        Dim gbCtrlList = GetAllCtrls(argFrm, GetType(GroupBox))
        Dim lbCtrlList = GetAllCtrls(argFrm, GetType(ListBox))
        Dim flpCtrlList = GetAllCtrls(argFrm, GetType(FlowLayoutPanel))
        Dim rdoCtrlList = GetAllCtrls(argFrm, GetType(RadioButton))
        Dim clbCtrlList = GetAllCtrls(argFrm, GetType(CheckedListBox))
        Dim dtpCtrlList = GetAllCtrls(argFrm, GetType(DateTimePicker))
        Dim vBtnCtrlList = GetAllCtrls(argFrm, GetType(C1.Win.C1Input.C1Button))
        Dim vSBtnCtrlList = GetAllCtrls(argFrm, GetType(C1.Win.C1Input.C1SplitButton))

        Call FormatCtrl(btnCtrlList, "")
        Call FormatCtrl(lblCtrlList, "")
        Call FormatCtrl(txtCtrlList, "")
        Call FormatCtrl(chkCtrlList, "")
        Call FormatCtrl(cboCtrlList, "C")
        Call FormatCtrl(PnlCtrlList, "")
        Call FormatCtrl(gbCtrlList, "")
        Call FormatCtrl(flpCtrlList, "")
        Call FormatCtrl(lbCtrlList, "")
        Call FormatCtrl(rdoCtrlList, "")
        Call FormatCtrl(clbCtrlList, "")
        Call FormatCtrl(dtpCtrlList, "")
        Call FormatCtrl(vBtnCtrlList, "B")
        Call FormatCtrl(vSBtnCtrlList, "S")

        For Each Ctrl As C1FlexGrid In vsfCtrlList
            Ctrl.Font = New Font(Ctrl.Font.FontFamily, gintFontSize, Ctrl.Font.Style)
            Ctrl.Styles.Fixed.Font = New Font(Ctrl.Font.FontFamily, gintFontSize, Ctrl.Font.Style)
        Next
    End Sub

    Public Function GetDriveLetter(ByVal Volume As String) As String
        For Each drive As DriveInfo In My.Computer.FileSystem.Drives
            If drive.DriveType = DriveType.Fixed AndAlso drive.VolumeLabel.ToLower = Volume.ToLower Then
                Return drive.RootDirectory.Name
            End If
        Next
        Return Nothing
    End Function

    Public Function ValidateNumber(argFormName As String, argTxt As String, argRetTxt As String,
        argMandatory As Boolean, argNoOfDec As Integer, argNegative As Boolean, argPer As Boolean) As Boolean
        ValidateNumber = False
        If argTxt = "" Then argTxt = 0
        If argMandatory Then
            If Val(argTxt) = 0 Then
                'MsgBox "Number cannot be empty", vbInformation, argFormName
                Exit Function
            End If
        End If
        If Not IsNumeric(argTxt) Then
            MsgBox("Not a valid number ", vbInformation, argFormName)
            Exit Function
        End If
        If Not argNegative And Val(argTxt) < 0 Then
            MsgBox("Negative Number not allowed", vbInformation, argFormName)
            Exit Function
        End If
        If argPer Then
            If Val(argTxt) > 100 Then
                MsgBox("Not a valid percentage ", vbInformation, argFormName)
                Exit Function
            End If
        End If
        argRetTxt = SVTROUND(Val(argTxt), argNoOfDec)
        ValidateNumber = True

    End Function

    Public Function ValidateFileName(ByVal argStr As String, ByVal argReplaceStr As String) As String
        ValidateFileName = ""
        argStr = argStr.Replace("/", argReplaceStr)
        argStr = argStr.Replace("\", argReplaceStr)
        argStr = argStr.Replace("*", argReplaceStr)
        argStr = argStr.Replace(":", argReplaceStr)
        argStr = argStr.Replace("?", argReplaceStr)
        argStr = argStr.Replace("""", argReplaceStr)
        argStr = argStr.Replace("<", argReplaceStr)
        argStr = argStr.Replace(">", argReplaceStr)
        argStr = argStr.Replace("|", argReplaceStr)
        Return argStr
    End Function


    Public Sub FillHdrSection(ByVal argFrm As Object, ByVal argQry As String)
        Dim rsDocHdr As OleDb.OleDbDataReader
        rsDocHdr = clsSVT.getDataReaderFromSQL(argQry)

        If Not rsDocHdr.HasRows Then
            rsDocHdr.Close()
            Exit Sub
        End If

        rsDocHdr.Read()
        Dim ctrlList As Object
        ctrlList = GetAllDsgrVsf(argFrm, GetType(C1.Win.C1FlexGrid.C1FlexGrid))
        With rsDocHdr
            For Each argVsf As C1FlexGrid In ctrlList
                For i = 0 To argVsf.Rows.Count - 1
                    For X = 0 To .FieldCount - 1
                        If argVsf.GetData(i, vOthPerAmount) = "Y" Then
                            If UCase(.GetName(X)) = UCase(Trim(argVsf.GetData(i, vOthFldCode))) & "PER" Then
                                argVsf.SetData(i, vOthFldDataPer, GetVal(.GetValue(X), ""))
                            End If
                            If UCase(.GetName(X)) = UCase(Trim(argVsf.GetData(i, vOthFldCode))) & "AMT" Then
                                argVsf.SetData(i, vOthFldDataValue, GetVal(.GetValue(X), ""))
                            End If
                        Else
                            If UCase(.GetName(X)) = UCase(Trim(argVsf.GetData(i, vOthFldCode))) Then
                                If Trim(argVsf.GetData(i, vOthFldCmbTblName)) = "" Then
                                    argVsf.SetData(i, vOthFldDataValue, GetVal(.GetValue(X), ""))
                                Else
                                    Select Case UCase(argVsf.GetData(i, vOthFldDataType))
                                        Case "NUMERIC", "LIST NUMERIC"
                                            argVsf.SetData(i, vOthFldDataValue,
                                        clsSVT.getDataStringValue("Select " & argVsf.GetData(i, vOthFldCmbDisplayCols) & " from " &
                                        argVsf.GetData(i, vOthFldCmbTblName) & " where " &
                                        argVsf.GetData(i, vOthFldCmbStorageFldName) & " = " & GetVal(.GetValue(X), 0), "", False))
                                        Case Else
                                            argVsf.SetData(i, vOthFldDataValue,
                                        clsSVT.getDataStringValue("Select " & argVsf.GetData(i, vOthFldCmbDisplayCols) & " from " &
                                        argVsf.GetData(i, vOthFldCmbTblName) & " where " &
                                        argVsf.GetData(i, vOthFldCmbStorageFldName) & " = '" & GetVal(.GetValue(X), "") & "'", "", False))
                                    End Select
                                End If
                            End If
                        End If
                    Next X
                Next
            Next
        End With
        rsDocHdr.Close()
    End Sub

    Public Function NumToAlphabet(ByVal ColIndex As Integer) As String
        Dim div As Integer = ColIndex
        Dim colLetter As String = String.Empty
        Dim modnum As Integer = 0

        While div > 0
            modnum = (div - 1) Mod 26
            colLetter = Chr(65 + modnum) & colLetter
            div = CInt((div - modnum) \ 26)
        End While

        Return colLetter
    End Function


    Public Function AlphabetToNum(ByVal columnLetter As String) As Integer
        columnLetter = columnLetter.ToUpper()
        Dim sum As Integer = 0

        For i As Integer = 0 To columnLetter.Length - 1
            sum *= 26
            Dim charA As Integer = Char.GetNumericValue("A")
            Dim charColLetter As Integer = Char.GetNumericValue(columnLetter(i))
            sum += (charColLetter - charA) + 1
        Next
        Return sum
    End Function

    Public Function fnMenuPresent(ByVal argMenuName As String) As Boolean
        Dim blnFound As Boolean
        clsSVT.getDataStringValue("Select menuitemcaption from mstmenu where MenuItemCaption='" & argMenuName & "'", "", blnFound)
        Return blnFound
    End Function


    Public Sub fnCreateMenu(ByVal argMenuName As String, ByVal argFrmName As String, ByVal argMenuItemType As Integer)
        Dim intMaxId As Int32
        Dim intMenuIndex As Int32
        Dim sqlstr As String

        intMaxId = clsSVT.getDataLongValue("select MAX(MenuItemID)+1 from MstMenu", 0, False)
        intMenuIndex = clsSVT.getDataLongValue("select MAX(MenuIndex)+1 from MstMenu where MenuItemType=" & argMenuItemType, 0, False)

        sqlstr = "INSERT INTO MstMenu (MenuItemid,"
        sqlstr &= "MenuItemCaption ,MenuItemName ,Menuindex,"
        sqlstr &= "MenuItemType ,DocID ,CustomReport,"
        sqlstr &= "MenuShow,FrmName,IconName,"
        sqlstr &= "MenuGroupName ,IsSepReq,SCKeys)  VALUES(" & intMaxId
        sqlstr &= ",'" & argMenuName & "','SVTMNU'," & intMenuIndex
        sqlstr &= "," & argMenuItemType & ",0,'N',"
        sqlstr &= "'Y','" & argFrmName & "','',"
        sqlstr &= "'','N','') "
        clsSVT.execSQLStatement(sqlstr)
    End Sub


    'Private Function getmaxNo() As Integer
    '    Dim i As Integer, intNo As Integer
    '    With vsfDet
    '        intNo = 0
    '        For i = 1 To .Rows.Count - 1
    '            If Val(.GetData(i, vDetItemSlNo)) > intNo Then
    '                intNo = Val(.GetData(i, vDetItemSlNo))
    '            End If
    '        Next i
    '    End With
    '    intNo = intNo + 1
    '    getmaxNo = intNo
    'End Function

    'Private Sub FillDtlSection(ByVal argFrm As Object, ByVal argQry As String, ByVal argItemSlno As Int32)
    '    Dim i As Integer
    '    Dim introw As Integer
    '    Dim rsDtl As oledb.oledbDataReader
    '    rsDtl = clsSVT.getDataReaderFromSQL(argQry)

    '    With rsDtl
    '        If Not .HasRows Then
    '            .Close()
    '            Exit Sub
    '        End If

    '        argFrm.vsfDet.Rows.Count = 1
    '        Do While .Read
    '            argFrm.vsfDet.Rows.Count = argFrm.vsfDet.Rows.Count + 1
    '            introw = argFrm.vsfDet.Rows.Count - 1
    '            i = introw
    '            argFrm.vsfDet.SetData(i, argItemSlno, getmaxNo())
    '            If intItemDocID > 0 Then
    '                argFrm.vsfDet.SetData(i, vDetItemID, rsDtl(strFldItemID))
    '                argFrm.vsfDet.SetData(i, vDetItemCode, rsDtl(strFldItemCode))
    '                argFrm.vsfDet.SetData(i, vDetItemName, rsDtl(strFldItemName))
    '                argFrm.vsfDet.SetData(i, vDetConvFact, !ConvFact)
    '                argFrm.vsfDet.SetData(i, vDetUOM, !UOM)
    '            End If

    '            For k As Int32 = 0 To argFrm.vsfOptions.Rows.Count - 1
    '                For x As Int32 = 0 To rsDtl.FieldCount - 1
    '                    If UCase(GetVal(argFrm.vsfOptions.GetData(k, vOthFldCode))) = UCase(rsDtl.GetName(x)) Then
    '                        argFrm.vsfDet.SetData(i, k, rsDtl.Item(x))
    '                    End If
    '                Next
    '            Next

    '        Loop
    '        .Close()
    '    End With
    'End Sub

    Public Function RemoveSplChar(ByVal argString As String) As String
        Dim str As String = argString
        str = str.Replace(" ", "").Replace("!", "").Replace("@", "").Replace("#", "").Replace("$", "")
        str = str.Replace("""", "").Replace("%", "").Replace("^", "").Replace("&", "").Replace("*", "")
        str = str.Replace(")", "").Replace("(", "").Replace("{", "").Replace("}", "").Replace("[", "")
        str = str.Replace("]", "").Replace("|", "").Replace(";", "").Replace(":", "").Replace("'", "")
        str = str.Replace("\", "").Replace("/", "").Replace("<", "").Replace(">", "").Replace("?", "")
        str = str.Replace(",", "").Replace(".", "").Replace("+", "").Replace("-", "")
        Return str
    End Function

    Public Function GetColDataType(ByVal argTblName As String, ByVal argFldName As String)
        Return clsSVT.getDataStringValue("Select distinct DATA_TYPE from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = '" & argTblName & "' and Column_Name = '" & argFldName & "'", "", False)
    End Function

    Public Sub setVsfCellColour(argVSF As C1FlexGrid, argRow As Integer, argCol As Integer,
                            Optional argBackColor As Color = Nothing,
                         Optional argForeClr As Color = Nothing,
                         Optional argBold As Boolean = False,
                         Optional argBorderWidth As Integer = -1, Optional argBorderClr As Color = Nothing)
        Dim rg As CellRange, strClrName As String
        strClrName = argForeClr.Name & "-" & argBackColor.Name
        Dim csCell As CellStyle = argVSF.Styles.Add(strClrName)
        If Not IsNothing(argBackColor) Then csCell.BackColor = argBackColor
        If argForeClr.Name <> "0" Then
            csCell.ForeColor = argForeClr
        Else
            csCell.ForeColor = Color.Black
        End If
        If argBold Then csCell.Font = New Font(argVSF.Font.Name, argVSF.Font.Size, FontStyle.Bold)
        If argBorderWidth <> -1 Then csCell.Border.Width = 4
        If Not IsNothing(argBorderClr) Then csCell.Border.Color = argBorderClr
        rg = argVSF.GetCellRange(argRow, argCol)
        rg.Style = argVSF.Styles(strClrName)
    End Sub
    Public Sub setVsfCellRangeColour(argVSF As C1FlexGrid, argRow As Integer, argCol As Integer, argRow1 As Integer, argCol1 As Integer,
                            Optional argBackColor As Color = Nothing,
                         Optional argForeClr As Color = Nothing,
                         Optional argBold As Boolean = False,
                         Optional argBorderWidth As Integer = -1, Optional argBorderClr As Color = Nothing)
        Dim rg As CellRange, strClrName As String
        strClrName = argForeClr.Name & "-" & argBackColor.Name
        Dim csCell As CellStyle = argVSF.Styles.Add(strClrName)
        If Not IsNothing(argBackColor) Then csCell.BackColor = argBackColor
        If argForeClr.Name <> "0" Then
            csCell.ForeColor = argForeClr
        Else
            csCell.ForeColor = Color.Black
        End If
        If argBold Then csCell.Font = New Font(argVSF.Font.Name, argVSF.Font.Size, FontStyle.Bold)
        If argBorderWidth <> -1 Then csCell.Border.Width = 4
        If Not IsNothing(argBorderClr) Then csCell.Border.Color = argBorderClr
        rg = argVSF.GetCellRange(argRow, argCol, argRow1, argCol1)
        rg.Style = argVSF.Styles(strClrName)
    End Sub

    Public Sub initSMSSettings()
        Dim rs As OleDb.OleDbDataReader, strSQL As String
        strSQL = "Select * from UtlMsgSettings where Def = 'Y'"
        rs = clsSVT.getDataReaderFromSQL(strSQL)
        If Not rs.HasRows Then
            strSQL = "Select top 1 * from UtlMsgSettings order by rowID"
            rs = clsSVT.getDataReaderFromSQL(strSQL)
        End If
        If rs.HasRows Then
            rs.Read()
            gDefProviderID = rs!RowID
            gDefProvider = rs!Provider
            gSMSURL = rs!SMSURL
            gIntDailyLimit = rs!DailyLimit
        End If
        rs.Close()
    End Sub


    Public Function getUUID() As String
        getUUID = ""
        Try
            Dim searcher As New ManagementObjectSearcher(
                    "root\CIMV2",
                    "SELECT * FROM Win32_ComputerSystemProduct")

            For Each queryObj As ManagementObject In searcher.Get()
                getUUID = queryObj("UUID").ToString
            Next
        Catch err As ManagementException
            'Console.WriteLine("An error occurred while querying for WMI data: " & err.Message)
        End Try

    End Function
    Public Sub PrintInvXL(ByVal argInvID As Long, Optional argPreview As Boolean = True)
        Dim strFile As String = gstrRptPath & "\FmtInv.xlsx"
        Dim i As Integer
        If Not File.Exists(strFile) Then
            MsgBox("Excel File " & strFile & " does not Exist!", MsgBoxStyle.Critical, "File not found")
            Exit Sub
        End If
        Dim xlApp As New Excel.Application
        With xlApp
            .Workbooks.Open(strFile, , True)
            'fill Data
            Dim rs As OleDb.OleDbDataReader
            Dim cmd As New OleDbCommand
            With cmd
                cmd.Connection = svtConn
                .CommandType = CommandType.StoredProcedure
                .Parameters.Add("@SinvID", OleDbType.Integer)
                .Parameters(0).Value = argInvID
                rs = .ExecuteReader
            End With
            With rs
                If .HasRows Then
                    .Read()
                    For i = 1 To 50
                        If xlApp.Range("M" & i).Value <> "" Then
                            Try
                                xlApp.Range("N" & i).Value = rs(xlApp.Range("M" & i).Value).ToString
                            Catch ex As Exception

                            End Try
                        End If
                    Next
                End If
                .Close()
            End With
            If Not argPreview Then
                .Worksheets.PrintOut()
                .Workbooks(1).Close(SaveChanges:=False)
            Else
                .Visible = True
            End If
        End With
    End Sub
    Public Function chkInternet() As Boolean
        Try
            Using client = New WebClient()
                Using stream = client.OpenRead("http://www.google.com")
                    Return True
                End Using
            End Using
        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Function GenVSfComboStringFromQry(ByVal argQry As String) As String

        Dim rs As oledbDataReader


        rs = clsSVT.getDataReaderFromSQL(argQry)

        Dim CboString As String
        CboString = "|"

        If Not rs.HasRows Then
            rs.Close()
            Return ""
        End If
        With rs
            Do While .Read()
                CboString = CboString & "|" & rs(0)
            Loop
            .Close()
        End With
        GenVSfComboStringFromQry = CboString
    End Function

    Public Sub autoEmailTrigger()
        Dim strSql As String, strAutoAlertNames As String = ""
        Dim rsEmail As OleDb.OleDbDataReader
        Dim strSrvrType As String = clsSVT.getDataStringValue("Select setValue From utlgsettings Where setName = 'AUTOEMAILSRVR'", "OUTLOOK", False)

        strSql = "Select RptName,EmailID from vwAutoEmailList order by RptName"
        rsEmail = clsSVT.getDataReaderFromSQL(strSql)
        If Not rsEmail.HasRows Then
            rsEmail.Close()
        Else

            Do While rsEmail.Read
                Select Case UCase(rsEmail!RptName)
                    Case "STOCK BALANCE REPORT"
                        Dim objfrm As New frmRptStockBal
                        objfrm.dtpFrom.Value = getSystemDate()
                        objfrm.dtpTo.Value = getSystemDate()
                        objfrm.gBlnAutoEmail = True
                        objfrm.strEmailID = rsEmail!EmailID
                        objfrm.strEmailSrvrType = strSrvrType
                        objfrm.Show()
                        'frmMain.OpenForms(objfrm, "Stock Bal Report")

                    Case "ITEM WISE SALES REPORT"
                        itemWiseSalesReportBillWise(rsEmail!EmailID, strSrvrType)

                End Select
            Loop
            rsEmail.Close()
        End If
    End Sub

    Private Sub itemWiseSalesReportBillWise(argEmailId As String, argSrvrType As String)
        Dim strsql As String, strFile As String
        Dim dt As DataTable
        Dim dv As DataView
        Dim vsfTemp As New C1FlexGrid
        Dim rsDummy As OleDb.OleDbDataReader

        strFile = gSysTempPath & "Item_Wise_Sales_Report-BillWise as on " & Format(getSystemDate(), "ddMMMyyyy") & ".xlsx"

        vsfTemp.Cols.Fixed = 0

        'dt = clsSVT.getDATATable(strsql)
        'dv = New DataView(dt)
        'vsfTemp.DataSource = dv

        strsql = "Select SINVNO,SINVDATE,INVVALUE,ITEMNAME,SINVQTY,SINVRATE,"
        strsql &= " ACTITEMAMT,ITEMGROUPNAME,CANCELLED"
        strsql &= " from vwItemSalesReportBilWise "
        strsql &= " where SINVDATE between " & getDateFormatInCond(getSystemDate()) & " and " & getDateFormatInCond(getSystemDate()) & ""
        strsql &= " order by SINVNO,SINVDATE,ITEMNAME,ITEMGROUPNAME"
        rsDummy = clsSVT.getDataReaderFromSQL(strsql)
        If Not rsDummy.HasRows Then
            rsDummy.Close()
            Exit Sub
        End If

        FileCopy(gstrRptPath & "ItemWiseSalesReport.xlsx", strFile)

        Dim xlApp As New Excel.Application
        Dim xlWB As Excel.Workbook
        xlWB = xlApp.Workbooks.Open(strFile,, False)
        Dim introw As Integer = 2

        xlApp.Range("A1").Value = "Sales InvNo"
        xlApp.Range("B1").Value = "Sales Inv Date"
        xlApp.Range("C1").Value = "Inv Value"
        xlApp.Range("D1").Value = "Item Name"
        xlApp.Range("E1").Value = "Sales Qty"
        xlApp.Range("F1").Value = "Rate"
        xlApp.Range("G1").Value = "Amount"
        xlApp.Range("H1").Value = "Item Group"
        xlApp.Range("I1").Value = "Cancelled"

        With vsfTemp
            Do While rsDummy.Read
                xlApp.Range("A" & introw).Value = rsDummy!SINVNO
                xlApp.Range("B" & introw).Value = rsDummy!SINVDATE
                xlApp.Range("C" & introw).Value = rsDummy!INVVALUE
                xlApp.Range("D" & introw).Value = rsDummy!ITEMNAME
                xlApp.Range("E" & introw).Value = rsDummy!SINVQTY
                xlApp.Range("F" & introw).Value = rsDummy!SINVRATE
                xlApp.Range("G" & introw).Value = rsDummy!ACTITEMAMT
                xlApp.Range("H" & introw).Value = rsDummy!ITEMGROUPNAME
                xlApp.Range("I" & introw).Value = rsDummy!CANCELLED
                introw = introw + 1
            Loop
        End With
        xlApp.mer
        xlWB.Save()
        xlWB.Close()
        xlWB = Nothing
        xlApp = Nothing

        'argEmailId = "akshay@softvent.com"

        If argSrvrType.ToUpper = "OUTLOOK" Then
            argEmailId = Replace(argEmailId, ",", ";")
            If Not sendOutLookMail(argEmailId, "", "Item_Wise_Sales_Report-BillWise as on " & Format(getSystemDate(), "ddMMMyyyy"), "Please Find the attached file of Item_Wise_Sales_Report-BillWise as on " & Format(getSystemDate(), "dd MMM yyyy"), argAttFileName:=strFile) Then
            End If
        Else
            argEmailId = Replace(argEmailId, ";", ",")
            If Not sendGenMail(argEmailId, "", "Item_Wise_Sales_Report-BillWise as on " & Format(getSystemDate(), "ddMMMyyyy"), "Please Find the attached file of Item_Wise_Sales_Report-BillWise as on " & Format(getSystemDate(), "dd MMM yyyy"), strFile) Then
            End If
        End If

    End Sub
    'Public Sub updateNewLicInfo()
    '    Dim strSQl As String, intRowID As Long, strSvtLicKey As String
    '    Dim strWebConn As String = "Provider=SQLOLEDB.1;Persist Security Info=true;User ID=sa;Pwd=svtSQLadmn@2020;Initial Catalog=Softvent;Data Source=103.12.1.88\SQLEXPRESS,62350"
    '    Dim webcon As OleDbConnection = New OleDbConnection(strWebConn)
    '    Try
    '        webcon.Open()
    '    Catch ex As Exception
    '        Exit Sub
    '    End Try
    '    strSvtLicKey = getHWInfo.GenerateUID("vLQMS")
    '    intRowID = clsSVT.getDataLongValue("select RecID from MstSoftventLic where PrdID = '" & strSvtLicKey & "'", 0, False,, webcon)
    '    If intRowID = 0 Then
    '        Dim cmdIns As New OleDbCommand()

    '        cmdIns.Connection = webcon
    '        cmdIns.CommandType = CommandType.Text
    '        strSQl = "insert into MstSoftventLic (PrdID, CompName, CompAdd1, CompAdd2, "
    '        strSQl = strSQl & " compAdd3, CompAdd4, PhNo,DBName, "
    '        strSQl = strSQl & " Remarks, ClientSystemName, DateAdded) values ("
    '        strSQl = strSQl & "'" & strSvtLicKey & "',"
    '        strSQl = strSQl & "'" & gSettings.gCompName & "',"
    '        strSQl = strSQl & "'" & gSettings.gCompAdd1 & "',"
    '        strSQl = strSQl & "'" & gSettings.gCompAdd2 & "',"
    '        strSQl = strSQl & "'" & gSettings.gCompAdd3 & "',"
    '        strSQl = strSQl & "'" & gSettings.gCompAdd4 & "',"
    '        strSQl = strSQl & "'" & gSettings.gCompPHNO & "',"
    '        strSQl = strSQl & "'" & gStrDBName & "','',"
    '        strSQl = strSQl & "'" & gStrServerName & "',getdate())"
    '        cmdIns.CommandText = strSQl
    '        cmdIns.ExecuteNonQuery()
    '        cmdIns.Dispose()
    '    End If
    '    webcon.Close()
    '    webcon.Dispose()
    'End Sub
End Module

