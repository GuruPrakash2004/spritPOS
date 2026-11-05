<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmUtlMstView
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmUtlMstView))
        Me.vsfDet = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.lblTotRec = New System.Windows.Forms.Label()
        Me.chkClose = New System.Windows.Forms.CheckBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.vsfSearch = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.pnlDtl = New System.Windows.Forms.Panel()
        Me.vsfInvDet = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.btnExport = New C1.Win.C1Input.C1Button()
        Me.btnFreeze = New C1.Win.C1Input.C1Button()
        Me.btnSearchSettings = New System.Windows.Forms.Button()
        Me.btnTranspose = New C1.Win.C1Input.C1Button()
        Me.btnClose = New C1.Win.C1Input.C1Button()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.vsfSearch, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlDtl.SuspendLayout()
        CType(Me.vsfInvDet, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'vsfDet
        '
        Me.vsfDet.AllowDragging = C1.Win.C1FlexGrid.AllowDraggingEnum.None
        Me.vsfDet.AllowFiltering = True
        Me.vsfDet.AllowFreezing = C1.Win.C1FlexGrid.AllowFreezingEnum.Columns
        Me.vsfDet.AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.RestrictAll
        Me.vsfDet.AllowMergingFixed = C1.Win.C1FlexGrid.AllowMergingEnum.Free
        Me.vsfDet.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.MultiColumn
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
        Me.vsfDet.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross
        Me.vsfDet.Location = New System.Drawing.Point(3, 22)
        Me.vsfDet.Name = "vsfDet"
        Me.vsfDet.Rows.DefaultSize = 24
        Me.vsfDet.Rows.Fixed = 3
        Me.vsfDet.Rows.GlyphRow = 3
        Me.vsfDet.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfDet.ShowCellLabels = True
        Me.vsfDet.Size = New System.Drawing.Size(1142, 400)
        Me.vsfDet.StyleInfo = resources.GetString("vsfDet.StyleInfo")
        Me.vsfDet.TabIndex = 1
        Me.vsfDet.Tag = "View"
        Me.vsfDet.VisualStyle = C1.Win.C1FlexGrid.VisualStyle.Office2007Blue
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
        Me.chkClose.TabIndex = 2
        Me.chkClose.Text = "Do NOT hide after loading the doc."
        Me.chkClose.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.Label1.ForeColor = System.Drawing.Color.Red
        Me.Label1.Location = New System.Drawing.Point(267, 447)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(276, 17)
        Me.Label1.TabIndex = 3
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
        Me.vsfSearch.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross
        Me.vsfSearch.Location = New System.Drawing.Point(3, 1)
        Me.vsfSearch.Name = "vsfSearch"
        Me.vsfSearch.Rows.Count = 1
        Me.vsfSearch.Rows.DefaultSize = 24
        Me.vsfSearch.Rows.Fixed = 0
        Me.vsfSearch.ScrollBars = System.Windows.Forms.ScrollBars.None
        Me.vsfSearch.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfSearch.ShowCellLabels = True
        Me.vsfSearch.Size = New System.Drawing.Size(1142, 21)
        Me.vsfSearch.StyleInfo = resources.GetString("vsfSearch.StyleInfo")
        Me.vsfSearch.TabIndex = 0
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
        Me.pnlDtl.Size = New System.Drawing.Size(560, 208)
        Me.pnlDtl.TabIndex = 9
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
        Me.vsfInvDet.Location = New System.Drawing.Point(182, 14)
        Me.vsfInvDet.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.vsfInvDet.Name = "vsfInvDet"
        Me.vsfInvDet.Rows.Count = 4
        Me.vsfInvDet.Rows.DefaultSize = 24
        Me.vsfInvDet.Rows.Fixed = 0
        Me.vsfInvDet.Size = New System.Drawing.Size(343, 155)
        Me.vsfInvDet.StyleInfo = resources.GetString("vsfInvDet.StyleInfo")
        Me.vsfInvDet.TabIndex = 0
        Me.vsfInvDet.Tag = "DSGR"
        '
        'btnExport
        '
        Me.btnExport.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnExport.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnExport.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.btnExport.Image = CType(resources.GetObject("btnExport.Image"), System.Drawing.Image)
        Me.btnExport.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnExport.Location = New System.Drawing.Point(1030, 440)
        Me.btnExport.Name = "btnExport"
        Me.btnExport.Size = New System.Drawing.Size(35, 31)
        Me.btnExport.TabIndex = 7
        Me.btnExport.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnExport, "Export To Excel:Alt+E")
        Me.btnExport.UseVisualStyleBackColor = False
        '
        'btnFreeze
        '
        Me.btnFreeze.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnFreeze.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnFreeze.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.btnFreeze.Image = CType(resources.GetObject("btnFreeze.Image"), System.Drawing.Image)
        Me.btnFreeze.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnFreeze.Location = New System.Drawing.Point(745, 440)
        Me.btnFreeze.Name = "btnFreeze"
        Me.btnFreeze.Size = New System.Drawing.Size(148, 31)
        Me.btnFreeze.TabIndex = 4
        Me.btnFreeze.Text = "&Freeze Columns"
        Me.btnFreeze.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnFreeze, "FreeZe Columns:Alt+F")
        Me.btnFreeze.UseVisualStyleBackColor = False
        '
        'btnSearchSettings
        '
        Me.btnSearchSettings.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSearchSettings.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSearchSettings.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.btnSearchSettings.Image = CType(resources.GetObject("btnSearchSettings.Image"), System.Drawing.Image)
        Me.btnSearchSettings.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnSearchSettings.Location = New System.Drawing.Point(991, 440)
        Me.btnSearchSettings.Margin = New System.Windows.Forms.Padding(4)
        Me.btnSearchSettings.Name = "btnSearchSettings"
        Me.btnSearchSettings.Size = New System.Drawing.Size(33, 31)
        Me.btnSearchSettings.TabIndex = 6
        Me.btnSearchSettings.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnSearchSettings, "Search:Alt+S")
        Me.btnSearchSettings.UseVisualStyleBackColor = True
        '
        'btnTranspose
        '
        Me.btnTranspose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnTranspose.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnTranspose.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.btnTranspose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnTranspose.Location = New System.Drawing.Point(899, 440)
        Me.btnTranspose.Name = "btnTranspose"
        Me.btnTranspose.Size = New System.Drawing.Size(86, 31)
        Me.btnTranspose.TabIndex = 5
        Me.btnTranspose.Text = "&Transpose"
        Me.btnTranspose.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnTranspose, "Transpose:Alt+T")
        Me.btnTranspose.UseVisualStyleBackColor = False
        '
        'btnClose
        '
        Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClose.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_CLOSE
        Me.btnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnClose.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnClose.Location = New System.Drawing.Point(1071, 440)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(74, 31)
        Me.btnClose.TabIndex = 8
        Me.btnClose.Text = "Cl&ose"
        Me.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnClose, "Close:Alt+O")
        Me.btnClose.UseVisualStyleBackColor = False
        '
        'frmUtlMstView
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(233, Byte), Integer), CType(CType(237, Byte), Integer), CType(CType(241, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(1148, 483)
        Me.Controls.Add(Me.btnSearchSettings)
        Me.Controls.Add(Me.btnTranspose)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.btnExport)
        Me.Controls.Add(Me.pnlDtl)
        Me.Controls.Add(Me.vsfSearch)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.chkClose)
        Me.Controls.Add(Me.lblTotRec)
        Me.Controls.Add(Me.btnFreeze)
        Me.Controls.Add(Me.vsfDet)
        Me.MinimumSize = New System.Drawing.Size(989, 521)
        Me.Name = "frmUtlMstView"
        Me.ShowIcon = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "View"
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.vsfSearch, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlDtl.ResumeLayout(False)
        CType(Me.vsfInvDet, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents vsfDet As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents btnFreeze As C1.Win.C1Input.C1Button
    Friend WithEvents lblTotRec As System.Windows.Forms.Label
    Friend WithEvents chkClose As System.Windows.Forms.CheckBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents vsfSearch As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents pnlDtl As System.Windows.Forms.Panel
    Friend WithEvents vsfInvDet As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents btnExport As C1.Win.C1Input.C1Button
    Friend WithEvents btnSearchSettings As Button
    Friend WithEvents btnTranspose As C1.Win.C1Input.C1Button
    Friend WithEvents btnClose As C1.Win.C1Input.C1Button
    Friend WithEvents ToolTip1 As ToolTip
End Class
