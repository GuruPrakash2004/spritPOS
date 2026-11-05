<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmUtlMnuGrpItemSel
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmUtlMnuGrpItemSel))
        Dim CBlendItems1 As vLQMS.cBlendItems = New vLQMS.cBlendItems()
        Dim CBlendItems2 As vLQMS.cBlendItems = New vLQMS.cBlendItems()
        Me.vsfDet = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.lblTotRec = New System.Windows.Forms.Label()
        Me.vsfSearch = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.pnlDtl = New System.Windows.Forms.Panel()
        Me.vsfInvDet = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.btnOK = New vLQMS.CButton()
        Me.btnExport = New vLQMS.CButton()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.btnCloseEdit = New C1.Win.C1Input.C1Button()
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.vsfSearch, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlDtl.SuspendLayout()
        CType(Me.vsfInvDet, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
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
        Me.vsfDet.ColumnInfo = "2,0,0,0,0,120,Columns:"
        Me.vsfDet.EditOptions = C1.Win.C1FlexGrid.EditFlags.AutoSearch
        Me.vsfDet.ExtendLastCol = True
        Me.vsfDet.FocusRect = C1.Win.C1FlexGrid.FocusRectEnum.Heavy
        Me.vsfDet.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.vsfDet.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross
        Me.vsfDet.Location = New System.Drawing.Point(3, 25)
        Me.vsfDet.Name = "vsfDet"
        Me.vsfDet.Rows.DefaultSize = 24
        Me.vsfDet.Rows.Fixed = 3
        Me.vsfDet.Rows.GlyphRow = 2
        Me.vsfDet.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfDet.ShowCellLabels = True
        Me.vsfDet.Size = New System.Drawing.Size(967, 400)
        Me.vsfDet.StyleInfo = resources.GetString("vsfDet.StyleInfo")
        Me.vsfDet.TabIndex = 13
        Me.vsfDet.Tag = "View"
        Me.vsfDet.VisualStyle = C1.Win.C1FlexGrid.VisualStyle.Office2007Blue
        '
        'lblTotRec
        '
        Me.lblTotRec.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblTotRec.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTotRec.ForeColor = System.Drawing.Color.Blue
        Me.lblTotRec.Location = New System.Drawing.Point(139, 439)
        Me.lblTotRec.Name = "lblTotRec"
        Me.lblTotRec.Size = New System.Drawing.Size(266, 25)
        Me.lblTotRec.TabIndex = 17
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
        Me.vsfSearch.Location = New System.Drawing.Point(3, 3)
        Me.vsfSearch.Name = "vsfSearch"
        Me.vsfSearch.Rows.Count = 1
        Me.vsfSearch.Rows.DefaultSize = 24
        Me.vsfSearch.Rows.Fixed = 0
        Me.vsfSearch.ScrollBars = System.Windows.Forms.ScrollBars.None
        Me.vsfSearch.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfSearch.ShowCellLabels = True
        Me.vsfSearch.Size = New System.Drawing.Size(967, 21)
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
        Me.pnlDtl.Location = New System.Drawing.Point(259, 149)
        Me.pnlDtl.Name = "pnlDtl"
        Me.pnlDtl.Size = New System.Drawing.Size(385, 208)
        Me.pnlDtl.TabIndex = 198
        Me.pnlDtl.Visible = False
        '
        'vsfInvDet
        '
        Me.vsfInvDet.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfInvDet.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle
        Me.vsfInvDet.ColumnInfo = "2,1,0,0,0,120,Columns:"
        Me.vsfInvDet.ExtendLastCol = True
        Me.vsfInvDet.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.vsfInvDet.Location = New System.Drawing.Point(23, 14)
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
        'btnOK
        '
        CBlendItems1.iColor = New System.Drawing.Color() {System.Drawing.Color.White, System.Drawing.Color.White, System.Drawing.Color.ForestGreen}
        CBlendItems1.iPoint = New Single() {0!, 0.1273292!, 1.0!}
        Me.btnOK.ColorFillBlend = CBlendItems1
        Me.btnOK.ColorFillSolid = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.btnOK.DesignerSelected = False
        Me.btnOK.ForeColor = System.Drawing.Color.Black
        Me.btnOK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnOK.ImageIndex = 0
        Me.btnOK.Location = New System.Drawing.Point(765, 439)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(131, 25)
        Me.btnOK.TabIndex = 201
        Me.btnOK.Text = "&Load all grid items"
        '
        'btnExport
        '
        Me.btnExport.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnExport.BorderColor = System.Drawing.Color.Black
        CBlendItems2.iColor = New System.Drawing.Color() {System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer)), System.Drawing.Color.White}
        CBlendItems2.iPoint = New Single() {0!, 1.0!}
        Me.btnExport.ColorFillBlend = CBlendItems2
        Me.btnExport.ColorFillSolid = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.btnExport.DesignerSelected = False
        Me.btnExport.ForeColor = System.Drawing.Color.Black
        Me.btnExport.ImageIndex = 0
        Me.btnExport.Location = New System.Drawing.Point(3, 439)
        Me.btnExport.Name = "btnExport"
        Me.btnExport.Size = New System.Drawing.Size(102, 25)
        Me.btnExport.TabIndex = 200
        Me.btnExport.Text = "Export to XL"
        '
        'Label1
        '
        Me.Label1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label1.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.Blue
        Me.Label1.Location = New System.Drawing.Point(411, 439)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(266, 25)
        Me.Label1.TabIndex = 203
        Me.Label1.Text = "Doubleclick on row to load single item."
        '
        'btnCloseEdit
        '
        Me.btnCloseEdit.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCloseEdit.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_CLOSE
        Me.btnCloseEdit.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.btnCloseEdit.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnCloseEdit.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnCloseEdit.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnCloseEdit.Location = New System.Drawing.Point(900, 437)
        Me.btnCloseEdit.Margin = New System.Windows.Forms.Padding(4)
        Me.btnCloseEdit.Name = "btnCloseEdit"
        Me.btnCloseEdit.Size = New System.Drawing.Size(68, 28)
        Me.btnCloseEdit.TabIndex = 204
        Me.btnCloseEdit.Text = "C&lose"
        Me.btnCloseEdit.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnCloseEdit.UseVisualStyleBackColor = False
        Me.btnCloseEdit.Visible = False
        '
        'frmUtlMnuGrpItemSel
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(233, Byte), Integer), CType(CType(237, Byte), Integer), CType(CType(241, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(973, 483)
        Me.Controls.Add(Me.btnCloseEdit)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.btnOK)
        Me.Controls.Add(Me.btnExport)
        Me.Controls.Add(Me.pnlDtl)
        Me.Controls.Add(Me.vsfSearch)
        Me.Controls.Add(Me.lblTotRec)
        Me.Controls.Add(Me.vsfDet)
        Me.MinimumSize = New System.Drawing.Size(989, 521)
        Me.Name = "frmUtlMnuGrpItemSel"
        Me.ShowIcon = False
        Me.Text = "View"
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.vsfSearch, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlDtl.ResumeLayout(False)
        CType(Me.vsfInvDet, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents vsfDet As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents lblTotRec As System.Windows.Forms.Label
    Friend WithEvents vsfSearch As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents pnlDtl As System.Windows.Forms.Panel
    Friend WithEvents vsfInvDet As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents btnExport As vLQMS.CButton
    Friend WithEvents btnOK As vLQMS.CButton
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents btnCloseEdit As C1.Win.C1Input.C1Button
End Class
