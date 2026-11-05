<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmUtlWebCam
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
        Me.btnStop = New System.Windows.Forms.Button()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.btnStart = New System.Windows.Forms.Button()
        Me.lblDevice = New System.Windows.Forms.Label()
        Me.lstDevices = New System.Windows.Forms.ListBox()
        Me.picCapture = New System.Windows.Forms.PictureBox()
        Me.sfdImage = New System.Windows.Forms.SaveFileDialog()
        CType(Me.picCapture, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'btnStop
        '
        Me.btnStop.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.btnStop.Location = New System.Drawing.Point(56, 273)
        Me.btnStop.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnStop.Name = "btnStop"
        Me.btnStop.Size = New System.Drawing.Size(131, 42)
        Me.btnStop.TabIndex = 11
        Me.btnStop.Text = "Stop Preview"
        '
        'btnSave
        '
        Me.btnSave.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSave.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_SAVE
        Me.btnSave.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSave.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.btnSave.Location = New System.Drawing.Point(56, 224)
        Me.btnSave.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(131, 42)
        Me.btnSave.TabIndex = 10
        Me.btnSave.Text = "Save Image"
        Me.btnSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'btnStart
        '
        Me.btnStart.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.btnStart.Location = New System.Drawing.Point(56, 179)
        Me.btnStart.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnStart.Name = "btnStart"
        Me.btnStart.Size = New System.Drawing.Size(131, 42)
        Me.btnStart.TabIndex = 9
        Me.btnStart.Text = "Start Preview"
        '
        'lblDevice
        '
        Me.lblDevice.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.lblDevice.Location = New System.Drawing.Point(14, 17)
        Me.lblDevice.Name = "lblDevice"
        Me.lblDevice.Size = New System.Drawing.Size(215, 21)
        Me.lblDevice.TabIndex = 8
        Me.lblDevice.Text = "Available Devices"
        Me.lblDevice.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lstDevices
        '
        Me.lstDevices.BackColor = System.Drawing.SystemColors.Control
        Me.lstDevices.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.lstDevices.ItemHeight = 17
        Me.lstDevices.Location = New System.Drawing.Point(14, 47)
        Me.lstDevices.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.lstDevices.Name = "lstDevices"
        Me.lstDevices.Size = New System.Drawing.Size(214, 106)
        Me.lstDevices.TabIndex = 7
        '
        'picCapture
        '
        Me.picCapture.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.picCapture.Location = New System.Drawing.Point(247, 7)
        Me.picCapture.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.picCapture.Name = "picCapture"
        Me.picCapture.Size = New System.Drawing.Size(298, 355)
        Me.picCapture.TabIndex = 6
        Me.picCapture.TabStop = False
        '
        'sfdImage
        '
        Me.sfdImage.FileName = "Webcam1"
        Me.sfdImage.Filter = "Bitmap|*.bmp"
        '
        'frmUtlWebCam
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 17.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(233, Byte), Integer), CType(CType(237, Byte), Integer), CType(CType(241, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(554, 378)
        Me.Controls.Add(Me.btnStop)
        Me.Controls.Add(Me.btnSave)
        Me.Controls.Add(Me.btnStart)
        Me.Controls.Add(Me.lblDevice)
        Me.Controls.Add(Me.lstDevices)
        Me.Controls.Add(Me.picCapture)
        Me.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.Name = "frmUtlWebCam"
        Me.ShowIcon = False
        Me.Text = "Capture Image from Web Camera"
        CType(Me.picCapture, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents btnStop As System.Windows.Forms.Button
    Friend WithEvents btnSave As System.Windows.Forms.Button
    Friend WithEvents btnStart As System.Windows.Forms.Button
    Friend WithEvents lblDevice As System.Windows.Forms.Label
    Friend WithEvents lstDevices As System.Windows.Forms.ListBox
    Friend WithEvents picCapture As System.Windows.Forms.PictureBox
    Friend WithEvents sfdImage As System.Windows.Forms.SaveFileDialog
End Class
