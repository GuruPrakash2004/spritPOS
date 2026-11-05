Imports System.Data.OleDb
Imports C1.Win.C1FlexGrid
Public Class frmMstCreateFinYear
    Dim svtTrn As OleDb.OleDbTransaction
    Dim clsSvt As New svtLibrary

    Const vRIItemGrpName = 0
    Const vRIItemCode = 1
    Const vRIItemName = 2
    Const vRIUOM = 3
    Const vRIMRP = 4
    Const vRIOPQty = 5
    Const vRIOPValue = 6
    Const vRIRcptQty = 7
    Const vRIRcptValue = 8
    Const vRIIssQty = 9
    Const vRIIssValue = 10
    Const vRIClQty = 11
    Const vRIClValue = 12
    Const vRIClRate = 13
    Const vRIInvDate = 14
    Const vRISupplier = 15
    Const vRITotCols = 16

    Const vDetLocID = 0
    Const vDetLocName = 1
    Const vDetItemID = 2
    Const vDetItemCode = 3
    Const vDetItemName = 4
    Const vDetItemDesc = 5
    Const vDetUOM = 6
    Const vDetBalQty = 7
    Const vDetRate = 8
    Const vDetValue = 9
    Const vDetTotCols = 10
    Private Sub frmMstCreateFinYear_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        dtCurDate.Value = getSystemDate()
        dtLFYStDate.Value = clsSvt.getDataDateValue("Select Max(YrFDate) From MstCompFinYear", Nothing, False)
        dtLFYEndDate.Value = clsSvt.getDataDateValue("Select Max(YrTDate) From MstCompFinYear", Nothing, False)
        dtCFYStDate.Value = DateAdd(DateInterval.Day, 1, dtLFYEndDate.Value)
        dtCFYEndDate.Value = DateAdd(DateInterval.Month, 12, dtLFYEndDate.Value)
        dtCFYEndDate.Value = getLastDateInMonth(dtCFYEndDate.Value)
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub btnCreate_Click(sender As Object, e As EventArgs) Handles btnCreate.Click

        If MsgBox("Do you want to create new Fin Year?", MsgBoxStyle.YesNo, Me.Text) = MsgBoxResult.No Then
            Me.Close()
            Exit Sub
        End If

        Dim dtLastFinYearDate As Date = clsSvt.getDataDateValue("Select Max(YrTDate) From MstCompFinYear", Nothing, False)
        If dtCurDate.Value < dtLastFinYearDate Then
            MsgBox("Already FinYear Created...", MsgBoxStyle.Exclamation, Me.Text)
            Me.Close()
            Exit Sub
        End If

        If clsSvt.getDataStringValue("Select FldBlnFinYrUpdate From UtlMisc", "", False) = "Y" Then
            MsgBox("FinYear Activity is going on other system...", MsgBoxStyle.Exclamation, Me.Text)
            Me.Close()
            Exit Sub
        End If

        'If Not FillStockData() Then
        '    MsgBox("Stock Backup not done...", MsgBoxStyle.Exclamation, Me.Text)
        '    Me.Close()
        '    Exit Sub
        'End If

        Call showWaitCursor()

        clsSvt.execSQLStatement("Update UtlMisc Set FldBlnFinYrUpdate = 'Y'")

        svtTrn = svtConn.BeginTransaction(IsolationLevel.ReadCommitted)
        Try
            Dim strSQL As String = ""

            If Not clsSvt.execSQLStatement("Update MstCompFinYear Set Active = 'N'", svtTrn) Then
                svtTrn.Rollback()
                GoTo exitQuery
            End If

            strSQL = "Insert into MstCompFinYear "
            strSQL = strSQL & "(yrFdate,yrTdate,Active) values ("
            strSQL = strSQL & "'" & Format(dtCFYStDate.Value, "dd MMM yyyy") & "',"
            strSQL = strSQL & "'" & Format(dtCFYEndDate.Value, "dd MMM yyyy") & "',"
            strSQL = strSQL & "'Y'" & ")"
            If Not clsSvt.execSQLStatement(strSQL, svtTrn) Then
                svtTrn.Rollback()
                GoTo exitQuery
            End If

            'If StkCarryForward(svtTrn) Then
            '    svtTrn.Rollback()
            '    GoTo exitQuery
            'End If

            svtTrn.Commit()
        Catch ex As Exception
            svtTrn.Rollback()
            MsgBox(ex.Message, MsgBoxStyle.Information, Me.Text)
        Finally
            Cursor.Current = Cursors.Arrow
        End Try
