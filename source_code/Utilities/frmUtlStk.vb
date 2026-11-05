Imports C1.Win.C1FlexGrid

Public Class frmUtlStk
    Const vDetDocID = 0
    Const vDetDocOrderBy = 1
    Const vDetDocName = 2
    Const vDetDocValuationReq = 3
    Const vDetStkUpdate = 4
    Const vDetStkBalColName = 5
    Const vDetMain = 6
    Const vDetParticularsFld = 7
    Const vDetQty = 8
    Const vDetValue = 9
    Const vDetItem = 10
    Const vDetLoc = 11
    Const vDetLOTNO = 12
    Const vDetDate = 13
    Const vDetCondition = 14
    Const vDetFinYear = 15
    Const vDetConsiderForStock = 16
    Const vTot = 17

    Dim clsSvt As New svtLibrary
    Dim intLastCol As Integer
    Public argForm As Object
    Public argVSF As C1.Win.C1FlexGrid.C1FlexGrid
    Dim str As String = "Y|N "

    Private Sub frmUtlStk_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Call initScreen()
        Me.StartPosition = FormStartPosition.CenterScreen

        Call ucBtnTools.HideCopyFrom("N")
        Call ucBtnTools.HideAttachment("N")
        Call ucBtnTools.HideAmmend("N")
        Call ucBtnTools.HideMultiDoc("N")
        Call ucBtnTools.HideInsertDoc("N")
        Call ucBtnTools.HideTender("N")
        Call ucBtnTools.HideImpExp("N")
        Call ucBtnTools.HideBOM("N")
        Call ucBtnTools.HideTotal("N")
        Call ucBtnTools.HidePending("N")
        'Call ucBtnTools.hid
        fillDtls()
        Me.KeyPreview = True
    End Sub

    Private Sub initScreen()

        Call initFormSize(Me, Me.Text, False, 816, 489, 0, 0)
        Call initFormVsfDet(Me, Me.Text, vsfDet, "DET")
        'stringReplace(Me)

        initGridStyles(Me)
        fillFormProperties(Me)

        With vsfDet
            .Rows.Count = 2
            .Cols.Count = vTot

            .SetData(0, vDetDocID, "DocID")
            .SetData(0, vDetDocOrderBy, "SlNo")
            .SetData(0, vDetDocName, "Document")
            .SetData(0, vDetDocValuationReq, "Valuation")
            .SetData(0, vDetStkUpdate, "Mode Of Update")
            .SetData(0, vDetStkBalColName, "StkColumn")
            .SetData(0, vDetMain, "MainFld")
            .SetData(0, vDetParticularsFld, "ParticularsFld")
            .SetData(0, vDetQty, "Qty")
            .SetData(0, vDetValue, "Value")
            .SetData(0, vDetItem, "Item")
            .SetData(0, vDetLoc, "Loc")
            .SetData(0, vDetLOTNO, "Batch No")
            .SetData(0, vDetDate, "DateFld")
            .SetData(0, vDetCondition, "Condition")
            .SetData(0, vDetFinYear, "FinYear")
            .SetData(0, vDetConsiderForStock, "Consider For Stock")
            .Cols(vDetDocName).ComboList = GenVSfComboStringCond("DocName", "UtlTransactions", " where DocType='Transaction' and AffectsStock Not in ('','NA')")
            ' .Cols(vDetDocName).ComboList = GenVSfComboStringCond("DocName", "UtlTransactions", " where DocType='Transaction' and AffectsStock Not in ('','NA')")
            .Cols(vDetStkUpdate).ComboList = "1|-1"
            .Cols(vDetDocValuationReq).ComboList = str
            .Cols(vDetFinYear).ComboList = str
            .Cols(vDetConsiderForStock).ComboList = str
        End With
        SetHideCol(vsfDet, True, vDetDocID)
        ReArrangeSlNo(vsfDet, vDetDocOrderBy)
        Call SetFlexColor(vsfDet, "", vDetDocOrderBy)
    End Sub

    Private Sub frmUtlStk_KeyUp(sender As Object, e As KeyEventArgs) Handles Me.KeyUp
        If e.KeyCode = Keys.F9 And e.Control = True Then
            Call initFormSize(Me, Me.Text, True, 816, 489, 0, 0)
            MsgBox("Restored Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
        If e.KeyCode = Keys.F8 And e.Control = True Then
            Call SaveActFormSettings(Me, Me.Text)
            Call SaveActFormVsfDet(Me, Me.Text, "DET", vsfDet)
            MsgBox("Saved Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
    End Sub

    Private Sub btnpnlClose_Click(sender As Object, e As EventArgs) Handles btnpnlClose.Click
        Me.Dispose()
    End Sub

    Private Sub btnExpExcel_Click(sender As Object, e As EventArgs)
        If vsfDet.Rows.Count < 1 Then
            Exit Sub
        End If
        ExportToXL(vsfDet)
    End Sub

    Private Sub vsfDet_BeforeEdit(sender As Object, e As RowColEventArgs) Handles vsfDet.BeforeEdit
        With vsfDet
            Select Case .Col
                Case vDetDocName, vDetDocValuationReq, vDetStkUpdate, vDetStkBalColName, vDetMain, vDetParticularsFld, vDetQty, vDetValue, vDetItem, vDetLoc, vDetLOTNO, vDetDate, vDetCondition, vDetFinYear, vDetConsiderForStock
                Case Else
                    e.Cancel = True
            End Select
        End With
    End Sub

    Private Sub ucBtnTools_btnClicked(btnValue As Integer) Handles ucBtnTools.btnClicked
        With ucBtnTools
            Select Case btnValue
                Case svtDtlBtns.svtInsert
                    Call .vsfInsert(vsfDet, vsfDet.Row, vDetDocName)
                    ReArrangeSlNo(vsfDet, vDetDocOrderBy)
                Case svtDtlBtns.svtRemove
                    Call .vsfRemove(vsfDet, vsfDet.Row, vDetDocName)
                Case svtDtlBtns.svtCopy
                    Call .vsfCopy(vsfDet, vsfDet.Row, vDetDocName)
                Case svtDtlBtns.svtPaste
                    Call .vsfPaste(vsfDet, vsfDet.Row, vDetDocName)
                Case svtDtlBtns.svtUp
                    Call MoveOneRowUp(vsfDet, vDetDocOrderBy)
                    vsfDet.Enabled = True
                Case svtDtlBtns.svtDown
                    Call MoveOneRowDown(vsfDet, vDetDocOrderBy)
                'Case svtDtlBtns.svtReplace
                '    Call .vsfReplace(vsfDet, vsfDet.Row, vsfDet.Col, vDetDocName)
                Case svtDtlBtns.svtExport
                    Call .vsfExport(vsfDet)
            End Select
        End With
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Dim svtTrn As oledb.oledbTransaction = Nothing
        Dim strSQL As String
        Dim i As Int32

        stringReplace(Me)

        With vsfDet
            If .Rows.Count <= 1 Then
                MsgBox("Grid Empty...", MsgBoxStyle.Critical, Me.Text)
                vsfDet.Focus()
                Exit Sub
            End If

            Try
                svtTrn = svtConn.BeginTransaction(IsolationLevel.ReadCommitted)
                strSQL = " Delete from UtlStock "
                clsSvt.execSQLStatement(strSQL, svtTrn)

                For i = 1 To .Rows.Count - 1
                    If Trim(.GetData(i, vDetDocName)) <> "" Then
                        strSQL = "Insert into UtlStock (DocID,DocOrderBy,DocName,DocValuationReq,StkUpdate,StkBalColName,MainFld,ParticularsFld,QtyFld,ValueFld,ItemFld,LocFld,LOTNOFld,DateFld,Condition,FinYear,ConsiderForStock) values("
                        '    strSQL &= Val(txtID.Text) & ","
                        strSQL &= "" & Val(.GetData(i, vDetDocID)) & ","
                        strSQL &= "'" & .GetData(i, vDetDocOrderBy) & "',"
                        strSQL &= "'" & .GetData(i, vDetDocName) & "',"
                        strSQL &= "'" & .GetData(i, vDetDocValuationReq) & "',"
                        strSQL &= "'" & .GetData(i, vDetStkUpdate) & "',"
                        strSQL &= "'" & .GetData(i, vDetStkBalColName) & "',"
                        strSQL &= "'" & .GetData(i, vDetMain) & "',"
                        strSQL &= "'" & .GetData(i, vDetParticularsFld) & "',"
                        strSQL &= "'" & .GetData(i, vDetQty) & "',"
                        strSQL &= "'" & .GetData(i, vDetValue) & "',"
                        strSQL &= "'" & .GetData(i, vDetItem) & "',"
                        strSQL &= "'" & .GetData(i, vDetLoc) & "',"
                        strSQL &= "'" & .GetData(i, vDetLOTNO) & "',"
                        strSQL &= "'" & .GetData(i, vDetDate) & "',"
                        strSQL &= "'" & .GetData(i, vDetCondition) & "',"
                        strSQL &= "'" & .GetData(i, vDetFinYear) & "',"
                        '    strSQL &= "'" & Mid(.GetData(i, vDetType), 1, 1) & "',"
                        strSQL &= "'" & .GetData(i, vDetConsiderForStock) & "') "
                        If Not clsSvt.execSQLStatement(strSQL, svtTrn) Then
                            svtTrn.Rollback()
                            Exit Sub
                        End If
                    End If
                Next
                svtTrn.Commit()
                MsgBox("Data saved successfully.", vbInformation, Me.Text)

            Catch ex As Exception
                MsgBox("Error during Save" & vbCrLf & ex.Message, MsgBoxStyle.Critical, Me.Text)
                svtTrn.Rollback()
            End Try
        End With
    End Sub

    Private Sub vsfDet_AfterEdit(sender As Object, e As RowColEventArgs) Handles vsfDet.AfterEdit
        If e.Col = vDetDocName Then
            vsfDet.SetData(e.Row, vDetDocID, clsSvt.getDataLongValue("Select DocID from UtlTransactions where DocName='" & vsfDet.GetData(e.Row, vDetDocName) & "'", 0, False))
        End If
    End Sub

    Private Sub fillDtls()
        Dim i As Integer
        Dim strsql As String
        Dim introw As Integer
        Dim rsDtl As oledb.oledbDataReader


        strsql = "Select * from UtlStock"
        rsDtl = clsSvt.getDataReaderFromSQL(strsql)

        With rsDtl
            If Not .HasRows Then
                .Close()
                Exit Sub
            End If

            vsfDet.Rows.Count = 1
            Do While .Read
                vsfDet.Rows.Count = vsfDet.Rows.Count + 1
                introw = vsfDet.Rows.Count - 1
                i = introw
                vsfDet.SetData(i, vDetDocID, GetVal(!DocID, 0))
                vsfDet.SetData(i, vDetDocOrderBy, GetVal(!DocOrderBy, 0))
                vsfDet.SetData(i, vDetDocName, GetVal(!DocName, ""))
                vsfDet.SetData(i, vDetDocValuationReq, GetVal(!DocValuationReq, ""))
                vsfDet.SetData(i, vDetStkUpdate, GetVal(!StkUpdate, ""))
                vsfDet.SetData(i, vDetStkBalColName, GetVal(!StkBalColName, ""))
                vsfDet.SetData(i, vDetMain, GetVal(!MainFld, ""))
                vsfDet.SetData(i, vDetParticularsFld, GetVal(!ParticularsFld, ""))
                vsfDet.SetData(i, vDetQty, GetVal(!QtyFld, ""))
                vsfDet.SetData(i, vDetValue, GetVal(!ValueFld, ""))
                vsfDet.SetData(i, vDetItem, GetVal(!ItemFld, ""))
                vsfDet.SetData(i, vDetLoc, GetVal(!LocFld, ""))
                vsfDet.SetData(i, vDetLOTNO, GetVal(!LOTNOFld, ""))
                vsfDet.SetData(i, vDetDate, GetVal(!DateFld, ""))
                vsfDet.SetData(i, vDetCondition, GetVal(!Condition, ""))
                vsfDet.SetData(i, vDetFinYear, GetVal(!FinYear, ""))
                vsfDet.SetData(i, vDetConsiderForStock, GetVal(!ConsiderForStock, ""))
            Loop
            .Close()
        End With
    End Sub


    Private Sub vsfDet_KeyUp(sender As Object, e As KeyEventArgs) Handles vsfDet.KeyUp
        With vsfDet
            If e.KeyCode = Keys.Tab And .Col = vDetConsiderForStock And .Row = .Rows.Count - 1 And Val(.GetData(.Row, vDetDocID)) > 0 Then
                .Rows.Count = .Rows.Count + 1
                .Row = .Rows.Count - 1
                .Col = vDetDocName
                ReArrangeSlNo(vsfDet, vDetDocOrderBy)
            End If
        End With
    End Sub

    'Private Sub vsfDet_KeyPressEdit(sender As Object, e As KeyPressEditEventArgs) Handles vsfDet.KeyPressEdit
    '    MsgBox("")
    'End Sub
End Class