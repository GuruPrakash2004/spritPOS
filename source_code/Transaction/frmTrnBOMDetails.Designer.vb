<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmTrnBOMDetails
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmTrnBOMDetails))
        Me.vsfDet = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.txtBOMDetails = New System.Windows.Forms.TextBox()
        Me.lblBOMQty = New System.Windows.Forms.Label()
        Me.lblBOMDetails = New System.Windows.Forms.Label()
        Me.txtBOMQty = New System.Windows.Forms.TextBox()
        Me.btnLoad = New C1.Win.C1Input.C1Button()
        Me.btnCancel = New C1.Win.C1Input.C1Button()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.btnClose = New C1.Win.C1Input.C1Button()
        Me.btnExport = New C1.Win.C1Input.C1Button()
        Me.btnNext = New C1.Win.C1Input.C1Button()
        Me.btnSel = New C1.Win.C1Input.C1Button()
        Me.vsfsearch = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.txtID = New System.Windows.Forms.TextBox()
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.vsfsearch, System.ComponentModel.ISupportInitialize).BeginInit()
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
        Me.vsfDet.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveDown
        Me.vsfDet.Location = New System.Drawing.Point(12, 95)
        Me.vsfDet.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.vsfDet.Name = "vsfDet"
        Me.vsfDet.Rows.Count = 2
        Me.vsfDet.Rows.DefaultSize = 24
        Me.vsfDet.Rows.GlyphRow = 0
        Me.vsfDet.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfDet.Size = New System.Drawing.Size(776, 304)
        Me.vsfDet.StyleInfo = resources.GetString("vsfDet.StyleInfo")
        Me.vsfDet.TabIndex = 265
        Me.vsfDet.Tag = ""
        '
        'txtBOMDetails
        '
        Me.txtBOMDetails.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtBOMDetails.Location = New System.Drawing.Point(156, 12)
        Me.txtBOMDetails.Name = "txtBOMDetails"
        Me.txtBOMDetails.Size = New System.Drawing.Size(436, 26)
        Me.txtBOMDetails.TabIndex = 262
        '
        'lblBOMQty
        '
        Me.lblBOMQty.AutoSize = True
        Me.lblBOMQty.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!)
        Me.lblBOMQty.ForeColor = System.Drawing.Color.Red
        Me.lblBOMQty.Location = New System.Drawing.Point(13, 65)
        Me.lblBOMQty.Name = "lblBOMQty"
        Me.lblBOMQty.Size = New System.Drawing.Size(73, 20)
        Me.lblBOMQty.TabIndex = 264
        Me.lblBOMQty.Tag = "M"
        Me.lblBOMQty.Text = "BOM Qty"
        '
        'lblBOMDetails
        '
        Me.lblBOMDetails.AutoSize = True
        Me.lblBOMDetails.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!)
        Me.lblBOMDetails.ForeColor = System.Drawing.Color.Red
        Me.lblBOMDetails.Location = New System.Drawing.Point(13, 15)
        Me.lblBOMDetails.Name = "lblBOMDetails"
        Me.lblBOMDetails.Size = New System.Drawing.Size(98, 20)
        Me.lblBOMDetails.TabIndex = 263
        Me.lblBOMDetails.Tag = "M"
        Me.lblBOMDetails.Text = "BOM Details"
        '
        'txtBOMQty
        '
        Me.txtBOMQty.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtBOMQty.Location = New System.Drawing.Point(156, 62)
        Me.txtBOMQty.MaxLength = 10
        Me.txtBOMQty.Name = "txtBOMQty"
        Me.txtBOMQty.Size = New System.Drawing.Size(100, 26)
        Me.txtBOMQty.TabIndex = 266
        '
        'btnLoad
        '
        Me.btnLoad.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnLoad.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnLoad.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnLoad.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnLoad.Location = New System.Drawing.Point(263, 60)
        Me.btnLoad.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.btnLoad.Name = "btnLoad"
        Me.btnLoad.Size = New System.Drawing.Size(90, 30)
        Me.btnLoad.TabIndex = 267
        Me.btnLoad.Text = "&Load"
        Me.ToolTip1.SetToolTip(Me.btnLoad, "Load : Alt + L")
        Me.btnLoad.UseVisualStyleBackColor = False
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_CANCEL
        Me.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnCancel.Location = New System.Drawing.Point(600, 407)
        Me.btnCancel.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(90, 30)
        Me.btnCancel.TabIndex = 268
        Me.btnCancel.Text = "&Cancel"
        Me.btnCancel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnCancel, "Cancel : Alt + C")
        Me.btnCancel.UseVisualStyleBackColor = False
        '
        'btnClose
        '
        Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClose.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_CLOSE
        Me.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnClose.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnClose.Location = New System.Drawing.Point(698, 407)
        Me.btnClose.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(90, 30)
        Me.btnClose.TabIndex = 270
        Me.btnClose.Text = "Cl&ose"
        Me.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnClose, "Close : Alt + O")
        Me.btnClose.UseVisualStyleBackColor = False
        '
        'btnExport
        '
        Me.btnExport.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnExport.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnExport.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.btnExport.Image = CType(resources.GetObject("btnExport.Image"), System.Drawing.Image)
        Me.btnExport.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnExport.Location = New System.Drawing.Point(559, 406)
        Me.btnExport.Name = "btnExport"
        Me.btnExport.Size = New System.Drawing.Size(33, 31)
        Me.btnExport.TabIndex = 271
        Me.btnExport.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnExport, "ExportToExcel : Alt + E")
        Me.btnExport.UseVisualStyleBackColor = False
        '
        'btnNext
        '
        Me.btnNext.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnNext.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnNext.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnNext.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnNext.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnNext.Location = New System.Drawing.Point(399, 406)
        Me.btnNext.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.btnNext.Name = "btnNext"
        Me.btnNext.Size = New System.Drawing.Size(150, 30)
        Me.btnNext.TabIndex = 274
        Me.btnNext.Text = "&Load BOM Items"
        Me.ToolTip1.SetToolTip(Me.btnNext, "Next")
        Me.btnNext.UseVisualStyleBackColor = False
        '
        'btnSel
        '
        Me.btnSel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnSel.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSel.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnSel.Location = New System.Drawing.Point(599, 10)
        Me.btnSel.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.btnSel.Name = "btnSel"
        Me.btnSel.Size = New System.Drawing.Size(35, 30)
        Me.btnSel.TabIndex = 269
        Me.btnSel.Text = "..."
        Me.btnSel.UseVisualStyleBackColor = False
        '
        'vsfsearch
        '
        Me.vsfsearch.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfsearch.ColumnInfo = "10,0,0,0,0,120,Columns:"
        Me.vsfsearch.ExtendLastCol = True
        Me.vsfsearch.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.vsfsearch.Location = New System.Drawing.Point(156, 45)
        Me.vsfsearch.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.vsfsearch.Name = "vsfsearch"
        Me.vsfsearch.Rows.DefaultSize = 24
        Me.vsfsearch.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.RowRange
        Me.vsfsearch.Size = New System.Drawing.Size(436, 140)
        Me.vsfsearch.TabIndex = 272
        Me.vsfsearch.Visible = False
        '
        'txtID
        '
        Me.txtID.Location = New System.Drawing.Point(767, 12)
        Me.txtID.Name = "txtID"
        Me.txtID.Size = New System.Drawing.Size(21, 20)
        Me.txtID.TabIndex = 273
        Me.txtID.Visible = False
        '
        'frmTrnBOMDetails
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(800, 450)
        Me.Controls.Add(Me.btnNext)
        Me.Controls.Add(Me.txtID)
        Me.Controls.Add(Me.vsfsearch)
        Me.Controls.Add(Me.btnExport)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.btnSel)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.txtBOMQty)
        Me.Controls.Add(Me.btnLoad)
        Me.Controls.Add(Me.vsfDet)
        Me.Controls.Add(Me.txtBOMDetails)
        Me.Controls.Add(Me.lblBOMQty)
        Me.Controls.Add(Me.lblBOMDetails)
        Me.Name = "frmTrnBOMDetails"
        Me.ShowIcon = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "BOM Details"
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.vsfsearch, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents vsfDet As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents txtBOMDetails As TextBox
    Friend WithEvents lblBOMQty As Label
    Friend WithEvents lblBOMDetails As Label
    Friend WithEvents txtBOMQty As TextBox
    Friend WithEvents btnLoad As C1.Win.C1Input.C1Button
    Friend WithEvents ToolTip1 As ToolTip
    Friend WithEvents btnCancel As C1.Win.C1Input.C1Button
    Friend WithEvents btnSel As C1.Win.C1Input.C1Button
    Friend WithEvents btnClose As C1.Win.C1Input.C1Button
    Friend WithEvents btnExport As C1.Win.C1Input.C1Button
    Friend WithEvents vsfsearch As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents txtID As TextBox
    Friend WithEvents btnNext As C1.Win.C1Input.C1Button
End Class
