Imports System.IO
Imports C1.Win.C1FlexGrid

Public Class frmUtlRestore
    Dim clsSVt As New svtLibrary

    Const vASSlno = 0
    Const vASDel = 1
    Const vASDate = 2
    Const vAStime = 3
    Const vASFileName = 4
    Const vAStot = 5

    Public strDocument As String
    Public blnDetSection As Boolean
    Public frm As Object

    Private Sub frmUtlDelCancel_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        initScreen()
        LoadFileList()
        btnDel.Visible = False
    End Sub

    Private Sub initScreen()
        With vsfAS
            .Rows.Count = 1
            .Cols.Count = vAStot

            .SetData(0, vASSlno, "Slno")
            .SetData(0, vASDel, "Sel")
            .SetData(0, vASDate, "Date")
            .SetData(0, vAStime, "Time")
            .SetData(0, vASFileName, "File Name")

            .Cols(vASDel).DataType = GetType(Boolean)

            If chkRemove.Checked = True Then
                Call SetHideCol(vsfAS, False, vASDel)
            Else
                Call SetHideCol(vsfAS, True, vASDel)
            End If
        End With
    End Sub

    Private Sub LoadFileList()
        With vsfAS
            Dim i As Int32 = 1
            Try
                .Rows.Count = 1
                For Each strFilePath As String In My.Computer.FileSystem.GetFiles(gstrAutoSave & strDocument,
                                    Microsoft.VisualBasic.FileIO.SearchOption.SearchAllSubDirectories, "*.xml")
                    If blnDetSection Then
                        If InStr(strFilePath, "vsf") <= 0 Then
                            .Rows.Count = .Rows.Count + 1
                            .SetData(i, vASSlno, i)
                            .SetData(i, vASDel, False)
                            .SetData(i, vASDate, File.GetCreationTime(strFilePath))
                            .SetData(i, vAStime, File.GetLastWriteTime(strFilePath))
                            .SetData(i, vASFileName, Path.GetFileName(strFilePath))
                            i = i + 1
                        End If
                    Else
                        .Rows.Count = .Rows.Count + 1
                        .SetData(i, vASSlno, i)
                        .SetData(i, vASDel, False)
                        .SetData(i, vASDate, File.GetCreationTime(strFilePath))
                        .SetData(i, vAStime, File.GetLastWriteTime(strFilePath))
                        .SetData(i, vASFileName, Path.GetFileName(strFilePath))
                        i = i + 1
                    End If
                Next
            Catch ex As Exception
                MsgBox(ex.Message)
            End Try
            .AutoSizeCols()
        End With
    End Sub

    Private Sub btnDel_Click(sender As System.Object, e As System.EventArgs) Handles btnDel.Click
        With vsfAS
            If .Rows.Count <= 1 Then Exit Sub
            Try
                Dim strFileName As String = ""
                For i As Int32 = 1 To .Rows.Count - 1
                    If .GetCellCheck(i, vASDel) = CheckEnum.Checked Then
                        strFileName = .GetData(i, vASFileName)
                        RemoveFile(strFileName)
                    End If
                Next
                MsgBox("Successfully Removed", MsgBoxStyle.Information, Me.Text)
                LoadFileList()
                chkRemove.Visible = True
                chkRemove.Checked = False
            Catch ex As Exception

            End Try
        End With
    End Sub

    Private Sub RemoveFile(ByVal argFileName As String)
        Try
            Dim strTempFileName As String = argFileName

            If blnDetSection AndAlso File.Exists(strDocument & argFileName) Then
                File.Delete(strDocument & argFileName)
            End If
            strTempFileName = strTempFileName.Replace(".xml", "")
            Dim argDsgrGrids = GetAllDsgrVsf(frm, GetType(C1FlexGrid))
            For Each argGrid As C1FlexGrid In argDsgrGrids
                If argGrid.Visible Then
                    If File.Exists(strDocument & strTempFileName & "_" & CStr(argGrid.Name) & ".xml") Then
                        File.Delete(strDocument & strTempFileName & "_" & CStr(argGrid.Name) & ".xml")
                    End If
                End If
            Next
        Catch ex As Exception

        End Try
    End Sub

    Private Sub vsfAS_BeforeEdit(sender As Object, e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfAS.BeforeEdit
        If e.Col <> vASDel Then
            e.Cancel = True
        End If
    End Sub

    Private Sub vsfAS_MouseDoubleClick(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles vsfAS.MouseDoubleClick
        With vsfAS
            If .Rows.Count <= 1 Then Exit Sub
            If GetVal(.GetData(.Row, vASFileName), "") = "" Then
                MsgBox("File Name is Empty", MsgBoxStyle.Exclamation, Me.Text)
                Exit Sub
            End If
            frm.strRestoreFileName = CStr(.GetData(.Row, vASFileName)).Replace(".xml", "")
            Me.Close()
        End With
    End Sub

    Private Sub chkRemove_CheckedChanged(sender As Object, e As EventArgs) Handles chkRemove.CheckedChanged
        If chkRemove.Checked = True Then
            btnDel.Visible = True
            chkSelectAll.Visible = True
            Call SetHideCol(vsfAS, False, vASDel)
            chkRemove.Visible = False
        Else
            btnDel.Visible = False
            chkSelectAll.Visible = False
            Call SetHideCol(vsfAS, True, vASDel)
        End If


    End Sub

    Private Sub chkSelectAll_CheckedChanged(sender As Object, e As EventArgs) Handles chkSelectAll.CheckedChanged
        If chkSelectAll.Checked = True Then

            With vsfAS
                For i As Int32 = 0 To .Rows.Count - 1
                    .SetData(i, vASDel, True)
                Next
            End With
        Else

            With vsfAS
                For i As Int32 = 0 To .Rows.Count - 1
                    .SetData(i, vASDel, False)
                Next
            End With
        End If
    End Sub

    Private Sub Label1_Click(sender As Object, e As EventArgs) Handles Label1.Click

    End Sub
End Class