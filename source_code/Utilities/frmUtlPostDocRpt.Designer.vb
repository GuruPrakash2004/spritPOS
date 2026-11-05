<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmUtlPostDocRpt
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmUtlPostDocRpt))
        Me.pnlPending = New System.Windows.Forms.Panel()
        Me.rdoPending = New System.Windows.Forms.RadioButton()
        Me.rdoAll = New System.Windows.Forms.RadioButton()
        Me.pnlDtRng = New System.Windows.Forms.Panel()
        Me.lblTo = New System.Windows.Forms.Label()
        Me.lblFrom = New System.Windows.Forms.Label()
        Me.dtpToDt = New System.Windows.Forms.DateTimePicker()
        Me.dtpFromDt = New System.Windows.Forms.DateTimePicker()
        Me.chkDtRng = New System.Windows.Forms.CheckBox()
        Me.btnLoad = New System.Windows.Forms.Button()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.vsfDet = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.pnlPending.SuspendLayout()
        Me.pnlDtRng.SuspendLayout()
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'pnlPending
        '
        Me.pnlPending.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlPending.Controls.Add(Me.rdoPending)
        Me.pnlPending.Controls.Add(Me.rdoAll)
        Me.pnlPending.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pnlPending.Location = New System.Drawing.Point(12, 80)
        Me.pnlPending.Name = "pnlPending"
        Me.pnlPending.Size = New System.Drawing.Size(160, 31)
        Me.pnlPending.TabIndex = 1
        Me.pnlPending.Visible = False
        '
        'rdoPending
        '
        Me.rdoPending.AutoSize = True
        Me.rdoPending.Location = New System.Drawing.Point(68, 4)
        Me.rdoPending.Name = "rdoPending"
        Me.rdoPending.Size = New System.Drawing.Size(85, 24)
        Me.rdoPending.TabIndex = 1
        Me.rdoPending.TabStop = True
        Me.rdoPending.Text = "Pending"
        Me.rdoPending.UseVisualStyleBackColor = True
        '
        'rdoAll
        '
        Me.rdoAll.AutoSize = True
        Me.rdoAll.Location = New System.Drawing.Point(4, 4)
        Me.rdoAll.Name = "rdoAll"
        Me.rdoAll.Size = New System.Drawing.Size(44, 24)
        Me.rdoAll.TabIndex = 0
        Me.rdoAll.TabStop = True
        Me.rdoAll.Text = "All"
        Me.rdoAll.UseVisualStyleBackColor = True
        '
        'pnlDtRng
        '
        Me.pnlDtRng.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlDtRng.Controls.Add(Me.lblTo)
        Me.pnlDtRng.Controls.Add(Me.lblFrom)
        Me.pnlDtRng.Controls.Add(Me.dtpToDt)
        Me.pnlDtRng.Controls.Add(Me.dtpFromDt)
        Me.pnlDtRng.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pnlDtRng.Location = New System.Drawing.Point(12, 21)
        Me.pnlDtRng.Name = "pnlDtRng"
        Me.pnlDtRng.Size = New System.Drawing.Size(305, 53)
        Me.pnlDtRng.TabIndex = 2
        '
        'lblTo
        '
        Me.lblTo.AutoSize = True
        Me.lblTo.Location = New System.Drawing.Point(161, 19)
        Me.lblTo.Name = "lblTo"
        Me.lblTo.Size = New System.Drawing.Size(27, 20)
        Me.lblTo.TabIndex = 4
        Me.lblTo.Text = "To"
        '
        'lblFrom
        '
        Me.lblFrom.AutoSize = True
        Me.lblFrom.Location = New System.Drawing.Point(3, 20)
        Me.lblFrom.Name = "lblFrom"
        Me.lblFrom.Size = New System.Drawing.Size(46, 20)
        Me.lblFrom.TabIndex = 3
        Me.lblFrom.Text = "From"
        '
        'dtpToDt
        '
        Me.dtpToDt.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpToDt.Location = New System.Drawing.Point(194, 19)
        Me.dtpToDt.Name = "dtpToDt"
        Me.dtpToDt.Size = New System.Drawing.Size(106, 26)
        Me.dtpToDt.TabIndex = 2
        '
        'dtpFromDt
        '
        Me.dtpFromDt.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpFromDt.Location = New System.Drawing.Point(55, 20)
        Me.dtpFromDt.Name = "dtpFromDt"
        Me.dtpFromDt.Size = New System.Drawing.Size(100, 26)
        Me.dtpFromDt.TabIndex = 1
        '
        'chkDtRng
        '
        Me.chkDtRng.AutoSize = True
        Me.chkDtRng.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkDtRng.Location = New System.Drawing.Point(20, 11)
        Me.chkDtRng.Name = "chkDtRng"
        Me.chkDtRng.Size = New System.Drawing.Size(158, 24)
        Me.chkDtRng.TabIndex = 0
        Me.chkDtRng.Text = "Apply Date Range"
        Me.chkDtRng.UseVisualStyleBackColor = True
        '
        'btnLoad
        '
        Me.btnLoad.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnLoad.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnLoad.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnLoad.Location = New System.Drawing.Point(224, 80)
        Me.btnLoad.Name = "btnLoad"
        Me.btnLoad.Size = New System.Drawing.Size(93, 31)
        Me.btnLoad.TabIndex = 6
        Me.btnLoad.Text = "&Load"
        Me.ToolTip1.SetToolTip(Me.btnLoad, "Load : Alt + L")
        Me.btnLoad.UseVisualStyleBackColor = True
        '
        'btnClose
        '
        Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClose.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_CLOSE
        Me.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnClose.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnClose.Location = New System.Drawing.Point(582, 475)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(93, 31)
        Me.btnClose.TabIndex = 5
        Me.btnClose.Text = "Cl&ose"
        Me.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnClose, "Close : Alt + O")
        Me.btnClose.UseVisualStyleBackColor = True
        '
        'btnSave
        '
        Me.btnSave.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSave.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_SAVE
        Me.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSave.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSave.Location = New System.Drawing.Point(483, 475)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(93, 31)
        Me.btnSave.TabIndex = 4
        Me.btnSave.Text = "&Save"
        Me.btnSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnSave, "Save : Alt + S")
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'vsfDet
        '
        Me.vsfDet.AllowFiltering = True
        Me.vsfDet.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfDet.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle
        Me.vsfDet.ColumnInfo = "2,0,0,0,0,120,Columns:"
        Me.vsfDet.ExtendLastCol = True
        Me.vsfDet.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.vsfDet.KeyActionEnter = C1.Win.C1FlexGrid.KeyActionEnum.None
        Me.vsfDet.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross
        Me.vsfDet.Location = New System.Drawing.Point(12, 121)
        Me.vsfDet.Name = "vsfDet"
        Me.vsfDet.Rows.DefaultSize = 24
        Me.vsfDet.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfDet.Size = New System.Drawing.Size(663, 348)
        Me.vsfDet.StyleInfo = resources.GetString("vsfDet.StyleInfo")
        Me.vsfDet.TabIndex = 65
        '
        'frmUtlPostDocRpt
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(687, 510)
        Me.Controls.Add(Me.vsfDet)
        Me.Controls.Add(Me.chkDtRng)
        Me.Controls.Add(Me.btnLoad)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.btnSave)
        Me.Controls.Add(Me.pnlDtRng)
        Me.Controls.Add(Me.pnlPending)
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmUtlPostDocRpt"
        Me.ShowIcon = False
        Me.Text = "Post Doc Report"
        Me.pnlPending.ResumeLayout(False)
        Me.pnlPending.PerformLayout()
        Me.pnlDtRng.ResumeLayout(False)
        Me.pnlDtRng.PerformLayout()
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents pnlPending As Panel
    Friend WithEvents rdoPending As RadioButton
    Friend WithEvents rdoAll As RadioButton
    Friend WithEvents pnlDtRng As Panel
    Friend WithEvents lblTo As Label
    Friend WithEvents lblFrom As Label
    Friend WithEvents dtpToDt As DateTimePicker
    Friend WithEvents dtpFromDt As DateTimePicker
    Friend WithEvents chkDtRng As CheckBox
    Friend WithEvents btnSave As Button
    Friend WithEvents btnClose As Button
    Friend WithEvents btnLoad As Button
    Friend WithEvents vsfDet As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents ToolTip1 As ToolTip
End Class
