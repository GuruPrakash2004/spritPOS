<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmTrnQuickBillNonTouch
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
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmTrnQuickBillNonTouch))
        Dim CBlendItems1 As vLQMS.cBlendItems = New vLQMS.cBlendItems()
        Dim CBlendItems2 As vLQMS.cBlendItems = New vLQMS.cBlendItems()
        Dim CBlendItems3 As vLQMS.cBlendItems = New vLQMS.cBlendItems()
        Dim CBlendItems4 As vLQMS.cBlendItems = New vLQMS.cBlendItems()
        Dim CBlendItems5 As vLQMS.cBlendItems = New vLQMS.cBlendItems()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.vsfColFlds = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.vsfBC = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.vsfHdr = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.pnlAmt = New System.Windows.Forms.Panel()
        Me.lblTotalCaption = New System.Windows.Forms.Label()
        Me.lblTotal = New System.Windows.Forms.Label()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.dtDocDate = New System.Windows.Forms.DateTimePicker()
        Me.txtDocNo = New System.Windows.Forms.TextBox()
        Me.txtNo = New System.Windows.Forms.TextBox()
        Me.cboPfx = New System.Windows.Forms.ComboBox()
        Me.vsfOptionsBC = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.vsfOptions = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.lblCounter = New System.Windows.Forms.Label()
        Me.lblDatetime = New System.Windows.Forms.Label()
        Me.lblParcel = New System.Windows.Forms.Label()
        Me.txtTblCode = New System.Windows.Forms.TextBox()
        Me.lblTbl = New System.Windows.Forms.Label()
        Me.btnOptions = New C1.Win.C1Input.C1SplitButton()
        Me.DropDownItem4 = New C1.Win.C1Input.DropDownItem()
        Me.DropDownItem5 = New C1.Win.C1Input.DropDownItem()
        Me.btnAlias = New C1.Win.C1Input.DropDownItem()
        Me.FlowLayoutPanel1 = New System.Windows.Forms.FlowLayoutPanel()
        Me.btnClose = New vLQMS.CButton()
        Me.btnSave = New vLQMS.CButton()
        Me.btnClearItems = New vLQMS.CButton()
        Me.BtnParcel = New vLQMS.CButton()
        Me.btnDelvryAdd = New vLQMS.CButton()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.cboMenuGroup = New System.Windows.Forms.ComboBox()
        Me.Panel4 = New System.Windows.Forms.Panel()
        Me.txtQty = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtItemCode = New System.Windows.Forms.TextBox()
        Me.lblItemCaption = New System.Windows.Forms.Label()
        Me.lblItemName = New System.Windows.Forms.Label()
        Me.btnAddItem = New System.Windows.Forms.Button()
        Me.vsfMenu = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.vsfDet = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.Panel1.SuspendLayout()
        CType(Me.vsfColFlds, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.vsfBC, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.vsfHdr, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlAmt.SuspendLayout()
        Me.Panel2.SuspendLayout()
        CType(Me.vsfOptionsBC, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.vsfOptions, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.FlowLayoutPanel1.SuspendLayout()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        Me.Panel4.SuspendLayout()
        CType(Me.vsfMenu, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.Gray
        Me.Panel1.Controls.Add(Me.vsfColFlds)
        Me.Panel1.Controls.Add(Me.vsfBC)
        Me.Panel1.Controls.Add(Me.vsfHdr)
        Me.Panel1.Controls.Add(Me.pnlAmt)
        Me.Panel1.Controls.Add(Me.TextBox1)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(218, 468)
        Me.Panel1.TabIndex = 18
        '
        'vsfColFlds
        '
        Me.vsfColFlds.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfColFlds.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.vsfColFlds.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.None
        Me.vsfColFlds.ColumnInfo = "2,0,0,0,0,130,Columns:"
        Me.vsfColFlds.Font = New System.Drawing.Font("Segoe UI", 11.0!)
        Me.vsfColFlds.ForeColor = System.Drawing.Color.Black
        Me.vsfColFlds.HighLight = C1.Win.C1FlexGrid.HighLightEnum.WithFocus
        Me.vsfColFlds.Location = New System.Drawing.Point(129, 202)
        Me.vsfColFlds.Name = "vsfColFlds"
        Me.vsfColFlds.Rows.Count = 2
        Me.vsfColFlds.Rows.DefaultSize = 26
        Me.vsfColFlds.Size = New System.Drawing.Size(37, 74)
        Me.vsfColFlds.StyleInfo = resources.GetString("vsfColFlds.StyleInfo")
        Me.vsfColFlds.TabIndex = 54
        Me.vsfColFlds.Tag = ""
        '
        'vsfBC
        '
        Me.vsfBC.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfBC.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.vsfBC.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.None
        Me.vsfBC.ColumnInfo = "2,0,0,0,0,130,Columns:"
        Me.vsfBC.Font = New System.Drawing.Font("Segoe UI", 11.0!)
        Me.vsfBC.ForeColor = System.Drawing.Color.Black
        Me.vsfBC.HighLight = C1.Win.C1FlexGrid.HighLightEnum.WithFocus
        Me.vsfBC.Location = New System.Drawing.Point(6, 203)
        Me.vsfBC.Name = "vsfBC"
        Me.vsfBC.Rows.Count = 4
        Me.vsfBC.Rows.DefaultSize = 26
        Me.vsfBC.Size = New System.Drawing.Size(55, 48)
        Me.vsfBC.StyleInfo = resources.GetString("vsfBC.StyleInfo")
        Me.vsfBC.TabIndex = 52
        Me.vsfBC.Tag = ""
        Me.vsfBC.Visible = False
        '
        'vsfHdr
        '
        Me.vsfHdr.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfHdr.AutoSearch = C1.Win.C1FlexGrid.AutoSearchEnum.FromCursor
        Me.vsfHdr.BackColor = System.Drawing.Color.Black
        Me.vsfHdr.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.None
        Me.vsfHdr.ColumnInfo = "2,0,0,0,0,130,Columns:"
        Me.vsfHdr.Font = New System.Drawing.Font("Segoe UI", 11.0!)
        Me.vsfHdr.ForeColor = System.Drawing.Color.White
        Me.vsfHdr.HighLight = C1.Win.C1FlexGrid.HighLightEnum.WithFocus
        Me.vsfHdr.Location = New System.Drawing.Point(7, 13)
        Me.vsfHdr.Name = "vsfHdr"
        Me.vsfHdr.Rows.Count = 4
        Me.vsfHdr.Rows.DefaultSize = 26
        Me.vsfHdr.Size = New System.Drawing.Size(207, 399)
        Me.vsfHdr.StyleInfo = resources.GetString("vsfHdr.StyleInfo")
        Me.vsfHdr.TabIndex = 53
        Me.vsfHdr.Tag = "DSGR"
        '
        'pnlAmt
        '
        Me.pnlAmt.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnlAmt.BackColor = System.Drawing.Color.Black
        Me.pnlAmt.Controls.Add(Me.lblTotalCaption)
        Me.pnlAmt.Controls.Add(Me.lblTotal)
        Me.pnlAmt.Location = New System.Drawing.Point(6, 418)
        Me.pnlAmt.Name = "pnlAmt"
        Me.pnlAmt.Size = New System.Drawing.Size(208, 48)
        Me.pnlAmt.TabIndex = 52
        '
        'lblTotalCaption
        '
        Me.lblTotalCaption.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblTotalCaption.AutoSize = True
        Me.lblTotalCaption.Font = New System.Drawing.Font("Bookman Old Style", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTotalCaption.ForeColor = System.Drawing.Color.Silver
        Me.lblTotalCaption.Location = New System.Drawing.Point(13, 8)
        Me.lblTotalCaption.Name = "lblTotalCaption"
        Me.lblTotalCaption.Size = New System.Drawing.Size(78, 22)
        Me.lblTotalCaption.TabIndex = 46
        Me.lblTotalCaption.Text = "Total : "
        '
        'lblTotal
        '
        Me.lblTotal.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblTotal.AutoSize = True
        Me.lblTotal.Font = New System.Drawing.Font("Arial Narrow", 21.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTotal.ForeColor = System.Drawing.Color.White
        Me.lblTotal.Location = New System.Drawing.Point(152, 3)
        Me.lblTotal.Name = "lblTotal"
        Me.lblTotal.Size = New System.Drawing.Size(100, 33)
        Me.lblTotal.TabIndex = 47
        Me.lblTotal.Text = "5488.00"
        '
        'TextBox1
        '
        Me.TextBox1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TextBox1.BackColor = System.Drawing.Color.White
        Me.TextBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox1.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.TextBox1.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.TextBox1.Location = New System.Drawing.Point(2, 277)
        Me.TextBox1.Margin = New System.Windows.Forms.Padding(4)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(19, 25)
        Me.TextBox1.TabIndex = 0
        Me.TextBox1.Visible = False
        '
        'Panel2
        '
        Me.Panel2.BackColor = System.Drawing.Color.Gray
        Me.Panel2.Controls.Add(Me.dtDocDate)
        Me.Panel2.Controls.Add(Me.txtDocNo)
        Me.Panel2.Controls.Add(Me.txtNo)
        Me.Panel2.Controls.Add(Me.cboPfx)
        Me.Panel2.Controls.Add(Me.vsfOptionsBC)
        Me.Panel2.Controls.Add(Me.vsfOptions)
        Me.Panel2.Controls.Add(Me.lblCounter)
        Me.Panel2.Controls.Add(Me.lblDatetime)
        Me.Panel2.Controls.Add(Me.lblParcel)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel2.Location = New System.Drawing.Point(0, 0)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(963, 30)
        Me.Panel2.TabIndex = 19
        '
        'dtDocDate
        '
        Me.dtDocDate.CustomFormat = "dd/MM/yyyy"
        Me.dtDocDate.Enabled = False
        Me.dtDocDate.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.dtDocDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtDocDate.ImeMode = System.Windows.Forms.ImeMode.Off
        Me.dtDocDate.Location = New System.Drawing.Point(810, 0)
        Me.dtDocDate.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.dtDocDate.Name = "dtDocDate"
        Me.dtDocDate.Size = New System.Drawing.Size(112, 25)
        Me.dtDocDate.TabIndex = 62
        Me.dtDocDate.Visible = False
        '
        'txtDocNo
        '
        Me.txtDocNo.BackColor = System.Drawing.SystemColors.Control
        Me.txtDocNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtDocNo.Font = New System.Drawing.Font("Arial", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDocNo.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.txtDocNo.Location = New System.Drawing.Point(907, 0)
        Me.txtDocNo.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtDocNo.Name = "txtDocNo"
        Me.txtDocNo.ReadOnly = True
        Me.txtDocNo.Size = New System.Drawing.Size(128, 25)
        Me.txtDocNo.TabIndex = 61
        Me.txtDocNo.TabStop = False
        Me.txtDocNo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.txtDocNo.Visible = False
        '
        'txtNo
        '
        Me.txtNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtNo.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.txtNo.Location = New System.Drawing.Point(833, 2)
        Me.txtNo.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtNo.Name = "txtNo"
        Me.txtNo.Size = New System.Drawing.Size(73, 25)
        Me.txtNo.TabIndex = 60
        Me.txtNo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.txtNo.Visible = False
        '
        'cboPfx
        '
        Me.cboPfx.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.cboPfx.FormattingEnabled = True
        Me.cboPfx.Location = New System.Drawing.Point(802, 2)
        Me.cboPfx.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.cboPfx.Name = "cboPfx"
        Me.cboPfx.Size = New System.Drawing.Size(70, 25)
        Me.cboPfx.TabIndex = 59
        Me.cboPfx.Visible = False
        '
        'vsfOptionsBC
        '
        Me.vsfOptionsBC.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None
        Me.vsfOptionsBC.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfOptionsBC.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle
        Me.vsfOptionsBC.ColumnInfo = "2,1,0,0,0,120,Columns:"
        Me.vsfOptionsBC.ExtendLastCol = True
        Me.vsfOptionsBC.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.vsfOptionsBC.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveDown
        Me.vsfOptionsBC.Location = New System.Drawing.Point(1143, 0)
        Me.vsfOptionsBC.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.vsfOptionsBC.Name = "vsfOptionsBC"
        Me.vsfOptionsBC.Rows.DefaultSize = 24
        Me.vsfOptionsBC.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfOptionsBC.Size = New System.Drawing.Size(0, 45)
        Me.vsfOptionsBC.StyleInfo = resources.GetString("vsfOptionsBC.StyleInfo")
        Me.vsfOptionsBC.TabIndex = 58
        Me.vsfOptionsBC.Tag = ""
        Me.vsfOptionsBC.Visible = False
        '
        'vsfOptions
        '
        Me.vsfOptions.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None
        Me.vsfOptions.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfOptions.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle
        Me.vsfOptions.ColumnInfo = "2,1,0,0,0,120,Columns:"
        Me.vsfOptions.ExtendLastCol = True
        Me.vsfOptions.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.vsfOptions.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveDown
        Me.vsfOptions.Location = New System.Drawing.Point(1078, 0)
        Me.vsfOptions.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.vsfOptions.Name = "vsfOptions"
        Me.vsfOptions.Rows.DefaultSize = 24
        Me.vsfOptions.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfOptions.Size = New System.Drawing.Size(0, 45)
        Me.vsfOptions.StyleInfo = resources.GetString("vsfOptions.StyleInfo")
        Me.vsfOptions.TabIndex = 57
        Me.vsfOptions.Tag = ""
        Me.vsfOptions.Visible = False
        '
        'lblCounter
        '
        Me.lblCounter.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblCounter.AutoSize = True
        Me.lblCounter.Font = New System.Drawing.Font("Bookman Old Style", 14.0!, System.Drawing.FontStyle.Bold)
        Me.lblCounter.ForeColor = System.Drawing.Color.White
        Me.lblCounter.Location = New System.Drawing.Point(685, 3)
        Me.lblCounter.Name = "lblCounter"
        Me.lblCounter.Size = New System.Drawing.Size(111, 22)
        Me.lblCounter.TabIndex = 2
        Me.lblCounter.Text = "lblCounter"
        '
        'lblDatetime
        '
        Me.lblDatetime.AutoSize = True
        Me.lblDatetime.ForeColor = System.Drawing.Color.White
        Me.lblDatetime.Location = New System.Drawing.Point(6, 8)
        Me.lblDatetime.Name = "lblDatetime"
        Me.lblDatetime.Size = New System.Drawing.Size(152, 13)
        Me.lblDatetime.TabIndex = 1
        Me.lblDatetime.Text = "Date Time : 06 Nov 2015 4PM"
        '
        'lblParcel
        '
        Me.lblParcel.AutoSize = True
        Me.lblParcel.BackColor = System.Drawing.Color.Transparent
        Me.lblParcel.Font = New System.Drawing.Font("Bookman Old Style", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblParcel.ForeColor = System.Drawing.Color.Lime
        Me.lblParcel.Location = New System.Drawing.Point(193, 0)
        Me.lblParcel.Name = "lblParcel"
        Me.lblParcel.Size = New System.Drawing.Size(89, 28)
        Me.lblParcel.TabIndex = 65
        Me.lblParcel.Text = "Parcel"
        Me.lblParcel.Visible = False
        '
        'txtTblCode
        '
        Me.txtTblCode.Font = New System.Drawing.Font("Bookman Old Style", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTblCode.Location = New System.Drawing.Point(75, 6)
        Me.txtTblCode.MaxLength = 15
        Me.txtTblCode.Name = "txtTblCode"
        Me.txtTblCode.Size = New System.Drawing.Size(101, 32)
        Me.txtTblCode.TabIndex = 66
        Me.txtTblCode.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'lblTbl
        '
        Me.lblTbl.AutoSize = True
        Me.lblTbl.Font = New System.Drawing.Font("Bookman Old Style", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTbl.ForeColor = System.Drawing.Color.White
        Me.lblTbl.Location = New System.Drawing.Point(0, 8)
        Me.lblTbl.Name = "lblTbl"
        Me.lblTbl.Size = New System.Drawing.Size(79, 28)
        Me.lblTbl.TabIndex = 67
        Me.lblTbl.Text = "Table"
        '
        'btnOptions
        '
        Me.btnOptions.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Bold)
        Me.btnOptions.Items.Add(Me.DropDownItem4)
        Me.btnOptions.Items.Add(Me.DropDownItem5)
        Me.btnOptions.Items.Add(Me.btnAlias)
        Me.btnOptions.Location = New System.Drawing.Point(396, 3)
        Me.btnOptions.Name = "btnOptions"
        Me.btnOptions.Size = New System.Drawing.Size(139, 45)
        Me.btnOptions.TabIndex = 7
        Me.btnOptions.Text = "Options"
        Me.btnOptions.UseVisualStyleBackColor = True
        '
        'DropDownItem4
        '
        Me.DropDownItem4.Text = "Re-Print Bill"
        '
        'DropDownItem5
        '
        Me.DropDownItem5.Text = "Customer / Member Dtls"
        '
        'btnAlias
        '
        Me.btnAlias.Text = "Add/Edit Form Alias"
        '
        'FlowLayoutPanel1
        '
        Me.FlowLayoutPanel1.BackColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.FlowLayoutPanel1.Controls.Add(Me.btnClose)
        Me.FlowLayoutPanel1.Controls.Add(Me.btnSave)
        Me.FlowLayoutPanel1.Controls.Add(Me.btnClearItems)
        Me.FlowLayoutPanel1.Controls.Add(Me.btnOptions)
        Me.FlowLayoutPanel1.Controls.Add(Me.BtnParcel)
        Me.FlowLayoutPanel1.Controls.Add(Me.btnDelvryAdd)
        Me.FlowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.FlowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft
        Me.FlowLayoutPanel1.Location = New System.Drawing.Point(0, 498)
        Me.FlowLayoutPanel1.Margin = New System.Windows.Forms.Padding(1)
        Me.FlowLayoutPanel1.Name = "FlowLayoutPanel1"
        Me.FlowLayoutPanel1.Size = New System.Drawing.Size(963, 54)
        Me.FlowLayoutPanel1.TabIndex = 17
        '
        'btnClose
        '
        CBlendItems1.iColor = New System.Drawing.Color() {System.Drawing.Color.White, System.Drawing.Color.Blue, System.Drawing.Color.Blue}
        CBlendItems1.iPoint = New Single() {0!, 0.3229814!, 1.0!}
        Me.btnClose.ColorFillBlend = CBlendItems1
        Me.btnClose.DesignerSelected = False
        Me.btnClose.ImageIndex = 0
        Me.btnClose.Location = New System.Drawing.Point(855, 3)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(105, 45)
        Me.btnClose.TabIndex = 5
        Me.btnClose.Text = "C&lose"
        '
        'btnSave
        '
        CBlendItems2.iColor = New System.Drawing.Color() {System.Drawing.Color.White, System.Drawing.Color.White, System.Drawing.Color.ForestGreen}
        CBlendItems2.iPoint = New Single() {0!, 0.1273292!, 1.0!}
        Me.btnSave.ColorFillBlend = CBlendItems2
        Me.btnSave.ColorFillSolid = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.btnSave.DesignerSelected = False
        Me.btnSave.ForeColor = System.Drawing.Color.Black
        Me.btnSave.ImageIndex = 0
        Me.btnSave.Location = New System.Drawing.Point(690, 3)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(159, 45)
        Me.btnSave.TabIndex = 3
        Me.btnSave.Text = "Print &Bill    [ F8 ]"
        '
        'btnClearItems
        '
        CBlendItems3.iColor = New System.Drawing.Color() {System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer)), System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer)), System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer)), System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))}
        CBlendItems3.iPoint = New Single() {0!, 0.1111111!, 0.9938272!, 1.0!}
        Me.btnClearItems.ColorFillBlend = CBlendItems3
        Me.btnClearItems.ColorFillSolid = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.btnClearItems.DesignerSelected = False
        Me.btnClearItems.ForeColor = System.Drawing.Color.Black
        Me.btnClearItems.ImageIndex = 0
        Me.btnClearItems.Location = New System.Drawing.Point(541, 3)
        Me.btnClearItems.Name = "btnClearItems"
        Me.btnClearItems.Size = New System.Drawing.Size(143, 45)
        Me.btnClearItems.TabIndex = 4
        Me.btnClearItems.Text = "&Clear Items   [ F3 ]"
        '
        'BtnParcel
        '
        CBlendItems4.iColor = New System.Drawing.Color() {System.Drawing.Color.White, System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer)), System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))}
        CBlendItems4.iPoint = New Single() {0!, 0.3229814!, 1.0!}
        Me.BtnParcel.ColorFillBlend = CBlendItems4
        Me.BtnParcel.DesignerSelected = False
        Me.BtnParcel.ForeColor = System.Drawing.Color.Black
        Me.BtnParcel.ImageIndex = 0
        Me.BtnParcel.Location = New System.Drawing.Point(269, 3)
        Me.BtnParcel.Name = "BtnParcel"
        Me.BtnParcel.Size = New System.Drawing.Size(121, 45)
        Me.BtnParcel.TabIndex = 10
        Me.BtnParcel.Text = "Pa&rcel [ F12 ] "
        '
        'btnDelvryAdd
        '
        CBlendItems5.iColor = New System.Drawing.Color() {System.Drawing.Color.White, System.Drawing.Color.White, System.Drawing.Color.ForestGreen}
        CBlendItems5.iPoint = New Single() {0!, 0.1273292!, 1.0!}
        Me.btnDelvryAdd.ColorFillBlend = CBlendItems5
        Me.btnDelvryAdd.ColorFillSolid = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.btnDelvryAdd.DesignerSelected = False
        Me.btnDelvryAdd.ForeColor = System.Drawing.Color.Black
        Me.btnDelvryAdd.ImageIndex = 0
        Me.btnDelvryAdd.Location = New System.Drawing.Point(148, 3)
        Me.btnDelvryAdd.Name = "btnDelvryAdd"
        Me.btnDelvryAdd.Size = New System.Drawing.Size(115, 45)
        Me.btnDelvryAdd.TabIndex = 13
        Me.btnDelvryAdd.Text = "&Delivery Address"
        '
        'Timer1
        '
        Me.Timer1.Interval = 60000
        '
        'SplitContainer1
        '
        Me.SplitContainer1.BackColor = System.Drawing.Color.LightSalmon
        Me.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer1.Location = New System.Drawing.Point(0, 30)
        Me.SplitContainer1.Name = "SplitContainer1"
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.BackColor = System.Drawing.Color.Black
        Me.SplitContainer1.Panel1.Controls.Add(Me.cboMenuGroup)
        Me.SplitContainer1.Panel1.Controls.Add(Me.Panel4)
        Me.SplitContainer1.Panel1.Controls.Add(Me.vsfMenu)
        Me.SplitContainer1.Panel1.Controls.Add(Me.vsfDet)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.Panel1)
        Me.SplitContainer1.Size = New System.Drawing.Size(963, 468)
        Me.SplitContainer1.SplitterDistance = 741
        Me.SplitContainer1.TabIndex = 47
        '
        'cboMenuGroup
        '
        Me.cboMenuGroup.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.cboMenuGroup.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cboMenuGroup.BackColor = System.Drawing.Color.FromArgb(CType(CType(249, Byte), Integer), CType(CType(251, Byte), Integer), CType(CType(176, Byte), Integer))
        Me.cboMenuGroup.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboMenuGroup.FormattingEnabled = True
        Me.cboMenuGroup.Location = New System.Drawing.Point(4, 57)
        Me.cboMenuGroup.Name = "cboMenuGroup"
        Me.cboMenuGroup.Size = New System.Drawing.Size(326, 28)
        Me.cboMenuGroup.TabIndex = 68
        '
        'Panel4
        '
        Me.Panel4.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel4.BackColor = System.Drawing.Color.DimGray
        Me.Panel4.Controls.Add(Me.txtTblCode)
        Me.Panel4.Controls.Add(Me.lblTbl)
        Me.Panel4.Controls.Add(Me.txtQty)
        Me.Panel4.Controls.Add(Me.Label3)
        Me.Panel4.Controls.Add(Me.txtItemCode)
        Me.Panel4.Controls.Add(Me.lblItemCaption)
        Me.Panel4.Controls.Add(Me.lblItemName)
        Me.Panel4.Controls.Add(Me.btnAddItem)
        Me.Panel4.Location = New System.Drawing.Point(3, 5)
        Me.Panel4.Name = "Panel4"
        Me.Panel4.Size = New System.Drawing.Size(735, 45)
        Me.Panel4.TabIndex = 67
        '
        'txtQty
        '
        Me.txtQty.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtQty.Font = New System.Drawing.Font("Bookman Old Style", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtQty.Location = New System.Drawing.Point(614, 4)
        Me.txtQty.MaxLength = 10
        Me.txtQty.Name = "txtQty"
        Me.txtQty.Size = New System.Drawing.Size(58, 32)
        Me.txtQty.TabIndex = 1
        Me.txtQty.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label3
        '
        Me.Label3.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Bookman Old Style", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.Color.White
        Me.Label3.Location = New System.Drawing.Point(556, 6)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(57, 28)
        Me.Label3.TabIndex = 64
        Me.Label3.Text = "Qty"
        '
        'txtItemCode
        '
        Me.txtItemCode.Font = New System.Drawing.Font("Bookman Old Style", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtItemCode.Location = New System.Drawing.Point(256, 6)
        Me.txtItemCode.MaxLength = 50
        Me.txtItemCode.Name = "txtItemCode"
        Me.txtItemCode.Size = New System.Drawing.Size(102, 32)
        Me.txtItemCode.TabIndex = 0
        Me.txtItemCode.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'lblItemCaption
        '
        Me.lblItemCaption.AutoSize = True
        Me.lblItemCaption.Font = New System.Drawing.Font("Bookman Old Style", 18.0!, System.Drawing.FontStyle.Bold)
        Me.lblItemCaption.ForeColor = System.Drawing.Color.White
        Me.lblItemCaption.Location = New System.Drawing.Point(182, 6)
        Me.lblItemCaption.Name = "lblItemCaption"
        Me.lblItemCaption.Size = New System.Drawing.Size(71, 28)
        Me.lblItemCaption.TabIndex = 57
        Me.lblItemCaption.Text = "Item"
        '
        'lblItemName
        '
        Me.lblItemName.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblItemName.Font = New System.Drawing.Font("Bookman Old Style", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblItemName.ForeColor = System.Drawing.Color.Yellow
        Me.lblItemName.Location = New System.Drawing.Point(364, 4)
        Me.lblItemName.Name = "lblItemName"
        Me.lblItemName.Size = New System.Drawing.Size(195, 41)
        Me.lblItemName.TabIndex = 59
        Me.lblItemName.Text = "Item Name"
        Me.lblItemName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btnAddItem
        '
        Me.btnAddItem.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnAddItem.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnAddItem.Location = New System.Drawing.Point(678, 4)
        Me.btnAddItem.Name = "btnAddItem"
        Me.btnAddItem.Size = New System.Drawing.Size(55, 33)
        Me.btnAddItem.TabIndex = 62
        Me.btnAddItem.Text = "OK"
        Me.btnAddItem.UseVisualStyleBackColor = True
        '
        'vsfMenu
        '
        Me.vsfMenu.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.vsfMenu.AutoSearch = C1.Win.C1FlexGrid.AutoSearchEnum.FromCursor
        Me.vsfMenu.BackColor = System.Drawing.Color.FromArgb(CType(CType(249, Byte), Integer), CType(CType(251, Byte), Integer), CType(CType(176, Byte), Integer))
        Me.vsfMenu.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle
        Me.vsfMenu.ColumnInfo = "2,0,0,0,0,130,Columns:"
        Me.vsfMenu.ExtendLastCol = True
        Me.vsfMenu.Font = New System.Drawing.Font("Segoe UI", 11.0!)
        Me.vsfMenu.ForeColor = System.Drawing.Color.Black
        Me.vsfMenu.HighLight = C1.Win.C1FlexGrid.HighLightEnum.WithFocus
        Me.vsfMenu.Location = New System.Drawing.Point(3, 86)
        Me.vsfMenu.Name = "vsfMenu"
        Me.vsfMenu.Rows.Count = 9
        Me.vsfMenu.Rows.DefaultSize = 26
        Me.vsfMenu.Rows.MinSize = 30
        Me.vsfMenu.Size = New System.Drawing.Size(327, 376)
        Me.vsfMenu.StyleInfo = resources.GetString("vsfMenu.StyleInfo")
        Me.vsfMenu.TabIndex = 65
        Me.vsfMenu.Tag = ""
        '
        'vsfDet
        '
        Me.vsfDet.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfDet.AutoSearch = C1.Win.C1FlexGrid.AutoSearchEnum.FromCursor
        Me.vsfDet.BackColor = System.Drawing.Color.White
        Me.vsfDet.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.None
        Me.vsfDet.ColumnInfo = "2,0,0,0,0,130,Columns:"
        Me.vsfDet.ExtendLastCol = True
        Me.vsfDet.Font = New System.Drawing.Font("Segoe UI", 11.0!)
        Me.vsfDet.ForeColor = System.Drawing.Color.Black
        Me.vsfDet.Location = New System.Drawing.Point(338, 58)
        Me.vsfDet.Name = "vsfDet"
        Me.vsfDet.Rows.Count = 4
        Me.vsfDet.Rows.DefaultSize = 26
        Me.vsfDet.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfDet.Size = New System.Drawing.Size(398, 403)
        Me.vsfDet.StyleInfo = resources.GetString("vsfDet.StyleInfo")
        Me.vsfDet.TabIndex = 42
        Me.vsfDet.Tag = ""
        '
        'frmTrnQuickBillNonTouch
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(963, 552)
        Me.Controls.Add(Me.SplitContainer1)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.FlowLayoutPanel1)
        Me.KeyPreview = True
        Me.MinimumSize = New System.Drawing.Size(962, 587)
        Me.Name = "frmTrnQuickBillNonTouch"
        Me.ShowIcon = False
        Me.Text = "Quick Service Bill"
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        CType(Me.vsfColFlds, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.vsfBC, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.vsfHdr, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlAmt.ResumeLayout(False)
        Me.pnlAmt.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        CType(Me.vsfOptionsBC, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.vsfOptions, System.ComponentModel.ISupportInitialize).EndInit()
        Me.FlowLayoutPanel1.ResumeLayout(False)
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.ResumeLayout(False)
        Me.Panel4.ResumeLayout(False)
        Me.Panel4.PerformLayout()
        CType(Me.vsfMenu, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents lblCounter As System.Windows.Forms.Label
    Friend WithEvents lblDatetime As System.Windows.Forms.Label
    Friend WithEvents lblTotal As System.Windows.Forms.Label
    Friend WithEvents lblTotalCaption As System.Windows.Forms.Label
    Friend WithEvents btnOptions As C1.Win.C1Input.C1SplitButton
    Friend WithEvents btnClearItems As vLQMS.CButton
    Friend WithEvents btnSave As vLQMS.CButton
    Friend WithEvents btnClose As vLQMS.CButton
    Friend WithEvents FlowLayoutPanel1 As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents Timer1 As System.Windows.Forms.Timer
    Friend WithEvents pnlAmt As System.Windows.Forms.Panel
    Friend WithEvents SplitContainer1 As System.Windows.Forms.SplitContainer
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents DropDownItem4 As C1.Win.C1Input.DropDownItem
    Friend WithEvents DropDownItem5 As C1.Win.C1Input.DropDownItem
    Friend WithEvents vsfHdr As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents vsfBC As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents vsfColFlds As C1.Win.C1FlexGrid.C1FlexGrid
    Public WithEvents vsfOptionsBC As C1.Win.C1FlexGrid.C1FlexGrid
    Public WithEvents vsfOptions As C1.Win.C1FlexGrid.C1FlexGrid
    Public WithEvents cboPfx As System.Windows.Forms.ComboBox
    Public WithEvents txtNo As System.Windows.Forms.TextBox
    Public WithEvents txtDocNo As System.Windows.Forms.TextBox
    Public WithEvents dtDocDate As System.Windows.Forms.DateTimePicker
    Public WithEvents vsfDet As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents vsfMenu As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents Panel4 As System.Windows.Forms.Panel
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents txtItemCode As System.Windows.Forms.TextBox
    Friend WithEvents lblItemCaption As System.Windows.Forms.Label
    Friend WithEvents lblItemName As System.Windows.Forms.Label
    Friend WithEvents btnAddItem As System.Windows.Forms.Button
    Friend WithEvents BtnParcel As vLQMS.CButton
    Friend WithEvents lblParcel As System.Windows.Forms.Label
    Friend WithEvents txtQty As System.Windows.Forms.TextBox
    Friend WithEvents cboMenuGroup As System.Windows.Forms.ComboBox
    Friend WithEvents btnAlias As C1.Win.C1Input.DropDownItem
    Friend WithEvents txtTblCode As System.Windows.Forms.TextBox
    Friend WithEvents lblTbl As System.Windows.Forms.Label
    Friend WithEvents btnDelvryAdd As vLQMS.CButton
End Class
