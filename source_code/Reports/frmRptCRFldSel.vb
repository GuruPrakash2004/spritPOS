Public Class frmRptCRFldSel
    Dim clsSVT As New svtLibrary
    Dim strRptName As String
    Dim strDocName As String
    Dim strDocType As String

    Const vFldTableCaption = 0
    Const vFldTableName = 1
    Const vFldCode = 2
    Const vFldOrder = 3
    Const vFldDataType = 4

    Const vTblName = 0
    Const vTblProcessed = 1
    Const vTblCaption = 2
    Const vTblLinkCond = 3


    Public Sub FillFldSettings(ByVal argType As String, ByVal argName As String)

        Dim strTblName As String
        With vsfTbls
            .Rows.Count = 1
            .Cols.Count = 4
        End With

        btnAdd.Enabled = True
        btnAddAll.Enabled = True
        With vsfFrom
            .Cols.Count = 5
            .Rows.Count = 1
            .Cols(vFldTableName).Visible = False
            .Cols(vFldOrder).Visible = False
            .Cols(vFldDataType).Visible = False
            .SetData(0, vFldCode, "Source")
            .ExtendLastCol = True
            .Cols(vFldOrder).AllowSorting = True
        End With

        With vsfTo
            .Cols.Count = 5
            .Rows.Count = 1
            .Cols(vFldTableName).Visible = False
            .Cols(vFldTableCaption).Visible = False
            .Cols(vFldOrder).Visible = False
            .Cols(vFldDataType).Visible = False
            .SetData(0, vFldCode, "Target")
            .ExtendLastCol = True
        End With
        strDocType = argType
        Select Case argType
            Case "M", "T"
                strDocName = argName
                strTblName = UCase(Trim(clsSVT.getDataStringValue("Select HdrTblName from UtlTransactions where DocName = '" & argName & "'", "", False)))
                If strTblName = "" Then
                    MsgBox("Invalid Document!", MsgBoxStyle.Critical, Me.Text)
                    Exit Sub
                End If
                Call fillFldsFromTbl(argName)
                txtRptID.Text = ""
            Case "A"
                strDocName = argName
                strTblName = argName
                Call fillFldsFromTbl(argName)
                txtRptID.Text = ""
            Case "E"
                strRptName = argName
                txtRptID.Text = clsSVT.getDataLongValue("Select CRID from UtlReportsCRHdr where CRName = '" & strRptName & "'", 0, False)
                If txtRptID.Text = 0 Then Exit Sub
                Call fillFldsFromExist(Val(txtRptID.Text))

        End Select

        If vsfTo.Rows.Count = 1 Then
            btnRemAll.Enabled = False
            btnRemove.Enabled = False
            btnNext.Enabled = False
        Else
            btnRemAll.Enabled = True
            btnRemove.Enabled = True
            btnNext.Enabled = True
        End If

        vsfFrom.AutoSizeCols()
    End Sub

    Private Sub fillFldsFromTbl(ByVal argName As String)
        Dim strSQL As String, strFMT As String = ""
        Dim strDocIDFld As String
        Dim strTblName As String, blnFound As Boolean
        Dim strDtlTblName As String
        Dim rs As oledb.oledbDataReader
        vsfTbls.Rows.Count = 1

        strTblName = UCase(Trim(clsSVT.getDataStringValue("Select HdrTblName from UtlTransactions where DocName = '" & argName & "'", "", False)))
        If strTblName = "" Then
            strTblName = UCase(Trim(clsSVT.getDataStringValue("Select HdrTblName from UtlTransactions where HdrTblName = '" & argName & "'", "", False)))
        End If
        If strTblName = "" Then
            'must be from view
            If fnTablePresent(argName) Then
                strTblName = argName
            End If
        End If
        If strTblName = "" Then
            MsgBox("Invalid Document!", MsgBoxStyle.Critical)
            Me.Close()
            Exit Sub
        End If
        txtHdrTblName.Text = strTblName

        strDocIDFld = Replace(strTblName, "MST", "")
        strDocIDFld = Replace(strDocIDFld, "TRNHDR", "")
        strDocIDFld = strDocIDFld & "ID"

        vsfTbls.Rows.Count = vsfTbls.Rows.Count + 1
        vsfTbls.SetData(vsfTbls.Rows.Count - 1, vTblName, strTblName)
        vsfTbls.SetData(vsfTbls.Rows.Count - 1, vTblCaption, argName)

        If strDocType <> "A" Then
            strDtlTblName = clsSVT.getDataStringValue("Select DtlTblName from UtlTransactions where HdrTblName = '" & strTblName & "'", "", False)
            If Trim(strDtlTblName) <> "" Then
                vsfTbls.Rows.Count = vsfTbls.Rows.Count + 1
                vsfTbls.SetData(vsfTbls.Rows.Count - 1, vTblName, strDtlTblName)
                vsfTbls.SetData(vsfTbls.Rows.Count - 1, vTblCaption, "Detail " & argName)
                vsfTbls.SetData(vsfTbls.Rows.Count - 1, vTblLinkCond, strTblName & "." & strDocIDFld & " = " & strDtlTblName & "." & strDocIDFld)
            End If
        End If

        Do While True
            With vsfTbls
                If .Rows.Count = 1 Then Exit Do
                blnFound = False
                For i = 1 To .Rows.Count - 1
                    If .GetData(i, vTblProcessed) = "" Then
                        blnFound = True
                    End If
                    If .GetData(i, vTblProcessed) = "" Then
                        strSQL = getFieldsFromTbl(.GetData(i, vTblName))
                        rs = clsSVT.getDataReaderFromSQL(strSQL)
                        rs.Read()
                        For x = 0 To rs.FieldCount - 1
                            vsfFrom.Rows.Count = vsfFrom.Rows.Count + 1
                            vsfFrom.SetData(vsfFrom.Rows.Count - 1, vFldCode, rs.GetName(x))
                            vsfFrom.SetData(vsfFrom.Rows.Count - 1, vFldTableCaption, .GetData(i, vTblCaption))
                            vsfFrom.SetData(vsfFrom.Rows.Count - 1, vFldTableName, .GetData(i, vTblName))
                            vsfFrom.SetData(vsfFrom.Rows.Count - 1, vFldOrder, vsfFrom.Rows.Count - 1)
                            vsfFrom.SetData(vsfFrom.Rows.Count - 1, vFldDataType, getFldDataType(rs.GetFieldType(x).Name))
                        Next x
                        .SetData(i, vTblProcessed, "Y")
                        rs.Close()
                    End If
                Next
            End With
            If Not blnFound Then Exit Do
        Loop
        vsfFrom.Cols(vFldTableCaption).AllowMerging = True
    End Sub

    Private Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        With vsfFrom
            If .Row <= 0 Then Exit Sub
            btnRemAll.Enabled = True
            btnRemove.Enabled = True
            btnNext.Enabled = True

            vsfTo.Rows.Count = vsfTo.Rows.Count + 1
            vsfTo.SetData(vsfTo.Rows.Count - 1, vFldCode, .GetData(vsfFrom.Row, vFldCode))
            vsfTo.SetData(vsfTo.Rows.Count - 1, vFldTableName, .GetData(vsfFrom.Row, vFldTableName))
            vsfTo.SetData(vsfTo.Rows.Count - 1, vFldTableCaption, .GetData(vsfFrom.Row, vFldTableCaption))
            vsfTo.SetData(vsfTo.Rows.Count - 1, vFldOrder, .GetData(vsfFrom.Row, vFldOrder))
            vsfTo.SetData(vsfTo.Rows.Count - 1, vFldDataType, .GetData(vsfFrom.Row, vFldDataType))
            vsfTo.Row = vsfTo.Rows.Count - 1
            .RemoveItem(.Row)
            If .Rows.Count = 1 Then
                btnAdd.Enabled = False
                btnAddAll.Enabled = False
            End If
        End With
    End Sub

    Private Sub btnRemove_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRemove.Click
        Dim order As C1.Win.C1FlexGrid.SortFlags = C1.Win.C1FlexGrid.SortFlags.Ascending
        With vsfTo
            If .Row <= 0 Then Exit Sub
            vsfFrom.Rows.Count = vsfFrom.Rows.Count + 1
            vsfFrom.SetData(vsfFrom.Rows.Count - 1, vFldCode, .GetData(vsfTo.Row, vFldCode))
            vsfFrom.SetData(vsfFrom.Rows.Count - 1, vFldTableCaption, .GetData(vsfTo.Row, vFldTableCaption))
            vsfFrom.SetData(vsfFrom.Rows.Count - 1, vFldTableName, .GetData(vsfTo.Row, vFldTableName))
            vsfFrom.SetData(vsfFrom.Rows.Count - 1, vFldOrder, .GetData(vsfTo.Row, vFldOrder))
            vsfFrom.SetData(vsfFrom.Rows.Count - 1, vFldDataType, .GetData(vsfTo.Row, vFldDataType))
            vsfFrom.Row = vsfFrom.Rows.Count - 1

            'vsfFrom.Cols(vFldOrder).Sort = C1.Win.C1FlexGrid.SortFlags.Ascending

            vsfFrom.Sort(order, vFldOrder)
            .RemoveItem(.Row)
            If .Rows.Count = 1 Then
                btnRemAll.Enabled = False
                btnRemove.Enabled = False
                btnNext.Enabled = False
                btnAdd.Enabled = True
                btnAddAll.Enabled = True
            End If
        End With
    End Sub

    Private Sub btnNext_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNext.Click
        frmRptCRFinal.txtRptID.Text = txtRptID.Text
        frmRptCRFinal.txtHdrTblName.Text = txtHdrTblName.Text
        frmRptCRFinal.FillFlds(vsfTo, vsfTbls)
        frmRptCRFinal.ShowDialog()
    End Sub

    Private Function getFieldsFromTbl(ByVal argTblName As String) As String
        getFieldsFromTbl = ""
        Dim rsFlds As oledb.oledbDataReader
        Dim blnInclude As Boolean, i As Integer, x As Integer
        Dim blnFound As Boolean
        Dim strFldList As String = "", strTblList As String, strFindFld As String
        Dim strCondList As String = ""
        Dim strSubTblCaption As String
        Dim strSQL As String
        Dim strFldName As String

        argTblName = UCase$(argTblName)

        strSQL = "Select top 1000 * from  " & argTblName & " where 1 <> 1"
        rsFlds = clsSVT.getDataReaderFromSQL(strSQL)

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
                    If strDocType <> "A" Then
                        If InStr(1, strFindFld, "ID") > 0 Then
                            strFindFld = Replace(strFindFld, "ID", "")
                            If "MST" & strFindFld <> argTblName And "TRNHDR" & strFindFld <> argTblName And "TRNDTL" & strFindFld <> argTblName Then
                                'if it is not from the same table MstItem or TrnHdrInv or TrnDtlInv
                                'and if the master exists then 
                                strSubTblCaption = clsSVT.getDataStringValue("select docName from UtlTransactions where HdrTblName = 'MST" & strFindFld & "'", "", blnFound)
                                If blnFound Then
                                    If fnTablePresent("MST" & strFindFld) Then
                                        blnFound = False
                                        With vsfTbls
                                            For x = 1 To .Rows.Count - 1
                                                If UCase("MST" & strFindFld) = UCase(.GetData(x, vTblName)) Then
                                                    blnFound = True
                                                End If
                                            Next
                                        End With
                                        If Not blnFound Then
                                            vsfTbls.Rows.Count = vsfTbls.Rows.Count + 1
                                            vsfTbls.SetData(vsfTbls.Rows.Count - 1, vTblName, "MST" & strFindFld)
                                            vsfTbls.SetData(vsfTbls.Rows.Count - 1, vTblCaption, strSubTblCaption)
                                            vsfTbls.SetData(vsfTbls.Rows.Count - 1, vTblLinkCond, argTblName & "." & strFldName & " = " & "MST" & strFindFld & "." & strFindFld & "ID")
                                        End If
                                    End If
                                End If
                            End If
                        End If
                    End If
                    strFldList = strFldList & argTblName & "." & GetVal(strFldName, "") & " AS " & strFldName & ","
                End If
            Next i
            rsFlds.Close()
        End With
        strFldList = Mid(strFldList, 1, Len(strFldList) - 1)
        strTblList = Mid(strTblList, 1, Len(strTblList) - 1)

        strSQL = "SELECT " & strFldList

        strSQL = strSQL & " FROM " & strTblList
        strSQL = strSQL & " WHERE 1=1 " & strCondList
        getFieldsFromTbl = strSQL
    End Function

    Private Sub fillFldsFromExist(ByVal intRptID As Integer)
        Dim strHdrTblName As String
        strHdrTblName = UCase(Trim(clsSVT.getDataStringValue("Select HdrTblName from UtlReportsCRHdr where CRID = " & intRptID, "", False)))
        If strHdrTblName = "" Then
            MsgBox("Invalid Report!", MsgBoxStyle.Critical, Me.Text)
            Me.Close()
            Exit Sub
        End If
        If UCase(Strings.Left(strHdrTblName, 2)) = "VW" Then strDocType = "A"
        Call fillFldsFromTbl(strHdrTblName)
        Call fillSelectedFldsFromStorage(intRptID)
    End Sub

    Private Sub fillSelectedFldsFromStorage(ByVal intRptID As Integer)
        Dim rsF As oledb.oledbDataReader
        rsF = clsSVT.getDataReaderFromSQL("Select * from UtlreportsCRDtl where CRID = " & intRptID & " Order by FldOrderBy")
        With rsF
            If .HasRows Then
                Do While .Read
                    For i = 1 To vsfFrom.Rows.Count - 1
                        If UCase(!fldCode) = UCase(vsfFrom.GetData(i, vFldCode)) And UCase(!TblName) = UCase(vsfFrom.GetData(i, vFldTableName)) Then
                            With vsfFrom
                                vsfTo.Rows.Count = vsfTo.Rows.Count + 1
                                vsfTo.SetData(vsfTo.Rows.Count - 1, vFldCode, .GetData(i, vFldCode))
                                vsfTo.SetData(vsfTo.Rows.Count - 1, vFldTableName, .GetData(i, vFldTableName))
                                vsfTo.SetData(vsfTo.Rows.Count - 1, vFldTableCaption, .GetData(i, vFldTableCaption))
                                vsfTo.SetData(vsfTo.Rows.Count - 1, vFldOrder, .GetData(i, vFldOrder))
                                vsfTo.SetData(vsfTo.Rows.Count - 1, vFldDataType, .GetData(i, vFldDataType))
                                vsfTo.Row = vsfTo.Rows.Count - 1
                                .RemoveItem(i)
                            End With
                            Exit For
                        End If
                    Next
                Loop
                .Close()
            End If
        End With
    End Sub

    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        Me.Close()
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

 
    Private Sub btnAddAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddAll.Click
        Dim i As Integer
        With vsfFrom
