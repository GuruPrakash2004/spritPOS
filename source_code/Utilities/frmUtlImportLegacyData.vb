Public Class frmUtlImportLegacyData
    Dim clsSvt As New svtLibrary
    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
        Call frmMain.CloseSelectedTabPage(Me)
    End Sub

    Private Sub frmUtlImportLegacyData_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        cboType.Text = "Drops"
        txtLegDB.Text = "Drops"
        vsfDet.Cols.Count = 1
        vsfDet.Rows.Count = 1
        vsfDet.SetData(0, 0, "Progress")
    End Sub

    Private Sub btnImportData_Click(sender As Object, e As EventArgs) Handles btnImportData.Click
        Select Case cboType.Text
            Case "Drops"
                Call importDataFromDropsDB
        End Select
    End Sub

    Private Sub importDataFromDropsDB()
        Dim strSQL As String
        If Not fnTablePresent(txtLegDB.Text & "..MstItem") Then
            MsgBox("Database " & txtLegDB.Text & " does not exist!", MsgBoxStyle.Critical, "Error")
            Exit Sub
        End If

        'importing item groups
        vsfDet.Rows.Count = vsfDet.Rows.Count + 1
        vsfDet.SetData(vsfDet.Rows.Count - 1, 0, "Importing ItemGroup Data")
        Application.DoEvents()

        Call clsSvt.execSQLStatement("Truncate Table MstItemGroup")
        strSQL = "Insert into MstItemGroup (ItemGroupID, ItemGroupCode, ItemGroupname, InActive, InActiveRmks, "
        strSQL &= "AddedBy, AddedOn, Authorised, AuthBy, AuthOn) "
        strSQL &= "select ItemGroupID, ItemGroupCode, ItemGroupname, 'N' as InActive, '' as InActiveRmks, "
        strSQL &= " 'ADMIN' as AddedBy, getdate() as AddedOn, 'Y' as Authorised, 'ADMIN' as AuthBy, getdate() as AuthOn"
        strSQL &= " From " & txtLegDB.Text & "..mstitemgroup"
        Call clsSvt.execSQLStatement(strSQL)
        vsfDet.SetData(vsfDet.Rows.Count - 1, 0, "Imported ItemGroup Data")
        Application.DoEvents()


        'importing UOM
        vsfDet.Rows.Count = vsfDet.Rows.Count + 1
        vsfDet.SetData(vsfDet.Rows.Count - 1, 0, "Importing UOM Data")
        Application.DoEvents()

        Call clsSvt.execSQLStatement("Truncate Table MstUOM")
        strSQL = "Insert into MstUOM (UOMID, UOMCode, UOMname, InActive, InActiveRmks, "
        strSQL &= "AddedBy, AddedOn, Authorised, AuthBy, AuthOn) "
        strSQL &= "select UOMID, UOM, UOM, 'N' as InActive, '' as InActiveRmks, "
        strSQL &= " 'ADMIN' as AddedBy, getdate() as AddedOn, 'Y' as Authorised, 'ADMIN' as AuthBy, getdate() as AuthOn"
        strSQL &= " From " & txtLegDB.Text & "..mstUOM"
        Call clsSvt.execSQLStatement(strSQL)
        vsfDet.SetData(vsfDet.Rows.Count - 1, 0, "Imported UOM Data")
        Application.DoEvents()

        'insert into mstPkg ()
        vsfDet.Rows.Count = vsfDet.Rows.Count + 1
        vsfDet.SetData(vsfDet.Rows.Count - 1, 0, "Importing Package Data")
        Application.DoEvents()

        Call clsSvt.execSQLStatement("Truncate Table MstPKG")
        Dim rsPkg As oledb.oledbDataReader, intPkgID As Integer, strCode As String

        strSQL = "Insert into MstPKG (PkgID, PKGCODE, PKGNAME, LTRPERBOT, InActive, InActiveRmks, "
        strSQL &= "AddedBy, AddedOn, Authorised, AuthBy, AuthOn) values ("
        strSQL &= "0,'-','0',0,'N','',"
        strSQL &= "'Admin',getdate(),'Y','ADMIN',getdate())"
        Call clsSvt.execSQLStatement(strSQL)

        strSQL = "select distinct ltrperbot from " & txtLegDB.Text & "..mstitem  where ltrperbot > 0 and ltrperbot <= 50"
        rsPkg = clsSvt.getDataReaderFromSQL(strSQL)
        With rsPkg
            Do While .Read
                Application.DoEvents()
                vsfDet.SetData(vsfDet.Rows.Count - 1, 0, "Importing PKG Data --- " & intPkgID)
                intPkgID = clsSvt.AutoIncrementedNum("PKGID", "MstPkg")
                strSQL = "Insert into MstPKG (PkgID, PKGCODE, PKGNAME, LTRPERBOT, InActive, InActiveRmks, "
                strSQL &= "AddedBy, AddedOn, Authorised, AuthBy, AuthOn) values ("
                strSQL &= intPkgID & ","
                If !LtrPerBot < 1 Then
                    strCode = Int((!LtrPerBot * 1000)) & "-ML"
                Else
                    strCode = fnNumberwithoutzero(!LtrPerBot) & "- Ltr"
                End If

                strSQL &= "'" & intPkgID.ToString("000") & "-" & strCode & "',"
                strSQL &= "'" & strCode & "',"
                strSQL &= !LtrPerBot & ",'N','','Admin',getdate(),'Y','ADMIN',getdate())"
                Call clsSvt.execSQLStatement(strSQL)
            Loop
            .Close()
        End With
        vsfDet.SetData(vsfDet.Rows.Count - 1, 0, "Imported PKG Data")
        Application.DoEvents()

        'insert into mstItem ()
        vsfDet.Rows.Count = vsfDet.Rows.Count + 1
        vsfDet.SetData(vsfDet.Rows.Count - 1, 0, "Importing Package Data")
        Application.DoEvents()

        Call clsSvt.execSQLStatement("Truncate Table MstItem")
        Dim rsItem As oledb.oledbDataReader, strItemName As String
        strSQL = "select i.*, itemgroupname from " & txtLegDB.Text & "..mstitem i,  " & txtLegDB.Text & "..mstitemgroup g where i.itemgroupid= g.itemgroupid order by itemid"
        rsItem = clsSvt.getDataReaderFromSQL(strSQL)

        With rsItem
            Do While .Read
                Application.DoEvents()
                vsfDet.SetData(vsfDet.Rows.Count - 1, 0, "Importing Item Data --- " & !ItemID)
                strItemName = !ItemName
                strItemName = strItemName.Replace("'", "`")

                intPkgID = clsSvt.getDataLongValue("Select PkgID from MstPkg where LTRPERBOT = " & GetVal(!ltrPerBot, 0), 0, False)

                strSQL = "Insert into MstItem (ITEMID, ITEMCODE,ITEMNAME,	ShortName,	"
                strSQL &= " HotKey,	HSNCode,	ITEMGroupID,	LiqType,	NonStkable,	"
                strSQL &= " InActive,	InActiveRmks,	UOMID,	ReOrderItem,	CRITICALLEVEL,	MAXLEVEL,	"
                strSQL &= " AddedBy,	AddedOn,Authorised,	AuthBy,	"
                strSQL &= " AuthOn,	MINLEVEL,	MAXLEVL,	ITEMSTDRATE,	CGSTPER,	SGSTPER,	BotPerCases,	"
                strSQL &= " PKGITEM,	PKGID,	CESSPER,	COMPCESS,	ITEMSELL,	MRP,	PURRATE,	"
                strSQL &= " ROL,	ROQ) values ("
                strSQL &= !ItemID & ","
                strSQL &= "'" & !ItemCode & "',"
                strSQL &= "'" & strItemName & "',"
                strSQL &= "'" & strItemName & "',"
                strSQL &= "'','',"
                strSQL &= !ItemGroupID & ","
                Select Case UCase(!itemgroupname)
                    Case "BEER"
                        strSQL &= "'BEER',"
                    Case "BRANDY", "VODKA & GIN", "WHISKY", "RUM", "GIN"
                        strSQL &= "'IML',"
                    Case "WINE", "FORTIFIED WINE"
                        strSQL &= "'WINE',"
                    Case Else
                        strSQL &= "'Others',"
                End Select
                strSQL &= "'N','N','',"
                strSQL &= !UomID & ","
                strSQL &= "'N', 0,0,"
                strSQL &= "'ADMIN', getdate(), 'Y','ADMIN',getdate(),0,0,"
                strSQL &= GetVal(!MRP, 0) & ","
                strSQL &= GetVal(!CgstPer, 0) & ","
                strSQL &= GetVal(!SGstPer, 0) & ","
                strSQL &= "0,"
                strSQL &= "'" & strItemName & "',"
                strSQL &= intPkgID & ","
                strSQL &= GetVal(!CessPer, 0) & ","
                strSQL &= GetVal(!CompCess, 0) & ","
                strSQL &= GetVal(!ItemSell, 0) & ","
                strSQL &= GetVal(!MRP, 0) & ","
                strSQL &= GetVal(!PurRate, 0) & ",0,0)"
                Call clsSvt.execSQLStatement(strSQL)
            Loop
            .Close()
        End With
        vsfDet.SetData(vsfDet.Rows.Count - 1, 0, "Imported Item Data")
        Application.DoEvents()


        'importing Supp
        vsfDet.Rows.Count = vsfDet.Rows.Count + 1
        vsfDet.SetData(vsfDet.Rows.Count - 1, 0, "Importing Supp Data")
        Application.DoEvents()

        Call clsSvt.execSQLStatement("Truncate Table MstSupp")
        strSQL = "insert into mstsupp (SUPPID, SUPPCODE, SUPPNAME, "
        strSQL &= "SUPPAliasName, InActive, InActiveRmks, BAdd1, BAdd2, BAdd3, BAdd4, City, "
        strSQL &= "PinCode, State, Country, TrnWarning, Defaultcust, AddrChange, CrLimitReq, CrLimitAmt, "
        strSQL &= "CRLmtWrng, BillWiseTrack, AddedBy, AddedOn, MOBNO, Remarks, SUPPGROUPID, "
        strSQL &= "Authorised, AuthBy, AuthOn, PHNO, FAX, WEBSITE, CONTPERSON, CURID, GSTNO, LedGroupID)"
        strSQL &= "Select SUPPID,	SUPPCODE,	SUPPNAME,	suppname As SUPPAliasName,	InActive,	'' as InActiveRmks,	"
        strSQL &= "BAdd1,BAdd2, BAdd3, BAdd4,  '' as City,	'' as PinCode,	'' as State,	"
        strSQL &= "'INDIA' as Country,	'N' as TrnWarning,	'N' as Defaultcust,"
        strSQL &= "'N' as AddrChange,	'N' as CrLimitReq,	0 as CrLimitAmt,	"
        strSQL &= "'N' as CRLmtWrng,	'N' as BillWiseTrack,"
        strSQL &= "'ADMIN' as AddedBy,	 getdate() as AddedOn,	MOBNO,	Remarks,	"
        strSQL &= "SUPPGROUPID, 'Y' as Authorised,	'ADMIN' as AuthBy,	getdate() as AuthOn,	"
        strSQL &= "'' as PHNO,	'' as FAX,	'' as WEBSITE,	'' as CONTPERSON,	CURID,	"
        strSQL &= "'' as GSTNO,	0 as LedGroupID from " & txtLegDB.Text & "..mstsupp"

        Call clsSvt.execSQLStatement(strSQL)
        vsfDet.SetData(vsfDet.Rows.Count - 1, 0, "Imported Supp Data")
        Application.DoEvents()


        'importing Purchase Bill
        vsfDet.Rows.Count = vsfDet.Rows.Count + 1
        vsfDet.SetData(vsfDet.Rows.Count - 1, 0, "Importing Header Purchase Bill Data")
        Application.DoEvents()

        Call clsSvt.execSQLStatement("Truncate Table TrnHdrPB")

        strSQL = "set identity_Insert trnhdrpb on;"
        strSQL &= " insert into trnhdrpb ("
        strSQL &= " PBID,	PBNO,	Prefix,	PrintPBNO,	PBDate,	SUPPID,	PartyName,	PartyAdd1,	"
        strSQL &= " PartyAdd2,	PartyAdd3,	PartyAdd4,	PartyCity,	PartyState,	PartyPinCode,	"
        strSQL &= " PartyCountry,	PartyCName,	PartyCMob,	PartyCEmail,	PartyCDesig,	"
        strSQL &= " Authorised,	AuthBy,	AuthOn,	LOCID,	ChangedBy,	ChangedOn,	Cancelled,	"
        strSQL &= " AddedBy,	AddedOn,	AMTINWRDS,	RMKS,	SUPPINVNO,	SUPPINVDATE,	"
        strSQL &= " SUPPINVVALUE,	TOTAMT,	OTHAMT,	GRNDTOT,	TOTCGST,	TOTSGST,	"
        strSQL &= " TOTIGST,	PURCHASETYPE)"
        strSQL &= " select row_number() over (order by pbno) as PBID, replace(PBNO,'/',''),	Prefix,	"
        strSQL &= " replace(PrintPBNO,'//','/'),	PBDate,	SUPPID,"
        strSQL &= " '' as PartyName,	'' as PartyAdd1,	'' as PartyAdd2,	'' as PartyAdd3,	"
        strSQL &= " '' as PartyAdd4,	'' as PartyCity,"
        strSQL &= " '' as PartyState,	'' as PartyPinCode,	'' as PartyCountry,	'' as PartyCName,	"
        strSQL &= " '' as PartyCMob,	'' as PartyCEmail,	"
        strSQL &= " '' as PartyCDesig,	'Y' as Authorised,	'ADMIN' as AuthBy,	pbdate as  AuthOn,"
        strSQL &= " 1 as LOCID,	'' as ChangedBy,	getdate() as ChangedOn,	Cancelled,	"
        strSQL &= " 'ADMIN' as AddedBy,	pbdate as AddedOn,	AMTINWRDS,	RMKS,"
        strSQL &= " supprefno,	supprefdt,	GRNDTOT,	TOT,	OTHAMT,	GRNDTOT,	TOTCGSTAMT,	TOTSGSTAMT,	0,	"
        strSQL &= " 'Local' from "& txtLegDB.Text &"..trnhdrpb;"
        strSQL &= " set identity_Insert trnhdrpb off"

        Call clsSvt.execSQLStatement(strSQL)
        vsfDet.SetData(vsfDet.Rows.Count - 1, 0, "Imported Header Purchase Bill Data")
        Application.DoEvents()


        'importing Detail Purchase Bill
        vsfDet.Rows.Count = vsfDet.Rows.Count + 1
        vsfDet.SetData(vsfDet.Rows.Count - 1, 0, "Importing Detail Purchase Bill Data")
        Application.DoEvents()

        Call clsSvt.execSQLStatement("Truncate Table TrnDtlPB")

        strSQL = " insert into trndtlpb ("
        strSQL &= " PBID,	slNo,	ITEMID,	UOMID,	ItemSlNO,	ShortClosed,	"
        strSQL &= " ShortClosedBy,	ShortClosedDT,	ShortClosedQty,	StkQty,	PBQty,	"
        strSQL &= " PBRate,	PBAmt,	NOOFCASES,	NOOFBOTTLES,	CGSTPER,	CGSTAMT,	"
        strSQL &= " SGSTPER,	SGSTAMT,	IGSTPER,	IGSTAMT,	NETAMT,	MRP,	PBQtyInLtrs)"
        strSQL &= " Select h.PBID,	slNo,	d.ITEMID,	d.UOMID,	ItemSlNO,	ShortClosed,	"
        strSQL &= " ShortClosedBy,	ShortClosedDT,	0 as ShortClosedQty,"
        strSQL &= " pbqty as StkQty,	PBQty,	PBRate,	PBAmt,	1,	pbqty,	d.CGSTPER,	"
        strSQL &= " CGSTAMT,	d.SGSTPER,	SGSTAMT,	0 as IGSTPER,"
        strSQL &= " 0 as IGSTAMT,	NetTot,	MRP,	pbqty * p.LTRPERBOT as PBQtyInLtrs"
        strSQL &= " From " & txtLegDB.Text & "..trndtlpb d, trnhdrpb h, mstitem i,MstPKG p"
        strSQL &= " where replace(d.pbno,'/','') = h.pbno"
        strSQL &= " And d.itemid = i.itemid"
        strSQL &= " And i.pkgid = p.pkgid"

        Call clsSvt.execSQLStatement(strSQL)
        vsfDet.SetData(vsfDet.Rows.Count - 1, 0, "Imported Detail Purchase Bill Data")
        Application.DoEvents()



        'importing Purchase Bill
        vsfDet.Rows.Count = vsfDet.Rows.Count + 1
        vsfDet.SetData(vsfDet.Rows.Count - 1, 0, "Importing Header Invoice Data")
        Application.DoEvents()

        Call clsSvt.execSQLStatement("Truncate Table TrnHdrSInv")

        strSQL = "set identity_Insert trnhdrSinv on;"
        strSQL &= " insert into trnhdrSinv ("
        strSQL &= " SINVID,	SINVNO,	Prefix,	PrintSINVNO,	SINVDate,	CUSTID,	PartyName,	"
        strSQL &= " PartyAdd1,	PartyAdd2,	PartyAdd3,	PartyAdd4,	PartyCity,	PartyState,	PartyPinCode,	"
        strSQL &= " PartyCountry,	PartyCName,	PartyCMob,	PartyCEmail,	PartyCDesig,	Authorised,	"
        strSQL &= " AuthBy,	AuthOn,		Cancelled,	AddedBy,	AddedOn,	AMTINWORDS,	RMKS,	"
        strSQL &= " LOCID,	DISC,	CGST,	SGST,	TOTQTY,	SPID,	CURID,	IsPrinted,	CASHAMT,	"
        strSQL &= " CARDAMT,	TOTAMT,	SCID,	RCVDAMT,	PAYBACKAMT,	SALTYP,	GRNDTOT,	FINYEAR,	"
        strSQL &= " InvCNo,	IsDelete,	Paid,	BILLSYSTEM,	TOTBASICAMT,	TOTCGSTAMT,	TOTSGSTAMT,	TOTCESSAMT,	"
        strSQL &= " TOTCOMPCESSAMT,	TOT,	CARDTYPE,	WRITEOFFAMT,		INVVALUE,	SINVDateTime)"
        strSQL &= " Select row_number() over (order by invno) as SINVID,	INVNO,	Prefix,	PrintINVNO,	INVDate,	"
        strSQL &= " CUSTID,	'' as PartyName, '' PartyAdd1,	'' PartyAdd2,	'' PartyAdd3,	'' PartyAdd4,	"
        strSQL &= " '' PartyCity,	'' PartyState,	'' PartyPinCode,	'' PartyCountry,"
        strSQL &= " '' PartyCName,	'' PartyCMob,	'' PartyCEmail,	'' PartyCDesig,	Authorised,	'ADMIN' AuthBy,	"
        strSQL &= " getdate() as AuthOn,	Cancelled,"
        strSQL &= " 'ADMIN' AddedBy,	getdate() AddedOn,	AMTINWRDS,	RMKS,	LOCID,	0 as DISC,	"
        strSQL &= " totcgstamt,	TOTSGSTAMT,	TOTQTY,"
        strSQL &= " 0 as SPID,	0 as CURID,	'Y' as IsPrinted,	GRNDTOT as  CASHAMT,	"
        strSQL &= " 0 as CARDAMT,	TOT,	0 as SCID,	RCVDAMT,"
        strSQL &= " PAYBACKAMT,	'Cash' as SALTYP,	GRNDTOT,	left(invno,2) as FINYEAR,	"
        strSQL &= " convert(int,right(invno,5)) as InvCNo,	'N' as IsDelete,"
        strSQL &= " 'Y' as Paid,	'' as BILLSYSTEM,	TOTBASICAMT,	TOTCGSTAMT,	TOTSGSTAMT,	"
        strSQL &= " TOTCESSAMT,	TOTCOMPCESSAMT,	TOT,	'' CARDTYPE,	0 as WRITEOFFAMT,		"
        strSQL &= " 0 as INVVALUE,	invdate SINVDateTime"
        strSQL &= " from " & txtLegDB.Text & "..trnhdrInv;"

        strSQL &= " set identity_Insert trnhdrSinv off"

        Call clsSvt.execSQLStatement(strSQL)
        vsfDet.SetData(vsfDet.Rows.Count - 1, 0, "Imported Header Invoice Data")
        Application.DoEvents()

        vsfDet.Rows.Count = vsfDet.Rows.Count + 1
        vsfDet.SetData(vsfDet.Rows.Count - 1, 0, "Importing Detail Invoice Data")
        Application.DoEvents()

        Call clsSvt.execSQLStatement("Truncate Table TrnDtlSInv")
        strSQL = " insert into trndtlsinv (SINVID, SlNo, ITEMID, UOMID, ItemSlNO, ShortClosed, "
        strSQL &= " ShortClosedBy, ShortClosedDT, ShortClosedQty, SINVQty, SINVRate, SINVAmt, CGSTPER,"
        strSQL &= " CGSTAMT, SGSTPER, SGSTAMT, MRP, DISCAMT, CESSPER, CESSAMT, ITEMCOMPCESS, BASICRATE, "
        strSQL &= " SUBTOT, COMPCESSAMT, GSTAMT, SINVQtyInLtrs, ITEMAMT, STKQty)"
        strSQL &= " Select SINVID,	SlNo,	d.ITEMID,	d.UOMID,	ItemSlNO,	ShortClosed,	"
        strSQL &= " ShortClosedBy,	ShortClosedDT,	0 As ShortClosedQty,	"
        strSQL &= " invqty as SINVQty, invrate As SINVRate, invamt As SINVAmt, d.CGSTPER, CGSTAMT, d.SGSTPER, SGSTAMT,"
        strSQL &= " invrate As MRP, 0 As DISCAMT, d.CESSPER, CESSAMT, ITEMCOMPCESS, invrate As	BASICRATE, SUBTOT, COMPCESSAMT,"
        strSQL &= " GSTTOTAMT As	GSTAMT, invqty * ltrperbot As SINVQtyInLtrs, NETTOT, invqty as STKQty "
        strSQL &= " From drops..trndtlinv d, trnhdrsinv h, drops..mstitem i"
        strSQL &= " Where d.invno = h.SINVNO"
        strSQL &= " And d.itemid = i.itemid"
        Call clsSvt.execSQLStatement(strSQL)

        vsfDet.SetData(vsfDet.Rows.Count - 1, 0, "Imported Detail Invoice Data")

        MsgBox("Data Imported Successfully", MsgBoxStyle.Information, "Imported Data")

    End Sub

    Public Function fnNumberwithoutzero(ByRef Value As String)
        fnNumberwithoutzero = 0

        Dim parts() As String = Value.Split("."c)
        If parts.Length = 2 Then
            Dim firsthalf As Object = parts(0)
            Dim secondhalf As Object = parts(1).ToString.TrimEnd("0".ToCharArray)

            If secondhalf = "" Then
                fnNumberwithoutzero = firsthalf
            Else
                fnNumberwithoutzero = firsthalf & "." & secondhalf
            End If
        End If

        Return fnNumberwithoutzero
    End Function

End Class