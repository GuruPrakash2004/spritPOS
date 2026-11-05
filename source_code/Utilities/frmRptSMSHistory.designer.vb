<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmRptSMSHistory
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmRptSMSHistory))
        Me.vsfView = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.btnExportToXL = New System.Windows.Forms.Button()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.btnView = New System.Windows.Forms.Button()
        Me.chkZeroBal = New System.Windows.Forms.CheckBox()
        Me.chkZeroValue = New System.Windows.Forms.CheckBox()
        Me.gbDateRange = New System.Windows.Forms.GroupBox()
        Me.lblTo = New System.Windows.Forms.Label()
        Me.lblFrom = New System.Windows.Forms.Label()
        Me.dtpTo = New System.Windows.Forms.DateTimePicker()
        Me.dtpFrom = New System.Windows.Forms.DateTimePicker()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.txtValBelow = New System.Windows.Forms.TextBox()
        Me.chkDate = New System.Windows.Forms.CheckBox()
        Me.ComboBox1 = New System.Windows.Forms.ComboBox()
        Me.Label1 = New System.Windows.Forms.Label()
        CType(Me.vsfView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.gbDateRange.SuspendLayout()
        Me.SuspendLayout()
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
        Me.vsfView.Location = New System.Drawing.Point(3, 102)
        Me.vsfView.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.vsfView.Name = "vsfView"
        Me.vsfView.Rows.DefaultSize = 24
        Me.vsfView.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfView.Size = New System.Drawing.Size(1040, 397)
        Me.vsfView.StyleInfo = resources.GetString("vsfView.StyleInfo")
        Me.vsfView.TabIndex = 250
        Me.vsfView.Tag = ""
        '
        'btnExportToXL
        '
        Me.btnExportToXL.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnExportToXL.BackColor = System.Drawing.Color.FromArgb(CType(CType(233, Byte), Integer), CType(CType(237, Byte), Integer), CType(CType(241, Byte), Integer))
        Me.btnExportToXL.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnExportToXL.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!)
        Me.btnExportToXL.Image = CType(resources.GetObject("btnExportToXL.Image"), System.Drawing.Image)
        Me.btnExportToXL.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnExportToXL.Location = New System.Drawing.Point(10, 525)
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
        Me.btnClose.BackgroundImage = Global.vFactIC.My.Resources.Resources.T3_CLOSE
        Me.btnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnClose.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!)
        Me.btnClose.Location = New System.Drawing.Point(964, 525)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(70, 29)
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
        Me.btnView.Location = New System.Drawing.Point(968, 67)
        Me.btnView.Name = "btnView"
        Me.btnView.Size = New System.Drawing.Size(75, 28)
        Me.btnView.TabIndex = 20
        Me.btnView.Text = "&View"
        Me.btnView.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnView, "View : Alt + V")
        Me.btnView.UseVisualStyleBackColor = False
        '
        'chkZeroBal
        '
        Me.chkZeroBal.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.chkZeroBal.AutoSize = True
        Me.chkZeroBal.Checked = True
        Me.chkZeroBal.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkZeroBal.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!)
        Me.chkZeroBal.Location = New System.Drawing.Point(74, 506)
        Me.chkZeroBal.Margin = New System.Windows.Forms.Padding(4)
        Me.chkZeroBal.Name = "chkZeroBal"
        Me.chkZeroBal.Size = New System.Drawing.Size(256, 20)
        Me.chkZeroBal.TabIndex = 255
        Me.chkZeroBal.Text = " Do Not Show Items With Zero Balance"
        Me.chkZeroBal.UseVisualStyleBackColor = True
        '
        'chkZeroValue
        '
        Me.chkZeroValue.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.chkZeroValue.AutoSize = True
        Me.chkZeroValue.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!)
        Me.chkZeroValue.Location = New System.Drawing.Point(74, 535)
        Me.chkZeroValue.Margin = New System.Windows.Forms.Padding(4)
        Me.chkZeroValue.Name = "chkZeroValue"
        Me.chkZeroValue.Size = New System.Drawing.Size(272, 20)
        Me.chkZeroValue.TabIndex = 256
        Me.chkZeroValue.Text = "Do Not Show Items With Closing Value <="
        Me.chkZeroValue.UseVisualStyleBackColor = True
        '
        'gbDateRange
        '
        Me.gbDateRange.Controls.Add(Me.chkDate)
        Me.gbDateRange.Controls.Add(Me.lblTo)
        Me.gbDateRange.Controls.Add(Me.lblFrom)
        Me.gbDateRange.Controls.Add(Me.dtpTo)
        Me.gbDateRange.Controls.Add(Me.dtpFrom)
        Me.gbDateRange.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!)
        Me.gbDateRange.Location = New System.Drawing.Point(12, 15)
        Me.gbDateRange.Name = "gbDateRange"
        Me.gbDateRange.Size = New System.Drawing.Size(343, 55)
        Me.gbDateRange.TabIndex = 259
        Me.gbDateRange.TabStop = False
        Me.gbDateRange.Text = "     For The Period"
        '
        'lblTo
        '
        Me.lblTo.AutoSize = True
        Me.lblTo.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!)
        Me.lblTo.Location = New System.Drawing.Point(186, 23)
        Me.lblTo.Name = "lblTo"
        Me.lblTo.Size = New System.Drawing.Size(25, 16)
        Me.lblTo.TabIndex = 15
        Me.lblTo.Text = "To"
        '
        'lblFrom
        '
        Me.lblFrom.AutoSize = True
        Me.lblFrom.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!)
        Me.lblFrom.Location = New System.Drawing.Point(11, 23)
        Me.lblFrom.Name = "lblFrom"
        Me.lblFrom.Size = New System.Drawing.Size(39, 16)
        Me.lblFrom.TabIndex = 14
        Me.lblFrom.Text = "From"
        '
        'dtpTo
        '
        Me.dtpTo.CustomFormat = "dd/MM/yyyy"
        Me.dtpTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtpTo.Location = New System.Drawing.Point(213, 18)
        Me.dtpTo.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.dtpTo.Name = "dtpTo"
        Me.dtpTo.Size = New System.Drawing.Size(124, 22)
        Me.dtpTo.TabIndex = 13
        '
        'dtpFrom
        '
        Me.dtpFrom.CustomFormat = "dd/MM/yyyy"
        Me.dtpFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtpFrom.Location = New System.Drawing.Point(56, 19)
        Me.dtpFrom.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.dtpFrom.Name = "dtpFrom"
        Me.dtpFrom.Size = New System.Drawing.Size(124, 22)
        Me.dtpFrom.TabIndex = 12
        '
        'txtValBelow
        '
        Me.txtValBelow.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.txtValBelow.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtValBelow.Location = New System.Drawing.Point(353, 532)
        Me.txtValBelow.MaxLength = 50
        Me.txtValBelow.Name = "txtValBelow"
        Me.txtValBelow.Size = New System.Drawing.Size(65, 23)
        Me.txtValBelow.TabIndex = 260
        '
        'chkDate
        '
        Me.chkDate.AutoSize = True
        Me.chkDate.Checked = True
        Me.chkDate.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkDate.Location = New System.Drawing.Point(6, 0)
        Me.chkDate.Name = "chkDate"
        Me.chkDate.Size = New System.Drawing.Size(15, 14)
        Me.chkDate.TabIndex = 261
        Me.chkDate.UseVisualStyleBackColor = True
        '
        'ComboBox1
        '
        Me.ComboBox1.FormattingEnabled = True
        Me.ComboBox1.Location = New System.Drawing.Point(380, 35)
        Me.ComboBox1.Name = "ComboBox1"
        Me.ComboBox1.Size = New System.Drawing.Size(263, 21)
        Me.ComboBox1.TabIndex = 261
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(377, 15)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(39, 13)
        Me.Label1.TabIndex = 262
        Me.Label1.Text = "Label1"
        '
        'frmRptSMSHistory
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(233, Byte), Integer), CType(CType(237, Byte), Integer), CType(CType(241, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(1053, 579)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.ComboBox1)
        Me.Controls.Add(Me.txtValBelow)
        Me.Controls.Add(Me.gbDateRange)
        Me.Controls.Add(Me.chkZeroValue)
        Me.Controls.Add(Me.chkZeroBal)
        Me.Controls.Add(Me.vsfView)
        Me.Controls.Add(Me.btnExportToXL)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.btnView)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MinimumSize = New System.Drawing.Size(773, 473)
        Me.Name = "frmRptSMSHistory"
        Me.ShowIcon = False
        Me.Text = "SMS History"
        CType(Me.vsfView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.gbDateRange.ResumeLayout(False)
        Me.gbDateRange.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnClose As System.Windows.Forms.Button
    Friend WithEvents btnView As System.Windows.Forms.Button
    Friend WithEvents btnExportToXL As System.Windows.Forms.Button
    Public WithEvents vsfView As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents chkZeroBal As CheckBox
    Friend WithEvents chkZeroValue As CheckBox
    Friend WithEvents gbDateRange As GroupBox
    Friend WithEvents lblTo As Label
    Friend WithEvents lblFrom As Label
    Friend WithEvents dtpTo As DateTimePicker
    Friend WithEvents dtpFrom As DateTimePicker
    Friend WithEvents ToolTip1 As ToolTip
    Friend WithEvents txtValBelow As TextBox
    Friend WithEvents chkDate As CheckBox
    Friend WithEvents ComboBox1 As ComboBox
    Friend WithEvents Label1 As Label
End Class
