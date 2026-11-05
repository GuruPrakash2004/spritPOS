'///////////////////////////////////
'/     ComboEz - Version 1.01      /
'/ DropDown Combo w/Search button  /
'/         Edgemeal - 2014         /
'///////////////////////////////////

#Region "Notes and History"
' Notes:
' 1) Add ComboEz.vb and ComboEz.resx to a project as existing item.
' 2) Build.
' 3) Add control from toolbox to form.
'
' Clicking the search image or calling PerformClick of combo,
' will raise the combo's ClickSearch event.
'
' Version 1.01: Nov 11, 2014
' ~~~~~~~~~~~~~~~~~~~~~~~~~~
' * Added property CueBannerText:
' Sets a textual cue displayed in the ComboBox edit control to prompt user for information.
'
' * Added property CueBannerAlways: (When edit control has no user text)...
' When TRUE the cue banner text will be shown even when the edit control has focus,
' when FALSE the cue banner text disappears when the edit control has focus.
'
' Version 1.00: June 05, 2014
' ~~~~~~~~~~~~~~~~~~~~~~~~~~~
'
#End Region

Imports System.Runtime.InteropServices
Imports System.ComponentModel

Public Class ComboEz
    Inherits ComboBox
    Private components As System.ComponentModel.IContainer
    Friend WithEvents ImageList1 As System.Windows.Forms.ImageList
    Friend WithEvents PicSearch As System.Windows.Forms.PictureBox
    ' An event to raise when user clicks the search button (a PicBox).
    Public Event ClickSearch(sender As System.Object, e As System.EventArgs)

