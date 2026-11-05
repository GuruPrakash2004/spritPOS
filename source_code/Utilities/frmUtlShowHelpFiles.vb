Imports System.IO
Imports C1.Win.C1FlexGrid


Public Class frmUtlShowHelpFiles

    Dim clsSVT As New svtLibrary
    Dim Mode As String

    Const vsfDetFileType = 0
    Const vsfDetFileName = 1

    Private Sub frmUtlSharedDocuments_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
    End Sub

    Private Sub btnClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub


    Private Sub vsfDet_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles vsfDet.DoubleClick
        Call showFile()
    End Sub
    Private Sub showFile()
        Dim strFile As String

        If vsfDet.Row > 0 Then
            strFile = gstrRptPath & "\Help\" & vsfDet.GetData(vsfDet.Row, vsfDetFileName)
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

    Public Sub LoadHelpFile(ByVal argfrmCaption As String, ByVal argfrmName As String)
        If Not Directory.Exists(gstrRptPath & "HelpFiles") Then
            Directory.CreateDirectory(gstrRptPath & "HelpFiles")
        End If
        With vsfDet
            .Cols.Count = 2
            .Rows.Count = 1
            .SetData(0, vsfDetFileType, "File Type")
            .SetData(0, vsfDetFileName, "File Name")
            .AllowEditing = False
            .ExtendLastCol = True
        End With
        Call initGridStyles(Me)

        Call SetColWidth(vsfDet, 150)

        Mode = "V"

        If Not Directory.Exists(gStrHelpFilePath) Then
            Directory.CreateDirectory(gStrHelpFilePath)
        End If
        Me.Text = "Help Files For - " & argfrmCaption
        Try
            Dim fileEntries As String() = Directory.GetFiles(gStrHelpFilePath)
            ' Process the list of files found in the directory. 
            Dim fileName As String
            vsfDet.Rows.Count = 1
            For Each fileName In fileEntries
                vsfDet.Rows.Count = vsfDet.Rows.Count + 1
                vsfDet.SetData(vsfDet.Rows.Count - 1, vsfDetFileName, Path.GetFileName(fileName))
                vsfDet.SetData(vsfDet.Rows.Count - 1, vsfDetFileType, GetFileExtType(Path.GetExtension(fileName)))
                'ProcessFile(fileName)
            Next fileName

        Catch ex As Exception
            MsgBox("File/Directory error", MsgBoxStyle.Critical, Me.Text)
            Exit Sub
        End Try

        If vsfDet.Rows.Count < 2 Then
            MsgBox("Could Not Find any help file with the name [" & _
                    argfrmName & "]" & vbCrLf & "Kindly create a file [Any type] with the same name in [" & gStrHelpFilePath & "]", vbInformation, argfrmCaption)
            Exit Sub
        End If
        If vsfDet.Rows.Count = 2 Then
            vsfDet.Row = vsfDet.Rows.Count - 1
            Call showFile()
        Else
            Me.Show()
        End If
        Call SetFlexColor(vsfDet, "", 0, 1)
    End Sub

    Private Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        Call showFile()
    End Sub
End Class