exitQuery:
        clsSvt.execSQLStatement("Update UtlMisc Set FldBlnFinYrUpdate = 'N'")
        Me.Close()
        End
    End Sub

    Private Function StkCarryForward(ByVal argSvtTrn As OleDb.OleDbTransaction) As Boolean
        StkCarryForward = False

        Dim strSql As String, strErrMsg As String = ""
        Dim intRecID As Long = 0
        Dim intFinYr As Int32 = Val(Strings.Right(DatePart(DateInterval.Year, dtCFYStDate.Value), 2))

        Try
            strSql = "Delete From MstOPBalance Where FinYear = " & intFinYr
            If Not clsSvt.execSQLStatement(strSql, argSvtTrn) Then
                Exit Function
            End If

            strSql = "Update UtlStockList Set DelFlag = 'Y' Where DocType = 'OP' and DocID = " & intFinYr
            If Not clsSvt.execSQLStatement(strSql, argSvtTrn) Then
                Exit Function
            End If

            With vsfDet
                For i As Int32 = 1 To .Rows.Count - 1

                    strSql = "Insert Into MstOPBalance "
                    strSql = strSql & " (FinYear, ItemID, LocID, RcptDate, RcptRef, OpQty, OPRate, Remarks)"
                    strSql = strSql & " Values ( "
                    strSql = strSql & intFinYr & ","
                    strSql = strSql & Val(.GetData(i, vDetItemID)) & ", "
                    strSql = strSql & Val(.GetData(i, vDetLocID)) & ", "
                    strSql = strSql & "'" & getDateFormatInInsert(dtCFYStDate.Value) & "', "
                    strSql = strSql & "'Stock Carry Forward From Previous Fin Year', "
                    strSql = strSql & Val(.GetData(i, vDetBalQty)) & ","
                    strSql = strSql & Val(.GetData(i, vDetRate)) & ","
                    strSql = strSql & "'Stock Carry Forward From Previous Fin Year')"
                    If Not clsSvt.execSQLStatement(strSql, argSvtTrn) Then
                        Exit Function
                    End If


                    strSql = " Insert Into UtlStockList "
                    strSql &= " (UTDocID, DocID, DocName, DocType, DocNo, "
                    strSql &= " DocDate, ItemID, ItemSlNo, LocID, StkMultiplier, Qty, Rate, particulars, ValuationReq, StkRate) "
                    strSql &= " values( "
                    strSql &= " 0,"
                    strSql &= intFinYr & ","
                    strSql &= "'Opening Balance',"
                    strSql &= "'OP',"
                    strSql &= "'" & i & "',"
                    strSql &= "'" & getDateFormatInInsert(dtCFYStDate.Value) & "', "
                    strSql &= Val(.GetData(i, vDetItemID)) & ", "
                    strSql &= "" & i & ","
                    strSql &= Val(.GetData(i, vDetLocID)) & ", "
                    strSql &= " 1,"
                    strSql &= Val(.GetData(i, vDetBalQty)) & ","
                    strSql &= Val(.GetData(i, vDetRate)) & ","
                    strSql &= "'Stock Carry Forward From Previous Fin Year',"
                    strSql &= "'N',"
                    strSql &= Val(.GetData(i, vDetRate)) & ")"
                    If Not clsSvt.execSQLStatement(strSql, argSvtTrn) Then
                        Exit Function
                    End If
                Next

                strSql = "Delete From UtlStockList Where DelFlag = 'Y' and  DocType = 'OP' and DocID = " & intFinYr
                If Not clsSvt.execSQLStatement(strSql, argSvtTrn) Then
                    Exit Function
                End If

                If Not clsSvt.execSQLStatement("Truncate Table TrnQOH", argSvtTrn) Then
                    Exit Function
                End If

                strSql = " Insert Into TrnQOH (ItemID, LocID, CLBal) "
                strSql &= " Select ItemID, LocID, Sum(StkMultiplier * Qty)  "
                strSql &= " From UtlStockList "
                strSql &= " Where DocDate >= '" & getDateFormatInInsert(dtCFYStDate.Value) & "' "
                strSql &= " Group by ItemID, LocID having Sum(StkMultiplier * Qty) > 0 "
                If Not clsSvt.execSQLStatement(strSql, argSvtTrn) Then
                    Exit Function
                End If

                StkCarryForward = True
            End With
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, Me.Text)
        End Try
    End Function

    Private Function FillStockData() As Boolean
        FillStockData = True
        'Dim vsfTemp As New C1FlexGrid
        'Dim rs As OleDb.OleDbDataReader
        'Dim intCtr As Integer, intStartRow As Integer, intLocId As Integer
        'Try
        '    vsfDet.Redraw = False
        '    Dim strLocList As String = ""
        '    rs = clsSvt.getDataReaderFromSQL("select LocName from MstLoc where Inactive='N' and LOCID>0")
        '    Do While rs.Read
        '        strLocList &= rs!locName & ","
        '    Loop
        '    rs.Close()
        '    strLocList = Mid(strLocList, 1, Len(strLocList) - 1)

        '    vsfDet.Rows.Count = 1
        '    strLocList = Replace(strLocList, "'", "")
        '    intCtr = 1
        '    intStartRow = 0

        '    For Each strloc As String In strLocList.Split(",")
        '        Dim obj As New frmRptStockBal
        '        vsfTemp = obj.GetStockDataForOPBal(dtpFrom.Value, dtpTo.Value, strloc)
        '        intLocId = clsSvt.getDataLongValue("Select LOCID from MstLoc where LocName='" & strloc & "'", 0, False)
        '        With vsfTemp
        '            For i As Int32 = 1 To .Rows.Count - 2
        '                vsfDet.Rows.Count = vsfDet.Rows.Count + 1
        '                vsfDet.Row = vsfDet.Rows.Count - 1
        '                vsfDet.TopRow = vsfDet.Row
        '                vsfDet.SetData(vsfDet.Rows.Count - 1, vDetLocID, intLocId)
        '                vsfDet.SetData(vsfDet.Rows.Count - 1, vDetLocName, strloc)
        '                vsfDet.SetData(vsfDet.Rows.Count - 1, vDetItemID, clsSvt.getDataLongValue("Select ItemID from MstItem where ItemCode='" & vsfTemp.GetData(i, vRIItemCode) & "'", 0, False))
        '                vsfDet.SetData(vsfDet.Rows.Count - 1, vDetItemCode, vsfTemp.GetData(i, vRIItemCode))
        '                vsfDet.SetData(vsfDet.Rows.Count - 1, vDetItemName, vsfTemp.GetData(i, vRIItemName))
        '                vsfDet.SetData(vsfDet.Rows.Count - 1, vDetItemDesc, "")
        '                vsfDet.SetData(vsfDet.Rows.Count - 1, vDetUOM, vsfTemp.GetData(i, vRIUOM))
        '                vsfDet.SetData(vsfDet.Rows.Count - 1, vDetBalQty, vsfTemp.GetData(i, vRIClQty))
        '                vsfDet.SetData(vsfDet.Rows.Count - 1, vDetRate, Math.Round(Math.Abs(vsfTemp.GetData(i, vRIClRate)), 2))
        '                vsfDet.SetData(vsfDet.Rows.Count - 1, vDetValue, vsfTemp.GetData(i, vRIClValue))
        '            Next
        '        End With
        '        obj.Dispose()
        '        obj.Close()
        '    Next
        '    StMsg("")
        '    vsfDet.Redraw = True
        '    vsfDet.AutoSizeCols()
        '    showNormalCursor()

        '    SetFlexColorFullGrid(vsfDet, "")

        '    FillStockData = True

        'Catch ex As Exception
        '    MsgBox("Error loading Data!" & ex.Message, MsgBoxStyle.Critical, Me.Text)
        'End Try
    End Function
End Class