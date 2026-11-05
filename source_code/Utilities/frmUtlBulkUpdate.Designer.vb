<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmUtlBulkUpdate
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmUtlBulkUpdate))
        Me.lblTableName = New System.Windows.Forms.Label()
        Me.cboTable = New System.Windows.Forms.ComboBox()
        Me.btnShowRec = New System.Windows.Forms.Button()
        Me.vsfDet = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.lblFields = New System.Windows.Forms.Label()
        Me.txtReplaceValue = New System.Windows.Forms.TextBox()
        Me.lblReplaceValue = New System.Windows.Forms.Label()
        Me.txtFields = New System.Windows.Forms.TextBox()
        Me.btnUpdate = New System.Windows.Forms.Button()
        Me.lblTotNoOfRec = New System.Windows.Forms.Label()
        Me.txtNoofRec = New System.Windows.Forms.TextBox()
        Me.txtCurValue = New System.Windows.Forms.TextBox()
        Me.dtpDate = New System.Windows.Forms.DateTimePicker()
        Me.btnDelete = New System.Windows.Forms.Button()
        Me.btnFreeze = New System.Windows.Forms.Button()
        Me.btnExcel = New System.Windows.Forms.Button()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.pnlExp = New System.Windows.Forms.Panel()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.GrpUpdate = New System.Windows.Forms.GroupBox()
        Me.RdoPartial = New System.Windows.Forms.RadioButton()
        Me.RdoComplete = New System.Windows.Forms.RadioButton()
        Me.txtPartial = New System.Windows.Forms.TextBox()
        Me.GrpDelete = New System.Windows.Forms.GroupBox()
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlExp.SuspendLayout()
        Me.GrpUpdate.SuspendLayout()
        Me.GrpDelete.SuspendLayout()
        Me.SuspendLayout()
        '
        'lblTableName
        '
        Me.lblTableName.AutoSize = True
        Me.lblTableName.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTableName.Location = New System.Drawing.Point(12, 12)
        Me.lblTableName.Name = "lblTableName"
        Me.lblTableName.Size = New System.Drawing.Size(75, 15)
        Me.lblTableName.TabIndex = 0
        Me.lblTableName.Text = "Table Name"
        '
        'cboTable
        '
        Me.cboTable.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboTable.FormattingEnabled = True
        Me.cboTable.Location = New System.Drawing.Point(88, 7)
        Me.cboTable.Name = "cboTable"
        Me.cboTable.Size = New System.Drawing.Size(198, 23)
        Me.cboTable.TabIndex = 0
        '
        'btnShowRec
        '
        Me.btnShowRec.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnShowRec.Location = New System.Drawing.Point(295, 6)
        Me.btnShowRec.Name = "btnShowRec"
        Me.btnShowRec.Size = New System.Drawing.Size(103, 25)
        Me.btnShowRec.TabIndex = 1
        Me.btnShowRec.Text = "&Show Record"
        Me.ToolTip1.SetToolTip(Me.btnShowRec, "Show Record : Alt + S")
        Me.btnShowRec.UseVisualStyleBackColor = True
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
        Me.vsfDet.KeyActionEnter = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross
        Me.vsfDet.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross
        Me.vsfDet.Location = New System.Drawing.Point(8, 37)
        Me.vsfDet.Name = "vsfDet"
        Me.vsfDet.Rows.Count = 10
        Me.vsfDet.Rows.DefaultSize = 24
        Me.vsfDet.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfDet.Size = New System.Drawing.Size(1074, 448)
        Me.vsfDet.StyleInfo = resources.GetString("vsfDet.StyleInfo")
        Me.vsfDet.TabIndex = 2
        Me.vsfDet.VisualStyle = C1.Win.C1FlexGrid.VisualStyle.Office2007Blue
        '
        'lblFields
        '
        Me.lblFields.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblFields.AutoSize = True
        Me.lblFields.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFields.Location = New System.Drawing.Point(68, 56)
        Me.lblFields.Name = "lblFields"
        Me.lblFields.Size = New System.Drawing.Size(40, 15)
        Me.lblFields.TabIndex = 23
        Me.lblFields.Tag = "R"
        Me.lblFields.Text = "Fields"
        '
        'txtReplaceValue
        '
        Me.txtReplaceValue.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.txtReplaceValue.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtReplaceValue.Location = New System.Drawing.Point(126, 80)
        Me.txtReplaceValue.Name = "txtReplaceValue"
        Me.txtReplaceValue.Size = New System.Drawing.Size(197, 21)
        Me.txtReplaceValue.TabIndex = 3
        '
        'lblReplaceValue
        '
        Me.lblReplaceValue.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblReplaceValue.AutoSize = True
        Me.lblReplaceValue.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblReplaceValue.Location = New System.Drawing.Point(3, 82)
        Me.lblReplaceValue.Name = "lblReplaceValue"
        Me.lblReplaceValue.Size = New System.Drawing.Size(114, 15)
        Me.lblReplaceValue.TabIndex = 25
        Me.lblReplaceValue.Text = "Replace With Value"
        '
        'txtFields
        '
        Me.txtFields.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.txtFields.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtFields.Location = New System.Drawing.Point(126, 53)
        Me.txtFields.Name = "txtFields"
        Me.txtFields.ReadOnly = True
        Me.txtFields.Size = New System.Drawing.Size(197, 21)
        Me.txtFields.TabIndex = 1
        '
        'btnUpdate
        '
        Me.btnUpdate.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnUpdate.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnUpdate.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnUpdate.Location = New System.Drawing.Point(329, 78)
        Me.btnUpdate.Name = "btnUpdate"
        Me.btnUpdate.Size = New System.Drawing.Size(90, 28)
        Me.btnUpdate.TabIndex = 4
        Me.btnUpdate.Text = "&Update"
        Me.ToolTip1.SetToolTip(Me.btnUpdate, "Update : Alt + U")
        Me.btnUpdate.UseVisualStyleBackColor = True
        '
        'lblTotNoOfRec
        '
        Me.lblTotNoOfRec.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblTotNoOfRec.AutoSize = True
        Me.lblTotNoOfRec.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTotNoOfRec.ForeColor = System.Drawing.Color.Blue
        Me.lblTotNoOfRec.Location = New System.Drawing.Point(255, 11)
        Me.lblTotNoOfRec.Name = "lblTotNoOfRec"
        Me.lblTotNoOfRec.Size = New System.Drawing.Size(212, 17)
        Me.lblTotNoOfRec.TabIndex = 292
        Me.lblTotNoOfRec.Tag = "R"
        Me.lblTotNoOfRec.Text = "Total Number of Records Loaded :"
        '
        'txtNoofRec
        '
        Me.txtNoofRec.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.txtNoofRec.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtNoofRec.Location = New System.Drawing.Point(473, 8)
        Me.txtNoofRec.Name = "txtNoofRec"
        Me.txtNoofRec.ReadOnly = True
        Me.txtNoofRec.Size = New System.Drawing.Size(92, 22)
        Me.txtNoofRec.TabIndex = 2
        Me.txtNoofRec.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtCurValue
        '
        Me.txtCurValue.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.txtCurValue.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCurValue.Location = New System.Drawing.Point(329, 52)
        Me.txtCurValue.Name = "txtCurValue"
        Me.txtCurValue.ReadOnly = True
        Me.txtCurValue.Size = New System.Drawing.Size(197, 21)
        Me.txtCurValue.TabIndex = 294
        '
        'dtpDate
        '
        Me.dtpDate.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.dtpDate.CustomFormat = "dd/MM/yyyy"
        Me.dtpDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtpDate.Location = New System.Drawing.Point(126, 81)
        Me.dtpDate.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.dtpDate.Name = "dtpDate"
        Me.dtpDate.Size = New System.Drawing.Size(124, 20)
        Me.dtpDate.TabIndex = 3
        Me.dtpDate.Visible = False
        '
        'btnDelete
        '
        Me.btnDelete.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnDelete.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnDelete.Location = New System.Drawing.Point(116, 12)
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.Size = New System.Drawing.Size(90, 28)
        Me.btnDelete.TabIndex = 0
        Me.btnDelete.Text = "&Delete"
        Me.ToolTip1.SetToolTip(Me.btnDelete, "Delete : Alt + D")
        Me.btnDelete.UseVisualStyleBackColor = True
        '
        'btnFreeze
        '
        Me.btnFreeze.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnFreeze.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnFreeze.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnFreeze.Location = New System.Drawing.Point(0, 5)
        Me.btnFreeze.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnFreeze.Name = "btnFreeze"
        Me.btnFreeze.Size = New System.Drawing.Size(90, 28)
        Me.btnFreeze.TabIndex = 0
        Me.btnFreeze.Text = "&Freeze"
        Me.btnFreeze.UseVisualStyleBackColor = True
        '
        'btnExcel
        '
        Me.btnExcel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnExcel.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnExcel.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnExcel.Image = Global.vLQMS.My.Resources.Resources.ucexport_to_XL
        Me.btnExcel.Location = New System.Drawing.Point(96, 5)
        Me.btnExcel.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnExcel.Name = "btnExcel"
        Me.btnExcel.Size = New System.Drawing.Size(38, 28)
        Me.btnExcel.TabIndex = 1
        Me.ToolTip1.SetToolTip(Me.btnExcel, "Export to Excel : Alt + E")
        Me.btnExcel.UseVisualStyleBackColor = True
        '
        'btnClose
        '
        Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClose.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_CLOSE
        Me.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnClose.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnClose.Location = New System.Drawing.Point(1002, 603)
        Me.btnClose.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(80, 28)
        Me.btnClose.TabIndex = 5
        Me.btnClose.Text = "Cl&ose"
        Me.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnClose, "Close:Alt+O")
        Me.btnClose.UseVisualStyleBackColor = True
        '
        'pnlExp
        '
        Me.pnlExp.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.pnlExp.Controls.Add(Me.txtNoofRec)
        Me.pnlExp.Controls.Add(Me.lblTotNoOfRec)
        Me.pnlExp.Controls.Add(Me.btnFreeze)
        Me.pnlExp.Controls.Add(Me.btnExcel)
        Me.pnlExp.Location = New System.Drawing.Point(8, 489)
        Me.pnlExp.Name = "pnlExp"
        Me.pnlExp.Size = New System.Drawing.Size(574, 36)
        Me.pnlExp.TabIndex = 301
        '
        'GrpUpdate
        '
        Me.GrpUpdate.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.GrpUpdate.Controls.Add(Me.RdoPartial)
        Me.GrpUpdate.Controls.Add(Me.RdoComplete)
        Me.GrpUpdate.Controls.Add(Me.txtPartial)
        Me.GrpUpdate.Controls.Add(Me.lblFields)
        Me.GrpUpdate.Controls.Add(Me.txtReplaceValue)
        Me.GrpUpdate.Controls.Add(Me.txtCurValue)
        Me.GrpUpdate.Controls.Add(Me.lblReplaceValue)
        Me.GrpUpdate.Controls.Add(Me.dtpDate)
        Me.GrpUpdate.Controls.Add(Me.txtFields)
        Me.GrpUpdate.Controls.Add(Me.btnUpdate)
        Me.GrpUpdate.Location = New System.Drawing.Point(8, 531)
        Me.GrpUpdate.Name = "GrpUpdate"
        Me.GrpUpdate.Size = New System.Drawing.Size(542, 111)
        Me.GrpUpdate.TabIndex = 3
        Me.GrpUpdate.TabStop = False
        Me.GrpUpdate.Text = "Updation"
        '
        'RdoPartial
        '
        Me.RdoPartial.AutoSize = True
        Me.RdoPartial.Location = New System.Drawing.Point(160, 20)
        Me.RdoPartial.Name = "RdoPartial"
        Me.RdoPartial.Size = New System.Drawing.Size(125, 17)
        Me.RdoPartial.TabIndex = 1
        Me.RdoPartial.Text = "Partial Field Updation"
        Me.RdoPartial.UseVisualStyleBackColor = True
        '
        'RdoComplete
        '
        Me.RdoComplete.AutoSize = True
        Me.RdoComplete.Checked = True
        Me.RdoComplete.Location = New System.Drawing.Point(7, 20)
        Me.RdoComplete.Name = "RdoComplete"
        Me.RdoComplete.Size = New System.Drawing.Size(140, 17)
        Me.RdoComplete.TabIndex = 0
        Me.RdoComplete.TabStop = True
        Me.RdoComplete.Text = "Complete Field Updation"
        Me.RdoComplete.UseVisualStyleBackColor = True
        '
        'txtPartial
        '
        Me.txtPartial.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.txtPartial.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPartial.Location = New System.Drawing.Point(329, 52)
        Me.txtPartial.Name = "txtPartial"
        Me.txtPartial.Size = New System.Drawing.Size(197, 21)
        Me.txtPartial.TabIndex = 2
        '
        'GrpDelete
        '
        Me.GrpDelete.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.GrpDelete.Controls.Add(Me.btnDelete)
        Me.GrpDelete.Location = New System.Drawing.Point(567, 584)
        Me.GrpDelete.Name = "GrpDelete"
        Me.GrpDelete.Size = New System.Drawing.Size(227, 53)
        Me.GrpDelete.TabIndex = 4
        Me.GrpDelete.TabStop = False
        Me.GrpDelete.Text = "Deletion"
        '
        'frmUtlBulkUpdate
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(188, Byte), Integer), CType(CType(205, Byte), Integer), CType(CType(227, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(1084, 644)
        Me.Controls.Add(Me.GrpDelete)
        Me.Controls.Add(Me.GrpUpdate)
        Me.Controls.Add(Me.pnlExp)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.vsfDet)
        Me.Controls.Add(Me.btnShowRec)
        Me.Controls.Add(Me.cboTable)
        Me.Controls.Add(Me.lblTableName)
        Me.Name = "frmUtlBulkUpdate"
        Me.ShowIcon = False
        Me.Text = "Bulk Update"
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlExp.ResumeLayout(False)
        Me.pnlExp.PerformLayout()
        Me.GrpUpdate.ResumeLayout(False)
        Me.GrpUpdate.PerformLayout()
        Me.GrpDelete.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents lblTableName As Label
    Friend WithEvents cboTable As ComboBox
    Friend WithEvents btnShowRec As Button
    Friend WithEvents vsfDet As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents lblFields As Label
    Friend WithEvents txtReplaceValue As TextBox
    Friend WithEvents lblReplaceValue As Label
    Friend WithEvents txtFields As TextBox
    Friend WithEvents btnUpdate As Button
    Friend WithEvents lblTotNoOfRec As Label
    Friend WithEvents txtNoofRec As TextBox
    Friend WithEvents txtCurValue As TextBox
    Friend WithEvents dtpDate As DateTimePicker
    Friend WithEvents btnDelete As Button
    Friend WithEvents btnFreeze As Button
    Friend WithEvents btnExcel As Button
    Friend WithEvents btnClose As Button
    Friend WithEvents pnlExp As Panel
    Friend WithEvents ToolTip1 As ToolTip
    Friend WithEvents GrpUpdate As GroupBox
    Friend WithEvents GrpDelete As GroupBox
    Friend WithEvents RdoPartial As RadioButton
    Friend WithEvents RdoComplete As RadioButton
    Friend WithEvents txtPartial As TextBox
End Class
