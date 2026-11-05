<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmUtlTrnView
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmUtlTrnView))
        Me.vsfDet = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.btnClose = New C1.Win.C1Input.C1Button()
        Me.lblTotRec = New System.Windows.Forms.Label()
        Me.chkClose = New System.Windows.Forms.CheckBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.vsfSearch = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.pnlDtl = New System.Windows.Forms.Panel()
        Me.vsfInvDet = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.TS = New System.Windows.Forms.ToolStrip()
        Me.TStripFreeze = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.TStripExportXL = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me.TStripSelCols = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator3 = New System.Windows.Forms.ToolStripSeparator()
        Me.TStripVwDetGrid = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator4 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolTip = New System.Windows.Forms.ToolTip(Me.components)
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.vsfSearch, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlDtl.SuspendLayout()
        CType(Me.vsfInvDet, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TS.SuspendLayout()
        Me.SuspendLayout()
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
        Me.vsfDet.Location = New System.Drawing.Point(3, 51)
        Me.vsfDet.Name = "vsfDet"
        Me.vsfDet.Rows.DefaultSize = 24
        Me.vsfDet.Rows.Fixed = 3
        Me.vsfDet.Rows.GlyphRow = 3
        Me.vsfDet.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfDet.ShowCellLabels = True
        Me.vsfDet.Size = New System.Drawing.Size(1042, 369)
        Me.vsfDet.StyleInfo = resources.GetString("vsfDet.StyleInfo")
        Me.vsfDet.TabIndex = 13
        Me.vsfDet.Tag = "View"
        Me.vsfDet.VisualStyle = C1.Win.C1FlexGrid.VisualStyle.Office2007Blue
        '
        'btnClose
        '
        Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClose.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_CLOSE
        Me.btnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnClose.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnClose.Location = New System.Drawing.Point(971, 439)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(74, 31)
        Me.btnClose.TabIndex = 16
        Me.btnClose.Text = "Cl&ose"
        Me.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnClose.UseVisualStyleBackColor = False
        '
        'lblTotRec
        '
        Me.lblTotRec.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblTotRec.AutoSize = True
        Me.lblTotRec.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.lblTotRec.Location = New System.Drawing.Point(6, 425)
        Me.lblTotRec.Name = "lblTotRec"
        Me.lblTotRec.Size = New System.Drawing.Size(0, 17)
        Me.lblTotRec.TabIndex = 17
        '
        'chkClose
        '
        Me.chkClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.chkClose.AutoSize = True
        Me.chkClose.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.chkClose.Location = New System.Drawing.Point(9, 445)
        Me.chkClose.Name = "chkClose"
        Me.chkClose.Size = New System.Drawing.Size(234, 21)
        Me.chkClose.TabIndex = 18
        Me.chkClose.Text = "Do NOT hide after loading the doc."
        Me.chkClose.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.Label1.ForeColor = System.Drawing.Color.Red
        Me.Label1.Location = New System.Drawing.Point(256, 447)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(276, 17)
        Me.Label1.TabIndex = 19
        Me.Label1.Text = "Use column headers for filtering and sorting..."
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
        Me.vsfSearch.Location = New System.Drawing.Point(3, 29)
        Me.vsfSearch.Name = "vsfSearch"
        Me.vsfSearch.Rows.Count = 1
        Me.vsfSearch.Rows.DefaultSize = 24
        Me.vsfSearch.Rows.Fixed = 0
        Me.vsfSearch.ScrollBars = System.Windows.Forms.ScrollBars.None
        Me.vsfSearch.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfSearch.ShowCellLabels = True
        Me.vsfSearch.Size = New System.Drawing.Size(1042, 21)
        Me.vsfSearch.StyleInfo = resources.GetString("vsfSearch.StyleInfo")
        Me.vsfSearch.TabIndex = 20
        Me.vsfSearch.Tag = ""
        Me.vsfSearch.VisualStyle = C1.Win.C1FlexGrid.VisualStyle.System
        '
        'pnlDtl
        '
        Me.pnlDtl.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnlDtl.Controls.Add(Me.vsfInvDet)
        Me.pnlDtl.Location = New System.Drawing.Point(259, 146)
        Me.pnlDtl.Name = "pnlDtl"
        Me.pnlDtl.Size = New System.Drawing.Size(460, 208)
        Me.pnlDtl.TabIndex = 198
        Me.pnlDtl.Visible = False
        '
        'vsfInvDet
        '
        Me.vsfInvDet.AllowDragging = C1.Win.C1FlexGrid.AllowDraggingEnum.None
        Me.vsfInvDet.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfInvDet.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle
        Me.vsfInvDet.ColumnInfo = "2,1,0,0,0,120,Columns:"
        Me.vsfInvDet.ExtendLastCol = True
        Me.vsfInvDet.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.vsfInvDet.Location = New System.Drawing.Point(98, 14)
        Me.vsfInvDet.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.vsfInvDet.Name = "vsfInvDet"
        Me.vsfInvDet.Rows.Count = 4
        Me.vsfInvDet.Rows.DefaultSize = 24
        Me.vsfInvDet.Rows.Fixed = 0
        Me.vsfInvDet.Size = New System.Drawing.Size(343, 155)
        Me.vsfInvDet.StyleInfo = resources.GetString("vsfInvDet.StyleInfo")
        Me.vsfInvDet.TabIndex = 199
        Me.vsfInvDet.Tag = "DSGR"
        '
        'TS
        '
        Me.TS.AutoSize = False
        Me.TS.BackColor = System.Drawing.Color.LightSteelBlue
        Me.TS.Dock = System.Windows.Forms.DockStyle.None
        Me.TS.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.TS.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.TStripFreeze, Me.ToolStripSeparator1, Me.TStripExportXL, Me.ToolStripSeparator2, Me.TStripSelCols, Me.ToolStripSeparator3, Me.TStripVwDetGrid, Me.ToolStripSeparator4})
        Me.TS.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow
        Me.TS.Location = New System.Drawing.Point(3, 3)
        Me.TS.Name = "TS"
        Me.TS.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional
        Me.TS.Size = New System.Drawing.Size(384, 24)
        Me.TS.TabIndex = 201
        '
        'TStripFreeze
        '
        Me.TStripFreeze.Image = CType(resources.GetObject("TStripFreeze.Image"), System.Drawing.Image)
        Me.TStripFreeze.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.TStripFreeze.Name = "TStripFreeze"
        Me.TStripFreeze.Size = New System.Drawing.Size(111, 20)
        Me.TStripFreeze.Text = "&Freeze Columns"
        Me.TStripFreeze.ToolTipText = "Freeze Columns:Alt+F"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(6, 23)
        '
        'TStripExportXL
        '
        Me.TStripExportXL.Image = CType(resources.GetObject("TStripExportXL.Image"), System.Drawing.Image)
        Me.TStripExportXL.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.TStripExportXL.Name = "TStripExportXL"
        Me.TStripExportXL.Size = New System.Drawing.Size(103, 20)
        Me.TStripExportXL.Text = "&Export to Excel"
        Me.TStripExportXL.ToolTipText = "Export to Excel:Alt+E"
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(6, 23)
        '
        'TStripSelCols
        '
        Me.TStripSelCols.Image = Global.vLQMS.My.Resources.Resources.D_FIND
        Me.TStripSelCols.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.TStripSelCols.Name = "TStripSelCols"
        Me.TStripSelCols.Size = New System.Drawing.Size(84, 20)
        Me.TStripSelCols.Text = "&Select Cols"
        Me.TStripSelCols.ToolTipText = "ShortCut Key:Alt+S"
        '
        'ToolStripSeparator3
        '
        Me.ToolStripSeparator3.Name = "ToolStripSeparator3"
        Me.ToolStripSeparator3.Size = New System.Drawing.Size(6, 23)
        '
        'TStripVwDetGrid
        '
        Me.TStripVwDetGrid.Image = CType(resources.GetObject("TStripVwDetGrid.Image"), System.Drawing.Image)
        Me.TStripVwDetGrid.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.TStripVwDetGrid.Name = "TStripVwDetGrid"
        Me.TStripVwDetGrid.Size = New System.Drawing.Size(110, 20)
        Me.TStripVwDetGrid.Text = "View &Detail Grid"
        Me.TStripVwDetGrid.ToolTipText = "View Detail Grid : Alt + D"
        '
        'ToolStripSeparator4
        '
        Me.ToolStripSeparator4.Name = "ToolStripSeparator4"
        Me.ToolStripSeparator4.Size = New System.Drawing.Size(6, 23)
        '
        'frmUtlTrnView
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(233, Byte), Integer), CType(CType(237, Byte), Integer), CType(CType(241, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(1048, 483)
        Me.Controls.Add(Me.TS)
        Me.Controls.Add(Me.pnlDtl)
        Me.Controls.Add(Me.vsfSearch)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.chkClose)
        Me.Controls.Add(Me.lblTotRec)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.vsfDet)
        Me.KeyPreview = True
        Me.MinimumSize = New System.Drawing.Size(989, 521)
        Me.Name = "frmUtlTrnView"
        Me.ShowIcon = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "View"
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.vsfSearch, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlDtl.ResumeLayout(False)
        CType(Me.vsfInvDet, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TS.ResumeLayout(False)
        Me.TS.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents vsfDet As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents btnClose As C1.Win.C1Input.C1Button
    Friend WithEvents lblTotRec As System.Windows.Forms.Label
    Friend WithEvents chkClose As System.Windows.Forms.CheckBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents vsfSearch As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents pnlDtl As System.Windows.Forms.Panel
    Friend WithEvents vsfInvDet As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents TS As System.Windows.Forms.ToolStrip
    Friend WithEvents TStripFreeze As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents TStripExportXL As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents TStripSelCols As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolTip As System.Windows.Forms.ToolTip
    Friend WithEvents ToolStripSeparator3 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents TStripVwDetGrid As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator4 As System.Windows.Forms.ToolStripSeparator
End Class
