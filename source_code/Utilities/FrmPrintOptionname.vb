Imports System.Data.OleDb
Imports C1.Win.C1FlexGrid

Public Class FrmPrintOptionname


    'For Grid vsfPrintoptions
    Const vsfvsfdtlCol = 3
    Const vFormulaField = 0
    Const vTable = 1
    Const vFormulaValue = 2

    Dim strSQL As String
    Dim clsSVT As New svtLibrary

    Private Sub FrmPrintOptionname_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'Initialize with Default columns in Grid and grid Styles
        initGridStyles(Me)
        initGrid()
        Call initFormVsfDet(Me, Me.Text, vsfdtl, "DET")
        'Initialize Form Properties with default settings
        fillFormProperties(Me)
        'Fill Data
        If btnSave.Text = "&Update" Then
            Call clearflds()
            Call LoadFormList()
        ElseIf btnSave.Text = "&Save" Then
            Call LoadFormnew()
        End If
        vsfdtl.Rows.Count = vsfdtl.Rows.Count + 1

    End Sub




    Private Function LoadFormList() As Boolean

        Dim strSQL As String
        Dim arrFrmlFld, arrFrmlVal As Array
        Dim cmdDummy As New OleDbCommand
        Dim rsDummy As OleDbDataReader
        Dim ipos As Integer
        Try
            LoadFormList = False

            strSQL = "Select * from utloption"
            strSQL = strSQL & " Where DocID = " & Trim(txtid.Text) & " and OptionOrder=" & Trim(txtordrno.Text) & " order By OptionOrder"
            rsDummy = clsSVT.getDataReaderFromSQL(strSQL)

            With rsDummy
                If Not rsDummy.HasRows Then
                    rsDummy.Close()
                    Exit Function
                End If
                If .Read() Then
                    txtcopies.Text = rsDummy.Item("NoOfCopies")
                    If GetVal(rsDummy.Item("DefaultPrint"), "") = "Y" Then Chkdefault.Checked = True
                    If GetVal(rsDummy.Item("PrintType"), "") = "Preview" Then
                        rdoPrvw.Checked = True
                    ElseIf GetVal(rsDummy.Item("PrintType"), "") = "Direct" Then
                        RdoDrctPrnt.Checked = True
                    ElseIf GetVal(rsDummy.Item("PrintType"), "") = "PDF" Then
                        Rdopdf.Checked = True
                    End If
                    If GetVal(Trim(rsDummy.Item("PrintAfterSave")), "") = "P" Then
                        Rdoprompt.Checked = True
                    ElseIf GetVal(Trim(rsDummy.Item("PrintAfterSave")), "") = "PP" Then
                        Rdowithoutprmpt.Checked = True
                    ElseIf GetVal(Trim(rsDummy.Item("PrintAfterSave")), "") = "DNP" Then
                        Rdodntprnt.Checked = True
                    End If
                    If rsDummy.Item("DefaultPrint") = "Y" Then
                        Chkdefault.Checked = True
                    Else
                        Chkdefault.Checked = False
                    End If
                    Dim fileNames = My.Computer.FileSystem.GetFiles(gstrRptPath, FileIO.SearchOption.SearchTopLevelOnly, "*.rpt")
                    cborpt.Items.Clear()
                    cborpt.Text = rsDummy.Item("CrptFileName")
                    For Each fileName As String In fileNames
                        fileName = Replace(fileName, gstrRptPath, "")
                        cborpt.Items.Add(fileName)
                    Next
                    txtfrmla.Text = GetVal(rsDummy.Item("SelFormula"), "")
                End If

                arrFrmlFld = Split(rsDummy.Item("FormulasFld"), ";")
                arrFrmlVal = Split(rsDummy.Item("Formulasval"), ";")

                For i = 0 To UBound(arrFrmlFld)
                    With vsfdtl
                        .Rows.Count = .Rows.Count + 1
                        .GetData(i, vFormulaField)
                        .SetData(.Rows.Count - 1, vFormulaField, arrFrmlFld(i))
                        ipos = InStr(1, arrFrmlVal(i), ".")
                        .SetData(.Rows.Count - 1, vTable, Strings.Left(arrFrmlVal(i), ipos - 1))
                        .SetData(.Rows.Count - 1, vFormulaValue, Mid(arrFrmlVal(i), ipos + 1))
                    End With
                Next i

            End With

            rsDummy.Close()
            LoadFormList = True
            Exit Function
        Catch ex As Exception
            MsgBox("Error during save" & vbCrLf & ex.Message, MsgBoxStyle.Critical, Me.Text)
            LoadFormList = False
        End Try
    End Function

    Private Sub LoadFormnew()
        Dim fileNames = My.Computer.FileSystem.GetFiles(gstrRptPath, FileIO.SearchOption.SearchTopLevelOnly, "*.rpt")
        cborpt.Items.Clear()
        cborpt.Text = ""
        For Each fileName As String In fileNames
            fileName = Replace(fileName, gstrRptPath, "")
            cborpt.Items.Add(fileName)
        Next
        rdoPrvw.Checked = True
        Rdodntprnt.Checked = True
    End Sub

    Private Sub clearflds()
        Chkdefault.Checked = False
        rdoPrvw.Checked = False
        RdoDrctPrnt.Checked = False
        Rdopdf.Checked = False
        chkoutlk.Checked = False
        Rdoprompt.Checked = False
        Rdowithoutprmpt.Checked = False
        Rdodntprnt.Checked = False
    End Sub

    Private Sub initGrid()
        With vsfdtl
            .Rows.Count = 1
            .Cols.Count = vsfvsfdtlCol
            '.Cols(vFormulaField).ComboList = ""
            .SetData(0, vFormulaField, "Formula Field")
            '.Cols(vTable).ComboList = ""
            .SetData(0, vTable, "Table")
            .SetData(0, vFormulaValue, "Formula Value")
        End With

    End Sub

    Private Sub EnableCntrls(argType As Integer, argBln As Boolean)
        'Enable Controls
        Select Case argType
            Case 1
                btnSave.Visible = argBln
                btnCancel.Visible = argBln
            Case 2
                btnSave.Visible = argBln
                btnCancel.Visible = argBln
        End Select
    End Sub

    Private Sub RdoDrctPrnt_CheckedChanged(sender As Object, e As EventArgs) Handles RdoDrctPrnt.CheckedChanged
        printtypechanged()
    End Sub

    Private Sub Rdopdf_CheckedChanged(sender As Object, e As EventArgs) Handles Rdopdf.CheckedChanged
        printtypechanged()
    End Sub

    Private Sub Rdoprvw_CheckedChanged(sender As Object, e As EventArgs) Handles rdoPrvw.CheckedChanged
        printtypechanged()
    End Sub

    Private Sub printtypechanged()
        If RdoDrctPrnt.Checked = True Then
            lblprtname.Visible = True
            txtprntname.Visible = True
            chkoutlk.Visible = False
        ElseIf Rdopdf.Checked = True Then
            chkoutlk.Visible = True
            lblprtname.Visible = False
            txtprntname.Visible = False
        Else
            lblprtname.Visible = False
            txtprntname.Visible = False
            chkoutlk.Visible = False
        End If

    End Sub

    Private Sub FrmPrintOptionname_KeyUp(sender As Object, e As KeyEventArgs) Handles Me.KeyUp
        If e.KeyCode = Keys.F9 And e.Control = True Then
            Call initFormSize(Me, Me.Text, True, 594, 505, 0, 0)
            Call initFormVsfDet(Me, Me.Text, vsfdtl, "DET")
            MsgBox("Restored Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
        If e.KeyCode = Keys.F8 And e.Control = True Then
            Call SaveActFormSettings(Me, Me.Text)
            Call SaveActFormVsfDet(Me, Me.Text, "DET", vsfdtl)
            MsgBox("Saved Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub


    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Dim svtTrn As OleDb.OleDbTransaction = Nothing
        Try
            Dim PrtType As String = ""
            Dim Printsave As String = ""
            Dim OpnOutlk As String = ""
            Dim defltprint As String = ""
            If txtprtname.Text = "" Then Exit Sub
            If rdoPrvw.Checked = True Then
                PrtType = "Preview"
            ElseIf RdoDrctPrnt.Checked = True Then
                PrtType = "Direct"
            ElseIf RdoDrctPrnt.Checked = True Then
                PrtType = "PDF"
            End If

            If Rdoprompt.Checked = True Then
                Printsave = "P"
            ElseIf Rdowithoutprmpt.Checked = True Then
                Printsave = "PP"
            ElseIf Rdodntprnt.Checked = True Then
                Printsave = "DNP"
            End If
            '
            If Chkdefault.Checked Then
                defltprint = "Y"
                strSQL = ""
                svtTrn = svtConn.BeginTransaction(IsolationLevel.ReadCommitted)
                strSQL = "Update  UTLOption set DefaultPrint='N' where DocID = " & Val(txtid.Text) & ""
                strSQL = strSQL & " and OptionOrder <> " & Val(txtordrno.Text) & ""
                clsSVT.execSQLStatement(strSQL, svtTrn)
                svtTrn.Commit()
            End If

            If btnSave.Text = "&Save" Then
                svtTrn = svtConn.BeginTransaction(IsolationLevel.ReadCommitted)
                strSQL = "insert into UTLOption ("
                strSQL = strSQL & " DocID,OptionOrder,OptionName,PrintType,CrptFileName,"
                strSQL = strSQL & " SelFormula,FormulasFld,FormulasVal,NoOfCopies"
                strSQL = strSQL & " ,Defaultprint,DirectPrinterName,OpenInOutlook,PrintAfterSave) values ("
                strSQL = strSQL & "" & txtid.Text & ""
                strSQL = strSQL & "," & clsSVT.getDataLongValue("select isnull(max(Optionorder),0) + 1 from utloption where DocID=" & Val(txtid.Text), 1, False, svtTrn) & ""
                strSQL = strSQL & ",'" & Trim(txtprtname.Text) & "'" 'OptionName
                strSQL = strSQL & ",'" & PrtType & "'" 'PrintType
                strSQL = strSQL & ",'" & Trim(cborpt.Text) & "'" 'CrptFileName
                strSQL = strSQL & ",'" & Trim(txtfrmla.Text) & "'"  'SelFormula
                strSQL = strSQL & ",'" & Trim(txtfrmla.Text) & "'"
                strSQL = strSQL & ",'" & Trim(txtfrmla.Text) & "'"
                strSQL = strSQL & "," & Val(txtcopies.Text) & ""  'noofcopies
                strSQL = strSQL & ",'" & defltprint & "'"
                strSQL = strSQL & ",'" & txtprntname.Text & "'"
                strSQL = strSQL & "," & OpnOutlk & "'"
                strSQL = strSQL & "','" & Printsave & "')" '
                clsSVT.execSQLStatement(strSQL, svtTrn)
                svtTrn.Commit()
                MsgBox("Data Saved Successfully!", vbInformation, Me.Text)
            ElseIf btnSave.Text = "&Save" Then
                svtTrn = svtConn.BeginTransaction(IsolationLevel.ReadCommitted)
                strSQL = "insert into UTLOption ("
                strSQL = strSQL & " DocID,OptionOrder,OptionName,PrintType,CrptFileName,"
                strSQL = strSQL & " SelFormula,FormulasFld,FormulasVal,NoOfCopies"
                strSQL = strSQL & " ,Defaultprint,DirectPrinterName,OpenInOutlook,PrintAfterSave) values ("
                strSQL = strSQL & "" & txtid.Text & ""
                strSQL = strSQL & "," & clsSVT.getDataLongValue("select isnull(max(Optionorder),0) + 1 from utloption where DocID=" & Val(txtid.Text), 1, False, svtTrn) & ""
                strSQL = strSQL & ",'" & Trim(txtprtname.Text) & "'" 'OptionName
                strSQL = strSQL & ",'" & PrtType & "'" 'PrintType
                strSQL = strSQL & ",'" & Trim(cborpt.Text) & "'" 'CrptFileName
                strSQL = strSQL & ",'" & Trim(txtfrmla.Text) & "'"  'SelFormula
                strSQL = strSQL & ",'" & Trim(txtfrmla.Text) & "'"
                strSQL = strSQL & ",'" & Trim(txtfrmla.Text) & "'"
                strSQL = strSQL & "," & Val(txtcopies.Text) & ""  'noofcopies
                strSQL = strSQL & ",'" & defltprint & "'"
                strSQL = strSQL & ",'" & txtprntname.Text & "'"
                strSQL = strSQL & "," & OpnOutlk & "'"
                strSQL = strSQL & "','" & Printsave & "')" '
                clsSVT.execSQLStatement(strSQL, svtTrn)
                svtTrn.Commit()
                MsgBox("Updated Successfully!", vbInformation, Me.Text)
            End If

            Exit Sub
        Catch ex As Exception
            MsgBox("Error during save" & vbCrLf & ex.Message, MsgBoxStyle.Critical, Me.Text)
            svtTrn.Rollback()
        End Try
    End Sub

    Private Sub btnremv_Click(sender As Object, e As EventArgs) Handles btnremv.Click
        vsfdtl.Rows.Remove(vsfdtl.Row)
    End Sub

    Private Sub vsfdtl_BeforeEdit(sender As Object, e As RowColEventArgs) Handles vsfdtl.BeforeEdit
        If e.Col = vTable Then
            vsfdtl.ComboList = GenVSfComboStringCond("table_name", "Information_schema.tables", "Table_name")
            Exit Sub
        End If

        If e.Col = vFormulaValue Then
            vsfdtl.ComboList = GenVSfComboStringCond("column_name", "Information_schema.Columns", " order by Column_name", " where table_name='" & vsfdtl.GetData(vsfdtl.Row, vTable) & "'")
            Exit Sub
        End If
    End Sub

    Private Sub vsfdtl_Click(sender As Object, e As EventArgs) Handles vsfdtl.Click

    End Sub
End Class