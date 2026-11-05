Imports System.Windows.Forms
Imports System.Data.oledb

Public Module ViewForm
    Dim clsSVT As New svtLibrary

    Public Sub showViewForm(ByVal argFrm As Form, ByVal argTblName As String, ByVal argHdr1 As String, ByVal argHdr2 As String, Optional ByVal argDTFldName As String = "", Optional ByVal argAdnlCond As String = "", Optional ByVal argDocName As String = "", Optional ByVal argMainFld As String = "", Optional ByVal argDOCID As Integer = 0, Optional ByVal argQry As String = "", Optional argOrderbyQry As String = "")
        Dim strSQL As String, strFMT As String
        strFMT = ""

        If argDocName = "" Then
            argDocName = argFrm.Text
        End If

        If Trim(argQry) = "" Then
            strSQL = getShowFindQry(argTblName, strFMT, argDocName, argDTFldName, argAdnlCond, argDOCID, argOrderbyQry)
        Else
            strSQL = argQry
        End If

        For Each frm As Form In Application.OpenForms
            If frm.Tag = "SVTSEARCH-" & argFrm.Text Then
                frm.WindowState = FormWindowState.Normal
                frm.Focus()
                Exit Sub
            End If
        Next

        Dim frmDisplay As New frmUtlMstView
        With frmDisplay
            .Text = argDocName
            Call .FillViewVsf(argFrm, strSQL, argHdr1, argHdr2, argDOCID, argDocName)
            Call .searchSettings(argDocName, argTblName, argMainFld)
            .Tag = "SVTSEARCH-" & argFrm.Text
            .Show()
        End With
    End Sub

    Public Sub showSelViewForm(ByVal argFrm As Form, ByVal argTblName As String, ByVal argHdr1 As String, ByVal argHdr2 As String, Optional ByVal argDTFldName As String = "", Optional ByVal argAdnlCond As String = "", Optional ByVal argDocName As String = "", Optional ByVal argMainFld As String = "", Optional ByVal argDOCID As Integer = 0, Optional ByVal argQry As String = "", Optional argOrderbyQry As String = "")
        Dim strSQL As String, strFMT As String
        strFMT = ""

        If argDocName = "" Then
            argDocName = Trim(argFrm.Text)
        End If

        If Trim(argQry) = "" Then
            strSQL = getShowFindQry(argTblName, strFMT, argDocName, argDTFldName, argAdnlCond, argDOCID, argOrderbyQry)
        Else
            strSQL = argQry
        End If
        'Dim frmDisplay As New frmUtlMstPartyAddress
        Dim frmDisplay As New frmUtlMstView
        With frmDisplay
            .Text = argDocName
            Call .FillViewVsf(argFrm, strSQL, argHdr1, argHdr2, argDOCID, argDocName)
            Call .searchSettings(argDocName, argTblName, argMainFld)
            .ShowDialog()
        End With
    End Sub

    Public Sub showViewTrnForm(ByVal argFrm As Form, ByVal argTblName As String, ByVal argHdr1 As String, ByVal argHdr2 As String, Optional ByVal argDTFldName As String = "", Optional ByVal argAdnlCond As String = "", Optional ByVal argDocName As String = "", Optional ByVal argMainFld As String = "", Optional ByVal argDOCID As Integer = 0, Optional ByVal argQry As String = "")
        Dim strSQL As String, strFMT As String
        strFMT = ""

        If Trim(argQry) = "" Then
            strSQL = getShowFindQry(argTblName, strFMT, argDtFld:=argDTFldName, argAdnlCond:=argAdnlCond, argDocId:=argDOCID, argDocName:=argDocName)
        Else
            strSQL = argQry
        End If



        Dim frmDisplay As New frmUtlTrnView

        'strRights = getUserRights(argDocID)
        With frmDisplay
            '.MdiParent = frmMain
            .Text = argDocName
            Call .FillViewVsf(argFrm, strSQL, argHdr1, argHdr2, argDOCID, argDocName)
            Call .searchSettings(argDocName, argTblName, argMainFld)
            .StartPosition = FormStartPosition.CenterScreen
            'frmMain_New.dockTabMain.TabPages.
            '.MdiParent = frmMain_New
            .Tag = "SVTTRNSEARCH-" & argFrm.Text
            .Show()

            '.vsfDet.Focus()

            'If Not chkOnlyUserRights(strRights, printRights) Then
            '    .cmdExport.Enabled = False
            'Else
            '    .cmdExport.Enabled = True
            'End If
        End With
    End Sub

    Public Function getShowFindQry(ByVal argTblName As String, _
                                ByRef strFormat As String, Optional ByVal argDocName As String = "", _
                                Optional ByVal argDtFld As String = "", Optional ByVal argAdnlCond As String = "", Optional ByVal argDocId As Integer = 0, Optional argOrderByQry As String = "") As String
        Dim rsFlds As oledb.oledbDataReader

        Dim blnFound As Boolean, strQry As String, blnTblFound As Boolean
        Dim strFldList As String = "", strTblList As String = ""
        Dim strCondList As String = "", strSTG As String
        Dim strFld As String = "", strTbl As String
        Dim strSQL As String, i As Integer, strFMT As String = ""
        Dim strTableList As New List(Of String)
        Dim strFldCaption As String = ""
        argTblName = UCase$(argTblName)
        strTableList.Clear()
        argDocName = argDocName & "View"
        getShowFindQry = ""


        strSQL = "Select * from  " & argTblName & " where 1 <> 1"
        If argDtFld <> "" Then
            strSQL = strSQL & " and " & getDateCond(argDtFld)
        End If

        rsFlds = clsSVT.getDataReaderFromSQL(strSQL)


        strTblList = argTblName & ","
        Dim strDocName As String = ""
        strDocName = clsSVT.getDataStringValue("Select DocShForm from UtlTransactions where DocId=" & argDocId, "", True)

        'Dim strFldCodeCaption() As String
        'strFldCodeCaption = Split(strFldCaption, ";")
        With rsFlds
            For i = 0 To .FieldCount - 1

                Dim strFldName As String = ""

                Select Case UCase(Trim(.GetName(i)))
                    Case "IDNO", "[ID No]"
                        strFldName = "IdentityNo"
                    Case "AMMENDMENT", "IMGPRINT", "AMMDETAILS", "AMMNO", "AMMDATE", "AUTHORISED", "AUTHON", "AUTHBY", "ADDEDBY", "ADDEDON", "CHANGEDBY", "CHANGEDON"
                        GoTo NxtFld
                    Case strDocName & "SERIES", IIf(InStr(UCase(argTblName), "TRN") > 0, "", strDocName & "NO")
                        If clsSVT.getDataStringValue("Select MstAutoGenCode from UtlTransactions where DocId=" & argDocId, "N", True) = "Y" Then
                            strFldName = UCase((.GetName(i)))
                        Else
                            GoTo nxtfld
                        End If
                    Case ""

                    Case Else
                        strFldName = ((.GetName(i)))
                        'strFldName = getFldCodeCaption((.GetName(i)), strFldCodeCaption(i))
                End Select

                If UCase$(.GetName(i)) <> "RGUID" Then
                    blnFound = False
                    If InStr(1, UCase(gStrUtlFldLst), UCase$(.GetName(i))) > 0 Then
                        strQry = "Select DisplayFld from UtlFindFlds where TableName <> '" & argTblName & "' AND  " &
                                            " (StorageFld ='" & .GetName(i) & "' OR " &
                                            " AliasFld1 ='" & .GetName(i) & "' OR " &
                                            " AliasFld2 ='" & .GetName(i) & "' OR " &
                                            " AliasFld3 ='" & .GetName(i) & "' OR " &
                                            " AliasFld4 ='" & .GetName(i) & "' OR " &
                                            " AliasFld5 ='" & .GetName(i) & "')"
                        strFld = UCase(clsSVT.getDataStringValue(strQry, "", blnFound))
                    End If

                    If blnFound = True Then
                        strQry = "Select StorageFld from UtlFindFlds where TableName <> '" & argTblName & "' AND  " &
                                            " (StorageFld ='" & .GetName(i) & "' OR " &
                                            " AliasFld1 ='" & .GetName(i) & "' OR " &
                                            " AliasFld2 ='" & .GetName(i) & "' OR " &
                                            " AliasFld3 ='" & .GetName(i) & "' OR " &
                                            " AliasFld4 ='" & .GetName(i) & "' OR " &
                                            " AliasFld5 ='" & .GetName(i) & "')"

                        strSTG = UCase(clsSVT.getDataStringValue(strQry, "", False))


                        strQry = "Select TableName from UtlFindFlds where TableName <> '" & argTblName & "' AND  " &
                            " (StorageFld ='" & .GetName(i) & "' OR " &
                            " AliasFld1 ='" & .GetName(i) & "' OR " &
                            " AliasFld2 ='" & .GetName(i) & "' OR " &
                            " AliasFld3 ='" & .GetName(i) & "' OR " &
                            " AliasFld4 ='" & .GetName(i) & "' OR " &
                            " AliasFld5 ='" & .GetName(i) & "')"

                        strTbl = UCase(clsSVT.getDataStringValue(strQry, "", False))

                        'add to table list

                        'If strTblList <> strTbl Then
                        '    strTblList = strTblList & strTbl & ","
                        '    strCondList = strCondList & " and " & argTblName & "." & .GetName(i) & _
                        '                    " = " & strTbl & "." & strSTG
                        'End If
                        blnTblFound = False
                        For Each strTblListItem As String In strTableList
                            If UCase(Trim(strTblListItem)) = UCase(Trim(strTbl)) Then
                                blnTblFound = True
                            End If
                        Next
                        If Not blnTblFound Then
                            strTableList.Add(strTbl)
                            strTblList = strTblList & strTbl & ","
                            strCondList = strCondList & " and " & argTblName & "." & .GetName(i) &
                                            " = " & strTbl & "." & strSTG
                        End If

                        'add to fld list
                        '                    strFldList = strFldList & strTbl & "." & strFld & ","
                        'If .Fields(i).Type = adDate Then
                        '    If gStrConnectionType = gCnstAccess Then
                        '        strFldList = strFldList & "Format(" & _
                        '                strTbl & "." & strFld & ",'dd/MM/yyyy') AS " & strFldName & ","
                        '    Else
                        '        strFldList = strFldList & "Format(" & _
                        '                strTbl & "." & strFld & ",'dd mmm yyyy') AS " & strFldName & ","
                        '    End If
                        'Else

                        strFldList = strFldList & strTbl & "." & strFld & ","
                        strFldList = Replace(strFldList, ",,", ",")

                        'End If
                        strFMT = strFMT & strFld & " |"
                    Else
                        'If .Fields(i).Type = adDate Then
                        '    If gStrConnectionType = gCnstAccess Then
                        '        strFldList = strFldList & "Format(" & argTblName & _
                        '        "." & UCase$(clsSVT.GetVal(.GetName(i), "")) & ", 'dd/MM/yyyy') AS " & strFldName & ","
                        '    Else
                        '        strFldList = strFldList & "Format(" & argTblName & _
                        '        "." & UCase$(clsSVT.GetVal(.GetName(i), "")) & ", 'dd mmm yyyy') AS " & strFldName & ","
                        '    End If
                        'Else

                        strFldList = strFldList & argTblName & "." & UCase$(GetVal(.GetName(i), "")) & " AS " & strFldName & ","
                        'strFldList = strFldList & argTblName & "." & UCase$(GetVal(.GetName(i), "")) & " [" & strFldName & "], " '& " AS " & strFldName & ","
                        'End If
                        strFMT = strFMT & UCase$(GetVal(.GetName(i), "")) & " |"
                    End If

                End If
