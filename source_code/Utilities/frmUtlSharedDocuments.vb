Imports System.IO
Imports C1.Win.C1FlexGrid


Public Class frmUtlSharedDocuments

    Dim clsSVT As New svtLibrary
    Dim Mode As String

    Const vsfDetGroup = 0
    Const vsfDetIcon = 1
    Const vsfDetFileType = 2
    Const vsfDetFileName = 3
    Const vsfDetRemarks = 4

    Public strFileName As String
    Public strFilepath As String
    Public strExtnName As String
    Public img As Image = Nothing

    Private Sub frmUtlSharedDocuments_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not Directory.Exists(gstrRptPath & "SharedDocs") Then
            Directory.CreateDirectory(gstrRptPath & "SharedDocs")
        End If
        With vsfDet
            .Cols.Count = 5
            .Rows.Count = 2

            .SetData(0, vsfDetGroup, "Group")
            .SetData(0, vsfDetIcon, "Icon")
            .SetData(0, vsfDetFileType, "File Type")
            .SetData(0, vsfDetFileName, "File Name")
            .SetData(0, vsfDetRemarks, "Remarks")
            .AllowEditing = False
            .Cols(vsfDetIcon).Visible = False
            .Cols(vsfDetIcon).DataType = GetType(PictureBox)
            .ExtendLastCol = True
        End With
        Call initGridStyles(Me)
        'vsfDet.AutoSizeRows()
        'vsfDet.AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.RestrictCols
        vsfDet.AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.RestrictAll
        Dim j%
        For j = vsfDet.Cols.Fixed To vsfDet.Cols.Count - 1
            vsfDet.Cols(vsfDetGroup).AllowMerging = True
        Next

        Call SetColWidth(vsfDet, 80, 50, 150, 200, 250)
        fillVsfDet()

        If vsfDet.Rows.Count > 1 Then
            vsfDet.Row = 1
            Call vsfDet_RowColChange(sender, e)
        End If
        Mode = "V"

        Me.KeyPreview = True
    End Sub

    Private Sub btnClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClose.Click
        frmMain.dockTabMain.SelectedTab.Dispose()
    End Sub

    Private Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        pnlAddSharedDocs.Visible = True
        Call bindCombo(cboGroup, "GroupName", "GroupName", "select distinct GroupName from UtlSharedDocs order by GroupName")
        cboGroup.Focus()
        Mode = "A"
        btnBrowse.Visible = True
        cboGroup.SelectedIndex = -1
        txtRemarks.Text = ""
        txtDoc.Text = ""
    End Sub

    Private Sub DeleteDoc()
        With vsfDet
            clsSVT.execSQLStatement("Delete  from utlSharedDocs where DocFileName = '" & _
                    .GetData(.Row, vsfDetFileName) & "'")
            File.Delete(gstrRptPath & "\SharedDocs\" & .GetData(.Row, vsfDetFileName))
        End With
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        pnlAddSharedDocs.Visible = False
    End Sub

    Private Sub btnBrowse_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowse.Click
        Dim n As Integer
        Dim blnFound As Boolean
        'On Error GoTo err
        Try

            With fsd
                .FileName = ""
                .ShowDialog()
                If fsd.FileName = "" Then Exit Sub
                strFileName = fsd.FileName
                strExtnName = System.IO.Path.GetExtension(fsd.FileName)
                strFilepath = System.IO.Path.GetFullPath(fsd.FileName)
                blnFound = True
                While blnFound
                    n = InStr(1, strFileName, "\")
                    If n <> 0 Then
                        strFileName = Microsoft.VisualBasic.Right(strFileName, Len(strFileName) - n)
                    Else
                        blnFound = False
                    End If
                End While

                txtDoc.Text = strFileName
                strFileName = strFilepath

            End With
        Catch ex As Exception
        End Try
        'err:
    End Sub


    Private Function ValidData() As Boolean
        Dim blnfound As Boolean
        Dim strDocID As String

        ValidData = False

        If Trim(cboGroup.Text) = "" Then
            MsgBox("Group name cannot be empty!", vbInformation + vbOKOnly, Me.Text)
            cboGroup.Focus()
            Exit Function
        End If
        If Trim(txtDoc.Text) = "" Then
            MsgBox("Please select a document", vbInformation + vbOKOnly, Me.Text)
            btnBrowse.Focus()
            Exit Function
        End If
        If Mode = "A" Then
            strDocID = clsSVT.getDataLongValue("Select DocID from UtlSharedDocs " _
                        & " where DocFileName ='" & Trim(txtDoc.Text) & "'", 0, blnfound)
            If strDocID <> txtDocID.Text Then
                Call clsSVT.getDataStringValue("Select DocFileName from UtlSharedDocs " _
                        & " where DocFileName ='" & Trim(txtDoc.Text) & "'", "", blnfound)
                If blnfound Then
                    MsgBox("Document already exists!", vbInformation + vbOKOnly, Me.Text)
                    btnBrowse.Focus()
                    txtDoc.Text = ""
                    Exit Function
                End If
            End If
        End If

        ValidData = True

    End Function

    Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click
        Dim blnFound As Boolean
        Dim strSql As String

        If Not ValidData() Then Exit Sub
        'Call clsSVT.getDataStringValue("Select CustName from MstCust " _
        '           & " where CustName ='" & Trim(txtCustName.Text) & "'", "", blnFound)

        If Mode = "M" Then
            Call clsSVT.getDataLongValue("Select DocID from UtlSharedDocs " _
                        & " where DocFileName ='" & Trim(txtDoc.Text) & "'", 0, blnFound)
            If blnFound Then
                strSql = "Update UtlSharedDocs Set "
                strSql = strSql & "GroupName = '" & cboGroup.Text & "',"
                'strSql = strSql & "DocFileName = '" & Trim(txtDoc.Text) & "',"
                strSql = strSql & "Remarks = '" & Trim(txtRemarks.Text) & "'"
                strSql = strSql & " where DocID = " & Trim(txtDocID.Text)
                Call clsSVT.execSQLStatement(strSql)
                'Call FileObj.CopyFile(strFileName, gstrRptPath & "\SharedDocs\")
                fillVsfDet()
                Exit Sub
            End If


        End If

        strSql = "Insert into UtlSharedDocs (GroupName, DocFileName, Remarks) values ("
        strSql = strSql & "'" & Trim(cboGroup.Text) & "',"
        strSql = strSql & "'" & Trim(txtDoc.Text) & "',"
        strSql = strSql & "'" & Trim(txtRemarks.Text) & "')"

        Call clsSVT.execSQLStatement(strSql)
        Call File.Copy(strFileName, gstrRptPath & "SharedDocs\" & Path.GetFileName(strFileName))
        fillVsfDet()
    End Sub

    Private Sub fillVsfDet()
        Dim RsTemp As oledb.oledbDataReader
        Dim strSQL As String
        Dim fName As String
        Dim dfname As String
        Dim strFilePath As String
        Dim fext As String
        Dim i As Integer

        cboGroup.Text = ""
        txtDoc.Text = ""
        txtRemarks.Text = ""
        pnlAddSharedDocs.Visible = False

        strSQL = " select * from UtlSharedDocs "
        RsTemp = clsSVT.getDataReaderFromSQL(strSQL)

        vsfDet.Rows.Count = 1
        With RsTemp
            Do While .Read()
                dfname = GetVal(RsTemp("DocFileName"))
                fext = GetFileExtType(Path.GetExtension(gstrRptPath & "\SharedDocs\" & dfname))
                strFilePath = gstrRptPath & "\SharedDocs\" & dfname
                'PictureBox1.Image = Icon.ExtractAssociatedIcon(strFilepath).ToBitmap
                With vsfDet
                    .Rows.Count += 1
                    i = .Rows.Count - 1

                    .SetData(i, vsfDetGroup, GetVal(RsTemp("GroupName"), ""))
                    Try
                        .SetCellImage(i, vsfDetIcon, Icon.ExtractAssociatedIcon(strFilePath).ToBitmap)
                    Catch ex As Exception

                    End Try

                    .SetData(i, vsfDetFileType, fext)
                    .SetData(i, vsfDetFileName, GetVal(RsTemp("DocFileName"), ""))
                    .SetData(i, vsfDetRemarks, GetVal(RsTemp("Remarks"), ""))

                End With
            Loop
            .Close()
        End With
        With vsfDet
            If .Rows.Count > 1 Then
                .Row = 1
                lblfname.Visible = True
                lblfsize.Visible = True
                lblmod.Visible = True
                fName = gstrRptPath & "\SharedDocs\" & .GetData(.Row, vsfDetFileName)
                If File.Exists(fName) Then
                    Dim infoReader As System.IO.FileInfo
                    infoReader = My.Computer.FileSystem.GetFileInfo(fName)

                    lblfname.Text = "File Name: " & Path.GetFileNameWithoutExtension(fName) & "." & Path.GetExtension(fName)
                    lblfsize.Text = "File Size: " & infoReader.Length
                    lblmod.Text = "Last Modified: " & File.GetLastAccessTime(fName)
                Else
                    lblfname.Text = "File Name: Physical File not found"
                End If
            Else
                .Row = 0
                lblfname.Visible = False
                lblfsize.Visible = False
                lblmod.Visible = False
            End If
        End With
        vsfDet.AutoSizeRows()

        vsfDet.AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.RestrictCols
        Dim j%
        For j = vsfDet.Cols.Fixed To vsfDet.Cols.Count - 1
            vsfDet.Cols(vsfDetGroup).AllowMerging = True
        Next

    End Sub

    Private Sub vsfDet_DoubleClick(sender As Object, e As System.EventArgs) Handles vsfDet.DoubleClick
        Dim strFile As String

        If vsfDet.Col <> vsfDetFileName Then Exit Sub

        If vsfDet.Row > 0 Then
            strFile = gstrRptPath & "\SharedDocs\" & vsfDet.GetData(vsfDet.Row, vsfDetFileName)
            If Dir(strFile) = "" Then
                MsgBox("File not found!", vbCritical, Me.Text)
                Exit Sub
            End If
            Dim p As New System.Diagnostics.Process
            Dim s As New System.Diagnostics.ProcessStartInfo(strFile)
            s.UseShellExecute = True
            s.WindowStyle = ProcessWindowStyle.Normal
            p.StartInfo = s
            p.Start()
        End If
    End Sub

    Private Sub vsfDet_RowColChange(ByVal sender As Object, ByVal e As System.EventArgs) Handles vsfDet.RowColChange
        'Dim FileObj As New FileSystemObject
        'Dim fname As String



        'With vsfDet
        '    If .Rows.Count > 1 And .Row > 0 Then
        '        lblfname.Visible = True
        '        lblfsize.Visible = True
        '        lblmod.Visible = True
        '        fname = gstrRptPath & "\SharedDocs\" & .GetData(.Row, vsfDetFileName)
        '        lblfname.Text = "File Name: " & FileObj.GetBaseName(fname) & "." & FileObj.GetExtensionName(fname)
        '        lblfsize.Text = "File Size: " & BytesToString(FileObj.GetFile(fname).Size)
        '        lblmod.Text = "Last Modified: " & FileObj.GetFile(fname).DateLastModified
        '    Else
        '        lblfname.Visible = False
        '        lblfsize.Visible = False
        '        lblmod.Visible = False
        '    End If
        'End With
        'Exit Sub
    End Sub

    Private Function BytesToString(ByVal byteCount As Long)
        Dim bytes As Long
        Dim place As Integer
        Dim num As Double
        Dim suf() As String = {" B", " KB", " MB", " GB", " TB", " PB", " EB"}

        If byteCount = 0 Then
            Return "0" & suf(0)
        End If

        bytes = Math.Abs(byteCount)
        place = Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)))
        num = Math.Round(bytes / Math.Pow(1024, place), 1)
        Return (Math.Sign(byteCount) * num).ToString & suf(place)
    End Function

    Private Sub btnRemove_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRemove.Click
        With vsfDet
            If .Row > 0 Then
                If MsgBox("Are you sure you want to delete the document '" &
                          vsfDet.GetData(vsfDet.Row, vsfDetFileName),
                          vbYesNo + vbCritical, Me.Text) = vbYes Then
                    DeleteDoc()
                    fillVsfDet()
                End If
            Else
                MsgBox("No files found!", vbInformation + vbOKOnly, Me.Text)
                Exit Sub
            End If
        End With
    End Sub

    'Private Sub btnList_Click(sender As System.Object, e As System.EventArgs) Handles btnList.Click
    '    ExportToXL(vsfDet, "")
    'End Sub

    Private Sub btnEdit_Click(sender As System.Object, e As System.EventArgs) Handles btnEdit.Click

        Dim blnfound As Boolean

        With vsfDet
            If .Row > 0 Then
                pnlAddSharedDocs.Visible = True
                btnBrowse.Visible = False
                Call bindCombo(cboGroup, "GroupName", "GroupName", "select distinct GroupName from UtlSharedDocs order by GroupName")
                cboGroup.Focus()
                Mode = "M"
                cboGroup.Text = .GetData(.Row, vsfDetGroup)
                txtDoc.Text = .GetData(.Row, vsfDetFileName)
                txtRemarks.Text = .GetData(.Row, vsfDetRemarks)
                txtDocID.Text = clsSVT.getDataLongValue("Select DocID from UtlSharedDocs " _
                        & " where DocFileName ='" & Trim(txtDoc.Text) & "'", 0, blnfound)
                '.Text = clsGen.GetDataValue("DocID", "", "utlSharedDocs", blnFlag, _
                '            " WHERE DocFileName = '" & Trim(txtDocument.Text) & "'")
                'imgFile.Picture = LoadPicture(App.Path & "\Images\Img" & Right(.TextMatrix(.Row, VFileName), 3) & ".jpg")
            Else
                MsgBox("You have not selected any File for Editing!!" & vbCrLf & vbCrLf, _
                        vbInformation + vbOKOnly, Me.Text)
                Exit Sub
            End If
        End With
        'pnlAddSharedDocs.Enabled = False
        ''fraShrdAdd.Visible = True
        ''fraShrdAdd.Enabled = True
        'cboGroup.Focus()


    End Sub

    Private Sub vsfDet_KeyDown(sender As Object, e As KeyEventArgs) Handles vsfDet.KeyDown
        If vsfDet.Row = vsfDet.Rows.Count - 1 AndAlso vsfDet.Col = vsfDet.Cols.Count - 1 AndAlso e.KeyCode = Keys.Tab Then
            btnAdd.Focus()
        End If
    End Sub

    Private Sub btnClose_Leave(sender As Object, e As EventArgs) Handles btnClose.Leave
        With vsfDet
            .Row = 1
            .Col = 0
            Call showWaitCursor()
        End With
    End Sub

    Private Sub btnCancel_Leave(sender As Object, e As EventArgs) Handles btnCancel.Leave
        cboGroup.Focus()
    End Sub

    Private Sub btnExport_Click(sender As Object, e As EventArgs) Handles btnExport.Click
        ExportToXL(vsfDet, "")
    End Sub

    Private Sub frmUtlSharedDocuments_KeyUp(sender As Object, e As KeyEventArgs) Handles Me.KeyUp
        If e.Alt = True AndAlso e.KeyCode = Keys.X Then
            ExportToXL(vsfDet, "")
        End If
    End Sub
End Class