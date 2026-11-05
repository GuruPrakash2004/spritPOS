<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmUtlFormColourSettings
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
        Dim TreeNode1 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("Dashboard")
        Dim TreeNode2 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("Masters")
        Dim TreeNode3 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("Transactions")
        Dim TreeNode4 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("Reports")
        Dim TreeNode5 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("Utilities")
        Dim TreeNode6 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("Help")
        Dim TreeNode7 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("About")
        Dim TreeNode8 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("Exit")
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmUtlFormColourSettings))
        Me.lblfsize = New System.Windows.Forms.Label()
        Me.lblfname = New System.Windows.Forms.Label()
        Me.lblmods = New System.Windows.Forms.Label()
        Me.Panel3 = New System.Windows.Forms.Panel()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.btnCloseq = New vLQMS.CustomButton.CustomButton()
        Me.btnFormClrSave = New vLQMS.CustomButton.CustomButton()
        Me.btnDefault = New vLQMS.CustomButton.CustomButton()
        Me.grpChild = New System.Windows.Forms.GroupBox()
        Me.pnlChild = New System.Windows.Forms.Panel()
        Me.TextBox3 = New System.Windows.Forms.TextBox()
        Me.lblOptional = New System.Windows.Forms.Label()
        Me.TextBox2 = New System.Windows.Forms.TextBox()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.lblReadonly = New System.Windows.Forms.Label()
        Me.lblMandtory = New System.Windows.Forms.Label()
        Me.Panel4 = New System.Windows.Forms.Panel()
        Me.gbSlide = New System.Windows.Forms.GroupBox()
        Me.tbFont = New System.Windows.Forms.TrackBar()
        Me.lblChildReadFore = New System.Windows.Forms.Label()
        Me.lblChildOptionalFore = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.lblChildMandatoryFore = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.lblChildBack = New System.Windows.Forms.Label()
        Me.lblChildMandtory = New System.Windows.Forms.Label()
        Me.lblAltBClr = New System.Windows.Forms.Label()
        Me.grpMain = New System.Windows.Forms.GroupBox()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.lblMainFont = New System.Windows.Forms.Label()
        Me.lblNormFClr = New System.Windows.Forms.Label()
        Me.lblNormBClr = New System.Windows.Forms.Label()
        Me.lblMainBack = New System.Windows.Forms.Label()
        Me.lblMainFore = New System.Windows.Forms.Label()
        Me.pnlMain = New System.Windows.Forms.Panel()
        Me.pnlLogin = New System.Windows.Forms.Panel()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.lblFin = New System.Windows.Forms.Label()
        Me.lblComp = New System.Windows.Forms.Label()
        Me.btLock = New System.Windows.Forms.Button()
        Me.picLogin = New System.Windows.Forms.PictureBox()
        Me.lblUsername = New System.Windows.Forms.Label()
        Me.tvMain = New System.Windows.Forms.TreeView()
        Me.pnlTlTip = New System.Windows.Forms.Panel()
        Me.PictureBox3 = New System.Windows.Forms.PictureBox()
        Me.lblToolTip = New System.Windows.Forms.Label()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.Panel3.SuspendLayout()
        Me.grpChild.SuspendLayout()
        Me.pnlChild.SuspendLayout()
        Me.Panel4.SuspendLayout()
        Me.gbSlide.SuspendLayout()
        CType(Me.tbFont, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpMain.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.pnlMain.SuspendLayout()
        Me.pnlLogin.SuspendLayout()
        Me.Panel2.SuspendLayout()
        CType(Me.picLogin, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlTlTip.SuspendLayout()
        CType(Me.PictureBox3, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lblfsize
        '
        Me.lblfsize.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblfsize.AutoSize = True
        Me.lblfsize.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblfsize.Location = New System.Drawing.Point(12, 560)
        Me.lblfsize.Name = "lblfsize"
        Me.lblfsize.Size = New System.Drawing.Size(0, 16)
        Me.lblfsize.TabIndex = 13
        '
        'lblfname
        '
        Me.lblfname.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblfname.AutoSize = True
        Me.lblfname.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblfname.Location = New System.Drawing.Point(12, 538)
        Me.lblfname.Name = "lblfname"
        Me.lblfname.Size = New System.Drawing.Size(0, 16)
        Me.lblfname.TabIndex = 14
        '
        'lblmods
        '
        Me.lblmods.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblmods.AutoSize = True
        Me.lblmods.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblmods.Location = New System.Drawing.Point(12, 580)
        Me.lblmods.Name = "lblmods"
        Me.lblmods.Size = New System.Drawing.Size(0, 16)
        Me.lblmods.TabIndex = 15
        '
        'Panel3
        '
        Me.Panel3.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel3.BackColor = System.Drawing.Color.FromArgb(CType(CType(246, Byte), Integer), CType(CType(248, Byte), Integer), CType(CType(252, Byte), Integer))
        Me.Panel3.Controls.Add(Me.btnClose)
        Me.Panel3.Controls.Add(Me.btnSave)
        Me.Panel3.Controls.Add(Me.btnCloseq)
        Me.Panel3.Controls.Add(Me.btnFormClrSave)
        Me.Panel3.Controls.Add(Me.btnDefault)
        Me.Panel3.Location = New System.Drawing.Point(25, 551)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(730, 41)
        Me.Panel3.TabIndex = 52
        '
        'btnClose
        '
        Me.btnClose.AutoSize = True
        Me.btnClose.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_CLOSE
        Me.btnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnClose.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnClose.Location = New System.Drawing.Point(639, 3)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(88, 35)
        Me.btnClose.TabIndex = 105
        Me.btnClose.Text = "&Close"
        Me.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnClose, "Shortcut : Alt + C")
        Me.btnClose.UseVisualStyleBackColor = True
        '
        'btnSave
        '
        Me.btnSave.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSave.BackColor = System.Drawing.Color.Transparent
        Me.btnSave.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_SAVE
        Me.btnSave.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSave.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSave.Location = New System.Drawing.Point(545, 3)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(88, 35)
        Me.btnSave.TabIndex = 83
        Me.btnSave.Text = "&Save"
        Me.btnSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnSave, "Shortcut : Alt + S")
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'btnCloseq
        '
        Me.btnCloseq.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCloseq.BackColor = System.Drawing.Color.Transparent
        Me.btnCloseq.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_CLOSE
        Me.btnCloseq.Cursor = System.Windows.Forms.Cursors.Arrow
        Me.btnCloseq.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnCloseq.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Bold)
        Me.btnCloseq.ForeColor = System.Drawing.Color.White
        Me.btnCloseq.Location = New System.Drawing.Point(152, 5)
        Me.btnCloseq.Margin = New System.Windows.Forms.Padding(4, 1, 4, 1)
        Me.btnCloseq.Name = "btnCloseq"
        Me.btnCloseq.Padding = New System.Windows.Forms.Padding(4)
        Me.btnCloseq.Size = New System.Drawing.Size(88, 35)
        Me.btnCloseq.TabIndex = 5
        Me.btnCloseq.Text = "Cl&ose"
        Me.btnCloseq.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnCloseq, "Close : Alt + O")
        Me.btnCloseq.UseVisualStyleBackColor = False
        Me.btnCloseq.Visible = False
        '
        'btnFormClrSave
        '
        Me.btnFormClrSave.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnFormClrSave.BackColor = System.Drawing.Color.Transparent
        Me.btnFormClrSave.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_SAVE
        Me.btnFormClrSave.Cursor = System.Windows.Forms.Cursors.Arrow
        Me.btnFormClrSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnFormClrSave.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Bold)
        Me.btnFormClrSave.ForeColor = System.Drawing.Color.White
        Me.btnFormClrSave.Location = New System.Drawing.Point(56, 3)
        Me.btnFormClrSave.Margin = New System.Windows.Forms.Padding(4, 1, 4, 1)
        Me.btnFormClrSave.Name = "btnFormClrSave"
        Me.btnFormClrSave.Padding = New System.Windows.Forms.Padding(4)
        Me.btnFormClrSave.Size = New System.Drawing.Size(88, 35)
        Me.btnFormClrSave.TabIndex = 4
        Me.btnFormClrSave.Text = "&Save"
        Me.btnFormClrSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnFormClrSave, "Save : Alt + S")
        Me.btnFormClrSave.UseVisualStyleBackColor = False
        Me.btnFormClrSave.Visible = False
        '
        'btnDefault
        '
        Me.btnDefault.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnDefault.BackColor = System.Drawing.Color.Gray
        Me.btnDefault.Cursor = System.Windows.Forms.Cursors.Arrow
        Me.btnDefault.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Bold)
        Me.btnDefault.ForeColor = System.Drawing.Color.White
        Me.btnDefault.Location = New System.Drawing.Point(453, 2)
        Me.btnDefault.Margin = New System.Windows.Forms.Padding(4, 1, 4, 1)
        Me.btnDefault.Name = "btnDefault"
        Me.btnDefault.Padding = New System.Windows.Forms.Padding(4)
        Me.btnDefault.Size = New System.Drawing.Size(88, 35)
        Me.btnDefault.TabIndex = 3
        Me.btnDefault.Text = "&Default"
        Me.ToolTip1.SetToolTip(Me.btnDefault, "Default : Alt + D")
        Me.btnDefault.UseVisualStyleBackColor = False
        '
        'grpChild
        '
        Me.grpChild.Controls.Add(Me.pnlChild)
        Me.grpChild.Controls.Add(Me.Panel4)
        Me.grpChild.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Bold)
        Me.grpChild.Location = New System.Drawing.Point(217, 5)
        Me.grpChild.Name = "grpChild"
        Me.grpChild.Size = New System.Drawing.Size(538, 445)
        Me.grpChild.TabIndex = 55
        Me.grpChild.TabStop = False
        Me.grpChild.Text = "Child Forms"
        '
        'pnlChild
        '
        Me.pnlChild.BackColor = System.Drawing.Color.FromArgb(CType(CType(188, Byte), Integer), CType(CType(205, Byte), Integer), CType(CType(227, Byte), Integer))
        Me.pnlChild.Controls.Add(Me.TextBox3)
        Me.pnlChild.Controls.Add(Me.lblOptional)
        Me.pnlChild.Controls.Add(Me.TextBox2)
        Me.pnlChild.Controls.Add(Me.TextBox1)
        Me.pnlChild.Controls.Add(Me.lblReadonly)
        Me.pnlChild.Controls.Add(Me.lblMandtory)
        Me.pnlChild.Location = New System.Drawing.Point(101, 178)
        Me.pnlChild.Name = "pnlChild"
        Me.pnlChild.Size = New System.Drawing.Size(349, 187)
        Me.pnlChild.TabIndex = 106
        '
        'TextBox3
        '
        Me.TextBox3.Location = New System.Drawing.Point(122, 114)
        Me.TextBox3.Name = "TextBox3"
        Me.TextBox3.ReadOnly = True
        Me.TextBox3.Size = New System.Drawing.Size(212, 20)
        Me.TextBox3.TabIndex = 6
        '
        'lblOptional
        '
        Me.lblOptional.AutoSize = True
        Me.lblOptional.Font = New System.Drawing.Font("Verdana", 12.0!)
        Me.lblOptional.Location = New System.Drawing.Point(10, 115)
        Me.lblOptional.Name = "lblOptional"
        Me.lblOptional.Size = New System.Drawing.Size(78, 18)
        Me.lblOptional.TabIndex = 5
        Me.lblOptional.Text = "Optional"
        Me.lblOptional.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'TextBox2
        '
        Me.TextBox2.Location = New System.Drawing.Point(122, 79)
        Me.TextBox2.Name = "TextBox2"
        Me.TextBox2.ReadOnly = True
        Me.TextBox2.Size = New System.Drawing.Size(212, 20)
        Me.TextBox2.TabIndex = 4
        '
        'TextBox1
        '
        Me.TextBox1.Location = New System.Drawing.Point(123, 43)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.ReadOnly = True
        Me.TextBox1.Size = New System.Drawing.Size(212, 20)
        Me.TextBox1.TabIndex = 3
        '
        'lblReadonly
        '
        Me.lblReadonly.AutoSize = True
        Me.lblReadonly.Font = New System.Drawing.Font("Verdana", 12.0!)
        Me.lblReadonly.Location = New System.Drawing.Point(11, 80)
        Me.lblReadonly.Name = "lblReadonly"
        Me.lblReadonly.Size = New System.Drawing.Size(89, 18)
        Me.lblReadonly.TabIndex = 2
        Me.lblReadonly.Text = "Read only"
        Me.lblReadonly.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblMandtory
        '
        Me.lblMandtory.AutoSize = True
        Me.lblMandtory.Font = New System.Drawing.Font("Verdana", 12.0!)
        Me.lblMandtory.Location = New System.Drawing.Point(10, 44)
        Me.lblMandtory.Name = "lblMandtory"
        Me.lblMandtory.Size = New System.Drawing.Size(93, 18)
        Me.lblMandtory.TabIndex = 1
        Me.lblMandtory.Text = "Mandatory"
        Me.lblMandtory.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Panel4
        '
        Me.Panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel4.Controls.Add(Me.gbSlide)
        Me.Panel4.Controls.Add(Me.lblChildReadFore)
        Me.Panel4.Controls.Add(Me.lblChildOptionalFore)
        Me.Panel4.Controls.Add(Me.Label5)
        Me.Panel4.Controls.Add(Me.lblChildMandatoryFore)
        Me.Panel4.Controls.Add(Me.Label3)
        Me.Panel4.Controls.Add(Me.lblChildBack)
        Me.Panel4.Controls.Add(Me.lblChildMandtory)
        Me.Panel4.Controls.Add(Me.lblAltBClr)
        Me.Panel4.Location = New System.Drawing.Point(13, 16)
        Me.Panel4.Name = "Panel4"
        Me.Panel4.Size = New System.Drawing.Size(519, 95)
        Me.Panel4.TabIndex = 105
        '
        'gbSlide
        '
        Me.gbSlide.Controls.Add(Me.tbFont)
        Me.gbSlide.Location = New System.Drawing.Point(227, 30)
        Me.gbSlide.Name = "gbSlide"
        Me.gbSlide.Size = New System.Drawing.Size(287, 62)
        Me.gbSlide.TabIndex = 48
        Me.gbSlide.TabStop = False
        Me.gbSlide.Text = "Font Size"
        '
        'tbFont
        '
        Me.tbFont.Location = New System.Drawing.Point(5, 14)
        Me.tbFont.Maximum = 12
        Me.tbFont.Minimum = 8
        Me.tbFont.Name = "tbFont"
        Me.tbFont.Size = New System.Drawing.Size(275, 45)
        Me.tbFont.TabIndex = 0
        Me.tbFont.Value = 8
        '
        'lblChildReadFore
        '
        Me.lblChildReadFore.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblChildReadFore.Location = New System.Drawing.Point(159, 37)
        Me.lblChildReadFore.Name = "lblChildReadFore"
        Me.lblChildReadFore.Size = New System.Drawing.Size(32, 15)
        Me.lblChildReadFore.TabIndex = 46
        Me.lblChildReadFore.Tag = "2"
        Me.lblChildReadFore.Text = "     "
        '
        'lblChildOptionalFore
        '
        Me.lblChildOptionalFore.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblChildOptionalFore.Location = New System.Drawing.Point(159, 65)
        Me.lblChildOptionalFore.Name = "lblChildOptionalFore"
        Me.lblChildOptionalFore.Size = New System.Drawing.Size(32, 15)
        Me.lblChildOptionalFore.TabIndex = 46
        Me.lblChildOptionalFore.Tag = "2"
        Me.lblChildOptionalFore.Text = "     "
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(5, 37)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(151, 13)
        Me.Label5.TabIndex = 0
        Me.Label5.Text = "Read only Fore Colour"
        '
        'lblChildMandatoryFore
        '
        Me.lblChildMandatoryFore.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblChildMandatoryFore.Location = New System.Drawing.Point(159, 7)
        Me.lblChildMandatoryFore.Name = "lblChildMandatoryFore"
        Me.lblChildMandatoryFore.Size = New System.Drawing.Size(32, 15)
        Me.lblChildMandatoryFore.TabIndex = 46
        Me.lblChildMandatoryFore.Tag = "2"
        Me.lblChildMandatoryFore.Text = "     "
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(5, 65)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(141, 13)
        Me.Label3.TabIndex = 0
        Me.Label3.Text = "Optional Fore Colour"
        '
        'lblChildBack
        '
        Me.lblChildBack.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblChildBack.Location = New System.Drawing.Point(323, 8)
        Me.lblChildBack.Name = "lblChildBack"
        Me.lblChildBack.Size = New System.Drawing.Size(32, 15)
        Me.lblChildBack.TabIndex = 47
        Me.lblChildBack.Tag = "3"
        Me.lblChildBack.Text = "     "
        '
        'lblChildMandtory
        '
        Me.lblChildMandtory.AutoSize = True
        Me.lblChildMandtory.Location = New System.Drawing.Point(5, 9)
        Me.lblChildMandtory.Name = "lblChildMandtory"
        Me.lblChildMandtory.Size = New System.Drawing.Size(148, 13)
        Me.lblChildMandtory.TabIndex = 0
        Me.lblChildMandtory.Text = "Mandtory Fore Colour"
        '
        'lblAltBClr
        '
        Me.lblAltBClr.AutoSize = True
        Me.lblAltBClr.Location = New System.Drawing.Point(224, 9)
        Me.lblAltBClr.Name = "lblAltBClr"
        Me.lblAltBClr.Size = New System.Drawing.Size(84, 13)
        Me.lblAltBClr.TabIndex = 1
        Me.lblAltBClr.Text = "Back Colour"
        '
        'grpMain
        '
        Me.grpMain.Controls.Add(Me.Panel1)
        Me.grpMain.Controls.Add(Me.pnlMain)
        Me.grpMain.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Bold)
        Me.grpMain.Location = New System.Drawing.Point(12, 5)
        Me.grpMain.Name = "grpMain"
        Me.grpMain.Size = New System.Drawing.Size(199, 461)
        Me.grpMain.TabIndex = 54
        Me.grpMain.TabStop = False
        Me.grpMain.Text = "Main Form"
        '
        'Panel1
        '
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.lblMainFont)
        Me.Panel1.Controls.Add(Me.lblNormFClr)
        Me.Panel1.Controls.Add(Me.lblNormBClr)
        Me.Panel1.Controls.Add(Me.lblMainBack)
        Me.Panel1.Controls.Add(Me.lblMainFore)
        Me.Panel1.Location = New System.Drawing.Point(13, 16)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(173, 95)
        Me.Panel1.TabIndex = 104
        '
        'lblMainFont
        '
        Me.lblMainFont.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblMainFont.Cursor = System.Windows.Forms.Cursors.Hand
        Me.lblMainFont.Location = New System.Drawing.Point(6, 58)
        Me.lblMainFont.Name = "lblMainFont"
        Me.lblMainFont.Size = New System.Drawing.Size(155, 26)
        Me.lblMainFont.TabIndex = 46
        Me.lblMainFont.Text = "Font"
        Me.lblMainFont.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblNormFClr
        '
        Me.lblNormFClr.AutoSize = True
        Me.lblNormFClr.Location = New System.Drawing.Point(3, 9)
        Me.lblNormFClr.Name = "lblNormFClr"
        Me.lblNormFClr.Size = New System.Drawing.Size(83, 13)
        Me.lblNormFClr.TabIndex = 0
        Me.lblNormFClr.Text = "Fore Colour"
        '
        'lblNormBClr
        '
        Me.lblNormBClr.AutoSize = True
        Me.lblNormBClr.Location = New System.Drawing.Point(3, 37)
        Me.lblNormBClr.Name = "lblNormBClr"
        Me.lblNormBClr.Size = New System.Drawing.Size(84, 13)
        Me.lblNormBClr.TabIndex = 1
        Me.lblNormBClr.Text = "Back Colour"
        '
        'lblMainBack
        '
        Me.lblMainBack.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblMainBack.Location = New System.Drawing.Point(107, 36)
        Me.lblMainBack.Name = "lblMainBack"
        Me.lblMainBack.Size = New System.Drawing.Size(32, 15)
        Me.lblMainBack.TabIndex = 45
        Me.lblMainBack.Tag = "1"
        Me.lblMainBack.Text = "     "
        '
        'lblMainFore
        '
        Me.lblMainFore.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblMainFore.Location = New System.Drawing.Point(107, 8)
        Me.lblMainFore.Name = "lblMainFore"
        Me.lblMainFore.Size = New System.Drawing.Size(32, 15)
        Me.lblMainFore.TabIndex = 44
        Me.lblMainFore.Tag = "0"
        Me.lblMainFore.Text = "     "
        '
        'pnlMain
        '
        Me.pnlMain.Controls.Add(Me.pnlLogin)
        Me.pnlMain.Controls.Add(Me.tvMain)
        Me.pnlMain.Location = New System.Drawing.Point(10, 117)
        Me.pnlMain.Name = "pnlMain"
        Me.pnlMain.Size = New System.Drawing.Size(179, 344)
        Me.pnlMain.TabIndex = 103
        '
        'pnlLogin
        '
        Me.pnlLogin.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnlLogin.BackColor = System.Drawing.Color.FromArgb(CType(CType(43, Byte), Integer), CType(CType(43, Byte), Integer), CType(CType(43, Byte), Integer))
        Me.pnlLogin.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pnlLogin.Controls.Add(Me.Panel2)
        Me.pnlLogin.Controls.Add(Me.btLock)
        Me.pnlLogin.Controls.Add(Me.picLogin)
        Me.pnlLogin.Controls.Add(Me.lblUsername)
        Me.pnlLogin.ForeColor = System.Drawing.Color.Black
        Me.pnlLogin.Location = New System.Drawing.Point(3, 3)
        Me.pnlLogin.Name = "pnlLogin"
        Me.pnlLogin.Size = New System.Drawing.Size(173, 118)
        Me.pnlLogin.TabIndex = 28
        '
        'Panel2
        '
        Me.Panel2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel2.BackColor = System.Drawing.Color.FromArgb(CType(CType(53, Byte), Integer), CType(CType(126, Byte), Integer), CType(CType(166, Byte), Integer))
        Me.Panel2.Controls.Add(Me.lblFin)
        Me.Panel2.Controls.Add(Me.lblComp)
        Me.Panel2.Location = New System.Drawing.Point(5, 56)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(160, 55)
        Me.Panel2.TabIndex = 9
        '
        'lblFin
        '
        Me.lblFin.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblFin.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFin.ForeColor = System.Drawing.Color.White
        Me.lblFin.Location = New System.Drawing.Point(5, 30)
        Me.lblFin.Name = "lblFin"
        Me.lblFin.Size = New System.Drawing.Size(150, 17)
        Me.lblFin.TabIndex = 1
        Me.lblFin.Text = "compName"
        Me.lblFin.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblComp
        '
        Me.lblComp.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblComp.ForeColor = System.Drawing.Color.White
        Me.lblComp.Location = New System.Drawing.Point(5, 13)
        Me.lblComp.Name = "lblComp"
        Me.lblComp.Size = New System.Drawing.Size(150, 17)
        Me.lblComp.TabIndex = 0
        Me.lblComp.Text = "compName"
        Me.lblComp.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'btLock
        '
        Me.btLock.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btLock.BackColor = System.Drawing.Color.Transparent
        Me.btLock.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.btLock.FlatAppearance.BorderSize = 0
        Me.btLock.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btLock.Location = New System.Drawing.Point(150, 4)
        Me.btLock.Name = "btLock"
        Me.btLock.Size = New System.Drawing.Size(17, 16)
        Me.btLock.TabIndex = 2
        Me.btLock.UseVisualStyleBackColor = False
        '
        'picLogin
        '
        Me.picLogin.BackColor = System.Drawing.Color.Transparent
        Me.picLogin.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.picLogin.Location = New System.Drawing.Point(4, 4)
        Me.picLogin.MaximumSize = New System.Drawing.Size(100, 100)
        Me.picLogin.Name = "picLogin"
        Me.picLogin.Size = New System.Drawing.Size(52, 44)
        Me.picLogin.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.picLogin.TabIndex = 0
        Me.picLogin.TabStop = False
        '
        'lblUsername
        '
        Me.lblUsername.AutoSize = True
        Me.lblUsername.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblUsername.ForeColor = System.Drawing.Color.White
        Me.lblUsername.Location = New System.Drawing.Point(62, 31)
        Me.lblUsername.Name = "lblUsername"
        Me.lblUsername.Size = New System.Drawing.Size(37, 13)
        Me.lblUsername.TabIndex = 1
        Me.lblUsername.Text = "Login"
        '
        'tvMain
        '
        Me.tvMain.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tvMain.BackColor = System.Drawing.Color.FromArgb(CType(CType(43, Byte), Integer), CType(CType(43, Byte), Integer), CType(CType(43, Byte), Integer))
        Me.tvMain.Font = New System.Drawing.Font("Segoe UI", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tvMain.ForeColor = System.Drawing.Color.White
        Me.tvMain.Location = New System.Drawing.Point(3, 122)
        Me.tvMain.Name = "tvMain"
        TreeNode1.Name = "mnuDashboard"
        TreeNode1.Text = "Dashboard"
        TreeNode2.Name = "mnuMaster"
        TreeNode2.Text = "Masters"
        TreeNode3.ImageKey = "transactions.png"
        TreeNode3.Name = "mnuTransactions"
        TreeNode3.SelectedImageKey = "transactions.png"
        TreeNode3.Text = "Transactions"
        TreeNode4.ImageKey = "reports.png"
        TreeNode4.Name = "mnuReports"
        TreeNode4.SelectedImageKey = "reports.png"
        TreeNode4.Text = "Reports"
        TreeNode5.ImageKey = "utilities.png"
        TreeNode5.Name = "mnuUtilities"
        TreeNode5.SelectedImageKey = "utilities.png"
        TreeNode5.Text = "Utilities"
        TreeNode6.Name = "mnuHelp"
        TreeNode6.Text = "Help"
        TreeNode7.Name = "mnuAbout"
        TreeNode7.Text = "About"
        TreeNode8.ImageKey = "Exit.png"
        TreeNode8.Name = "Node0"
        TreeNode8.Text = "Exit"
        Me.tvMain.Nodes.AddRange(New System.Windows.Forms.TreeNode() {TreeNode1, TreeNode2, TreeNode3, TreeNode4, TreeNode5, TreeNode6, TreeNode7, TreeNode8})
        Me.tvMain.ShowLines = False
        Me.tvMain.Size = New System.Drawing.Size(173, 216)
        Me.tvMain.TabIndex = 1
        '
        'pnlTlTip
        '
        Me.pnlTlTip.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnlTlTip.BackColor = System.Drawing.Color.FromArgb(CType(CType(52, Byte), Integer), CType(CType(68, Byte), Integer), CType(CType(88, Byte), Integer))
        Me.pnlTlTip.Controls.Add(Me.PictureBox3)
        Me.pnlTlTip.Controls.Add(Me.lblToolTip)
        Me.pnlTlTip.Location = New System.Drawing.Point(12, 472)
        Me.pnlTlTip.Name = "pnlTlTip"
        Me.pnlTlTip.Size = New System.Drawing.Size(743, 73)
        Me.pnlTlTip.TabIndex = 102
        '
        'PictureBox3
        '
        Me.PictureBox3.BackColor = System.Drawing.Color.Transparent
        Me.PictureBox3.BackgroundImage = CType(resources.GetObject("PictureBox3.BackgroundImage"), System.Drawing.Image)
        Me.PictureBox3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.PictureBox3.Location = New System.Drawing.Point(0, 0)
        Me.PictureBox3.Name = "PictureBox3"
        Me.PictureBox3.Size = New System.Drawing.Size(45, 40)
        Me.PictureBox3.TabIndex = 219
        Me.PictureBox3.TabStop = False
        '
        'lblToolTip
        '
        Me.lblToolTip.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblToolTip.BackColor = System.Drawing.Color.Transparent
        Me.lblToolTip.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblToolTip.ForeColor = System.Drawing.Color.White
        Me.lblToolTip.Location = New System.Drawing.Point(46, 4)
        Me.lblToolTip.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblToolTip.Name = "lblToolTip"
        Me.lblToolTip.Size = New System.Drawing.Size(516, 64)
        Me.lblToolTip.TabIndex = 216
        Me.lblToolTip.Text = "Help Tips:"
        '
        'frmUtlFormColourSettings
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(771, 605)
        Me.Controls.Add(Me.pnlTlTip)
        Me.Controls.Add(Me.Panel3)
        Me.Controls.Add(Me.grpChild)
        Me.Controls.Add(Me.grpMain)
        Me.Controls.Add(Me.lblmods)
        Me.Controls.Add(Me.lblfname)
        Me.Controls.Add(Me.lblfsize)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.MinimumSize = New System.Drawing.Size(530, 583)
        Me.Name = "frmUtlFormColourSettings"
        Me.ShowIcon = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Form Colour Settings"
        Me.Panel3.ResumeLayout(False)
        Me.Panel3.PerformLayout()
        Me.grpChild.ResumeLayout(False)
        Me.pnlChild.ResumeLayout(False)
        Me.pnlChild.PerformLayout()
        Me.Panel4.ResumeLayout(False)
        Me.Panel4.PerformLayout()
        Me.gbSlide.ResumeLayout(False)
        Me.gbSlide.PerformLayout()
        CType(Me.tbFont, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpMain.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.pnlMain.ResumeLayout(False)
        Me.pnlLogin.ResumeLayout(False)
        Me.pnlLogin.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        CType(Me.picLogin, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlTlTip.ResumeLayout(False)
        CType(Me.PictureBox3, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lblfsize As System.Windows.Forms.Label
    Friend WithEvents lblfname As System.Windows.Forms.Label
    Friend WithEvents lblmods As System.Windows.Forms.Label
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
    Friend WithEvents btnCloseq As vLQMS.CustomButton.CustomButton
    Friend WithEvents btnFormClrSave As vLQMS.CustomButton.CustomButton
    Friend WithEvents btnDefault As vLQMS.CustomButton.CustomButton
    Friend WithEvents grpChild As System.Windows.Forms.GroupBox
    Friend WithEvents lblChildBack As System.Windows.Forms.Label
    Friend WithEvents lblChildMandatoryFore As System.Windows.Forms.Label
    Friend WithEvents lblAltBClr As System.Windows.Forms.Label
    Friend WithEvents lblChildMandtory As System.Windows.Forms.Label
    Friend WithEvents grpMain As System.Windows.Forms.GroupBox
    Friend WithEvents lblMainBack As System.Windows.Forms.Label
    Friend WithEvents lblMainFore As System.Windows.Forms.Label
    Friend WithEvents lblNormBClr As System.Windows.Forms.Label
    Friend WithEvents lblNormFClr As System.Windows.Forms.Label
    Friend WithEvents pnlTlTip As System.Windows.Forms.Panel
    Friend WithEvents lblToolTip As System.Windows.Forms.Label
    Friend WithEvents PictureBox3 As System.Windows.Forms.PictureBox
    Friend WithEvents pnlMain As Panel
    Friend WithEvents tvMain As TreeView
    Private WithEvents pnlLogin As Panel
    Friend WithEvents Panel2 As Panel
    Friend WithEvents lblFin As Label
    Friend WithEvents lblComp As Label
    Private WithEvents btLock As Button
    Private WithEvents picLogin As PictureBox
    Private WithEvents lblUsername As Label
    Friend WithEvents Panel1 As Panel
    Friend WithEvents Panel4 As Panel
    Friend WithEvents pnlChild As Panel
    Friend WithEvents lblReadonly As Label
    Friend WithEvents lblMandtory As Label
    Friend WithEvents TextBox2 As TextBox
    Friend WithEvents TextBox1 As TextBox
    Friend WithEvents TextBox3 As TextBox
    Friend WithEvents lblOptional As Label
    Friend WithEvents lblMainFont As Label
    Friend WithEvents lblChildReadFore As Label
    Friend WithEvents lblChildOptionalFore As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents gbSlide As GroupBox
    Friend WithEvents tbFont As TrackBar
    Friend WithEvents ToolTip1 As ToolTip
    Friend WithEvents btnSave As Button
    Friend WithEvents btnClose As Button
End Class