#Region "Initalize" ' Added by VB
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ComboEz))
        Me.PicSearch = New System.Windows.Forms.PictureBox()
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        CType(Me.PicSearch, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'PicSearch
        '
        Me.PicSearch.Cursor = System.Windows.Forms.Cursors.Hand
        Me.PicSearch.Location = New System.Drawing.Point(0, 0)
        Me.PicSearch.Name = "PicSearch"
        Me.PicSearch.Size = New System.Drawing.Size(16, 16)
        Me.PicSearch.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage
        Me.PicSearch.TabIndex = 0
        Me.PicSearch.TabStop = False
        '
        'ImageList1
        '
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.Transparent
        Me.ImageList1.Images.SetKeyName(0, "Search.png")
        Me.ImageList1.Images.SetKeyName(1, "Search_Disabled.png")
        '
        'ComboEz
        '
        CType(Me.PicSearch, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
#End Region

#Region "Subs / Overrides / Shadows"
    ' sub new
    Public Sub New()
        InitializeComponent()
        ' change picbox parent to Me(combobox).
        PicSearch.Parent = Me
        ' Set button image for enable/disabled state.
        'SetButtonEnableImage()
    End Sub

    ' support for PerformClick method.
    Public Sub PerformClick()
        RaiseEvent ClickSearch(Me, EventArgs.Empty)
    End Sub

    ' Raise event when picbox clicked. (Our search button)
    Private Sub PicSearch_Click(sender As System.Object, e As System.EventArgs) Handles PicSearch.Click
        RaiseEvent ClickSearch(Me, EventArgs.Empty)
    End Sub

    ' subclass
    Protected Overrides Sub WndProc(ByRef m As System.Windows.Forms.Message)
        If m.Msg = &HF Then ' WM_PAINT = &HF
            UpdateControls() ' When combo is painted re-adjust controls.
        End If
        MyBase.WndProc(m)
    End Sub

    ' adjust Picbox & Combo's editbox bounds.
    Private Sub UpdateControls()
        ' if control not visible then ignore updating.
        If Not Me.Visible Then Return
        ' get handle to Edit box in combo
        Dim hWndEdit As IntPtr = FindWindowEx(Me.Handle, IntPtr.Zero, "Edit", Nothing)
        If Not hWndEdit = IntPtr.Zero Then
            ' get combo's Edit window size
            Dim editRect As New RECT
            GetWindowRect(hWndEdit, editRect)
            ' me rect
            Dim containerRect As New RECT
            GetWindowRect(Me.Handle, containerRect)
            ' edit box
            Dim editBoxLocation As Rectangle
            With editBoxLocation
                .X = editRect.Left - containerRect.Left
                .Y = editRect.Top - containerRect.Top
                .Width = Me.ClientSize.Width - (SystemInformation.HorizontalScrollBarThumbWidth + 4)
                .Height = editRect.Bottom - editRect.Top
                ' set picbox bounds (size & location).
                PicSearch.SetBounds(.X + (.Width - 16), .Y, 16, .Height)
                ' set picbox backcolor for enable or disabled state
                If Me.Enabled Then
                    PicSearch.BackColor = Me.BackColor
                Else
                    PicSearch.BackColor = System.Drawing.SystemColors.ButtonFace
                End If
                ' resize combo editbox width (so we can see our picbox/search button).
                MoveWindow(hWndEdit, .X, .Y, .Width - 16, .Height, True)
            End With
        End If
    End Sub

    ' set button image on enabled change
    Private Sub ComboBoxButton_EnabledChanged(sender As Object, e As System.EventArgs) Handles Me.EnabledChanged
        SetButtonEnableImage()
    End Sub

    ' set button image for enable or disabled states
    Private Sub SetButtonEnableImage()
        If Me.Enabled Then
            PicSearch.Image = Me.ImageList1.Images(0) ' Search.png
        Else
            PicSearch.Image = Me.ImageList1.Images(1) ' Search_Disabled.png
        End If
    End Sub

    ' make sure style can't be changed at runtime.
    Protected Overrides Sub OnDropDownStyleChanged(e As System.EventArgs)
        MyBase.DropDownStyle = ComboBoxStyle.DropDown
        MyBase.OnDropDownStyleChanged(e)
    End Sub

    ' make sure style can't be changed at design time, hide from properties.
    <Browsable(False), EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public Shadows Property DropDownStyle As ComboBoxStyle
        Get
            Return MyBase.DropDownStyle
        End Get
        Set(value As ComboBoxStyle)
            MyBase.DropDownStyle = value
        End Set
    End Property

    ' OnCreateControl: Set cuebanner text on startup here.(Note: Raised after OnHandleCreated).
    Protected Overrides Sub OnCreateControl()
        MyBase.OnCreateControl()
        ' To set the cuebanner text at startup (from properties set at design time),
        ' we must call the API after the control has been created!
        If Not _CueBannerText = String.Empty Then
            Me.CueBannerText = _CueBannerText
        End If
    End Sub
#End Region

#Region "CueBanner"
    ' Get/Set Cue Banner Text
    Private _CueBannerText As String = String.Empty
    Public Property CueBannerText() As String
        Get
            Return _CueBannerText
        End Get
        Set(ByVal value As String)
            _CueBannerText = value
            ' Make sure this control is created before calling API or app will crash!
            If Me.Created Then
                ' get handle to the "edit" control in combobox control
                Dim hWndEdit As IntPtr = FindWindowEx(Me.Handle, IntPtr.Zero, "Edit", Nothing)
                If Not hWndEdit.Equals(IntPtr.Zero) Then
                    'Set cue banner text. Note: Requires Visual Styles enabled!
                    SendMessageW(hWndEdit, EM_SETCUEBANNER, CueBannerAlways, value)
                End If
            End If
        End Set
    End Property

    ' CueBannerAlways:
    Public Property CueBannerAlways As Boolean = False

#End Region '-CueBanner

#Region "Windows API"
    <StructLayout(LayoutKind.Sequential)> _
    Private Structure RECT
        Public Left, Top, Right, Bottom As Int32
    End Structure

    <DllImport("user32.dll")> _
    Private Shared Function GetWindowRect(ByVal hWnd As IntPtr, ByRef lpRect As RECT) As Boolean
    End Function

    <DllImport("user32.dll", SetLastError:=True, CharSet:=CharSet.Auto)> _
    Private Shared Function FindWindowEx(ByVal parentHandle As IntPtr, ByVal childAfter As IntPtr, ByVal lclassName As String, ByVal windowTitle As String) As IntPtr
    End Function

    <DllImport("user32.dll")> _
    Private Shared Function MoveWindow(ByVal hWnd As IntPtr, ByVal x As Int32, ByVal y As Int32, ByVal nWidth As Int32, ByVal nHeight As Int32, ByVal bRepaint As Boolean) As Boolean
    End Function

    ' cue banner, http://msdn.microsoft.com/en-us/library/windows/desktop/bb761639%28v=vs.85%29.aspx
    Private Const EM_SETCUEBANNER As Integer = &H1501
    <DllImport("user32.dll", EntryPoint:="SendMessageW")>
    Private Shared Function SendMessageW(hWnd As IntPtr, Msg As UInt32, wParam As Boolean, <MarshalAs(UnmanagedType.LPWStr)> lParam As String) As IntPtr
    End Function

#End Region

End Class
