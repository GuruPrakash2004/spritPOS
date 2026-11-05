<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmRptProMrg
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmRptProMrg))
        Me.btnExportToXL = New System.Windows.Forms.Button()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.btnView = New System.Windows.Forms.Button()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.dtpFrom = New System.Windows.Forms.DateTimePicker()
        Me.dtpTo = New System.Windows.Forms.DateTimePicker()
        Me.lblFrom = New System.Windows.Forms.Label()
        Me.lblTo = New System.Windows.Forms.Label()
        Me.gbDateRange = New System.Windows.Forms.GroupBox()
        Me.vsfView = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.chkItem = New System.Windows.Forms.CheckBox()
        Me.chkItemGroup = New System.Windows.Forms.CheckBox()
        Me.chkFIfo = New System.Windows.Forms.CheckBox()
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
        Me.gbDateRange.SuspendLayout()
        CType(Me.vsfView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlCopyShortCut.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnExportToXL
        '
        Me.btnExportToXL.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnExportToXL.BackColor = System.Drawing.Color.FromArgb(CType(CType(233, Byte), Integer), CType(CType(237, Byte), Integer), CType(CType(241, Byte), Integer))
        Me.btnExportToXL.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnExportToXL.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!)
        Me.btnExportToXL.Image = CType(resources.GetObject("btnExportToXL.Image"), System.Drawing.Image)
        Me.btnExportToXL.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnExportToXL.Location = New System.Drawing.Point(12, 560)
        Me.btnExportToXL.Name = "btnExportToXL"
        Me.btnExportToXL.Size = New System.Drawing.Size(35, 29)
        Me.btnExportToXL.TabIndex = 65
        Me.btnExportToXL.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnExportToXL, "Export To Excel : Alt + E")
        Me.btnExportToXL.UseVisualStyleBackColor = False
        '
        'btnClose
        '
        Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClose.BackColor = System.Drawing.Color.FromArgb(CType(CType(233, Byte), Integer), CType(CType(237, Byte), Integer), CType(CType(241, Byte), Integer))
        Me.btnClose.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_CLOSE
        Me.btnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnClose.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!)
        Me.btnClose.Location = New System.Drawing.Point(959, 560)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(82, 29)
        Me.btnClose.TabIndex = 21
        Me.btnClose.Text = "Cl&ose"
        Me.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnClose, "Close : Alt + O")
        Me.btnClose.UseVisualStyleBackColor = False
        '
        'btnView
        '
        Me.btnView.BackColor = System.Drawing.Color.FromArgb(CType(CType(233, Byte), Integer), CType(CType(237, Byte), Integer), CType(CType(241, Byte), Integer))
        Me.btnView.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnView.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.btnView.Image = CType(resources.GetObject("btnView.Image"), System.Drawing.Image)
        Me.btnView.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnView.Location = New System.Drawing.Point(375, 63)
        Me.btnView.Name = "btnView"
        Me.btnView.Size = New System.Drawing.Size(75, 28)
        Me.btnView.TabIndex = 20
        Me.btnView.Text = "&View"
        Me.btnView.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnView, "View : Alt + V")
        Me.btnView.UseVisualStyleBackColor = False
        '
        'dtpFrom
        '
        Me.dtpFrom.CustomFormat = "dd/MM/yyyy"
        Me.dtpFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtpFrom.Location = New System.Drawing.Point(81, 19)
        Me.dtpFrom.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.dtpFrom.Name = "dtpFrom"
        Me.dtpFrom.Size = New System.Drawing.Size(103, 22)
        Me.dtpFrom.TabIndex = 12
        '
        'dtpTo
        '
        Me.dtpTo.CustomFormat = "dd/MM/yyyy"
        Me.dtpTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtpTo.Location = New System.Drawing.Point(238, 18)
        Me.dtpTo.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.dtpTo.Name = "dtpTo"
        Me.dtpTo.Size = New System.Drawing.Size(104, 22)
        Me.dtpTo.TabIndex = 13
        '
        'lblFrom
        '
        Me.lblFrom.AutoSize = True
        Me.lblFrom.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!)
        Me.lblFrom.Location = New System.Drawing.Point(36, 23)
        Me.lblFrom.Name = "lblFrom"
        Me.lblFrom.Size = New System.Drawing.Size(39, 16)
        Me.lblFrom.TabIndex = 14
        Me.lblFrom.Text = "From"
        '
        'lblTo
        '
        Me.lblTo.AutoSize = True
        Me.lblTo.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!)
        Me.lblTo.Location = New System.Drawing.Point(211, 23)
        Me.lblTo.Name = "lblTo"
        Me.lblTo.Size = New System.Drawing.Size(25, 16)
        Me.lblTo.TabIndex = 15
        Me.lblTo.Text = "To"
        '
        'gbDateRange
        '
        Me.gbDateRange.Controls.Add(Me.lblTo)
        Me.gbDateRange.Controls.Add(Me.lblFrom)
        Me.gbDateRange.Controls.Add(Me.dtpTo)
        Me.gbDateRange.Controls.Add(Me.dtpFrom)
        Me.gbDateRange.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!)
        Me.gbDateRange.Location = New System.Drawing.Point(12, 12)
        Me.gbDateRange.Name = "gbDateRange"
        Me.gbDateRange.Size = New System.Drawing.Size(383, 49)
        Me.gbDateRange.TabIndex = 259
        Me.gbDateRange.TabStop = False
        Me.gbDateRange.Text = "For The Period"
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
        Me.vsfView.Location = New System.Drawing.Point(12, 95)
        Me.vsfView.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.vsfView.Name = "vsfView"
        Me.vsfView.Rows.DefaultSize = 24
        Me.vsfView.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfView.Size = New System.Drawing.Size(1029, 458)
        Me.vsfView.StyleInfo = resources.GetString("vsfView.StyleInfo")
        Me.vsfView.TabIndex = 260
        Me.vsfView.Tag = ""
        '
        'chkItem
        '
        Me.chkItem.AutoSize = True
        Me.chkItem.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkItem.Location = New System.Drawing.Point(155, 69)
        Me.chkItem.Margin = New System.Windows.Forms.Padding(5)
        Me.chkItem.Name = "chkItem"
        Me.chkItem.Size = New System.Drawing.Size(93, 20)
        Me.chkItem.TabIndex = 263
        Me.chkItem.Text = "Select Item"
        Me.chkItem.UseVisualStyleBackColor = True
        '
        'chkItemGroup
        '
        Me.chkItemGroup.AutoSize = True
        Me.chkItemGroup.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkItemGroup.Location = New System.Drawing.Point(12, 68)
        Me.chkItemGroup.Margin = New System.Windows.Forms.Padding(4)
        Me.chkItemGroup.Name = "chkItemGroup"
        Me.chkItemGroup.Size = New System.Drawing.Size(136, 20)
        Me.chkItemGroup.TabIndex = 266
        Me.chkItemGroup.Text = "Select Item Group "
        Me.chkItemGroup.UseVisualStyleBackColor = True
        '
        'chkFIfo
        '
        Me.chkFIfo.AutoSize = True
        Me.chkFIfo.Location = New System.Drawing.Point(250, 70)
        Me.chkFIfo.Name = "chkFIfo"
        Me.chkFIfo.Size = New System.Drawing.Size(97, 17)
        Me.chkFIfo.TabIndex = 267
        Me.chkFIfo.Text = "Based on FIFO"
        Me.chkFIfo.UseVisualStyleBackColor = True
        Me.chkFIfo.Visible = False
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
        Me.pnlCopyShortCut.Location = New System.Drawing.Point(345, 144)
        Me.pnlCopyShortCut.Name = "pnlCopyShortCut"
        Me.pnlCopyShortCut.Size = New System.Drawing.Size(329, 177)
        Me.pnlCopyShortCut.TabIndex = 310
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
        'frmRptProMrg
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(233, Byte), Integer), CType(CType(237, Byte), Integer), CType(CType(241, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(1053, 601)
        Me.Controls.Add(Me.pnlCopyShortCut)
        Me.Controls.Add(Me.chkFIfo)
        Me.Controls.Add(Me.chkItemGroup)
        Me.Controls.Add(Me.chkItem)
        Me.Controls.Add(Me.vsfView)
        Me.Controls.Add(Me.btnExportToXL)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.btnView)
        Me.Controls.Add(Me.gbDateRange)
        Me.MinimumSize = New System.Drawing.Size(773, 473)
        Me.Name = "frmRptProMrg"
        Me.ShowIcon = False
        Me.Text = "Profit Margin Report"
        Me.gbDateRange.ResumeLayout(False)
        Me.gbDateRange.PerformLayout()
        CType(Me.vsfView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlCopyShortCut.ResumeLayout(False)
        Me.pnlCopyShortCut.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnClose As System.Windows.Forms.Button
    Friend WithEvents btnView As System.Windows.Forms.Button
    Friend WithEvents btnExportToXL As System.Windows.Forms.Button
    Friend WithEvents ToolTip1 As ToolTip
    Friend WithEvents dtpFrom As DateTimePicker
    Friend WithEvents dtpTo As DateTimePicker
    Friend WithEvents lblFrom As Label
    Friend WithEvents lblTo As Label
    Friend WithEvents gbDateRange As GroupBox
    Public WithEvents vsfView As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents chkItem As CheckBox
    Friend WithEvents chkItemGroup As CheckBox
    Friend WithEvents chkFIfo As CheckBox
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
