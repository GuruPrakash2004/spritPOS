<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmUtlGenDoc
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmUtlGenDoc))
        Me.cboCategory = New System.Windows.Forms.ComboBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.btnLoad = New System.Windows.Forms.Button()
        Me.btnImport = New System.Windows.Forms.Button()
        Me.vsfCustItemList = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.chkSelectAll = New System.Windows.Forms.CheckBox()
        Me.gb = New System.Windows.Forms.GroupBox()
        Me.lblHelp = New System.Windows.Forms.Label()
        Me.btnExport = New System.Windows.Forms.Button()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.btnRemove = New System.Windows.Forms.Button()
        Me.tooltip = New System.Windows.Forms.ToolTip(Me.components)
        Me.btnGenerate = New System.Windows.Forms.Button()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.cdlg = New System.Windows.Forms.OpenFileDialog()
        Me.vsfCustItemListDummy = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.vsfExpStruct = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.vsfGenCopy = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.fraSearch = New System.Windows.Forms.Panel()
        Me.vsfSearch = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.btnCloseSearch = New System.Windows.Forms.Button()
        Me.vsfView = New C1.Win.C1FlexGrid.C1FlexGrid()
        CType(Me.vsfCustItemList, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.gb.SuspendLayout()
        CType(Me.vsfCustItemListDummy, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.vsfExpStruct, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.vsfGenCopy, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.fraSearch.SuspendLayout()
        CType(Me.vsfSearch, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.vsfView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cboCategory
        '
        Me.cboCategory.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest
        Me.cboCategory.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cboCategory.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.cboCategory.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboCategory.FormattingEnabled = True
        Me.cboCategory.ItemHeight = 17
        Me.cboCategory.Location = New System.Drawing.Point(89, 9)
        Me.cboCategory.MaxDropDownItems = 5
        Me.cboCategory.MaxLength = 10
        Me.cboCategory.Name = "cboCategory"
        Me.cboCategory.Size = New System.Drawing.Size(305, 25)
        Me.cboCategory.TabIndex = 1
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.Red
        Me.Label1.Location = New System.Drawing.Point(12, 13)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(61, 17)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Category"
        '
        'btnLoad
        '
        Me.btnLoad.BackColor = System.Drawing.Color.Transparent
        Me.btnLoad.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnLoad.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnLoad.Image = CType(resources.GetObject("btnLoad.Image"), System.Drawing.Image)
        Me.btnLoad.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnLoad.Location = New System.Drawing.Point(400, 7)
        Me.btnLoad.Name = "btnLoad"
        Me.btnLoad.Size = New System.Drawing.Size(73, 29)
        Me.btnLoad.TabIndex = 2
        Me.btnLoad.Text = "&Load"
        Me.btnLoad.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.tooltip.SetToolTip(Me.btnLoad, "Load : Alt + L")
        Me.btnLoad.UseVisualStyleBackColor = False
        '
        'btnImport
        '
        Me.btnImport.BackColor = System.Drawing.Color.Transparent
        Me.btnImport.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnImport.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnImport.Image = CType(resources.GetObject("btnImport.Image"), System.Drawing.Image)
        Me.btnImport.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnImport.Location = New System.Drawing.Point(479, 7)
        Me.btnImport.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnImport.Name = "btnImport"
        Me.btnImport.Size = New System.Drawing.Size(159, 29)
        Me.btnImport.TabIndex = 4
        Me.btnImport.Text = "&Import from Excel"
        Me.btnImport.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.tooltip.SetToolTip(Me.btnImport, "Import from Excel : Alt + I")
        Me.btnImport.UseVisualStyleBackColor = False
        '
        'vsfCustItemList
        '
        Me.vsfCustItemList.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None
        Me.vsfCustItemList.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfCustItemList.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle
        Me.vsfCustItemList.ColumnInfo = "2,1,0,0,0,120,Columns:"
        Me.vsfCustItemList.ExtendLastCol = True
        Me.vsfCustItemList.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.vsfCustItemList.KeyActionEnter = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross
        Me.vsfCustItemList.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross
        Me.vsfCustItemList.Location = New System.Drawing.Point(12, 39)
        Me.vsfCustItemList.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.vsfCustItemList.Name = "vsfCustItemList"
        Me.vsfCustItemList.Rows.DefaultSize = 24
        Me.vsfCustItemList.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfCustItemList.Size = New System.Drawing.Size(782, 380)
        Me.vsfCustItemList.StyleInfo = resources.GetString("vsfCustItemList.StyleInfo")
        Me.vsfCustItemList.TabIndex = 5
        Me.vsfCustItemList.Tag = ""
        '
        'chkSelectAll
        '
        Me.chkSelectAll.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.chkSelectAll.AutoSize = True
        Me.chkSelectAll.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkSelectAll.Location = New System.Drawing.Point(12, 434)
        Me.chkSelectAll.Name = "chkSelectAll"
        Me.chkSelectAll.Size = New System.Drawing.Size(79, 21)
        Me.chkSelectAll.TabIndex = 6
        Me.chkSelectAll.Text = "Select All"
        Me.chkSelectAll.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.tooltip.SetToolTip(Me.chkSelectAll, "Select All")
        Me.chkSelectAll.UseVisualStyleBackColor = True
        '
        'gb
        '
        Me.gb.Controls.Add(Me.lblHelp)
        Me.gb.Location = New System.Drawing.Point(414, 421)
        Me.gb.Name = "gb"
        Me.gb.Size = New System.Drawing.Size(380, 100)
        Me.gb.TabIndex = 7
        Me.gb.TabStop = False
        '
        'lblHelp
        '
        Me.lblHelp.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblHelp.ForeColor = System.Drawing.Color.Blue
        Me.lblHelp.Location = New System.Drawing.Point(6, 13)
        Me.lblHelp.Name = "lblHelp"
        Me.lblHelp.Size = New System.Drawing.Size(368, 84)
        Me.lblHelp.TabIndex = 0
        '
        'btnExport
        '
        Me.btnExport.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnExport.BackColor = System.Drawing.Color.Transparent
        Me.btnExport.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnExport.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnExport.Image = CType(resources.GetObject("btnExport.Image"), System.Drawing.Image)
        Me.btnExport.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnExport.Location = New System.Drawing.Point(561, 533)
        Me.btnExport.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnExport.Name = "btnExport"
        Me.btnExport.Size = New System.Drawing.Size(152, 29)
        Me.btnExport.TabIndex = 8
        Me.btnExport.Text = "&Export Template"
        Me.btnExport.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.tooltip.SetToolTip(Me.btnExport, "Export Template : Alt + E")
        Me.btnExport.UseVisualStyleBackColor = False
        '
        'btnClose
        '
        Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClose.BackColor = System.Drawing.Color.Transparent
        Me.btnClose.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_CLOSE
        Me.btnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnClose.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnClose.Location = New System.Drawing.Point(719, 533)
        Me.btnClose.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(75, 29)
        Me.btnClose.TabIndex = 9
        Me.btnClose.Text = "&Close"
        Me.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.tooltip.SetToolTip(Me.btnClose, "Close : Alt + C")
        Me.btnClose.UseVisualStyleBackColor = False
        '
        'btnRemove
        '
        Me.btnRemove.BackColor = System.Drawing.Color.Transparent
        Me.btnRemove.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.btnRemove.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnRemove.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnRemove.Image = CType(resources.GetObject("btnRemove.Image"), System.Drawing.Image)
        Me.btnRemove.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnRemove.Location = New System.Drawing.Point(116, 427)
        Me.btnRemove.Name = "btnRemove"
        Me.btnRemove.Size = New System.Drawing.Size(148, 29)
        Me.btnRemove.TabIndex = 10
        Me.btnRemove.Text = "Re&move from Grid"
        Me.btnRemove.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.tooltip.SetToolTip(Me.btnRemove, "Remove From Grid : Alt +M")
        Me.btnRemove.UseVisualStyleBackColor = False
        '
        'btnGenerate
        '
        Me.btnGenerate.BackColor = System.Drawing.Color.Transparent
        Me.btnGenerate.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.btnGenerate.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnGenerate.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnGenerate.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnGenerate.Location = New System.Drawing.Point(12, 462)
        Me.btnGenerate.Name = "btnGenerate"
        Me.btnGenerate.Size = New System.Drawing.Size(252, 29)
        Me.btnGenerate.TabIndex = 11
        Me.btnGenerate.Text = "Generate Documents"
        Me.tooltip.SetToolTip(Me.btnGenerate, "Generate Documents")
        Me.btnGenerate.UseVisualStyleBackColor = False
        '
        'btnSave
        '
        Me.btnSave.BackColor = System.Drawing.Color.Transparent
        Me.btnSave.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_SAVE
        Me.btnSave.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSave.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnSave.Location = New System.Drawing.Point(77, 497)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(87, 29)
        Me.btnSave.TabIndex = 12
        Me.btnSave.Text = "&Save"
        Me.btnSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.tooltip.SetToolTip(Me.btnSave, "Save : Alt + S")
        Me.btnSave.UseVisualStyleBackColor = False
        '
        'btnCancel
        '
        Me.btnCancel.BackColor = System.Drawing.Color.Transparent
        Me.btnCancel.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_CANCEL
        Me.btnCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnCancel.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnCancel.Location = New System.Drawing.Point(174, 497)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(87, 29)
        Me.btnCancel.TabIndex = 13
        Me.btnCancel.Text = "Ca&ncel"
        Me.btnCancel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.tooltip.SetToolTip(Me.btnCancel, "Cancel : Alt + N")
        Me.btnCancel.UseVisualStyleBackColor = False
        '
        'vsfCustItemListDummy
        '
        Me.vsfCustItemListDummy.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None
        Me.vsfCustItemListDummy.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfCustItemListDummy.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle
        Me.vsfCustItemListDummy.ColumnInfo = "2,1,0,0,0,120,Columns:"
        Me.vsfCustItemListDummy.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.vsfCustItemListDummy.KeyActionEnter = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross
        Me.vsfCustItemListDummy.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross
        Me.vsfCustItemListDummy.Location = New System.Drawing.Point(283, 427)
        Me.vsfCustItemListDummy.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.vsfCustItemListDummy.Name = "vsfCustItemListDummy"
        Me.vsfCustItemListDummy.Rows.DefaultSize = 24
        Me.vsfCustItemListDummy.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfCustItemListDummy.Size = New System.Drawing.Size(131, 135)
        Me.vsfCustItemListDummy.StyleInfo = resources.GetString("vsfCustItemListDummy.StyleInfo")
        Me.vsfCustItemListDummy.TabIndex = 14
        Me.vsfCustItemListDummy.Tag = ""
        Me.vsfCustItemListDummy.Visible = False
        '
        'vsfExpStruct
        '
        Me.vsfExpStruct.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None
        Me.vsfExpStruct.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfExpStruct.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle
        Me.vsfExpStruct.ColumnInfo = "2,1,0,0,0,120,Columns:"
        Me.vsfExpStruct.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.vsfExpStruct.KeyActionEnter = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross
        Me.vsfExpStruct.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross
        Me.vsfExpStruct.Location = New System.Drawing.Point(32, 540)
        Me.vsfExpStruct.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.vsfExpStruct.Name = "vsfExpStruct"
        Me.vsfExpStruct.Rows.DefaultSize = 24
        Me.vsfExpStruct.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfExpStruct.Size = New System.Drawing.Size(382, 29)
        Me.vsfExpStruct.StyleInfo = resources.GetString("vsfExpStruct.StyleInfo")
        Me.vsfExpStruct.TabIndex = 15
        Me.vsfExpStruct.Tag = ""
        Me.vsfExpStruct.Visible = False
        '
        'vsfGenCopy
        '
        Me.vsfGenCopy.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None
        Me.vsfGenCopy.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfGenCopy.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle
        Me.vsfGenCopy.ColumnInfo = "2,1,0,0,0,120,Columns:"
        Me.vsfGenCopy.ExtendLastCol = True
        Me.vsfGenCopy.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.vsfGenCopy.KeyActionEnter = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross
        Me.vsfGenCopy.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross
        Me.vsfGenCopy.Location = New System.Drawing.Point(12, 94)
        Me.vsfGenCopy.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.vsfGenCopy.Name = "vsfGenCopy"
        Me.vsfGenCopy.Rows.DefaultSize = 24
        Me.vsfGenCopy.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfGenCopy.Size = New System.Drawing.Size(782, 326)
        Me.vsfGenCopy.StyleInfo = resources.GetString("vsfGenCopy.StyleInfo")
        Me.vsfGenCopy.TabIndex = 16
        Me.vsfGenCopy.Tag = ""
        Me.vsfGenCopy.Visible = False
        '
        'fraSearch
        '
        Me.fraSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.fraSearch.Controls.Add(Me.vsfSearch)
        Me.fraSearch.Controls.Add(Me.btnCloseSearch)
        Me.fraSearch.Controls.Add(Me.vsfView)
        Me.fraSearch.Cursor = System.Windows.Forms.Cursors.Arrow
        Me.fraSearch.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.fraSearch.Location = New System.Drawing.Point(116, 94)
        Me.fraSearch.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.fraSearch.Name = "fraSearch"
        Me.fraSearch.Size = New System.Drawing.Size(459, 182)
        Me.fraSearch.TabIndex = 36
        '
        'vsfSearch
        '
        Me.vsfSearch.AllowFiltering = True
        Me.vsfSearch.AllowFreezing = C1.Win.C1FlexGrid.AllowFreezingEnum.Columns
        Me.vsfSearch.AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.Free
        Me.vsfSearch.AllowMergingFixed = C1.Win.C1FlexGrid.AllowMergingEnum.Free
        Me.vsfSearch.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfSearch.AutoResize = True
        Me.vsfSearch.AutoSearch = C1.Win.C1FlexGrid.AutoSearchEnum.FromTop
        Me.vsfSearch.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle
        Me.vsfSearch.ColumnInfo = "2,0,0,0,0,120,Columns:"
        Me.vsfSearch.EditOptions = C1.Win.C1FlexGrid.EditFlags.AutoSearch
        Me.vsfSearch.ExtendLastCol = True
        Me.vsfSearch.FocusRect = C1.Win.C1FlexGrid.FocusRectEnum.Heavy
        Me.vsfSearch.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.vsfSearch.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross
        Me.vsfSearch.Location = New System.Drawing.Point(3, 4)
        Me.vsfSearch.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.vsfSearch.Name = "vsfSearch"
        Me.vsfSearch.Rows.Count = 1
        Me.vsfSearch.Rows.DefaultSize = 24
        Me.vsfSearch.Rows.Fixed = 0
        Me.vsfSearch.ScrollBars = System.Windows.Forms.ScrollBars.None
        Me.vsfSearch.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfSearch.ShowCellLabels = True
        Me.vsfSearch.Size = New System.Drawing.Size(425, 26)
        Me.vsfSearch.StyleInfo = resources.GetString("vsfSearch.StyleInfo")
        Me.vsfSearch.TabIndex = 0
        Me.vsfSearch.Tag = ""
        Me.vsfSearch.VisualStyle = C1.Win.C1FlexGrid.VisualStyle.System
        '
        'btnCloseSearch
        '
        Me.btnCloseSearch.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCloseSearch.BackColor = System.Drawing.Color.FromArgb(CType(CType(233, Byte), Integer), CType(CType(237, Byte), Integer), CType(CType(241, Byte), Integer))
        Me.btnCloseSearch.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.btnCloseSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnCloseSearch.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.btnCloseSearch.Image = CType(resources.GetObject("btnCloseSearch.Image"), System.Drawing.Image)
        Me.btnCloseSearch.Location = New System.Drawing.Point(428, 4)
        Me.btnCloseSearch.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnCloseSearch.Name = "btnCloseSearch"
        Me.btnCloseSearch.Size = New System.Drawing.Size(26, 26)
        Me.btnCloseSearch.TabIndex = 28
        Me.btnCloseSearch.UseVisualStyleBackColor = False
        '
        'vsfView
        '
        Me.vsfView.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfView.ColumnInfo = "10,0,0,0,0,120,Columns:"
        Me.vsfView.ExtendLastCol = True
        Me.vsfView.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.vsfView.Location = New System.Drawing.Point(3, 29)
        Me.vsfView.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.vsfView.Name = "vsfView"
        Me.vsfView.Rows.DefaultSize = 24
        Me.vsfView.Size = New System.Drawing.Size(451, 147)
        Me.vsfView.StyleInfo = resources.GetString("vsfView.StyleInfo")
        Me.vsfView.TabIndex = 1
        '
        'frmUtlGenDoc
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(188, Byte), Integer), CType(CType(205, Byte), Integer), CType(CType(227, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(796, 570)
        Me.Controls.Add(Me.vsfExpStruct)
        Me.Controls.Add(Me.fraSearch)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.cboCategory)
        Me.Controls.Add(Me.btnLoad)
        Me.Controls.Add(Me.btnImport)
        Me.Controls.Add(Me.vsfCustItemList)
        Me.Controls.Add(Me.chkSelectAll)
        Me.Controls.Add(Me.gb)
        Me.Controls.Add(Me.btnExport)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.btnRemove)
        Me.Controls.Add(Me.btnGenerate)
        Me.Controls.Add(Me.btnSave)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.vsfCustItemListDummy)
        Me.Controls.Add(Me.vsfGenCopy)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MaximumSize = New System.Drawing.Size(812, 608)
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(812, 608)
        Me.Name = "frmUtlGenDoc"
        Me.ShowIcon = False
        Me.Text = "Document Generation"
        CType(Me.vsfCustItemList, System.ComponentModel.ISupportInitialize).EndInit()
        Me.gb.ResumeLayout(False)
        CType(Me.vsfCustItemListDummy, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.vsfExpStruct, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.vsfGenCopy, System.ComponentModel.ISupportInitialize).EndInit()
        Me.fraSearch.ResumeLayout(False)
        CType(Me.vsfSearch, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.vsfView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents btnLoad As System.Windows.Forms.Button
    Friend WithEvents btnImport As System.Windows.Forms.Button
    Public WithEvents vsfCustItemList As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents chkSelectAll As System.Windows.Forms.CheckBox
    Friend WithEvents gb As System.Windows.Forms.GroupBox
    Friend WithEvents lblHelp As System.Windows.Forms.Label
    Friend WithEvents btnExport As System.Windows.Forms.Button
    Friend WithEvents btnClose As System.Windows.Forms.Button
    Friend WithEvents btnRemove As System.Windows.Forms.Button
    Friend WithEvents tooltip As System.Windows.Forms.ToolTip
    Friend WithEvents btnGenerate As System.Windows.Forms.Button
    Friend WithEvents btnSave As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents cdlg As System.Windows.Forms.OpenFileDialog
    Public WithEvents vsfCustItemListDummy As C1.Win.C1FlexGrid.C1FlexGrid
    Public WithEvents vsfExpStruct As C1.Win.C1FlexGrid.C1FlexGrid
    Public WithEvents vsfGenCopy As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents cboCategory As System.Windows.Forms.ComboBox
    Friend WithEvents fraSearch As System.Windows.Forms.Panel
    Friend WithEvents vsfSearch As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents btnCloseSearch As System.Windows.Forms.Button
    Friend WithEvents vsfView As C1.Win.C1FlexGrid.C1FlexGrid
End Class
