<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmUtlSMTPSettings
    Inherits System.Windows.Forms.Form
    'Inherits MetroFramework.Forms.MetroForm

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
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmUtlSMTPSettings))
        Me.fbd = New System.Windows.Forms.FolderBrowserDialog()
        Me.fsd = New System.Windows.Forms.OpenFileDialog()
        Me.lblfsize = New System.Windows.Forms.Label()
        Me.lblfname = New System.Windows.Forms.Label()
        Me.lblmods = New System.Windows.Forms.Label()
        Me.Panel7 = New System.Windows.Forms.Panel()
        Me.txtTestMailID = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.txtSmtpHost = New System.Windows.Forms.TextBox()
        Me.txtSmtpMailID = New System.Windows.Forms.TextBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.chkSSL = New System.Windows.Forms.CheckBox()
        Me.txtSmtpMailPwd = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.txtSmtpServerPort = New System.Windows.Forms.TextBox()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.btnTestMail = New System.Windows.Forms.Button()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.btnSmtpClose = New vLQMS.CustomButton.CustomButton()
        Me.btnSmtpSave = New vLQMS.CustomButton.CustomButton()
        Me.chkTLS = New System.Windows.Forms.CheckBox()
        Me.Panel7.SuspendLayout()
        Me.SuspendLayout()
        '
        'fsd
        '
        Me.fsd.FileName = "OpenFileDialog1"
        '
        'lblfsize
        '
        Me.lblfsize.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblfsize.AutoSize = True
        Me.lblfsize.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblfsize.Location = New System.Drawing.Point(12, 246)
        Me.lblfsize.Name = "lblfsize"
        Me.lblfsize.Size = New System.Drawing.Size(0, 16)
        Me.lblfsize.TabIndex = 13
        '
        'lblfname
        '
        Me.lblfname.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblfname.AutoSize = True
        Me.lblfname.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblfname.Location = New System.Drawing.Point(12, 224)
        Me.lblfname.Name = "lblfname"
        Me.lblfname.Size = New System.Drawing.Size(0, 16)
        Me.lblfname.TabIndex = 14
        '
        'lblmods
        '
        Me.lblmods.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblmods.AutoSize = True
        Me.lblmods.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblmods.Location = New System.Drawing.Point(12, 267)
        Me.lblmods.Name = "lblmods"
        Me.lblmods.Size = New System.Drawing.Size(0, 16)
        Me.lblmods.TabIndex = 15
        '
        'Panel7
        '
        Me.Panel7.BackColor = System.Drawing.Color.Transparent
        Me.Panel7.Controls.Add(Me.chkTLS)
        Me.Panel7.Controls.Add(Me.txtTestMailID)
        Me.Panel7.Controls.Add(Me.Label1)
        Me.Panel7.Controls.Add(Me.Label5)
        Me.Panel7.Controls.Add(Me.txtSmtpHost)
        Me.Panel7.Controls.Add(Me.txtSmtpMailID)
        Me.Panel7.Controls.Add(Me.Label8)
        Me.Panel7.Controls.Add(Me.chkSSL)
        Me.Panel7.Controls.Add(Me.txtSmtpMailPwd)
        Me.Panel7.Controls.Add(Me.Label6)
        Me.Panel7.Controls.Add(Me.Label7)
        Me.Panel7.Controls.Add(Me.txtSmtpServerPort)
        Me.Panel7.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Panel7.Location = New System.Drawing.Point(12, 4)
        Me.Panel7.Name = "Panel7"
        Me.Panel7.Size = New System.Drawing.Size(507, 229)
        Me.Panel7.TabIndex = 16
        '
        'txtTestMailID
        '
        Me.txtTestMailID.Location = New System.Drawing.Point(197, 147)
        Me.txtTestMailID.MaxLength = 250
        Me.txtTestMailID.Name = "txtTestMailID"
        Me.txtTestMailID.Size = New System.Drawing.Size(300, 26)
        Me.txtTestMailID.TabIndex = 32
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(85, 150)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(105, 20)
        Me.Label1.TabIndex = 31
        Me.Label1.Text = "Test Mail ID"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(42, 11)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(150, 20)
        Me.Label5.TabIndex = 22
        Me.Label5.Text = "SMTP Host Name"
        '
        'txtSmtpHost
        '
        Me.txtSmtpHost.Location = New System.Drawing.Point(197, 8)
        Me.txtSmtpHost.MaxLength = 250
        Me.txtSmtpHost.Name = "txtSmtpHost"
        Me.txtSmtpHost.Size = New System.Drawing.Size(300, 26)
        Me.txtSmtpHost.TabIndex = 26
        '
        'txtSmtpMailID
        '
        Me.txtSmtpMailID.Location = New System.Drawing.Point(197, 42)
        Me.txtSmtpMailID.MaxLength = 250
        Me.txtSmtpMailID.Name = "txtSmtpMailID"
        Me.txtSmtpMailID.Size = New System.Drawing.Size(300, 26)
        Me.txtSmtpMailID.TabIndex = 27
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(41, 115)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(151, 20)
        Me.Label8.TabIndex = 25
        Me.Label8.Text = "SMTP Server Port"
        '
        'chkSSL
        '
        Me.chkSSL.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkSSL.Location = New System.Drawing.Point(79, 184)
        Me.chkSSL.Name = "chkSSL"
        Me.chkSSL.Size = New System.Drawing.Size(86, 40)
        Me.chkSSL.TabIndex = 30
        Me.chkSSL.Text = "Enable SSL"
        Me.chkSSL.UseVisualStyleBackColor = True
        '
        'txtSmtpMailPwd
        '
        Me.txtSmtpMailPwd.Location = New System.Drawing.Point(197, 76)
        Me.txtSmtpMailPwd.MaxLength = 250
        Me.txtSmtpMailPwd.Name = "txtSmtpMailPwd"
        Me.txtSmtpMailPwd.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtSmtpMailPwd.Size = New System.Drawing.Size(300, 26)
        Me.txtSmtpMailPwd.TabIndex = 28
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(75, 45)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(117, 20)
        Me.Label6.TabIndex = 23
        Me.Label6.Text = "SMTP Mail ID"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(17, 79)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(175, 20)
        Me.Label7.TabIndex = 24
        Me.Label7.Text = "SMTP Mail Password"
        '
        'txtSmtpServerPort
        '
        Me.txtSmtpServerPort.Location = New System.Drawing.Point(198, 112)
        Me.txtSmtpServerPort.MaxLength = 250
        Me.txtSmtpServerPort.Name = "txtSmtpServerPort"
        Me.txtSmtpServerPort.Size = New System.Drawing.Size(299, 26)
        Me.txtSmtpServerPort.TabIndex = 29
        '
        'btnTestMail
        '
        Me.btnTestMail.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnTestMail.BackgroundImage = CType(resources.GetObject("btnTestMail.BackgroundImage"), System.Drawing.Image)
        Me.btnTestMail.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnTestMail.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnTestMail.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnTestMail.ImageAlign = System.Drawing.ContentAlignment.TopCenter
        Me.btnTestMail.Location = New System.Drawing.Point(275, 239)
        Me.btnTestMail.Name = "btnTestMail"
        Me.btnTestMail.Size = New System.Drawing.Size(43, 35)
        Me.btnTestMail.TabIndex = 19
        Me.ToolTip1.SetToolTip(Me.btnTestMail, "Test Mail : Alt + T")
        Me.btnTestMail.UseVisualStyleBackColor = True
        '
        'btnSave
        '
        Me.btnSave.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSave.BackColor = System.Drawing.Color.Transparent
        Me.btnSave.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_SAVE
        Me.btnSave.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSave.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSave.Location = New System.Drawing.Point(324, 239)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(88, 35)
        Me.btnSave.TabIndex = 82
        Me.btnSave.Text = "&Save"
        Me.btnSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnSave, "Shortcut : Alt + S")
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'btnClose
        '
        Me.btnClose.AutoSize = True
        Me.btnClose.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_CLOSE
        Me.btnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnClose.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnClose.Location = New System.Drawing.Point(419, 239)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(88, 35)
        Me.btnClose.TabIndex = 104
        Me.btnClose.Text = "&Close"
        Me.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnClose, "Shortcut : Alt + C")
        Me.btnClose.UseVisualStyleBackColor = True
        '
        'btnSmtpClose
        '
        Me.btnSmtpClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSmtpClose.BackColor = System.Drawing.Color.Transparent
        Me.btnSmtpClose.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_CLOSE
        Me.btnSmtpClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnSmtpClose.Cursor = System.Windows.Forms.Cursors.Arrow
        Me.btnSmtpClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSmtpClose.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSmtpClose.ForeColor = System.Drawing.Color.White
        Me.btnSmtpClose.Location = New System.Drawing.Point(140, 243)
        Me.btnSmtpClose.Margin = New System.Windows.Forms.Padding(4, 1, 4, 1)
        Me.btnSmtpClose.Name = "btnSmtpClose"
        Me.btnSmtpClose.Padding = New System.Windows.Forms.Padding(4)
        Me.btnSmtpClose.Size = New System.Drawing.Size(88, 35)
        Me.btnSmtpClose.TabIndex = 18
        Me.btnSmtpClose.Text = "Cl&ose"
        Me.btnSmtpClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnSmtpClose, "Close : Alt + O")
        Me.btnSmtpClose.UseVisualStyleBackColor = True
        Me.btnSmtpClose.Visible = False
        '
        'btnSmtpSave
        '
        Me.btnSmtpSave.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSmtpSave.BackColor = System.Drawing.Color.Transparent
        Me.btnSmtpSave.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_SAVE
        Me.btnSmtpSave.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnSmtpSave.Cursor = System.Windows.Forms.Cursors.Arrow
        Me.btnSmtpSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSmtpSave.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSmtpSave.ForeColor = System.Drawing.Color.White
        Me.btnSmtpSave.Location = New System.Drawing.Point(44, 243)
        Me.btnSmtpSave.Margin = New System.Windows.Forms.Padding(4, 1, 4, 1)
        Me.btnSmtpSave.Name = "btnSmtpSave"
        Me.btnSmtpSave.Padding = New System.Windows.Forms.Padding(4)
        Me.btnSmtpSave.Size = New System.Drawing.Size(88, 35)
        Me.btnSmtpSave.TabIndex = 17
        Me.btnSmtpSave.Text = "&Save"
        Me.btnSmtpSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnSmtpSave, "Save : Alt + S")
        Me.btnSmtpSave.UseVisualStyleBackColor = True
        Me.btnSmtpSave.Visible = False
        '
        'chkTLS
        '
        Me.chkTLS.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkTLS.Location = New System.Drawing.Point(172, 179)
        Me.chkTLS.Margin = New System.Windows.Forms.Padding(4)
        Me.chkTLS.Name = "chkTLS"
        Me.chkTLS.Size = New System.Drawing.Size(115, 49)
        Me.chkTLS.TabIndex = 34
        Me.chkTLS.Text = "Enable TLS"
        Me.chkTLS.UseVisualStyleBackColor = True
        '
        'frmUtlSMTPSettings
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(530, 282)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.btnSave)
        Me.Controls.Add(Me.btnTestMail)
        Me.Controls.Add(Me.btnSmtpClose)
        Me.Controls.Add(Me.btnSmtpSave)
        Me.Controls.Add(Me.Panel7)
        Me.Controls.Add(Me.lblmods)
        Me.Controls.Add(Me.lblfname)
        Me.Controls.Add(Me.lblfsize)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.MaximumSize = New System.Drawing.Size(546, 332)
        Me.MinimumSize = New System.Drawing.Size(546, 281)
        Me.Name = "frmUtlSMTPSettings"
        Me.ShowIcon = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "SMTP Settings"
        Me.Panel7.ResumeLayout(False)
        Me.Panel7.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents fbd As System.Windows.Forms.FolderBrowserDialog
    Friend WithEvents fsd As System.Windows.Forms.OpenFileDialog
    Friend WithEvents lblfsize As System.Windows.Forms.Label
    Friend WithEvents lblfname As System.Windows.Forms.Label
    Friend WithEvents lblmods As System.Windows.Forms.Label
    Friend WithEvents Panel7 As System.Windows.Forms.Panel
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents txtSmtpHost As System.Windows.Forms.TextBox
    Friend WithEvents txtSmtpMailID As System.Windows.Forms.TextBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents chkSSL As System.Windows.Forms.CheckBox
    Friend WithEvents txtSmtpMailPwd As System.Windows.Forms.TextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents txtSmtpServerPort As System.Windows.Forms.TextBox
    Friend WithEvents btnSmtpClose As vLQMS.CustomButton.CustomButton
    Friend WithEvents btnSmtpSave As vLQMS.CustomButton.CustomButton
    Friend WithEvents ToolTip1 As ToolTip
    Friend WithEvents btnTestMail As Button
    Friend WithEvents btnSave As Button
    Friend WithEvents btnClose As Button
    Friend WithEvents txtTestMailID As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents chkTLS As CheckBox
End Class
