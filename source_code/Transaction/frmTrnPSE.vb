Imports Microsoft.Office.Interop
Imports C1.Win.C1FlexGrid
Imports System.IO
Imports System.ComponentModel

Public Class frmTrnPSE

    Dim clsSVT As New svtLibrary
    Dim strRights As String
    Dim mode As String = ""
    Dim lngInvCNO As Long

    Public strList As String
    Dim strItemGroupList As String
    Dim strItemList As String
    Dim strPkgList As String



    Const vDetGroup = 0
    Const vDetItemName = 1
    Const vDetPkg = 2
    Const vDetItemID = 3
    Const vDetOldPurRate = 4
    Const vDetOldMRPRate = 5
    Const vDetPurRate = 6
    Const vDetMRPRate = 7
    Const vDetCLBal = 8
    Const vDetQOH = 9
    Const vDetDiff = 10
    Const vDetUpdate = 11
    Const vDetPBNO = 12
    Const vDetStkDedNo = 13
    Const vDetBotPerCase = 14
    Const vDetTotCols = 15

    Const vTotGroup = 0
    Const vTotPkg = 1
    Const vTotPhyStock = 2
    Const vTotSysStock = 3
    Const vTotDiff = 4
    Const vTotPhyStkLtrs = 5
    Const vTotPhyStkCase = 6
    Const vTotPhyStkLosBot = 7
    Const vTotSysStkLtrs = 8
    Const vTotSysStkCase = 9
    Const vTotSysStkLosBot = 10
    Const vTotPhyStkCases = 11
    Const vTotSYSStkCases = 12
    Const vTotKey = 13
    Const vTotCols = 14

    Private Sub frmRptDailySummary_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.KeyPreview = True
        'fillFormProperties(Me)
        'ChangeFontSize(Me)
        InitScreen()
        ClearFlds()
        fraSummary.Visible = False
        btnSave.Visible = False
        btnCancel.Visible = False
        btnPrint.Visible = False
        strRights = getUserRights(GetMenuId("Physical Stock Entry"))
        initFormVsfDet(Me, Me.Text, vsfDet, "DET")
        initFormVsfDet(Me, Me.Text, vsfTotal, "TOTAL")
    End Sub

    Private Sub InitScreen()
        With vsfDet
            .Rows.Count = 1
            .Cols.Count = vDetTotCols

            .SetData(0, vDetPkg, "Package")
            .SetData(0, vDetGroup, "Group")
            .SetData(0, vDetItemName, "Itemname")
            .SetData(0, vDetItemID, "ItemID")
            .SetData(0, vDetOldPurRate, "OldPurRate")
            .SetData(0, vDetOldMRPRate, "OldMRPRate")
            .SetData(0, vDetPurRate, "Pur Rate")
            .SetData(0, vDetMRPRate, "MRP Rate")
            .SetData(0, vDetCLBal, "Physical Stock")
            .SetData(0, vDetQOH, "System Stock")
            .SetData(0, vDetDiff, "Diff")
            .SetData(0, vDetPBNO, "PBNO")
            .SetData(0, vDetStkDedNo, "StkDedNO")
            '.SetData(0, vDetTotCols, "")
            .Cols(vDetCLBal).Format = "#"
            .Cols(vDetDiff).Format = "#"
            .Cols(vDetQOH).Format = "#"
            .Cols(vDetCLBal).DataType = GetType(Integer)
            .Cols(vDetDiff).DataType = GetType(Integer)
            .Cols(vDetQOH).DataType = GetType(Integer)

            SetColAlign(vsfDet, svtAlign.Left, vDetPkg, vDetGroup, vDetItemName)
            SetFlexColor(vsfDet, "", vDetPkg, vDetGroup, vDetItemName, vDetItemID, vDetOldPurRate, vDetOldMRPRate, vDetQOH, vDetDiff, vDetUpdate)
            SetFlexColor(vsfDet, "M", vDetPurRate, vDetMRPRate, vDetCLBal)
            SetHideCol(vsfDet, True, vDetItemID, vDetOldPurRate, vDetOldMRPRate, vDetUpdate, vDetPBNO, vDetStkDedNo, vDetBotPerCase)
        End With

        With vsfTotal
            .Rows.Count = 1
            .Cols.Count = vTotCols

            .SetData(0, vTotPkg, "Package")
            .SetData(0, vTotGroup, "Group")
            .SetData(0, vTotPhyStock, "Physical Stock (Bottles)")
            .SetData(0, vTotSysStock, "System Stock (Bottles)")
            .SetData(0, vTotDiff, "Difference")
            .SetData(0, vTotPhyStkLtrs, "Physical Stock (Ltrs)")
            .SetData(0, vTotSysStkLtrs, "System Stock (Ltrs)")
            .SetData(0, vTotPhyStkCase, "Physical Stock (Cases)")
            .SetData(0, vTotSysStkCase, "System Stock (Cases)")
            .SetData(0, vTotPhyStkLosBot, "Physical Stock (Loose Bottles)")
            .SetData(0, vTotSysStkLosBot, "System Stock (Loose Bottles)")
            .SetData(0, vTotPhyStkCases, "Physical Stock Cases")
            .SetData(0, vTotSYSStkCases, "System Stock Cases")

            SetColAlign(vsfTotal, svtAlign.Left, vTotPkg, vTotGroup)
            SetFlexColorFullGrid(vsfTotal, "")
            SetHideCol(vsfTotal, True, vTotKey, vTotPhyStkCase, vTotSysStkCase, vTotPhyStkLosBot, vTotSysStkLosBot)
        End With
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        frmMain.CloseSelectedTabPage(Me)
    End Sub

    Private Sub frmRptReport_KeyUp(sender As Object, e As KeyEventArgs) Handles Me.KeyUp
        If e.KeyCode = Keys.F9 And e.Control = True Then
            Call initFormSize(Me, Me.Text, False, 577, 349, 0, 0)
            MsgBox("Restored Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
        If e.KeyCode = Keys.F8 And e.Control = True Then
            Call SaveActFormVsfDet(Me, Me.Text, "DET", vsfDet)
            Call SaveActFormVsfDet(Me, Me.Text, "TOTAL", vsfTotal)
            MsgBox("Saved Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If

    End Sub

    Private Sub vsfView_BeforeEdit(sender As Object, e As RowColEventArgs) Handles vsfTotal.BeforeEdit
        e.Cancel = True
    End Sub

    Private Sub btnExportToXL_Click(sender As Object, e As EventArgs) Handles btnExportToXL.Click
        ExportToXL(vsfDet)
    End Sub

    Private Sub vsfDet_AfterEdit(sender As Object, e As RowColEventArgs) Handles vsfDet.AfterEdit
        If mode = "V" Or mode = "" Then Exit Sub
        Call calTotal()
    End Sub

    Private Sub vsfDet_KeyPress(sender As Object, e As KeyPressEventArgs) Handles vsfDet.KeyPress
        If e.KeyChar = "'" Then
            e.Handled = True
        End If
    End Sub

    Private Sub vsfDet_BeforeEdit(sender As Object, e As RowColEventArgs) Handles vsfDet.BeforeEdit

        If Not (mode = "A" Or mode = "M") Then
            e.Cancel = True
            Exit Sub
        End If

        vsfDet.EditMask = ""
        vsfDet.ComboList = ""
        With vsfDet
            Select Case .Col
                Case vDetPurRate, vDetMRPRate, vDetCLBal
                Case Else
                    e.Cancel = True
            End Select
        End With

    End Sub

    Private Sub vsfDet_EnterCell(sender As Object, e As EventArgs) Handles vsfDet.EnterCell
        If Not (mode = "A" Or mode = "M") Then Exit Sub
        With vsfDet
            If .Row <= 0 Then Exit Sub
            Select Case .Col
                Case vDetPurRate, vDetMRPRate, vDetCLBal
                    .StartEditing()
                Case Else
                    Exit Sub
            End Select
        End With
    End Sub

    Private Sub vsfDet_DoubleClick(sender As Object, e As EventArgs) Handles vsfDet.DoubleClick

    End Sub

    Private Sub vsfDet_GotFocus(sender As Object, e As EventArgs) Handles vsfDet.GotFocus
        If mode = "V" Or mode = "" Then Exit Sub
        'If vsfDet.Rows.Count = 2 And Val(vsfDet.GetData(1, vDetPurRate)) = "" Then
        '    vsfDet.Col = vDetPurRate
        'End If
    End Sub

    Private Sub calTotal()
        Dim strKey As String = "", dblLtr As Double = 0, dblQty As Double = 0
        Dim dblMrpVal As Double = 0, dblPurVal As Double = 0
        Dim intRow As Int32 = 0
        With vsfDet
            For i As Int32 = 1 To .Rows.Count - 1
                .SetData(i, vDetDiff, GetVal(.GetData(i, vDetCLBal), 0) - GetVal(.GetData(i, vDetQOH), 0))
                .SetData(i, vDetUpdate, "")
                If Val(.GetData(i, vDetDiff)) <> 0 Then .SetData(i, vDetUpdate, "Y")
                If Val(.GetData(i, vDetMRPRate)) <> Val(.GetData(i, vDetOldMRPRate)) Then .SetData(i, vDetUpdate, "Y")
                If Val(.GetData(i, vDetPurRate)) <> Val(.GetData(i, vDetOldPurRate)) Then .SetData(i, vDetUpdate, "Y")

                'If strKey <> GetVal(.GetData(i, vDetPkg), "") & "-" & GetVal(.GetData(i, vDetGroup), "") Then
                '    strKey = GetVal(.GetData(i, vDetPkg), "") & "-" & GetVal(.GetData(i, vDetGroup), "")
                '    intRow = vsfTotal.FindRow(strKey, 1, vTotKey, False, True, False)
                '    dblLtr = 0
                '    dblMrpVal = 0
                '    dblPurVal = 0
                '    dblQty = 0
                'End If
                'If intRow > 0 Then
                '    dblLtr = dblLtr + (Val(.GetData(i, vDetLtrs)) * GetVal(.GetData(i, vDetDiff), 0))
                '    dblQty = dblQty + GetVal(.GetData(i, vDetDiff), 0)
                '    dblMrpVal = dblMrpVal + (GetVal(.GetData(i, vDetMRPRate), 0) * GetVal(.GetData(i, vDetDiff), 0))
                '    dblPurVal = dblPurVal + (GetVal(.GetData(i, vDetPurRate), 0) * GetVal(.GetData(i, vDetDiff), 0))
                '    vsfTotal.SetData(intRow, vTotQty, dblQty)
                '    vsfTotal.SetData(intRow, vTotMRPVal, dblMrpVal)
                '    vsfTotal.SetData(intRow, vTotPurVal, dblPurVal)
                '    vsfTotal.SetData(intRow, vTotLtrs, dblLtr)
                'End If
            Next
        End With
    End Sub

    Private Sub ClearFlds()
        chkItem.Checked = False
        chkItemGroup.Checked = False
        chkPkg.Checked = False
        txtDocID.Text = ""
        mode = ""
        vsfDet.Rows.Count = 1
        vsfTotal.Rows.Count = 1

        btnSave.Visible = False
        btnCancel.Visible = False
        btnView.Visible = True
        btnFind.Visible = True
        btnClose.Visible = True
        btnUpdate.Visible = False
        pnlDocNo.Visible = False
        btnPrint.Visible = False
        btnShowSumm.Visible = False
        btnExportToXL.Visible = False
    End Sub

    Private Sub chkPkg_CheckedChanged(sender As Object, e As EventArgs) Handles chkPkg.CheckedChanged
        Dim strSQL As String
        If chkPkg.Checked = False Then
            strList = ""
            strPkgList = ""
            Exit Sub
        End If

        Dim frmSel As New frmUtlMultiSelection

        strSQL = "Select distinct 0 as sel, PKGID, PKGCode, LtrPerBot from MstPKG where PKGID > 0 and "
        strSQL = strSQL & " InActive = 'N' order by LtrPerBot"
        Call frmSel.InitVsfSel(strSQL, 1, 1, True, Me, False)
        frmSel.Text = "Select [Multiple] Package"
        frmSel.vsfDet.SetData(0, 0, "Sel")
        frmSel.ShowDialog()
        strPkgList = strList

        If Trim(strPkgList) = "" Then chkPkg.Checked = False
    End Sub

    Private Sub chkItemGroup_CheckedChanged(sender As Object, e As EventArgs) Handles chkItemGroup.CheckedChanged
        Dim strSQL As String
        If chkItemGroup.Checked = False Then
            strList = ""
            strItemGroupList = ""
            Exit Sub
        End If

        Dim frmSel As New frmUtlMultiSelection

        strSQL = "Select distinct 0 as sel, ItemGroupID, ItemGroupCode from MstItemGroup where ItemGroupID > 0 and "
        strSQL = strSQL & " InActive = 'N' order by ItemGroupCode"
        Call frmSel.InitVsfSel(strSQL, 1, 1, True, Me, False)
        frmSel.Text = "Select [Multiple] Item Groups"
        frmSel.vsfDet.SetData(0, 0, "Sel")
        frmSel.ShowDialog()
        strItemGroupList = strList

        If Trim(strItemGroupList) = "" Then chkItemGroup.Checked = False
    End Sub


    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        ClearFlds()
        btnSave.Visible = False
        btnCancel.Visible = False
    End Sub

    Private Sub btnFind_Click(sender As Object, e As EventArgs) Handles btnFind.Click
        Dim strSQL As String, strFMT As String
        strFMT = ""

        strSQL = "Select Distinct h.PSEID,h.PSEDAte,SysName, UserName, EntryTime"
        strSQL &= " from TrnHdrPSE h"
        strSQL &= " Order by 1 desc"

        Dim frmDisplay As New frmUtlSingleSel

        With frmDisplay
            .Text = "View Previous Entered Records"
            Call .FillViewVsf(Me, strSQL, txtDocID, 0, -1)
            .ShowDialog()
            .vsfDet.Focus()
        End With
        If Val(txtDocID.Text) = 0 Then Exit Sub
        mode = "V"
        fillData()
        showBtns("LOADDOC")
        pnlDocNo.Visible = True
    End Sub

    Private Sub loadPSE()
        Dim strSql As String
        strSql = ""
    End Sub

    Private Sub btnView_Click(sender As Object, e As EventArgs) Handles btnView.Click
        If Not (chkItem.Checked Or chkPkg.Checked Or chkItemGroup.Checked) Then
            MsgBox("Please Select Alteast One Item / ItemGroup / Package..", MsgBoxStyle.Exclamation, Me.Text)
            Exit Sub
        End If
        mode = "A"
        txtDocID.Text = ""
        Call fillData()
        Call calTotal()

        If vsfDet.Rows.Count > 1 Then
            showBtns("LOAD")
        End If

    End Sub

    Private Sub showBtns(argAction As String)
        'ClearFlds()

        Select Case argAction.ToUpper
            Case "LOAD"
                btnSave.Visible = True
                btnCancel.Visible = True
                btnView.Visible = False
                btnFind.Visible = False
                btnClose.Visible = False
                btnUpdate.Visible = True
                pnlDocNo.Visible = False
                btnPrint.Visible = False
                btnShowSumm.Visible = True

            Case "LOADDOC"
                pnlDocNo.Visible = True
                btnUpdate.Visible = False
                btnSave.Visible = False
                btnCancel.Visible = False
                btnExportToXL.Visible = True
                btnShowSumm.Visible = True
                btnPrint.Visible = True

            Case "SAVE"
                btnSave.Visible = False
                btnCancel.Visible = False
                btnClose.Visible = True
                btnPrint.Visible = True
                btnExportToXL.Visible = True
                btnShowSumm.Visible = True
                btnUpdate.Visible = False
                btnView.Visible = True

        End Select
    End Sub

    Private Sub fillData()
        Dim strSQL As String = "", strKey As String = "", intRow As Int32 = 0
        strSQL = " Select i.ItemID, ITEMCODE, ITEMNAME, ITEMGROUPCODE, PKGCODE, LTRPERBOT, MRP, PURRATE,BOTPERCASES,  "
        strSQL &= " IsNull((Select Sum(Qty * StkMultiplier) From UtlStockList u  "
        strSQL &= " Where u.ItemID=i.ItemID and DocDate "
        strSQL &= " Between '2017-01-01' "
        strSQL &= " and '" & getDateFormatInInsert(getSystemDate) & "' "
        strSQL &= " ),0) as QOH "
        strSQL &= " From MstItem i, MstItemGroup ig, MstPkg p "
        strSQL &= " Where i.ITEMGroupID=ig.ITEMGROUPID "
        strSQL &= " and i.PKGID=p.PKGID "
        If Val(txtDocID.Text) > 0 Then
            strSQL &= " and i.ITEMID in (Select ITEMID from TrnDtlPSE where PSEID=" & Val(txtDocID.Text) & ")"
        End If
        If chkItem.Checked AndAlso strItemList <> "" Then
            strSQL &= " and i.ItemID in (" & strItemList & ")"
        End If
        If chkItemGroup.Checked AndAlso strItemGroupList <> "" Then
            strSQL &= " and i.ItemGroupID in (" & strItemGroupList & ")"
        End If
        If chkPkg.Checked AndAlso strPkgList <> "" Then
            strSQL &= " and i.PKGID in (" & strPkgList & ")"
        End If
        strSQL &= " and i.InActive = 'N' "
        strSQL &= " Order by  GroupOrderBy, ItemGroupName, Pkgitem , pkgorderby, PURRATE Desc  "

        Dim rs As oledb.oledbDataReader
        rs = clsSVT.getDataReaderFromSQL(strSQL)
        With vsfDet
            .Rows.Count = 1
            vsfTotal.Rows.Count = 1
            Do While rs.Read
                .Rows.Count = .Rows.Count + 1
                .SetData(.Rows.Count - 1, vDetPkg, rs("PKGCODE"))
                .SetData(.Rows.Count - 1, vDetGroup, rs("ITEMGROUPCODE"))
                .SetData(.Rows.Count - 1, vDetItemName, rs("ITEMNAME"))
                .SetData(.Rows.Count - 1, vDetItemID, rs("ItemID"))
                .SetData(.Rows.Count - 1, vDetOldPurRate, rs("PURRATE"))
                .SetData(.Rows.Count - 1, vDetOldMRPRate, rs("MRP"))
                .SetData(.Rows.Count - 1, vDetPurRate, rs("PurRate"))
                .SetData(.Rows.Count - 1, vDetMRPRate, rs("MRP"))
                .SetData(.Rows.Count - 1, vDetCLBal, rs("QOH"))
                .SetData(.Rows.Count - 1, vDetQOH, rs("QOH"))
                .SetData(.Rows.Count - 1, vDetBotPerCase, rs("BOTPERCASES"))
                '.SetData(.Rows.Count - 1, vDetDiff, rs("Diff"))

                strKey = GetVal(.GetData(.Rows.Count - 1, vDetPkg), "") & "-" & GetVal(.GetData(.Rows.Count - 1, vDetGroup), "")
                intRow = vsfTotal.FindRow(strKey, 1, vTotKey, False, True, False)
                If intRow <= 0 Then
                    With vsfTotal
                        .Rows.Count = .Rows.Count + 1
                        .SetData(.Rows.Count - 1, vTotPkg, rs("PKGCODE"))
                        .SetData(.Rows.Count - 1, vTotGroup, rs("ITEMGROUPCODE"))
                        .SetData(.Rows.Count - 1, vTotKey, strKey)
                    End With
                End If
            Loop
            rs.Close()
        End With
        Call calTotal()
        vsfDet.AllowMerging = AllowMergingEnum.RestrictAll
        vsfDet.Cols(vDetPkg).AllowMerging = True
        vsfDet.Cols(vDetGroup).AllowMerging = True
    End Sub

    Private Sub btnPrint_Click(sender As Object, e As EventArgs) Handles btnPrint.Click
        Dim strSF As String, strRptName As String
        Dim strFormulas(,) As String
        strRptName = "fmtPSE.rpt"

        strSF = "{TrnHdrPSE.PSEID} =" & Val(txtDocID.Text)
        ReDim strFormulas(1, 2)
        strFormulas = Nothing
        Dim frmv As New frmUtlReportViewer
        With frmv
            .StartPosition = FormStartPosition.CenterScreen
            .WindowState = FormWindowState.Maximized
            .showRptFile(RemoveSplChar(Me.Text), strRptName, strSF, strFormulas, False)
            .Show()
            '.ShowRptFile(RemoveSplChar(Me.Text), gstrRptPath & strRptName, strSF, strFormulas, False, argPrinterName:=strPrinterName)
        End With

    End Sub

    Private Sub chkItem_CheckedChanged(sender As Object, e As EventArgs) Handles chkItem.CheckedChanged
        Dim strSQL As String
        If chkItem.Checked = False Then
            strList = ""
            strItemList = ""
            Exit Sub
        End If

        Dim frmSel As New frmUtlMultiSelection

        strSQL = "Select distinct 0 as sel, ItemID, ItemName from MstItem where ItemID > 0 and "
        strSQL = strSQL & " InActive = 'N' order by ItemName"
        Call frmSel.InitVsfSel(strSQL, 1, 1, True, Me, False)
        frmSel.Text = "Select [Multiple] Items"
        frmSel.vsfDet.SetData(0, 0, "Sel")
        frmSel.ShowDialog()
        strItemList = strList

        If Trim(strItemList) = "" Then chkItem.Checked = False
    End Sub

    Private Sub btnUpdate_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click
        With vsfDet
            For i As Int32 = 1 To .Rows.Count - 1
                .SetData(i, vDetCLBal, 0)
            Next
        End With
        Call calTotal()
    End Sub
    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Dim strsql As String = ""
        Dim blnPBUpdate As Boolean = False, blnSDUpdate As Boolean = False
        RemoveEmptVSFRows(vsfDet, 0, vDetUpdate)

        If vsfDet.Rows.Count = 1 Then
            MsgBox("There are no Changes to Update!", MsgBoxStyle.Exclamation, Me.Text)
            Exit Sub
        End If

        Call showWaitCursor()
        Dim svtTrn As oledb.oledbTransaction
        svtTrn = svtConn.BeginTransaction(IsolationLevel.ReadCommitted)
        Try
            With vsfDet
                For i = 1 To .Rows.Count - 1
                    If .GetData(i, vDetUpdate) = "Y" Then
                        If Val(.GetData(i, vDetDiff)) > 0 Then blnPBUpdate = True
                        If Val(.GetData(i, vDetDiff)) < 0 Then blnSDUpdate = True

                        If Val(.GetData(i, vDetMRPRate)) <> Val(.GetData(i, vDetOldMRPRate)) Then
                            strsql = "Update MstItem set MRP = " & Val(.GetData(i, vDetMRPRate))
                            strsql = strsql & " where ItemID = " & Val(.GetData(i, vDetItemID))
                            clsSVT.execSQLStatement(strsql, svtTrn)
                        End If
                        If Val(.GetData(i, vDetPurRate)) <> Val(.GetData(i, vDetOldPurRate)) Then
                            strsql = "Update MstItem set Purrate = " & Val(.GetData(i, vDetPurRate))
                            strsql = strsql & " where ItemID = " & Val(.GetData(i, vDetItemID))
                            clsSVT.execSQLStatement(strsql, svtTrn)
                        End If
                    End If
                Next
            End With

            If blnPBUpdate Then Call updatePB(svtTrn)
            If blnSDUpdate Then
                If chkSales.Checked Then
                    Call updateSales(svtTrn)
                Else
                    Call updateSD(svtTrn)
                End If
            End If

            With vsfDet
                strsql = " Insert Into TrnhdrPSE (SysName, UserName) "
                strsql &= " Values ( "
                strsql &= "'" & gStrSystemName & "',"
                strsql &= "'" & gUserCode & "')"
                clsSVT.execSQLStatement(strsql, svtTrn)

                txtDocID.Text = clsSVT.getDataLongValue("Select Max(PSEID) From TrnhdrPSE", 0, False, svtTrn)

                For i As Int32 = 1 To .Rows.Count - 1
                    strsql = " Insert Into TrnDtlPSE (PSEID, Slno, ItemID, PRate, MRate, ClQty, QOH, DiffQty,PBNO, STKDedNo) "
                    strsql &= " Values ("
                    strsql &= Val(txtDocID.Text) & ","
                    strsql &= Val(i) & ","
                    strsql &= Val(.GetData(i, vDetItemID)) & ","
                    strsql &= Val(.GetData(i, vDetPurRate)) & ","
                    strsql &= Val(.GetData(i, vDetMRPRate)) & ","
                    strsql &= Val(.GetData(i, vDetCLBal)) & ","
                    strsql &= Val(.GetData(i, vDetQOH)) & ","
                    strsql &= Val(.GetData(i, vDetDiff)) & ","
                    strsql &= "'" & (.GetData(i, vDetPBNO)) & "',"
                    strsql &= "'" & (.GetData(i, vDetStkDedNo)) & "')"
                    clsSVT.execSQLStatement(strsql, svtTrn)

                Next
            End With

            svtTrn.Commit()

            btnSave.Visible = False
            btnCancel.Visible = False
            showBtns("SAVE")
            pnlDocNo.Visible = True

        Catch ex As Exception
            svtTrn.Rollback()
            Call showNormalCursor()
            MsgBox("Error during saving." & vbCrLf & ex.Message, MsgBoxStyle.Critical, Me.Text)
        End Try
        Call showNormalCursor()
    End Sub

    Private Sub updatePB(argTrn As oledb.oledbTransaction)
        Dim strSQL As String = "", intPBID As Long, intSuppID As Integer
        Dim strTNo As String, sngCases As Single
        Dim sngPurPrice As Single
        strTNo = TrnNoAutoGen("TrnHdrPB", "PBNO", "", getSystemDate(argTrn), 1276,,, argTrn)

        intSuppID = clsSVT.getDataLongValue("Select SuppID from MstSupp where suppCode = 'KSBCL'", 0, False, argTrn)

        strTNo = getFmtDocNo("", strTNo, getSystemDate(argTrn))
        strSQL = "insert into trnhdrpb "
        strSQL &= " (PBNO,Prefix,PrintPBNO,PBDate,SUPPID, "
        strSQL &= " PartyName,PartyAdd1,PartyAdd2,PartyAdd3,PartyAdd4,PartyCity,PartyState,PartyPinCode, "
        strSQL &= " PartyCountry,PartyCName,PartyCMob,PartyCEmail,PartyCDesig,Authorised,AuthBy,AuthOn, "
        strSQL &= " LOCID,ChangedBy,ChangedOn,Cancelled,AddedBy,AddedOn,AMTINWRDS,RMKS,SUPPINVNO, "
        strSQL &= " SUPPINVDATE,SUPPINVVALUE,TOTAMT,OTHAMT,GRNDTOT,TOTCGST,TOTSGST,TOTIGST,PURCHASETYPE) values ("
        strSQL &= "'" & strTNo & "',"
        strSQL &= "'',"
        strSQL &= "'" & strTNo & "',"
        strSQL &= "'" & getDateFormatInInsert(getSystemDate(argTrn)) & "',"
        strSQL &= intSuppID & ",'-','-','-','-','-','-','-','-','-','-','-','-','-','Y','ADMIN',"
        strSQL &= "'" & getDateFormatInInsert(getSystemDate(argTrn)) & "',1,'',Null,'N','ADMIN',"
        strSQL &= "'" & getDateFormatInInsert(getSystemDate(argTrn)) & "',"
        strSQL &= "'','From Physical Stock Entry','',Null,0,0,0,0,0,0,0,'')"
        Call clsSVT.execSQLStatement(strSQL, argTrn)

        intPBID = clsSVT.getDataLongValue("Select max(PBID) from TrnHdrPB", 0, False, argTrn)

        With vsfDet
            For i As Int32 = 1 To .Rows.Count - 1
                If GetVal(.GetData(i, vDetDiff)) > 0 Then
                    .SetData(i, vDetPBNO, strTNo)
                    strSQL = "Insert into TrnDtlPB (PBID, SlNo, ITEMID,UOMID,ItemSlNO,StkQty,PBQty,PBRate,PBAmt,NOOFCASES,NOOFBOTTLES,"
                    strSQL &= "CGSTPER,CGSTAMT,SGSTPER,SGSTAMT,IGSTPER,IGSTAMT,NETAMT,MRP,PBQtyInLtrs) values ("
                    strSQL &= intPBID & ","
                    strSQL &= i & ","
                    strSQL &= .GetData(i, vDetItemID) & ","
                    strSQL &= clsSVT.getDataLongValue("Select UomID From MstItem Where ItemID = " & GetVal(.GetData(i, vDetItemID), 0), 0, False, argTrn) & ","
                    strSQL &= i & ","
                    strSQL &= Val(GetVal(.GetData(i, vDetDiff), 0)) & ","
                    strSQL &= Val(GetVal(.GetData(i, vDetDiff), 0)) & ","
                    strSQL &= Val(GetVal(.GetData(i, vDetPurRate), 0)) & ","
                    strSQL &= Val(GetVal(.GetData(i, vDetPurRate), 0)) * Val(GetVal(.GetData(i, vDetDiff), 0)) & ","
                    sngCases = clsSVT.getDataLongValue("Select BotPerCases From MstItem Where ItemID = " & GetVal(.GetData(i, vDetItemID), 0), 0, False, argTrn)
                    If sngCases > 0 Then
                        strSQL &= Math.Round(Val(GetVal(.GetData(i, vDetDiff), 0)) / sngCases, 2) & ","
                    Else
                        strSQL &= "0,"
                    End If
                    strSQL &= sngPurPrice * Val(GetVal(.GetData(i, vDetDiff), 0)) & ","
                    strSQL &= "0,0,0,0,0,0,"
                    strSQL &= sngPurPrice * Val(GetVal(.GetData(i, vDetDiff), 0)) & ","
                    strSQL &= Val(GetVal(.GetData(i, vDetMRPRate), 0)) & ","
                    sngCases = clsSVT.getDataLongValue("Select LtrPerBot From MstPkg p, MstItem i Where i.PkgID = i.PkgID and ItemID = " & GetVal(.GetData(i, vDetItemID), 0), 0, False, argTrn)
                    strSQL &= Math.Round(Val(GetVal(.GetData(i, vDetDiff), 0)) * sngCases, 2) & ")"
                    Call clsSVT.execSQLStatement(strSQL, argTrn)
                End If
            Next
        End With

        strSQL = " Insert Into UtlStockList "
        strSQL &= " (ItemID, StkMultiplier, DocType, DocDate, DocID, DocNo, Particulars, Qty, LtrQty, Rate, LocID) "
        strSQL &= " Select ItemID, '1', 'PB' , PBDate, h.PBID, PBNO, PartyName, PBQty, PBQTYINLTRS, PBRate, h.LOCID "
        strSQL &= " From TrnDtlPB d, TrnHdrPB h "
        strSQL &= " Where d.PBID = h.PBID "
        strSQL &= " and d.PBID = " & intPBID
        Call clsSVT.execSQLStatement(strSQL, argTrn)

    End Sub

    Private Sub updateSD(argTrn As oledb.oledbTransaction)
        Dim strSQL As String = "", intStkDedID As Long
        Dim strTNo As String, sngCases As Single
        strTNo = TrnNoAutoGen("TrnHdrStkDed", "StkDedNO", "", getSystemDate(argTrn), 1279,,, argTrn)

        strTNo = getFmtDocNo("", strTNo, getSystemDate(argTrn))
        strSQL = "insert into trnhdrStkDed "
        strSQL &= " (STKDEDNo, Prefix, PrintSTKDEDNo, STKDEDDate,  AddedBy, AddedOn, Authorised, Cancelled,LOCID) values ("
        strSQL &= "'" & strTNo & "','',"
        strSQL &= "'" & strTNo & "',"
        strSQL &= "'" & getDateFormatInInsert(getSystemDate(argTrn)) & "',"
        strSQL &= "'ADMIN',"
        strSQL &= "'" & getDateFormatInInsert(getSystemDate(argTrn)) & "','Y','N',1)"
        Call clsSVT.execSQLStatement(strSQL, argTrn)

        intStkDedID = clsSVT.getDataLongValue("Select max(StkDedID) from TrnHdrStkDed", 0, False, argTrn)

        With vsfDet
            For i As Int32 = 1 To .Rows.Count - 1
                If GetVal(.GetData(i, vDetDiff)) < 0 Then
                    .SetData(i, vDetStkDedNo, strTNo)
                    strSQL = "Insert into TrnDtlStkDed (STKDEDID , slNo,ITEMSlno, ITEMID, UOMID, STKDEDQTY,"
                    strSQL &= " STKQTY,LTRPERBOT,STKDEDQTYINLTRS) values ("
                    strSQL &= intStkDedID & ","
                    strSQL &= i & ","
                    strSQL &= i & ","
                    strSQL &= .GetData(i, vDetItemID) & ","
                    strSQL &= clsSVT.getDataLongValue("Select UomID From MstItem Where ItemID = " & GetVal(.GetData(i, vDetItemID), 0), 0, False, argTrn) & ","
                    strSQL &= Math.Abs(Val(GetVal(.GetData(i, vDetDiff), 0))) & ","
                    strSQL &= Math.Abs(Val(GetVal(.GetData(i, vDetDiff), 0))) & ","
                    sngCases = clsSVT.getDataLongValue("Select LtrPerBot From MstPkg p, MstItem i Where i.PkgID = i.PkgID And ItemID = " & GetVal(.GetData(i, vDetItemID), 0), 0, False, argTrn)
                    If sngCases > 0 Then
                        strSQL &= sngCases & ","
                        strSQL &= Math.Abs(Math.Round(Val(GetVal(.GetData(i, vDetDiff), 0)) / sngCases, 2)) & ")"
                    Else
                        strSQL &= "0,"
                        strSQL &= "0)"
                    End If
                    Call clsSVT.execSQLStatement(strSQL, argTrn)
                End If
            Next
        End With

        strSQL = " Insert Into UtlStockList "
        strSQL &= " (ItemID, StkMultiplier, DocType, DocDate, DocID, DocNo, Particulars, Qty, LtrQty, Rate, LocID) "
        strSQL &= "  Select ItemID, '-1', 'STKDED' , STKDEDDate, h.STKDEDID, STKDEDNO, "
        strSQL &= " 'Auto from PhyStock' , STKDEDQty, STKDEDQTYINLTRS,0, 1  "
        strSQL &= " From TrnDtlStkDed d, TrnHdrStkDed h "
        strSQL &= " Where d.StkDedID = h.StkDedID "
        strSQL &= " and d.StkDedID = " & intStkDedID
        Call clsSVT.execSQLStatement(strSQL, argTrn)
    End Sub

    Private Sub btnShowSumm_Click(sender As Object, e As EventArgs) Handles btnShowSumm.Click
        Dim strPkgGrp As String, strCases As String = ""
        Dim intCtr As Integer = -1, dblLtrsPerBot As Double

        With vsfTotal
            .Rows.Count = 1

            For i As Int32 = 1 To vsfDet.Rows.Count - 1
                strPkgGrp = vsfDet.GetData(i, vDetPkg) & "|" & vsfDet.GetData(i, vDetGroup)
                intCtr = -1
                For t As Int32 = 1 To .Rows.Count - 1
                    If strPkgGrp.ToUpper = .GetData(t, vTotPkg) & "|" & .GetData(t, vTotGroup) Then
                        intCtr = t
                        .SetData(t, vTotPhyStock, Val(vsfDet.GetData(i, vDetCLBal)) + Val(.GetData(t, vTotPhyStock)))
                        .SetData(t, vTotSysStock, Val(vsfDet.GetData(i, vDetQOH)) + Val(.GetData(t, vTotSysStock)))
                        .SetData(t, vTotDiff, GetVal(.GetData(t, vTotPhyStock), 0) - GetVal(.GetData(t, vTotSysStock), 0))

                        If Val(vsfDet.GetData(i, vDetBotPerCase)) > 0 Then .SetData(t, vTotPhyStkCase, Val(.GetData(t, vTotPhyStkCase)) + Int(Val(vsfDet.GetData(i, vDetCLBal)) / Val(vsfDet.GetData(i, vDetBotPerCase))))
                        If Val(vsfDet.GetData(i, vDetBotPerCase)) > 0 Then .SetData(t, vTotSysStkCase, Val(.GetData(t, vTotSysStkCase)) + Int(Val(vsfDet.GetData(i, vDetQOH)) / Val(vsfDet.GetData(i, vDetBotPerCase))))

                        If Val(vsfDet.GetData(i, vDetBotPerCase)) > 0 Then .SetData(t, vTotPhyStkLosBot, Val(.GetData(t, vTotPhyStkLosBot)) + (Val(vsfDet.GetData(i, vDetCLBal)) - (Int(Val(vsfDet.GetData(i, vDetCLBal)) / Val(vsfDet.GetData(i, vDetBotPerCase))) * Val(vsfDet.GetData(i, vDetBotPerCase)))))
                        If Val(vsfDet.GetData(i, vDetBotPerCase)) > 0 Then .SetData(t, vTotSysStkLosBot, Val(.GetData(t, vTotSysStkLosBot)) + (Val(vsfDet.GetData(i, vDetQOH)) - (Int(Val(vsfDet.GetData(i, vDetQOH)) / Val(vsfDet.GetData(i, vDetBotPerCase))) * Val(vsfDet.GetData(i, vDetBotPerCase)))))
                        Exit For
                    End If
                Next
                'If InStr(strPkgGrp, "250") > 0 Then Stop
                If intCtr = -1 Then
                    .Rows.Count = .Rows.Count + 1
                    .SetData(.Rows.Count - 1, vTotGroup, vsfDet.GetData(i, vDetGroup))
                    .SetData(.Rows.Count - 1, vTotPkg, vsfDet.GetData(i, vDetPkg))
                    .SetData(.Rows.Count - 1, vTotPhyStock, Val(vsfDet.GetData(i, vDetCLBal)))
                    .SetData(.Rows.Count - 1, vTotSysStock, Val(vsfDet.GetData(i, vDetQOH)))
                    .SetData(.Rows.Count - 1, vTotDiff, GetVal(vsfDet.GetData(i, vDetCLBal), 0) - GetVal(vsfDet.GetData(i, vDetQOH), 0))
                    If Val(vsfDet.GetData(i, vDetBotPerCase)) > 0 Then .SetData(.Rows.Count - 1, vTotPhyStkCase, Int(Val(vsfDet.GetData(i, vDetCLBal)) / Val(vsfDet.GetData(i, vDetBotPerCase))))
                    If Val(vsfDet.GetData(i, vDetBotPerCase)) > 0 Then .SetData(.Rows.Count - 1, vTotSysStkCase, Int(Val(vsfDet.GetData(i, vDetQOH)) / Val(vsfDet.GetData(i, vDetBotPerCase))))

                    If Val(vsfDet.GetData(i, vDetBotPerCase)) > 0 Then .SetData(.Rows.Count - 1, vTotPhyStkLosBot, Val(vsfDet.GetData(i, vDetCLBal)) - (Int(Val(vsfDet.GetData(i, vDetCLBal)) / Val(vsfDet.GetData(i, vDetBotPerCase))) * Val(vsfDet.GetData(i, vDetBotPerCase))))
                    If Val(vsfDet.GetData(i, vDetBotPerCase)) > 0 Then .SetData(.Rows.Count - 1, vTotSysStkLosBot, Val(vsfDet.GetData(i, vDetQOH)) - (Int(Val(vsfDet.GetData(i, vDetQOH)) / Val(vsfDet.GetData(i, vDetBotPerCase))) * Val(vsfDet.GetData(i, vDetBotPerCase))))
                End If
            Next
            .AutoSizeCols()
            .Cols(vTotGroup).AllowMerging = True
            .Sort(SortFlags.Ascending, vTotGroup)


            'to get the stock in ltrs
            For i As Int32 = 1 To .Rows.Count - 1
                strCases = ""
                dblLtrsPerBot = clsSVT.getDataDoubleValue("Select LTRPERBOT from MstPkg where PKGCODE='" & GetVal(.GetData(i, vTotPkg), "") & "'", 0, False)
                .SetData(i, vTotPhyStkLtrs, Math.Round(Val(.GetData(i, vTotPhyStock)) * dblLtrsPerBot, 3))
                .SetData(i, vTotSysStkLtrs, Math.Round(Val(.GetData(i, vTotSysStock)) * dblLtrsPerBot, 3))

                If Val(.GetData(i, vTotPhyStkCase)) <> 0 Then
                    If Val(.GetData(i, vTotPhyStkCase)) > 1 Then
                        strCases = Val(.GetData(i, vTotPhyStkCase)) & " Cases"
                    Else
                        strCases = Val(.GetData(i, vTotPhyStkCase)) & " Case"
                    End If
                End If
                If Val(.GetData(i, vTotPhyStkLosBot)) <> 0 Then
                    If Val(.GetData(i, vTotPhyStkLosBot)) > 1 Then
                        strCases = strCases & "," & Val(.GetData(i, vTotPhyStkLosBot)) & " Bottles"
                    Else
                        strCases = strCases & "," & Val(.GetData(i, vTotPhyStkLosBot)) & " Bottle"
                    End If
                End If
                If Strings.Left(strCases, 1) = "," Then strCases = Replace(strCases, ",", "")
                .SetData(i, vTotPhyStkCases, strCases)
                '================================
                strCases = ""
                If Val(.GetData(i, vTotSysStkCase)) <> 0 Then
                    If Val(.GetData(i, vTotSysStkCase)) > 1 Then
                        strCases = Val(.GetData(i, vTotSysStkCase)) & " Cases"
                    Else
                        strCases = Val(.GetData(i, vTotSysStkCase)) & " Case"
                    End If
                End If
                If Val(.GetData(i, vTotSysStkLosBot)) <> 0 Then
                    If Val(.GetData(i, vTotSysStkLosBot)) > 1 Then
                        strCases = strCases & "," & Val(.GetData(i, vTotSysStkLosBot)) & " Bottles"
                    Else
                        strCases = strCases & "," & Val(.GetData(i, vTotSysStkLosBot)) & " Bottle"
                    End If
                End If
                If Strings.Left(strCases, 1) = "," Then strCases = Replace(strCases, ",", "")
                .SetData(i, vTotSYSStkCases, strCases)
            Next

            .Tree.Column = 0
            .SubtotalPosition = C1.Win.C1FlexGrid.SubtotalPositionEnum.BelowData

            .Subtotal(C1.Win.C1FlexGrid.AggregateEnum.Sum, 0, vTotGroup, vTotPhyStock, "{0} Total Stock")
            .Subtotal(C1.Win.C1FlexGrid.AggregateEnum.Sum, 0, vTotGroup, vTotSysStock, "{0} Total Stock")
            .Subtotal(C1.Win.C1FlexGrid.AggregateEnum.Sum, 0, vTotGroup, vTotDiff, "{0} Total Stock")

            .Cols(vTotPhyStkLtrs).Format = "0.000"
            .Cols(vTotSysStkLtrs).Format = "0.000"
        End With
        fraSummary.Visible = True
        fraSummary.Dock = DockStyle.Fill
        initFormVsfDet(Me, Me.Text, vsfTotal, "TOTAL")
    End Sub

    Private Sub btnCloseSumm_Click(sender As Object, e As EventArgs) Handles btnCloseSumm.Click
        fraSummary.Visible = False
    End Sub

    Private Sub btnExpToXlSumm_Click(sender As Object, e As EventArgs) Handles btnExpToXlSumm.Click
        SetHideCol(vsfTotal, False, vTotPhyStkCase, vTotSysStkCase, vTotPhyStkLosBot, vTotSysStkLosBot)
        ExportToXL(vsfTotal, argHdr1:=gSettings.gCompName, argHdr2:="Physical Stock Summary Report as on " & getSystemDate())
        SetHideCol(vsfTotal, True, vTotKey, vTotPhyStkCase, vTotSysStkCase, vTotPhyStkLosBot, vTotSysStkLosBot)
    End Sub

    Private Sub vsfDet_KeyUp(sender As Object, e As KeyEventArgs) Handles vsfDet.KeyUp
        If e.KeyCode = Keys.C And e.Control = True Then
            vsfCopyItemCode(vsfDet, vsfDet.Row, vsfDet.Col)
        End If
    End Sub

    Private Sub updateSales(argTrn As OleDb.OleDbTransaction)
        Dim strSQL As String = "", intMaxDocID As Long
        Dim strTNo As String, sngCases As Single
        strTNo = getInvNo(argTrn)

        strSQL = "Insert into TrnHdrSInv (FINYEAR, SINVNO, InvCNo, SCID, BILLSYSTEM, SPID, SINVDate, SINVDATETIME,"
        strSQL = strSQL & "  CUSTID, PartyName, PartyCMob, "
        strSQL = strSQL & "  AddedBy, AddedOn) "
        strSQL = strSQL & " output inserted.SInvID into @testTable values ("
        strSQL = strSQL & gSettings.gCompFinYear & ","
        strSQL = strSQL & "'" & strTNo & "',"
        strSQL = strSQL & lngInvCNO & ","
        strSQL = strSQL & "0,"
        strSQL = strSQL & "'" & gStrSystemName & "',"
        strSQL = strSQL & "0,"
        strSQL = strSQL & "'" & getDateFormatInInsert(getSystemDate(argTrn), False) & "', "
        strSQL = strSQL & "'" & getDateFormatInInsert(getSystemDate(argTrn), True, True) & "', "
        strSQL = strSQL & "0,"
        strSQL = strSQL & "'',"
        strSQL = strSQL & "'',"
        strSQL = strSQL & "'" & gUserCode & "', "
        strSQL = strSQL & "'" & getDateFormatInInsert(getSystemDateTime(argTrn), True, True) & "')"
        Try
            intMaxDocID = getMaxInsertID(strSQL, "TrnHdrSInv", argTrn)
            If intMaxDocID = -1 Then
                'argTrn.Rollback()
                MsgBox("Error during invoice saving.." & strSQL, MsgBoxStyle.Critical)
                Exit Sub
            End If

            'Insert detail
            With vsfDet
                For i = 1 To .Rows.Count - 1
                    If GetVal(.GetData(i, vDetDiff)) < 0 Then
                        .SetData(i, vDetStkDedNo, strTNo)
                        strSQL = "Insert into TrnDtlSInv (SINVID,SlNo,ITEMID,UOMID,ItemSlNO,SinvQty,SInvRate,SinvAmt,STKQTY,SINVQtyInLtrs) values ("
                        strSQL = strSQL & intMaxDocID & ","
                        strSQL = strSQL & i & ","
                        strSQL = strSQL & Val(.GetData(i, vDetItemID)) & ","
                        strSQL = strSQL & "1,"
                        strSQL = strSQL & i & ","
                        strSQL &= Math.Abs(Val(GetVal(.GetData(i, vDetDiff), 0))) & ","
                        strSQL &= clsSVT.getDataLongValue("Select MRP From MstItem i Where ItemID = " & GetVal(.GetData(i, vDetItemID), 0), 0, False, argTrn) & ","
                        strSQL &= Math.Abs(Val(GetVal(.GetData(i, vDetDiff), 0))) * clsSVT.getDataLongValue("Select MRP From MstItem i Where ItemID = " & GetVal(.GetData(i, vDetItemID), 0), 0, False, argTrn) & ","
                        strSQL &= Math.Abs(Val(GetVal(.GetData(i, vDetDiff), 0))) & ","
                        sngCases = clsSVT.getDataLongValue("Select LtrPerBot From MstPkg p, MstItem i Where i.PkgID = i.PkgID And ItemID = " & GetVal(.GetData(i, vDetItemID), 0), 0, False, argTrn)
                        strSQL &= Math.Abs(Val(GetVal(.GetData(i, vDetDiff), 0))) * sngCases & ")"
                        If Not clsSVT.execSQLStatement(strSQL, argTrn) Then
                            MsgBox("Error during save -- " & strSQL, MsgBoxStyle.Critical, Me.Text)
                            'argTrn.Rollback()
                            Exit Sub
                        End If
                    End If
                Next
            End With
            strSQL = "Update TrnHdrSInv set InvValue = (select sum(SinvAMt) from trndtlsinv d where d.sinvid = trnhdrsinv.sinvid) where trnhdrsinv.sinvid = " & intMaxDocID
            If Not clsSVT.execSQLStatement(strSQL, argTrn) Then
                MsgBox("Error during save -- " & strSQL, MsgBoxStyle.Critical, Me.Text)
                'argTrn.Rollback()
                Exit Sub
            End If

            strSQL = " Insert Into UtlStockList "
            strSQL &= " (ItemID, StkMultiplier, DocType, DocDate, DocID, DocNo, Particulars, Qty, LtrQty, Rate, LocID) "
            strSQL &= "  Select ItemID, '-1', 'SInv' , SInvDate, h.SInvID, SInvNO, "
            strSQL &= " 'Auto from Sales' , SInvQty, SInvQTYINLTRS,SInvRate, 1  "
            strSQL &= " From TrnDtlSInv d, TrnHdrSInv h "
            strSQL &= " Where d.SInvID = h.SInvID "
            strSQL &= " and d.SInvID = " & intMaxDocID
            Call clsSVT.execSQLStatement(strSQL, argTrn)

        Catch ex As Exception
            MsgBox("Error during save -- " & ex.Message, MsgBoxStyle.Critical, Me.Text)
            'argTrn.Rollback()
        End Try
    End Sub

    Private Function getInvNo(argTrn As OleDb.OleDbTransaction) As String
        Dim strSQL As String, strPfx As String = "", strFldValue As String
        getInvNo = ""
        strSQL = "Select max(invCNo) + 1 from TrnHdrSInv"
        strSQL = strSQL & " where FinYear = " & gSettings.gCompFinYear
        lngInvCNO = clsSVT.getDataLongValue(strSQL, 1, False, argTrn)

        strFldValue = Strings.Right(gSettings.gCompStDt, 2) & Format(lngInvCNO, "0000000")
        getInvNo = strFldValue
    End Function
End Class