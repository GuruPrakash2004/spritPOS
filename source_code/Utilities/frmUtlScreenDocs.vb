Imports System.IO
Imports C1.Win.C1FlexGrid
Public Class frmUtlScreenDocs

    Dim clsSVT As New svtLibrary
    Dim Mode As String
    Dim dlg As New OpenFileDialog()
    Dim strformcaption As String = ""
    Const vDetDocID = 0
    Const vDetSLno = 1
    Const vDetFrmCaption = 2
    Const vDetGroup = 3
    Const vDetFileName = 4
    Const vDetRemarks = 5
    Dim intSLNo As Integer
    Dim SvtTrn As oledb.oledbTransaction
    Public strFileName As String
    Public strFilepath As String

    Public argFormCaption As String = ""

    Private Sub frmUtlScreenDocs_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        strformcaption = ""
        pnlAddScreenDocs.Visible = False
        'Try
        '    If frmMain.ActiveMdiChild.Text = "" Then
        '        lblformcaption.Text = frmMain.Text
        '        strformcaption = frmMain.Name
        '    Else
        '        lblformcaption.Text = frmMain.ActiveMdiChild.Text
        '        strformcaption = frmMain.ActiveMdiChild.Name
        '    End If

        'Catch ex As Exception
        '    lblformcaption.Text = frmMain.Text
        '    strformcaption = frmMain.Name
        '    Exit Sub
        'End Try
        lblformcaption.Text = argFormCaption
        strformcaption = argFormCaption

        Initialization()


    End Sub

    Private Function GlobalFilePath(ByRef argFileName As String) As String
        If Not File.Exists(gstrRptPath & "ScreenDocs") Then
            Directory.CreateDirectory(gstrRptPath & "ScreenDocs")
        End If
        Return gstrRptPath & "ScreenDocs\" & Replace(argFileName, "'", "")
    End Function

    Private Sub Initialization()
        With vsfDet
            .Cols.Count = 6
            .Rows.Count = 2
            .SetData(0, vDetDocID, "DOCID")
            .SetData(0, vDetSLno, "Sl No")
            .SetData(0, vDetFrmCaption, "Form Caption")
            .SetData(0, vDetGroup, "Group Name")
            .SetData(0, vDetFileName, "File Name")
            .SetData(0, vDetRemarks, "Remarks")
            .AllowEditing = False
            .ExtendLastCol = True
            .Styles("Fixed").Font = New Font("Segoe UI", 12)
            .Styles.Fixed.TextAlign = TextAlignEnum.CenterCenter
            .Styles.Normal.TextAlign = TextAlignEnum.LeftCenter
            '.Styles("Fixed").TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter
        End With
        'SetColAlign(vsfDet, svtAlign.Center, vsfDetSLno, vsfDetGroup, vsfDetFileName, vsfDetRemarks)
        Call initGridStyles(Me)
        Call SetColWidth(vsfDet, 80, 100, 200, 200, 200)
        SetHideCol(vsfDet, True, vDetFrmCaption, vDetDocID)
        SetFlexColor(vsfDet, "", vDetFileName, vDetGroup, vDetRemarks, vDetSLno)
        Mode = "V"
        pnlAddScreenDocs.Visible = False
        fillDocDetails(strformcaption)
        ReArrangeSlNo(vsfDet, vDetSLno)
        vsfDetHeight()
        vsfDet.Focus()
        'vsfDet.Cols(vDetGroup).AllowMerging = True
        'vsfDet.AllowMerging = AllowMergingEnum.RestrictRows
    End Sub

    Private Sub vsfDetHeight()
        Dim FixedRowHeight As Int32 = 40
        Dim NormalRowHeight As Int32 = 30
        vsfDet.Font = New Font(vsfDet.Font.FontFamily, 12)
        For Each row As C1.Win.C1FlexGrid.Row In vsfDet.Rows
            If row.Index < vsfDet.Rows.Fixed Then
                'For Fixed rows
                row.HeightDisplay = FixedRowHeight
            Else
                'For other rows
                row.HeightDisplay = NormalRowHeight
            End If
        Next
    End Sub
    Private Sub btnBrowse_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowse.Click
        If dlg.ShowDialog() = DialogResult.OK Then
            ' txtexpFile.Text = Path.GetFileName(Replace(dlg.FileName, "'", ""))
            txtDoc.Text = Path.GetFileName(Replace(dlg.FileName, "'", ""))
            Exit Sub
        End If
    End Sub
    Private Function ValidFields() As Boolean
        ValidFields = False
        If txtGroupName.Text = "" Then
            MsgBox("Please enter the group name!", MsgBoxStyle.Critical)
            txtGroupName.Focus()
            Exit Function
        End If
        If txtDoc.Text = "" Then
            MsgBox("File attachment name is blank!", MsgBoxStyle.Critical)
            btnBrowse.Focus()
            Exit Function
        End If
        ValidFields = True
    End Function
    Private Sub vsfDet_DoubleClick(sender As Object, e As System.EventArgs) Handles vsfDet.DoubleClick
        With vsfDet
            If .MouseCol = vDetFileName Then
                Dim strFilePath As String
                Try
                    With vsfDet
                        If Trim(.GetData(.Row, vDetFileName)) <> "" Then
                            strFilePath = gstrRptPath & "ScreenDocs\" & Replace(.GetData(.Row, vDetFileName), "'", "")
                            If (File.Exists(strFilePath)) Then
                                Process.Start(strFilePath)
                            Else
                                MsgBox("This  file :" & .GetData(.Row, vDetFileName) & " is not Foundin this directory " & gstrRptPath & "ScreenDocs\" & vbCrLf, vbCritical, Me.Text)
                            End If
                        Else
                        End If
                    End With
                Catch ex As Exception
                    If vsfDet.Rows.Count > 1 Then
                        MsgBox("This file" & .GetData(.Row, vDetFileName) & " does not exist in this directory" & gstrRptPath & "ScreenDocs\" & vbCrLf & ex.Message, vbCritical, Me.Text)
                        Exit Sub
                    End If
                End Try
            End If

            If .MouseCol = vDetGroup Or .MouseCol = vDetRemarks Then
                If vsfDet.Rows.Count > 1 Then
                    txtGroupName.Text = Trim(.GetData(.Row, vDetGroup))
                    txtDoc.Text = Trim(.GetData(.Row, vDetFileName))
                    txtRemarks.Text = .GetData(.Row, vDetRemarks)
                    txtDocID.Text = .GetData(.Row, vDetDocID)
                    enableDisable(False, False)
                    Mode = "M"
                    pnlAddScreenDocs.Visible = True
                End If
            End If


        End With

    End Sub

    Private Sub vsfDet_RowColChange(ByVal sender As Object, ByVal e As System.EventArgs) Handles vsfDet.RowColChange
        Dim strfname As String = ""
        Dim clsfileinfo As System.IO.FileInfo
        Dim intCalculatedsize As Decimal
        If vsfDet.Rows.Count > 1 Then
            With vsfDet
                If .GetData(.Row, vDetFileName) <> "" And .Row > 0 Then
                    lblfname.Visible = True
                    lblfsize.Visible = True
                    lblmods.Visible = True
                    Pnlfilesproperty.Visible = True
                    Try
                        strfname = GlobalFilePath(.GetData(.Row, vDetFileName))
                    Catch es As Exception
                        MsgBox("This  file :" & .GetData(.Row, vDetFileName) & "does not exist in this directory " & gstrRptPath & "ScreenDocs\" & vbCrLf, vbCritical, Me.Text)
                    End Try
                    clsfileinfo = My.Computer.FileSystem.GetFileInfo(strfname)
                    intCalculatedsize = Math.Round((clsfileinfo.Length / 1024), 2)
                    lblFileSize.Text = "File Size: " & intCalculatedsize & " kb"
                    lblmod.Text = "Last Modified: " & clsfileinfo.LastWriteTime
                Else
                    Pnlfilesproperty.Visible = False
                End If
            End With
        End If

        Exit Sub
    End Sub
    Private Sub btnAdd_Click_1(sender As System.Object, e As System.EventArgs) Handles btnAdd.Click
        pnlAddScreenDocs.Visible = True
        Mode = "A"
        enableDisable(False, True)
        txtGroupName.Focus()

    End Sub

    Private Sub btnClose_Click_1(sender As System.Object, e As System.EventArgs) Handles btnClose.Click
        strformcaption = ""
        Me.Close()
        'frmMain.tvMain.SelectedNode = Nothing
    End Sub

    Private Sub btnCancel_Click_1(sender As System.Object, e As System.EventArgs) Handles btnCancel.Click
        ClearFlds()
        enableDisable(True, False)
        With vsfDet
            If .Rows.Count > 1 Then
                btnEdit.Enabled = True
                btnRemove.Enabled = True
            Else
                btnEdit.Enabled = False
                btnRemove.Enabled = False
                Pnlfilesproperty.Visible = False
            End If

        End With
        pnlAddScreenDocs.Visible = False
    End Sub

    Private Sub ClearFlds()
        txtGroupName.Text = ""
        txtRemarks.Text = ""
        txtDoc.Text = ""
        pnlAddScreenDocs.Visible = False
        vsfDet.Enabled = True
        btnAdd.Enabled = True
        btnClose.Enabled = True
        fillDocDetails(strformcaption)
    End Sub

    Private Sub btnSave_Click(sender As System.Object, e As System.EventArgs) Handles btnSave.Click
        stringReplace(Me)
        If ValidFields() Then
            Call SaveDetails()
            ClearFlds()
        End If
    End Sub

    Private Sub fillDocDetails(ByVal argFrmCapt As String)
        Dim RsTemp As oledb.oledbDataReader
        Dim strSQL As String
        Dim i As Integer
        'Dim i As Integer, X As Integer
        If argFrmCapt = "" Then Exit Sub
        'txtfrmCaption.Text = argFrmCapt
        strSQL = " select DocID,FrmCaption,GrpName,FileName,Remarks from UtlScreenDocs where FrmCaption= '" & argFrmCapt & "' order by GrpName"
        RsTemp = clsSVT.getDataReaderFromSQL(strSQL)
        With vsfDet
            .Rows.Count = 1

            Do While RsTemp.Read()
                i = .Rows.Count
                .Rows.Count = .Rows.Count + 1
                .SetData(i, vDetGroup, RsTemp!GrpName)
                .SetData(i, vDetFileName, RsTemp!FileName)
                .SetData(i, vDetRemarks, RsTemp!Remarks)
                .SetData(i, vDetFrmCaption, RsTemp!FrmCaption)
                .SetData(i, vDetDocID, RsTemp!DocID)
            Loop
            RsTemp.Close()
        End With
        vsfDetHeight()
        With vsfDet
            If .Rows.Count > 1 Then
                .Col = vDetSLno
                .Row = 1
                .Focus()
            Else

            End If

            If .Rows.Count = 1 Then
                btnEdit.Enabled = False
                btnRemove.Enabled = False
            Else
                btnEdit.Enabled = True
                btnRemove.Enabled = True
                btnAdd.Focus()
            End If

        End With
        ReArrangeSlNo(vsfDet, vDetSLno)
    End Sub
    Private Sub ClearFieldvsfVwdet()
        With vsfDet
            .Rows.Count = 1
            .Rows.Count = 2
        End With
    End Sub

    Private Function SaveDetails() As Boolean
        Dim strSQL As String = ""
        SaveDetails = True
        SvtTrn = svtConn.BeginTransaction(IsolationLevel.ReadCommitted)
        If Mode = "A" Then
            intSLNo = clsSVT.getDataLongValue("select max(SlNo) from UtlScreenDocs where FrmCaption ='" & strformcaption & "'", 0, False, SvtTrn)
            If intSLNo = 0 Then
                intSLNo = 1
            Else
                intSLNo += intSLNo
            End If
            txtSlNo.Text = intSLNo
            If File.Exists(GlobalFilePath(txtDoc.Text)) Then
                If MsgBox("Are you sure you want to overwrite existing document'" & _
                          txtDoc.Text, _
                          vbYesNo + vbQuestion, Me.Text) = vbYes Then
                    strSQL = "insert into UtlScreenDocs (FrmCaption, GrpName, FileName, Remarks,SlNo, AddedBy, AddedOn) values("

                    strSQL = strSQL & "'" & strformcaption & "',"
                    strSQL = strSQL & "'" & txtGroupName.Text & "',"
                    strSQL = strSQL & "'" & txtDoc.Text & "',"
                    strSQL = strSQL & "'" & txtRemarks.Text & "',"
                    strSQL = strSQL & "" & Val(txtSlNo.Text) & ","
                    strSQL = strSQL & "'" & gUserCode & "', "
                    strSQL = strSQL & "'" & getDateFormatInInsert(getSystemDateTime(SvtTrn), True) & "')"
                    File.Copy(dlg.FileName, gstrRptPath & "ScreenDocs\" & txtDoc.Text, True)
                Else
                    vsfDet.Focus()
                    Exit Function
                End If
            Else
                strSQL = "insert into UtlScreenDocs (FrmCaption, GrpName, FileName, Remarks,SlNo, AddedBy, AddedOn) values("

                strSQL = strSQL & "'" & strformcaption & "',"
                strSQL = strSQL & "'" & txtGroupName.Text & "',"
                strSQL = strSQL & "'" & txtDoc.Text & "',"
                strSQL = strSQL & "'" & txtRemarks.Text & "',"
                strSQL = strSQL & "'" & Val(txtSlNo.Text) & "',"
                strSQL = strSQL & "'" & gUserCode & "', "
                strSQL = strSQL & "'" & getDateFormatInInsert(getSystemDateTime(SvtTrn), True) & "')"

                File.Copy(dlg.FileName, gstrRptPath & "ScreenDocs\" & txtDoc.Text, False)
            End If
        Else
            strSQL = "update UtlScreenDocs set Remarks= '" & txtRemarks.Text & "',"
            strSQL = strSQL & " ChangedBy = '" & gUserCode & "', "
            strSQL = strSQL & " ChangedOn = '" & getDateFormatInInsert(getSystemDateTime(SvtTrn), True) & "'"
            strSQL = strSQL & " where FrmCaption= '" & strformcaption & "' and DocID =" & Val(vsfDet.GetData(vsfDet.Row, vDetDocID))
        End If
        Try

            If clsSVT.execSQLStatement(strSQL, SvtTrn) Then
                If Mode = "A" Then
                    MsgBox("Details saved Successfully!!!", vbInformation, Me.Text)

                Else
                    MsgBox("Details Updated Successfully!!!", vbInformation, Me.Text)

                End If
            End If
        Catch ex As Exception
            MsgBox("Error while inserting  data!" & vbCrLf & ex.Message, vbCritical, Me.Text)
            SvtTrn.Rollback()
            Exit Function
        End Try

        SvtTrn.Commit()
        fillDocDetails(strformcaption)
    End Function

    Private Sub btnRemove_Click_1(sender As System.Object, e As System.EventArgs) Handles btnRemove.Click
        Dim strActForm As String = ""
        With vsfDet
            If .Row >= 1 Then
                If MsgBox("Are you sure you want to delete this " & .GetData(.Row, vDetFileName) & " document ", _
                          vbYesNo + vbCritical, Me.Text) = vbYes Then
                    DeleteDoc()
                    fillDocDetails(strformcaption)
                End If
            Else
                MsgBox("This  file :" & .GetData(.Row, vDetFileName) & " does not exist in this directory " & gstrRptPath & "ScreenDocs\" & vbCrLf, vbCritical, Me.Text)
                Exit Sub
            End If
        End With

        vsfDet.Focus()
    End Sub
    Private Sub DeleteDoc()

        SvtTrn = svtConn.BeginTransaction(IsolationLevel.ReadCommitted)
        With vsfDet
            Try
                clsSVT.execSQLStatement("Delete  from UtlScreenDocs where FileName = '" &
                        .GetData(.Row, vDetFileName) & "'", SvtTrn)

                File.Delete(GlobalFilePath(.GetData(.Row, vDetFileName)))
            Catch ex As FileNotFoundException
                MsgBox("This  file :" & .GetData(.Row, vDetFileName) & " does not exist in this directory " & gstrRptPath & "ScreenDocs\" & vbCrLf, vbCritical, Me.Text)
                Exit Sub
            Catch ex As oledb.oledbException
                MsgBox("Error while deleting  file!" & vbCrLf & ex.Message, vbCritical, Me.Text)
                SvtTrn.Rollback()
            End Try

        End With

        SvtTrn.Commit()
    End Sub

  
    Private Sub btnDelete_Click(sender As System.Object, e As System.EventArgs) Handles btnDelete.Click

        If dlg.FileName = "" Then
            MsgBox("Please browse a file!", vbInformation)
            btnBrowse.Focus()
        Else
            If MsgBox("Are you sure you want to delete this " & txtDoc.Text & " selected document'", vbYesNo + vbCritical, Me.Text) = vbYes Then
                dlg.FileName = ""
                txtDoc.Text = ""
                btnBrowse.Focus()
            End If
        End If
       
    End Sub

    Private Sub btnEdit_Click(sender As System.Object, e As System.EventArgs) Handles btnEdit.Click
        Mode = "M"
        With vsfDet


            txtGroupName.Text = Trim(.GetData(.Row, vDetGroup))
            txtDoc.Text = Trim(.GetData(.Row, vDetFileName))
            txtRemarks.Text = .GetData(.Row, vDetRemarks)
            txtDocID.Text = .GetData(.Row, vDetDocID)
        End With
        enableDisable(False, False)
        Mode = "M"
        pnlAddScreenDocs.Visible = True
    End Sub
    Private Sub enableDisable(ByVal argBln As Boolean, ByVal argblnedit As Boolean)


        vsfDet.Enabled = argBln
        btnAdd.Enabled = argBln
        btnEdit.Enabled = argBln
        btnRemove.Enabled = argBln

        btnClose.Enabled = argBln
      

            txtGroupName.Enabled = argblnedit
            btnBrowse.Enabled = argblnedit
            btnDelete.Enabled = argblnedit

    End Sub

    Private Sub vsfDet_Click(sender As System.Object, e As System.EventArgs) Handles vsfDet.Click

    End Sub


End Class