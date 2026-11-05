Imports System.IO
Imports System.Data.oledb

Public Class frmUpdateExe
    Dim clsSVT As New svtLibrary

    Private Sub frmUpdateExe_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Application.DoEvents()

    End Sub

    Public Sub chkExeVersion()
        If Not fnFldPresent("vLQMSFileVer", "UtlMisc") Then
            Call clsSVT.execSQLStatement("Alter table UtlMisc add vLQMSFileVer varchar(50) not null default ''")
        End If

        Try
            Dim webConn As New oledb.OleDbConnection

            webConn.ConnectionString = webConnString
            Try
                webConn.Open()
                Dim strLocDBexever = clsSVT.getDataStringValue("Select vLQMSFileVer from UtlMisc", "", False)
                Dim strCloudDBexever = clsSVT.getDataStringValue("Select vLQMSFileVer from MstVLQMS", "", False, Nothing, webConn)
                If strLocDBexever <> strCloudDBexever Or Not File.Exists(gstrRptPath & "vLQMS.exe") Then
                    Dim rsW As oledbDataReader
                    rsW = clsSVT.getDataReaderFromSQL("Select vLQMSAppFile from MstVLQMS", Nothing, webConn)
                    rsW.Read()
                    Dim excelContents() As Byte
                    excelContents = CType(rsW(0), Byte())
                    File.WriteAllBytes(gstrRptPath & "vLQMS.exe", excelContents)
                    clsSVT.execSQLStatement("Update UtlMisc set vLQMSFileVer = '" & strCloudDBexever & "'")
                End If
                webConn.Close()
            Catch ex As Exception

            End Try

            If File.Exists(gstrRptPath & "vLQMS.exe") Then
                'check for file dates from rptpath to local


                Dim strLocVersion As String = File.GetLastWriteTime(Application.StartupPath & "\vLQMS.exe").ToString
                Dim strRptVersion As String = File.GetLastWriteTime(gstrRptPath & "vLQMS.exe").ToString

                'MsgBox("Local version:" & strLocVersion & " RptVersion: " & strRptVersion)

                If strLocVersion <> strRptVersion Then
                    If File.Exists(Application.StartupPath & "\BakvLQMS.exe") Then
                        File.Delete(Application.StartupPath & "\BakvLQMS.exe")
                    End If
                    File.Move(Application.StartupPath & "\vLQMS.exe", Application.StartupPath & "\BakvLQMS.exe")
                    File.Copy(gstrRptPath & "vLQMS.exe", Application.StartupPath & "\vLQMS.exe")
                    MsgBox("Successfully updated latest EXE.... Application will Close. Re-Run again", MsgBoxStyle.Information, Me.Text)
                    End
                Else
                    MsgBox("You are already using Latest version.", MsgBoxStyle.Information, Me.Text)
                End If
            Else
                MsgBox("Latest version is not present in Rpts Folder " & gstrRptPath & vbCrLf & "Not able to update!", MsgBoxStyle.Critical, Me.Text)
            End If
        Catch ex As Exception

        End Try
        Me.Close()
    End Sub
End Class