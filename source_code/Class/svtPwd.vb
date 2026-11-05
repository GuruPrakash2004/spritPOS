Imports System.IO
Imports System.Text
Imports System.Security.Cryptography

Public Class svtCrypto

    Private Shared DES As New TripleDESCryptoServiceProvider
    Private Shared MD5 As New MD5CryptoServiceProvider

    Public Shared Function MD5Hash(ByVal value As String) As Byte()
        Return MD5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(value))
    End Function

    Public Shared Function Encrypt(ByVal stringToEncrypt As String) As String
        Dim strPassKey As String = "svt$$12@#"
        DES.Key = svtCrypto.MD5Hash(strPassKey)
        DES.Mode = CipherMode.ECB
        Dim Buffer As Byte() = ASCIIEncoding.ASCII.GetBytes(stringToEncrypt)
        Return Convert.ToBase64String(DES.CreateEncryptor().TransformFinalBlock(Buffer, 0, Buffer.Length))
    End Function

    Public Shared Function Decrypt(ByVal encryptedString As String) As String
        Decrypt = ""
        If encryptedString = "" Then Exit Function
        Dim strPassKey As String = "svt$$12@#"
        Try
            DES.Key = svtCrypto.MD5Hash(strPassKey)
            DES.Mode = CipherMode.ECB
            Dim Buffer As Byte() = Convert.FromBase64String(encryptedString)
            Return ASCIIEncoding.ASCII.GetString(DES.CreateDecryptor().TransformFinalBlock(Buffer, 0, Buffer.Length))
        Catch ex As Exception
            MessageBox.Show("Invalid Key", "Decryption Failed", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End Try
    End Function
End Class


