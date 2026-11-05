<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmUtlAbout
    Inherits System.Windows.Forms.Form
    'Inherits MetroFramework.Forms.MetroForm

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.WB = New System.Windows.Forms.WebBrowser()
        Me.SuspendLayout()
        '
        'WB
        '
        Me.WB.AllowNavigation = False
        Me.WB.AllowWebBrowserDrop = False
        Me.WB.Dock = System.Windows.Forms.DockStyle.Fill
        Me.WB.IsWebBrowserContextMenuEnabled = False
        Me.WB.Location = New System.Drawing.Point(0, 0)
        Me.WB.MinimumSize = New System.Drawing.Size(20, 20)
        Me.WB.Name = "WB"
        Me.WB.ScriptErrorsSuppressed = True
        Me.WB.Size = New System.Drawing.Size(664, 408)
        Me.WB.TabIndex = 0
        Me.WB.Url = New System.Uri("R:\SqlDBRpt\vLQMS\Rpts\Media\index.html", System.UriKind.Absolute)
        Me.WB.WebBrowserShortcutsEnabled = False
        '
        'frmUtlAbout
        '
        Me.ClientSize = New System.Drawing.Size(664, 408)
        Me.Controls.Add(Me.WB)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmUtlAbout"
        Me.ShowIcon = False
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents fbd As System.Windows.Forms.FolderBrowserDialog
    Friend WithEvents fsd As System.Windows.Forms.OpenFileDialog
    Friend WithEvents Timer1 As System.Windows.Forms.Timer
    Friend WithEvents Panel1 As Panel
    Friend WithEvents WB As WebBrowser
End Class
