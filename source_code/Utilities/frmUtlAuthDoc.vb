Imports C1.Win.C1FlexGrid

Public Class frmUtlAuthDoc
    Dim clsSVT As New svtLibrary
    Dim strDocNo As String
    Dim strDocName As String
    Public intDocID As Int32
    Dim strShForm As String

    Dim strDocHdrTblName As String
    Dim strDocDtlTblName As String
    Dim strDocIDFldName As String
    Dim strDocNoFldName As String
    Dim strDocDateFldName As String
    Dim strAuthQuery As String
    Dim intNoOfAuth As Integer
    Dim intPartyDocID As Int32
    Dim strPartyTblName As String = ""
    Dim strFldPartyID As String = ""
    Dim strFldPartyCode As String = ""
    Dim strFldPartyName As String = ""
    Dim blnManualPartySel As Boolean
    Dim blnSavePartyData As Boolean


    Dim strAuthQryFlds As String = ""
    Dim strAuthQryTbls As String = ""
    Dim strAuthQryCond As String = ""


    Private Sub frmUtlAuthDoc_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyUp
        If e.KeyCode = Keys.F9 And e.Control = True Then
            Call initFormSize(Me, Me.Text, False, 762, 524, 0, 0)
            MsgBox("Restored Form & Grid Layout Successfully!!", MsgBoxStyle.Information, Me.Text)
        End If
        If e.KeyCode = Keys.F8 And e.Control = True Then
            Call SaveActFormSettings(Me, Me.Text)
            Call SaveActFormVsfDet(Me, Me.Text, "Authorise", vsfAuth)
            MsgBox("Saved Form & Grid Layout Successfully!!", MsgBoxStyle.Information, Me.Text)
        End If
    End Sub

    Private Sub frmUtlSearchSettings_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Call initFormSize(Me, Me.Text, False, 762, 524, 0, 0)
        Call initFormVsfDet(Me, Me.Text, vsfAuth, "Authorise")

        Call getUtlTrnData(intDocID)
        Call FillFlds()

        Me.KeyPreview = True
        vsfAuth.Styles.Fixed.Font = New Font("Lucida Sans", vsfAuth.Font.Size, FontStyle.Bold)
        vsfAuth.Styles.Fixed.TextAlign = TextAlignEnum.CenterCenter
    End Sub

    Public Sub FillFlds()
        Dim i As Integer, blnFirstTime As Boolean = False
        Dim rs As oledb.oledbDataReader = Nothing
        Dim strSQL As String = ""
        Dim cs As CellStyle = Nothing
        btnAuth.Visible = False
        strSQL = getQuery()
        rs = clsSVT.getDataReaderFromSQL(strSQL)

        With vsfAuth
            .Cols.Count = 0
            .Rows.Count = 1
            blnFirstTime = True
            If Not rs.HasRows Then
                rs.Close()
                Exit Sub
            End If
            Do While rs.Read
                If blnFirstTime Then
                    For i = 0 To rs.FieldCount - 1
                        .Cols.Count = .Cols.Count + 1
                        .SetData(0, .Cols.Count - 1, rs.GetName(i))
                    Next
                End If
                .Rows.Count = .Rows.Count + 1
                For i = 0 To rs.FieldCount - 1
                    .SetData(.Rows.Count - 1, i, rs.GetValue(i))
                Next
                blnFirstTime = False
            Loop
            rs.Close()

            .Cols(0).DataType = GetType(Boolean)
            cs = .Styles.Add("Boolean")
            cs.DataType = GetType(Boolean)
            cs.ImageAlign = C1.Win.C1FlexGrid.ImageAlignEnum.CenterCenter
            vsfAuth.SetCellStyle(0, 0, cs)
        End With
        Call vsfAuth.AutoSizeCols()
        If vsfAuth.Rows.Count > 1 Then
            btnAuth.Visible = True
        End If
    End Sub

    Private Sub vsfAuth_MouseClick(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles vsfAuth.MouseClick
        Dim ht = vsfAuth.HitTest
        If ht.Row < 0 Then Exit Sub

        If ht.Row = 0 And ht.Column = 0 Then
            If vsfAuth.GetCellCheck(0, 0) = C1.Win.C1FlexGrid.CheckEnum.Checked Then
                For i = 1 To vsfAuth.Rows.Count - 1
                    vsfAuth.SetCellCheck(i, 0, C1.Win.C1FlexGrid.CheckEnum.Checked)
                Next
            Else
                For i = 1 To vsfAuth.Rows.Count - 1
                    vsfAuth.SetCellCheck(i, 0, C1.Win.C1FlexGrid.CheckEnum.Unchecked)
                Next
            End If

        End If

    End Sub

    Private Sub getUtlTrnData(ByVal argDocID As Integer)
        Dim rsDummy As oledb.oledbDataReader
        rsDummy = clsSVT.getDataReaderFromSQL("Select * from UtlTransactions where DocID = " & argDocID)

        With rsDummy
            If .HasRows Then
                .Read()

                strDocName = GetVal(!DocName, "")
                strShForm = GetVal(!DocShForm, "")

                Me.Text = GetVal(!DocName, "") & " - Pending for Authorization"
                strDocNoFldName = strShForm & "No"
                strDocIDFldName = strShForm & "ID"
                strDocHdrTblName = "TrnHdr" & strShForm
                strDocDtlTblName = "TrnDtl" & strShForm
                strDocDateFldName = strShForm & "Date"
                strAuthQuery = GetVal(!NoOfAuthQueryCond, "")
                intNoOfAuth = GetVal(!NoOfAuth, 0)

                blnManualPartySel = IIf(GetVal(!AllowManualPartySel, "N") = "Y", True, False)
                blnSavePartyData = IIf(GetVal(!SavePartyData, "Y") = "Y", True, False)

                intPartyDocID = GetVal(!MstDocIDParty, 0)
                If intPartyDocID > 0 Then
                    Dim strPartyShForm As String
                    strPartyShForm = clsSVT.getDataStringValue("Select DocShForm From UtlTransactions where DocID = " & Val(intPartyDocID), "", False)
                    strPartyTblName = "Mst" & strPartyShForm
                    strFldPartyID = strPartyShForm & "ID"
                    strFldPartyCode = strPartyShForm & "Code"
                    strFldPartyName = strPartyShForm & "Name"
                End If
            End If
            .Close()
        End With
    End Sub

    Private Function getQuery()
        Dim strSQL As String = ""

        strSQL = "Select 0 Sel,"
        strSQL &= strDocNoFldName & ", " & strDocDateFldName & " "
        If intPartyDocID > 0 Then
            strSQL &= ", " & strFldPartyCode & ", " & strFldPartyName
        Else
            If blnManualPartySel AndAlso blnSavePartyData Then
                strSQL &= " ,PartyName"
            End If
        End If
        If strAuthQryFlds <> "" Then strSQL &= ", " & strAuthQryFlds
        strSQL &= " From " & strDocHdrTblName & " h"
        If intPartyDocID > 0 Then
            strSQL &= ", " & strPartyTblName & " p "
        End If
        If strAuthQryTbls <> "" Then strSQL &= ", " & strAuthQryTbls
        strSQL &= " Where 1=1 "
        If intPartyDocID > 0 Then
            strSQL &= "And h." & strFldPartyID & " = p." & strFldPartyID
        End If
        If strAuthQryCond <> "" Then strSQL &= "And " & strAuthQryCond
        strSQL &= " And h.Authorised = 'N'"
        strSQL &= " And h.Cancelled = 'N'"
        strSQL &= " Order by h." & strDocIDFldName

        getQuery = strSQL
    End Function

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Call frmMain.CloseSelectedTabPage(Me)
    End Sub

    Private Sub btnAuth_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAuth.Click
        Dim strSQL As String, iRow As Integer, blnAuth As Boolean
        If vsfAuth.Rows.Count = 1 Then
            MsgBox("Nothing to Authorise!", MsgBoxStyle.Critical, Me.Text)
            Exit Sub
        End If

        blnAuth = False
        With vsfAuth
            If MsgBox(" Are you sure you want authorise selected documents?", vbYesNo + vbDefaultButton2 + vbCritical, Me.Text) = vbNo Then Exit Sub

            For iRow = 1 To vsfAuth.Rows.Count - 1
                If .GetData(iRow, 0) = True Then
                    strSQL = UpdateAuthQuery(strDocName, vsfAuth.GetData(iRow, 1))
                    clsSVT.execSQLStatement(strSQL)
                    blnAuth = True
                End If
            Next
            If blnAuth Then
                MsgBox("Selected Document Authorised Successfully!", MsgBoxStyle.Information, Me.Text)
                Call FillFlds()
            Else
                MsgBox("No records selected to Authorise!", MsgBoxStyle.Information, Me.Text)
            End If
        End With
        strDocNo = ""
    End Sub

    Private Sub getAuthDoc()

        Dim frmOpen As New frmTrnPartyDoc
        With frmOpen
            .initForm(intDocID)
            If strDocNo <> "" Then .FillDocDetails(clsSVT.getDataLongValue("Select " & strDocIDFldName & " from " & strDocHdrTblName & " Where " & strDocNoFldName & " = '" & strDocNo & "'", 0, False))
            frmMain.OpenForms(frmOpen, strDocName, True)
        End With

        'frmOpen.Dispose()
        '        With frmOpen
        '            .initForm(clsSVT.getDataLongValue("Select DocID from mstMenu where MenuItemCaption = 'Purchase Order'", 0, False))
        '            .Mode = "V"
        '            .svtBtnCtrl.Enabled = False
        '            If vsfAuth.Rows.Count > 1 Then
        '                .lngViewDocID = clsSVT.getDataLongValue("Select poid from TrnHdrPO Where PONO ='" & vsfAuth.GetData(vsfAuth.Row, 1) & "'", 0, True)
        '                Show()
        '            End If
        '            Exit Sub

        '        End With
    End Sub

    Private Sub vsfAuth_BeforeEdit(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfAuth.BeforeEdit
        If e.Col = 0 Then Exit Sub
        e.Cancel = True
    End Sub

    Private Sub vsfAuth_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles vsfAuth.Click
        If vsfAuth.Rows.Count <= 0 Then
            strDocNo = vsfAuth.GetData(vsfAuth.Row, 1)
        End If
    End Sub

    Private Sub vsfAuth_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles vsfAuth.DoubleClick
        If vsfAuth.Rows.Count > 1 Then
            strDocNo = vsfAuth.GetData(vsfAuth.Row, 1)
        End If
        Call getAuthDoc()
    End Sub

    Private Function UpdateAuthQuery(ByVal argDocName As String, ByVal argDocNo As String)
        Dim strSQL As String = ""

        strSQL = "Update " & strDocHdrTblName & " set Authorised = 'Y' , AuthBy = '" & gUserCode & "' ,"
        strSQL = strSQL & " AuthOn = '" & getDateFormatInInsert(getSystemDate) & "' where " & strDocNoFldName & " = '" & argDocNo & "'"

        UpdateAuthQuery = strSQL
    End Function

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click

    End Sub
End Class