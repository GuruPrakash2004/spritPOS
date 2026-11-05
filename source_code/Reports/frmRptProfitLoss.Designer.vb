<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmRptProfitLoss
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmRptProfitLoss))
        Me.chkAgent = New System.Windows.Forms.CheckBox()
        Me.chkItem = New System.Windows.Forms.CheckBox()
        Me.lblItem = New System.Windows.Forms.Label()
        Me.pnlDate = New System.Windows.Forms.Panel()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.dtpToDate = New System.Windows.Forms.DateTimePicker()
        Me.dtpFromDate = New System.Windows.Forms.DateTimePicker()
        Me.chkDate = New System.Windows.Forms.CheckBox()
        Me.txtItemCode = New System.Windows.Forms.TextBox()
        Me.btnExcel = New System.Windows.Forms.Button()
        Me.chkMerge = New System.Windows.Forms.CheckBox()
        Me.FlowLayoutPanel1 = New System.Windows.Forms.FlowLayoutPanel()
        Me.vsfDet = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.vsfView = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.btnView = New System.Windows.Forms.Button()
        Me.pnlDate.SuspendLayout()
        Me.FlowLayoutPanel1.SuspendLayout()
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.vsfView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'chkAgent
        '
        Me.chkAgent.AutoSize = True
        Me.chkAgent.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkAgent.Location = New System.Drawing.Point(106, 98)
        Me.chkAgent.Margin = New System.Windows.Forms.Padding(4)
        Me.chkAgent.Name = "chkAgent"
        Me.chkAgent.Size = New System.Drawing.Size(99, 21)
        Me.chkAgent.TabIndex = 225
        Me.chkAgent.Text = "Select Agent"
        Me.chkAgent.UseVisualStyleBackColor = True
        Me.chkAgent.Visible = False
        '
        'chkItem
        '
        Me.chkItem.AutoSize = True
        Me.chkItem.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkItem.Location = New System.Drawing.Point(4, 98)
        Me.chkItem.Margin = New System.Windows.Forms.Padding(4)
        Me.chkItem.Name = "chkItem"
        Me.chkItem.Size = New System.Drawing.Size(94, 21)
        Me.chkItem.TabIndex = 224
        Me.chkItem.Text = "Select Item "
        Me.chkItem.UseVisualStyleBackColor = True
        Me.chkItem.Visible = False
        '
        'lblItem
        '
        Me.lblItem.AutoSize = True
        Me.lblItem.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblItem.Location = New System.Drawing.Point(3, 63)
        Me.lblItem.Name = "lblItem"
        Me.lblItem.Size = New System.Drawing.Size(68, 17)
        Me.lblItem.TabIndex = 221
        Me.lblItem.Text = "Item Code"
        Me.lblItem.Visible = False
        '
        'pnlDate
        '
        Me.pnlDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlDate.Controls.Add(Me.Label4)
        Me.pnlDate.Controls.Add(Me.Label3)
        Me.pnlDate.Controls.Add(Me.dtpToDate)
        Me.pnlDate.Controls.Add(Me.dtpFromDate)
        Me.pnlDate.Controls.Add(Me.chkDate)
        Me.pnlDate.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pnlDate.Location = New System.Drawing.Point(3, 4)
        Me.pnlDate.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.pnlDate.Name = "pnlDate"
        Me.pnlDate.Size = New System.Drawing.Size(378, 55)
        Me.pnlDate.TabIndex = 219
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(172, 28)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(22, 17)
        Me.Label4.TabIndex = 11
        Me.Label4.Text = "To"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(3, 26)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(38, 17)
        Me.Label3.TabIndex = 10
        Me.Label3.Text = "From"
        '
        'dtpToDate
        '
        Me.dtpToDate.CustomFormat = "dd/MM/yyyy"
        Me.dtpToDate.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dtpToDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtpToDate.Location = New System.Drawing.Point(199, 22)
        Me.dtpToDate.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.dtpToDate.Name = "dtpToDate"
        Me.dtpToDate.Size = New System.Drawing.Size(112, 25)
        Me.dtpToDate.TabIndex = 4
        '
        'dtpFromDate
        '
        Me.dtpFromDate.CustomFormat = "dd/MM/yyyy"
        Me.dtpFromDate.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dtpFromDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtpFromDate.Location = New System.Drawing.Point(47, 22)
        Me.dtpFromDate.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.dtpFromDate.Name = "dtpFromDate"
        Me.dtpFromDate.Size = New System.Drawing.Size(119, 25)
        Me.dtpFromDate.TabIndex = 3
        '
        'chkDate
        '
        Me.chkDate.AutoSize = True
        Me.chkDate.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkDate.Location = New System.Drawing.Point(5, 2)
        Me.chkDate.Margin = New System.Windows.Forms.Padding(4)
        Me.chkDate.Name = "chkDate"
        Me.chkDate.Size = New System.Drawing.Size(95, 21)
        Me.chkDate.TabIndex = 228
        Me.chkDate.Text = "Date Range"
        Me.chkDate.UseVisualStyleBackColor = True
        '
        'txtItemCode
        '
        Me.txtItemCode.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtItemCode.Location = New System.Drawing.Point(77, 66)
        Me.txtItemCode.Name = "txtItemCode"
        Me.txtItemCode.Size = New System.Drawing.Size(226, 25)
        Me.txtItemCode.TabIndex = 229
        Me.txtItemCode.Visible = False
        '
        'btnExcel
        '
        Me.btnExcel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnExcel.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnExcel.Location = New System.Drawing.Point(12, 565)
        Me.btnExcel.Name = "btnExcel"
        Me.btnExcel.Size = New System.Drawing.Size(113, 27)
        Me.btnExcel.TabIndex = 233
        Me.btnExcel.Text = "Export to Excel"
        Me.btnExcel.UseVisualStyleBackColor = True
        '
        'chkMerge
        '
        Me.chkMerge.AutoSize = True
        Me.chkMerge.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkMerge.Location = New System.Drawing.Point(15, 152)
        Me.chkMerge.Margin = New System.Windows.Forms.Padding(4)
        Me.chkMerge.Name = "chkMerge"
        Me.chkMerge.Size = New System.Drawing.Size(120, 21)
        Me.chkMerge.TabIndex = 236
        Me.chkMerge.Text = "Merge Columns"
        Me.chkMerge.UseVisualStyleBackColor = True
        Me.chkMerge.Visible = False
        '
        'FlowLayoutPanel1
        '
        Me.FlowLayoutPanel1.Controls.Add(Me.pnlDate)
        Me.FlowLayoutPanel1.Controls.Add(Me.lblItem)
        Me.FlowLayoutPanel1.Controls.Add(Me.txtItemCode)
        Me.FlowLayoutPanel1.Controls.Add(Me.chkItem)
        Me.FlowLayoutPanel1.Controls.Add(Me.chkAgent)
        Me.FlowLayoutPanel1.Location = New System.Drawing.Point(12, 0)
        Me.FlowLayoutPanel1.Name = "FlowLayoutPanel1"
        Me.FlowLayoutPanel1.Size = New System.Drawing.Size(395, 134)
        Me.FlowLayoutPanel1.TabIndex = 237
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
        Me.vsfDet.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross
        Me.vsfDet.Location = New System.Drawing.Point(12, 184)
        Me.vsfDet.Name = "vsfDet"
        Me.vsfDet.Rows.DefaultSize = 24
        Me.vsfDet.Rows.Fixed = 3
        Me.vsfDet.Rows.GlyphRow = 2
        Me.vsfDet.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfDet.ShowCellLabels = True
        Me.vsfDet.Size = New System.Drawing.Size(839, 372)
        Me.vsfDet.StyleInfo = resources.GetString("vsfDet.StyleInfo")
        Me.vsfDet.TabIndex = 239
        Me.vsfDet.Tag = "View"
        '
        'vsfView
        '
        Me.vsfView.AllowDragging = C1.Win.C1FlexGrid.AllowDraggingEnum.None
        Me.vsfView.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfView.ColumnInfo = "10,0,0,0,0,120,Columns:"
        Me.vsfView.ExtendLastCol = True
        Me.vsfView.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.vsfView.Location = New System.Drawing.Point(76, 210)
        Me.vsfView.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.vsfView.Name = "vsfView"
        Me.vsfView.Rows.DefaultSize = 24
        Me.vsfView.Size = New System.Drawing.Size(609, 339)
        Me.vsfView.StyleInfo = resources.GetString("vsfView.StyleInfo")
        Me.vsfView.TabIndex = 240
        '
        'btnClose
        '
        Me.btnClose.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnClose.Image = CType(resources.GetObject("btnClose.Image"), System.Drawing.Image)
        Me.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnClose.Location = New System.Drawing.Point(774, 152)
        Me.btnClose.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(77, 28)
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
        Me.btnView.Location = New System.Drawing.Point(686, 153)
        Me.btnView.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnView.Name = "btnView"
        Me.btnView.Size = New System.Drawing.Size(78, 27)
        Me.btnView.TabIndex = 222
        Me.btnView.Text = "View"
        Me.btnView.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnView.UseVisualStyleBackColor = False
        '
        'frmRptProfitLoss
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Gainsboro
        Me.ClientSize = New System.Drawing.Size(857, 604)
        Me.Controls.Add(Me.vsfView)
        Me.Controls.Add(Me.vsfDet)
        Me.Controls.Add(Me.FlowLayoutPanel1)
        Me.Controls.Add(Me.chkMerge)
        Me.Controls.Add(Me.btnExcel)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.btnView)
        Me.KeyPreview = True
        Me.Name = "frmRptProfitLoss"
        Me.ShowIcon = False
        Me.Text = "Profit & Loss Report"
        Me.pnlDate.ResumeLayout(False)
        Me.pnlDate.PerformLayout()
        Me.FlowLayoutPanel1.ResumeLayout(False)
        Me.FlowLayoutPanel1.PerformLayout()
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.vsfView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents chkAgent As System.Windows.Forms.CheckBox
    Friend WithEvents chkItem As System.Windows.Forms.CheckBox
    Friend WithEvents btnClose As System.Windows.Forms.Button
    Friend WithEvents btnView As System.Windows.Forms.Button
    Friend WithEvents lblItem As System.Windows.Forms.Label
    Friend WithEvents pnlDate As System.Windows.Forms.Panel
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents dtpToDate As System.Windows.Forms.DateTimePicker
    Friend WithEvents dtpFromDate As System.Windows.Forms.DateTimePicker
    Friend WithEvents chkDate As System.Windows.Forms.CheckBox
    Friend WithEvents txtItemCode As System.Windows.Forms.TextBox
    Friend WithEvents btnExcel As System.Windows.Forms.Button
    Friend WithEvents chkMerge As System.Windows.Forms.CheckBox
    Friend WithEvents FlowLayoutPanel1 As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents vsfDet As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents vsfView As C1.Win.C1FlexGrid.C1FlexGrid
End Class
