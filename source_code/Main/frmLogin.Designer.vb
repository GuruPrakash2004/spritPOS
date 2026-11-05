<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
<Global.System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1726")>
Partial Class frmLogin
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
    Friend WithEvents PasswordLabel As System.Windows.Forms.Label
    Friend WithEvents txtUsername As System.Windows.Forms.TextBox
    Friend WithEvents txtPwd As System.Windows.Forms.TextBox

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmLogin))
        Me.PasswordLabel = New System.Windows.Forms.Label()
        Me.txtUsername = New System.Windows.Forms.TextBox()
        Me.txtPwd = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.btnOk = New System.Windows.Forms.Button()
        Me.btnChgPwd = New System.Windows.Forms.Button()
        Me.txtOldPwd = New System.Windows.Forms.TextBox()
        Me.txtNewPwd = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.btnCnfOk = New System.Windows.Forms.Button()
        Me.btnCnfCancel = New System.Windows.Forms.Button()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.txtCnfNewPwd = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.pnlCnfPwd = New System.Windows.Forms.Panel()
        Me.PnlPwdRules = New System.Windows.Forms.Panel()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.lblMinLen = New System.Windows.Forms.Label()
        Me.lblMinLCase = New System.Windows.Forms.Label()
        Me.lblMinUCase = New System.Windows.Forms.Label()
        Me.lblMinAlpha = New System.Windows.Forms.Label()
        Me.lblMinNum = New System.Windows.Forms.Label()
        Me.lblMinSplChars = New System.Windows.Forms.Label()
        Me.PicUser = New System.Windows.Forms.PictureBox()
        Me.chkRememberMe = New System.Windows.Forms.CheckBox()
        Me.lblFgtPwd = New System.Windows.Forms.LinkLabel()
        Me.Err = New System.Windows.Forms.ErrorProvider(Me.components)
        Me.pnlLogin = New System.Windows.Forms.Panel()
        Me.imgEye = New System.Windows.Forms.PictureBox()
        Me.lblMask = New System.Windows.Forms.Label()
        Me.lblExpiry = New System.Windows.Forms.Label()
        Me.picLoginImg = New System.Windows.Forms.PictureBox()
        Me.tt = New System.Windows.Forms.ToolTip(Me.components)
        Me.lblAdmPwd = New System.Windows.Forms.Label()
        Me.txtAdmPwd = New System.Windows.Forms.TextBox()
        Me.pnlCnfPwd.SuspendLayout()
        Me.PnlPwdRules.SuspendLayout()
        CType(Me.PicUser, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Err, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlLogin.SuspendLayout()
        CType(Me.imgEye, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.picLoginImg, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'PasswordLabel
        '
        Me.PasswordLabel.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.PasswordLabel.BackColor = System.Drawing.Color.Transparent
        Me.PasswordLabel.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.PasswordLabel.ForeColor = System.Drawing.Color.FromArgb(CType(CType(2, Byte), Integer), CType(CType(36, Byte), Integer), CType(CType(79, Byte), Integer))
        Me.PasswordLabel.Location = New System.Drawing.Point(10, 147)
        Me.PasswordLabel.Name = "PasswordLabel"
        Me.PasswordLabel.Size = New System.Drawing.Size(78, 19)
        Me.PasswordLabel.TabIndex = 2
        Me.PasswordLabel.Text = "&Password"
        Me.PasswordLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtUsername
        '
        Me.txtUsername.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.txtUsername.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtUsername.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtUsername.Location = New System.Drawing.Point(10, 118)
        Me.txtUsername.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtUsername.Name = "txtUsername"
        Me.txtUsername.Size = New System.Drawing.Size(302, 25)
        Me.txtUsername.TabIndex = 0
        '
        'txtPwd
        '
        Me.txtPwd.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.txtPwd.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPwd.Location = New System.Drawing.Point(10, 170)
        Me.txtPwd.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtPwd.Name = "txtPwd"
        Me.txtPwd.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtPwd.Size = New System.Drawing.Size(302, 25)
        Me.txtPwd.TabIndex = 1
        '
        'Label1
        '
        Me.Label1.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.Label1.BackColor = System.Drawing.Color.Transparent
        Me.Label1.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.FromArgb(CType(CType(2, Byte), Integer), CType(CType(36, Byte), Integer), CType(CType(79, Byte), Integer))
        Me.Label1.Location = New System.Drawing.Point(10, 94)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(82, 19)
        Me.Label1.TabIndex = 4
        Me.Label1.Text = "&Username"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btnCancel
        '
        Me.btnCancel.BackColor = System.Drawing.Color.FromArgb(CType(CType(2, Byte), Integer), CType(CType(36, Byte), Integer), CType(CType(79, Byte), Integer))
        Me.btnCancel.FlatAppearance.BorderSize = 0
        Me.btnCancel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(117, Byte), Integer), CType(CType(159, Byte), Integer))
        Me.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnCancel.ForeColor = System.Drawing.Color.White
        Me.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnCancel.Location = New System.Drawing.Point(299, 285)
        Me.btnCancel.Margin = New System.Windows.Forms.Padding(3, 5, 3, 5)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(121, 29)
        Me.btnCancel.TabIndex = 93
        Me.btnCancel.Text = "&Cancel"
        Me.btnCancel.UseVisualStyleBackColor = False
        Me.btnCancel.Visible = False
        '
        'btnOk
        '
        Me.btnOk.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.btnOk.BackColor = System.Drawing.Color.FromArgb(CType(CType(2, Byte), Integer), CType(CType(36, Byte), Integer), CType(CType(79, Byte), Integer))
        Me.btnOk.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnOk.FlatAppearance.BorderSize = 0
        Me.btnOk.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(117, Byte), Integer), CType(CType(159, Byte), Integer))
        Me.btnOk.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnOk.ForeColor = System.Drawing.Color.White
        Me.btnOk.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnOk.Location = New System.Drawing.Point(10, 283)
        Me.btnOk.Margin = New System.Windows.Forms.Padding(3, 5, 3, 5)
        Me.btnOk.Name = "btnOk"
        Me.btnOk.Size = New System.Drawing.Size(302, 29)
        Me.btnOk.TabIndex = 92
        Me.btnOk.Text = "&Login"
        Me.btnOk.UseVisualStyleBackColor = False
        '
        'btnChgPwd
        '
        Me.btnChgPwd.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.btnChgPwd.BackColor = System.Drawing.Color.FromArgb(CType(CType(2, Byte), Integer), CType(CType(36, Byte), Integer), CType(CType(79, Byte), Integer))
        Me.btnChgPwd.BackgroundImage = CType(resources.GetObject("btnChgPwd.BackgroundImage"), System.Drawing.Image)
        Me.btnChgPwd.FlatAppearance.BorderSize = 0
        Me.btnChgPwd.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(117, Byte), Integer), CType(CType(159, Byte), Integer))
        Me.btnChgPwd.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnChgPwd.ForeColor = System.Drawing.Color.White
        Me.btnChgPwd.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnChgPwd.Location = New System.Drawing.Point(330, 270)
        Me.btnChgPwd.Margin = New System.Windows.Forms.Padding(3, 5, 3, 5)
        Me.btnChgPwd.Name = "btnChgPwd"
        Me.btnChgPwd.Size = New System.Drawing.Size(16, 17)
        Me.btnChgPwd.TabIndex = 94
        Me.btnChgPwd.Tag = "Change Password"
        Me.btnChgPwd.Text = "Change &Password"
        Me.btnChgPwd.UseVisualStyleBackColor = False
        Me.btnChgPwd.Visible = False
        '
        'txtOldPwd
        '
        Me.txtOldPwd.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtOldPwd.Location = New System.Drawing.Point(5, 51)
        Me.txtOldPwd.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtOldPwd.Name = "txtOldPwd"
        Me.txtOldPwd.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtOldPwd.Size = New System.Drawing.Size(371, 25)
        Me.txtOldPwd.TabIndex = 0
        '
        'txtNewPwd
        '
        Me.txtNewPwd.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtNewPwd.Location = New System.Drawing.Point(5, 98)
        Me.txtNewPwd.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtNewPwd.Name = "txtNewPwd"
        Me.txtNewPwd.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtNewPwd.Size = New System.Drawing.Size(371, 25)
        Me.txtNewPwd.TabIndex = 1
        '
        'Label2
        '
        Me.Label2.BackColor = System.Drawing.Color.Transparent
        Me.Label2.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.Color.FromArgb(CType(CType(2, Byte), Integer), CType(CType(36, Byte), Integer), CType(CType(79, Byte), Integer))
        Me.Label2.Location = New System.Drawing.Point(5, 30)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(82, 19)
        Me.Label2.TabIndex = 98
        Me.Label2.Text = "Old Password"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btnCnfOk
        '
        Me.btnCnfOk.BackColor = System.Drawing.Color.FromArgb(CType(CType(2, Byte), Integer), CType(CType(36, Byte), Integer), CType(CType(79, Byte), Integer))
        Me.btnCnfOk.FlatAppearance.BorderSize = 0
        Me.btnCnfOk.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(117, Byte), Integer), CType(CType(159, Byte), Integer))
        Me.btnCnfOk.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnCnfOk.ForeColor = System.Drawing.Color.White
        Me.btnCnfOk.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnCnfOk.Location = New System.Drawing.Point(146, 202)
        Me.btnCnfOk.Margin = New System.Windows.Forms.Padding(3, 5, 3, 5)
        Me.btnCnfOk.Name = "btnCnfOk"
        Me.btnCnfOk.Size = New System.Drawing.Size(114, 29)
        Me.btnCnfOk.TabIndex = 3
        Me.btnCnfOk.Text = "&Ok"
        Me.btnCnfOk.UseVisualStyleBackColor = False
        '
        'btnCnfCancel
        '
        Me.btnCnfCancel.BackColor = System.Drawing.Color.FromArgb(CType(CType(2, Byte), Integer), CType(CType(36, Byte), Integer), CType(CType(79, Byte), Integer))
        Me.btnCnfCancel.FlatAppearance.BorderSize = 0
        Me.btnCnfCancel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(117, Byte), Integer), CType(CType(159, Byte), Integer))
        Me.btnCnfCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnCnfCancel.ForeColor = System.Drawing.Color.White
        Me.btnCnfCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnCnfCancel.Location = New System.Drawing.Point(262, 202)
        Me.btnCnfCancel.Margin = New System.Windows.Forms.Padding(3, 5, 3, 5)
        Me.btnCnfCancel.Name = "btnCnfCancel"
        Me.btnCnfCancel.Size = New System.Drawing.Size(114, 29)
        Me.btnCnfCancel.TabIndex = 4
        Me.btnCnfCancel.Text = "&Cancel"
        Me.btnCnfCancel.UseVisualStyleBackColor = False
        '
        'Label4
        '
        Me.Label4.BackColor = System.Drawing.Color.Transparent
        Me.Label4.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.Color.FromArgb(CType(CType(2, Byte), Integer), CType(CType(36, Byte), Integer), CType(CType(79, Byte), Integer))
        Me.Label4.Location = New System.Drawing.Point(5, 127)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(171, 19)
        Me.Label4.TabIndex = 103
        Me.Label4.Text = "Confirm New Password"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtCnfNewPwd
        '
        Me.txtCnfNewPwd.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCnfNewPwd.Location = New System.Drawing.Point(5, 148)
        Me.txtCnfNewPwd.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtCnfNewPwd.Name = "txtCnfNewPwd"
        Me.txtCnfNewPwd.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtCnfNewPwd.Size = New System.Drawing.Size(371, 25)
        Me.txtCnfNewPwd.TabIndex = 2
        '
        'Label3
        '
        Me.Label3.BackColor = System.Drawing.Color.Transparent
        Me.Label3.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.Color.FromArgb(CType(CType(2, Byte), Integer), CType(CType(36, Byte), Integer), CType(CType(79, Byte), Integer))
        Me.Label3.Location = New System.Drawing.Point(5, 80)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(76, 19)
        Me.Label3.TabIndex = 104
        Me.Label3.Text = "New Password"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'pnlCnfPwd
        '
        Me.pnlCnfPwd.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.pnlCnfPwd.BackColor = System.Drawing.Color.Transparent
        Me.pnlCnfPwd.Controls.Add(Me.PnlPwdRules)
        Me.pnlCnfPwd.Controls.Add(Me.Label3)
        Me.pnlCnfPwd.Controls.Add(Me.txtCnfNewPwd)
        Me.pnlCnfPwd.Controls.Add(Me.Label4)
        Me.pnlCnfPwd.Controls.Add(Me.btnCnfCancel)
        Me.pnlCnfPwd.Controls.Add(Me.btnCnfOk)
        Me.pnlCnfPwd.Controls.Add(Me.Label2)
        Me.pnlCnfPwd.Controls.Add(Me.txtNewPwd)
        Me.pnlCnfPwd.Controls.Add(Me.txtOldPwd)
        Me.pnlCnfPwd.Location = New System.Drawing.Point(44, 93)
        Me.pnlCnfPwd.Name = "pnlCnfPwd"
        Me.pnlCnfPwd.Size = New System.Drawing.Size(714, 260)
        Me.pnlCnfPwd.TabIndex = 95
        Me.pnlCnfPwd.Visible = False
        '
        'PnlPwdRules
        '
        Me.PnlPwdRules.BackColor = System.Drawing.Color.White
        Me.PnlPwdRules.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.PnlPwdRules.Controls.Add(Me.Label5)
        Me.PnlPwdRules.Controls.Add(Me.Label11)
        Me.PnlPwdRules.Controls.Add(Me.lblMinLen)
        Me.PnlPwdRules.Controls.Add(Me.lblMinLCase)
        Me.PnlPwdRules.Controls.Add(Me.lblMinUCase)
        Me.PnlPwdRules.Controls.Add(Me.lblMinAlpha)
        Me.PnlPwdRules.Controls.Add(Me.lblMinNum)
        Me.PnlPwdRules.Controls.Add(Me.lblMinSplChars)
        Me.PnlPwdRules.Location = New System.Drawing.Point(391, 56)
        Me.PnlPwdRules.Name = "PnlPwdRules"
        Me.PnlPwdRules.Size = New System.Drawing.Size(253, 175)
        Me.PnlPwdRules.TabIndex = 274
        '
        'Label5
        '
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.0!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.ForeColor = System.Drawing.SystemColors.ControlDarkDark
        Me.Label5.Location = New System.Drawing.Point(4, 142)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(244, 30)
        Me.Label5.TabIndex = 7
        Me.Label5.Text = "(Above settings can be changed in " & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Global Settings --> Login Settings)"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Underline)
        Me.Label11.Location = New System.Drawing.Point(70, 8)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(119, 17)
        Me.Label11.TabIndex = 6
        Me.Label11.Text = "*Password Rules*"
        '
        'lblMinLen
        '
        Me.lblMinLen.AutoSize = True
        Me.lblMinLen.Location = New System.Drawing.Point(6, 122)
        Me.lblMinLen.Name = "lblMinLen"
        Me.lblMinLen.Size = New System.Drawing.Size(15, 17)
        Me.lblMinLen.TabIndex = 5
        Me.lblMinLen.Text = "1"
        '
        'lblMinLCase
        '
        Me.lblMinLCase.AutoSize = True
        Me.lblMinLCase.Location = New System.Drawing.Point(6, 105)
        Me.lblMinLCase.Name = "lblMinLCase"
        Me.lblMinLCase.Size = New System.Drawing.Size(15, 17)
        Me.lblMinLCase.TabIndex = 4
        Me.lblMinLCase.Text = "1"
        '
        'lblMinUCase
        '
        Me.lblMinUCase.AutoSize = True
        Me.lblMinUCase.Location = New System.Drawing.Point(6, 88)
        Me.lblMinUCase.Name = "lblMinUCase"
        Me.lblMinUCase.Size = New System.Drawing.Size(15, 17)
        Me.lblMinUCase.TabIndex = 3
        Me.lblMinUCase.Text = "1"
        '
        'lblMinAlpha
        '
        Me.lblMinAlpha.AutoSize = True
        Me.lblMinAlpha.Location = New System.Drawing.Point(6, 71)
        Me.lblMinAlpha.Name = "lblMinAlpha"
        Me.lblMinAlpha.Size = New System.Drawing.Size(15, 17)
        Me.lblMinAlpha.TabIndex = 2
        Me.lblMinAlpha.Text = "1"
        '
        'lblMinNum
        '
        Me.lblMinNum.AutoSize = True
        Me.lblMinNum.Location = New System.Drawing.Point(6, 54)
        Me.lblMinNum.Name = "lblMinNum"
        Me.lblMinNum.Size = New System.Drawing.Size(15, 17)
        Me.lblMinNum.TabIndex = 1
        Me.lblMinNum.Text = "1"
        '
        'lblMinSplChars
        '
        Me.lblMinSplChars.AutoSize = True
        Me.lblMinSplChars.Location = New System.Drawing.Point(6, 37)
        Me.lblMinSplChars.Name = "lblMinSplChars"
        Me.lblMinSplChars.Size = New System.Drawing.Size(15, 17)
        Me.lblMinSplChars.TabIndex = 0
        Me.lblMinSplChars.Text = "1"
        '
        'PicUser
        '
        Me.PicUser.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.PicUser.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.PicUser.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.PicUser.Location = New System.Drawing.Point(106, 18)
        Me.PicUser.Name = "PicUser"
        Me.PicUser.Size = New System.Drawing.Size(110, 93)
        Me.PicUser.TabIndex = 458
        Me.PicUser.TabStop = False
        '
        'chkRememberMe
        '
        Me.chkRememberMe.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.chkRememberMe.AutoSize = True
        Me.chkRememberMe.BackColor = System.Drawing.Color.Transparent
        Me.chkRememberMe.Location = New System.Drawing.Point(49, 478)
        Me.chkRememberMe.Name = "chkRememberMe"
        Me.chkRememberMe.Size = New System.Drawing.Size(114, 21)
        Me.chkRememberMe.TabIndex = 508
        Me.chkRememberMe.Text = "Remember Me"
        Me.chkRememberMe.UseVisualStyleBackColor = False
        Me.chkRememberMe.Visible = False
        '
        'lblFgtPwd
        '
        Me.lblFgtPwd.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.lblFgtPwd.AutoSize = True
        Me.lblFgtPwd.BackColor = System.Drawing.Color.Transparent
        Me.lblFgtPwd.Location = New System.Drawing.Point(112, 315)
        Me.lblFgtPwd.Name = "lblFgtPwd"
        Me.lblFgtPwd.Size = New System.Drawing.Size(113, 17)
        Me.lblFgtPwd.TabIndex = 509
        Me.lblFgtPwd.TabStop = True
        Me.lblFgtPwd.Text = "Forgot Password?"
        Me.lblFgtPwd.Visible = False
        '
        'Err
        '
        Me.Err.BlinkRate = 500
        Me.Err.ContainerControl = Me
        '
        'pnlLogin
        '
        Me.pnlLogin.AllowDrop = True
        Me.pnlLogin.BackColor = System.Drawing.Color.Transparent
        Me.pnlLogin.Controls.Add(Me.lblAdmPwd)
        Me.pnlLogin.Controls.Add(Me.txtAdmPwd)
        Me.pnlLogin.Controls.Add(Me.imgEye)
        Me.pnlLogin.Controls.Add(Me.PicUser)
        Me.pnlLogin.Controls.Add(Me.btnChgPwd)
        Me.pnlLogin.Controls.Add(Me.lblMask)
        Me.pnlLogin.Controls.Add(Me.PasswordLabel)
        Me.pnlLogin.Controls.Add(Me.Label1)
        Me.pnlLogin.Controls.Add(Me.txtUsername)
        Me.pnlLogin.Controls.Add(Me.btnOk)
        Me.pnlLogin.Controls.Add(Me.btnCancel)
        Me.pnlLogin.Controls.Add(Me.txtPwd)
        Me.pnlLogin.Controls.Add(Me.lblFgtPwd)
        Me.pnlLogin.Location = New System.Drawing.Point(44, 91)
        Me.pnlLogin.Name = "pnlLogin"
        Me.pnlLogin.Size = New System.Drawing.Size(446, 332)
        Me.pnlLogin.TabIndex = 511
        '
        'imgEye
        '
        Me.imgEye.Image = CType(resources.GetObject("imgEye.Image"), System.Drawing.Image)
        Me.imgEye.Location = New System.Drawing.Point(316, 172)
        Me.imgEye.Name = "imgEye"
        Me.imgEye.Size = New System.Drawing.Size(30, 24)
        Me.imgEye.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.imgEye.TabIndex = 514
        Me.imgEye.TabStop = False
        '
        'lblMask
        '
        Me.lblMask.Location = New System.Drawing.Point(3, 19)
        Me.lblMask.Name = "lblMask"
        Me.lblMask.Size = New System.Drawing.Size(100, 23)
        Me.lblMask.TabIndex = 0
        '
        'lblExpiry
        '
        Me.lblExpiry.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.lblExpiry.BackColor = System.Drawing.Color.Transparent
        Me.lblExpiry.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblExpiry.ForeColor = System.Drawing.Color.Maroon
        Me.lblExpiry.Location = New System.Drawing.Point(47, 497)
        Me.lblExpiry.Name = "lblExpiry"
        Me.lblExpiry.Size = New System.Drawing.Size(404, 17)
        Me.lblExpiry.TabIndex = 513
        Me.lblExpiry.Text = "Password"
        Me.lblExpiry.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.lblExpiry.Visible = False
        '
        'picLoginImg
        '
        Me.picLoginImg.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.picLoginImg.BackColor = System.Drawing.Color.Transparent
        Me.picLoginImg.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.picLoginImg.Location = New System.Drawing.Point(608, 35)
        Me.picLoginImg.Name = "picLoginImg"
        Me.picLoginImg.Size = New System.Drawing.Size(337, 415)
        Me.picLoginImg.TabIndex = 512
        Me.picLoginImg.TabStop = False
        '
        'lblAdmPwd
        '
        Me.lblAdmPwd.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.lblAdmPwd.BackColor = System.Drawing.Color.Transparent
        Me.lblAdmPwd.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAdmPwd.ForeColor = System.Drawing.Color.FromArgb(CType(CType(2, Byte), Integer), CType(CType(36, Byte), Integer), CType(CType(79, Byte), Integer))
        Me.lblAdmPwd.Location = New System.Drawing.Point(14, 209)
        Me.lblAdmPwd.Name = "lblAdmPwd"
        Me.lblAdmPwd.Size = New System.Drawing.Size(180, 19)
        Me.lblAdmPwd.TabIndex = 516
        Me.lblAdmPwd.Text = "Additional ADMIN Password"
        Me.lblAdmPwd.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtAdmPwd
        '
        Me.txtAdmPwd.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.txtAdmPwd.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAdmPwd.Location = New System.Drawing.Point(14, 232)
        Me.txtAdmPwd.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtAdmPwd.Name = "txtAdmPwd"
        Me.txtAdmPwd.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtAdmPwd.Size = New System.Drawing.Size(302, 25)
        Me.txtAdmPwd.TabIndex = 515
        '
        'frmLogin
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 17.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoSize = True
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(233, Byte), Integer), CType(CType(237, Byte), Integer), CType(CType(241, Byte), Integer))
        Me.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.ClientSize = New System.Drawing.Size(957, 511)
        Me.Controls.Add(Me.picLoginImg)
        Me.Controls.Add(Me.pnlLogin)
        Me.Controls.Add(Me.pnlCnfPwd)
        Me.Controls.Add(Me.chkRememberMe)
        Me.Controls.Add(Me.lblExpiry)
        Me.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.MaximizeBox = False
        Me.MaximumSize = New System.Drawing.Size(973, 550)
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(973, 550)
        Me.Name = "frmLogin"
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Login"
        Me.TopMost = True
        Me.pnlCnfPwd.ResumeLayout(False)
        Me.pnlCnfPwd.PerformLayout()
        Me.PnlPwdRules.ResumeLayout(False)
        Me.PnlPwdRules.PerformLayout()
        CType(Me.PicUser, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Err, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlLogin.ResumeLayout(False)
        Me.pnlLogin.PerformLayout()
        CType(Me.imgEye, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.picLoginImg, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnOk As System.Windows.Forms.Button
    Friend WithEvents btnChgPwd As System.Windows.Forms.Button
    Friend WithEvents txtOldPwd As System.Windows.Forms.TextBox
    Friend WithEvents txtNewPwd As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents btnCnfOk As System.Windows.Forms.Button
    Friend WithEvents btnCnfCancel As System.Windows.Forms.Button
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents txtCnfNewPwd As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents pnlCnfPwd As System.Windows.Forms.Panel
    Friend WithEvents PicUser As PictureBox
    Friend WithEvents chkRememberMe As CheckBox
    Friend WithEvents lblFgtPwd As LinkLabel
    Friend WithEvents Err As ErrorProvider
    Friend WithEvents pnlLogin As Panel
    Friend WithEvents picLoginImg As PictureBox
    Friend WithEvents lblExpiry As Label
    Friend WithEvents lblMask As Label
    Friend WithEvents tt As ToolTip
    Friend WithEvents imgEye As PictureBox
    Friend WithEvents PnlPwdRules As Panel
    Friend WithEvents Label5 As Label
    Friend WithEvents Label11 As Label
    Friend WithEvents lblMinLen As Label
    Friend WithEvents lblMinLCase As Label
    Friend WithEvents lblMinUCase As Label
    Friend WithEvents lblMinAlpha As Label
    Friend WithEvents lblMinNum As Label
    Friend WithEvents lblMinSplChars As Label
    Friend WithEvents lblAdmPwd As Label
    Friend WithEvents txtAdmPwd As TextBox
End Class
