<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmRptCustomiseRptView
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmRptCustomiseRptView))
        Me.vsfDet = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.btnViewReport = New System.Windows.Forms.Button()
        Me.txtRptName = New System.Windows.Forms.TextBox()
        Me.txtDesc = New System.Windows.Forms.TextBox()
        Me.fraFilters = New System.Windows.Forms.Panel()
        Me.btnPrintReport = New System.Windows.Forms.Button()
        Me.vsfRetGrid = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.btnClearFilters = New System.Windows.Forms.Button()
        Me.btnRemFilter = New System.Windows.Forms.Button()
        Me.lblSelFilters = New System.Windows.Forms.Label()
        Me.vsfSelFilters = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.vsfFilter = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.lbl = New System.Windows.Forms.Label()
        Me.lblReportName = New System.Windows.Forms.Label()
        Me.lblSearch = New System.Windows.Forms.Label()
        Me.txtSearch = New System.Windows.Forms.TextBox()
        Me.vsfSearch = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.lblDescription = New System.Windows.Forms.Label()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.btnImport = New System.Windows.Forms.Button()
        Me.btnRights = New System.Windows.Forms.Button()
        Me.btnDesign = New System.Windows.Forms.Button()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.vsfExp = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.btnEmail = New System.Windows.Forms.Button()
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.fraFilters.SuspendLayout()
        CType(Me.vsfRetGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.vsfSelFilters, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.vsfFilter, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.vsfSearch, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.vsfExp, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'vsfDet
        '
        Me.vsfDet.AllowEditing = False
        Me.vsfDet.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfDet.BackColor = System.Drawing.SystemColors.Control
        Me.vsfDet.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.None
        Me.vsfDet.ColumnInfo = "2,0,0,0,0,120,Columns:"
        Me.vsfDet.ExtendLastCol = True
        Me.vsfDet.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.vsfDet.Location = New System.Drawing.Point(12, 37)
        Me.vsfDet.Name = "vsfDet"
        Me.vsfDet.Rows.Count = 4
        Me.vsfDet.Rows.DefaultSize = 24
        Me.vsfDet.Rows.Fixed = 0
        Me.vsfDet.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfDet.Size = New System.Drawing.Size(473, 431)
        Me.vsfDet.StyleInfo = resources.GetString("vsfDet.StyleInfo")
        Me.vsfDet.TabIndex = 205
        Me.vsfDet.Tag = "DSGR"
        '
        'btnViewReport
        '
        Me.btnViewReport.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnViewReport.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnViewReport.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.btnViewReport.Location = New System.Drawing.Point(426, 333)
        Me.btnViewReport.Name = "btnViewReport"
        Me.btnViewReport.Size = New System.Drawing.Size(89, 29)
        Me.btnViewReport.TabIndex = 211
        Me.btnViewReport.Text = "&View Report"
        Me.ToolTip1.SetToolTip(Me.btnViewReport, "View Report : Alt + V")
        Me.btnViewReport.UseVisualStyleBackColor = False
        '
        'txtRptName
        '
        Me.txtRptName.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtRptName.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.txtRptName.Location = New System.Drawing.Point(665, 443)
        Me.txtRptName.Name = "txtRptName"
        Me.txtRptName.Size = New System.Drawing.Size(168, 25)
        Me.txtRptName.TabIndex = 212
        Me.txtRptName.Visible = False
        '
        'txtDesc
        '
        Me.txtDesc.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtDesc.BackColor = System.Drawing.Color.FromArgb(CType(CType(254, Byte), Integer), CType(CType(254, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.txtDesc.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtDesc.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.txtDesc.ForeColor = System.Drawing.Color.Black
        Me.txtDesc.Location = New System.Drawing.Point(492, 12)
        Me.txtDesc.MaxLength = 250
        Me.txtDesc.Multiline = True
        Me.txtDesc.Name = "txtDesc"
        Me.txtDesc.ReadOnly = True
        Me.txtDesc.Size = New System.Drawing.Size(527, 82)
        Me.txtDesc.TabIndex = 213
        '
        'fraFilters
        '
        Me.fraFilters.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.fraFilters.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.fraFilters.Controls.Add(Me.btnEmail)
        Me.fraFilters.Controls.Add(Me.btnPrintReport)
        Me.fraFilters.Controls.Add(Me.vsfRetGrid)
        Me.fraFilters.Controls.Add(Me.btnClearFilters)
        Me.fraFilters.Controls.Add(Me.btnRemFilter)
        Me.fraFilters.Controls.Add(Me.lblSelFilters)
        Me.fraFilters.Controls.Add(Me.vsfSelFilters)
        Me.fraFilters.Controls.Add(Me.Label1)
        Me.fraFilters.Controls.Add(Me.vsfFilter)
        Me.fraFilters.Controls.Add(Me.btnViewReport)
        Me.fraFilters.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.fraFilters.Location = New System.Drawing.Point(492, 134)
        Me.fraFilters.Name = "fraFilters"
        Me.fraFilters.Size = New System.Drawing.Size(529, 367)
        Me.fraFilters.TabIndex = 214
        '
        'btnPrintReport
        '
        Me.btnPrintReport.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnPrintReport.Enabled = False
        Me.btnPrintReport.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnPrintReport.Location = New System.Drawing.Point(348, 333)
        Me.btnPrintReport.Name = "btnPrintReport"
        Me.btnPrintReport.Size = New System.Drawing.Size(75, 29)
        Me.btnPrintReport.TabIndex = 219
        Me.btnPrintReport.Text = "&Print Report"
        Me.ToolTip1.SetToolTip(Me.btnPrintReport, "Print Report : Alt + P")
        Me.btnPrintReport.UseVisualStyleBackColor = False
        Me.btnPrintReport.Visible = False
        '
        'vsfRetGrid
        '
        Me.vsfRetGrid.AllowEditing = False
        Me.vsfRetGrid.AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.RestrictAll
        Me.vsfRetGrid.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None
        Me.vsfRetGrid.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfRetGrid.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle
        Me.vsfRetGrid.ColumnInfo = "1,0,0,0,0,95,Columns:"
        Me.vsfRetGrid.ExtendLastCol = True
        Me.vsfRetGrid.FocusRect = C1.Win.C1FlexGrid.FocusRectEnum.Solid
        Me.vsfRetGrid.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!)
        Me.vsfRetGrid.Location = New System.Drawing.Point(209, 83)
        Me.vsfRetGrid.Name = "vsfRetGrid"
        Me.vsfRetGrid.Rows.Count = 4
        Me.vsfRetGrid.Rows.DefaultSize = 19
        Me.vsfRetGrid.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfRetGrid.Size = New System.Drawing.Size(245, 45)
        Me.vsfRetGrid.StyleInfo = resources.GetString("vsfRetGrid.StyleInfo")
        Me.vsfRetGrid.TabIndex = 222
        Me.vsfRetGrid.Tag = "DSGR"
        Me.vsfRetGrid.Visible = False
        '
        'btnClearFilters
        '
        Me.btnClearFilters.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClearFilters.Enabled = False
        Me.btnClearFilters.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnClearFilters.Location = New System.Drawing.Point(241, 333)
        Me.btnClearFilters.Name = "btnClearFilters"
        Me.btnClearFilters.Size = New System.Drawing.Size(104, 29)
        Me.btnClearFilters.TabIndex = 219
        Me.btnClearFilters.Text = "C&lear All Filters"
        Me.ToolTip1.SetToolTip(Me.btnClearFilters, "Clear Filters : Alt + L")
        Me.btnClearFilters.UseVisualStyleBackColor = False
        '
        'btnRemFilter
        '
        Me.btnRemFilter.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnRemFilter.Enabled = False
        Me.btnRemFilter.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnRemFilter.Location = New System.Drawing.Point(133, 333)
        Me.btnRemFilter.Name = "btnRemFilter"
        Me.btnRemFilter.Size = New System.Drawing.Size(104, 29)
        Me.btnRemFilter.TabIndex = 218
        Me.btnRemFilter.Text = "&Remove Filter"
        Me.ToolTip1.SetToolTip(Me.btnRemFilter, "Remove Filter : Alt + R")
        Me.btnRemFilter.UseVisualStyleBackColor = False
        '
        'lblSelFilters
        '
        Me.lblSelFilters.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblSelFilters.AutoSize = True
        Me.lblSelFilters.Location = New System.Drawing.Point(93, 195)
        Me.lblSelFilters.Name = "lblSelFilters"
        Me.lblSelFilters.Size = New System.Drawing.Size(95, 17)
        Me.lblSelFilters.TabIndex = 217
        Me.lblSelFilters.Text = "Selected Filters"
        '
        'vsfSelFilters
        '
        Me.vsfSelFilters.AllowDragging = C1.Win.C1FlexGrid.AllowDraggingEnum.None
        Me.vsfSelFilters.AllowEditing = False
        Me.vsfSelFilters.AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.RestrictAll
        Me.vsfSelFilters.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None
        Me.vsfSelFilters.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfSelFilters.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle
        Me.vsfSelFilters.ColumnInfo = "1,0,0,0,0,95,Columns:"
        Me.vsfSelFilters.ExtendLastCol = True
        Me.vsfSelFilters.FocusRect = C1.Win.C1FlexGrid.FocusRectEnum.Solid
        Me.vsfSelFilters.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!)
        Me.vsfSelFilters.Location = New System.Drawing.Point(16, 220)
        Me.vsfSelFilters.Name = "vsfSelFilters"
        Me.vsfSelFilters.Rows.Count = 4
        Me.vsfSelFilters.Rows.DefaultSize = 19
        Me.vsfSelFilters.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfSelFilters.Size = New System.Drawing.Size(499, 110)
        Me.vsfSelFilters.StyleInfo = resources.GetString("vsfSelFilters.StyleInfo")
        Me.vsfSelFilters.TabIndex = 216
        Me.vsfSelFilters.Tag = "DSGR"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(13, 2)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(51, 17)
        Me.Label1.TabIndex = 215
        Me.Label1.Text = "Filters..."
        '
        'vsfFilter
        '
        Me.vsfFilter.AllowDragging = C1.Win.C1FlexGrid.AllowDraggingEnum.None
        Me.vsfFilter.AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.RestrictAll
        Me.vsfFilter.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None
        Me.vsfFilter.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfFilter.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle
        Me.vsfFilter.ColumnInfo = "1,0,0,0,0,95,Columns:"
        Me.vsfFilter.ExtendLastCol = True
        Me.vsfFilter.FocusRect = C1.Win.C1FlexGrid.FocusRectEnum.Solid
        Me.vsfFilter.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!)
        Me.vsfFilter.Location = New System.Drawing.Point(16, 22)
        Me.vsfFilter.Name = "vsfFilter"
        Me.vsfFilter.Rows.Count = 4
        Me.vsfFilter.Rows.DefaultSize = 19
        Me.vsfFilter.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfFilter.ShowButtons = C1.Win.C1FlexGrid.ShowButtonsEnum.Always
        Me.vsfFilter.Size = New System.Drawing.Size(498, 165)
        Me.vsfFilter.StyleInfo = resources.GetString("vsfFilter.StyleInfo")
        Me.vsfFilter.TabIndex = 214
        Me.vsfFilter.Tag = "DSGR"
        '
        'lbl
        '
        Me.lbl.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lbl.AutoSize = True
        Me.lbl.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.lbl.Location = New System.Drawing.Point(494, 107)
        Me.lbl.Name = "lbl"
        Me.lbl.Size = New System.Drawing.Size(140, 17)
        Me.lbl.TabIndex = 215
        Me.lbl.Text = "Report to be Viewed : "
        '
        'lblReportName
        '
        Me.lblReportName.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblReportName.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.lblReportName.Location = New System.Drawing.Point(630, 97)
        Me.lblReportName.Name = "lblReportName"
        Me.lblReportName.Size = New System.Drawing.Size(220, 34)
        Me.lblReportName.TabIndex = 216
        Me.lblReportName.Text = "Report to be Viewed : "
        Me.lblReportName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblSearch
        '
        Me.lblSearch.AutoSize = True
        Me.lblSearch.BackColor = System.Drawing.Color.Transparent
        Me.lblSearch.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSearch.ForeColor = System.Drawing.Color.Black
        Me.lblSearch.Location = New System.Drawing.Point(10, 9)
        Me.lblSearch.Name = "lblSearch"
        Me.lblSearch.Size = New System.Drawing.Size(177, 20)
        Me.lblSearch.TabIndex = 234
        Me.lblSearch.Text = "Search Report Name"
        '
        'txtSearch
        '
        Me.txtSearch.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtSearch.BackColor = System.Drawing.Color.White
        Me.txtSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtSearch.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtSearch.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSearch.Location = New System.Drawing.Point(201, 9)
        Me.txtSearch.Margin = New System.Windows.Forms.Padding(4)
        Me.txtSearch.MaxLength = 250
        Me.txtSearch.Name = "txtSearch"
        Me.txtSearch.Size = New System.Drawing.Size(284, 21)
        Me.txtSearch.TabIndex = 233
        '
        'vsfSearch
        '
        Me.vsfSearch.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfSearch.BackColor = System.Drawing.Color.White
        Me.vsfSearch.ColumnInfo = "1,0,0,0,0,110,Columns:"
        Me.vsfSearch.ExtendLastCol = True
        Me.vsfSearch.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.vsfSearch.KeyActionEnter = C1.Win.C1FlexGrid.KeyActionEnum.None
        Me.vsfSearch.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveDown
        Me.vsfSearch.Location = New System.Drawing.Point(12, 36)
        Me.vsfSearch.Name = "vsfSearch"
        Me.vsfSearch.Rows.Count = 1
        Me.vsfSearch.Rows.DefaultSize = 22
        Me.vsfSearch.Rows.Fixed = 0
        Me.vsfSearch.Size = New System.Drawing.Size(473, 393)
        Me.vsfSearch.StyleInfo = resources.GetString("vsfSearch.StyleInfo")
        Me.vsfSearch.TabIndex = 235
        Me.vsfSearch.Visible = False
        '
        'lblDescription
        '
        Me.lblDescription.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblDescription.AutoSize = True
        Me.lblDescription.BackColor = System.Drawing.Color.Gainsboro
        Me.lblDescription.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDescription.ForeColor = System.Drawing.Color.Black
        Me.lblDescription.Location = New System.Drawing.Point(578, 19)
        Me.lblDescription.Name = "lblDescription"
        Me.lblDescription.Size = New System.Drawing.Size(49, 16)
        Me.lblDescription.TabIndex = 236
        Me.lblDescription.Text = "Label2"
        Me.lblDescription.Visible = False
        '
        'btnImport
        '
        Me.btnImport.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnImport.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnImport.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.btnImport.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnImport.Location = New System.Drawing.Point(217, 472)
        Me.btnImport.Name = "btnImport"
        Me.btnImport.Size = New System.Drawing.Size(75, 29)
        Me.btnImport.TabIndex = 237
        Me.btnImport.Text = "&Import"
        Me.ToolTip1.SetToolTip(Me.btnImport, "Design : Alt + D")
        Me.btnImport.UseVisualStyleBackColor = False
        '
        'btnRights
        '
        Me.btnRights.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnRights.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnRights.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.btnRights.Image = CType(resources.GetObject("btnRights.Image"), System.Drawing.Image)
        Me.btnRights.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnRights.Location = New System.Drawing.Point(93, 472)
        Me.btnRights.Name = "btnRights"
        Me.btnRights.Size = New System.Drawing.Size(118, 29)
        Me.btnRights.TabIndex = 217
        Me.btnRights.Text = "&Assign Rights"
        Me.btnRights.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnRights, "Assign Rights : Alt + A")
        Me.btnRights.UseVisualStyleBackColor = False
        '
        'btnDesign
        '
        Me.btnDesign.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnDesign.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnDesign.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.btnDesign.Image = CType(resources.GetObject("btnDesign.Image"), System.Drawing.Image)
        Me.btnDesign.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnDesign.Location = New System.Drawing.Point(12, 472)
        Me.btnDesign.Name = "btnDesign"
        Me.btnDesign.Size = New System.Drawing.Size(75, 29)
        Me.btnDesign.TabIndex = 209
        Me.btnDesign.Text = "&Design"
        Me.btnDesign.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnDesign, "Design : Alt + D")
        Me.btnDesign.UseVisualStyleBackColor = False
        '
        'btnClose
        '
        Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnClose.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.btnClose.Image = CType(resources.GetObject("btnClose.Image"), System.Drawing.Image)
        Me.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnClose.Location = New System.Drawing.Point(383, 472)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(80, 29)
        Me.btnClose.TabIndex = 210
        Me.btnClose.Text = "&Close"
        Me.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnClose, "Close : Alt + C")
        Me.btnClose.UseVisualStyleBackColor = False
        '
        'vsfExp
        '
        Me.vsfExp.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfExp.ColumnInfo = "10,1,0,0,0,120,Columns:"
        Me.vsfExp.ExtendLastCol = True
        Me.vsfExp.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.vsfExp.Location = New System.Drawing.Point(213, 267)
        Me.vsfExp.Margin = New System.Windows.Forms.Padding(2)
        Me.vsfExp.Name = "vsfExp"
        Me.vsfExp.Rows.DefaultSize = 24
        Me.vsfExp.Size = New System.Drawing.Size(154, 103)
        Me.vsfExp.StyleInfo = resources.GetString("vsfExp.StyleInfo")
        Me.vsfExp.TabIndex = 238
        Me.vsfExp.Visible = False
        '
        'btnEmail
        '
        Me.btnEmail.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnEmail.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnEmail.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.btnEmail.Location = New System.Drawing.Point(40, 333)
        Me.btnEmail.Name = "btnEmail"
        Me.btnEmail.Size = New System.Drawing.Size(89, 29)
        Me.btnEmail.TabIndex = 223
        Me.btnEmail.Text = "Email"
        Me.btnEmail.UseVisualStyleBackColor = False
        '
        'frmRptCustomiseRptView
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1033, 513)
        Me.Controls.Add(Me.vsfExp)
        Me.Controls.Add(Me.btnImport)
        Me.Controls.Add(Me.lblDescription)
        Me.Controls.Add(Me.vsfSearch)
        Me.Controls.Add(Me.lblSearch)
        Me.Controls.Add(Me.txtSearch)
        Me.Controls.Add(Me.btnRights)
        Me.Controls.Add(Me.lblReportName)
        Me.Controls.Add(Me.lbl)
        Me.Controls.Add(Me.fraFilters)
        Me.Controls.Add(Me.txtRptName)
        Me.Controls.Add(Me.btnDesign)
        Me.Controls.Add(Me.txtDesc)
        Me.Controls.Add(Me.vsfDet)
        Me.Controls.Add(Me.btnClose)
        Me.MinimumSize = New System.Drawing.Size(974, 551)
        Me.Name = "frmRptCustomiseRptView"
        Me.ShowIcon = False
        Me.Text = "Customised Reports..."
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).EndInit()
        Me.fraFilters.ResumeLayout(False)
        Me.fraFilters.PerformLayout()
        CType(Me.vsfRetGrid, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.vsfSelFilters, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.vsfFilter, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.vsfSearch, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.vsfExp, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents vsfDet As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents btnDesign As System.Windows.Forms.Button
    Friend WithEvents btnClose As System.Windows.Forms.Button
    Friend WithEvents btnViewReport As System.Windows.Forms.Button
    Friend WithEvents txtRptName As System.Windows.Forms.TextBox
    Friend WithEvents txtDesc As System.Windows.Forms.TextBox
    Friend WithEvents fraFilters As System.Windows.Forms.Panel
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents vsfFilter As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents vsfSelFilters As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents lblSelFilters As System.Windows.Forms.Label
    Friend WithEvents btnClearFilters As System.Windows.Forms.Button
    Friend WithEvents btnRemFilter As System.Windows.Forms.Button
    Public WithEvents vsfRetGrid As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents lbl As System.Windows.Forms.Label
    Friend WithEvents lblReportName As System.Windows.Forms.Label
    Friend WithEvents btnRights As System.Windows.Forms.Button
    Friend WithEvents btnPrintReport As Button
    Friend WithEvents lblSearch As Label
    Friend WithEvents txtSearch As TextBox
    Friend WithEvents vsfSearch As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents lblDescription As Label
    Friend WithEvents ToolTip1 As ToolTip
    Friend WithEvents btnImport As Button
    Private WithEvents vsfExp As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents btnEmail As Button
End Class