NxtFld:
            Next i
            .Close()
        End With

        strFldList = Mid(strFldList, 1, Len(strFldList) - 1)
        strTblList = Mid(strTblList, 1, Len(strTblList) - 1)
        strFormat = Mid(strFMT, 1, Len(strFMT) - 1)

        strSQL = "SELECT " & strFldList

        strSQL = strSQL & " FROM " & strTblList
        strSQL = strSQL & " WHERE 1=1 " & strCondList
        strSQL = strSQL & argAdnlCond
        If argDtFld <> "" Then
            If UCase(argTblName) <> "TRNHDRINV" Then
                strSQL = strSQL & " and " & getDateCond(argDtFld)
            Else
                If gUserCode = "ADMIN" Then
                    strSQL = strSQL & " and " & getDateCond(argDtFld)
                Else
                    strSQL = strSQL & " and " & argDtFld & " = '" & Format(getSystemDate, "dd MMM yyyy") & "'"
                End If
            End If
        End If
        strSQL = strSQL & " Order by 1 desc"
        getShowFindQry = strSQL

    End Function

    Private Function getFldCaptions(ByVal argFldlist As String, ByVal argfldCaption As String) As String
        getFldCaptions = ""
        Dim strCaptions() As String
        Dim strCode() As String
        Dim strFldCode() As String
        Dim k As Integer = 0
        Dim j As Integer = 0
        Dim strflds As String
        strCaptions = Split(argfldCaption, ";")
        strFldCode = Split(argFldlist, ",")
        For j = 0 To UBound(strFldCode) - 1
            For k = 0 To UBound(strCaptions) - 1
                strCode = Split(strCaptions(k), ":")
                If InStr(strFldCode(j), strCode(0)) > 0 Then
                    strflds = Replace(strFldCode(j), strCode(0), strCode(0) & " as " & strCode(1))
                    getFldCaptions = getFldCaptions & strflds & ","
                End If
            Next
        Next j
        ' getFldCaptions = argFldlist
    End Function

    Private Function getFldCodeCaption(ByVal argFldCode As String, ByVal argFldCodeCaption As String)
        getFldCodeCaption = ""
        Dim strFldCaption() As String
        strFldCaption = Split(argFldCodeCaption, ":")
        getFldCodeCaption = strFldCaption(1)
    End Function

End Module
