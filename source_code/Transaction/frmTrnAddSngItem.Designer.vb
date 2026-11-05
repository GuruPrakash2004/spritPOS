<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmTrnAddSngItem
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmTrnAddSngItem))
        Me.lblSearch = New System.Windows.Forms.Label()
        Me.txtSearch = New System.Windows.Forms.TextBox()
        Me.lblPresentStock = New System.Windows.Forms.Label()
        Me.lblOrderLevel = New System.Windows.Forms.Label()
        Me.lblMinLevel = New System.Windows.Forms.Label()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.btnOK = New System.Windows.Forms.Button()
        Me.btnClear = New System.Windows.Forms.Button()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.vsfView = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.txtDummy = New System.Windows.Forms.TextBox()
        Me.lblType = New System.Windows.Forms.Label()
        Me.lblInfo = New System.Windows.Forms.Label()
        Me.lblInformation = New System.Windows.Forms.Label()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.lblSalesPer = New System.Windows.Forms.Label()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.txtPurDummy = New System.Windows.Forms.TextBox()
        Me.txtItemType = New System.Windows.Forms.TextBox()
        Me.pnlBottles = New System.Windows.Forms.Panel()
        Me.txtCases = New System.Windows.Forms.TextBox()
        Me.lblCases = New System.Windows.Forms.Label()
        Me.lblLooseBottles = New System.Windows.Forms.Label()
        Me.lblTotalBottles = New System.Windows.Forms.Label()
        Me.txtTotBot = New System.Windows.Forms.TextBox()
        Me.txtLooseBot = New System.Windows.Forms.TextBox()
        Me.pnlRateAmt = New System.Windows.Forms.Panel()
        Me.txtAromedPer = New System.Windows.Forms.TextBox()
        Me.lblAroedPer = New System.Windows.Forms.Label()
        Me.txtPurRate = New System.Windows.Forms.TextBox()
        Me.lblPurchaseRate = New System.Windows.Forms.Label()
        Me.lblSellingPrice = New System.Windows.Forms.Label()
        Me.txtAmt = New System.Windows.Forms.TextBox()
        Me.txtSellPrice = New System.Windows.Forms.TextBox()
        Me.lblAmount = New System.Windows.Forms.Label()
        Me.txtItemInfo = New System.Windows.Forms.TextBox()
        Me.txtItemName = New System.Windows.Forms.TextBox()
        Me.txtBotPerCases = New System.Windows.Forms.TextBox()
        Me.lblItemInfo = New System.Windows.Forms.Label()
        CType(Me.vsfView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel1.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.pnlBottles.SuspendLayout()
        Me.pnlRateAmt.SuspendLayout()
        Me.SuspendLayout()
        '
        'lblSearch
        '
        Me.lblSearch.AutoSize = True
        Me.lblSearch.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSearch.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblSearch.Location = New System.Drawing.Point(8, 12)
        Me.lblSearch.Name = "lblSearch"
        Me.lblSearch.Size = New System.Drawing.Size(60, 20)
        Me.lblSearch.TabIndex = 0
        Me.lblSearch.Text = "Search"
        '
        'txtSearch
        '
        Me.txtSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtSearch.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSearch.Location = New System.Drawing.Point(117, 8)
        Me.txtSearch.Name = "txtSearch"
        Me.txtSearch.Size = New System.Drawing.Size(540, 29)
        Me.txtSearch.TabIndex = 1
        '
        'lblPresentStock
        '
        Me.lblPresentStock.AutoSize = True
        Me.lblPresentStock.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPresentStock.ForeColor = System.Drawing.Color.Black
        Me.lblPresentStock.Location = New System.Drawing.Point(8, 37)
        Me.lblPresentStock.Name = "lblPresentStock"
        Me.lblPresentStock.Size = New System.Drawing.Size(109, 20)
        Me.lblPresentStock.TabIndex = 0
        Me.lblPresentStock.Tag = "R"
        Me.lblPresentStock.Text = "Present Stock"
        '
        'lblOrderLevel
        '
        Me.lblOrderLevel.AutoSize = True
        Me.lblOrderLevel.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblOrderLevel.ForeColor = System.Drawing.Color.Black
        Me.lblOrderLevel.Location = New System.Drawing.Point(8, 64)
        Me.lblOrderLevel.Name = "lblOrderLevel"
        Me.lblOrderLevel.Size = New System.Drawing.Size(90, 20)
        Me.lblOrderLevel.TabIndex = 1
        Me.lblOrderLevel.Tag = "R"
        Me.lblOrderLevel.Text = "Order Level"
        '
        'lblMinLevel
        '
        Me.lblMinLevel.AutoSize = True
        Me.lblMinLevel.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMinLevel.ForeColor = System.Drawing.Color.Black
        Me.lblMinLevel.Location = New System.Drawing.Point(8, 91)
        Me.lblMinLevel.Name = "lblMinLevel"
        Me.lblMinLevel.Size = New System.Drawing.Size(69, 20)
        Me.lblMinLevel.TabIndex = 2
        Me.lblMinLevel.Tag = "R"
        Me.lblMinLevel.Text = "Min level"
        '
        'btnOK
        '
        Me.btnOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnOK.ForeColor = System.Drawing.Color.Black
        Me.btnOK.Location = New System.Drawing.Point(562, 355)
        Me.btnOK.Margin = New System.Windows.Forms.Padding(4)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(95, 31)
        Me.btnOK.TabIndex = 4
        Me.btnOK.Text = "&OK"
        Me.ToolTip1.SetToolTip(Me.btnOK, "Shortcut: Alt+O")
        Me.btnOK.UseVisualStyleBackColor = True
        '
        'btnClear
        '
        Me.btnClear.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClear.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnClear.ForeColor = System.Drawing.Color.Black
        Me.btnClear.Location = New System.Drawing.Point(665, 355)
        Me.btnClear.Margin = New System.Windows.Forms.Padding(4)
        Me.btnClear.Name = "btnClear"
        Me.btnClear.Size = New System.Drawing.Size(95, 31)
        Me.btnClear.TabIndex = 5
        Me.btnClear.Text = "C&lear"
        Me.ToolTip1.SetToolTip(Me.btnClear, "Shortcut: Alt+L")
        Me.btnClear.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnCancel.ForeColor = System.Drawing.Color.Black
        Me.btnCancel.Location = New System.Drawing.Point(767, 355)
        Me.btnCancel.Margin = New System.Windows.Forms.Padding(4)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(95, 31)
        Me.btnCancel.TabIndex = 6
        Me.btnCancel.Text = "&Cancel"
        Me.ToolTip1.SetToolTip(Me.btnCancel, "Shortcut: Alt+C")
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'vsfView
        '
        Me.vsfView.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfView.ColumnInfo = "10,0,0,0,0,120,Columns:"
        Me.vsfView.ExtendLastCol = True
        Me.vsfView.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.vsfView.KeyActionEnter = C1.Win.C1FlexGrid.KeyActionEnum.None
        Me.vsfView.Location = New System.Drawing.Point(5, 36)
        Me.vsfView.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.vsfView.Name = "vsfView"
        Me.vsfView.Rows.DefaultSize = 24
        Me.vsfView.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfView.Size = New System.Drawing.Size(864, 311)
        Me.vsfView.StyleInfo = resources.GetString("vsfView.StyleInfo")
        Me.vsfView.TabIndex = 2
        Me.vsfView.Visible = False
        '
        'txtDummy
        '
        Me.txtDummy.Location = New System.Drawing.Point(767, 63)
        Me.txtDummy.Name = "txtDummy"
        Me.txtDummy.Size = New System.Drawing.Size(30, 20)
        Me.txtDummy.TabIndex = 10
        Me.txtDummy.Visible = False
        '
        'lblType
        '
        Me.lblType.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblType.AutoSize = True
        Me.lblType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblType.ForeColor = System.Drawing.Color.Red
        Me.lblType.Location = New System.Drawing.Point(2, 390)
        Me.lblType.Name = "lblType"
        Me.lblType.Size = New System.Drawing.Size(280, 13)
        Me.lblType.TabIndex = 7
        Me.lblType.Text = "Type in Search area[Barcode/ShortName/Name/HotKey]"
        '
        'lblInfo
        '
        Me.lblInfo.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblInfo.AutoSize = True
        Me.lblInfo.ForeColor = System.Drawing.Color.Red
        Me.lblInfo.Location = New System.Drawing.Point(355, 390)
        Me.lblInfo.Name = "lblInfo"
        Me.lblInfo.Size = New System.Drawing.Size(155, 13)
        Me.lblInfo.TabIndex = 8
        Me.lblInfo.Text = "Press F12 to Save/Click on OK" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
        '
        'lblInformation
        '
        Me.lblInformation.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblInformation.AutoSize = True
        Me.lblInformation.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblInformation.ForeColor = System.Drawing.Color.Red
        Me.lblInformation.Location = New System.Drawing.Point(627, 390)
        Me.lblInformation.Name = "lblInformation"
        Me.lblInformation.Size = New System.Drawing.Size(229, 13)
        Me.lblInformation.TabIndex = 9
        Me.lblInformation.Text = "Press Escape/Click Cancel to the close screen"
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.FromArgb(CType(CType(188, Byte), Integer), CType(CType(205, Byte), Integer), CType(CType(227, Byte), Integer))
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Panel1.Controls.Add(Me.lblPresentStock)
        Me.Panel1.Controls.Add(Me.lblOrderLevel)
        Me.Panel1.Controls.Add(Me.lblMinLevel)
        Me.Panel1.Controls.Add(Me.lblSalesPer)
        Me.Panel1.ForeColor = System.Drawing.Color.White
        Me.Panel1.Location = New System.Drawing.Point(469, 118)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(388, 164)
        Me.Panel1.TabIndex = 4
        '
        'lblSalesPer
        '
        Me.lblSalesPer.AutoSize = True
        Me.lblSalesPer.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.75!)
        Me.lblSalesPer.ForeColor = System.Drawing.Color.Red
        Me.lblSalesPer.Location = New System.Drawing.Point(8, 10)
        Me.lblSalesPer.Name = "lblSalesPer"
        Me.lblSalesPer.Size = New System.Drawing.Size(46, 20)
        Me.lblSalesPer.TabIndex = 3
        Me.lblSalesPer.Tag = "M"
        Me.lblSalesPer.Text = "0.0%"
        '
        'Panel2
        '
        Me.Panel2.BackColor = System.Drawing.Color.FromArgb(CType(CType(188, Byte), Integer), CType(CType(205, Byte), Integer), CType(CType(227, Byte), Integer))
        Me.Panel2.Controls.Add(Me.txtPurDummy)
        Me.Panel2.Controls.Add(Me.txtItemType)
        Me.Panel2.Controls.Add(Me.Panel1)
        Me.Panel2.Controls.Add(Me.pnlBottles)
        Me.Panel2.Controls.Add(Me.pnlRateAmt)
        Me.Panel2.Controls.Add(Me.txtItemInfo)
        Me.Panel2.Controls.Add(Me.txtItemName)
        Me.Panel2.Controls.Add(Me.txtBotPerCases)
        Me.Panel2.Controls.Add(Me.lblItemInfo)
        Me.Panel2.Location = New System.Drawing.Point(5, 38)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(864, 285)
        Me.Panel2.TabIndex = 3
        '
        'txtPurDummy
        '
        Me.txtPurDummy.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtPurDummy.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPurDummy.Location = New System.Drawing.Point(741, 83)
        Me.txtPurDummy.Name = "txtPurDummy"
        Me.txtPurDummy.Size = New System.Drawing.Size(88, 29)
        Me.txtPurDummy.TabIndex = 8
        Me.txtPurDummy.Visible = False
        '
        'txtItemType
        '
        Me.txtItemType.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtItemType.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtItemType.Location = New System.Drawing.Point(704, 50)
        Me.txtItemType.Name = "txtItemType"
        Me.txtItemType.Size = New System.Drawing.Size(88, 29)
        Me.txtItemType.TabIndex = 11
        Me.txtItemType.Visible = False
        '
        'pnlBottles
        '
        Me.pnlBottles.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pnlBottles.Controls.Add(Me.txtCases)
        Me.pnlBottles.Controls.Add(Me.lblCases)
        Me.pnlBottles.Controls.Add(Me.lblLooseBottles)
        Me.pnlBottles.Controls.Add(Me.lblTotalBottles)
        Me.pnlBottles.Controls.Add(Me.txtTotBot)
        Me.pnlBottles.Controls.Add(Me.txtLooseBot)
        Me.pnlBottles.Location = New System.Drawing.Point(0, 118)
        Me.pnlBottles.Name = "pnlBottles"
        Me.pnlBottles.Size = New System.Drawing.Size(246, 164)
        Me.pnlBottles.TabIndex = 2
        '
        'txtCases
        '
        Me.txtCases.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtCases.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCases.Location = New System.Drawing.Point(111, 4)
        Me.txtCases.Name = "txtCases"
        Me.txtCases.Size = New System.Drawing.Size(124, 29)
        Me.txtCases.TabIndex = 1
        '
        'lblCases
        '
        Me.lblCases.AutoSize = True
        Me.lblCases.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCases.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCases.Location = New System.Drawing.Point(2, 7)
        Me.lblCases.Name = "lblCases"
        Me.lblCases.Size = New System.Drawing.Size(54, 20)
        Me.lblCases.TabIndex = 0
        Me.lblCases.Text = "Cases"
        '
        'lblLooseBottles
        '
        Me.lblLooseBottles.AutoSize = True
        Me.lblLooseBottles.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblLooseBottles.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLooseBottles.Location = New System.Drawing.Point(2, 50)
        Me.lblLooseBottles.Name = "lblLooseBottles"
        Me.lblLooseBottles.Size = New System.Drawing.Size(107, 20)
        Me.lblLooseBottles.TabIndex = 2
        Me.lblLooseBottles.Text = "Loose Bottles"
        '
        'lblTotalBottles
        '
        Me.lblTotalBottles.AutoSize = True
        Me.lblTotalBottles.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTotalBottles.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTotalBottles.Location = New System.Drawing.Point(2, 91)
        Me.lblTotalBottles.Name = "lblTotalBottles"
        Me.lblTotalBottles.Size = New System.Drawing.Size(98, 20)
        Me.lblTotalBottles.TabIndex = 4
        Me.lblTotalBottles.Text = "Total Bottles"
        '
        'txtTotBot
        '
        Me.txtTotBot.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtTotBot.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTotBot.Location = New System.Drawing.Point(111, 88)
        Me.txtTotBot.Name = "txtTotBot"
        Me.txtTotBot.Size = New System.Drawing.Size(124, 29)
        Me.txtTotBot.TabIndex = 5
        '
        'txtLooseBot
        '
        Me.txtLooseBot.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtLooseBot.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtLooseBot.Location = New System.Drawing.Point(111, 47)
        Me.txtLooseBot.Name = "txtLooseBot"
        Me.txtLooseBot.Size = New System.Drawing.Size(124, 29)
        Me.txtLooseBot.TabIndex = 3
        '
        'pnlRateAmt
        '
        Me.pnlRateAmt.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pnlRateAmt.Controls.Add(Me.txtAromedPer)
        Me.pnlRateAmt.Controls.Add(Me.lblAroedPer)
        Me.pnlRateAmt.Controls.Add(Me.txtPurRate)
        Me.pnlRateAmt.Controls.Add(Me.lblPurchaseRate)
        Me.pnlRateAmt.Controls.Add(Me.lblSellingPrice)
        Me.pnlRateAmt.Controls.Add(Me.txtAmt)
        Me.pnlRateAmt.Controls.Add(Me.txtSellPrice)
        Me.pnlRateAmt.Controls.Add(Me.lblAmount)
        Me.pnlRateAmt.Location = New System.Drawing.Point(252, 118)
        Me.pnlRateAmt.Name = "pnlRateAmt"
        Me.pnlRateAmt.Size = New System.Drawing.Size(211, 164)
        Me.pnlRateAmt.TabIndex = 3
        '
        'txtAromedPer
        '
        Me.txtAromedPer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtAromedPer.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAromedPer.Location = New System.Drawing.Point(117, 124)
        Me.txtAromedPer.Name = "txtAromedPer"
        Me.txtAromedPer.Size = New System.Drawing.Size(88, 29)
        Me.txtAromedPer.TabIndex = 7
        '
        'lblAroedPer
        '
        Me.lblAroedPer.AutoSize = True
        Me.lblAroedPer.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAroedPer.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAroedPer.Location = New System.Drawing.Point(0, 127)
        Me.lblAroedPer.Name = "lblAroedPer"
        Me.lblAroedPer.Size = New System.Drawing.Size(115, 18)
        Me.lblAroedPer.TabIndex = 6
        Me.lblAroedPer.Text = "AROED Per Bot"
        '
        'txtPurRate
        '
        Me.txtPurRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtPurRate.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPurRate.Location = New System.Drawing.Point(116, 4)
        Me.txtPurRate.Name = "txtPurRate"
        Me.txtPurRate.Size = New System.Drawing.Size(88, 29)
        Me.txtPurRate.TabIndex = 1
        '
        'lblPurchaseRate
        '
        Me.lblPurchaseRate.AutoSize = True
        Me.lblPurchaseRate.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPurchaseRate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPurchaseRate.Location = New System.Drawing.Point(-1, 7)
        Me.lblPurchaseRate.Name = "lblPurchaseRate"
        Me.lblPurchaseRate.Size = New System.Drawing.Size(115, 20)
        Me.lblPurchaseRate.TabIndex = 0
        Me.lblPurchaseRate.Tag = "M"
        Me.lblPurchaseRate.Text = "Purchase Rate"
        '
        'lblSellingPrice
        '
        Me.lblSellingPrice.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblSellingPrice.AutoSize = True
        Me.lblSellingPrice.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSellingPrice.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblSellingPrice.Location = New System.Drawing.Point(-1, 50)
        Me.lblSellingPrice.Name = "lblSellingPrice"
        Me.lblSellingPrice.Size = New System.Drawing.Size(95, 20)
        Me.lblSellingPrice.TabIndex = 2
        Me.lblSellingPrice.Text = "Selling Price"
        '
        'txtAmt
        '
        Me.txtAmt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtAmt.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAmt.Location = New System.Drawing.Point(116, 88)
        Me.txtAmt.Name = "txtAmt"
        Me.txtAmt.Size = New System.Drawing.Size(88, 29)
        Me.txtAmt.TabIndex = 5
        '
        'txtSellPrice
        '
        Me.txtSellPrice.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtSellPrice.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSellPrice.Location = New System.Drawing.Point(116, 47)
        Me.txtSellPrice.Name = "txtSellPrice"
        Me.txtSellPrice.Size = New System.Drawing.Size(88, 29)
        Me.txtSellPrice.TabIndex = 3
        '
        'lblAmount
        '
        Me.lblAmount.AutoSize = True
        Me.lblAmount.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAmount.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAmount.Location = New System.Drawing.Point(-1, 91)
        Me.lblAmount.Name = "lblAmount"
        Me.lblAmount.Size = New System.Drawing.Size(65, 20)
        Me.lblAmount.TabIndex = 4
        Me.lblAmount.Text = "Amount"
        '
        'txtItemInfo
        '
        Me.txtItemInfo.BackColor = System.Drawing.Color.White
        Me.txtItemInfo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtItemInfo.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtItemInfo.ForeColor = System.Drawing.SystemColors.ControlText
        Me.txtItemInfo.Location = New System.Drawing.Point(111, 5)
        Me.txtItemInfo.Multiline = True
        Me.txtItemInfo.Name = "txtItemInfo"
        Me.txtItemInfo.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.txtItemInfo.Size = New System.Drawing.Size(541, 107)
        Me.txtItemInfo.TabIndex = 1
        '
        'txtItemName
        '
        Me.txtItemName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtItemName.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtItemName.ForeColor = System.Drawing.Color.Black
        Me.txtItemName.Location = New System.Drawing.Point(589, 25)
        Me.txtItemName.Name = "txtItemName"
        Me.txtItemName.ReadOnly = True
        Me.txtItemName.Size = New System.Drawing.Size(34, 29)
        Me.txtItemName.TabIndex = 0
        Me.txtItemName.Visible = False
        '
        'txtBotPerCases
        '
        Me.txtBotPerCases.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtBotPerCases.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtBotPerCases.Location = New System.Drawing.Point(541, 25)
        Me.txtBotPerCases.Name = "txtBotPerCases"
        Me.txtBotPerCases.Size = New System.Drawing.Size(43, 29)
        Me.txtBotPerCases.TabIndex = 1
        Me.txtBotPerCases.Visible = False
        '
        'lblItemInfo
        '
        Me.lblItemInfo.AutoSize = True
        Me.lblItemInfo.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblItemInfo.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblItemInfo.Location = New System.Drawing.Point(2, 9)
        Me.lblItemInfo.Name = "lblItemInfo"
        Me.lblItemInfo.Size = New System.Drawing.Size(73, 20)
        Me.lblItemInfo.TabIndex = 0
        Me.lblItemInfo.Tag = "R"
        Me.lblItemInfo.Text = "Item Info"
        '
        'frmTrnAddSngItem
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(188, Byte), Integer), CType(CType(205, Byte), Integer), CType(CType(227, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(872, 406)
        Me.Controls.Add(Me.lblInfo)
        Me.Controls.Add(Me.lblInformation)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.lblType)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnClear)
        Me.Controls.Add(Me.btnOK)
        Me.Controls.Add(Me.txtSearch)
        Me.Controls.Add(Me.txtDummy)
        Me.Controls.Add(Me.lblSearch)
        Me.Controls.Add(Me.vsfView)
        Me.ForeColor = System.Drawing.Color.White
        Me.MinimumSize = New System.Drawing.Size(797, 272)
        Me.Name = "frmTrnAddSngItem"
        Me.ShowIcon = False
        Me.Tag = "R"
        Me.Text = "Add Item Into Purchase Bill"
        CType(Me.vsfView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.pnlBottles.ResumeLayout(False)
        Me.pnlBottles.PerformLayout()
        Me.pnlRateAmt.ResumeLayout(False)
        Me.pnlRateAmt.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents lblSearch As Label
    Friend WithEvents txtSearch As TextBox
    Friend WithEvents lblPresentStock As Label
    Friend WithEvents lblOrderLevel As Label
    Friend WithEvents lblMinLevel As Label
    Friend WithEvents ToolTip1 As ToolTip
    Friend WithEvents vsfView As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents btnOK As Button
    Friend WithEvents btnClear As Button
    Friend WithEvents btnCancel As Button
    Friend WithEvents txtDummy As TextBox
    Friend WithEvents lblType As Label
    Friend WithEvents lblInfo As Label
    Friend WithEvents lblInformation As Label
    Friend WithEvents Panel1 As Panel
    Friend WithEvents Panel2 As Panel
    Friend WithEvents txtItemInfo As TextBox
    Friend WithEvents lblSalesPer As Label
    Friend WithEvents txtAmt As TextBox
    Friend WithEvents lblAmount As Label
    Friend WithEvents txtSellPrice As TextBox
    Friend WithEvents lblSellingPrice As Label
    Friend WithEvents lblPurchaseRate As Label
    Friend WithEvents txtPurRate As TextBox
    Friend WithEvents txtTotBot As TextBox
    Friend WithEvents txtLooseBot As TextBox
    Friend WithEvents txtCases As TextBox
    Friend WithEvents txtItemName As TextBox
    Friend WithEvents txtBotPerCases As TextBox
    Friend WithEvents lblTotalBottles As Label
    Friend WithEvents lblLooseBottles As Label
    Friend WithEvents lblCases As Label
    Friend WithEvents lblItemInfo As Label
    Friend WithEvents pnlRateAmt As Panel
    Friend WithEvents pnlBottles As Panel
    Friend WithEvents txtItemType As TextBox
    Friend WithEvents txtAromedPer As TextBox
    Friend WithEvents lblAroedPer As Label
    Friend WithEvents txtPurDummy As TextBox
End Class
