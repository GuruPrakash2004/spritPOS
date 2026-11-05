<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmUtlRewrite
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmUtlRewrite))
        Me.vsfDet = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.btnView = New System.Windows.Forms.Button()
        Me.vsfSearch = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.pnlDet = New System.Windows.Forms.Panel()
        Me.chkFreeze2 = New System.Windows.Forms.CheckBox()
        Me.btnReplaace = New System.Windows.Forms.Button()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.btnClear = New System.Windows.Forms.Button()
        Me.btnExportDetGrid = New C1.Win.C1Input.C1SplitButton()
        Me.DropDownItem1 = New C1.Win.C1Input.DropDownItem()
        Me.BackgroundWorker1 = New System.ComponentModel.BackgroundWorker()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.lblTotItems = New System.Windows.Forms.Label()
        Me.lblCurItem = New System.Windows.Forms.Label()
        Me.lblItemStk = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.lblTotStkItems = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.lblUpdated = New System.Windows.Forms.Label()
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.vsfSearch, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlDet.SuspendLayout()
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
        Me.vsfDet.Location = New System.Drawing.Point(8, 134)
        Me.vsfDet.Margin = New System.Windows.Forms.Padding(3, 5, 3, 5)
        Me.vsfDet.Name = "vsfDet"
        Me.vsfDet.Rows.DefaultSize = 24
        Me.vsfDet.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfDet.Size = New System.Drawing.Size(777, 324)
        Me.vsfDet.StyleInfo = resources.GetString("vsfDet.StyleInfo")
        Me.vsfDet.TabIndex = 232
        Me.vsfDet.Tag = ""
        '
        'btnClose
        '
        Me.btnClose.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnClose.Image = CType(resources.GetObject("btnClose.Image"), System.Drawing.Image)
        Me.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnClose.Location = New System.Drawing.Point(709, 75)
        Me.btnClose.Margin = New System.Windows.Forms.Padding(3, 5, 3, 5)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(77, 30)
        Me.btnClose.TabIndex = 223
        Me.btnClose.Text = "Close"
        Me.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnClose.UseVisualStyleBackColor = False
        '
        'btnView
        '
        Me.btnView.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnView.Image = CType(resources.GetObject("btnView.Image"), System.Drawing.Image)
        Me.btnView.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnView.Location = New System.Drawing.Point(626, 75)
        Me.btnView.Margin = New System.Windows.Forms.Padding(3, 5, 3, 5)
        Me.btnView.Name = "btnView"
        Me.btnView.Size = New System.Drawing.Size(77, 30)
        Me.btnView.TabIndex = 222
        Me.btnView.Text = "View"
        Me.btnView.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnView.UseVisualStyleBackColor = False
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
        Me.vsfSearch.Location = New System.Drawing.Point(172, 65)
        Me.vsfSearch.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.vsfSearch.Name = "vsfSearch"
        Me.vsfSearch.Rows.Count = 1
        Me.vsfSearch.Rows.DefaultSize = 24
        Me.vsfSearch.Rows.Fixed = 0
        Me.vsfSearch.ScrollBars = System.Windows.Forms.ScrollBars.None
        Me.vsfSearch.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfSearch.ShowCellLabels = True
        Me.vsfSearch.Size = New System.Drawing.Size(581, 27)
        Me.vsfSearch.StyleInfo = resources.GetString("vsfSearch.StyleInfo")
        Me.vsfSearch.TabIndex = 239
        Me.vsfSearch.Tag = ""
        Me.vsfSearch.VisualStyle = C1.Win.C1FlexGrid.VisualStyle.System
        '
        'pnlDet
        '
        Me.pnlDet.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnlDet.BackColor = System.Drawing.Color.Linen
        Me.pnlDet.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pnlDet.Controls.Add(Me.chkFreeze2)
        Me.pnlDet.Controls.Add(Me.btnReplaace)
        Me.pnlDet.Controls.Add(Me.btnCancel)
        Me.pnlDet.Controls.Add(Me.btnSave)
        Me.pnlDet.Controls.Add(Me.vsfSearch)
        Me.pnlDet.Location = New System.Drawing.Point(8, 208)
        Me.pnlDet.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.pnlDet.Name = "pnlDet"
        Me.pnlDet.Size = New System.Drawing.Size(778, 250)
        Me.pnlDet.TabIndex = 240
        Me.pnlDet.Visible = False
        '
        'chkFreeze2
        '
        Me.chkFreeze2.AutoSize = True
        Me.chkFreeze2.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkFreeze2.Location = New System.Drawing.Point(5, 4)
        Me.chkFreeze2.Margin = New System.Windows.Forms.Padding(5)
        Me.chkFreeze2.Name = "chkFreeze2"
        Me.chkFreeze2.Size = New System.Drawing.Size(65, 21)
        Me.chkFreeze2.TabIndex = 245
        Me.chkFreeze2.Text = "Freeze"
        Me.chkFreeze2.UseVisualStyleBackColor = True
        Me.chkFreeze2.Visible = False
        '
        'btnReplaace
        '
        Me.btnReplaace.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnReplaace.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnReplaace.Location = New System.Drawing.Point(495, 213)
        Me.btnReplaace.Margin = New System.Windows.Forms.Padding(3, 5, 3, 5)
        Me.btnReplaace.Name = "btnReplaace"
        Me.btnReplaace.Size = New System.Drawing.Size(92, 29)
        Me.btnReplaace.TabIndex = 236
        Me.btnReplaace.Text = "Replace Rest"
        Me.btnReplaace.UseVisualStyleBackColor = False
        Me.btnReplaace.Visible = False
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnCancel.Location = New System.Drawing.Point(695, 213)
        Me.btnCancel.Margin = New System.Windows.Forms.Padding(3, 5, 3, 5)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 29)
        Me.btnCancel.TabIndex = 235
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = False
        Me.btnCancel.Visible = False
        '
        'btnSave
        '
        Me.btnSave.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnSave.Location = New System.Drawing.Point(603, 213)
        Me.btnSave.Margin = New System.Windows.Forms.Padding(3, 5, 3, 5)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(76, 29)
        Me.btnSave.TabIndex = 234
        Me.btnSave.Text = "Save"
        Me.btnSave.UseVisualStyleBackColor = False
        Me.btnSave.Visible = False
        '
        'btnClear
        '
        Me.btnClear.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnClear.Location = New System.Drawing.Point(626, 5)
        Me.btnClear.Margin = New System.Windows.Forms.Padding(3, 5, 3, 5)
        Me.btnClear.Name = "btnClear"
        Me.btnClear.Size = New System.Drawing.Size(108, 29)
        Me.btnClear.TabIndex = 242
        Me.btnClear.Text = "Clear Filter"
        Me.btnClear.TextAlign = System.Drawing.ContentAlignment.TopCenter
        Me.btnClear.UseVisualStyleBackColor = False
        '
        'btnExportDetGrid
        '
        Me.btnExportDetGrid.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.btnExportDetGrid.Image = CType(resources.GetObject("btnExportDetGrid.Image"), System.Drawing.Image)
        Me.btnExportDetGrid.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnExportDetGrid.Items.Add(Me.DropDownItem1)
        Me.btnExportDetGrid.Location = New System.Drawing.Point(626, 40)
        Me.btnExportDetGrid.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnExportDetGrid.Name = "btnExportDetGrid"
        Me.btnExportDetGrid.Size = New System.Drawing.Size(159, 29)
        Me.btnExportDetGrid.TabIndex = 247
        Me.btnExportDetGrid.Text = "Export to Excel"
        Me.btnExportDetGrid.UseVisualStyleBackColor = True
        '
        'DropDownItem1
        '
        Me.DropDownItem1.Image = CType(resources.GetObject("DropDownItem1.Image"), System.Drawing.Image)
        Me.DropDownItem1.Text = "Export - CSV Format"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.ForeColor = System.Drawing.Color.Blue
        Me.Label1.Location = New System.Drawing.Point(12, 20)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(72, 17)
        Me.Label1.TabIndex = 248
        Me.Label1.Text = "Total Items"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.ForeColor = System.Drawing.Color.Blue
        Me.Label2.Location = New System.Drawing.Point(12, 78)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(131, 17)
        Me.Label2.TabIndex = 248
        Me.Label2.Text = "Current Item Running"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.ForeColor = System.Drawing.Color.Blue
        Me.Label3.Location = New System.Drawing.Point(12, 107)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(115, 17)
        Me.Label3.TabIndex = 248
        Me.Label3.Text = "Current Item Stock"
        '
        'lblTotItems
        '
        Me.lblTotItems.BackColor = System.Drawing.Color.Yellow
        Me.lblTotItems.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTotItems.ForeColor = System.Drawing.Color.Red
        Me.lblTotItems.Location = New System.Drawing.Point(179, 12)
        Me.lblTotItems.Name = "lblTotItems"
        Me.lblTotItems.Size = New System.Drawing.Size(131, 24)
        Me.lblTotItems.TabIndex = 248
        '
        'lblCurItem
        '
        Me.lblCurItem.BackColor = System.Drawing.Color.Yellow
        Me.lblCurItem.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCurItem.ForeColor = System.Drawing.Color.Red
        Me.lblCurItem.Location = New System.Drawing.Point(179, 72)
        Me.lblCurItem.Name = "lblCurItem"
        Me.lblCurItem.Size = New System.Drawing.Size(381, 24)
        Me.lblCurItem.TabIndex = 249
        '
        'lblItemStk
        '
        Me.lblItemStk.BackColor = System.Drawing.Color.Yellow
        Me.lblItemStk.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblItemStk.ForeColor = System.Drawing.Color.Red
        Me.lblItemStk.Location = New System.Drawing.Point(179, 103)
        Me.lblItemStk.Name = "lblItemStk"
        Me.lblItemStk.Size = New System.Drawing.Size(381, 24)
        Me.lblItemStk.TabIndex = 250
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.ForeColor = System.Drawing.Color.Blue
        Me.Label4.Location = New System.Drawing.Point(317, 16)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(135, 17)
        Me.Label4.TabIndex = 251
        Me.Label4.Text = "Total Item With Stock "
        '
        'lblTotStkItems
        '
        Me.lblTotStkItems.BackColor = System.Drawing.Color.Yellow
        Me.lblTotStkItems.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTotStkItems.ForeColor = System.Drawing.Color.Red
        Me.lblTotStkItems.Location = New System.Drawing.Point(458, 12)
        Me.lblTotStkItems.Name = "lblTotStkItems"
        Me.lblTotStkItems.Size = New System.Drawing.Size(102, 24)
        Me.lblTotStkItems.TabIndex = 252
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.ForeColor = System.Drawing.Color.Blue
        Me.Label5.Location = New System.Drawing.Point(12, 47)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(94, 17)
        Me.Label5.TabIndex = 248
        Me.Label5.Text = "Items Updated"
        '
        'lblUpdated
        '
        Me.lblUpdated.BackColor = System.Drawing.Color.Yellow
        Me.lblUpdated.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblUpdated.ForeColor = System.Drawing.Color.Red
        Me.lblUpdated.Location = New System.Drawing.Point(178, 42)
        Me.lblUpdated.Name = "lblUpdated"
        Me.lblUpdated.Size = New System.Drawing.Size(131, 24)
        Me.lblUpdated.TabIndex = 248
        '
        'frmUtlRewrite
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 17.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(188, Byte), Integer), CType(CType(205, Byte), Integer), CType(CType(227, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(791, 464)
        Me.Controls.Add(Me.lblTotStkItems)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.lblItemStk)
        Me.Controls.Add(Me.lblCurItem)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.lblUpdated)
        Me.Controls.Add(Me.lblTotItems)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.btnExportDetGrid)
        Me.Controls.Add(Me.btnClear)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.btnView)
        Me.Controls.Add(Me.vsfDet)
        Me.Controls.Add(Me.pnlDet)
        Me.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.KeyPreview = True
        Me.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.Name = "frmUtlRewrite"
        Me.ShowIcon = False
        Me.Text = "Rewrite"
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.vsfSearch, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlDet.ResumeLayout(False)
        Me.pnlDet.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnClose As System.Windows.Forms.Button
    Friend WithEvents btnView As System.Windows.Forms.Button
    Public WithEvents vsfDet As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents vsfSearch As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents pnlDet As System.Windows.Forms.Panel
    Friend WithEvents btnReplaace As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnSave As System.Windows.Forms.Button
    Friend WithEvents btnClear As Button
    Friend WithEvents chkFreeze2 As CheckBox
    Friend WithEvents btnExportDetGrid As C1.Win.C1Input.C1SplitButton
    Friend WithEvents DropDownItem1 As C1.Win.C1Input.DropDownItem
    Friend WithEvents BackgroundWorker1 As System.ComponentModel.BackgroundWorker
    Friend WithEvents Timer1 As Timer
    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents lblTotItems As Label
    Friend WithEvents lblCurItem As Label
    Friend WithEvents lblItemStk As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents lblTotStkItems As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents lblUpdated As Label
End Class
