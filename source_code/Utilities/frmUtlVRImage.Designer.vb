<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmUtlVRImage
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
        Me.PBVR = New System.Windows.Forms.PictureBox()
        CType(Me.PBVR, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'PBVR
        '
        Me.PBVR.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PBVR.Image = Global.vLQMS.My.Resources.Resources.VoiceRecon
        Me.PBVR.Location = New System.Drawing.Point(0, 0)
        Me.PBVR.Name = "PBVR"
        Me.PBVR.Size = New System.Drawing.Size(485, 177)
        Me.PBVR.TabIndex = 0
        Me.PBVR.TabStop = False
        '
        'frmUtlVRImage
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(485, 177)
        Me.Controls.Add(Me.PBVR)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "frmUtlVRImage"
        Me.ShowIcon = False
        Me.Text = "frmUtlVRImage"
        CType(Me.PBVR, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents PBVR As PictureBox
End Class
