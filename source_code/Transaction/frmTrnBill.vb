Imports System.ComponentModel
Imports C1.Win.C1FlexGrid

Public Class frmTrnBill
    Public strCancelRemarks As String

    Public strType As String

    Const vDetSel = 0
    Const vDetBillId = 1
    Const vDetBillNo = 2
    Const vDetBillDate = 3
    Const vDetBillAmt = 4
    Const vDetSalesPerson = 5
    'ADded new column Counter by Surabhi
    Const vDetCounter = 6
    Const vDetParty = 9
    Const vDetCancelled = 7
    Const vDetDelete = 8

    Const intDetCols = 10
    Dim dt As DataTable

    Dim clsSvt As New svtLibrary
    Dim tooltip As New ToolTip
    Public intSalesCounterID As Integer
    Public strCboText As String

    Public frmInv As Object
    Dim intDocID As Int32
    Dim svtTrn As oledb.oledbTransaction
    Public strFrmName As String

    Public blnPDFPreview As Boolean = False
    Public strRights As String = ""
    Public intTopRows As Integer
    Public blnAuditTrack As Boolean = False
    Public strInvAuditHdrTbl As String = ""
    Public strInvAuditHdrSql As String = ""
    Public strInvAuditDtlSql As String = ""

    Private Sub frmTrnBill_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Call initFormVsfDet(Me, Me.Text, vsfDet, "DET")
        Call fillFormProperties(Me)
        Call initGridStyles(Me)
        Call initscreen()
        Call FillDtlFlds()
        txtSearch.Focus()

        cboPrintFormat.Text = INIGet("Print Format", "PrintName", "")
        If cboPrintFormat.Text = "" Then
            cboPrintFormat.Text = "FMTSINV"
        End If

    End Sub

    Private Sub initscreen()

        intTopRows = getGsettings("Invoice", "InvreprintTopRows", "Y")



        intDocID = clsSvt.getDataLongValue("Select DocID From UtlTransactions Where DocName='Sales Invoice'", 0, False)
        Select Case strType
            Case "D"
                If gStrDBName.ToUpper() = "VLQMSDROPSJP" Then
                    btnGeneric.Visible = False
                    Exit Select
                End If
                Me.Text = "Delete Bill"
                btnGeneric.Text = "&Delete Bill"
                tooltip.SetToolTip(Me.btnGeneric, "Delete Bill : Alt + D")
            Case "C"
                Me.Text = "Cancel Bill"
                btnGeneric.Text = "&Cancel Bill"
                tooltip.SetToolTip(Me.btnGeneric, "Cancel Bill : Alt + C")
            Case "R"
                Me.Text = "Re-Print Bill"
                btnGeneric.Text = "&Re-Print Bill"
                tooltip.SetToolTip(Me.btnGeneric, "Re-Print Bill : Alt + R")
                btnPrintPre.Visible = False
                lblPrintFormat.Visible = True
                cboPrintFormat.Visible = True
        End Select

        With vsfDet
            .Rows.Fixed = 1
            .Cols.Count = intDetCols
            .Rows.Count = 1
            .SetData(0, vDetSel, "Sel")
            .SetData(0, vDetBillId, "BillId")
            .SetData(0, vDetBillNo, "Bill No")
            .SetData(0, vDetBillDate, "Bill Date")
            .SetData(0, vDetBillAmt, "Bill Amt")
            .SetData(0, vDetSalesPerson, "Sales Person")
            .SetData(0, vDetCounter, "Counter")
            .SetData(0, vDetParty, "Party")
            .SetData(0, vDetCancelled, "Cancelled")
            .SetData(0, vDetDelete, "Delete")
            .Cols(vDetSel).DataType = GetType(Boolean)

            If strType = "R" Then
                Call SetHideCol(vsfDet, True, vDetSel)
            Else
                Call SetHideCol(vsfDet, True, vDetCancelled, vDetDelete)
            End If

            Call SetHideCol(vsfDet, True, vDetBillId)
            Call SetFlexColor(vsfDet, "", vDetBillId, vDetBillNo, vDetBillDate, vDetBillAmt, vDetSalesPerson, vDetParty, vDetCancelled, vDetDelete, vDetCounter)
        End With

        Dim dir = gstrRptPath
        cboPrintFormat.Items.Clear()

        Try
            For Each foundFile As String In My.Computer.FileSystem.GetFiles(dir,
    Microsoft.VisualBasic.FileIO.SearchOption.SearchAllSubDirectories, "FMTSINV*.rpt")
                cboPrintFormat.Items.Add(System.IO.Path.GetFileNameWithoutExtension(foundFile))
            Next
        Catch ex As Exception

        End Try

        cboPrintFormat.Sorted = True

    End Sub

    Private Sub FillDtlFlds()
        Dim rs As oledb.oledbDataReader
        Dim intRow As Int32
        Dim strSQL As String = ""

        strSQL = " Select Top " & intTopRows & "SINVID, SINVNO, SINVDATE, INVVALUE, "
        strSQL &= " isnull((Select top 1 SPCode from MstSP s where s.SPID = h.SPID),'') as SPCode, "
        strSQL &= " isnull((Select top 1 SCCode from MstSC s where s.SCID = h.SCID),'') as SCCode, "
        strSQL &= " PartyName, Cancelled, isDelete "
        strSQL &= " From TrnHdrSinv h"
        strSQL &= " Where 1=1 And "

        strSQL &= " SINVDate = " & getDateFormatInCond(getSystemDate()) & ""

        If Not gUserCode = "ADMIN" And strType = "R" Then
            strSQL &= " And SCID = '" & intSalesCounterID & "'"
        End If
        If strType <> "R" Then
            strSQL &= " And Cancelled <> 'Y'"
            strSQL &= " And isDelete <> 'Y'"
        End If
        strSQL &= " Order by SINVID Desc"

        rs = clsSvt.getDataReaderFromSQL(strSQL)
        vsfDet.Rows.Count = 1
        If Not rs.HasRows Then
            rs.Close()
            Exit Sub
        End If

        With rs
            Do While rs.Read
                vsfDet.Rows.Count += 1
                intRow = vsfDet.Rows.Count - 1
                vsfDet.SetData(intRow, vDetBillId, !SINVID)
                vsfDet.SetData(intRow, vDetBillNo, !SINVNO)
                vsfDet.SetData(intRow, vDetBillDate, !SINVDATE)
                vsfDet.SetData(intRow, vDetBillAmt, !INVVALUE)
                vsfDet.SetData(intRow, vDetCounter, !SCCode)
                vsfDet.SetData(intRow, vDetSalesPerson, !SPCode)
                vsfDet.SetData(intRow, vDetParty, !PARTYNAME)
                vsfDet.SetData(intRow, vDetCancelled, !Cancelled)
                vsfDet.SetData(intRow, vDetDelete, !isDelete)
            Loop
        End With
        rs.Close()
    End Sub

    Private Sub vsfDet_BeforeEdit(sender As Object, e As RowColEventArgs) Handles vsfDet.BeforeEdit
        Select Case e.Col
            Case vDetBillId, vDetBillNo, vDetBillDate, vDetBillAmt, vDetSalesPerson, vDetParty, vDetCancelled, vDetDelete, vDetCounter
                e.Cancel = True
        End Select
    End Sub

    Private Sub txtSearch_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtSearch.KeyPress
        If e.KeyChar = "'" Then
            e.Handled = True
        End If
    End Sub

    Private Sub txtSearch_KeyUp(sender As Object, e As KeyEventArgs) Handles txtSearch.KeyUp
        Dim strSQL As String = ""
        Dim intRow As Int32
        Dim strSC As String = ""
        Dim rs As oledb.oledbDataReader

        If e.KeyCode = Keys.Enter Then
            If txtSearch.Text = "" Then Exit Sub
            If txtSearch.Text <> "" Then txtSearch.Focus()
        End If

        strSQL = " Select Top " & intTopRows & "SINVID, SINVNO, SINVDATE, INVVALUE, "
        strSQL &= " isnull((Select top 1 SPCode from MstSP s where s.SPID = h.SPID),'') as SPCode, "
        strSQL &= " isnull((Select top 1 SCCode from MstSC s where s.SCID = h.SCID),'') as SCCode, "
        strSQL &= " PartyName, Cancelled, isDelete "
        strSQL &= " From TrnHdrSinv h"
        strSQL &= " Where 1=1  "
        'strSQL &= " and SINVDate = " & getDateFormatInCond(getSystemDate()) & ""
        strSQL &= " And SINVNO Like '%" & txtSearch.Text & "%'"
        If Not gUserCode = "ADMIN" And strType = "R" Then
            strSQL &= " And SCID = '" & intSalesCounterID & "'"
        End If
        If strType <> "R" Then
            strSQL &= " And Cancelled <> 'Y'"
            strSQL &= " And isDelete <> 'Y'"
        End If
        strSQL &= " Order by SINVID Desc"

        vsfDet.Rows.Count = 1
        rs = clsSvt.getDataReaderFromSQL(strSQL)
        If Not rs.HasRows Then
            rs.Close()
            Exit Sub
        End If

        With rs
            Do While rs.Read
                vsfDet.Rows.Count += 1
                intRow = vsfDet.Rows.Count - 1
                vsfDet.SetData(intRow, vDetBillId, !SINVID)
                vsfDet.SetData(intRow, vDetBillNo, !SINVNO)
                vsfDet.SetData(intRow, vDetBillDate, !SINVDATE)
                vsfDet.SetData(intRow, vDetBillAmt, !INVVALUE)
                vsfDet.SetData(intRow, vDetSalesPerson, !SPCode)
                vsfDet.SetData(intRow, vDetCounter, !SCCode)
                vsfDet.SetData(intRow, vDetParty, !PARTYNAME)
                vsfDet.SetData(intRow, vDetCancelled, !Cancelled)
                vsfDet.SetData(intRow, vDetDelete, !isDelete)
            Loop
        End With
        rs.Close()
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
        frmInv.txtScan.Focus()
    End Sub

    Private Sub btnGeneric_Click(sender As Object, e As EventArgs) Handles btnGeneric.Click
        Select Case strType
            Case "D"
                If Not chkUserRights(strRights, chkRightsPos.DelRights, Me.Text) Then Exit Sub
                Call deleteBill()
            Case "C"
                If Not chkUserRights(strRights, chkRightsPos.DelRights, Me.Text) Then Exit Sub
                Call cancelBill()
            Case "R"
                If Not chkUserRights(strRights, chkRightsPos.PrintRights, Me.Text) Then Exit Sub
                Call rePrintBill()
        End Select
    End Sub

    Private Sub deleteBill()
        'svtTrn = svtConn.BeginTransaction(IsolationLevel.ReadCommitted)
        svtTrn = Nothing
        Try
            With vsfDet

                'the below code will not allow to delete if the doc date is less than or equal to freeze date in Gsettings
                For i As Int32 = 1 To .Rows.Count - 1
                    If .GetData(i, vDetSel) = True Then
                        If CDate(.GetData(i, vDetBillDate)) <= gdtFreezeDate Then
                            MsgBox("Documents cannot be Deleted. Some of the Document dates are before Freezed Date (Global Settings)!!", MsgBoxStyle.Critical, Me.Text)
                            Exit Sub
                        End If
                    End If
                Next

                Dim strSQL As String = ""
                For i As Int32 = 1 To .Rows.Count - 1
                    If .GetData(i, vDetSel) = True Then

                        Dim intMaxDocID As Int32 = Val(.GetData(i, vDetBillId))

                        clsSvt.execSQLStatement("Update TrnHdrSinv Set isDelete = 'Y' Where SINVID = " & intMaxDocID, svtTrn)
                        If gBlnSMSInvDeleted Then
                            Dim strSMS As String = getSMS(strType, intMaxDocID)
                            If strSMS <> "" Then Call SendSMS(strSMS, gStrSalesMobNo, "", svtTrn)
                        End If

                        'for audit track
                        If blnAuditTrack Then
                            Call RecordAuditLog("Invoice Deletion", intMaxDocID, UserActions.DeleteAction, strInvAuditHdrTbl, strInvAuditHdrSql, strInvAuditDtlSql, svtTrn)
                        End If

                        strSQL = "Insert into UtlAdmAudit (ScreenName,DAction,DocDate,DocDateTime,DocSysName,SysUserName,MainFldID,MainFld, EarlierData,ChangedData) values ("
                        strSQL &= "'Invoice',"
                        strSQL &= "'Deleted',"
                        strSQL &= "'" & getDateFormatInInsert(getSystemDate(svtTrn)) & "',"
                        strSQL &= "'" & getDateFormatInInsert(getSystemDateTime(svtTrn), True) & "',"
                        strSQL &= "'" & gStrSystemName & "',"
                        strSQL &= "'" & gStrSystemUserName & "',"
                        strSQL &= "'" & intMaxDocID & "',"
                        strSQL &= "'" & .GetData(i, vDetBillNo) & "',"
                        strSQL &= "'Invice Value: " & .GetData(i, vDetBillAmt).ToString & "',"
                        strSQL &= "'')"
                        Call clsSvt.execSQLStatement(strSQL, svtTrn)

                        strSQL = " Delete From UtlStockList Where DocType='INV' and DocID=" & intMaxDocID
                        clsSvt.execSQLStatement(strSQL, svtTrn)
                        Dim strErrMsg As String = clsSvt.execSQLStmtMsg(strSQL, svtTrn)
                        If strErrMsg <> "" Then
                            If InStr(UCase(strErrMsg), "CHECK CONSTRAINT") > 0 Then
                                MsgBox("Stock Is going negative. Cannot Save.", MsgBoxStyle.Critical, "No Stock")
                            Else
                                MsgBox(strErrMsg & " Error during updating stock", MsgBoxStyle.Critical, "No Stock")
                            End If
                            'svtTrn.Rollback()
                            Exit Sub
                        End If
                    End If
                Next
                'svtTrn.Commit()
                Call FillDtlFlds()



                If UCase(gUserCode) <> "ADMIN" Then
                    If getGsettings("INVOICE", "SHOWSALESVALUE", "N") = "Y" Then
                        Dim dblTotalSales As Double = clsSvt.getDataDoubleValue("Select Sum(InvValue) From TrnHdrSINV Where IsDelete = 'N' and SINVDate='" & getDateFormatInInsert(getSystemDate()) & "'", 0, False)
                        Dim intTotalBills As Integer = clsSvt.getDataDoubleValue("Select Count(InvValue) From TrnHdrSINV Where IsDelete = 'N' and SINVDate='" & getDateFormatInInsert(getSystemDate()) & "'", 0, False)
                        frmMain.lblSalesValue.Text = "Total Bills: " & intTotalBills & " & Sales: " & dblTotalSales
                    End If
                Else
                    Dim dblTotalSales As Double = clsSvt.getDataDoubleValue("Select Sum(InvValue) From TrnHdrSINV Where IsDelete = 'N' and SINVDate='" & getDateFormatInInsert(getSystemDate()) & "'", 0, False)
                    Dim intTotalBills As Integer = clsSvt.getDataDoubleValue("Select Count(InvValue) From TrnHdrSINV Where IsDelete = 'N' and SINVDate='" & getDateFormatInInsert(getSystemDate()) & "'", 0, False)
                    frmMain.lblSalesValue.Text = "Total Bills: " & intTotalBills & " & Sales: " & dblTotalSales
                End If

            End With
        Catch ex As Exception
            'svtTrn.Rollback()
            MsgBox(ex.Message, MsgBoxStyle.Exclamation, Me.Text)
        End Try
    End Sub

    Private Sub cancelBill()
        'svtTrn = svtConn.BeginTransaction(IsolationLevel.ReadCommitted)
        strCancelRemarks = ""
        Dim frmCRmks As New frmUtlCancelInvRmks
        frmCRmks.argFrm = Me
        frmCRmks.ShowDialog()
        If strCancelRemarks = "" Then
            ' MsgBox("Remarks for cancelling invoices not entered!", MsgBoxStyle.Critical, Me.Text)
            Exit Sub
        End If

        svtTrn = Nothing
        Try
            Dim strSQL As String = ""
            With vsfDet
                For i As Int32 = 1 To .Rows.Count - 1
                    If .GetData(i, vDetSel) = True Then
                        Dim intMaxDocID As Int32 = Val(.GetData(i, vDetBillId))
                        clsSvt.execSQLStatement("Update TrnHdrSinv Set Cancelled = 'Y', CancelRmks = '" & strCancelRemarks & "' Where SINVID = " & intMaxDocID, svtTrn)
                        If gBlnSMSInvCancelled Then
                            Dim strSMS As String = getSMS("CANCELLED", intMaxDocID)
                            If strSMS <> "" Then Call SendSMS(strSMS, gStrSalesMobNo, "", svtTrn)
                        End If

                        strSQL = " Delete From UtlStockList Where DocType='INV' and DocID=" & intMaxDocID
                        clsSvt.execSQLStatement(strSQL, svtTrn)
                        Dim strErrMsg As String = clsSvt.execSQLStmtMsg(strSQL, svtTrn)
                        If strErrMsg <> "" Then
                            If InStr(UCase(strErrMsg), "CHECK CONSTRAINT") > 0 Then
                                MsgBox("Stock Is going negative. Cannot Save.", MsgBoxStyle.Critical, "No Stock")
                            Else
                                MsgBox(strErrMsg & " Error during updating stock", MsgBoxStyle.Critical, "No Stock")
                            End If
                            'svtTrn.Rollback()
                            Exit Sub
                        End If
                    End If
                Next
                'svtTrn.Commit()
                Call FillDtlFlds()
            End With
        Catch ex As Exception
            svtTrn.Rollback()
            MsgBox(ex.Message, MsgBoxStyle.Exclamation, Me.Text)
        End Try
    End Sub

    Private Sub rePrintBill()
        If Not chkUserRights(strRights, chkRightsPos.PrintRights, Me.Text) Then Exit Sub
        Dim intNoofCopies As Integer = 1
        intNoofCopies = clsSvt.getDataLongValue("Select NOOFCOPIES from UtlOption Where DocID = " & intDocID & " and DefaultPrint='Y'", 1, False)
        Call PrintInvCrpt(vsfDet.GetData(vsfDet.Row, vDetBillId), Trim(cboPrintFormat.Text) & ".rpt", True, blnPDFPreview, intNoofCopies)
    End Sub

    Private Sub btnPrintPre_Click(sender As Object, e As EventArgs) Handles btnPrintPre.Click
        If Not chkUserRights(strRights, chkRightsPos.PrintRights, Me.Text) Then Exit Sub
        Call showWaitCursor()
        Call PrintInvCrpt(vsfDet.GetData(vsfDet.Row, vDetBillId), Trim(cboPrintFormat.Text) & ".rpt", True, blnPDFPreview)
        Call INIStore("Print Format", "PrintName", cboPrintFormat.Text)
        Call showNormalCursor()
    End Sub

    Private Sub frmTrnBill_KeyUp(sender As Object, e As KeyEventArgs) Handles Me.KeyUp
        If e.KeyCode = Keys.F9 And e.Control Then
            Call initFormVsfDet(Me, Me.Text, vsfDet, "DET")

            MsgBox("Restored Form & Grid Layout Successfully!!", vbInformation, Me.Text)
            Exit Sub
        End If
        If e.KeyCode = Keys.F8 And e.Control = True Then

            Call SaveActFormVsfDet(Me, Me.Text, "DET", vsfDet)
            MsgBox("Saved Form & Grid Layout Successfully!!", vbInformation, Me.Text)
            Exit Sub
        End If
    End Sub

    Private Function getSMS(argType As String, argBillID As Long) As String
        Dim txttemplate As New TextBox
        getSMS = ""
        Select Case argType
            Case "D"
                txttemplate.Text = gstrDeleteSMSMsg
            Case "CANCELLED"
                txttemplate.Text = gstrCancelledSMSMsg
        End Select

        txttemplate.Multiline = True
        If Trim(txttemplate.Text) <> "" Then
            Dim strSQL As String = getFieldsFromTbl(argBillID)

            Dim dtTbl As DataTable = clsSvt.getDATATable(strSQL)

            For i As Int32 = 0 To dtTbl.Rows.Count - 1
                For j As Int32 = 0 To dtTbl.Columns.Count - 1
                    txttemplate.Text = txttemplate.Text.Replace(UCase("<<" & dtTbl.Columns(j).Caption & ">>"), GetVal(dtTbl.Rows(i).Item(j).ToString(), ""))
                Next
            Next
            getSMS = txttemplate.Text
        End If
    End Function

    Private Function getFieldsFromTbl(ByVal argID As Long) As String
        getFieldsFromTbl = ""
        Dim rsFlds As oledb.oledbDataReader
        Dim blnInclude As Boolean, i As Integer, x As Integer
        Dim blnFound As Boolean
        Dim strFldList As String = "", strFindFld As String
        Dim strCondList As String = "", strTblList As String
        Dim strSubTblCaption As String
        Dim strSQL As String
        Dim strFldName As String, argTblName As String
        argTblName = "TRNHDRSINV"



        strSQL = "Select * from  " & argTblName & " where 1 <> 1"
        rsFlds = clsSvt.getDataReaderFromSQL(strSQL)

        strTblList = argTblName & ","

        With rsFlds
            For i = 0 To .FieldCount - 1
                strFldName = UCase(Trim(.GetName(i)))
                blnInclude = True
                Select Case strFldName
                            'Case "AMMENDMENT", "IMGPRINT", "AMMDETAILS", "AMMNO", "AMMDATE", "AUTHORISED", "AUTHDATE", "AUTHORISEDBY"
                    Case "RGUID", "IMGPRINT"
                        blnInclude = False
                End Select

                If blnInclude Then
                    blnFound = False
                    'list the table name if it is from another table
                    strFindFld = strFldName
                    If InStr(1, strFindFld, "ID") > 0 Then
                        strFindFld = Replace(strFindFld, "ID", "")
                        If "MST" & strFindFld <> argTblName And "TRNHDR" & strFindFld <> argTblName And "TRNDTL" & strFindFld <> argTblName Then
                            '                        'if it is not from the same table MstItem or TrnHdrInv or TrnDtlInv
                            '                        'and if the master exists then 
                            strSubTblCaption = clsSvt.getDataStringValue("select docName from UtlTransactions where HdrTblName = 'MST" & strFindFld & "'", "", blnFound)
                            If blnFound Then
                                If fnTablePresent("MST" & strFindFld) Then
                                    blnFound = False
                                    strTblList &= "MST" & strFindFld & ","
                                    strCondList &= " and " & argTblName & "." & strFldName & " = " & "MST" & strFindFld & "." & strFindFld & "ID"

                                End If
                            End If
                        End If
                    End If
                End If
            Next i
            rsFlds.Close()
        End With

        strTblList = Mid(strTblList, 1, Len(strTblList) - 1)

        strSQL = " SELECT * " '
        strSQL = strSQL & " FROM " & strTblList
        strSQL = strSQL & " WHERE 1=1 " & strCondList
        strSQL = strSQL & " and TrnHdrSInv.SInvID = " & Val(argID)
        getFieldsFromTbl = strSQL
    End Function

    Private Sub btnEdit_Click(sender As Object, e As EventArgs) Handles btnEdit.Click
        If Not chkUserRights(strRights, chkRightsPos.EditRights, Me.Text, True, vsfDet.GetData(vsfDet.Row, vDetBillDate)) Then Exit Sub
        With frmInv
            Call .filldocdetails(vsfDet.GetData(vsfDet.Row, vDetBillId))
        End With
        Me.Close()
    End Sub


End Class