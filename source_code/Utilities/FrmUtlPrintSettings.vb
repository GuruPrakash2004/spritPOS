Imports System.Data.oledb
Imports C1.Win.C1FlexGrid

Public Class FrmUtlPrintSettings

    Dim intDocID As Integer

    'For Grid vsfPrintoptions
    Const vDetDocID = 0
    Const vDetDocOrder = 1
    Const vDetOptionname = 2
    Const vDetDefaultPrint = 3

    Const intTotCols = 4

    Dim strSQL As String
    Dim clsSVT As New svtLibrary


    Private Sub FrmUtlPrintSettings_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'Initialize with Default columns in Grid and grid Styles
        Call initGridStyles(Me)
        Call initGrid()
        Call initFormVsfDet(Me, Me.Text, vsfDtl, "DET")
        'Initialize Form Properties with default settings
        Call fillFormProperties(Me)
        'Combo Load
        bindCombo(cboTrnName, "DocID", "DocName", "select DocID,DocName from utltransactions where DocType='Transaction' order by DocName")
        btnFalse()
        btnClose.Visible = True
        intDocID = 0
        Call ChangeFontSize(Me)
    End Sub


    Private Sub GridLoad()
        If Trim(cboTrnName.Text) = "" Then Exit Sub
        Try
            Dim rsDummy As oledbDataReader
            strSQL = "select *  from UTLOption where   DocID = " & intDocID & " order by OptionOrder"
            rsDummy = clsSVT.getDataReaderFromSQL(strSQL)
            vsfDtl.Rows.Count = 1
            Using rsDummy
                With vsfDtl
                    If Not rsDummy.HasRows Then
                        rsDummy.Close()
                        Exit Sub
                    End If
                    .Rows.Count = 1
                    Do While rsDummy.Read
                        .Rows.Count = .Rows.Count + 1
                        .SetData(.Rows.Count - 1, vDetOptionname, GetVal(rsDummy.Item("OptionName"), ""))
                        .SetData(.Rows.Count - 1, vDetDocID, GetVal(rsDummy.Item("DocID"), 0))
                        .SetData(.Rows.Count - 1, vDetDocOrder, GetVal(rsDummy.Item("optionOrder"), 0))
                        .SetData(.Rows.Count - 1, vDetDefaultPrint, GetVal(rsDummy.Item("DefaultPrint"), ""))
                    Loop
                End With
            End Using
        Catch ex As Exception
            MsgBox("Error during Load" & vbCrLf & ex.Message, MsgBoxStyle.Critical, Me.Text & " [GridLoad]")
        End Try
    End Sub

    Private Sub initGrid()
        With vsfDtl
            .Rows.Count = 1
            .Cols.Count = intTotCols
            .SetData(0, vDetOptionname, "Print Option Name")
            .SetData(0, vDetDefaultPrint, "Default Print")
            .Cols(vDetDefaultPrint).ComboList = "Y|N"
            .SetData(0, vDetDocOrder, "Option Order")
        End With

        Call SetColAlign(vsfDtl, svtAlign.Center, vDetDocOrder)
        Call SetRowAlign(vsfDtl, svtAlign.Center, 0)
        Call SetFlexColor(vsfDtl, "", vDetOptionname, vDetDefaultPrint, vDetDocOrder)

        Call SetHideCol(vsfDtl, True, vDetDocID, vDetDocOrder)
    End Sub

    Private Sub btnMoveUP_Click(sender As Object, e As EventArgs) Handles btnMvUP.Click
        'Grid Rows move to Up
        If vsfDtl.Row > 1 Then
            Call MoveOneRowUp(vsfDtl)
            Call ReArrangeSlNo(vsfDtl, vDetDocOrder)
        End If
    End Sub

    Private Sub btnMoveDown_Click(sender As Object, e As EventArgs) Handles btnMvDwn.Click
        'Grid Rows move to Down
        If vsfDtl.Row < vsfDtl.Rows.Count - 1 Then
            Call MoveOneRowDown(vsfDtl)
            Call ReArrangeSlNo(vsfDtl, vDetDocOrder)
        End If
    End Sub



    Private Sub btnFalse()
        btnAlter.Visible = False
        PnlArrow.Visible = False
        pnlSav.Visible = False
        btnAdd.Visible = False
        pnlEdit.Visible = False
    End Sub

    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        If Trim(cboTrnName.Text) = "" Then Exit Sub
        If IsNothing(cboTrnName.SelectedValue) Then Exit Sub
        Dim objFrm As New FrmUtlPrintOptionname

        With objFrm
            .Text = "Print Option Name for " & cboTrnName.Text
            .Mode = "A"
            Call .loadOptionDetails(intDocID, "")
            .ShowDialog()
            Call Btnload_Click(sender, e)
        End With

    End Sub

    Private Sub FrmUtlPrintSettings_KeyUp(sender As Object, e As KeyEventArgs) Handles Me.KeyUp
        If e.KeyCode = Keys.F9 And e.Control = True Then
            Call initFormSize(Me, Me.Text, True, 594, 505, 0, 0)
            Call initFormVsfDet(Me, Me.Text, vsfDtl, "DET")
            MsgBox("Restored Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
        If e.KeyCode = Keys.F8 And e.Control = True Then
            Call SaveActFormSettings(Me, Me.Text)
            Call SaveActFormVsfDet(Me, Me.Text, "DET", vsfDtl)
            MsgBox("Saved Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
    End Sub

    Private Sub BtnAlterOrder_Click(sender As Object, e As EventArgs) Handles btnAlter.Click
        'Button Enable Disable
        Call btnFalse()
        If vsfDtl.Rows.Count > 2 Then
            PnlArrow.Visible = True
            pnlSav.Visible = True
            btnClose.Visible = False
        End If
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Call Btnload_Click(sender, e)
        'Button Enable Disable
        If vsfDtl.Rows.Count > 2 Then
            pnlEdit.Visible = True
            btnAdd.Visible = True
            btnAlter.Visible = True
        ElseIf vsfDtl.Rows.Count > 1 Then
            btnAdd.Visible = True
            pnlEdit.Visible = True
        End If
        btnClose.Visible = True
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Dim svtTrn As oledb.oledbTransaction = Nothing
        Try
            strSQL = ""
            svtTrn = svtConn.BeginTransaction(IsolationLevel.ReadCommitted)
            With vsfDtl
                For i = 1 To .Rows.Count - 1
                    strSQL = "Update UtlOption set "
                    strSQL = strSQL & " OptionOrder = " & GetVal(.GetData(i, vDetDocOrder), 0)
                    strSQL = strSQL & " where OptionName ='" & GetVal(.GetData(i, vDetOptionname), "") & "'"
                    strSQL = strSQL & " and DocID = " & intDocID
                    clsSVT.execSQLStatement(strSQL, svtTrn)
                Next i
            End With
            svtTrn.Commit()
            MsgBox("Saved Successfully!", vbInformation, Me.Text)
            Call Btnload_Click(sender, e)
            Exit Sub
        Catch ex As Exception
            MsgBox("Error during save" & vbCrLf & ex.Message, MsgBoxStyle.Critical, Me.Text & " [BtnSave]")
            svtTrn.Rollback()
            Exit Sub
        End Try
    End Sub

    Private Sub vsfPrintoptions_BeforeEdit(sender As Object, e As RowColEventArgs) Handles vsfDtl.BeforeEdit
        e.Cancel = True
    End Sub

    Private Sub btnEdit_Click(sender As Object, e As EventArgs) Handles btnEdit.Click
        If Trim(cboTrnName.Text) = "" Then Exit Sub
        If intDocID <= 0 Then Exit Sub
        If vsfDtl.Rows.Count <= 1 Then Exit Sub
        Dim objFrm As New FrmUtlPrintOptionname
        With objFrm
            .Text = "Print Option Name for " & cboTrnName.Text
            .txtPrintName.Enabled = False
            .Mode = "M"
            .txtPrintName.Enabled = True
            Call .loadOptionDetails(intDocID, vsfDtl.GetData(vsfDtl.Row, vDetOptionname))
            .ShowDialog()
            Call Btnload_Click(sender, e)
        End With
    End Sub

    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        Dim svtTrn As oledb.oledbTransaction = Nothing
        Try
            If MsgBox("Are you sure you want to delete ?", MsgBoxStyle.YesNo + vbDefaultButton2 + MsgBoxStyle.Critical, "Deletion Confirmation...") = vbNo Then Exit Sub
            strSQL = "Delete from UTLOption where DocID = " & GetVal(vsfDtl.GetData(vsfDtl.Row, vDetDocID), 0)
            strSQL = strSQL & " and OptionName = '" & GetVal(vsfDtl.GetData(vsfDtl.Row, vDetOptionname), "") & "'"

            svtTrn = svtConn.BeginTransaction(IsolationLevel.ReadCommitted)
            If Not clsSVT.execSQLStatement(strSQL, svtTrn) Then

            End If
            svtTrn.Commit()
            vsfDtl.Rows.Remove(vsfDtl.Row)
            MsgBox("Deleted Successfully!", vbInformation, Me.Text)
            Call Btnload_Click(sender, e)
            Exit Sub
        Catch ex As Exception
            MsgBox("Error during Delete" & vbCrLf & ex.Message, MsgBoxStyle.Critical, Me.Text & " [BtnDelete]")
            svtTrn.Rollback()
        End Try
    End Sub

    Private Sub vsfPrintoptions_DoubleClick(sender As Object, e As EventArgs) Handles vsfDtl.DoubleClick
        Call btnEdit_Click(sender, e)
    End Sub

    Private Sub cboTransactionname_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cboTrnName.KeyPress
        If e.KeyChar = "'" Then
            e.Handled = True
        End If
    End Sub

    Private Sub Btnload_Click(sender As Object, e As EventArgs) Handles BtnLoad.Click
        Dim blnFound As Boolean

        If Trim(cboTrnName.Text) = "" Then
            Exit Sub
        End If

        Try
            intDocID = clsSVT.getDataLongValue("select DocID from utltransactions Where DocName = '" & Trim(cboTrnName.Text) & "' ", 0, blnFound)
            If Not blnFound Then
                MsgBox("Selected Document Not Found in Transaction List! " & vbCrLf & "Check Entry...", vbInformation, Me.Text)
                cboTrnName.Text = ""
                cboTrnName.Focus()
                Exit Sub
            End If
            Call GridLoad()
            'Button Enable Disable
            Call btnFalse()
            btnAdd.Visible = True
            If vsfDtl.Rows.Count > 2 Then
                pnlEdit.Visible = True
                btnAlter.Visible = True
                btnClose.Visible = True
            ElseIf vsfDtl.Rows.Count > 1 Then
                pnlEdit.Visible = True
            End If
        Catch ex As Exception
            MsgBox("Error during Load" & vbCrLf & ex.Message, MsgBoxStyle.Critical, Me.Text & " [BtnLoad]")
        End Try
    End Sub


    Private Sub BtnLoad_LostFocus(sender As Object, e As EventArgs) Handles BtnLoad.LostFocus
        With vsfDtl
            If vsfDtl.Rows.Count <= 1 Then
                btnClose.Focus()
            ElseIf btnAlter.Visible Then
                btnAlter.Focus()
            Else
                btnAdd.Focus()
            End If
        End With
    End Sub

    Private Sub btnAlter_LostFocus(sender As Object, e As EventArgs) Handles btnAlter.LostFocus
        btnAdd.Focus()
    End Sub
End Class