Imports C1.Win.C1FlexGrid

Public Class frmUtlEnv
    Dim clsSvt As New svtLibrary
    Public strDocName As String
    Public intPartyId As Integer
    Dim StrTbleName As String
    Dim StrIdName As String
    Dim strRptName As String
    Const vdetFldSettings = 0
    Dim _src As C1FlexGrid = Nothing

    Private Sub btnDesign_Click(sender As Object, e As EventArgs) Handles btnDesign.Click
        Dim objFrm As New frmutlEnvDesign
        objFrm.strDocName = Me.strDocName
        objFrm.Text = objFrm.Text & " - " & Me.Text
        objFrm.ShowDialog()
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()

    End Sub

    Private Sub frmUtlEnv_Load(sender As Object, e As EventArgs) Handles Me.Load

        StrTbleName = clsSvt.getDataStringValue("select HdrTblName from UtlTransactions where DocName='" & Trim(UCase(strDocName)) & "'", "", False)
        Dim strTemp As String = StrTbleName
        strTemp = Trim(Replace(UCase(strTemp), "MST", ""))
        strTemp = Trim(Replace(UCase(strTemp), "TRNHDR", ""))
        strTemp = Trim(Replace(UCase(strTemp), "TRNDTL", ""))
        StrIdName = strTemp & "ID"
        Call bindCombo(CboTemplate, "TemplateName", "TemplateName", "select distinct TemplateName from UtlPartyEnv where templateName <>'' and DocName='" & strDocName & "' order by templateName ")
        btnPrint.Visible = False
        If CboTemplate.Items.Count = 1 Then
            btnPrint.Visible = True
            Call fillgridData()
            'btnPrint_Click(sender, e)
        End If

        With vsfDet
            .Rows.Count = 21
            .Cols.Count = 1
            .SetData(0, vdetFldSettings, "Printing Values")
            .DragMode = DragModeEnum.Manual
            .DropMode = DropModeEnum.Manual
        End With
        Call SetColAlign(vsfDet, svtAlign.Left, vdetFldSettings)
        Call ucBtnEnabledisable()
    End Sub

    Private Sub ucBtnEnabledisable()
        With ucBtnCntrl
            .btnVisible("", False, False)
            .tsbExport.Visible = False
            .tsbFreeze.Visible = False
            .tsbDocExport.Visible = False
            .tsbCopyFrom.Visible = False
            .tsbCopy.Visible = False
            .tsbPaste.Visible = False
            .tsbCalTotal.Visible = False
            .tsbMultiDoc.Visible = False
            .tsbBOM.Visible = False
            .tsbInserDoc.Visible = False
            .tsbImport.Visible = False
            .tsbTemplate.Visible = False
            .tsbAmmend.Visible = False
            .tsbTender.Visible = False
            .tsbMoveUp.Visible = True
            .tsbMoveDown.Visible = True
            .tsbRemove.Visible = True
        End With
    End Sub

    Private Sub CboTemplate_KeyPress(sender As Object, e As KeyPressEventArgs) Handles CboTemplate.KeyPress
        If e.KeyChar = "'" Then
            e.Handled = True
        End If
    End Sub

    Private Sub btnPrint_Click(sender As Object, e As EventArgs) Handles btnPrint.Click
        If CboTemplate.Text = "" Then Exit Sub
        Dim svtTrn As oledb.oledbTransaction
        Dim strSql As String

        Cursor.Current = Cursors.WaitCursor

        svtTrn = svtConn.BeginTransaction(IsolationLevel.ReadCommitted)
        Try
            Call clsSvt.execSQLStatement("Delete from TblPartyEnv where UserName ='" & Trim(UCase(gUserCode)) & "'", svtTrn)

            strSql = " Insert Into TblPartyEnv( "
            strSql = strSql & " Fld1,Fld2,Fld3,Fld4,Fld5,Fld6,Fld7,Fld8,Fld9,Fld10,Fld11,Fld12,"
            strSql = strSql & " Fld13,Fld14,Fld15,Fld16,Fld17,Fld18,Fld19,Fld20,UserName"
            strSql = strSql & " ) values("
            For i = 1 To vsfDet.Rows.Count - 1
                strSql = strSql & " '" & Trim(vsfDet.GetData(i, vdetFldSettings)).Replace("'", "`") & "',"
            Next
            strSql = strSql & " '" & Trim(UCase(gUserCode)) & "')"
            If Not clsSvt.execSQLStatement(strSql, svtTrn) Then
                svtTrn.Rollback()
                Exit Sub
            End If
            svtTrn.Commit()
            Dim strSF As String = "{MstUser.USERID} = " & gUserID
            Dim frmv As New frmUtlReportViewer
            With frmv
                .StartPosition = FormStartPosition.CenterScreen
                .showRptFile(Me.Text, gstrRptPath & strRptName, strSF, Nothing, False)
                .ShowDialog()
            End With
            Cursor.Current = Cursors.Default
        Catch ex As Exception
            MsgBox("Error during Update" & vbCrLf & ex.Message, MsgBoxStyle.Critical, Me.Text)
            svtTrn.Rollback()
            Cursor.Current = Cursors.Default
        End Try
    End Sub

    Private Sub frmUtlEnv_KeyUp(sender As Object, e As KeyEventArgs) Handles Me.KeyUp
        If e.KeyCode = Keys.F5 Then
            Call bindCombo(CboTemplate, "TemplateName", "TemplateName", "select TemplateName from UtlPartyEnv where TemplateName <>'' and DocName='" & Trim(UCase(strDocName)) & "' order by TemplateName ")
        End If

        If e.KeyCode = Keys.F9 And e.Control = True Then
            Call initFormVsfDet(Me, Me.Text, vsfDet, "DET")
            MsgBox("Restored Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
        If e.KeyCode = Keys.F8 And e.Control = True Then
            Call SaveActFormVsfDet(Me, Me.Text, "DET", vsfDet)
            MsgBox("Saved Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
        Call raiseShortCutDetEvent(e)
    End Sub

    Private Sub raiseShortCutDetEvent(ByVal e As System.Windows.Forms.KeyEventArgs)
        'Remove Row event
        If e.Control = True And e.Shift = True And e.KeyCode = Keys.R Then
            If Not vsfDet.Rows.Count > 1 Then
                MsgBox("No data to Remove", vbExclamation)
            End If
            vsfDet.SetData(vsfDet.Row, -1, "")
        End If

        'MoveUp Event
        If e.Control = True And e.Shift = True And e.KeyCode = Keys.U Then
            Call MoveOneRowUp(vsfDet, 0)
        End If

        'MoveDown Event
        If e.Control = True And e.Shift = True And e.KeyCode = Keys.V Then
            Call MoveOneRowDown(vsfDet, -1)
        End If

    End Sub


    Private Sub btnLoad_Click(sender As Object, e As EventArgs) Handles btnLoad.Click
        Call fillgridData()
    End Sub
    Private Sub fillgridData()

        Dim blnFound As Boolean
        Dim rstemp As oledb.oledbDataReader
        Dim strSql As String
        Dim i As Integer


        txtEnvID.Text = clsSvt.getDataStringValue("Select EnvID from UtlPartyEnv where TemplateName='" & CboTemplate.Text & "'", "", blnFound)
        If Not blnFound Then
            MsgBox("Template is not found", MsgBoxStyle.Information, Me.Text)
            CboTemplate.Focus()
            pnlGrid.Visible = False
            Exit Sub
        End If
        pnlGrid.Visible = True
        strSql = ("select * from UtlPartyEnv where Envid=" & Val(txtEnvID.Text))
        rstemp = clsSvt.getDataReaderFromSQL(strSql)
        Try
            With rstemp
                If .Read() Then
                    vsfDet.Rows.Count = 21
                    strRptName = GetVal(rstemp("ReportName"), "")
                    CboTemplate.Text = GetVal(rstemp("TemplateName"), "")
                    txtEnvID.Text = (GetVal(rstemp("EnvID"), 0))

                    For i = 1 To vsfDet.Rows.Count - 1
                        vsfDet.SetData((i), vdetFldSettings, GetVal(rstemp("FLDContent" & i), ""))
                    Next
                Else
                    vsfDet.Rows.Count = 1
                    vsfDet.Rows.Count = 21
                End If
                .Close()
            End With
            If vsfDet.Rows.Count > 1 Then
                btnPrint.Visible = True
                Call ReplaceData()
            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, Me.Text)
            rstemp.Close()
            Cursor.Current = Cursors.Default
        End Try
    End Sub
    Private Sub ReplaceData()
        Dim strval As String
        Dim strFlds() As String
        Dim rs As oledb.oledbDataReader
        Dim i As Integer, j As Integer
        rs = clsSvt.getDataReaderFromSQL("select * from " & StrTbleName & " where " & StrIdName & "=" & intPartyId)
        Try
            With vsfDet
                rs.Read()
                strFlds = Split(getStrFlds, ",")
                For i = 1 To vsfDet.Rows.Count - 1

                    For j = 0 To UBound(strFlds)
                        'MsgBox((UCase(vsfDet.GetData(i, vdetFldSettings))))
                        If InStr(UCase(vsfDet.GetData(i, vdetFldSettings)), UCase(strFlds(j))) > 0 Then
                            strval = Replace(strFlds(j), "{", "")
                            strval = Replace(strval, "}", "")
                            vsfDet.SetData(i, vdetFldSettings, Replace(Trim(UCase(vsfDet.GetData(i, vdetFldSettings))), strFlds(j), GetVal(rs(strval))))
                        End If
                    Next
                Next
                rs.Close()
            End With
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, Me.Text)
            rs.Close()
            Cursor.Current = Cursors.Default
        End Try
    End Sub

    Private Function getStrFlds() As String
        getStrFlds = ""
        Dim rsFldsCodes As oledb.oledbDataReader
        rsFldsCodes = clsSvt.getDataReaderFromSQL("Select Column_Name from Information_schema.Columns where Table_name='" & StrTbleName & "'")
        Try
            With rsFldsCodes
                Do While .Read()
                    getStrFlds = getStrFlds & "{" & Trim(UCase(!Column_Name)) & "}" & ","
                Loop
                getStrFlds = Mid(getStrFlds, 1, Len(getStrFlds) - 1)
                .Close()
            End With
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, Me.Text)
            rsFldsCodes.Close()
            Cursor.Current = Cursors.Default
        End Try
    End Function

    Private Sub vsfDet_SetupEditor(sender As Object, e As RowColEventArgs) Handles vsfDet.SetupEditor
        With vsfDet
            Select Case e.Col
                Case vdetFldSettings
                    If TypeOf (vsfDet.Editor) Is TextBox Then
                        Dim tb As TextBox = vsfDet.Editor
                        tb.MaxLength = 250
                    End If
            End Select
        End With
    End Sub

    Private Sub vsfDet_KeyPress(sender As Object, e As KeyPressEventArgs) Handles vsfDet.KeyPress
        If e.KeyChar = "'" Then
            e.Handled = True
        End If
    End Sub

    Private Sub frmUtlEnv_Activated(sender As Object, e As EventArgs) Handles Me.Activated
        Call bindCombo(CboTemplate, "TemplateName", "TemplateName", "select TemplateName from UtlPartyEnv where TemplateName <>'' and DocName='" & strDocName & "' order by TemplateName ")
    End Sub

    Private Sub ucBtnCntrl_btnClicked(btnValue As Integer) Handles ucBtnCntrl.btnClicked
        Try
            With ucBtnCntrl
                Select Case btnValue
                    Case svtDtlBtns.svtDown
                        Call MoveOneRowDown(vsfDet, -1)

                    Case svtDtlBtns.svtRemove
                        If Not vsfDet.Rows.Count > 1 Then
                            MsgBox("No data to Remove", vbExclamation)
                        End If
                        ' Call .vsfRemove(vsfDet, vsfDet.Row, vdetFldSettings)
                        vsfDet.SetData(vsfDet.Row, 0, "")


                    Case svtDtlBtns.svtUp
                        Call MoveOneRowUp(vsfDet, -1)

                End Select
            End With
        Catch ex As Exception

        End Try
    End Sub

    Private Sub ucBtnCntrl_Load(sender As Object, e As EventArgs) Handles ucBtnCntrl.Load

    End Sub
End Class