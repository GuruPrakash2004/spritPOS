<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmTrnPendPopUp
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmTrnPendPopUp))
        Me.vsfSearch = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.vsfView = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.btnSelFull = New System.Windows.Forms.Button()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.btnOK = New System.Windows.Forms.Button()
        Me.btnRemove = New System.Windows.Forms.Button()
        Me.btnRemoveAll = New System.Windows.Forms.Button()
        Me.vsfPend = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.chkPendQty = New System.Windows.Forms.CheckBox()
        Me.lblNoofRows = New System.Windows.Forms.Label()
        Me.txtNoofRows = New System.Windows.Forms.TextBox()
        Me.lblPart = New System.Windows.Forms.Label()
        Me.txtQtyTot = New System.Windows.Forms.TextBox()
        Me.btnExportXLVw = New C1.Win.C1Input.C1Button()
        Me.btnExportXLPend = New C1.Win.C1Input.C1Button()
        Me.lblSelItemsDoc = New System.Windows.Forms.Label()
        Me.txtNoRowsDoc = New System.Windows.Forms.TextBox()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        CType(Me.vsfSearch, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.vsfView, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.vsfPend, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'vsfSearch
        '
        Me.vsfSearch.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfSearch.ColumnInfo = "2,0,0,0,0,120,Columns:"
        Me.vsfSearch.EditOptions = C1.Win.C1FlexGrid.EditFlags.AutoSearch
        Me.vsfSearch.ExtendLastCol = True
        Me.vsfSearch.FocusRect = C1.Win.C1FlexGrid.FocusRectEnum.Heavy
        Me.vsfSearch.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.vsfSearch.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross
        Me.vsfSearch.Location = New System.Drawing.Point(2, -1)
        Me.vsfSearch.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.vsfSearch.Name = "vsfSearch"
        Me.vsfSearch.Rows.Count = 1
        Me.vsfSearch.Rows.DefaultSize = 24
        Me.vsfSearch.Rows.Fixed = 0
        Me.vsfSearch.ScrollBars = System.Windows.Forms.ScrollBars.None
        Me.vsfSearch.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfSearch.ShowCellLabels = True
        Me.vsfSearch.Size = New System.Drawing.Size(1035, 26)
        Me.vsfSearch.StyleInfo = resources.GetString("vsfSearch.StyleInfo")
        Me.vsfSearch.TabIndex = 74
        Me.vsfSearch.Tag = ""
        Me.vsfSearch.VisualStyle = C1.Win.C1FlexGrid.VisualStyle.System
        '
        'vsfView
        '
        Me.vsfView.AllowDragging = C1.Win.C1FlexGrid.AllowDraggingEnum.None
        Me.vsfView.AllowFiltering = True
        Me.vsfView.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfView.BackColor = System.Drawing.SystemColors.Info
        Me.vsfView.ColumnInfo = "1,0,0,0,0,120,Columns:"
        Me.vsfView.ExtendLastCol = True
        Me.vsfView.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.vsfView.KeyActionEnter = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcrossOut
        Me.vsfView.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcrossOut
        Me.vsfView.Location = New System.Drawing.Point(2, 23)
        Me.vsfView.Name = "vsfView"
        Me.vsfView.Rows.DefaultSize = 24
        Me.vsfView.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfView.Size = New System.Drawing.Size(1035, 111)
        Me.vsfView.StyleInfo = resources.GetString("vsfView.StyleInfo")
        Me.vsfView.TabIndex = 73
        '
        'btnSelFull
        '
        Me.btnSelFull.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnSelFull.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSelFull.Location = New System.Drawing.Point(4, 144)
        Me.btnSelFull.Name = "btnSelFull"
        Me.btnSelFull.Size = New System.Drawing.Size(143, 28)
        Me.btnSelFull.TabIndex = 76
        Me.btnSelFull.Text = "Select All Items in SO"
        Me.btnSelFull.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_CANCEL
        Me.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnCancel.Location = New System.Drawing.Point(942, 401)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(95, 26)
        Me.btnCancel.TabIndex = 77
        Me.btnCancel.Text = "&Cancel"
        Me.ToolTip1.SetToolTip(Me.btnCancel, "Cancel:Alt+C")
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'btnOK
        '
        Me.btnOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnOK.Location = New System.Drawing.Point(839, 402)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(97, 26)
        Me.btnOK.TabIndex = 78
        Me.btnOK.Text = "O&K"
        Me.ToolTip1.SetToolTip(Me.btnOK, "Ok:Alt+K")
        Me.btnOK.UseVisualStyleBackColor = True
        '
        'btnRemove
        '
        Me.btnRemove.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnRemove.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnRemove.Location = New System.Drawing.Point(4, 402)
        Me.btnRemove.Name = "btnRemove"
        Me.btnRemove.Size = New System.Drawing.Size(102, 28)
        Me.btnRemove.TabIndex = 79
        Me.btnRemove.Text = "Remove Item"
        Me.btnRemove.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnRemove.UseVisualStyleBackColor = True
        '
        'btnRemoveAll
        '
        Me.btnRemoveAll.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnRemoveAll.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnRemoveAll.Location = New System.Drawing.Point(112, 402)
        Me.btnRemoveAll.Name = "btnRemoveAll"
        Me.btnRemoveAll.Size = New System.Drawing.Size(193, 28)
        Me.btnRemoveAll.TabIndex = 80
        Me.btnRemoveAll.Text = "Remove All Item of selected"
        Me.btnRemoveAll.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnRemoveAll.UseVisualStyleBackColor = True
        '
        'vsfPend
        '
        Me.vsfPend.AllowFiltering = True
        Me.vsfPend.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfPend.BackColor = System.Drawing.SystemColors.Info
        Me.vsfPend.ColumnInfo = "1,0,0,0,0,120,Columns:"
        Me.vsfPend.ExtendLastCol = True
        Me.vsfPend.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.vsfPend.KeyActionEnter = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcrossOut
        Me.vsfPend.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcrossOut
        Me.vsfPend.Location = New System.Drawing.Point(1, 179)
        Me.vsfPend.Name = "vsfPend"
        Me.vsfPend.Rows.DefaultSize = 24
        Me.vsfPend.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfPend.Size = New System.Drawing.Size(1036, 199)
        Me.vsfPend.StyleInfo = resources.GetString("vsfPend.StyleInfo")
        Me.vsfPend.TabIndex = 81
        '
        'chkPendQty
        '
        Me.chkPendQty.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.chkPendQty.AutoSize = True
        Me.chkPendQty.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkPendQty.Location = New System.Drawing.Point(879, 380)
        Me.chkPendQty.Name = "chkPendQty"
        Me.chkPendQty.Size = New System.Drawing.Size(158, 19)
        Me.chkPendQty.TabIndex = 82
        Me.chkPendQty.Text = "Take PendQty as DocQty"
        Me.chkPendQty.UseVisualStyleBackColor = True
        '
        'lblNoofRows
        '
        Me.lblNoofRows.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblNoofRows.AutoSize = True
        Me.lblNoofRows.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblNoofRows.ForeColor = System.Drawing.Color.Blue
        Me.lblNoofRows.Location = New System.Drawing.Point(356, 384)
        Me.lblNoofRows.Name = "lblNoofRows"
        Me.lblNoofRows.Size = New System.Drawing.Size(123, 15)
        Me.lblNoofRows.TabIndex = 83
        Me.lblNoofRows.Text = "No. of Items Selected"
        '
        'txtNoofRows
        '
        Me.txtNoofRows.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.txtNoofRows.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtNoofRows.Font = New System.Drawing.Font("Microsoft Sans Serif", 13.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtNoofRows.Location = New System.Drawing.Point(351, 402)
        Me.txtNoofRows.Name = "txtNoofRows"
        Me.txtNoofRows.ReadOnly = True
        Me.txtNoofRows.Size = New System.Drawing.Size(133, 27)
        Me.txtNoofRows.TabIndex = 84
        Me.txtNoofRows.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'lblPart
        '
        Me.lblPart.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblPart.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPart.ForeColor = System.Drawing.Color.Blue
        Me.lblPart.Location = New System.Drawing.Point(492, 384)
        Me.lblPart.Name = "lblPart"
        Me.lblPart.Size = New System.Drawing.Size(286, 15)
        Me.lblPart.TabIndex = 88
        Me.lblPart.Text = "Qty Total for"
        Me.lblPart.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtQtyTot
        '
        Me.txtQtyTot.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.txtQtyTot.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.txtQtyTot.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtQtyTot.Font = New System.Drawing.Font("Microsoft Sans Serif", 13.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtQtyTot.Location = New System.Drawing.Point(569, 402)
        Me.txtQtyTot.Name = "txtQtyTot"
        Me.txtQtyTot.ReadOnly = True
        Me.txtQtyTot.Size = New System.Drawing.Size(133, 27)
        Me.txtQtyTot.TabIndex = 87
        Me.txtQtyTot.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'btnExportXLVw
        '
        Me.btnExportXLVw.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnExportXLVw.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnExportXLVw.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.btnExportXLVw.Image = CType(resources.GetObject("btnExportXLVw.Image"), System.Drawing.Image)
        Me.btnExportXLVw.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnExportXLVw.Location = New System.Drawing.Point(153, 144)
        Me.btnExportXLVw.Name = "btnExportXLVw"
        Me.btnExportXLVw.Size = New System.Drawing.Size(33, 28)
        Me.btnExportXLVw.TabIndex = 206
        Me.btnExportXLVw.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnExportXLVw, "Export  to Excel : Alt + E")
        Me.btnExportXLVw.UseVisualStyleBackColor = False
        '
        'btnExportXLPend
        '
        Me.btnExportXLPend.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnExportXLPend.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnExportXLPend.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.btnExportXLPend.Image = CType(resources.GetObject("btnExportXLPend.Image"), System.Drawing.Image)
        Me.btnExportXLPend.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnExportXLPend.Location = New System.Drawing.Point(312, 402)
        Me.btnExportXLPend.Name = "btnExportXLPend"
        Me.btnExportXLPend.Size = New System.Drawing.Size(33, 28)
        Me.btnExportXLPend.TabIndex = 207
        Me.btnExportXLPend.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnExportXLPend, "Export  to Excel : Alt + X")
        Me.btnExportXLPend.UseVisualStyleBackColor = False
        '
        'lblSelItemsDoc
        '
        Me.lblSelItemsDoc.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblSelItemsDoc.AutoSize = True
        Me.lblSelItemsDoc.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSelItemsDoc.ForeColor = System.Drawing.Color.Blue
        Me.lblSelItemsDoc.Location = New System.Drawing.Point(205, 150)
        Me.lblSelItemsDoc.Name = "lblSelItemsDoc"
        Me.lblSelItemsDoc.Size = New System.Drawing.Size(163, 15)
        Me.lblSelItemsDoc.TabIndex = 208
        Me.lblSelItemsDoc.Text = "No. of Items  for Selected PO"
        '
        'txtNoRowsDoc
        '
        Me.txtNoRowsDoc.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.txtNoRowsDoc.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtNoRowsDoc.Font = New System.Drawing.Font("Microsoft Sans Serif", 13.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtNoRowsDoc.Location = New System.Drawing.Point(374, 145)
        Me.txtNoRowsDoc.Name = "txtNoRowsDoc"
        Me.txtNoRowsDoc.ReadOnly = True
        Me.txtNoRowsDoc.Size = New System.Drawing.Size(52, 27)
        Me.txtNoRowsDoc.TabIndex = 84
        Me.txtNoRowsDoc.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'frmTrnPendPopUp
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(188, Byte), Integer), CType(CType(205, Byte), Integer), CType(CType(227, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(1039, 430)
        Me.Controls.Add(Me.lblSelItemsDoc)
        Me.Controls.Add(Me.btnExportXLPend)
        Me.Controls.Add(Me.btnExportXLVw)
        Me.Controls.Add(Me.lblPart)
        Me.Controls.Add(Me.txtQtyTot)
        Me.Controls.Add(Me.txtNoRowsDoc)
        Me.Controls.Add(Me.txtNoofRows)
        Me.Controls.Add(Me.lblNoofRows)
        Me.Controls.Add(Me.chkPendQty)
        Me.Controls.Add(Me.vsfPend)
        Me.Controls.Add(Me.btnRemoveAll)
        Me.Controls.Add(Me.btnRemove)
        Me.Controls.Add(Me.btnOK)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnSelFull)
        Me.Controls.Add(Me.vsfSearch)
        Me.Controls.Add(Me.vsfView)
        Me.KeyPreview = True
        Me.MinimumSize = New System.Drawing.Size(1041, 468)
        Me.Name = "frmTrnPendPopUp"
        Me.ShowIcon = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Tag = "Pending Service Orders"
        Me.Text = "Pending Service Orders"
        CType(Me.vsfSearch, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.vsfView, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.vsfPend, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents vsfSearch As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents vsfView As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents btnSelFull As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents btnRemove As System.Windows.Forms.Button
    Friend WithEvents btnRemoveAll As System.Windows.Forms.Button
    Friend WithEvents vsfPend As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents chkPendQty As System.Windows.Forms.CheckBox
    Friend WithEvents lblNoofRows As System.Windows.Forms.Label
    Friend WithEvents txtNoofRows As System.Windows.Forms.TextBox
    Friend WithEvents lblPart As Label
    Friend WithEvents txtQtyTot As TextBox
    Friend WithEvents btnExportXLVw As C1.Win.C1Input.C1Button
    Friend WithEvents btnExportXLPend As C1.Win.C1Input.C1Button
    Friend WithEvents lblSelItemsDoc As Label
    Friend WithEvents txtNoRowsDoc As TextBox
    Friend WithEvents ToolTip1 As ToolTip
End Class
