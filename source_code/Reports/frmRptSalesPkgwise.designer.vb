<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmRptSalesPkgwise
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
        Me.gbDateRange = New System.Windows.Forms.GroupBox()
        Me.lblTo = New System.Windows.Forms.Label()
        Me.lblFrom = New System.Windows.Forms.Label()
        Me.dtpTo = New System.Windows.Forms.DateTimePicker()
        Me.dtpFrom = New System.Windows.Forms.DateTimePicker()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.btnPrint = New System.Windows.Forms.Button()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.btnPkgItem = New System.Windows.Forms.Button()
        Me.chkItemGroup = New System.Windows.Forms.CheckBox()
        Me.txtDummy = New System.Windows.Forms.TextBox()
        Me.txtDummy2 = New System.Windows.Forms.TextBox()
        Me.gbDateRange.SuspendLayout()
        Me.SuspendLayout()
        '
        'gbDateRange
        '
        Me.gbDateRange.Controls.Add(Me.lblTo)
        Me.gbDateRange.Controls.Add(Me.lblFrom)
        Me.gbDateRange.Controls.Add(Me.dtpTo)
        Me.gbDateRange.Controls.Add(Me.dtpFrom)
        Me.gbDateRange.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!)
        Me.gbDateRange.Location = New System.Drawing.Point(3, 12)
        Me.gbDateRange.Name = "gbDateRange"
        Me.gbDateRange.Size = New System.Drawing.Size(343, 49)
        Me.gbDateRange.TabIndex = 259
        Me.gbDateRange.TabStop = False
        Me.gbDateRange.Text = "For The Period"
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
        'btnPrint
        '
        Me.btnPrint.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnPrint.BackColor = System.Drawing.Color.FromArgb(CType(CType(233, Byte), Integer), CType(CType(237, Byte), Integer), CType(CType(241, Byte), Integer))
        Me.btnPrint.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnPrint.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.btnPrint.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnPrint.Location = New System.Drawing.Point(196, 67)
        Me.btnPrint.Name = "btnPrint"
        Me.btnPrint.Size = New System.Drawing.Size(75, 29)
        Me.btnPrint.TabIndex = 280
        Me.btnPrint.Text = "&Print"
        Me.ToolTip1.SetToolTip(Me.btnPrint, "Print : Alt + P")
        Me.btnPrint.UseVisualStyleBackColor = False
        '
        'btnClose
        '
        Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClose.BackColor = System.Drawing.Color.FromArgb(CType(CType(233, Byte), Integer), CType(CType(237, Byte), Integer), CType(CType(241, Byte), Integer))
        Me.btnClose.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_CLOSE
        Me.btnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnClose.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!)
        Me.btnClose.Location = New System.Drawing.Point(277, 67)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(70, 29)
        Me.btnClose.TabIndex = 21
        Me.btnClose.Text = "Cl&ose"
        Me.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnClose, "Close : Alt + O")
        Me.btnClose.UseVisualStyleBackColor = False
        '
        'btnPkgItem
        '
        Me.btnPkgItem.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnPkgItem.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnPkgItem.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!)
        Me.btnPkgItem.Image = Global.vLQMS.My.Resources.Resources.Paste
        Me.btnPkgItem.Location = New System.Drawing.Point(392, 17)
        Me.btnPkgItem.Name = "btnPkgItem"
        Me.btnPkgItem.Size = New System.Drawing.Size(34, 33)
        Me.btnPkgItem.TabIndex = 279
        Me.ToolTip1.SetToolTip(Me.btnPkgItem, "Past package : Alt + P")
        Me.btnPkgItem.UseVisualStyleBackColor = True
        Me.btnPkgItem.Visible = False
        '
        'chkItemGroup
        '
        Me.chkItemGroup.AutoSize = True
        Me.chkItemGroup.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkItemGroup.Location = New System.Drawing.Point(17, 72)
        Me.chkItemGroup.Name = "chkItemGroup"
        Me.chkItemGroup.Size = New System.Drawing.Size(121, 20)
        Me.chkItemGroup.TabIndex = 264
        Me.chkItemGroup.Text = "Selected Group"
        Me.chkItemGroup.UseVisualStyleBackColor = True
        '
        'txtDummy
        '
        Me.txtDummy.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.txtDummy.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDummy.Location = New System.Drawing.Point(385, -295)
        Me.txtDummy.MaxLength = 50
        Me.txtDummy.Name = "txtDummy"
        Me.txtDummy.Size = New System.Drawing.Size(65, 23)
        Me.txtDummy.TabIndex = 270
        Me.txtDummy.Visible = False
        '
        'txtDummy2
        '
        Me.txtDummy2.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.txtDummy2.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDummy2.Location = New System.Drawing.Point(456, -295)
        Me.txtDummy2.MaxLength = 50
        Me.txtDummy2.Name = "txtDummy2"
        Me.txtDummy2.Size = New System.Drawing.Size(65, 23)
        Me.txtDummy2.TabIndex = 284
        Me.txtDummy2.Visible = False
        '
        'frmRptSalesPkgwise
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(233, Byte), Integer), CType(CType(237, Byte), Integer), CType(CType(241, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(357, 109)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.txtDummy2)
        Me.Controls.Add(Me.btnPrint)
        Me.Controls.Add(Me.btnPkgItem)
        Me.Controls.Add(Me.txtDummy)
        Me.Controls.Add(Me.chkItemGroup)
        Me.Controls.Add(Me.gbDateRange)
        Me.Name = "frmRptSalesPkgwise"
        Me.ShowIcon = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Sales Report Package Wise"
        Me.gbDateRange.ResumeLayout(False)
        Me.gbDateRange.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnClose As System.Windows.Forms.Button
    Friend WithEvents gbDateRange As GroupBox
    Friend WithEvents lblTo As Label
    Friend WithEvents lblFrom As Label
    Friend WithEvents dtpTo As DateTimePicker
    Friend WithEvents dtpFrom As DateTimePicker
    Friend WithEvents ToolTip1 As ToolTip
    Friend WithEvents chkItemGroup As CheckBox
    Friend WithEvents txtDummy As TextBox
    Friend WithEvents Panel1 As Panel
    Friend WithEvents optDocWiseBal As RadioButton
    Friend WithEvents optOPRcptIssBal As RadioButton
    Friend WithEvents optPkgWiseBal As RadioButton
    Friend WithEvents optCBal As RadioButton
    Friend WithEvents btnPrint As Button
    Friend WithEvents txtDummy2 As TextBox
    Friend WithEvents btnPkgItem As Button
End Class
