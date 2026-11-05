<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmRptStockAgeing
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmRptStockAgeing))
        Me.lblAsOn = New System.Windows.Forms.Label()
        Me.dtpDate = New System.Windows.Forms.DateTimePicker()
        Me.vsfRange = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.vsfDet = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.lblSelectloaction = New System.Windows.Forms.Label()
        Me.cboSelectLocation = New System.Windows.Forms.ComboBox()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.btnExportToXL = New System.Windows.Forms.Button()
        Me.btnItem = New System.Windows.Forms.Button()
        Me.btnPkgItem = New System.Windows.Forms.Button()
        Me.btnView = New System.Windows.Forms.Button()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.btnEmailPDF = New System.Windows.Forms.Button()
        Me.btnPrint = New System.Windows.Forms.Button()
        Me.vsfPkgItem = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.txtPkgItem = New System.Windows.Forms.TextBox()
        Me.vsfItem = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.txtItem = New System.Windows.Forms.TextBox()
        Me.vsfFldVal = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.lblSItem = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtDummy2 = New System.Windows.Forms.TextBox()
        Me.txtDummy = New System.Windows.Forms.TextBox()
        Me.cboValuation = New System.Windows.Forms.ComboBox()
        Me.lblValuation = New System.Windows.Forms.Label()
        Me.chkItemGroup = New System.Windows.Forms.CheckBox()
        Me.lblSelectItem = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.chkShowPB = New System.Windows.Forms.CheckBox()
        Me.pnlCopyShortCut = New System.Windows.Forms.Panel()
        Me.btnCopy = New System.Windows.Forms.Button()
        Me.pnlbtnClose = New System.Windows.Forms.Button()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.RbnCopyGrid = New System.Windows.Forms.RadioButton()
        Me.RbnCopyRow = New System.Windows.Forms.RadioButton()
        Me.RbnCopyColumn = New System.Windows.Forms.RadioButton()
        Me.RbnCopyCell = New System.Windows.Forms.RadioButton()
        CType(Me.vsfRange, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.vsfPkgItem, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.vsfItem, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.vsfFldVal, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlCopyShortCut.SuspendLayout()
        Me.SuspendLayout()
        '
        'lblAsOn
        '
        Me.lblAsOn.AutoSize = True
        Me.lblAsOn.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAsOn.Location = New System.Drawing.Point(3, 10)
        Me.lblAsOn.Name = "lblAsOn"
        Me.lblAsOn.Size = New System.Drawing.Size(53, 20)
        Me.lblAsOn.TabIndex = 0
        Me.lblAsOn.Text = "As On"
        '
        'dtpDate
        '
        Me.dtpDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dtpDate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpDate.Location = New System.Drawing.Point(94, 7)
        Me.dtpDate.Name = "dtpDate"
        Me.dtpDate.Size = New System.Drawing.Size(130, 26)
        Me.dtpDate.TabIndex = 0
        '
        'vsfRange
        '
        Me.vsfRange.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None
        Me.vsfRange.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle
        Me.vsfRange.ColumnInfo = "2,0,0,0,0,110,Columns:"
        Me.vsfRange.ExtendLastCol = True
        Me.vsfRange.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.vsfRange.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross
        Me.vsfRange.Location = New System.Drawing.Point(14, 166)
        Me.vsfRange.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.vsfRange.Name = "vsfRange"
        Me.vsfRange.Rows.Count = 4
        Me.vsfRange.Rows.DefaultSize = 24
        Me.vsfRange.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfRange.Size = New System.Drawing.Size(241, 84)
        Me.vsfRange.StyleInfo = resources.GetString("vsfRange.StyleInfo")
        Me.vsfRange.TabIndex = 10
        Me.vsfRange.Visible = False
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
        Me.vsfDet.AutoResize = True
        Me.vsfDet.AutoSearch = C1.Win.C1FlexGrid.AutoSearchEnum.FromTop
        Me.vsfDet.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle
        Me.vsfDet.ColumnInfo = "2,0,0,0,0,120,Columns:"
        Me.vsfDet.EditOptions = C1.Win.C1FlexGrid.EditFlags.AutoSearch
        Me.vsfDet.ExtendLastCol = True
        Me.vsfDet.FocusRect = C1.Win.C1FlexGrid.FocusRectEnum.Heavy
        Me.vsfDet.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.vsfDet.KeyActionEnter = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross
        Me.vsfDet.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross
        Me.vsfDet.Location = New System.Drawing.Point(7, 238)
        Me.vsfDet.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.vsfDet.Name = "vsfDet"
        Me.vsfDet.Rows.DefaultSize = 24
        Me.vsfDet.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfDet.Size = New System.Drawing.Size(900, 318)
        Me.vsfDet.StyleInfo = resources.GetString("vsfDet.StyleInfo")
        Me.vsfDet.TabIndex = 12
        Me.vsfDet.Tag = ""
        '
        'lblSelectloaction
        '
        Me.lblSelectloaction.AutoSize = True
        Me.lblSelectloaction.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSelectloaction.Location = New System.Drawing.Point(292, 582)
        Me.lblSelectloaction.Name = "lblSelectloaction"
        Me.lblSelectloaction.Size = New System.Drawing.Size(119, 20)
        Me.lblSelectloaction.TabIndex = 252
        Me.lblSelectloaction.Text = "Select Location"
        Me.lblSelectloaction.Visible = False
        '
        'cboSelectLocation
        '
        Me.cboSelectLocation.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboSelectLocation.FormattingEnabled = True
        Me.cboSelectLocation.Location = New System.Drawing.Point(417, 578)
        Me.cboSelectLocation.Name = "cboSelectLocation"
        Me.cboSelectLocation.Size = New System.Drawing.Size(153, 28)
        Me.cboSelectLocation.TabIndex = 1
        Me.cboSelectLocation.Visible = False
        '
        'btnExportToXL
        '
        Me.btnExportToXL.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnExportToXL.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnExportToXL.Image = CType(resources.GetObject("btnExportToXL.Image"), System.Drawing.Image)
        Me.btnExportToXL.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnExportToXL.Location = New System.Drawing.Point(7, 577)
        Me.btnExportToXL.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnExportToXL.Name = "btnExportToXL"
        Me.btnExportToXL.Size = New System.Drawing.Size(33, 29)
        Me.btnExportToXL.TabIndex = 13
        Me.btnExportToXL.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnExportToXL, "Export To Excel : Alt + E")
        Me.btnExportToXL.UseVisualStyleBackColor = False
        '
        'btnItem
        '
        Me.btnItem.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnItem.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnItem.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!)
        Me.btnItem.Image = Global.vLQMS.My.Resources.Resources.Paste
        Me.btnItem.Location = New System.Drawing.Point(602, 6)
        Me.btnItem.Name = "btnItem"
        Me.btnItem.Size = New System.Drawing.Size(21, 22)
        Me.btnItem.TabIndex = 4
        Me.ToolTip1.SetToolTip(Me.btnItem, "Past Item : Alt + I")
        Me.btnItem.UseVisualStyleBackColor = True
        '
        'btnPkgItem
        '
        Me.btnPkgItem.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnPkgItem.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!)
        Me.btnPkgItem.Image = Global.vLQMS.My.Resources.Resources.Paste
        Me.btnPkgItem.Location = New System.Drawing.Point(1033, 10)
        Me.btnPkgItem.Name = "btnPkgItem"
        Me.btnPkgItem.Size = New System.Drawing.Size(34, 33)
        Me.btnPkgItem.TabIndex = 7
        Me.ToolTip1.SetToolTip(Me.btnPkgItem, "Past package : Alt + A")
        Me.btnPkgItem.UseVisualStyleBackColor = True
        Me.btnPkgItem.Visible = False
        '
        'btnView
        '
        Me.btnView.BackColor = System.Drawing.Color.FromArgb(CType(CType(233, Byte), Integer), CType(CType(237, Byte), Integer), CType(CType(241, Byte), Integer))
        Me.btnView.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnView.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.btnView.Image = CType(resources.GetObject("btnView.Image"), System.Drawing.Image)
        Me.btnView.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnView.Location = New System.Drawing.Point(242, 8)
        Me.btnView.Name = "btnView"
        Me.btnView.Size = New System.Drawing.Size(76, 29)
        Me.btnView.TabIndex = 11
        Me.btnView.Text = "&View"
        Me.btnView.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnView, "View : Alt + V")
        Me.btnView.UseVisualStyleBackColor = False
        '
        'btnClose
        '
        Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClose.BackColor = System.Drawing.Color.FromArgb(CType(CType(233, Byte), Integer), CType(CType(237, Byte), Integer), CType(CType(241, Byte), Integer))
        Me.btnClose.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_CLOSE
        Me.btnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnClose.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!)
        Me.btnClose.Location = New System.Drawing.Point(837, 577)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(70, 29)
        Me.btnClose.TabIndex = 16
        Me.btnClose.Text = "Cl&ose"
        Me.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnClose, "Close : Alt + O")
        Me.btnClose.UseVisualStyleBackColor = False
        '
        'btnEmailPDF
        '
        Me.btnEmailPDF.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnEmailPDF.BackColor = System.Drawing.Color.FromArgb(CType(CType(233, Byte), Integer), CType(CType(237, Byte), Integer), CType(CType(241, Byte), Integer))
        Me.btnEmailPDF.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnEmailPDF.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.btnEmailPDF.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnEmailPDF.Location = New System.Drawing.Point(127, 577)
        Me.btnEmailPDF.Name = "btnEmailPDF"
        Me.btnEmailPDF.Size = New System.Drawing.Size(75, 29)
        Me.btnEmailPDF.TabIndex = 15
        Me.btnEmailPDF.Text = "&Email PDF"
        Me.ToolTip1.SetToolTip(Me.btnEmailPDF, "Email PDF : Alt + E")
        Me.btnEmailPDF.UseVisualStyleBackColor = False
        '
        'btnPrint
        '
        Me.btnPrint.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnPrint.BackColor = System.Drawing.Color.FromArgb(CType(CType(233, Byte), Integer), CType(CType(237, Byte), Integer), CType(CType(241, Byte), Integer))
        Me.btnPrint.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnPrint.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.btnPrint.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnPrint.Location = New System.Drawing.Point(46, 577)
        Me.btnPrint.Name = "btnPrint"
        Me.btnPrint.Size = New System.Drawing.Size(75, 29)
        Me.btnPrint.TabIndex = 14
        Me.btnPrint.Text = "&Print"
        Me.ToolTip1.SetToolTip(Me.btnPrint, "Print : Alt + P")
        Me.btnPrint.UseVisualStyleBackColor = False
        '
        'vsfPkgItem
        '
        Me.vsfPkgItem.AllowDragging = C1.Win.C1FlexGrid.AllowDraggingEnum.None
        Me.vsfPkgItem.AllowFiltering = True
        Me.vsfPkgItem.AllowFreezing = C1.Win.C1FlexGrid.AllowFreezingEnum.Columns
        Me.vsfPkgItem.AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.Free
        Me.vsfPkgItem.AllowMergingFixed = C1.Win.C1FlexGrid.AllowMergingEnum.Free
        Me.vsfPkgItem.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfPkgItem.AutoResize = True
        Me.vsfPkgItem.AutoSearch = C1.Win.C1FlexGrid.AutoSearchEnum.FromTop
        Me.vsfPkgItem.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle
        Me.vsfPkgItem.ColumnInfo = "2,0,0,0,0,105,Columns:0{Width:40;}" & Global.Microsoft.VisualBasic.ChrW(9)
        Me.vsfPkgItem.EditOptions = C1.Win.C1FlexGrid.EditFlags.AutoSearch
        Me.vsfPkgItem.ExtendLastCol = True
        Me.vsfPkgItem.FocusRect = C1.Win.C1FlexGrid.FocusRectEnum.Heavy
        Me.vsfPkgItem.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!)
        Me.vsfPkgItem.KeyActionEnter = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross
        Me.vsfPkgItem.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross
        Me.vsfPkgItem.Location = New System.Drawing.Point(632, 45)
        Me.vsfPkgItem.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.vsfPkgItem.Name = "vsfPkgItem"
        Me.vsfPkgItem.Rows.DefaultSize = 21
        Me.vsfPkgItem.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfPkgItem.Size = New System.Drawing.Size(274, 157)
        Me.vsfPkgItem.StyleInfo = resources.GetString("vsfPkgItem.StyleInfo")
        Me.vsfPkgItem.TabIndex = 6
        Me.vsfPkgItem.Tag = ""
        '
        'txtPkgItem
        '
        Me.txtPkgItem.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtPkgItem.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!)
        Me.txtPkgItem.Location = New System.Drawing.Point(753, 6)
        Me.txtPkgItem.MaxLength = 250
        Me.txtPkgItem.Name = "txtPkgItem"
        Me.txtPkgItem.Size = New System.Drawing.Size(152, 22)
        Me.txtPkgItem.TabIndex = 5
        '
        'vsfItem
        '
        Me.vsfItem.AllowDragging = C1.Win.C1FlexGrid.AllowDraggingEnum.None
        Me.vsfItem.AllowFiltering = True
        Me.vsfItem.AllowFreezing = C1.Win.C1FlexGrid.AllowFreezingEnum.Columns
        Me.vsfItem.AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.Free
        Me.vsfItem.AllowMergingFixed = C1.Win.C1FlexGrid.AllowMergingEnum.Free
        Me.vsfItem.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfItem.AutoResize = True
        Me.vsfItem.AutoSearch = C1.Win.C1FlexGrid.AutoSearchEnum.FromTop
        Me.vsfItem.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle
        Me.vsfItem.ColumnInfo = "2,0,0,0,0,105,Columns:0{Width:49;}" & Global.Microsoft.VisualBasic.ChrW(9)
        Me.vsfItem.EditOptions = C1.Win.C1FlexGrid.EditFlags.AutoSearch
        Me.vsfItem.ExtendLastCol = True
        Me.vsfItem.FocusRect = C1.Win.C1FlexGrid.FocusRectEnum.Heavy
        Me.vsfItem.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!)
        Me.vsfItem.KeyActionEnter = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross
        Me.vsfItem.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross
        Me.vsfItem.Location = New System.Drawing.Point(342, 44)
        Me.vsfItem.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.vsfItem.Name = "vsfItem"
        Me.vsfItem.Rows.DefaultSize = 21
        Me.vsfItem.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfItem.Size = New System.Drawing.Size(281, 158)
        Me.vsfItem.StyleInfo = resources.GetString("vsfItem.StyleInfo")
        Me.vsfItem.TabIndex = 3
        Me.vsfItem.Tag = ""
        '
        'txtItem
        '
        Me.txtItem.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtItem.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!)
        Me.txtItem.Location = New System.Drawing.Point(429, 6)
        Me.txtItem.MaxLength = 100
        Me.txtItem.Name = "txtItem"
        Me.txtItem.Size = New System.Drawing.Size(164, 22)
        Me.txtItem.TabIndex = 2
        '
        'vsfFldVal
        '
        Me.vsfFldVal.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None
        Me.vsfFldVal.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle
        Me.vsfFldVal.ColumnInfo = "2,0,0,0,0,105,Columns:0{Width:95;}" & Global.Microsoft.VisualBasic.ChrW(9)
        Me.vsfFldVal.ExtendLastCol = True
        Me.vsfFldVal.FocusRect = C1.Win.C1FlexGrid.FocusRectEnum.Heavy
        Me.vsfFldVal.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!)
        Me.vsfFldVal.KeyActionEnter = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross
        Me.vsfFldVal.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross
        Me.vsfFldVal.Location = New System.Drawing.Point(619, 577)
        Me.vsfFldVal.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.vsfFldVal.Name = "vsfFldVal"
        Me.vsfFldVal.Rows.DefaultSize = 21
        Me.vsfFldVal.Rows.Fixed = 0
        Me.vsfFldVal.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfFldVal.ShowThemedHeaders = C1.Win.C1FlexGrid.ShowThemedHeadersEnum.None
        Me.vsfFldVal.Size = New System.Drawing.Size(65, 29)
        Me.vsfFldVal.StyleInfo = resources.GetString("vsfFldVal.StyleInfo")
        Me.vsfFldVal.TabIndex = 8
        Me.vsfFldVal.Tag = ""
        Me.vsfFldVal.Visible = False
        '
        'lblSItem
        '
        Me.lblSItem.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblSItem.AutoSize = True
        Me.lblSItem.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSItem.Location = New System.Drawing.Point(338, 6)
        Me.lblSItem.Name = "lblSItem"
        Me.lblSItem.Size = New System.Drawing.Size(90, 20)
        Me.lblSItem.TabIndex = 291
        Me.lblSItem.Text = "Select Item"
        '
        'Label1
        '
        Me.Label1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(627, 6)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(120, 20)
        Me.Label1.TabIndex = 293
        Me.Label1.Text = "Select Package"
        '
        'txtDummy2
        '
        Me.txtDummy2.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.txtDummy2.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDummy2.Location = New System.Drawing.Point(768, 581)
        Me.txtDummy2.MaxLength = 50
        Me.txtDummy2.Name = "txtDummy2"
        Me.txtDummy2.Size = New System.Drawing.Size(65, 23)
        Me.txtDummy2.TabIndex = 295
        Me.txtDummy2.Visible = False
        '
        'txtDummy
        '
        Me.txtDummy.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.txtDummy.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDummy.Location = New System.Drawing.Point(697, 581)
        Me.txtDummy.MaxLength = 50
        Me.txtDummy.Name = "txtDummy"
        Me.txtDummy.Size = New System.Drawing.Size(65, 23)
        Me.txtDummy.TabIndex = 294
        Me.txtDummy.Visible = False
        '
        'cboValuation
        '
        Me.cboValuation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboValuation.FormattingEnabled = True
        Me.cboValuation.Items.AddRange(New Object() {"StdRate", "Last Purchase Rate", "FIFO", "Wt. Avg Rate"})
        Me.cboValuation.Location = New System.Drawing.Point(88, 258)
        Me.cboValuation.Name = "cboValuation"
        Me.cboValuation.Size = New System.Drawing.Size(256, 21)
        Me.cboValuation.TabIndex = 297
        Me.cboValuation.Visible = False
        '
        'lblValuation
        '
        Me.lblValuation.AutoSize = True
        Me.lblValuation.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!)
        Me.lblValuation.ForeColor = System.Drawing.Color.Red
        Me.lblValuation.Location = New System.Drawing.Point(18, 260)
        Me.lblValuation.Name = "lblValuation"
        Me.lblValuation.Size = New System.Drawing.Size(64, 16)
        Me.lblValuation.TabIndex = 296
        Me.lblValuation.Tag = "M"
        Me.lblValuation.Text = "Valuation"
        Me.lblValuation.Visible = False
        '
        'chkItemGroup
        '
        Me.chkItemGroup.AutoSize = True
        Me.chkItemGroup.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkItemGroup.Location = New System.Drawing.Point(14, 68)
        Me.chkItemGroup.Name = "chkItemGroup"
        Me.chkItemGroup.Size = New System.Drawing.Size(121, 20)
        Me.chkItemGroup.TabIndex = 298
        Me.chkItemGroup.Text = "Selected Group"
        Me.chkItemGroup.UseVisualStyleBackColor = True
        '
        'lblSelectItem
        '
        Me.lblSelectItem.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblSelectItem.AutoSize = True
        Me.lblSelectItem.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!)
        Me.lblSelectItem.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblSelectItem.Location = New System.Drawing.Point(387, 218)
        Me.lblSelectItem.Name = "lblSelectItem"
        Me.lblSelectItem.Size = New System.Drawing.Size(206, 16)
        Me.lblSelectItem.TabIndex = 299
        Me.lblSelectItem.Tag = ""
        Me.lblSelectItem.Text = "Press Down Arrow To Select Item"
        '
        'Label2
        '
        Me.Label2.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!)
        Me.Label2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label2.Location = New System.Drawing.Point(661, 218)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(239, 16)
        Me.Label2.TabIndex = 300
        Me.Label2.Tag = ""
        Me.Label2.Text = "Press  Down Arrow To Select Package"
        '
        'chkShowPB
        '
        Me.chkShowPB.AutoSize = True
        Me.chkShowPB.Checked = True
        Me.chkShowPB.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkShowPB.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkShowPB.Location = New System.Drawing.Point(362, 258)
        Me.chkShowPB.Name = "chkShowPB"
        Me.chkShowPB.Size = New System.Drawing.Size(231, 20)
        Me.chkShowPB.TabIndex = 301
        Me.chkShowPB.Text = "Show Ageing - Purchase Bill Wise "
        Me.chkShowPB.UseVisualStyleBackColor = True
        Me.chkShowPB.Visible = False
        '
        'pnlCopyShortCut
        '
        Me.pnlCopyShortCut.Controls.Add(Me.btnCopy)
        Me.pnlCopyShortCut.Controls.Add(Me.pnlbtnClose)
        Me.pnlCopyShortCut.Controls.Add(Me.Label5)
        Me.pnlCopyShortCut.Controls.Add(Me.Label3)
        Me.pnlCopyShortCut.Controls.Add(Me.Label6)
        Me.pnlCopyShortCut.Controls.Add(Me.Label7)
        Me.pnlCopyShortCut.Controls.Add(Me.RbnCopyGrid)
        Me.pnlCopyShortCut.Controls.Add(Me.RbnCopyRow)
        Me.pnlCopyShortCut.Controls.Add(Me.RbnCopyColumn)
        Me.pnlCopyShortCut.Controls.Add(Me.RbnCopyCell)
        Me.pnlCopyShortCut.Location = New System.Drawing.Point(283, 304)
        Me.pnlCopyShortCut.Name = "pnlCopyShortCut"
        Me.pnlCopyShortCut.Size = New System.Drawing.Size(329, 177)
        Me.pnlCopyShortCut.TabIndex = 305
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
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(178, 79)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(77, 16)
        Me.Label3.TabIndex = 6
        Me.Label3.Text = "Ctrl + Alt + C"
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
        'frmRptStockAgeing
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(915, 609)
        Me.Controls.Add(Me.pnlCopyShortCut)
        Me.Controls.Add(Me.chkShowPB)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.lblSelectItem)
        Me.Controls.Add(Me.chkItemGroup)
        Me.Controls.Add(Me.cboValuation)
        Me.Controls.Add(Me.lblValuation)
        Me.Controls.Add(Me.txtDummy2)
        Me.Controls.Add(Me.txtDummy)
        Me.Controls.Add(Me.btnEmailPDF)
        Me.Controls.Add(Me.btnPrint)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.btnView)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.btnItem)
        Me.Controls.Add(Me.lblSItem)
        Me.Controls.Add(Me.vsfFldVal)
        Me.Controls.Add(Me.vsfPkgItem)
        Me.Controls.Add(Me.txtPkgItem)
        Me.Controls.Add(Me.vsfItem)
        Me.Controls.Add(Me.txtItem)
        Me.Controls.Add(Me.btnExportToXL)
        Me.Controls.Add(Me.cboSelectLocation)
        Me.Controls.Add(Me.lblSelectloaction)
        Me.Controls.Add(Me.vsfDet)
        Me.Controls.Add(Me.vsfRange)
        Me.Controls.Add(Me.dtpDate)
        Me.Controls.Add(Me.lblAsOn)
        Me.Controls.Add(Me.btnPkgItem)
        Me.Name = "frmRptStockAgeing"
        Me.ShowIcon = False
        Me.Text = "Stock Ageing Report"
        CType(Me.vsfRange, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.vsfPkgItem, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.vsfItem, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.vsfFldVal, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlCopyShortCut.ResumeLayout(False)
        Me.pnlCopyShortCut.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents lblAsOn As Label
    Friend WithEvents dtpDate As DateTimePicker
    Friend WithEvents vsfRange As C1.Win.C1FlexGrid.C1FlexGrid
    Public WithEvents vsfDet As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents lblSelectloaction As Label
    Friend WithEvents cboSelectLocation As ComboBox
    Friend WithEvents ToolTip1 As ToolTip
    Friend WithEvents btnExportToXL As Button
    Public WithEvents vsfPkgItem As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents txtPkgItem As TextBox
    Public WithEvents vsfItem As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents txtItem As TextBox
    Public WithEvents vsfFldVal As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents lblSItem As Label
    Friend WithEvents btnItem As Button
    Friend WithEvents Label1 As Label
    Friend WithEvents btnPkgItem As Button
    Friend WithEvents btnView As Button
    Friend WithEvents btnClose As Button
    Friend WithEvents btnEmailPDF As Button
    Friend WithEvents btnPrint As Button
    Friend WithEvents txtDummy2 As TextBox
    Friend WithEvents txtDummy As TextBox
    Friend WithEvents cboValuation As ComboBox
    Friend WithEvents lblValuation As Label
    Friend WithEvents chkItemGroup As CheckBox
    Friend WithEvents lblSelectItem As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents chkShowPB As CheckBox
    Friend WithEvents pnlCopyShortCut As Panel
    Friend WithEvents btnCopy As Button
    Friend WithEvents pnlbtnClose As Button
    Friend WithEvents Label5 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents Label6 As Label
    Friend WithEvents Label7 As Label
    Friend WithEvents RbnCopyGrid As RadioButton
    Friend WithEvents RbnCopyRow As RadioButton
    Friend WithEvents RbnCopyColumn As RadioButton
    Friend WithEvents RbnCopyCell As RadioButton
End Class
