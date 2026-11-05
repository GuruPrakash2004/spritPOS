<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmFMTViewer
    Inherits System.Windows.Forms.Form

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
        Me.Label1 = New System.Windows.Forms.Label()
        Me.rptViewer = New CrystalDecisions.Windows.Forms.CrystalReportViewer()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label1.Location = New System.Drawing.Point(479, 3)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(251, 26)
        Me.Label1.TabIndex = 3
        '
        'rptViewer
        '
        Me.rptViewer.ActiveViewIndex = -1
        Me.rptViewer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.rptViewer.Cursor = System.Windows.Forms.Cursors.Default
        Me.rptViewer.Dock = System.Windows.Forms.DockStyle.Fill
        Me.rptViewer.Location = New System.Drawing.Point(0, 0)
        Me.rptViewer.Name = "rptViewer"
        Me.rptViewer.Size = New System.Drawing.Size(730, 446)
        Me.rptViewer.TabIndex = 2
        Me.rptViewer.ToolPanelView = CrystalDecisions.Windows.Forms.ToolPanelViewType.None
        '
        'frmFMTViewer
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(730, 446)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.rptViewer)
        Me.Name = "frmFMTViewer"
        Me.ShowIcon = False
        Me.Text = "frmFMTViewer"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Private WithEvents rptViewer As CrystalDecisions.Windows.Forms.CrystalReportViewer
End Class
