Imports System
Imports System.Collections.Generic
Imports System.Text
Imports System.Management
Imports System.Security.Cryptography

Public Class getHWInfo
    ''' <summary>
    ''' Get volume serial number of drive C
    ''' </summary>
    ''' <returns></returns>
    Private Shared Function GetDiskVolumeSerialNumber() As String
        Try
            Dim _disk As ManagementObject = New ManagementObject("Win32_LogicalDisk.deviceid=""c:""")
            _disk.[Get]()
            Return _disk("VolumeSerialNumber").ToString()
        Catch
            Return String.Empty
        End Try
    End Function

    ''' <summary>
    ''' Get CPU ID
    ''' </summary>
    ''' <returns></returns>
    Private Shared Function GetProcessorId() As String
        Try
            Dim _mbs As ManagementObjectSearcher = New ManagementObjectSearcher("Select ProcessorId From Win32_processor")
            Dim _mbsList As ManagementObjectCollection = _mbs.[Get]()
            Dim _id = String.Empty
            For Each _mo As ManagementObject In _mbsList
                _id = _mo("ProcessorId").ToString()
                Exit For
            Next

            Return _id
        Catch
            Return String.Empty
        End Try

    End Function

    ''' <summary>
    ''' Get motherboard serial number
    ''' </summary>
    ''' <returns></returns>
    Private Shared Function GetMotherboardID() As String

        Try
            Dim _mbs As ManagementObjectSearcher = New ManagementObjectSearcher("Select SerialNumber From Win32_BaseBoard")
            Dim _mbsList As ManagementObjectCollection = _mbs.[Get]()
            Dim _id = String.Empty
            For Each _mo As ManagementObject In _mbsList
                _id = _mo("SerialNumber").ToString()
                Exit For
            Next

            Return _id
        Catch
            Return String.Empty
        End Try

    End Function

    Private Shared Iterator Function SplitInParts(input As String, partLength As Integer) As IEnumerable(Of String)
        If Equals(input, Nothing) Then Throw New ArgumentNullException("input")
        If partLength <= 0 Then Throw New ArgumentException("Part length has to be positive.", "partLength")

        Dim i = 0

        While i < input.Length
            Yield input.Substring(i, Math.Min(partLength, input.Length - i))
            i += partLength
        End While
    End Function

    ''' <summary>
    ''' Combine CPU ID, Disk C Volume Serial Number and Motherboard Serial Number as device Id
    ''' </summary>
    ''' <returns></returns>
    Public Shared Function GenerateUID(appName As String) As String
        'Combine the IDs and get bytes
        Dim _id As String = String.Concat(appName, GetProcessorId(), GetMotherboardID(), GetDiskVolumeSerialNumber())
        Dim _byteIds = Encoding.UTF8.GetBytes(_id)

        'Use MD5 to get the fixed length checksum of the ID string
        Dim _md5 As MD5CryptoServiceProvider = New MD5CryptoServiceProvider()
        Dim _checksum = _md5.ComputeHash(_byteIds)

        'Convert checksum into 4 ulong parts and use BASE36 to encode both
        Dim _part1Id As String = BASE36.Encode(BitConverter.ToUInt32(_checksum, 0))
        Dim _part2Id As String = BASE36.Encode(BitConverter.ToUInt32(_checksum, 4))
        Dim _part3Id As String = BASE36.Encode(BitConverter.ToUInt32(_checksum, 8))
        Dim _part4Id As String = BASE36.Encode(BitConverter.ToUInt32(_checksum, 12))

        'Concat these 4 part into one string
        Return String.Format("{0}-{1}-{2}-{3}", _part1Id, _part2Id, _part3Id, _part4Id)
    End Function

    Public Shared Function GetUIDInBytes(UID As String) As Byte()
        'Split 4 part Id into 4 ulong
        Dim _ids = UID.Split("-"c)

        If _ids.Length <> 4 Then Throw New ArgumentException("Wrong UID")

        'Combine 4 part Id into one byte array
        Dim _value = New Byte(15) {}
        Buffer.BlockCopy(BitConverter.GetBytes(BASE36.Decode(_ids(0))), 0, _value, 0, 8)
        Buffer.BlockCopy(BitConverter.GetBytes(BASE36.Decode(_ids(1))), 0, _value, 8, 8)
        Buffer.BlockCopy(BitConverter.GetBytes(BASE36.Decode(_ids(2))), 0, _value, 16, 8)
        Buffer.BlockCopy(BitConverter.GetBytes(BASE36.Decode(_ids(3))), 0, _value, 24, 8)

        Return _value
    End Function

    Public Shared Function ValidateUIDFormat(UID As String) As Boolean
        If Not StringExtensions.IsNullOrWhiteSpace(UID) Then
            Dim _ids = UID.Split("-"c)

            Return _ids.Length = 4
        Else
            Return False
        End If

    End Function
End Class

Public Module StringExtensions
    Public Function IsNullOrWhiteSpace(vale As String) As Boolean
        Return Equals(vale, Nothing) OrElse vale.All(AddressOf Char.IsWhiteSpace)
    End Function
End Module

Public Class Base36

    Private Const _charList As String = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ"
    Private Shared ReadOnly _charArray As Char() = _charList.ToCharArray()

    Public Shared Function Decode(input As String) As Long
        Dim _result As Long = 0
        Dim _pow As Double = 0
        For _i = input.Length - 1 To 0 Step -1
            Dim _c = input(_i)
            Dim pos = _charList.IndexOf(_c)
            If pos > -1 Then
                _result += pos * CLng(Math.Pow(_charList.Length, _pow))
            Else
                Return -1
            End If
            _pow += 1
        Next
        Return _result
    End Function

    Public Shared Function Encode(input As ULong) As String
        Dim _sb As StringBuilder = New StringBuilder()
        Do
            _sb.Append(_charArray(input Mod CULng(_charList.Length)))
            input /= CULng(_charList.Length)
        Loop While input <> 0

        Return Reverse(_sb.ToString())
    End Function

    Private Shared Function Reverse(s As String) As String
        Dim charArray = s.ToCharArray()
        Array.Reverse(charArray)
        Return New String(charArray)
    End Function
End Class