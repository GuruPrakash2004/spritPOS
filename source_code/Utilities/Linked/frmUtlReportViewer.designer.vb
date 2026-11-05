<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmUtlReportViewer
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmUtlReportViewer))
        Me.crptViewr = New CrystalDecisions.Windows.Forms.CrystalReportViewer()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'crptViewr
        '
        Me.crptViewr.ActiveViewIndex = -1
        Me.crptViewr.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.crptViewr.Cursor = System.Windows.Forms.Cursors.Default
        Me.crptViewr.Dock = System.Windows.Forms.DockStyle.Fill
        Me.crptViewr.Location = New System.Drawing.Point(0, 0)
        Me.crptViewr.Name = "crptViewr"
        Me.crptViewr.Size = New System.Drawing.Size(1123, 562)
        Me.crptViewr.TabIndex = 0
        Me.crptViewr.ToolPanelView = CrystalDecisions.Windows.Forms.ToolPanelViewType.None
        '
        'Label1
        '
        Me.Label1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label1.Location = New System.Drawing.Point(844, 2)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(251, 26)
        Me.Label1.TabIndex = 1
        '
        'frmUtlReportViewer
        '
        Me.ClientSize = New System.Drawing.Size(1123, 562)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.crptViewr)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MinimumSize = New System.Drawing.Size(950, 550)
        Me.Name = "frmUtlReportViewer"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents crptVwr As CrystalDecisions.Windows.Forms.CrystalReportViewer
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Private WithEvents crptViewr As CrystalDecisions.Windows.Forms.CrystalReportViewer
End Class
