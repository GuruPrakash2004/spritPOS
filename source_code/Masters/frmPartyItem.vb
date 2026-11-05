Imports System.ComponentModel
Imports C1.Win.C1FlexGrid

Public Class frmPartyItem
    Dim clsSVT As New svtLibrary
    Dim Mode As String
    Dim dtSearch As New DataTable
    Dim dvSearch As New DataView
    Public intDocID As Integer
    Dim intRateDecimals As Integer
    Dim strRights As String
    Public strfrmCaption As String

    Const vMemNo = 0
    Const vMemDate = 1
    Const vMemUser = 2
    Const vMemEffDtFrom = 3
    Const vMemEffDtTo = 4
    Const vMemRate = 5
    Const vMemTotal = 6

    Const vDetSlNo = 0
    Const vDetItemID = 1
    Const vDetItemCode = 2
    Const vDetItemname = 3
    Const vDetUOM = 4
    Const vDetMemno = 5
    Const vDetRate = 6
    Const intDetCols = 7
    Dim intLastCol As Integer

    Dim blnMemoradum As Boolean

    Dim strMstTableName As String
    Dim strMstShForm As String
    Dim strMstFldID As String

    'Item Related Information
    Dim intItemDocID As Int32 = 0
    Dim strItemTblName As String
    Dim strFldItemID As String
    Dim strFldItemCode As String
    Dim strFldItemName As String
    Dim strItemShForm As String

    'Party Related Information
    Dim intPartyDocID As Int32 = 0
    Dim strPartyCaption As String
    Dim strPartyTblName As String
    Dim strFldPartyID As String
    Dim strFldPartyCode As String
    Dim strFldPartyName As String
    Dim strPartyShForm As String

    Dim blnPartyValidate As Boolean, blnItemValidate As Boolean
    Private e As EventArgs

    Private Sub frmTrnPartyDoc_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        'e.Cancel = False
        'If Mode = "A" Or Mode = "M" Then
        '    If MsgBox("Are you sure you want to exit the form without saving?!", MsgBoxStyle.Question _
        '              + MsgBoxStyle.YesNo + vbDefaultButton2, "Confirmation...") = MsgBoxResult.No Then
        '        e.Cancel = True
        '        Exit Sub
        '    End If
        'End If
    End Sub

    Private Sub frmSuppVsItem_KeyUp(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyUp
        If e.KeyCode = Keys.F9 And e.Control Then
            Call initFormSize(Me, Me.Text, True, 662, 442, 0, 0)
            Call initFormVsfDet(Me, Me.Text, vsfDet, "DET")
            MsgBox("Restored Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
        If e.KeyCode = Keys.F8 And e.Control = True Then
            Call SaveActFormSettings(Me, Me.Text)
            Call SaveActFormVsfDet(Me, Me.Text, "DET", vsfDet)
            MsgBox("Saved Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If

        If e.Alt = True And e.KeyCode = Keys.E Then
            Call ExportToXL(vsfDet)
        End If

        If e.Alt = True And e.KeyCode = Keys.R Then
            If GetVal(vsfDet.GetData(vsfDet.Row, vDetItemname), "") = "" Then
                MsgBox("" & strFldItemCode & " Empty....", MsgBoxStyle.Information, Me.Text)
                Exit Sub
            End If

            If MsgBox("Do you want to delete the " & strFldItemCode & " ...?", MsgBoxStyle.YesNo, Me.Text) = MsgBoxResult.Yes Then
                vsfDet.Rows.Remove(vsfDet.Row)
                MsgBox("Click on save to update new item list", MsgBoxStyle.Information, Me.Text)
            End If
        End If

    End Sub

    Private Sub frmPartyItem_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        'modified by Pavithra
        btnSave.Visible = False
        btnCancel.Visible = False

        strRights = getUserRights(GetMenuId(Me.Text))
        intDocID = GetFrmId(Me.Text)
        lblTitle.Text = Me.Text
        Call getUtlTrnData(intDocID)
        Call initScreen()
        Call fillFormProperties(Me)
        Call initGridStyles(Me)
        KeyPreview = True
        Mode = "A"
        Call initFormSize(Me, Me.Text, False, 662, 442, 0, 0)
        Call initFormVsfDet(Me, Me.Text, vsfDet, "DET")
        lblTitle.ForeColor = Color.White
        txtPartyCode.Focus()
        Call ChangeFontSize(Me)
        lblTitle.Font = New Font("Microsoft Sans Serif", 14, FontStyle.Bold)
        vsfDet.Size = New Point(956, 587)
        vsfDet.Enabled = False
    End Sub

    Private Sub getUtlTrnData(ByVal argDocID As Integer)
        Dim rsDummy As oledb.oledbDataReader

        rsDummy = clsSVT.getDataReaderFromSQL("Select * from UtlTransactions where DocID = " & argDocID)

        With rsDummy
            If .HasRows Then
                .Read()
                Me.Tag = Me.Text

                intPartyDocID = GetVal(!MstDocIDParty, 0)

                strMstTableName = GetVal(!HdrTblName, "")
                strMstShForm = GetVal(!DocShForm, "")
                strMstFldID = strMstShForm & "ID"

                If intPartyDocID <= 0 Then
                    MsgBox("No Party Table Present.. Application Closed...", MsgBoxStyle.Information, Me.Text)
                    Me.Close()
                Else
                    strPartyShForm = clsSVT.getDataStringValue("Select DocShform From UtlTransactions where DocID = " & Val(intPartyDocID), "", False)
                    'strPartyCaption = clsSVT.getDataStringValue("Select DocName From UtlTransactions where DocID = " & Val(intPartyDocID), "", False).Replace("Master", "")
                    strPartyTblName = "Mst" & strPartyShForm
                    strFldPartyID = strPartyShForm & "ID"
                    strFldPartyCode = strPartyShForm & "Code"
                    strFldPartyName = strPartyShForm & "Name"
                    'strFldPartyCode = strfrmCaption & "Code"
                    'strFldPartyName = strfrmCaption & "Name"
                End If

                Dim strMemorandum As String = clsSVT.getDataStringValue("Select FldBlnFromMemorandum from UtlTransactions where DocID=" & intPartyDocID, "", False)
                If strMemorandum = "Y" Then
                    blnMemoradum = True
                    btnTransactions.Visible = True
                Else
                    btnTransactions.Visible = False
                End If


                intItemDocID = GetVal(!MstDocIDItem, 0)

                If intItemDocID = 0 Then
                    MsgBox("No " & gItemCode & " Table Present.. Application Closed...", MsgBoxStyle.Information, Me.Text)
                    Me.Close()
                Else
                    strItemShForm = clsSVT.getDataStringValue("Select DocShform From UtlTransactions where DocID = " & Val(intItemDocID), "", False)
                    strItemTblName = "Mst" & strItemShForm
                    strFldItemID = strItemShForm & "ID"
                    strFldItemCode = strItemShForm & "Code"
                    strFldItemName = strItemShForm & "Name"
                End If
            End If
            .Close()

            lblPartyCode.Text = strFldPartyCode    'strPartyCaption & " Code "
            lblPartyName.Text = strFldPartyName   'strPartyCaption & " Name "
        End With
    End Sub

    Public Sub initScreen()

        If blnMemoradum Then
            gbMemorandum.Visible = True
            btnSave.Text = "&Update"
            vsfDet.Size = New Point(vsfDet.Size.Width, 228)

        Else
            btnSave.Text = "&Save"
            vsfDet.Size = New Point(vsfDet.Size.Width, 290)
            gbMemorandum.Visible = False
        End If

        With vsfMem
            .Cols.Count = vMemTotal
            .Rows.Count = 1
            .SetData(0, vMemNo, "Memorandum No")
            .SetData(0, vMemDate, "Entry Date")
            .SetData(0, vMemUser, "Entry By")
            .SetData(0, vMemEffDtFrom, "Effictive From Date")
            .SetData(0, vMemEffDtTo, "Effictive To Date")
            .SetData(0, vMemRate, "Rate")

            Call SetColAlign(vsfMem, svtAlign.Left, vMemNo, vMemDate, vMemEffDtFrom, vMemEffDtTo)
            Call SetColAlign(vsfMem, svtAlign.Right, vMemRate)
            Call SetHideCol(vsfMem, True, vMemDate, vMemUser)
            Call SetFlexColor(vsfMem, "", vMemNo, vMemEffDtFrom, vMemEffDtTo, vMemRate)

        End With


        With vsfDet
            .Cols.Count = intDetCols
            .Rows.Count = 2
            .SetData(0, vDetSlNo, "Sl No")
            .SetData(0, vDetItemID, "Item ID")
            .SetData(0, vDetItemCode, gItemCode)
            .SetData(0, vDetItemname, gItemName)
            .SetData(0, vDetUOM, "UOM")
            .SetData(0, vDetRate, "Rate")
            .SetData(0, vDetMemno, "Last Mem No.")

            .Cols(vDetSlNo).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.LeftCenter
            Call SetColAlign(vsfDet, svtAlign.Left, vDetItemCode, vDetItemname, vDetUOM)
            Call SetColAlign(vsfDet, svtAlign.Right, vDetRate)

            'Call initItemVsfCombo()
            Call InitDetAddnlGrid(Me, vsfDet, vsfOptions, intDetCols, "", intDocID, True)


            If blnMemoradum Then
                Call SetHideCol(vsfDet, True, vDetItemID)
                Call SetFlexColor(vsfDet, "", vDetUOM, vDetSlNo, vDetItemCode, vDetRate, vDetItemname, vDetMemno, vDetUOM)
            Else
                Call SetHideCol(vsfDet, True, vDetItemID, vDetMemno)
                Call SetFlexColor(vsfDet, "", vDetSlNo, vDetItemname, vDetMemno, vDetUOM)
                Call SetFlexColor(vsfDet, "M", vDetItemCode, vDetRate)
            End If

            intLastCol = 1
            For j = vsfDet.Cols.Count - 1 To 0 Step -1
                If vsfDet.Cols(j).Visible = True Then
                    intLastCol = j
                    Exit For
                End If
            Next

            With vsfOptions
                .SetData(vDetSlNo, vOthFldDataType, "Numeric")
                .SetData(vDetItemID, vOthFldDataType, "Numeric")
                .SetData(vDetItemCode, vOthFldDataType, "Text")
                .SetData(vDetItemname, vOthFldDataType, "Text")
                .SetData(vDetItemCode, vOthFldCode, "SVTITEMCODE")
                .SetData(vDetItemID, vOthFldCode, "SVTITEM")
                .SetData(vDetItemID, vOthFldBlnChkDuplicate, "Y")
                .SetData(vDetUOM, vOthFldBlnChkDuplicate, "Y")
                .SetData(vDetItemID, vOthFldBlnChkDuplicate, clsSVT.getDataStringValue("Select case isnull(BlnAllowDuplicateItems,'N') when 'N' then 'Y' else 'N' end from UtlTransactions where DocID=" & intDocID, "N", False))
                .SetData(vDetUOM, vOthFldBlnChkDuplicate, .GetData(vDetItemID, vOthFldBlnChkDuplicate))
            End With
            Call ReArrangeSlNo(vsfDet, vDetSlNo)
        End With
    End Sub


    Private Sub btLoad_Click(sender As System.Object, e As System.EventArgs) Handles btLoad.Click
        btnSave.Visible = True
        btnCancel.Visible = True
        If Trim(txtPartyCode.Text) = "" Then
            txtPartyCode.Focus()
            btnSave.Visible = False '''Changed
            btnCancel.Visible = False '''Changed
            MsgBox("Please Select " & lblPartyCode.Text, vbInformation, Me.Text) '''Changed
            Exit Sub
        End If

        With vsfDet
            .FinishEditing()
            .EndUpdate()
            .Enabled = False
            .Col = vDetItemname
            .Row = 1
            .Focus()
        End With

        Dim strSQL As String
        Dim rs As oledb.oledbDataReader
        strSQL = "Select i." & strFldItemID & ", i." & strFldItemCode & ", i." & strFldItemName & ", u.UOMCode, mpi.* "
        strSQL &= " from " & strMstTableName & " mpi, " & strItemTblName & " i, " & strPartyTblName & " p, MstUOM u"
        strSQL &= " Where mpi." & strFldPartyID & "=p." & strFldPartyID
        strSQL &= " and mpi." & strFldItemID & " = i." & strFldItemID
        strSQL &= " and mpi.UOMID=u.UOMID"
        strSQL &= " and mpi.InActive='N' "
        strSQL &= " and p." & strFldPartyCode & "='" & txtPartyCode.Text & "'"
        strSQL &= " Order by mpi." & strMstFldID

        rs = clsSVT.getDataReaderFromSQL(strSQL)

        With vsfDet
            .Rows.Count = 1
            .Rows.Count = 2
        End With
        If Not rs.HasRows Then
            vsfDet.Enabled = True
            Call ReArrangeSlNo(vsfDet, vDetSlNo)
            rs.Close()
            'vsfDet.Rows.Count=2'''Changed
            Exit Sub
        End If

        Do While rs.Read()
            With vsfDet
                .SetData(.Rows.Count - 1, vDetItemID, rs.Item(strFldItemID))
                .SetData(.Rows.Count - 1, vDetItemCode, rs.Item(strFldItemCode))
                .SetData(.Rows.Count - 1, vDetItemname, rs.Item(strFldItemName))
                .SetData(.Rows.Count - 1, vDetUOM, rs.Item("UOMCode"))
                .SetData(.Rows.Count - 1, vDetRate, rs.Item("Rate"))
                .SetData(.Rows.Count - 1, vDetMemno, "")

                Call FillvsfOptDtlFlds(Me, vsfDet, vsfOptions, rs, .Rows.Count - 1, intDetCols)
                .Rows.Count = .Rows.Count + 1
            End With
        Loop
        rs.Close()

        With vsfDet
            Call RemoveEmptVSFRows(vsfDet, 1, vDetItemCode)
            Call ReArrangeSlNo(vsfDet, vDetSlNo)
            vsfDet.Enabled = True
            .Row = .Rows.Count - 1
            If blnMemoradum Then
                .Col = intDetCols
            Else
                .Col = vDetItemCode
            End If
            .Focus()
        End With
    End Sub

    Private Function CalTotal(ByVal rate As Double, ByVal disc As Double) As Boolean
        CalTotal = False
        'Try
        '    With vsfDet
        '        If disc = 0 Then
        '            .SetData(.Row, vDetRate, Format(rate, "##,###.00"))
        '            .SetData(.Row, vDetDisc, Format(disc, "##,###.00"))
        '            .SetData(.Row, vDetNetPrice, Format(rate, "##,###.00"))
        '        Else
        '            .SetData(.Row, vDetRate, Format(rate, "##,###.00"))
        '            .SetData(.Row, vDetDisc, Format(disc, "##,###.00"))
        '            .SetData(.Row, vDetNetPrice, Format(rate * (1 - (disc / 100)), "##,###.00"))
        '        End If
        '    End With
        '    Return True
        'Catch ex As Exception
        '    MsgBox("Error On Calculation" & vbCrLf & ex.Message, MsgBoxStyle.Critical, Me.Text)
        '    Exit Function
        'End Try
    End Function

    Private Function applyFilterFromGrid() As Boolean
        Dim strSQL As String
        applyFilterFromGrid = False
        With vsfDet
            If .Editor.Text = "" Then
                vsfView.Visible = False
                Exit Function
            End If

            strSQL = "Select Top " & gIntTopNrows & " " & strFldItemCode & ", u.UOMCode, " & strFldItemName
            strSQL &= " From " & strItemTblName & " i, MstUOM u, MstItemUOM iu"
            strSQL &= " where i.InActive = 'N'  "
            strSQL &= " and i.Authorised = 'Y'  "
            strSQL &= " and i.ItemID = iu.ItemID  "
            strSQL &= " and iu.UOMID = u.UOMID  "
            If Not .Editor.Text = " " Then
                strSQL &= " And (" & strFldItemCode & " like '%" & .Editor.Text & "%'"
                strSQL &= " or " & strFldItemName & " like '%" & .Editor.Text & "%')"
            End If

            strSQL &= " order by " & strFldItemCode
        End With

        Call showWaitCursor()
        dtSearch = clsSVT.getDATATable(strSQL)
        dvSearch = New DataView(dtSearch)
        vsfView.DataSource = dvSearch
        Call showNormalCursor()

        applyFilterFromGrid = True
    End Function

    Private Sub loadItemToDtl()
        If Not FillItemDet(vsfView.GetData(vsfView.Row, 0), vsfDet.Row, argUOM:=vsfView.GetData(vsfView.Row, 1)) Then
            MsgBox("Invalid " & gItemCode & "!!", vbInformation, Me.Text)
            vsfView.Visible = True
            Exit Sub
        Else
            vsfView.Visible = False
            vsfDet.Focus()
            If blnMemoradum Then
                vsfDet.Col = intDetCols
            Else
                vsfDet.Col = vDetRate
            End If
            vsfDet.StartEditing()
        End If
    End Sub

    Public Function FillItemDet(ByVal argItem As String, ByVal argRow As Long, Optional ByVal rowIndex As Integer = -1, Optional ByVal argUOM As String = "") As Boolean
        Dim rsDummy As oledb.oledbDataReader
        Dim strSQL As String
        Dim blnInActive As Boolean
        Dim intItemID As Int32 = 0
        FillItemDet = False

        If rowIndex = -1 Then
            rowIndex = vsfDet.Row
        End If

        With vsfDet
            If argItem = "" Then
                Exit Function
            End If

            intItemID = clsSVT.getDataLongValue("Select " & strFldItemID & " from " & strItemTblName & " where " & strFldItemCode & " = '" & argItem & "'", 0, blnInActive)
            If intItemID = 0 Then
                MsgBox(strItemShForm & " code '" & argItem & "' does not exist!", vbInformation, Me.Text)
                Exit Function
            End If

            Call clsSVT.getDataLongValue("Select " & strFldItemID & " from " & strItemTblName & " where " & strFldItemCode & " = '" & argItem & "' and InActive='Y'", 0, blnInActive)

            If blnInActive Then
                MsgBox(strItemShForm & " code '" & argItem & "' is inactive! Please select an active " & strItemShForm & " from the list.", vbInformation, Me.Text)
                Exit Function
            End If

            strSQL = "Select i.*, UOMCode from "
            strSQL = strSQL & strItemTblName & " i, MstUOM u"
            strSQL = strSQL & "  where i.UomID = u.UomID  "
            strSQL = strSQL & " and  " & strFldItemCode & " = '" & argItem & "'"

            rsDummy = clsSVT.getDataReaderFromSQL(strSQL)

            With rsDummy
                If Not .HasRows Then
                    MsgBox(strItemShForm & " does not exist!!", vbInformation, Me.Text)
                    Exit Function
                End If
                .Read()

                vsfDet.SetData(rowIndex, vDetSlNo, rowIndex)
                vsfDet.SetData(rowIndex, vDetItemID, rsDummy(strFldItemID))
                vsfDet.SetData(rowIndex, vDetItemname, rsDummy(strFldItemName))
                vsfDet.SetData(rowIndex, vDetItemCode, rsDummy(strFldItemCode))
                If argUOM <> "" Then
                    vsfDet.SetData(rowIndex, vDetUOM, argUOM)
                Else
                    vsfDet.SetData(rowIndex, vDetUOM, !UOMCode)
                End If

                .Close()
            End With
        End With
        Call fillItemRelatedDataFromDocDsgnr(intDocID, Val(vsfDet.GetData(rowIndex, vDetItemID)), Me)
        Call vsfOptValidateEdit(Me, vsfDet, vsfOptions, vsfDet.Row, vDetItemCode, "M", "", "", "")
        FillItemDet = True
    End Function

    Private Function getmaxNo() As Integer
        Dim i As Integer, intNo As Integer
        With vsfDet
            intNo = 0
            For i = 1 To .Rows.Count - 1
                If Val(.GetData(i, vDetSlNo)) > intNo Then
                    intNo = Val(.GetData(i, vDetSlNo))
                End If
            Next i
        End With
        intNo = intNo + 1
        getmaxNo = intNo
    End Function

    'Private Sub btTranspose_Click(sender As System.Object, e As System.EventArgs) Handles btTranspose.Click
    '    Dim frm As New frmItemVsSupp
    '    'frm.MdiParent = frmMain
    '    frm.ShowDialog()
    'End Sub

    Private Sub vsfDet_GotFocus(sender As Object, e As System.EventArgs) Handles vsfDet.GotFocus
        If vsfDet.Rows.Count = 1 And Trim(GetVal(vsfDet.GetData(0, vDetItemCode), "")) = "" Then
            vsfDet.Col = vDetItemCode
        End If
    End Sub

    Private Function validFlds() As Boolean
        validFlds = False

        If Trim(txtPartyCode.Text) = "" Then
            MsgBox("Party Name should not be blank!!", MsgBoxStyle.Exclamation, Me.Text)
            txtPartyCode.Focus()
            Exit Function
        End If

        txtPartyID.Text = clsSVT.getDataLongValue("Select " & strFldPartyID & " from " & strPartyTblName & " where " & strFldPartyCode & " = '" & txtPartyCode.Text & "'", 0, False)
        If Val(txtPartyID.Text) = 0 Then
            MsgBox("Invalid Party(" & Trim(txtPartyCode.Text) & ")....", MsgBoxStyle.Information, Me.Text)
            txtPartyCode.Focus()
            Exit Function
        End If

        If Not blnMemoradum AndAlso vsfDet.Rows.Count = 2 AndAlso GetVal(vsfDet.GetData(1, vDetItemCode), "") = "" Then
            MsgBox("Details section should not be Empty....", MsgBoxStyle.Exclamation, Me.Text)
            vsfDet.Col = vDetItemCode
            vsfDet.Row = 1
            vsfDet.Focus()
            ''''''Changed
            If vsfDet.Enabled = False Then
                btLoad.Focus()
            End If
            '''Finished on 6Feb19
            Exit Function
        End If

        Dim intDupRow As Integer

        With vsfDet
            If ChkDuplicateData(vsfOptions, vsfDet, intDupRow, "", "") Then
                MsgBox("Duplicate Data present (in row " & intDupRow & ") " & vbCrLf & "for combination of columns " & "" & vbCrLf & "for the values " & "", vbInformation, Me.Text)
                .Row = intDupRow
                .TopRow = intDupRow
                .Col = vDetItemCode
                .Focus()
                Exit Function
            End If

            For i As Int32 = 1 To .Rows.Count - 1
                If GetVal(.GetData(i, vDetItemID), 0) <= 0 Then
                    MsgBox("" & strFldItemCode & " Empty....", MsgBoxStyle.Exclamation, Me.Text)
                    vsfDet.Col = vDetItemCode
                    vsfDet.Row = i
                    vsfDet.Focus()
                    Exit Function
                End If

                If GetVal(.GetData(i, vDetRate), 0) <= 0 Then
                    MsgBox("Rate Empty....", MsgBoxStyle.Exclamation, Me.Text)
                    vsfDet.Col = vDetRate
                    vsfDet.Row = i
                    vsfDet.Focus()
                    Exit Function
                End If
            Next
        End With

        validFlds = True
    End Function

    Private Sub btnSave_Click(sender As System.Object, e As System.EventArgs) Handles btnSave.Click
        Dim strFlds As String = ""
        Dim strFlds1 As String = ""
        Dim strValues As String = ""
        Dim strColFlds As String = ""
        Dim strColValues As String = ""
        Dim strSQL As String = ""

        stringReplace(Me)
        Call RemoveEmptVSFRows(vsfDet, 1, vDetItemCode)
        Call ReArrangeSlNo(vsfDet, vDetSlNo)
        If validFlds() = False Then Exit Sub

        Dim svtTrn As oledb.oledbTransaction
        svtTrn = svtConn.BeginTransaction(IsolationLevel.ReadCommitted)

        If Not blnMemoradum Then
            If Not clsSVT.execSQLStatement("Delete from " & strMstTableName & " Where " & strFldPartyID & " = " & Val(txtPartyID.Text), svtTrn) Then
                svtTrn.Rollback()
                Exit Sub
            End If
        End If

        Call GetvsfOptHdrDetInsertString(Me, vsfDet, vsfOptions, intDetCols, 1, strColFlds, strColValues, svtTrn)

        With vsfDet
            If Not blnMemoradum Then
                strFlds = "Insert Into " & strMstTableName & " (" & strMstShForm & "Code," & strMstShForm & "Name, UOMID,"
                strFlds &= "InActiveRmks, InActive, AddedBy, AddedOn, " & strFldItemID & ", " & strFldPartyID & ", Rate, EffDateFrom, EffDateTo,"
                For i As Int32 = 1 To .Rows.Count - 1
                    strValues &= "('" & Date.Now.ToString("yyyyMMddHHmmssfff") & "','" & Date.Now.ToString("yyyyMMddHHmmssfff") & "',"
                    strValues &= clsSVT.getDataLongValue("Select UOMID from MstUOM where UOMCode = '" & .GetData(i, vDetUOM) & "'", 0, False, svtTrn) & ","
                    strValues &= "'','N','" & gUserCode & "'," & getDateFormatInCond(getSystemDate(svtTrn)) & ","
                    strValues &= Val(.GetData(i, vDetItemID)) & ","
                    strValues &= Val(txtPartyID.Text) & ","
                    strValues &= Val(.GetData(i, vDetRate)) & ","
                    strValues &= getDateFormatInCond(getSystemDate(svtTrn)) & ",'9999-12-31',"
                    Call GetvsfOptHdrDetInsertString(Me, vsfDet, vsfOptions, intDetCols, i, strFlds1, strValues, svtTrn)
                    strValues = strValues & ","
                Next

                strValues = Mid(strValues, 1, Len(strValues) - 1)
                strSQL = strFlds & strColFlds & " values " & strValues
            Else
                strSQL = ""
                For i As Int32 = 1 To vsfDet.Rows.Count - 1
                    strSQL &= " Update " & strMstTableName & " Set "
                    With vsfOptions
                        For k As Int32 = intDetCols To vsfOptions.Rows.Count - 1
                            strSQL &= .GetData(k, vOthFldCode) & " = "
                            Select Case UCase(.GetData(k, vOthFldDataType))
                                Case "TEXT", "LIST STRING"
                                    strSQL &= "'" & GetVal(vsfDet.GetData(i, k), "") & "',"
                                Case "DATE"
                                    strSQL &= "'" & getDateFormatInInsert(vsfDet.GetData(i, k)) & "',"
                                Case Else
                                    strSQL &= Val(vsfDet.GetData(i, k)) & ","
                            End Select
                        Next
                    End With
                    strSQL = Mid(strSQL, 1, Len(strSQL) - 1)
                    strSQL &= " Where " & strFldItemID & " = " & Val(.GetData(i, vDetItemID))
                    strSQL &= " and " & strFldPartyID & " = " & Val(txtPartyID.Text) & ";"
                Next
            End If
        End With
        Try
            If clsSVT.execSQLStatement(strSQL, svtTrn) Then
                svtTrn.Commit()
                If blnMemoradum Then
                    MsgBox("Updated successfully.", MsgBoxStyle.Information, Me.Text)
                Else
                    MsgBox("Saved successfully.", MsgBoxStyle.Information, Me.Text)
                End If
            Else
                svtTrn.Rollback()
            End If
        Catch ex As Exception
            MsgBox("Error during save" & vbCrLf & ex.Message, MsgBoxStyle.Critical, Me.Text)
            svtTrn.Rollback()
            Exit Sub
        End Try
    End Sub

    Private Sub btnClose_Click(sender As System.Object, e As System.EventArgs) Handles btnClose.Click
        Call frmMain.CloseSelectedTabPage(Me)
    End Sub

    Private Sub btnImpXL_Click(sender As System.Object, e As System.EventArgs) Handles btnImpXL.Click
        If Trim(txtPartyCode.Text) = "" Then Exit Sub
        Dim obj As New frmUtlImportItemFmExcel
        With obj
            .getUtlTrnData(intDocID)
            .objFrm = Me
            .argVSF = vsfDet
            .ShowDialog()
        End With
    End Sub

    Private Sub vsfView_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles vsfView.KeyUp
        If e.KeyCode = Keys.Escape Then
            vsfView.Visible = False
        End If
        If vsfView.Row > 0 Then
            If e.KeyCode = Keys.Return Or e.KeyCode = Keys.Tab Then
                Call loadItemToDtl()
            End If
        End If
    End Sub

    Private Sub vsfView_BeforeEdit(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfView.BeforeEdit
        e.Cancel = True
    End Sub

    Private Sub vsfView_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles vsfView.DoubleClick
        If vsfView.Row > 0 Then
            Call loadItemToDtl()
        End If
    End Sub

    Private Sub vsfDet_BeforeEdit(sender As Object, e As RowColEventArgs) Handles vsfDet.BeforeEdit
        If (Not (Mode = "A" Or Mode = "M")) Then
            e.Cancel = True
            Exit Sub
        End If

        vsfDet.EditMask = ""
        vsfDet.ComboList = ""

        Select Case e.Col
            Case vDetSlNo, vDetItemID, vDetItemname, vDetMemno, vDetUOM
                e.Cancel = True
            Case vDetItemCode, vDetRate
                If blnMemoradum Then
                    e.Cancel = True
                End If
                'Case vDetUOM
                '    If Val(vsfDet.GetData(vsfDet.Row, vDetItemID)) <= 0 Then
                '        e.Cancel = True
                '        Exit Sub
                '    End If
                '    vsfDet.ComboList = GenVSfComboStringCond("UOMCode", "vwItemWiseUOM", "", " Where ItemID=" & Val(vsfDet.GetData(vsfDet.Row, vDetItemID)))
                '    Exit Sub
        End Select

        If e.Col < intDetCols Then Exit Sub

        If vsfOptBeforeEditCancel(Me, vsfDet, vsfOptions, "", Mode, vsfDet.Row, vsfDet.Col, intDetCols) Then
            e.Cancel = True
            Exit Sub
        End If

    End Sub

    Private Sub vsfDet_AfterEdit(sender As Object, e As RowColEventArgs) Handles vsfDet.AfterEdit
        If Mode = "V" Or Mode = "" Then Exit Sub
        If e.Col <> vDetSlNo Then Call vsfOptAfterEditBfrCalTot(Me, vsfDet, vsfOptions, vsfDet.Row, vsfDet.Col)
    End Sub

    Private Sub vsfDet_ChangeEdit(sender As Object, e As EventArgs) Handles vsfDet.ChangeEdit
        If vsfDet.Col = vDetItemCode Then
            Select Case vsfDet.Editor.Text
                Case ""
                    vsfView.Visible = False
                Case Else
                    If vsfDet.ScrollBarsVisible = ScrollBars.Both Or vsfDet.ScrollBarsVisible = ScrollBars.Vertical Then
                        vsfView.Top = vsfDet.Bottom + 5
                    Else
                        vsfView.Top = vsfDet.Top + vsfDet.Rows(vsfDet.Row).Top + (vsfDet.Rows(vsfDet.Row).Height) + 20
                    End If
                    vsfView.Left = vsfDet.Left + vsfDet.Cols(vsfDet.Col).Left
                    vsfView.Size = New Point(vsfView.Width, btnExport.Bottom - vsfView.Top - 10)
                    vsfView.Visible = True
                    vsfView.BringToFront()
                    Call applyFilterFromGrid()
            End Select
        End If
    End Sub

    Private Sub vsfDet_EnterCell(sender As Object, e As EventArgs) Handles vsfDet.EnterCell
        If (Not (Mode = "A" Or Mode = "M")) Then Exit Sub

        With vsfDet
            Select Case .Col
                Case Is >= intDetCols
                    If vsfOptions.GetData(.Col, vOthFldBlnComboData) = "Y" Then Exit Sub
                    If vsfOptions.GetData(.Col, vOthFldDataType) = "List String" Then Exit Sub
                    If vsfOptions.GetData(.Col, vOthFldBlnReadOnly) = "Y" Then Exit Sub
                    If vsfOptions.GetData(.Col, vOthFldBlnRefill) = "Y" Then Exit Sub
                    Try
                        ' .StartEditing()
                    Catch ex As Exception

                    End Try
                Case vDetRate, vDetItemCode
                    If Not blnMemoradum Then
                        Try
                            .StartEditing()
                        Catch ex As Exception

                        End Try
                    End If
            End Select
        End With
    End Sub

    Private Sub vsfDet_ValidateEdit(sender As Object, e As ValidateEditEventArgs) Handles vsfDet.ValidateEdit
        If (Not (Mode = "A" Or Mode = "M")) Then
            e.Cancel = True
            Exit Sub
        End If

        If vsfDet.Col < intDetCols Then Exit Sub

        If vsfOptValidateEdit(Me, vsfDet, vsfOptions, vsfDet.Row, vsfDet.Col, "M", "", "", "") = False Then
            e.Cancel = True
        End If
    End Sub

    Private Sub vsfDet_KeyPressEdit(sender As Object, e As KeyPressEditEventArgs) Handles vsfDet.KeyPressEdit

        If e.KeyChar = "'" Then
            e.Handled = True
        End If

        '''Changed
        If vsfDet.Col = vDetRate Then
            If KeyValidate("Integer", e.KeyChar) Then
                e.Handled = False
            Else
                e.Handled = True
            End If
        End If
        '''Finished on 6Feb19

        If e.Col < intDetCols Then Exit Sub

        If vsfOptKeyPressEdit(Me, vsfDet, vsfOptions, e.Col, e.Row, e.KeyChar) Then
            e.Handled = False
        Else
            e.Handled = True
        End If


    End Sub

    Private Sub vsfDet_KeyUp(sender As Object, e As KeyEventArgs) Handles vsfDet.KeyUp
        If e.KeyCode = Keys.F7 Then
            vsfDet.Cols.Frozen = vsfDet.Col
            MsgBox("Column Freezing Activated up to " & vsfDet.GetData(0, vsfDet.Col), vbInformation, Me.Text)
        End If

        If e.Control = True And e.KeyCode = Keys.C Then
            Call vsfCopyItemCode(vsfDet, vsfDet.Row, vsfDet.Col)
        End If

        If (Not (Mode = "A" Or Mode = "M")) Then Exit Sub

        With vsfDet
            If (e.KeyCode = Keys.Escape) And .Row = .Rows.Count - 1 And (.Col = vDetItemCode Or .Col = vDetSlNo) Then
                If Trim(.GetData(.Row, vDetItemCode)) = "" Then
                    Call ReArrangeSlNo(vsfDet, vDetSlNo)
                    Exit Sub
                End If
            End If
            If .Col = vDetItemCode And e.KeyCode = Keys.Enter Then
                Call loadItemToDtl()
                Exit Sub
            End If
            If .Col = intLastCol And e.KeyCode = Keys.Tab And .Row = .Rows.Count - 1 Then
                If Trim(.GetData(.Row, vDetItemCode)) <> "" Then
                    .LeftCol = vDetSlNo
                    .Col = vDetSlNo
                    .Col = vDetItemCode
                    .Rows.Count = .Rows.Count + 1
                    .Row = .Rows.Count - 1
                    .TopRow = .Rows.Count - 1
                    .SetData(.Row, vDetSlNo, .Rows.Count - 1)
                Else
                    If .Rows.Count > 2 Then
                        .Rows.Count = .Rows.Count - 1
                    End If
                    Exit Sub
                End If
            End If
            If .Col = vDetItemCode Then
                If e.KeyCode = Keys.Escape Then
                    vsfView.Visible = False
                End If
            End If
            If e.KeyCode = Keys.Down AndAlso vsfView.Visible Then
                vsfView.Focus()
                vsfView.Row = 1
                vsfView.Col = 1
            End If
            Exit Sub

        End With
        If intDetCols < intDetCols Then Exit Sub
    End Sub

    Private Sub vsfDet_LostFocus(sender As Object, e As EventArgs) Handles vsfDet.LostFocus
        If (Not (Mode = "A" Or Mode = "M")) Then Exit Sub
        With vsfDet
            If .Rows.Count = 1 Then
                .Rows.Count = 2
                ReArrangeSlNo(vsfDet, vDetSlNo)
            End If
        End With
    End Sub

    Private Sub vsfDet_SetupEditor(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfDet.SetupEditor
        If (Not (Mode = "A" Or Mode = "M")) Then Exit Sub
        If vsfDet.Col < intDetCols Then Exit Sub

        Call vsfOptionsSetupEditor(Me, vsfDet, vsfOptions, intDocID, "", Mode, e.Row, e.Col, "M")
    End Sub

    Private Sub btnCancel_Click_1(sender As Object, e As EventArgs) Handles btnCancel.Click
        txtPartyCode.Text = ""
        txtDummy.Text = ""
        txtPartyID.Text = ""
        lblPartyName2.Text = ""
        vsfDet.Rows.Count = 1
        vsfDet.Rows.Count = 2
        vsfView.Visible = False
        vsfSearch.Visible = False
        Call ReArrangeSlNo(vsfDet, vDetSlNo)
        vsfDet.Enabled = False ''''Changed on 6Feb19
    End Sub

    Private Sub txtpartycode_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles txtPartyCode.KeyPress
        If e.KeyChar = "'" Then
            e.Handled = True
        End If
    End Sub

    Private Sub txtpartycode_KeyUp(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles txtPartyCode.KeyUp
        Dim dt As DataTable
        If Mode = "V" Then Exit Sub
        Dim strSQL As String, strCond As String = ""

        If e.KeyCode = Keys.Return And txtPartyCode.Text <> "" Then
            Call vsfSearchSelectedItem()
            Exit Sub
        End If

        If e.KeyCode = Keys.Down Then
            blnPartyValidate = True
            vsfSearch.Visible = True
            vsfSearch.Focus()
            If txtPartyCode.Text = "" Then
                vsfSearch.DataSource = Nothing
                vsfSearch.BringToFront()
                vsfSearch.Visible = True

                strSQL = "Select top " & gIntTopNrows & " " & strFldPartyCode & "," & strFldPartyName
                strSQL = strSQL & " from " & strPartyTblName
                strSQL = strSQL & " order by " & strFldPartyCode

                dt = clsSVT.getDATATable(strSQL)
                vsfSearch.DataSource = dt
                vsfSearch.AutoSizeCols()
                vsfSearch.Refresh()
                Exit Sub
            End If
        End If

        If e.KeyCode = Keys.Escape Then
            vsfSearch.Visible = False
            Exit Sub
        End If

        If Len(txtPartyCode.Text) < 1 Then
            vsfSearch.Visible = False
            Exit Sub
        End If

        vsfSearch.DataSource = Nothing
        vsfSearch.BringToFront()
        vsfSearch.Visible = True
        ' Dim dt As DataTable

        strSQL = "Select top " & gIntTopNrows & " " & strFldPartyCode & "," & strFldPartyName
        strSQL = strSQL & " from " & strPartyTblName
        strSQL = strSQL & " Where ( " & strFldPartyCode & " Like '%" & txtPartyCode.Text & "%' or  " & strFldPartyName & " like '%" & txtPartyCode.Text & "%')"

        dt = clsSVT.getDATATable(strSQL)
        vsfSearch.DataSource = dt
        vsfSearch.AutoSizeCols()
        vsfSearch.Refresh()
    End Sub

    Private Sub txtpartycode_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtPartyCode.TextChanged
        If Mode = "M" Or Mode = "A" Then
            If Trim(txtPartyCode.Text) <> "" Then
                vsfSearch.Visible = True
                Dim blnfound As Boolean
                clsSVT.getDataStringValue("Select " & strFldPartyCode & " from " & strPartyTblName & " where " & strFldPartyCode & "='" & Trim(txtPartyCode.Text) & "'", "", blnfound)
                If blnfound Then
                    vsfSearch.Visible = False
                End If
            Else
                vsfSearch.Visible = False
            End If
        End If
    End Sub

    Private Sub vsfSearchSelectedItem()
        If vsfSearch.Rows.Count <= 1 Then Exit Sub
        Dim str As String = txtDummy.Text
        txtPartyCode.Text = vsfSearch.GetData(vsfSearch.Row, 0)
        lblPartyName2.Text = vsfSearch.GetData(vsfSearch.Row, 1)
        txtDummy.Text = vsfSearch.GetData(vsfSearch.Row, 0)

        If Not str = txtDummy.Text Then
            With vsfDet
                .Rows.Count = 1
                .Rows.Count = 2
                Call ReArrangeSlNo(vsfDet, vDetSlNo)
            End With
        End If
        vsfSearch.Visible = False
        btLoad.Focus()
        'Call btLoad_Click(btLoad, e)
    End Sub

    Private Sub vsfSearch_DoubleClick(sender As Object, e As System.EventArgs) Handles vsfSearch.DoubleClick
        vsfSearchSelectedItem()
    End Sub

    Private Sub btSearch_Click(sender As Object, e As EventArgs) Handles btSearch.Click
        Dim frmUS As New frmUtlSingleSel
        Dim strsql As String

        txtPartyID.Text = ""

        strsql = "Select "
        strsql = strsql & " (Select count(" & strFldItemID & ") from " & strPartyTblName & "Item a "
        strsql = strsql & " where a." & strFldPartyID & " = p." & strFldPartyID & ") as ItemsAttached,*"
        strsql = strsql & " from " & strPartyTblName & " p"
        strsql = strsql & " where InActive = 'N'"
        strsql = strsql & " and Authorised = 'Y'"
        strsql = strsql & " order by " & strFldPartyCode

        With frmUS
            .Text = "Select from " & strPartyShForm & " Master... "

            Call .FillViewVsf(Me, strsql, txtPartyID, 1, 1)

            .ShowDialog()
            vsfDet.Rows.Count = 1
            vsfDet.Rows.Count = 2
            Call btLoad_Click(sender, e)
        End With


        If Val(txtPartyID.Text) > 0 Then
            Dim rs As oledb.oledbDataReader
            rs = clsSVT.getDataReaderFromSQL("Select *  from " & strPartyTblName & " where " & strFldPartyID & "=" & Val(txtPartyID.Text))
            If Not rs.HasRows Then
                MsgBox("Invalid Party....", MsgBoxStyle.Critical, Me.Text)
                txtPartyCode.Focus()
                rs.Close()
                Exit Sub
            End If

            rs.Read()
            txtPartyCode.Text = rs.Item(strFldPartyCode)
            lblPartyName2.Text = rs.Item(strFldPartyName)
            rs.Close()
            btLoad.Focus()
        Else
            txtPartyCode.Focus()
        End If
    End Sub

    Private Sub vsfSearch_KeyUp(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles vsfSearch.KeyUp
        If e.KeyCode = Keys.Enter Then
            vsfSearchSelectedItem()
        End If
        If e.KeyCode = Keys.Escape Then
            blnPartyValidate = False
            txtPartyCode.Focus()
            vsfSearch.Visible = False
        End If
    End Sub

    Private Sub txtPartyCode_Validating(sender As Object, e As CancelEventArgs) Handles txtPartyCode.Validating
        If Trim(txtPartyCode.Text) = "" Then Exit Sub
        If blnPartyValidate Then Exit Sub
        If Me.ActiveControl Is Nothing Then Exit Sub
        If vsfSearch.Visible AndAlso vsfSearch.Rows.Count > 0 AndAlso vsfSearch.Name = Me.ActiveControl.Name Then
            blnPartyValidate = True
            Exit Sub
        End If
        Dim blnFound As Boolean
        clsSVT.getDataStringValue("Select " & strFldPartyCode & " from " & strPartyTblName & " where " & strFldPartyCode & " = '" & txtPartyCode.Text & "'", "", blnFound)
        If Not blnFound Then
            MsgBox("Invalid Party....", MsgBoxStyle.Critical, Me.Text)
            txtPartyCode.Focus()
            Exit Sub
        End If

        With vsfDet
            If Not blnMemoradum Then
                .Col = vDetItemCode
                .Row = 1
                .Focus()
                .StartEditing()
            End If
        End With
    End Sub

    Private Sub vsfSearch_LostFocus(sender As Object, e As EventArgs) Handles vsfSearch.LostFocus
        If blnPartyValidate = True Then
            blnPartyValidate = False
            txtPartyCode.Focus()
            Dim obj As Object = Nothing
            Call txtPartyCode_Validating(obj, obj)
        End If
    End Sub

    Private Sub btnTransactions_Click(sender As Object, e As EventArgs) Handles btnTransactions.Click
        Dim intDocID As Int32 = clsSVT.getDataLongValue("Select DocID from UtlTransactions where DocShForm = '" & strPartyShForm & "MEM'", 0, False)
        If intDocID <= 0 Then Exit Sub

        Dim objFrm As New frmTrnMemorandum

        objFrm.initForm(intDocID)
        objFrm.ShowDialog()
        objFrm.Dispose()

    End Sub

    Private Sub btnRemoveRow_Click(sender As Object, e As EventArgs) Handles btnRemoveRow.Click
        If GetVal(vsfDet.GetData(vsfDet.Row, vDetItemname), "") = "" Then
            MsgBox("" & strFldItemCode & " Empty....", MsgBoxStyle.Information, Me.Text)
            Exit Sub
        End If

        If MsgBox("Do you want to delete the" & strFldItemCode & "...?", MsgBoxStyle.YesNo, Me.Text) = MsgBoxResult.Yes Then
            vsfDet.Rows.Remove(vsfDet.Row)
            MsgBox("Click on save to update new item list", MsgBoxStyle.Information, Me.Text)
        End If
    End Sub

    Private Sub vsfDet_MouseClick(sender As Object, e As MouseEventArgs) Handles vsfDet.MouseClick

        If Not blnMemoradum Then Exit Sub
        If vsfDet.Col = vDetItemCode Then
            If Val(vsfDet.GetData(vsfDet.Row, vDetItemID)) > 0 Then
                gbMemorandum.Text = vsfDet.GetData(vsfDet.Row, vsfDet.Col)
                With vsfMem
                    .Rows.Count = 1

                    Dim rs As oledb.oledbDataReader
                    rs = clsSVT.getDataReaderFromSQL("Select * from " & strMstTableName & " where " & strFldItemID & " = " & Val(vsfDet.GetData(vsfDet.Row, vDetItemID)))

                    If Not rs.HasRows Then
                        rs.Close()
                        Exit Sub
                    End If

                    Do While rs.Read
                        .Rows.Count = .Rows.Count + 1
                        .SetData(.Rows.Count - 1, vMemNo, rs.Item(strMstShForm & "CODE"))
                        .SetData(.Rows.Count - 1, vMemDate, rs.Item("AddedOn"))
                        .SetData(.Rows.Count - 1, vMemEffDtFrom, rs.Item("EffDateFrom"))
                        .SetData(.Rows.Count - 1, vMemEffDtTo, rs.Item("EffDateTo"))
                        .SetData(.Rows.Count - 1, vMemRate, rs.Item("Rate"))
                    Loop

                    rs.Close()
                    .AutoSizeCols()
                End With
            End If
        End If
    End Sub

    Private Sub btnExport_Click(sender As Object, e As EventArgs) Handles btnExport.Click
        If vsfDet.Rows.Count > 1 AndAlso Trim(vsfDet.GetData(1, vDetItemCode)) <> "" Then
            Call ExportToXL(vsfDet)
        End If

    End Sub

    Private Sub btLoad_CausesValidationChanged(sender As Object, e As EventArgs) Handles btLoad.CausesValidationChanged

    End Sub


End Class