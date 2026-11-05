Public Class WEBDBCHANGES
    Private Sub WEBDBCHANGES_Load(sender As Object, e As EventArgs) Handles MyBase.Load




        'INSERT INTO MstMenu (MenuItemid,
        '        MenuItemCaption , MenuItemName, Menuindex,
        '        MenuItemType, DocID, CustomReport,
        '        MenuShow, FrmName, IconName,
        '        MenuGroupName, IsSepReq, SCKeys)  VALUES(
        '		(select max(menuitemid) +1 from mstmenu),
        '        'Data Port To Cloud(203)','SVTMNU',6008, 7,0,'N','Y','','','','N','') 

        'go






        'CREATE TABLE RptWebStk(
        '                StkDate DateTime Not NULL, BranchID int Not NULL Default 0, ItemName varchar(100) Not NULL default '',
        '				ItemGroupName varchar(100) Not NULL default '',	OPQty numeric(18, 2) NOT NULL default 0,	OPAmt numeric(18, 2) NOT NULL default 0,
        'RcptQty numeric(18, 2) Not NULL Default 0, RcptAmt numeric(18, 2) Not NULL default 0,	IssQty numeric(18, 2) Not NULL default 0,
        '				IssAmt numeric(18, 2) Not NULL Default 0, ClQty numeric(18, 2) Not NULL default 0,	ClAmt numeric(18, 2) Not NULL default 0)

        'go







        'Create procedure [dbo].[msp_GenWebStock] 
        '	@mDate smalldatetime, @mBranchID int
        'as 

        'Select Case ItemGroupName, ItemName, 
        'Sum(OpBalQty) As OpBalQty, Sum(clQty) As clQty, sum(RcptQty) As RcptQty, Sum(IssQty) As IssQty,



        'isnull((select top 1 MRP from mstitem  where ItemID = a.itemid),0) 



        '--isnull((select top 1 PBRate from TrnDtlPB s, TrnhdrPB sh where s.pbid = sh.pbid And ItemID = a.itemid  And pbdate <=  '05 Jan 2021'  
        '           -- order by s.pbid desc),0)




        'as StkRate into #r from (

        'Select Case s.itemid, ItemGroupName, ItemName, Sum(Qty*StkMultiplier) As OpBalQty, 0 As RcptQty, 0 As IssQty, Sum(Qty*StkMultiplier) As clQty 
        'From MstItemGroup g, MstItem i, UtlStockList s 
        'Where s.ItemID = i.ItemID And i.ItemGroupID = g.ItemGroupId And DocDate <  '01 Jan 2021' 
        'group by i.PkgItem, g.ItemGroupID,ItemGroupName, s.itemid, ItemName, i.MRP, s.itemid

        'Union All  

        'Select Case s.itemid, ItemGroupName, ItemName,0 As OpBalQty, Sum(Qty*StkMultiplier) As RcptQty, 
        '0 as IssQty, Sum(Qty*StkMultiplier) as clQty 
        'From MstItemGroup g, MstItem i, MstPkg u, UtlStockList s 
        'Where s.ItemID = i.ItemID And i.PkgID = u.PkgID And i.ItemGroupID = g.ItemGroupId
        'And StkMultiplier = 1 And DocDate between  '01 Jan 2021' And   '05 Jan 2021' 
        'group by ItemGroupName, ItemName, s.itemid

        'Union All  

        'Select Case s.itemid, ItemGroupName, ItemName, 0 As OpBalQty, 0 As RcptQty, Sum(Qty*StkMultiplier) As IssQty, 
        'Sum(Qty * StkMultiplier) As clQty 
        'From MstItemGroup g, MstItem i, UtlStockList s 
        'Where s.ItemID = i.ItemID And i.ItemGroupID = g.ItemGroupId And
        'DocDate between  '01 Jan 2021' And   '05 Jan 2021' And StkMultiplier = -1 
        'group by ItemGroupName, ItemName, s.itemid) a  
        'group by ItemGroupName, ItemName, ItemID 
        'order by ItemGroupName, StkRate Desc 

        'Truncate table RptWebStk

        'Insert into rptwebstk (StkDate, BranchID, ItemName, ItemGroupName,
        'OPQty, OPAmt, RcptQty, RcptAmt, IssQty, IssAmt, ClQty, ClAmt)
        'Select Case Case@mDate, @mBranchID, ItemName, ItemGroupName, 
        'OpBalQty, OpBalQty * StkRate, RcptQty, RcptQty * stkRate, IssQty, IssQty * StkRate,
        'clQty, clQty * stkRate
        'from #r

        'Update rptwebstk set clamt = 0 where clamt < 0
        'go




    End Sub
End Class