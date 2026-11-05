<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmOTP
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmOTP))
        Me.pnlOTP = New System.Windows.Forms.Panel()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.btnEmail = New System.Windows.Forms.Button()
        Me.lblOtpMail = New System.Windows.Forms.Label()
        Me.pnlContinue = New System.Windows.Forms.Panel()
        Me.btnContinue = New System.Windows.Forms.Button()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtOTP = New System.Windows.Forms.MaskedTextBox()
        Me.lblGoBack = New System.Windows.Forms.LinkLabel()
        Me.btnSendOTP = New System.Windows.Forms.Button()
        Me.lblOTPMsg = New System.Windows.Forms.Label()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.pnlOTP.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.pnlContinue.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'pnlOTP
        '
        Me.pnlOTP.Controls.Add(Me.Panel1)
        Me.pnlOTP.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlOTP.Location = New System.Drawing.Point(0, 0)
        Me.pnlOTP.Name = "pnlOTP"
        Me.pnlOTP.Size = New System.Drawing.Size(661, 286)
        Me.pnlOTP.TabIndex = 511
        '
        'Panel1
        '
        Me.Panel1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.btnEmail)
        Me.Panel1.Controls.Add(Me.lblOtpMail)
        Me.Panel1.Controls.Add(Me.pnlContinue)
        Me.Panel1.Controls.Add(Me.lblGoBack)
        Me.Panel1.Controls.Add(Me.btnSendOTP)
        Me.Panel1.Controls.Add(Me.lblOTPMsg)
        Me.Panel1.Controls.Add(Me.PictureBox1)
        Me.Panel1.Location = New System.Drawing.Point(3, 3)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(655, 280)
        Me.Panel1.TabIndex = 0
        '
        'btnEmail
        '
        Me.btnEmail.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.btnEmail.Location = New System.Drawing.Point(14, 38)
        Me.btnEmail.Name = "btnEmail"
        Me.btnEmail.Size = New System.Drawing.Size(110, 26)
        Me.btnEmail.TabIndex = 1
        Me.btnEmail.Text = "eMail - OTP"
        Me.btnEmail.UseVisualStyleBackColor = True
        '
        'lblOtpMail
        '
        Me.lblOtpMail.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.lblOtpMail.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblOtpMail.Location = New System.Drawing.Point(130, 45)
        Me.lblOtpMail.Name = "lblOtpMail"
        Me.lblOtpMail.Size = New System.Drawing.Size(532, 19)
        Me.lblOtpMail.TabIndex = 9
        Me.lblOtpMail.Text = "<<<< Click here to send Four digit OTP to your mail "
        '
        'pnlContinue
        '
        Me.pnlContinue.Controls.Add(Me.btnContinue)
        Me.pnlContinue.Controls.Add(Me.Label5)
        Me.pnlContinue.Controls.Add(Me.Label2)
        Me.pnlContinue.Controls.Add(Me.txtOTP)
        Me.pnlContinue.Location = New System.Drawing.Point(12, 73)
        Me.pnlContinue.Name = "pnlContinue"
        Me.pnlContinue.Size = New System.Drawing.Size(330, 98)
        Me.pnlContinue.TabIndex = 8
        '
        'btnContinue
        '
        Me.btnContinue.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.btnContinue.Location = New System.Drawing.Point(225, 58)
        Me.btnContinue.Name = "btnContinue"
        Me.btnContinue.Size = New System.Drawing.Size(101, 25)
        Me.btnContinue.TabIndex = 3
        Me.btnContinue.Text = "Continue"
        Me.btnContinue.UseVisualStyleBackColor = True
        '
        'Label5
        '
        Me.Label5.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.Label5.Font = New System.Drawing.Font("Segoe UI", 11.0!)
        Me.Label5.Location = New System.Drawing.Point(11, 6)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(296, 49)
        Me.Label5.TabIndex = 0
        Me.Label5.Text = "To proceed, please enter the OTP sent to your Mobile / eMail  and click 'Continue" &
    "'" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Segoe UI", 20.0!)
        Me.Label2.Location = New System.Drawing.Point(8, 53)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(66, 37)
        Me.Label2.TabIndex = 7
        Me.Label2.Text = "OTP"
        '
        'txtOTP
        '
        Me.txtOTP.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtOTP.Font = New System.Drawing.Font("Segoe UI", 12.0!)
        Me.txtOTP.Location = New System.Drawing.Point(80, 56)
        Me.txtOTP.Name = "txtOTP"
        Me.txtOTP.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtOTP.Size = New System.Drawing.Size(139, 29)
        Me.txtOTP.TabIndex = 2
        Me.txtOTP.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'lblGoBack
        '
        Me.lblGoBack.ActiveLinkColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.lblGoBack.AutoSize = True
        Me.lblGoBack.Font = New System.Drawing.Font("Segoe UI", 12.0!)
        Me.lblGoBack.LinkColor = System.Drawing.Color.Black
        Me.lblGoBack.Location = New System.Drawing.Point(8, 241)
        Me.lblGoBack.Name = "lblGoBack"
        Me.lblGoBack.Size = New System.Drawing.Size(93, 21)
        Me.lblGoBack.TabIndex = 4
        Me.lblGoBack.TabStop = True
        Me.lblGoBack.Text = "<-- Go Back"
        '
        'btnSendOTP
        '
        Me.btnSendOTP.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.btnSendOTP.Location = New System.Drawing.Point(14, 8)
        Me.btnSendOTP.Name = "btnSendOTP"
        Me.btnSendOTP.Size = New System.Drawing.Size(110, 26)
        Me.btnSendOTP.TabIndex = 0
        Me.btnSendOTP.Text = "SMS - OTP "
        Me.btnSendOTP.UseVisualStyleBackColor = True
        '
        'lblOTPMsg
        '
        Me.lblOTPMsg.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.lblOTPMsg.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblOTPMsg.Location = New System.Drawing.Point(130, 15)
        Me.lblOTPMsg.Name = "lblOTPMsg"
        Me.lblOTPMsg.Size = New System.Drawing.Size(532, 19)
        Me.lblOTPMsg.TabIndex = 5
        Me.lblOTPMsg.Text = "<<<< Click here to send Four digit OTP to your mobile ending "
        '
        'PictureBox1
        '
        Me.PictureBox1.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.PictureBox1.BackgroundImage = CType(resources.GetObject("PictureBox1.BackgroundImage"), System.Drawing.Image)
        Me.PictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.PictureBox1.Location = New System.Drawing.Point(354, 73)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(296, 201)
        Me.PictureBox1.TabIndex = 3
        Me.PictureBox1.TabStop = False
        '
        'frmOTP
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(661, 286)
        Me.Controls.Add(Me.pnlOTP)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmOTP"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Change Password Request through OTP"
        Me.pnlOTP.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.pnlContinue.ResumeLayout(False)
        Me.pnlContinue.PerformLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents pnlOTP As Panel
    Friend WithEvents lblGoBack As LinkLabel
    Friend WithEvents Panel1 As Panel
    Friend WithEvents btnContinue As Button
    Friend WithEvents PictureBox1 As PictureBox
    Friend WithEvents txtOTP As MaskedTextBox
    Friend WithEvents Label5 As Label
    Friend WithEvents lblOTPMsg As Label
    Friend WithEvents btnSendOTP As Button
    Friend WithEvents Label2 As Label
    Friend WithEvents pnlContinue As Panel
    Friend WithEvents btnEmail As Button
    Friend WithEvents lblOtpMail As Label
End Class
