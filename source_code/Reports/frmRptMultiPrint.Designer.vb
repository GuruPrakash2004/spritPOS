<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmRptMultiPrint
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmRptMultiPrint))
        Me.pnlDateRange = New System.Windows.Forms.Panel()
        Me.dtpToDate = New System.Windows.Forms.DateTimePicker()
        Me.dtpFromDate = New System.Windows.Forms.DateTimePicker()
        Me.lblDateTo = New System.Windows.Forms.Label()
        Me.lblDateFrom = New System.Windows.Forms.Label()
        Me.chkDateRange = New System.Windows.Forms.CheckBox()
        Me.pnlDocNumber = New System.Windows.Forms.Panel()
        Me.txtTo = New System.Windows.Forms.TextBox()
        Me.txtFrom = New System.Windows.Forms.TextBox()
        Me.lblDocNumberTo = New System.Windows.Forms.Label()
        Me.lblDocNumberFrom = New System.Windows.Forms.Label()
        Me.chkDocNumberRange = New System.Windows.Forms.CheckBox()
        Me.pnlPrint = New System.Windows.Forms.Panel()
        Me.optNo = New System.Windows.Forms.RadioButton()
        Me.optYes = New System.Windows.Forms.RadioButton()
        Me.lblPrinted = New System.Windows.Forms.Label()
        Me.btnLoad = New System.Windows.Forms.Button()
        Me.chkSelectAll = New System.Windows.Forms.CheckBox()
        Me.btnPrint = New System.Windows.Forms.Button()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.vsfSearch = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.vsfDet = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.pnlCopyShortCut = New System.Windows.Forms.Panel()
        Me.btnCopy = New System.Windows.Forms.Button()
        Me.pnlbtnClose = New System.Windows.Forms.Button()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.RbnCopyGrid = New System.Windows.Forms.RadioButton()
        Me.RbnCopyRow = New System.Windows.Forms.RadioButton()
        Me.RbnCopyColumn = New System.Windows.Forms.RadioButton()
        Me.RbnCopyCell = New System.Windows.Forms.RadioButton()
        Me.pnlDateRange.SuspendLayout()
        Me.pnlDocNumber.SuspendLayout()
        Me.pnlPrint.SuspendLayout()
        CType(Me.vsfSearch, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlCopyShortCut.SuspendLayout()
        Me.SuspendLayout()
        '
        'pnlDateRange
        '
        Me.pnlDateRange.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlDateRange.Controls.Add(Me.dtpToDate)
        Me.pnlDateRange.Controls.Add(Me.dtpFromDate)
        Me.pnlDateRange.Controls.Add(Me.lblDateTo)
        Me.pnlDateRange.Controls.Add(Me.lblDateFrom)
        Me.pnlDateRange.Controls.Add(Me.chkDateRange)
        Me.pnlDateRange.Location = New System.Drawing.Point(12, 8)
        Me.pnlDateRange.Name = "pnlDateRange"
        Me.pnlDateRange.Size = New System.Drawing.Size(418, 89)
        Me.pnlDateRange.TabIndex = 0
        '
        'dtpToDate
        '
        Me.dtpToDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dtpToDate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpToDate.Location = New System.Drawing.Point(254, 51)
        Me.dtpToDate.Name = "dtpToDate"
        Me.dtpToDate.Size = New System.Drawing.Size(145, 26)
        Me.dtpToDate.TabIndex = 4
        '
        'dtpFromDate
        '
        Me.dtpFromDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dtpFromDate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpFromDate.Location = New System.Drawing.Point(55, 51)
        Me.dtpFromDate.Name = "dtpFromDate"
        Me.dtpFromDate.Size = New System.Drawing.Size(142, 26)
        Me.dtpFromDate.TabIndex = 3
        '
        'lblDateTo
        '
        Me.lblDateTo.AutoSize = True
        Me.lblDateTo.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDateTo.Location = New System.Drawing.Point(211, 51)
        Me.lblDateTo.Name = "lblDateTo"
        Me.lblDateTo.Size = New System.Drawing.Size(27, 20)
        Me.lblDateTo.TabIndex = 2
        Me.lblDateTo.Text = "To"
        '
        'lblDateFrom
        '
        Me.lblDateFrom.AutoSize = True
        Me.lblDateFrom.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDateFrom.Location = New System.Drawing.Point(3, 51)
        Me.lblDateFrom.Name = "lblDateFrom"
        Me.lblDateFrom.Size = New System.Drawing.Size(46, 20)
        Me.lblDateFrom.TabIndex = 1
        Me.lblDateFrom.Text = "From"
        '
        'chkDateRange
        '
        Me.chkDateRange.AutoSize = True
        Me.chkDateRange.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkDateRange.Location = New System.Drawing.Point(7, 12)
        Me.chkDateRange.Name = "chkDateRange"
        Me.chkDateRange.Size = New System.Drawing.Size(115, 24)
        Me.chkDateRange.TabIndex = 0
        Me.chkDateRange.Text = "Date Range"
        Me.chkDateRange.UseVisualStyleBackColor = True
        '
        'pnlDocNumber
        '
        Me.pnlDocNumber.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlDocNumber.Controls.Add(Me.txtTo)
        Me.pnlDocNumber.Controls.Add(Me.txtFrom)
        Me.pnlDocNumber.Controls.Add(Me.lblDocNumberTo)
        Me.pnlDocNumber.Controls.Add(Me.lblDocNumberFrom)
        Me.pnlDocNumber.Controls.Add(Me.chkDocNumberRange)
        Me.pnlDocNumber.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pnlDocNumber.Location = New System.Drawing.Point(452, 8)
        Me.pnlDocNumber.Name = "pnlDocNumber"
        Me.pnlDocNumber.Size = New System.Drawing.Size(378, 89)
        Me.pnlDocNumber.TabIndex = 1
        '
        'txtTo
        '
        Me.txtTo.Location = New System.Drawing.Point(237, 54)
        Me.txtTo.Name = "txtTo"
        Me.txtTo.Size = New System.Drawing.Size(116, 26)
        Me.txtTo.TabIndex = 4
        '
        'txtFrom
        '
        Me.txtFrom.Location = New System.Drawing.Point(60, 51)
        Me.txtFrom.Name = "txtFrom"
        Me.txtFrom.Size = New System.Drawing.Size(116, 26)
        Me.txtFrom.TabIndex = 3
        '
        'lblDocNumberTo
        '
        Me.lblDocNumberTo.AutoSize = True
        Me.lblDocNumberTo.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDocNumberTo.Location = New System.Drawing.Point(191, 54)
        Me.lblDocNumberTo.Name = "lblDocNumberTo"
        Me.lblDocNumberTo.Size = New System.Drawing.Size(27, 20)
        Me.lblDocNumberTo.TabIndex = 2
        Me.lblDocNumberTo.Text = "To"
        '
        'lblDocNumberFrom
        '
        Me.lblDocNumberFrom.AutoSize = True
        Me.lblDocNumberFrom.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDocNumberFrom.Location = New System.Drawing.Point(8, 56)
        Me.lblDocNumberFrom.Name = "lblDocNumberFrom"
        Me.lblDocNumberFrom.Size = New System.Drawing.Size(46, 20)
        Me.lblDocNumberFrom.TabIndex = 1
        Me.lblDocNumberFrom.Text = "From"
        '
        'chkDocNumberRange
        '
        Me.chkDocNumberRange.AutoSize = True
        Me.chkDocNumberRange.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkDocNumberRange.Location = New System.Drawing.Point(12, 12)
        Me.chkDocNumberRange.Name = "chkDocNumberRange"
        Me.chkDocNumberRange.Size = New System.Drawing.Size(169, 24)
        Me.chkDocNumberRange.TabIndex = 0
        Me.chkDocNumberRange.Text = "Doc Number Range"
        Me.chkDocNumberRange.UseVisualStyleBackColor = True
        '
        'pnlPrint
        '
        Me.pnlPrint.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlPrint.Controls.Add(Me.optNo)
        Me.pnlPrint.Controls.Add(Me.optYes)
        Me.pnlPrint.Controls.Add(Me.lblPrinted)
        Me.pnlPrint.Location = New System.Drawing.Point(12, 117)
        Me.pnlPrint.Name = "pnlPrint"
        Me.pnlPrint.Size = New System.Drawing.Size(197, 46)
        Me.pnlPrint.TabIndex = 2
        '
        'optNo
        '
        Me.optNo.AutoSize = True
        Me.optNo.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optNo.Location = New System.Drawing.Point(141, 11)
        Me.optNo.Name = "optNo"
        Me.optNo.Size = New System.Drawing.Size(47, 24)
        Me.optNo.TabIndex = 2
        Me.optNo.TabStop = True
        Me.optNo.Text = "No"
        Me.optNo.UseVisualStyleBackColor = True
        '
        'optYes
        '
        Me.optYes.AutoSize = True
        Me.optYes.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optYes.Location = New System.Drawing.Point(80, 9)
        Me.optYes.Name = "optYes"
        Me.optYes.Size = New System.Drawing.Size(55, 24)
        Me.optYes.TabIndex = 1
        Me.optYes.TabStop = True
        Me.optYes.Text = "Yes"
        Me.optYes.UseVisualStyleBackColor = True
        '
        'lblPrinted
        '
        Me.lblPrinted.AutoSize = True
        Me.lblPrinted.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPrinted.Location = New System.Drawing.Point(8, 11)
        Me.lblPrinted.Name = "lblPrinted"
        Me.lblPrinted.Size = New System.Drawing.Size(41, 20)
        Me.lblPrinted.TabIndex = 0
        Me.lblPrinted.Text = "Print"
        '
        'btnLoad
        '
        Me.btnLoad.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnLoad.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnLoad.Location = New System.Drawing.Point(889, 149)
        Me.btnLoad.Name = "btnLoad"
        Me.btnLoad.Size = New System.Drawing.Size(75, 33)
        Me.btnLoad.TabIndex = 3
        Me.btnLoad.Text = "&Load"
        Me.ToolTip1.SetToolTip(Me.btnLoad, "Shortcut:Alt+L")
        Me.btnLoad.UseVisualStyleBackColor = True
        '
        'chkSelectAll
        '
        Me.chkSelectAll.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.chkSelectAll.AutoSize = True
        Me.chkSelectAll.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkSelectAll.Location = New System.Drawing.Point(11, 557)
        Me.chkSelectAll.Name = "chkSelectAll"
        Me.chkSelectAll.Size = New System.Drawing.Size(94, 24)
        Me.chkSelectAll.TabIndex = 5
        Me.chkSelectAll.Text = "Select All"
        Me.chkSelectAll.UseVisualStyleBackColor = True
        '
        'btnPrint
        '
        Me.btnPrint.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnPrint.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnPrint.Location = New System.Drawing.Point(785, 553)
        Me.btnPrint.Name = "btnPrint"
        Me.btnPrint.Size = New System.Drawing.Size(87, 39)
        Me.btnPrint.TabIndex = 6
        Me.btnPrint.Text = "&Print"
        Me.ToolTip1.SetToolTip(Me.btnPrint, "Shortcut:Alt+P")
        Me.btnPrint.UseVisualStyleBackColor = True
        '
        'btnClose
        '
        Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClose.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnClose.Location = New System.Drawing.Point(878, 553)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(86, 39)
        Me.btnClose.TabIndex = 7
        Me.btnClose.Text = "Cl&ose"
        Me.ToolTip1.SetToolTip(Me.btnClose, "Shortcut:Alt+O")
        Me.btnClose.UseVisualStyleBackColor = True
        '
        'vsfSearch
        '
        Me.vsfSearch.AllowDragging = C1.Win.C1FlexGrid.AllowDraggingEnum.None
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
        Me.vsfSearch.KeyActionEnter = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross
        Me.vsfSearch.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross
        Me.vsfSearch.Location = New System.Drawing.Point(11, 188)
        Me.vsfSearch.Name = "vsfSearch"
        Me.vsfSearch.Rows.Count = 1
        Me.vsfSearch.Rows.DefaultSize = 24
        Me.vsfSearch.Rows.Fixed = 0
        Me.vsfSearch.ScrollBars = System.Windows.Forms.ScrollBars.None
        Me.vsfSearch.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfSearch.ShowCellLabels = True
        Me.vsfSearch.Size = New System.Drawing.Size(953, 21)
        Me.vsfSearch.StyleInfo = resources.GetString("vsfSearch.StyleInfo")
        Me.vsfSearch.TabIndex = 22
        Me.vsfSearch.Tag = ""
        Me.vsfSearch.VisualStyle = C1.Win.C1FlexGrid.VisualStyle.System
        '
        'vsfDet
        '
        Me.vsfDet.AllowDragging = C1.Win.C1FlexGrid.AllowDraggingEnum.None
        Me.vsfDet.AllowFiltering = True
        Me.vsfDet.AllowFreezing = C1.Win.C1FlexGrid.AllowFreezingEnum.Columns
        Me.vsfDet.AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.Free
        Me.vsfDet.AllowMergingFixed = C1.Win.C1FlexGrid.AllowMergingEnum.Free
        Me.vsfDet.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfDet.AutoClipboard = True
        Me.vsfDet.AutoResize = True
        Me.vsfDet.AutoSearch = C1.Win.C1FlexGrid.AutoSearchEnum.FromTop
        Me.vsfDet.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle
        Me.vsfDet.ColumnInfo = "2,0,0,0,0,120,Columns:"
        Me.vsfDet.EditOptions = C1.Win.C1FlexGrid.EditFlags.AutoSearch
        Me.vsfDet.ExtendLastCol = True
        Me.vsfDet.FocusRect = C1.Win.C1FlexGrid.FocusRectEnum.Heavy
        Me.vsfDet.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.vsfDet.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross
        Me.vsfDet.Location = New System.Drawing.Point(11, 209)
        Me.vsfDet.Name = "vsfDet"
        Me.vsfDet.Rows.DefaultSize = 24
        Me.vsfDet.Rows.GlyphRow = 0
        Me.vsfDet.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfDet.ShowCellLabels = True
        Me.vsfDet.Size = New System.Drawing.Size(953, 338)
        Me.vsfDet.StyleInfo = resources.GetString("vsfDet.StyleInfo")
        Me.vsfDet.TabIndex = 21
        Me.vsfDet.Tag = "View"
        Me.vsfDet.VisualStyle = C1.Win.C1FlexGrid.VisualStyle.Office2007Blue
        '
        'pnlCopyShortCut
        '
        Me.pnlCopyShortCut.Controls.Add(Me.btnCopy)
        Me.pnlCopyShortCut.Controls.Add(Me.pnlbtnClose)
        Me.pnlCopyShortCut.Controls.Add(Me.Label5)
        Me.pnlCopyShortCut.Controls.Add(Me.Label4)
        Me.pnlCopyShortCut.Controls.Add(Me.Label6)
        Me.pnlCopyShortCut.Controls.Add(Me.Label7)
        Me.pnlCopyShortCut.Controls.Add(Me.RbnCopyGrid)
        Me.pnlCopyShortCut.Controls.Add(Me.RbnCopyRow)
        Me.pnlCopyShortCut.Controls.Add(Me.RbnCopyColumn)
        Me.pnlCopyShortCut.Controls.Add(Me.RbnCopyCell)
        Me.pnlCopyShortCut.Location = New System.Drawing.Point(267, 262)
        Me.pnlCopyShortCut.Name = "pnlCopyShortCut"
        Me.pnlCopyShortCut.Size = New System.Drawing.Size(329, 177)
        Me.pnlCopyShortCut.TabIndex = 307
        Me.pnlCopyShortCut.Visible = False
        '
        'btnCopy
        '
        Me.btnCopy.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCopy.BackColor = System.Drawing.Color.FromArgb(CType(CType(233, Byte), Integer), CType(CType(237, Byte), Integer), CType(CType(241, Byte), Integer))
        Me.btnCopy.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_ok
        Me.btnCopy.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.btnCopy.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnCopy.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!)
        Me.btnCopy.Location = New System.Drawing.Point(161, 135)
        Me.btnCopy.Name = "btnCopy"
        Me.btnCopy.Size = New System.Drawing.Size(70, 29)
        Me.btnCopy.TabIndex = 23
        Me.btnCopy.Text = "Copy"
        Me.btnCopy.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnCopy.UseVisualStyleBackColor = False
        '
        'pnlbtnClose
        '
        Me.pnlbtnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnlbtnClose.BackColor = System.Drawing.Color.FromArgb(CType(CType(233, Byte), Integer), CType(CType(237, Byte), Integer), CType(CType(241, Byte), Integer))
        Me.pnlbtnClose.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_CLOSE
        Me.pnlbtnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.pnlbtnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.pnlbtnClose.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!)
        Me.pnlbtnClose.Location = New System.Drawing.Point(237, 135)
        Me.pnlbtnClose.Name = "pnlbtnClose"
        Me.pnlbtnClose.Size = New System.Drawing.Size(70, 29)
        Me.pnlbtnClose.TabIndex = 22
        Me.pnlbtnClose.Text = "Cl&ose"
        Me.pnlbtnClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.pnlbtnClose.UseVisualStyleBackColor = False
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(178, 109)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(88, 16)
        Me.Label5.TabIndex = 7
        Me.Label5.Text = "Ctrl + Shift + G"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(178, 79)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(77, 16)
        Me.Label4.TabIndex = 6
        Me.Label4.Text = "Ctrl + Alt + C"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(178, 48)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(87, 16)
        Me.Label6.TabIndex = 5
        Me.Label6.Text = "Ctrl + Shift + C"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(178, 19)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(49, 16)
        Me.Label7.TabIndex = 4
        Me.Label7.Text = "Ctrl + C"
        '
        'RbnCopyGrid
        '
        Me.RbnCopyGrid.AutoSize = True
        Me.RbnCopyGrid.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RbnCopyGrid.Location = New System.Drawing.Point(16, 109)
        Me.RbnCopyGrid.Name = "RbnCopyGrid"
        Me.RbnCopyGrid.Size = New System.Drawing.Size(129, 20)
        Me.RbnCopyGrid.TabIndex = 3
        Me.RbnCopyGrid.TabStop = True
        Me.RbnCopyGrid.Text = "Copy Entire Grid  "
        Me.RbnCopyGrid.UseVisualStyleBackColor = True
        '
        'RbnCopyRow
        '
        Me.RbnCopyRow.AutoSize = True
        Me.RbnCopyRow.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RbnCopyRow.Location = New System.Drawing.Point(16, 77)
        Me.RbnCopyRow.Name = "RbnCopyRow"
        Me.RbnCopyRow.Size = New System.Drawing.Size(128, 20)
        Me.RbnCopyRow.TabIndex = 2
        Me.RbnCopyRow.TabStop = True
        Me.RbnCopyRow.Text = "Copy Entire  Row"
        Me.RbnCopyRow.UseVisualStyleBackColor = True
        '
        'RbnCopyColumn
        '
        Me.RbnCopyColumn.AutoSize = True
        Me.RbnCopyColumn.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RbnCopyColumn.Location = New System.Drawing.Point(15, 46)
        Me.RbnCopyColumn.Name = "RbnCopyColumn"
        Me.RbnCopyColumn.Size = New System.Drawing.Size(143, 20)
        Me.RbnCopyColumn.TabIndex = 1
        Me.RbnCopyColumn.TabStop = True
        Me.RbnCopyColumn.Text = "Copy Entire Column"
        Me.RbnCopyColumn.UseVisualStyleBackColor = True
        '
        'RbnCopyCell
        '
        Me.RbnCopyCell.AutoSize = True
        Me.RbnCopyCell.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RbnCopyCell.Location = New System.Drawing.Point(15, 19)
        Me.RbnCopyCell.Name = "RbnCopyCell"
        Me.RbnCopyCell.Size = New System.Drawing.Size(129, 20)
        Me.RbnCopyCell.TabIndex = 0
        Me.RbnCopyCell.TabStop = True
        Me.RbnCopyCell.Text = "Copy Data In Cell"
        Me.RbnCopyCell.UseVisualStyleBackColor = True
        '
        'frmRptMultiPrint
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(976, 597)
        Me.Controls.Add(Me.pnlCopyShortCut)
        Me.Controls.Add(Me.vsfSearch)
        Me.Controls.Add(Me.vsfDet)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.btnPrint)
        Me.Controls.Add(Me.chkSelectAll)
        Me.Controls.Add(Me.btnLoad)
        Me.Controls.Add(Me.pnlPrint)
        Me.Controls.Add(Me.pnlDocNumber)
        Me.Controls.Add(Me.pnlDateRange)
        Me.Name = "frmRptMultiPrint"
        Me.ShowIcon = False
        Me.Text = "Report Multi Print"
        Me.pnlDateRange.ResumeLayout(False)
        Me.pnlDateRange.PerformLayout()
        Me.pnlDocNumber.ResumeLayout(False)
        Me.pnlDocNumber.PerformLayout()
        Me.pnlPrint.ResumeLayout(False)
        Me.pnlPrint.PerformLayout()
        CType(Me.vsfSearch, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlCopyShortCut.ResumeLayout(False)
        Me.pnlCopyShortCut.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents pnlDateRange As Panel
    Friend WithEvents lblDateTo As Label
    Friend WithEvents lblDateFrom As Label
    Friend WithEvents chkDateRange As CheckBox
    Friend WithEvents pnlDocNumber As Panel
    Friend WithEvents chkDocNumberRange As CheckBox
    Friend WithEvents pnlPrint As Panel
    Friend WithEvents dtpToDate As DateTimePicker
    Friend WithEvents dtpFromDate As DateTimePicker
    Friend WithEvents lblDocNumberTo As Label
    Friend WithEvents lblDocNumberFrom As Label
    Friend WithEvents optNo As RadioButton
    Friend WithEvents optYes As RadioButton
    Friend WithEvents lblPrinted As Label
    Friend WithEvents btnLoad As Button
    Friend WithEvents chkSelectAll As CheckBox
    Friend WithEvents txtTo As TextBox
    Friend WithEvents txtFrom As TextBox
    Friend WithEvents btnPrint As Button
    Friend WithEvents btnClose As Button
    Friend WithEvents vsfSearch As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents vsfDet As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents ToolTip1 As ToolTip
    Friend WithEvents pnlCopyShortCut As Panel
    Friend WithEvents btnCopy As Button
    Friend WithEvents pnlbtnClose As Button
    Friend WithEvents Label5 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents Label6 As Label
    Friend WithEvents Label7 As Label
    Friend WithEvents RbnCopyGrid As RadioButton
    Friend WithEvents RbnCopyRow As RadioButton
    Friend WithEvents RbnCopyColumn As RadioButton
    Friend WithEvents RbnCopyCell As RadioButton
End Class
