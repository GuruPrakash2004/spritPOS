<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmRptSPurPkgwise
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmRptSPurPkgwise))
        Me.btnClose = New System.Windows.Forms.Button()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.btnPrint = New System.Windows.Forms.Button()
        Me.dtpFrom = New System.Windows.Forms.DateTimePicker()
        Me.dtpTo = New System.Windows.Forms.DateTimePicker()
        Me.lblFrom = New System.Windows.Forms.Label()
        Me.lblTo = New System.Windows.Forms.Label()
        Me.gbDateRange = New System.Windows.Forms.GroupBox()
        Me.vsfView = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.chkItemGroup = New System.Windows.Forms.CheckBox()
        Me.gbDateRange.SuspendLayout()
        CType(Me.vsfView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'btnClose
        '
        Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClose.BackColor = System.Drawing.Color.FromArgb(CType(CType(233, Byte), Integer), CType(CType(237, Byte), Integer), CType(CType(241, Byte), Integer))
        Me.btnClose.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_CLOSE
        Me.btnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnClose.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!)
        Me.btnClose.Location = New System.Drawing.Point(323, 74)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(82, 29)
        Me.btnClose.TabIndex = 21
        Me.btnClose.Text = "Cl&ose"
        Me.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnClose, "Close : Alt + O")
        Me.btnClose.UseVisualStyleBackColor = False
        '
        'btnPrint
        '
        Me.btnPrint.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnPrint.BackColor = System.Drawing.Color.FromArgb(CType(CType(233, Byte), Integer), CType(CType(237, Byte), Integer), CType(CType(241, Byte), Integer))
        Me.btnPrint.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnPrint.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.btnPrint.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnPrint.Location = New System.Drawing.Point(242, 74)
        Me.btnPrint.Name = "btnPrint"
        Me.btnPrint.Size = New System.Drawing.Size(75, 29)
        Me.btnPrint.TabIndex = 281
        Me.btnPrint.Text = "&Print"
        Me.ToolTip1.SetToolTip(Me.btnPrint, "Print : Alt + P")
        Me.btnPrint.UseVisualStyleBackColor = False
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
        'gbDateRange
        '
        Me.gbDateRange.Controls.Add(Me.lblTo)
        Me.gbDateRange.Controls.Add(Me.lblFrom)
        Me.gbDateRange.Controls.Add(Me.dtpTo)
        Me.gbDateRange.Controls.Add(Me.dtpFrom)
        Me.gbDateRange.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!)
        Me.gbDateRange.Location = New System.Drawing.Point(12, 12)
        Me.gbDateRange.Name = "gbDateRange"
        Me.gbDateRange.Size = New System.Drawing.Size(394, 49)
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
        Me.vsfView.Location = New System.Drawing.Point(0, 0)
        Me.vsfView.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.vsfView.Name = "vsfView"
        Me.vsfView.Rows.DefaultSize = 24
        Me.vsfView.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfView.Size = New System.Drawing.Size(0, 0)
        Me.vsfView.StyleInfo = resources.GetString("vsfView.StyleInfo")
        Me.vsfView.TabIndex = 260
        Me.vsfView.Tag = ""
        Me.vsfView.Visible = False
        '
        'chkItemGroup
        '
        Me.chkItemGroup.AutoSize = True
        Me.chkItemGroup.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkItemGroup.Location = New System.Drawing.Point(13, 68)
        Me.chkItemGroup.Margin = New System.Windows.Forms.Padding(4)
        Me.chkItemGroup.Name = "chkItemGroup"
        Me.chkItemGroup.Size = New System.Drawing.Size(136, 20)
        Me.chkItemGroup.TabIndex = 267
        Me.chkItemGroup.Text = "Select Item Group "
        Me.chkItemGroup.UseVisualStyleBackColor = True
        '
        'frmRptSPurPkgwise
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(233, Byte), Integer), CType(CType(237, Byte), Integer), CType(CType(241, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(417, 115)
        Me.Controls.Add(Me.btnPrint)
        Me.Controls.Add(Me.chkItemGroup)
        Me.Controls.Add(Me.vsfView)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.gbDateRange)
        Me.Name = "frmRptSPurPkgwise"
        Me.ShowIcon = False
        Me.Text = "Purchase Report - Package Wise"
        Me.gbDateRange.ResumeLayout(False)
        Me.gbDateRange.PerformLayout()
        CType(Me.vsfView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnClose As System.Windows.Forms.Button
    Friend WithEvents ToolTip1 As ToolTip
    Friend WithEvents dtpFrom As DateTimePicker
    Friend WithEvents dtpTo As DateTimePicker
    Friend WithEvents lblFrom As Label
    Friend WithEvents lblTo As Label
    Friend WithEvents gbDateRange As GroupBox
    Public WithEvents vsfView As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents chkItemGroup As CheckBox
    Friend WithEvents btnPrint As Button
End Class