repeat:
            For i = 1 To .Rows.Count - 1
                btnNext.Enabled = True
                vsfTo.Rows.Count = vsfTo.Rows.Count + 1
                vsfTo.SetData(vsfTo.Rows.Count - 1, vFldCode, .GetData(i, vFldCode))
                vsfTo.SetData(vsfTo.Rows.Count - 1, vFldTableName, .GetData(i, vFldTableName))
                vsfTo.SetData(vsfTo.Rows.Count - 1, vFldTableCaption, .GetData(i, vFldTableCaption))
                vsfTo.SetData(vsfTo.Rows.Count - 1, vFldOrder, .GetData(i, vFldOrder))
                vsfTo.SetData(vsfTo.Rows.Count - 1, vFldDataType, .GetData(i, vFldDataType))
                vsfTo.Row = vsfTo.Rows.Count - 1
                .RemoveItem(i)
                GoTo repeat
            Next i
        End With
        btnRemAll.Enabled = True
        btnRemove.Enabled = True
        btnNext.Enabled = True
        btnAdd.Enabled = False
        btnAddAll.Enabled = False
    End Sub

    Private Sub btnRemAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRemAll.Click
        Dim order As C1.Win.C1FlexGrid.SortFlags = C1.Win.C1FlexGrid.SortFlags.Ascending
        Dim i As Integer
        With vsfTo
