Imports System.Runtime.InteropServices
Imports System

Public Class frmUtlWebCam
    Inherits System.Windows.Forms.Form

    Const WM_CAP As Short = &H400S

    Const WM_CAP_DRIVER_CONNECT As Integer = WM_CAP + 10
    Const WM_CAP_DRIVER_DISCONNECT As Integer = WM_CAP + 11
    Const WM_CAP_EDIT_COPY As Integer = WM_CAP + 30

    Const WM_CAP_SET_PREVIEW As Integer = WM_CAP + 50
    Const WM_CAP_SET_PREVIEWRATE As Integer = WM_CAP + 52
    Const WM_CAP_SET_SCALE As Integer = WM_CAP + 53
    Const WS_CHILD As Integer = &H40000000
    Const WS_VISIBLE As Integer = &H10000000
    Const SWP_NOMOVE As Short = &H2S
    Const SWP_NOSIZE As Short = 1
    Const SWP_NOZORDER As Short = &H4S
    Const HWND_BOTTOM As Short = 1

    Dim iDevice As Integer = 0 ' Current device ID
    Dim hHwnd As Integer ' Handle to preview window

    Declare Function SendMessage Lib "user32" Alias "SendMessageA" _
        (ByVal hwnd As Integer, ByVal wMsg As Integer, ByVal wParam As Integer, _
        <MarshalAs(UnmanagedType.AsAny)> ByVal lParam As Object) As Integer

    Declare Function SetWindowPos Lib "user32" Alias "SetWindowPos" (ByVal hwnd As Integer, _
        ByVal hWndInsertAfter As Integer, ByVal x As Integer, ByVal y As Integer, _
        ByVal cx As Integer, ByVal cy As Integer, ByVal wFlags As Integer) As Integer

    Declare Function DestroyWindow Lib "user32" (ByVal hndw As Integer) As Boolean

    Declare Function capCreateCaptureWindowA Lib "avicap32.dll" _
        (ByVal lpszWindowName As String, ByVal dwStyle As Integer, _
        ByVal x As Integer, ByVal y As Integer, ByVal nWidth As Integer, _
        ByVal nHeight As Short, ByVal hWndParent As Integer, _
        ByVal nID As Integer) As Integer

    Declare Function capGetDriverDescriptionA Lib "avicap32.dll" (ByVal wDriver As Short, _
        ByVal lpszName As String, ByVal cbName As Integer, ByVal lpszVer As String, _
        ByVal cbVer As Integer) As Boolean

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        picCapture.BackgroundImage = Nothing
        picCapture.Image = Nothing
        LoadDeviceList()
        If lstDevices.Items.Count > 0 Then
            btnStart.Enabled = True
            lstDevices.SelectedIndex = 0
            btnStart.Enabled = True
        Else
            lstDevices.Items.Add("No Capture Device")
            btnStart.Enabled = False
        End If

        btnStop.Enabled = False
        btnSave.Enabled = False
        picCapture.SizeMode = PictureBoxSizeMode.StretchImage

        initGridStyles(Me)
    End Sub

    Private Sub LoadDeviceList()
        Dim strName As String = Space(100)
        Dim strVer As String = Space(100)
        Dim bReturn As Boolean
        lstDevices.Items.Clear()
        Dim x As Integer = 0

        ' 
        ' Load name of all avialable devices into the lstDevices
        '

        Do
            '
            '   Get Driver name and version
            '
            bReturn = capGetDriverDescriptionA(x, strName, 100, strVer, 100)

            '
            ' If there was a device add device name to the list
            '
            If bReturn Then lstDevices.Items.Add(strName.Trim)
            x += 1
        Loop Until bReturn = False
    End Sub

    Private Sub OpenPreviewWindow()
        Dim iHeight As Integer = picCapture.Height
        Dim iWidth As Integer = picCapture.Width

        '
        ' Open Preview window in picturebox
        '
        hHwnd = capCreateCaptureWindowA(iDevice, WS_VISIBLE Or WS_CHILD, 0, 0, 640, _
            480, picCapture.Handle.ToInt32, 0)

        '
        ' Connect to device
        '
        If SendMessage(hHwnd, WM_CAP_DRIVER_CONNECT, iDevice, 0) Then
            '
            'Set the preview scale
            '
            SendMessage(hHwnd, WM_CAP_SET_SCALE, True, 0)

            '
            'Set the preview rate in milliseconds
            '
            SendMessage(hHwnd, WM_CAP_SET_PREVIEWRATE, 66, 0)

            '
            'Start previewing the image from the camera
            '
            SendMessage(hHwnd, WM_CAP_SET_PREVIEW, True, 0)

            '
            ' Resize window to fit in picturebox
            '
            SetWindowPos(hHwnd, HWND_BOTTOM, 0, 0, picCapture.Width, picCapture.Height, _
                    SWP_NOMOVE Or SWP_NOZORDER)

            btnSave.Enabled = True
            btnStop.Enabled = True
            btnStart.Enabled = False
        Else
            '
            ' Error connecting to device close window
            ' 
            DestroyWindow(hHwnd)

            btnSave.Enabled = False
        End If
    End Sub

    Private Sub btnStart_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnStart.Click
        iDevice = lstDevices.SelectedIndex
        OpenPreviewWindow()
    End Sub

    Private Sub ClosePreviewWindow()
        '
        ' Disconnect from device
        '
        SendMessage(hHwnd, WM_CAP_DRIVER_DISCONNECT, iDevice, 0)

        '
        ' close window
        '

        DestroyWindow(hHwnd)
    End Sub

    Private Sub btnStop_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnStop.Click
        ClosePreviewWindow()
        btnSave.Enabled = False
        btnStart.Enabled = True
        btnStop.Enabled = False
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim data As IDataObject
        Dim bmap As Image
        Dim strFileName As String

        '
        ' Copy image to clipboard
        '
        SendMessage(hHwnd, WM_CAP_EDIT_COPY, 0, 0)

        '
        ' Get image from clipboard and convert it to a bitmap
        '
        strFileName = ""
        Try
            data = Clipboard.GetDataObject()
            If data.GetDataPresent(GetType(System.Drawing.Bitmap)) Then
                bmap = CType(data.GetData(GetType(System.Drawing.Bitmap)), Image)
                picCapture.Image = bmap
                ClosePreviewWindow()
                btnSave.Enabled = False
                btnStop.Enabled = False
                btnStart.Enabled = True

                'If sfdImage.ShowDialog = DialogResult.OK Then
                strFileName = gstrRptPath & "TrnDocFiles\" & Date.Now.Ticks & ".jpg"
                bmap.Save(strFileName, Imaging.ImageFormat.Bmp)
                'End If
                strWebCamFile = strFileName
                ClosePreviewWindow()
                Me.Close()
                Me.Dispose()
            End If
        Catch ex As Exception
            MsgBox("Trying to save " & strFileName & vbCrLf & ex.Message, MsgBoxStyle.Critical, Me.Text)
        End Try
    End Sub

    Private Sub Form1_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing
        If btnStop.Enabled Then
            ClosePreviewWindow()
        End If
    End Sub
End Class