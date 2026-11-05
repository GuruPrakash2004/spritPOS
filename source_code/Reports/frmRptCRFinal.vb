Imports C1.Win.C1FlexGrid

Public Class frmRptCRFinal
    Dim clsSVT As New svtLibrary
    Dim strRights As String
    Const vFldTableCaption = 0
    Const vFldTableName = 1
    Const vFldCode = 2
    Const vFldOrder = 3
    Const vFldDataType = 4


    Const vDetFldCode = 0
    Const vDetFldCaption = 1
    Const vDetFldTableName = 2
    Const vDetFldDataType = 3
    Const vDetAggregate = 4
    Const vDetTotReq = 5
    Const vDetSubTotal = 6
    Const vDetFilterReq = 7
    Const vDetHideCol = 8
    Const vAddnlCol = 9

    Const vTblName = 0
    Const vTblProcessed = 1
    Const vTblCaption = 2
    Const vTblLinkCond = 3

    Const vDetFldName = 0
    Const vDetField = 1

    Public strvalue As String

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Public Sub FillFlds(ByVal argVsf As C1.Win.C1FlexGrid.C1FlexGrid, ByVal argVsfTbl As C1.Win.C1FlexGrid.C1FlexGrid)
        With vsfTbls
            .Rows.Count = 1
            .Cols.Count = 4
        End With

        With vsfDet
            .Rows.Count = 1
            .Cols.Count = 10
            .SetData(0, vDetFldCode, "Field")
            .SetData(0, vDetFldCaption, "Caption")
            .SetData(0, vDetFldTableName, "TableName")
            .SetData(0, vDetHideCol, "Hide Field")
            .SetData(0, vDetFldDataType, "DataType")
            .SetData(0, vDetAggregate, "Aggregate")
            .SetData(0, vDetTotReq, "Total Required")
            .SetData(0, vDetSubTotal, "SubTotal Required")
            .SetData(0, vDetFilterReq, "Filter Required")
            .SetData(0, vAddnlCol, "AdditionalColumn")

            .Cols(vDetFilterReq).DataType = GetType(Boolean)
            .Cols(vDetHideCol).DataType = GetType(Boolean)
            .Cols(vDetAggregate).ComboList = "Sum|Count|Avg| |"
            .Cols(vDetSubTotal).DataType = GetType(Boolean)
            .Cols(vDetTotReq).DataType = GetType(Boolean)
            '.Cols(vDetFldTableName).Visible = False
            .Cols(vAddnlCol).ComboList = "..."
        End With
        Call bindCombo(cboGroup, "", "ReportGroup", "Select distinct ReportGroup from UtlReportsCRHdr order by 1")

        For i = 1 To argVsf.Rows.Count - 1
            vsfDet.Rows.Count = vsfDet.Rows.Count + 1
            vsfDet.SetData(vsfDet.Rows.Count - 1, vDetFldCode, argVsf.GetData(i, vFldCode))
            vsfDet.SetData(vsfDet.Rows.Count - 1, vDetFldCaption, argVsf.GetData(i, vFldCode))
            vsfDet.SetData(vsfDet.Rows.Count - 1, vDetFldTableName, argVsf.GetData(i, vFldTableName))
            vsfDet.SetData(vsfDet.Rows.Count - 1, vDetFldDataType, argVsf.GetData(i, vFldDataType))
        Next

        For i = 1 To argVsfTbl.Rows.Count - 1
            vsfTbls.Rows.Count = vsfTbls.Rows.Count + 1
            vsfTbls.SetData(vsfTbls.Rows.Count - 1, vTblName, argVsfTbl.GetData(i, vTblName))
            vsfTbls.SetData(vsfTbls.Rows.Count - 1, vTblCaption, argVsfTbl.GetData(i, vTblCaption))
            vsfTbls.SetData(vsfTbls.Rows.Count - 1, vTblLinkCond, argVsfTbl.GetData(i, vTblLinkCond))
        Next
        ' Dim blnFound As Boolean
        'clsSVT.getDataLongValue("Select CRID from UtlreportsCRHdr where TblName = " & strvalue & "", "", blnFound)
        'If blnFound Then

        'End If

        If Val(txtRptID.Text) > 0 Then
            Call fillFromTblStorage()
        Else
            txtRptName.Text = ""
            'txtRptName.Text = clsSVT.getDataStringValue("Select CRName from UtlreportsCRHdr where CRID = " &  & , "", False)
        End If

    End Sub

    Private Sub btnUp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Call MoveOneRowUp(vsfDet, -1)
    End Sub

    Private Sub btnDown_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Call MoveOneRowDown(vsfDet, -1)
    End Sub

    Private Sub btnFinish_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFinish.Click
        If Not chkUserRights(strRights, chkRightsPos.AddRights, Me.Text) Then Exit Sub
        If txtRptName.Text = "" Then
            MsgBox("Please enter the report name to save this report!", MsgBoxStyle.Critical, Me.Text)
            Exit Sub
        End If
        'check for unique report name
        If Not saveReport() Then Exit Sub
        MsgBox("Report saved successfully!", MsgBoxStyle.Information, Me.Text)
        Me.Close()
    End Sub
    Private Function saveReport() As Boolean
        saveReport = False
        Dim i As Integer, strSQL As String
        Dim blnFound As Boolean
        'insert into Header
        If cboGroup.Text = "" Then
            cboGroup.Text = "General"
        End If

        txtRptName.Text = Replace(txtRptName.Text, "'", "")
        cboGroup.Text = Replace(cboGroup.Text, "'", "")
        txtCondition.Text = Replace(txtCondition.Text, "'", "''")

        'txtRptName.Text = StrConv(txtRptName.Text, VbStrConv.ProperCase)
        cboGroup.Text = StrConv(cboGroup.Text, VbStrConv.ProperCase)

        If Val(txtRptID.Text) <= 0 Then
            Call clsSVT.getDataLongValue("Select CRID from UtlReportsCRHdr where CRName = '" & txtRptName.Text & "' and ReportGroup = '" & cboGroup.Text & "'", 0, blnFound)
        Else
            Call clsSVT.getDataLongValue("Select CRID from UtlReportsCRHdr where CRID <> " & Val(txtRptID.Text) & " and CRName = '" & txtRptName.Text & "' and ReportGroup = '" & cboGroup.Text & "'", 0, blnFound)
        End If

        If blnFound Then
            MsgBox("Report with name  [ " & txtRptName.Text & " ] already Exists!", MsgBoxStyle.Critical, Me.Text)
            Exit Function
        End If

        If Val(txtRptID.Text) > 0 Then
            Call clsSVT.execSQLStatement("Delete from UtlReportsCRHdr where CRID  = " & Val(txtRptID.Text))
            Call clsSVT.execSQLStatement("Delete from UtlReportsCRDtl where CRID  = " & Val(txtRptID.Text))
            Call clsSVT.execSQLStatement("Delete from UtlReportsCRTbl where CRID  = " & Val(txtRptID.Text))
        Else
            txtRptID.Text = clsSVT.AutoIncrementedNum("CRID", "UtlReportsCRHdr").ToString
        End If
        strSQL = "Insert into UtlReportsCRHdr (CRID, CRName,CRDesc, HdrTblName,ReportGroup,CondText,RptFileName,IsSlnoReq) values ("
        strSQL = strSQL & Val(txtRptID.Text) & ","
        strSQL = strSQL & "'" & Trim(txtRptName.Text) & "',"
        strSQL = strSQL & "'" & Trim(txtCRDesc.Text) & "',"
        strSQL = strSQL & "'" & Trim(txtHdrTblName.Text) & "',"
        strSQL = strSQL & "'" & Trim(cboGroup.Text) & "',"
        strSQL = strSQL & "'" & Trim(txtCondition.Text) & "',"
        strSQL = strSQL & "'" & Trim(txtRptFileName.Text) & "',"
        strSQL = strSQL & "'" & getCheckString(chkSlno) & "')"

        Call clsSVT.execSQLStatement(strSQL)

        'insert into detail
        With vsfDet
            For i = 1 To .Rows.Count - 1
                strSQL = "Insert into UtlReportsCRDtl(CRID,"
                strSQL = strSQL & " TblName, FldCode,FldCaption,FldDataType,TotReq,FldOrderBy,"
                strSQL = strSQL & " FilterReq,FldHideCol,FldAggregate,SubTotOn,AddnlCol"
                strSQL = strSQL & " )values("

                strSQL = strSQL & Val(txtRptID.Text) & ","
                strSQL = strSQL & "'" & .GetData(i, vDetFldTableName) & "',"
                strSQL = strSQL & "'" & .GetData(i, vDetFldCode) & "',"
                strSQL = strSQL & "'" & .GetData(i, vDetFldCaption) & "',"
                strSQL = strSQL & "'" & .GetData(i, vDetFldDataType) & "',"
                strSQL = strSQL & getVsfCheckedYN(vsfDet, i, vDetTotReq) & ","
                strSQL = strSQL & i & ","
                strSQL = strSQL & getVsfCheckedYN(vsfDet, i, vDetFilterReq) & ","
                strSQL = strSQL & getVsfCheckedYN(vsfDet, i, vDetHideCol) & ","
                strSQL = strSQL & "'" & Trim(.GetData(i, vDetAggregate)) & "',"
                strSQL = strSQL & getVsfCheckedYN(vsfDet, i, vDetSubTotal) & ","
                strSQL = strSQL & "'" & .GetData(i, vAddnlCol) & "'"
                strSQL = strSQL & ") "
                Call clsSVT.execSQLStatement(strSQL)
            Next
        End With

        'insert into Tbl
        With vsfTbls
            For i = 1 To .Rows.Count - 1
                strSQL = "Insert into UtlReportsCRTbl(CRID,"
                strSQL = strSQL & " TblName, TblLinkCond"
                strSQL = strSQL & " )values("

                strSQL = strSQL & Val(txtRptID.Text) & ","
                strSQL = strSQL & "'" & .GetData(i, vTblName) & "',"
                strSQL = strSQL & "'" & .GetData(i, vTblLinkCond) & "')"
                Call clsSVT.execSQLStatement(strSQL)
            Next
        End With
        MsgBox("Saved Successfully!", MsgBoxStyle.Information, Me.Text)

        Return True
    End Function
    Private Sub fillFromTblStorage()
        Dim rsF As oledb.oledbDataReader
        rsF = clsSVT.getDataReaderFromSQL("Select * from UtlReportsCRHdr Where CRID = " & Val(txtRptID.Text))
        If Not rsF.HasRows Then
            rsF.Close()
            Exit Sub
        End If

        rsF.Read()
        txtCRDesc.Text = GetVal(rsF!CRDesc, "")
        txtRptName.Text = GetVal(rsF!CRName, "")
        cboGroup.Text = GetVal(rsF!ReportGroup, "")
        txtCondition.Text = GetVal(rsF!CondText, "")
        txtRptFileName.Text = GetVal(rsF!RptFileName, "")
        If GetVal(rsF!IsSlnoReq, "") = "Y" Then
            chkSlno.Checked = True
        Else
            chkSlno.Checked = False
        End If
        rsF.Close()
        'txtCRDesc.Text = clsSVT.getDataStringValue("Select CRDesc from UtlreportsCRHdr where CRID = " & Val(txtRptID.Text), "", False)
        'txtRptName.Text = clsSVT.getDataStringValue("Select CRName from UtlreportsCRHdr where CRID = " & Val(txtRptID.Text), "", False)
        'cboGroup.Text = clsSVT.getDataStringValue("Select ReportGroup from UtlreportsCRHdr where CRID = " & Val(txtRptID.Text), "", False)
        'txtCondition.Text = clsSVT.getDataStringValue("Select CondText from UtlreportsCRHdr where CRID = " & Val(txtRptID.Text), "", False)
        'txtRptFileName.Text = clsSVT.getDataStringValue("Select RptFileName from UtlreportsCRHdr where CRID = " & Val(txtRptID.Text), "", False)

        rsF = clsSVT.getDataReaderFromSQL("Select * from UtlreportsCRDtl where CRID = " & Val(txtRptID.Text) & " Order by FldOrderBy")
        With rsF
            If .HasRows Then
                Do While .Read
                    For i = 1 To vsfDet.Rows.Count - 1
                        If !fldCode = vsfDet.GetData(i, vDetFldCode) And !TblName = vsfDet.GetData(i, vDetFldTableName) Then
                            vsfDet.SetData(i, vDetFldCaption, !FldCaption)
                            SetVsfChecked(!TotReq, vsfDet, i, vDetTotReq)
                            SetVsfChecked(!FilterReq, vsfDet, i, vDetFilterReq)
                            SetVsfChecked(!FldHideCol, vsfDet, i, vDetHideCol)
                            SetVsfChecked(!SubTotOn, vsfDet, i, vDetSubTotal)
                            vsfDet.SetData(i, vDetAggregate, !FldAggregate)
                            vsfDet.SetData(i, vAddnlCol, !AddnlCol)
                            Exit For
                        End If
                    Next
                Loop
            End If
        End With
    End Sub

    Private Sub vsfDet_BeforeEdit(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfDet.BeforeEdit
        With vsfDet
            Select Case e.Col
                Case vDetTotReq
                    If .GetData(e.Row, vDetFldDataType) <> "NUMERIC" Then
                        e.Cancel = True
                        Exit Sub
                    End If
                Case vDetSubTotal
                    If .GetData(e.Row, vDetFldDataType) = "NUMERIC" Then
                        e.Cancel = True
                        Exit Sub
                    End If

            End Select
        End With
    End Sub

    Private Sub vsfDet_ValidateEdit(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.ValidateEditEventArgs) Handles vsfDet.ValidateEdit
        With vsfDet
            Select Case e.Col
                Case vDetAggregate
                    If Trim(.Editor.Text) = "" Then Exit Sub
                    'If Trim(.GetData(e.Row, vDetAggregate)) = "" Then Exit Sub
                    If .GetData(e.Row, vDetFldDataType) <> "NUMERIC" Then
                        If .Editor.Text <> "Count" Then
                            MsgBox("Please select Count for Fields other than Numeric", MsgBoxStyle.Critical, Me.Text)
                            e.Cancel = True
                            Exit Sub
                        End If
                    End If
            End Select
        End With
    End Sub

    Private Sub frmRptCRFinal_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        initGridStyles(Me)
        strRights = getUserRights(GetMenuId(Me.Text))
        ' txtRptName.Text = ""
    End Sub
    Private Sub btnMoveUp_Click(sender As System.Object, e As System.EventArgs) Handles btnMoveUp.Click
        Call MoveOneRowUp(vsfDet, -1)
    End Sub

    Private Sub btnMoveDown_Click(sender As Object, e As System.EventArgs) Handles btnMoveDown.Click
        Call MoveOneRowDown(vsfDet, -1)
    End Sub

    Private Sub vsfDet_CellButtonClick(sender As Object, e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfDet.CellButtonClick
        Dim strValue As String
        With vsfFields
            .Rows.Count = 1
            .Cols.Count = 2
            .SetData(0, vDetFldName, "TableName")
            .SetData(0, vDetField, "Fields")

        End With

        For i = 1 To vsfDet.Rows.Count - 1
            vsfFields.Rows.Count = vsfFields.Rows.Count + 1
            vsfFields.SetData(i, vDetFldName, vsfDet.GetData(i, vDetFldTableName))
            vsfFields.SetData(i, vDetField, vsfDet.GetData(i, vDetFldCode))
        Next

        strValue = vsfDet.GetData(e.Row, vAddnlCol)
        If strValue <> "" Then
            Dim strWordArr() As String = strValue.Split("|")
            txtCaption.Text = strWordArr(1)
            txtAdd.Text = strWordArr(0)
            pnlAddnlCol.Visible = True
            txtCaption.Focus()
        Else
            pnlAddnlCol.Visible = True
            txtCaption.Focus()
        End If

    End Sub

    Private Sub btnOK_Click(sender As Object, e As EventArgs) Handles btnOK.Click



        If Trim(txtCaption.Text) = "" AndAlso Trim(txtAdd.Text) = "" Then
            If MsgBox("No Caption / Clear Caption... Do you want to update empty...?", MsgBoxStyle.OkCancel, Me.Text) = MsgBoxResult.Ok Then
                vsfDet.SetData(vsfDet.Row, vAddnlCol, "")
                pnlAddnlCol.Visible = False
            End If
        ElseIf Trim(txtCaption.Text) = "" Then
            MsgBox("Please enter caption", MsgBoxStyle.Critical, Me.Text)
            txtCaption.Focus()
        ElseIf Trim(txtAdd.Text) = "" Then
            MsgBox("Please enter value", MsgBoxStyle.Critical, Me.Text)
            txtAdd.Focus()
        Else
            vsfDet.SetData(vsfDet.Row, vAddnlCol, Trim(txtAdd.Text) & "|" & Trim(txtCaption.Text))
            pnlAddnlCol.Visible = False
        End If
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        pnlAddnlCol.Visible = "False"
        txtAdd.Text = ""
        txtCaption.Text = ""
    End Sub

    Private Sub vsfFields_DoubleClick(sender As Object, e As EventArgs) Handles vsfFields.DoubleClick
        ''''Changed
        If vsfFields.Row = 0 Then
            Exit Sub
        End If
        ''''Changed
        If txtAdd.Text <> "" Then
            txtAdd.Text &= " "
            txtAdd.Text &= vsfFields.GetData(vsfFields.Row, vDetFldName)
            txtAdd.Text &= "."
            txtAdd.Text &= vsfFields.GetData(vsfFields.Row, vDetField)
        Else
            txtAdd.Text = vsfFields.GetData(vsfFields.Row, vDetFldName)
            txtAdd.Text &= "."
            txtAdd.Text &= vsfFields.GetData(vsfFields.Row, vDetField)
        End If
    End Sub


End Class