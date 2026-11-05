Imports System.IO
Imports System.Data.OleDb

Public Class frmDownloadExe

    Dim clsSVT As New svtLibrary
    Dim strExename As String = My.Application.Info.AssemblyName & ".exe"


    Private Sub frmDownloadExe_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim str As String = ""
        If gstrServerExePath <> "" Then
            str = gstrServerExePath.Replace(strExename, "")
            str = Mid(str, 1, Len(str) - 1)
        End If
        txtLocation.Text = str
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Try
            If Trim(txtLocation.Text) = "" Then
                MsgBox("Please Select Location....", MsgBoxStyle.Exclamation, Me.Text)
                Exit Sub
            End If

            If Not IO.Directory.Exists(txtLocation.Text) Then
                MsgBox("Directory Not Exists....", MsgBoxStyle.Exclamation, Me.Text)
                Exit Sub
            End If

            Dim strCustCode As String = clsSVT.getDataStringValue("Select Top 1 CustCode From MstComp", "", False)
            If strCustCode = "" Then
                Call showNormalCursor()
                MsgBox("Customer Code is blank in Company master...", MsgBoxStyle.Exclamation, Me.Text)
                Exit Sub
            End If
            Call showWaitCursor()
            Dim strSQL As String = ""

            Dim strConn As String, blnfound As Boolean = False
            strConn = "Provider=SQLOLEDB.1;Persist Security Info=False"
            strConn = strConn & ";User ID=sa"
            strConn = strConn & ";Pwd=svt@123$$"
            strConn = strConn & ";Initial Catalog=ClientEXE"
            strConn = strConn & ";Data Source=47.23.106.203"

            Dim svtTempConn As New OleDbConnection

            Using svtTempConn
                svtTempConn.ConnectionString = strConn
                svtTempConn.Open()

                clsSVT.getDataStringValue("Select CustCode From TblExe Where ExeFile is Not Null and Custcode='" & strCustCode & "'", "", blnfound, argBranchConn:=svtTempConn)
                If Not blnfound Then
                    Call showNormalCursor()
                    MsgBox("Exe not available in cloud server...", MsgBoxStyle.Exclamation, Me.Text)
                    Exit Sub
                End If

                Dim rs As OleDb.OleDbDataReader
                rs = clsSVT.getDataReaderFromSQL("Select ExeFile from TblExe Where CustCode = '" & strCustCode & "'", argBranchConn:=svtTempConn)
                rs.Read()
                Dim exebytes As Byte()
                exebytes = DirectCast(rs("ExeFile"), Byte())
                File.WriteAllBytes(txtLocation.Text & "\" & strExename, exebytes)
                rs.Close()
                'If File.Exists(txtLocation.Text & "\Bak" & strExename) Then
                '    File.Delete(txtLocation.Text & "\Bak" & strExename)
                'End If

                'File.Move(txtLocation.Text & "\" & strExename, txtLocation.Text & "\Bak" & strExename)
                'File.Copy(txtLocation.Text & "\Lat" & strExename, txtLocation.Text & "\" & strExename)
                MsgBox("Exe downloaded successfully!.", MsgBoxStyle.Information, Me.Text)
            End Using
            Call showNormalCursor()
        Catch ex As Exception
            Call showNormalCursor()
            MsgBox("Exe download failed." & ex.Message, MsgBoxStyle.Exclamation, Me.Text)
        End Try
    End Sub

    Private Sub btnOpen_Click(sender As Object, e As EventArgs) Handles btnOpen.Click
        fldBrowser.ShowDialog()
        txtLocation.Text = fldBrowser.SelectedPath
    End Sub
End Class