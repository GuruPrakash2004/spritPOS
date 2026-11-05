Imports System.IO
Imports C1.Win.C1FlexGrid
Imports System.Collections.Specialized

Public Class svtFileCtrl
    Const vDetImgPrint = 0
    Const vDetDispFileName = 1
    Const vDetActFileName = 2
    Const vDetFileCreated = 3

    Public argFilePath As String
    Public argDocID As Integer
    Public argOwnerCaption As String

    Private mShowPrintSelection As Boolean

    Private NewViewMode As Boolean
    Dim clsSVT As New svtLibrary

    Public Property ViewMode() As Boolean
        Get
            Return NewViewMode
        End Get
        Set(ByVal value As Boolean)
            NewViewMode = value
            If value Then
                btnAddImg.Enabled = False
                btnRemoveImg.Enabled = False
            Else
                btnAddImg.Enabled = True
                btnRemoveImg.Enabled = True
            End If
        End Set
    End Property

    Public Property ShowPrintSelection() As Boolean
        Get
            Return mShowPrintSelection
        End Get
        Set(ByVal value As Boolean)
            mShowPrintSelection = value
            vsfDet.Cols(vDetImgPrint).Visible = value
        End Set
    End Property

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        img.Image = Nothing
        lblPreview.Text = "Preview Pane"
        lblPreview.Visible = True

        With vsfDet
            .Cols.Count = 4
            .Rows.Count = 1
            .Cols(vDetImgPrint).Width = 30
            .SetData(0, vDetImgPrint, "Print")
            .SetData(0, vDetDispFileName, "Files Attached")
            .Cols(vDetActFileName).Visible = False
            .Cols(vDetFileCreated).Visible = False
            .Cols(vDetImgPrint).DataType = GetType(Boolean)
        End With

    End Sub

    Public Sub ClearFiles()
        lblPreview.Text = "Preview Pane"
        lblPreview.Visible = True

        vsfDet.Rows.Count = 1
    End Sub

    Private Sub btnAddImg_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddImg.Click
        If viewMode Then Exit Sub
        Dim strDispFileName As String
        Try
            If ofd.ShowDialog() = DialogResult.OK Then
                strDispFileName = Path.GetFileName(ofd.FileName)
                'strFileName = Date.Now.Ticks & "_" & strDispFileName
                vsfDet.Rows.Add()
                vsfDet.SetData(vsfDet.Rows.Count - 1, vDetActFileName, ofd.FileName)
                vsfDet.SetData(vsfDet.Rows.Count - 1, vDetDispFileName, strDispFileName)
                vsfDet.SetData(vsfDet.Rows.Count - 1, vDetFileCreated, "N")
                vsfDet.Row = vsfDet.Rows.Count - 1
                Call showImagePreview()
            End If

        Catch ex As Exception
            MsgBox("Cannot consider this as image.." & vbCrLf & ex.Message, MsgBoxStyle.Critical, "Image Processing error")
        End Try
    End Sub

    Private Sub btnRemoveImg_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRemoveImg.Click
        With vsfDet
            If .Rows.Count > 1 Then
                If .Rows(.Row).Visible Then
                    If .Row <> 0 Then
                        If MsgBox("Are you Sure, You want to Delete? " & vbCr & .GetData(.Row, vDetDispFileName), _
                            vbCritical + vbYesNo + vbDefaultButton2, argOwnerCaption) = vbYes Then
                            .Rows(.Row).Visible = False
                            img.Image = Nothing
                            Call MoveToNextRow()
                            Call showImagePreview()
                        End If
                    End If
                End If
            End If
        End With
    End Sub

    Private Sub MoveToNextRow()
        Dim i As Integer
        On Error Resume Next
        With vsfDet
            'Down
            For i = .Row To .Rows.Count - 1
                If .Rows(i).Visible Then
                    .Row = i
                    .RowSel = .Row
                    .Focus()
                    Exit Sub
                End If
            Next

            'Up
            For i = .Row - 1 To 1 Step -1
                If .Rows(i).Visible Then
                    .Row = i
                    .RowSel = .Row
                    .Focus()
                    Exit Sub
                End If
            Next
        End With
    End Sub

    Private Sub vsfDet_AfterEdit(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfDet.AfterEdit
        Dim i As Integer
        With vsfDet
            If .GetCellCheck(e.Row, vDetImgPrint) = C1.Win.C1FlexGrid.CheckEnum.Checked Then
                For i = 1 To .Rows.Count - 1
                    If i <> e.Row Then
                        .SetCellCheck(i, vDetImgPrint, C1.Win.C1FlexGrid.CheckEnum.Unchecked)
                    End If
                Next
            End If
        End With
    End Sub

    Private Sub vsfDet_BeforeEdit(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfDet.BeforeEdit
        If e.Col <> vDetImgPrint Then e.Cancel = True
    End Sub

    Private Sub vsfDet_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles vsfDet.DoubleClick

        Dim strFile As String
        If vsfDet.Row <= 0 Then Exit Sub
        strFile = vsfDet.GetData(vsfDet.Row, vDetActFileName)
        '  MsgBox(strFile.Substring(2, 1))
        If strFile.Substring(1, 1) = ":" Then
            MessageBox.Show("Please save and then Open the file...!", argOwnerCaption, MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If
        strFile = gstrRptPath & "TrnDocFiles\" & strFile
        Try
            Dim p As New System.Diagnostics.Process
            Dim s As New System.Diagnostics.ProcessStartInfo(strFile)
            s.UseShellExecute = True
            s.WindowStyle = ProcessWindowStyle.Normal
            p.StartInfo = s
            p.Start()
        Catch ex As Exception
            MessageBox.Show("File '" & strFile & "' could not be found!", argOwnerCaption, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub vsfDet_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles vsfDet.KeyDown
        If ViewMode Then Exit Sub
        Dim strDispFileName As String
        Try
            If e.Modifiers = Keys.Control AndAlso e.KeyCode = Keys.V Then
                If Clipboard.ContainsFileDropList() Then
                    Dim file_names As StringCollection = Clipboard.GetFileDropList()
                    For Each file_name As String In file_names
                        For Each strExtname As String In file_name.Split(".")
                            Select Case strExtname.ToUpper()
                                Case "DOC", "DOCX", "PDF", "XLS", "XLSX", "TXT", "RTF", "JPG", "PNG", "JPEG", "GIF", "ZIP", "RAR"
                                    strDispFileName = file_name
                                    vsfDet.Rows.Add()
                                    vsfDet.SetData(vsfDet.Rows.Count - 1, vDetActFileName, strDispFileName)
                                    vsfDet.SetData(vsfDet.Rows.Count - 1, vDetDispFileName, strDispFileName)
                                    vsfDet.SetData(vsfDet.Rows.Count - 1, vDetFileCreated, "N")
                                    vsfDet.Row = vsfDet.Rows.Count - 1
                            End Select
                        Next
                    Next file_name
                End If
                Call showImagePreview()
            End If
        Catch ex As Exception
            MsgBox("Cannot consider this as image.." & vbCrLf & ex.Message, MsgBoxStyle.Critical, "Image Processing error")
        End Try
    End Sub

    Private Sub vsfDet_RowColChange(ByVal sender As Object, ByVal e As System.EventArgs) Handles vsfDet.RowColChange
        Call showImagePreview()
    End Sub

    Private Sub showImagePreview()
        lblPreview.Text = "Preview Pane"
        img.Image = Nothing
        Dim fext As String = ""
        If vsfDet.Row <= 0 Then Exit Sub
        If vsfDet.Rows(vsfDet.Row).Visible = False Then
            Exit Sub
        End If

        'Dim Extension As String = Path.GetExtension(imagePath)

        'If StrComp(Extension, ".jpg", CompareMethod.Text) = 0 Or _
        '   StrComp(Extension, ".jpeg", CompareMethod.Text) = 0 Then

        'End If
        'dotPosition = filePath.LastIndexOf(".")
        'filePath.Substring(dotPosition + 1)

        'Dim imgThumb As Drawing.Image = imgImage.GetThumbnailImage(64, 64, Nothing, IntPtr.Zero)
        'pbPicture.Image = imgThumb

        'With AxAcroPDF1
        '    .LoadFile("D:\t\p.pdf")
        '    .gotoFirstPage()
        '    .setShowToolbar(False)
        '    .setPageMode("none")
        '    .setView("FitBH")
        '    .setShowScrollbars(True)
        'End With

        Try
            If File.Exists(vsfDet.GetData(vsfDet.Row, vDetActFileName)) Then
                'fext = IO.Path.GetExtension(vsfDet.GetData(vsfDet.Row, vDetActFileName))

                'If UCase(fext) = ".PDF" Then
                '    img.Visible = False
                '    AxAcroPDF1.Visible = True
                '    With AxAcroPDF1
                '        .LoadFile(vsfDet.GetData(vsfDet.Row, vDetActFileName))
                '        .gotoFirstPage()
                '        .setShowToolbar(False)
                '        .setPageMode("none")
                '        .setView("FitBH")
                '        .setShowScrollbars(True)
                '    End With
                'Else
                img.Image = Image.FromFile(vsfDet.GetData(vsfDet.Row, vDetActFileName))
                lblPreview.Visible = False
                'End If
            Else
                'fext = IO.Path.GetExtension(gstrRptPath & "TrnDocFiles\" & vsfDet.GetData(vsfDet.Row, vDetActFileName))
                'If UCase(fext) = ".PDF" Then
                '    'img.Visible = False
                '    With AxAcroPDF1
                '        .BringToFront()
                '        '.LoadFile(gstrRptPath & "TrnDocFiles\" & vsfDet.GetData(vsfDet.Row, vDetActFileName))
                '        .LoadFile("\\IBMSSPLSRVR\ClientData\SqlDBRpt\UPSMfgSys\RPTS\TrnDocFiles\p.pdf")
                '        .gotoFirstPage()
                '        .setShowToolbar(False)
                '        .setPageMode("none")
                '        .setView("FitBH")
                '        .setShowScrollbars(True)
                '    End With
                '    AxAcroPDF1.Visible = True
                'Else
                'AxAcroPDF1.Visible = False
                Try
                    img.Image = Image.FromFile(gstrRptPath & "TrnDocFiles\" & vsfDet.GetData(vsfDet.Row, vDetActFileName))
                Catch
                End Try
                img.Visible = True
                lblPreview.Visible = False
            End If
            'fext = GetFileExtType(gstrRptPath & "TrnDocFiles\" & vsfDet.GetData(vsfDet.Row, vDetActFileName))
        Catch ex As Exception
            lblPreview.Text = "No Preview Available"
            lblPreview.Visible = True
        End Try
    End Sub

    Public Sub saveAttachments(ByVal argDocID As Integer, ByVal argDocValueID As String, ByVal argBlnNumeric As Boolean, Optional ByVal sTrans As oledb.oledbTransaction = Nothing)
        Dim i As Integer, strSQL As String, intslno As Long
        Dim strFileName As String, blnSaveFile As String

        strSQL = "Delete from UtlAttachFiles where DocID = " & argDocID
        If argBlnNumeric Then
            strSQL = strSQL & " and DocValueID = '" & argDocValueID & "'"
        Else
            strSQL = strSQL & " and DocValueID = " & argDocValueID
        End If
        clsSVT.execSQLStatement(strSQL, sTrans)

        With vsfDet
            For i = 1 To .Rows.Count - 1
                If .Rows(i).Visible Then
                    blnSaveFile = True
                    vsfDet.SetData(i, vDetDispFileName, Replace(Path.GetFileName(vsfDet.GetData(i, vDetActFileName)), "'", "`"))
                    'strSQL = vsfDet.GetData(i, vDetActFileName)
                    'strSQL = Path.GetDirectoryName(vsfDet.GetData(i, vDetDispFileName))
                    If File.Exists(vsfDet.GetData(i, vDetActFileName)) AndAlso Path.GetDirectoryName(vsfDet.GetData(i, vDetActFileName)) = gstrRptPath & "TrnDocFiles" Then
                        blnSaveFile = False
                    End If
                    If File.Exists(gstrRptPath & "TrnDocFiles\" & Replace(vsfDet.GetData(i, vDetActFileName), "'", "`")) Then
                        blnSaveFile = False
                    End If
                    If blnSaveFile Then
                        strFileName = Date.Now.Ticks & "_" & Replace(Path.GetFileName(.GetData(i, vDetActFileName)), "'", "`")
                        File.Copy(.GetData(i, vDetActFileName), gstrRptPath & "TrnDocFiles\" & strFileName)
                        .SetData(i, vDetActFileName, strFileName)
                    Else
                        .SetData(i, vDetDispFileName, Replace(Path.GetFileName(.GetData(i, vDetActFileName)), "'", "`"))
                    End If

                    'If .GetData(i, vDetFileCreated) = "N" Then
                    '    If Path.GetDirectoryName(vsfDet.GetData(i, vDetActFileName)) = gstrRptPath & "Images" Then
                    '        strFileName = Path.GetFileName(.GetData(i, vDetActFileName))
                    '        File.Copy(.GetData(i, vDetActFileName), strFileName)
                    '        .SetData(i, vDetActFileName, strFileName)
                    '    Else
                    '        strFileName = Date.Now.Ticks & "_" & Path.GetFileName(.GetData(i, vDetActFileName))
                    '        File.Copy(.GetData(i, vDetActFileName), strFileName)
                    '        .SetData(i, vDetActFileName, strFileName)
                    '    End If
                    'End If

                    strSQL = "Insert into UtlAttachFiles (DocID, DocValueID, AFileName, DocSlNo) values ("
                    strSQL = strSQL & argDocID & ","
                    strSQL = strSQL & "'" & argDocValueID & "',"
                    strSQL = strSQL & "'" & Replace(Path.GetFileName(.GetData(i, vDetActFileName)), "'", "`") & "',"

                    intslno = clsSVT.getDataLongValue("Select DocSlNo from UtlAttachFiles where DocValueID = '" & argDocValueID & "' order by DocSlNo DESC", 0, False, sTrans)
                    If intslno = 0 Then
                        strSQL = strSQL & 1 & ")"
                    Else
                        strSQL = strSQL & Val(clsSVT.getDataLongValue("Select DocSlNo from UtlAttachFiles where DocValueID = '" & argDocValueID & "'order by DocSlNo DESC", 0, False, sTrans)) + 1 & ")"

                    End If
                    clsSVT.execSQLStatement(strSQL, sTrans)
                Else
                    If .GetData(i, vDetFileCreated) = "Y" Then
                        If File.Exists(.GetData(i, vDetActFileName)) Then
                            Try
                                File.Delete(.GetData(i, vDetActFileName))
                            Catch ex As Exception

                            End Try

                        End If
                    End If
                End If
            Next
        End With
    End Sub

    Public Sub LoadFilesFromData(ByVal argSQL As String)
        Dim rs As oledb.oledbDataReader
        rs = clsSVT.getDataReaderFromSQL(argSQL)
        Do While rs.Read
            vsfDet.Rows.Add()
            vsfDet.SetData(vsfDet.Rows.Count - 1, vDetActFileName, rs("AFileName"))
            vsfDet.SetData(vsfDet.Rows.Count - 1, vDetDispFileName, rs("AFileName")) 'Mid(rs("AFileName"), 21))
            vsfDet.SetData(vsfDet.Rows.Count - 1, vDetFileCreated, "Y")
            vsfDet.Row = 1
            Call showImagePreview()
        Loop

        rs.Close()
    End Sub

    Private Sub srcWebCam_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles srcWebCam.Click
        'strWebCamFile = ""
        'frmUtlWebCam.ShowDialog()
        'vsfDet.Rows.Add()
        'vsfDet.SetData(vsfDet.Rows.Count - 1, vDetActFileName, strWebCamFile)
        'vsfDet.SetData(vsfDet.Rows.Count - 1, vDetDispFileName, strWebCamFile)
        'vsfDet.SetData(vsfDet.Rows.Count - 1, vDetFileCreated, "N")
        'vsfDet.Row = vsfDet.Rows.Count - 1
        'Call showImagePreview()
    End Sub

    Private Sub svtFileCtrl_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub
End Class
