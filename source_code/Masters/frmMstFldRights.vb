Imports System.ComponentModel
Imports System.Data.oledb
Imports System.IO
Imports C1.Win.C1FlexGrid

Public Class frmMstFldRights
    Dim intTotRec As Integer

    Dim clsSVT As New svtLibrary
    Dim strSQL As String
    Dim strTable As String
    Dim strAllFlds As String

    Const vDetType = 0
    Const vDetSection = 1
    Const vDetFldName = 2
    Const vDetFldCode = 3
    Const vDetRights = 4
    Const vTot = 5

    Public argForm As Object
    Public argVSF As C1.Win.C1FlexGrid.C1FlexGrid
    Dim Str As String = " |Opt 1|Opt 2|Opt 3|Opt 4|Opt 5"

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub initScreen()

        Call initFormSize(Me, Me.Text, False, 554, 490, 0, 0)
        Call initFormVsfDet(Me, Me.Text, vsfDet, "DET")
        initGridStyles(Me)
        fillFormProperties(Me)

        With vsfDet
            .Rows.Count = 1
            .Cols.Count = vTot

            .SetData(0, vDetType, "")
            .SetData(0, vDetFldName, "Field Caption")
            .SetData(0, vDetFldCode, "Field Code")
            .SetData(0, vDetRights, "Rights")
            .SetData(0, vDetSection, "Section")
            .Cols(vDetRights).ComboList = Str
        End With
        SetHideCol(vsfDet, True, vDetFldCode)
    End Sub

    Private Sub frmMstFldRights_Load(sender As Object, e As EventArgs) Handles Me.Load
        Call initScreen()
        Call fillCombo()
        Me.StartPosition = FormStartPosition.CenterScreen
        Call ChangeFontSize(Me)

    End Sub

    Private Sub vsfDet_BeforeEdit(sender As Object, e As RowColEventArgs) Handles vsfDet.BeforeEdit
        With vsfDet
            .ComboList = ""
            Select Case .Col
                Case vDetRights
                Case Else
                    e.Cancel = True
            End Select
        End With
        'e.Cancel = True

    End Sub

    Private Sub cboDocument_Validating(sender As Object, e As CancelEventArgs) Handles cboDocument.Validating
        stringReplace(Me)

        If Trim(cboDocument.Text) = "" Then
            vsfDet.Rows.Count = 1
            Exit Sub
        End If
        Dim intDocID As Integer
        Dim blnFound As Boolean
        intDocID = clsSVT.getDataLongValue("select DocID from UtlTransactions where Upper(DocName) = '" & UCase(cboDocument.Text) & "'", 0, blnFound)
        txtID.Text = intDocID

        If Not blnFound Then
            MsgBox("DocName Not found in the list!!", MsgBoxStyle.Information, Me.Text)
            vsfDet.Rows.Count = 1
            cboDocument.Text = ""
            cboDocument.Focus()
            Exit Sub
        End If
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Dim svtTrn As oledb.oledbTransaction = Nothing
        Dim strSQL As String

        stringReplace(Me)

        If Trim(cboDocument.Text) = "" Then
            MsgBox("Select Document...", MsgBoxStyle.Exclamation, Me.Text)
            cboDocument.Focus()
            Exit Sub
        End If

        With vsfDet
            If .Rows.Count <= 1 Then
                MsgBox("Grid Empty...", MsgBoxStyle.Critical, Me.Text)
                vsfDet.Focus()
                Exit Sub
            End If

            Try
                svtTrn = svtConn.BeginTransaction(IsolationLevel.ReadCommitted)

                strSQL = " Delete from UtlMstSpecialRights where DocID = " & Val(txtID.Text)
                clsSVT.execSQLStatement(strSQL, svtTrn)

                For i As Int32 = 1 To .Rows.Count - 1
                    If Trim(.GetData(i, vDetRights)) <> "" Then
                        strSQL = "Insert into UtlMstSpecialRights (DocID, FldCode, Std, RightsNumber) values("
                        strSQL &= Val(txtID.Text) & ","
                        strSQL &= "'" & .GetData(i, vDetFldCode) & "',"
                        strSQL &= "'" & Mid(.GetData(i, vDetType), 1, 1) & "',"
                        strSQL &= Strings.Right(.GetData(i, vDetRights), 1) & ") "
                        If Not clsSVT.execSQLStatement(strSQL, svtTrn) Then
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

    Private Sub frmMstFldRights_KeyUp(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyUp
        Dim blnFnKey As Boolean = False
        Select Case e.KeyCode
            Case Keys.F1, Keys.F2, Keys.F3, Keys.F4, Keys.F5, Keys.F6, Keys.F7, Keys.F8, Keys.F9, Keys.F10, Keys.F11, Keys.F12
                blnFnKey = True
        End Select
        If e.KeyCode = Keys.F9 And e.Control = True Then
            '958, 579
            Call initFormSize(Me, Me.Text, True, 474, 471, 0, 0)
            Call initFormVsfDet(Me, Me.Text, vsfDet, "DET")
            MsgBox("Restored Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
        If e.KeyCode = Keys.F8 And e.Control = True Then
            Call SaveActFormSettings(Me, Me.Text)
            Call SaveActFormVsfDet(Me, Me.Text, "DET", vsfDet)
            MsgBox("Saved Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
    End Sub

    Private Sub FillColumns(ByVal argDOCID As Int32)

        Dim strSQL As String = ""

        If rdoMst.Checked Then
            strSQL = " Select 'Standard' as FldType, c.Name as FldCaption, c.Name as FldCode,"
            strSQL &= " Case when rightsnumber Is null then '' else "
            strSQL &= " case rightsnumber when 1 then 'Opt 1' When 2 then 'Opt 2' When 3 then 'Opt 3' When 4 then 'Opt 4' else 'Opt 5' End End as UserRights  "
            strSQL &= " from Sys.Columns c "
            strSQL &= " Join Sys.Tables t on c.Object_id=t.Object_id "
            strSQL &= " Left Join UtlmstSpecialRights sr on c.Name=sr.FldCode "
            strSQL &= " Join sys.extended_properties sep on t.object_id = sep.major_id and c.column_id = sep.minor_id and sep.name = 'MS_Description'  "
            strSQL &= " Where t.Name = (Select HdrTblName from UtlTransactions where DocID=" & Val(argDOCID) & ") "
            strSQL &= " and sep.value='SR' "
            strSQL &= " Union All "
            strSQL &= " Select 'Designer' as FldType, Case When f.FldCode Is Null Then c.Name  else f.FldCaption End as FldCaption, c.Name as FldCode, "
            strSQL &= " Case when rightsnumber Is null then '' else  "
            strSQL &= " case rightsnumber when 1 then 'Opt 1' When 2 then 'Opt 2' When 3 then 'Opt 3' When 4 then 'Opt 4' else 'Opt 5' End End as UserRights "
            strSQL &= " from Sys.Columns c "
            strSQL &= " Left Join Sys.Tables t on c.Object_id=t.Object_id "
            strSQL &= " Join UtlDocDsgrFlds f on c.Name=f.FldCode And f.DocID=" & Val(argDOCID)
            strSQL &= " Left Join UtlmstSpecialRights sr on c.Name=sr.FldCode "
            strSQL &= " Where t.Name = (Select HdrTblName from UtlTransactions where DocID=" & Val(argDOCID) & ") "
            strSQL &= " Order by FldType, FldCaption "

        Else
            strSQL &= " Select 'Designer' as FldType, FldSection, FldCaption, f.FldCode, "
            strSQL &= " Case when rightsnumber Is null then '' else  "
            strSQL &= " case rightsnumber when 1 then 'Opt 1' When 2 then 'Opt 2' When 3 then 'Opt 3' When 4 then 'Opt 4' else 'Opt 5' End End as UserRights "
            strSQL &= " from UtlDocDsgrFlds f "
            strSQL &= " Left Join UtlmstSpecialRights sr on f.fldcode=sr.FldCode  "
            strSQL &= " Where f.DocID=" & Val(argDOCID)
            strSQL &= " Order by FldCaption "
        End If



        Dim rs As oledb.oledbDataReader
        rs = clsSVT.getDataReaderFromSQL(strSQL)

        If Not rs.HasRows Then
            rs.Close()
            MsgBox("Table is Empty...", MsgBoxStyle.Information, Me.Text)
            cboDocument.Focus()
            Exit Sub
        End If

        With vsfDet
            .Rows.Count = 1
            Do While rs.Read
                .Rows.Count = .Rows.Count + 1
                .SetData(.Rows.Count - 1, vDetFldName, rs!FldCaption)
                .SetData(.Rows.Count - 1, vDetFldCode, rs!FldCode)
                .SetData(.Rows.Count - 1, vDetType, rs!FldType)
                .SetData(.Rows.Count - 1, vDetRights, rs!UserRights)
                If rdoTrans.Checked Then .SetData(.Rows.Count - 1, vDetSection, rs!FldSection)
            Loop
        End With
        rs.Close()
        vsfDet.Cols(vDetType).AllowMerging = True
        If rdoMst.Checked Then
            vsfDet.Cols(vDetSection).Visible = False
        Else
            vsfDet.Cols(vDetSection).Visible = True
        End If
        vsfDet.AutoSizeCols()
    End Sub

    Private Sub btnLoad_Click(sender As Object, e As EventArgs) Handles btnLoad.Click
        stringReplace(Me)

        If Trim(cboDocument.Text) = "" Then
            vsfDet.Rows.Count = 1
            Exit Sub
        End If

        Dim blnFound As Boolean
        Dim intDocID As Integer
        intDocID = clsSVT.getDataLongValue("select DocID from UtlTransactions where upper(DocName) = '" & UCase(cboDocument.Text) & "'", 0, blnFound)
        txtID.Text = intDocID

        If Not blnFound Then
            MsgBox("DocName Not found in the list!!", MsgBoxStyle.Information, Me.Text)
            vsfDet.Rows.Count = 1
            cboDocument.Text = ""
            cboDocument.Focus()
            Exit Sub
        End If

        Call FillColumns(intDocID)
    End Sub

    Private Sub fillCombo()
        Dim strSQL As String = ""
        If rdoMst.Checked Then
            strSQL &= " Select distinct u.DocName as DocName"
            strSQL &= " From Sys.Columns c "
            strSQL &= " Join Sys.Tables t on c.Object_id=t.Object_Id "
            strSQL &= " Left Join UtlTransactions u on t.Name=u.HdrTblName and u.DocType='Master' "
            strSQL &= " Left Join sys.extended_properties sep on t.object_id = sep.major_id and c.column_id = sep.minor_id and sep.name = 'MS_Description' "
            strSQL &= " Where sep.value='SR' "
        Else
            strSQL &= "Select DocName from UtlTransactions Where DocType = 'Transaction' Order by 1 "
        End If

        bindCombo(cboDocument, "DocName", "DocName", strSQL)
        cboDocument.Text = ""
        cboDocument.Focus()
    End Sub

    Private Sub rdoMst_CheckedChanged(sender As Object, e As EventArgs) Handles rdoMst.CheckedChanged
        Call fillCombo()
    End Sub

    Private Sub rdoTrans_CheckedChanged(sender As Object, e As EventArgs) Handles rdoTrans.CheckedChanged
        Call fillCombo()
    End Sub


End Class