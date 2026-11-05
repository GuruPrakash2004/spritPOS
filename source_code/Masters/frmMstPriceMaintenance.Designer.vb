<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmMstPriceMaintenance
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMstPriceMaintenance))
        Me.pnlSelFlds = New System.Windows.Forms.Panel()
        Me.lblRateSel = New System.Windows.Forms.Label()
        Me.lblItemSel = New System.Windows.Forms.Label()
        Me.btnExpXLvsfItemGroup = New C1.Win.C1Input.C1Button()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.btnLoad = New System.Windows.Forms.Button()
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.vsfItemGroup = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.vsfItemCol = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.gbSel = New System.Windows.Forms.GroupBox()
        Me.optItem = New System.Windows.Forms.RadioButton()
        Me.optItemGroup = New System.Windows.Forms.RadioButton()
        Me.lblvsfItemGroup = New System.Windows.Forms.Label()
        Me.pnlMain = New System.Windows.Forms.Panel()
        Me.chkAll = New System.Windows.Forms.CheckBox()
        Me.btnExport = New C1.Win.C1Input.C1Button()
        Me.btnApply = New System.Windows.Forms.Button()
        Me.btnUpdtToDB = New System.Windows.Forms.Button()
        Me.btnBack = New System.Windows.Forms.Button()
        Me.btnReplace = New System.Windows.Forms.Button()
        Me.lblRecCnt = New System.Windows.Forms.Label()
        Me.vsfItemGroupDet = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.lblPriceUpdtHelp = New System.Windows.Forms.Label()
        Me.gbRateUpdateBy = New System.Windows.Forms.GroupBox()
        Me.pnlPerAmt = New System.Windows.Forms.Panel()
        Me.optPer = New System.Windows.Forms.RadioButton()
        Me.optAmt = New System.Windows.Forms.RadioButton()
        Me.pnlIncDec = New System.Windows.Forms.Panel()
        Me.optInc = New System.Windows.Forms.RadioButton()
        Me.optDec = New System.Windows.Forms.RadioButton()
        Me.cboRound = New System.Windows.Forms.ComboBox()
        Me.txtAmt = New System.Windows.Forms.TextBox()
        Me.lblAmt = New System.Windows.Forms.Label()
        Me.tt = New System.Windows.Forms.ToolTip(Me.components)
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.lblFrm = New System.Windows.Forms.Label()
        Me.pnlSelFlds.SuspendLayout()
        Me.TableLayoutPanel1.SuspendLayout()
        CType(Me.vsfItemGroup, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.vsfItemCol, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.gbSel.SuspendLayout()
        Me.pnlMain.SuspendLayout()
        CType(Me.vsfItemGroupDet, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.gbRateUpdateBy.SuspendLayout()
        Me.pnlPerAmt.SuspendLayout()
        Me.pnlIncDec.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'pnlSelFlds
        '
        Me.pnlSelFlds.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnlSelFlds.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlSelFlds.Controls.Add(Me.lblRateSel)
        Me.pnlSelFlds.Controls.Add(Me.lblItemSel)
        Me.pnlSelFlds.Controls.Add(Me.btnExpXLvsfItemGroup)
        Me.pnlSelFlds.Controls.Add(Me.btnClose)
        Me.pnlSelFlds.Controls.Add(Me.btnLoad)
        Me.pnlSelFlds.Controls.Add(Me.TableLayoutPanel1)
        Me.pnlSelFlds.Controls.Add(Me.gbSel)
        Me.pnlSelFlds.Controls.Add(Me.lblvsfItemGroup)
        Me.pnlSelFlds.Location = New System.Drawing.Point(10, 38)
        Me.pnlSelFlds.Name = "pnlSelFlds"
        Me.pnlSelFlds.Size = New System.Drawing.Size(809, 424)
        Me.pnlSelFlds.TabIndex = 0
        '
        'lblRateSel
        '
        Me.lblRateSel.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblRateSel.AutoSize = True
        Me.lblRateSel.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRateSel.ForeColor = System.Drawing.Color.Maroon
        Me.lblRateSel.Location = New System.Drawing.Point(506, 37)
        Me.lblRateSel.Name = "lblRateSel"
        Me.lblRateSel.Size = New System.Drawing.Size(0, 20)
        Me.lblRateSel.TabIndex = 237
        Me.lblRateSel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblItemSel
        '
        Me.lblItemSel.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblItemSel.AutoSize = True
        Me.lblItemSel.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblItemSel.ForeColor = System.Drawing.Color.Maroon
        Me.lblItemSel.Location = New System.Drawing.Point(506, 3)
        Me.lblItemSel.Name = "lblItemSel"
        Me.lblItemSel.Size = New System.Drawing.Size(0, 20)
        Me.lblItemSel.TabIndex = 237
        Me.lblItemSel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'btnExpXLvsfItemGroup
        '
        Me.btnExpXLvsfItemGroup.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnExpXLvsfItemGroup.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.btnExpXLvsfItemGroup.Image = CType(resources.GetObject("btnExpXLvsfItemGroup.Image"), System.Drawing.Image)
        Me.btnExpXLvsfItemGroup.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnExpXLvsfItemGroup.Location = New System.Drawing.Point(3, 383)
        Me.btnExpXLvsfItemGroup.Name = "btnExpXLvsfItemGroup"
        Me.btnExpXLvsfItemGroup.Size = New System.Drawing.Size(33, 31)
        Me.btnExpXLvsfItemGroup.TabIndex = 236
        Me.btnExpXLvsfItemGroup.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.tt.SetToolTip(Me.btnExpXLvsfItemGroup, "Export to XL : Alt + E")
        Me.btnExpXLvsfItemGroup.UseVisualStyleBackColor = False
        '
        'btnClose
        '
        Me.btnClose.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_CLOSE
        Me.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnClose.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnClose.Location = New System.Drawing.Point(404, 16)
        Me.btnClose.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(88, 30)
        Me.btnClose.TabIndex = 234
        Me.btnClose.Text = "&Close"
        Me.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnClose.UseVisualStyleBackColor = False
        '
        'btnLoad
        '
        Me.btnLoad.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnLoad.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnLoad.Image = CType(resources.GetObject("btnLoad.Image"), System.Drawing.Image)
        Me.btnLoad.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnLoad.Location = New System.Drawing.Point(311, 16)
        Me.btnLoad.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnLoad.Name = "btnLoad"
        Me.btnLoad.Size = New System.Drawing.Size(87, 30)
        Me.btnLoad.TabIndex = 233
        Me.btnLoad.Text = "&Load"
        Me.btnLoad.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnLoad.UseVisualStyleBackColor = False
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TableLayoutPanel1.ColumnCount = 2
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 61.48374!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 38.51626!))
        Me.TableLayoutPanel1.Controls.Add(Me.vsfItemGroup, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.vsfItemCol, 1, 0)
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(3, 61)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 1
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 319.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(801, 319)
        Me.TableLayoutPanel1.TabIndex = 232
        '
        'vsfItemGroup
        '
        Me.vsfItemGroup.AllowFiltering = True
        Me.vsfItemGroup.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfItemGroup.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle
        Me.vsfItemGroup.ColumnInfo = "2,0,0,0,0,110,Columns:"
        Me.vsfItemGroup.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.vsfItemGroup.Location = New System.Drawing.Point(3, 4)
        Me.vsfItemGroup.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.vsfItemGroup.Name = "vsfItemGroup"
        Me.vsfItemGroup.Rows.Count = 2
        Me.vsfItemGroup.Rows.DefaultSize = 24
        Me.vsfItemGroup.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfItemGroup.Size = New System.Drawing.Size(486, 311)
        Me.vsfItemGroup.StyleInfo = resources.GetString("vsfItemGroup.StyleInfo")
        Me.vsfItemGroup.TabIndex = 231
        '
        'vsfItemCol
        '
        Me.vsfItemCol.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.MultiColumn
        Me.vsfItemCol.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfItemCol.AutoResize = True
        Me.vsfItemCol.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle
        Me.vsfItemCol.ColumnInfo = "2,0,0,0,0,110,Columns:"
        Me.vsfItemCol.ExtendLastCol = True
        Me.vsfItemCol.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.vsfItemCol.Location = New System.Drawing.Point(495, 4)
        Me.vsfItemCol.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.vsfItemCol.Name = "vsfItemCol"
        Me.vsfItemCol.Rows.Count = 2
        Me.vsfItemCol.Rows.DefaultSize = 24
        Me.vsfItemCol.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.vsfItemCol.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfItemCol.Size = New System.Drawing.Size(303, 311)
        Me.vsfItemCol.StyleInfo = resources.GetString("vsfItemCol.StyleInfo")
        Me.vsfItemCol.TabIndex = 231
        '
        'gbSel
        '
        Me.gbSel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.gbSel.Controls.Add(Me.optItem)
        Me.gbSel.Controls.Add(Me.optItemGroup)
        Me.gbSel.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.gbSel.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.gbSel.Location = New System.Drawing.Point(6, 3)
        Me.gbSel.Name = "gbSel"
        Me.gbSel.Size = New System.Drawing.Size(299, 52)
        Me.gbSel.TabIndex = 0
        Me.gbSel.TabStop = False
        Me.gbSel.Text = "Selection By"
        '
        'optItem
        '
        Me.optItem.AutoSize = True
        Me.optItem.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optItem.Location = New System.Drawing.Point(164, 19)
        Me.optItem.Name = "optItem"
        Me.optItem.Size = New System.Drawing.Size(129, 24)
        Me.optItem.TabIndex = 1
        Me.optItem.TabStop = True
        Me.optItem.Text = "Item Selection"
        Me.optItem.UseVisualStyleBackColor = True
        '
        'optItemGroup
        '
        Me.optItemGroup.AutoSize = True
        Me.optItemGroup.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optItemGroup.Location = New System.Drawing.Point(6, 19)
        Me.optItemGroup.Name = "optItemGroup"
        Me.optItemGroup.Size = New System.Drawing.Size(142, 24)
        Me.optItemGroup.TabIndex = 1
        Me.optItemGroup.TabStop = True
        Me.optItemGroup.Text = "Group Selection"
        Me.optItemGroup.UseVisualStyleBackColor = True
        '
        'lblvsfItemGroup
        '
        Me.lblvsfItemGroup.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblvsfItemGroup.AutoSize = True
        Me.lblvsfItemGroup.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblvsfItemGroup.Location = New System.Drawing.Point(51, 388)
        Me.lblvsfItemGroup.Name = "lblvsfItemGroup"
        Me.lblvsfItemGroup.Size = New System.Drawing.Size(86, 20)
        Me.lblvsfItemGroup.TabIndex = 232
        Me.lblvsfItemGroup.Text = "Records :"
        '
        'pnlMain
        '
        Me.pnlMain.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnlMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlMain.Controls.Add(Me.chkAll)
        Me.pnlMain.Controls.Add(Me.btnExport)
        Me.pnlMain.Controls.Add(Me.btnApply)
        Me.pnlMain.Controls.Add(Me.btnUpdtToDB)
        Me.pnlMain.Controls.Add(Me.btnBack)
        Me.pnlMain.Controls.Add(Me.btnReplace)
        Me.pnlMain.Controls.Add(Me.lblRecCnt)
        Me.pnlMain.Controls.Add(Me.vsfItemGroupDet)
        Me.pnlMain.Controls.Add(Me.lblPriceUpdtHelp)
        Me.pnlMain.Controls.Add(Me.gbRateUpdateBy)
        Me.pnlMain.Location = New System.Drawing.Point(10, 38)
        Me.pnlMain.Name = "pnlMain"
        Me.pnlMain.Size = New System.Drawing.Size(809, 428)
        Me.pnlMain.TabIndex = 236
        '
        'chkAll
        '
        Me.chkAll.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.chkAll.AutoSize = True
        Me.chkAll.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!)
        Me.chkAll.Location = New System.Drawing.Point(413, 349)
        Me.chkAll.Name = "chkAll"
        Me.chkAll.Size = New System.Drawing.Size(94, 24)
        Me.chkAll.TabIndex = 237
        Me.chkAll.Text = "Select &All"
        Me.chkAll.UseVisualStyleBackColor = True
        '
        'btnExport
        '
        Me.btnExport.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnExport.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnExport.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.btnExport.Image = CType(resources.GetObject("btnExport.Image"), System.Drawing.Image)
        Me.btnExport.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnExport.Location = New System.Drawing.Point(769, 300)
        Me.btnExport.Name = "btnExport"
        Me.btnExport.Size = New System.Drawing.Size(33, 31)
        Me.btnExport.TabIndex = 236
        Me.btnExport.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.tt.SetToolTip(Me.btnExport, "Export to XL : Alt + E")
        Me.btnExport.UseVisualStyleBackColor = False
        '
        'btnApply
        '
        Me.btnApply.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnApply.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnApply.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnApply.Location = New System.Drawing.Point(411, 384)
        Me.btnApply.Name = "btnApply"
        Me.btnApply.Size = New System.Drawing.Size(101, 31)
        Me.btnApply.TabIndex = 233
        Me.btnApply.Text = "&Apply"
        Me.btnApply.UseVisualStyleBackColor = True
        '
        'btnUpdtToDB
        '
        Me.btnUpdtToDB.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnUpdtToDB.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnUpdtToDB.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnUpdtToDB.Location = New System.Drawing.Point(569, 386)
        Me.btnUpdtToDB.Name = "btnUpdtToDB"
        Me.btnUpdtToDB.Size = New System.Drawing.Size(233, 34)
        Me.btnUpdtToDB.TabIndex = 233
        Me.btnUpdtToDB.Text = "Update to &DB"
        Me.btnUpdtToDB.UseVisualStyleBackColor = True
        '
        'btnBack
        '
        Me.btnBack.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnBack.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.btnBack.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnBack.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnBack.Image = Global.vLQMS.My.Resources.Resources.D_PREVIOUS
        Me.btnBack.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnBack.Location = New System.Drawing.Point(710, 26)
        Me.btnBack.Name = "btnBack"
        Me.btnBack.Size = New System.Drawing.Size(92, 30)
        Me.btnBack.TabIndex = 233
        Me.btnBack.Text = "&Back"
        Me.btnBack.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnBack.UseVisualStyleBackColor = True
        '
        'btnReplace
        '
        Me.btnReplace.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnReplace.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnReplace.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnReplace.Location = New System.Drawing.Point(513, 300)
        Me.btnReplace.Name = "btnReplace"
        Me.btnReplace.Size = New System.Drawing.Size(250, 31)
        Me.btnReplace.TabIndex = 233
        Me.btnReplace.Text = "&Replace for rest of the list"
        Me.btnReplace.UseVisualStyleBackColor = True
        '
        'lblRecCnt
        '
        Me.lblRecCnt.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblRecCnt.AutoSize = True
        Me.lblRecCnt.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRecCnt.Location = New System.Drawing.Point(8, 300)
        Me.lblRecCnt.Name = "lblRecCnt"
        Me.lblRecCnt.Size = New System.Drawing.Size(86, 20)
        Me.lblRecCnt.TabIndex = 232
        Me.lblRecCnt.Text = "Records :"
        '
        'vsfItemGroupDet
        '
        Me.vsfItemGroupDet.AllowFiltering = True
        Me.vsfItemGroupDet.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None
        Me.vsfItemGroupDet.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfItemGroupDet.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle
        Me.vsfItemGroupDet.ColumnInfo = "2,0,0,0,0,110,Columns:"
        Me.vsfItemGroupDet.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.vsfItemGroupDet.Location = New System.Drawing.Point(7, 63)
        Me.vsfItemGroupDet.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.vsfItemGroupDet.Name = "vsfItemGroupDet"
        Me.vsfItemGroupDet.Rows.Count = 2
        Me.vsfItemGroupDet.Rows.DefaultSize = 24
        Me.vsfItemGroupDet.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfItemGroupDet.Size = New System.Drawing.Size(796, 233)
        Me.vsfItemGroupDet.StyleInfo = resources.GetString("vsfItemGroupDet.StyleInfo")
        Me.vsfItemGroupDet.TabIndex = 231
        '
        'lblPriceUpdtHelp
        '
        Me.lblPriceUpdtHelp.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblPriceUpdtHelp.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPriceUpdtHelp.ForeColor = System.Drawing.Color.Maroon
        Me.lblPriceUpdtHelp.Location = New System.Drawing.Point(4, 7)
        Me.lblPriceUpdtHelp.Name = "lblPriceUpdtHelp"
        Me.lblPriceUpdtHelp.Size = New System.Drawing.Size(700, 49)
        Me.lblPriceUpdtHelp.TabIndex = 0
        Me.lblPriceUpdtHelp.Text = "On click of Load, Items are loaded ready for price alteration. Bulk Rate updation" &
    " can also be done for selected Items by entering the Update rate by Amount(given" &
    " below)"
        Me.lblPriceUpdtHelp.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'gbRateUpdateBy
        '
        Me.gbRateUpdateBy.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.gbRateUpdateBy.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.gbRateUpdateBy.Controls.Add(Me.pnlPerAmt)
        Me.gbRateUpdateBy.Controls.Add(Me.pnlIncDec)
        Me.gbRateUpdateBy.Controls.Add(Me.cboRound)
        Me.gbRateUpdateBy.Controls.Add(Me.txtAmt)
        Me.gbRateUpdateBy.Controls.Add(Me.lblAmt)
        Me.gbRateUpdateBy.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.gbRateUpdateBy.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.gbRateUpdateBy.Location = New System.Drawing.Point(7, 323)
        Me.gbRateUpdateBy.Name = "gbRateUpdateBy"
        Me.gbRateUpdateBy.Size = New System.Drawing.Size(398, 96)
        Me.gbRateUpdateBy.TabIndex = 0
        Me.gbRateUpdateBy.TabStop = False
        Me.gbRateUpdateBy.Text = "Update Rate by"
        '
        'pnlPerAmt
        '
        Me.pnlPerAmt.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.pnlPerAmt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlPerAmt.Controls.Add(Me.optPer)
        Me.pnlPerAmt.Controls.Add(Me.optAmt)
        Me.pnlPerAmt.Location = New System.Drawing.Point(226, 24)
        Me.pnlPerAmt.Name = "pnlPerAmt"
        Me.pnlPerAmt.Size = New System.Drawing.Size(165, 29)
        Me.pnlPerAmt.TabIndex = 235
        '
        'optPer
        '
        Me.optPer.AutoSize = True
        Me.optPer.Checked = True
        Me.optPer.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optPer.Location = New System.Drawing.Point(3, 1)
        Me.optPer.Name = "optPer"
        Me.optPer.Size = New System.Drawing.Size(68, 24)
        Me.optPer.TabIndex = 1
        Me.optPer.TabStop = True
        Me.optPer.Text = "%age"
        Me.optPer.UseVisualStyleBackColor = True
        '
        'optAmt
        '
        Me.optAmt.AutoSize = True
        Me.optAmt.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optAmt.Location = New System.Drawing.Point(77, 1)
        Me.optAmt.Name = "optAmt"
        Me.optAmt.Size = New System.Drawing.Size(83, 24)
        Me.optAmt.TabIndex = 1
        Me.optAmt.TabStop = True
        Me.optAmt.Text = "Amount"
        Me.optAmt.UseVisualStyleBackColor = True
        '
        'pnlIncDec
        '
        Me.pnlIncDec.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.pnlIncDec.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlIncDec.Controls.Add(Me.optInc)
        Me.pnlIncDec.Controls.Add(Me.optDec)
        Me.pnlIncDec.Location = New System.Drawing.Point(10, 24)
        Me.pnlIncDec.Name = "pnlIncDec"
        Me.pnlIncDec.Size = New System.Drawing.Size(210, 29)
        Me.pnlIncDec.TabIndex = 235
        '
        'optInc
        '
        Me.optInc.AutoSize = True
        Me.optInc.Checked = True
        Me.optInc.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optInc.Location = New System.Drawing.Point(3, 1)
        Me.optInc.Name = "optInc"
        Me.optInc.Size = New System.Drawing.Size(89, 24)
        Me.optInc.TabIndex = 1
        Me.optInc.TabStop = True
        Me.optInc.Text = "Increase"
        Me.optInc.UseVisualStyleBackColor = True
        '
        'optDec
        '
        Me.optDec.AutoSize = True
        Me.optDec.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optDec.Location = New System.Drawing.Point(109, 1)
        Me.optDec.Name = "optDec"
        Me.optDec.Size = New System.Drawing.Size(96, 24)
        Me.optDec.TabIndex = 1
        Me.optDec.TabStop = True
        Me.optDec.Text = "Decrease"
        Me.optDec.UseVisualStyleBackColor = True
        '
        'cboRound
        '
        Me.cboRound.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cboRound.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboRound.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.cboRound.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboRound.FormattingEnabled = True
        Me.cboRound.Items.AddRange(New Object() {"No RoundOff", "50p RoundOff", "1Rs RoundOff"})
        Me.cboRound.Location = New System.Drawing.Point(226, 62)
        Me.cboRound.Name = "cboRound"
        Me.cboRound.Size = New System.Drawing.Size(165, 28)
        Me.cboRound.TabIndex = 234
        '
        'txtAmt
        '
        Me.txtAmt.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.txtAmt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtAmt.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAmt.Location = New System.Drawing.Point(116, 63)
        Me.txtAmt.MaxLength = 6
        Me.txtAmt.Name = "txtAmt"
        Me.txtAmt.Size = New System.Drawing.Size(104, 26)
        Me.txtAmt.TabIndex = 233
        Me.txtAmt.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'lblAmt
        '
        Me.lblAmt.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblAmt.AutoSize = True
        Me.lblAmt.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAmt.Location = New System.Drawing.Point(6, 66)
        Me.lblAmt.Name = "lblAmt"
        Me.lblAmt.Size = New System.Drawing.Size(65, 20)
        Me.lblAmt.TabIndex = 232
        Me.lblAmt.Text = "Amount"
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.FromArgb(CType(CType(53, Byte), Integer), CType(CType(73, Byte), Integer), CType(CType(106, Byte), Integer))
        Me.Panel1.Controls.Add(Me.lblFrm)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Margin = New System.Windows.Forms.Padding(4)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(826, 35)
        Me.Panel1.TabIndex = 237
        '
        'lblFrm
        '
        Me.lblFrm.AutoSize = True
        Me.lblFrm.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.lblFrm.Font = New System.Drawing.Font("Segoe UI", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFrm.ForeColor = System.Drawing.Color.White
        Me.lblFrm.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.lblFrm.Location = New System.Drawing.Point(17, 6)
        Me.lblFrm.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblFrm.Name = "lblFrm"
        Me.lblFrm.Size = New System.Drawing.Size(0, 25)
        Me.lblFrm.TabIndex = 1
        '
        'frmMstPriceMaintenance
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(826, 478)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.pnlSelFlds)
        Me.Controls.Add(Me.pnlMain)
        Me.KeyPreview = True
        Me.MinimumSize = New System.Drawing.Size(842, 445)
        Me.Name = "frmMstPriceMaintenance"
        Me.ShowIcon = False
        Me.Text = "Item Price Maintenance Master"
        Me.pnlSelFlds.ResumeLayout(False)
        Me.pnlSelFlds.PerformLayout()
        Me.TableLayoutPanel1.ResumeLayout(False)
        CType(Me.vsfItemGroup, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.vsfItemCol, System.ComponentModel.ISupportInitialize).EndInit()
        Me.gbSel.ResumeLayout(False)
        Me.gbSel.PerformLayout()
        Me.pnlMain.ResumeLayout(False)
        Me.pnlMain.PerformLayout()
        CType(Me.vsfItemGroupDet, System.ComponentModel.ISupportInitialize).EndInit()
        Me.gbRateUpdateBy.ResumeLayout(False)
        Me.gbRateUpdateBy.PerformLayout()
        Me.pnlPerAmt.ResumeLayout(False)
        Me.pnlPerAmt.PerformLayout()
        Me.pnlIncDec.ResumeLayout(False)
        Me.pnlIncDec.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents pnlSelFlds As Panel
    Friend WithEvents gbSel As GroupBox
    Friend WithEvents optItem As RadioButton
    Friend WithEvents optItemGroup As RadioButton
    Friend WithEvents vsfItemGroup As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents TableLayoutPanel1 As TableLayoutPanel
    Friend WithEvents vsfItemCol As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents btnClose As Button
    Friend WithEvents btnLoad As Button
    Friend WithEvents pnlMain As Panel
    Friend WithEvents lblPriceUpdtHelp As Label
    Friend WithEvents vsfItemGroupDet As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents gbRateUpdateBy As GroupBox
    Friend WithEvents optDec As RadioButton
    Friend WithEvents optInc As RadioButton
    Friend WithEvents lblAmt As Label
    Friend WithEvents txtAmt As TextBox
    Friend WithEvents cboRound As ComboBox
    Friend WithEvents pnlIncDec As Panel
    Friend WithEvents pnlPerAmt As Panel
    Friend WithEvents optPer As RadioButton
    Friend WithEvents optAmt As RadioButton
    Friend WithEvents lblRecCnt As Label
    Friend WithEvents btnReplace As Button
    Friend WithEvents btnUpdtToDB As Button
    Friend WithEvents btnBack As Button
    Friend WithEvents btnApply As Button
    Friend WithEvents tt As ToolTip
    Friend WithEvents btnExport As C1.Win.C1Input.C1Button
    Friend WithEvents lblvsfItemGroup As Label
    Friend WithEvents btnExpXLvsfItemGroup As C1.Win.C1Input.C1Button
    Friend WithEvents chkAll As CheckBox
    Friend WithEvents lblItemSel As Label
    Friend WithEvents lblRateSel As Label
    Friend WithEvents Panel1 As Panel
    Public WithEvents lblFrm As Label
End Class
