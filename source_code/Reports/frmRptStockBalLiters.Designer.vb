<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmRptStockBalLiters
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmRptStockBalLiters))
        Me.grbForThePeriod = New System.Windows.Forms.GroupBox()
        Me.dtpToDate = New System.Windows.Forms.DateTimePicker()
        Me.lblTo = New System.Windows.Forms.Label()
        Me.dtpFromDate = New System.Windows.Forms.DateTimePicker()
        Me.lblFrom = New System.Windows.Forms.Label()
        Me.chkSelectedGroup = New System.Windows.Forms.CheckBox()
        Me.btnView = New System.Windows.Forms.Button()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.btnExportToXL = New System.Windows.Forms.Button()
        Me.vsfView = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.grbForThePeriod.SuspendLayout()
        CType(Me.vsfView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'grbForThePeriod
        '
        Me.grbForThePeriod.Controls.Add(Me.dtpToDate)
        Me.grbForThePeriod.Controls.Add(Me.lblTo)
        Me.grbForThePeriod.Controls.Add(Me.dtpFromDate)
        Me.grbForThePeriod.Controls.Add(Me.lblFrom)
        Me.grbForThePeriod.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grbForThePeriod.Location = New System.Drawing.Point(9, 13)
        Me.grbForThePeriod.Name = "grbForThePeriod"
        Me.grbForThePeriod.Size = New System.Drawing.Size(330, 59)
        Me.grbForThePeriod.TabIndex = 0
        Me.grbForThePeriod.TabStop = False
        Me.grbForThePeriod.Text = "For The Period "
        '
        'dtpToDate
        '
        Me.dtpToDate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpToDate.Location = New System.Drawing.Point(205, 22)
        Me.dtpToDate.Name = "dtpToDate"
        Me.dtpToDate.Size = New System.Drawing.Size(106, 26)
        Me.dtpToDate.TabIndex = 3
        '
        'lblTo
        '
        Me.lblTo.AutoSize = True
        Me.lblTo.Location = New System.Drawing.Point(172, 26)
        Me.lblTo.Name = "lblTo"
        Me.lblTo.Size = New System.Drawing.Size(27, 20)
        Me.lblTo.TabIndex = 2
        Me.lblTo.Text = "To"
        '
        'dtpFromDate
        '
        Me.dtpFromDate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpFromDate.Location = New System.Drawing.Point(61, 22)
        Me.dtpFromDate.Name = "dtpFromDate"
        Me.dtpFromDate.Size = New System.Drawing.Size(106, 26)
        Me.dtpFromDate.TabIndex = 1
        '
        'lblFrom
        '
        Me.lblFrom.AutoSize = True
        Me.lblFrom.Location = New System.Drawing.Point(11, 26)
        Me.lblFrom.Name = "lblFrom"
        Me.lblFrom.Size = New System.Drawing.Size(46, 20)
        Me.lblFrom.TabIndex = 0
        Me.lblFrom.Text = "From"
        '
        'chkSelectedGroup
        '
        Me.chkSelectedGroup.AutoSize = True
        Me.chkSelectedGroup.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkSelectedGroup.Location = New System.Drawing.Point(14, 80)
        Me.chkSelectedGroup.Name = "chkSelectedGroup"
        Me.chkSelectedGroup.Size = New System.Drawing.Size(140, 24)
        Me.chkSelectedGroup.TabIndex = 1
        Me.chkSelectedGroup.Text = "Selected Group"
        Me.chkSelectedGroup.UseVisualStyleBackColor = True
        '
        'btnView
        '
        Me.btnView.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnView.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnView.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnView.Location = New System.Drawing.Point(834, 110)
        Me.btnView.Name = "btnView"
        Me.btnView.Size = New System.Drawing.Size(81, 30)
        Me.btnView.TabIndex = 2
        Me.btnView.Text = "&View"
        Me.ToolTip1.SetToolTip(Me.btnView, "Shortcut: Alt+V")
        Me.btnView.UseVisualStyleBackColor = True
        '
        'btnClose
        '
        Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnClose.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnClose.Location = New System.Drawing.Point(921, 110)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(76, 30)
        Me.btnClose.TabIndex = 3
        Me.btnClose.Text = "&Close"
        Me.ToolTip1.SetToolTip(Me.btnClose, "Shortcut: Alt+C")
        Me.btnClose.UseVisualStyleBackColor = True
        '
        'btnExportToXL
        '
        Me.btnExportToXL.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnExportToXL.BackColor = System.Drawing.Color.FromArgb(CType(CType(233, Byte), Integer), CType(CType(237, Byte), Integer), CType(CType(241, Byte), Integer))
        Me.btnExportToXL.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnExportToXL.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!)
        Me.btnExportToXL.Image = CType(resources.GetObject("btnExportToXL.Image"), System.Drawing.Image)
        Me.btnExportToXL.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnExportToXL.Location = New System.Drawing.Point(13, 470)
        Me.btnExportToXL.Name = "btnExportToXL"
        Me.btnExportToXL.Size = New System.Drawing.Size(35, 29)
        Me.btnExportToXL.TabIndex = 252
        Me.btnExportToXL.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnExportToXL, "Export To Excel : Alt + E")
        Me.btnExportToXL.UseVisualStyleBackColor = False
        '
        'vsfView
        '
        Me.vsfView.AllowDragging = C1.Win.C1FlexGrid.AllowDraggingEnum.None
        Me.vsfView.AllowFiltering = True
        Me.vsfView.AllowFreezing = C1.Win.C1FlexGrid.AllowFreezingEnum.Columns
        Me.vsfView.AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.Free
        Me.vsfView.AllowMergingFixed = C1.Win.C1FlexGrid.AllowMergingEnum.Free
        Me.vsfView.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfView.AutoResize = True
        Me.vsfView.AutoSearch = C1.Win.C1FlexGrid.AutoSearchEnum.FromTop
        Me.vsfView.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle
        Me.vsfView.ColumnInfo = "2,0,0,0,0,120,Columns:"
        Me.vsfView.EditOptions = C1.Win.C1FlexGrid.EditFlags.AutoSearch
        Me.vsfView.ExtendLastCol = True
        Me.vsfView.FocusRect = C1.Win.C1FlexGrid.FocusRectEnum.Heavy
        Me.vsfView.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.vsfView.KeyActionEnter = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross
        Me.vsfView.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross
        Me.vsfView.Location = New System.Drawing.Point(13, 151)
        Me.vsfView.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.vsfView.Name = "vsfView"
        Me.vsfView.Rows.DefaultSize = 24
        Me.vsfView.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfView.Size = New System.Drawing.Size(985, 314)
        Me.vsfView.StyleInfo = resources.GetString("vsfView.StyleInfo")
        Me.vsfView.TabIndex = 251
        Me.vsfView.Tag = ""
        '
        'frmRptStockBalLiters
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1010, 511)
        Me.Controls.Add(Me.btnExportToXL)
        Me.Controls.Add(Me.vsfView)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.btnView)
        Me.Controls.Add(Me.chkSelectedGroup)
        Me.Controls.Add(Me.grbForThePeriod)
        Me.Name = "frmRptStockBalLiters"
        Me.Text = "frmRptStockBalLiters"
        Me.grbForThePeriod.ResumeLayout(False)
        Me.grbForThePeriod.PerformLayout()
        CType(Me.vsfView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents grbForThePeriod As GroupBox
    Friend WithEvents dtpToDate As DateTimePicker
    Friend WithEvents lblTo As Label
    Friend WithEvents dtpFromDate As DateTimePicker
    Friend WithEvents lblFrom As Label
    Friend WithEvents chkSelectedGroup As CheckBox
    Friend WithEvents btnView As Button
    Friend WithEvents btnClose As Button
    Friend WithEvents ToolTip1 As ToolTip
    Public WithEvents vsfView As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents btnExportToXL As Button
End Class
