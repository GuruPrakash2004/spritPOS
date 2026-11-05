<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmUtlLoginSettings
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
        Me.components = New System.ComponentModel.Container()
        Me.gbADType = New System.Windows.Forms.GroupBox()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.pnlDomain = New System.Windows.Forms.Panel()
        Me.txtADDomain = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.opADReg = New System.Windows.Forms.RadioButton()
        Me.opADAuto = New System.Windows.Forms.RadioButton()
        Me.pnlPwdRules = New System.Windows.Forms.Panel()
        Me.txtLCase = New System.Windows.Forms.TextBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.txtUCase = New System.Windows.Forms.TextBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.txtPwdLen = New System.Windows.Forms.TextBox()
        Me.txtAlpha = New System.Windows.Forms.TextBox()
        Me.txtNos = New System.Windows.Forms.TextBox()
        Me.txtSplChrs = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.PnlAuth = New System.Windows.Forms.Panel()
        Me.GrpAut = New System.Windows.Forms.GroupBox()
        Me.opReg = New System.Windows.Forms.RadioButton()
        Me.opAD = New System.Windows.Forms.RadioButton()
        Me.chkPwd = New System.Windows.Forms.CheckBox()
        Me.chkRemember = New System.Windows.Forms.CheckBox()
        Me.grpRights = New System.Windows.Forms.GroupBox()
        Me.opUserBased = New System.Windows.Forms.RadioButton()
        Me.opRoleBased = New System.Windows.Forms.RadioButton()
        Me.gbPwd = New System.Windows.Forms.GroupBox()
        Me.Panel3 = New System.Windows.Forms.Panel()
        Me.opNeverExpires = New System.Windows.Forms.RadioButton()
        Me.opExpiryDays = New System.Windows.Forms.RadioButton()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtExpDays = New System.Windows.Forms.TextBox()
        Me.opNoRules = New System.Windows.Forms.RadioButton()
        Me.opPwdRules = New System.Windows.Forms.RadioButton()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.chkLoginReq = New System.Windows.Forms.CheckBox()
        Me.gbADType.SuspendLayout()
        Me.pnlDomain.SuspendLayout()
        Me.pnlPwdRules.SuspendLayout()
        Me.PnlAuth.SuspendLayout()
        Me.GrpAut.SuspendLayout()
        Me.grpRights.SuspendLayout()
        Me.gbPwd.SuspendLayout()
        Me.Panel3.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.SuspendLayout()
        '
        'gbADType
        '
        Me.gbADType.Controls.Add(Me.Label10)
        Me.gbADType.Controls.Add(Me.Label7)
        Me.gbADType.Controls.Add(Me.pnlDomain)
        Me.gbADType.Controls.Add(Me.opADReg)
        Me.gbADType.Controls.Add(Me.opADAuto)
        Me.gbADType.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.gbADType.Location = New System.Drawing.Point(262, 44)
        Me.gbADType.Name = "gbADType"
        Me.gbADType.Size = New System.Drawing.Size(367, 264)
        Me.gbADType.TabIndex = 2
        Me.gbADType.TabStop = False
        Me.gbADType.Text = "Type of Login"
        Me.gbADType.Visible = False
        '
        'Label10
        '
        Me.Label10.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.ForeColor = System.Drawing.Color.Blue
        Me.Label10.Location = New System.Drawing.Point(65, 120)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(298, 53)
        Me.Label10.TabIndex = 6
        Me.Label10.Tag = "R"
        Me.Label10.Text = "During opening of the software, Login screen will be shown for the user to enter " &
    "UserName and Password."
        '
        'Label7
        '
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.ForeColor = System.Drawing.Color.Blue
        Me.Label7.Location = New System.Drawing.Point(65, 58)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(295, 36)
        Me.Label7.TabIndex = 5
        Me.Label7.Tag = "R"
        Me.Label7.Text = "During opening of the software, will not show login screen. Will take user as the" &
    " windows user  only."
        '
        'pnlDomain
        '
        Me.pnlDomain.Controls.Add(Me.txtADDomain)
        Me.pnlDomain.Controls.Add(Me.Label1)
        Me.pnlDomain.Location = New System.Drawing.Point(68, 178)
        Me.pnlDomain.Name = "pnlDomain"
        Me.pnlDomain.Size = New System.Drawing.Size(289, 74)
        Me.pnlDomain.TabIndex = 4
        Me.pnlDomain.Visible = False
        '
        'txtADDomain
        '
        Me.txtADDomain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtADDomain.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtADDomain.Location = New System.Drawing.Point(3, 38)
        Me.txtADDomain.MaxLength = 50
        Me.txtADDomain.Name = "txtADDomain"
        Me.txtADDomain.Size = New System.Drawing.Size(280, 26)
        Me.txtADDomain.TabIndex = 2
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(3, 9)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(127, 20)
        Me.Label1.TabIndex = 3
        Me.Label1.Text = "Domain Settings"
        '
        'opADReg
        '
        Me.opADReg.AutoSize = True
        Me.opADReg.Location = New System.Drawing.Point(44, 97)
        Me.opADReg.Name = "opADReg"
        Me.opADReg.Size = New System.Drawing.Size(126, 24)
        Me.opADReg.TabIndex = 1
        Me.opADReg.TabStop = True
        Me.opADReg.Text = "Regular Login"
        Me.opADReg.UseVisualStyleBackColor = True
        '
        'opADAuto
        '
        Me.opADAuto.AutoSize = True
        Me.opADAuto.Location = New System.Drawing.Point(44, 33)
        Me.opADAuto.Name = "opADAuto"
        Me.opADAuto.Size = New System.Drawing.Size(104, 24)
        Me.opADAuto.TabIndex = 0
        Me.opADAuto.TabStop = True
        Me.opADAuto.Text = "Auto Login"
        Me.opADAuto.UseVisualStyleBackColor = True
        '
        'pnlPwdRules
        '
        Me.pnlPwdRules.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlPwdRules.Controls.Add(Me.txtLCase)
        Me.pnlPwdRules.Controls.Add(Me.Label9)
        Me.pnlPwdRules.Controls.Add(Me.txtUCase)
        Me.pnlPwdRules.Controls.Add(Me.Label8)
        Me.pnlPwdRules.Controls.Add(Me.txtPwdLen)
        Me.pnlPwdRules.Controls.Add(Me.txtAlpha)
        Me.pnlPwdRules.Controls.Add(Me.txtNos)
        Me.pnlPwdRules.Controls.Add(Me.txtSplChrs)
        Me.pnlPwdRules.Controls.Add(Me.Label6)
        Me.pnlPwdRules.Controls.Add(Me.Label5)
        Me.pnlPwdRules.Controls.Add(Me.Label4)
        Me.pnlPwdRules.Controls.Add(Me.Label3)
        Me.pnlPwdRules.Location = New System.Drawing.Point(7, 107)
        Me.pnlPwdRules.Name = "pnlPwdRules"
        Me.pnlPwdRules.Size = New System.Drawing.Size(360, 157)
        Me.pnlPwdRules.TabIndex = 4
        Me.pnlPwdRules.Visible = False
        '
        'txtLCase
        '
        Me.txtLCase.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtLCase.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtLCase.Location = New System.Drawing.Point(251, 106)
        Me.txtLCase.MaxLength = 2
        Me.txtLCase.Name = "txtLCase"
        Me.txtLCase.Size = New System.Drawing.Size(58, 26)
        Me.txtLCase.TabIndex = 12
        Me.txtLCase.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(14, 108)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(166, 20)
        Me.Label9.TabIndex = 14
        Me.Label9.Text = "Min. no of Lower Case"
        '
        'txtUCase
        '
        Me.txtUCase.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtUCase.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtUCase.Location = New System.Drawing.Point(251, 80)
        Me.txtUCase.MaxLength = 2
        Me.txtUCase.Name = "txtUCase"
        Me.txtUCase.Size = New System.Drawing.Size(58, 26)
        Me.txtUCase.TabIndex = 11
        Me.txtUCase.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(14, 81)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(167, 20)
        Me.Label8.TabIndex = 12
        Me.Label8.Text = "Min. no of Upper Case"
        '
        'txtPwdLen
        '
        Me.txtPwdLen.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtPwdLen.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPwdLen.Location = New System.Drawing.Point(251, 132)
        Me.txtPwdLen.MaxLength = 2
        Me.txtPwdLen.Name = "txtPwdLen"
        Me.txtPwdLen.Size = New System.Drawing.Size(58, 26)
        Me.txtPwdLen.TabIndex = 13
        Me.txtPwdLen.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtAlpha
        '
        Me.txtAlpha.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtAlpha.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAlpha.Location = New System.Drawing.Point(251, 54)
        Me.txtAlpha.MaxLength = 2
        Me.txtAlpha.Name = "txtAlpha"
        Me.txtAlpha.Size = New System.Drawing.Size(58, 26)
        Me.txtAlpha.TabIndex = 10
        Me.txtAlpha.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtNos
        '
        Me.txtNos.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtNos.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtNos.Location = New System.Drawing.Point(251, 28)
        Me.txtNos.MaxLength = 2
        Me.txtNos.Name = "txtNos"
        Me.txtNos.Size = New System.Drawing.Size(58, 26)
        Me.txtNos.TabIndex = 9
        Me.txtNos.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtSplChrs
        '
        Me.txtSplChrs.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtSplChrs.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSplChrs.Location = New System.Drawing.Point(251, 2)
        Me.txtSplChrs.MaxLength = 2
        Me.txtSplChrs.Name = "txtSplChrs"
        Me.txtSplChrs.Size = New System.Drawing.Size(58, 26)
        Me.txtSplChrs.TabIndex = 8
        Me.txtSplChrs.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.ForeColor = System.Drawing.Color.Red
        Me.Label6.Location = New System.Drawing.Point(14, 134)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(177, 20)
        Me.Label6.TabIndex = 7
        Me.Label6.Tag = "M"
        Me.Label6.Text = "Min. length of Password"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(14, 56)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(154, 20)
        Me.Label5.TabIndex = 6
        Me.Label5.Text = "Min. no of Alphabets"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(14, 30)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(140, 20)
        Me.Label4.TabIndex = 5
        Me.Label4.Text = "Min. no of Numeric"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(14, 5)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(216, 20)
        Me.Label3.TabIndex = 4
        Me.Label3.Text = "Min. no of Special Characters"
        '
        'PnlAuth
        '
        Me.PnlAuth.BackColor = System.Drawing.Color.DimGray
        Me.PnlAuth.Controls.Add(Me.GrpAut)
        Me.PnlAuth.Controls.Add(Me.grpRights)
        Me.PnlAuth.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.PnlAuth.ForeColor = System.Drawing.Color.White
        Me.PnlAuth.Location = New System.Drawing.Point(12, 43)
        Me.PnlAuth.Name = "PnlAuth"
        Me.PnlAuth.Size = New System.Drawing.Size(242, 264)
        Me.PnlAuth.TabIndex = 3
        '
        'GrpAut
        '
        Me.GrpAut.BackColor = System.Drawing.Color.Transparent
        Me.GrpAut.Controls.Add(Me.opReg)
        Me.GrpAut.Controls.Add(Me.opAD)
        Me.GrpAut.Controls.Add(Me.chkPwd)
        Me.GrpAut.Controls.Add(Me.chkRemember)
        Me.GrpAut.ForeColor = System.Drawing.Color.White
        Me.GrpAut.Location = New System.Drawing.Point(6, 1)
        Me.GrpAut.Name = "GrpAut"
        Me.GrpAut.Size = New System.Drawing.Size(228, 131)
        Me.GrpAut.TabIndex = 7
        Me.GrpAut.TabStop = False
        Me.GrpAut.Text = "Authentication"
        '
        'opReg
        '
        Me.opReg.AutoSize = True
        Me.opReg.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.opReg.ForeColor = System.Drawing.Color.White
        Me.opReg.Location = New System.Drawing.Point(29, 47)
        Me.opReg.Name = "opReg"
        Me.opReg.Size = New System.Drawing.Size(198, 24)
        Me.opReg.TabIndex = 7
        Me.opReg.TabStop = True
        Me.opReg.Text = "Software Authentication"
        Me.opReg.UseVisualStyleBackColor = True
        '
        'opAD
        '
        Me.opAD.AutoSize = True
        Me.opAD.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.opAD.ForeColor = System.Drawing.Color.White
        Me.opAD.Location = New System.Drawing.Point(29, 19)
        Me.opAD.Name = "opAD"
        Me.opAD.Size = New System.Drawing.Size(157, 24)
        Me.opAD.TabIndex = 6
        Me.opAD.TabStop = True
        Me.opAD.Text = "AD Authentication"
        Me.opAD.UseVisualStyleBackColor = True
        '
        'chkPwd
        '
        Me.chkPwd.AutoSize = True
        Me.chkPwd.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkPwd.ForeColor = System.Drawing.Color.White
        Me.chkPwd.Location = New System.Drawing.Point(51, 81)
        Me.chkPwd.Name = "chkPwd"
        Me.chkPwd.Size = New System.Drawing.Size(141, 24)
        Me.chkPwd.TabIndex = 3
        Me.chkPwd.Text = "Password Policy"
        Me.chkPwd.UseVisualStyleBackColor = True
        '
        'chkRemember
        '
        Me.chkRemember.AutoSize = True
        Me.chkRemember.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkRemember.ForeColor = System.Drawing.Color.White
        Me.chkRemember.Location = New System.Drawing.Point(51, 80)
        Me.chkRemember.Name = "chkRemember"
        Me.chkRemember.Size = New System.Drawing.Size(177, 24)
        Me.chkRemember.TabIndex = 4
        Me.chkRemember.Text = "Show Remember Me"
        Me.ToolTip1.SetToolTip(Me.chkRemember, "If checked, will display ""[ ] Show Remember me"" check box in login screen.")
        Me.chkRemember.UseVisualStyleBackColor = True
        Me.chkRemember.Visible = False
        '
        'grpRights
        '
        Me.grpRights.Controls.Add(Me.opUserBased)
        Me.grpRights.Controls.Add(Me.opRoleBased)
        Me.grpRights.ForeColor = System.Drawing.Color.White
        Me.grpRights.Location = New System.Drawing.Point(6, 138)
        Me.grpRights.Name = "grpRights"
        Me.grpRights.Size = New System.Drawing.Size(228, 119)
        Me.grpRights.TabIndex = 7
        Me.grpRights.TabStop = False
        Me.grpRights.Text = "Rights Allocation"
        '
        'opUserBased
        '
        Me.opUserBased.AutoSize = True
        Me.opUserBased.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.opUserBased.ForeColor = System.Drawing.Color.White
        Me.opUserBased.Location = New System.Drawing.Point(32, 36)
        Me.opUserBased.Name = "opUserBased"
        Me.opUserBased.Size = New System.Drawing.Size(111, 24)
        Me.opUserBased.TabIndex = 8
        Me.opUserBased.TabStop = True
        Me.opUserBased.Text = "User Based"
        Me.opUserBased.UseVisualStyleBackColor = True
        '
        'opRoleBased
        '
        Me.opRoleBased.AutoSize = True
        Me.opRoleBased.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.opRoleBased.ForeColor = System.Drawing.Color.White
        Me.opRoleBased.Location = New System.Drawing.Point(33, 66)
        Me.opRoleBased.Name = "opRoleBased"
        Me.opRoleBased.Size = New System.Drawing.Size(110, 24)
        Me.opRoleBased.TabIndex = 9
        Me.opRoleBased.TabStop = True
        Me.opRoleBased.Text = "Role Based"
        Me.opRoleBased.UseVisualStyleBackColor = True
        '
        'gbPwd
        '
        Me.gbPwd.Controls.Add(Me.Panel3)
        Me.gbPwd.Controls.Add(Me.opNoRules)
        Me.gbPwd.Controls.Add(Me.opPwdRules)
        Me.gbPwd.Controls.Add(Me.pnlPwdRules)
        Me.gbPwd.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.gbPwd.Location = New System.Drawing.Point(262, 42)
        Me.gbPwd.Name = "gbPwd"
        Me.gbPwd.Size = New System.Drawing.Size(370, 267)
        Me.gbPwd.TabIndex = 6
        Me.gbPwd.TabStop = False
        Me.gbPwd.Text = "Password Policy"
        Me.gbPwd.Visible = False
        '
        'Panel3
        '
        Me.Panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel3.Controls.Add(Me.opNeverExpires)
        Me.Panel3.Controls.Add(Me.opExpiryDays)
        Me.Panel3.Controls.Add(Me.Label2)
        Me.Panel3.Controls.Add(Me.txtExpDays)
        Me.Panel3.Location = New System.Drawing.Point(7, 22)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(360, 60)
        Me.Panel3.TabIndex = 10
        '
        'opNeverExpires
        '
        Me.opNeverExpires.AutoSize = True
        Me.opNeverExpires.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.opNeverExpires.Location = New System.Drawing.Point(8, 3)
        Me.opNeverExpires.Name = "opNeverExpires"
        Me.opNeverExpires.Size = New System.Drawing.Size(124, 24)
        Me.opNeverExpires.TabIndex = 0
        Me.opNeverExpires.TabStop = True
        Me.opNeverExpires.Text = "Never Expires"
        Me.opNeverExpires.UseVisualStyleBackColor = True
        '
        'opExpiryDays
        '
        Me.opExpiryDays.AutoSize = True
        Me.opExpiryDays.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.opExpiryDays.Location = New System.Drawing.Point(8, 26)
        Me.opExpiryDays.Name = "opExpiryDays"
        Me.opExpiryDays.Size = New System.Drawing.Size(122, 24)
        Me.opExpiryDays.TabIndex = 1
        Me.opExpiryDays.TabStop = True
        Me.opExpiryDays.Text = "Expires Every"
        Me.opExpiryDays.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(186, 30)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(45, 20)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "Days"
        '
        'txtExpDays
        '
        Me.txtExpDays.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtExpDays.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtExpDays.Location = New System.Drawing.Point(135, 26)
        Me.txtExpDays.Name = "txtExpDays"
        Me.txtExpDays.Size = New System.Drawing.Size(45, 26)
        Me.txtExpDays.TabIndex = 2
        Me.txtExpDays.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'opNoRules
        '
        Me.opNoRules.AutoSize = True
        Me.opNoRules.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.opNoRules.ForeColor = System.Drawing.Color.Black
        Me.opNoRules.Location = New System.Drawing.Point(238, 83)
        Me.opNoRules.Name = "opNoRules"
        Me.opNoRules.Size = New System.Drawing.Size(100, 24)
        Me.opNoRules.TabIndex = 9
        Me.opNoRules.TabStop = True
        Me.opNoRules.Text = "No Rules"
        Me.opNoRules.UseVisualStyleBackColor = True
        '
        'opPwdRules
        '
        Me.opPwdRules.AutoSize = True
        Me.opPwdRules.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.opPwdRules.ForeColor = System.Drawing.Color.Black
        Me.opPwdRules.Location = New System.Drawing.Point(16, 83)
        Me.opPwdRules.Name = "opPwdRules"
        Me.opPwdRules.Size = New System.Drawing.Size(155, 24)
        Me.opPwdRules.TabIndex = 8
        Me.opPwdRules.TabStop = True
        Me.opPwdRules.Text = "Password Rules"
        Me.opPwdRules.UseVisualStyleBackColor = True
        '
        'btnSave
        '
        Me.btnSave.BackColor = System.Drawing.Color.Transparent
        Me.btnSave.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_SAVE
        Me.btnSave.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSave.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSave.Location = New System.Drawing.Point(460, 315)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(82, 29)
        Me.btnSave.TabIndex = 4
        Me.btnSave.Text = "&Save"
        Me.btnSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnSave, "Save : Alt + S")
        Me.btnSave.UseVisualStyleBackColor = False
        '
        'btnClose
        '
        Me.btnClose.BackColor = System.Drawing.Color.Transparent
        Me.btnClose.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_CLOSE
        Me.btnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnClose.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnClose.Location = New System.Drawing.Point(547, 315)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(82, 29)
        Me.btnClose.TabIndex = 5
        Me.btnClose.Text = "Cl&ose"
        Me.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnClose, "Close : Alt + O")
        Me.btnClose.UseVisualStyleBackColor = False
        '
        'Panel2
        '
        Me.Panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel2.Controls.Add(Me.chkLoginReq)
        Me.Panel2.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Panel2.Location = New System.Drawing.Point(13, 3)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(242, 35)
        Me.Panel2.TabIndex = 7
        '
        'chkLoginReq
        '
        Me.chkLoginReq.AutoSize = True
        Me.chkLoginReq.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkLoginReq.ForeColor = System.Drawing.Color.Black
        Me.chkLoginReq.Location = New System.Drawing.Point(5, 6)
        Me.chkLoginReq.Name = "chkLoginReq"
        Me.chkLoginReq.Size = New System.Drawing.Size(136, 24)
        Me.chkLoginReq.TabIndex = 4
        Me.chkLoginReq.Text = "Login Required"
        Me.chkLoginReq.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.chkLoginReq.UseVisualStyleBackColor = True
        '
        'frmUtlLoginSettings
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoSize = True
        Me.ClientSize = New System.Drawing.Size(634, 348)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.btnSave)
        Me.Controls.Add(Me.PnlAuth)
        Me.Controls.Add(Me.gbPwd)
        Me.Controls.Add(Me.gbADType)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(650, 335)
        Me.Name = "frmUtlLoginSettings"
        Me.ShowIcon = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Login Settings"
        Me.gbADType.ResumeLayout(False)
        Me.gbADType.PerformLayout()
        Me.pnlDomain.ResumeLayout(False)
        Me.pnlDomain.PerformLayout()
        Me.pnlPwdRules.ResumeLayout(False)
        Me.pnlPwdRules.PerformLayout()
        Me.PnlAuth.ResumeLayout(False)
        Me.GrpAut.ResumeLayout(False)
        Me.GrpAut.PerformLayout()
        Me.grpRights.ResumeLayout(False)
        Me.grpRights.PerformLayout()
        Me.gbPwd.ResumeLayout(False)
        Me.gbPwd.PerformLayout()
        Me.Panel3.ResumeLayout(False)
        Me.Panel3.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents gbADType As GroupBox
    Friend WithEvents PnlAuth As Panel
    Friend WithEvents btnSave As Button
    Friend WithEvents btnClose As Button
    Friend WithEvents opADAuto As RadioButton
    Friend WithEvents opADReg As RadioButton
    Friend WithEvents Label1 As Label
    Friend WithEvents txtADDomain As TextBox
    Friend WithEvents pnlDomain As Panel
    Friend WithEvents chkRemember As CheckBox
    Friend WithEvents chkPwd As CheckBox
    Friend WithEvents opAD As RadioButton
    Friend WithEvents opReg As RadioButton
    Friend WithEvents gbPwd As GroupBox
    Friend WithEvents opNeverExpires As RadioButton
    Friend WithEvents opExpiryDays As RadioButton
    Friend WithEvents txtExpDays As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents pnlPwdRules As Panel
    Friend WithEvents opPwdRules As RadioButton
    Friend WithEvents opNoRules As RadioButton
    Friend WithEvents Label3 As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents Label6 As Label
    Friend WithEvents txtPwdLen As TextBox
    Friend WithEvents txtAlpha As TextBox
    Friend WithEvents txtNos As TextBox
    Friend WithEvents txtSplChrs As TextBox
    Friend WithEvents opRoleBased As RadioButton
    Friend WithEvents opUserBased As RadioButton
    Friend WithEvents txtLCase As TextBox
    Friend WithEvents Label9 As Label
    Friend WithEvents txtUCase As TextBox
    Friend WithEvents Label8 As Label
    Friend WithEvents Panel3 As Panel
    Friend WithEvents Label7 As Label
    Friend WithEvents Label10 As Label
    Friend WithEvents ToolTip1 As ToolTip
    Friend WithEvents grpRights As GroupBox
    Friend WithEvents GrpAut As GroupBox
    Friend WithEvents Panel2 As Panel
    Friend WithEvents chkLoginReq As CheckBox
End Class
