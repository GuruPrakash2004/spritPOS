<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmUtlMstTrnBulkUpdate
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmUtlMstTrnBulkUpdate))
        Me.cboDocName = New System.Windows.Forms.ComboBox()
        Me.vsfDet = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.lblTotNoOfRec = New System.Windows.Forms.Label()
        Me.txtNoofRec = New System.Windows.Forms.TextBox()
        Me.btnExcel = New System.Windows.Forms.Button()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.btnDesign = New System.Windows.Forms.Button()
        Me.btnClearFilters = New System.Windows.Forms.Button()
        Me.btnRemFilter = New System.Windows.Forms.Button()
        Me.btnViewReport = New System.Windows.Forms.Button()
        Me.btnLoad = New System.Windows.Forms.Button()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.fraFilters = New System.Windows.Forms.Panel()
        Me.pnlFilter = New System.Windows.Forms.Panel()
        Me.vsfRetGrid = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.lblSelFilters = New System.Windows.Forms.Label()
        Me.vsfSelFilters = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.vsfFilter = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.txtSelect = New System.Windows.Forms.TextBox()
        Me.lblSelectFile = New System.Windows.Forms.Label()
        Me.btnSelect = New System.Windows.Forms.Button()
        Me.btnTemplate = New System.Windows.Forms.Button()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.vsfFlds = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.btnDBData = New System.Windows.Forms.Button()
        Me.txtTblName = New System.Windows.Forms.TextBox()
        Me.txtIDfld = New System.Windows.Forms.TextBox()
        Me.vsfTemp = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.lblLegend = New System.Windows.Forms.Label()
        Me.btnReplRest = New System.Windows.Forms.Button()
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.fraFilters.SuspendLayout()
        CType(Me.vsfRetGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.vsfSelFilters, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.vsfFilter, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.vsfFlds, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.vsfTemp, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cboDocName
        '
        Me.cboDocName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboDocName.Font = New System.Drawing.Font("Segoe UI", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboDocName.FormattingEnabled = True
        Me.cboDocName.Location = New System.Drawing.Point(302, 13)
        Me.cboDocName.Margin = New System.Windows.Forms.Padding(5, 7, 5, 7)
        Me.cboDocName.Name = "cboDocName"
        Me.cboDocName.Size = New System.Drawing.Size(338, 31)
        Me.cboDocName.TabIndex = 0
        '
        'vsfDet
        '
        Me.vsfDet.AllowFiltering = True
        Me.vsfDet.AllowFreezing = C1.Win.C1FlexGrid.AllowFreezingEnum.Columns
        Me.vsfDet.AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.Free
        Me.vsfDet.AllowMergingFixed = C1.Win.C1FlexGrid.AllowMergingEnum.Free
        Me.vsfDet.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfDet.AutoResize = True
        Me.vsfDet.AutoSearch = C1.Win.C1FlexGrid.AutoSearchEnum.FromTop
        Me.vsfDet.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle
        Me.vsfDet.ColumnInfo = "2,0,0,0,0,120,Columns:0{Width:146;}" & Global.Microsoft.VisualBasic.ChrW(9)
        Me.vsfDet.EditOptions = C1.Win.C1FlexGrid.EditFlags.AutoSearch
        Me.vsfDet.FocusRect = C1.Win.C1FlexGrid.FocusRectEnum.Heavy
        Me.vsfDet.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.vsfDet.KeyActionEnter = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross
        Me.vsfDet.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross
        Me.vsfDet.Location = New System.Drawing.Point(15, 394)
        Me.vsfDet.Margin = New System.Windows.Forms.Padding(5, 7, 5, 7)
        Me.vsfDet.Name = "vsfDet"
        Me.vsfDet.Rows.Count = 10
        Me.vsfDet.Rows.DefaultSize = 28
        Me.vsfDet.Rows.Fixed = 5
        Me.vsfDet.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfDet.Size = New System.Drawing.Size(1069, 160)
        Me.vsfDet.StyleInfo = resources.GetString("vsfDet.StyleInfo")
        Me.vsfDet.TabIndex = 2
        Me.vsfDet.VisualStyle = C1.Win.C1FlexGrid.VisualStyle.Office2007Blue
        '
        'lblTotNoOfRec
        '
        Me.lblTotNoOfRec.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblTotNoOfRec.AutoSize = True
        Me.lblTotNoOfRec.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTotNoOfRec.ForeColor = System.Drawing.Color.Blue
        Me.lblTotNoOfRec.Location = New System.Drawing.Point(52, 569)
        Me.lblTotNoOfRec.Margin = New System.Windows.Forms.Padding(5, 0, 5, 0)
        Me.lblTotNoOfRec.Name = "lblTotNoOfRec"
        Me.lblTotNoOfRec.Size = New System.Drawing.Size(269, 23)
        Me.lblTotNoOfRec.TabIndex = 292
        Me.lblTotNoOfRec.Tag = "R"
        Me.lblTotNoOfRec.Text = "Total Number of Records Loaded :"
        '
        'txtNoofRec
        '
        Me.txtNoofRec.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.txtNoofRec.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtNoofRec.Location = New System.Drawing.Point(741, 567)
        Me.txtNoofRec.Margin = New System.Windows.Forms.Padding(5, 7, 5, 7)
        Me.txtNoofRec.Name = "txtNoofRec"
        Me.txtNoofRec.ReadOnly = True
        Me.txtNoofRec.Size = New System.Drawing.Size(84, 26)
        Me.txtNoofRec.TabIndex = 2
        Me.txtNoofRec.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.txtNoofRec.Visible = False
        '
        'btnExcel
        '
        Me.btnExcel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnExcel.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnExcel.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnExcel.Image = Global.vLQMS.My.Resources.Resources.ucexport_to_XL
        Me.btnExcel.Location = New System.Drawing.Point(16, 561)
        Me.btnExcel.Margin = New System.Windows.Forms.Padding(5, 8, 5, 8)
        Me.btnExcel.Name = "btnExcel"
        Me.btnExcel.Size = New System.Drawing.Size(34, 29)
        Me.btnExcel.TabIndex = 1
        Me.ToolTip1.SetToolTip(Me.btnExcel, "Export to Excel : Alt + E")
        Me.btnExcel.UseVisualStyleBackColor = True
        '
        'btnClose
        '
        Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClose.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_CLOSE
        Me.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnClose.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnClose.Location = New System.Drawing.Point(995, 560)
        Me.btnClose.Margin = New System.Windows.Forms.Padding(5, 8, 5, 8)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(90, 30)
        Me.btnClose.TabIndex = 5
        Me.btnClose.Text = "Cl&ose"
        Me.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnClose, "Close:Alt+O")
        Me.btnClose.UseVisualStyleBackColor = True
        '
        'btnDesign
        '
        Me.btnDesign.BackColor = System.Drawing.SystemColors.InactiveBorder
        Me.btnDesign.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnDesign.Location = New System.Drawing.Point(863, 13)
        Me.btnDesign.Margin = New System.Windows.Forms.Padding(5, 8, 5, 8)
        Me.btnDesign.Name = "btnDesign"
        Me.btnDesign.Size = New System.Drawing.Size(184, 31)
        Me.btnDesign.TabIndex = 304
        Me.btnDesign.Text = "Master Data Design"
        Me.ToolTip1.SetToolTip(Me.btnDesign, "Freez : Alt + F")
        Me.btnDesign.UseVisualStyleBackColor = False
        '
        'btnClearFilters
        '
        Me.btnClearFilters.Enabled = False
        Me.btnClearFilters.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnClearFilters.Location = New System.Drawing.Point(444, 168)
        Me.btnClearFilters.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btnClearFilters.Name = "btnClearFilters"
        Me.btnClearFilters.Size = New System.Drawing.Size(133, 34)
        Me.btnClearFilters.TabIndex = 219
        Me.btnClearFilters.Text = "C&lear All Filters"
        Me.ToolTip1.SetToolTip(Me.btnClearFilters, "Clear Filters : Alt + L")
        Me.btnClearFilters.UseVisualStyleBackColor = False
        '
        'btnRemFilter
        '
        Me.btnRemFilter.Enabled = False
        Me.btnRemFilter.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnRemFilter.Location = New System.Drawing.Point(444, 124)
        Me.btnRemFilter.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btnRemFilter.Name = "btnRemFilter"
        Me.btnRemFilter.Size = New System.Drawing.Size(133, 34)
        Me.btnRemFilter.TabIndex = 218
        Me.btnRemFilter.Text = "&Remove Filter"
        Me.ToolTip1.SetToolTip(Me.btnRemFilter, "Remove Filter : Alt + R")
        Me.btnRemFilter.UseVisualStyleBackColor = False
        Me.btnRemFilter.Visible = False
        '
        'btnViewReport
        '
        Me.btnViewReport.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnViewReport.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.btnViewReport.Location = New System.Drawing.Point(444, 212)
        Me.btnViewReport.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btnViewReport.Name = "btnViewReport"
        Me.btnViewReport.Size = New System.Drawing.Size(133, 62)
        Me.btnViewReport.TabIndex = 211
        Me.btnViewReport.Text = "Load FIltered Data"
        Me.ToolTip1.SetToolTip(Me.btnViewReport, "View Report : Alt + V")
        Me.btnViewReport.UseVisualStyleBackColor = False
        '
        'btnLoad
        '
        Me.btnLoad.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnLoad.Location = New System.Drawing.Point(650, 12)
        Me.btnLoad.Margin = New System.Windows.Forms.Padding(5, 8, 5, 8)
        Me.btnLoad.Name = "btnLoad"
        Me.btnLoad.Size = New System.Drawing.Size(95, 32)
        Me.btnLoad.TabIndex = 306
        Me.btnLoad.Text = "Load"
        Me.ToolTip1.SetToolTip(Me.btnLoad, "Freez : Alt + F")
        Me.btnLoad.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnCancel.Location = New System.Drawing.Point(755, 12)
        Me.btnCancel.Margin = New System.Windows.Forms.Padding(5, 8, 5, 8)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(98, 32)
        Me.btnCancel.TabIndex = 315
        Me.btnCancel.Text = "Cancel"
        Me.ToolTip1.SetToolTip(Me.btnCancel, "Freez : Alt + F")
        Me.btnCancel.UseVisualStyleBackColor = True
        Me.btnCancel.Visible = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 17)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(295, 23)
        Me.Label1.TabIndex = 302
        Me.Label1.Text = "Select Doc Name (Press F5 to refresh)"
        '
        'fraFilters
        '
        Me.fraFilters.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.fraFilters.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.fraFilters.Controls.Add(Me.pnlFilter)
        Me.fraFilters.Controls.Add(Me.vsfRetGrid)
        Me.fraFilters.Controls.Add(Me.btnClearFilters)
        Me.fraFilters.Controls.Add(Me.btnRemFilter)
        Me.fraFilters.Controls.Add(Me.lblSelFilters)
        Me.fraFilters.Controls.Add(Me.vsfSelFilters)
        Me.fraFilters.Controls.Add(Me.Label2)
        Me.fraFilters.Controls.Add(Me.vsfFilter)
        Me.fraFilters.Controls.Add(Me.btnViewReport)
        Me.fraFilters.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.fraFilters.Location = New System.Drawing.Point(494, 69)
        Me.fraFilters.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.fraFilters.Name = "fraFilters"
        Me.fraFilters.Size = New System.Drawing.Size(590, 283)
        Me.fraFilters.TabIndex = 305
        '
        'pnlFilter
        '
        Me.pnlFilter.Location = New System.Drawing.Point(4, 143)
        Me.pnlFilter.Name = "pnlFilter"
        Me.pnlFilter.Size = New System.Drawing.Size(432, 131)
        Me.pnlFilter.TabIndex = 319
        '
        'vsfRetGrid
        '
        Me.vsfRetGrid.AllowEditing = False
        Me.vsfRetGrid.AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.RestrictAll
        Me.vsfRetGrid.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None
        Me.vsfRetGrid.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle
        Me.vsfRetGrid.ColumnInfo = "1,0,0,0,0,95,Columns:"
        Me.vsfRetGrid.ExtendLastCol = True
        Me.vsfRetGrid.FocusRect = C1.Win.C1FlexGrid.FocusRectEnum.Solid
        Me.vsfRetGrid.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!)
        Me.vsfRetGrid.Location = New System.Drawing.Point(206, 49)
        Me.vsfRetGrid.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.vsfRetGrid.Name = "vsfRetGrid"
        Me.vsfRetGrid.Rows.Count = 4
        Me.vsfRetGrid.Rows.DefaultSize = 22
        Me.vsfRetGrid.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfRetGrid.Size = New System.Drawing.Size(161, 55)
        Me.vsfRetGrid.StyleInfo = resources.GetString("vsfRetGrid.StyleInfo")
        Me.vsfRetGrid.TabIndex = 222
        Me.vsfRetGrid.Tag = "DSGR"
        Me.vsfRetGrid.Visible = False
        '
        'lblSelFilters
        '
        Me.lblSelFilters.AutoSize = True
        Me.lblSelFilters.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSelFilters.Location = New System.Drawing.Point(4, 140)
        Me.lblSelFilters.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblSelFilters.Name = "lblSelFilters"
        Me.lblSelFilters.Size = New System.Drawing.Size(156, 28)
        Me.lblSelFilters.TabIndex = 217
        Me.lblSelFilters.Text = "Selected Filters"
        '
        'vsfSelFilters
        '
        Me.vsfSelFilters.AllowDragging = C1.Win.C1FlexGrid.AllowDraggingEnum.None
        Me.vsfSelFilters.AllowEditing = False
        Me.vsfSelFilters.AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.RestrictAll
        Me.vsfSelFilters.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None
        Me.vsfSelFilters.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle
        Me.vsfSelFilters.ColumnInfo = "1,0,0,0,0,95,Columns:"
        Me.vsfSelFilters.ExtendLastCol = True
        Me.vsfSelFilters.FocusRect = C1.Win.C1FlexGrid.FocusRectEnum.Solid
        Me.vsfSelFilters.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!)
        Me.vsfSelFilters.Location = New System.Drawing.Point(4, 169)
        Me.vsfSelFilters.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.vsfSelFilters.Name = "vsfSelFilters"
        Me.vsfSelFilters.Rows.Count = 4
        Me.vsfSelFilters.Rows.DefaultSize = 22
        Me.vsfSelFilters.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfSelFilters.Size = New System.Drawing.Size(432, 105)
        Me.vsfSelFilters.StyleInfo = resources.GetString("vsfSelFilters.StyleInfo")
        Me.vsfSelFilters.TabIndex = 216
        Me.vsfSelFilters.Tag = "DSGR"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(4, 0)
        Me.Label2.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(85, 28)
        Me.Label2.TabIndex = 215
        Me.Label2.Text = "Filters..."
        '
        'vsfFilter
        '
        Me.vsfFilter.AllowDragging = C1.Win.C1FlexGrid.AllowDraggingEnum.None
        Me.vsfFilter.AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.RestrictAll
        Me.vsfFilter.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None
        Me.vsfFilter.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle
        Me.vsfFilter.ColumnInfo = "1,0,0,0,0,95,Columns:"
        Me.vsfFilter.ExtendLastCol = True
        Me.vsfFilter.FocusRect = C1.Win.C1FlexGrid.FocusRectEnum.Solid
        Me.vsfFilter.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!)
        Me.vsfFilter.Location = New System.Drawing.Point(4, 28)
        Me.vsfFilter.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.vsfFilter.Name = "vsfFilter"
        Me.vsfFilter.Rows.Count = 4
        Me.vsfFilter.Rows.DefaultSize = 22
        Me.vsfFilter.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfFilter.ShowButtons = C1.Win.C1FlexGrid.ShowButtonsEnum.Always
        Me.vsfFilter.Size = New System.Drawing.Size(432, 107)
        Me.vsfFilter.StyleInfo = resources.GetString("vsfFilter.StyleInfo")
        Me.vsfFilter.TabIndex = 214
        Me.vsfFilter.Tag = "DSGR"
        '
        'txtSelect
        '
        Me.txtSelect.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtSelect.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSelect.Location = New System.Drawing.Point(579, 358)
        Me.txtSelect.MaxLength = 100
        Me.txtSelect.Name = "txtSelect"
        Me.txtSelect.Size = New System.Drawing.Size(464, 30)
        Me.txtSelect.TabIndex = 309
        '
        'lblSelectFile
        '
        Me.lblSelectFile.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblSelectFile.AutoSize = True
        Me.lblSelectFile.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSelectFile.Location = New System.Drawing.Point(472, 360)
        Me.lblSelectFile.Name = "lblSelectFile"
        Me.lblSelectFile.Size = New System.Drawing.Size(103, 25)
        Me.lblSelectFile.TabIndex = 308
        Me.lblSelectFile.Text = "Select File"
        '
        'btnSelect
        '
        Me.btnSelect.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSelect.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSelect.Location = New System.Drawing.Point(1049, 355)
        Me.btnSelect.Name = "btnSelect"
        Me.btnSelect.Size = New System.Drawing.Size(35, 35)
        Me.btnSelect.TabIndex = 307
        Me.btnSelect.Text = "..."
        Me.btnSelect.UseVisualStyleBackColor = True
        '
        'btnTemplate
        '
        Me.btnTemplate.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnTemplate.Location = New System.Drawing.Point(16, 354)
        Me.btnTemplate.Margin = New System.Windows.Forms.Padding(2)
        Me.btnTemplate.Name = "btnTemplate"
        Me.btnTemplate.Size = New System.Drawing.Size(242, 32)
        Me.btnTemplate.TabIndex = 312
        Me.btnTemplate.Text = "Generate Excel to edit Data"
        Me.btnTemplate.UseVisualStyleBackColor = True
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(13, 49)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(177, 20)
        Me.Label3.TabIndex = 311
        Me.Label3.Text = "Select Fields to import"
        '
        'vsfFlds
        '
        Me.vsfFlds.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle
        Me.vsfFlds.ColumnInfo = "10,0,0,0,0,145,Columns:0{Width:182;}" & Global.Microsoft.VisualBasic.ChrW(9)
        Me.vsfFlds.ExtendLastCol = True
        Me.vsfFlds.Font = New System.Drawing.Font("Segoe UI", 10.0!)
        Me.vsfFlds.Location = New System.Drawing.Point(16, 69)
        Me.vsfFlds.Margin = New System.Windows.Forms.Padding(2)
        Me.vsfFlds.Name = "vsfFlds"
        Me.vsfFlds.Rows.DefaultSize = 29
        Me.vsfFlds.Size = New System.Drawing.Size(472, 283)
        Me.vsfFlds.StyleInfo = resources.GetString("vsfFlds.StyleInfo")
        Me.vsfFlds.TabIndex = 310
        '
        'btnSave
        '
        Me.btnSave.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSave.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSave.Location = New System.Drawing.Point(832, 558)
        Me.btnSave.Margin = New System.Windows.Forms.Padding(2)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(156, 32)
        Me.btnSave.TabIndex = 313
        Me.btnSave.Text = "Upload Data"
        Me.btnSave.UseVisualStyleBackColor = True
        Me.btnSave.Visible = False
        '
        'btnDBData
        '
        Me.btnDBData.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnDBData.Location = New System.Drawing.Point(262, 354)
        Me.btnDBData.Margin = New System.Windows.Forms.Padding(2)
        Me.btnDBData.Name = "btnDBData"
        Me.btnDBData.Size = New System.Drawing.Size(211, 32)
        Me.btnDBData.TabIndex = 314
        Me.btnDBData.Text = "Load Data to edit in Grid"
        Me.btnDBData.UseVisualStyleBackColor = True
        '
        'txtTblName
        '
        Me.txtTblName.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.txtTblName.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTblName.Location = New System.Drawing.Point(959, 18)
        Me.txtTblName.Margin = New System.Windows.Forms.Padding(5, 7, 5, 7)
        Me.txtTblName.Name = "txtTblName"
        Me.txtTblName.ReadOnly = True
        Me.txtTblName.Size = New System.Drawing.Size(84, 26)
        Me.txtTblName.TabIndex = 316
        Me.txtTblName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.txtTblName.Visible = False
        '
        'txtIDfld
        '
        Me.txtIDfld.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.txtIDfld.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtIDfld.Location = New System.Drawing.Point(959, 43)
        Me.txtIDfld.Margin = New System.Windows.Forms.Padding(5, 7, 5, 7)
        Me.txtIDfld.Name = "txtIDfld"
        Me.txtIDfld.ReadOnly = True
        Me.txtIDfld.Size = New System.Drawing.Size(84, 26)
        Me.txtIDfld.TabIndex = 317
        Me.txtIDfld.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.txtIDfld.Visible = False
        '
        'vsfTemp
        '
        Me.vsfTemp.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle
        Me.vsfTemp.ColumnInfo = "10,0,0,0,0,145,Columns:0{Width:182;}" & Global.Microsoft.VisualBasic.ChrW(9)
        Me.vsfTemp.ExtendLastCol = True
        Me.vsfTemp.Font = New System.Drawing.Font("Segoe UI", 10.0!)
        Me.vsfTemp.Location = New System.Drawing.Point(1014, 111)
        Me.vsfTemp.Margin = New System.Windows.Forms.Padding(2)
        Me.vsfTemp.Name = "vsfTemp"
        Me.vsfTemp.Rows.DefaultSize = 29
        Me.vsfTemp.Size = New System.Drawing.Size(71, 127)
        Me.vsfTemp.StyleInfo = resources.GetString("vsfTemp.StyleInfo")
        Me.vsfTemp.TabIndex = 318
        Me.vsfTemp.Visible = False
        '
        'lblLegend
        '
        Me.lblLegend.AutoSize = True
        Me.lblLegend.BackColor = System.Drawing.Color.Transparent
        Me.lblLegend.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblLegend.Location = New System.Drawing.Point(867, 43)
        Me.lblLegend.Name = "lblLegend"
        Me.lblLegend.Size = New System.Drawing.Size(292, 20)
        Me.lblLegend.TabIndex = 319
        Me.lblLegend.Text = "Select fields to import master data records"
        '
        'btnReplRest
        '
        Me.btnReplRest.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnReplRest.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnReplRest.Location = New System.Drawing.Point(422, 564)
        Me.btnReplRest.Margin = New System.Windows.Forms.Padding(2)
        Me.btnReplRest.Name = "btnReplRest"
        Me.btnReplRest.Size = New System.Drawing.Size(124, 32)
        Me.btnReplRest.TabIndex = 320
        Me.btnReplRest.Text = "Replace Rest"
        Me.btnReplRest.UseVisualStyleBackColor = True
        Me.btnReplRest.Visible = False
        '
        'frmUtlMstTrnBulkUpdate
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 23.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(188, Byte), Integer), CType(CType(205, Byte), Integer), CType(CType(227, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(1087, 590)
        Me.Controls.Add(Me.btnReplRest)
        Me.Controls.Add(Me.lblLegend)
        Me.Controls.Add(Me.vsfTemp)
        Me.Controls.Add(Me.txtIDfld)
        Me.Controls.Add(Me.txtTblName)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnDBData)
        Me.Controls.Add(Me.btnSave)
        Me.Controls.Add(Me.btnTemplate)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.vsfFlds)
        Me.Controls.Add(Me.txtSelect)
        Me.Controls.Add(Me.lblSelectFile)
        Me.Controls.Add(Me.btnSelect)
        Me.Controls.Add(Me.txtNoofRec)
        Me.Controls.Add(Me.btnLoad)
        Me.Controls.Add(Me.lblTotNoOfRec)
        Me.Controls.Add(Me.fraFilters)
        Me.Controls.Add(Me.btnDesign)
        Me.Controls.Add(Me.btnExcel)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.vsfDet)
        Me.Controls.Add(Me.cboDocName)
        Me.Font = New System.Drawing.Font("Segoe UI", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Margin = New System.Windows.Forms.Padding(5, 7, 5, 7)
        Me.Name = "frmUtlMstTrnBulkUpdate"
        Me.ShowIcon = False
        Me.Text = "Master Transaction Bulk Update"
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).EndInit()
        Me.fraFilters.ResumeLayout(False)
        Me.fraFilters.PerformLayout()
        CType(Me.vsfRetGrid, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.vsfSelFilters, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.vsfFilter, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.vsfFlds, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.vsfTemp, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents cboDocName As ComboBox
    Friend WithEvents vsfDet As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents lblTotNoOfRec As Label
    Friend WithEvents txtNoofRec As TextBox
    Friend WithEvents btnExcel As Button
    Friend WithEvents btnClose As Button
    Friend WithEvents ToolTip1 As ToolTip
    Friend WithEvents Label1 As Label
    Friend WithEvents btnDesign As Button
    Friend WithEvents fraFilters As Panel
    Public WithEvents vsfRetGrid As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents btnClearFilters As Button
    Friend WithEvents btnRemFilter As Button
    Friend WithEvents lblSelFilters As Label
    Friend WithEvents vsfSelFilters As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents Label2 As Label
    Friend WithEvents vsfFilter As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents btnViewReport As Button
    Friend WithEvents btnLoad As Button
    Friend WithEvents txtSelect As TextBox
    Friend WithEvents lblSelectFile As Label
    Friend WithEvents btnSelect As Button
    Friend WithEvents btnTemplate As Button
    Friend WithEvents Label3 As Label
    Private WithEvents vsfFlds As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents btnSave As Button
    Friend WithEvents btnDBData As Button
    Friend WithEvents btnCancel As Button
    Friend WithEvents txtTblName As TextBox
    Friend WithEvents txtIDfld As TextBox
    Private WithEvents vsfTemp As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents pnlFilter As Panel
    Friend WithEvents lblLegend As Label
    Friend WithEvents btnReplRest As Button
End Class
