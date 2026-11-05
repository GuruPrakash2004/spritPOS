<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmMstUOM
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
        Dim CBlendItems1 As vLQMS.cBlendItems = New vLQMS.cBlendItems()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMstUOM))
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
        Me.btnFileAttach = New vLQMS.CButton()
        Me.ColorDialog = New System.Windows.Forms.ColorDialog()
        Me.txtNo = New System.Windows.Forms.TextBox()
        Me.txtName = New System.Windows.Forms.TextBox()
        Me.lblName = New System.Windows.Forms.Label()
        Me.pnlCodeName = New System.Windows.Forms.Panel()
        Me.txtDecimals = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtInActiveRem = New System.Windows.Forms.TextBox()
        Me.chkInactive = New System.Windows.Forms.CheckBox()
        Me.pnlToolTip = New System.Windows.Forms.Panel()
        Me.PictureBox3 = New System.Windows.Forms.PictureBox()
        Me.lblToolTip = New System.Windows.Forms.Label()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.tbAdnlFlds = New System.Windows.Forms.TabControl()
        Me.TabPage1 = New System.Windows.Forms.TabPage()
        Me.vsfHdr = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.txtItemNO = New System.Windows.Forms.TextBox()
        Me.pnlSeries = New System.Windows.Forms.Panel()
        Me.cboSeries = New System.Windows.Forms.ComboBox()
        Me.lblSeries = New System.Windows.Forms.Label()
        Me.lblApproved = New System.Windows.Forms.Label()
        Me.PnlAttach = New System.Windows.Forms.Panel()
        Me.vsfFiles = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.svtBtnCtrl = New vLQMS.btnCtrl()
        Me.Panel1.SuspendLayout()
        Me.pnlSearch.SuspendLayout()
        CType(Me.vsfSearch, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlCodeName.SuspendLayout()
        Me.pnlToolTip.SuspendLayout()
        CType(Me.PictureBox3, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tbAdnlFlds.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        CType(Me.vsfHdr, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlSeries.SuspendLayout()
        Me.PnlAttach.SuspendLayout()
        CType(Me.vsfFiles, System.ComponentModel.ISupportInitialize).BeginInit()
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
        Me.Panel1.Size = New System.Drawing.Size(1088, 35)
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
        Me.pnlSearch.Location = New System.Drawing.Point(497, 2)
        Me.pnlSearch.Margin = New System.Windows.Forms.Padding(4)
        Me.pnlSearch.Name = "pnlSearch"
        Me.pnlSearch.Size = New System.Drawing.Size(587, 37)
        Me.pnlSearch.TabIndex = 0
        '
        'lblSearchImage
        '
        Me.lblSearchImage.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblSearchImage.BackColor = System.Drawing.Color.White
        Me.lblSearchImage.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSearchImage.ForeColor = System.Drawing.Color.Transparent
        Me.lblSearchImage.Image = Global.vLQMS.My.Resources.Resources.D_FIND
        Me.lblSearchImage.Location = New System.Drawing.Point(501, 7)
        Me.lblSearchImage.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblSearchImage.Name = "lblSearchImage"
        Me.lblSearchImage.Size = New System.Drawing.Size(34, 19)
        Me.lblSearchImage.TabIndex = 1
        '
        'lblSearch
        '
        Me.lblSearch.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblSearch.AutoSize = True
        Me.lblSearch.BackColor = System.Drawing.Color.Transparent
        Me.lblSearch.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSearch.ForeColor = System.Drawing.Color.White
        Me.lblSearch.Location = New System.Drawing.Point(293, 5)
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
        Me.txtSearch.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSearch.Location = New System.Drawing.Point(368, 4)
        Me.txtSearch.Margin = New System.Windows.Forms.Padding(5)
        Me.txtSearch.MaxLength = 50
        Me.txtSearch.Name = "txtSearch"
        Me.txtSearch.Size = New System.Drawing.Size(170, 26)
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
        Me.txtCode.Location = New System.Drawing.Point(393, 2)
        Me.txtCode.Margin = New System.Windows.Forms.Padding(5)
        Me.txtCode.MaxLength = 50
        Me.txtCode.Name = "txtCode"
        Me.txtCode.Size = New System.Drawing.Size(144, 26)
        Me.txtCode.TabIndex = 1
        Me.ToolTip1.SetToolTip(Me.txtCode, "Enter Code")
        '
        'lblCode
        '
        Me.lblCode.BackColor = System.Drawing.Color.Transparent
        Me.lblCode.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.lblCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCode.ForeColor = System.Drawing.Color.Red
        Me.lblCode.Location = New System.Drawing.Point(267, 5)
        Me.lblCode.Margin = New System.Windows.Forms.Padding(5, 0, 5, 0)
        Me.lblCode.Name = "lblCode"
        Me.lblCode.Size = New System.Drawing.Size(128, 21)
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
        Me.vsfSearch.Location = New System.Drawing.Point(700, 42)
        Me.vsfSearch.Margin = New System.Windows.Forms.Padding(5)
        Me.vsfSearch.Name = "vsfSearch"
        Me.vsfSearch.Rows.DefaultSize = 24
        Me.vsfSearch.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.vsfSearch.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.RowRange
        Me.vsfSearch.Size = New System.Drawing.Size(387, 326)
        Me.vsfSearch.TabIndex = 1
        Me.vsfSearch.Visible = False
        '
        'Panel3
        '
        Me.Panel3.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel3.ForeColor = System.Drawing.Color.Black
        Me.Panel3.Location = New System.Drawing.Point(0, 369)
        Me.Panel3.Margin = New System.Windows.Forms.Padding(4)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(1087, 4)
        Me.Panel3.TabIndex = 2
        '
        'Panel4
        '
        Me.Panel4.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel4.Location = New System.Drawing.Point(695, 42)
        Me.Panel4.Margin = New System.Windows.Forms.Padding(4)
        Me.Panel4.Name = "Panel4"
        Me.Panel4.Size = New System.Drawing.Size(5, 328)
        Me.Panel4.TabIndex = 4
        '
        'btnFileAttach
        '
        CBlendItems1.iColor = New System.Drawing.Color() {System.Drawing.Color.AliceBlue, System.Drawing.Color.FromArgb(CType(CType(65, Byte), Integer), CType(CType(105, Byte), Integer), CType(CType(225, Byte), Integer)), System.Drawing.Color.FromArgb(CType(CType(65, Byte), Integer), CType(CType(105, Byte), Integer), CType(CType(225, Byte), Integer)), System.Drawing.Color.Navy}
        CBlendItems1.iPoint = New Single() {0!, 0.00308642!, 0.9969136!, 1.0!}
        Me.btnFileAttach.ColorFillBlend = CBlendItems1
        Me.btnFileAttach.DesignerSelected = False
        Me.btnFileAttach.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnFileAttach.Image = CType(resources.GetObject("btnFileAttach.Image"), System.Drawing.Image)
        Me.btnFileAttach.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnFileAttach.ImageIndex = 0
        Me.btnFileAttach.Imagelist = Nothing
        Me.btnFileAttach.Location = New System.Drawing.Point(2, 2)
        Me.btnFileAttach.Name = "btnFileAttach"
        Me.btnFileAttach.ShowFocus = vLQMS.CButton.eFocus.None
        Me.btnFileAttach.Size = New System.Drawing.Size(310, 33)
        Me.btnFileAttach.TabIndex = 0
        Me.btnFileAttach.Text = "File Attachment "
        Me.tt.SetToolTip(Me.btnFileAttach, "Shortcut : Ctrl + A")
        '
        'txtNo
        '
        Me.txtNo.BackColor = System.Drawing.Color.White
        Me.txtNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtNo.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtNo.ForeColor = System.Drawing.Color.Black
        Me.txtNo.Location = New System.Drawing.Point(328, 70)
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
        Me.txtName.Location = New System.Drawing.Point(135, 3)
        Me.txtName.Margin = New System.Windows.Forms.Padding(5)
        Me.txtName.MaxLength = 50
        Me.txtName.Name = "txtName"
        Me.txtName.Size = New System.Drawing.Size(402, 26)
        Me.txtName.TabIndex = 0
        Me.ToolTip1.SetToolTip(Me.txtName, "Enter Name")
        '
        'lblName
        '
        Me.lblName.BackColor = System.Drawing.Color.Transparent
        Me.lblName.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.lblName.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblName.ForeColor = System.Drawing.Color.Red
        Me.lblName.Location = New System.Drawing.Point(8, 5)
        Me.lblName.Margin = New System.Windows.Forms.Padding(5, 0, 5, 0)
        Me.lblName.Name = "lblName"
        Me.lblName.Size = New System.Drawing.Size(122, 21)
        Me.lblName.TabIndex = 91
        Me.lblName.Tag = "M"
        Me.lblName.Text = "Billing Category"
        '
        'pnlCodeName
        '
        Me.pnlCodeName.Controls.Add(Me.txtDecimals)
        Me.pnlCodeName.Controls.Add(Me.Label1)
        Me.pnlCodeName.Controls.Add(Me.txtInActiveRem)
        Me.pnlCodeName.Controls.Add(Me.txtName)
        Me.pnlCodeName.Controls.Add(Me.lblName)
        Me.pnlCodeName.Controls.Add(Me.chkInactive)
        Me.pnlCodeName.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pnlCodeName.Location = New System.Drawing.Point(8, 72)
        Me.pnlCodeName.Margin = New System.Windows.Forms.Padding(4)
        Me.pnlCodeName.Name = "pnlCodeName"
        Me.pnlCodeName.Size = New System.Drawing.Size(677, 183)
        Me.pnlCodeName.TabIndex = 1
        '
        'txtDecimals
        '
        Me.txtDecimals.BackColor = System.Drawing.Color.White
        Me.txtDecimals.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtDecimals.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDecimals.ForeColor = System.Drawing.Color.Black
        Me.txtDecimals.Location = New System.Drawing.Point(135, 129)
        Me.txtDecimals.Margin = New System.Windows.Forms.Padding(5)
        Me.txtDecimals.MaxLength = 1
        Me.txtDecimals.Name = "txtDecimals"
        Me.txtDecimals.Size = New System.Drawing.Size(56, 26)
        Me.txtDecimals.TabIndex = 2
        Me.ToolTip1.SetToolTip(Me.txtDecimals, "Enter Name")
        Me.txtDecimals.Visible = False
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.Color.Transparent
        Me.Label1.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.Black
        Me.Label1.Location = New System.Drawing.Point(8, 131)
        Me.Label1.Margin = New System.Windows.Forms.Padding(5, 0, 5, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(122, 21)
        Me.Label1.TabIndex = 93
        Me.Label1.Tag = ""
        Me.Label1.Text = "Decimal places"
        Me.Label1.Visible = False
        '
        'txtInActiveRem
        '
        Me.txtInActiveRem.BackColor = System.Drawing.Color.White
        Me.txtInActiveRem.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtInActiveRem.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtInActiveRem.ForeColor = System.Drawing.Color.Black
        Me.txtInActiveRem.Location = New System.Drawing.Point(135, 64)
        Me.txtInActiveRem.Margin = New System.Windows.Forms.Padding(5)
        Me.txtInActiveRem.MaxLength = 250
        Me.txtInActiveRem.Multiline = True
        Me.txtInActiveRem.Name = "txtInActiveRem"
        Me.txtInActiveRem.Size = New System.Drawing.Size(402, 47)
        Me.txtInActiveRem.TabIndex = 1
        '
        'chkInactive
        '
        Me.chkInactive.AutoSize = True
        Me.chkInactive.BackColor = System.Drawing.Color.Transparent
        Me.chkInactive.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.chkInactive.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkInactive.ForeColor = System.Drawing.Color.Black
        Me.chkInactive.Location = New System.Drawing.Point(136, 30)
        Me.chkInactive.Margin = New System.Windows.Forms.Padding(5)
        Me.chkInactive.Name = "chkInactive"
        Me.chkInactive.Size = New System.Drawing.Size(80, 24)
        Me.chkInactive.TabIndex = 3
        Me.chkInactive.Text = "Inactive"
        Me.chkInactive.UseVisualStyleBackColor = False
        '
        'pnlToolTip
        '
        Me.pnlToolTip.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.pnlToolTip.BackColor = System.Drawing.Color.FromArgb(CType(CType(52, Byte), Integer), CType(CType(68, Byte), Integer), CType(CType(88, Byte), Integer))
        Me.pnlToolTip.Controls.Add(Me.PictureBox3)
        Me.pnlToolTip.Controls.Add(Me.lblToolTip)
        Me.pnlToolTip.Location = New System.Drawing.Point(0, 389)
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
        Me.lblToolTip.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
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
        Me.tbAdnlFlds.Location = New System.Drawing.Point(700, 42)
        Me.tbAdnlFlds.Margin = New System.Windows.Forms.Padding(4)
        Me.tbAdnlFlds.Name = "tbAdnlFlds"
        Me.tbAdnlFlds.SelectedIndex = 0
        Me.tbAdnlFlds.Size = New System.Drawing.Size(388, 323)
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
        Me.TabPage1.Size = New System.Drawing.Size(380, 295)
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
        Me.vsfHdr.Size = New System.Drawing.Size(372, 287)
        Me.vsfHdr.StyleInfo = resources.GetString("vsfHdr.StyleInfo")
        Me.vsfHdr.TabIndex = 5
        Me.vsfHdr.Tag = "DSGR"
        '
        'txtItemNO
        '
        Me.txtItemNO.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtItemNO.Location = New System.Drawing.Point(588, 348)
        Me.txtItemNO.Name = "txtItemNO"
        Me.txtItemNO.Size = New System.Drawing.Size(25, 26)
        Me.txtItemNO.TabIndex = 0
        Me.txtItemNO.Visible = False
        '
        'pnlSeries
        '
        Me.pnlSeries.Controls.Add(Me.cboSeries)
        Me.pnlSeries.Controls.Add(Me.txtCode)
        Me.pnlSeries.Controls.Add(Me.lblSeries)
        Me.pnlSeries.Controls.Add(Me.lblCode)
        Me.pnlSeries.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pnlSeries.Location = New System.Drawing.Point(8, 37)
        Me.pnlSeries.Name = "pnlSeries"
        Me.pnlSeries.Size = New System.Drawing.Size(543, 32)
        Me.pnlSeries.TabIndex = 281
        '
        'cboSeries
        '
        Me.cboSeries.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.cboSeries.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboSeries.FormattingEnabled = True
        Me.cboSeries.Location = New System.Drawing.Point(136, 2)
        Me.cboSeries.MaxLength = 10
        Me.cboSeries.Name = "cboSeries"
        Me.cboSeries.Size = New System.Drawing.Size(123, 28)
        Me.cboSeries.TabIndex = 0
        '
        'lblSeries
        '
        Me.lblSeries.AutoSize = True
        Me.lblSeries.BackColor = System.Drawing.Color.Transparent
        Me.lblSeries.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.lblSeries.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSeries.ForeColor = System.Drawing.Color.Red
        Me.lblSeries.Location = New System.Drawing.Point(8, 5)
        Me.lblSeries.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblSeries.Name = "lblSeries"
        Me.lblSeries.Size = New System.Drawing.Size(54, 20)
        Me.lblSeries.TabIndex = 87
        Me.lblSeries.Tag = "M"
        Me.lblSeries.Text = "Series"
        '
        'lblApproved
        '
        Me.lblApproved.BackColor = System.Drawing.Color.Transparent
        Me.lblApproved.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblApproved.ForeColor = System.Drawing.Color.Green
        Me.lblApproved.Location = New System.Drawing.Point(551, 42)
        Me.lblApproved.Name = "lblApproved"
        Me.lblApproved.Size = New System.Drawing.Size(135, 24)
        Me.lblApproved.TabIndex = 282
        Me.lblApproved.Text = "APPROVED"
        Me.lblApproved.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.lblApproved.Visible = False
        '
        'PnlAttach
        '
        Me.PnlAttach.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.PnlAttach.BackColor = System.Drawing.Color.Transparent
        Me.PnlAttach.Controls.Add(Me.btnFileAttach)
        Me.PnlAttach.Controls.Add(Me.vsfFiles)
        Me.PnlAttach.Location = New System.Drawing.Point(21, 244)
        Me.PnlAttach.Name = "PnlAttach"
        Me.PnlAttach.Size = New System.Drawing.Size(314, 121)
        Me.PnlAttach.TabIndex = 484
        '
        'vsfFiles
        '
        Me.vsfFiles.AllowDragging = C1.Win.C1FlexGrid.AllowDraggingEnum.None
        Me.vsfFiles.AllowEditing = False
        Me.vsfFiles.AllowFreezing = C1.Win.C1FlexGrid.AllowFreezingEnum.Columns
        Me.vsfFiles.AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.Free
        Me.vsfFiles.AllowMergingFixed = C1.Win.C1FlexGrid.AllowMergingEnum.Free
        Me.vsfFiles.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfFiles.AutoResize = True
        Me.vsfFiles.AutoSearch = C1.Win.C1FlexGrid.AutoSearchEnum.FromTop
        Me.vsfFiles.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle
        Me.vsfFiles.ColumnInfo = "2,0,0,0,0,105,Columns:"
        Me.vsfFiles.EditOptions = C1.Win.C1FlexGrid.EditFlags.AutoSearch
        Me.vsfFiles.ExtendLastCol = True
        Me.vsfFiles.FocusRect = C1.Win.C1FlexGrid.FocusRectEnum.Heavy
        Me.vsfFiles.Font = New System.Drawing.Font("Segoe UI", 8.25!)
        Me.vsfFiles.ForeColor = System.Drawing.Color.Black
        Me.vsfFiles.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross
        Me.vsfFiles.Location = New System.Drawing.Point(1, 35)
        Me.vsfFiles.Name = "vsfFiles"
        Me.vsfFiles.Rows.Count = 2
        Me.vsfFiles.Rows.DefaultSize = 21
        Me.vsfFiles.Rows.Fixed = 0
        Me.vsfFiles.Rows.GlyphRow = 0
        Me.vsfFiles.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfFiles.ShowCellLabels = True
        Me.vsfFiles.Size = New System.Drawing.Size(311, 83)
        Me.vsfFiles.StyleInfo = resources.GetString("vsfFiles.StyleInfo")
        Me.vsfFiles.TabIndex = 481
        Me.vsfFiles.Tag = "View"
        Me.vsfFiles.VisualStyle = C1.Win.C1FlexGrid.VisualStyle.Office2007Blue
        '
        'svtBtnCtrl
        '
        Me.svtBtnCtrl.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.svtBtnCtrl.BackColor = System.Drawing.Color.FromArgb(CType(CType(188, Byte), Integer), CType(CType(205, Byte), Integer), CType(CType(227, Byte), Integer))
        Me.svtBtnCtrl.CausesValidation = False
        Me.svtBtnCtrl.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.svtBtnCtrl.LayoutStyle = vLQMS.svtModule.typeLayoutStyles.Horizontal
        Me.svtBtnCtrl.Location = New System.Drawing.Point(-2, 382)
        Me.svtBtnCtrl.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.svtBtnCtrl.Name = "svtBtnCtrl"
        Me.svtBtnCtrl.Size = New System.Drawing.Size(970, 52)
        Me.svtBtnCtrl.TabIndex = 4
        '
        'frmMstUOM
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(188, Byte), Integer), CType(CType(205, Byte), Integer), CType(CType(227, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(1088, 443)
        Me.Controls.Add(Me.PnlAttach)
        Me.Controls.Add(Me.lblApproved)
        Me.Controls.Add(Me.pnlSeries)
        Me.Controls.Add(Me.txtItemNO)
        Me.Controls.Add(Me.pnlToolTip)
        Me.Controls.Add(Me.pnlCodeName)
        Me.Controls.Add(Me.svtBtnCtrl)
        Me.Controls.Add(Me.Panel4)
        Me.Controls.Add(Me.Panel3)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.txtID)
        Me.Controls.Add(Me.vsfSearch)
        Me.Controls.Add(Me.tbAdnlFlds)
        Me.Controls.Add(Me.txtNo)
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.KeyPreview = True
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.MinimumSize = New System.Drawing.Size(1104, 474)
        Me.Name = "frmMstUOM"
        Me.ShowIcon = False
        Me.Text = "Billing Category Master"
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.pnlSearch.ResumeLayout(False)
        Me.pnlSearch.PerformLayout()
        CType(Me.vsfSearch, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlCodeName.ResumeLayout(False)
        Me.pnlCodeName.PerformLayout()
        Me.pnlToolTip.ResumeLayout(False)
        CType(Me.PictureBox3, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tbAdnlFlds.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        CType(Me.vsfHdr, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlSeries.ResumeLayout(False)
        Me.pnlSeries.PerformLayout()
        Me.PnlAttach.ResumeLayout(False)
        CType(Me.vsfFiles, System.ComponentModel.ISupportInitialize).EndInit()
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
    Friend WithEvents txtItemNO As TextBox
    Friend WithEvents pnlSeries As Panel
    Friend WithEvents cboSeries As ComboBox
    Friend WithEvents lblSeries As Label
    Friend WithEvents lblApproved As Label
    Friend WithEvents txtDecimals As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents PnlAttach As Panel
    Public WithEvents vsfFiles As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents btnFileAttach As CButton
End Class
