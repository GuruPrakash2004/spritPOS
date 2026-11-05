Imports System.Drawing.Drawing2D
Public Class FrmMainSplash
    Public Sub New()
        MyBase.New()
        InitializeComponent()

        'Create the elliptical form
        Dim gPath As GraphicsPath = New GraphicsPath()
        gPath.AddEllipse(40, 40, Me.ClientSize.Width - 80, Me.ClientSize.Height - 80)
        Me.Region = New Region(gPath)

    End Sub

End Class
