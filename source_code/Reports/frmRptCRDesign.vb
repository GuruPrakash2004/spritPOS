Imports C1.Win.C1FlexGrid
Imports System.Reflection
Imports System.IO
Imports System.ComponentModel

Public Class frmRptCRDesign
    Dim clsSvt As New svtLibrary
    Dim strRights As String
    Public StrValue As String = ""
    Dim svtTrn As oledb.oledbTransaction

    Const vExpColQry = 0
    Const vExpColTol = 1

    Const vExpRowCaption = 0
    Const vExpRowCRName = 1
    Const vExpRowHdr = 2
    Const vExpRowDtl = 3
    Const vExpRowTbl = 4
    Const vExpRowTol = 5

    Private Sub optMasters_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles optMasters.CheckedChanged
        If optMasters.Checked = True Then
            btnDelete.Visible = False
            pnlExp.Visible = False
            btnExport.Visible = False
            lblSel.Text = "Select Master"
            cboSel.Text = ""
            Call bindCombo(cboSel, "docID", "DocName", "Select docID, DocName from UtlTransactions where  doctype = 'Master' and docid not in (1,3,6,9,13,14,62,33,17,10,5) order by 1")
            Call enabledisable(False)
        End If
    End Sub

    Private Sub optTrnHdr_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles optTrnHdr.CheckedChanged
        If optTrnHdr.Checked = True Then
            btnDelete.Visible = False
            pnlExp.Visible = False
            btnExport.Visible = False
            lblSel.Text = "Select Transaction [Header]"
            cboSel.Text = ""
            Call bindCombo(cboSel, "docID", "DocName", "Select docID, DocName from UtlTransactions where  doctype = 'Transaction' order by 1")
            Call enabledisable(False)
        End If
    End Sub

    Private Sub optExisting_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles optExisting.CheckedChanged
        If optExisting.Checked = True Then
            btnDelete.Visible = True
            btnExport.Visible = True
            lblSel.Text = "Select Existing"
            cboSel.Text = ""
            Call bindCombo(cboSel, "CRID", "CRNAME", "Select CRID, CRName from UtlReportsCRHdr order by CRNAME")
            Call enabledisable(False)
        End If
    End Sub

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Function execView(ByVal argMOde As String) As Boolean
        execView = False
        Dim strSQL As String = ""
        If argMOde = "A" Then
            strSQL = "CREATE view " & cboSel.Text & " as " & txtView.Text & ""
        Else
            strSQL = txtView.Text
        End If

        If Not clsSvt.execSQLStatement(strSQL) Then
            Exit Function
        End If
        Return True
    End Function

    Private Sub btnNext_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNext.Click
        Dim strType As String = ""
        Dim strSql As String

        ' Edited 
        If optAdhoc.Checked Then
            Dim blnFound As Boolean
            clsSvt.getDataStringValue("Select name from sys.views where name='" & cboSel.Text & "'", "", blnFound)

            If blnFound Then
                If Trim(txtView.Text) <> "" Then
                    If Not execView("M") Then Exit Sub
                End If
            Else
                If Trim(txtView.Text) = "" Then
                    MsgBox("Please enter the query", MsgBoxStyle.Critical, Me.Text)
                    txtView.Text = ""
                    txtView.Focus()
                    Exit Sub
                End If
                If Not execView("A") Then Exit Sub
            End If

            'nextExec:




            '            ' Dim blnFound As Boolean
            '            clsSvt.getDataStringValue("Select name from sys.views where name='" & cboSel.Text & "'", "", blnFound)
            '            If Not blnFound Then
            '                If txtView.Text.ToUpper.Contains("SELECT") AndAlso txtView.Text.ToUpper.Contains("FROM") Then
            '                    strSql = "CREATE view " & cboSel.Text & " as " & txtView.Text & ""
            '                Else

            '                    MsgBox("Please enter the query", MsgBoxStyle.Critical, Me.Text)
            '                    txtView.Text = ""
            '                    txtView.Focus()
            '                    Exit Sub
            '                End If
            '            Else
            '                If txtView.Text = "" Then
            '                    strSql = "select VIEW_DEFINITION from INFORMATION_SCHEMA.VIEWS where table_name='" & cboSel.Text & "'"


            '                ElseIf txtView.Text.ToUpper.Contains("SELECT") AndAlso txtView.Text.ToUpper.Contains("FROM") Then
            '                    strSql = "select VIEW_DEFINITION from INFORMATION_SCHEMA.VIEWS where table_name='" & cboSel.Text & "'"
            '                Else
            '                    MsgBox("Please enter the query", MsgBoxStyle.Critical, Me.Text)
            '                    txtView.Text = ""
            '                    txtView.Focus()
            '                    Exit Sub
            '                End If

            '            End If

            '            clsSvt.execSQLStatement(strSql)
            '            'If Not clsSvt.execSQLStatement(strSql) Then
            '            '    MsgBox("Please enter proper query", MsgBoxStyle.Critical, Me.Text)
            '            '    Exit Sub

            '            ' End If


        End If

        If optExisting.Checked Then
            strType = "E"
            Dim intCRID As Int32 = clsSvt.getDataLongValue("Select CRID from UtlReportsCRHdr where CRName = '" & Trim(cboSel.Text) & "'", 0, False)
            If intCRID = 0 Then
                MsgBox("Invalid Document!", MsgBoxStyle.Critical, Me.Text)
                Exit Sub
            End If
        Else
            'clsSvt.getDataStringValue("Select HdrTblName from UtlTransactions where DocName = '" & argName & "'", "", False)
        End If

        'If optExisting.Checked Then
        '    strType = "E"
        'End If
        If optMasters.Checked Then strType = "M"
        If optTrnHdr.Checked Then strType = "T"
        If optAdhoc.Checked Then strType = "A"

        frmRptCRFldSel.Text = "Field Selection for " & cboSel.Text
        frmRptCRFldSel.FillFldSettings(strType, cboSel.Text)
        frmRptCRFldSel.ShowDialog()
        'txtView.Text = ""
    End Sub

    Private Sub frmRptCRDesign_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        btnDelete.Visible = False

        initGridStyles(Me)
        strRights = getUserRights(GetMenuId(Me.Text))
        ' Edited
        If StrValue <> "" Then
            optExisting.Checked = True
        Else
            optMasters.Checked = True
        End If
        cboSel.Text = StrValue
        frmRptCRFinal.strvalue = StrValue
    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If Not chkUserRights(strRights, chkRightsPos.DelRights, Me.Text) Then Exit Sub
        Dim intRptID As Integer
        If MsgBox("Do you want To Delete this Report ?", MsgBoxStyle.Critical + MsgBoxStyle.YesNo, Me.Text) = MsgBoxResult.No Then
            Exit Sub
        End If
        intRptID = clsSvt.getDataLongValue("Select CRID from UtlReportsCRHdr where CRName = '" & cboSel.Text & "'", 0, False)
        If intRptID = 0 Then
            MsgBox("Invalid report!", MsgBoxStyle.Critical, Me.Text)
            Exit Sub
        End If
        Call clsSvt.execSQLStatement("Delete from UtlReportsCRHdr where CRID  = " & intRptID)
        Call clsSvt.execSQLStatement("Delete from UtlReportsCRDtl where CRID  = " & intRptID)
        Call clsSvt.execSQLStatement("Delete from UtlReportsCRTbl where CRID  = " & intRptID)
        MsgBox("Deleted Report Successfully!", MsgBoxStyle.Information, Me.Text)
        Call bindCombo(cboSel, "CRID", "CRNAME", "Select CRID, CRName from UtlReportsCRHdr order by CRNAME")
        cboSel.Text = ""
    End Sub

    Private Sub optAdhoc_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles optAdhoc.CheckedChanged
        If optAdhoc.Checked = True Then
            Call enabledisable(True)
            btnDelete.Visible = False
            pnlExp.Visible = False
            btnExport.Visible = False
            lblSel.Text = "Select View"
            cboSel.Text = ""
            Call bindCombo(cboSel, "", "Name", "Select Name from sys.views order by 1")
        End If
    End Sub

    Private Sub btnLoad_Click(sender As Object, e As EventArgs) Handles btnLoad.Click

        txtView.Text = clsSvt.getDataStringValue("select VIEW_DEFINITION from INFORMATION_SCHEMA.VIEWS where table_name='" & cboSel.Text & "'", "", False)
        txtView.Text = CStr(txtView.Text).ToUpper().Replace("CREATE", "ALTER")
        txtView.Text = Trim(txtView.Text)

    End Sub

    Private Sub enabledisable(ByVal argbln As Boolean)
        btnLoad.Visible = argbln
        lblCreateView.Visible = argbln
        txtView.Visible = argbln
        txtView.Text = ""
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles btnClear.Click
        txtView.Text = ""
    End Sub

    Private Sub btnExport_Click(sender As Object, e As EventArgs) Handles btnExport.Click
        Dim intCrID As Int32

        If cboSel.Text = "" Then
            MsgBox("Please select existing report", vbInformation, Me.Text)
            cboSel.Focus()
            Exit Sub
        End If

        intCrID = clsSvt.getDataLongValue("Select crID from UtlReportsCRHdr where crName = '" & cboSel.Text & "'", 0, False)
        If intCrID <= 0 Then
            MsgBox("Invalid document", vbInformation, Me.Text)
            Exit Sub
        End If

        txtFileName.Text = ""
        pnlExp.Dock = DockStyle.Fill
        pnlExp.Visible = True
    End Sub

    Private Sub btnExpClose_Click(sender As Object, e As EventArgs) Handles btnExpClose.Click
        pnlExp.Visible = False
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        If txtFileName.Text = "" Then
            MsgBox("Please enter file name", vbInformation, Me.Text)
            Exit Sub
        End If

        If File.Exists(Application.StartupPath & "\" & "Temp" & "\" & Trim(txtFileName.Text) & ".ini") Then
            MsgBox("File already exist", vbInformation, Me.Text)
            Exit Sub
        End If

        If FillVsfExp() Then
            MsgBox("Saved successfully", vbInformation, Me.Text)
            Process.Start(Application.StartupPath & "\" & "Temp")
            txtFileName.Text = ""
            pnlExp.Visible = False
        End If
    End Sub

    Private Function FillVsfExp() As Boolean
        FillVsfExp = False

        Dim crID As Int32
        Dim strQry As String = ""
        Dim rs As oledb.oledbDataReader

        With vsfExp
            .Rows.Count = 0
            .Cols.Count = 0
            .Rows.Count = vExpRowTol
            .Cols.Count = vExpColTol
            .SetData(vExpRowCaption, vExpColQry, "Export UDR report")
            '.ClipSeparators = ";" & vbCr
        End With

        crID = clsSvt.getDataLongValue("Select crID from UtlReportsCRHdr where crName = '" & cboSel.Text & "'", 0, False)

        rs = clsSvt.getDataReaderFromSQL("Select * from UtlReportsCRHdr where crID = " & crID & "")

        With rs
            .Read()
            vsfExp.SetData(vExpRowCRName, vExpColQry, !CRName)

            strQry = "Insert into UtlReportsCRHdr (CRID, CRName, CRDesc, HdrTblName, ReportGroup, RptFileName, CondText, isSlNoReq) values ("
            strQry &= "<<CRID>>, "
            strQry &= "'" & GetVal(!CRName) & "', "
            strQry &= "'" & GetVal(!CRDesc) & "', "
            strQry &= "'" & GetVal(!HdrTblName) & "', "
            strQry &= "'" & GetVal(!ReportGroup) & "', "
            strQry &= "'" & GetVal(!RptFileName) & "', "
            strQry &= "'" & GetVal(!CondText) & "', "
            strQry &= "'" & GetVal(!isSlNoReq) & "')"
            .Close()
        End With

        vsfExp.SetData(vExpRowHdr, vExpColQry, strQry)

        rs = clsSvt.getDataReaderFromSQL("Select * from UtlReportsCRdtl where crID = " & crID & "")
        With rs
            strQry = "Insert into UtlReportsCRdtl      (CRID,TblName,FldCode,FldCaption,TotReq,FilterReq,FldAggregate,SubTotOn,FldOrderBy,FldDataType,FldHideCol,AddnlCol) Values "
            Do While .Read()
                strQry &= "( <<CRID>>, "
                strQry &= "'" & GetVal(!TblName) & "', "
                strQry &= "'" & GetVal(!FldCode) & "', "
                strQry &= "'" & GetVal(!FldCaption) & "', "
                strQry &= "'" & GetVal(!TotReq) & "', "
                strQry &= "'" & GetVal(!FilterReq) & "', "
                strQry &= "'" & GetVal(!FldAggregate) & "', "
                strQry &= "'" & GetVal(!SubTotOn) & "', "
                strQry &= "'" & GetVal(!FldOrderBy) & "', "
                strQry &= "'" & GetVal(!FldDataType) & "', "
                strQry &= "'" & GetVal(!FldHideCol) & "', "
                strQry &= "'" & GetVal(!AddnlCol) & "'),"
            Loop
            strQry = strQry.Remove(strQry.Length - 1)
            .Close()
        End With

        vsfExp.SetData(vExpRowDtl, vExpColQry, strQry)

        rs = clsSvt.getDataReaderFromSQL("Select * from UtlReportsCRTbl where crID = " & crID & "")
        With rs
            strQry = "Insert into UtlReportsCRTbl (CRID,TblName,TblLinkCond) Values"
            Do While .Read()
                strQry &= "( <<CRID>>, "
                strQry &= "'" & GetVal(!TblName) & "', "
                strQry &= "'" & GetVal(!TblLinkCond) & "'),"
            Loop
            strQry = strQry.Remove(strQry.Length - 1)
            .Close()
        End With

        vsfExp.SetData(vExpRowTbl, vExpColQry, strQry)

        vsfExp.SaveGrid(Application.StartupPath & "\" & "Temp" & "\" & Trim(txtFileName.Text) & ".ini", FileFormatEnum.TextTab)

        FillVsfExp = True
    End Function
End Class