<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmMstItem
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMstItem))
        Me.txtSearch = New System.Windows.Forms.TextBox()
        Me.txtItemCode = New System.Windows.Forms.TextBox()
        Me.lblItemCode = New System.Windows.Forms.Label()
        Me.txtInActiveRem = New System.Windows.Forms.TextBox()
        Me.chkInactive = New System.Windows.Forms.CheckBox()
        Me.pnlSearch = New System.Windows.Forms.Panel()
        Me.lblSearch = New System.Windows.Forms.Label()
        Me.lblSearchImage = New System.Windows.Forms.Label()
        Me.vsfSearch = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.tt = New System.Windows.Forms.ToolTip(Me.components)
        Me.btnRemoveSupp = New System.Windows.Forms.Button()
        Me.btnFileAttach = New vLQMS.CButton()
        Me.lblTotalRecords = New System.Windows.Forms.Label()
        Me.txtItemName = New System.Windows.Forms.TextBox()
        Me.lblItemName = New System.Windows.Forms.Label()
        Me.Panel3 = New System.Windows.Forms.Panel()
        Me.lblToolTip = New System.Windows.Forms.Label()
        Me.cboIG = New System.Windows.Forms.ComboBox()
        Me.lblItemGrp = New System.Windows.Forms.Label()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.PnlCopyFrom = New System.Windows.Forms.Panel()
        Me.btnCopyFrom = New System.Windows.Forms.Button()
        Me.txtCopyItem = New System.Windows.Forms.TextBox()
        Me.lblDispItemCode = New System.Windows.Forms.Label()
        Me.Panel4 = New System.Windows.Forms.Panel()
        Me.txtItemID = New System.Windows.Forms.TextBox()
        Me.txtItemNO = New System.Windows.Forms.TextBox()
        Me.tbAdnlFlds = New System.Windows.Forms.TabControl()
        Me.TabPage1 = New System.Windows.Forms.TabPage()
        Me.vsfHdr = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.pnlToolTip = New System.Windows.Forms.Panel()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.chkNonStkable = New System.Windows.Forms.CheckBox()
        Me.lblLiqCategary = New System.Windows.Forms.Label()
        Me.txtShortName = New System.Windows.Forms.TextBox()
        Me.lblShortName = New System.Windows.Forms.Label()
        Me.txtNo = New System.Windows.Forms.TextBox()
        Me.pnlSeries = New System.Windows.Forms.Panel()
        Me.cboType = New System.Windows.Forms.ComboBox()
        Me.btnMstItemGrp = New System.Windows.Forms.Button()
        Me.txtHotKey = New System.Windows.Forms.TextBox()
        Me.lblHotKey = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtHSNCode = New System.Windows.Forms.TextBox()
        Me.btnMstUOM = New System.Windows.Forms.Button()
        Me.cboUOM = New System.Windows.Forms.ComboBox()
        Me.lblStkUOM = New System.Windows.Forms.Label()
        Me.vsfAlias = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.PnlAttach = New System.Windows.Forms.Panel()
        Me.vsfFiles = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.pnlReOrder = New System.Windows.Forms.Panel()
        Me.txtROL = New System.Windows.Forms.TextBox()
        Me.Label21 = New System.Windows.Forms.Label()
        Me.Label20 = New System.Windows.Forms.Label()
        Me.txtROQ = New System.Windows.Forms.TextBox()
        Me.lblBottPerCases = New System.Windows.Forms.Label()
        Me.txtBotPerCases = New System.Windows.Forms.TextBox()
        Me.svtBtnCtrl = New vLQMS.btnCtrl()
        Me.pnlSearch.SuspendLayout()
        CType(Me.vsfSearch, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel3.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.PnlCopyFrom.SuspendLayout()
        Me.tbAdnlFlds.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        CType(Me.vsfHdr, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlToolTip.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlSeries.SuspendLayout()
        CType(Me.vsfAlias, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PnlAttach.SuspendLayout()
        CType(Me.vsfFiles, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlReOrder.SuspendLayout()
        Me.SuspendLayout()
        '
        'txtSearch
        '
        Me.txtSearch.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtSearch.BackColor = System.Drawing.Color.White
        Me.txtSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtSearch.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtSearch.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSearch.Location = New System.Drawing.Point(115, 3)
        Me.txtSearch.Margin = New System.Windows.Forms.Padding(4)
        Me.txtSearch.MaxLength = 50
        Me.txtSearch.Name = "txtSearch"
        Me.txtSearch.Size = New System.Drawing.Size(170, 26)
        Me.txtSearch.TabIndex = 0
        '
        'txtItemCode
        '
        Me.txtItemCode.BackColor = System.Drawing.Color.White
        Me.txtItemCode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtItemCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtItemCode.ForeColor = System.Drawing.Color.Black
        Me.txtItemCode.Location = New System.Drawing.Point(151, 6)
        Me.txtItemCode.Margin = New System.Windows.Forms.Padding(4)
        Me.txtItemCode.MaxLength = 50
        Me.txtItemCode.Name = "txtItemCode"
        Me.txtItemCode.Size = New System.Drawing.Size(489, 21)
        Me.txtItemCode.TabIndex = 1
        Me.txtItemCode.Tag = "ITEMCODE"
        Me.txtItemCode.Text = " "
        Me.tt.SetToolTip(Me.txtItemCode, "Item Code")
        '
        'lblItemCode
        '
        Me.lblItemCode.BackColor = System.Drawing.Color.Transparent
        Me.lblItemCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblItemCode.ForeColor = System.Drawing.Color.Red
        Me.lblItemCode.Location = New System.Drawing.Point(4, 6)
        Me.lblItemCode.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblItemCode.Name = "lblItemCode"
        Me.lblItemCode.Size = New System.Drawing.Size(88, 21)
        Me.lblItemCode.TabIndex = 41
        Me.lblItemCode.Tag = "M"
        Me.lblItemCode.Text = "Item Code"
        '
        'txtInActiveRem
        '
        Me.txtInActiveRem.BackColor = System.Drawing.Color.White
        Me.txtInActiveRem.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtInActiveRem.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtInActiveRem.ForeColor = System.Drawing.Color.Black
        Me.txtInActiveRem.Location = New System.Drawing.Point(9, 322)
        Me.txtInActiveRem.Margin = New System.Windows.Forms.Padding(4)
        Me.txtInActiveRem.MaxLength = 250
        Me.txtInActiveRem.Multiline = True
        Me.txtInActiveRem.Name = "txtInActiveRem"
        Me.txtInActiveRem.Size = New System.Drawing.Size(319, 42)
        Me.txtInActiveRem.TabIndex = 9
        Me.txtInActiveRem.Tag = "InActiveRmks"
        '
        'chkInactive
        '
        Me.chkInactive.AutoSize = True
        Me.chkInactive.BackColor = System.Drawing.Color.Transparent
        Me.chkInactive.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkInactive.ForeColor = System.Drawing.Color.Black
        Me.chkInactive.Location = New System.Drawing.Point(9, 294)
        Me.chkInactive.Margin = New System.Windows.Forms.Padding(4)
        Me.chkInactive.Name = "chkInactive"
        Me.chkInactive.Size = New System.Drawing.Size(83, 24)
        Me.chkInactive.TabIndex = 8
        Me.chkInactive.Tag = "InActive"
        Me.chkInactive.Text = "Inactive"
        Me.chkInactive.UseVisualStyleBackColor = False
        '
        'pnlSearch
        '
        Me.pnlSearch.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnlSearch.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.pnlSearch.Controls.Add(Me.lblSearch)
        Me.pnlSearch.Controls.Add(Me.lblSearchImage)
        Me.pnlSearch.Controls.Add(Me.txtSearch)
        Me.pnlSearch.Location = New System.Drawing.Point(695, 1)
        Me.pnlSearch.Name = "pnlSearch"
        Me.pnlSearch.Size = New System.Drawing.Size(287, 30)
        Me.pnlSearch.TabIndex = 4
        '
        'lblSearch
        '
        Me.lblSearch.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblSearch.AutoSize = True
        Me.lblSearch.BackColor = System.Drawing.Color.Transparent
        Me.lblSearch.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSearch.ForeColor = System.Drawing.Color.White
        Me.lblSearch.Location = New System.Drawing.Point(42, 7)
        Me.lblSearch.Name = "lblSearch"
        Me.lblSearch.Size = New System.Drawing.Size(66, 20)
        Me.lblSearch.TabIndex = 16
        Me.lblSearch.Text = "Search"
        '
        'lblSearchImage
        '
        Me.lblSearchImage.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblSearchImage.BackColor = System.Drawing.Color.White
        Me.lblSearchImage.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSearchImage.Image = Global.vLQMS.My.Resources.Resources.D_FIND
        Me.lblSearchImage.Location = New System.Drawing.Point(253, 4)
        Me.lblSearchImage.Name = "lblSearchImage"
        Me.lblSearchImage.Size = New System.Drawing.Size(32, 19)
        Me.lblSearchImage.TabIndex = 15
        '
        'vsfSearch
        '
        Me.vsfSearch.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfSearch.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle
        Me.vsfSearch.ColumnInfo = "10,0,0,0,0,120,Columns:"
        Me.vsfSearch.ExtendLastCol = True
        Me.vsfSearch.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.vsfSearch.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveDown
        Me.vsfSearch.Location = New System.Drawing.Point(655, 34)
        Me.vsfSearch.Margin = New System.Windows.Forms.Padding(4)
        Me.vsfSearch.Name = "vsfSearch"
        Me.vsfSearch.Rows.DefaultSize = 24
        Me.vsfSearch.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.RowRange
        Me.vsfSearch.Size = New System.Drawing.Size(330, 612)
        Me.vsfSearch.StyleInfo = resources.GetString("vsfSearch.StyleInfo")
        Me.vsfSearch.TabIndex = 12
        Me.vsfSearch.Visible = False
        '
        'btnRemoveSupp
        '
        Me.btnRemoveSupp.BackgroundImage = CType(resources.GetObject("btnRemoveSupp.BackgroundImage"), System.Drawing.Image)
        Me.btnRemoveSupp.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.btnRemoveSupp.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnRemoveSupp.ForeColor = System.Drawing.Color.Red
        Me.btnRemoveSupp.Location = New System.Drawing.Point(598, 353)
        Me.btnRemoveSupp.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnRemoveSupp.Name = "btnRemoveSupp"
        Me.btnRemoveSupp.Size = New System.Drawing.Size(34, 26)
        Me.btnRemoveSupp.TabIndex = 287
        Me.btnRemoveSupp.TextAlign = System.Drawing.ContentAlignment.TopCenter
        Me.tt.SetToolTip(Me.btnRemoveSupp, "Remove Row")
        Me.btnRemoveSupp.UseVisualStyleBackColor = True
        '
        'btnFileAttach
        '
        Me.btnFileAttach.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnFileAttach.DesignerSelected = False
        Me.btnFileAttach.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnFileAttach.Image = CType(resources.GetObject("btnFileAttach.Image"), System.Drawing.Image)
        Me.btnFileAttach.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnFileAttach.ImageIndex = 0
        Me.btnFileAttach.Imagelist = Nothing
        Me.btnFileAttach.Location = New System.Drawing.Point(1, -5)
        Me.btnFileAttach.Name = "btnFileAttach"
        Me.btnFileAttach.ShowFocus = vLQMS.CButton.eFocus.None
        Me.btnFileAttach.Size = New System.Drawing.Size(311, 35)
        Me.btnFileAttach.TabIndex = 479
        Me.btnFileAttach.Text = "File Attachment "
        Me.tt.SetToolTip(Me.btnFileAttach, "Shortcut : Ctrl + A")
        '
        'lblTotalRecords
        '
        Me.lblTotalRecords.AutoSize = True
        Me.lblTotalRecords.Location = New System.Drawing.Point(12, 592)
        Me.lblTotalRecords.Name = "lblTotalRecords"
        Me.lblTotalRecords.Size = New System.Drawing.Size(0, 17)
        Me.lblTotalRecords.TabIndex = 456
        '
        'txtItemName
        '
        Me.txtItemName.BackColor = System.Drawing.Color.White
        Me.txtItemName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtItemName.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtItemName.ForeColor = System.Drawing.Color.Black
        Me.txtItemName.Location = New System.Drawing.Point(151, 70)
        Me.txtItemName.Margin = New System.Windows.Forms.Padding(4)
        Me.txtItemName.MaxLength = 100
        Me.txtItemName.Name = "txtItemName"
        Me.txtItemName.Size = New System.Drawing.Size(488, 26)
        Me.txtItemName.TabIndex = 0
        Me.txtItemName.Tag = "ITEMNAME"
        '
        'lblItemName
        '
        Me.lblItemName.AutoSize = True
        Me.lblItemName.BackColor = System.Drawing.Color.Transparent
        Me.lblItemName.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblItemName.ForeColor = System.Drawing.Color.Red
        Me.lblItemName.Location = New System.Drawing.Point(4, 72)
        Me.lblItemName.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblItemName.Name = "lblItemName"
        Me.lblItemName.Size = New System.Drawing.Size(87, 20)
        Me.lblItemName.TabIndex = 58
        Me.lblItemName.Tag = "M"
        Me.lblItemName.Text = "Item Name"
        '
        'Panel3
        '
        Me.Panel3.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel3.Controls.Add(Me.lblToolTip)
        Me.Panel3.ForeColor = System.Drawing.Color.Black
        Me.Panel3.Location = New System.Drawing.Point(0, 649)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(984, 4)
        Me.Panel3.TabIndex = 45
        '
        'lblToolTip
        '
        Me.lblToolTip.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblToolTip.BackColor = System.Drawing.Color.Transparent
        Me.lblToolTip.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!)
        Me.lblToolTip.ForeColor = System.Drawing.Color.White
        Me.lblToolTip.Location = New System.Drawing.Point(70, -23)
        Me.lblToolTip.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblToolTip.Name = "lblToolTip"
        Me.lblToolTip.Size = New System.Drawing.Size(493, 33)
        Me.lblToolTip.TabIndex = 216
        Me.lblToolTip.Text = "Help Tips:"
        '
        'cboIG
        '
        Me.cboIG.BackColor = System.Drawing.Color.White
        Me.cboIG.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.cboIG.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboIG.ForeColor = System.Drawing.Color.Black
        Me.cboIG.FormattingEnabled = True
        Me.cboIG.Location = New System.Drawing.Point(151, 177)
        Me.cboIG.Margin = New System.Windows.Forms.Padding(4)
        Me.cboIG.Name = "cboIG"
        Me.cboIG.Size = New System.Drawing.Size(479, 28)
        Me.cboIG.TabIndex = 5
        Me.cboIG.Tag = "ITEMGroupID"
        '
        'lblItemGrp
        '
        Me.lblItemGrp.AutoSize = True
        Me.lblItemGrp.BackColor = System.Drawing.Color.Transparent
        Me.lblItemGrp.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblItemGrp.ForeColor = System.Drawing.Color.Red
        Me.lblItemGrp.Location = New System.Drawing.Point(5, 175)
        Me.lblItemGrp.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblItemGrp.Name = "lblItemGrp"
        Me.lblItemGrp.Size = New System.Drawing.Size(90, 20)
        Me.lblItemGrp.TabIndex = 38
        Me.lblItemGrp.Tag = "M"
        Me.lblItemGrp.Text = "Item Group"
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.FromArgb(CType(CType(53, Byte), Integer), CType(CType(73, Byte), Integer), CType(CType(106, Byte), Integer))
        Me.Panel1.Controls.Add(Me.PnlCopyFrom)
        Me.Panel1.Controls.Add(Me.pnlSearch)
        Me.Panel1.Controls.Add(Me.lblDispItemCode)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(985, 32)
        Me.Panel1.TabIndex = 0
        '
        'PnlCopyFrom
        '
        Me.PnlCopyFrom.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PnlCopyFrom.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.PnlCopyFrom.Controls.Add(Me.btnCopyFrom)
        Me.PnlCopyFrom.Controls.Add(Me.txtCopyItem)
        Me.PnlCopyFrom.Location = New System.Drawing.Point(366, 0)
        Me.PnlCopyFrom.Name = "PnlCopyFrom"
        Me.PnlCopyFrom.Size = New System.Drawing.Size(187, 30)
        Me.PnlCopyFrom.TabIndex = 5
        '
        'btnCopyFrom
        '
        Me.btnCopyFrom.BackColor = System.Drawing.Color.FromArgb(CType(CType(188, Byte), Integer), CType(CType(205, Byte), Integer), CType(CType(227, Byte), Integer))
        Me.btnCopyFrom.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnCopyFrom.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnCopyFrom.ForeColor = System.Drawing.Color.Black
        Me.btnCopyFrom.Location = New System.Drawing.Point(10, 0)
        Me.btnCopyFrom.Name = "btnCopyFrom"
        Me.btnCopyFrom.Size = New System.Drawing.Size(123, 30)
        Me.btnCopyFrom.TabIndex = 2
        Me.btnCopyFrom.Text = "Copy From"
        Me.btnCopyFrom.UseVisualStyleBackColor = False
        '
        'txtCopyItem
        '
        Me.txtCopyItem.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtCopyItem.BackColor = System.Drawing.Color.White
        Me.txtCopyItem.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtCopyItem.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtCopyItem.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCopyItem.Location = New System.Drawing.Point(140, 2)
        Me.txtCopyItem.Margin = New System.Windows.Forms.Padding(4)
        Me.txtCopyItem.Name = "txtCopyItem"
        Me.txtCopyItem.Size = New System.Drawing.Size(37, 26)
        Me.txtCopyItem.TabIndex = 1
        Me.txtCopyItem.Visible = False
        '
        'lblDispItemCode
        '
        Me.lblDispItemCode.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblDispItemCode.BackColor = System.Drawing.Color.Transparent
        Me.lblDispItemCode.Font = New System.Drawing.Font("Segoe UI", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDispItemCode.ForeColor = System.Drawing.Color.White
        Me.lblDispItemCode.Location = New System.Drawing.Point(4, 4)
        Me.lblDispItemCode.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblDispItemCode.Name = "lblDispItemCode"
        Me.lblDispItemCode.Size = New System.Drawing.Size(478, 26)
        Me.lblDispItemCode.TabIndex = 1
        Me.lblDispItemCode.Text = "display item code"
        '
        'Panel4
        '
        Me.Panel4.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel4.Location = New System.Drawing.Point(645, 31)
        Me.Panel4.Name = "Panel4"
        Me.Panel4.Size = New System.Drawing.Size(6, 617)
        Me.Panel4.TabIndex = 365
        '
        'txtItemID
        '
        Me.txtItemID.BackColor = System.Drawing.Color.White
        Me.txtItemID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtItemID.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtItemID.ForeColor = System.Drawing.Color.Black
        Me.txtItemID.Location = New System.Drawing.Point(583, 423)
        Me.txtItemID.Margin = New System.Windows.Forms.Padding(4)
        Me.txtItemID.MaxLength = 250
        Me.txtItemID.Multiline = True
        Me.txtItemID.Name = "txtItemID"
        Me.txtItemID.Size = New System.Drawing.Size(58, 26)
        Me.txtItemID.TabIndex = 22
        Me.txtItemID.Visible = False
        '
        'txtItemNO
        '
        Me.txtItemNO.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtItemNO.Location = New System.Drawing.Point(590, 456)
        Me.txtItemNO.Name = "txtItemNO"
        Me.txtItemNO.Size = New System.Drawing.Size(25, 21)
        Me.txtItemNO.TabIndex = 11
        Me.txtItemNO.Visible = False
        '
        'tbAdnlFlds
        '
        Me.tbAdnlFlds.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbAdnlFlds.Controls.Add(Me.TabPage1)
        Me.tbAdnlFlds.Location = New System.Drawing.Point(655, 34)
        Me.tbAdnlFlds.Name = "tbAdnlFlds"
        Me.tbAdnlFlds.SelectedIndex = 0
        Me.tbAdnlFlds.Size = New System.Drawing.Size(325, 612)
        Me.tbAdnlFlds.TabIndex = 275
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.vsfHdr)
        Me.TabPage1.Location = New System.Drawing.Point(4, 26)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(317, 582)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "Header Info"
        Me.TabPage1.UseVisualStyleBackColor = True
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
        Me.vsfHdr.Location = New System.Drawing.Point(3, 3)
        Me.vsfHdr.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.vsfHdr.Name = "vsfHdr"
        Me.vsfHdr.Rows.DefaultSize = 20
        Me.vsfHdr.ScrollOptions = C1.Win.C1FlexGrid.ScrollFlags.AlwaysVisible
        Me.vsfHdr.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfHdr.Size = New System.Drawing.Size(311, 576)
        Me.vsfHdr.StyleInfo = resources.GetString("vsfHdr.StyleInfo")
        Me.vsfHdr.TabIndex = 0
        Me.vsfHdr.Tag = "DSGR"
        '
        'pnlToolTip
        '
        Me.pnlToolTip.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.pnlToolTip.BackColor = System.Drawing.Color.Transparent
        Me.pnlToolTip.Controls.Add(Me.PictureBox1)
        Me.pnlToolTip.ForeColor = System.Drawing.SystemColors.ControlText
        Me.pnlToolTip.Location = New System.Drawing.Point(4, 655)
        Me.pnlToolTip.Name = "pnlToolTip"
        Me.pnlToolTip.Size = New System.Drawing.Size(541, 39)
        Me.pnlToolTip.TabIndex = 281
        Me.pnlToolTip.Visible = False
        '
        'PictureBox1
        '
        Me.PictureBox1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.PictureBox1.BackColor = System.Drawing.Color.Transparent
        Me.PictureBox1.BackgroundImage = CType(resources.GetObject("PictureBox1.BackgroundImage"), System.Drawing.Image)
        Me.PictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.PictureBox1.Location = New System.Drawing.Point(0, 0)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(45, 38)
        Me.PictureBox1.TabIndex = 217
        Me.PictureBox1.TabStop = False
        '
        'chkNonStkable
        '
        Me.chkNonStkable.AutoSize = True
        Me.chkNonStkable.BackColor = System.Drawing.Color.Transparent
        Me.chkNonStkable.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkNonStkable.ForeColor = System.Drawing.Color.Black
        Me.chkNonStkable.Location = New System.Drawing.Point(8, 371)
        Me.chkNonStkable.Margin = New System.Windows.Forms.Padding(4)
        Me.chkNonStkable.Name = "chkNonStkable"
        Me.chkNonStkable.Size = New System.Drawing.Size(132, 24)
        Me.chkNonStkable.TabIndex = 10
        Me.chkNonStkable.Tag = "NonStkable"
        Me.chkNonStkable.Text = "Non Stockable"
        Me.chkNonStkable.UseVisualStyleBackColor = False
        '
        'lblLiqCategary
        '
        Me.lblLiqCategary.AutoSize = True
        Me.lblLiqCategary.BackColor = System.Drawing.Color.Transparent
        Me.lblLiqCategary.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblLiqCategary.ForeColor = System.Drawing.Color.Black
        Me.lblLiqCategary.Location = New System.Drawing.Point(5, 214)
        Me.lblLiqCategary.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblLiqCategary.Name = "lblLiqCategary"
        Me.lblLiqCategary.Size = New System.Drawing.Size(121, 20)
        Me.lblLiqCategary.TabIndex = 287
        Me.lblLiqCategary.Tag = "M"
        Me.lblLiqCategary.Text = "Liqour Category"
        '
        'txtShortName
        '
        Me.txtShortName.BackColor = System.Drawing.Color.White
        Me.txtShortName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtShortName.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtShortName.ForeColor = System.Drawing.Color.Black
        Me.txtShortName.Location = New System.Drawing.Point(151, 105)
        Me.txtShortName.Margin = New System.Windows.Forms.Padding(4)
        Me.txtShortName.MaxLength = 250
        Me.txtShortName.Name = "txtShortName"
        Me.txtShortName.Size = New System.Drawing.Size(206, 26)
        Me.txtShortName.TabIndex = 1
        Me.txtShortName.Tag = "ITEMDesc"
        '
        'lblShortName
        '
        Me.lblShortName.AutoSize = True
        Me.lblShortName.BackColor = System.Drawing.Color.Transparent
        Me.lblShortName.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblShortName.ForeColor = System.Drawing.Color.Black
        Me.lblShortName.Location = New System.Drawing.Point(5, 107)
        Me.lblShortName.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblShortName.Name = "lblShortName"
        Me.lblShortName.Size = New System.Drawing.Size(94, 20)
        Me.lblShortName.TabIndex = 299
        Me.lblShortName.Tag = ""
        Me.lblShortName.Text = "Short Name"
        '
        'txtNo
        '
        Me.txtNo.BackColor = System.Drawing.Color.White
        Me.txtNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtNo.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtNo.ForeColor = System.Drawing.Color.Black
        Me.txtNo.Location = New System.Drawing.Point(607, 37)
        Me.txtNo.Margin = New System.Windows.Forms.Padding(4)
        Me.txtNo.MaxLength = 250
        Me.txtNo.Multiline = True
        Me.txtNo.Name = "txtNo"
        Me.txtNo.Size = New System.Drawing.Size(17, 29)
        Me.txtNo.TabIndex = 461
        Me.txtNo.Visible = False
        '
        'pnlSeries
        '
        Me.pnlSeries.BackColor = System.Drawing.Color.Transparent
        Me.pnlSeries.Controls.Add(Me.lblItemCode)
        Me.pnlSeries.Controls.Add(Me.txtItemCode)
        Me.pnlSeries.Location = New System.Drawing.Point(0, 36)
        Me.pnlSeries.Name = "pnlSeries"
        Me.pnlSeries.Size = New System.Drawing.Size(639, 30)
        Me.pnlSeries.TabIndex = 1
        '
        'cboType
        '
        Me.cboType.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboType.FormattingEnabled = True
        Me.cboType.Items.AddRange(New Object() {"Wine", "Beer", "IML", "Others"})
        Me.cboType.Location = New System.Drawing.Point(151, 217)
        Me.cboType.Name = "cboType"
        Me.cboType.Size = New System.Drawing.Size(182, 28)
        Me.cboType.TabIndex = 6
        Me.cboType.Tag = "ItemType"
        '
        'btnMstItemGrp
        '
        Me.btnMstItemGrp.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnMstItemGrp.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnMstItemGrp.ForeColor = System.Drawing.Color.Black
        Me.btnMstItemGrp.Location = New System.Drawing.Point(102, 177)
        Me.btnMstItemGrp.Name = "btnMstItemGrp"
        Me.btnMstItemGrp.Size = New System.Drawing.Size(25, 23)
        Me.btnMstItemGrp.TabIndex = 464
        Me.btnMstItemGrp.Text = "..."
        Me.btnMstItemGrp.UseVisualStyleBackColor = True
        '
        'txtHotKey
        '
        Me.txtHotKey.BackColor = System.Drawing.Color.White
        Me.txtHotKey.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtHotKey.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtHotKey.ForeColor = System.Drawing.Color.Black
        Me.txtHotKey.Location = New System.Drawing.Point(450, 104)
        Me.txtHotKey.Margin = New System.Windows.Forms.Padding(4)
        Me.txtHotKey.MaxLength = 100
        Me.txtHotKey.Name = "txtHotKey"
        Me.txtHotKey.Size = New System.Drawing.Size(188, 26)
        Me.txtHotKey.TabIndex = 2
        Me.txtHotKey.Tag = "TallyAlias"
        '
        'lblHotKey
        '
        Me.lblHotKey.AutoSize = True
        Me.lblHotKey.BackColor = System.Drawing.Color.Transparent
        Me.lblHotKey.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblHotKey.ForeColor = System.Drawing.Color.Black
        Me.lblHotKey.Location = New System.Drawing.Point(361, 105)
        Me.lblHotKey.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblHotKey.Name = "lblHotKey"
        Me.lblHotKey.Size = New System.Drawing.Size(65, 20)
        Me.lblHotKey.TabIndex = 472
        Me.lblHotKey.Tag = ""
        Me.lblHotKey.Text = "Hot Key"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.Black
        Me.Label1.Location = New System.Drawing.Point(344, 136)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(85, 20)
        Me.Label1.TabIndex = 473
        Me.Label1.Text = "HSN Code"
        '
        'txtHSNCode
        '
        Me.txtHSNCode.BackColor = System.Drawing.Color.White
        Me.txtHSNCode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtHSNCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtHSNCode.ForeColor = System.Drawing.Color.Black
        Me.txtHSNCode.Location = New System.Drawing.Point(450, 136)
        Me.txtHSNCode.Margin = New System.Windows.Forms.Padding(4)
        Me.txtHSNCode.MaxLength = 15
        Me.txtHSNCode.Multiline = True
        Me.txtHSNCode.Name = "txtHSNCode"
        Me.txtHSNCode.Size = New System.Drawing.Size(188, 28)
        Me.txtHSNCode.TabIndex = 4
        Me.txtHSNCode.Tag = "HSNCode"
        '
        'btnMstUOM
        '
        Me.btnMstUOM.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnMstUOM.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnMstUOM.ForeColor = System.Drawing.Color.Black
        Me.btnMstUOM.Location = New System.Drawing.Point(102, 142)
        Me.btnMstUOM.Name = "btnMstUOM"
        Me.btnMstUOM.Size = New System.Drawing.Size(26, 23)
        Me.btnMstUOM.TabIndex = 465
        Me.btnMstUOM.Text = "..."
        Me.btnMstUOM.TextAlign = System.Drawing.ContentAlignment.TopCenter
        Me.btnMstUOM.UseVisualStyleBackColor = True
        '
        'cboUOM
        '
        Me.cboUOM.BackColor = System.Drawing.Color.White
        Me.cboUOM.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.cboUOM.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboUOM.ForeColor = System.Drawing.Color.Black
        Me.cboUOM.FormattingEnabled = True
        Me.cboUOM.Location = New System.Drawing.Point(151, 139)
        Me.cboUOM.Margin = New System.Windows.Forms.Padding(4)
        Me.cboUOM.Name = "cboUOM"
        Me.cboUOM.Size = New System.Drawing.Size(186, 23)
        Me.cboUOM.TabIndex = 3
        Me.cboUOM.Tag = "UOMID"
        '
        'lblStkUOM
        '
        Me.lblStkUOM.AutoSize = True
        Me.lblStkUOM.BackColor = System.Drawing.Color.Transparent
        Me.lblStkUOM.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStkUOM.ForeColor = System.Drawing.Color.Red
        Me.lblStkUOM.Location = New System.Drawing.Point(4, 142)
        Me.lblStkUOM.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblStkUOM.Name = "lblStkUOM"
        Me.lblStkUOM.Size = New System.Drawing.Size(84, 20)
        Me.lblStkUOM.TabIndex = 289
        Me.lblStkUOM.Tag = "M"
        Me.lblStkUOM.Text = "UOM [Mst]"
        '
        'vsfAlias
        '
        Me.vsfAlias.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None
        Me.vsfAlias.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle
        Me.vsfAlias.ColumnInfo = "1,0,0,0,0,120,Columns:"
        Me.vsfAlias.ExtendLastCol = True
        Me.vsfAlias.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.vsfAlias.KeyActionEnter = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross
        Me.vsfAlias.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross
        Me.vsfAlias.Location = New System.Drawing.Point(334, 214)
        Me.vsfAlias.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.vsfAlias.Name = "vsfAlias"
        Me.vsfAlias.Rows.DefaultSize = 24
        Me.vsfAlias.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfAlias.Size = New System.Drawing.Size(296, 166)
        Me.vsfAlias.StyleInfo = resources.GetString("vsfAlias.StyleInfo")
        Me.vsfAlias.TabIndex = 0
        Me.vsfAlias.Tag = ""
        '
        'PnlAttach
        '
        Me.PnlAttach.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.PnlAttach.BackColor = System.Drawing.Color.Transparent
        Me.PnlAttach.Controls.Add(Me.vsfFiles)
        Me.PnlAttach.Controls.Add(Me.btnFileAttach)
        Me.PnlAttach.Location = New System.Drawing.Point(5, 519)
        Me.PnlAttach.Name = "PnlAttach"
        Me.PnlAttach.Size = New System.Drawing.Size(314, 114)
        Me.PnlAttach.TabIndex = 481
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
        Me.vsfFiles.Location = New System.Drawing.Point(1, 30)
        Me.vsfFiles.Name = "vsfFiles"
        Me.vsfFiles.Rows.Count = 2
        Me.vsfFiles.Rows.DefaultSize = 21
        Me.vsfFiles.Rows.Fixed = 0
        Me.vsfFiles.Rows.GlyphRow = 0
        Me.vsfFiles.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfFiles.ShowCellLabels = True
        Me.vsfFiles.Size = New System.Drawing.Size(311, 82)
        Me.vsfFiles.StyleInfo = resources.GetString("vsfFiles.StyleInfo")
        Me.vsfFiles.TabIndex = 0
        Me.vsfFiles.Tag = "View"
        Me.vsfFiles.VisualStyle = C1.Win.C1FlexGrid.VisualStyle.Office2007Blue
        '
        'pnlReOrder
        '
        Me.pnlReOrder.Controls.Add(Me.txtROL)
        Me.pnlReOrder.Controls.Add(Me.Label21)
        Me.pnlReOrder.Controls.Add(Me.Label20)
        Me.pnlReOrder.Controls.Add(Me.txtROQ)
        Me.pnlReOrder.Location = New System.Drawing.Point(9, 429)
        Me.pnlReOrder.Name = "pnlReOrder"
        Me.pnlReOrder.Size = New System.Drawing.Size(559, 70)
        Me.pnlReOrder.TabIndex = 484
        Me.pnlReOrder.Visible = False
        '
        'txtROL
        '
        Me.txtROL.BackColor = System.Drawing.Color.White
        Me.txtROL.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtROL.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtROL.ForeColor = System.Drawing.Color.Black
        Me.txtROL.Location = New System.Drawing.Point(83, 4)
        Me.txtROL.Margin = New System.Windows.Forms.Padding(4)
        Me.txtROL.MaxLength = 15
        Me.txtROL.Name = "txtROL"
        Me.txtROL.Size = New System.Drawing.Size(126, 26)
        Me.txtROL.TabIndex = 1
        Me.txtROL.Tag = "ROL"
        '
        'Label21
        '
        Me.Label21.AutoSize = True
        Me.Label21.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label21.ForeColor = System.Drawing.Color.Black
        Me.Label21.Location = New System.Drawing.Point(5, 7)
        Me.Label21.Name = "Label21"
        Me.Label21.Size = New System.Drawing.Size(42, 20)
        Me.Label21.TabIndex = 14
        Me.Label21.Text = "ROL"
        '
        'Label20
        '
        Me.Label20.AutoSize = True
        Me.Label20.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label20.ForeColor = System.Drawing.Color.Black
        Me.Label20.Location = New System.Drawing.Point(5, 35)
        Me.Label20.Name = "Label20"
        Me.Label20.Size = New System.Drawing.Size(45, 20)
        Me.Label20.TabIndex = 15
        Me.Label20.Text = "ROQ"
        '
        'txtROQ
        '
        Me.txtROQ.BackColor = System.Drawing.Color.White
        Me.txtROQ.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtROQ.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtROQ.ForeColor = System.Drawing.Color.Black
        Me.txtROQ.Location = New System.Drawing.Point(83, 35)
        Me.txtROQ.Margin = New System.Windows.Forms.Padding(4)
        Me.txtROQ.MaxLength = 15
        Me.txtROQ.Name = "txtROQ"
        Me.txtROQ.Size = New System.Drawing.Size(126, 26)
        Me.txtROQ.TabIndex = 1
        Me.txtROQ.Tag = "ROQ"
        '
        'lblBottPerCases
        '
        Me.lblBottPerCases.AutoSize = True
        Me.lblBottPerCases.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!)
        Me.lblBottPerCases.ForeColor = System.Drawing.Color.Black
        Me.lblBottPerCases.Location = New System.Drawing.Point(5, 263)
        Me.lblBottPerCases.Name = "lblBottPerCases"
        Me.lblBottPerCases.Size = New System.Drawing.Size(100, 20)
        Me.lblBottPerCases.TabIndex = 485
        Me.lblBottPerCases.Text = "Bottle/Cases"
        '
        'txtBotPerCases
        '
        Me.txtBotPerCases.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtBotPerCases.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!)
        Me.txtBotPerCases.Location = New System.Drawing.Point(150, 263)
        Me.txtBotPerCases.MaxLength = 10
        Me.txtBotPerCases.Name = "txtBotPerCases"
        Me.txtBotPerCases.Size = New System.Drawing.Size(182, 26)
        Me.txtBotPerCases.TabIndex = 7
        '
        'svtBtnCtrl
        '
        Me.svtBtnCtrl.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.svtBtnCtrl.BackColor = System.Drawing.Color.FromArgb(CType(CType(188, Byte), Integer), CType(CType(205, Byte), Integer), CType(CType(227, Byte), Integer))
        Me.svtBtnCtrl.CausesValidation = False
        Me.svtBtnCtrl.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.svtBtnCtrl.LayoutStyle = vLQMS.svtModule.typeLayoutStyles.Horizontal
        Me.svtBtnCtrl.Location = New System.Drawing.Point(-5, 653)
        Me.svtBtnCtrl.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.svtBtnCtrl.Name = "svtBtnCtrl"
        Me.svtBtnCtrl.Size = New System.Drawing.Size(948, 42)
        Me.svtBtnCtrl.TabIndex = 0
        '
        'frmMstItem
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 17.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(188, Byte), Integer), CType(CType(205, Byte), Integer), CType(CType(227, Byte), Integer))
        Me.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.ClientSize = New System.Drawing.Size(985, 695)
        Me.Controls.Add(Me.txtBotPerCases)
        Me.Controls.Add(Me.lblBottPerCases)
        Me.Controls.Add(Me.pnlReOrder)
        Me.Controls.Add(Me.btnRemoveSupp)
        Me.Controls.Add(Me.vsfAlias)
        Me.Controls.Add(Me.btnMstUOM)
        Me.Controls.Add(Me.PnlAttach)
        Me.Controls.Add(Me.pnlSeries)
        Me.Controls.Add(Me.cboUOM)
        Me.Controls.Add(Me.lblStkUOM)
        Me.Controls.Add(Me.txtHSNCode)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.txtHotKey)
        Me.Controls.Add(Me.lblHotKey)
        Me.Controls.Add(Me.btnMstItemGrp)
        Me.Controls.Add(Me.cboType)
        Me.Controls.Add(Me.txtNo)
        Me.Controls.Add(Me.vsfSearch)
        Me.Controls.Add(Me.txtShortName)
        Me.Controls.Add(Me.lblShortName)
        Me.Controls.Add(Me.lblLiqCategary)
        Me.Controls.Add(Me.chkNonStkable)
        Me.Controls.Add(Me.pnlToolTip)
        Me.Controls.Add(Me.tbAdnlFlds)
        Me.Controls.Add(Me.txtItemID)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.cboIG)
        Me.Controls.Add(Me.lblItemGrp)
        Me.Controls.Add(Me.Panel3)
        Me.Controls.Add(Me.txtItemName)
        Me.Controls.Add(Me.lblItemName)
        Me.Controls.Add(Me.lblTotalRecords)
        Me.Controls.Add(Me.txtInActiveRem)
        Me.Controls.Add(Me.chkInactive)
        Me.Controls.Add(Me.txtItemNO)
        Me.Controls.Add(Me.Panel4)
        Me.Controls.Add(Me.svtBtnCtrl)
        Me.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ForeColor = System.Drawing.Color.Transparent
        Me.Location = New System.Drawing.Point(11, 52)
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.MinimumSize = New System.Drawing.Size(932, 720)
        Me.Name = "frmMstItem"
        Me.ShowIcon = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = " Item Master"
        Me.pnlSearch.ResumeLayout(False)
        Me.pnlSearch.PerformLayout()
        CType(Me.vsfSearch, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel3.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        Me.PnlCopyFrom.ResumeLayout(False)
        Me.PnlCopyFrom.PerformLayout()
        Me.tbAdnlFlds.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        CType(Me.vsfHdr, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlToolTip.ResumeLayout(False)
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlSeries.ResumeLayout(False)
        Me.pnlSeries.PerformLayout()
        CType(Me.vsfAlias, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PnlAttach.ResumeLayout(False)
        CType(Me.vsfFiles, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlReOrder.ResumeLayout(False)
        Me.pnlReOrder.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents txtSearch As System.Windows.Forms.TextBox
    Friend WithEvents txtInActiveRem As System.Windows.Forms.TextBox
    Friend WithEvents chkInactive As System.Windows.Forms.CheckBox
    Friend WithEvents svtFileAttach As vLQMS.svtFileCtrl
    Friend WithEvents vsfSearch As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents lblTotalRecords As System.Windows.Forms.Label
    Friend WithEvents pnlSearch As System.Windows.Forms.Panel
    Friend WithEvents lblItemCode As System.Windows.Forms.Label
    Friend WithEvents txtItemCode As System.Windows.Forms.TextBox
    Private WithEvents tt As System.Windows.Forms.ToolTip
    Friend WithEvents txtItemName As System.Windows.Forms.TextBox
    Friend WithEvents lblItemName As System.Windows.Forms.Label
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
    Friend WithEvents cboIG As System.Windows.Forms.ComboBox
    Friend WithEvents lblItemGrp As System.Windows.Forms.Label
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents Panel4 As System.Windows.Forms.Panel
    Friend WithEvents lblDispItemCode As System.Windows.Forms.Label
    Friend WithEvents svtBtnCtrl As vLQMS.btnCtrl
    Friend WithEvents txtItemID As System.Windows.Forms.TextBox
    Friend WithEvents PnlCopyFrom As System.Windows.Forms.Panel
    Friend WithEvents txtCopyItem As System.Windows.Forms.TextBox
    Friend WithEvents btnCopyFrom As System.Windows.Forms.Button
    Friend WithEvents txtItemNO As System.Windows.Forms.TextBox
    Public WithEvents tbAdnlFlds As System.Windows.Forms.TabControl
    Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
    Public WithEvents vsfHdr As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents pnlToolTip As Panel
    Friend WithEvents lblToolTip As Label
    Friend WithEvents PictureBox1 As PictureBox
    Friend WithEvents chkNonStkable As CheckBox
    Friend WithEvents lblLiqCategary As Label
    Friend WithEvents txtShortName As TextBox
    Friend WithEvents lblShortName As Label
    Friend WithEvents lblSearchImage As Label
    Friend WithEvents lblSearch As Label
    Friend WithEvents txtNo As TextBox
    Friend WithEvents pnlSeries As Panel
    Friend WithEvents cboType As ComboBox
    Friend WithEvents btnMstItemGrp As Button
    Friend WithEvents txtHotKey As TextBox
    Friend WithEvents lblHotKey As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents txtHSNCode As TextBox
    Friend WithEvents cboUOM As ComboBox
    Friend WithEvents lblStkUOM As Label
    Friend WithEvents btnMstUOM As Button
    Friend WithEvents btnFileAttach As CButton
    Friend WithEvents PnlAttach As Panel
    Public WithEvents vsfFiles As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents btnRemoveSupp As Button
    Public WithEvents vsfAlias As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents pnlReOrder As Panel
    Friend WithEvents txtROL As TextBox
    Friend WithEvents Label21 As Label
    Friend WithEvents Label20 As Label
    Friend WithEvents txtROQ As TextBox
    Friend WithEvents lblBottPerCases As Label
    Friend WithEvents txtBotPerCases As TextBox
End Class