repeat:
            For i = 1 To .Rows.Count - 1
                vsfFrom.Rows.Count = vsfFrom.Rows.Count + 1
                vsfFrom.SetData(vsfFrom.Rows.Count - 1, vFldCode, .GetData(i, vFldCode))
                vsfFrom.SetData(vsfFrom.Rows.Count - 1, vFldTableCaption, .GetData(i, vFldTableCaption))
                vsfFrom.SetData(vsfFrom.Rows.Count - 1, vFldTableName, .GetData(i, vFldTableName))
                vsfFrom.SetData(vsfFrom.Rows.Count - 1, vFldOrder, .GetData(i, vFldOrder))
                vsfFrom.SetData(vsfFrom.Rows.Count - 1, vFldDataType, .GetData(i, vFldDataType))
                vsfFrom.Row = vsfFrom.Rows.Count - 1

                'vsfFrom.Cols(vFldOrder).Sort = C1.Win.C1FlexGrid.SortFlags.Ascending

                .RemoveItem(i)
                GoTo repeat
            Next i
            vsfFrom.Sort(order, vFldOrder)
            btnNext.Enabled = False
            btnRemAll.Enabled = False
            btnRemove.Enabled = False
            btnAdd.Enabled = True
            btnAddAll.Enabled = True
        End With
    End Sub

    Private Sub vsfFrom_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles vsfFrom.DoubleClick
        With vsfFrom
            If .Row <= 0 Then Exit Sub
            btnRemAll.Enabled = True
            btnRemove.Enabled = True
            btnNext.Enabled = True

            vsfTo.Rows.Count = vsfTo.Rows.Count + 1
            vsfTo.SetData(vsfTo.Rows.Count - 1, vFldCode, .GetData(vsfFrom.Row, vFldCode))
            vsfTo.SetData(vsfTo.Rows.Count - 1, vFldTableName, .GetData(vsfFrom.Row, vFldTableName))
            vsfTo.SetData(vsfTo.Rows.Count - 1, vFldTableCaption, .GetData(vsfFrom.Row, vFldTableCaption))
            vsfTo.SetData(vsfTo.Rows.Count - 1, vFldOrder, .GetData(vsfFrom.Row, vFldOrder))
            vsfTo.SetData(vsfTo.Rows.Count - 1, vFldDataType, .GetData(vsfFrom.Row, vFldDataType))
            vsfTo.Row = vsfTo.Rows.Count - 1
            .RemoveItem(.Row)
            If .Rows.Count = 1 Then
                btnAdd.Enabled = False
                btnAddAll.Enabled = False
            End If
        End With
    End Sub

    Private Sub frmRptCRFldSel_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        initGridStyles(Me)
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub
End Class