<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmMstUSer
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMstUSer))
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.lblFrm = New System.Windows.Forms.Label()
        Me.pnlSearch = New System.Windows.Forms.Panel()
        Me.lblSearchImage = New System.Windows.Forms.Label()
        Me.lblSearch = New System.Windows.Forms.Label()
        Me.txtSearch = New System.Windows.Forms.TextBox()
        Me.txtID = New System.Windows.Forms.TextBox()
        Me.txtCode = New System.Windows.Forms.TextBox()
        Me.lblCode = New System.Windows.Forms.Label()
        Me.vsfSearch = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.Panel3 = New System.Windows.Forms.Panel()
        Me.Panel4 = New System.Windows.Forms.Panel()
        Me.tt = New System.Windows.Forms.ToolTip(Me.components)
        Me.ColorDialog = New System.Windows.Forms.ColorDialog()
        Me.txtNo = New System.Windows.Forms.TextBox()
        Me.txtName = New System.Windows.Forms.TextBox()
        Me.lblName = New System.Windows.Forms.Label()
        Me.pnlCodeName = New System.Windows.Forms.Panel()
        Me.lblApproved = New System.Windows.Forms.Label()
        Me.lblPwdReset = New System.Windows.Forms.Label()
        Me.dtPwdReset = New System.Windows.Forms.DateTimePicker()
        Me.PnlPwdRules = New System.Windows.Forms.Panel()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.lblMinLen = New System.Windows.Forms.Label()
        Me.lblMinLCase = New System.Windows.Forms.Label()
        Me.lblMinUCase = New System.Windows.Forms.Label()
        Me.lblMinAlpha = New System.Windows.Forms.Label()
        Me.lblMinNum = New System.Windows.Forms.Label()
        Me.lblMinSplChars = New System.Windows.Forms.Label()
        Me.pnlPwd = New System.Windows.Forms.Panel()
        Me.imgEye = New System.Windows.Forms.PictureBox()
        Me.txtCnfPassword = New System.Windows.Forms.TextBox()
        Me.txtPassword = New System.Windows.Forms.TextBox()
        Me.lblPwd = New System.Windows.Forms.Label()
        Me.lblCnfPwd = New System.Windows.Forms.Label()
        Me.pnlRole = New System.Windows.Forms.Panel()
        Me.btnRole = New C1.Win.C1Input.C1Button()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.cboRole = New System.Windows.Forms.ComboBox()
        Me.btnRmvImg = New C1.Win.C1Input.C1Button()
        Me.PicBox = New System.Windows.Forms.PictureBox()
        Me.txtPhoneNo = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.txtEmail = New System.Windows.Forms.TextBox()
        Me.DtpDOB = New System.Windows.Forms.DateTimePicker()
        Me.chkDOB = New System.Windows.Forms.CheckBox()
        Me.txtDept = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtInActiveRem = New System.Windows.Forms.TextBox()
        Me.chkInactive = New System.Windows.Forms.CheckBox()
        Me.btnRights = New C1.Win.C1Input.C1Button()
        Me.pnlToolTip = New System.Windows.Forms.Panel()
        Me.PictureBox3 = New System.Windows.Forms.PictureBox()
        Me.lblToolTip = New System.Windows.Forms.Label()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.tbAdnlFlds = New System.Windows.Forms.TabControl()
        Me.TabPage1 = New System.Windows.Forms.TabPage()
        Me.vsfHdr = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.svtBtnCtrl = New vLQMS.btnCtrl()
        Me.svtAttach = New vLQMS.svtFileCtrl()
        Me.Panel1.SuspendLayout()
        Me.pnlSearch.SuspendLayout()
        CType(Me.vsfSearch, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlCodeName.SuspendLayout()
        Me.PnlPwdRules.SuspendLayout()
        Me.pnlPwd.SuspendLayout()
        CType(Me.imgEye, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlRole.SuspendLayout()
        CType(Me.PicBox, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlToolTip.SuspendLayout()
        CType(Me.PictureBox3, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tbAdnlFlds.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        CType(Me.vsfHdr, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.FromArgb(CType(CType(53, Byte), Integer), CType(CType(73, Byte), Integer), CType(CType(106, Byte), Integer))
        Me.Panel1.Controls.Add(Me.lblFrm)
        Me.Panel1.Controls.Add(Me.pnlSearch)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Margin = New System.Windows.Forms.Padding(4)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(1126, 35)
        Me.Panel1.TabIndex = 7
        '
        'lblFrm
        '
        Me.lblFrm.AutoSize = True
        Me.lblFrm.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.lblFrm.Font = New System.Drawing.Font("Segoe UI", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFrm.ForeColor = System.Drawing.Color.White
        Me.lblFrm.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.lblFrm.Location = New System.Drawing.Point(17, 7)
        Me.lblFrm.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblFrm.Name = "lblFrm"
        Me.lblFrm.Size = New System.Drawing.Size(0, 25)
        Me.lblFrm.TabIndex = 1
        '
        'pnlSearch
        '
        Me.pnlSearch.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnlSearch.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.pnlSearch.Controls.Add(Me.lblSearchImage)
        Me.pnlSearch.Controls.Add(Me.lblSearch)
        Me.pnlSearch.Controls.Add(Me.txtSearch)
        Me.pnlSearch.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!)
        Me.pnlSearch.Location = New System.Drawing.Point(497, 2)
        Me.pnlSearch.Margin = New System.Windows.Forms.Padding(4)
        Me.pnlSearch.Name = "pnlSearch"
        Me.pnlSearch.Size = New System.Drawing.Size(625, 37)
        Me.pnlSearch.TabIndex = 0
        '
        'lblSearchImage
        '
        Me.lblSearchImage.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblSearchImage.BackColor = System.Drawing.Color.White
        Me.lblSearchImage.ForeColor = System.Drawing.Color.Transparent
        Me.lblSearchImage.Image = Global.vLQMS.My.Resources.Resources.D_FIND
        Me.lblSearchImage.Location = New System.Drawing.Point(556, 5)
        Me.lblSearchImage.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblSearchImage.Name = "lblSearchImage"
        Me.lblSearchImage.Size = New System.Drawing.Size(34, 20)
        Me.lblSearchImage.TabIndex = 14
        '
        'lblSearch
        '
        Me.lblSearch.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblSearch.AutoSize = True
        Me.lblSearch.BackColor = System.Drawing.Color.Transparent
        Me.lblSearch.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSearch.ForeColor = System.Drawing.Color.White
        Me.lblSearch.Location = New System.Drawing.Point(332, 7)
        Me.lblSearch.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblSearch.Name = "lblSearch"
        Me.lblSearch.Size = New System.Drawing.Size(66, 20)
        Me.lblSearch.TabIndex = 0
        Me.lblSearch.Text = "Search"
        '
        'txtSearch
        '
        Me.txtSearch.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtSearch.BackColor = System.Drawing.Color.White
        Me.txtSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtSearch.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtSearch.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSearch.Location = New System.Drawing.Point(407, 4)
        Me.txtSearch.Margin = New System.Windows.Forms.Padding(5)
        Me.txtSearch.MaxLength = 50
        Me.txtSearch.Name = "txtSearch"
        Me.txtSearch.Size = New System.Drawing.Size(170, 25)
        Me.txtSearch.TabIndex = 0
        '
        'txtID
        '
        Me.txtID.BackColor = System.Drawing.Color.White
        Me.txtID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtID.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtID.ForeColor = System.Drawing.Color.Black
        Me.txtID.Location = New System.Drawing.Point(633, 155)
        Me.txtID.Margin = New System.Windows.Forms.Padding(5)
        Me.txtID.MaxLength = 250
        Me.txtID.Multiline = True
        Me.txtID.Name = "txtID"
        Me.txtID.Size = New System.Drawing.Size(29, 28)
        Me.txtID.TabIndex = 82
        Me.txtID.Visible = False
        '
        'txtCode
        '
        Me.txtCode.BackColor = System.Drawing.Color.White
        Me.txtCode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtCode.Enabled = False
        Me.txtCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCode.ForeColor = System.Drawing.Color.Black
        Me.txtCode.Location = New System.Drawing.Point(148, 4)
        Me.txtCode.Margin = New System.Windows.Forms.Padding(5)
        Me.txtCode.MaxLength = 50
        Me.txtCode.Multiline = True
        Me.txtCode.Name = "txtCode"
        Me.txtCode.Size = New System.Drawing.Size(312, 24)
        Me.txtCode.TabIndex = 0
        Me.txtCode.Tag = "UserCode"
        Me.ToolTip1.SetToolTip(Me.txtCode, "Enter Code")
        '
        'lblCode
        '
        Me.lblCode.BackColor = System.Drawing.Color.Transparent
        Me.lblCode.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.lblCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCode.ForeColor = System.Drawing.Color.Red
        Me.lblCode.Location = New System.Drawing.Point(6, 6)
        Me.lblCode.Margin = New System.Windows.Forms.Padding(5, 0, 5, 0)
        Me.lblCode.Name = "lblCode"
        Me.lblCode.Size = New System.Drawing.Size(130, 26)
        Me.lblCode.TabIndex = 85
        Me.lblCode.Tag = "M"
        Me.lblCode.Text = "Billing Category"
        '
        'vsfSearch
        '
        Me.vsfSearch.AllowDragging = C1.Win.C1FlexGrid.AllowDraggingEnum.None
        Me.vsfSearch.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfSearch.AutoResize = True
        Me.vsfSearch.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle
        Me.vsfSearch.ColumnInfo = "10,0,0,0,0,120,Columns:"
        Me.vsfSearch.ExtendLastCol = True
        Me.vsfSearch.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.vsfSearch.Location = New System.Drawing.Point(746, 42)
        Me.vsfSearch.Margin = New System.Windows.Forms.Padding(5)
        Me.vsfSearch.Name = "vsfSearch"
        Me.vsfSearch.Rows.DefaultSize = 24
        Me.vsfSearch.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.vsfSearch.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.RowRange
        Me.vsfSearch.Size = New System.Drawing.Size(378, 489)
        Me.vsfSearch.TabIndex = 3
        Me.vsfSearch.Visible = False
        '
        'Panel3
        '
        Me.Panel3.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel3.ForeColor = System.Drawing.Color.Black
        Me.Panel3.Location = New System.Drawing.Point(0, 536)
        Me.Panel3.Margin = New System.Windows.Forms.Padding(4)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(1125, 4)
        Me.Panel3.TabIndex = 2
        '
        'Panel4
        '
        Me.Panel4.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel4.Location = New System.Drawing.Point(739, 42)
        Me.Panel4.Margin = New System.Windows.Forms.Padding(4)
        Me.Panel4.Name = "Panel4"
        Me.Panel4.Size = New System.Drawing.Size(5, 495)
        Me.Panel4.TabIndex = 4
        '
        'txtNo
        '
        Me.txtNo.BackColor = System.Drawing.Color.White
        Me.txtNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtNo.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtNo.ForeColor = System.Drawing.Color.Black
        Me.txtNo.Location = New System.Drawing.Point(328, 59)
        Me.txtNo.Margin = New System.Windows.Forms.Padding(5)
        Me.txtNo.MaxLength = 250
        Me.txtNo.Multiline = True
        Me.txtNo.Name = "txtNo"
        Me.txtNo.Size = New System.Drawing.Size(29, 28)
        Me.txtNo.TabIndex = 89
        Me.txtNo.Visible = False
        '
        'txtName
        '
        Me.txtName.BackColor = System.Drawing.Color.White
        Me.txtName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtName.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtName.ForeColor = System.Drawing.Color.Black
        Me.txtName.Location = New System.Drawing.Point(148, 30)
        Me.txtName.Margin = New System.Windows.Forms.Padding(5)
        Me.txtName.MaxLength = 50
        Me.txtName.Multiline = True
        Me.txtName.Name = "txtName"
        Me.txtName.Size = New System.Drawing.Size(312, 24)
        Me.txtName.TabIndex = 1
        Me.txtName.Tag = "UserName"
        Me.ToolTip1.SetToolTip(Me.txtName, "Enter Name")
        '
        'lblName
        '
        Me.lblName.BackColor = System.Drawing.Color.Transparent
        Me.lblName.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.lblName.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblName.ForeColor = System.Drawing.Color.Red
        Me.lblName.Location = New System.Drawing.Point(6, 32)
        Me.lblName.Margin = New System.Windows.Forms.Padding(5, 0, 5, 0)
        Me.lblName.Name = "lblName"
        Me.lblName.Size = New System.Drawing.Size(121, 26)
        Me.lblName.TabIndex = 91
        Me.lblName.Tag = "M"
        Me.lblName.Text = "Billing Category"
        '
        'pnlCodeName
        '
        Me.pnlCodeName.Controls.Add(Me.lblApproved)
        Me.pnlCodeName.Controls.Add(Me.lblPwdReset)
        Me.pnlCodeName.Controls.Add(Me.dtPwdReset)
        Me.pnlCodeName.Controls.Add(Me.PnlPwdRules)
        Me.pnlCodeName.Controls.Add(Me.pnlPwd)
        Me.pnlCodeName.Controls.Add(Me.pnlRole)
        Me.pnlCodeName.Controls.Add(Me.btnRmvImg)
        Me.pnlCodeName.Controls.Add(Me.PicBox)
        Me.pnlCodeName.Controls.Add(Me.txtPhoneNo)
        Me.pnlCodeName.Controls.Add(Me.Label6)
        Me.pnlCodeName.Controls.Add(Me.Label5)
        Me.pnlCodeName.Controls.Add(Me.txtEmail)
        Me.pnlCodeName.Controls.Add(Me.DtpDOB)
        Me.pnlCodeName.Controls.Add(Me.chkDOB)
        Me.pnlCodeName.Controls.Add(Me.txtDept)
        Me.pnlCodeName.Controls.Add(Me.Label1)
        Me.pnlCodeName.Controls.Add(Me.txtInActiveRem)
        Me.pnlCodeName.Controls.Add(Me.txtName)
        Me.pnlCodeName.Controls.Add(Me.lblCode)
        Me.pnlCodeName.Controls.Add(Me.lblName)
        Me.pnlCodeName.Controls.Add(Me.txtCode)
        Me.pnlCodeName.Controls.Add(Me.chkInactive)
        Me.pnlCodeName.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!)
        Me.pnlCodeName.Location = New System.Drawing.Point(0, 40)
        Me.pnlCodeName.Margin = New System.Windows.Forms.Padding(4)
        Me.pnlCodeName.Name = "pnlCodeName"
        Me.pnlCodeName.Size = New System.Drawing.Size(737, 401)
        Me.pnlCodeName.TabIndex = 1
        Me.pnlCodeName.Tag = "RoleID"
        '
        'lblApproved
        '
        Me.lblApproved.BackColor = System.Drawing.Color.Transparent
        Me.lblApproved.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold)
        Me.lblApproved.ForeColor = System.Drawing.Color.Green
        Me.lblApproved.Location = New System.Drawing.Point(557, 6)
        Me.lblApproved.Name = "lblApproved"
        Me.lblApproved.Size = New System.Drawing.Size(166, 28)
        Me.lblApproved.TabIndex = 465
        Me.lblApproved.Text = "APPROVED"
        Me.lblApproved.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblPwdReset
        '
        Me.lblPwdReset.AutoSize = True
        Me.lblPwdReset.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPwdReset.ForeColor = System.Drawing.Color.Black
        Me.lblPwdReset.Location = New System.Drawing.Point(6, 377)
        Me.lblPwdReset.Name = "lblPwdReset"
        Me.lblPwdReset.Size = New System.Drawing.Size(125, 20)
        Me.lblPwdReset.TabIndex = 275
        Me.lblPwdReset.Text = "Password Reset"
        '
        'dtPwdReset
        '
        Me.dtPwdReset.CustomFormat = "dd/MM/yyyy"
        Me.dtPwdReset.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dtPwdReset.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtPwdReset.Location = New System.Drawing.Point(149, 372)
        Me.dtPwdReset.Name = "dtPwdReset"
        Me.dtPwdReset.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.dtPwdReset.Size = New System.Drawing.Size(117, 26)
        Me.dtPwdReset.TabIndex = 15
        Me.dtPwdReset.Tag = "PwdResetDt"
        '
        'PnlPwdRules
        '
        Me.PnlPwdRules.BackColor = System.Drawing.Color.White
        Me.PnlPwdRules.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.PnlPwdRules.Controls.Add(Me.Label3)
        Me.PnlPwdRules.Controls.Add(Me.Label11)
        Me.PnlPwdRules.Controls.Add(Me.lblMinLen)
        Me.PnlPwdRules.Controls.Add(Me.lblMinLCase)
        Me.PnlPwdRules.Controls.Add(Me.lblMinUCase)
        Me.PnlPwdRules.Controls.Add(Me.lblMinAlpha)
        Me.PnlPwdRules.Controls.Add(Me.lblMinNum)
        Me.PnlPwdRules.Controls.Add(Me.lblMinSplChars)
        Me.PnlPwdRules.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.PnlPwdRules.Location = New System.Drawing.Point(503, 210)
        Me.PnlPwdRules.Name = "PnlPwdRules"
        Me.PnlPwdRules.Size = New System.Drawing.Size(223, 188)
        Me.PnlPwdRules.TabIndex = 273
        Me.PnlPwdRules.Visible = False
        '
        'Label3
        '
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.SystemColors.ControlDarkDark
        Me.Label3.Location = New System.Drawing.Point(3, 142)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(250, 30)
        Me.Label3.TabIndex = 7
        Me.Label3.Text = "(Above settings can be changed in " & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Global Settings --> Login Settings)"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.Location = New System.Drawing.Point(53, 5)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(135, 20)
        Me.Label11.TabIndex = 6
        Me.Label11.Text = "*Password Rules*"
        '
        'lblMinLen
        '
        Me.lblMinLen.AutoSize = True
        Me.lblMinLen.Location = New System.Drawing.Point(6, 122)
        Me.lblMinLen.Name = "lblMinLen"
        Me.lblMinLen.Size = New System.Drawing.Size(18, 20)
        Me.lblMinLen.TabIndex = 5
        Me.lblMinLen.Text = "1"
        '
        'lblMinLCase
        '
        Me.lblMinLCase.AutoSize = True
        Me.lblMinLCase.Location = New System.Drawing.Point(6, 105)
        Me.lblMinLCase.Name = "lblMinLCase"
        Me.lblMinLCase.Size = New System.Drawing.Size(18, 20)
        Me.lblMinLCase.TabIndex = 4
        Me.lblMinLCase.Text = "1"
        '
        'lblMinUCase
        '
        Me.lblMinUCase.AutoSize = True
        Me.lblMinUCase.Location = New System.Drawing.Point(6, 88)
        Me.lblMinUCase.Name = "lblMinUCase"
        Me.lblMinUCase.Size = New System.Drawing.Size(18, 20)
        Me.lblMinUCase.TabIndex = 3
        Me.lblMinUCase.Text = "1"
        '
        'lblMinAlpha
        '
        Me.lblMinAlpha.AutoSize = True
        Me.lblMinAlpha.Location = New System.Drawing.Point(6, 71)
        Me.lblMinAlpha.Name = "lblMinAlpha"
        Me.lblMinAlpha.Size = New System.Drawing.Size(18, 20)
        Me.lblMinAlpha.TabIndex = 2
        Me.lblMinAlpha.Text = "1"
        '
        'lblMinNum
        '
        Me.lblMinNum.AutoSize = True
        Me.lblMinNum.Location = New System.Drawing.Point(6, 54)
        Me.lblMinNum.Name = "lblMinNum"
        Me.lblMinNum.Size = New System.Drawing.Size(18, 20)
        Me.lblMinNum.TabIndex = 1
        Me.lblMinNum.Text = "1"
        '
        'lblMinSplChars
        '
        Me.lblMinSplChars.AutoSize = True
        Me.lblMinSplChars.Location = New System.Drawing.Point(6, 37)
        Me.lblMinSplChars.Name = "lblMinSplChars"
        Me.lblMinSplChars.Size = New System.Drawing.Size(18, 20)
        Me.lblMinSplChars.TabIndex = 0
        Me.lblMinSplChars.Text = "1"
        '
        'pnlPwd
        '
        Me.pnlPwd.Controls.Add(Me.imgEye)
        Me.pnlPwd.Controls.Add(Me.txtCnfPassword)
        Me.pnlPwd.Controls.Add(Me.txtPassword)
        Me.pnlPwd.Controls.Add(Me.lblPwd)
        Me.pnlPwd.Controls.Add(Me.lblCnfPwd)
        Me.pnlPwd.Location = New System.Drawing.Point(5, 202)
        Me.pnlPwd.Name = "pnlPwd"
        Me.pnlPwd.Size = New System.Drawing.Size(499, 130)
        Me.pnlPwd.TabIndex = 10
        '
        'imgEye
        '
        Me.imgEye.Image = CType(resources.GetObject("imgEye.Image"), System.Drawing.Image)
        Me.imgEye.Location = New System.Drawing.Point(462, 6)
        Me.imgEye.Name = "imgEye"
        Me.imgEye.Size = New System.Drawing.Size(30, 24)
        Me.imgEye.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.imgEye.TabIndex = 515
        Me.imgEye.TabStop = False
        '
        'txtCnfPassword
        '
        Me.txtCnfPassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtCnfPassword.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCnfPassword.Location = New System.Drawing.Point(143, 35)
        Me.txtCnfPassword.MaxLength = 50
        Me.txtCnfPassword.Name = "txtCnfPassword"
        Me.txtCnfPassword.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtCnfPassword.Size = New System.Drawing.Size(312, 26)
        Me.txtCnfPassword.TabIndex = 12
        '
        'txtPassword
        '
        Me.txtPassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtPassword.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPassword.Location = New System.Drawing.Point(143, 6)
        Me.txtPassword.MaxLength = 50
        Me.txtPassword.Name = "txtPassword"
        Me.txtPassword.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtPassword.Size = New System.Drawing.Size(312, 26)
        Me.txtPassword.TabIndex = 11
        Me.txtPassword.Tag = "LoginPwd"
        '
        'lblPwd
        '
        Me.lblPwd.AutoSize = True
        Me.lblPwd.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPwd.ForeColor = System.Drawing.Color.Red
        Me.lblPwd.Location = New System.Drawing.Point(1, 8)
        Me.lblPwd.Name = "lblPwd"
        Me.lblPwd.Size = New System.Drawing.Size(78, 20)
        Me.lblPwd.TabIndex = 222
        Me.lblPwd.Tag = "M"
        Me.lblPwd.Text = "Password"
        '
        'lblCnfPwd
        '
        Me.lblCnfPwd.AutoSize = True
        Me.lblCnfPwd.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCnfPwd.ForeColor = System.Drawing.Color.Red
        Me.lblCnfPwd.Location = New System.Drawing.Point(1, 37)
        Me.lblCnfPwd.Name = "lblCnfPwd"
        Me.lblCnfPwd.Size = New System.Drawing.Size(137, 20)
        Me.lblCnfPwd.TabIndex = 225
        Me.lblCnfPwd.Tag = "M"
        Me.lblCnfPwd.Text = "Confirm Password"
        '
        'pnlRole
        '
        Me.pnlRole.Controls.Add(Me.btnRole)
        Me.pnlRole.Controls.Add(Me.Label2)
        Me.pnlRole.Controls.Add(Me.cboRole)
        Me.pnlRole.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pnlRole.Location = New System.Drawing.Point(5, 173)
        Me.pnlRole.Name = "pnlRole"
        Me.pnlRole.Size = New System.Drawing.Size(456, 36)
        Me.pnlRole.TabIndex = 7
        Me.pnlRole.Visible = False
        '
        'btnRole
        '
        Me.btnRole.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnRole.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnRole.Location = New System.Drawing.Point(407, 0)
        Me.btnRole.Margin = New System.Windows.Forms.Padding(4)
        Me.btnRole.Name = "btnRole"
        Me.btnRole.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.btnRole.Size = New System.Drawing.Size(49, 30)
        Me.btnRole.TabIndex = 9
        Me.btnRole.Text = "..."
        Me.ToolTip1.SetToolTip(Me.btnRole, "Role Master")
        Me.btnRole.UseVisualStyleBackColor = False
        '
        'Label2
        '
        Me.Label2.BackColor = System.Drawing.Color.Transparent
        Me.Label2.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.Color.Red
        Me.Label2.Location = New System.Drawing.Point(1, 5)
        Me.Label2.Margin = New System.Windows.Forms.Padding(5, 0, 5, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(48, 26)
        Me.Label2.TabIndex = 220
        Me.Label2.Tag = "M"
        Me.Label2.Text = "Role"
        '
        'cboRole
        '
        Me.cboRole.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboRole.FormattingEnabled = True
        Me.cboRole.Location = New System.Drawing.Point(144, 2)
        Me.cboRole.Name = "cboRole"
        Me.cboRole.Size = New System.Drawing.Size(256, 28)
        Me.cboRole.TabIndex = 8
        '
        'btnRmvImg
        '
        Me.btnRmvImg.BackgroundImage = Global.vLQMS.My.Resources.Resources.cancel
        Me.btnRmvImg.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.btnRmvImg.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.btnRmvImg.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnRmvImg.Location = New System.Drawing.Point(696, 167)
        Me.btnRmvImg.Margin = New System.Windows.Forms.Padding(4)
        Me.btnRmvImg.Name = "btnRmvImg"
        Me.btnRmvImg.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.btnRmvImg.Size = New System.Drawing.Size(22, 22)
        Me.btnRmvImg.TabIndex = 229
        Me.btnRmvImg.UseVisualStyleBackColor = False
        Me.btnRmvImg.Visible = False
        '
        'PicBox
        '
        Me.PicBox.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PicBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.PicBox.Cursor = System.Windows.Forms.Cursors.Hand
        Me.PicBox.Location = New System.Drawing.Point(600, 60)
        Me.PicBox.Name = "PicBox"
        Me.PicBox.Size = New System.Drawing.Size(127, 130)
        Me.PicBox.TabIndex = 228
        Me.PicBox.TabStop = False
        '
        'txtPhoneNo
        '
        Me.txtPhoneNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtPhoneNo.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPhoneNo.Location = New System.Drawing.Point(148, 146)
        Me.txtPhoneNo.MaxLength = 50
        Me.txtPhoneNo.Name = "txtPhoneNo"
        Me.txtPhoneNo.Size = New System.Drawing.Size(312, 26)
        Me.txtPhoneNo.TabIndex = 6
        Me.txtPhoneNo.Tag = "Mobileno"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.ForeColor = System.Drawing.Color.Black
        Me.Label6.Location = New System.Drawing.Point(6, 148)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(79, 20)
        Me.Label6.TabIndex = 219
        Me.Label6.Text = "Mobile No"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.ForeColor = System.Drawing.Color.Black
        Me.Label5.Location = New System.Drawing.Point(6, 119)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(69, 20)
        Me.Label5.TabIndex = 218
        Me.Label5.Text = "Email ID"
        '
        'txtEmail
        '
        Me.txtEmail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtEmail.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtEmail.Location = New System.Drawing.Point(148, 117)
        Me.txtEmail.MaxLength = 150
        Me.txtEmail.Name = "txtEmail"
        Me.txtEmail.Size = New System.Drawing.Size(312, 26)
        Me.txtEmail.TabIndex = 5
        Me.txtEmail.Tag = "EmailID"
        '
        'DtpDOB
        '
        Me.DtpDOB.CustomFormat = "dd/MM/yyyy"
        Me.DtpDOB.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DtpDOB.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.DtpDOB.Location = New System.Drawing.Point(148, 87)
        Me.DtpDOB.Name = "DtpDOB"
        Me.DtpDOB.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.DtpDOB.Size = New System.Drawing.Size(117, 26)
        Me.DtpDOB.TabIndex = 4
        Me.DtpDOB.Tag = "DOB"
        Me.DtpDOB.Visible = False
        '
        'chkDOB
        '
        Me.chkDOB.AutoSize = True
        Me.chkDOB.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkDOB.Location = New System.Drawing.Point(12, 89)
        Me.chkDOB.Name = "chkDOB"
        Me.chkDOB.Size = New System.Drawing.Size(63, 24)
        Me.chkDOB.TabIndex = 3
        Me.chkDOB.Text = "DOB"
        Me.chkDOB.UseVisualStyleBackColor = True
        '
        'txtDept
        '
        Me.txtDept.BackColor = System.Drawing.Color.White
        Me.txtDept.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtDept.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDept.ForeColor = System.Drawing.Color.Black
        Me.txtDept.Location = New System.Drawing.Point(148, 57)
        Me.txtDept.Margin = New System.Windows.Forms.Padding(5)
        Me.txtDept.MaxLength = 50
        Me.txtDept.Multiline = True
        Me.txtDept.Name = "txtDept"
        Me.txtDept.Size = New System.Drawing.Size(312, 24)
        Me.txtDept.TabIndex = 2
        Me.txtDept.Tag = "Department"
        Me.ToolTip1.SetToolTip(Me.txtDept, "Enter Name")
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.Color.Transparent
        Me.Label1.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.Black
        Me.Label1.Location = New System.Drawing.Point(6, 59)
        Me.Label1.Margin = New System.Windows.Forms.Padding(5, 0, 5, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(107, 26)
        Me.Label1.TabIndex = 93
        Me.Label1.Tag = ""
        Me.Label1.Text = "Department"
        '
        'txtInActiveRem
        '
        Me.txtInActiveRem.BackColor = System.Drawing.Color.White
        Me.txtInActiveRem.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtInActiveRem.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtInActiveRem.ForeColor = System.Drawing.Color.Black
        Me.txtInActiveRem.Location = New System.Drawing.Point(147, 340)
        Me.txtInActiveRem.Margin = New System.Windows.Forms.Padding(5)
        Me.txtInActiveRem.MaxLength = 250
        Me.txtInActiveRem.Multiline = True
        Me.txtInActiveRem.Name = "txtInActiveRem"
        Me.txtInActiveRem.Size = New System.Drawing.Size(313, 24)
        Me.txtInActiveRem.TabIndex = 14
        Me.txtInActiveRem.Tag = "InactiveRmks"
        '
        'chkInactive
        '
        Me.chkInactive.AutoSize = True
        Me.chkInactive.BackColor = System.Drawing.Color.Transparent
        Me.chkInactive.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.chkInactive.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkInactive.ForeColor = System.Drawing.Color.Black
        Me.chkInactive.Location = New System.Drawing.Point(9, 340)
        Me.chkInactive.Margin = New System.Windows.Forms.Padding(5)
        Me.chkInactive.Name = "chkInactive"
        Me.chkInactive.Size = New System.Drawing.Size(80, 24)
        Me.chkInactive.TabIndex = 13
        Me.chkInactive.Text = "Inactive"
        Me.chkInactive.UseVisualStyleBackColor = False
        '
        'btnRights
        '
        Me.btnRights.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnRights.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnRights.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnRights.Location = New System.Drawing.Point(575, 449)
        Me.btnRights.Margin = New System.Windows.Forms.Padding(4)
        Me.btnRights.Name = "btnRights"
        Me.btnRights.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.btnRights.Size = New System.Drawing.Size(152, 34)
        Me.btnRights.TabIndex = 226
        Me.btnRights.Text = "ASSIGN RIGHTS"
        Me.btnRights.UseVisualStyleBackColor = False
        '
        'pnlToolTip
        '
        Me.pnlToolTip.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.pnlToolTip.BackColor = System.Drawing.Color.FromArgb(CType(CType(52, Byte), Integer), CType(CType(68, Byte), Integer), CType(CType(88, Byte), Integer))
        Me.pnlToolTip.Controls.Add(Me.PictureBox3)
        Me.pnlToolTip.Controls.Add(Me.lblToolTip)
        Me.pnlToolTip.Location = New System.Drawing.Point(0, 548)
        Me.pnlToolTip.Margin = New System.Windows.Forms.Padding(4)
        Me.pnlToolTip.Name = "pnlToolTip"
        Me.pnlToolTip.Size = New System.Drawing.Size(836, 58)
        Me.pnlToolTip.TabIndex = 272
        Me.pnlToolTip.Visible = False
        '
        'PictureBox3
        '
        Me.PictureBox3.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PictureBox3.BackColor = System.Drawing.Color.Transparent
        Me.PictureBox3.BackgroundImage = CType(resources.GetObject("PictureBox3.BackgroundImage"), System.Drawing.Image)
        Me.PictureBox3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.PictureBox3.Location = New System.Drawing.Point(0, 1)
        Me.PictureBox3.Margin = New System.Windows.Forms.Padding(4)
        Me.PictureBox3.Name = "PictureBox3"
        Me.PictureBox3.Size = New System.Drawing.Size(55, 53)
        Me.PictureBox3.TabIndex = 219
        Me.PictureBox3.TabStop = False
        '
        'lblToolTip
        '
        Me.lblToolTip.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblToolTip.BackColor = System.Drawing.Color.Transparent
        Me.lblToolTip.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!)
        Me.lblToolTip.ForeColor = System.Drawing.Color.White
        Me.lblToolTip.Location = New System.Drawing.Point(56, 5)
        Me.lblToolTip.Margin = New System.Windows.Forms.Padding(5, 0, 5, 0)
        Me.lblToolTip.Name = "lblToolTip"
        Me.lblToolTip.Size = New System.Drawing.Size(772, 47)
        Me.lblToolTip.TabIndex = 216
        Me.lblToolTip.Text = "Help Tips:"
        '
        'tbAdnlFlds
        '
        Me.tbAdnlFlds.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbAdnlFlds.Controls.Add(Me.TabPage1)
        Me.tbAdnlFlds.Location = New System.Drawing.Point(745, 42)
        Me.tbAdnlFlds.Margin = New System.Windows.Forms.Padding(4)
        Me.tbAdnlFlds.Name = "tbAdnlFlds"
        Me.tbAdnlFlds.SelectedIndex = 0
        Me.tbAdnlFlds.Size = New System.Drawing.Size(379, 489)
        Me.tbAdnlFlds.TabIndex = 4
        '
        'TabPage1
        '
        Me.TabPage1.BackColor = System.Drawing.SystemColors.Window
        Me.TabPage1.Controls.Add(Me.vsfHdr)
        Me.TabPage1.Location = New System.Drawing.Point(4, 24)
        Me.TabPage1.Margin = New System.Windows.Forms.Padding(4)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(4)
        Me.TabPage1.Size = New System.Drawing.Size(371, 461)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "Header Info"
        '
        'vsfHdr
        '
        Me.vsfHdr.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None
        Me.vsfHdr.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle
        Me.vsfHdr.ColumnInfo = "2,1,0,0,0,105,Columns:"
        Me.vsfHdr.Dock = System.Windows.Forms.DockStyle.Fill
        Me.vsfHdr.ExtendLastCol = True
        Me.vsfHdr.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!)
        Me.vsfHdr.KeyActionEnter = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross
        Me.vsfHdr.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross
        Me.vsfHdr.Location = New System.Drawing.Point(4, 4)
        Me.vsfHdr.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.vsfHdr.Name = "vsfHdr"
        Me.vsfHdr.Rows.DefaultSize = 20
        Me.vsfHdr.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfHdr.Size = New System.Drawing.Size(363, 453)
        Me.vsfHdr.StyleInfo = resources.GetString("vsfHdr.StyleInfo")
        Me.vsfHdr.TabIndex = 0
        Me.vsfHdr.Tag = "DSGR"
        '
        'Timer1
        '
        Me.Timer1.Interval = 1000
        '
        'svtBtnCtrl
        '
        Me.svtBtnCtrl.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.svtBtnCtrl.BackColor = System.Drawing.Color.FromArgb(CType(CType(188, Byte), Integer), CType(CType(205, Byte), Integer), CType(CType(227, Byte), Integer))
        Me.svtBtnCtrl.CausesValidation = False
        Me.svtBtnCtrl.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.svtBtnCtrl.LayoutStyle = vLQMS.svtModule.typeLayoutStyles.Horizontal
        Me.svtBtnCtrl.Location = New System.Drawing.Point(41, 549)
        Me.svtBtnCtrl.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.svtBtnCtrl.Name = "svtBtnCtrl"
        Me.svtBtnCtrl.Size = New System.Drawing.Size(1084, 52)
        Me.svtBtnCtrl.TabIndex = 6
        '
        'svtAttach
        '
        Me.svtAttach.BackColor = System.Drawing.Color.FromArgb(CType(CType(233, Byte), Integer), CType(CType(237, Byte), Integer), CType(CType(241, Byte), Integer))
        Me.svtAttach.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.svtAttach.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.svtAttach.Location = New System.Drawing.Point(18, 461)
        Me.svtAttach.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.svtAttach.MaximumSize = New System.Drawing.Size(338, 135)
        Me.svtAttach.MinimumSize = New System.Drawing.Size(338, 135)
        Me.svtAttach.Name = "svtAttach"
        Me.svtAttach.ShowPrintSelection = False
        Me.svtAttach.Size = New System.Drawing.Size(338, 135)
        Me.svtAttach.TabIndex = 2
        Me.svtAttach.ViewMode = False
        '
        'frmMstUSer
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(188, Byte), Integer), CType(CType(205, Byte), Integer), CType(CType(227, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(1126, 610)
        Me.Controls.Add(Me.pnlToolTip)
        Me.Controls.Add(Me.pnlCodeName)
        Me.Controls.Add(Me.txtNo)
        Me.Controls.Add(Me.svtBtnCtrl)
        Me.Controls.Add(Me.Panel4)
        Me.Controls.Add(Me.Panel3)
        Me.Controls.Add(Me.btnRights)
        Me.Controls.Add(Me.svtAttach)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.txtID)
        Me.Controls.Add(Me.vsfSearch)
        Me.Controls.Add(Me.tbAdnlFlds)
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.KeyPreview = True
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.MinimumSize = New System.Drawing.Size(1142, 648)
        Me.Name = "frmMstUSer"
        Me.ShowIcon = False
        Me.Text = "Billing Category Master"
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.pnlSearch.ResumeLayout(False)
        Me.pnlSearch.PerformLayout()
        CType(Me.vsfSearch, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlCodeName.ResumeLayout(False)
        Me.pnlCodeName.PerformLayout()
        Me.PnlPwdRules.ResumeLayout(False)
        Me.PnlPwdRules.PerformLayout()
        Me.pnlPwd.ResumeLayout(False)
        Me.pnlPwd.PerformLayout()
        CType(Me.imgEye, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlRole.ResumeLayout(False)
        CType(Me.PicBox, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlToolTip.ResumeLayout(False)
        CType(Me.PictureBox3, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tbAdnlFlds.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        CType(Me.vsfHdr, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents lblSearch As System.Windows.Forms.Label
    Friend WithEvents txtSearch As System.Windows.Forms.TextBox
    Friend WithEvents pnlSearch As System.Windows.Forms.Panel
    Friend WithEvents txtID As System.Windows.Forms.TextBox
    Friend WithEvents txtCode As System.Windows.Forms.TextBox
    Friend WithEvents lblCode As System.Windows.Forms.Label
    Friend WithEvents svtAttach As vLQMS.svtFileCtrl
    Friend WithEvents vsfSearch As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
    Friend WithEvents Panel4 As System.Windows.Forms.Panel
    Friend WithEvents tt As System.Windows.Forms.ToolTip
    Friend WithEvents ColorDialog As System.Windows.Forms.ColorDialog
    Friend WithEvents svtBtnCtrl As vLQMS.btnCtrl
    Friend WithEvents txtNo As System.Windows.Forms.TextBox
    Friend WithEvents txtName As System.Windows.Forms.TextBox
    Friend WithEvents lblName As System.Windows.Forms.Label
    Friend WithEvents pnlCodeName As System.Windows.Forms.Panel
    Friend WithEvents chkInactive As System.Windows.Forms.CheckBox
    Friend WithEvents txtInActiveRem As System.Windows.Forms.TextBox
    Friend WithEvents pnlToolTip As System.Windows.Forms.Panel
    Friend WithEvents lblToolTip As System.Windows.Forms.Label
    Friend WithEvents PictureBox3 As System.Windows.Forms.PictureBox
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Public WithEvents tbAdnlFlds As System.Windows.Forms.TabControl
    Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
    Public WithEvents vsfHdr As C1.Win.C1FlexGrid.C1FlexGrid
    Public WithEvents lblFrm As Label
    Friend WithEvents lblSearchImage As Label
    Friend WithEvents txtDept As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents chkDOB As CheckBox
    Friend WithEvents DtpDOB As DateTimePicker
    Friend WithEvents Label2 As Label
    Friend WithEvents txtPhoneNo As TextBox
    Friend WithEvents Label6 As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents txtEmail As TextBox
    Friend WithEvents btnRole As C1.Win.C1Input.C1Button
    Friend WithEvents lblCnfPwd As Label
    Friend WithEvents lblPwd As Label
    Friend WithEvents txtPassword As TextBox
    Friend WithEvents txtCnfPassword As TextBox
    Friend WithEvents btnRights As C1.Win.C1Input.C1Button
    Friend WithEvents cboRole As ComboBox
    Friend WithEvents btnRmvImg As C1.Win.C1Input.C1Button
    Friend WithEvents PicBox As PictureBox
    Friend WithEvents Timer1 As Timer
    Friend WithEvents pnlRole As Panel
    Friend WithEvents pnlPwd As Panel
    Friend WithEvents PnlPwdRules As Panel
    Friend WithEvents lblMinLCase As Label
    Friend WithEvents lblMinUCase As Label
    Friend WithEvents lblMinAlpha As Label
    Friend WithEvents lblMinNum As Label
    Friend WithEvents lblMinSplChars As Label
    Friend WithEvents lblMinLen As Label
    Friend WithEvents Label11 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents lblApproved As Label
    Friend WithEvents lblPwdReset As Label
    Friend WithEvents dtPwdReset As DateTimePicker
    Friend WithEvents imgEye As PictureBox
End Class
